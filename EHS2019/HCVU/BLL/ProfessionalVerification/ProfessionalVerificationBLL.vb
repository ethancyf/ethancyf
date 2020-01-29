Imports System.Data
Imports System.Data.SqlClient
Imports Common.DataAccess

Imports Common.Component
Imports Common.Component.StaticData

' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

' -----------------------------------------------------------------------------------------

Imports Common.Component.Profession

' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

Imports Common.Component.Professional
Imports Common.Component.ServiceProvider

Imports Common.ComObject
Imports Common.ComFunction.ParameterFunction


''' <summary>
''' Business Layer For [ServiceProvider--Health Profession Verification]
''' </summary>
''' <remarks></remarks>
''' 

Public Class ProfessionalVerificationBLL

#Region "StoreProc List"

    Private strSP_SearchProfessionVerificationToExport As String = "proc_ProfessionalVerification_get_ToExport"
    Private strSP_SearchProfessionVerificationToExportByProfessionCode As String = "proc_ProfessionalVerification_get_ToExportByProCode"
    Private strSP_SearchProfessionVerificationToVerify As String = "proc_ProfessionalVerification_get_ForVerify"

    Private strSP_SearchProfessionViewResult As String = "proc_ProfessionalSubmissionViewResult_get_ByFile"


    Private strSP_AddProfessionalVerification As String = "proc_ProfessionalVerification_add"

    Private strSP_UpdateProfessionalVerificationForCancelExport As String = "proc_ProfessionalVerification_upd_CancelExport"
    Private strSP_UpdateProfessionalVerificationForExport As String = "proc_ProfessionalVerification_upd_Export"
    Private strSP_UpdateProfessionalVerificationForImport As String = "proc_ProfessionalVerification_upd_Import"
    Private strSP_UpdateProfessionalVerificationForDefer As String = "proc_ProfessionalVerification_upd_Defer"
    Private strSP_UpdateProfessionalVerificationForAccept As String = "proc_ProfessionalVerification_upd_Accept"
    Private strSP_UpdateProfessionalVerificationForReject As String = "proc_ProfessionalVerification_upd_reject"
    Private strSP_UpdateProfessionalVerificationStatus As String = "proc_ProfessionalVerification_upd_status"

    Private strSP_DeleteProfessionalVerification As String = "proc_ProfessionalVerification_del"

    Private strSP_GetProfessionalVerificationByEnrolmentRefNo As String = "proc_ProfessionalVerification_get_byERN"
    Private strSP_GetProfessionalVerificationByKey As String = "proc_ProfessionalVerification_get_ByKey"

    Private strSP_GetSubmissionHeader As String = "proc_ProfessionalSubmissionHeader_get"
    Private strSP_SearchSubmissionHeader As String = "proc_ProfessionalSubmissionHeader_get_search"
    Private strSP_GetSubmissionHeaderByKey As String = "proc_ProfessionalSubmissionHeader_get_ByKey"
    Private strSP_AddSubmissionHeader As String = "proc_ProfessionalSubmissionHeader_add"
    Private strSP_UpdateSubmissionHeaderForImport As String = "proc_ProfessionalSubmissionHeader_upd_Import"
    Private strSP_UpdateSubmissionHeaderMarkDelete As String = "proc_ProfessionalSubmissionHeader_upd_markdelete"

    Private strSP_GetSubmissionRecord As String = "proc_ProfessionalSubmission_get"
    Private strSP_AddSubmissionRecord As String = "proc_ProfessionalSubmission_add"
    Private strSP_DeleteSubmissionRecord As String = "proc_ProfessionalSubmission_del"

    Private strSP_ReplaceSubmissionResult As String = "proc_ProfessionalSubmissionResult_replace"

#End Region

#Region "Variables"

    Public Shared ReadOnly st_strAllProfessionCode As String = "*"

    Private Const _strProfessionCode As String = "PROFESSION"
    Private Const _strProfessionCodeTextField As String = "DataValue"
    Private Const _strProfessionCodeValueField As String = "ItemNo"

#End Region

#Region "Declaration"

    ' Database Object
    Private udtDB As New Common.DataAccess.Database()
    Private udtFormatter As New Common.Format.Formatter()

#End Region

#Region "Constructor"

    Public Sub New()

    End Sub

#End Region

#Region "Export For Verification [Pend Record]"

    ''' <summary>
    ''' [Provide For UI] Retrieve Profession Verify Record To Be Export
    ''' </summary>
    ''' <returns>DataTable[RecordNum,EnrolmentNum,HKID,SPName(EngName,ChiName),Profession,RegistrationCode]</returns>
    ''' <remarks>[RecordNum,EnrolmentNum,HKID, SPName(EngName,ChiName),Profession,RegistrationCode]
    ''' </remarks>
    Public Function SearchProfessionVerifyRecordToBeExport() As DataTable

        Dim dtRaw As New DataTable()

        Try
            ' Record from StoreProc 
            ' [Enrolment_Ref_No], [Professional_Seq], [SP_ID], [Service_Category_Code]
            ' [Registration_Code], [Profession_Description], [SP_HKID], [SP_Eng_Name], [SP_Chi_Name], [TSMP]
            udtDB.RunProc(Me.strSP_SearchProfessionVerificationToExport, dtRaw)
        Catch ex As Exception
            Throw ex
        End Try

        Return Me.ProcessUIVerifyRecordByDataTable(dtRaw)

    End Function

    ''' <summary>
    ''' [Provide For UI] Retrieve Profession Verify Record To Be Export By Profession Code
    ''' </summary>
    ''' <param name="strProfessionCode">Profession Code</param>
    ''' <returns>DataTable[RecordNum,EnrolmentNum,HKID,SPName(EngName,ChiName),Profession,RegistrationCode]</returns>
    ''' <remarks></remarks>
    Public Function SearchProfessionVerifyRecordToBeExportByProfessionCode(ByVal strProfessionCode As String) As DataTable

        Dim dtRaw As New DataTable()

        Dim params() As SqlParameter = {udtDB.MakeInParam("@Profession_Code", SqlDbType.Char, 5, strProfessionCode)}

        Try
            ' Record from StoreProc 
            ' [Enrolment_Ref_No], [Professional_Seq], [SP_ID], [Service_Category_Code]
            ' [Registration_Code], [Profession_Description], [SP_HKID], [SP_Eng_Name], [SP_Chi_Name], [TSMP]

            udtDB.RunProc(Me.strSP_SearchProfessionVerificationToExportByProfessionCode, params, dtRaw)
        Catch ex As Exception
            Throw ex
        End Try

        Return Me.ProcessUIVerifyRecordByDataTable(dtRaw)

    End Function

    ''' <summary>
    ''' [Private] For UI Search Verify Record To Be Export Only
    ''' </summary>
    ''' <param name="dtRaw"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ProcessUIVerifyRecordByDataTable(ByVal dtRaw As DataTable) As DataTable

        ' Record from StoreProc 
        ' [Enrolment_Ref_No], [Professional_Seq], [SP_ID], [Service_Category_Code]
        ' [Registration_Code], [Profession_Description], [SP_HKID], [SP_Eng_Name], [SP_Chi_Name], [TSMP]

        Dim strRecordNum As String = "RecordNum"
        Dim strEnrolmentNum As String = "EnrolmentNum"
        Dim strProfessionalSeq As String = "ProfessionalSeq"
        Dim strHKID As String = "HKID"
        Dim strSPEngName As String = "SPEngName"
        Dim strSPChiName As String = "SPChiName"
        Dim strProfession As String = "Profession"
        Dim strRegistrationCode As String = "RegistrationCode"
        Dim strProfessionCode As String = "ProfessionCode"
        Dim strTSMP As String = "TSMP"

        Dim dtResult As New DataTable()
        Dim drResult As DataRow
        dtResult.Columns.Add(New DataColumn(strRecordNum, GetType(Integer)))
        dtResult.Columns.Add(New DataColumn(strProfessionalSeq, GetType(Integer)))
        dtResult.Columns.Add(New DataColumn(strEnrolmentNum, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strHKID, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strSPEngName, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strSPChiName, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strProfession, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strRegistrationCode, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strProfessionCode, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strTSMP, GetType(Byte())))

        Dim i As Integer = 0
        For Each drRaw As DataRow In dtRaw.Rows
            i = i + 1
            drResult = dtResult.NewRow()
            drResult.BeginEdit()
            drResult(strRecordNum) = i
            drResult(strEnrolmentNum) = Me.udtFormatter.formatSystemNumber(drRaw("Enrolment_Ref_No").ToString().Trim())
            drResult(strProfessionalSeq) = Convert.ToInt32(drRaw("Professional_Seq"))
            drResult(strHKID) = Me.udtFormatter.formatHKID(drRaw("SP_HKID").ToString(), False)

            drResult(strSPEngName) = drRaw("SP_Eng_Name")
            If drRaw.IsNull("SP_Chi_Name") Then
                drResult(strSPChiName) = drRaw("SP_Chi_Name")
            Else
                drResult(strSPChiName) = Me.udtFormatter.formatChineseName(drRaw("SP_Chi_Name"))
            End If


            drResult(strProfession) = drRaw("Profession_Description")
            drResult(strRegistrationCode) = drRaw("Registration_Code")
            drResult(strProfessionCode) = drRaw("Service_Category_Code")

            drResult(strTSMP) = drRaw("TSMP")

            drResult.EndEdit()
            dtResult.Rows.Add(drResult)
        Next
        dtResult.AcceptChanges()

        Return dtResult
    End Function

    ''' <summary>
    ''' [Provide For UI] Export File 
    ''' </summary>
    ''' <param name="strProfessionCode">* for All</param>
    ''' <param name="strUserID"></param>
    ''' <returns>Export File Name List: Empty for no file Exported</returns>
    ''' <remarks></remarks>
    Public Function ExportFile(ByVal strProfessionCode As String, ByVal dtPend As DataTable, ByVal strUserID As String) As String

        Dim strFileList As String = ""
        Dim strFileName As String = ""
        Dim blnErrorOccur As Boolean = False
        Dim blnExportAll As Boolean = False
        Dim arrListCode As New ArrayList()

        If strProfessionCode Is Nothing OrElse strProfessionCode.Trim() = "" Then
            Throw New ArgumentException("Professional Code Missing")
        End If

        If strProfessionCode.Trim() = st_strAllProfessionCode Then            
            blnExportAll = True
            arrListCode = Me.RetrieveAllProfessionCode()
        End If

        Try
            udtDB.BeginTransaction()

            If blnExportAll Then
                ' Export All Profession One By One
                For Each strTmpProCode As String In arrListCode

                    Dim dsPend As New DataSet()
                    dsPend.Merge(dtPend.Select("ProfessionCode='" & strTmpProCode.Trim() & "'"))

                    If dsPend.Tables.Count > 0 AndAlso dsPend.Tables(0).Rows.Count > 0 Then

                        strFileName = Me.ExportFileByProfession(strTmpProCode, dsPend.Tables(0), strUserID)

                        If strFileName.Trim() <> "" Then
                            If strFileList.Trim() = "" Then
                                strFileList = strFileName.Trim()
                            Else
                                strFileList = strFileList + "," + strFileName.Trim()
                            End If
                        End If
                    End If
                Next
            Else
                ' Export Selected Profession Code
                strFileName = Me.ExportFileByProfession(strProfessionCode, dtPend, strUserID)
                strFileList = strFileName.Trim()
            End If

            udtDB.CommitTransaction()

        Catch exArg As ArgumentException
            blnErrorOccur = True
            udtDB.RollBackTranscation()
            Throw exArg
        Catch ex As Exception
            blnErrorOccur = True
            udtDB.RollBackTranscation()
            Throw ex
        End Try

        Return strFileList

    End Function

    ''' <summary>
    ''' Export Single File By ProfessionCode (Submission Header)
    ''' </summary>
    ''' <param name="strProfessionCode"></param>
    ''' <param name="dtPend">Pending Record</param>
    ''' <param name="strUserID"></param>
    ''' <returns>Export File Name: Empty for no file Exported</returns>
    ''' <remarks></remarks>
    Private Function ExportFileByProfession(ByVal strProfessionCode As String, ByVal dtPend As DataTable, ByVal strUserID As String) As String

        'Dim strRecordNum As String = "RecordNum"
        'Dim strEnrolmentNum As String = "EnrolmentNum"
        'Dim strProfessionalSeq As String = "ProfessionalSeq"
        'Dim strHKID As String = "HKID"
        'Dim strSPEngName As String = "SPEngName"
        'Dim strSPChiName As String = "SPChiName"
        'Dim strProfession As String = "Profession"
        'Dim strRegistrationCode As String = "RegistrationCode"
        'Dim strProfessionCode As String = "ProfessionCode"
        'Dim strTSMP As String = "TSMP"

        Dim strFileName As String = ""
        Dim dtmExport As DateTime = DateTime.MinValue

        If strProfessionCode Is Nothing OrElse strProfessionCode.Trim() = "" Then
            Throw New ArgumentException("Professional Code Missing")
        End If

        ' Insert File Submission Header [dbo].[ProfessionalSubmissionHeader]
        Me.InsertProfessionalSubmissionHeader(strProfessionCode, strUserID, strFileName, dtmExport)

        If strFileName.Trim() <> "" AndAlso dtmExport <> DateTime.MinValue Then

            ' Insert Record to [dbo].[ProfessionalSubmission]
            ' Update [dbo].[ProfessionalVerification]

            Dim counter As Integer = 0
            For Each drRecord As DataRow In dtPend.Rows

                counter = counter + 1
                Dim strERN As String = Me.udtFormatter.formatSystemNumberReverse(drRecord("EnrolmentNum").ToString().Trim())
                Dim intSeq As Integer = Convert.ToInt32(drRecord("ProfessionalSeq"))
                Dim strReferenceNo As String = udtFormatter.ConcatProfessionalReferenceNo(strERN, intSeq)

                Dim strRegCode As String = drRecord("RegistrationCode").ToString().Trim()
                Dim strHKID As String = Me.udtFormatter.formatHKIDInternal(drRecord("HKID").ToString().Trim())
                Dim strName As String = drRecord("SPEngName").ToString().Trim()

                Dim strSurName As String = ""
                Dim strOtherName As String = ""
                Me.udtFormatter.seperateEName(strName, strSurName, strOtherName)

                Dim arrByteTSMP As Byte() = CType(drRecord("TSMP"), Byte())

                ' Insert File Entries [dbo].[ProfessionalSubmission]
                Me.InsertProfessionalSubmissionRecord(strFileName, strReferenceNo, counter, strRegCode, strHKID, strSurName, strOtherName)

                ' Update [dbo].[ProfessionalVerification]
                Me.UpdateProfessionalVerificationExport(strUserID, dtmExport, strERN, intSeq, arrByteTSMP)

            Next

            ' ----------------------------------------------------------------------------------
            ' Add FileGenerationQueue Record
            Dim udtFileGenerationBLL As New FileGeneration.FileGenerationBLL()
            Dim udtFileGenerationQueueModel As New FileGeneration.FileGenerationQueueModel()

            Dim udtFileGenerationModel As FileGeneration.FileGenerationModel = udtFileGenerationBLL.RetrieveFileGeneration(udtDB, Common.Component.DataDownloadFileID.BoardAndCouncil)

            Dim udtParamFunction As New Common.ComFunction.ParameterFunction()
            Dim udtCommon As New Common.ComFunction.GeneralFunction()

            Dim udtSpParamCollection As New StoreProcParamCollection()
            udtSpParamCollection.AddParam("@File_Name", ProfessionalSubmissionModel.File_NameDataType, ProfessionalSubmissionModel.File_NameDataSize, strFileName)

            udtFileGenerationQueueModel.GenerationID = udtCommon.generateFileSeqNo()
            udtFileGenerationQueueModel.FileID = Common.Component.DataDownloadFileID.BoardAndCouncil
            udtFileGenerationQueueModel.InParm = udtParamFunction.GetSPParamString(udtSpParamCollection)
            udtFileGenerationQueueModel.OutputFile = udtCommon.generateFileOutputPath(strFileName)
            udtFileGenerationQueueModel.Status = Common.Component.DataDownloadStatus.Pending
            udtFileGenerationQueueModel.FilePassword = udtCommon.generateSystemPassword(strUserID)
            udtFileGenerationQueueModel.RequestBy = strUserID
            udtFileGenerationQueueModel.FileDescription = udtFileGenerationModel.FileName + "-" + strFileName
            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            udtFileGenerationQueueModel.ScheduleGenDtm = Nothing
            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]

            udtFileGenerationBLL.AddFileGenerationQueue(udtDB, udtFileGenerationQueueModel)

            Dim udtDataDownloadBLL As New DatadownloadBLL()
            udtDataDownloadBLL.InsertFileDownloadRecordsToUsersForFileGeneration(udtDB, udtFileGenerationQueueModel.GenerationID)

        End If

        Return strFileName

    End Function

    ''' <summary>
    ''' Retrieve Profession Verify Record To Be Export By Profession Code
    ''' </summary>
    ''' <param name="strProfessionCode"></param>
    ''' <returns>DataTable[Enrolment_Ref_No,Professional_Seq,Service_Category_Code,Registration_Code,SP_HKID,SP_Eng_Name]</returns>
    ''' <remarks>[Enrolment_Ref_No,Professional_Seq,Service_Category_Code,Registration_Code,SP_HKID,SP_Eng_Name]</remarks>
    Private Function RetrieveProfessionVerifyRecordToBeExportByProfessionCode(ByVal strProfessionCode As String) As DataTable

        Dim dtResult As New DataTable

        Dim params() As SqlParameter = {udtDB.MakeInParam("@Profession_Code", SqlDbType.Char, 5, strProfessionCode)}

        Try
            udtDB.RunProc(Me.strSP_SearchProfessionVerificationToExportByProfessionCode, params, dtResult)
        Catch ex As Exception
            Throw ex
        End Try

        ' Record from StoreProc 
        ' [Enrolment_Ref_No], [Professional_Seq], [Service_Category_Code],
        ' [Registration_Code], [SP_HKID], [SP_Eng_Name], [TSMP]

        dtResult.AcceptChanges()

        Return dtResult
    End Function

    Public Function GetProfessionalVerificationRowCountToBeExport() As Integer

        Dim dtResult As DataTable = New DataTable
        Dim intRes As Integer = 0
        Try

            udtDB.RunProc("proc_ProfessionalVerificationRowCount_ToExport", dtResult)

            If dtResult.Rows(0)(0) > 0 Then
                intRes = CInt(dtResult.Rows(0)(0))
            End If
            Return intRes
        Catch ex As Exception
            Throw ex
        End Try

    End Function
