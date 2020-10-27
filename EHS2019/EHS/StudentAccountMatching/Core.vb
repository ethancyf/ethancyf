Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.Scheme
Imports Common.Component.StudentFile
Imports Common.DataAccess
Imports Common.Format
Imports StudentAccountMatching.AccountMatchingBLL
Imports System.Data.SqlClient
Imports Common.Component.FileGeneration

Module Core
    Sub Main(ByVal args() As String)
        Dim objScheduleJob As New ScheduleJob

        If ValidArg(args) Then
            objScheduleJob.Start(args)
        End If

    End Sub

    Function ValidArg(ByVal args() As String) As Boolean
        Dim blnIsValid As Boolean = True
        Dim strMode As String = String.Empty
        Dim strErrMsg As String = String.Empty

        If args.Length = 0 Then
            strErrMsg = "Please input mode"

        ElseIf args.Length = 1 Then
            strMode = args(0).Trim.ToUpper

            If strMode <> Mode.INITIAL AndAlso strMode <> Mode.RECHECK Then
                strErrMsg = String.Format("Invalid mode: {0}", strMode)
            End If
        Else
            strErrMsg = "Invalid mode"
        End If

        If strErrMsg <> String.Empty Then
            Console.WriteLine(strErrMsg)

            ' [INITIAL] 
            ' Execution peiod: Before ImmD
            ' Action: Search and Create Account
            ' AccProcessStage: {empty}

            ' [RECHECK] 
            ' Execution peiod: After ImmD
            ' Action: 
            ' (1) Update Temp Voucher Account once converted to Validated Account and update Student Entry
            ' (2) Update unmatch field for report
            ' AccProcessStage: {any}

            Console.WriteLine("==================================================")
            Console.WriteLine("Usage: StudentAccountMatching.exe [mode]")
            Console.WriteLine("mode = INITIAL / RECHECK")
            Console.WriteLine("==================================================")

            blnIsValid = False
        End If

        Return blnIsValid
    End Function
End Module

Public Class ScheduleJob
    Inherits CommonScheduleJob.BaseScheduleJob

#Region "Audit Log Description"
    Public Class AuditLogDesc

        Public Const ProcessStart_ID As String = LogID.LOG00001
        Public Const ProcessStart As String = "Student Account Matching Start"

        Public Const ProcessEnd_ID As String = LogID.LOG00002
        Public Const ProcessEnd As String = "Student Account Matching End"

        Public Const Exception_ID As String = LogID.LOG00003
        Public Const Exception As String = "Exception"

        Public Const RecheckTempAccountStart_ID As String = LogID.LOG00004
        Public Const RecheckTempAccountStart As String = "Batch check Temp Account Latest Result Start"

        Public Const RecheckTempAccountEnd_ID As String = LogID.LOG00005
        Public Const RecheckTempAccountEnd As String = "Batch check Temp Account Latest Result End"
    End Class

#End Region

#Region "Fields"
    Private udtStudentFileBLL As New StudentFileBLL
    Private udtEHSAccountBLL As New EHSAccountBLL
    Private udtAccountMatchingBLL As New AccountMatchingBLL
    Private udtGeneralFunction As New GeneralFunction

    Private _intStudent_Count As Integer = 0
    Private _intCreateAcctSuccess_Count As Integer = 0
    Private _intFail_Count As Integer = 0
    Private _intValidAcctFound_Count As Integer = 0
    Private _intRectifyAcct_Count As Integer = 0
    Private _intException_Count As Integer = 0

#End Region

#Region "Abstract Property of [CommonScheduleJob.BaseScheduleJob]"
    Protected Overrides ReadOnly Property FunctionCode() As String
        Get
            Return Common.Component.ScheduleJobFunctionCode.StudentAccountMatching
        End Get
    End Property
#End Region

#Region "Abstract Property of [CommonScheduleJob.BaseScheduleJob]"
    Public Overrides ReadOnly Property ScheduleJobID() As String
        Get
            Return Common.Component.ScheduleJobID.StudentAccountMatching
        End Get
    End Property
#End Region

