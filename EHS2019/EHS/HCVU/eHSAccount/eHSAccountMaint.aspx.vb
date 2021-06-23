Imports System.Web.Security.AntiXss
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.eHealthAccountDeathRecord
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.HCVUUser
Imports Common.Component.Practice
Imports Common.Component.RedirectParameter
Imports Common.Component.Scheme
Imports Common.Component.ServiceProvider
Imports Common.Component.SortedGridviewHeader
Imports Common.DataAccess
Imports Common.Format
Imports Common.Validation
Imports Common.Component.PassportIssueRegion


Partial Public Class eHSAccountMaint
    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    ' -------------------------------------------------------------------------
    'Inherits System.Web.UI.Page
    'Inherits BasePageWithGridView
    Inherits BasePageWithControl
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    Private udtAuditLogEntry As AuditLogEntry
    Private udtSM As Common.ComObject.SystemMessage
    Private udtValidator As Validator = New Validator
    Private udtformatter As Common.Format.Formatter = New Common.Format.Formatter
    Private udtCommonFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
    Private udteHSAccountMaintBLL As eHSAccountMaintBLL = New eHSAccountMaintBLL
    Private udtEHSAccountBLL As EHSAccountBLL = New EHSAccountBLL
    Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL
    Private udtEHSAccount As EHSAccountModel
    Private udtEHSAccount_Amendment As EHSAccountModel
    Private udtSessionHandlerBLL As New BLL.SessionHandlerBLL
    Private udtSchemeClaimBLL As SchemeClaimBLL = New SchemeClaimBLL
    Dim udtPassportIssueRegionBLL As PassportIssueRegionBLL = New PassportIssueRegionBLL

#Region "Audit Log Description"
    Public Class AuditLogDesc
        Public Const eHSAccountMaintPageLoad = "eHealth Account Maintenance Loaded"  '0
        'Public Const SearchRoute1 = "Search By Route 1"   '1
        'Public Const SearchRoute1Success = "Search By Route 1 Success"   '2
        'Public Const SearchRoute1Fail = "Search By Route 1 Fail"   '3
        'Public Const SearchRoute2 = "Search By Route 2"   '4
        'Public Const SearchRoute2Success = "Search By Route 2 Success"  '5
        'Public Const SearchRoute2Fail = "Search By Route 2 Fail"  '6
        Public Const SearchFail = "Search Fail" '7
        Public Const SelectEHSAccount As String = "Select and view eHealth Account"   '8
        Public Const SelectEHSAccountSuccess As String = "Select and view eHealth Account Success"    '9
        Public Const SelectEHSAccountFail As String = "Select and view eHealth Account Fail"    '10
        'Amendment History
        Public Const GetAmendmentHistory As String = "Get Amendment History" '11
        Public Const GetAmendmentHistorySuccess As String = "Get Amendment History Success" '12
        Public Const GetAmendmentHistorySuccessNoRecordFound As String = "Get Amendment History Success No record Found" '13
        Public Const GetAmendmentHistoryFail As String = "Get Amendment History Fail" '14
        'Amendment
        Public Const AmendEHSAccountRecord As String = "Amend eHealth Account Record"  '15
        Public Const SaveEHSAccount As String = "Save Amended eHealth Account" '16
        '------------------- Check Inout data (Personal particulars) ------------------------------------------------
        Public Const ValidateAccountDetailInfo As String = "Validate Account Detail Info (Personal Particulars)" '17
        Public Const ValidateAccountDetailInfoComplete As String = "Validate Account Detail Info (Personal Particulars) Complete" '18
        Public Const ValidateAccountDetailInfoFail As String = "Validate Account Detail Info (Personal Particulars) Fail" '19
        '------------------------------------------------------------------------------------------------------------
        Public Const ConfirmSaveEHSAccount As String = "Confirm Save Amending eHealth Account" '20
        Public Const ConfirmSaveEHSAccountComplete As String = "Confirm Save Amending eHealth Account Complete" '21
        Public Const ConfirmSaveEHSAccountFail As String = "Confirm Save Amending eHealth Account Fail" '22
        'Suspend Public Enquiry
        Public Const SuspendEnquiryClick As String = "Suspend Enquiry Button Click" '23
        Public Const SaveSuspendEnquiryClick As String = "Save Suspend Enquiry Click" '89
        Public Const SaveSuspendEnquiry As String = "Save Suspend Enquiry" '24
        Public Const SaveSuspendEnquirySuccess As String = "Save Suspend Enquiry Success" '25
        Public Const SaveSuspendEnquiryFail As String = "Save Suspend Enquiry Fail" '26
        Public Const SaveSuspendEnquiryFailNoReasonProvided As String = "Save Suspend Enquiry Fail due to absent of reason" '27
        Public Const CancelSuspendEnquiry As String = "Cancel Suspend Enquiry" '28
        'Reactive Public Enquiry
        Public Const ReactiveEnquiryClick As String = "Reactive Enquiry Button Click" '29
        Public Const SaveReactiveEnquiryClick As String = "Save Reactive Enquiry Click" '90
        Public Const SaveReactiveEnquiry As String = "Save Reactive Enquiry" '30
        Public Const SaveReactiveEnquirySuccess As String = "Save Reactive Enquiry Success" '31
        Public Const SaveReactiveEnquiryFail As String = "Save Reactive Enquiry Fail" '32
        Public Const SaveReactiveEnquiryFailNoReasonProvided As String = "Save Suspend Reactive Fail due to absent of reason" '33
        Public Const CancelReactiveEnquiry As String = "Cancel Reactive Enquiry" '34
        'Suspend Account
        Public Const SuspendAccountClick As String = "Suspend Account Button Click" '35
        Public Const SaveSuspendAccountClick As String = "Save Suspend Account Click" '91
        Public Const SaveSuspendAccount As String = "Save Suspend Account" '36
        Public Const SaveSuspendAccountSuccess As String = "Save Suspend Account Success" '37
        Public Const SaveSuspendAccountFail As String = "Save Suspend Account Fail" '38
        Public Const SaveSuspendAccountFailNoReasonProvided As String = "Save Suspend Account Fail due to absent of reason" '39
        Public Const CancelSuspendAccount As String = "Cancel Suspend Account" '40
        'Reactivate Account
        Public Const ReactivateAccountClick As String = "Reactivate Account Button Click" '41
        Public Const SaveReactivateAccountClick As String = "Save Reactivate Account Click" '92
        Public Const SaveReactivateAccount As String = "Save Reactivate Account" '42
        Public Const SaveReactivateAccountSuccess As String = "Save Reactivate Account Success" '43
        Public Const SaveReactivateAccountFail As String = "Save Reactivate Account Fail" '44
        Public Const SaveReactivateAccountFailNoReasonProvided As String = "Save Reactivate Account Fail due to absent of reason" '45
        Public Const CancelReactivateAccount As String = "Cancel Reactivate Account" '46
        'Terminate Account
        Public Const TerminateAccountClick As String = "Terminate Account Button Click" '47
        Public Const SaveTerminateAccounttClick As String = "Save Terminate Account Click" '93
        Public Const SaveTerminateAccount As String = "Save Terminate Account" '48
        Public Const SaveTerminateAccountSuccess As String = "Save Terminate Account Success" '49
        Public Const SaveTerminateAccountFail As String = "Save Terminate Account Fail" '50
        Public Const SaveTerminateAccountFailNoReasonProvided As String = "Save Terminate Account Fail due to absent of reason" '51
        Public Const CancelTerminateAccount As String = "Cancel Terminate Account" '52
        'Temp Account Action ------------------------------------------------------------------------------------------
        ' - Mark As Immd Valid
        Public Const MarkImmdValidClick As String = "Mark As Immd Valid Button Click" '53
        Public Const ConfirmIMMDvalidAccount As String = "Confirm Immd Valid" '54
        Public Const ConfirmIMMDvalidSuccess As String = "Confirm Immd Valid Success" '55
        Public Const ConfirmIMMDvalidFail As String = "Confirm Immd Valid Fail" '56
        Public Const CancelIMMDvalidAccount As String = "Cancel Immd Valid" '57
        ' - Confirm As Valid Acct
        Public Const MarkValidAcctClick As String = "Mark As Validated Account Button Click" '58
        Public Const ConfirmValidAccount As String = "Confirm Validated Account" '59
        Public Const ConfirmValidAccountSuccess As String = "Confirm Validated Account Success" '60
        Public Const ConfirmValidAccountFail As String = "Confirm Validated Account Fail" '61
        Public Const CancelValidAccount As String = "Cancel Validated Account" '62
        ' - Remove Temporary Account
        Public Const RemoveTempAcctClick As String = "Remove Temporary Account Click" '63
        Public Const ConfirmRemoveTempAcct As String = "Confirm Remove Temporary Account" '64
        Public Const ConfirmRemoveTempAcctSuccess As String = "Confirm Remove Temporary Account Success" '65
        Public Const ConfirmRemoveTempAcctFail As String = "Confirm Remove Temporary Account Fail" '66
        Public Const CancelRemoveTempAcct As String = "Cancel Remove Account" '67
        Public Const RemoveTempAcctClickFail As String = "Remove Temporary Account Click Fail" '130
        ' - Release for Rectification
        Public Const ReleaseRectifiClick As String = "Release for Rectification Click" '68
        Public Const ConfirmReleaseRectifi As String = "Confirm Release for Rectification" '69
        Public Const ConfirmReleaseRectifiSuccess As String = "Confirm Release for Rectification Success" '70
        Public Const ConfirmReleaseRectifiFail As String = "Confirm Release for Rectification Fail" '71
        Public Const CancelReleaseRectifi As String = "Cancel Release for Rectification" '72
        ' - Withdraw Amendment
        Public Const WithdtawAmendClick As String = "Withdraw Amendment Click" '73
        Public Const ConfirmWithdtawAmend As String = "Confirm Withdraw Amendment" '74
        Public Const ConfirmWithdtawAmendSuccess As String = "Confirm Withdraw Amendment Success" '75
        Public Const ConfirmWithdtawAmendFail As String = "Confirm Withdraw Amendment Fail" '76
        Public Const CancelWithdtawAmend As String = "Cancel Withdraw Amendment" '77


        '---------------------------------------------------------------------------------------------------------------
        Public Const BackToSearch As String = "Back To Search" '78
        Public Const BackToResultList As String = "Back To Search Result List" '79
        Public Const BackToAccDetailFromAmendmentHist As String = "Back to Account Detail From Amendment History" '80
        Public Const BackToAccDetailFromSchemeInfo As String = "Back to Account Detail From Scheme Info" '81
        '----- for both account amendment and account creation
        Public Const BackToAccDetailCancelAmend As String = "Back to Account Detail" '82
        Public Const BackToResultListFromComplete As String = "Back To Search Result List From Completion Page" '83
        'CCCode
        Public Const ClickSelectChineseNameButton As String = "Click Select Chinese Name Button"  '84
        Public Const ChineseNameCodeCheckingSuccess As String = "Chinese Name Code Checking Success" '85
        Public Const ChineseNameCodeCheckingFail As String = "Chinese Name Code Checking Fail" '86
        Public Const ConfirmChineseName As String = "Confirm the selection of Chinese Name" '87
        Public Const CancelChineseName As String = "Cancel the selection of Chinese Name" '88

        ' Cancel Validation for special account
        Public Const SpecialAccCancelValiationClick As String = "Special Acc Cancel Validation" '105
        Public Const ConfirmSpecialAccCancelValidation As String = "Confirm Special Acc Cancel Validation" '106
        Public Const ConfirmSpecialAccCancelValidationSuccess As String = "Confirm Special Acc Validation Success" '107
        Public Const ConfirmSpecialAccCancelValidationFail As String = "Confirm Special Acc Cancel Validation Fail" '108
        Public Const CancelSpecialAccCancelValidaton As String = "Cancel Special Acc Cancel Validation" '109

        'For EC control --> specific format / free format
        '94 Serial No. Not Provided checked: <Previous Serial No.><Checked after: [Y|N]>
        '95 Reference Other Formats clicked: <Previous Reference 1><Previous Reference 2><Previous Reference 3><Previous Reference 4>
        '96 Reference Specific Format clicked: <Previous Reference>

        'New Temp Account Created by Back Office     
        Public Const CreateNewAccountButtonClick As String = "New Account Button Click" '97
        Public Const CreateNewAccount_Reject As String = "New Account Creation - Reject" '98
        Public Const CreateNewAccount_EnterAccInputDetails As String = "New Account Creation - Proceed to Account Detail Input " '99

        Public Const CreateNewAccountSaveButtonClick As String = "New Account Creation - Save Button Click" '100
        Public Const SaveNewAccount As String = "New Account Creation - Save New Account" '101
        Public Const SaveNewAccountSuccess As String = "New Account Creation - Save New Account Success" '102
        Public Const SaveNewAccountFail As String = "New Account Creation - Save New Account Fail" '103
        Public Const CancelCreateNewAccount As String = "New Account Creation - Cancel New Account Creation" '104

        '(Not Applicable)
        Public Const NewAccCreation_SearchAccountClick As String = "New Account Creation - Search Account click"
        Public Const NewAccCreation_CreationDetailsClick As String = "New Account Creation - Creation Details Save button click"

        ' CRE11-007
        Public Const BatchSuspendAccountClick As String = "Suspend Selected Account Button Click" '110
        Public Const BatchSuspendAccountClick_ID As String = LogID.LOG00110
        Public Const BatchReactivateAccountClick As String = "Reactivate Selected Account Button Click" '111
        Public Const BatchReactivateAccountClick_ID As String = LogID.LOG00111
        Public Const BatchTerminateAccountClick As String = "Terminate Selected Account Button Click" '112
        Public Const BatchTerminateAccountClick_ID As String = LogID.LOG00112
        Public Const BatchAccountBackClick As String = "Batch Account Back Button Click" '113
        Public Const BatchAccountBackClick_ID As String = LogID.LOG00113
        Public Const BatchAccountSaveClick As String = "Batch Account Save Button Click" '114
        Public Const BatchAccountSaveClick_ID As String = LogID.LOG00114

        Public Const MaskIdentityDocumentNoClick As String = "Search Result - Mask Identity Document No. click"
        Public Const MaskIdentityDocumentNoClick_ID As String = LogID.LOG00115
        Public Const MaskIdentityDocumentNoSuccess As String = "Search Result - Unmask Identity Document No. success"
        Public Const MaskIdentityDocumentNoSuccess_ID As String = LogID.LOG00116

        Public Const BatchReactivateAccountFail As String = "Batch Suspend Account Fail"
        Public Const BatchReactivateAccountFail_ID As String = LogID.LOG00117
        Public Const BatchSuspendAccountFail As String = "Batch Suspend Account Fail"
        Public Const BatchSuspendAccountFail_ID As String = LogID.LOG00118
        Public Const BatchTerminateAccountFail As String = "Batch Terminate Account Fail"
        Public Const BatchTerminateAccountFail_ID As String = LogID.LOG00119
        Public Const BatchAccountActionFailNoSelection As String = "Batch Account Action Fail - No selection"
        Public Const BatchAccountActionFailNoSelection_ID As String = LogID.LOG00120

        Public Const MaskIdentityDocumentNoASClick As String = "Batch Account Save - Mask Identity Document No. click"
        Public Const MaskIdentityDocumentNoASClick_ID As String = LogID.LOG00121
        Public Const MaskIdentityDocumentNoACClick As String = "Batch Account Confirm - Mask Identity Document No. click"
        Public Const MaskIdentityDocumentNoACClick_ID As String = LogID.LOG00122

        Public Const BatchAccountActionSelection As String = "Batch Account Action - Selection"
        Public Const BatchAccountActionSelection_ID As String = LogID.LOG00123

        Public Const SearchByParticulars = "Search By Particulars"   '124
        Public Const SearchByParticularsSuccess = "Search By Particulars Success"   '125
        Public Const SearchByParticularsFail = "Search By Particulars Fail"   '126
        Public Const SearchbyManualValidation = "Search By Manual Validation"   '127
        Public Const SearchbyManualValidationSuccess = "Search By Manual Validation Success"  '128
        Public Const SearchbyManualValidationFail = "Search By Manual Validation Fail"  '129
    End Class

#End Region

#Region "Constant Value"
    Private Const intSearchView As Integer = 0
    Private Const intSearchResult As Integer = 1
    Private Const intAccountDetails As Integer = 2
    Private Const intAmendmentHistroy As Integer = 3
    Private Const intConfirm As Integer = 4
    Private Const intComplete As Integer = 5
    Private Const intNewAccount As Integer = 6
    Private Const intSave As Integer = 7

    Private Const SESS_Language As String = "language"
    Private Const SESS_Result As String = "eHSAccountMaint_SearchResult"
    Private Const SESS_InputMode As String = "eHSAccountMaint_InputMode"
    Private Const SESS_DefaultSetCCCode As String = "eHSAccountMaint_DefaultSetCCCode"
    Private Const SESS_ClickSave As String = "eHSAccountMaint_PressSave"
    Private Const SESS_AmendHistory As String = "eHSAccount_Maint_AmendHistory"
    Private Const SESS_ActionMode As String = "eHSAccount_Maint_ActionMode"
    Private Const SESS_PopupActionMode As String = "eHSAccount_Maint_PopupActionMode"
    Private Const SESS_AdvancedSearchSP As String = "eHSAccount_Maint_AdvancedSearchSP"
    Private Const SESS_ServiceProvider As String = "eHSAccount_Maint_ServiceProviderModel"
    Private Const SESS_AccountCreateBy As String = "eHSAccount_Maint_AccountCreateBy"
    Private Const strValidationFail As String = "ValidationFail"
    Private Const FuncCode As String = FunctCode.FUNT010301
    'Private Const FunctionCode As String = FunctCode.FUNT010301
    Private Const CommonFunctionCode As String = Common.Component.FunctCode.FUNT990000

    ' CRE11-007
    Public Const EHAccountIDSeparator As String = ","
#End Region

#Region "Private Class"

    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <remarks></remarks>
    Private Class VS
        Public Const UnmaskPopup As String = "010301_UnmaskPopup"
    End Class

    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <remarks></remarks>
    Private Class PopupStatus
        Public Const Active As String = "A"
        Public Const Closed As String = "C"
    End Class

    Private Class AccountTypeClass
        Public Const Validated As String = "V"
        Public Const Temporary As String = "T"
    End Class

    Private Class AccountStatusClass
        Public Const Active As String = "A"
        Public Const Suspended As String = "S"
        Public Const Terminiated As String = "D"
    End Class

    Private Class AccountActionTypeClass
        Public Const Suspend As String = "S"
        Public Const Terminate As String = "T"
        Public Const Reactivate As String = "R"
        Public Const SuspendEnquiry As String = "SE"
        Public Const ReactivateEnquiry As String = "RE"
    End Class

    Private Class MessageBoxHeaderKey
        Public Const ValidationFail As String = "ValidationFail"
    End Class

    Private Enum EnumBatchAccountAction
        Suspend
        Terminate
        Reactivate
        SuspendEnquiry
    End Enum
#End Region
#Region "Page Event"

    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    ' -------------------------------------------------------------------------
#Region "SF Search"

    Protected Overrides Sub SF_InitSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry)
    End Sub

    Protected Overrides Function SF_ValidateSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry) As Boolean
        Dim blnReturn As Boolean = True

        Select Case Me.tcSearchRoute.ActiveTabIndex

            Case 0
                '    'English Name
                '    If Me.txtSearchENameR1.Text.Trim.Equals(String.Empty) Then
                '        Me.lblAcctListENameR1.Text = Me.GetGlobalResourceObject("Text", "Any")
                '    Else
                '        Me.lblAcctListENameR1.Text = Me.txtSearchENameR1.Text.Trim
                '    End If

                '    'Account Type
                '    If Me.ddlSearchAcctTypeR1.SelectedValue.Trim = VRAcctMaintenanceStatus.Temporary OrElse _
                '           Me.ddlSearchAcctTypeR1.SelectedValue.Trim = VRAcctMaintenanceStatus.Special Then
                '        Me.lblAcctListAcctTypeR1.Text = Me.ddlSearchAcctTypeR1.SelectedItem.Text + "(" + Me.ddlSearchTempAcct.SelectedItem.Text + ")"
                '    Else
                '        Me.lblAcctListAcctTypeR1.Text = Me.ddlSearchAcctTypeR1.SelectedItem.Text
                '    End If

                '    'Creation Date
                '    Dim blnIsValid As Boolean = True

                '    If Me.txtSearchCreationDateFromR1.Text.Trim.Equals(String.Empty) Then
                '        Me.lblAcctListCreateDateFromR1.Text = Me.GetGlobalResourceObject("Text", "Any")
                '    Else
                '        'INT15-0015 (Fix date format checking in HCVU) [Start][Chris YIM]
                '        '-----------------------------------------------------------------------------------------
                '        'udtSM = udtvalidator.chkInputDate(Me.txtSearchCreationDateFromR1.Text, False)
                '        udtSM = udtvalidator.chkInputDate(Me.txtSearchCreationDateFromR1.Text, True, True)

                '        If Not udtSM Is Nothing Then
                '            Me.imgFromDateError.Visible = True
                '            'If udtSM.MessageCode = MsgCode.MSG00022 Then
                '            '    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00139)
                '            'Else
                '            '    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00137)
                '            'End If
                '            Select Case udtSM.MessageCode
                '                Case MsgCode.MSG00022
                '                    '"Creation Date From" should not be future date.
                '                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00139)
                '                Case MsgCode.MSG00028
                '                    'Please input the "Creation Date From".
                '                    Me.udcMsgBox.AddMessage(udtSM, "%s", Me.GetGlobalResourceObject("Text", "CreationDateFrom"))
                '                Case MsgCode.MSG00029
                '                    '"Creation Date From" is invalid.
                '                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00137)
                '                Case Else
                '                    'Please input the "Creation Date From".
                '                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00028, "%s", Me.GetGlobalResourceObject("Text", "CreationDateFrom"))
                '            End Select

                '            blnIsValid = False
                '        Else
                '            Me.lblAcctListCreateDateFromR1.Text = udtformatter.formatSearchDate(Me.txtSearchCreationDateFromR1.Text.Trim())
                '            Me.lblAcctListCreateDateFromR1.Text = udtformatter.convertDate(Me.lblAcctListCreateDateFromR1.Text.Trim(), "en")
                '        End If
                '        'INT15-0015 (Fix date format checking in HCVU) [End][Chris YIM]
                '    End If

                '    If Me.txtSearchCreationDateFromR1.Text.Trim.Equals(String.Empty) Then
                '        Me.lblAcctListCreateDateToR1.Text = Me.GetGlobalResourceObject("Text", "Any")
                '    Else
                '        'INT15-0015 (Fix date format checking in HCVU) [Start][Chris YIM]
                '        '-----------------------------------------------------------------------------------------
                '        'udtSM = udtvalidator.chkInputDate(Me.txtSearchCreationDateToR1.Text, False)
                '        udtSM = udtvalidator.chkInputDate(Me.txtSearchCreationDateToR1.Text, True, True)


                '        If Not udtSM Is Nothing Then
                '            Me.imgToDateError.Visible = True
                '            'If udtSM.MessageCode = MsgCode.MSG00022 Then
                '            '    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00140)
                '            'Else
                '            '    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00138)
                '            'End If
                '            Select Case udtSM.MessageCode
                '                Case MsgCode.MSG00022
                '                    '"Creation Date To" should not be future date.
                '                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00140)
                '                Case MsgCode.MSG00028
                '                    'Please input the "Creation Date To".
                '                    Me.udcMsgBox.AddMessage(udtSM, "%s", Me.GetGlobalResourceObject("Text", "CreationDateTo"))
                '                Case MsgCode.MSG00029
                '                    '"Creation Date To" is invalid.
                '                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00138)
                '                Case Else
                '                    'Please input the "Creation Date To".
                '                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00028, "%s", Me.GetGlobalResourceObject("Text", "CreationDateTo"))
                '            End Select

                '            blnIsValid = False
                '        Else
                '            Me.lblAcctListCreateDateToR1.Text = udtformatter.formatSearchDate(Me.txtSearchCreationDateToR1.Text.Trim())
                '            Me.lblAcctListCreateDateToR1.Text = udtformatter.convertDate(Me.lblAcctListCreateDateToR1.Text.Trim(), "en")
                '        End If
                '        'INT15-0015 (Fix date format checking in HCVU) [End][Chris YIM]
                '    End If

                '    If blnIsValid AndAlso Not Me.lblAcctListCreateDateFromR1.Text.Equals(Me.GetGlobalResourceObject("Text", "Any")) AndAlso _
                '        Not Me.lblAcctListCreateDateToR1.Text.Equals(Me.GetGlobalResourceObject("Text", "Any")) Then

                '        udtSM = udtvalidator.chkInputValidFromDateCutoffDate(FunctCode.FUNT990000, MsgCode.MSG00136, Me.lblAcctListCreateDateFromR1.Text, Me.lblAcctListCreateDateToR1.Text)
                '        If Not udtSM Is Nothing Then
                '            Me.imgFromDateError.Visible = True
                '            Me.imgToDateError.Visible = True
                '            blnIsValid = False
                '            Me.udcMsgBox.AddMessage(udtSM)
                '        End If
                '    End If

                '    Dim dt As DataTable = Nothing

                '    If udcMsgBox.GetCodeTable.Rows.Count = 0 Then
                '        blnReturn = True
                '    Else
                '        Me.udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00003, AuditLogDesc.SearchRoute1Fail)
                '        blnReturn = False
                '    End If


                'Case 1
                ' Search Route 2
                Dim strExactDOB As String = String.Empty
                Dim dtDOB As Nullable(Of DateTime) = Nothing
                Dim strDocCode As String = String.Empty
                Dim strAdoptionPrefixNum As String = String.Empty
                Dim strIdentityNum As String = String.Empty
                Dim streHSAccountID As String = String.Empty
                Dim arreHSAccountID() As String = Nothing
                Dim strRefNo As String = String.Empty
                Dim strAcctType As String = String.Empty
                Dim dtCreationDate As Nullable(Of DateTime) = Nothing

                'If Me.txtSearchENameR2.Text.Trim.Equals(String.Empty) AndAlso _
                '    Me.ddlSearchDocTypeR2.SelectedValue.Trim.Equals(String.Empty) AndAlso _
                '    Me.txtSearchIdentityNumR2.Text.Trim.Equals(String.Empty) AndAlso _
                '    Me.txtSearchDOBR2.Text.Trim.Equals(String.Empty) AndAlso _
                '    Me.txtSearchAccountIDR2.Text.Trim.Equals(String.Empty) AndAlso _
                '    Me.txtSearchRefNo.Text.Trim.Equals(String.Empty) Then

                '    udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001)
                '    Me.udcMsgBox.AddMessage(udtSM)
                '    Me.udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00006, AuditLogDesc.SearchRoute2Fail)
                '    'CRE13-006 HCVS Ceiling [Start][Karl]
                '    blnReturn = False
                '    'CRE13-006 HCVS Ceiling [End][Karl]

                'Else
                If Not Me.txtSearchAccountIDR2.Text.Trim() = String.Empty AndAlso Not IsValidEHSAccountNumber(Me.txtSearchAccountIDR2.Text.Trim()) Then
                    ' Invalid EHS Account ID

                    ' Replace the system message to common function_code
                    'udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00007)
                    udtSM = New SystemMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00362)
                    Me.udcMsgBox.AddMessage(udtSM)
                    'Me.udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00006, AuditLogDesc.SearchRoute2Fail)
                    Me.udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00126, AuditLogDesc.SearchByParticularsFail)
                    'CRE13-006 HCVS Ceiling [Start][Karl]
                    blnReturn = False
                    'CRE13-006 HCVS Ceiling [End][Karl]
                    Me.imgSearchAccountIDR2Error.Visible = True
                Else
                    'Doc Type
                    Me.lblAcctListDocTypeR2.Text = Me.ddlSearchDocTypeR2.SelectedItem.Text.Trim
                    strDocCode = Me.ddlSearchDocTypeR2.SelectedValue.Trim

                    'Identity Num
                    If Me.txtSearchIdentityNumR2.Text.Trim.Equals(String.Empty) Then
                        Me.lblAcctListIdentityNumR2.Text = Me.GetGlobalResourceObject("Text", "Any")
                    Else
                        Me.lblAcctListIdentityNumR2.Text = Me.txtSearchIdentityNumR2.Text.Trim.ToUpper

                        Dim strIdentityNumFullTemp As String
                        strIdentityNumFullTemp = Me.txtSearchIdentityNumR2.Text.Trim.ToUpper.Replace("-", "").Replace("(", "").Replace(")", "")

                        Dim strIdentityNumFull() As String
                        strIdentityNumFull = strIdentityNumFullTemp.Trim.Split("/")
                        If strIdentityNumFull.Length > 1 Then
                            strIdentityNum = strIdentityNumFull(1)
                            strAdoptionPrefixNum = strIdentityNumFull(0)
                        Else
                            strIdentityNum = strIdentityNumFullTemp
                        End If
                    End If

                    'English Name
                    If Me.txtSearchENameR2.Text.Equals(String.Empty) Then
                        Me.lblAcctListENameR2.Text = Me.GetGlobalResourceObject("Text", "Any")
                    Else
                        Me.lblAcctListENameR2.Text = Me.txtSearchENameR2.Text.Trim
                    End If

                    ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
                    'Chinese Name
                    If Me.txtSearchCNameR2.Text.Equals(String.Empty) Then
                        Me.lblAcctListCNameR2.Text = Me.GetGlobalResourceObject("Text", "Any")
                    Else
                        Me.lblAcctListCNameR2.Text = Me.txtSearchCNameR2.Text.Trim
                    End If
                    ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]

                    'DOB
                    If Me.txtSearchDOBR2.Text.Trim.Equals(String.Empty) Then
                        Me.lblAcctListDOBR2.Text = Me.GetGlobalResourceObject("Text", "Any")
                    Else
                        Dim dtDOBValue As DateTime
                        'DOB passed to "chkDOB" must be of DateTime instead of Nullable(of DateTime) 
                        udtSM = Me.udtValidator.chkDOB(strDocCode, Me.txtSearchDOBR2.Text.Trim, dtDOBValue, strExactDOB)
                        If Not IsNothing(udtSM) Then
                            Me.imgDOBError.Visible = True
                            Me.udcMsgBox.AddMessage(udtSM)
                        Else
                            dtDOB = dtDOBValue
                            ' CRE12-015 Add the respective practice number in ¡§Practice¡¨ in the functions under ¡§Reimbursement¡¨ in eHS [Start][Koala]
                            ' -----------------------------------------------------------------------------------------------------------------------------
                            Me.lblAcctListDOBR2.Text = udtformatter.formatDOB(dtDOB, strExactDOB, Session(SESS_Language), Nothing, Nothing)
                            ' CRE12-015 Add the respective practice number in ¡§Practice¡¨ in the functions under ¡§Reimbursement¡¨ in eHS [End][Koala]
                        End If
                    End If

                    'eHealth Account ID (Vaildated Account ID)
                    Me.CollapsiblePanelExtenderAccountIDR2.Enabled = False
                    Me.CollapsiblePanelExtenderAccountIDR2.Collapsed = True
                    Me.ibtnAcctListAccountIDR2Multiple.Visible = False
                    Me.txtAcctListAccountIDR2Multiple.Text = String.Empty

                    If Me.txtSearchAccountIDR2.Text.Trim.Equals(String.Empty) Then
                        Me.lblAcctListAccountIDR2.Text = Me.GetGlobalResourceObject("Text", "Any")
                    Else

                        'If udtvalidator.chkSystemNumber(Me.txtSearchAccountIDR2.Text.Trim) Then
                        '    streHSAccountID = Common.Format.Formatter.ReverseSystemNumber(Me.txtSearchAccountIDR2.Text.Trim)
                        'Else
                        '    streHSAccountID = Me.txtSearchAccountIDR2.Text.Trim
                        'End If

                        ' Remove Check Digit Before Search
                        ' ToDo: Check is valid check digit
                        streHSAccountID = Me.txtSearchAccountIDR2.Text.Trim
                        If Not String.IsNullOrEmpty(streHSAccountID) Then
                            arreHSAccountID = streHSAccountID.Split(New Char() {EHAccountIDSeparator}, StringSplitOptions.RemoveEmptyEntries)
                            For i As Integer = 0 To arreHSAccountID.Length - 1
                                arreHSAccountID(i) = arreHSAccountID(i).Substring(0, arreHSAccountID(i).Length - 1)
                            Next
                        End If

                        If arreHSAccountID.Length = 1 Then
                            Me.lblAcctListAccountIDR2.Text = GetMultipleAccountIDList(arreHSAccountID, 0, 0)
                        ElseIf arreHSAccountID.Length = 2 Then
                            Me.lblAcctListAccountIDR2.Text = GetMultipleAccountIDList(arreHSAccountID, 0, 1)
                        ElseIf arreHSAccountID.Length > 2 Then
                            Me.lblAcctListAccountIDR2.Text = GetMultipleAccountIDList(arreHSAccountID, 0, 1)
                            Me.CollapsiblePanelExtenderAccountIDR2.Enabled = True
                            Me.ibtnAcctListAccountIDR2Multiple.Visible = True
                            Me.txtAcctListAccountIDR2Multiple.Text = GetMultipleAccountIDList(arreHSAccountID, 2, arreHSAccountID.Length - 1)
                        End If

                    End If

                    'Ref No. (Temporary Account ID, Special Account ID, Ivalid Account ID)
                    If Me.txtSearchRefNo.Text.Trim.Equals(String.Empty) Then
                        Me.lblAcctListRefNoR2.Text = Me.GetGlobalResourceObject("Text", "Any")
                    Else
                        Me.lblAcctListRefNoR2.Text = Me.txtSearchRefNo.Text.Trim
                        If udtValidator.chkSystemNumber(Me.txtSearchRefNo.Text.Trim) Then
                            strRefNo = Common.Format.Formatter.ReverseSystemNumber(Me.txtSearchRefNo.Text.Trim)
                        Else
                            strRefNo = Me.txtSearchRefNo.Text.Trim
                        End If
                    End If

                    'Account Type
                    Me.lblAcctListAcctTypeR2.Text = Me.ddlSearchAcctTypeR2.SelectedItem.Text

                    If Me.ddlSearchAcctTypeR2.SelectedValue.Trim = VRAcctMaintenanceStatus.Temporary Then
                        If Not String.IsNullOrEmpty(ddlSearchTempAcct.SelectedValue) Then
                            Me.lblAcctListAcctTypeR2.Text = String.Format("{0} ({1})", Me.ddlSearchAcctTypeR2.SelectedItem.Text, Me.ddlSearchTempAcct.SelectedItem.Text)
                        End If
                    End If

                    'Creation Date
                    Dim blnIsValid As Boolean = True
                    Dim strCreationDateFrom As String = String.Empty
                    Dim strCreationDateTo As String = String.Empty

                    If Me.txtSearchCreationDateFromR2.Text.Trim.Equals(String.Empty) And _
                        Me.txtSearchCreationDateToR2.Text.Trim.Equals(String.Empty) Then
                        Me.lblAcctListCreateDateR2.Text = Me.GetGlobalResourceObject("Text", "Any")
                    Else
                        'Creation Date From
                        udtSM = udtValidator.chkInputDate(Me.txtSearchCreationDateFromR2.Text, True, True)

                        If Not udtSM Is Nothing Then
                            Me.imgDateError.Visible = True
                            Select Case udtSM.MessageCode
                                Case MsgCode.MSG00022
                                    '"Creation Date From" should not be future date.
                                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00139)
                                Case MsgCode.MSG00028
                                    'Please input the "Creation Date From".
                                    Me.udcMsgBox.AddMessage(udtSM, "%s", Me.GetGlobalResourceObject("Text", "CreationDateFrom"))
                                Case MsgCode.MSG00029
                                    '"Creation Date From" is invalid.
                                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00137)
                                Case Else
                                    'Please input the "Creation Date From".
                                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00028, "%s", Me.GetGlobalResourceObject("Text", "CreationDateFrom"))
                            End Select

                            blnIsValid = False
                        Else
                            strCreationDateFrom = udtformatter.formatSearchDate(Me.txtSearchCreationDateFromR2.Text.Trim())
                            strCreationDateFrom = udtformatter.convertDate(strCreationDateFrom.Trim(), "en")
                        End If

                        'Creation Date To
                        udtSM = udtValidator.chkInputDate(Me.txtSearchCreationDateToR2.Text, True, True)

                        If Not udtSM Is Nothing Then
                            Me.imgDateError.Visible = True
                            Select Case udtSM.MessageCode
                                Case MsgCode.MSG00022
                                    '"Creation Date To" should not be future date.
                                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00140)
                                Case MsgCode.MSG00028
                                    'Please input the "Creation Date To".
                                    Me.udcMsgBox.AddMessage(udtSM, "%s", Me.GetGlobalResourceObject("Text", "CreationDateTo"))
                                Case MsgCode.MSG00029
                                    '"Creation Date To" is invalid.
                                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00138)
                                Case Else
                                    'Please input the "Creation Date To".
                                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00028, "%s", Me.GetGlobalResourceObject("Text", "CreationDateTo"))
                            End Select

                            blnIsValid = False
                        Else
                            strCreationDateTo = udtformatter.formatSearchDate(Me.txtSearchCreationDateToR2.Text.Trim())
                            strCreationDateTo = udtformatter.convertDate(strCreationDateTo.Trim(), "en")
                        End If
                    End If

                    If blnIsValid AndAlso Not String.IsNullOrEmpty(strCreationDateFrom) AndAlso Not String.IsNullOrEmpty(strCreationDateTo) Then
                        udtSM = udtValidator.chkInputValidFromDateCutoffDate(FunctCode.FUNT990000, MsgCode.MSG00136, strCreationDateFrom, strCreationDateTo)
                        If Not udtSM Is Nothing Then
                            Me.imgDateError.Visible = True
                            blnIsValid = False
                            Me.udcMsgBox.AddMessage(udtSM)
                        Else
                            lblAcctListCreateDateR2.Text = String.Format("{0} {1} {2}", strCreationDateFrom, Me.GetGlobalResourceObject("Text", "To_S"), strCreationDateTo)
                        End If
                    End If

                    If Me.udcMsgBox.GetCodeTable.Rows.Count = 0 Then
                        blnReturn = True
                    Else
                        'Me.udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00006, AuditLogDesc.SearchRoute2Fail)
                        Me.udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00126, AuditLogDesc.SearchByParticularsFail)
                        blnReturn = False
                    End If
                End If

            Case 1
                'Search Route 3
                Dim strSPID As String = String.Empty
                Dim strManualValidationStatus As String = String.Empty
                Dim dtCreationDate As Nullable(Of DateTime) = Nothing
                Dim strWithClaims As String = String.Empty
                Dim strDeceased As String = String.Empty
                Dim dtDateofDeath As Nullable(Of DateTime) = Nothing
                Dim strAcctType As String = String.Empty

                'Service Provider ID
                If Me.txtSPIDR3.Text.Trim.Equals(String.Empty) Then
                    Me.lblAcctListSPIDR3.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    Me.lblAcctListSPIDR3.Text = Me.lblAcctListSPIDR3.Text.Trim
                End If

                'Manual Validation Status
                Me.lblAcctListManualValidStatusR3.Text = Me.ddlManualValidStatusR3.SelectedItem.Text.Trim

                'Creation Date
                Dim blnCDIsValid As Boolean = True
                Dim strCreationDateFrom As String = String.Empty
                Dim strCreationDateTo As String = String.Empty

                If Me.txtCreationDateFromR3.Text.Trim.Equals(String.Empty) And _
                    Me.txtCreationDateToR3.Text.Trim.Equals(String.Empty) Then
                    Me.lblAcctListCreationDateR3.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    'Creation Date From
                    udtSM = udtValidator.chkInputDate(Me.txtCreationDateFromR3.Text, True, True)

                    If Not udtSM Is Nothing Then
                        Me.imgCreationDateErrorR3.Visible = True
                        Select Case udtSM.MessageCode
                            Case MsgCode.MSG00022
                                '"Creation Date From" should not be future date.
                                Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00139)
                            Case MsgCode.MSG00028
                                'Please input the "Creation Date From".
                                Me.udcMsgBox.AddMessage(udtSM, "%s", Me.GetGlobalResourceObject("Text", "CreationDateFrom"))
                            Case MsgCode.MSG00029
                                '"Creation Date From" is invalid.
                                Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00137)
                            Case Else
                                'Please input the "Creation Date From".
                                Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00028, "%s", Me.GetGlobalResourceObject("Text", "CreationDateFrom"))
                        End Select

                        blnCDIsValid = False
                    Else
                        strCreationDateFrom = udtformatter.formatSearchDate(Me.txtCreationDateFromR3.Text.Trim())
                        strCreationDateFrom = udtformatter.convertDate(strCreationDateFrom.Trim(), "en")
                    End If

                    'Creation Date To
                    udtSM = udtValidator.chkInputDate(Me.txtCreationDateToR3.Text, True, True)

                    If Not udtSM Is Nothing Then
                        Me.imgCreationDateErrorR3.Visible = True
                        Select Case udtSM.MessageCode
                            Case MsgCode.MSG00022
                                '"Creation Date To" should not be future date.
                                Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00140)
                            Case MsgCode.MSG00028
                                'Please input the "Creation Date To".
                                Me.udcMsgBox.AddMessage(udtSM, "%s", Me.GetGlobalResourceObject("Text", "CreationDateTo"))
                            Case MsgCode.MSG00029
                                '"Creation Date To" is invalid.
                                Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00138)
                            Case Else
                                'Please input the "Creation Date To".
                                Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00028, "%s", Me.GetGlobalResourceObject("Text", "CreationDateTo"))
                        End Select

                        blnCDIsValid = False
                    Else
                        strCreationDateTo = udtformatter.formatSearchDate(Me.txtCreationDateToR3.Text.Trim())
                        strCreationDateTo = udtformatter.convertDate(strCreationDateTo.Trim(), "en")
                    End If
                End If

                If blnCDIsValid AndAlso Not String.IsNullOrEmpty(strCreationDateFrom) AndAlso Not String.IsNullOrEmpty(strCreationDateTo) Then
                    udtSM = udtValidator.chkInputValidFromDateCutoffDate(FunctCode.FUNT990000, MsgCode.MSG00136, strCreationDateFrom, strCreationDateTo)
                    If Not udtSM Is Nothing Then
                        Me.imgCreationDateErrorR3.Visible = True
                        blnCDIsValid = False
                        Me.udcMsgBox.AddMessage(udtSM)
                    Else
                        lblAcctListCreationDateR3.Text = String.Format("{0} {1} {2}", strCreationDateFrom, Me.GetGlobalResourceObject("Text", "To_S"), strCreationDateTo)
                    End If
                End If

                'With Claims
                If Me.ddlWithClaimsR3.SelectedValue.Trim.Equals(String.Empty) Then
                    'Any
                    lblAcctListWithClaimsR3.Text = ddlWithClaimsR3.SelectedItem.Text.Trim
                ElseIf Me.ddlWithClaimsR3.SelectedValue.Trim.Equals(YesNo.Yes) Then
                    'Yes
                    If String.IsNullOrEmpty(ddlSchemeR3.SelectedValue) Then
                        lblAcctListWithClaimsR3.Text = ddlWithClaimsR3.SelectedItem.Text.Trim
                    Else
                        lblAcctListWithClaimsR3.Text = String.Format("{0} ({1})", ddlWithClaimsR3.SelectedItem.Text.Trim, ddlSchemeR3.SelectedValue)
                    End If
                ElseIf Me.ddlWithClaimsR3.SelectedValue.Trim.Equals(YesNo.No) Then
                    'No
                    lblAcctListWithClaimsR3.Text = Me.GetGlobalResourceObject("Text", "No")
                End If

                'Deceased
                If Me.ddlDeceasedR3.SelectedValue.Trim.Equals(String.Empty) Then
                    lblAcctListDeceasedR3.Text = Me.GetGlobalResourceObject("Text", "Any")
                    'lblAcctListDateofDeathR3.Text = Me.GetGlobalResourceObject("Text", "Any")
                ElseIf Me.ddlDeceasedR3.SelectedValue.Trim.Equals(YesNo.Yes) Then
                    lblAcctListDeceasedR3.Text = Me.GetGlobalResourceObject("Text", "Yes")

                    'Date of Death
                    Dim blnDateofDeathIsValid As Boolean = True
                    Dim strDateofDeathFrom As String = String.Empty
                    Dim strDateofDeathTo As String = String.Empty

                    If Me.txtDateofDeathFromR3.Text.Trim.Equals(String.Empty) And Me.txtDateofDeathToR3.Text.Trim.Equals(String.Empty) Then
                        Me.lblAcctListDateofDeathR3.Text = Me.GetGlobalResourceObject("Text", "Any")
                    Else
                        'Date of Death From
                        udtSM = udtValidator.chkInputDate(Me.txtDateofDeathFromR3.Text, True, True)

                        If Not udtSM Is Nothing Then
                            Me.imgDateofDeathErrorR3.Visible = True
                            Select Case udtSM.MessageCode
                                Case MsgCode.MSG00022
                                    '"Date of Death From" should not be future date.
                                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00405)
                                Case MsgCode.MSG00028
                                    'Please input the "Date of Death From.
                                    Me.udcMsgBox.AddMessage(udtSM, "%s", Me.GetGlobalResourceObject("Text", "DateofDeathFrom"))
                                Case MsgCode.MSG00029
                                    '"Date of Death From" is invalid.
                                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00403)
                                Case Else
                                    'Please input the "Date of Death From".
                                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00028, "%s", Me.GetGlobalResourceObject("Text", "DateofDeathFrom"))
                            End Select

                            blnDateofDeathIsValid = False
                        Else
                            strDateofDeathFrom = udtformatter.formatSearchDate(Me.txtDateofDeathFromR3.Text.Trim())
                            strDateofDeathFrom = udtformatter.convertDate(strDateofDeathFrom.Trim(), "en")
                        End If

                        'Date of Death To
                        udtSM = udtValidator.chkInputDate(Me.txtDateofDeathToR3.Text, True, True)

                        If Not udtSM Is Nothing Then
                            Me.imgDateofDeathErrorR3.Visible = True
                            Select Case udtSM.MessageCode
                                Case MsgCode.MSG00022
                                    '"Date of Death To" should not be future date.
                                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00406)
                                Case MsgCode.MSG00028
                                    'Please input the "Date of Death To".
                                    Me.udcMsgBox.AddMessage(udtSM, "%s", Me.GetGlobalResourceObject("Text", "DateofDeathTo"))
                                Case MsgCode.MSG00029
                                    '"Date of Death To" is invalid.
                                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00404)
                                Case Else
                                    'Please input the "Date of Death To".
                                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00028, "%s", Me.GetGlobalResourceObject("Text", "DateofDeathTo"))
                            End Select

                            blnDateofDeathIsValid = False
                        Else
                            strDateofDeathTo = udtformatter.formatSearchDate(Me.txtDateofDeathToR3.Text.Trim())
                            strDateofDeathTo = udtformatter.convertDate(strDateofDeathTo.Trim(), "en")
                        End If
                    End If

                    If blnDateofDeathIsValid AndAlso Not String.IsNullOrEmpty(strDateofDeathFrom) AndAlso Not String.IsNullOrEmpty(strDateofDeathTo) Then
                        udtSM = udtValidator.chkInputValidFromDateCutoffDate(FunctCode.FUNT990000, MsgCode.MSG00402, strDateofDeathFrom, strDateofDeathTo)
                        If Not udtSM Is Nothing Then
                            Me.imgDateofDeathErrorR3.Visible = True
                            blnDateofDeathIsValid = False
                            Me.udcMsgBox.AddMessage(udtSM)
                        Else
                            lblAcctListDateofDeathR3.Text = String.Format("{0} {1} {2}", strDateofDeathFrom, Me.GetGlobalResourceObject("Text", "To_S"), strDateofDeathTo)
                        End If
                    End If
                ElseIf Me.ddlDeceasedR3.SelectedValue.Trim.Equals(YesNo.No) Then
                    lblAcctListDeceasedR3.Text = Me.GetGlobalResourceObject("Text", "No")
                    lblAcctListDateofDeathR3.Text = Me.GetGlobalResourceObject("Text", "Any")
                End If

                'Account Type
                Me.lblAcctListAcctTypeR3.Text = Me.ddlAcctTypeR3.SelectedItem.Text

                If Me.udcMsgBox.GetCodeTable.Rows.Count = 0 Then
                    blnReturn = True
                Else
                    Me.udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00129, AuditLogDesc.SearchbyManualValidationFail)
                    blnReturn = False
                End If

        End Select

        Return blnReturn
    End Function

    Protected Overrides Function SF_Search(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry, ByVal blnOverrideResultLimit As Boolean) As Common.Component.BaseBLL.BLLSearchResult
        Dim bllSearchResult As Common.Component.BaseBLL.BLLSearchResult

        bllSearchResult = Nothing

        'If blnOverrideResultLimit Then
        Select Case Me.tcSearchRoute.ActiveTabIndex
            Case 0
                '    Dim strAccountType As String = String.Empty

                '    strAccountType = Me.ddlSearchAcctTypeR1.SelectedValue.Trim

                '    If strAccountType = VRAcctMaintenanceStatus.Temporary Then
                '        strAccountType = EHSAccountModel.SysAccountSourceClass.TemporaryAccount

                '    ElseIf strAccountType = VRAcctMaintenanceStatus.Special Then
                '        strAccountType = EHSAccountModel.SysAccountSourceClass.SpecialAccount
                '    End If

                '    If Me.ddlSearchAcctTypeR1.SelectedValue.Trim = VRAcctMaintenanceStatus.Temporary OrElse _
                '        Me.ddlSearchAcctTypeR1.SelectedValue.Trim = VRAcctMaintenanceStatus.Special Then

                '        Select Case Me.ddlSearchTempAcct.SelectedValue.Trim
                '            Case TempAcctMaintenanceStatus.PendingRemove
                '                bllSearchResult = udteHSAccountMaintBLL.GeteHSAcctOustandingListFor29Days(Me.FunctionCode, _
                '                                                                            Me.txtSearchENameR1.Text.Trim, _
                '                                                                            Me.txtSearchCreationDateFromR1.Text.Trim, _
                '                                                                            Me.txtSearchCreationDateToR1.Text.Trim, True)

                '            Case TempAcctMaintenanceStatus.OutstandingValidation
                '                bllSearchResult = udteHSAccountMaintBLL.GeteHSAcctOutstandingValidationList(Me.FunctionCode, _
                '                                                                                Me.txtSearchENameR1.Text.Trim, _
                '                                                                                Me.txtSearchCreationDateFromR1.Text.Trim, _
                '                                                                                Me.txtSearchCreationDateToR1.Text.Trim, strAccountType, True)


                '            Case TempAcctMaintenanceStatus.PendingImmdValidation
                '                bllSearchResult = udteHSAccountMaintBLL.GeteHSAcctPendingImmdValidationList(Me.FunctionCode, _
                '                                                                                Me.txtSearchENameR1.Text.Trim, _
                '                                                                                Me.txtSearchCreationDateFromR1.Text.Trim, _
                '                                                                                Me.txtSearchCreationDateToR1.Text.Trim, strAccountType, True)

                '            Case Else
                '                bllSearchResult = udteHSAccountMaintBLL.GeteHSAcctListInRouteOne(Me.FunctionCode, _
                '                                                                                               Me.txtSearchENameR1.Text.Trim, _
                '                                                                                               Me.txtSearchCreationDateFromR1.Text.Trim, _
                '                                                                                               Me.txtSearchCreationDateToR1.Text.Trim, _
                '                                                                                               Me.ddlSearchAcctTypeR1.SelectedValue, True)

                '        End Select
                '    Else
                '        bllSearchResult = udteHSAccountMaintBLL.GeteHSAcctListInRouteOne(Me.FunctionCode, _
                '                                                           Me.txtSearchENameR1.Text.Trim, _
                '                                                           Me.txtSearchCreationDateFromR1.Text.Trim, _
                '                                                           Me.txtSearchCreationDateToR1.Text.Trim, _
                '                                                           Me.ddlSearchAcctTypeR1.SelectedValue, True)
                '    End If

                'Case 1
                Dim strExactDOB As String = String.Empty
                Dim dtDOB As Nullable(Of DateTime) = Nothing
                Dim strDocCode As String = String.Empty
                Dim strAdoptionPrefixNum As String = String.Empty
                Dim strIdentityNum As String = String.Empty
                Dim streHSAccountID As String = String.Empty
                Dim arreHSAccountID() As String = Nothing
                Dim strRefNo As String = String.Empty
                Dim strAccountType As String = String.Empty
                Dim strAccountStatus As String = String.Empty
                Dim dtmCreationDateFrom As Nullable(Of DateTime) = Nothing
                Dim dtmCreationDateTo As Nullable(Of DateTime) = Nothing
                Dim strGender As String = String.Empty

                'Doc Type
                Me.lblAcctListDocTypeR2.Text = Me.ddlSearchDocTypeR2.SelectedItem.Text.Trim
                strDocCode = Me.ddlSearchDocTypeR2.SelectedValue.Trim

                'Identity Num
                If Me.txtSearchIdentityNumR2.Text.Trim.Equals(String.Empty) Then
                    Me.lblAcctListIdentityNumR2.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    Me.lblAcctListIdentityNumR2.Text = Me.txtSearchIdentityNumR2.Text.Trim.ToUpper

                    'only OW and PASS are free text
                    If strDocCode = DocTypeModel.DocTypeCode.PASS Or strDocCode = DocTypeModel.DocTypeCode.OW Then
                        strIdentityNum = Me.txtSearchIdentityNumR2.Text.Trim.ToUpper
                    Else
                        Dim strIdentityNumFullTemp As String
                        strIdentityNumFullTemp = Me.txtSearchIdentityNumR2.Text.Trim.ToUpper.Replace("-", "").Replace("(", "").Replace(")", "")

                        Dim strIdentityNumFull() As String
                        strIdentityNumFull = strIdentityNumFullTemp.Trim.Split("/")
                        If strIdentityNumFull.Length > 1 Then
                            strIdentityNum = strIdentityNumFull(1)
                            strAdoptionPrefixNum = strIdentityNumFull(0)
                        Else
                            strIdentityNum = strIdentityNumFullTemp
                        End If
                    End If

                End If

                'English Name
                If Me.txtSearchENameR2.Text.Equals(String.Empty) Then
                    Me.lblAcctListENameR2.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    Me.lblAcctListENameR2.Text = Me.txtSearchENameR2.Text.Trim
                End If

                ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
                'Chinese Name
                If Me.txtSearchCNameR2.Text.Equals(String.Empty) Then
                    Me.lblAcctListCNameR2.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    Me.lblAcctListCNameR2.Text = Me.txtSearchCNameR2.Text.Trim
                End If
                ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]

                'DOB
                If Me.txtSearchDOBR2.Text.Trim.Equals(String.Empty) Then
                    Me.lblAcctListDOBR2.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    Dim dtDOBValue As DateTime
                    'DOB passed to "chkDOB" must be of DateTime instead of Nullable(of DateTime) 
                    udtSM = Me.udtValidator.chkDOB(strDocCode, Me.txtSearchDOBR2.Text.Trim, dtDOBValue, strExactDOB)
                    If Not IsNothing(udtSM) Then
                        Me.imgDOBError.Visible = True
                        Me.udcMsgBox.AddMessage(udtSM)
                    Else
                        dtDOB = dtDOBValue
                        ' CRE12-015 Add the respective practice number in ¡§Practice¡¨ in the functions under ¡§Reimbursement¡¨ in eHS [Start][Koala]
                        ' -----------------------------------------------------------------------------------------------------------------------------
                        Me.lblAcctListDOBR2.Text = udtformatter.formatDOB(dtDOB, strExactDOB, Session(SESS_Language), Nothing, Nothing)
                        ' CRE12-015 Add the respective practice number in ¡§Practice¡¨ in the functions under ¡§Reimbursement¡¨ in eHS [End][Koala]
                    End If
                End If

                'eHealth Account ID (Vaildated Account ID)
                Me.CollapsiblePanelExtenderAccountIDR2.Enabled = False
                Me.CollapsiblePanelExtenderAccountIDR2.Collapsed = True
                Me.ibtnAcctListAccountIDR2Multiple.Visible = False
                Me.txtAcctListAccountIDR2Multiple.Text = String.Empty

                If Me.txtSearchAccountIDR2.Text.Trim.Equals(String.Empty) Then
                    Me.lblAcctListAccountIDR2.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    ' Remove Check Digit Before Search
                    ' ToDo: Check is valid check digit
                    streHSAccountID = Me.txtSearchAccountIDR2.Text.Trim
                    If Not String.IsNullOrEmpty(streHSAccountID) Then
                        arreHSAccountID = streHSAccountID.Split(New Char() {EHAccountIDSeparator}, StringSplitOptions.RemoveEmptyEntries)
                        For i As Integer = 0 To arreHSAccountID.Length - 1
                            arreHSAccountID(i) = arreHSAccountID(i).Substring(0, arreHSAccountID(i).Length - 1)
                        Next
                    End If

                    If arreHSAccountID.Length = 1 Then
                        Me.lblAcctListAccountIDR2.Text = GetMultipleAccountIDList(arreHSAccountID, 0, 0)
                    ElseIf arreHSAccountID.Length = 2 Then
                        Me.lblAcctListAccountIDR2.Text = GetMultipleAccountIDList(arreHSAccountID, 0, 1)
                    ElseIf arreHSAccountID.Length > 2 Then
                        Me.lblAcctListAccountIDR2.Text = GetMultipleAccountIDList(arreHSAccountID, 0, 1)
                        Me.CollapsiblePanelExtenderAccountIDR2.Enabled = True
                        Me.ibtnAcctListAccountIDR2Multiple.Visible = True
                        Me.txtAcctListAccountIDR2Multiple.Text = GetMultipleAccountIDList(arreHSAccountID, 2, arreHSAccountID.Length - 1)
                    End If

                End If

                'Ref No. (Temporary Account ID, Special Account ID, Ivalid Account ID)
                If Me.txtSearchRefNo.Text.Trim.Equals(String.Empty) Then
                    Me.lblAcctListRefNoR2.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    Me.lblAcctListRefNoR2.Text = Me.txtSearchRefNo.Text.Trim
                    If udtValidator.chkSystemNumber(Me.txtSearchRefNo.Text.Trim) Then
                        strRefNo = Common.Format.Formatter.ReverseSystemNumber(Me.txtSearchRefNo.Text.Trim)
                    Else
                        strRefNo = Me.txtSearchRefNo.Text.Trim
                    End If
                End If

                'Account Type (+ Account Status)
                strAccountType = Me.ddlSearchAcctTypeR2.SelectedValue.Trim
                Me.lblAcctListAcctTypeR2.Text = Me.ddlSearchAcctTypeR2.SelectedItem.Text

                If Me.ddlSearchAcctTypeR2.SelectedValue.Trim = VRAcctMaintenanceStatus.Temporary Then
                    If Not String.IsNullOrEmpty(ddlSearchTempAcct.SelectedValue) Then
                        strAccountStatus = Me.ddlSearchTempAcct.SelectedValue.Trim
                        Me.lblAcctListAcctTypeR2.Text = String.Format("{0} ({1})", Me.ddlSearchAcctTypeR2.SelectedItem.Text, Me.ddlSearchTempAcct.SelectedItem.Text)
                    End If
                End If

                'Creation Date
                If Me.txtSearchCreationDateFromR2.Text.Trim.Equals(String.Empty) AndAlso _
                    Me.txtSearchCreationDateToR2.Text.Trim.Equals(String.Empty) Then
                    lblAcctListCreateDateR2.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    If IsDate(udtformatter.convertDate(Me.txtSearchCreationDateFromR2.Text.Trim, "E")) Then
                        dtmCreationDateFrom = udtformatter.convertDate(Me.txtSearchCreationDateFromR2.Text.Trim, "E")
                    End If

                    If IsDate(udtformatter.convertDate(Me.txtSearchCreationDateToR2.Text.Trim, "E")) Then
                        dtmCreationDateTo = udtformatter.convertDate(Me.txtSearchCreationDateToR2.Text.Trim, "E")
                    End If
                    lblAcctListCreateDateR2.Text = String.Format("{0} {1} {2}", _
                                                    udtformatter.convertDate(Me.txtSearchCreationDateFromR2.Text.Trim, String.Empty), _
                                                    Me.GetGlobalResourceObject("Text", "To_S"), _
                                                    udtformatter.convertDate(Me.txtSearchCreationDateToR2.Text.Trim, String.Empty))
                End If

                ' CRE19-026 (HCVS hotline service) [Start][Winnie]
                bllSearchResult = udteHSAccountMaintBLL.GeteHSAcctListByParticularMultiple(Me.FunctionCode, strDocCode, strIdentityNum, strAdoptionPrefixNum, Me.txtSearchENameR2.Text.Trim, Me.txtSearchCNameR2.Text.Trim, dtDOB, _
                                                                arreHSAccountID, strRefNo, strGender, _
                                                                strAccountType, strAccountStatus, dtmCreationDateFrom, dtmCreationDateTo, _
                                                                blnOverrideResultLimit)
                ' CRE19-026 (HCVS hotline service) [End][Winnie]

            Case 1  'Manual Validaion Route
                Dim strServiceProviderID As String = String.Empty
                Dim strManualValidaitonStatus As String = String.Empty
                Dim dtmCreationDateFrom As Nullable(Of DateTime) = Nothing
                Dim dtmCreationDateTo As Nullable(Of DateTime) = Nothing
                Dim strWithClaims As String = String.Empty
                Dim strScheme As String = String.Empty
                Dim strDeceased As String = String.Empty
                Dim dtmDateofDeathFrom As Nullable(Of DateTime) = Nothing
                Dim dtmDateofDeathTo As Nullable(Of DateTime) = Nothing
                Dim strAccountType As String = String.Empty
                Dim strUserID As String = String.Empty

                'UserID
                Dim udtHCVUUserBLL As New HCVUUserBLL
                Dim udtHCVUUser As HCVUUserModel = udtHCVUUserBLL.GetHCVUUser
                If Not IsNothing(udtHCVUUser) Then strUserID = udtHCVUUser.UserID

                'SPID
                strServiceProviderID = Me.txtSPIDR3.Text.Trim
                If Me.txtSPIDR3.Text.Trim.Equals(String.Empty) Then
                    lblAcctListSPIDR3.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    lblAcctListSPIDR3.Text = strServiceProviderID
                End If

                'Manual Validation
                strManualValidaitonStatus = Me.ddlManualValidStatusR3.SelectedValue.Trim
                Me.lblAcctListManualValidStatusR3.Text = Me.ddlManualValidStatusR3.SelectedItem.Text.Trim

                'Creation Date
                If Me.txtCreationDateFromR3.Text.Trim.Equals(String.Empty) AndAlso _
                    Me.txtCreationDateToR3.Text.Trim.Equals(String.Empty) Then
                    lblAcctListCreationDateR3.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    If IsDate(udtformatter.convertDate(Me.txtCreationDateFromR3.Text.Trim, "E")) Then
                        dtmCreationDateFrom = udtformatter.convertDate(Me.txtCreationDateFromR3.Text.Trim, "E")
                    End If

                    If IsDate(udtformatter.convertDate(Me.txtCreationDateToR3.Text.Trim, "E")) Then
                        dtmCreationDateTo = udtformatter.convertDate(Me.txtCreationDateToR3.Text.Trim, "E")
                    End If
                    lblAcctListCreationDateR3.Text = String.Format("{0} {1} {2}", _
                                                    udtformatter.convertDate(Me.txtCreationDateFromR3.Text.Trim, String.Empty), _
                                                    Me.GetGlobalResourceObject("Text", "To_S"), _
                                                    udtformatter.convertDate(Me.txtCreationDateToR3.Text.Trim, String.Empty))
                End If

                'With Claims (Scheme)
                strWithClaims = Me.ddlWithClaimsR3.SelectedValue.Trim
                If Not Me.ddlWithClaimsR3.SelectedValue.Trim.Equals(String.Empty) Then
                    strScheme = ddlSchemeR3.SelectedValue.Trim
                End If

                'With Claims
                If Me.ddlWithClaimsR3.SelectedValue.Trim.Equals(String.Empty) Then
                    'Any
                    lblAcctListWithClaimsR3.Text = ddlWithClaimsR3.SelectedItem.Text.Trim
                ElseIf Me.ddlWithClaimsR3.SelectedValue.Trim.Equals(YesNo.Yes) Then
                    'Yes
                    If String.IsNullOrEmpty(ddlSchemeR3.SelectedValue) Then
                        lblAcctListWithClaimsR3.Text = ddlWithClaimsR3.SelectedItem.Text.Trim
                    Else
                        lblAcctListWithClaimsR3.Text = String.Format("{0} ({1})", ddlWithClaimsR3.SelectedItem.Text.Trim, ddlSchemeR3.SelectedValue)
                    End If
                ElseIf Me.ddlWithClaimsR3.SelectedValue.Trim.Equals(YesNo.Yes) Then
                    'No
                    lblAcctListWithClaimsR3.Text = Me.GetGlobalResourceObject("Text", "Nil")
                End If

                'Deceased + Date of Death
                strDeceased = Me.ddlDeceasedR3.SelectedValue.Trim

                If Me.ddlDeceasedR3.SelectedValue.Trim.Equals(String.Empty) Then
                    lblAcctListDeceasedR3.Text = Me.GetGlobalResourceObject("Text", "Any")
                    lblAcctListDateofDeathR3.Text = Me.GetGlobalResourceObject("Text", "Any")
                ElseIf Me.ddlDeceasedR3.SelectedValue.Trim.Equals(YesNo.Yes) Then
                    lblAcctListDeceasedR3.Text = Me.GetGlobalResourceObject("Text", "Yes")
                ElseIf Me.ddlDeceasedR3.SelectedValue.Trim.Equals(YesNo.No) Then
                    lblAcctListDeceasedR3.Text = Me.GetGlobalResourceObject("Text", "No")
                    lblAcctListDateofDeathR3.Text = Me.GetGlobalResourceObject("Text", "Any")
                End If

                If Me.ddlDeceasedR3.SelectedValue.Trim.Equals(YesNo.Yes) Then
                    'No input
                    If Me.txtDateofDeathFromR3.Text.Trim.Equals(String.Empty) AndAlso _
                        Me.txtDateofDeathToR3.Text.Trim.Equals(String.Empty) Then
                        lblAcctListDateofDeathR3.Text = Me.GetGlobalResourceObject("Text", "Any")
                    Else
                        'With value
                        If IsDate(udtformatter.convertDate(Me.txtDateofDeathFromR3.Text.Trim, "E")) Then
                            dtmDateofDeathFrom = udtformatter.convertDate(Me.txtDateofDeathFromR3.Text.Trim, "E")
                        End If

                        If IsDate(udtformatter.convertDate(Me.txtDateofDeathToR3.Text.Trim, "E")) Then
                            dtmDateofDeathTo = udtformatter.convertDate(Me.txtDateofDeathToR3.Text.Trim, "E")
                        End If
                        lblAcctListDateofDeathR3.Text = String.Format("{0} {1} {2}", _
                                                        udtformatter.convertDate(Me.txtDateofDeathFromR3.Text.Trim, String.Empty), _
                                                        Me.GetGlobalResourceObject("Text", "To_S"), _
                                                        udtformatter.convertDate(Me.txtDateofDeathToR3.Text.Trim, String.Empty))
                    End If
                End If

                'Account Type
                strAccountType = ddlAcctTypeR3.SelectedValue.Trim
                Me.lblAcctListAcctTypeR3.Text = Me.ddlAcctTypeR3.SelectedItem.Text


                'bllSearchResult = udteHSAccountMaintBLL.GeteHSAcctManualValidation(Me.FunctionCode, strServiceProviderID, strManualValidaitonStatus, dtmCreationDateFrom, dtmCreationDateTo, strWithClaims, strScheme, strDeceased, dtmDateofDeathFrom, dtmDateofDeathTo, blnOverrideResultLimit)
                bllSearchResult = udteHSAccountMaintBLL.GeteHSAcctManualValidation(Me.FunctionCode, _
                                                strServiceProviderID, strManualValidaitonStatus, _
                                                dtmCreationDateFrom, dtmCreationDateTo, _
                                                strWithClaims, strScheme, _
                                                strDeceased, dtmDateofDeathFrom, dtmDateofDeathTo, strAccountType, _
                                                strUserID, _
                                                blnOverrideResultLimit)
        End Select

        'Else
        '    ' blnOverrideResultLimit = False
        '    Select Case Me.tcSearchRoute.ActiveTabIndex
        '        Case 0
        '            Dim strAccountType As String = String.Empty

        '            strAccountType = Me.ddlSearchAcctTypeR1.SelectedValue.Trim

        '            If strAccountType = VRAcctMaintenanceStatus.Temporary Then
        '                strAccountType = EHSAccountModel.SysAccountSourceClass.TemporaryAccount

        '            ElseIf strAccountType = VRAcctMaintenanceStatus.Special Then
        '                strAccountType = EHSAccountModel.SysAccountSourceClass.SpecialAccount
        '            End If

        '            If Me.ddlSearchAcctTypeR1.SelectedValue.Trim = VRAcctMaintenanceStatus.Temporary OrElse _
        '                Me.ddlSearchAcctTypeR1.SelectedValue.Trim = VRAcctMaintenanceStatus.Special Then

        '                Select Case Me.ddlSearchTempAcct.SelectedValue.Trim
        '                    Case TempAcctMaintenanceStatus.PendingRemove
        '                        bllSearchResult = udteHSAccountMaintBLL.GeteHSAcctOustandingListFor29Days(Me.FunctionCode, _
        '                                                                                    Me.txtSearchENameR1.Text.Trim, _
        '                                                                                    Me.txtSearchCreationDateFromR1.Text.Trim, _
        '                                                                                    Me.txtSearchCreationDateToR1.Text.Trim)

        '                    Case TempAcctMaintenanceStatus.OutstandingValidation
        '                        bllSearchResult = udteHSAccountMaintBLL.GeteHSAcctOutstandingValidationList(Me.FunctionCode, _
        '                                                                                        Me.txtSearchENameR1.Text.Trim, _
        '                                                                                        Me.txtSearchCreationDateFromR1.Text.Trim, _
        '                                                                                        Me.txtSearchCreationDateToR1.Text.Trim, strAccountType)


        '                    Case TempAcctMaintenanceStatus.PendingImmdValidation
        '                        bllSearchResult = udteHSAccountMaintBLL.GeteHSAcctPendingImmdValidationList(Me.FunctionCode, _
        '                                                                                        Me.txtSearchENameR1.Text.Trim, _
        '                                                                                        Me.txtSearchCreationDateFromR1.Text.Trim, _
        '                                                                                        Me.txtSearchCreationDateToR1.Text.Trim, strAccountType)

        '                    Case Else
        '                        bllSearchResult = udteHSAccountMaintBLL.GeteHSAcctListInRouteOne(Me.FunctionCode, _
        '                                                                                                       Me.txtSearchENameR1.Text.Trim, _
        '                                                                                                       Me.txtSearchCreationDateFromR1.Text.Trim, _
        '                                                                                                       Me.txtSearchCreationDateToR1.Text.Trim, _
        '                                                                                                       Me.ddlSearchAcctTypeR1.SelectedValue)

        '                End Select
        '            Else
        '                bllSearchResult = udteHSAccountMaintBLL.GeteHSAcctListInRouteOne(Me.FunctionCode, _
        '                                                                   Me.txtSearchENameR1.Text.Trim, _
        '                                                                   Me.txtSearchCreationDateFromR1.Text.Trim, _
        '                                                                   Me.txtSearchCreationDateToR1.Text.Trim, _
        '                                                                   Me.ddlSearchAcctTypeR1.SelectedValue)
        '            End If

        '        Case 1
        '            Dim strExactDOB As String = String.Empty
        '            Dim dtDOB As Nullable(Of DateTime) = Nothing
        '            Dim strDocCode As String = String.Empty
        '            Dim strAdoptionPrefixNum As String = String.Empty
        '            Dim strIdentityNum As String = String.Empty
        '            Dim streHSAccountID As String = String.Empty
        '            Dim arreHSAccountID() As String = Nothing
        '            Dim strRefNo As String = String.Empty

        '            'Doc Type
        '            Me.lblAcctListDocTypeR2.Text = Me.ddlSearchDocTypeR2.SelectedItem.Text.Trim
        '            strDocCode = Me.ddlSearchDocTypeR2.SelectedValue.Trim

        '            'Identity Num
        '            If Me.txtSearchIdentityNumR2.Text.Trim.Equals(String.Empty) Then
        '                Me.lblAcctListIdentityNumR2.Text = Me.GetGlobalResourceObject("Text", "Any")
        '            Else
        '                Me.lblAcctListIdentityNumR2.Text = Me.txtSearchIdentityNumR2.Text.Trim.ToUpper

        '                Dim strIdentityNumFullTemp As String
        '                strIdentityNumFullTemp = Me.txtSearchIdentityNumR2.Text.Trim.ToUpper.Replace("-", "").Replace("(", "").Replace(")", "")

        '                Dim strIdentityNumFull() As String
        '                strIdentityNumFull = strIdentityNumFullTemp.Trim.Split("/")
        '                If strIdentityNumFull.Length > 1 Then
        '                    strIdentityNum = strIdentityNumFull(1)
        '                    strAdoptionPrefixNum = strIdentityNumFull(0)
        '                Else
        '                    strIdentityNum = strIdentityNumFullTemp
        '                End If
        '            End If

        '            'English Name
        '            If Me.txtSearchENameR2.Text.Equals(String.Empty) Then
        '                Me.lblAcctListENameR2.Text = Me.GetGlobalResourceObject("Text", "Any")
        '            Else
        '                Me.lblAcctListENameR2.Text = Me.txtSearchENameR2.Text.Trim
        '            End If

        '            'DOB
        '            If Me.txtSearchDOBR2.Text.Trim.Equals(String.Empty) Then
        '                Me.lblAcctListDOBR2.Text = Me.GetGlobalResourceObject("Text", "Any")
        '            Else
        '                Dim dtDOBValue As DateTime
        '                'DOB passed to "chkDOB" must be of DateTime instead of Nullable(of DateTime) 
        '                udtSM = Me.udtvalidator.chkDOB(strDocCode, Me.txtSearchDOBR2.Text.Trim, dtDOBValue, strExactDOB)
        '                If Not IsNothing(udtSM) Then
        '                    Me.imgDOBError.Visible = True
        '                    Me.udcMsgBox.AddMessage(udtSM)
        '                Else
        '                    dtDOB = dtDOBValue
        '                    ' CRE12-015 Add the respective practice number in ¡§Practice¡¨ in the functions under ¡§Reimbursement¡¨ in eHS [Start][Koala]
        '                    ' -----------------------------------------------------------------------------------------------------------------------------
        '                    Me.lblAcctListDOBR2.Text = udtformatter.formatDOB(dtDOB, strExactDOB, Session(SESS_Language), Nothing, Nothing)
        '                    ' CRE12-015 Add the respective practice number in ¡§Practice¡¨ in the functions under ¡§Reimbursement¡¨ in eHS [End][Koala]
        '                End If
        '            End If

        '            'eHealth Account ID (Vaildated Account ID)
        '            Me.CollapsiblePanelExtenderAccountIDR2.Enabled = False
        '            Me.CollapsiblePanelExtenderAccountIDR2.Collapsed = True
        '            Me.ibtnAcctListAccountIDR2Multiple.Visible = False
        '            Me.txtAcctListAccountIDR2Multiple.Text = String.Empty

        '            If Me.txtSearchAccountIDR2.Text.Trim.Equals(String.Empty) Then
        '                Me.lblAcctListAccountIDR2.Text = Me.GetGlobalResourceObject("Text", "Any")
        '            Else

        '                'If udtvalidator.chkSystemNumber(Me.txtSearchAccountIDR2.Text.Trim) Then
        '                '    streHSAccountID = Common.Format.Formatter.ReverseSystemNumber(Me.txtSearchAccountIDR2.Text.Trim)
        '                'Else
        '                '    streHSAccountID = Me.txtSearchAccountIDR2.Text.Trim
        '                'End If

        '                ' Remove Check Digit Before Search
        '                ' ToDo: Check is valid check digit
        '                streHSAccountID = Me.txtSearchAccountIDR2.Text.Trim
        '                If Not String.IsNullOrEmpty(streHSAccountID) Then
        '                    arreHSAccountID = streHSAccountID.Split(New Char() {EHAccountIDSeparator}, StringSplitOptions.RemoveEmptyEntries)
        '                    For i As Integer = 0 To arreHSAccountID.Length - 1
        '                        arreHSAccountID(i) = arreHSAccountID(i).Substring(0, arreHSAccountID(i).Length - 1)
        '                    Next
        '                End If

        '                If arreHSAccountID.Length = 1 Then
        '                    Me.lblAcctListAccountIDR2.Text = GetMultipleAccountIDList(arreHSAccountID, 0, 0)
        '                ElseIf arreHSAccountID.Length = 2 Then
        '                    Me.lblAcctListAccountIDR2.Text = GetMultipleAccountIDList(arreHSAccountID, 0, 1)
        '                ElseIf arreHSAccountID.Length > 2 Then
        '                    Me.lblAcctListAccountIDR2.Text = GetMultipleAccountIDList(arreHSAccountID, 0, 1)
        '                    Me.CollapsiblePanelExtenderAccountIDR2.Enabled = True
        '                    Me.ibtnAcctListAccountIDR2Multiple.Visible = True
        '                    Me.txtAcctListAccountIDR2Multiple.Text = GetMultipleAccountIDList(arreHSAccountID, 2, arreHSAccountID.Length - 1)
        '                End If

        '            End If

        '            'Ref No. (Temporary Account ID, Special Account ID, Ivalid Account ID)
        '            If Me.txtSearchRefNo.Text.Trim.Equals(String.Empty) Then
        '                Me.lblAcctListRefNoR2.Text = Me.GetGlobalResourceObject("Text", "Any")
        '            Else
        '                Me.lblAcctListRefNoR2.Text = Me.txtSearchRefNo.Text.Trim
        '                If udtvalidator.chkSystemNumber(Me.txtSearchRefNo.Text.Trim) Then
        '                    strRefNo = Common.Format.Formatter.ReverseSystemNumber(Me.txtSearchRefNo.Text.Trim)
        '                Else
        '                    strRefNo = Me.txtSearchRefNo.Text.Trim
        '                End If
        '            End If

        '            bllSearchResult = udteHSAccountMaintBLL.GeteHSAcctListInRouteTwoMultiple(Me.FunctionCode, strDocCode, strIdentityNum, strAdoptionPrefixNum, Me.txtSearchENameR2.Text.Trim, dtDOB, _
        '                                                            arreHSAccountID, strRefNo)

        '    End Select

        'End If

        Return bllSearchResult
    End Function

    Protected Overrides Function SF_BindSearchResult(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry, ByVal udtBLLSearchResult As Common.Component.BaseBLL.BLLSearchResult) As Integer
        Dim dt As DataTable
        Dim intRowCount As Integer
        Dim blnShowResultList As Boolean = False

        Try
            dt = CType(udtBLLSearchResult.Data, DataTable)
        Catch ex As Exception
            Throw
        End Try

        intRowCount = dt.Rows.Count

        Select Case dt.Rows.Count
            Case 0
                ' No record found
                blnShowResultList = False

            Case Else
                blnShowResultList = True

        End Select

        If blnShowResultList Then
            Session(SESS_Result) = dt

            Select Case Me.tcSearchRoute.ActiveTabIndex
                'Case 0
                '    ' Search Route 1
                '    Me.GridViewDataBind(Me.gvAcctListR1, dt, "IdentityNum", "ASC", False)
                '    Me.pnlSearchCriteriaRoute1.Visible = True
                '    Me.pnlSearchCriteriaRoute2.Visible = False

                'Case 1
                '    ' Search Route 2
                '    Me.GridViewDataBind(Me.gvAcctListR2, dt, "Voucher_Acc_ID", "ASC", False)
                '    Me.pnlSearchCriteriaRoute1.Visible = False
                '    Me.pnlSearchCriteriaRoute2.Visible = True

                Case 0
                    ' Search Route 2
                    'Me.GridViewDataBind(Me.gvAcctListR2, dt, "Voucher_Acc_ID", "ASC", False)
                    Me.GridViewDataBind(Me.gvAcctListR2, dt, "", "ASC", False)
                    Me.pnlSearchCriteriaRoute2.Visible = True
                    Me.pnlSearchCriteriaRoute3.Visible = False

                Case 1
                    ' Search Route 3
                    'Me.GridViewDataBind(Me.gvAcctListR3, dt, "Voucher_Acc_ID", "ASC", False)
                    Me.GridViewDataBind(Me.gvAcctListR3, dt, "", "ASC", False)
                    Me.pnlSearchCriteriaRoute2.Visible = False
                    Me.pnlSearchCriteriaRoute3.Visible = True

            End Select

            Me.mveHSAccount.ActiveViewIndex = intSearchResult

        End If

        Return intRowCount
    End Function

    Protected Overrides Sub SF_FinalizeSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry)
    End Sub

    Protected Overrides Sub SF_ConfirmSearch_Click()
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim enumSearchResult As SearchResultEnum

        Select Case Me.tcSearchRoute.ActiveTabIndex
            Case 0
                'Case 1
                ' Search Route 2
                udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00124, AuditLogDesc.SearchByParticulars)

            Case 1
                ' Search Route 3
                udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00127, AuditLogDesc.SearchbyManualValidation)

        End Select

        Try
            enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, udcMsgBox, udcInfoMsgBox, False, True)

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                udcMsgBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, eSQL.Message))

                If udcMsgBox.GetCodeTable.Rows.Count = 0 Then
                    udcMsgBox.Visible = False
                Else
                    udcMsgBox.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00007, AuditLogDesc.SearchFail)
                End If

            Else
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00007, AuditLogDesc.SearchFail)
                Throw eSQL
            End If

        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00007, AuditLogDesc.SearchFail)
            Throw ex
        End Try

        Select Case Me.tcSearchRoute.ActiveTabIndex
            Case 0
                ' Search Route 2
                Select Case enumSearchResult
                    Case SearchResultEnum.Success
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00125, AuditLogDesc.SearchByParticularsSuccess)

                    Case Else
                        Throw New Exception("Error: Class = [HCVU.eHSAccountMaint], Method = [SF_ConfirmSearch_Click], Message = The unexpected error for the result of re-search")

                End Select


            Case 1
                ' Search Route 3
                Select Case enumSearchResult
                    Case SearchResultEnum.Success
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00128, AuditLogDesc.SearchbyManualValidationSuccess)

                    Case Else
                        Throw New Exception("Error: Class = [HCVU.eHSAccountMaint], Method = [SF_ConfirmSearch_Click], Message = The unexpected error for the result of re-search")

                End Select

        End Select

    End Sub

    Protected Overrides Sub SF_CancelSearch_Click()
    End Sub

#End Region
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        FunctionCode = FunctCode.FUNT010301
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        If Not IsPostBack Then
            udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00000, AuditLogDesc.eHSAccountMaintPageLoad)

            ResetControls()

            ' Prevent double-click 
            Dim strBrowser As String = String.Empty
            Try
                If Not HttpContext.Current.Request.Browser Is Nothing Then
                    strBrowser = HttpContext.Current.Request.Browser.Type
                End If
            Catch ex As Exception
                strBrowser = String.Empty
            End Try

            MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnDialogConfirm)
            MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnConfirmAmendedAccount)

        Else
            udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
            udtEHSAccount_Amendment = Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FunctionCode)
            Me.SetupInputDocControl(udtEHSAccount, udtEHSAccount_Amendment, False)

        End If

        If ddlSearchAcctTypeR2.SelectedValue.Trim = VRAcctMaintenanceStatus.Temporary Then
            Me.pnlAdvTempSearchR2.Visible = True
        Else
            Me.pnlAdvTempSearchR2.Visible = False
        End If

        If ddlDeceasedR3.SelectedValue = YesNo.Yes Then
            lblDateofDeathFromR3Text.ForeColor = Drawing.Color.Black
            lblDateofDeathToR3Text.ForeColor = Drawing.Color.Black
            txtDateofDeathFromR3.Enabled = True
            txtDateofDeathToR3.Enabled = True
        Else
            lblDateofDeathFromR3Text.ForeColor = Drawing.Color.Gray
            lblDateofDeathToR3Text.ForeColor = Drawing.Color.Gray
            txtDateofDeathFromR3.Enabled = False
            txtDateofDeathToR3.Enabled = False
        End If

        If ddlWithClaimsR3.SelectedValue = YesNo.Yes Then
            ddlSchemeR3.Enabled = True
        Else
            ddlSchemeR3.SelectedIndex = 0
            ddlSchemeR3.Enabled = False
        End If

        If Not IsPostBack Then
            HandleRedirectAction()
        End If

    End Sub

    ''' <summary>
    ''' CRE11-007
    ''' Keep Mask document no. popup show default on status
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Select Case ViewState(VS.UnmaskPopup)
            Case PopupStatus.Active
                popupUnmask.Show()
        End Select
    End Sub

    Private Sub HandleRedirectAction()
        Dim udtRedirectParameterBLL As New RedirectParameterBLL
        Dim udtRedirectParameter As RedirectParameterModel = udtRedirectParameterBLL.GetFromSession()
        If IsNothing(udtRedirectParameter) Then Return

        udtRedirectParameterBLL.RemoveFromSession()
        udtRedirectParameterBLL.WriteAuditLog(FunctionCode, Me, udtRedirectParameter)

        If udtRedirectParameter.ActionList.Contains(RedirectParameterModel.EnumRedirectAction.Search) Then
            ' --- Auto-perform Search action ---

            If udtRedirectParameter.EHealthAccountID <> String.Empty Then
                ' Validated account
                Dim udtGeneralFunction As New GeneralFunction
                Dim strEHealthAccountPrefix As String = String.Empty
                udtGeneralFunction.getSytemParameterByParameterName("eHealthAccountPrefix", strEHealthAccountPrefix, String.Empty)
                strEHealthAccountPrefix = strEHealthAccountPrefix.Trim

                'tcSearchRoute.ActiveTabIndex = 1
                tcSearchRoute.ActiveTabIndex = 0
                ddlSearchDocTypeR2.SelectedValue = udtRedirectParameter.EHealthAccountDocCode

                ' Handle multiple account ID
                If udtRedirectParameter.EHealthAccountID.Contains(EHAccountIDSeparator) Then
                    Dim arrAccID() As String = udtRedirectParameter.EHealthAccountID.Split(EHAccountIDSeparator)
                    For i As Integer = 0 To arrAccID.Length - 1
                        arrAccID(i) = String.Format("{0}{1}", arrAccID(i), udtGeneralFunction.generateChkDgt(String.Format("{0}{1}", strEHealthAccountPrefix, arrAccID(i))))
                    Next
                    udtRedirectParameter.EHealthAccountID = Join(arrAccID, EHAccountIDSeparator)
                ElseIf udtRedirectParameter.EHealthAccountID.Trim.Length = 8 Then
                    udtRedirectParameter.EHealthAccountID = String.Format("{0}{1}", udtRedirectParameter.EHealthAccountID, udtGeneralFunction.generateChkDgt(String.Format("{0}{1}", strEHealthAccountPrefix, udtRedirectParameter.EHealthAccountID)))
                End If

                txtSearchAccountIDR2.Text = udtRedirectParameter.EHealthAccountID
                ibtnSearch_Click(Nothing, Nothing)

            ElseIf udtRedirectParameter.EHealthAccountReferenceNo <> String.Empty Then
                ' Temporary account
                Dim udtFormatter As New Formatter

                'tcSearchRoute.ActiveTabIndex = 1
                tcSearchRoute.ActiveTabIndex = 0
                txtSearchRefNo.Text = udtFormatter.formatSystemNumber(udtRedirectParameter.EHealthAccountReferenceNo)
                ibtnSearch_Click(Nothing, Nothing)

            End If

        End If

        If udtRedirectParameter.ActionList.Contains(RedirectParameterModel.EnumRedirectAction.ViewDetail) Then
            ' --- Auto-perform Row Command (click) action ---

            ' Locate the link button for the row command action
            If gvAcctListR2.Rows.Count = 1 Then ' Throw New Exception(String.Format("eHSAccountEnquiry.HandleRedirectAction: Unexpected no. of rows {0}", gvAcctListR2.Rows.Count))

                Dim lbtnAccountID As LinkButton = gvAcctListR2.Rows(0).FindControl("lbtnAccountID")

                Dim arg As New CommandEventArgs(lbtnAccountID.CommandName, lbtnAccountID.CommandArgument)
                Dim e As New GridViewCommandEventArgs(gvAcctListR2.Rows(0), lbtnAccountID, arg)

                gvAcctListR2_RowCommand(gvAcctListR2, e)

                ibtnAccountDetailsBack.Visible = False
            ElseIf gvAcctListR3.Rows.Count = 1 Then
                Dim lbtnAccountID As LinkButton = gvAcctListR3.Rows(0).FindControl("lbtnAccountID")

                Dim arg As New CommandEventArgs(lbtnAccountID.CommandName, lbtnAccountID.CommandArgument)
                Dim e As New GridViewCommandEventArgs(gvAcctListR3.Rows(0), lbtnAccountID, arg)

                gvAcctListR3_RowCommand(gvAcctListR3, e)

                ibtnAccountDetailsBack.Visible = False

            Else
                ibtnSearchResultBack.Visible = False
            End If
        End If

    End Sub

#End Region

#Region "View 1 - Search"

    Private Sub tcSearchRoute_ActiveTabChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tcSearchRoute.ActiveTabChanged

        ' Turn On / Off the making new claim function
        Dim strTurnOnNewClaim As String = String.Empty
        Me.udtCommonFunction.getSystemParameter("TurnOnOutsidePaymentClaim", strTurnOnNewClaim, String.Empty)

        If strTurnOnNewClaim.Trim.Equals("Y") Then
            Me.ibtnNewAccountR2.Enabled = True
            Me.ibtnNewAccountR2.Visible = True
            Me.ibtnNewAccountR2.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "NewAccountBtn")
        Else
            Me.ibtnNewAccountR2.Visible = False
        End If

    End Sub

    Protected Sub ddlSearchAcctTypeR2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If ddlSearchAcctTypeR2.SelectedValue.Trim = VRAcctMaintenanceStatus.Temporary Then

            Me.pnlAdvTempSearchR2.Visible = True

            Me.ddlSearchTempAcct.Items.Clear()

            Dim dv As New DataView(Common.Component.Status.GetDescriptionListFromDBEnumCode(TempAcctMaintenanceStatusByParticular.ClassCode, True))
            Me.ddlSearchTempAcct.DataSource = dv

            Me.ddlSearchTempAcct.DataTextField = "Status_Description"
            Me.ddlSearchTempAcct.DataValueField = "Status_Value"
            Me.ddlSearchTempAcct.DataBind()

            Me.ddlSearchTempAcct.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))

        Else
            Me.pnlAdvTempSearchR2.Visible = False
        End If

    End Sub

    Protected Sub ddlWithClaimsR3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If ddlWithClaimsR3.SelectedValue.Trim = YesNo.Yes Then
            ddlSchemeR3.Enabled = True
        Else
            ddlSchemeR3.SelectedIndex = 0
            ddlSchemeR3.Enabled = False
        End If
    End Sub

    Protected Sub ddlDeceasedR3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If ddlDeceasedR3.SelectedValue.Trim = YesNo.Yes Then
            lblDateofDeathFromR3Text.ForeColor = Drawing.Color.Black
            txtDateofDeathFromR3.Enabled = True
            lblDateofDeathToR3Text.ForeColor = Drawing.Color.Black
            txtDateofDeathToR3.Enabled = True
            btnDateofDeathFromR3.Enabled = True
            btnDateofDeathToR3.Enabled = True
        Else
            lblDateofDeathFromR3Text.ForeColor = Drawing.Color.Gray
            txtDateofDeathFromR3.Text = String.Empty
            txtDateofDeathFromR3.Enabled = False
            lblDateofDeathToR3Text.ForeColor = Drawing.Color.Gray
            txtDateofDeathToR3.Text = String.Empty
            txtDateofDeathToR3.Enabled = False
            btnDateofDeathFromR3.Enabled = False
            btnDateofDeathToR3.Enabled = False
            imgDateofDeathErrorR3.Visible = False
        End If
    End Sub

    Protected Sub ibtnSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        Me.udcInfoMsgBox.Visible = False
        Me.udcMsgBox.Visible = False
        Me.chkMaskDocumentNoR2.Checked = True
        Me.chkMaskDocumentNoR3.Checked = True

        ClearErrorImage()

        ' CRE12-015 Add the respective practice number in ¡§Practice¡¨ in the functions under ¡§Reimbursement¡¨ in eHS [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------------------------------------------
        ' Implement Collapsible Search Criteria Review
        'udcCollapsibleSearchCriteriaReview1.Collapsed = True
        'udcCollapsibleSearchCriteriaReview1.ClientState = "True"
        udcCollapsibleSearchCriteriaReview2.Collapsed = True
        udcCollapsibleSearchCriteriaReview2.ClientState = "True"
        udcCollapsibleSearchCriteriaReview3.Collapsed = True
        udcCollapsibleSearchCriteriaReview3.ClientState = "True"
        ' CRE12-015 Add the respective practice number in ¡§Practice¡¨ in the functions under ¡§Reimbursement¡¨ in eHS [End][Tommy L]

        If IsNothing(sender) And IsNothing(e) Then
            udtAuditLogEntry.AddDescripton("Trigger", "Auto")
        Else
            udtAuditLogEntry.AddDescripton("Trigger", "Manual")
        End If

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
        ' -------------------------------------------------------------------------
        Dim enumSearchResult As SearchResultEnum
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

        Try
            Select Case Me.tcSearchRoute.ActiveTabIndex

                Case 0
                    ' Search Route 2
                    udtAuditLogEntry.AddDescripton("EngName", Me.txtSearchENameR2.Text.Trim)
                    ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
                    udtAuditLogEntry.AddDescripton("ChiName", Me.txtSearchCNameR2.Text.Trim)
                    ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]
                    udtAuditLogEntry.AddDescripton("DocType", Me.ddlSearchDocTypeR2.SelectedValue)
                    udtAuditLogEntry.AddDescripton("IdentityNumber", Me.txtSearchIdentityNumR2.Text)
                    udtAuditLogEntry.AddDescripton("DOB", Me.txtSearchDOBR2.Text)
                    udtAuditLogEntry.AddDescripton("AccountID", Me.txtSearchAccountIDR2.Text)
                    udtAuditLogEntry.AddDescripton("RefNo", Me.txtSearchRefNo.Text)
                    'udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00004, AuditLogDesc.SearchRoute2)
                    udtAuditLogEntry.AddDescripton("AccountType", ddlSearchAcctTypeR2.SelectedItem.Text)
                    If Me.ddlSearchAcctTypeR2.SelectedValue.Trim = VRAcctMaintenanceStatus.Temporary Then
                        udtAuditLogEntry.AddDescripton("Temp Account Type", ddlSearchTempAcct.SelectedItem.Text)
                    Else
                        udtAuditLogEntry.AddDescripton("Temp Account Type", String.Empty)
                    End If
                    udtAuditLogEntry.AddDescripton("CreationDateFrom", Me.txtSearchCreationDateFromR2.Text)
                    udtAuditLogEntry.AddDescripton("CreationDateTo", Me.txtSearchCreationDateToR2.Text)
                    udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00124, AuditLogDesc.SearchByParticulars)

                    If sender Is Nothing Then
                        enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, udcMsgBox, udcInfoMsgBox, False, True)
                    Else
                        enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, udcMsgBox, udcInfoMsgBox)
                    End If

                    Select Case enumSearchResult
                        Case SearchResultEnum.Success
                            udtAuditLogEntry.WriteEndLog(LogID.LOG00125, AuditLogDesc.SearchByParticularsSuccess)

                        Case SearchResultEnum.ValidationFail
                            ' Audit Log has been handled in [SF_ValidateSearch] method

                        Case SearchResultEnum.NoRecordFound
                            udtAuditLogEntry.WriteEndLog(LogID.LOG00125, AuditLogDesc.SearchByParticularsSuccess)

                        Case SearchResultEnum.OverResultList1stLimit_PopUp
                            udtAuditLogEntry.WriteEndLog(LogID.LOG00126, AuditLogDesc.SearchByParticularsFail)

                        Case SearchResultEnum.OverResultList1stLimit_Alert
                            udtAuditLogEntry.WriteEndLog(LogID.LOG00126, AuditLogDesc.SearchByParticularsFail)

                        Case SearchResultEnum.OverResultListOverrideLimit
                            udtAuditLogEntry.WriteEndLog(LogID.LOG00126, AuditLogDesc.SearchByParticularsFail)

                        Case Else
                            Throw New Exception("Error: Class = [HCVU.eHSAccountMaint], Method = [ibtnSearch_Click], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")

                    End Select

                Case 1
                    ' Search Route 3
                    udtAuditLogEntry.AddDescripton("SPID", Me.txtSPIDR3.Text.Trim)
                    udtAuditLogEntry.AddDescripton("ManualValidationStatus", Me.ddlManualValidStatusR3.SelectedItem.Text)
                    udtAuditLogEntry.AddDescripton("CreationDateFrom", Me.txtCreationDateFromR3.Text)
                    udtAuditLogEntry.AddDescripton("CreationDateTo", Me.txtCreationDateToR3.Text)
                    udtAuditLogEntry.AddDescripton("WithClaims", Me.ddlWithClaimsR3.SelectedItem.Text)
                    If Me.ddlWithClaimsR3.SelectedValue.Trim.Equals(YesNo.Yes) Then
                        udtAuditLogEntry.AddDescripton("Scheme", Me.ddlSchemeR3.SelectedItem.Text)
                    Else
                        udtAuditLogEntry.AddDescripton("Scheme", String.Empty)
                    End If
                    udtAuditLogEntry.AddDescripton("Deceased", Me.ddlDeceasedR3.SelectedItem.Text)
                    udtAuditLogEntry.AddDescripton("DateofDeathFrom", Me.txtDateofDeathFromR3.Text)
                    udtAuditLogEntry.AddDescripton("DateofDeathTo", Me.txtDateofDeathToR3.Text)
                    udtAuditLogEntry.AddDescripton("AccountType", ddlAcctTypeR3.SelectedItem.Text)
                    udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00127, AuditLogDesc.SearchbyManualValidation)

                    If sender Is Nothing Then
                        enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, udcMsgBox, udcInfoMsgBox, False, True)
                    Else
                        enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, udcMsgBox, udcInfoMsgBox)
                    End If

                    Select Case enumSearchResult
                        Case SearchResultEnum.Success
                            udtAuditLogEntry.WriteEndLog(LogID.LOG00128, AuditLogDesc.SearchbyManualValidationSuccess)

                        Case SearchResultEnum.ValidationFail
                            ' Audit Log has been handled in [SF_ValidateSearch] method

                        Case SearchResultEnum.NoRecordFound
                            udtAuditLogEntry.WriteEndLog(LogID.LOG00128, AuditLogDesc.SearchbyManualValidationSuccess)

                        Case SearchResultEnum.OverResultList1stLimit_PopUp
                            udtAuditLogEntry.WriteEndLog(LogID.LOG00129, AuditLogDesc.SearchbyManualValidationFail)

                        Case SearchResultEnum.OverResultList1stLimit_Alert
                            udtAuditLogEntry.WriteEndLog(LogID.LOG00129, AuditLogDesc.SearchbyManualValidationFail)

                        Case SearchResultEnum.OverResultListOverrideLimit
                            udtAuditLogEntry.WriteEndLog(LogID.LOG00129, AuditLogDesc.SearchbyManualValidationFail)

                        Case Else
                            Throw New Exception("Error: Class = [HCVU.eHSAccountMaint], Method = [ibtnSearch_Click], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")

                    End Select

            End Select

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                Dim strmsg As String
                strmsg = eSQL.Message

                udtSM = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                udcMsgBox.AddMessage(udtSM)
                If udcMsgBox.GetCodeTable.Rows.Count = 0 Then
                    udcMsgBox.Visible = False
                Else
                    udcMsgBox.BuildMessageBox("SearchFail", udtAuditLogEntry, Common.Component.LogID.LOG00007, AuditLogDesc.SearchFail)
                End If
            Else
                Throw eSQL
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub ibtnNewAccount_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim udtAuditLogInfo As AuditLogInfo

        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogInfo = New AuditLogInfo(Nothing, Nothing, Nothing, _
                                        Nothing, Me.ddlSearchDocTypeR2.SelectedValue, Me.udtformatter.formatDocumentIdentityNumber(Me.ddlSearchDocTypeR2.SelectedValue, Me.txtSearchIdentityNumR2.Text.Replace("(", "").Replace(")", "").Replace("-", "")))
        udtAuditLogEntry.AddDescripton("DocType", Me.ddlSearchDocTypeR2.SelectedValue)
        udtAuditLogEntry.AddDescripton("IdentityNo", Me.txtSearchIdentityNumR2.Text)
        udtAuditLogEntry.AddDescripton("AccName", txtSearchENameR2.Text)
        udtAuditLogEntry.AddDescripton("DOB", Me.txtSearchDOBR2.Text.Trim)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00097, AuditLogDesc.CreateNewAccountButtonClick, udtAuditLogInfo)

        'Account Creation : Not to select scheme and input SP info
        Me.pnlCreationSPIDScheme.Visible = False

        ClearErrorImage()

        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]

        If Not Me.ddlSearchDocTypeR2.SelectedValue.Trim.Equals(String.Empty) Then
            If Not (New DocTypeBLL).getAllDocType.Filter(Me.ddlSearchDocTypeR2.SelectedValue).IMMDorManualValidationAvailable Then
                Me.imgDocTypeError.Visible = True
                udtSM = New SystemMessage(FuncCode, SeverityCode.SEVE, MsgCode.MSG00012)
                Me.udcMsgBox.AddMessage(udtSM)

                udtAuditLogEntry.AddDescripton("DocType", Me.ddlSearchDocTypeR2.SelectedValue)
                udtAuditLogEntry.AddDescripton("IdentityNo", Me.txtSearchIdentityNumR2.Text)
                udtAuditLogInfo = New AuditLogInfo(Nothing, Nothing, Nothing, _
                                                Nothing, Me.ddlSearchDocTypeR2.SelectedValue, Me.udtformatter.formatDocumentIdentityNumber(Me.ddlSearchDocTypeR2.SelectedValue, Me.txtSearchIdentityNumR2.Text.Replace("(", "").Replace(")", "").Replace("-", "")))
                Me.udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00098, AuditLogDesc.CreateNewAccount_Reject, udtAuditLogInfo)
            End If
        End If

        If Not IsNothing(udtSM) Then
            Exit Sub
        End If
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]

        If Me.ddlSearchDocTypeR2.SelectedValue.Trim.Equals(String.Empty) Or Me.txtSearchIdentityNumR2.Text.Trim.Equals(String.Empty) Then
            'Not enough input information

            If Me.ddlSearchDocTypeR2.SelectedValue.Trim.Equals(String.Empty) Then
                Me.imgDocTypeError.Visible = True
                udtSM = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00268)
                Me.udcMsgBox.AddMessage(udtSM)
            End If

            If Me.txtSearchIdentityNumR2.Text.Trim.Equals(String.Empty) Then
                Me.imgIdentityNumErr.Visible = True
                udtSM = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00269)
                Me.udcMsgBox.AddMessage(udtSM)
            End If

            udtAuditLogEntry.AddDescripton("DocType", Me.ddlSearchDocTypeR2.SelectedValue)
            udtAuditLogEntry.AddDescripton("IdentityNo", Me.txtSearchIdentityNumR2.Text)
            udtAuditLogInfo = New AuditLogInfo(Nothing, Nothing, Nothing, _
                                            Nothing, Me.ddlSearchDocTypeR2.SelectedValue, Me.udtformatter.formatDocumentIdentityNumber(Me.ddlSearchDocTypeR2.SelectedValue, Me.txtSearchIdentityNumR2.Text.Replace("(", "").Replace(")", "").Replace("-", "")))
            Me.udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00098, AuditLogDesc.CreateNewAccount_Reject, udtAuditLogInfo)
        Else
            '--------Temporary code ------------
            Me.pnlDocumentTypeRadioButtonGroup.Visible = False
            '-----------------------------------

            Dim strDocCode As String = String.Empty
            Dim strIdentityNumFullTemp As String = String.Empty
            Dim strIdentityNum As String = String.Empty
            Dim strAdoptionPrefixNum As String = String.Empty
            Dim strSurName As String = String.Empty
            Dim strGivenName As String = String.Empty
            Dim dtDOBValue As DateTime
            Dim dtDOB As DateTime
            Dim strExactDOB As String = String.Empty

            'Doc Code
            strDocCode = Me.ddlSearchDocTypeR2.SelectedValue.Trim
            Me.udcStep1DocumentTypeRadioButtonGroup.SelectedValue = strDocCode
            ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [Start][Lawrence]
            Me.lblCreationDocumentType.Text = AntiXssEncoder.HtmlEncode(Me.ddlSearchDocTypeR2.SelectedItem.Text, True)
            ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [End][Lawrence]
            Me.txtDocCode.Text = strDocCode

            'Identity Number
            strIdentityNumFullTemp = Me.txtSearchIdentityNumR2.Text.Trim.ToUpper.Replace("-", "").Replace("(", "").Replace(")", "")


            ' CRE20-0022 (Immu record) [Start][Martin]
            'only OW and PASS are free text
            If strDocCode = DocTypeModel.DocTypeCode.PASS Or strDocCode = DocTypeModel.DocTypeCode.OW Then
                strIdentityNum = Me.txtSearchIdentityNumR2.Text.Trim()
            Else
                Dim strIdentityNumFull() As String
                strIdentityNumFull = strIdentityNumFullTemp.Trim.Split("/")
                If strIdentityNumFull.Length > 1 Then
                    strIdentityNum = strIdentityNumFull(1)
                    strAdoptionPrefixNum = strIdentityNumFull(0)
                Else
                    strIdentityNum = strIdentityNumFullTemp
                End If
            End If
            ' CRE20-0022 (Immu record) [End][Martin]

            udtSM = Me.udtValidator.chkIdentityNumber(strDocCode, strIdentityNum, strAdoptionPrefixNum)
            If Not IsNothing(udtSM) Then
                Me.imgIdentityNumErr.Visible = True
                Me.udcMsgBox.AddMessage(udtSM)
            End If

            'Name
            Dim strFullName() As String
            strFullName = txtSearchENameR2.Text.Trim.Split(",")
            If strFullName.Length > 1 Then
                strSurName = strFullName(0)
                strGivenName = strFullName(1)
            Else
                strSurName = txtSearchENameR2.Text.Trim
            End If

            'DOB
            If Not Me.txtSearchDOBR2.Text.Trim = String.Empty Then
                udtSM = Me.udtValidator.chkDOB(strDocCode, Me.txtSearchDOBR2.Text.Trim, dtDOBValue, strExactDOB)
                If Not IsNothing(udtSM) Then
                    Me.imgDOBError.Visible = True
                    Me.udcMsgBox.AddMessage(udtSM)
                Else
                    dtDOB = dtDOBValue
                End If
            End If

            'Check existenance of validated account
            Dim blnSameIDAccFoundButAllowProceed As Boolean
            blnSameIDAccFoundButAllowProceed = False

            If IsNothing(udtSM) Then
                Me.udtSM = Me.udteHSAccountMaintBLL.SearchEHSAccountForBOAccountCreation(strDocCode, _
                                            strIdentityNum, blnSameIDAccFoundButAllowProceed, _
                                            strAdoptionPrefixNum)
                Me.udcMsgBox.AddMessage(udtSM)
            End If

            If IsNothing(udtSM) Then

                ResetAccountCreationControls()

                If blnSameIDAccFoundButAllowProceed Then
                    Me.udcInfoMsgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00032))
                    Me.udcInfoMsgBox.BuildMessageBox()
                    Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Information
                End If

                Me.mveHSAccount.ActiveViewIndex = intNewAccount

                'Create a EHSAccount object for account creation
                Me.udtEHSAccount = New EHSAccountModel()

                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                ' Remove dummy EHSAccountModel in EHSPersonalInformationList
                ' Me.udtEHSAccount.EHSPersonalInformationList.Add(New EHSAccount.EHSAccountModel)
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                Me.udtEHSAccount.EHSPersonalInformationList(0).DocCode = strDocCode
                Me.udtEHSAccount.EHSPersonalInformationList(0).IdentityNum = strIdentityNum
                Me.udtEHSAccount.EHSPersonalInformationList(0).AdoptionPrefixNum = strAdoptionPrefixNum
                Me.udtEHSAccount.EHSPersonalInformationList(0).ENameSurName = strSurName.Trim
                Me.udtEHSAccount.EHSPersonalInformationList(0).ENameFirstName = strGivenName.Trim
                Me.udtEHSAccount.EHSPersonalInformationList(0).DOB = dtDOB
                Me.udtEHSAccount.EHSPersonalInformationList(0).ExactDOB = strExactDOB

                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
                ' -----------------------------------------------------------------------------------------
                ' Fill Deceased Information
                If udtEHSAccount.DeathRecord.IsDead Then
                    udtEHSAccount.Deceased = True
                    udtEHSAccount.EHSPersonalInformationList(0).Deceased = True
                    udtEHSAccount.EHSPersonalInformationList(0).DOD = udtEHSAccount.DeathRecord.DOD
                    udtEHSAccount.EHSPersonalInformationList(0).ExactDOD = udtEHSAccount.DeathRecord.ExactDOD
                End If
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]

                'Save to session
                Me.udteHSAccountMaintBLL.EHSAccountSaveToSession(Me.udtEHSAccount, FunctionCode)

                'clear error message (Controls)
                Me.ucInputDocumentType_NewAcc.ActiveViewChanged = True
                'bind controls
                BindPersonalInfoForCreation(Me.udtEHSAccount, strDocCode, True)

                Session(SESS_ActionMode) = EditAccountModel.NewAccount

                udtAuditLogEntry.AddDescripton("DocType", Me.ddlSearchDocTypeR2.SelectedValue)
                udtAuditLogEntry.AddDescripton("IdentityNo", Me.txtSearchIdentityNumR2.Text)
                udtAuditLogInfo = New AuditLogInfo(Nothing, Nothing, Nothing, _
                                                           Nothing, Me.ddlSearchDocTypeR2.SelectedValue, Me.udtformatter.formatDocumentIdentityNumber(Me.ddlSearchDocTypeR2.SelectedValue, Me.txtSearchIdentityNumR2.Text.Replace("(", "").Replace(")", "").Replace("-", "")))
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00099, AuditLogDesc.CreateNewAccount_EnterAccInputDetails, udtAuditLogInfo)
            Else
                udtAuditLogEntry.AddDescripton("DocType", Me.ddlSearchDocTypeR2.SelectedValue)
                udtAuditLogEntry.AddDescripton("IdentityNo", Me.txtSearchIdentityNumR2.Text)
                udtAuditLogInfo = New AuditLogInfo(Nothing, Nothing, Nothing, _
                                                           Nothing, Me.ddlSearchDocTypeR2.SelectedValue, Me.udtformatter.formatDocumentIdentityNumber(Me.ddlSearchDocTypeR2.SelectedValue, Me.txtSearchIdentityNumR2.Text.Replace("(", "").Replace(")", "").Replace("-", "")))
                Me.udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00098, AuditLogDesc.CreateNewAccount_Reject, udtAuditLogInfo)
            End If

        End If
    End Sub

    Private Function getSchemeList() As DataTable
        ' Bind scheme
        Dim dt As New DataTable

        dt.Columns.Add("SchemeCode")
        dt.Columns.Add("Description")
        dt.Columns.Add("SortingField")

        Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = udtSchemeClaimBLL.getAllSchemeClaim_WithSubsidizeGroup
        Dim udtSubsidizeGroupClaimModelCollection As SubsidizeGroupClaimModelCollection = New SubsidizeGroupClaimModelCollection

        For Each udtSchemeClaimModel As SchemeClaimModel In udtSchemeClaimModelCollection
            Dim dr As DataRow = dt.NewRow()
            dr.Item(0) = udtSchemeClaimModel.DisplayCode.Trim
            dr.Item(1) = udtSchemeClaimModel.SchemeDesc.Trim
            dt.Rows.Add(dr)
            For Each udtSubsidizeGroupClaimModel As SubsidizeGroupClaimModel In udtSchemeClaimModel.SubsidizeGroupClaimList
                udtSubsidizeGroupClaimModelCollection.Add(udtSubsidizeGroupClaimModel)
            Next
        Next

        'filter duplicate 
        Dim dtResult As DataTable = dt.Clone
        Dim aryScheme As New ArrayList
        Dim strSchemeCode As String = "SchemeCode"

        For Each dr As DataRow In dt.Rows
            If aryScheme.Contains(dr(strSchemeCode).ToString.Trim) Then Continue For
            dtResult.ImportRow(dr)
            aryScheme.Add(dr(strSchemeCode).ToString.Trim)
        Next

        Return dtResult

    End Function

#End Region

#Region "View 2 - Search Result"

    '#Region "Gridview Function - gvAcctListR1"

    '    Private Sub gvAcctListR1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvAcctListR1.PageIndexChanging
    '        Me.GridViewPageIndexChangingHandler(sender, e, SESS_Result)
    '    End Sub

    '    Private Sub gvAcctListR1_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvAcctListR1.PreRender
    '        Me.GridViewPreRenderHandler(sender, e, SESS_Result)
    '    End Sub

    '    Private Sub gvAcctListR1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvAcctListR1.RowCommand
    '        If TypeOf e.CommandSource Is LinkButton Then

    '            txtDocCode.Text = String.Empty

    '            Dim strDocCode As String = String.Empty
    '            Dim strAccountID As String = String.Empty
    '            Dim strAccountSource As String = String.Empty
    '            Dim strPersonalInformationStatus As String = String.Empty
    '            Dim strIdentityNum As String = String.Empty
    '            Dim strSPID As String = String.Empty

    '            Dim blnShowAmendmentRecord As Boolean = False

    '            Dim strCommandArgument As String

    '            strCommandArgument = e.CommandArgument.ToString.Trim
    '            strAccountID = strCommandArgument.Split("|")(0).Trim
    '            strDocCode = strCommandArgument.Split("|")(1).Trim
    '            strAccountSource = strCommandArgument.Split("|")(2).Trim
    '            strPersonalInformationStatus = strCommandArgument.Split("|")(3).Trim
    '            strIdentityNum = strCommandArgument.Split("|")(4).Trim
    '            strSPID = strCommandArgument.Split("|")(5).Trim

    '            'Audit Log
    '            Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
    '            Me.udtAuditLogEntry.AddDescripton("AccountID", strAccountID)
    '            Me.udtAuditLogEntry.AddDescripton("DocCode", strDocCode)
    '            Me.udtAuditLogEntry.AddDescripton("AccountSource", strAccountSource)
    '            Me.udtAuditLogEntry.AddDescripton("PersonalInformationStatus", strPersonalInformationStatus)
    '            Me.udtAuditLogEntry.AddDescripton("IdentityNum", strIdentityNum)
    '            Dim udtAuditLogInfo As AuditLogInfo
    '            udtAuditLogInfo = New AuditLogInfo(strSPID, Nothing, strAccountSource, _
    '                                            strAccountID, strDocCode, Me.udtformatter.formatDocumentIdentityNumber(strDocCode, strIdentityNum))
    '            Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00008, AuditLogDesc.SelectEHSAccount, udtAuditLogInfo)

    '            If strAccountSource.Trim.Equals(EHSAccountModel.SysAccountSourceClass.ValidateAccount) Then
    '                If strPersonalInformationStatus.Trim.Equals(EHSAccountModel.PersonalInformationRecordStatusClass.UnderAmendment) Then
    '                    blnShowAmendmentRecord = True
    '                End If
    '            End If

    '            txtDocCode.Text = strDocCode

    '            If Me.GeteHSAcc(strAccountID, strAccountSource, blnShowAmendmentRecord) Then
    '                udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
    '                udtEHSAccount_Amendment = Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FunctionCode)

    '                If blnShowAmendmentRecord Then
    '                    Session(SESS_InputMode) = ActionModel.ReadOnly_N_Amending
    '                Else
    '                    Session(SESS_InputMode) = ActionModel.ReadOnly
    '                End If

    '                Session(SESS_DefaultSetCCCode) = True

    '                Me.mveHSAccount.ActiveViewIndex = intAccountDetails

    '                Me.udtAuditLogEntry.AddDescripton("AccountID", strAccountID)
    '                Me.udtAuditLogEntry.AddDescripton("DocCode", strDocCode)
    '                Me.udtAuditLogEntry.AddDescripton("AccountSource", strAccountSource)
    '                Me.udtAuditLogEntry.AddDescripton("IdentityNum", strIdentityNum)
    '                Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00009, AuditLogDesc.SelectEHSAccountSuccess)
    '            Else
    '                udcMsgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
    '                Me.udtAuditLogEntry.AddDescripton("AccountID", strAccountID)
    '                Me.udtAuditLogEntry.AddDescripton("DocCode", strDocCode)
    '                Me.udtAuditLogEntry.AddDescripton("AccountSource", strAccountSource)
    '                Me.udtAuditLogEntry.AddDescripton("IdentityNum", strIdentityNum)
    '                udcMsgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00010, AuditLogDesc.SelectEHSAccountFail)
    '            End If
    '        End If
    '    End Sub

    '    Private Sub gvAcctListR1_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAcctListR1.RowCreated
    '        Dim udtSortedGVHeaderList As SortedGridviewHeaderModelCollection = New SortedGridviewHeaderModelCollection
    '        Dim strImageURL As String = Me.GetGlobalResourceObject("ImageURL", "infoicon")

    '        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(1, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
    '        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(2, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
    '        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(3, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))

    '        Me.GirdViewRowCreated(sender, e, udtSortedGVHeaderList)
    '    End Sub

    '    Private Sub gvAcctListR1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAcctListR1.RowDataBound
    '        If e.Row.RowType = DataControlRowType.DataRow Then
    '            Dim dr As DataRow = CType(e.Row.DataItem, System.Data.DataRowView).Row
    '            Dim lblCName As Label = CType(e.Row.FindControl("lblCName"), Label)
    '            Dim lbtnIdentityNum As LinkButton = CType(e.Row.FindControl("lbtnIdentityNum"), LinkButton)
    '            Dim lbtnIdentityNumUnmask As LinkButton = CType(e.Row.FindControl("lbtnIdentityNumUnmask"), LinkButton)
    '            Dim lblDOB As Label = CType(e.Row.FindControl("lblDOB"), Label)
    '            Dim lblSex As Label = CType(e.Row.FindControl("lblSex"), Label)
    '            Dim lblCreate_By As Label = CType(e.Row.FindControl("lblCreate_By"), Label)
    '            Dim lblCreate_By_DH As Label = CType(e.Row.FindControl("lblCreate_By_DH"), Label)
    '            Dim lblCreateDtm As Label = CType(e.Row.FindControl("lblCreateDtm"), Label)

    '            Dim strIdentityNum As String = CStr(dr.Item("IdentityNum")).Trim
    '            Dim strVoucherAcctID As String = CStr(dr.Item("Voucher_Acc_ID")).Trim
    '            Dim strSchemeCode As String = CStr(dr.Item("Scheme_Code")).Trim
    '            Dim strChiName As String = CStr(dr.Item("CName")).Trim
    '            Dim dtmDOB As DateTime = CType(dr.Item("DOB"), DateTime)
    '            Dim strExactDOB As String = CStr(dr.Item("Exact_DOB")).Trim
    '            Dim strSex As String = CStr(dr.Item("Sex")).Trim
    '            Dim strCreate_by As String = CStr(dr.Item("Create_By"))
    '            Dim strSPID As String = CStr(dr.Item("SP_ID")).Trim
    '            Dim strSPPracticeDisplaySeq As Integer = CInt(dr.Item("SP_Practice_Display_Seq"))
    '            Dim dtmCreateDtm As DateTime = CType(dr.Item("Create_Dtm"), DateTime)
    '            Dim intAge As Nullable(Of Integer)
    '            Dim dtDOR As Nullable(Of Date)
    '            Dim strDocType As String = CStr(dr.Item("doc_code")).Trim
    '            Dim strAccountSource As String = CStr(dr.Item("Source")).Trim
    '            Dim strAdoptionPrefixNum As String = CStr(dr.Item("Adoption_Prefix_Num")).Trim
    '            Dim strOtherInfo As String

    '            Dim strAccountStatus As String = CStr(dr.Item("Account_Status")).Trim
    '            Dim strPersonalInformationStatus As String = CStr(dr.Item("PersonalInformation_Status")).Trim

    '            If IsDBNull(dr.Item("EC_Age")) Then
    '                intAge = Nothing
    '            Else
    '                intAge = CInt(dr.Item("EC_Age"))
    '            End If

    '            If IsDBNull(dr.Item("EC_Date_of_Registration")) Then
    '                dtDOR = Nothing
    '            Else
    '                dtDOR = CType(dr.Item("EC_Date_of_Registration"), Date)
    '            End If

    '            If IsDBNull(dr.Item("other_info")) Then
    '                strOtherInfo = String.Empty
    '            Else
    '                strOtherInfo = CStr(dr.Item("other_info"))
    '            End If

    '            lbtnIdentityNum.Text = udtformatter.FormatDocIdentityNoForDisplay(strDocType, strIdentityNum, True, strAdoptionPrefixNum)
    '            lbtnIdentityNumUnmask.Text = udtformatter.FormatDocIdentityNoForDisplay(strDocType, strIdentityNum, False, strAdoptionPrefixNum)
    '            lbtnIdentityNum.CommandArgument = strVoucherAcctID & "|" & strDocType & "|" & strAccountSource & "|" & strPersonalInformationStatus & "|" & strIdentityNum & "|" & strSPID
    '            lbtnIdentityNumUnmask.CommandArgument = lbtnIdentityNum.CommandArgument

    '            lblCName.Text = udtformatter.formatChineseName(strChiName.Trim)

    '            lblDOB.Text = udtformatter.formatDOB(strDocType, dtmDOB, strExactDOB, Session(SESS_Language), intAge, dtDOR, strOtherInfo)

    '            lblSex.Text = Me.GetGlobalResourceObject("Text", udtformatter.formatGender(strSex))

    '            lblCreateDtm.Text = udtformatter.formatDateTime(dtmCreateDtm)

    '            'If IsDBNull(dr.Item("Create_By_BO")) Then
    '            '    If strSPID.Trim.Equals(String.Empty) Then
    '            '        lblCreate_By.Text = CStr(dr.Item("Create_By")).Trim
    '            '        lblCreate_By_DH.Text = "<br>(Created by DH)"
    '            '    Else
    '            '        lblCreate_By.Text = strSPID + "(" + strSPPracticeDisplaySeq.ToString().Trim + ")"
    '            '    End If
    '            'Else
    '            '    If CStr(dr.Item("Create_By_BO")).Trim = "Y" Then
    '            '        If strSPID.Trim.Equals(String.Empty) Then
    '            '            lblCreate_By.Text = CStr(dr.Item("Create_By")).Trim
    '            '            lblCreate_By_DH.Text = "<br>(Created by DH)"
    '            '        Else
    '            '            lblCreate_By.Text = strSPID + "(" + strSPPracticeDisplaySeq.ToString().Trim + ")"
    '            '            lblCreate_By_DH.Text = "<br>(Created by " + CStr(dr.Item("Create_By")).Trim + ")"
    '            '        End If
    '            '    Else
    '            '        lblCreate_By.Text = strSPID + "(" + strSPPracticeDisplaySeq.ToString().Trim + ")"
    '            '    End If
    '            'End If

    '            If Not IsDBNull(dr.Item("Create_By_BO")) Then
    '                'has value
    '                If CStr(dr.Item("Create_By_BO")).Trim = "Y" Then
    '                    lblCreate_By.Text = CStr(dr.Item("Create_By")).Trim
    '                Else
    '                    lblCreate_By.Text = strSPID + "(" + strSPPracticeDisplaySeq.ToString().Trim + ")"
    '                End If
    '            Else
    '                lblCreate_By.Text = strSPID + "(" + strSPPracticeDisplaySeq.ToString().Trim + ")"
    '            End If

    '            dr.Item("Create_By") = lblCreate_By.Text


    '        End If

    '    End Sub

    '    Private Sub gvAcctListR1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvAcctListR1.Sorting
    '        Me.GridViewSortingHandler(sender, e, SESS_Result)
    '    End Sub

    '#End Region

#Region "Gridview Function - gvAcctListR2"

    Private Sub gvAcctListR2_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvAcctListR2.PageIndexChanging
        Me.GridViewPageIndexChangingHandler(sender, e, SESS_Result)
    End Sub

    Private Sub gvAcctListR2_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvAcctListR2.PreRender
        Me.GridViewPreRenderHandler(sender, e, SESS_Result)
    End Sub

    Private Sub gvAcctListR2_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvAcctListR2.RowCommand
        If TypeOf e.CommandSource Is LinkButton Then

            txtDocCode.Text = String.Empty

            Dim strDocCode As String = String.Empty
            Dim strAccountID As String = String.Empty
            Dim strAccountSource As String = String.Empty
            Dim strPersonalInformationStatus As String = String.Empty
            Dim strIdentityNum As String = String.Empty
            Dim strSPID As String = String.Empty

            Dim blnShowAmendmentRecord As Boolean = False


            Dim strCommandArgument As String

            strCommandArgument = e.CommandArgument.ToString.Trim
            strAccountID = strCommandArgument.Split("|")(0).Trim
            strDocCode = strCommandArgument.Split("|")(1).Trim
            strAccountSource = strCommandArgument.Split("|")(2).Trim
            strPersonalInformationStatus = strCommandArgument.Split("|")(3).Trim
            strIdentityNum = strCommandArgument.Split("|")(4).Trim
            strSPID = strCommandArgument.Split("|")(5).Trim()

            'Audit Log
            Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
            Me.udtAuditLogEntry.AddDescripton("AccountID", strAccountID)
            Me.udtAuditLogEntry.AddDescripton("DocCode", strDocCode)
            Me.udtAuditLogEntry.AddDescripton("AccountSource", strAccountSource)
            Me.udtAuditLogEntry.AddDescripton("PersonalInformationStatus", strPersonalInformationStatus)
            Me.udtAuditLogEntry.AddDescripton("IdentityNum", strIdentityNum)
            Dim udtAuditLogInfo As AuditLogInfo
            udtAuditLogInfo = New AuditLogInfo(strSPID, Nothing, strAccountSource, _
                                            strAccountID, strDocCode, Me.udtformatter.formatDocumentIdentityNumber(strDocCode, strIdentityNum))
            Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00008, AuditLogDesc.SelectEHSAccount, udtAuditLogInfo)

            txtDocCode.Text = strDocCode

            If strAccountSource.Trim.Equals(EHSAccountModel.SysAccountSourceClass.ValidateAccount) Then
                If strPersonalInformationStatus.Trim.Equals(EHSAccountModel.PersonalInformationRecordStatusClass.UnderAmendment) Then
                    blnShowAmendmentRecord = True
                End If
            End If

            If Me.GeteHSAcc(strAccountID, strAccountSource, blnShowAmendmentRecord) Then
                udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
                Me.udtEHSAccount_Amendment = Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FunctionCode)

                If blnShowAmendmentRecord Then
                    Session(SESS_InputMode) = ActionModel.ReadOnly_N_Amending
                Else
                    Session(SESS_InputMode) = ActionModel.ReadOnly
                End If

                Session(SESS_DefaultSetCCCode) = True

                Me.mveHSAccount.ActiveViewIndex = intAccountDetails

                ibtnAccountDetailsBack.Visible = True

                Me.udtAuditLogEntry.AddDescripton("AccountID", strAccountID)
                Me.udtAuditLogEntry.AddDescripton("DocCode", strDocCode)
                Me.udtAuditLogEntry.AddDescripton("AccountSource", strAccountSource)
                Me.udtAuditLogEntry.AddDescripton("IdentityNum", strIdentityNum)
                Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00009, AuditLogDesc.SelectEHSAccountSuccess)
                udcMsgBox.Clear()
            Else
                udcMsgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
                Me.udtAuditLogEntry.AddDescripton("AccountID", strAccountID)
                Me.udtAuditLogEntry.AddDescripton("DocCode", strDocCode)
                Me.udtAuditLogEntry.AddDescripton("AccountSource", strAccountSource)
                Me.udtAuditLogEntry.AddDescripton("IdentityNum", strIdentityNum)
                udcMsgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00010, AuditLogDesc.SelectEHSAccountFail)
            End If
        End If
    End Sub

    Private Sub gvAcctListR2_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAcctListR2.RowCreated
        Dim udtSortedGVHeaderList As SortedGridviewHeaderModelCollection = New SortedGridviewHeaderModelCollection
        Dim strImageURL As String = Me.GetGlobalResourceObject("ImageURL", "infoicon")

        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(2, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(3, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(4, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))

        Me.GirdViewRowCreated(sender, e, udtSortedGVHeaderList)
    End Sub

    Private Sub gvAcctListR2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAcctListR2.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            DirectCast(e.Row.FindControl("HeaderLevelCheckBox"), CheckBox).Attributes.Add("onclick", "javascript:SelectAll('" & _
            CType(sender, GridView).ClientID & "','" & _
            DirectCast(e.Row.FindControl("HeaderLevelCheckBox"), CheckBox).ClientID & "')")
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRow = CType(e.Row.DataItem, System.Data.DataRowView).Row
            Dim chkSelect As CheckBox
            Dim lbtnAccountID As LinkButton
            Dim lblIdentityNum As Label
            Dim lblIdentityNumUnmask As Label
            Dim lblCreateDtm As Label
            Dim lblName As Label
            Dim lblCName As Label
            Dim lblDOB As Label
            Dim lblSex As Label
            Dim lblAccountType As Label
            Dim lblAccountStatus As Label
            Dim lblEnquiryStatus As Label
            'Dim lblDateOfIssue As Label
            Dim lblAmendmentStatus As Label
            Dim lblCreate_By As Label
            Dim lblCreate_By_DH As Label

            chkSelect = e.Row.FindControl("chkSelect")
            lbtnAccountID = CType(e.Row.FindControl("lbtnAccountID"), LinkButton) ' CRE11-007
            lblIdentityNum = CType(e.Row.FindControl("lblIdentityNum"), Label) ' CRE11-007
            lblIdentityNumUnmask = CType(e.Row.FindControl("lblIdentityNumUnmask"), Label) ' CRE11-007
            lblCreateDtm = CType(e.Row.FindControl("lblCreateDtm"), Label)
            lblName = CType(e.Row.FindControl("lblName"), Label)
            lblCName = CType(e.Row.FindControl("lblCName"), Label)
            lblDOB = CType(e.Row.FindControl("lblDOB"), Label)
            lblSex = CType(e.Row.FindControl("lblSex"), Label)
            'lblDateOfIssue = CType(e.Row.FindControl("lblDateOfIssue"), Label)
            lblAccountType = CType(e.Row.FindControl("lblAccountType"), Label)
            lblAccountStatus = CType(e.Row.FindControl("lblAccountStatus"), Label)
            lblEnquiryStatus = CType(e.Row.FindControl("lblEnquiryStatus"), Label)
            lblAmendmentStatus = CType(e.Row.FindControl("lblAmendmentStatus"), Label)
            lblCreate_By = CType(e.Row.FindControl("lblCreate_By"), Label)
            lblCreate_By_DH = CType(e.Row.FindControl("lblCreate_By_DH"), Label)

            Dim dtmCreateDtm As DateTime = CType(dr.Item("Create_Dtm"), DateTime)
            Dim strEngName As String = CStr(dr.Item("Eng_Name"))
            Dim strChiName As String = CStr(dr.Item("Chi_Name"))
            Dim strIdentityNum As String = CStr(dr.Item("IdentityNum"))
            Dim strVoucherAcctID As String = CStr(dr.Item("Voucher_Acc_ID"))
            Dim strSchemeCode As String = CStr(dr.Item("Scheme_Code"))
            Dim dtmDOB As DateTime = CType(dr.Item("DOB"), DateTime)
            Dim strExactDOB As String = CStr(dr.Item("Exact_DOB"))
            Dim strSex As String = CStr(dr.Item("Sex"))
            'Dim dtmDateOfIssue As Nullable(Of Date)
            Dim strAccountSource As String = CStr(dr.Item("Source"))
            Dim strAccountStatus As String = CStr(dr.Item("Account_Status"))
            Dim strEnquiryStatus As String = CStr(dr.Item("Public_Enquiry_Status"))
            Dim strAmendmentStatus As String = CStr(dr.Item("PersonalInformation_Status"))
            Dim strSPID As String = CStr(dr.Item("SP_ID"))
            Dim strCreate_by As String = CStr(dr.Item("Create_By"))
            Dim strSPPracticeDisplaySeq As Integer = CInt(dr.Item("SP_Practice_Display_Seq"))
            Dim intAge As Nullable(Of Integer)
            Dim dtDOR As Nullable(Of Date)
            Dim strAcctTypeCode As String = String.Empty
            Dim strAcctStatusCode As String = String.Empty
            Dim strAdoptionPrefixNum As String = CStr(dr.Item("Adoption_Prefix_Num")).Trim
            Dim strDocCode As String = CStr(dr.Item("Doc_Code")).Trim
            Dim strAccountPurpose As String = CStr(dr.Item("Account_Purpose")).Trim
            Dim strOtherInfo As String

            'If IsDBNull(dr.Item("Date_of_Issue")) Then
            '    dtmDateOfIssue = Nothing
            'Else
            '    dtmDateOfIssue = CType(dr.Item("Date_of_Issue"), DateTime)
            'End If

            If IsDBNull(dr.Item("EC_Age")) Then
                intAge = Nothing
            Else
                intAge = CInt(dr.Item("EC_Age"))
            End If

            If IsDBNull(dr.Item("EC_Date_of_Registration")) Then
                dtDOR = Nothing
            Else
                dtDOR = CType(dr.Item("EC_Date_of_Registration"), Date)
            End If

            'If IsDBNull(dr.Item("Create_By_BO")) Then
            '    If strSPID.Trim.Equals(String.Empty) Then
            '        lblCreate_By.Text = CStr(dr.Item("Create_By")).Trim
            '        lblCreate_By_DH.Text = "<br>(Created by DH)"
            '    Else
            '        lblCreate_By.Text = strSPID + "(" + strSPPracticeDisplaySeq.ToString().Trim + ")"
            '    End If
            'Else
            '    If CStr(dr.Item("Create_By_BO")).Trim = "Y" Then
            '        If strSPID.Trim.Equals(String.Empty) Then
            '            lblCreate_By.Text = CStr(dr.Item("Create_By")).Trim
            '            lblCreate_By_DH.Text = "<br>(Created by DH)"
            '        Else
            '            lblCreate_By.Text = strSPID + "(" + strSPPracticeDisplaySeq.ToString().Trim + ")"
            '            lblCreate_By_DH.Text = "<br>(Created by " + CStr(dr.Item("Create_By")).Trim + ")"
            '        End If
            '    Else
            '        lblCreate_By.Text = strSPID + "(" + strSPPracticeDisplaySeq.ToString().Trim + ")"
            '    End If
            'End If

            If Not IsDBNull(dr.Item("Create_By_BO")) Then
                'has value
                If CStr(dr.Item("Create_By_BO")).Trim = "Y" Then
                    lblCreate_By.Text = CStr(dr.Item("Create_By")).Trim
                Else
                    lblCreate_By.Text = strSPID + "(" + strSPPracticeDisplaySeq.ToString().Trim + ")"
                End If
            Else
                lblCreate_By.Text = strSPID + "(" + strSPPracticeDisplaySeq.ToString().Trim + ")"
            End If

            If IsDBNull(dr.Item("other_info")) Then
                strOtherInfo = String.Empty
            Else
                strOtherInfo = CStr(dr.Item("other_info"))
            End If


            lblIdentityNum.Text = udtformatter.FormatDocIdentityNoForDisplay(strDocCode, strIdentityNum, True, strAdoptionPrefixNum)
            lblIdentityNumUnmask.Text = udtformatter.FormatDocIdentityNoForDisplay(strDocCode, strIdentityNum, False, strAdoptionPrefixNum)
            lbtnAccountID.CommandArgument = strVoucherAcctID & "|" & strDocCode & "|" & strAccountSource & "|" & strAmendmentStatus & "|" & strIdentityNum & "|" & strSPID

            If strAccountSource.Trim = AccountTypeClass.Validated Then
                lbtnAccountID.Text = udtformatter.formatValidatedEHSAccountNumber(strVoucherAcctID.Trim)
                ' CRE11-007 : Checkbox
                chkSelect.Enabled = True
            Else
                lbtnAccountID.Text = udtformatter.formatSystemNumber(strVoucherAcctID.Trim)
                ' CRE11-007 : Checkbox
                chkSelect.Enabled = False
            End If


            lblCreateDtm.Text = udtformatter.formatDateTime(dtmCreateDtm)
            lblName.Text = strEngName
            lblCName.Text = udtformatter.formatChineseName(strChiName.Trim)
            lblDOB.Text = udtformatter.formatDOB(strDocCode, dtmDOB, strExactDOB, Session(SESS_Language), intAge, dtDOR, strOtherInfo)



            'lblDateOfIssue.Text = udtformatter.formatDOI(strDocCode, dtmDateOfIssue)
            'lblDateOfIssue.Text = udtformatter.formatDOI_GV(dtmDateOfIssue)
            'If lblDateOfIssue.Text.Trim.Equals(String.Empty) Then
            '    lblDateOfIssue.Text = Me.GetGlobalResourceObject("Text", "N/A")
            'End If

            lblSex.Text = Me.GetGlobalResourceObject("Text", udtformatter.formatGender(strSex))

            If strAccountSource.Trim.Equals(EHSAccountModel.SysAccountSourceClass.ValidateAccount) Then
                If strAmendmentStatus.Trim.Equals(EHSAccountModel.PersonalInformationRecordStatusClass.UnderAmendment) Then
                    lblAmendmentStatus.Text = "Under Modification"
                Else
                    lblAmendmentStatus.Text = Me.GetGlobalResourceObject("Text", "N/A")
                End If
                Status.GetDescriptionFromDBCode(EHSAccountModel.EnquiryStatusClass.ClassCode, strEnquiryStatus, lblEnquiryStatus.Text, String.Empty)
            Else
                lblAmendmentStatus.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lblEnquiryStatus.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            Status.GetDescriptionFromDBCode(EHSAccountModel.SysAccountSourceClass.ClassCode, strAccountSource, lblAccountType.Text, String.Empty)

            If strAccountSource.Trim.Equals(EHSAccountModel.SysAccountSourceClass.TemporaryAccount) Then
                If strAccountPurpose.Trim.Equals(EHSAccountModel.AccountPurposeClass.ForAmendmentOld) Then
                    'CRE13-006 HCVS Ceiling [Start][Karl]
                    'lblAccountType.Text = "Erased"
                    lblAccountType.Text = eHealthAccountStatus.Erased_Desc
                    'CRE13-006 HCVS Ceiling [End][Karl]
                End If
            End If

            lblAccountStatus.Text = Me.udteHSAccountMaintBLL.getAcctStatus(strAccountStatus, strAccountSource)
        End If
    End Sub

    Private Sub gvAcctListR2_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvAcctListR2.Sorting
        Me.GridViewSortingHandler(sender, e, SESS_Result)
    End Sub

#End Region


#Region "Gridview Function - gvAcctListR3"
    Private Sub gvAcctListR3_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvAcctListR3.PageIndexChanging
        Me.GridViewPageIndexChangingHandler(sender, e, SESS_Result)
    End Sub

    Private Sub gvAcctListR3_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvAcctListR3.PreRender
        Me.GridViewPreRenderHandler(sender, e, SESS_Result)
    End Sub

    Private Sub gvAcctListR3_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvAcctListR3.RowCommand
        If TypeOf e.CommandSource Is LinkButton Then

            txtDocCode.Text = String.Empty

            Dim strDocCode As String = String.Empty
            Dim strAccountID As String = String.Empty
            Dim strAccountSource As String = String.Empty
            'Dim strPersonalInformationStatus As String = String.Empty
            Dim strIdentityNum As String = String.Empty
            Dim strSPID As String = String.Empty

            Dim blnShowAmendmentRecord As Boolean = False


            Dim strCommandArgument As String

            strCommandArgument = e.CommandArgument.ToString.Trim
            strAccountID = strCommandArgument.Split("|")(0).Trim
            strDocCode = strCommandArgument.Split("|")(1).Trim
            strAccountSource = strCommandArgument.Split("|")(2).Trim
            'strPersonalInformationStatus = strCommandArgument.Split("|")(3).Trim
            strIdentityNum = strCommandArgument.Split("|")(3).Trim
            strSPID = strCommandArgument.Split("|")(4).Trim()

            'Audit Log
            Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
            Me.udtAuditLogEntry.AddDescripton("AccountID", strAccountID)
            Me.udtAuditLogEntry.AddDescripton("DocCode", strDocCode)
            Me.udtAuditLogEntry.AddDescripton("AccountSource", strAccountSource)
            'Me.udtAuditLogEntry.AddDescripton("PersonalInformationStatus", strPersonalInformationStatus)
            Me.udtAuditLogEntry.AddDescripton("IdentityNum", strIdentityNum)
            Dim udtAuditLogInfo As AuditLogInfo
            udtAuditLogInfo = New AuditLogInfo(strSPID, Nothing, strAccountSource, _
                                            strAccountID, strDocCode, Me.udtformatter.formatDocumentIdentityNumber(strDocCode, strIdentityNum))
            Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00008, AuditLogDesc.SelectEHSAccount, udtAuditLogInfo)

            txtDocCode.Text = strDocCode

            'If strAccountSource.Trim.Equals(EHSAccountModel.SysAccountSourceClass.ValidateAccount) Then
            '    If strPersonalInformationStatus.Trim.Equals(EHSAccountModel.PersonalInformationRecordStatusClass.UnderAmendment) Then
            '        blnShowAmendmentRecord = True
            '    End If
            'End If

            If Me.GeteHSAcc(strAccountID, strAccountSource, blnShowAmendmentRecord) Then
                udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
                Me.udtEHSAccount_Amendment = Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FunctionCode)

                If blnShowAmendmentRecord Then
                    Session(SESS_InputMode) = ActionModel.ReadOnly_N_Amending
                Else
                    Session(SESS_InputMode) = ActionModel.ReadOnly
                End If

                Session(SESS_DefaultSetCCCode) = True

                Me.mveHSAccount.ActiveViewIndex = intAccountDetails

                ibtnAccountDetailsBack.Visible = True

                Me.udtAuditLogEntry.AddDescripton("AccountID", strAccountID)
                Me.udtAuditLogEntry.AddDescripton("DocCode", strDocCode)
                Me.udtAuditLogEntry.AddDescripton("AccountSource", strAccountSource)
                Me.udtAuditLogEntry.AddDescripton("IdentityNum", strIdentityNum)
                Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00009, AuditLogDesc.SelectEHSAccountSuccess)
                udcMsgBox.Clear()
            Else
                udcMsgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
                Me.udtAuditLogEntry.AddDescripton("AccountID", strAccountID)
                Me.udtAuditLogEntry.AddDescripton("DocCode", strDocCode)
                Me.udtAuditLogEntry.AddDescripton("AccountSource", strAccountSource)
                Me.udtAuditLogEntry.AddDescripton("IdentityNum", strIdentityNum)
                udcMsgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00010, AuditLogDesc.SelectEHSAccountFail)
            End If
        End If
    End Sub

    Private Sub gvAcctListR3_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAcctListR3.RowCreated
        Dim udtSortedGVHeaderList As SortedGridviewHeaderModelCollection = New SortedGridviewHeaderModelCollection
        Dim strImageURL As String = Me.GetGlobalResourceObject("ImageURL", "infoicon")

        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(2, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(3, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(4, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))

        Me.GirdViewRowCreated(sender, e, udtSortedGVHeaderList)
    End Sub

    Private Sub gvAcctListR3_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAcctListR3.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            DirectCast(e.Row.FindControl("HeaderLevelCheckBox"), CheckBox).Attributes.Add("onclick", "javascript:SelectAll('" & _
            CType(sender, GridView).ClientID & "','" & _
            DirectCast(e.Row.FindControl("HeaderLevelCheckBox"), CheckBox).ClientID & "')")
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRow = CType(e.Row.DataItem, System.Data.DataRowView).Row
            Dim chkSelect As CheckBox
            Dim lbtnAccountID As LinkButton
            Dim lblIdentityNum As Label
            Dim lblIdentityNumUnmask As Label
            Dim lblCreateDtm As Label
            Dim lblName As Label
            Dim lblCName As Label
            Dim lblDOB As Label
            Dim lblSex As Label
            'Dim lblAccountType As Label
            'Dim lblAccountStatus As Label
            'Dim lblEnquiryStatus As Label
            'Dim lblDateOfIssue As Label
            'Dim lblAmendmentStatus As Label
            Dim lblCreate_By As Label
            Dim lblCreate_By_DH As Label
            Dim lblManualValidationStatus As Label
            'Dim lblScheme As Label
            'Dim lblDeceased As Label
            'Dim lblDateofDeath As Label

            chkSelect = e.Row.FindControl("chkSelect")
            lbtnAccountID = CType(e.Row.FindControl("lbtnAccountID"), LinkButton) ' CRE11-007
            lblIdentityNum = CType(e.Row.FindControl("lblIdentityNum"), Label) ' CRE11-007
            lblIdentityNumUnmask = CType(e.Row.FindControl("lblIdentityNumUnmask"), Label) ' CRE11-007
            lblCreateDtm = CType(e.Row.FindControl("lblCreateDtm"), Label)
            lblName = CType(e.Row.FindControl("lblName"), Label)
            lblCName = CType(e.Row.FindControl("lblCName"), Label)
            lblDOB = CType(e.Row.FindControl("lblDOB"), Label)
            lblSex = CType(e.Row.FindControl("lblSex"), Label)
            'lblDateOfIssue = CType(e.Row.FindControl("lblDateOfIssue"), Label)
            'lblAccountType = CType(e.Row.FindControl("lblAccountType"), Label)
            'lblAccountStatus = CType(e.Row.FindControl("lblAccountStatus"), Label)
            'lblEnquiryStatus = CType(e.Row.FindControl("lblEnquiryStatus"), Label)
            'lblAmendmentStatus = CType(e.Row.FindControl("lblAmendmentStatus"), Label)
            lblCreate_By = CType(e.Row.FindControl("lblCreate_By"), Label)
            lblCreate_By_DH = CType(e.Row.FindControl("lblCreate_By_DH"), Label)
            lblManualValidationStatus = CType(e.Row.FindControl("lblManualValidationStatus"), Label)
            'lblScheme = CType(e.Row.FindControl("lblScheme"), Label)
            'lblDeceased = CType(e.Row.FindControl("lblDeceased"), Label)
            'lblDateofDeath = CType(e.Row.FindControl("lblDateOfDeath"), Label)

            Dim dtmCreateDtm As DateTime = CType(dr.Item("Create_Dtm"), DateTime)
            Dim strEngName As String = CStr(dr.Item("Eng_Name"))
            Dim strChiName As String = CStr(dr.Item("Chi_Name"))
            Dim strIdentityNum As String = CStr(dr.Item("IdentityNum"))
            Dim strVoucherAcctID As String = CStr(dr.Item("Voucher_Acc_ID"))
            Dim strSchemeCode As String = CStr(dr.Item("Scheme_Code"))
            Dim dtmDOB As DateTime = CType(dr.Item("DOB"), DateTime)
            Dim strExactDOB As String = CStr(dr.Item("Exact_DOB"))
            Dim strSex As String = CStr(dr.Item("Sex"))
            'Dim dtmDateOfIssue As Nullable(Of Date)
            Dim strAccountSource As String = CStr(dr.Item("Source"))
            'Dim strAccountStatus As String = CStr(dr.Item("Account_Status"))
            'Dim strEnquiryStatus As String = CStr(dr.Item("Public_Enquiry_Status"))
            'Dim strAmendmentStatus As String = CStr(dr.Item("PersonalInformation_Status"))
            Dim strSPID As String = CStr(dr.Item("SP_ID"))
            Dim strCreate_by As String = CStr(dr.Item("Create_By"))
            Dim strSPPracticeDisplaySeq As Integer = CInt(dr.Item("SP_Practice_Display_Seq"))
            Dim intAge As Nullable(Of Integer)
            Dim dtDOR As Nullable(Of Date)
            'Dim strAcctTypeCode As String = String.Empty
            'Dim strAcctStatusCode As String = String.Empty
            Dim strAdoptionPrefixNum As String = CStr(dr.Item("Adoption_Prefix_Num")).Trim
            Dim strDocCode As String = CStr(dr.Item("Doc_Code")).Trim
            Dim strAccountPurpose As String = CStr(dr.Item("Account_Purpose")).Trim
            Dim strOtherInfo As String
            Dim strManualValidationStatus As String = CStr(dr.Item("ManualValidationStatus"))
            'Dim strScheme As String
            'Dim strDeceased As String = CStr(dr.Item("Deceased"))
            'Dim dtmDOD As DateTime = CType(dr.Item("DOD"), DateTime)

            'If IsDBNull(dr.Item("Date_of_Issue")) Then
            '    dtmDateOfIssue = Nothing
            'Else
            '    dtmDateOfIssue = CType(dr.Item("Date_of_Issue"), DateTime)
            'End If

            If IsDBNull(dr.Item("EC_Age")) Then
                intAge = Nothing
            Else
                intAge = CInt(dr.Item("EC_Age"))
            End If

            If IsDBNull(dr.Item("EC_Date_of_Registration")) Then
                dtDOR = Nothing
            Else
                dtDOR = CType(dr.Item("EC_Date_of_Registration"), Date)
            End If

            'If IsDBNull(dr.Item("Create_By_BO")) Then
            '    If strSPID.Trim.Equals(String.Empty) Then
            '        lblCreate_By.Text = CStr(dr.Item("Create_By")).Trim
            '        lblCreate_By_DH.Text = "<br>(Created by DH)"
            '    Else
            '        lblCreate_By.Text = strSPID + "(" + strSPPracticeDisplaySeq.ToString().Trim + ")"
            '    End If
            'Else
            '    If CStr(dr.Item("Create_By_BO")).Trim = "Y" Then
            '        If strSPID.Trim.Equals(String.Empty) Then
            '            lblCreate_By.Text = CStr(dr.Item("Create_By")).Trim
            '            lblCreate_By_DH.Text = "<br>(Created by DH)"
            '        Else
            '            lblCreate_By.Text = strSPID + "(" + strSPPracticeDisplaySeq.ToString().Trim + ")"
            '            lblCreate_By_DH.Text = "<br>(Created by " + CStr(dr.Item("Create_By")).Trim + ")"
            '        End If
            '    Else
            '        lblCreate_By.Text = strSPID + "(" + strSPPracticeDisplaySeq.ToString().Trim + ")"
            '    End If
            'End If

            If Not IsDBNull(dr.Item("Create_By_BO")) Then
                'has value
                If CStr(dr.Item("Create_By_BO")).Trim = "Y" Then
                    lblCreate_By.Text = CStr(dr.Item("Create_By")).Trim
                Else
                    lblCreate_By.Text = strSPID + "(" + strSPPracticeDisplaySeq.ToString().Trim + ")"
                End If
            Else
                lblCreate_By.Text = strSPID + "(" + strSPPracticeDisplaySeq.ToString().Trim + ")"
            End If

            If IsDBNull(dr.Item("other_info")) Then
                strOtherInfo = String.Empty
            Else
                strOtherInfo = CStr(dr.Item("other_info"))
            End If


            lblIdentityNum.Text = udtformatter.FormatDocIdentityNoForDisplay(strDocCode, strIdentityNum, True, strAdoptionPrefixNum)
            lblIdentityNumUnmask.Text = udtformatter.FormatDocIdentityNoForDisplay(strDocCode, strIdentityNum, False, strAdoptionPrefixNum)
            'lbtnAccountID.CommandArgument = strVoucherAcctID & "|" & strDocCode & "|" & strAccountSource & "|" & strAmendmentStatus & "|" & strIdentityNum & "|" & strSPID
            lbtnAccountID.CommandArgument = strVoucherAcctID & "|" & strDocCode & "|" & strAccountSource & "|" & strIdentityNum & "|" & strSPID

            If strAccountSource.Trim = AccountTypeClass.Validated Then
                lbtnAccountID.Text = udtformatter.formatValidatedEHSAccountNumber(strVoucherAcctID.Trim)
                ' CRE11-007 : Checkbox
                chkSelect.Enabled = True
            Else
                lbtnAccountID.Text = udtformatter.formatSystemNumber(strVoucherAcctID.Trim)
                ' CRE11-007 : Checkbox
                chkSelect.Enabled = False
            End If


            lblCreateDtm.Text = udtformatter.formatDateTime(dtmCreateDtm)
            lblName.Text = strEngName
            lblCName.Text = udtformatter.formatChineseName(strChiName.Trim)
            lblDOB.Text = udtformatter.formatDOB(strDocCode, dtmDOB, strExactDOB, Session(SESS_Language), intAge, dtDOR, strOtherInfo)



            'lblDateOfIssue.Text = udtformatter.formatDOI(strDocCode, dtmDateOfIssue)
            'lblDateOfIssue.Text = udtformatter.formatDOI_GV(dtmDateOfIssue)
            'If lblDateOfIssue.Text.Trim.Equals(String.Empty) Then
            '    lblDateOfIssue.Text = Me.GetGlobalResourceObject("Text", "N/A")
            'End If

            lblSex.Text = Me.GetGlobalResourceObject("Text", udtformatter.formatGender(strSex))

            'If strAccountSource.Trim.Equals(EHSAccountModel.SysAccountSourceClass.ValidateAccount) Then
            '    If strAmendmentStatus.Trim.Equals(EHSAccountModel.PersonalInformationRecordStatusClass.UnderAmendment) Then
            '        lblAmendmentStatus.Text = "Under Modification"
            '    Else
            '        lblAmendmentStatus.Text = Me.GetGlobalResourceObject("Text", "N/A")
            '    End If
            '    Status.GetDescriptionFromDBCode(EHSAccountModel.EnquiryStatusClass.ClassCode, strEnquiryStatus, lblEnquiryStatus.Text, String.Empty)
            'Else
            '    lblAmendmentStatus.Text = Me.GetGlobalResourceObject("Text", "N/A")
            '    lblEnquiryStatus.Text = Me.GetGlobalResourceObject("Text", "N/A")
            'End If

            'Status.GetDescriptionFromDBCode(EHSAccountModel.SysAccountSourceClass.ClassCode, strAccountSource, lblAccountType.Text, String.Empty)

            'If strAccountSource.Trim.Equals(EHSAccountModel.SysAccountSourceClass.TemporaryAccount) Then
            '    If strAccountPurpose.Trim.Equals(EHSAccountModel.AccountPurposeClass.ForAmendmentOld) Then
            '        'CRE13-006 HCVS Ceiling [Start][Karl]
            '        'lblAccountType.Text = "Erased"
            '        lblAccountType.Text = eHealthAccountStatus.Erased_Desc
            '        'CRE13-006 HCVS Ceiling [End][Karl]
            '    End If
            'End If

            'lblAccountStatus.Text = Me.udteHSAccountMaintBLL.getAcctStatus(strAccountStatus, strAccountSource)

            'Manual Validation
            Status.GetDescriptionFromDBCode(TempAcctMaintenanceStatusByManualValidation.ClassCode, strManualValidationStatus, lblManualValidationStatus.Text, String.Empty)

            ''Scheme_claim
            'If IsDBNull(dr.Item("Scheme_Claim")) Then
            '    lblScheme.Text = String.Empty
            'Else
            '    lblScheme.Text = CStr(dr.Item("Scheme_Claim")).Trim
            'End If

            ''Deceased
            'If strDeceased = YesNo.Yes Then
            '    lblDeceased.Text = Me.GetGlobalResourceObject("Text", "Yes")
            'ElseIf strDeceased = YesNo.No Then
            '    lblDeceased.Text = Me.GetGlobalResourceObject("Text", "No")
            'End If

            ''Date of Death
            'If IsDBNull(dr.Item("DOD")) Then
            '    lblDateofDeath.Text = String.Empty
            'Else
            '    lblDateofDeath.Text = udtformatter.formatDateTime(CType(dr.Item("DOD"), DateTime))
            'End If

        End If
    End Sub

    Private Sub gvAcctListR3_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvAcctListR3.Sorting
        Me.GridViewSortingHandler(sender, e, SESS_Result)
    End Sub
#End Region

    Protected Sub ibtnSearchResultBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'audit log
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00078, AuditLogDesc.BackToSearch)

        'Me.ResetControls()

        Me.mveHSAccount.ActiveViewIndex = intSearchView
    End Sub

#End Region

#Region "View 3 - Account Details"

    Protected Sub ibtnAccountDetailsBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'audit log
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00079, AuditLogDesc.BackToResultList)

        Me.mveHSAccount.ActiveViewIndex = intSearchResult
        Me.ucInputDocumentType.Clear()
        udteHSAccountMaintBLL.EHSAccountRemoveFromSession(FunctionCode)
    End Sub

    Protected Sub ibtnAmendHistory_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udcInfoMsgBox.Clear()

        udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountType", udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00011, AuditLogDesc.GetAmendmentHistory)

        Dim udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel
        udtEHSPersonalInfo = udtEHSAccount.getPersonalInformation(Me.txtDocCode.Text.Trim)

        'Me.lblAmendmentHistoryDocType.Text = udtDocTypeBLL.getAllDocType.Filter(Me.txtDocCode.Text.Trim).DocName
        'Me.lblAmendmentHistoryIdentityNum.Text = udtformatter.FormatDocIdentityNoForDisplay(Me.txtDocCode.Text.Trim, udtEHSPersonalInfo.IdentityNum, False, udtEHSPersonalInfo.AdoptionPrefixNum)

        Dim dt As DataTable

        Try
            dt = Me.udteHSAccountMaintBLL.GetAmendmentHistory(udtEHSAccount.VoucherAccID, Me.txtDocCode.Text.Trim)

            If dt.Rows.Count = 0 Then
                udtSM = New Common.ComObject.SystemMessage(CommonFunctionCode, SeverityCode.SEVI, MsgCode.MSG00001)
                Me.udcInfoMsgBox.AddMessage(udtSM)
                Me.udcInfoMsgBox.BuildMessageBox()
                Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Information

                Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                Me.udtAuditLogEntry.AddDescripton("AccountType", udtEHSAccount.AccountSourceString)
                Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00013, AuditLogDesc.GetAmendmentHistorySuccessNoRecordFound)
            Else
                Me.gvAmendHistory.DataSource = dt
                Me.gvAmendHistory.DataBind()

                Session(SESS_AmendHistory) = dt

                Me.GridViewDataBind(Me.gvAmendHistory, dt, "Amend_Dtm", "DESC", False)

                Me.gvAmendHistory.Columns(6).Visible = False 'EC_Serial_No
                Me.gvAmendHistory.Columns(7).Visible = False 'EC_Reference_No
                Me.gvAmendHistory.Columns(8).Visible = False 'Foreign_Passport_No
                Me.gvAmendHistory.Columns(9).Visible = False 'PASS_Issue_Region
                Me.gvAmendHistory.Columns(10).Visible = False 'Permit_To_Remain_Until
                Me.gvAmendHistory.Columns(11).Visible = False 'HKIC - Create By SmartID

                Select Case txtDocCode.Text.Trim
                    Case DocType.DocTypeModel.DocTypeCode.EC
                        Me.gvAmendHistory.Columns(6).Visible = True 'EC_Serial_No
                        Me.gvAmendHistory.Columns(7).Visible = True 'EC_Reference_No
                    Case DocType.DocTypeModel.DocTypeCode.ID235B
                        Me.gvAmendHistory.Columns(10).Visible = True 'Permit_To_Remain_Until
                    Case DocType.DocTypeModel.DocTypeCode.VISA
                        Me.gvAmendHistory.Columns(8).Visible = True 'Foreign_Passport_No
                    Case DocType.DocTypeModel.DocTypeCode.HKIC
                        Me.gvAmendHistory.Columns(11).Visible = True 'HKIC - Create By SmartID
                    Case DocType.DocTypeModel.DocTypeCode.PASS
                        Me.gvAmendHistory.Columns(9).Visible = True 'PASS_Issue_Region
                End Select


                Me.mveHSAccount.ActiveViewIndex = intAmendmentHistroy

                Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                Me.udtAuditLogEntry.AddDescripton("AccountType", udtEHSAccount.AccountSourceString)
                Me.udtAuditLogEntry.AddDescripton("NoOfRecord", dt.Rows.Count)
                Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00012, AuditLogDesc.GetAmendmentHistorySuccess)
            End If
        Catch ex As Exception
            Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
            Me.udtAuditLogEntry.AddDescripton("AccountType", udtEHSAccount.AccountSourceString)
            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00014, AuditLogDesc.GetAmendmentHistoryFail)
            Throw
        End Try

    End Sub

#Region "Amendment"

    Protected Sub ibtnWithdrawAmendment_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00073, AuditLogDesc.WithdtawAmendClick)

        Me.udcMsgBox.Clear()
        udtSM = New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVQ, MsgCode.MSG00004)

        Dim strMsg As String
        strMsg = udtSM.GetMessage
        Me.lblMsgContent.Text = strMsg

        Session(SESS_PopupActionMode) = PopupActionModel.WithdrawAmendment

        Me.ModalPopupExtenderConfirm.Show()
    End Sub

    Protected Sub ibtnAmendRecord_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Me.udcInfoMsgBox.Clear()

        ' INT20-0047 (Fix throw error for invalid CCCode) [Start][Winnie]
        ' Reset
        Me.udcCCCode.Clean()
        ' INT20-0047 (Fix throw error for invalid CCCode) [End][Winnie]

        udtEHSAccount = udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        udtEHSAccount_Amendment = udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FunctionCode)

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00015, AuditLogDesc.AmendEHSAccountRecord)

        If IsNothing(udtEHSAccount_Amendment) Then
            udtEHSAccount_Amendment = udtEHSAccount
            udteHSAccountMaintBLL.EHSAccount_Amend_SaveToSession(udtEHSAccount_Amendment, FunctionCode)
        End If

        Session(SESS_InputMode) = ActionModel.Amending


        ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [Start][Winnie] Step 0
        Session(SESS_DefaultSetCCCode) = True
        ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [End][Winnie] Step 0

        Me.BindPersonalInfo(udtEHSAccount, udtEHSAccount_Amendment, Me.txtDocCode.Text.Trim, True)
        SetAccountBtn(Session(SESS_InputMode))
        Me.SetPanelAction(EditAccountModel.Amending)
    End Sub

    Protected Sub ibtnAmendSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udcMsgBox.Visible = False
        Dim blnProceed As Boolean = True

        Me.udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        Me.udtEHSAccount_Amendment = Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FunctionCode)

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("DocCode", txtDocCode.Text.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount_Amendment.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount_Amendment.AccountSourceString)
        Me.udtAuditLogEntry.AddDescripton("IdentityNumber", udtEHSAccount_Amendment.EHSPersonalInformationList.Filter(txtDocCode.Text.Trim).IdentityNum)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00016, AuditLogDesc.SaveEHSAccount)
        'Prepare audit log for validation
        Me.udtAuditLogEntry.AddDescripton("DocCode", txtDocCode.Text.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount_Amendment.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount_Amendment.AccountSourceString)
        Me.udtAuditLogEntry.AddDescripton("IdentityNumber", udtEHSAccount_Amendment.EHSPersonalInformationList.Filter(txtDocCode.Text.Trim).IdentityNum)

        Select Case Me.txtDocCode.Text.Trim
            Case DocTypeModel.DocTypeCode.HKIC
                Dim udcInputHKIC As ucInputHKID = Me.ucInputDocumentType.GetHKICControl

                If udcInputHKIC.CCCodeIsEmpty Then
                    udcInputHKIC.SetCnameAmend(String.Empty)
                    Me.udcCCCode.Clean()
                    Me.udcCCCode.GetChineseName(FunctionCode, False)
                Else
                    If udcInputHKIC.IsValidCCCodeInput() Then
                        'Check CCCode
                        ' If CCCode is changed (session value <> input value) => pop up CCCode Panel
                        If Me.NeedPopupCCCodeDialog(ucInputDocTypeBase.BuildMode.Modification, DocTypeModel.DocTypeCode.HKIC) Then
                            Me.ucInputDocumentType_SelectChineseName_HKIC(ucInputDocTypeBase.BuildMode.Modification, udcInputHKIC, DocTypeModel.DocTypeCode.HKIC, Nothing, Nothing)
                            blnProceed = False
                        End If
                    Else
                        Me.udcMsgBox.AddMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00039)
                        udcInputHKIC.SetCCCodeError(True)
                        blnProceed = False

                    End If

                End If

                If blnProceed Then
                    blnProceed = Me.ValidateRectifyDetail_HKID(udtEHSAccount_Amendment, udtAuditLogEntry, ucInputDocTypeBase.BuildMode.Modification)
                End If

            Case DocTypeModel.DocTypeCode.EC
                blnProceed = Me.ValidateRectifyDetail_EC(udtEHSAccount_Amendment, udtAuditLogEntry, ucInputDocTypeBase.BuildMode.Modification)

            Case DocTypeModel.DocTypeCode.HKBC

                blnProceed = Me.ValidateRectifyDetail_HKBC(udtEHSAccount_Amendment, udtAuditLogEntry, ucInputDocTypeBase.BuildMode.Modification)

            Case DocTypeModel.DocTypeCode.ADOPC

                blnProceed = Me.ValidateRectifyDetail_Adopt(udtEHSAccount_Amendment, udtAuditLogEntry, ucInputDocTypeBase.BuildMode.Modification)

            Case DocTypeModel.DocTypeCode.DI

                blnProceed = Me.ValidateRectifyDetail_DI(udtEHSAccount_Amendment, udtAuditLogEntry, ucInputDocTypeBase.BuildMode.Modification)

            Case DocTypeModel.DocTypeCode.ID235B

                blnProceed = Me.ValidateRectifyDetail_ID235B(udtEHSAccount_Amendment, udtAuditLogEntry, ucInputDocTypeBase.BuildMode.Modification)

            Case DocTypeModel.DocTypeCode.REPMT

                blnProceed = Me.ValidateRectifyDetail_ReEntryPermit(udtEHSAccount_Amendment, udtAuditLogEntry, ucInputDocTypeBase.BuildMode.Modification)

            Case DocTypeModel.DocTypeCode.VISA

                blnProceed = Me.ValidateRectifyDetail_Visa(udtEHSAccount_Amendment, udtAuditLogEntry, ucInputDocTypeBase.BuildMode.Modification)

                ' CRE20-023 (Immu record) [Start][Martin]
            Case DocTypeModel.DocTypeCode.OW,
                DocTypeModel.DocTypeCode.RFNo8

                udtEHSAccount.EHSPersonalInformationList(0).DocCode = Me.txtDocCode.Text.Trim
                blnProceed = Me.ValidateRectifyDetail_OW_RFNo8(udtEHSAccount_Amendment, udtAuditLogEntry, ucInputDocTypeBase.BuildMode.Modification)

            Case DocTypeModel.DocTypeCode.TW
                blnProceed = Me.ValidateRectifyDetail_TW(udtEHSAccount_Amendment, udtAuditLogEntry, ucInputDocTypeBase.BuildMode.Modification)
                ' CRE20-0023 (Immu record) [End][Martin]

            Case DocTypeModel.DocTypeCode.CCIC

                blnProceed = Me.ValidateRectifyDetail_CCIC(udtEHSAccount_Amendment, udtAuditLogEntry, ucInputDocTypeBase.BuildMode.Modification)

            Case DocTypeModel.DocTypeCode.ROP140
                Dim udcInputROP140 As ucInputROP140 = Me.ucInputDocumentType.GetROP140Control

                If udcInputROP140.CCCodeIsEmpty Then
                    udcInputROP140.SetCnameAmend(String.Empty)
                    Me.udcCCCode.Clean()
                    Me.udcCCCode.GetChineseName(FunctionCode, False)
                Else
                    If udcInputROP140.IsValidCCCodeInput() Then
                        'Check CCCode
                        ' If CCCode is changed (session value <> input value) => pop up CCCode Panel
                        If Me.NeedPopupCCCodeDialog(ucInputDocTypeBase.BuildMode.Modification, DocTypeModel.DocTypeCode.ROP140) Then
                            Me.ucInputDocumentType_SelectChineseName_HKIC(ucInputDocTypeBase.BuildMode.Modification, udcInputROP140, DocTypeModel.DocTypeCode.ROP140, Nothing, Nothing)
                            blnProceed = False
                        End If
                    Else
                        Me.udcMsgBox.AddMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00039)
                        udcInputROP140.SetCCCodeError(True)
                        blnProceed = False

                    End If
                End If

                If blnProceed Then
                    blnProceed = Me.ValidateRectifyDetail_ROP140(udtEHSAccount_Amendment, udtAuditLogEntry, ucInputDocTypeBase.BuildMode.Modification)
                End If
            Case DocTypeModel.DocTypeCode.PASS

                blnProceed = Me.ValidateRectifyDetail_PASS(udtEHSAccount_Amendment, udtAuditLogEntry, ucInputDocTypeBase.BuildMode.Modification)



        End Select

        If blnProceed Then
            Dim udtClaimRulesBLL As New ClaimRules.ClaimRulesBLL
            Dim udtSM As SystemMessage = Nothing
            Dim udtEligibleResult As ClaimRules.ClaimRulesBLL.EligibleResult = Nothing

            udtSM = udtClaimRulesBLL.CheckAmendEHSAccountInBackOffice(udtEHSAccount_Amendment.SchemeCode, Me.txtDocCode.Text.Trim, udtEHSAccount, udtEHSAccount_Amendment, udtEligibleResult)

            Dim blnShowDeclaration As Boolean = False

            If IsNothing(udtSM) Then
                If Not IsNothing(udtEligibleResult) Then
                    If udtEligibleResult.HandleMethod = ClaimRules.ClaimRulesBLL.HandleMethodENum.Declaration Then
                        ''ModalPopupExtenderClaimDEclaration.Show()
                        blnShowDeclaration = True
                    Else
                        blnShowDeclaration = False
                    End If
                Else
                    blnShowDeclaration = False
                End If

                If Not blnShowDeclaration Then
                    Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FunctionCode)

                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    ' If changed fields is read by smart id, consider as Manual Input, otherwise, keep 
                    udtEHSAccount_Amendment.EHSPersonalInformationList.Filter(Me.txtDocCode.Text.Trim).CreateBySmartID = Me.udteHSAccountMaintBLL.IsCreatedBySmartID(udtEHSAccount, udtEHSAccount_Amendment, txtDocCode.Text.Trim)

                    If Not udtEHSAccount_Amendment.EHSPersonalInformationList.Filter(Me.txtDocCode.Text.Trim).CreateBySmartID Then
                        udtEHSAccount_Amendment.EHSPersonalInformationList.Filter(Me.txtDocCode.Text.Trim).SmartIDVer = String.Empty
                    End If
                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                    SetupConfirmAccount(udtEHSAccount_Amendment)
                    Me.mveHSAccount.ActiveViewIndex = intConfirm

                    'Audit log
                    Me.udtAuditLogEntry.AddDescripton("DocCode", txtDocCode.Text.Trim)
                    Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount_Amendment.VoucherAccID.Trim)
                    Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount_Amendment.AccountSourceString)
                    Me.udtAuditLogEntry.AddDescripton("IdentityNumber", udtEHSAccount_Amendment.AccountSourceString)
                    Me.udtAuditLogEntry.WriteEndLog(LogID.LOG00018, AuditLogDesc.ValidateAccountDetailInfoComplete)
                End If
            Else
                Me.udcMsgBox.Clear()
                Me.udcMsgBox.AddMessage(udtSM)
                'Audit log
                Me.udtAuditLogEntry.AddDescripton("DocCode", txtDocCode.Text.Trim)
                Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount_Amendment.VoucherAccID.Trim)
                Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount_Amendment.AccountSourceString)
                Me.udcMsgBox.BuildMessageBox(strValidationFail, Me.udtAuditLogEntry, LogID.LOG00019, AuditLogDesc.ValidateAccountDetailInfoFail)
            End If
        Else
            'Audit
            Me.udtAuditLogEntry.AddDescripton("DocCode", txtDocCode.Text.Trim)
            Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount_Amendment.VoucherAccID.Trim)
            Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount_Amendment.AccountSourceString)
            Me.udcMsgBox.BuildMessageBox(strValidationFail, Me.udtAuditLogEntry, LogID.LOG00019, AuditLogDesc.ValidateAccountDetailInfoFail)
        End If
    End Sub

    Protected Sub ibtnAmendCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)

        If udtEHSAccount.getPersonalInformation(txtDocCode.Text.Trim).RecordStatus = "U" Then
            Session(SESS_InputMode) = ActionModel.ReadOnly_N_Amending
            GetAmendeHSAccountOnly(udtEHSAccount.VoucherAccID)
        Else
            Session(SESS_InputMode) = ActionModel.ReadOnly
            udtEHSAccount_Amendment = Nothing

            udteHSAccountMaintBLL.EHSAccount_Amend_RemoveFromSession(FunctionCode)
            udteHSAccountMaintBLL.EHSAccount_Amend_SaveToSession(udtEHSAccount_Amendment, FunctionCode)
        End If

        udtEHSAccount_Amendment = Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FunctionCode)

        Me.BindPersonalInfo(udtEHSAccount, udtEHSAccount_Amendment, Me.txtDocCode.Text.Trim, True)
        SetAccountBtn(Session(SESS_InputMode))
        Me.SetPanelAction(EditAccountModel.None)

        udcMsgBox.Clear()

    End Sub

#End Region

#Region "Temp Account Action Btn"

    Protected Sub ibtnMarkAsImmDValid_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtEHSAccount = udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00053, AuditLogDesc.MarkImmdValidClick)

        Me.udcMsgBox.Clear()
        udtSM = New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVQ, MsgCode.MSG00001)

        Dim strMsg As String
        strMsg = udtSM.GetMessage
        Me.lblMsgContent.Text = strMsg

        Session(SESS_PopupActionMode) = PopupActionModel.MarkAsImmdValid

        Me.ModalPopupExtenderConfirm.Show()
    End Sub

    Protected Sub ibtnConfirmAsValidAcct_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtEHSAccount = udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00058, AuditLogDesc.MarkValidAcctClick)

        Me.udcMsgBox.Clear()
        udtSM = New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVQ, MsgCode.MSG00001)

        Dim strMsg As String
        strMsg = udtSM.GetMessage
        Me.lblMsgContent.Text = strMsg

        Session(SESS_PopupActionMode) = PopupActionModel.ConfirmAsValidatedAcct

        Me.ModalPopupExtenderConfirm.Show()
    End Sub

    Protected Sub ibtnReleaseForRect_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtEHSAccount = udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00068, AuditLogDesc.ReleaseRectifiClick)

        Me.udcMsgBox.Clear()
        udtSM = New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVQ, MsgCode.MSG00001)

        Dim strMsg As String
        strMsg = udtSM.GetMessage
        Me.lblMsgContent.Text = strMsg

        Session(SESS_PopupActionMode) = PopupActionModel.RelaseForRectification

        Me.ModalPopupExtenderConfirm.Show()
    End Sub

    Protected Sub ibtnCancelValidation_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtEHSAccount = udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00105, AuditLogDesc.SpecialAccCancelValiationClick)

        Me.udcMsgBox.Clear()
        udtSM = New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVQ, MsgCode.MSG00001)

        Dim strMsg As String
        strMsg = udtSM.GetMessage
        Me.lblMsgContent.Text = strMsg

        Session(SESS_PopupActionMode) = PopupActionModel.CancelValidation

        Me.ModalPopupExtenderConfirm.Show()
    End Sub

    Protected Sub ibtnRemove_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00063, AuditLogDesc.RemoveTempAcctClick)

        Me.udcMsgBox.Clear()

        ' CRE20-003-03 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
        ' -------------------------------------------------------------------------------
        ' Not allow to remove account when Vaccine File still in process
        If udtEHSAccount.SourceApp = EHSAccountModel.SourceAppClass.SFUpload Then
            Dim strStudentFileList As String = Me.getStudentFileInProcess(udtEHSAccount.VoucherAccID.Trim)

            If strStudentFileList <> String.Empty Then
                Me.udcMsgBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00013), "%s", strStudentFileList)
                Me.udcMsgBox.BuildMessageBox("ValidationFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00130, AuditLogDesc.RemoveTempAcctClickFail)

                Return
            End If
        End If
        ' CRE20-003-03 Enhancement on Programme or Scheme using batch upload [End][Winnie]

        If udtEHSAccount.TransactionID.Equals(String.Empty) Then
            udtSM = New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVQ, MsgCode.MSG00002)
        Else
            udtSM = New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVQ, MsgCode.MSG00003)
        End If

        Dim strMsg As String
        strMsg = udtSM.GetMessage
        Me.lblMsgContent.Text = strMsg

        Session(SESS_PopupActionMode) = PopupActionModel.Remove

        Me.ModalPopupExtenderConfirm.Show()
    End Sub

    Protected Sub ibtnRemoveTempAccountByBO_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00063, AuditLogDesc.RemoveTempAcctClick)

        Me.udcMsgBox.Clear()

        ' CRE20-003-03 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
        ' -------------------------------------------------------------------------------
        ' Not allow to remove account when Vaccine File still in process
        If udtEHSAccount.SourceApp = EHSAccountModel.SourceAppClass.SFUpload Then
            Dim strStudentFileList As String = Me.getStudentFileInProcess(udtEHSAccount.VoucherAccID.Trim)

            If strStudentFileList <> String.Empty Then
                Me.udcMsgBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00013), "%s", strStudentFileList)
                Me.udcMsgBox.BuildMessageBox("ValidationFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00078, AuditLogDesc.RemoveTempAcctClickFail)

                Return
            End If
        End If
        ' CRE20-003-03 Enhancement on Programme or Scheme using batch upload [End][Winnie]

        If udtEHSAccount.TransactionID.Equals(String.Empty) Then
            udtSM = New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVQ, MsgCode.MSG00002)
        Else
            udtSM = New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVQ, MsgCode.MSG00003)
        End If

        Dim strMsg As String
        strMsg = udtSM.GetMessage
        Me.lblMsgContent.Text = strMsg

        Session(SESS_PopupActionMode) = PopupActionModel.Remove

        Me.ModalPopupExtenderConfirm.Show()
    End Sub

#End Region

#Region "Suspend Public Enquiry"

    Protected Sub ibtnSuspendEnquiry_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udcInfoMsgBox.Clear()

        udtEHSAccount = udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        udtEHSAccount_Amendment = udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FunctionCode)

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00023, AuditLogDesc.SuspendEnquiryClick)

        If IsNothing(udtEHSAccount_Amendment) Then
            udteHSAccountMaintBLL.EHSAccount_Amend_RemoveFromSession(FunctionCode)
        End If
        udtEHSAccount_Amendment = udtEHSAccount
        udteHSAccountMaintBLL.EHSAccount_Amend_SaveToSession(udtEHSAccount_Amendment, FunctionCode)


        Me.SetPanelAction(EditAccountModel.SuspendEnquiry)
    End Sub

    Protected Sub ibtnSuspendPublicEnquirySave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udcMsgBox.Clear()
        Dim blnProceed As Boolean = True

        udtEHSAccount_Amendment = udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FunctionCode)

        'audit log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.AddDescripton("Reason", txtSuspendPublicEnquiryInput.Text)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00089, AuditLogDesc.SaveSuspendEnquiryClick)

        Me.imgSuspendPublicEnquiryInputErr.Visible = False

        'Check whether reason is provided
        If txtSuspendPublicEnquiryInput.Text.Trim.Equals(String.Empty) Then
            blnProceed = False
            Me.imgSuspendPublicEnquiryInputErr.Visible = True
            Me.udcMsgBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00004))
        End If

        If blnProceed Then
            udtEHSAccount_Amendment.PublicEnquiryStatus = EHSAccount.EHSAccountModel.EnquiryStatusClass.ManualSuspend
            udtEHSAccount_Amendment.PublicEnquiryRemark = txtSuspendPublicEnquiryInput.Text
            udteHSAccountMaintBLL.EHSAccount_Amend_SaveToSession(udtEHSAccount_Amendment, FunctionCode)

            SetupConfirmAccount(udtEHSAccount_Amendment)

            Me.lblConfirmReasonText.Text = Me.GetGlobalResourceObject("Text", "PublicEnquirySuspendReason")
            Me.lblConfirmReason.Text = udtEHSAccount_Amendment.PublicEnquiryRemark.Trim

            Me.mveHSAccount.ActiveViewIndex = intConfirm
        Else
            'Add audit log
            Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
            Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
            Me.udcMsgBox.BuildMessageBox(strValidationFail, Me.udtAuditLogEntry, LogID.LOG00027, AuditLogDesc.SaveSuspendEnquiryFailNoReasonProvided)
        End If
    End Sub

    Protected Sub ibtnSuspendPublicEnquiryCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'audit log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00028, AuditLogDesc.CancelSuspendEnquiry)

        Me.udcMsgBox.Clear()
        udteHSAccountMaintBLL.EHSAccount_Amend_RemoveFromSession(FunctionCode)

        Dim actionMode As ActionModel
        actionMode = Session(SESS_InputMode)
        If actionMode = ActionModel.Amending Then
            Me.SetPanelAction(EditAccountModel.Amending)
        Else
            Me.SetPanelAction(EditAccountModel.None)
        End If
    End Sub

#End Region

#Region "Reactive Public Enquiry"

    Protected Sub ibtnReactiveEnquiry_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udcInfoMsgBox.Clear()

        udtEHSAccount = udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        udtEHSAccount_Amendment = udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FunctionCode)

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00029, AuditLogDesc.ReactiveEnquiryClick)

        If IsNothing(udtEHSAccount_Amendment) Then
            udteHSAccountMaintBLL.EHSAccount_Amend_RemoveFromSession(FunctionCode)
        End If
        udtEHSAccount_Amendment = udtEHSAccount
        udteHSAccountMaintBLL.EHSAccount_Amend_SaveToSession(udtEHSAccount_Amendment, FunctionCode)


        Me.SetPanelAction(EditAccountModel.ReactiveEnquiry)
    End Sub

    Protected Sub ibtnReactivePublicEnquirySave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udcMsgBox.Clear()
        Dim blnProceed As Boolean = True

        Me.imgReactivePublicEnquiryInputErr.Visible = False

        udtEHSAccount_Amendment = udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FunctionCode)

        'audit log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount_Amendment.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount_Amendment.AccountSourceString)
        Me.udtAuditLogEntry.AddDescripton("Reason", Me.txtReactivePublicEnquiryInput.Text)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00090, AuditLogDesc.SaveReactiveEnquiryClick)

        'Check whether reason is provided
        If Me.txtReactivePublicEnquiryInput.Text.Trim.Equals(String.Empty) Then
            blnProceed = False
            Me.imgReactivePublicEnquiryInputErr.Visible = True
            Me.udcMsgBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00005))
        End If

        If blnProceed Then
            udtEHSAccount_Amendment.PublicEnquiryStatus = EHSAccount.EHSAccountModel.EnquiryStatusClass.Available
            udtEHSAccount_Amendment.PublicEnquiryRemark = txtReactivePublicEnquiryInput.Text
            udteHSAccountMaintBLL.EHSAccount_Amend_SaveToSession(udtEHSAccount_Amendment, FunctionCode)

            SetupConfirmAccount(udtEHSAccount_Amendment)

            Me.lblConfirmReasonText.Text = Me.GetGlobalResourceObject("Text", "PublicEnquiryReactivateReason")
            Me.lblConfirmReason.Text = udtEHSAccount_Amendment.PublicEnquiryRemark.Trim

            Me.mveHSAccount.ActiveViewIndex = intConfirm

        Else
            'Add audit log
            Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
            Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
            Me.udcMsgBox.BuildMessageBox(strValidationFail, Me.udtAuditLogEntry, LogID.LOG00033, AuditLogDesc.SaveReactiveEnquiryFailNoReasonProvided)
        End If
    End Sub

    Protected Sub ibtnReactivePublicEnquiryCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'audit log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00034, AuditLogDesc.CancelReactiveEnquiry)

        Me.udcMsgBox.Clear()

        Dim actionMode As ActionModel
        actionMode = Session(SESS_InputMode)
        If actionMode = ActionModel.Amending Then
            Me.SetPanelAction(EditAccountModel.Amending)
        Else
            Me.SetPanelAction(EditAccountModel.None)
        End If
    End Sub

#End Region

#Region "Suspend Account"

    Protected Sub ibtnSuspend_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udcInfoMsgBox.Clear()

        udtEHSAccount = udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        udtEHSAccount_Amendment = udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FunctionCode)

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00035, AuditLogDesc.SuspendAccountClick)

        If Not IsNothing(udtEHSAccount_Amendment) Then
            udteHSAccountMaintBLL.EHSAccount_Amend_RemoveFromSession(FunctionCode)
        End If
        udtEHSAccount_Amendment = udtEHSAccount
        udteHSAccountMaintBLL.EHSAccount_Amend_SaveToSession(udtEHSAccount_Amendment, FunctionCode)

        Me.SetPanelAction(EditAccountModel.SuspendAccount)
    End Sub

    Protected Sub ibtnSuspendAccountSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udcMsgBox.Clear()
        Dim blnProceed As Boolean = True

        Me.imgSuspendAccountInputErr.Visible = False

        udtEHSAccount_Amendment = udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FunctionCode)

        'audit log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount_Amendment.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount_Amendment.AccountSourceString)
        Me.udtAuditLogEntry.AddDescripton("Reason", txtSuspendAccountInput.Text)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00091, AuditLogDesc.SaveSuspendAccountClick)

        'Check whether reason is provided
        If txtSuspendAccountInput.Text.Trim.Equals(String.Empty) Then
            blnProceed = False
            Me.imgSuspendAccountInputErr.Visible = True
            Me.udcMsgBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002))
        End If

        If blnProceed Then
            udtEHSAccount_Amendment.RecordStatus = EHSAccountModel.ValidatedAccountRecordStatusClass.Suspended
            udtEHSAccount_Amendment.Remark = txtSuspendAccountInput.Text
            udteHSAccountMaintBLL.EHSAccount_Amend_SaveToSession(udtEHSAccount_Amendment, FunctionCode)

            SetupConfirmAccount(udtEHSAccount_Amendment)

            Me.lblConfirmReasonText.Text = Me.GetGlobalResourceObject("Text", "AccountSuspendReason")
            Me.lblConfirmReason.Text = udtEHSAccount_Amendment.Remark.Trim

            Me.mveHSAccount.ActiveViewIndex = intConfirm
        Else
            'Add audit log
            Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
            Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
            Me.udcMsgBox.BuildMessageBox(strValidationFail, Me.udtAuditLogEntry, LogID.LOG00039, AuditLogDesc.SaveSuspendAccountFailNoReasonProvided)
        End If
    End Sub

    Protected Sub ibtnSuspendAccountCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'audit log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00040, AuditLogDesc.CancelSuspendAccount)

        Me.udcMsgBox.Clear()
        udteHSAccountMaintBLL.EHSAccount_Amend_RemoveFromSession(FunctionCode)

        Dim actionMode As ActionModel
        actionMode = Session(SESS_InputMode)
        If actionMode = ActionModel.Amending Then
            Me.SetPanelAction(EditAccountModel.Amending)
        Else
            Me.SetPanelAction(EditAccountModel.None)
        End If

    End Sub

#End Region

#Region "Reactive Account"

    Protected Sub itbnReactivate_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udcInfoMsgBox.Clear()

        udtEHSAccount = udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        udtEHSAccount_Amendment = udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FunctionCode)

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00041, AuditLogDesc.ReactivateAccountClick)

        If Not IsNothing(udtEHSAccount_Amendment) Then
            udteHSAccountMaintBLL.EHSAccount_Amend_RemoveFromSession(FunctionCode)
        End If
        udtEHSAccount_Amendment = udtEHSAccount
        udteHSAccountMaintBLL.EHSAccount_Amend_SaveToSession(udtEHSAccount_Amendment, FunctionCode)


        Me.SetPanelAction(EditAccountModel.ReactiveAccount)
    End Sub

    Protected Sub ibtnReactiveAccountSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udcMsgBox.Clear()
        Dim blnProceed As Boolean = True

        Me.imgReactiveAccountInputErr.Visible = False

        udtEHSAccount_Amendment = udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FunctionCode)

        'audit log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount_Amendment.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount_Amendment.AccountSourceString)
        Me.udtAuditLogEntry.AddDescripton("Reason", Me.txtReactiveAccountInput.Text)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00092, AuditLogDesc.SaveReactivateAccountClick)

        'Check whether reason is provided
        If Me.txtReactiveAccountInput.Text.Trim.Equals(String.Empty) Then
            blnProceed = False
            Me.imgReactiveAccountInputErr.Visible = True
            Me.udcMsgBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00003))
        End If

        If blnProceed Then
            udtEHSAccount_Amendment.RecordStatus = EHSAccountModel.ValidatedAccountRecordStatusClass.Active
            udtEHSAccount_Amendment.Remark = txtReactiveAccountInput.Text.Trim
            udteHSAccountMaintBLL.EHSAccount_Amend_SaveToSession(udtEHSAccount_Amendment, FunctionCode)

            SetupConfirmAccount(udtEHSAccount_Amendment)

            Me.lblConfirmReasonText.Text = Me.GetGlobalResourceObject("Text", "AccountReactivateReason")
            ' INT16-0008 (Fix Chinese in RCH List Maintenance) [Start][Lawrence]
            Me.lblConfirmReason.Text = AntiXssEncoder.HtmlEncode(udtEHSAccount_Amendment.Remark.Trim, True)
            ' INT16-0008 (Fix Chinese in RCH List Maintenance) [End][Lawrence]

            Me.mveHSAccount.ActiveViewIndex = intConfirm
        Else
            'Add audit log
            Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
            Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
            Me.udcMsgBox.BuildMessageBox(strValidationFail, Me.udtAuditLogEntry, LogID.LOG00045, AuditLogDesc.SaveReactivateAccountFailNoReasonProvided)
        End If

    End Sub

    Protected Sub ibtnReactiveAccountCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'audit log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00046, AuditLogDesc.CancelReactivateAccount)

        Me.udcMsgBox.Clear()

        Dim actionMode As ActionModel
        actionMode = Session(SESS_InputMode)
        If actionMode = ActionModel.Amending Then
            Me.SetPanelAction(EditAccountModel.Amending)
        Else
            Me.SetPanelAction(EditAccountModel.None)
        End If
    End Sub

#End Region

#Region "Terminate Account"

    Protected Sub ibtnTerminate_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udcInfoMsgBox.Clear()

        udtEHSAccount = udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        udtEHSAccount_Amendment = udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FunctionCode)

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00047, AuditLogDesc.TerminateAccountClick)

        If Not IsNothing(udtEHSAccount_Amendment) Then
            udteHSAccountMaintBLL.EHSAccount_Amend_RemoveFromSession(FunctionCode)
        End If
        udtEHSAccount_Amendment = udtEHSAccount
        udteHSAccountMaintBLL.EHSAccount_Amend_SaveToSession(udtEHSAccount_Amendment, FunctionCode)


        Me.SetPanelAction(EditAccountModel.TerminateAccount)
    End Sub

    Protected Sub ibtnTerminateAccountSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Me.udcMsgBox.Clear()
        Dim blnProceed As Boolean = True

        Me.imgTerminateAccountInputErr.Visible = False

        udtEHSAccount_Amendment = udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FunctionCode)

        'audit log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount_Amendment.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount_Amendment.AccountSourceString)
        Me.udtAuditLogEntry.AddDescripton("Reason", Me.txtTerminateAccountInput.Text)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00093, AuditLogDesc.SaveTerminateAccounttClick)

        'Check whether reason is provided
        If Me.txtTerminateAccountInput.Text.Trim.Equals(String.Empty) Then
            blnProceed = False
            Me.imgTerminateAccountInputErr.Visible = True
            Me.udcMsgBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00006))
        End If

        If blnProceed Then
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            udtEHSAccount_Amendment.RecordStatus = EHSAccountModel.ValidatedAccountRecordStatusClass.Terminated
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
            udtEHSAccount_Amendment.Remark = txtTerminateAccountInput.Text
            udteHSAccountMaintBLL.EHSAccount_Amend_SaveToSession(udtEHSAccount_Amendment, FunctionCode)

            SetupConfirmAccount(udtEHSAccount_Amendment)

            Me.lblConfirmReasonText.Text = Me.GetGlobalResourceObject("Text", "AccountTerminateReason")
            Me.lblConfirmReason.Text = udtEHSAccount_Amendment.Remark.Trim

            Me.mveHSAccount.ActiveViewIndex = intConfirm
        Else
            'Add audit log
            Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
            Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
            Me.udcMsgBox.BuildMessageBox(strValidationFail, Me.udtAuditLogEntry, LogID.LOG00051, AuditLogDesc.SaveTerminateAccountFailNoReasonProvided)
        End If

    End Sub

    Protected Sub ibtnTerminateAccountCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'audit log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00052, AuditLogDesc.CancelTerminateAccount)

        Me.udcMsgBox.Clear()
        udteHSAccountMaintBLL.EHSAccount_Amend_RemoveFromSession(FunctionCode)

        Dim actionMode As ActionModel
        actionMode = Session(SESS_InputMode)
        If actionMode = ActionModel.Amending Then
            Me.SetPanelAction(EditAccountModel.Amending)
        Else
            Me.SetPanelAction(EditAccountModel.None)
        End If
    End Sub

#End Region

#Region "Popup Confirmation Action - Remove Temporary Account (first validation time over 29 days / Created by BO) / Mark As Immd Valid / Confirm As Valid Acct / Release for Rectification / Withdraw Amendment"

    Protected Sub ibtnDialogConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'Audit log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)

        Me.udcMsgBox.Clear()
        Me.udcInfoMsgBox.Clear()

        Me.udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        Dim udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel

        udtEHSPersonalInfo = udtEHSAccount.getPersonalInformation(Me.txtDocCode.Text.Trim)

        Dim udtHCVUUserBLL As New HCVUUserBLL
        Dim udtHCVUUser As HCVUUserModel = udtHCVUUserBLL.GetHCVUUser

        Dim strUpdateBy As String = udtHCVUUser.UserID

        If Not IsNothing(Session(SESS_PopupActionMode)) Then
            Dim _PopupActionModel As PopupActionModel
            _PopupActionModel = Session(SESS_PopupActionMode)

            udtSM = Nothing

            Try
                Select Case _PopupActionModel
                    Case PopupActionModel.ConfirmAsValidatedAcct

                        'Audit Log
                        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
                        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00059, AuditLogDesc.ConfirmValidAccount)

                        ' Confirm as a validated account
                        Dim udtImmDBLL As Common.Component.ImmD.ImmDBLL = New Common.Component.ImmD.ImmDBLL
                        udtImmDBLL.ValidateAccountEHSModel(udtEHSAccount, strUpdateBy)
                        udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00009)

                        'Audit Log
                        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
                        Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00060, AuditLogDesc.ConfirmValidAccountSuccess)
                    Case PopupActionModel.MarkAsImmdValid

                        'Audit Log
                        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
                        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00054, AuditLogDesc.ConfirmIMMDvalidAccount)

                        ' Mark As ImmD Validating
                        If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.TemporaryAccount Then
                            Me.udtEHSAccountBLL.UpdateTempEHSAccountAsImmdValidatingByBackOffice(udtEHSPersonalInfo, strUpdateBy)
                        Else
                            Me.udtEHSAccountBLL.UpdateSpecialEHSAccountAsImmdValidatingByBackOffice(udtEHSPersonalInfo, strUpdateBy)
                        End If

                        udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00007)

                        'Audit Log
                        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
                        Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00055, AuditLogDesc.ConfirmIMMDvalidSuccess)
                    Case PopupActionModel.RelaseForRectification

                        'Audit Log
                        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
                        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00069, AuditLogDesc.ConfirmReleaseRectifi)

                        'Release for Rectification
                        Me.udteHSAccountMaintBLL.ReleaseTempAcctForRectification(udtEHSAccount, Me.txtDocCode.Text.Trim, strUpdateBy)
                        udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00008)

                        'Audit Log
                        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
                        Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00070, AuditLogDesc.ConfirmReleaseRectifiSuccess)

                    Case PopupActionModel.CancelValidation

                        'Audit Log
                        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
                        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00106, AuditLogDesc.ConfirmSpecialAccCancelValidation)

                        'Cancel Validation
                        Me.udteHSAccountMaintBLL.CancelValidationForSpecialAcc(udtEHSAccount, Me.txtDocCode.Text.Trim, strUpdateBy)
                        udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00011)

                        'Audit Log
                        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
                        Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00107, AuditLogDesc.ConfirmSpecialAccCancelValidationSuccess)


                    Case PopupActionModel.Remove

                        'Audit Log
                        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
                        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00064, AuditLogDesc.ConfirmRemoveTempAcct)

                        'TO: DO
                        ''Remove Temp EHS Account which first validation time over 29 days
                        Me.udteHSAccountMaintBLL.RemoveTempAcct(udtEHSAccount, strUpdateBy)
                        If udtEHSAccount.TransactionID.Equals(String.Empty) Then
                            udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00005)
                        Else
                            udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00006)
                        End If

                        'Audit Log
                        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
                        Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00065, AuditLogDesc.ConfirmRemoveTempAcctSuccess)

                    Case PopupActionModel.WithdrawAmendment

                        'Audit Log
                        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
                        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00074, AuditLogDesc.ConfirmWithdtawAmend)

                        Me.udtEHSAccount_Amendment = udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FunctionCode)

                        Me.udteHSAccountMaintBLL.WithdrawAmendment(udtEHSAccount_Amendment, udtEHSAccount, Me.txtDocCode.Text.Trim, strUpdateBy)
                        udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00002)

                        'Audit Log
                        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
                        Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00075, AuditLogDesc.ConfirmWithdtawAmendSuccess)

                End Select

                If Not IsNothing(udtSM) Then
                    Me.udcInfoMsgBox.AddMessage(udtSM)
                    Me.udcInfoMsgBox.BuildMessageBox()
                    Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Complete

                    Me.mveHSAccount.ActiveViewIndex = intComplete
                End If

            Catch eSQL As SqlClient.SqlException
                If eSQL.Number = 50000 Then
                    'Log Void Fail and Build Message Box
                    Me.udtSM = New Common.ComObject.SystemMessage("990001", "D", eSQL.Message)
                    Me.udcMsgBox.AddMessage(Me.udtSM)
                    Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                    Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
                    Select Case _PopupActionModel
                        Case PopupActionModel.ConfirmAsValidatedAcct
                            Me.udcMsgBox.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00061, AuditLogDesc.ConfirmValidAccountFail)
                        Case PopupActionModel.MarkAsImmdValid
                            Me.udcMsgBox.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00056, AuditLogDesc.ConfirmIMMDvalidFail)
                        Case PopupActionModel.RelaseForRectification
                            Me.udcMsgBox.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00071, AuditLogDesc.ConfirmReleaseRectifiFail)
                        Case PopupActionModel.CancelValidation
                            Me.udcMsgBox.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00108, AuditLogDesc.ConfirmSpecialAccCancelValidationFail)
                        Case PopupActionModel.Remove
                            Me.udcMsgBox.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00066, AuditLogDesc.ConfirmRemoveTempAcctFail)
                        Case PopupActionModel.WithdrawAmendment
                            Me.udcMsgBox.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00076, AuditLogDesc.ConfirmWithdtawAmendFail)
                    End Select
                Else
                    Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                    Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
                    Select Case _PopupActionModel
                        Case PopupActionModel.ConfirmAsValidatedAcct
                            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00061, AuditLogDesc.ConfirmValidAccountFail)
                        Case PopupActionModel.MarkAsImmdValid
                            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00056, AuditLogDesc.ConfirmIMMDvalidFail)
                        Case PopupActionModel.RelaseForRectification
                            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00071, AuditLogDesc.ConfirmReleaseRectifiFail)
                        Case PopupActionModel.CancelValidation
                            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00108, AuditLogDesc.ConfirmSpecialAccCancelValidationFail)
                        Case PopupActionModel.Remove
                            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00066, AuditLogDesc.ConfirmRemoveTempAcctFail)
                        Case PopupActionModel.WithdrawAmendment
                            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00076, AuditLogDesc.ConfirmWithdtawAmendFail)
                    End Select
                    Throw eSQL
                End If
            Catch ex As Exception
                Throw ex
            End Try
        Else
            Throw New Exception("EHSAccount Maintenance: User action is nothing in Confirmation Popup")

        End If


    End Sub

    Protected Sub ibtnDialogCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)

        'Audit log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
        If Not IsNothing(Session(SESS_PopupActionMode)) Then
            Dim _PopupActionModel As PopupActionModel
            _PopupActionModel = Session(SESS_PopupActionMode)
            Select Case _PopupActionModel
                Case PopupActionModel.ConfirmAsValidatedAcct
                    Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00062, AuditLogDesc.CancelValidAccount)
                Case PopupActionModel.MarkAsImmdValid
                    Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00057, AuditLogDesc.CancelIMMDvalidAccount)
                Case PopupActionModel.RelaseForRectification
                    Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00072, AuditLogDesc.CancelReleaseRectifi)
                Case PopupActionModel.CancelValidation
                    Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00109, AuditLogDesc.CancelSpecialAccCancelValidaton)
                Case PopupActionModel.Remove
                    Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00067, AuditLogDesc.CancelRemoveTempAcct)
                Case PopupActionModel.WithdrawAmendment
                    Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00077, AuditLogDesc.CancelWithdtawAmend)
            End Select
        End If

        Me.ModalPopupExtenderConfirm.Hide()
    End Sub


#End Region

#End Region

#Region "View 4 - Amendment History"

    Protected Sub ibtnAmendmentHistoryBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'audit log
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00080, AuditLogDesc.BackToAccDetailFromAmendmentHist)

        Me.mveHSAccount.ActiveViewIndex = intAccountDetails
    End Sub

#Region "Gridview Function - gvAmendHistory"
    Private Sub gvAmendHistory_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvAmendHistory.PageIndexChanging
        Me.GridViewPageIndexChangingHandler(sender, e, SESS_AmendHistory)
    End Sub

    Private Sub gvAmendHistory_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvAmendHistory.PreRender
        Me.GridViewPreRenderHandler(sender, e, SESS_AmendHistory)
    End Sub

    Private Sub gvAmendHistory_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAmendHistory.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRow = CType(e.Row.DataItem, System.Data.DataRowView).Row

            Dim lblAmendDtm As Label
            Dim lblName As Label
            Dim lblCName As Label
            Dim lblDOB As Label
            Dim lblSex As Label
            Dim lblDateOfIssue As Label
            Dim lblUpdate_By As Label
            Dim lblAmendmentStatus As Label
            Dim lblECSN As Label
            Dim lblECRef As Label
            Dim lblForeignPassportNo As Label
            Dim lblPASS_Issue_Region As Label

            Dim lblPermitToRemainUntil As Label
            Dim lblCreationMethod As Label

            lblAmendDtm = CType(e.Row.FindControl("lblAmendDtm"), Label)
            lblName = CType(e.Row.FindControl("lblName"), Label)
            lblCName = CType(e.Row.FindControl("lblCName"), Label)
            lblDOB = CType(e.Row.FindControl("lblDOB"), Label)
            lblSex = CType(e.Row.FindControl("lblSex"), Label)
            lblDateOfIssue = CType(e.Row.FindControl("lblDateOfIssue"), Label)
            lblUpdate_By = CType(e.Row.FindControl("lblUpdate_By"), Label)
            lblAmendmentStatus = CType(e.Row.FindControl("lblAmendmentStatus"), Label)

            lblECSN = CType(e.Row.FindControl("lblECSN"), Label)
            lblECRef = CType(e.Row.FindControl("lblECRef"), Label)

            lblForeignPassportNo = CType(e.Row.FindControl("lblForeignPassportNo"), Label)
            lblPermitToRemainUntil = CType(e.Row.FindControl("lblPermitToRemainUntil"), Label)

            lblPASS_Issue_Region = CType(e.Row.FindControl("lblPASS_Issue_Region"), Label)


            lblCreationMethod = CType(e.Row.FindControl("lblCreationMethod"), Label)

            Dim dtmAmendDtm As DateTime = CType(dr.Item("Amend_Dtm"), DateTime)
            Dim strEngName As String = CStr(dr.Item("Eng_Name"))
            Dim strChiName As String = CStr(dr.Item("Chi_Name"))

            Dim dtmDOB As DateTime = CType(dr.Item("DOB"), DateTime)
            Dim strExactDOB As String = CStr(dr.Item("Exact_DOB"))
            Dim strSex As String = CStr(dr.Item("Sex"))
            Dim dtmDateOfIssue As Nullable(Of Date) '= CType(dr.Item("Date_of_Issue"), DateTime)
            Dim strUpdate_By As String = CStr(dr.Item("Update_By"))
            Dim strAmendmentStatus As String = CStr(dr.Item("SubmitToVerify"))
            Dim strRecord_status As String = dr.Item("Record_Status").ToString.Trim
            Dim strDocCode As String = dr.Item("doc_code").ToString.Trim

            Dim intAge As Nullable(Of Integer)
            Dim dtDOR As Nullable(Of Date)
            Dim dtmPermitToRemainUntil As Nullable(Of Date)
            Dim strOtherInfo As String

            Dim strCreateBySmartID As String

            If IsDBNull(dr.Item("EC_Age")) Then
                intAge = Nothing
            Else
                intAge = CInt(dr.Item("EC_Age"))
            End If

            If IsDBNull(dr.Item("EC_Date_of_Registration")) Then
                dtDOR = Nothing
            Else
                dtDOR = CType(dr.Item("EC_Date_of_Registration"), Date)
            End If

            If IsDBNull(dr.Item("Date_of_Issue")) Then
                dtmDateOfIssue = Nothing
            Else
                dtmDateOfIssue = CType(dr.Item("Date_of_Issue"), Date)
            End If

            Dim strSN As String
            If IsDBNull(dr.Item("EC_Serial_No")) Then
                strSN = Me.GetGlobalResourceObject("Text", "NotProvided")
            Else
                strSN = CStr(dr.Item("EC_Serial_No")).Trim
            End If

            Dim strRef As String
            If IsDBNull(dr.Item("EC_Reference_No")) Then
                strRef = String.Empty
            Else
                strRef = CStr(dr.Item("EC_Reference_No")).Trim
            End If

            Dim blnRefOtherFormat As Boolean = False
            If Not IsDBNull(dr.Item("EC_Reference_No_Other_Format")) AndAlso CStr(dr.Item("EC_Reference_No_Other_Format")).Trim = "Y" Then
                blnRefOtherFormat = True
            End If

            Dim strForeignPassportNo
            If IsDBNull(dr.Item("Foreign_Passport_No")) Then
                strForeignPassportNo = String.Empty
            Else
                strForeignPassportNo = CStr(dr.Item("Foreign_Passport_No")).Trim
            End If

            Dim strPASSIssueRegion
            If IsDBNull(dr.Item("PASS_Issue_Region")) Then
                strPASSIssueRegion = String.Empty
            Else
                strPASSIssueRegion = CStr(dr.Item("PASS_Issue_Region")).Trim
            End If


            If IsDBNull(dr.Item("Permit_To_Remain_Until")) Then
                dtmPermitToRemainUntil = Nothing
            Else
                dtmPermitToRemainUntil = CType(dr.Item("Permit_To_Remain_Until"), Date)
            End If

            If IsDBNull(dr.Item("other_info")) Then
                strOtherInfo = String.Empty
            Else
                strOtherInfo = CStr(dr.Item("other_info"))
            End If

            If IsDBNull(dr.Item("Create_By_SmartID")) Then
                strCreateBySmartID = String.Empty
            Else
                strCreateBySmartID = CStr(dr.Item("Create_By_SmartID"))

                If strCreateBySmartID.Equals("Y") Then
                    strCreateBySmartID = Me.GetGlobalResourceObject("Text", "SmartIC")
                Else
                    strCreateBySmartID = Me.GetGlobalResourceObject("Text", "ManualInput")
                End If
            End If

            lblAmendDtm.Text = udtformatter.formatDateTime(dtmAmendDtm)
            lblName.Text = strEngName
            lblCName.Text = udtformatter.formatChineseName(strChiName.Trim)

            lblDOB.Text = udtformatter.formatDOB(strDocCode, dtmDOB, strExactDOB, Session(SESS_Language), intAge, dtDOR, strOtherInfo)

            lblSex.Text = Me.GetGlobalResourceObject("Text", udtformatter.formatGender(strSex))
            lblUpdate_By.Text = strUpdate_By

            lblDateOfIssue.Text = udtformatter.formatDOI_GV(dtmDateOfIssue)
            If lblDateOfIssue.Text.Trim.Equals(String.Empty) Then
                lblDateOfIssue.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            lblECSN.Text = strSN

            If blnRefOtherFormat Then
                lblECRef.Text = strRef
            Else
                lblECRef.Text = udtformatter.formatReferenceNo(strRef, False)
            End If

            lblForeignPassportNo.Text = strForeignPassportNo

            If (IsNothing(strPASSIssueRegion.Trim) Or strPASSIssueRegion.Trim.Equals(String.Empty)) Then
                lblPASS_Issue_Region.Text = Me.GetGlobalResourceObject("Text", "NotProvided")
            Else
                lblPASS_Issue_Region.Text = udtPassportIssueRegionBLL.GetPassportIssueRegion.Filter(strPASSIssueRegion.Trim).NationalDesc
            End If



            If Not IsNothing(dtmPermitToRemainUntil) Then
                lblPermitToRemainUntil.Text = udtformatter.formatID235BPermittedToRemainUntil(dtmPermitToRemainUntil)
            End If

            lblCreationMethod.Text = strCreateBySmartID

            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ' Get the Record_status wording 
            If strRecord_status = "V" Then
                lblAmendmentStatus.Text = Me.GetGlobalResourceObject("Text", "PendingValidation")
            ElseIf strRecord_status = "A" Then
                lblAmendmentStatus.Text = Me.GetGlobalResourceObject("Text", "N/A")
            ElseIf strRecord_status = "C" Then
                'lblAmendmentStatus.Text = "Current Record"
                lblAmendmentStatus.Text = Me.GetGlobalResourceObject("Text", "LatestMergedRecord")
            End If
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
        End If
    End Sub

    Private Sub gvAmendHistory_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvAmendHistory.Sorting
        Me.GridViewSortingHandler(sender, e, SESS_AmendHistory)
    End Sub
#End Region

#End Region

#Region "View 5 - Scheme Information"

    Protected Sub ibtnSchemeInfoBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'audit log
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00081, AuditLogDesc.BackToAccDetailFromSchemeInfo)

        Me.mveHSAccount.ActiveViewIndex = intAccountDetails
    End Sub

#End Region

#Region "View 6 - Confirm Account"

    Protected Sub ibtnConfirmAmendedAccount_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Me.udcMsgBox.Clear()
        Me.udcInfoMsgBox.Clear()

        Me.udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        Me.udtEHSAccount_Amendment = Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FunctionCode)

        Dim udtHCVUUserBLL As New HCVUUserBLL
        Dim udtHCVUUser As HCVUUserModel = udtHCVUUserBLL.GetHCVUUser

        Dim strUpdateBy As String = udtHCVUUser.UserID

        If Not IsNothing(Session(SESS_ActionMode)) Then

            Dim actionMode As EditAccountModel
            actionMode = Session(SESS_ActionMode)

            udtSM = Nothing

            Dim blnErr As Boolean = False

            Try
                ' CRE11-007
                If Me.panConfrimBatchAccount.Visible Then
                    ' ---------------------------------------------------------------------------------------------
                    ' Handle mulitple account action
                    ' ---------------------------------------------------------------------------------------------
                    udtSM = Me.BatchAccountAction(actionMode, udtHCVUUser)
                Else
                    ' ---------------------------------------------------------------------------------------------
                    ' Handle single account action
                    ' ---------------------------------------------------------------------------------------------
                    Select Case actionMode
                        Case EditAccountModel.Amending
                            ' Amend Record

                            'Audit Log
                            Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
                            With udtEHSAccount_Amendment
                                Me.udtAuditLogEntry.AddDescripton("Account ID", .VoucherAccID.Trim)
                                Me.udtAuditLogEntry.AddDescripton("AcctType", .AccountPurpose)
                                Me.udtAuditLogEntry.AddDescripton("AccountSource", .AccountSource)
                                Me.udtAuditLogEntry.AddDescripton("AccountSourceString", .AccountSourceString)
                                With udtEHSAccount_Amendment.EHSPersonalInformationList(0)
                                    Me.udtAuditLogEntry.AddDescripton("IdentityNum", .IdentityNum)
                                    Me.udtAuditLogEntry.AddDescripton("DocCode", .DocCode)
                                    Me.udtAuditLogEntry.AddDescripton("DOB", .DOB)
                                    Me.udtAuditLogEntry.AddDescripton("ExactDOB", IIf(IsNothing(.ExactDOB), String.Empty, .ExactDOB))
                                    Me.udtAuditLogEntry.AddDescripton("EngSurname", IIf(IsNothing(.ENameSurName), String.Empty, .ENameSurName))
                                    Me.udtAuditLogEntry.AddDescripton("EngOthername", IIf(IsNothing(.ENameFirstName), String.Empty, .ENameFirstName))
                                    Me.udtAuditLogEntry.AddDescripton("ChiName", IIf(IsNothing(.CName), String.Empty, .CName))
                                    Me.udtAuditLogEntry.AddDescripton("CCCode1", IIf(IsNothing(.CCCode1), String.Empty, .CCCode1))
                                    Me.udtAuditLogEntry.AddDescripton("CCCode2", IIf(IsNothing(.CCCode2), String.Empty, .CCCode2))
                                    Me.udtAuditLogEntry.AddDescripton("CCCode3", IIf(IsNothing(.CCCode3), String.Empty, .CCCode3))
                                    Me.udtAuditLogEntry.AddDescripton("CCCode4", IIf(IsNothing(.CCCode4), String.Empty, .CCCode4))
                                    Me.udtAuditLogEntry.AddDescripton("CCCode5", IIf(IsNothing(.CCCode5), String.Empty, .CCCode5))
                                    Me.udtAuditLogEntry.AddDescripton("CCCode6", IIf(IsNothing(.CCCode6), String.Empty, .CCCode6))
                                    Me.udtAuditLogEntry.AddDescripton("Gender", IIf(IsNothing(.Gender), String.Empty, .Gender))

                                    Me.udtAuditLogEntry.AddDescripton("ECReferenceNo", IIf(IsNothing(.ECReferenceNo), String.Empty, .ECReferenceNo))
                                    Dim strECReferenceNoOtherFormat As String = String.Empty
                                    If Not IsNothing(.ECReferenceNo) Then
                                        strECReferenceNoOtherFormat = IIf(.ECReferenceNoOtherFormat, "Y", "N")
                                    End If
                                    Me.udtAuditLogEntry.AddDescripton("ECReferenceNoOtherFormat", strECReferenceNoOtherFormat)

                                    Me.udtAuditLogEntry.AddDescripton("ECSerialNumber", IIf(IsNothing(.ECSerialNo), String.Empty, .ECSerialNo))
                                    Me.udtAuditLogEntry.AddDescripton("DateOfIssue", IIf(IsNothing(.DateofIssue), String.Empty, .DateofIssue))
                                    Me.udtAuditLogEntry.AddDescripton("ECAge", IIf(IsNothing(.ECAge), String.Empty, .ECAge))
                                    Me.udtAuditLogEntry.AddDescripton("ECDateOfRegistration", IIf(IsNothing(.ECDateOfRegistration), String.Empty, .ECDateOfRegistration))
                                    Me.udtAuditLogEntry.AddDescripton("DOBTypeSelected", IIf(IsNothing(.DOBTypeSelected), String.Empty, .DOBTypeSelected))
                                    Me.udtAuditLogEntry.AddDescripton("AdoptionField", IIf(IsNothing(.AdoptionField), String.Empty, .AdoptionField))
                                    Me.udtAuditLogEntry.AddDescripton("AdoptionPrefixNum", IIf(IsNothing(.AdoptionPrefixNum), String.Empty, .AdoptionPrefixNum))
                                    Me.udtAuditLogEntry.AddDescripton("ForeignPassportNo", IIf(IsNothing(.Foreign_Passport_No), String.Empty, .Foreign_Passport_No))
                                    Me.udtAuditLogEntry.AddDescripton("OtherInfo", IIf(IsNothing(.OtherInfo), String.Empty, .OtherInfo))
                                    Me.udtAuditLogEntry.AddDescripton("PermitToRemainUntil", IIf(IsNothing(.PermitToRemainUntil), String.Empty, .PermitToRemainUntil))
                                    Me.udtAuditLogEntry.AddDescripton("PassportIssueRegion", IIf(IsNothing(.PassportIssueRegion), String.Empty, .PassportIssueRegion))
                                    Me.udtAuditLogEntry.AddDescripton("RecordStatus", IIf(IsNothing(.RecordStatus), String.Empty, .RecordStatus))
                                End With
                            End With
                            Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00020, AuditLogDesc.ConfirmSaveEHSAccount)

                            udtSM = Me.udteHSAccountMaintBLL.AmendEHSAccount(udtEHSAccount, udtEHSAccount_Amendment, Me.txtDocCode.Text.Trim, strUpdateBy, blnErr)

                            'audit log
                            Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount_Amendment.VoucherAccID.Trim)
                            Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount_Amendment.AccountSourceString)
                            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00021, AuditLogDesc.ConfirmSaveEHSAccountComplete)
                        Case EditAccountModel.ReactiveAccount
                            'Reactivate Account
                            udtSM = ReactivateAccount(udtEHSAccount, udtEHSAccount_Amendment, strUpdateBy)

                        Case EditAccountModel.ReactiveEnquiry
                            'Reactivate Public Enquiry
                            udtSM = ReactivateEnquiryAccount(udtEHSAccount, udtEHSAccount_Amendment, strUpdateBy)

                        Case EditAccountModel.SuspendAccount
                            'Suspend Account
                            udtSM = SuspendAccount(udtEHSAccount, udtEHSAccount_Amendment, strUpdateBy)

                        Case EditAccountModel.SuspendEnquiry
                            'Suspend Public Enquiry
                            udtSM = SuspendEnquiryAccount(udtEHSAccount, udtEHSAccount_Amendment, strUpdateBy)

                        Case EditAccountModel.TerminateAccount
                            'Terminate Account
                            udtSM = TerminateAccount(udtEHSAccount, udtEHSAccount_Amendment, strUpdateBy)

                        Case EditAccountModel.NewAccount
                            'Create New Account
                            Dim udteHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel
                            udteHSAccountPersonalInfo = udtEHSAccount.getPersonalInformation(Me.txtDocCode.Text.Trim)

                            Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)


                            Dim source As String = udtEHSAccount.AccountSource


                            Dim udtAuditLogInfo As AuditLogInfo
                            Me.udtAuditLogEntry.AddDescripton("DocType", Me.txtDocCode.Text)
                            Me.udtAuditLogEntry.AddDescripton("IdentityNo", udtEHSAccount.EHSPersonalInformationList.Filter(Me.txtDocCode.Text).IdentityNum)
                            udtAuditLogInfo = New AuditLogInfo(Nothing, Nothing, Nothing, _
                                                                 Nothing, txtDocCode.Text.Trim, udtEHSAccount.EHSPersonalInformationList.Filter(Me.txtDocCode.Text).IdentityNum)
                            Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00101, AuditLogDesc.SaveNewAccount, udtAuditLogInfo)

                            udtSM = Me.udteHSAccountMaintBLL.CreateTemporaryEHSAccountByBO(Me.udtEHSAccount)

                            Me.udtAuditLogEntry.AddDescripton("DocType", Me.txtDocCode.Text)
                            Me.udtAuditLogEntry.AddDescripton("IdentityNo", udtEHSAccount.EHSPersonalInformationList.Filter(Me.txtDocCode.Text).IdentityNum)
                            Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.EHSPersonalInformationList.Filter(Me.txtDocCode.Text).VoucherAccID)
                            udtAuditLogInfo = New AuditLogInfo(Nothing, Nothing, EHealthAccountType.Temporary, udtEHSAccount.EHSPersonalInformationList.Filter(Me.txtDocCode.Text).VoucherAccID, _
                                              txtDocCode.Text.Trim, udtEHSAccount.EHSPersonalInformationList.Filter(Me.txtDocCode.Text).IdentityNum)
                            Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00102, AuditLogDesc.SaveNewAccountSuccess)

                            If IsNothing(udtSM) Then
                                'Success
                                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
                                Dim blnManualValidation As Nullable(Of Boolean) = Nothing
                                blnManualValidation = udtValidator.chkManualValidation(Me.txtDocCode.Text.Trim, udteHSAccountPersonalInfo)
                                If blnManualValidation.HasValue Then
                                    If blnManualValidation.Value Then
                                        Me.udcInfoMsgBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00015, _
                                                                    New String() {"%s"}, New Object() {udtformatter.formatSystemNumber(udtEHSAccount.VoucherAccID.Trim)})
                                    Else
                                        Me.udcInfoMsgBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00012, _
                                                                    New String() {"%s"}, New Object() {udtformatter.formatSystemNumber(udtEHSAccount.VoucherAccID.Trim)})
                                    End If
                                Else
                                    Me.udcInfoMsgBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00012, _
                                                                New String() {"%s"}, New Object() {udtformatter.formatSystemNumber(udtEHSAccount.VoucherAccID.Trim)})
                                End If

                                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]
                            Else
                                'Fail
                                blnErr = True
                            End If

                    End Select
                End If

                If blnErr Then
                    If Not IsNothing(udtSM) Then
                        Me.mveHSAccount.ActiveViewIndex = intComplete

                        Me.udcMsgBox.AddMessage(udtSM)
                        Me.udcMsgBox.BuildMessageBox("UpdateFail")
                    End If

                Else
                    If Not IsNothing(udtSM) Then
                        Me.udcInfoMsgBox.AddMessage(udtSM)
                        Me.udcInfoMsgBox.BuildMessageBox()
                        Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Complete

                        Me.mveHSAccount.ActiveViewIndex = intComplete
                    Else
                        Me.udcInfoMsgBox.BuildMessageBox()
                        Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Complete

                        Me.mveHSAccount.ActiveViewIndex = intComplete
                    End If
                End If

            Catch eSQL As SqlClient.SqlException
                If eSQL.Number = 50000 Then
                    'Log Void Fail and Build Message Box
                    Me.udtSM = New Common.ComObject.SystemMessage("990001", "D", eSQL.Message)
                    Me.udcMsgBox.AddMessage(Me.udtSM)
                    Select Case actionMode
                        Case EditAccountModel.Amending
                            Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount_Amendment.VoucherAccID.Trim)
                            Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount_Amendment.AccountSource)
                            Me.udcMsgBox.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00022, AuditLogDesc.ConfirmSaveEHSAccountFail)
                        Case EditAccountModel.ReactiveAccount
                            Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount_Amendment.VoucherAccID.Trim)
                            Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount_Amendment.AccountSource)
                            Me.udcMsgBox.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00044, AuditLogDesc.SaveReactivateAccountFail)
                        Case EditAccountModel.ReactiveEnquiry
                            Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount_Amendment.VoucherAccID.Trim)
                            Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount_Amendment.AccountSource)
                            Me.udcMsgBox.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00032, AuditLogDesc.SaveReactiveEnquiryFail)
                        Case EditAccountModel.SuspendAccount
                            Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount_Amendment.VoucherAccID.Trim)
                            Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount_Amendment.AccountSource)
                            Me.udcMsgBox.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00038, AuditLogDesc.SaveSuspendAccountFail)
                        Case EditAccountModel.SuspendEnquiry
                            Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount_Amendment.VoucherAccID.Trim)
                            Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount_Amendment.AccountSource)
                            Me.udcMsgBox.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00026, AuditLogDesc.SaveSuspendEnquiryFail)
                        Case EditAccountModel.TerminateAccount
                            Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount_Amendment.VoucherAccID.Trim)
                            Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount_Amendment.AccountSource)
                            Me.udcMsgBox.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00050, AuditLogDesc.SaveTerminateAccountFail)
                        Case EditAccountModel.NewAccount
                            Me.udtAuditLogEntry.AddDescripton("DocType", Me.txtDocCode.Text)
                            Me.udtAuditLogEntry.AddDescripton("IdentityNo", udtEHSAccount.EHSPersonalInformationList.Filter(Me.txtDocCode.Text).IdentityNum)
                            Dim udtAuditLogInfo As AuditLogInfo
                            udtAuditLogInfo = New AuditLogInfo(Nothing, Nothing, Nothing, _
                                                                 Nothing, txtDocCode.Text.Trim, Me.udtformatter.formatDocumentIdentityNumber(txtDocCode.Text, Me.txtSearchIdentityNumR2.Text.Replace("(", "").Replace(")", "").Replace("-", "")))
                            Me.udcMsgBox.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00103, AuditLogDesc.SaveNewAccountFail, udtAuditLogInfo)
                    End Select
                Else
                    Throw eSQL
                End If
            Catch ex As Exception
                Select Case actionMode
                    Case EditAccountModel.Amending
                        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount_Amendment.VoucherAccID.Trim)
                        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount_Amendment.AccountSource)
                        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00022, AuditLogDesc.ConfirmSaveEHSAccountFail)
                    Case EditAccountModel.ReactiveAccount
                        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount_Amendment.VoucherAccID.Trim)
                        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount_Amendment.AccountSource)
                        Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00044, AuditLogDesc.SaveReactivateAccountFail)
                    Case EditAccountModel.ReactiveEnquiry
                        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount_Amendment.VoucherAccID.Trim)
                        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount_Amendment.AccountSource)
                        Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00032, AuditLogDesc.SaveReactiveEnquiryFail)
                    Case EditAccountModel.SuspendAccount
                        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount_Amendment.VoucherAccID.Trim)
                        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount_Amendment.AccountSource)
                        Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00038, AuditLogDesc.SaveSuspendAccountFail)
                    Case EditAccountModel.SuspendEnquiry
                        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount_Amendment.VoucherAccID.Trim)
                        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount_Amendment.AccountSource)
                        Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00026, AuditLogDesc.SaveSuspendEnquiryFail)
                    Case EditAccountModel.TerminateAccount
                        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount_Amendment.VoucherAccID.Trim)
                        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount_Amendment.AccountSource)
                        Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00050, AuditLogDesc.SaveTerminateAccountFail)
                    Case EditAccountModel.NewAccount
                        Me.udtAuditLogEntry.AddDescripton("DocType", Me.txtDocCode.Text)
                        Me.udtAuditLogEntry.AddDescripton("IdentityNo", udtEHSAccount.EHSPersonalInformationList.Filter(Me.txtDocCode.Text).IdentityNum)
                        Dim udtAuditLogInfo As AuditLogInfo
                        udtAuditLogInfo = New AuditLogInfo(Nothing, Nothing, Nothing, _
                                                             Nothing, txtDocCode.Text.Trim, Me.udtformatter.formatDocumentIdentityNumber(txtDocCode.Text.Trim, Me.txtSearchIdentityNumR2.Text.Replace("(", "").Replace(")", "").Replace("-", "")))
                        Me.udcMsgBox.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00103, AuditLogDesc.SaveNewAccountFail, udtAuditLogInfo)
                End Select

                Throw
            End Try
        Else
            Throw New Exception("EHSAccount Maintenance: User action is nothing in Confirm Page")
        End If

    End Sub

    Protected Sub ibtnConfirmCancelAmendedAccont_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'audit log
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00082, AuditLogDesc.BackToAccDetailCancelAmend)

        Dim udcActionMode As EditAccountModel
        udcActionMode = Session(SESS_ActionMode)

        udcInfoMsgBox.Clear()

        If udcActionMode = EditAccountModel.NewAccount Then

            Me.udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
            Me.ucInputDocumentType_NewAcc.EHSAccountAmend = Me.udtEHSAccount
            Me.ucInputDocumentType_NewAcc.DocType = Me.udtEHSAccount.EHSPersonalInformationList(0).DocCode
            Me.ucInputDocumentType_NewAcc.Mode = ucInputDocTypeBase.BuildMode.Creation
            Me.ucInputDocumentType_NewAcc.Built()

            udcStep1DocumentTypeRadioButtonGroup.ShowAllSelection = True
            Me.udcStep1DocumentTypeRadioButtonGroup.ShowLegend = False
            Me.udcStep1DocumentTypeRadioButtonGroup.SelectedValue = Me.udtEHSAccount.EHSPersonalInformationList(0).DocCode
            udcStep1DocumentTypeRadioButtonGroup.Build()

            Me.mveHSAccount.ActiveViewIndex = intNewAccount
        Else
            ' CRE11-007
            If panConfrimBatchAccount.Visible Then
                ' Multiple account action back to search result
                Me.mveHSAccount.SetActiveView(Me.vSearchResult)
            Else
                ' Single account action back to detail
                Me.mveHSAccount.ActiveViewIndex = intAccountDetails
            End If

        End If

    End Sub

#End Region

#Region "View 7 - Complete"

    Protected Sub ibtnReturn_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'audit log
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00083, AuditLogDesc.BackToResultListFromComplete)

        Me.mveHSAccount.ActiveViewIndex = intSearchView

        Dim actionMode As EditAccountModel
        actionMode = Session(SESS_ActionMode)

        If Not actionMode = EditAccountModel.NewAccount Then
            Me.ibtnSearch_Click(Nothing, Nothing)
        Else
            Me.ResetControls()
            Me.udcMsgBox.Clear()
            Me.udcInfoMsgBox.Clear()
        End If
    End Sub

#End Region

#Region "View 8 - Creation New Account"

    Protected Sub ibtnCreationCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        Dim udtAuditLogInfo As AuditLogInfo
        udtAuditLogInfo = New AuditLogInfo(Nothing, Nothing, Nothing, _
                                             Nothing, txtDocCode.Text.Trim, Me.udtformatter.formatDocumentIdentityNumber(txtDocCode.Text, Me.txtSearchIdentityNumR2.Text.Replace("(", "").Replace(")", "").Replace("-", "")))
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00104, AuditLogDesc.CancelCreateNewAccount)

        Me.ucInputDocumentType_NewAcc.Clear()
        Me.ResetControls()
        Me.mveHSAccount.ActiveViewIndex = intSearchView
    End Sub

    Private Sub udcStep1DocumentTypeRadioButtonGroup_CheckedChanged(ByVal sender As Object, ByVal e As CustomControls.DocumentTypeRadioButtonGroup.DocumentTypeRadioButtonGroupArgs) _
       Handles udcStep1DocumentTypeRadioButtonGroup.CheckedChanged
        Dim strSelectDocType As String = String.Empty

        Me.udcMsgBox.Clear()

        strSelectDocType = udcStep1DocumentTypeRadioButtonGroup.SelectedValue.Trim
        'Update the model
        Me.udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        Me.udtEHSAccount.EHSPersonalInformationList(0).DocCode = strSelectDocType
        Me.udteHSAccountMaintBLL.EHSAccountSaveToSession(Me.udtEHSAccount, FunctionCode)

        BindPersonalInfoForCreation(Me.udtEHSAccount, strSelectDocType, True)
    End Sub

    Protected Sub ibtnNewAccountSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udcMsgBox.Visible = False
        Dim blnProceed As Boolean = True

        'Show data of death redirect button
        Dim blnShowDateOfDeathBtn As Boolean = True  ' CRE14-016

        'Clear Error Images
        Me.imgSPIDErr.Visible = False
        Me.imgPracticeErr.Visible = False

        ' TO: Add the log ID
        'Audit Log
        Dim udtAuditLogInfo As AuditLogInfo
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("DocCode", txtDocCode.Text.Trim)
        udtAuditLogInfo = New AuditLogInfo(Nothing, Nothing, Nothing, _
                                             Nothing, txtDocCode.Text.Trim, Me.udtformatter.formatDocumentIdentityNumber(txtDocCode.Text, Me.txtSearchIdentityNumR2.Text.Replace("(", "").Replace(")", "").Replace("-", "")))
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00100, AuditLogDesc.CreateNewAccountSaveButtonClick, udtAuditLogInfo)

        Select Case Me.txtDocCode.Text.Trim
            Case DocTypeModel.DocTypeCode.HKIC

                udtEHSAccount.EHSPersonalInformationList(0).DocCode = DocTypeModel.DocTypeCode.HKIC
                Dim udcInputHKIC As ucInputHKID = Me.ucInputDocumentType_NewAcc.GetHKICControl

                If udcInputHKIC.CCCodeIsEmpty Then
                    udcInputHKIC.SetCnameAmend(String.Empty)
                    Me.udcCCCode.Clean()
                    Me.udcCCCode.GetChineseName(FunctionCode, False)
                Else
                    If udcInputHKIC.IsValidCCCodeNewInput() Then
                        'Check CCCode
                        ' If CCCode is changed (session value <> input value) => pop up CCCode Panel
                        If Me.NeedPopupCCCodeDialog(ucInputDocTypeBase.BuildMode.Creation, DocTypeModel.DocTypeCode.HKIC) Then
                            Me.ucInputDocumentType_SelectChineseName_HKIC(ucInputDocTypeBase.BuildMode.Creation, udcInputHKIC, DocTypeModel.DocTypeCode.HKIC, Nothing, Nothing)
                            blnProceed = False
                        End If
                    Else
                        Me.udcMsgBox.AddMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00039)
                        udcInputHKIC.SetCCCodeError(True)
                        blnProceed = False

                    End If
                End If

                If blnProceed Then
                    blnProceed = Me.ValidateRectifyDetail_HKID(udtEHSAccount, udtAuditLogEntry, ucInputDocTypeBase.BuildMode.Creation)
                End If

            Case DocTypeModel.DocTypeCode.EC
                udtEHSAccount.EHSPersonalInformationList(0).DocCode = DocTypeModel.DocTypeCode.EC
                blnProceed = Me.ValidateRectifyDetail_EC(udtEHSAccount, udtAuditLogEntry, ucInputDocTypeBase.BuildMode.Creation)

            Case DocTypeModel.DocTypeCode.HKBC
                udtEHSAccount.EHSPersonalInformationList(0).DocCode = DocTypeModel.DocTypeCode.HKBC
                blnProceed = Me.ValidateRectifyDetail_HKBC(udtEHSAccount, udtAuditLogEntry, ucInputDocTypeBase.BuildMode.Creation)

            Case DocTypeModel.DocTypeCode.ADOPC
                udtEHSAccount.EHSPersonalInformationList(0).DocCode = DocTypeModel.DocTypeCode.ADOPC
                blnProceed = Me.ValidateRectifyDetail_Adopt(udtEHSAccount, udtAuditLogEntry, ucInputDocTypeBase.BuildMode.Creation)

            Case DocTypeModel.DocTypeCode.DI
                udtEHSAccount.EHSPersonalInformationList(0).DocCode = DocTypeModel.DocTypeCode.DI
                blnProceed = Me.ValidateRectifyDetail_DI(udtEHSAccount, udtAuditLogEntry, ucInputDocTypeBase.BuildMode.Creation)

            Case DocTypeModel.DocTypeCode.ID235B
                udtEHSAccount.EHSPersonalInformationList(0).DocCode = DocTypeModel.DocTypeCode.ID235B
                blnProceed = Me.ValidateRectifyDetail_ID235B(udtEHSAccount, udtAuditLogEntry, ucInputDocTypeBase.BuildMode.Creation)

            Case DocTypeModel.DocTypeCode.REPMT
                udtEHSAccount.EHSPersonalInformationList(0).DocCode = DocTypeModel.DocTypeCode.REPMT
                blnProceed = Me.ValidateRectifyDetail_ReEntryPermit(udtEHSAccount, udtAuditLogEntry, ucInputDocTypeBase.BuildMode.Creation)

            Case DocTypeModel.DocTypeCode.VISA
                udtEHSAccount.EHSPersonalInformationList(0).DocCode = DocTypeModel.DocTypeCode.VISA
                blnProceed = Me.ValidateRectifyDetail_Visa(udtEHSAccount, udtAuditLogEntry, ucInputDocTypeBase.BuildMode.Creation)

                ' CRE20-0023 (Immu record) [Start][Martin]
            Case DocTypeModel.DocTypeCode.OW,
                DocTypeModel.DocTypeCode.RFNo8
                udtEHSAccount.EHSPersonalInformationList(0).DocCode = Me.txtDocCode.Text.Trim
                blnProceed = Me.ValidateRectifyDetail_OW_RFNo8(udtEHSAccount, udtAuditLogEntry, ucInputDocTypeBase.BuildMode.Creation)

            Case DocTypeModel.DocTypeCode.TW
                udtEHSAccount.EHSPersonalInformationList(0).DocCode = DocTypeModel.DocTypeCode.TW
                blnProceed = Me.ValidateRectifyDetail_TW(udtEHSAccount, udtAuditLogEntry, ucInputDocTypeBase.BuildMode.Creation)
                ' CRE20-0023 (Immu record) [End][Martin]

            Case DocTypeModel.DocTypeCode.CCIC
                udtEHSAccount.EHSPersonalInformationList(0).DocCode = DocTypeModel.DocTypeCode.CCIC
                blnProceed = Me.ValidateRectifyDetail_CCIC(udtEHSAccount, udtAuditLogEntry, ucInputDocTypeBase.BuildMode.Creation)

            Case DocTypeModel.DocTypeCode.ROP140
                udtEHSAccount.EHSPersonalInformationList(0).DocCode = DocTypeModel.DocTypeCode.ROP140
                Dim udcInputROP140 As ucInputROP140 = Me.ucInputDocumentType_NewAcc.GetROP140Control

                If udcInputROP140.CCCodeIsEmpty Then
                    udcInputROP140.SetCnameAmend(String.Empty)
                    Me.udcCCCode.Clean()
                    Me.udcCCCode.GetChineseName(FunctionCode, False)
                Else
                    If udcInputROP140.IsValidCCCodeNewInput() Then
                        'Check CCCode
                        ' If CCCode is changed (session value <> input value) => pop up CCCode Panel
                        If Me.NeedPopupCCCodeDialog(ucInputDocTypeBase.BuildMode.Creation, DocTypeModel.DocTypeCode.ROP140) Then
                            Me.ucInputDocumentType_SelectChineseName_HKIC(ucInputDocTypeBase.BuildMode.Creation, udcInputROP140, DocTypeModel.DocTypeCode.ROP140, Nothing, Nothing)
                            blnProceed = False
                        End If
                    Else
                        Me.udcMsgBox.AddMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00039)
                        udcInputROP140.SetCCCodeError(True)
                        blnProceed = False

                    End If

                End If

                If blnProceed Then
                    blnProceed = Me.ValidateRectifyDetail_ROP140(udtEHSAccount, udtAuditLogEntry, ucInputDocTypeBase.BuildMode.Creation)
                End If

            Case DocTypeModel.DocTypeCode.PASS
                udtEHSAccount.EHSPersonalInformationList(0).DocCode = DocTypeModel.DocTypeCode.PASS
                blnProceed = Me.ValidateRectifyDetail_PASS(udtEHSAccount, udtAuditLogEntry, ucInputDocTypeBase.BuildMode.Creation)


        End Select

        'Check other account creation information
        'SP ID
        'If Me.txtEnterCreationDetailSPID.Text = "" And Not Me.cboCreateByDH.Checked Then
        '    Me.udcMsgBox.AddMessage(New SystemMessage("990000", "E", "00132"))
        '    blnProceed = False
        '    Me.imgSPIDErr.Visible = True
        'End If

        ''Practice
        'If Me.ddlEnterCreationDetailPractice.SelectedValue.Trim = "" And Not Me.cboCreateByDH.Checked Then
        '    Me.udcMsgBox.AddMessage(New SystemMessage("990000", "E", "00270"))
        '    blnProceed = False
        '    Me.imgPracticeErr.Visible = True
        'End If

        'Scheme
        'If Me.ddlEnterCreationDetailScheme.SelectedValue.Trim = "" Then
        '    Me.udcMsgBox.AddMessage(New SystemMessage("990000", "E", "00236"))
        '    blnProceed = False
        '    Me.imgSchemeErr.Visible = True
        'End If

        'Check existence of temporary account and validated account
        If blnProceed Then

            Dim blnSameIDAccFoundButAllowProceed As Boolean = False
            'Remark: udtEHSAccount.EHSPersonalInformationList(0).DOB --> output format 99/99/9999 instead of 99-99-9999
            Me.udtSM = Me.udteHSAccountMaintBLL.SearchEHSAccountForBOAccountCreation(Me.txtDocCode.Text, _
                                                    udtEHSAccount.EHSPersonalInformationList(0).IdentityNum, _
                                                    blnSameIDAccFoundButAllowProceed, _
                                                    udtEHSAccount.EHSPersonalInformationList(0).AdoptionPrefixNum)
            If Me.udtSM IsNot Nothing Then
                Me.udcMsgBox.AddMessage(Me.udtSM)
                blnProceed = False
            End If
        End If


        If blnProceed Then
            'Fill in account information ---------------------------------------------------------------
            Dim udtHCVUUser As HCVUUserModel
            Dim udtHCVUUserBLL As New HCVUUserBLL
            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            udtEHSAccount.CreateByBO = True

            udtEHSAccount.SchemeCode = String.Empty
            udtEHSAccount.CreateSPID = String.Empty
            udtEHSAccount.CreateSPPracticeDisplaySeq = 0
            udtEHSAccount.CreateBy = udtHCVUUser.UserID
            udtEHSAccount.AccountPurpose = EHSAccountModel.AccountPurposeClass.ForClaim

            udtEHSAccount.DataEntryBy = String.Empty
            udtEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingVerify
            udtEHSAccount.EHSPersonalInformationList(0).DataEntryBy = String.Empty
            udtEHSAccount.EHSPersonalInformationList(0).RecordStatus = "N"
            udtEHSAccount.EHSPersonalInformationList(0).CreateBy = udtEHSAccount.CreateBy

            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            ' Fill Deceased Information
            If udtEHSAccount.DeathRecord.IsDead Then
                udtEHSAccount.Deceased = True
                udtEHSAccount.EHSPersonalInformationList(0).Deceased = True
                udtEHSAccount.EHSPersonalInformationList(0).DOD = udtEHSAccount.DeathRecord.DOD
                udtEHSAccount.EHSPersonalInformationList(0).ExactDOD = udtEHSAccount.DeathRecord.ExactDOD
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
                'Show data of death icon instead of redirect button
                blnShowDateOfDeathBtn = False
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]
            End If
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            '---------------------------------------------------------------------------------------------
            'Clear all error images on input control (when back from confirmation page)
            Me.ucInputDocumentType_NewAcc.ActiveViewChanged = True
            Me.imgSPIDErr.Visible = False
            Me.imgPracticeErr.Visible = False
            Me.imgSchemeErr.Visible = False

            udteHSAccountMaintBLL.EHSAccountSaveToSession(udtEHSAccount, FuncCode)

            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
            SetupConfirmAccount(udtEHSAccount, blnShowDateOfDeathBtn)
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]

            Me.mveHSAccount.ActiveViewIndex = intConfirm

            Me.udtAuditLogEntry.AddDescripton("DocCode", txtDocCode.Text.Trim)
            Me.udtAuditLogEntry.WriteEndLog(LogID.LOG00018, AuditLogDesc.ValidateAccountDetailInfoComplete)
        Else
            Me.udtAuditLogEntry.AddDescripton("DocCode", txtDocCode.Text.Trim)
            Me.udcMsgBox.BuildMessageBox(strValidationFail, Me.udtAuditLogEntry, LogID.LOG00019, AuditLogDesc.ValidateAccountDetailInfoFail)
        End If
    End Sub


    Protected Sub ibtnSearchSP_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        'Me.udcMsgBox.Clear()

        'If Me.txtEnterCreationDetailSPID.Text.Trim.Length = 8 Then
        '    Dim udtAccountChangeMaintBLL As New AccountChangeMaintenance.AccountChangeMaintenanceBLL

        '    Dim dt As DataTable
        '    dt = udtAccountChangeMaintBLL.MaintenanceSearch(String.Empty, Me.txtEnterCreationDetailSPID.Text.Trim, String.Empty, String.Empty, _
        '         String.Empty, String.Empty, String.Empty)

        '    If dt.Rows.Count > 0 Then
        '        GetReadyServiceProvider(CStr(dt.Rows(0)("SP_ID")))
        '    Else
        '        ShowModalPopupSearchSP()
        '    End If
        'Else
        '    ShowModalPopupSearchSP()
        'End If


        Me.udcMsgBox.Visible = False
        Me.udcInfoMsgBox.Visible = False
        Me.imgEnterCreationDetailSPIDError.Visible = False

        Dim sm As SystemMessage

        sm = Me.udtValidator.chkSPID(Me.txtEnterCreationDetailSPID.Text.Trim)

        If IsNothing(sm) Then
            If Me.txtEnterCreationDetailSPID.Text.Trim.Length = 8 Then
                Dim udtAccountChangeMaintBLL As New AccountChangeMaintenance.AccountChangeMaintenanceBLL

                Dim dt As DataTable
                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
                ' -------------------------------------------------------------------------
                'dt = udtAccountChangeMaintBLL.MaintenanceSearch(String.Empty, Me.txtEnterCreationDetailSPID.Text.Trim, String.Empty, String.Empty, _
                '     String.Empty, String.Empty, String.Empty)

                Dim udtBLLSearchResult As BaseBLL.BLLSearchResult

                ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
                udtBLLSearchResult = udtAccountChangeMaintBLL.MaintenanceSearch(FunctionCode, String.Empty, Me.txtEnterCreationDetailSPID.Text.Trim, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty)
                ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]

                dt = CType(udtBLLSearchResult.Data, DataTable)
                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

                If dt.Rows.Count = 0 Then
                    ' No Record Found
                    udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Information
                    udcInfoMsgBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
                    udcInfoMsgBox.BuildMessageBox()
                Else
                    GetReadyServiceProvider(CStr(dt.Rows(0)("SP_ID")))
                End If

            ElseIf Me.txtEnterCreationDetailSPID.Text.Trim.Length = 0 Then
                Me.udcInfoMsgAdvancedSearch.Visible = False
                Me.udcSystemMsgAdvancedSearch.Visible = False

                Me.txtAdvancedSearchHKIC.Text = String.Empty
                Me.txtAdvancedSearchName.Text = String.Empty
                Me.txtAdvancedSearchPhone.Text = String.Empty
                Me.txtAdvancedSearchSPID.Text = String.Empty

                Me.pnlAdvancedSearchCritieria.Visible = True
                Me.pnlAdvancedSearchResult.Visible = False

                Session.Remove(SESS_AdvancedSearchSP)

                ModalPopupSearchSP.Show()
            End If
        Else
            Me.imgEnterCreationDetailSPIDError.Visible = True
            Me.udcMsgBox.AddMessage(sm)
            Me.udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00064, AuditLogDesc.NewAccCreation_CreationDetailsClick)
        End If
    End Sub

    Private Sub GetReadyServiceProvider(ByVal strSPID)
        Dim udtSPBLL As New ServiceProvider.ServiceProviderBLL
        Dim udtSP As ServiceProvider.ServiceProviderModel

        udtSP = udtSPBLL.GetServiceProviderPermanentProfileWithMaintenanceBySPID(strSPID, New Common.DataAccess.Database)

        Me.lblEnterCreationDetailSPName.Text = udtSP.EnglishName
        'Status.GetDescriptionFromDBCode(ServiceProviderStatus.ClassCode, udtSP.RecordStatus, Me.lblEnterCreationDetailSPStatus.Text, String.Empty)

        Dim dtPracticeList = getAllPractice(udtSP.SPID, Practice.PracticeBLL.PracticeDisplayType.Practice)
        Dim udtPracticeBLL As New Practice.PracticeBLL

        Me.udtSessionHandlerBLL.PracticeDisplayListRemoveFromSession(FunctionCode)
        Me.udtSessionHandlerBLL.PracticeDisplayListSaveToSession(udtPracticeBLL.convertPractice(dtPracticeList), FunctionCode)

        Me.ddlEnterCreationDetailPractice.DataSource = dtPracticeList

        Me.ddlEnterCreationDetailPractice.DataTextField = Practice.PracticeBLL.PracticeDisplayField.Display_Eng
        Me.ddlEnterCreationDetailPractice.DataValueField = "PracticeID"
        Me.ddlEnterCreationDetailPractice.DataBind()

        ddlEnterCreationDetailPractice.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), ""))

        ddlEnterCreationDetailPractice.SelectedIndex = 0

        Me.ddlEnterCreationDetailPractice.Enabled = True

        Me.ibtnSearchSP.Enabled = False
        Me.ibtnClearSearchSP.Enabled = True

        Me.ibtnSearchSP.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchDisableSBtn")
        Me.ibtnClearSearchSP.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ClearSBtn")

        Me.txtEnterCreationDetailSPID.Text = udtSP.SPID
        Me.txtEnterCreationDetailSPID.Enabled = False
    End Sub

    Private Sub ShowModalPopupSearchSP()
        Me.udcInfoMsgAdvancedSearch.Visible = False
        Me.udcSystemMsgAdvancedSearch.Visible = False

        Me.txtAdvancedSearchHKIC.Text = String.Empty
        Me.txtAdvancedSearchName.Text = String.Empty
        Me.txtAdvancedSearchPhone.Text = String.Empty
        Me.txtAdvancedSearchSPID.Text = String.Empty

        Me.pnlAdvancedSearchCritieria.Visible = True
        Me.pnlAdvancedSearchResult.Visible = False

        Session.Remove(SESS_AdvancedSearchSP)

        ModalPopupSearchSP.Show()
    End Sub


#Region "(Not Used : As SP information is not required for account creation by back office) "
    Protected Sub ibtnAdvancedSearchSP_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        Dim strHKIC As String = String.Empty

        If Me.txtAdvancedSearchHKIC.Text.Trim.Equals(String.Empty) AndAlso Me.txtAdvancedSearchName.Text.Trim.Equals(String.Empty) AndAlso _
           Me.txtAdvancedSearchPhone.Text.Trim.Equals(String.Empty) AndAlso Me.txtAdvancedSearchSPID.Text.Trim.Equals(String.Empty) Then
            Me.ModalPopupSearchSP.Show()
            udtSM = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00257)
            Me.udcSystemMsgAdvancedSearch.AddMessage(udtSM)
            Me.udcSystemMsgAdvancedSearch.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00063, AuditLogDesc.NewAccCreation_SearchAccountClick)

        Else
            Dim blnKeywordSearch As Boolean = False
            If Not Me.txtAdvancedSearchSPID.Text.Trim.Equals(String.Empty) AndAlso Me.txtAdvancedSearchSPID.Text.Trim.Length = 8 Then
                blnKeywordSearch = True
                strHKIC = Me.udtformatter.formatHKID(Me.txtAdvancedSearchSPID.Text, False)
            Else
                strHKIC = Me.txtAdvancedSearchSPID.Text
            End If

            If Not Me.txtAdvancedSearchHKIC.Text.Trim.Equals(String.Empty) Then
                udtSM = Me.udtValidator.chkIdentityNumber(DocTypeModel.DocTypeCode.HKIC, Me.txtAdvancedSearchHKIC.Text.Trim, String.Empty)
                If IsNothing(udtSM) Then
                    blnKeywordSearch = True
                Else
                    Me.ModalPopupSearchSP.Show()
                    Me.udcSystemMsgAdvancedSearch.AddMessage(udtSM)
                    Me.udcSystemMsgAdvancedSearch.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00063, AuditLogDesc.NewAccCreation_SearchAccountClick)
                    Exit Sub
                End If

            End If

            Dim udtAccountChangeMaintBLL As New AccountChangeMaintenance.AccountChangeMaintenanceBLL

            Dim dt As DataTable
            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
            ' -------------------------------------------------------------------------
            'dt = udtAccountChangeMaintBLL.MaintenanceSearch(String.Empty, Me.txtAdvancedSearchSPID.Text.Trim, Me.txtAdvancedSearchHKIC.Text.Trim, _
            '                                                Me.txtAdvancedSearchName.Text.Trim, Me.txtAdvancedSearchPhone.Text.Trim, String.Empty, String.Empty)

            Dim udtBLLSearchResult As BaseBLL.BLLSearchResult

            ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
            udtBLLSearchResult = udtAccountChangeMaintBLL.MaintenanceSearch(FunctionCode, String.Empty, Me.txtAdvancedSearchSPID.Text.Trim, Me.txtAdvancedSearchHKIC.Text.Trim, _
                                                                            Me.txtAdvancedSearchName.Text.Trim, String.Empty, Me.txtAdvancedSearchPhone.Text.Trim, String.Empty, String.Empty)
            ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]

            dt = CType(udtBLLSearchResult.Data, DataTable)
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

            If dt.Rows.Count = 0 Then
                Me.ModalPopupSearchSP.Show()
                Me.pnlAdvancedSearchCritieria.Visible = True
                Me.pnlAdvancedSearchResult.Visible = False

                udcSystemMsgAdvancedSearch.Clear()
                Me.udcInfoMsgAdvancedSearch.Type = CustomControls.InfoMessageBoxType.Information
                udcInfoMsgAdvancedSearch.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
                udcInfoMsgAdvancedSearch.BuildMessageBox()
            ElseIf dt.Rows.Count = 1 AndAlso blnKeywordSearch Then
                'Return to Enter Creation Details and close the popup
                Me.GetReadyServiceProvider(CStr(dt.Rows(0)("SP_ID")))
                Me.ModalPopupSearchSP.Hide()

                Session.Remove(SESS_AdvancedSearchSP)
            Else
                Me.ModalPopupSearchSP.Show()

                If Me.txtAdvancedSearchSPID.Text.Trim.Equals(String.Empty) Then
                    Me.lblAdvancedSearchResultSPID.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    Me.lblAdvancedSearchResultSPID.Text = Me.txtAdvancedSearchSPID.Text.Trim
                End If

                If Me.txtAdvancedSearchHKIC.Text.Trim.Equals(String.Empty) Then
                    Me.lblAdvancedSearchResultHKIC.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    Me.lblAdvancedSearchResultHKIC.Text = strHKIC
                End If

                If Me.txtAdvancedSearchName.Text.Trim.Equals(String.Empty) Then
                    Me.lblAdvancedSearchResultName.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    Me.lblAdvancedSearchResultName.Text = Me.txtAdvancedSearchName.Text.Trim
                End If

                If Me.txtAdvancedSearchPhone.Text.Trim.Equals(String.Empty) Then
                    Me.lblAdvancedSearchResultPhone.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    Me.lblAdvancedSearchResultPhone.Text = Me.txtAdvancedSearchPhone.Text.Trim
                End If


                Me.pnlAdvancedSearchCritieria.Visible = False
                Me.pnlAdvancedSearchResult.Visible = True

                Session(SESS_AdvancedSearchSP) = dt
                Me.GridViewDataBind(gvAdvancedSearchSP, dt, "SP_ID", "ASC", False)


            End If

        End If
    End Sub

    Protected Sub ibtnAdvancedSearchSPClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.ModalPopupSearchSP.Hide()
        Session.Remove(SESS_AdvancedSearchSP)
    End Sub

    Protected Sub ibtnAdvancedSearchResultBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.pnlAdvancedSearchCritieria.Visible = True
        Me.pnlAdvancedSearchResult.Visible = False
    End Sub

    Private Sub gvAdvancedSearchSP_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvAdvancedSearchSP.PageIndexChanging
        Me.GridViewPageIndexChangingHandler(sender, e, SESS_AdvancedSearchSP)
        Me.ModalPopupSearchSP.Show()
    End Sub

    Private Sub gvAdvancedSearchSP_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvAdvancedSearchSP.PreRender
        Me.GridViewPreRenderHandler(sender, e, SESS_AdvancedSearchSP)
        Me.ModalPopupSearchSP.Show()
    End Sub

    Private Sub gvAdvancedSearchSP_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvAdvancedSearchSP.RowCommand
        If TypeOf e.CommandSource Is LinkButton Then
            Dim strSPID As String = String.Empty

            Dim strCommandArgument As String = e.CommandArgument.ToString.Trim
            strSPID = strCommandArgument
            Me.GetReadyServiceProvider(strSPID)

            Session.Remove(SESS_AdvancedSearchSP)

            Me.ModalPopupSearchSP.Hide()
        End If
    End Sub

    Private Sub gvAdvancedSearchSP_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAdvancedSearchSP.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblAdvancedSearchCname As Label = CType(e.Row.FindControl("lblAdvancedSearchCname"), Label)
            lblAdvancedSearchCname.Text = udtformatter.formatChineseName(lblAdvancedSearchCname.Text.Trim)
        End If
    End Sub

    Protected Sub ibtnClearSearchSP_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Me.udcMsgBox.Clear()

        Me.txtEnterCreationDetailSPID.Enabled = True
        Me.txtEnterCreationDetailSPID.Text = String.Empty
        Me.lblEnterCreationDetailSPName.Text = String.Empty
        Me.ddlEnterCreationDetailPractice.Items.Clear()
        Me.ddlEnterCreationDetailPractice.Enabled = False

        Me.ibtnSearchSP.Enabled = True
        Me.ibtnClearSearchSP.Enabled = False

        Me.ibtnSearchSP.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchSBtn")
        Me.ibtnClearSearchSP.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ClearDisableSBtn")

    End Sub



    Protected Sub cboCreateByDH_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If cboCreateByDH.Checked Then
            Me.ibtnSearchSP.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchSDisableBtn")
            Me.ibtnClearSearchSP.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ClearDisableSBtn")
            Me.ibtnSearchSP.Enabled = False
            Me.ibtnClearSearchSP.Enabled = False
            Me.txtEnterCreationDetailSPID.Text = ""
            Me.txtEnterCreationDetailSPID.Enabled = False
            Me.lblEnterCreationDetailSPName.Text = ""
            Me.ddlEnterCreationDetailPractice.Enabled = False
            Me.ddlEnterCreationDetailPractice.Items.Clear()
        Else
            Me.ibtnSearchSP.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchSBtn")
            Me.ibtnClearSearchSP.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ClearDisableSBtn")
            Me.ibtnSearchSP.Enabled = True
            Me.ibtnClearSearchSP.Enabled = True
            Me.txtEnterCreationDetailSPID.Enabled = True
            Me.ddlEnterCreationDetailPractice.Enabled = True
        End If

    End Sub

    Private Function getAllPractice(ByVal strSPID As String, ByVal enumPracticeDisplayType As Practice.PracticeBLL.PracticeDisplayType) As DataTable

        Dim udtPracticeBLL As New Practice.PracticeBLL
        ' Get Practice Information
        Dim dtPractice As DataTable = udtPracticeBLL.getRawAllPracticeBankAcct(strSPID)

        Practice.PracticeBLL.ConcatePracticeDisplayColumn(dtPractice, Practice.PracticeBLL.PracticeDisplayType.Practice)

        Return dtPractice
    End Function
#End Region

#End Region

#Region "Muilt View Function"

    Private Sub mveHSAccount_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mveHSAccount.ActiveViewChanged
        udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        udtEHSAccount_Amendment = Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FunctionCode)


        Select Case Me.mveHSAccount.ActiveViewIndex
            Case intSearchView
                Me.udcMsgBox.Clear()
                Me.udcInfoMsgBox.Clear()

                Dim actionMode As EditAccountModel
                actionMode = Session(SESS_ActionMode)
                If Not actionMode = EditAccountModel.NewAccount Then
                    ClearSession(True)
                End If

                ClearErrorImage()

                Me.lblConfirmReasonText.Text = String.Empty
                Me.lblConfirmReason.Text = String.Empty

            Case intSearchResult
                Me.udcMsgBox.Clear()
                Me.udcInfoMsgBox.Clear()

                Session.Remove(SESS_ActionMode) ' CRE11-007

                ClearSession(False)

                If tcSearchRoute.ActiveTabIndex = 0 Then
                    '    ' Search Route 1
                    '    Me.ibtnRReactivateSelectedAccount.Visible = False
                    '    Me.ibtnRSuspendSelectedAccount.Visible = False
                    '    Me.ibtnRTerminateSelectedAccount.Visible = False

                    ' Search Route 2
                    Me.ibtnRReactivateSelectedAccount.Visible = True
                    Me.ibtnRSuspendSelectedAccount.Visible = True
                    Me.ibtnRTerminateSelectedAccount.Visible = True
                Else
                    '    ' Search Route 2
                    '    Me.ibtnRReactivateSelectedAccount.Visible = True
                    '    Me.ibtnRSuspendSelectedAccount.Visible = True
                    '    Me.ibtnRTerminateSelectedAccount.Visible = True

                    ' other route
                    Me.ibtnRReactivateSelectedAccount.Visible = False
                    Me.ibtnRSuspendSelectedAccount.Visible = False
                    Me.ibtnRTerminateSelectedAccount.Visible = False
                End If

                Me.lblConfirmReasonText.Text = String.Empty
                Me.lblConfirmReason.Text = String.Empty
                ' CRE11-007
                Me.lblASActionReasonText.Text = String.Empty
                Me.txtASActionReason.Text = String.Empty

                ' CRE11-007
                ' Unmask HKID checkbox
                'chkMaskDocumentNoR1.Checked = True
                chkMaskDocumentNoR2.Checked = True
                chkMaskDocumentNoR3.Checked = True
                'chkMaskDocumentNoR1.Visible = (New GeneralFunction).CheckTurnOnInstantUnmaskIdentityDocumentNo = GeneralFunction.EnumTurnOnStatus.Yes
                chkMaskDocumentNoR2.Visible = (New GeneralFunction).CheckTurnOnInstantUnmaskIdentityDocumentNo = GeneralFunction.EnumTurnOnStatus.Yes
                chkMaskDocumentNoR3.Visible = (New GeneralFunction).CheckTurnOnInstantUnmaskIdentityDocumentNo = GeneralFunction.EnumTurnOnStatus.Yes
                'Me.gvAcctListR1.Columns(2).Visible = True
                'Me.gvAcctListR1.Columns(3).Visible = False

                Me.gvAcctListR2.Columns(3).Visible = True
                Me.gvAcctListR2.Columns(4).Visible = False

                Me.gvAcctListR3.Columns(3).Visible = True
                Me.gvAcctListR3.Columns(4).Visible = False

            Case intAccountDetails
                'Me.udcInfoMsgBox.Clear()

                Session(SESS_ActionMode) = Nothing
                Session.Remove(SESS_ActionMode)

                Dim actionMode As ActionModel
                actionMode = Session(SESS_InputMode)
                SetAccountBtn(actionMode)
                If actionMode = ActionModel.Amending Then
                    Me.SetPanelAction(EditAccountModel.Amending)
                Else
                    Me.SetPanelAction(EditAccountModel.None)
                End If
                Me.SetupInputDocControl(udtEHSAccount, udtEHSAccount_Amendment, True)

                Me.lblConfirmReasonText.Text = String.Empty
                Me.lblConfirmReason.Text = String.Empty

                ' Reset confirm view for single account
                panConfrimBatchAccount.Visible = False
                udcConfirmAccount.Visible = True
            Case intNewAccount
                ' CRE11-007
                panConfrimBatchAccount.Visible = False
                udcConfirmAccount.Visible = True
            Case intConfirm
                Me.udcMsgBox.Clear()

                chkMaskDocumentNoAC.Visible = (New GeneralFunction).CheckTurnOnInstantUnmaskIdentityDocumentNo = GeneralFunction.EnumTurnOnStatus.Yes
                ' Default mask status follow save view 
                MaskBatchActionDocumentNo(chkMaskDocumentNoAS.Checked)

                udtSM = New SystemMessage(CommonFunctionCode, SeverityCode.SEVI, MsgCode.MSG00021)
                Me.udcInfoMsgBox.AddMessage(udtSM)
                Me.udcInfoMsgBox.BuildMessageBox()
                Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Information

            Case intComplete
                Me.udcMsgBox.Clear()

            Case intAmendmentHistroy
                'By Paul, added on 22/9 (Trigger the binding action on readonly control in Amendment History page)
                Me.udcInfoMsgBox.Clear()
                Me.SetupInputDocControl(udtEHSAccount, udtEHSAccount_Amendment, True)

            Case intSave ' Batch Account Action
                chkMaskDocumentNoAS.Visible = (New GeneralFunction).CheckTurnOnInstantUnmaskIdentityDocumentNo = GeneralFunction.EnumTurnOnStatus.Yes
                ' Default mask when go to save view
                MaskBatchActionDocumentNo(True)
        End Select
    End Sub

#End Region

#Region "Set Controls"

    Private Sub ResetControls()

        'Me.ddlSearchAcctTypeR1.Items.Clear()
        'Me.ddlSearchAcctTypeR1.DataSource = Common.Component.Status.GetDescriptionListFromDBEnumCode(VRAcctMaintenanceStatus.ClassCode, True)
        'Me.ddlSearchAcctTypeR1.DataTextField = "Status_Description"
        'Me.ddlSearchAcctTypeR1.DataValueField = "Status_Value"
        'Me.ddlSearchAcctTypeR1.DataBind()

        'ddlSearchAcctTypeR1.SelectedIndex = 0

        'Route 2
        Me.ddlSearchAcctTypeR2.Items.Clear()
        Me.ddlSearchAcctTypeR2.DataSource = Common.Component.Status.GetDescriptionListFromDBEnumCode(VRAcctMaintenanceStatus.ClassCode, True)
        Me.ddlSearchAcctTypeR2.DataTextField = "Status_Description"
        Me.ddlSearchAcctTypeR2.DataValueField = "Status_Value"
        Me.ddlSearchAcctTypeR2.DataBind()
        Me.ddlSearchAcctTypeR2.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))
        Me.ddlSearchAcctTypeR2.SelectedIndex = 0

        Me.ddlSearchTempAcct.Items.Clear()

        'If ddlSearchAcctTypeR1.SelectedValue = VRAcctMaintenanceStatus.Special Then
        '    Dim dv As New DataView(Common.Component.Status.GetDescriptionListFromDBEnumCode(TempAcctMaintenanceStatus.ClassCode, True))
        '    dv.RowFilter = "Status_Value <>'" + TempAcctMaintenanceStatus.PendingRemove.Trim + "' and Status_Value <>'" + TempAcctMaintenanceStatus.OutstandingValidation.Trim + "'"

        '    Me.ddlSearchTempAcct.DataSource = dv
        'Else
        '    Me.ddlSearchTempAcct.DataSource = Common.Component.Status.GetDescriptionListFromDBEnumCode(TempAcctMaintenanceStatus.ClassCode, True)
        'End If

        Me.ddlSearchTempAcct.DataSource = Common.Component.Status.GetDescriptionListFromDBEnumCode(TempAcctMaintenanceStatusByParticular.ClassCode, True)

        Me.ddlSearchTempAcct.DataTextField = "Status_Description"
        Me.ddlSearchTempAcct.DataValueField = "Status_Value"
        Me.ddlSearchTempAcct.DataBind()

        Me.ddlSearchTempAcct.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))
        Me.ddlSearchTempAcct.SelectedIndex = 0

        Dim udtDocTypeBLL As DocTypeBLL = New DocTypeBLL
        Me.ddlSearchDocTypeR2.Items.Clear()
        Me.ddlSearchDocTypeR2.DataSource = udtDocTypeBLL.getAllDocType
        Me.ddlSearchDocTypeR2.DataTextField = "DocName"
        Me.ddlSearchDocTypeR2.DataValueField = "DocCode"
        Me.ddlSearchDocTypeR2.DataBind()
        Me.ddlSearchDocTypeR2.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))
        Me.ddlSearchDocTypeR2.SelectedIndex = 0

        ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
        'txtSearchENameR1.Text = String.Empty
        'txtSearchCreationDateFromR1.Text = String.Empty
        'txtSearchCreationDateToR1.Text = String.Empty
        txtSearchENameR2.Text = String.Empty
        txtSearchCNameR2.Text = String.Empty
        txtSearchCreationDateFromR2.Text = String.Empty
        txtSearchCreationDateToR2.Text = String.Empty
        txtSearchIdentityNumR2.Text = String.Empty
        txtSearchDOBR2.Text = String.Empty
        txtSearchAccountIDR2.Text = String.Empty
        txtSearchRefNo.Text = String.Empty
        ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]

        'If ddlSearchAcctTypeR1.SelectedValue.Trim = VRAcctMaintenanceStatus.Temporary _
        '    OrElse ddlSearchAcctTypeR1.SelectedValue.Trim = VRAcctMaintenanceStatus.Special Then
        '    Me.pnlAdvTempSearch.Visible = True
        'Else
        '    Me.pnlAdvTempSearch.Visible = False
        'End If

        If ddlSearchAcctTypeR2.SelectedValue.Trim = VRAcctMaintenanceStatus.Temporary Then
            Me.pnlAdvTempSearchR2.Visible = True
        Else
            Me.pnlAdvTempSearchR2.Visible = False
        End If

        'Route 3(Manual Validation)
        'SPID
        txtSPIDR3.Text = String.Empty

        'Creation Date
        txtCreationDateFromR3.Text = String.Empty
        txtCreationDateToR3.Text = String.Empty

        'Manual Validation
        ddlManualValidStatusR3.Items.Clear()
        ddlManualValidStatusR3.DataSource = Common.Component.Status.GetDescriptionListFromDBEnumCode(TempAcctMaintenanceStatusByManualValidation.ClassCode, True)
        ddlManualValidStatusR3.DataTextField = "Status_Description"
        ddlManualValidStatusR3.DataValueField = "Status_Value"
        ddlManualValidStatusR3.DataBind()
        ddlManualValidStatusR3.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))
        ddlManualValidStatusR3.SelectedIndex = 0

        Dim udtStaticDataList As StaticData.StaticDataModelCollection = (New StaticData.StaticDataBLL).GetStaticDataListByColumnName("YesNo")

        'WithClaims
        ddlWithClaimsR3.DataSource = udtStaticDataList
        ddlWithClaimsR3.DataValueField = "ItemNo"
        ddlWithClaimsR3.DataTextField = "DataValue"
        ddlWithClaimsR3.DataBind()
        ddlWithClaimsR3.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), String.Empty))
        ddlWithClaimsR3.SelectedIndex = 0

        'Scheme
        Dim udtSchemeClaimModelListFilter As New SchemeClaimModelCollection
        Dim udtUserRoleCollection As UserRole.UserRoleModelCollection = (New UserRole.UserRoleBLL).GetUserRoleCollection((New HCVUUser.HCVUUserBLL).GetHCVUUser.UserID)
        Dim udtSchemeCList As SchemeClaimModelCollection = udtSchemeClaimBLL.getAllDistinctSchemeClaim()

        For Each udtSchemeC As SchemeClaimModel In udtSchemeCList
            For Each udtUserRoleModel As UserRole.UserRoleModel In udtUserRoleCollection.Values
                If udtUserRoleModel.SchemeCode.Trim = udtSchemeC.SchemeCode Then
                    If Not udtSchemeClaimModelListFilter.Contains(udtSchemeC) Then udtSchemeClaimModelListFilter.Add(udtSchemeC)
                End If
            Next
        Next
        ddlSchemeR3.DataSource = udtSchemeClaimModelListFilter
        ddlSchemeR3.DataValueField = "SchemeCode"
        ddlSchemeR3.DataTextField = "DisplayCode"
        ddlSchemeR3.DataBind()

        txtSPIDR3.Text = String.Empty
        txtCreationDateFromR3.Text = String.Empty
        txtCreationDateToR3.Text = String.Empty


        ' Set the scheme list to disabled if only 1 scheme
        If udtSchemeClaimModelListFilter.Count = 1 Then
            ddlSchemeR3.SelectedIndex = 1
            'ddlSchemeR3.Enabled = False
        Else
            ddlSchemeR3.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))
            ddlSchemeR3.SelectedIndex = 0
            'ddlSchemeR3.Enabled = True
        End If

        'Deceased
        ddlDeceasedR3.DataSource = udtStaticDataList
        ddlDeceasedR3.DataValueField = "ItemNo"
        ddlDeceasedR3.DataTextField = "DataValue"
        ddlDeceasedR3.DataBind()
        ddlDeceasedR3.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), String.Empty))
        ddlDeceasedR3.SelectedIndex = 0

        'Date of Death
        btnDateofDeathFromR3.Enabled = False
        btnDateofDeathToR3.Enabled = False
        txtDateofDeathFromR3.Text = String.Empty
        txtDateofDeathToR3.Text = String.Empty

        'Account Type R3
        Me.ddlAcctTypeR3.Items.Clear()
        Dim dtAcctTypeR3 As DataTable = Common.Component.Status.GetDescriptionListFromDBEnumCode(VRAcctMaintenanceStatus.ClassCode, True)
        Me.ddlAcctTypeR3.DataSource = dtAcctTypeR3.AsEnumerable().Where(Function(r) _
                                    r.Field(Of String)("Status_Value") = VRAcctMaintenanceStatus.Temporary Or _
                                    r.Field(Of String)("Status_Value") = VRAcctMaintenanceStatus.Special).CopyToDataTable
        Me.ddlAcctTypeR3.DataTextField = "Status_Description"
        Me.ddlAcctTypeR3.DataValueField = "Status_Value"
        Me.ddlAcctTypeR3.DataBind()
        Me.ddlAcctTypeR3.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))
        Me.ddlAcctTypeR3.SelectedIndex = 0

        ' eHealth Account Prefix
        Dim strParm1 As String = String.Empty
        Dim strParm2 As String = String.Empty
        If udtCommonFunction.getSystemParameter("eHealthAccountPrefix", strParm1, strParm2) Then
            lblSearchAccountIDR2Prefix.Text = strParm1
        Else
            Throw New ArgumentNullException("Parameter: eHealthAccountPrefix not found")
        End If

        ' CRE11-007
        ' eHealth Account ID length
        Me.txtSearchAccountIDR2.MaxLength = 10 * Me.udtCommonFunction.GetPageSize()

        ' Turn On / Off the making new claim function
        Dim strTurnOnNewClaim As String = String.Empty
        udtCommonFunction.getSystemParameter("TurnOnOutsidePaymentClaim", strTurnOnNewClaim, String.Empty)

        If strTurnOnNewClaim.Trim.Equals("Y") Then
            Me.ibtnNewAccountR2.Visible = True
            ''If Me.tcSearchRoute.ActiveTabIndex = 1 Then
            'If Me.tcSearchRoute.ActiveTabIndex = 0 Then
            '    'Route 2
            '    Me.ibtnNewAccount.Visible = True
            'Else
            '    Me.ibtnNewAccount.Visible = False
            'End If
        Else
            Me.ibtnNewAccountR2.Visible = False
        End If

    End Sub

    Private Sub ResetAccountCreationControls()
        Me.udcInfoMsgBox.Clear()
        Me.udcMsgBox.Clear()

        udcStep1DocumentTypeRadioButtonGroup.ShowAllSelection = True
        Me.udcStep1DocumentTypeRadioButtonGroup.ShowLegend = False
        Me.udcStep1DocumentTypeRadioButtonGroup.SelectedValue = DocTypeModel.DocTypeCode.HKIC
        udcStep1DocumentTypeRadioButtonGroup.Build()

        'Bind Scheme drop down list
        Dim dtSchemeList As DataTable = getSchemeList()
        Me.ddlEnterCreationDetailScheme.DataSource = dtSchemeList
        Me.ddlEnterCreationDetailScheme.DataTextField = "Description"
        Me.ddlEnterCreationDetailScheme.DataValueField = "SchemeCode"
        Me.ddlEnterCreationDetailScheme.DataBind()
        Me.ddlEnterCreationDetailScheme.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), ""))
        Me.ddlEnterCreationDetailScheme.SelectedIndex = 0

        'Clear the controls
        Me.lblEnterCreationDetailSPName.Text = String.Empty
        Me.txtEnterCreationDetailSPID.Text = String.Empty
        Me.txtEnterCreationDetailSPID.Enabled = True
        Me.cboCreateByDH.Checked = False
        Me.ddlEnterCreationDetailScheme.SelectedIndex = 0
        Me.ddlEnterCreationDetailPractice.ClearSelection()
        Me.ddlEnterCreationDetailPractice.Enabled = False
        Me.ibtnSearchSP.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchSBtn")
        Me.ibtnClearSearchSP.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ClearDisableSBtn")
        Me.ibtnSearchSP.Enabled = True
        Me.ibtnClearSearchSP.Enabled = False

        Me.imgEnterCreationDetailSPIDError.Visible = False
        Me.imgSPIDErr.Visible = False
        Me.imgPracticeErr.Visible = False
        Me.imgSchemeErr.Visible = False
    End Sub

    Private Sub SetAccountBtn(ByVal ActionMode As ActionModel)

        Me.ibtnAmendHistory.Visible = False
        Me.ibtnMarkAsImmDValid.Visible = False
        Me.ibtnReleaseForRect.Visible = False
        Me.ibtnConfirmAsValidAcct.Visible = False
        Me.ibtnRemove.Visible = False
        Me.ibtnCancelValidation.Visible = False
        Me.ibtnRemoveTempAccountByBO.Visible = False

        Me.ibtnSuspend.Visible = False
        Me.ibtnTerminate.Visible = False
        Me.itbnReactivate.Visible = False

        Me.ibtnSuspendEnquiry.Visible = False
        Me.ibtnReactiveEnquiry.Visible = False

        Me.ibtnAmendRecord.Visible = False
        Me.ibtnWithdrawAmendment.Visible = False

        Dim blnManualCheckingToImmd As Nullable(Of Boolean) = Nothing
        Dim blnShowEnquiry As Boolean = False
        Dim blnAmend As Boolean = False

        Me.pnlTransactionInfo.Visible = False
        Me.pnlAmendingSmartID.Visible = False

        udtEHSAccount = udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        Dim udteHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel
        udteHSAccountPersonalInfo = udtEHSAccount.getPersonalInformation(Me.txtDocCode.Text.Trim)

        Select Case udtEHSAccount.AccountSource
            Case EHSAccountModel.SysAccountSource.ValidateAccount
                ' Amendment History Btn (Only Show when account is validated)
                Me.ibtnAmendHistory.Visible = True

                '==================================================================== Code for SmartID ============================================================================
                If Me.udteHSAccountMaintBLL.IsAmendingBySmartID(udtEHSAccount.VoucherAccID, udteHSAccountPersonalInfo.DocCode) Then
                    Me.pnlAmendingSmartID.Visible = False
                Else
                    Me.pnlAmendingSmartID.Visible = False
                End If
                '==================================================================================================================================================================

                Select Case udtEHSAccount.RecordStatus.Trim
                    Case EHSAccountModel.ValidatedAccountRecordStatusClass.Active
                        Me.ibtnSuspend.Visible = True
                        Me.ibtnTerminate.Visible = True
                        Me.itbnReactivate.Visible = False

                        blnShowEnquiry = True
                        blnAmend = True


                    Case EHSAccountModel.ValidatedAccountRecordStatusClass.Suspended

                        Me.ibtnSuspend.Visible = False
                        Me.ibtnTerminate.Visible = True
                        Me.itbnReactivate.Visible = True

                        blnShowEnquiry = True
                        blnAmend = True

                        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                        ' -----------------------------------------------------------------------------------------
                    Case EHSAccountModel.ValidatedAccountRecordStatusClass.Terminated
                        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                        Me.ibtnSuspend.Visible = False
                        Me.ibtnTerminate.Visible = False
                        Me.itbnReactivate.Visible = False

                        blnShowEnquiry = False
                        blnAmend = False

                End Select

                If blnAmend Then
                    If udteHSAccountPersonalInfo.RecordStatus.Trim.Equals("U") Then
                        Me.ibtnAmendRecord.Visible = False
                        If udteHSAccountPersonalInfo.Validating Then
                            Me.ibtnWithdrawAmendment.Visible = False
                        Else
                            Me.ibtnWithdrawAmendment.Visible = True
                        End If

                    Else
                        Me.ibtnAmendRecord.Visible = True
                        Me.ibtnWithdrawAmendment.Visible = False
                    End If
                Else
                    Me.ibtnAmendRecord.Visible = False
                    Me.ibtnWithdrawAmendment.Visible = False
                End If

                If blnShowEnquiry Then
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
                    If udtEHSAccount.Deceased Then
                        Me.ibtnSuspendEnquiry.Enabled = False
                        ibtnSuspendEnquiry.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SuspendEnquiryDisableBtn")
                        Me.ibtnSuspendEnquiry.Visible = True
                        Me.ibtnReactiveEnquiry.Visible = False
                    Else
                        Me.ibtnSuspendEnquiry.Enabled = True
                        ibtnSuspendEnquiry.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SuspendEnquiryBtn")
                        If udtEHSAccount.PublicEnquiryStatus.Trim.Equals(EHSAccountModel.EnquiryStatusClass.Available) Then
                            Me.ibtnSuspendEnquiry.Visible = True
                            Me.ibtnReactiveEnquiry.Visible = False
                        ElseIf udtEHSAccount.PublicEnquiryStatus.Trim.Equals(EHSAccountModel.EnquiryStatusClass.ManualSuspend) Then
                            Me.ibtnSuspendEnquiry.Visible = False
                            Me.ibtnReactiveEnquiry.Visible = True
                        ElseIf udtEHSAccount.PublicEnquiryStatus.Trim.Equals(EHSAccountModel.EnquiryStatusClass.AutomaticSuspend) Then
                            Me.ibtnSuspendEnquiry.Visible = False
                            Me.ibtnReactiveEnquiry.Visible = True
                        End If
                    End If
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]

                End If

            Case EHSAccountModel.SysAccountSource.TemporaryAccount
                'Amendment History Btn
                Me.ibtnAmendHistory.Visible = False
                Me.ibtnAmendRecord.Visible = False
                Me.ibtnWithdrawAmendment.Visible = False

                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
                If udtEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingVerify Then
                    ' Manual Checking to Immd
                    blnManualCheckingToImmd = udtValidator.chkManualValidation(Me.txtDocCode.Text.Trim, udteHSAccountPersonalInfo)

                End If

                pnlTransactionInfo.Visible = True
                If udtEHSAccount.TransactionID IsNot Nothing Then
                    If Not udtEHSAccount.TransactionID.Trim.Equals(String.Empty) Then

                        Me.lblTransactionID.Text = udtformatter.formatSystemNumber(udtEHSAccount.TransactionID)
                    Else
                        Me.lblTransactionID.Text = Me.GetGlobalResourceObject("Text", "N/A")
                    End If
                Else
                    Me.lblTransactionID.Text = Me.GetGlobalResourceObject("Text", "N/A")
                End If
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]

                ' CRE20-003-03 Enhancement on Programme or Scheme using batch upload [Start][Winnie] (Allow remove account from batch)
                ' ---------------------------------------------------------------------------------------------------------
                'Show "Remove" button
                '1. Record Status <> "D", "V" 
                '2. ImmD checked and date difference between checking date and current date > 28 days
                If udtEHSAccount.RecordStatus <> TempAccountRecordStatusClass.Removed _
                    AndAlso udtEHSAccount.RecordStatus <> TempAccountRecordStatusClass.Validated _
                    AndAlso udtEHSAccount.FirstValidateDtm.HasValue _
                    AndAlso DateDiff(DateInterval.Day, udtEHSAccount.FirstValidateDtm.Value, Now, Microsoft.VisualBasic.FirstDayOfWeek.Sunday, FirstWeekOfYear.System) > 28 _
                    Then
                    Me.ibtnRemove.Visible = True
                End If

                If blnManualCheckingToImmd.HasValue Then
                    If blnManualCheckingToImmd.Value Then
                        ' Mark Sending to Immd for Vaidation
                        Me.ibtnMarkAsImmDValid.Visible = True
                        Me.ibtnReleaseForRect.Visible = False
                        Me.ibtnConfirmAsValidAcct.Visible = False
                    Else
                        ' Release for rectification or Confirm as a valid account after Immd Checking Manually
                        If udteHSAccountMaintBLL.IsValidatingByMannual(udtEHSAccount.VoucherAccID.Trim) Then
                            Me.ibtnReleaseForRect.Visible = True
                            Me.ibtnConfirmAsValidAcct.Visible = True
                        Else
                            Me.ibtnReleaseForRect.Visible = False
                            Me.ibtnConfirmAsValidAcct.Visible = False
                        End If

                        Me.ibtnMarkAsImmDValid.Visible = False
                    End If

                End If

                'Show "Remove" button
                '1. The account is created by back office
                '2. Record Status = "P" (Pending Validation)
                '3. Account Purpose <> "A","O"
                '4. Without validation as before
                If udtEHSAccount.CreateByBO _
                    And udtEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingVerify _
                    And Not (udtEHSAccount.AccountPurpose = EHSAccountModel.AccountPurposeClass.ForAmendmentOld Or udtEHSAccount.AccountPurpose = EHSAccountModel.AccountPurposeClass.ForAmendment) _
                    And Not udteHSAccountPersonalInfo.Validating _
                Then
                    If Me.ibtnRemove.Visible = False Then
                        Me.ibtnRemoveTempAccountByBO.Visible = True
                    End If
                Else
                    Me.ibtnRemoveTempAccountByBO.Visible = False
                End If
                ' CRE20-003-03 Enhancement on Programme or Scheme using batch upload [End][Winnie]

            Case EHSAccountModel.SysAccountSource.SpecialAccount
                Me.ibtnAmendHistory.Visible = False

                Dim udtDocTypeList As DocTypeModelCollection
                udtDocTypeList = udtDocTypeBLL.getAllDocType

                If udtEHSAccount.RecordStatus = EHSAccountModel.SpecialAccountRecordStatusClass.PendingVerify Then
                    ' All special account which status is 'P' pending verify should allow to mark validating to ImmD manually

                    If udteHSAccountPersonalInfo.Validating Then
                        blnManualCheckingToImmd = False
                    Else
                        blnManualCheckingToImmd = True
                    End If

                End If

                Me.ibtnAmendRecord.Visible = False

                If blnManualCheckingToImmd.HasValue Then
                    If blnManualCheckingToImmd.Value Then
                        ' Mark Sending to Immd for Vaidation
                        Me.ibtnMarkAsImmDValid.Visible = True

                        Me.ibtnConfirmAsValidAcct.Visible = False
                        Me.ibtnCancelValidation.Visible = False
                    Else
                        ' Remain as special account or Confirm as a valid account after Immd Checking Manually
                        If udteHSAccountMaintBLL.IsValidatingByMannual(udtEHSAccount.VoucherAccID.Trim) Then
                            Me.ibtnConfirmAsValidAcct.Visible = True
                            Me.ibtnCancelValidation.Visible = True
                        Else
                            Me.ibtnConfirmAsValidAcct.Visible = False
                            Me.ibtnCancelValidation.Visible = False
                        End If

                        Me.ibtnMarkAsImmDValid.Visible = False
                        Me.ibtnAmendRecord.Visible = False
                    End If
                End If

                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
                pnlTransactionInfo.Visible = True
                If udtEHSAccount.TransactionID IsNot Nothing Then
                    If Not udtEHSAccount.TransactionID.Trim.Equals(String.Empty) Then

                        Me.lblTransactionID.Text = udtformatter.formatSystemNumber(udtEHSAccount.TransactionID)
                    Else
                        Me.lblTransactionID.Text = Me.GetGlobalResourceObject("Text", "N/A")
                    End If
                Else
                    Me.lblTransactionID.Text = Me.GetGlobalResourceObject("Text", "N/A")
                End If
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]

            Case Else
                Me.ibtnAmendHistory.Visible = False
        End Select


    End Sub

    Private Sub ClearSession(ByVal blnClearResultListSession As Boolean)

        If blnClearResultListSession Then
            Session(SESS_Result) = Nothing
            Session.Remove(SESS_Result)
        End If

        Session(SESS_InputMode) = Nothing
        Session.Remove(SESS_InputMode)

        Session(SESS_DefaultSetCCCode) = Nothing
        Session.Remove(SESS_DefaultSetCCCode)

        Session(SESS_ClickSave) = Nothing
        Session.Remove(SESS_ClickSave)

        Session(SESS_AmendHistory) = Nothing
        Session.Remove(SESS_AmendHistory)

        Session(SESS_ActionMode) = Nothing
        Session.Remove(SESS_ActionMode)

        Session(SESS_PopupActionMode) = Nothing
        Session.Remove(SESS_PopupActionMode)

        udteHSAccountMaintBLL.EHSAccountRemoveFromSession(FunctionCode)
        udteHSAccountMaintBLL.EHSAccount_Amend_RemoveFromSession(FunctionCode)

        txtDocCode.Text = String.Empty

        Me.udcCCCode.Clean()
        Me.udcCCCode.CleanSession(FunctionCode)


    End Sub

    Private Sub ClearErrorImage()
        Me.udcMsgBox.Clear()
        Me.udcInfoMsgAdvancedSearch.Clear()
        Me.udcInfoMsgBox.Clear()

        'Route 2
        Me.imgDocTypeError.Visible = False
        Me.imgIdentityNumErr.Visible = False
        Me.imgSearchAccountIDR2Error.Visible = False
        Me.imgDOBError.Visible = False
        Me.imgDateError.Visible = False
        Me.imgSearchAccountIDR2Error.Visible = False

        'Route 3
        Me.imgSPIDErrorR3.Visible = False
        Me.imgCreationDateErrorR3.Visible = False
        Me.imgDateofDeathErrorR3.Visible = False
    End Sub

    Private Sub SetPanelAction(ByVal EditAccount As EditAccountModel)
        Session(SESS_ActionMode) = Nothing
        Session.Remove(SESS_ActionMode)

        Me.pnlAccountDetailsActionBtn.Visible = False
        Me.pnlAmendActionBtn.Visible = False
        Me.pnlReactiveAccountInput.Visible = False
        Me.pnlReactiveEnquiryInput.Visible = False
        Me.pnlSuspendAccountInput.Visible = False
        Me.pnlSuspendEnquiryInput.Visible = False
        Me.pnlTerminateAccountInput.Visible = False

        Select Case EditAccount
            Case EditAccountModel.Amending
                pnlAmendActionBtn.Visible = True

                Session(SESS_ActionMode) = EditAccount

            Case EditAccountModel.None
                pnlAccountDetailsActionBtn.Visible = True

            Case EditAccountModel.ReactiveAccount
                Me.imgReactiveAccountInputErr.Visible = False
                Me.txtReactiveAccountInput.Text = String.Empty
                pnlReactiveAccountInput.Visible = True

                Session(SESS_ActionMode) = EditAccount

            Case EditAccountModel.ReactiveEnquiry
                Me.imgReactivePublicEnquiryInputErr.Visible = False
                Me.txtReactivePublicEnquiryInput.Text = String.Empty
                pnlReactiveEnquiryInput.Visible = True

                Session(SESS_ActionMode) = EditAccount

            Case EditAccountModel.SuspendAccount
                Me.imgSuspendAccountInputErr.Visible = False
                Me.txtSuspendAccountInput.Text = String.Empty
                pnlSuspendAccountInput.Visible = True

                Session(SESS_ActionMode) = EditAccount

            Case EditAccountModel.SuspendEnquiry
                Me.imgSuspendPublicEnquiryInputErr.Visible = False
                Me.txtSuspendPublicEnquiryInput.Text = String.Empty
                pnlSuspendEnquiryInput.Visible = True

                Session(SESS_ActionMode) = EditAccount

            Case EditAccountModel.TerminateAccount
                Me.imgTerminateAccountInputErr.Visible = False
                Me.txtTerminateAccountInput.Text = String.Empty
                pnlTerminateAccountInput.Visible = True

                Session(SESS_ActionMode) = EditAccount

        End Select

        udcInfoMsgBox.Clear()

        Select Case EditAccount
            Case EditAccountModel.None
            Case Else

                ' Check Suspicious Claim on action 
                Dim udtEHAccount As EHSAccountModel = udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)

                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                If udtEHAccount.Deceased Then
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
                    ' -------------------------------------------------------------------------
                    'Dim dtMatchResult As DataTable = (New eHealthAccountDeathRecordBLL).GetDeathRecordMatchResult(String.Empty, udtEHAccount.EHSPersonalInformationList(0).IdentityNum, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, -1, -1)
                    Dim bllSearchResult As Common.Component.BaseBLL.BLLSearchResult
                    bllSearchResult = (New eHealthAccountDeathRecordBLL).GetDeathRecordMatchResult(Me.FunctionCode, String.Empty, udtEHAccount.EHSPersonalInformationList(0).IdentityNum, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, -1, -1, Nothing, True)
                    Dim dtMatchResult As DataTable = CType(bllSearchResult.Data, DataTable)

                    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]
                    'Dim ds As DataSet = (New eHealthAccountDeathRecordBLL).GetDeathRecordMatchResultDetail(udtEHAccount.VoucherAccID, udtEHAccount.EHSPersonalInformationList(0).DocCode)

                    If dtMatchResult.Rows.Count > 0 AndAlso dtMatchResult.Rows(0)("With_Suspicious_Claim") = eHealthAccountDeathRecord.eHealthAccountDeathRecordBLL.DeathRecordMatchResult.WithSuspiciousClaim.Y Then
                        Me.udcInfoMsgBox.AddMessage(New SystemMessage(FuncCode, "I", "00013"))
                        Me.udcInfoMsgBox.BuildMessageBox()
                        Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Information
                    End If
                End If
        End Select

    End Sub

#End Region

#Region "Get eHSAccount Function"

    Private Function GeteHSAcc(ByVal strAccountID As String, ByVal strAccountSource As String, ByVal blnGetAmendmentRecord As Boolean) As Boolean
        Dim blnRes As Boolean = False
        udteHSAccountMaintBLL.EHSAccountRemoveFromSession(FunctionCode)
        udteHSAccountMaintBLL.EHSAccount_Amend_RemoveFromSession(FunctionCode)

        ' CRE11-007: Consolidate Function
        udtEHSAccount = GeteHSAcc(strAccountID, strAccountSource)
        'Select Case strAccountSource
        '    Case EHealthAccountType.Temporary
        '        udtEHSAccount = udtEHSAccountBLL.LoadTempEHSAccountByVRID(strAccountID)

        '    Case EHealthAccountType.Special
        '        udtEHSAccount = udtEHSAccountBLL.LoadSpecialEHSAccountByVRID(strAccountID)

        '    Case EHealthAccountType.Validated
        '        udtEHSAccount = udtEHSAccountBLL.LoadEHSAccountByVRID(strAccountID)

        '    Case EHealthAccountType.Invalid
        '        udtEHSAccount = udtEHSAccountBLL.LoadInvalidEHSAccountByVRID(strAccountID)

        'End Select

        If blnGetAmendmentRecord Then
            udtEHSAccount_Amendment = udtEHSAccountBLL.LoadAmendingEHSAccountByVRID(strAccountID, Me.txtDocCode.Text.Trim)
        Else
            udtEHSAccount_Amendment = Nothing
        End If


        If Not IsNothing(udtEHSAccount) Then
            Me.udteHSAccountMaintBLL.EHSAccountSaveToSession(udtEHSAccount, FunctionCode)
            Me.udteHSAccountMaintBLL.EHSAccount_Amend_SaveToSession(udtEHSAccount_Amendment, FunctionCode)
            blnRes = True
        End If
        Return blnRes
    End Function

    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <param name="strAccountID"></param>
    ''' <param name="strAccountSource"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GeteHSAcc(ByVal strAccountID As String, ByVal strAccountSource As String) As EHSAccountModel
        Select Case strAccountSource
            Case EHealthAccountType.Temporary
                Return udtEHSAccountBLL.LoadTempEHSAccountByVRID(strAccountID)

            Case EHealthAccountType.Special
                Return udtEHSAccountBLL.LoadSpecialEHSAccountByVRID(strAccountID)

            Case EHealthAccountType.Validated
                Return udtEHSAccountBLL.LoadEHSAccountByVRID(strAccountID)

            Case EHealthAccountType.Invalid
                Return udtEHSAccountBLL.LoadInvalidEHSAccountByVRID(strAccountID)
        End Select

        Return Nothing
    End Function

    Private Sub GetAmendeHSAccountOnly(ByVal strAccountID As String)
        udteHSAccountMaintBLL.EHSAccount_Amend_RemoveFromSession(FunctionCode)

        ' TO: DO
        ' Get the amendment eHS Account
        udtEHSAccount_Amendment = udtEHSAccountBLL.LoadAmendingEHSAccountByVRID(strAccountID, Me.txtDocCode.Text.Trim)

        Me.udteHSAccountMaintBLL.EHSAccount_Amend_SaveToSession(udtEHSAccount_Amendment, FunctionCode)
    End Sub

    Private Sub BindPersonalInfo(ByVal _udtEHSAccount As EHSAccountModel, ByVal _udtEHSAccount_Amendment As EHSAccountModel, ByVal strDocCode As String, ByVal activeViewChanged As Boolean)
        Dim blnRes As Boolean = False
        If Not IsNothing(_udtEHSAccount) AndAlso Not strDocCode.Equals(String.Empty) Then
            Dim mode As ActionModel


            If Not IsNothing(Session(SESS_InputMode)) Then
                mode = CType(Session(SESS_InputMode), ActionModel)

                Me.ucReadOnlyDocumnetType.Visible = False
                Me.ucInputDocumentType.Visible = False
                Me.ucInputDocumentType.ActiveViewChanged = activeViewChanged

                Select Case mode
                    Case ActionModel.ReadOnly
                        'ReadOnly Control

                        Me.ucReadOnlyDocumnetType.DocumentType = strDocCode
                        Me.ucReadOnlyDocumnetType.EHSPersonalInformation = _udtEHSAccount.EHSPersonalInformationList.Filter(strDocCode)
                        Me.ucReadOnlyDocumnetType.Vertical = True
                        Me.ucReadOnlyDocumnetType.Width = 220
                        Me.ucReadOnlyDocumnetType.Build()

                        ucReadOnlyDocumnetType.Visible = True

                        Me.phReadOnlyAccountInfo.Controls.Clear()
                        Dim ucReadOnlyAccountInfo As ucReadOnlyAccountInfo = Me.LoadControl("~/UIControl/ucReadOnlyAccountInfo.ascx")
                        ucReadOnlyAccountInfo.EHSAccount = _udtEHSAccount
                        ucReadOnlyAccountInfo.Width = 220
                        Me.phReadOnlyAccountInfo.Controls.Add(ucReadOnlyAccountInfo)

                    Case ActionModel.Amending

                        Me.ucInputDocumentType.DocType = strDocCode
                        Me.ucInputDocumentType.EHSAccountOriginal = _udtEHSAccount
                        Me.ucInputDocumentType.EHSAccountAmend = _udtEHSAccount_Amendment
                        Me.ucInputDocumentType.Mode = ucInputDocTypeBase.BuildMode.Modification
                        Me.ucInputDocumentType.FillValue = True
                        Me.ucInputDocumentType.AuditLogEntry = New AuditLogEntry(FuncCode, Me)
                        Me.ucInputDocumentType.Built()

                        Me.ucInputDocumentType.Visible = True

                        blnRes = True

                        Me.phReadOnlyAccountInfo.Controls.Clear()
                        Dim ucReadOnlyAccountInfo As ucReadOnlyAccountInfo = Me.LoadControl("~/UIControl/ucReadOnlyAccountInfo.ascx")
                        ucReadOnlyAccountInfo.EHSAccount = _udtEHSAccount
                        ucReadOnlyAccountInfo.Width = 220
                        Me.phReadOnlyAccountInfo.Controls.Add(ucReadOnlyAccountInfo)

                    Case ActionModel.ReadOnly_N_Amending

                        Me.ucInputDocumentType.DocType = strDocCode
                        Me.ucInputDocumentType.EHSAccountOriginal = _udtEHSAccount
                        Me.ucInputDocumentType.EHSAccountAmend = _udtEHSAccount_Amendment
                        Me.ucInputDocumentType.Mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly
                        Me.ucInputDocumentType.FillValue = True
                        Me.ucInputDocumentType.AuditLogEntry = New AuditLogEntry(FuncCode, Me)
                        Me.ucInputDocumentType.Built()

                        Me.ucInputDocumentType.Visible = True

                        Me.phReadOnlyAccountInfo.Controls.Clear()
                        Dim ucReadOnlyAccountInfo As ucReadOnlyAccountInfo = Me.LoadControl("~/UIControl/ucReadOnlyAccountInfo.ascx")
                        ucReadOnlyAccountInfo.EHSAccount = _udtEHSAccount
                        ucReadOnlyAccountInfo.Width = 220
                        Me.phReadOnlyAccountInfo.Controls.Add(ucReadOnlyAccountInfo)
                        'blnRes = True
                End Select
            End If

        End If

        Dim blnSetCCCode As Boolean

        If blnRes Then
            If Not IsNothing(Session(SESS_DefaultSetCCCode)) Then
                blnSetCCCode = CBool(Session(SESS_DefaultSetCCCode))

                If blnSetCCCode Then
                    If ucInputDocumentType.DocType.Trim.Equals(DocType.DocTypeModel.DocTypeCode.HKIC) Or ucInputDocumentType.DocType.Trim.Equals(DocType.DocTypeModel.DocTypeCode.ROP140) Then

                        Select Case ucInputDocumentType.DocType.Trim
                            Case DocType.DocTypeModel.DocTypeCode.HKIC
                                Dim udcInputHKID As ucInputHKID = Me.ucInputDocumentType.GetHKICControl
                                Me.udcCCCode.CCCode1 = IIf(IsNothing(udcInputHKID.CCCode1), String.Empty, udcInputHKID.CCCode1)
                                Me.udcCCCode.CCCode2 = IIf(IsNothing(udcInputHKID.CCCode2), String.Empty, udcInputHKID.CCCode2)
                                Me.udcCCCode.CCCode3 = IIf(IsNothing(udcInputHKID.CCCode3), String.Empty, udcInputHKID.CCCode3)
                                Me.udcCCCode.CCCode4 = IIf(IsNothing(udcInputHKID.CCCode4), String.Empty, udcInputHKID.CCCode4)
                                Me.udcCCCode.CCCode5 = IIf(IsNothing(udcInputHKID.CCCode5), String.Empty, udcInputHKID.CCCode5)
                                Me.udcCCCode.CCCode6 = IIf(IsNothing(udcInputHKID.CCCode6), String.Empty, udcInputHKID.CCCode6)

                            Case DocType.DocTypeModel.DocTypeCode.ROP140
                                Dim udcInputROP140 As ucInputROP140 = Me.ucInputDocumentType.GetROP140Control
                                Me.udcCCCode.CCCode1 = IIf(IsNothing(udcInputROP140.CCCode1), String.Empty, udcInputROP140.CCCode1)
                                Me.udcCCCode.CCCode2 = IIf(IsNothing(udcInputROP140.CCCode2), String.Empty, udcInputROP140.CCCode2)
                                Me.udcCCCode.CCCode3 = IIf(IsNothing(udcInputROP140.CCCode3), String.Empty, udcInputROP140.CCCode3)
                                Me.udcCCCode.CCCode4 = IIf(IsNothing(udcInputROP140.CCCode4), String.Empty, udcInputROP140.CCCode4)
                                Me.udcCCCode.CCCode5 = IIf(IsNothing(udcInputROP140.CCCode5), String.Empty, udcInputROP140.CCCode5)
                                Me.udcCCCode.CCCode6 = IIf(IsNothing(udcInputROP140.CCCode6), String.Empty, udcInputROP140.CCCode6)
                        End Select

                        udcCCCode.BindCCCode()
                        Me.udcCCCode.GetChineseName(FunctionCode, True)
                        'End If

                        Session(SESS_DefaultSetCCCode) = Nothing
                        Session.Remove(SESS_DefaultSetCCCode)
                    End If
                End If
            End If
        End If

    End Sub

    'By Paul, bind reaonly control to show document type related information in Amendment History)
    Private Sub BindPersonalInfoInAmendmentHistory(ByVal _udtEHSAccount As EHSAccountModel, ByVal _udtEHSAccount_Amendment As EHSAccountModel, ByVal strDocCode As String)
        Dim blnRes As Boolean = False
        If Not IsNothing(_udtEHSAccount) AndAlso Not strDocCode.Equals(String.Empty) Then
            Me.ucReadOnlyDocTypeAmendHistory.DocumentType = strDocCode
            Me.ucReadOnlyDocTypeAmendHistory.EHSPersonalInformation = _udtEHSAccount.EHSPersonalInformationList.Filter(strDocCode)
            Me.ucReadOnlyDocTypeAmendHistory.Vertical = False
            Me.ucReadOnlyDocTypeAmendHistory.Width = 220
            Me.ucReadOnlyDocTypeAmendHistory.Build()

            Me.ucReadOnlyDocTypeAmendHistory.Visible = True
        End If
    End Sub

    Private Sub SetupInputDocControl(ByVal _udtEHSAccount As EHSAccountModel, ByVal _udtEHSAccount_Amendment As EHSAccountModel, ByVal activeChanged As Boolean)
        Select Case Me.mveHSAccount.ActiveViewIndex
            Case intSearchView
            Case intSearchResult
            Case intAccountDetails
                BindPersonalInfo(_udtEHSAccount, _udtEHSAccount_Amendment, Me.txtDocCode.Text.Trim, activeChanged)
            Case intConfirm
                Me.SetupConfirmAccount(_udtEHSAccount_Amendment)
            Case intAmendmentHistroy
                'By Paul, added on 22/9 (Use read-only control to show document type related information in Amendment History)
                BindPersonalInfoInAmendmentHistory(_udtEHSAccount, _udtEHSAccount_Amendment, Me.txtDocCode.Text.Trim)
            Case intNewAccount
                Me.udcStep1DocumentTypeRadioButtonGroup.ShowAllSelection = True
                Me.udcStep1DocumentTypeRadioButtonGroup.ShowLegend = False
                'Me.udcStep1DocumentTypeRadioButtonGroup.SelectedValue = strSelectDocType
                Me.udcStep1DocumentTypeRadioButtonGroup.Build()
                BindPersonalInfoForCreation(_udtEHSAccount_Amendment, Me.txtDocCode.Text.Trim, False)
        End Select
    End Sub

    Private Sub BindPersonalInfoForCreation(ByVal udtEHSAccount As EHSAccountModel, ByVal strDocCode As String, ByVal blnClear As Boolean)

        If blnClear Then
            Me.ucInputDocumentType_NewAcc.Clear()
        End If
        Me.ucInputDocumentType_NewAcc.EHSAccountAmend = udtEHSAccount
        Me.ucInputDocumentType_NewAcc.DocType = strDocCode
        Me.ucInputDocumentType_NewAcc.Mode = ucInputDocTypeBase.BuildMode.Creation

        Me.ucInputDocumentType_NewAcc.Built()

    End Sub


#End Region

#Region "Enter Details Validation"
    'HKID
    Private Function ValidateRectifyDetail_HKID(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry, ByVal udcControlMode As ucInputDocTypeBase.BuildMode) As Boolean
        Dim isValid As Boolean = True

        Dim udcInputHKIC As ucInputHKID

        If udcControlMode = ucInputDocTypeBase.BuildMode.Creation Then
            udcInputHKIC = Me.ucInputDocumentType_NewAcc.GetHKICControl
            Me.ucInputDocumentType_NewAcc.ActiveViewChanged = False
        Else
            udcInputHKIC = Me.ucInputDocumentType.GetHKICControl()
        End If

        udcInputHKIC.SetProperty(udcControlMode)
        udcInputHKIC.SetErrorImage(False)

        _udtAuditLogEntry.AddDescripton("HKID", udcInputHKIC.HKID)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputHKIC.DOB)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputHKIC.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputHKIC.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Chiname", udcInputHKIC.CName)
        _udtAuditLogEntry.AddDescripton("CCCode1", udcInputHKIC.CCCode1)
        _udtAuditLogEntry.AddDescripton("CCCode2", udcInputHKIC.CCCode2)
        _udtAuditLogEntry.AddDescripton("CCCode3", udcInputHKIC.CCCode3)
        _udtAuditLogEntry.AddDescripton("CCCode4", udcInputHKIC.CCCode4)
        _udtAuditLogEntry.AddDescripton("CCCode5", udcInputHKIC.CCCode5)
        _udtAuditLogEntry.AddDescripton("CCCode6", udcInputHKIC.CCCode6)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputHKIC.Gender)
        _udtAuditLogEntry.AddDescripton("DateOfIssue", udcInputHKIC.HKIDIssuseDate)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00017, AuditLogDesc.ValidateAccountDetailInfo)

        'HKIC
        Me.udtSM = Me.udtValidator.chkIdentityNumber(DocTypeModel.DocTypeCode.HKIC, udcInputHKIC.HKID.Trim, String.Empty)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputHKIC.SetHKIDError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String
        Dim dtmDOB As Date

        strDOB = udcInputHKIC.DOB
        Me.udtSM = Me.udtValidator.chkDOB(DocType.DocTypeModel.DocTypeCode.HKIC, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputHKIC.SetDOBError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'English Name
        Me.udtSM = Me.udtValidator.chkEngName(udcInputHKIC.ENameSurName, udcInputHKIC.ENameFirstName)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputHKIC.SetENameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'CCCode
        Me.udtSM = Me.udtValidator.chkCCCode_UsingDDL(String.Format("{0}{1}", udcInputHKIC.CCCode1, Me.udcCCCode.SelectedCCCodeTail1), _
                                                String.Format("{0}{1}", udcInputHKIC.CCCode2, Me.udcCCCode.SelectedCCCodeTail2), _
                                                String.Format("{0}{1}", udcInputHKIC.CCCode3, Me.udcCCCode.SelectedCCCodeTail3), _
                                                String.Format("{0}{1}", udcInputHKIC.CCCode4, Me.udcCCCode.SelectedCCCodeTail4), _
                                                String.Format("{0}{1}", udcInputHKIC.CCCode5, Me.udcCCCode.SelectedCCCodeTail5), _
                                                String.Format("{0}{1}", udcInputHKIC.CCCode6, Me.udcCCCode.SelectedCCCodeTail6))
        If Not Me.udtSM Is Nothing Then
            isValid = False
            udcInputHKIC.SetCCCodeError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'HKIC Gender
        Me.udtSM = Me.udtValidator.chkGender(udcInputHKIC.Gender)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputHKIC.SetGenderError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'DOI
        Dim strHKIDIssueDate As String = Nothing
        Dim dtHKIDIssueDate As DateTime
        strHKIDIssueDate = Me.udtformatter.formatHKIDIssueDateBeforeValidate(udcInputHKIC.HKIDIssuseDate)
        'Me.udtSM = Me.udtvalidator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.HKIC, udcInputHKIC.HKIDIssuseDate, dtmDOB)
        Me.udtSM = Me.udtValidator.chkHKIDIssueDate(strHKIDIssueDate, dtmDOB)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputHKIC.SetHKIDIssueDateError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        Else
            dtHKIDIssueDate = Me.udtformatter.convertHKIDIssueDateStringToDate(strHKIDIssueDate)
        End If

        If isValid Then
            Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKIC)

            If udcControlMode = ucInputDocTypeBase.BuildMode.Creation Then
                udtEHSAccountPersonalInfo.IdentityNum = udcInputHKIC.HKID
            End If

            udtEHSAccountPersonalInfo.ENameSurName = udcInputHKIC.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputHKIC.ENameFirstName

            udtEHSAccountPersonalInfo.CCCode1 = String.Format("{0}{1}", udcInputHKIC.CCCode1, Me.udcCCCode.SelectedCCCodeTail1)
            udtEHSAccountPersonalInfo.CCCode2 = String.Format("{0}{1}", udcInputHKIC.CCCode2, Me.udcCCCode.SelectedCCCodeTail2)
            udtEHSAccountPersonalInfo.CCCode3 = String.Format("{0}{1}", udcInputHKIC.CCCode3, Me.udcCCCode.SelectedCCCodeTail3)
            udtEHSAccountPersonalInfo.CCCode4 = String.Format("{0}{1}", udcInputHKIC.CCCode4, Me.udcCCCode.SelectedCCCodeTail4)
            udtEHSAccountPersonalInfo.CCCode5 = String.Format("{0}{1}", udcInputHKIC.CCCode5, Me.udcCCCode.SelectedCCCodeTail5)
            udtEHSAccountPersonalInfo.CCCode6 = String.Format("{0}{1}", udcInputHKIC.CCCode6, Me.udcCCCode.SelectedCCCodeTail6)

            'Retervie Chinese Name from Choose
            udcInputHKIC.CCCode1 = udtEHSAccountPersonalInfo.CCCode1
            udcInputHKIC.CCCode2 = udtEHSAccountPersonalInfo.CCCode2
            udcInputHKIC.CCCode3 = udtEHSAccountPersonalInfo.CCCode3
            udcInputHKIC.CCCode4 = udtEHSAccountPersonalInfo.CCCode4
            udcInputHKIC.CCCode5 = udtEHSAccountPersonalInfo.CCCode5
            udcInputHKIC.CCCode6 = udtEHSAccountPersonalInfo.CCCode6
            udcInputHKIC.SetCName()
            udtEHSAccountPersonalInfo.CName = udcInputHKIC.CName

            udtEHSAccountPersonalInfo.DOB = dtmDOB 'CDate(Me.udtformatter.convertDate(strDOB, Common.Component.CultureLanguage.English))
            udtEHSAccountPersonalInfo.Gender = udcInputHKIC.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DateofIssue = dtHKIDIssueDate
        End If

        Return isValid
    End Function

    'EC
    Private Function ValidateRectifyDetail_EC(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry, ByVal udcControlMode As ucInputDocTypeBase.BuildMode) As Boolean
        Dim isValid As Boolean = True
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.EC)

        Dim udcInputEC As ucInputEC

        If udcControlMode = ucInputDocTypeBase.BuildMode.Creation Then
            udcInputEC = Me.ucInputDocumentType_NewAcc.GetECControl
            Me.ucInputDocumentType_NewAcc.ActiveViewChanged = False
        Else
            udcInputEC = Me.ucInputDocumentType.GetECControl()
        End If

        udcInputEC.SetProperty(Session(SESS_InputMode))
        udcInputEC.SetModificationErrorImage(False)

        _udtAuditLogEntry.AddDescripton("HKIC", udcInputEC.HKID)
        _udtAuditLogEntry.AddDescripton("ECReference", udcInputEC.Reference)
        _udtAuditLogEntry.AddDescripton("ECReferenceOtherFormat", IIf(udcInputEC.ReferenceOtherFormat, "Y", "N"))
        _udtAuditLogEntry.AddDescripton("ECSerialNumber", udcInputEC.SerialNumber)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputEC.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputEC.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Chiname", udcInputEC.CName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputEC.Gender)
        _udtAuditLogEntry.AddDescripton("DateOfIssue Day", udcInputEC.ECDateDay)
        _udtAuditLogEntry.AddDescripton("DateOfIssue Month", udcInputEC.ECDateMonth)
        _udtAuditLogEntry.AddDescripton("DateOfIssue Year", udcInputEC.ECDateYear)
        _udtAuditLogEntry.AddDescripton("ExactDOB", udcInputEC.DOBtype)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputEC.DOB)
        _udtAuditLogEntry.AddDescripton("Age", udcInputEC.ECAge)
        _udtAuditLogEntry.AddDescripton("DateOfReg Day", udcInputEC.ECDateOfRegDay)
        _udtAuditLogEntry.AddDescripton("DateOfReg Month", udcInputEC.ECDateOfRegMonth)
        _udtAuditLogEntry.AddDescripton("DateOfReg Year", udcInputEC.ECDateOfRegYear)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00017, AuditLogDesc.ValidateAccountDetailInfo)

        ' Serial No.
        Me.udtSM = Me.udtValidator.chkSerialNo(udcInputEC.SerialNumber, udcInputEC.SerialNumberNotProvided)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputEC.SetECSerialNoError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        ' Reference
        Me.udtSM = Me.udtValidator.chkReferenceNo(udcInputEC.Reference, udcInputEC.ReferenceOtherFormat)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputEC.SetECReferenceError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'DOB
        Dim sm_DOB As Common.ComObject.SystemMessage = Nothing
        Dim sm_DOR As Common.ComObject.SystemMessage = Nothing

        Dim strDOB As String = udcInputEC.DOB
        Dim strAge As String = udcInputEC.ECAge
        Dim strDateOfRegDay As String = udcInputEC.ECDateOfRegDay
        Dim strDateOfRegMth As String = udcInputEC.ECDateOfRegMonth
        Dim strDateOfRegYr As String = udcInputEC.ECDateOfRegYear
        Dim strDateOfReg As String = String.Empty

        Dim dtDOR As Nullable(Of DateTime) = Nothing

        Dim dtmDOB As DateTime
        Dim strExactDOB As String = String.Empty

        'Selection of DOB type is identified by by the following enum value (DOBSelection)
        '- ExactDOB
        '- YearOfBirthReported
        '- RecordOnTravDoc
        '- AgeWithDateOfRegistration
        '- NoValue
        Select Case udcInputEC.DOBtype
            Case ucInputEC.DOBSelection.NoValue
                sm_DOB = New SystemMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00003)
            Case ucInputEC.DOBSelection.AgeWithDateOfRegistration
                'Check Age
                sm_DOB = Me.udtValidator.chkECAge(udcInputEC.ECAge)
                If Not sm_DOB Is Nothing Then
                    isValid = False
                    udcInputEC.SetDOBAgeError(True)
                Else
                    strAge = udcInputEC.ECAge
                End If

                ' validate Date of Age
                sm_DOR = Me.udtValidator.chkECDOAge(strDateOfRegDay, strDateOfRegMth, strDateOfRegYr)
                If Not sm_DOR Is Nothing Then
                    isValid = False
                    udcInputEC.SetDateOfRegError(True)
                Else
                    strDateOfReg = String.Format("{0:00}-{1}-{2}", Convert.ToInt32(strDateOfRegDay), strDateOfRegMth, strDateOfRegYr)

                    dtDOR = CDate(Me.udtformatter.convertDate(strDateOfReg, Session(SESS_Language)))
                End If

                ' validate Age + Date of Age if Within Age
                If isValid Then
                    sm_DOB = Me.udtValidator.chkECAgeAndDOAge(udcInputEC.ECAge, strDateOfRegDay, strDateOfRegMth, strDateOfRegYr)
                    If Not sm_DOB Is Nothing Then
                        isValid = False
                        udcInputEC.SetDOBAgeError(True)
                        udcInputEC.SetDateOfRegError(True)
                    Else
                        dtDOR = Date.ParseExact(strDateOfRegDay.Trim + " " + strDateOfRegMth.Trim + " " + strDateOfRegYr.Trim, "d M yyyy", Nothing, System.Globalization.DateTimeStyles.None)
                        strExactDOB = "A"
                        dtmDOB = dtDOR.Value.AddYears(-CInt(strAge))
                    End If
                End If
                'sm_DOB = Me.udtvalidator.chkECAgeAndDOAge(strAge, strDateOfRegDay, strDateOfRegMth, strDateOfRegYr)
                'If Not IsNothing(sm_DOB) Then
                '    isValid = False
                '    udcInputEC.SetDOBAgeError(True)
                '    'Me.udcMsgBox.AddMessage(udtSM)
                'Else
                '    dtDOR = Date.ParseExact(strDateOfRegDay.Trim + " " + strDateOfRegMth.Trim + " " + strDateOfRegYr.Trim, "d M yyyy", Nothing, System.Globalization.DateTimeStyles.None)

                '    strExactDOB = "A"
                '    dtmDOB = dtDOR.Value.AddYears(-CInt(strAge))
                'End If

            Case Else
                sm_DOB = Me.udtValidator.chkDOB(DocTypeModel.DocTypeCode.EC, strDOB, dtmDOB, strExactDOB)

                If Not IsNothing(sm_DOB) Then
                    'Error Found, Invalid Data
                    Select Case udcInputEC.DOBtype
                        Case ucInputEC.DOBSelection.ExactDOB
                            isValid = False
                            udcInputEC.SetDOBDateError(True)
                            'Me.udcMsgBox.AddMessage(udtSM)

                        Case ucInputEC.DOBSelection.YearOfBirthReported
                            udcInputEC.SetDOByearError(True)
                            isValid = False
                            'Me.udcMsgBox.AddMessage(udtSM)

                        Case ucInputEC.DOBSelection.RecordOnTravDoc
                            isValid = False
                            udcInputEC.SetDOBTravelDocError(True)
                            'Me.udcMsgBox.AddMessage(udtSM)

                        Case ucInputEC.DOBSelection.AgeWithDateOfRegistration
                            isValid = False
                            udcInputEC.SetDOBAgeError(True)
                            'Me.udcMsgBox.AddMessage(udtSM)

                    End Select
                Else
                    'Valid Data
                    'Mapping
                    Select Case udcInputEC.DOBtype
                        Case ucInputEC.DOBSelection.RecordOnTravDoc
                            Select Case strExactDOB
                                Case "D"
                                    strExactDOB = "T"
                                Case "M"
                                    strExactDOB = "U"
                                Case "Y"
                                    strExactDOB = "V"
                            End Select
                        Case ucInputEC.DOBSelection.YearOfBirthReported
                            Select Case strExactDOB
                                Case "Y"
                                    strExactDOB = "R"
                                Case Else
                                    'DOB is invalid
                                    isValid = False
                                    'udtSM = New SystemMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00004)
                            End Select


                    End Select
                End If

        End Select

        'Date of Issue
        Dim strECDateDay As String = udcInputEC.ECDateDay.Trim()
        Dim strECDateMonth As String = udcInputEC.ECDateMonth.Trim()
        Dim strECDateYear As String = udcInputEC.ECDateYear.Trim()
        If isValid Then
            udtEHSAccountPersonalInfo.DOB = dtmDOB
        End If
        'Me.udtSM = Me.udtvalidator.chkECDate(strECDateDay, strECDateMonth, strECDateYear, dtmDOB)
        Me.udtSM = Me.udtValidator.chkECDate(strECDateDay, strECDateMonth, strECDateYear, dtmDOB)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputEC.SetECDateError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'HKIC No
        If udcControlMode = ucInputDocTypeBase.BuildMode.Creation Then
            Me.udtSM = Me.udtValidator.chkHKID(udcInputEC.HKID)
            If Not IsNothing(udtSM) Then
                isValid = False
                udcInputEC.SetHKICNoError(True)
                Me.udcMsgBox.AddMessage(udtSM)
            End If
        End If

        'English Name
        Me.udtSM = Me.udtValidator.chkEngName(udcInputEC.ENameSurName, udcInputEC.ENameFirstName)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputEC.SetENameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        ' CRE15-014 HA_MingLiu UTF32 [Start][Winnie]
        'Chinese Name
        Me.udtSM = Me.udtValidator.chkChiName(udcInputEC.CName)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputEC.SetCNameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If
        ' CRE15-014 HA_MingLiu UTF32 [End][Winnie]

        'Gender
        Me.udtSM = Me.udtValidator.chkGender(udcInputEC.Gender)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputEC.SetGenderError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'DOB error message (in sequence)
        If Not IsNothing(sm_DOB) Then
            Me.udcMsgBox.AddMessage(sm_DOB)
        End If
        If Not IsNothing(sm_DOR) Then
            Me.udcMsgBox.AddMessage(sm_DOR)
        End If

        ' Serial No. Not Provided and Reference free format is only allowed for Date of Issue < {SystemParameters: EC_DOI}
        Dim strECDate As String = Nothing

        If isValid Then
            ' Get user input Date of Issue
            If strECDateDay.Length = 1 Then
                strECDate = String.Format("0{0}-{1}-{2}", strECDateDay, strECDateMonth, strECDateYear)
            Else
                strECDate = String.Format("{0}-{1}-{2}", strECDateDay, strECDateMonth, strECDateYear)
            End If

            Dim dtmECDate As Date = Date.ParseExact(strECDate, "dd-MM-yyyy", Nothing)

            udtSM = udtValidator.chkSerialNoNotProvidedAllow(dtmECDate, udcInputEC.SerialNumberNotProvided)
            If Not IsNothing(udtSM) Then
                isValid = False
                udcInputEC.SetECSerialNoError(True)
                udcMsgBox.AddMessage(udtSM)
            End If

            ' Try parse the Reference if all the previous inputs are valid
            If isValid Then
                If udcInputEC.ReferenceOtherFormat Then
                    Dim dtmECDOI As New Date(udcInputEC.ECDateYear, udcInputEC.ECDateMonth, udcInputEC.ECDateDay)
                    udtValidator.TryParseECReference(udcInputEC.Reference, udcInputEC.ReferenceOtherFormat, dtmECDOI)
                End If

            End If

            udtSM = udtValidator.chkReferenceOtherFormatAllow(dtmECDate, udcInputEC.ReferenceOtherFormat)
            If Not IsNothing(udtSM) Then
                isValid = False
                udcInputEC.SetECReferenceError(True)
                udcMsgBox.AddMessage(udtSM)
            End If

        End If

        If isValid Then
            udtEHSAccountPersonalInfo.ENameSurName = udcInputEC.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputEC.ENameFirstName
            udtEHSAccountPersonalInfo.CName = udcInputEC.CName
            udtEHSAccountPersonalInfo.Gender = udcInputEC.Gender

            udtEHSAccountPersonalInfo.ECSerialNo = udcInputEC.SerialNumber
            udtEHSAccountPersonalInfo.ECSerialNoNotProvided = udcInputEC.SerialNumberNotProvided

            udtEHSAccountPersonalInfo.ECReferenceNoOtherFormat = udcInputEC.ReferenceOtherFormat
            If udcInputEC.ReferenceOtherFormat Then
                udtEHSAccountPersonalInfo.ECReferenceNo = udcInputEC.Reference
            Else
                udtEHSAccountPersonalInfo.ECReferenceNo = udcInputEC.Reference.Replace("(", String.Empty).Replace(")", String.Empty)
            End If

            udtEHSAccountPersonalInfo.DateofIssue = CDate(Me.udtformatter.convertDate(strECDate, Common.Component.CultureLanguage.English))
            udtEHSAccountPersonalInfo.DOB = dtmDOB
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            If strExactDOB.Trim = "A" Then
                If strAge.Trim.Equals(String.Empty) Then
                    udtEHSAccountPersonalInfo.ECAge = Nothing
                Else
                    If strAge = "-1" Then
                        udtEHSAccountPersonalInfo.ECAge = Nothing
                    Else
                        udtEHSAccountPersonalInfo.ECAge = CInt(strAge)
                    End If
                End If
            Else
                udtEHSAccountPersonalInfo.ECAge = Nothing
            End If
            udtEHSAccountPersonalInfo.ECDateOfRegistration = dtDOR 'CDate(Me.udtFormatter.convertDate(strDOReg, Common.Component.CultureLanguage.English))

            'udtEHSAccountPersonalInfo.ExactDOB = strIsExactDOB
            'Select Case strIsExactDOB
            '    Case "D", "M", "Y", "T", "U", "V"
            '        udtEHSAccountPersonalInfo.DOB = CDate(Me.udtformatter.convertDate(strDOB, Common.Component.CultureLanguage.English))
            '    Case "R"
            '        udtEHSAccountPersonalInfo.DOB = CDate(Me.udtformatter.convertDate("01-01-" + strDOB.Trim, Common.Component.CultureLanguage.English))
            '    Case "A"
            '        udtEHSAccountPersonalInfo.ECAge = strAge
            '        Dim strDOReg As String
            '        If strDateOfRegDay.Length = 1 Then
            '            strDOReg = String.Format("0{0}-{1}-{2}", strDateOfRegDay, strDateOfRegMth, strDateOfRegYr)
            '        Else
            '            strDOReg = String.Format("{0}-{1}-{2}", strDateOfRegDay, strDateOfRegMth, strDateOfRegYr)
            '        End If
            '        udtEHSAccountPersonalInfo.ECDateOfRegistration = CDate(Me.udtformatter.convertDate(strDOReg, Common.Component.CultureLanguage.English))
            'End Select
        End If

        Return isValid

    End Function

    'HKBC
    Private Function ValidateRectifyDetail_HKBC(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry, ByVal udcControlMode As ucInputDocTypeBase.BuildMode) As Boolean
        Dim isValid As Boolean = True
        Dim udcInputHKBC As ucInputHKBC

        If udcControlMode = ucInputDocTypeBase.BuildMode.Creation Then
            udcInputHKBC = Me.ucInputDocumentType_NewAcc.GetHKBCControl
            Me.ucInputDocumentType_NewAcc.ActiveViewChanged = False
        Else
            udcInputHKBC = Me.ucInputDocumentType.GetHKBCControl
        End If

        udcInputHKBC.SetProperty(udcControlMode)
        udcInputHKBC.SetErrorImage(udcControlMode, False)

        _udtAuditLogEntry.AddDescripton("RegistrationNo", udcInputHKBC.RegistrationNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputHKBC.DOB)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputHKBC.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputHKBC.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputHKBC.Gender)
        _udtAuditLogEntry.AddDescripton("DOBInWord", udcInputHKBC.DOBInWord)
        _udtAuditLogEntry.AddDescripton("DOBInWordCase", udcInputHKBC.DOBInWordCase.ToString())
        _udtAuditLogEntry.AddDescripton("ExactDOB", udcInputHKBC.IsExactDOB)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00017, AuditLogDesc.ValidateAccountDetailInfo)

        'RegNo.
        Me.udtSM = Me.udtValidator.chkIdentityNumber(DocTypeModel.DocTypeCode.HKBC, udcInputHKBC.RegistrationNo.Trim, String.Empty)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputHKBC.SetRegNoError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String = udcInputHKBC.DOB
        Dim dtmDOB As Date


        Select Case udcInputHKBC.DOB.Trim
            Case String.Empty
                udtSM = New SystemMessage("990000", "E", "00003")
                'DOBInWordCase (By radio button selection)
                ' True  --> exact DOB
                ' False --> Not exact DOB
                If udcInputHKBC.DOBInWordCase Then
                    udcInputHKBC.SetDOBTypeError(True)
                Else
                    udcInputHKBC.SetDOBError(True)
                End If
            Case Else
                udtSM = Me.udtValidator.chkDOB(DocTypeModel.DocTypeCode.HKBC, strDOB, dtmDOB, strExactDOB)

                If udtSM Is Nothing Then
                    'If DOBInWordCase = true , it implies that the exact DOB must be "T", "U" or "V"
                    'MAPPING
                    If udcInputHKBC.DOBInWordCase Then
                        Select Case strExactDOB.Trim
                            Case "D"
                                strExactDOB = "T"
                            Case "M"
                                strExactDOB = "U"
                            Case "Y"
                                strExactDOB = "V"
                        End Select
                    End If
                Else
                    If udcInputHKBC.DOBInWordCase Then
                        udcInputHKBC.SetDOBTypeError(True)
                    Else
                        udcInputHKBC.SetDOBError(True)
                    End If
                End If
        End Select

        If Not IsNothing(udtSM) Then
            Me.udcMsgBox.AddMessage(udtSM)
            isValid = False
        End If


        'DOBInWordCase
        If udcInputHKBC.DOBInWordCase Then
            If udcInputHKBC.DOBInWord Is Nothing OrElse udcInputHKBC.DOBInWord = String.Empty Then
                isValid = False
                udtSM = New SystemMessage("990000", "E", "00160")
                udcInputHKBC.SetDOBTypeError(True)
                Me.udcMsgBox.AddMessage(udtSM)
            End If
        End If

        'English Name
        Me.udtSM = Me.udtValidator.chkEngName(udcInputHKBC.ENameSurName, udcInputHKBC.ENameFirstName)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputHKBC.SetENameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'Gender
        Me.udtSM = Me.udtValidator.chkGender(udcInputHKBC.Gender)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputHKBC.SetGenderError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        If isValid Then
            Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKBC)
            If udcControlMode = ucInputDocTypeBase.BuildMode.Creation Then
                udtEHSAccountPersonalInfo.IdentityNum = udcInputHKBC.RegistrationNo
            End If
            udtEHSAccountPersonalInfo.ENameSurName = udcInputHKBC.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputHKBC.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputHKBC.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB 'udcInputHKBC.IsExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB 'CDate(Me.udtformatter.convertDate(strDOB, Common.Component.CultureLanguage.English))
            udtEHSAccountPersonalInfo.OtherInfo = udcInputHKBC.DOBInWord
        End If


        Return isValid
    End Function

    'Adoption
    Private Function ValidateRectifyDetail_Adopt(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry, ByVal udcControlMode As ucInputDocTypeBase.BuildMode) As Boolean
        Dim isvalid As Boolean = True

        Dim udcInputAdopt As ucInputAdoption

        If udcControlMode = ucInputDocTypeBase.BuildMode.Creation Then
            udcInputAdopt = Me.ucInputDocumentType_NewAcc.GetADOPCControl
            Me.ucInputDocumentType_NewAcc.ActiveViewChanged = False
        Else
            udcInputAdopt = Me.ucInputDocumentType.GetADOPCControl()
        End If

        udcInputAdopt.SetProperty(udcControlMode)
        udcInputAdopt.SetErrorImage(udcControlMode, False)

        _udtAuditLogEntry.AddDescripton("NoOfEntry", udcInputAdopt.IdentityNo)
        _udtAuditLogEntry.AddDescripton("Prefix", udcInputAdopt.PerfixNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputAdopt.DOB)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputAdopt.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputAdopt.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputAdopt.Gender)
        _udtAuditLogEntry.AddDescripton("DOBInWord", udcInputAdopt.DOBInWord)
        _udtAuditLogEntry.AddDescripton("DOBInWordCase", udcInputAdopt.DOBInWordCase.ToString())
        _udtAuditLogEntry.AddDescripton("ExactDOB", udcInputAdopt.IsExactDOB)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00017, AuditLogDesc.ValidateAccountDetailInfo)

        'Prefix
        'If udcInputAdopt.PerfixNo.Equals(String.Empty) Then
        '    isvalid = False
        '    udcInputAdopt.SetEntryNoError(True)
        '    Me.udcMsgBox.AddMessage(New SystemMessage("990000", "E", "00210"))
        'End If

        Me.udtSM = Me.udtValidator.chkIdentityNumber(DocTypeModel.DocTypeCode.ADOPC, udcInputAdopt.IdentityNo, udcInputAdopt.PerfixNo)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputAdopt.SetEntryNoError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'English Name
        Me.udtSM = Me.udtValidator.chkEngName(udcInputAdopt.ENameSurName, udcInputAdopt.ENameFirstName)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputAdopt.SetENameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'Gender
        Me.udtSM = Me.udtValidator.chkGender(udcInputAdopt.Gender)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputAdopt.SetGenderError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String = udcInputAdopt.DOB
        Dim dtmDOB As Date

        Select Case udcInputAdopt.DOB.Trim
            Case String.Empty
                udtSM = New SystemMessage("990000", "E", "00003")
                'DOBInWordCase (By radio button selection)
                ' True  --> exact DOB
                ' False --> Not exact DOB
                If udcInputAdopt.DOBInWordCase Then
                    udcInputAdopt.SetDOBInWordError(True)
                Else
                    udcInputAdopt.SetDOBError(True)
                End If
            Case Else
                udtSM = Me.udtValidator.chkDOB(DocTypeModel.DocTypeCode.ADOPC, strDOB, dtmDOB, strExactDOB)

                If udtSM Is Nothing Then
                    'If DOBInWordCase = true , it implies that the exact DOB must be "T", "U" or "V"
                    'MAPPING
                    If udcInputAdopt.DOBInWordCase Then
                        Select Case strExactDOB.Trim
                            Case "D"
                                strExactDOB = "T"
                            Case "M"
                                strExactDOB = "U"
                            Case "Y"
                                strExactDOB = "V"
                        End Select
                    End If
                Else
                    If udcInputAdopt.DOBInWordCase Then
                        udcInputAdopt.SetDOBInWordError(True)
                    Else
                        udcInputAdopt.SetDOBError(True)
                    End If
                End If
        End Select

        If Not IsNothing(udtSM) Then
            Me.udcMsgBox.AddMessage(udtSM)
            isvalid = False
        End If

        'DOBInWordCase
        If udcInputAdopt.DOBInWordCase Then
            If udcInputAdopt.DOBInWord Is Nothing OrElse udcInputAdopt.DOBInWord = String.Empty Then
                isvalid = False
                udtSM = New SystemMessage("990000", "E", "00160")
                udcInputAdopt.SetDOBInWordError(True)
                Me.udcMsgBox.AddMessage(udtSM)
            End If
        End If

        If isvalid Then
            Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.ADOPC)
            If udcControlMode = ucInputDocTypeBase.BuildMode.Creation Then
                udtEHSAccountPersonalInfo.IdentityNum = udcInputAdopt.IdentityNo
            End If

            udtEHSAccountPersonalInfo.ENameSurName = udcInputAdopt.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputAdopt.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputAdopt.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB 'udcInputAdopt.IsExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB 'CDate(Me.udtformatter.convertDate(strDOB, Common.Component.CultureLanguage.English))
            udtEHSAccountPersonalInfo.OtherInfo = udcInputAdopt.DOBInWord
            udtEHSAccountPersonalInfo.AdoptionPrefixNum = udcInputAdopt.PerfixNo
        End If

        Return isvalid
    End Function

    'DI
    Private Function ValidateRectifyDetail_DI(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry, ByVal udcControlMode As ucInputDocTypeBase.BuildMode) As Boolean
        Dim isvalid As Boolean = True
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.DI)

        Dim udcInputDI As ucInputDI

        If udcControlMode = ucInputDocTypeBase.BuildMode.Creation Then
            udcInputDI = Me.ucInputDocumentType_NewAcc.GetDIControl
            Me.ucInputDocumentType_NewAcc.ActiveViewChanged = False
        Else
            udcInputDI = Me.ucInputDocumentType.GetDIControl()
        End If

        udcInputDI.SetProperty(udcControlMode)
        udcInputDI.SetErrorImage(udcControlMode, False)

        _udtAuditLogEntry.AddDescripton("DocNo", udcInputDI.TravelDocNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputDI.DOB)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputDI.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputDI.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputDI.Gender)
        _udtAuditLogEntry.AddDescripton("DOI", udcInputDI.DateOfIssue)
        '_udtAuditLogEntry.AddDescripton("ExactDOB", udcInputDI.ExactDOB)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00017, AuditLogDesc.ValidateAccountDetailInfo)

        'TravelDocNo
        Me.udtSM = Me.udtValidator.chkIdentityNumber(DocTypeModel.DocTypeCode.DI, udcInputDI.TravelDocNo.Trim, String.Empty)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputDI.SetTDError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'English Name
        Me.udtSM = Me.udtValidator.chkEngName(udcInputDI.ENameSurName, udcInputDI.ENameFirstName)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputDI.SetENameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'Gender
        Me.udtSM = Me.udtValidator.chkGender(udcInputDI.Gender)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputDI.SetGenderError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String
        Dim dtmDOB As Date

        strDOB = udcInputDI.DOB

        Me.udtSM = Me.udtValidator.chkDOB(DocType.DocTypeModel.DocTypeCode.DI, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputDI.SetDOBError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        Else
            udtEHSAccountPersonalInfo.DOB = dtmDOB
        End If

        'DOI
        'skip issue date checking if DOB is empty / Invalid
        'as the checking of DOI relies on the supply of DOB
        Dim strIssueDate As String = Nothing
        Dim dtIssueDate As DateTime
        'If IsNothing(udtSM) Then
        'strIssueDate = Me.udtformatter.formatDOI(DocType.DocTypeModel.CertOfException, udcInputDI.DateOfIssue)
        'Me.udtSM = Me.udtvalidator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.DI, udcInputDI.DateOfIssue, dtmDOB)
        Dim strDOI As String = String.Empty
        strDOI = Me.udtformatter.formatDateBeforValidation_DDMMYYYY(udcInputDI.DateOfIssue)
        Me.udtSM = Me.udtValidator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.DI, strDOI, udtEHSAccountPersonalInfo.DOB)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputDI.SetDOIError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        Else
            'dtIssueDate = Me.udtformatter.convertHKIDIssueDateStringToDate(strIssueDate)
            dtIssueDate = CDate(Me.udtformatter.convertDate(Me.udtformatter.formatInputDate(strDOI), Common.Component.CultureLanguage.English))
        End If
        'End If


        If isvalid Then
            If udcControlMode = ucInputDocTypeBase.BuildMode.Creation Then
                udtEHSAccountPersonalInfo.IdentityNum = udcInputDI.TravelDocNo
            End If

            udtEHSAccountPersonalInfo.ENameSurName = udcInputDI.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputDI.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputDI.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB 'CDate(Me.udtformatter.convertDate(strDOB, Common.Component.CultureLanguage.English))
            udtEHSAccountPersonalInfo.DateofIssue = dtIssueDate
        End If

        Return isvalid
    End Function

    'ID235B
    Private Function ValidateRectifyDetail_ID235B(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry, ByVal udcControlMode As ucInputDocTypeBase.BuildMode) As Boolean
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.ID235B)
        Dim isvalid As Boolean = True
        Dim dtPermit As DateTime

        Dim udcInputID235B As ucInputID235B

        If udcControlMode = ucInputDocTypeBase.BuildMode.Creation Then
            udcInputID235B = Me.ucInputDocumentType_NewAcc.GetID235BControl
            Me.ucInputDocumentType_NewAcc.ActiveViewChanged = False
        Else
            udcInputID235B = Me.ucInputDocumentType.GetID235BControl()
        End If

        udcInputID235B.SetProperty(udcControlMode)
        udcInputID235B.SetErrorImage(udcControlMode, False)

        _udtAuditLogEntry.AddDescripton("BirthEntryNo", udcInputID235B.BirthEntryNo)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputID235B.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputID235B.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputID235B.Gender)
        _udtAuditLogEntry.AddDescripton("RemainUntil", udcInputID235B.PermitRemain)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputID235B.DateOfBirth)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00017, AuditLogDesc.ValidateAccountDetailInfo)

        'BirthEntryNo
        Me.udtSM = Me.udtValidator.chkIdentityNumber(DocTypeModel.DocTypeCode.ID235B, udcInputID235B.BirthEntryNo.Trim, String.Empty)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputID235B.SetBirthEntryNoError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'English Name
        Me.udtSM = Me.udtValidator.chkEngName(udcInputID235B.ENameSurName, udcInputID235B.ENameFirstName)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputID235B.SetENameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'Gender
        Me.udtSM = Me.udtValidator.chkGender(udcInputID235B.Gender)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputID235B.SetGenderError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String
        Dim dtmDOB As Date

        strDOB = udcInputID235B.DateOfBirth
        Me.udtSM = Me.udtValidator.chkDOB(DocType.DocTypeModel.DocTypeCode.ID235B, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputID235B.SetDOBError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        Else
            udtEHSAccountPersonalInfo.DOB = dtmDOB
        End If

        'If isvalid Then

        'Permit to remain until
        Dim strPermit As String = Nothing
        strPermit = Me.udtformatter.formatDateBeforValidation_DDMMYYYY(udcInputID235B.PermitRemain)
        'strPermit = Me.udtformatter.formatPermitToReminUntilBeforeValidate(udcInputID235B.PermitRemain)
        'Me.udtSM = Me.udtvalidator.chkPremitToRemainUntil(strPermit, dtmDOB)
        strPermit = Me.udtformatter.formatDateBeforValidation_DDMMYYYY(udcInputID235B.PermitRemain)
        Me.udtSM = Me.udtValidator.chkPremitToRemainUntil(strPermit, udtEHSAccountPersonalInfo.DOB, DocType.DocTypeModel.DocTypeCode.ID235B)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputID235B.SetPermitRemainError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        Else
            dtPermit = CDate(Me.udtformatter.convertDate(Me.udtformatter.formatInputDate(strPermit), Common.Component.CultureLanguage.English))
        End If

        'End If

        If isvalid Then
            If udcControlMode = ucInputDocTypeBase.BuildMode.Creation Then
                udtEHSAccountPersonalInfo.IdentityNum = udcInputID235B.BirthEntryNo
            End If

            udtEHSAccountPersonalInfo.ENameSurName = udcInputID235B.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputID235B.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputID235B.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB
            udtEHSAccountPersonalInfo.PermitToRemainUntil = dtPermit
        End If

        Return isvalid
    End Function

    'Re-entry Permit
    Private Function ValidateRectifyDetail_ReEntryPermit(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry, ByVal udcControlMode As ucInputDocTypeBase.BuildMode) As Boolean
        Dim isvalid As Boolean = True
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.REPMT)

        Dim udcInputReentryPermit As ucInputReentryPermit

        If udcControlMode = ucInputDocTypeBase.BuildMode.Creation Then
            udcInputReentryPermit = Me.ucInputDocumentType_NewAcc.GetREPMTControl
            Me.ucInputDocumentType_NewAcc.ActiveViewChanged = False
        Else
            udcInputReentryPermit = Me.ucInputDocumentType.GetREPMTControl()
        End If

        udcInputReentryPermit.SetProperty(udcControlMode)
        udcInputReentryPermit.SetErrorImage(udcControlMode, False)

        _udtAuditLogEntry.AddDescripton("REPMTNo", udcInputReentryPermit.REPMTNo)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputReentryPermit.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputReentryPermit.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputReentryPermit.Gender)
        _udtAuditLogEntry.AddDescripton("DOI", udcInputReentryPermit.DateOfIssue)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputReentryPermit.DateOfBirth)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00017, AuditLogDesc.ValidateAccountDetailInfo)

        'REPMTNo
        Me.udtSM = Me.udtValidator.chkIdentityNumber(DocTypeModel.DocTypeCode.REPMT, udcInputReentryPermit.REPMTNo.Trim, String.Empty)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputReentryPermit.SetREPMTNoError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'English Name
        Me.udtSM = Me.udtValidator.chkEngName(udcInputReentryPermit.ENameSurName, udcInputReentryPermit.ENameFirstName)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputReentryPermit.SetENameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'Gender
        Me.udtSM = Me.udtValidator.chkGender(udcInputReentryPermit.Gender)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputReentryPermit.SetGenderError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String
        Dim dtmDOB As Date

        strDOB = udcInputReentryPermit.DateOfBirth
        Me.udtSM = Me.udtValidator.chkDOB(DocType.DocTypeModel.DocTypeCode.REPMT, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputReentryPermit.SetDOBError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        Else
            udtEHSAccountPersonalInfo.DOB = dtmDOB
        End If

        'DOI
        'skip issue date checking if DOB is empty / Invalid
        'as the checking of DOI relies on the supply of DOB
        Dim strIssueDate As String = Nothing
        Dim dtIssueDate As DateTime
        'If IsNothing(udtSM) Then
        'strIssueDate = Me.udtformatter.formatDateBeforValidation_DDMMYYYY(udcInputReentryPermit.DateOfIssue)
        'Me.udtSM = Me.udtvalidator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.REPMT, strIssueDate, dtmDOB)
        strIssueDate = Me.udtformatter.formatDateBeforValidation_DDMMYYYY(udcInputReentryPermit.DateOfIssue)
        Me.udtSM = Me.udtValidator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.REPMT, strIssueDate, udtEHSAccountPersonalInfo.DOB)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputReentryPermit.SetDOIError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        Else
            dtIssueDate = CDate(Me.udtformatter.convertDate(Me.udtformatter.formatInputDate(strIssueDate), Common.Component.CultureLanguage.English))
        End If
        'End If

        If isvalid Then
            If udcControlMode = ucInputDocTypeBase.BuildMode.Creation Then
                udtEHSAccountPersonalInfo.IdentityNum = udcInputReentryPermit.REPMTNo
            End If

            udtEHSAccountPersonalInfo.ENameSurName = udcInputReentryPermit.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputReentryPermit.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputReentryPermit.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB 'CDate(Me.udtformatter.convertDate(strDOB, Common.Component.CultureLanguage.English))
            udtEHSAccountPersonalInfo.DateofIssue = dtIssueDate
        End If

        Return isvalid
    End Function

    'Visa
    Private Function ValidateRectifyDetail_Visa(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry, ByVal udcControlMode As ucInputDocTypeBase.BuildMode) As Boolean
        Dim isValid As Boolean = True
        Dim udcInputVisa As ucInputVISA

        If udcControlMode = ucInputDocTypeBase.BuildMode.Creation Then
            udcInputVisa = Me.ucInputDocumentType_NewAcc.GetVISAControl
            Me.ucInputDocumentType_NewAcc.ActiveViewChanged = False
        Else
            udcInputVisa = Me.ucInputDocumentType.GetVISAControl()
        End If

        udcInputVisa.SetProperty(udcControlMode)
        udcInputVisa.SetErrorImage(udcControlMode, False)

        _udtAuditLogEntry.AddDescripton("VISANo", udcInputVisa.VISANo)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputVisa.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputVisa.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputVisa.Gender)
        _udtAuditLogEntry.AddDescripton("PassportNo", udcInputVisa.PassportNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputVisa.DOB)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00017, AuditLogDesc.ValidateAccountDetailInfo)


        'VISANo
        Me.udtSM = Me.udtValidator.chkIdentityNumber(DocTypeModel.DocTypeCode.VISA, udcInputVisa.VISANo.Trim, String.Empty)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputVisa.SetVISANoError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'VISA
        If udcInputVisa.PassportNo.Equals(String.Empty) Then
            isValid = False
            udcInputVisa.SetPassportNoError(True)
            Me.udcMsgBox.AddMessage(New SystemMessage("990000", "E", "00236"))
        End If

        'English Name
        Me.udtSM = Me.udtValidator.chkEngName(udcInputVisa.ENameSurName, udcInputVisa.ENameFirstName)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputVisa.SetENameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'Gender
        Me.udtSM = Me.udtValidator.chkGender(udcInputVisa.Gender)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputVisa.SetGenderError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String
        Dim dtmDOB As Date

        strDOB = udcInputVisa.DOB
        Me.udtSM = Me.udtValidator.chkDOB(DocType.DocTypeModel.DocTypeCode.VISA, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputVisa.SetDOBError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        If isValid Then
            Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.VISA)
            If udcControlMode = ucInputDocTypeBase.BuildMode.Creation Then
                udtEHSAccountPersonalInfo.IdentityNum = udcInputVisa.VISANo
            End If
            udtEHSAccountPersonalInfo.ENameSurName = udcInputVisa.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputVisa.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputVisa.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB 'CDate(Me.udtformatter.convertDate(strDOB, Common.Component.CultureLanguage.English))
            udtEHSAccountPersonalInfo.Foreign_Passport_No = udcInputVisa.PassportNo
        End If


        Return isValid
    End Function


    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    'OW or RFNo8
    Private Function ValidateRectifyDetail_OW_RFNo8(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry, ByVal udcControlMode As ucInputDocTypeBase.BuildMode) As Boolean
        Dim isValid As Boolean = True
        Dim udcInputOW As ucInputOW

        If udcControlMode = ucInputDocTypeBase.BuildMode.Creation Then
            udcInputOW = Me.ucInputDocumentType_NewAcc.GetOWControl
            Me.ucInputDocumentType_NewAcc.ActiveViewChanged = False
        Else
            udcInputOW = Me.ucInputDocumentType.GetOWControl
        End If

        udcInputOW.SetProperty(udcControlMode)
        udcInputOW.SetErrorImage(udcControlMode, False)

        _udtAuditLogEntry.AddDescripton("DocumentNo", udcInputOW.DocumentNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputOW.DOB)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputOW.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputOW.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputOW.Gender)
        _udtAuditLogEntry.AddDescripton("ExactDOB", udcInputOW.IsExactDOB)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00017, AuditLogDesc.ValidateAccountDetailInfo)

        'DocNo.
        Me.udtSM = Me.udtValidator.chkIdentityNumber(txtDocCode.Text.Trim, udcInputOW.DocumentNo.Trim, String.Empty)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputOW.SetDocNoError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String = udcInputOW.DOB
        Dim dtmDOB As Date

        Select Case udcInputOW.DOB.Trim
            Case String.Empty
                udtSM = New SystemMessage("990000", "E", "00003")
                udcInputOW.SetDOBError(True)

            Case Else
                udtSM = Me.udtValidator.chkDOB(txtDocCode.Text.Trim, strDOB, dtmDOB, strExactDOB)

                If Not udtSM Is Nothing Then
                    udcInputOW.SetDOBError(True)
                End If
        End Select

        If Not IsNothing(udtSM) Then
            Me.udcMsgBox.AddMessage(udtSM)
            isValid = False
        End If

        'English Name
        Me.udtSM = Me.udtValidator.chkEngName(udcInputOW.ENameSurName, udcInputOW.ENameFirstName)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputOW.SetENameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'Gender
        Me.udtSM = Me.udtValidator.chkGender(udcInputOW.Gender)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputOW.SetGenderError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        If isValid Then
            Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(txtDocCode.Text.Trim)
            If udcControlMode = ucInputDocTypeBase.BuildMode.Creation Then
                udtEHSAccountPersonalInfo.IdentityNum = udcInputOW.DocumentNo
            End If
            udtEHSAccountPersonalInfo.ENameSurName = udcInputOW.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputOW.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputOW.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB
        End If

        Return isValid
    End Function
    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

    'TW
    Private Function ValidateRectifyDetail_TW(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry, ByVal udcControlMode As ucInputDocTypeBase.BuildMode) As Boolean
        Dim isValid As Boolean = True
        Dim udcInputTW As ucInputTW

        If udcControlMode = ucInputDocTypeBase.BuildMode.Creation Then
            udcInputTW = Me.ucInputDocumentType_NewAcc.GetTWControl
            Me.ucInputDocumentType_NewAcc.ActiveViewChanged = False
        Else
            udcInputTW = Me.ucInputDocumentType.GetTWControl
        End If

        udcInputTW.SetProperty(udcControlMode)
        udcInputTW.SetErrorImage(udcControlMode, False)

        _udtAuditLogEntry.AddDescripton("DocumentNo", udcInputTW.DocumentNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputTW.DOB)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputTW.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputTW.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputTW.Gender)
        _udtAuditLogEntry.AddDescripton("ExactDOB", udcInputTW.IsExactDOB)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00017, AuditLogDesc.ValidateAccountDetailInfo)

        'DocNo.
        Me.udtSM = Me.udtValidator.chkIdentityNumber(txtDocCode.Text.Trim, udcInputTW.DocumentNo.Trim, String.Empty)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputTW.SetDocNoError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String = udcInputTW.DOB
        Dim dtmDOB As Date

        Select Case udcInputTW.DOB.Trim
            Case String.Empty
                udtSM = New SystemMessage("990000", "E", "00003")
                udcInputTW.SetDOBError(True)

            Case Else
                udtSM = Me.udtValidator.chkDOB(txtDocCode.Text.Trim, strDOB, dtmDOB, strExactDOB)

                If Not udtSM Is Nothing Then
                    udcInputTW.SetDOBError(True)
                End If
        End Select

        If Not IsNothing(udtSM) Then
            Me.udcMsgBox.AddMessage(udtSM)
            isValid = False
        End If

        'English Name
        Me.udtSM = Me.udtValidator.chkEngName(udcInputTW.ENameSurName, udcInputTW.ENameFirstName)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputTW.SetENameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'Gender
        Me.udtSM = Me.udtValidator.chkGender(udcInputTW.Gender)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputTW.SetGenderError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        If isValid Then
            Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(txtDocCode.Text.Trim)
            If udcControlMode = ucInputDocTypeBase.BuildMode.Creation Then
                udtEHSAccountPersonalInfo.IdentityNum = udcInputTW.DocumentNo
            End If
            udtEHSAccountPersonalInfo.ENameSurName = udcInputTW.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputTW.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputTW.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB
        End If

        Return isValid
    End Function



    'CCIC
    Private Function ValidateRectifyDetail_CCIC(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry, ByVal udcControlMode As ucInputDocTypeBase.BuildMode) As Boolean
        Dim isvalid As Boolean = True
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.CCIC)

        Dim udcInputCCIC As ucInputCCIC

        If udcControlMode = ucInputDocTypeBase.BuildMode.Creation Then
            udcInputCCIC = Me.ucInputDocumentType_NewAcc.GetCCICControl
            Me.ucInputDocumentType_NewAcc.ActiveViewChanged = False
        Else
            udcInputCCIC = Me.ucInputDocumentType.GetCCICControl()
        End If

        udcInputCCIC.SetProperty(udcControlMode)
        udcInputCCIC.SetErrorImage(udcControlMode, False)

        _udtAuditLogEntry.AddDescripton("DocNo", udcInputCCIC.TravelDocNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputCCIC.DOB)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputCCIC.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputCCIC.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputCCIC.Gender)
        _udtAuditLogEntry.AddDescripton("DOI", udcInputCCIC.DateOfIssue)
        '_udtAuditLogEntry.AddDescripton("ExactDOB", udcInputCCIC.ExactDOB)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00017, AuditLogDesc.ValidateAccountDetailInfo)

        'TravelDocNo
        Me.udtSM = Me.udtValidator.chkIdentityNumber(DocTypeModel.DocTypeCode.CCIC, udcInputCCIC.TravelDocNo.Trim, String.Empty)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputCCIC.SetTDError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'English Name
        Me.udtSM = Me.udtValidator.chkEngName(udcInputCCIC.ENameSurName, udcInputCCIC.ENameFirstName, DocTypeModel.DocTypeCode.CCIC)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputCCIC.SetENameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'Gender
        Me.udtSM = Me.udtValidator.chkGender(udcInputCCIC.Gender)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputCCIC.SetGenderError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String
        Dim dtmDOB As Date

        strDOB = udcInputCCIC.DOB

        Me.udtSM = Me.udtValidator.chkDOB(DocType.DocTypeModel.DocTypeCode.CCIC, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputCCIC.SetDOBError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        Else
            udtEHSAccountPersonalInfo.DOB = dtmDOB
        End If

        'DOI
        Dim strHKIDIssueDate As String = Nothing
        Dim dtHKIDIssueDate As DateTime
        strHKIDIssueDate = Me.udtformatter.formatHKIDIssueDateBeforeValidate(udcInputCCIC.DateOfIssue)
        Me.udtSM = Me.udtValidator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.CCIC, strHKIDIssueDate, udtEHSAccountPersonalInfo.DOB)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputCCIC.SetDOIError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        Else
            dtHKIDIssueDate = Me.udtformatter.convertHKIDIssueDateStringToDate(strHKIDIssueDate)
        End If

        If isvalid Then
            If udcControlMode = ucInputDocTypeBase.BuildMode.Creation Then
                udtEHSAccountPersonalInfo.IdentityNum = udcInputCCIC.TravelDocNo
            End If

            udtEHSAccountPersonalInfo.ENameSurName = udcInputCCIC.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputCCIC.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputCCIC.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB
            udtEHSAccountPersonalInfo.DateofIssue = dtHKIDIssueDate
        End If

        Return isvalid
    End Function

    'ROP140
    Private Function ValidateRectifyDetail_ROP140(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry, ByVal udcControlMode As ucInputDocTypeBase.BuildMode) As Boolean
        Dim isvalid As Boolean = True
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.ROP140)

        Dim udcInputROP140 As ucInputROP140

        If udcControlMode = ucInputDocTypeBase.BuildMode.Creation Then
            udcInputROP140 = Me.ucInputDocumentType_NewAcc.GetROP140Control
            Me.ucInputDocumentType_NewAcc.ActiveViewChanged = False
        Else
            udcInputROP140 = Me.ucInputDocumentType.GetROP140Control()
        End If

        udcInputROP140.SetProperty(udcControlMode)
        udcInputROP140.SetErrorImage(udcControlMode, False)

        _udtAuditLogEntry.AddDescripton("DocNo", udcInputROP140.TravelDocNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputROP140.DOB)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputROP140.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputROP140.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputROP140.Gender)
        _udtAuditLogEntry.AddDescripton("DOI", udcInputROP140.DateOfIssue)
        _udtAuditLogEntry.AddDescripton("Chiname", udcInputROP140.CName)
        _udtAuditLogEntry.AddDescripton("CCCode1", udcInputROP140.CCCode1)
        _udtAuditLogEntry.AddDescripton("CCCode2", udcInputROP140.CCCode2)
        _udtAuditLogEntry.AddDescripton("CCCode3", udcInputROP140.CCCode3)
        _udtAuditLogEntry.AddDescripton("CCCode4", udcInputROP140.CCCode4)
        _udtAuditLogEntry.AddDescripton("CCCode5", udcInputROP140.CCCode5)
        _udtAuditLogEntry.AddDescripton("CCCode6", udcInputROP140.CCCode6)
        '_udtAuditLogEntry.AddDescripton("ExactDOB", udcInputROP140.ExactDOB)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00017, AuditLogDesc.ValidateAccountDetailInfo)

        'TravelDocNo
        Me.udtSM = Me.udtValidator.chkIdentityNumber(DocTypeModel.DocTypeCode.ROP140, udcInputROP140.TravelDocNo.Trim, String.Empty)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputROP140.SetTDError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'English Name
        Me.udtSM = Me.udtValidator.chkEngName(udcInputROP140.ENameSurName, udcInputROP140.ENameFirstName)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputROP140.SetENameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'CCCode
        Me.udtSM = Me.udtValidator.chkCCCode_UsingDDL(String.Format("{0}{1}", udcInputROP140.CCCode1, Me.udcCCCode.SelectedCCCodeTail1), _
                                                String.Format("{0}{1}", udcInputROP140.CCCode2, Me.udcCCCode.SelectedCCCodeTail2), _
                                                String.Format("{0}{1}", udcInputROP140.CCCode3, Me.udcCCCode.SelectedCCCodeTail3), _
                                                String.Format("{0}{1}", udcInputROP140.CCCode4, Me.udcCCCode.SelectedCCCodeTail4), _
                                                String.Format("{0}{1}", udcInputROP140.CCCode5, Me.udcCCCode.SelectedCCCodeTail5), _
                                                String.Format("{0}{1}", udcInputROP140.CCCode6, Me.udcCCCode.SelectedCCCodeTail6))
        If Not Me.udtSM Is Nothing Then
            isvalid = False
            udcInputROP140.SetCCCodeError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'Gender
        Me.udtSM = Me.udtValidator.chkGender(udcInputROP140.Gender)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputROP140.SetGenderError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String
        Dim dtmDOB As Date

        strDOB = udcInputROP140.DOB

        Me.udtSM = Me.udtValidator.chkDOB(DocType.DocTypeModel.DocTypeCode.ROP140, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputROP140.SetDOBError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        Else
            udtEHSAccountPersonalInfo.DOB = dtmDOB
        End If

        'DOI       
        Dim strIssueDate As String = Nothing
        Dim dtIssueDate As DateTime
        Dim strDOI As String = String.Empty
        strDOI = Me.udtformatter.formatDateBeforValidation_DDMMYYYY(udcInputROP140.DateOfIssue)
        Me.udtSM = Me.udtValidator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.ROP140, strDOI, udtEHSAccountPersonalInfo.DOB)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputROP140.SetDOIError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        Else
            dtIssueDate = CDate(Me.udtformatter.convertDate(Me.udtformatter.formatInputDate(strDOI), Common.Component.CultureLanguage.English))
        End If

        If isvalid Then
            If udcControlMode = ucInputDocTypeBase.BuildMode.Creation Then
                udtEHSAccountPersonalInfo.IdentityNum = udcInputROP140.TravelDocNo
            End If

            udtEHSAccountPersonalInfo.ENameSurName = udcInputROP140.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputROP140.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputROP140.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB
            udtEHSAccountPersonalInfo.DateofIssue = dtIssueDate

            udtEHSAccountPersonalInfo.CCCode1 = String.Format("{0}{1}", udcInputROP140.CCCode1, Me.udcCCCode.SelectedCCCodeTail1)
            udtEHSAccountPersonalInfo.CCCode2 = String.Format("{0}{1}", udcInputROP140.CCCode2, Me.udcCCCode.SelectedCCCodeTail2)
            udtEHSAccountPersonalInfo.CCCode3 = String.Format("{0}{1}", udcInputROP140.CCCode3, Me.udcCCCode.SelectedCCCodeTail3)
            udtEHSAccountPersonalInfo.CCCode4 = String.Format("{0}{1}", udcInputROP140.CCCode4, Me.udcCCCode.SelectedCCCodeTail4)
            udtEHSAccountPersonalInfo.CCCode5 = String.Format("{0}{1}", udcInputROP140.CCCode5, Me.udcCCCode.SelectedCCCodeTail5)
            udtEHSAccountPersonalInfo.CCCode6 = String.Format("{0}{1}", udcInputROP140.CCCode6, Me.udcCCCode.SelectedCCCodeTail6)

            'Retervie Chinese Name from Choose
            udcInputROP140.CCCode1 = udtEHSAccountPersonalInfo.CCCode1
            udcInputROP140.CCCode2 = udtEHSAccountPersonalInfo.CCCode2
            udcInputROP140.CCCode3 = udtEHSAccountPersonalInfo.CCCode3
            udcInputROP140.CCCode4 = udtEHSAccountPersonalInfo.CCCode4
            udcInputROP140.CCCode5 = udtEHSAccountPersonalInfo.CCCode5
            udcInputROP140.CCCode6 = udtEHSAccountPersonalInfo.CCCode6
            udcInputROP140.SetCName()
            udtEHSAccountPersonalInfo.CName = udcInputROP140.CName

        End If

        Return isvalid
    End Function

    'PASS
    Private Function ValidateRectifyDetail_PASS(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry, ByVal udcControlMode As ucInputDocTypeBase.BuildMode) As Boolean
        Dim isvalid As Boolean = True
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.PASS)

        Dim udcInputPASS As ucInputPASS

        If udcControlMode = ucInputDocTypeBase.BuildMode.Creation Then
            udcInputPASS = Me.ucInputDocumentType_NewAcc.GetPASSControl
            Me.ucInputDocumentType_NewAcc.ActiveViewChanged = False
        Else
            udcInputPASS = Me.ucInputDocumentType.GetPASSControl()
        End If

        udcInputPASS.SetProperty(udcControlMode)
        udcInputPASS.SetErrorImage(udcControlMode, False)

        _udtAuditLogEntry.AddDescripton("DocNo", udcInputPASS.TravelDocNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputPASS.DOB)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputPASS.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputPASS.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputPASS.Gender)
        _udtAuditLogEntry.AddDescripton("PassportIssueRegion", udcInputPASS.PassportIssueRegion)
        '_udtAuditLogEntry.AddDescripton("ExactDOB", udcInputCCIC.ExactDOB)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00017, AuditLogDesc.ValidateAccountDetailInfo)

        'TravelDocNo
        Me.udtSM = Me.udtValidator.chkIdentityNumber(DocTypeModel.DocTypeCode.PASS, udcInputPASS.TravelDocNo.Trim, String.Empty)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputPASS.SetTDError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'English Name
        Me.udtSM = Me.udtValidator.chkEngName(udcInputPASS.ENameSurName, udcInputPASS.ENameFirstName, DocTypeModel.DocTypeCode.PASS)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputPASS.SetENameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'Gender
        Me.udtSM = Me.udtValidator.chkGender(udcInputPASS.Gender)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputPASS.SetGenderError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String
        Dim dtmDOB As Date

        strDOB = udcInputPASS.DOB

        Me.udtSM = Me.udtValidator.chkDOB(DocType.DocTypeModel.DocTypeCode.PASS, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputPASS.SetDOBError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        Else
            udtEHSAccountPersonalInfo.DOB = dtmDOB
        End If



        ' CRE20-023 Add Issue country/region to passport document [Start][Raiman]
        'Add Passport checking
        If udcInputPASS.PassportIssueRegion.Equals(String.Empty) Then
            Me.udtSM = New Common.ComObject.SystemMessage("990000", "E", "00462")
            isvalid = False
            udcInputPASS.SetPassportIssueRegionError(True)
            Me.udcMsgBox.AddMessage(Me.udtSM, "%en", Me.GetGlobalResourceObject("Text", "PassportIssueRegion"))
        End If
        ' CRE20-023 Add Issue country/region to passport document [End][Raiman]

        If isvalid Then
            If udcControlMode = ucInputDocTypeBase.BuildMode.Creation Then
                udtEHSAccountPersonalInfo.IdentityNum = udcInputPASS.TravelDocNo
            End If

            udtEHSAccountPersonalInfo.ENameSurName = udcInputPASS.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputPASS.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputPASS.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB

            ' CRE20-023 Add Issue country/region to passport document [Start][Raiman]
            udtEHSAccountPersonalInfo.PassportIssueRegion = udcInputPASS.PassportIssueRegion
            ' CRE20-023 Add Issue country/region to passport document [End][Raiman]

        End If

        Return isvalid
    End Function


#End Region

#Region "Confirm Details"

    'for Amend Account
    Private Sub SetupConfirmAccount(ByVal _udtEHSAccount As EHSAccountModel, Optional ByVal blnShowDateOfDeathBtn As Boolean = True)
        If Not IsNothing(_udtEHSAccount) Then

            Me.udcConfirmAccount.DocumentType = Me.txtDocCode.Text.Trim '_udtEHSAccount.EHSPersonalInformationList(0).DocCode.Trim
            Me.udcConfirmAccount.EHSPersonalInformation = _udtEHSAccount.getPersonalInformation(Me.txtDocCode.Text.Trim)
            Me.udcConfirmAccount.MaskIdentityNo = False
            Me.udcConfirmAccount.Vertical = True
            Me.udcConfirmAccount.Width = 220
            Me.udcConfirmAccount.DisplayFormat = HCVU.ucReadOnlyDocumnetType.EnumDisplayFormat.InputFormat
            If Not blnShowDateOfDeathBtn Then
                Me.udcConfirmAccount.ShowDateOfDeathBtn = False
            End If

            Me.udcConfirmAccount.Build()

            'Show Account Creation Information
            Dim udcActionMode As EditAccountModel
            udcActionMode = Session(SESS_ActionMode)
            If udcActionMode = EditAccountModel.NewAccount Then

                Me.pnlShowAccCreateInfoConfirm_BySP.Visible = False
                Me.pnlShowAccCreateInfoConfirm_ByBO.Visible = True
                Me.lblConfirmCreationCreatedBy.Text = _udtEHSAccount.CreateBy
                'If _udtEHSAccount.CreateSPID.Trim = String.Empty Then
                '    Me.pnlShowAccCreateInfoConfirm_BySP.Visible = False
                '    Me.pnlShowAccCreateInfoConfirm_ByBO.Visible = True

                '    Me.lblConfirmCreationCreatedBy.Text = _udtEHSAccount.CreateBy
                '    'If _udtEHSAccount.CreateSPID.Equals(String.Empty) Then
                '    '    Me.lblConfirmCreationCreatedBy.Text = _udtEHSAccount.CreateBy + " (Created by DH)"
                '    'Else
                '    '    Me.lblConfirmCreationCreatedBy.Text = _udtEHSAccount.CreateBy + "(" + _udtEHSAccount.CreateSPPracticeDisplaySeq.ToString() + ") (Created by" + _udtEHSAccount.CreateBy + ")"
                '    'End If
                'Else
                '    Me.pnlShowAccCreateInfoConfirm_BySP.Visible = True
                '    Me.pnlShowAccCreateInfoConfirm_ByBO.Visible = False

                '    Me.lblConfirmCreationSPName.Text = _udtEHSAccount.CreateSPID
                '    Me.lblConfirmCreationPractice.Text = Me.ddlEnterCreationDetailPractice.SelectedItem.Text
                'End If

                'If Me.ddlEnterCreationDetailScheme.SelectedIndex = 0 Then
                '    Me.pnlShowSchemeAccCreateConfirm.Visible = False
                'Else
                '    Me.pnlShowSchemeAccCreateConfirm.Visible = True
                '    Me.lblConfirmCreationScheme.Text = Me.ddlEnterCreationDetailScheme.SelectedItem.Text
                'End If
            Else
                Me.pnlShowAccCreateInfoConfirm_BySP.Visible = False
                Me.pnlShowAccCreateInfoConfirm_ByBO.Visible = False
            End If

        End If
    End Sub

#End Region

#Region "CCCode"


    Protected Sub ucInputDocumentType_SelectChineseName_HKIC(ByVal mode As ucInputDocTypeBase.BuildMode, ByVal udcInputDocumentType As ucInputDocTypeBase, ByVal strDocCode As String, ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ucInputDocumentType.SelectChineseName_mode, ucInputDocumentType_NewAcc.SelectChineseName_mode
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00084, AuditLogDesc.ClickSelectChineseNameButton)

        Dim sm As SystemMessage

        Me.Session.Remove(SESS_ClickSave)

        'Sender = Nothing => User Click "Save" Btn to fire this event
        If IsNothing(sender) Then
            Session(SESS_ClickSave) = True
        End If

        Select strDocCode
            Case DocTypeModel.DocTypeCode.HKIC
                Dim udcInputHKID As ucInputHKID = CType(udcInputDocumentType, ucInputHKID)
                udcInputHKID.SetErrorImage(False)
                udcInputHKID.SetProperty(mode)

                If udcInputHKID.CCCodeIsEmpty Then

                    'No CCCode
                    udcInputHKID.SetCnameAmend(String.Empty)

                    sm = New SystemMessage(CommonFunctionCode, Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00143)
                    Me.udcMsgBox.AddMessage(sm)
                    udcInputHKID.SetCCCodeError(True)

                Else
                    ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [Start][Winnie] Step 1
                    Me.udcCCCode.DocCode = DocTypeModel.DocTypeCode.HKIC
                    Me.udcCCCode.CCCode1 = udcInputHKID.GetCCCode(udcInputHKID.CCCode1, Me.udcCCCode.getCCCodeFromSession(1, FuncCode))
                    Me.udcCCCode.CCCode2 = udcInputHKID.GetCCCode(udcInputHKID.CCCode2, Me.udcCCCode.getCCCodeFromSession(2, FuncCode))
                    Me.udcCCCode.CCCode3 = udcInputHKID.GetCCCode(udcInputHKID.CCCode3, Me.udcCCCode.getCCCodeFromSession(3, FuncCode))
                    Me.udcCCCode.CCCode4 = udcInputHKID.GetCCCode(udcInputHKID.CCCode4, Me.udcCCCode.getCCCodeFromSession(4, FuncCode))
                    Me.udcCCCode.CCCode5 = udcInputHKID.GetCCCode(udcInputHKID.CCCode5, Me.udcCCCode.getCCCodeFromSession(5, FuncCode))
                    Me.udcCCCode.CCCode6 = udcInputHKID.GetCCCode(udcInputHKID.CCCode6, Me.udcCCCode.getCCCodeFromSession(6, FuncCode))
                    ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [End][Winnie] Step 1

                    Me.udcCCCode.RowDisplayStyle = ChooseCCCode.DisplayStyle.SingalRow

                    ' INT20-0047 (Fix throw error for invalid CCCode) [Start][Winnie]
                    Me.udtAuditLogEntry.AddDescripton("CCCode1", Me.udcCCCode.CCCode1)
                    Me.udtAuditLogEntry.AddDescripton("CCCode2", Me.udcCCCode.CCCode2)
                    Me.udtAuditLogEntry.AddDescripton("CCCode3", Me.udcCCCode.CCCode3)
                    Me.udtAuditLogEntry.AddDescripton("CCCode4", Me.udcCCCode.CCCode4)
                    Me.udtAuditLogEntry.AddDescripton("CCCode5", Me.udcCCCode.CCCode5)
                    Me.udtAuditLogEntry.AddDescripton("CCCode6", Me.udcCCCode.CCCode6)
                    ' INT20-0047 (Fix throw error for invalid CCCode) [End][Winnie]

                    sm = Me.udcCCCode.BindCCCode()
                    'Bind CCCode Drop Down List
                    If sm Is Nothing Then
                        udcInputHKID.SetCCCodeError(False)
                        Me.ModalPopupExtenderChooseCCCode.Show()
                        Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00085, AuditLogDesc.ChineseNameCodeCheckingSuccess)
                    Else
                        sm = New SystemMessage(CommonFunctionCode, Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00039)
                        Me.udcMsgBox.AddMessage(sm)
                        udcInputHKID.SetCCCodeError(True)
                    End If
                End If



            Case DocTypeModel.DocTypeCode.ROP140
                Dim udcInputROP140 As ucInputROP140 = CType(udcInputDocumentType, ucInputROP140)
                udcInputROP140.SetErrorImage(mode, False)
                udcInputROP140.SetProperty(mode)

                If udcInputROP140.CCCodeIsEmpty Then
                    'No CCCode
                    udcInputROP140.SetCnameAmend(String.Empty)

                    sm = New SystemMessage(CommonFunctionCode, Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00143)
                    Me.udcMsgBox.AddMessage(sm)
                    udcInputROP140.SetCCCodeError(True)

                Else
                    Me.udcCCCode.DocCode = DocTypeModel.DocTypeCode.ROP140
                    Me.udcCCCode.CCCode1 = udcInputROP140.GetCCCode(udcInputROP140.CCCode1, Me.udcCCCode.getCCCodeFromSession(1, FuncCode))
                    Me.udcCCCode.CCCode2 = udcInputROP140.GetCCCode(udcInputROP140.CCCode2, Me.udcCCCode.getCCCodeFromSession(2, FuncCode))
                    Me.udcCCCode.CCCode3 = udcInputROP140.GetCCCode(udcInputROP140.CCCode3, Me.udcCCCode.getCCCodeFromSession(3, FuncCode))
                    Me.udcCCCode.CCCode4 = udcInputROP140.GetCCCode(udcInputROP140.CCCode4, Me.udcCCCode.getCCCodeFromSession(4, FuncCode))
                    Me.udcCCCode.CCCode5 = udcInputROP140.GetCCCode(udcInputROP140.CCCode5, Me.udcCCCode.getCCCodeFromSession(5, FuncCode))
                    Me.udcCCCode.CCCode6 = udcInputROP140.GetCCCode(udcInputROP140.CCCode6, Me.udcCCCode.getCCCodeFromSession(6, FuncCode))

                    Me.udcCCCode.RowDisplayStyle = ChooseCCCode.DisplayStyle.SingalRow

                    Me.udtAuditLogEntry.AddDescripton("CCCode1", Me.udcCCCode.CCCode1)
                    Me.udtAuditLogEntry.AddDescripton("CCCode2", Me.udcCCCode.CCCode2)
                    Me.udtAuditLogEntry.AddDescripton("CCCode3", Me.udcCCCode.CCCode3)
                    Me.udtAuditLogEntry.AddDescripton("CCCode4", Me.udcCCCode.CCCode4)
                    Me.udtAuditLogEntry.AddDescripton("CCCode5", Me.udcCCCode.CCCode5)
                    Me.udtAuditLogEntry.AddDescripton("CCCode6", Me.udcCCCode.CCCode6)

                    sm = Me.udcCCCode.BindCCCode()
                    'Bind CCCode Drop Down List
                    If sm Is Nothing Then
                        udcInputROP140.SetCCCodeError(False)
                        Me.ModalPopupExtenderChooseCCCode.Show()
                        Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00085, AuditLogDesc.ChineseNameCodeCheckingSuccess)
                    Else
                        sm = New SystemMessage(CommonFunctionCode, Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00039)
                        Me.udcMsgBox.AddMessage(sm)
                        udcInputROP140.SetCCCodeError(True)
                    End If
                End If
        End Select

        If Not IsNothing(sender) Then
            Me.udcMsgBox.BuildMessageBox(strValidationFail, udtAuditLogEntry, LogID.LOG00086, AuditLogDesc.ChineseNameCodeCheckingFail)
        End If

    End Sub

    Private Function NeedPopupCCCodeDialog(ByVal mode As ucInputDocTypeBase.BuildMode, ByVal strDocCode As String) As Boolean
        'isDiff is using for check the sessoion CCCode is same as current CCCode 
        'isDiff = true : sessoion CCCode <> current CCCode 
        Dim isDiff As Boolean = True
        Select Case strDocCode
            Case DocTypeModel.DocTypeCode.HKIC
                Dim udcInputHKIC As ucInputHKID

                If mode = ucInputDocTypeBase.BuildMode.Creation Then
                    udcInputHKIC = Me.ucInputDocumentType_NewAcc.GetHKICControl()
                Else
                    udcInputHKIC = Me.ucInputDocumentType.GetHKICControl()
                End If

                udcInputHKIC.SetProperty(mode)
                isDiff = Me.udcCCCode.CCCodeDiff(udcInputHKIC.CCCode1, FuncCode, 1)

                If Not isDiff Then
                    isDiff = Me.udcCCCode.CCCodeDiff(udcInputHKIC.CCCode2, FuncCode, 2)
                End If
                If Not isDiff Then
                    isDiff = Me.udcCCCode.CCCodeDiff(udcInputHKIC.CCCode3, FuncCode, 3)
                End If
                If Not isDiff Then
                    isDiff = Me.udcCCCode.CCCodeDiff(udcInputHKIC.CCCode4, FuncCode, 4)
                End If
                If Not isDiff Then
                    isDiff = Me.udcCCCode.CCCodeDiff(udcInputHKIC.CCCode5, FuncCode, 5)
                End If
                If Not isDiff Then
                    isDiff = Me.udcCCCode.CCCodeDiff(udcInputHKIC.CCCode6, FuncCode, 6)
                End If

            Case DocTypeModel.DocTypeCode.ROP140

                Dim udcInputROP140 As ucInputROP140

                If mode = ucInputDocTypeBase.BuildMode.Creation Then
                    udcInputROP140 = Me.ucInputDocumentType_NewAcc.GetROP140Control()
                Else
                    udcInputROP140 = Me.ucInputDocumentType.GetROP140Control()
                End If

                udcInputROP140.SetProperty(mode)
                isDiff = Me.udcCCCode.CCCodeDiff(udcInputROP140.CCCode1, FuncCode, 1)

                If Not isDiff Then
                    isDiff = Me.udcCCCode.CCCodeDiff(udcInputROP140.CCCode2, FuncCode, 2)
                End If
                If Not isDiff Then
                    isDiff = Me.udcCCCode.CCCodeDiff(udcInputROP140.CCCode3, FuncCode, 3)
                End If
                If Not isDiff Then
                    isDiff = Me.udcCCCode.CCCodeDiff(udcInputROP140.CCCode4, FuncCode, 4)
                End If
                If Not isDiff Then
                    isDiff = Me.udcCCCode.CCCodeDiff(udcInputROP140.CCCode5, FuncCode, 5)
                End If
                If Not isDiff Then
                    isDiff = Me.udcCCCode.CCCodeDiff(udcInputROP140.CCCode6, FuncCode, 6)
                End If

        End Select


        Return isDiff
    End Function

    Private Sub udcChooseCCCode_Cancel(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcCCCode.Cancel
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00088, AuditLogDesc.CancelChineseName)

        Me.ModalPopupExtenderChooseCCCode.Hide()
    End Sub

    Private Sub udcChooseCCCode_Confirm(ByVal strDocCode As String, ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcCCCode.Confirm
        Dim _udtEHSAccount As EHSAccountModel
        Dim mode As ucInputDocTypeBase.BuildMode
        Dim strCName As String = String.Empty


        Select Case strDocCode
            Case DocTypeModel.DocTypeCode.HKIC

                Dim udcIputHKIC As ucInputHKID = Me.ucInputDocumentType.GetHKICControl

                If udcIputHKIC Is Nothing Then
                    'it indicates that the current mode is "Creation Mode"
                    udcIputHKIC = Me.ucInputDocumentType_NewAcc.GetHKICControl
                    udcIputHKIC.SetProperty(ucInputDocTypeBase.BuildMode.Creation)
                    _udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FuncCode)
                    mode = ucInputDocTypeBase.BuildMode.Creation
                Else
                    'Modification mode
                    udcIputHKIC.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
                    _udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FuncCode)
                    mode = ucInputDocTypeBase.BuildMode.Modification
                End If

                'Dim _udtEHSAccount As EHSAccountModel = Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FuncCode)
                Me.udcCCCode.DocCode = DocTypeModel.DocTypeCode.HKIC
                Me.udcCCCode.CCCode1 = udcIputHKIC.CCCode1
                Me.udcCCCode.CCCode2 = udcIputHKIC.CCCode2
                Me.udcCCCode.CCCode3 = udcIputHKIC.CCCode3
                Me.udcCCCode.CCCode4 = udcIputHKIC.CCCode4
                Me.udcCCCode.CCCode5 = udcIputHKIC.CCCode5
                Me.udcCCCode.CCCode6 = udcIputHKIC.CCCode6


                'Get Chinese Name from Drop Down List, Save to Session
                udcCCCode.CleanSession(FuncCode)
                strCName = Me.udcCCCode.GetChineseName(FuncCode, True)
                'udcIputHKIC.SetCName(strCName)
                udcIputHKIC.SetCnameAmend(strCName)

                _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKIC).CName = strCName

                If mode = ucInputDocTypeBase.BuildMode.Creation Then
                    Me.udteHSAccountMaintBLL.EHSAccountSaveToSession(_udtEHSAccount, FuncCode)
                Else
                    Me.udteHSAccountMaintBLL.EHSAccount_Amend_SaveToSession(_udtEHSAccount, FuncCode)
                End If




            Case DocTypeModel.DocTypeCode.ROP140

                Dim udcIputROP140 As ucInputROP140 = Me.ucInputDocumentType.GetROP140Control

                If udcIputROP140 Is Nothing Then
                    'it indicates that the current mode is "Creation Mode"
                    udcIputROP140 = Me.ucInputDocumentType_NewAcc.GetROP140Control
                    udcIputROP140.SetProperty(ucInputDocTypeBase.BuildMode.Creation)
                    _udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FuncCode)
                    mode = ucInputDocTypeBase.BuildMode.Creation
                Else
                    'Modification mode
                    udcIputROP140.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
                    _udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FuncCode)
                    mode = ucInputDocTypeBase.BuildMode.Modification
                End If

                Me.udcCCCode.DocCode = DocTypeModel.DocTypeCode.ROP140
                Me.udcCCCode.CCCode1 = udcIputROP140.CCCode1
                Me.udcCCCode.CCCode2 = udcIputROP140.CCCode2
                Me.udcCCCode.CCCode3 = udcIputROP140.CCCode3
                Me.udcCCCode.CCCode4 = udcIputROP140.CCCode4
                Me.udcCCCode.CCCode5 = udcIputROP140.CCCode5
                Me.udcCCCode.CCCode6 = udcIputROP140.CCCode6


                'Get Chinese Name from Drop Down List, Save to Session
                udcCCCode.CleanSession(FuncCode)
                strCName = Me.udcCCCode.GetChineseName(FuncCode, True)
                'udcIputHKIC.SetCName(strCName)
                udcIputROP140.SetCnameAmend(strCName)

                _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.ROP140).CName = strCName

                If mode = ucInputDocTypeBase.BuildMode.Creation Then
                    Me.udteHSAccountMaintBLL.EHSAccountSaveToSession(_udtEHSAccount, FuncCode)
                Else
                    Me.udteHSAccountMaintBLL.EHSAccount_Amend_SaveToSession(_udtEHSAccount, FuncCode)
                End If
        End Select

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("ChineseName", strCName)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00087, AuditLogDesc.ConfirmChineseName)

        Me.ModalPopupExtenderChooseCCCode.Hide()

        Dim blnClickSave As Boolean = False
        If Not IsNothing(Session(SESS_ClickSave)) Then
            ' CCCode incorrect & user had clicked "Save" btn in Rectify Account
            blnClickSave = CBool(Session(SESS_ClickSave))
            If blnClickSave Then
                Session(SESS_ClickSave) = Nothing
                Me.Session.Remove(SESS_ClickSave)

                'Validation
                If mode = ucInputDocTypeBase.BuildMode.Creation Then
                    Me.ibtnNewAccountSave_Click(Nothing, Nothing)
                Else
                    Me.ibtnAmendSave_Click(Nothing, Nothing)
                End If
            End If
        End If
    End Sub

#End Region

#Region "Action Model"
    Public Enum ActionModel
        [ReadOnly]
        Amending
        ReadOnly_N_Amending
        Creation

    End Enum

    Public Enum EditAccountModel
        SuspendEnquiry
        ReactiveEnquiry
        SuspendAccount
        ReactiveAccount
        TerminateAccount
        Amending
        None
        NewAccount
    End Enum

    Public Enum PopupActionModel
        Remove
        MarkAsImmdValid
        ConfirmAsValidatedAcct
        RelaseForRectification
        WithdrawAmendment
        CancelValidation
    End Enum
#End Region

#Region "Pop-up event"
    Public Overrides Sub GridViewHeaderImage_Click(ByVal sender As Object, ByVal e As Common.Component.SortedGridviewHeader.SortedGridviewHeaderModel.GridViewHeaderImageEventArgs)
        'Dim intColumn As Integer
        'intColumn = e.intColumn

        popupDocTypeHelp.Show()
        udcDocTypeLegend.BindDocType(Session("language"))
    End Sub

    Protected Sub ibtnCloseDocTypeHelp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCloseDocTypeHelp.Click
        popupDocTypeHelp.Hide()
    End Sub

#End Region


    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <param name="strSearchAccountID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsValidEHSAccountNumber(ByVal strSearchAccountID As String) As Boolean
        Dim arrAccountID() As String = strSearchAccountID.Split(EHAccountIDSeparator)
        For i As Integer = 0 To arrAccountID.Length - 1
            'CRE13-006 HCVS Ceiling [Start][Karl]
            If arrAccountID(i).Trim() <> String.Empty Then
                If Not udtValidator.chkValidatedEHSAccountNumber(arrAccountID(i).Trim()) Then
                    'If arrAccountID(0).Trim() <> String.Empty Then
                    '    If Not udtvalidator.chkValidatedEHSAccountNumber(arrAccountID(0).Trim()) Then
                    'CRE13-006 HCVS Ceiling [End][Karl]
                    Return False
                End If
            End If
        Next

        Return True
    End Function

    Protected Sub ibtnASBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtAuditLogEntry.WriteEndLog(AuditLogDesc.BatchAccountBackClick_ID, AuditLogDesc.BatchAccountBackClick)
        Me.mveHSAccount.SetActiveView(Me.vSearchResult)
    End Sub

    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ibtnRSuspendSelectedAccount_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtAuditLogEntry.WriteEndLog(AuditLogDesc.BatchSuspendAccountClick_ID, AuditLogDesc.BatchSuspendAccountClick)
        BatchAccountActionSave(EditAccountModel.SuspendAccount)
    End Sub

    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ibtnRTerminateSelectedAccount_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtAuditLogEntry.WriteEndLog(AuditLogDesc.BatchTerminateAccountClick_ID, AuditLogDesc.BatchTerminateAccountClick)
        BatchAccountActionSave(EditAccountModel.TerminateAccount)
    End Sub

    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ibtnRReactivateSelectedAccount_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtAuditLogEntry.WriteEndLog(AuditLogDesc.BatchReactivateAccountClick_ID, AuditLogDesc.BatchReactivateAccountClick)
        BatchAccountActionSave(EditAccountModel.ReactiveAccount)
    End Sub

    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ibtnRSuspendEnquirySelectedAccount_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        BatchAccountActionSave(EditAccountModel.SuspendEnquiry)
    End Sub

    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ibtnRReactivateEnquirySelectedAccount_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        BatchAccountActionSave(EditAccountModel.ReactiveEnquiry)
    End Sub

    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <param name="enumEditAccountModel"></param>
    ''' <remarks></remarks>
    Private Sub BatchAccountActionSave(ByVal enumEditAccountModel As EditAccountModel)
        Dim udtStaticDataBLL As New Common.Component.StaticData.StaticDataBLL
        Dim blnWithSuspiciousClaim As Boolean = False
        Dim strAccountIDList As String = String.Empty

        Dim gv As GridView = Nothing
        Dim dt As New DataTable
        dt.Columns.Add("Account_ID", GetType(String))
        dt.Columns.Add("Account_ID_F", GetType(String))
        dt.Columns.Add("Document_Code", GetType(String))
        dt.Columns.Add("Document_No_FM", GetType(String))
        dt.Columns.Add("Document_No", GetType(String))
        dt.Columns.Add("Eng_Name", GetType(String))
        dt.Columns.Add("Chi_Name", GetType(String))
        dt.Columns.Add("DOB", GetType(String))
        dt.Columns.Add("Sex", GetType(String))
        dt.Columns.Add("Account_Status", GetType(String))
        dt.Columns.Add("With_Suspicious_Claim", GetType(String))
        dt.Columns.Add("Account_Source", GetType(String))

        If tcSearchRoute.ActiveTabIndex = 0 Then
            '    ' Route 1
            '    gv = Me.gvAcctListR1
            'Else
            ' Route 2
            gv = Me.gvAcctListR2
        Else
            ' Route 3
            gv = Me.gvAcctListR3

        End If

        Dim blnErrorFound As Boolean = False
        For Each gvr As GridViewRow In gv.Rows
            Dim imgWarning As Image = gvr.FindControl("imgWarning")
            imgWarning.Visible = False

            If CType(gvr.FindControl("chkSelect"), CheckBox).Checked Then
                ' Validation
                Dim strAccountStatus As String = CType(gvr.FindControl("hfAccountStatus"), TextBox).Text
                Dim strAccountID As String = CType(gvr.FindControl("hfAccountID"), TextBox).Text.Trim
                Dim strDocCode As String = CType(gvr.FindControl("lblDocType"), Label).Text.Trim
                Dim strDocNo As String = CType(gvr.FindControl("lblIdentityNumUnmask"), Label).Text.Trim
                Dim strEnquiryStatus As String = CType(gvr.FindControl("hfEnquiryStatus"), TextBox).Text.Trim
                Dim strAccountSource As String = CType(gvr.FindControl("hfAccountSource"), TextBox).Text.Trim

                ' Concat selected account ID for logging
                If strAccountIDList.Length > 0 Then
                    strAccountIDList &= ", "
                End If
                strAccountIDList &= strAccountID

                ' Action title
                Select Case enumEditAccountModel
                    Case EditAccountModel.TerminateAccount
                        If strAccountStatus <> AccountStatusClass.Active AndAlso strAccountStatus <> AccountStatusClass.Suspended Then
                            imgWarning.Visible = True
                            If Not blnErrorFound Then
                                udcMsgBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00008)
                                udcMsgBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLogEntry, AuditLogDesc.BatchTerminateAccountFail_ID, AuditLogDesc.BatchTerminateAccountFail)
                            End If
                            blnErrorFound = True
                        End If
                    Case EditAccountModel.SuspendAccount
                        If strAccountStatus <> AccountStatusClass.Active Then
                            imgWarning.Visible = True
                            If Not blnErrorFound Then
                                udcMsgBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00007)
                                udcMsgBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLogEntry, AuditLogDesc.BatchSuspendAccountFail_ID, AuditLogDesc.BatchSuspendAccountFail)
                            End If
                            blnErrorFound = True
                        End If
                    Case EditAccountModel.ReactiveAccount
                        If strAccountStatus <> AccountStatusClass.Suspended Then
                            imgWarning.Visible = True
                            If Not blnErrorFound Then
                                udcMsgBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00009)
                                udcMsgBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLogEntry, AuditLogDesc.BatchReactivateAccountFail_ID, AuditLogDesc.BatchReactivateAccountFail)
                            End If
                            blnErrorFound = True
                        End If
                    Case EditAccountModel.SuspendEnquiry
                        If Not strEnquiryStatus.Trim.Equals(EHSAccountModel.EnquiryStatusClass.Available) Then
                            imgWarning.Visible = True
                            If Not blnErrorFound Then
                                udcMsgBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00010)
                                udcMsgBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail)
                            End If
                            blnErrorFound = True
                        End If
                    Case EditAccountModel.ReactiveEnquiry
                        If strEnquiryStatus.Trim.Equals(EHSAccountModel.EnquiryStatusClass.Available) Then
                            imgWarning.Visible = True
                            If Not blnErrorFound Then
                                udcMsgBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00011)
                                udcMsgBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail)
                            End If
                            blnErrorFound = True
                        End If
                End Select

                If blnErrorFound Then
                    Continue For
                End If

                Dim dr As DataRow = dt.NewRow
                dr("Account_ID_F") = CType(gvr.FindControl("lbtnAccountID"), LinkButton).Text.Trim
                dr("Account_ID") = strAccountID
                dr("Document_Code") = strDocCode
                dr("Document_No_FM") = CType(gvr.FindControl("lblIdentityNum"), Label).Text.Trim
                dr("Document_No") = strDocNo
                dr("Eng_Name") = CType(gvr.FindControl("lblName"), Label).Text.Trim
                dr("Chi_Name") = CType(gvr.FindControl("lblCName"), Label).Text.Trim
                dr("DOB") = CType(gvr.FindControl("lblDOB"), Label).Text.Trim
                dr("Sex") = CType(gvr.FindControl("lblSex"), Label).Text.Trim
                dr("Account_Status") = CType(gvr.FindControl("lblAccountStatus"), Label).Text.Trim
                dr("Account_Source") = strAccountSource

                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
                ' -------------------------------------------------------------------------
                'Dim dtMatchResult As DataTable = (New eHealthAccountDeathRecordBLL).GetDeathRecordMatchResult(String.Empty, udtformatter.formatDocumentIdentityNumber(strDocCode, udtformatter.formatHKIDInternal(strDocNo)), String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, -1, -1)
                Dim bllSearchResult As Common.Component.BaseBLL.BLLSearchResult
                bllSearchResult = (New eHealthAccountDeathRecordBLL).GetDeathRecordMatchResult(Me.FunctionCode, String.Empty, udtformatter.formatDocumentIdentityNumber(strDocCode, udtformatter.formatHKIDInternal(strDocNo)), String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, -1, -1, Nothing, True)
                Dim dtMatchResult As DataTable = CType(bllSearchResult.Data, DataTable)

                'Dim ds As DataSet = (New eHealthAccountDeathRecordBLL).GetDeathRecordMatchResultDetail(strAccountID, strDocCode)
                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

                If dtMatchResult.Rows.Count = 0 Then
                    ' Real time checking with suspicious transaction
                    dr("With_Suspicious_Claim") = Me.GetGlobalResourceObject("Text", "N/A")
                Else
                    ' Real time checking without suspicious transaction
                    dr("With_Suspicious_Claim") = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("YesNo", dtMatchResult.Rows(0)("With_Suspicious_Claim")).DataValue

                    If (dtMatchResult.Rows(0)("With_Suspicious_Claim") = eHealthAccountDeathRecord.eHealthAccountDeathRecordBLL.DeathRecordMatchResult.WithSuspiciousClaim.Y) Then
                        blnWithSuspiciousClaim = True
                    End If

                End If

                dt.Rows.Add(dr)

            End If

        Next

        ' Log selected account ID
        Me.udtAuditLogEntry.AddDescripton("Account ID", strAccountIDList)
        Me.udtAuditLogEntry.WriteLog(AuditLogDesc.BatchAccountActionSelection_ID, AuditLogDesc.BatchAccountActionSelection)

        If blnErrorFound Then
            ' Warning found, Block futher action
            Return
        End If

        If dt.Rows.Count = 0 Then
            udcMsgBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00023)
            udcMsgBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, Me.udtAuditLogEntry, AuditLogDesc.BatchAccountActionFailNoSelection_ID, AuditLogDesc.BatchAccountActionFailNoSelection)

            Return
        End If

        ' Action title
        Select Case enumEditAccountModel
            Case EditAccountModel.TerminateAccount
                lblASActionTitle.Text = Me.GetGlobalResourceObject("Text", "TitleTerminateAccount")
                lblASActionReasonText.Text = Me.GetGlobalResourceObject("Text", "AccountTerminateReason")
                lblConfirmReasonText.Text = Me.GetGlobalResourceObject("Text", "AccountTerminateReason")
            Case EditAccountModel.SuspendAccount
                lblASActionTitle.Text = Me.GetGlobalResourceObject("Text", "TitleSuspendAccount")
                lblASActionReasonText.Text = Me.GetGlobalResourceObject("Text", "AccountSuspendReason")
                lblConfirmReasonText.Text = Me.GetGlobalResourceObject("Text", "AccountSuspendReason")
            Case EditAccountModel.ReactiveAccount
                lblASActionTitle.Text = Me.GetGlobalResourceObject("Text", "TitleReactivateAccount")
                lblASActionReasonText.Text = Me.GetGlobalResourceObject("Text", "AccountReactivateReason")
                lblConfirmReasonText.Text = Me.GetGlobalResourceObject("Text", "AccountReactivateReason")
            Case EditAccountModel.SuspendEnquiry
                lblASActionTitle.Text = Me.GetGlobalResourceObject("Text", "TitleSuspendEnquiryAccount")
                lblASActionReasonText.Text = Me.GetGlobalResourceObject("Text", "PublicEnquirySuspendReason")
                lblConfirmReasonText.Text = Me.GetGlobalResourceObject("Text", "PublicEnquirySuspendReason")
            Case EditAccountModel.ReactiveEnquiry
                lblASActionTitle.Text = Me.GetGlobalResourceObject("Text", "TitleReactivateEnquiryAccount")
                lblASActionReasonText.Text = Me.GetGlobalResourceObject("Text", "PublicEnquiryReactivateReason")
                lblConfirmReasonText.Text = Me.GetGlobalResourceObject("Text", "PublicEnquiryReactivateReason")
        End Select

        Session(SESS_ActionMode) = enumEditAccountModel

        ' Grid
        gvAS.DataSource = dt
        gvAS.DataBind()

        gvAC.DataSource = dt
        gvAC.DataBind()

        ' Set confirm view for multiple account
        panConfrimBatchAccount.Visible = True
        udcConfirmAccount.Visible = False

        ' Reason
        txtASActionReason.Text = String.Empty
        imgASActionReasonErr.Visible = False

        udcInfoMsgBox.Visible = False
        udcMsgBox.Visible = False

        If blnWithSuspiciousClaim Then
            Me.udcInfoMsgBox.AddMessage(New SystemMessage(FuncCode, "I", "00013"))
            Me.udcInfoMsgBox.BuildMessageBox()
            Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Information
        End If
        Me.mveHSAccount.SetActiveView(Me.vAccountActionSave)
    End Sub



    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ibtnASSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtAuditLogEntry.AddDescripton("Reason", txtASActionReason.Text.Trim())
        Me.udtAuditLogEntry.WriteEndLog(AuditLogDesc.BatchAccountSaveClick_ID, AuditLogDesc.BatchAccountSaveClick)
        'Check whether reason is provided
        If txtASActionReason.Text.Trim.Equals(String.Empty) Then
            Me.imgASActionReasonErr.Visible = True
            Me.udcInfoMsgBox.Visible = False

            Dim actionMode As EditAccountModel = Session(SESS_ActionMode)

            Select Case actionMode
                Case EditAccountModel.SuspendAccount
                    Me.udcMsgBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002))
                Case EditAccountModel.ReactiveAccount
                    Me.udcMsgBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00003))
                Case EditAccountModel.TerminateAccount
                    Me.udcMsgBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00006))
                Case EditAccountModel.SuspendEnquiry
                    Me.udcMsgBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00004))
                Case EditAccountModel.ReactiveEnquiry
                    Me.udcMsgBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00005))
            End Select

            Me.udcMsgBox.BuildMessageBox("ValidationFail")
            Exit Sub
        End If

        ' Go to confirm view

        ' Action title
        Me.lblConfirmReason.Text = txtASActionReason.Text.Trim

        Me.mveHSAccount.SetActiveView(Me.vConfirmAmendedAccount)
    End Sub

    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <param name="arreHSAccountID"></param>
    ''' <param name="iStartIndex"></param>
    ''' <param name="iEndIndex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetMultipleAccountIDList(ByVal arreHSAccountID() As String, ByVal iStartIndex As Integer, ByVal iEndIndex As Integer) As String
        Dim udtFormatter As New Formatter
        Dim udtGeneralFunction = New GeneralFunction
        Dim sbResult As New StringBuilder()
        For i As Integer = iStartIndex To iEndIndex
            If sbResult.Length > 0 Then sbResult.Append(", ")
            sbResult.Append(udtFormatter.formatValidatedEHSAccountNumber(arreHSAccountID(i)))
        Next

        Return sbResult.ToString
    End Function

    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <param name="objEHSAccount"></param>
    ''' <param name="objEHSAccount_Amend"></param>
    ''' <param name="strUpdateBy"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SuspendAccount(ByVal objEHSAccount As EHSAccountModel, ByVal objEHSAccount_Amend As EHSAccountModel, ByVal strUpdateBy As String) As SystemMessage
        'Suspend Account
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)

        Me.udtAuditLogEntry.AddDescripton("AccountID", objEHSAccount_Amend.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("Reason", objEHSAccount_Amend.Remark)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", objEHSAccount_Amend.AccountSourceString)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00036, AuditLogDesc.SaveSuspendAccount)

        Me.udtEHSAccountBLL.SuspendEHSAccountByBackOffice(objEHSAccount_Amend, strUpdateBy)
        udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00004)

        'Add audit log
        Me.udtAuditLogEntry.AddDescripton("AccountID", objEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", objEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00037, AuditLogDesc.SaveSuspendAccountSuccess)

        Return udtSM
    End Function

    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <param name="objEHSAccount"></param>
    ''' <param name="objEHSAccount_Amend"></param>
    ''' <param name="strUpdateBy"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function TerminateAccount(ByVal objEHSAccount As EHSAccountModel, ByVal objEHSAccount_Amend As EHSAccountModel, ByVal strUpdateBy As String) As SystemMessage
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)

        Me.udtAuditLogEntry.AddDescripton("AccountID", objEHSAccount_Amend.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", objEHSAccount_Amend.AccountSourceString)
        Me.udtAuditLogEntry.AddDescripton("Reason", objEHSAccount_Amend.Remark)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00048, AuditLogDesc.SaveTerminateAccount)

        Me.udtEHSAccountBLL.TerminateEHSAccountByBackOffice(objEHSAccount_Amend, strUpdateBy)
        udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00004)

        'Add audit log
        Me.udtAuditLogEntry.AddDescripton("AccountID", objEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", objEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00049, AuditLogDesc.SaveTerminateAccountSuccess)

        Return udtSM
    End Function

    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <param name="objEHSAccount"></param>
    ''' <param name="objEHSAccount_Amend"></param>
    ''' <param name="strUpdateBy"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ReactivateAccount(ByVal objEHSAccount As EHSAccountModel, ByVal objEHSAccount_Amend As EHSAccountModel, ByVal strUpdateBy As String) As SystemMessage
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)

        Me.udtAuditLogEntry.AddDescripton("AccountID", objEHSAccount_Amend.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", objEHSAccount_Amend.AccountSourceString)
        Me.udtAuditLogEntry.AddDescripton("Reason", objEHSAccount_Amend.Remark)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00042, AuditLogDesc.SaveReactivateAccount)

        Me.udtEHSAccountBLL.ReactivateEHSAccountByBackOffice(objEHSAccount_Amend, strUpdateBy)
        udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00004)

        'Add audit log
        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00043, AuditLogDesc.SaveReactivateAccountSuccess)

        Return udtSM
    End Function


    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <param name="objEHSAccount"></param>
    ''' <param name="objEHSAccount_Amend"></param>
    ''' <param name="strUpdateBy"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SuspendEnquiryAccount(ByVal objEHSAccount As EHSAccountModel, ByVal objEHSAccount_Amend As EHSAccountModel, ByVal strUpdateBy As String) As SystemMessage
        'Suspend Public Enquiry
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)

        Me.udtAuditLogEntry.AddDescripton("AccountID", objEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", objEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00024, AuditLogDesc.SaveSuspendEnquiry)

        Me.udtEHSAccountBLL.SuspendEHSAccountPublicEnquiryByBackOffice(objEHSAccount_Amend, strUpdateBy)
        udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00010)

        'Add audit log
        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00025, AuditLogDesc.SaveSuspendEnquirySuccess)

        Return udtSM
    End Function

    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <param name="objEHSAccount"></param>
    ''' <param name="objEHSAccount_Amend"></param>
    ''' <param name="strUpdateBy"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ReactivateEnquiryAccount(ByVal objEHSAccount As EHSAccountModel, ByVal objEHSAccount_Amend As EHSAccountModel, ByVal strUpdateBy As String) As SystemMessage
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)

        Me.udtAuditLogEntry.AddDescripton("AccountID", objEHSAccount_Amend.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", objEHSAccount_Amend.AccountSourceString)
        Me.udtAuditLogEntry.AddDescripton("Reason", objEHSAccount_Amend.PublicEnquiryRemark)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00030, AuditLogDesc.SaveReactiveEnquiry)

        Me.udtEHSAccountBLL.ReactivateEHSAccountPublicEnquiryByBackOffice(objEHSAccount_Amend, strUpdateBy)
        udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00010)

        'Add audit log
        Me.udtAuditLogEntry.AddDescripton("AccountID", objEHSAccount_Amend.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", objEHSAccount_Amend.AccountSourceString)
        Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00031, AuditLogDesc.SaveReactiveEnquirySuccess)

        Return udtSM
    End Function

    Private Function BatchAccountAction(ByVal enumEditAccountModel As EditAccountModel, ByVal udtHCVUUser As HCVUUserModel) As SystemMessage
        Dim objSysMsg As SystemMessage = Nothing
        Dim strReason As String = Me.txtASActionReason.Text.Trim
        For Each gvr As GridViewRow In gvAC.Rows
            ' Validation
            Dim strAccountID As String = CType(gvr.FindControl("hfAccountID"), TextBox).Text.Trim
            Dim strAccountSource As String = CType(gvr.FindControl("hfAccountSource"), TextBox).Text.Trim
            Me.GeteHSAcc(strAccountID, strAccountSource, False)
            'Dim objEHSAccount As EHSAccountModel = Me.GeteHSAcc(strAccountID, strAccountSource)
            Dim objEHSAccount As EHSAccountModel = udtEHSAccount


            ' Action title
            Select Case enumEditAccountModel
                Case EditAccountModel.TerminateAccount
                    objEHSAccount.Remark = strReason
                    objSysMsg = Me.TerminateAccount(objEHSAccount, objEHSAccount, udtHCVUUser.UserID)
                Case EditAccountModel.SuspendAccount
                    objEHSAccount.Remark = strReason
                    objSysMsg = Me.SuspendAccount(objEHSAccount, objEHSAccount, udtHCVUUser.UserID)
                Case EditAccountModel.ReactiveAccount
                    objEHSAccount.Remark = strReason
                    objSysMsg = Me.ReactivateAccount(objEHSAccount, objEHSAccount, udtHCVUUser.UserID)
                Case EditAccountModel.SuspendEnquiry
                    objEHSAccount.PublicEnquiryRemark = strReason
                    objSysMsg = Me.SuspendEnquiryAccount(objEHSAccount, objEHSAccount, udtHCVUUser.UserID)
                Case EditAccountModel.ReactiveEnquiry
                    objEHSAccount.PublicEnquiryRemark = strReason
                    objSysMsg = Me.ReactivateEnquiryAccount(objEHSAccount, objEHSAccount, udtHCVUUser.UserID)
            End Select
        Next

        Return objSysMsg
    End Function

#Region "Implement IWorkingData (CRE11-004)"

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Account which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        'Temporarily set the values for audit log (as they are not available, but compulsory to write to audit log table) 
        If Not IsNothing(Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FuncCode)) Then
            Dim udtEHSAccount As EHSAccountModel
            udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FuncCode)
            'Account ID
            If IsNothing(udtEHSAccount.VoucherAccID) Then
                udtEHSAccount.VoucherAccID = ""
            End If
            'Create By
            If IsNothing(udtEHSAccount.CreateBy) Then
                udtEHSAccount.CreateBy = ""
            End If
            Return Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FuncCode)
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Service Provider which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Transaction which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        Dim udtSP As ServiceProviderModel = Nothing
        Dim udtEHSAccount As EHSAccountModel = Nothing
        Dim udtSPBLL As ServiceProviderBLL = Nothing
        Dim strAccountCreateBy As String = String.Empty
        Dim udtDB As Database = Nothing

        If Not IsNothing(Session(SESS_AccountCreateBy)) Then
            strAccountCreateBy = CType(Session(SESS_AccountCreateBy), String)
        End If

        udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FuncCode)
        If Not IsNothing(udtEHSAccount) Then
            If udtEHSAccount.CreateByBO Then
                'Create By BO
                If strAccountCreateBy <> udtEHSAccount.CreateBy Or IsNothing(Session(SESS_ServiceProvider)) Then
                    udtSP = New ServiceProviderModel()
                    'udtSP.SPID = udtEHSAccount.CreateBy
                    udtSP.SPID = String.Empty
                    Session(SESS_ServiceProvider) = udtSP
                    Session(SESS_AccountCreateBy) = udtEHSAccount.CreateBy
                    Return udtSP
                Else
                    udtSP = CType(Session(SESS_ServiceProvider), ServiceProviderModel)
                    Return udtSP
                End If
            Else
                'Create By SP
                If strAccountCreateBy <> udtEHSAccount.CreateBy Or IsNothing(Session(SESS_ServiceProvider)) Then
                    udtSPBLL = New ServiceProviderBLL()
                    udtDB = New Database()
                    udtSP = New ServiceProviderModel()
                    udtSP.SPID = IIf(IsNothing(udtEHSAccount.CreateSPID), String.Empty, udtEHSAccount.CreateSPID)
                    Session(SESS_ServiceProvider) = udtSP
                    Session(SESS_AccountCreateBy) = udtEHSAccount.CreateBy
                    Return udtSP
                Else
                    udtSP = CType(Session(SESS_ServiceProvider), ServiceProviderModel)
                    Return udtSP
                End If
            End If
        End If

        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve Document Code which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetDocCode() As String
        If IsNothing(Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FuncCode)) Then
            Return Nothing
        End If

        If IsNothing(txtDocCode.Text) Then
            Return Nothing
        Else
            If txtDocCode.Text.Trim = "" Then
                Return Nothing
            Else
                Return txtDocCode.Text.Trim
            End If
        End If
    End Function


#End Region

#Region "Unmask Document No (CRE11-007)"

    Protected Sub chkMaskDocumentNoAS_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        Dim chk As CheckBox = CType(sender, CheckBox)
        udtAuditLog.AddDescripton("Checked change to", IIf(chk.Checked, "T", "F"))
        udtAuditLog.WriteLog(AuditLogDesc.MaskIdentityDocumentNoASClick_ID, AuditLogDesc.MaskIdentityDocumentNoASClick)

        MaskDocumentNoCheckedChanged(sender, e)
    End Sub

    Protected Sub chkMaskDocumentNoAC_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        Dim chk As CheckBox = CType(sender, CheckBox)
        udtAuditLog.AddDescripton("Checked change to", IIf(chk.Checked, "T", "F"))
        udtAuditLog.WriteLog(AuditLogDesc.MaskIdentityDocumentNoACClick_ID, AuditLogDesc.MaskIdentityDocumentNoACClick)

        MaskDocumentNoCheckedChanged(sender, e)
    End Sub

    Protected Sub chkMaskDocumentNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        Dim chk As CheckBox = CType(sender, CheckBox)
        udtAuditLog.AddDescripton("Checked change to", IIf(chk.Checked, "T", "F"))
        udtAuditLog.WriteLog(AuditLogDesc.MaskIdentityDocumentNoClick_ID, AuditLogDesc.MaskIdentityDocumentNoClick)

        MaskDocumentNoCheckedChanged(sender, e)
    End Sub

    Protected Sub MaskDocumentNoCheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As CheckBox = CType(sender, CheckBox)

        If chk.Checked Then
            ' Unchecked -> Checked
            'gvAcctListR1.Columns(3).Visible = False
            'gvAcctListR1.Columns(2).Visible = True

            gvAcctListR2.Columns(4).Visible = False
            gvAcctListR2.Columns(3).Visible = True

            gvAcctListR3.Columns(4).Visible = False
            gvAcctListR3.Columns(3).Visible = True

            MaskBatchActionDocumentNo(True)
        Else
            ' Checked -> Unchecked
            popupUnmask.Show()
            ViewState(VS.UnmaskPopup) = PopupStatus.Active
            InitPopupUnmask()
        End If

    End Sub

    Protected Sub ibtnPopupUnmaskConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcPUInputToken.Confirm_Click
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(AuditLogDesc.MaskIdentityDocumentNoSuccess_ID, AuditLogDesc.MaskIdentityDocumentNoSuccess)

        ViewState(VS.UnmaskPopup) = PopupStatus.Closed

        Select Case tcSearchRoute.ActiveTabIndex
            'Case 0
            '    gvAcctListR1.Columns(3).Visible = True
            '    gvAcctListR1.Columns(2).Visible = False

            'Case 1
            '    gvAcctListR2.Columns(4).Visible = True
            '    gvAcctListR2.Columns(3).Visible = False

            Case 0
                gvAcctListR2.Columns(4).Visible = True
                gvAcctListR2.Columns(3).Visible = False

            Case 1
                gvAcctListR3.Columns(4).Visible = True
                gvAcctListR3.Columns(3).Visible = False
        End Select

        ' Handle Batch Action View
        If Me.mveHSAccount.ActiveViewIndex = intSave Or Me.mveHSAccount.ActiveViewIndex = intConfirm Then
            MaskBatchActionDocumentNo(False)
        End If
    End Sub

    Protected Sub ibtnPopupUnmaskCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcPUInputToken.Cancel_Click
        ViewState(VS.UnmaskPopup) = PopupStatus.Closed

        Select Case tcSearchRoute.ActiveTabIndex
            'Case 0
            '    chkMaskDocumentNoR1.Checked = True
            'Case 1
            '    chkMaskDocumentNoR2.Checked = True

            Case 0
                chkMaskDocumentNoR2.Checked = True
            Case 1
                chkMaskDocumentNoR2.Checked = True
        End Select

        ' Handle Batch Action View
        If Me.mveHSAccount.ActiveViewIndex = intSave Or Me.mveHSAccount.ActiveViewIndex = intConfirm Then
            MaskBatchActionDocumentNo(True)
        End If
    End Sub

    Private Sub InitPopupUnmask()
        ' CRE12-014 - Relax 500 row limit in back office platform [Start][Twinsen]
        popupUnmask.PopupDragHandleControlID = udcPUInputToken.Header.ClientID
        udcPUInputToken.HeaderText = Me.GetGlobalResourceObject("Text", "ConfirmUnmaskIdentityDocumentNo")
        ' CRE12-014 - Relax 500 row limit in back office platform [End] [Twinsen]
        udcPUInputToken.Message = Me.GetGlobalResourceObject("Text", "ConfirmUnmaskMessage")
        udcPUInputToken.Build()
    End Sub

    Private Sub MaskBatchActionDocumentNo(ByVal blnMask As Boolean)
        chkMaskDocumentNoAS.Checked = blnMask
        gvAS.Columns(2).Visible = blnMask
        gvAS.Columns(3).Visible = Not blnMask

        chkMaskDocumentNoAC.Checked = blnMask
        gvAC.Columns(2).Visible = blnMask
        gvAC.Columns(3).Visible = Not blnMask

    End Sub
#End Region

    ' CRE20-003-03 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
    ' -------------------------------------------------------------------------------
    Private Function getStudentFileInProcess(ByVal strTempVouhcerAccID As String) As String
        Dim udtstudentFileBLL As New StudentFile.StudentFileBLL
        Dim lstStudentFile As New List(Of String)
        Dim strStudentFileList As String = String.Empty

        Dim dt As DataTable = udtstudentFileBLL.GetStudentFileEntryByTempAccID(strTempVouhcerAccID)

        For Each dr As DataRow In dt.Rows

            Select Case CStr(dr("Record_Status"))
                Case Formatter.EnumToString(StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.Removed), _
                    Formatter.EnumToString(StudentFile.StudentFileHeaderModel.RecordStatusEnumClass.Completed) _
                    ' File already completed

                Case Else
                    ' Processing
                    Dim strStudentFileID As String = dr("Student_File_ID").ToString.Trim

                    If lstStudentFile.Contains(strStudentFileID) = False Then
                        lstStudentFile.Add(strStudentFileID)
                    End If

            End Select
        Next

        If lstStudentFile.Count > 0 Then strStudentFileList = String.Join(", ", lstStudentFile.ToArray)

        Return strStudentFileList
    End Function
    ' CRE20-003-03 Enhancement on Programme or Scheme using batch upload [End][Winnie]
End Class