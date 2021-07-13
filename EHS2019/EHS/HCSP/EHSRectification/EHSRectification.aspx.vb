Imports System.Net

Imports Common.ComFunction
Imports Common.Component.UserAC
Imports Common.Component.ServiceProvider
Imports Common.Component.DataEntryUser
Imports Common.Component
Imports Common.Format
Imports Common.Validation
Imports Common.ComObject
Imports System.Data.SqlClient
Imports HCSP.BLL
Imports Common.Component.EHSAccount
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component.DocType
Imports Common.Component.SortedGridviewHeader
Imports System.Globalization
Imports Common.Component.RedirectParameter                                      ' CRE11-024-02
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.WebService.Interface
Imports IdeasRM

Partial Public Class EHSRectification
    'Inherits System.Web.UI.Page
    Inherits BasePageWithGridView

#Region "Audit Log Description"
    Public Class AuditLogDesc
        Public Const LoadAccountRectification As String = "eHealth Account Rectification Loaded"  '0
        Public Const SearchTempEHSAccount As String = "Search Temp eHealth Account"  '1
        Public Const SearchSuccess As String = "Search Result Success" '2
        Public Const SearchFail As String = "Search Fail"   '3
        Public Const SelectAccount As String = "Select eHealth Account"  '4
        Public Const SelectAccountComplete As String = "Select eHealth Account Complete" '5
        Public Const SelectAccountFail As String = "Select eHealth Account Fail"  '6

        Public Const ModifyButtonClick As String = "Modify Button Click"  '7
        Public Const ModifyYes As String = "Continue to Modify"  '8
        Public Const ModifyNo As String = "Refuse to Modify"  '9

        Public Const SelectPractice As String = "Select Practice"  '10
        Public Const SelectPracticeSuccess As String = "Select Practice Success"  '11
        Public Const SelectPracticeFail As String = "Select Practice Fail"  '12

        Public Const SaveRectifiedAccount As String = "Save rectified eHealth Account" '13
        Public Const ValidateRectifiedAccount As String = "Validate rectified eHealth Account"  '14
        Public Const ValidateRectifiedAccountComplete As String = "Validate rectified eHealth Account Complete"  '15
        Public Const ValidateRectifiedAccountFail As String = "Validate rectified eHealth Account Fail"  '16

        Public Const ShowDeclarationWithValidationComplete As String = "Show Declaration and validation complete" '17
        Public Const ClaimDeclarationConfirm As String = "Claim Declaration Confirm"  '18
        Public Const ClaimDeclarationCancel As String = "Claim Declaration Cancel"    '19

        Public Const RectifyAccount As String = "Rectify Temp eHealth Account"          '20
        Public Const RectifyAccountSuccess As String = "Rectify Temp eHealth Account Success"  '21
        Public Const RectifyAccountFail As String = "Rectify Temp eHealth Account Fail" '22

        Public Const ModifyAccount As String = "Modify Temp eHealth Account" ' 23
        Public Const ModifyAccountSuccess As String = "Modify Temp eHealth Account Success"  '22
        Public Const ModifyAccountFail As String = "Modify Temp eHealth Account Fail"   '25

        Public Const ViewTransaction As String = "View Transaction" '26

        Public Const ClickRemoveAccountbutton As String = "Click Remove Account button (Void temp EHS account)" '27
        Public Const VoidAccount As String = "Void Temp eHealth Account"  '28
        Public Const VoidAccountSuccess As String = "Void Temp eHealth Account Success"  '29
        Public Const VoidAccountFail As String = "Void Temp eHealth Account Fail"  '30
        Public Const CancelRemoveAccount As String = "Cancel Remove Account (Void temp EHS account)" '31

        Public Const BackToSearch As String = "Back to Search Page"  '32
        Public Const BackToAccountDetail As String = "Back to Account Detail Page"  '33 
        Public Const ReturnFromCompletePage As String = "Return from Complete Page"   '34

        Public Const ClickSelectChineseNameButton As String = "Click Select Chinese Name Button"  '35
        Public Const ChineseNameCodeCheckingSuccess As String = "Chinese Name Code Checking Success"  '36
        Public Const ChineseNameCodeCheckingFail As String = "Chinese Name Code Checking Fail"  '37
        Public Const ConfirmChineseName As String = "Confirm the selection of Chinese Name"  '38
        Public Const CancelChineseName As String = "Cancel the selection of Chinese Name"  '39
        'Void transaction
        Public Const BackToAccDetailFromViewTran As String = "Back to account detail from view transaction" '40
        Public Const ClickToVoidTran As String = "Click To Void Transaction"  '41
        Public Const ClickBackToCancelVoidTran As String = "Click Back To Cancel Void Transaction"  '42
        Public Const VoidTransaction As String = "Void Transaction" '43
        Public Const VoidTransactionSuccess As String = "Void Transaction Success"  '44
        Public Const VoidTransactionFail As String = "Void Transaction Fail"  '45
        Public Const VoidTransactionFailNoVoidReason As String = "Void Transaction Fail due to no reason provided"  '46

        'Declaration
        Public Const chkDeclaration As String = "Check the Declaration statement" '47

    End Class

#End Region

#Region "Constant Value"
    Private Const intSearchView As Integer = 0
    Private Const intSearchResultList As Integer = 1
    Private Const intRectifyAccount As Integer = 2
    Private Const intConfirmAccount As Integer = 3
    Private Const intComplete As Integer = 4
    Private Const intPracticeSelection As Integer = 5
    Private Const intTran As Integer = 6
    Private Const intSmartIDConfirmation As Integer = 7

    Private Const SESS_SearchResultList As String = "eHS_Rectification_SearchResultList"
    Private Const SESS_ClickSave As String = "eHS_Rectification_PressSave"
    Private Const SESS_InputMode As String = "eHS_Rectification_InputDocMode"
    Private Const SESS_DefaultSetCCCode As String = "eHS_Rectification_DefaultSetCCCode"
    Private Const SESS_ModifyAcc As String = "eHS_Rectification_ModifyAcc"
    Private Const SESS_PracticeCollection As String = "eHS_Rectification_PracticeCollection"
    'Private Const SESS_PracticeDisplay As String = "eHS_Rectification_PracticeDisplay"

    'CRE13-006 HCVS Ceiling [Start][Karl]
    Private Const SESS_OrgEHSAccount As String = "eHS_Rectification_EHSAccountBeforeAmendment"
    'CRE13-006 HCVS Ceiling [End][Karl]

    Private Const SESS_ConfirmDeclaration As String = "eHS_Rectification_ConfirmDeclaration"

    Private Const strValidationFail As String = "ValidationFail"
    Private Const strPracticeName As String = "practice_name"
    Private Const strPracticeNameChi As String = "practice_name_chi"

    Private Const strValid As String = "Y"

    Private Const FuncCode As String = Common.Component.FunctCode.FUNT020401
    Private Const CommonFuncCode As String = Common.Component.FunctCode.FUNT990000

#End Region

    Dim udtSP As ServiceProviderModel
    Dim udtDataEntry As DataEntryUserModel
    Dim udtUserAC As UserACModel = New UserACModel

    Dim validator As Validator = New Validator
    Dim udtFormatter As Formatter = New Formatter
    Dim sm As Common.ComObject.SystemMessage

    Dim udtAuditLogEntry As AuditLogEntry
    Dim formatter As Common.Format.Formatter = New Common.Format.Formatter
    Dim udtClaimVoucherBLL As ClaimVoucherBLL = New ClaimVoucherBLL
    Dim udtGeneralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
    Dim udtPracticeAcctBLL As PracticeBankAcctBLL = New PracticeBankAcctBLL

    Dim udtEHSClaimBLL As EHSClaimBLL = New EHSClaimBLL
    Dim udtEHSAccountBLL As EHSAccountBLL = New EHSAccountBLL
    Dim udtEHSTransactionBLL As EHSTransaction.EHSTransactionBLL = New EHSTransaction.EHSTransactionBLL
    Dim udtSessionHandler As BLL.SessionHandler = New BLL.SessionHandler
    Dim udtEHSAccount As EHSAccountModel
    Dim udtEHSTransaction As EHSTransaction.EHSTransactionModel
    Dim _udtPracticeBankAccBLL As New BLL.PracticeBankAcctBLL()

#Region "Page Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim udtSmartIDContent As BLL.SmartIDContentModel = Nothing

        If Not IsPostBack Then
            'Log Page Load 
            Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
            '-- ---------------------------------------------- --
            FunctionCode = "020401"     ' Record Confirmation
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            udtSmartIDContent = Me.udtSessionHandler.SmartIDContentGetFormSession(FuncCode)

            '==================================================================== Code for SmartID ============================================================================
            If Not Me.ReadSmartIC(udtSmartIDContent) Then
                Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00000, AuditLogDesc.LoadAccountRectification)
                Me.mvRectify.ActiveViewIndex = EHSRectification.intSearchView
            End If
            '==================================================================================================================================================================

            Dim strSelectedLanguage As String
            strSelectedLanguage = LCase(udtSessionHandler.Language())

            'Bind Status DDL
            ddlAcctStatus.DataSource = Status.GetDescriptionListFromDBEnumCode("HCSPeHSAccRectificationStatus", True)
            ddlAcctStatus.DataValueField = "Status_Value"

            If LCase(strSelectedLanguage).Equals(TradChinese) Then
                ddlAcctStatus.DataTextField = "Status_Description_Chi"
            ElseIf LCase(strSelectedLanguage).Equals(SimpChinese) Then
                ddlAcctStatus.DataTextField = "Status_Description_CN"
            Else
                ddlAcctStatus.DataTextField = "Status_Description"
            End If
            ddlAcctStatus.DataBind()

            preventMultiImgClick(Me.ClientScript, Me.ibtnConfirm)
            preventMultiImgClick(Me.ClientScript, Me.ibtnRemoveAccConfirm)

            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            preventMultiImgClick(Me.ClientScript, Me.ibtnRectifyReadOldSmartIC)
            preventMultiImgClick(Me.ClientScript, Me.ibtnRectifyReadNewSmartIC)
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]

        Else
            udtEHSAccount = Me.udtSessionHandler.EHSAccountGetFromSession(FuncCode)
            Me.SetupInputDocControl(udtEHSAccount, False)
        End If

        GetCurrentUser(udtSP, udtDataEntry)

        ' if data entry login => the only "pending confirmation" in DDL for selection
        If Not IsNothing(udtDataEntry) Then
            Me.ddlAcctStatus.SelectedValue = HCSPeHSAccRectificationStatus.PendingToConfirm
            Me.ddlAcctStatus.Enabled = False
        End If

        ' Check whether user click the shortcut in Home Page or not
        If Not IsNothing(Session("fromMain_GoRectify")) AndAlso Session("fromMain_GoRectify") = "Y" Then
            Session("fromMain_GoRectify") = ""
            Me.ddlAcctStatus.SelectedValue = HCSPeHSAccRectificationStatus.InvalidValid 'Common.Component.VRAcctValidatedStatus.Invalid
            Me.ibtnSearch_Click(Nothing, Nothing)
        End If

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        '-- ---------------------------------------------- --
        ' discard redirect parameter if found any that does not belong to this function
        Dim udtRedirectParameterBLL As New RedirectParameterBLL
        Dim udtRedirectParameter As RedirectParameterModel = udtRedirectParameterBLL.GetFromSession()

        If Not IsNothing(udtRedirectParameter) AndAlso Not IsNothing(udtRedirectParameter.ReturnParameter) AndAlso _
            Not IsNothing(udtRedirectParameter.ReturnParameter.SourceFunctionCode) Then
            If udtRedirectParameter.ReturnParameter.SourceFunctionCode <> CType(Me.Page, BasePage).FunctionCode Then
                udtRedirectParameterBLL.RemoveFromSession()
            End If
        End If

        HandleRedirectAction()

        ' INT13-0006 Fix eHA Recification save check [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        ' Build redirect button <View Transaction> in "Detail" & "Confirm Detail" view

        If (Me.mvRectify.ActiveViewIndex = intRectifyAccount Or Me.mvRectify.ActiveViewIndex = intConfirmAccount) AndAlso _
            Not udtEHSAccount Is Nothing AndAlso _
            udtEHSAccount.TransactionID <> String.Empty AndAlso _
            Not Session("REDIRECT_DocCode") Is Nothing AndAlso _
            Not Session("REDIRECT_strIdentityNumber") Is Nothing Then

            ibtnRectifyAccountViewTransaction.Visible = False
            Dim udtEHSTransaction As EHSTransaction.EHSTransactionModel = udtEHSTransactionBLL.LoadClaimTran(udtEHSAccount.TransactionID)
            BuildRedirectButton(Me.ibtnManagement, ClaimTransactionMaintenance.BuildSearchCriteria(udtEHSTransaction.RecordStatus, _
                                      udtEHSTransaction.TransactionDtm, _
                                      udtEHSTransaction.TransactionDtm, _
                                       udtEHSTransaction.TransactionID), EHSRectification.BuildSearchCriteria(Me.ddlAcctStatus.SelectedValue, _
                                                udtEHSAccount.VoucherAccID, _
                                                udtEHSAccount.AccountSourceString, _
                                                 Session("REDIRECT_DocCode").ToString(), _
                                                 Session("REDIRECT_strIdentityNumber").ToString()))

            '-- ---------------------------------------------- --
        End If
        ' INT13-0006 Fix eHA Recification save check [End][Koala]
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        AddHandler udcClaimTran.VaccineLegendClicked1, AddressOf udcClaimTran_VaccineLegendClicked
        'AddHandler PracticeRadioButtonGroup.PracticeSelected, AddressOf PracticeRadioButtonGroup_PracticeSelected
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If IsPostBack Then
            Dim controlID As String = Page.Request.Params.Get(PostBackEventTarget)
            If controlID.Equals(SelectTradChinese) OrElse controlID.Equals(SelectEnglish) OrElse controlID.Equals(SelectSimpChinese) Then
                RenderLanguage()
            End If
        Else
            Me.udcClaimTran.chgLanguage()
        End If
    End Sub

#End Region

#Region "Search Result List - Gridview Function"
    Private Sub gvAcctList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvAcctList.PageIndexChanging
        Me.GridViewPageIndexChangingHandler(sender, e, SESS_SearchResultList)
    End Sub

    Private Sub gvAcctList_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvAcctList.PreRender
        Me.GridViewPreRenderHandler(sender, e, SESS_SearchResultList)
    End Sub

    Private Sub gvAcctList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvAcctList.RowCommand
        If TypeOf e.CommandSource Is LinkButton Then

            Dim strDocCode As String = String.Empty
            Dim strAccountID As String = String.Empty
            Dim strSource As String = String.Empty
            Dim strIdentityNumber As String = String.Empty

            Dim strCommandArgument As String

            strCommandArgument = e.CommandArgument.ToString.Trim
            strDocCode = strCommandArgument.Split("|")(0).Trim
            strAccountID = strCommandArgument.Split("|")(1).Trim
            strSource = strCommandArgument.Split("|")(2).Trim
            strIdentityNumber = strCommandArgument.Split("|")(3).Trim

            Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
            Me.udtAuditLogEntry.AddDescripton("AccountID", strAccountID)
            Me.udtAuditLogEntry.AddDescripton("AccountSource", strSource)
            Me.udtAuditLogEntry.AddDescripton("DocCode", strDocCode)
            Dim udtAuditLogInfo As New AuditLogInfo(Nothing, Nothing, strSource, strAccountID, strDocCode, strIdentityNumber)
            Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00004, AuditLogDesc.SelectAccount, udtAuditLogInfo)

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
            '-- ---------------------------------------------- --
            Session("REDIRECT_DocCode") = strDocCode
            Session("REDIRECT_strIdentityNumber") = strIdentityNumber
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            If GeteHSAccount(strDocCode, strAccountID, strSource) Then
                udtEHSAccount = Me.udtSessionHandler.EHSAccountGetFromSession(FuncCode)


                Session(SESS_InputMode) = Me.AccountMode(udtEHSAccount)

                Session(SESS_DefaultSetCCCode) = True

                Me.txtDocCode.Text = strDocCode.Trim
                Me.SetupRectifyDetailScreen(False)
                Me.mvRectify.ActiveViewIndex = intRectifyAccount

                Me.udtAuditLogEntry.AddDescripton("AccountID", strAccountID)
                Me.udtAuditLogEntry.AddDescripton("AccountSource", strSource)
                Me.udtAuditLogEntry.AddDescripton("DocCode", strDocCode)
                Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00005, AuditLogDesc.SelectAccountComplete)

            Else
                udcMsgBoxErr.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
                Me.udtAuditLogEntry.AddDescripton("AccountID", strAccountID)
                Me.udtAuditLogEntry.AddDescripton("AccountSource", strSource)
                Me.udtAuditLogEntry.AddDescripton("DocCode", strDocCode)
                udcMsgBoxErr.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00006, AuditLogDesc.SelectAccountFail)
            End If


        End If
    End Sub

    Private Sub gvAcctList_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAcctList.RowCreated
        Dim udtSortedGVHeaderList As SortedGridviewHeaderModelCollection = New SortedGridviewHeaderModelCollection
        Dim strImageURL As String = Me.GetGlobalResourceObject("ImageURL", "infoicon")

        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(1, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(2, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(3, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))

        Me.GirdViewRowCreated(sender, e, udtSortedGVHeaderList)
    End Sub

    Private Sub gvAcctList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAcctList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRow = CType(e.Row.DataItem, System.Data.DataRowView).Row

            Dim lbtnIdentityNum As LinkButton = CType(e.Row.FindControl("lbtnIdentityNum"), LinkButton)
            Dim lblDateOfIssue As Label = CType(e.Row.FindControl("lblDateOfIssue"), Label)
            Dim lblName As Label = CType(e.Row.FindControl("lblName"), Label)
            Dim lblCName As Label = CType(e.Row.FindControl("lblCName"), Label)
            Dim lblDOB As Label = CType(e.Row.FindControl("lblDOB"), Label)
            Dim lblSex As Label = CType(e.Row.FindControl("lblSex"), Label)
            Dim lblPractice As Label = CType(e.Row.FindControl("lblPractice"), Label)
            Dim lblPracticeChi As Label = CType(e.Row.FindControl("lblPracticeChi"), Label)
            Dim lblRecordStatus As Label = CType(e.Row.FindControl("lblRecordStatus"), Label)
            Dim lblVRAcctID As Label = CType(e.Row.FindControl("lblVRAcctID"), Label)
            Dim lblTransactionID As Label = CType(e.Row.FindControl("lblTransactionID"), Label)
            Dim lblCreateDtm As Label = CType(e.Row.FindControl("lblCreateDtm"), Label)
            Dim lblDataEntryBy As Label = CType(e.Row.FindControl("lblDataEntryBy"), Label)
            Dim hiddenAdoptionPrefixNum As HiddenField = CType(e.Row.FindControl("hiddenAdoptionPrefixNum"), HiddenField)

            Dim strSelectedLanguage As String
            strSelectedLanguage = LCase(udtSessionHandler.Language())

            Dim strDocCode As String = CStr(dr.Item("Doc_code")).Trim

            'Identity Number
            lbtnIdentityNum.Text = udtFormatter.FormatDocIdentityNoForDisplay(strDocCode, CStr(dr.Item("IdentityNum")).Trim, True, hiddenAdoptionPrefixNum.Value)
            lbtnIdentityNum.CommandArgument = strDocCode + "|" + CStr(dr.Item("voucher_acc_id")).Trim + "|" + CStr(dr.Item("source")).Trim + "|" + CStr(dr.Item("IdentityNum")).Trim

            'Chinese Name
            lblCName.Text = udtFormatter.formatChineseName(lblCName.Text.Trim)

            'DOB
            Dim dtDOB As DateTime = CType(dr.Item("DOB"), DateTime)
            Dim strExactDOB As String = CStr(dr.Item("Exact_DOB"))
            Dim intAge As Nullable(Of Integer)
            Dim dtDOR As Nullable(Of DateTime)
            Dim strOtherInfo As String

            If IsDBNull(dr.Item("EC_Age")) Then
                intAge = Nothing
            Else
                intAge = CType(dr.Item("EC_Age"), Integer)
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

            lblDOB.Text = udtFormatter.formatDOB(strDocCode, dtDOB, strExactDOB, strSelectedLanguage, intAge, dtDOR, strOtherInfo)

            'Date of Issue
            Dim dtDOI As Nullable(Of DateTime)
            If IsDBNull(dr.Item("Date_of_Issue")) Then
                dtDOI = Nothing
            Else
                dtDOI = CType(dr.Item("Date_of_Issue"), DateTime)
            End If

            lblDateOfIssue.Text = udtFormatter.formatDOI_GV(dtDOI) 'udtFormatter.formatDOI(strDocCode, dtDOI)
            If lblDateOfIssue.Text.Trim.Equals(String.Empty) Then
                lblDateOfIssue.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            'Sex
            lblSex.Text = Me.GetGlobalResourceObject("Text", udtFormatter.formatGender(CStr(dr.Item("Sex")).Trim))

            'Practice Name
            If LCase(strSelectedLanguage) = TradChinese OrElse LCase(strSelectedLanguage) = SimpChinese Then
                lblPracticeChi.Text = CStr(dr.Item(strPracticeNameChi)).Trim
                lblPracticeChi.Visible = True
                lblPractice.Visible = False
            Else
                lblPractice.Text = CStr(dr.Item(strPracticeName)).Trim
                lblPracticeChi.Visible = False
                lblPractice.Visible = True
            End If

            'Status
            Dim strChiStatus As String = String.Empty
            Dim strEngStatus As String = String.Empty
            Dim strCNStatus As String = String.Empty
            Dim strAccStatus As String = String.Empty

            strAccStatus = CStr(dr.Item("VAStatus")).Trim

            Status.GetDescriptionFromDBCode(HCSPeHSAccRectificationStatus.ClassCode, strAccStatus, strEngStatus, strChiStatus, strCNStatus)

            If LCase(strSelectedLanguage) = TradChinese Then
                lblRecordStatus.Text = strChiStatus.Trim
            ElseIf LCase(strSelectedLanguage) = SimpChinese Then
                lblRecordStatus.Text = strCNStatus.Trim
            Else
                lblRecordStatus.Text = strEngStatus.Trim
            End If

            'AccountID
            lblVRAcctID.Text = udtFormatter.formatSystemNumber(CStr(dr.Item("Display_Acc_ID")).Trim)

            'Transaction ID
            Dim strTransactionId = CStr(dr.Item("Transaction_ID")).Trim
            If strTransactionId.Trim.Equals(String.Empty) Then
                lblTransactionID.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                lblTransactionID.Text = udtFormatter.formatSystemNumber(strTransactionId.Trim)
            End If

            'Create Datetime
            lblCreateDtm.Text = udtFormatter.formatDateTime(CType(dr.Item("Create_Dtm"), DateTime))

            'Data Entry
            Dim strDataEntryBy As String
            If IsDBNull(dr.Item("DataEntry_By")) Then
                strDataEntryBy = String.Empty
            Else
                strDataEntryBy = CStr(dr.Item("DataEntry_By")).Trim
            End If

            If strDataEntryBy.Equals(String.Empty) Then
                lblDataEntryBy.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                lblDataEntryBy.Text = strDataEntryBy
            End If


        End If
    End Sub

    Private Sub gvAcctList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvAcctList.Sorting
        Me.GridViewSortingHandler(sender, e, SESS_SearchResultList)
    End Sub

#End Region

#Region "Multi View Function"

    Private Sub mvRectify_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mvRectify.ActiveViewChanged
        udtEHSAccount = Me.udtSessionHandler.EHSAccountGetFromSession(FuncCode)

        Select Case Me.mvRectify.ActiveViewIndex
            Case intSearchView
                Me.udcMsgBoxErr.Clear()
                Me.udcInfoMsgBox.Clear()
                Me.udcMsgBoxErr.Visible = False
                ClearCurrentSession(True)
            Case intSearchResultList
                Me.udcMsgBoxErr.Clear()
                Me.udcInfoMsgBox.Clear()
                Me.udcMsgBoxErr.Visible = False
                ClearCurrentSession(False)
            Case intRectifyAccount
                Me.SetupInputDocControl(udtEHSAccount, True)
                Me.udtSessionHandler.EHSTransactionRemoveFromSession(FuncCode)
            Case intConfirmAccount
                Me.udcMsgBoxErr.Clear()
                Me.udcInfoMsgBox.Clear()
                Me.udcMsgBoxErr.Visible = False

                ' Set Confirm Btn in Confirm View
                If Not IsNothing(Session(SESS_ModifyAcc)) Then
                    If CBool(Session(SESS_ModifyAcc)) = True Then
                        'Set Declaration for creating new account
                        Me.ibtnConfirm.Enabled = False
                        Me.chkDeclaration.Checked = False
                        Me.ibtnConfirm.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "ConfirmDisableBtn")
                    Else
                        Me.ibtnConfirm.Enabled = True
                        Me.ibtnConfirm.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "ConfirmBtn")
                    End If
                Else
                    Me.ibtnConfirm.Enabled = True
                    Me.ibtnConfirm.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "ConfirmBtn")
                End If

                Me.udtSessionHandler.EHSTransactionRemoveFromSession(FuncCode)

            Case intComplete
                Me.ClearCurrentSession(True)
            Case intTran
                Me.udcMsgBoxErr.Clear()
                Me.udcInfoMsgBox.Clear()
                Me.udcMsgBoxErr.Visible = False
                Me.pnlTranActionBtn.Visible = True
                Me.pnlVoidTran.Visible = False
                Me.txtVoidReason.Text = String.Empty
                Me.imgVoidReasonErr.Visible = False

            Case intSmartIDConfirmation
                Me.udcMsgBoxErr.Clear()
                Me.udcInfoMsgBox.Clear()
                Me.SetupConfirmSmarIDRectify(udtEHSAccount, True)

            Case intPracticeSelection
                Me.udcMsgBoxErr.Clear()
                Me.udcInfoMsgBox.Clear()

        End Select

    End Sub

#End Region

#Region "View 1 - Search View Button Action"

    Protected Sub ibtnSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim dt As DataTable
        Dim strDataEntry As String = String.Empty
        Dim strStatus As String = String.Empty

        udcInfoMsgBox.Visible = False
        Me.lblDisplayStatus.Text = Me.ddlAcctStatus.SelectedItem.ToString
        strStatus = Me.ddlAcctStatus.SelectedValue.Trim
        'Unusual case: sometime it return "Any" instead of ""
        If strStatus.Trim = "Any" Then
            strStatus = ""
        End If

        GetCurrentUser(udtSP, udtDataEntry)

        If Not IsNothing(udtDataEntry) Then
            strDataEntry = udtDataEntry.DataEntryAccount
            strStatus = VRAcctValidatedStatus.PendingForConfirmation
        End If

        'Log start search 
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("Status", strStatus)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00001, AuditLogDesc.SearchTempEHSAccount)

        Dim udtVAMaintBLL As New VoucherAccountMaintenanceBLL

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'dt = udtVAMaintBLL.loadRectifyList(udtSP.SPID, strDataEntry, strStatus)
        dt = udtVAMaintBLL.loadRectifyList(udtSP.SPID, strDataEntry, strStatus, Me.SubPlatform)
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        If Not IsNothing(dt) Then
            Me.udtAuditLogEntry.AddDescripton("NoOfRecord", dt.Rows.Count)

            If dt.Rows.Count = 0 Then
                ' No record found
                udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Information
                udcInfoMsgBox.AddMessage("990000", "I", "00001")
                udcInfoMsgBox.BuildMessageBox()
            Else

                BLL.PracticeBankAcctBLL.HandleSwapPracticeLanguage(dt, strPracticeName, strPracticeNameChi)

                Session(SESS_SearchResultList) = dt

                GridViewDataBind(gvAcctList, dt, "Create_Dtm", "ASC", False)
                Me.mvRectify.ActiveViewIndex = intSearchResultList
            End If

            Me.udtAuditLogEntry.AddDescripton("Status", strStatus)
            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00002, AuditLogDesc.SearchSuccess)
        Else
            Me.udtAuditLogEntry.AddDescripton("Status", strStatus)
            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00003, AuditLogDesc.SearchFail)
        End If

    End Sub

#End Region

#Region "View 2 - Search Result List View Button Action"

    Protected Sub btnBackSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.mvRectify.ActiveViewIndex = intSearchView
    End Sub

    Protected Sub ibtnCloseDocTypeHelp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCloseDocTypeHelp.Click
        popupDocTypeHelp.Hide()
    End Sub

    Protected Sub ibtnCloseSchemeNameHelp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCloseSchemeNameHelp.Click
        popupSchemeNameHelp.Hide()
    End Sub
#End Region

