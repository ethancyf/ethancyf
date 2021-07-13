Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.ServiceProvider
Imports Common.Component.Practice
Imports Common.Validation
Imports Common.Component.HCVUUser
Imports Common.ComObject
Imports Common.DataAccess
Imports Common.Component.DocType
Imports Common.Component.SortedGridviewHeader

Partial Public Class eHSAccountRectification
    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    ' -------------------------------------------------------------------------
    'Inherits BasePageWithGridView
    Inherits BasePageWithControl
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    Dim udtAuditLogEntry As AuditLogEntry
    Dim udtSM As Common.ComObject.SystemMessage
    Dim udtvalidator As Validator = New Validator
    Dim udtformatter As Common.Format.Formatter = New Common.Format.Formatter
    Dim udtCommonFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
    Dim udteHSAccountMaintBLL As eHSAccountMaintBLL = New eHSAccountMaintBLL
    Dim udtEHSAccountBLL As EHSAccountBLL = New EHSAccountBLL
    Dim udtDocTypeBLL As DocTypeBLL = New DocTypeBLL
    Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    ' -------------------------------------------------------------------------
    Dim _blnSkipInputToken As Boolean = False
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    Dim udtEHSAccount As EHSAccountModel
    Dim udtEHSAccount_Amendment As EHSAccountModel
    Dim udtEHSAccount_Rectify As EHSAccountModel

    Enum PageState
        ShowRecordList
        ShowDetailReadOnly
        ShowEnterDetail
        ComfirmDetail
    End Enum

    ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [Start][Koala]
    Private Class AccountTypeClass
        Public Const Validated As String = "V"
        Public Const Temporary As String = "T"
    End Class
    ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [End][Koala]

#Region "Audit Log Description"
    Public Class AuditLogDesc

        Public Const LoadEHSAccountRectification As String = "eHealth Account Rectification Load"  '0

        Public Const SearchEHSAccount As String = "Search eHealth Account for Rectification" '1
        Public Const SearchEHSAccountTooManyRecords As String = "Search eHealth Account for Rectification - Too Many records" '29
        Public Const SearchEHSAccountComplete As String = "Complete Search eHealth Account for Rectification" '2
        Public Const SearchEHSAccountFail As String = "Fail to Search eHealth Account for Rectification" '42
        Public Const NoRecordFound As String = "No Record Found" '3

        Public Const SelectEHSAccount As String = "Select eHealth Account" '4
        Public Const SelectEHSAccountComplete As String = "Complete Select eHealth Account" '5
        Public Const SelectEHSAccountFail As String = "Select eHealth Account Fail" '28
        Public Const EditEHSAccount As String = "Edit eHealth Account" '6
        Public Const SaveEHSAccount As String = "Save eHealth Account" '7
        '------------------- Check Inout data (Personal particulars) ------------------------------------------------
        Public Const ValidateAccountDetailInfo As String = "Validate Account Detail Info (Personal Particulars)" '8
        Public Const ValidateAccountDetailInfoComplete As String = "Validate Account Detail Info (Personal Particulars) Complete" '9
        Public Const ValidateAccountDetailInfoFail As String = "Validate Account Detail Info (Personal Particulars) Fail" '10
        '------------------------------------------------------------------------------------------------------------
        Public Const ConfirmSaveEHSAccount As String = "Confirm Save Amending eHealth Account"  '11
        Public Const ConfirmSaveEHSAccountComplete As String = "Confirm Save Amending eHealth Account Complete" '12
        Public Const ConfirmSaveEHSAccountFail As String = "Confirm Save Amending eHealth Account Fail" '13

        Public Const ClickWithDrawAdmendmentButton As String = "Click Withdraw Admendment button" '14
        Public Const ConfirmWithDrawAdmendment As String = "Confirm withdraw Admendment" '15
        Public Const WithDrawAdmendment As String = "Withdraw Admendment" '16
        Public Const WithDrawAdmendmentComplete As String = "Withdraw Admendment Complete" '17
        Public Const WithDrawAdmendmentFail As String = "Withdraw Admendment Fail" '18
        Public Const CancelWithDrawAdmendment As String = "Cancel withdraw Admendment" '19

        Public Const ReturnToSearchResult As String = "Return To Search Result Page from Completion Page" '20
        Public Const CancelEdit As String = "Cancel Edit" '21
        Public Const BackToDetail As String = "Back to Account Detail" '22

        Public Const ClickSelectChineseNameButton As String = "Click Select Chinese Name Button" '23
        Public Const ChineseNameCodeCheckingSuccess As String = "Chinese Name Code Checking Success" '24
        Public Const ChineseNameCodeCheckingFail As String = "Chinese Name Code Checking Fail" '25
        Public Const ConfirmChineseName As String = "Confirm the selection of Chinese Name" '26
        Public Const CancelChineseName As String = "Cancel the selection of Chinese Name" '27

        ' - Remove Temporary Account
        Public Const RemoveTempAcctClick As String = "Remove Temporary Account Click" '30
        Public Const ConfirmRemoveTempAcct As String = "Confirm Remove Temporary Account" '31
        Public Const ConfirmRemoveTempAcctSuccess As String = "Confirm Remove Temporary Account Success" '32
        Public Const ConfirmRemoveTempAcctFail As String = "Confirm Remove Temporary Account Fail" '33
        Public Const CancelRemoveTempAcct As String = "Cancel Remove Temporary Account" '34

        '-- Temporary Account
        Public Const ConfirmModifiedTempEHSAccount As String = "Confirm Modified Temp eHealth Account"  '35
        Public Const SaveModifiedTempEHSAccountComplete As String = "Save Modified Temp eHealth Account Complete" '36
        Public Const SaveModifiedTempEHSAccountFail As String = "Save Modified Temp eHealth Account Fail" '37

        '-- Special Account
        Public Const ConfirmModifiedSpecialEHSAccount As String = "Confirm Modified Special eHealth Account"  '38
        Public Const SaveModifiedSpecialEHSAccountComplete As String = "Confirm Modified Special eHealth Account Complete" '39
        Public Const SaveModifiedSpecialEHSAccountFail As String = "Confirm Modified Special eHealth Account Fail" '40

        '--> Max : 41

        'For EC control --> specific format / free format
        '94 Serial No. Not Provided checked: <Previous Serial No.><Checked after: [Y|N]>
        '95 Reference Other Formats clicked: <Previous Reference 1><Previous Reference 2><Previous Reference 3><Previous Reference 4>
        '96 Reference Specific Format clicked: <Previous Reference>

        Public Const ClickShowSpecialAccountCheckBox As String = "Click Show Special Account Check box" '41

        Public Const ClickFilterRecordButton As String = "Click Filter Record Button" ' 42
        ''Current Max : 42
    End Class

#End Region

#Region "Constant Value"
    Private Const intShowList As Integer = 0
    Private Const intAccountDetails As Integer = 1
    Private Const intConfirm As Integer = 2
    Private Const intComplete As Integer = 3

    Private Const SESS_FilterAdoptionPrefixNum As String = "eHSAccountMaint_AdoptionPrefixNum"
    Private Const SESS_TurnOnAccountRectificationFilter As String = "eHSAccountMaint_TurnOnAccountRectificationFilter"
    Private Const SESS_FilterSpecialAccount As String = "eHSAccountMaint_FilterSpecialAccount"
    Private Const SESS_FilterAccountStatus As String = "eHSAccountMaint_FilterAccountStatus"
    Private Const SESS_FilterDocCode As String = "eHSAccountMaint_FilterDocCode"
    Private Const SESS_FilterIdentityNum As String = "eHSAccountMaint_FilterIdentityNum"
    Private Const SESS_ShowSpecialAccount As String = "eHSAccountMaint_ShowSpecialAccount"
    Private Const SESS_InputMode As String = "eHSAccountMaint_InputMode"
    Private Const SESS_PageState As String = "eHSAccountRectification_PageState"
    Private Const SESS_RectifyAccList As String = "eHSAccountRectification_RectifyAccList"
    Private Const SESS_DefaultSetCCCode As String = "eHSAccountRectification_DefaultSetCCCode"
    Private Const SESS_Language As String = "language"
    Private Const SESS_RequirePageLoadPersonalBind As String = "eHSAccountRectification_RequirePageLoadBind"
    Private Const SESS_DetailPageShowDeceased As String = "SESS_DetailPageShowDeceased"
    Private Const SESS_PopupActionMode As String = "eHSAccountRectification_PopupActionMode"
    Private Const strValidationFail As String = "ValidationFail"
    Private Const SESS_ClickSave As String = "eHSAccountRectification_PressSave"
    Private Const SESS_ServiceProvider As String = "eHSAccountRectification_ServiceProviderModel"
    Private Const SESS_AccountCreateBy As String = "eHSAccountRectification_AccountCreateBy"
    Private Const FuncCode As String = FunctCode.FUNT010303
    Private Const CommonFuncCode As String = Common.Component.FunctCode.FUNT990000
#End Region

#Region "Page Event"

    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    ' -------------------------------------------------------------------------
#Region "SF Search"

    Protected Overrides Sub SF_InitSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry)
    End Sub

    Protected Overrides Function SF_ValidateSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry) As Boolean
        Return True
    End Function

    Protected Overrides Function SF_Search(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry, ByVal blnOverrideResultLimit As Boolean) As Common.Component.BaseBLL.BLLSearchResult
        Dim bllSearchResult As Common.Component.BaseBLL.BLLSearchResult

        'clear the session
        Me.Session(SESS_RectifyAccList) = Nothing

        'Default : Not to show special account
        Dim blnShowSpecialAcc As Boolean = False
        'Dim dt As DataTable
        Dim strDocCode As String = ""
        Dim strIdentityNum As String = ""
        Dim strAccountStatus As String = ""
        Dim strAdoptionPrefixNum As String = ""

        If Session(SESS_TurnOnAccountRectificationFilter) = "Y" Then
            If Not IsNothing(Me.Session(SESS_FilterDocCode)) Then
                strDocCode = Session(SESS_FilterDocCode)
            End If
            If Not IsNothing(Me.Session(SESS_FilterIdentityNum)) Then
                strIdentityNum = Session(SESS_FilterIdentityNum)
            End If
            If Not IsNothing(Me.Session(SESS_FilterAccountStatus)) Then
                strAccountStatus = Session(SESS_FilterAccountStatus)
            End If
            If Not IsNothing(Me.Session(SESS_FilterSpecialAccount)) Then
                blnShowSpecialAcc = Me.Session(SESS_FilterSpecialAccount)
            End If
            If Not IsNothing(Me.Session(SESS_FilterAdoptionPrefixNum)) Then
                strAdoptionPrefixNum = Me.Session(SESS_FilterAdoptionPrefixNum)
            End If
        Else
            If Not IsNothing(Me.Session(SESS_ShowSpecialAccount)) Then
                blnShowSpecialAcc = Me.Session(SESS_ShowSpecialAccount)
            End If
        End If

        If blnOverrideResultLimit Then
            bllSearchResult = udteHSAccountMaintBLL.getRectifyList(Me.FunctionCode, blnShowSpecialAcc, strDocCode, strIdentityNum, strAdoptionPrefixNum, strAccountStatus, True)
        Else
            bllSearchResult = udteHSAccountMaintBLL.getRectifyList(Me.FunctionCode, blnShowSpecialAcc, strDocCode, strIdentityNum, strAdoptionPrefixNum, strAccountStatus)
        End If

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

                Me.mveHSAccount.ActiveViewIndex = intShowList

                Me.panAccountList.Visible = False

                'Log record no record found
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00003, AuditLogDesc.NoRecordFound)

            Case Else
                blnShowResultList = True

        End Select

        If blnShowResultList Then
            Me.Session(SESS_RectifyAccList) = dt

            Me.mveHSAccount.ActiveViewIndex = intShowList
            Me.panAccountList.Visible = True

            Dim strHeaderTitle As String = String.Empty
            strHeaderTitle = Me.GetGlobalResourceObject("Text", "AccountID") + " / " + Me.GetGlobalResourceObject("Text", "RefNo")
            Me.gvAcctList.Columns.Item(3).HeaderText = strHeaderTitle
            Me.GridViewDataBind(Me.gvAcctList, dt, "IdentityNum", "ASC", False)

            'Log record exist
            If Not IsNothing(Me.udtAuditLogEntry) Then
                Me.udtAuditLogEntry.AddDescripton("NoOfRecord", dt.Rows.Count)
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00002, AuditLogDesc.SearchEHSAccountComplete)
            End If

        End If

        Return intRowCount
    End Function

    Protected Overrides Sub SF_FinalizeSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry)
    End Sub

    Protected Overrides Sub SF_ConfirmSearch_Click()
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim enumSearchResult As SearchResultEnum

        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00001, AuditLogDesc.SearchEHSAccount)

        ' Clear message box
        Me.udcInfoMsgBox.Clear()
        Me.udcMsgBox.Clear()

        'clear the session
        Me.Session(SESS_RectifyAccList) = Nothing

        'Default : Not to show special account
        Dim blnShowSpecialAcc As Boolean = False
        Dim strDocCode As String = ""
        Dim strIdentityNum As String = ""
        Dim strAdoptionPrefixNum As String = ""

        If Session(SESS_TurnOnAccountRectificationFilter) = "Y" Then
            If Not IsNothing(Me.Session(SESS_FilterDocCode)) Then
                strDocCode = Session(SESS_FilterDocCode)
            End If
            If Not IsNothing(Me.Session(SESS_FilterIdentityNum)) Then
                strIdentityNum = Session(SESS_FilterIdentityNum)
            End If
            If Not IsNothing(Me.Session(SESS_FilterSpecialAccount)) Then
                blnShowSpecialAcc = Me.Session(SESS_FilterSpecialAccount)
            End If
            If Not IsNothing(Me.Session(SESS_FilterAdoptionPrefixNum)) Then
                strAdoptionPrefixNum = Me.Session(SESS_FilterAdoptionPrefixNum)
            End If
        Else
            If Not IsNothing(Me.Session(SESS_ShowSpecialAccount)) Then
                blnShowSpecialAcc = Me.Session(SESS_ShowSpecialAccount)
            End If
        End If

        'If Not IsNothing(Me.Session(SESS_ShowSpecialAccount)) Then
        ' blnShowSpecialAcc = Me.Session(SESS_ShowSpecialAccount)
        ' End If

        Try
            '-----------------------------------------------
            'Retrieve Validated Accounts 
            '-----------------------------------------------           
            'dt = udteHSAccountMaintBLL.getRectifyList(blnShowSpecialAcc, strIdentityNum, strAdoptionPrefixNum)
            enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, udcMsgBox, udcInfoMsgBox, False, True)

        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00042, AuditLogDesc.SearchEHSAccountFail)
            Throw ex
        End Try

        Select Case enumSearchResult
            Case SearchResultEnum.Success
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00002, AuditLogDesc.SearchEHSAccountComplete)

            Case Else
                Throw New Exception("Error: Class = [HCVU.eHSAccountRectification], Method = [SF_ConfirmSearch_Click], Message = The unexpected error for the result of re-search")

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

        If Not IsPostBack Then
            Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
            udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00000, AuditLogDesc.LoadEHSAccountRectification)

            FunctionCode = FunctCode.FUNT010303

            Dim strParmValue As String = ""
            udtGeneralFunction.getSystemParameter("TurnOnAccountRectificationFilter", strParmValue, String.Empty)
            Session(SESS_TurnOnAccountRectificationFilter) = strParmValue
            If strParmValue.Trim = "Y" Then
                mveFilter.ActiveViewIndex = 0
            Else
                mveFilter.ActiveViewIndex = 1
            End If

            SetupAccountTypeDropDown()
            SetupAccountStatusDropDown()

            Session(SESS_ShowSpecialAccount) = False

            'Me.SetupPageContent(Nothing, Nothing, Nothing, False)

            Session(SESS_RequirePageLoadPersonalBind) = False

            Dim strBrowser As String = String.Empty
            Try
                If Not HttpContext.Current.Request.Browser Is Nothing Then
                    strBrowser = HttpContext.Current.Request.Browser.Type
                End If
            Catch ex As Exception
                strBrowser = String.Empty
            End Try

            MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnConfirmAmendedAccount)
            MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnDialogConfirm)

        Else
            'Decide when to bind personal information (PageLoad or Page_PreRender)
            If Session(SESS_RequirePageLoadPersonalBind) Or Session(SESS_DetailPageShowDeceased) Then
                udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FuncCode)
                udtEHSAccount_Amendment = Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FuncCode)
                udtEHSAccount_Rectify = Me.udteHSAccountMaintBLL.EHSAccount_Rectify_GetFromSession(FuncCode)
                Me.SetupPageContent(udtEHSAccount, udtEHSAccount_Amendment, udtEHSAccount_Rectify, False)

                Session(SESS_DetailPageShowDeceased) = False
            End If
        End If
    End Sub
    ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [Start][Koala]

    Private Sub SetupAccountStatusDropDown()
        'Rebind the Dropdownlist in search critria
        ddlAcctStatus.Items.Clear()

        Dim dt As DataTable = Status.GetDescriptionListFromDBEnumCode(HCVUeHSAccRectificationStatus.ClassCode, True)
        ddlAcctStatus.DataSource = dt
        ddlAcctStatus.DataValueField = "Status_Value"
        ddlAcctStatus.DataTextField = "Status_Description"
        ddlAcctStatus.DataBind()
        ddlAcctStatus.SelectedValue = HCVUeHSAccRectificationStatus.ValidationFailed

        Session(SESS_FilterAccountStatus) = ddlAcctStatus.SelectedValue
    End Sub

    Private Sub SetupAccountTypeDropDown()
        Dim udtDocTypeBLL As DocTypeBLL = New DocTypeBLL
        Me.ddlSearchDocType.Items.Clear()
        Me.ddlSearchDocType.DataSource = udtDocTypeBLL.getAllDocType
        Me.ddlSearchDocType.DataTextField = "DocName"
        Me.ddlSearchDocType.DataValueField = "DocCode"
        Me.ddlSearchDocType.DataBind()
        Me.ddlSearchDocType.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))
        Me.ddlSearchDocType.SelectedIndex = 0
    End Sub
    ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [End][Koala]

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If IsPostBack Then
            If Not Session(SESS_RequirePageLoadPersonalBind) AndAlso Me.mveHSAccount.ActiveViewIndex <> intShowList Then
                udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FuncCode)
                udtEHSAccount_Amendment = Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FuncCode)
                udtEHSAccount_Rectify = Me.udteHSAccountMaintBLL.EHSAccount_Rectify_GetFromSession(FuncCode)
            End If

            'when returning from complete view
            If Session(SESS_RequirePageLoadPersonalBind) And Me.mveHSAccount.ActiveViewIndex = intShowList Then
                udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FuncCode)
                udtEHSAccount_Amendment = Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FuncCode)
                udtEHSAccount_Rectify = Me.udteHSAccountMaintBLL.EHSAccount_Rectify_GetFromSession(FuncCode)
                Me.SetupPageContent(udtEHSAccount, udtEHSAccount_Amendment, udtEHSAccount_Rectify, True)
            End If
        End If
    End Sub
