Imports Common.Component
Imports System.Data
Imports System.Data.SqlClient

Namespace Component

    Public MustInherit Class Status

        Shared Function GetStatusCache() As DataTable
            Dim dt As New DataTable
            Dim db As New DataAccess.Database

            If HttpContext.Current.Cache.Get("StatusData") Is Nothing Then
                Try
                    ' run the stored procedure
                    db.RunProc("proc_StatusData_get_cache", dt)
                Catch ex As Exception
                    'ErrorHandler.Log(ex)
                    Throw ex
                End Try

                'HttpContext.Current.Cache.Insert("Status", dt)
                Common.ComObject.CacheHandler.InsertCache("StatusData", dt)
            Else
                dt = CType(HttpContext.Current.Cache.Get("StatusData"), DataTable)
            End If

            Return dt
        End Function

        Shared Function GetStatusAllCache() As DataTable
            Dim dt As New DataTable
            Dim db As New DataAccess.Database

            If HttpContext.Current.Cache.Get("StatusDataAll") Is Nothing Then
                Try
                    ' run the stored procedure
                    db.RunProc("proc_StatusData_get_all_cache", dt)
                Catch ex As Exception
                    'ErrorHandler.Log(ex)
                    Throw ex
                End Try

                'HttpContext.Current.Cache.Insert("Status", dt)
                Common.ComObject.CacheHandler.InsertCache("StatusDataAll", dt)
            Else
                dt = CType(HttpContext.Current.Cache.Get("StatusDataAll"), DataTable)
            End If

            Return dt
        End Function

        'Public MustOverride Function GetDescriptionFromDBCode(ByVal strCode As String) As String

        Shared Function GetDescriptionListFromDBEnumCode(ByVal strEnumCode As String, ByVal blnTrim As Boolean) As DataTable

            Dim dtCache As DataTable = GetStatusCache()
            Dim dtResult As DataTable = dtCache.Clone

            Dim i As Integer = 0

            Dim drsCache As DataRow() = dtCache.Select("Enum_class='" + strEnumCode + "'", "Display_Order ASC")

            For i = 0 To drsCache.Length - 1
                Dim drRow As DataRow = dtResult.NewRow()
                Dim j As Integer = 0
                For j = 0 To dtCache.Columns.Count - 1
                    If blnTrim AndAlso dtCache.Columns(j).DataType Is GetType(String) Then
                        drRow(j) = drsCache(i)(j).ToString().Trim()
                    Else
                        drRow(j) = drsCache(i)(j)
                    End If
                Next
                dtResult.Rows.Add(drRow)
            Next
            dtResult.AcceptChanges()

            Return dtResult

        End Function

        Shared Function GetDescriptionAllFromDBCode(ByVal strClassCode As String, ByVal strStatusValue As String, ByRef strEngDesc As String, ByRef strChiDesc As String, Optional ByRef strCNDesc As String = "") As DataTable
            Dim strDesc As String = ""
            Dim dt As DataTable = GetStatusAllCache()
            Dim dtRes As DataTable = dt.Clone

            Dim i As Integer = 0

            strEngDesc = ""
            strChiDesc = ""

            While i < dt.Rows.Count
                If Trim(dt.Rows(i)("Enum_class")).Equals(strClassCode) And Trim(dt.Rows(i)("Status_Value").ToString).Equals(Trim(strStatusValue)) Then
                    strEngDesc = dt.Rows(i)("Status_Description").ToString
                    strChiDesc = dt.Rows(i)("Status_Description_Chi").ToString
                    strCNDesc = dt.Rows(i)("Status_Description_CN").ToString

                    Dim drRow As DataRow = dtRes.NewRow()
                    For intCt As Integer = 0 To dt.Columns.Count - 1
                        drRow.Item(intCt) = dt.Rows(i).Item(intCt)
                    Next
                    dtRes.Rows.Add(drRow)
                End If
                i = i + 1
            End While

            Return dtRes

        End Function

        Shared Sub GetDescriptionFromDBCode(ByVal strClassCode As String, ByVal strStatusValue As String, ByRef strEngDesc As String, ByRef strChiDesc As String, Optional ByRef strCNDesc As String = "")
            Dim strDesc As String = ""
            Dim dt As DataTable
            dt = GetStatusCache()

            Dim i As Integer = 0

            strEngDesc = ""
            strChiDesc = ""

            While i < dt.Rows.Count
                If Trim(dt.Rows(i)("Enum_class")).Equals(strClassCode) And Trim(dt.Rows(i)("Status_Value").ToString).Equals(Trim(strStatusValue)) Then
                    strEngDesc = dt.Rows(i)("Status_Description").ToString
                    strChiDesc = dt.Rows(i)("Status_Description_Chi").ToString
                    strCNDesc = dt.Rows(i)("Status_Description_CN").ToString
                End If
                i = i + 1
            End While
        End Sub

        Shared Function GetDescriptionAllListFromDBEnumCode(ByVal strEnumCode As String) As DataTable

            Dim dtCache As DataTable = GetStatusAllCache()
            Dim dtResult As DataTable = dtCache.Clone

            Dim i As Integer = 0

            Dim drsCache As DataRow() = dtCache.Select("Enum_class='" + strEnumCode + "'", "Display_Order ASC")

            For i = 0 To drsCache.Length - 1
                Dim drRow As DataRow = dtResult.NewRow()
                Dim j As Integer = 0
                For j = 0 To dtCache.Columns.Count - 1
                    drRow(j) = drsCache(i)(j)
                Next
                dtResult.Rows.Add(drRow)
            Next

            Return dtResult
        End Function

        Shared Function GetDescriptionListFromDBEnumCode(ByVal strEnumCode As String) As DataTable

            Dim dtCache As DataTable = GetStatusCache()
            Dim dtResult As DataTable = dtCache.Clone

            Dim i As Integer = 0

            Dim drsCache As DataRow() = dtCache.Select("Enum_class='" + strEnumCode + "'", "Display_Order ASC")

            For i = 0 To drsCache.Length - 1
                Dim drRow As DataRow = dtResult.NewRow()
                Dim j As Integer = 0
                For j = 0 To dtCache.Columns.Count - 1
                    drRow(j) = drsCache(i)(j)
                Next
                dtResult.Rows.Add(drRow)
            Next

            Return dtResult
        End Function
    End Class

    Public Class PracticeStatus
        Public Const Active As String = "A"
        Public Const Suspended As String = "S"
        Public Const Delisted As String = "D"

        Public Const ClassCode As String = "PracticeStatus"
    End Class

    Public Class BankAccountStatus
        Public Const Active As String = "A"
        Public Const Suspended As String = "S"
        Public Const Delisted As String = "D"

        Public Const ClassCode As String = "BankAccountStatus"
    End Class

    Public Class VRAcctStatus
        Public Const Active As String = "A"
        Public Const Suspended As String = "S"
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]        
        ' -----------------------------------------------------------------------------------------
        Public Const Terminated As String = "D" ' Revise Description "Deceased" -> "Terminated"
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
        Public Const ClassCode As String = "VRAcctStatus"
    End Class

    Public Class VRAcctEnquiryStatus
        Public Const Available As String = "A"
        Public Const AutomaticSuspended As String = "L"
        Public Const ManualSuspended As String = "S"
        Public Const ClassCode As String = "VRAcctEnquiryStatus"
    End Class

    Public Class SubmitChannel
        Public Const Electronic As String = "E"
        Public Const Paper As String = "P"
    End Class

    Public Class SPProfileVerifyStatus
        Public Const Updated As String = "U"
        Public Const Checked As String = "C"
        Public Const DataEntryConfirmed As String = "E"
        Public Const Vetted As String = "V"
        Public Const Reject As String = "R"
    End Class

    Public Class PracticeVerifyStatus
        Public Const Updated As String = "U"
        Public Const Checked As String = "C"
        Public Const DataEntryConfirmed As String = "E"
        Public Const Vetted As String = "V"
        Public Const Reject As String = "R"
    End Class

    Public Class BankAcctVerifyStatus
        Inherits Status
        Public Const Active As String = "U"
        Public Const Checked As String = "C"
        Public Const DataEntryConfirmed As String = "E"
        Public Const FirstVerified As String = "F"
        Public Const SecondVerified As String = "S"
        Public Const Reject As String = "R"
        Public Const Defer As String = "D"
        Public Const ClassCode As String = "BankAcctVerifyStatus"
    End Class

    Public Class HealthProfVerifyStatus
        Public Const Validated As String = "Y"
        Public Const Invalid As String = "N"
        Public Const Suspect As String = "S"
    End Class

    Public Class PrintFormOptionValue
        Public Const PreprintForm As String = "P"
        Public Const PrintPurposeAndConsent As String = "A"
        Public Const PrintConsentOnly As String = "C"
    End Class

    'CRE13-019-02 Extend HCVS to China [Start][Winnie]
    Public Class PrintFormAvailableVersion
        Public Const Both As String = "BOTH"
        Public Const Full As String = "FULL"
        Public Const Condense As String = "CONDENSE"
    End Class
    'CRE13-019-02 Extend HCVS to China [End][Winnie]

    Public Class ClaimTransStatus
        Inherits Status
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------
        Public Const Incomplete As String = "U"
        ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]
        Public Const Pending As String = "P"
        Public Const RejectedBySP As String = "W"
        Public Const PendingVRValidate As String = "V"
        Public Const Inactive As String = "I"
        Public Const Active As String = "A"
        Public Const Suspended As String = "S"
        Public Const Reimbursed As String = "R"
        Public Const PendingApprovalForNonReimbursedClaim As String = "B"
        Public Const ManualReimbursedClaim As String = "M" ' Should be Obsolete
        Public Const Removed As String = "D"
        ' CRE13-001 EHAPP [Start][Karl]
        ' -----------------------------------------------------------------------------------------
        Public Const Joined As String = "J"
        ' CRE13-001 EHAPP [End][Karl]
        Public Const ClassCode As String = "ClaimTransStatus"
    End Class

    Public Class ReimbursementStatus
        Inherits Status
        Public Const StartReimbursement As String = "S"
        Public Const HoldForFirstAuthorisation As String = "P"
        Public Const FirstAuthorised As String = "1"
        Public Const SecondAuthorised As String = "2"
        Public Const Reimbursed As String = "R"
        Public Const Voided As String = "V"
        Public Const NotAuthorized As String = "N"
        Public Const ClassCode As String = "ReimbursementStatus"
    End Class

    Public Class AuthorizedDisplayStatus
        Inherits Status
        Public Const PaymentFileSubmitted As String = "G"
        Public Const HoldForFirstAuthorisation As String = "P"
        Public Const FirstAuthorised As String = "1"
        Public Const SecondAuthorised As String = "2"
        Public Const NotAuthorized As String = "N"
        Public Const ClassCode As String = "AuthorizedDisplayStatus"
    End Class

    Public Class ReimbursementAuthorisationStatus
        Public Const Active As String = "A"
        Public Const Voided As String = "V"
    End Class

    Public Class ReimbursementAuthorisationSchemeCode
        Public Const All As String = "ALL"
    End Class

    ' CRE17-004 Generate a new DPAR on EHCP basis [Start][Dickson]
    Public Class ReimbursementVerificationCaseAvailable
        Public Const Available As String = "Y"
    End Class

    Public Class DPAReportType
        Public Const Practice As String = "PracticeBasis"
        Public Const EHCP As String = "EHCPBasis"
    End Class
    ' CRE17-004 Generate a new DPAR on EHCP basis [End][Dickson]

    Public Class SPAcctType
        Public Const ServiceProvider As String = "S"
        Public Const DataEntryAcct As String = "D"
    End Class

    Public Class ServiceProviderStatus
        Public Const Active As String = "A"
        Public Const Suspended As String = "S"
        Public Const Delisted As String = "D"

        Public Const ClassCode As String = "ServiceProviderStatus"
    End Class

    Public Class SPAccountStatus
        Public Const Active As String = "A"
        Public Const Delisted As String = "D"
        Public Const Suspended As String = "S"
        Public Const PendingForActivation As String = "P"

        Public Const ClassCode As String = "SPAccountStatus"

    End Class


    'CRE13-006 HCVS Ceiling [Start][Karl]

    Public Class eHealthAccountStatus
        Public Const Erased_Desc As String = "Erased"
    End Class

    'CRE13-006 HCVS Ceiling [End][Karl]

    Public Class DelistStatus
        Public Const Voluntary As String = "V"
        Public Const Involuntary As String = "I"

        Public Const ClassCode As String = "DelistStatus"
    End Class

    Public Class VRAcctType
        Public Const Temporary As String = "T"
        Public Const Validated As String = "V"
        Public Const ClassCode As String = "VRAcctType"
    End Class

    Public Class EHealthAccountType
        Public Const Validated As String = "V"
        Public Const Temporary As String = "T"
        Public Const Special As String = "S"
        Public Const Invalid As String = "I"

        Public Const ClassCode As String = "EHealthAccountType"
    End Class

    Public Class VRAcctValidatedStatus
        Public Const PendingForConfirmation As String = "C"
        Public Const PendingForVerify As String = "P"
        Public Const Validated As String = "V"
        Public Const Deleted As String = "D"
        Public Const Invalid As String = "I"
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
        Public Const Restricted As String = "R"
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]

        Public Const ClassCode As String = "VRAcctValidatedStatus"
    End Class

    Public Class TableLocation
        Public Const Enrolment As String = "E"
        Public Const Staging As String = "S"
        Public Const Permanent As String = "P"
    End Class

    Public Class JoinEHRSSStatus
        Public Const Yes As String = "Y"
        Public Const No As String = "N"
        Public Const NA As String = "I"
    End Class

    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
    ' ==========================================================
    Public Class JoinPCDStatus
        Public Const Yes As String = "Y"
        Public Const No As String = "N"
        Public Const Enrolled As String = "E"
        Public Const NA As String = "I"
    End Class
    ' CRE17-016 Remove PPIePR Enrolment [End][Koala]

    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    'Public Class PCDStatus
    '    Public Const Unknown As String = ""
    '    Public Const Enrolled As String = "E"
    '    Public Const Unprocessed As String = "U"
    '    Public Const Processing As String = "P"
    '    Public Const NotEnrolled As String = "N"
    '    Public Const NA As String = "I"
    'End Class

    Public Class PCDAccountStatus
        Public Const Unknown As String = ""
        Public Const Enrolled As String = "E"
        Public Const NotEnrolled As String = "N"
        Public Const Delisted As String = "D"
        Public Const Unavailable As String = "I"
        Public Const ConnectionFail As String = "C"
    End Class

    Public Class PCDEnrolmentStatus
        Public Const Unknown As String = ""
        Public Const Unprocessed As String = "U"
        Public Const Processing As String = "P"
        Public Const NA As String = "N"
        Public Const ConnectionFail As String = "C"
        Public Const Unavailable As String = "I"
    End Class
    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

    'Integration Start

    Public Class JoinProjectStatus
        Public Const Yes As String = "Y"
        Public Const No As String = "N"
        Public Const NA As String = "I"
    End Class

    'Integration End

    Public Class EVSPlatform
        Public Const HCVU As String = "01"
        Public Const HCSP As String = "02"
        Public Const PublicPlatform As String = "03"
        Public Const SDIR As String = "04"
        Public Const InterfaceInternal As String = "06"
        Public Const InterfaceExternal As String = "07"
    End Class

    Public Class ServiceProviderStagingStatus
        Public Const Active As String = "A"
        Public Const Suspended As String = "S"
        Public Const Reject As String = "R"
        Public Const Merged As String = "M"

        Public Const ClassCode As String = "ServiceProviderStagingStatus"
    End Class

    Public Class PracticeStagingStatus
        Public Const Active As String = "A"
        Public Const Existing As String = "E"
        Public Const Update As String = "U"
        Public Const Suspended As String = "S"
        Public Const Delisted As String = "D"
        Public Const Reject As String = "R"
        Public Const Merged As String = "M"

        Public Const ClassCode As String = "PracticeStagingStatus"
    End Class

    Public Class ProfessionalStagingStatus
        Public Const Active As String = "A"
        'Public Const Inactive As String = "I"
        Public Const Existing As String = "E"
        Public Const Delete As String = "I"
        Public Const Reject As String = "R"
        Public Const Merged As String = "M"
        Public Const Delisted As String = "D"
    End Class

    Public Class SchemeInformationStagingStatus
        Public Const Active As String = "A"
        Public Const Existing As String = "E"
        Public Const Suspended As String = "W"
        Public Const DelistedVoluntary As String = "V"
        Public Const DelistedInvoluntary As String = "I"
        Public Const ActivePendingSuspend As String = "S"
        Public Const ActivePendingDelist As String = "D"
        Public Const SuspendedPendingDelist As String = "X"
        Public Const SuspendedPendingReactivate As String = "Y"
        Public Const Reject As String = "R"
        Public Const Merged As String = "M"

        Public Const ClassCode As String = "SchemeInformationStagingStatus"
    End Class

    Public Class BankAcctStagingStatus
        Inherits Status
        Public Const Active As String = "A"
        Public Const Existing As String = "E"
        Public Const Suspended As String = "S"
        Public Const Delisted As String = "D"
        Public Const Reject As String = "R"
        Public Const Merged As String = "M"

        Public Const ClassCode As String = "BankAcctStagingStatus"
    End Class

    Public Class EmailChanged
        Public Const Changed As String = "Y"
        Public Const Unchanged As String = "N"
    End Class

    Public Class VRACreationPurpose
        Public Const ForClaim As String = "C"
        Public Const ForAmendment As String = "A"
        Public Const ForValidate As String = "V"
        Public Const ForAmendmentOld As String = "O"
    End Class

    Public Class ServiceProviderVerificationStatus
        Public Const Update As String = "U"
        Public Const DataEntryConfirmed As String = "E"
        Public Const Vetted As String = "V"
        Public Const Reject As String = "R"
        Public Const ReturnForAmendment As String = "A"
        Public Const Defer As String = "D"
        Public Const ClassCode As String = "ServiceProviderVerificationStatus"
    End Class

    Public Class SPAccountUpdateProgressStatus
        Public Const DataEntryStage As String = "E"
        Public Const VettingStage As String = "V"
        Public Const BankAcctVerification As String = "B"
        Public Const ProfessionalVerification As String = "P"
        Public Const WaitingForIssueToken As String = "T"
        Public Const WaitingForSchemeEnrolment As String = "S"
        Public Const CompletionStageWithTokenIssued As String = "C"
        Public Const Reject As String = "R"
        Public Const ClassCode As String = "SPAccountUpdateProgressStatus"
    End Class

    Public Class ProfessionalVerificationRecordStatus
        'Format: 'U' - updated, 'O' - export, 'I' - imported, 'C' - confirmed, 'R' - reject, 'D' - defer
        Public Const Update As String = "U"
        Public Const Export As String = "O"
        Public Const Import As String = "I"
        Public Const Confirm As String = "C"
        Public Const Reject As String = "R"
        Public Const Defer As String = "D"
        Public Const ReturnForAmendment As String = "A"

        Public Const ClassCode As String = "ProfessionalVerificationRecordStatus"
    End Class

    Public Class ProfVRRecordResultCat
        Public Const Valid As String = "Y"
        Public Const InValid As String = "N"
        Public Const Suspect As String = "S"
        Public Const NA As String = "NA"
    End Class

    Public Class ProfVRExportStatus
        Public Const Outstanding As String = "O"
        Public Const All As String = "A"
    End Class

    Public Class ProfVRSubmissHeaderStatus
        Public Const Active As String = "A"
        Public Const InActive As String = "I"
    End Class

    Public Class EmailStatus
        Public Const Unread As String = "U"
        Public Const Read As String = "R"
        Public Const Deleted As String = "D"
    End Class

    Public Class ServiceProviderTokenStatus
        Public Const NewEnrolment As String = "N"
        Public Const Active As String = "A"
        Public Const Suspeneded As String = "S"
        Public Const Delisted As String = "D"
        Public Const SchemeEnrolment As String = "E"
        Public Const ClassCode As String = "ServiceProviderTokenStatus"
    End Class

    Public Class TokenStatus
        Public Const NoToken As String = "N"
        Public Const Active As String = "A"
        Public Const Suspended As String = "S"
        Public Const Deactivated As String = "D"
        Public Const ClassCode As String = "TokenStatus"
    End Class

    Public Class TokenPendingStatus
        Public Const PendingReactivate As String = "R"
        Public Const PendingDeactivate As String = "D"
        Public Const Classcode As String = "TokenPendingStatus"
    End Class

    ' CRE13-003 - Token Replacement [Start][Tommy L]
    ' -------------------------------------------------------------------------------------
    Public Class TokenDisableReason
        Public Const Missing As String = "1"
        Public Const OutOfOrder As String = "2"
        Public Const Delist As String = "3"
        Public Const Replacement As String = "4"
        Public Const BOUserTokenRemoval As String = "5"
    End Class
    ' CRE13-003 - Token Replacement [End][Tommy L]

    Public Class DataDownloadStatus
        Public Const Pending As String = "P"
        Public Const Completed As String = "C"
        Public Const ErrorCase As String = "E"
        Public Const Inactive As String = "I"
        Public Const ClassCode As String = "DataDownloadStatus"
    End Class

    Public Class SPAccountMaintenanceUpdTypeStatus
        Public Const SPSuspend As String = "S"
        Public Const SPDelist As String = "D"
        Public Const SPReactivate As String = "R"
        Public Const PracticeDelist As String = "DP"
        Public Const PracticeSuspend As String = "SP"
        Public Const PracticeReactivate As String = "RP"
        Public Const TokenActivate As String = "AT"
        Public Const TokenDeactivate As String = "DT"
        Public Const ClassCode As String = "SPAccountMaintenanceUpdTypeStatus"
    End Class

    Public Class SPAccountMaintenanceRecordStatus
        Public Const Active As String = "A"
        Public Const Confrim As String = "C"
        Public Const Reject As String = "R"
        Public Const ClassCode As String = "SPAccountMaintenanceRecordStatus"
    End Class

    Public Class TokenProjectType
        Public Const EHCVS As String = "EHS"
        Public Const EHR As String = "EHR"
    End Class

    Public Class SchemeCode
        Public Const EHCVS As String = "EHCVS"
        Public Const IVSS As String = "IVSS"
    End Class

    Public Class SPMaintenanceDisplayStatus
        Public Const Active As String = "A"
        Public Const Suspended As String = "W"
        Public Const DelistedInvoluntary As String = "I"
        Public Const DelistedVoluntary As String = "V"
        Public Const SPPendingDelist As String = "D"
        Public Const SPPendingSuspend As String = "S"
        Public Const SPPendingReactivate As String = "R"
        Public Const SPSuspendPendingDelist As String = "X"
        Public Const PracticePendingDelist As String = "DP"
        Public Const PracticePendingSuspend As String = "SP"
        Public Const PracticePendingReactivate As String = "RP"
        Public Const PracticeSuspendPendingDelist As String = "TP"
        Public Const Locked As String = "LA"
        Public Const LockedPendingDelist As String = "LD"
        Public Const LockedPendingSuspend As String = "LS"
        Public Const ClassCode As String = "SPMaintenanceDisplayStatus"
    End Class

    ' INT14-0028 - Remove redundant status [Start][Lawrence]
    Public Class SPEnquiryDisplayStatus
        Public Const Unprocessed As String = "U"
        Public Const DataEntering As String = "E"
        Public Const Vetting As String = "N"
        Public Const BankVerifying As String = "B"
        Public Const ProfessionalVerifying As String = "P"
        Public Const WaitingIssueToken As String = "T"
        Public Const Active As String = "A"
        Public Const Suspended As String = "S"
        Public Const Delisted As String = "D"
        Public Const Locked As String = "LA"
        Public Const ClassCode As String = "SPEnquiryDisplayStatus"
    End Class
    ' INT14-0028 - Remove redundant status [End][Lawrence]

    Public Class VRAcctMaintenanceStatus
        Public Const Temporary As String = "T"
        Public Const Active As String = "A"
        Public Const Suspend As String = "S"
        Public Const Deleted As String = "D"
        Public Const Invalid As String = "E"
        Public Const Amended As String = "U"
        Public Const Special As String = "P"
        Public Const Fail As String = "N"

        Public Const ClassCode As String = "VRAcctMaintenanceStatus"
    End Class

    'Public Class TempAcctMaintenanceStatus
    '    Public Const PendingRemove As String = "N"
    '    Public Const OutstandingValidation As String = "O"
    '    Public Const PendingImmdValidation As String = "P"
    '    Public Const ClassCode As String = "TempAcctMaintenanceStatus"
    'End Class

    Public Class TempAcctMaintenanceStatusByParticular
        Public Const PendingRemove As String = "N"
        Public Const ClassCode As String = "TempAcctMaintenanceStatusByParticular"
    End Class


    Public Class TempAcctMaintenanceStatusByManualValidation
        Public Const OutstandingValidation As String = "O"
        Public Const PendingImmdValidation As String = "P"
        Public Const ClassCode As String = "TempAcctMaintenanceStatusByManualValidation"
    End Class

    'CRE20-023 Immu record (Vaccine lot management/creation) [Start][Nichole]
    Public Class VaccineLotMappingRecordStatus
        Public Const Active As String = "A"
        Public Const Pending As String = "P"
        Public Const Remove As String = "D"

        Public Const ClassCode As String = "VaccineLotRecordStatus"
    End Class

    Public Class VaccineLotDetailRecordStatus
        Public Const Active As String = "A"
        Public Const Pending As String = "P"
        Public Const Remove As String = "D"
        Public Const Expired As String = "E"

        Public Const ClassCode As String = "VaccineLotCreationRecordStatus"
        'Public Const ClassCode As String = "VaccineLotRecordStatus"
    End Class

    Public Class VaccineLotDetailLotAssignStatus
        Public Const Available As String = "A"
        Public Const Unavailable As String = "U"


        Public Const ClassCode As String = "VaccineLotAssignStatus"
    End Class
    'CRE20-023 Immue record (Vaccine lot management/creation) [End][Nichole]

    Public Class FunctCode

        ''' <summary>
        ''' Landing Page
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FUNT010001 As String = "010001"
        Public Const FUNT010002 As String = "010002"
        Public Const FUNT010003 As String = "010003" ' [CRE12-012] Infrastructure on Sending Messages through eHealth System Inbox
        Public Const FUNT010101 As String = "010101"
        Public Const FUNT010102 As String = "010102"
        Public Const FUNT010103 As String = "010103"
        Public Const FUNT010106 As String = "010106"
        Public Const FUNT010201 As String = "010201"
        Public Const FUNT010202 As String = "010202"
        Public Const FUNT010203 As String = "010203"
        Public Const FUNT010204 As String = "010204"
        Public Const FUNT010205 As String = "010205"
        Public Const FUNT010206 As String = "010206"
        Public Const FUNT010301 As String = "010301"
        Public Const FUNT010303 As String = "010303"
        Public Const FUNT010302 As String = "010302"
        Public Const FUNT010304 As String = "010304" ' Back Office - eHealth Account Death Record File Import
        Public Const FUNT010305 As String = "010305" ' Back Office - eHealth Account Death Record File Confirmation
        Public Const FUNT010306 As String = "010306" ' Back Office - eHealth Account Death Record Matching
        Public Const FUNT010307 As String = "010307" ' Back Office - eHealth Account Death Record Maintenance
        Public Const FUNT010308 As String = "010308" ' Back Office - eHealth Account Death Record Enquiry
        Public Const FUNT010309 As String = "010309" ' Back Office - eHealth Account Enquiry (Hotline)
        Public Const FUNT010401 As String = "010401"
        Public Const FUNT010402 As String = "010402"
        Public Const FUNT010403 As String = "010403"
        Public Const FUNT010404 As String = "010404"
        Public Const FUNT010405 As String = "010405"
        Public Const FUNT010406 As String = "010406"
        Public Const FUNT010407 As String = "010407"
        Public Const FUNT010408 As String = "010408"
        Public Const FUNT010409 As String = "010409"
        Public Const FUNT010410 As String = "010410" ' [CRE13-019-02 Extend HCVS to China] - Exchange Rate Management
        Public Const FUNT010411 As String = "010411" ' [CRE13-019-02 Extend HCVS to China] - Exchange Rate Enquiry
        Public Const FUNT010412 As String = "010412" ' [CRE13-019-02 Extend HCVS to China] - Exchange Rate Request Approval
        Public Const FUNT010413 As String = "010413" ' Claim Management - Student File - Student File Upload
        Public Const FUNT010414 As String = "010414" ' Claim Management - Student File - Student File Rectification
        Public Const FUNT010415 As String = "010415" ' Claim Management - Student File - Student File Claim Creation
        Public Const FUNT010416 As String = "010416" ' Claim Management - Student File - Student File Confirmation
        Public Const FUNT010417 As String = "010417" ' Claim Management - Student File - Student File Enquiry
        Public Const FUNT010418 As String = "010418" ' Claim Management - Claim Creation
        Public Const FUNT010421 As String = "010421" ' Claim Management - Reprint Vaccination Record
        Public Const FUNT010422 As String = "010422" ' Claim Management -  Vaccine Lot Management
        Public Const FUNT010423 As String = "010423" ' Claim Management - Vaccine Lot Approval
        Public Const FUNT010424 As String = "010424" ' Vaccine Lot Creation
        Public Const FUNT010425 As String = "010425" ' Vaccine Lot Creation Approval
        Public Const FUNT010427 As String = "010427" ' Claim Management -  Vaccine Lot Management in Private Clinic
        Public Const FUNT010428 As String = "010428" ' Claim Management - Vaccine Lot Approval in Private Clinic
        Public Const FUNT010501 As String = "010501"
        Public Const FUNT010601 As String = "010601"
        Public Const FUNT010701 As String = "010701"
        Public Const FUNT010702 As String = "010702"
        Public Const FUNT010703 As String = "010703"
        Public Const FUNT010704 As String = "010704"
        Public Const FUNT010801 As String = "010801"
        Public Const FUNT010901 As String = "010901" ' [CRE12-012] Infrastructure on Sending Messages through eHealth System Inbox
        Public Const FUNT011001 As String = "011001" ' [CRE11-013] RVP Home List Maintenance
        Public Const FUNT011002 As String = "011002"  'Outreach List Maintenance
        Public Const FUNT011101 As String = "011101" ' Inspection Record Management
        Public Const FUNT011102 As String = "011102" ' Inspection Record Approval
        Public Const FUNT019916 As String = "019916" ' [CRE12-012] Infrastructure on Sending Messages through eHealth System Inbox
        Public Const FUNT020001 As String = "020001"
        Public Const FUNT020002 As String = "020002"
        Public Const FUNT020003 As String = "020003"
        Public Const FUNT020004 As String = "020004"
        Public Const FUNT020005 As String = "020005"
        Public Const FUNT020007 As String = "020007"
        Public Const FUNT020008 As String = "020008" ' Recover Login
        Public Const FUNT020101 As String = "020101"
        Public Const FUNT020102 As String = "020102"
        Public Const FUNT020201 As String = "020201"
        Public Const FUNT020202 As String = "020202"
        Public Const FUNT020203 As String = "020203"
        Public Const FUNT020301 As String = "020301"
        Public Const FUNT020302 As String = "020302"
        Public Const FUNT020303 As String = "020303"
        Public Const FUNT020304 As String = "020304"
        Public Const FUNT020401 As String = "020401"
        Public Const FUNT020501 As String = "020501"
        Public Const FUNT020601 As String = "020601"
        Public Const FUNT020701 As String = "020701"
        Public Const FUNT020801 As String = "020801"
        Public Const FUNT020901 As String = "020901"
        Public Const FUNT021001 As String = "021001"
        Public Const FUNT021101 As String = "021101"
        Public Const FUNT021201 As String = "021201"  'CRE20-006 DHC Claim Access Nichole
        ''' <summary>
        ''' HCSP Common Log, e.g. System error, current browser handling
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FUNT029901 As String = "029901" ' HCSP Common Log, e.g. System error, current browser handling
        Public Const FUNT030001 As String = "030001"
        Public Const FUNT030101 As String = "030101"
        Public Const FUNT030102 As String = "030102"
        Public Const FUNT030103 As String = "030103"
        Public Const FUNT040101 As String = "040101"
        Public Const FUNT050101 As String = "050101"

        ' ===========================================================================
        ' 0601XX: Reserved for Interface for eVaccination Record
        ' ===========================================================================
        ''' <summary>
        ''' HA CMS / DH CIMS > EHS
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FUNT060101 As String = "060101"
        ''' <summary>
        ''' CMS Health Check - EHS > HA CMS
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FUNT060102 As String = "060102"

        ''' <summary>
        ''' CMS Health Check - EHS > DH CIMS
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FUNT060103 As String = "060103"

        ''' <summary>
        ''' EHS > HA CMS
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FUNT060104 As String = "060104"

        ''' <summary>
        ''' EHS > DH CIMS
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FUNT060105 As String = "060105"

        ' ===========================================================================
        ' 0602XX: Reserved for eHS & PCD Integration interface (In & Out)
        ' ===========================================================================
        ''' <summary>
        ''' Call PCD Integration web service to check PCD account active status (eHS -> PCD)
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FUNT060201 As String = "060201"
        ''' <summary>
        ''' Call PCD Integration web service to create PCD account (eHS -> PCD)
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FUNT060202 As String = "060202"
        ''' <summary>
        ''' Call PCD Integration web service to transfer practice information (eHS -> PCD)
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FUNT060203 As String = "060203"
        ''' <summary>
        ''' Call PCD Integration web service to upload enrolment information (eHS -> PCD)
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FUNT060204 As String = "060204"
        ''' <summary>
        ''' Web service for PCD to enquire eHS SP scheme information (PCD -> eHS)
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FUNT060205 As String = "060205"
        ''' <summary>
        ''' Call PCD Integration web service to available for verified enrolment (PCD -> eHS)
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FUNT060206 As String = "060206"
        ''' <summary>
        ''' Call PCD Integration web service to available for upload verified enrolment (PCD -> eHS)
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FUNT060207 As String = "060207"

        ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [Start] (Marco) ---
        ''' <summary>
        ''' Call PCD Check Account Status (PCD -> eHS)
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FUNT060208 As String = "060208"
        ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [End]   (Marco) ---

        ' CRE15-001 RSA Server Upgrade [Start][Winnie]
        Public Const FUNT060301 As String = "060301"
        ' CRE15-001 RSA Server Upgrade [End][Winnie]

        ' CRE17-010 OCSSS integration [Start][Koala]
        ''' <summary>
        ''' EHS -> DH OCSSS (Eligibility Check)
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FUNT060401 As String = "060401" ' EHS -> DH OCSSS

        ''' <summary>
        ''' EHS -> DH OCSSS (Health Check)
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FUNT060402 As String = "060402" ' EHS -> DH OCSSS (Health Check)
        ' CRE17-010 OCSSS integration [End][Koala]

        ' TODO: Waiting Paul to update and confirm function code format and approach
        Public Const FUNT070101 As String = "070101" ' External Interface for XXX
        Public Const FUNT070102 As String = "070102"
        Public Const FUNT070103 As String = "070103"
        Public Const FUNT070104 As String = "070104"
        Public Const FUNT070105 As String = "070105"
        Public Const FUNT070106 As String = "070106"

        Public Const FUNT070201 As String = "070201" ' eHR-to-eHS: getExternalWebS() - TokenService
        Public Const FUNT070202 As String = "070202" ' eHR-to-eHS: getExternalWebS() - PatientPortal
        Public Const FUNT070301 As String = "070301" ' eHS-to-eHR: verifySystem()
        Public Const FUNT070302 As String = "070302" ' eHS-to-eHR: getEhrWebS()

        'CRE20-006 DHC integration [Start][Nichole]
        Public Const FUNT070401 As String = "070401" ' DHC-to-eHS: getExternalWebS() - DHC integration
        'CRE20-006 DHC integration [End][Nichole]

        Public Const FUNT080101 As String = "080101" ' Form Generation Service
        Public Const FUNT090101 As String = "090101" ' Interface Control Webpage - EVaccine Check
        Public Const FUNT090102 As String = "090102" ' Interface Control Webpage - EVaccine Control
        Public Const FUNT090103 As String = "090103" ' Interface Control Webpage - Schedule Job Pause
        'CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Public Const FUNT090104 As String = "090104" ' Interface Control Webpage - PPI-ePR Control
        Public Const FUNT090104 As String = "090104" ' Interface Control Webpage - TSW Patient List Control
        'CRE15-019 (Rename PPI-ePR to eHRSS) [End][Chris YIM]
        Public Const FUNT090105 As String = "090105" ' Interface Control Webpage - Token Server Control
        Public Const FUNT090106 As String = "090106"
        Public Const FUNT090107 As String = "090107"
        Public Const FUNT090108 As String = "090108"
        Public Const FUNT090109 As String = "090109"
        ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
        Public Const FUNT090110 As String = "090110" ' Interface Control Webpage - Connection String Tester
        ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]
        Public Const FUNT090111 As String = "090111" ' Interface Control Webpage - OCSSS Control Site
        Public Const FUNT090112 As String = "090112" ' Interface Control Webpage - OCSSS - Enquire OCSSS


        Public Const FUNT100101 As String = "100101" ' Token Replacement interface - eHS to PPI-ePR - IsCommonUser
        Public Const FUNT100102 As String = "100102" ' Token Replacement interface - eHS to PPI-ePR - ReplaceToken
        Public Const FUNT100201 As String = "100201" ' Token Replacement interface - PPI-ePR to eHS - IsCommonUser
        Public Const FUNT100202 As String = "100202" ' Token Replacement interface - PPI-ePR to eHS - ReplaceToken

        ' ===========================================================================
        ' 11XXXX: Reserved for Schedule Job Function Code
        ' ===========================================================================
        ''' <summary>
        ''' Schedule Job CMS Health Check
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FUNT110101 As String = "110101"
        Public Const FUNT110201 As String = "110201"
        Public Const FUNT110301 As String = "110301"
        Public Const FUNT110401 As String = "110401"

        Public Const FUNT990000 As String = "990000"
        Public Const FUNT990001 As String = "990001"
        Public Const FUNT990002 As String = "990002"
        Public Const FUNT990003 As String = "990003" ' PCD Type Of Practice
    End Class

    Public Class MsgCode
        Public Const MSG00001 As String = "00001"
        Public Const MSG00002 As String = "00002"
        Public Const MSG00003 As String = "00003"
        Public Const MSG00004 As String = "00004"
        Public Const MSG00005 As String = "00005"
        Public Const MSG00006 As String = "00006"
        Public Const MSG00007 As String = "00007"
        Public Const MSG00008 As String = "00008"
        Public Const MSG00009 As String = "00009"
        Public Const MSG00010 As String = "00010"
        Public Const MSG00011 As String = "00011"
        Public Const MSG00012 As String = "00012"
        Public Const MSG00013 As String = "00013"
        Public Const MSG00014 As String = "00014"
        Public Const MSG00015 As String = "00015"
        Public Const MSG00016 As String = "00016"
        Public Const MSG00017 As String = "00017"
        Public Const MSG00018 As String = "00018"
        Public Const MSG00019 As String = "00019"
        Public Const MSG00020 As String = "00020"
        Public Const MSG00021 As String = "00021"
        Public Const MSG00022 As String = "00022"
        Public Const MSG00023 As String = "00023"
        Public Const MSG00024 As String = "00024"
        Public Const MSG00025 As String = "00025"
        Public Const MSG00026 As String = "00026"
        Public Const MSG00027 As String = "00027"
        Public Const MSG00028 As String = "00028"
        Public Const MSG00029 As String = "00029"
        Public Const MSG00030 As String = "00030"
        Public Const MSG00031 As String = "00031"
        Public Const MSG00032 As String = "00032"
        Public Const MSG00033 As String = "00033"
        Public Const MSG00034 As String = "00034"
        Public Const MSG00035 As String = "00035"
        Public Const MSG00036 As String = "00036"
        Public Const MSG00037 As String = "00037"
        Public Const MSG00038 As String = "00038"
        Public Const MSG00039 As String = "00039"
        Public Const MSG00040 As String = "00040"
        Public Const MSG00041 As String = "00041"
        Public Const MSG00042 As String = "00042"
        Public Const MSG00043 As String = "00043"
        Public Const MSG00044 As String = "00044"
        Public Const MSG00045 As String = "00045"
        Public Const MSG00046 As String = "00046"
        Public Const MSG00047 As String = "00047"
        Public Const MSG00048 As String = "00048"
        Public Const MSG00049 As String = "00049"
        Public Const MSG00050 As String = "00050"
        Public Const MSG00051 As String = "00051"
        Public Const MSG00052 As String = "00052"
        Public Const MSG00053 As String = "00053"
        Public Const MSG00054 As String = "00054"
        Public Const MSG00055 As String = "00055"
        Public Const MSG00056 As String = "00056"
        Public Const MSG00057 As String = "00057"
        Public Const MSG00058 As String = "00058"
        Public Const MSG00059 As String = "00059"
        Public Const MSG00060 As String = "00060"
        Public Const MSG00061 As String = "00061"
        Public Const MSG00062 As String = "00062"
        Public Const MSG00063 As String = "00063"
        Public Const MSG00064 As String = "00064"
        Public Const MSG00065 As String = "00065"
        Public Const MSG00066 As String = "00066"
        Public Const MSG00067 As String = "00067"
        Public Const MSG00068 As String = "00068"
        Public Const MSG00069 As String = "00069"
        Public Const MSG00070 As String = "00070"
        Public Const MSG00071 As String = "00071"
        Public Const MSG00072 As String = "00072"
        Public Const MSG00073 As String = "00073"
        Public Const MSG00074 As String = "00074"
        Public Const MSG00075 As String = "00075"
        Public Const MSG00076 As String = "00076"
        Public Const MSG00077 As String = "00077"
        Public Const MSG00078 As String = "00078"
        Public Const MSG00079 As String = "00079"
        Public Const MSG00080 As String = "00080"
        Public Const MSG00081 As String = "00081"
        Public Const MSG00082 As String = "00082"
        Public Const MSG00083 As String = "00083"
        Public Const MSG00084 As String = "00084"
        Public Const MSG00085 As String = "00085"
        Public Const MSG00086 As String = "00086"
        Public Const MSG00087 As String = "00087"
        Public Const MSG00088 As String = "00088"
        Public Const MSG00089 As String = "00089"
        Public Const MSG00090 As String = "00090"
        Public Const MSG00091 As String = "00091"
        Public Const MSG00092 As String = "00092"
        Public Const MSG00093 As String = "00093"
        Public Const MSG00094 As String = "00094"
        Public Const MSG00095 As String = "00095"
        Public Const MSG00096 As String = "00096"
        Public Const MSG00097 As String = "00097"
        Public Const MSG00098 As String = "00098"
        Public Const MSG00099 As String = "00099"
        Public Const MSG00100 As String = "00100"
        Public Const MSG00101 As String = "00101"
        Public Const MSG00102 As String = "00102"
        Public Const MSG00103 As String = "00103"
        Public Const MSG00104 As String = "00104"
        Public Const MSG00105 As String = "00105"
        Public Const MSG00106 As String = "00106"
        Public Const MSG00107 As String = "00107"
        Public Const MSG00108 As String = "00108"
        Public Const MSG00109 As String = "00109"
        Public Const MSG00110 As String = "00110"
        Public Const MSG00111 As String = "00111"
        Public Const MSG00112 As String = "00112"
        Public Const MSG00113 As String = "00113"
        Public Const MSG00114 As String = "00114"
        Public Const MSG00115 As String = "00115"
        Public Const MSG00116 As String = "00116"
        Public Const MSG00117 As String = "00117"
        Public Const MSG00118 As String = "00118"
        Public Const MSG00119 As String = "00119"
        Public Const MSG00120 As String = "00120"
        Public Const MSG00121 As String = "00121"
        Public Const MSG00122 As String = "00122"
        Public Const MSG00123 As String = "00123"
        Public Const MSG00124 As String = "00124"
        Public Const MSG00125 As String = "00125"
        Public Const MSG00126 As String = "00126"
        Public Const MSG00127 As String = "00127"
        Public Const MSG00128 As String = "00128"
        Public Const MSG00129 As String = "00129"
        Public Const MSG00130 As String = "00130"
        Public Const MSG00131 As String = "00131"
        Public Const MSG00132 As String = "00132"
        Public Const MSG00133 As String = "00133"
        Public Const MSG00134 As String = "00134"
        Public Const MSG00135 As String = "00135"
        Public Const MSG00136 As String = "00136"
        Public Const MSG00137 As String = "00137"
        Public Const MSG00138 As String = "00138"
        Public Const MSG00139 As String = "00139"
        Public Const MSG00140 As String = "00140"
        Public Const MSG00141 As String = "00141"
        Public Const MSG00142 As String = "00142"
        Public Const MSG00143 As String = "00143"
        Public Const MSG00144 As String = "00144"
        Public Const MSG00145 As String = "00145"
        Public Const MSG00146 As String = "00146"
        Public Const MSG00147 As String = "00147"
        Public Const MSG00148 As String = "00148"
        Public Const MSG00149 As String = "00149"
        Public Const MSG00150 As String = "00150"
        Public Const MSG00151 As String = "00151"
        Public Const MSG00152 As String = "00152"
        Public Const MSG00153 As String = "00153"
        Public Const MSG00154 As String = "00154"
        Public Const MSG00155 As String = "00155"
        Public Const MSG00156 As String = "00156"
        Public Const MSG00157 As String = "00157"
        Public Const MSG00158 As String = "00158"
        Public Const MSG00159 As String = "00159"
        Public Const MSG00160 As String = "00160"
        Public Const MSG00161 As String = "00161"
        Public Const MSG00162 As String = "00162"
        Public Const MSG00163 As String = "00163"
        Public Const MSG00164 As String = "00164"
        Public Const MSG00165 As String = "00165"
        Public Const MSG00166 As String = "00166"
        Public Const MSG00167 As String = "00167"
        Public Const MSG00168 As String = "00168"
        Public Const MSG00169 As String = "00169"
        Public Const MSG00170 As String = "00170"
        Public Const MSG00171 As String = "00171"
        Public Const MSG00172 As String = "00172"
        Public Const MSG00173 As String = "00173"
        Public Const MSG00174 As String = "00174"
        Public Const MSG00175 As String = "00175"
        Public Const MSG00176 As String = "00176"
        Public Const MSG00177 As String = "00177"
        Public Const MSG00178 As String = "00178"
        Public Const MSG00179 As String = "00179"
        Public Const MSG00180 As String = "00180"
        Public Const MSG00181 As String = "00181"
        Public Const MSG00182 As String = "00182"
        Public Const MSG00183 As String = "00183"
        Public Const MSG00184 As String = "00184"
        Public Const MSG00185 As String = "00185"
        Public Const MSG00186 As String = "00186"
        Public Const MSG00187 As String = "00187"
        Public Const MSG00188 As String = "00188"
        Public Const MSG00189 As String = "00189"
        Public Const MSG00190 As String = "00190"
        Public Const MSG00191 As String = "00191"
        Public Const MSG00192 As String = "00192"
        Public Const MSG00193 As String = "00193"
        Public Const MSG00194 As String = "00194"
        Public Const MSG00195 As String = "00195"
        Public Const MSG00196 As String = "00196"
        Public Const MSG00197 As String = "00197"
        Public Const MSG00198 As String = "00198"
        Public Const MSG00199 As String = "00199"
        Public Const MSG00200 As String = "00200"
        Public Const MSG00201 As String = "00201"
        Public Const MSG00202 As String = "00202"
        Public Const MSG00203 As String = "00203"
        Public Const MSG00204 As String = "00204"
        Public Const MSG00205 As String = "00205"
        Public Const MSG00206 As String = "00206"
        Public Const MSG00207 As String = "00207"
        Public Const MSG00208 As String = "00208"
        Public Const MSG00209 As String = "00209"
        Public Const MSG00210 As String = "00210"
        Public Const MSG00211 As String = "00211"
        Public Const MSG00212 As String = "00212"
        Public Const MSG00213 As String = "00213"
        Public Const MSG00214 As String = "00214"
        Public Const MSG00215 As String = "00215"
        Public Const MSG00216 As String = "00216"
        Public Const MSG00217 As String = "00217"
        Public Const MSG00218 As String = "00218"
        Public Const MSG00219 As String = "00219"
        Public Const MSG00220 As String = "00220"
        Public Const MSG00221 As String = "00221"
        Public Const MSG00222 As String = "00222"
        Public Const MSG00223 As String = "00223"
        Public Const MSG00224 As String = "00224"
        Public Const MSG00225 As String = "00225"
        Public Const MSG00226 As String = "00226"
        Public Const MSG00227 As String = "00227"
        Public Const MSG00228 As String = "00228"
        Public Const MSG00229 As String = "00229"
        Public Const MSG00230 As String = "00230"
        Public Const MSG00231 As String = "00231"
        Public Const MSG00232 As String = "00232"
        Public Const MSG00233 As String = "00233"
        Public Const MSG00234 As String = "00234"
        Public Const MSG00235 As String = "00235"
        Public Const MSG00236 As String = "00236"
        Public Const MSG00237 As String = "00237"
        Public Const MSG00238 As String = "00238"
        Public Const MSG00239 As String = "00239"
        Public Const MSG00240 As String = "00240"
        Public Const MSG00241 As String = "00241"
        Public Const MSG00242 As String = "00242"
        Public Const MSG00243 As String = "00243"
        Public Const MSG00244 As String = "00244"
        Public Const MSG00245 As String = "00245"
        Public Const MSG00246 As String = "00246"
        Public Const MSG00247 As String = "00247"
        Public Const MSG00248 As String = "00248"
        Public Const MSG00249 As String = "00249"
        Public Const MSG00250 As String = "00250"
        Public Const MSG00251 As String = "00251"
        Public Const MSG00252 As String = "00252"
        Public Const MSG00253 As String = "00253"
        Public Const MSG00254 As String = "00254"
        Public Const MSG00255 As String = "00255"
        Public Const MSG00256 As String = "00256"
        Public Const MSG00257 As String = "00257"
        Public Const MSG00258 As String = "00258"
        Public Const MSG00259 As String = "00259"
        Public Const MSG00260 As String = "00260"
        Public Const MSG00261 As String = "00261"
        Public Const MSG00262 As String = "00262"
        Public Const MSG00263 As String = "00263"
        Public Const MSG00264 As String = "00264"
        Public Const MSG00265 As String = "00265"
        Public Const MSG00266 As String = "00266"
        Public Const MSG00267 As String = "00267"
        Public Const MSG00268 As String = "00268"
        Public Const MSG00269 As String = "00269"
        Public Const MSG00270 As String = "00270"
        Public Const MSG00271 As String = "00271"
        Public Const MSG00272 As String = "00272"
        Public Const MSG00273 As String = "00273"
        Public Const MSG00274 As String = "00274"
        Public Const MSG00275 As String = "00275"
        Public Const MSG00276 As String = "00276"
        Public Const MSG00277 As String = "00277"
        Public Const MSG00278 As String = "00278"
        Public Const MSG00279 As String = "00279"
        Public Const MSG00280 As String = "00280"
        Public Const MSG00281 As String = "00281"
        Public Const MSG00282 As String = "00282"
        Public Const MSG00283 As String = "00283"
        Public Const MSG00284 As String = "00284"
        Public Const MSG00285 As String = "00285"
        Public Const MSG00286 As String = "00286"
        Public Const MSG00287 As String = "00287"
        Public Const MSG00288 As String = "00288"
        Public Const MSG00289 As String = "00289"
        Public Const MSG00290 As String = "00290"
        Public Const MSG00291 As String = "00291"
        Public Const MSG00292 As String = "00292"
        Public Const MSG00293 As String = "00293"
        Public Const MSG00294 As String = "00294"
        Public Const MSG00295 As String = "00295"
        Public Const MSG00296 As String = "00296"
        Public Const MSG00297 As String = "00297"
        Public Const MSG00298 As String = "00298"
        Public Const MSG00299 As String = "00299"
        Public Const MSG00300 As String = "00300"
        Public Const MSG00301 As String = "00301"
        Public Const MSG00302 As String = "00302"
        Public Const MSG00303 As String = "00303"
        Public Const MSG00304 As String = "00304"
        Public Const MSG00305 As String = "00305"
        Public Const MSG00306 As String = "00306"
        Public Const MSG00307 As String = "00307"
        Public Const MSG00308 As String = "00308"
        Public Const MSG00309 As String = "00309"
        Public Const MSG00310 As String = "00310"
        Public Const MSG00311 As String = "00311"
        Public Const MSG00312 As String = "00312"
        Public Const MSG00313 As String = "00313"
        Public Const MSG00314 As String = "00314"
        Public Const MSG00315 As String = "00315"
        Public Const MSG00316 As String = "00316"
        Public Const MSG00317 As String = "00317"
        Public Const MSG00318 As String = "00318"
        Public Const MSG00319 As String = "00319"
        Public Const MSG00320 As String = "00320"
        Public Const MSG00321 As String = "00321"
        Public Const MSG00322 As String = "00322"
        Public Const MSG00323 As String = "00323"
        Public Const MSG00324 As String = "00324"
        Public Const MSG00325 As String = "00325"
        Public Const MSG00326 As String = "00326"
        Public Const MSG00327 As String = "00327"
        Public Const MSG00328 As String = "00328"
        Public Const MSG00329 As String = "00329"
        Public Const MSG00330 As String = "00330"
        Public Const MSG00331 As String = "00331"
        Public Const MSG00332 As String = "00332"
        Public Const MSG00333 As String = "00333"
        Public Const MSG00334 As String = "00334"
        Public Const MSG00335 As String = "00335"
        Public Const MSG00336 As String = "00336"
        Public Const MSG00337 As String = "00337"
        Public Const MSG00338 As String = "00338"
        Public Const MSG00339 As String = "00339"
        Public Const MSG00340 As String = "00340"
        Public Const MSG00341 As String = "00341"
        Public Const MSG00342 As String = "00342"
        Public Const MSG00343 As String = "00343"
        Public Const MSG00344 As String = "00344"
        Public Const MSG00345 As String = "00345"
        Public Const MSG00346 As String = "00346"
        Public Const MSG00347 As String = "00347"
        Public Const MSG00348 As String = "00348"
        Public Const MSG00349 As String = "00349"
        Public Const MSG00350 As String = "00350"
        Public Const MSG00351 As String = "00351"
        Public Const MSG00352 As String = "00352"
        Public Const MSG00353 As String = "00353"
        Public Const MSG00354 As String = "00354"
        Public Const MSG00355 As String = "00355"
        Public Const MSG00356 As String = "00356"
        Public Const MSG00357 As String = "00357"
        Public Const MSG00358 As String = "00358"
        Public Const MSG00359 As String = "00359"
        Public Const MSG00360 As String = "00360"
        Public Const MSG00361 As String = "00361"
        Public Const MSG00362 As String = "00362"
        Public Const MSG00363 As String = "00363"
        Public Const MSG00364 As String = "00364"
        Public Const MSG00365 As String = "00365"
        Public Const MSG00366 As String = "00366"
        Public Const MSG00367 As String = "00367"
        Public Const MSG00368 As String = "00368"
        Public Const MSG00369 As String = "00369"
        Public Const MSG00370 As String = "00370"
        Public Const MSG00371 As String = "00371"
        Public Const MSG00372 As String = "00372"
        Public Const MSG00373 As String = "00373"
        Public Const MSG00374 As String = "00374"
        Public Const MSG00375 As String = "00375"
        Public Const MSG00376 As String = "00376"
        Public Const MSG00377 As String = "00377"
        Public Const MSG00378 As String = "00378"
        Public Const MSG00379 As String = "00379"
        Public Const MSG00380 As String = "00380"
        Public Const MSG00381 As String = "00381"
        Public Const MSG00382 As String = "00382"
        Public Const MSG00383 As String = "00383"
        Public Const MSG00384 As String = "00384"
        Public Const MSG00385 As String = "00385"
        Public Const MSG00386 As String = "00386"
        Public Const MSG00387 As String = "00387"
        Public Const MSG00388 As String = "00388"
        Public Const MSG00389 As String = "00389"
        Public Const MSG00390 As String = "00390"
        Public Const MSG00391 As String = "00391"
        Public Const MSG00392 As String = "00392"
        Public Const MSG00393 As String = "00393"
        Public Const MSG00394 As String = "00394"
        Public Const MSG00395 As String = "00395"
        Public Const MSG00396 As String = "00396"
        Public Const MSG00397 As String = "00397"
        Public Const MSG00398 As String = "00398"
        Public Const MSG00399 As String = "00399"
        Public Const MSG00400 As String = "00400"
        Public Const MSG00401 As String = "00401"
        Public Const MSG00402 As String = "00402"
        Public Const MSG00403 As String = "00403"
        Public Const MSG00404 As String = "00404"
        Public Const MSG00405 As String = "00405"
        Public Const MSG00406 As String = "00406"
        Public Const MSG00407 As String = "00407"
        Public Const MSG00408 As String = "00408"
        Public Const MSG00409 As String = "00409"
        Public Const MSG00410 As String = "00410"
        Public Const MSG00411 As String = "00411"
        Public Const MSG00412 As String = "00412"
        Public Const MSG00413 As String = "00413"
        Public Const MSG00414 As String = "00414"
        Public Const MSG00415 As String = "00415"
        Public Const MSG00416 As String = "00416"
        Public Const MSG00417 As String = "00417"
        Public Const MSG00418 As String = "00418"
        Public Const MSG00419 As String = "00419"
        Public Const MSG00420 As String = "00420"
        Public Const MSG00421 As String = "00421"
        Public Const MSG00422 As String = "00422"
        Public Const MSG00423 As String = "00423"
        Public Const MSG00424 As String = "00424"
        Public Const MSG00425 As String = "00425"
        Public Const MSG00426 As String = "00426"
        Public Const MSG00427 As String = "00427"
        Public Const MSG00428 As String = "00428"
        Public Const MSG00429 As String = "00429"
        Public Const MSG00430 As String = "00430"
        Public Const MSG00431 As String = "00431"
        Public Const MSG00432 As String = "00432"
        Public Const MSG00433 As String = "00433"
        Public Const MSG00434 As String = "00434"
        Public Const MSG00435 As String = "00435"
        Public Const MSG00436 As String = "00436"
        Public Const MSG00437 As String = "00437"
        Public Const MSG00438 As String = "00438"
        Public Const MSG00439 As String = "00439"
        Public Const MSG00440 As String = "00440"
        Public Const MSG00441 As String = "00441"
        Public Const MSG00442 As String = "00442"
        Public Const MSG00443 As String = "00443"
        Public Const MSG00444 As String = "00444"
        Public Const MSG00445 As String = "00445"
        Public Const MSG00446 As String = "00446"
        Public Const MSG00447 As String = "00447"
        Public Const MSG00448 As String = "00448"
        Public Const MSG00449 As String = "00449"
        Public Const MSG00450 As String = "00450"
        Public Const MSG00451 As String = "00451"
        Public Const MSG00452 As String = "00452"
        Public Const MSG00453 As String = "00453"
        Public Const MSG00454 As String = "00454"
        Public Const MSG00455 As String = "00455"
        Public Const MSG00456 As String = "00456"
        Public Const MSG00457 As String = "00457"
        Public Const MSG00458 As String = "00458"
        Public Const MSG00459 As String = "00459"
        Public Const MSG00460 As String = "00460"
        Public Const MSG00461 As String = "00461"
        Public Const MSG00462 As String = "00462"
        Public Const MSG00463 As String = "00463"
        Public Const MSG00464 As String = "00464"
        Public Const MSG00465 As String = "00465"
        Public Const MSG00466 As String = "00466"
        Public Const MSG00467 As String = "00467"
        Public Const MSG00468 As String = "00468"
        Public Const MSG00469 As String = "00469"
        Public Const MSG00470 As String = "00470"
        Public Const MSG00471 As String = "00471"
        Public Const MSG00472 As String = "00472"
        Public Const MSG00473 As String = "00473"
        Public Const MSG00474 As String = "00474"
        Public Const MSG00475 As String = "00475"
        Public Const MSG00476 As String = "00476"
        Public Const MSG00477 As String = "00477"
        Public Const MSG00478 As String = "00478"
        Public Const MSG00479 As String = "00479"
        Public Const MSG00480 As String = "00480"
        Public Const MSG00481 As String = "00481"
        Public Const MSG00482 As String = "00482"
        Public Const MSG00483 As String = "00483"
        Public Const MSG00484 As String = "00484"
        Public Const MSG00485 As String = "00485"
        Public Const MSG00486 As String = "00486"
        Public Const MSG00487 As String = "00487"
        Public Const MSG00488 As String = "00488"
        Public Const MSG00489 As String = "00489"
        Public Const MSG00490 As String = "00490"
        Public Const MSG00491 As String = "00491"
        Public Const MSG00492 As String = "00492"
        Public Const MSG00493 As String = "00493"
        Public Const MSG00494 As String = "00494"
        Public Const MSG00495 As String = "00495"
        Public Const MSG00496 As String = "00496"
        Public Const MSG00497 As String = "00497"
        Public Const MSG00498 As String = "00498"
        Public Const MSG00499 As String = "00499"
        Public Const MSG00500 As String = "00500"
        Public Const MSG00501 As String = "00501"
        Public Const MSG00502 As String = "00502"
        Public Const MSG00503 As String = "00503"
        Public Const MSG00504 As String = "00504"
        Public Const MSG00505 As String = "00505"
        Public Const MSG00506 As String = "00506"
        Public Const MSG00507 As String = "00507"
        Public Const MSG00508 As String = "00508"
        Public Const MSG00509 As String = "00509"
        Public Const MSG00510 As String = "00510"
        Public Const MSG00511 As String = "00511"
        Public Const MSG00512 As String = "00512"
        Public Const MSG00513 As String = "00513"
        Public Const MSG00514 As String = "00514"
        Public Const MSG00515 As String = "00515"
        Public Const MSG00516 As String = "00516"
        Public Const MSG00517 As String = "00517"
        Public Const MSG00518 As String = "00518"
        Public Const MSG00519 As String = "00519"
        Public Const MSG00520 As String = "00520"
        Public Const MSG00521 As String = "00521"
        Public Const MSG00522 As String = "00522"
        Public Const MSG00523 As String = "00523"
        Public Const MSG00524 As String = "00524"
        Public Const MSG00525 As String = "00525"
        Public Const MSG00526 As String = "00526"
        Public Const MSG00527 As String = "00527"
        Public Const MSG00528 As String = "00528"
        Public Const MSG00529 As String = "00529"
        Public Const MSG00530 As String = "00530"
        Public Const MSG00531 As String = "00531"
        Public Const MSG00532 As String = "00532"
        Public Const MSG00533 As String = "00533"
        Public Const MSG00534 As String = "00534"
        Public Const MSG00535 As String = "00535"
        Public Const MSG00536 As String = "00536"
        Public Const MSG00537 As String = "00537"
        Public Const MSG00538 As String = "00538"
        Public Const MSG00539 As String = "00539"
        Public Const MSG00540 As String = "00540"
        Public Const MSG00541 As String = "00541"
        Public Const MSG00542 As String = "00542"
        Public Const MSG00543 As String = "00543"
        Public Const MSG00544 As String = "00544"
        Public Const MSG00545 As String = "00545"
        Public Const MSG00546 As String = "00546"
        Public Const MSG00547 As String = "00547"
        Public Const MSG00548 As String = "00548"
        Public Const MSG00549 As String = "00549"
        Public Const MSG00550 As String = "00550"
        Public Const MSG00551 As String = "00551"
        Public Const MSG00552 As String = "00552"
        Public Const MSG00553 As String = "00553"
        Public Const MSG00554 As String = "00554"
        Public Const MSG00555 As String = "00555"
        Public Const MSG00556 As String = "00556"
        Public Const MSG00557 As String = "00557"
        Public Const MSG00558 As String = "00558"
        Public Const MSG00559 As String = "00559"
        Public Const MSG00560 As String = "00560"
        Public Const MSG00561 As String = "00561"
        Public Const MSG00562 As String = "00562"
        Public Const MSG00563 As String = "00563"
        Public Const MSG00564 As String = "00564"
        Public Const MSG00565 As String = "00565"
        Public Const MSG00566 As String = "00566"
        Public Const MSG00567 As String = "00567"
        Public Const MSG00568 As String = "00568"
        Public Const MSG00569 As String = "00569"
        Public Const MSG00570 As String = "00570"
        Public Const MSG00571 As String = "00571"
        Public Const MSG00572 As String = "00572"
        Public Const MSG00573 As String = "00573"
        Public Const MSG00574 As String = "00574"
        Public Const MSG00575 As String = "00575"
        Public Const MSG00576 As String = "00576"
        Public Const MSG00577 As String = "00577"
        Public Const MSG00578 As String = "00578"
        Public Const MSG00579 As String = "00579"
        Public Const MSG00580 As String = "00580"
        Public Const MSG00581 As String = "00581"
        Public Const MSG00582 As String = "00582"
        Public Const MSG00583 As String = "00583"
        Public Const MSG00584 As String = "00584"
        Public Const MSG00585 As String = "00585"
        Public Const MSG00586 As String = "00586"
        Public Const MSG00587 As String = "00587"
        Public Const MSG00588 As String = "00588"
        Public Const MSG00589 As String = "00589"
        Public Const MSG00590 As String = "00590"
        Public Const MSG00591 As String = "00591"
        Public Const MSG00592 As String = "00592"
        Public Const MSG00593 As String = "00593"
        Public Const MSG00594 As String = "00594"
        Public Const MSG00595 As String = "00595"
        Public Const MSG00596 As String = "00596"
        Public Const MSG00597 As String = "00597"
        Public Const MSG00598 As String = "00598"
        Public Const MSG00599 As String = "00599"
        Public Const MSG00600 As String = "00600"
        Public Const MSG00601 As String = "00601"
        Public Const MSG00602 As String = "00602"
        Public Const MSG00603 As String = "00603"
        Public Const MSG00604 As String = "00604"
        Public Const MSG00605 As String = "00605"
        Public Const MSG00606 As String = "00606"
        Public Const MSG00607 As String = "00607"
        Public Const MSG00608 As String = "00608"
        Public Const MSG00609 As String = "00609"
        Public Const MSG00610 As String = "00610"
        Public Const MSG00611 As String = "00611"
        Public Const MSG00612 As String = "00612"
        Public Const MSG00613 As String = "00613"
        Public Const MSG00614 As String = "00614"
        Public Const MSG00615 As String = "00615"
        Public Const MSG00616 As String = "00616"
        Public Const MSG00617 As String = "00617"
        Public Const MSG00618 As String = "00618"
        Public Const MSG00619 As String = "00619"
        Public Const MSG00620 As String = "00620"
        Public Const MSG00621 As String = "00621"
        Public Const MSG00622 As String = "00622"
        Public Const MSG00623 As String = "00623"
        Public Const MSG00624 As String = "00624"
        Public Const MSG00625 As String = "00625"
        Public Const MSG00626 As String = "00626"
        Public Const MSG00627 As String = "00627"
        Public Const MSG00628 As String = "00628"
        Public Const MSG00629 As String = "00629"
        Public Const MSG00630 As String = "00630"
        Public Const MSG00631 As String = "00631"
        Public Const MSG00632 As String = "00632"
        Public Const MSG00633 As String = "00633"
        Public Const MSG00634 As String = "00634"
        Public Const MSG00635 As String = "00635"
        Public Const MSG00636 As String = "00636"
        Public Const MSG00637 As String = "00637"
        Public Const MSG00638 As String = "00638"
        Public Const MSG00639 As String = "00639"
        Public Const MSG00640 As String = "00640"
        Public Const MSG00641 As String = "00641"
        Public Const MSG00642 As String = "00642"
        Public Const MSG00643 As String = "00643"
        Public Const MSG00644 As String = "00644"
        Public Const MSG00645 As String = "00645"
        Public Const MSG00646 As String = "00646"
        Public Const MSG00647 As String = "00647"
        Public Const MSG00648 As String = "00648"
        Public Const MSG00649 As String = "00649"
        Public Const MSG00650 As String = "00650"
        Public Const MSG00651 As String = "00651"
        Public Const MSG00652 As String = "00652"
        Public Const MSG00653 As String = "00653"
        Public Const MSG00654 As String = "00654"
        Public Const MSG00655 As String = "00655"
        Public Const MSG00656 As String = "00656"
        Public Const MSG00657 As String = "00657"
        Public Const MSG00658 As String = "00658"
        Public Const MSG00659 As String = "00659"
        Public Const MSG00660 As String = "00660"
        Public Const MSG00661 As String = "00661"
        Public Const MSG00662 As String = "00662"
        Public Const MSG00663 As String = "00663"
        Public Const MSG00664 As String = "00664"
        Public Const MSG00665 As String = "00665"
        Public Const MSG00666 As String = "00666"
        Public Const MSG00667 As String = "00667"
        Public Const MSG00668 As String = "00668"
        Public Const MSG00669 As String = "00669"
        Public Const MSG00670 As String = "00670"
        Public Const MSG00671 As String = "00671"
        Public Const MSG00672 As String = "00672"
        Public Const MSG00673 As String = "00673"
        Public Const MSG00674 As String = "00674"
        Public Const MSG00675 As String = "00675"
        Public Const MSG00676 As String = "00676"
        Public Const MSG00677 As String = "00677"
        Public Const MSG00678 As String = "00678"
        Public Const MSG00679 As String = "00679"
        Public Const MSG00680 As String = "00680"
        Public Const MSG00681 As String = "00681"
        Public Const MSG00682 As String = "00682"
        Public Const MSG00683 As String = "00683"
        Public Const MSG00684 As String = "00684"
        Public Const MSG00685 As String = "00685"
        Public Const MSG00686 As String = "00686"
        Public Const MSG00687 As String = "00687"
        Public Const MSG00688 As String = "00688"
        Public Const MSG00689 As String = "00689"
        Public Const MSG00690 As String = "00690"
        Public Const MSG00691 As String = "00691"
        Public Const MSG00692 As String = "00692"
        Public Const MSG00693 As String = "00693"
        Public Const MSG00694 As String = "00694"
        Public Const MSG00695 As String = "00695"
        Public Const MSG00696 As String = "00696"
        Public Const MSG00697 As String = "00697"
        Public Const MSG00698 As String = "00698"
        Public Const MSG00699 As String = "00699"
        Public Const MSG00700 As String = "00700"
    End Class

    Public Class SeverityCode
        Public Const SEVD As String = "D"
        Public Const SEVE As String = "E"
        Public Const SEVI As String = "I"
        Public Const SEVQ As String = "Q"
    End Class

    Public Class MessageBoxHeaderKey
        Public Const ValidationFail As String = "ValidationFail"
        Public Const UpdateFail As String = "UpdateFail"
        Public Const ValidationWarning As String = "Warning"
    End Class

    Public Class LogID
        Public Const LOG00000 As String = "00000"   ' For Entering the Function (In Page Load)
        Public Const LOG00001 As String = "00001"
        Public Const LOG00002 As String = "00002"
        Public Const LOG00003 As String = "00003"
        Public Const LOG00004 As String = "00004"
        Public Const LOG00005 As String = "00005"
        Public Const LOG00006 As String = "00006"
        Public Const LOG00007 As String = "00007"
        Public Const LOG00008 As String = "00008"
        Public Const LOG00009 As String = "00009"
        Public Const LOG00010 As String = "00010"
        Public Const LOG00011 As String = "00011"
        Public Const LOG00012 As String = "00012"
        Public Const LOG00013 As String = "00013"
        Public Const LOG00014 As String = "00014"
        Public Const LOG00015 As String = "00015"
        Public Const LOG00016 As String = "00016"
        Public Const LOG00017 As String = "00017"
        Public Const LOG00018 As String = "00018"
        Public Const LOG00019 As String = "00019"
        Public Const LOG00020 As String = "00020"
        Public Const LOG00021 As String = "00021"
        Public Const LOG00022 As String = "00022"
        Public Const LOG00023 As String = "00023"
        Public Const LOG00024 As String = "00024"
        Public Const LOG00025 As String = "00025"
        Public Const LOG00026 As String = "00026"
        Public Const LOG00027 As String = "00027"
        Public Const LOG00028 As String = "00028"
        Public Const LOG00029 As String = "00029"
        Public Const LOG00030 As String = "00030"
        Public Const LOG00031 As String = "00031"
        Public Const LOG00032 As String = "00032"
        Public Const LOG00033 As String = "00033"
        Public Const LOG00034 As String = "00034"
        Public Const LOG00035 As String = "00035"
        Public Const LOG00036 As String = "00036"
        Public Const LOG00037 As String = "00037"
        Public Const LOG00038 As String = "00038"
        Public Const LOG00039 As String = "00039"
        Public Const LOG00040 As String = "00040"
        Public Const LOG00041 As String = "00041"
        Public Const LOG00042 As String = "00042"
        Public Const LOG00043 As String = "00043"
        Public Const LOG00044 As String = "00044"
        Public Const LOG00045 As String = "00045"
        Public Const LOG00046 As String = "00046"
        Public Const LOG00047 As String = "00047"
        Public Const LOG00048 As String = "00048"
        Public Const LOG00049 As String = "00049"
        Public Const LOG00050 As String = "00050"
        Public Const LOG00051 As String = "00051"
        Public Const LOG00052 As String = "00052"
        Public Const LOG00053 As String = "00053"
        Public Const LOG00054 As String = "00054"
        Public Const LOG00055 As String = "00055"
        Public Const LOG00056 As String = "00056"
        Public Const LOG00057 As String = "00057"
        Public Const LOG00058 As String = "00058"
        Public Const LOG00059 As String = "00059"
        Public Const LOG00060 As String = "00060"
        Public Const LOG00061 As String = "00061"
        Public Const LOG00062 As String = "00062"
        Public Const LOG00063 As String = "00063"
        Public Const LOG00064 As String = "00064"
        Public Const LOG00065 As String = "00065"
        Public Const LOG00066 As String = "00066"
        Public Const LOG00067 As String = "00067"
        Public Const LOG00068 As String = "00068"
        Public Const LOG00069 As String = "00069"
        Public Const LOG00070 As String = "00070"
        Public Const LOG00071 As String = "00071"
        Public Const LOG00072 As String = "00072"
        Public Const LOG00073 As String = "00073"
        Public Const LOG00074 As String = "00074"
        Public Const LOG00075 As String = "00075"
        Public Const LOG00076 As String = "00076"
        Public Const LOG00077 As String = "00077"
        Public Const LOG00078 As String = "00078"
        Public Const LOG00079 As String = "00079"
        Public Const LOG00080 As String = "00080"
        Public Const LOG00081 As String = "00081"
        Public Const LOG00082 As String = "00082"
        Public Const LOG00083 As String = "00083"
        Public Const LOG00084 As String = "00084"
        Public Const LOG00085 As String = "00085"
        Public Const LOG00086 As String = "00086"
        Public Const LOG00087 As String = "00087"
        Public Const LOG00088 As String = "00088"
        Public Const LOG00089 As String = "00089"
        Public Const LOG00090 As String = "00090"
        Public Const LOG00091 As String = "00091"
        Public Const LOG00092 As String = "00092"
        Public Const LOG00093 As String = "00093"
        Public Const LOG00094 As String = "00094"
        Public Const LOG00095 As String = "00095"
        Public Const LOG00096 As String = "00096"
        Public Const LOG00097 As String = "00097"
        Public Const LOG00098 As String = "00098"
        Public Const LOG00099 As String = "00099"
        Public Const LOG00100 As String = "00100"
        Public Const LOG00101 As String = "00101"
        Public Const LOG00102 As String = "00102"
        Public Const LOG00103 As String = "00103"
        Public Const LOG00104 As String = "00104"
        Public Const LOG00105 As String = "00105"
        Public Const LOG00106 As String = "00106"
        Public Const LOG00107 As String = "00107"
        Public Const LOG00108 As String = "00108"
        Public Const LOG00109 As String = "00109"
        Public Const LOG00110 As String = "00110"
        Public Const LOG00111 As String = "00111"
        Public Const LOG00112 As String = "00112"
        Public Const LOG00113 As String = "00113"
        Public Const LOG00114 As String = "00114"
        Public Const LOG00115 As String = "00115"
        Public Const LOG00116 As String = "00116"
        Public Const LOG00117 As String = "00117"
        Public Const LOG00118 As String = "00118"
        Public Const LOG00119 As String = "00119"
        Public Const LOG00120 As String = "00120"
        Public Const LOG00121 As String = "00121"
        Public Const LOG00122 As String = "00122"
        Public Const LOG00123 As String = "00123"
        Public Const LOG00124 As String = "00124"
        Public Const LOG00125 As String = "00125"
        Public Const LOG00126 As String = "00126"
        Public Const LOG00127 As String = "00127"
        Public Const LOG00128 As String = "00128"
        Public Const LOG00129 As String = "00129"
        Public Const LOG00130 As String = "00130"
        Public Const LOG00131 As String = "00131"
        Public Const LOG00132 As String = "00132"
        Public Const LOG00133 As String = "00133"
        Public Const LOG00134 As String = "00134"
        Public Const LOG00135 As String = "00135"
        Public Const LOG00136 As String = "00136"
        Public Const LOG00137 As String = "00137"
        Public Const LOG00138 As String = "00138"
        Public Const LOG00139 As String = "00139"
        Public Const LOG00140 As String = "00140"
        Public Const LOG00141 As String = "00141"
        Public Const LOG00142 As String = "00142"
        Public Const LOG00143 As String = "00143"
        Public Const LOG00144 As String = "00144"
        Public Const LOG00145 As String = "00145"
        Public Const LOG00146 As String = "00146"
        Public Const LOG00147 As String = "00147"
        Public Const LOG00148 As String = "00148"
        Public Const LOG00149 As String = "00149"
        Public Const LOG00150 As String = "00150"
        Public Const LOG00151 As String = "00151"
        Public Const LOG00152 As String = "00152"
        Public Const LOG00153 As String = "00153"
        Public Const LOG00154 As String = "00154"
        Public Const LOG00155 As String = "00155"
        Public Const LOG00156 As String = "00156"
        Public Const LOG00157 As String = "00157"
        Public Const LOG00158 As String = "00158"
        Public Const LOG00159 As String = "00159"
        Public Const LOG00160 As String = "00160"
        Public Const LOG00161 As String = "00161"
        Public Const LOG00162 As String = "00162"
        Public Const LOG00163 As String = "00163"
        Public Const LOG00164 As String = "00164"
        Public Const LOG00165 As String = "00165"
        Public Const LOG00166 As String = "00166"
        Public Const LOG00167 As String = "00167"
        Public Const LOG00168 As String = "00168"
        Public Const LOG00169 As String = "00169"
        Public Const LOG00170 As String = "00170"
        Public Const LOG00171 As String = "00171"
        Public Const LOG00172 As String = "00172"
        Public Const LOG00173 As String = "00173"
        Public Const LOG00174 As String = "00174"
        Public Const LOG00175 As String = "00175"
        Public Const LOG00176 As String = "00176"
        Public Const LOG00177 As String = "00177"
        Public Const LOG00178 As String = "00178"
        Public Const LOG00179 As String = "00179"
        Public Const LOG00180 As String = "00180"
        Public Const LOG00181 As String = "00181"
        Public Const LOG00182 As String = "00182"
        Public Const LOG00183 As String = "00183"
        Public Const LOG00184 As String = "00184"
        Public Const LOG00185 As String = "00185"
        Public Const LOG00186 As String = "00186"
        Public Const LOG00187 As String = "00187"
        Public Const LOG00188 As String = "00188"
        Public Const LOG00189 As String = "00189"
        Public Const LOG00190 As String = "00190"
        Public Const LOG00191 As String = "00191"
        Public Const LOG00192 As String = "00192"
        Public Const LOG00193 As String = "00193"
        Public Const LOG00194 As String = "00194"
        Public Const LOG00195 As String = "00195"
        Public Const LOG00196 As String = "00196"
        Public Const LOG00197 As String = "00197"
        Public Const LOG00198 As String = "00198"
        Public Const LOG00199 As String = "00199"
        Public Const LOG00200 As String = "00200"
        Public Const LOG00201 As String = "00201"
        Public Const LOG00202 As String = "00202"
        Public Const LOG00203 As String = "00203"
        Public Const LOG00204 As String = "00204"
        Public Const LOG00205 As String = "00205"
        Public Const LOG00206 As String = "00206"
        Public Const LOG00207 As String = "00207"
        Public Const LOG00208 As String = "00208"
        Public Const LOG00209 As String = "00209"
        Public Const LOG00210 As String = "00210"
        Public Const LOG00211 As String = "00211"
        Public Const LOG00212 As String = "00212"
        Public Const LOG00213 As String = "00213"
        Public Const LOG00214 As String = "00214"
        Public Const LOG00215 As String = "00215"
        Public Const LOG00216 As String = "00216"
        Public Const LOG00217 As String = "00217"
        Public Const LOG00218 As String = "00218"
        Public Const LOG00219 As String = "00219"
        Public Const LOG00220 As String = "00220"
        Public Const LOG00221 As String = "00221"
        Public Const LOG00222 As String = "00222"
        Public Const LOG00223 As String = "00223"
        Public Const LOG00224 As String = "00224"
        Public Const LOG00225 As String = "00225"
        Public Const LOG00226 As String = "00226"
        Public Const LOG00227 As String = "00227"
        Public Const LOG00228 As String = "00228"
        Public Const LOG00229 As String = "00229"
        Public Const LOG00230 As String = "00230"
        Public Const LOG00231 As String = "00231"
        Public Const LOG00232 As String = "00232"
        Public Const LOG00233 As String = "00233"
        Public Const LOG00234 As String = "00234"
        Public Const LOG00235 As String = "00235"
        Public Const LOG00236 As String = "00236"
        Public Const LOG00237 As String = "00237"
        Public Const LOG00238 As String = "00238"
        Public Const LOG00239 As String = "00239"
        Public Const LOG00240 As String = "00240"
        Public Const LOG00241 As String = "00241"
        Public Const LOG00242 As String = "00242"
        Public Const LOG00243 As String = "00243"
        Public Const LOG00244 As String = "00244"
        Public Const LOG00245 As String = "00245"
        Public Const LOG00246 As String = "00246"
        Public Const LOG00247 As String = "00247"
        Public Const LOG00248 As String = "00248"
        Public Const LOG00249 As String = "00249"
        Public Const LOG00250 As String = "00250"
        Public Const LOG00251 As String = "00251"
        Public Const LOG00252 As String = "00252"
        Public Const LOG00253 As String = "00253"
        Public Const LOG00254 As String = "00254"
        Public Const LOG00255 As String = "00255"
        Public Const LOG00256 As String = "00256"
        Public Const LOG00257 As String = "00257"
        Public Const LOG00258 As String = "00258"
        Public Const LOG00259 As String = "00259"
        Public Const LOG00260 As String = "00260"
        Public Const LOG00261 As String = "00261"
        Public Const LOG00262 As String = "00262"
        Public Const LOG00263 As String = "00263"
        Public Const LOG00264 As String = "00264"
        Public Const LOG00265 As String = "00265"
        Public Const LOG00266 As String = "00266"
        Public Const LOG00267 As String = "00267"
        Public Const LOG00268 As String = "00268"
        Public Const LOG00269 As String = "00269"
        Public Const LOG00270 As String = "00270"
        Public Const LOG00271 As String = "00271"
        Public Const LOG00272 As String = "00272"
        Public Const LOG00273 As String = "00273"
        Public Const LOG00274 As String = "00274"
        Public Const LOG00275 As String = "00275"
        Public Const LOG00276 As String = "00276"
        Public Const LOG00277 As String = "00277"
        Public Const LOG00278 As String = "00278"
        Public Const LOG00279 As String = "00279"
        Public Const LOG00280 As String = "00280"
        Public Const LOG00281 As String = "00281"
        Public Const LOG00282 As String = "00282"
        Public Const LOG00283 As String = "00283"
        Public Const LOG00284 As String = "00284"
        Public Const LOG00285 As String = "00285"
        Public Const LOG00286 As String = "00286"
        Public Const LOG00287 As String = "00287"
        Public Const LOG00288 As String = "00288"
        Public Const LOG00289 As String = "00289"
        Public Const LOG00290 As String = "00290"
        Public Const LOG00291 As String = "00291"
        Public Const LOG00292 As String = "00292"
        Public Const LOG00293 As String = "00293"
        Public Const LOG00294 As String = "00294"
        Public Const LOG00295 As String = "00295"
        Public Const LOG00296 As String = "00296"
        Public Const LOG00297 As String = "00297"
        Public Const LOG00298 As String = "00298"
        Public Const LOG00299 As String = "00299"

        ' 300 to 499 reserved to common control cross-function within platform
        Public Const LOG00300 As String = "00300"
        Public Const LOG00301 As String = "00301"
        Public Const LOG00302 As String = "00302"
        Public Const LOG00303 As String = "00303"
        Public Const LOG00304 As String = "00304"
        Public Const LOG00305 As String = "00305"
        Public Const LOG00306 As String = "00306"
        Public Const LOG00307 As String = "00307"
        Public Const LOG00308 As String = "00308"
        Public Const LOG00309 As String = "00309"
        Public Const LOG00310 As String = "00310"
        Public Const LOG00311 As String = "00311"
        Public Const LOG00312 As String = "00312"
        Public Const LOG00313 As String = "00313"
        Public Const LOG00314 As String = "00314"
        Public Const LOG00315 As String = "00315"
        Public Const LOG00316 As String = "00316"
        Public Const LOG00317 As String = "00317"
        Public Const LOG00318 As String = "00318"
        Public Const LOG00319 As String = "00319"
        Public Const LOG00320 As String = "00320"
        Public Const LOG00321 As String = "00321"
        Public Const LOG00322 As String = "00322"
        Public Const LOG00323 As String = "00323"
        Public Const LOG00324 As String = "00324"
        Public Const LOG00325 As String = "00325"
        Public Const LOG00326 As String = "00326"
        Public Const LOG00327 As String = "00327"
        Public Const LOG00328 As String = "00328"
        Public Const LOG00329 As String = "00329"
        Public Const LOG00330 As String = "00330"
        Public Const LOG00331 As String = "00331"
        Public Const LOG00332 As String = "00332"
        Public Const LOG00333 As String = "00333"
        Public Const LOG00334 As String = "00334"
        Public Const LOG00335 As String = "00335"
        Public Const LOG00336 As String = "00336"
        Public Const LOG00337 As String = "00337"
        Public Const LOG00338 As String = "00338"
        Public Const LOG00339 As String = "00339"
        Public Const LOG00340 As String = "00340"
        Public Const LOG00341 As String = "00341"
        Public Const LOG00342 As String = "00342"
        Public Const LOG00343 As String = "00343"
        Public Const LOG00344 As String = "00344"
        Public Const LOG00345 As String = "00345"
        Public Const LOG00346 As String = "00346"
        Public Const LOG00347 As String = "00347"
        Public Const LOG00348 As String = "00348"
        Public Const LOG00349 As String = "00349"
        Public Const LOG00350 As String = "00350"
        Public Const LOG00351 As String = "00351"
        Public Const LOG00352 As String = "00352"
        Public Const LOG00353 As String = "00353"
        Public Const LOG00354 As String = "00354"
        Public Const LOG00355 As String = "00355"
        Public Const LOG00356 As String = "00356"
        Public Const LOG00357 As String = "00357"
        Public Const LOG00358 As String = "00358"
        Public Const LOG00359 As String = "00359"
        Public Const LOG00360 As String = "00360"
        Public Const LOG00361 As String = "00361"
        Public Const LOG00362 As String = "00362"
        Public Const LOG00363 As String = "00363"
        Public Const LOG00364 As String = "00364"
        Public Const LOG00365 As String = "00365"
        Public Const LOG00366 As String = "00366"
        Public Const LOG00367 As String = "00367"
        Public Const LOG00368 As String = "00368"
        Public Const LOG00369 As String = "00369"
        Public Const LOG00370 As String = "00370"
        Public Const LOG00371 As String = "00371"
        Public Const LOG00372 As String = "00372"
        Public Const LOG00373 As String = "00373"
        Public Const LOG00374 As String = "00374"
        Public Const LOG00375 As String = "00375"
        Public Const LOG00376 As String = "00376"
        Public Const LOG00377 As String = "00377"
        Public Const LOG00378 As String = "00378"
        Public Const LOG00379 As String = "00379"
        Public Const LOG00380 As String = "00380"
        Public Const LOG00381 As String = "00381"
        Public Const LOG00382 As String = "00382"
        Public Const LOG00383 As String = "00383"
        Public Const LOG00384 As String = "00384"
        Public Const LOG00385 As String = "00385"
        Public Const LOG00386 As String = "00386"
        Public Const LOG00387 As String = "00387"
        Public Const LOG00388 As String = "00388"
        Public Const LOG00389 As String = "00389"
        Public Const LOG00390 As String = "00390"
        Public Const LOG00391 As String = "00391"
        Public Const LOG00392 As String = "00392"
        Public Const LOG00393 As String = "00393"
        Public Const LOG00394 As String = "00394"
        Public Const LOG00395 As String = "00395"
        Public Const LOG00396 As String = "00396"
        Public Const LOG00397 As String = "00397"
        Public Const LOG00398 As String = "00398"
        Public Const LOG00399 As String = "00399"
        Public Const LOG00400 As String = "00400"
        Public Const LOG00401 As String = "00401"
        Public Const LOG00402 As String = "00402"
        Public Const LOG00403 As String = "00403"
        Public Const LOG00404 As String = "00404"
        Public Const LOG00405 As String = "00405"
        Public Const LOG00406 As String = "00406"
        Public Const LOG00407 As String = "00407"
        Public Const LOG00408 As String = "00408"
        Public Const LOG00409 As String = "00409"
        Public Const LOG00410 As String = "00410"
        Public Const LOG00411 As String = "00411"
        Public Const LOG00412 As String = "00412"
        Public Const LOG00413 As String = "00413"
        Public Const LOG00414 As String = "00414"
        Public Const LOG00415 As String = "00415"
        Public Const LOG00416 As String = "00416"
        Public Const LOG00417 As String = "00417"
        Public Const LOG00418 As String = "00418"
        Public Const LOG00419 As String = "00419"
        Public Const LOG00420 As String = "00420"
        Public Const LOG00421 As String = "00421"
        Public Const LOG00422 As String = "00422"
        Public Const LOG00423 As String = "00423"
        Public Const LOG00424 As String = "00424"
        Public Const LOG00425 As String = "00425"
        Public Const LOG00426 As String = "00426"
        Public Const LOG00427 As String = "00427"
        Public Const LOG00428 As String = "00428"
        Public Const LOG00429 As String = "00429"
        Public Const LOG00430 As String = "00430"
        Public Const LOG00431 As String = "00431"
        Public Const LOG00432 As String = "00432"
        Public Const LOG00433 As String = "00433"
        Public Const LOG00434 As String = "00434"
        Public Const LOG00435 As String = "00435"
        Public Const LOG00436 As String = "00436"
        Public Const LOG00437 As String = "00437"
        Public Const LOG00438 As String = "00438"
        Public Const LOG00439 As String = "00439"
        Public Const LOG00440 As String = "00440"
        Public Const LOG00441 As String = "00441"
        Public Const LOG00442 As String = "00442"
        Public Const LOG00443 As String = "00443"
        Public Const LOG00444 As String = "00444"
        Public Const LOG00445 As String = "00445"
        Public Const LOG00446 As String = "00446"
        Public Const LOG00447 As String = "00447"
        Public Const LOG00448 As String = "00448"
        Public Const LOG00449 As String = "00449"
        Public Const LOG00450 As String = "00450"
        Public Const LOG00451 As String = "00451"
        Public Const LOG00452 As String = "00452"
        Public Const LOG00453 As String = "00453"
        Public Const LOG00454 As String = "00454"
        Public Const LOG00455 As String = "00455"
        Public Const LOG00456 As String = "00456"
        Public Const LOG00457 As String = "00457"
        Public Const LOG00458 As String = "00458"
        Public Const LOG00459 As String = "00459"
        Public Const LOG00460 As String = "00460"
        Public Const LOG00461 As String = "00461"
        Public Const LOG00462 As String = "00462"
        Public Const LOG00463 As String = "00463"
        Public Const LOG00464 As String = "00464"
        Public Const LOG00465 As String = "00465"
        Public Const LOG00466 As String = "00466"
        Public Const LOG00467 As String = "00467"
        Public Const LOG00468 As String = "00468"
        Public Const LOG00469 As String = "00469"
        Public Const LOG00470 As String = "00470"
        Public Const LOG00471 As String = "00471"
        Public Const LOG00472 As String = "00472"
        Public Const LOG00473 As String = "00473"
        Public Const LOG00474 As String = "00474"
        Public Const LOG00475 As String = "00475"
        Public Const LOG00476 As String = "00476"
        Public Const LOG00477 As String = "00477"
        Public Const LOG00478 As String = "00478"
        Public Const LOG00479 As String = "00479"
        Public Const LOG00480 As String = "00480"
        Public Const LOG00481 As String = "00481"
        Public Const LOG00482 As String = "00482"
        Public Const LOG00483 As String = "00483"
        Public Const LOG00484 As String = "00484"
        Public Const LOG00485 As String = "00485"
        Public Const LOG00486 As String = "00486"
        Public Const LOG00487 As String = "00487"
        Public Const LOG00488 As String = "00488"
        Public Const LOG00489 As String = "00489"
        Public Const LOG00490 As String = "00490"
        Public Const LOG00491 As String = "00491"
        Public Const LOG00492 As String = "00492"
        Public Const LOG00493 As String = "00493"
        Public Const LOG00494 As String = "00494"
        Public Const LOG00495 As String = "00495"
        Public Const LOG00496 As String = "00496"
        Public Const LOG00497 As String = "00497"
        Public Const LOG00498 As String = "00498"
        Public Const LOG00499 As String = "00499"
        'Public Const LOG00500 As String = "00500"
        'Public Const LOG00501 As String = "00501"

        ' 1000 to 1299 reserved to common functions to cross-platform
        Public Const LOG01000 As String = "01000"
        Public Const LOG01001 As String = "01001"
        Public Const LOG01002 As String = "01002"
        Public Const LOG01003 As String = "01003"
        Public Const LOG01004 As String = "01004"
        Public Const LOG01005 As String = "01005"
        Public Const LOG01006 As String = "01006"
        Public Const LOG01007 As String = "01007"
        Public Const LOG01008 As String = "01008"
        Public Const LOG01009 As String = "01009"
        Public Const LOG01010 As String = "01010"
        Public Const LOG01011 As String = "01011"
        Public Const LOG01012 As String = "01012"
        Public Const LOG01013 As String = "01013"
        Public Const LOG01014 As String = "01014"
        Public Const LOG01015 As String = "01015"
        Public Const LOG01016 As String = "01016"
        Public Const LOG01017 As String = "01017"
        Public Const LOG01018 As String = "01018"
        Public Const LOG01019 As String = "01019"
        Public Const LOG01020 As String = "01020"
        Public Const LOG01021 As String = "01021"
        Public Const LOG01022 As String = "01022"
        Public Const LOG01023 As String = "01023"
        Public Const LOG01024 As String = "01024"
        Public Const LOG01025 As String = "01025"
        Public Const LOG01026 As String = "01026"
        Public Const LOG01027 As String = "01027"
        Public Const LOG01028 As String = "01028"
        Public Const LOG01029 As String = "01029"
        Public Const LOG01030 As String = "01030"
        Public Const LOG01031 As String = "01031"
        Public Const LOG01032 As String = "01032"
        Public Const LOG01033 As String = "01033"
        Public Const LOG01034 As String = "01034"
        Public Const LOG01035 As String = "01035"
        Public Const LOG01036 As String = "01036"
        Public Const LOG01037 As String = "01037"
        Public Const LOG01038 As String = "01038"
        Public Const LOG01039 As String = "01039"
        Public Const LOG01040 As String = "01040"
        Public Const LOG01041 As String = "01041"
        Public Const LOG01042 As String = "01042"
        Public Const LOG01043 As String = "01043"
        Public Const LOG01044 As String = "01044"
        Public Const LOG01045 As String = "01045"
        Public Const LOG01046 As String = "01046"
        Public Const LOG01047 As String = "01047"
        Public Const LOG01048 As String = "01048"
        Public Const LOG01049 As String = "01049"
        Public Const LOG01050 As String = "01050"
        Public Const LOG01051 As String = "01051"
        Public Const LOG01052 As String = "01052"
        Public Const LOG01053 As String = "01053"
        Public Const LOG01054 As String = "01054"
        Public Const LOG01055 As String = "01055"
        Public Const LOG01056 As String = "01056"
        Public Const LOG01057 As String = "01057"
        Public Const LOG01058 As String = "01058"
        Public Const LOG01059 As String = "01059"
        Public Const LOG01060 As String = "01060"
        Public Const LOG01061 As String = "01061"
        Public Const LOG01062 As String = "01062"
        Public Const LOG01063 As String = "01063"
        Public Const LOG01064 As String = "01064"
        Public Const LOG01065 As String = "01065"
        Public Const LOG01066 As String = "01066"
        Public Const LOG01067 As String = "01067"
        Public Const LOG01068 As String = "01068"
        Public Const LOG01069 As String = "01069"
        Public Const LOG01070 As String = "01070"
        Public Const LOG01071 As String = "01071"
        Public Const LOG01072 As String = "01072"
        Public Const LOG01073 As String = "01073"
        Public Const LOG01074 As String = "01074"
        Public Const LOG01075 As String = "01075"
        Public Const LOG01076 As String = "01076"
        Public Const LOG01077 As String = "01077"
        Public Const LOG01078 As String = "01078"
        Public Const LOG01079 As String = "01079"
        Public Const LOG01080 As String = "01080"
        Public Const LOG01081 As String = "01081"
        Public Const LOG01082 As String = "01082"
        Public Const LOG01083 As String = "01083"
        Public Const LOG01084 As String = "01084"
        Public Const LOG01085 As String = "01085"
        Public Const LOG01086 As String = "01086"
        Public Const LOG01087 As String = "01087"
        Public Const LOG01088 As String = "01088"
        Public Const LOG01089 As String = "01089"
        Public Const LOG01090 As String = "01090"
        Public Const LOG01091 As String = "01091"
        Public Const LOG01092 As String = "01092"
        Public Const LOG01093 As String = "01093"
        Public Const LOG01094 As String = "01094"
        Public Const LOG01095 As String = "01095"
        Public Const LOG01096 As String = "01096"
        Public Const LOG01097 As String = "01097"
        Public Const LOG01098 As String = "01098"
        Public Const LOG01099 As String = "01099"
        Public Const LOG01100 As String = "01100"
        Public Const LOG01101 As String = "01101"
        Public Const LOG01102 As String = "01102"
        Public Const LOG01103 As String = "01103"
        Public Const LOG01104 As String = "01104"
        Public Const LOG01105 As String = "01105"
        Public Const LOG01106 As String = "01106"
        Public Const LOG01107 As String = "01107"
        Public Const LOG01108 As String = "01108"
        Public Const LOG01109 As String = "01109"
        Public Const LOG01110 As String = "01110"
        Public Const LOG01111 As String = "01111"
        Public Const LOG01112 As String = "01112"
        Public Const LOG01113 As String = "01113"
        Public Const LOG01114 As String = "01114"
        Public Const LOG01115 As String = "01115"
        Public Const LOG01116 As String = "01116"
        Public Const LOG01117 As String = "01117"
        Public Const LOG01118 As String = "01118"
        Public Const LOG01119 As String = "01119"
        Public Const LOG01120 As String = "01120"
        Public Const LOG01121 As String = "01121"
        Public Const LOG01122 As String = "01122"
        Public Const LOG01123 As String = "01123"
        Public Const LOG01124 As String = "01124"
        Public Const LOG01125 As String = "01125"
        Public Const LOG01126 As String = "01126"
        Public Const LOG01127 As String = "01127"
        Public Const LOG01128 As String = "01128"
        Public Const LOG01129 As String = "01129"
        Public Const LOG01130 As String = "01130"
        Public Const LOG01131 As String = "01131"
        Public Const LOG01132 As String = "01132"
        Public Const LOG01133 As String = "01133"
        Public Const LOG01134 As String = "01134"
        Public Const LOG01135 As String = "01135"
        Public Const LOG01136 As String = "01136"
        Public Const LOG01137 As String = "01137"
        Public Const LOG01138 As String = "01138"
        Public Const LOG01139 As String = "01139"
        Public Const LOG01140 As String = "01140"
        Public Const LOG01141 As String = "01141"
        Public Const LOG01142 As String = "01142"
        Public Const LOG01143 As String = "01143"
        Public Const LOG01144 As String = "01144"
        Public Const LOG01145 As String = "01145"
        Public Const LOG01146 As String = "01146"
        Public Const LOG01147 As String = "01147"
        Public Const LOG01148 As String = "01148"
        Public Const LOG01149 As String = "01149"
        Public Const LOG01150 As String = "01150"
        Public Const LOG01151 As String = "01151"
        Public Const LOG01152 As String = "01152"
        Public Const LOG01153 As String = "01153"
        Public Const LOG01154 As String = "01154"
        Public Const LOG01155 As String = "01155"
        Public Const LOG01156 As String = "01156"
        Public Const LOG01157 As String = "01157"
        Public Const LOG01158 As String = "01158"
        Public Const LOG01159 As String = "01159"
        Public Const LOG01160 As String = "01160"
        Public Const LOG01161 As String = "01161"
        Public Const LOG01162 As String = "01162"
        Public Const LOG01163 As String = "01163"
        Public Const LOG01164 As String = "01164"
        Public Const LOG01165 As String = "01165"
        Public Const LOG01166 As String = "01166"
        Public Const LOG01167 As String = "01167"
        Public Const LOG01168 As String = "01168"
        Public Const LOG01169 As String = "01169"
        Public Const LOG01170 As String = "01170"
        Public Const LOG01171 As String = "01171"
        Public Const LOG01172 As String = "01172"
        Public Const LOG01173 As String = "01173"
        Public Const LOG01174 As String = "01174"
        Public Const LOG01175 As String = "01175"
        Public Const LOG01176 As String = "01176"
        Public Const LOG01177 As String = "01177"
        Public Const LOG01178 As String = "01178"
        Public Const LOG01179 As String = "01179"
        Public Const LOG01180 As String = "01180"
        Public Const LOG01181 As String = "01181"
        Public Const LOG01182 As String = "01182"
        Public Const LOG01183 As String = "01183"
        Public Const LOG01184 As String = "01184"
        Public Const LOG01185 As String = "01185"
        Public Const LOG01186 As String = "01186"
        Public Const LOG01187 As String = "01187"
        Public Const LOG01188 As String = "01188"
        Public Const LOG01189 As String = "01189"
        Public Const LOG01190 As String = "01190"
        Public Const LOG01191 As String = "01191"
        Public Const LOG01192 As String = "01192"
        Public Const LOG01193 As String = "01193"
        Public Const LOG01194 As String = "01194"
        Public Const LOG01195 As String = "01195"
        Public Const LOG01196 As String = "01196"
        Public Const LOG01197 As String = "01197"
        Public Const LOG01198 As String = "01198"
        Public Const LOG01199 As String = "01199"
        Public Const LOG01200 As String = "01200"
        Public Const LOG01201 As String = "01201"
        Public Const LOG01202 As String = "01202"
        Public Const LOG01203 As String = "01203"
        Public Const LOG01204 As String = "01204"
        Public Const LOG01205 As String = "01205"
        Public Const LOG01206 As String = "01206"
        Public Const LOG01207 As String = "01207"
        Public Const LOG01208 As String = "01208"
        Public Const LOG01209 As String = "01209"
        Public Const LOG01210 As String = "01210"
        Public Const LOG01211 As String = "01211"
        Public Const LOG01212 As String = "01212"
        Public Const LOG01213 As String = "01213"
        Public Const LOG01214 As String = "01214"
        Public Const LOG01215 As String = "01215"
        Public Const LOG01216 As String = "01216"
        Public Const LOG01217 As String = "01217"
        Public Const LOG01218 As String = "01218"
        Public Const LOG01219 As String = "01219"
        Public Const LOG01220 As String = "01220"
        Public Const LOG01221 As String = "01221"
        Public Const LOG01222 As String = "01222"
        Public Const LOG01223 As String = "01223"
        Public Const LOG01224 As String = "01224"
        Public Const LOG01225 As String = "01225"
        Public Const LOG01226 As String = "01226"
        Public Const LOG01227 As String = "01227"
        Public Const LOG01228 As String = "01228"
        Public Const LOG01229 As String = "01229"
        Public Const LOG01230 As String = "01230"
        Public Const LOG01231 As String = "01231"
        Public Const LOG01232 As String = "01232"
        Public Const LOG01233 As String = "01233"
        Public Const LOG01234 As String = "01234"
        Public Const LOG01235 As String = "01235"
        Public Const LOG01236 As String = "01236"
        Public Const LOG01237 As String = "01237"
        Public Const LOG01238 As String = "01238"
        Public Const LOG01239 As String = "01239"
        Public Const LOG01240 As String = "01240"
        Public Const LOG01241 As String = "01241"
        Public Const LOG01242 As String = "01242"
        Public Const LOG01243 As String = "01243"
        Public Const LOG01244 As String = "01244"
        Public Const LOG01245 As String = "01245"
        Public Const LOG01246 As String = "01246"
        Public Const LOG01247 As String = "01247"
        Public Const LOG01248 As String = "01248"
        Public Const LOG01249 As String = "01249"
        Public Const LOG01250 As String = "01250"
        Public Const LOG01251 As String = "01251"
        Public Const LOG01252 As String = "01252"
        Public Const LOG01253 As String = "01253"
        Public Const LOG01254 As String = "01254"
        Public Const LOG01255 As String = "01255"
        Public Const LOG01256 As String = "01256"
        Public Const LOG01257 As String = "01257"
        Public Const LOG01258 As String = "01258"
        Public Const LOG01259 As String = "01259"
        Public Const LOG01260 As String = "01260"
        Public Const LOG01261 As String = "01261"
        Public Const LOG01262 As String = "01262"
        Public Const LOG01263 As String = "01263"
        Public Const LOG01264 As String = "01264"
        Public Const LOG01265 As String = "01265"
        Public Const LOG01266 As String = "01266"
        Public Const LOG01267 As String = "01267"
        Public Const LOG01268 As String = "01268"
        Public Const LOG01269 As String = "01269"
        Public Const LOG01270 As String = "01270"
        Public Const LOG01271 As String = "01271"
        Public Const LOG01272 As String = "01272"
        Public Const LOG01273 As String = "01273"
        Public Const LOG01274 As String = "01274"
        Public Const LOG01275 As String = "01275"
        Public Const LOG01276 As String = "01276"
        Public Const LOG01277 As String = "01277"
        Public Const LOG01278 As String = "01278"
        Public Const LOG01279 As String = "01279"
        Public Const LOG01280 As String = "01280"
        Public Const LOG01281 As String = "01281"
        Public Const LOG01282 As String = "01282"
        Public Const LOG01283 As String = "01283"
        Public Const LOG01284 As String = "01284"
        Public Const LOG01285 As String = "01285"
        Public Const LOG01286 As String = "01286"
        Public Const LOG01287 As String = "01287"
        Public Const LOG01288 As String = "01288"
        Public Const LOG01289 As String = "01289"
        Public Const LOG01290 As String = "01290"
        Public Const LOG01291 As String = "01291"
        Public Const LOG01292 As String = "01292"
        Public Const LOG01293 As String = "01293"
        Public Const LOG01294 As String = "01294"
        Public Const LOG01295 As String = "01295"
        Public Const LOG01296 As String = "01296"
        Public Const LOG01297 As String = "01297"
        Public Const LOG01298 As String = "01298"
        Public Const LOG01299 As String = "01299"

        ' 1300 to 1399 reserved to CommonScheduleJob functions to cross-platform
        Public Const LOG01300 As String = "01300"
        Public Const LOG01301 As String = "01301"
        Public Const LOG01302 As String = "01302"
        Public Const LOG01303 As String = "01303"
        Public Const LOG01304 As String = "01304"
        Public Const LOG01305 As String = "01305"
        Public Const LOG01306 As String = "01306"
        Public Const LOG01307 As String = "01307"
        Public Const LOG01308 As String = "01308"
        Public Const LOG01309 As String = "01309"
        Public Const LOG01310 As String = "01310"
        Public Const LOG01311 As String = "01311"
        Public Const LOG01312 As String = "01312"
        Public Const LOG01313 As String = "01313"
        Public Const LOG01314 As String = "01314"
        Public Const LOG01315 As String = "01315"
        Public Const LOG01316 As String = "01316"
        Public Const LOG01317 As String = "01317"
        Public Const LOG01318 As String = "01318"
        Public Const LOG01319 As String = "01319"
        Public Const LOG01320 As String = "01320"
        Public Const LOG01321 As String = "01321"
        Public Const LOG01322 As String = "01322"
        Public Const LOG01323 As String = "01323"
        Public Const LOG01324 As String = "01324"
        Public Const LOG01325 As String = "01325"
        Public Const LOG01326 As String = "01326"
        Public Const LOG01327 As String = "01327"
        Public Const LOG01328 As String = "01328"
        Public Const LOG01329 As String = "01329"
        Public Const LOG01330 As String = "01330"
        Public Const LOG01331 As String = "01331"
        Public Const LOG01332 As String = "01332"
        Public Const LOG01333 As String = "01333"
        Public Const LOG01334 As String = "01334"
        Public Const LOG01335 As String = "01335"
        Public Const LOG01336 As String = "01336"
        Public Const LOG01337 As String = "01337"
        Public Const LOG01338 As String = "01338"
        Public Const LOG01339 As String = "01339"
        Public Const LOG01340 As String = "01340"
        Public Const LOG01341 As String = "01341"
        Public Const LOG01342 As String = "01342"
        Public Const LOG01343 As String = "01343"
        Public Const LOG01344 As String = "01344"
        Public Const LOG01345 As String = "01345"
        Public Const LOG01346 As String = "01346"
        Public Const LOG01347 As String = "01347"
        Public Const LOG01348 As String = "01348"
        Public Const LOG01349 As String = "01349"
        Public Const LOG01350 As String = "01350"
        Public Const LOG01351 As String = "01351"
        Public Const LOG01352 As String = "01352"
        Public Const LOG01353 As String = "01353"
        Public Const LOG01354 As String = "01354"
        Public Const LOG01355 As String = "01355"
        Public Const LOG01356 As String = "01356"
        Public Const LOG01357 As String = "01357"
        Public Const LOG01358 As String = "01358"
        Public Const LOG01359 As String = "01359"
        Public Const LOG01360 As String = "01360"
        Public Const LOG01361 As String = "01361"
        Public Const LOG01362 As String = "01362"
        Public Const LOG01363 As String = "01363"
        Public Const LOG01364 As String = "01364"
        Public Const LOG01365 As String = "01365"
        Public Const LOG01366 As String = "01366"
        Public Const LOG01367 As String = "01367"
        Public Const LOG01368 As String = "01368"
        Public Const LOG01369 As String = "01369"
        Public Const LOG01370 As String = "01370"
        Public Const LOG01371 As String = "01371"
        Public Const LOG01372 As String = "01372"
        Public Const LOG01373 As String = "01373"
        Public Const LOG01374 As String = "01374"
        Public Const LOG01375 As String = "01375"
        Public Const LOG01376 As String = "01376"
        Public Const LOG01377 As String = "01377"
        Public Const LOG01378 As String = "01378"
        Public Const LOG01379 As String = "01379"
        Public Const LOG01380 As String = "01380"
        Public Const LOG01381 As String = "01381"
        Public Const LOG01382 As String = "01382"
        Public Const LOG01383 As String = "01383"
        Public Const LOG01384 As String = "01384"
        Public Const LOG01385 As String = "01385"
        Public Const LOG01386 As String = "01386"
        Public Const LOG01387 As String = "01387"
        Public Const LOG01388 As String = "01388"
        Public Const LOG01389 As String = "01389"
        Public Const LOG01390 As String = "01390"
        Public Const LOG01391 As String = "01391"
        Public Const LOG01392 As String = "01392"
        Public Const LOG01393 As String = "01393"
        Public Const LOG01394 As String = "01394"
        Public Const LOG01395 As String = "01395"
        Public Const LOG01396 As String = "01396"
        Public Const LOG01397 As String = "01397"
        Public Const LOG01398 As String = "01398"
        Public Const LOG01399 As String = "01399"

    End Class


    Public Class ServiceProviderDefaultLanguage
        Public Const English As String = "E"
        Public Const Chinese As String = "C"
    End Class


    Public Class HCSPMenuType
        Public Const Login As String = "Login"
        Public Const Menu As String = "Menu"
        Public Const TextOnlyVersionMenu As String = "TextOnlyVersion_Menu"
    End Class

    Public Class UserParamCategory
        Public Const General As String = "General"
        Public Const Mail As String = "Mail"
        Public Const Printout As String = "Printout"
        Public Const Report As String = "Report"
    End Class

    Public Class LoginStatus
        Public Const Success As String = "S"
        Public Const Fail As String = "F"
    End Class

    Public Class CultureLanguage
        Public Const TradChinese As String = "zh-tw"
        Public Const SimpChinese As String = "zh-cn"
        Public Const English As String = "en-us"
    End Class

    Public Enum EnumLanguage
        EN
        TC
        SC
    End Enum