#Region "View 3 - Rectify Account View Button Action"

    Protected Sub ibtnRectifyAccountBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00032, AuditLogDesc.BackToSearch)

        Me.mvRectify.ActiveViewIndex = intSearchResultList
        Me.udcRectifyAccount.Clear()
        Me.udcInfoMsgBox.Clear()
    End Sub

    Protected Sub ibtnRectifyAccountSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udcMsgBoxErr.Visible = False
        Dim blnProceed As Boolean = True
        Dim blnSmartIDCase As Boolean = False
        Dim strDocCode As String = String.Empty

        Me.udtEHSAccount = Me.udtSessionHandler.EHSAccountGetFromSession(FuncCode)

        ' I-CRE17-003 (Enhance eHA rectification on HCSP to check with eHS(S) and CMS vaccination) [Start][Winnie]
        Me.udtSessionHandler.CMSVaccineResultRemoveFromSession(FuncCode)
        ' I-CRE17-003 (Enhance eHA rectification on HCSP to check with eHS(S) and CMS vaccination) [End][Winnie]
        Me.udtSessionHandler.CIMSVaccineResultRemoveFromSession(FuncCode)

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", Me.udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", Me.udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.AddDescripton("DocCode", Me.txtDocCode.Text)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00013, AuditLogDesc.SaveRectifiedAccount)
        'Prepare next audit log
        Me.udtAuditLogEntry.AddDescripton("AccountID", Me.udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", Me.udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.AddDescripton("DocCode", Me.txtDocCode.Text)

        Select Case Me.txtDocCode.Text.Trim

            Case DocTypeModel.DocTypeCode.HKIC
                Dim udcInputHKIC As ucInputHKID = Me.udcRectifyAccount.GetHKICControl

                If Me.udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKIC).CreateBySmartID Then
                    blnSmartIDCase = True
                End If

                If Not blnSmartIDCase Then
                    If udcInputHKIC.CCCodeIsEmptyModification Then
                        udcInputHKIC.SetCNameModification(String.Empty)
                        Me.udcCCCode.Clean()
                        Me.udcCCCode.CleanSeesion(FuncCode)
                        'Me.udtSessionHandler.CCCodeRemoveFromSession(FuncCode)
                    Else
                        If udcInputHKIC.IsValidCCCodeModificationInput() Then
                            'Check CCCode
                            ' If CCCode is changed (session value <> input value) => pop up CCCode Panel
                            If Me.NeedPopupCCCodeDialog(DocTypeModel.DocTypeCode.HKIC) Then
                                Me.udcRectifyAccount_SelectChineseName_HKIC(udcInputHKIC, DocTypeModel.DocTypeCode.HKIC, Nothing, Nothing)
                                blnProceed = False

                                Exit Sub
                            End If
                        Else
                            Me.udcMsgBoxErr.AddMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00039)
                            udcInputHKIC.SetCCCodeError(True)
                            blnProceed = False

                        End If

                    End If
                End If

                If blnProceed Then
                    blnProceed = Me.ValidateRectifyDetail_HKID(udtEHSAccount, blnSmartIDCase, udtAuditLogEntry)
                End If

            Case DocTypeModel.DocTypeCode.EC

                blnProceed = Me.ValidateRectifyDetail_EC(udtEHSAccount, udtAuditLogEntry)

            Case DocTypeModel.DocTypeCode.HKBC

                blnProceed = Me.ValidateRectifyDetail_HKBC(udtEHSAccount, udtAuditLogEntry)

            Case DocTypeModel.DocTypeCode.ADOPC

                blnProceed = Me.ValidateRectifyDetail_Adopt(udtEHSAccount, udtAuditLogEntry)

            Case DocTypeModel.DocTypeCode.DI

                blnProceed = Me.ValidateRectifyDetail_DI(udtEHSAccount, udtAuditLogEntry)

            Case DocTypeModel.DocTypeCode.ID235B

                blnProceed = Me.ValidateRectifyDetail_ID235B(udtEHSAccount, udtAuditLogEntry)

            Case DocTypeModel.DocTypeCode.REPMT

                blnProceed = Me.ValidateRectifyDetail_ReEntryPermit(udtEHSAccount, udtAuditLogEntry)

            Case DocTypeModel.DocTypeCode.VISA

                blnProceed = Me.ValidateRectifyDetail_Visa(udtEHSAccount, udtAuditLogEntry)

            Case DocTypeModel.DocTypeCode.OW

                blnProceed = Me.ValidateRectifyDetail_OW(udtEHSAccount, udtAuditLogEntry)

            Case DocTypeModel.DocTypeCode.TW

                blnProceed = Me.ValidateRectifyDetail_TW(udtEHSAccount, udtAuditLogEntry)

            Case DocTypeModel.DocTypeCode.RFNo8

                blnProceed = Me.ValidateRectifyDetail_RFNo8(udtEHSAccount, udtAuditLogEntry)

            Case DocTypeModel.DocTypeCode.OTHER

                blnProceed = Me.ValidateRectifyDetail_OTHER(udtEHSAccount, udtAuditLogEntry)
                ' CRE20-0022 (Immu record) [Start][Martin]
            Case DocTypeModel.DocTypeCode.CCIC

                blnProceed = Me.ValidateRectifyDetail_CCIC(udtEHSAccount, udtAuditLogEntry)
            Case DocTypeModel.DocTypeCode.ROP140
                Dim udcInputROP140 As ucInputROP140 = Me.udcRectifyAccount.GetROP140Control

                If udcInputROP140.CCCodeIsEmpty Then
                    udcInputROP140.SetCName(String.Empty)
                    Me.udcCCCode.Clean()
                    Me.udcCCCode.CleanSeesion(FuncCode)

                Else
                    If udcInputROP140.IsValidCCCodeModificationInput() Then
                        'Check CCCode
                        ' If CCCode is changed (session value <> input value) => pop up CCCode Panel
                        If Me.NeedPopupCCCodeDialog(DocTypeModel.DocTypeCode.ROP140) Then
                            Me.udcRectifyAccount_SelectChineseName_HKIC(udcInputROP140, DocTypeModel.DocTypeCode.ROP140, Nothing, Nothing)
                            blnProceed = False

                            Exit Sub
                        End If
                    Else
                        Me.udcMsgBoxErr.AddMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00039)
                        udcInputROP140.SetCCCodeError(True)
                        blnProceed = False

                    End If

                End If

                If blnProceed Then
                    blnProceed = Me.ValidateRectifyDetail_ROP140(udtEHSAccount, udtAuditLogEntry)
                End If

            Case DocTypeModel.DocTypeCode.PASS
                blnProceed = Me.ValidateRectifyDetail_PASS(udtEHSAccount, udtAuditLogEntry)

            Case DocTypeModel.DocTypeCode.ISSHK, DocTypeModel.DocTypeCode.ET
                blnProceed = Me.ValidateRectifyDetail_ISSHK(udtEHSAccount, udtAuditLogEntry, Me.txtDocCode.Text.Trim)

            Case DocTypeModel.DocTypeCode.MEP, DocTypeModel.DocTypeCode.TWMTP, DocTypeModel.DocTypeCode.TWPAR, DocTypeModel.DocTypeCode.TWVTD, _
               DocTypeModel.DocTypeCode.TWNS, DocTypeModel.DocTypeCode.MD, DocTypeModel.DocTypeCode.MP, DocTypeModel.DocTypeCode.TD, _
               DocTypeModel.DocTypeCode.CEEP, DocTypeModel.DocTypeCode.DS
                blnProceed = Me.ValidateRectifyDetail_Common(udtEHSAccount, udtAuditLogEntry, Me.txtDocCode.Text.Trim)

                ' CRE20-0022 (Immu record) [End][Martin]
        End Select

        If blnProceed Then
            ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Dim udtClaimRulesBLL As New ClaimRules.ClaimRulesBLL
            Dim sm As SystemMessage
            Dim udtEligibleResult As ClaimRules.ClaimRulesBLL.EligibleResult = Nothing
            Dim udtTranDetailVaccineList As TransactionDetailVaccineModelCollection = Nothing
            Dim enumCheckEligiblity As ClaimRules.ClaimRulesBLL.Eligiblity = ClaimRules.ClaimRulesBLL.Eligiblity.Check

            If udtEHSAccount.TransactionID <> "" Then
                udtTranDetailVaccineList = Me.GetVaccinationRecord(udtEHSAccount, sm)
            Else
                enumCheckEligiblity = ClaimRules.ClaimRulesBLL.Eligiblity.NotCheck
            End If

            If IsNothing(sm) Then
                sm = udtClaimRulesBLL.CheckRectifyEHSAccount(udtEHSAccount.SchemeCode, Me.txtDocCode.Text.Trim, _
                                                             udtEHSAccount, udtEligibleResult, udtTranDetailVaccineList, _
                                                             enumCheckEligiblity, ClaimRules.ClaimRulesBLL.Unique.Include_Self_EHSAccount)

            End If
            ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

            Dim blnShowDeclaration As Boolean = False

            If IsNothing(sm) Then
                If Not IsNothing(udtEligibleResult) Then
                    If udtEligibleResult.HandleMethod = ClaimRules.ClaimRulesBLL.HandleMethodENum.Declaration Then

                        Dim strText As String = String.Empty
                        If Not udtEligibleResult.RelatedEligibleRule Is Nothing AndAlso Not String.IsNullOrEmpty(udtEligibleResult.RelatedEligibleRule.ObjectName3) Then
                            strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleRule.ObjectName3.Trim())
                        ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing AndAlso Not String.IsNullOrEmpty(udtEligibleResult.RelatedEligibleExceptionRule.ObjectName3) Then
                            strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleExceptionRule.ObjectName3.Trim())
                        ElseIf Not udtEligibleResult.RelatedClaimCategoryEligibilityModel Is Nothing AndAlso Not String.IsNullOrEmpty(udtEligibleResult.RelatedClaimCategoryEligibilityModel.ObjectName3) Then
                            strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedClaimCategoryEligibilityModel.ObjectName3.Trim())
                        End If

                        If Not String.IsNullOrEmpty(strText) Then
                            Me.lblClamDeclaration.Text = strText
                            ModalPopupExtenderClaimDEclaration.Show()
                            blnShowDeclaration = True
                        Else
                            blnShowDeclaration = False
                        End If
                    Else
                        blnShowDeclaration = False
                    End If
                Else
                    blnShowDeclaration = False
                End If

                If Not blnShowDeclaration Then
                    Me.udtSessionHandler.EHSAccountSaveToSession(udtEHSAccount, FuncCode)
                    SetupConfirmRectifyAcc(udtEHSAccount)
                    Me.mvRectify.ActiveViewIndex = intConfirmAccount

                    Dim blnModify As Boolean = False

                    If Not IsNothing(Session(SESS_ModifyAcc)) Then
                        If CBool(Session(SESS_ModifyAcc)) = True Then
                            pnlCreateAccDeclaration.Visible = True
                        Else
                            pnlCreateAccDeclaration.Visible = False
                        End If
                    Else
                        pnlCreateAccDeclaration.Visible = False
                    End If
                    Me.udtAuditLogEntry.AddDescripton("AccountID", Me.udtEHSAccount.VoucherAccID.Trim)
                    Me.udtAuditLogEntry.AddDescripton("AccountSource", Me.udtEHSAccount.AccountSourceString)
                    Me.udtAuditLogEntry.AddDescripton("DocCode", Me.txtDocCode.Text)
                    Me.udtAuditLogEntry.WriteEndLog(LogID.LOG00015, AuditLogDesc.ValidateRectifiedAccountComplete)
                Else
                    Me.udtAuditLogEntry.AddDescripton("AccountID", Me.udtEHSAccount.VoucherAccID.Trim)
                    Me.udtAuditLogEntry.AddDescripton("AccountSource", Me.udtEHSAccount.AccountSourceString)
                    Me.udtAuditLogEntry.AddDescripton("DocCode", Me.txtDocCode.Text)
                    Me.udtAuditLogEntry.WriteEndLog(LogID.LOG00017, AuditLogDesc.ShowDeclarationWithValidationComplete)
                End If
            Else
                Me.udcMsgBoxErr.Clear()
                Me.udcMsgBoxErr.AddMessage(sm)
                Me.udtAuditLogEntry.AddDescripton("AccountID", Me.udtEHSAccount.VoucherAccID.Trim)
                Me.udtAuditLogEntry.AddDescripton("AccountSource", Me.udtEHSAccount.AccountSourceString)
                Me.udtAuditLogEntry.AddDescripton("DocCode", Me.txtDocCode.Text)
                Me.udcMsgBoxErr.BuildMessageBox(strValidationFail, Me.udtAuditLogEntry, LogID.LOG00016, AuditLogDesc.ValidateRectifiedAccountFail)
            End If
        Else
            Me.udtAuditLogEntry.AddDescripton("AccountID", Me.udtEHSAccount.VoucherAccID.Trim)
            Me.udtAuditLogEntry.AddDescripton("AccountSource", Me.udtEHSAccount.AccountSourceString)
            Me.udtAuditLogEntry.AddDescripton("DocCode", Me.txtDocCode.Text)
            Me.udcMsgBoxErr.BuildMessageBox(strValidationFail, Me.udtAuditLogEntry, LogID.LOG00016, AuditLogDesc.ValidateRectifiedAccountFail)
        End If
    End Sub

    Protected Sub ibtnRectifyAccountModify_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtEHSAccount = Me.udtSessionHandler.EHSAccountGetFromSession(FuncCode)

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", Me.udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", Me.udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.AddDescripton("DocCode", Me.txtDocCode.Text)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00007, AuditLogDesc.ModifyButtonClick)

        'Modify Account should not have smartID Content
        Me.udtSessionHandler.SmartIDContentRemoveFormSession(FuncCode)

        Me.ModalPopupExtenderModify.Show()
    End Sub

    Protected Sub ibtnRectifyAccountViewTransaction_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.AddDescripton("TransactionNo", udtEHSAccount.TransactionID.Trim)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00026, AuditLogDesc.ViewTransaction)

        Me.udtSessionHandler.EHSTransactionRemoveFromSession(FuncCode)

        udtEHSTransaction = Me.udtEHSTransactionBLL.LoadClaimTran(udtEHSAccount.TransactionID)

        Me.udtSessionHandler.EHSTransactionSaveToSession(udtEHSTransaction, FuncCode)

        Me.udcClaimTran.buildClaimObject(udtEHSTransaction.TransactionID.Trim, udtEHSTransaction, False)

        Dim udtTranMaintBLL As TransactionMaintenanceBLL = New TransactionMaintenanceBLL
        If udtTranMaintBLL.CheckTransactionVoidable(udtEHSAccount.TransactionID.Trim) Then
            Me.ibtnTranVoid.Visible = True
        Else
            Me.ibtnTranVoid.Visible = False
        End If
        Me.mvRectify.ActiveViewIndex = intTran

    End Sub

    Protected Sub ibtnRectifyAccountRemove_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtEHSAccount = Me.udtSessionHandler.EHSAccountGetFromSession(FuncCode)

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", Me.udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00027, AuditLogDesc.ClickRemoveAccountbutton)

        Me.ModalPopupExtenderRemoveAcc.Show()
    End Sub

    Protected Sub ibtnRemoveAccConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtEHSAccount = Me.udtSessionHandler.EHSAccountGetFromSession(FuncCode)

        GetCurrentUser(udtSP, udtDataEntry)

        Dim strUpdateBy As String = String.Empty
        If Not IsNothing(udtDataEntry) Then
            strUpdateBy = udtDataEntry.DataEntryAccount
        Else
            strUpdateBy = udtSP.SPID
        End If

        Dim blnSuccess As Boolean = True

        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("IdentityDocNo", udtEHSAccount.EHSPersonalInformationList(0).IdentityNum)
        Me.udtAuditLogEntry.AddDescripton("DocCode", udtEHSAccount.EHSPersonalInformationList(0).DocCode)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSource)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00028, AuditLogDesc.VoidAccount)

        Try
            ' Pending - Find status in enum
            udtEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.Removed
            ' Remove
            Dim dtmCurrent = Me.udtGeneralFunction.GetSystemDateTime

            If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.TemporaryAccount Then
                If udtEHSAccount.AccountPurpose = EHSAccountModel.AccountPurposeClass.ForAmendment Then
                    '==================================================================== Code for SmartID ============================================================================
                    Dim udtEHSAccount_Temp_O As EHSAccountModel = udtEHSAccountBLL.LoadTempEHSAccountByVRID(udtEHSAccount.OriginalAmendAccID)

                    Me.udtEHSAccountBLL.UpdateAmendEHSAccountReject(udtEHSAccount, udtEHSAccount_Temp_O, strUpdateBy, dtmCurrent)
                    '==================================================================================================================================================================
                Else
                    Me.udtEHSAccountBLL.UpdateTempEHSAccountReject(udtEHSAccount, strUpdateBy, dtmCurrent)
                End If

            ElseIf udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.SpecialAccount Then
                Me.udtEHSAccountBLL.UpdateSpecialEHSAccountReject(udtEHSAccount, strUpdateBy, dtmCurrent)
            End If

            'Log Void Success
            Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID)
            Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSource)
            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00029, AuditLogDesc.VoidAccountSuccess)
        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                'Lof Void Fail and Build Message Box
                Me.sm = New Common.ComObject.SystemMessage("990001", "D", eSQL.Message)
                Me.udcMsgBoxErr.AddMessage(Me.sm)
                Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID)
                Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSource)
                Me.udcMsgBoxErr.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00030, AuditLogDesc.VoidAccountFail)
                blnSuccess = False
            Else
                Throw eSQL
            End If
        Catch ex As Exception
            Throw ex
        End Try

        If blnSuccess Then
            sm = New Common.ComObject.SystemMessage(FuncCode, SeverityCode.SEVI, MsgCode.MSG00003)
            Me.udcInfoMsgBox.AddMessage(sm)
            Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Complete
            Me.udcInfoMsgBox.BuildMessageBox()
            Me.udcMsgBoxErr.Clear()
            Me.panCompleteVoidTrans.Visible = False
            Me.mvRectify.ActiveViewIndex = intComplete
        Else
            Me.panCompleteVoidTrans.Visible = False
            Me.mvRectify.ActiveViewIndex = intComplete
        End If

    End Sub

    Protected Sub ibtnRemoveAccCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtEHSAccount = Me.udtSessionHandler.EHSAccountGetFromSession(FuncCode)

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", Me.udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00031, AuditLogDesc.CancelRemoveAccount)

        Me.ModalPopupExtenderRemoveAcc.Hide()
    End Sub

    Protected Sub ibtnModifyYes_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtSmartIDContent As SmartIDContentModel = Me.udtSessionHandler.SmartIDContentGetFormSession(FuncCode)
        Dim udtSelectedPracticeDisplay As BLL.PracticeDisplayModel = Nothing
        Dim udtPracticeDisplays As BLL.PracticeDisplayModelCollection = Nothing

        Me.udtEHSAccount = Me.udtSessionHandler.EHSAccountGetFromSession(FuncCode)

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", Me.udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", Me.udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.AddDescripton("DocCode", Me.udtEHSAccount.EHSPersonalInformationList(0).DocCode)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00008, AuditLogDesc.ModifyYes)

        GetCurrentUser(udtSP, udtDataEntry)

        Dim udtEHSTransactionBLL As New EHSTransaction.EHSTransactionBLL
        Dim udtEHSTransaction As EHSTransaction.EHSTransactionModel
        udtEHSTransaction = udtEHSTransactionBLL.LoadClaimTran(udtEHSAccount.TransactionID)

        If Not udtDataEntry Is Nothing Then
            udtPracticeDisplays = Me._udtPracticeBankAccBLL.getActivePractice(udtSP.SPID, udtDataEntry.DataEntryAccount)
        Else
            udtPracticeDisplays = Me._udtPracticeBankAccBLL.getActivePractice(udtSP.SPID)
        End If

        udtPracticeDisplays = udtPracticeDisplays.FilterByActiveScheme(udtSP.PracticeList, True)

        Session(SESS_PracticeCollection) = udtPracticeDisplays

        Session(SESS_ModifyAcc) = True

        If Not udtSmartIDContent Is Nothing AndAlso udtSmartIDContent.IsReadSmartID Then
            '==================================================================== Code for SmartID ============================================================================

            ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Select Case udtSmartIDContent.IdeasVersion
                Case IdeasBLL.EnumIdeasVersion.One, IdeasBLL.EnumIdeasVersion.Two, IdeasBLL.EnumIdeasVersion.TwoGender
                    Me.RedirectToIdeas(True, udtSmartIDContent.IdeasVersion)

                Case IdeasBLL.EnumIdeasVersion.Combo, IdeasBLL.EnumIdeasVersion.ComboGender
                    Me.RedirectToIdeasCombo(True, udtSmartIDContent.IdeasVersion)
            End Select
            ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

            '==================================================================================================================================================================
        Else

            If udtPracticeDisplays.Count = 1 Then

                Session(SESS_InputMode) = ucInputDocTypeBase.BuildMode.Modification
                Me.udcRectifyAccount.Clear()
                Me.udcRectifyAccount.DocType = String.Empty
                Me.BindPersonalInfo(udtEHSAccount, True)
                Me.SetupRectifyDetailScreen(True)

                'Save Practice to session Practice ID will assign to EHS Account before confirm save
                Me.udtSessionHandler.PracticeDisplaySaveToSession(udtPracticeDisplays.Item(0), FuncCode)
                'udtEHSAccount.CreateSPPracticeDisplaySeq = udtPracticeDisplays.Item(0).PracticeID

            Else
                ' CRE20-0XX (HA Scheme) [Start][Winnie]
                Me.PracticeRadioButtonGroup.SchemeSelection = False
                ' CRE20-0XX (HA Scheme) [End][Winnie]
                Me.PracticeRadioButtonGroup.BuildRadioButtonGroup(udtPracticeDisplays, udtSP.PracticeList, udtSP.SchemeInfoList, udtSessionHandler.Language, PracticeRadioButtonGroup.DisplayMode.BankAccount)

                Dim strCurrentPracticeName As String = String.Empty
                Dim strCurrentPracticeNameChi As String = String.Empty

                For Each PracticeDisplay As BLL.PracticeDisplayModel In udtPracticeDisplays
                    If PracticeDisplay.PracticeID = udtEHSTransaction.PracticeID Then
                        'strCurrentPracticeName = PracticeDisplay.PracticeName.Trim + " (" + udtEHSTransaction.PracticeID.ToString() + ") [" + formatter.formatAddress(PracticeDisplay.Room, PracticeDisplay.Floor, PracticeDisplay.Block, PracticeDisplay.Building, PracticeDisplay.District, PracticeDisplay.AddressCode) + "]"
                        'If PracticeDisplay.BuildingChi.Trim() <> "" Then
                        '    strCurrentPracticeNameChi = PracticeDisplay.PracticeNameChi.Trim + "(" + udtEHSTransaction.PracticeID.ToString() + ") [" + formatter.formatAddressChi(PracticeDisplay.Room, PracticeDisplay.Floor, PracticeDisplay.Block, PracticeDisplay.BuildingChi, PracticeDisplay.District, PracticeDisplay.AddressCode) + "]"
                        'Else
                        '    strCurrentPracticeNameChi = PracticeDisplay.PracticeNameChi.Trim + "(" + udtEHSTransaction.PracticeID.ToString() + ") [" + formatter.formatAddressChi(PracticeDisplay.Room, PracticeDisplay.Floor, PracticeDisplay.Block, PracticeDisplay.Building, PracticeDisplay.District, PracticeDisplay.AddressCode) + "]"
                        'End If
                        strCurrentPracticeName = Me.formatDisplayPracticeNameAdress(PracticeDisplay)
                        strCurrentPracticeNameChi = Me.formatDisplayPracticeChiNameAdress(PracticeDisplay)
                        Me.udtSessionHandler.PracticeDisplaySaveToSession(PracticeDisplay, FuncCode)
                        'Session(SESS_PracticeDisplay) = PracticeDisplay
                        Exit For
                    End If
                Next

                Dim strSelectedLanguage As String
                strSelectedLanguage = LCase(udtSessionHandler.Language())

                If LCase(strSelectedLanguage).Equals(TradChinese) OrElse LCase(strSelectedLanguage).Equals(SimpChinese) Then
                    If Not IsNothing(udtEHSTransaction) Then
                        Me.lblPracticeSelection.Text = strCurrentPracticeNameChi
                    End If
                Else
                    If Not IsNothing(udtEHSTransaction) Then
                        Me.lblPracticeSelection.Text = strCurrentPracticeName
                    End If
                End If

                Me.mvRectify.ActiveViewIndex = intPracticeSelection
            End If
        End If
    End Sub

    Protected Sub ibtnModifyNo_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtEHSAccount = Me.udtSessionHandler.EHSAccountGetFromSession(FuncCode)

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", Me.udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", Me.udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.AddDescripton("DocCode", Me.txtDocCode.Text)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00009, AuditLogDesc.ModifyNo)

        ' Clear smart id content to avoid loading smart ic content in page load
        Me.udtSessionHandler.SmartIDContentRemoveFormSession(FuncCode)

        Me.ModalPopupExtenderModify.Hide()
    End Sub

    Protected Sub ibtnClaimDeclarationConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'Audit Log
        Me.udtEHSAccount = Me.udtSessionHandler.EHSAccountGetFromSession(FuncCode)
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", Me.udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", Me.udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.AddDescripton("DocCode", Me.txtDocCode.Text)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00018, AuditLogDesc.ClaimDeclarationConfirm)

        ModalPopupExtenderClaimDEclaration.Hide()

        ' I-CRE17-003 (Enhance eHA rectification on HCSP to check with eHS(S) and CMS vaccination) [Start][Winnie]
        If Me.mvRectify.ActiveViewIndex = intRectifyAccount Then

            ' From Save button
            Me.udtSessionHandler.EHSAccountSaveToSession(udtEHSAccount, FuncCode)
            SetupConfirmRectifyAcc(udtEHSAccount)

            If Not IsNothing(Session(SESS_ModifyAcc)) Then
                If CBool(Session(SESS_ModifyAcc)) = True Then
                    pnlCreateAccDeclaration.Visible = True
                Else
                    pnlCreateAccDeclaration.Visible = False
                End If
            Else
                pnlCreateAccDeclaration.Visible = False
            End If

            Me.mvRectify.ActiveViewIndex = intConfirmAccount

        ElseIf Me.mvRectify.ActiveViewIndex = intSmartIDConfirmation Then

            ' From Smart IC Confirm button
            Session(SESS_ConfirmDeclaration) = True
            Me.ibtnSmartIDConfirmationConfirm_Click(Nothing, Nothing)
            Me.Session.Remove(SESS_ConfirmDeclaration)
        End If

        ' I-CRE17-003 (Enhance eHA rectification on HCSP to check with eHS(S) and CMS vaccination) [End][Winnie]
    End Sub

    Protected Sub ibtnClaimDeclarationCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'Audit Log
        Me.udtEHSAccount = Me.udtSessionHandler.EHSAccountGetFromSession(FuncCode)
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", Me.udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", Me.udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.AddDescripton("DocCode", Me.txtDocCode.Text)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00019, AuditLogDesc.ClaimDeclarationCancel)

        ModalPopupExtenderClaimDEclaration.Hide()
    End Sub

    '==================================================================== Code for SmartID ============================================================================
    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Protected Sub ibtnRectifyReadOldSmartIC_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnRectifyReadOldSmartIC.Click
        Me.PreRedirectToIdeas(True, IdeasBLL.EnumIdeasVersion.One)
    End Sub
    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Protected Sub ibtnRectifyReadNewSmartIC_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnRectifyReadNewSmartIC.Click
        Me.PreRedirectToIdeas(True, IdeasBLL.EnumIdeasVersion.Two)
    End Sub
    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Protected Sub ibtnRectifyReadNewSmartICCombo_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnRectifyReadNewSmartICCombo.Click
        Me.PreRedirectToIdeas(True, IdeasBLL.EnumIdeasVersion.Combo)
    End Sub
    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

    '==================================================================================================================================================================

    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Protected Sub ibtnRectifyReadSmartIDTips_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnRectifyReadSmartIDTips.Click
        Dim strReadSmartIDTipsUrl As String = Me.GetGlobalResourceObject("Url", "HCSPSmartIDCardUserGuideUrl")
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "ReadSmartIDTips", "javascript:openNewWin('" + ResolveClientUrl(strReadSmartIDTipsUrl) + "');", True)
    End Sub
    ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
#End Region