#End Region

#Region "View 1 - Show Rectification List"

#Region "Gridview Function - gvAcctList"

    Private Sub gvAcctList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvAcctList.PageIndexChanging
        Me.GridViewPageIndexChangingHandler(sender, e, SESS_RectifyAccList)
    End Sub

    Private Sub gvAcctList_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvAcctList.PreRender

        Me.GridViewPreRenderHandler(sender, e, SESS_RectifyAccList)
    End Sub

    Private Sub gvAcctList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvAcctList.RowCommand
        If TypeOf e.CommandSource Is LinkButton Then
            Dim strDocCode As String = String.Empty
            Dim strAccountID As String = String.Empty
            Dim strAccountSource As String = String.Empty
            Dim strIdentityNum As String = String.Empty

            Dim blnShowAmendmentRecord As Boolean = False

            Dim strCommandArgument As String

            strCommandArgument = e.CommandArgument.ToString.Trim
            strAccountID = strCommandArgument.Split("|")(0).Trim
            strDocCode = strCommandArgument.Split("|")(1).Trim
            strAccountSource = strCommandArgument.Split("|")(2).Trim
            strIdentityNum = strCommandArgument.Split("|")(3).Trim

            'Audit Log
            Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
            Me.udtAuditLogEntry.AddDescripton("Account_ID", strAccountID)
            Me.udtAuditLogEntry.AddDescripton("Doc_Code", strDocCode)
            Me.udtAuditLogEntry.AddDescripton("Account_Source", strAccountSource)
            Me.udtAuditLogEntry.AddDescripton("IdentityNum", strIdentityNum)
            Dim udtAuditLogInfo As AuditLogInfo
            udtAuditLogInfo = New AuditLogInfo("", Nothing, strAccountSource, _
                                            strAccountID, strDocCode, Me.udtformatter.formatDocumentIdentityNumber(strDocCode, strIdentityNum))
            Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00004, AuditLogDesc.SelectEHSAccount, udtAuditLogInfo)

            txtDocCode.Text = strDocCode

            If Me.GeteHSAcc(strAccountID, strAccountSource) Then

                If strAccountSource.Trim = EHSAccountModel.SysAccountSourceClass.ValidateAccount Then

                    udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FuncCode)
                    udtEHSAccount_Amendment = Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FuncCode)

                    Session(SESS_InputMode) = ActionModel.ReadOnly_withOriginal
                    SetAccountBtn(Session(SESS_InputMode))

                    BindPersonalInfo(udtEHSAccount, udtEHSAccount_Amendment, Nothing, strDocCode, True)

                ElseIf strAccountSource.Trim = EHSAccountModel.SysAccountSourceClass.SpecialAccount Then

                    udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FuncCode)

                    Session(SESS_InputMode) = ActionModel.ReadOnly
                    SetAccountBtn(Session(SESS_InputMode))

                    BindPersonalInfo(udtEHSAccount, Nothing, Nothing, strDocCode, True)

                ElseIf strAccountSource.Trim = EHSAccountModel.SysAccountSourceClass.TemporaryAccount Then

                    udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FuncCode)

                    Session(SESS_InputMode) = ActionModel.ReadOnly
                    SetAccountBtn(Session(SESS_InputMode))

                    BindPersonalInfo(udtEHSAccount, Nothing, Nothing, strDocCode, True)

                End If

                Session(SESS_DefaultSetCCCode) = True

                Me.mveHSAccount.ActiveViewIndex = intAccountDetails
                panFilter.Visible = False

                Session(SESS_RequirePageLoadPersonalBind) = False

                Me.udtAuditLogEntry.AddDescripton("Account_ID", strAccountID)
                Me.udtAuditLogEntry.AddDescripton("Doc_Code", strDocCode)
                Me.udtAuditLogEntry.AddDescripton("Account_Source", strAccountSource)
                Me.udtAuditLogEntry.AddDescripton("IdentityNum", strIdentityNum)
                Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00005, AuditLogDesc.SelectEHSAccountComplete)
            Else
                udcMsgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
                Me.udtAuditLogEntry.AddDescripton("Account_ID", strAccountID)
                Me.udtAuditLogEntry.AddDescripton("Doc_Code", strDocCode)
                Me.udtAuditLogEntry.AddDescripton("Account_Source", strAccountSource)
                Me.udtAuditLogEntry.AddDescripton("IdentityNum", strIdentityNum)
                udcMsgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00028, AuditLogDesc.SelectEHSAccountFail)
            End If
        End If
    End Sub

    Private Sub gvAcctList_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAcctList.RowCreated
        Dim udtSortedGVHeaderList As SortedGridviewHeaderModelCollection = New SortedGridviewHeaderModelCollection
        Dim strImageURL As String = Me.GetGlobalResourceObject("ImageURL", "infoicon")

        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(1, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(2, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))

        Me.GirdViewRowCreated(sender, e, udtSortedGVHeaderList)
    End Sub

    Private Sub gvAcctList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAcctList.RowDataBound

        'Dim strStatus As String = String.Empty
        'strStatus = Me.Session(SESS_ShowSpecialAccount)

        'If e.Row.RowType = DataControlRowType.Header Then
        'If strStatus.Trim = "V" Then
        '    Me.gvAcctList.Columns.Item(3).HeaderText = Me.GetGlobalResourceObject("Text", "VoucherAccountID")
        'Else
        '    Me.gvAcctList.Columns.Item(3).HeaderText = Me.GetGlobalResourceObject("Text", "RefNo")
        'End If
        'End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRow = CType(e.Row.DataItem, System.Data.DataRowView).Row
            Dim lblCName As Label = CType(e.Row.FindControl("lblCName"), Label)
            Dim lbtnIdentityNum As LinkButton = CType(e.Row.FindControl("lbtnIdentityNum"), LinkButton)
            Dim lblDOB As Label = CType(e.Row.FindControl("lblDOB"), Label)
            Dim lblSex As Label = CType(e.Row.FindControl("lblSex"), Label)
            Dim lblCreate_By As Label = CType(e.Row.FindControl("lblCreate_By"), Label)
            Dim lblDateOfIssue As Label = CType(e.Row.FindControl("lblDateOfIssue"), Label)
            Dim lblRecordType As Label = CType(e.Row.FindControl("lblRecordType"), Label)
            Dim lblAccID As Label = CType(e.Row.FindControl("lblAccID"), Label)
            Dim lblAccStatus As Label = CType(e.Row.FindControl("lblAccStatus"), Label)
            Dim lblLastFailedDate As Label = CType(e.Row.FindControl("lblLastFailedDate"), Label)
            ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [Start][Koala]
            Dim lblAccountID As Label = CType(e.Row.FindControl("lblAccountID"), Label)
            ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [End][Koala]

            Dim strIdentityNum As String = CStr(dr.Item("IdentityNum")).Trim
            Dim strVoucherAcctID As String = CStr(dr.Item("Voucher_Acc_ID")).Trim
            Dim strChiName As String = CStr(dr.Item("Chi_Name")).Trim
            Dim dtmDOB As DateTime = CType(dr.Item("DOB"), DateTime)
            Dim strExactDOB As String = CStr(dr.Item("Exact_DOB")).Trim
            Dim strSex As String = CStr(dr.Item("Sex")).Trim
            Dim intAge As Nullable(Of Integer)
            Dim dtDOR As Nullable(Of Date)
            Dim dtDOI As Nullable(Of Date)
            Dim strDocType As String = CStr(dr.Item("doc_code")).Trim
            Dim strAccountSource As String = CStr(dr.Item("Source")).Trim
            Dim strAdoptionPrefixNum As String = CStr(dr.Item("Adoption_Prefix_Num")).Trim
            Dim strOtherInfo As String
            Dim strRecordType As String = CStr(dr.Item("source")).Trim
            Dim strAccID As String = CStr(dr.Item("Voucher_Acc_ID")).Trim
            Dim strAccStatus As String = CStr(dr.Item("Account_Status")).Trim
            Dim dtLastFailDate As Nullable(Of Date)

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

            If IsDBNull(dr.Item("date_of_issue")) Then
                dtDOI = Nothing
            Else
                dtDOI = CType(dr.Item("date_of_issue"), Date)
            End If

            If IsDBNull(dr.Item("Last_Fail_Validate_Dtm")) Then
                dtLastFailDate = Nothing
            Else
                dtLastFailDate = CType(dr.Item("Last_Fail_Validate_Dtm"), Date)
            End If


            If IsDBNull(dr.Item("other_info")) Then
                strOtherInfo = String.Empty
            Else
                strOtherInfo = CStr(dr.Item("other_info"))
            End If


            lbtnIdentityNum.Text = udtformatter.FormatDocIdentityNoForDisplay(strDocType, strIdentityNum, True, strAdoptionPrefixNum)
            lbtnIdentityNum.CommandArgument = strVoucherAcctID & "|" & strDocType & "|" & strAccountSource & "|" & strIdentityNum

            lblCName.Text = udtformatter.formatChineseName(strChiName.Trim)

            lblDOB.Text = udtformatter.formatDOB(strDocType, dtmDOB, strExactDOB, Session(SESS_Language), intAge, dtDOR, strOtherInfo)

            lblSex.Text = Me.GetGlobalResourceObject("Text", udtformatter.formatGender(strSex))

            lblDateOfIssue.Text = udtformatter.formatDOI_GV(dtDOI)
            If lblDateOfIssue.Text.Trim.Equals(String.Empty) Then
                lblDateOfIssue.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            If strRecordType.Trim = "V" Then
                If Not IsDBNull(dr.Item("TempAcc_Status")) Then
                    strAccStatus = CStr(dr.Item("TempAcc_Status")).Trim
                Else
                    strAccStatus = "I"
                End If

                lblAccID.Text = udtformatter.formatValidatedEHSAccountNumber(strAccID)
                lblRecordType.Text = "Validated"
            ElseIf strRecordType.Trim = "S" Then
                lblAccID.Text = udtformatter.formatSystemNumber(strAccID)
                lblRecordType.Text = "Special"
            ElseIf strRecordType.Trim = "T" Then
                lblAccID.Text = udtformatter.formatSystemNumber(strAccID)
                lblRecordType.Text = "Temporary"
            End If

            ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [Start][Koala]
            If strAccountSource.Trim = AccountTypeClass.Validated Then
                lblAccountID.Text = udtformatter.formatValidatedEHSAccountNumber(strVoucherAcctID.Trim)
            Else
                lblAccountID.Text = udtformatter.formatSystemNumber(strVoucherAcctID.Trim)
            End If
            ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [End][Koala]

            'Check_Dtm
            'If strAccStatus = "I" Then
            If dtLastFailDate.HasValue Then
                lblLastFailedDate.Text = udtformatter.formatDateTime(dtLastFailDate)
            Else
                lblLastFailedDate.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            'Status
            Dim strChiStatus As String = String.Empty
            Dim strEngStatus As String = String.Empty
            ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [Start][Koala]
            Status.GetDescriptionFromDBCode(HCVUeHSAccRectificationStatus.ClassCode, strAccStatus, strEngStatus, strChiStatus)
            'Status.GetDescriptionFromDBCode(HCSPeHSAccRectificationStatus.ClassCode, strAccStatus, strEngStatus, strChiStatus)
            ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [End][Koala]
            lblAccStatus.Text = strEngStatus


        End If


    End Sub

    Private Sub gvAcctList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvAcctList.Sorting
        Me.GridViewSortingHandler(sender, e, SESS_RectifyAccList)
    End Sub

#End Region

    Protected Sub cboShowSpecialAccount_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Checked", cboShowSpecialAccount.Checked.ToString())
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00041, AuditLogDesc.ClickShowSpecialAccountCheckBox)

        Me.udcInfoMsgBox.Clear()
        Session(SESS_ShowSpecialAccount) = Me.cboShowSpecialAccount.Checked
        Me.LoadRectifyRecord()

    End Sub

    Protected Sub ibtnFilter_GetRecords(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim strIdentityNum As String = String.Empty
        Dim strAdoptionPrefixNum As String = String.Empty

        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("DocCode", ddlSearchDocType.SelectedValue)
        udtAuditLogEntry.AddDescripton("IdentityNum", txtIdentityNum.Text.Trim())
        udtAuditLogEntry.AddDescripton("Checked", cboFilterSpecialAccount.Checked.ToString())
        udtAuditLogEntry.AddDescripton("AccountStatus", ddlAcctStatus.SelectedValue)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00042, AuditLogDesc.ClickFilterRecordButton)

        Me.udcInfoMsgBox.Clear()

        ' format identity num
        If Not Me.txtIdentityNum.Text.Trim.Equals(String.Empty) Then
            Me.txtIdentityNum.Text = Me.txtIdentityNum.Text.Trim.ToUpper

            Dim strIdentityNumFullTemp As String
            strIdentityNumFullTemp = Me.txtIdentityNum.Text.Trim.ToUpper.Replace("-", "").Replace("(", "").Replace(")", "")

            Dim strIdentityNumFull() As String
            strIdentityNumFull = strIdentityNumFullTemp.Trim.Split("/")
            If strIdentityNumFull.Length > 1 Then
                strIdentityNum = strIdentityNumFull(1)
                strAdoptionPrefixNum = strIdentityNumFull(0)
            Else
                strIdentityNum = strIdentityNumFullTemp
            End If
        End If

        Session(SESS_FilterSpecialAccount) = Me.cboFilterSpecialAccount.Checked
        Session(SESS_FilterDocCode) = Me.ddlSearchDocType.SelectedValue
        Session(SESS_FilterIdentityNum) = strIdentityNum
        Session(SESS_FilterAdoptionPrefixNum) = strAdoptionPrefixNum
        Session(SESS_FilterAccountStatus) = ddlAcctStatus.SelectedValue

        Me.LoadRectifyRecord()
    End Sub


#End Region