#Region "Internet Mail"

    Public Class MailTemplateID
        Public Const AccountActivationEmail As String = "E0001a"
        Public Const SchemeEnrolmentEmail As String = "E0001b"
        Public Const ReimburseNotificationEmailToSP As String = "E0002"
        ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        'Public Const ForgotPasswordEmail As String = "E0003"
        Public Const ResetPasswordEmail As String = "E0003"
        ' CRE16-004 (Enable SP to unlock account) [End][Winnie]
        Public Const NoticeToHCSPToRectifyTemporaryRecipientAccount As String = "E0004"
        Public Const DelistingNotificationMail As String = "E0007"
        Public Const ConfirmationChangeEmail As String = "E0008"
        Public Const DelistingNotificationMailWithLogo As String = "E0009"
        Public Const DelistingNotificationMailWithLogoToken As String = "E0010"
        Public Const DelistingNotificationMailWithToken As String = "E0011"
        Public Const DelistingNotificationMailWithNothing As String = "E0012"
    End Class

    Public Class InternetMailSentStatus
        Public Const Sent As String = "S"
        Public Const Pending As String = "P"
    End Class

    Public Class MailTemplateStatus
        Public Const Active As String = "A"
        Public Const InActive As String = "I"
    End Class

    Public Class InternetMailLanguage
        Public Const ChiHeader As String = "C"
        Public Const EngHeader As String = "E"
        Public Const Both As String = "B"
    End Class

    Public Class InternetMailType
        Public Const HTML As String = "H"
        Public Const Text As String = "T"
    End Class