#Region "View 4 - Confirm Rectify Account View Button Action"

    Protected Sub ibtnConfirmBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udcMsgBoxErr.Clear()
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00033, AuditLogDesc.BackToAccountDetail)

        Me.mvRectify.ActiveViewIndex = intRectifyAccount
    End Sub

    Protected Sub ibtnConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udcMsgBoxErr.Clear()
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtEHSAccount = Me.udtSessionHandler.EHSAccountGetFromSession(FuncCode)

        Dim blnModifyAcc As Boolean = False
        Dim blnChkDeclare As Boolean = False

        'CRE13-006 HCVS Ceiling [Start][Karl]
        Dim udtOrgEHSAccount As EHSAccountModel = CType(Me.Session(SESS_OrgEHSAccount), EHSAccountModel)
        'CRE13-006 HCVS Ceiling [End][Karl]

        If Me.IsReusedAcc(udtEHSAccount.OriginalAccID) Then
            blnChkDeclare = Me.chkDeclaration.Checked
        Else
            blnChkDeclare = True
        End If

        If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.SpecialAccount Then
            blnChkDeclare = True
        End If

        Dim strUpdateBy As String = String.Empty

        GetCurrentUser(udtSP, udtDataEntry)

        If Not IsNothing(udtDataEntry) Then
            strUpdateBy = udtDataEntry.DataEntryAccount
        Else
            strUpdateBy = udtSP.SPID
        End If

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        With udtEHSAccount
            Me.udtAuditLogEntry.AddDescripton("AccountID", .VoucherAccID)
            Me.udtAuditLogEntry.AddDescripton("OriginalAccountID", .OriginalAccID)
            Me.udtAuditLogEntry.AddDescripton("AcctType", .AccountPurpose)
            Me.udtAuditLogEntry.AddDescripton("AccountSource", .AccountSourceString)
            With udtEHSAccount.EHSPersonalInformationList(0)
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
        If Me.IsReusedAcc(udtEHSAccount.OriginalAccID) Then
            blnModifyAcc = True
            Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00023, AuditLogDesc.ModifyAccount)
        Else
            blnModifyAcc = False
            Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00020, AuditLogDesc.RectifyAccount)
        End If

        Try
            If blnChkDeclare Then
                ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                Dim udtClaimRulesBLL As New ClaimRules.ClaimRulesBLL
                Dim sm As SystemMessage
                Dim udtEligibleResult As ClaimRules.ClaimRulesBLL.EligibleResult = Nothing
                Dim strNewAccountID As String = String.Empty
                Dim udtDirectUpdateExistingAccount As Boolean = False
                Dim udtPracticeDisplay As PracticeDisplayModel = Me.udtSessionHandler.PracticeDisplayGetFromSession(FuncCode)
                Dim udtTranDetailVaccineList As TransactionDetailVaccineModelCollection = Nothing
                Dim enumCheckEligiblity As ClaimRules.ClaimRulesBLL.Eligiblity = ClaimRules.ClaimRulesBLL.Eligiblity.Check

                If udtEHSAccount.TransactionID <> "" Then
                    udtTranDetailVaccineList = Me.GetVaccinationRecord(udtEHSAccount, sm)
                Else
                    enumCheckEligiblity = ClaimRules.ClaimRulesBLL.Eligiblity.NotCheck
                End If

                sm = udtClaimRulesBLL.CheckRectifyEHSAccount(udtEHSAccount.SchemeCode, Me.txtDocCode.Text.Trim, _
                                                             udtEHSAccount, udtEligibleResult, udtTranDetailVaccineList, _
                                                             enumCheckEligiblity, ClaimRules.ClaimRulesBLL.Unique.Include_Self_EHSAccount)


                If IsNothing(sm) Then

                    If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.TemporaryAccount Then
                        If IsReusedAcc(udtEHSAccount.OriginalAccID) Then
                            udtDirectUpdateExistingAccount = False
                        Else
                            udtDirectUpdateExistingAccount = True
                        End If
                    ElseIf udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.SpecialAccount Then
                        udtDirectUpdateExistingAccount = True

                    End If

                    If udtDirectUpdateExistingAccount Then
                        Me.udtAuditLogEntry.AddDescripton("DirectUpdateExistingAccount", "true")
                        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID)

                        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                        ' Update Status to PendingVerify (Missing Info case will not happen since all fields must be inputted in UI)
                        If udtEHSAccount.RecordStatus.Trim.Equals(VRAcctValidatedStatus.Invalid) Or _
                            udtEHSAccount.RecordStatus.Trim.Equals(VRAcctValidatedStatus.Restricted) Then
                            udtEHSAccount.RecordStatus = VRAcctValidatedStatus.PendingForVerify
                        End If
                        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

                        ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
                        If Not (New DocTypeBLL).getAllDocType.Filter(udtEHSAccount.EHSPersonalInformationList(0).DocCode).IMMDorManualValidationAvailable Then
                            udtEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.NotForImmDValidation
                        End If
                        ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]

                        Dim dtmCurrentDate = udtGeneralFunction.GetSystemDateTime

                        'CRE13-006 HCVS Ceiling [Start][Karl]
                        'Me.udtEHSAccountBLL.UpdateEHSAccountRectify(udtEHSAccount, strUpdateBy, dtmCurrentDate)
                        Me.udtEHSAccountBLL.UpdateEHSAccountRectify(udtOrgEHSAccount, udtEHSAccount, strUpdateBy, dtmCurrentDate)
                        'CRE13-006 HCVS Ceiling [End][Karl]

                    Else
                        strNewAccountID = udtGeneralFunction.generateSystemNum("C")

                        Me.udtAuditLogEntry.AddDescripton("DirectUpdateExistingAccount", "False")
                        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID)
                        Me.udtAuditLogEntry.AddDescripton("NewAccountID", strNewAccountID)

                        Dim udtNew_EHSAccount As EHSAccountModel

                        udtNew_EHSAccount = udtEHSAccount.CloneData()
                        udtNew_EHSAccount.VoucherAccID = strNewAccountID
                        udtNew_EHSAccount.CreateSPPracticeDisplaySeq = udtPracticeDisplay.PracticeID

                        'CRE13-006 HCVS Ceiling [Start][Karl]
                        udtNew_EHSAccount.SubsidizeWriteOff_CreateReason = eHASubsidizeWriteOff_CreateReason.PersonalInfoAmend
                        sm = udtEHSClaimBLL.CreateRectifyAccount(udtSP, udtDataEntry, udtOrgEHSAccount, udtNew_EHSAccount)
                        'sm = udtEHSClaimBLL.CreateRectifyAccount(udtSP, udtDataEntry, udtEHSAccount, udtNew_EHSAccount)
                        'CRE13-006 HCVS Ceiling [End][Karl]                        
                    End If

                    If IsNothing(sm) Then
                        Dim strMsgCode As String = String.Empty
                        If Me.IsReusedAcc(udtEHSAccount.OriginalAccID) And Not udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.SpecialAccount Then
                            'retake eHS account from DB and save it to session
                            GeteHSAccount(udtEHSAccount.EHSPersonalInformationList(0).DocCode, strNewAccountID, udtEHSAccount.AccountSourceString)

                            strMsgCode = MsgCode.MSG00006 '"00006"
                            Dim strOld As String() = {"%s"}
                            Dim strNew As String() = {""}
                            strNew(0) = Me.udtFormatter.formatSystemNumber(udtEHSAccount.VoucherAccID.Trim)
                            sm = New Common.ComObject.SystemMessage(FuncCode, SeverityCode.SEVI, strMsgCode)
                            Me.udcInfoMsgBox.AddMessage(sm, strOld, strNew)
                        Else
                            strMsgCode = MsgCode.MSG00001 '"00001"
                            sm = New Common.ComObject.SystemMessage(FuncCode, SeverityCode.SEVI, strMsgCode)
                            Me.udcInfoMsgBox.AddMessage(sm)
                        End If

                        Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Complete
                        Me.udcInfoMsgBox.BuildMessageBox()

                        'audit log
                        If blnModifyAcc Then
                            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00024, AuditLogDesc.ModifyAccountSuccess)
                        Else
                            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00021, AuditLogDesc.RectifyAccountSuccess)
                        End If

                        Me.panCompleteVoidTrans.Visible = False
                        Me.mvRectify.ActiveViewIndex = intComplete
                    Else
                        Me.udcMsgBoxErr.AddMessage(sm)
                        'audit log
                        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID)
                        If blnModifyAcc Then
                            Me.udcMsgBoxErr.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00025, AuditLogDesc.ModifyAccountFail)
                        Else
                            Me.udcMsgBoxErr.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00022, AuditLogDesc.RectifyAccountFail)
                        End If

                        Me.panCompleteVoidTrans.Visible = False

                    End If
                Else
                    Me.udcMsgBoxErr.AddMessage(sm)

                    'audit log
                    Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID)
                    If blnModifyAcc Then
                        Me.udcMsgBoxErr.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00025, AuditLogDesc.ModifyAccountFail)
                    Else
                        Me.udcMsgBoxErr.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00022, AuditLogDesc.RectifyAccountFail)
                    End If

                    Me.panCompleteVoidTrans.Visible = False

                End If
                ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

            End If

        Catch eSQL As SqlException
            If eSQL.Number = 50000 Then

                sm = New Common.ComObject.SystemMessage("990001", SeverityCode.SEVD, eSQL.Message)
                Me.udcMsgBoxErr.AddMessage(sm)

                'Log Save Fail
                Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID)
                If blnModifyAcc Then
                    Me.udcMsgBoxErr.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00025, AuditLogDesc.ModifyAccountFail)
                Else
                    Me.udcMsgBoxErr.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00022, AuditLogDesc.RectifyAccountFail)
                End If

                Me.panCompleteVoidTrans.Visible = False
                Me.mvRectify.ActiveViewIndex = intComplete
            Else
                Throw eSQL
            End If
        Catch ex As Exception
            Throw

        End Try

    End Sub

    Private Sub chkDeclaration_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDeclaration.CheckedChanged
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", Me.udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", Me.udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.AddDescripton("DocCode", Me.udtEHSAccount.EHSPersonalInformationList(0).DocCode)
        Me.udtAuditLogEntry.AddDescripton("Checked", chkDeclaration.Checked)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00047, AuditLogDesc.chkDeclaration)

        If chkDeclaration.Checked Then
            Me.ibtnConfirm.Enabled = True
            Me.ibtnConfirm.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "ConfirmBtn")
        Else
            Me.ibtnConfirm.Enabled = False
            Me.ibtnConfirm.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "ConfirmDisableBtn")
        End If
    End Sub

#End Region

#Region "View 5 - Complete View Button Action"

    Protected Sub ibtnCompleteReturn_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00034, AuditLogDesc.ReturnFromCompletePage)

        Me.mvRectify.ActiveViewIndex = intSearchView
        Me.ibtnSearch_Click(Nothing, Nothing)
    End Sub

#End Region

#Region "View 6 - Practice Selection View Button Action"

    Protected Sub PracticeRadioButtonGroup_PracticeSelected(ByVal strPracticeName As String, ByVal strBankAcctNo As String, ByVal intBankAccountDisplaySeq As Integer, ByVal strSchemeCode As String, ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles PracticeRadioButtonGroup.PracticeSelected
        'Audit log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("PracticeID", intBankAccountDisplaySeq)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00010, AuditLogDesc.SelectPractice)

        Dim udtSelectedPracticeDisplay As BLL.PracticeDisplayModel = Nothing
        Dim udtPracticeDisplays As BLL.PracticeDisplayModelCollection = Nothing
        Dim udtUpdatedPracticedisplay As BLL.PracticeDisplayModel = Nothing
        Dim udtSmartIDContent As SmartIDContentModel = Nothing

        udtPracticeDisplays = Session(SESS_PracticeCollection)
        udtUpdatedPracticedisplay = udtPracticeDisplays.Filter(intBankAccountDisplaySeq)

        Dim blnErr As Boolean = False

        If IsNothing(udtUpdatedPracticedisplay) Then
            sm = New Common.ComObject.SystemMessage(FuncCode, SeverityCode.SEVE, MsgCode.MSG00002)
            If Not IsNothing(sm) Then
                blnErr = True
                Me.udcMsgBoxErr.AddMessage(sm)
            End If
        End If

        If blnErr Then
            Me.udtAuditLogEntry.AddDescripton("PracticeID", intBankAccountDisplaySeq)
            Me.udcMsgBoxErr.BuildMessageBox(strValidationFail, udtAuditLogEntry, LogID.LOG00012, AuditLogDesc.SelectPracticeFail)
        Else
            udtSmartIDContent = Me.udtSessionHandler.SmartIDContentGetFormSession(FuncCode)
            Session(SESS_InputMode) = ucInputDocTypeBase.BuildMode.Modification
            'Me.BindPersonalInfo(udtEHSAccount, ucInputDocumentType.Mode.Modification, True)
            Me.SetupRectifyDetailScreen(True)

            'Save Practice to session Practice ID will assign to EHS Account before confirm save
            Me.udtSessionHandler.PracticeDisplaySaveToSession(udtUpdatedPracticedisplay, FuncCode)

            If Not udtSmartIDContent Is Nothing AndAlso udtSmartIDContent.IsReadSmartID Then
                '==================================================================== Code for SmartID ============================================================================
                'udtSmartIDContent.EHSAccount.CreateSPPracticeDisplaySeq = udtUpdatedPracticedisplay.PracticeID
                'Me.udtSessionHandler.SmartIDContentSaveToSession(FuncCode, udtSmartIDContent)

                Me.mvRectify.ActiveViewIndex = EHSRectification.intSmartIDConfirmation
                '==================================================================================================================================================================
            Else
                'Me.udtEHSAccount = Me.udtSessionHandler.EHSAccountGetFromSession(FuncCode)
                'udtEHSAccount.CreateSPPracticeDisplaySeq = udtUpdatedPracticedisplay.PracticeID
                'Me.udtSessionHandler.EHSAccountSaveToSession(Me.udtEHSAccount, FuncCode)

                Me.mvRectify.ActiveViewIndex = EHSRectification.intRectifyAccount
            End If


            Me.udtAuditLogEntry.AddDescripton("PracticeID", udtUpdatedPracticedisplay.PracticeID)
            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00011, AuditLogDesc.SelectPracticeSuccess)
        End If

    End Sub

#End Region

#Region "View 7 - Transaction View Button Action"

    Protected Sub ibtnTranBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtEHSAccount = Me.udtSessionHandler.EHSAccountGetFromSession(FuncCode)

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", Me.udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00040, AuditLogDesc.BackToAccDetailFromViewTran)

        Me.mvRectify.ActiveViewIndex = intRectifyAccount

        udcClaimTran.Clear()
    End Sub

    Protected Sub ibtnTranVoid_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtEHSAccount = Me.udtSessionHandler.EHSAccountGetFromSession(FuncCode)

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", Me.udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", Me.udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.AddDescripton("TransactionID", Me.udtEHSAccount.TransactionID.Trim)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00041, AuditLogDesc.ClickToVoidTran)

        Me.udcMsgBoxErr.Visible = False
        Me.pnlTranActionBtn.Visible = False
        Me.pnlVoidTran.Visible = True
        Me.txtVoidReason.Text = String.Empty
        Me.imgVoidReasonErr.Visible = False
    End Sub

    Protected Sub ibtnVoidTranBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtEHSAccount = Me.udtSessionHandler.EHSAccountGetFromSession(FuncCode)

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", Me.udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", Me.udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.AddDescripton("TransactionID", Me.udtEHSAccount.TransactionID.Trim)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00042, AuditLogDesc.ClickBackToCancelVoidTran)

        Me.udcMsgBoxErr.Visible = False
        Me.pnlTranActionBtn.Visible = True
        Me.pnlVoidTran.Visible = False
        Me.txtVoidReason.Text = String.Empty
        Me.imgVoidReasonErr.Visible = False
    End Sub

    Protected Sub ibtnVoidTranConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtEHSAccount = Me.udtSessionHandler.EHSAccountGetFromSession(FuncCode)

        Me.udcMsgBoxErr.Visible = False
        Me.imgVoidReasonErr.Visible = False

        Dim blnSuccess As Boolean = False

        If Me.txtVoidReason.Text.Trim.Equals(String.Empty) Then
            'Audit Log
            Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
            Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
            Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
            Me.udtAuditLogEntry.AddDescripton("TransactionID", udtEHSAccount.TransactionID.Trim)
            Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00046, AuditLogDesc.VoidTransactionFailNoVoidReason)

            Me.sm = New Common.ComObject.SystemMessage(FuncCode, SeverityCode.SEVE, MsgCode.MSG00001)
            Me.udcMsgBoxErr.AddMessage(sm)
            Me.imgVoidReasonErr.Visible = True
        End If

        If Me.udcMsgBoxErr.GetCodeTable.Rows.Count = 0 Then
            Dim udtEHSTransactionBLL As Common.Component.EHSTransaction.EHSTransactionBLL = New Common.Component.EHSTransaction.EHSTransactionBLL
            Dim udtTranMainBLL As TransactionMaintenanceBLL = New TransactionMaintenanceBLL

            'Audit Log
            Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
            Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
            Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
            Me.udtAuditLogEntry.AddDescripton("TransactionID", udtEHSAccount.TransactionID.Trim)
            Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00043, AuditLogDesc.VoidTransaction)

            Try
                Dim udtEHSTransaction As Common.Component.EHSTransaction.EHSTransactionModel

                GetCurrentUser(udtSP, udtDataEntry)

                udtEHSTransaction = udtEHSTransactionBLL.LoadClaimTran(udtEHSAccount.TransactionID.Trim)
                udtEHSTransaction.VoidReason = txtVoidReason.Text.Trim
                If Not IsNothing(udtDataEntry) Then
                    udtEHSTransaction.VoidUser = udtSP.SPID
                    udtEHSTransaction.VoidByDataEntry = udtDataEntry.DataEntryAccount
                Else
                    udtEHSTransaction.VoidUser = udtSP.SPID
                End If

                If udtTranMainBLL.OnVoid(udtEHSTransaction) Then
                    blnSuccess = True
                Else
                    Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                    Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
                    Me.udtAuditLogEntry.AddDescripton("TransactionID", udtEHSAccount.TransactionID.Trim)

                    Me.sm = New Common.ComObject.SystemMessage(CommonFuncCode, SeverityCode.SEVE, MsgCode.MSG00184)
                    Me.udcMsgBoxErr.AddMessage(sm)
                    Me.udcMsgBoxErr.BuildMessageBox("UpdateFail", udtAuditLogEntry, Common.Component.LogID.LOG00045, AuditLogDesc.VoidTransactionFail)
                End If


            Catch eSQL As SqlClient.SqlException
                If eSQL.Number = 50000 Then
                    'Log Void Transaction Fail and Build Message Box
                    sm = New Common.ComObject.SystemMessage("990001", "D", eSQL.Message)
                    Me.udcMsgBoxErr.AddMessage(sm)
                    Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                    Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
                    Me.udtAuditLogEntry.AddDescripton("TransactionID", udtEHSAccount.TransactionID.Trim)
                    Me.udcMsgBoxErr.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00045, AuditLogDesc.VoidTransactionFail)

                End If
            Catch ex As Exception
                Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
                Me.udtAuditLogEntry.AddDescripton("TransactionID", udtEHSAccount.TransactionID.Trim)
                Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00045, AuditLogDesc.VoidTransactionFail)

                Throw ex
            End Try

            If blnSuccess Then

                Dim udtEHSUpdatedTransaction As Common.Component.EHSTransaction.EHSTransactionModel
                udtEHSUpdatedTransaction = udtEHSTransactionBLL.LoadClaimTran(udtEHSAccount.TransactionID.Trim)

                Dim strOldString As String()
                Dim strNewString As String()


                strOldString = New String() {"%r", "%s"}
                strNewString = New String() {Me.udtFormatter.formatSystemNumber(udtEHSUpdatedTransaction.EHSAcct.VoucherAccID), Me.udtFormatter.formatSystemNumber(udtEHSUpdatedTransaction.VoidTranNo)}
                sm = New Common.ComObject.SystemMessage(FuncCode, SeverityCode.SEVI, MsgCode.MSG00007)

                Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Complete
                Me.udcInfoMsgBox.AddMessage(sm, strOldString, strNewString)
                Me.udcInfoMsgBox.BuildMessageBox()

                ViewState("RejectDatetime") = udtEHSUpdatedTransaction.VoidDate
                Me.lblRejectDate.Text = udtFormatter.formatDateTime(udtEHSUpdatedTransaction.VoidDate)
                Me.lblRejectReferenceNo.Text = udtFormatter.formatSystemNumber(udtEHSUpdatedTransaction.VoidTranNo)

                Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
                Me.udtAuditLogEntry.AddDescripton("TransactionID", udtEHSAccount.TransactionID.Trim)
                Me.udtAuditLogEntry.AddDescripton("VoidTransNo", udtEHSUpdatedTransaction.VoidTranNo)
                Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00044, AuditLogDesc.VoidTransactionSuccess)

                Me.panCompleteVoidTrans.Visible = True
                Me.mvRectify.ActiveViewIndex = intComplete
            Else
                Me.panCompleteVoidTrans.Visible = False
                Me.mvRectify.ActiveViewIndex = intComplete

            End If
        Else

            Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
            Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
            Me.udtAuditLogEntry.AddDescripton("TransactionID", udtEHSAccount.TransactionID.Trim)
            Me.udcMsgBoxErr.BuildMessageBox(strValidationFail, udtAuditLogEntry, Common.Component.LogID.LOG00045, AuditLogDesc.VoidTransactionFail)
        End If
    End Sub

#End Region

    '==================================================================== Code for SmartID ============================================================================
#Region "View 8 - Smart ID confirmation"

    Private Sub udcSmartIDConfirmationInputDocumentType_SelectGender(ByVal udcInputHKIC As ucInputDocTypeBase, ByVal sender As Object, ByVal e As System.EventArgs) Handles udcSmartIDConfirmationInputDocumentType.SelectGender

        Dim udcSmartIDContent As BLL.SmartIDContentModel = Me.udtSessionHandler.SmartIDContentGetFormSession(FuncCode)
        Dim strGender As String = String.Empty
        Dim isShowSmartIDDiff As Boolean = False

        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        'Dim udcInputHKIDSmartID As ucInputHKIDSmartID = udcInputHKIC
        'udcInputHKIDSmartID.SetProperty(ucInputDocTypeBase.BuildMode.Creation)
        'strGender = udcInputHKIDSmartID.Gender
        'isShowSmartIDDiff = True

        Select Case udcSmartIDContent.SmartIDReadStatus
            Case BLL.SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_NotCreateBySmartID, _
                 BLL.SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_DiffDOIDOB, _
                 BLL.SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB_WithoutGender_SameName

                Dim udcInputHKIDSmartID As ucInputHKIDSmartID = udcInputHKIC
                udcInputHKIDSmartID.SetProperty(ucInputDocTypeBase.BuildMode.Creation)
                strGender = udcInputHKIDSmartID.Gender

                isShowSmartIDDiff = True

            Case Else
                Dim udcInputHKIDSmartIDSignal As ucInputHKIDSmartIDSignal = udcInputHKIC
                udcInputHKIDSmartIDSignal.SetProperty(ucInputDocTypeBase.BuildMode.Creation)
                strGender = udcInputHKIDSmartIDSignal.Gender
                isShowSmartIDDiff = False
        End Select
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

        If Not String.IsNullOrEmpty(strGender) Then

            If Not IsNothing(Session(SESS_ModifyAcc)) Then
                If CBool(Session(SESS_ModifyAcc)) = True Then
                    panSmartIDConfirmationConsent.Visible = True
                    Me.chkSmartIDConfirmationConsent.Enabled = True

                Else
                    Me.EnableConfirmButton(True, Me.ibtnSmartIDConfirmationConfirm)
                    panSmartIDConfirmationConsent.Visible = False

                End If
            Else
                Me.EnableConfirmButton(True, Me.ibtnSmartIDConfirmationConfirm)
                panSmartIDConfirmationConsent.Visible = False
            End If

            udcSmartIDContent.EHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).Gender = strGender
            Me.udtSessionHandler.SmartIDContentSaveToSession(FuncCode, udcSmartIDContent)

        End If
    End Sub

    Private Sub ibtnSmartIDConfirmationBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSmartIDConfirmationBack.Click
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00032, AuditLogDesc.BackToSearch)
        Me.udtSessionHandler.SmartIDContentRemoveFormSession(FuncCode)
        Me.udtEHSAccount = Me.udtSessionHandler.EHSAccountGetFromSession(FuncCode)

        Session(SESS_InputMode) = Me.AccountMode(Me.udtEHSAccount)
        Session(SESS_DefaultSetCCCode) = True
        Me.txtDocCode.Text = DocType.DocTypeModel.DocTypeCode.HKIC
        Me.SetupRectifyDetailScreen(False)

        Me.mvRectify.ActiveViewIndex = intRectifyAccount

        Me.udcMsgBoxErr.Clear()
        Me.udcInfoMsgBox.Clear()
    End Sub

    Private Sub ibtnSmartIDConfirmationConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSmartIDConfirmationConfirm.Click
        Dim udcSmartIDContent As BLL.SmartIDContentModel = Me.udtSessionHandler.SmartIDContentGetFormSession(FuncCode)
        Dim udtExistingEHSAccount As EHSAccountModel = Me.udtSessionHandler.EHSAccountGetFromSession(FuncCode)
        Dim strUpdateBy As String = String.Empty
        Dim strDataEntry As String = String.Empty
        Dim blnChkDeclare As Boolean = False
        Dim sm As SystemMessage
        Dim blnModifyAcc As Boolean = False
        'CRE13-006 HCVS Ceiling [Start][Karl]
        Dim udtOrgEHSAccount As EHSAccountModel = CType(Me.Session(SESS_OrgEHSAccount), EHSAccountModel)
        'CRE13-006 HCVS Ceiling [End][Karl]

        ' I-CRE17-003 (Enhance eHA rectification on HCSP to check with eHS(S) and CMS vaccination) [Start][Winnie]
        Me.udcMsgBoxErr.Clear()
        ' I-CRE17-003 (Enhance eHA rectification on HCSP to check with eHS(S) and CMS vaccination) [End][Winnie]

        GetCurrentUser(udtSP, udtDataEntry)

        If Not IsNothing(udtDataEntry) Then
            strUpdateBy = udtDataEntry.DataEntryAccount
            strDataEntry = udtDataEntry.DataEntryAccount
        Else
            strUpdateBy = udtSP.SPID
        End If

        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtEHSAccount = udcSmartIDContent.EHSAccount

        'Combine SmartID Account and DataBase Record 
        Me.udtEHSAccount = Me.udtEHSAccount.CloneDataForSmartIC(udtExistingEHSAccount, udtSP.SPID, strDataEntry, Me.udtEHSAccount.CreateSPPracticeDisplaySeq)

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)

        If Me.IsReusedAcc(udtEHSAccount.OriginalAccID) Then
            blnModifyAcc = True
            blnChkDeclare = Me.chkSmartIDConfirmationConsent.Checked
            Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00023, AuditLogDesc.ModifyAccount)
        Else
            blnModifyAcc = False
            blnChkDeclare = True
            Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00020, AuditLogDesc.RectifyAccount)
        End If


        Try
            If blnChkDeclare Then
                Dim udtClaimRulesBLL As New ClaimRules.ClaimRulesBLL
                Dim udtEligibleResult As ClaimRules.ClaimRulesBLL.EligibleResult
                Dim strNewAccountID As String = String.Empty
                Dim udtPracticeDisplay As PracticeDisplayModel = Me.udtSessionHandler.PracticeDisplayGetFromSession(FuncCode)

                Dim udtDirectUpdateExistingAccount As Boolean = False

                ' I-CRE17-003 (Enhance eHA rectification on HCSP to check with eHS(S) and CMS vaccination) [Start][Winnie]                
                Dim blnConfirmDeclaration As Boolean = False

                If Not IsNothing(Session(SESS_ConfirmDeclaration)) Then
                    blnConfirmDeclaration = CBool(Session(SESS_ConfirmDeclaration))
                End If

                Dim blnShowDeclaration As Boolean = False

                If Not blnConfirmDeclaration Then
                    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                    Dim udtTranDetailVaccineList As TransactionDetailVaccineModelCollection = Nothing
                    Dim enumCheckEligiblity As ClaimRules.ClaimRulesBLL.Eligiblity = ClaimRules.ClaimRulesBLL.Eligiblity.Check

                    Me.udtSessionHandler.CMSVaccineResultRemoveFromSession(FuncCode)
                    Me.udtSessionHandler.CIMSVaccineResultRemoveFromSession(FuncCode)

                    If udtEHSAccount.TransactionID <> "" Then
                        udtTranDetailVaccineList = Me.GetVaccinationRecord(udtEHSAccount, sm)
                    Else
                        'enumCheckEligiblity = ClaimRules.ClaimRulesBLL.Eligiblity.NotCheck
                    End If

                    If IsNothing(sm) Then
                        sm = udtClaimRulesBLL.CheckRectifyEHSAccount(udtEHSAccount.SchemeCode.Trim(), Me.txtDocCode.Text.Trim, _
                                                                     udtEHSAccount, udtEligibleResult, udtTranDetailVaccineList, _
                                                                     enumCheckEligiblity, ClaimRules.ClaimRulesBLL.Unique.Include_Self_EHSAccount)

                    End If
                    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

                    If IsNothing(sm) Then
                        If Not IsNothing(udtEligibleResult) Then
                            If udtEligibleResult.HandleMethod = ClaimRules.ClaimRulesBLL.HandleMethodENum.Declaration Then

                                Dim strText As String = String.Empty
                                If Not udtEligibleResult.RelatedEligibleRule Is Nothing AndAlso Not String.IsNullOrEmpty(udtEligibleResult.RelatedEligibleRule.ObjectName3) Then
                                    strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleRule.ObjectName3.Trim())
                                ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing AndAlso Not String.IsNullOrEmpty(udtEligibleResult.RelatedEligibleExceptionRule.ObjectName3) Then
                                    strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleExceptionRule.ObjectName3.Trim())
                                ElseIf Not udtEligibleResult.RelatedClaimCategoryEligibilityModel Is Nothing AndAlso Not String.IsNullOrEmpty(udtEligibleResult.RelatedClaimCategoryEligibilityModel.ObjectName3) Then
                                    strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedClaimCategoryEligibilityModel.ObjectName3.Trim())
                                End If

                                If Not String.IsNullOrEmpty(strText) Then
                                    Me.lblClamDeclaration.Text = strText
                                    ModalPopupExtenderClaimDEclaration.Show()
                                    blnShowDeclaration = True
                                End If
                            End If
                        End If
                    Else
                        'audit log
                        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID)
                        Me.udcMsgBoxErr.AddMessage(sm)
                        If blnModifyAcc Then
                            Me.udcMsgBoxErr.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00025, AuditLogDesc.ModifyAccountFail)
                        Else
                            Me.udcMsgBoxErr.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00022, AuditLogDesc.RectifyAccountFail)
                        End If

                        Me.panCompleteVoidTrans.Visible = False
                    End If
                End If

                If IsNothing(sm) AndAlso blnShowDeclaration = False Then
                    ' I-CRE17-003 (Enhance eHA rectification on HCSP to check with eHS(S) and CMS vaccination) [End][Winnie]

                    If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.TemporaryAccount Then
                        If IsReusedAcc(udtEHSAccount.OriginalAccID) Then
                            udtDirectUpdateExistingAccount = False
                        Else
                            udtDirectUpdateExistingAccount = True
                        End If
                    ElseIf udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.SpecialAccount Then
                        udtDirectUpdateExistingAccount = True

                    End If

                    If udtDirectUpdateExistingAccount Then
                        Me.udtAuditLogEntry.AddDescripton("DirectUpdateExistingAccount", "true")
                        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID)

                        If udtEHSAccount.RecordStatus.Trim.Equals(VRAcctValidatedStatus.Invalid) Then
                            udtEHSAccount.RecordStatus = VRAcctValidatedStatus.PendingForVerify
                        End If

                        'CRE13-006 HCVS Ceiling [Start][Karl]
                        'Me.udtEHSAccountBLL.UpdateEHSAccountRectify(udtEHSAccount, strUpdateBy, Me.udtEHSAccount.CreateDtm)
                        Me.udtEHSAccountBLL.UpdateEHSAccountRectify(udtOrgEHSAccount, udtEHSAccount, strUpdateBy, Me.udtEHSAccount.CreateDtm)
                        'CRE13-006 HCVS Ceiling [End][Karl]
                    Else
                        strNewAccountID = udtGeneralFunction.generateSystemNum("C")

                        Me.udtAuditLogEntry.AddDescripton("DirectUpdateExistingAccount", "False")
                        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID)
                        Me.udtAuditLogEntry.AddDescripton("NewAccountID", strNewAccountID)

                        Dim udtNew_EHSAccount As EHSAccountModel

                        udtNew_EHSAccount = udtEHSAccount.CloneData()
                        udtNew_EHSAccount.VoucherAccID = strNewAccountID
                        udtNew_EHSAccount.CreateSPPracticeDisplaySeq = udtPracticeDisplay.PracticeID

                        'udtEHSAccount use for get the original ID

                        'CRE13-006 HCVS Ceiling [Start][Karl]
                        udtNew_EHSAccount.SubsidizeWriteOff_CreateReason = eHASubsidizeWriteOff_CreateReason.PersonalInfoAmend
                        sm = udtEHSClaimBLL.CreateRectifyAccount(udtSP, udtDataEntry, udtOrgEHSAccount, udtNew_EHSAccount)
                        'sm = udtEHSClaimBLL.CreateRectifyAccount(udtSP, udtDataEntry, udtEHSAccount, udtNew_EHSAccount)
                        'CRE13-006 HCVS Ceiling [End][Karl]          

                    End If

                    If IsNothing(sm) Then
                        Dim strMsgCode As String = String.Empty
                        If Me.IsReusedAcc(udtEHSAccount.OriginalAccID) And Not udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.SpecialAccount Then
                            'retake eHS account from DB and save it to session
                            GeteHSAccount(udtEHSAccount.EHSPersonalInformationList(0).DocCode, strNewAccountID, udtEHSAccount.AccountSourceString)

                            strMsgCode = MsgCode.MSG00006 '"00006"
                            Dim strOld As String() = {"%s"}
                            Dim strNew As String() = {""}
                            strNew(0) = Me.udtFormatter.formatSystemNumber(udtEHSAccount.VoucherAccID.Trim)
                            sm = New Common.ComObject.SystemMessage(FuncCode, SeverityCode.SEVI, strMsgCode)
                            Me.udcInfoMsgBox.AddMessage(sm, strOld, strNew)
                        Else
                            strMsgCode = MsgCode.MSG00001 '"00001"
                            sm = New Common.ComObject.SystemMessage(FuncCode, SeverityCode.SEVI, strMsgCode)
                            Me.udcInfoMsgBox.AddMessage(sm)
                        End If

                        Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Complete
                        Me.udcInfoMsgBox.BuildMessageBox()

                        'audit log
                        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID)
                        If blnModifyAcc Then
                            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00024, AuditLogDesc.ModifyAccountSuccess)
                        Else
                            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00021, AuditLogDesc.RectifyAccountSuccess)
                        End If

                        Me.panCompleteVoidTrans.Visible = False
                        Me.mvRectify.ActiveViewIndex = intComplete

                    Else
                        Me.udcMsgBoxErr.AddMessage(sm)

                        'audit log
                        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID)
                        If blnModifyAcc Then
                            Me.udcMsgBoxErr.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00025, AuditLogDesc.ModifyAccountFail)
                        Else
                            Me.udcMsgBoxErr.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00022, AuditLogDesc.RectifyAccountFail)
                        End If

                        Me.panCompleteVoidTrans.Visible = False
                        Me.mvRectify.ActiveViewIndex = intComplete
                    End If
                End If
            End If

        Catch eSQL As SqlException
            If eSQL.Number = 50000 Then

                sm = New Common.ComObject.SystemMessage("990001", SeverityCode.SEVD, eSQL.Message)
                Me.udcMsgBoxErr.AddMessage(sm)

                'Log Save Fail
                Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID)
                If blnModifyAcc Then
                    Me.udcMsgBoxErr.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00025, AuditLogDesc.ModifyAccountFail)
                Else
                    Me.udcMsgBoxErr.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00022, AuditLogDesc.RectifyAccountFail)
                End If

                Me.panCompleteVoidTrans.Visible = False
                Me.mvRectify.ActiveViewIndex = intComplete
            Else
                Throw eSQL
            End If
        Catch ex As Exception
            Throw

        End Try

    End Sub

    Private Sub chkSmartIDConfirmationConsent_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSmartIDConfirmationConsent.CheckedChanged

    End Sub

