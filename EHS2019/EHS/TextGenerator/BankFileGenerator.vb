Imports System.Data.SqlClient
Imports Common.Component
Imports Common.Component.InternetMail
Imports Common.ComFunction.ParameterFunction
Imports Common.Component.Inbox

Public Class BankFileGenerator
    Inherits BaseGenerator

    Implements ITextFileGenerable

    Sub New(ByVal udtQueue As Common.Component.FileGeneration.FileGenerationQueueModel, ByVal udtFileGenerationModel As Common.Component.FileGeneration.FileGenerationModel)

        MyBase.New(udtQueue, udtFileGenerationModel)

        ' Get Out Put Path
        Dim strPath As String = String.Empty
        Dim udtCommonGenFunction As New Common.ComFunction.GeneralFunction()
        udtCommonGenFunction.getSystemParameter("BankFileStoragePath", strPath, String.Empty)
        Me.m_strFileOutPath = strPath

    End Sub

    Public Overrides Function GetData() As String()
        Dim dsData As New DataSet()

        Dim udtDB As New Common.DataAccess.Database()
        Dim udtParamFunction As New Common.ComFunction.ParameterFunction()
        Dim udtSPParamCollection As StoreProcParamCollection = udtParamFunction.GetSPParamCollection(Me.m_udtQueue.InParm)

        Dim params(udtSPParamCollection.Count) As SqlParameter

        For i As Integer = 0 To udtSPParamCollection.Count - 1
            Dim udtSPParamObject As StoreProcParamObject = udtSPParamCollection(i)
            params(i) = udtDB.MakeInParam(udtSPParamObject.ParamName, udtSPParamObject.ParamDBType, udtSPParamObject.ParamDBSize, udtSPParamObject.ParamValue)
        Next

        udtDB.RunProc(Me.m_udtFileGeneration.FileDataSP, params, dsData)

        ' Two DataTable is Return: 1 Row Header Record + (n) Content Records
        'Dim arrStrData() As String = Nothing

        Dim arrstrData(0) As String
        If (dsData.Tables.Count > 0) Then

            Dim strValue As String = ""

            For Each dtTable As DataTable In dsData.Tables
                For Each drRow As DataRow In dtTable.Rows
                    strValue = strValue & Me.ConvertHeaderRow(dtTable.Columns.Count, drRow)
                Next
            Next
            arrstrData(0) = strValue

            'Dim intTotalRowCount As Integer = 0
            'For Each dtTable As DataTable In dsData.Tables
            '    intTotalRowCount = intTotalRowCount + dtTable.Rows.Count
            'Next

            'ReDim arrStrData(intTotalRowCount - 1)

            'Dim intRowNum As Integer = 0
            'For Each dtTable As DataTable In dsData.Tables
            '    For Each drRow As DataRow In dtTable.Rows
            '        arrStrData(intRowNum) = Me.ConvertHeaderRow(dtTable.Columns.Count, drRow)
            '        intRowNum = intRowNum + 1
            '    Next
            'Next
        End If

        Return arrStrData
    End Function

    Private Function ConvertHeaderRow(ByVal intColumnCount As Integer, ByVal drRow As DataRow) As String

        Dim strReturn As String = ""

        For i As Integer = 0 To intColumnCount - 1
            strReturn = strReturn + drRow(i).ToString()
        Next

        Return strReturn
    End Function

    Public Overrides Sub ConstructMessageParamaterList(ByRef udtDB As Common.DataAccess.Database, ByRef udtMessageCollection As MessageModelCollection, ByRef udtMessageReaderCollection As MessageReaderModelCollection)
        Dim udtFileGenerationBLL As New Common.Component.FileGeneration.FileGenerationBLL()
        Dim udtGeneral As New Common.ComFunction.GeneralFunction()
        Dim udtParamFunction As New Common.ComFunction.ParameterFunction()
        Dim udtFormatter As New Common.Format.Formatter

        Dim dtmCurrent As DateTime = udtGeneral.GetSystemDateTime()

        'udtMessageCollection = New MessageModelCollection()
        'udtMessageReaderCollection = New MessageReaderModelCollection()

        Dim dtResult As DataTable = udtFileGenerationBLL.GetViewAccessibleUser(udtDb, Me.m_udtQueue.GenerationID)

        ' Construct Message & MessageReader
        ' One Message, To Mutilple User (Other Case May Mutil Message, Each Message to Single User
        Dim udtMessage As New MessageModel()
        udtMessage.MessageID = udtGeneral.generateInboxMsgID()

        Dim paramsSubject As New ParameterCollection()
        paramsSubject.AddParam("FileName", Me.GetFileFullName())

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'paramsSubject.AddParam("Date", Now.ToString("yyyyMMMdd"))
        'paramsSubject.AddParam("Date", udtFormatter.formatDate(Now, "en"))
        paramsSubject.AddParam("Date", udtFormatter.formatDisplayDate(Now, CultureLanguage.English))
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        Dim paramsContent As New ParameterCollection()
        paramsContent.AddParam("FileName", Me.GetFileFullName())
        paramsContent.AddParam("Link", "../ReportAndDownload/Datadownload.aspx")

        udtMessage.Subject = udtParamFunction.GetParsedStringByparameter(Me.m_udtFileGeneration.MessageSubject, paramsSubject)
        udtMessage.Message = udtParamFunction.GetParsedStringByparameter(Me.m_udtFileGeneration.MessageContent, paramsContent)

        udtMessage.CreateBy = "EHCVS"
        udtMessage.CreateDtm = dtmCurrent
        udtMessageCollection.Add(udtMessage)

        For Each drRow As DataRow In dtResult.Rows

            Dim strUserId As String = drRow("User_ID").ToString().Trim()
            Dim strUserName As String = drRow("User_Name").ToString().Trim()

            Dim udtMessageReader As New MessageReaderModel()
            udtMessageReader.MessageID = udtMessage.MessageID
            udtMessageReader.MessageReader = strUserId
            udtMessageReader.UpdateBy = "EHCVS"
            udtMessageReader.UpdateDtm = dtmCurrent

            udtMessageReaderCollection.Add(udtMessageReader)
        Next

        ' Reimbursement Notification To Service Provider (Group By Service Provider)
        ' Retrieve Inbox Message Template
        Dim udtInternetMailBLL As New InternetMailBLL()
        Dim udtMailTemplate As MailTemplateModel = udtInternetMailBLL.GetMailTemplate(udtDB, Common.Component.InboxMsgTemplateID.ReimbursementNotification)

        ' Retrieve 
        Dim dtSP As New DataTable()
        Dim params() As SqlParameter = {udtDB.MakeInParam("@reimburse_id", SqlDbType.Char, 15, Left(Me.m_udtQueue.OutputFile, Me.m_udtQueue.OutputFile.LastIndexOf("-")))}
        udtDB.RunProc("proc_Reimbursement_get_SPID", params, dtSP)

        'Reimburse_ID, CutOff_Date, SP_ID, Default_Language

        ' For Each SP
        For Each drRow As DataRow In dtSP.Rows

            ' Retrieve SP Information
            Dim strSPID As String = drRow("SP_ID").ToString().Trim()
            Dim strLang As String = Common.Component.InternetMailLanguage.EngHeader
            Dim dtmCutOff As DateTime = Convert.ToDateTime(drRow("CutOff_Date"))
            If Not drRow.IsNull("Default_Language") Then strLang = drRow("Default_Language").ToString().Trim()

            Dim strSubject As String = ""
            If strLang = Common.Component.InternetMailLanguage.EngHeader Then
                strSubject = udtMailTemplate.MailSubjectEng
            Else
                strSubject = udtMailTemplate.MailSubjectChi
            End If

            Dim strNumofDay As String = String.Empty
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction()
            udtGeneralFunction.getSystemParameter("ReimbNotifyDayNum", strNumofDay, String.Empty)

            Dim paramsChiContent As New ParameterCollection()
            paramsChiContent.AddParam("Day", dtmCutOff.ToString("dd"))
            paramsChiContent.AddParam("Month", dtmCutOff.ToString("MM"))
            paramsChiContent.AddParam("Year", dtmCutOff.Year.ToString())
            paramsChiContent.AddParam("NumOfDay", strNumofDay)

            Dim paramsEngContent As New ParameterCollection()
            paramsEngContent.AddParam("cutOffDate", dtmCutOff.ToString(udtFormatter.DisplayDateFormat))
            'paramsEngContent.AddParam("Month", dtmCutOff.ToString("MMM"))
            'paramsEngContent.AddParam("Year", dtmCutOff.Year.ToString())
            paramsEngContent.AddParam("NumOfDay", strNumofDay)

            Dim strChiContent As String = udtMailTemplate.MailBodyChi
            Dim strEngContent As String = udtMailTemplate.MailBodyEng

            'Check if the SP_ID is already existed in the collection, then no need to add
            Dim bSPExist As Boolean = False
            If Not IsNothing(udtMessageReaderCollection) Then
                For Each udtMsgReaderModel As MessageReaderModel In udtMessageReaderCollection.Values
                    If udtMsgReaderModel.MessageReader.Trim.Equals(strSPID.Trim) Then bSPExist = True
                Next

                'If not exist, then create a new message and add to message reader collection
                If Not bSPExist Then
                    udtMessage = New MessageModel()
                    udtMessage.MessageID = udtGeneral.generateInboxMsgID()

                    '[%Month%] [%Year%] 
                    ' [%NumOfDay%] 
                    udtMessage.Subject = strSubject
                    udtMessage.Message = udtParamFunction.GetParsedStringByparameter(strChiContent, paramsChiContent) + " " + udtParamFunction.GetParsedStringByparameter(strEngContent, paramsEngContent)

                    udtMessage.CreateBy = "EHCVS"
                    udtMessage.CreateDtm = dtmCurrent
                    udtMessageCollection.Add(udtMessage)

                    Dim udtMessageReader As New MessageReaderModel()
                    udtMessageReader.MessageID = udtMessage.MessageID
                    udtMessageReader.MessageReader = strSPID
                    udtMessageReader.UpdateBy = "EHCVS"
                    udtMessageReader.UpdateDtm = dtmCurrent

                    udtMessageReaderCollection.Add(udtMessageReader)
                End If
            End If
        Next
    End Sub
End Class