#End Region

#Region "Inbox"

    Public Class InboxMsgTemplateID
        Public Const ReimbursementNotification As String = "RN_Inbox"
        Public Const HCSPRectifyNotification As String = "Rect_Inbox"
        Public Const VoucherAccSuspendNotification As String = "VASuspend"
        Public Const Level3NotificationIn4LevelAlert As String = "Rect_VR_L3"
        Public Const HCVUDeleteVRAcctAlert As String = "RectFinal1"
        Public Const HCVUDeleteVRAcctTransactionAlert As String = "RectFinal2"
        Public Const HCVUDeleteXVRAcctTransactionAlert As String = "RectFinal3"
        Public Const HCVUReadyToDeleteVRAcctAlert As String = "Rect_Del"
        Public Const HCVUVManualValidateECTempVRAcct As String = "EC_MANUAL "
        Public Const DeathRecordFileImportSuccess As String = "DR_Imp_Suc"
        Public Const DeathRecordFileMatchSuccess As String = "DR_Mat_Suc"
        Public Const DeathRecordFileMatchFail As String = "DR_Mat_Fai"


        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
        Public Const VacccintaionFIleReportVF000 As String = "VF000Rpt"
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]
        ' CRE19-001-04 (PPP 2019-20) [Start][Koala]
        Public Const VacccintaionFIleReportVF001 As String = "VF001Rpt"
        Public Const VacccintaionFIleReportVF002 As String = "VF002Rpt"
        Public Const VacccintaionFIleReportVF003 As String = "VF003Rpt"
        ' CRE19-001-04 (PPP 2019-20) [End][Koala]
    End Class

