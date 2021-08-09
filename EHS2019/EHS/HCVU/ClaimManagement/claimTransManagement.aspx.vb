Imports AjaxControlToolkit
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.HCVUUser
Imports Common.Component.ReasonForVisit
Imports Common.Component.RedirectParameter
Imports Common.Component.Scheme
Imports Common.Component.SortedGridviewHeader
Imports Common.Component.StaticData
Imports Common.Component.UserRole
Imports Common.Format
Imports Common.SearchCriteria
Imports Common.Validation
Imports Common.WebService.Interface
Imports System.Web.Services
Imports Common.Component.InputPicker
Imports Common.Component.HATransaction
Imports Common.Component.VoucherInfo


<System.Web.Script.Services.ScriptService()> _
Partial Public Class claimTransManagement
    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
    'Inherits BasePageWithGridView
    Inherits BasePageWithControl
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

    ' FunctionCode = FunctCode.FUNT010404
    Dim udtSM As Common.ComObject.SystemMessage
    Dim udtAuditLogEntry As AuditLogEntry

#Region "Private Classes"

    Private Class AuditLogDescription
        Public Const Load As String = "Claim Transaction Management load" '00000
        Public Const Search As String = "Search" '00001
        Public Const SearchSuccessful As String = "Search successful" '00002
        Public Const SearchFail As String = "Search fail" '00003
        Public Const SelectTransaction As String = "Select Transaction" '00004
        Public Const SelectTransactionSuccessful As String = "Select Transaction successful" '00005
        Public Const SelectTransactionFail As String = "Select Transaction fail" '00006
        Public Const PreviousRecordClick As String = "Previous Record click" '00007
        Public Const PreviousRecordSuccessful As String = "Previous Record successful" '00008
        Public Const PreviousRecordFail As String = "Previous Record fail" '00009
        Public Const NextRecordClick As String = "Next Record click" '00010
        Public Const NextRecordSuccessful As String = "Next Record successful" '00011
        Public Const NextRecordFail As String = "Next Record fail" '00012
        Public Const BatchReactivateClick As String = "Batch Reactivate click" '00013
        Public Const BatchReactivateSuccessful As String = "Batch Reactivate successful" '00014
        Public Const BatchReactivateFail As String = "Batch Reactivate fail" '00015
        Public Const BatchSuspendClick As String = "Batch Suspend click" '00016
        Public Const BatchSuspendSuccessful As String = "Batch Suspend successful" '00017
        Public Const BatchSuspendFail As String = "Batch Suspend fail" '00018
        Public Const SaveBatchSuspendReasonClick As String = "Save Batch Suspend Reason click" '00019
        Public Const SaveBatchSuspendReasonSuccessful As String = "Save Batch Suspend Reason successful" '00020
        Public Const SaveBatchSuspendReasonFail As String = "Save Batch Suspend Reason fail" '00021
        Public Const ConfirmBatchSuspendReasonClick As String = "Confirm Batch Suspend Reason click" '00022
        Public Const ConfirmBatchSuspendReasonSuccessful As String = "Confirm Batch Suspend Reason successful" '00023
        Public Const ConfirmBatchSuspendReasonFail As String = "Confirm Batch Suspend Reason fail" '00024
        Public Const BatchVoidClick As String = "Batch Void click" '00025
        Public Const BatchVoidSuccessful As String = "Batch Void successful" '00026
        Public Const BatchVoidFail As String = "Batch Void fail" '00027
        Public Const SaveBatchVoidReasonClick As String = "Save Batch Void Reason click" '00028
        Public Const SaveBatchVoidReasonSuccessful As String = "Save Batch Void Reason successful" '00029
        Public Const SaveBatchVoidReasonFail As String = "Save Batch Void Reason fail" '00030
        Public Const ConfirmBatchVoidReasonClick As String = "Confirm Batch Void Reason click" '00031
        Public Const ConfirmBatchVoidReasonSuccessful As String = "Confirm Batch Void Reason successful" '00032
        Public Const ConfirmBatchVoidReasonFail As String = "Confirm Batch Void Reason fail" '00033
        Public Const SaveSuspendReasonClick As String = "Save Suspend Reason click" '00034
        Public Const SaveSuspendReasonSuccessful As String = "Save Suspend Reason successful" '00035
        Public Const SaveSuspendReasonFail As String = "Save Suspend Reason fail" '00036
        Public Const ConfirmSuspendReasonClick As String = "Confirm Suspend Reason click" '00037
        Public Const ConfirmSuspendReasonSuccessful As String = "Confirm Suspend Reason successful" '00038
        Public Const ConfirmSuspendReasonFail As String = "Confirm Suspend Reason fail" '00039
        Public Const SaveVoidReasonClick As String = "Save Void Reason click" '00040
        Public Const SaveVoidReasonSuccessful As String = "Save Void Reason successful" '00041
        Public Const SaveVoidReasonFail As String = "Save Void Reason fail" '00042
        Public Const ConfirmVoidReasonClick As String = "Confirm Void Reason click" '00043
        Public Const ConfirmVoidReasonSuccessful As String = "Confirm Void Reason successful" '00044
        Public Const ConfirmVoidReasonFail As String = "Confirm Void Reason fail" '00045
        Public Const SearchCompleteNoRecordFound As String = "Search complete. No record found" '00046
        Public Const TransactionGridBackClick As String = "Transaction Grid Back click" '00047
        Public Const DetailBackClick As String = "Detail Back click" ' 00048
        Public Const ReturnClick As String = "Return click" '00049
        Public Const ReactivateClick As String = "Reactivate click" '00050
        Public Const ReactivateSuccessful As String = "Reactivate successful" '00051
        Public Const ReactivateFail As String = "Reactivate fail" '00052
        Public Const SuspendClick As String = "Suspend click" '00053
        Public Const VoidClick As String = "Void click" '00054
        Public Const SaveBatchVoidReasonCancelClick As String = "Cancel Save Batch Void Reason click" '00055
        Public Const ConfirmBatchVoidReasonCancelClick As String = "Cancel Confirm Batch Void Reason click" '00056
        Public Const SaveBatchSuspendReasonCancelClick As String = "Cancel Save Batch Suspend Reason click" '00057
        Public Const ConfirmBatchSuspendReasonCancelClick As String = "Cancel Confirm Batch Suspend Reason click" '00058
        Public Const SaveVoidReasonCancelClick As String = "Cancel Save Void Reason click" '00059
        Public Const ConfirmVoidReasonCancelClick As String = "Cancel Confirm Void Reason click" '00060
        Public Const SaveSuspendReasonCancelClick As String = "Cancel Save Suspend Reason click" '00061
        Public Const ConfirmSuspendReasonCancelClick As String = "Cancel Confirm Suspend Reason click" '00062
        Public Const HAConnectionFail As String = "Fail to obtain HA vaccine result " '00100
        Public Const DHConnectionFail As String = "Fail to obtain DH vaccine result " '00110
        Public Const HADHConnectionFail As String = "Fail to obtain HA & DH vaccine result " '00111

        ' New Claim Transaction
        ' - Search Account
        Public Const NewClaimTransaction_SearchAccountClick As String = "New Claim Transaction - Search Account" '00063
        Public Const NewClaimTransaction_SearchAccountClick_Success As String = "New Claim Transaction - Search Account Successful" '00064
        Public Const NewClaimTransaction_SearchAccountClick_Fail As String = "New Claim Transaction - Search Account Fail" '00065

        ' - Select Account
        Public Const NewClaimTransaction_SelectAccount = "New Claim Transaction - Select Account" '00066
        Public Const NewClaimTransaction_SelectAccount_Back = "New Claim Transaction - Select Account - Back Click" '00067

        ' - Search SP
        Public Const NewClaimTransaction_SearchSP As String = "New Claim Transaction - Search SP" '00068
        Public Const NewClaimTransaction_SearchSP_Success As String = "New Claim Transaction - Search SP Successful" '00069
        Public Const NewClaimTransaction_SearchSP_Fail As String = "New Claim Transaction - Search SP Fail" '00070
        Public Const NewClaimTransaction_SearchSP_Clear As String = "New Claim Transaction - Search SP - Clear click" '00071

        ' - Advanced Search SP
        Public Const NewClaimTransaction_AdvancedSearchSP As String = "New Claim Transaction - Advanced Search SP" '00072
        Public Const NewClaimTransaction_AdvancedSearchSP_Success As String = "New Claim Transaction - Advanced Search SP Successful" '00073
        Public Const NewClaimTransaction_AdvancedSearchSP_Fail As String = "New Claim Transaction - Advanced Search SP Fail" '00074
        Public Const NewClaimTransaction_AdvancedSearchSP_Close As String = "New Claim Transaction - Advanced Search SP - Close click" '00075

        ' - Advanced Select SP
        Public Const NewClaimTransaction_AdvancedSelectSP As String = "New Claim Transaction - Advanced Select SP" '00076
        Public Const NewClaimTransaction_AdvancedSelectSP_Back As String = "New Claim Transaction - Advanced Select SP - Back click" '00077

        ' - Enter Creation Detail
        Public Const NewClaimTransaction_EnterCreationDetail As String = "New Claim Transaction - Enter Creation Detail" '00078
        Public Const NewClaimTransaction_EnterCreationDetail_Success As String = "New Claim Transaction - Enter Creation Detail Successful" '00079
        Public Const NewClaimTransaction_EnterCreationDetail_Fail As String = "New Claim Transaction - Enter Creation Detail Fail" '00080
        Public Const NewClaimTransaction_EnterCreationDetail_Back As String = "New Claim Transaction - Enter Creation Detail - Back click" '00081

        ' - Enter Claim Detail
        Public Const NewClaimTransaction_EnterClaimDetail As String = "New Claim Transaction - Enter Claim Detail" '00082
        Public Const NewClaimTransaction_EnterClaimDetail_Success As String = "New Claim Transaction - Enter Claim Detail Successful" '00083
        Public Const NewClaimTransaction_EnterClaimDetail_Fail As String = "New Claim Transaction - Enter Claim Detail Fail" '00084
        Public Const NewClaimTransaction_EnterClaimDetail_Back As String = "New Claim Transaction - Enter Claim Detail - Back click" '00085
        Public Const NewClaimTransaction_EnterClaimDetail_WarningMsg As String = "New Claim Transaction - Enter Claim Detail - Warning Msg" '00094

        ' - Change Service Date
        Public Const NewClaimTransaction_ChangeServiceDate As String = "New Claim Transaction - Change Service Date" '00101
        Public Const NewClaimTransaction_ChangeServiceDate_Success As String = "New Claim Transaction - Change Service Date Successful" '00095
        Public Const NewClaimTransaction_ChangeServiceDate_Fail As String = "New Claim Transaction - Change Service Date Fail" '00096

        ' - Enter Override Reason
        Public Const NewClaimTransaction_EnterOverrideReason As String = "New Claim Transaction - Enter Override Reason" '00086
        Public Const NewClaimTransaction_EnterOverrideReason_Success As String = "New Claim Transaction - Enter Override Reason Successful" '00087
        Public Const NewClaimTransaction_EnterOverrideReason_Fail As String = "New Claim Transaction - Enter Override Reason Fail" '00088
        Public Const NewClaimTransaction_EnterOverrideReason_Cancel As String = "New Claim Transaction - Enter Override Reason - Cancel click" '00089

        ' - Confirm Claim Transaction Creation
        Public Const NewClaimTransaction_ConfirmClaim As String = "New Claim Transaction - Confirm Claim" '00090
        Public Const NewClaimTransaction_ConfirmClaim_Success As String = "New Claim Transaction - Confirm Claim Successful" '00091
        Public Const NewClaimTransaction_ConfirmClaim_Fail As String = "New Claim Transaction - Confirm Claim Fail" '00092
        Public Const NewClaimTransaction_ConfirmClaim_Cancel As String = "New Claim Transaction - Confirm Claim - Cancel click" '00093

        ' - Delete Pending Approval Record
        Public Const DeleteClaim As String = "Delete Claim" '00106
        Public Const DeleteClaim_Success As String = "Delete Claim Successful" '00102
        Public Const DeleteClaim_Fail As String = "Delete Claim Fail" '00097
        Public Const DeleteClaim_Click As String = "Delete Claim Click" '00098
        Public Const DeleteClaim_Cancel As String = "Delete Claim Cancel" '00099

        ' - Vaccination Record
        Public Const VaccinationRecordClick As String = "Vaccination record button click" '00103
        Public Const VaccinationRecordCloseClick As String = "Vaccination record close button click" '00105

        ' - Mark Invalid
        Public Const MarkInvalidClick As String = "Mark Invalid Click" '00107"
        Public Const MarkInvalidCancelClick As String = "Mark Invalid Cancel Click" '00108"
        Public Const MarkInvalidSaveClick As String = "Mark Invalid Save Click" '00109"

    End Class

    Private Class ViewIndex
        Public Const InputCriteria As Integer = 0
        Public Const Transaction As Integer = 1
        Public Const BatchActionConfirm As Integer = 2
        Public Const Complete As Integer = 3
        Public Const Detail As Integer = 4
        Public Const CompleteRemark As Integer = 5
        Public Const ViewNewClaimTransaction As Integer = 6
    End Class

    Private Class ViewIndexBatchAction
        Public Const BatchInputReason As Integer = 0
        Public Const BatchConfirmReason As Integer = 1
    End Class

    Private Class ViewIndexDetailAction
        Public Const Button As Integer = 0
        Public Const InputReason As Integer = 1
        Public Const ConfirmReason As Integer = 2
        Public Const InputInvalidationReason As Integer = 3
        Public Const ConfirmInvalidationReason As Integer = 4
    End Class

    Private Class ErrorMessageBoxHeaderKey
        Public Const SearchFail As String = "SearchFail"
        Public Const ValidationFail As String = "ValidationFail"
        Public Const UpdateFail As String = "UpdateFail"
        Public Const ValidationWarning As String = "Warning"
        Public Const ConnectionFail As String = "ConnectionFail"
    End Class

    Private Class SingleAction
        Public Const Suspend As String = "S"
        Public Const Void As String = "I"
        Public Const CancelInvalidation As String = "C"
        Public Const ConfirmInvalid As String = "D"
    End Class

    Private Class InvalidationType
        Public Const Others As String = "O"
    End Class

    Private Class NewClaimTransaction
        Public Const SearchAccountResults As Integer = 0
        Public Const EnterClaimDetails As Integer = 1
        Public Const Confirm = 2
        Public Const Complete = 3
    End Class

    Private Class InputTransactionDetails
        Public Const CreationDetails = 0
        Public Const ClaimDetails = 1

    End Class

    Private Class VaccinationRecordPopupStatusClass
        Public Const Active As String = "A"
    End Class

    Private Class VaccinationRecordPopupShownClass
        Public Const Active As String = "A"
    End Class

    Private Class VS
        Public Const VaccinationRecordPopupStatus As String = "VaccinationRecordPopupStatus"
        Public Const VaccinationRecordPopupShown As String = "VaccinationRecordPopupShown"
    End Class

    Private Class TypeOfDate
        Public Const ServiceDate As String = "SD"
        Public Const TransactionDate As String = "TD"
    End Class

    Private Class SESS
        Public Const SelectedTabIndex As String = "010404_TabContainer_SelectedTabIndex"
    End Class

#End Region

#Region "Fields"

    Private udtDocTypeBLL As New DocTypeBLL
    Private udtEHSTransactionBLL As New EHSTransactionBLL
    Private udtFormatter As New Formatter
    Private udtGeneralFunction As New GeneralFunction
    Private udtHCVUUserBLL As New HCVUUserBLL
    Private udtReimbursementBLL As New ReimbursementBLL
    Private udtSchemeClaimBLL As New SchemeClaimBLL
    Private udtSearchEngineBLL As New SearchEngineBLL
    Private udtSPProfileBLL As New SPProfileBLL
    Private udtStaticDataBLL As New StaticDataBLL
    Private udtUserRoleBLL As New UserRoleBLL
    Private udtValidator As New Validator
    Private udtCommonFunction As New Common.ComFunction.GeneralFunction
    Private udtSessionHandlerBLL As New BLL.SessionHandlerBLL
    Private udtEHSClaimBLL As New BLL.EHSClaimBLL

#End Region

#Region "Session Constants"
    Private Const SESS_SchemeClaimList As String = "010404_SchemeClaimList"
    Private Const SESS_SearchCriteria As String = "010404_SearchCriteria"
    Private Const SESS_TransactionDataTable As String = "010404_TransactionDataTable"
    Private Const SESS_SelectedDataTable As String = "010404_SelectedDataTable"
    Private Const SESS_SearchAccount As String = "010404_SearchAccountDataTable"
    Private Const SESS_AdvancedSearchSP As String = "010404_AdvancedSearchSP"
    Private Const SESS_ServiceProvider As String = "010404_ServiceProvider"
    Private Const SESS_FromRedirect As String = "010404_FromRedirect"
    Private Const SESS_SearchRVPHomeList As String = "010404_SearchRVPHomeList"
    Private Const SESS_SearchSchoolList As String = "010404_SearchSchoolList"
    ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Const SESS_SchemeClaimListFilteredByUserRole As String = "010403_SchemeClaimListFilteredByUserRole"
    ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        If Not IsPostBack Then
            FunctionCode = FunctCode.FUNT010404

            ' Set the gridview page size
            Dim strParmValue As String = String.Empty
            Dim intPageSize As Integer = udtGeneralFunction.GetPageSize() ' CRE11-007

            gvTransaction.PageSize = intPageSize

            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, AuditLogDescription.Load)

            ' Set the default Transaction Date From / Transaction Date To
            'txtTransactionDateFrom.Text = udtFormatter.formatEnterDate(FormatDateTime(Now.AddMonths(-1), DateFormat.GeneralDate))
            'txtTransactionDateTo.Text = udtFormatter.formatEnterDate(Now)

            'CRE13-012 (RCH Code sorting) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            ' Bind Health Profession
            BindHealthProfession(Me.ddlTabTransactionHealthProfession)
            BindHealthProfession(Me.ddlTabAdvancedSearchHealthProfession)


            ' eHealth Account Prefix
            Dim strParm1 As String = String.Empty
            Dim strParm2 As String = String.Empty
            If udtCommonFunction.getSystemParameter("eHealthAccountPrefix", strParm1, strParm2) Then
                lblTabeHSAccountIDPrefix.Text = strParm1
                lblTabAdvancedSearchAccountIDPrefix.Text = strParm1
            Else
                Throw New ArgumentNullException("Parameter: eHealthAccountPrefix not found")
            End If

            ' Bind Transaction Status
            BindTransactionStatus(Me.ddlTabTransactionTransactionStatus)
            BindTransactionStatus(Me.ddlTabServiceProviderTransactionStatus)
            BindTransactionStatus(Me.ddlTabeHSAccountTransactionStatus)
            BindTransactionStatus(Me.ddlTabAdvancedSearchTransactionStatus)

            ' Bind Authorized Status
            BindAuthorisedStatus(Me.ddlTabTransactionAuthorizedStatus)
            BindAuthorisedStatus(Me.ddlTabServiceProviderAuthorizedStatus)
            BindAuthorisedStatus(Me.ddlTabeHSAccountAuthorizedStatus)
            BindAuthorisedStatus(Me.ddlTabAdvancedSearchAuthorizedStatus)

            ' Bind eHealth Account Identity Document Type
            BindDocumentType(Me.ddlTabeHSAccountDocType)
            BindDocumentType(Me.ddlTabAdvancedSearchDocType)

            ' Bind Scheme
            BindScheme(Me.ddlTabTransactionScheme)
            BindScheme(Me.ddlTabServiceProviderScheme)
            BindScheme(Me.ddlTabeHSAccountScheme)
            BindScheme(Me.ddlTabAdvancedSearchScheme)

            ' Bind Invalidation
            BindInvalidation(Me.ddlTabTransactionInvalidationStatus, Me.ddlTabTransactionTransactionStatus, Me.ddlTabTransactionAuthorizedStatus)
            BindInvalidation(Me.ddlTabServiceProviderInvalidationStatus, Me.ddlTabServiceProviderTransactionStatus, Me.ddlTabServiceProviderAuthorizedStatus)
            BindInvalidation(Me.ddlTabeHSAccountInvalidationStatus, Me.ddlTabeHSAccountTransactionStatus, Me.ddlTabeHSAccountAuthorizedStatus)
            BindInvalidation(Me.ddlTabAdvancedSearchInvalidationStatus, Me.ddlTabAdvancedSearchTransactionStatus, Me.ddlTabAdvancedSearchAuthorizedStatus)

            'Bind Reimbursement Method
            BindReimbursementMethod(Me.ddlTabTransactionReimbursementMethod)
            BindReimbursementMethod(Me.ddlTabServiceProviderReimbursementMethod)
            BindReimbursementMethod(Me.ddlTabeHSAccountReimbursementMethod)
            BindReimbursementMethod(Me.ddlTabAdvancedSearchReimbursementMethod)

            ' Bind Means of Input
            BindMeansOfInput(Me.ddlTabTransactionMeansOfInput, Me.lblTabTransactionMeansOfInputText)
            BindMeansOfInput(Me.ddlTabServiceProviderMeansOfInput, Me.lblTabServiceProviderMeansOfInputText)
            BindMeansOfInput(Me.ddlTabeHSAccountMeansOfInput, Me.lblTabeHSAccountMeansOfInputText)
            BindMeansOfInput(Me.ddlTabAdvancedSearchMeansOfInput, Me.lblTabAdvancedSearchMeansOfInputText)

            'CRE20-003 (add search criteria) [Start][Martin]
            'Bind Dose
            BindDose(Me.ddlTabTransactionDose)
            'Bind Vaccine
            BindVaccine(Me.ddlTabTransactionVaccines)
            'CRE20-003 (add search criteria) [End][Martin]

            Me.MultiViewReimClaimTransManagement.ActiveViewIndex = ViewIndex.InputCriteria
            Me.TabContainerCTM.ActiveTabIndex = Aspect.Transaction
            Session(SESS.SelectedTabIndex) = Aspect.Transaction
            'CRE13-012 (RCH Code sorting) [End][Chris YIM]

            ' Turn On / Off the making new claim function
            Dim strTurnOnNewClaim As String = String.Empty
            udtGeneralFunction.getSystemParameter("TurnOnOutsidePaymentClaim", strTurnOnNewClaim, String.Empty)

            ' CRE17-018-07 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
            ' --------------------------------------------------------------------------------------
            'If strTurnOnNewClaim.Trim.Equals("Y") Then
            '    Me.ibtnNewClaimTransaction.Visible = True
            'Else
            '    Me.ibtnNewClaimTransaction.Visible = False
            'End If
            Me.ibtnNewClaimTransaction.Visible = False
            ' CRE17-018-07 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

            ' Prevent double-click 
            Dim strBrowser As String = String.Empty
            Try
                If Not HttpContext.Current.Request.Browser Is Nothing Then
                    strBrowser = HttpContext.Current.Request.Browser.Type
                End If
            Catch ex As Exception
                strBrowser = String.Empty
            End Try

            MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnConfirmClaimCreationConfirm)
            MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnWarningMessageConfirm)
            MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnWarningMessageCancel)

            ' Clear the eVaccination Record
            Dim udtSession As New BLL.SessionHandlerBLL()
            udtSession.CMSVaccineResultRemoveFromSession(FunctionCode)
            ViewState.Remove(VS.VaccinationRecordPopupStatus)
            ViewState.Remove(VS.VaccinationRecordPopupShown)

            HandleRedirectAction()

        Else
            If MultiViewReimClaimTransManagement.ActiveViewIndex = ViewIndex.Detail Then
                ' Rebind the details
                BuildDetail(hfCurrentDetailTransactionNo.Value)
            End If

            If MultiViewReimClaimTransManagement.ActiveViewIndex = ViewIndex.ViewNewClaimTransaction Then


                If Me.mvNewClaimTransaction.ActiveViewIndex = NewClaimTransaction.EnterClaimDetails Then
                    Dim udteHSAccountMaintBLL As New eHSAccountMaintBLL
                    Dim udtEHSAccount As EHSAccount.EHSAccountModel

                    udtEHSAccount = udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)

                    If Not IsNothing(udtEHSAccount) Then
                        BindPersonalInfo(udtEHSAccount)
                    End If

                    If Not IsNothing(ViewState(VS.VaccinationRecordPopupStatus)) _
                            AndAlso ViewState(VS.VaccinationRecordPopupStatus) = VaccinationRecordPopupStatusClass.Active Then
                        popupVaccinationRecord.Show()
                        ucVaccinationRecord.BuildEHSAccount(udtEHSAccount)
                    End If

                    If mvEnterDetails.ActiveViewIndex = InputTransactionDetails.ClaimDetails Then
                        SetUpEnterClaimDetails(True)
                    End If
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
                ElseIf Me.mvNewClaimTransaction.ActiveViewIndex = NewClaimTransaction.Confirm Then
                    Dim udtEHSTransaction As EHSTransactionModel
                    udtEHSTransaction = Me.udtSessionHandlerBLL.EHSTransactionGetFromSession(FunctionCode)
                    Me.udcConfirmClaimCreation.EnableVaccinationRecordChecking = False
                    Me.udcConfirmClaimCreation.LoadTranInfo(udtEHSTransaction, New DataTable())

                End If
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
            End If

        End If

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender, TabContainerCTM.PreRender
        Select Case MultiViewReimClaimTransManagement.ActiveViewIndex
            Case ViewIndex.InputCriteria
                'CRE17-008 (Remind Delist Practice) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                If Not IsPopupShow() Then
                    Select Case TabContainerCTM.ActiveTabIndex
                        Case Aspect.Transaction
                            ScriptManager1.SetFocus(txtTabTransactionTransactionNo)
                            panTransaction.DefaultButton = ibtnTabTransactionSearch.ID
                        Case Aspect.ServiceProvider
                            ScriptManager1.SetFocus(txtTabServiceProviderSPID)
                            panServiceProvider.DefaultButton = ibtnTabServiceProviderSearch.ID
                        Case Aspect.eHSAccount
                            ScriptManager1.SetFocus(txtTabeHSAccountDocNo)
                            panEHSAccount.DefaultButton = ibtnTabeHSAccountSearch.ID
                        Case Aspect.AdvancedSearch
                            ScriptManager1.SetFocus(txtTabAdvancedSearchTransactionNo)
                            panAdvancedSearch.DefaultButton = ibtnTabAdvancedSearchSearch.ID
                    End Select
                End If
                'CRE17-008 (Remind Delist Practice) [End][Chris YIM]

            Case ViewIndex.Detail
                ScriptManager1.SetFocus(btnFocus)

                ibtnPreviousRecord.Enabled = lblCurrentRecordNo.Text <> "1"
                ibtnNextRecord.Enabled = lblCurrentRecordNo.Text <> lblMaxRecordNo.Text
            Case ViewIndex.ViewNewClaimTransaction
                Select Case Me.mvEnterDetails.ActiveViewIndex
                    Case InputTransactionDetails.ClaimDetails
                        ScriptManager1.SetFocus(ibtnEnterClaimDetailBack)

                    Case InputTransactionDetails.CreationDetails

                        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                        ' ----------------------------------------------------------
                        'Dim udtEHSTransaction As EHSTransactionModel = GetEHSTransaction()
                        Dim udtEHSTransaction As EHSTransactionModel = Me.udtSessionHandlerBLL.EHSTransactionWithoutTransactionDetailGetFromSession(FunctionCode)

                        If Not udtEHSTransaction Is Nothing Then
                            Me.udtSessionHandlerBLL.EHSTransactionRemoveFromSession(FunctionCode)
                            Me.udtSessionHandlerBLL.EHSTransactionWithoutTransactionDetailRemoveFromSession(FunctionCode)

                            Dim i As Integer
                            For i = 0 To ddlEnterCreationDetailPractice.Items.Count - 1
                                If ddlEnterCreationDetailPractice.Items(i).Value.ToString().Trim() = udtEHSTransaction.PracticeID.ToString().Trim() Then
                                    ddlEnterCreationDetailPractice.SelectedIndex = i
                                End If
                            Next
                        End If
                        ' CRE17-010 (OCSSS integration) [End][Chris YIM]
                End Select


                'Dim udtSession As New BLL.SessionHandlerBLL()
                'Dim udtHAResult As Common.WebService.Interface.HAVaccineResult = udtSession.CMSVaccineResultGetFromSession(FunctionCode)
                'If Not udtHAResult Is Nothing AndAlso Not udtHAResult.Exception Is Nothing Then
                '    udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00254)
                '    'udcMessageBox.BuildMessageBox("Connection Fail")
                '    udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ConnectionFail, udtAuditLogEntry, LogID.LOG00100, AuditLogDescription.HAConnectionFail)
                '    udcMessageBox.Visible = True
                'End If

            Case Else
                ScriptManager1.SetFocus(btnHidden)

        End Select
        ' udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00254)
        ' udcMessageBox.BuildMessageBox("Connection Fail")
        ' udcMessageBox.Visible = True
    End Sub

#End Region

#Region "Support Function"
    'CRE13-012 (RCH Code sorting) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub BindHealthProfession(ByVal ddlHealthProfession As DropDownList)
        ddlHealthProfession.DataSource = udtSPProfileBLL.GetHealthProf
        ddlHealthProfession.DataValueField = "ServiceCategoryCode"
        ddlHealthProfession.DataTextField = "ServiceCategoryDesc"
        ddlHealthProfession.DataBind()

        ddlHealthProfession.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))

        ddlHealthProfession.SelectedIndex = 0
    End Sub
    'CRE13-012 (RCH Code sorting) [End][Chris YIM]

    'CRE13-012 (RCH Code sorting) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub BindTransactionStatus(ByVal ddlTransactionStatus As DropDownList)
        ddlTransactionStatus.DataSource = Status.GetDescriptionListFromDBEnumCode("HCVUClaimTransManagementStatus")
        ddlTransactionStatus.DataValueField = "Status_Value"
        ddlTransactionStatus.DataTextField = "Status_Description"
        ddlTransactionStatus.DataBind()

        ddlTransactionStatus.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))

        ddlTransactionStatus.SelectedIndex = 0

    End Sub
    'CRE13-012 (RCH Code sorting) [End][Chris YIM]

    'CRE13-012 (RCH Code sorting) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub BindAuthorisedStatus(ByVal ddlAuthorizedStatus As DropDownList)
        ddlAuthorizedStatus.DataSource = Status.GetDescriptionListFromDBEnumCode("AuthorizedDisplayStatus")
        ddlAuthorizedStatus.DataValueField = "Status_Value"
        ddlAuthorizedStatus.DataTextField = "Status_Description"
        ddlAuthorizedStatus.DataBind()

        ddlAuthorizedStatus.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))

        ddlAuthorizedStatus.SelectedIndex = 0

    End Sub
    'CRE13-012 (RCH Code sorting) [End][Chris YIM]

    'CRE13-012 (RCH Code sorting) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub BindInvalidation(ByVal ddlInvalidationStatus As DropDownList, ByVal ddlTransactionStatus As DropDownList, ByVal ddlAuthorizedStatus As DropDownList)
        ddlInvalidationStatus.DataSource = Status.GetDescriptionListFromDBEnumCode(TransactionInvalidationModel.TransactionInvalidationStatusClass.ClassCode)
        ddlInvalidationStatus.DataValueField = "Status_Value"
        ddlInvalidationStatus.DataTextField = "Status_Description"
        ddlInvalidationStatus.DataBind()

        ddlInvalidationStatus.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))

        SetddlInvalidationStatusEnable(ddlInvalidationStatus, ddlTransactionStatus, ddlAuthorizedStatus)

    End Sub
    'CRE13-012 (RCH Code sorting) [End][Chris YIM]

    'CRE13-012 (RCH Code sorting) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub BindDocumentType(ByVal ddlEHealthDocType As DropDownList)
        ddlEHealthDocType.DataSource = udtDocTypeBLL.getAllDocType()
        ddlEHealthDocType.DataTextField = "DocName"
        ddlEHealthDocType.DataValueField = "DocCode"
        ddlEHealthDocType.DataBind()

        ddlEHealthDocType.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))

        ddlEHealthDocType.SelectedIndex = 0

    End Sub
    'CRE13-012 (RCH Code sorting) [End][Chris YIM]

    'CRE20-003 (add search criteria) [Start][Martin]
    '-----------------------------------------------------------------------------------------
    Private Sub BindScheme(ByVal ddlScheme As DropDownList)
        Dim udtSchemeClaimModelListFilter As New SchemeClaimModelCollection
        Dim udtUserRoleCollection As UserRoleModelCollection = udtUserRoleBLL.GetUserRoleCollection(udtHCVUUserBLL.GetHCVUUser.UserID)

        Dim udtSchemeCList As SchemeClaimModelCollection = udtSchemeClaimBLL.getAllDistinctSchemeClaim()
        Session(SESS_SchemeClaimList) = udtSchemeCList

        For Each udtSchemeC As SchemeClaimModel In udtSchemeCList
            For Each udtUserRoleModel As UserRoleModel In udtUserRoleCollection.Values
                If udtUserRoleModel.SchemeCode.Trim = udtSchemeC.SchemeCode Then
                    If Not udtSchemeClaimModelListFilter.Contains(udtSchemeC) Then udtSchemeClaimModelListFilter.Add(udtSchemeC)
                End If
            Next
        Next

        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Session(SESS_SchemeClaimListFilteredByUserRole) = udtSchemeClaimModelListFilter
        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

        ddlScheme.DataSource = udtSchemeClaimModelListFilter
        ddlScheme.DataValueField = "SchemeCode"
        ddlScheme.DataTextField = "DisplayCode"
        ddlScheme.DataBind()

        ' Set the scheme list to disabled if only 1 scheme
        If udtSchemeClaimModelListFilter.Count = 1 Then
            ddlScheme.SelectedIndex = 1
            ddlScheme.Enabled = False

            If ddlScheme.SelectedValue.Trim = SchemeClaimModel.RVP Or _
                ddlScheme.SelectedValue.Trim = SchemeClaimModel.PPP Or _
                ddlScheme.SelectedValue.Trim = SchemeClaimModel.PPPKG Then

                Me.txtTabTransactionRCHRode.Enabled = True
                Me.txtTabServiceProviderRCHRode.Enabled = True
                Me.txtTabeHSAccountRCHRode.Enabled = True
                Me.txtTabAdvancedSearchRCHRode.Enabled = True
            Else
                Me.txtTabTransactionRCHRode.Enabled = False
                Me.txtTabTransactionRCHRode.Text = String.Empty
                Me.txtTabServiceProviderRCHRode.Enabled = False
                Me.txtTabServiceProviderRCHRode.Text = String.Empty
                Me.txtTabeHSAccountRCHRode.Enabled = False
                Me.txtTabeHSAccountRCHRode.Text = String.Empty
                Me.txtTabAdvancedSearchRCHRode.Enabled = False
                Me.txtTabAdvancedSearchRCHRode.Text = String.Empty
            End If
        Else
            ddlScheme.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))
            ddlScheme.SelectedIndex = 0
            ddlScheme.Enabled = True

            Dim HasRVPorPPP As Boolean = False

            For idxItem As Integer = 0 To ddlScheme.Items.Count - 1
                Dim strScheme As String = ddlScheme.Items(idxItem).Value

                If strScheme = SchemeClaimModel.RVP Or _
                    strScheme = SchemeClaimModel.PPP Or _
                    strScheme = SchemeClaimModel.PPPKG Then

                    HasRVPorPPP = True
                End If
            Next

            If HasRVPorPPP Then
                Me.txtTabTransactionRCHRode.Enabled = True
                Me.txtTabServiceProviderRCHRode.Enabled = True
                Me.txtTabeHSAccountRCHRode.Enabled = True
                Me.txtTabAdvancedSearchRCHRode.Enabled = True
            Else
                Me.txtTabTransactionRCHRode.Enabled = False
                Me.txtTabTransactionRCHRode.Text = String.Empty
                Me.txtTabServiceProviderRCHRode.Enabled = False
                Me.txtTabServiceProviderRCHRode.Text = String.Empty
                Me.txtTabeHSAccountRCHRode.Enabled = False
                Me.txtTabeHSAccountRCHRode.Text = String.Empty
                Me.txtTabAdvancedSearchRCHRode.Enabled = False
                Me.txtTabAdvancedSearchRCHRode.Text = String.Empty
            End If
        End If

    End Sub
    'CRE20-003 (add search criteria) [End][Martin]

    'CRE13-012 (RCH Code sorting) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub BindReimbursementMethod(ByVal ddlReimbursementMethod As DropDownList)
        Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL

        ddlReimbursementMethod.DataSource = udtStaticDataBLL.GetStaticDataListByColumnName("ReimbursementMethod")
        ddlReimbursementMethod.DataValueField = "ItemNo"
        ddlReimbursementMethod.DataTextField = "DataValue"
        ddlReimbursementMethod.DataBind()

        ddlReimbursementMethod.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))

        ddlReimbursementMethod.SelectedIndex = 0
    End Sub
    'CRE13-012 (RCH Code sorting) [End][Chris YIM]

    'CRE13-012 (RCH Code sorting) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub BindMeansOfInput(ByVal ddlMeansOfInput As DropDownList, ByVal lblMeansOfInputText As Label)
        ddlMeansOfInput.DataSource = (New StaticDataBLL).GetStaticDataListByColumnName("MeansOfInput")
        ddlMeansOfInput.DataValueField = "ItemNo"
        ddlMeansOfInput.DataTextField = "DataValue"
        ddlMeansOfInput.DataBind()

        ddlMeansOfInput.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))

        ddlMeansOfInput.SelectedIndex = 0

        If (New GeneralFunction).CheckTurnOnMeansOfInput = GeneralFunction.EnumTurnOnStatus.Yes Then
            ddlMeansOfInput.Visible = True
            lblMeansOfInputText.Visible = True

        Else
            ddlMeansOfInput.Visible = False
            lblMeansOfInputText.Visible = False

        End If

    End Sub
    'CRE13-012 (RCH Code sorting) [End][Chris YIM]

    Private Sub BindCreationReason()
        Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL

        Me.ddlEnterCreationDetailCreationReason.DataSource = udtStaticDataBLL.GetStaticDataListByColumnName("ClaimCreationReason")
        ddlEnterCreationDetailCreationReason.DataValueField = "ItemNo"
        ddlEnterCreationDetailCreationReason.DataTextField = "DataValue"
        ddlEnterCreationDetailCreationReason.DataBind()

        ddlEnterCreationDetailCreationReason.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), ""))

        ddlEnterCreationDetailCreationReason.SelectedIndex = 0
    End Sub

    Private Sub BindPaymentMethod()
        Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL
        Me.ddlEnterCreationDetailPaymentMethod.DataSource = udtStaticDataBLL.GetStaticDataListByColumnName("ReimbursementPaymentMethod")
        ddlEnterCreationDetailPaymentMethod.DataValueField = "ItemNo"
        ddlEnterCreationDetailPaymentMethod.DataTextField = "DataValue"
        ddlEnterCreationDetailPaymentMethod.DataBind()

        ddlEnterCreationDetailPaymentMethod.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), ""))

        ddlEnterCreationDetailPaymentMethod.SelectedIndex = 0
    End Sub

    'CRE20-003 (add search criteria) [Start][Martin]
    '-----------------------------------------------------------------------------------------
    Private Sub BindVaccine(ByVal ddlReimbursementMethod As DropDownList)
        Dim udtSubsidizeBLL As New SubsidizeBLL

        Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL

        ddlReimbursementMethod.DataSource = udtSubsidizeBLL.GetSubsidizeItemBySubsidizeType("VACCINE")
        ddlReimbursementMethod.DataValueField = "Subsidize_Item_Code"
        ddlReimbursementMethod.DataTextField = "Subsidize_item_Display_Code"
        ddlReimbursementMethod.DataBind()

        ddlReimbursementMethod.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))

        ddlReimbursementMethod.SelectedIndex = 0
    End Sub

    Private Sub BindDose(ByVal ddlDose As DropDownList)
        Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL

        ddlDose.DataSource = udtStaticDataBLL.GetStaticDataListByColumnName("Dose")
        ddlDose.DataValueField = "ItemNo"
        ddlDose.DataTextField = "DataValue"
        ddlDose.DataBind()

        ddlDose.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))

        ddlDose.SelectedIndex = 0
    End Sub
    'CRE20-003 (add search criteria) [End][Martin]


    Private Sub HandleRedirectAction()
        Dim udtRedirectParameterBLL As New RedirectParameterBLL
        Dim udtRedirectParameter As RedirectParameterModel = udtRedirectParameterBLL.GetFromSession()
        If IsNothing(udtRedirectParameter) Then Return

        udtRedirectParameterBLL.RemoveFromSession()
        udtRedirectParameterBLL.WriteAuditLog(FunctionCode, Me, udtRedirectParameter)

        If udtRedirectParameter.ActionList.Contains(RedirectParameterModel.EnumRedirectAction.Search) Then
            ' --- Auto-perform Search action ---

            If udtRedirectParameter.TransactionNo <> String.Empty Then
                'CRE13-012 (RCH Code sorting) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                txtTabTransactionTransactionNo.Text = (New Formatter).formatSystemNumber(udtRedirectParameter.TransactionNo)

                ddlTabTransactionTransactionStatus.SelectedIndex = 0
                'CRE13-012 (RCH Code sorting) [End][Chris YIM]

                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
                ' -------------------------------------------------------------------------
                Session(SESS_FromRedirect) = "Y"
                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
                ibtnSearch_Click(Nothing, Nothing)

            End If

        End If

        If udtRedirectParameter.ActionList.Contains(RedirectParameterModel.EnumRedirectAction.ViewDetail) Then
            ' --- Auto-perform Row Command (click) action ---

            ' Locate the link button for the row command action
            If gvTransaction.Rows.Count <> 1 Then Throw New Exception(String.Format("claimTransManagement.HandleRedirectAction: Unexpected no. of rows {0}", gvTransaction.Rows.Count))

            Dim lbtnTransNum As LinkButton = gvTransaction.Rows(0).FindControl("lbtn_transNum")

            Dim arg As New CommandEventArgs(String.Empty, Nothing)
            Dim e As New GridViewCommandEventArgs(gvTransaction.Rows(0), lbtnTransNum, arg)

            gvTransaction_RowCommand(gvTransaction, e)

            ibtnDetailBack.Visible = False

        End If

    End Sub

    'CRE13-012 (RCH Code sorting) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub BuildRecordSummary(ByVal dt As DataTable)
        Dim dblPendingConfirm As Double = 0
        Dim dblPendingValidation As Double = 0
        Dim dblReady As Double = 0
        Dim dblReimbursed As Double = 0
        Dim dblSuspended As Double = 0
        Dim dblVoided As Double = 0
        Dim dblPendingApprovalBO As Double = 0
        Dim dblRemovedBO As Double = 0
        Dim dblIncomplete As Double = 0

        For Each dr As DataRow In dt.Rows
            If IsNumeric(dr("totalAmount")) = True Then
                Select Case CStr(dr("transStatus")).Trim
                    Case ClaimTransStatus.Pending
                        dblPendingConfirm += CDbl(dr("totalAmount"))

                    Case ClaimTransStatus.PendingVRValidate
                        dblPendingValidation += CDbl(dr("totalAmount"))

                    Case ClaimTransStatus.Active
                        dblReady += CDbl(dr("totalAmount"))

                    Case ClaimTransStatus.Reimbursed, ClaimTransStatus.ManualReimbursedClaim
                        dblReimbursed += CDbl(dr("totalAmount"))

                    Case ClaimTransStatus.Suspended
                        dblSuspended += CDbl(dr("totalAmount"))

                    Case ClaimTransStatus.Inactive, ClaimTransStatus.RejectedBySP
                        dblVoided += CDbl(dr("totalAmount"))

                    Case ClaimTransStatus.PendingApprovalForNonReimbursedClaim
                        dblPendingApprovalBO += CDbl(dr("totalAmount"))

                    Case ClaimTransStatus.Removed
                        dblRemovedBO += CDbl(dr("totalAmount"))

                    Case ClaimTransStatus.Incomplete
                        dblIncomplete += CDbl(dr("totalAmount"))

                End Select
            End If

        Next

        'Total Amount Claimed ($)
        lblSummaryPendingComfirm.Text = udtFormatter.formatMoney(dblPendingConfirm.ToString, False)
        lblSummaryPendingEHSAcctValidation.Text = udtFormatter.formatMoney(dblPendingValidation.ToString, False)
        lblSummaryReadyToReimburse.Text = udtFormatter.formatMoney(dblReady.ToString, False)
        lblSummaryReimbursed.Text = udtFormatter.formatMoney(dblReimbursed.ToString, False)
        lblSummarySuspended.Text = udtFormatter.formatMoney(dblSuspended.ToString, False)
        lblSummaryVoided.Text = udtFormatter.formatMoney(dblVoided.ToString, False)
        lblSummaryPendingApprovalBO.Text = udtFormatter.formatMoney(dblPendingApprovalBO.ToString, False)
        lblSummaryRemovedBO.Text = udtFormatter.formatMoney(dblRemovedBO.ToString, False)
        lblSummaryIncomplete.Text = udtFormatter.formatMoney(dblIncomplete.ToString, False)

    End Sub
    'CRE13-012 (RCH Code sorting) [End][Chris YIM]

    Private Function SearchAspectRedirection() As Aspect
        Dim blnInputedCommonField As Integer = False
        Dim blnInputedTxField As Integer = False
        Dim blnInputedSPField As Integer = False
        Dim blnInputedEHSAcctField As Integer = False

        Dim EnumSelectedStoredProc As Aspect = Aspect.AdvancedSearch

        'Group: Transaction
        blnInputedTxField = txtTabAdvancedSearchTransactionNo.Text.Trim <> String.Empty OrElse _
                            ddlTabAdvancedSearchHealthProfession.SelectedValue <> String.Empty

        'Group: Common
        blnInputedCommonField = txtTabAdvancedSearchDateFrom.Text.Trim <> String.Empty OrElse _
                                txtTabAdvancedSearchDateTo.Text.Trim <> String.Empty OrElse _
                                ddlTabAdvancedSearchScheme.SelectedValue <> String.Empty OrElse _
                                ddlTabAdvancedSearchTransactionStatus.SelectedValue <> String.Empty OrElse _
                                ddlTabAdvancedSearchAuthorizedStatus.SelectedValue <> String.Empty OrElse _
                                ddlTabAdvancedSearchInvalidationStatus.SelectedValue <> String.Empty OrElse _
                                ddlTabAdvancedSearchReimbursementMethod.SelectedValue <> String.Empty OrElse _
                                ddlTabAdvancedSearchMeansOfInput.SelectedValue <> String.Empty OrElse _
                                txtTabAdvancedSearchRCHRode.Text.Trim <> String.Empty

        'Group: SP
        blnInputedSPField = txtTabAdvancedSearchSPID.Text.Trim <> String.Empty OrElse _
                            txtTabAdvancedSearchSPHKID.Text.Trim <> String.Empty OrElse _
                            txtTabAdvancedSearchSPName.Text.Trim <> String.Empty OrElse _
                            txtTabAdvancedSearchBankAccountNo.Text.Trim <> String.Empty

        'Group: EHS Account
        blnInputedEHSAcctField = ddlTabAdvancedSearchDocType.SelectedValue <> String.Empty OrElse _
                                    txtTabAdvancedSearchDocNo.Text.Trim <> String.Empty OrElse _
                                    txtTabAdvancedSearchAccountID.Text.Trim <> String.Empty

        'Determine which aspect to search transaction
        Dim intCaseID As Integer = 0
        If blnInputedCommonField Then
            intCaseID = intCaseID + 1000
        End If

        If blnInputedTxField Then
            intCaseID = intCaseID + 100
        End If

        If blnInputedSPField Then
            intCaseID = intCaseID + 10
        End If

        If blnInputedEHSAcctField Then
            intCaseID = intCaseID + 1
        End If

        Select Case intCaseID
            Case 1000       'Only inputed the "Common" field
                EnumSelectedStoredProc = Aspect.Transaction

            Case 100, 1100  'Only inputed the "Transaction" field / Both inputed the "Transaction" field and "Common" field
                EnumSelectedStoredProc = Aspect.Transaction

            Case 10, 1010   'Only inputed the "Service Provider" field / Both inputed the "Service Provider" field and "Common" field
                EnumSelectedStoredProc = Aspect.ServiceProvider

            Case 1, 1001    'Only inputed the "EHS Account" field / Both inputed the "EHS Account" field and "Common" field
                EnumSelectedStoredProc = Aspect.eHSAccount

            Case Else       'Default
                EnumSelectedStoredProc = Aspect.AdvancedSearch
        End Select

        Return EnumSelectedStoredProc

    End Function
#End Region

#Region "Event Handler"
    'CRE13-012 (RCH Code sorting) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Protected Sub ddlScheme_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddlScheme As DropDownList = CType(sender, DropDownList)

        'CRE20-003 (add search criteria) [Martin]
        If (ddlScheme.Items.Contains(New ListItem(SchemeClaimModel.RVP, SchemeClaimModel.RVP)) Or _
            ddlScheme.Items.Contains(New ListItem(SchemeClaimModel.PPP, SchemeClaimModel.PPP)) Or _
            ddlScheme.Items.Contains(New ListItem(SchemeClaimModel.PPPKG, SchemeClaimModel.PPPKG))) _
            AndAlso (ddlScheme.SelectedValue.Trim = String.Empty Or _
                     ddlScheme.SelectedValue.Trim = SchemeClaimModel.RVP Or _
                     ddlScheme.SelectedValue.Trim = SchemeClaimModel.PPP Or _
                     ddlScheme.SelectedValue.Trim = SchemeClaimModel.PPPKG) Then

            Select Case Session(SESS.SelectedTabIndex)
                Case Aspect.Transaction
                    Me.txtTabTransactionRCHRode.Enabled = True
                Case Aspect.ServiceProvider
                    Me.txtTabServiceProviderRCHRode.Enabled = True
                Case Aspect.eHSAccount
                    Me.txtTabeHSAccountRCHRode.Enabled = True
                Case Aspect.AdvancedSearch
                    Me.txtTabAdvancedSearchRCHRode.Enabled = True
            End Select
        Else
            Select Case Session(SESS.SelectedTabIndex)
                Case Aspect.Transaction
                    Me.txtTabTransactionRCHRode.Enabled = False
                    Me.txtTabTransactionRCHRode.Text = String.Empty
                Case Aspect.ServiceProvider
                    Me.txtTabServiceProviderRCHRode.Enabled = False
                    Me.txtTabServiceProviderRCHRode.Text = String.Empty
                Case Aspect.eHSAccount
                    Me.txtTabeHSAccountRCHRode.Enabled = False
                    Me.txtTabeHSAccountRCHRode.Text = String.Empty
                Case Aspect.AdvancedSearch
                    Me.txtTabAdvancedSearchRCHRode.Enabled = False
                    Me.txtTabAdvancedSearchRCHRode.Text = String.Empty
            End Select
        End If
    End Sub
    'CRE13-012 (RCH Code sorting) [End][Chris YIM]

    'CRE13-012 (RCH Code sorting) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Protected Sub ddlTransactionStatus_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Select Case Session(SESS.SelectedTabIndex)
            Case Aspect.Transaction
                SetddlInvalidationStatusEnable(Me.ddlTabTransactionInvalidationStatus, Me.ddlTabTransactionTransactionStatus, Me.ddlTabTransactionAuthorizedStatus)
            Case Aspect.ServiceProvider
                SetddlInvalidationStatusEnable(Me.ddlTabServiceProviderInvalidationStatus, Me.ddlTabServiceProviderTransactionStatus, Me.ddlTabServiceProviderAuthorizedStatus)
            Case Aspect.eHSAccount
                SetddlInvalidationStatusEnable(Me.ddlTabeHSAccountInvalidationStatus, Me.ddlTabeHSAccountTransactionStatus, Me.ddlTabeHSAccountAuthorizedStatus)
            Case Aspect.AdvancedSearch
                SetddlInvalidationStatusEnable(Me.ddlTabAdvancedSearchInvalidationStatus, Me.ddlTabAdvancedSearchTransactionStatus, Me.ddlTabAdvancedSearchAuthorizedStatus)
        End Select
    End Sub
    'CRE13-012 (RCH Code sorting) [End][Chris YIM]

    'CRE13-012 (RCH Code sorting) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Protected Sub ddlAuthorizedStatus_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Select Case Session(SESS.SelectedTabIndex)
            Case Aspect.Transaction
                SetddlInvalidationStatusEnable(Me.ddlTabTransactionInvalidationStatus, Me.ddlTabTransactionTransactionStatus, Me.ddlTabTransactionAuthorizedStatus)
            Case Aspect.ServiceProvider
                SetddlInvalidationStatusEnable(Me.ddlTabServiceProviderInvalidationStatus, Me.ddlTabServiceProviderTransactionStatus, Me.ddlTabServiceProviderAuthorizedStatus)
            Case Aspect.eHSAccount
                SetddlInvalidationStatusEnable(Me.ddlTabeHSAccountInvalidationStatus, Me.ddlTabeHSAccountTransactionStatus, Me.ddlTabeHSAccountAuthorizedStatus)
            Case Aspect.AdvancedSearch
                SetddlInvalidationStatusEnable(Me.ddlTabAdvancedSearchInvalidationStatus, Me.ddlTabAdvancedSearchTransactionStatus, Me.ddlTabAdvancedSearchAuthorizedStatus)
        End Select

    End Sub
    'CRE13-012 (RCH Code sorting) [End][Chris YIM]

    'CRE13-012 (RCH Code sorting) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub SetddlInvalidationStatusEnable(ByVal ddlInvalidationStatus As DropDownList, ByVal ddlTransactionStatus As DropDownList, ByVal ddlAuthorizedStatus As DropDownList)
        Dim blnEnable As Boolean = False
        Dim strTransactionStatus = ddlTransactionStatus.SelectedValue.Trim
        Dim strAuthorizedStatus = ddlAuthorizedStatus.SelectedValue.Trim

        If strTransactionStatus = String.Empty AndAlso strAuthorizedStatus = String.Empty Then blnEnable = True

        If strTransactionStatus = ClaimTransStatus.Reimbursed Then
            If strAuthorizedStatus = String.Empty OrElse strAuthorizedStatus = AuthorizedDisplayStatus.PaymentFileSubmitted Then blnEnable = True
        End If

        If strAuthorizedStatus = AuthorizedDisplayStatus.PaymentFileSubmitted Then
            If strTransactionStatus = String.Empty OrElse strTransactionStatus = ClaimTransStatus.Reimbursed Then blnEnable = True
        End If

        ddlInvalidationStatus.Enabled = blnEnable

        If blnEnable = False Then ddlInvalidationStatus.SelectedIndex = 0

    End Sub
    'CRE13-012 (RCH Code sorting) [End][Chris YIM]

    'CRE13-012 (RCH Code sorting) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Protected Sub TabContainerCTM_ActiveTabChanged(sender As Object, e As EventArgs)
        Dim udtTabContainerCTM As TabContainer = CType(sender, TabContainer)
        Session(SESS.SelectedTabIndex) = udtTabContainerCTM.ActiveTabIndex

    End Sub
    'CRE13-012 (RCH Code sorting) [End][Chris YIM]
#End Region

    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
#Region "Abstract Method of [HCVU.BasePageWithControl]"
    Protected Overrides Sub SF_InitSearch(ByRef udtAuditLogEntry As AuditLogEntry)
    End Sub

    Protected Overrides Function SF_ValidateSearch(ByRef udtAuditLogEntry As AuditLogEntry) As Boolean
        ' Data validation
        Dim blnValidDate As Boolean = True
        Dim udtSystemMessage As SystemMessage

        ' If any text fields are inputted, bypass the Transaction Date From/To empty checking
        'CRE13-012 (RCH Code sorting) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim blnTextFieldInputted As Boolean = False
        Dim txtDateFrom As TextBox = Nothing
        Dim txtDateTo As TextBox = Nothing
        Dim txtAccID As TextBox = Nothing
        Dim imgDateErr As Image = Nothing
        Dim imgAccIDErr As Image = Nothing
        Dim lblDateText As Label = Nothing

        'Invisible all alert
        imgTabTransactionTransactionNoErr.Visible = False
        imgTabTransactionDateErr.Visible = False

        ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
        imgTabServiceProviderSPIDErr.Visible = False
        imgTabServiceProviderSPHKIDErr.Visible = False
        imgTabServiceProviderSPNameErr.Visible = False
        imgTabServiceProviderSPChiNameErr.Visible = False
        imgTabServiceProviderBankAccountNoErr.Visible = False
        imgTabServiceProviderDateErr.Visible = False

        imgTabeHSAccountDocNo.Visible = False
        imgTabeHSAccountIDErr.Visible = False
        imgTabeHSAccountDateErr.Visible = False
        imgTabeHSAccountNameErr.Visible = False
        imgTabeHSAccountChiNameErr.Visible = False
        ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]

        imgTabAdvancedSearchTransactionNoErr.Visible = False
        imgTabAdvancedSearchSPIDErr.Visible = False
        imgTabAdvancedSearchSPHKIDErr.Visible = False
        imgTabAdvancedSearchSPNameErr.Visible = False
        imgTabAdvancedSearchBankAccountNoErr.Visible = False
        imgTabAdvancedSearchDocNo.Visible = False
        imgTabAdvancedSearchAccountIDErr.Visible = False
        imgTabAdvancedSearchDateErr.Visible = False

        'Start validation
        Select Case Session(SESS.SelectedTabIndex)
            Case Aspect.Transaction
                blnTextFieldInputted = Me.txtTabTransactionTransactionNo.Text.Trim <> String.Empty OrElse _
                                        Me.txtTabTransactionDateFrom.Text.Trim <> String.Empty OrElse _
                                        Me.txtTabTransactionDateTo.Text.Trim <> String.Empty

                If Not blnTextFieldInputted Then
                    udtSM = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00257)
                    Me.udcMessageBox.AddMessage(udtSM)
                    imgTabTransactionTransactionNoErr.Visible = True
                    imgTabTransactionDateErr.Visible = True

                    udtAuditLogEntry.AddDescripton("Transaction No", Me.txtTabTransactionTransactionNo.Text.Trim)
                    udtAuditLogEntry.AddDescripton("Date From", Me.txtTabTransactionDateFrom.Text.Trim)
                    udtAuditLogEntry.AddDescripton("Date To", Me.txtTabTransactionDateTo.Text.Trim)
                End If

                txtDateFrom = Me.txtTabTransactionDateFrom
                txtDateTo = Me.txtTabTransactionDateTo
                imgDateErr = Me.imgTabTransactionDateErr
                lblDateText = Me.lblTabTransactionDateText

                ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
            Case Aspect.ServiceProvider
                blnTextFieldInputted = Me.txtTabServiceProviderSPID.Text.Trim <> String.Empty _
                                                        OrElse Me.txtTabServiceProviderSPHKID.Text.Trim <> String.Empty _
                                                        OrElse Me.txtTabServiceProviderSPName.Text.Trim <> String.Empty _
                                                        OrElse Me.txtTabServiceProviderSPChiName.Text.Trim <> String.Empty _
                                                        OrElse Me.txtTabServiceProviderBankAccountNo.Text.Trim <> String.Empty _
                                                        OrElse Me.txtTabServiceProviderDateFrom.Text.Trim <> String.Empty _
                                                        OrElse Me.txtTabServiceProviderDateTo.Text.Trim <> String.Empty

                If Not blnTextFieldInputted Then
                    udtSM = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00257)
                    Me.udcMessageBox.AddMessage(udtSM)
                    imgTabServiceProviderSPIDErr.Visible = True
                    imgTabServiceProviderSPHKIDErr.Visible = True
                    imgTabServiceProviderSPNameErr.Visible = True
                    imgTabServiceProviderSPChiNameErr.Visible = True
                    imgTabServiceProviderBankAccountNoErr.Visible = True
                    imgTabServiceProviderDateErr.Visible = True

                    udtAuditLogEntry.AddDescripton("SPID", Me.txtTabServiceProviderSPID.Text.Trim)
                    udtAuditLogEntry.AddDescripton("SP HKIC No.", Me.txtTabServiceProviderSPHKID.Text.Trim)
                    udtAuditLogEntry.AddDescripton("SP Name", Me.txtTabServiceProviderSPName.Text.Trim)
                    udtAuditLogEntry.AddDescripton("SP Chi Name", Me.txtTabServiceProviderSPChiName.Text.Trim)
                    udtAuditLogEntry.AddDescripton("Bank Acc No.", Me.txtTabServiceProviderBankAccountNo.Text.Trim)
                    udtAuditLogEntry.AddDescripton("Date From", Me.txtTabServiceProviderDateFrom.Text.Trim)
                    udtAuditLogEntry.AddDescripton("Date To", Me.txtTabServiceProviderDateTo.Text.Trim)
                End If

                txtDateFrom = Me.txtTabServiceProviderDateFrom
                txtDateTo = Me.txtTabServiceProviderDateTo
                imgDateErr = Me.imgTabServiceProviderDateErr
                lblDateText = Me.lblTabServiceProviderDateText

            Case Aspect.eHSAccount
                blnTextFieldInputted = Me.txtTabeHSAccountDocNo.Text.Trim <> String.Empty _
                                                        OrElse Me.txtTabeHSAccountID.Text.Trim <> String.Empty _
                                                        OrElse Me.txtTabeHSAccountName.Text.Trim <> String.Empty _
                                                        OrElse Me.txtTabeHSAccountChiName.Text.Trim <> String.Empty

                If Not blnTextFieldInputted Then
                    udtSM = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00257)
                    Me.udcMessageBox.AddMessage(udtSM)
                    imgTabeHSAccountDocNo.Visible = True
                    imgTabeHSAccountIDErr.Visible = True
                    imgTabeHSAccountNameErr.Visible = True
                    imgTabeHSAccountChiNameErr.Visible = True

                    udtAuditLogEntry.AddDescripton("Doc No.", Me.txtTabeHSAccountDocNo.Text.Trim)
                    udtAuditLogEntry.AddDescripton("EH(S)A ID", Me.txtTabeHSAccountID.Text.Trim)
                    udtAuditLogEntry.AddDescripton("EH(S)A Name", Me.txtTabeHSAccountName.Text.Trim)
                    udtAuditLogEntry.AddDescripton("EH(S)A Chi Name", Me.txtTabeHSAccountChiName.Text.Trim)
                End If

                txtDateFrom = Me.txtTabeHSAccountDateFrom
                txtDateTo = Me.txtTabeHSAccountDateTo
                txtAccID = Me.txtTabeHSAccountID
                imgDateErr = Me.imgTabeHSAccountDateErr
                imgAccIDErr = Me.imgTabeHSAccountIDErr
                lblDateText = Me.lblTabeHSAccountDateText
                ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]

            Case Aspect.AdvancedSearch
                blnTextFieldInputted = Me.txtTabAdvancedSearchSPID.Text.Trim <> String.Empty _
                                                        OrElse Me.txtTabAdvancedSearchSPHKID.Text.Trim <> String.Empty _
                                                        OrElse Me.txtTabAdvancedSearchSPName.Text.Trim <> String.Empty _
                                                        OrElse Me.txtTabAdvancedSearchBankAccountNo.Text.Trim <> String.Empty _
                                                        OrElse Me.txtTabAdvancedSearchTransactionNo.Text.Trim <> String.Empty _
                                                        OrElse Me.txtTabAdvancedSearchDocNo.Text.Trim <> String.Empty _
                                                        OrElse Me.txtTabAdvancedSearchAccountID.Text.Trim <> String.Empty _
                                                        OrElse Me.txtTabAdvancedSearchDateFrom.Text.Trim <> String.Empty _
                                                        OrElse Me.txtTabAdvancedSearchDateTo.Text.Trim <> String.Empty

                If Not blnTextFieldInputted Then
                    udtSM = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00257)
                    Me.udcMessageBox.AddMessage(udtSM)

                    imgTabAdvancedSearchTransactionNoErr.Visible = True
                    imgTabAdvancedSearchSPIDErr.Visible = True
                    imgTabAdvancedSearchSPHKIDErr.Visible = True
                    imgTabAdvancedSearchSPNameErr.Visible = True
                    imgTabAdvancedSearchBankAccountNoErr.Visible = True
                    imgTabAdvancedSearchDocNo.Visible = True
                    imgTabAdvancedSearchAccountIDErr.Visible = True
                    imgTabAdvancedSearchDateErr.Visible = True

                    udtAuditLogEntry.AddDescripton("Transaction No", Me.txtTabAdvancedSearchTransactionNo.Text.Trim)
                    udtAuditLogEntry.AddDescripton("Date From", Me.txtTabAdvancedSearchDateFrom.Text.Trim)
                    udtAuditLogEntry.AddDescripton("Date To", Me.txtTabAdvancedSearchDateTo.Text.Trim)
                    udtAuditLogEntry.AddDescripton("SPID", Me.txtTabAdvancedSearchSPID.Text.Trim)
                    udtAuditLogEntry.AddDescripton("SP HKIC No.", Me.txtTabAdvancedSearchSPHKID.Text.Trim)
                    udtAuditLogEntry.AddDescripton("SP Name", Me.txtTabAdvancedSearchSPName.Text.Trim)
                    udtAuditLogEntry.AddDescripton("Bank Acc No.", Me.txtTabAdvancedSearchBankAccountNo.Text.Trim)
                    udtAuditLogEntry.AddDescripton("Doc No.", Me.txtTabAdvancedSearchDocNo.Text.Trim)
                    udtAuditLogEntry.AddDescripton("EH(S)A ID", Me.txtTabAdvancedSearchAccountID.Text.Trim)
                End If

                txtDateFrom = Me.txtTabAdvancedSearchDateFrom
                txtDateTo = Me.txtTabAdvancedSearchDateTo
                txtAccID = Me.txtTabAdvancedSearchAccountID
                imgDateErr = Me.imgTabAdvancedSearchDateErr
                imgAccIDErr = Me.imgTabAdvancedSearchAccountIDErr
                lblDateText = Me.lblTabAdvancedSearchDateText

        End Select

        If blnTextFieldInputted Then
            ' Transaction Date can be empty only if any text fields are inputted
            If txtDateFrom.Text.Trim = String.Empty AndAlso txtDateTo.Text.Trim = String.Empty Then
                ' Okay, bypass the checking
            Else
                ' One or both fields have been inputted, need checking

                ' 1: Check completeness
                If (txtDateFrom.Text.Trim = String.Empty AndAlso txtDateTo.Text.Trim <> String.Empty) _
                        OrElse (txtDateFrom.Text.Trim <> String.Empty AndAlso txtDateTo.Text.Trim = String.Empty) Then
                    ' Please complete "Date". 
                    udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00364, "%s", lblDateText.Text.Trim)
                    imgDateErr.Visible = True
                    blnValidDate = False
                End If

                ' 2: Check the date format
                Dim strTransactionDateFrom As String = IIf(udtFormatter.formatInputDate(txtDateFrom.Text.Trim) <> String.Empty, udtFormatter.formatInputDate(txtDateFrom.Text.Trim), txtDateFrom.Text.Trim)
                Dim strTransactionDateTo As String = IIf(udtFormatter.formatInputDate(txtDateTo.Text.Trim) <> String.Empty, udtFormatter.formatInputDate(txtDateTo.Text.Trim), txtDateTo.Text.Trim)

                If blnValidDate Then
                    ' Format the input date (Date From / To)
                    udtSystemMessage = udtValidator.chkInputDate(strTransactionDateFrom, True, True)
                    If IsNothing(udtSystemMessage) Then udtSystemMessage = udtValidator.chkInputDate(strTransactionDateTo, True, True)

                    If Not IsNothing(udtSystemMessage) Then
                        udcMessageBox.AddMessage(udtSystemMessage, "%s", lblDateText.Text.Trim)
                        imgDateErr.Visible = True
                        blnValidDate = False
                    End If
                End If

                ' 3: Check date dependency: From < To
                If blnValidDate Then
                    udtSystemMessage = udtValidator.chkInputValidFromDateCutoffDate(FunctCode.FUNT990000, MsgCode.MSG00374, _
                            udtFormatter.convertDate(strTransactionDateFrom, String.Empty), udtFormatter.convertDate(strTransactionDateTo, String.Empty))

                    ' The From Date should not be later than the To Date in "Date".
                    If Not IsNothing(udtSystemMessage) Then
                        imgDateErr.Visible = True
                        udcMessageBox.AddMessage(udtSystemMessage, "%s", lblDateText.Text.Trim)
                    End If
                End If

                If blnValidDate Then
                    txtDateFrom.Text = strTransactionDateFrom
                    txtDateTo.Text = strTransactionDateTo
                End If
            End If

            If Not txtAccID Is Nothing AndAlso txtAccID.Text.Trim() <> String.Empty Then
                If Not udtValidator.chkValidatedEHSAccountNumber(txtAccID.Text.Trim()) Then
                    ' Invalid EHS Account ID
                    udcMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00362))
                    If Not imgAccIDErr Is Nothing Then
                        imgAccIDErr.Visible = True
                    End If
                End If
            End If

        End If

        '' Check Service Date
        'If txtSServiceDateFrom.Text.Trim <> String.Empty OrElse txtSServiceDateTo.Text.Trim <> String.Empty Then
        '    ' One or both fields have been inputted, need checking
        '    blnValidDate = True

        '    ' 1: Check completeness
        '    If txtSServiceDateFrom.Text.Trim = String.Empty OrElse txtSServiceDateTo.Text.Trim = String.Empty Then
        '        ' Please complete the "Service Date".
        '        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00016)
        '        imgAlertSServiceDate.Visible = True
        '        blnValidDate = False
        '    End If

        '    ' 2: Check the date format
        '    'INT15-0015 (Fix date format checking in HCVU) [Start][Chris YIM]
        '    '-----------------------------------------------------------------------------------------
        '    Dim strServiceDateFrom As String = IIf(udtFormatter.formatInputDate(txtSServiceDateFrom.Text.Trim) <> String.Empty, udtFormatter.formatInputDate(txtSServiceDateFrom.Text.Trim), txtSServiceDateFrom.Text.Trim)
        '    Dim strServiceDateTo As String = IIf(udtFormatter.formatInputDate(txtSServiceDateTo.Text.Trim) <> String.Empty, udtFormatter.formatInputDate(txtSServiceDateTo.Text.Trim), txtSServiceDateTo.Text.Trim)

        '    If blnValidDate Then
        '        ' Format the input date (Service Date From / To)
        '        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '        '-----------------------------------------------------------------------------------------
        '        'txtSServiceDateFrom.Text = udtFormatter.formatDate(txtSServiceDateFrom.Text.Trim)
        '        'txtSServiceDateTo.Text = udtFormatter.formatDate(txtSServiceDateTo.Text.Trim)
        '        'txtSServiceDateFrom.Text = udtFormatter.formatInputDate(txtSServiceDateFrom.Text.Trim)
        '        'txtSServiceDateTo.Text = udtFormatter.formatInputDate(txtSServiceDateTo.Text.Trim)
        '        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        '        'udtSystemMessage = udtValidator.chkInputDate(txtSServiceDateFrom.Text, False)
        '        'If IsNothing(udtSystemMessage) Then udtSystemMessage = udtValidator.chkInputDate(txtSServiceDateTo.Text, False)
        '        udtSystemMessage = udtValidator.chkInputDate(strServiceDateFrom, True, True)
        '        If IsNothing(udtSystemMessage) Then udtSystemMessage = udtValidator.chkInputDate(strServiceDateTo, True, True)

        '        'If Not IsNothing(udtSystemMessage) AndAlso udtSystemMessage.MessageCode <> MsgCode.MSG00028 Then
        '        If Not IsNothing(udtSystemMessage) Then
        '            udcMessageBox.AddMessage(udtSystemMessage, "%s", lblSServiceDateText.Text)
        '            imgAlertSServiceDate.Visible = True
        '            blnValidDate = False
        '        End If
        '    End If

        '    ' 3: Check date dependency: From < To
        '    If blnValidDate Then
        '        'udtSystemMessage = udtValidator.chkInputValidFromDateCutoffDate(FunctionCode, MsgCode.MSG00017, _
        '        '         udtFormatter.convertDate(txtSServiceDateFrom.Text, String.Empty), udtFormatter.convertDate(txtSServiceDateTo.Text, String.Empty))
        '        udtSystemMessage = udtValidator.chkInputValidFromDateCutoffDate(FunctionCode, MsgCode.MSG00017, _
        '                udtFormatter.convertDate(strServiceDateFrom, String.Empty), udtFormatter.convertDate(strServiceDateTo, String.Empty))


        '        ' The "Service Date From" should not be later than the "Service Date To".
        '        If Not IsNothing(udtSystemMessage) Then
        '            imgAlertSServiceDate.Visible = True
        '            udcMessageBox.AddMessage(udtSystemMessage)
        '        End If
        '    End If

        '    If blnValidDate Then
        '        txtSServiceDateFrom.Text = strServiceDateFrom
        '        txtSServiceDateTo.Text = strServiceDateTo
        '    End If
        '    'INT15-0015 (Fix date format checking in HCVU) [End][Chris YIM]
        'End If
        'CRE13-012 (RCH Code sorting) [End][Chris YIM]

        If udcMessageBox.GetCodeTable.Rows.Count <> 0 Then
            udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationFail, udtAuditLogEntry, LogID.LOG00003, AuditLogDescription.SearchFail)
            Return False
        Else
            Return True
        End If
    End Function

    Protected Overrides Function SF_Search(ByRef udtAuditLogEntry As AuditLogEntry, ByVal blnOverrideResultLimit As Boolean) As BaseBLL.BLLSearchResult
        If blnOverrideResultLimit Then
            Return GetTransaction(Session(SESS_SearchCriteria), True)
        Else
            Return GetTransaction()
        End If
    End Function

    Protected Overrides Function SF_BindSearchResult(ByRef udtAuditLogEntry As AuditLogEntry, ByVal udtBLLSearchResult As BaseBLL.BLLSearchResult) As Integer
        Dim dtTransaction As DataTable
        Dim intRowCount As Integer

        Try
            dtTransaction = CType(udtBLLSearchResult.Data, DataTable)

        Catch ex As Exception
            Throw

        End Try

        intRowCount = dtTransaction.Rows.Count

        If intRowCount > 0 Then
            LoadTransactionGrid(dtTransaction)

            Dim udtSearchCriteria As SearchCriteria = Session(SESS_SearchCriteria)
            BuildSearchCriteriaReview(udtSearchCriteria)
            'CRE13-012 (RCH Code sorting) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Select Case udtSearchCriteria.Aspect
                Case Aspect.Transaction
                    AdjustButtonsBehaviour(Me.ddlTabTransactionTransactionStatus.SelectedValue.Trim, Me.ddlTabTransactionAuthorizedStatus.SelectedValue.Trim)
                Case Aspect.ServiceProvider
                    AdjustButtonsBehaviour(Me.ddlTabServiceProviderTransactionStatus.SelectedValue.Trim, Me.ddlTabServiceProviderAuthorizedStatus.SelectedValue.Trim)
                Case Aspect.eHSAccount
                    AdjustButtonsBehaviour(Me.ddlTabeHSAccountTransactionStatus.SelectedValue.Trim, Me.ddlTabeHSAccountAuthorizedStatus.SelectedValue.Trim)
                Case Aspect.AdvancedSearch
                    AdjustButtonsBehaviour(Me.ddlTabAdvancedSearchTransactionStatus.SelectedValue.Trim, Me.ddlTabAdvancedSearchAuthorizedStatus.SelectedValue.Trim)
            End Select

            BuildRecordSummary(dtTransaction)
            'CRE13-012 (RCH Code sorting) [End][Chris YIM]

            ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            If udtHCVUUserBLL.IsSSSCMCUser(udtHCVUUserBLL.GetHCVUUser) Then
                panRecordSummary.Visible = False
            Else
                panRecordSummary.Visible = True
            End If
            ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

            MultiViewReimClaimTransManagement.ActiveViewIndex = ViewIndex.Transaction
        End If

        Return intRowCount
    End Function

    Protected Overrides Sub SF_FinalizeSearch(ByRef udtAuditLogEntry As AuditLogEntry)
    End Sub

    Protected Overrides Sub SF_ConfirmSearch_Click()
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim enumSearchResult As SearchResultEnum

        udtAuditLogEntry.WriteStartLog(LogID.LOG00001, AuditLogDescription.Search)

        If Not IsNothing(Session(SESS_FromRedirect)) Then
            If Session(SESS_FromRedirect) = "Y" Then
                'CRE13-012 (RCH Code sorting) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Me.ddlTabTransactionTransactionStatus.SelectedIndex = 0
                Me.ddlTabServiceProviderTransactionStatus.SelectedIndex = 0
                Me.ddlTabeHSAccountTransactionStatus.SelectedIndex = 0
                Me.ddlTabAdvancedSearchTransactionStatus.SelectedIndex = 0
                'CRE13-012 (RCH Code sorting) [End][Chris YIM]

                Session(SESS_FromRedirect) = Nothing
            End If
        End If

        Try
            enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, udcMessageBox, udcInfoMessageBox, False, True)

        Catch eSQL As SqlClient.SqlException

            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown SqlException")
            udtAuditLogEntry.AddDescripton("Message", eSQL.Message)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditLogDescription.SearchFail)
            Throw

        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception")
            udtAuditLogEntry.AddDescripton("Message", ex.Message)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditLogDescription.SearchFail)
            Throw

        End Try

        Select Case enumSearchResult
            Case SearchResultEnum.Success
                udtAuditLogEntry.WriteEndLog(LogID.LOG00002, AuditLogDescription.SearchSuccessful)

            Case SearchResultEnum.OverResultList1stLimit_Alert
                ' Here is unexpected. The user may perform the search within the non-peak period, but he waits and inputs
                ' the token passcode for too long time and passed into the peak period
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Unexpected enumSearchResult={0}", enumSearchResult.ToString))
                udcMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, "00009"))
                udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.SearchFail, udtAuditLogEntry, LogID.LOG00003, AuditLogDescription.SearchFail)

            Case SearchResultEnum.OverResultListOverrideLimit
                ' Here is unexpected. The number of result grows while the user is inputting the token passcode
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Unexpected enumSearchResult={0}", enumSearchResult.ToString))
                udcMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, "00009"))
                udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.SearchFail, udtAuditLogEntry, LogID.LOG00003, AuditLogDescription.SearchFail)

            Case Else
                Throw New Exception("Error: Class = [HCVU.claimTransManagement], Method = [SF_ConfirmSearch_Click], Message = The unexpected error for the result of re-search")

        End Select
    End Sub

    Protected Overrides Sub SF_CancelSearch_Click()
    End Sub
#End Region
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

    Protected Sub ibtnSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcInfoMessageBox.Visible = False
        udcMessageBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        'CRE13-012 (RCH Code sorting) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Select Case Session(SESS.SelectedTabIndex)
            Case Aspect.Transaction
                imgTabTransactionDateErr.Visible = False

                udtAuditLogEntry.AddDescripton("Transaction No", Me.txtTabTransactionTransactionNo.Text.Trim)
                udtAuditLogEntry.AddDescripton("Profession", Me.ddlTabTransactionHealthProfession.SelectedItem.Value)

                udtAuditLogEntry.AddDescripton("Type of Date", Me.rblTabTransactionTypeOfDate.SelectedItem.Text.Trim)
                udtAuditLogEntry.AddDescripton("Date From", Me.txtTabTransactionDateFrom.Text.Trim)
                udtAuditLogEntry.AddDescripton("Date To", Me.txtTabTransactionDateTo.Text.Trim)
                udtAuditLogEntry.AddDescripton("Scheme", Me.ddlTabTransactionScheme.SelectedValue)
                udtAuditLogEntry.AddDescripton("Status", Me.ddlTabTransactionTransactionStatus.SelectedItem.Value)
                udtAuditLogEntry.AddDescripton("Authorized Status", Me.ddlTabTransactionAuthorizedStatus.SelectedItem.Value)
                udtAuditLogEntry.AddDescripton("Invalidation Status", Me.ddlTabTransactionInvalidationStatus.SelectedValue)
                udtAuditLogEntry.AddDescripton("Reimbursement Method ", Me.ddlTabTransactionReimbursementMethod.SelectedValue)
                'CRE20-003 (add search criteria) [Start][Martin]
                udtAuditLogEntry.AddDescripton("Vaccine ", Me.ddlTabTransactionVaccines.SelectedValue)
                udtAuditLogEntry.AddDescripton("Dose ", Me.ddlTabTransactionDose.SelectedValue)

                If (New GeneralFunction).CheckTurnOnMeansOfInput = GeneralFunction.EnumTurnOnStatus.Yes Then udtAuditLogEntry.AddDescripton("Means of Input", Me.ddlTabTransactionMeansOfInput.SelectedValue)
                udtAuditLogEntry.AddDescripton("School/RCH Code", Me.txtTabTransactionRCHRode.Text.Trim)
                'CRE20-003 (add search criteria) [End][Martin]

                udtAuditLogEntry.AddDescripton("User Aspect", "Transaction")
                udtAuditLogEntry.AddDescripton("Program Aspect", "Transaction")

                ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
            Case Aspect.ServiceProvider
                imgTabServiceProviderDateErr.Visible = False

                ' Service Provider
                udtAuditLogEntry.AddDescripton("SPID", Me.txtTabServiceProviderSPID.Text.Trim)
                udtAuditLogEntry.AddDescripton("SP HKID", Me.txtTabServiceProviderSPHKID.Text.Trim)
                udtAuditLogEntry.AddDescripton("SP Name", Me.txtTabServiceProviderSPName.Text.Trim)
                udtAuditLogEntry.AddDescripton("SP Chi Name", Me.txtTabServiceProviderSPChiName.Text.Trim)
                udtAuditLogEntry.AddDescripton("Bank Account No", Me.txtTabServiceProviderBankAccountNo.Text.Trim)

                udtAuditLogEntry.AddDescripton("Type of Date", Me.rblTabServiceProviderTypeOfDate.SelectedItem.Text.Trim)
                udtAuditLogEntry.AddDescripton("Date From", Me.txtTabServiceProviderDateFrom.Text.Trim)
                udtAuditLogEntry.AddDescripton("Date To", Me.txtTabServiceProviderDateTo.Text.Trim)
                udtAuditLogEntry.AddDescripton("Scheme", Me.ddlTabServiceProviderScheme.SelectedValue)
                udtAuditLogEntry.AddDescripton("Status", Me.ddlTabServiceProviderTransactionStatus.SelectedItem.Value)
                udtAuditLogEntry.AddDescripton("Authorized Status", Me.ddlTabServiceProviderAuthorizedStatus.SelectedItem.Value)
                udtAuditLogEntry.AddDescripton("Invalidation Status", Me.ddlTabServiceProviderInvalidationStatus.SelectedValue)
                udtAuditLogEntry.AddDescripton("Reimbursement Method ", Me.ddlTabServiceProviderReimbursementMethod.SelectedValue)

                If (New GeneralFunction).CheckTurnOnMeansOfInput = GeneralFunction.EnumTurnOnStatus.Yes Then udtAuditLogEntry.AddDescripton("Means of Input", Me.ddlTabServiceProviderMeansOfInput.SelectedValue)
                udtAuditLogEntry.AddDescripton("RCH Code", Me.txtTabServiceProviderRCHRode.Text.Trim)

                udtAuditLogEntry.AddDescripton("User Aspect", "Service Provider")
                udtAuditLogEntry.AddDescripton("Program Aspect", "Service Provider")

            Case Aspect.eHSAccount
                imgTabeHSAccountDateErr.Visible = False
                imgTabeHSAccountIDErr.Visible = False
                imgTabeHSAccountDocNo.Visible = False

                ' eHealth (Subsidies) Account
                udtAuditLogEntry.AddDescripton("Doc Code", Me.ddlTabeHSAccountDocType.SelectedValue)
                udtAuditLogEntry.AddDescripton("Doc No", Me.txtTabeHSAccountDocNo.Text.Trim)
                udtAuditLogEntry.AddDescripton("EH(S)A ID", Me.txtTabeHSAccountID.Text.Trim)
                udtAuditLogEntry.AddDescripton("EH(S)A Name", Me.txtTabeHSAccountName.Text.Trim)
                udtAuditLogEntry.AddDescripton("EH(S)A Chi Name", Me.txtTabeHSAccountChiName.Text.Trim)

                udtAuditLogEntry.AddDescripton("Type of Date", Me.rblTabeHSAccountTypeOfDate.SelectedItem.Text.Trim)
                udtAuditLogEntry.AddDescripton("Date From", Me.txtTabeHSAccountDateFrom.Text.Trim)
                udtAuditLogEntry.AddDescripton("Date To", Me.txtTabeHSAccountDateTo.Text.Trim)
                udtAuditLogEntry.AddDescripton("Scheme", Me.ddlTabeHSAccountScheme.SelectedValue)
                udtAuditLogEntry.AddDescripton("Status", Me.ddlTabeHSAccountTransactionStatus.SelectedItem.Value)
                udtAuditLogEntry.AddDescripton("Authorized Status", Me.ddlTabeHSAccountAuthorizedStatus.SelectedItem.Value)
                udtAuditLogEntry.AddDescripton("Invalidation Status", Me.ddlTabeHSAccountInvalidationStatus.SelectedValue)
                udtAuditLogEntry.AddDescripton("Reimbursement Method ", Me.ddlTabeHSAccountReimbursementMethod.SelectedValue)

                If (New GeneralFunction).CheckTurnOnMeansOfInput = GeneralFunction.EnumTurnOnStatus.Yes Then udtAuditLogEntry.AddDescripton("Means of Input", Me.ddlTabeHSAccountMeansOfInput.SelectedValue)
                udtAuditLogEntry.AddDescripton("RCH Code", Me.txtTabeHSAccountRCHRode.Text.Trim)

                udtAuditLogEntry.AddDescripton("User Aspect", "eHealth (Subsidies) Account")
                udtAuditLogEntry.AddDescripton("Program Aspect", "eHealth (Subsidies) Account")
                ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]

            Case Aspect.AdvancedSearch
                imgTabAdvancedSearchDateErr.Visible = False
                imgTabAdvancedSearchAccountIDErr.Visible = False
                imgTabAdvancedSearchDocNo.Visible = False

                udtAuditLogEntry.AddDescripton("Transaction No", Me.txtTabAdvancedSearchTransactionNo.Text.Trim)
                udtAuditLogEntry.AddDescripton("Profession", Me.ddlTabAdvancedSearchHealthProfession.SelectedItem.Value)

                udtAuditLogEntry.AddDescripton("Type of Date", Me.rblTabAdvancedSearchTypeOfDate.SelectedItem.Text.Trim)
                udtAuditLogEntry.AddDescripton("Date From", Me.txtTabAdvancedSearchDateFrom.Text.Trim)
                udtAuditLogEntry.AddDescripton("Date To", Me.txtTabAdvancedSearchDateTo.Text.Trim)
                udtAuditLogEntry.AddDescripton("Scheme", Me.ddlTabAdvancedSearchScheme.SelectedValue)
                udtAuditLogEntry.AddDescripton("Status", Me.ddlTabAdvancedSearchTransactionStatus.SelectedItem.Value)
                udtAuditLogEntry.AddDescripton("Authorized Status", Me.ddlTabAdvancedSearchAuthorizedStatus.SelectedItem.Value)
                udtAuditLogEntry.AddDescripton("Invalidation Status", Me.ddlTabAdvancedSearchInvalidationStatus.SelectedValue)
                udtAuditLogEntry.AddDescripton("Reimbursement Method ", Me.ddlTabAdvancedSearchReimbursementMethod.SelectedValue)

                If (New GeneralFunction).CheckTurnOnMeansOfInput = GeneralFunction.EnumTurnOnStatus.Yes Then udtAuditLogEntry.AddDescripton("Means of Input", Me.ddlTabAdvancedSearchMeansOfInput.SelectedValue)
                udtAuditLogEntry.AddDescripton("RCH Code", Me.txtTabAdvancedSearchRCHRode.Text.Trim)

                udtAuditLogEntry.AddDescripton("SPID", Me.txtTabAdvancedSearchSPID.Text.Trim)
                udtAuditLogEntry.AddDescripton("SP HKID", Me.txtTabAdvancedSearchSPHKID.Text.Trim)
                udtAuditLogEntry.AddDescripton("SP Name", Me.txtTabAdvancedSearchSPName.Text.Trim)
                udtAuditLogEntry.AddDescripton("Bank Account No", Me.txtTabAdvancedSearchBankAccountNo.Text.Trim)

                udtAuditLogEntry.AddDescripton("Doc Code", Me.ddlTabAdvancedSearchDocType.SelectedValue)
                udtAuditLogEntry.AddDescripton("Doc No", Me.txtTabAdvancedSearchDocNo.Text.Trim)
                udtAuditLogEntry.AddDescripton("EHA ID", Me.txtTabAdvancedSearchAccountID.Text.Trim)

                udtAuditLogEntry.AddDescripton("User Aspect", "Advanced Search")
                Select Case SearchAspectRedirection()
                    Case Aspect.Transaction
                        udtAuditLogEntry.AddDescripton("Program Aspect", "Transaction")
                    Case Aspect.ServiceProvider
                        udtAuditLogEntry.AddDescripton("Program Aspect", "Service Provider")
                    Case Aspect.eHSAccount
                        udtAuditLogEntry.AddDescripton("Program Aspect", "eHealth (Subsidies) Account")
                    Case Aspect.AdvancedSearch
                        udtAuditLogEntry.AddDescripton("Program Aspect", "Advanced Search")
                End Select
        End Select
        'CRE13-012 (RCH Code sorting) [End][Chris YIM]

        udtAuditLogEntry.WriteStartLog(LogID.LOG00001, AuditLogDescription.Search)

        ' CRE12-015 Add the respective practice number in “Practice” in the functions under “Reimbursement” in eHS [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------------------------------------------
        ' Implement Collapsible Search Criteria Review
        udcCollapsibleSearchCriteriaReview.Collapsed = True
        udcCollapsibleSearchCriteriaReview.ClientState = "True"
        ' CRE12-015 Add the respective practice number in “Practice” in the functions under “Reimbursement” in eHS [End][Tommy L]

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
        ' -------------------------------------------------------------------------
        Dim enumSearchResult As SearchResultEnum

        Try
            enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, udcMessageBox, udcInfoMessageBox)

        Catch eSQL As SqlClient.SqlException
            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown SqlException")
            udtAuditLogEntry.AddDescripton("Message", eSQL.Message)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditLogDescription.SearchFail)
            Throw

        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception")
            udtAuditLogEntry.AddDescripton("Message", ex.Message)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditLogDescription.SearchFail)
            Throw

        End Try

        Select Case enumSearchResult
            Case SearchResultEnum.Success
                udtAuditLogEntry.WriteEndLog(LogID.LOG00002, AuditLogDescription.SearchSuccessful)

            Case SearchResultEnum.ValidationFail
                ' Audit Log has been handled in [SF_ValidateSearch] method

            Case SearchResultEnum.NoRecordFound
                udtAuditLogEntry.WriteEndLog(LogID.LOG00046, AuditLogDescription.SearchCompleteNoRecordFound)

            Case SearchResultEnum.OverResultList1stLimit_PopUp
                udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditLogDescription.SearchFail)

            Case SearchResultEnum.OverResultList1stLimit_Alert
                udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditLogDescription.SearchFail)

            Case SearchResultEnum.OverResultListOverrideLimit
                udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditLogDescription.SearchFail)

            Case Else
                Throw New Exception("Error: Class = [HCVU.claimTransManagement], Method = [ibtnSearch_Click], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")

        End Select

    End Sub


    Private Function GetTransaction(Optional ByVal udtSearchCriteria As SearchCriteria = Nothing, Optional ByVal blnOverrideResultLimit As Boolean = False) As BaseBLL.BLLSearchResult
        'CRE13-012 (RCH Code sorting) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtBLLSearchResult As BaseBLL.BLLSearchResult = Nothing
        Dim EnumSelectedStoredProc As Aspect = Aspect.AdvancedSearch
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        If IsNothing(udtSearchCriteria) Then

            udtSearchCriteria = New SearchCriteria

            udtSearchCriteria.ServiceDateFrom = String.Empty
            udtSearchCriteria.ServiceDateTo = String.Empty
            udtSearchCriteria.FromDate = String.Empty
            udtSearchCriteria.CutoffDate = String.Empty

            ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
            Select Case Session(SESS.SelectedTabIndex)
                Case Aspect.Transaction
                    EnumSelectedStoredProc = Aspect.Transaction

                    udtSearchCriteria.Aspect = Aspect.Transaction

                    'Group: Transaction
                    udtSearchCriteria.TransNum = IIf(Me.txtTabTransactionTransactionNo.Text.Trim = String.Empty, String.Empty, udtFormatter.formatSystemNumberReverse(Me.txtTabTransactionTransactionNo.Text.Trim))
                    udtSearchCriteria.HealthProf = Me.ddlTabTransactionHealthProfession.SelectedValue.Trim
                    udtSearchCriteria.SubsidizeItemCode = Me.ddlTabTransactionVaccines.SelectedValue.Trim 'CRE20-003 (add search criteria) [Martin]
                    udtSearchCriteria.DoseCode = Me.ddlTabTransactionDose.SelectedValue.Trim 'CRE20-003 (add search criteria) [Martin]

                    'Group: Common
                    If rblTabTransactionTypeOfDate.SelectedValue = TypeOfDate.ServiceDate Then
                        udtSearchCriteria.ServiceDateFrom = IIf(Me.txtTabTransactionDateFrom.Text.Trim = String.Empty, String.Empty, udtFormatter.convertDate(Me.txtTabTransactionDateFrom.Text.Trim, String.Empty))
                        udtSearchCriteria.ServiceDateTo = IIf(Me.txtTabTransactionDateTo.Text.Trim = String.Empty, String.Empty, udtFormatter.convertDate(Me.txtTabTransactionDateTo.Text.Trim, String.Empty))
                    End If

                    If rblTabTransactionTypeOfDate.SelectedValue = TypeOfDate.TransactionDate Then
                        udtSearchCriteria.FromDate = IIf(Me.txtTabTransactionDateFrom.Text.Trim = String.Empty, String.Empty, udtFormatter.convertDate(Me.txtTabTransactionDateFrom.Text.Trim, String.Empty))
                        udtSearchCriteria.CutoffDate = IIf(Me.txtTabTransactionDateTo.Text.Trim = String.Empty, String.Empty, udtFormatter.convertDate(Me.txtTabTransactionDateTo.Text.Trim, String.Empty))
                    End If

                    udtSearchCriteria.SchemeCode = Me.ddlTabTransactionScheme.SelectedValue.Trim
                    udtSearchCriteria.TransStatus = Me.ddlTabTransactionTransactionStatus.SelectedValue.Trim
                    udtSearchCriteria.AuthorizedStatus = Me.ddlTabTransactionAuthorizedStatus.SelectedValue.Trim
                    udtSearchCriteria.Invalidation = Me.ddlTabTransactionInvalidationStatus.SelectedValue
                    udtSearchCriteria.ReimbursementMethod = Me.ddlTabTransactionReimbursementMethod.SelectedValue.Trim
                    udtSearchCriteria.MeansOfInput = Me.ddlTabTransactionMeansOfInput.SelectedValue.Trim
                    udtSearchCriteria.SchoolOrRCHCode = Me.txtTabTransactionRCHRode.Text.Trim  'CRE20-003 (add search criteria) [Martin]

                    'Group: SP
                    udtSearchCriteria.ServiceProviderID = String.Empty
                    udtSearchCriteria.ServiceProviderHKIC = String.Empty
                    udtSearchCriteria.ServiceProviderName = String.Empty
                    udtSearchCriteria.ServiceProviderChiName = String.Empty
                    udtSearchCriteria.BankAcctNo = String.Empty

                    'Group: EHS Account
                    udtSearchCriteria.DocumentType = String.Empty
                    udtSearchCriteria.VoucherAccID = String.Empty
                    udtSearchCriteria.DocumentNo1 = String.Empty
                    udtSearchCriteria.DocumentNo2 = String.Empty
                    udtSearchCriteria.RawIdentityNum = String.Empty
                    udtSearchCriteria.VoucherRecipientName = String.Empty
                    udtSearchCriteria.VoucherRecipientChiName = String.Empty

                Case Aspect.ServiceProvider
                    EnumSelectedStoredProc = Aspect.ServiceProvider

                    udtSearchCriteria.Aspect = Aspect.ServiceProvider

                    'Group: Transaction
                    udtSearchCriteria.TransNum = String.Empty
                    udtSearchCriteria.HealthProf = String.Empty
                    'CRE20-003 (add search criteria) [Start][Martin]
                    udtSearchCriteria.SubsidizeItemCode = String.Empty
                    udtSearchCriteria.DoseCode = String.Empty
                    'CRE20-003 (add search criteria) [End][Martin]

                    'Group: Common
                    If rblTabServiceProviderTypeOfDate.SelectedValue = TypeOfDate.ServiceDate Then
                        udtSearchCriteria.ServiceDateFrom = IIf(Me.txtTabServiceProviderDateFrom.Text.Trim = String.Empty, String.Empty, udtFormatter.convertDate(Me.txtTabServiceProviderDateFrom.Text.Trim, String.Empty))
                        udtSearchCriteria.ServiceDateTo = IIf(Me.txtTabServiceProviderDateTo.Text.Trim = String.Empty, String.Empty, udtFormatter.convertDate(Me.txtTabServiceProviderDateTo.Text.Trim, String.Empty))
                    End If

                    If rblTabServiceProviderTypeOfDate.SelectedValue = TypeOfDate.TransactionDate Then
                        udtSearchCriteria.FromDate = IIf(Me.txtTabServiceProviderDateFrom.Text.Trim = String.Empty, String.Empty, udtFormatter.convertDate(Me.txtTabServiceProviderDateFrom.Text.Trim, String.Empty))
                        udtSearchCriteria.CutoffDate = IIf(Me.txtTabServiceProviderDateTo.Text.Trim = String.Empty, String.Empty, udtFormatter.convertDate(Me.txtTabServiceProviderDateTo.Text.Trim, String.Empty))
                    End If

                    udtSearchCriteria.SchemeCode = Me.ddlTabServiceProviderScheme.SelectedValue.Trim
                    udtSearchCriteria.TransStatus = Me.ddlTabServiceProviderTransactionStatus.SelectedValue.Trim
                    udtSearchCriteria.AuthorizedStatus = Me.ddlTabServiceProviderAuthorizedStatus.SelectedValue.Trim
                    udtSearchCriteria.Invalidation = Me.ddlTabServiceProviderInvalidationStatus.SelectedValue
                    udtSearchCriteria.ReimbursementMethod = Me.ddlTabServiceProviderReimbursementMethod.SelectedValue.Trim
                    udtSearchCriteria.MeansOfInput = Me.ddlTabServiceProviderMeansOfInput.SelectedValue.Trim
                    udtSearchCriteria.SchoolOrRCHCode = Me.txtTabServiceProviderRCHRode.Text.Trim

                    'Group: SP
                    udtSearchCriteria.ServiceProviderID = Me.txtTabServiceProviderSPID.Text.Trim
                    udtSearchCriteria.ServiceProviderHKIC = Me.txtTabServiceProviderSPHKID.Text.Trim.ToUpper.Replace("(", String.Empty).Replace(")", String.Empty)
                    udtSearchCriteria.ServiceProviderName = Me.txtTabServiceProviderSPName.Text.Trim
                    udtSearchCriteria.ServiceProviderChiName = Me.txtTabServiceProviderSPChiName.Text.Trim
                    udtSearchCriteria.BankAcctNo = Me.txtTabServiceProviderBankAccountNo.Text.Trim

                    'Group: EHS Account
                    udtSearchCriteria.DocumentType = String.Empty
                    udtSearchCriteria.VoucherAccID = String.Empty
                    udtSearchCriteria.DocumentNo1 = String.Empty
                    udtSearchCriteria.DocumentNo2 = String.Empty
                    udtSearchCriteria.RawIdentityNum = String.Empty
                    udtSearchCriteria.VoucherRecipientName = String.Empty
                    udtSearchCriteria.VoucherRecipientChiName = String.Empty



                Case Aspect.eHSAccount
                    EnumSelectedStoredProc = Aspect.eHSAccount

                    udtSearchCriteria.Aspect = Aspect.eHSAccount

                    'Group: Transaction
                    udtSearchCriteria.TransNum = String.Empty
                    udtSearchCriteria.HealthProf = String.Empty
                    'CRE20-003 (add search criteria) [Start][Martin]
                    udtSearchCriteria.SubsidizeItemCode = String.Empty
                    udtSearchCriteria.DoseCode = String.Empty
                    'CRE20-003 (add search criteria) [End][Martin]

                    'Group: Common
                    If rblTabeHSAccountTypeOfDate.SelectedValue = TypeOfDate.ServiceDate Then
                        udtSearchCriteria.ServiceDateFrom = IIf(Me.txtTabeHSAccountDateFrom.Text.Trim = String.Empty, String.Empty, udtFormatter.convertDate(Me.txtTabeHSAccountDateFrom.Text.Trim, String.Empty))
                        udtSearchCriteria.ServiceDateTo = IIf(Me.txtTabeHSAccountDateTo.Text.Trim = String.Empty, String.Empty, udtFormatter.convertDate(Me.txtTabeHSAccountDateTo.Text.Trim, String.Empty))
                    End If

                    If rblTabeHSAccountTypeOfDate.SelectedValue = TypeOfDate.TransactionDate Then
                        udtSearchCriteria.FromDate = IIf(Me.txtTabeHSAccountDateFrom.Text.Trim = String.Empty, String.Empty, udtFormatter.convertDate(Me.txtTabeHSAccountDateFrom.Text.Trim, String.Empty))
                        udtSearchCriteria.CutoffDate = IIf(Me.txtTabeHSAccountDateTo.Text.Trim = String.Empty, String.Empty, udtFormatter.convertDate(Me.txtTabeHSAccountDateTo.Text.Trim, String.Empty))
                    End If

                    udtSearchCriteria.SchemeCode = Me.ddlTabeHSAccountScheme.SelectedValue.Trim
                    udtSearchCriteria.TransStatus = Me.ddlTabeHSAccountTransactionStatus.SelectedValue.Trim
                    udtSearchCriteria.AuthorizedStatus = Me.ddlTabeHSAccountAuthorizedStatus.SelectedValue.Trim
                    udtSearchCriteria.Invalidation = Me.ddlTabeHSAccountInvalidationStatus.SelectedValue
                    udtSearchCriteria.ReimbursementMethod = Me.ddlTabeHSAccountReimbursementMethod.SelectedValue.Trim
                    udtSearchCriteria.MeansOfInput = Me.ddlTabeHSAccountMeansOfInput.SelectedValue.Trim
                    udtSearchCriteria.SchoolOrRCHCode = Me.txtTabeHSAccountRCHRode.Text.Trim  'CRE20-003 (add search criteria)[Martin]

                    'Group: SP
                    udtSearchCriteria.ServiceProviderID = String.Empty
                    udtSearchCriteria.ServiceProviderHKIC = String.Empty
                    udtSearchCriteria.ServiceProviderName = String.Empty
                    udtSearchCriteria.ServiceProviderChiName = String.Empty
                    udtSearchCriteria.BankAcctNo = String.Empty

                    'Group: EHS Account
                    udtSearchCriteria.DocumentType = Me.ddlTabeHSAccountDocType.SelectedValue

                    If Not String.IsNullOrEmpty(Me.txtTabeHSAccountID.Text) Then
                        udtSearchCriteria.VoucherAccID = Me.txtTabeHSAccountID.Text.Substring(0, Me.txtTabeHSAccountID.Text.Length - 1)
                    Else
                        udtSearchCriteria.VoucherAccID = ""
                    End If

                    Dim aryDocumentNo As String() = Me.txtTabeHSAccountDocNo.Text.Replace("(", "").Replace(")", "").Replace("-", "").Split("/")
                    If aryDocumentNo.Length > 1 Then
                        udtSearchCriteria.DocumentNo1 = aryDocumentNo(1)
                        udtSearchCriteria.DocumentNo2 = aryDocumentNo(0)
                    Else
                        udtSearchCriteria.DocumentNo1 = aryDocumentNo(0)
                        udtSearchCriteria.DocumentNo2 = String.Empty
                    End If

                    udtSearchCriteria.RawIdentityNum = Me.txtTabeHSAccountDocNo.Text.Trim
                    udtSearchCriteria.VoucherRecipientName = Me.txtTabeHSAccountName.Text.Trim
                    udtSearchCriteria.VoucherRecipientChiName = Me.txtTabeHSAccountChiName.Text.Trim


                Case Aspect.AdvancedSearch
                    EnumSelectedStoredProc = Aspect.AdvancedSearch

                    udtSearchCriteria.Aspect = Aspect.AdvancedSearch

                    'Group: Transaction
                    udtSearchCriteria.TransNum = IIf(Me.txtTabAdvancedSearchTransactionNo.Text.Trim = String.Empty, String.Empty, udtFormatter.formatSystemNumberReverse(Me.txtTabAdvancedSearchTransactionNo.Text.Trim))
                    udtSearchCriteria.HealthProf = Me.ddlTabAdvancedSearchHealthProfession.SelectedValue.Trim
                    'CRE20-003 (add search criteria) [Start][Martin]
                    udtSearchCriteria.SubsidizeItemCode = String.Empty
                    udtSearchCriteria.DoseCode = String.Empty
                    'CRE20-003 (add search criteria) [End][Martin]

                    'Group: Common
                    If rblTabAdvancedSearchTypeOfDate.SelectedValue = TypeOfDate.ServiceDate Then
                        udtSearchCriteria.ServiceDateFrom = IIf(Me.txtTabAdvancedSearchDateFrom.Text.Trim = String.Empty, String.Empty, udtFormatter.convertDate(Me.txtTabAdvancedSearchDateFrom.Text.Trim, String.Empty))
                        udtSearchCriteria.ServiceDateTo = IIf(Me.txtTabAdvancedSearchDateTo.Text.Trim = String.Empty, String.Empty, udtFormatter.convertDate(Me.txtTabAdvancedSearchDateTo.Text.Trim, String.Empty))
                    End If

                    If rblTabAdvancedSearchTypeOfDate.SelectedValue = TypeOfDate.TransactionDate Then
                        udtSearchCriteria.FromDate = IIf(Me.txtTabAdvancedSearchDateFrom.Text.Trim = String.Empty, String.Empty, udtFormatter.convertDate(Me.txtTabAdvancedSearchDateFrom.Text.Trim, String.Empty))
                        udtSearchCriteria.CutoffDate = IIf(Me.txtTabAdvancedSearchDateTo.Text.Trim = String.Empty, String.Empty, udtFormatter.convertDate(Me.txtTabAdvancedSearchDateTo.Text.Trim, String.Empty))
                    End If

                    udtSearchCriteria.SchemeCode = Me.ddlTabAdvancedSearchScheme.SelectedValue.Trim
                    udtSearchCriteria.TransStatus = Me.ddlTabAdvancedSearchTransactionStatus.SelectedValue.Trim
                    udtSearchCriteria.AuthorizedStatus = Me.ddlTabAdvancedSearchAuthorizedStatus.SelectedValue.Trim
                    udtSearchCriteria.Invalidation = Me.ddlTabAdvancedSearchInvalidationStatus.SelectedValue
                    udtSearchCriteria.ReimbursementMethod = Me.ddlTabAdvancedSearchReimbursementMethod.SelectedValue.Trim
                    udtSearchCriteria.MeansOfInput = Me.ddlTabAdvancedSearchMeansOfInput.SelectedValue.Trim
                    udtSearchCriteria.SchoolOrRCHCode = Me.txtTabAdvancedSearchRCHRode.Text.Trim  'CRE20-003 (add search criteria)[Martin]

                    'Group: SP
                    udtSearchCriteria.ServiceProviderID = Me.txtTabAdvancedSearchSPID.Text.Trim
                    udtSearchCriteria.ServiceProviderHKIC = Me.txtTabAdvancedSearchSPHKID.Text.Trim.ToUpper.Replace("(", String.Empty).Replace(")", String.Empty)
                    udtSearchCriteria.ServiceProviderName = Me.txtTabAdvancedSearchSPName.Text.Trim
                    udtSearchCriteria.ServiceProviderChiName = String.Empty
                    udtSearchCriteria.BankAcctNo = Me.txtTabAdvancedSearchBankAccountNo.Text.Trim

                    'Group: EHS Account
                    udtSearchCriteria.DocumentType = Me.ddlTabAdvancedSearchDocType.SelectedValue

                    If Not String.IsNullOrEmpty(Me.txtTabAdvancedSearchAccountID.Text) Then
                        udtSearchCriteria.VoucherAccID = Me.txtTabAdvancedSearchAccountID.Text.Substring(0, Me.txtTabAdvancedSearchAccountID.Text.Length - 1)
                    Else
                        udtSearchCriteria.VoucherAccID = ""
                    End If

                    Dim aryDocumentNo As String() = Me.txtTabAdvancedSearchDocNo.Text.Replace("(", "").Replace(")", "").Replace("-", "").Split("/")
                    If aryDocumentNo.Length > 1 Then
                        udtSearchCriteria.DocumentNo1 = aryDocumentNo(1)
                        udtSearchCriteria.DocumentNo2 = aryDocumentNo(0)
                    Else
                        udtSearchCriteria.DocumentNo1 = aryDocumentNo(0)
                        udtSearchCriteria.DocumentNo2 = String.Empty
                    End If

                    udtSearchCriteria.RawIdentityNum = Me.txtTabAdvancedSearchDocNo.Text.Trim
                    udtSearchCriteria.VoucherRecipientName = String.Empty
                    udtSearchCriteria.VoucherRecipientChiName = String.Empty

                    EnumSelectedStoredProc = SearchAspectRedirection()

                    'CRE16-026 (Add PCV13) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    'Update Aspect
                    udtSearchCriteria.Aspect = EnumSelectedStoredProc
                    'CRE16-026 (Add PCV13) [End][Chris YIM]


            End Select
            ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]

            Session(SESS_SearchCriteria) = udtSearchCriteria

            'CRE16-026 (Add PCV13) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'If criteria is collected, reload the Aspect from model in session
        Else

            If IsNothing(udtSearchCriteria.Aspect) Then
                EnumSelectedStoredProc = Aspect.AdvancedSearch
            Else
                EnumSelectedStoredProc = udtSearchCriteria.Aspect
            End If
            'CRE16-026 (Add PCV13) [End][Chris YIM]

        End If

        udtBLLSearchResult = udtReimbursementBLL.GetTransactionByAny(FunctionCode, udtSearchCriteria, udtHCVUUserBLL.GetHCVUUser.UserID.Trim, blnOverrideResultLimit, EnumSelectedStoredProc)

        ClearSearchCriteriaInput(Session(SESS.SelectedTabIndex))
        'CRE13-012 (RCH Code sorting) [End][Chris YIM]

        Return udtBLLSearchResult

    End Function

    Private Sub LoadTransactionGrid(ByVal dt As DataTable)
        GridViewDataBind(gvTransaction, dt, "transDate", "ASC", False)
        Session(SESS_TransactionDataTable) = dt

        ' ===== CRE10-027: Means of Input =====
        If (New GeneralFunction).CheckTurnOnMeansOfInput = GeneralFunction.EnumTurnOnStatus.Yes Then
            gvTransaction.Columns(14).Visible = True
        Else
            gvTransaction.Columns(14).Visible = False
        End If
        ' ===== End of CRE10-027 =====

        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Dim blnShowRMB As Boolean = False
        Dim udtSchemeClaimList As SchemeClaimModelCollection = Session(SESS_SchemeClaimListFilteredByUserRole)

        For Each udtSchemeClaim As SchemeClaimModel In udtSchemeClaimList
            If udtSchemeClaim.ReimbursementCurrency = SchemeClaimModel.EnumReimbursementCurrency.HKDRMB OrElse _
                udtSchemeClaim.ReimbursementCurrency = SchemeClaimModel.EnumReimbursementCurrency.RMB Then

                blnShowRMB = True
            End If
        Next

        If blnShowRMB Then
            gvTransaction.Columns(10).Visible = True 'TotalAmountRMB
        Else
            gvTransaction.Columns(10).Visible = False 'TotalAmountRMB
        End If
        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

    End Sub

    Private Sub BuildSearchCriteriaReview(ByVal udtSearchCriteria As SearchCriteria)

        Dim strServiceDate As String = String.Empty
        Dim strTransactionDate As String = String.Empty
        Dim strTypeOfDate As String = String.Empty
        Dim udtSubsidizeBLL As New SubsidizeBLL

        'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Select Case Session(SESS.SelectedTabIndex)
            Case Aspect.Transaction
                lblAspect.Text = GetGlobalResourceObject("Text", "Transaction")

            Case Aspect.ServiceProvider
                lblAspect.Text = GetGlobalResourceObject("Text", "ServiceProvider_gp")

            Case Aspect.eHSAccount
                lblAspect.Text = GetGlobalResourceObject("Text", "VoucherAccountMaintenance_gp")

            Case Aspect.AdvancedSearch
                lblAspect.Text = GetGlobalResourceObject("Text", "AdvancedSearch")

        End Select
        'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

        ' Service Provider ID
        lblRSPID.Text = FillAnyToEmptyString(udtSearchCriteria.ServiceProviderID)

        ' Service Provider HKIC No.
        lblRSPHKID.Text = FillAnyToEmptyString(udtSearchCriteria.ServiceProviderHKIC)

        ' Service Provider Name
        lblRSPName.Text = FillAnyToEmptyString(udtSearchCriteria.ServiceProviderName)

        ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
        ' Service Provider Chi Name
        lblRSPChiName.Text = FillAnyToEmptyString(udtSearchCriteria.ServiceProviderChiName)
        ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]

        ' Bank Account No.
        lblRBankAccountNo.Text = FillAnyToEmptyString(udtSearchCriteria.BankAcctNo)

        ' Transaction No.
        lblRTransactionNo.Text = IIf(udtSearchCriteria.TransNum = String.Empty, FillAnyToEmptyString(udtSearchCriteria.TransNum), udtFormatter.formatSystemNumber(udtSearchCriteria.TransNum))

        ' Health Profession
        If udtValidator.IsEmpty(udtSearchCriteria.HealthProf) Then
            lblRHealthProfession.Text = FillAnyToEmptyString(udtSearchCriteria.HealthProf)
        Else
            Status.GetDescriptionFromDBCode(ClaimTransStatus.ClassCode, udtSearchCriteria.TransStatus, lblRTransactionStatus.Text, String.Empty)
            Dim udtProfessionModelCollection As Profession.ProfessionModelCollection = udtSPProfileBLL.GetHealthProf.Filter(udtSearchCriteria.HealthProf)

            If udtProfessionModelCollection.Count = 1 Then
                Dim udtProfessionModel As Profession.ProfessionModel = udtProfessionModelCollection(0)
                lblRHealthProfession.Text = udtProfessionModel.ServiceCategoryDesc
            Else
                lblRHealthProfession.Text = FillAnyToEmptyString(udtSearchCriteria.HealthProf)
            End If
        End If

        ' Transaction Status
        If udtValidator.IsEmpty(udtSearchCriteria.TransStatus) Then
            lblRTransactionStatus.Text = FillAnyToEmptyString(udtSearchCriteria.TransStatus)
        Else
            Status.GetDescriptionFromDBCode(ClaimTransStatus.ClassCode, udtSearchCriteria.TransStatus, lblRTransactionStatus.Text, String.Empty)
        End If

        ' Reimbursement Status
        If udtValidator.IsEmpty(udtSearchCriteria.AuthorizedStatus) Then
            lblRAuthorizedStatus.Text = FillAnyToEmptyString(udtSearchCriteria.AuthorizedStatus)
        Else
            Status.GetDescriptionFromDBCode(ReimbursementStatus.ClassCode, udtSearchCriteria.AuthorizedStatus, lblRAuthorizedStatus.Text, String.Empty)
        End If

        ' Date From/To
        ' Service Date
        If udtSearchCriteria.ServiceDateFrom = String.Empty AndAlso udtSearchCriteria.ServiceDateTo = String.Empty Then
            strServiceDate = FillAnyToEmptyString(String.Empty)
        Else
            strServiceDate = String.Format("{0} {1} {2}", udtSearchCriteria.ServiceDateFrom, Me.GetGlobalResourceObject("Text", "To_S"), udtSearchCriteria.ServiceDateTo)
        End If

        ' Transaction Date
        If udtSearchCriteria.FromDate = String.Empty AndAlso udtSearchCriteria.CutoffDate = String.Empty Then
            strTransactionDate = FillAnyToEmptyString(String.Empty)
        Else
            strTransactionDate = String.Format("{0} {1} {2}", udtSearchCriteria.FromDate, Me.GetGlobalResourceObject("Text", "To_S"), udtSearchCriteria.CutoffDate)
        End If

        Select Case udtSearchCriteria.Aspect
            Case Aspect.Transaction
                strTypeOfDate = Me.rblTabTransactionTypeOfDate.SelectedValue
            Case Aspect.ServiceProvider
                strTypeOfDate = Me.rblTabServiceProviderTypeOfDate.SelectedValue
            Case Aspect.eHSAccount
                strTypeOfDate = Me.rblTabeHSAccountTypeOfDate.SelectedValue
            Case Aspect.AdvancedSearch
                strTypeOfDate = Me.rblTabAdvancedSearchTypeOfDate.SelectedValue
        End Select

        If strTypeOfDate = TypeOfDate.ServiceDate Then
            Me.lblRDateText.Text = Me.GetGlobalResourceObject("Text", "ServiceDate")
            Me.lblRDate.Text = strServiceDate
        End If

        If strTypeOfDate = TypeOfDate.TransactionDate Then
            Me.lblRDateText.Text = Me.GetGlobalResourceObject("Text", "TransactionDateVU")
            Me.lblRDate.Text = strTransactionDate
        End If

        ' eHealth Account Identity Document Type
        If udtValidator.IsEmpty(udtSearchCriteria.DocumentType) Then
            lblREHealthDocType.Text = FillAnyToEmptyString(udtSearchCriteria.DocumentType)
        Else
            Dim udtDocType As DocTypeModel = udtDocTypeBLL.getAllDocType().Filter(udtSearchCriteria.DocumentType)
            lblREHealthDocType.Text = udtDocType.DocName
        End If

        ' eHealth Account Identity Document No.
        Select Case udtSearchCriteria.Aspect
            Case Aspect.Transaction, Aspect.ServiceProvider
                lblREHealthDocNo.Text = FillAnyToEmptyString(String.Empty)
            Case Aspect.eHSAccount
                lblREHealthDocNo.Text = FillAnyToEmptyString(Me.txtTabeHSAccountDocNo.Text)
            Case Aspect.AdvancedSearch
                lblREHealthDocNo.Text = FillAnyToEmptyString(Me.txtTabAdvancedSearchDocNo.Text)
        End Select

        ' Scheme
        If udtSearchCriteria.SchemeCode <> String.Empty Then
            For Each udtSchemeC As SchemeClaimModel In CType(Session(SESS_SchemeClaimList), SchemeClaimModelCollection)
                If udtSchemeC.SchemeCode = udtSearchCriteria.SchemeCode Then
                    lblRSchemeCode.Text = udtSchemeC.DisplayCode
                    Exit For
                End If
            Next
        Else
            lblRSchemeCode.Text = FillAnyToEmptyString(udtSearchCriteria.SchemeCode)
        End If

        ' Invalidation Status
        If udtValidator.IsEmpty(udtSearchCriteria.Invalidation) Then
            lblRInvalidationStatus.Text = FillAnyToEmptyString(udtSearchCriteria.Invalidation)
        Else
            Status.GetDescriptionFromDBCode(EHSTransactionModel.InvalidationStatusClass.ClassCode, udtSearchCriteria.Invalidation, lblRInvalidationStatus.Text, String.Empty)
        End If

        ' Reimbursement Method
        If udtValidator.IsEmpty(udtSearchCriteria.ReimbursementMethod) Then
            lblRReimbursementMethod.Text = FillAnyToEmptyString(udtSearchCriteria.ReimbursementMethod)
        Else
            Dim udtStaticData As StaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("ReimbursementMethod", udtSearchCriteria.ReimbursementMethod)
            lblRReimbursementMethod.Text = udtStaticData.DataValue
        End If

        ' eHealth Account ID
        If udtSearchCriteria.VoucherAccID <> String.Empty Then
            lblREHealthAccountID.Text = udtFormatter.formatValidatedEHSAccountNumber(udtSearchCriteria.VoucherAccID)
        Else
            lblREHealthAccountID.Text = FillAnyToEmptyString(String.Empty)
        End If

        ' Means of Input
        If (New GeneralFunction).CheckTurnOnMeansOfInput = GeneralFunction.EnumTurnOnStatus.Yes Then
            lblRMeansOfInputText.Visible = True
            lblRMeansOfInput.Visible = True

            If udtSearchCriteria.MeansOfInput = String.Empty Then
                lblRMeansOfInput.Text = FillAnyToEmptyString(String.Empty)
            Else
                Status.GetDescriptionFromDBCode(EHSTransactionModel.MeansOfInputClass.ClassCode, udtSearchCriteria.MeansOfInput, lblRMeansOfInput.Text, String.Empty)
            End If

        Else
            lblRMeansOfInputText.Visible = False
            lblRMeansOfInput.Visible = False
        End If

        'CRE20-003 (add search criteria) [Start] [Martin]
        'RCH Code or School Code 
        lblRRCHCode.Text = FillAnyToEmptyString(udtSearchCriteria.SchoolOrRCHCode)

        If udtValidator.IsEmpty(udtSearchCriteria.SubsidizeItemCode) Then
            lblRVaccine.Text = FillAnyToEmptyString(udtSearchCriteria.SubsidizeItemCode)
        Else
            lblRVaccine.Text = udtSubsidizeBLL.GetSubsidizeItemDisplayCode(udtSearchCriteria.SubsidizeItemCode)
        End If

        If udtValidator.IsEmpty(udtSearchCriteria.DoseCode) Then
            lblRDose.Text = FillAnyToEmptyString(udtSearchCriteria.DoseCode)
        Else
            Dim udtStaticData As StaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("Dose", udtSearchCriteria.DoseCode)
            lblRDose.Text = udtStaticData.DataValue
        End If

        'CRE20-003 (add search criteria) [End] [Martin]


        ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
        ' Service Provider Name
        lblReHAName.Text = FillAnyToEmptyString(udtSearchCriteria.VoucherRecipientName)

        ' Service Provider Name
        lblReHAChiName.Text = FillAnyToEmptyString(udtSearchCriteria.VoucherRecipientChiName)
        ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]

    End Sub

    Private Function FillAnyToEmptyString(ByVal value As String) As String
        If IsNothing(value) OrElse value.Trim = String.Empty Then
            Return Me.GetGlobalResourceObject("Text", "Any")
        End If

        Return value
    End Function

    Private Sub AdjustButtonsBehaviour(ByVal strTransactionStatus As String, ByVal strAuthorizedStatus As String)
        ibtnBatchReactivate.Enabled = False
        ibtnBatchSuspend.Enabled = False
        ibtnBatchVoid.Enabled = False

        If strAuthorizedStatus = String.Empty OrElse strAuthorizedStatus = ReimbursementStatus.NotAuthorized Then
            Select Case strTransactionStatus
                Case String.Empty
                    ibtnBatchReactivate.Enabled = True
                    ibtnBatchSuspend.Enabled = True
                    ibtnBatchVoid.Enabled = True

                    ' CRE13-001 EHAPP [Start][Karl] - added status "joined"
                    ' -----------------------------------------------------------------------------------------
                Case ClaimTransStatus.Active, ClaimTransStatus.Pending, ClaimTransStatus.PendingVRValidate, ClaimTransStatus.Joined
                    ibtnBatchSuspend.Enabled = True

                Case ClaimTransStatus.Suspended
                    ibtnBatchReactivate.Enabled = True
                    ibtnBatchVoid.Enabled = True

            End Select

        End If

        ibtnBatchReactivate.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", IIf(ibtnBatchReactivate.Enabled, "MarkSelectedReactivateBtn", "MarkSelectedReactivateDisableBtn"))
        ibtnBatchSuspend.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", IIf(ibtnBatchSuspend.Enabled, "MarkSelectedSuspendBtn", "MarkSelectedSuspendDisabledBtn"))
        ibtnBatchVoid.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", IIf(ibtnBatchVoid.Enabled, "MarkSelectedVoidBtn", "MarkSelectedVoidDisabledBtn"))

    End Sub

    '

    Protected Sub gvTransaction_RowDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If (e.Row.RowType = DataControlRowType.Header) Then
            DirectCast(e.Row.FindControl("HeaderLevelCheckBox"), CheckBox).Attributes.Add("onclick", "javascript:SelectAll('" & _
            DirectCast(e.Row.FindControl("HeaderLevelCheckBox"), CheckBox).ClientID & "')")
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Transaction No.
            Dim lbtnTransactionNo As LinkButton = e.Row.FindControl("lbtn_transNum")
            lbtnTransactionNo.Text = udtFormatter.formatSystemNumber(lbtnTransactionNo.Text)

            ' Transaction Time
            Dim lblGTransactionTime As Label = e.Row.FindControl("lblGTransactionTime")
            lblGTransactionTime.Text = udtFormatter.formatDateTime(lblGTransactionTime.Text, String.Empty)

            ' Bank Account No.
            Dim lblMaskedBankAccountNo As Label = e.Row.FindControl("lblMaskedBank")
            Dim lblOriBankAccountNo As Label = e.Row.FindControl("lblOriBank")
            lblMaskedBankAccountNo.Text = udtFormatter.maskBankAccount(lblOriBankAccountNo.Text)

            ' Transaction Status
            Dim lblGTransactionStatus As Label = e.Row.FindControl("lblGTransactionStatus")
            Dim hfGTransactionStatus As HiddenField = e.Row.FindControl("hfGTransactionStatus")

            Status.GetDescriptionFromDBCode(ClaimTransStatus.ClassCode, hfGTransactionStatus.Value.Trim, lblGTransactionStatus.Text, String.Empty)

            ' Reimbursement Status
            Dim lblGAuthorisedStatus As Label = e.Row.FindControl("lblGAuthorisedStatus")
            Dim hfGAuthorizedStatus As HiddenField = e.Row.FindControl("hfGAuthorizedStatus")

            If hfGAuthorizedStatus.Value.Trim = String.Empty Then
                lblGAuthorisedStatus.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                Status.GetDescriptionFromDBCode(ReimbursementStatus.ClassCode, hfGAuthorizedStatus.Value.Trim, lblGAuthorisedStatus.Text, String.Empty)
            End If

            'Total Amount
            Dim strTotalAmount As String
            Dim lblTotalAmount As Label
            Dim dr As DataRow = CType(e.Row.DataItem, System.Data.DataRowView).Row

            lblTotalAmount = CType(e.Row.FindControl("lblTotalAmount"), Label)

            If IsDBNull(dr.Item("totalAmount")) = True Then
                strTotalAmount = Me.GetGlobalResourceObject("Text", "ServiceFeeN/A")
            Else
                strTotalAmount = CDbl(dr.Item("totalAmount")).ToString("#,##0")
            End If

            lblTotalAmount.Text = strTotalAmount

            ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            'Total Amount
            Dim strTotalAmountRMB As String = String.Empty
            Dim lblTotalAmountRMB As Label = CType(e.Row.FindControl("lblTotalAmountRMB"), Label)

            If IsDBNull(dr.Item("totalAmountRMB")) = True Then
                strTotalAmountRMB = Me.GetGlobalResourceObject("Text", "ServiceFeeN/A")
            Else
                strTotalAmountRMB = udtFormatter.formatMoneyRMB(dr.Item("totalAmountRMB").ToString, False)
            End If

            lblTotalAmountRMB.Text = strTotalAmountRMB
            ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

            ' Invalidation
            Dim lblGInvalidation As Label = e.Row.FindControl("lblGInvalidation")
            Dim hfGInvalidation As HiddenField = e.Row.FindControl("hfGInvalidation")

            If hfGInvalidation.Value.Trim = String.Empty Then
                lblGInvalidation.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                Status.GetDescriptionFromDBCode(EHSTransactionModel.InvalidationStatusClass.ClassCode, hfGInvalidation.Value.Trim, lblGInvalidation.Text, String.Empty)
            End If

            ' ===== CRE10-027: Means of Input =====

            ' Means of Input
            Dim lblGMeansOfInput As Label = e.Row.FindControl("lblGMeansOfInput")
            Dim hfGMeansOfInput As HiddenField = e.Row.FindControl("hfGMeansOfInput")

            Status.GetDescriptionFromDBCode(EHSTransactionModel.MeansOfInputClass.ClassCode, hfGMeansOfInput.Value.Trim, lblGMeansOfInput.Text, String.Empty)

            ' ===== End of CRE10-027 =====

            'CRE13-012 (RCH Code sorting) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim lblGRCHCode As Label = e.Row.FindControl("lblGRCHCode")
            Dim hfGRCHCode As HiddenField = e.Row.FindControl("hfGRCHCode")

            If hfGRCHCode.Value.Trim = String.Empty Then
                lblGRCHCode.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                lblGRCHCode.Text = hfGRCHCode.Value.Trim
            End If
            'CRE13-012 (RCH Code sorting) [End][Chris YIM]
        End If
    End Sub

    Protected Sub gvTransaction_RowCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        udcMessageBox.BuildMessageBox()
        udcInfoMessageBox.BuildMessageBox()

        Me.udtSessionHandlerBLL.EHSTransactionRemoveFromSession(FunctionCode)

        If e.CommandName.ToUpper <> "PAGE" AndAlso e.CommandName.ToUpper <> "SORT" Then
            Dim r As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)

            Dim strTransactionNo As String = CType(r.FindControl("hfTransactionNo"), HiddenField).Value.Trim
            Dim strRecordNum As String = CType(r.FindControl("lblRecordNum"), Label).Text.Trim

            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00004, AuditLogDescription.SelectTransaction)

            Try
                Dim udtEHSTransaction As EHSTransactionModel = BuildDetail(strTransactionNo)

                Dim dtTransaction As DataTable = Session(SESS_TransactionDataTable)

                lblCurrentRecordNo.Text = gvTransaction.PageIndex * gvTransaction.PageSize + r.RowIndex + 1
                lblMaxRecordNo.Text = dtTransaction.Rows.Count

                Dim aryCurrentRecordNo As New ArrayList
                aryCurrentRecordNo.Add(CInt(strRecordNum))
                SaveSelectedRecordDataTable(dtTransaction, aryCurrentRecordNo)

                AdjustDetailButtonBehaviour(udtEHSTransaction)

                MultiViewReimClaimTransManagement.ActiveViewIndex = ViewIndex.Detail

                ibtnDetailBack.Visible = True

                udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00005, AuditLogDescription.SelectTransactionSuccessful)

            Catch ex As Exception
                udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
                udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception: " + ex.Message)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00006, AuditLogDescription.SelectTransactionFail)

                Throw
            End Try

        End If
    End Sub

    Protected Sub gvTransaction_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        GridViewPageIndexChangingHandler(sender, e, SESS_TransactionDataTable)
    End Sub

    Protected Sub gvTransaction_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs)
        GridViewPreRenderHandler(sender, e, SESS_TransactionDataTable)
    End Sub

    Protected Sub gvTransaction_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        GridViewSortingHandler(sender, e, SESS_TransactionDataTable)
    End Sub

    Private Sub AdjustDetailButtonBehaviour(ByVal udtEHSTransaction As EHSTransactionModel)
        MultiViewDetailAction.ActiveViewIndex = ViewIndexDetailAction.Button

        Me.pnlTransactionCreatedByBoBtnAction.Visible = False
        Me.pnlNormalTransactionBtnAction.Visible = False

        If udtEHSTransaction.RecordStatus.Equals(EHSTransaction.EHSTransactionModel.TransRecordStatusClass.PendingApprovalForNonReimbursedClaim) Then
            Me.pnlTransactionCreatedByBoBtnAction.Visible = True

        Else
            Me.pnlNormalTransactionBtnAction.Visible = True

            ibtnReactivate.Enabled = False
            ibtnSuspend.Enabled = False
            ibtnVoid.Enabled = False
            ibtnCancelInvalidation.Enabled = False
            ibtnMarkInvalid.Enabled = False
            ibtnConfirmInvalid.Enabled = False

            Select Case udtEHSTransaction.RecordStatus
                Case ClaimTransStatus.Suspended
                    ibtnReactivate.Enabled = True
                    ibtnVoid.Enabled = True

                    ' CRE13-001 EHAPP [Start][Karl] - added status "joined"
                    ' -----------------------------------------------------------------------------------------
                    ' CRE11-024-02 HCVS Pilot Extension Part 2 [start]
                    ' - added "incomplete" status to the case next line
                Case ClaimTransStatus.Active, ClaimTransStatus.Pending, ClaimTransStatus.PendingVRValidate, ClaimTransStatus.Incomplete, ClaimTransStatus.Joined
                    ' CRE11-024-02 HCVS Pilot Extension Part 2 [end]
                    If udtEHSTransaction.AuthorisedStatus = String.Empty Then
                        ibtnSuspend.Enabled = True
                    End If

                Case ClaimTransStatus.Inactive
                    ' Nothing here

            End Select

            If udtEHSTransaction.RecordStatus = EHSTransactionModel.TransRecordStatusClass.Reimbursed _
                    AndAlso udtEHSTransaction.Invalidation = EHSTransactionModel.InvalidationStatusClass.NA Then
                ibtnMarkInvalid.Enabled = True
            End If

            If udtEHSTransaction.Invalidation = EHSTransactionModel.InvalidationStatusClass.PendingInvalidation Then
                ibtnCancelInvalidation.Enabled = True
                ibtnConfirmInvalid.Enabled = True
            End If

            ibtnReactivate.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", IIf(ibtnReactivate.Enabled, "ReactivateBtn", "ReactivateDisableBtn"))
            ibtnSuspend.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", IIf(ibtnSuspend.Enabled, "SuspendBtn", "SuspendDisableBtn"))
            ibtnVoid.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", IIf(ibtnVoid.Enabled, "VoidBtn", "VoidDisableBtn"))
            ibtnCancelInvalidation.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", IIf(ibtnCancelInvalidation.Enabled, "CancelInvalidationBtn", "CancelInvalidationDisableBtn"))
            ibtnMarkInvalid.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", IIf(ibtnMarkInvalid.Enabled, "MarkInvalidBtn", "MarkInvalidDisableBtn"))
            ibtnConfirmInvalid.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", IIf(ibtnConfirmInvalid.Enabled, "ConfirmInvalidBtn", "ConfirmInvalidDisableBtn"))

            ' CRE20-015-02 (Special Support Scheme) [Start][Winnie]
            ' ---------------------------------------------------------------------------------------------------------
            ' SSSCMC user: No right to invalid tx
            If udtHCVUUserBLL.IsSSSCMCUser(udtHCVUUserBLL.GetHCVUUser) Then
                ibtnCancelInvalidation.Visible = False
                ibtnMarkInvalid.Visible = False
                ibtnConfirmInvalid.Visible = False
            Else
                ibtnCancelInvalidation.Visible = True
                ibtnMarkInvalid.Visible = True
                ibtnConfirmInvalid.Visible = True
            End If
            ' CRE20-015-02 (Special Support Scheme) [End][Winnie]
        End If

    End Sub

    '

    Private Function BuildDetail(ByVal strTransactionNo As String) As EHSTransactionModel
        Dim udtEHSTransaction As EHSTransactionModel = udtEHSTransactionBLL.LoadClaimTran(strTransactionNo, True, True)
        Me.udtSessionHandlerBLL.EHSTransactionRemoveFromSession(FunctionCode)
        Me.udtSessionHandlerBLL.EHSTransactionSaveToSession(udtEHSTransaction, FunctionCode)

        Dim udtSearchCriteria As New SearchCriteria
        udtSearchCriteria.TransNum = strTransactionNo

        Dim dtSuspendHistory As DataTable = udtSearchEngineBLL.SearchSuspendHistory(udtSearchCriteria)

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        udcClaimTransDetail.ShowHKICSymbol = True
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]
        udcClaimTransDetail.LoadTranInfo(udtEHSTransaction, dtSuspendHistory)

        ' Save the current Transaction No to hidden field for the rebind in clicking action buttons (Reactivate, Suspend, Void)
        hfCurrentDetailTransactionNo.Value = strTransactionNo

        ' Previous Record, Next Record: Visible when have multiple rows
        If CType(Session(SESS_TransactionDataTable), DataTable).Rows.Count > 1 Then
            ibtnPreviousRecord.Visible = True
            panRecordNo.Visible = True
            ibtnNextRecord.Visible = True
        Else
            ibtnPreviousRecord.Visible = False
            panRecordNo.Visible = False
            ibtnNextRecord.Visible = False
        End If

        Return udtEHSTransaction

    End Function

    Private Function SaveSelectedRecordDataTable(ByVal dtTransaction As DataTable, ByVal aryCurrentRecordNo As ArrayList) As DataTable
        Dim dtSelected As New DataTable()
        dtSelected.Columns.Add(New DataColumn("lineNum", GetType(Integer)))
        dtSelected.Columns.Add(New DataColumn("transNum", GetType(String)))
        dtSelected.Columns.Add(New DataColumn("transDate", GetType(String)))
        dtSelected.Columns.Add(New DataColumn("transStatus", GetType(String)))
        dtSelected.Columns.Add(New DataColumn("voidRef", GetType(String)))
        dtSelected.Columns.Add(New DataColumn("void_dtm", GetType(String)))
        dtSelected.Columns.Add(New DataColumn("tsmp", GetType(Byte())))
        dtSelected.Columns.Add(New DataColumn("Invalidation_TSMP", GetType(Byte())))

        For Each intCurrentRecordNo As Integer In aryCurrentRecordNo
            Dim drTransaction As DataRow = dtTransaction.Rows(intCurrentRecordNo - 1)

            Dim drSelected As DataRow = dtSelected.NewRow
            drSelected("lineNum") = intCurrentRecordNo
            drSelected("transNum") = drTransaction("transNum").ToString.Trim
            drSelected("transDate") = drTransaction("transDate").ToString.Trim
            drSelected("transStatus") = drTransaction("transStatus").ToString.Trim
            drSelected("voidRef") = String.Empty
            drSelected("void_dtm") = String.Empty
            drSelected("tsmp") = drTransaction("tsmp")
            drSelected("Invalidation_TSMP") = drTransaction("Invalidation_TSMP")
            dtSelected.Rows.Add(drSelected)
        Next

        Session(SESS_SelectedDataTable) = dtSelected

        Return dtSelected

    End Function

    '

    Protected Sub ibtnBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcInfoMessageBox.Visible = False
        udcMessageBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00047, AuditLogDescription.TransactionGridBackClick)

        Session(SESS_SearchCriteria) = Nothing

        MultiViewReimClaimTransManagement.ActiveViewIndex = ViewIndex.InputCriteria

    End Sub

    Protected Sub ibtnBatchReactivate_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Visible = False
        udcInfoMessageBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00013, AuditLogDescription.BatchReactivateClick)

        If Not CheckGvTransactionAtLeastOneRowSelected() Then
            udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00023)
            udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationFail, udtAuditLogEntry, LogID.LOG00015, AuditLogDescription.BatchReactivateFail)

        Else
            Dim aryTargetStatus As New ArrayList
            aryTargetStatus.Add(ClaimTransStatus.Suspended)

            If CheckGvTransactionRowsWithinStatus(aryTargetStatus) Then
                Dim dtTransaction As DataTable = Session(SESS_TransactionDataTable)
                Dim aryCurrentRecordNo As New ArrayList

                For Each r As GridViewRow In gvTransaction.Rows
                    If Not CType(r.FindControl("chk_selected"), CheckBox).Checked Then Continue For

                    Dim strTransactionNo As String = CType(r.FindControl("hfTransactionNo"), HiddenField).Value.Trim
                    Dim strLineNum As String = CType(r.FindControl("lblRecordNum"), Label).Text.Trim

                    aryCurrentRecordNo.Add(CInt(strLineNum))

                    udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
                Next

                Dim dtSelected As DataTable = SaveSelectedRecordDataTable(dtTransaction, aryCurrentRecordNo)

                Try
                    udtReimbursementBLL.UpdateVoucherTransactionStatus(dtTransaction, dtSelected, ClaimTransStatus.Active, udtHCVUUserBLL.GetHCVUUser.UserID.Trim, String.Empty)

                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003)
                    udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                    udcInfoMessageBox.BuildMessageBox()

                    panVoidResult.Visible = False
                    MultiViewReimClaimTransManagement.ActiveViewIndex = ViewIndex.Complete

                    udtAuditLogEntry.WriteEndLog(LogID.LOG00014, AuditLogDescription.BatchReactivateSuccessful)

                Catch eSQL As SqlClient.SqlException
                    If eSQL.Number = 50000 Then
                        udcMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, eSQL.Message))
                        udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.UpdateFail, udtAuditLogEntry, LogID.LOG00015, AuditLogDescription.BatchReactivateFail)

                    Else
                        udtAuditLogEntry.AddDescripton("StackTrace", "Unknown SqlException: " + eSQL.Message)
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00015, AuditLogDescription.BatchReactivateFail)
                        Throw eSQL
                    End If

                Catch ex As Exception
                    udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception: " + ex.Message)
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00015, AuditLogDescription.BatchReactivateFail)
                    Throw ex
                End Try

            Else
                For Each r As GridViewRow In gvTransaction.Rows
                    If Not CType(r.FindControl("chk_selected"), CheckBox).Checked Then Continue For
                    udtAuditLogEntry.AddDescripton("Transaction No", CType(r.FindControl("hfTransactionNo"), HiddenField).Value.Trim)
                Next

                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00003)
                udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationFail, udtAuditLogEntry, LogID.LOG00015, AuditLogDescription.BatchReactivateFail)
            End If
        End If
    End Sub

    Protected Sub ibtnBatchSuspend_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Visible = False
        udcInfoMessageBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00016, AuditLogDescription.BatchSuspendClick)

        If Not CheckGvTransactionAtLeastOneRowSelected() Then
            udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00023)
            udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationFail, udtAuditLogEntry, LogID.LOG00018, AuditLogDescription.BatchSuspendFail)

        Else
            Dim aryTargetStatus As New ArrayList
            aryTargetStatus.Add(ClaimTransStatus.Active)
            aryTargetStatus.Add(ClaimTransStatus.Pending)
            aryTargetStatus.Add(ClaimTransStatus.PendingVRValidate)
            ' CRE13-001 EHAPP [Start][Karl]
            ' -----------------------------------------------------------------------------------------
            aryTargetStatus.Add(ClaimTransStatus.Joined)
            ' CRE13-001 EHAPP [End][Karl]

            If CheckGvTransactionRowsWithinStatus(aryTargetStatus) Then
                Dim dtTransaction As DataTable = Session(SESS_TransactionDataTable)
                Dim aryCurrentRecordNo As New ArrayList

                For Each r As GridViewRow In gvTransaction.Rows
                    If Not CType(r.FindControl("chk_selected"), CheckBox).Checked Then Continue For

                    Dim strTransactionNo As String = CType(r.FindControl("hfTransactionNo"), HiddenField).Value.Trim
                    Dim strLineNum As String = CType(r.FindControl("lblRecordNum"), Label).Text.Trim

                    aryCurrentRecordNo.Add(CInt(strLineNum))

                    udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
                Next

                Dim dtSelected As DataTable = SaveSelectedRecordDataTable(dtTransaction, aryCurrentRecordNo)

                gvBatchActionConfirm.DataSource = dtSelected
                gvBatchActionConfirm.DataBind()

                lblBatchActionHeading.Text = Me.GetGlobalResourceObject("Text", "SuspendTransaction")
                lblBatchReasonText.Text = Me.GetGlobalResourceObject("Text", "SuspendReason")
                lblConfirmBatchReasonText.Text = lblBatchReasonText.Text

                HiddenFieldAction.Value = ClaimTransStatus.Suspended
                txtBatchReason.Text = String.Empty
                lblConfirmBatchReason.Text = txtBatchReason.Text
                imgAlertBatchReason.Visible = False

                MultiViewReimClaimTransManagement.ActiveViewIndex = ViewIndex.BatchActionConfirm
                MultiViewBatchAction.ActiveViewIndex = ViewIndexBatchAction.BatchInputReason

                udtAuditLogEntry.WriteEndLog(LogID.LOG00017, AuditLogDescription.BatchSuspendSuccessful)

            Else
                For Each r As GridViewRow In gvTransaction.Rows
                    If Not CType(r.FindControl("chk_selected"), CheckBox).Checked Then Continue For
                    udtAuditLogEntry.AddDescripton("Transaction No", CType(r.FindControl("hfTransactionNo"), HiddenField).Value.Trim)
                Next

                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00004)
                udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationFail, udtAuditLogEntry, LogID.LOG00018, AuditLogDescription.BatchSuspendFail)

            End If

        End If
    End Sub

    Protected Sub ibtnBatchVoid_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Visible = False
        udcInfoMessageBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        ' CRE11-004
        AuditLogAddDescriptonTranNo(udtAuditLogEntry, gvTransaction)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00025, AuditLogDescription.BatchVoidClick)

        If Not CheckGvTransactionAtLeastOneRowSelected() Then
            udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00023)

            ' CRE11-004
            AuditLogAddDescriptonTranNo(udtAuditLogEntry, gvTransaction)
            udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationFail, udtAuditLogEntry, LogID.LOG00027, AuditLogDescription.BatchVoidFail)

        Else
            Dim aryTargetStatus As New ArrayList
            aryTargetStatus.Add(ClaimTransStatus.Suspended)

            If CheckGvTransactionRowsWithinStatus(aryTargetStatus) Then
                Dim dtTransaction As DataTable = Session(SESS_TransactionDataTable)
                Dim aryCurrentRecordNo As New ArrayList

                For Each r As GridViewRow In gvTransaction.Rows
                    If Not CType(r.FindControl("chk_selected"), CheckBox).Checked Then Continue For

                    Dim strTransactionNo As String = CType(r.FindControl("hfTransactionNo"), HiddenField).Value.Trim
                    Dim strLineNum As String = CType(r.FindControl("lblRecordNum"), Label).Text.Trim

                    aryCurrentRecordNo.Add(CInt(strLineNum))

                    udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
                Next

                Dim dtSelected As DataTable = SaveSelectedRecordDataTable(dtTransaction, aryCurrentRecordNo)

                gvBatchActionConfirm.DataSource = dtSelected
                gvBatchActionConfirm.DataBind()

                lblBatchActionHeading.Text = Me.GetGlobalResourceObject("Text", "VoidTransaction")
                lblBatchReasonText.Text = Me.GetGlobalResourceObject("Text", "VoidReason")
                lblConfirmBatchReasonText.Text = lblBatchReasonText.Text

                txtBatchReason.Text = String.Empty
                imgAlertBatchReason.Visible = False

                HiddenFieldAction.Value = ClaimTransStatus.Inactive

                MultiViewReimClaimTransManagement.ActiveViewIndex = ViewIndex.BatchActionConfirm
                MultiViewBatchAction.ActiveViewIndex = ViewIndexBatchAction.BatchInputReason

                udtAuditLogEntry.WriteEndLog(LogID.LOG00026, AuditLogDescription.BatchVoidSuccessful)

            Else
                For Each r As GridViewRow In gvTransaction.Rows
                    If Not CType(r.FindControl("chk_selected"), CheckBox).Checked Then Continue For
                    udtAuditLogEntry.AddDescripton("Transaction No", CType(r.FindControl("hfTransactionNo"), HiddenField).Value.Trim)
                Next

                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00005)
                udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationFail, udtAuditLogEntry, LogID.LOG00027, AuditLogDescription.BatchVoidFail)
            End If
        End If
    End Sub


    Private Function CheckGvTransactionAtLeastOneRowSelected() As Boolean
        ' CRE11-007
        ' Show warning image beside transaction no.
        Dim blnResult As Boolean = False
        For Each r As GridViewRow In gvTransaction.Rows
            Dim imgWarning As Image = r.FindControl("imgWarning")
            imgWarning.Visible = False

            If CType(r.FindControl("chk_selected"), CheckBox).Checked Then
                blnResult = True
            End If

        Next

        Return blnResult

    End Function

    Private Function CheckGvTransactionRowsWithinStatus(ByVal aryTargetStatus As ArrayList) As Boolean
        ' CRE11-007
        ' Show warning image beside transaction no.
        Dim blnResult As Boolean = True
        For Each r As GridViewRow In Me.gvTransaction.Rows
            Dim imgWarning As Image = r.FindControl("imgWarning")
            imgWarning.Visible = False

            If CType(r.FindControl("chk_selected"), CheckBox).Checked Then
                Dim hfGAuthorizedStatus As HiddenField = r.FindControl("hfGAuthorizedStatus")

                If hfGAuthorizedStatus.Value.Trim <> String.Empty Then
                    blnResult = False
                    imgWarning.Visible = True
                End If

                Dim hfGTransactionStatus As HiddenField = r.FindControl("hfGTransactionStatus")
                If Not aryTargetStatus.Contains(hfGTransactionStatus.Value.Trim) Then
                    blnResult = False
                    imgWarning.Visible = True
                End If
            End If
        Next

        Return blnResult

    End Function

    '

    Protected Sub gvBatchActionConfirm_RowDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Transaction No.
            Dim lblBTransactionNo As Label = e.Row.FindControl("lblBTransactionNo")
            Dim hfBTransactionNo As HiddenField = e.Row.FindControl("hfBTransactionNo")
            lblBTransactionNo.Text = udtFormatter.formatSystemNumber(hfBTransactionNo.Value)

            ' Transaction Time
            Dim lblBTransactionDate As Label = e.Row.FindControl("lblBTransactionDate")
            lblBTransactionDate.Text = udtFormatter.formatDateTime(lblBTransactionDate.Text.Trim, String.Empty)

        End If
    End Sub

    ' Batch action

    Protected Sub ibtnBatchInputReasonCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim dtSelected As DataTable = Session(SESS_SelectedDataTable)
        ' CRE11-004
        AuditLogAddDescriptonTranNo(udtAuditLogEntry, dtSelected)

        Select Case HiddenFieldAction.Value
            Case ClaimTransStatus.Suspended
                udtAuditLogEntry.WriteLog(LogID.LOG00057, AuditLogDescription.SaveBatchSuspendReasonCancelClick)
            Case ClaimTransStatus.Inactive
                udtAuditLogEntry.WriteLog(LogID.LOG00055, AuditLogDescription.SaveBatchVoidReasonCancelClick)
        End Select

        MultiViewReimClaimTransManagement.ActiveViewIndex = ViewIndex.Transaction
    End Sub

    Protected Sub ibtnBatchInputReasonSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Visible = False
        udcInfoMessageBox.Visible = False

        imgAlertBatchReason.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        Dim dtSelected As DataTable = Session(SESS_SelectedDataTable)
        ' CRE11-004
        AuditLogAddDescriptonTranNo(udtAuditLogEntry, dtSelected)
        AuditLogAddDescriptonBatchReason(udtAuditLogEntry)

        Select Case HiddenFieldAction.Value
            Case ClaimTransStatus.Suspended
                udtAuditLogEntry.WriteStartLog(LogID.LOG00019, AuditLogDescription.SaveBatchSuspendReasonClick)
            Case ClaimTransStatus.Inactive
                udtAuditLogEntry.WriteStartLog(LogID.LOG00028, AuditLogDescription.SaveBatchVoidReasonClick)
        End Select

        If udtValidator.IsEmpty(txtBatchReason.Text.Trim) Then
            imgAlertBatchReason.Visible = True

            ' CRE11-004
            AuditLogAddDescriptonTranNo(udtAuditLogEntry, dtSelected)
            AuditLogAddDescriptonBatchReason(udtAuditLogEntry)

            Select Case HiddenFieldAction.Value
                Case ClaimTransStatus.Suspended
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001)
                    udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationFail, udtAuditLogEntry, LogID.LOG00021, AuditLogDescription.SaveBatchSuspendReasonFail)

                Case ClaimTransStatus.Inactive
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002)
                    udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationFail, udtAuditLogEntry, LogID.LOG00030, AuditLogDescription.SaveBatchVoidReasonFail)

            End Select

        Else
            lblConfirmBatchReason.Text = txtBatchReason.Text.Trim

            udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00021)
            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
            udcInfoMessageBox.BuildMessageBox()

            MultiViewBatchAction.ActiveViewIndex = ViewIndexBatchAction.BatchConfirmReason

            ' CRE11-004
            AuditLogAddDescriptonTranNo(udtAuditLogEntry, dtSelected)
            AuditLogAddDescriptonBatchReason(udtAuditLogEntry)

            Select Case HiddenFieldAction.Value
                Case ClaimTransStatus.Suspended
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00020, AuditLogDescription.SaveBatchSuspendReasonSuccessful)

                Case ClaimTransStatus.Inactive
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00029, AuditLogDescription.SaveBatchVoidReasonSuccessful)

            End Select

        End If
    End Sub

    Protected Sub ibtnBatchConfirmReasonCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcInfoMessageBox.Visible = False
        udcMessageBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim dtSelected As DataTable = Session(SESS_SelectedDataTable)
        ' CRE11-004
        AuditLogAddDescriptonTranNo(udtAuditLogEntry, dtSelected)

        Select Case HiddenFieldAction.Value
            Case ClaimTransStatus.Suspended
                udtAuditLogEntry.WriteLog(LogID.LOG00058, AuditLogDescription.ConfirmBatchSuspendReasonCancelClick)
            Case ClaimTransStatus.Inactive
                udtAuditLogEntry.WriteLog(LogID.LOG00056, AuditLogDescription.ConfirmBatchVoidReasonCancelClick)
        End Select

        MultiViewBatchAction.ActiveViewIndex = ViewIndexBatchAction.BatchInputReason
    End Sub

    Protected Sub ibtnBatchConfirmReasonConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Visible = False
        udcInfoMessageBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        Dim dtSelected As DataTable = Session(SESS_SelectedDataTable)
        ' CRE11-004
        AuditLogAddDescriptonTranNo(udtAuditLogEntry, dtSelected)
        AuditLogAddDescriptonConfirmBatchReason(udtAuditLogEntry)

        Try
            Dim dtTransaction As DataTable = Session(SESS_TransactionDataTable)
            Dim strUserID As String = udtHCVUUserBLL.GetHCVUUser.UserID.Trim

            Select Case HiddenFieldAction.Value
                Case ClaimTransStatus.Suspended
                    udtAuditLogEntry.WriteStartLog(LogID.LOG00022, AuditLogDescription.ConfirmBatchSuspendReasonClick)

                    udtReimbursementBLL.UpdateVoucherTransactionStatus(dtTransaction, dtSelected, ClaimTransStatus.Suspended, strUserID, lblConfirmBatchReason.Text)
                    Session(SESS_SelectedDataTable) = Nothing

                    panVoidResult.Visible = False

                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001)

                Case ClaimTransStatus.Inactive
                    udtAuditLogEntry.WriteStartLog(LogID.LOG00031, AuditLogDescription.ConfirmBatchVoidReasonClick)

                    udtReimbursementBLL.VoidVoucherTransaction(dtTransaction, dtSelected, strUserID, lblConfirmBatchReason.Text)

                    gvVoidResult.DataSource = dtSelected
                    gvVoidResult.DataBind()

                    panVoidResult.Visible = True

                    Session(SESS_SelectedDataTable) = Nothing

                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00002)

            End Select

            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            udcInfoMessageBox.BuildMessageBox()

            MultiViewReimClaimTransManagement.ActiveViewIndex = ViewIndex.Complete

            ' CRE11-004
            AuditLogAddDescriptonTranNo(udtAuditLogEntry, dtSelected)
            AuditLogAddDescriptonConfirmBatchReason(udtAuditLogEntry)

            Select Case HiddenFieldAction.Value
                Case ClaimTransStatus.Suspended
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00023, AuditLogDescription.ConfirmBatchSuspendReasonSuccessful)
                Case ClaimTransStatus.Inactive
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00032, AuditLogDescription.ConfirmBatchVoidReasonSuccessful)
            End Select

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                udcMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, eSQL.Message))

                ' CRE11-004
                AuditLogAddDescriptonTranNo(udtAuditLogEntry, dtSelected)
                AuditLogAddDescriptonConfirmBatchReason(udtAuditLogEntry)

                Select Case HiddenFieldAction.Value
                    Case ClaimTransStatus.Suspended
                        udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.UpdateFail, udtAuditLogEntry, LogID.LOG00024, AuditLogDescription.ConfirmBatchSuspendReasonFail)
                    Case ClaimTransStatus.Inactive
                        udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.UpdateFail, udtAuditLogEntry, LogID.LOG00033, AuditLogDescription.ConfirmBatchVoidReasonFail)
                End Select

            Else
                ' CRE11-004
                AuditLogAddDescriptonTranNo(udtAuditLogEntry, dtSelected)
                AuditLogAddDescriptonConfirmBatchReason(udtAuditLogEntry)

                udtAuditLogEntry.AddDescripton("StackTrace", "Unknown SqlException: " + eSQL.Message)

                Select Case HiddenFieldAction.Value
                    Case ClaimTransStatus.Suspended
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00024, AuditLogDescription.ConfirmBatchSuspendReasonFail)
                    Case ClaimTransStatus.Inactive
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00033, AuditLogDescription.ConfirmBatchVoidReasonFail)
                End Select

                Throw eSQL
            End If

        Catch ex As Exception
            ' CRE11-004
            AuditLogAddDescriptonTranNo(udtAuditLogEntry, dtSelected)
            AuditLogAddDescriptonConfirmBatchReason(udtAuditLogEntry)

            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception: " + ex.Message)

            Select Case HiddenFieldAction.Value
                Case ClaimTransStatus.Suspended
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00024, AuditLogDescription.ConfirmBatchSuspendReasonFail)
                Case ClaimTransStatus.Inactive
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00033, AuditLogDescription.ConfirmBatchVoidReasonFail)
            End Select

            Throw ex
        End Try

    End Sub

    '

    Protected Sub ibtnReturn_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        udcInfoMessageBox.Visible = False
        udcMessageBox.Visible = False

        Me.ClearSessionForNewClaim(True)

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00049, AuditLogDescription.ReturnClick)

        Try
            Dim udtSearchCriteria As SearchCriteria = Session(SESS_SearchCriteria)

            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
            ' -------------------------------------------------------------------------
            Dim udtBLLSearchResult As BaseBLL.BLLSearchResult
            Dim dtTransaction As DataTable = Nothing

            'Dim dtTransaction As DataTable = GetTransaction(udtSearchCriteria)
            udtBLLSearchResult = GetTransaction(udtSearchCriteria, True)

            Select Case udtBLLSearchResult.SqlErrorMessage
                Case BaseBLL.EnumSqlErrorMessage.Normal
                    dtTransaction = CType(udtBLLSearchResult.Data, DataTable)

                Case BaseBLL.EnumSqlErrorMessage.OverResultListOverrideLimit
                    udcMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, MsgCode.MSG00017))
                    udcMessageBox.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00003, "Search Fail")

                    MultiViewReimClaimTransManagement.ActiveViewIndex = ViewIndex.InputCriteria
                    Return

                Case Else
                    Throw New Exception("Error: Class = [HCVU.claimTransManagement], Method = [ibtnReturn_Click], Message = The unexpected error for the result of re-search")

            End Select

            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

            If dtTransaction.Rows.Count = 0 Then
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
                udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
                udcInfoMessageBox.BuildMessageBox()

                udtAuditLogEntry.AddDescripton("StackTrace", "dtTransaction.Rows.Count = 0: No rows can be obtained with the search criteria")
                udtAuditLogEntry.WriteEndLog(LogID.LOG00046, AuditLogDescription.SearchCompleteNoRecordFound)

                MultiViewReimClaimTransManagement.ActiveViewIndex = ViewIndex.InputCriteria

            Else
                LoadTransactionGrid(dtTransaction)
                MultiViewReimClaimTransManagement.ActiveViewIndex = ViewIndex.Transaction

            End If

        Catch eSQL As SqlClient.SqlException
            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
            ' -------------------------------------------------------------------------
            'If eSQL.Number = 50000 Then
            'udcMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, eSQL.Message))
            'udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.SearchFail)
            'Else
            Throw eSQL
            'End If
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    '

    Protected Sub ibtnDetailBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcClaimTransDetail.ClearDocumentType()

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00048, AuditLogDescription.DetailBackClick)

        MultiViewReimClaimTransManagement.ActiveViewIndex = ViewIndex.Transaction
    End Sub

    Protected Sub ibtnNextRecord_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        If CInt(lblCurrentRecordNo.Text) < CInt(lblMaxRecordNo.Text) Then
            Dim ee As New System.Web.UI.WebControls.GridViewPageEventArgs(udtReimbursementBLL.GetPageIndexInRecordNavigation(gvTransaction.PageSize, CInt(lblCurrentRecordNo.Text) + 1))

            GridViewPageIndexChangingHandler(gvTransaction, ee, SESS_TransactionDataTable)

            Dim intActualIndex As Integer = CInt(CType(gvTransaction.Rows( _
                CInt(lblCurrentRecordNo.Text) - gvTransaction.PageSize * gvTransaction.PageIndex).Cells(0).FindControl("lblRecordNum"), Label).Text.Trim) - 1

            Dim dtTransaction As DataTable = Session(SESS_TransactionDataTable)
            Dim strTransactionNo As String = udtFormatter.formatSystemNumberReverse(dtTransaction.Rows(intActualIndex)(2).ToString)

            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00010, AuditLogDescription.NextRecordClick)

            Try
                Dim udtEHSTransaction As EHSTransactionModel = BuildDetail(strTransactionNo)

                lblCurrentRecordNo.Text = CInt(lblCurrentRecordNo.Text) + 1

                Dim aryCurrentRecordNo As New ArrayList
                aryCurrentRecordNo.Add(CInt(lblCurrentRecordNo.Text))
                SaveSelectedRecordDataTable(dtTransaction, aryCurrentRecordNo)

                AdjustDetailButtonBehaviour(udtEHSTransaction)

                udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00011, AuditLogDescription.NextRecordSuccessful)

            Catch ex As Exception
                udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
                udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception: " + ex.Message)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00012, AuditLogDescription.NextRecordFail)
                Throw
            End Try

        End If
    End Sub

    Protected Sub ibtnPreviousRecord_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        If CInt(lblCurrentRecordNo.Text) > 1 Then
            Dim ee As New System.Web.UI.WebControls.GridViewPageEventArgs(udtReimbursementBLL.GetPageIndexInRecordNavigation(gvTransaction.PageSize, CInt(lblCurrentRecordNo.Text) - 1))

            GridViewPageIndexChangingHandler(gvTransaction, ee, SESS_TransactionDataTable)

            Dim intActualIndex As Integer = CInt(CType(gvTransaction.Rows( _
                CInt(lblCurrentRecordNo.Text) - (gvTransaction.PageSize * gvTransaction.PageIndex) - 2).Cells(0).FindControl("lblRecordNum"), Label).Text.Trim) - 1

            Dim dtTransaction As DataTable = Session(SESS_TransactionDataTable)
            Dim strTransactionNo As String = udtFormatter.formatSystemNumberReverse(dtTransaction.Rows(intActualIndex)(2).ToString)

            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00007, AuditLogDescription.PreviousRecordClick)

            Try
                Dim udtEHSTransaction As EHSTransactionModel = BuildDetail(strTransactionNo)

                lblCurrentRecordNo.Text = CInt(lblCurrentRecordNo.Text) - 1

                Dim aryCurrentRecordNo As New ArrayList
                aryCurrentRecordNo.Add(CInt(lblCurrentRecordNo.Text))
                SaveSelectedRecordDataTable(dtTransaction, aryCurrentRecordNo)

                AdjustDetailButtonBehaviour(udtEHSTransaction)

                udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00008, AuditLogDescription.PreviousRecordSuccessful)

            Catch ex As Exception
                udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
                udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception: " + ex.Message)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00009, AuditLogDescription.PreviousRecordFail)
                Throw
            End Try
        End If
    End Sub

    Protected Sub ibtnReactivate_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim dtSelected As DataTable = Session(SESS_SelectedDataTable)
        ' CRE11-004
        AuditLogAddDescriptonTranNo(udtAuditLogEntry, dtSelected)

        udtAuditLogEntry.WriteStartLog(LogID.LOG00050, AuditLogDescription.ReactivateClick)

        Try
            Dim strUserID As String = udtHCVUUserBLL.GetHCVUUser.UserID.Trim
            Dim dtTransaction As DataTable = Session(SESS_TransactionDataTable)

            udtReimbursementBLL.UpdateVoucherTransactionStatus(dtTransaction, dtSelected, ClaimTransStatus.Active, strUserID, String.Empty)

            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003)
            udcInfoMessageBox.BuildMessageBox()

            panVoidResult.Visible = False

            ' CRE11-004
            AuditLogAddDescriptonTranNo(udtAuditLogEntry, dtSelected)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00051, AuditLogDescription.ReactivateSuccessful)

            MultiViewReimClaimTransManagement.ActiveViewIndex = ViewIndex.Complete

        Catch eSQL As SqlClient.SqlException

            ' CRE11-004
            AuditLogAddDescriptonTranNo(udtAuditLogEntry, dtSelected)

            If eSQL.Number = 50000 Then
                udcMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, eSQL.Message))
                udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.UpdateFail, udtAuditLogEntry, LogID.LOG00052, AuditLogDescription.ReactivateFail)

                MultiViewReimClaimTransManagement.ActiveViewIndex = ViewIndex.Complete
            Else
                udtAuditLogEntry.AddDescripton("StackTrace", "Unknown SqlException: " + eSQL.Message)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00052, AuditLogDescription.ReactivateFail)

                Throw eSQL
            End If

        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception: " + ex.Message)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00052, AuditLogDescription.ReactivateFail)

            Throw ex
        End Try

    End Sub

    Protected Sub ibtnSuspend_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim dtSelected As DataTable = Session(SESS_SelectedDataTable)
        ' CRE11-004
        AuditLogAddDescriptonTranNo(udtAuditLogEntry, dtSelected)

        udtAuditLogEntry.WriteLog(LogID.LOG00053, AuditLogDescription.SuspendClick)

        HiddenFieldAction.Value = ClaimTransStatus.Suspended
        imgAlertReason.Visible = False

        MultiViewDetailAction.ActiveViewIndex = ViewIndexDetailAction.InputReason

        lblReasonText.Text = Me.GetGlobalResourceObject("Text", "SuspendReason")
        lblConfirmReasonText.Text = lblReasonText.Text

        txtReason.Text = String.Empty
    End Sub

    Protected Sub ibtnVoid_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim dtSelected As DataTable = Session(SESS_SelectedDataTable)
        ' CRE11-004
        AuditLogAddDescriptonTranNo(udtAuditLogEntry, dtSelected)

        udtAuditLogEntry.WriteStartLog(LogID.LOG00054, AuditLogDescription.VoidClick)

        HiddenFieldAction.Value = ClaimTransStatus.Inactive
        imgAlertReason.Visible = False

        MultiViewDetailAction.ActiveViewIndex = ViewIndexDetailAction.InputReason

        lblReasonText.Text = Me.GetGlobalResourceObject("Text", "VoidReason")
        lblConfirmReasonText.Text = lblReasonText.Text

        txtReason.Text = String.Empty
    End Sub

    Protected Sub ibtnCancelInvalidation_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        lblPopupInvalidationText.Text = Me.GetGlobalResourceObject("Text", "ConfirmCancelInvalidation")

        popupInvalidation.Show()

        HiddenFieldAction.Value = SingleAction.CancelInvalidation

    End Sub

    Protected Sub ibtnMarkInvalid_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00107, AuditLogDescription.MarkInvalidClick)
        ' CRE11-021 log the missed essential information [End]

        imgAlertReasonRemark.Visible = False

        ' Bind the Invalidation Type for the first time
        If ddlInvalidationReason.Items.Count = 1 Then
            ddlInvalidationReason.DataSource = udtStaticDataBLL.GetStaticDataListByColumnName("TransactionInvalidationType")
            ddlInvalidationReason.DataValueField = "ItemNo"
            ddlInvalidationReason.DataTextField = "DataValue"
            ddlInvalidationReason.DataBind()
        End If

        MultiViewDetailAction.ActiveViewIndex = ViewIndexDetailAction.InputInvalidationReason

        ddlInvalidationReason.SelectedIndex = 0
        txtReasonRemark.Text = String.Empty
    End Sub

    Protected Sub ibtnConfirmInvalid_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        lblPopupInvalidationText.Text = Me.GetGlobalResourceObject("Text", "ConfirmInvalid")

        popupInvalidation.Show()

        HiddenFieldAction.Value = SingleAction.ConfirmInvalid

    End Sub

    Protected Sub ibtnDelete_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        Me.udtAuditLogEntry.AddDescripton("Transaction_ID", hfCurrentDetailTransactionNo.Value)

        Dim udtSM As New SystemMessage(FunctionCode, SeverityCode.SEVQ, MsgCode.MSG00001)
        Me.lblConfirmDelete.Text = udtSM.GetMessage

        Me.ModalPopupExtenderConfirmDelete.Show()

        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00098, AuditLogDescription.DeleteClaim_Click)
    End Sub

    Protected Sub ibtnlblConfirmDeleteConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        Dim udtEHSTransaction As EHSTransactionModel
        udtEHSTransaction = Me.udtSessionHandlerBLL.EHSTransactionGetFromSession(FunctionCode)

        Me.udtAuditLogEntry.AddDescripton("Transaction_ID", udtEHSTransaction.TransactionID)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00106, AuditLogDescription.DeleteClaim)

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL
        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

        Dim dtmUpdateTime As String = udtGeneralFunction.GetSystemDateTime

        Dim udtEHSTransactionBLL As New EHSTransactionBLL

        If Not IsNothing(udtEHSTransaction) Then
            Try

                udtEHSTransactionBLL.DeleteEHSTransactionManualReimburse(udtEHSTransaction, udtHCVUUser.UserID, dtmUpdateTime)

                udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00009)
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                udcInfoMessageBox.BuildMessageBox()

                panVoidResult.Visible = False
                MultiViewReimClaimTransManagement.ActiveViewIndex = ViewIndex.Complete

                ' CRE11-004
                Me.udtAuditLogEntry.AddDescripton("Transaction_ID", udtEHSTransaction.TransactionID)
                Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00102, AuditLogDescription.DeleteClaim_Success)
            Catch eSQL As SqlClient.SqlException
                If eSQL.Number = 50000 Then

                    ' CRE11-004
                    udtAuditLogEntry.AddDescripton("Transaction ID", udtEHSTransaction.TransactionID)
                    udcMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, eSQL.Message))
                    udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.UpdateFail, udtAuditLogEntry, LogID.LOG00097, AuditLogDescription.DeleteClaim_Fail)
                Else
                    Throw eSQL
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End If
    End Sub

    Protected Sub ibtnlblConfirmDeleteCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        Me.udtAuditLogEntry.AddDescripton("Transaction_ID", hfCurrentDetailTransactionNo.Value)

        Me.ModalPopupExtenderConfirmDelete.Hide()
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00099, AuditLogDescription.DeleteClaim_Cancel)
    End Sub

    ' Single action

    Protected Sub ibtnInputReasonCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim dtSelected As DataTable = Session(SESS_SelectedDataTable)
        ' CRE11-004
        AuditLogAddDescriptonTranNo(udtAuditLogEntry, dtSelected)

        Select Case HiddenFieldAction.Value
            Case ClaimTransStatus.Suspended
                udtAuditLogEntry.WriteLog(LogID.LOG00061, AuditLogDescription.SaveSuspendReasonCancelClick)
            Case ClaimTransStatus.Inactive
                udtAuditLogEntry.WriteLog(LogID.LOG00059, AuditLogDescription.SaveVoidReasonCancelClick)
        End Select

        HiddenFieldAction.Value = String.Empty

        MultiViewDetailAction.ActiveViewIndex = ViewIndexDetailAction.Button
    End Sub

    Protected Sub ibtnInputReasonSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Visible = False

        imgAlertReason.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        Dim dtSelected As DataTable = Session(SESS_SelectedDataTable)
        ' CRE11-004
        AuditLogAddDescriptonTranNo(udtAuditLogEntry, dtSelected)
        AuditLogAddDescriptonBatchReason(udtAuditLogEntry)

        Select Case HiddenFieldAction.Value
            Case ClaimTransStatus.Suspended
                udtAuditLogEntry.WriteStartLog(LogID.LOG00034, AuditLogDescription.SaveSuspendReasonClick)
            Case ClaimTransStatus.Inactive
                udtAuditLogEntry.WriteStartLog(LogID.LOG00040, AuditLogDescription.SaveVoidReasonClick)
        End Select

        If udtValidator.IsEmpty(txtReason.Text.Trim) Then
            imgAlertReason.Visible = True

            ' CRE11-004
            AuditLogAddDescriptonTranNo(udtAuditLogEntry, dtSelected)
            AuditLogAddDescriptonBatchReason(udtAuditLogEntry)

            Select Case HiddenFieldAction.Value
                Case ClaimTransStatus.Suspended
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001)
                    udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationFail, udtAuditLogEntry, LogID.LOG00036, AuditLogDescription.SaveSuspendReasonFail)

                Case ClaimTransStatus.Inactive
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002)
                    udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationFail, udtAuditLogEntry, LogID.LOG00042, AuditLogDescription.SaveVoidReasonFail)
            End Select

        Else
            lblConfirmReason.Text = txtReason.Text.Trim

            udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00021)
            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
            udcInfoMessageBox.BuildMessageBox()

            MultiViewDetailAction.ActiveViewIndex = ViewIndexDetailAction.ConfirmReason

            ' CRE11-004
            AuditLogAddDescriptonTranNo(udtAuditLogEntry, dtSelected)
            AuditLogAddDescriptonBatchReason(udtAuditLogEntry)

            Select Case HiddenFieldAction.Value
                Case SingleAction.Suspend
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00035, AuditLogDescription.SaveSuspendReasonSuccessful)
                Case SingleAction.Void
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00041, AuditLogDescription.SaveVoidReasonSuccessful)
            End Select

        End If

    End Sub

    Protected Sub ibtnConfirmReasonCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcInfoMessageBox.Visible = False
        udcMessageBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim dtSelected As DataTable = Session(SESS_SelectedDataTable)
        ' CRE11-004
        AuditLogAddDescriptonTranNo(udtAuditLogEntry, dtSelected)

        Select Case HiddenFieldAction.Value
            Case ClaimTransStatus.Suspended
                udtAuditLogEntry.WriteLog(LogID.LOG00062, AuditLogDescription.ConfirmSuspendReasonCancelClick)
            Case ClaimTransStatus.Inactive
                udtAuditLogEntry.WriteLog(LogID.LOG00060, AuditLogDescription.ConfirmVoidReasonCancelClick)
        End Select

        MultiViewDetailAction.ActiveViewIndex = ViewIndexDetailAction.InputReason
    End Sub

    Protected Sub ibtnConfirmReasonConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcInfoMessageBox.Visible = False
        udcMessageBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        AuditLogAddDescriptonConfirmReason(udtAuditLogEntry)

        Dim dtSelected As DataTable = Session(SESS_SelectedDataTable)
        ' CRE11-004
        AuditLogAddDescriptonTranNo(udtAuditLogEntry, dtSelected)
        AuditLogAddDescriptonConfirmReason(udtAuditLogEntry)
        Try
            Dim strUserID As String = udtHCVUUserBLL.GetHCVUUser.UserID.Trim
            Dim dtTransaction As DataTable = Session(SESS_TransactionDataTable)

            Select Case HiddenFieldAction.Value
                Case SingleAction.Suspend
                    udtAuditLogEntry.WriteStartLog(LogID.LOG00037, AuditLogDescription.ConfirmSuspendReasonClick)

                    udtReimbursementBLL.UpdateVoucherTransactionStatus(dtTransaction, dtSelected, ClaimTransStatus.Suspended, strUserID, lblConfirmReason.Text)

                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001)

                    panVoidResult.Visible = False

                    ' CRE11-004
                    AuditLogAddDescriptonTranNo(udtAuditLogEntry, dtSelected)
                    AuditLogAddDescriptonConfirmReason(udtAuditLogEntry)
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00038, AuditLogDescription.ConfirmSuspendReasonSuccessful)

                Case SingleAction.Void
                    udtAuditLogEntry.WriteStartLog(LogID.LOG00043, AuditLogDescription.ConfirmVoidReasonClick)

                    udtReimbursementBLL.VoidVoucherTransaction(dtTransaction, dtSelected, strUserID, lblConfirmReason.Text)

                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00002)

                    gvVoidResult.DataSource = dtSelected
                    gvVoidResult.DataBind()

                    panVoidResult.Visible = True

                    ' CRE11-004
                    AuditLogAddDescriptonTranNo(udtAuditLogEntry, dtSelected)
                    AuditLogAddDescriptonConfirmReason(udtAuditLogEntry)
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00044, AuditLogDescription.ConfirmVoidReasonSuccessful)

            End Select

            udcInfoMessageBox.BuildMessageBox()
            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete

            MultiViewReimClaimTransManagement.ActiveViewIndex = ViewIndex.Complete

        Catch eSQL As SqlClient.SqlException

            ' CRE11-004
            AuditLogAddDescriptonTranNo(udtAuditLogEntry, dtSelected)
            AuditLogAddDescriptonConfirmReason(udtAuditLogEntry)

            If eSQL.Number = 50000 Then
                udcMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, eSQL.Message))

                Select Case HiddenFieldAction.Value
                    Case ClaimTransStatus.Suspended
                        udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.UpdateFail, udtAuditLogEntry, LogID.LOG00039, AuditLogDescription.ConfirmSuspendReasonFail)
                    Case ClaimTransStatus.Inactive
                        udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.UpdateFail, udtAuditLogEntry, LogID.LOG00045, AuditLogDescription.ConfirmVoidReasonFail)
                End Select

            Else
                udtAuditLogEntry.AddDescripton("StackTrace", "Unknown SqlException: " + eSQL.Message)

                Select Case HiddenFieldAction.Value
                    Case ClaimTransStatus.Suspended
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00039, AuditLogDescription.ConfirmSuspendReasonFail)
                    Case ClaimTransStatus.Inactive
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00045, AuditLogDescription.ConfirmVoidReasonFail)
                End Select

                Throw eSQL
            End If

        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception: " + ex.Message)

            Select Case HiddenFieldAction.Value
                Case ClaimTransStatus.Suspended
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00039, AuditLogDescription.ConfirmSuspendReasonFail)
                Case ClaimTransStatus.Inactive
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00045, AuditLogDescription.ConfirmVoidReasonFail)
            End Select

            Throw ex
        End Try
    End Sub

    Protected Sub ibtnInputInvalidationReasonCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00108, AuditLogDescription.MarkInvalidCancelClick)
        ' CRE11-021 log the missed essential information [End]

        udcMessageBox.Visible = False
        imgAlertInvalidationReason.Visible = False
        imgAlertReasonRemark.Visible = False

        MultiViewDetailAction.ActiveViewIndex = ViewIndexDetailAction.Button
    End Sub

    Protected Sub ibtnInputInvalidationReasonSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Invalidation Reason", ddlInvalidationReason.SelectedValue)
        udtAuditLogEntry.AddDescripton("Invalidation Reason Remark", txtReasonRemark.Text)
        udtAuditLogEntry.WriteLog(LogID.LOG00109, AuditLogDescription.MarkInvalidSaveClick)
        ' CRE11-021 log the missed essential information [End]

        udcMessageBox.Visible = False
        imgAlertInvalidationReason.Visible = False
        imgAlertReasonRemark.Visible = False

        If ddlInvalidationReason.SelectedValue = String.Empty Then
            imgAlertInvalidationReason.Visible = True

            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00009)
            udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationFail)

            Return

        End If

        If ddlInvalidationReason.SelectedValue.Trim = InvalidationType.Others AndAlso txtReasonRemark.Text.Trim = String.Empty Then
            imgAlertReasonRemark.Visible = True

            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00010)
            udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationFail)

            Return

        End If

        lblConfirmInvalidationReason.Text = ddlInvalidationReason.SelectedItem.Text.Trim
        lblConfirmReasonRemark.Text = txtReasonRemark.Text.Trim

        If lblConfirmReasonRemark.Text = String.Empty Then lblConfirmReasonRemark.Text = Me.GetGlobalResourceObject("Text", "N/A")

        udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00021)
        udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
        udcInfoMessageBox.BuildMessageBox()

        MultiViewDetailAction.ActiveViewIndex = ViewIndexDetailAction.ConfirmInvalidationReason

    End Sub

    Protected Sub ibtnConfirmInvalidationReasonCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcInfoMessageBox.Visible = False
        udcMessageBox.Visible = False

        MultiViewDetailAction.ActiveViewIndex = ViewIndexDetailAction.InputInvalidationReason
    End Sub

    Protected Sub ibtnConfirmInvalidationReasonConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcInfoMessageBox.Visible = False
        udcMessageBox.Visible = False

        Dim dtSelected As DataTable = Session(SESS_SelectedDataTable)
        Dim drSelected As DataRow = dtSelected.Rows(0)

        Try
            Dim strUserID As String = udtHCVUUserBLL.GetHCVUUser.UserID.Trim

            udtReimbursementBLL.MarkInvalid(CStr(drSelected("transNum")).Trim, drSelected("tsmp"), strUserID, ddlInvalidationReason.SelectedValue.Trim, txtReasonRemark.Text.Trim)

            udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00004)
            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            udcInfoMessageBox.BuildMessageBox()

            MultiViewReimClaimTransManagement.ActiveViewIndex = ViewIndex.CompleteRemark

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then

            Else


                Throw eSQL
            End If

        Catch ex As Exception
            Throw ex

        End Try

    End Sub

    '

    Protected Sub ibtnPopupInvalidationConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcInfoMessageBox.Visible = False
        udcMessageBox.Visible = False

        Dim dtSelected As DataTable = Session(SESS_SelectedDataTable)
        Dim drSelected As DataRow = dtSelected.Rows(0)

        Try
            Dim strUserID As String = udtHCVUUserBLL.GetHCVUUser.UserID.Trim

            Select Case HiddenFieldAction.Value
                Case SingleAction.CancelInvalidation
                    udtReimbursementBLL.CancelInvalidation(CStr(drSelected("transNum")).Trim, drSelected("tsmp"), drSelected("Invalidation_TSMP"), strUserID)

                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00005)

                Case SingleAction.ConfirmInvalid
                    udtReimbursementBLL.ConfirmInvalid(CStr(drSelected("transNum")).Trim, drSelected("tsmp"), drSelected("Invalidation_TSMP"), strUserID)

                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00006)

            End Select

            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            udcInfoMessageBox.BuildMessageBox()

            MultiViewReimClaimTransManagement.ActiveViewIndex = ViewIndex.CompleteRemark

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then

                'CRE13-006 HCVS Ceiling [Start][Karl]
                Me.udcMessageBox.AddMessage(New SystemMessage("990001", "D", eSQL.Message))
                Me.udcMessageBox.BuildMessageBox("UpdateFail")
                'CRE13-006 HCVS Ceiling [End][Karl]

            Else


                Throw eSQL
            End If

        Catch ex As Exception
            Throw ex

        End Try

    End Sub

    Protected Sub ibtnPopupInvalidationCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    End Sub

    '

    Protected Sub gvVoidResult_RowDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Transaction No.
            Dim lblTransactionNo As Label = e.Row.FindControl("lblTransactionNo")
            lblTransactionNo.Text = udtFormatter.formatSystemNumber(lblTransactionNo.Text)

        End If
    End Sub

#Region "New Claim Transaction"

    Protected Sub ibtnNewClaimTransaction_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        ' Clear the eVaccination Record
        Dim udtSession As New BLL.SessionHandlerBLL()
        udtSession.CMSVaccineResultRemoveFromSession(FunctionCode)
        udtSession.CIMSVaccineResultRemoveFromSession(FunctionCode)
        ViewState.Remove(VS.VaccinationRecordPopupShown)

        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        Dim udtAuditLogInfo As New AuditLogInfo(Nothing, Nothing, Nothing, _
                                                    Me.txtTabeHSAccountID.Text.Trim, Me.ddlTabeHSAccountDocType.SelectedValue.Trim, Me.txtTabeHSAccountDocNo.Text.Trim)
        udtAuditLogEntry.AddDescripton("Doc Type", Me.ddlTabeHSAccountDocType.SelectedValue.Trim)
        udtAuditLogEntry.AddDescripton("Doc ID", Me.txtTabeHSAccountDocNo.Text.Trim)
        udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", Me.txtTabeHSAccountID.Text.Trim)

        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00063, AuditLogDescription.NewClaimTransaction_SearchAccountClick, udtAuditLogInfo)

        Me.udcMessageBox.Visible = False
        Me.udcInfoMessageBox.Visible = False

        imgTabeHSAccountIDErr.Visible = False
        imgTabeHSAccountDocNo.Visible = False

        If txtTabeHSAccountDocNo.Text.Trim.Equals(String.Empty) AndAlso txtTabeHSAccountID.Text.Trim.Equals(String.Empty) Then
            udtSM = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00257)
            Me.udcMessageBox.AddMessage(udtSM)

            udtAuditLogEntry.AddDescripton("Doc Type", Me.ddlTabeHSAccountDocType.SelectedValue.Trim)
            udtAuditLogEntry.AddDescripton("Doc ID", Me.txtTabeHSAccountDocNo.Text.Trim)
            udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", Me.txtTabeHSAccountID.Text.Trim)

            Me.udcMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00065, AuditLogDescription.NewClaimTransaction_SearchAccountClick_Fail)
            imgTabeHSAccountIDErr.Visible = True
            imgTabeHSAccountDocNo.Visible = True

        Else
            If Not Me.txtTabeHSAccountID.Text.Trim.Equals(String.Empty) AndAlso _
                      Not udtValidator.chkValidatedEHSAccountNumber(Me.txtTabeHSAccountID.Text.Trim()) Then

                udtSM = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00256)
                Me.udcMessageBox.AddMessage(udtSM)

                udtAuditLogEntry.AddDescripton("Doc Type", Me.ddlTabeHSAccountDocType.SelectedValue.Trim)
                udtAuditLogEntry.AddDescripton("Doc ID", Me.txtTabeHSAccountDocNo.Text.Trim)
                udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", Me.txtTabeHSAccountID.Text.Trim)

                Me.udcMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00065, AuditLogDescription.NewClaimTransaction_SearchAccountClick_Fail)
                imgTabeHSAccountIDErr.Visible = True
            Else
                Dim blnError As Boolean = False
                Dim strAdoptionPrefixNum As String = String.Empty
                Dim strIdentityNum As String = String.Empty
                Dim strRawIdentityNum As String = String.Empty
                Dim strIdentityNumFullTemp As String
                strIdentityNumFullTemp = Me.txtTabeHSAccountDocNo.Text.Trim.ToUpper.Replace("-", "").Replace("(", "").Replace(")", "")
                strRawIdentityNum = Me.txtTabeHSAccountDocNo.Text.Trim.ToUpper

                Dim strIdentityNumFull() As String
                strIdentityNumFull = strIdentityNumFullTemp.Trim.Split("/")
                If strIdentityNumFull.Length > 1 Then
                    strIdentityNum = strIdentityNumFull(1)
                    strAdoptionPrefixNum = strIdentityNumFull(0)
                Else
                    strIdentityNum = strIdentityNumFullTemp
                End If

                If Not Me.ddlTabeHSAccountDocType.SelectedValue.Trim.Equals(String.Empty) AndAlso Not Me.txtTabeHSAccountDocNo.Text.Trim.Equals(String.Empty) Then
                    Me.udtSM = Me.udtValidator.chkIdentityNumber(Me.ddlTabeHSAccountDocType.SelectedValue.Trim, strIdentityNum, strAdoptionPrefixNum)
                    If Not IsNothing(udtSM) Then
                        Me.imgTabeHSAccountDocNo.Visible = True
                        Me.udcMessageBox.AddMessage(udtSM)
                        Me.udcMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00065, AuditLogDescription.NewClaimTransaction_SearchAccountClick_Fail)
                        blnError = True
                    End If
                End If

                If Not blnError Then
                    Dim streHSAccountID As String
                    Dim blnNoRecord As Boolean = False

                    streHSAccountID = Me.txtTabeHSAccountID.Text.Trim
                    If Not String.IsNullOrEmpty(streHSAccountID) Then
                        streHSAccountID = streHSAccountID.Substring(0, streHSAccountID.Length - 1)
                    End If

                    Dim udtEHSAccountBLL As New EHSAccount.EHSAccountBLL
                    'Dim udtEHSAccountModelList As EHSAccount.EHSAccountModelCollection = Nothing

                    'udtEHSAccountModelList = udtEHSAccountBLL.LoadEHSAccountByIdentityVRID(Me.txtSearchAccID.Text.Trim, Me.ddlSearchAccDocType.SelectedValue.Trim, streHSAccountID)

                    Dim dtRes As DataTable = udtEHSAccountBLL.LoadEHSAccountByIdentityVRID(strIdentityNum, strAdoptionPrefixNum, Me.ddlTabeHSAccountDocType.SelectedValue.Trim, streHSAccountID, strRawIdentityNum)

                    If IsNothing(dtRes) Then
                        blnNoRecord = True
                    Else
                        If dtRes.Rows.Count = 0 Then
                            blnNoRecord = True
                        End If
                    End If

                    If blnNoRecord Then
                        Me.udcInfoMessageBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00007))
                        Me.udcInfoMessageBox.BuildMessageBox()
                        Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information

                        udtAuditLogEntry.AddDescripton("Doc Type", Me.ddlTabeHSAccountDocType.SelectedValue.Trim)
                        udtAuditLogEntry.AddDescripton("Doc ID", Me.txtTabeHSAccountDocNo.Text.Trim)
                        udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", Me.txtTabeHSAccountID.Text.Trim)
                        udtAuditLogEntry.AddDescripton("No Of Record Found", "0")
                        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00064, AuditLogDescription.NewClaimTransaction_SearchAccountClick_Success, udtAuditLogInfo)
                    Else
                        If dtRes.Rows.Count = 1 Then
                            'Go to Enter Claim details view

                            If Not Me.ddlTabeHSAccountDocType.SelectedValue.Trim.Equals(String.Empty) AndAlso Not CStr(dtRes.Rows(0)("Doc_Code")).Trim.Equals(Me.ddlTabeHSAccountDocType.SelectedValue.Trim) Then
                                Me.udcInfoMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00032))
                                Me.udcInfoMessageBox.BuildMessageBox()
                                Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information

                                udtAuditLogEntry.AddDescripton("Same Doc Type", "N")
                            Else
                                Me.GetReadyEHSAccount(dtRes.Rows(0)("IdentityNum"), dtRes.Rows(0)("Doc_Code"), True)
                                Me.MultiViewReimClaimTransManagement.ActiveViewIndex = ViewIndex.ViewNewClaimTransaction
                                Me.mvNewClaimTransaction.ActiveViewIndex = NewClaimTransaction.EnterClaimDetails

                                udtAuditLogEntry.AddDescripton("Same Doc Type", "Y")
                            End If

                            udtAuditLogEntry.AddDescripton("Doc Type", Me.ddlTabeHSAccountDocType.SelectedValue.Trim)
                            udtAuditLogEntry.AddDescripton("Doc ID", Me.txtTabeHSAccountDocNo.Text.Trim)
                            udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", Me.txtTabeHSAccountID.Text.Trim)
                            udtAuditLogEntry.AddDescripton("No Of Record Found", "1")
                            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00064, AuditLogDescription.NewClaimTransaction_SearchAccountClick_Success)

                            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
                            ' ----------------------------------------------------------
                            ' Check to show the Vaccination Record button
                            If VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N Or _
                                VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CIMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N Then
                                ibtnVaccinationRecord.Visible = True

                                ' Force Retrieve eVaccination Record once
                                Dim udtHAResult As HAVaccineResult = Nothing
                                Dim udtDHResult As DHVaccineResult = Nothing
                                Dim udtVaccineResultBag As New VaccineResultCollection

                                If udtSession.CMSVaccineResultGetFromSession(FunctionCode) Is Nothing Or _
                                    udtSession.CIMSVaccineResultGetFromSession(FunctionCode) Is Nothing Then
                                    Dim udtEHSAccount As EHSAccountModel = (New eHSAccountMaintBLL).EHSAccountGetFromSession(FunctionCode)

                                    udtVaccineResultBag = GetVaccinationRecordFromSession(udtEHSAccount, "")

                                    udtSession.CMSVaccineResultSaveToSession(udtVaccineResultBag.HAVaccineResult, FunctionCode)
                                    udtSession.CIMSVaccineResultSaveToSession(udtVaccineResultBag.DHVaccineResult, FunctionCode)

                                End If

                                BuildSystemMessage(udtVaccineResultBag)

                            Else
                                ibtnVaccinationRecord.Visible = False
                            End If
                            ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

                        Else
                            'Go to Select eHS Account view
                            If Me.ddlTabeHSAccountDocType.SelectedValue.Trim.Equals(String.Empty) Then
                                lblSerachAccountResultDocType.Text = Me.GetGlobalResourceObject("Text", "Any")
                            Else
                                lblSerachAccountResultDocType.Text = ddlTabeHSAccountDocType.SelectedItem.Text.Trim
                            End If

                            If Me.txtTabeHSAccountDocNo.Text.Trim.Equals(String.Empty) Then
                                lblSerachAccountResultIdentityNum.Text = Me.GetGlobalResourceObject("Text", "Any")
                            Else
                                lblSerachAccountResultIdentityNum.Text = txtTabeHSAccountDocNo.Text.Trim
                            End If

                            If Me.txtTabeHSAccountID.Text.Trim.Equals(String.Empty) Then
                                lblSerachAccountResultEHSAccountID.Text = Me.GetGlobalResourceObject("Text", "Any")
                            Else
                                lblSerachAccountResultEHSAccountID.Text = udtFormatter.formatValidatedEHSAccountNumber(streHSAccountID)
                            End If

                            Session(SESS_SearchAccount) = dtRes
                            Me.GridViewDataBind(Me.gvSearchAccount, dtRes, "Doc_Code", "ASC", False)

                            Me.MultiViewReimClaimTransManagement.ActiveViewIndex = ViewIndex.ViewNewClaimTransaction
                            Me.mvNewClaimTransaction.ActiveViewIndex = NewClaimTransaction.SearchAccountResults

                            udtAuditLogEntry.AddDescripton("Doc Type", Me.ddlTabeHSAccountDocType.SelectedValue.Trim)
                            udtAuditLogEntry.AddDescripton("Doc ID", Me.txtTabeHSAccountDocNo.Text.Trim)
                            udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", Me.txtTabeHSAccountID.Text.Trim)
                            udtAuditLogEntry.AddDescripton("No Of Record Found", dtRes.Rows.Count)
                            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00064, AuditLogDescription.NewClaimTransaction_SearchAccountClick_Success)


                        End If

                    End If
                End If
            End If

        End If

        'Me.MultiViewReimClaimTransManagement.ActiveViewIndex = ViewIndex.ViewNewClaimTransaction
        'Me.mvNewClaimTransaction.ActiveViewIndex = NewClaimTransaction.SearchAccount
    End Sub

#Region "Search Account"

    Protected Sub ibtnSearchAccountResultBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        Me.MultiViewReimClaimTransManagement.ActiveViewIndex = ViewIndex.InputCriteria

        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00067, AuditLogDescription.NewClaimTransaction_SelectAccount_Back)
    End Sub

    Private Sub gvSearchAccount_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSearchAccount.PageIndexChanging
        Me.GridViewPageIndexChangingHandler(sender, e, SESS_SearchAccount)
    End Sub

    Private Sub gvSearchAccount_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSearchAccount.PreRender
        Me.GridViewPreRenderHandler(sender, e, SESS_SearchAccount)
    End Sub

    Private Sub gvSearchAccount_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSearchAccount.RowCommand
        If TypeOf e.CommandSource Is LinkButton Then

            Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

            Dim strDocCode As String = String.Empty
            Dim strIdentityNum As String = String.Empty
            Dim strEHSAccountID As String = String.Empty

            Dim strCommandArgument As String = e.CommandArgument.ToString.Trim
            strIdentityNum = strCommandArgument.Split("|")(0).Trim
            strDocCode = strCommandArgument.Split("|")(1).Trim
            strEHSAccountID = strCommandArgument.Split("|")(2).Trim

            udtAuditLogEntry.AddDescripton("Doc Type", strDocCode)
            udtAuditLogEntry.AddDescripton("Doc ID", strIdentityNum)
            udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", strEHSAccountID)

            GetReadyEHSAccount(strIdentityNum, strDocCode, False)

            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
            ' ----------------------------------------------------------
            ' Check to show the Vaccination Record button
            If VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N Or _
                VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CIMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N Then
                ibtnVaccinationRecord.Visible = True

                ' Force Retrieve eVaccination Record once (everytime when the particular document is selected from the gridview)
                Dim udtSession As New BLL.SessionHandlerBLL()

                udtSession.CMSVaccineResultRemoveFromSession(FunctionCode)
                udtSession.CIMSVaccineResultRemoveFromSession(FunctionCode)
                ViewState.Remove(VS.VaccinationRecordPopupShown)

                Dim udtHAResult As HAVaccineResult = Nothing
                If udtSession.CMSVaccineResultGetFromSession(FunctionCode) Is Nothing Then
                    Dim udtEHSAccount As EHSAccountModel = (New eHSAccountMaintBLL).EHSAccountGetFromSession(FunctionCode)
                    Dim udtWSProxyCMS As New WSProxyCMS(Me.udtAuditLogEntry)
                    udtHAResult = udtWSProxyCMS.GetVaccine(udtEHSAccount)
                    udtSession.CMSVaccineResultSaveToSession(udtHAResult, FunctionCode)

                End If

                Dim udtDHResult As DHVaccineResult = Nothing
                If udtSession.CIMSVaccineResultGetFromSession(FunctionCode) Is Nothing Then
                    Dim udtEHSAccount As EHSAccountModel = (New eHSAccountMaintBLL).EHSAccountGetFromSession(FunctionCode)
                    Dim udtWSProxyCIMS As New WSProxyDHCIMS(Me.udtAuditLogEntry)
                    udtDHResult = udtWSProxyCIMS.GetVaccine(udtEHSAccount)
                    udtSession.CIMSVaccineResultSaveToSession(udtDHResult, FunctionCode)

                End If

                Dim udtVaccineResultBag As New VaccineResultCollection
                udtVaccineResultBag.DHVaccineResult = udtDHResult
                udtVaccineResultBag.HAVaccineResult = udtHAResult

                BuildSystemMessage(udtVaccineResultBag)

            Else
                ibtnVaccinationRecord.Visible = False
            End If
            ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]


            udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00066, AuditLogDescription.NewClaimTransaction_SelectAccount)

        End If
    End Sub

    Private Sub gvSearchAccount_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSearchAccount.RowCreated
        Dim udtSortedGVHeaderList As SortedGridviewHeaderModelCollection = New SortedGridviewHeaderModelCollection
        Dim strImageURL As String = Me.GetGlobalResourceObject("ImageURL", "infoicon")

        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(2, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(3, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))

        Me.GirdViewRowCreated(sender, e, udtSortedGVHeaderList)
    End Sub

    Private Sub gvSearchAccount_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSearchAccount.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRow = CType(e.Row.DataItem, System.Data.DataRowView).Row

            Dim lbtnEHSAccountID As LinkButton = CType(e.Row.FindControl("lbtnEHSAccountID"), LinkButton)
            Dim lblDocType As Label = CType(e.Row.FindControl("lblDocType"), Label)
            Dim lbtnIdentityNum As LinkButton = CType(e.Row.FindControl("lbtnIdentityNum"), LinkButton)
            Dim lblName As Label = CType(e.Row.FindControl("lblName"), Label)
            Dim lblCName As Label = CType(e.Row.FindControl("lblCName"), Label)
            Dim lblDOB As Label = CType(e.Row.FindControl("lblDOB"), Label)
            Dim lblSex As Label = CType(e.Row.FindControl("lblSex"), Label)
            Dim udtDocTypeBLL As New DocTypeBLL

            Dim strDocCode As String = CStr(dr.Item("Doc_Code")).Trim
            Dim strEHSAccountID As String = CStr(dr.Item("Voucher_Acc_ID")).Trim
            Dim strIdentityNum As String = CStr(dr.Item("IdentityNum")).Trim
            Dim strAdoptionPrefixNum As String = CStr(dr.Item("AdoptionPrefixNum")).Trim
            Dim strEName As String = CStr(dr.Item("EName")).Trim
            Dim strCName As String = CStr(dr.Item("CName")).Trim
            Dim dtmDOB As DateTime = CType(dr.Item("DOB"), DateTime)
            Dim strExactDOB As String = CStr(dr.Item("Exact_DOB")).Trim
            Dim strSex As String = CStr(dr.Item("Sex")).Trim
            Dim intAge As Nullable(Of Integer)
            Dim dtDOR As Nullable(Of Date)
            Dim strOtherInfo As String

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

            If IsDBNull(dr.Item("other_info")) Then
                strOtherInfo = String.Empty
            Else
                strOtherInfo = CStr(dr.Item("other_info"))
            End If

            lblDocType.Text = udtDocTypeBLL.getAllDocType.Filter(strDocCode).DocDisplayCode.Trim

            lbtnEHSAccountID.Text = udtFormatter.formatValidatedEHSAccountNumber(strEHSAccountID)
            lbtnEHSAccountID.CommandArgument = strIdentityNum & "|" & strDocCode & "|" & strEHSAccountID

            lbtnIdentityNum.Text = udtFormatter.FormatDocIdentityNoForDisplay(strDocCode, strIdentityNum, False, strAdoptionPrefixNum)
            lbtnIdentityNum.CommandArgument = strIdentityNum & "|" & strDocCode & "|" & strEHSAccountID

            lblName.Text = strEName
            lblCName.Text = udtFormatter.formatChineseName(strCName)

            lblDOB.Text = udtFormatter.formatDOB(strDocCode, dtmDOB, strExactDOB, "en-US", intAge, dtDOR, strOtherInfo)

            lblSex.Text = Me.GetGlobalResourceObject("Text", udtFormatter.formatGender(strSex))


        End If
    End Sub

    Private Sub gvSearchAccount_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvSearchAccount.Sorting
        Me.GridViewSortingHandler(sender, e, SESS_SearchAccount)
    End Sub

#End Region

#Region "Enter Creation Details"
    Protected Sub ibtnSearchSP_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim objAuditLogInfo As AuditLogInfo = Nothing
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        Dim udtEHSAccount As EHSAccount.EHSAccountModel
        Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
        udtEHSAccount = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)

        ' CRE1-004
        objAuditLogInfo = New AuditLogInfo(Me.txtEnterCreationDetailSPID.Text.Trim, Nothing, _
                                            udtEHSAccount.AccountSourceString, udtEHSAccount.VoucherAccID, _
                                            udtEHSAccount.SearchDocCode, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).IdentityNum)

        udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
        udtAuditLogEntry.AddDescripton("SP ID", Me.txtEnterCreationDetailSPID.Text.Trim)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00068, AuditLogDescription.NewClaimTransaction_SearchSP, objAuditLogInfo)

        Me.udcMessageBox.Visible = False
        Me.udcInfoMessageBox.Visible = False
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
                    udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
                    udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
                    udcInfoMessageBox.BuildMessageBox()

                    udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
                    udtAuditLogEntry.AddDescripton("SP ID", Me.txtEnterCreationDetailSPID.Text.Trim)
                    udtAuditLogEntry.AddDescripton("No of record", "0")
                    udtAuditLogEntry.AddDescripton("Go To Advanced Search", "N")
                    ' ' CRE1-004
                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00069, AuditLogDescription.NewClaimTransaction_SearchSP_Success, objAuditLogInfo)

                Else
                    GetReadyServiceProvider(CStr(dt.Rows(0)("SP_ID")))
                    udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
                    udtAuditLogEntry.AddDescripton("SP ID", Me.txtEnterCreationDetailSPID.Text.Trim)
                    udtAuditLogEntry.AddDescripton("No of record", "1")
                    udtAuditLogEntry.AddDescripton("Go To Advanced Search", "N")
                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00069, AuditLogDescription.NewClaimTransaction_SearchSP_Success, objAuditLogInfo)
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

                Me.imgAdvancedSearchSPIDErr.Visible = False
                Me.imgAdvancedSearchHKICErr.Visible = False

                Session.Remove(SESS_AdvancedSearchSP)

                ModalPopupSearchSP.Show()

                udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
                udtAuditLogEntry.AddDescripton("SP ID", Me.txtEnterCreationDetailSPID.Text.Trim)
                udtAuditLogEntry.AddDescripton("No of record", "-")
                udtAuditLogEntry.AddDescripton("Go To Advanced Search", "Y")
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00069, AuditLogDescription.NewClaimTransaction_SearchSP_Success, objAuditLogInfo)
            End If
        Else
            Me.imgEnterCreationDetailSPIDError.Visible = True
            udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
            udtAuditLogEntry.AddDescripton("SP ID", Me.txtEnterCreationDetailSPID.Text.Trim)
            udtAuditLogEntry.AddDescripton("No of record", "-")
            udtAuditLogEntry.AddDescripton("Go To Advanced Search", "N")

            Me.udcMessageBox.AddMessage(sm)
            Me.udcMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00070, AuditLogDescription.NewClaimTransaction_SearchSP_Fail, objAuditLogInfo)
        End If

    End Sub

    Protected Sub ibtnClearSearchSP_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        Dim udtEHSAccount As EHSAccount.EHSAccountModel
        Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
        udtEHSAccount = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)

        Me.txtEnterCreationDetailSPID.Text = String.Empty
        Me.lblEnterCreationDetailSPName.Text = String.Empty
        Me.lblEnterCreationDetailSPStatus.Text = String.Empty
        Me.lblEnterCreationDetailPracticeStatus.Text = String.Empty

        Me.ddlEnterCreationDetailPractice.Items.Clear()
        Me.ddlEnterCreationDetailPractice.Enabled = False

        Me.txtEnterCreationDetailSPID.Enabled = True

        Me.ibtnSearchSP.Enabled = True
        Me.ibtnClearSearchSP.Enabled = False

        Me.ibtnSearchSP.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchSBtn")
        Me.ibtnClearSearchSP.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ClearDisableSBtn")

        Me.ibtnEnterCreationDetailNext.Enabled = False
        Me.ibtnEnterCreationDetailNext.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "NextDisableBtn")

        Me.udcMessageBox.Visible = False
        ClearEnterCreationDetailsErrorImage()

        Session(SESS_ServiceProvider) = Nothing

        ' INT11-0011
        Me.udtSessionHandlerBLL.EHSTransactionRemoveFromSession(FunctionCode)
        ' End INT11-0011

        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00071, AuditLogDescription.NewClaimTransaction_SearchSP_Clear)

    End Sub

    Protected Sub ibtnEnterCreationDetailBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        Dim udtEHSAccount As EHSAccount.EHSAccountModel
        Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
        udtEHSAccount = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)

        Me.udcMessageBox.Visible = False
        Me.udcInfoMessageBox.Visible = False

        Me.ClearEnterCreationDetailsErrorImage()

        If IsNothing(Session(SESS_SearchAccount)) Then
            'Me.mvNewClaimTransaction.ActiveViewIndex = NewClaimTransaction.SearchAccount
            Me.MultiViewReimClaimTransManagement.ActiveViewIndex = ViewIndex.InputCriteria
        Else
            Me.mvNewClaimTransaction.ActiveViewIndex = NewClaimTransaction.SearchAccountResults
        End If
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00081, AuditLogDescription.NewClaimTransaction_EnterCreationDetail_Back)
    End Sub

    Protected Sub ibtnEnterCreationDetailNext_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim objAuditLogInfo As AuditLogInfo = Nothing
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        Dim udtEHSAccount As EHSAccount.EHSAccountModel
        Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
        udtEHSAccount = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)

        Dim strSPStatus As String = String.Empty

        If Me.lblEnterCreationDetailSPStatus.Text.Trim.Equals(String.Empty) Then
            strSPStatus = "Active"
        Else
            strSPStatus = Me.lblEnterCreationDetailSPStatus.Text.Trim.Replace("(", "").Replace(")", "")
        End If

        Dim strPracticeStatus As String = String.Empty

        If Me.lblEnterCreationDetailPracticeStatus.Text.Trim.Equals(String.Empty) Then
            strPracticeStatus = "Active"
        Else
            strPracticeStatus = Me.lblEnterCreationDetailPracticeStatus.Text.Replace("(", "").Replace(")", "")
        End If

        objAuditLogInfo = New AuditLogInfo(Me.txtEnterCreationDetailSPID.Text.Trim, Nothing, _
                                            udtEHSAccount.AccountSourceString, udtEHSAccount.VoucherAccID, _
                                            udtEHSAccount.SearchDocCode, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).IdentityNum)

        Me.udtAuditLogEntry.AddDescripton("SP ID", Me.txtEnterCreationDetailSPID.Text.Trim)
        Me.udtAuditLogEntry.AddDescripton("SP Name", Me.lblEnterCreationDetailSPName.Text.Trim)
        Me.udtAuditLogEntry.AddDescripton("SP Status", strSPStatus)
        Me.udtAuditLogEntry.AddDescripton("Practice ID", Me.ddlEnterCreationDetailPractice.SelectedValue.Trim)
        Me.udtAuditLogEntry.AddDescripton("Practice Status", strPracticeStatus)
        Me.udtAuditLogEntry.AddDescripton("Creation Reason", Me.ddlEnterCreationDetailCreationReason.SelectedValue.Trim)
        Me.udtAuditLogEntry.AddDescripton("Creation Reason Remark", Me.txtEnterCreationDetailRemarks.Text.Trim)
        Me.udtAuditLogEntry.AddDescripton("Payment Settlement", Me.ddlEnterCreationDetailPaymentMethod.SelectedValue.Trim)
        Me.udtAuditLogEntry.AddDescripton("Payment Settlement Remark", Me.txtEnterCreationDetailPaymentRemarks.Text.Trim)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00078, AuditLogDescription.NewClaimTransaction_EnterCreationDetail)

        'Checking
        Me.udcMessageBox.Visible = False
        Dim blnError As Boolean = False
        ClearEnterCreationDetailsErrorImage()


        If Me.ddlEnterCreationDetailPractice.SelectedValue.Trim = String.Empty Then
            blnError = True
            imgEnterCreationDetailPractice.Visible = True
            udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00012)
            Me.udcMessageBox.AddMessage(udtSM)
        End If

        If Me.ddlEnterCreationDetailCreationReason.SelectedValue.Trim = String.Empty Then
            blnError = True
            imgEnterCreationDetailCreationReason.Visible = True
            udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00013)
            Me.udcMessageBox.AddMessage(udtSM)
        End If

        If Me.ddlEnterCreationDetailCreationReason.SelectedValue.Trim = "O" AndAlso _
            Me.txtEnterCreationDetailRemarks.Text.Trim = String.Empty Then
            Me.imgEnterCreationDetailRemarks.Visible = True
            udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00011)
            Me.udcMessageBox.AddMessage(udtSM)
            blnError = True
        End If

        If Me.ddlEnterCreationDetailPaymentMethod.SelectedValue.Trim = String.Empty Then
            imgEnterCreationDetailPaymentMethod.Visible = True
            udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00014)
            Me.udcMessageBox.AddMessage(udtSM)
            blnError = True
        End If

        If Me.ddlEnterCreationDetailPaymentMethod.SelectedValue.Trim = "O" AndAlso _
            Me.txtEnterCreationDetailPaymentRemarks.Text.Trim = String.Empty Then
            Me.imgEnterCreationDetailPaymentRemarks.Visible = True
            udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00011)
            Me.udcMessageBox.AddMessage(udtSM)
            blnError = True
        End If

        If blnError Then
            udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
            Me.udcMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00080, AuditLogDescription.NewClaimTransaction_EnterCreationDetail_Fail, objAuditLogInfo)
        Else

            Dim udtManualEHSTransaction As New EHSTransactionModel

            Dim udtPracticeDisplayList As Practice.PracticeBLL.PracticeDisplayModelCollection
            Dim udtPracticeDisplay As Practice.PracticeBLL.PracticeDisplayModel

            udtPracticeDisplayList = Me.udtSessionHandlerBLL.PracticeDisplayListGetFromSession(FunctionCode)
            udtPracticeDisplay = udtPracticeDisplayList.Filter(CInt(Me.ddlEnterCreationDetailPractice.SelectedValue.Trim))

            udtManualEHSTransaction.EHSAcct = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
            udtManualEHSTransaction.ServiceProviderID = Me.txtEnterCreationDetailSPID.Text.Trim
            udtManualEHSTransaction.ServiceProviderName = Me.lblEnterCreationDetailSPName.Text.Trim
            udtManualEHSTransaction.PracticeID = udtPracticeDisplay.PracticeID
            udtManualEHSTransaction.PracticeName = udtPracticeDisplay.PracticeName
            udtManualEHSTransaction.PracticeNameChi = udtPracticeDisplay.PracticeNameChi
            udtManualEHSTransaction.BankAccountID = udtPracticeDisplay.BankAcctID
            udtManualEHSTransaction.BankAccountNo = udtPracticeDisplay.BankAccountNo
            udtManualEHSTransaction.BankAccountOwner = udtPracticeDisplay.BankAccHolder
            udtManualEHSTransaction.ServiceType = udtPracticeDisplay.ServiceCategoryCode

            udtManualEHSTransaction.CreationReason = Me.ddlEnterCreationDetailCreationReason.SelectedValue.Trim
            udtManualEHSTransaction.CreationRemarks = Me.txtEnterCreationDetailRemarks.Text.Trim
            udtManualEHSTransaction.PaymentMethod = Me.ddlEnterCreationDetailPaymentMethod.SelectedValue.Trim
            udtManualEHSTransaction.PaymentRemarks = Me.txtEnterCreationDetailPaymentRemarks.Text.Trim

            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            'Me.udtSessionHandlerBLL.EHSTransactionRemoveFromSession(FunctionCode)
            'Me.udtSessionHandlerBLL.EHSTransactionSaveToSession(udtManualEHSTransaction, FunctionCode)
            Me.udtSessionHandlerBLL.EHSTransactionWithoutTransactionDetailRemoveFromSession(FunctionCode)
            Me.udtSessionHandlerBLL.EHSTransactionWithoutTransactionDetailSaveToSession(udtManualEHSTransaction, FunctionCode)
            ' CRE17-010 (OCSSS integration) [End][Chris YIM]

            Me.udtSessionHandlerBLL.PracticeDisplayRemoveFromSession(FunctionCode)
            Me.udtSessionHandlerBLL.PracticeDisplaySaveToSession(udtPracticeDisplay, FunctionCode)
            Me.lblEnterClaimDetailSPID.Text = "(" + udtManualEHSTransaction.ServiceProviderID + ")"
            Me.lblEnterClaimDetailSPName.Text = udtManualEHSTransaction.ServiceProviderName
            Me.lblEnterClaimDetailSPStatus.Text = Me.lblEnterCreationDetailSPStatus.Text.Trim
            Me.lblEnterClaimDetailPractice.Text = Me.ddlEnterCreationDetailPractice.SelectedItem.Text.Trim
            Me.lblEnterClaimDetailPracticeStatus.Text = Me.lblEnterCreationDetailPracticeStatus.Text.Trim
            Me.panNonClinicSetting.Visible = False
            Me.lblNonClinicSetting.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting"))
            Me.lblEnterClaimDetailPaymentMethod.Text = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("ReimbursementPaymentMethod", udtManualEHSTransaction.PaymentMethod).DataValue
            Me.lblEnterClaimDetailCreationReason.Text = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("ClaimCreationReason", udtManualEHSTransaction.CreationReason).DataValue
            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Me.panHKICSymbol.Visible = False
            'Clear radio button list
            ClearHKICSymbolButtonList()
            ' CRE17-010 (OCSSS integration) [End][Chris YIM]

            If Not udtManualEHSTransaction.CreationRemarks.Trim.Equals(String.Empty) Then
                Me.lblEnterClaimDetailCreationReason.Text = Me.lblEnterClaimDetailCreationReason.Text & " (" & udtManualEHSTransaction.CreationRemarks.Trim & ")"
            End If

            If Not udtManualEHSTransaction.PaymentRemarks.Trim.Equals(String.Empty) Then
                Me.lblEnterClaimDetailPaymentMethod.Text = Me.lblEnterClaimDetailPaymentMethod.Text & " (" & udtManualEHSTransaction.PaymentRemarks.Trim & ")"
            End If

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'Me.txtEnterClaimDetailServiceDate.Text = udtFormatter.formatEnterDate(Me.udtCommonFunction.GetSystemDateTime())
            Me.txtEnterClaimDetailServiceDate.Text = udtFormatter.formatInputTextDate(Me.udtCommonFunction.GetSystemDateTime())
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            BindEnterClaimDetailsScheme()
            Me.SetSaveButtonEnable(Me.ibtnEnterClaimDetailSave, False)
            'Me.udInputEHSClaim.ClearCategorySelection()
            Me.udtSessionHandlerBLL.ClaimCategoryRemoveFromSession(FunctionCode)

            Me.mvEnterDetails.ActiveViewIndex = InputTransactionDetails.ClaimDetails

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
            ' -----------------------------------------------------------------------------------------
            Me.ClearClaimControlErrorImage()
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]

            Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00079, AuditLogDescription.NewClaimTransaction_EnterCreationDetail_Success, objAuditLogInfo)

        End If

    End Sub

    Protected Sub ibtnAdvancedSearchSP_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim objAuditLogInfo As AuditLogInfo = Nothing
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)


        Dim udtEHSAccount As EHSAccount.EHSAccountModel
        Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
        udtEHSAccount = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)

        udtAuditLogEntry.AddDescripton("SP ID", Me.txtAdvancedSearchSPID.Text.Trim)
        udtAuditLogEntry.AddDescripton("SP HKIC No.", Me.txtAdvancedSearchHKIC.Text.Trim)
        udtAuditLogEntry.AddDescripton("SP Name", Me.txtAdvancedSearchName.Text.Trim)
        udtAuditLogEntry.AddDescripton("SP Phone No.", Me.txtAdvancedSearchPhone.Text.Trim)

        objAuditLogInfo = New AuditLogInfo(Me.txtAdvancedSearchSPID.Text.Trim, Me.txtAdvancedSearchHKIC.Text.Trim, _
                                            udtEHSAccount.AccountSourceString, udtEHSAccount.VoucherAccID, _
                                            udtEHSAccount.SearchDocCode, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).IdentityNum)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00072, AuditLogDescription.NewClaimTransaction_AdvancedSearchSP)

        Me.udcSystemMsgAdvancedSearch.Visible = False
        Me.udcInfoMsgAdvancedSearch.Visible = False

        Me.imgAdvancedSearchSPIDErr.Visible = False
        Me.imgAdvancedSearchHKICErr.Visible = False

        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        Dim strHKIC As String = String.Empty

        If Me.txtAdvancedSearchHKIC.Text.Trim.Equals(String.Empty) AndAlso Me.txtAdvancedSearchName.Text.Trim.Equals(String.Empty) AndAlso _
           Me.txtAdvancedSearchPhone.Text.Trim.Equals(String.Empty) AndAlso Me.txtAdvancedSearchSPID.Text.Trim.Equals(String.Empty) Then
            udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
            udtAuditLogEntry.AddDescripton("SP ID", Me.txtAdvancedSearchSPID.Text.Trim)
            udtAuditLogEntry.AddDescripton("SP HKIC No.", Me.txtAdvancedSearchHKIC.Text.Trim)
            udtAuditLogEntry.AddDescripton("SP Name", Me.txtAdvancedSearchName.Text.Trim)
            udtAuditLogEntry.AddDescripton("SP Phone No.", Me.txtAdvancedSearchPhone.Text.Trim)

            Me.ModalPopupSearchSP.Show()
            udtSM = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00257)
            Me.udcSystemMsgAdvancedSearch.AddMessage(udtSM)
            Me.udcSystemMsgAdvancedSearch.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00074, AuditLogDescription.NewClaimTransaction_AdvancedSearchSP_Fail, objAuditLogInfo)

        Else
            Dim blnKeywordSearch As Boolean = False
            Dim blnError As Boolean = False

            If Not Me.txtAdvancedSearchSPID.Text.Trim.Equals(String.Empty) AndAlso Me.txtAdvancedSearchSPID.Text.Trim.Length = 8 Then
                blnKeywordSearch = True
            Else
                udtSM = Me.udtValidator.chkSPID(Me.txtAdvancedSearchSPID.Text.Trim)
                If Not IsNothing(udtSM) Then
                    Me.udcSystemMsgAdvancedSearch.AddMessage(udtSM)
                    Me.imgAdvancedSearchSPIDErr.Visible = True
                    blnError = True
                End If
            End If

            If Not Me.txtAdvancedSearchHKIC.Text.Trim.Equals(String.Empty) Then
                udtSM = Me.udtValidator.chkIdentityNumber(DocTypeModel.DocTypeCode.HKIC, Me.txtAdvancedSearchHKIC.Text.Trim, String.Empty)
                If IsNothing(udtSM) Then
                    strHKIC = Me.udtFormatter.formatHKID(Me.txtAdvancedSearchSPID.Text, False)
                    blnKeywordSearch = True
                Else
                    Me.udcSystemMsgAdvancedSearch.AddMessage(udtSM)
                    Me.imgAdvancedSearchHKICErr.Visible = True
                    blnError = True
                End If

            End If

            If blnError Then
                Me.ModalPopupSearchSP.Show()

                udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
                udtAuditLogEntry.AddDescripton("SP ID", Me.txtAdvancedSearchSPID.Text.Trim)
                udtAuditLogEntry.AddDescripton("SP HKIC No.", Me.txtAdvancedSearchHKIC.Text.Trim)
                udtAuditLogEntry.AddDescripton("SP Name", Me.txtAdvancedSearchName.Text.Trim)
                udtAuditLogEntry.AddDescripton("SP Phone No.", Me.txtAdvancedSearchPhone.Text.Trim)

                Me.udcSystemMsgAdvancedSearch.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00074, AuditLogDescription.NewClaimTransaction_AdvancedSearchSP_Fail, objAuditLogInfo)
            Else
                Dim udtAccountChangeMaintBLL As New AccountChangeMaintenance.AccountChangeMaintenanceBLL

                Dim dt As DataTable

                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
                ' -------------------------------------------------------------------------
                'dt = udtAccountChangeMaintBLL.MaintenanceSearch(String.Empty, Me.txtAdvancedSearchSPID.Text.Trim, _
                '        udtFormatter.formatHKIDInternal(Me.txtAdvancedSearchHKIC.Text), Me.txtAdvancedSearchName.Text.Trim, _
                '        Me.txtAdvancedSearchPhone.Text.Trim, String.Empty, String.Empty)

                Dim udtBLLSearchResult As BaseBLL.BLLSearchResult

                ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
                udtBLLSearchResult = udtAccountChangeMaintBLL.MaintenanceSearch(FunctionCode, String.Empty, Me.txtAdvancedSearchSPID.Text.Trim, _
                                                                                udtFormatter.formatHKIDInternal(Me.txtAdvancedSearchHKIC.Text), Me.txtAdvancedSearchName.Text.Trim, String.Empty, _
                                                                                Me.txtAdvancedSearchPhone.Text.Trim, String.Empty, String.Empty)
                ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]

                dt = CType(udtBLLSearchResult.Data, DataTable)
                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

                ' Sort the datatable by SPID
                dt = SortServiceProvider(dt, "SP_ID")

                If dt.Rows.Count = 0 Then
                    Me.ModalPopupSearchSP.Show()
                    Me.pnlAdvancedSearchCritieria.Visible = True
                    Me.pnlAdvancedSearchResult.Visible = False

                    Me.udcInfoMsgAdvancedSearch.Type = CustomControls.InfoMessageBoxType.Information
                    udcInfoMsgAdvancedSearch.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
                    udcInfoMsgAdvancedSearch.BuildMessageBox()

                    udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
                    udtAuditLogEntry.AddDescripton("SP ID", Me.txtAdvancedSearchSPID.Text.Trim)
                    udtAuditLogEntry.AddDescripton("SP HKIC No.", Me.txtAdvancedSearchHKIC.Text.Trim)
                    udtAuditLogEntry.AddDescripton("SP Name", Me.txtAdvancedSearchName.Text.Trim)
                    udtAuditLogEntry.AddDescripton("SP Phone No.", Me.txtAdvancedSearchPhone.Text.Trim)
                    udtAuditLogEntry.AddDescripton("No of record", "0")
                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00073, AuditLogDescription.NewClaimTransaction_AdvancedSearchSP_Success, objAuditLogInfo)

                ElseIf dt.Rows.Count = 1 AndAlso blnKeywordSearch Then
                    'Return to Enter Creation Details and close the popup
                    Me.GetReadyServiceProvider(CStr(dt.Rows(0)("SP_ID")))
                    Me.ModalPopupSearchSP.Hide()

                    Session.Remove(SESS_AdvancedSearchSP)

                    udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
                    udtAuditLogEntry.AddDescripton("SP ID", Me.txtAdvancedSearchSPID.Text.Trim)
                    udtAuditLogEntry.AddDescripton("SP HKIC No.", Me.txtAdvancedSearchHKIC.Text.Trim)
                    udtAuditLogEntry.AddDescripton("SP Name", Me.txtAdvancedSearchName.Text.Trim)
                    udtAuditLogEntry.AddDescripton("SP Phone No.", Me.txtAdvancedSearchPhone.Text.Trim)
                    udtAuditLogEntry.AddDescripton("No of record", "1")
                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00073, AuditLogDescription.NewClaimTransaction_AdvancedSearchSP_Success, objAuditLogInfo)
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

                    If dt.Rows.Count >= 10 Then
                        pnlAdvancedSearchResult.Attributes.Remove("style")
                        pnlAdvancedSearchResult.Attributes.Add("style", "height: 500px;overflow: auto;")
                    Else
                        pnlAdvancedSearchResult.Attributes.Remove("style")
                        pnlAdvancedSearchResult.Attributes.Add("style", "height: auto;overflow: auto;")
                    End If

                    Session(SESS_AdvancedSearchSP) = dt
                    Me.GridViewDataBind(gvAdvancedSearchSP, dt, "SP_ID", "ASC", False)

                    udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
                    udtAuditLogEntry.AddDescripton("SP ID", Me.txtAdvancedSearchSPID.Text.Trim)
                    udtAuditLogEntry.AddDescripton("SP HKIC No.", Me.txtAdvancedSearchHKIC.Text.Trim)
                    udtAuditLogEntry.AddDescripton("SP Name", Me.txtAdvancedSearchName.Text.Trim)
                    udtAuditLogEntry.AddDescripton("SP Phone No.", Me.txtAdvancedSearchPhone.Text.Trim)
                    udtAuditLogEntry.AddDescripton("No of record", dt.Rows.Count)
                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00073, AuditLogDescription.NewClaimTransaction_AdvancedSearchSP_Success)

                End If
            End If
        End If
    End Sub

    Protected Sub ibtnAdvancedSearchSPClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        Dim udtEHSAccount As EHSAccount.EHSAccountModel
        Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
        udtEHSAccount = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)

        Me.ModalPopupSearchSP.Hide()
        Session.Remove(SESS_AdvancedSearchSP)

        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00075, AuditLogDescription.NewClaimTransaction_AdvancedSearchSP_Close)
    End Sub

    Protected Sub ibtnAdvancedSearchResultBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        Dim udtEHSAccount As EHSAccount.EHSAccountModel
        Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
        udtEHSAccount = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)

        Me.pnlAdvancedSearchCritieria.Visible = True
        Me.pnlAdvancedSearchResult.Visible = False
        Me.ModalPopupSearchSP.Show()
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00077, AuditLogDescription.NewClaimTransaction_AdvancedSelectSP_Back)
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
        Dim objAuditLogInfo As AuditLogInfo = Nothing
        If TypeOf e.CommandSource Is LinkButton Then
            Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

            Dim strSPID As String = String.Empty

            Dim strCommandArgument As String = e.CommandArgument.ToString.Trim
            strSPID = strCommandArgument
            Me.GetReadyServiceProvider(strSPID)

            Session.Remove(SESS_AdvancedSearchSP)

            Me.ModalPopupSearchSP.Hide()

            Dim udtEHSAccount As EHSAccount.EHSAccountModel
            Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
            udtEHSAccount = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
            udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
            Me.udtAuditLogEntry.AddDescripton("SP ID", strSPID)

            objAuditLogInfo = New AuditLogInfo(strSPID, Nothing, _
                                            udtEHSAccount.AccountSourceString, udtEHSAccount.VoucherAccID, _
                                            udtEHSAccount.SearchDocCode, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).IdentityNum)

            Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00076, AuditLogDescription.NewClaimTransaction_AdvancedSelectSP, objAuditLogInfo)
        End If
    End Sub

    Private Sub gvAdvancedSearchSP_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAdvancedSearchSP.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblAdvancedSearchCname As Label = CType(e.Row.FindControl("lblAdvancedSearchCname"), Label)
            lblAdvancedSearchCname.Text = udtFormatter.formatChineseName(lblAdvancedSearchCname.Text.Trim)

            Dim lblAdvancedSearchSPHKID As Label = CType(e.Row.FindControl("lblAdvancedSearchSPHKID"), Label)
            lblAdvancedSearchSPHKID.Text = Me.udtFormatter.FormatDocIdentityNoForDisplay(DocTypeModel.DocTypeCode.HKIC, lblAdvancedSearchSPHKID.Text.Trim, False)
        End If
    End Sub

    Private Sub gvAdvancedSearchSP_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvAdvancedSearchSP.Sorting
        Me.GridViewSortingHandler(sender, e, SESS_AdvancedSearchSP)
        Me.ModalPopupSearchSP.Show()
    End Sub

    Private Sub ddlEnterCreationDetailPractice_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlEnterCreationDetailPractice.SelectedIndexChanged
        If Me.ddlEnterCreationDetailPractice.SelectedValue.Trim.Equals(String.Empty) Then
            Me.lblEnterCreationDetailPracticeStatus.Text = String.Empty
        Else
            Dim udtPracticeDisplayList As Practice.PracticeBLL.PracticeDisplayModelCollection
            Dim udtPracticeDisplay As Practice.PracticeBLL.PracticeDisplayModel

            udtPracticeDisplayList = Me.udtSessionHandlerBLL.PracticeDisplayListGetFromSession(FunctionCode)
            udtPracticeDisplay = udtPracticeDisplayList.Filter(CInt(Me.ddlEnterCreationDetailPractice.SelectedValue.Trim))

            If udtPracticeDisplay.PracticeStatus.Trim.Equals(PracticeStatus.Active) Then
                Me.lblEnterCreationDetailPracticeStatus.Text = String.Empty
            Else
                Status.GetDescriptionFromDBCode(PracticeStatus.ClassCode, udtPracticeDisplay.PracticeStatus, Me.lblEnterCreationDetailPracticeStatus.Text, String.Empty)
                Me.lblEnterCreationDetailPracticeStatus.Text = " (" + Me.lblEnterCreationDetailPracticeStatus.Text + ")"
            End If

        End If

    End Sub

    Private Function SortServiceProvider(ByVal dt As DataTable, ByVal strField As String) As DataTable
        Dim dtResult As DataTable = dt.Clone

        For Each dr As DataRow In dt.Select(String.Empty, strField)
            dtResult.ImportRow(dr)
        Next

        Return dtResult

    End Function

#End Region

#Region "Enter Claim Details"
    Protected Sub ddlEnterClaimDetailsSchemeText_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim udtSchemeClaimList As SchemeClaimModelCollection
        Dim udtSchemeClaim As SchemeClaimModel
        Dim blnWithoutConversionRate As Boolean = False

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Dim udtEHSAccount As EHSAccount.EHSAccountModel
        Dim udteHSAccountMaintBLL As New eHSAccountMaintBLL
        Dim dtmServiceDate As Date = udtFormatter.convertDate(udtFormatter.formatInputDate(Me.txtEnterClaimDetailServiceDate.Text.Trim), Common.Component.CultureLanguage.English)

        udtEHSAccount = udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]
        udtSchemeClaimList = Me.udtSessionHandlerBLL.SchemeListGetFromSession(FunctionCode)
        udtSchemeClaim = udtSchemeClaimList.Filter(Me.ddlEnterClaimDetailsSchemeText.SelectedValue.Trim)

        Me.udtSessionHandlerBLL.SelectSchemeRemoveFromSession(FunctionCode)
        Me.udtSessionHandlerBLL.SelectSchemeSaveToSession(udtSchemeClaim, FunctionCode)
        Me.udtSessionHandlerBLL.ChangeSchemeInPracticeSaveToSession(True, FunctionCode)
        Me.udInputEHSClaim.ResetSchemeType()

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        'Dim udtEHSTransaction As EHSTransactionModel = Me.udtSessionHandlerBLL.EHSTransactionGetFromSession(FunctionCode)
        Dim udtEHSTransaction As EHSTransactionModel = Me.udtSessionHandlerBLL.EHSTransactionWithoutTransactionDetailGetFromSession(FunctionCode)
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]


        'CRE15-004 (TIV and QIV) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        lblEnterClaimDetailsSchemeStatus.Visible = False
        lblEnterClaimDetailsSchemeStatus.Text = String.Empty
        Dim ddlScheme As DropDownList = CType(sender, DropDownList)

        If Not ddlScheme Is Nothing Then
            If ddlScheme.SelectedIndex <> 0 Then
                Dim strSchemeCodeEnrol As String = New SchemeClaimBLL().ConvertSchemeEnrolFromSchemeClaimCode(ddlScheme.SelectedValue.Trim)

                Dim udtPracticeSchemeInfoBLL As New PracticeSchemeInfo.PracticeSchemeInfoBLL
                Dim udtPracticeSchemeInfoModelCollection As PracticeSchemeInfo.PracticeSchemeInfoModelCollection = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListPermanentBySPIDPracticeDisplaySeq(udtEHSTransaction.ServiceProviderID, udtEHSTransaction.PracticeID, New Common.DataAccess.Database)
                Dim udtResPracticeSchemeInfoModelCollection As PracticeSchemeInfo.PracticeSchemeInfoModelCollection = udtPracticeSchemeInfoModelCollection.FilterByPracticeScheme(udtEHSTransaction.PracticeID, strSchemeCodeEnrol)

                If Not udtResPracticeSchemeInfoModelCollection Is Nothing Then
                    For Each udtPracticeSchemeInfoModel As PracticeSchemeInfo.PracticeSchemeInfoModel In udtResPracticeSchemeInfoModelCollection.Values
                        'If one of practice scheme is delisted, the label "delisted" will be shown.
                        If udtPracticeSchemeInfoModel.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.DelistedInvoluntary Or _
                            udtPracticeSchemeInfoModel.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.DelistedVoluntary Then
                            Status.GetDescriptionFromDBCode(PracticeStatus.ClassCode, PracticeSchemeInfoStatus.Delisted, Me.lblEnterClaimDetailsSchemeStatus.Text, String.Empty)
                            lblEnterClaimDetailsSchemeStatus.Text = "(" + lblEnterClaimDetailsSchemeStatus.Text + ")"
                            lblEnterClaimDetailsSchemeStatus.Visible = True
                        End If
                    Next

                    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    If udtResPracticeSchemeInfoModelCollection.IsNonClinic Then
                        Me.panNonClinicSetting.Visible = True
                        udtSessionHandlerBLL.NonClinicSettingSaveToSession(True, FunctionCode)
                    Else
                        Me.panNonClinicSetting.Visible = False
                        udtSessionHandlerBLL.NonClinicSettingSaveToSession(False, FunctionCode)
                    End If
                    'CRE16-002 (Revamp VSS) [End][Chris YIM]

                End If

                ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                ' ----------------------------------------------------------
                If udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode.Trim).DocCode = DocTypeModel.DocTypeCode.HKIC Then
                    EnableHKICSymbolRadioButtonList(True, strSchemeCodeEnrol, dtmServiceDate)
                End If
                ' CRE17-010 (OCSSS integration) [End][Chris YIM]

            End If
        End If
        'CRE15-004 (TIV and QIV) [End][Chris YIM]

        'udtEHSTransaction.TransactionDetails = Nothing
        'udtEHSTransaction.TransactionAdditionFields = Nothing
        '' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        ''If ddlEnterClaimDetailsSchemeText.SelectedValue.Trim.Equals(SchemeClaimModel.HCVS) Then
        ''CRE13-019-02 Extend HCVS to China [Start][Karl]
        If New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(ddlEnterClaimDetailsSchemeText.SelectedValue) = SchemeClaimModel.EnumControlType.VOUCHER OrElse _
            New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(ddlEnterClaimDetailsSchemeText.SelectedValue) = SchemeClaimModel.EnumControlType.VOUCHERCHINA Then
            ''CRE13-019-02 Extend HCVS to China [End][Karl]
            ''udtEHSTransaction.PerVoucherValue = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeValue
            'Dim dtmServiceDate As Date = udtFormatter.convertDate(udtFormatter.formatInputDate(Me.txtEnterClaimDetailServiceDate.Text.Trim), Common.Component.CultureLanguage.English)
            'udtEHSTransaction.PerVoucherValue = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVoucher, dtmServiceDate).SubsidizeFee

            'check exchange rate
            If CheckExchangeRateAbsence(dtmServiceDate) = True Then
                blnWithoutConversionRate = True
            End If

        End If

        If blnWithoutConversionRate = False Then
            'Me.udtSessionHandlerBLL.EHSTransactionSaveToSession(udtEHSTransaction, FunctionCode)

            Me.udtSessionHandlerBLL.EHSClaimVaccineRemoveFromSession(FunctionCode)

            If Me.ddlEnterClaimDetailsSchemeText.SelectedIndex = 0 Then
                Me.SetSaveButtonEnable(Me.ibtnEnterClaimDetailSave, False)
                Me.udcMessageBox.Visible = False
                Me.panNonClinicSetting.Visible = False
                ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                ' ----------------------------------------------------------
                Me.panHKICSymbol.Visible = False
                ' CRE17-010 (OCSSS integration) [End][Chris YIM]
            End If

            'Me.udInputEHSClaim.Clear()
            Me.SetUpEnterClaimDetails(False)

            Me.udtSessionHandlerBLL.ClaimCategoryRemoveFromSession(FunctionCode)
        Else

            Me.SetSaveButtonEnable(Me.ibtnEnterClaimDetailSave, False)
            Me.udcMessageBox.Visible = True
        End If

    End Sub
    'CRE13-019-02 Extend HCVS to China [Start][Karl]
    Private Function CheckExchangeRateAbsence(ByVal pdtmServiceDate As Date) As Boolean
        If New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(ddlEnterClaimDetailsSchemeText.SelectedValue) = SchemeClaimModel.EnumControlType.VOUCHERCHINA Then
            'check exchange rate
            Dim decExchangeRate As Decimal = 0
            Dim udtExchangeRateBLL As ExchangeRate.ExchangeRateBLL = New ExchangeRate.ExchangeRateBLL()

            decExchangeRate = udtExchangeRateBLL.GetExchangeRateValue(pdtmServiceDate)
            If decExchangeRate <= 0 Then

                Me.udtSM = New Common.ComObject.SystemMessage(FunctCode.FUNT010404, SeverityCode.SEVE, MsgCode.MSG00018)
                Me.udcMessageBox.AddMessage(Me.udtSM)
                Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
                udtAuditLogEntry.AddDescripton("Error", "No conversion rate can be found")
                udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationFail, udtAuditLogEntry, LogID.LOG00084, AuditLogDescription.NewClaimTransaction_EnterClaimDetail_Fail)

                Me.SetSaveButtonEnable(Me.ibtnEnterClaimDetailSave, False)
                Me.udcMessageBox.Visible = True

                Return True
            End If
        End If
    End Function
    'CRE13-019-02 Extend HCVS to China [End][Karl]

    Private Sub SetUpEnterClaimDetails(Optional ByVal blnPostbackRebuild As Boolean = True)
        Dim strServiceDate As String = Me.udtFormatter.formatInputDate(Me.txtEnterClaimDetailServiceDate.Text.Trim)
        Dim dtmServiceDate As Date

        Dim strValidatedServiceDate As String = "ValidatedServiceDate"
        strServiceDate = udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English)

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtInputPicker As InputPickerModel = Nothing
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL
        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

        Dim udtEHSTransaction As EHSTransactionModel
        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        udtEHSTransaction = Me.udtSessionHandlerBLL.EHSTransactionWithoutTransactionDetailGetFromSession(FunctionCode)
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        ' Not necessary to clear inputted value when postback rebuild
        If Not blnPostbackRebuild Then Me.udInputEHSClaim.Clear()
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Koala]

        If Not IsNothing(ddlEnterClaimDetailsSchemeText) AndAlso Not Me.ddlEnterClaimDetailsSchemeText.SelectedValue.Trim.Equals(String.Empty) AndAlso DateTime.TryParse(strServiceDate, dtmServiceDate) Then

            Dim strSchemeCode As String = Me.ddlEnterClaimDetailsSchemeText.SelectedValue.Trim
            Dim blnNoCategory As Boolean = True
            Dim blnNotAvailableForClaim As Boolean = True
            Dim isEligibleForClaim As Boolean = True

            Dim udtClaimCategory As ClaimCategory.ClaimCategoryModel = Me.udtSessionHandlerBLL.ClaimCategoryGetFromSession(FunctionCode)

            'If Not DateTime.TryParse(strServiceDate, dtmServiceDate) OrElse Not IsValidServiceDate(Me.txtEnterClaimDetailServiceDate.Text, strSchemeCode) Then
            'If DateTime.TryParse(strServiceDate, dtmServiceDate) Then 'OrElse Not IsValidServiceDate(Me.txtEnterClaimDetailServiceDate.Text, strSchemeCode) Then
            dtmServiceDate = strServiceDate 'udtFormatter.convertDate(Me.txtEnterClaimDetailServiceDate.Text.Trim, Common.Component.CultureLanguage.English)
            'End If

            Dim dtmCurrentDate As Date = Me.udtGeneralFunction.GetSystemDateTime

            ' Scheme Information
            Dim udtSchemeClaimList As SchemeClaimModelCollection
            Dim udtSchemeClaim As SchemeClaimModel

            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
            ' Refresh Available Voucher Amount when Service Date changed
            'udtSchemeClaimList = Me.udtSessionHandlerBLL.SchemeListGetFromSession(FunctionCode)
            udtSchemeClaimList = Me.udtSchemeClaimBLL.getSchemeClaimFromBackOfficeUserAndPractice(udtHCVUUser.UserID, FunctionCode, udtEHSTransaction.ServiceProviderID, udtEHSTransaction.PracticeID, dtmServiceDate)
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]
            udtSchemeClaim = udtSchemeClaimList.Filter(Me.ddlEnterClaimDetailsSchemeText.SelectedValue.Trim)

            Me.udtSessionHandlerBLL.SelectSchemeSaveToSession(udtSchemeClaim, FunctionCode)

            'EHS Account
            Dim udtEHSAccount As EHSAccount.EHSAccountModel
            Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
            udtEHSAccount = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)

            Dim udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel
            udtEHSPersonalInfo = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)

            'RVP Home List
            If Not IsNothing(Session(SESS_SearchRVPHomeList)) AndAlso CBool(Session(SESS_SearchRVPHomeList)) Then
                Me.ModalPopupExtenderRVPHomeListSearch.Show()
            Else
                Me.ModalPopupExtenderRVPHomeListSearch.Hide()
            End If

            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
            ' --------------------------------------------------------------------------------------
            'School List
            If Not IsNothing(Session(SESS_SearchSchoolList)) AndAlso CBool(Session(SESS_SearchSchoolList)) Then
                Me.ModalPopupExtenderSchoolListSearch.Show()
            Else
                Me.ModalPopupExtenderSchoolListSearch.Hide()
            End If
            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

            If Not strSchemeCode.Equals(String.Empty) AndAlso _
                Not udtSchemeClaim Is Nothing AndAlso _
                udtSchemeClaim.SubsidizeGroupClaimList.Filter(dtmServiceDate).Count > 0 Then

                ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Select Case New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(strSchemeCode)
                    Case SchemeClaimModel.EnumControlType.VOUCHER, SchemeClaimModel.EnumControlType.VOUCHERCHINA
                        'Get Voucher Info.

                        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                        Dim udtSelectedPractice As Common.Component.Practice.PracticeBLL.PracticeDisplayModel = Me.udtSessionHandlerBLL.PracticeDisplayGetFromSession(FunctionCode)

                        Dim udtVoucherInfo As New VoucherInfoModel(VoucherInfoModel.AvailableVoucher.Include, _
                                                                   VoucherInfoModel.AvailableQuota.Include)

                        udtVoucherInfo.GetInfo(dtmServiceDate, udtSchemeClaim, udtEHSPersonalInfo, udtSelectedPractice.ServiceCategoryCode)

                        udtEHSAccount.VoucherInfo = udtVoucherInfo

                        udtEHSAccountMaintBLL.EHSAccountSaveToSession(udtEHSAccount, FunctionCode)

                        If udtEHSAccount.VoucherInfo.GetAvailableVoucher() > 0 Then
                            blnNotAvailableForClaim = False
                        End If
                        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

                    Case SchemeClaimModel.EnumControlType.CIVSS, SchemeClaimModel.EnumControlType.EVSS, SchemeClaimModel.EnumControlType.HSIVSS, _
                         SchemeClaimModel.EnumControlType.RVP, SchemeClaimModel.EnumControlType.PIDVSS, SchemeClaimModel.EnumControlType.VSS, _
                         SchemeClaimModel.EnumControlType.ENHVSSO, SchemeClaimModel.EnumControlType.PPP

                        Dim udtEHSClaimVaccine As EHSClaimVaccine.EHSClaimVaccineModel = Nothing
                        Dim blnNeedCreateVaccine As Boolean = False

                        udtEHSClaimVaccine = Me.udtSessionHandlerBLL.EHSClaimVaccineGetFromSession(FunctionCode)

                        If IsNothing(udtEHSClaimVaccine) OrElse Not udtEHSClaimVaccine.SchemeCode.Equals(strSchemeCode) Then
                            blnNeedCreateVaccine = True
                        End If

                        'Search available subsidy of Vaccine with different scheme 
                        Select Case udtSchemeClaim.ControlType
                            Case SchemeClaimModel.EnumControlType.HSIVSS, _
                                 SchemeClaimModel.EnumControlType.RVP, _
                                 SchemeClaimModel.EnumControlType.VSS, _
                                 SchemeClaimModel.EnumControlType.ENHVSSO, _
                                 SchemeClaimModel.EnumControlType.PPP

                                '--------------------
                                ' With Category
                                '--------------------
                                Dim udtClaimCategorys As ClaimCategory.ClaimCategoryModelCollection
                                Dim udtPersonalInformation As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode)
                                Dim strEnableClaimCategory As String = String.Empty

                                '--------------------------------------
                                'Part 1: Retrieve Claim Category
                                '--------------------------------------
                                'Retrieve Claim Category
                                Dim udtClaimCategoryBLL As ClaimCategory.ClaimCategoryBLL = New ClaimCategory.ClaimCategoryBLL()
                                udtClaimCategorys = udtClaimCategoryBLL.getDistinctCategoryBySchemeOnly(udtSchemeClaim)

                                'Assign Claim Category List to control
                                Me.udInputEHSClaim.ClaimCategorys = udtClaimCategorys

                                '--------------------------------------
                                'Part 2.1: Search Vaccine (RVP,HSIVSS)
                                '--------------------------------------
                                If udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.RVP) Then
                                    udtCommonFunction.getSytemParameterByParameterNameSchemeCode("RVPEnableClaimCategory", strEnableClaimCategory, String.Empty, strSchemeCode)
                                End If


                                If strEnableClaimCategory = "Y" OrElse udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.HSIVSS) Then

                                    If Not udtClaimCategory Is Nothing AndAlso udtClaimCategory.SchemeCode = udtSchemeClaim.SchemeCode.Trim() Then
                                        'Category has been selected
                                        If blnNeedCreateVaccine Then
                                            udtInputPicker = New InputPickerModel
                                            udtInputPicker.CategoryCode = udtClaimCategory.CategoryCode
                                            udtEHSClaimVaccine = Me.udtEHSClaimBLL.SearchEHSClaimVaccine(udtSchemeClaim, udtEHSAccount.SearchDocCode, udtEHSAccount, dtmServiceDate, True, udtInputPicker)
                                        End If

                                    Else
                                        'Category not selected or categrory is not for this scheme
                                        If udtClaimCategorys.Count = 1 Then
                                            udtClaimCategory = udtClaimCategorys(0)
                                            Me.udtSessionHandlerBLL.ClaimCategorySaveToSession(udtClaimCategory, FunctionCode)

                                            If blnNeedCreateVaccine Then
                                                udtInputPicker = New InputPickerModel
                                                udtInputPicker.CategoryCode = udtClaimCategory.CategoryCode
                                                udtEHSClaimVaccine = Me.udtEHSClaimBLL.SearchEHSClaimVaccine(udtSchemeClaim, udtEHSAccount.SearchDocCode, udtEHSAccount, dtmServiceDate, True, udtInputPicker)
                                            End If

                                        Else
                                            'Scheme Change 
                                            '1) Remove category
                                            '2) no vaccine
                                            Me.udtSessionHandlerBLL.ClaimCategoryRemoveFromSession(FunctionCode)
                                            Me.udtSessionHandlerBLL.EHSClaimVaccineRemoveFromSession(FunctionCode)
                                            udtEHSClaimVaccine = Nothing
                                        End If

                                    End If

                                    '----------------------------------------------------------------------
                                    'Check Claim Category list
                                    '----------------------------------------------------------------------
                                    If Not udtClaimCategorys Is Nothing AndAlso udtClaimCategorys.Count > 0 Then
                                        blnNoCategory = False
                                    Else
                                        isEligibleForClaim = False
                                        Me.udtSessionHandlerBLL.ClaimCategoryRemoveFromSession(FunctionCode)
                                    End If

                                ElseIf strEnableClaimCategory = "N" Then
                                    'For RVP ONLY
                                    udtClaimCategory = udtClaimCategorys.FilterByCategoryCode(udtSchemeClaim.SchemeCode, "RESIDENT")

                                    Me.udtSessionHandlerBLL.ClaimCategorySaveToSession(udtClaimCategory, FunctionCode)

                                    If blnNeedCreateVaccine Then
                                        udtInputPicker = New InputPickerModel
                                        udtInputPicker.CategoryCode = udtClaimCategory.CategoryCode
                                        udtEHSClaimVaccine = Me.udtEHSClaimBLL.SearchEHSClaimVaccine(udtSchemeClaim, udtEHSAccount.SearchDocCode, udtEHSAccount, dtmServiceDate, True, udtInputPicker)
                                    End If

                                    blnNoCategory = False
                                End If

                                '--------------------------------------
                                'Part 2.2: Search Vaccine (VSS, ENHVSSO, PPP)
                                '--------------------------------------
                                If udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.VSS) OrElse _
                                   udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.ENHVSSO) OrElse _
                                   udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.PPP) Then

                                    If Not udtClaimCategory Is Nothing AndAlso udtClaimCategory.SchemeCode = udtSchemeClaim.SchemeCode.Trim() Then
                                        'Category has been selected
                                        If blnNeedCreateVaccine Then
                                            udtInputPicker = New InputPickerModel
                                            udtInputPicker.CategoryCode = udtClaimCategory.CategoryCode
                                            udtEHSClaimVaccine = Me.udtEHSClaimBLL.SearchEHSClaimVaccine(udtSchemeClaim, udtEHSAccount.SearchDocCode, udtEHSAccount, dtmServiceDate, True, udtInputPicker)
                                        End If

                                    Else
                                        'Category not selected or categrory is not for this scheme
                                        If udtClaimCategorys.Count = 1 Then
                                            udtClaimCategory = udtClaimCategorys(0)
                                            Me.udtSessionHandlerBLL.ClaimCategorySaveToSession(udtClaimCategory, FunctionCode)

                                            If blnNeedCreateVaccine Then
                                                udtInputPicker = New InputPickerModel
                                                udtInputPicker.CategoryCode = udtClaimCategory.CategoryCode
                                                udtEHSClaimVaccine = Me.udtEHSClaimBLL.SearchEHSClaimVaccine(udtSchemeClaim, udtEHSAccount.SearchDocCode, udtEHSAccount, dtmServiceDate, True, udtInputPicker)
                                            End If

                                        Else
                                            'Scheme Change 
                                            '1) Remove category
                                            '2) no vaccine
                                            Me.udtSessionHandlerBLL.ClaimCategoryRemoveFromSession(FunctionCode)
                                            Me.udtSessionHandlerBLL.EHSClaimVaccineRemoveFromSession(FunctionCode)
                                            udtEHSClaimVaccine = Nothing
                                        End If

                                    End If

                                    '----------------------------------------------------------------------
                                    'Check Claim Category list
                                    '----------------------------------------------------------------------
                                    If Not udtClaimCategorys Is Nothing AndAlso udtClaimCategorys.Count > 0 Then
                                        blnNoCategory = False
                                    Else
                                        ' Me.udcMsgBoxInfo.AddMessage(New SystemMessage("990000", "E", "00106"))
                                        isEligibleForClaim = False
                                        Me.udtSessionHandlerBLL.ClaimCategoryRemoveFromSession(FunctionCode)
                                    End If

                                End If

                                '------------------------------------------------------------
                                ' Part 3: Determine whether it is available for claim
                                '------------------------------------------------------------
                                If Not udtEHSClaimVaccine Is Nothing AndAlso Not blnNoCategory AndAlso Not udtClaimCategory Is Nothing Then
                                    If Not udtEHSClaimVaccine.SubsidizeList Is Nothing Then
                                        'Check if no vaccine is avaliable for the recipient -> change "noAvailableForClaim" to false
                                        For Each udtEHSClaimSubsidize As EHSClaimVaccine.EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
                                            If udtEHSClaimSubsidize.Available Then
                                                blnNotAvailableForClaim = False
                                                Exit For
                                            End If
                                        Next
                                    Else
                                        udtEHSClaimVaccine = Nothing
                                        blnNotAvailableForClaim = True
                                    End If

                                ElseIf Not blnNoCategory Then
                                    blnNotAvailableForClaim = False

                                Else
                                    blnNotAvailableForClaim = True

                                End If

                            Case Else
                                '--------------------
                                ' Without Category
                                '--------------------
                                'For EVSS and CIVSS

                                blnNoCategory = False
                                'Default
                                If blnNeedCreateVaccine Then
                                    udtEHSClaimVaccine = Me.udtEHSClaimBLL.SearchEHSClaimVaccine(udtSchemeClaim, udtEHSAccount.SearchDocCode, udtEHSAccount, dtmServiceDate, True, Nothing)
                                End If

                                '----------------------------------------------------------------------
                                'Check Vaccine is available for Claim
                                '----------------------------------------------------------------------
                                If Not udtEHSClaimVaccine Is Nothing Then
                                    If Not udtEHSClaimVaccine.SubsidizeList Is Nothing Then
                                        'Check if no vaccine is avaliable for the recipient -> change "noAvailableForClaim" to false
                                        For Each udtEHSClaimSubsidize As EHSClaimVaccine.EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
                                            If udtEHSClaimSubsidize.Available Then
                                                blnNotAvailableForClaim = False
                                                Exit For
                                            End If
                                        Next
                                    Else
                                        udtEHSClaimVaccine = Nothing
                                        ' No available subsidize for Claim
                                        ' Case 1: Not Eligiblity
                                        ' Case 2: Out of period
                                        ' Case 3: The subsidizes is used
                                        For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtSchemeClaim.SubsidizeGroupClaimList
                                            If udtSubsidizeGroupClaim.LastServiceDtm >= dtmServiceDate Then
                                                isEligibleForClaim = False
                                            End If
                                        Next
                                    End If
                                Else
                                    ' No available subsidize for Claim
                                    ' Case 1: Not Eligiblity
                                    ' Case 2: Out of period
                                    ' Case 3: The subsidizes is used
                                    For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtSchemeClaim.SubsidizeGroupClaimList
                                        If udtSubsidizeGroupClaim.LastServiceDtm >= dtmServiceDate Then
                                            isEligibleForClaim = False
                                        End If
                                    Next
                                End If
                        End Select

                        Me.udtSessionHandlerBLL.EHSClaimVaccineSaveToSession(udtEHSClaimVaccine, FunctionCode)

                        Me.udInputEHSClaim.EHSClaimVaccine = udtEHSClaimVaccine

                        AddHandler Me.udInputEHSClaim.VaccineLegendClicked, AddressOf udcInputEHSClaim_VaccineLegendClick

                    Case SchemeClaimModel.EnumControlType.EHAPP
                        blnNotAvailableForClaim = False

                End Select

                Me.udcMessageBox.Clear()
                Me.SetSaveButtonEnable(Me.ibtnEnterClaimDetailSave, True)

                'Bulid Vaccine Input Control
                Me.udInputEHSClaim.AvaliableForClaim = True
                Me.udInputEHSClaim.CurrentPractice = Me.udtSessionHandlerBLL.PracticeDisplayGetFromSession(FunctionCode)
                Me.udInputEHSClaim.SchemeType = udtSchemeClaim.SchemeCode.Trim()
                Me.udInputEHSClaim.EHSAccount = udtEHSAccount
                Me.udInputEHSClaim.EHSTransaction = Me.udtSessionHandlerBLL.EHSTransactionGetFromSession(FunctionCode)
                Me.udInputEHSClaim.TableTitleWidth = 200
                Me.udInputEHSClaim.ServiceDate = dtmServiceDate
                Me.udInputEHSClaim.FunctionCode = FunctionCode
                Me.udInputEHSClaim.ShowLegend = False
                Me.udInputEHSClaim.NonClinic = Me.udtSessionHandlerBLL.NonClinicSettingGetFromSession(FunctionCode)

                Me.udInputEHSClaim.Built(blnPostbackRebuild)

                Select Case udtSchemeClaim.ControlType
                    Case SchemeClaimModel.EnumControlType.HSIVSS, SchemeClaimModel.EnumControlType.RVP, SchemeClaimModel.EnumControlType.VSS, _
                         SchemeClaimModel.EnumControlType.ENHVSSO, SchemeClaimModel.EnumControlType.PPP

                        If udtClaimCategory Is Nothing OrElse blnNotAvailableForClaim OrElse blnNoCategory Then
                            Me.SetSaveButtonEnable(Me.ibtnEnterClaimDetailSave, False)
                        Else
                            Me.SetSaveButtonEnable(Me.ibtnEnterClaimDetailSave, True)
                        End If

                End Select
                ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

            End If
        End If
    End Sub

    Private Sub SetSaveButtonEnable(ByVal btnSave As ImageButton, ByVal blnEnable As Boolean)
        btnSave.Enabled = blnEnable
        If blnEnable Then
            btnSave.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "SaveBtn")
        Else
            btnSave.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "SaveDisableBtn")
        End If
        btnSave.AlternateText = Me.GetGlobalResourceObject("AlternateText", "SaveBtn")
    End Sub

    Private Sub udcInputEHSClaim_VaccineLegendClick(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    End Sub

    Private Sub udInputEHSClaim_CategorySelected(ByVal sender As Object, ByVal e As System.EventArgs) Handles udInputEHSClaim.CategorySelected
        Dim udtSchemeClaim As SchemeClaimModel = Me.udtSessionHandlerBLL.SelectSchemeGetFromSession(FunctionCode)
        Dim udtFormatter As Formatter = New Formatter
        Dim strCategory As String = Nothing

        ' Reset Error Message when Category changed
        Me.udcMessageBox.Clear()
        Me.udcInfoMessageBox.Clear()

        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Select Case udtSchemeClaim.ControlType
            Case SchemeClaimModel.EnumControlType.HSIVSS
                Dim udcInputHSIVSS As ucInputHSIVSS = Me.udInputEHSClaim.GetHSIVSSControl()
                strCategory = udcInputHSIVSS.Category

            Case SchemeClaimModel.EnumControlType.RVP
                Dim udcInputRVP As ucInputRVP = Me.udInputEHSClaim.GetRVPControl()
                strCategory = udcInputRVP.Category

            Case SchemeClaimModel.EnumControlType.VSS
                Dim udcInputVSS As ucInputVSS = Me.udInputEHSClaim.GetVSSControl()
                strCategory = udcInputVSS.Category

            Case SchemeClaimModel.EnumControlType.ENHVSSO
                Dim udcInputENHVSSO As ucInputENHVSSO = Me.udInputEHSClaim.GetENHVSSOControl()
                strCategory = udcInputENHVSSO.Category

            Case SchemeClaimModel.EnumControlType.PPP
                Dim udcInputPPP As ucInputPPP = Me.udInputEHSClaim.GetPPPControl()
                strCategory = udcInputPPP.Category

        End Select
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        If String.IsNullOrEmpty(strCategory) Then
            Me.SetSaveButtonEnable(Me.ibtnEnterClaimDetailSave, False)
            Me.udtSessionHandlerBLL.ClaimCategoryRemoveFromSession(FunctionCode)
        Else
            Dim strServiceDate As String = udtFormatter.formatInputDate(Me.txtEnterClaimDetailServiceDate.Text)
            Dim dtmServiceDate As DateTime = udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English)
            Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
            Dim udtEHSAccount As EHSAccount.EHSAccountModel = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
            Dim udtPersonalInformation As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode)
            Dim udtClaimCategorys As ClaimCategory.ClaimCategoryModelCollection

            'udtClaimCategorys = Me._udtClaimCategoryBLL.getDistinctCategoryByScheme(udtSchemeClaim, udtPersonalInformation.DOB, udtPersonalInformation.ExactDOB, dtmServiceDate)

            Dim udtClaimCategoryBLL As New ClaimCategory.ClaimCategoryBLL
            'udtClaimCategorys = udtClaimCategoryBLL.getDistinctCategoryByScheme(udtSchemeClaim, udtPersonalInformation.DOB, udtPersonalInformation.ExactDOB, dtmServiceDate)
            udtClaimCategorys = udtClaimCategoryBLL.getDistinctCategoryBySchemeOnly(udtSchemeClaim)

            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            Me.udtSessionHandlerBLL.ClaimCategorySaveToSession(udtClaimCategorys.FilterByCategoryCode(udtSchemeClaim.SchemeCode, strCategory), FunctionCode)
            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]
            Me.udtSessionHandlerBLL.EHSClaimVaccineRemoveFromSession(FunctionCode)
            Me.udInputEHSClaim.ResetSchemeType()

            Me.SetUpEnterClaimDetails(False)

        End If

        Me.udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationFail)

    End Sub

    Private Sub udInputEHSClaim_ClaimControlEventFired(ByVal strSchemeCode As String, ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udInputEHSClaim.ClaimControlEventFired

        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Select Case strSchemeCode
            Case SchemeClaimModel.VSS, SchemeClaimModel.RVP
                Session(SESS_SearchRVPHomeList) = True
                Me.udcRVPHomeListSearch.BindRVPHomeList(Nothing)
                Me.udcRVPHomeListSearch.ClearFilter()

                Me.ibtnPopupRVPHomeListSearchSelect.Enabled = False
                Me.ibtnPopupRVPHomeListSearchSelect.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectDisableBtn")

                Me.ModalPopupExtenderRVPHomeListSearch.Show()

            Case SchemeClaimModel.PPP
                Session(SESS_SearchSchoolList) = True
                Me.udcSchoolListSearch.BindSchoolList(Nothing)
                Me.udcSchoolListSearch.ClearFilter()

                Me.ibtnPopupSchoolListSearchSelect.Enabled = False
                Me.ibtnPopupSchoolListSearchSelect.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectDisableBtn")

                Me.ModalPopupExtenderSchoolListSearch.Show()

            Case Else
                Throw New Exception(String.Format("No available popup for scheme({0}).", strSchemeCode))

        End Select

        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

    End Sub

    Protected Sub ibtnEnterClaimDetailSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        Dim udtEHSAccount As EHSAccount.EHSAccountModel
        Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
        udtEHSAccount = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)

        udtAuditLogEntry.AddDescripton("Scheme Code", Me.ddlEnterClaimDetailsSchemeText.SelectedValue.Trim)
        udtAuditLogEntry.AddDescripton("Service Date", Me.txtEnterClaimDetailServiceDate.Text.Trim)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00082, AuditLogDescription.NewClaimTransaction_EnterClaimDetail)

        ClearClaimControlErrorImage()

        Dim blnIsValid As Boolean = True
        Dim blnNeedOverrideReason As Boolean = False
        Dim udtEHSTransaction As EHSTransactionModel

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        'udtEHSTransaction = Me.udtSessionHandlerBLL.EHSTransactionGetFromSession(FunctionCode)
        Dim udtEHSTransactionWithoutTransactionDetail As EHSTransactionModel = Me.udtSessionHandlerBLL.EHSTransactionWithoutTransactionDetailGetFromSession(FunctionCode)

        udtEHSTransaction = New EHSTransactionModel(udtEHSTransactionWithoutTransactionDetail)
        udtEHSTransaction.EHSAcct = udtEHSTransactionWithoutTransactionDetail.EHSAcct
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]
        udtEHSTransaction.OverrideReason = String.Empty
        udtEHSTransaction.WarningMessage = Nothing

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL
        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

        Dim udtSchemeClaim As SchemeClaimModel
        udtSchemeClaim = Me.udtSessionHandlerBLL.SelectSchemeGetFromSession(FunctionCode)

        Dim strServiceDate As String = Me.udtFormatter.formatInputDate(Me.txtEnterClaimDetailServiceDate.Text.Trim)
        Dim dtmServiceDate As DateTime
        Me.imgEnterClaimDetailServiceDateErr.Visible = False

        Me.udtSM = udtValidator.chkServiceDate(Me.txtEnterClaimDetailServiceDate.Text.Trim)
        If Not Me.udtSM Is Nothing Then
            blnIsValid = False
            Me.imgEnterClaimDetailServiceDateErr.Visible = True
            Me.udcMessageBox.AddMessage(Me.udtSM)
        Else
            strServiceDate = udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English)
            If Not DateTime.TryParse(strServiceDate, dtmServiceDate) Then
                dtmServiceDate = udtFormatter.convertDate(Me.txtEnterClaimDetailServiceDate.Text.Trim, Common.Component.CultureLanguage.English)
            End If
            ' Me.txtEnterClaimDetailServiceDate.Text = strServiceDate
        End If

        If blnIsValid Then
            udtEHSTransaction.DocCode = udtEHSAccount.SearchDocCode
            udtEHSTransaction.SchemeCode = udtSchemeClaim.SchemeCode.Trim
            udtEHSTransaction.ServiceDate = dtmServiceDate

            Dim udtEHSClaimBLL As New BLL.EHSClaimBLL
            ' Check Service date with profession claim period
            Me.udtSM = udtEHSClaimBLL.CheckServiceDateClaimPeriod(udtEHSTransaction.SchemeCode, udtEHSTransaction.ServiceDate.AddDays(1).AddMinutes(-1))
            If Not Me.udtSM Is Nothing Then
                blnIsValid = False
                Me.imgEnterClaimDetailServiceDateErr.Visible = True
                Me.udcMessageBox.AddMessage(Me.udtSM)
            End If
        End If

        ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]
        ' -----------------------------------------------------------------------------------------
        If blnIsValid Then
            ' Check Service date with profession claim period
            Dim udtPractice As Practice.PracticeBLL.PracticeDisplayModel = Me.udtSessionHandlerBLL.PracticeDisplayGetFromSession(FunctionCode)

            If Not udtPractice.Profession.IsClaimPeriod(udtEHSTransaction.ServiceDate) Then
                blnIsValid = False
                Me.imgEnterClaimDetailServiceDateErr.Visible = True
                Me.udtSM = New Common.ComObject.SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00150) ' The "Service Date" should not be earlier than %s
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Me.udcMessageBox.AddMessage(Me.udtSM, "%s", udtFormatter.formatDate(udtPractice.Profession.ClaimPeriodFrom.Value, Me.udtSessionHandlerBLL.Language))
                Me.udcMessageBox.AddMessage(Me.udtSM, "%s", udtFormatter.formatDisplayDate(udtPractice.Profession.ClaimPeriodFrom.Value))
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            End If
        End If
        ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        If blnIsValid Then
            'Default value on HKIC symbol
            Me.imgErrHKICSymbol.Visible = False
            udtEHSTransaction.EHSAcct.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode).HKICSymbol = String.Empty
            udtEHSTransaction.HKICSymbol = String.Empty
            udtEHSTransaction.OCSSSRefStatus = String.Empty

            'If HKIC, go to validation
            If udtEHSTransaction.EHSAcct.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode).DocCode = DocTypeModel.DocTypeCode.HKIC Then
                'If HKIC symbol selection is shown, check symbol whether is inputted
                If panHKICSymbol.Visible = True Then
                    'Collect value from HKIC symbol selection
                    udtEHSTransaction.EHSAcct.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode).HKICSymbol = rblHKICSymbol.SelectedValue.Trim
                    udtEHSTransaction.HKICSymbol = rblHKICSymbol.SelectedValue.Trim
                    udtEHSTransaction.OCSSSRefStatus = (New Common.OCSSS.OCSSSResult(Common.OCSSS.OCSSSResult.OCSSSConnection.SkipForChecking, Nothing)).OCSSSStatus

                    'If no input, arise the warning
                    If rblHKICSymbol.SelectedValue = String.Empty Then
                        blnIsValid = False
                        Me.imgErrHKICSymbol.Visible = True

                        Me.udtSM = New Common.ComObject.SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00028) ' Please input "%s".
                        Me.udcMessageBox.AddMessage(Me.udtSM, "%s", lblHKICSymbolText.Text)
                    End If

                End If

            End If

        End If
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        If blnIsValid Then
            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
            ' --------------------------------------------------------------------------------------
            Select Case udtSchemeClaim.ControlType
                Case SchemeClaimModel.EnumControlType.CIVSS
                    blnIsValid = Me.CIVSSValidation(udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.EVSS
                    blnIsValid = Me.EVSSValidation(udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.VOUCHER
                    blnIsValid = Me.HCVSValidation(udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.VOUCHERCHINA
                    blnIsValid = Me.HCVSChinaValidation(udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.HSIVSS
                    blnIsValid = Me.HSIVSSValidation(udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.RVP
                    blnIsValid = Me.RVPValidation(udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.EHAPP
                    blnIsValid = Me.EHAPPValidation(udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.PIDVSS
                    blnIsValid = Me.PIDVSSValidation(udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.VSS
                    blnIsValid = Me.VSSValidation(udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.ENHVSSO
                    blnIsValid = Me.ENHVSSOValidation(udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.PPP
                    blnIsValid = Me.PPPValidation(udtEHSTransaction)

            End Select
            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        End If

        Dim udtEHSClaimVaccine As EHSClaimVaccine.EHSClaimVaccineModel
        udtEHSClaimVaccine = Me.udtSessionHandlerBLL.EHSClaimVaccineGetFromSession(FunctionCode)

        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Select Case udtSchemeClaim.ControlType
            Case SchemeClaimModel.EnumControlType.VOUCHER
                Me.AuditLogVoucher(udtAuditLogEntry, udtSchemeClaim.SchemeCode, dtmServiceDate)

            Case SchemeClaimModel.EnumControlType.VOUCHERCHINA
                Me.AuditLogChinaVoucher(udtAuditLogEntry, udtSchemeClaim.SchemeCode, dtmServiceDate)

            Case SchemeClaimModel.EnumControlType.CIVSS, SchemeClaimModel.EnumControlType.EVSS, SchemeClaimModel.EnumControlType.HSIVSS, _
                 SchemeClaimModel.EnumControlType.RVP, SchemeClaimModel.EnumControlType.PIDVSS, SchemeClaimModel.EnumControlType.VSS, _
                 SchemeClaimModel.EnumControlType.ENHVSSO, SchemeClaimModel.EnumControlType.PPP

                Me.AuditLogVaccination(udtAuditLogEntry, udtEHSClaimVaccine, dtmServiceDate, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.EHAPP
                Me.AuditLogEHAPP(udtAuditLogEntry, udtSchemeClaim.SchemeCode, dtmServiceDate)

        End Select
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        Dim udtValidationResults As EHSClaim.EHSClaimBLL.EHSClaimBLL.ValidationResults 'OutsideClaimValidation.OutsideClaimValidationModel
        Dim udtBlockMessage As EHSClaim.EHSClaimBLL.EHSClaimBLL.RuleResultList
        Dim udtWarningMessage As EHSClaim.EHSClaimBLL.EHSClaimBLL.RuleResultList

        If blnIsValid Then
            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim udtCommonEHSClaimBLL As New Common.Component.EHSClaim.EHSClaimBLL.EHSClaimBLL
            Dim udtHAVaccineResult As Common.WebService.Interface.HAVaccineResult = udtSessionHandlerBLL.CMSVaccineResultGetFromSession(FunctionCode)
            Dim udtDHVaccineResult As Common.WebService.Interface.DHVaccineResult = udtSessionHandlerBLL.CIMSVaccineResultGetFromSession(FunctionCode)

            'If nothing, get HA Vaccine through CMS 
            If udtHAVaccineResult Is Nothing Then
                Dim udtWSProxyCMS As New Common.WebService.Interface.WSProxyCMS(Me.udtAuditLogEntry)
                udtHAVaccineResult = udtWSProxyCMS.GetVaccine(udtEHSAccount)
                udtSessionHandlerBLL.CMSVaccineResultSaveToSession(udtHAVaccineResult, FunctionCode)
            End If

            'If nothing, get DH Vaccine through CIMS 
            If udtDHVaccineResult Is Nothing Then
                Dim udtWSProxyCIMS As New Common.WebService.Interface.WSProxyDHCIMS(Me.udtAuditLogEntry)
                udtDHVaccineResult = udtWSProxyCIMS.GetVaccine(udtEHSAccount)
                udtSessionHandlerBLL.CIMSVaccineResultSaveToSession(udtDHVaccineResult, FunctionCode)
            End If

            If udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVaccine Then
                '----------------
                ' Vaccine Type
                '----------------
                Dim udtTransactionBenefitDetailList As TransactionDetailVaccineModelCollection = Nothing
                'Get EHS Vaccine to Benefit List
                udtTransactionBenefitDetailList = udtEHSTransactionBLL.getTransactionDetailVaccine(udtEHSTransaction.DocCode, udtEHSAccount.getPersonalInformation(udtEHSTransaction.DocCode).IdentityNum)

                Dim objVaccinationBLL As New VaccinationBLL

                'Add HA Vaccine to Benefit List
                If objVaccinationBLL.SchemeContainVaccine(udtSchemeClaim) Then
                    If Not udtHAVaccineResult Is Nothing And Not udtDHVaccineResult Is Nothing Then
                        udtTransactionBenefitDetailList.JoinVaccineList(udtEHSAccount.getPersonalInformation(udtEHSTransaction.DocCode), udtHAVaccineResult.SinglePatient.VaccineList, udtAuditLogEntry, udtSchemeClaim.SchemeCode)
                        udtTransactionBenefitDetailList.JoinVaccineList(udtEHSAccount.getPersonalInformation(udtEHSTransaction.DocCode), udtDHVaccineResult.SingleClient.VaccineRecordList, udtAuditLogEntry, udtSchemeClaim.SchemeCode)
                    End If
                End If

                udtEHSTransaction.HAVaccineRefStatus = New EHSTransaction.EHSTransactionModel.ExtRefStatusClass(udtHAVaccineResult, udtEHSTransaction.DocCode).Code
                udtEHSTransaction.DHVaccineRefStatus = New EHSTransaction.EHSTransactionModel.ExtRefStatusClass(udtDHVaccineResult, udtEHSTransaction.DocCode).Code

                Dim dicVaccineRef As Dictionary(Of String, String) = EHSTransactionModel.GetVaccineRef(udtTransactionBenefitDetailList, udtEHSTransaction)
                udtEHSTransaction.EHSVaccineResult = dicVaccineRef(EHSTransactionModel.VaccineRefType.EHS)
                udtEHSTransaction.HAVaccineResult = dicVaccineRef(EHSTransactionModel.VaccineRefType.HA)
                udtEHSTransaction.DHVaccineResult = dicVaccineRef(EHSTransactionModel.VaccineRefType.DH)

                Me.udtEHSClaimBLL.ConstructEHSTransactionDetails(udtEHSTransaction, udtEHSAccount, Me.udtSessionHandlerBLL.EHSClaimVaccineGetFromSession(FunctionCode), udtHCVUUser.UserID)


            ElseIf udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeRegistration Then
                '-------------------
                ' Registration Type
                '-------------------
                Me.udtEHSClaimBLL.ConstructEHSTransDetail_Registration(udtEHSTransaction, udtEHSAccount, udtHCVUUser.UserID)

            Else
                '-------------------
                ' Voucher Type
                '-------------------
                ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                ' ----------------------------------------------------------
                udtEHSTransaction.PerVoucherValue = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVoucher, dtmServiceDate).SubsidizeFee
                ' CRE17-010 (OCSSS integration) [End][Chris YIM]
                Me.udtEHSClaimBLL.ConstructEHSTransactionDetails(udtEHSTransaction, udtEHSAccount, udtHCVUUser.UserID)
            End If

            udtValidationResults = udtCommonEHSClaimBLL.ValidateClaimCreation(EHSClaim.EHSClaimBLL.EHSClaimBLL.ClaimAction.HCVUClaim, udtEHSTransaction, udtHAVaccineResult, udtDHVaccineResult, udtAuditLogEntry)
            ' CRE18-001(CIMS Vaccination Sharing) [End][Chris YIM]

            udtBlockMessage = udtValidationResults.BlockResults
            For Each udtBlock As EHSClaim.EHSClaimBLL.EHSClaimBLL.RuleResult In udtBlockMessage.RuleResults
                Me.udcMessageBox.AddMessage(udtBlock.ErrorMessage)
                blnIsValid = False
            Next

            If blnIsValid Then
                udtWarningMessage = udtValidationResults.WarningResults
                If udtWarningMessage.RuleResults.Count > 0 Then
                    udtEHSTransaction.WarningMessage = udtWarningMessage
                    For Each udtWarning As EHSClaim.EHSClaimBLL.EHSClaimBLL.RuleResult In udtWarningMessage.RuleResults
                        'Me.udcMessageBox.AddMessage(udtOutsideClaimWarningMessage)
                        If Not IsNothing(udtWarning.MessageVariableNameArrayList) And Not IsNothing(udtWarning.MessageVariableNameArrayList) Then
                            Me.udcWarningMessageBox.AddMessage(udtWarning.ErrorMessage, udtWarning.MessageVariableNameArrayList.ToArray(Type.GetType("System.String")), udtWarning.MessageVariableValueArrayList.ToArray(Type.GetType("System.String")))
                        Else
                            Me.udcWarningMessageBox.AddMessage(udtWarning.ErrorMessage)
                        End If

                        blnNeedOverrideReason = True
                    Next
                End If
            End If
        End If

        If blnIsValid Then
            Me.udtSessionHandlerBLL.EHSTransactionRemoveFromSession(FunctionCode)
            Me.udtSessionHandlerBLL.EHSTransactionSaveToSession(udtEHSTransaction, FunctionCode)

            Me.udcConfirmClaimCreation.EnableVaccinationRecordChecking = False
            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Me.udcConfirmClaimCreation.ShowHKICSymbol = True
            Me.udcConfirmClaimCreation.ShowOCSSSCheckingResult = False
            ' CRE17-010 (OCSSS integration) [End][Chris YIM]
            Me.udcConfirmClaimCreation.LoadTranInfo(udtEHSTransaction, New DataTable())

            If blnNeedOverrideReason Then

                Me.txtConfirmClaimCreationOverrideReason.Text = String.Empty
                Me.imgConfirmClaimCreationOverrideReason.Visible = False

                If udtWarningMessage.RuleResults.Count > 6 Then
                    pnlWarningMsgContent.Attributes.Remove("style")
                    pnlWarningMsgContent.Attributes.Add("style", "height: 300px;overflow: auto;")
                Else
                    pnlWarningMsgContent.Attributes.Remove("style")
                    pnlWarningMsgContent.Attributes.Add("style", "height: auto;overflow: auto;")
                End If

                'udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationWarning, udtAuditLogEntry, LogID.LOG00064, AuditLogDescription.NewClaimTransaction_CreationDetailsClick)
                udcWarningMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationWarning, udtAuditLogEntry, LogID.LOG00094, AuditLogDescription.NewClaimTransaction_EnterClaimDetail_WarningMsg)
                Me.udcOverrideReasonMsgBox.Visible = False
                Me.ModalPopupExtenderWarningMessage.Show()


                udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
                udtAuditLogEntry.AddDescripton("Has Warning Msg", "Y")
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00083, AuditLogDescription.NewClaimTransaction_EnterClaimDetail_Success)


            Else

                Me.udcInfoMessageBox.AddMessage(New SystemMessage("990000", SeverityCode.SEVI, MsgCode.MSG00021))
                Me.udcInfoMessageBox.BuildMessageBox()
                Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
                Me.mvNewClaimTransaction.ActiveViewIndex = NewClaimTransaction.Confirm

                udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
                udtAuditLogEntry.AddDescripton("Has Warning Msg", "N")
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00083, AuditLogDescription.NewClaimTransaction_EnterClaimDetail_Success)

            End If
            'Me.mvEnterDetails.ActiveViewIndex = InputTransactionDetails.Confirm
        Else
            udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
            udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationFail, udtAuditLogEntry, LogID.LOG00084, AuditLogDescription.NewClaimTransaction_EnterClaimDetail_Fail)
        End If


    End Sub

    Protected Sub ibtnEnterClaimDetailBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        Me.udcMessageBox.Visible = False
        Me.udcInfoMessageBox.Visible = False

        Dim udtEHSAccount As EHSAccount.EHSAccountModel
        Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
        udtEHSAccount = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)


        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        ' Clear Inputted value if cancel claim
        Me.udInputEHSClaim.Clear()
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Koala]

        'CRE13-006 HCVS Ceiling [Start][Karl]
        Me.udInputEHSClaim.ClearErrorMessage()
        'CRE13-006 HCVS Ceiling [End][Karl]

        Me.mvNewClaimTransaction.ActiveViewIndex = NewClaimTransaction.EnterClaimDetails
        Me.mvEnterDetails.ActiveViewIndex = InputTransactionDetails.CreationDetails

        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00085, AuditLogDescription.NewClaimTransaction_EnterClaimDetail_Back)
    End Sub

    Protected Sub ibtnWarningMessageCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        Dim udtEHSAccount As EHSAccount.EHSAccountModel
        Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
        udtEHSAccount = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)

        Me.ModalPopupExtenderWarningMessage.Hide()

        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        ' Clear the saved Transaction Details and Transaction Additional Fields from the Transaction
        Dim udtEHSTransaction As EHSTransactionModel
        udtEHSTransaction = Me.udtSessionHandlerBLL.EHSTransactionGetFromSession(FunctionCode)

        If Not udtEHSTransaction.TransactionDetails Is Nothing Then
            udtEHSTransaction.TransactionDetails.Clear()
            udtEHSTransaction.TransactionDetails = Nothing
        End If

        If Not udtEHSTransaction.TransactionAdditionFields Is Nothing Then
            udtEHSTransaction.TransactionAdditionFields.Clear()
            udtEHSTransaction.TransactionAdditionFields = Nothing
        End If

        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        Me.udtSessionHandlerBLL.EHSTransactionSaveToSession(udtEHSTransaction, FunctionCode)

        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00089, AuditLogDescription.NewClaimTransaction_EnterOverrideReason_Cancel)
    End Sub

    Protected Sub ibtnWarningMessageConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        Dim udtEHSAccount As EHSAccount.EHSAccountModel
        Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
        udtEHSAccount = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
        udtAuditLogEntry.AddDescripton("Override Reason", txtConfirmClaimCreationOverrideReason.Text.Trim)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00086, AuditLogDescription.NewClaimTransaction_EnterOverrideReason)

        Me.udcOverrideReasonMsgBox.Visible = False
        Me.udcInfoMessageBox.Visible = False

        Dim blnError As Boolean = False

        imgConfirmClaimCreationOverrideReason.Visible = False
        If Me.txtConfirmClaimCreationOverrideReason.Text.Trim = String.Empty Then
            imgConfirmClaimCreationOverrideReason.Visible = True
            udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00015)
            Me.udcOverrideReasonMsgBox.AddMessage(udtSM)
            Me.ModalPopupExtenderWarningMessage.Show()
            blnError = True
        End If

        If Not blnError Then

            Dim udtEHSTransaction As EHSTransactionModel
            udtEHSTransaction = Me.udtSessionHandlerBLL.EHSTransactionGetFromSession(FunctionCode)
            udtEHSTransaction.OverrideReason = Me.txtConfirmClaimCreationOverrideReason.Text.Trim

            Me.udtSessionHandlerBLL.EHSTransactionSaveToSession(udtEHSTransaction, FunctionCode)

            Me.udcConfirmClaimCreation.ClearDocumentType()
            Me.udcConfirmClaimCreation.ClearEHSClaim()

            Me.udcConfirmClaimCreation.EnableVaccinationRecordChecking = False
            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Me.udcConfirmClaimCreation.ShowHKICSymbol = True
            Me.udcConfirmClaimCreation.ShowOCSSSCheckingResult = False
            ' CRE17-010 (OCSSS integration) [End][Chris YIM]
            Me.udcConfirmClaimCreation.LoadTranInfo(udtEHSTransaction, New DataTable(), True, True, True)

            Me.udcInfoMessageBox.AddMessage(New SystemMessage("990000", SeverityCode.SEVI, MsgCode.MSG00021))
            Me.udcInfoMessageBox.BuildMessageBox()
            Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information

            Me.mvNewClaimTransaction.ActiveViewIndex = NewClaimTransaction.Confirm

            udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00087, AuditLogDescription.NewClaimTransaction_EnterOverrideReason_Success)

        Else
            udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
            udcOverrideReasonMsgBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationFail, udtAuditLogEntry, LogID.LOG00088, AuditLogDescription.NewClaimTransaction_EnterOverrideReason_Fail)
        End If
    End Sub

    Protected Sub txtEnterClaimDetailServiceDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        Dim udtEHSAccount As EHSAccount.EHSAccountModel
        Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
        udtEHSAccount = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
        udtAuditLogEntry.AddDescripton("Service Date", Me.txtEnterClaimDetailServiceDate.Text.Trim)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00101, AuditLogDescription.NewClaimTransaction_ChangeServiceDate)

        Me.udcMessageBox.Visible = False
        Me.udcInfoMessageBox.Visible = False
        Me.imgEnterClaimDetailServiceDateErr.Visible = False

        Me.udInputEHSClaim.Clear()

        Dim blnIsValid As Boolean = True
        Dim blnDisableDDL As Boolean = False

        Dim strSelectedScheme As String = Me.ddlEnterClaimDetailsSchemeText.SelectedValue.Trim

        Dim strServiceDate As String = Me.udtFormatter.formatInputDate(Me.txtEnterClaimDetailServiceDate.Text.Trim)
        Dim dtmServiceDate As DateTime


        Me.udtSM = udtValidator.chkServiceDate(Me.txtEnterClaimDetailServiceDate.Text.Trim)
        If Not Me.udtSM Is Nothing Then
            blnIsValid = False
            Me.imgEnterClaimDetailServiceDateErr.Visible = True
            Me.udcMessageBox.AddMessage(Me.udtSM)
            blnDisableDDL = True
        Else
            strServiceDate = udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English)
            If DateTime.TryParse(strServiceDate, dtmServiceDate) Then
                dtmServiceDate = udtFormatter.convertDate(Me.txtEnterClaimDetailServiceDate.Text.Trim, Common.Component.CultureLanguage.English)
            Else
                blnIsValid = False
                Me.imgEnterClaimDetailServiceDateErr.Visible = True
                Me.udtSM = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00120)
                Me.udcMessageBox.AddMessage(Me.udtSM)
                blnDisableDDL = True
            End If
        End If

        Dim udtSchemeClaimList As SchemeClaimModelCollection = Nothing

        If blnIsValid Then
            Me.BindEnterClaimDetailsScheme()

            udtSchemeClaimList = Me.udtSessionHandlerBLL.SchemeListGetFromSession(FunctionCode)

            If blnIsValid Then
                If IsNothing(udtSchemeClaimList) Then
                    blnIsValid = False
                    Me.imgEnterClaimDetailServiceDateErr.Visible = True
                    Me.udtSM = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00279)
                    Me.udcMessageBox.AddMessage(Me.udtSM)
                    blnDisableDDL = True
                Else
                    If udtSchemeClaimList.Count = 0 Then
                        blnIsValid = False
                        Me.imgEnterClaimDetailServiceDateErr.Visible = True
                        Me.udtSM = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00279)
                        Me.udcMessageBox.AddMessage(Me.udtSM)
                        blnDisableDDL = True
                    End If
                End If
            End If
        End If

        If blnDisableDDL Then
            Me.ddlEnterClaimDetailsSchemeText.SelectedIndex = 0
            Me.ddlEnterClaimDetailsSchemeText.Enabled = False
            Me.SetSaveButtonEnable(Me.ibtnEnterClaimDetailSave, False)
        Else
            Me.ddlEnterClaimDetailsSchemeText.Enabled = True
        End If

        Me.ddlEnterClaimDetailsSchemeText_SelectedIndexChanged(Nothing, Nothing)

        If blnIsValid Then
            Me.udInputEHSClaim.ResetSchemeType()
            Me.udtSessionHandlerBLL.EHSClaimVaccineRemoveFromSession(FunctionCode)
            If Not IsNothing(udtSchemeClaimList.Filter(strSelectedScheme)) Then
                Me.ddlEnterClaimDetailsSchemeText.SelectedValue = strSelectedScheme

                'The session ClaimCategory has been removed as before when trigger dropdownlist of scheme
                Dim udtSchemeClaim As SchemeClaimModel = udtSchemeClaimList.Filter(strSelectedScheme)

                ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
                ' --------------------------------------------------------------------------------------
                Select Case udtSchemeClaim.ControlType
                    Case SchemeClaimModel.EnumControlType.VSS
                        Dim udcInputVSS As ucInputVSS = Me.udInputEHSClaim.GetVSSControl()

                        If Not udcInputVSS Is Nothing Then
                            udcInputVSS.ClearClaimDetail()
                        End If

                    Case SchemeClaimModel.EnumControlType.ENHVSSO
                        Dim udcInputENHVSSO As ucInputENHVSSO = Me.udInputEHSClaim.GetENHVSSOControl()

                        If Not udcInputENHVSSO Is Nothing Then
                            udcInputENHVSSO.ClearClaimDetail()
                        End If

                    Case SchemeClaimModel.EnumControlType.PPP
                        Dim udcInputPPP As ucInputPPP = Me.udInputEHSClaim.GetPPPControl()

                        If Not udcInputPPP Is Nothing Then
                            udcInputPPP.ClearClaimDetail()
                        End If

                End Select
                ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

                ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                ' ----------------------------------------------------------
                If udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode.Trim).DocCode = DocTypeModel.DocTypeCode.HKIC Then
                    EnableHKICSymbolRadioButtonList(True, strSelectedScheme, dtmServiceDate)
                End If
                ' CRE17-010 (OCSSS integration) [End][Chris YIM]
            End If

            'Me.udtSessionHandlerBLL.ClaimCategoryRemoveFromSession(FunctionCode)
            Me.SetUpEnterClaimDetails(False)

            'check exchange rate 
            Call CheckExchangeRateAbsence(dtmServiceDate)

            udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
            udtAuditLogEntry.AddDescripton("Service Date", Me.txtEnterClaimDetailServiceDate.Text.Trim)
            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00095, AuditLogDescription.NewClaimTransaction_ChangeServiceDate_Success)
        Else
            udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
            udtAuditLogEntry.AddDescripton("Service Date", Me.txtEnterClaimDetailServiceDate.Text.Trim)
            udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationFail, udtAuditLogEntry, LogID.LOG00096, AuditLogDescription.NewClaimTransaction_ChangeServiceDate_Fail)
        End If
    End Sub

#Region "Scheme Audit Log"
    'Voucher Scheme
    Private Sub AuditLogVoucher(ByRef udtAuditLogEntry As AuditLogEntry, ByVal strSchemecode As String, ByVal dtmServiceDate As Date)
        'udtAuditLogEntry.AddDescripton("Scheme Code", SchemeClaimModel.HCVS)

        Dim udtSchemeClaimBLL As New SchemeClaimBLL
        Dim udtSchemeDetailBLL As New SchemeDetails.SchemeDetailBLL
        Dim udtSchemeClaimModel As SchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(strSchemecode, dtmServiceDate.AddDays(1).AddMinutes(-1))
        Dim udtSubsidizeItemDetailList As SchemeDetails.SubsidizeItemDetailsModelCollection = udtSchemeDetailBLL.getSubsidizeItemDetails(udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeItemCode)

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        'udtAuditLogEntry.AddDescripton("Scheme Seq", udtSchemeClaimModel.SchemeSeq)
        udtAuditLogEntry.AddDescripton("Scheme Seq", udtSchemeClaimModel.SubsidizeGroupClaimList(0).SchemeSeq)
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        udtAuditLogEntry.AddDescripton("Subsidize Code", udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeCode)
        udtAuditLogEntry.AddDescripton("Subsidize Item Code", udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeItemCode)
        udtAuditLogEntry.AddDescripton("Available Item Code", udtSubsidizeItemDetailList(0).AvailableItemCode)

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]        
        Dim udcInputHCVS As ucInputHCVS = Me.udInputEHSClaim.GetHCVSControl

        udtAuditLogEntry.AddDescripton("Voucher Redeem", udcInputHCVS.VoucherRedeem)
        udtAuditLogEntry.AddDescripton("Copayment Fee", udcInputHCVS.CoPaymentFee)

        udtAuditLogEntry.AddDescripton("Reason_for_Visit_L1", udcInputHCVS.ReasonForVisitFirst)
        udtAuditLogEntry.AddDescripton("Reason_for_Visit_L2", udcInputHCVS.ReasonForVisitSecond)

        If String.IsNullOrEmpty(udcInputHCVS.ReasonForVisitSecondaryL1(1)) = False Then
            udtAuditLogEntry.AddDescripton("ReasonforVisit_S1_L1", udcInputHCVS.ReasonForVisitSecondaryL1(1))
        End If

        If String.IsNullOrEmpty(udcInputHCVS.ReasonForVisitSecondaryL2(1)) = False Then
            udtAuditLogEntry.AddDescripton("ReasonforVisit_S1_L2", udcInputHCVS.ReasonForVisitSecondaryL2(1))
        End If

        If String.IsNullOrEmpty(udcInputHCVS.ReasonForVisitSecondaryL1(2)) = False Then
            udtAuditLogEntry.AddDescripton("ReasonforVisit_S2_L1", udcInputHCVS.ReasonForVisitSecondaryL1(2))
        End If

        If String.IsNullOrEmpty(udcInputHCVS.ReasonForVisitSecondaryL2(2)) = False Then
            udtAuditLogEntry.AddDescripton("ReasonforVisit_S2_L2", udcInputHCVS.ReasonForVisitSecondaryL2(2))
        End If

        If String.IsNullOrEmpty(udcInputHCVS.ReasonForVisitSecondaryL1(3)) = False Then
            udtAuditLogEntry.AddDescripton("ReasonforVisit_S3_L1", udcInputHCVS.ReasonForVisitSecondaryL1(3))
        End If

        If String.IsNullOrEmpty(udcInputHCVS.ReasonForVisitSecondaryL2(3)) = False Then
            udtAuditLogEntry.AddDescripton("ReasonforVisit_S3_L2", udcInputHCVS.ReasonForVisitSecondaryL2(3))
        End If
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

    End Sub
    'CRE13-019-02 Extend HCVS to China [Start][Karl]
    'China Voucher Scheme
    Private Sub AuditLogChinaVoucher(ByRef udtAuditLogEntry As AuditLogEntry, ByVal strSchemecode As String, ByVal dtmServiceDate As Date)
        'udtAuditLogEntry.AddDescripton("Scheme Code", SchemeClaimModel.HCVS)

        Dim udtSchemeClaimBLL As New SchemeClaimBLL
        Dim udtSchemeDetailBLL As New SchemeDetails.SchemeDetailBLL
        Dim udtSchemeClaimModel As SchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(strSchemecode, dtmServiceDate.AddDays(1).AddMinutes(-1))
        Dim udtSubsidizeItemDetailList As SchemeDetails.SubsidizeItemDetailsModelCollection = udtSchemeDetailBLL.getSubsidizeItemDetails(udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeItemCode)

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        'udtAuditLogEntry.AddDescripton("Scheme Seq", udtSchemeClaimModel.SchemeSeq)
        udtAuditLogEntry.AddDescripton("Scheme Seq", udtSchemeClaimModel.SubsidizeGroupClaimList(0).SchemeSeq)
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        udtAuditLogEntry.AddDescripton("Subsidize Code", udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeCode)
        udtAuditLogEntry.AddDescripton("Subsidize Item Code", udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeItemCode)
        udtAuditLogEntry.AddDescripton("Available Item Code", udtSubsidizeItemDetailList(0).AvailableItemCode)


        Dim udcInputHCVSChina As ucInputHCVSChina = Me.udInputEHSClaim.GetHCVSChinaControl
        Dim udtSchemeClaim As SchemeClaimModel
        Dim udtExchangeRate As New ExchangeRate.ExchangeRateBLL
        Dim dblSubsidizeFee As Double
        Dim intVoucherRedeem As Integer

        udtSchemeClaim = udtSchemeClaimBLL.getAllDistinctSchemeClaim_WithEffectiveSubsidizeGroup(udcInputHCVSChina.ServiceDate).Filter(strSchemecode)
        If Not udtSchemeClaim Is Nothing Then
            dblSubsidizeFee = (udtSchemeClaim.SubsidizeGroupClaimList.Filter(udcInputHCVSChina.ServiceDate))(0).SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVoucher, udcInputHCVSChina.ServiceDate).SubsidizeFee
        End If

        udtAuditLogEntry.AddDescripton("ExchangeRate", udcInputHCVSChina.ExchangeRate)

        If String.IsNullOrEmpty(udcInputHCVSChina.VoucherRedeem) = False Then
            intVoucherRedeem = udcInputHCVSChina.VoucherRedeem
            udtAuditLogEntry.AddDescripton("Voucher amount claimed (in HKD)", intVoucherRedeem)
            udtAuditLogEntry.AddDescripton("Voucher amount claimed (HKD * subsidizeFee)", intVoucherRedeem * dblSubsidizeFee)

            If String.IsNullOrEmpty(udcInputHCVSChina.VoucherRedeemRMB) = False Then
                If Double.TryParse(udcInputHCVSChina.VoucherRedeemRMB, Nothing) = True Then
                    udtAuditLogEntry.AddDescripton("Voucher amount claimed (in RMB)", udcInputHCVSChina.VoucherRedeemRMB)
                    udtAuditLogEntry.AddDescripton("Voucher amount claimed (RMB * subsidizeFee)", udcInputHCVSChina.VoucherRedeemRMB * dblSubsidizeFee)
                Else
                    udtAuditLogEntry.AddDescripton("Voucher amount claimed (in RMB)", "Invalid character : " & udcInputHCVSChina.VoucherRedeemRMB)
                End If
            End If
        End If

        'If String.IsNullOrEmpty(udcInputHCVSChina.CoPaymentFee) = False Then
        '    If Decimal.TryParse(udcInputHCVSChina.CoPaymentFee, Nothing) Then
        '        udtAuditLogEntry.AddDescripton("CoPaymentFeeHKD", udtExchangeRate.CalculateRMBtoHKD(udcInputHCVSChina.CoPaymentFee, udcInputHCVSChina.ExchangeRate))            
        '    End If
        'Else
        '    udtAuditLogEntry.AddDescripton("CoPaymentFeeHKD", String.Empty)
        'End If

        udtAuditLogEntry.AddDescripton("CoPaymentFeeRMB", udcInputHCVSChina.CoPaymentFee)

        udtAuditLogEntry.AddDescripton("PaymentType", udcInputHCVSChina.PaymentType)

        udtAuditLogEntry.AddDescripton("Reason_for_Visit_L1", udcInputHCVSChina.ReasonForVisitFirst)
        'udtAuditLogEntry.AddDescripton("Reason_for_Visit_L2", udcInputHCVSChina.ReasonForVisitSecond)

        If String.IsNullOrEmpty(udcInputHCVSChina.ReasonForVisitSecondaryL1(1)) = False Then
            udtAuditLogEntry.AddDescripton("ReasonforVisit_S1_L1", udcInputHCVSChina.ReasonForVisitSecondaryL1(1))
        End If

        'If String.IsNullOrEmpty(udcInputHCVSChina.ReasonForVisitSecondaryL2(1)) = False Then
        '    udtAuditLogEntry.AddDescripton("ReasonforVisit_S1_L2", udcInputHCVSChina.ReasonForVisitSecondaryL2(1))
        'End If

        If String.IsNullOrEmpty(udcInputHCVSChina.ReasonForVisitSecondaryL1(2)) = False Then
            udtAuditLogEntry.AddDescripton("ReasonforVisit_S2_L1", udcInputHCVSChina.ReasonForVisitSecondaryL1(2))
        End If

        'If String.IsNullOrEmpty(udcInputHCVSChina.ReasonForVisitSecondaryL2(2)) = False Then
        '    udtAuditLogEntry.AddDescripton("ReasonforVisit_S2_L2", udcInputHCVSChina.ReasonForVisitSecondaryL2(2))
        'End If

        If String.IsNullOrEmpty(udcInputHCVSChina.ReasonForVisitSecondaryL1(3)) = False Then
            udtAuditLogEntry.AddDescripton("ReasonforVisit_S3_L1", udcInputHCVSChina.ReasonForVisitSecondaryL1(3))
        End If

        'If String.IsNullOrEmpty(udcInputHCVSChina.ReasonForVisitSecondaryL2(3)) = False Then
        '    udtAuditLogEntry.AddDescripton("ReasonforVisit_S3_L2", udcInputHCVSChina.ReasonForVisitSecondaryL2(3))
        'End If

    End Sub
    'CRE13-019-02 Extend HCVS to China [End][Karl]

    'Vaccination Scheme
    'CRE15-005-03 (New PIDVSS scheme) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    'Private Sub AuditLogVaccination(ByRef udtAuditLogEntry As AuditLogEntry, ByVal udtEHSClaimVaccineModel As EHSClaimVaccine.EHSClaimVaccineModel, ByVal dtmServiceDate As Date)
    Private Sub AuditLogVaccination(ByRef udtAuditLogEntry As AuditLogEntry, ByVal udtEHSClaimVaccineModel As EHSClaimVaccine.EHSClaimVaccineModel, ByVal dtmServiceDate As Date, ByVal udtEHSTransaction As EHSTransactionModel)
        'udtAuditLogEntry.AddDescripton("Scheme Code", SchemeClaimModel.EVSS)

        For Each udtSubsidize As EHSClaimVaccine.EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccineModel.SubsidizeList

            If udtSubsidize.Selected Then

                Dim udtSchemeClaimBLL As New SchemeClaimBLL
                Dim udtSchemeClaimModel As SchemeClaimModel = Me.udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSClaimVaccineModel.SchemeCode, dtmServiceDate.AddDays(1).AddMinutes(-1))

                If udtSubsidize.SubsidizeDetailList.Count = 1 Then
                    udtAuditLogEntry.AddDescripton("Scheme Seq", udtSchemeClaimModel.SubsidizeGroupClaimList.Filter(udtEHSClaimVaccineModel.SchemeCode, udtSubsidize.SubsidizeCode).SchemeSeq)
                    udtAuditLogEntry.AddDescripton("Subsidize Code", udtSubsidize.SubsidizeCode)
                    udtAuditLogEntry.AddDescripton("Subsidize Item Code", udtSubsidize.SubsidizeItemCode)
                    udtAuditLogEntry.AddDescripton("Available Item Code", udtSubsidize.SubsidizeDetailList(0).AvailableItemCode)


                ElseIf udtSubsidize.SubsidizeDetailList.Count > 1 Then
                    For Each udtSubsidizeDetail As EHSClaimVaccine.EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtSubsidize.SubsidizeDetailList
                        If udtSubsidizeDetail.Selected Then
                            udtAuditLogEntry.AddDescripton("Scheme Seq", udtSchemeClaimModel.SubsidizeGroupClaimList.Filter(udtEHSClaimVaccineModel.SchemeCode, udtSubsidize.SubsidizeCode).SchemeSeq)
                            udtAuditLogEntry.AddDescripton("Subsidize Code", udtSubsidize.SubsidizeCode)
                            udtAuditLogEntry.AddDescripton("Subsidize Item Code", udtSubsidize.SubsidizeItemCode)
                            udtAuditLogEntry.AddDescripton("Available Item Code", udtSubsidize.SubsidizeDetailList(0).AvailableItemCode)
                        End If
                    Next
                End If
            End If
        Next

        If Not udtEHSTransaction Is Nothing AndAlso Not udtEHSTransaction.TransactionAdditionFields Is Nothing Then
            For Each udtTransactAdditionfield As TransactionAdditionalFieldModel In udtEHSTransaction.TransactionAdditionFields
                'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                If udtTransactAdditionfield.AdditionalFieldValueDesc Is Nothing OrElse udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty Then
                    udtAuditLogEntry.AddDescripton(udtTransactAdditionfield.AdditionalFieldID, udtTransactAdditionfield.AdditionalFieldValueCode)
                Else
                    udtAuditLogEntry.AddDescripton(udtTransactAdditionfield.AdditionalFieldID, udtTransactAdditionfield.AdditionalFieldValueCode + " - " + udtTransactAdditionfield.AdditionalFieldValueDesc)
                End If
                'CRE16-002 (Revamp VSS) [End][Chris YIM]
            Next
        End If
    End Sub
    'CRE15-005-03 (New PIDVSS scheme) [End][Chris YIM]

    ' CRE13-001 - EHAPP [Start][Tommy L]
    ' -------------------------------------------------------------------------------------
    'EHAPP Scheme
    Private Sub AuditLogEHAPP(ByRef udtAuditLogEntry As AuditLogEntry, ByVal strSchemeCode As String, ByVal dtmServiceDate As Date)
        Dim udtSchemeClaimBLL As New SchemeClaimBLL
        Dim udtSchemeDetailBLL As New SchemeDetails.SchemeDetailBLL
        Dim udtSchemeClaimModel As SchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(strSchemeCode, dtmServiceDate.AddDays(1).AddMinutes(-1))
        Dim udtSubsidizeItemDetailList As SchemeDetails.SubsidizeItemDetailsModelCollection = udtSchemeDetailBLL.getSubsidizeItemDetails(udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeItemCode)
        Dim udcInputEHAPP As ucInputEHAPP = Me.udInputEHSClaim.GetEHAPPControl()

        udtAuditLogEntry.AddDescripton("Scheme Seq", udtSchemeClaimModel.SubsidizeGroupClaimList(0).SchemeSeq)
        udtAuditLogEntry.AddDescripton("Subsidize Code", udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeCode)
        udtAuditLogEntry.AddDescripton("Subsidize Item Code", udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeItemCode)
        udtAuditLogEntry.AddDescripton("Available Item Code", udtSubsidizeItemDetailList(0).AvailableItemCode)
        udtAuditLogEntry.AddDescripton("Co-payment Item No", udcInputEHAPP.CoPayment)
        'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]      
        Select Case udcInputEHAPP.CoPayment
            Case EHAPP_Copayment.VOUCHER
                udtAuditLogEntry.AddDescripton("HCV Amount", udcInputEHAPP.HCVAmount)
                udtAuditLogEntry.AddDescripton("Net Service Fee", udcInputEHAPP.NetServiceFee)

            Case Else

        End Select
        'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]      
    End Sub
    ' CRE13-001 - EHAPP [End][Tommy L]

#End Region

#Region "Scheme Validation"

    Private Function CIVSSValidation(ByRef udtEHSTransaction As EHSTransactionModel) As Boolean

        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = False

        Dim udcInputCIVSS As ucInputCIVSS = Me.udInputEHSClaim.GetCIVSSControl()
        Dim udtEHSClaimVaccine As EHSClaimVaccine.EHSClaimVaccineModel = Me.udtSessionHandlerBLL.EHSClaimVaccineGetFromSession(FunctionCode)

        udcInputCIVSS.SetDoseErrorImage(False)

        ' -----------------------------------------------
        ' UI Input Validation
        '------------------------------------------------
        udtEHSClaimVaccine = udcInputCIVSS.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
        Me.udtSessionHandlerBLL.EHSClaimVaccineSaveToSession(udtEHSClaimVaccine, FunctionCode)

        Dim udtSMList As Dictionary(Of SystemMessage, List(Of String()))
        isValid = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, udtSMList)
        If Not isValid Then
            udcInputCIVSS.SetDoseErrorImage(True)
            For Each kvp As KeyValuePair(Of SystemMessage, List(Of String())) In udtSMList
                If IsNothing(kvp.Value) Then
                    Me.udcMessageBox.AddMessage(kvp.Key)
                Else
                    Dim s As List(Of String())
                    s = kvp.Value
                    Me.udcMessageBox.AddMessage(kvp.Key, s(0), s(1))
                End If

            Next

        End If

        Return isValid

    End Function

    Private Function EVSSValidation(ByRef udtEHSTransaction As EHSTransactionModel)
        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = False

        Dim udcInputEVSS As ucInputEVSS = Me.udInputEHSClaim.GetEVSSControl()
        Dim udtEHSClaimVaccine As EHSClaimVaccine.EHSClaimVaccineModel = Me.udtSessionHandlerBLL.EHSClaimVaccineGetFromSession(FunctionCode)

        udcInputEVSS.SetDoseErrorImage(False)

        ' -----------------------------------------------
        ' UI Input Validation
        '------------------------------------------------
        udtEHSClaimVaccine = udcInputEVSS.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
        Me.udtSessionHandlerBLL.EHSClaimVaccineSaveToSession(udtEHSClaimVaccine, FunctionCode)

        Dim udtSMList As Dictionary(Of SystemMessage, List(Of String()))
        isValid = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, udtSMList)
        If Not isValid Then
            udcInputEVSS.SetDoseErrorImage(True)
            For Each kvp As KeyValuePair(Of SystemMessage, List(Of String())) In udtSMList
                If IsNothing(kvp.Value) Then
                    Me.udcMessageBox.AddMessage(kvp.Key)
                Else
                    Dim s As List(Of String())
                    s = kvp.Value
                    Me.udcMessageBox.AddMessage(kvp.Key, s(0), s(1))
                End If

            Next


        End If


        Return isValid

    End Function

    Private Function HCVSValidation(ByRef udtEHSTransaction As EHSTransactionModel)
        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = True
        Dim strDOB As String = String.Empty

        'Dim systemMessage As SystemMessage = Nothing

        Dim udcInputHCVS As ucInputHCVS = Me.udInputEHSClaim.GetHCVSControl
        Dim udtValidator As Validator = New Validator()

        Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL

        Dim udtEHSAccount As EHSAccount.EHSAccountModel = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        Dim udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtSchemeClaim As SchemeClaimModel = Me.udtSessionHandlerBLL.SelectSchemeGetFromSession(FunctionCode)

        Dim intAvailableVoucher As Integer = 0

        udcInputHCVS.SetReasonForVisitError(False)
        udcInputHCVS.SetVoucherredeemError(False)

        ' -----------------------------------------------
        ' UI Input Validation
        '------------------------------------------------
        'systemMessage = udtValidator.chkVoucherRedeem(udcInputHCVS.VoucherRedeem, udtEHSAccount.AvailableVoucher())
        'If Not systemMessage Is Nothing Then
        '    isValid = False
        '    udcInputHCVS.SetVoucherredeemError(True)
        '    Me.udcMessageBox.AddMessage(systemMessage)
        'End If


        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        If Not udcInputHCVS.Validate(True, Me.udcMessageBox) Then
            isValid = False
        End If

        'Dim intVoucherRedeem As Integer = 0

        ''if radio button selected index is more then 6 and voucher redeem is not entered
        'If udcInputHCVS.VoucherRedeem.Equals(String.Empty) Then
        '    systemMessage = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00122)
        '    isValid = False
        '    udcInputHCVS.SetVoucherredeemError(True)
        '    Me.udcMessageBox.AddMessage(systemMessage)
        'Else
        '    If Integer.TryParse(udcInputHCVS.VoucherRedeem, intVoucherRedeem) Then
        '        If intVoucherRedeem < 1 Then
        '            systemMessage = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00124)
        '            isValid = False
        '            udcInputHCVS.SetVoucherredeemError(True)
        '            Me.udcMessageBox.AddMessage(systemMessage)
        '        End If
        '    Else
        '        systemMessage = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00124)
        '        isValid = False
        '        udcInputHCVS.SetVoucherredeemError(True)
        '        Me.udcMessageBox.AddMessage(systemMessage)
        '    End If
        'End If

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Koala]

        '' ------------------------------------------------------------------
        '' Check Last Service Date of SubsidizeGroupClaim
        '' ------------------------------------------------------------------
        'systemMessage = udtValidator.chkServiceDataSubsidizeGroupLastServiceData(udtEHSTransaction.ServiceDate, udtSchemeClaim.SubsidizeGroupClaimList(0))
        'If Not systemMessage Is Nothing Then
        '    isValid = False
        '    Me.imgStep2aServiceDateError.Visible = True
        '    Me.udcMsgBoxErr.AddMessage(systemMessage)
        'End If

        If isValid Then

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            udcInputHCVS.Save(udtEHSTransaction)
            ' -----------------------------------------------
            ' Set up Transaction Model: Addition Fields
            '------------------------------------------------
            'udtEHSTransaction.VoucherClaim = udcInputHCVS.VoucherRedeem
            'udtEHSTransaction.UIInput = udcInputHCVS.UIInput

            '' Reason For Visit Level1
            'Dim udtTransactAdditionfield As TransactionAdditionalFieldModel = New TransactionAdditionalFieldModel()
            'udtTransactAdditionfield.AdditionalFieldID = "Reason_for_Visit_L1"
            'udtTransactAdditionfield.AdditionalFieldValueCode = udcInputHCVS.ReasonForVisitFirst
            'udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
            'udtTransactAdditionfield.SchemeCode = SchemeClaimModel.HCVS
            'udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SchemeSeq
            'udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode
            'udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

            '' Reason For Visit Level2
            'udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            'udtTransactAdditionfield.AdditionalFieldID = "Reason_for_Visit_L2"
            'udtTransactAdditionfield.AdditionalFieldValueCode = udcInputHCVS.ReasonForVisitSecond
            'udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
            'udtTransactAdditionfield.SchemeCode = SchemeClaimModel.HCVS
            'udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SchemeSeq
            'udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode
            'udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Koala]
        End If

        Return isValid
    End Function
    'CRE13-019-02 Extend HCVS to China [Start][Karl]
    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
    Private Function HCVSChinaValidation(ByRef udtEHSTransaction As EHSTransactionModel)
        'CRE13-019-02 Extend HCVS to China [End][Karl]
        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = True
        Dim strDOB As String = String.Empty

        'CRE13-019-02 Extend HCVS to China [Start][Karl]
        Dim udcInputHCVSChina As ucInputHCVSChina = Me.udInputEHSClaim.GetHCVSChinaControl
        'CRE13-019-02 Extend HCVS to China [Start][Karl]

        Dim udtValidator As Validator = New Validator()

        Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL

        Dim udtEHSAccount As EHSAccount.EHSAccountModel = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        Dim udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtSchemeClaim As SchemeClaimModel = Me.udtSessionHandlerBLL.SelectSchemeGetFromSession(FunctionCode)

        Dim intAvailableVoucher As Integer = 0
        'CRE13-019-02 Extend HCVS to China [Start][Karl]
        udcInputHCVSChina.SetVoucherredeemError(False)
        'CRE13-019-02 Extend HCVS to China [End][Karl]
        ' -----------------------------------------------
        ' UI Input Validation
        '------------------------------------------------
        'CRE13-019-02 Extend HCVS to China [Start][Karl]
        If Not udcInputHCVSChina.Validate(True, Me.udcMessageBox) Then
            isValid = False
        End If

        If isValid Then
            udcInputHCVSChina.Save(udtEHSTransaction)
        End If
        'CRE13-019-02 Extend HCVS to China [End][Karl]
        Return isValid
    End Function
    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

    Private Function HSIVSSValidation(ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
        ' ---------------------------------------------
        ' Init
        '----------------------------------------------

        Dim isValid As Boolean = True
        Dim noDoseSelected As Boolean = True
        Dim dtmServiceDate As Date = udtEHSTransaction.ServiceDate


        Dim udtSchemeDetailBLL As New Common.Component.SchemeDetails.SchemeDetailBLL()
        Dim udcInputHSIVSS As ucInputHSIVSS = Me.udInputEHSClaim.GetHSIVSSControl()
        Dim udtEHSClaimVaccine As EHSClaimVaccine.EHSClaimVaccineModel = Me.udtSessionHandlerBLL.EHSClaimVaccineGetFromSession(FunctionCode)
        Dim udtEHSClaimSubsidize As EHSClaimVaccine.EHSClaimVaccineModel.EHSClaimSubsidizeModel = udtEHSClaimVaccine.SubsidizeList(0)

        Dim udtSchemeClaim As SchemeClaimModel = Me.udtSessionHandlerBLL.SelectSchemeGetFromSession(FunctionCode)
        Dim udtClaimCategory As ClaimCategory.ClaimCategoryModel = Me.udtSessionHandlerBLL.ClaimCategoryGetFromSession(FunctionCode)

        'Init Controls
        udcInputHSIVSS.SetPreConditionError(False)
        udcInputHSIVSS.SetDoseErrorImage(False)

        ' -----------------------------------------------
        ' UI Input Validation
        '------------------------------------------------

        If String.IsNullOrEmpty(udcInputHSIVSS.Category) Then
            isValid = False
            udcInputHSIVSS.SetCategoryError(True)
            Me.udtSM = New SystemMessage("990000", "E", "00238")
            Me.udcMessageBox.AddMessage(udtSM)
        Else
            If udtClaimCategory.IsMedicalCondition = "Y" AndAlso String.IsNullOrEmpty(udcInputHSIVSS.PreCondition) Then
                isValid = False
                udcInputHSIVSS.SetPreConditionError(True)
                Me.udtSM = New SystemMessage("990000", "E", "00196")
                Me.udcMessageBox.AddMessage(Me.udtSM)
            End If
        End If

        If isValid Then
            udtEHSClaimVaccine = udcInputHSIVSS.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
            Dim udtSMList As Dictionary(Of SystemMessage, List(Of String()))
            isValid = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, udtSMList)
            If Not isValid Then
                udcInputHSIVSS.SetDoseErrorImage(True)
                For Each kvp As KeyValuePair(Of SystemMessage, List(Of String())) In udtSMList
                    If IsNothing(kvp.Value) Then
                        Me.udcMessageBox.AddMessage(kvp.Key)
                    Else
                        Dim s As List(Of String())
                        s = kvp.Value
                        Me.udcMessageBox.AddMessage(kvp.Key, s(0), s(1))
                    End If
                Next

            End If
        End If

        If isValid Then

            Dim udtTransactAdditionfield As TransactionAdditionalFieldModel
            udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()

            '-------------------------------------------------
            ' Set up Transaction Model Addition Fields : Category
            '-------------------------------------------------
            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            udtEHSTransaction.CategoryCode = udcInputHSIVSS.Category
            'CRE16-002 (Revamp VSS) [End][Chris YIM]



            ' -----------------------------------------------
            ' Set up Transaction Model Addition Fields : PreCondition
            '------------------------------------------------
            If udtClaimCategory.IsMedicalCondition = "Y" Then
                udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                udtTransactAdditionfield.AdditionalFieldID = "PreCondition"
                udtTransactAdditionfield.AdditionalFieldValueCode = udcInputHSIVSS.PreCondition
                udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                'udtTransactAdditionfield.SchemeCode = SchemeClaimModel.HSIVSS
                'udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SchemeSeq
                udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
                udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
                udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode.Trim()
                udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
            End If

        End If

        Return isValid
    End Function

    Private Function RVPValidation(ByRef udtehstransaction As EHSTransactionModel) As Boolean

        ' ---------------------------------------------
        ' Init
        '----------------------------------------------

        Dim isValid As Boolean = True
        Dim noVaccineSelected As Boolean = True
        Dim noDoseSelected As Boolean = True
        Dim dtmServiceDate As Date = udtehstransaction.ServiceDate

        Dim udtSchemeDetailBLL As New Common.Component.SchemeDetails.SchemeDetailBLL()
        Dim udcInputRVP As ucInputRVP = Me.udInputEHSClaim.GetRVPControl()
        Dim udtEHSClaimVaccine As EHSClaimVaccine.EHSClaimVaccineModel = Me.udtSessionHandlerBLL.EHSClaimVaccineGetFromSession(FunctionCode)
        Dim udtGeneralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
        udtEHSClaimVaccine = udcInputRVP.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)

        Dim udtSchemeClaim As SchemeClaimModel = Me.udtSessionHandlerBLL.SelectSchemeGetFromSession(FunctionCode)

        Dim strEnableClaimCategory As String = Nothing
        udtGeneralFunction.getSytemParameterByParameterNameSchemeCode("RVPEnableClaimCategory", strEnableClaimCategory, String.Empty, SchemeClaimModel.RVP)

        'Init Controls
        udcInputRVP.SetRCHCodeError(False)
        udcInputRVP.SetCategoryError(False)
        udcInputRVP.SetDoseErrorImage(False)

        ' -----------------------------------------------
        ' UI Input Validation
        '------------------------------------------------

        'Claim Detial Part & Vaccine Part
        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        isValid = udcInputRVP.Validate(True, Me.udcMessageBox, strEnableClaimCategory)

        If isValid Then
            udcInputRVP.Save(udtehstransaction, udtEHSClaimVaccine, strEnableClaimCategory)
        End If
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        Return isValid

    End Function

    ' CRE13-001 - EHAPP [Start][Tommy L]
    ' -------------------------------------------------------------------------------------
    Private Function EHAPPValidation(ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = True

        Dim udcInputEHAPP As ucInputEHAPP = Me.udInputEHSClaim.GetEHAPPControl()

        ' -----------------------------------------------
        ' UI Input Validation
        '------------------------------------------------
        If Not udcInputEHAPP.Validate(Me.udcMessageBox) Then
            isValid = False
        End If

        If isValid Then
            udcInputEHAPP.Save(udtEHSTransaction)
        End If

        Return isValid
    End Function
    ' CRE13-001 - EHAPP [End][Tommy L]

    'CRE15-005-03 (New PIDVSS scheme) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Function PIDVSSValidation(ByRef udtEHSTransaction As EHSTransactionModel) As Boolean

        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = False

        Dim udcInputPIDVSS As ucInputPIDVSS = Me.udInputEHSClaim.GetPIDVSSControl()
        Dim udtEHSClaimVaccine As EHSClaimVaccine.EHSClaimVaccineModel = Me.udtSessionHandlerBLL.EHSClaimVaccineGetFromSession(FunctionCode)

        udcInputPIDVSS.SetDoseErrorImage(False)

        ' -----------------------------------------------
        ' UI Input Validation
        '------------------------------------------------
        Dim isValidDetail, isValidVaccineSelection As Boolean

        'Claim Detial Part
        If Not udcInputPIDVSS.Validate(True, Me.udcMessageBox) Then
            isValidDetail = False
        Else
            isValidDetail = True
        End If

        'Select Vaccine Part
        udtEHSClaimVaccine = udcInputPIDVSS.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
        Me.udtSessionHandlerBLL.EHSClaimVaccineSaveToSession(udtEHSClaimVaccine, FunctionCode)

        Dim udtSMList As Dictionary(Of SystemMessage, List(Of String())) = Nothing
        isValidVaccineSelection = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, udtSMList)
        If Not isValidVaccineSelection Then
            udcInputPIDVSS.SetDoseErrorImage(True)
        End If

        'Combine Result
        isValid = isValidDetail And isValidVaccineSelection

        If Not isValid Then
            For Each kvp As KeyValuePair(Of SystemMessage, List(Of String())) In udtSMList
                If IsNothing(kvp.Value) Then
                    Me.udcMessageBox.AddMessage(kvp.Key)
                Else
                    Dim s As List(Of String())
                    s = kvp.Value
                    Me.udcMessageBox.AddMessage(kvp.Key, s(0), s(1))
                End If

            Next

        End If

        If isValid Then
            udcInputPIDVSS.Save(udtEHSTransaction, udtEHSClaimVaccine)
        End If

        Return isValid

    End Function
    'CRE15-005-03 (New PIDVSS scheme) [End][Chris YIM]

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Function VSSValidation(ByRef udtEHSTransaction As EHSTransactionModel) As Boolean

        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = False

        Dim udcInputVSS As ucInputVSS = Me.udInputEHSClaim.GetVSSControl()
        Dim udtEHSClaimVaccine As EHSClaimVaccine.EHSClaimVaccineModel = Me.udtSessionHandlerBLL.EHSClaimVaccineGetFromSession(FunctionCode)

        udcInputVSS.SetDoseErrorImage(False)

        ' -----------------------------------------------
        ' UI Input Validation
        '------------------------------------------------
        Dim isValidDetail, isValidVaccineSelection As Boolean

        'Claim Detial Part
        If Not udcInputVSS.Validate(True, Me.udcMessageBox) Then
            isValidDetail = False
        Else
            isValidDetail = True
        End If

        'Select Vaccine Part
        udtEHSClaimVaccine = udcInputVSS.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
        Me.udtSessionHandlerBLL.EHSClaimVaccineSaveToSession(udtEHSClaimVaccine, FunctionCode)

        Dim udtSMList As Dictionary(Of SystemMessage, List(Of String())) = Nothing
        isValidVaccineSelection = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, udtSMList)
        If Not isValidVaccineSelection Then
            udcInputVSS.SetDoseErrorImage(True)
        End If

        'Combine Result
        isValid = isValidDetail And isValidVaccineSelection

        If Not isValid Then
            For Each kvp As KeyValuePair(Of SystemMessage, List(Of String())) In udtSMList
                If IsNothing(kvp.Value) Then
                    Me.udcMessageBox.AddMessage(kvp.Key)
                Else
                    Dim s As List(Of String())
                    s = kvp.Value
                    Me.udcMessageBox.AddMessage(kvp.Key, s(0), s(1))
                End If

            Next

        End If

        If isValid Then
            udcInputVSS.Save(udtEHSTransaction, udtEHSClaimVaccine)
        End If

        Return isValid

    End Function
    'CRE16-002 (Revamp VSS) [End][Chris YIM]

    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Function ENHVSSOValidation(ByRef udtEHSTransaction As EHSTransactionModel) As Boolean

        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = False

        Dim udcInputENHVSSO As ucInputENHVSSO = Me.udInputEHSClaim.GetENHVSSOControl()
        Dim udtEHSClaimVaccine As EHSClaimVaccine.EHSClaimVaccineModel = Me.udtSessionHandlerBLL.EHSClaimVaccineGetFromSession(FunctionCode)

        udcInputENHVSSO.SetDoseErrorImage(False)

        ' -----------------------------------------------
        ' UI Input Validation
        '------------------------------------------------
        Dim isValidDetail, isValidVaccineSelection As Boolean

        'Claim Detial Part
        If Not udcInputENHVSSO.Validate(True, Me.udcMessageBox) Then
            isValidDetail = False
        Else
            isValidDetail = True
        End If

        'Select Vaccine Part
        udtEHSClaimVaccine = udcInputENHVSSO.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
        Me.udtSessionHandlerBLL.EHSClaimVaccineSaveToSession(udtEHSClaimVaccine, FunctionCode)

        Dim udtSMList As Dictionary(Of SystemMessage, List(Of String())) = Nothing
        isValidVaccineSelection = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, udtSMList)
        If Not isValidVaccineSelection Then
            udcInputENHVSSO.SetDoseErrorImage(True)
        End If

        'Combine Result
        isValid = isValidDetail And isValidVaccineSelection

        If Not isValid Then
            For Each kvp As KeyValuePair(Of SystemMessage, List(Of String())) In udtSMList
                If IsNothing(kvp.Value) Then
                    Me.udcMessageBox.AddMessage(kvp.Key)
                Else
                    Dim s As List(Of String())
                    s = kvp.Value
                    Me.udcMessageBox.AddMessage(kvp.Key, s(0), s(1))
                End If

            Next

        End If

        If isValid Then
            udcInputENHVSSO.Save(udtEHSTransaction, udtEHSClaimVaccine)
        End If

        Return isValid

    End Function
    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Function PPPValidation(ByRef udtEHSTransaction As EHSTransactionModel) As Boolean

        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = False

        Dim udcInputPPP As ucInputPPP = Me.udInputEHSClaim.GetPPPControl()
        Dim udtEHSClaimVaccine As EHSClaimVaccine.EHSClaimVaccineModel = Me.udtSessionHandlerBLL.EHSClaimVaccineGetFromSession(FunctionCode)

        udcInputPPP.SetDoseErrorImage(False)

        ' -----------------------------------------------
        ' UI Input Validation
        '------------------------------------------------
        Dim isValidDetail, isValidVaccineSelection As Boolean

        'Claim Detial Part
        If Not udcInputPPP.Validate(True, Me.udcMessageBox) Then
            isValidDetail = False
        Else
            isValidDetail = True
        End If

        'Select Vaccine Part
        udtEHSClaimVaccine = udcInputPPP.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
        Me.udtSessionHandlerBLL.EHSClaimVaccineSaveToSession(udtEHSClaimVaccine, FunctionCode)

        Dim udtSMList As Dictionary(Of SystemMessage, List(Of String())) = Nothing

        isValidVaccineSelection = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, udtSMList)
        If Not isValidVaccineSelection Then
            udcInputPPP.SetDoseErrorImage(True)
        End If

        'Combine Result
        isValid = isValidDetail And isValidVaccineSelection

        If Not isValid Then
            For Each kvp As KeyValuePair(Of SystemMessage, List(Of String())) In udtSMList
                If IsNothing(kvp.Value) Then
                    Me.udcMessageBox.AddMessage(kvp.Key)
                Else
                    Dim s As List(Of String())
                    s = kvp.Value
                    Me.udcMessageBox.AddMessage(kvp.Key, s(0), s(1))
                End If

            Next

        End If

        If isValid Then
            udcInputPPP.Save(udtEHSTransaction, udtEHSClaimVaccine)
        End If

        Return isValid

    End Function
    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

#End Region

#Region "RVP Home List"
    Private Sub udcRVPHomeListSearch_RCHSelectedChanged(ByVal blnSelected As Boolean, ByVal sender As Object) Handles udcRVPHomeListSearch.RCHSelectedChanged
        If blnSelected Then
            Me.ibtnPopupRVPHomeListSearchSelect.Enabled = True
            Me.ibtnPopupRVPHomeListSearchSelect.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectBtn")
        Else
            Me.ibtnPopupRVPHomeListSearchSelect.Enabled = False
            Me.ibtnPopupRVPHomeListSearchSelect.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectDisableBtn")
        End If
    End Sub

    Protected Sub ibtnPopupRVPHomeListSearchCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Session.Remove(SESS_SearchRVPHomeList)
        Me.ModalPopupExtenderRVPHomeListSearch.Hide()
    End Sub

    Protected Sub ibtnPopupRVPHomeListSearchSelect_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim strRCHCode As String = Me.udcRVPHomeListSearch.getSelectedCode()

        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtSelectedScheme As SchemeClaimModel = udtSessionHandlerBLL.SelectSchemeGetFromSession(FunctionCode)

        Select Case udtSelectedScheme.SchemeCode
            Case SchemeClaimModel.EnumControlType.RVP.ToString.Trim
                CType(Me.udInputEHSClaim.GetRVPControl(), ucInputRVP).SetRCHCode(strRCHCode)

            Case SchemeClaimModel.EnumControlType.VSS.ToString.Trim
                CType(Me.udInputEHSClaim.GetVSSControl(), ucInputVSS).SetPIDCode(strRCHCode)

            Case Else

        End Select
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

        Session.Remove(SESS_SearchRVPHomeList)
        Me.ModalPopupExtenderRVPHomeListSearch.Hide()
    End Sub
#End Region

#Region "School List"

    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Private Sub udcSchoolListSearch_SchoolSelectedChanged(ByVal blnSelected As Boolean, ByVal sender As Object) Handles udcSchoolListSearch.SchoolSelectedChanged
        If blnSelected Then
            Me.ibtnPopupSchoolListSearchSelect.Enabled = True
            Me.ibtnPopupSchoolListSearchSelect.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectBtn")
        Else
            Me.ibtnPopupSchoolListSearchSelect.Enabled = False
            Me.ibtnPopupSchoolListSearchSelect.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectDisableBtn")
        End If
    End Sub

    Protected Sub ibtnPopupSchoolListSearchCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Session.Remove(SESS_SearchSchoolList)
        Me.ModalPopupExtenderSchoolListSearch.Hide()
    End Sub

    Protected Sub ibtnPopupSchoolListSearchSelect_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim strSchoolCode As String = Me.udcSchoolListSearch.GetSelectedCode()
        Dim udtSelectedScheme As SchemeClaimModel = udtSessionHandlerBLL.SelectSchemeGetFromSession(FunctionCode)

        Select Case udtSelectedScheme.SchemeCode
            Case SchemeClaimModel.EnumControlType.PPP.ToString.Trim
                CType(Me.udInputEHSClaim.GetPPPControl(), ucInputPPP).SetSchoolCode(strSchoolCode)

            Case Else
                Throw New Exception(String.Format("Invalid Scheme({0}.)", udtSelectedScheme.SchemeCode))
        End Select

        Session.Remove(SESS_SearchSchoolList)
        Me.ModalPopupExtenderSchoolListSearch.Hide()
    End Sub
    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

#End Region


#End Region

#Region "Confirm Claim Details"
    ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Protected Sub ibtnConfirmClaimCreationConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        Dim udtEHSAccount As EHSAccount.EHSAccountModel
        Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
        udtEHSAccount = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)

        Dim udtEHSTransaction As EHSTransactionModel
        udtEHSTransaction = Me.udtSessionHandlerBLL.EHSTransactionGetFromSession(FunctionCode)

        Dim udtHAVaccineResult As Common.WebService.Interface.HAVaccineResult
        udtHAVaccineResult = Me.udtSessionHandlerBLL.CMSVaccineResultGetFromSession(FunctionCode)

        Dim udtDHVaccineResult As Common.WebService.Interface.DHVaccineResult
        udtDHVaccineResult = Me.udtSessionHandlerBLL.CIMSVaccineResultGetFromSession(FunctionCode)

        Dim udtSchemeClaim As SchemeClaimModel
        udtSchemeClaim = Me.udtSessionHandlerBLL.SelectSchemeGetFromSession(FunctionCode)

        Dim strTransactionID As String = Me.udtCommonFunction.generateTransactionNumber(udtEHSTransaction.SchemeCode, True)

        udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
        udtAuditLogEntry.AddDescripton("Transaction ID", strTransactionID)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00090, AuditLogDescription.NewClaimTransaction_ConfirmClaim)

        Me.udcMessageBox.Visible = False
        Me.udcInfoMessageBox.Visible = False

        Dim blnValid As Boolean = False

        udtEHSTransaction.TransactionID = strTransactionID
        ' ----------------------------
        ' Set HA Vaccine Ref. Status
        ' ----------------------------
        Dim udtHAVaccineRefStatus As EHSTransaction.EHSTransactionModel.ExtRefStatusClass = Nothing
        If udtHAVaccineResult Is Nothing Then
            udtHAVaccineRefStatus = New EHSTransaction.EHSTransactionModel.ExtRefStatusClass()
        Else
            udtHAVaccineRefStatus = New EHSTransaction.EHSTransactionModel.ExtRefStatusClass(udtHAVaccineResult, udtEHSAccount.EHSPersonalInformationList(0).DocCode)
        End If

        udtHAVaccineRefStatus = EHSTransactionModel.ExtRefStatusClass.AmendExtRefStatus(udtSchemeClaim, udtHAVaccineRefStatus, VaccinationBLL.VaccineRecordProvider.HA)
        If udtHAVaccineRefStatus Is Nothing Then
            udtEHSTransaction.HAVaccineRefStatus = Nothing
        Else
            ' Change show flag if user no view vaccination record (HCVU available only)
            If ViewState(VS.VaccinationRecordPopupShown) Is Nothing Then
                udtHAVaccineRefStatus.ResultShown = EHSTransactionModel.ExtRefStatusClass.ResultShownEnum.No
            End If
            udtEHSTransaction.HAVaccineRefStatus = udtHAVaccineRefStatus.Code
        End If

        ' ----------------------------
        ' Set DH Vaccine Ref. Status
        ' ----------------------------
        Dim udtDHVaccineRefStatus As EHSTransaction.EHSTransactionModel.ExtRefStatusClass = Nothing
        If udtDHVaccineResult Is Nothing Then
            udtDHVaccineRefStatus = New EHSTransaction.EHSTransactionModel.ExtRefStatusClass()
        Else
            udtDHVaccineRefStatus = New EHSTransaction.EHSTransactionModel.ExtRefStatusClass(udtDHVaccineResult, udtEHSAccount.EHSPersonalInformationList(0).DocCode)
        End If

        udtDHVaccineRefStatus = EHSTransactionModel.ExtRefStatusClass.AmendExtRefStatus(udtSchemeClaim, udtDHVaccineRefStatus, VaccinationBLL.VaccineRecordProvider.DH)
        If udtDHVaccineRefStatus Is Nothing Then
            udtEHSTransaction.DHVaccineRefStatus = Nothing
        Else
            ' Change show flag if user no view vaccination record (HCVU available only)
            If ViewState(VS.VaccinationRecordPopupShown) Is Nothing Then
                udtDHVaccineRefStatus.ResultShown = EHSTransactionModel.ExtRefStatusClass.ResultShownEnum.No
            End If
            udtEHSTransaction.DHVaccineRefStatus = udtDHVaccineRefStatus.Code
        End If

        ' ----------------------------

        Dim udtWarningMessage As EHSClaim.EHSClaimBLL.EHSClaimBLL.RuleResultList = Nothing

        udtWarningMessage = udtEHSTransaction.WarningMessage

        If Not IsNothing(udtWarningMessage) Then
            udtEHSTransaction.OverrideReason = Me.txtConfirmClaimCreationOverrideReason.Text.Trim
        End If

        udtEHSTransaction.RecordStatus = EHSTransactionModel.TransRecordStatusClass.PendingApprovalForNonReimbursedClaim

        Try
            Me.udtSM = Me.udtEHSClaimBLL.CreateEHSTransaction(udtEHSTransaction, udtEHSAccount, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode.Trim), udtSchemeClaim)
            blnValid = True

        Catch eSql As SqlClient.SqlException
            If eSql.Number = 50000 Then
                blnValid = False

                ' CRE11-004
                udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
                udtAuditLogEntry.AddDescripton("Transaction ID", strTransactionID)
                Me.udcMessageBox.AddMessage(New SystemMessage("990001", "D", eSql.Message))
                Me.udcMessageBox.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00092, AuditLogDescription.NewClaimTransaction_ConfirmClaim_Fail)
            Else
                Throw eSql
            End If
        Catch ex As Exception
            Throw
        End Try

        'if Success
        If blnValid = True Then
            udtEHSTransaction = udtEHSTransactionBLL.LoadEHSTransaction(udtEHSTransaction.TransactionID)

            Dim strOld As String() = {"%s"}
            Dim strNew As String() = {""}

            strNew(0) = Me.udtFormatter.formatSystemNumber(udtEHSTransaction.TransactionID)

            Me.udcInfoMessageBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00008), strOld, strNew)
            Me.udcInfoMessageBox.BuildMessageBox()
            Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete

            Me.mvNewClaimTransaction.ActiveViewIndex = NewClaimTransaction.Complete

            udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
            udtAuditLogEntry.AddDescripton("Transaction ID", udtEHSTransaction.TransactionID)
            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00091, AuditLogDescription.NewClaimTransaction_ConfirmClaim_Success)

            'clear session
            ClearSessionForNewClaim(True)

        End If

    End Sub
    ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

    Protected Sub ibtnConfirmClaimCreationCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        Dim udtEHSAccount As EHSAccount.EHSAccountModel
        Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
        udtEHSAccount = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)

        Me.udcMessageBox.Visible = False
        Me.udcInfoMessageBox.Visible = False

        Dim udtEHSTransaction As EHSTransactionModel = Me.udtSessionHandlerBLL.EHSTransactionGetFromSession(FunctionCode)

        If Not IsNothing(udtEHSAccount) Then
            BindPersonalInfo(udtEHSAccount)
        End If

        Me.mvNewClaimTransaction.ActiveViewIndex = NewClaimTransaction.EnterClaimDetails
        Me.mvEnterDetails.ActiveViewIndex = InputTransactionDetails.ClaimDetails

        SetUpEnterClaimDetails(False)

        ' Clear the saved Transaction Details from the Transaction
        If Not udtEHSTransaction.TransactionDetails Is Nothing Then
            udtEHSTransaction.TransactionDetails.Clear()
            udtEHSTransaction.TransactionDetails = Nothing

        End If

        If Not udtEHSTransaction.TransactionAdditionFields Is Nothing Then
            udtEHSTransaction.TransactionAdditionFields.Clear()
            udtEHSTransaction.TransactionAdditionFields = Nothing
        End If

        Me.udtSessionHandlerBLL.EHSTransactionSaveToSession(udtEHSTransaction, FunctionCode)

        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00093, AuditLogDescription.NewClaimTransaction_ConfirmClaim_Cancel)

    End Sub

#End Region

#Region "Complete Claim Creation"
    Protected Sub ibtnCompleteClaimCreationReturn_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        ClearSearchCriteriaInput()

        Me.udcInfoMessageBox.Visible = False
        Me.MultiViewReimClaimTransManagement.ActiveViewIndex = ViewIndex.InputCriteria
    End Sub
#End Region

    Private Sub BindPersonalInfo(ByVal udtEHSAccount As EHSAccount.EHSAccountModel)
        udceHealthAccountInfo.Clear()
        udceHealthAccountInfo.DocumentType = udtEHSAccount.SearchDocCode.Trim
        udceHealthAccountInfo.MaskIdentityNo = True
        udceHealthAccountInfo.IsInvalidAccount = False
        udceHealthAccountInfo.EHSAccountModel = udtEHSAccount
        udceHealthAccountInfo.EHSPersonalInformation = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode.Trim)
        udceHealthAccountInfo.ShowAccountID = True
        udceHealthAccountInfo.ShowAccountStatus = True
        udceHealthAccountInfo.Build()

    End Sub

    Public Function getAllPractice(ByVal strSPID As String, ByVal enumPracticeDisplayType As Practice.PracticeBLL.PracticeDisplayType) As DataTable

        Dim udtPracticeBLL As New Practice.PracticeBLL
        ' Get Practice Information
        Dim dtPractice As DataTable = udtPracticeBLL.getRawAllPracticeBankAcct(strSPID)

        Practice.PracticeBLL.ConcatePracticeDisplayColumn(dtPractice, Practice.PracticeBLL.PracticeDisplayType.Practice)

        Return dtPractice
    End Function

    Private Sub mvNewClaimTransaction_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mvNewClaimTransaction.ActiveViewChanged
        If mvNewClaimTransaction.ActiveViewIndex <> NewClaimTransaction.Confirm And Me.mvNewClaimTransaction.ActiveViewIndex <> NewClaimTransaction.Complete Then
            Me.udcInfoMessageBox.Visible = False
            Me.udcMessageBox.Visible = False
        End If
    End Sub

    Private Sub mvEnterDetails_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mvEnterDetails.ActiveViewChanged
        Select Case mvEnterDetails.ActiveViewIndex
            Case InputTransactionDetails.CreationDetails
                ClearEnterCreationDetailsErrorImage()
        End Select
    End Sub

    Private Sub BindEnterClaimDetailsScheme()
        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL
        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

        Dim udtEHSTransaction As EHSTransactionModel

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        'udtEHSTransaction = Me.udtSessionHandlerBLL.EHSTransactionGetFromSession(FunctionCode)
        udtEHSTransaction = Me.udtSessionHandlerBLL.EHSTransactionWithoutTransactionDetailGetFromSession(FunctionCode)
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        Dim udtEHSAccount As EHSAccount.EHSAccountModel
        Dim udteHSAccountMaintBLL As New eHSAccountMaintBLL

        udtEHSAccount = udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)

        Dim udtSchemeClaimList As SchemeClaimModelCollection

        Dim strServiceDate As String = Me.udtFormatter.formatInputDate(Me.txtEnterClaimDetailServiceDate.Text.Trim)
        Dim dtmServiceDate As DateTime

        strServiceDate = udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English)
        If Not DateTime.TryParse(strServiceDate, dtmServiceDate) Then
            dtmServiceDate = udtFormatter.convertDate(Me.txtEnterClaimDetailServiceDate.Text.Trim, Common.Component.CultureLanguage.English)
        End If

        'Get all available List By Back Office User and SP's Practice
        udtSchemeClaimList = Me.udtSchemeClaimBLL.getSchemeClaimFromBackOfficeUserAndPractice(udtHCVUUser.UserID, FunctionCode, udtEHSTransaction.ServiceProviderID, udtEHSTransaction.PracticeID, dtmServiceDate)

        'CRE13-019-02 Extend HCVS to China [Start][Karl]
        Dim strMinDate As String
        Dim dtmMinDate As DateTime
        Dim udtCommfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction()
        Dim udtSystemMessage As SystemMessage
        Dim intMaxDateBackDate As Integer
        intMaxDateBackDate = DateDiff(DateInterval.Day, CDate(strServiceDate), DateTime.Now) + 1  'Back office platform do not restrict max date back day but display warning during claim

        'CRE13-019-02 Extend HCVS to China [End][Karl]

        Dim udtSchemeClaimResList As New SchemeClaimModelCollection
        For Each udtSchemeClaim As SchemeClaimModel In udtSchemeClaimList
            If udtSchemeClaim.SubsidizeGroupClaimList.Count > 0 Then
                'CRE13-019-02 Extend HCVS to China [Start][Karl]
                'To filter out scheme beyond min date back day
                dtmMinDate = Nothing
                strMinDate = Nothing
                udtSystemMessage = Nothing

                udtCommfunct.getSystemParameter("DateBackClaimMinDate", strMinDate, String.Empty, udtSchemeClaim.SchemeCode)
                dtmMinDate = Convert.ToDateTime(strMinDate)

                udtSystemMessage = udtValidator.chkDateBackClaimServiceDate(CDate(strServiceDate).ToString("dd-MM-yyyy"), _
                                                                            intMaxDateBackDate, dtmMinDate)

                If udtSystemMessage Is Nothing Then
                    udtSchemeClaimResList.Add(udtSchemeClaim)
                End If

                'CRE13-019-02 Extend HCVS to China [End][Karl]
            End If
        Next

        'Get all Eligible Scheme form available List
        'udtSchemeClaimList = Me.udtSchemeClaimBLL.searchEligibleClaimScheme(udtEHSAccount, udtEHSAccount.SearchDocCode, udtSchemeClaimList)

        Me.udtSessionHandlerBLL.SchemeListRemoveFromSession(FunctionCode)
        Me.udtSessionHandlerBLL.SchemeListSaveToSession(udtSchemeClaimResList, FunctionCode)

        Me.ddlEnterClaimDetailsSchemeText.DataSource = udtSchemeClaimResList 'udtSchemeClaimList
        Me.ddlEnterClaimDetailsSchemeText.DataTextField = "SchemeDesc"
        Me.ddlEnterClaimDetailsSchemeText.DataValueField = "SchemeCode"

        Me.ddlEnterClaimDetailsSchemeText.DataBind()

        ddlEnterClaimDetailsSchemeText.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), ""))

        ddlEnterClaimDetailsSchemeText.SelectedIndex = 0

        If udtSchemeClaimResList.Count = 0 Then
            ddlEnterClaimDetailsSchemeText.Enabled = False
        Else
            ddlEnterClaimDetailsSchemeText.Enabled = True
        End If

        'CRE15-004 (TIV and QIV) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Me.lblEnterClaimDetailsSchemeStatus.Visible = False
        Me.lblEnterClaimDetailsSchemeStatus.Text = String.Empty
        'CRE15-004 (TIV and QIV) [End][Chris YIM]
        Me.imgEnterClaimDetailServiceDateErr.Visible = False
        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Me.imgErrHKICSymbol.Visible = False
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        Me.udInputEHSClaim.Clear()
    End Sub

    Private Sub ClearSessionForNewClaim(ByVal blnClearSearchAccountList As Boolean)
        Me.udtSessionHandlerBLL.EHSClaimVaccineRemoveFromSession(FunctionCode)
        Me.udtSessionHandlerBLL.EHSTransactionRemoveFromSession(FunctionCode)
        Me.udtSessionHandlerBLL.PracticeDisplayListRemoveFromSession(FunctionCode)
        Me.udtSessionHandlerBLL.PracticeDisplayRemoveFromSession(FunctionCode)
        Me.udtSessionHandlerBLL.SchemeListRemoveFromSession(FunctionCode)
        Me.udtSessionHandlerBLL.SelectSchemeRemoveFromSession(FunctionCode)
        Me.udtSessionHandlerBLL.RVPRCHCodeRemoveFromSession(FunctionCode)
        Me.udtSessionHandlerBLL.ClaimCategoryRemoveFromSession(FunctionCode)

        Dim udteHSAccountMaintBLL As New eHSAccountMaintBLL
        udteHSAccountMaintBLL.EHSAccountRemoveFromSession(FunctionCode)

        Session.Remove(SESS_AdvancedSearchSP)
        Session.Remove(SESS_SearchRVPHomeList)
        Session.Remove(SESS_ServiceProvider)

        If blnClearSearchAccountList Then
            Session.Remove(SESS_SearchAccount)
        End If
    End Sub

    Private Sub GetReadyEHSAccount(ByVal strIdentityNum As String, ByVal strDocCode As String, ByVal blnClearSearchAccountList As Boolean)
        Dim udtEHSAccountBLL As New EHSAccount.EHSAccountBLL
        Dim udtEHSAccount As EHSAccount.EHSAccountModel

        udtEHSAccount = udtEHSAccountBLL.LoadEHSAccountByIdentity(strIdentityNum, strDocCode)


        'clear related session
        ClearSessionForNewClaim(blnClearSearchAccountList)

        'Bind the eHS Account Details        
        udtEHSAccount.SetSearchDocCode(strDocCode)

        Dim udteHSAccountMaintBLL As New eHSAccountMaintBLL

        udteHSAccountMaintBLL.EHSAccountSaveToSession(udtEHSAccount, FunctionCode)

        BindPersonalInfo(udtEHSAccount)

        BindCreationReason()
        Me.txtEnterCreationDetailRemarks.Text = String.Empty

        BindPaymentMethod()
        Me.txtEnterCreationDetailPaymentRemarks.Text = String.Empty

        txtEnterCreationDetailSPID.Text = String.Empty
        txtEnterCreationDetailSPID.Enabled = True

        Me.lblEnterCreationDetailSPName.Text = String.Empty
        Me.lblEnterCreationDetailSPStatus.Text = String.Empty
        Me.lblEnterCreationDetailPracticeStatus.Text = String.Empty

        Me.ibtnSearchSP.Enabled = True
        Me.ibtnClearSearchSP.Enabled = False

        Me.ibtnSearchSP.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchSBtn")
        Me.ibtnClearSearchSP.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ClearDisableSBtn")

        Me.ddlEnterCreationDetailPractice.Items.Clear()
        Me.ddlEnterCreationDetailPractice.Enabled = False

        Me.ibtnEnterCreationDetailNext.Enabled = False
        Me.ibtnEnterCreationDetailNext.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "NextDisableBtn")

        Me.mvNewClaimTransaction.ActiveViewIndex = NewClaimTransaction.EnterClaimDetails
        Me.mvEnterDetails.ActiveViewIndex = InputTransactionDetails.CreationDetails

    End Sub

    Private Sub GetReadyServiceProvider(ByVal strSPID)
        Dim udtSPBLL As New ServiceProvider.ServiceProviderBLL

        Dim udtSP As ServiceProvider.ServiceProviderModel

        udtSP = udtSPBLL.GetServiceProviderPermanentProfileWithMaintenanceBySPID(strSPID, New Common.DataAccess.Database)

        Me.lblEnterCreationDetailSPName.Text = udtSP.EnglishName
        If Not udtSP.RecordStatus.Trim.Equals(ServiceProviderStatus.Active) Then
            Status.GetDescriptionFromDBCode(ServiceProviderStatus.ClassCode, udtSP.RecordStatus, Me.lblEnterCreationDetailSPStatus.Text, String.Empty)

            Me.lblEnterCreationDetailSPStatus.Text = " (" + Me.lblEnterCreationDetailSPStatus.Text + ")"
        End If

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

        Me.ibtnEnterCreationDetailNext.Enabled = True
        Me.ibtnEnterCreationDetailNext.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "NextBtn")

        Me.txtEnterCreationDetailSPID.Text = udtSP.SPID
        Me.txtEnterCreationDetailSPID.Enabled = False

        Session(SESS_ServiceProvider) = udtSP
    End Sub

    Private Sub ClearEnterCreationDetailsErrorImage()
        imgEnterCreationDetailPractice.Visible = False
        imgEnterCreationDetailCreationReason.Visible = False
        imgEnterCreationDetailRemarks.Visible = False
        imgEnterCreationDetailPaymentMethod.Visible = False
        Me.imgEnterCreationDetailSPIDError.Visible = False
        Me.imgEnterCreationDetailPaymentRemarks.Visible = False
    End Sub

    Private Sub ClearClaimControlErrorImage()
        Dim udtSchemeClaim As SchemeClaimModel
        udtSchemeClaim = Me.udtSessionHandlerBLL.SelectSchemeGetFromSession(FunctionCode)

        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        If Not udtSchemeClaim Is Nothing Then
            Select Case udtSchemeClaim.ControlType
                Case SchemeClaimModel.EnumControlType.CIVSS
                    Dim udcInputCIVSS As ucInputCIVSS = Me.udInputEHSClaim.GetCIVSSControl()
                    If Not udcInputCIVSS Is Nothing Then
                        udcInputCIVSS.SetDoseErrorImage(False)
                    End If

                Case SchemeClaimModel.EnumControlType.EVSS
                    Dim udcInputEVSS As ucInputEVSS = Me.udInputEHSClaim.GetEVSSControl()
                    If Not udcInputEVSS Is Nothing Then
                        udcInputEVSS.SetDoseErrorImage(False)
                    End If
                Case SchemeClaimModel.EnumControlType.VOUCHER
                    Dim udcInputHCVS As ucInputHCVS = Me.udInputEHSClaim.GetHCVSControl
                    If Not udcInputHCVS Is Nothing Then
                        udcInputHCVS.SetReasonForVisitError(False)
                        udcInputHCVS.SetVoucherredeemError(False)
                        udcInputHCVS.SetCoPaymentFeeError(False)
                    End If

                Case SchemeClaimModel.EnumControlType.VOUCHERCHINA
                    Dim udcInputHCVSChina As ucInputHCVSChina = Me.udInputEHSClaim.GetHCVSChinaControl
                    If Not udcInputHCVSChina Is Nothing Then
                        udcInputHCVSChina.SetVoucherredeemError(False)
                    End If

                Case SchemeClaimModel.EnumControlType.HSIVSS
                    Dim udcInputHSIVSS As ucInputHSIVSS = Me.udInputEHSClaim.GetHSIVSSControl()
                    If Not udcInputHSIVSS Is Nothing Then
                        udcInputHSIVSS.SetPreConditionError(False)
                        udcInputHSIVSS.SetDoseErrorImage(False)
                    End If

                Case SchemeClaimModel.EnumControlType.RVP
                    Dim udcInputRVP As ucInputRVP = Me.udInputEHSClaim.GetRVPControl()
                    If Not udcInputRVP Is Nothing Then
                        udcInputRVP.SetRCHCodeError(False)
                        udcInputRVP.SetCategoryError(False)
                        udcInputRVP.SetDoseErrorImage(False)
                    End If

                Case SchemeClaimModel.EnumControlType.EHAPP
                    Dim udcInputEHAPP As ucInputEHAPP = Me.udInputEHSClaim.GetEHAPPControl()
                    If Not udcInputEHAPP Is Nothing Then
                        udcInputEHAPP.SetAllAlertVisible(False)
                    End If

                Case SchemeClaimModel.EnumControlType.PIDVSS
                    Dim udcInputPIDVSS As ucInputPIDVSS = Me.udInputEHSClaim.GetPIDVSSControl()
                    If Not udcInputPIDVSS Is Nothing Then
                        udcInputPIDVSS.SetDoseErrorImage(False)
                    End If

                Case SchemeClaimModel.EnumControlType.VSS
                    Dim udcInputVSS As ucInputVSS = Me.udInputEHSClaim.GetVSSControl()
                    If Not udcInputVSS Is Nothing Then
                        udcInputVSS.SetCategoryError(False)
                        udcInputVSS.SetDoseErrorImage(False)
                    End If

                Case SchemeClaimModel.EnumControlType.ENHVSSO
                    Dim udcInputENHVSSO As ucInputENHVSSO = Me.udInputEHSClaim.GetENHVSSOControl()
                    If Not udcInputENHVSSO Is Nothing Then
                        udcInputENHVSSO.SetCategoryError(False)
                        udcInputENHVSSO.SetDoseErrorImage(False)
                    End If

                Case SchemeClaimModel.EnumControlType.PPP
                    Dim udcInputPPP As ucInputPPP = Me.udInputEHSClaim.GetPPPControl()
                    If Not udcInputPPP Is Nothing Then
                        udcInputPPP.SetCategoryError(False)
                        udcInputPPP.SetSchoolCodeError(False)
                        udcInputPPP.SetDoseErrorImage(False)
                    End If

            End Select
        End If
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

    End Sub

#End Region

    Private Sub ClearSearchCriteriaInput(Optional ByVal intRetainInputAspect As Integer = -1)
        ' Transaction
        If intRetainInputAspect <> Aspect.Transaction Then
            Me.txtTabTransactionTransactionNo.Text = String.Empty
            Me.ddlTabTransactionHealthProfession.SelectedIndex = 0

            Me.rblTabTransactionTypeOfDate.SelectedIndex = 1
            Me.rblTabTransactionTypeOfDate.SelectedValue = TypeOfDate.TransactionDate
            Me.txtTabTransactionDateFrom.Text = String.Empty
            Me.txtTabTransactionDateTo.Text = String.Empty
            Me.ddlTabTransactionScheme.SelectedIndex = 0
            Me.ddlTabTransactionTransactionStatus.SelectedIndex = 0
            Me.ddlTabTransactionAuthorizedStatus.SelectedIndex = 0
            Me.ddlTabTransactionInvalidationStatus.SelectedIndex = 0
            Me.ddlTabTransactionInvalidationStatus.Enabled = True
            Me.ddlTabTransactionReimbursementMethod.SelectedIndex = 0
            Me.ddlTabTransactionMeansOfInput.SelectedIndex = 0
            Me.txtTabTransactionRCHRode.Text = String.Empty
            Me.txtTabTransactionRCHRode.Enabled = True
            Me.ddlTabTransactionVaccines.SelectedIndex = 0
            Me.ddlTabTransactionDose.SelectedIndex = 0
        End If

        ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
        ' Service Provider
        If intRetainInputAspect <> Aspect.ServiceProvider Then
            Me.txtTabServiceProviderSPID.Text = String.Empty
            Me.txtTabServiceProviderSPHKID.Text = String.Empty
            Me.txtTabServiceProviderSPName.Text = String.Empty
            Me.txtTabServiceProviderSPChiName.Text = String.Empty
            Me.txtTabServiceProviderBankAccountNo.Text = String.Empty

            Me.rblTabServiceProviderTypeOfDate.SelectedIndex = 1
            Me.rblTabServiceProviderTypeOfDate.SelectedValue = TypeOfDate.TransactionDate
            Me.txtTabServiceProviderDateFrom.Text = String.Empty
            Me.txtTabServiceProviderDateTo.Text = String.Empty
            Me.ddlTabServiceProviderScheme.SelectedIndex = 0
            Me.ddlTabServiceProviderTransactionStatus.SelectedIndex = 0
            Me.ddlTabServiceProviderAuthorizedStatus.SelectedIndex = 0
            Me.ddlTabServiceProviderInvalidationStatus.SelectedIndex = 0
            Me.ddlTabServiceProviderInvalidationStatus.Enabled = True
            Me.ddlTabServiceProviderReimbursementMethod.SelectedIndex = 0
            Me.ddlTabServiceProviderMeansOfInput.SelectedIndex = 0
            Me.txtTabServiceProviderRCHRode.Text = String.Empty
            Me.txtTabServiceProviderRCHRode.Enabled = True
        End If


        ' eHealth (Subsidies) Account
        If intRetainInputAspect <> Aspect.eHSAccount Then
            Me.ddlTabeHSAccountDocType.SelectedIndex = 0
            Me.txtTabeHSAccountDocNo.Text = String.Empty
            Me.txtTabeHSAccountID.Text = String.Empty
            Me.txtTabeHSAccountName.Text = String.Empty
            Me.txtTabeHSAccountChiName.Text = String.Empty

            Me.rblTabeHSAccountTypeOfDate.SelectedIndex = 1
            Me.rblTabeHSAccountTypeOfDate.SelectedValue = TypeOfDate.TransactionDate
            Me.txtTabeHSAccountDateFrom.Text = String.Empty
            Me.txtTabeHSAccountDateTo.Text = String.Empty
            Me.ddlTabeHSAccountScheme.SelectedIndex = 0
            Me.ddlTabeHSAccountTransactionStatus.SelectedIndex = 0
            Me.ddlTabeHSAccountAuthorizedStatus.SelectedIndex = 0
            Me.ddlTabeHSAccountInvalidationStatus.SelectedIndex = 0
            Me.ddlTabeHSAccountInvalidationStatus.Enabled = True
            Me.ddlTabeHSAccountReimbursementMethod.SelectedIndex = 0
            Me.ddlTabeHSAccountMeansOfInput.SelectedIndex = 0
            Me.txtTabeHSAccountRCHRode.Text = String.Empty
            Me.txtTabeHSAccountRCHRode.Enabled = True
        End If
        ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]

        ' Advanced Search
        If intRetainInputAspect <> Aspect.AdvancedSearch Then
            Me.txtTabAdvancedSearchTransactionNo.Text = String.Empty
            Me.ddlTabAdvancedSearchHealthProfession.SelectedIndex = 0

            Me.rblTabAdvancedSearchTypeOfDate.SelectedIndex = 1
            Me.rblTabAdvancedSearchTypeOfDate.SelectedValue = TypeOfDate.TransactionDate
            Me.txtTabAdvancedSearchDateFrom.Text = String.Empty
            Me.txtTabAdvancedSearchDateTo.Text = String.Empty
            Me.ddlTabAdvancedSearchScheme.SelectedIndex = 0
            Me.ddlTabAdvancedSearchTransactionStatus.SelectedIndex = 0
            Me.ddlTabAdvancedSearchAuthorizedStatus.SelectedIndex = 0
            Me.ddlTabAdvancedSearchInvalidationStatus.SelectedIndex = 0
            Me.ddlTabAdvancedSearchInvalidationStatus.Enabled = True
            Me.ddlTabAdvancedSearchReimbursementMethod.SelectedIndex = 0
            Me.ddlTabAdvancedSearchMeansOfInput.SelectedIndex = 0
            Me.txtTabAdvancedSearchRCHRode.Text = String.Empty
            Me.txtTabAdvancedSearchRCHRode.Enabled = True

            Me.txtTabAdvancedSearchSPID.Text = String.Empty
            Me.txtTabAdvancedSearchSPHKID.Text = String.Empty
            Me.txtTabAdvancedSearchSPName.Text = String.Empty
            Me.txtTabAdvancedSearchBankAccountNo.Text = String.Empty

            Me.ddlTabAdvancedSearchDocType.SelectedIndex = 0
            Me.txtTabAdvancedSearchDocNo.Text = String.Empty
            Me.txtTabAdvancedSearchAccountID.Text = String.Empty
        End If

    End Sub

#Region "Document Type help popup"

    Public Overrides Sub GridViewHeaderImage_Click(ByVal sender As Object, ByVal e As Common.Component.SortedGridviewHeader.SortedGridviewHeaderModel.GridViewHeaderImageEventArgs)
        popupDocTypeHelp.Show()
        udcDocTypeLegend.BindDocType(Session("language"))
    End Sub

    Protected Sub ibtnCloseDocTypeHelp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCloseDocTypeHelp.Click
        popupDocTypeHelp.Hide()
    End Sub

#End Region

#Region "Vaccination Record"

    Protected Sub ibtnVaccinationRecord_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00103, AuditLogDescription.VaccinationRecordClick)

        Dim udtEHSAccount As EHSAccountModel = (New eHSAccountMaintBLL).EHSAccountGetFromSession(FunctionCode)
        ucVaccinationRecord.Build(udtEHSAccount, New AuditLogEntry(FunctionCode, Me))

        Dim udtSession As New BLL.SessionHandlerBLL()
        udtSession.CMSVaccineResultSaveToSession(ucVaccinationRecord.HAVaccineResult, FunctionCode)
        udtSession.CIMSVaccineResultSaveToSession(ucVaccinationRecord.DHVaccineResult, FunctionCode)

        popupVaccinationRecord.Show()
        ViewState(VS.VaccinationRecordPopupStatus) = VaccinationRecordPopupStatusClass.Active
        ViewState(VS.VaccinationRecordPopupShown) = VaccinationRecordPopupShownClass.Active

    End Sub

    Protected Sub ibtnVaccinationRecordClose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00105, AuditLogDescription.VaccinationRecordCloseClick)

        ViewState.Remove(VS.VaccinationRecordPopupStatus)
        popupVaccinationRecord.Hide()

    End Sub

    ''' <summary>
    ''' Get EHS Vaccination record and CMS Vaccination record, and Join together by current claiming scheme (no cache)
    ''' </summary>
    ''' <param name="udtEHSAccount"></param>
    ''' <param name="strSchemeCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetVaccinationRecord(ByVal udtEHSAccount As EHSAccountModel, Optional ByVal strSchemeCode As String = "") As TransactionDetailVaccineModelCollection
        Dim udtTranDetailVaccineList As TransactionDetailVaccineModelCollection = Nothing
        Dim udtHAVaccineResult As Common.WebService.Interface.HAVaccineResult = Nothing
        Dim udtDHVaccineResult As Common.WebService.Interface.DHVaccineResult = Nothing
        'Dim htRecordSummary As Hashtable = Nothing
        Dim htRecordSummary As New Hashtable

        Dim udtVaccinationBLL As New VaccinationBLL

        Dim udtVaccineResultBag As New VaccineResultCollection
        udtVaccineResultBag.DHVaccineResult = udtDHVaccineResult
        udtVaccineResultBag.HAVaccineResult = udtHAVaccineResult

        udtVaccinationBLL.GetVaccinationRecord(udtEHSAccount, udtTranDetailVaccineList, udtVaccineResultBag, htRecordSummary, New AuditLogEntry(FunctionCode, Me), strSchemeCode)
        'Dim enumDHStatus As VaccinationBLL.EnumVaccinationRecordReturnStatus = udtVaccinationBLL.GetVaccinationRecord(udtEHSAccount, udtTranDetailVaccineList, udtDHVaccineResult, htRecordSummary, New AuditLogEntry(FunctionCode, Me), strSchemeCode, Nothing, False)

        Return udtTranDetailVaccineList
    End Function

    ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Public Function GetVaccinationRecordFromSession(ByVal udtEHSAccount As EHSAccountModel, Optional ByVal strSchemeCode As String = "") As VaccineResultCollection
        Dim udtVaccinationBLL As New VaccinationBLL
        Dim udtEHSTransactionBLL As New EHSTransactionBLL
        Dim udtSession As New BLL.SessionHandlerBLL

        Dim htRecordSummary As New Hashtable
        Dim udtTranDetailVaccineList As TransactionDetailVaccineModelCollection = Nothing

        'HA CMS
        Dim udtHAVaccineResult As HAVaccineResult = New HAVaccineResult(HAVaccineResult.enumReturnCode.Error)
        Dim udtHAVaccineResultSession As HAVaccineResult = udtSession.CMSVaccineResultGetFromSession(FunctionCode)

        'DH CIMS
        Dim udtDHVaccineResult As DHVaccineResult = New DHVaccineResult(DHVaccineResult.enumReturnCode.UnexpectedError)
        Dim udtDHVaccineResultSession As DHVaccineResult = udtSession.CIMSVaccineResultGetFromSession(FunctionCode)

        Dim udtVaccineResultBag As New VaccineResultCollection
        udtVaccineResultBag.DHVaccineResult = udtDHVaccineResult
        udtVaccineResultBag.HAVaccineResult = udtHAVaccineResult

        Dim udtVaccineResultBagSession As New VaccineResultCollection
        udtVaccineResultBagSession.DHVaccineResult = udtDHVaccineResultSession
        udtVaccineResultBagSession.HAVaccineResult = udtHAVaccineResultSession

        'Enquiry vaccine record
        udtVaccinationBLL.GetVaccinationRecord(udtEHSAccount, udtTranDetailVaccineList, udtVaccineResultBag, htRecordSummary, New AuditLogEntry(FunctionCode, Me), strSchemeCode, udtVaccineResultBagSession)

        'Save Vaccine Result to Session Variables
        udtSession.CMSVaccineResultSaveToSession(udtVaccineResultBag.HAVaccineResult, FunctionCode)
        udtSession.CIMSVaccineResultSaveToSession(udtVaccineResultBag.DHVaccineResult, FunctionCode)

        Return udtVaccineResultBag

    End Function
    ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

    ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Private Sub BuildSystemMessage(ByVal udtVaccineResultBag As VaccineResultCollection)
        Dim udtSystemMessage As SystemMessage = Nothing
        Dim udtSystemMessageList As New List(Of SystemMessage)
        Dim blnHAError As Boolean = False
        Dim blnDHError As Boolean = False
        Dim strLogID As String = String.Empty
        Dim strDescription As String = String.Empty

        If VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N Then
            If udtVaccineResultBag.HAVaccineResult Is Nothing Then
                If udtVaccineResultBag.HAReturnStatus = VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail Then
                    udtSystemMessageList.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00272))
                    strLogID = LogID.LOG00100
                    strDescription = AuditLogDescription.HAConnectionFail

                    blnHAError = True
                End If
            Else
                Select Case udtVaccineResultBag.HAReturnStatus
                    Case VaccinationBLL.EnumVaccinationRecordReturnStatus.DocumentNotAccept
                        'Nothing to do
                    Case Else
                        If udtVaccineResultBag.HAVaccineResult.ReturnCode <> HAVaccineResult.enumReturnCode.SuccessWithData Then
                            udtSystemMessageList.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00272))
                            strLogID = LogID.LOG00100
                            strDescription = AuditLogDescription.HAConnectionFail

                            blnHAError = True
                        End If
                End Select
            End If
        End If

        If VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CIMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N Then
            If udtVaccineResultBag.DHVaccineResult Is Nothing Then
                If udtVaccineResultBag.DHReturnStatus = VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail Then
                    udtSystemMessageList.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00411))
                    strLogID = LogID.LOG00110
                    strDescription = AuditLogDescription.DHConnectionFail

                    blnDHError = True
                End If
            Else
                Select Case udtVaccineResultBag.DHReturnStatus
                    Case VaccinationBLL.EnumVaccinationRecordReturnStatus.DocumentNotAccept
                        'Nothing to do
                    Case Else
                        If udtVaccineResultBag.DHVaccineResult.ReturnCode <> DHVaccineResult.enumReturnCode.Success Then
                            udtSystemMessageList.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00411))
                            strLogID = LogID.LOG00110
                            strDescription = AuditLogDescription.DHConnectionFail

                            blnDHError = True
                        End If
                End Select
            End If
        End If

        If blnHAError And blnDHError Then
            udtSystemMessageList.Clear()
            udtSystemMessageList.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00412))
            strLogID = LogID.LOG00111
            strDescription = AuditLogDescription.HADHConnectionFail
        End If

        For Each udtSystemMessage In udtSystemMessageList
            If Not udtSystemMessage Is Nothing Then
                Select Case udtSystemMessage.SeverityCode
                    Case "E"
                        udcMessageBox.AddMessage(udtSystemMessage)
                        udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ConnectionFail, udtAuditLogEntry, strLogID, strDescription)
                    Case Else
                        'Not to show MessageBox
                End Select
            End If
        Next
    End Sub
    ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

#End Region

#Region "Implement IWorkingData (CRE11-004)"

    Public Overrides Function GetDocCode() As String
        If GetEHSAccount() Is Nothing Then Return Nothing
        If GetEHSAccount.SearchDocCode <> String.Empty Then
            Return GetEHSAccount.SearchDocCode
        End If

        If GetEHSTransaction() IsNot Nothing Then
            Return GetEHSTransaction.DocCode
        End If

        Return Nothing
    End Function

    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Dim udtEHSAccount As EHSAccountModel = (New eHSAccountMaintBLL).EHSAccountGetFromSession(FunctionCode)
        If udtEHSAccount IsNot Nothing Then
            Return udtEHSAccount
        Else
            If GetEHSTransaction() IsNot Nothing Then
                Return GetEHSTransaction().EHSAcct
            Else
                Return Nothing
            End If
        End If

    End Function

    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Me.udtSessionHandlerBLL.EHSTransactionGetFromSession(FunctionCode)

    End Function

    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        If GetEHSTransaction() IsNot Nothing Then
            Me.GetReadyServiceProvider(GetEHSTransaction().ServiceProviderID)
        End If

        Return Session(SESS_ServiceProvider)

    End Function

#End Region

    ''' <summary>
    ''' CRE11-004
    ''' </summary>
    ''' <param name="dtTranSelected"></param>
    ''' <remarks></remarks>
    Protected Sub AuditLogAddDescriptonTranNo(ByVal objAuditLogEntry As AuditLogEntry, ByVal dtTranSelected As DataTable)
        For Each drSelected As DataRow In dtTranSelected.Rows
            objAuditLogEntry.AddDescripton("Transaction No", CStr(drSelected("transNum")).Trim)
        Next
    End Sub

    ''' <summary>
    ''' CRE11-004
    ''' </summary>
    ''' <param name="gvTransaction"></param>
    ''' <remarks></remarks>
    Protected Sub AuditLogAddDescriptonTranNo(ByVal objAuditLogEntry As AuditLogEntry, ByVal gvTransaction As GridView)
        For Each r As GridViewRow In gvTransaction.Rows
            If Not CType(r.FindControl("chk_selected"), CheckBox).Checked Then Continue For
            objAuditLogEntry.AddDescripton("Transaction No", CType(r.FindControl("hfTransactionNo"), HiddenField).Value.Trim)
        Next
    End Sub

    ''' <summary>
    ''' CRE11-004
    ''' </summary>
    ''' <param name="objAuditLogEntry"></param>
    ''' <remarks></remarks>
    Protected Sub AuditLogAddDescriptonBatchReason(ByVal objAuditLogEntry As AuditLogEntry)
        objAuditLogEntry.AddDescripton("Reason", txtBatchReason.Text)
    End Sub

    ''' <summary>
    ''' CRE11-004
    ''' </summary>
    ''' <param name="objAuditLogEntry"></param>
    ''' <remarks></remarks>
    Protected Sub AuditLogAddDescriptonConfirmReason(ByVal objAuditLogEntry As AuditLogEntry)
        objAuditLogEntry.AddDescripton("Reason", lblConfirmReason.Text)
    End Sub

    ''' <summary>
    ''' CRE11-004
    ''' </summary>
    ''' <param name="objAuditLogEntry"></param>
    ''' <remarks></remarks>
    Protected Sub AuditLogAddDescriptonConfirmBatchReason(ByVal objAuditLogEntry As AuditLogEntry)
        objAuditLogEntry.AddDescripton("Reason", lblConfirmBatchReason.Text)
    End Sub

#Region "Page Web Method"
    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    <WebMethod()> _
    <System.Web.Script.Services.ScriptMethod()> _
    Public Shared Function GetReasonForVisitL1(ByVal knownCategoryValues As String, ByVal category As String, ByVal contextKey As String) As CascadingDropDownNameValue()

        Dim udtReasonforVisitBLL As ReasonForVisitBLL = New ReasonForVisitBLL
        Dim dtReasonForVisit As DataTable

        dtReasonForVisit = udtReasonforVisitBLL.getReasonForVisitL1(category)

        Dim lst As New List(Of CascadingDropDownNameValue)
        For Each dr As DataRow In dtReasonForVisit.Rows
            lst.Add(New CascadingDropDownNameValue(dr("Reason_L1"), dr("Reason_L1_Code")))
        Next

        Return lst.ToArray
    End Function

    <WebMethod()> _
    <System.Web.Script.Services.ScriptMethod()> _
    Public Shared Function GetReasonForVisitL2(ByVal knownCategoryValues As String, ByVal category As String, ByVal contextKey As String) As CascadingDropDownNameValue()

        Dim udtReasonforVisitBLL As ReasonForVisitBLL = New ReasonForVisitBLL
        Dim dtReasonForVisit As DataTable
        Dim kv As StringDictionary

        Dim arrCategoryValues() As String = knownCategoryValues.Split(New Char() {";"}, StringSplitOptions.RemoveEmptyEntries)
        If arrCategoryValues.Length = 1 Then
            kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Else
            kv = CascadingDropDown.ParseKnownCategoryValuesString(arrCategoryValues(arrCategoryValues.Length - 1) + ";")
        End If

        dtReasonForVisit = udtReasonforVisitBLL.getReasonForVisitL2(category, kv(category))

        Dim lst As New List(Of CascadingDropDownNameValue)
        For Each dr As DataRow In dtReasonForVisit.Rows
            lst.Add(New CascadingDropDownNameValue(dr("Reason_L2"), dr("Reason_L2_Code")))
        Next

        Return lst.ToArray
    End Function

    Private Function CovertReasonForVisitToArray(ByVal dtReasonForVisit As DataTable) As CascadingDropDownNameValue()
        Dim lst As New List(Of CascadingDropDownNameValue)
        For Each dr As DataRow In dtReasonForVisit.Rows
            lst.Add(New CascadingDropDownNameValue(dr("Reason_L2"), dr("Reason_L2_Code")))
        Next

        Return lst.ToArray
    End Function
    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Koala]
#End Region

#Region "HKIC Symbol Input"
    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Public Sub EnableHKICSymbolRadioButtonList(ByVal blnShow As Boolean, ByVal strSchemeCode As String, ByVal dtmServiceDate As Date)
        If blnShow Then
            ' Check system parameters to see if enable
            If Common.OCSSS.OCSSSServiceBLL.EnableHKICSymbolInputForBackOffice(strSchemeCode, dtmServiceDate) Then
                Dim strSelectedValue As String = String.Empty

                'Store selected value into temp variable
                If rblHKICSymbol.SelectedValue <> String.Empty Then
                    strSelectedValue = rblHKICSymbol.SelectedValue
                End If

                'Clear radio button list
                ClearHKICSymbolButtonList()

                'Reload radio button list
                rblHKICSymbol.DataSource = Status.GetDescriptionListFromDBEnumCode("HKICSymbol")

                Select Case Session("language")
                    Case CultureLanguage.English
                        rblHKICSymbol.DataTextField = "Status_Description"
                    Case CultureLanguage.TradChinese
                        rblHKICSymbol.DataTextField = "Status_Description_Chi"
                    Case CultureLanguage.SimpChinese
                        rblHKICSymbol.DataTextField = "Status_Description_CN"
                    Case Else
                        rblHKICSymbol.DataTextField = "Status_Description"
                End Select

                rblHKICSymbol.DataValueField = "Status_Value"
                rblHKICSymbol.DataBind()

                'Restore selected value from temp variable
                If strSelectedValue <> String.Empty Then
                    rblHKICSymbol.SelectedValue = strSelectedValue
                End If

                panHKICSymbol.Visible = True

            Else
                panHKICSymbol.Visible = False

            End If

        Else
            panHKICSymbol.Visible = False

        End If

    End Sub

    ' CRE17-010 (OCSSS integration) [End][Chris YIM]

    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Private Sub rblHKICSymbol_DataBound(sender As Object, e As EventArgs) Handles rblHKICSymbol.DataBound
        Dim rbl As RadioButtonList = CType(sender, RadioButtonList)

        For idx As Integer = 0 To rbl.Items.Count - 1
            rbl.Items(idx).Value = rbl.Items(idx).Value.Trim
        Next

    End Sub
    ' CRE17-010 (OCSSS integration) [End][Chris YIM]

    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Private Sub ClearHKICSymbolButtonList()
        Me.rblHKICSymbol.Items.Clear()
        Me.rblHKICSymbol.SelectedIndex = -1
        Me.rblHKICSymbol.SelectedValue = Nothing
        Me.imgErrHKICSymbol.Visible = False
    End Sub
    ' CRE17-010 (OCSSS integration) [End][Chris YIM]
#End Region

End Class