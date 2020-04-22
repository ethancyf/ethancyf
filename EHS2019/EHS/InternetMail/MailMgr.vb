Public Class MailMgr

    Public Const cDayMaxCount As String = "EmailDayMaxCount"
    Public Const cHourMaxCount As String = "EmailHourMaxCount"
    Public Const cEachProcessCount As String = "EmailEachProcessCount"
    Public Const cEmailPauseRate As String = "EmailPauseRate"

    Private Shared _mailMgr As MailMgr = Nothing

    Private intDayCounterParam As Integer = -1
    Private intHourCounterParam As Integer = -1

    Private intDaySentCounter As Integer = -1
    Private intHourSentCounter As Integer = -1
    Private intPauseRateMilliSecond As Integer = 3000

    Private intEmailEachProcessCount As Integer = 0

    Private m_udtCommonGeneralFunction As New Common.ComFunction.GeneralFunction

    Private m_udtInternetMailBLL As New Common.Component.InternetMail.InternetMailBLL()
    Private m_udtInternetMailCollection As Common.Component.InternetMail.InternetMailModelCollection
    Private m_udtMailTemplateCollection As New Common.Component.InternetMail.MailTemplateModelCollection()


#Region "Constructor"

    Private Sub New()

    End Sub

    Public Shared Function GetInstance() As MailMgr
        If _mailMgr Is Nothing Then _mailMgr = New MailMgr()
        Return _mailMgr
    End Function

