Imports Common.ComFunction
Imports Common.Component.Scheme
Imports Common.DataAccess
Imports Common.Format
Imports System.Data.SqlClient
Imports System.Data
Imports Common.Component
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.EHSTransaction
Imports Common.ComFunction.ParameterFunction



Namespace BLL

    Public Class StudentFileBLL

        Public Enum StudentFileLocation
            Staging
            Permanence
        End Enum

        Public Shared Sub ResetStudentFileEntryVaccineProcess()
            Dim udtDB As New Database
            Dim dt As New DataTable

            Try
                udtDB.BeginTransaction()
                udtDB.RunProc("proc_StudentFileEntryStaging_upd_ResetVaccinationProcess")
                udtDB.RunProc("proc_StudentFileEntry_upd_ResetVaccinationProcess")
                udtDB.CommitTransaction()
            Catch ex As Exception
                Try
                    udtDB.RollBackTranscation()
                Catch ex2 As Exception
                    ' Do nothing
                End Try
                Throw
            End Try
        End Sub

        ''' <summary>
        ''' Get student file header for processing vaccination check
        ''' </summary>
        ''' <returns>Collection of Student_File_ID</returns>
        ''' <remarks></remarks>
        Public Shared Function GetStudentFileHeaderVaccineCheck(ByVal eStudentFileLocation As StudentFileLocation) As List(Of StudentFile.StudentFileHeaderModel)
            Dim udtDB As New Database
            Dim dt As New DataTable

            Dim udtStudentBLL As New StudentFile.StudentFileBLL
            Dim lstStudentHeader As New List(Of StudentFile.StudentFileHeaderModel)

            Select Case eStudentFileLocation
                Case StudentFileLocation.Staging
                    udtDB.RunProc("proc_StudentFileHeaderStaging_get_forVaccineCheck", dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows
                            lstStudentHeader.Add(udtStudentBLL.GetStudentFileHeaderStaging(dr("Student_File_ID").ToString, False))
                        Next
                    End If
                Case StudentFileLocation.Permanence
                    udtDB.RunProc("proc_StudentFileHeader_get_forVaccineCheck", dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows
                            lstStudentHeader.Add(udtStudentBLL.GetStudentFileHeader(dr("Student_File_ID").ToString, False))
                        Next
                    End If
            End Select

            Return lstStudentHeader
        End Function


        ''' <summary>
        ''' Get student file header for processing vaccination Entitle
        ''' </summary>
        ''' <returns>Collection of Student_File_ID</returns>
        ''' <remarks></remarks>
        Public Shared Function GetStudentFileHeaderVaccineEntitle(ByVal eStudentFileLocation As StudentFileLocation) As List(Of StudentFile.StudentFileHeaderModel)
            Dim udtDB As New Database
            Dim dt As New DataTable

            Dim udtStudentBLL As New StudentFile.StudentFileBLL
            Dim lstStudentHeader As New List(Of StudentFile.StudentFileHeaderModel)

            Select Case eStudentFileLocation
                Case StudentFileLocation.Staging
                    udtDB.RunProc("proc_StudentFileHeaderStaging_get_forVaccineEntitle", dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows
                            lstStudentHeader.Add(udtStudentBLL.GetStudentFileHeaderStaging(dr("Student_File_ID").ToString, False))
                        Next
                    End If
                Case StudentFileLocation.Permanence
                    udtDB.RunProc("proc_StudentFileHeader_get_forVaccineEntitle", dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows
                            lstStudentHeader.Add(udtStudentBLL.GetStudentFileHeader(dr("Student_File_ID").ToString, False))
                        Next
                    End If
            End Select

            Return lstStudentHeader
        End Function

        ''' <summary>
        ''' Get student for enquiring HA or DH vaccination
        ''' </summary>
        ''' <param name="strStudentFileID"></param>
        ''' <param name="ProviderConnectionFailOnly">Empty string for all entries, HA for HA connection fail case only, or DH for DH connection fail case only</param>
        ''' <returns>Student with personal information</returns>
        ''' <remarks></remarks>
        Public Shared Function GetStudentFileEntryVaccineCheck(ByVal eStudentFileLocation As StudentFileLocation, _
                                                               ByVal strStudentFileID As String, _
                                                               ByVal ProviderConnectionFailOnly As String) As StudentModelCollection
            Dim cllnStudentModel As New StudentModelCollection

            Dim udtDB As New Database
            Dim dt As New DataTable

            Dim prams() As SqlParameter = { _
                        udtDB.MakeInParam("@StudentFileID", SqlDbType.VarChar, 15, strStudentFileID), _
                        udtDB.MakeInParam("@HA_Connection_Fail_Only", SqlDbType.Char, 1, IIf(ProviderConnectionFailOnly = EHSTransactionModel.VaccineRefType.HA, 1, 0)), _
                        udtDB.MakeInParam("@DH_Connection_Fail_Only", SqlDbType.Char, 1, IIf(ProviderConnectionFailOnly = EHSTransactionModel.VaccineRefType.DH, 1, 0))}


            Select Case eStudentFileLocation
                Case StudentFileLocation.Staging
                    udtDB.RunProc("proc_StudentFileEntryStaging_get_forVaccineCheck", prams, dt)
                Case StudentFileLocation.Permanence
                    udtDB.RunProc("proc_StudentFileEntry_get_forVaccineCheck", prams, dt)
            End Select

            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                    If IsDBNull(dr("Service_Receive_Dtm")) Then
                        dr("Service_Receive_Dtm") = dr("Service_Receive_Dtm_Header")
                    End If
                    ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

                    cllnStudentModel.Add(BuildStudentModel(dr))
                Next
            End If

            Return cllnStudentModel

        End Function

        Private Shared Function BuildStudentModel(ByVal dr As DataRow) As StudentModel
            Dim strSurName As String = String.Empty
            Dim strFirstName As String = String.Empty
            Dim strEName As String = dr("Eng_Name").ToString().Trim()
            Dim udtFormater As New Formatter()
            udtFormater.seperateEName(strEName, strSurName, strFirstName)

            Dim udtTempPersonalInformation As EHSPersonalInformationModel = Nothing

            If Not dr.IsNull("Voucher_Acc_ID") Then

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Add dummy [SmartID_Ver]
                udtTempPersonalInformation = New EHSPersonalInformationModel( _
                                        dr("Voucher_Acc_ID"), _
                                        dr("DOB"), _
                                        dr("Exact_DOB"), _
                                        dr("Sex"), _
                                        Nothing, _
                                        String.Empty, _
                                        String.Empty, _
                                        Nothing,
                                        String.Empty, _
                                        Nothing, _
                                        String.Empty, _
                                        String.Empty, _
                                        dr("Doc_No"), _
                                        strSurName, _
                                        strFirstName, _
                                        String.Empty, _
                                        String.Empty, _
                                        String.Empty, _
                                        String.Empty, _
                                        String.Empty, _
                                        String.Empty, _
                                        String.Empty, _
                                        Nothing, _
                                        IIf(dr.IsNull("EC_Serial_No"), String.Empty, dr("EC_Serial_No")), _
                                        String.Empty, _
                                        0, _
                                        Nothing, _
                                        dr("Doc_Code").ToString.Trim, _
                                        String.Empty, _
                                        Nothing, _
                                        String.Empty, _
                                        String.Empty, _
                                        IIf(dr.IsNull("EC_Serial_No"), True, False), _
                                        False, _
                                        String.Empty, _
                                        Nothing, _
                                        String.Empty, _
                                        String.Empty)
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
            End If

            Dim udtStudent As StudentModel = New StudentModel(dr("Student_File_ID"), _
                                                    dr("School_Code"), _
                                                    dr("SP_ID"), _
                                                    dr("Practice_Display_Seq"), _
                                                    IIf(dr.IsNull("Service_Receive_Dtm"), Nothing, dr("Service_Receive_Dtm")), _
                                                    dr("Scheme_Code"), _
                                                    dr("Scheme_Seq"), _
                                                    dr("Dose"), _
                                                    IIf(dr.IsNull("Claim_Upload_By"), String.Empty, dr("Claim_Upload_By")), _
                                                    dr("Student_Seq"), _
                                                    dr("Class_Name"), _
                                                    IIf(dr.IsNull("Acc_Process_Stage"), String.Empty, dr("Acc_Process_Stage")), _
                                                    IIf(dr.IsNull("Vaccination_Process_Stage"), String.Empty, dr("Vaccination_Process_Stage")), _
                                                    IIf(dr.IsNull("Entitle_ONLYDOSE"), String.Empty, dr("Entitle_ONLYDOSE")), _
                                                    IIf(dr.IsNull("Entitle_1STDOSE"), String.Empty, dr("Entitle_1STDOSE")), _
                                                    IIf(dr.IsNull("Entitle_2NDDOSE"), String.Empty, dr("Entitle_2NDDOSE")), _
                                                    IIf(dr.IsNull("Entitle_3RDDOSE"), String.Empty, dr("Entitle_3RDDOSE")), _
                                                    IIf(dr.IsNull("Entitle_Inject"), String.Empty, dr("Entitle_Inject")), _
                                                    IIf(dr.IsNull("Injected"), String.Empty, dr("Injected")), _
                                                    IIf(dr.IsNull("HA_Vaccine_Ref_Status"), String.Empty, dr("HA_Vaccine_Ref_Status")), _
                                                    IIf(dr.IsNull("DH_Vaccine_Ref_Status"), String.Empty, dr("DH_Vaccine_Ref_Status")), _
                                                    IIf(dr.IsNull("Acc_Type"), String.Empty, dr("Acc_Type")), _
                                                    IIf(dr.IsNull("HKIC_Symbol"), String.Empty, dr("HKIC_Symbol")), _
                                                    IIf(dr.IsNull("Acc_Record_Status"), String.Empty, dr("Acc_Record_Status")), _
                                                    udtTempPersonalInformation)

            Return udtStudent
        End Function

        ''' <summary>
        ''' Get student file header for processing vaccination claim
        ''' </summary>
        ''' <returns>Collection of Student_File_ID</returns>
        ''' <remarks></remarks>
        Public Shared Function GetStudentFileHeaderVaccineClaim() As List(Of StudentFile.StudentFileHeaderModel)
            Dim udtDB As New Database
            Dim dt As New DataTable

            Dim udtStudentBLL As New StudentFile.StudentFileBLL
            Dim lstStudentHeader As New List(Of StudentFile.StudentFileHeaderModel)

            udtDB.RunProc("proc_StudentFileHeaderStaging_get_forVaccineClaim", dt)

            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    lstStudentHeader.Add(udtStudentBLL.GetStudentFileHeaderStaging(dr("Student_File_ID").ToString, False))
                Next
            End If

            Return lstStudentHeader
        End Function

        ''' <summary>
        ''' Get student file entry for processing vaccination claim
        ''' </summary>
        ''' <param name="strStudentFileID"></param>
        ''' <returns>Student with personal information</returns>
        ''' <remarks></remarks>
        Public Shared Function GetStudentFileEntryVaccineClaim(ByVal strStudentFileID As String) As StudentModelCollection
            Dim cllnStudentModel As New StudentModelCollection

            Dim udtDB As New Database
            Dim dt As New DataTable

            Dim prams() As SqlParameter = { _
                        udtDB.MakeInParam("@StudentFileID", SqlDbType.VarChar, 15, strStudentFileID)}

            udtDB.RunProc("proc_StudentFileEntryStaging_get_forVaccineClaim", prams, dt)

            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                    If IsDBNull(dr("Service_Receive_Dtm")) Then
                        dr("Service_Receive_Dtm") = dr("Service_Receive_Dtm_Header")
                    End If
                    ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

                    cllnStudentModel.Add(BuildStudentModel(dr))
                Next
            End If

            Return cllnStudentModel

        End Function


        Public Shared Sub UpdateStudentFileEntryVaccineStaging(ByVal eStudentFileLocation As StudentFileLocation, _
                                                               ByVal strProvider As String, ByRef udtStudentModel As StudentModel, ByRef cllnTranVaccine As EHSTransaction.TransactionDetailVaccineModelCollection, ByVal strVaccineRefStatus As String)
            Dim udtDB As New Database
            Try
                udtDB.BeginTransaction()
                ' CRE19-001-04 (PPP 2019-20) [Start][Koala]
                If strProvider <> TransactionDetailVaccineModel.ProviderClass.Private Then
                    ' Update Vaccine Ref Status of HA and DH only
                    UpdateStudentFileEntry_VaccineRefStatus(eStudentFileLocation, strProvider, udtStudentModel, strVaccineRefStatus, udtDB)
                    DeleteStudentFileEntryVaccine(eStudentFileLocation, TransactionDetailVaccineModel.ProviderClass.RVP, udtStudentModel, udtDB)
                End If
                ' CRE19-001-04 (PPP 2019-20) [End][Koala]
                DeleteStudentFileEntryVaccine(eStudentFileLocation, strProvider, udtStudentModel, udtDB)
                InsertStudentFileEntryVaccine(eStudentFileLocation, udtStudentModel, cllnTranVaccine, udtDB)

                udtDB.CommitTransaction()
            Catch ex As Exception
                Try
                    udtDB.RollBackTranscation()
                Catch ex2 As Exception
                    ' Do nothing
                End Try
                Throw
            End Try
        End Sub

        Public Shared Sub UpdateStudentFileEntry_VaccineCheck(ByVal eStudentFileLocation As StudentFileLocation, _
                                                                        ByVal udtStudentHeaderModel As StudentFile.StudentFileHeaderModel, _
                                                                        ByVal udtStudentModel As StudentModel, _
                                                                        ByVal udtVaccineEntitle As VaccineEntitleModel, _
                                                                        Optional ByVal udtDB As Database = Nothing)
            If udtDB Is Nothing Then udtDB = New Database()


            Dim prams() As SqlParameter

            ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            If udtStudentHeaderModel.Precheck Then
                ' For RVP pre-check, all [StudentFileEntryStaging].[Entitle_XXX] value will be null.
                ' Actual subsidize entitlement will be stored in [StudentFileEntrySubsidizePrecheckStaging]
                prams = { _
                            udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, udtStudentModel.StudentFileID), _
                            udtDB.MakeInParam("@Student_Seq", SqlDbType.Int, 2, udtStudentModel.StudentSeq), _
                            udtDB.MakeInParam("@Entitle_ONLYDOSE", SqlDbType.Char, 1, DBNull.Value),
                            udtDB.MakeInParam("@Entitle_1STDOSE", SqlDbType.Char, 1, DBNull.Value), _
                            udtDB.MakeInParam("@Entitle_2NDDOSE", SqlDbType.Char, 1, DBNull.Value), _
                            udtDB.MakeInParam("@Entitle_3RDDOSE", SqlDbType.Char, 1, DBNull.Value), _
                            udtDB.MakeInParam("@Entitle_Inject", SqlDbType.Char, 1, DBNull.Value), _
                            udtDB.MakeInParam("@Entitle_Inject_Fail_Reason", SqlDbType.VarChar, 1000, DBNull.Value)}
            Else

                Dim objEntitle3rdDose As Object = Nothing

                If udtVaccineEntitle.Entitle3rdDose Is Nothing Then
                    objEntitle3rdDose = DBNull.Value
                Else
                    objEntitle3rdDose = IIf(udtVaccineEntitle.Entitle3rdDose, YesNo.Yes, YesNo.No)
                End If

                prams = { _
                            udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, udtStudentModel.StudentFileID), _
                            udtDB.MakeInParam("@Student_Seq", SqlDbType.Int, 2, udtStudentModel.StudentSeq), _
                            udtDB.MakeInParam("@Entitle_ONLYDOSE", SqlDbType.Char, 1, IIf(udtVaccineEntitle.EntitleOnlyDose, YesNo.Yes, YesNo.No)),
                            udtDB.MakeInParam("@Entitle_1STDOSE", SqlDbType.Char, 1, IIf(udtVaccineEntitle.Entitle1stDose, YesNo.Yes, YesNo.No)), _
                            udtDB.MakeInParam("@Entitle_2NDDOSE", SqlDbType.Char, 1, IIf(udtVaccineEntitle.Entitle2ndDose, YesNo.Yes, YesNo.No)), _
                            udtDB.MakeInParam("@Entitle_3RDDOSE", SqlDbType.Char, 1, objEntitle3rdDose), _
                            udtDB.MakeInParam("@Entitle_Inject", SqlDbType.Char, 1, IIf(udtVaccineEntitle.EntitleInject, YesNo.Yes, YesNo.No)), _
                            udtDB.MakeInParam("@Entitle_Inject_Fail_Reason", SqlDbType.VarChar, 1000, IIf(udtVaccineEntitle.EntitleInjectFailReason = String.Empty, DBNull.Value, udtVaccineEntitle.EntitleInjectFailReason))}
            End If
            ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

            Select Case eStudentFileLocation
                Case StudentFileLocation.Staging
                    udtDB.RunProc("proc_StudentFileEntryStaging_upd_VaccineCheck", prams)
                Case StudentFileLocation.Permanence
                    udtDB.RunProc("proc_StudentFileEntry_upd_VaccineCheck", prams)
            End Select
        End Sub

        Private Shared Sub UpdateStudentFileEntry_VaccineRefStatus(ByVal eStudentFileLocation As StudentFileLocation, _
                                                                          ByVal strProvider As String, ByRef udtStudentModel As StudentModel, ByVal strVaccineRefStatus As String, Optional ByVal udtDB As Database = Nothing)
            If udtDB Is Nothing Then udtDB = New Database()
            Dim prams() As SqlParameter = { _
                        udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, udtStudentModel.StudentFileID), _
                        udtDB.MakeInParam("@Student_Seq", SqlDbType.Int, 2, udtStudentModel.StudentSeq), _
                        udtDB.MakeInParam("@Provider", SqlDbType.VarChar, 100, strProvider),
                        udtDB.MakeInParam("@Vaccine_Ref_Status", SqlDbType.VarChar, 10, strVaccineRefStatus)}

            Select Case eStudentFileLocation
                Case StudentFileLocation.Staging
                    udtDB.RunProc("proc_StudentFileEntryStaging_upd_VaccineRefStatus", prams)
                Case StudentFileLocation.Permanence
                    udtDB.RunProc("proc_StudentFileEntry_upd_VaccineRefStatus", prams)
            End Select
        End Sub

        Public Shared Sub UpdateStudentFileEntryStaging_VaccineClaim(ByRef udtStudentModel As StudentModel, _
                                                                    ByVal strTransactionID As String, _
                                                                    ByVal strTransactionResult As String, _
                                                                    Optional ByVal udtDB As Database = Nothing)
            If udtDB Is Nothing Then udtDB = New Database()
            Dim prams() As SqlParameter = { _
                        udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, udtStudentModel.StudentFileID), _
                        udtDB.MakeInParam("@Student_Seq", SqlDbType.Int, 2, udtStudentModel.StudentSeq), _
                        udtDB.MakeInParam("@Transaction_ID", SqlDbType.Char, 20, IIf(strTransactionID = String.Empty, DBNull.Value, strTransactionID)),
                        udtDB.MakeInParam("@Transaction_Result", SqlDbType.VarChar, 1000, IIf(strTransactionResult = String.Empty, DBNull.Value, strTransactionResult))}
            udtDB.RunProc("proc_StudentFileEntryStaging_upd_VaccineClaim", prams)

        End Sub

        Public Shared Sub DeleteStudentFileEntryVaccine(ByVal eStudentFileLocation As StudentFileLocation, _
                                                                ByVal strProvider As String, ByRef udtStudentModel As StudentModel, Optional ByVal udtDB As Database = Nothing)
            If udtDB Is Nothing Then udtDB = New Database()
            Dim prams() As SqlParameter = { _
                        udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, udtStudentModel.StudentFileID), _
                        udtDB.MakeInParam("@Student_Seq", SqlDbType.Int, 2, udtStudentModel.StudentSeq), _
                        udtDB.MakeInParam("@Provider", SqlDbType.VarChar, 100, strProvider)}

            Select Case eStudentFileLocation
                Case StudentFileLocation.Staging
                    udtDB.RunProc("proc_StudentFileEntryVaccineStaging_del", prams)
                Case StudentFileLocation.Permanence
                    udtDB.RunProc("proc_StudentFileEntryVaccine_del", prams)
            End Select
        End Sub

        Private Shared Sub InsertStudentFileEntryVaccine(ByVal eStudentFileLocation As StudentFileLocation, _
                                                                ByRef udtStudentModel As StudentModel, ByRef cllnTranVaccine As EHSTransaction.TransactionDetailVaccineModelCollection, Optional ByVal udtDB As Database = Nothing)
            If udtDB Is Nothing Then udtDB = New Database()

            If cllnTranVaccine IsNot Nothing Then
                For intVaccineSeq As Integer = 0 To cllnTranVaccine.Count - 1

                    With cllnTranVaccine(intVaccineSeq)
                        Dim prams() As SqlParameter = { _
                            udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, udtStudentModel.StudentFileID), _
                            udtDB.MakeInParam("@Student_Seq", SqlDbType.Int, 2, udtStudentModel.StudentSeq), _
                            udtDB.MakeInParam("@Vaccine_Seq", SqlDbType.Int, 2, intVaccineSeq + 1), _
                            udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, .SchemeCode.Trim()), _
                            udtDB.MakeInParam("@Scheme_Seq", SqlDbType.SmallInt, 2, .SchemeSeq), _
                            udtDB.MakeInParam("@Subsidize_Code", SqlDbType.Char, 10, .SubsidizeCode), _
                            udtDB.MakeInParam("@Subsidize_Item_Code", SqlDbType.Char, 10, .SubsidizeItemCode), _
                            udtDB.MakeInParam("@Subsidize_Desc", SqlDbType.VarChar, 1000, .SubsidizeDesc), _
                            udtDB.MakeInParam("@Subsidize_Desc_Chi", SqlDbType.NVarChar, 200, .SubsidizeDescChi), _
                            udtDB.MakeInParam("@ForBar", SqlDbType.Bit, 1, .ForBar), _
                            udtDB.MakeInParam("@Available_item_Code", SqlDbType.Char, 20, .AvailableItemCode), _
                            udtDB.MakeInParam("@Available_Item_Desc", SqlDbType.VarChar, 1000, .AvailableItemDesc), _
                            udtDB.MakeInParam("@Available_Item_Desc_Chi", SqlDbType.NVarChar, 100, .AvailableItemDescChi), _
                            udtDB.MakeInParam("@Provider", SqlDbType.VarChar, 100, .Provider), _
                            udtDB.MakeInParam("@Service_Receive_Dtm", SqlDbType.DateTime, 8, .ServiceReceiveDtm), _
                            udtDB.MakeInParam("@Record_Type", SqlDbType.Char, 1, .RecordType), _
                            udtDB.MakeInParam("@Is_Unknown_Vaccine", SqlDbType.Bit, 1, .IsUnknownVaccine), _
                            udtDB.MakeInParam("@Practice_Name", SqlDbType.NVarChar, 100, .PracticeName), _
                            udtDB.MakeInParam("@Practice_Name_Chi", SqlDbType.NVarChar, 100, .PracticeNameChi)}

                        Select Case eStudentFileLocation
                            Case StudentFileLocation.Staging
                                udtDB.RunProc("proc_StudentFileEntryVaccineStaging_add", prams)
                            Case StudentFileLocation.Permanence
                                udtDB.RunProc("proc_StudentFileEntryVaccine_add", prams)
                        End Select
                    End With
                Next
            End If
        End Sub

        Public Shared Sub UpdateStudentFileHeaderStatus(ByVal strStudentFileID As String, ByVal eRecordStatus As StudentFile.StudentFileHeaderModel.RecordStatusEnumClass)
            Dim udtBLL As New StudentFile.StudentFileBLL
            Dim udtStudentFileHeader As StudentFile.StudentFileHeaderModel = Nothing
            Select Case eRecordStatus
                ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
                Case StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration,
                    StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]
                    udtStudentFileHeader = udtBLL.GetStudentFileHeaderStaging(strStudentFileID, False)
                    udtStudentFileHeader.RecordStatusEnum = eRecordStatus
                Case StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim
                    udtStudentFileHeader = udtBLL.GetStudentFileHeader(strStudentFileID, False)
                    udtStudentFileHeader.RecordStatusEnum = eRecordStatus

                    ' CRE19-001 (VSS 2019) [Start][Winnie]
                Case StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.ClaimSuspended, _
                    StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.Completed
                    ' CRE19-001 (VSS 2019) [End][Winnie]
                    udtStudentFileHeader = udtBLL.GetStudentFileHeaderStaging(strStudentFileID, False)
                    udtStudentFileHeader.RecordStatusEnum = eRecordStatus
            End Select

            Dim udtDB As New Database
            Try
                udtDB.BeginTransaction()

                Select Case eRecordStatus
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
                    Case StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration

                        ' eHSVF000 Report (RVP Precheck)
                        udtStudentFileHeader.VaccinationReportFileID = StudentFile.StudentFileBLL.SubmitReport(DataDownloadFileID.eHSVF000, udtStudentFileHeader, udtStudentFileHeader.UploadBy, udtDB).GenerationID
                        ' eHSVF006 Name list
                        udtStudentFileHeader.NameListFileID = StudentFile.StudentFileBLL.SubmitReport(DataDownloadFileID.eHSVF006, udtStudentFileHeader, udtStudentFileHeader.UploadBy, udtDB).GenerationID
                        ' eHSVF002 Report (clear old report)
                        udtStudentFileHeader.OnsiteVaccinationFileID = String.Empty

                        ' [Staging] Update header staging (and trigger loggging)
                        udtBLL.UpdateStudentFileHeaderStaging(udtStudentFileHeader, udtDB)
                        ' [Staging to Permanent ] Update header staging (and trigger loggging)
                        udtBLL.MoveStudentFileHeaderStaging(udtStudentFileHeader, udtDB)
                        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]

                    Case StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration
                        ' CRE19-001-04 (PPP 2019-20) [Start][Koala]
                        ' CRE18-010 (Adding one short form of student vaccination file upon the first upload) [Start][Koala]
                        ' eHSVF001 Report
                        udtStudentFileHeader.VaccinationReportFileID = StudentFile.StudentFileBLL.SubmitReport(DataDownloadFileID.eHSVF001, udtStudentFileHeader, udtStudentFileHeader.UploadBy, udtDB).GenerationID
                        ' eHSVF006 Name list
                        udtStudentFileHeader.NameListFileID = StudentFile.StudentFileBLL.SubmitReport(DataDownloadFileID.eHSVF006, udtStudentFileHeader, udtStudentFileHeader.UploadBy, udtDB).GenerationID
                        ' eHSVF002 Report (clear old report)
                        udtStudentFileHeader.OnsiteVaccinationFileID = String.Empty
                        '' eHSSF001 Report for back office 
                        ''udtStudentFileHeader.StudentReportFileID = SubmitReport("eHSSF001", udtStudentFileHeader, udtStudentFileHeader.UploadBy, udtDB).GenerationID
                        '' eHSSF001B Report for SP
                        'SubmitReport("eHSSF001B", udtStudentFileHeader, udtStudentFileHeader.UploadBy, udtDB)
                        ' CRE19-001-04 (PPP 2019-20) [End][Koala]

                        ' CRE18-010 (Adding one short form of student vaccination file upon the first upload) [End][Koala]
                        udtBLL.UpdateStudentFileHeaderStaging(udtStudentFileHeader, udtDB)
                        udtBLL.MoveStudentFileHeaderStaging(udtStudentFileHeader, udtDB)

                    Case StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim
                        ' No need move student file from staging to parmenance

                        ' CRE19-001-04 (PPP 2019-20) [Start][Koala]
                        ' eHSVF001 Report
                        udtStudentFileHeader.VaccinationReportFileID = StudentFile.StudentFileBLL.SubmitReport(DataDownloadFileID.eHSVF001, udtStudentFileHeader, udtStudentFileHeader.UploadBy, udtDB).GenerationID
                        ' eHSVF002 Report
                        udtStudentFileHeader.OnsiteVaccinationFileID = StudentFile.StudentFileBLL.SubmitReport(DataDownloadFileID.eHSVF002, udtStudentFileHeader, udtStudentFileHeader.UploadBy, udtDB).GenerationID

                        '' eHSSF002A Report for back office 
                        'udtStudentFileHeader.StudentReportFileID = SubmitReport("eHSSF002A", udtStudentFileHeader, udtStudentFileHeader.UploadBy, udtDB).GenerationID
                        '' eHSSF002B Report for SP
                        'SubmitReport("eHSSF002B", udtStudentFileHeader, udtStudentFileHeader.UploadBy, udtDB)
                        ' CRE19-001-04 (PPP 2019-20) [End][Koala]

                        udtBLL.UpdateStudentFileHeader(udtStudentFileHeader, udtDB)

                        ' CRE19-001 (VSS 2019) [Start][Winnie]
                    Case StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.ClaimSuspended, _
                        StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.Completed
                        ' CRE19-001 (VSS 2019) [End][Winnie]

                        ' CRE19-001-04 (PPP 2019-20) [Start][Koala]
                        Dim udtHCVUUserBLL As New HCVUUser.HCVUUserBLL
                        If udtHCVUUserBLL.IsExist(udtStudentFileHeader.ClaimUploadBy) Then
							' eHSVF003 Vaccination claim report
                            udtStudentFileHeader.ClaimCreationReportFileID = StudentFile.StudentFileBLL.SubmitReport(DataDownloadFileID.eHSVF003, udtStudentFileHeader, udtStudentFileHeader.ClaimUploadBy, udtDB).GenerationID
	                        ' eHSVF006 Name list
	                        udtStudentFileHeader.NameListFileID = StudentFile.StudentFileBLL.SubmitReport(DataDownloadFileID.eHSVF006, udtStudentFileHeader, udtStudentFileHeader.ClaimUploadBy, udtDB).GenerationID
                        Else
							' eHSVF003 Vaccination claim report
                            udtStudentFileHeader.ClaimCreationReportFileID = StudentFile.StudentFileBLL.SubmitReport(DataDownloadFileID.eHSVF003, udtStudentFileHeader, udtStudentFileHeader.UploadBy, udtDB).GenerationID
	                        ' eHSVF006 Name list
	                        udtStudentFileHeader.NameListFileID = StudentFile.StudentFileBLL.SubmitReport(DataDownloadFileID.eHSVF006, udtStudentFileHeader, udtStudentFileHeader.UploadBy, udtDB).GenerationID
                        End If
                        '' INT18-0022 (Fix users to download student claim file) [Start][Winnie]
                        'udtStudentFileHeader.StudentReportFileID = SubmitReport("eHSSF003", udtStudentFileHeader, udtStudentFileHeader.ClaimUploadBy, udtDB).GenerationID
                        ' CRE19-001-04 (PPP 2019-20) [End][Koala]

                        ' INT18-0022 (Fix users to download student claim file) [End][Winnie]
                        udtBLL.UpdateStudentFileHeaderStaging(udtStudentFileHeader, udtDB)
                        udtBLL.MoveStudentFileHeaderStaging(udtStudentFileHeader, udtDB)

                        ' CRE19-001 (VSS 2019 - Claim Creation) [Start][Winnie]
                        ' ----------------------------------------------------------------------------------------
                        ' Create 2nd Dose Vaccination File once 1st Dose claim is created
                        If udtStudentFileHeader.Dose = SchemeDetails.SubsidizeItemDetailsModel.DoseCode.FirstDOSE _
                            AndAlso udtStudentFileHeader.ServiceReceiveDtm2ndDose.HasValue Then

                            Create2ndDoseVaccinationFile(udtStudentFileHeader, udtDB)
                        End If

                        ' CRE19-001 (VSS 2019 - Claim Creation) [End][Winnie]
                End Select

                udtDB.CommitTransaction()
            Catch ex As Exception
                Try
                    udtDB.RollBackTranscation()
                Catch ex2 As Exception
                    ' Do nothing
                End Try
                Throw
            End Try
        End Sub

        ' CRE19-025 (Display of unmatched PV for batch upload under RVP) [Start][Winnie]
        Public Shared Function GetStudentFileEntryVaccine(ByVal eStudentFileLocation As StudentFileLocation, _
                                                          ByVal udtStudent As StudentModel, _
                                                          ByVal blnForBarOnly As Boolean) As TransactionDetailVaccineModelCollection
            ' CRE19-025 (Display of unmatched PV for batch upload under RVP) [End][Winnie]
            Dim cllnStudentModel As New StudentModelCollection

            Dim udtDB As New Database
            Dim dt As New DataTable

            Dim prams() As SqlParameter = { _
                        udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, udtStudent.StudentFileID), _
                        udtDB.MakeInParam("@Student_Seq", SqlDbType.Int, 2, udtStudent.StudentSeq)}

            Select Case eStudentFileLocation
                Case StudentFileLocation.Staging
                    udtDB.RunProc("proc_StudentFileEntryVaccineStaging_get", prams, dt)
                Case StudentFileLocation.Permanence
                    udtDB.RunProc("proc_StudentFileEntryVaccine_get", prams, dt)
            End Select

            Dim udtTranVaccine As TransactionDetailVaccineModel
            Dim cllnTranVaccine As New TransactionDetailVaccineModelCollection

            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    udtTranVaccine = New TransactionDetailVaccineModel( _
                                String.Empty, _
                                dr("Scheme_Code").ToString().Trim, _
                                CInt(dr("Scheme_Seq")), _
                                dr("Subsidize_Code").ToString().Trim, _
                                dr("Subsidize_Item_Code").ToString().Trim, _
                                dr("Available_Item_Code").ToString().Trim, _
                                0, _
                                0, _
                                0, _
                                String.Empty, _
                                dr("Service_Receive_Dtm"), _
                                dr("Available_Item_Desc").ToString(), _
                                dr("Available_Item_Desc_Chi").ToString())

                    udtTranVaccine.ServiceReceiveDtm = dr("Service_Receive_Dtm")
                    udtTranVaccine.SubsidizeDesc = dr("Subsidize_Desc").ToString()
                    udtTranVaccine.SubsidizeDescChi = dr("Subsidize_Desc_Chi").ToString()
                    udtTranVaccine.PracticeName = dr("Practice_Name").ToString()
                    udtTranVaccine.PracticeNameChi = dr("Practice_Name_Chi").ToString()
                    udtTranVaccine.DOB = udtStudent.PersonalInformation.DOB
                    udtTranVaccine.ExactDOB = udtStudent.PersonalInformation.ExactDOB
                    udtTranVaccine.IsUnknownVaccine = dr("Is_Unknown_Vaccine")
                    udtTranVaccine.RecordType = dr("Record_Type")
                    udtTranVaccine.ForBar = dr("ForBar")

                    ' CRE19-025 (Display of unmatched PV for batch upload under RVP) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------                    
                    If blnForBarOnly AndAlso udtTranVaccine.ForBar = False Then
                        Continue For
                    End If
                    ' CRE19-025 (Display of unmatched PV for batch upload under RVP) [End][Winnie]

                    cllnTranVaccine.Add(udtTranVaccine)
                Next
            End If

            Return cllnTranVaccine

        End Function

        ' CRE19-001 (VSS 2019 - Claim Creation) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Check Claim exceed Date back limit
        ''' </summary>
        ''' <param name="udtStudentFile"></param>
        ''' <returns></returns>
        ''' <remarks>Check period between vaccine date and claim upload date</remarks>
        Public Shared Function isExceedClaimPeriod(ByVal udtStudentFile As StudentFile.StudentFileHeaderModel) As Boolean
            Dim blnRes As Boolean = False
            Dim strClaimDayLimit As String = String.Empty

            Dim udtGenFunct As New GeneralFunction
            udtGenFunct.getSystemParameter("DateBackClaimDayLimit", strClaimDayLimit, String.Empty, udtStudentFile.SchemeCode)

            Dim intDayLimit As Integer = CInt(strClaimDayLimit)
            Dim dtmServiceDate As Date = CDate(udtStudentFile.ServiceReceiveDtm.Value).Date

            ' SP: The date SP press "Confirm"; VU: The date upload claim file
            Dim dtmConfirmClaimDate As Date = CDate(udtStudentFile.ClaimUploadDtm.Value).Date

            ' Check period between vaccine date and file confirm date
            If dtmServiceDate.AddDays(intDayLimit) <= dtmConfirmClaimDate Then
                blnRes = True
            End If

            Return blnRes

        End Function

        ''' <summary>
        ''' Create X EHSAccount
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <param name="strOrignalEHSAccountID"></param>
        ''' <param name="udtAccount"></param>
        ''' <param name="udtStudent"></param>
        ''' <param name="udtVaccineEntitle"></param>
        ''' <remarks></remarks>
        Public Sub CreateXEHSAccount(ByRef udtDB As Database, _
                                     ByVal strOrignalEHSAccountID As String, _
                                     ByRef udtAccount As EHSAccount.EHSAccountModel, _
                                     ByVal udtStudent As StudentModel, _
                                     ByVal udtVaccineEntitle As BLL.VaccineEntitleModel)

            ' Construct X EHS Account
            udtAccount = udtAccount.CloneData()
            udtAccount.OriginalAccID = strOrignalEHSAccountID
            udtAccount.VoucherAccID = (New GeneralFunction).generateSystemNum("X")
            udtAccount.DataEntryBy = String.Empty

            ' Record Status
            If udtAccount.RecordStatus <> EHSAccount.EHSAccountModel.TempAccountRecordStatusClass.NotForImmDValidation Then
                udtAccount.RecordStatus = EHSAccount.EHSAccountModel.TempAccountRecordStatusClass.PendingVerify
            End If

            udtAccount.EHSPersonalInformationList(0).DataEntryBy = String.Empty
            udtAccount.CreateSPID = udtStudent.ServiceProviderID
            udtAccount.CreateBy = udtStudent.ServiceProviderID

            udtAccount.EHSPersonalInformationList(0).RecordStatus = "N"
            udtAccount.EHSPersonalInformationList(0).CreateBy = udtStudent.ServiceProviderID
            udtAccount.EHSPersonalInformationList(0).CreateBySmartID = False
            udtAccount.EHSPersonalInformationList(0).SmartIDVer = String.Empty

            'X Account Practice ID and Scheme Code must same as transaction
            udtAccount.CreateSPPracticeDisplaySeq = udtVaccineEntitle.EHSTransaction.PracticeID
            udtAccount.SchemeCode = udtVaccineEntitle.EHSTransaction.SchemeCode

            ' Create X EHS Account
            Call (New EHSAccount.EHSAccountBLL).InsertEHSAccount_Core(udtDB, udtAccount)

        End Sub

        Public Shared Sub Create2ndDoseVaccinationFile(ByVal udtOriStudentFile As StudentFile.StudentFileHeaderModel, _
                                                       ByRef udtDB As Database)

            Dim udtStudentFileBLL As New StudentFile.StudentFileBLL
            Dim dtFullVaccFile As DataTable = Nothing

            dtFullVaccFile = udtStudentFileBLL.GetStudentFileEntryDT(udtOriStudentFile.StudentFileID, udtDB)

            Dim dtFiltered As DataTable = StudentFile.StudentFileBLL.GenerateStudentFileEntryDT
            Dim i As Integer = 1

            For Each dr As DataRow In dtFullVaccFile.Select(String.Empty, "Student_Seq")

                ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Dim blnEntitle2ndDose As Boolean = False

                If (Not IsDBNull(dr("Entitle_2NDDOSE")) AndAlso dr("Entitle_2NDDOSE") = YesNo.Yes) Then

                    ' RVP : Add student to new vaccination file for student entitle 2nd Dose
                    ' PPP/PPPKG : Add student to new vaccination file for student with success claim on 1st Dose and entitle 2nd Dose
                    If udtOriStudentFile.SchemeCode.Trim = SchemeClaimModel.RVP Then
                        blnEntitle2ndDose = True
                    Else
                        If (Not IsDBNull(dr("Transaction_ID")) AndAlso dr("Transaction_ID") <> String.Empty) Then
                            blnEntitle2ndDose = True
                        End If
                    End If

                End If

                If blnEntitle2ndDose Then
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]

                    Dim drFiltered As DataRow = dtFiltered.NewRow
                    ' Student Personal Info
                    drFiltered("Student_Seq") = i
                    drFiltered("Class_Name") = dr("Class_Name")
                    drFiltered("Class_No") = dr("Class_No")
                    drFiltered("Contact_No") = dr("Contact_No")
                    drFiltered("Name_CH") = dr("Name_CH")
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
                    drFiltered("Name_CH_Excel") = dr("Name_CH_Excel")
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]
                    drFiltered("Name_EN") = dr("Name_EN")
                    drFiltered("Surname_EN") = dr("Surname_EN")
                    drFiltered("Given_Name_EN") = dr("Given_Name_EN")
                    drFiltered("Sex") = dr("Sex")
                    drFiltered("DOB") = dr("DOB")
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
                    drFiltered("Exact_DOB") = dr("Exact_DOB")
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]
                    drFiltered("Doc_Code") = dr("Doc_Code")
                    drFiltered("Doc_No") = dr("Doc_No")
                    drFiltered("Date_of_Issue") = dr("Date_of_Issue")
                    drFiltered("Permit_To_Remain_Until") = dr("Permit_To_Remain_Until")
                    drFiltered("Foreign_Passport_No") = dr("Foreign_Passport_No")
                    drFiltered("EC_Serial_No") = dr("EC_Serial_No")
                    drFiltered("EC_Reference_No") = dr("EC_Reference_No")
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
                    drFiltered("EC_Reference_No_Other_Format") = dr("EC_Reference_No_Other_Format")
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]
                    drFiltered("Reject_Injection") = dr("Reject_Injection")
                    drFiltered("Upload_Warning") = dr("Upload_Warning")

                    ' Account Info, retain the same account for new file
                    drFiltered("Acc_Process_Stage") = dr("Acc_Process_Stage")
                    drFiltered("Acc_Process_Stage_Dtm") = dr("Acc_Process_Stage_Dtm")
                    drFiltered("Voucher_Acc_ID") = dr("Voucher_Acc_ID")
                    drFiltered("Temp_Voucher_Acc_ID") = dr("Temp_Voucher_Acc_ID")
                    drFiltered("Acc_Type") = dr("Acc_Type")
                    drFiltered("Acc_Doc_Code") = dr("Acc_Doc_Code")
                    drFiltered("Temp_Acc_Record_Status") = dr("Temp_Acc_Record_Status")
                    drFiltered("Temp_Acc_Validate_Dtm") = dr("Temp_Acc_Validate_Dtm")
                    drFiltered("Acc_Validation_Result") = dr("Acc_Validation_Result")
                    drFiltered("Validated_Acc_Found") = dr("Validated_Acc_Found")
                    drFiltered("Validated_Acc_Unmatch_Result") = dr("Validated_Acc_Unmatch_Result")

                    drFiltered("Create_By") = udtOriStudentFile.UploadBy
                    drFiltered("Create_Dtm") = Now()
                    drFiltered("Update_By") = udtOriStudentFile.UploadBy
                    drFiltered("Update_Dtm") = Now()

                    drFiltered("Original_Student_File_ID") = dr("Student_File_ID")
                    drFiltered("Original_Student_Seq") = dr("Student_Seq")

                    dtFiltered.Rows.Add(drFiltered)
                    i += 1

                End If
            Next

            If dtFiltered.Rows.Count > 0 Then

                ' Import new student file
                Dim udtNewStudentFile As New StudentFile.StudentFileHeaderModel
                ' StudentFileHeader
                udtNewStudentFile.StudentFileID = (New GeneralFunction).GenerateStudentFileID

                udtNewStudentFile.SchemeCode = udtOriStudentFile.SchemeCode
                udtNewStudentFile.SubsidizeCode = udtOriStudentFile.SubsidizeCode
                udtNewStudentFile.SchemeSeq = udtOriStudentFile.SchemeSeq
                udtNewStudentFile.Precheck = udtOriStudentFile.Precheck
                udtNewStudentFile.SchoolCode = udtOriStudentFile.SchoolCode
                udtNewStudentFile.SPID = udtOriStudentFile.SPID
                udtNewStudentFile.PracticeDisplaySeq = udtOriStudentFile.PracticeDisplaySeq

                udtNewStudentFile.Dose = SchemeDetails.SubsidizeItemDetailsModel.DoseCode.SecondDOSE
                udtNewStudentFile.ServiceReceiveDtm = udtOriStudentFile.ServiceReceiveDtm2ndDose
                udtNewStudentFile.FinalCheckingReportGenerationDate = udtOriStudentFile.FinalCheckingReportGenerationDate2ndDose

                ' Consider new file is upload confirmed and pending for checking
                udtNewStudentFile.RecordStatusEnum = StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Upload
                udtNewStudentFile.UploadBy = udtOriStudentFile.UploadBy
                udtNewStudentFile.UploadDtm = udtOriStudentFile.UploadDtm
                udtNewStudentFile.FileConfirmBy = udtOriStudentFile.FileConfirmBy
                udtNewStudentFile.FileConfirmDtm = udtOriStudentFile.FileConfirmDtm

                udtNewStudentFile.UpdateBy = udtOriStudentFile.UpdateBy
                udtNewStudentFile.UpdateDtm = udtOriStudentFile.UpdateDtm

                udtNewStudentFile.OriginalStudentFileID = udtOriStudentFile.StudentFileID

                ' Insert Vaccination File to Staging                    
                udtStudentFileBLL.InsertStudentFileStaging(udtNewStudentFile, dtFiltered, udtDB)
            End If

        End Sub
        ' CRE19-001 (VSS 2019 - Claim Creation) [End][Winnie]

        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]

        Public Shared Sub DeleteStudentFileEntrySubsidizePrecheck(ByRef udtStudentModel As StudentModel, Optional ByVal udtDB As Database = Nothing)
            If udtDB Is Nothing Then udtDB = New Database()
            Dim prams() As SqlParameter = { _
                        udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, udtStudentModel.StudentFileID), _
                        udtDB.MakeInParam("@Student_Seq", SqlDbType.Int, 2, udtStudentModel.StudentSeq)}


            udtDB.RunProc("proc_StudentFileEntrySubsidizePrecheckStaging_del", prams)
        End Sub

        Public Shared Sub InsertStudentFileEntrySubsidizePrecheck(ByRef udtStudentModel As StudentModel, ByVal udtSubsidizeGroupClaim As SubsidizeGroupClaimModel, _
                                                                   ByVal udtVaccineEntitle As VaccineEntitleModel, Optional ByVal udtDB As Database = Nothing)
            If udtDB Is Nothing Then udtDB = New Database()

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, udtStudentModel.StudentFileID), _
                udtDB.MakeInParam("@Student_Seq", SqlDbType.Int, 2, udtStudentModel.StudentSeq), _
                udtDB.MakeInParam("@Class_Name", SqlDbType.Char, 40, udtStudentModel.ClassName.Trim()), _
                udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, udtSubsidizeGroupClaim.SchemeCode.Trim()), _
                udtDB.MakeInParam("@Scheme_Seq", SqlDbType.SmallInt, 2, udtSubsidizeGroupClaim.SchemeSeq), _
                udtDB.MakeInParam("@Subsidize_Code", SqlDbType.Char, 10, udtSubsidizeGroupClaim.SubsidizeCode), _
                udtDB.MakeInParam("@Entitle_ONLYDOSE", SqlDbType.Char, 1, IIf(udtVaccineEntitle.EntitleOnlyDose, YesNo.Yes, YesNo.No)), _
                udtDB.MakeInParam("@Entitle_1STDOSE", SqlDbType.Char, 1, IIf(udtVaccineEntitle.Entitle1stDose, YesNo.Yes, YesNo.No)), _
                udtDB.MakeInParam("@Entitle_2NDDOSE", SqlDbType.Char, 1, IIf(udtVaccineEntitle.Entitle2ndDose, YesNo.Yes, YesNo.No)), _
                udtDB.MakeInParam("@Remark_ONLYDOSE", SqlDbType.VarChar, 1000, IIf(String.IsNullOrEmpty(udtVaccineEntitle.RemarkOnlyDose), DBNull.Value, udtVaccineEntitle.RemarkOnlyDose)), _
                udtDB.MakeInParam("@Remark_1STDOSE", SqlDbType.VarChar, 1000, IIf(String.IsNullOrEmpty(udtVaccineEntitle.Remark1stDose), DBNull.Value, udtVaccineEntitle.Remark1stDose)), _
                udtDB.MakeInParam("@Remark_2NDDOSE", SqlDbType.VarChar, 1000, IIf(String.IsNullOrEmpty(udtVaccineEntitle.Remark2ndDose), DBNull.Value, udtVaccineEntitle.Remark2ndDose)), _
                udtDB.MakeInParam("@Entitle_Inject_Fail_Reason", SqlDbType.VarChar, 1000, udtVaccineEntitle.EntitleInjectFailReason)}


            udtDB.RunProc("proc_StudentFileEntrySubsidizePrecheckStaging_add", prams)

        End Sub
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]
    End Class
End Namespace