#Region "View 2 - Account Details"

    Protected Sub btnBackAcctList_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00020, AuditLogDesc.ReturnToSearchResult)

        Me.mveHSAccount.ActiveViewIndex = intShowList
        panFilter.Visible = True

        'rebind the gridview
        If IsNothing(Session(SESS_RectifyAccList)) Then
            'LoadRectifyRecord(Me.Session(SESS_ShowSpecialAccount))
            LoadRectifyRecord()
        Else
            Me.GridViewDataBind(Me.gvAcctList, Session(SESS_RectifyAccList), "IdentityNum", "ASC", False)
        End If
    End Sub

    Protected Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)

        udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FuncCode)

        ' INT20-0047 (Fix throw error for invalid CCCode) [Start][Winnie]
        ' Reset
        Me.udcCCCode.Clean()
        ' INT20-0047 (Fix throw error for invalid CCCode) [End][Winnie]

        If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
            '------------------------------------------------------------
            'Amendment of validated account
            '------------------------------------------------------------
            udtEHSAccount_Amendment = Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FuncCode)

            'Audit Log
            Me.udtAuditLogEntry.AddDescripton("IdentityNum", udtEHSAccount.EHSPersonalInformationList.Filter(txtDocCode.Text).IdentityNum)
            Me.udtAuditLogEntry.AddDescripton("(Original)Account_ID", udtEHSAccount.VoucherAccID.Trim)
            Me.udtAuditLogEntry.AddDescripton("Doc_Code", txtDocCode.Text)
            Me.udtAuditLogEntry.AddDescripton("Account_Source", udtEHSAccount.AccountSourceString)
            Me.udtAuditLogEntry.AddDescripton("(Amend)Account_ID", udtEHSAccount_Amendment.VoucherAccID.Trim)
            Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00006, AuditLogDesc.EditEHSAccount)

            Session(SESS_InputMode) = ActionModel.Amending_withOriginal
            SetAccountBtn(Session(SESS_InputMode))

            'Create a copy of Amended EHSAccount for rectification
            udtEHSAccount_Rectify = udtEHSAccount_Amendment
            udteHSAccountMaintBLL.EHSAccount_Rectify_SaveToSession(udtEHSAccount_Rectify, FuncCode)

            ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [Start][Winnie] Step 0
            Session(SESS_DefaultSetCCCode) = True
            ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [End][Winnie] Step 0

            BindPersonalInfo(udtEHSAccount, udtEHSAccount_Amendment, udtEHSAccount_Rectify, txtDocCode.Text, True)

        ElseIf udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.SpecialAccount Then
            '------------------------------------------------------------
            'Special account
            '------------------------------------------------------------
            'Audit Log
            Me.udtAuditLogEntry.AddDescripton("IdentityNum", udtEHSAccount.EHSPersonalInformationList.Filter(txtDocCode.Text).IdentityNum)
            Me.udtAuditLogEntry.AddDescripton("Account_ID", udtEHSAccount.VoucherAccID.Trim)
            Me.udtAuditLogEntry.AddDescripton("Doc_Code", txtDocCode.Text)
            Me.udtAuditLogEntry.AddDescripton("Account_Source", udtEHSAccount.AccountSourceString)
            Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00006, AuditLogDesc.EditEHSAccount)

            Session(SESS_InputMode) = ActionModel.Amending
            SetAccountBtn(Session(SESS_InputMode))

            'Create a copy of original EHSAccount for rectification
            udtEHSAccount_Rectify = udtEHSAccount
            udteHSAccountMaintBLL.EHSAccount_Rectify_SaveToSession(udtEHSAccount_Rectify, FuncCode)

            ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [Start][Winnie] Step 0
            Session(SESS_DefaultSetCCCode) = True
            ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [End][Winnie] Step 0

            BindPersonalInfo(udtEHSAccount, Nothing, udtEHSAccount_Rectify, txtDocCode.Text, True)

        ElseIf udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.TemporaryAccount Then
            '------------------------------------------------------------
            'Temporary account
            '------------------------------------------------------------
            'Audit Log
            Me.udtAuditLogEntry.AddDescripton("Account_ID", udtEHSAccount.VoucherAccID.Trim)
            Me.udtAuditLogEntry.AddDescripton("Doc_Code", txtDocCode.Text)
            Me.udtAuditLogEntry.AddDescripton("Account_Source", udtEHSAccount.AccountSourceString)
            Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00006, AuditLogDesc.EditEHSAccount)

            Session(SESS_InputMode) = ActionModel.Amending
            SetAccountBtn(Session(SESS_InputMode))

            'Create a copy of original EHSAccount for rectification
            udtEHSAccount_Rectify = udtEHSAccount
            udteHSAccountMaintBLL.EHSAccount_Rectify_SaveToSession(udtEHSAccount_Rectify, FuncCode)

            ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [Start][Winnie] Step 0
            Session(SESS_DefaultSetCCCode) = True
            ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [End][Winnie] Step 0

            BindPersonalInfo(udtEHSAccount, Nothing, udtEHSAccount_Rectify, txtDocCode.Text, True)

        End If

        Session(SESS_RequirePageLoadPersonalBind) = True
    End Sub

    Protected Sub btnWithdrawAmendment_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnWithdrawAmendment.Click
        'audit log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00014, AuditLogDesc.ClickWithDrawAdmendmentButton)

        Session(SESS_PopupActionMode) = PopupActionModel.Withdraw

        Me.ModalPopupExtenderCancelAmend.Show()

        'rebind after post back
        udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FuncCode)
        udtEHSAccount_Amendment = Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FuncCode)
        BindPersonalInfo(udtEHSAccount, udtEHSAccount_Amendment, Nothing, txtDocCode.Text, True)
        Session(SESS_RequirePageLoadPersonalBind) = False
    End Sub

    Protected Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSave.Click
        Me.udcMsgBox.Visible = False
        Dim blnProceed As Boolean = True

        Me.udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FuncCode)
        Me.udtEHSAccount_Rectify = Me.udteHSAccountMaintBLL.EHSAccount_Rectify_GetFromSession(FuncCode)
        Me.udtEHSAccount_Amendment = Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FuncCode)

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("Account_ID", udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("Doc_Code", txtDocCode.Text.Trim)
        Me.udtAuditLogEntry.AddDescripton("Account_Source", udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.AddDescripton("IdentityNum", udtEHSAccount.EHSPersonalInformationList.Filter(txtDocCode.Text.Trim).IdentityNum)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00007, AuditLogDesc.SaveEHSAccount)

        Select Case Me.txtDocCode.Text.Trim
            Case DocTypeModel.DocTypeCode.HKIC
                Dim udcInputHKIC As ucInputHKID = Me.ucInputDocumentType.GetHKICControl

                If udcInputHKIC.CCCodeIsEmpty Then
                    udcInputHKIC.SetCnameAmend(String.Empty)
                    Me.udcCCCode.Clean()
                    Me.udcCCCode.GetChineseName(FunctionCode, False)
                Else
                    Dim mode As ActionModel = Session(SESS_InputMode)
                    Dim DocInputMode As ucInputDocTypeBase.BuildMode
                    Dim blnValid As Boolean = True

                    If mode = ActionModel.Amending Then
                        'For temp/special account
                        DocInputMode = ucInputDocTypeBase.BuildMode.Modification_OneSide
                        blnValid = udcInputHKIC.IsValidCCCodeNewInput()
                    ElseIf mode = ActionModel.Amending_withOriginal Then
                        'For amendment of validated account
                        DocInputMode = ucInputDocTypeBase.BuildMode.Modification
                        blnValid = udcInputHKIC.IsValidCCCodeInput()
                    End If

                    If blnValid Then
                        'Check CCCode
                        ' If CCCode is changed (session value <> input value) => pop up CCCode Panel
                        If Me.NeedPopupCCCodeDialog(DocInputMode, DocTypeModel.DocTypeCode.HKIC) Then
                            Me.ucInputDocumentType_SelectChineseName_HKIC(DocInputMode, udcInputHKIC, DocTypeModel.DocTypeCode.HKIC, Nothing, Nothing)
                            blnProceed = False

                        End If
                    Else
                        Me.udcMsgBox.AddMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00039)
                        udcInputHKIC.SetCCCodeError(True)
                        blnProceed = False

                    End If

                End If

                If blnProceed Then
                    blnProceed = Me.ValidateRectifyDetail_HKID(udtEHSAccount_Rectify, udtAuditLogEntry)
                End If

            Case DocTypeModel.DocTypeCode.EC
                blnProceed = Me.ValidateRectifyDetail_EC(udtEHSAccount_Rectify, udtAuditLogEntry)

            Case DocTypeModel.DocTypeCode.HKBC

                blnProceed = Me.ValidateRectifyDetail_HKBC(udtEHSAccount_Rectify, udtAuditLogEntry)

            Case DocTypeModel.DocTypeCode.ADOPC

                blnProceed = Me.ValidateRectifyDetail_Adopt(udtEHSAccount_Rectify, udtAuditLogEntry)

            Case DocTypeModel.DocTypeCode.DI

                blnProceed = Me.ValidateRectifyDetail_DI(udtEHSAccount_Rectify, udtAuditLogEntry)

            Case DocTypeModel.DocTypeCode.ID235B

                blnProceed = Me.ValidateRectifyDetail_ID235B(udtEHSAccount_Rectify, udtAuditLogEntry)

            Case DocTypeModel.DocTypeCode.REPMT

                blnProceed = Me.ValidateRectifyDetail_ReEntryPermit(udtEHSAccount_Rectify, udtAuditLogEntry)

            Case DocTypeModel.DocTypeCode.VISA

                blnProceed = Me.ValidateRectifyDetail_Visa(udtEHSAccount_Rectify, udtAuditLogEntry)

                ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
            Case DocTypeModel.DocTypeCode.OW,
                DocTypeModel.DocTypeCode.RFNo8

                blnProceed = Me.ValidateRectifyDetail_OW_RFNo8(udtEHSAccount_Rectify, udtAuditLogEntry)
                ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]
            Case DocTypeModel.DocTypeCode.TW
                blnProceed = Me.ValidateRectifyDetail_TW(udtEHSAccount_Rectify, udtAuditLogEntry)

                ' CRE19-001 (VSS 2019) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
            Case DocTypeModel.DocTypeCode.OC,
                DocTypeModel.DocTypeCode.IR,
                DocTypeModel.DocTypeCode.HKP,
                DocTypeModel.DocTypeCode.OTHER
                ' CRE19-001 (VSS 2019) [End][Winnie]
                blnProceed = Me.ValidateRectifyDetail_OTHER(udtEHSAccount_Rectify, udtAuditLogEntry)

                ' CRE20-0022 (Immu record) [Start][Martin]
            Case DocTypeModel.DocTypeCode.CCIC
                blnProceed = Me.ValidateRectifyDetail_CCIC(udtEHSAccount_Rectify, udtAuditLogEntry)
            Case DocTypeModel.DocTypeCode.ROP140
                Dim udcInputROP140 As ucInputROP140 = Me.ucInputDocumentType.GetROP140Control

                If udcInputROP140.CCCodeIsEmpty Then
                    udcInputROP140.SetCnameAmend(String.Empty)
                    Me.udcCCCode.Clean()
                    Me.udcCCCode.GetChineseName(FunctionCode, False)
                Else
                    Dim mode As ActionModel = Session(SESS_InputMode)
                    Dim DocInputMode As ucInputDocTypeBase.BuildMode
                    Dim blnValid As Boolean = True

                    If mode = ActionModel.Amending Then
                        'For temp/special account
                        DocInputMode = ucInputDocTypeBase.BuildMode.Modification_OneSide
                        blnValid = udcInputROP140.IsValidCCCodeNewInput()
                    ElseIf mode = ActionModel.Amending_withOriginal Then
                        'For amendment of validated account
                        DocInputMode = ucInputDocTypeBase.BuildMode.Modification
                        blnValid = udcInputROP140.IsValidCCCodeInput()
                    End If

                    If blnValid Then
                        'Check CCCode
                        ' If CCCode is changed (session value <> input value) => pop up CCCode Panel
                        If Me.NeedPopupCCCodeDialog(DocInputMode, DocTypeModel.DocTypeCode.ROP140) Then
                            Me.ucInputDocumentType_SelectChineseName_HKIC(DocInputMode, udcInputROP140, DocTypeModel.DocTypeCode.ROP140, Nothing, Nothing)
                            blnProceed = False

                        End If
                    Else
                        Me.udcMsgBox.AddMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00039)
                        udcInputROP140.SetCCCodeError(True)
                        blnProceed = False

                    End If
                End If

                If blnProceed Then
                    blnProceed = Me.ValidateRectifyDetail_ROP140(udtEHSAccount_Rectify, udtAuditLogEntry)
                End If
            Case DocTypeModel.DocTypeCode.PASS
                blnProceed = Me.ValidateRectifyDetail_PASS(udtEHSAccount_Rectify, udtAuditLogEntry)

            Case DocTypeModel.DocTypeCode.ISSHK, DocTypeModel.DocTypeCode.ET
                blnProceed = Me.ValidateRectifyDetail_ISSHK(udtEHSAccount_Rectify, udtAuditLogEntry)

            Case DocTypeModel.DocTypeCode.MEP, DocTypeModel.DocTypeCode.TWMTP, DocTypeModel.DocTypeCode.TWPAR, DocTypeModel.DocTypeCode.TWVTD, _
                DocTypeModel.DocTypeCode.TWNS, DocTypeModel.DocTypeCode.MD, DocTypeModel.DocTypeCode.MP, DocTypeModel.DocTypeCode.TD, _
                DocTypeModel.DocTypeCode.CEEP, DocTypeModel.DocTypeCode.DS
                blnProceed = Me.ValidateRectifyDetail_Common(udtEHSAccount_Rectify, udtAuditLogEntry)


                ' CRE20-0022 (Immu record) [End][Martin]


        End Select

        If blnProceed Then
            Dim udtClaimRulesBLL As New ClaimRules.ClaimRulesBLL
            Dim udtSM As SystemMessage = Nothing
            Dim udtEligibleResult As ClaimRules.ClaimRulesBLL.EligibleResult = Nothing

            'Check Rectify back end logic (For amendment of validated account only)
            If Me.udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                udtSM = udtClaimRulesBLL.CheckRectifyEHSAccountInBackOffice(udtEHSAccount.SchemeCode.Trim, Me.txtDocCode.Text.Trim, udtEHSAccount, udtEHSAccount_Amendment, udtEHSAccount_Rectify, udtEligibleResult)
            End If

            Dim blnShowDeclaration As Boolean = False

            If IsNothing(udtSM) Then
                If Not IsNothing(udtEligibleResult) Then
                    If udtEligibleResult.HandleMethod = ClaimRules.ClaimRulesBLL.HandleMethodENum.Declaration Then
                        blnShowDeclaration = True
                    Else
                        blnShowDeclaration = False
                    End If
                Else
                    blnShowDeclaration = False
                End If

                If Not blnShowDeclaration Then
                    Me.mveHSAccount.ActiveViewIndex = intConfirm
                    panFilter.Visible = False

                    SetupPageContent(Nothing, Nothing, udtEHSAccount_Rectify, False)

                    Session(SESS_RequirePageLoadPersonalBind) = True
                    'save to session (Rectify)
                    Me.udteHSAccountMaintBLL.EHSAccount_Rectify_SaveToSession(Me.udtEHSAccount_Rectify, FuncCode)
                    Me.udtAuditLogEntry.AddDescripton("Account_ID", udtEHSAccount.VoucherAccID.Trim)
                    Me.udtAuditLogEntry.AddDescripton("Doc_Code", txtDocCode.Text.Trim)
                    Me.udtAuditLogEntry.AddDescripton("Account_Source", udtEHSAccount.AccountSourceString)
                    Me.udtAuditLogEntry.AddDescripton("IdentityNum", udtEHSAccount.EHSPersonalInformationList.Filter(txtDocCode.Text.Trim).IdentityNum)
                    Me.udtAuditLogEntry.WriteEndLog(LogID.LOG00009, AuditLogDesc.ValidateAccountDetailInfoComplete)
                End If
            Else
                Me.udcMsgBox.Clear()
                Me.udcMsgBox.AddMessage(udtSM)
                Me.udtAuditLogEntry.AddDescripton("Account_ID", udtEHSAccount.VoucherAccID.Trim)
                Me.udtAuditLogEntry.AddDescripton("Doc_Code", txtDocCode.Text.Trim)
                Me.udtAuditLogEntry.AddDescripton("Account_Source", udtEHSAccount.AccountSourceString)
                Me.udtAuditLogEntry.AddDescripton("IdentityNum", udtEHSAccount.EHSPersonalInformationList.Filter(txtDocCode.Text.Trim).IdentityNum)
                Me.udcMsgBox.BuildMessageBox(strValidationFail, Me.udtAuditLogEntry, LogID.LOG00010, AuditLogDesc.ValidateAccountDetailInfoFail)
            End If
        Else
            Me.udtAuditLogEntry.AddDescripton("Account_ID", udtEHSAccount.VoucherAccID.Trim)
            Me.udtAuditLogEntry.AddDescripton("Doc_Code", txtDocCode.Text.Trim)
            Me.udtAuditLogEntry.AddDescripton("Account_Source", udtEHSAccount.AccountSourceString)
            Me.udcMsgBox.BuildMessageBox(strValidationFail, Me.udtAuditLogEntry, LogID.LOG00010, AuditLogDesc.ValidateAccountDetailInfoFail)
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00021, AuditLogDesc.CancelEdit)

        Me.udcMsgBox.Clear()
        Me.udcInfoMsgBox.Clear()

        'remove the controls added in Page load
        Me.ucInputDocumentType.Clear()

        If Session(SESS_InputMode) = ActionModel.Amending_withOriginal Then
            '------------------------------------------------------------
            'Amendment of validated account
            '------------------------------------------------------------
            Session(SESS_InputMode) = ActionModel.ReadOnly_withOriginal
            SetAccountBtn(Session(SESS_InputMode))

            udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FuncCode)
            udtEHSAccount_Amendment = Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FuncCode)
            BindPersonalInfo(udtEHSAccount, udtEHSAccount_Amendment, Nothing, txtDocCode.Text, True)

        ElseIf Session(SESS_InputMode) = ActionModel.Amending Then
            '------------------------------------------------------------
            'Special account
            '------------------------------------------------------------
            Session(SESS_InputMode) = ActionModel.ReadOnly
            SetAccountBtn(Session(SESS_InputMode))

            udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FuncCode)
            BindPersonalInfo(udtEHSAccount, Nothing, Nothing, txtDocCode.Text, True)

        End If

        Session(SESS_RequirePageLoadPersonalBind) = False

        If udtEHSAccount.EHSPersonalInformationList.Filter(Me.txtDocCode.Text.Trim).Deceased Then
            Session(SESS_DetailPageShowDeceased) = True
        Else
            Session(SESS_DetailPageShowDeceased) = False
        End If

    End Sub

    Protected Sub ibtnRemoveTempAccountByBO_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("Doc_Code", txtDocCode.Text.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.AddDescripton("IdentityNum", udtEHSAccount.EHSPersonalInformationList.Filter(txtDocCode.Text).IdentityNum)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00030, AuditLogDesc.RemoveTempAcctClick)

        Me.udcMsgBox.Clear()

        udtSM = New Common.ComObject.SystemMessage("010301", SeverityCode.SEVQ, MsgCode.MSG00002)

        Dim strMsg As String
        strMsg = udtSM.GetMessage
        Me.lblConfirmCanel.Text = strMsg

        Session(SESS_PopupActionMode) = PopupActionModel.Remove

        Me.ModalPopupExtenderCancelAmend.Show()

        'rebind after post back
        udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FuncCode)
        BindPersonalInfo(udtEHSAccount, Nothing, Nothing, txtDocCode.Text, True)
        Session(SESS_RequirePageLoadPersonalBind) = False

    End Sub

#End Region

