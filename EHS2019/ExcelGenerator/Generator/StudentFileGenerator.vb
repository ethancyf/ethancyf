Imports System.Data.SqlClient
Imports Common.Component
Imports Common.ComFunction.ParameterFunction
Imports Common.Component.Inbox
Imports Common.Component.InternetMail

Public Class StudentFileGenerator
    Inherits Generator.StudentFileGeneratorBase
    Implements IExcelGenerable

    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
    'Public Shared Function StudentFileGenerateCheck(ByVal strInputFileName As String) As Boolean
    '    Dim blnValid As Boolean = False
    '    Dim lstStudentFileNameList As String()

    '    lstStudentFileNameList = StudentFileNameList

    '    For Each strStudentFileName As String In lstStudentFileNameList
    '        If strInputFileName.ToUpper.IndexOf(strStudentFileName.ToUpper) > 0 Then
    '            blnValid = True
    '            Exit For
    '        End If
    '    Next

    '    Return blnValid
    'End Function
    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]

    'Public Function GetDynamicXLSParameter() As List(Of Integer)

    '    Dim lstSystemXLSParameter As List(Of Integer) = Nothing
    '    Dim lstDataSetXLSParameter As List(Of Integer) = Nothing
    '    Dim lstFullXLSParameter As List(Of Integer) = Nothing
    '    'Dim intDynamicSheetStartPosition As Integer = 0
    '    'Dim intDynamicSheetEndPosition As Integer = 0
    '    Dim intHeaderSheetCount As Integer = 0
    '    Dim intFooterSheetCount As Integer = 0
    '    Dim dsDataSource As New DataSet


    '    'Example XLS Parameter 1,-2,1
    '    lstSystemXLSParameter = MyBase.GetXLSParameter
    '    For i As Integer = 0 To lstSystemXLSParameter.Count - 1
    '        If lstSystemXLSParameter(i) > 0 Then
    '            intHeaderSheetCount = intHeaderSheetCount + 1
    '        Else
    '            Exit For
    '        End If
    '    Next

    '    For i As Integer = lstSystemXLSParameter.Count - 1 To 0 Step -1
    '        If lstSystemXLSParameter(i) > 0 Then
    '            intFooterSheetCount = intFooterSheetCount + 1
    '        Else
    '            Exit For
    '        End If
    '    Next

    '    dsDataSource = Me.GetDataSet()
    '    Dim intDataTableCount As Integer = dsDataSource.Tables.Count
    '    If intDataTableCount > 0 Then
    '        lstDataSetXLSParameter = New List(Of Integer)
    '        For i As Integer = 0 To intDataTableCount
    '            Dim intNumber As Integer = 1
    '            lstDataSetXLSParameter.Add(intNumber)
    '        Next
    '    End If

    '    For each





    '        Return lstFullXLSParameter
    'End Function

    Sub New(ByVal udtQueue As Common.Component.FileGeneration.FileGenerationQueueModel, ByVal udtFileGenerationModel As Common.Component.FileGeneration.FileGenerationModel)

        MyBase.New(udtQueue, udtFileGenerationModel)

        ' Get Out Put Path
        Dim strPath As String = String.Empty

        Dim udtCommonGenFunction As New Common.ComFunction.GeneralFunction()
        udtCommonGenFunction.getSystemParameter("GeneralFileStoragePath", strPath, String.Empty)
        If strPath.Trim() = "" Then
            Throw New ArgumentException("GeneralFileStoragePath Empty!")
        End If

        If Not strPath.Trim().EndsWith("\") Then
            strPath = strPath & "\"
        End If
        Me.m_strFileOutPath = strPath

    End Sub

    'Public Overrides Function GetDataSet() As System.Data.DataSet
    'Dim dsData As New DataSet()

    'Dim udtDB As New Common.DataAccess.Database()
    'Dim udtParamFunction As New Common.ComFunction.ParameterFunction()
    'Dim udtSPParamCollection As StoreProcParamCollection = udtParamFunction.GetSPParamCollection(Me.m_udtQueue.InParm)

    'Dim params(udtSPParamCollection.Count) As SqlParameter

    'For i As Integer = 0 To udtSPParamCollection.Count - 1
    '    Dim udtSPParamObject As StoreProcParamObject = udtSPParamCollection(i)
    '    params(i) = udtDB.MakeInParam(udtSPParamObject.ParamName, udtSPParamObject.ParamDBType, udtSPParamObject.ParamDBSize, udtSPParamObject.ParamValue)
    'Next

    'udtDB.RunProc(Me.m_udtFileGeneration.FileDataSP, params, dsData)

    'Return dsData
    'End Function

    Public Overrides Sub ConstructMessageParamaterList(ByRef udtDB As Common.DataAccess.Database, ByRef udtMessageCollection As MessageModelCollection, ByRef udtMessageReaderCollection As MessageReaderModelCollection)
        Dim udtFileGenerationBLL As New Common.Component.FileGeneration.FileGenerationBLL()
        Dim udtGeneral As New Common.ComFunction.GeneralFunction()
        Dim udtParamFunction As New Common.ComFunction.ParameterFunction()
        Dim udtFormatter As New Common.Format.Formatter

        Dim dtmCurrent As DateTime = udtGeneral.GetSystemDateTime()

        udtMessageCollection = New MessageModelCollection()
        udtMessageReaderCollection = New MessageReaderModelCollection()

        'Dim udtDB As New Common.DataAccess.Database()
        Dim dtResult As DataTable = udtFileGenerationBLL.GetViewAccessibleUser(udtDB, Me.m_udtQueue.GenerationID)

        ' Construct Message & MessageReader
        ' One Message, To Mutilple User (Other Case May Mutil Message, Each Message to Single User
        Dim udtMessage As New MessageModel()
        udtMessage.MessageID = udtGeneral.generateInboxMsgID()

        Dim paramsSubject As New ParameterCollection()
        paramsSubject.AddParam("FileName", Me.GetFileName())

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'paramsSubject.AddParam("Date", Now.ToString("yyyyMMMdd"))
        'paramsSubject.AddParam("Date", udtFormatter.formatDate(Now, "en"))
        paramsSubject.AddParam("Date", udtFormatter.formatDisplayDate(Now, CultureLanguage.English))
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]



        Dim paramsContent As New ParameterCollection()
        paramsContent.AddParam("FileName", Me.GetFileName())
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

        ConstructSPInboxMessageParamaterList(udtDB, udtMessageCollection, udtMessageReaderCollection)
    End Sub

    ' CRE19-001-02 (PPP 2019-20) [Start][Koala]
    Private Sub ConstructSPInboxMessageParamaterList(ByRef udtDB As Common.DataAccess.Database, ByRef udtMessageCollection As MessageModelCollection, ByRef udtMessageReaderCollection As MessageReaderModelCollection)
        Dim udtFileGenerationBLL As New Common.Component.FileGeneration.FileGenerationBLL()
        Dim udtGeneral As New Common.ComFunction.GeneralFunction()
        Dim udtParamFunction As New Common.ComFunction.ParameterFunction()
        Dim udtFormatter As New Common.Format.Formatter

        Dim dtmCurrent As DateTime = udtGeneral.GetSystemDateTime()

        ' Retrieve Inbox Message Template
        Dim udtInternetMailBLL As New InternetMailBLL()
        Dim udtMailTemplate As MailTemplateModel

        Select Case Me.m_udtFileGeneration.FileID
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
            Case DataDownloadFileID.eHSVF000
                udtMailTemplate = udtInternetMailBLL.GetMailTemplate(udtDB, Common.Component.InboxMsgTemplateID.VacccintaionFIleReportVF000)
                ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]
            Case DataDownloadFileID.eHSVF001
                udtMailTemplate = udtInternetMailBLL.GetMailTemplate(udtDB, Common.Component.InboxMsgTemplateID.VacccintaionFIleReportVF001)
            Case DataDownloadFileID.eHSVF002
                udtMailTemplate = udtInternetMailBLL.GetMailTemplate(udtDB, Common.Component.InboxMsgTemplateID.VacccintaionFIleReportVF002)
            Case DataDownloadFileID.eHSVF003
                udtMailTemplate = udtInternetMailBLL.GetMailTemplate(udtDB, Common.Component.InboxMsgTemplateID.VacccintaionFIleReportVF003)
            Case DataDownloadFileID.eHSVF005, DataDownloadFileID.eHSVF006
                ' No SP Inbox msg
                Return
            Case Else
                Throw New Exception("Undefined File ID (" + Me.m_udtFileGeneration.FileID + ") for creating inbox message to service provider")
        End Select

        ' Retrieve Student File Header
        Dim udtStudentHeader As Common.Component.StudentFile.StudentFileHeaderModel = GetStudentFileHeader()

        ' Prepare SubsidizeItemDetial (Dose: 1STDOSE, 2NDDOSE, ONLYDOSE)
        Dim strSubsidizeItemCode As String = String.Empty
        Dim cllnSubsidizeItemDetail As SchemeDetails.SubsidizeItemDetailsModelCollection = Nothing
        Dim udtSubsidizeItemDetail As SchemeDetails.SubsidizeItemDetailsModel = Nothing
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
        If Not udtStudentHeader.Precheck Then
            strSubsidizeItemCode = (New Scheme.SubsidizeBLL).GetSubsidizeItemBySubsidize(udtStudentHeader.SubsidizeCode)
            cllnSubsidizeItemDetail = (New SchemeDetails.SchemeDetailBLL).getSubsidizeItemDetails(strSubsidizeItemCode)
            udtSubsidizeItemDetail = cllnSubsidizeItemDetail.Filter(strSubsidizeItemCode, udtStudentHeader.Dose)
        End If
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]


        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
        ' Get vaccine type, e.g. QIV, 23vPPV, PCV13, MMR
        Dim udtSubsidizeBLL As New Scheme.SubsidizeBLL
        Dim strVaccineType As String = String.Empty
        Dim strDoseEN As String = String.Empty
        Dim strDoseCH As String = String.Empty
        If Not udtStudentHeader.Precheck Then
            strVaccineType = udtSubsidizeBLL.GetVaccineTypeBySubsidizeCode(udtStudentHeader.SubsidizeCode)
            strDoseEN = udtSubsidizeItemDetail.AvailableItemDesc.ToLower
            strDoseCH = udtSubsidizeItemDetail.AvailableItemDescChi.ToLower
        End If
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]

        ' Construct Message & MessageReader
        Dim udtMessage As New MessageModel()
        udtMessage.MessageID = udtGeneral.generateInboxMsgID()

        ' Construct Inbox Message Subject (No param)
        Dim paramsSubject As New ParameterCollection()
        paramsSubject.AddParam("SchoolCode", udtStudentHeader.SchoolCode)
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
        paramsSubject.AddParam("VaccineType", strVaccineType)
        paramsSubject.AddParam("Dose", strDoseEN)
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]


        ' Construct Inbox Message Content
        Dim paramsContentEN As New ParameterCollection()
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
        paramsContentEN.AddParam("FileID", udtStudentHeader.StudentFileID)
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]
        paramsContentEN.AddParam("SchoolCode", udtStudentHeader.SchoolCode)
        paramsContentEN.AddParam("SchoolName", udtStudentHeader.SchoolNameEN)
        If Not udtStudentHeader.Precheck Then
            paramsContentEN.AddParam("ServiceDate", udtStudentHeader.ServiceReceiveDtm.Value.ToString(udtFormatter.DisplayDateFormat))
            paramsContentEN.AddParam("Dose", strDoseEN)
        End If

        Dim paramsContentCH As New ParameterCollection()
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
        paramsContentCH.AddParam("FileID", udtStudentHeader.StudentFileID)
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]
        paramsContentCH.AddParam("SchoolCode", udtStudentHeader.SchoolCode)
        paramsContentCH.AddParam("SchoolName", udtStudentHeader.SchoolNameCH)
        If Not udtStudentHeader.Precheck Then
            paramsContentCH.AddParam("Day", udtStudentHeader.ServiceReceiveDtm.Value.ToString("dd"))
            paramsContentCH.AddParam("Month", udtStudentHeader.ServiceReceiveDtm.Value.ToString("MM"))
            paramsContentCH.AddParam("Year", udtStudentHeader.ServiceReceiveDtm.Value.Year.ToString())
            paramsContentCH.AddParam("Dose", strDoseCH)
        End If

        ' Build Message Subject
        Dim strLang As EnumLanguage = (New UserAC.UserACBLL).GetUserACDefaultLang(udtStudentHeader.SPID)
        If strLang = EnumLanguage.EN Then
            udtMessage.Subject = udtParamFunction.GetParsedStringByparameter(udtMailTemplate.MailSubjectEng, paramsSubject)
        Else
            udtMessage.Subject = udtParamFunction.GetParsedStringByparameter(udtMailTemplate.MailSubjectChi, paramsSubject)
        End If

        ' Build Message Content and other information
        udtMessage.Message = udtParamFunction.GetParsedStringByparameter(udtMailTemplate.MailBodyChi, paramsContentCH) + " " + udtParamFunction.GetParsedStringByparameter(udtMailTemplate.MailBodyEng, paramsContentEN)
        udtMessage.CreateBy = "EHCVS"
        udtMessage.CreateDtm = dtmCurrent
        udtMessageCollection.Add(udtMessage)

        ' Build Message reader (SPID)
        Dim udtMessageReader As New MessageReaderModel()
        udtMessageReader.MessageID = udtMessage.MessageID
        udtMessageReader.MessageReader = udtStudentHeader.SPID
        udtMessageReader.UpdateBy = "EHCVS"
        udtMessageReader.UpdateDtm = dtmCurrent

        udtMessageReaderCollection.Add(udtMessageReader)

    End Sub
    ' CRE19-001-02 (PPP 2019-20) [End][Koala]
End Class