#Region "Abstract Method of [CommonScheduleJob.BaseScheduleJob]"

    Protected Overrides Sub Process()

        ' -------------------------------------------------------------
        ' Retrieve Pending Record(s) from [StudentFileHeader] & [StudentFileHeaderStaging]
        ' -------------------------------------------------------------
        Try

            Dim strMode As String = Args(0).Trim.ToUpper
            Dim blnGetFromStaging As Boolean = False
            Dim blnGetFromPerm As Boolean = False
            ' CRE19-001 (VSS 2019) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Dim blnCheckAndUpdateTempVoucherAccount As Boolean = False
            ' CRE19-001 (VSS 2019) [End][Winnie]

            Select Case strMode
                Case Mode.INITIAL
                    blnGetFromStaging = True

                Case Mode.RECHECK
                    ' CRE19-001 (VSS 2019) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    blnCheckAndUpdateTempVoucherAccount = True
                    ' CRE19-001 (VSS 2019) [End][Winnie]
                    blnGetFromStaging = True
                    blnGetFromPerm = True

            End Select

            If blnCheckAndUpdateTempVoucherAccount Then
                CheckAndUpdateTempVoucherAccount()
            End If

            ' ==================================================================================
            ' Pick up the Student File need to process (Check Account before Report Generation)
            ' ==================================================================================

            ' CRE20-003-02 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
            ' -------------------------------------------------------------------------------
            ' Get Student File Header List from staging
            If blnGetFromStaging Then

                Dim lstStudentHeader As List(Of StudentFile.StudentFileHeaderModel) = udtAccountMatchingBLL.GetStudentFileHeaderAccountMatching(StudentFileBLL.StudentFileLocation.Staging)

                For intCount As Integer = 0 To lstStudentHeader.Count - 1
                    Dim udtStudentHeader As StudentFile.StudentFileHeaderModel = lstStudentHeader(intCount)
                    AccountMatching(strMode, udtStudentHeader, udtStudentHeader.RecordStatusEnum, StudentFileBLL.StudentFileLocation.Staging)
                Next

            End If


            ' Get Student File Header List from Perm (Same logic with "proc_StudentFileHeader_get_forVaccineEntitle")
            If blnGetFromPerm Then

                Dim lstStudentHeader As List(Of StudentFile.StudentFileHeaderModel) = udtAccountMatchingBLL.GetStudentFileHeaderAccountMatching(StudentFileBLL.StudentFileLocation.Permanence)

                For intCount As Integer = 0 To lstStudentHeader.Count - 1
                    Dim udtStudentHeader As StudentFile.StudentFileHeaderModel = lstStudentHeader(intCount)
                    AccountMatching(strMode, udtStudentHeader, udtStudentHeader.RecordStatusEnum, StudentFileBLL.StudentFileLocation.Permanence)
                Next
            End If
            ' CRE20-003-02 Enhancement on Programme or Scheme using batch upload [End][Winnie]

            ' End of Account Matching

            ConsoleLog("Account Matching is Completed.")

        Catch ex As Exception
            MyBase.AuditLog.AddDescripton("Message", ex.Message)
            MyBase.AuditLog.WriteLog(AuditLogDesc.Exception_ID, AuditLogDesc.Exception)
            Throw
        End Try

    End Sub

#End Region