#Region "View 3 - Confirmation"
    Private Sub ibtnConfirmAmendedAccount_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnConfirmAmendedAccount.Click
        Me.ucInputDocumentType.Clear()
        Me.udcMsgBox.Clear()
        Me.udcInfoMsgBox.Clear()

        Dim udtHCVUUserBLL As New HCVUUserBLL
        Dim udtHCVUUser As HCVUUserModel = udtHCVUUserBLL.GetHCVUUser
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)

        Dim strUpdateBy As String = udtHCVUUser.UserID
        udtSM = Nothing

        'Retrieve Rectified eHS account
        udtEHSAccount_Rectify = udteHSAccountMaintBLL.EHSAccount_Rectify_GetFromSession(FuncCode)

        'Audit Log --------------------------Log all eHS accout personal particulars------------------------
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        With udtEHSAccount_Rectify
            Me.udtAuditLogEntry.AddDescripton("AccountID", .VoucherAccID)
            Me.udtAuditLogEntry.AddDescripton("AccountSource", .AccountSourceString)
            With udtEHSAccount_Rectify.EHSPersonalInformationList(0)
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
        '---------------------------------------------------------------------------------------------------------------

        Try
            udtEHSAccount = udteHSAccountMaintBLL.EHSAccountGetFromSession(FuncCode)

            If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                '----------------------------------------------
                'Amendment of validated account
                '----------------------------------------------
                udtEHSAccount_Amendment = udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FuncCode)
                udtEHSAccount_Rectify.VoucherAccID = udtEHSAccount.VoucherAccID

                Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00011, AuditLogDesc.ConfirmSaveEHSAccount)

                udtSM = udteHSAccountMaintBLL.RectifyEHSAccount(udtEHSAccount, udtEHSAccount_Amendment, udtEHSAccount_Rectify, txtDocCode.Text, strUpdateBy)

                If Not IsNothing(udtSM) Then
                    Me.udcMsgBox.AddMessage(udtSM)
                    Me.udcMsgBox.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00013, AuditLogDesc.ConfirmSaveEHSAccountFail)
                Else
                    'create complete message
                    Me.udtSM = New Common.ComObject.SystemMessage("010303", "I", LogID.LOG00001)

                    Me.udcInfoMsgBox.AddMessage(udtSM)
                    Me.udcInfoMsgBox.BuildMessageBox()
                    Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Complete

                    Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount_Rectify.VoucherAccID)
                    Me.udtAuditLogEntry.AddDescripton("DocCode", txtDocCode.Text)
                    Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount_Rectify.AccountSourceString)
                    Me.udtAuditLogEntry.AddDescripton("IdentityNum", udtEHSAccount_Rectify.EHSPersonalInformationList.Filter(txtDocCode.Text).IdentityNum)
                    Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00012, AuditLogDesc.ConfirmSaveEHSAccountComplete)
                    Me.mveHSAccount.ActiveViewIndex = intComplete
                    panFilter.Visible = False
                End If

            ElseIf udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.SpecialAccount Then
                '----------------------------------------------
                'Special account
                '----------------------------------------------
                udtEHSAccount_Rectify = udteHSAccountMaintBLL.EHSAccount_Rectify_GetFromSession(FuncCode)
                Dim blnErr As Boolean = False

                Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00038, AuditLogDesc.ConfirmModifiedSpecialEHSAccount)

                udtSM = Me.udteHSAccountMaintBLL.AmendEHSAccount(udtEHSAccount, udtEHSAccount_Rectify, Me.txtDocCode.Text.Trim, strUpdateBy, blnErr)

                If blnErr Then
                    If Not IsNothing(udtSM) Then
                        Me.udcMsgBox.AddMessage(udtSM)
                        Me.udcMsgBox.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00040, AuditLogDesc.SaveModifiedSpecialEHSAccountFail)
                    End If
                Else
                    If Not IsNothing(udtSM) Then
                        Me.udcInfoMsgBox.AddMessage(udtSM)
                        Me.udcInfoMsgBox.BuildMessageBox()
                        Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Complete

                        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount_Rectify.VoucherAccID)
                        Me.udtAuditLogEntry.AddDescripton("DocCode", txtDocCode.Text)
                        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount_Rectify.AccountSourceString)
                        Me.udtAuditLogEntry.AddDescripton("IdentityNum", udtEHSAccount.EHSPersonalInformationList.Filter(txtDocCode.Text).IdentityNum)
                        Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00039, AuditLogDesc.SaveModifiedSpecialEHSAccountComplete)
                        Me.mveHSAccount.ActiveViewIndex = intComplete
                        panFilter.Visible = False
                    End If
                End If

            ElseIf udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.TemporaryAccount Then
                '----------------------------------------------
                'Temporary account
                '----------------------------------------------
                udtEHSAccount_Rectify = udteHSAccountMaintBLL.EHSAccount_Rectify_GetFromSession(FuncCode)
                Dim blnErr As Boolean = False

                Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00035, AuditLogDesc.ConfirmModifiedTempEHSAccount)

                udtSM = Me.udteHSAccountMaintBLL.AmendEHSAccount(udtEHSAccount, udtEHSAccount_Rectify, Me.txtDocCode.Text.Trim, strUpdateBy, blnErr)

                If blnErr Then
                    If Not IsNothing(udtSM) Then
                        Me.udcMsgBox.AddMessage(udtSM)
                        Me.udcMsgBox.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00037, AuditLogDesc.SaveModifiedTempEHSAccountFail)
                    End If
                Else
                    If Not IsNothing(udtSM) Then
                        Me.udcInfoMsgBox.AddMessage(udtSM)
                        Me.udcInfoMsgBox.BuildMessageBox()
                        Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Complete

                        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount_Rectify.VoucherAccID)
                        Me.udtAuditLogEntry.AddDescripton("DocCode", txtDocCode.Text)
                        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount_Rectify.AccountSourceString)
                        Me.udtAuditLogEntry.AddDescripton("IdentityNum", udtEHSAccount_Rectify.EHSPersonalInformationList.Filter(txtDocCode.Text).IdentityNum)
                        Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00036, AuditLogDesc.SaveModifiedTempEHSAccountComplete)
                        Me.mveHSAccount.ActiveViewIndex = intComplete
                        panFilter.Visible = False
                    End If
                End If
            End If

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                Me.udtSM = New Common.ComObject.SystemMessage("990001", "D", eSQL.Message)
                Me.udcMsgBox.AddMessage(Me.udtSM)
                Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount_Rectify.VoucherAccID)
                Me.udtAuditLogEntry.AddDescripton("DocCode", txtDocCode.Text)
                Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount_Rectify.AccountSourceString)
                Me.udtAuditLogEntry.AddDescripton("IdentityNum", udtEHSAccount_Rectify.EHSPersonalInformationList.Filter(txtDocCode.Text).IdentityNum)
                Me.udcMsgBox.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00013, AuditLogDesc.ConfirmSaveEHSAccountFail)
            Else
                Throw eSQL
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub ibtnConfirmCancelAmendedAccont_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnConfirmCancelAmendedAccont.Click
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00022, AuditLogDesc.BackToDetail)

        Me.mveHSAccount.ActiveViewIndex = intAccountDetails
        panFilter.Visible = False

        'Since SESS_RequirePageLoadPersonalBind has already be set to 'True'
        'Clear the controls added in page load first
        ucInputDocumentType.Clear()

        Me.udcInfoMsgBox.Clear()

        udtEHSAccount = udteHSAccountMaintBLL.EHSAccountGetFromSession(FuncCode)
        udtEHSAccount_Rectify = udteHSAccountMaintBLL.EHSAccount_Rectify_GetFromSession(FuncCode)

        If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
            'Amendment of validated account
            'show both original and rectified records in 2 sides
            BindPersonalInfo(udtEHSAccount, Nothing, udtEHSAccount_Rectify, txtDocCode.Text, True)
        ElseIf udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.SpecialAccount Then
            'Special Account
            'only show rectified record in one side
            BindPersonalInfo(udtEHSAccount_Rectify, Nothing, Nothing, txtDocCode.Text, True)
        ElseIf udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.TemporaryAccount Then
            'Temporary Account
            'only show rectified record in one side
            BindPersonalInfo(udtEHSAccount_Rectify, Nothing, Nothing, txtDocCode.Text, True)
        End If

    End Sub
#End Region

#Region "View 4 - Complete"

    Private Sub ibtnReturn_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnReturn.Click
        Me.ucInputDocumentType.Clear()
        Me.udcMsgBox.Clear()
        Me.udcInfoMsgBox.Clear()

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00020, AuditLogDesc.ReturnToSearchResult)

        Session(SESS_RequirePageLoadPersonalBind) = False

        Me.mveHSAccount.ActiveViewIndex = intShowList
        panFilter.Visible = True

        'LoadRectifyRecord(Session(SESS_ShowSpecialAccount))
        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
        ' -------------------------------------------------------------------------
        _blnSkipInputToken = True
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

        LoadRectifyRecord()
    End Sub

#End Region

#Region "Muilt View Function"

    Private Sub mveHSAccount_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mveHSAccount.ActiveViewChanged
        udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FuncCode)
        udtEHSAccount_Amendment = Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FuncCode)

        Select Case Me.mveHSAccount.ActiveViewIndex
            'Case intSearch
            '    ClearSession(True)
            Case intShowList
                Me.udcInfoMsgBox.Clear()
                'Me.udcMsgBox.Clear()
                'Me.udcInfoMsgBox.Clear()
                'ClearSession(True)
            Case intAccountDetails
                'Me.udcInfoMsgBox.Clear()
                'Me.SetupPageContent(udtEHSAccount, udtEHSAccount_Amendment)

                If udtEHSAccount.EHSPersonalInformationList.Filter(Me.txtDocCode.Text.Trim).Deceased Then
                    Session(SESS_DetailPageShowDeceased) = True
                Else
                    Session(SESS_DetailPageShowDeceased) = False
                End If

            Case intConfirm
                Me.udcMsgBox.Clear()
                Me.udcInfoMsgBox.Clear()

                udtSM = New SystemMessage(CommonFuncCode, SeverityCode.SEVI, MsgCode.MSG00021)
                Me.udcInfoMsgBox.AddMessage(udtSM)
                Me.udcInfoMsgBox.BuildMessageBox()
                Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Information
        End Select
    End Sub

#End Region

#Region "Get eHSAccount Function"
    Public Sub LoadRectifyRecord()

        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("IdentityNum", txtIdentityNum.Text)
        Me.udtAuditLogEntry.AddDescripton("ShowSpecial", cboFilterSpecialAccount.Checked.ToString())
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00001, AuditLogDesc.SearchEHSAccount)

        Me.panAccountList.Visible = True
        Me.panAccList.Visible = True

        Me.udcInfoMsgBox.Clear()
        Me.udcMsgBox.Clear()

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
        ' -------------------------------------------------------------------------

        ''clear the session
        'Me.Session(SESS_RectifyAccList) = Nothing

        ''Default : Not to show special account
        'Dim blnShowSpecialAcc As Boolean = False
        'Dim dt As DataTable
        'Dim strIdentityNum As String = ""
        'Dim strAdoptionPrefixNum As String = ""

        'If Session(SESS_TurnOnAccountRectificationFilter) = "Y" Then
        '    If Not IsNothing(Me.Session(SESS_FilterIdentityNum)) Then
        '        strIdentityNum = Session(SESS_FilterIdentityNum)
        '    End If
        '    If Not IsNothing(Me.Session(SESS_FilterSpecialAccount)) Then
        '        blnShowSpecialAcc = Me.Session(SESS_FilterSpecialAccount)
        '    End If
        '    If Not IsNothing(Me.Session(SESS_FilterAdoptionPrefixNum)) Then
        '        strAdoptionPrefixNum = Me.Session(SESS_FilterAdoptionPrefixNum)
        '    End If
        'Else
        '    If Not IsNothing(Me.Session(SESS_ShowSpecialAccount)) Then
        '        blnShowSpecialAcc = Me.Session(SESS_ShowSpecialAccount)
        '    End If
        'End If

        ''If Not IsNothing(Me.Session(SESS_ShowSpecialAccount)) Then
        '' blnShowSpecialAcc = Me.Session(SESS_ShowSpecialAccount)
        '' End If

        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
        ' -------------------------------------------------------------------------
        Dim enumSearchResult As SearchResultEnum
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

        Try
            '-----------------------------------------------
            'Retrieve Validated Accounts 
            '-----------------------------------------------

            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
            ' -------------------------------------------------------------------------
            'dt = udteHSAccountMaintBLL.getRectifyList(blnShowSpecialAcc, strIdentityNum, strAdoptionPrefixNum)

            'Me.Session(SESS_RectifyAccList) = dt

            'If dt.Rows.Count > 0 Then

            '    Me.mveHSAccount.ActiveViewIndex = intShowList
            '    Me.panAccountList.Visible = True

            '    Dim strHeaderTitle As String = String.Empty
            '    strHeaderTitle = Me.GetGlobalResourceObject("Text", "VoucherAccountID") + " / " + Me.GetGlobalResourceObject("Text", "RefNo")
            '    Me.gvAcctList.Columns.Item(3).HeaderText = strHeaderTitle
            '    Me.GridViewDataBind(Me.gvAcctList, dt, "IdentityNum", "ASC", False)

            '    'Log record exist
            '    If Not IsNothing(Me.udtAuditLogEntry) Then
            '        Me.udtAuditLogEntry.AddDescripton("NoOfRecord", dt.Rows.Count)
            '        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00002, AuditLogDesc.SearchEHSAccountComplete)
            '    End If
            'Else
            '    Me.mveHSAccount.ActiveViewIndex = intShowList

            '    Me.panAccountList.Visible = False
            '    udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Information
            '    udcInfoMsgBox.AddMessage(FunctCode.FUNT990000, "I", LogID.LOG00001)
            '    udcInfoMsgBox.BuildMessageBox()

            '    'Log record no record found
            '    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00003, AuditLogDesc.NoRecordFound)
            'End If

            'enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, udcMsgBox, udcInfoMsgBox)
            If _blnSkipInputToken = True Then
                enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, Nothing, udcInfoMsgBox, False, True)
            Else
                enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, Nothing, udcInfoMsgBox)
            End If
            'enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, Nothing, udcInfoMsgBox)

            Select Case enumSearchResult
                Case SearchResultEnum.Success
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00002, AuditLogDesc.SearchEHSAccountComplete)

                Case SearchResultEnum.ValidationFail
                    ' Audit Log has been handled in [SF_ValidateSearch] method

                Case SearchResultEnum.NoRecordFound
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditLogDesc.NoRecordFound)

                Case SearchResultEnum.OverResultList1stLimit_PopUp
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00042, AuditLogDesc.SearchEHSAccountFail)
                    Me.panAccountList.Visible = False

                    'udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Information
                    'udcInfoMsgBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00002)
                    'udcInfoMsgBox.BuildMessageBox()

                Case SearchResultEnum.OverResultList1stLimit_Alert
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00042, AuditLogDesc.SearchEHSAccountFail)
                    Me.panAccountList.Visible = False

                    udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Information
                    udcInfoMsgBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00002)
                    udcInfoMsgBox.BuildMessageBox()

                Case SearchResultEnum.OverResultListOverrideLimit
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00042, AuditLogDesc.SearchEHSAccountFail)
                    Me.panAccountList.Visible = False

                    udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Information
                    udcInfoMsgBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00002)
                    udcInfoMsgBox.BuildMessageBox()

                Case Else
                    Throw New Exception("Error: Class = [HCVU.eHSAccountRectification], Method = [LoadRectifyRecord], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")

            End Select

            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
            ' -------------------------------------------------------------------------
            'Catch eSQL As SqlClient.SqlException
            'If eSQL.Number = 50000 Then

            'Dim strmsg As String
            'strmsg = eSQL.Message

            'udtSM = New Common.ComObject.SystemMessage("990001", "D", strmsg)
            'udcInfoMsgBox.AddMessage(udtSM)
            'udcInfoMsgBox.BuildMessageBox()

            'Me.mveHSAccount.ActiveViewIndex = intShowList

            'If blnShowSpecialAcc Then
            '    Me.panAccountList.Visible = True
            '    Me.panAccList.Visible = False
            'Else
            '    'default view (no special account)
            '    Me.panAccountList.Visible = False
            'End If

            ''Log record exist
            'If Not IsNothing(Me.udtAuditLogEntry) Then
            '    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00029, AuditLogDesc.SearchEHSAccountTooManyRecords)
            'End If
            'Else

            'udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00042, AuditLogDesc.SearchEHSAccountFail)
            'Throw eSQL
            'End If
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00042, AuditLogDesc.SearchEHSAccountFail)
            Throw ex
        End Try

    End Sub



    Private Function GeteHSAcc(ByVal strAccountID As String, ByVal strAccountSource As String) As Boolean
        Dim blnRes As Boolean = False
        udteHSAccountMaintBLL.EHSAccountRemoveFromSession(FuncCode)
        udteHSAccountMaintBLL.EHSAccount_Amend_RemoveFromSession(FuncCode)

        Select Case strAccountSource
            Case EHealthAccountType.Temporary
                udtEHSAccount = udtEHSAccountBLL.LoadTempEHSAccountByVRID(strAccountID)

            Case EHealthAccountType.Special
                udtEHSAccount = udtEHSAccountBLL.LoadSpecialEHSAccountByVRID(strAccountID)

            Case EHealthAccountType.Validated
                udtEHSAccount = udtEHSAccountBLL.LoadEHSAccountByVRID(strAccountID)

            Case EHealthAccountType.Invalid

        End Select

        udtEHSAccount_Amendment = udtEHSAccountBLL.LoadAmendingEHSAccountByVRID(strAccountID, Me.txtDocCode.Text.Trim)

        If Not IsNothing(udtEHSAccount) Then
            Me.udteHSAccountMaintBLL.EHSAccountSaveToSession(udtEHSAccount, FuncCode)
            Me.udteHSAccountMaintBLL.EHSAccount_Amend_SaveToSession(udtEHSAccount_Amendment, FuncCode)
            blnRes = True
        End If
        Return blnRes
    End Function

    Private Sub BindPersonalInfo(ByVal _udtEHSAccount As EHSAccountModel, ByVal _udtEHSAccount_Amendment As EHSAccountModel, ByVal _udtEHSAccount_Rectify As EHSAccountModel, ByVal strDocCode As String, ByVal activeChanged As Boolean)
        Dim blnRes As Boolean = False
        Dim mode As ActionModel

        If Not IsNothing(_udtEHSAccount) AndAlso Not strDocCode.Equals(String.Empty) Then

            If Not IsNothing(Session(SESS_InputMode)) Then
                mode = CType(Session(SESS_InputMode), ActionModel)

                Me.ucReadOnlyDocumnetType.Visible = False
                Me.ucInputDocumentType.Visible = False
                Me.ucInputDocumentType.ActiveViewChanged = activeChanged
                Select Case mode
                    Case ActionModel.ReadOnly_withOriginal
                        'For amendment of validated account
                        'Show both original and amending record, ReadOnly

                        Me.ucInputDocumentType.DocType = strDocCode
                        Me.ucInputDocumentType.EHSAccountOriginal = _udtEHSAccount
                        Me.ucInputDocumentType.EHSAccountAmend = _udtEHSAccount_Amendment
                        Me.ucInputDocumentType.Mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly
                        Me.ucInputDocumentType.FillValue = True
                        Me.ucInputDocumentType.UseDefaultAmendingHeader = True
                        Me.ucInputDocumentType.Built()

                        Me.ucInputDocumentType.Visible = True
                        Me.ucReadOnlyDocumnetType.Visible = False

                        Me.phReadOnlyAccountInfo.Controls.Clear()
                        Dim ucReadOnlyAccountInfo As ucReadOnlyAccountInfo = Me.LoadControl("~/UIControl/ucReadOnlyAccountInfo.ascx")
                        ucReadOnlyAccountInfo.EHSAccount = _udtEHSAccount
                        ucReadOnlyAccountInfo.Width = 220
                        ucReadOnlyAccountInfo.ShowPublicEnquiryStatus = False
                        Me.phReadOnlyAccountInfo.Controls.Add(ucReadOnlyAccountInfo)

                        Me.panDocumentType.Visible = False
                    Case ActionModel.Amending_withOriginal
                        'For amendment of validated account
                        'Show both original and amending record, Amending

                        Me.ucInputDocumentType.DocType = strDocCode
                        Me.ucInputDocumentType.EHSAccountOriginal = _udtEHSAccount
                        Me.ucInputDocumentType.EHSAccountAmend = _udtEHSAccount_Rectify
                        Me.ucInputDocumentType.Mode = ucInputDocTypeBase.BuildMode.Modification
                        Me.ucInputDocumentType.FillValue = True
                        Me.ucInputDocumentType.UseDefaultAmendingHeader = True
                        Me.ucInputDocumentType.AuditLogEntry = New AuditLogEntry(FuncCode, Me)
                        Me.ucInputDocumentType.Built()

                        Me.ucInputDocumentType.Visible = True
                        Me.ucReadOnlyDocumnetType.Visible = False

                        blnRes = True

                        Me.phReadOnlyAccountInfo.Controls.Clear()
                        Dim ucReadOnlyAccountInfo As ucReadOnlyAccountInfo = Me.LoadControl("~/UIControl/ucReadOnlyAccountInfo.ascx")
                        ucReadOnlyAccountInfo.EHSAccount = _udtEHSAccount
                        ucReadOnlyAccountInfo.Width = 220
                        ucReadOnlyAccountInfo.ShowPublicEnquiryStatus = False
                        Me.phReadOnlyAccountInfo.Controls.Add(ucReadOnlyAccountInfo)

                        Me.panDocumentType.Visible = False
                    Case ActionModel.ReadOnly
                        'For Special Account
                        'ReadOnly
                        Me.ucReadOnlyDocumnetType.DocumentType = strDocCode
                        Me.ucReadOnlyDocumnetType.EHSPersonalInformation = _udtEHSAccount.EHSPersonalInformationList.Filter(strDocCode)
                        Me.ucReadOnlyDocumnetType.Vertical = True
                        Me.ucReadOnlyDocumnetType.Width = 220
                        Me.ucReadOnlyDocumnetType.Build()

                        Me.ucInputDocumentType.Visible = False
                        Me.ucReadOnlyDocumnetType.Visible = True

                        Me.phReadOnlyAccountInfo.Controls.Clear()
                        Dim ucReadOnlyAccountInfo As ucReadOnlyAccountInfo = Me.LoadControl("~/UIControl/ucReadOnlyAccountInfo.ascx")
                        ucReadOnlyAccountInfo.EHSAccount = _udtEHSAccount
                        ucReadOnlyAccountInfo.Width = 220
                        ucReadOnlyAccountInfo.ShowPublicEnquiryStatus = False
                        Me.phReadOnlyAccountInfo.Controls.Add(ucReadOnlyAccountInfo)

                        Me.panDocumentType.Visible = False
                    Case ActionModel.Amending
                        'For Special Account
                        'Amending

                        Me.ucInputDocumentType.DocType = strDocCode
                        Me.ucInputDocumentType.EHSAccountOriginal = _udtEHSAccount
                        Me.ucInputDocumentType.EHSAccountAmend = Nothing
                        Me.ucInputDocumentType.Mode = ucInputDocTypeBase.BuildMode.Modification_OneSide
                        Me.ucInputDocumentType.FillValue = True
                        Me.ucInputDocumentType.UseDefaultAmendingHeader = True
                        Me.ucInputDocumentType.AuditLogEntry = New AuditLogEntry(FuncCode, Me)
                        Me.ucInputDocumentType.Built()

                        Me.ucInputDocumentType.Visible = True
                        Me.ucReadOnlyDocumnetType.Visible = False

                        blnRes = True

                        Me.phReadOnlyAccountInfo.Controls.Clear()
                        Dim ucReadOnlyAccountInfo As ucReadOnlyAccountInfo = Me.LoadControl("~/UIControl/ucReadOnlyAccountInfo.ascx")
                        ucReadOnlyAccountInfo.EHSAccount = _udtEHSAccount
                        ucReadOnlyAccountInfo.Width = 220
                        ucReadOnlyAccountInfo.ShowPublicEnquiryStatus = False
                        Me.phReadOnlyAccountInfo.Controls.Add(ucReadOnlyAccountInfo)

                        Me.panDocumentType.Visible = True
                        lblDocumentType.Text = udtDocTypeBLL.getAllDocType().Filter(_udtEHSAccount.EHSPersonalInformationList(0).DocCode).DocName
                End Select
            End If

        End If

        Dim blnSetCCCode As Boolean

        If blnRes Then
            If Not IsNothing(Session(SESS_DefaultSetCCCode)) Then
                blnSetCCCode = CBool(Session(SESS_DefaultSetCCCode))

                If blnSetCCCode Then
                    If ucInputDocumentType.DocType.Trim.Equals(DocType.DocTypeModel.DocTypeCode.HKIC) Or ucInputDocumentType.DocType.Trim.Equals(DocType.DocTypeModel.DocTypeCode.ROP140) Then
                        If Not IsNothing(Session(SESS_InputMode)) Then
                            mode = CType(Session(SESS_InputMode), ActionModel)
                        End If

                        Select Case ucInputDocumentType.DocType.Trim
                            Case DocType.DocTypeModel.DocTypeCode.HKIC
                                Dim udcInputHKID As ucInputHKID = Me.ucInputDocumentType.GetHKICControl
                                If Not IsNothing(mode) AndAlso mode = ActionModel.Amending_withOriginal Then
                                    Me.udcCCCode.CCCode1 = IIf(IsNothing(udcInputHKID.CCCode1Amend), String.Empty, udcInputHKID.CCCode1Amend)
                                    Me.udcCCCode.CCCode2 = IIf(IsNothing(udcInputHKID.CCCode2Amend), String.Empty, udcInputHKID.CCCode2Amend)
                                    Me.udcCCCode.CCCode3 = IIf(IsNothing(udcInputHKID.CCCode3Amend), String.Empty, udcInputHKID.CCCode3Amend)
                                    Me.udcCCCode.CCCode4 = IIf(IsNothing(udcInputHKID.CCCode4Amend), String.Empty, udcInputHKID.CCCode4Amend)
                                    Me.udcCCCode.CCCode5 = IIf(IsNothing(udcInputHKID.CCCode5Amend), String.Empty, udcInputHKID.CCCode5Amend)
                                    Me.udcCCCode.CCCode6 = IIf(IsNothing(udcInputHKID.CCCode6Amend), String.Empty, udcInputHKID.CCCode6Amend)
                                Else
                                    Me.udcCCCode.CCCode1 = IIf(IsNothing(udcInputHKID.CCCode1), String.Empty, udcInputHKID.CCCode1)
                                    Me.udcCCCode.CCCode2 = IIf(IsNothing(udcInputHKID.CCCode2), String.Empty, udcInputHKID.CCCode2)
                                    Me.udcCCCode.CCCode3 = IIf(IsNothing(udcInputHKID.CCCode3), String.Empty, udcInputHKID.CCCode3)
                                    Me.udcCCCode.CCCode4 = IIf(IsNothing(udcInputHKID.CCCode4), String.Empty, udcInputHKID.CCCode4)
                                    Me.udcCCCode.CCCode5 = IIf(IsNothing(udcInputHKID.CCCode5), String.Empty, udcInputHKID.CCCode5)
                                    Me.udcCCCode.CCCode6 = IIf(IsNothing(udcInputHKID.CCCode6), String.Empty, udcInputHKID.CCCode6)
                                End If


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

                        Me.udcCCCode.GetChineseName(FuncCode, True)
                        Session(SESS_DefaultSetCCCode) = Nothing
                        Session.Remove(SESS_DefaultSetCCCode)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub SetupPageContent(ByVal _udtEHSAccount As EHSAccountModel, ByVal _udtEHSAccount_Amend As EHSAccountModel, ByVal _udtEHSAccount_Rectify As EHSAccountModel, ByVal activeChanged As Boolean)
        Select Case Me.mveHSAccount.ActiveViewIndex
            Case intShowList
                If IsNothing(Session(SESS_RectifyAccList)) Then
                    'LoadRectifyRecord(Me.Session(SESS_ShowSpecialAccount))
                    LoadRectifyRecord()
                End If
            Case intAccountDetails
                If Session(SESS_RequirePageLoadPersonalBind) Or Session(SESS_DetailPageShowDeceased) Then
                    BindPersonalInfo(_udtEHSAccount, _udtEHSAccount_Amend, _udtEHSAccount_Rectify, Me.txtDocCode.Text.Trim, activeChanged)
                End If
            Case intConfirm
                SetupConfirmAccount(_udtEHSAccount_Rectify)
        End Select
    End Sub
