Imports AjaxControlToolkit
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.EHSTransaction
Imports Common.Component.HCVUUser
Imports Common.Component.RedirectParameter
Imports Common.Component.Scheme
Imports Common.Component.StaticData
Imports Common.Component.UserRole
Imports Common.Format
Imports Common.SearchCriteria
Imports Common.Validation

Partial Public Class claimTransEnquiry
    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
    'Inherits BasePageWithGridView
    Inherits BasePageWithControl
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

    ' FunctionCode = FunctCode.FUNT010403

#Region "Private Classes"

    Private Class ViewIndex
        Public Const InputCriteria As Integer = 0
        Public Const Transaction As Integer = 1
        Public Const Detail As Integer = 2
    End Class

    Private Class TypeOfDate
        Public Const ServiceDate As String = "SD"
        Public Const TransactionDate As String = "TD"
    End Class

    Private Class SESS
        Public Const SelectedTabIndex As String = "010403_TabContainer_SelectedTabIndex"
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
    Private udtUserRoleBLL As New UserRoleBLL
    Private udtValidator As New Validator
    Private udtCommonFunction As New Common.ComFunction.GeneralFunction
    Private udtStaticDataBLL As New StaticDataBLL

#End Region

#Region "Session Constants"

    Private Const SESS_SchemeClaimList As String = "010403_SchemeClaimList"
    Private Const SESS_SearchCriteria As String = "010403_SearchCriteria"
    Private Const SESS_TransactionDataTable As String = "010403_TransactionDataTable"
    Private Const SESS_EHSTransaction As String = "010403_EHSTransaction"
    Private Const SESS_FromRedirect As String = "010403_FromRedirect"
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

        ' Get HCVU User to check session expire
        Dim udtHCVUUserBLL As New HCVUUserBLL
        udtHCVUUserBLL.GetHCVUUser()

        If Not IsPostBack Then
            FunctionCode = FunctCode.FUNT010403

            Dim intPageSize As Integer = udtCommonFunction.GetPageSize() ' CRE11-007

            gvTransaction.PageSize = intPageSize

            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, "Claim Trans Enquiry Loaded")

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

            ' Bind Reimbursement Method
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

            MultiViewClaimTransEnquiry.ActiveViewIndex = ViewIndex.InputCriteria

            Me.TabContainerCTM.ActiveTabIndex = Aspect.Transaction
            Session(SESS.SelectedTabIndex) = Aspect.Transaction
            'CRE13-012 (RCH Code sorting) [End][Chris YIM]

            HandleRedirectAction()

        Else
            If MultiViewClaimTransEnquiry.ActiveViewIndex = ViewIndex.Detail Then
                ' Rebind the details
                BuildClaimTransDetail(hfCurrentDetailTransactionNo.Value)
            End If

        End If

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Select Case MultiViewClaimTransEnquiry.ActiveViewIndex
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
            Case Else
                ScriptManager1.SetFocus(btnHidden)

        End Select

        If MultiViewClaimTransEnquiry.ActiveViewIndex = ViewIndex.Detail Then
            ibtnPreviousRecord.Enabled = lblCurrentRecordNo.Text <> "1"
            ibtnNextRecord.Enabled = lblCurrentRecordNo.Text <> lblMaxRecordNo.Text
        End If

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
            If gvTransaction.Rows.Count <> 1 Then Throw New Exception(String.Format("claimTransEnquiry.HandleRedirectAction: Unexpected no. of rows {0}", gvTransaction.Rows.Count))

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



    'CRE13-012 (RCH Code sorting) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
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
    'CRE13-012 (RCH Code sorting) [End][Chris YIM]

#End Region