#End Region

#Region "Schedule Job"

    Public Class ScheduleJobID
        Public Const ExcelGenerator As String = "ExcelGenerator"
        Public Const InternetMail As String = "InternetMail"
        Public Const TextGenerator As String = "TextGenerator"
        Public Const PDFGenerator As String = "PDFGenerator"
        Public Const ImmdValidation As String = "ImmdValidation"
        'Public Const GetPPIEPRToken As String = "GetPPIEPRToken"
        'Public Const TokenReplacement As String = "TokenReplacement"
        Public Const TempVRAcct4LevelAlert As String = "TempVRAcct4LvlAlert"
        Public Const TSWPatientList As String = "TSWPatientList"
        Public Const ConfigDispatcher As String = "ConfigDispatcher"
        Public Const DeathRecordMatching As String = "DeathRecordMatching"
        Public Const ThirdPartyScheduleJob As String = "ThirdPartyScheduleJob"
        Public Const SubsidizeWriteOffGenerator As String = "SubsidizeWriteOffGenerator"
        Public Const SentOutMessage As String = "SentOutMessage"
        Public Const TokenNotification As String = "TokenNotification"
        Public Const PCDStatusUpdater As String = "PCDStatusUpdater" ' CRE17-016 (Checking of PCD status during VSS enrolment) [Chris YIM]
        Public Const StudentFileChecking As String = "StudentFileChecking" ' CRE17-018-03 (New initiatives for VSS and RVP in 2018-19 - Phase 3 - Claim) [Koala]
        Public Const StudentAccountMatching As String = "StudentAccountMatching"  ' CRE17-018 (New initiatives for VSS and RVP in 2018-19)
        ' CRE20-005 (Providing users' data in HCVS to eHR Patient Portal) [Start][Chris YIM]
        Public Const HAServicePatientImporter As String = "HAServicePatientImporter"  ' CRE20-015 (HA Scheme)[Raiman]
        ' ---------------------------------------------------------------------------------------------------------
        Public Const PatientPortalDoctorListGenerator As String = "PatientPortalDoctorListGenerator"
        ' CRE20-005 (Providing users' data in HCVS to eHR Patient Portal) [End][Chris YIM]	
        Public Const COVID19Exporter As String = "COVID19Exporter" 'CRE20-0023 (Immu record)[Martin Tang]
        Public Const COVID19MECExporter As String = "COVID19MECExporter"  'CRE20-0023-73 (COVID19 - Medical Exemption) [Winnie SUEN]

        Public Const COVID19BatchConfirm As String = "COVID19BatchConfirm"  'CRE20-0023 (Immu record)[Winnie SUEN]
        Public Const COVID19DischargeImporter As String = "COVID19DischargeImporter"  ' CRE20-023 (Immu)[Raiman]
    End Class

    Public Class ScheduleJobFunctionCode
        Public Const ExcelGenerator As String = "019901"
        Public Const InternetMail As String = "019902"
        Public Const TextGenerator As String = "019903"
        Public Const PDFGenerator As String = "019904"
        Public Const ImmdValidation As String = "019905"
        Public Const TempVRAcct4LevelAlert As String = "019906"
        Public Const GetTSWPatientList As String = "019907"
        'Public Const GetPPIEPRToken As String = "019908"
        Public Const ConfigDispatcher As String = "019909"
        Public Const DeathRecordMatching As String = "019910"
        Public Const ThirdPartyScheduleJob As String = "019911"
        Public Const SubsidizeWriteOffGenerator As String = "019912"
        Public Const SentOutMessage As String = "019916"
        Public Const TokenNotification As String = "019917"
        Public Const PCDStatusUpdater As String = "019918"  ' CRE17-016 (Checking of PCD status during VSS enrolment) [Chris YIM]
        Public Const StudentFileChecking As String = "019919" ' CRE17-018-03 (New initiatives for VSS and RVP in 2018-19 - Phase 3 - Claim) [Koala]
        Public Const StudentAccountMatching As String = "019920"  ' CRE17-018 (New initiatives for VSS and RVP in 2018-19)
        Public Const PatientPortalDoctorListGenerator As String = "019921" ' CRE18-XXX (Provide data to eHR Portal) [Chris YIM]
        Public Const HAServicePatientImporter As String = "019922" ' CRE20-015 (HA Scheme)[Raiman]
        Public Const COVID19Exporter As String = "019923" 'CRE20-0022 (Immu record)[Martin Tang]
        Public Const COVID19eHRIntegration As String = "019924" 'CRE20-0022 (Immu record)[Martin Tang]
        Public Const COVID19DischargeImporter As String = "019925"  ' CRE20-023 (Immu)[Raiman]
        Public Const COVID19MECExporter As String = "019926"

    End Class

    Public Class ScheduleJobLogStatus
        Public Const Information As String = "Information"
        Public Const Success As String = "Success"
        Public Const Fail As String = "Fail"
    End Class

    Public Class ScheduleJobSetting
        Public Const ActiveServer As String = "ScheduleJobActiveServer"
    End Class

    Public Class ImmdProcessStatus
        Public Const Pend As String = "P"
        Public Const Export As String = "E"
        Public Const Import As String = "I"
        Public Const Complete As String = "C"
    End Class