#End Region
    '==================================================================================================================================================================

#Region "Enter Details Validation"

    'HKID
    Private Function ValidateRectifyDetail_HKID(ByRef _udtEHSAccount As EHSAccountModel, ByVal blnSmartIDCase As Boolean, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isValid As Boolean = True

        Dim udcInputHKIC As ucInputHKID = Me.udcRectifyAccount.GetHKICControl
        udcInputHKIC.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
        udcInputHKIC.SetErrorImage_M(False)

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
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00014, AuditLogDesc.ValidateRectifiedAccount)

        '--HKIC Amended 30 Nov 2009
        Me.sm = Me.validator.chkIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, udcInputHKIC.HKID, String.Empty)
        If Not IsNothing(sm) Then
            isValid = False
            Me.udcMsgBoxErr.AddMessage(sm)
        End If
        '--

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String
        Dim dtmDOB As Date

        strDOB = udcInputHKIC.DOB
        Me.sm = Me.validator.chkDOB(DocType.DocTypeModel.DocTypeCode.HKIC, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputHKIC.SetDOBModificationError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'English Name
        Me.sm = Me.validator.chkEngName(udcInputHKIC.ENameSurName, udcInputHKIC.ENameFirstName, DocTypeModel.DocTypeCode.HKIC)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputHKIC.SetENameModificationError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        If Not blnSmartIDCase Then
            'CCCode
            Me.sm = Me.validator.chkCCCode_UsingDDL(String.Format("{0}{1}", udcInputHKIC.CCCode1, Me.udcCCCode.SelectedCCCodeTail1), _
                                                    String.Format("{0}{1}", udcInputHKIC.CCCode2, Me.udcCCCode.SelectedCCCodeTail2), _
                                                    String.Format("{0}{1}", udcInputHKIC.CCCode3, Me.udcCCCode.SelectedCCCodeTail3), _
                                                    String.Format("{0}{1}", udcInputHKIC.CCCode4, Me.udcCCCode.SelectedCCCodeTail4), _
                                                    String.Format("{0}{1}", udcInputHKIC.CCCode5, Me.udcCCCode.SelectedCCCodeTail5), _
                                                    String.Format("{0}{1}", udcInputHKIC.CCCode6, Me.udcCCCode.SelectedCCCodeTail6))
            If Not Me.sm Is Nothing Then
                isValid = False
                udcInputHKIC.SetCCCodeModificationError(True)
                Me.udcMsgBoxErr.AddMessage(sm)
            End If
        End If

        'HKIC Gender
        Me.sm = Me.validator.chkGender(udcInputHKIC.Gender)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputHKIC.SetGenderModificationError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'DOI
        Dim strHKIDIssueDate As String = Nothing
        Dim dtHKIDIssueDate As DateTime
        strHKIDIssueDate = Me.formatter.formatHKIDIssueDateBeforeValidate(udcInputHKIC.HKIDIssuseDate)
        'Me.sm = Me.validator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.HKIC, udcInputHKIC.HKIDIssuseDate, dtmDOB)
        Me.sm = Me.validator.chkHKIDIssueDate(strHKIDIssueDate, dtmDOB)
        If Not Me.sm Is Nothing Then
            isValid = False
            udcInputHKIC.SetHKIDIssueDateModificationError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        Else
            dtHKIDIssueDate = Me.formatter.convertHKIDIssueDateStringToDate(strHKIDIssueDate)
        End If

        If isValid Then
            Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKIC)

            udtEHSAccountPersonalInfo.ENameSurName = udcInputHKIC.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputHKIC.ENameFirstName

            If Not blnSmartIDCase Then
                udtEHSAccountPersonalInfo.CCCode1 = String.Format("{0}{1}", udcInputHKIC.CCCode1, Me.udcCCCode.SelectedCCCodeTail1)
                udtEHSAccountPersonalInfo.CCCode2 = String.Format("{0}{1}", udcInputHKIC.CCCode2, Me.udcCCCode.SelectedCCCodeTail2)
                udtEHSAccountPersonalInfo.CCCode3 = String.Format("{0}{1}", udcInputHKIC.CCCode3, Me.udcCCCode.SelectedCCCodeTail3)
                udtEHSAccountPersonalInfo.CCCode4 = String.Format("{0}{1}", udcInputHKIC.CCCode4, Me.udcCCCode.SelectedCCCodeTail4)
                udtEHSAccountPersonalInfo.CCCode5 = String.Format("{0}{1}", udcInputHKIC.CCCode5, Me.udcCCCode.SelectedCCCodeTail5)
                udtEHSAccountPersonalInfo.CCCode6 = String.Format("{0}{1}", udcInputHKIC.CCCode6, Me.udcCCCode.SelectedCCCodeTail6)
            Else
                If udtEHSAccountPersonalInfo.CCCode1 Is Nothing Then udtEHSAccountPersonalInfo.CCCode1 = String.Empty
                If udtEHSAccountPersonalInfo.CCCode2 Is Nothing Then udtEHSAccountPersonalInfo.CCCode2 = String.Empty
                If udtEHSAccountPersonalInfo.CCCode3 Is Nothing Then udtEHSAccountPersonalInfo.CCCode3 = String.Empty
                If udtEHSAccountPersonalInfo.CCCode4 Is Nothing Then udtEHSAccountPersonalInfo.CCCode4 = String.Empty
                If udtEHSAccountPersonalInfo.CCCode5 Is Nothing Then udtEHSAccountPersonalInfo.CCCode5 = String.Empty
                If udtEHSAccountPersonalInfo.CCCode6 Is Nothing Then udtEHSAccountPersonalInfo.CCCode6 = String.Empty
            End If

            'Retervie Chinese Name from Choose
            udcInputHKIC.CCCode1 = udtEHSAccountPersonalInfo.CCCode1
            udcInputHKIC.CCCode2 = udtEHSAccountPersonalInfo.CCCode2
            udcInputHKIC.CCCode3 = udtEHSAccountPersonalInfo.CCCode3
            udcInputHKIC.CCCode4 = udtEHSAccountPersonalInfo.CCCode4
            udcInputHKIC.CCCode5 = udtEHSAccountPersonalInfo.CCCode5
            udcInputHKIC.CCCode6 = udtEHSAccountPersonalInfo.CCCode6
            udcInputHKIC.SetCName()
            udtEHSAccountPersonalInfo.CName = udcInputHKIC.CName

            udtEHSAccountPersonalInfo.DOB = dtmDOB 'CDate(Me.udtFormatter.convertDate(strDOB, Common.Component.CultureLanguage.English))
            udtEHSAccountPersonalInfo.Gender = udcInputHKIC.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DateofIssue = dtHKIDIssueDate
        End If

        Return isValid
    End Function

    'EC
    Private Function ValidateRectifyDetail_EC(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isValid As Boolean = True
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.EC)

        Dim udcInputEC As ucInputEC = Me.udcRectifyAccount.GetECControl
        udcInputEC.SetProperty(Session(SESS_InputMode))
        udcInputEC.SetErrorImageModification(False)

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
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00014, AuditLogDesc.ValidateRectifiedAccount)

        '--HKIC Amended 30 Nov 2009
        Me.sm = Me.validator.chkIdentityNumber(DocType.DocTypeModel.DocTypeCode.EC, udcInputEC.HKID, String.Empty)
        If Not IsNothing(sm) Then
            isValid = False
            Me.udcMsgBoxErr.AddMessage(sm)
        End If
        '--

        'Serial Number
        Me.sm = Me.validator.chkSerialNo(udcInputEC.SerialNumber, udcInputEC.SerialNumberNotProvided)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputEC.SetECSerialNoError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'Reference
        Me.sm = Me.validator.chkReferenceNo(udcInputEC.Reference, udcInputEC.ReferenceOtherFormat)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputEC.SetECReferenceError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'DOB
        Dim sm_DOB As Common.ComObject.SystemMessage
        Dim sm_DOR As Common.ComObject.SystemMessage

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
                sm_DOB = New SystemMessage(CommonFuncCode, SeverityCode.SEVE, MsgCode.MSG00003)
            Case ucInputEC.DOBSelection.AgeWithDateOfRegistration
                'Check Age
                sm_DOB = validator.chkECAge(udcInputEC.ECAge)
                If Not sm_DOB Is Nothing Then
                    isValid = False
                    udcInputEC.SetDOBAgeModificationError(True)
                Else
                    strAge = udcInputEC.ECAge
                End If

                ' validate Date of Age
                sm_DOR = validator.chkECDOAge(strDateOfRegDay, strDateOfRegMth, strDateOfRegYr)
                If Not sm_DOR Is Nothing Then
                    isValid = False
                    udcInputEC.SetDateOfRegModificationError(True)
                Else
                    strDateOfReg = String.Format("{0:00}-{1}-{2}", Convert.ToInt32(strDateOfRegDay), strDateOfRegMth, strDateOfRegYr)

                    dtDOR = CDate(formatter.convertDate(strDateOfReg, udtSessionHandler.Language))
                End If

                ' validate Age + Date of Age if Within Age
                If isValid Then
                    sm_DOB = validator.chkECAgeAndDOAge(udcInputEC.ECAge, strDateOfRegDay, strDateOfRegMth, strDateOfRegYr)
                    If Not sm_DOB Is Nothing Then
                        isValid = False
                        udcInputEC.SetDOBAgeModificationError(True)
                        udcInputEC.SetDateOfRegModificationError(True)
                    Else
                        dtDOR = Date.ParseExact(strDateOfRegDay.Trim + " " + strDateOfRegMth.Trim + " " + strDateOfRegYr.Trim, "d M yyyy", Nothing, System.Globalization.DateTimeStyles.None)
                        strExactDOB = "A"
                        dtmDOB = dtDOR.Value.AddYears(-CInt(strAge))
                    End If
                End If
                'sm_DOB = Me.validator.chkECAgeAndDOAge(strAge, strDateOfRegDay, strDateOfRegMth, strDateOfRegYr)
                'If Not IsNothing(sm_DOB) Then
                '    isValid = False
                '    udcInputEC.SetDOBAgeModificationError(True)
                '    'Me.udcMsgBoxErr.AddMessage(sm_DOB)
                'Else
                '    dtDOR = Date.ParseExact(strDateOfRegDay.Trim + " " + strDateOfRegMth.Trim + " " + strDateOfRegYr.Trim, "d M yyyy", Nothing, System.Globalization.DateTimeStyles.None)

                '    strExactDOB = "A"
                '    dtmDOB = dtDOR.Value.AddYears(-CInt(strAge))
                'End If

            Case Else
                sm_DOB = Me.validator.chkDOB(DocTypeModel.DocTypeCode.EC, strDOB, dtmDOB, strExactDOB)

                If Not IsNothing(sm_DOB) Then
                    'Error Found, Invalid Data
                    Select Case udcInputEC.DOBtype
                        Case ucInputEC.DOBSelection.ExactDOB
                            isValid = False
                            udcInputEC.SetDOBDateModificationError(True)
                            'Me.udcMsgBoxErr.AddMessage(sm)

                        Case ucInputEC.DOBSelection.YearOfBirthReported
                            udcInputEC.SetDOByearModificationError(True)
                            isValid = False
                            'Me.udcMsgBoxErr.AddMessage(sm)

                        Case ucInputEC.DOBSelection.RecordOnTravDoc
                            isValid = False
                            udcInputEC.SetDOBTravelDocModificationError(True)
                            'Me.udcMsgBoxErr.AddMessage(sm)

                        Case ucInputEC.DOBSelection.AgeWithDateOfRegistration
                            isValid = False
                            udcInputEC.SetDOBAgeModificationError(True)
                            'Me.udcMsgBoxErr.AddMessage(sm)

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
                                    sm_DOB = New SystemMessage(CommonFuncCode, SeverityCode.SEVE, MsgCode.MSG00004)
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
        Me.sm = Me.validator.chkECDate(strECDateDay, strECDateMonth, strECDateYear, udtEHSAccountPersonalInfo.DOB)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputEC.SetECDateError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
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

            sm = validator.chkSerialNoNotProvidedAllow(dtmECDate, udcInputEC.SerialNumberNotProvided)
            If Not IsNothing(sm) Then
                isValid = False
                udcInputEC.SetECSerialNoError(True)
                udcMsgBoxErr.AddMessage(sm)
            End If

            ' Try parse the Reference if all the previous inputs are valid
            If isValid Then
                If udcInputEC.ReferenceOtherFormat Then
                    Dim dtmECDOI As New Date(udcInputEC.ECDateYear, udcInputEC.ECDateMonth, udcInputEC.ECDateDay)
                    validator.TryParseECReference(udcInputEC.Reference, udcInputEC.ReferenceOtherFormat, dtmECDOI)
                End If

            End If

            sm = validator.chkReferenceOtherFormatAllow(dtmECDate, udcInputEC.ReferenceOtherFormat)
            If Not IsNothing(sm) Then
                isValid = False
                udcInputEC.SetECReferenceError(True)
                udcMsgBoxErr.AddMessage(sm)
            End If

        End If

        'English Name
        Me.sm = Me.validator.chkEngName(udcInputEC.ENameSurName, udcInputEC.ENameFirstName, DocTypeModel.DocTypeCode.EC)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputEC.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        ' CRE15-014 HA_MingLiu UTF32 [Start][Winnie]
        'Chinese Name
        Me.sm = Me.validator.chkChiName(udcInputEC.CName)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputEC.SetCNameError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If
        ' CRE15-014 HA_MingLiu UTF32 [End][Winnie]

        'Gender
        Me.sm = Me.validator.chkGender(udcInputEC.Gender)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputEC.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'DOB error message (in sequence)
        If Not IsNothing(sm_DOB) Then
            Me.udcMsgBoxErr.AddMessage(sm_DOB)
        End If
        If Not IsNothing(sm_DOR) Then
            Me.udcMsgBoxErr.AddMessage(sm_DOR)
        End If

        If isValid Then
            'TODO: Lawrence
            'Dim strECDate As String
            'If strECDateDay.Length = 1 Then
            '    strECDate = String.Format("0{0}-{1}-{2}", strECDateDay, strECDateMonth, strECDateYear)
            'Else
            '    strECDate = String.Format("{0}-{1}-{2}", strECDateDay, strECDateMonth, strECDateYear)
            'End If

            udtEHSAccountPersonalInfo.ENameSurName = udcInputEC.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputEC.ENameFirstName
            udtEHSAccountPersonalInfo.CName = udcInputEC.CName
            udtEHSAccountPersonalInfo.Gender = udcInputEC.Gender
            udtEHSAccountPersonalInfo.ECSerialNoNotProvided = udcInputEC.SerialNumberNotProvided
            udtEHSAccountPersonalInfo.ECSerialNo = udcInputEC.SerialNumber
            udtEHSAccountPersonalInfo.ECReferenceNoOtherFormat = udcInputEC.ReferenceOtherFormat
            'udtEHSAccountPersonalInfo.ECReferenceNo = udcInputEC.Reference.Replace("(", String.Empty).Replace(")", String.Empty)
            udtEHSAccountPersonalInfo.ECReferenceNo = udcInputEC.Reference
            udtEHSAccountPersonalInfo.DateofIssue = CDate(Me.udtFormatter.convertDate(strECDate, Common.Component.CultureLanguage.English))
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

        End If

        Return isValid

    End Function

    'HKBC
    Private Function ValidateRectifyDetail_HKBC(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isValid As Boolean = True
        Dim udcInputHKBC As ucInputHKBC = Me.udcRectifyAccount.GetHKBCControl

        udcInputHKBC.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
        udcInputHKBC.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)

        _udtAuditLogEntry.AddDescripton("RegistrationNo", udcInputHKBC.RegistrationNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputHKBC.DOB)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputHKBC.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputHKBC.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("DOBInWord", udcInputHKBC.DOBInWord)
        _udtAuditLogEntry.AddDescripton("DOBInWordCase", udcInputHKBC.DOBInWordCase.ToString())
        _udtAuditLogEntry.AddDescripton("ExactDOB", udcInputHKBC.IsExactDOB)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputHKBC.Gender)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00014, AuditLogDesc.ValidateRectifiedAccount)

        '--HKIC Amended 30 Nov 2009
        Me.sm = Me.validator.chkIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKBC, udcInputHKBC.RegistrationNo, String.Empty)
        If Not IsNothing(sm) Then
            isValid = False
            Me.udcMsgBoxErr.AddMessage(sm)
        End If
        '--

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String = udcInputHKBC.DOB
        Dim dtmDOB As Date

        Select Case udcInputHKBC.DOB.Trim
            Case String.Empty
                sm = New SystemMessage("990000", "E", "00003")
                'DOBInWordCase (By radio button selection)
                ' True  --> exact DOB
                ' False --> Not exact DOB
                If udcInputHKBC.DOBInWordCase Then
                    udcInputHKBC.SetDOBTypeError(True)
                Else
                    udcInputHKBC.SetDOBError(True)
                End If
            Case Else
                Me.sm = Me.validator.chkDOB(DocTypeModel.DocTypeCode.HKBC, strDOB, dtmDOB, strExactDOB)

                If sm Is Nothing Then
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

        If Not IsNothing(sm) Then
            Me.udcMsgBoxErr.AddMessage(sm)
            isValid = False
        End If

        'DOBInWordCase
        If udcInputHKBC.DOBInWordCase Then
            If udcInputHKBC.DOBInWord Is Nothing OrElse udcInputHKBC.DOBInWord = String.Empty Then
                isValid = False
                sm = New SystemMessage("990000", "E", "00160")
                udcInputHKBC.SetDOBTypeError(True)
                Me.udcMsgBoxErr.AddMessage(sm)
            End If
        End If

        'English Name
        Me.sm = Me.validator.chkEngName(udcInputHKBC.ENameSurName, udcInputHKBC.ENameFirstName, DocTypeModel.DocTypeCode.HKBC)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputHKBC.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'Gender
        Me.sm = Me.validator.chkGender(udcInputHKBC.Gender)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputHKBC.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'Me.sm = Me.validator.chkDOB(DocTypeModel.DocTypeCode.HKBC, strDOB, dtmDOB, strExactDOB)
        'If Not IsNothing(sm) Then
        '    isValid = False
        '    Select Case udcInputHKBC.IsExactDOB.Trim
        '        Case "D", "M", "Y"
        '            udcInputHKBC.SetDOBError(True)
        '        Case "T", "U", "V"
        '            udcInputHKBC.SetDOBTypeError(True)
        '    End Select
        '    Me.udcMsgBoxErr.AddMessage(sm)
        'End If

        If isValid Then
            Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKBC)
            udtEHSAccountPersonalInfo.ENameSurName = udcInputHKBC.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputHKBC.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputHKBC.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB 'udcInputHKBC.IsExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB 'CDate(Me.udtFormatter.convertDate(strDOB, Common.Component.CultureLanguage.English))
            udtEHSAccountPersonalInfo.OtherInfo = udcInputHKBC.DOBInWord
        End If


        Return isValid
    End Function

    'Adoption
    Private Function ValidateRectifyDetail_Adopt(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isvalid As Boolean = True

        Dim udcInputAdopt As ucInputAdoption = Me.udcRectifyAccount.GetADOPCControl
        udcInputAdopt.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
        udcInputAdopt.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)

        _udtAuditLogEntry.AddDescripton("NoOfEntry", udcInputAdopt.IdentityNo)
        _udtAuditLogEntry.AddDescripton("Prefix", udcInputAdopt.PerfixNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputAdopt.DOB)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputAdopt.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputAdopt.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputAdopt.Gender)
        _udtAuditLogEntry.AddDescripton("DOBInWord", udcInputAdopt.DOBInWord)
        _udtAuditLogEntry.AddDescripton("DOBInWordCase", udcInputAdopt.DOBInWordCase.ToString())
        _udtAuditLogEntry.AddDescripton("ExactDOB", udcInputAdopt.IsExactDOB)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00014, AuditLogDesc.ValidateRectifiedAccount)

        'English Name
        Me.sm = Me.validator.chkEngName(udcInputAdopt.ENameSurName, udcInputAdopt.ENameFirstName, DocTypeModel.DocTypeCode.ADOPC)
        If Not IsNothing(sm) Then
            isvalid = False
            udcInputAdopt.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'Gender
        Me.sm = Me.validator.chkGender(udcInputAdopt.Gender)
        If Not IsNothing(sm) Then
            isvalid = False
            udcInputAdopt.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String = udcInputAdopt.DOB
        Dim dtmDOB As Date

        Select Case udcInputAdopt.DOB.Trim
            Case String.Empty
                sm = New SystemMessage("990000", "E", "00003")
                'DOBInWordCase (By radio button selection)
                ' True  --> exact DOB
                ' False --> Not exact DOB
                If udcInputAdopt.DOBInWordCase Then
                    udcInputAdopt.SetDOBInWordError(True)
                Else
                    udcInputAdopt.SetDOBError(True)
                End If
            Case Else
                Me.sm = Me.validator.chkDOB(DocTypeModel.DocTypeCode.ADOPC, strDOB, dtmDOB, strExactDOB)

                If sm Is Nothing Then
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

        If Not IsNothing(sm) Then
            Me.udcMsgBoxErr.AddMessage(sm)
            isvalid = False
        End If

        'DOBInWordCase
        If udcInputAdopt.DOBInWordCase Then
            If udcInputAdopt.DOBInWord Is Nothing OrElse udcInputAdopt.DOBInWord = String.Empty Then
                isvalid = False
                sm = New SystemMessage("990000", "E", "00160")
                udcInputAdopt.SetDOBInWordError(True)
                Me.udcMsgBoxErr.AddMessage(sm)
            End If
        End If

        ''DOB
        'Dim strExactDOB As String = String.Empty
        'Dim strDOB As String

        'strDOB = udcInputAdopt.DOB
        'Me.sm = Me.validator.chkDOB_WithDocCode(strDOB, strExactDOB, DocType.DocTypeModel.DocTypeCode.ADOPC)
        'If Not IsNothing(sm) Then
        '    isvalid = False
        '    Select Case udcInputAdopt.IsExactDOB.Trim
        '        Case "D", "M", "Y"
        '            udcInputAdopt.SetDOBError(True)
        '        Case "T", "U", "V"
        '            udcInputAdopt.SetDOBInWordError(True)
        '    End Select
        '    Me.udcMsgBoxErr.AddMessage(sm)
        'End If

        If isvalid Then
            Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.ADOPC)
            udtEHSAccountPersonalInfo.ENameSurName = udcInputAdopt.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputAdopt.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputAdopt.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB 'udcInputAdopt.IsExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB 'CDate(Me.udtFormatter.convertDate(strDOB, Common.Component.CultureLanguage.English))
            udtEHSAccountPersonalInfo.OtherInfo = udcInputAdopt.DOBInWord
        End If

        Return isvalid
    End Function

    'DI
    Private Function ValidateRectifyDetail_DI(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isvalid As Boolean = True
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.DI)

        Dim udcInputDI As ucInputDI = Me.udcRectifyAccount.GetDIControl
        udcInputDI.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
        udcInputDI.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)

        _udtAuditLogEntry.AddDescripton("DocNo", udcInputDI.TravelDocNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputDI.DOB)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputDI.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputDI.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputDI.Gender)
        _udtAuditLogEntry.AddDescripton("DOI", udcInputDI.DateOfIssue)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputDI.DOB)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00014, AuditLogDesc.ValidateRectifiedAccount)

        'English Name
        Me.sm = Me.validator.chkEngName(udcInputDI.ENameSurName, udcInputDI.ENameFirstName, DocType.DocTypeModel.DocTypeCode.DI)
        If Not IsNothing(sm) Then
            isvalid = False
            udcInputDI.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'Gender
        Me.sm = Me.validator.chkGender(udcInputDI.Gender)
        If Not IsNothing(sm) Then
            isvalid = False
            udcInputDI.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String
        Dim dtmDOB As Date

        strDOB = udcInputDI.DOB
        Me.sm = Me.validator.chkDOB(DocType.DocTypeModel.DocTypeCode.DI, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(sm) Then
            isvalid = False
            udcInputDI.SetDOBError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        Else
            udtEHSAccountPersonalInfo.DOB = dtmDOB
        End If

        'DOI
        'skip issue date checking if DOB is empty / Invalid
        'as the checking of DOI relies on the supply of DOB
        Dim strIssueDate As String = Nothing
        Dim dtIssueDate As DateTime
        'If IsNothing(sm) Then
        'strIssueDate = Me.formatter.formatDOI(DocType.DocTypeModel.CertOfException, udcInputDI.DateOfIssue)
        'Me.sm = Me.validator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.DI, udcInputDI.DateOfIssue, dtmDOB)
        Dim strDOI As String = String.Empty
        strDOI = Me.formatter.formatDateBeforValidation_DDMMYYYY(udcInputDI.DateOfIssue)
        sm = Me.validator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.DI, strDOI, udtEHSAccountPersonalInfo.DOB)
        If Not IsNothing(sm) Then
            isvalid = False
            udcInputDI.SetDOIError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        Else
            'dtIssueDate = Me.formatter.convertHKIDIssueDateStringToDate(strIssueDate)
            dtIssueDate = CDate(Me.udtFormatter.convertDate(Me.udtFormatter.formatInputDate(strDOI), Common.Component.CultureLanguage.English))
        End If
        'End If


        If isvalid Then
            'Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.DI)
            udtEHSAccountPersonalInfo.ENameSurName = udcInputDI.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputDI.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputDI.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB
            udtEHSAccountPersonalInfo.DateofIssue = dtIssueDate
        End If

        Return isvalid
    End Function

    'ID235B
    Private Function ValidateRectifyDetail_ID235B(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.ID235B)
        Dim isvalid As Boolean = True
        Dim dtPermit As DateTime

        Dim udcInputID235B As ucInputID235B = Me.udcRectifyAccount.GetID235BControl
        udcInputID235B.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
        udcInputID235B.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)

        _udtAuditLogEntry.AddDescripton("BirthEntryNo", udcInputID235B.BirthEntryNo)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputID235B.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputID235B.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputID235B.Gender)
        _udtAuditLogEntry.AddDescripton("RemainUntil", udcInputID235B.PermitRemain)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputID235B.DateOfBirth)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00014, AuditLogDesc.ValidateRectifiedAccount)

        'English Name
        Me.sm = Me.validator.chkEngName(udcInputID235B.ENameSurName, udcInputID235B.ENameFirstName, DocType.DocTypeModel.DocTypeCode.ID235B)
        If Not IsNothing(sm) Then
            isvalid = False
            udcInputID235B.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'Gender
        Me.sm = Me.validator.chkGender(udcInputID235B.Gender)
        If Not IsNothing(sm) Then
            isvalid = False
            udcInputID235B.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String
        Dim dtmDOB As Date

        strDOB = udcInputID235B.DateOfBirth
        Me.sm = Me.validator.chkDOB(DocType.DocTypeModel.DocTypeCode.ID235B, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(sm) Then
            isvalid = False
            udcInputID235B.SetDOBError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        Else
            udtEHSAccountPersonalInfo.DOB = dtmDOB
        End If

        'Permit to remain until
        'If isvalid Then
        Dim strPermit As String = Nothing
        'strPermit = Me.formatter.formatDateBeforValidation_DDMMYYYY(udcInputID235B.PermitRemain)
        strPermit = Me.formatter.formatDateBeforValidation_DDMMYYYY(udcInputID235B.PermitRemain)
        Me.sm = Me.validator.chkPremitToRemainUntil(strPermit, udtEHSAccountPersonalInfo.DOB, DocType.DocTypeModel.DocTypeCode.ID235B)
        'Me.sm = Me.validator.chkPremitToRemainUntil(strPermit, dtmDOB)
        If Not IsNothing(sm) Then
            isvalid = False
            udcInputID235B.SetPermitRemainError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        Else
            dtPermit = CDate(Me.udtFormatter.convertDate(Me.udtFormatter.formatInputDate(strPermit), Common.Component.CultureLanguage.English))
        End If
        'End If

        If isvalid Then
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
    Private Function ValidateRectifyDetail_ReEntryPermit(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isvalid As Boolean = True
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.REPMT)

        Dim udcInputReentryPermit As ucInputReentryPermit = Me.udcRectifyAccount.GetREPMTControl
        udcInputReentryPermit.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
        udcInputReentryPermit.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)

        _udtAuditLogEntry.AddDescripton("REPMTNo", udcInputReentryPermit.REPMTNo)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputReentryPermit.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputReentryPermit.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputReentryPermit.Gender)
        _udtAuditLogEntry.AddDescripton("DOI", udcInputReentryPermit.DateOfIssue)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputReentryPermit.DateOfBirth)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00014, AuditLogDesc.ValidateRectifiedAccount)

        'English Name
        Me.sm = Me.validator.chkEngName(udcInputReentryPermit.ENameSurName, udcInputReentryPermit.ENameFirstName, DocType.DocTypeModel.DocTypeCode.REPMT)
        If Not IsNothing(sm) Then
            isvalid = False
            udcInputReentryPermit.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'Gender
        Me.sm = Me.validator.chkGender(udcInputReentryPermit.Gender)
        If Not IsNothing(sm) Then
            isvalid = False
            udcInputReentryPermit.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String
        Dim dtmDOB As Date

        strDOB = udcInputReentryPermit.DateOfBirth
        Me.sm = Me.validator.chkDOB(DocType.DocTypeModel.DocTypeCode.REPMT, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(sm) Then
            isvalid = False
            udcInputReentryPermit.SetDOBError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        Else
            udtEHSAccountPersonalInfo.DOB = dtmDOB
        End If

        'DOI
        'skip issue date checking if DOB is empty / Invalid
        'as the checking of DOI relies on the supply of DOB
        Dim strIssueDate As String = Nothing
        Dim dtIssueDate As DateTime
        'If IsNothing(sm) Then
        strIssueDate = Me.formatter.formatDateBeforValidation_DDMMYYYY(udcInputReentryPermit.DateOfIssue)
        sm = Me.validator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.REPMT, strIssueDate, udtEHSAccountPersonalInfo.DOB)
        If Not IsNothing(sm) Then
            isvalid = False
            udcInputReentryPermit.SetDOIError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        Else
            dtIssueDate = CDate(Me.udtFormatter.convertDate(Me.udtFormatter.formatInputDate(strIssueDate), Common.Component.CultureLanguage.English))
        End If
        'End If

        If isvalid Then
            udtEHSAccountPersonalInfo.ENameSurName = udcInputReentryPermit.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputReentryPermit.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputReentryPermit.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB 'CDate(Me.udtFormatter.convertDate(strDOB, Common.Component.CultureLanguage.English))
            udtEHSAccountPersonalInfo.DateofIssue = dtIssueDate
        End If

        Return isvalid
    End Function

    'Visa
    Private Function ValidateRectifyDetail_Visa(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isValid As Boolean = True
        Dim udcInputVisa As ucInputVISA = Me.udcRectifyAccount.GetVISAControl
        udcInputVisa.SetProperty(ucInputDocTypeBase.BuildMode.Creation)
        udcInputVisa.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)

        _udtAuditLogEntry.AddDescripton("VISANo", udcInputVisa.VISANo)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputVisa.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputVisa.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputVisa.Gender)
        _udtAuditLogEntry.AddDescripton("PassportNo", udcInputVisa.PassportNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputVisa.DOB)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00014, AuditLogDesc.ValidateRectifiedAccount)

        'PassportNo
        If udcInputVisa.PassportNo.Equals(String.Empty) Then
            isValid = False
            udcInputVisa.SetPassportNoError(True)
            Me.udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00236"))
        End If

        'English Name
        Me.sm = Me.validator.chkEngName(udcInputVisa.ENameSurName, udcInputVisa.ENameFirstName, DocTypeModel.DocTypeCode.VISA)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputVisa.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'Gender
        Me.sm = Me.validator.chkGender(udcInputVisa.Gender)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputVisa.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String
        Dim dtmDOB As Date

        strDOB = udcInputVisa.DOB
        Me.sm = Me.validator.chkDOB(DocType.DocTypeModel.DocTypeCode.VISA, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputVisa.SetDOBError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If



        If isValid Then
            Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.VISA)
            udtEHSAccountPersonalInfo.ENameSurName = udcInputVisa.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputVisa.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputVisa.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB 'CDate(Me.udtFormatter.convertDate(strDOB, Common.Component.CultureLanguage.English))
            udtEHSAccountPersonalInfo.Foreign_Passport_No = udcInputVisa.PassportNo
        End If


        Return isValid
    End Function

    ' CRE20-003 (Batch Upload) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------

    'OW
    Private Function ValidateRectifyDetail_OW(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isValid As Boolean = True
        Dim sm As SystemMessage = Nothing
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.OW)

        Dim udcInputOW As ucInputOW = Me.udcRectifyAccount.GetOWControl
        udcInputOW.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
        udcInputOW.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)

        _udtAuditLogEntry.AddDescripton("DocumentNo", udcInputOW.DocumentNo)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputOW.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputOW.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputOW.DOB)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputOW.Gender)
        _udtAuditLogEntry.WriteLog(LogID.LOG00014, AuditLogDesc.ValidateRectifiedAccount)

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String = udcInputOW.DOB
        Dim dtmDOB As Date

        sm = Me.validator.chkDOB(DocType.DocTypeModel.DocTypeCode.OW, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(sm) Then
            udcInputOW.SetDOBError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
            isValid = False
        End If

        'English Name
        sm = Me.validator.chkEngName(udcInputOW.ENameSurName, udcInputOW.ENameFirstName, DocType.DocTypeModel.DocTypeCode.OW)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputOW.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'Gender
        sm = Me.validator.chkGender(udcInputOW.Gender)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputOW.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        If isValid Then
            udtEHSAccountPersonalInfo.IdentityNum = udcInputOW.DocumentNo
            udtEHSAccountPersonalInfo.ENameSurName = udcInputOW.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputOW.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputOW.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB

        End If

        Return isValid
    End Function

    'TW
    Private Function ValidateRectifyDetail_TW(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isValid As Boolean = True
        Dim sm As SystemMessage = Nothing
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.TW)

        Dim udcInputTW As ucInputTW = Me.udcRectifyAccount.GetTWControl
        udcInputTW.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
        udcInputTW.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)

        _udtAuditLogEntry.AddDescripton("DocumentNo", udcInputTW.DocumentNo)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputTW.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputTW.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputTW.DOB)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputTW.Gender)
        _udtAuditLogEntry.WriteLog(LogID.LOG00014, AuditLogDesc.ValidateRectifiedAccount)

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String = udcInputTW.DOB
        Dim dtmDOB As Date

        sm = Me.validator.chkDOB(DocType.DocTypeModel.DocTypeCode.TW, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(sm) Then
            udcInputTW.SetDOBError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
            isValid = False
        End If

        'English Name
        sm = Me.validator.chkEngName(udcInputTW.ENameSurName, udcInputTW.ENameFirstName, DocType.DocTypeModel.DocTypeCode.TW)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputTW.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'Gender
        sm = Me.validator.chkGender(udcInputTW.Gender)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputTW.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        If isValid Then
            udtEHSAccountPersonalInfo.IdentityNum = udcInputTW.DocumentNo
            udtEHSAccountPersonalInfo.ENameSurName = udcInputTW.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputTW.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputTW.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB

        End If

        Return isValid
    End Function

    'RFNo8
    Private Function ValidateRectifyDetail_RFNo8(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isValid As Boolean = True
        Dim sm As SystemMessage = Nothing
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.RFNo8)

        Dim udcInputRFNo8 As ucInputRFNo8 = Me.udcRectifyAccount.GetRFNo8Control
        udcInputRFNo8.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
        udcInputRFNo8.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)

        _udtAuditLogEntry.AddDescripton("DocumentNo", udcInputRFNo8.DocumentNo)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputRFNo8.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputRFNo8.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputRFNo8.DOB)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputRFNo8.Gender)
        _udtAuditLogEntry.WriteLog(LogID.LOG00014, AuditLogDesc.ValidateRectifiedAccount)

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String = udcInputRFNo8.DOB
        Dim dtmDOB As Date

        sm = Me.validator.chkDOB(DocType.DocTypeModel.DocTypeCode.RFNo8, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(sm) Then
            udcInputRFNo8.SetDOBError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
            isValid = False
        End If

        'English Name
        sm = Me.validator.chkEngName(udcInputRFNo8.ENameSurName, udcInputRFNo8.ENameFirstName, DocType.DocTypeModel.DocTypeCode.RFNo8)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputRFNo8.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'Gender
        sm = Me.validator.chkGender(udcInputRFNo8.Gender)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputRFNo8.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        If isValid Then
            udtEHSAccountPersonalInfo.IdentityNum = udcInputRFNo8.DocumentNo
            udtEHSAccountPersonalInfo.ENameSurName = udcInputRFNo8.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputRFNo8.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputRFNo8.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB

        End If

        Return isValid
    End Function

    'Other
    Private Function ValidateRectifyDetail_OTHER(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isValid As Boolean = True
        Dim sm As SystemMessage = Nothing
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.OTHER)

        Dim udcInputOTHER As ucInputOTHER = Me.udcRectifyAccount.GetOTHERControl
        udcInputOTHER.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
        udcInputOTHER.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)

        _udtAuditLogEntry.AddDescripton("DocumentNo", udcInputOTHER.DocumentNo)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputOTHER.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputOTHER.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputOTHER.DOB)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputOTHER.Gender)
        _udtAuditLogEntry.WriteLog(LogID.LOG00014, AuditLogDesc.ValidateRectifiedAccount)

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String = udcInputOTHER.DOB
        Dim dtmDOB As Date

        sm = Me.validator.chkDOB(DocType.DocTypeModel.DocTypeCode.OTHER, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(sm) Then
            udcInputOTHER.SetDOBError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
            isValid = False
        End If

        'English Name
        sm = Me.validator.chkEngName(udcInputOTHER.ENameSurName, udcInputOTHER.ENameFirstName, DocType.DocTypeModel.DocTypeCode.OTHER)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputOTHER.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'Gender
        sm = Me.validator.chkGender(udcInputOTHER.Gender)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputOTHER.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        If isValid Then
            udtEHSAccountPersonalInfo.IdentityNum = udcInputOTHER.DocumentNo
            udtEHSAccountPersonalInfo.ENameSurName = udcInputOTHER.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputOTHER.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputOTHER.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB

        End If

        Return isValid
    End Function

    ' CRE20-003 (Batch Upload) [End][Chris YIM]


    'CCIC
    Private Function ValidateRectifyDetail_CCIC(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isvalid As Boolean = True
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.CCIC)

        Dim udcInputCCIC As ucInputCCIC = Me.udcRectifyAccount.GetCCICControl
        udcInputCCIC.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
        udcInputCCIC.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)

        _udtAuditLogEntry.AddDescripton("DocNo", udcInputCCIC.TravelDocNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputCCIC.DOB)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputCCIC.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputCCIC.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputCCIC.Gender)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputCCIC.DOB)
        _udtAuditLogEntry.AddDescripton("DOI", udcInputCCIC.DateOfIssue)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00014, AuditLogDesc.ValidateRectifiedAccount)

        'English Name
        Me.sm = Me.validator.chkEngName(udcInputCCIC.ENameSurName, udcInputCCIC.ENameFirstName, DocType.DocTypeModel.DocTypeCode.CCIC)
        If Not IsNothing(sm) Then
            isvalid = False
            udcInputCCIC.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'Gender
        Me.sm = Me.validator.chkGender(udcInputCCIC.Gender)
        If Not IsNothing(sm) Then
            isvalid = False
            udcInputCCIC.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String
        Dim dtmDOB As Date

        strDOB = udcInputCCIC.DOB
        Me.sm = Me.validator.chkDOB(DocType.DocTypeModel.DocTypeCode.CCIC, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(sm) Then
            isvalid = False
            udcInputCCIC.SetDOBError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        Else
            udtEHSAccountPersonalInfo.DOB = dtmDOB
        End If

        'DOI
        Dim strHKIDIssueDate As String = Nothing
        Dim dtHKIDIssueDate As DateTime
        strHKIDIssueDate = Me.formatter.formatHKIDIssueDateBeforeValidate(udcInputCCIC.DateOfIssue)
        Me.sm = Me.validator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.CCIC, strHKIDIssueDate, udtEHSAccountPersonalInfo.DOB)
        If Not Me.sm Is Nothing Then
            isvalid = False
            udcInputCCIC.SetDOIError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        Else
            dtHKIDIssueDate = Me.formatter.convertHKIDIssueDateStringToDate(strHKIDIssueDate)
        End If

        If isvalid Then
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
    Private Function ValidateRectifyDetail_ROP140(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isvalid As Boolean = True
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.ROP140)

        Dim udcInputCROP140 As ucInputROP140 = Me.udcRectifyAccount.GetROP140Control
        udcInputCROP140.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
        udcInputCROP140.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)

        _udtAuditLogEntry.AddDescripton("DocNo", udcInputCROP140.TravelDocNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputCROP140.DOB)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputCROP140.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputCROP140.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("ChiName", udcInputCROP140.CName)
        _udtAuditLogEntry.AddDescripton("CCCode1", udcInputCROP140.CCCode1)
        _udtAuditLogEntry.AddDescripton("CCCode2", udcInputCROP140.CCCode2)
        _udtAuditLogEntry.AddDescripton("CCCode3", udcInputCROP140.CCCode3)
        _udtAuditLogEntry.AddDescripton("CCCode4", udcInputCROP140.CCCode4)
        _udtAuditLogEntry.AddDescripton("CCCode5", udcInputCROP140.CCCode5)
        _udtAuditLogEntry.AddDescripton("CCCode6", udcInputCROP140.CCCode6)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputCROP140.Gender)
        _udtAuditLogEntry.AddDescripton("DOI", udcInputCROP140.DateOfIssue)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00014, AuditLogDesc.ValidateRectifiedAccount)

        'English Name
        Me.sm = Me.validator.chkEngName(udcInputCROP140.ENameSurName, udcInputCROP140.ENameFirstName, DocType.DocTypeModel.DocTypeCode.ROP140)
        If Not IsNothing(sm) Then
            isvalid = False
            udcInputCROP140.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'CCCode
        Me.sm = Me.validator.chkCCCode_UsingDDL(String.Format("{0}{1}", udcInputCROP140.CCCode1, Me.udcCCCode.SelectedCCCodeTail1), _
                                                String.Format("{0}{1}", udcInputCROP140.CCCode2, Me.udcCCCode.SelectedCCCodeTail2), _
                                                String.Format("{0}{1}", udcInputCROP140.CCCode3, Me.udcCCCode.SelectedCCCodeTail3), _
                                                String.Format("{0}{1}", udcInputCROP140.CCCode4, Me.udcCCCode.SelectedCCCodeTail4), _
                                                String.Format("{0}{1}", udcInputCROP140.CCCode5, Me.udcCCCode.SelectedCCCodeTail5), _
                                                String.Format("{0}{1}", udcInputCROP140.CCCode6, Me.udcCCCode.SelectedCCCodeTail6))
        If Not Me.sm Is Nothing Then
            isvalid = False
            udcInputCROP140.SetCCCodeError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'Gender
        Me.sm = Me.validator.chkGender(udcInputCROP140.Gender)
        If Not IsNothing(sm) Then
            isvalid = False
            udcInputCROP140.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String
        Dim dtmDOB As Date

        strDOB = udcInputCROP140.DOB
        Me.sm = Me.validator.chkDOB(DocType.DocTypeModel.DocTypeCode.ROP140, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(sm) Then
            isvalid = False
            udcInputCROP140.SetDOBError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        Else
            udtEHSAccountPersonalInfo.DOB = dtmDOB
        End If

        'DOI        
        Dim strIssueDate As String = Nothing
        Dim dtIssueDate As DateTime

        Dim strDOI As String = String.Empty
        strDOI = Me.formatter.formatDateBeforValidation_DDMMYYYY(udcInputCROP140.DateOfIssue)
        sm = Me.validator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.DI, strDOI, udtEHSAccountPersonalInfo.DOB)
        If Not IsNothing(sm) Then
            isvalid = False
            udcInputCROP140.SetDOIError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        Else
            dtIssueDate = CDate(Me.udtFormatter.convertDate(Me.udtFormatter.formatInputDate(strDOI), Common.Component.CultureLanguage.English))
        End If


        If isvalid Then
            udtEHSAccountPersonalInfo.ENameSurName = udcInputCROP140.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputCROP140.ENameFirstName

            udtEHSAccountPersonalInfo.CCCode1 = String.Format("{0}{1}", udcInputCROP140.CCCode1, Me.udcCCCode.SelectedCCCodeTail1)
            udtEHSAccountPersonalInfo.CCCode2 = String.Format("{0}{1}", udcInputCROP140.CCCode2, Me.udcCCCode.SelectedCCCodeTail2)
            udtEHSAccountPersonalInfo.CCCode3 = String.Format("{0}{1}", udcInputCROP140.CCCode3, Me.udcCCCode.SelectedCCCodeTail3)
            udtEHSAccountPersonalInfo.CCCode4 = String.Format("{0}{1}", udcInputCROP140.CCCode4, Me.udcCCCode.SelectedCCCodeTail4)
            udtEHSAccountPersonalInfo.CCCode5 = String.Format("{0}{1}", udcInputCROP140.CCCode5, Me.udcCCCode.SelectedCCCodeTail5)
            udtEHSAccountPersonalInfo.CCCode6 = String.Format("{0}{1}", udcInputCROP140.CCCode6, Me.udcCCCode.SelectedCCCodeTail6)

            'Retervie Chinese Name from Choose
            udcInputCROP140.CCCode1 = udtEHSAccountPersonalInfo.CCCode1
            udcInputCROP140.CCCode2 = udtEHSAccountPersonalInfo.CCCode2
            udcInputCROP140.CCCode3 = udtEHSAccountPersonalInfo.CCCode3
            udcInputCROP140.CCCode4 = udtEHSAccountPersonalInfo.CCCode4
            udcInputCROP140.CCCode5 = udtEHSAccountPersonalInfo.CCCode5
            udcInputCROP140.CCCode6 = udtEHSAccountPersonalInfo.CCCode6
            udcInputCROP140.SetCName()
            udtEHSAccountPersonalInfo.CName = udcInputCROP140.CName

            udtEHSAccountPersonalInfo.Gender = udcInputCROP140.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB
            udtEHSAccountPersonalInfo.DateofIssue = dtIssueDate
        End If

        Return isvalid

    End Function


    'PASS
    Private Function ValidateRectifyDetail_PASS(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isvalid As Boolean = True
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.PASS)

        Dim udcInputPASS As ucInputPASS = Me.udcRectifyAccount.GetPASSControl
        udcInputPASS.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
        udcInputPASS.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)

        _udtAuditLogEntry.AddDescripton("DocNo", udcInputPASS.TravelDocNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputPASS.DOB)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputPASS.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputPASS.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputPASS.Gender)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputPASS.DOB)
        _udtAuditLogEntry.AddDescripton("PassportIssueRegion", udcInputPASS.PassportIssueRegion)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00014, AuditLogDesc.ValidateRectifiedAccount)

        'English Name
        Me.sm = Me.validator.chkEngName(udcInputPASS.ENameSurName, udcInputPASS.ENameFirstName, DocType.DocTypeModel.DocTypeCode.PASS)
        If Not IsNothing(sm) Then
            isvalid = False
            udcInputPASS.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'Gender
        Me.sm = Me.validator.chkGender(udcInputPASS.Gender)
        If Not IsNothing(sm) Then
            isvalid = False
            udcInputPASS.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String
        Dim dtmDOB As Date

        strDOB = udcInputPASS.DOB
        Me.sm = Me.validator.chkDOB(DocType.DocTypeModel.DocTypeCode.PASS, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(sm) Then
            isvalid = False
            udcInputPASS.SetDOBError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        Else
            udtEHSAccountPersonalInfo.DOB = dtmDOB
        End If


        ' CRE20-023 Add Issue country/region to passport document [Start][Raiman]
        'Add Passport checking
        If udcInputPASS.PassportIssueRegion.Equals(String.Empty) Then
            Me.sm = New Common.ComObject.SystemMessage("990000", "E", "00462")
            isvalid = False
            udcInputPASS.SetPassportIssueRegionError(True)
            Me.udcMsgBoxErr.AddMessage(Me.sm, _
                                     New String() {"%en", "%tc", "%sc"}, _
                                     New String() {HttpContext.GetGlobalResourceObject("Text", "PassportIssueRegion", New System.Globalization.CultureInfo(CultureLanguage.English)), _
                                                   HttpContext.GetGlobalResourceObject("Text", "PassportIssueRegion", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)), _
                                                   HttpContext.GetGlobalResourceObject("Text", "PassportIssueRegion", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese)) _
                                                   })
        End If
        ' CRE20-023 Add Issue country/region to passport document [End][Raiman]

        If isvalid Then
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

    Private Function ValidateRectifyDetail_ISSHK(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry, ByVal strDocTypeCode As String) As Boolean
        Dim isvalid As Boolean = True
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(strDocTypeCode)

        Dim udcInputISSHK As ucInputISSHK = Me.udcRectifyAccount.GetISSHKControl
        udcInputISSHK.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
        udcInputISSHK.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)

        _udtAuditLogEntry.AddDescripton("DocNo", udcInputISSHK.TravelDocNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputISSHK.DOB)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputISSHK.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputISSHK.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputISSHK.Gender)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputISSHK.DOB)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00014, AuditLogDesc.ValidateRectifiedAccount)

        'English Name
        Me.sm = Me.validator.chkEngName(udcInputISSHK.ENameSurName, udcInputISSHK.ENameFirstName, strDocTypeCode)
        If Not IsNothing(sm) Then
            isvalid = False
            udcInputISSHK.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'Gender
        Me.sm = Me.validator.chkGender(udcInputISSHK.Gender)
        If Not IsNothing(sm) Then
            isvalid = False
            udcInputISSHK.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String
        Dim dtmDOB As Date

        strDOB = udcInputISSHK.DOB
        Me.sm = Me.validator.chkDOB(strDocTypeCode, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(sm) Then
            isvalid = False
            udcInputISSHK.SetDOBError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        Else
            udtEHSAccountPersonalInfo.DOB = dtmDOB
        End If


        If isvalid Then
            udtEHSAccountPersonalInfo.ENameSurName = udcInputISSHK.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputISSHK.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputISSHK.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB
        End If

        Return isvalid

    End Function


    'Common
    Private Function ValidateRectifyDetail_Common(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry, ByVal strDocTypeCode As String) As Boolean
        Dim isValid As Boolean = True
        Dim sm As SystemMessage = Nothing
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(strDocTypeCode)

        Dim udcInputCommon As ucInputCommon = Me.udcRectifyAccount.GetCommonControl
        udcInputCommon.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
        udcInputCommon.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)

        _udtAuditLogEntry.AddDescripton("DocumentNo", udcInputCommon.DocumentNo)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputCommon.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputCommon.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputCommon.DOB)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputCommon.Gender)
        _udtAuditLogEntry.WriteLog(LogID.LOG00014, AuditLogDesc.ValidateRectifiedAccount)

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String = udcInputCommon.DOB
        Dim dtmDOB As Date

        sm = Me.validator.chkDOB(strDocTypeCode, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(sm) Then
            udcInputCommon.SetDOBError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
            isValid = False
        End If

        'English Name
        sm = Me.validator.chkEngName(udcInputCommon.ENameSurName, udcInputCommon.ENameFirstName, strDocTypeCode)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputCommon.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        'Gender
        sm = Me.validator.chkGender(udcInputCommon.Gender)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputCommon.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(sm)
        End If

        If isValid Then
            udtEHSAccountPersonalInfo.IdentityNum = udcInputCommon.DocumentNo
            udtEHSAccountPersonalInfo.ENameSurName = udcInputCommon.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputCommon.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputCommon.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB

        End If

        Return isValid
    End Function
  




#End Region

#Region "Confirm Details"

    Private Sub SetupConfirmRectifyAcc(ByVal _udtEHSAccount As EHSAccountModel)
        If Not IsNothing(_udtEHSAccount) Then
            Me.udcConfirmAccount.DocumentType = Me.txtDocCode.Text.Trim '_udtEHSAccount.EHSPersonalInformationList(0).DocCode.Trim
            Me.udcConfirmAccount.EHSAccount = _udtEHSAccount
            Me.udcConfirmAccount.Vertical = True
            Me.udcConfirmAccount.MaskIdentityNo = False
            Me.udcConfirmAccount.ShowAccountRefNo = False
            Me.udcConfirmAccount.ShowTempAccountNotice = False
            Me.udcConfirmAccount.ShowAccountCreationDate = False
            Me.udcConfirmAccount.Mode = ucInputDocTypeBase.BuildMode.Modification
            Me.udcConfirmAccount.TableTitleWidth = 200
            Me.udcConfirmAccount.Built()
        End If
    End Sub

#End Region

#Region "eHealth Account"

    Private Function BindPersonalInfo(ByVal _udtEHSAccount As EHSAccountModel, ByVal activeViewChanged As Boolean) As Boolean
        Dim blnRes As Boolean = False

        If Not IsNothing(_udtEHSAccount) Then
            Me.udcRectifyAccount.DocType = Me.txtDocCode.Text.Trim '_udtEHSAccount.EHSPersonalInformationList(0).DocCode.Trim
            Me.udcRectifyAccount.EHSAccount = _udtEHSAccount
            Me.udcRectifyAccount.ActiveViewChanged = activeViewChanged
            If IsNothing(Session(SESS_InputMode)) Then
                Me.udcRectifyAccount.Mode = ucInputDocTypeBase.BuildMode.Modification
            Else
                Dim mode As ucInputDocTypeBase.BuildMode
                mode = CType(Session(SESS_InputMode), ucInputDocTypeBase.BuildMode)
                Me.udcRectifyAccount.Mode = mode
            End If

            Me.udcRectifyAccount.FillValue = True

            ' CRE16-012 Removal of DOB InWord [Start][Winnie]
            Dim udtOrgEHSAccount As EHSAccountModel = CType(Me.Session(SESS_OrgEHSAccount), EHSAccountModel)
            Me.udcRectifyAccount.OrgEHSAccount = udtOrgEHSAccount
            ' CRE16-012 Removal of DOB InWord [End][Winnie]

            Me.udcRectifyAccount.AuditLogEntry = New AuditLogEntry(FuncCode, Me)
            Me.udcRectifyAccount.Built()
            blnRes = True
        End If

        Dim blnSetCCCode As Boolean

        If blnRes Then

            If Not IsNothing(Session(SESS_DefaultSetCCCode)) Then
                blnSetCCCode = CBool(Session(SESS_DefaultSetCCCode))

                If blnSetCCCode Then
                    If udcRectifyAccount.DocType.Trim.Equals(DocType.DocTypeModel.DocTypeCode.HKIC) Then
                        Dim udcInputHKID As ucInputHKID = Me.udcRectifyAccount.GetHKICControl

                        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKIC)
                        Me.BuildCCCode(udtEHSAccountPersonalInfo.CCCode1, _
                                      udtEHSAccountPersonalInfo.CCCode2, _
                                      udtEHSAccountPersonalInfo.CCCode3, _
                                      udtEHSAccountPersonalInfo.CCCode4, _
                                      udtEHSAccountPersonalInfo.CCCode5, _
                                      udtEHSAccountPersonalInfo.CCCode6)

                        Me.udcCCCode.GetChineseName(FuncCode, True)

                        Session(SESS_DefaultSetCCCode) = Nothing
                        Session.Remove(SESS_DefaultSetCCCode)
                    End If

                    If udcRectifyAccount.DocType.Trim.Equals(DocType.DocTypeModel.DocTypeCode.ROP140) Then
                        Dim udcInputROP140 As ucInputROP140 = Me.udcRectifyAccount.GetROP140Control

                        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.ROP140)
                        Me.BuildCCCode(udtEHSAccountPersonalInfo.CCCode1, _
                                      udtEHSAccountPersonalInfo.CCCode2, _
                                      udtEHSAccountPersonalInfo.CCCode3, _
                                      udtEHSAccountPersonalInfo.CCCode4, _
                                      udtEHSAccountPersonalInfo.CCCode5, _
                                      udtEHSAccountPersonalInfo.CCCode6)

                        Me.udcCCCode.GetChineseName(FuncCode, True)

                        Session(SESS_DefaultSetCCCode) = Nothing
                        Session.Remove(SESS_DefaultSetCCCode)
                    End If
                End If
            End If
        End If
        Return blnRes

    End Function

    Private Overloads Function GeteHSAccount(ByVal strDocCode As String, ByVal strAccountID As String, ByVal strSource As String) As Boolean
        Dim blnRes As Boolean = False
        udtSessionHandler.EHSAccountRemoveFromSession(FuncCode)
        Me.Session.Remove(SESS_OrgEHSAccount)

        Select Case strSource
            Case EHSAccountModel.SysAccountSourceClass.TemporaryAccount
                udtEHSAccount = udtEHSAccountBLL.LoadTempEHSAccountByVRID(strAccountID)

            Case EHSAccountModel.SysAccountSourceClass.SpecialAccount
                udtEHSAccount = udtEHSAccountBLL.LoadSpecialEHSAccountByVRID(strAccountID)
        End Select

        If Not IsNothing(udtEHSAccount) Then
            Me.udtSessionHandler.EHSAccountSaveToSession(udtEHSAccount, FuncCode)

            'CRE13-006 HCVS Ceiling [Start][Karl]
            Me.Session(SESS_OrgEHSAccount) = udtEHSAccount
            'CRE13-006 HCVS Ceiling [End][Karl]

            blnRes = True
        End If
        Return blnRes
    End Function