#Region "Account Matching"

    ' CRE19-001 (VSS 2019) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Public Sub CheckAndUpdateTempVoucherAccount()

        ConsoleLog(AuditLogDesc.RecheckTempAccountStart)
        MyBase.AuditLog.WriteLog(AuditLogDesc.RecheckTempAccountStart_ID, AuditLogDesc.RecheckTempAccountStart)

        Try
            udtAccountMatchingBLL.CheckTempAccountLatestResult(AccProcessStage.RECHECKTEMPACCOUNT)

        Catch ex As Exception
            MyBase.AuditLog.AddDescripton("Message", ex.Message)
            MyBase.AuditLog.WriteLog(AuditLogDesc.Exception_ID, AuditLogDesc.Exception)
            ConsoleLog(String.Format("Error, Batch check Temp Account Latest Result Fail. Exception: {0}", ex.Message))

        End Try

        ConsoleLog(AuditLogDesc.RecheckTempAccountEnd)
        MyBase.AuditLog.WriteLog(AuditLogDesc.RecheckTempAccountEnd_ID, AuditLogDesc.RecheckTempAccountEnd)

    End Sub
    ' CRE19-001 (VSS 2019) [End][Winnie]

    Public Sub AccountMatching(ByVal strMode As String, _
                               ByVal udtStudentFileHeader As StudentFileHeaderModel, _
                               ByVal RecordStatusEnum As StudentFileHeaderModel.RecordStatusEnumClass, _
                               ByVal eStudentFileLocation As StudentFileBLL.StudentFileLocation)

        Dim udtFormatter As New Formatter
        Dim strCurrentAction As String = String.Empty
        Dim strFileID As String = udtStudentFileHeader.StudentFileID

        ConsoleLog(String.Format("Processing Student File ID: {0}, Status: {1}", _
                                 udtStudentFileHeader.StudentFileID, udtStudentFileHeader.RecordStatus))

        MyBase.AuditLog.AddDescripton("Student File ID", udtStudentFileHeader.StudentFileID)
        MyBase.AuditLog.WriteLog(AuditLogDesc.ProcessStart_ID, AuditLogDesc.ProcessStart)

        ' Get Student File Entry        
        Dim udtStudentFileEntryList As StudentFileEntryModelCollection
        udtStudentFileEntryList = udtAccountMatchingBLL.GetStudentFileEntryList(strFileID, eStudentFileLocation)

        Dim strAccProcessStage As String = String.Empty

        ' Reset count for each file id
        _intStudent_Count = 0
        _intValidAcctFound_Count = 0
        _intCreateAcctSuccess_Count = 0
        _intFail_Count = 0
        _intRectifyAcct_Count = 0
        _intException_Count = 0

        ' Handle student entry one by one
        For Each udtStudentFileEntry As StudentFileEntryModel In udtStudentFileEntryList

            Try
                ' -------------------------------------------------------------
                ' Filter Student by Account Process Stage based on the current mode
                ' -------------------------------------------------------------
                Dim udtStudent As StudentFileEntryModel = udtStudentFileEntry
                Dim udtStudentAmend As New StudentFileEntryModel

                Select Case strMode

                    ' MODE: INITIAL, Job run before ImmD
                    ' Only pick the student never check before, AccProcessStage = (EMPTY)
                    Case Mode.INITIAL

                        strAccProcessStage = AccProcessStage.INITIAL
                        If udtStudentFileEntry.AccProcessStage <> String.Empty Then
                            Continue For
                        End If

                        ' MODE: RECHECK, Job run after ImmD
                        ' Pick all status of [AccProcessStage]
                    Case Mode.RECHECK

                        strAccProcessStage = AccProcessStage.RECHECK

                        ' Skip checking if already rechecked on the same day
                        If udtStudentFileEntry.AccProcessStage = AccProcessStage.RECHECK Then

                            Dim dtmCurrent As Date = udtGeneralFunction.GetSystemDateTime
                            If dtmCurrent.Date <= udtStudentFileEntry.AccProcessStageDtm Then
                                Continue For
                            End If
                        End If

                End Select

                _intStudent_Count += 1


                Dim udtDB As New Database()

                Try
                    udtDB.BeginTransaction()

                    ' -------------------------------------------------------------
                    ' 1. Account Matching
                    ' -------------------------------------------------------------
                    Dim eVASearchResult As enumVAcctSearchResult = enumVAcctSearchResult.Not_Found
                    Dim blnCreateNewAcct As Boolean = False
                    Dim blnRemoveExistTempAccount As Boolean = False
                    Dim blnDirectUpdateExistingAccount As Boolean = False

                    Dim udtExistingTempAccount As EHSAccountModel = Nothing
                    Dim udtExistingTempPersonalInfo As EHSPersonalInformationModel = Nothing
                    Dim udtNewEHSAccount As New EHSAccountModel
                    Dim udtStudentPersonalInfo As New EHSPersonalInformationModel

                    strCurrentAction = "Matching Student Account"

                    ' Fill Personal Info from StudentFileEntry
                    udtAccountMatchingBLL.SetPersonalInfo(udtStudent, udtStudentAmend, udtNewEHSAccount)
                    udtStudentPersonalInfo = udtNewEHSAccount.EHSPersonalInformationList(0)


                    If udtStudent.AccProcessStage = String.Empty Then
                        ' AccProcessStage = NULL, first check after upload / rectify

                        ' -------------------------------------------------------------
                        ' 1. Find Validated Account
                        ' 2. Handle existing temp account created by program previously (Remove/Update)
                        ' -------------------------------------------------------------

                        ' 1. Find Validated Account
                        eVASearchResult = udtAccountMatchingBLL.IsVAcctExisted(udtStudentAmend, udtStudentPersonalInfo)

                        Select Case eVASearchResult
                            Case enumVAcctSearchResult.Exist
                                _intValidAcctFound_Count += 1

                            Case enumVAcctSearchResult.DOB_Not_Match, enumVAcctSearchResult.Not_Found
                                blnCreateNewAcct = True

                        End Select


                        ' 2. Handle existing temp account created by program previously
                        ' (1) Remove existing temp account if VAcct found or
                        ' (2) Reuse exist temp account instead of create new account
                        If udtStudent.TempVoucherAccID <> String.Empty Then
                            udtExistingTempAccount = udtEHSAccountBLL.LoadTempEHSAccountByVRID(udtStudent.TempVoucherAccID)

                            If udtExistingTempAccount IsNot Nothing Then
                                udtExistingTempPersonalInfo = udtExistingTempAccount.EHSPersonalInformationList(0)
                            End If
                        End If


                        If udtExistingTempAccount IsNot Nothing Then
                            ' With existing temp acct

                            ' =====================================================
                            ' For case below will not reuse/remove existing temp account:
                            ' (1) Account being "Validated"
                            ' (2) Account being "Removed"
                            ' (3) Account being "Validating"
                            ' (4) Account with transaction
                            ' =====================================================
                            If udtExistingTempAccount.RecordStatus <> TempAccountRecordStatusClass.Validated _
                                AndAlso udtExistingTempAccount.RecordStatus <> TempAccountRecordStatusClass.Removed _
                                AndAlso Not udtExistingTempPersonalInfo.Validating _
                                AndAlso udtExistingTempAccount.TransactionID = String.Empty Then

                                ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
                                'If eVASearchResult = enumVAcctSearchResult.Exist OrElse _
                                '    eVASearchResult = enumVAcctSearchResult.DOB_Not_Match Then
                                If eVASearchResult = enumVAcctSearchResult.Exist Then
                                    ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]
                                    blnRemoveExistTempAccount = True

                                Else
                                    blnDirectUpdateExistingAccount = True
                                    udtStudentPersonalInfo.TSMP = udtExistingTempPersonalInfo.TSMP
                                    blnCreateNewAcct = False
                                End If
                            End If
                        End If

                    Else
                        ' AccProcessStage = INITIAL / RECHECK / RECHECKTEMPACCOUNT
                        udtStudentAmend = udtStudent

                        ' Check temp acct created previously is converted to validated account
                        If udtStudentAmend.VoucherAccID = String.Empty AndAlso udtStudent.TempVoucherAccID <> String.Empty Then

                            ' Get existing temp acct directly
                            udtExistingTempAccount = udtEHSAccountBLL.LoadTempEHSAccountByVRID(udtStudent.TempVoucherAccID)

                            If udtExistingTempAccount IsNot Nothing Then
                                udtAccountMatchingBLL.convertTempAccountInfo(udtExistingTempAccount, udtStudentAmend)
                            End If

                        End If

                        ' Check matching field if the Validated acct existed
                        If udtStudentAmend.VoucherAccID <> String.Empty Then
                            ' Find Validated Account By Account ID

                            ' Get existing Validated Account with specific doc code
                            Dim udtValidatedAccount As EHSAccountModel = Nothing
                            Dim udtValidAccPersonalInfo As EHSPersonalInformationModel = Nothing

                            udtValidatedAccount = udtEHSAccountBLL.LoadEHSAccountByVRID(udtStudentAmend.VoucherAccID)
                            udtValidAccPersonalInfo = udtValidatedAccount.getPersonalInformation(udtStudentAmend.AccDocCode)

                            ' Check personal info fields different
                            Dim strUnmatchField As String = String.Empty
                            Dim blnCheckDocType As Boolean = False

                            ' Compare VA info and Student Entry Info
                            strUnmatchField = udtAccountMatchingBLL.CheckPersonalInfoMatch(blnCheckDocType, udtStudentPersonalInfo, udtValidAccPersonalInfo)

                            ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
                            ' -------------------------------------------------------------------------------
                            ' convert student account matching result
                            udtAccountMatchingBLL.convertValidatedAccountInfo(udtValidatedAccount, udtStudentAmend.AccDocCode, udtStudentAmend)

                            udtStudentAmend.ValidatedAccFound = YesNo.Yes
                            udtStudentAmend.ValidatedAccUnmatchResult = strUnmatchField
                            udtAccountMatchingBLL.setAccValidationResult(udtValidatedAccount, udtStudentAmend.AccDocCode, udtStudentAmend)
                            ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]

                            ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
                            ' -------------------------------------------------------------------------------
                        ElseIf udtExistingTempAccount IsNot Nothing Then
                            ' Find Validated Account by Doc No. 
                            Dim udtStudentCopy As New StudentFileEntryModel
                            Dim udtEHSAccountCopy As New EHSAccountModel
                            Dim eVASearchResultRecheck As enumVAcctSearchResult
                            udtAccountMatchingBLL.SetPersonalInfo(udtStudentAmend, udtStudentCopy, udtEHSAccountCopy)

                            ' Check field different between TA Info and VA Info
                            eVASearchResultRecheck = udtAccountMatchingBLL.IsVAcctExisted(udtStudentCopy, udtExistingTempAccount.EHSPersonalInformationList(0))

                            If Not eVASearchResultRecheck = enumVAcctSearchResult.Not_Found Then
                                udtStudentAmend.ValidatedAccFound = udtStudentCopy.ValidatedAccFound
                                udtStudentAmend.ValidatedAccUnmatchResult = udtStudentCopy.ValidatedAccUnmatchResult

                                udtAccountMatchingBLL.setAccValidationResult(udtExistingTempAccount, udtStudentAmend.AccDocCode, udtStudentAmend)
                            End If

                        End If
                        ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]
                    End If
                    ' ----- End Matching ------- '


                    ' ----- Start Update Account & Student File ------- '

                    ' -------------------------------------------------------------
                    ' 2.1 Remove Existing Temp Account
                    ' -------------------------------------------------------------
                    If blnRemoveExistTempAccount Then
                        strCurrentAction = "Remove Temp Account"
                        udtAccountMatchingBLL.RemoveTempAcct(udtExistingTempAccount, udtStudent.CreateBy, udtDB)
                    End If


                    ' -------------------------------------------------------------
                    ' 2.2 Create Temp Account
                    ' -------------------------------------------------------------
                    If blnCreateNewAcct Then
                        strCurrentAction = "Create Temp Account"

                        If udtAccountMatchingBLL.CreateTemporaryEHSAccount(udtNewEHSAccount, udtStudentAmend, udtStudentFileHeader, eVASearchResult, udtDB) Then
                            udtAccountMatchingBLL.convertTempAccountInfo(udtNewEHSAccount, udtStudentAmend)
                            _intCreateAcctSuccess_Count += 1
                        Else
                            _intFail_Count += 1
                        End If
                    End If

                    ' -------------------------------------------------------------
                    ' 2.3 Update existing temp account for rectification
                    ' -------------------------------------------------------------
                    If blnDirectUpdateExistingAccount Then
                        strCurrentAction = "Rectify Temp Account"

                        udtNewEHSAccount = New EHSAccountModel(udtExistingTempAccount)

                        ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                        If udtStudentFileHeader.SchemeCode = SchemeClaimModel.VSS And udtStudentFileHeader.SubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.VNIAMMR Then
                            udtNewEHSAccount.CreateByBO = True
                        Else
                            udtNewEHSAccount.CreateByBO = False
                        End If
                        ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

                        If udtAccountMatchingBLL.RectifyTemporaryEHSAccount(udtExistingTempAccount, udtNewEHSAccount, udtStudentAmend, udtDB) Then
                            udtAccountMatchingBLL.convertTempAccountInfo(udtNewEHSAccount, udtStudentAmend)
                            _intRectifyAcct_Count += 1
                        Else
                            _intFail_Count += 1
                        End If
                    End If

                    ' -------------------------------------------------------------
                    ' 3. Update [StudentFileEntry(Staging)] Section 2 - Account information
                    ' ------------------------------------------------------------- 
                    strCurrentAction = "Update StudentFileEntry"

                    ' Update Account Info with process stage
                    udtStudentAmend.AccProcessStage = strAccProcessStage
                    udtStudentFileBLL.UpdateStudentAccountInfo(udtStudentAmend, eStudentFileLocation, udtDB)

                    ' ----- End Update Account & Student File ------- '

                    udtDB.CommitTransaction()

                Catch eSQL As SqlException
                    udtDB.RollBackTranscation()
                    Throw New Exception(String.Format("Action ({0}) fail: {1}", strCurrentAction, eSQL.Message))
                Catch ex As Exception
                    udtDB.RollBackTranscation()
                    Throw New Exception(String.Format("Action ({0}) fail: {1}", strCurrentAction, ex.Message))
                End Try


            Catch ex As Exception
                MyBase.AuditLog.AddDescripton("Message", ex.Message)
                MyBase.AuditLog.AddDescripton("Exception on Student File ID", strFileID)
                MyBase.AuditLog.AddDescripton("Exception on Student Student Seq", udtStudentFileEntry.StudentSeq)
                MyBase.AuditLog.WriteLog(AuditLogDesc.Exception_ID, AuditLogDesc.Exception)

                ConsoleLog(String.Format("Error, Student({0}-{1}) is terminated to process. Exception: {2}", strFileID, udtStudentFileEntry.StudentSeq, ex.Message))

                _intException_Count += 1
                ' Continue process even any exception occurs
            End Try

        Next ' Run Next Student

        ' CRE19-001 (VSS 2019) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ConsoleLog(String.Format("No. of Student: {0}", _intStudent_Count))
        ConsoleLog(String.Format("No. of Validate account Found: {0}", _intValidAcctFound_Count))
        ConsoleLog(String.Format("No. of Temp account Created: {0}", _intCreateAcctSuccess_Count))
        ConsoleLog(String.Format("No. of Temp account Rectified: {0}", _intRectifyAcct_Count))
        ConsoleLog(String.Format("No. of Temp account Created/Rectified fail: {0}", _intFail_Count))
        ConsoleLog(String.Format("No. of Exception: {0}", _intException_Count))

        MyBase.AuditLog.AddDescripton("Student File ID", udtStudentFileHeader.StudentFileID)
        MyBase.AuditLog.AddDescripton("No. of Student", CStr(_intStudent_Count))
        MyBase.AuditLog.AddDescripton("No. of Validate account Found", CStr(_intValidAcctFound_Count))
        MyBase.AuditLog.AddDescripton("No. of Temp account Created", CStr(_intCreateAcctSuccess_Count))
        MyBase.AuditLog.AddDescripton("No. of Temp account Rectified", CStr(_intRectifyAcct_Count))
        MyBase.AuditLog.AddDescripton("No. of Temp account Created/Rectified fail", CStr(_intFail_Count))
        MyBase.AuditLog.AddDescripton("No. of Exception", CStr(_intException_Count))
        ' CRE19-001 (VSS 2019) [End][Winnie]

        MyBase.AuditLog.WriteLog(AuditLogDesc.ProcessEnd_ID, AuditLogDesc.ProcessEnd)
    End Sub

#End Region



#Region "Console Log"
    Public Shared Sub ConsoleLog(ByVal strText As String)

        Console.WriteLine("<" + Now.ToString("yyyy-MM-dd HH:mm:ss") + "> " + strText)

    End Sub
#End Region


End Class