#Region "Event Handler"
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
        'CRE20-003 (add search criteria) [End][Martin]
    End Sub

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
                    udtSystemMessage = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00257)
                    Me.udcMessageBox.AddMessage(udtSystemMessage)
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
                    udtSystemMessage = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00257)
                    Me.udcMessageBox.AddMessage(udtSystemMessage)
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
                    udtSystemMessage = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00257)
                    Me.udcMessageBox.AddMessage(udtSystemMessage)
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
                    udtSystemMessage = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00257)
                    Me.udcMessageBox.AddMessage(udtSystemMessage)

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
        '        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00009)
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
        '        'udtSystemMessage = udtValidator.chkInputValidFromDateCutoffDate(FunctionCode, MsgCode.MSG00010, _
        '        '        udtFormatter.convertDate(txtSServiceDateFrom.Text, String.Empty), udtFormatter.convertDate(txtSServiceDateTo.Text, String.Empty))
        '        udtSystemMessage = udtValidator.chkInputValidFromDateCutoffDate(FunctionCode, MsgCode.MSG00010, _
        '                udtFormatter.convertDate(strServiceDateFrom, String.Empty), udtFormatter.convertDate(strServiceDateTo, String.Empty))
        '        'INT15-0015 (Fix date format checking in HCVU) [End][Chris YIM]

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
            udcMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00003, "Search Fail")
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

            BuildRecordSummary(dtTransaction)

            ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            If udtHCVUUserBLL.IsSSSCMCUser(udtHCVUUserBLL.GetHCVUUser) Then
                panRecordSummary.Visible = False
            Else
                panRecordSummary.Visible = True
            End If
            ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

            MultiViewClaimTransEnquiry.ActiveViewIndex = ViewIndex.Transaction
        End If

        Return intRowCount
    End Function

    Protected Overrides Sub SF_FinalizeSearch(ByRef udtAuditLogEntry As AuditLogEntry)
    End Sub

    Protected Overrides Sub SF_ConfirmSearch_Click()
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim enumSearchResult As SearchResultEnum

        udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Search")

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
            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search fail")
            Throw

        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception")
            udtAuditLogEntry.AddDescripton("Message", ex.Message)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search fail")
            Throw

        End Try

        Select Case enumSearchResult
            Case SearchResultEnum.Success
                udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search successful")

            Case SearchResultEnum.OverResultList1stLimit_Alert
                ' Here is unexpected. The user may perform the search within the non-peak period, but he waits and inputs
                ' the token passcode for too long time and passed into the peak period
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Unexpected enumSearchResult={0}", enumSearchResult.ToString))
                udcMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, "00009"))
                udcMessageBox.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00003, "Search fail")

            Case SearchResultEnum.OverResultListOverrideLimit
                ' Here is unexpected. The number of result grows while the user is inputting the token passcode
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Unexpected enumSearchResult={0}", enumSearchResult.ToString))
                udcMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, "00009"))
                udcMessageBox.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00003, "Search fail")

            Case Else
                Throw New Exception("Error: Class = [HCVU.claimTransEnquiry], Method = [SF_ConfirmSearch_Click], Message = The unexpected error for the result of re-search")

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
                udtAuditLogEntry.AddDescripton("EHA ID", Me.txtTabeHSAccountID.Text.Trim)
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
        udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Search")

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
            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search fail")
            Throw

        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception")
            udtAuditLogEntry.AddDescripton("Message", ex.Message)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search fail")
            Throw

        End Try

        Select Case enumSearchResult
            Case SearchResultEnum.Success
                udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search successful")

            Case SearchResultEnum.ValidationFail
                ' Audit Log has been handled in [SF_ValidateSearch] method

            Case SearchResultEnum.NoRecordFound
                udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search successful")

            Case SearchResultEnum.OverResultList1stLimit_PopUp
                udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search fail")

            Case SearchResultEnum.OverResultList1stLimit_Alert
                udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search fail")

            Case SearchResultEnum.OverResultListOverrideLimit
                udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search fail")

            Case Else
                Throw New Exception("Error: Class = [HCVU.claimTransEnquiry], Method = [ibtnSearch_Click], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")

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
                    udtSearchCriteria.SchoolOrRCHCode = Me.txtTabTransactionRCHRode.Text.Trim

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
                    udtSearchCriteria.SchoolOrRCHCode = Me.txtTabeHSAccountRCHRode.Text.Trim

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
                    udtSearchCriteria.SchoolOrRCHCode = Me.txtTabAdvancedSearchRCHRode.Text.Trim

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



                    'Update Aspect
                    udtSearchCriteria.Aspect = EnumSelectedStoredProc



            End Select
            ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]

            Session(SESS_SearchCriteria) = udtSearchCriteria



            'If criteria is collected, reload the Aspect from model in session
        Else

            If IsNothing(udtSearchCriteria.Aspect) Then
                EnumSelectedStoredProc = Aspect.AdvancedSearch
            Else
                EnumSelectedStoredProc = udtSearchCriteria.Aspect
            End If


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
            gvTransaction.Columns(14).Visible = True ' Means of Input
        Else
            gvTransaction.Columns(14).Visible = False ' Means of Input
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
        lblRTransactionNo.Text = IIf(udtValidator.IsEmpty(udtSearchCriteria.TransNum), FillAnyToEmptyString(udtSearchCriteria.TransNum), udtFormatter.formatSystemNumber(udtSearchCriteria.TransNum))

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

        'CRE20-003 (add search criteria) [Start][Martin]
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

    '

    Protected Sub gvTransaction_RowDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
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
        udcInfoMessageBox.Visible = False
        udcMessageBox.Visible = False

        If Not (e.CommandName.ToUpper.Equals("PAGE") Or e.CommandName.Equals("Sort")) Then
            Dim r As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            Dim strTransactionNo As String = CType(r.FindControl("hfTransactionNo"), HiddenField).Value.Trim

            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00004, "Select Transaction")

            Try
                BuildClaimTransDetail(strTransactionNo)
                ' CRE11-004      
                Dim objAuditLogInfo As AuditLogInfo = BuildAuditLogInfoWithTransaction(Session(SESS_EHSTransaction))

                Dim dt As DataTable = Session(SESS_TransactionDataTable)

                lblCurrentRecordNo.Text = gvTransaction.PageIndex * gvTransaction.PageSize + r.RowIndex + 1
                lblMaxRecordNo.Text = dt.Rows.Count

                MultiViewClaimTransEnquiry.ActiveViewIndex = ViewIndex.Detail

                ibtnDetailBack.Visible = True

                udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00005, "Select Transaction end", objAuditLogInfo)

            Catch ex As Exception
                udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00006, "Select Transaction fail")
                Throw ex
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

    Protected Sub ibtnBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00013, "Back Click")
        ' CRE11-021 log the missed essential information [End]

        udcInfoMessageBox.Visible = False
        udcMessageBox.Visible = False

        Session(SESS_SearchCriteria) = Nothing
        MultiViewClaimTransEnquiry.ActiveViewIndex = ViewIndex.InputCriteria

    End Sub

    Protected Sub ibtnDetailBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcClaimTransDetail.ClearDocumentType()

        MultiViewClaimTransEnquiry.ActiveViewIndex = ViewIndex.Transaction
    End Sub

    Protected Sub ibtnPreviousRecord_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        If CInt(lblCurrentRecordNo.Text) > 1 Then

            Dim ee As New System.Web.UI.WebControls.GridViewPageEventArgs(udtReimbursementBLL.GetPageIndexInRecordNavigation(Me.gvTransaction.PageSize, CInt(Me.lblCurrentRecordNo.Text) - 1))

            GridViewPageIndexChangingHandler(gvTransaction, ee, SESS_TransactionDataTable)

            Dim intActualIndex As Integer = CInt(CType(gvTransaction.Rows( _
                CInt(lblCurrentRecordNo.Text) - (gvTransaction.PageSize * gvTransaction.PageIndex) - 2).Cells(0).FindControl("lblRecordNum"), Label).Text.Trim) - 1

            Dim dt As DataTable = Session(SESS_TransactionDataTable)
            Dim strTransactionNo As String = udtFormatter.formatSystemNumberReverse(dt.Rows(intActualIndex)(2).ToString).Trim

            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00007, "Previous record")

            Try
                BuildClaimTransDetail(strTransactionNo)
                ' CRE11-004      
                Dim objAuditLogInfo As AuditLogInfo = BuildAuditLogInfoWithTransaction(Session(SESS_EHSTransaction))

                lblCurrentRecordNo.Text = CInt(lblCurrentRecordNo.Text) - 1

                udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00008, "Previous record successful", objAuditLogInfo)

            Catch ex As Exception
                udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Previous record fail")
                Throw ex
            End Try
        End If
    End Sub

    Protected Sub ibtnNextRecord_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        If CInt(lblCurrentRecordNo.Text) < CInt(lblMaxRecordNo.Text) Then
            Dim ee As New System.Web.UI.WebControls.GridViewPageEventArgs(udtReimbursementBLL.GetPageIndexInRecordNavigation(Me.gvTransaction.PageSize, CInt(Me.lblCurrentRecordNo.Text) + 1))

            GridViewPageIndexChangingHandler(gvTransaction, ee, SESS_TransactionDataTable)

            Dim intActualIndex As Integer = CInt(CType(gvTransaction.Rows( _
                    CInt(lblCurrentRecordNo.Text) - gvTransaction.PageSize * gvTransaction.PageIndex).Cells(0).FindControl("lblRecordNum"), Label).Text.Trim) - 1

            Dim dt As DataTable = Session(SESS_TransactionDataTable)
            Dim strTransactionNo As String = udtFormatter.formatSystemNumberReverse(dt.Rows(intActualIndex)(2).ToString).Trim

            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00010, "Next record")

            Try
                BuildClaimTransDetail(strTransactionNo)
                ' CRE11-004      
                Dim objAuditLogInfo As AuditLogInfo = BuildAuditLogInfoWithTransaction(Session(SESS_EHSTransaction))

                lblCurrentRecordNo.Text = CInt(lblCurrentRecordNo.Text) + 1

                udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00011, "Next record successful", objAuditLogInfo)

            Catch ex As Exception
                udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00012, "Next record fail")
                Throw ex
            End Try

        End If
    End Sub

    Protected Sub ibtnSuspendHistory_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00046, "Show Suspend History")

        ' CRE11-004      
        Dim udtSessionHandlerBLL As New BLL.SessionHandlerBLL
        Dim objAuditLogInfo As AuditLogInfo = BuildAuditLogInfoWithTransaction(Session(SESS_EHSTransaction))

        Try
            popupSuspendHistory.Show()
            udtAuditLogEntry.AddDescripton("Transaction No", hfCurrentDetailTransactionNo.Value)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00047, "Show Suspend History successful", objAuditLogInfo)

        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("Transaction No", hfCurrentDetailTransactionNo.Value)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00048, "Show Suspend History fail", objAuditLogInfo)
        End Try

    End Sub

    Protected Sub ibtnManagement_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00049, "Detail - Management click", BuildAuditLogInfoWithTransaction(Session(SESS_EHSTransaction)))

        Dim udtRedirectParameter As New RedirectParameterModel
        udtRedirectParameter.SourceFunctionCode = FunctionCode
        udtRedirectParameter.TransactionNo = hfCurrentDetailTransactionNo.Value
        udtRedirectParameter.ActionList.Add(RedirectParameterModel.EnumRedirectAction.Search)
        udtRedirectParameter.ActionList.Add(RedirectParameterModel.EnumRedirectAction.ViewDetail)

        Dim udtRedirectParameterBLL As New RedirectParameterBLL
        udtRedirectParameterBLL.SaveToSession(udtRedirectParameter)

        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
        RedirectHandler.ToURL((New Component.Menu.MenuBLL).GetURLByFunctionCode(FunctCode.FUNT010404))
        ' CRE19-026 (HCVS hotline service) [End][Winnie]

    End Sub

    Protected Sub gvSuspendHistory_RowDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' System Time
            Dim lblSystemTime As Label = e.Row.FindControl("lblSystemTime")
            lblSystemTime.Text = udtFormatter.formatDateTime(lblSystemTime.Text.Trim, String.Empty)

            ' Status
            Dim lblRecordStatus As Label = e.Row.FindControl("lblRecordStatus")
            Status.GetDescriptionFromDBCode(ClaimTransStatus.ClassCode, lblRecordStatus.Text.Trim, lblRecordStatus.Text, String.Empty)

            ' Remarks
            Dim lblRemark As Label = e.Row.FindControl("lblRemark")
            If lblRemark.Text.Trim = String.Empty Then lblRemark.Text = Me.GetGlobalResourceObject("Text", "N/A")

        End If

    End Sub

    Private Sub BuildClaimTransDetail(ByVal strTransactionNo As String)
        Dim udtEHSTransaction As EHSTransactionModel = udtEHSTransactionBLL.LoadClaimTran(strTransactionNo, True, True)
        ' CRE11-004
        Session(SESS_EHSTransaction) = udtEHSTransaction
        ' End CRE11-004

        Dim udtSearchCriteria As New SearchCriteria
        udtSearchCriteria.TransNum = strTransactionNo

        Dim dtSuspendHistory As DataTable = udtSearchEngineBLL.SearchSuspendHistory(udtSearchCriteria)

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        udcClaimTransDetail.ShowHKICSymbol = True
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]
        udcClaimTransDetail.LoadTranInfo(udtEHSTransaction, dtSuspendHistory)

        ' Bind data to suspend history gridview
        If dtSuspendHistory.Rows.Count > 0 Then
            gvSuspendHistory.DataSource = dtSuspendHistory
            gvSuspendHistory.DataBind()

            ibtnSuspendHistory.Enabled = True
            ibtnSuspendHistory.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SuspendHistoryBtn")

        Else
            ibtnSuspendHistory.Enabled = False
            ibtnSuspendHistory.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SuspendHistoryDisableBtn")
        End If

        ' Save the current Transaction No to hidden field for the rebind in clicking action button (Suspend History)
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

        ' Management: Enable when have access right
        If (New HCVUUserBLL).GetHCVUUser().AccessRightCollection.Item(FunctCode.FUNT010404).Allow Then
            ibtnManagement.Enabled = True
            ibtnManagement.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ManagementBtn")
        Else
            ibtnManagement.Enabled = False
            ibtnManagement.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ManagementDisableBtn")
        End If

    End Sub

    '

    Protected Sub ibtnClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    End Sub


    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Account which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Transaction which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Service Provider which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve Document Code which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetDocCode() As String
        Return Nothing
    End Function
    ''' <summary>
    ''' CRE11-004
    '''  Clear all working data
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub ClearWorkingData()
        MyBase.ClearWorkingData()
        Session(SESS_EHSTransaction) = Nothing
        'Session(SESS_AuditLogInfo) = Nothing
    End Sub


    ''' <summary>
    ''' CRE11-004
    ''' Build Audit Log Info Object
    ''' </summary>
    ''' <remarks></remarks>
    Private Function BuildAuditLogInfoWithTransaction(ByVal udtEHSTransactionModel As EHSTransaction.EHSTransactionModel) As AuditLogInfo
        Dim strSPID As String = udtEHSTransactionModel.ServiceProviderID
        Dim strSPDocNo As String = Nothing
        Dim strAccType As String = udtEHSTransactionModel.EHSAcct.AccountSourceString
        Dim strAccID As String = udtEHSTransactionModel.EHSAcct.VoucherAccID
        Dim udtEHSPersonalInformation As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = udtEHSTransactionModel.EHSAcct.getPersonalInformation(udtEHSTransactionModel.DocCode)
        Dim strDocCode As String = udtEHSTransactionModel.DocCode
        Dim strDocNo As String = udtEHSPersonalInformation.IdentityNum
        Dim objAuditLogInfo As New AuditLogInfo(strSPID, strSPDocNo, strAccType, strAccID, strDocCode, strDocNo)

        Return objAuditLogInfo
    End Function

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

        ' Service Provider
        If intRetainInputAspect <> Aspect.ServiceProvider Then
            Me.txtTabServiceProviderSPID.Text = String.Empty
            Me.txtTabServiceProviderSPHKID.Text = String.Empty
            Me.txtTabServiceProviderSPName.Text = String.Empty
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

End Class