#End Region

    Protected Sub LoadParameters()

        Dim strValue As String = Nothing

        Me.m_udtCommonGeneralFunction.getSystemParameter(MailMgr.cDayMaxCount, strValue, String.Empty)
        Me.intDayCounterParam = CInt(strValue)

        Me.m_udtCommonGeneralFunction.getSystemParameter(MailMgr.cHourMaxCount, strValue, String.Empty)
        Me.intHourCounterParam = CInt(strValue)

        Me.m_udtCommonGeneralFunction.getSystemParameter(MailMgr.cEachProcessCount, strValue, String.Empty)
        Me.intEmailEachProcessCount = CInt(strValue)

        Me.m_udtCommonGeneralFunction.getSystemParameter(MailMgr.cEmailPauseRate, strValue, String.Empty)
        Me.intPauseRateMilliSecond = CInt(strValue)


    End Sub

    Protected Sub LoadSentMailCounter()
        Me.intDaySentCounter = Me.m_udtInternetMailBLL.GetMailSentCountByDay()
        Me.intHourSentCounter = Me.m_udtInternetMailBLL.GetMailSentCountByHour()
    End Sub

    Protected Function GetRetrieveEmailCount() As Integer
        Dim intDayLimit As Integer = Me.intDayCounterParam - Me.intDaySentCounter
        Dim intHourLimit As Integer = Me.intHourCounterParam - Me.intHourSentCounter

        If intDayLimit <= intHourLimit AndAlso intDayLimit <= Me.intEmailEachProcessCount Then
            Return intDayLimit
        ElseIf intHourLimit <= intDayLimit AndAlso intHourLimit <= Me.intEmailEachProcessCount Then
            Return intHourLimit
        Else
            Return Me.intEmailEachProcessCount
        End If

    End Function

    Protected Function IsExceedSentLimit() As Boolean
        If Me.intDaySentCounter >= Me.intDayCounterParam OrElse Me.intHourSentCounter >= Me.intHourCounterParam Then
            Return True
        End If
        Return False
    End Function

    Protected Function RetrieveMailToBeSent() As Common.Component.InternetMail.InternetMailModelCollection
        Return Me.m_udtInternetMailBLL.GetMailToBeSent(Me.GetRetrieveEmailCount())
    End Function

    Protected Function RetrieveMailTemplate(ByVal strMailID As String, ByVal strVersion As String) As Common.Component.InternetMail.MailTemplateModel
        Return Me.m_udtInternetMailBLL.GetMailTemplate(strMailID, strVersion)
    End Function

    Public Sub ProcessEMails()
        'Try
        '    Dim udtCommonGeneralFunction As New Common.ComFunction.GeneralFunction()
        '    Dim strValue As String = ""
        '    udtCommonGeneralFunction.getSystemParameter(Common.Component.ScheduleJobSetting.ActiveServer, strValue, String.Empty)

        '    If MailUtil.GetHostName().Trim().ToUpper <> strValue.ToUpper.Trim() Then
        '        MailLogger.LogLine(strValue + ":" + MailUtil.GetHostName())
        '        Return
        '    End If
        'Catch ex As Exception
        '    MailLogger.LogLine(ex.ToString())
        '    Return
        'End Try

        Try
            Dim strActiveServer As String = System.Configuration.ConfigurationManager.AppSettings(Common.Component.ScheduleJobSetting.ActiveServer).ToString()
            If MailUtil.GetHostName().Trim().ToUpper <> strActiveServer Then
                MailLogger.LogLine(strActiveServer + "<>" + MailUtil.GetHostName())
                Return
            End If
        Catch ex As Exception
            MailLogger.LogLine(ex.ToString())
            MailLogger.ErrorLog(ex)
            Return
        End Try

        Try
            MailLogger.Log("Start", Common.Component.ScheduleJobLogStatus.Information, Nothing, "Program Start")

        Catch sql As SqlClient.SqlException
            Try
                MailLogger.LogLine(sql.ToString())
                MailLogger.ErrorLog(sql)
            Catch ex As Exception
                Return
            End Try
        Catch ex As Exception
            MailLogger.LogLine(ex.ToString())
            MailLogger.ErrorLog(ex)
        End Try

        Try

            WSSetting.InitParameters()

            Me.LoadParameters()
            Me.LoadSentMailCounter()

            Dim strDescription As String = "<Day Limit:" + Me.intDayCounterParam.ToString() + "><Hour Limit:" + Me.intHourCounterParam.ToString() + "><Each Process Limit:" + Me.intEmailEachProcessCount.ToString() + "><Day Sent:" + Me.intDaySentCounter.ToString() + "><HourSent:" + Me.intHourSentCounter.ToString() + ">"

            MailLogger.LogLine(strDescription)

            MailLogger.Log("Parameters", Common.Component.ScheduleJobLogStatus.Information, Nothing, strDescription)

            If IsExceedSentLimit() Then
                MailLogger.LogLine("Exceed Limit")
                Return
            End If

            ' Retrieve Task To Be Sent : Linear Sent Mail Or Group By Mail Type Sent ?
            Me.m_udtInternetMailCollection = Me.RetrieveMailToBeSent()

            MailLogger.LogLine("Mail To Send:" + Me.m_udtInternetMailCollection.Count.ToString())
            MailLogger.Log("LoadMail", Common.Component.ScheduleJobLogStatus.Information, Nothing, "<NumofMail:" + Me.m_udtInternetMailCollection.Count.ToString() + ">")

            Dim intCounter As Integer = 1
            ' For Each Mail
            For Each udtInternetMail As Common.Component.InternetMail.InternetMailModel In Me.m_udtInternetMailCollection.Values
                ' Search For The Mail Template

                If Not Me.m_udtMailTemplateCollection.IndexOfKey(udtInternetMail.MailID + udtInternetMail.Version) >= 0 Then
                    Me.m_udtMailTemplateCollection.Add(Me.RetrieveMailTemplate(udtInternetMail.MailID, udtInternetMail.Version))
                End If

                ' Process Each Mail
                Me.ProcessEmail(intCounter, udtInternetMail, Me.m_udtMailTemplateCollection.GetByIndex(Me.m_udtMailTemplateCollection.IndexOfKey(udtInternetMail.MailID + udtInternetMail.Version)))
                intCounter = intCounter + 1

                Threading.Thread.Sleep(intPauseRateMilliSecond)

            Next

        Catch ex As Exception
            MailLogger.LogLine(ex.ToString())
            MailLogger.ErrorLog(ex)
        End Try

        Try
            MailLogger.Log("End", Common.Component.ScheduleJobLogStatus.Information, Nothing, "Program End")
        Catch ex As Exception
            MailLogger.LogLine(ex.ToString())
            MailLogger.ErrorLog(ex)
        End Try

    End Sub

    Protected Sub ProcessEmail(ByVal intCounter As Integer, ByVal udtInternetMail As Common.Component.InternetMail.InternetMailModel, ByVal udtMailTemplate As Common.Component.InternetMail.MailTemplateModel)

        Try
            Dim email As MailBuilder.Email = MailBuilder.GetInstance().ConstructEmail(udtInternetMail, udtMailTemplate)

            'MailLogger.LogLine(email.ToString())
            MailLogger.LogLine(udtInternetMail.SystemDtm.ToString() + "," + email.EmailAddress + "," + email.Subject)

            Dim blnSuccess As Boolean = SendMailUtil.GetInstance().SendMail(email)

            If blnSuccess Then
                ' Update Email Status
                Me.m_udtInternetMailBLL.UpdateInternetMailSent(udtInternetMail)
            Else
                ' Log Fail
                MailLogger.Log("SendMail", Common.Component.ScheduleJobLogStatus.Fail, Nothing, "<SystemDtm:" + udtInternetMail.SystemDtm.ToString + "><Mail_ID:" + udtInternetMail.MailID.Trim() + "><Version:" + udtInternetMail.Version.Trim() + ">")
            End If

        Catch ex As Exception
            MailLogger.LogLine(ex.ToString())
            MailLogger.ErrorLog(ex)
        End Try
    End Sub
End Class