#End Region

    '==================================================================== Code for SmartID ============================================================================
#Region "SmartID Confirmation Page"

    Private Sub SetupConfirmSmarIDRectify(ByVal _udtEHSAccount As EHSAccountModel, ByVal activeViewChanged As Boolean)
        Dim udcSmartIDContent As BLL.SmartIDContentModel = Me.udtSessionHandler.SmartIDContentGetFormSession(FuncCode)

        Me.udcSmartIDConfirmationInputDocumentType.EHSAccount = udtEHSAccount
        Me.udcSmartIDConfirmationInputDocumentType.DocType = DocType.DocTypeModel.DocTypeCode.HKIC
        Me.udcSmartIDConfirmationInputDocumentType.Mode = ucInputDocTypeBase.BuildMode.Modification
        Me.udcSmartIDConfirmationInputDocumentType.FillValue = True
        Me.udcSmartIDConfirmationInputDocumentType.ActiveViewChanged = activeViewChanged
        Me.udcSmartIDConfirmationInputDocumentType.SchemeClaim = Me.udtSessionHandler.SchemeSelectedGetFromSession(FuncCode)
        Me.udcSmartIDConfirmationInputDocumentType.SmartIDContent = Me.udtSessionHandler.SmartIDContentGetFormSession(FuncCode)
        Me.udcSmartIDConfirmationInputDocumentType.Built()

        If String.IsNullOrEmpty(udcSmartIDContent.EHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).Gender) Then
            Me.chkSmartIDConfirmationConsent.Enabled = False
            Me.chkSmartIDConfirmationConsent.Checked = False
            Me.EnableConfirmButton(False, Me.ibtnSmartIDConfirmationConfirm)

            If Not IsNothing(Session(SESS_ModifyAcc)) Then
                If CBool(Session(SESS_ModifyAcc)) = True Then
                    panSmartIDConfirmationConsent.Visible = True
                Else
                    panSmartIDConfirmationConsent.Visible = False
                End If
            Else
                panSmartIDConfirmationConsent.Visible = False
            End If
        Else

            If Not IsNothing(Session(SESS_ModifyAcc)) Then
                If CBool(Session(SESS_ModifyAcc)) = True Then
                    panSmartIDConfirmationConsent.Visible = True
                    Me.EnableConfirmButton(Me.chkSmartIDConfirmationConsent.Checked, Me.ibtnSmartIDConfirmationConfirm)
                Else
                    panSmartIDConfirmationConsent.Visible = False
                    Me.EnableConfirmButton(True, Me.ibtnSmartIDConfirmationConfirm)
                End If
            Else
                panSmartIDConfirmationConsent.Visible = False
                Me.EnableConfirmButton(True, Me.ibtnSmartIDConfirmationConfirm)
            End If

        End If

    End Sub

#End Region
    '==================================================================================================================================================================