#End Region

#Region "Export List & Import Result [Import File]"

    ''' <summary>
    ''' [Provide For UI] Import File
    ''' </summary>
    ''' <param name="strFileName"></param>
    ''' <param name="dtImport"></param>
    ''' <param name="strUserID"></param>
    ''' <returns>True for Success</returns>
    ''' <remarks></remarks>
    Public Function ImportFile(ByVal strFileName As String, ByVal dtImport As DataTable, ByVal strUserID As String, ByVal arrByteFileContent As Byte()) As Boolean

        ' 1. Update ProfessionalSubmissionHeader (Only Latest Import Dtm + Import User Stored)
        ' (Newly Add) Save the Import File to Professional Submission Header
        ' 2. Insert / Update ProfessionalSubmissionResult
        ' 3. Update ProfessionalVerification

        Dim dtmImportDtm As DateTime
        Try
            Me.udtDB.BeginTransaction()

            ' Update Professional SubmissionHeader
            ' Get dtImportDtm From Database GetDate()
            Me.UpdateProfessionalSubmissionHeader(strFileName, strUserID, dtmImportDtm)           

            For Each drImport As DataRow In dtImport.Rows
                ' Import Result 
                Dim strReference As String = drImport(0).ToString().Trim()
                Dim strResult As String = drImport(1).ToString().Trim()
                Dim strRemark As String = drImport(2).ToString().Trim()

                Dim strEnrolRefNo As String = ""
                Dim intProfSeq As Integer = -1
                Dim udtFormatter As New Common.Format.Formatter()
                udtFormatter.SplitProfessionalReferenceNo(strReference, strEnrolRefNo, intProfSeq)

                ' Retrieve ProfessionalVerification Record to Check:

                Dim udtProfessionalVerificationModel As ProfessionalVerificationModel = Me.GetProfessionalVerification(strEnrolRefNo, intProfSeq)

                If Not udtProfessionalVerificationModel Is Nothing Then
                    ' The Record is still be valid (eg. SPAccountUpdate Status = 'P')
                    If Not udtProfessionalVerificationModel.ExportBy Is Nothing And udtProfessionalVerificationModel.ExportDtm.HasValue Then
                        ' The Record Exported

                        'Newly Export: [ProfessionalVerification].Export_By, Export_Dtm IS NOT NULL,  Import_By, Import_Dtm IS Null
                        If udtProfessionalVerificationModel.ImportBy Is Nothing And Not udtProfessionalVerificationModel.ImportDtm.HasValue And udtProfessionalVerificationModel.RecordStatus.Trim() = Common.Component.ProfessionalVerificationRecordStatus.Export Then
                            ' Newly Import : Status From 'O' To I
                            Me.UpdateProfessionalVerificationImport(strEnrolRefNo, intProfSeq, strUserID, dtmImportDtm, strResult, strRemark, Common.Component.ProfessionalVerificationRecordStatus.Import, udtProfessionalVerificationModel.TSMP)

                            Me.ReplaceProfessionalSubmissionResult(strFileName, strReference, strResult, strRemark)

                        ElseIf Not udtProfessionalVerificationModel.ImportBy Is Nothing And udtProfessionalVerificationModel.ImportDtm.HasValue Then
                            '' Re-Import: [ProfessionalVerification].Export_By, Export_Dtm,Import_By, Import_Dtm IS NOT NULL
                            '' Confirm By, Confirm Dtm, Void_By, Void_Dtm = null
                            'If udtProfessionalVerificationModel.ConfirmBy Is Nothing And Not udtProfessionalVerificationModel.ConfirmDtm.HasValue And udtProfessionalVerificationModel.VoidBy Is Nothing And Not udtProfessionalVerificationModel.VoidDtm.HasValue Then
                            '    ' 2.2 Re-Import : Status Unchange
                            '    Me.UpdateProfessionalVerificationImport(strEnrolRefNo, intProfSeq, strUserID, dtmImportDtm, strResult, strRemark, "", udtProfessionalVerificationModel.TSMP)
                            '    Me.ReplaceProfessionalSubmissionResult(strFileName, strReference, strResult, strRemark)
                            'End If

                        End If
                    End If
                Else
                    ' Still Save The Submission Result
                    Me.ReplaceProfessionalSubmissionResult(strFileName, strReference, strResult, strRemark)
                End If
            Next

            Me.UpdateProfessionalSubmissionHeaderFileContent(strFileName, arrByteFileContent)

            Me.udtDB.CommitTransaction()
        Catch ex As Exception
            Me.udtDB.RollBackTranscation()
            Throw ex
            Return False
        End Try

        Return True

    End Function

    ''' <summary>
    ''' [Provide For UI] Cancel the Export File and entitle record entries
    ''' </summary>
    ''' <param name="strFileName"></param>
    ''' <returns>1: Success, -1: Fail, 0: Status Error</returns>
    ''' <remarks>Status Error: Eg. Already Imported , cannot cancel Export</remarks>
    Public Function CancelExport(ByVal strFileName As String) As Integer

        ' Changed to Mark Delete
        ' 1. Validate the Export Entries has not been Import Yet
        ' 2. Update [dbo].[ProfessionalVerification] Status for Every Export Entries in [dbo].[ProfessionalSubmission]
        ' 3. [Void] Mark Delete corresponding Entries in [dbo].[ProfessionalSubmission]
        ' 4. Mark Delete the Export Entries [dbo].[ProfessionalSubmissionHeader]
        ' 5. Cancel The File Generation Queue: Mark Status to I - Inactive

        Try
            Me.udtDB.BeginTransaction()

            ' Retrieve the record [dbo].[ProfessionalSubmissionHeader]
            Dim udtProfessionalSubmissionHeaderModel As ProfessionalSubmissionHeaderModel = Me.GetProfessionalSubmissionHeader(strFileName)

            ' Validate The Export Entries Has Not Yet Import
            If udtProfessionalSubmissionHeaderModel.ImportDtm.HasValue OrElse Not udtProfessionalSubmissionHeaderModel.ImportBy Is Nothing Then
                Return 0
            End If

            ' Retrieve All [dbo].[ProfessionalSubmission]
            Dim udtPSMCollection As ProfessionalSubmissionModelCollection = Me.GetProfessionalSubmissionRecordList(strFileName)

            For Each udtProfSubModel As ProfessionalSubmissionModel In udtPSMCollection.Values

                ' Retrieve [dbo].[ProfessionalVerification] & Update Status For Cancel Export
                Dim strEnrolmentNo As String = ""
                Dim intProfSeq As Integer = -1

                Me.udtFormatter.SplitProfessionalReferenceNo(udtProfSubModel.ReferenceNo, strEnrolmentNo, intProfSeq)
                Dim udtPVM As ProfessionalVerificationModel = Me.GetProfessionalVerification(strEnrolmentNo, intProfSeq)

                ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Fix Professional Verification removed by auto reject amendment when delist Scheme
                ' Skip check and update ProfessionalVerification status if record has been removed
                If Not udtPVM Is Nothing Then
                    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

                    ' Validate The Record has Not Yet Import
                    If Not udtPVM.RecordStatus.Trim() = Common.Component.ProfessionalVerificationRecordStatus.Export OrElse udtPVM.ImportDtm.HasValue OrElse Not udtPVM.ImportBy Is Nothing Then
                        Me.udtDB.RollBackTranscation()
                        Return 0
                    End If

                    ' Update Status For Cancel Export
                    Me.UpdateProfessionalVerificationCancelExport(udtPVM.EnrolmentRefNo, udtPVM.ProfessionalSeq, udtPVM.TSMP)
                End If

            Next

            ' Mark Delete [dbo].[ProfessionalSubmissionHeader]
            Me.MarkDeleteProfessionalSubmissionHeader(strFileName)

            ' Cancel The File Generation Queue if the Queue Not Yet Proceed
            Dim udtFileGenerationBLL As New FileGeneration.FileGenerationBLL()
            Dim udtFileQueue As FileGeneration.FileGenerationQueueModel = udtFileGenerationBLL.RetrieveFileGenerationQueueByFileIDFileName(Common.Component.DataDownloadFileID.BoardAndCouncil, strFileName)
            If Not udtFileQueue Is Nothing Then
                If udtFileQueue.Status = Common.Component.DataDownloadStatus.Pending Then
                    udtFileGenerationBLL.UpdateFileGenerationQueueStatus(udtDB, udtFileQueue.GenerationID, FileGenerationQueueStatus.Inactive)
                End If
            End If

            Me.udtDB.CommitTransaction()
        Catch ex As Exception
            Me.udtDB.RollBackTranscation()
            Throw ex
            Return -1
        End Try

        Return 1

    End Function

    Public Function ViewResult(ByVal strFileName As String) As DataTable

        ' DataTable Variables
        Dim strReferenceNo As String = "ReferenceNo"
        Dim strDisplaySeq As String = "DisplaySeq"
        Dim strRegistrationCode As String = "RegistrationCode"
        Dim strSPHKID As String = "SPHKID"
        Dim strSurname As String = "Surname"
        Dim strOtherName As String = "OtherName"
        Dim strResult As String = "Result"
        Dim strRemark As String = "Remark"
        Dim strStatus As String = "Status"


        Dim dtResult As New DataTable
        Dim drResult As DataRow
        dtResult.Columns.Add(New DataColumn(strReferenceNo, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strDisplaySeq, GetType(Integer)))
        dtResult.Columns.Add(New DataColumn(strRegistrationCode, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strSPHKID, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strSurname, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strOtherName, GetType(String)))

        dtResult.Columns.Add(New DataColumn(strResult, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strRemark, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strStatus, GetType(String)))


        Dim dtRaw As New DataTable
        Dim drRaw As DataRow

        Try
            Dim params() As SqlParameter = { _
                udtDB.MakeInParam("@File_Name", ProfessionalSubmissionModel.File_NameDataType, ProfessionalSubmissionModel.File_NameDataSize, strFileName) _
            }

            '[File_Name], [Reference_No], [Display_Seq], [Registration_Code], [SP_HKID], [Surname], [Other_Name],
            '[Result], [Remark], 
            '[Record_Status]

            udtDB.RunProc(Me.strSP_SearchProfessionViewResult, params, dtRaw)
        Catch ex As Exception
            Throw ex
        End Try

        For Each drRaw In dtRaw.Rows

            drResult = dtResult.NewRow()
            drResult.BeginEdit()

            drResult(strReferenceNo) = drRaw("Reference_No")
            drResult(strDisplaySeq) = drRaw("Display_Seq")
            drResult(strRegistrationCode) = drRaw("Registration_Code")
            drResult(strSPHKID) = drRaw("SP_HKID")
            drResult(strSurname) = drRaw("Surname")
            drResult(strOtherName) = drRaw("Other_Name")
            drResult(strResult) = drRaw("Result")
            drResult(strRemark) = drRaw("Remark")

            Dim strEngDesc As String = ""
            Dim strChiDesc As String = ""
            Common.Component.Status.GetDescriptionFromDBCode(ProfessionalVerificationRecordStatus.ClassCode, drRaw("Record_Status").ToString.Trim(), strEngDesc, strChiDesc)
            drResult(strStatus) = strEngDesc
            drResult.EndEdit()

            dtResult.Rows.Add(drResult)
        Next
        dtResult.AcceptChanges()

        Return dtResult

    End Function