#End Region

#Region "DataDownload & Report Submission"

    Public Class FileDownloadAccessType
        Public Const All As String = "A"
        Public Const Generate As String = "G"
        Public Const Download As String = "D"
    End Class

    Public Class FileDownloadStatus
        Public Const NotDownloadYet As String = "N"
        Public Const Downloaded As String = "D"
        Public Const Deleted As String = "I"
    End Class

    ' CRE15-016 (Randomly genereate the valid claim transaction) [Start][Winnie]
    Public Class FileGenerationQueueStatus
        Public Const Pending As String = "P"
        Public Const GenError As String = "E"
        Public Const Completed As String = "C"
        Public Const Inactive As String = "I"
        Public Const Terminated As String = "T"
    End Class

    Public Class SubmissionReportType
        Public Const PostPaymentCheck As String = "P" 'In "Report And Download", Post Payment Check
        Public Const ReportSubmission As String = "R" 'In "Report And Download", Report Submission
        Public Const StatisticEnquiry As String = "S" 'In "Report And Download", Statistic Enquiry
    End Class
    ' CRE15-016 (Randomly genereate the valid claim transaction) [End][Winnie]

    Public Class DataDownloadFileType
        ' CRE13-016 - Upgrade to excel 2007 [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        'Public Const Excel As String = "XLS"
        Public Const XLS As String = "XLS"
        Public Const XLSX As String = "XLSX"
        ' CRE13-016 - Upgrade to excel 2007 [End][Tommy L]
        Public Const PDF As String = "PDF"
        Public Const Text As String = "TXT"
    End Class

    Public Class DataDownloadFileID
        Public Const BankPaymentFile As String = "BANK"
        Public Const SuperDownload As String = "SUPER"
        Public Const SuperDownloadRMB As String = "SUPERRMB" ' CRE13-019-02 Extend HCVS to China [Lawrence]
        Public Const SuperDownloadSSSCMC As String = "SUPERSSSCMC" 'CRE20-015 (Special Support Scheme) [Martin]
        Public Const BoardAndCouncil As String = "BNC"
        Public Const EnrolmentDownload As String = "Enrolment"
        Public Const RMPDownload As String = "RMPDownload"
        Public Const VoucherClaimFile As String = "eHSD0001"
        'CRE13-001 EHAPP [Start][Karl]
        '------------------------------------------------------
        Public Const EHAPPFile As String = "eHSD0023"
        'CRE13-001 EHAPP [End][Karl]
        Public Const VaccineClaimFile As String = "VaccineClaim"
        'Added by Eric Tse on 21 Oct 2010
        Public Const VaccineClaimEVSSFile As String = "eHSD0002"
        Public Const VaccineClaimCIVSSFile As String = "eHSD0003"
        'CRE13-017-05 CVSSPCV13 [Start][Karl]
        Public Const VaccineClaimCVSSPCV13File As String = "eHSD0025"
        'CRE13-017-05 CVSSPCV13 [End][Karl]
        'CRE14-017-04 OMPCV13E [Start][Winnie]
        Public Const VaccineClaimOMPCV13EFile As String = "eHSD0026"
        'CRE14-017-04 OMPCV13E [End][Winnie]
        'CRE15-005-03 PIDVSS [Start][Winnie]
        Public Const VaccineClaimPIDVSSFile As String = "eHSD0027"
        'CRE15-005-03 PIDVSS [End][Winnie]
        'Modify by Eric Tse on 21 Oct 2010
        Public Const VaccineClaimRVPFile As String = "eHSD0004"
        'End of section         
        Public Const eHSD0014 As String = "eHSD0014"
        Public Const PreAuthorizationChecking As String = "PreAuthorizationCheck"
        Public Const PreAuthorizationCheckingRMB As String = "PreAuthorizationCheckRMB" ' CRE13-019-02 Extend HCVS to China [Lawrence]
        Public Const PreAuthorizationCheckingSSSCMC As String = "PreAuthorizationCheckSSSCMC" 'CRE20-015 (Special Support Scheme) [Martin]
        ' Aberrant Report
        Public Const SPFrequentRejectionFile As String = "eHSW0001"
        Public Const VoucherRecipientAberrantPatternFile As String = "eHSD0015"
        Public Const SPAberrantPatternFile As String = "eHSD0016"
        Public Const eHSW0002 As String = "eHSW0002"
        Public Const eHSM0003 As String = "eHSM0003"

        Public Const eHSD0017 As String = "eHSD0017" ' (CMS Vaccination Record Service) Connection Failed Transaction

        ' CRE15-016 (Randomly genereate the valid claim transaction) [Start][Winnie]
        Public Const PPC0001 As String = "PPC0001"
        Public Const PPC0002 As String = "PPC0002"
        Public Const PPC0003 As String = "PPC0003"
        ' CRE15-016 (Randomly genereate the valid claim transaction) [End][Winnie]

        'CRE16-019 (To implement token sharing between eHS(S) and eHRSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        ' Deactivated eHRSS Token Report
        Public Const eHSD0029 As String = "eHSD0029"
        Public Const eHSD0030 As String = "eHSD0030" 'CRE16-016 Voucher aberrant and new monitoring [Koala]
        Public Const eHSM0004 As String = "eHSM0004"
        'CRE16-019 (To implement token sharing between eHS(S) and eHRSS) [End][Chris YIM]

        ' --- CRE16-026-04 (Add PCV13) [Start] (Marco) ---
        Public Const eHSM0007 As String = "eHSM0007"
        ' --- CRE16-026-04  (Add PCV13) [End] (Marco) ---

        'CRE17-018 (New initiatives for VSS and RVP in 2018-19) [Start]	[Marco CHOI]
        Public Const eHSSF001 As String = "eHSSF001"
        Public Const eHSSF001B As String = "eHSSF001B"
        Public Const eHSSF002A As String = "eHSSF002A"
        Public Const eHSSF002B As String = "eHSSF002B"
        Public Const eHSSF003 As String = "eHSSF003"
        Public Const eHSSF004 As String = "eHSSF004"
        Public Const eHSSF005 As String = "eHSSF005"
        'CRE17-018 (New initiatives for VSS and RVP in 2018-19) [End]	[Marco CHOI]

        ' CRE19-001-04 (PPP 2019-20) [Start][Koala]
        Public Const eHSVF000 As String = "eHSVF000"
        Public Const eHSVF001 As String = "eHSVF001"
        Public Const eHSVF002 As String = "eHSVF002"
        Public Const eHSVF003 As String = "eHSVF003"
        Public Const eHSVF004 As String = "eHSVF004"
        Public Const eHSVF005 As String = "eHSVF005"
        Public Const eHSVF006 As String = "eHSVF006"
        ' CRE19-001-04 (PPP 2019-20) [End][Koala]

        ' CRE19-022 Inspection Module [Start][Winnie]
        ' Inspection Report 
        Public Const INSP0001 As String = "INSP0001" ' Search Result
        ' CRE19-022 Inspection Module [End][Winnie]

    End Class

    ' To Be Confirm The Report Submission File
    Public Class ReportSubmissionFileID
        Public Const RR01 As String = "RR01"
        Public Const RR2 As String = "RR2"
        Public Const RR3 As String = "RR3"
        Public Const AR1 As String = "AR1"
        Public Const AR2 As String = "AR2"
        Public Const AR3 As String = "AR3"
        Public Const DetailedPaymentAnalysisRptID As String = "DPA1"
        Public Const ClaimTransactions As String = "ClaimTransactions"
        Public Const SPEnrolRecord As String = "SPEnrolmentRecord"
        Public Const eHSD0014 As String = "eHSD0014"
        Public Const eHSU0001 As String = "eHSU0001" ' CRE11-014
    End Class