#Region "CCCode"

    Private Sub udcRectifyAccount_SelectChineseName_HKIC(ByVal udcInputDocumentType As ucInputDocTypeBase, ByVal strDocCode As String, ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcRectifyAccount.SelectChineseName_HKIC
        Me.udtEHSAccount = Me.udtSessionHandler.EHSAccountGetFromSession(FuncCode)

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", Me.udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00035, AuditLogDesc.ClickSelectChineseNameButton)

        Dim sm As SystemMessage

        Me.Session.Remove(SESS_ClickSave)

        'Sender = Nothing => User Click "Save" Btn to fire this event
        If IsNothing(sender) Then
            Session(SESS_ClickSave) = True
        End If

        Select Case strDocCode
            Case DocTypeModel.DocTypeCode.HKIC
                Dim udcInputHKID As ucInputHKID = CType(udcInputDocumentType, ucInputHKID)

                udcInputHKID.SetProperty(ucInputDocTypeBase.BuildMode.Modification)

                If udcInputHKID.CCCodeIsEmptyModification Then

                    'No CCCode
                    udcInputHKID.SetCNameModification(String.Empty)

                    sm = New SystemMessage(CommonFuncCode, Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00143)
                    Me.udcMsgBoxErr.AddMessage(sm)
                    udcInputHKID.SetCCCodeModificationError(True)

                Else
                    Me.udcCCCode.DocCode = DocTypeModel.DocTypeCode.HKIC

                    Me.udcCCCode.CCCode1 = udcInputHKID.GetCCCode(udcInputHKID.CCCode1, Me.udcCCCode.getCCCodeFromSession(1, FuncCode))
                    Me.udcCCCode.CCCode2 = udcInputHKID.GetCCCode(udcInputHKID.CCCode2, Me.udcCCCode.getCCCodeFromSession(2, FuncCode))
                    Me.udcCCCode.CCCode3 = udcInputHKID.GetCCCode(udcInputHKID.CCCode3, Me.udcCCCode.getCCCodeFromSession(3, FuncCode))
                    Me.udcCCCode.CCCode4 = udcInputHKID.GetCCCode(udcInputHKID.CCCode4, Me.udcCCCode.getCCCodeFromSession(4, FuncCode))
                    Me.udcCCCode.CCCode5 = udcInputHKID.GetCCCode(udcInputHKID.CCCode5, Me.udcCCCode.getCCCodeFromSession(5, FuncCode))
                    Me.udcCCCode.CCCode6 = udcInputHKID.GetCCCode(udcInputHKID.CCCode6, Me.udcCCCode.getCCCodeFromSession(6, FuncCode))

                    Me.udcCCCode.RowDisplayStyle = ChooseCCCode.DisplayStyle.SingalRow

                    sm = Me.udcCCCode.BindCCCode()
                    'Bind CCCode Drop Down List
                    If sm Is Nothing Then
                        udcInputHKID.SetCCCodeModificationError(False)
                        Me.ModalPopupExtenderChooseCCCode.Show()

                        Me.udtAuditLogEntry.AddDescripton("AccountID", Me.udtEHSAccount.VoucherAccID.Trim)
                        Me.udtAuditLogEntry.AddDescripton("CCCode1", Me.udcCCCode.CCCode1)
                        Me.udtAuditLogEntry.AddDescripton("CCCode2", Me.udcCCCode.CCCode2)
                        Me.udtAuditLogEntry.AddDescripton("CCCode3", Me.udcCCCode.CCCode3)
                        Me.udtAuditLogEntry.AddDescripton("CCCode4", Me.udcCCCode.CCCode4)
                        Me.udtAuditLogEntry.AddDescripton("CCCode5", Me.udcCCCode.CCCode5)
                        Me.udtAuditLogEntry.AddDescripton("CCCode6", Me.udcCCCode.CCCode6)
                        Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00036, AuditLogDesc.ChineseNameCodeCheckingSuccess)
                    Else
                        Me.udcMsgBoxErr.AddMessage(sm)
                        udcInputHKID.SetCCCodeModificationError(True)
                    End If
                End If

            Case DocTypeModel.DocTypeCode.ROP140
                Dim udcInputROP140 As ucInputROP140 = CType(udcInputDocumentType, ucInputROP140)

                udcInputROP140.SetProperty(ucInputDocTypeBase.BuildMode.Modification)

                If udcInputROP140.CCCodeIsEmpty Then

                    'No CCCode
                    udcInputROP140.SetCName(String.Empty)

                    sm = New SystemMessage(CommonFuncCode, Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00143)
                    Me.udcMsgBoxErr.AddMessage(sm)
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

                    sm = Me.udcCCCode.BindCCCode()
                    'Bind CCCode Drop Down List
                    If sm Is Nothing Then
                        udcInputROP140.SetCCCodeError(False)
                        Me.ModalPopupExtenderChooseCCCode.Show()

                        Me.udtAuditLogEntry.AddDescripton("AccountID", Me.udtEHSAccount.VoucherAccID.Trim)
                        Me.udtAuditLogEntry.AddDescripton("CCCode1", Me.udcCCCode.CCCode1)
                        Me.udtAuditLogEntry.AddDescripton("CCCode2", Me.udcCCCode.CCCode2)
                        Me.udtAuditLogEntry.AddDescripton("CCCode3", Me.udcCCCode.CCCode3)
                        Me.udtAuditLogEntry.AddDescripton("CCCode4", Me.udcCCCode.CCCode4)
                        Me.udtAuditLogEntry.AddDescripton("CCCode5", Me.udcCCCode.CCCode5)
                        Me.udtAuditLogEntry.AddDescripton("CCCode6", Me.udcCCCode.CCCode6)
                        Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00036, AuditLogDesc.ChineseNameCodeCheckingSuccess)
                    Else
                        Me.udcMsgBoxErr.AddMessage(sm)
                        udcInputROP140.SetCCCodeError(True)
                    End If
                End If

        End Select

        Me.udtAuditLogEntry.AddDescripton("AccountID", Me.udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("CCCode1", Me.udcCCCode.CCCode1)
        Me.udtAuditLogEntry.AddDescripton("CCCode2", Me.udcCCCode.CCCode2)
        Me.udtAuditLogEntry.AddDescripton("CCCode3", Me.udcCCCode.CCCode3)
        Me.udtAuditLogEntry.AddDescripton("CCCode4", Me.udcCCCode.CCCode4)
        Me.udtAuditLogEntry.AddDescripton("CCCode5", Me.udcCCCode.CCCode5)
        Me.udtAuditLogEntry.AddDescripton("CCCode6", Me.udcCCCode.CCCode6)
        Me.udcMsgBoxErr.BuildMessageBox(strValidationFail, udtAuditLogEntry, LogID.LOG00037, AuditLogDesc.ChineseNameCodeCheckingFail)

    End Sub

    Private Function NeedPopupCCCodeDialog(ByVal strDocCode As String) As Boolean
        'isDiff is using for check the sessoion CCCode is same as current CCCode 
        'isDiff = true : sessoion CCCode <> current CCCode 
        Dim isDiff As Boolean = True

        Select Case strDocCode
            Case DocTypeModel.DocTypeCode.HKIC
                Dim udcInputHKIC As ucInputHKID = Me.udcRectifyAccount.GetHKICControl()

                udcInputHKIC.SetProperty(ucInputDocTypeBase.BuildMode.Modification)

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
                Dim udcInputROP140 As ucInputROP140 = Me.udcRectifyAccount.GetROP140Control()

                udcInputROP140.SetProperty(ucInputDocTypeBase.BuildMode.Modification)

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
        Me.udtEHSAccount = Me.udtSessionHandler.EHSAccountGetFromSession(FuncCode)

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", Me.udtEHSAccount.VoucherAccID.Trim)

        ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [Start][Winnie] Step 3
        'Dim strCName As String
        'strCName = Me.udcCCCode.GetChineseName(FuncCode, True)
        'Me.udtAuditLogEntry.AddDescripton("ChineseName", strCName)
        ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [End][Winnie] Step 3

        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00039, AuditLogDesc.CancelChineseName)

        Me.ModalPopupExtenderChooseCCCode.Hide()
    End Sub

    Private Sub udcChooseCCCode_Confirm(ByVal strDocCode As String, ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcCCCode.Confirm
        Dim _udtEHSAccount As EHSAccountModel = Me.udtSessionHandler.EHSAccountGetFromSession(FuncCode)
        Dim strCName As String = String.Empty

        Select Case strDocCode
            Case DocTypeModel.DocTypeCode.HKIC
                Dim udcIputHKIC As ucInputHKID = Me.udcRectifyAccount.GetHKICControl

                udcIputHKIC.SetProperty(ucInputDocTypeBase.BuildMode.Modification)

                Me.udcCCCode.CCCode1 = udcIputHKIC.CCCode1
                Me.udcCCCode.CCCode2 = udcIputHKIC.CCCode2
                Me.udcCCCode.CCCode3 = udcIputHKIC.CCCode3
                Me.udcCCCode.CCCode4 = udcIputHKIC.CCCode4
                Me.udcCCCode.CCCode5 = udcIputHKIC.CCCode5
                Me.udcCCCode.CCCode6 = udcIputHKIC.CCCode6

                'Get Chinese Name from Drop Down List, Save to Session
                udcCCCode.CleanSeesion(FuncCode)
                strCName = Me.udcCCCode.GetChineseName(FuncCode, True)
                udcIputHKIC.SetCNameModification(strCName)

                _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKIC).CName = strCName

            Case DocTypeModel.DocTypeCode.ROP140
                Dim udcIputROP140 As ucInputROP140 = Me.udcRectifyAccount.GetROP140Control

                udcIputROP140.SetProperty(ucInputDocTypeBase.BuildMode.Modification)

                Me.udcCCCode.CCCode1 = udcIputROP140.CCCode1
                Me.udcCCCode.CCCode2 = udcIputROP140.CCCode2
                Me.udcCCCode.CCCode3 = udcIputROP140.CCCode3
                Me.udcCCCode.CCCode4 = udcIputROP140.CCCode4
                Me.udcCCCode.CCCode5 = udcIputROP140.CCCode5
                Me.udcCCCode.CCCode6 = udcIputROP140.CCCode6

                'Get Chinese Name from Drop Down List, Save to Session
                udcCCCode.CleanSeesion(FuncCode)
                strCName = Me.udcCCCode.GetChineseName(FuncCode, True)
                udcIputROP140.SetCName(strCName)

                _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.ROP140).CName = strCName

        End Select

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", _udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("ChineseName", strCName)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00038, AuditLogDesc.ConfirmChineseName)

        Me.udtSessionHandler.EHSAccountSaveToSession(_udtEHSAccount, FuncCode)

        Me.ModalPopupExtenderChooseCCCode.Hide()

        Dim blnClickSave As Boolean = False
        If Not IsNothing(Session(SESS_ClickSave)) Then
            ' CCCode incorrect & user had clicked "Save" btn in Rectify Account
            blnClickSave = CBool(Session(SESS_ClickSave))
            If blnClickSave Then
                Session(SESS_ClickSave) = Nothing
                Me.Session.Remove(SESS_ClickSave)
                Me.ibtnRectifyAccountSave_Click(Nothing, Nothing)
            End If

        End If

    End Sub

#End Region