#End Region

#Region "Confirm Result [Verify Record]"

    Public Function SearchProfessionalVerifyRecordToBeVerify(ByVal strInputVerifyStatus As String, ByVal strInputRecordStatus As String, ByVal strERN As String, ByVal strInputHKID As String) As DataTable

        ' DataTable Variables
        Dim strRecordNum As String = "RecordNum"
        Dim strEnrolmentNum As String = "EnrolmentNum"
        Dim strSPID As String = "SPID"
        Dim strSeqNum As String = "SeqNum"
        Dim strHKID As String = "HKID"
        Dim strSPEngName As String = "SPEngName"
        Dim strSPChiName As String = "SPChiName"
        Dim strProfession As String = "Profession"
        Dim strRegistrationCode As String = "RegistrationCode"
        Dim strStatus As String = "Status"
        Dim strRecordStatus As String = "RecordStatus"
        Dim strFile As String = "FileName"
        Dim strExportDtm As String = "ExportDtm"
        Dim strResult As String = "Result"
        Dim strRemark As String = "Remark"
        Dim strTSMP As String = "TSMP"
        Dim strTSMPSpAccountUpdate As String = "TSMPSpAccountUpdate"
        Dim strCount As String = "Count"

        Dim drResult As DataRow
        Dim dtResult As New DataTable()
        dtResult.Columns.Add(New DataColumn(strRecordNum, GetType(Integer)))
        dtResult.Columns.Add(New DataColumn(strEnrolmentNum, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strSPID, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strSeqNum, GetType(Integer)))
        dtResult.Columns.Add(New DataColumn(strHKID, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strSPEngName, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strSPChiName, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strProfession, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strRegistrationCode, GetType(String)))

        dtResult.Columns.Add(New DataColumn(strStatus, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strRecordStatus, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strFile, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strExportDtm, GetType(DateTime)))
        dtResult.Columns.Add(New DataColumn(strResult, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strRemark, GetType(String)))
        dtResult.Columns.Add(New DataColumn(strTSMP, GetType(Byte())))
        dtResult.Columns.Add(New DataColumn(strTSMPSpAccountUpdate, GetType(Byte())))
        dtResult.Columns.Add(New DataColumn(strCount, GetType(String)))

        Dim i As Integer = 0

        Dim drRaw As DataRow
        Dim dtRaw As DataTable = Me.SearchProfessionalVerifyRecord(strInputVerifyStatus, strInputRecordStatus, strERN, strInputHKID)

        ' Record from StoreProc 
        'PV.Enrolment_Ref_No, PV.Professional_Seq, PV.SP_ID, PV.Export_By, PV.Export_Dtm,
        'PV.Import_By, PV.Import_Dtm, PV.Verification_result, PV.Verification_Remark,
        'PV.Final_Result, PV.Defer_By, Defer_Dtm, PV.Record_Status, PV.TSMP,

        'PS.Service_Category_Code, PS.Registration_Code, 
        'SD.Data_Value as Profession_Description,
        'SPS.SP_HKID, SPS.SP_Eng_Name, SPS.SP_Chi_Name, SPS.Enrolment_Dtm,

        'PSH.File_Name,
        'SPAU.TSMP as TSMPSpAccountUpdate,
        'Count

        For Each drRaw In dtRaw.Rows
            i = i + 1
            drResult = dtResult.NewRow()
            drResult.BeginEdit()
            drResult(strRecordNum) = i

            drResult(strEnrolmentNum) = Me.udtFormatter.formatSystemNumber(drRaw("Enrolment_Ref_No").ToString().Trim())
            drResult(strSeqNum) = drRaw("Professional_Seq")
            drResult(strSPID) = drRaw("SP_ID")
            'drResult(strHKID) = drRaw("SP_HKID")
            drResult(strHKID) = Me.udtFormatter.formatHKID(drRaw("SP_HKID").ToString(), False)

            drResult(strSPEngName) = drRaw("SP_Eng_Name")
            If Not drRaw.IsNull("SP_Chi_Name") Then
                drResult(strSPChiName) = Me.udtFormatter.formatChineseName(drRaw("SP_Chi_Name").ToString())
            Else
                drResult(strSPChiName) = drRaw("SP_Chi_Name")
            End If

            drResult(strProfession) = drRaw("Profession_Description")
            drResult(strRegistrationCode) = drRaw("Registration_Code")
            drResult(strRecordStatus) = drRaw("Record_Status").ToString.Trim()

            Dim strEngDesc = ""
            Dim strChiDesc As String = ""
            Common.Component.Status.GetDescriptionFromDBCode(ProfessionalVerificationRecordStatus.ClassCode, drRaw("Record_Status").ToString.Trim(), strEngDesc, strChiDesc)

            drResult(strStatus) = strEngDesc

            If drRaw.IsNull("File_Name") Then
                drResult(strFile) = ""
            Else
                drResult(strFile) = drRaw("File_Name").ToString().Trim()
            End If
            drResult(strExportDtm) = drRaw("Export_Dtm")
            drResult(strResult) = drRaw("Verification_result")
            drResult(strRemark) = drRaw("Verification_Remark")
            drResult(strTSMP) = drRaw("TSMP")
            drResult(strTSMPSpAccountUpdate) = drRaw("TSMPSpAccountUpdate")
            drResult(strCount) = drRaw("Count")
            drResult.EndEdit()
            dtResult.Rows.Add(drResult)
        Next
        dtResult.AcceptChanges()
        Return dtResult

    End Function

    Private Function SearchProfessionalVerifyRecord(ByVal strInputVerifyStatus As String, ByVal strInputRecordStatus As String, ByVal strERN As String, ByVal strInputHKID As String) As DataTable

        Dim dtResult As New DataTable
        Try
            Dim objHKID As Object = Nothing
            If strInputHKID.Trim() = "" Then
                objHKID = DBNull.Value
            Else
                objHKID = Me.udtFormatter.formatHKIDInternal(strInputHKID)
            End If

            Dim objERN As Object = Nothing
            If strERN.Trim() = "" Then
                objERN = DBNull.Value
            Else
                objERN = Me.udtFormatter.formatSystemNumberReverse(strERN.Trim())
            End If

            Dim objRecordStatus As Object = Nothing
            If strInputRecordStatus.Trim() = "" Then
                objRecordStatus = DBNull.Value
            Else
                objRecordStatus = strInputRecordStatus.Trim()
            End If

            Dim objVerifyStatus As Object = Nothing
            If strInputVerifyStatus.Trim() = "" Or strInputVerifyStatus.Trim() = "NA" Then
                objVerifyStatus = DBNull.Value
            Else
                objVerifyStatus = strInputVerifyStatus.Trim()
            End If


            Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Enrolment_Ref_No", ProfessionalVerificationModel.Enrolment_Ref_NoDataType, ProfessionalVerificationModel.Enrolment_Ref_NoDataSize, objERN), _
                    udtDB.MakeInParam("@SP_HKID", SqlDbType.Char, 9, objHKID), _
                    udtDB.MakeInParam("@Verify_Status", ProfessionalVerificationModel.Verification_ResultDataType, ProfessionalVerificationModel.Verification_ResultDataSize, objVerifyStatus), _
                    udtDB.MakeInParam("@Record_Status", ProfessionalVerificationModel.Record_StatusDataType, ProfessionalVerificationModel.Record_StatusDataSize, objRecordStatus)}
            udtDB.RunProc(Me.strSP_SearchProfessionVerificationToVerify, prams, dtResult)
        Catch ex As Exception
            Throw ex
        End Try

        Return dtResult

    End Function

    ''' <summary>
    ''' [Provide For UI] Search All Profession Verify Record to be Verify:
    ''' [SPAccountUpdate].Progress_Status = 'P' 
    ''' Already Export: Export_By, Export_Dtm Not Null
    ''' Either Imported: Not Yet Confiremd / Reject
    ''' Or Pending for Import
    ''' </summary>
    ''' <param name="dtValid">[Out]: Result='Y'</param>
    ''' <param name="dtInValid">[Out]: Result='N'</param>
    ''' <param name="dtSuspect">[Out]: Result='S'</param>
    ''' <param name="dtNA">[Out]: Result=Null</param>
    ''' <remarks></remarks>
    Public Sub Void_SearchProfessionalVerifyRecordToBeVerify(ByRef dtValid As DataTable, ByRef dtInValid As DataTable, ByRef dtSuspect As DataTable, ByRef dtNA As DataTable)

        'Dim drRaw As DataRow
        ''Dim dtRaw As DataTable = Me.SearchProfessionalVerifyRecordToBeVerify()

        '' DataTable Variables
        'Dim strRecordNum As String = "RecordNum"
        'Dim strEnrolmentNum As String = "EnrolmentNum"
        'Dim strSPID As String = "SPID"
        'Dim strSeqNum As String = "SeqNum"
        'Dim strHKID As String = "HKID"
        'Dim strSPEngName As String = "SPEngName"
        'Dim strSPChiName As String = "SPChiName"
        'Dim strProfession As String = "Profession"
        'Dim strRegistrationCode As String = "RegistrationCode"
        'Dim strStatus As String = "Status"
        'Dim strRecordStatus As String = "RecordStatus"
        'Dim strFile As String = "FileName"
        'Dim strExportDtm As String = "ExportDtm"
        'Dim strResult As String = "Result"
        'Dim strRemark As String = "Remark"
        'Dim strTSMP As String = "TSMP"
        'Dim strTSMPSpAccountUpdate As String = "TSMPSpAccountUpdate"
        'Dim strCount As String = "Count"

        'Dim drResult As DataRow
        'Dim dtResult As New DataTable()
        'dtResult.Columns.Add(New DataColumn(strRecordNum, GetType(Integer)))
        'dtResult.Columns.Add(New DataColumn(strEnrolmentNum, GetType(String)))
        'dtResult.Columns.Add(New DataColumn(strSPID, GetType(String)))
        'dtResult.Columns.Add(New DataColumn(strSeqNum, GetType(Integer)))
        'dtResult.Columns.Add(New DataColumn(strHKID, GetType(String)))
        'dtResult.Columns.Add(New DataColumn(strSPEngName, GetType(String)))
        'dtResult.Columns.Add(New DataColumn(strSPChiName, GetType(String)))
        'dtResult.Columns.Add(New DataColumn(strProfession, GetType(String)))
        'dtResult.Columns.Add(New DataColumn(strRegistrationCode, GetType(String)))

        'dtResult.Columns.Add(New DataColumn(strStatus, GetType(String)))
        'dtResult.Columns.Add(New DataColumn(strRecordStatus, GetType(String)))
        'dtResult.Columns.Add(New DataColumn(strFile, GetType(String)))
        'dtResult.Columns.Add(New DataColumn(strExportDtm, GetType(DateTime)))
        'dtResult.Columns.Add(New DataColumn(strResult, GetType(String)))
        'dtResult.Columns.Add(New DataColumn(strRemark, GetType(String)))
        'dtResult.Columns.Add(New DataColumn(strTSMP, GetType(Byte())))
        'dtResult.Columns.Add(New DataColumn(strTSMPSpAccountUpdate, GetType(Byte())))
        'dtResult.Columns.Add(New DataColumn(strCount, GetType(String)))

        'Dim i As Integer = 0

        '' Record from StoreProc 
        ''PV.Enrolment_Ref_No, PV.Professional_Seq, PV.SP_ID, PV.Export_By, PV.Export_Dtm,
        ''PV.Import_By, PV.Import_Dtm, PV.Verification_result, PV.Verification_Remark,
        ''PV.Final_Result, PV.Defer_By, Defer_Dtm, PV.Record_Status, PV.TSMP,

        ''PS.Service_Category_Code, PS.Registration_Code, 
        ''SD.Data_Value as Profession_Description,
        ''SPS.SP_HKID, SPS.SP_Eng_Name, SPS.SP_Chi_Name, SPS.Enrolment_Dtm,

        ''PSH.File_Name,
        ''SPAU.TSMP as TSMPSpAccountUpdate,
        ''Count

        'For Each drRaw In dtRaw.Rows
        '    i = i + 1
        '    drResult = dtResult.NewRow()
        '    drResult.BeginEdit()
        '    drResult(strRecordNum) = i

        '    drResult(strEnrolmentNum) = Me.udtFormatter.formatSystemNumber(drRaw("Enrolment_Ref_No").ToString().Trim())
        '    drResult(strSeqNum) = drRaw("Professional_Seq")
        '    drResult(strSPID) = drRaw("SP_ID")
        '    'drResult(strHKID) = drRaw("SP_HKID")
        '    drResult(strHKID) = Me.udtFormatter.formatHKID(drRaw("SP_HKID").ToString(), False)

        '    drResult(strSPEngName) = drRaw("SP_Eng_Name")
        '    If Not drRaw.IsNull("SP_Chi_Name") Then
        '        drResult(strSPChiName) = Me.udtFormatter.formatChineseName(drRaw("SP_Chi_Name").ToString())
        '    Else
        '        drResult(strSPChiName) = drRaw("SP_Chi_Name")
        '    End If

        '    drResult(strProfession) = drRaw("Profession_Description")
        '    drResult(strRegistrationCode) = drRaw("Registration_Code")
        '    drResult(strRecordStatus) = drRaw("Record_Status").ToString.Trim()

        '    Dim strEngDesc = ""
        '    Dim strChiDesc As String = ""
        '    Common.Component.Status.GetDescriptionFromDBCode("ProfessionalVerificationRecordStatus", drRaw("Record_Status").ToString.Trim(), strEngDesc, strChiDesc)

        '    drResult(strStatus) = strEngDesc

        '    If drRaw.IsNull("File_Name") Then
        '        drResult(strFile) = ""
        '    Else
        '        drResult(strFile) = drRaw("File_Name").ToString().Trim()
        '    End If
        '    drResult(strExportDtm) = drRaw("Export_Dtm")
        '    drResult(strResult) = drRaw("Verification_result")
        '    drResult(strRemark) = drRaw("Verification_Remark")
        '    drResult(strTSMP) = drRaw("TSMP")
        '    drResult(strTSMPSpAccountUpdate) = drRaw("TSMPSpAccountUpdate")
        '    drResult(strCount) = drRaw("Count")
        '    drResult.EndEdit()
        '    dtResult.Rows.Add(drResult)
        'Next
        'dtResult.AcceptChanges()

        '' Valid: Result = 'Y'
        '' dtInvalid: Result = 'N'
        '' dtSuspect: Result = 'S'
        '' N/A: Result = Null

        'Dim dsValid As New DataSet
        'Dim dsInValid As New DataSet
        'Dim dsSuspect As New DataSet
        'Dim dsNA As New DataSet

        'dsValid.Merge(dtResult.Select(strResult + "='" + Common.Component.HealthProfVerifyStatus.Validated + "'"))
        'dsInValid.Merge(dtResult.Select(strResult + "='" + Common.Component.HealthProfVerifyStatus.Invalid + "'"))
        'dsSuspect.Merge(dtResult.Select(strResult + "='" + Common.Component.HealthProfVerifyStatus.Suspect + "'"))
        'dsNA.Merge(dtResult.Select(strResult + " IS Null"))

        'If dsValid.Tables.Count > 0 Then
        '    dtValid = dsValid.Tables(0)
        '    Me.SetRecordNum(dtValid, strRecordNum, True)
        'End If

        'If dsInValid.Tables.Count > 0 Then
        '    dtInValid = dsInValid.Tables(0)
        '    Me.SetRecordNum(dtInValid, strRecordNum, True)
        'End If

        'If dsSuspect.Tables.Count > 0 Then
        '    dtSuspect = dsSuspect.Tables(0)
        '    Me.SetRecordNum(dtSuspect, strRecordNum, True)
        'End If

        'If dsNA.Tables.Count > 0 Then
        '    dtNA = dsNA.Tables(0)
        '    Me.SetRecordNum(dtNA, strRecordNum, True)
        'End If

    End Sub

    ''' <summary>
    ''' Search All Professional Verify Record to Be Verify:
    ''' [SPAccountUpdate].Progress_Status = 'P' 
    ''' Already Export: Export_By, Export_Dtm Not Null
    ''' Either Imported: Not Yet Confiremd / Reject
    ''' Or Pending for Import
    ''' </summary>
    ''' <returns>DataTable With Many Rows</returns>
    ''' <remarks></remarks>
    Private Function Void_SearchProfessionalVerifyRecordToBeVerify() As DataTable

        ' Record from StoreProc 
        'PV.Enrolment_Ref_No, PV.Professional_Seq, PV.SP_ID, PV.Export_By, PV.Export_Dtm,
        'PV.Import_By, PV.Import_Dtm, PV.Verification_result, PV.Verification_Remark,
        'PV.Final_Result, PV.Defer_By, Defer_Dtm, PV.Record_Status, PV.TSMP,

        'PS.Service_Category_Code, PS.Registration_Code, 
        'SD.Data_Value as Profession_Description,
        'SPS.SP_HKID, SPS.SP_Eng_Name, SPS.SP_Chi_Name, SPS.Enrolment_Dtm,

        'PSH.File_Name,
        'SPAU.TSMP as TSMPSpAccountUpdate,
        'Count

        Dim dtResult As New DataTable
        Try
            udtDB.RunProc(Me.strSP_SearchProfessionVerificationToVerify, dtResult)
        Catch ex As Exception
            Throw ex
        End Try

        dtResult.AcceptChanges()
        Return dtResult

    End Function

    ''' <summary>
    ''' [Provide For UI] Defer Selected Profession Verify Record
    ''' </summary>
    ''' <param name="udtPVMCollection"></param>
    ''' <param name="strUserID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeferProfessionalVerificationRecord(ByVal udtPVMCollection As ProfessionalVerificationModelCollection, ByVal strUserID As String) As Boolean

        Dim udtGeneral As New Common.ComFunction.GeneralFunction()
        Dim dtmCurrent As DateTime = udtGeneral.GetSystemDateTime()

        Try
            ' Update ProfessionVerificationRecord Defer_By & Defer_Dtm & Record_Status = 'D'
            Me.udtDB.BeginTransaction()

            For Each udtPVModel As ProfessionalVerificationModel In udtPVMCollection.Values

                Me.UpdateProfessionalVerificationDefer(udtPVModel.EnrolmentRefNo, udtPVModel.ProfessionalSeq, strUserID, dtmCurrent, udtPVModel.TSMP)
            Next

            Me.udtDB.CommitTransaction()

        Catch ex As Exception
            Me.udtDB.RollBackTranscation()
            Throw ex
            Return False
        End Try

        Return True

    End Function

    ''' <summary>
    ''' [Provide For UI] Accept Selected Profession Verify Record
    ''' Two Case: New Enrolment, Exising Service Provider
    ''' </summary>
    ''' <param name="udtPVMCollection">Professional Verification List</param>
    ''' <param name="dicSPAccountUpdateTsmp">SPAccountUpdate TSMP List</param>
    ''' <param name="strUserID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AcceptProfessionalVerificationRecord(ByVal udtPVMCollection As ProfessionalVerificationModelCollection, _
                                                         ByVal dicSPAccountUpdateTsmp As Dictionary(Of String, Byte()), ByVal strUserID As String) As Boolean

        ' Remark: Validate Against [ProfessionalVerification].[TSMP] + Validate Against [SPAccountUpdate].TSMP
        ' 1. Update [dbo].[ProfessionalVerification] Confirmed_By, Confirm_Dtm & Record_Status = 'C'
        ' 2. Get All unique Enrolment Reference No, Check all ProfessionalVerification Record_status = 'C'
        ' 3. If New Enrolment: Update [dbo].[SPAccountUpdate] Progress Status To 'T'
        ' 4. If Scheme Enrolment: Update [dbo].[SPAccountUpdate] Progress Status To 'S'
        ' 5. If Complete Enrolment:
        '   -- Update [dbo].[SPAccountUpdate] Progress Status To C
        '   -- Copy Staging To SP & Remove Staging, Verification, [dbo].[SPAccountUpdate] Entries

        Dim udtGeneral As New Common.ComFunction.GeneralFunction()
        Dim dtmCurrent As DateTime = udtGeneral.GetSystemDateTime()

        Try
            Me.udtDB.BeginTransaction()

            ' Enrolment Reference No List
            Dim lstStrEnrolRefNo As New List(Of String)
            ' SPAccountUpdate TSMP List
            Dim dicTsmp As New Dictionary(Of String, Byte())

            ' Update [dbo].[ProfessionalVerification] Confirm Record_Status
            Dim intCounter As Integer = 0

            For Each udtPVModel As ProfessionalVerificationModel In udtPVMCollection.Values
                Me.UpdateProfessionalVerificationAccept(udtPVModel.EnrolmentRefNo, udtPVModel.ProfessionalSeq, strUserID, dtmCurrent, udtPVModel.TSMP)
                If Not lstStrEnrolRefNo.Contains(udtPVModel.EnrolmentRefNo) Then
                    lstStrEnrolRefNo.Add(udtPVModel.EnrolmentRefNo)
                    dicTsmp.Add(udtPVModel.EnrolmentRefNo, dicSPAccountUpdateTsmp(udtPVModel.EnrolmentRefNo))
                End If
                intCounter = intCounter + 1
            Next

            ' For Each Enrolment / SP
            intCounter = 0
            For Each strEnrolRefNo As String In lstStrEnrolRefNo

                ' Check all ProfessionalVerification Record_status = 'C' Under Same Enrolment

                Dim udtVerifyPVMColl As ProfessionalVerificationModelCollection = Me.GetProfessionalVerificationListByERN(strEnrolRefNo, Me.udtDB)
                Dim blnNewEnrolment As Boolean = True
                Dim blnReadyForAccept As Boolean = True

                For Each udtPVModel As ProfessionalVerificationModel In udtVerifyPVMColl.Values
                    If Not udtPVModel.SPID Is Nothing Then blnNewEnrolment = False
                    If udtPVModel.RecordStatus.Trim() <> Common.Component.ProfessionalVerificationRecordStatus.Confirm Then
                        blnReadyForAccept = False
                    End If
                Next

                'Check whether the accept should pass to scheme enrolment
                Dim blnSchemeEnrolment As Boolean = False
                Dim udtSPAccUpdateBLL As New SPAccountUpdateBLL()
                Dim udtSPAccUpdateModelCheck As New SPAccountUpdateModel()
                udtSPAccUpdateModelCheck = udtSPAccUpdateBLL.GetSPAccountUpdateByERN(strEnrolRefNo, Me.udtDB)
                If Not IsNothing(udtSPAccUpdateModelCheck) Then
                    If udtSPAccUpdateModelCheck.SchemeConfirm = True Then
                        blnSchemeEnrolment = True
                    End If
                End If

                ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                If blnReadyForAccept Then
                    If blnNewEnrolment Then
                        ' New Enrolment: Update [dbo].[SPAccountUpdate] Progress Status To 'T'
                        
                        Dim udtSPAccUpdateModel As New SPAccountUpdateModel()
                        udtSPAccUpdateModel.EnrolRefNo = strEnrolRefNo
                        udtSPAccUpdateModel.UpdateBy = strUserID
                        udtSPAccUpdateModel.ProgressStatus = Common.Component.SPAccountUpdateProgressStatus.WaitingForIssueToken
                        udtSPAccUpdateModel.TSMP = dicTsmp(strEnrolRefNo)
                        Dim blnSuccess As Boolean = udtSPAccUpdateBLL.UpdateSPAccountUpdateProgressStatus(udtSPAccUpdateModel, Me.udtDB)

                    ElseIf blnSchemeEnrolment Then
                        'Scheme Enrolment: Update [dbo].[SPAccountUpdate] Progress Status To 'S'
                        Dim udtSPAccUpdateModel As New SPAccountUpdateModel()
                        udtSPAccUpdateModel.EnrolRefNo = strEnrolRefNo
                        udtSPAccUpdateModel.UpdateBy = strUserID
                        udtSPAccUpdateModel.ProgressStatus = Common.Component.SPAccountUpdateProgressStatus.WaitingForSchemeEnrolment
                        udtSPAccUpdateModel.TSMP = dicTsmp(strEnrolRefNo)
                        Dim blnSuccess As Boolean = udtSPAccUpdateBLL.UpdateSPAccountUpdateProgressStatus(udtSPAccUpdateModel, Me.udtDB)

                        ' ----------------------------------------------------------------------------------------
                        ' Remove partial accept flow (Pass to Scheme Enrolment to complete whole enrolment)
                        ' ----------------------------------------------------------------------------------------
                        ''Retrieve the latest time stamp after SPAccountUpdate status is updated
                        'udtSPAccUpdateModelCheck = udtSPAccUpdateBLL.GetSPAccountUpdateByERN(strEnrolRefNo, Me.udtDB)
                        'dicTsmp(strEnrolRefNo) = udtSPAccUpdateModelCheck.TSMP

                        'Dim alEnrolledSchemeCode As New ArrayList
                        'Dim alNewSchemeCode As New ArrayList
                        'Dim udtServiceProviderBLL As New ServiceProvider.ServiceProviderBLL

                        'Dim udtSPM As ServiceProviderModel
                        'udtSPM = udtServiceProviderBLL.GetServiceProviderStagingByERN(strEnrolRefNo, New Common.DataAccess.Database)

                        'If Not IsNothing(udtSPM) AndAlso Not IsNothing(udtSPM.SchemeInfoList) Then
                        '    For Each udtSchemeInfoModel As SchemeInformation.SchemeInformationModel In udtSPM.SchemeInfoList.Values
                        '        If udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.Existing) Then
                        '            If Not alEnrolledSchemeCode.Contains(udtSchemeInfoModel.SchemeCode) Then
                        '                alEnrolledSchemeCode.Add(udtSchemeInfoModel.SchemeCode)
                        '            End If
                        '        ElseIf udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.Active) Then
                        '            If Not alNewSchemeCode.Contains(udtSchemeInfoModel.SchemeCode) Then
                        '                alNewSchemeCode.Add(udtSchemeInfoModel.SchemeCode)
                        '            End If
                        '        Else
                        '            'Do nothing
                        '        End If
                        '    Next
                        'End If

                        'Dim udtSPProfile As New SPProfileBLL()
                        'Dim blnReturn As Boolean = udtSPProfile.PartiallyAcceptSPProfileUserCUserD(udtDB, strEnrolRefNo, strUserID, dicTsmp(strEnrolRefNo), alEnrolledSchemeCode)
                        ' ----------------------------------------------------------------------------------------

                    Else
                        ' Complete Enrolment
                        Dim alEnrolledSchemeCode As New ArrayList
                        Dim alNewSchemeCode As New ArrayList
                        Dim udtServiceProviderBLL As New ServiceProvider.ServiceProviderBLL

                        Dim udtSPM As ServiceProviderModel
                        udtSPM = udtServiceProviderBLL.GetServiceProviderStagingByERN(strEnrolRefNo, New Common.DataAccess.Database)

                        If Not IsNothing(udtSPM) AndAlso Not IsNothing(udtSPM.SchemeInfoList) Then
                            For Each udtSchemeInfoModel As SchemeInformation.SchemeInformationModel In udtSPM.SchemeInfoList.Values
                                If udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.Existing) OrElse _
                                   udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.ActivePendingDelist) OrElse _
                                   udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.ActivePendingSuspend) OrElse _
                                   udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.Suspended) OrElse _
                                   udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.SuspendedPendingDelist) OrElse _
                                   udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.SuspendedPendingReactivate) Then

                                    If Not alEnrolledSchemeCode.Contains(udtSchemeInfoModel.SchemeCode) Then
                                        alEnrolledSchemeCode.Add(udtSchemeInfoModel.SchemeCode)
                                    End If
                                ElseIf udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.Active) Then
                                    If Not alNewSchemeCode.Contains(udtSchemeInfoModel.SchemeCode) Then
                                        alNewSchemeCode.Add(udtSchemeInfoModel.SchemeCode)
                                    End If
                                Else
                                    'Do nothing
                                End If
                            Next
                        End If

                        ' Update [dbo].[SPAccountUpdate] Progress Status To C
                        Dim udtSPProfile As New SPProfileBLL()

                        ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
                        ' ----------------------------------------------------------------------------------------
                        'Dim blnReturn As Boolean = udtSPProfile.AcceptSPProfileUserCUserD(udtDB, strEnrolRefNo, strUserID, dicTsmp(strEnrolRefNo), alEnrolledSchemeCode)
                        Dim blnReturn As Boolean = udtSPProfile.AcceptSPProfile(udtDB, strEnrolRefNo, strUserID, dicTsmp(strEnrolRefNo), alEnrolledSchemeCode)
                        ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

                        If blnReturn = False Then
                            Me.udtDB.RollBackTranscation()
                            Return False
                        End If
                    End If
                    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

                End If

                intCounter = intCounter + 1
            Next

            Me.udtDB.CommitTransaction()

        Catch ex As Exception
            Me.udtDB.RollBackTranscation()
            Throw ex
            Return False
        End Try

    End Function

    ''' <summary>
    ''' [Provide For UI] To Display the Profession Verify Record By Enrolment 
    ''' </summary>
    ''' <param name="strERN"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetProfessionalVerifyRecordByERN(ByVal strERN As String) As DataTable

        Dim dtResult As New DataTable
        Try
            'PV.Enrolment_Ref_No, PV.Professional_Seq, PV.SP_ID, PV.Export_By, PV.Export_Dtm,
            'PV.Import_By, PV.Import_Dtm, PV.Verification_result, PV.Verification_Remark,
            'PV.Final_Result, PV.Defer_By, Defer_Dtm, PV.Record_Status,

            'PS.Service_Category_Code, PS.Registration_Code, 
            'SD.Data_Value as Profession_Description,
            'SPS.SP_HKID, SPS.SP_Eng_Name, SPS.SP_Chi_Name, SPS.Enrolment_Dtm,

            'PSH.File_Name

            Dim params() As SqlParameter = {udtDB.MakeInParam("@Enrolment_Ref_No", ProfessionalVerificationModel.Enrolment_Ref_NoDataType, ProfessionalVerificationModel.Enrolment_Ref_NoDataSize, strERN)}
            udtDB.RunProc("proc_ProfessionalVerification_get_byERN_toDisplay", params, dtResult)

            For Each drRow As DataRow In dtResult.Rows
                Dim strEngDesc As String = ""
                Dim strChiDesc As String = ""
                Common.Component.Status.GetDescriptionFromDBCode(ProfessionalVerificationRecordStatus.ClassCode, drRow("Record_Status").ToString.Trim(), strEngDesc, strChiDesc)
                drRow.BeginEdit()
                drRow("Record_Status") = strEngDesc

                drRow("SP_HKID") = Me.udtFormatter.formatHKID(drRow("SP_HKID").ToString(), False)
                drRow.EndEdit()
            Next
            dtResult.AcceptChanges()
        Catch ex As Exception
            Throw ex
        End Try

        dtResult.AcceptChanges()
        Return dtResult

    End Function
#End Region

#Region "Table Related"

#Region "[dbo].[ProfessionalVerification]"

    ''' <summary>
    ''' [External] Get List of [dbo].[ProfessionalVerification]
    ''' By EnrolmentRefNo
    ''' </summary>
    ''' <param name="strEnrolRefNo"></param>
    ''' <param name="udtDB"></param>
    ''' <returns>ProfessionalVerificationModelCollection</returns>
    ''' <remarks></remarks>
    Public Function GetProfessionalVerificationListByERN(ByVal strEnrolRefNo As String, ByRef udtDB As Database) As ProfessionalVerificationModelCollection
        Dim udtPVMCollection As New ProfessionalVerificationModelCollection()

        'Enrolment_Ref_No, Professional_Seq, SP_ID,
        'Export_By, Export_Dtm,
        'Import_By, Import_Dtm,
        'Verification_Result, Verification_Remark, Final_Result,
        'Confirm_By, Confirm_Dtm, 
        'Void_By, Void_Dtm,
        'Defer_By, Defer_Dtm,
        'Record_Status, TSMP

        Dim intProfSeq As Integer
        Dim strSPID, strExportBy, strImportBy, strConfirmBy, strVoidBy, strDeferBy As String
        Dim strResult, strRemark, strFinalResult, strRecordStatus As String
        Dim dtmExport, dtmImport, dtmConfirm, dtmVoid, dtmDefer As Nullable(Of DateTime)
        Dim arrByteTSMP As Byte()
        Dim udtProfessionalVerificationModel As ProfessionalVerificationModel = Nothing

        Try
            Dim params() As SqlParameter = { _
                            udtDB.MakeInParam("@Enrolment_Ref_No", ProfessionalVerificationModel.Enrolment_Ref_NoDataType, ProfessionalVerificationModel.Enrolment_Ref_NoDataSize, strEnrolRefNo) _
            }

            Dim dtResult As New DataTable()
            Dim i As Integer = 0
            Dim drRow As DataRow
            udtDB.RunProc(Me.strSP_GetProfessionalVerificationByEnrolmentRefNo, params, dtResult)

            For i = 0 To dtResult.Rows.Count - 1

                drRow = dtResult.Rows(i)

                intProfSeq = Convert.ToInt32(drRow("Professional_Seq"))
                If IsDBNull(drRow("SP_ID")) Then strSPID = Nothing Else strSPID = drRow("SP_ID").ToString().Trim()
                If IsDBNull(drRow("Export_By")) Then strExportBy = Nothing Else strExportBy = drRow("Export_By").ToString().Trim()
                If IsDBNull(drRow("Export_Dtm")) Then dtmExport = Nothing Else dtmExport = Convert.ToDateTime(drRow("Export_Dtm"))
                If IsDBNull(drRow("Import_By")) Then strImportBy = Nothing Else strImportBy = drRow("Import_By").ToString().Trim()
                If IsDBNull(drRow("Import_Dtm")) Then dtmImport = Nothing Else dtmImport = Convert.ToDateTime(drRow("Import_Dtm"))
                If IsDBNull(drRow("Verification_Result")) Then strResult = Nothing Else strResult = drRow("Verification_Result").ToString().Trim()
                If IsDBNull(drRow("Verification_Remark")) Then strRemark = Nothing Else strRemark = drRow("Verification_Remark").ToString().Trim()
                If IsDBNull(drRow("Final_Result")) Then strFinalResult = Nothing Else strFinalResult = drRow("Final_Result").ToString().Trim()
                If IsDBNull(drRow("Confirm_By")) Then strConfirmBy = Nothing Else strConfirmBy = drRow("Confirm_By").ToString().Trim()
                If IsDBNull(drRow("Confirm_Dtm")) Then dtmConfirm = Nothing Else dtmConfirm = Convert.ToDateTime(drRow("Confirm_Dtm"))
                If IsDBNull(drRow("Void_By")) Then strVoidBy = Nothing Else strVoidBy = drRow("Void_By").ToString().Trim()
                If IsDBNull(drRow("Void_Dtm")) Then dtmVoid = Nothing Else dtmVoid = Convert.ToDateTime(drRow("Void_Dtm"))
                If IsDBNull(drRow("Defer_By")) Then strDeferBy = Nothing Else strDeferBy = drRow("Defer_By").ToString().Trim()
                If IsDBNull(drRow("Defer_Dtm")) Then dtmDefer = Nothing Else dtmDefer = Convert.ToDateTime(drRow("Defer_Dtm"))
                If IsDBNull(drRow("Record_Status")) Then strRecordStatus = Nothing Else strRecordStatus = drRow("Record_Status").ToString().Trim()
                If IsDBNull(drRow("TSMP")) Then arrByteTSMP = Nothing Else arrByteTSMP = CType(drRow("TSMP"), Byte())

                udtProfessionalVerificationModel = New ProfessionalVerificationModel(strEnrolRefNo, intProfSeq, strSPID, strExportBy, _
                    dtmExport, strImportBy, dtmImport, strResult, strRemark, strFinalResult, strConfirmBy, _
                    dtmConfirm, strVoidBy, dtmVoid, strDeferBy, dtmDefer, strRecordStatus, arrByteTSMP)

                udtPVMCollection.Add(udtProfessionalVerificationModel)
            Next


        Catch ex As Exception
            Throw ex
        End Try

        Return udtPVMCollection
    End Function

    ''' <summary>
    ''' Get Single Entry in [dbo].[ProfessionalVerification]
    ''' By Params, [SPAccountUpdate].Progess_Status = 'P'
    ''' </summary>
    ''' <param name="strEnrolRefNo"></param>
    ''' <param name="intProfSeq"></param>
    ''' <returns>ProfessionalVerificationModel</returns>
    ''' <remarks></remarks>
    Public Function GetProfessionalVerification(ByVal strEnrolRefNo As String, ByVal intProfSeq As Integer) As ProfessionalVerificationModel

        'Enrolment_Ref_No, Professional_Seq, SP_ID,
        'Export_By, Export_Dtm,
        'Import_By, Import_Dtm,
        'Verification_Result, Verification_Remark, Final_Result,
        'Confirm_By, Confirm_Dtm, 
        'Void_By, Void_Dtm,
        'Defer_By, Defer_Dtm,
        'Record_Status, TSMP

        Dim strSPID, strExportBy, strImportBy, strConfirmBy, strVoidBy, strDeferBy As String
        Dim strResult, strRemark, strFinalResult, strRecordStatus As String
        Dim dtmExport, dtmImport, dtmConfirm, dtmVoid, dtmDefer As Nullable(Of DateTime)
        Dim arrByteTSMP As Byte()
        Dim udtProfessionalVerificationModel As ProfessionalVerificationModel = Nothing

        Try
            Dim params() As SqlParameter = { _
                udtDB.MakeInParam("@Enrolment_Ref_No", ProfessionalVerificationModel.Enrolment_Ref_NoDataType, ProfessionalVerificationModel.Enrolment_Ref_NoDataSize, strEnrolRefNo), _
                udtDB.MakeInParam("@Professional_Seq", ProfessionalVerificationModel.Professional_SeqDataType, ProfessionalVerificationModel.Professional_SeqDataSize, intProfSeq) _
            }

            Dim dtResult As New DataTable()
            Dim drRow As DataRow
            udtDB.RunProc(Me.strSP_GetProfessionalVerificationByKey, params, dtResult)

            If dtResult.Rows.Count > 0 Then
                drRow = dtResult.Rows(0)

                If IsDBNull(drRow("SP_ID")) Then strSPID = Nothing Else strSPID = drRow("SP_ID").ToString().Trim()
                If IsDBNull(drRow("Export_By")) Then strExportBy = Nothing Else strExportBy = drRow("Export_By").ToString().Trim()
                If IsDBNull(drRow("Export_Dtm")) Then dtmExport = Nothing Else dtmExport = Convert.ToDateTime(drRow("Export_Dtm"))
                If IsDBNull(drRow("Import_By")) Then strImportBy = Nothing Else strImportBy = drRow("Import_By").ToString().Trim()
                If IsDBNull(drRow("Import_Dtm")) Then dtmImport = Nothing Else dtmImport = Convert.ToDateTime(drRow("Import_Dtm"))
                If IsDBNull(drRow("Verification_Result")) Then strResult = Nothing Else strResult = drRow("Verification_Result").ToString().Trim()
                If IsDBNull(drRow("Verification_Remark")) Then strRemark = Nothing Else strRemark = drRow("Verification_Remark").ToString().Trim()
                If IsDBNull(drRow("Final_Result")) Then strFinalResult = Nothing Else strFinalResult = drRow("Final_Result").ToString().Trim()
                If IsDBNull(drRow("Confirm_By")) Then strConfirmBy = Nothing Else strConfirmBy = drRow("Confirm_By").ToString().Trim()
                If IsDBNull(drRow("Confirm_Dtm")) Then dtmConfirm = Nothing Else dtmConfirm = Convert.ToDateTime(drRow("Confirm_Dtm"))
                If IsDBNull(drRow("Void_By")) Then strVoidBy = Nothing Else strVoidBy = drRow("Void_By").ToString().Trim()
                If IsDBNull(drRow("Void_Dtm")) Then dtmVoid = Nothing Else dtmVoid = Convert.ToDateTime(drRow("Void_Dtm"))
                If IsDBNull(drRow("Defer_By")) Then strDeferBy = Nothing Else strDeferBy = drRow("Defer_By").ToString().Trim()
                If IsDBNull(drRow("Defer_Dtm")) Then dtmDefer = Nothing Else dtmDefer = Convert.ToDateTime(drRow("Defer_Dtm"))
                If IsDBNull(drRow("Record_Status")) Then strRecordStatus = Nothing Else strRecordStatus = drRow("Record_Status").ToString().Trim()
                If IsDBNull(drRow("TSMP")) Then arrByteTSMP = Nothing Else arrByteTSMP = CType(drRow("TSMP"), Byte())

                udtProfessionalVerificationModel = New ProfessionalVerificationModel(strEnrolRefNo, intProfSeq, strSPID, strExportBy, _
                    dtmExport, strImportBy, dtmImport, strResult, strRemark, strFinalResult, strConfirmBy, _
                    dtmConfirm, strVoidBy, dtmVoid, strDeferBy, dtmDefer, strRecordStatus, arrByteTSMP)
            Else
                Return Nothing
            End If

        Catch ex As Exception
            Throw ex
        End Try

        Return udtProfessionalVerificationModel

    End Function

    ''' <summary>
    ''' [External] Add List of Entry in [dbo].[ProfessionalVerification]
    ''' By Professional
    ''' </summary>
    ''' <param name="strERN"></param>
    ''' <param name="udtProfessionalModelCollection"></param>
    ''' <param name="udtDB"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddProfessionalVerification(ByVal strERN As String, ByVal udtProfessionalModelCollection As ProfessionalModelCollection, ByVal udtDB As Database) As Boolean

        ' To Handle Return For Amendment, ProfessionVerificationRecord Already Exist

        Dim blnRes As Boolean = False
        Try

            Dim udtExistPVCollection As ProfessionalVerificationModelCollection = Me.GetProfessionalVerificationListByERN(strERN, udtDB)

            For Each udtProfessionalModel As ProfessionalModel In udtProfessionalModelCollection.Values

                Dim blnAdd As Boolean = True

                If Not udtExistPVCollection Is Nothing AndAlso udtExistPVCollection.IndexOfKey(strERN + "-" + udtProfessionalModel.ProfessionalSeq.ToString()) >= 0 Then
                    Dim udtTempPVModel As ProfessionalVerificationModel = CType(udtExistPVCollection.GetByIndex(udtExistPVCollection.IndexOfKey(strERN + "-" + udtProfessionalModel.ProfessionalSeq.ToString())), ProfessionalVerificationModel)
                    If udtTempPVModel.ConfirmBy <> "" And udtTempPVModel.ConfirmDtm.HasValue Then
                        Me.UpdateProfessionalVerificationStatus(udtDB, strERN, udtProfessionalModel.ProfessionalSeq, udtProfessionalModel.CreateBy, Common.Component.ProfessionalVerificationRecordStatus.Confirm, udtTempPVModel.TSMP)
                    Else
                        Me.UpdateProfessionalVerificationStatus(udtDB, strERN, udtProfessionalModel.ProfessionalSeq, udtProfessionalModel.CreateBy, Common.Component.ProfessionalVerificationRecordStatus.Import, udtTempPVModel.TSMP)
                    End If
                    blnAdd = False
                End If

                If blnAdd Then

                    Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtProfessionalModel.EnrolRefNo), _
                    udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtProfessionalModel.SPID.Equals(String.Empty), DBNull.Value, udtProfessionalModel.SPID)), _
                    udtDB.MakeInParam("@professional_seq", ProfessionalModel.ProfessionalSeqDataType, ProfessionalModel.ProfessionalSeqDataSize, udtProfessionalModel.ProfessionalSeq), _
                    udtDB.MakeInParam("@record_status", ProfessionalVerificationModel.Record_StatusDataType, ProfessionalVerificationModel.Record_StatusDataSize, ProfessionalVerificationRecordStatus.Update)}

                    udtDB.RunProc(strSP_AddProfessionalVerification, prams)
                End If

            Next
            Return True
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function


    ''' <summary>
    ''' Update Single Entry in [dbo].[ProfessionalVerification] For Cancel Export
    ''' </summary>
    ''' <param name="strEnrolmentRefNo"></param>
    ''' <param name="intProfessionSeq"></param>
    ''' <param name="arrByteTSMP"></param>
    ''' <remarks></remarks>
    Private Sub UpdateProfessionalVerificationCancelExport(ByVal strEnrolmentRefNo As String, ByVal intProfessionSeq As Integer, ByVal arrByteTSMP As Byte())
        Try
            Dim params() As SqlParameter = { _
                udtDB.MakeInParam("@Enrolment_Ref_No", ProfessionalVerificationModel.Enrolment_Ref_NoDataType, ProfessionalVerificationModel.Enrolment_Ref_NoDataSize, strEnrolmentRefNo), _
                udtDB.MakeInParam("@Professional_Seq", ProfessionalVerificationModel.Professional_SeqDataType, ProfessionalVerificationModel.Professional_SeqDataSize, intProfessionSeq), _
                udtDB.MakeInParam("@tsmp", ProfessionalVerificationModel.TSMPDataType, ProfessionalVerificationModel.TSMPDataSize, arrByteTSMP) _
            }
            udtDB.RunProc(Me.strSP_UpdateProfessionalVerificationForCancelExport, params)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Update Single Entry in [dbo].[ProfessionalVerification] For Export
    ''' </summary>
    ''' <param name="strExportBy"></param>
    ''' <param name="dtmExport"></param>
    ''' <param name="strEnrolmentRefNo"></param>
    ''' <param name="intProfessionSeq"></param>
    ''' <param name="arrByteTSMP"></param>
    ''' <remarks>Record_Status = 'O'</remarks>
    Private Sub UpdateProfessionalVerificationExport(ByVal strExportBy As String, ByVal dtmExport As DateTime, ByVal strEnrolmentRefNo As String, ByVal intProfessionSeq As Integer, ByVal arrByteTSMP As Byte())
        Try
            Dim params() As SqlParameter = { _
                udtDB.MakeInParam("@Export_By", ProfessionalVerificationModel.Export_ByDataType, ProfessionalVerificationModel.Export_ByDataSize, strExportBy), _
                udtDB.MakeInParam("@Export_Dtm", ProfessionalVerificationModel.Export_DtmDataType, ProfessionalVerificationModel.Export_DtmDataSize, dtmExport), _
                udtDB.MakeInParam("@Enrolment_Ref_No", ProfessionalVerificationModel.Enrolment_Ref_NoDataType, ProfessionalVerificationModel.Enrolment_Ref_NoDataSize, strEnrolmentRefNo), _
                udtDB.MakeInParam("@Professional_Seq", ProfessionalVerificationModel.Professional_SeqDataType, ProfessionalVerificationModel.Professional_SeqDataSize, intProfessionSeq), _
                udtDB.MakeInParam("@tsmp", ProfessionalVerificationModel.TSMPDataType, ProfessionalVerificationModel.TSMPDataSize, arrByteTSMP) _
            }

            udtDB.RunProc(Me.strSP_UpdateProfessionalVerificationForExport, params)

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    ''' <summary>
    ''' Update Single Entry in [dbo].[ProfessionalVerification] For Export
    ''' </summary>
    ''' <param name="strEnrolRefNo"></param>
    ''' <param name="intProfSeq"></param>
    ''' <param name="strUserID"></param>
    ''' <param name="dtmImportDate"></param>
    ''' <param name="strResult"></param>
    ''' <param name="strRemark"></param>
    ''' <remarks>Record_Status='I' For Newly Import</remarks>
    Private Sub UpdateProfessionalVerificationImport(ByVal strEnrolRefNo As String, ByVal intProfSeq As Integer, ByVal strUserID As String, ByVal dtmImportDate As DateTime, ByVal strResult As String, ByVal strRemark As String, ByVal strRecordStatus As String, ByVal arrByteTSMP As Byte())
        Try
            Dim params() As SqlParameter = { _
                udtDB.MakeInParam("@Enrolment_Ref_No", ProfessionalVerificationModel.Enrolment_Ref_NoDataType, ProfessionalVerificationModel.Enrolment_Ref_NoDataSize, strEnrolRefNo), _
                udtDB.MakeInParam("@Professional_Seq", ProfessionalVerificationModel.Professional_SeqDataType, ProfessionalVerificationModel.Professional_SeqDataSize, intProfSeq), _
                udtDB.MakeInParam("@Import_By", ProfessionalVerificationModel.Import_ByDataType, ProfessionalVerificationModel.Import_ByDataSize, strUserID), _
                udtDB.MakeInParam("@Import_Dtm", ProfessionalVerificationModel.Import_DtmDataType, ProfessionalVerificationModel.Import_DtmDataSize, dtmImportDate), _
                udtDB.MakeInParam("@Result", ProfessionalVerificationModel.Verification_ResultDataType, ProfessionalVerificationModel.Verification_ResultDataSize, strResult), _
                udtDB.MakeInParam("@Remark", ProfessionalVerificationModel.Verification_RemarkDataType, ProfessionalVerificationModel.Verification_RemarkDataSize, strRemark), _
                udtDB.MakeInParam("@Record_Status", ProfessionalVerificationModel.Record_StatusDataType, ProfessionalVerificationModel.Record_StatusDataSize, IIf(strRecordStatus.Trim() = "", DBNull.Value, strRecordStatus.Trim())), _
                udtDB.MakeInParam("@tsmp", ProfessionalVerificationModel.TSMPDataType, ProfessionalVerificationModel.TSMPDataSize, arrByteTSMP) _
            }


            udtDB.RunProc(Me.strSP_UpdateProfessionalVerificationForImport, params)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Update Single Entry in [dbo].[ProfessionalVerification] For Defer
    ''' </summary>
    ''' <param name="strEnrolRefNo"></param>
    ''' <param name="intProfSeq"></param>
    ''' <param name="strUserId"></param>
    ''' <param name="dtmDeferDate"></param>
    ''' <param name="arrByteTSMP"></param>
    ''' <remarks></remarks>
    Private Sub UpdateProfessionalVerificationDefer(ByVal strEnrolRefNo As String, ByVal intProfSeq As Integer, ByVal strUserId As String, ByVal dtmDeferDate As DateTime, ByVal arrByteTSMP As Byte())
        Try
            Dim params() As SqlParameter = { _
                udtDB.MakeInParam("@Enrolment_Ref_No", ProfessionalVerificationModel.Enrolment_Ref_NoDataType, ProfessionalVerificationModel.Enrolment_Ref_NoDataSize, strEnrolRefNo), _
                udtDB.MakeInParam("@Professional_Seq", ProfessionalVerificationModel.Professional_SeqDataType, ProfessionalVerificationModel.Professional_SeqDataSize, intProfSeq), _
                udtDB.MakeInParam("@Defer_By", ProfessionalVerificationModel.Defer_ByDataType, ProfessionalVerificationModel.Defer_ByDataSize, strUserId), _
                udtDB.MakeInParam("@Defer_Dtm", ProfessionalVerificationModel.Defer_DtmDataType, ProfessionalVerificationModel.Defer_DtmDataSize, dtmDeferDate), _
                udtDB.MakeInParam("@tsmp", ProfessionalVerificationModel.TSMPDataType, ProfessionalVerificationModel.TSMPDataSize, arrByteTSMP) _
            }

            udtDB.RunProc(Me.strSP_UpdateProfessionalVerificationForDefer, params)
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    ''' <summary>
    ''' Update Singl Entry in [dbo].[ProfessionalVerification] For Accept
    ''' </summary>
    ''' <param name="strEnrolRefNo"></param>
    ''' <param name="intProfSeq"></param>
    ''' <param name="strUserId"></param>
    ''' <param name="dtmConfirmDate"></param>
    ''' <param name="arrByteTSMP"></param>
    ''' <remarks></remarks>
    Private Sub UpdateProfessionalVerificationAccept(ByVal strEnrolRefNo As String, ByVal intProfSeq As Integer, ByVal strUserId As String, ByVal dtmConfirmDate As DateTime, ByVal arrByteTSMP As Byte())
        Try
            Dim params() As SqlParameter = { _
                udtDB.MakeInParam("@Enrolment_Ref_No", ProfessionalVerificationModel.Enrolment_Ref_NoDataType, ProfessionalVerificationModel.Enrolment_Ref_NoDataSize, strEnrolRefNo), _
                udtDB.MakeInParam("@Professional_Seq", ProfessionalVerificationModel.Professional_SeqDataType, ProfessionalVerificationModel.Professional_SeqDataSize, intProfSeq), _
                udtDB.MakeInParam("@Confirm_By", ProfessionalVerificationModel.Confirm_ByDataType, ProfessionalVerificationModel.Confirm_ByDataSize, strUserId), _
                udtDB.MakeInParam("@Confirm_Dtm", ProfessionalVerificationModel.Confirm_DtmDataType, ProfessionalVerificationModel.Confirm_DtmDataSize, dtmConfirmDate), _
                udtDB.MakeInParam("@tsmp", ProfessionalVerificationModel.TSMPDataType, ProfessionalVerificationModel.TSMPDataSize, arrByteTSMP) _
            }

            udtDB.RunProc(Me.strSP_UpdateProfessionalVerificationForAccept, params)
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    ''' <summary>
    ''' [External] Update Singl Entry in [dbo].[ProfessionalVerification] For Reject
    ''' </summary>
    ''' <param name="strEnrolRefNo"></param>
    ''' <param name="intProfSeq"></param>
    ''' <param name="strUserId"></param>
    ''' <param name="dtmRejectDate"></param>
    ''' <param name="arrByteTSMP"></param>
    ''' <remarks></remarks>
    Public Sub UpdateProfessionalVerificationReject(ByRef udtDB As Database, ByVal strEnrolRefNo As String, ByVal intProfSeq As Integer, ByVal strUserId As String, ByVal dtmRejectDate As DateTime, ByVal arrByteTSMP As Byte())
        Try
            Dim params() As SqlParameter = { _
                udtDB.MakeInParam("@Enrolment_Ref_No", ProfessionalVerificationModel.Enrolment_Ref_NoDataType, ProfessionalVerificationModel.Enrolment_Ref_NoDataSize, strEnrolRefNo), _
                udtDB.MakeInParam("@Professional_Seq", ProfessionalVerificationModel.Professional_SeqDataType, ProfessionalVerificationModel.Professional_SeqDataSize, intProfSeq), _
                udtDB.MakeInParam("@Void_By", ProfessionalVerificationModel.Void_ByDataType, ProfessionalVerificationModel.Void_ByDataSize, strUserId), _
                udtDB.MakeInParam("@Void_Dtm", ProfessionalVerificationModel.Void_DtmDataType, ProfessionalVerificationModel.Void_DtmDataSize, dtmRejectDate), _
                udtDB.MakeInParam("@tsmp", ProfessionalVerificationModel.TSMPDataType, ProfessionalVerificationModel.TSMPDataSize, arrByteTSMP) _
            }

            udtDB.RunProc(Me.strSP_UpdateProfessionalVerificationForReject, params)
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    ''' <summary>
    ''' [External] Update Singl Entry in [dbo].[ProfessionalVerification] Status
    ''' </summary>
    ''' <param name="udtDB"></param>
    ''' <param name="strEnrolRefNo"></param>
    ''' <param name="intProfSeq"></param>
    ''' <param name="strUserId"></param>
    ''' <param name="strStatus"></param>
    ''' <param name="arrByteTSMP"></param>
    ''' <remarks></remarks>
    Public Sub UpdateProfessionalVerificationStatus(ByRef udtDB As Database, ByVal strEnrolRefNo As String, ByVal intProfSeq As Integer, ByVal strUserId As String, ByVal strStatus As String, ByVal arrByteTSMP As Byte())
        Try
            Dim params() As SqlParameter = { _
                udtDB.MakeInParam("@Enrolment_Ref_No", ProfessionalVerificationModel.Enrolment_Ref_NoDataType, ProfessionalVerificationModel.Enrolment_Ref_NoDataSize, strEnrolRefNo), _
                udtDB.MakeInParam("@Professional_Seq", ProfessionalVerificationModel.Professional_SeqDataType, ProfessionalVerificationModel.Professional_SeqDataSize, intProfSeq), _
                udtDB.MakeInParam("@Record_Status", ProfessionalVerificationModel.Record_StatusDataType, ProfessionalVerificationModel.Record_StatusDataSize, strStatus), _
                udtDB.MakeInParam("@tsmp", ProfessionalVerificationModel.TSMPDataType, ProfessionalVerificationModel.TSMPDataSize, arrByteTSMP) _
            }

            udtDB.RunProc(Me.strSP_UpdateProfessionalVerificationStatus, params)
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    ''' <summary>
    ''' [External] Delete Single Entry in [dbo].[ProfessionalVerification]
    ''' </summary>
    ''' <param name="udtDB"></param>
    ''' <param name="strEnrolmentRefNo"></param>
    ''' <param name="intProfSeq"></param>
    ''' <param name="TSMP"></param>
    ''' <param name="blnCheckTSMP"></param>
    ''' <remarks></remarks>
    Public Sub DeleteProfessionalVerification(ByRef udtDB As Database, ByVal strEnrolmentRefNo As String, ByVal intProfSeq As Integer, ByVal TSMP As Byte(), ByVal blnCheckTSMP As Boolean)
        Try
            Dim params() As SqlParameter = { _
                udtDB.MakeInParam("@Enrolment_Ref_No", ProfessionalVerificationModel.Enrolment_Ref_NoDataType, ProfessionalVerificationModel.Enrolment_Ref_NoDataSize, strEnrolmentRefNo), _
                udtDB.MakeInParam("@Professional_Seq", ProfessionalVerificationModel.Professional_SeqDataType, ProfessionalVerificationModel.Professional_SeqDataSize, intProfSeq), _
                udtDB.MakeInParam("@tsmp", ProfessionalVerificationModel.TSMPDataType, ProfessionalVerificationModel.TSMPDataSize, TSMP), _
                udtDB.MakeInParam("@checkTSMP", SqlDbType.TinyInt, 1, blnCheckTSMP)}

            udtDB.RunProc(Me.strSP_DeleteProfessionalVerification, params)

        Catch ex As Exception
            Throw ex
        End Try

    End Sub