#End Region

    Public Class DBFlagStr
        Public Const DBFlag As String = "DBFlag"
        Public Const DBFlag2 As String = "DBFlag2"
        Public Const DBFlag3 As String = "DBFlag3"
        Public Const DBFlag4 As String = "DBFlag4"
        Public Const ReadDBFlag As String = "DBFlagRead"
        Public Const DBFlagInterfaceLog As String = "DBFlag_dbEVS_InterfaceLog"
    End Class

    Public Class PersonalInfoHistoryActionType
        Public Const Create As String = "C"
        Public Const Merge As String = "M"
        Public Const Amendment As String = "A"
    End Class

    Public Class PersonalInfoRecordStatus
        Public Const Active As String = "A"
        Public Const Erased As String = "E"
        Public Const ValidationFailed As String = "I"
        Public Const PendingValidation As String = "V"
    End Class

    Public Class StaticDataColumnName
        Public Const PracticeType As String = "PRACTICETYPE"
        Public Const Profession As String = "PROFESSION"
        Public Const Scheme As String = "SCHEME"
        Public Const SPAction As String = "SPACTION"
        Public Const TokenDisable As String = "TOKENDISABLE"
        Public Const VoidTransReason As String = "VOIDTRANSREASON"
    End Class

    Public Class SPDataEntryStatus
        Public Const Unprocessed As String = "U"
        Public Const Processing As String = "P"
        Public Const Enrolled As String = "E"
        Public Const ClassCode As String = "SPDataEntryStatus"
    End Class

    Public Class SchemeInformationStatus
        Public Const Active As String = "A"
        Public Const Suspended As String = "S"
        Public Const Delisted As String = "D"

        'Public Const DelistedInvoluntary As String = "I"
        'Public Const DelistedVoluntary As String = "V"
        'Public Const PendingDelist As String = "D"
        'Public Const PendingSuspend As String = "S"
        'Public Const PendingReactivate As String = "R"
        'Public Const SuspendPendingDelist As String = "X"
    End Class

    Public Class SchemeInformationMaintenanceDisplayStatus
        Public Const Active As String = "A"
        Public Const Suspended As String = "W"
        Public Const DelistedVoluntary As String = "V"
        Public Const DelistedInvoluntary As String = "I"
        Public Const ActivePendingSuspend As String = "S"
        Public Const ActivePendingDelist As String = "D"
        Public Const SuspendedPendingDelist As String = "X"
        Public Const SuspendedPendingReactivate As String = "Y"

        Public Const ClassCode As String = "SchemeInformationMaintenanceDisplayStatus"
    End Class

    Public Class MedicalOrganizationStatus
        Public Const Active As String = "A"
        Public Const Delisted As String = "D"

        Public Const ClassCode As String = "MedicalOrganizationStatus"
    End Class

    Public Class MedicalOrganizationStagingStatus
        Public Const Active As String = "A"
        Public Const Existing As String = "E"
        Public Const Update As String = "U"
        Public Const Delisted As String = "D"
        Public Const Reject As String = "R"
        Public Const Merged As String = "M"

        Public Const ClassCode As String = "MedicalOrganizationStagingStatus"
    End Class

    Public Class PracticeSchemeInfoStagingStatus
        Public Const Active As String = "A"
        Public Const Existing As String = "E"
        Public Const Update As String = "U"
        Public Const Suspended As String = "W"
        Public Const DelistedVoluntary As String = "V"
        Public Const DelistedInvoluntary As String = "I"
        Public Const ActivePendingSuspend As String = "SP"
        Public Const ActivePendingDelist As String = "DP"
        Public Const SuspendedPendingDelist As String = "XP"
        Public Const SuspendedPendingReactivate As String = "YP"
        Public Const Reject As String = "R"
        Public Const Merged As String = "M"

        Public Const ClassCode As String = "PracticeSchemeInfoStagingStatus"
    End Class

    Public Class ProfessionalStatus
        Public Const Active As String = "A"
        Public Const Delisted As String = "D"
    End Class

    Public Class PracticeSchemeInfoStatus
        Public Const Active As String = "A"
        Public Const Suspended As String = "S"
        Public Const Delisted As String = "D"

        'Public Const DelistedInvoluntary As String = "I"
        'Public Const DelistedVoluntary As String = "V"
        'Public Const PendingDelist As String = "DP"
        'Public Const PendingSuspend As String = "SP"
        'Public Const PendingReactivate As String = "RP"
        'Public Const SuspendPendingDelist As String = "TP"
    End Class

    Public Class PracticeSchemeInfoMaintenanceDisplayStatus
        Public Const Active As String = "A"
        Public Const Suspended As String = "W"
        Public Const DelistedVoluntary As String = "V"
        Public Const DelistedInvoluntary As String = "I"
        Public Const ActivePendingSuspend As String = "SP"
        Public Const ActivePendingDelist As String = "DP"
        Public Const SuspendedPendingDelist As String = "XP"
        Public Const SuspendedPendingReactivate As String = "YP"

        Public Const ClassCode As String = "PracticeSchemeInfoMaintenanceDisplayStatus"
    End Class

    Public Class IVSSDosage
        Public Const FirstDose As String = "1"
        Public Const SecondDose As String = "2"
        Public Const ClassCode As String = "IVSSDosage"
    End Class

    Public Class SPMigrationStatus
        Public Const NotMigrated As String = "N"
        Public Const ReadyToMigrate As String = "R"
        Public Const Processed As String = "P"
    End Class

    Public Class ServiceCategoryCode
        Public Const ENU As String = "ENU"
        Public Const RCM As String = "RCM"
        Public Const RCP As String = "RCP"
        Public Const RDT As String = "RDT"
        Public Const RMP As String = "RMP"
        Public Const RMT As String = "RMT"
        Public Const RNU As String = "RNU"
        Public Const ROT As String = "ROT"
        Public Const RPT As String = "RPT"
        Public Const RRD As String = "RRD"
        Public Const ALL As String = "ALL"
    End Class

    'Public Class MasterSchemeCode
    '    Public Const CIVSS As String = "CIVSS"
    '    Public Const EVSS As String = "EVSS"
    '    Public Const HCVS As String = "HCVS"
    '    Public Const RVS As String = "RVS"
    'End Class

    'Public Class VoucherSchemeCode
    '    Public Const CSFV As String = "CSFV"
    '    Public Const EHCVS As String = "EHCVS"
    '    Public Const ESFV As String = "ESFV"
    '    Public Const HSIV As String = "HSIV"
    '    Public Const PV As String = "PV"
    '    Public Const RPV As String = "RPV"
    '    Public Const RHSI As String = "RHSI"
    'End Class


    'Don't use this enum class (Only for back-compatitible purpose)
    Public Class SchemeBackOfficeSchemeCode
        Public Const CIVSS As String = "CIVSS"
    End Class

    Public Class SPVettingStatus
        Public Const Active As String = "E"
        Public Const Defer As String = "D"
        Public Const ClassCode As String = "SPVettingStatus"
    End Class

    Public Class DataMigrationStatus
        Public Const Processed As String = "R"
        Public Const Unprocessed As String = "N"

        Public Const ClassCode As String = "DataMigrationStatus"
    End Class

    Public Class RVPHomeListStatus
        Public Const Active As String = "Y"
        Public Const Inactive As String = "N"

        Public Const ClassCode As String = "RCHStatus"
    End Class

    'CRE20-023 Immue Record COVID19 [Start][Martin]
    Public Class OutreachListStatus
        Public Const Active As String = "A"
        Public Const Inactive As String = "S"

        Public Const ClassCode As String = "OutreachStatus"
    End Class
    'CRE20-023 Immue Record COVID19 [End][Martin]


    'CRE20-022 Immue Record COVID19 [Start][Raiman]
    Public Class VaccineLotRequestStatus
        Public Const RequestAssign As String = "A"
        Public Const RequestNew As String = "N"
        Public Const RequestRemove As String = "R"

        Public Const RequstTypeClassCode As String = "VaccineLotRequestType"
    End Class
    'CRE20-022 Immue Record COVID19 [End][Raiman]


