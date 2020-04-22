Imports Common.Component.Inbox
Imports Common.Component.InternetMail
Imports Common.ComFunction.ParameterFunction

Public Class ProgramMgr

    Private Shared _programMgr As ProgramMgr

    Private m_udtCommonGeneralFunction As New Common.ComFunction.GeneralFunction()

    Dim arrSP3List As New List(Of String)
    Dim arrSP1List As New List(Of String)

#Region "Constructor"

    Private Sub New()

    End Sub

    Public Shared Function GetInstance() As ProgramMgr
        If _programMgr Is Nothing Then _programMgr = New ProgramMgr()
        Return _programMgr
    End Function

#End Region

    Public Sub StartProcess()

        Try
            Dim strActiveServer As String = System.Configuration.ConfigurationManager.AppSettings(Common.Component.ScheduleJobSetting.ActiveServer).ToString()
            If ProgramUtil.GetHostName().Trim().ToUpper <> strActiveServer Then
                ProgramLogger.LogLine(strActiveServer + "<>" + ProgramUtil.GetHostName())
                Return
            End If
        Catch ex As Exception
            ProgramLogger.LogLine(ex.ToString())
            ProgramLogger.ErrorLog(ex)
            Return
        End Try

        Try
            ProgramLogger.Log("Start", Common.Component.ScheduleJobLogStatus.Information, Nothing, "Program Start")

        Catch sql As SqlClient.SqlException
            Try
                ProgramLogger.LogLine(sql.ToString())
                ProgramLogger.ErrorLog(sql)
            Catch ex As Exception
                Return
            End Try
        Catch ex As Exception
            ProgramLogger.LogLine(ex.ToString())
            ProgramLogger.ErrorLog(ex)
        End Try

        Try
            Me.LevelThreeAlert()

            'Must Execute LevelThreeAlert checking before the LevelOneAlert
            Me.LevelOneAlert()

            Me.LevelFiveAlert()

            Me.ManualValidateECTempAccount()

        Catch ex As Exception
            ProgramLogger.LogLine(ex.ToString())
            ProgramLogger.ErrorLog(ex)
        End Try

        Try
            ProgramLogger.Log("End", Common.Component.ScheduleJobLogStatus.Information, Nothing, "Program End")
        Catch ex As Exception
            ProgramLogger.LogLine(ex.ToString())
            ProgramLogger.ErrorLog(ex)
        End Try

    End Sub

    Private Sub LevelOneAlert()
        Dim TempVRAcctBLL As New TempVRAcctBLL
        Dim dt As New DataTable
        Dim i As Integer

        Dim udtDB As New Common.DataAccess.Database

        Try
            udtDB.BeginTransaction()

            dt = TempVRAcctBLL.GetSPListFor4LevelAlert(udtDB, 1)

            For i = 0 To dt.Rows.Count - 1
                If Not arrSP3List.Contains(dt.Rows(i)("SP_ID").ToString.Trim) And Not arrSP1List.Contains(dt.Rows(i)("SP_ID").ToString.Trim) Then
                    arrSP1List.Add(dt.Rows(i)("SP_ID").ToString.Trim)
                End If
            Next

            ProgramLogger.LogLine("LevelOneAlert: SP List Count=" + arrSP1List.Count.ToString())

            If arrSP1List.Count > 0 Then
                'Call the send inbox to sp
                Dim udtInboxBLL As New Common.Component.Inbox.InboxBLL()
                Dim udtMessageCollection As Common.Component.Inbox.MessageModelCollection = Nothing
                Dim udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection = Nothing

                Me.ConstructHCSP4LevelAlertMessages(udtDB, arrSP1List, udtMessageCollection, udtMessageReaderCollection, Common.Component.InboxMsgTemplateID.HCSPRectifyNotification)
                udtInboxBLL.AddMessageAndMessageReaderList(udtDB, udtMessageCollection, udtMessageReaderCollection)
            End If

            udtDB.CommitTransaction()
        Catch ex As Exception
            udtDB.RollBackTranscation()
            ProgramLogger.LogLine(ex.ToString())
            ProgramLogger.ErrorLog(ex)
        End Try
    End Sub

    Private Sub LevelThreeAlert()
        Dim TempVRAcctBLL As New TempVRAcctBLL
        Dim dt As New DataTable
        Dim i As Integer

        Dim udtDB As New Common.DataAccess.Database

        Try
            udtDB.BeginTransaction()

            dt = TempVRAcctBLL.GetSPListFor4LevelAlert(udtDB, 3)

            For i = 0 To dt.Rows.Count - 1
                If Not arrSP3List.Contains(dt.Rows(i)("SP_ID").ToString.Trim) Then
                    arrSP3List.Add(dt.Rows(i)("SP_ID").ToString.Trim)
                End If
            Next

            ProgramLogger.LogLine("LevelThreeAlert: SP List Count=" + arrSP3List.Count.ToString())

            If arrSP3List.Count > 0 Then
                'Call the send inbox to sp
                Dim udtInboxBLL As New Common.Component.Inbox.InboxBLL()
                Dim udtMessageCollection As Common.Component.Inbox.MessageModelCollection = Nothing
                Dim udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection = Nothing

                Me.ConstructHCSP4LevelAlertMessages(udtDB, arrSP3List, udtMessageCollection, udtMessageReaderCollection, Common.Component.InboxMsgTemplateID.Level3NotificationIn4LevelAlert)
                udtInboxBLL.AddMessageAndMessageReaderList(udtDB, udtMessageCollection, udtMessageReaderCollection)
            End If

            udtDB.CommitTransaction()
        Catch ex As Exception
            udtDB.RollBackTranscation()
            ProgramLogger.LogLine(ex.ToString())
            ProgramLogger.ErrorLog(ex)
        End Try


    End Sub

    Private Sub LevelFiveAlert()
        Try
            Dim TempVRAcctBLL As New TempVRAcctBLL
            Dim dt As New DataTable
            Dim udtDB As New Common.DataAccess.Database
            Dim arrHCVUUserList As New List(Of String)

            dt = TempVRAcctBLL.GetSPListFor4LevelAlert(udtDB, 5)

            ProgramLogger.LogLine("LevelFiveAlert: Deleted List Count=" + dt.Rows.Count.ToString())

            If dt.Rows.Count > 0 Then
                'Get the list of HCVU User that will receive the inbox message 

                Dim strFunctionCode = String.Empty
                Dim udtCommonGenFunction As New Common.ComFunction.GeneralFunction()
                udtCommonGenFunction.getSystemParameter("RectDelFunctionCode", strFunctionCode, String.Empty)

                arrHCVUUserList = TempVRAcctBLL.GetHCVUUserList(udtDB, strFunctionCode) '- Common.Component.FunctCode.FUNT010301)

                'Send Inbox Message to HCVU
                Dim udtInboxBLL As New Common.Component.Inbox.InboxBLL()
                Dim udtMessageCollection As Common.Component.Inbox.MessageModelCollection = Nothing
                Dim udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection = Nothing

                Me.ConstructHCVU4LevelAlertMessages(udtDB, arrHCVUUserList, udtMessageCollection, udtMessageReaderCollection)
                udtInboxBLL.AddMessageAndMessageReaderList(udtDB, udtMessageCollection, udtMessageReaderCollection)
            End If
        Catch ex As Exception
            ProgramLogger.LogLine(ex.ToString())
            ProgramLogger.ErrorLog(ex)
        End Try
    End Sub

    Private Sub ConstructHCSP4LevelAlertMessages(ByRef udtDB As Common.DataAccess.Database, ByRef arrStrSPID As List(Of String), ByRef udtMessageCollection As Common.Component.Inbox.MessageModelCollection, ByRef udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection, ByVal strMsgTemplateID As String)

        Dim udtTempVRAcctBLL As New TempVRAcctBLL

        udtMessageCollection = New MessageModelCollection()
        udtMessageReaderCollection = New MessageReaderModelCollection()

        ' Retrieve Message Template
        Dim udtInternetMailBLL As New InternetMailBLL()
        Dim udtMailTemplate As MailTemplateModel = udtInternetMailBLL.GetMailTemplate(udtDB, strMsgTemplateID)
        Dim dtmCurrent As DateTime = Me.m_udtCommonGeneralFunction.GetSystemDateTime()

        For Each strSPID As String In arrStrSPID

            ' Retrieve SP Defaul Language
            Dim dtSP As DataTable = udtTempVRAcctBLL.RetrieveSPDefaultLanguage(udtDB, strSPID)

            Dim strLang As String = Common.Component.InternetMailLanguage.EngHeader
            If Not dtSP.Rows(0).IsNull("Default_Language") Then strLang = dtSP.Rows(0)("Default_Language").ToString().Trim()


            Dim strSubject As String = ""
            If strLang = Common.Component.InternetMailLanguage.EngHeader Then
                strSubject = udtMailTemplate.MailSubjectEng
            Else
                strSubject = udtMailTemplate.MailSubjectChi
            End If

            Dim strChiContent As String = udtMailTemplate.MailBodyChi
            Dim strEngContent As String = udtMailTemplate.MailBodyEng
            Dim udtMessage As New MessageModel()
            udtMessage.MessageID = Me.m_udtCommonGeneralFunction.generateInboxMsgID()


            udtMessage.Subject = strSubject
            udtMessage.Message = strChiContent + " " + strEngContent

            udtMessage.CreateBy = "EHCVS"
            udtMessage.CreateDtm = dtmCurrent
            udtMessageCollection.Add(udtMessage)

            Dim udtMessageReader As New MessageReaderModel()
            udtMessageReader.MessageID = udtMessage.MessageID
            udtMessageReader.MessageReader = strSPID
            udtMessageReader.UpdateBy = "EHCVS"
            udtMessageReader.UpdateDtm = dtmCurrent

            udtMessageReaderCollection.Add(udtMessageReader)
        Next

    End Sub

    Private Sub ConstructHCVU4LevelAlertMessages(ByRef udtDB As Common.DataAccess.Database, ByRef arrStrHCVU As List(Of String), ByRef udtMessageCollection As Common.Component.Inbox.MessageModelCollection, ByRef udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection)

        Dim udtTempVRAcctBLL As New TempVRAcctBLL

        udtMessageCollection = New MessageModelCollection()
        udtMessageReaderCollection = New MessageReaderModelCollection()

        ' Retrieve Message Template
        Dim udtInternetMailBLL As New InternetMailBLL()
        Dim udtMailTemplate As MailTemplateModel = udtInternetMailBLL.GetMailTemplate(udtDB, Common.Component.InboxMsgTemplateID.HCVUReadyToDeleteVRAcctAlert)
        Dim dtmCurrent As DateTime = Me.m_udtCommonGeneralFunction.GetSystemDateTime()

        For Each strHCVU As String In arrStrHCVU

            Dim strSubject As String = ""
            strSubject = udtMailTemplate.MailSubjectEng

            Dim strEngContent As String = udtMailTemplate.MailBodyEng
            Dim udtMessage As New MessageModel()
            udtMessage.MessageID = Me.m_udtCommonGeneralFunction.generateInboxMsgID()

            udtMessage.Subject = strSubject
            udtMessage.Message = strEngContent

            udtMessage.CreateBy = "EHCVS"
            udtMessage.CreateDtm = dtmCurrent
            udtMessageCollection.Add(udtMessage)

            Dim udtMessageReader As New MessageReaderModel()
            udtMessageReader.MessageID = udtMessage.MessageID
            udtMessageReader.MessageReader = strHCVU
            udtMessageReader.UpdateBy = "EHCVS"
            udtMessageReader.UpdateDtm = dtmCurrent

            udtMessageReaderCollection.Add(udtMessageReader)
        Next

    End Sub

    Private Sub ManualValidateECTempAccount()

        Dim TempVRAcctBLL As New TempVRAcctBLL()
        Dim udtDB As New Common.DataAccess.Database()

        Try
            udtDB.BeginTransaction()

            Dim intRowCount As Integer = TempVRAcctBLL.GetECNewlyCreateTempVoucherAccountCount(udtDB)

            If intRowCount > 0 Then

                Dim arrStrUserID As List(Of String) = TempVRAcctBLL.GetHCVUUserList(udtDB, "010301", "010302")
                Dim udtMessageCollection As Common.Component.Inbox.MessageModelCollection = Nothing
                Dim udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection = Nothing
                Me.ConstructHCVUManualValidateECMessage(udtDB, udtMessageCollection, udtMessageReaderCollection, arrStrUserID)

                Dim udtInboxBLL As New Common.Component.Inbox.InboxBLL()
                udtInboxBLL.AddMessageAndMessageReaderList(udtDB, udtMessageCollection, udtMessageReaderCollection)
            End If
            udtDB.CommitTransaction()

            ProgramLogger.LogLine("Manual Validate EC Account Success: Row=" + intRowCount.ToString())

        Catch ex As Exception
            udtDB.RollBackTranscation()
            ProgramLogger.LogLine(ex.ToString())
            ProgramLogger.ErrorLog(ex)
        End Try
    End Sub

    Private Sub ConstructHCVUManualValidateECMessage(ByRef udtDB As Common.DataAccess.Database, ByRef udtMessageCollection As Common.Component.Inbox.MessageModelCollection, ByRef udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection, ByVal arrStrUserList As List(Of String))
        udtMessageCollection = New MessageModelCollection()
        udtMessageReaderCollection = New MessageReaderModelCollection()

        ' Retrieve Message Template
        Dim udtInternetMailBLL As New InternetMailBLL()
        Dim udtMailTemplate As MailTemplateModel = udtInternetMailBLL.GetMailTemplate(udtDB, Common.Component.InboxMsgTemplateID.HCVUVManualValidateECTempVRAcct)
        Dim dtmCurrent As DateTime = Me.m_udtCommonGeneralFunction.GetSystemDateTime()

        Dim strSubject As String = ""
        strSubject = udtMailTemplate.MailSubjectEng

        Dim strEngContent As String = udtMailTemplate.MailBodyEng
        Dim udtMessage As New MessageModel()
        udtMessage.MessageID = Me.m_udtCommonGeneralFunction.generateInboxMsgID()

        udtMessage.Subject = strSubject
        udtMessage.Message = strEngContent

        udtMessage.CreateBy = "EHCVS"
        udtMessage.CreateDtm = dtmCurrent
        udtMessageCollection.Add(udtMessage)

        For Each strUserId As String In arrStrUserList

            Dim udtMessageReader As New MessageReaderModel()
            udtMessageReader.MessageID = udtMessage.MessageID
            udtMessageReader.MessageReader = strUserId
            udtMessageReader.UpdateBy = "EHCVS"
            udtMessageReader.UpdateDtm = dtmCurrent

            udtMessageReaderCollection.Add(udtMessageReader)
        Next

    End Sub
End Class