#End Region

#Region "[dbo].[ProfessionalSubmissionHeader]"

    Public Function SearchProfessionalSubmissionHeader(ByVal strStatus As String, ByVal strProfessionCode As String, ByVal dtmExportFrom As Nullable(Of DateTime), ByVal dtmExportTo As Nullable(Of DateTime)) As DataTable

        Dim dtResult As New DataTable()
        Try
            ' Record from StoreProc
            ' [File_Name], [Export_Dtm], [Export_By], [Service_Category_Code], [Import_Dtm], [Import_By], Export_User, Import_User, Profession_Description

            If strStatus = Common.Component.ProfVRExportStatus.All Then
                strStatus = "All"
            End If

            Dim objFrom As Object = DBNull.Value
            Dim objTo As Object = DBNull.Value

            If dtmExportFrom.HasValue Then
                objFrom = dtmExportFrom.Value
            End If

            If dtmExportTo.HasValue Then
                objTo = dtmExportTo.Value
            End If


            Dim params() As SqlParameter = { _
                udtDB.MakeInParam("@Status", SqlDbType.VarChar, 20, strStatus), _
                udtDB.MakeInParam("@Service_Category_Code", ProfessionalSubmissionHeaderModel.Service_Category_CodeDataType, ProfessionalSubmissionHeaderModel.Service_Category_CodeDataSize, IIf(strProfessionCode Is Nothing, DBNull.Value, strProfessionCode)), _
                udtDB.MakeInParam("@Export_Dtm_From", ProfessionalSubmissionHeaderModel.Export_DtmDataType, ProfessionalSubmissionHeaderModel.Export_DtmDataSize, objFrom), _
                udtDB.MakeInParam("@Export_Dtm_To", ProfessionalSubmissionHeaderModel.Export_DtmDataType, ProfessionalSubmissionHeaderModel.Export_DtmDataSize, objTo) _
            }


            udtDB.RunProc(Me.strSP_SearchSubmissionHeader, params, dtResult)
        Catch ex As Exception
            Throw ex
        End Try

        Return dtResult
    End Function

    ''' <summary>
    ''' [Provide For UI] To Retrieve All ProfessionalSubmissionHeader
    ''' </summary>
    ''' <returns>DataTable[File_Name,Export_Dtm,Export_By,Service_Category_Code,Import_Dtm,Import_By], Export_User, Import_User, Profession_Description</returns>
    ''' <remarks>[File_Name,Export_Dtm,Export_By,Service_Category_Code,Import_Dtm,Import_By, Export_User, Import_User, Profession_Description]</remarks>
    Public Function GetProfessionalSubmissionHeader() As DataTable
        Dim dtResult As New DataTable

        Try
            udtDB.RunProc(Me.strSP_GetSubmissionHeader, dtResult)
        Catch ex As Exception
            Throw ex
        End Try

        ' Record from StoreProc
        ' [File_Name], [Export_Dtm], [Export_By], [Service_Category_Code], [Import_Dtm], [Import_By], Export_User, Import_User, Profession_Description

        dtResult.AcceptChanges()
        Return dtResult
    End Function

    ''' <summary>
    ''' Get Single Entry in [dbo].[ProfessionalSubmissionHeader]
    ''' </summary>
    ''' <param name="strFileName"></param>
    ''' <returns>ProfessionalSubmissionHeaderModel</returns>
    ''' <remarks></remarks>
    Public Function GetProfessionalSubmissionHeader(ByVal strFileName As String) As ProfessionalSubmissionHeaderModel

        Dim strServiceCategoryCode As String
        Dim strExportBy, strImportBy As String
        Dim dtmExport, dtmImport As Nullable(Of DateTime)

        Dim udtProfessionalSubmissionHeaderModel As ProfessionalSubmissionHeaderModel = Nothing

        Try
            Dim params() As SqlParameter = { _
                udtDB.MakeInParam("@File_Name", ProfessionalSubmissionHeaderModel.File_NameDataType, ProfessionalSubmissionHeaderModel.File_NameDataSize, strFileName) _
            }
            Dim dtResult As New DataTable
            Dim drRow As DataRow

            ' Record from StoreProc
            ' [File_Name], [Export_Dtm], [Export_By], [Service_Category_Code], [Import_Dtm], [Import_By]
            udtDB.RunProc(Me.strSP_GetSubmissionHeaderByKey, params, dtResult)

            If dtResult.Rows.Count > 0 Then
                drRow = dtResult.Rows(0)

                If IsDBNull(drRow("Export_Dtm")) Then dtmExport = Nothing Else dtmExport = Convert.ToDateTime(drRow("Export_Dtm"))
                If IsDBNull(drRow("Export_By")) Then strExportBy = Nothing Else strExportBy = drRow("Export_By").ToString().Trim()
                If IsDBNull(drRow("Service_Category_Code")) Then strServiceCategoryCode = Nothing Else strServiceCategoryCode = drRow("Service_Category_Code").ToString().Trim()
                If IsDBNull(drRow("Import_Dtm")) Then dtmImport = Nothing Else dtmImport = Convert.ToDateTime(drRow("Import_Dtm"))
                If IsDBNull(drRow("Import_By")) Then strImportBy = Nothing Else strImportBy = drRow("Import_By").ToString().Trim()

                udtProfessionalSubmissionHeaderModel = New ProfessionalSubmissionHeaderModel(strFileName, dtmExport, strExportBy, strServiceCategoryCode, dtmImport, strImportBy)
            End If
        Catch ex As Exception
            Throw ex
        End Try

        Return udtProfessionalSubmissionHeaderModel
    End Function

    ''' <summary>
    ''' Update Single Entry in [dbo].[ProfessionalSubmissionHeader] For Import
    ''' </summary>
    ''' <param name="strFileName">File Name</param>
    ''' <param name="strUserID">Import User</param>
    ''' <param name="dtmImportDtm">[Out]: Import DateTime</param>
    ''' <remarks></remarks>
    Private Sub UpdateProfessionalSubmissionHeader(ByVal strFileName As String, ByVal strUserID As String, ByRef dtmImportDtm As DateTime)
        Try
            Dim params() As SqlParameter = { _
             udtDB.MakeInParam("@File_Name", ProfessionalSubmissionHeaderModel.File_NameDataType, ProfessionalSubmissionHeaderModel.File_NameDataSize, strFileName), _
             udtDB.MakeInParam("@Import_By", ProfessionalSubmissionHeaderModel.Import_ByDataType, ProfessionalSubmissionHeaderModel.Import_ByDataSize, strUserID), _
             udtDB.MakeOutParam("@Import_Dtm", ProfessionalSubmissionHeaderModel.Import_DtmDataType, ProfessionalSubmissionHeaderModel.Import_DtmDataSize)}

            udtDB.RunProc(Me.strSP_UpdateSubmissionHeaderForImport, params)

            dtmImportDtm = Convert.ToDateTime(IIf(params(2).Value Is DBNull.Value, DateTime.MinValue, params(2).Value))

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' [Private] Update [dbo].[ProfessionalSubmissionHeader].File_Content with "File -> Byte()"
    ''' </summary>
    ''' <param name="strFileName"></param>
    ''' <param name="arrByteContent"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateProfessionalSubmissionHeaderFileContent(ByVal strFileName As String, ByVal arrByteContent As Byte()) As Boolean
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@File_Name", ProfessionalSubmissionHeaderModel.File_NameDataType, ProfessionalSubmissionHeaderModel.File_NameDataSize, strFileName), _
                    udtDB.MakeInParam("@File_Content", ProfessionalSubmissionHeaderModel.File_ContentDataType, ProfessionalSubmissionHeaderModel.File_ContentDataSize, arrByteContent)}

            udtDB.RunProc("proc_ProfessionalSubmissionHeader_upd_FileContent", prams)

            Return True
        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Insert Single Entry in [dbo].[ProfessionalSubmissionHeader]
    ''' </summary>
    ''' <param name="strProfessionCode"></param>
    ''' <param name="strUserID"></param>
    ''' <param name="strFileName">[Out]:Generated File Name</param>
    ''' <param name="dtmExportDtm">[Out]:Export DateTime</param>
    ''' <remarks></remarks>
    Private Sub InsertProfessionalSubmissionHeader(ByVal strProfessionCode As String, ByVal strUserID As String, ByRef strFileName As String, ByRef dtmExportDtm As DateTime)

        Try
            Dim params() As SqlParameter = { _
             udtDB.MakeInParam("@Profession_Code", ProfessionalSubmissionHeaderModel.Service_Category_CodeDataType, ProfessionalSubmissionHeaderModel.Service_Category_CodeDataSize, strProfessionCode), _
             udtDB.MakeInParam("@Create_By", ProfessionalSubmissionHeaderModel.Export_ByDataType, ProfessionalSubmissionHeaderModel.Export_ByDataSize, strUserID), _
             udtDB.MakeOutParam("@File_Name", ProfessionalSubmissionHeaderModel.File_NameDataType, ProfessionalSubmissionHeaderModel.File_NameDataSize), _
             udtDB.MakeOutParam("@Export_Dtm", ProfessionalSubmissionHeaderModel.Export_DtmDataType, ProfessionalSubmissionHeaderModel.Export_DtmDataSize)}

            udtDB.RunProc(Me.strSP_AddSubmissionHeader, params)

            strFileName = CStr(IIf(params(2).Value Is DBNull.Value, "", params(2).Value))
            dtmExportDtm = Convert.ToDateTime(IIf(params(3).Value Is DBNull.Value, DateTime.MinValue, params(3).Value))

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    ''' <summary>
    ''' Delete Single Entry in [dbo].[ProfessionalSubmissionHeader]
    ''' </summary>
    ''' <param name="strFileName"></param>
    ''' <remarks></remarks>
    Private Sub MarkDeleteProfessionalSubmissionHeader(ByVal strFileName As String)
        Try
            Dim params() As SqlParameter = { _
                udtDB.MakeInParam("@File_Name", ProfessionalSubmissionModel.File_NameDataType, ProfessionalSubmissionModel.File_NameDataSize, strFileName)}

            udtDB.RunProc(Me.strSP_UpdateSubmissionHeaderMarkDelete, params)
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "[dbo].[ProfessionalSubmission]"

    ''' <summary>
    ''' Get List of [dbo].[ProfessionalSubmission] File_Name
    ''' </summary>
    ''' <returns>ProfessionalSubmissionModelCollection</returns>
    ''' <remarks></remarks>
    Public Function GetProfessionalSubmissionRecordList(ByVal strFileName As String) As ProfessionalSubmissionModelCollection

        Dim udtPSMCollection As New ProfessionalSubmissionModelCollection()

        Dim intDisplaySeq As Integer
        Dim strReferenceNo, strRegistrationCode, strSPHKID, strSurname, strOtherName As String
        Dim udtProfessionalSubmissionModel As ProfessionalSubmissionModel = Nothing

        Try
            Dim dtResult As New DataTable()
            Dim params() As SqlParameter = {udtDB.MakeInParam("@File_Name", ProfessionalSubmissionModel.File_NameDataType, ProfessionalSubmissionModel.File_NameDataSize, strFileName)}

            ' Record from StoreProc
            ' File_Name, Reference_No, Display_Seq, Registration_Code, SP_HKID, Surname, Other_Name
            udtDB.RunProc(Me.strSP_GetSubmissionRecord, params, dtResult)

            For Each drRow As DataRow In dtResult.Rows

                strReferenceNo = drRow("Reference_No").ToString().Trim()
                intDisplaySeq = Convert.ToInt32(drRow("Display_Seq"))
                If IsDBNull(drRow("Registration_Code")) Then strRegistrationCode = Nothing Else strRegistrationCode = drRow("Registration_Code").ToString().Trim()
                If IsDBNull(drRow("SP_HKID")) Then strSPHKID = Nothing Else strSPHKID = drRow("SP_HKID").ToString().Trim()

                If IsDBNull(drRow("Surname")) Then strSurname = Nothing Else strSurname = drRow("Surname").ToString().Trim()
                If IsDBNull(drRow("Other_Name")) Then strOtherName = Nothing Else strOtherName = drRow("Other_Name").ToString().Trim()

                udtProfessionalSubmissionModel = New ProfessionalSubmissionModel(strFileName, strReferenceNo, intDisplaySeq, strRegistrationCode, strSPHKID, strSurname, strOtherName)
                udtPSMCollection.Add(udtProfessionalSubmissionModel)
            Next


        Catch ex As Exception
            Throw ex
        End Try

        Return udtPSMCollection
    End Function

    ''' <summary>
    ''' Insert Single Entry in [dbo].[ProfessionalSubmission]
    ''' </summary>
    ''' <param name="strFileName"></param>
    ''' <param name="strReferenceNo"></param>
    ''' <param name="intSeq"></param>
    ''' <param name="strRegCode"></param>
    ''' <param name="strHKID"></param>
    ''' <param name="strSurname"></param>
    ''' <param name="strOtherName"></param>
    ''' <remarks></remarks>
    Private Sub InsertProfessionalSubmissionRecord(ByVal strFileName As String, ByVal strReferenceNo As String, ByVal intSeq As Integer, ByVal strRegCode As String, ByVal strHKID As String, ByVal strSurname As String, ByVal strOtherName As String)

        Try
            Dim params() As SqlParameter = { _
             udtDB.MakeInParam("@File_Name", ProfessionalSubmissionModel.File_NameDataType, ProfessionalSubmissionModel.File_NameDataSize, strFileName), _
             udtDB.MakeInParam("@Reference_No", ProfessionalSubmissionModel.Reference_NoDataType, ProfessionalSubmissionModel.Reference_NoDataSize, strReferenceNo), _
             udtDB.MakeInParam("@Display_Seq", ProfessionalSubmissionModel.Display_SeqDataType, ProfessionalSubmissionModel.Display_SeqDataSize, intSeq), _
             udtDB.MakeInParam("@Registration_code", ProfessionalSubmissionModel.Registration_CodeDataType, ProfessionalSubmissionModel.Registration_CodeDataSize, strRegCode), _
             udtDB.MakeInParam("@SP_HKID", ProfessionalSubmissionModel.SP_HKIDDataType, ProfessionalSubmissionModel.SurnameDataSize, strHKID), _
             udtDB.MakeInParam("@Surname", ProfessionalSubmissionModel.SurnameDataType, ProfessionalSubmissionModel.SurnameDataSize, strSurname), _
             udtDB.MakeInParam("@Other_Name", ProfessionalSubmissionModel.Other_NameDataType, ProfessionalSubmissionModel.Other_NameDataSize, strOtherName)}

            udtDB.RunProc(Me.strSP_AddSubmissionRecord, params)

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    ''' <summary>
    ''' Delete Single Entry in [dbo].[ProfessionalSubmission]
    ''' </summary>
    ''' <param name="strFileName"></param>
    ''' <param name="strReferenceNo"></param>
    ''' <remarks></remarks>
    Private Sub DeleteProfessionalSubmissionRecord(ByVal strFileName As String, ByVal strReferenceNo As String)

        Try
            Dim params() As SqlParameter = { _
                udtDB.MakeInParam("@File_Name", ProfessionalSubmissionModel.File_NameDataType, ProfessionalSubmissionModel.File_NameDataSize, strFileName), _
                udtDB.MakeInParam("@Reference_No", ProfessionalSubmissionModel.Reference_NoDataType, ProfessionalSubmissionModel.Reference_NoDataSize, strReferenceNo)}

            udtDB.RunProc(Me.strSP_DeleteSubmissionRecord, params)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region

    ''' <summary>
    ''' Insert / Update Single Enry in [dbo].[ProfessionalSubmissionResult]
    ''' </summary>
    ''' <param name="strFileName"></param>
    ''' <param name="strReference"></param>
    ''' <param name="strResult"></param>
    ''' <param name="strRemark"></param>
    ''' <remarks></remarks>
    Private Sub ReplaceProfessionalSubmissionResult(ByVal strFileName As String, ByVal strReference As String, ByVal strResult As String, ByVal strRemark As String)
        Try
            '[INT14-0023] Fix HCVU cannot display imported BNC remark [Start][Karl]
            'Change from char(15) to varchar(50)
            'Dim params() As SqlParameter = { _
            '    udtDB.MakeInParam("@File_Name", SqlDbType.Char, 15, strFileName), _
            '[INT14-0023] Fix HCVU cannot display imported BNC remark [End][Karl]

            Dim params() As SqlParameter = { _
                udtDB.MakeInParam("@File_Name", SqlDbType.VarChar, 50, strFileName), _
                udtDB.MakeInParam("@Reference_No ", SqlDbType.Char, 20, strReference), _
                udtDB.MakeInParam("@Result", SqlDbType.Char, 1, strResult), _
                udtDB.MakeInParam("@Remark", SqlDbType.NVarChar, 70, strRemark)}
            udtDB.RunProc(Me.strSP_ReplaceSubmissionResult, params)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region