#Region "Support Function"

    Private Sub RenderLanguage()

        Dim strSelectedLanguage As String
        strSelectedLanguage = LCase(udtSessionHandler.Language())

        Dim strSelectedStatus As String = Me.ddlAcctStatus.SelectedValue.Trim

        'Rebind the Dropdownlist in search critria
        ddlAcctStatus.Items.Clear()

        Dim dt As DataTable = Status.GetDescriptionListFromDBEnumCode("HCSPeHSAccRectificationStatus", True)
        ddlAcctStatus.DataSource = dt
        ddlAcctStatus.DataValueField = "Status_Value"
        If LCase(strSelectedLanguage).Equals(TradChinese) Then
            ddlAcctStatus.DataTextField = "Status_Description_Chi"
        ElseIf LCase(strSelectedLanguage).Equals(SimpChinese) Then
            ddlAcctStatus.DataTextField = "Status_Description_CN"
        Else
            ddlAcctStatus.DataTextField = "Status_Description"
        End If

        ddlAcctStatus.DataBind()
        ddlAcctStatus.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))

        ddlAcctStatus.SelectedValue = strSelectedStatus

        'If lblDisplayStatus.Text.Trim = HttpContext.GetGlobalResourceObject("Text", "Any", New CultureInfo("zh-TW")) Or lblDisplayStatus.Text.Trim = HttpContext.GetGlobalResourceObject("Text", "Any", New CultureInfo("en-us")) Then
        '    lblDisplayStatus.Text = Me.GetGlobalResourceObject("Text", "Any").ToString().Trim
        'Else
        '    For Each dr As DataRow In dt.Rows
        '        If lblDisplayStatus.Text.Trim = dr("Status_Description_Chi").ToString.Trim Or lblDisplayStatus.Text.Trim = dr("Status_Description").ToString.Trim Then
        '            If LCase(strSelectedLanguage).Equals(TradChinese) Then
        '                lblDisplayStatus.Text = dr("Status_Description_Chi").ToString.Trim
        '            Else
        '                lblDisplayStatus.Text = dr("Status_Description").ToString.Trim
        '            End If
        '        End If
        '    Next
        'End If

        If strSelectedStatus.Trim.Equals(String.Empty) Then
            lblDisplayStatus.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else
            Dim strEngStatus As String = String.Empty
            Dim strChiStatus As String = String.Empty
            Dim strCNStatus As String = String.Empty

            Status.GetDescriptionFromDBCode("HCSPeHSAccRectificationStatus", strSelectedStatus, strEngStatus, strChiStatus, strCNStatus)
            If LCase(strSelectedLanguage).Equals(TradChinese) Then
                lblDisplayStatus.Text = strChiStatus
            ElseIf LCase(strSelectedLanguage).Equals(SimpChinese) Then
                lblDisplayStatus.Text = strCNStatus
            Else
                lblDisplayStatus.Text = strEngStatus
            End If

        End If

        ' Rebind Gridview gvAcctList to display the search result
        If Not IsNothing(Session(SESS_SearchResultList)) Then
            Me.GridViewDataBind(Me.gvAcctList, Session(SESS_SearchResultList))
        End If

        Me.udcClaimTran.chgLanguage()

        'Rebind Doc type in rectification
        'Get Documnet type full name
        If Not IsNothing(udtEHSAccount) Then
            Dim strDocumentTypeFullName As String
            Dim udtDocTypeBLL As DocTypeBLL = New DocTypeBLL
            Dim udtDocTypeModelList As DocType.DocTypeModelCollection
            udtDocTypeModelList = udtDocTypeBLL.getAllDocType()
            strDocumentTypeFullName = udtDocTypeModelList.Filter(Me.txtDocCode.Text.Trim).DocName(udtSessionHandler.Language)
            lblRectifyDocType.Text = strDocumentTypeFullName


            'Reminder
            udtEHSAccount = Me.udtSessionHandler.EHSAccountGetFromSession(FuncCode)

            If udtEHSAccount.TransactionID.Equals(String.Empty) Then
                If udtEHSAccount.RecordStatus.Trim.Equals(VRAcctValidatedStatus.Invalid) Then
                    Me.lblReminder.Text = Me.GetGlobalResourceObject("Text", "RectificationRemoveAccountReminder")
                Else
                    Me.panReminder.Visible = False
                End If

            Else
                Dim udtTranMaintBLL As TransactionMaintenanceBLL = New TransactionMaintenanceBLL

                If udtTranMaintBLL.CheckTransactionVoidable(udtEHSAccount.TransactionID.Trim) Then
                    ' Show Message to reminder SP to void transaction when sp can void the transaction
                    Me.lblReminder.Text = Me.GetGlobalResourceObject("Text", "RectificationReminder")
                Else
                    ' TO:DO
                    'Show Message to reminder SP to contact VU since the transaction is not able to void
                    Me.lblReminder.Text = Me.GetGlobalResourceObject("Text", "RectificationReminderContactDH")
                End If
            End If

            If Me.txtDocCode.Text.Trim.Equals(DocType.DocTypeModel.DocTypeCode.HKIC) Then
                If udtEHSAccount.EHSPersonalInformationList(0).CreateBySmartID Then
                    Me.lblRectifyCreationMethod.Text = Me.GetGlobalResourceObject("Text", "SmartIC")
                Else
                    Me.lblRectifyCreationMethod.Text = Me.GetGlobalResourceObject("Text", "ManualInput")
                End If
            End If

            ' CRE20-003-03 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
            If IsReadOnly(udtEHSAccount) Then
                ' CRE20-003-03 Enhancement on Programme or Scheme using batch upload [End][Winnie]
                ibtnRectifyAccountModify.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ModifyDisableBtn")
                ibtnRectifyAccountSave.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SaveDisableBtn")
            Else
                ibtnRectifyAccountModify.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ModifyBtn")
                ibtnRectifyAccountSave.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SaveBtn")
            End If

            Me.lblReminder.Text = Me.lblReminder.Text.Replace("%s", udtDocTypeModelList.Filter(Me.txtDocCode.Text.Trim).DocIdentityDesc(udtSessionHandler.Language))

        End If

        If Not ViewState("RejectDatetime") Is Nothing Then
            Dim dtmReject As DateTime
            dtmReject = CType(ViewState("RejectDatetime"), DateTime)
            Me.lblRejectDate.Text = udtFormatter.formatDateTime(dtmReject)
        End If

        If Me.mvRectify.ActiveViewIndex = intPracticeSelection Then
            Dim udtPracticeDisplay As PracticeDisplayModel = Me.udtSessionHandler.PracticeDisplayGetFromSession(FuncCode)

            If Not IsNothing(udtPracticeDisplay) Then
                If LCase(udtSessionHandler.Language()).Equals(TradChinese) OrElse LCase(udtSessionHandler.Language()).Equals(SimpChinese) Then
                    Me.lblPracticeSelection.Text = Me.formatDisplayPracticeChiNameAdress(udtPracticeDisplay)
                Else
                    Me.lblPracticeSelection.Text = Me.formatDisplayPracticeNameAdress(udtPracticeDisplay)
                End If
            End If
        End If

        If Me.ibtnConfirm.Enabled Then
            Me.ibtnConfirm.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmBtn")
        Else
            Me.ibtnConfirm.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmDisableBtn")
        End If

        lblCreateByOtherSPText.Text = Me.GetGlobalResourceObject("Text", "CreateByOtherSPText")

        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        If Me.ibtnRectifyReadOldSmartIC.Enabled Then
            Me.ibtnRectifyReadOldSmartIC.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ReadOldSmartIDCardBtn")
        Else
            Me.ibtnRectifyReadOldSmartIC.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "ReadOldSmartIDCardDisabledBtn")
        End If

        If Me.ibtnRectifyReadNewSmartIC.Enabled Then
            Me.ibtnRectifyReadNewSmartIC.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ReadNewSmartIDCardBtn")
        Else
            Me.ibtnRectifyReadNewSmartIC.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "ReadNewSmartIDCardDisabledBtn")
        End If

        If Me.ibtnRectifyReadNewSmartICCombo.Enabled Then
            Me.ibtnRectifyReadNewSmartICCombo.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ReadSmartIDBtn")
        Else
            Me.ibtnRectifyReadNewSmartICCombo.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "ReadSmartIDDisableBtn")
        End If

        lblReadOldSmartIDCardNA.Text = Me.GetGlobalResourceObject("Text", "ReadCardAndSearchNA")
        lblReadNewSmartIDCardNA.Text = Me.GetGlobalResourceObject("Text", "ReadCardAndSearchNA")
        lblReadNewSmartIDComboCardNA.Text = Me.GetGlobalResourceObject("Text", "ReadCardAndSearchNA")
        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	        

    End Sub

    Private Sub ClearCurrentSession(ByVal blnCleanSearchResult As Boolean)
        If blnCleanSearchResult Then
            Session(SESS_SearchResultList) = Nothing
            Session.Remove(SESS_SearchResultList)
        End If

        Me.udtSessionHandler.EHSAccountRemoveFromSession(FuncCode)
        Me.udtSessionHandler.EHSTransactionRemoveFromSession(FuncCode)
        Me.udtSessionHandler.SmartIDContentRemoveFormSession(FuncCode)
        Me.udtSessionHandler.PracticeDisplayRemoveFromSession(FuncCode)

        ' CRE20-0XX (HA Scheme) [Start][Winnie]
        Me.udtSessionHandler.SchemeSelectedForPracticeRemoveFromSession(FuncCode)
        ' CRE20-0XX (HA Scheme) [Start][Winnie]

        Session(SESS_InputMode) = Nothing
        Session.Remove(SESS_InputMode)

        Session(SESS_DefaultSetCCCode) = Nothing
        Session.Remove(SESS_DefaultSetCCCode)

        Session(SESS_ModifyAcc) = Nothing
        Session.Remove(SESS_ModifyAcc)

        txtDocCode.Text = String.Empty

        Me.udcCCCode.Clean()
        Me.udcCCCode.CleanSeesion(FuncCode)

        ' I-CRE17-003 (Enhance eHA rectification on HCSP to check with eHS(S) and CMS vaccination) [Start][Winnie]
        Me.udtSessionHandler.CMSVaccineResultRemoveFromSession(FuncCode)
        ' I-CRE17-003 (Enhance eHA rectification on HCSP to check with eHS(S) and CMS vaccination) [End][Winnie]
        Me.udtSessionHandler.CIMSVaccineResultRemoveFromSession(FuncCode)

    End Sub

    Private Sub SetupRectifyDetailScreen(ByVal blnModified As Boolean)
        udtEHSAccount = Me.udtSessionHandler.EHSAccountGetFromSession(FuncCode)

        With udtEHSAccount

            lblRectifyRefNo.Text = Me.udtFormatter.formatSystemNumber(udtEHSAccount.VoucherAccID)

            Dim strDocumentTypeFullName As String
            Dim udtDocTypeBLL As DocTypeBLL = New DocTypeBLL
            Dim udtDocTypeModelList As DocType.DocTypeModelCollection
            Dim udtSmartIDcontent As SmartIDContentModel = New SmartIDContentModel()

            'Get Documnet type full name
            udtDocTypeModelList = udtDocTypeBLL.getAllDocType()
            strDocumentTypeFullName = udtDocTypeModelList.Filter(Me.txtDocCode.Text.Trim).DocName(udtSessionHandler.Language)
            lblRectifyDocType.Text = strDocumentTypeFullName

            If .TransactionID.Equals(String.Empty) Then
                pnlTransactionID.Visible = False
                ibtnRectifyAccountRemove.Visible = True
                ibtnRectifyAccountViewTransaction.Visible = False

                ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                ' If a account is created by "Batch Upload" or "Vaccination File Management", the account cannot remove by "Rectification" 
                If .SourceApp = EHSAccountModel.SourceAppClass.SFUpload Then
                    ibtnRectifyAccountRemove.Visible = False
                    Me.panReminder.Visible = False
                Else
                    If .RecordStatus.Trim.Equals(VRAcctValidatedStatus.Invalid) Then
                        Me.panReminder.Visible = True
                        Me.lblReminder.Text = Me.GetGlobalResourceObject("Text", "RectificationRemoveAccountReminder")
                    Else
                        Me.panReminder.Visible = False
                    End If
                End If
                ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

            Else
                pnlTransactionID.Visible = True
                Me.lblRectifyTransactionID.Text = Me.udtFormatter.formatSystemNumber(udtEHSAccount.TransactionID)
                ibtnRectifyAccountRemove.Visible = False
                ibtnRectifyAccountViewTransaction.Visible = True

                ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
                '-- ---------------------------------------------- --
                If Not Session("REDIRECT_DocCode") Is Nothing AndAlso _
                    Not Session("REDIRECT_strIdentityNumber") Is Nothing Then
                    ibtnRectifyAccountViewTransaction.Visible = False
                    Dim udtEHSTransaction As EHSTransaction.EHSTransactionModel = udtEHSTransactionBLL.LoadClaimTran(udtEHSAccount.TransactionID)
                    BuildRedirectButton(Me.ibtnManagement, ClaimTransactionMaintenance.BuildSearchCriteria(udtEHSTransaction.RecordStatus, _
                                              udtEHSTransaction.TransactionDtm, _
                                              udtEHSTransaction.TransactionDtm, _
                                               udtEHSTransaction.TransactionID), _
                                                 EHSRectification.BuildSearchCriteria(Me.ddlAcctStatus.SelectedValue, _
                                                    udtEHSAccount.VoucherAccID, _
                                                    udtEHSAccount.AccountSourceString, _
                                                      Session("REDIRECT_DocCode").ToString, _
                                                        Session("REDIRECT_strIdentityNumber").ToString))
                End If
                '-- ---------------------------------------------- --
                ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

                Me.panReminder.Visible = True

                Dim udtTranMaintBLL As TransactionMaintenanceBLL = New TransactionMaintenanceBLL

                If udtTranMaintBLL.CheckTransactionVoidable(.TransactionID.Trim) Then
                    ' Show Message to reminder SP to void transaction when sp can void the transaction
                    Me.lblReminder.Text = Me.GetGlobalResourceObject("Text", "RectificationReminder")
                Else
                    ' TO:DO
                    'Show Message to reminder SP to contact VU since the transaction is not able to void
                    Me.lblReminder.Text = Me.GetGlobalResourceObject("Text", "RectificationReminderContactDH")
                End If


            End If

            If Me.txtDocCode.Text.Trim.Equals(DocType.DocTypeModel.DocTypeCode.HKIC) Then
                Me.pnlCreationMethod.Visible = True
                Me.GetCurrentUser(Me.udtSP, Me.udtDataEntry)

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                If SmartIDHandler.EnableSmartID Then
                    Me.ibtnRectifyReadOldSmartIC.Visible = True
                    Me.ibtnRectifyReadNewSmartIC.Visible = True
                Else
                    Me.ibtnRectifyReadOldSmartIC.Visible = False
                    Me.ibtnRectifyReadNewSmartIC.Visible = False
                End If
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                If .EHSPersonalInformationList(0).CreateBySmartID Then
                    Me.lblRectifyCreationMethod.Text = Me.GetGlobalResourceObject("Text", "SmartIC")
                Else
                    Me.lblRectifyCreationMethod.Text = Me.GetGlobalResourceObject("Text", "ManualInput")
                End If

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                'SetupSmartID(udtSP.SPID, txtDocCode.Text.Trim, True)
                SetupSmartID(udtSP.SPID, txtDocCode.Text.Trim, True, .EHSPersonalInformationList(0).SmartIDVer)
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
            Else
                Me.pnlCreationMethod.Visible = False
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Me.ibtnRectifyReadOldSmartIC.Visible = False
                Me.ibtnRectifyReadNewSmartIC.Visible = False

                'SetupSmartID(udtSP.SPID, txtDocCode.Text.Trim, False)
                SetupSmartID(udtSP.SPID, txtDocCode.Text.Trim, False, .EHSPersonalInformationList(0).SmartIDVer)
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

            End If

            Me.lblReminder.Text = Me.lblReminder.Text.Replace("%s", udtDocTypeModelList.Filter(Me.txtDocCode.Text.Trim).DocIdentityDesc(udtSessionHandler.Language))

            ' If Temp A/C is reused
            '   => Modify Btn is allowed
            ' Else
            '   => Save Btn is allowed

            ' if Special A/C => Save Btn is allowed
            If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.TemporaryAccount Then
                If IsReusedAcc(.OriginalAccID) Then
                    lblRectifyRefNo.Text = Me.udtFormatter.formatSystemNumber(udtEHSAccount.OriginalAccID)

                    If blnModified Then
                        ibtnRectifyAccountModify.Visible = False
                        ibtnRectifyAccountSave.Visible = True
                    Else
                        ibtnRectifyAccountModify.Visible = True
                        ibtnRectifyAccountSave.Visible = False
                    End If

                    lblCreateByOtherSPText.Visible = True
                    lblCreateByOtherSPText.Text = Me.GetGlobalResourceObject("Text", "CreateByOtherSPText")
                Else
                    ibtnRectifyAccountModify.Visible = False
                    ibtnRectifyAccountSave.Visible = True

                    lblCreateByOtherSPText.Visible = False
                End If

            ElseIf udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.SpecialAccount Then
                ibtnRectifyAccountModify.Visible = False
                ibtnRectifyAccountSave.Visible = True

                lblCreateByOtherSPText.Visible = False

            End If

            ' CRE20-003-03 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            If udtEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.Validated OrElse _
                udtEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.Removed Then
                Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Information
                Me.udcInfoMsgBox.AddMessage(FunctCode.FUNT990001, SeverityCode.SEVD, MsgCode.MSG00018)
                Me.udcInfoMsgBox.BuildMessageBox()
                ' CRE20-003-03 Enhancement on Programme or Scheme using batch upload [End][Winnie]

            ElseIf .EHSPersonalInformationList(0).Validating Then
                Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Information
                Me.udcInfoMsgBox.AddMessage(FuncCode, SeverityCode.SEVI, MsgCode.MSG00004)
                Me.udcInfoMsgBox.BuildMessageBox()
            End If

            ' CRE20-003-03 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
            If IsReadOnly(udtEHSAccount) Then
                ibtnRectifyAccountModify.Enabled = False
                ibtnRectifyAccountSave.Enabled = False
                ibtnRectifyAccountModify.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ModifyDisableBtn")
                ibtnRectifyAccountSave.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SaveDisableBtn")

                Me.udcRectifyAccount.Mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly

                If IsReadOnly(udtEHSAccount, blnCheckSmartID:=False) Then
                    SetupSmartID(udtSP.SPID, .EHSPersonalInformationList(0).DocCode, False, .EHSPersonalInformationList(0).SmartIDVer)
                Else
                    ' Read Only + Not Validating = IDEAS2.5
                    SetupSmartID(udtSP.SPID, .EHSPersonalInformationList(0).DocCode, True, .EHSPersonalInformationList(0).SmartIDVer)
                End If
                ' CRE20-003-03 Enhancement on Programme or Scheme using batch upload [End][Winnie]
            Else
                ibtnRectifyAccountModify.Enabled = True
                ibtnRectifyAccountSave.Enabled = True
                ibtnRectifyAccountModify.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ModifyBtn")
                ibtnRectifyAccountSave.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SaveBtn")

                SetupSmartID(udtSP.SPID, .EHSPersonalInformationList(0).DocCode, True, .EHSPersonalInformationList(0).SmartIDVer)
            End If
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

        End With

    End Sub

    Private Function IsReusedAcc(ByVal strOriAccID As String) As Boolean
        Dim blnRes As Boolean = False

        If Not IsNothing(strOriAccID) Then
            If Not strOriAccID.Equals(String.Empty) Then
                blnRes = True
            End If
        End If

        Return blnRes
    End Function

    Private Sub SetupInputDocControl(ByVal _udtEHSAccount As EHSAccountModel, ByVal activeChanged As Boolean)
        'BindPersonalInfo(_udtEHSAccount)
        'SetupConfirmRectifyAcc(_udtEHSAccount)
        Select Case mvRectify.ActiveViewIndex
            Case intRectifyAccount
                BindPersonalInfo(_udtEHSAccount, activeChanged)

            Case intConfirmAccount
                SetupConfirmRectifyAcc(_udtEHSAccount)

            Case intTran
                Me.udtEHSTransaction = Me.udtSessionHandler.EHSTransactionGetFromSession(FuncCode)
                Me.udcClaimTran.buildClaimObject(udtEHSTransaction.TransactionID.Trim, udtEHSTransaction, False)

            Case intPracticeSelection
                Dim udtSelectedPracticeDisplay As BLL.PracticeDisplayModel = Nothing
                Dim udtPracticeDisplays As BLL.PracticeDisplayModelCollection = Nothing

                GetCurrentUser(udtSP, udtDataEntry)
                udtPracticeDisplays = Session(SESS_PracticeCollection)

                ' CRE20-0XX (HA Scheme) [Start][Winnie]
                Me.PracticeRadioButtonGroup.SchemeSelection = False
                ' CRE20-0XX (HA Scheme) [End][Winnie]
                Me.PracticeRadioButtonGroup.BuildRadioButtonGroup(udtPracticeDisplays, udtSP.PracticeList, udtSP.SchemeInfoList, udtSessionHandler.Language, PracticeRadioButtonGroup.DisplayMode.BankAccount)

            Case intSmartIDConfirmation
                Me.SetupConfirmSmarIDRectify(_udtEHSAccount, activeChanged)

        End Select
    End Sub

    Private Sub GetCurrentUser(ByRef _udtSP As ServiceProviderModel, ByRef _udtDataEntry As DataEntryUserModel)
        Me.udtSessionHandler.CurrentUserGetFromSession(_udtSP, _udtDataEntry)

        If IsNothing(_udtSP) Then
            Dim udtClaimVoucherBLL As New ClaimVoucherBLL

            'Get Current USer Account
            Me.udtUserAC = UserACBLL.GetUserAC

            If Me.udtUserAC.UserType = Common.Component.SPAcctType.ServiceProvider Then
                'Get SP from form database
                _udtSP = CType(udtUserAC, ServiceProviderModel)
                ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
                _udtSP = Me.udtClaimVoucherBLL.loadSP(_udtSP.SPID, Me.SubPlatform)
                ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

                _udtDataEntry = Nothing

            ElseIf Me.udtUserAC.UserType = Common.Component.SPAcctType.DataEntryAcct Then
                _udtDataEntry = CType(udtUserAC, DataEntryUserModel)

                Dim udtDataEntryAcctBLL As BLL.DataEntryAcctBLL = New BLL.DataEntryAcctBLL
                _udtDataEntry = udtDataEntryAcctBLL.LoadDataEntry(_udtDataEntry.SPID, _udtDataEntry.DataEntryAccount)
                ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
                _udtSP = Me.udtClaimVoucherBLL.loadSP(_udtDataEntry.SPID, Me.SubPlatform)
                ' CRE13-019-02 Extend HCVS to China [End][Lawrence]
            End If

            Me.udtSessionHandler.CurrentUserSaveToSession(_udtSP, _udtDataEntry)

        End If

    End Sub

    Private Function formatDisplayPracticeNameAdress(ByVal PracticeDisplay As PracticeDisplayModel) As String
        Dim strAreaCode As String = String.Empty
        If PracticeDisplay.AddressCode.HasValue Then
            strAreaCode = PracticeDisplay.AddressCode.Value
        End If

        Return PracticeDisplay.PracticeName.Trim + " (" + PracticeDisplay.PracticeID.ToString() + ") [" + formatter.formatAddress(PracticeDisplay.Room, PracticeDisplay.Floor, PracticeDisplay.Block, PracticeDisplay.Building, PracticeDisplay.District, strAreaCode) + "]"
    End Function

    Private Function formatDisplayPracticeChiNameAdress(ByVal PracticeDisplay As PracticeDisplayModel) As String
        Dim strAreaCode As String = String.Empty
        If PracticeDisplay.AddressCode.HasValue Then
            strAreaCode = PracticeDisplay.AddressCode.Value
        End If

        If PracticeDisplay.BuildingChi.Trim() <> "" Then
            Return PracticeDisplay.PracticeNameChi.Trim + "(" + PracticeDisplay.PracticeID.ToString() + ") [" + formatter.formatAddressChi(PracticeDisplay.Room, PracticeDisplay.Floor, PracticeDisplay.Block, PracticeDisplay.BuildingChi, PracticeDisplay.District, strAreaCode) + "]"
        Else
            Return PracticeDisplay.PracticeNameChi.Trim + "(" + PracticeDisplay.PracticeID.ToString() + ") [" + formatter.formatAddressChi(PracticeDisplay.Room, PracticeDisplay.Floor, PracticeDisplay.Block, PracticeDisplay.Building, PracticeDisplay.District, strAreaCode) + "]"
        End If
    End Function

    Private Sub EnableConfirmButton(ByVal enable As Boolean, ByVal confirmButton As ImageButton)
        If enable Then
            confirmButton.Enabled = enable
            confirmButton.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmBtn")
        Else
            confirmButton.Enabled = enable
            confirmButton.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmDisableBtn")
        End If
        confirmButton.AlternateText = Me.GetGlobalResourceObject("AlternateText", "ConfirmBtn")
    End Sub

    '==================================================================== Code for SmartID ============================================================================
    Private Function ReadSmartIC(ByVal udtSmartIDContent As BLL.SmartIDContentModel) As Boolean
        If IsNothing(udtSmartIDContent) OrElse udtSmartIDContent.IsReadSmartID = False OrElse udtSmartIDContent.IsEndOfReadSmartID Then Return False

        Dim blnIsReadSmartIC As Boolean = False

        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)

        Dim ideasBLL As BLL.IdeasBLL = New BLL.IdeasBLL
        Dim strIdeasVersion As String = ideasBLL.ConvertIdeasVersion(udtSmartIDContent.IdeasVersion)

        'Write Start Audit log
        AuditLogRedirectFormIDEAS(udtAuditLogEntry, strIdeasVersion)

        blnIsReadSmartIC = True
        udtSmartIDContent.IsEndOfReadSmartID = True
        Me.udtSessionHandler.SmartIDContentSaveToSession(FuncCode, udtSmartIDContent)
        udtSmartIDContent = Me.udtSessionHandler.SmartIDContentGetFormSession(FuncCode)

        '--------------------------------------------------------------------------------------------------------------------------------------------------
        ' Smart ID Form Ideas
        '--------------------------------------------------------------------------------------------------------------------------------------------------
        Dim ideasHelper As IdeasRM.IHelper = IdeasRM.HelpFactory.createHelper()

        '--------------------------------------------------------------------------------------------------------------------------------------------------
        ' Get CFD
        '--------------------------------------------------------------------------------------------------------------------------------------------------
        Dim udtAuditLogEntry_GetCFD As AuditLogEntry = New AuditLogEntry(FuncCode, Me)

        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Dim ideasSamlResponse As IdeasRM.IdeasResponse = Nothing
        Dim strArtifact As String = String.Empty

        If udtSmartIDContent.IdeasVersion = BLL.IdeasBLL.EnumIdeasVersion.Combo Or _
            udtSmartIDContent.IdeasVersion = BLL.IdeasBLL.EnumIdeasVersion.ComboGender Then

            strArtifact = udtSmartIDContent.Artifact
            ideasSamlResponse = udtSmartIDContent.IdeasSamlResponse
        Else
            strArtifact = ideasBLL.Artifact
            ideasSamlResponse = ideasHelper.getCardFaceData(udtSmartIDContent.TokenResponse, strArtifact, strIdeasVersion)

        End If

        AuditLogGetCFD(udtAuditLogEntry_GetCFD, strArtifact)

        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

        Dim udtPersonalInfoSmartIC As EHSAccountModel.EHSPersonalInformationModel
        Dim isValid As Boolean = True

        Me.udtEHSAccount = Me.udtSessionHandler.EHSAccountGetFromSession(FuncCode)

        If strArtifact Is Nothing OrElse strArtifact = String.Empty Then
            '----------------------------- Error Handling -----------------------------------------------
            ' Error100 - 113
            If Not Request.QueryString("status") Is Nothing Then
                Dim strErrorCode As String = Request.QueryString("status").Trim()
                Dim strErrorMsg As String = IdeasRM.ErrorMessageMapper.MapMAStatus(strErrorCode)
                If Not strErrorMsg Is Nothing Then

                    Me.ReadSamrtIDFail()
                    Me.udcMsgBoxErr.AddMessageDesc(FuncCode, strErrorCode, strErrorMsg)

                    'Write End Audit log
                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    AuditLogGetCFDFail(udtAuditLogEntry_GetCFD, strArtifact, strErrorCode, strErrorMsg, strIdeasVersion)
                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
                    Me.udcMsgBoxErr.BuildMessageDescBox("SmartIDActionFail", udtAuditLogEntry, Common.Component.LogID.LOG00058, "Get CFD Fail")
                    isValid = False
                End If
            End If
        End If

        If isValid Then

            If ideasSamlResponse.StatusCode.Equals("samlp:Success") Then
                EHSClaimBasePage.AuditLogGetCFDComplete(udtAuditLogEntry_GetCFD, strArtifact)

                Dim udtEHSAccountExist As EHSAccountModel = Nothing
                Dim blnNotMatchAccountExist As Boolean = False
                Dim blnExceedDocTypeLimit As Boolean = False
                Dim goToCreation As Boolean = True
                Dim udtEligibleResult As ClaimRules.ClaimRulesBLL.EligibleResult = Nothing
                Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel
                Dim strError As String = String.Empty

                Try
                    If udtSmartIDContent.IsDemonVersion Then
                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                        ' ----------------------------------------------------------------------------------------
                        udtSmartIDContent.EHSAccount = SmartIDDummyCase.GetDummyEHSAccount(udtEHSAccount.SchemeCode, udtSmartIDContent.IdeasVersion)
                        udtSmartIDContent.EHSAccount.EHSPersonalInformationList(0).CName = BLL.VoucherAccountMaintenanceBLL.GetCName(udtSmartIDContent.EHSAccount.EHSPersonalInformationList(0))
                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
                    Else
                        Dim udtCFD As IdeasRM.CardFaceData
                        udtCFD = ideasSamlResponse.CardFaceDate()
                        If IsNothing(udtCFD) Then
                            strError = "ideasSamlResponse.CardFaceDate() is nothing"
                        End If
                        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                        udtSmartIDContent.EHSAccount = ideasBLL.GetCardFaceDataEHSAccount(udtCFD, udtEHSAccount.SchemeCode, FuncCode, udtSmartIDContent)
                        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]
                    End If
                Catch ex As Exception
                    udtSmartIDContent.EHSAccount = Nothing
                    strError = ex.Message
                End Try

                Dim udtAuditlogEntry_Search As AuditLogEntry = New AuditLogEntry(FuncCode, Me)

                Dim strHKICNo As String = String.Empty

                If Not udtSmartIDContent.EHSAccount Is Nothing Then
                    strHKICNo = udtSmartIDContent.EHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).IdentityNum.Trim
                End If

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                AuditLogSearchNvaliatedACwithCFD(udtAuditlogEntry_Search, strHKICNo, strError, strIdeasVersion)
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                If Not IsNothing(udtSmartIDContent.EHSAccount) Then
                    udtPersonalInfoSmartIC = udtSmartIDContent.EHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC)


                    ' Block Case:   Different HKIC No.
                    If Not udtEHSAccount.EHSPersonalInformationList(0).IdentityNum.Trim.Equals(udtPersonalInfoSmartIC.IdentityNum.Trim) Then
                        Me.sm = New SystemMessage(FuncCode, SeverityCode.SEVE, MsgCode.MSG00002)
                        Me.udcMsgBoxErr.AddMessage(sm)
                        isValid = False
                    End If

                    '------------------------------------------------------------------------------------------------------
                    'Card Face Data Validation
                    '------------------------------------------------------------------------------------------------------
                    If isValid Then
                        Me.sm = EHSClaimBasePage.SmartIDCardFaceDataValidation(udtPersonalInfoSmartIC)
                        If Not Me.sm Is Nothing Then
                            isValid = False
                            Me.udcMsgBoxErr.AddMessage(Me.sm)
                        End If
                    End If

                    If isValid Then

                        If IsNothing(sm) Then
                            sm = Me.udtEHSClaimBLL.SearchEHSAccountSmartIDRectification(udtPersonalInfoSmartIC.IdentityNum, udtEHSAccount, udtSmartIDContent.EHSAccount, udtEligibleResult, udtSmartIDContent.SmartIDReadStatus)
                        End If

                        If Not Me.sm Is Nothing Then
                            isValid = False
                            Me.udcMsgBoxErr.AddMessage(Me.sm)
                        End If

                    End If

                    If isValid Then
                        Me.txtDocCode.Text = DocType.DocTypeModel.DocTypeCode.HKIC
                        Me.udtSessionHandler.SmartIDContentSaveToSession(FuncCode, udtSmartIDContent)

                        Me.mvRectify.ActiveViewIndex = Me.ReadSmartICActiveView(udtSmartIDContent, Me.udtEHSAccount)
                        udtEHSPersonalInfo = Me.udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKIC)

                        'After EHSRectification.intRectifyAccount, DocTypeControl cannot update values, we must assign CCCode Here
                        Me.BuildCCCode(udtEHSPersonalInfo.CCCode1, udtEHSPersonalInfo.CCCode2, udtEHSPersonalInfo.CCCode3, udtEHSPersonalInfo.CCCode4, udtEHSPersonalInfo.CCCode5, udtEHSPersonalInfo.CCCode6)

                        AuditLogSearchNvaliatedACwithCFDComplete(udtAuditlogEntry_Search, udtEHSAccount, udtSmartIDContent, strIdeasVersion)
                    Else
                        Me.ReadSamrtIDFail()
                    End If

                Else
                    '---------------------------------------------------------------------------------------------------------------
                    ' udtSmartIDContent.EHSAccount is nothing, crad face data may not be able to return 
                    '---------------------------------------------------------------------------------------------------------------
                    Me.ReadSamrtIDFail()
                    Me.udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00253"))
                    isValid = False
                End If

                Me.udcMsgBoxErr.BuildMessageBox(EHSRectification.strValidationFail, udtAuditlogEntry_Search, Common.Component.LogID.LOG00053, "Search & validate account with CFD Fail")
            Else
                '---------------------------------------------------------------------------------------------------------------
                ' ideasSamlResponse.StatusCode is not "samlp:Success"
                '---------------------------------------------------------------------------------------------------------------
                Me.ReadSamrtIDFail()
                Me.udcMsgBoxErr.AddMessageDesc(FuncCode, ideasSamlResponse.StatusMessage, ideasSamlResponse.StatusDetail)

                'Write End Audit log
                EHSClaimBasePage.AuditLogGetCFDFail(udtAuditLogEntry_GetCFD, strArtifact, ideasSamlResponse.StatusMessage, ideasSamlResponse.StatusDetail, strIdeasVersion)

                Me.udcMsgBoxErr.BuildMessageDescBox("SmartIDActionFail", udtAuditLogEntry_GetCFD, Common.Component.LogID.LOG00058, "Get CFD Fail")
                isValid = False
            End If

        End If

        ' Rebind Gridview gvAcctList to display the search result
        If Not IsNothing(Session(SESS_SearchResultList)) Then
            Me.GridViewDataBind(Me.gvAcctList, Session(SESS_SearchResultList))
        End If

        Return blnIsReadSmartIC
    End Function

    Private Function EHSAccountInputControlBuilMode(ByVal udtEHSAccountPassed As EHSAccountModel) As ucInputDocTypeBase.BuildMode
        Dim mode As ucInputDocTypeBase.BuildMode

        If udtEHSAccountPassed.AccountSource = EHSAccountModel.SysAccountSource.SpecialAccount Then

            ' CRE20-003-03 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
            If IsReadOnly(udtEHSAccount) Then
                ' CRE20-003-03 Enhancement on Programme or Scheme using batch upload [End][Winnie]
                mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly
            Else
                mode = ucInputDocTypeBase.BuildMode.Modification
            End If

        Else
            If Me.IsReusedAcc(udtEHSAccountPassed.OriginalAccID) Then
                mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly
            Else
                ' CRE20-003-03 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
                If IsReadOnly(udtEHSAccount) Then
                    ' CRE20-003-03 Enhancement on Programme or Scheme using batch upload [End][Winnie]
                    mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly
                Else
                    mode = ucInputDocTypeBase.BuildMode.Modification
                End If
            End If
        End If

        Return mode
    End Function

    Private Function ReadSmartICActiveView(ByVal udtSmartIDContent As SmartIDContentModel, ByVal udtEHSAccountOrg As EHSAccountModel) As Integer
        Dim udtSelectedPracticeDisplay As BLL.PracticeDisplayModel = Me.udtSessionHandler.PracticeDisplayGetFromSession(FuncCode)
        Dim udtPracticeDisplays As BLL.PracticeDisplayModelCollection = Nothing

        If Not udtSelectedPracticeDisplay Is Nothing Then
            'Practice have been selected 
            'case 1) SP pressed Modify before Read SmartID (View in edit mode)
            Return EHSRectification.intSmartIDConfirmation
        End If

        Me.GetCurrentUser(udtSP, udtDataEntry)

        If Not udtDataEntry Is Nothing Then
            udtPracticeDisplays = Me._udtPracticeBankAccBLL.getActivePractice(udtSP.SPID, udtDataEntry.DataEntryAccount)
        Else
            udtPracticeDisplays = Me._udtPracticeBankAccBLL.getActivePractice(udtSP.SPID)
        End If

        Session(SESS_PracticeCollection) = udtPracticeDisplays

        If IsReusedAcc(udtEHSAccountOrg.OriginalAccID) Then
            If udtPracticeDisplays.Count = 1 Then

                'Save Practice to session Practice ID will assign to EHS Account before confirm save
                Me.udtSessionHandler.PracticeDisplaySaveToSession(udtPracticeDisplays.Item(0), FuncCode)

                'udtSmartIDContent.EHSAccount.CreateSPPracticeDisplaySeq = udtPracticeDisplays.Item(0).PracticeID
                'Me.udtSessionHandler.SmartIDContentSaveToSession(FuncCode, udtSmartIDContent)

                Return EHSRectification.intSmartIDConfirmation
            Else

                Dim strCurrentPracticeName As String = String.Empty
                Dim strCurrentPracticeNameChi As String = String.Empty

                ' CRE20-0XX (HA Scheme) [Start][Winnie]
                Me.PracticeRadioButtonGroup.SchemeSelection = False
                ' CRE20-0XX (HA Scheme) [End][Winnie]

                Me.PracticeRadioButtonGroup.BuildRadioButtonGroup(udtPracticeDisplays, udtSP.PracticeList, udtSP.SchemeInfoList, udtSessionHandler.Language, PracticeRadioButtonGroup.DisplayMode.BankAccount)
                Me.udtEHSTransaction = udtEHSTransactionBLL.LoadClaimTran(udtEHSAccountOrg.TransactionID)

                For Each PracticeDisplay As BLL.PracticeDisplayModel In udtPracticeDisplays
                    If PracticeDisplay.PracticeID = udtEHSTransaction.PracticeID Then

                        strCurrentPracticeName = Me.formatDisplayPracticeNameAdress(PracticeDisplay)
                        strCurrentPracticeNameChi = Me.formatDisplayPracticeChiNameAdress(PracticeDisplay)
                        Me.udtSessionHandler.PracticeDisplaySaveToSession(PracticeDisplay, FuncCode)
                        'Session(SESS_PracticeDisplay) = PracticeDisplay
                        Exit For
                    End If
                Next

                Dim strSelectedLanguage As String
                strSelectedLanguage = LCase(udtSessionHandler.Language())

                If LCase(strSelectedLanguage).Equals(TradChinese) OrElse LCase(strSelectedLanguage).Equals(SimpChinese) Then
                    If Not IsNothing(udtEHSTransaction) Then
                        Me.lblPracticeSelection.Text = strCurrentPracticeNameChi
                    End If
                Else
                    If Not IsNothing(udtEHSTransaction) Then
                        Me.lblPracticeSelection.Text = strCurrentPracticeName
                    End If
                End If

                Return EHSRectification.intPracticeSelection

            End If
        Else
            Return EHSRectification.intSmartIDConfirmation
        End If

    End Function

    Private Sub ReadSamrtIDFail()
        '---------------------------------------------------------------------------------------------------------------
        ' any invalid action or flow will keep in Step1 Search Account
        '---------------------------------------------------------------------------------------------------------------
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = Me.udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKIC)

        'Preset Values
        MyBase.Session(SESS_InputMode) = Me.EHSAccountInputControlBuilMode(Me.udtEHSAccount)
        Me.txtDocCode.Text = DocType.DocTypeModel.DocTypeCode.HKIC

        'Setup UI
        Me.SetupRectifyDetailScreen(False)
        Me.mvRectify.ActiveViewIndex = EHSRectification.intRectifyAccount

        'After EHSRectification.intRectifyAccount, DocTypeControl cannot update values, we must assign CCCode Here
        Me.BuildCCCode(udtEHSPersonalInfo.CCCode1, udtEHSPersonalInfo.CCCode2, udtEHSPersonalInfo.CCCode3, udtEHSPersonalInfo.CCCode4, udtEHSPersonalInfo.CCCode5, udtEHSPersonalInfo.CCCode6)

    End Sub

    Private Sub BuildCCCode(ByVal strCCCode1 As String, ByVal strCCCode2 As String, ByVal strCCCode3 As String, ByVal strCCCode4 As String, ByVal strCCCode5 As String, ByVal strCCCode6 As String)
        Me.udcCCCode.CCCode1 = strCCCode1
        Me.udcCCCode.CCCode2 = strCCCode2
        Me.udcCCCode.CCCode3 = strCCCode3
        Me.udcCCCode.CCCode4 = strCCCode4
        Me.udcCCCode.CCCode5 = strCCCode5
        Me.udcCCCode.CCCode6 = strCCCode6

        udcCCCode.BindCCCode()
    End Sub

    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub PreRedirectToIdeas(ByVal blnShowPopup As Boolean, ByVal enumIDEASVersion As IdeasBLL.EnumIdeasVersion)
        Dim udtSmarIDContent As BLL.SmartIDContentModel = New BLL.SmartIDContentModel
        Dim udtPracticeDisplay As PracticeDisplayModel = Me.udtSessionHandler.PracticeDisplayGetFromSession(FuncCode)

        'Practice have been selected 
        'case 1) SP pressed Modify before Read SmartID (View in edit mode)
        If Not udtPracticeDisplay Is Nothing Then
            Select Case enumIDEASVersion
                Case IdeasBLL.EnumIdeasVersion.One, IdeasBLL.EnumIdeasVersion.Two, IdeasBLL.EnumIdeasVersion.TwoGender
                    Me.RedirectToIdeas(blnShowPopup, enumIDEASVersion)
                Case IdeasBLL.EnumIdeasVersion.Combo, IdeasBLL.EnumIdeasVersion.ComboGender
                    Me.RedirectToIdeasCombo(blnShowPopup, enumIDEASVersion)
                Case Else
                    'Nothing to do
            End Select

        Else
            udtSmarIDContent.IsReadSmartID = True

            Dim eIdeasVersion As IdeasBLL.EnumIdeasVersion = IdeasBLL.GetIdeasVersion(enumIDEASVersion)
            udtSmarIDContent.IdeasVersion = eIdeasVersion

            Me.udtSessionHandler.SmartIDContentSaveToSession(FuncCode, udtSmarIDContent)
            Me.udtEHSAccount = Me.udtSessionHandler.EHSAccountGetFromSession(FuncCode)

            If IsReusedAcc(Me.udtEHSAccount.OriginalAccID) Then
                Me.ModalPopupExtenderModify.Show()
            Else
                Select Case enumIDEASVersion
                    Case IdeasBLL.EnumIdeasVersion.One, IdeasBLL.EnumIdeasVersion.Two, IdeasBLL.EnumIdeasVersion.TwoGender
                        Me.RedirectToIdeas(blnShowPopup, enumIDEASVersion)
                    Case IdeasBLL.EnumIdeasVersion.Combo, IdeasBLL.EnumIdeasVersion.ComboGender
                        Me.RedirectToIdeasCombo(blnShowPopup, enumIDEASVersion)
                    Case Else
                        'Nothing to do
                End Select

            End If
        End If

    End Sub
    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

    Private Sub RedirectToIdeas(ByVal blnShowPopup As Boolean, ByVal enumIDEASVersion As IdeasBLL.EnumIdeasVersion)

        ' Clear smart id content to avoid loading smart ic content in page load
        Me.udtSessionHandler.SmartIDContentRemoveFormSession(FuncCode)

        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Dim eIdeasVersion As IdeasBLL.EnumIdeasVersion = IdeasBLL.GetIdeasVersion(enumIDEASVersion)
        Dim strIdeasVersion As String = IdeasBLL.ConvertIdeasVersion(eIdeasVersion)

        Me.AuditLogReadSamrtID(udtAuditLogEntry, strIdeasVersion, Nothing)

        ' (1) Language
        Dim strLang As String = String.Empty

        If LCase(Session("language")) = CultureLanguage.TradChinese Then
            strLang = "zh_HK"
        Else
            strLang = "en_US"
        End If

        ' (2) Remove Card Setting
        Dim strRemoveCard As String = String.Empty

        Dim udtGeneralFunction As New GeneralFunction
        udtGeneralFunction.getSystemParameter("SmartID_RemoveCard", strRemoveCard, String.Empty)
        If strRemoveCard = String.Empty Then
            strRemoveCard = "Y"
        End If

        ' (3) IDEAS token
        Dim ideasHelper As IHelper = HelpFactory.createHelper()
        Dim ideasTokenResponse As TokenResponse = Nothing

        ' Enforce HCSP accept server cert for connecting IDEAS Testing server
        ServicePointManager.ServerCertificateValidationCallback = New Security.RemoteCertificateValidationCallback(AddressOf (New IdeasBLL).ValidateCertificate)

        ' Get Token From Ideas, input: the return URL from Ideas to eHS
        ideasTokenResponse = IdeasBLL.GetToken(eIdeasVersion, Me.Page.Request.Url.GetLeftPart(UriPartial.Path), strLang, strRemoveCard)

        Dim isDemoVersion As String = ConfigurationManager.AppSettings("SmartIDDemoVersion")

        If Not ideasTokenResponse.ErrorCode Is Nothing And Not isDemoVersion.Equals("Y") Then

            Me.udcMsgBoxErr.AddMessageDesc(FuncCode, ideasTokenResponse.ErrorCode, ideasTokenResponse.ErrorMessage)

            AuditLogConnectIdeasFail(udtAuditLogEntry, ideasTokenResponse, "N", strIdeasVersion)

            Me.udcMsgBoxErr.BuildMessageDescBox("SmartIDActionFail", udtAuditLogEntry, Common.Component.LogID.LOG00050, "Click 'Read Smart ID Card' and Token Request Fail")

        Else
            Dim udtSessionHandler As New SessionHandler
            Dim udtSmarIDContent As New SmartIDContentModel
            udtSmarIDContent.IsReadSmartID = True
            udtSmarIDContent.TokenResponse = ideasTokenResponse
            udtSmarIDContent.IdeasVersion = eIdeasVersion

            If isDemoVersion.Equals("Y") Then
                udtSmarIDContent.IsDemonVersion = True

                Me.udtSessionHandler.SmartIDContentSaveToSession(FuncCode, udtSmarIDContent)

                AuditLogConnectIdeasComplete(udtAuditLogEntry, ideasTokenResponse, "Y", strIdeasVersion)

                RedirectHandler.ToURL(ConfigurationManager.AppSettings("SmartIDTestRedirectPageVRARectification").ToString().Replace("@", "&"))

            Else
                udtSmarIDContent.IsDemonVersion = False

                Me.udtSessionHandler.SmartIDContentSaveToSession(FuncCode, udtSmarIDContent)

                AuditLogConnectIdeasComplete(udtAuditLogEntry, ideasTokenResponse, "N", strIdeasVersion)

                ' Redirect to Ideas, no need to add page key
                Response.Redirect(ideasTokenResponse.IdeasMAURL)

            End If
        End If
    End Sub

    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub RedirectToIdeasCombo(ByVal blnShowPopup As Boolean, ByVal enumIDEASVersion As IdeasBLL.EnumIdeasVersion)

        ' Clear smart id content to avoid loading smart ic content in page load
        Me.udtSessionHandler.SmartIDContentRemoveFormSession(FuncCode)

        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Dim eIdeasVersion As IdeasBLL.EnumIdeasVersion = IdeasBLL.GetIdeasVersion(enumIDEASVersion)
        Dim strIdeasVersion As String = IdeasBLL.ConvertIdeasVersion(eIdeasVersion)

        Me.AuditLogReadSamrtID(udtAuditLogEntry, strIdeasVersion, Nothing)

        ' (1) Language
        Dim strLang As String = String.Empty

        If LCase(Session("language")) = CultureLanguage.TradChinese Then
            strLang = "zh_HK"
        Else
            strLang = "en_US"
        End If

        ' (2) Remove Card Setting
        Dim strRemoveCard As String = String.Empty

        Dim udtGeneralFunction As New GeneralFunction
        udtGeneralFunction.getSystemParameter("SmartID_RemoveCard", strRemoveCard, String.Empty)
        If strRemoveCard = String.Empty Then
            strRemoveCard = "Y"
        End If

        ' (3) IDEAS token
        Dim ideasHelper As IHelper = HelpFactory.createHelper()
        Dim ideasTokenResponse As TokenResponse = Nothing

        ' Enforce HCSP accept server cert for connecting IDEAS Testing server
        ServicePointManager.ServerCertificateValidationCallback = New Security.RemoteCertificateValidationCallback(AddressOf (New IdeasBLL).ValidateCertificate)

        ' Get Token From Ideas, input: the return URL from Ideas to eHS
        Select Case eIdeasVersion
            Case IdeasBLL.EnumIdeasVersion.One, IdeasBLL.EnumIdeasVersion.Two, IdeasBLL.EnumIdeasVersion.TwoGender
                ideasTokenResponse = IdeasBLL.GetToken(eIdeasVersion, Me.Page.Request.Url.GetLeftPart(UriPartial.Path), strLang, strRemoveCard)

            Case IdeasBLL.EnumIdeasVersion.Combo, IdeasBLL.EnumIdeasVersion.ComboGender
                Dim strPageName As String = New IO.FileInfo(Me.Request.Url.LocalPath).Name
                Dim strComboReturnURL As String = Me.Page.Request.Url.GetLeftPart(UriPartial.Path)
                Dim strFolderName As String = "/EHSRectification"

                strComboReturnURL = strComboReturnURL.Replace(strFolderName + "/" + strPageName, "/IDEASComboReader/IDEASComboReader.aspx")
                ideasTokenResponse = IdeasBLL.GetToken(eIdeasVersion, strComboReturnURL, strLang, strRemoveCard)

        End Select

        Dim isDemoVersion As String = ConfigurationManager.AppSettings("SmartIDDemoVersion")

        If Not ideasTokenResponse.ErrorCode Is Nothing And Not isDemoVersion.Equals("Y") Then
            udcMsgBoxErr.AddMessageDesc(FuncCode, ideasTokenResponse.ErrorCode, ideasTokenResponse.ErrorMessage)

            AuditLogConnectIdeasFail(udtAuditLogEntry, ideasTokenResponse, "N", strIdeasVersion)

            Me.udcMsgBoxErr.BuildMessageDescBox("SmartIDActionFail", udtAuditLogEntry, Common.Component.LogID.LOG00050, "Click 'Read Smart ID Card' and Token Request Fail")

        Else
            Dim udtSessionHandler As New SessionHandler
            Dim udtSmarIDContent As New SmartIDContentModel
            udtSmarIDContent.IsReadSmartID = True
            udtSmarIDContent.TokenResponse = ideasTokenResponse
            udtSmarIDContent.IdeasVersion = eIdeasVersion

            If isDemoVersion.Equals("Y") Then
                udtSmarIDContent.IsDemonVersion = True

                Me.udtSessionHandler.SmartIDContentSaveToSession(FuncCode, udtSmarIDContent)

                AuditLogConnectIdeasComplete(udtAuditLogEntry, ideasTokenResponse, "Y", strIdeasVersion)

                RedirectHandler.ToURL(ConfigurationManager.AppSettings("SmartIDTestRedirectPageVRARectification").ToString().Replace("@", "&"))

            Else
                udtSmarIDContent.IsDemonVersion = False

                Me.udtSessionHandler.SmartIDContentSaveToSession(FuncCode, udtSmarIDContent)

                AuditLogConnectIdeasComplete(udtAuditLogEntry, ideasTokenResponse, "N", strIdeasVersion)

                ' Prompt the popup include iframe to show IDEAS Combo UI
                ucIDEASCombo.ReadSmartIC(IdeasBLL.EnumIdeasVersion.Combo, ideasTokenResponse, FuncCode)

            End If
        End If
    End Sub
    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	
    '

    ''' <summary>
    ''' Determine the visibility and enable of the "Read Smart ID Card"
    ''' </summary>
    ''' <param name="strSPID">Check whether the SP is in pilot run.</param>
    ''' <param name="strDocCode">Check whether the document is HKIC.</param>
    ''' <param name="blnEnable">Control the enable of the "Read Smart ID Card" (actually only disable if the account is validating).</param>
    ''' <param name="strSmartIDVer">Control the enable of the "Read Old Smart ID Card"</param>
    ''' <remarks></remarks>
    Private Sub SetupSmartID(ByVal strSPID As String, ByVal strDocCode As String, ByVal blnEnable As Boolean, ByVal strSmartIDVer As String)
        ' Init

        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Dim strIDEASComboClientInstalled As String = IIf(udtSessionHandler.IDEASComboClientGetFormSession() Is Nothing, YesNo.No, udtSessionHandler.IDEASComboClientGetFormSession())
        Dim blnIDEASComboClientInstalled As Boolean = IIf(strIDEASComboClientInstalled = YesNo.Yes, True, False)
        Dim blnIDEASComboClientForceToUse As Boolean = IIf((New GeneralFunction).getSystemParameter("SmartID_IDEAS_Combo_Force_To_Use") = YesNo.Yes, True, False)

        tblRectifyReadSmartIC.Visible = False

        tblRectifyReadOldSmartIC.Style.Add("display", "none")
        ibtnRectifyReadOldSmartIC.Visible = False
        ibtnRectifyReadOldSmartIC.Enabled = False
        ibtnRectifyReadOldSmartIC.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "ReadOldSmartIDCardDisabledBtn")

        tblRectifyReadNewSmartIC.Style.Add("display", "none")
        ibtnRectifyReadNewSmartIC.Visible = False
        ibtnRectifyReadNewSmartIC.Enabled = False
        ibtnRectifyReadNewSmartIC.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "ReadNewSmartIDCardDisabledBtn")

        tblRectifyReadNewSmartICCombo.Style.Add("display", "none")
        ibtnRectifyReadNewSmartICCombo.Visible = False
        ibtnRectifyReadNewSmartICCombo.Enabled = False
        ibtnRectifyReadNewSmartICCombo.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "ReadSmartIDDisableBtn")

        divReadOldSmartIDCardNA.Visible = False
        divReadNewSmartIDCardNA.Visible = False
        divReadNewSmartIDComboCardNA.Visible = False

        ibtnRectifyReadSmartIDTips.Visible = False

        trSmartIDSoftwareNotInstalled.Visible = False

        ' Check visible (visible = is pilot run SP + HKIC)
        Dim udtSmartIDcontent As New SmartIDContentModel

        If strDocCode = DocType.DocTypeModel.DocTypeCode.HKIC Then
            If SmartIDHandler.EnableSmartID Then
                tblRectifyReadSmartIC.Visible = True

                ' Check Combo Client installation
                If blnIDEASComboClientForceToUse Then
                    ' Combo Client Only Period
                    tblRectifyReadNewSmartICCombo.Style.Remove("display")
                    ibtnRectifyReadNewSmartICCombo.Visible = True

                Else
                    ' Transition Period
                    If blnIDEASComboClientInstalled Then
                        tblRectifyReadNewSmartICCombo.Style.Remove("display")
                        ibtnRectifyReadNewSmartICCombo.Visible = True

                    Else
                        tblRectifyReadOldSmartIC.Style.Remove("display")
                        tblRectifyReadNewSmartIC.Style.Remove("display")

                        ibtnRectifyReadOldSmartIC.Visible = True
                        ibtnRectifyReadNewSmartIC.Visible = True

                        ibtnRectifyReadSmartIDTips.Visible = True
                    End If
                End If

                ' Check turn on
                If SmartIDHandler.TurnOnSmartID() Then
                    ' Check enable
                    If blnEnable Then
                        If blnIDEASComboClientForceToUse Then
                            ' Combo Client Only Period
                            If blnIDEASComboClientInstalled Then
                                ibtnRectifyReadNewSmartICCombo.Enabled = True
                                ibtnRectifyReadNewSmartICCombo.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "ReadSmartIDBtn")

                            Else
                                ' Disable "Read Smart ID" if the client is not installed
                                ibtnRectifyReadNewSmartICCombo.Enabled = False
                                ibtnRectifyReadNewSmartICCombo.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "ReadSmartIDDisableBtn")
                                trSmartIDSoftwareNotInstalled.Visible = True
                            End If

                        Else
                            ' Transition Period
                            If blnIDEASComboClientInstalled Then
                                ibtnRectifyReadNewSmartICCombo.Enabled = True
                                ibtnRectifyReadNewSmartICCombo.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "ReadSmartIDBtn")

                            Else
                                ' Disable read Old Smart ID if acct is created by New Smart ID Card
                                If SmartIDHandler.CompareVersion(Replace(strSmartIDVer, "C", ""), SmartIDVersion.IDEAS1, "<=") Then
                                    ibtnRectifyReadOldSmartIC.Enabled = True
                                    ibtnRectifyReadOldSmartIC.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "ReadOldSmartIDCardBtn")
                                End If

                                ibtnRectifyReadNewSmartIC.Enabled = True
                                ibtnRectifyReadNewSmartIC.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "ReadNewSmartIDCardBtn")
                            End If
                        End If

                    End If

                Else
                    If blnIDEASComboClientInstalled Then
                        divReadNewSmartIDComboCardNA.Visible = True
                        lblReadNewSmartIDComboCardNA.Text = Me.GetGlobalResourceObject("Text", "ReadCardAndSearchNA")
                    Else
                        divReadOldSmartIDCardNA.Visible = True
                        lblReadOldSmartIDCardNA.Text = Me.GetGlobalResourceObject("Text", "ReadCardAndSearchNA")

                        divReadNewSmartIDCardNA.Visible = True
                        lblReadNewSmartIDCardNA.Text = Me.GetGlobalResourceObject("Text", "ReadCardAndSearchNA")
                    End If

                End If

            End If
        End If

        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

    End Sub

    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    ' Changed to Common Function
    'Private Function TurnOnSmartID() As Boolean
    '    Dim strParmValue As String = String.Empty
    '    Dim udtGeneralFunction As New GeneralFunction
    '    udtGeneralFunction.getSystemParameter("TurnOnSmartID", strParmValue, String.Empty)
    '    Return strParmValue.Trim = "Y"

    'End Function
    ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

    '==================================================================================================================================================================

    Public Function AccountMode(ByVal udtEHAccountPassed As EHSAccountModel) As ucInputDocTypeBase.BuildMode

        If udtEHAccountPassed.AccountSource = EHSAccountModel.SysAccountSource.SpecialAccount Then

            ' CRE20-003-03 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
            If IsReadOnly(udtEHSAccount) Then
                ' CRE20-003-03 Enhancement on Programme or Scheme using batch upload [End][Winnie]

                Return ucInputDocTypeBase.BuildMode.ModifyReadOnly
            Else
                Return ucInputDocTypeBase.BuildMode.Modification
            End If

        Else
            If Me.IsReusedAcc(udtEHAccountPassed.OriginalAccID) Then
                Return ucInputDocTypeBase.BuildMode.ModifyReadOnly
            Else

                ' CRE20-003-03 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
                If IsReadOnly(udtEHSAccount) Then
                    ' CRE20-003-03 Enhancement on Programme or Scheme using batch upload [End][Winnie]
                    Return ucInputDocTypeBase.BuildMode.ModifyReadOnly
                Else
                    Return ucInputDocTypeBase.BuildMode.Modification
                End If
            End If
        End If
    End Function