#Region "Print Letter"
    Public Class ApplicantType
        'CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Public Const Applicant_of_fresh As Integer = 1
        'Public Const Applicant_of_PPiePR As Integer = 2
        'Public Const Applicant_of_eHS As Integer = 3
        'Public Const Applicant_of_eHS_PPiePR As Integer = 4

        Public Const NewEnrolment_EHSS_Token As Integer = 1
        Public Const NewEnrolment_EHRSS_Token As Integer = 2
        Public Const SchemeEnrolment_EHSS_Token As Integer = 3
        Public Const SchemeEnrolment_EHRSS_Token As Integer = 4
        'CRE15-019 (Rename PPI-ePR to eHRSS) [End][Chris YIM]

        'Public Const Fresh_of_HCVS_IVSS As Integer = 1
        'Public Const Fresh_of_IVSS As Integer = 2
        'Public Const Fresh_of_HCVS As Integer = 3
        'Public Const HCVS_Join_IVSS As Integer = 4
        'Public Const HCVS_Optin_IVSS As Integer = 5
        'Public Const HCVS_PPiePR_Join_IVSS As Integer = 6
        'Public Const HCVS_PPiePR_Optin_IVSS As Integer = 7
        'Public Const IVSS_Optin As Integer = 8
        'Public Const PPiePR_Optin_IVSS As Integer = 9
        'Public Const PPiePR_Join_IVSS As Integer = 10
        'Public Const PPiePR_Join_HCVS_IVSS As Integer = 11
        'Public Const PPiePR_Join_HCVS As Integer = 12
    End Class

    Public Class PrintFunctionStatus
        Public Const ClosePrintFunction As String = "C"
        Public Const ActivePrintFunction As String = "A"
        Public Const FinishPrintFunction As String = "F"

        ' CRE16-018 Display SP tentative email in HCVU [Start][Winnie]
        Public Const ErrorPrintFunction As String = "E"
        ' CRE16-018 Display SP tentative email in HCVU [End][Winnie]
    End Class