#Region "Supporting Function"

    ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

    ' -----------------------------------------------------------------------------------------

    ''' <summary>
    ''' Retrieve All Profession Code
    ''' </summary>
    ''' <returns>ArrayList of String</returns>
    ''' <remarks></remarks>
    Private Function RetrieveAllProfessionCode() As ArrayList

        Dim arrListCode As New ArrayList()

        Dim udtProfessionBLL As New ProfessionBLL
        Dim udtProfessionModelCollection As ProfessionModelCollection
        Dim udtProfessionModel As ProfessionModel
        udtProfessionModelCollection = udtProfessionBLL.GetProfessionList

        If (Not udtProfessionModelCollection Is Nothing) Then
            For Each udtProfessionModel In udtProfessionModelCollection
                arrListCode.Add(udtProfessionModel.ServiceCategoryCode.ToString())
            Next
        End If

        Return arrListCode

    End Function

    ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

    Private Sub SetRecordNum(ByRef dtTable As DataTable, ByVal strRecordField As String, ByVal blnAcceptChanges As Boolean)
        If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 Then
            If dtTable.Columns.Contains(strRecordField) Then
                Dim i As Integer = 0
                For Each drRow As DataRow In dtTable.Rows
                    i = i + 1
                    drRow.BeginEdit()
                    drRow(strRecordField) = i
                    drRow.EndEdit()
                Next
                If blnAcceptChanges Then dtTable.AcceptChanges()
            End If
        End If
    End Sub
#End Region

    Public Sub AddProfessionalVerificationWholeRecord(ByVal dtProfessionalVer As DataTable, ByVal udtDB As Database)
        Try
            Dim i As Integer
            If Not IsNothing(dtProfessionalVer) Then
                For i = 0 To dtProfessionalVer.Rows.Count - 1
                    Dim dr As DataRow = dtProfessionalVer.Rows(i)
                    Dim params() As SqlParameter = { _
                                    udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, dr("Enrolment_ref_no")), _
                                    udtDB.MakeInParam("@professional_seq", ProfessionalModel.ProfessionalSeqDataType, ProfessionalModel.ProfessionalSeqDataSize, dr("Professional_Seq")), _
                                    udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(dr("SP_ID").Equals(String.Empty), DBNull.Value, dr("SP_ID"))), _
                                    udtDB.MakeInParam("@Export_By", ProfessionalVerificationModel.Export_ByDataType, ProfessionalVerificationModel.Export_ByDataSize, IIf(dr("Export_By").Equals(String.Empty), DBNull.Value, dr("Export_By"))), _
                                    udtDB.MakeInParam("@Export_Dtm", ProfessionalVerificationModel.Export_DtmDataType, ProfessionalVerificationModel.Export_DtmDataSize, IIf(dr("Export_Dtm").Equals(String.Empty), DBNull.Value, dr("Export_Dtm"))), _
                                    udtDB.MakeInParam("@import_By", ProfessionalVerificationModel.Import_ByDataType, ProfessionalVerificationModel.Import_ByDataSize, IIf(dr("import_By").Equals(String.Empty), DBNull.Value, dr("import_By"))), _
                                    udtDB.MakeInParam("@import_Dtm", ProfessionalVerificationModel.Import_DtmDataType, ProfessionalVerificationModel.Import_DtmDataSize, IIf(dr("import_Dtm").Equals(String.Empty), DBNull.Value, dr("import_Dtm"))), _
                                    udtDB.MakeInParam("@verification_result", ProfessionalVerificationModel.Verification_ResultDataType, ProfessionalVerificationModel.Verification_ResultDataSize, IIf(dr("verification_result").Equals(String.Empty), DBNull.Value, dr("verification_result"))), _
                                    udtDB.MakeInParam("@verification_remark", ProfessionalVerificationModel.Verification_RemarkDataType, ProfessionalVerificationModel.Verification_RemarkDataSize, IIf(dr("verification_remark").Equals(String.Empty), DBNull.Value, dr("verification_remark"))), _
                                    udtDB.MakeInParam("@final_result", ProfessionalVerificationModel.Final_ResultDataType, ProfessionalVerificationModel.Final_ResultDataSize, IIf(dr("final_result").Equals(String.Empty), DBNull.Value, dr("final_result"))), _
                                    udtDB.MakeInParam("@confirm_By", ProfessionalVerificationModel.Export_ByDataType, ProfessionalVerificationModel.Export_ByDataSize, IIf(dr("confirm_By").Equals(String.Empty), DBNull.Value, dr("confirm_By"))), _
                                    udtDB.MakeInParam("@confirm_Dtm", ProfessionalVerificationModel.Export_DtmDataType, ProfessionalVerificationModel.Export_DtmDataSize, IIf(dr("confirm_Dtm").Equals(String.Empty), DBNull.Value, dr("confirm_Dtm"))), _
                                    udtDB.MakeInParam("@void_By", ProfessionalVerificationModel.Import_ByDataType, ProfessionalVerificationModel.Import_ByDataSize, IIf(dr("void_By").Equals(String.Empty), DBNull.Value, dr("void_By"))), _
                                    udtDB.MakeInParam("@void_Dtm", ProfessionalVerificationModel.Import_DtmDataType, ProfessionalVerificationModel.Import_DtmDataSize, IIf(dr("void_Dtm").Equals(String.Empty), DBNull.Value, dr("void_Dtm"))), _
                                    udtDB.MakeInParam("@defer_By", ProfessionalVerificationModel.Export_ByDataType, ProfessionalVerificationModel.Export_ByDataSize, IIf(dr("defer_By").Equals(String.Empty), DBNull.Value, dr("defer_By"))), _
                                    udtDB.MakeInParam("@defer_Dtm", ProfessionalVerificationModel.Export_DtmDataType, ProfessionalVerificationModel.Export_DtmDataSize, IIf(dr("defer_Dtm").Equals(String.Empty), DBNull.Value, dr("defer_Dtm"))), _
                                    udtDB.MakeInParam("@record_status", ProfessionalVerificationModel.Record_StatusDataType, ProfessionalVerificationModel.Record_StatusDataSize, IIf(dr("record_status").Equals(String.Empty), DBNull.Value, dr("record_status")))}

                    udtDB.RunProc("proc_ProfessionalVerification_add_wholeRecord", params)
                Next
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

End Class