#End Region

#Region "Override Function"

    Public Overrides Sub GridViewHeaderImage_Click(ByVal sender As Object, ByVal e As Common.Component.SortedGridviewHeader.SortedGridviewHeaderModel.GridViewHeaderImageEventArgs)
        Dim intColumn As Integer
        intColumn = e.intColumn

        Dim strSelectedLanguage As String
        strSelectedLanguage = LCase(udtSessionHandler.Language())

        Select Case intColumn
            Case 1
                popupSchemeNameHelp.Show()
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'udcSchemeLegend.BindSchemeClaim(strSelectedLanguage)
                udcSchemeLegend.BindSchemeClaim(strSelectedLanguage, Me.SubPlatform)
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            Case 2, 3
                popupDocTypeHelp.Show()
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                udcDocTypeLegend.DocTypeLegendSubPlatform = Me.SubPlatform
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
                udcDocTypeLegend.BindDocType(strSelectedLanguage)
        End Select

    End Sub


#End Region

#Region "Pop-up event"

    Public Sub udcClaimTran_VaccineLegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'udcSchemeLegend.BindSchemeClaim(Session("language"))
        udcSchemeLegend.BindSchemeClaim(Session("language"), Me.SubPlatform)
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
        popupSchemeNameHelp.Show()
    End Sub

#End Region

    '==================================================================== Code for SmartID ============================================================================
#Region "Read Smart ID Card"
    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    'Search Account : Read Samrt ID: LOG00048
    Private Sub AuditLogReadSamrtID(ByRef udtAuditLogEntry As AuditLogEntry, ByVal strIdeasVersion As String, ByVal blnIsNewSmartIC As Nullable(Of Boolean))
        Dim strNewSmartIC As String = String.Empty

        If Not blnIsNewSmartIC Is Nothing Then
            strNewSmartIC = IIf(blnIsNewSmartIC, YesNo.Yes, YesNo.No)
        End If

        Me.udtEHSAccount = Me.udtSessionHandler.EHSAccountGetFromSession(FuncCode)

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", Me.udtEHSAccount.VoucherAccID.Trim)
        udtAuditLogEntry.AddDescripton("New Card", strNewSmartIC)
        udtAuditLogEntry.AddDescripton("IDEAS Version", strIdeasVersion)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00048, "Click 'Read Smart ID Card' and Token Request")
    End Sub
    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	


    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    'Search Account : Connect Ideas Complelet: LOG00049
    'Public Shared Sub AuditLogConnectIdeasComplete(ByRef udtAuditLogEntry As AuditLogEntry, ByVal ideasTokenResponse As IdeasRM.TokenResponse, ByVal strDemoVersion As String)
    Public Shared Sub AuditLogConnectIdeasComplete(ByRef udtAuditLogEntry As AuditLogEntry, ByVal ideasTokenResponse As IdeasRM.TokenResponse, ByVal strDemoVersion As String, ByVal strIdeasVersion As String)
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
        udtAuditLogEntry.AddDescripton("Ideas Artifact Receiver URL", ideasTokenResponse.IdeasArtifactReceiverURL)
        udtAuditLogEntry.AddDescripton("Ideas MAURL", ideasTokenResponse.IdeasMAURL)
        udtAuditLogEntry.AddDescripton("Demo Version", strDemoVersion)
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        udtAuditLogEntry.AddDescripton("IDEAS Version", strIdeasVersion)
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00049, "Click 'Read Smart ID Card' and Token Request Complete")
    End Sub

    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    'Search Account : Connect Ideas Fail: LOG00050
    'Public Shared Sub AuditLogConnectIdeasFail(ByRef udtAuditLogEntry As AuditLogEntry, ByVal ideasTokenResponse As IdeasRM.TokenResponse, ByVal strDemoVersion As String)
    Public Shared Sub AuditLogConnectIdeasFail(ByRef udtAuditLogEntry As AuditLogEntry, ByVal ideasTokenResponse As IdeasRM.TokenResponse, ByVal strDemoVersion As String, ByVal strIdeasVersion As String)
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
        udtAuditLogEntry.AddDescripton("Ideas Error Code", ideasTokenResponse.ErrorCode)
        udtAuditLogEntry.AddDescripton("Ideas Error Detail", ideasTokenResponse.ErrorDetail)
        udtAuditLogEntry.AddDescripton("Ideas Error Message", ideasTokenResponse.ErrorMessage)
        udtAuditLogEntry.AddDescripton("Demo Version", strDemoVersion)
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        udtAuditLogEntry.AddDescripton("IDEAS Version", strIdeasVersion)
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

    End Sub
#End Region

#Region "Redirect From IDEAS"

    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    'From IDEAS  : Redirect from IDEAS : LOG00051
    'Public Shared Sub AuditLogRedirectFormIDEAS(ByVal udtAuditLogEntry As AuditLogEntry)
    Public Shared Sub AuditLogRedirectFormIDEAS(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strIdeasVersion As String)
        udtAuditLogEntry.AddDescripton("IDEAS Version", strIdeasVersion)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00051, "Redirect from IDEAS after Token Request")
    End Sub
    ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
#End Region

#Region "Search & validate account with CFD"

    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Public Shared Sub AuditLogSearchNvaliatedACwithCFD(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strHKIC As String, ByVal strError As String, ByVal strIdeasVersion As String)
        udtAuditLogEntry.AddDescripton("HKIC No.", strHKIC)
        udtAuditLogEntry.AddDescripton("Error", strError)
        udtAuditLogEntry.AddDescripton("IDEAS Version", strIdeasVersion)
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00059, "Search & validate account with CFD")
    End Sub

    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    'From IDEAS  : Redirect from IDEAS Complete : LOG00052
    Public Shared Sub AuditLogSearchNvaliatedACwithCFDComplete(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSAccount As EHSAccountModel, ByVal udtSmartIDContent As BLL.SmartIDContentModel, ByVal strIdeasVersion As String)
        udtAuditLogEntry.AddDescripton("Smart ID Read Status", udtSmartIDContent.SmartIDReadStatus.ToString())
        udtAuditLogEntry.AddDescripton("EHS Account", "True")
        udtAuditLogEntry.AddDescripton("IDEAS Version", strIdeasVersion)
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
        EHSAccountCreationBase.AuditLogHKIC(udtAuditLogEntry, udtEHSAccount)

        udtAuditLogEntry.AddDescripton("Smart ID EHSAccount", "True")
        EHSAccountCreationBase.AuditLogHKIC(udtAuditLogEntry, udtSmartIDContent.EHSAccount)

        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00052, "Search & validate account with CFD Complete")
    End Sub

    'From IDEAS  : Redirect from IDEAS Fail : LOG00053
    Public Shared Sub AuditLogSearchNvaliatedACwithCFDFail(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strStepLocation As String, ByVal strErrorCode As String, ByVal strErrorMessage As String)
        udtAuditLogEntry.AddDescripton("Steps Location", strStepLocation)
        udtAuditLogEntry.AddDescripton("Error Code", strErrorCode)
        udtAuditLogEntry.AddDescripton("Error Message", strErrorMessage)

        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00053, "Search & validate account with CFD Fail")
    End Sub

    'From IDEAS  : SmartID Account Validation Fail: LOG00054

#End Region

#Region "Get CFD"

    'Get CFD : Start : 00056
    Public Shared Sub AuditLogGetCFD(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strArtifact As String)
        udtAuditLogEntry.AddDescripton("Artifact", strArtifact)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00056, "Get CFD")
    End Sub

    'Get CFD Complete: 00057
    Public Shared Sub AuditLogGetCFDComplete(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strArtifact As String)
        udtAuditLogEntry.AddDescripton("Artifact", strArtifact)
        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00057, "Get CFD Complete")
    End Sub

    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    'Get CFD Fail: 00058
    Public Shared Sub AuditLogGetCFDFail(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strArtifact As String, ByVal strErrorCode As String, ByVal strErrorMsg As String, ByVal strIdeasVersion As String)
        udtAuditLogEntry.AddDescripton("Artifact", strArtifact)
        udtAuditLogEntry.AddDescripton("Error Code", strErrorCode)
        udtAuditLogEntry.AddDescripton("Error Message", strErrorMsg)
        udtAuditLogEntry.AddDescripton("IDEAS Version", strIdeasVersion)
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
        'udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00058, "Get CFD Fail")
    End Sub

#End Region
    '==================================================================================================================================================================


    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Account which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Return Me.udtSessionHandler.EHSAccountGetFromSession(FuncCode)
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Service Provider which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Me.udtSessionHandler.EHSTransactionGetFromSession(FuncCode)
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Transaction which user working on
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


    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    '-- ---------------------------------------------- --
    Private Sub BuildRedirectButton(ByVal btn As CustomControls.CustomImageButton, ByVal objSearchCriteria As RedirectParameter.SearchCriteriaCollection, Optional ByVal objReturnSearchCriteria As RedirectParameter.SearchCriteriaCollection = Nothing)
        btn.SourceFunctionCode = CType(Me.Page, BasePage).FunctionCode
        btn.TargetFunctionCode = FunctCode.FUNT020301
        btn.TargetUrl = RedirectHandler.AppendPageKeyToURL((New BLL.MenuBLL).GetURL(FunctCode.FUNT020301))

        btn.Build()

        btn.ConstructNewRedirectParameter()
        btn.RedirectParameter.ActionList.Add(RedirectParameterModel.EnumRedirectAction.Search)
        btn.RedirectParameter.ActionList.Add(RedirectParameterModel.EnumRedirectAction.ViewDetail)
        btn.RedirectParameter.SearchCriteria = objSearchCriteria
        btn.RedirectParameter.ReturnParameter = New RedirectParameterModel
        btn.RedirectParameter.ReturnParameter.SourceFunctionCode = btn.TargetFunctionCode
        btn.RedirectParameter.ReturnParameter.TargetFunctionCode = btn.SourceFunctionCode
        btn.RedirectParameter.ReturnParameter.ActionList.Add(RedirectParameterModel.EnumRedirectAction.Search)
        btn.RedirectParameter.ReturnParameter.ActionList.Add(RedirectParameterModel.EnumRedirectAction.ViewDetail)
        btn.RedirectParameter.ReturnParameter.SearchCriteria = objReturnSearchCriteria

    End Sub

    Private Function GetURLByFunctionCode(ByVal strFunctionCode As String) As String
        Dim dr() As DataRow = (New MenuBLL).GetMenuItem.Select(String.Format("Function_Code='{0}'", strFunctionCode))
        If dr.Length <> 1 Then Throw New Exception("RecordConfirmation.GetURLByFunctionCode: Unexpected no. of rows")
        Return dr(0)("URL")
    End Function

    Private Sub ibtnManagement_Click(ByVal sender As System.Web.UI.WebControls.ImageButton, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnManagement.Click
        '_udtAuditLogEntry.WriteLog(AuditLogDesc.ManagementClick_ID, AuditLogDesc.ManagementClick)
        Session.Remove("REDIRECT_DocCode")
        Session.Remove("REDIRECT_strIdentityNumber")
        Dim btn As CustomControls.CustomImageButton = sender.Parent
        btn.TargetUrl = RedirectHandler.AppendPageKeyToURL((New BLL.MenuBLL).GetURL(FunctCode.FUNT020301))
        btn.Redirect()
    End Sub

    Private Sub HandleRedirectAction()
        ' remove the session variables
        Dim udtRedirectParameterBLL As New RedirectParameterBLL
        Dim udtRedirectParameter As RedirectParameterModel = udtRedirectParameterBLL.GetFromSession()
        If IsNothing(udtRedirectParameter) Then Return

        udtRedirectParameterBLL.RemoveFromSession()
        udtRedirectParameterBLL.WriteAuditLog(FunctionCode, Me.Page, udtRedirectParameter)

        If udtRedirectParameter.ActionList.Contains(RedirectParameterModel.EnumRedirectAction.Search) Then
            ' 1st Search for list of accounts
            udtRedirectParameter.SearchCriteria.TryGetValue(EHSRectification.SEARCH_PARAM_EHS_ACCOUNT_RECTIFICATION_LIST, Me.ddlAcctStatus.SelectedValue)
            ibtnSearch_Click(Nothing, Nothing)
        End If
        ' if has view detail, then display view detail.
        If udtRedirectParameter.ActionList.Contains(RedirectParameterModel.EnumRedirectAction.ViewDetail) Then
            ' need to set current EHSAccount information
            Dim strAccountID As String = String.Empty
            Dim strSource As String = String.Empty
            Dim strDocID As String = String.Empty
            Dim strIdentityNo As String = String.Empty

            udtRedirectParameter.SearchCriteria.TryGetValue(EHSRectification.SEARCH_PARAM_EHS_ACCOUNT_ID, strAccountID)
            udtRedirectParameter.SearchCriteria.TryGetValue(EHSRectification.SEARCH_PARAM_EHS_ACCOUNT_SOURCE, strSource)
            udtRedirectParameter.SearchCriteria.TryGetValue(EHSRectification.SEARCH_PARAM_EHS_ACCOUNT_DOCCODE, strDocID)
            udtRedirectParameter.SearchCriteria.TryGetValue(EHSRectification.SEARCH_PARAM_EHS_ACCOUNT_IDENTITY_NO, strIdentityNo)

            Session("REDIRECT_DocCode") = strDocID
            Session("REDIRECT_strIdentityNumber") = strIdentityNo

            Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
            Me.udtAuditLogEntry.AddDescripton("AccountID", strAccountID)
            Me.udtAuditLogEntry.AddDescripton("AccountSource", strSource)
            Me.udtAuditLogEntry.AddDescripton("DocCode", strDocID)

            Dim udtAuditLogInfo As New AuditLogInfo(Nothing, Nothing, strSource, strAccountID, strDocID, strIdentityNo)
            Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00004, AuditLogDesc.SelectAccount, udtAuditLogInfo)

            ' INT13-0006 Fix eHA Recification save check [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            If IsEHSAccountInSearchResult(strAccountID) AndAlso GeteHSAccount(strDocID, strAccountID, strSource) Then
                udtEHSAccount = Me.udtSessionHandler.EHSAccountGetFromSession(FuncCode)

                Session(SESS_InputMode) = Me.AccountMode(udtEHSAccount)

                Session(SESS_DefaultSetCCCode) = True

                Me.txtDocCode.Text = strDocID.Trim
                Me.SetupRectifyDetailScreen(False)
                Me.mvRectify.ActiveViewIndex = intRectifyAccount

                Me.udtAuditLogEntry.AddDescripton("AccountID", strAccountID)
                Me.udtAuditLogEntry.AddDescripton("AccountSource", strSource)
                Me.udtAuditLogEntry.AddDescripton("DocCode", strDocID)
                Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00005, AuditLogDesc.SelectAccountComplete)

            Else
                udcMsgBoxErr.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
                Me.udtAuditLogEntry.AddDescripton("AccountID", strAccountID)
                Me.udtAuditLogEntry.AddDescripton("AccountSource", strSource)
                Me.udtAuditLogEntry.AddDescripton("DocCode", strDocID)
                'udcMsgBoxErr.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00006, AuditLogDesc.SelectAccountFail)
            End If
            ' INT13-0006 Fix eHA Recification save check [End][Koala]
        End If
    End Sub

    Public Const SEARCH_PARAM_EHS_ACCOUNT_RECTIFICATION_LIST As String = "EHS_ACCOUNT_RECTIFICATION_LIST"
    Public Const SEARCH_PARAM_EHS_ACCOUNT_ID As String = "EHS_ACCOUNT_ID"
    Public Const SEARCH_PARAM_EHS_ACCOUNT_SOURCE As String = "EHS_ACCOUNT_SOURCE"
    Public Const SEARCH_PARAM_EHS_ACCOUNT_DOCCODE As String = "EHS_ACCOUNT_DOCCODE"
    Public Const SEARCH_PARAM_EHS_ACCOUNT_IDENTITY_NO As String = "EHS_ACCOUNT_IDENTITY_NO"


    Public Shared Function BuildSearchCriteria(ByVal strEhsAccountRectificationList As String, ByVal strEhsAccountID As String, ByVal strEhsAccountSource As String, ByVal strDocCode As String, ByVal strIdentityNumber As String) As RedirectParameter.SearchCriteriaCollection
        Dim clln As New RedirectParameter.SearchCriteriaCollection
        clln.Add(SEARCH_PARAM_EHS_ACCOUNT_RECTIFICATION_LIST, strEhsAccountRectificationList)
        clln.Add(SEARCH_PARAM_EHS_ACCOUNT_ID, strEhsAccountID)
        clln.Add(SEARCH_PARAM_EHS_ACCOUNT_SOURCE, strEhsAccountSource)
        clln.Add(SEARCH_PARAM_EHS_ACCOUNT_DOCCODE, strDocCode)
        clln.Add(SEARCH_PARAM_EHS_ACCOUNT_IDENTITY_NO, strIdentityNumber)

        Return clln
    End Function

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

    ' INT13-0006 Fix eHA Recification save check [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    ''' <summary>
    ''' Check the specified eHA exist in search result 
    ''' (For confirm the account exist when redirect from other function, e.g. Claim Transaction Management)
    ''' </summary>
    ''' <param name="strEhsAccountID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsEHSAccountInSearchResult(ByVal strEhsAccountID As String) As Boolean
        Dim dt As DataTable = Session(SESS_SearchResultList)
        If dt Is Nothing Then Return False
        Return dt.Select(String.Format("voucher_acc_id='{0}'", strEhsAccountID)).Length > 0
    End Function

    ' INT13-0006 Fix eHA Recification save check [End][Koala]

    ' I-CRE17-003 (Enhance eHA rectification on HCSP to check with eHS(S) and CMS vaccination) [Start][Winnie]
    ''' <summary>
    ''' Get EHS Vaccination record and CMS Vaccination record, and Join together by current claiming scheme
    ''' </summary>
    ''' <param name="udtEHSAccount"></param>
    ''' <param name="SystemMessage"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetVaccinationRecord(ByVal udtEHSAccount As EHSAccountModel, ByRef SystemMessage As Common.ComObject.SystemMessage) As TransactionDetailVaccineModelCollection
        Dim udtVaccinationBLL As New VaccinationBLL
        Dim udtTranDetailVaccineList As TransactionDetailVaccineModelCollection = Nothing
        Dim strSchemeCode As String

        If udtEHSAccount.TransactionID Is Nothing OrElse udtEHSAccount.TransactionID.Trim() = "" Then
            ' without tx
            Return Nothing
        End If

        Dim udtTransactionBLL As New EHSTransactionBLL()
        udtEHSTransaction = udtTransactionBLL.LoadEHSTransaction(udtEHSAccount.TransactionID)

        ' EHSAccount Scheme Code Different with Transaction SchemeCode.
        ' Eg. Re-use DataEntry create EHS Account Or Re-use self created EHS Account (without Transaction), Combine transaction with EHS Account

        strSchemeCode = udtEHSTransaction.SchemeCode.Trim

        ' Check for vaccine only
        Dim udtSchemeClaimModel As SchemeClaimModel = (New SchemeClaimBLL).getAllSchemeClaim_WithSubsidizeGroup().Filter(strSchemeCode)
        If udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeType <> SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVaccine Then
            Return Nothing
        End If

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Dim udtVaccineResultBag As New VaccineResultCollection
        udtVaccineResultBag = udtEHSClaimBLL.GetVaccinationRecord(udtEHSAccount, udtTranDetailVaccineList, FuncCode, _
                                                                  New AuditLogEntry(FuncCode, Me), strSchemeCode)

        Dim blnErrorHA As Boolean = False
        Dim blnErrorDH As Boolean = False

        If udtVaccineResultBag.HAReturnStatus = VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail Then
            ' if fail to enquiry latest record, then show error
            If VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N Then
                SystemMessage = New Common.ComObject.SystemMessage(FuncCode, SeverityCode.SEVE, MsgCode.MSG00003)
                blnErrorHA = True
            End If
        End If

        If udtVaccineResultBag.DHReturnStatus = VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail Then
            ' if fail to enquiry latest record, then show error
            If VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CIMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N Then
                SystemMessage = New Common.ComObject.SystemMessage(FuncCode, SeverityCode.SEVE, MsgCode.MSG00004)
                blnErrorDH = True
            End If
        End If

        If blnErrorHA And blnErrorDH Then
            SystemMessage = New Common.ComObject.SystemMessage(FuncCode, SeverityCode.SEVE, MsgCode.MSG00005)
        End If
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        Return udtTranDetailVaccineList
    End Function
    ' I-CRE17-003 (Enhance eHA rectification on HCSP to check with eHS(S) and CMS vaccination) [End][Winnie]

    ' CRE20-003-03 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
    ' ---------------------------------------------------------------------------------------------------------
    Private Function IsReadOnly(ByVal udtEHSAccount As EHSAccountModel, _
                                Optional ByVal blnCheckSmartID As Boolean = True) As Boolean
        Dim blnReadOnly As Boolean = False

        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)

        If udtEHSPersonalInfo.Validating Then
            ' Cannot modify info during immd validating
            blnReadOnly = True
        End If

        Select Case udtEHSAccount.RecordStatus
            Case EHSAccountModel.TempAccountRecordStatusClass.Removed,
                EHSAccountModel.TempAccountRecordStatusClass.Validated
                blnReadOnly = True
        End Select

        If blnCheckSmartID Then
            If udtEHSPersonalInfo.CreateBySmartID Then
                If udtEHSPersonalInfo.SmartIDVer = SmartIDVersion.IDEAS2_WithGender Or _
                    udtEHSPersonalInfo.SmartIDVer = SmartIDVersion.IDEAS_Combo_New_WithGender Then
                    ' All fields read from smart id
                    blnReadOnly = True
                End If
            End If
        End If

        Return blnReadOnly
    End Function
    ' CRE20-003-03 Enhancement on Programme or Scheme using batch upload [End][Winnie]

    Private Sub lbtnHere_Click(sender As Object, e As EventArgs) Handles lbtnSmartIDSoftwareNotInstalled2.Click

        ScriptManager.RegisterStartupScript(Me, Page.GetType, "UpdateNow", String.Format("javascript:showUpdateNow('{0}');", Session("language")), True)

        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)

        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00071, "Click HERE for software of reading Smart ID card")

    End Sub
End Class
