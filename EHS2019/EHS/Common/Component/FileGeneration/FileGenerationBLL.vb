Imports System.Data.SqlClient
Imports System.Data
Imports System.Xml

' [CRE12-004] Statistic Enquiry [Start][Tommy Tse]

Imports Common.Component.HCVUUser
Imports Common.Component.Inbox
Imports Common.ComFunction.ParameterFunction
Imports Microsoft.Office.Interop
Imports System.Runtime.InteropServices.Marshal
Imports System.IO
Imports Common.ComFunction

' [CRE12-004] Statistic Enquiry [End][Tommy Tse]

Imports Common.DataAccess

Namespace Component.FileGeneration
    ''' <summary>
    ''' File Generation BLL To Handle [dbo].[FileGeneration] And [dbo].[FileGenerationQueue]
    ''' It will Also Provide User_ID that can access the FileGeneration
    ''' </summary>
    ''' <remarks></remarks>
    Public Class FileGenerationBLL

        ' [CRE12-004] Statistic Enquiry [Start][Tommy Tse]

        Private Const _excelTemplateDate = "dd MMM yyyy HH:mm:ss"

        ' [CRE12-004] Statistic Enquiry [End][Tommy Tse]

        Private udtDB As New Database()

        Public Sub New()
        End Sub


        Public Function AddFileGenerationQueue(ByRef udtDB As Database, ByVal udtFileGenerationQueueModel As FileGenerationQueueModel) As Boolean
            Try

                ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Dim prams() As SqlParameter = { _
                              udtDB.MakeInParam("@Generation_ID", FileGenerationQueueModel.Generation_IDDataType, FileGenerationQueueModel.Generation_IDDataSize, udtFileGenerationQueueModel.GenerationID), _
                              udtDB.MakeInParam("@File_ID", FileGenerationQueueModel.File_IDDataType, FileGenerationQueueModel.File_IDDataSize, udtFileGenerationQueueModel.FileID), _
                              udtDB.MakeInParam("@In_Parm", FileGenerationQueueModel.In_ParmDataType, FileGenerationQueueModel.In_ParmDataSize, udtFileGenerationQueueModel.InParm), _
                              udtDB.MakeInParam("@Output_File", FileGenerationQueueModel.Output_FileDataType, FileGenerationQueueModel.Output_FileDataSize, udtFileGenerationQueueModel.OutputFile), _
                              udtDB.MakeInParam("@Status", FileGenerationQueueModel.StatusDataType, FileGenerationQueueModel.StatusDataSize, udtFileGenerationQueueModel.Status), _
                              udtDB.MakeInParam("@File_Password", FileGenerationQueueModel.File_PasswordDataType, FileGenerationQueueModel.File_PasswordDataSize, udtFileGenerationQueueModel.FilePassword), _
                              udtDB.MakeInParam("@Request_By", FileGenerationQueueModel.Request_ByDataType, FileGenerationQueueModel.Request_ByDataSize, udtFileGenerationQueueModel.RequestBy), _
                              udtDB.MakeInParam("@File_Description", FileGenerationQueueModel.File_DescriptionDataType, FileGenerationQueueModel.File_DescriptionDataSize, udtFileGenerationQueueModel.FileDescription), _
                              udtDB.MakeInParam("@Schedule_Gen_Dtm", FileGenerationQueueModel.Schedule_Gen_DtmDataType, FileGenerationQueueModel.Schedule_Gen_DtmDataSize, IIf(udtFileGenerationQueueModel.ScheduleGenDtm Is Nothing, DBNull.Value, udtFileGenerationQueueModel.ScheduleGenDtm))}
                ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]

                udtDB.RunProc("proc_FileGenerationQueue_add", prams)

                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Public Function RetrieveFileGenerationListForReportSubmission(ByVal strUserID As String) As DataTable

        Public Function RetrieveFileGenerationListForReportSubmission(ByVal strUserID As String, ByVal strSubmissionReportType As String) As DataTable

            Dim dtResult As New DataTable()
            Try
                '[File_ID],	[File_Name], [File_Desc], [File_Name_Prefix], [File_Type],
                '[File_Data_SP], [FilterCriteria_UC], [Report_Template], [Group_ID], [Is_SelfAccess],
                '[Auto_Generate], [Show_for_Generation], [Frequency], [Day_of_Generation], [Message_Subject], [Message_Content],
                '[Create_Dtm], [Create_By], [Update_Dtm], [Update_By]

                'Dim prams() As SqlParameter = { _
                '              udtDB.MakeInParam("@User_ID", SqlDbType.VarChar, 20, strUserID)}
                Dim prams() As SqlParameter = { _
                              udtDB.MakeInParam("@User_ID", SqlDbType.VarChar, 20, strUserID), _
                              udtDB.MakeInParam("@Show_for_Generation", SqlDbType.VarChar, 1, strSubmissionReportType)}

                udtDB.RunProc("proc_FileGeneration_get_ReportSubmission", prams, dtResult)
                Return dtResult

            Catch ex As Exception
                Throw ex
            End Try
        End Function
        'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]

        ''' <summary>
        ''' [Public] Retrieve Single Entry [dbo].[FileGeneration] By File ID 
        ''' </summary>
        ''' <param name="strFileID"></param>
        ''' <returns>FileGenerationModel</returns>
        ''' <remarks></remarks>
        Public Function RetrieveFileGeneration(ByVal strFileID As String) As FileGenerationModel
            Dim udtFileGenerationModel As FileGenerationModel = Nothing

            Dim dtmCreateDtm, dtmUpdateDtm As Nullable(Of DateTime)
            Dim strFileName, strFileDesc, strFileNamePrefix, strFileType, strDisplayCode, strOutputFileName, strOutputFileDescription, strFilePrepareDataSP, strFileDataSP, strFilterCriteriaUC, strReportTemplate As String
            Dim strGroupID, strIsSelfAccess, strAutoGenerate, strFrequency, strShowForGeneration, strMessageSubject, strMessageContent As String
            Dim intDayOfGeneration As Integer
            Dim strCreateBy, strUpdateBy As String
            Dim strGet_Data_From_Bak As String
            Dim listOFXLSParameter As List(Of Integer)

            Try
                Dim params() As SqlParameter = { _
                    udtDB.MakeInParam("@File_ID", FileGenerationModel.File_IDDataType, FileGenerationModel.File_IDDataSize, strFileID)}

                Dim dtResult As New DataTable()

                '[File_ID],	[File_Name], [File_Desc], [File_Name_Prefix], [File_Type],
                '[File_Data_SP], [FilterCriteria_UC], [Report_Template], [Group_ID], [Is_SelfAccess],
                '[Auto_Generate], [Frequency], [Day_of_Generation], [Message_Subject], [Message_Content],
                '[Create_Dtm], [Create_By], [Update_Dtm], [Update_By]

                udtDB.RunProc("proc_FileGeneration_get_ByKey", params, dtResult)

                If dtResult.Rows.Count > 0 Then
                    Dim drRow As DataRow = dtResult.Rows(0)

                    If IsDBNull(drRow("File_Name")) Then strFileName = Nothing Else strFileName = drRow("File_Name").ToString().Trim()
                    If IsDBNull(drRow("File_Desc")) Then strFileDesc = Nothing Else strFileDesc = drRow("File_Desc").ToString().Trim()
                    If IsDBNull(drRow("File_Name_Prefix")) Then strFileNamePrefix = Nothing Else strFileNamePrefix = drRow("File_Name_Prefix").ToString().Trim()
                    If IsDBNull(drRow("File_Type")) Then strFileType = Nothing Else strFileType = drRow("File_Type").ToString().Trim()
                    ' CRE15-006 Rename of eHS [Start][Lawrence]
                    If IsDBNull(drRow("Display_Code")) Then strDisplayCode = Nothing Else strDisplayCode = drRow("Display_Code").ToString().Trim()
                    If IsDBNull(drRow("Output_File_Name")) Then strOutputFileName = Nothing Else strOutputFileName = drRow("Output_File_Name").ToString().Trim()
                    If IsDBNull(drRow("Output_File_Description")) Then strOutputFileDescription = Nothing Else strOutputFileDescription = drRow("Output_File_Description").ToString().Trim()
                    If IsDBNull(drRow("File_Prepare_Data_SP")) Then strFilePrepareDataSP = Nothing Else strFilePrepareDataSP = drRow("File_Prepare_Data_SP").ToString().Trim()
                    ' CRE15-006 Rename of eHS [End][Lawrence]
                    If IsDBNull(drRow("File_Data_SP")) Then strFileDataSP = Nothing Else strFileDataSP = drRow("File_Data_SP").ToString().Trim()
                    If IsDBNull(drRow("FilterCriteria_UC")) Then strFilterCriteriaUC = Nothing Else strFilterCriteriaUC = drRow("FilterCriteria_UC").ToString().Trim()

                    If IsDBNull(drRow("Report_Template")) Then strReportTemplate = Nothing Else strReportTemplate = drRow("Report_Template").ToString().Trim()
                    If IsDBNull(drRow("Group_ID")) Then strGroupID = Nothing Else strGroupID = drRow("Group_ID").ToString().Trim()
                    If IsDBNull(drRow("Is_SelfAccess")) Then strIsSelfAccess = Nothing Else strIsSelfAccess = drRow("Is_SelfAccess").ToString().Trim()
                    If IsDBNull(drRow("Auto_Generate")) Then strAutoGenerate = Nothing Else strAutoGenerate = drRow("Auto_Generate").ToString().Trim()
                    If IsDBNull(drRow("Show_for_Generation")) Then strShowForGeneration = Nothing Else strShowForGeneration = drRow("Show_for_Generation").ToString().Trim()

                    If IsDBNull(drRow("Frequency")) Then strFrequency = Nothing Else strFrequency = drRow("Frequency").ToString().Trim()
                    If IsDBNull(drRow("Day_of_Generation")) Then intDayOfGeneration = Nothing Else intDayOfGeneration = Convert.ToInt32(drRow("Day_of_Generation"))

                    If IsDBNull(drRow("Message_Subject")) Then strMessageSubject = Nothing Else strMessageSubject = drRow("Message_Subject").ToString().Trim()
                    If IsDBNull(drRow("Message_Content")) Then strMessageContent = Nothing Else strMessageContent = drRow("Message_Content").ToString().Trim()

                    If IsDBNull(drRow("Create_Dtm")) Then dtmCreateDtm = Nothing Else dtmCreateDtm = Convert.ToDateTime(drRow("Create_Dtm"))
                    If IsDBNull(drRow("Create_By")) Then strCreateBy = Nothing Else strCreateBy = drRow("Create_By").ToString().Trim()
                    If IsDBNull(drRow("Update_Dtm")) Then dtmUpdateDtm = Nothing Else dtmUpdateDtm = Convert.ToDateTime(drRow("Update_Dtm"))
                    If IsDBNull(drRow("Update_By")) Then strUpdateBy = Nothing Else strUpdateBy = drRow("Update_By").ToString().Trim()

                    If IsDBNull(drRow("Get_Data_From_Bak")) Then strGet_Data_From_Bak = Nothing Else strGet_Data_From_Bak = drRow("Get_Data_From_Bak").ToString().Trim()

                    If IsDBNull(drRow("XLS_Parameter")) Then
                        listOFXLSParameter = Nothing
                    Else
                        listOFXLSParameter = New List(Of Integer)(Array.ConvertAll(Of String, Integer)(CStr(drRow("XLS_Parameter")).Trim.Split(","c), AddressOf ConvertStringToInt))
                    End If

                    ' [CRE12-004] Statistic Enquiry [Start][Tommy Tse]

                    udtFileGenerationModel = New FileGenerationModel(strFileID, strFileName, strFileDesc, strFileNamePrefix, strFileType, strDisplayCode, strOutputFileName, strOutputFileDescription, strFilePrepareDataSP, strFileDataSP, _
                        strFilterCriteriaUC, strReportTemplate, strGroupID, strIsSelfAccess, strAutoGenerate, strShowForGeneration, strFrequency, intDayOfGeneration, _
                        strMessageSubject, strMessageContent, dtmCreateDtm, strCreateBy, dtmUpdateDtm, strUpdateBy, strGet_Data_From_Bak, listOFXLSParameter)

                    ' [CRE12-004] Statistic Enquiry [End][Tommy Tse]

                End If

            Catch ex As Exception
                Throw ex
            End Try

            Return udtFileGenerationModel
        End Function

        ''' <summary>
        ''' [External] Retrieve Single Entry [dbo].[FileGeneration] By File ID 
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <param name="strFileID"></param>
        ''' <returns>FileGeneration</returns>
        ''' <remarks></remarks>
        Public Function RetrieveFileGeneration(ByVal udtDB As Database, ByVal strFileID As String) As FileGenerationModel
            Dim udtFileGenerationModel As FileGenerationModel = Nothing

            Dim dtmCreateDtm, dtmUpdateDtm As Nullable(Of DateTime)
            Dim strFileName, strFileDesc, strFileNamePrefix, strFileType, strDisplayCode, strOutputFileName, strOutputFileDescription, strFilePrepareDataSP, strFileDataSP, strFilterCriteriaUC, strReportTemplate As String
            Dim strGroupID, strIsSelfAccess, strAutoGenerate, strShowForGeneration, strFrequency, strMessageSubject, strMessageContent As String
            Dim intDayOfGeneration As Integer
            Dim strCreateBy, strUpdateBy As String
            Dim strGet_Data_From_Bak As String
            Dim listOFXLSParameter As List(Of Integer)

            Try
                Dim params() As SqlParameter = { _
                    udtDB.MakeInParam("@File_ID", FileGenerationModel.File_IDDataType, FileGenerationModel.File_IDDataSize, strFileID)}

                Dim dtResult As New DataTable()

                '[File_ID],	[File_Name], [File_Desc], [File_Name_Prefix], [File_Type],
                '[File_Data_SP], [FilterCriteria_UC], [Report_Template], [Group_ID], [Is_SelfAccess],
                '[Auto_Generate], [Frequency], [Day_of_Generation], [Message_Subject], [Message_Content],
                '[Create_Dtm], [Create_By], [Update_Dtm], [Update_By]

                udtDB.RunProc("proc_FileGeneration_get_ByKey", params, dtResult)

                If dtResult.Rows.Count > 0 Then
                    Dim drRow As DataRow = dtResult.Rows(0)

                    If IsDBNull(drRow("File_Name")) Then strFileName = Nothing Else strFileName = drRow("File_Name").ToString().Trim()
                    If IsDBNull(drRow("File_Desc")) Then strFileDesc = Nothing Else strFileDesc = drRow("File_Desc").ToString().Trim()
                    If IsDBNull(drRow("File_Name_Prefix")) Then strFileNamePrefix = Nothing Else strFileNamePrefix = drRow("File_Name_Prefix").ToString().Trim()
                    If IsDBNull(drRow("File_Type")) Then strFileType = Nothing Else strFileType = drRow("File_Type").ToString().Trim()
                    ' CRE15-006 Rename of eHS [Start][Lawrence]
                    If IsDBNull(drRow("Display_Code")) Then strDisplayCode = Nothing Else strDisplayCode = drRow("Display_Code").ToString().Trim()
                    If IsDBNull(drRow("Output_File_Name")) Then strOutputFileName = Nothing Else strOutputFileName = drRow("Output_File_Name").ToString().Trim()
                    If IsDBNull(drRow("Output_File_Description")) Then strOutputFileDescription = Nothing Else strOutputFileDescription = drRow("Output_File_Description").ToString().Trim()
                    If IsDBNull(drRow("File_Prepare_Data_SP")) Then strFilePrepareDataSP = Nothing Else strFilePrepareDataSP = drRow("File_Prepare_Data_SP").ToString().Trim()
                    ' CRE15-006 Rename of eHS [End][Lawrence]
                    If IsDBNull(drRow("File_Data_SP")) Then strFileDataSP = Nothing Else strFileDataSP = drRow("File_Data_SP").ToString().Trim()
                    If IsDBNull(drRow("FilterCriteria_UC")) Then strFilterCriteriaUC = Nothing Else strFilterCriteriaUC = drRow("FilterCriteria_UC").ToString().Trim()

                    If IsDBNull(drRow("Report_Template")) Then strReportTemplate = Nothing Else strReportTemplate = drRow("Report_Template").ToString().Trim()
                    If IsDBNull(drRow("Group_ID")) Then strGroupID = Nothing Else strGroupID = drRow("Group_ID").ToString().Trim()
                    If IsDBNull(drRow("Is_SelfAccess")) Then strIsSelfAccess = Nothing Else strIsSelfAccess = drRow("Is_SelfAccess").ToString().Trim()
                    If IsDBNull(drRow("Auto_Generate")) Then strAutoGenerate = Nothing Else strAutoGenerate = drRow("Auto_Generate").ToString().Trim()
                    If IsDBNull(drRow("Show_for_Generation")) Then strShowForGeneration = Nothing Else strShowForGeneration = drRow("Show_for_Generation").ToString().Trim()


                    If IsDBNull(drRow("Frequency")) Then strFrequency = Nothing Else strFrequency = drRow("Frequency").ToString().Trim()
                    If IsDBNull(drRow("Day_of_Generation")) Then intDayOfGeneration = Nothing Else intDayOfGeneration = Convert.ToInt32(drRow("Day_of_Generation"))

                    If IsDBNull(drRow("Message_Subject")) Then strMessageSubject = Nothing Else strMessageSubject = drRow("Message_Subject").ToString().Trim()
                    If IsDBNull(drRow("Message_Content")) Then strMessageContent = Nothing Else strMessageContent = drRow("Message_Content").ToString().Trim()

                    If IsDBNull(drRow("Create_Dtm")) Then dtmCreateDtm = Nothing Else dtmCreateDtm = Convert.ToDateTime(drRow("Create_Dtm"))
                    If IsDBNull(drRow("Create_By")) Then strCreateBy = Nothing Else strCreateBy = drRow("Create_By").ToString().Trim()
                    If IsDBNull(drRow("Update_Dtm")) Then dtmUpdateDtm = Nothing Else dtmUpdateDtm = Convert.ToDateTime(drRow("Update_Dtm"))
                    If IsDBNull(drRow("Update_By")) Then strUpdateBy = Nothing Else strUpdateBy = drRow("Update_By").ToString().Trim()

                    If IsDBNull(drRow("Get_Data_From_Bak")) Then strGet_Data_From_Bak = Nothing Else strGet_Data_From_Bak = drRow("Get_Data_From_Bak").ToString().Trim()

                    If IsDBNull(drRow("XLS_Parameter")) Then
                        listOFXLSParameter = Nothing
                    Else
                        listOFXLSParameter = New List(Of Integer)(Array.ConvertAll(Of String, Integer)(CStr(drRow("XLS_Parameter")).Trim.Split(","c), AddressOf ConvertStringToInt))
                    End If

                    ' [CRE12-004] Statistic Enquiry [Start][Tommy Tse]

                    udtFileGenerationModel = New FileGenerationModel(strFileID, strFileName, strFileDesc, strFileNamePrefix, strFileType, strDisplayCode, strOutputFileName, strOutputFileDescription, strFilePrepareDataSP, strFileDataSP, _
                        strFilterCriteriaUC, strReportTemplate, strGroupID, strIsSelfAccess, strAutoGenerate, strShowForGeneration, strFrequency, intDayOfGeneration, _
                        strMessageSubject, strMessageContent, dtmCreateDtm, strCreateBy, dtmUpdateDtm, strUpdateBy, strGet_Data_From_Bak, listOFXLSParameter)

                    ' [CRE12-004] Statistic Enquiry [End][Tommy Tse]

                End If

            Catch ex As Exception
                Throw ex
            End Try

            Return udtFileGenerationModel
        End Function

        ' CRE13-016 - Upgrade to excel 2007 [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieve [dbo].[FileGenerationQueue] Records With Status = 'P', Start DateTime, Complete DateTime = NULL
        ''' </summary>
        ''' <param name="strFileTypeList">List of File Type of [dbo].[FileGeneration]</param>
        ''' <returns>FileGenerationQueue List</returns>
        ''' <remarks></remarks>
        Public Function RetrieveFileGenerationQueueToRun(ByVal strFileTypeList As String) As FileGenerationQueueModelCollection
            'Public Function RetrieveFileGenerationQueueToRun(ByVal strFileType As String) As FileGenerationQueueModelCollection
            ' CRE13-016 - Upgrade to excel 2007 [End][Tommy L]

            Dim udtFileGenerationQueueModelCollection As New FileGenerationQueueModelCollection()

            Dim strGenerationID, strFileID, strInParam, strOutputFile, strStatus, strFilePassword As String
            Dim strRequestBy, strFileDescription As String
            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Dim dtmRequestDtm, dtmStartDtm, dtmCompleteDtm, dtmScheduleGenDtm As Nullable(Of DateTime)
            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]

            Try
                Dim dtResult As New DataTable()
                '[Generation_ID], [File_ID], [In_Parm], [Output_File], [Status],
                '[File_Password], [Request_Dtm], [Request_By]
                '[Start_dtm], [Complete_dtm], [File_Description], [Schedule_Gen_Dtm]

                ' CRE13-016 - Upgrade to excel 2007 [Start][Tommy L]
                ' -----------------------------------------------------------------------------------------
                'Dim params() As SqlParameter = { _
                '    udtDB.MakeInParam("@File_Type", FileGenerationModel.File_TypeDataType, FileGenerationModel.File_TypeDataSize, strFileType)}
                Dim params() As SqlParameter = { _
                    udtDB.MakeInParam("@File_Type_List", SqlDbType.VarChar, 500, strFileTypeList)}
                ' CRE13-016 - Upgrade to excel 2007 [End][Tommy L]

                udtDB.RunProc("proc_FileGenerationQueue_get_ToRun", params, dtResult)

                For i As Integer = 0 To dtResult.Rows.Count - 1

                    Dim drRow As DataRow = dtResult.Rows(i)

                    If IsDBNull(drRow("Generation_ID")) Then strGenerationID = Nothing Else strGenerationID = drRow("Generation_ID").ToString().Trim()
                    If IsDBNull(drRow("File_ID")) Then strFileID = Nothing Else strFileID = drRow("File_ID").ToString().Trim()
                    If IsDBNull(drRow("In_Parm")) Then strInParam = Nothing Else strInParam = drRow("In_Parm").ToString().Trim()
                    If IsDBNull(drRow("Output_File")) Then strOutputFile = Nothing Else strOutputFile = drRow("Output_File").ToString().Trim()
                    If IsDBNull(drRow("Status")) Then strStatus = Nothing Else strStatus = drRow("Status").ToString().Trim()
                    If IsDBNull(drRow("File_Password")) Then strFilePassword = Nothing Else strFilePassword = drRow("File_Password").ToString().Trim()

                    If IsDBNull(drRow("Request_Dtm")) Then dtmRequestDtm = Nothing Else dtmRequestDtm = Convert.ToDateTime(drRow("Request_Dtm"))
                    If IsDBNull(drRow("Request_By")) Then strRequestBy = Nothing Else strRequestBy = drRow("Request_By").ToString().Trim()
                    If IsDBNull(drRow("Start_dtm")) Then dtmStartDtm = Nothing Else dtmStartDtm = Convert.ToDateTime(drRow("Start_dtm"))
                    If IsDBNull(drRow("Complete_dtm")) Then dtmCompleteDtm = Nothing Else dtmCompleteDtm = Convert.ToDateTime(drRow("Complete_dtm"))

                    If IsDBNull(drRow("File_Description")) Then strFileDescription = Nothing Else strFileDescription = drRow("File_Description").ToString().Trim()

                    ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    If IsDBNull(drRow("Schedule_Gen_Dtm")) Then dtmScheduleGenDtm = Nothing Else dtmScheduleGenDtm = Convert.ToDateTime(drRow("Schedule_Gen_Dtm"))

                    Dim udtFileGenerationQueueModel As FileGenerationQueueModel = New FileGenerationQueueModel(strGenerationID, strFileID, strInParam, strOutputFile, _
                        strStatus, strFilePassword, dtmRequestDtm, strRequestBy, dtmStartDtm, dtmCompleteDtm, strFileDescription, dtmScheduleGenDtm)
                    ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]

                    udtFileGenerationQueueModelCollection.Add(udtFileGenerationQueueModel)
                Next

            Catch ex As Exception
                Throw ex
            End Try

            Return udtFileGenerationQueueModelCollection
        End Function

        ''' <summary>
        ''' For BNC Cancel Export To Retrieve Related File Generation Queue
        ''' </summary>
        ''' <param name="strFileID"></param>
        ''' <param name="strFileName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function RetrieveFileGenerationQueueByFileIDFileName(ByVal strFileID As String, ByVal strFileName As String) As FileGenerationQueueModel

            Dim udtFileGenerationQueueModel As FileGenerationQueueModel = Nothing

            Dim strGenerationID, strInParam, strOutputFile, strStatus, strFilePassword As String
            Dim strRequestBy, strFileDescription As String
            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Dim dtmRequestDtm, dtmStartDtm, dtmCompleteDtm, dtmScheduleGenDtm As Nullable(Of DateTime)
            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]

            Try
                Dim dtResult As New DataTable()
                '[Generation_ID], [File_ID], [In_Parm], [Output_File], [Status],
                '[File_Password], [Request_Dtm], [Request_By],
                '[Start_dtm], [Complete_dtm], [File_Description], [Schedule_Gen_Dtm]

                Dim params() As SqlParameter = { _
                    udtDB.MakeInParam("@File_ID", FileGenerationModel.File_IDDataType, FileGenerationModel.File_IDDataSize, strFileID), _
                    udtDB.MakeInParam("@File_Name", SqlDbType.Char, 15, strFileName)}

                udtDB.RunProc("proc_FileGenerationQueue_get_ByFileIDFileName", params, dtResult)

                If dtResult.Rows.Count = 1 Then

                    Dim drRow As DataRow = dtResult.Rows(0)

                    If IsDBNull(drRow("Generation_ID")) Then strGenerationID = Nothing Else strGenerationID = drRow("Generation_ID").ToString().Trim()
                    If IsDBNull(drRow("File_ID")) Then strFileID = Nothing Else strFileID = drRow("File_ID").ToString().Trim()
                    If IsDBNull(drRow("In_Parm")) Then strInParam = Nothing Else strInParam = drRow("In_Parm").ToString().Trim()
                    If IsDBNull(drRow("Output_File")) Then strOutputFile = Nothing Else strOutputFile = drRow("Output_File").ToString().Trim()
                    If IsDBNull(drRow("Status")) Then strStatus = Nothing Else strStatus = drRow("Status").ToString().Trim()
                    If IsDBNull(drRow("File_Password")) Then strFilePassword = Nothing Else strFilePassword = drRow("File_Password").ToString().Trim()

                    If IsDBNull(drRow("Request_Dtm")) Then dtmRequestDtm = Nothing Else dtmRequestDtm = Convert.ToDateTime(drRow("Request_Dtm"))
                    If IsDBNull(drRow("Request_By")) Then strRequestBy = Nothing Else strRequestBy = drRow("Request_By").ToString().Trim()
                    If IsDBNull(drRow("Start_dtm")) Then dtmStartDtm = Nothing Else dtmStartDtm = Convert.ToDateTime(drRow("Start_dtm"))
                    If IsDBNull(drRow("Complete_dtm")) Then dtmCompleteDtm = Nothing Else dtmCompleteDtm = Convert.ToDateTime(drRow("Complete_dtm"))

                    If IsDBNull(drRow("File_Description")) Then strFileDescription = Nothing Else strFileDescription = drRow("File_Description").ToString().Trim()

                    ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    If IsDBNull(drRow("Schedule_Gen_Dtm")) Then dtmScheduleGenDtm = Nothing Else dtmScheduleGenDtm = Convert.ToDateTime(drRow("Schedule_Gen_Dtm"))

                    udtFileGenerationQueueModel = New FileGenerationQueueModel(strGenerationID, strFileID, strInParam, strOutputFile, _
                        strStatus, strFilePassword, dtmRequestDtm, strRequestBy, dtmStartDtm, dtmCompleteDtm, strFileDescription, dtmScheduleGenDtm)
                    ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]
                End If

            Catch ex As Exception
                Throw ex
            End Try

            Return udtFileGenerationQueueModel
        End Function

        Public Function RetrieveFileGenerationUserControl(ByVal strFileID As String) As FileGenerationUserControlModelCollection
            Dim udtUserControlList As New FileGenerationUserControlModelCollection

            Dim dtResult As New DataTable

            Dim params() As SqlParameter = { _
                udtDB.MakeInParam("@File_ID", SqlDbType.VarChar, 30, strFileID) _
            }

            udtDB.RunProc("proc_FileGenerationUserControl_Get_ByFileID", params, dtResult)

            For Each dr As DataRow In dtResult.Rows
                Dim strParameterSuffix As String = String.Empty
                If Not IsDBNull(dr("Parameter_Suffix")) Then strParameterSuffix = dr("Parameter_Suffix").ToString.Trim

                Dim dicUserControlSetting As New Dictionary(Of String, String)

                If Not IsDBNull(dr("UC_Setting")) Then
                    Dim xml As New XmlDocument
                    xml.LoadXml(dr("UC_Setting").ToString.Trim)

                    'CRE13-003 Token Replacement [Start][Karl]
                    Select Case xml.DocumentElement.Name.ToLower
                        Case "setting"
                            'CRE13-003 Token Replacement [End][Karl]
                            For Each node As XmlNode In xml.GetElementsByTagName("Setting")(0).ChildNodes
                                dicUserControlSetting.Add(node.Name, node.InnerText)
                            Next


                            'CRE13-003 Token Replacement [Start][Karl]
                        Case "controllist"
                            'do not prase xml dictionary

                    End Select
                    'CRE13-003 Token Replacement [End][Karl]
                End If

                'udtUserControlList.Add(New FileGenerationUserControlModel( _
                '      dr("File_ID").ToString.Trim, _
                '      CInt(dr("Display_Seq")), _
                '      dr("UC_ID").ToString.Trim, _
                '      dicUserControlSetting, _
                '      strParameterSuffix) _
                '  )
                udtUserControlList.Add(New FileGenerationUserControlModel( _
                dr("File_ID").ToString.Trim, _
                CInt(dr("Display_Seq")), _
                dr("UC_ID").ToString.Trim, _
                dicUserControlSetting, _
                strParameterSuffix, _
                dr("UC_Setting").ToString.Trim) _
              )

            Next

            Return udtUserControlList

        End Function

        ' CRE15-016 (Randomly genereate the valid claim transaction) [Start][Winnie]
        Public Function GetFileGenerationQueueToRun_PPCCount() As Integer
            Dim dtCnt As New DataTable
            udtDB.RunProc("proc_FileGenerationQueue_get_ToRun_PPCCount", dtCnt)
            Return CInt(dtCnt.Rows(0).Item(0))
        End Function
        ' CRE15-016 (Randomly genereate the valid claim transaction) [End][Winnie]

        '

        Public Function RetrieveDropDownItemBySP(ByVal strSP As String) As DataTable
            Dim dt As New DataTable

            udtDB.RunProc(strSP, dt)

            Return dt

        End Function

        ''' <summary>
        ''' Update [dbo].[FileGenerationQueue] Entry, Mark Start DateTime
        ''' </summary>
        ''' <param name="strGenerationID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function UpdateFileGenerationQueueStart(ByVal strGenerationID As String) As Boolean
            Try
                udtDB.BeginTransaction()

                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Generation_ID", FileGenerationQueueModel.Generation_IDDataType, FileGenerationQueueModel.Generation_IDDataSize, strGenerationID)}

                udtDB.RunProc("proc_FileGenerationQueue_upd_start", prams)

                udtDB.CommitTransaction()

                Return True
            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                Throw eSQL
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw ex
                Return False
            End Try
        End Function

        '''' <summary>
        '''' Update [dbo].[FileGenerationQueue] Entry, Mark Complete DateTime And Complete Status
        '''' </summary>
        '''' <param name="strGenerationID"></param>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Public Function UpdateFileGenerationQueueComplete(ByVal strGenerationID As String) As Boolean
        '    Try
        '        udtDB.BeginTransaction()

        '        Dim prams() As SqlParameter = {udtDB.MakeInParam("@Generation_ID", FileGenerationQueueModel.Generation_IDDataType, FileGenerationQueueModel.Generation_IDDataSize, strGenerationID)}

        '        udtDB.RunProc("proc_FileGenerationQueue_upd_complete", prams)

        '        udtDB.CommitTransaction()

        '        Return True
        '    Catch eSQL As SqlException
        '        udtDB.RollBackTranscation()
        '        Throw eSQL
        '    Catch ex As Exception
        '        udtDB.RollBackTranscation()
        '        Throw ex
        '        Return False
        '    End Try
        'End Function

        ' [CRE12-004] Statistic Enquiry [Start][Tommy Tse]

        Public Function UpdateFileGenerationQueueStart(ByRef udtDB As Database, ByVal strGenerationID As String) As Boolean
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Generation_ID", FileGenerationQueueModel.Generation_IDDataType, FileGenerationQueueModel.Generation_IDDataSize, strGenerationID)}

                udtDB.RunProc("proc_FileGenerationQueue_upd_start", prams)

                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        ' [CRE12-004] Statistic Enquiry [End][Tommy Tse]

        ' CRE15-016 (Randomly genereate the valid claim transaction) [Start][Winnie]
        Public Function UpdateFileGenerationQueueStatus(ByRef udtDB As Database, ByVal strGenerationID As String, ByVal strStatus As String) As Boolean
            Try
                If udtDB Is Nothing Then
                    udtDB = New Common.DataAccess.Database()
                End If

                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Generation_ID", FileGenerationQueueModel.Generation_IDDataType, FileGenerationQueueModel.Generation_IDDataSize, strGenerationID),
                                              udtDB.MakeInParam("@Status", FileGenerationQueueModel.StatusDataType, FileGenerationQueueModel.StatusDataSize, strStatus)}

                udtDB.RunProc("proc_FileGenerationQueue_upd_status", prams)

                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function
        ' CRE15-016 (Randomly genereate the valid claim transaction) [End][Winnie]

        ''' <summary>
        ''' [External] Get User List that have View Access
        ''' (Access_Type = 'A' All or 'D' Download) And Is_SelfAccess = 'N'
        ''' Or Is_SelfAccess='Y' And Request_By User
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <param name="strGenerationID">File Generation ID</param>
        ''' <returns>DataTable [Generation_ID, User_ID]</returns>
        ''' <remarks></remarks>
        Public Function GetViewAccessibleUser(ByRef udtDB As Database, ByVal strGenerationID As String) As DataTable

            Dim dtResult As New DataTable()
            Try

                '[Generation_ID], [User_ID]
                Dim params() As SqlParameter = {udtDB.MakeInParam("@Generation_ID", FileGenerationQueueModel.Generation_IDDataType, FileGenerationQueueModel.Generation_IDDataSize, strGenerationID)}

                udtDB.RunProc("proc_DataDownloadUser_get_ByGenID", params, dtResult)

                Return dtResult

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        '''' <summary>
        '''' [Public] Update [dbo].[FileGenerationQueue].File_Content with "File -> Byte()"
        '''' </summary>
        '''' <param name="strGenerationID"></param>
        '''' <param name="arrByteContent"></param>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Public Function UpdateFileContent(ByVal strGenerationID As String, ByVal arrByteContent As Byte()) As Boolean
        '    Try
        '        udtDB.BeginTransaction()

        '        Dim prams() As SqlParameter = {udtDB.MakeInParam("@Generation_ID", FileGenerationQueueModel.Generation_IDDataType, FileGenerationQueueModel.Generation_IDDataSize, strGenerationID), _
        '                udtDB.MakeInParam("@File_Content", FileGenerationQueueModel.File_ContentDataType, FileGenerationQueueModel.File_ContentDataSize, arrByteContent)}

        '        udtDB.RunProc("proc_FileGenerationQueue_upd_FileContent", prams)

        '        udtDB.CommitTransaction()

        '        Return True
        '    Catch eSQL As SqlException
        '        udtDB.RollBackTranscation()
        '        Throw eSQL
        '    Catch ex As Exception
        '        udtDB.RollBackTranscation()
        '        Throw ex
        '        Return False
        '    End Try
        'End Function

        Public Function UpdateFileContent(ByRef udtDB As Database, ByVal strGenerationID As String, ByVal arrByteContent As Byte()) As Boolean
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Generation_ID", FileGenerationQueueModel.Generation_IDDataType, FileGenerationQueueModel.Generation_IDDataSize, strGenerationID), _
                        udtDB.MakeInParam("@File_Content", FileGenerationQueueModel.File_ContentDataType, FileGenerationQueueModel.File_ContentDataSize, arrByteContent)}

                udtDB.RunProc("proc_FileGenerationQueue_upd_FileContent", prams)

                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        ''' <summary>
        ''' [Public] Retrieve [dbo].[FileGenerationQueue] with [dbo].[FileGenerationQueue].File_Content filled
        ''' </summary>
        ''' <param name="strGenerationID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetFileContent(ByVal strGenerationID As String) As FileGenerationQueueModel

            Dim udtFileGenerationQueueModel As FileGenerationQueueModel = Nothing

            Dim strFileID, strInParam, strOutputFile, strStatus, strFilePassword As String
            Dim strRequestBy, strFileDescription As String
            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Dim dtmRequestDtm, dtmStartDtm, dtmCompleteDtm, dtmScheduleGenDtm As Nullable(Of DateTime)
            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]

            Try
                Dim dtResult As New DataTable()
                '[Generation_ID], [File_ID], [In_Parm], [Output_File], [Status],
                '[File_Password], [Request_Dtm], [Request_By], 
                '[Start_dtm], [Complete_dtm], [File_Description], [Schedule_Gen_Dtm]

                Dim params() As SqlParameter = { _
                    udtDB.MakeInParam("@Generation_ID", FileGenerationQueueModel.Generation_IDDataType, FileGenerationQueueModel.Generation_IDDataSize, strGenerationID)}

                udtDB.RunProc("proc_FileGenerationQueue_get_withFileContent", params, dtResult)

                If dtResult.Rows.Count > 0 Then

                    Dim drRow As DataRow = dtResult.Rows(0)

                    If IsDBNull(drRow("Generation_ID")) Then strGenerationID = Nothing Else strGenerationID = drRow("Generation_ID").ToString().Trim()
                    If IsDBNull(drRow("File_ID")) Then strFileID = Nothing Else strFileID = drRow("File_ID").ToString().Trim()
                    If IsDBNull(drRow("In_Parm")) Then strInParam = Nothing Else strInParam = drRow("In_Parm").ToString().Trim()
                    If IsDBNull(drRow("Output_File")) Then strOutputFile = Nothing Else strOutputFile = drRow("Output_File").ToString().Trim()
                    If IsDBNull(drRow("Status")) Then strStatus = Nothing Else strStatus = drRow("Status").ToString().Trim()
                    If IsDBNull(drRow("File_Password")) Then strFilePassword = Nothing Else strFilePassword = drRow("File_Password").ToString().Trim()

                    If IsDBNull(drRow("Request_Dtm")) Then dtmRequestDtm = Nothing Else dtmRequestDtm = Convert.ToDateTime(drRow("Request_Dtm"))
                    If IsDBNull(drRow("Request_By")) Then strRequestBy = Nothing Else strRequestBy = drRow("Request_By").ToString().Trim()
                    If IsDBNull(drRow("Start_dtm")) Then dtmStartDtm = Nothing Else dtmStartDtm = Convert.ToDateTime(drRow("Start_dtm"))
                    If IsDBNull(drRow("Complete_dtm")) Then dtmCompleteDtm = Nothing Else dtmCompleteDtm = Convert.ToDateTime(drRow("Complete_dtm"))

                    If IsDBNull(drRow("File_Description")) Then strFileDescription = Nothing Else strFileDescription = drRow("File_Description").ToString().Trim()

                    Dim arrByteFileContent As Byte()
                    If IsDBNull(drRow("File_Content")) Then arrByteFileContent = Nothing Else arrByteFileContent = CType(drRow("File_Content"), Byte())

                    ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    If IsDBNull(drRow("Schedule_Gen_Dtm")) Then dtmScheduleGenDtm = Nothing Else dtmScheduleGenDtm = Convert.ToDateTime(drRow("Schedule_Gen_Dtm"))

                    udtFileGenerationQueueModel = New FileGenerationQueueModel(strGenerationID, strFileID, strInParam, strOutputFile, _
                        strStatus, strFilePassword, dtmRequestDtm, strRequestBy, dtmStartDtm, dtmCompleteDtm, strFileDescription, dtmScheduleGenDtm)
                    ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]

                    udtFileGenerationQueueModel.FileContent = arrByteFileContent
                End If

            Catch ex As Exception
                Throw ex
            End Try

            Return udtFileGenerationQueueModel

        End Function

        ''' <summary>
        ''' Get the boolean value whether the user has already submitted to print the same Detailed Payment Analysis Rpt with the same Reimbursement ID
        ''' </summary>
        ''' <param name="strFileID">DPA1: Detailed Payment Analysis Rpt</param>
        ''' <param name="strReimburseID"></param>
        ''' <param name="strUserID">Request_By</param>
        ''' <returns>False: Not submitted yet; True: Already submitted</returns>
        ''' <remarks></remarks>
        Public Function IsFileGenerationQueueDPAByUserIDReimburseIDAlreadySubmitted(ByVal strFileID As String, ByVal strReimburseID As String, ByVal strUserID As String) As Boolean

            Try
                Dim dtResult As New DataTable()

                Dim params() As SqlParameter = { _
                    udtDB.MakeInParam("@File_ID", FileGenerationModel.File_IDDataType, FileGenerationModel.File_IDDataSize, strFileID), _
                    udtDB.MakeInParam("@File_Name", SqlDbType.VarChar, 15, strReimburseID), udtDB.MakeInParam("@User_ID", SqlDbType.VarChar, 20, strUserID)}

                udtDB.RunProc("proc_FileGenerationQueue_get_DPAByUserIDReimburseID", params, dtResult)

                Dim drRow As DataRow = dtResult.Rows(0)

                If CInt(drRow(0)) = 0 Then
                    Return False
                Else
                    Return True
                End If
                'Catch eSQL As SqlException
                '    Throw eSQL
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Private Shared Function ConvertStringToInt(ByVal input As String) As Integer
            Dim result As Integer
            If Not Integer.TryParse(input, result) Then
                result = -1
            End If
            Return result
        End Function

        ' [CRE12-004] Statistic Enquiry [Start][Tommy Tse]

        Public Function AddInboxMessage(ByRef udtDB As Database, ByVal strFileName As String, ByVal strReportName As String, ByRef strMessageID As String) As Boolean
            Dim blnSuccess As Boolean = False
            Try
                ' Insert Message To User For InBox
                Dim udtInboxBLL As New Common.Component.Inbox.InboxBLL()
                Dim udtMessageCollection As Common.Component.Inbox.MessageModelCollection = Nothing
                Dim udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection = Nothing
                ConstructMessageParamaterList(udtDB, udtMessageCollection, udtMessageReaderCollection, strFileName, strReportName, strMessageID)

                If Not IsNothing(udtMessageCollection) AndAlso Not IsNothing(udtMessageReaderCollection) Then
                    udtInboxBLL.AddMessageAndMessageReaderList(udtDB, udtMessageCollection, udtMessageReaderCollection)
                End If
                blnSuccess = True
            Catch ex As Exception
                Throw ex
            End Try
            Return blnSuccess
        End Function

        Public Sub ConstructMessageParamaterList(ByRef udtDB As Common.DataAccess.Database, ByRef udtMessageCollection As Common.Component.Inbox.MessageModelCollection, ByRef udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection, ByVal strFileName As String, ByVal strReportName As String, ByRef strMessageID As String)

            Dim udtHCVUUserBLL As New HCVUUserBLL

            Dim udtFileGenerationBLL As New FileGeneration.FileGenerationBLL()
            Dim udtFileGenerationModel As FileGeneration.FileGenerationModel = udtFileGenerationBLL.RetrieveFileGeneration(udtDB, strReportName)

            Dim udtGeneral As New Common.ComFunction.GeneralFunction()
            Dim udtParamFunction As New Common.ComFunction.ParameterFunction()
            Dim udtFormatter As New Common.Format.Formatter

            Dim dtmCurrent As DateTime = udtGeneral.GetSystemDateTime()

            udtMessageCollection = Nothing
            udtMessageReaderCollection = Nothing


            'Dim udtDB As New Common.DataAccess.Database()
            'Dim dtResult As DataTable = udtFileGenerationBLL.GetViewAccessibleUser(udtDB, Me.m_udtQueue.GenerationID)

            ' This report will not send to any users



            udtMessageCollection = New MessageModelCollection()
            udtMessageReaderCollection = New MessageReaderModelCollection()

            ' Construct Message & MessageReader
            ' One Message, To Mutilple User (Other Case May Mutil Message, Each Message to Single User
            Dim udtMessage As New MessageModel()
            udtMessage.MessageID = udtGeneral.generateInboxMsgID()
            strMessageID = udtMessage.MessageID

            Dim paramsSubject As New ParameterCollection()
            paramsSubject.AddParam("FileName", strFileName)
            'paramsSubject.AddParam("Date", DateTime.Now.ToString("yyyyMMMdd", New System.Globalization.CultureInfo("en-US")))

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'paramsSubject.AddParam("Date", udtFormatter.formatDate(Now.AddDays(-1), "en"))
            paramsSubject.AddParam("Date", udtFormatter.formatDisplayDate(Now.AddDays(-1), CultureLanguage.English))
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]


            Dim paramsContent As New ParameterCollection()
            paramsContent.AddParam("FileName", strFileName)
            paramsContent.AddParam("Link", "../ReportAndDownload/Datadownload.aspx")



            udtMessage.Subject = udtParamFunction.GetParsedStringByparameter(udtFileGenerationModel.MessageSubject, paramsSubject)
            udtMessage.Message = udtParamFunction.GetParsedStringByparameter(udtFileGenerationModel.MessageContent, paramsContent)

            udtMessage.CreateBy = "EHCVS"
            udtMessage.CreateDtm = dtmCurrent
            udtMessageCollection.Add(udtMessage)

            Dim strUserId As String = udtHCVUUserBLL.GetHCVUUser.UserID
            Dim strUserName As String = udtHCVUUserBLL.GetHCVUUser.UserName

            Dim udtMessageReader As New MessageReaderModel()
            udtMessageReader.MessageID = udtMessage.MessageID
            udtMessageReader.MessageReader = strUserId
            udtMessageReader.UpdateBy = "EHCVS"
            udtMessageReader.UpdateDtm = dtmCurrent

            udtMessageReaderCollection.Add(udtMessageReader)

        End Sub

        Public Function AddFileDownload(ByRef udtDB As Database, ByVal strGenerationID As String, ByVal RequestBy As String) As Boolean
            Try

                Dim prams() As SqlClient.SqlParameter = {udtDB.MakeInParam("@generation_id", SqlDbType.Char, 12, strGenerationID), udtDB.MakeInParam("@user_id", SqlDbType.VarChar, 20, RequestBy)}
                udtDB.RunProc("proc_FileDownload_add", prams)

                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        'Export the dataset to excel with the standard template
        Public Function ConstructExcelFile(ByVal dsData As DataSet, ByVal strFilePath As String, ByVal strFileName As String, ByVal strPassword As String, ByVal strTemplate As String, ByVal intStartRow As List(Of Integer)) As Boolean
            Dim thisThread As System.Threading.Thread = System.Threading.Thread.CurrentThread
            Dim originalCulture As System.Globalization.CultureInfo = thisThread.CurrentCulture

            Dim oExcel As Excel.Application = Nothing
            Dim oBooks As Excel.Workbooks = Nothing, oBook As Excel.Workbook = Nothing
            Dim oSheets As Excel.Sheets = Nothing, oSheet As Excel.Worksheet = Nothing
            Dim oCells As Excel.Range = Nothing

            Try
                thisThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

                oExcel = New Excel.Application
                Dim filePath As String
                Dim templatePath As String

                filePath = strFilePath
                filePath = filePath & strFileName

                templatePath = strTemplate
                Dim i As Integer = 1
                Dim iCurrentSheet As Integer = 1

                'copy the template file to a new excel file

                If Not File.Exists(filePath) Then
                    Dim fileOpen As New FileInfo(templatePath)
                    If fileOpen.Exists() Then
                        Dim fileSave As FileInfo = fileOpen.CopyTo(filePath)

                        If Not fileSave.Exists Then
                            Throw New Exception("File Copy Fail!" + fileSave.FullName + "," + fileOpen.FullName)
                        End If

                        oExcel.Visible = False : oExcel.DisplayAlerts = False

                        oBooks = oExcel.Workbooks
                        oBooks.Open(filePath)
                        oBook = oBooks.Item(1)

                        oSheets = oBook.Worksheets


                        For Each dtFormData As DataTable In dsData.Tables

                            oSheet = CType(oSheets.Item(iCurrentSheet), Excel.Worksheet)

                            oCells = DataTable2Excel(dtFormData, oSheet, intStartRow(i - 1), oSheets, iCurrentSheet)


                            'Fill in the data
                            'If i > intStartRow.Length Then
                            '    oCells = DataTable2Excel(dtFormData, oSheet, 5)
                            'Else
                            '    oCells = DataTable2Excel(dtFormData, oSheet, intStartRow(i - 1))
                            'End If

                            i = i + 1
                        Next


                        'oSheet.SaveAs(filePath)
                        oBook.SaveAs(filePath)

                        oBook.Close()

                        'Quit Excel and thoroughly deallocate everything
                        oExcel.Quit()

                        ReleaseComObject(oCells) : ReleaseComObject(oSheet)
                        ReleaseComObject(oSheets) : ReleaseComObject(oBook)
                        ReleaseComObject(oBooks) : ReleaseComObject(oExcel)

                        oExcel = Nothing : oBooks = Nothing : oBook = Nothing
                        oSheets = Nothing : oSheet = Nothing : oCells = Nothing

                        System.GC.Collect()

                    Else
                        Throw New Exception("Template Not Found!" + fileOpen.FullName)
                    End If
                End If

                'Dim Copyfile As New System.IO.FileInfo(filePath)
                Return True
            Catch ex As Exception
                'Throw ex
                Throw
                'Return False
            Finally
                thisThread.CurrentCulture = originalCulture

                If oBook IsNot Nothing Then
                    Try
                        oBook.Close()
                    Catch ex2 As Exception
                        ' Do nothing
                    End Try
                End If

                'Quit Excel and thoroughly deallocate everything
                If oExcel IsNot Nothing Then
                    Try
                        oExcel.Quit()
                    Catch ex2 As Exception
                        ' Do nothing
                    End Try
                End If

                Try
                    If oCells IsNot Nothing Then ReleaseComObject(oCells)
                    If oSheet IsNot Nothing Then ReleaseComObject(oSheet)
                    If oSheets IsNot Nothing Then ReleaseComObject(oSheets)
                    If oBook IsNot Nothing Then ReleaseComObject(oBook)
                    If oBooks IsNot Nothing Then ReleaseComObject(oBooks)
                    If oExcel IsNot Nothing Then ReleaseComObject(oExcel)

                    oExcel = Nothing : oBooks = Nothing : oBook = Nothing
                    oSheets = Nothing : oSheet = Nothing : oCells = Nothing

                    System.GC.Collect()
                Catch ex As Exception
                    ' Do nothing
                End Try
            End Try
        End Function

        'Outputs a DataTable to an Excel Worksheet
        Public Function DataTable2Excel(ByVal dt As DataTable, ByRef oSheet As Excel.Worksheet, ByVal intStartRow As Integer, ByRef oSheets As Excel.Sheets, ByRef iCurrentSheet As Integer) As Excel.Range
            Dim oCells As Excel.Range
            oCells = oSheet.Cells

            Dim dr As DataRow, ary() As Object
            Dim iRow As Integer, iCol As Integer

            ' CRE13-016 - Upgrade to excel 2007 [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            'Dim iBreakAtRow As Integer = 65500
            Dim iBreakAtRow As Integer = GetExcelWorkSheetMaxRow()
            ' CRE13-016 - Upgrade to excel 2007 [End][Tommy L]

            Dim iSheetAdd As Integer = 0

            If dt.Rows.Count > iBreakAtRow Then
                iSheetAdd = (dt.Rows.Count \ iBreakAtRow)
            End If

            ' Add Sheet
            Dim iSheet As Integer = 0
            Dim strSheetName = oSheet.Name
            If iSheetAdd > 0 Then
                For iSheet = 1 To iSheetAdd
                    oSheet.Copy(oSheet)
                Next
            End If

            'Output Data
            If iSheetAdd > 0 Then
                Dim iCurrentStartRow As Integer = 0
                Dim iCurrentEndRow As Integer = 0


                For iSheet = 0 To iSheetAdd
                    Dim oTempSheet As Excel.Worksheet
                    Dim oTempCells As Excel.Range
                    iCurrentStartRow = iSheet * iBreakAtRow
                    iCurrentEndRow = ((iSheet + 1) * iBreakAtRow) - 1
                    oTempSheet = CType(oSheets.Item(iCurrentSheet + iSheet), Excel.Worksheet)
                    oTempSheet.Name = strSheetName + "-" + (iSheet + 1).ToString()
                    oTempCells = oTempSheet.Cells

                    If iCurrentEndRow > dt.Rows.Count Then
                        iCurrentEndRow = dt.Rows.Count - 1
                    End If

                    Dim iWritingRow As Integer = 0
                    iWritingRow = iWritingRow + intStartRow
                    For iRow = iCurrentStartRow To iCurrentEndRow
                        dr = dt.Rows.Item(iRow)
                        ary = dr.ItemArray
                        For iCol = 0 To UBound(ary)
                            ' INT12-003 ExcelGenerator tamplate report date format incorrect [Start][Koala]
                            ' -----------------------------------------------------------------------------------------
                            If TypeOf ary(iCol) Is DateTime Then
                                oTempCells(iWritingRow, iCol + 1) = CType(ary(iCol), DateTime).ToString(_excelTemplateDate)
                            Else
                                oTempCells(iWritingRow, iCol + 1) = ary(iCol).ToString
                            End If
                            ' INT12-003 ExcelGenerator tamplate report date format incorrect [End][Koala]
                        Next
                        iWritingRow = iWritingRow + 1
                    Next
                Next
            Else
                For iRow = 0 To dt.Rows.Count - 1
                    dr = dt.Rows.Item(iRow)
                    ary = dr.ItemArray

                    For iCol = 0 To UBound(ary)
                        ' INT12-003 ExcelGenerator tamplate report date format incorrect [Start][Koala]
                        ' -----------------------------------------------------------------------------------------
                        If TypeOf ary(iCol) Is DateTime Then
                            oCells(iRow + intStartRow, iCol + 1) = CType(ary(iCol), DateTime).ToString(_excelTemplateDate)
                        Else
                            oCells(iRow + intStartRow, iCol + 1) = ary(iCol).ToString
                        End If
                        ' INT12-003 ExcelGenerator tamplate report date format incorrect [End][Koala]
                    Next
                Next
            End If

            iCurrentSheet = iCurrentSheet + iSheetAdd + 1

            Return oCells
        End Function

        Public Function Export(ByVal strFileID As String, ByVal ds As DataSet, ByRef strGenerationID As String, ByRef strMessageID As String, Optional ByVal strReimburseID As String = "", Optional ByVal strSchemeCode As String = "") As Boolean

            Dim udtDB As New Common.DataAccess.Database()
            Dim udtGeneralFunction As New GeneralFunction
            Dim strStatisticsTemplateFolder As String = String.Empty
            Dim strFolderPath As String = String.Empty
            udtGeneralFunction.getSystemParameter("StatisticsTemplateFolder", strStatisticsTemplateFolder, String.Empty)
            udtGeneralFunction.getSystemParameter("ExcelWithTemplateDownloadStoragePath", strFolderPath, String.Empty)

            Dim blnSuccess As Boolean = True

            Try

                Dim udtFileGenerationBLL As New FileGeneration.FileGenerationBLL()
                Dim udtFileGenerationModel As FileGeneration.FileGenerationModel = udtFileGenerationBLL.RetrieveFileGeneration(udtDB, strFileID)
                Dim udtCommon As New Common.ComFunction.GeneralFunction()
                Dim udtHCVUUserBLL As New HCVUUserBLL
                Dim udtQueue As FileGenerationQueueModel = New FileGenerationQueueModel

                strGenerationID = udtCommon.generateFileSeqNo()
                udtQueue.GenerationID = strGenerationID
                udtQueue.FileID = strFileID
                udtQueue.InParm = ""
                ' CRE17-004 Generate a new DPAR on EHCP basis [Start][Dickson]
                If strReimburseID = "" And strSchemeCode = "" Then
                    udtQueue.OutputFile = udtFileGenerationModel.FileNameWithDateTimeStamp
                Else
                    udtQueue.OutputFile = udtFileGenerationModel.FileNamePrefix + strReimburseID + "-" + strSchemeCode + "-PA" + Year(DateTime.Now).ToString().Substring(2, 2) + Month(DateTime.Now).ToString().PadLeft(2, "0") + Day(DateTime.Now).ToString().PadLeft(2, "0") + Hour(DateTime.Now).ToString().PadLeft(2, "0") + Minute(DateTime.Now).ToString().PadLeft(2, "0") + (New Common.Format.Formatter).FormatFileExt(udtFileGenerationModel.FileType)
                End If
                ' udtQueue.OutputFile = udtFileGenerationModel.FileNameWithDateTimeStamp
                ' CRE17-004 Generate a new DPAR on EHCP basis [End][Dickson]
                udtQueue.Status = Common.Component.DataDownloadStatus.Pending
                udtQueue.FilePassword = ""
                udtQueue.RequestDtm = Now
                udtQueue.RequestBy = udtHCVUUserBLL.GetHCVUUser.UserID
                udtQueue.FileDescription = udtFileGenerationModel.FileDesc + "-" + udtQueue.OutputFile
                ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                udtQueue.ScheduleGenDtm = Nothing
                ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]

                ClearTempFolder(strFolderPath, 15)

                'Generate output file
                If blnSuccess Then
                    blnSuccess = udtFileGenerationBLL.ConstructExcelFile(ds, strFolderPath, udtQueue.OutputFile, udtQueue.FilePassword, strStatisticsTemplateFolder + udtFileGenerationModel.ReportTemplate, udtFileGenerationModel.XLS_Parameter)
                End If

                udtDB.BeginTransaction()

                'Add record to table FileGenerationQueue
                If blnSuccess Then
                    blnSuccess = udtFileGenerationBLL.AddFileGenerationQueue(udtDB, udtQueue)
                End If

                'Update record in table FileGenerationQueue
                If blnSuccess Then
                    blnSuccess = udtFileGenerationBLL.UpdateFileGenerationQueueStart(udtDB, udtQueue.GenerationID)
                End If

                ' Save output file Into File Database
                If blnSuccess Then
                    udtQueue.FileContent = System.IO.File.ReadAllBytes(strFolderPath + udtQueue.OutputFile)
                    blnSuccess = udtFileGenerationBLL.UpdateFileContent(udtDB, udtQueue.GenerationID, udtQueue.FileContent)
                End If

                'Add record to table FileDownloads
                If blnSuccess Then
                    blnSuccess = udtFileGenerationBLL.AddFileDownload(udtDB, udtQueue.GenerationID, udtQueue.RequestBy)
                End If

                'Add Inbox Message
                If blnSuccess Then
                    blnSuccess = udtFileGenerationBLL.AddInboxMessage(udtDB, udtQueue.OutputFile, udtQueue.FileID, strMessageID)
                End If

                'Update record in table FileGenerationQueue
                If blnSuccess Then
                    blnSuccess = udtFileGenerationBLL.UpdateFileGenerationQueueStatus(udtDB, udtQueue.GenerationID, FileGenerationQueueStatus.Completed)
                End If

                'Show popup for File Download redirection
                If blnSuccess Then
                    udtDB.CommitTransaction()
                Else
                    udtDB.RollBackTranscation()
                End If

            Catch ex As Exception
                blnSuccess = False
                udtDB.RollBackTranscation()
                Throw
            Finally
                ClearTempFolder(strFolderPath, 15)
            End Try

            Return blnSuccess
        End Function

        Sub ClearTempFolder(ByVal strFolderPath As String, ByVal intMinute As String)
            Try
                Dim dtLastModified As Date
                For Each strFilePath As String In My.Computer.FileSystem.GetFiles(strFolderPath)
                    dtLastModified = System.IO.File.GetLastWriteTime(strFilePath)
                    If DateDiff(DateInterval.Minute, dtLastModified, Now) > intMinute Then
                        My.Computer.FileSystem.DeleteFile(strFilePath, FileIO.UIOption.AllDialogs, FileIO.RecycleOption.DeletePermanently)
                    End If
                Next

                For Each strDirectoryPath As String In My.Computer.FileSystem.GetDirectories(strFolderPath)
                    dtLastModified = System.IO.File.GetLastWriteTime(strDirectoryPath)
                    If DateDiff(DateInterval.Minute, dtLastModified, Now) > intMinute Then
                        My.Computer.FileSystem.DeleteDirectory(strDirectoryPath, FileIO.UIOption.AllDialogs, FileIO.RecycleOption.DeletePermanently)
                    End If
                Next
            Catch ex As Exception

            End Try
        End Sub

        ' [CRE12-004] Statistic Enquiry [End][Tommy Tse]

        ' CRE13-016 - Upgrade to excel 2007 [Start][Karl L]
        Public Function GetExcelWorkSheetMaxRow() As Integer
            Dim udtGeneralFunction As New GeneralFunction
            Dim strExcelWorkSheetMaxRow As String = String.Empty

            Call udtGeneralFunction.getSystemParameter("ExcelWorkSheetMaxRow", strExcelWorkSheetMaxRow, String.Empty)

            If String.IsNullOrEmpty(strExcelWorkSheetMaxRow) = False Then
                Return CInt(strExcelWorkSheetMaxRow)
            Else
                Throw New Exception("FileGenerationBLL: Parameter [ExcelWorkSheetMaxRow] is empty ")
            End If

        End Function

        ' CRE13-016 - Upgrade to excel 2007 [Start][Karl L]

#Region "FileGenerationQueueAdditionalParameter"

        Public Sub AddFileGenerationAdditionalParameter(udtDB As Database, strGenerationID As String, strParmName As String, intParmSeq As Integer, strParmValue As String)
            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Generation_ID", SqlDbType.Char, 12, strGenerationID), _
                udtDB.MakeInParam("@Parm_Name", SqlDbType.VarChar, 30, strParmName), _
                udtDB.MakeInParam("@Parm_Seq", SqlDbType.Int, 8, intParmSeq), _
                udtDB.MakeInParam("@Parm_Value", SqlDbType.VarChar, 100, strParmValue) _
            }

            udtDB.RunProc("proc_FileGenerationQueueAdditionalParameter_add", prams)

        End Sub

#End Region

    End Class
End Namespace