#End Region

#Region "Set Controls"
    Private Sub ClearSession(ByVal blnClearResultListSession As Boolean)

        If blnClearResultListSession Then
            Session(SESS_RectifyAccList) = Nothing
            Session.Remove(SESS_RectifyAccList)
        End If

        Session(SESS_InputMode) = Nothing
        Session.Remove(SESS_InputMode)

        Session(SESS_DefaultSetCCCode) = Nothing
        Session.Remove(SESS_DefaultSetCCCode)

        Session(SESS_ClickSave) = Nothing
        Session.Remove(SESS_ClickSave)

        udteHSAccountMaintBLL.EHSAccountRemoveFromSession(FuncCode)
        udteHSAccountMaintBLL.EHSAccount_Amend_RemoveFromSession(FuncCode)

        txtDocCode.Text = String.Empty

        Me.udcCCCode.Clean()
        Me.udcCCCode.CleanSession(FuncCode)


    End Sub

    Private Sub SetAccountBtn(ByVal ActionMode As ActionModel)

        Me.udcInfoMsgBox.Clear()
        Me.ibtnRemoveTempAccountByBO.Visible = False

        Select Case ActionMode
            Case ActionModel.ReadOnly_withOriginal
                '--------------For Validated Account (2 sides --> show both original and amending record)
                'Me.btnEdit.Visible = True
                'Me.btnWithdrawAmendment.Visible = True
                'Me.tblShowDetailEditButtonGroup.Visible = False
                'Me.btnBackAcctList.Visible = True
                Dim udteHSAccountAmendPersonalInfo As EHSAccountModel.EHSPersonalInformationModel
                udteHSAccountAmendPersonalInfo = udtEHSAccount_Amendment.getPersonalInformation(Me.txtDocCode.Text.Trim)

                If udteHSAccountAmendPersonalInfo.Validating Then

                    Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Information
                    Me.udcInfoMsgBox.AddMessage(FunctCode.FUNT010301, SeverityCode.SEVI, MsgCode.MSG00014)
                    Me.udcInfoMsgBox.BuildMessageBox()

                    Me.btnEdit.Visible = False
                    Me.btnWithdrawAmendment.Visible = False
                    Me.tblShowDetailEditButtonGroup.Visible = False
                    Me.btnBackAcctList.Visible = True
                Else
                    Me.btnEdit.Visible = True
                    Me.btnWithdrawAmendment.Visible = True
                    Me.tblShowDetailEditButtonGroup.Visible = False
                    Me.btnBackAcctList.Visible = True
                End If

            Case ActionModel.Amending_withOriginal
                '--------------For Validated Account (2 sides --> show both original and amending record)
                Me.btnEdit.Visible = False
                Me.btnWithdrawAmendment.Visible = False
                Me.tblShowDetailEditButtonGroup.Visible = True
                Me.btnBackAcctList.Visible = False

            Case ActionModel.ReadOnly
                '--------------For Temp / Special Account
                Me.btnEdit.Visible = False
                Me.ibtnRemoveTempAccountByBO.Visible = False
                Me.btnWithdrawAmendment.Visible = False
                Me.tblShowDetailEditButtonGroup.Visible = False
                Me.btnBackAcctList.Visible = True

                'Whethter to show "Remove" and "Edit" button
                Dim udtEHSAccount As EHSAccount.EHSAccountModel
                Dim udteHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel

                udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FuncCode)
                udteHSAccountPersonalInfo = udtEHSAccount.getPersonalInformation(Me.txtDocCode.Text.Trim)

                Select Case udtEHSAccount.AccountSource
                    Case EHSAccountModel.SysAccountSource.TemporaryAccount
                        If udtEHSAccount.CreateByBO And Not udteHSAccountPersonalInfo.Validating Then

                            'the temporary account can not be removed if a transaction is linked with this eHS account
                            If Not IsNothing(udtEHSAccount.TransactionID) AndAlso Not udtEHSAccount.TransactionID.Equals(String.Empty) Then
                                Me.ibtnRemoveTempAccountByBO.Visible = False
                            Else
                                ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [Start][Koala]
                                If udtEHSAccount.SchemeCode.Trim <> Scheme.SchemeClaimModel.PPP Then
                                    Me.ibtnRemoveTempAccountByBO.Visible = True
                                End If
                                ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [End][Koala]
                            End If

                            Me.btnEdit.Visible = True
                        End If

                        If udteHSAccountPersonalInfo.Validating Then
                            Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Information
                            Me.udcInfoMsgBox.AddMessage(FunctCode.FUNT010301, SeverityCode.SEVI, MsgCode.MSG00014)
                            Me.udcInfoMsgBox.BuildMessageBox()
                        End If
                    Case EHSAccountModel.SysAccountSource.SpecialAccount
                        'Allow to edit special account when record status = I or (recordstatus = P and validating = N)
                        If udtEHSAccount.RecordStatus = EHSAccountModel.SpecialAccountRecordStatusClass.PendingVerify And Not udteHSAccountPersonalInfo.Validating _
                            Or (udtEHSAccount.RecordStatus = EHSAccountModel.SpecialAccountRecordStatusClass.InValid) Then
                            Me.btnEdit.Visible = True
                        End If

                        If udteHSAccountPersonalInfo.Validating Then
                            Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Information
                            Me.udcInfoMsgBox.AddMessage(FunctCode.FUNT010301, SeverityCode.SEVI, MsgCode.MSG00014)
                            Me.udcInfoMsgBox.BuildMessageBox()
                        End If
                End Select

            Case ActionModel.Amending
                '--------------For Temp / Special Account
                Me.btnEdit.Visible = False
                Me.btnWithdrawAmendment.Visible = False
                Me.tblShowDetailEditButtonGroup.Visible = True
                Me.btnBackAcctList.Visible = False
        End Select



    End Sub
#End Region

#Region "CCCode"

    Private Sub ucInputDocumentType_SelectChineseName_HKIC(ByVal mode As ucInputDocTypeBase.BuildMode, ByVal udcInputDocumentType As ucInputDocTypeBase, ByVal strDocCode As String, ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ucInputDocumentType.SelectChineseName_mode
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00023, AuditLogDesc.ClickSelectChineseNameButton)

        Dim sm As SystemMessage

        Me.Session.Remove(SESS_ClickSave)

        'Sender = Nothing => User Click "Save" Btn to fire this event
        If IsNothing(sender) Then
            Session(SESS_ClickSave) = True
        End If


        Select Case strDocCode
            Case DocTypeModel.DocTypeCode.HKIC
                Dim udcInputHKID As ucInputHKID = CType(udcInputDocumentType, ucInputHKID)

                udcInputHKID.SetProperty(mode)

                If udcInputHKID.CCCodeIsEmpty Then
                    'No CCCode
                    udcInputHKID.SetCnameAmend(String.Empty)

                    sm = New SystemMessage(CommonFuncCode, Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00143)
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

                    sm = Me.udcCCCode.BindCCCode()

                    ' INT20-0047 (Fix throw error for invalid CCCode) [Start][Winnie]
                    Me.udtAuditLogEntry.AddDescripton("CCCode1", Me.udcCCCode.CCCode1)
                    Me.udtAuditLogEntry.AddDescripton("CCCode2", Me.udcCCCode.CCCode2)
                    Me.udtAuditLogEntry.AddDescripton("CCCode3", Me.udcCCCode.CCCode3)
                    Me.udtAuditLogEntry.AddDescripton("CCCode4", Me.udcCCCode.CCCode4)
                    Me.udtAuditLogEntry.AddDescripton("CCCode5", Me.udcCCCode.CCCode5)
                    Me.udtAuditLogEntry.AddDescripton("CCCode6", Me.udcCCCode.CCCode6)
                    ' INT20-0047 (Fix throw error for invalid CCCode) [End][Winnie]

                    'Bind CCCode Drop Down List
                    If sm Is Nothing Then
                        udcInputHKID.SetCCCodeError(False)
                        Me.ModalPopupExtenderChooseCCCode.Show()
                        Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00024, AuditLogDesc.ChineseNameCodeCheckingSuccess)
                    Else
                        sm = New SystemMessage(CommonFuncCode, Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00039)
                        Me.udcMsgBox.AddMessage(sm)
                        udcInputHKID.SetCCCodeError(True)
                    End If
                End If


            Case DocTypeModel.DocTypeCode.ROP140

                Dim udcInputROP140 As ucInputROP140 = CType(udcInputDocumentType, ucInputROP140)
                udcInputROP140.SetProperty(mode)

                If udcInputROP140.CCCodeIsEmpty Then

                    'No CCCode
                    udcInputROP140.SetCnameAmend(String.Empty)

                    sm = New SystemMessage(CommonFuncCode, Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00143)
                    Me.udcMsgBox.AddMessage(sm)
                    udcInputROP140.SetCCCodeError(True)

                Else
                    ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [Start][Winnie] Step 1
                    Me.udcCCCode.DocCode = DocTypeModel.DocTypeCode.ROP140
                    Me.udcCCCode.CCCode1 = udcInputROP140.GetCCCode(udcInputROP140.CCCode1, Me.udcCCCode.getCCCodeFromSession(1, FuncCode))
                    Me.udcCCCode.CCCode2 = udcInputROP140.GetCCCode(udcInputROP140.CCCode2, Me.udcCCCode.getCCCodeFromSession(2, FuncCode))
                    Me.udcCCCode.CCCode3 = udcInputROP140.GetCCCode(udcInputROP140.CCCode3, Me.udcCCCode.getCCCodeFromSession(3, FuncCode))
                    Me.udcCCCode.CCCode4 = udcInputROP140.GetCCCode(udcInputROP140.CCCode4, Me.udcCCCode.getCCCodeFromSession(4, FuncCode))
                    Me.udcCCCode.CCCode5 = udcInputROP140.GetCCCode(udcInputROP140.CCCode5, Me.udcCCCode.getCCCodeFromSession(5, FuncCode))
                    Me.udcCCCode.CCCode6 = udcInputROP140.GetCCCode(udcInputROP140.CCCode6, Me.udcCCCode.getCCCodeFromSession(6, FuncCode))
                    ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [End][Winnie] Step 1

                    Me.udcCCCode.RowDisplayStyle = ChooseCCCode.DisplayStyle.SingalRow

                    sm = Me.udcCCCode.BindCCCode()

                    ' INT20-0047 (Fix throw error for invalid CCCode) [Start][Winnie]
                    Me.udtAuditLogEntry.AddDescripton("CCCode1", Me.udcCCCode.CCCode1)
                    Me.udtAuditLogEntry.AddDescripton("CCCode2", Me.udcCCCode.CCCode2)
                    Me.udtAuditLogEntry.AddDescripton("CCCode3", Me.udcCCCode.CCCode3)
                    Me.udtAuditLogEntry.AddDescripton("CCCode4", Me.udcCCCode.CCCode4)
                    Me.udtAuditLogEntry.AddDescripton("CCCode5", Me.udcCCCode.CCCode5)
                    Me.udtAuditLogEntry.AddDescripton("CCCode6", Me.udcCCCode.CCCode6)
                    ' INT20-0047 (Fix throw error for invalid CCCode) [End][Winnie]

                    'Bind CCCode Drop Down List
                    If sm Is Nothing Then
                        udcInputROP140.SetCCCodeError(False)
                        Me.ModalPopupExtenderChooseCCCode.Show()
                        Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00024, AuditLogDesc.ChineseNameCodeCheckingSuccess)
                    Else
                        sm = New SystemMessage(CommonFuncCode, Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00039)
                        Me.udcMsgBox.AddMessage(sm)
                        udcInputROP140.SetCCCodeError(True)
                    End If
                End If


        End Select


        If Not IsNothing(sender) Then
            Me.udcMsgBox.BuildMessageBox(strValidationFail, udtAuditLogEntry, LogID.LOG00025, AuditLogDesc.ChineseNameCodeCheckingFail)
        End If

    End Sub

    Private Function NeedPopupCCCodeDialog(ByVal mode As ucInputDocTypeBase.BuildMode, ByVal strDocCode As String) As Boolean
        'isDiff is using for check the sessoion CCCode is same as current CCCode 
        'isDiff = true : sessoion CCCode <> current CCCode 
        Dim isDiff As Boolean = True
        Select Case strDocCode
            Case DocTypeModel.DocTypeCode.HKIC
                Dim udcInputHKIC As ucInputHKID = Me.ucInputDocumentType.GetHKICControl()
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
                Dim udcInputROP140 As ucInputROP140 = Me.ucInputDocumentType.GetROP140Control()
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
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00027, AuditLogDesc.CancelChineseName)

        Me.ModalPopupExtenderChooseCCCode.Hide()
    End Sub

    Private Sub udcChooseCCCode_Confirm(ByVal strDocCode As String, ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcCCCode.Confirm

        Dim _udtEHSAccount As EHSAccountModel = Me.udteHSAccountMaintBLL.EHSAccount_Rectify_GetFromSession(FuncCode)
        Dim strCName As String = String.Empty


        Dim mode As ActionModel = Session(SESS_InputMode)
        Dim DocInputMode As ucInputDocTypeBase.BuildMode

        'Set suitable document input mode --> Retrieve CCC Code
        If mode = ActionModel.Amending Then
            'For amendment of validated account
            DocInputMode = ucInputDocTypeBase.BuildMode.Modification_OneSide
        ElseIf mode = ActionModel.Amending_withOriginal Then
            'For temp/special account
            DocInputMode = ucInputDocTypeBase.BuildMode.Modification
        End If

        Select Case strDocCode
            Case DocTypeModel.DocTypeCode.HKIC

                Dim udcIputHKIC As ucInputHKID = Me.ucInputDocumentType.GetHKICControl
                'Retrieve CCC Code
                udcIputHKIC.SetProperty(DocInputMode)
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
                Me.udteHSAccountMaintBLL.EHSAccount_Rectify_SaveToSession(_udtEHSAccount, FuncCode)


            Case DocTypeModel.DocTypeCode.ROP140

                Dim udcIputROP140 As ucInputROP140 = Me.ucInputDocumentType.GetROP140Control
                'Retrieve CCC Code
                udcIputROP140.SetProperty(DocInputMode)
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
                Me.udteHSAccountMaintBLL.EHSAccount_Rectify_SaveToSession(_udtEHSAccount, FuncCode)
        End Select


        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("ChineseName", strCName)
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00026, AuditLogDesc.ConfirmChineseName)



        Me.ModalPopupExtenderChooseCCCode.Hide()

        Dim blnClickSave As Boolean = False
        If Not IsNothing(Session(SESS_ClickSave)) Then
            ' CCCode incorrect & user had clicked "Save" btn in Rectify Account
            blnClickSave = CBool(Session(SESS_ClickSave))
            If blnClickSave Then
                Session(SESS_ClickSave) = Nothing
                Me.Session.Remove(SESS_ClickSave)
                Me.btnSave_Click(Nothing, Nothing)
            End If

        End If
    End Sub