#End Region

#Region "HCSP - eHS Account Rectification"
    Public Class HCSPeHSAccRectificationStatus
        Public Const PendingToConfirm As String = "C"
        Public Const PendingToVerify As String = "P"
        Public Const InvalidValid As String = "I"
        Public Const PendingToSpecialAcc As String = "S"

        Public Const ClassCode As String = "HCSPeHSAccRectificationStatus"

    End Class
#End Region
    ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [Start][Koala]

    Public Class HCVUeHSAccRectificationStatus
        Public Const PendingToConfirm As String = "C"
        Public Const PendingToVerify As String = "P"
        Public Const ValidationFailed As String = "I"
        Public Const NoImmDValidation As String = "R"
        Public Const PendingToSpecialAcc As String = "S"

        Public Const ClassCode As String = "HCVUeHSAccRectificationStatus"

    End Class

    ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [End][Koala]

    Public Class ReportConnection
        Public Const Primary As String = "P"
        Public Const Secondary As String = "S"
        Public Const Report As String = "R"
    End Class

    Public Class YesNo
        Public Const Yes As String = "Y"
        Public Const No As String = "N"
    End Class

    Public Class HAVaccinationRecordStatus
        Public Const FullMatchWithRecord As String = "YFY"
        Public Const FullMatchWithoutRecord As String = "YFN"
        Public Const DocumentNoNotFound As String = "YNN"
        Public Const DemographicNotMatch As String = "YPN"
        Public Const DocumentNotAccept As String = "YDN"
        Public Const ConnectionFail As String = "YCN"
        Public Const Suspend As String = "YUN"
        Public Const NotAvailable As String = ""
        Public Const OldRecord As String = "NSN"
        Public Const ClassCode As String = "HAVaccinationRecordStatus"
    End Class

    ' CRE12-001 eHS and PCD integration [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    Public Enum EnumEnrolCopy
        Enrolment
        Original
    End Enum
    ' CRE12-001 eHS and PCD integration [End][Koala]


    ' CRE13-006 HCVS Ceiling [Start][Karl]
    ' -----------------------------------------------------------------------------------------
    Public Enum HandleWriteOffMode
        MissingSeasonOnly
        SpecificSeason
    End Enum

    ' CRE13-006 HCVS Ceiling [End][Karl]

    'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Class eHASubsidizeWriteOff_CreateReason
        Public Const TxEnquiry As String = "TE"
        Public Const TxCreation As String = "TC"
        Public Const TxBackSeasonCreation As String = "TB"
        Public Const TxRemoval As String = "TR"
        Public Const PersonalInfoCreation As String = "AC"
        Public Const PersonalInfoAmend As String = "AA"
        Public Const PersonalInfoRemoval As String = "AR"
        Public Const DeathRecordImport As String = "DI"
        Public Const DeathRecordRemoval As String = "DR"
    End Class
    'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

    ' CRE13-015 - add URL for new EAI server [Start][Karl]
    Public Class CMSVaccineHealthCheck
        Public Const FuncCode As String = "HEALTH%i"
    End Class

    Public Enum CIMSEndpoint
        DHSITE
        EMULATE
    End Enum

    Public Enum EndpointEnum
        WEBLOGIC
        EAIWSPROXY
        EMULATE
    End Enum
    ' CRE13-015 - add URL for new EAI server [End][Karl]

    'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
    Public Class EHAPP_Copayment
        Public Const VOUCHER As String = "HCV"
        Public Const HUNDREDCASH As String = "F100"
        Public Const CSSA As String = "CSSA"
        Public Const MEDWAIVE As String = "MED_WAIVE "
    End Class
    'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]

    ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
    Public Enum EnumHCSPSubPlatform
        NA
        HK
        CN
    End Enum
    ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

    'CRE15-004 TIV & QIV [Start][Winnie]
    Public Class SubsidizeGroupBackOfficeStatus
        Public Const Active As String = "A"
        Public Const Expired As String = "E"
        Public Const Inactive As String = "I"
    End Class

    Public Class SystemGenerateRecord
        Public Const Username As String = "SYS"
    End Class
    'CRE15-004 TIV & QIV [End][Winnie]

    'INT15-0015 (Fix date format checking in HCVU) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Class DateValidation
        Public Const YearMaxValue As Integer = 9999
        Public Const YearMinValue As Integer = 1753
    End Class
    'INT15-0015 (Fix date format checking in HCVU) [End][Chris YIM]

    ' CRE15-022 (Change of parameter maintenance) [Start][Winnie]
    Public Class Parm_ApplyLimit
        Public Const Numeric As String = "NUMERIC"
        Public Const BOUserID As String = "BO_USER_ID"  ' Back Office User ID
    End Class

    Public Class Parm_ExternalUse
        Public Const Yes As String = "Y"        ' Can be read and modify in Parm Maint
        Public Const Read_Only As String = "R"  ' Can be read but canont be modified in Parm Maint
        Public Const No As String = "N"         ' Cannot be read or modified in Parm Maint
    End Class
    ' CRE15-022 (Change of parameter maintenance) [End][Winnie]

    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Class RCH_TYPE
        Public Const RCHE As String = "E"
        Public Const RCHD As String = "D"
        Public Const IPID As String = "I"
        Public Const RCCC As String = "C"
    End Class
    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

    Public Class TYPE_OF_OUTREACH
        Public Const RCH As String = "RCH"
        Public Const OTHER As String = "O"
    End Class

    Public Class RECIPIENT_TYPE
        Public Const RESIDENT As String = "RESIDENT"
        Public Const RCH_STAFF As String = "RCH_STAFF"
        Public Const CCSU_STAFF As String = "CCSU_STAFF"
    End Class

    Public Class CategoryCode
        Public Const VSS_CHILD As String = "VSSCHILD"
        Public Const VSS_DA As String = "VSSDA"
        Public Const VSS_ADULT As String = "VSSADULT"
        Public Const VSS_ELDER As String = "VSSELDER"
        Public Const VSS_PID As String = "VSSPID"
        Public Const VSS_PW As String = "VSSPREG"
        Public Const EVSSO_CHILD As String = "EVSSOCHILD"
        Public Const PPP_CHILD As String = "PPPCHILD"
        Public Const PPPKG_CHILD As String = "PPPKGCHILD"
        ' CRE20-0023 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Const VSS_NIA As String = "VSSNIA"
        Public Const VSS_VC As String = "VSSVC"
        Public Const VSS_COVID19 As String = "VSSCOVID19"
        Public Const VSS_COVID19_Outreach As String = "VSSC19O"
        Public Const RVP_COVID19 As String = "RVPCOVID19"
        ' CRE20-0023 (Immu record) [End][Chris YIM]
    End Class

    Public Enum Aspect
        Transaction
        ServiceProvider
        eHSAccount
        AdvancedSearch
        SpecialAdvancedSearch 'CRE20-XXX Claim transaction improvement [Nichole]
    End Enum

    'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Enum GetTotalEntitlement
        ByLastDayOfClaimPeriod
        ByServiceDate
    End Enum

    Public Enum ProcessedBySystem
        eHS
    End Enum
    'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Class SmartIDVersion
        Public Const IDEAS1 As String = "10"
        Public Const IDEAS2 As String = "20"
        Public Const IDEAS2_WithGender As String = "25"
        Public Const IDEAS_Combo_Old As String = "10C"
        Public Const IDEAS_Combo_New As String = "20C"
        Public Const IDEAS_Combo_New_WithGender As String = "25C"
    End Class
    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

    ' CRE19-026 (HCVS hotline service) [Start][Winnie]
    ' ------------------------------------------------------------------------
    Public Enum EnumHCVUSubPlatform
        NA
        BO  'Back Office
        CC  'Call Centre
        VC  'Vaccination Centre
    End Enum
    ' CRE19-026 (HCVS hotline service) [End][Winnie]

    ' I-CRE20-001 (Mid term solution to handle special characters in HA_MingLiu) [Start][Winnie]
    Public Enum EnumMappingCodeType
        ' PCD
        ' mapping for corresponding web service return code
        WS_PCD_CheckAccountStatus_Return_Code
        WS_PCD_CheckIsActiveSP_Return_Code
        WS_PCD_CreatePCDSPAcct_Return_Code
        WS_PCD_HealthCheck_Return_Code
        WS_PCD_TransferPracticeInfo_Return_Code
        WS_PCD_UploadEnrolInfo_Return_Code
        WS_PCD_CheckForVerifiedEnrolment_Return_Code
        WS_PCD_UploadVerifiedEnrolment_Return_Code

        ' mapping for platform code
        WS_PCD_Platform_Code
        PCDEnrolmentFormProfessional_TC
        PCDEnrolmentFormProfessional_EN

        ' Font
        FONT_Unicode_Mingliu_to_HAMingliu
    End Enum
    ' I-CRE20-001 (Mid term solution to handle special characters in HA_MingLiu) [End][Winnie]

    ' CRE19-022 (Inspection management) [Start][Golden]
    Public Class InspectionStatus
        Public Const Creating As String = ""
        Public Const Closed As String = "C"
        Public Const Incomplete As String = "I"
        Public Const InspectionResultInputted As String = "RI"
        Public Const ClosePendingApproval As String = "CC"
        Public Const PendingForSiteVisit As String = "PV"
        Public Const ReopenPendingApproval As String = "CO"
        Public Const RemovePendingApproval As String = "CD"
        Public Const Removed As String = "D"
    End Class
    Public Class RoleType
        Public Const InspectionObserver As String = "19"
        Public Const InspectionOfficer As String = "20"
        Public Const InspectionEndorser As String = "21"
        Public Const InspectionSEO As String = "22"

        ' CRE20-015-02 (Special Support Scheme) [Start][Winnie]
        Public Const SSSCMCReimbursement As String = "23"
        Public Const SSSCMCAdmin As String = "24"
        Public Const SSSCMCEnquiry As String = "25"
        ' CRE20-015-02 (Special Support Scheme) [End][Winnie]

        ' CRE20-023 (Special Support Scheme) [End][Raiman]
        Public Const CentreVaccineLotAdmin As String = "27"
        Public Const CentreVaccineLotSupervisor As String = "28"
        ' CRE20-023 (Special Support Scheme) [End][Raiman]


    End Class

    Public Class InspectionReportType
        Public Const InternalReference As String = "InternalReference"
        Public Const ConfirmationLetter As String = "ConfirmationLetter"
        Public Const InspectionSummary As String = "InspectionSummary"
    End Class

    ' CRE19-022 (Inspection management) [End][Golden]

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Enum ClaimMode
        All = 1
        DHC = 2
        COVID19 = 3
    End Enum
    ' CRE20-0022 (Immu record) [End][Chris YIM]

    Public Class StaticDataStatus
        Public Const Active As String = "A"
        Public Const Inactive As String = "I"
    End Class
End Namespace