#End Region

#Region "Enter Details Validation"
    'HKID
    Private Function ValidateRectifyDetail_HKID(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isValid As Boolean = True
        Dim udcInputHKIC As ucInputHKID = Me.ucInputDocumentType.GetHKICControl
        Dim mode As ActionModel
        mode = CType(Session(SESS_InputMode), ActionModel)

        If mode = ActionModel.Amending Then
            'Special Account, one side
            udcInputHKIC.SetProperty(ucInputDocTypeBase.BuildMode.Modification_OneSide)
            udcInputHKIC.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification_OneSide, False)
        Else
            'Validated Account, with original
            udcInputHKIC.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
            udcInputHKIC.SetErrorImage(False)
        End If

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
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00008, AuditLogDesc.ValidateAccountDetailInfo)

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String
        Dim dtmDOB As Date

        strDOB = udcInputHKIC.DOB
        Me.udtSM = Me.udtvalidator.chkDOB(DocType.DocTypeModel.DocTypeCode.HKIC, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputHKIC.SetDOBError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'English Name
        Me.udtSM = Me.udtvalidator.chkEngName(udcInputHKIC.ENameSurName, udcInputHKIC.ENameFirstName, DocType.DocTypeModel.DocTypeCode.HKIC)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputHKIC.SetENameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'CCCode
        Me.udtSM = Me.udtvalidator.chkCCCode_UsingDDL(String.Format("{0}{1}", udcInputHKIC.CCCode1, Me.udcCCCode.SelectedCCCodeTail1), _
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
        Me.udtSM = Me.udtvalidator.chkGender(udcInputHKIC.Gender)
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
        Me.udtSM = Me.udtvalidator.chkHKIDIssueDate(strHKIDIssueDate, dtmDOB)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputHKIC.SetHKIDIssueDateError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        Else
            dtHKIDIssueDate = Me.udtformatter.convertHKIDIssueDateStringToDate(strHKIDIssueDate)
        End If

        If isValid Then
            Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKIC)

            udtEHSAccountPersonalInfo.ENameSurName = udcInputHKIC.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputHKIC.ENameFirstName

            udtEHSAccountPersonalInfo.CCCode1 = String.Format("{0}{1}", udcInputHKIC.CCCode1, Me.udcCCCode.SelectedCCCodeTail1)
            udtEHSAccountPersonalInfo.CCCode2 = String.Format("{0}{1}", udcInputHKIC.CCCode2, Me.udcCCCode.SelectedCCCodeTail2)
            udtEHSAccountPersonalInfo.CCCode3 = String.Format("{0}{1}", udcInputHKIC.CCCode3, Me.udcCCCode.SelectedCCCodeTail3)
            udtEHSAccountPersonalInfo.CCCode4 = String.Format("{0}{1}", udcInputHKIC.CCCode4, Me.udcCCCode.SelectedCCCodeTail4)
            udtEHSAccountPersonalInfo.CCCode5 = String.Format("{0}{1}", udcInputHKIC.CCCode5, Me.udcCCCode.SelectedCCCodeTail5)
            udtEHSAccountPersonalInfo.CCCode6 = String.Format("{0}{1}", udcInputHKIC.CCCode6, Me.udcCCCode.SelectedCCCodeTail6)

            'udcInputHKIC.SetCName()
            udtEHSAccountPersonalInfo.CName = udcInputHKIC.CName
            udtEHSAccountPersonalInfo.DOB = dtmDOB
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

        Dim udcInputEC As ucInputEC = Me.ucInputDocumentType.GetECControl
        Dim mode As ActionModel
        mode = CType(Session(SESS_InputMode), ActionModel)

        If mode = ActionModel.Amending Then
            'Special Account, one side
            udcInputEC.SetProperty(ucInputDocTypeBase.BuildMode.Modification_OneSide)
            udcInputEC.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification_OneSide, False)
        Else
            'Validated Account, with original
            udcInputEC.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
            udcInputEC.SetModificationErrorImage(False)
        End If

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
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00008, AuditLogDesc.ValidateAccountDetailInfo)

        ' Serial No.
        Me.udtSM = Me.udtvalidator.chkSerialNo(udcInputEC.SerialNumber, udcInputEC.SerialNumberNotProvided)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputEC.SetECSerialNoError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        ' Reference
        Me.udtSM = Me.udtvalidator.chkReferenceNo(udcInputEC.Reference, udcInputEC.ReferenceOtherFormat)
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
                sm_DOB = New SystemMessage(CommonFuncCode, SeverityCode.SEVE, MsgCode.MSG00003)
            Case ucInputEC.DOBSelection.AgeWithDateOfRegistration
                'Check Age
                sm_DOB = Me.udtvalidator.chkECAge(udcInputEC.ECAge)
                If Not sm_DOB Is Nothing Then
                    isValid = False
                    udcInputEC.SetDOBAgeError(True)
                Else
                    strAge = udcInputEC.ECAge
                End If

                ' validate Date of Age
                sm_DOR = Me.udtvalidator.chkECDOAge(strDateOfRegDay, strDateOfRegMth, strDateOfRegYr)
                If Not sm_DOR Is Nothing Then
                    isValid = False
                    udcInputEC.SetDateOfRegError(True)
                Else
                    strDateOfReg = String.Format("{0:00}-{1}-{2}", Convert.ToInt32(strDateOfRegDay), strDateOfRegMth, strDateOfRegYr)

                    dtDOR = CDate(Me.udtformatter.convertDate(strDateOfReg, Session(SESS_Language)))
                End If

                ' validate Age + Date of Age if Within Age
                If isValid Then
                    sm_DOB = Me.udtvalidator.chkECAgeAndDOAge(udcInputEC.ECAge, strDateOfRegDay, strDateOfRegMth, strDateOfRegYr)
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
                sm_DOB = Me.udtvalidator.chkDOB(DocTypeModel.DocTypeCode.EC, strDOB, dtmDOB, strExactDOB)

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
                                    'udtSM = New SystemMessage(CommonFuncCode, SeverityCode.SEVE, MsgCode.MSG00004)
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
        Me.udtSM = Me.udtvalidator.chkECDate(strECDateDay, strECDateMonth, strECDateYear, dtmDOB)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputEC.SetECDateError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'English Name
        Me.udtSM = Me.udtvalidator.chkEngName(udcInputEC.ENameSurName, udcInputEC.ENameFirstName, DocType.DocTypeModel.DocTypeCode.EC)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputEC.SetENameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        ' CRE15-014 HA_MingLiu UTF32 [Start][Winnie]
        'Chinese Name
        Me.udtSM = Me.udtvalidator.chkChiName(udcInputEC.CName)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputEC.SetCNameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If
        ' CRE15-014 HA_MingLiu UTF32 [End][Winnie]

        'Gender
        Me.udtSM = Me.udtvalidator.chkGender(udcInputEC.Gender)
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

            udtSM = udtvalidator.chkSerialNoNotProvidedAllow(dtmECDate, udcInputEC.SerialNumberNotProvided)
            If Not IsNothing(udtSM) Then
                isValid = False
                udcInputEC.SetECSerialNoError(True)
                udcMsgBox.AddMessage(udtSM)
            End If

            ' Try parse the Reference if all the previous inputs are valid
            If isValid Then
                If udcInputEC.ReferenceOtherFormat Then
                    Dim dtmECDOI As New Date(udcInputEC.ECDateYear, udcInputEC.ECDateMonth, udcInputEC.ECDateDay)
                    udtvalidator.TryParseECReference(udcInputEC.Reference, udcInputEC.ReferenceOtherFormat, dtmECDOI)
                End If

            End If

            udtSM = udtvalidator.chkReferenceOtherFormatAllow(dtmECDate, udcInputEC.ReferenceOtherFormat)
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
    Private Function ValidateRectifyDetail_HKBC(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isValid As Boolean = True
        Dim udcInputHKBC As ucInputHKBC = Me.ucInputDocumentType.GetHKBCControl
        Dim mode As ActionModel
        mode = CType(Session(SESS_InputMode), ActionModel)

        If mode = ActionModel.Amending Then
            'Special Account, one side
            udcInputHKBC.SetProperty(ucInputDocTypeBase.BuildMode.Modification_OneSide)
            udcInputHKBC.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification_OneSide, False)
        Else
            'Validated Account, with original
            udcInputHKBC.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
            udcInputHKBC.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)
        End If

        _udtAuditLogEntry.AddDescripton("RegistrationNo", udcInputHKBC.RegistrationNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputHKBC.DOB)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputHKBC.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputHKBC.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputHKBC.Gender)
        _udtAuditLogEntry.AddDescripton("DOBInWord", udcInputHKBC.DOBInWord)
        _udtAuditLogEntry.AddDescripton("DOBInWordCase", udcInputHKBC.DOBInWordCase.ToString())
        _udtAuditLogEntry.AddDescripton("ExactDOB", udcInputHKBC.IsExactDOB)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00008, AuditLogDesc.ValidateAccountDetailInfo)

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
                udtSM = Me.udtvalidator.chkDOB(DocTypeModel.DocTypeCode.HKBC, strDOB, dtmDOB, strExactDOB)

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
        Me.udtSM = Me.udtvalidator.chkEngName(udcInputHKBC.ENameSurName, udcInputHKBC.ENameFirstName, DocType.DocTypeModel.DocTypeCode.HKBC)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputHKBC.SetENameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'Gender
        Me.udtSM = Me.udtvalidator.chkGender(udcInputHKBC.Gender)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputHKBC.SetGenderError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        If isValid Then
            Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKBC)
            udtEHSAccountPersonalInfo.ENameSurName = udcInputHKBC.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputHKBC.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputHKBC.Gender
            udtEHSAccountPersonalInfo.ExactDOB = udcInputHKBC.IsExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB
            udtEHSAccountPersonalInfo.OtherInfo = udcInputHKBC.DOBInWord
        End If


        Return isValid
    End Function

    'Adoption
    Private Function ValidateRectifyDetail_Adopt(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isvalid As Boolean = True
        Dim mode As ActionModel
        mode = CType(Session(SESS_InputMode), ActionModel)

        Dim udcInputAdopt As ucInputAdoption = Me.ucInputDocumentType.GetADOPCControl

        If mode = ActionModel.Amending Then
            'Special Account, one side
            udcInputAdopt.SetProperty(ucInputDocTypeBase.BuildMode.Modification_OneSide)
            udcInputAdopt.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification_OneSide, False)
        Else
            'Validated Account, with original
            udcInputAdopt.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
            udcInputAdopt.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)
        End If

        _udtAuditLogEntry.AddDescripton("NoOfEntry", udcInputAdopt.IdentityNo)
        _udtAuditLogEntry.AddDescripton("Prefix", udcInputAdopt.PerfixNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputAdopt.DOB)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputAdopt.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputAdopt.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputAdopt.Gender)
        _udtAuditLogEntry.AddDescripton("DOBInWord", udcInputAdopt.DOBInWord)
        _udtAuditLogEntry.AddDescripton("DOBInWordCase", udcInputAdopt.DOBInWordCase.ToString())
        _udtAuditLogEntry.AddDescripton("ExactDOB", udcInputAdopt.IsExactDOB)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00008, AuditLogDesc.ValidateAccountDetailInfo)

        'Prefix
        'If udcInputAdopt.PerfixNo.Equals(String.Empty) Then
        '    isvalid = False
        '    udcInputAdopt.SetEntryNoError(True)
        '    Me.udcMsgBox.AddMessage(New SystemMessage("990000", "E", "00210"))
        'End If
        Me.udtSM = Me.udtvalidator.chkIdentityNumber(DocTypeModel.DocTypeCode.ADOPC, udcInputAdopt.IdentityNo, udcInputAdopt.PerfixNo)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputAdopt.SetEntryNoError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'English Name
        Me.udtSM = Me.udtvalidator.chkEngName(udcInputAdopt.ENameSurName, udcInputAdopt.ENameFirstName, DocType.DocTypeModel.DocTypeCode.ADOPC)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputAdopt.SetENameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'Gender
        Me.udtSM = Me.udtvalidator.chkGender(udcInputAdopt.Gender)
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
                udtSM = Me.udtvalidator.chkDOB(DocTypeModel.DocTypeCode.ADOPC, strDOB, dtmDOB, strExactDOB)

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
            udtEHSAccountPersonalInfo.ENameSurName = udcInputAdopt.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputAdopt.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputAdopt.Gender
            udtEHSAccountPersonalInfo.ExactDOB = udcInputAdopt.IsExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB
            udtEHSAccountPersonalInfo.OtherInfo = udcInputAdopt.DOBInWord
            udtEHSAccountPersonalInfo.AdoptionPrefixNum = udcInputAdopt.PerfixNo
        End If

        Return isvalid
    End Function

    'DI
    Private Function ValidateRectifyDetail_DI(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isvalid As Boolean = True
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.DI)
        Dim udcInputDI As ucInputDI = Me.ucInputDocumentType.GetDIControl
        Dim mode As ActionModel
        mode = CType(Session(SESS_InputMode), ActionModel)

        If mode = ActionModel.Amending Then
            'Special Account, one side
            udcInputDI.SetProperty(ucInputDocTypeBase.BuildMode.Modification_OneSide)
            udcInputDI.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification_OneSide, False)
        Else
            'Validated Account, with original
            udcInputDI.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
            udcInputDI.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)
        End If

        _udtAuditLogEntry.AddDescripton("DocNo", udcInputDI.TravelDocNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputDI.DOB)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputDI.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputDI.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputDI.Gender)
        _udtAuditLogEntry.AddDescripton("DOI", udcInputDI.DateOfIssue)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputDI.DOB)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00008, AuditLogDesc.ValidateAccountDetailInfo)

        'English Name
        Me.udtSM = Me.udtvalidator.chkEngName(udcInputDI.ENameSurName, udcInputDI.ENameFirstName, DocType.DocTypeModel.DocTypeCode.DI)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputDI.SetENameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'Gender
        Me.udtSM = Me.udtvalidator.chkGender(udcInputDI.Gender)
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

        Me.udtSM = Me.udtvalidator.chkDOB(DocType.DocTypeModel.DocTypeCode.DI, strDOB, dtmDOB, strExactDOB)
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
        Me.udtSM = Me.udtvalidator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.DI, strDOI, udtEHSAccountPersonalInfo.DOB)
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
        Dim isvalid As Boolean = True
        'Dim dtmDOB As DateTime
        Dim dtPermit As DateTime
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.ID235B)
        Dim udcInputID235B As ucInputID235B = Me.ucInputDocumentType.GetID235BControl
        Dim mode As ActionModel
        mode = CType(Session(SESS_InputMode), ActionModel)

        If mode = ActionModel.Amending Then
            'Special Account, one side
            udcInputID235B.SetProperty(ucInputDocTypeBase.BuildMode.Modification_OneSide)
            udcInputID235B.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification_OneSide, False)
        Else
            'Validated Account, with original
            udcInputID235B.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
            udcInputID235B.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)
        End If

        _udtAuditLogEntry.AddDescripton("BirthEntryNo", udcInputID235B.BirthEntryNo)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputID235B.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputID235B.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputID235B.Gender)
        _udtAuditLogEntry.AddDescripton("RemainUntil", udcInputID235B.PermitRemain)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputID235B.DateOfBirth)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00008, AuditLogDesc.ValidateAccountDetailInfo)

        'English Name
        Me.udtSM = Me.udtvalidator.chkEngName(udcInputID235B.ENameSurName, udcInputID235B.ENameFirstName, DocType.DocTypeModel.DocTypeCode.ID235B)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputID235B.SetENameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'Gender
        Me.udtSM = Me.udtvalidator.chkGender(udcInputID235B.Gender)
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
        Me.udtSM = Me.udtvalidator.chkDOB(DocType.DocTypeModel.DocTypeCode.ID235B, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputID235B.SetDOBError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        Else
            udtEHSAccountPersonalInfo.DOB = dtmDOB
        End If

        'Permit to remain until
        Dim strPermit As String = Nothing
        strPermit = Me.udtformatter.formatDateBeforValidation_DDMMYYYY(udcInputID235B.PermitRemain)
        'strPermit = Me.udtformatter.formatPermitToReminUntilBeforeValidate(udcInputID235B.PermitRemain)
        'Me.udtSM = Me.udtvalidator.chkPremitToRemainUntil(strPermit, dtmDOB)
        strPermit = Me.udtformatter.formatDateBeforValidation_DDMMYYYY(udcInputID235B.PermitRemain)
        Me.udtSM = Me.udtvalidator.chkPremitToRemainUntil(strPermit, udtEHSAccountPersonalInfo.DOB, DocType.DocTypeModel.DocTypeCode.ID235B)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputID235B.SetPermitRemainError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        Else
            dtPermit = CDate(Me.udtformatter.convertDate(Me.udtformatter.formatInputDate(strPermit), Common.Component.CultureLanguage.English))
        End If

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
        Dim udcInputReentryPermit As ucInputReentryPermit = Me.ucInputDocumentType.GetREPMTControl
        Dim mode As ActionModel
        mode = CType(Session(SESS_InputMode), ActionModel)

        If mode = ActionModel.Amending Then
            'Special Account, one side
            udcInputReentryPermit.SetProperty(ucInputDocTypeBase.BuildMode.Modification_OneSide)
            udcInputReentryPermit.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification_OneSide, False)
        Else
            'Validated Account, with original
            udcInputReentryPermit.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
            udcInputReentryPermit.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)
        End If

        _udtAuditLogEntry.AddDescripton("REPMTNo", udcInputReentryPermit.REPMTNo)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputReentryPermit.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputReentryPermit.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputReentryPermit.Gender)
        _udtAuditLogEntry.AddDescripton("DOI", udcInputReentryPermit.DateOfIssue)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputReentryPermit.DateOfBirth)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00008, AuditLogDesc.ValidateAccountDetailInfo)

        'English Name
        Me.udtSM = Me.udtvalidator.chkEngName(udcInputReentryPermit.ENameSurName, udcInputReentryPermit.ENameFirstName, DocType.DocTypeModel.DocTypeCode.REPMT)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputReentryPermit.SetENameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'Gender
        Me.udtSM = Me.udtvalidator.chkGender(udcInputReentryPermit.Gender)
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
        Me.udtSM = Me.udtvalidator.chkDOB(DocType.DocTypeModel.DocTypeCode.REPMT, strDOB, dtmDOB, strExactDOB)
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
        Me.udtSM = Me.udtvalidator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.REPMT, strIssueDate, udtEHSAccountPersonalInfo.DOB)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputReentryPermit.SetDOIError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        Else
            dtIssueDate = CDate(Me.udtformatter.convertDate(Me.udtformatter.formatInputDate(strIssueDate), Common.Component.CultureLanguage.English))
        End If
        'End If

        If isvalid Then
            udtEHSAccountPersonalInfo.ENameSurName = udcInputReentryPermit.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputReentryPermit.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputReentryPermit.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB
            udtEHSAccountPersonalInfo.DateofIssue = dtIssueDate
        End If

        Return isvalid
    End Function

    'Visa
    Private Function ValidateRectifyDetail_Visa(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isValid As Boolean = True
        Dim udcInputVisa As ucInputVISA = Me.ucInputDocumentType.GetVISAControl
        Dim mode As ActionModel
        mode = CType(Session(SESS_InputMode), ActionModel)

        If mode = ActionModel.Amending Then
            'Special Account, one side
            udcInputVisa.SetProperty(ucInputDocTypeBase.BuildMode.Modification_OneSide)
            udcInputVisa.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification_OneSide, False)
        Else
            'Validated Account, with original
            udcInputVisa.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
            udcInputVisa.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)
        End If

        _udtAuditLogEntry.AddDescripton("VISANo", udcInputVisa.VISANo)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputVisa.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputVisa.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputVisa.Gender)
        _udtAuditLogEntry.AddDescripton("PassportNo", udcInputVisa.PassportNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputVisa.DOB)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00008, AuditLogDesc.ValidateAccountDetailInfo)

        'VISA
        If udcInputVisa.PassportNo.Equals(String.Empty) Then
            isValid = False
            udcInputVisa.SetPassportNoError(True)
            Me.udcMsgBox.AddMessage(New SystemMessage("990000", "E", "00236"))
        End If

        'English Name
        Me.udtSM = Me.udtvalidator.chkEngName(udcInputVisa.ENameSurName, udcInputVisa.ENameFirstName, DocType.DocTypeModel.DocTypeCode.VISA)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputVisa.SetENameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'Gender
        Me.udtSM = Me.udtvalidator.chkGender(udcInputVisa.Gender)
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
        Me.udtSM = Me.udtvalidator.chkDOB(DocType.DocTypeModel.DocTypeCode.VISA, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputVisa.SetDOBError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        If isValid Then
            Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.VISA)
            udtEHSAccountPersonalInfo.ENameSurName = udcInputVisa.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputVisa.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputVisa.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB
            udtEHSAccountPersonalInfo.Foreign_Passport_No = udcInputVisa.PassportNo
        End If


        Return isValid
    End Function

    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    'OW or RFNo8
    Private Function ValidateRectifyDetail_OW_RFNo8(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isValid As Boolean = True
        Dim udcInputOW As ucInputOW = Me.ucInputDocumentType.GetOWControl
        Dim mode As ActionModel
        mode = CType(Session(SESS_InputMode), ActionModel)

        If mode = ActionModel.Amending Then
            'Special Account, one side
            udcInputOW.SetProperty(ucInputDocTypeBase.BuildMode.Modification_OneSide)
            udcInputOW.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification_OneSide, False)
        Else
            'Validated Account, with original
            udcInputOW.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
            udcInputOW.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)
        End If

        _udtAuditLogEntry.AddDescripton("DocumentNo", udcInputOW.DocumentNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputOW.DOB)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputOW.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputOW.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputOW.Gender)
        _udtAuditLogEntry.AddDescripton("ExactDOB", udcInputOW.IsExactDOB)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00008, AuditLogDesc.ValidateAccountDetailInfo)

        ' CRE19-001 (VSS 2019) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        'Doc No.
        Me.udtSM = udtvalidator.chkDocumentNoForNonEHSDocType(udcInputOW.DocumentNo)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputOW.SetDocNoError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If
        ' CRE19-001 (VSS 2019) [End][Winnie]

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String = udcInputOW.DOB
        Dim dtmDOB As Date

        Select Case udcInputOW.DOB.Trim
            Case String.Empty
                ' Please input "Date of Birth".
                udtSM = New SystemMessage("990000", "E", "00003")
                udcInputOW.SetDOBError(True)
            Case Else
                udtSM = Me.udtvalidator.chkDOB(udcInputOW.EHSPersonalInfoAmend.DocCode, strDOB, dtmDOB, strExactDOB)

                If udtSM IsNot Nothing Then
                    udcInputOW.SetDOBError(True)
                End If
        End Select

        If Not IsNothing(udtSM) Then
            Me.udcMsgBox.AddMessage(udtSM)
            isValid = False
        End If

        'English Name
        Me.udtSM = Me.udtvalidator.chkEngName(udcInputOW.ENameSurName, udcInputOW.ENameFirstName, udcInputOW.EHSPersonalInfoAmend.DocCode)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputOW.SetENameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'Gender
        Me.udtSM = Me.udtvalidator.chkGender(udcInputOW.Gender)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputOW.SetGenderError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        If isValid Then
            Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(udcInputOW.EHSPersonalInfoAmend.DocCode)
            udtEHSAccountPersonalInfo.IdentityNum = udcInputOW.DocumentNo
            udtEHSAccountPersonalInfo.ENameSurName = udcInputOW.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputOW.ENameFirstName
            udtEHSAccountPersonalInfo.CName = udcInputOW.CName
            udtEHSAccountPersonalInfo.Gender = udcInputOW.Gender
            udtEHSAccountPersonalInfo.ExactDOB = udcInputOW.IsExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB
        End If

        Return isValid
    End Function
    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

    ' CRE20-0023 (Immu record) [Start][Martin]
    'TW
    Private Function ValidateRectifyDetail_TW(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isValid As Boolean = True
        Dim udcInputTW As ucInputTW = Me.ucInputDocumentType.GetTWControl
        Dim mode As ActionModel
        mode = CType(Session(SESS_InputMode), ActionModel)

        If mode = ActionModel.Amending Then
            'Special Account, one side
            udcInputTW.SetProperty(ucInputDocTypeBase.BuildMode.Modification_OneSide)
            udcInputTW.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification_OneSide, False)
        Else
            'Validated Account, with original
            udcInputTW.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
            udcInputTW.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)
        End If

        _udtAuditLogEntry.AddDescripton("DocumentNo", udcInputTW.DocumentNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputTW.DOB)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputTW.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputTW.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputTW.Gender)
        _udtAuditLogEntry.AddDescripton("ExactDOB", udcInputTW.IsExactDOB)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00008, AuditLogDesc.ValidateAccountDetailInfo)
        ' ----------------------------------------------------------------------------------------
        'Doc No.
        Me.udtSM = udtvalidator.chkDocumentNoForNonEHSDocType(udcInputTW.DocumentNo)
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
                ' Please input "Date of Birth".
                udtSM = New SystemMessage("990000", "E", "00003")
                udcInputTW.SetDOBError(True)
            Case Else
                udtSM = Me.udtvalidator.chkDOB(udcInputTW.EHSPersonalInfoAmend.DocCode, strDOB, dtmDOB, strExactDOB)

                If udtSM IsNot Nothing Then
                    udcInputTW.SetDOBError(True)
                End If
        End Select

        If Not IsNothing(udtSM) Then
            Me.udcMsgBox.AddMessage(udtSM)
            isValid = False
        End If

        'English Name
        Me.udtSM = Me.udtvalidator.chkEngName(udcInputTW.ENameSurName, udcInputTW.ENameFirstName, DocType.DocTypeModel.DocTypeCode.TW)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputTW.SetENameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'Gender
        Me.udtSM = Me.udtvalidator.chkGender(udcInputTW.Gender)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputTW.SetGenderError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        If isValid Then
            Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(udcInputTW.EHSPersonalInfoAmend.DocCode)
            udtEHSAccountPersonalInfo.IdentityNum = udcInputTW.DocumentNo
            udtEHSAccountPersonalInfo.ENameSurName = udcInputTW.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputTW.ENameFirstName
            udtEHSAccountPersonalInfo.CName = udcInputTW.CName
            udtEHSAccountPersonalInfo.Gender = udcInputTW.Gender
            udtEHSAccountPersonalInfo.ExactDOB = udcInputTW.IsExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB
        End If

        Return isValid
    End Function
    ' CRE20-0023 (Immu record) [Start][Martin]

    ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
    ' OC, IR, HKP, OTHER
    ''' <summary>
    ''' Validate rectification of document type OC, IR, HKP, OTHER
    ''' </summary>
    ''' <param name="_udtEHSAccount"></param>
    ''' <param name="_udtAuditLogEntry"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateRectifyDetail_OTHER(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isValid As Boolean = True
        Dim udcInputOTHER As ucInputOTHER = Me.ucInputDocumentType.GetOTHERControl
        Dim mode As ActionModel
        mode = CType(Session(SESS_InputMode), ActionModel)

        If mode = ActionModel.Amending Then
            'Special Account, one side
            udcInputOTHER.SetProperty(ucInputDocTypeBase.BuildMode.Modification_OneSide)
            udcInputOTHER.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification_OneSide, False)
        Else
            'Validated Account, with original
            udcInputOTHER.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
            udcInputOTHER.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)
        End If

        _udtAuditLogEntry.AddDescripton("DocumentNo", udcInputOTHER.DocumentNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputOTHER.DOB)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputOTHER.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputOTHER.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("ChiName", udcInputOTHER.CName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputOTHER.Gender)
        _udtAuditLogEntry.AddDescripton("ExactDOB", udcInputOTHER.IsExactDOB)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00008, AuditLogDesc.ValidateAccountDetailInfo)

        ' CRE19-001 (VSS 2019) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        'Doc No.
        Me.udtSM = udtvalidator.chkDocumentNoForNonEHSDocType(udcInputOTHER.DocumentNo)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputOTHER.SetRegNoError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If
        ' CRE19-001 (VSS 2019) [End][Winnie]

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String = udcInputOTHER.DOB
        Dim dtmDOB As Date

        Select Case udcInputOTHER.DOB.Trim
            Case String.Empty
                ' Please input "Date of Birth".
                udtSM = New SystemMessage("990000", "E", "00003")
                udcInputOTHER.SetDOBError(True)
            Case Else
                udtSM = Me.udtvalidator.chkDOB(DocTypeModel.DocTypeCode.OTHER, strDOB, dtmDOB, strExactDOB)

                If udtSM IsNot Nothing Then
                    udcInputOTHER.SetDOBError(True)
                End If
        End Select

        If Not IsNothing(udtSM) Then
            Me.udcMsgBox.AddMessage(udtSM)
            isValid = False
        End If


        'English Name
        Me.udtSM = Me.udtvalidator.chkEngName(udcInputOTHER.ENameSurName, udcInputOTHER.ENameFirstName, DocType.DocTypeModel.DocTypeCode.OTHER)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputOTHER.SetENameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'Chinese Name
        Me.udtSM = Me.udtvalidator.chkChiName(udcInputOTHER.CName)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputOTHER.SetCNameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'Gender
        Me.udtSM = Me.udtvalidator.chkGender(udcInputOTHER.Gender)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputOTHER.SetGenderError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        If isValid Then
            Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(udcInputOTHER.EHSPersonalInfoAmend.DocCode)
            udtEHSAccountPersonalInfo.IdentityNum = udcInputOTHER.DocumentNo
            udtEHSAccountPersonalInfo.ENameSurName = udcInputOTHER.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputOTHER.ENameFirstName
            udtEHSAccountPersonalInfo.CName = udcInputOTHER.CName
            udtEHSAccountPersonalInfo.Gender = udcInputOTHER.Gender
            udtEHSAccountPersonalInfo.ExactDOB = udcInputOTHER.IsExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB
        End If

        Return isValid
    End Function
    ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]

    'CCIC
    Private Function ValidateRectifyDetail_CCIC(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isvalid As Boolean = True
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.CCIC)
        Dim udcInputCCIC As ucInputCCIC = Me.ucInputDocumentType.GetCCICControl
        Dim mode As ActionModel
        mode = CType(Session(SESS_InputMode), ActionModel)

        If mode = ActionModel.Amending Then
            'Special Account, one side
            udcInputCCIC.SetProperty(ucInputDocTypeBase.BuildMode.Modification_OneSide)
            udcInputCCIC.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification_OneSide, False)
        Else
            'Validated Account, with original
            udcInputCCIC.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
            udcInputCCIC.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)
        End If

        _udtAuditLogEntry.AddDescripton("DocNo", udcInputCCIC.TravelDocNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputCCIC.DOB)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputCCIC.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputCCIC.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputCCIC.Gender)
        _udtAuditLogEntry.AddDescripton("DOI", udcInputCCIC.DateOfIssue)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00008, AuditLogDesc.ValidateAccountDetailInfo)

        'English Name
        Me.udtSM = Me.udtvalidator.chkEngName(udcInputCCIC.ENameSurName, udcInputCCIC.ENameFirstName, DocType.DocTypeModel.DocTypeCode.CCIC)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputCCIC.SetENameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'Gender
        Me.udtSM = Me.udtvalidator.chkGender(udcInputCCIC.Gender)
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

        Me.udtSM = Me.udtvalidator.chkDOB(DocType.DocTypeModel.DocTypeCode.CCIC, strDOB, dtmDOB, strExactDOB)
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
        Me.udtSM = Me.udtvalidator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.CCIC, strHKIDIssueDate, udtEHSAccountPersonalInfo.DOB)

        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputCCIC.SetDOIError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        Else
            dtHKIDIssueDate = Me.udtformatter.convertHKIDIssueDateStringToDate(strHKIDIssueDate)
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
        Dim udcInputROP140 As ucInputROP140 = Me.ucInputDocumentType.GetROP140Control
        Dim mode As ActionModel
        mode = CType(Session(SESS_InputMode), ActionModel)

        If mode = ActionModel.Amending Then
            'Special Account, one side
            udcInputROP140.SetProperty(ucInputDocTypeBase.BuildMode.Modification_OneSide)
            udcInputROP140.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification_OneSide, False)
        Else
            'Validated Account, with original
            udcInputROP140.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
            udcInputROP140.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)
        End If

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
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00008, AuditLogDesc.ValidateAccountDetailInfo)

        'English Name
        Me.udtSM = Me.udtvalidator.chkEngName(udcInputROP140.ENameSurName, udcInputROP140.ENameFirstName, DocType.DocTypeModel.DocTypeCode.ROP140)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputROP140.SetENameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'CCCode
        Me.udtSM = Me.udtvalidator.chkCCCode_UsingDDL(String.Format("{0}{1}", udcInputROP140.CCCode1, Me.udcCCCode.SelectedCCCodeTail1), _
                                                String.Format("{0}{1}", udcInputROP140.CCCode2, Me.udcCCCode.SelectedCCCodeTail2), _
                                                String.Format("{0}{1}", udcInputROP140.CCCode3, Me.udcCCCode.SelectedCCCodeTail3), _
                                                String.Format("{0}{1}", udcInputROP140.CCCode4, Me.udcCCCode.SelectedCCCodeTail4), _
                                                String.Format("{0}{1}", udcInputROP140.CCCode5, Me.udcCCCode.SelectedCCCodeTail5), _
                                                String.Format("{0}{1}", udcInputROP140.CCCode6, Me.udcCCCode.SelectedCCCodeTail6))


        'Gender
        Me.udtSM = Me.udtvalidator.chkGender(udcInputROP140.Gender)
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

        Me.udtSM = Me.udtvalidator.chkDOB(DocType.DocTypeModel.DocTypeCode.ROP140, strDOB, dtmDOB, strExactDOB)
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
        Me.udtSM = Me.udtvalidator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.ROP140, strDOI, udtEHSAccountPersonalInfo.DOB)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputROP140.SetDOIError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        Else
            dtIssueDate = CDate(Me.udtformatter.convertDate(Me.udtformatter.formatInputDate(strDOI), Common.Component.CultureLanguage.English))
        End If




        If isvalid Then
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

            'udcInputROP140.SetCName()
            udtEHSAccountPersonalInfo.CName = udcInputROP140.CName
        End If

        Return isvalid
    End Function

    'PASS
    Private Function ValidateRectifyDetail_PASS(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isvalid As Boolean = True
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.PASS)
        Dim udcInputPASS As ucInputPASS = Me.ucInputDocumentType.GetPASSControl
        Dim mode As ActionModel
        mode = CType(Session(SESS_InputMode), ActionModel)

        If mode = ActionModel.Amending Then
            'Special Account, one side
            udcInputPASS.SetProperty(ucInputDocTypeBase.BuildMode.Modification_OneSide)
            udcInputPASS.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification_OneSide, False)
        Else
            'Validated Account, with original
            udcInputPASS.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
            udcInputPASS.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)
        End If

        _udtAuditLogEntry.AddDescripton("DocNo", udcInputPASS.TravelDocNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputPASS.DOB)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputPASS.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputPASS.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputPASS.Gender)
        _udtAuditLogEntry.AddDescripton("PassportIssueRegion", udcInputPASS.PassportIssueRegion)

        _udtAuditLogEntry.WriteStartLog(LogID.LOG00008, AuditLogDesc.ValidateAccountDetailInfo)

        'English Name
        Me.udtSM = Me.udtvalidator.chkEngName(udcInputPASS.ENameSurName, udcInputPASS.ENameFirstName, DocType.DocTypeModel.DocTypeCode.PASS)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputPASS.SetENameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'Gender
        Me.udtSM = Me.udtvalidator.chkGender(udcInputPASS.Gender)
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

        Me.udtSM = Me.udtvalidator.chkDOB(DocType.DocTypeModel.DocTypeCode.PASS, strDOB, dtmDOB, strExactDOB)
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

    'ISSHk
    Private Function ValidateRectifyDetail_ISSHK(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isvalid As Boolean = True
        Dim udcInputISSHK As ucInputISSHK = Me.ucInputDocumentType.GetISSHKControl
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(udcInputISSHK.EHSPersonalInfoAmend.DocCode)
        Dim mode As ActionModel
        mode = CType(Session(SESS_InputMode), ActionModel)

        If mode = ActionModel.Amending Then
            'Special Account, one side
            udcInputISSHK.SetProperty(ucInputDocTypeBase.BuildMode.Modification_OneSide)
            udcInputISSHK.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification_OneSide, False)
        Else
            'Validated Account, with original
            udcInputISSHK.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
            udcInputISSHK.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)
        End If

        _udtAuditLogEntry.AddDescripton("DocNo", udcInputISSHK.TravelDocNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputISSHK.DOB)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputISSHK.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputISSHK.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputISSHK.Gender)

        _udtAuditLogEntry.WriteStartLog(LogID.LOG00008, AuditLogDesc.ValidateAccountDetailInfo)

        'English Name
        Me.udtSM = Me.udtvalidator.chkEngName(udcInputISSHK.ENameSurName, udcInputISSHK.ENameFirstName, udcInputISSHK.EHSPersonalInfoAmend.DocCode)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputISSHK.SetENameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'Gender
        Me.udtSM = Me.udtvalidator.chkGender(udcInputISSHK.Gender)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputISSHK.SetGenderError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String
        Dim dtmDOB As Date

        strDOB = udcInputISSHK.DOB

        Me.udtSM = Me.udtvalidator.chkDOB(udcInputISSHK.EHSPersonalInfoAmend.DocCode, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(udtSM) Then
            isvalid = False
            udcInputISSHK.SetDOBError(True)
            Me.udcMsgBox.AddMessage(udtSM)
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
    Private Function ValidateRectifyDetail_Common(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isValid As Boolean = True
        Dim udcInputCommon As ucInputCommon = Me.ucInputDocumentType.GetCommonControl
        Dim mode As ActionModel
        mode = CType(Session(SESS_InputMode), ActionModel)

        If mode = ActionModel.Amending Then
            'Special Account, one side
            udcInputCommon.SetProperty(ucInputDocTypeBase.BuildMode.Modification_OneSide)
            udcInputCommon.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification_OneSide, False)
        Else
            'Validated Account, with original
            udcInputCommon.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
            udcInputCommon.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)
        End If

        _udtAuditLogEntry.AddDescripton("DocumentNo", udcInputCommon.DocumentNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputCommon.DOB)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputCommon.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputCommon.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputCommon.Gender)
        _udtAuditLogEntry.AddDescripton("ExactDOB", udcInputCommon.IsExactDOB)
        _udtAuditLogEntry.WriteStartLog(LogID.LOG00008, AuditLogDesc.ValidateAccountDetailInfo)
        ' ----------------------------------------------------------------------------------------
        'Doc No.
        Me.udtSM = udtvalidator.chkDocumentNoForNonEHSDocType(udcInputCommon.DocumentNo)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputCommon.SetDocNoError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String = udcInputCommon.DOB
        Dim dtmDOB As Date

        Select Case udcInputCommon.DOB.Trim
            Case String.Empty
                ' Please input "Date of Birth".
                udtSM = New SystemMessage("990000", "E", "00003")
                udcInputCommon.SetDOBError(True)
            Case Else
                udtSM = Me.udtvalidator.chkDOB(udcInputCommon.EHSPersonalInfoAmend.DocCode, strDOB, dtmDOB, strExactDOB)

                If udtSM IsNot Nothing Then
                    udcInputCommon.SetDOBError(True)
                End If
        End Select

        If Not IsNothing(udtSM) Then
            Me.udcMsgBox.AddMessage(udtSM)
            isValid = False
        End If

        'English Name
        Me.udtSM = Me.udtvalidator.chkEngName(udcInputCommon.ENameSurName, udcInputCommon.ENameFirstName, udcInputCommon.EHSPersonalInfoAmend.DocCode)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputCommon.SetENameError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        'Gender
        Me.udtSM = Me.udtvalidator.chkGender(udcInputCommon.Gender)
        If Not IsNothing(udtSM) Then
            isValid = False
            udcInputCommon.SetGenderError(True)
            Me.udcMsgBox.AddMessage(udtSM)
        End If

        If isValid Then
            Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(udcInputCommon.EHSPersonalInfoAmend.DocCode)
            udtEHSAccountPersonalInfo.IdentityNum = udcInputCommon.DocumentNo
            udtEHSAccountPersonalInfo.ENameSurName = udcInputCommon.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputCommon.ENameFirstName
            udtEHSAccountPersonalInfo.CName = udcInputCommon.CName
            udtEHSAccountPersonalInfo.Gender = udcInputCommon.Gender
            udtEHSAccountPersonalInfo.ExactDOB = udcInputCommon.IsExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB
        End If

        Return isValid
    End Function

#End Region

#Region "Action Model"
    Public Enum ActionModel
        'For amendment of validated acc
        ReadOnly_withOriginal
        Amending_withOriginal
        ReadOnly_N_Amending_withOriginal

        'For special account
        [ReadOnly]
        Amending
        ReadOnly_N_Amending
    End Enum

    Public Enum PopupActionModel
        Remove
        Withdraw
    End Enum
#End Region

#Region "Conirm Msg Dialog (Withdraw Amendment / Remove Account)"
    Protected Sub ibtnDialogConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDialogConfirm.Click

        If Not IsNothing(Session(SESS_PopupActionMode)) Then
            Dim _PopupActionModel As PopupActionModel
            _PopupActionModel = Session(SESS_PopupActionMode)

            'Audit log
            udtEHSAccount = udteHSAccountMaintBLL.EHSAccountGetFromSession(FuncCode)

            Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
            Select Case _PopupActionModel
                Case PopupActionModel.Remove
                    Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                    Me.udtAuditLogEntry.AddDescripton("Doc_Code", txtDocCode.Text.Trim)
                    Me.udtAuditLogEntry.AddDescripton("Account_Source", udtEHSAccount.AccountSourceString)
                    Me.udtAuditLogEntry.AddDescripton("IdentityNum", udtEHSAccount.EHSPersonalInformationList.Filter(txtDocCode.Text).IdentityNum)
                    Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00030, AuditLogDesc.RemoveTempAcctClick)
                Case PopupActionModel.Withdraw
                    Me.udtAuditLogEntry.AddDescripton("(Original)AccountID", udtEHSAccount.VoucherAccID.Trim)
                    Me.udtAuditLogEntry.AddDescripton("Doc_Code", txtDocCode.Text.Trim)
                    Me.udtAuditLogEntry.AddDescripton("Account_Source", udtEHSAccount.AccountSourceString)
                    Me.udtAuditLogEntry.AddDescripton("IdentityNum", udtEHSAccount.EHSPersonalInformationList.Filter(txtDocCode.Text).IdentityNum)
                    Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00015, AuditLogDesc.ConfirmWithDrawAdmendment)
            End Select

            udtSM = Nothing
            Me.udcMsgBox.Clear()
            Me.udcInfoMsgBox.Clear()
            Dim udtHCVUUserBLL As New HCVUUserBLL
            Dim udtHCVUUser As HCVUUserModel = udtHCVUUserBLL.GetHCVUUser

            Me.ModalPopupExtenderCancelAmend.Hide()

            Dim strUpdateBy As String = udtHCVUUser.UserID
            udtSM = Nothing

            Try
                Select Case _PopupActionModel
                    Case PopupActionModel.Remove

                        'Audit Log
                        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
                        Me.udtAuditLogEntry.AddDescripton("Doc_Code", txtDocCode.Text.Trim)
                        Me.udtAuditLogEntry.AddDescripton("IdentityNum", udtEHSAccount.EHSPersonalInformationList.Filter(txtDocCode.Text).IdentityNum)
                        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00031, AuditLogDesc.ConfirmRemoveTempAcct)

                        Me.udteHSAccountMaintBLL.RemoveTempAcct(udtEHSAccount, strUpdateBy)
                        udtSM = New SystemMessage("010301", SeverityCode.SEVI, MsgCode.MSG00005)

                        If Not IsNothing(udtSM) Then
                            Me.udcInfoMsgBox.AddMessage(udtSM)
                            Me.udcInfoMsgBox.BuildMessageBox()
                            Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Complete

                            Me.mveHSAccount.ActiveViewIndex = intComplete
                        End If
                        'Audit Log
                        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
                        Me.udtAuditLogEntry.AddDescripton("Doc_Code", txtDocCode.Text.Trim)
                        Me.udtAuditLogEntry.AddDescripton("IdentityNum", udtEHSAccount.EHSPersonalInformationList.Filter(txtDocCode.Text).IdentityNum)
                        Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00032, AuditLogDesc.ConfirmRemoveTempAcctSuccess)

                    Case PopupActionModel.Withdraw

                        'Audit Log
                        Me.udtAuditLogEntry.AddDescripton("(Original)AccountID", udtEHSAccount.VoucherAccID.Trim)
                        Me.udtAuditLogEntry.AddDescripton("Doc_Code", txtDocCode.Text.Trim)
                        Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
                        Me.udtAuditLogEntry.AddDescripton("IdentityNum", udtEHSAccount.EHSPersonalInformationList.Filter(txtDocCode.Text).IdentityNum)
                        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00016, AuditLogDesc.WithDrawAdmendment)

                        Me.udtEHSAccount_Amendment = udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FuncCode)
                        udteHSAccountMaintBLL.WithdrawAmendment(udtEHSAccount_Amendment, udtEHSAccount, Me.txtDocCode.Text.Trim, strUpdateBy)
                        'create complete message
                        Me.udtSM = New Common.ComObject.SystemMessage("010303", "I", LogID.LOG00002)
                        If Not IsNothing(udtSM) Then
                            Me.udcInfoMsgBox.AddMessage(udtSM)
                            Me.udcInfoMsgBox.BuildMessageBox()
                            Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Complete

                            udtAuditLogEntry.AddDescripton("(Original)AccountID", udtEHSAccount.VoucherAccID)
                            Me.udtAuditLogEntry.AddDescripton("Doc_Code", txtDocCode.Text.Trim)
                            Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
                            Me.udtAuditLogEntry.AddDescripton("IdentityNum", udtEHSAccount.EHSPersonalInformationList.Filter(txtDocCode.Text).IdentityNum)
                            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00017, AuditLogDesc.WithDrawAdmendmentComplete)
                            Me.mveHSAccount.ActiveViewIndex = intComplete
                            panFilter.Visible = False
                        End If

                End Select

            Catch eSQL As SqlClient.SqlException

                If eSQL.Number = 50000 Then
                    Me.udtSM = New Common.ComObject.SystemMessage("990001", "D", eSQL.Message)
                    Me.udcMsgBox.AddMessage(Me.udtSM)

                    'Audit Log
                    Select Case _PopupActionModel
                        Case PopupActionModel.Remove
                            Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                            Me.udtAuditLogEntry.AddDescripton("Doc_Code", txtDocCode.Text.Trim)
                            Me.udtAuditLogEntry.AddDescripton("IdentityNum", udtEHSAccount.EHSPersonalInformationList.Filter(txtDocCode.Text).IdentityNum)
                            Me.udcMsgBox.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00033, AuditLogDesc.ConfirmRemoveTempAcctFail)
                        Case PopupActionModel.Withdraw
                            Me.udtAuditLogEntry.AddDescripton("(Original)AccountID", udtEHSAccount.VoucherAccID.Trim)
                            Me.udtAuditLogEntry.AddDescripton("Doc_Code", txtDocCode.Text.Trim)
                            Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
                            Me.udtAuditLogEntry.AddDescripton("IdentityNum", udtEHSAccount.EHSPersonalInformationList.Filter(txtDocCode.Text).IdentityNum)
                            Me.udcMsgBox.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00018, AuditLogDesc.WithDrawAdmendmentFail)
                    End Select

                    'rebind the content
                    udtEHSAccount_Amendment = Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FuncCode)
                    BindPersonalInfo(udtEHSAccount, udtEHSAccount_Amendment, Nothing, txtDocCode.Text, True)
                    Session(SESS_RequirePageLoadPersonalBind) = False
                Else
                    'Audit Log
                    Select Case _PopupActionModel
                        Case PopupActionModel.Remove
                            Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                            Me.udtAuditLogEntry.AddDescripton("Doc_Code", txtDocCode.Text.Trim)
                            Me.udtAuditLogEntry.AddDescripton("IdentityNum", udtEHSAccount.EHSPersonalInformationList.Filter(txtDocCode.Text).IdentityNum)
                            Me.udcMsgBox.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00033, AuditLogDesc.ConfirmRemoveTempAcctFail)
                        Case PopupActionModel.Withdraw
                            'log update fail
                            Me.udtAuditLogEntry.AddDescripton("(Original)AccountID", udtEHSAccount.VoucherAccID.Trim)
                            Me.udtAuditLogEntry.AddDescripton("Doc_Code", txtDocCode.Text.Trim)
                            Me.udtAuditLogEntry.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
                            Me.udtAuditLogEntry.AddDescripton("IdentityNum", udtEHSAccount.EHSPersonalInformationList.Filter(txtDocCode.Text).IdentityNum)
                            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00018, AuditLogDesc.WithDrawAdmendmentFail)
                    End Select

                    'rebind the content
                    udtEHSAccount_Amendment = Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FuncCode)
                    BindPersonalInfo(udtEHSAccount, udtEHSAccount_Amendment, Nothing, txtDocCode.Text, True)
                    Session(SESS_RequirePageLoadPersonalBind) = False
                    Throw eSQL
                End If
            Catch ex As Exception
                Throw ex
            End Try
        Else
            Throw New Exception("EHSAccount Rectification: User action is nothing in Confirmation Popup")
        End If

    End Sub

    Protected Sub ibtnDialogCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDialogCancel.Click

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)

        Dim _PopupActionModel As PopupActionModel
        _PopupActionModel = Session(SESS_PopupActionMode)

        Select Case _PopupActionModel
            Case PopupActionModel.Remove
                Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00034, AuditLogDesc.CancelRemoveTempAcct)
            Case PopupActionModel.Withdraw
                Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00019, AuditLogDesc.CancelWithDrawAdmendment)
        End Select

        Me.ModalPopupExtenderCancelAmend.Hide()

        'rebind after post back
        udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FuncCode)
        udtEHSAccount_Amendment = Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FuncCode)
        BindPersonalInfo(udtEHSAccount, udtEHSAccount_Amendment, Nothing, txtDocCode.Text, True)
        Session(SESS_RequirePageLoadPersonalBind) = False

        If udtEHSAccount.EHSPersonalInformationList.Filter(Me.txtDocCode.Text.Trim).Deceased Then
            Session(SESS_DetailPageShowDeceased) = True
        Else
            Session(SESS_DetailPageShowDeceased) = False
        End If
    End Sub
#End Region

#Region "Confirm Details"
    'for rectifying Account
    Private Sub SetupConfirmAccount(ByVal _udtEHSAccount_Rectify As EHSAccountModel)
        If Not IsNothing(_udtEHSAccount_Rectify) Then
            Me.udcConfirmAccount.DocumentType = Me.txtDocCode.Text.Trim
            Me.udcConfirmAccount.EHSPersonalInformation = _udtEHSAccount_Rectify.getPersonalInformation(Me.txtDocCode.Text.Trim)
            Me.udcConfirmAccount.MaskIdentityNo = False
            Me.udcConfirmAccount.Vertical = True
            Me.udcConfirmAccount.Width = 220
            Me.udcConfirmAccount.Build()
        End If
    End Sub

#End Region

#Region "Pop-up event"
    Public Overrides Sub GridViewHeaderImage_Click(ByVal sender As Object, ByVal e As Common.Component.SortedGridviewHeader.SortedGridviewHeaderModel.GridViewHeaderImageEventArgs)
        popupDocTypeHelp.Show()
        udcDocTypeLegend.BindDocType(Session("language"))
    End Sub

    Protected Sub ibtnCloseDocTypeHelp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCloseDocTypeHelp.Click
        popupDocTypeHelp.Hide()
    End Sub

#End Region


#Region "Implement IWorkingData (CRE11-004)"

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Account which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Return Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FuncCode)
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

End Class