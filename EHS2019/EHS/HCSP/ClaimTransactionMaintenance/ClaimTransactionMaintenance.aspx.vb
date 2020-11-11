Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.ClaimTrans
Imports Common.Component.DataEntryUser
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component.EHSTransaction
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.ReasonForVisit
Imports Common.Component.Scheme
Imports Common.Component.Scheme.SchemeClaimModel
Imports Common.Component.ServiceProvider
Imports Common.Component.SortedGridviewHeader
Imports Common.Component.UserAC
Imports Common.Component.RedirectParameter
Imports Common.ComObject
Imports Common.Format
Imports Common.Validation
Imports HCSP.BLL
Imports System.Globalization
Imports System.Threading
Imports System.Web.Script.Services.ScriptMethodAttribute
Imports AjaxControlToolkit
Imports System.Web.Services
Imports System.Web.Security.AntiXss

Partial Public Class ClaimTransactionMaintenance
    Inherits BasePageWithGridView

    ' FunctionCode = FunctCode.FUNT020301

#Region "Private Classes"

    Private Class AuditLogDescription
        Public Const Load As String = "Claim Transaction Management load" '00000
        Public Const Search As String = "Search" '00001
        Public Const SearchSuccessful As String = "Search successful" '00002
        Public Const SearchFail As String = "Search fail" '00003
        Public Const ViewDetail As String = "View Detail" '00004
        Public Const VoidTransactionSuccessful As String = "Void Transaction successful" '00005
        Public Const VoidTransactionFail As String = "Void Transaction fail" '00006
        Public Const VoidTransaction = "Void Transaction" '00007
        Public Const LoadFromRectification As String = "Claim Transaction Management Loaded Form Rectification " '00008 (Depreciated)
        Public Const ReturnClick As String = "Return click" '00009
        Public Const BackClick As String = "Back click" '00010
        Public Const DetailBackClick As String = "Detail Back click" '00011
        Public Const CancelConfirmVoidClick As String = "Cancel Confirm Void click" '00012
        Public Const VoidClick As String = "Void click" '00013
        Public Const ShowLegend As String = "Show Legend" '00014
        Public Const ModifyClick As String = "Modify Click" '00015
        Public Const ModifyNextClick As String = "Modify - Next Click" '00016
        Public Const ModifyBackClick As String = "Modify - Back Click" '00017
        Public Const ModifySaveClick As String = "Modify - Save Click" '00018
        Public Const ModifyConfirmClick As String = "Modify - Confirm Click" '00019
        Public Const ModifyCancelClick As String = "Modify - Cancel Click" '00020
    End Class

    Private Class ErrorMessageBoxHeaderKey
        Public Const SearchFail As String = "SearchFail"
        Public Const UpdateFail As String = "UpdateFail"
        Public Const ValidationFail As String = "ValidationFail"
    End Class

    Private Class ViewIndex
        Public Const InputSearch As Integer = 0
        Public Const TransactionList As Integer = 1
        Public Const Detail As Integer = 2
        Public Const CompleteVoid As Integer = 3
    End Class

    Private Class ViewIndexDetailAction
        Public Const Button As Integer = 0
        Public Const Void As Integer = 1
        Public Const Modify As Integer = 2
        Public Const ModifyConfirm As Integer = 3
        Public Const CompleteModify As Integer = 4
    End Class

    <Serializable()> Private Class SearchCriteria
        Public strSPID As String
        Public strDataEntryAccount As String
        Public intPracticeNo As Integer
        Public intBankNo As Integer
        Public strStatus As String
        Public dtmTransactionDateFrom As String
        Public dtmTransactionDateTo As String
        Public strTransactionNo As String
        Public strSchemeCode As String
    End Class

#End Region

#Region "Fields"

    Private udtClaimTranBLL As New ClaimTransBLL
    Private udtEHSTransactionBLL As New EHSTransactionBLL
    Private udtFormatter As New Formatter
    Private udtGeneralFunction As New GeneralFunction
    Private udtPracticeAcctBLL As New PracticeBankAcctBLL
    Private udtReasonForVisitBLL As New ReasonForVisitBLL
    Private udtSchemeClaimBLL As New SchemeClaimBLL
    Private udtTransactionMaintenanceBLL As New TransactionMaintenanceBLL
    Private udtValidator As New Validator

#End Region

#Region "Constants"

    Private Const VoidablePeriodHour As Integer = 24
    Private ValidationFail As String = "ValidationFail"
#End Region

#Region "Session Constants"

    Private Const SESS_PracticeSchemeInfoList As String = "202301_PracticeSchemeInfoList"
    Private Const SESS_PracticeDropDownList As String = "020301_PracticeDropDownList"
    Private Const SESS_TransactionDataTable As String = "020301_TransactionDataTable"
    Private Const SESS_HideDetailBackButton As String = "020301_HideDetailBackButton"
    Private Const SESS_TransactionNo As String = "020301_TransactionNo"
    Private Const SESS_SearchCriteria As String = "020301_SearchCriteria"
    Private Const SESS_BuildRecordSummary As String = "020301_BuildRecordSummary"
    Private Const SESS_EHSTransaction As String = "020301_EHSTransaction"
    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    ' -----------------------------------------------------------------------------------------
    Private Const SESS_EHSTransactionOriginal As String = "020301_EHSTransactionOriginal"
    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]
    'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Const SESS_StatusDropDownList As String = "020301_StatusDropDownList"
    'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        If Not IsPostBack Then
            ' Init the Active View in Page Load only
            Me.MultiViewClaimTranManagement.ActiveViewIndex = 0
            Me.MultiViewDetailAction.ActiveViewIndex = 0
        End If

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

        'Get Current USer Account for check Session Expired
        Dim udtUserAC As UserACModel
        udtUserAC = UserACBLL.GetUserAC

        If Not IsPostBack Then
            FunctionCode = FunctCode.FUNT020301

            If Not IsNothing(Session("FromRectification")) AndAlso Session("FromRectification") = "Y" Then
                Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
                udtAuditLogEntry.WriteLog(LogID.LOG00008, AuditLogDescription.LoadFromRectification)

                Session(SESS_HideDetailBackButton) = "Y"
                Session(SESS_TransactionNo) = Session("TransactionNo")

                Session.Remove("TransactionNo")
                Session.Remove("FromRectification")

                MultiViewClaimTranManagement.ActiveViewIndex = ViewIndex.Detail

            Else
                Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
                udtAuditLogEntry.WriteLog(LogID.LOG00000, AuditLogDescription.Load)

                MultiViewClaimTranManagement.ActiveViewIndex = ViewIndex.InputSearch
                Session(SESS_HideDetailBackButton) = "N"

                'CRE16-026 (Add PCV13)(Fix claim transaction management worng button display) [Start][DICKSON]'
				'Clear Session Return From value
                Dim udtRedirectParameterBLL As New RedirectParameterBLL
                udtRedirectParameterBLL.RemoveReturnFromSession()
                'CRE16-026 (Add PCV13)(Fix claim transaction management worng button display) [End][DICKSON]'
            End If

            BindControl()

            HandleRedirectAction()      ' CRE11-024-02 HCVS

        Else
            'CRE16-026 (Add PCV13)(Fix claim transaction management worng button display) [Start][DICKSON]'
			'If ActiveViewIndex is 1 or 2, clear Session Return From value
            If MultiViewClaimTranManagement.ActiveViewIndex = ViewIndex.InputSearch Or MultiViewClaimTranManagement.ActiveViewIndex = ViewIndex.TransactionList Then
                Dim udtRedirectParameterBLL As New RedirectParameterBLL
                udtRedirectParameterBLL.RemoveReturnFromSession()
            End If
            'CRE16-026 (Add PCV13)(Fix claim transaction management worng button display) [End][DICKSON]'
        End If

        If MultiViewClaimTranManagement.ActiveViewIndex = ViewIndex.Detail Then
            BuildDetail(Session(SESS_TransactionNo))
        End If

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------
        HandleRedirectButtons()
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        If Me.ibtnDetailBack.Visible Then
            Me.SetFocus(Me.ibtnDetailBack)
        Else
            Me.SetFocus(Me.ibtnReturnBtn)
        End If

        AddHandler udcClaimTranEnquiry.VaccineLegendClicked1, AddressOf udcClaimTranEnquiry_VaccineLegendClicked

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If IsPostBack Then
            Dim controlID As String = Page.Request.Params.Get(PostBackEventTarget)
            If controlID = SelectTradChinese OrElse controlID = SelectEnglish OrElse controlID = SelectSimpChinese Then
                RenderLanguage()
            End If
        End If
    End Sub

    Private Sub BindControl()
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtSP As ServiceProviderModel = Nothing
        Dim udtDataEntry As DataEntryUserModel = Nothing
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        ' Bind Practice
        Dim dtPracticeList As DataTable = Nothing
        Dim dtStatusList As DataTable = Nothing
        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC

        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'Dim udtSP As ServiceProviderModel = udtUserAC
            udtSP = udtUserAC
            dtPracticeList = udtPracticeAcctBLL.getAllPractice(udtSP.SPID, PracticeBankAcctBLL.PracticeDisplayType.Practice)
            FilterPracticeList(dtPracticeList, udtSP.PracticeList)
            dtStatusList = Status.GetDescriptionListFromDBEnumCode("HCSPClaimTransManagementStatusSP", True)
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
        Else
            'Dim udtDataEntry As DataEntryUserModel = udtUserAC
            udtDataEntry = udtUserAC
            dtPracticeList = udtPracticeAcctBLL.getAllPractice(udtDataEntry.SPID, udtDataEntry.DataEntryAccount, PracticeBankAcctBLL.PracticeDisplayType.Practice)
            'ddlSearchStatus.SelectedValue = ClaimTransStatus.Pending
            ' ddlSearchStatus.Enabled = False

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            udtDataEntry.ServiceProvider.FilterByHCSPSubPlatform(Me.SubPlatform)
            FilterPracticeList(dtPracticeList, udtDataEntry.ServiceProvider.PracticeList)
            dtStatusList = Status.GetDescriptionListFromDBEnumCode("HCSPClaimTransManagementStatusDataEntry", True)
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
        End If

        ddlSearchPractice.Items.Clear()
        ddlSearchPractice.DataSource = dtPracticeList


        If Session("language") = TradChinese OrElse Session("language") = SimpChinese Then
            ddlSearchPractice.DataTextField = BLL.PracticeBankAcctBLL.PracticeDisplayField.Display_Chi
        Else
            ddlSearchPractice.DataTextField = BLL.PracticeBankAcctBLL.PracticeDisplayField.Display_Eng
        End If

        Session(SESS_PracticeDropDownList) = dtPracticeList

        ddlSearchPractice.DataValueField = "BankAccountKey"
        ddlSearchPractice.DataBind()
        ddlSearchPractice.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), String.Empty))

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'For i As Integer = 1 To ddlSearchStatus.Items.Count - 1
        '    Select Case ddlSearchStatus.Items(i).Value
        '        Case ClaimTransStatus.Incomplete
        '            ddlSearchStatus.Items(i).Text = Me.GetGlobalResourceObject("Text", "Incomplete")
        '        Case ClaimTransStatus.Active
        '            ddlSearchStatus.Items(i).Text = Me.GetGlobalResourceObject("Text", "ReadytoReimburse")
        '        Case ClaimTransStatus.Inactive
        '            ddlSearchStatus.Items(i).Text = Me.GetGlobalResourceObject("Text", "Voided")
        '        Case ClaimTransStatus.Pending
        '            ddlSearchStatus.Items(i).Text = Me.GetGlobalResourceObject("Text", "PendingConfirmation")
        '        Case ClaimTransStatus.PendingVRValidate
        '            ddlSearchStatus.Items(i).Text = Me.GetGlobalResourceObject("Text", "PendingVoucherAccountValidation")
        '        Case ClaimTransStatus.Reimbursed
        '            ddlSearchStatus.Items(i).Text = Me.GetGlobalResourceObject("Text", "Reimbursed")
        '        Case ClaimTransStatus.Suspended
        '            ddlSearchStatus.Items(i).Text = Me.GetGlobalResourceObject("Text", "ClaimSuspended")
        '        Case ClaimTransStatus.ManualReimbursedClaim
        '            ddlSearchStatus.Items(i).Text = Me.GetGlobalResourceObject("Text", "ManualReimbursedClaim")
        '            ' CRE13-001 EHAPP [Start][Karl]
        '            ' -----------------------------------------------------------------------------------------
        '        Case ClaimTransStatus.Joined
        '            ddlSearchStatus.Items(i).Text = Me.GetGlobalResourceObject("Text", "Joined")
        '            ' CRE13-001 EHAPP [End][Karl]
        '    End Select
        'Next
        If Not dtStatusList Is Nothing Then
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Session(SESS_StatusDropDownList) = dtStatusList

            If Me.SubPlatform.Equals(EnumHCSPSubPlatform.CN) Then
                Dim strFilterCriteria As String = String.Empty
                Dim dtResStatusList As DataTable
                Dim drStatusList() As DataRow

                strFilterCriteria = "Status_Value NOT IN ('" & ClaimTransStatus.Incomplete & "', '" & ClaimTransStatus.Joined & "')"

                drStatusList = dtStatusList.Select(strFilterCriteria)

                dtResStatusList = dtStatusList.Clone

                For Each dr As DataRow In drStatusList
                    dtResStatusList.ImportRow(dr)
                Next

                ddlSearchStatus.Items.Clear()
                Session(SESS_StatusDropDownList) = dtResStatusList
                ddlSearchStatus.DataSource = dtResStatusList
            Else
                ddlSearchStatus.Items.Clear()
                Session(SESS_StatusDropDownList) = dtStatusList
                ddlSearchStatus.DataSource = dtStatusList
            End If

            'If Session("language") = TradChinese OrElse Session("language") = SimpChinese Then
            '    ddlSearchStatus.DataTextField = "Status_Description_Chi"
            'Else
            '    ddlSearchStatus.DataTextField = "Status_Description"
            'End If
            Select Case Session("language")
                Case English
                    ddlSearchStatus.DataTextField = "Status_Description"
                Case TradChinese
                    ddlSearchStatus.DataTextField = "Status_Description_Chi"
                Case SimpChinese
                    ddlSearchStatus.DataTextField = "Status_Description_CN"
                Case Else
                    ddlSearchStatus.DataTextField = "Status_Description"
            End Select
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            ddlSearchStatus.DataValueField = "Status_Value"
            ddlSearchStatus.DataBind()
        End If

        ' Bind Status
        ddlSearchStatus.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), String.Empty))

        'If udtUserAC.UserType <> SPAcctType.ServiceProvider Then
        '    ' CRE13-001 EHAPP [Start][Karl]
        '    ' -----------------------------------------------------------------------------------------
        '    ddlSearchStatus.Items.RemoveAt(8)
        '    ' CRE13-001 EHAPP [End][Karl]
        '    ' -----------------------------------------------------------------------------------------
        '    ddlSearchStatus.Items.RemoveAt(7)
        '    ddlSearchStatus.Items.RemoveAt(6)
        '    ddlSearchStatus.Items.RemoveAt(5)
        '    ddlSearchStatus.Items.RemoveAt(4)
        '    ddlSearchStatus.Items.RemoveAt(3)
        'End If
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        ' Set the default Transaction Time From/To to today
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'txtSearchTransactionDateFrom.Text = Now.ToString("dd-MM-yyyy")
        'txtSearchTransactionDateTo.Text = Now.ToString("dd-MM-yyyy")
        Dim udtFormatter As New Formatter
        Dim udtSubPlatformBLL As New SubPlatformBLL
        txtSearchTransactionDateFrom.Text = Now.ToString(udtFormatter.EnterDateFormat(udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform)))
        txtSearchTransactionDateTo.Text = Now.ToString(udtFormatter.EnterDateFormat(udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform)))
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]


        ' Bind Scheme
        Dim udtServiceProvider As ServiceProviderModel = Nothing
        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'udtServiceProvider = udtUserAC
            udtServiceProvider = udtSP

        Else
            'Dim udtDataEntry As DataEntryUserModel = udtUserAC
            'Dim udtSPBLL As New ServiceProviderBLL
            'udtServiceProvider = udtSPBLL.GetServiceProviderBySPID(New Common.DataAccess.Database, udtDataEntry.SPID)
            udtServiceProvider = udtDataEntry.ServiceProvider
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
        End If

        Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = New PracticeSchemeInfoModelCollection

        If ddlSearchPractice.SelectedIndex = 0 Then
            For Each udtPractice As PracticeModel In udtServiceProvider.PracticeList.Values
                For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                    udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                Next
            Next
        Else
            For Each udtPractice As PracticeModel In udtServiceProvider.PracticeList.Values
                If udtPractice.DisplaySeq = ddlSearchPractice.SelectedValue.Trim.Split("-")(1).Trim Then
                    For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                        udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                    Next
                End If
            Next
        End If

        ' Save the practice scheme info list to session for re-render language
        Session(SESS_PracticeSchemeInfoList) = udtPracticeSchemeInfoList

        Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = udtSchemeClaimBLL.ConvertSchemeClaimCodeFromSchemeEnrol(udtPracticeSchemeInfoList)

        ddlScheme.DataSource = udtSchemeClaimModelCollection
        ddlScheme.DataValueField = "SchemeCode"
        If Session("language") = TradChinese Then
            ddlScheme.DataTextField = "SchemeDescChi"
        ElseIf Session("language") = SimpChinese Then
            ddlScheme.DataTextField = "SchemeDescCN"
        Else
            ddlScheme.DataTextField = "SchemeDesc"
        End If

        ddlScheme.DataBind()

        ddlScheme.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), String.Empty))

    End Sub

    Private Sub RenderLanguage()
        Dim strTranDateFrom As String = String.Empty
        Dim strTranDateTo As String = String.Empty

        Dim dtToday As DateTime = udtGeneralFunction.GetSystemDateTime
        Dim dtTrandateFrom As Date = dtToday
        Dim dtTrandateTo As Date = dtToday
        Dim strDateFormat As String = String.Empty

        ddlSearchPractice.Items(0).Text = Me.GetGlobalResourceObject("Text", "Any")
        ddlSearchStatus.Items(0).Text = Me.GetGlobalResourceObject("Text", "Any")
        If txtSearchTranNoPrefix.Text.Trim.Equals(String.Empty) And Me.txtSearchTranNoContent.Text.Trim.Equals(String.Empty) And Me.txtSearchTranNochkdgt.Text.Trim.Equals(String.Empty) Then
            lblTargetTranNo.Text = Me.GetGlobalResourceObject("Text", "Any")
        End If

        SwitchGridViewLanguage()

        For i As Integer = 1 To ddlSearchStatus.Items.Count - 1
            Select Case ddlSearchStatus.Items(i).Value
                Case Common.Component.ClaimTransStatus.Incomplete
                    Me.ddlSearchStatus.Items(i).Text = Me.GetGlobalResourceObject("Text", "Incomplete")
                Case Common.Component.ClaimTransStatus.Active
                    Me.ddlSearchStatus.Items(i).Text = Me.GetGlobalResourceObject("Text", "ReadytoReimburse")
                Case Common.Component.ClaimTransStatus.Inactive
                    Me.ddlSearchStatus.Items(i).Text = Me.GetGlobalResourceObject("Text", "Voided")
                Case Common.Component.ClaimTransStatus.Pending
                    Me.ddlSearchStatus.Items(i).Text = Me.GetGlobalResourceObject("Text", "PendingConfirmation")
                Case Common.Component.ClaimTransStatus.PendingVRValidate
                    Me.ddlSearchStatus.Items(i).Text = Me.GetGlobalResourceObject("Text", "PendingVoucherAccountValidation")
                Case Common.Component.ClaimTransStatus.Reimbursed
                    Me.ddlSearchStatus.Items(i).Text = Me.GetGlobalResourceObject("Text", "Reimbursed")
                Case Common.Component.ClaimTransStatus.Suspended
                    Me.ddlSearchStatus.Items(i).Text = Me.GetGlobalResourceObject("Text", "ClaimSuspended")
                Case Common.Component.ClaimTransStatus.ManualReimbursedClaim
                    Me.ddlSearchStatus.Items(i).Text = Me.GetGlobalResourceObject("Text", "ManualReimbursedClaim")
                    ' CRE13-001 EHAPP [Start][Karl]
                    ' -----------------------------------------------------------------------------------------
                Case ClaimTransStatus.Joined
                    ddlSearchStatus.Items(i).Text = Me.GetGlobalResourceObject("Text", "Joined")
                    ' CRE13-001 EHAPP [End][Karl]
            End Select
        Next

        ' Handle Practice Bank Change Language
        Dim strPracticeBankSelected As String = Me.ddlSearchPractice.SelectedValue
        Dim dtPracticeList As DataTable = CType(Session(SESS_PracticeDropDownList), DataTable)
        Me.ddlSearchPractice.Items.Clear()
        Me.ddlSearchPractice.DataSource = dtPracticeList

        If LCase(Session("language")) = TradChinese OrElse LCase(Session("language")) = SimpChinese Then
            Me.ddlSearchPractice.DataTextField = BLL.PracticeBankAcctBLL.PracticeDisplayField.Display_Chi
        Else
            Me.ddlSearchPractice.DataTextField = BLL.PracticeBankAcctBLL.PracticeDisplayField.Display_Eng
        End If

        Me.ddlSearchPractice.DataValueField = "BankAccountKey"
        Me.ddlSearchPractice.DataBind()
        Me.ddlSearchPractice.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), String.Empty))

        Me.ddlSearchPractice.SelectedValue = strPracticeBankSelected

        Me.lblTargetPractice.Text = Me.ddlSearchPractice.SelectedItem.ToString
        Me.lblTargetPracticeChi.Text = Me.ddlSearchPractice.SelectedItem.ToString

        If LCase(Session("language")) = TradChinese OrElse LCase(Session("language")) = SimpChinese Then
            lblTargetPractice.Visible = False
            lblTargetPracticeChi.Visible = True
        Else
            lblTargetPractice.Visible = True
            lblTargetPracticeChi.Visible = False
        End If

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        'Handle Status Change Language
        '-----------------------------------------------------------------------------------------
        Dim strStatusSelected As String = Me.ddlSearchStatus.SelectedValue
        Dim dtStatusList As DataTable = CType(Session(SESS_StatusDropDownList), DataTable)
        Me.ddlSearchStatus.Items.Clear()
        Me.ddlSearchStatus.DataSource = dtStatusList

        Select Case Session("language")
            Case English
                ddlSearchStatus.DataTextField = "Status_Description"
            Case TradChinese
                ddlSearchStatus.DataTextField = "Status_Description_Chi"
            Case SimpChinese
                ddlSearchStatus.DataTextField = "Status_Description_CN"
            Case Else
                ddlSearchStatus.DataTextField = "Status_Description"
        End Select

        ddlSearchStatus.DataValueField = "Status_Value"
        ddlSearchStatus.DataBind()

        ddlSearchStatus.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), String.Empty))

        Me.ddlSearchStatus.SelectedValue = strStatusSelected
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        Me.lblTargetStatus.Text = Me.ddlSearchStatus.SelectedItem.ToString

        ' Handle Scheme Change Language
        If IsPostBack Then
            Dim strSchemeSelected As String = ddlScheme.SelectedValue
            Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = udtSchemeClaimBLL.ConvertSchemeClaimCodeFromSchemeEnrol(Session(SESS_PracticeSchemeInfoList))

            ddlScheme.Items.Clear()
            ddlScheme.DataSource = udtSchemeClaimModelCollection

            If Session("language") = TradChinese Then
                ddlScheme.DataTextField = "SchemeDescChi"
            ElseIf Session("language") = SimpChinese Then
                ddlScheme.DataTextField = "SchemeDescCN"
            Else
                ddlScheme.DataTextField = "SchemeDesc"
            End If

            ddlScheme.DataBind()
            ddlScheme.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), String.Empty))
            ddlScheme.SelectedValue = strSchemeSelected

            ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [Start][Winnie]
            lblTargetScheme.Text = AntiXssEncoder.HtmlEncode(ddlScheme.SelectedItem.Text.Trim(), True)
            ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [End][Winnie]
        End If

        'If strTranDateFrom.Equals(String.Empty) And strTranDateTo.Equals(String.Empty) Then
        strTranDateFrom = Me.txtSearchTransactionDateFrom.Text

        If Not strTranDateFrom.Equals(String.Empty) Then
            strTranDateFrom = udtFormatter.convertDate(strTranDateFrom, "E")
            If Not strTranDateFrom.Equals(String.Empty) Then
                dtTrandateFrom = CType(strTranDateFrom, DateTime)
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim udtSubPlatformBLL As New SubPlatformBLL
                'strTranDateFrom = udtFormatter.formatDate(dtTrandateFrom)
                strTranDateFrom = udtFormatter.formatDisplayDate(dtTrandateFrom, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            End If
        End If

        strTranDateTo = Me.txtSearchTransactionDateTo.Text
        If Not strTranDateTo.Equals(String.Empty) Then
            strTranDateTo = udtFormatter.convertDate(strTranDateTo, "E")
            If Not strTranDateTo.Equals(String.Empty) Then
                dtTrandateTo = CType(strTranDateTo, DateTime)
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim udtSubPlatformBLL As New SubPlatformBLL
                'strTranDateTo = udtFormatter.formatDate(dtTrandateTo)
                strTranDateTo = udtFormatter.formatDisplayDate(dtTrandateTo, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            End If
        End If

        'End If

        lblTargetTranDate.Text = Me.GetGlobalResourceObject("Text", "From") + " " + strTranDateFrom + " " + Me.GetGlobalResourceObject("Text", "To") + " " + strTranDateTo
        lblTargetStatus.Text = ddlSearchStatus.SelectedItem.ToString

        lblVoidReasonText.Text = Me.GetGlobalResourceObject("Text", "VoidReason")
        udcClaimTranEnquiry.chgLanguage()

    End Sub

#End Region

    Protected Sub ddlSearchPractice_SelectedIndexChange(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC
        Dim udtServiceProvider As ServiceProviderModel = Nothing

        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            udtServiceProvider = udtUserAC
        Else
            Dim udtDataEntry As DataEntryUserModel = udtUserAC
            Dim udtSPBLL As New ServiceProviderBLL
            udtServiceProvider = udtSPBLL.GetServiceProviderBySPID(New Common.DataAccess.Database, udtDataEntry.SPID)
        End If

        Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = New PracticeSchemeInfoModelCollection

        If ddlSearchPractice.SelectedIndex = 0 Then
            For Each udtPractice As PracticeModel In udtServiceProvider.PracticeList.Values
                For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                    udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                Next
            Next
        Else
            For Each udtPractice As PracticeModel In udtServiceProvider.PracticeList.Values
                If udtPractice.DisplaySeq = ddlSearchPractice.SelectedValue.Trim.Split("-")(1).Trim Then
                    For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                        udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                    Next
                End If
            Next
        End If

        ' Save the practice scheme info list to session for re-render language
        Session(SESS_PracticeSchemeInfoList) = udtPracticeSchemeInfoList

        Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = udtSchemeClaimBLL.ConvertSchemeClaimCodeFromSchemeEnrol(udtPracticeSchemeInfoList)

        ddlScheme.DataSource = udtSchemeClaimModelCollection
        ddlScheme.DataValueField = "SchemeCode"

        If Session("language") = TradChinese Then
            ddlScheme.DataTextField = "SchemeDescChi"
        ElseIf Session("language") = SimpChinese Then
            ddlScheme.DataTextField = "SchemeDescCN"
        Else
            ddlScheme.DataTextField = "SchemeDesc"
        End If

        ddlScheme.DataBind()

        ddlScheme.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), String.Empty))
    End Sub

    '

    Private Sub BuildDetail(Optional ByVal strTransactionNo As String = Nothing)
        ibtnDetailBack.Visible = True
        ibtnVoid.Visible = False
        ibtnModify.Visible = False
        ibtnModify.Visible = False

        Dim udtEHSTransaction As EHSTransactionModel = Nothing

        If IsNothing(strTransactionNo) Then
            udtEHSTransaction = Session(SESS_EHSTransaction)
            If IsNothing(udtEHSTransaction) Then udtEHSTransaction = (New EHSTransactionBLL).LoadClaimTran(strTransactionNo)

            If IsNothing(Session(SESS_EHSTransactionOriginal)) Then
                Session(SESS_EHSTransactionOriginal) = (New EHSTransactionBLL).LoadClaimTran(udtEHSTransaction.TransactionID)
            End If
        Else
            If IsNothing(Session(SESS_EHSTransactionOriginal)) Then
                Session(SESS_EHSTransactionOriginal) = (New EHSTransactionBLL).LoadClaimTran(strTransactionNo)
            End If
        End If

        udcClaimTranEnquiry.buildClaimObject(strTransactionNo, udtEHSTransaction, Session(SESS_EHSTransactionOriginal), True, True)
        udcClaimTranEnquiry.chgLanguage()

        ' Save the EHSTransactionModel to session
        If Not IsNothing(strTransactionNo) Then Session(SESS_EHSTransaction) = udtEHSTransaction

        ' Handle visible of buttons
        If Session(SESS_HideDetailBackButton) = "Y" Then
            ibtnDetailBack.Visible = False
        End If

        If udtTransactionMaintenanceBLL.CheckTransactionVoidable(udtEHSTransaction) Then
            ibtnVoid.Visible = True
        End If

        If udtTransactionMaintenanceBLL.CheckTransactionEditable(udtEHSTransaction, UserACBLL.GetUserAC) Then
            ibtnModify.Visible = True
        End If

        MultiViewClaimTranManagement.ActiveViewIndex = ViewIndex.Detail

        Me._ScriptManager.SetFocus(ibtnDetailBack)

    End Sub

    ''' <summary>
    ''' Update modified transaction from Modify view to Read only view
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub UpdateDetail()
        Dim udtEHSTransaction As EHSTransactionModel = Session(SESS_EHSTransaction)
        Me.udcClaimTranEnquiry.Save(udtEHSTransaction)
        BuildDetail()
    End Sub

    Private Sub SwitchTableRecordSummaryLanguage()
        If Me.SubPlatform.Equals(EnumHCSPSubPlatform.CN) Then
            trRecordSummaryTotalAmountRMB.Style.Add("display", "default")
            trRecordSummaryTotalAmountRMB_SSSCMC.Style.Add("display", "default")
            tdSummarySchemeHCVSCHN.Style.Add("display", "default")
            tdSummarySchemeSSSCMC.Style.Add("display", "default")
            tdSummarySchemeTitle.Style.Add("display", "default")
            tdSummaryIncompleteTitle.Style.Add("display", "none")
            tdSummaryIncomplete.Style.Add("display", "none")
        Else
            trRecordSummaryTotalAmountRMB.Style.Add("display", "none")
            trRecordSummaryTotalAmountRMB_SSSCMC.Style.Add("display", "none")
            tdSummarySchemeTitle.Style.Add("display", "none")
            tdSummarySchemeHCVSCHN.Style.Add("display", "none")
            tdSummarySchemeSSSCMC.Style.Add("display", "none")
        End If

    End Sub

    Private Sub SwitchGridViewLanguage()
        If LCase(Session("language")) = TradChinese Then
            gvTranList.Columns(2).Visible = False   'lblTranListTranDtm
            gvTranList.Columns(3).Visible = True    'lblTranListTranDtm_Chi

            gvTranList.Columns(8).Visible = False   'lblTranListAccountType
            gvTranList.Columns(9).Visible = True    'lblTranListAccountType_Chi
            gvTranList.Columns(10).Visible = False  'lblTranListAccountType_CN

            'gvTranList.Columns(11).Visible = False  'lblTotalAmountRMB
            gvTranList.Columns(12).Visible = True   'lblTotalAmount

            gvTranList.Columns(13).Visible = False  'lblTranListTranStatusEng
            gvTranList.Columns(14).Visible = True   'lblTranListTranStatusChi
            gvTranList.Columns(15).Visible = False  'lblTranListTranStatusCN

            gvTranList.Columns(16).Visible = False  'lblTranListOtherInformation
            gvTranList.Columns(17).Visible = True   'lblTranListOtherInformationChi
            gvTranList.Columns(18).Visible = False  'lblTranListOtherInformationCN

            gvTranList.Columns(19).Visible = False  'lblTranListPracticeName
            gvTranList.Columns(20).Visible = True   'lblTranListPracticeNameChi

            gvTranList.Columns(22).Visible = False  'lblTranListVia
            gvTranList.Columns(23).Visible = True   'lblTranListVia_Chi
            gvTranList.Columns(24).Visible = False   'lblTranListVia_CN

        ElseIf LCase(Session("language")) = SimpChinese Then
            gvTranList.Columns(2).Visible = False   'lblTranListTranDtm
            gvTranList.Columns(3).Visible = True    'lblTranListTranDtm_Chi

            gvTranList.Columns(8).Visible = False   'lblTranListAccountType
            gvTranList.Columns(9).Visible = False    'lblTranListAccountType_Chi
            gvTranList.Columns(10).Visible = True   'lblTranListAccountType_CN

            'gvTranList.Columns(11).Visible = True  'lblTotalAmountRMB
            gvTranList.Columns(12).Visible = True   'lblTotalAmount

            gvTranList.Columns(13).Visible = False  'lblTranListTranStatusEng
            gvTranList.Columns(14).Visible = False   'lblTranListTranStatusChi
            gvTranList.Columns(15).Visible = True   'lblTranListTranStatusCN

            gvTranList.Columns(16).Visible = False  'lblTranListOtherInformation
            gvTranList.Columns(17).Visible = False  'lblTranListOtherInformationChi
            gvTranList.Columns(18).Visible = True   'lblTranListOtherInformationCN

            gvTranList.Columns(19).Visible = False  'lblTranListPracticeName
            gvTranList.Columns(20).Visible = True   'lblTranListPracticeNameChi

            gvTranList.Columns(22).Visible = False  'lblTranListVia
            gvTranList.Columns(23).Visible = False   'lblTranListVia_Chi
            gvTranList.Columns(24).Visible = True   'lblTranListVia_CN

        Else
            gvTranList.Columns(2).Visible = True    'lblTranListTranDtm
            gvTranList.Columns(3).Visible = False   'lblTranListTranDtm_Chi

            gvTranList.Columns(8).Visible = True    'lblTranListAccountType
            gvTranList.Columns(9).Visible = False   'lblTranListAccountType_Chi
            gvTranList.Columns(10).Visible = False   'lblTranListAccountType_CN

            'gvTranList.Columns(11).Visible = False  'lblTotalAmountRMB
            gvTranList.Columns(12).Visible = True   'lblTotalAmount

            gvTranList.Columns(13).Visible = True   'lblTranListTranStatusEng
            gvTranList.Columns(14).Visible = False  'lblTranListTranStatusChi
            gvTranList.Columns(15).Visible = False   'lblTranListTranStatusCN

            gvTranList.Columns(16).Visible = True   'lblTranListOtherInformation
            gvTranList.Columns(17).Visible = False  'lblTranListOtherInformationChi
            gvTranList.Columns(18).Visible = False  'lblTranListOtherInformationCN

            gvTranList.Columns(19).Visible = True   'lblTranListPracticeName
            gvTranList.Columns(20).Visible = False  'lblTranListPracticeNameChi

            gvTranList.Columns(22).Visible = True   'lblTranListVia
            gvTranList.Columns(23).Visible = False  'lblTranListVia_Chi
            gvTranList.Columns(24).Visible = False  'lblTranListVia_CN
        End If

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        If Me.SubPlatform.Equals(EnumHCSPSubPlatform.CN) Then
            gvTranList.Columns(11).Visible = True    'lblTotalAmountRMB
        Else
            gvTranList.Columns(11).Visible = False    'lblTotalAmountRMB
        End If
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        For Each r As GridViewRow In gvTranList.Rows
            Dim lblTranListOtherInformation As Label = r.FindControl("lblTranListOtherInformation")
            lblTranListOtherInformation.Text = Me.GetGlobalResourceObject("Text", "Details")

            Dim lblTranListOtherInformationChi As Label = r.FindControl("lblTranListOtherInformationChi")
            lblTranListOtherInformationChi.Text = Me.GetGlobalResourceObject("Text", "Details")

            Dim lblInvalidation As Label = r.FindControl("lblInvalidation")
            Dim lblDocCode As Label = r.FindControl("lblDocCode")
            Dim lblTranListHKID As Label = r.FindControl("lblTranListHKID")
            Dim lblTranListEname As Label = r.FindControl("lblTranListEname")
            Dim lblTranListCname As Label = r.FindControl("lblTranListCname")
            ' CRE13-001 EHAPP [Start][Karl]
            ' ----------------------------------------------------------------------------------------
            Dim lblTotalAmount As Label = r.FindControl("lblTotalAmount")

            If IsNumeric(lblTotalAmount.Text) = False Then
                lblTotalAmount.Text = Me.GetGlobalResourceObject("Text", "ServiceFeeN/A")
            End If
            ' CRE13-001 EHAPP [End][Karl]

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim lblTotalAmountRMB As Label = r.FindControl("lblTotalAmountRMB")

            If IsNumeric(lblTotalAmountRMB.Text) = False Then
                lblTotalAmountRMB.Text = Me.GetGlobalResourceObject("Text", "ServiceFeeN/A")
            End If
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            If lblInvalidation.Text.Trim = EHSTransactionModel.InvalidationStatusClass.Invalidated Then
                If LCase(Session("language")) = TradChinese Then
                    lblDocCode.Text = Me.GetGlobalResourceObject("Text", "ChiN/A")
                    lblTranListHKID.Text = Me.GetGlobalResourceObject("Text", "ChiN/A")
                    lblTranListEname.Text = Me.GetGlobalResourceObject("Text", "ChiN/A")
                ElseIf LCase(Session("language")) = SimpChinese Then
                    lblDocCode.Text = Me.GetGlobalResourceObject("Text", "CNN/A")
                    lblTranListHKID.Text = Me.GetGlobalResourceObject("Text", "CNN/A")
                    lblTranListEname.Text = Me.GetGlobalResourceObject("Text", "CNN/A")
                Else
                    lblDocCode.Text = Me.GetGlobalResourceObject("Text", "EngN/A")
                    lblTranListHKID.Text = Me.GetGlobalResourceObject("Text", "EngN/A")
                    lblTranListEname.Text = Me.GetGlobalResourceObject("Text", "EngN/A")
                End If
                lblTranListEname.Visible = True
                lblTranListCname.Visible = False
            End If
        Next

    End Sub

    '


    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    ' -----------------------------------------------------------------------------------------
    Private Sub HandleRedirectButtons()
        ' to be called after the view indexes are updated

        If getReturnTargetFunctCode() <> String.Empty AndAlso Not getReturnTargetSearchCriteria() Is Nothing Then
            Me.ibtnBack.Visible = False

            Select Case (MultiViewClaimTranManagement.ActiveViewIndex)
                Case ViewIndex.Detail
                    If MultiViewDetailAction.ActiveViewIndex = ViewIndexDetailAction.Button Then                ' ViewDetailButton is on
                        BuildRedirectButton(Me.ibtnReturnBtn, getReturnTargetSearchCriteria())
                        Me.ibtnDetailBack.Visible = False
                    ElseIf MultiViewDetailAction.ActiveViewIndex = ViewIndexDetailAction.CompleteModify Then    ' ViewCompleteModify is on
                        BuildRedirectButton(Me.ibtnReturnBtnCompleteModify, getReturnTargetSearchCriteria())
                        Me.ibtnCompleteModifyReturn.Visible = False
                    End If

                Case ViewIndex.CompleteVoid
                    BuildRedirectButton(Me.ibtnReturnBtnCompleteVoid, getReturnTargetSearchCriteria())
                    Me.ibtnReturn.Visible = False
            End Select
        End If
    End Sub

    Protected Sub ibtnSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Search()
    End Sub

    Protected Sub Search(ByVal cllnSearchCriteria As SearchCriteriaCollection, Optional ByVal blnDirectViewDetailForSingleTxn As Boolean = False)
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtSubPlatformBLL As New SubPlatformBLL

        Me.ddlSearchStatus.SelectedValue = cllnSearchCriteria(SEARCH_PARAM_RECORD_STATUS)

        'Me.txtSearchTransactionDateFrom.Text = udtFormatter.formatEnterDate(cllnSearchCriteria.GetDatetime(SEARCH_PARAM_DATE_FROM).Value)
        'Me.txtSearchTransactionDateTo.Text = udtFormatter.formatEnterDate(cllnSearchCriteria.GetDatetime(SEARCH_PARAM_DATE_TO).Value)
        Me.txtSearchTransactionDateFrom.Text = udtFormatter.formatInputTextDate(cllnSearchCriteria.GetDatetime(SEARCH_PARAM_DATE_FROM).Value, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
        Me.txtSearchTransactionDateTo.Text = udtFormatter.formatInputTextDate(cllnSearchCriteria.GetDatetime(SEARCH_PARAM_DATE_TO).Value, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        Dim strTranID As String = cllnSearchCriteria(SEARCH_PARAM_TRAN_ID)
        If Not String.IsNullOrEmpty(strTranID) Then
            Formatter.SplitTransactionNo(strTranID, Me.txtSearchTranNoPrefix.Text, Me.txtSearchTranNoContent.Text, Me.txtSearchTranNochkdgt.Text)
        End If
        Search(blnDirectViewDetailForSingleTxn)
    End Sub

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    ' -----------------------------------------------------------------------------------------
    'Protected Sub Search()
    Protected Sub Search(Optional ByVal blnDirectViewDetailForSingleTxn As Boolean = False)
        ' added parameter to directly display the detail view if the search result set contains only 1 transaction
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]
        imgSearchTransactionDateError.Visible = False

        udcMsgBox.Visible = False
        udcInfoMsgBox.Visible = False

        ' SPID / Data Entry Account
        Dim strSPID As String = String.Empty
        Dim strDataEntryAccount As String = String.Empty

        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC
        Dim blnBuildRecordSummary As Boolean = False

        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            Dim udtSP As ServiceProviderModel = udtUserAC
            strSPID = udtSP.SPID
            strDataEntryAccount = String.Empty
            blnBuildRecordSummary = True
        Else
            Dim udtDataEntryUser As DataEntryUserModel = udtUserAC
            strSPID = udtDataEntryUser.SPID
            strDataEntryAccount = udtDataEntryUser.DataEntryAccount
            blnBuildRecordSummary = False
        End If

        Session(SESS_BuildRecordSummary) = blnBuildRecordSummary

        ' Practice
        Dim strPractice As String = ddlSearchPractice.SelectedValue
        Dim intPracticeNo As Integer
        Dim intBankNo As Integer

        If strPractice <> String.Empty Then
            Dim aryPracticeSelectedValue As String() = strPractice.Split("-")
            intPracticeNo = CInt(aryPracticeSelectedValue(1))
            intBankNo = CInt(aryPracticeSelectedValue(2))
        Else
            intPracticeNo = 0
            intBankNo = 0
        End If

        ' Status
        Dim strStatus As String = ddlSearchStatus.SelectedValue

        ' Format the input date
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtSubPlatformBLL As New SubPlatformBLL
        'txtSearchTransactionDateFrom.Text = udtFormatter.formatDate(txtSearchTransactionDateFrom.Text.Trim)
        'txtSearchTransactionDateTo.Text = udtFormatter.formatDate(txtSearchTransactionDateTo.Text.Trim)

        '' Transaction Time
        'Dim strTransactionDateFrom As String = txtSearchTransactionDateFrom.Text.Trim
        'Dim strTransactionDateTo As String = txtSearchTransactionDateTo.Text.Trim

        Dim strTransactionDateFrom As String = udtFormatter.formatInputDate(txtSearchTransactionDateFrom.Text.Trim, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
        Dim strTransactionDateTo As String = udtFormatter.formatInputDate(txtSearchTransactionDateTo.Text.Trim, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))

        If udtValidator.chkValidSearchDate(strTransactionDateFrom) = String.Empty Then
            txtSearchTransactionDateFrom.Text = udtFormatter.formatInputTextDate(strTransactionDateFrom, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
        End If

        If udtValidator.chkValidSearchDate(strTransactionDateTo) = String.Empty Then
            txtSearchTransactionDateTo.Text = udtFormatter.formatInputTextDate(strTransactionDateTo, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
        End If
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        ' Transaction No.
        Dim strTransactionNo As String = String.Empty
        If txtSearchTranNoPrefix.Text.Trim <> String.Empty AndAlso txtSearchTranNoContent.Text.Trim <> String.Empty AndAlso txtSearchTranNochkdgt.Text.Trim <> String.Empty Then
            strTransactionNo = txtSearchTranNoPrefix.Text.Trim + "-" + txtSearchTranNoContent.Text.Trim + "-" + txtSearchTranNochkdgt.Text.Trim
        End If

        ' Scheme
        Dim strSchemeCode As String = ddlScheme.SelectedValue.Trim

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("SPID", strSPID)
        udtAuditLogEntry.AddDescripton("Data Entry Account", strDataEntryAccount)
        udtAuditLogEntry.AddDescripton("Practice", ddlSearchPractice.SelectedValue)
        udtAuditLogEntry.AddDescripton("Status", strStatus)
        udtAuditLogEntry.AddDescripton("Transaction Time From", strTransactionDateFrom)
        udtAuditLogEntry.AddDescripton("Transaction Time To", strTransactionDateTo)
        udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
        udtAuditLogEntry.AddDescripton("Scheme", strSchemeCode)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00001, AuditLogDescription.Search)

        ' Data validation
        Dim strMessageCode As String = String.Empty

        strMessageCode = udtValidator.chkValidSearchDate(strTransactionDateFrom)

        If strMessageCode <> String.Empty Then
            imgSearchTransactionDateError.Visible = True
        Else
            strMessageCode = udtValidator.chkValidSearchDate(strTransactionDateTo)
            If strMessageCode <> String.Empty Then
                imgSearchTransactionDateError.Visible = True
            End If
        End If

        If strMessageCode = String.Empty Then
            Dim udtSystemMessage As SystemMessage = udtValidator.chkInputValidFromDateCutoffDate(FunctionCode, MsgCode.MSG00006, _
                                udtFormatter.convertDate(strTransactionDateFrom, String.Empty), udtFormatter.convertDate(strTransactionDateTo, String.Empty))
            If Not IsNothing(udtSystemMessage) Then
                imgSearchTransactionDateError.Visible = True
                udcMsgBox.AddMessage(udtSystemMessage)
                udcMsgBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationFail, udtAuditLogEntry, LogID.LOG00003, AuditLogDescription.SearchFail)

                Return
            End If

        End If

        If strMessageCode <> String.Empty Then
            ' Message code mapping
            Select Case strMessageCode
                Case MsgCode.MSG00101
                    strMessageCode = MsgCode.MSG00001
                Case MsgCode.MSG00102
                    strMessageCode = MsgCode.MSG00004
                Case MsgCode.MSG00103
                    strMessageCode = MsgCode.MSG00005
                Case MsgCode.MSG00104
                    strMessageCode = MsgCode.MSG00003
            End Select

            udcMsgBox.AddMessage(FunctionCode, SeverityCode.SEVE, strMessageCode)
            udcMsgBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationFail, udtAuditLogEntry, LogID.LOG00003, AuditLogDescription.SearchFail)

            Return
        End If

        Dim dtmTransactionDateFrom As Date = udtFormatter.convertDate(udtFormatter.formatSearchDate(strTransactionDateFrom), String.Empty)
        Dim dtmTransactionDateTo As Date = udtFormatter.convertDate(udtFormatter.formatSearchDate(strTransactionDateTo), String.Empty)

        ' Build search criteria review
        lblTargetPractice.Text = ddlSearchPractice.SelectedItem.ToString
        lblTargetPracticeChi.Text = ddlSearchPractice.SelectedItem.ToString

        If LCase(Session("language")) = TradChinese OrElse LCase(Session("language")) = SimpChinese Then
            lblTargetPractice.Visible = False
            lblTargetPracticeChi.Visible = True
        Else
            lblTargetPractice.Visible = True
            lblTargetPracticeChi.Visible = False
        End If

        lblTargetStatus.Text = ddlSearchStatus.SelectedItem.ToString
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'lblTargetTranDate.Text = Me.GetGlobalResourceObject("Text", "From") + " " + udtFormatter.formatDate(dtmTransactionDateFrom) + " " + _
        '                            Me.GetGlobalResourceObject("Text", "To") + " " + udtFormatter.formatDate(dtmTransactionDateTo)
        udtSubPlatformBLL = New SubPlatformBLL

        lblTargetTranDate.Text = Me.GetGlobalResourceObject("Text", "From") + " " + udtFormatter.formatDisplayDate(dtmTransactionDateFrom, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform)) + " " + _
                            Me.GetGlobalResourceObject("Text", "To") + " " + udtFormatter.formatDisplayDate(dtmTransactionDateTo, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        If strTransactionNo = String.Empty Then
            lblTargetTranNo.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else
            lblTargetTranNo.Text = strTransactionNo.ToUpper
            strTransactionNo = udtFormatter.formatSystemNumberReverse(strTransactionNo)
        End If

        lblTargetScheme.Text = ddlScheme.SelectedItem.ToString

        Try
            Dim udtSearchCriteria As SearchCriteria = BuildSearchCriteria(strSPID, strDataEntryAccount, intPracticeNo, intBankNo, strStatus, _
                                                                    dtmTransactionDateFrom, dtmTransactionDateTo.AddDays(1), strTransactionNo, strSchemeCode)

            Session(SESS_SearchCriteria) = udtSearchCriteria

            Dim dtTransaction As DataTable = GetTransaction(udtSearchCriteria)

            If dtTransaction.Rows.Count = 0 Then
                udcInfoMsgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001))
                udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Information
                udcInfoMsgBox.BuildMessageBox()

                udtAuditLogEntry.AddDescripton("No of Record", "0")
                udtAuditLogEntry.WriteEndLog(LogID.LOG00002, AuditLogDescription.SearchSuccessful)

                ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
                ' -----------------------------------------------------------------------------------------
            ElseIf blnDirectViewDetailForSingleTxn Then
                ' directly displays the detail view when found only 1 record
                If Not dtTransaction Is Nothing Then
                    Session(SESS_EHSTransactionOriginal) = Nothing
                    Session(SESS_EHSTransaction) = Nothing
                    MultiViewClaimTranManagement.ActiveViewIndex = ViewIndex.Detail
                    MultiViewDetailAction.ActiveViewIndex = ViewIndexDetailAction.Button
                    BuildDetail(dtTransaction.Rows(0).Item("Transaction_ID").ToString())
                    udtAuditLogEntry.AddDescripton("No of Record", "1")
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00002, AuditLogDescription.SearchSuccessful)
                End If
                ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]
            Else
                Session(SESS_TransactionDataTable) = dtTransaction
                GridViewDataBind(gvTranList, dtTransaction, "Transaction_Dtm", "ASC", False)

                If blnBuildRecordSummary Then
                    BuildRecordSummary(dtTransaction)
                    panRecordSummary.Visible = True
                Else
                    panRecordSummary.Visible = False
                End If

                SwitchGridViewLanguage()

                SwitchTableRecordSummaryLanguage()

                MultiViewClaimTranManagement.ActiveViewIndex = ViewIndex.TransactionList

                udtAuditLogEntry.AddDescripton("No of Record", dtTransaction.Rows.Count)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00002, AuditLogDescription.SearchSuccessful)

            End If



        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                udcMsgBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, eSQL.Message))
                udcMsgBox.BuildMessageBox(ErrorMessageBoxHeaderKey.SearchFail, udtAuditLogEntry, LogID.LOG00003, AuditLogDescription.SearchFail)
            Else
                udtAuditLogEntry.AddDescripton("StackTrace", "Unknown SqlException")
                udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditLogDescription.SearchFail)
                Throw eSQL
            End If

        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception")
            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditLogDescription.SearchFail)
            Throw ex
        End Try

    End Sub

    Private Function BuildSearchCriteria(ByVal strSPID As String, ByVal strDataEntryAccount As String, ByVal intPracticeNo As Integer, _
                                            ByVal intBankNo As Integer, ByVal strStatus As String, ByVal dtmTransactionDateFrom As Date, _
                                            ByVal dtmTransactionDateTo As Date, ByVal strTransactionNo As String, ByVal strSchemeCode As String) As SearchCriteria
        Dim udtSearchCriteria As New SearchCriteria

        udtSearchCriteria.strSPID = strSPID
        udtSearchCriteria.strDataEntryAccount = strDataEntryAccount
        udtSearchCriteria.intPracticeNo = intPracticeNo
        udtSearchCriteria.intBankNo = intBankNo
        udtSearchCriteria.strStatus = strStatus
        udtSearchCriteria.dtmTransactionDateFrom = dtmTransactionDateFrom
        udtSearchCriteria.dtmTransactionDateTo = dtmTransactionDateTo
        udtSearchCriteria.strTransactionNo = strTransactionNo
        udtSearchCriteria.strSchemeCode = strSchemeCode

        Return udtSearchCriteria

    End Function

    Private Function GetTransaction(ByVal udtSearchCriteria As SearchCriteria) As DataTable
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim enumSubPlatform As [Enum] = Me.SubPlatform()
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        Dim dtTransaction As DataTable = udtTransactionMaintenanceBLL.SearchClaimTrans(udtSearchCriteria.strSPID, udtSearchCriteria.strDataEntryAccount, _
                udtSearchCriteria.intPracticeNo, udtSearchCriteria.intBankNo, udtSearchCriteria.strStatus, udtSearchCriteria.dtmTransactionDateFrom, _
                udtSearchCriteria.dtmTransactionDateTo, udtSearchCriteria.strTransactionNo, udtSearchCriteria.strSchemeCode, enumSubPlatform)

        ' Handle Mirgration Complete Show Practice Chi
        Dim strHCSPDataMirgrationCompleteTurnOn As String = String.Empty
        udtGeneralFunction.getSystemParameter("HCSPDataMirgrationCompleteTurnOn", strHCSPDataMirgrationCompleteTurnOn, String.Empty)

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Dim udtSchemeClaim As SchemeClaimModel
        'Dim udtSchemeClaimBLL As New SchemeClaimBLL
        'Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection
        'Dim drTransaction() As DataRow
        'Dim dtResTransaction As New DataTable

        'Dim strFilterCriteria As String = String.Empty

        'udtSchemeClaimModelCollection = udtSchemeClaimBLL.getAllDistinctSchemeClaim.FilterByHCSPSubPlatform(enumSubPlatform)

        'Dim strArrSchemeCode(udtSchemeClaimModelCollection.Count - 1) As String
        'Dim intCnt As String = 0

        'strFilterCriteria = "Scheme_Code in ('"
        'For Each udtSchemeClaim In udtSchemeClaimModelCollection
        '    strArrSchemeCode(intCnt) = udtSchemeClaim.SchemeCode.Trim
        '    intCnt += 1
        'Next
        'strFilterCriteria = strFilterCriteria & String.Join("','", strArrSchemeCode)
        'strFilterCriteria = strFilterCriteria & "')"

        'If enumSubPlatform.Equals(EnumHCSPSubPlatform.CN) Then
        '    strFilterCriteria = strFilterCriteria & " And Record_Status NOT IN ('" & ClaimTransStatus.Incomplete & "', '" & ClaimTransStatus.Joined & "')"
        'End If

        'drTransaction = dtTransaction.Select(strFilterCriteria)

        'dtResTransaction = dtTransaction.Clone

        'For Each dr As DataRow In drTransaction
        '    dtResTransaction.ImportRow(dr)
        'Next
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        For Each dr As DataRow In dtTransaction.Rows

            If dr.IsNull("Practice_Name_Chi") OrElse CStr(dr("Practice_Name_Chi")).Trim = String.Empty Then
                dr.BeginEdit()
                dr("Practice_Name_Chi") = CStr(dr("Practice_Name")).Trim
                dr.EndEdit()
            End If

            ' Handle Mirgration Complete Show Practice Chi
            If strHCSPDataMirgrationCompleteTurnOn.Trim = "Y" Then
            Else
                dr.BeginEdit()
                dr("Practice_Name_Chi") = CStr(dr("Practice_Name")).Trim
                dr.EndEdit()
            End If

            If dr("Record_Status") = Common.Component.ClaimTransStatus.Removed Or dr("Record_Status") = Common.Component.ClaimTransStatus.PendingApprovalForNonReimbursedClaim Then
                dr.Delete()
            End If

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            If enumSubPlatform.Equals(EnumHCSPSubPlatform.CN) Then
                If dr("Record_Status") = ClaimTransStatus.Incomplete Or dr("Record_Status") = ClaimTransStatus.Joined Then
                    dr.Delete()
                End If
            End If
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
        Next


        dtTransaction.AcceptChanges()

        Return dtTransaction

        'dtResTransaction.AcceptChanges()

        'Return dtResTransaction

    End Function

    Private Sub BuildRecordSummary(ByVal dt As DataTable)
        Dim dblIncomplete As Double = 0
        Dim dblPendingConfirm As Double = 0
        Dim dblPendingValidation As Double = 0
        Dim dblReady As Double = 0
        Dim dblVoided As Double = 0
        Dim dblReimbursed As Double = 0
        Dim dblSuspended As Double = 0
        Dim dblManualReimbursedClaim As Double = 0

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim dblIncompleteRMB As Double = 0
        Dim dblPendingConfirmRMB As Double = 0
        Dim dblPendingValidationRMB As Double = 0
        Dim dblReadyRMB As Double = 0
        Dim dblVoidedRMB As Double = 0
        Dim dblReimbursedRMB As Double = 0
        Dim dblSuspendedRMB As Double = 0
        Dim dblManualReimbursedClaimRMB As Double = 0
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        ' CRE20-0XX (HA Scheme) [Start][Winnie]
        Dim dblIncompleteRMB_SSSCMC As Double = 0
        Dim dblPendingConfirmRMB_SSSCMC As Double = 0
        Dim dblPendingValidationRMB_SSSCMC As Double = 0
        Dim dblReadyRMB_SSSCMC As Double = 0
        Dim dblVoidedRMB_SSSCMC As Double = 0
        Dim dblReimbursedRMB_SSSCMC As Double = 0
        Dim dblSuspendedRMB_SSSCMC As Double = 0
        Dim dblManualReimbursedClaimRMB_SSSCMC As Double = 0
        ' CRE20-0XX (HA Scheme) [End][Winnie]

        For Each dr As DataRow In dt.Rows
            If IsNumeric(dr("Total_Claim_Amount")) = True Then

                ' CRE20-0XX (HA Scheme) [Start][Winnie]
                ' Show HKD only when (1) HK Platform  or  (2) Scheme HCVSCHN in CN platform
                If Me.SubPlatform <> EnumHCSPSubPlatform.CN OrElse _
                    (Me.SubPlatform = EnumHCSPSubPlatform.CN AndAlso CStr(dr("Scheme_Code")).Trim = SchemeClaimModel.HCVSCHN) Then
                    ' CRE20-0XX (HA Scheme) [End][Winnie]

                    Select Case CStr(dr("Record_Status")).Trim
                        Case ClaimTransStatus.Incomplete
                            dblIncomplete += CDbl(dr("Total_Claim_Amount"))

                        Case ClaimTransStatus.Pending
                            dblPendingConfirm += CDbl(dr("Total_Claim_Amount"))

                        Case ClaimTransStatus.PendingVRValidate
                            dblPendingValidation += CDbl(dr("Total_Claim_Amount"))

                        Case ClaimTransStatus.Active
                            dblReady += CDbl(dr("Total_Claim_Amount"))

                        Case ClaimTransStatus.Inactive, ClaimTransStatus.RejectedBySP
                            dblVoided += CDbl(dr("Total_Claim_Amount"))

                        Case ClaimTransStatus.Reimbursed, ClaimTransStatus.ManualReimbursedClaim
                            dblReimbursed += CDbl(dr("Total_Claim_Amount"))

                        Case ClaimTransStatus.Suspended
                            dblSuspended += CDbl(dr("Total_Claim_Amount"))

                    End Select

                End If

            End If

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            If IsNumeric(dr("Total_Claim_Amount_RMB")) = True Then

                Select Case CStr(dr("Scheme_Code")).Trim

                    Case SchemeClaimModel.HCVSCHN
                        Select Case CStr(dr("Record_Status")).Trim
                            Case ClaimTransStatus.Incomplete
                                dblIncompleteRMB += CDbl(dr("Total_Claim_Amount_RMB"))

                            Case ClaimTransStatus.Pending
                                dblPendingConfirmRMB += CDbl(dr("Total_Claim_Amount_RMB"))

                            Case ClaimTransStatus.PendingVRValidate
                                dblPendingValidationRMB += CDbl(dr("Total_Claim_Amount_RMB"))

                            Case ClaimTransStatus.Active
                                dblReadyRMB += CDbl(dr("Total_Claim_Amount_RMB"))

                            Case ClaimTransStatus.Inactive, ClaimTransStatus.RejectedBySP
                                dblVoidedRMB += CDbl(dr("Total_Claim_Amount_RMB"))

                            Case ClaimTransStatus.Reimbursed, ClaimTransStatus.ManualReimbursedClaim
                                dblReimbursedRMB += CDbl(dr("Total_Claim_Amount_RMB"))

                            Case ClaimTransStatus.Suspended
                                dblSuspendedRMB += CDbl(dr("Total_Claim_Amount_RMB"))
                        End Select

                        ' CRE20-0XX (HA Scheme) [Start][Winnie]
                    Case SchemeClaimModel.SSSCMC
                        Select Case CStr(dr("Record_Status")).Trim
                            Case ClaimTransStatus.Incomplete
                                dblIncompleteRMB_SSSCMC += CDbl(dr("Total_Claim_Amount_RMB"))

                            Case ClaimTransStatus.Pending
                                dblPendingConfirmRMB_SSSCMC += CDbl(dr("Total_Claim_Amount_RMB"))

                            Case ClaimTransStatus.PendingVRValidate
                                dblPendingValidationRMB_SSSCMC += CDbl(dr("Total_Claim_Amount_RMB"))

                            Case ClaimTransStatus.Active
                                dblReadyRMB_SSSCMC += CDbl(dr("Total_Claim_Amount_RMB"))

                            Case ClaimTransStatus.Inactive, ClaimTransStatus.RejectedBySP
                                dblVoidedRMB_SSSCMC += CDbl(dr("Total_Claim_Amount_RMB"))

                            Case ClaimTransStatus.Reimbursed, ClaimTransStatus.ManualReimbursedClaim
                                dblReimbursedRMB_SSSCMC += CDbl(dr("Total_Claim_Amount_RMB"))

                            Case ClaimTransStatus.Suspended
                                dblSuspendedRMB_SSSCMC += CDbl(dr("Total_Claim_Amount_RMB"))
                        End Select
                        ' CRE20-0XX (HA Scheme) [End][Winnie]
                End Select
            End If
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
        Next

        'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Chris YIM]
        ' ----------------------------------------------------------------------------------------
        'lblSummaryIncomplete.Text = dblIncomplete.ToString("#,##0")
        'lblSummaryPendingComfirm.Text = dblPendingConfirm.ToString("#,##0")
        'lblSummaryPendingVRAcctValidate.Text = dblPendingValidation.ToString("#,##0")
        'lblSummaryReadyToReimburse.Text = dblReady.ToString("#,##0")
        'lblSummaryVoided.Text = dblVoided.ToString("#,##0")
        'lblSummaryReimbursed.Text = dblReimbursed.ToString("#,##0")
        'lblSummarySuspended.Text = dblSuspended.ToString("#,##0")
        lblSummaryIncomplete.Text = udtFormatter.formatMoney(dblIncomplete.ToString, False)
        lblSummaryPendingComfirm.Text = udtFormatter.formatMoney(dblPendingConfirm.ToString, False)
        lblSummaryPendingVRAcctValidate.Text = udtFormatter.formatMoney(dblPendingValidation.ToString, False)
        lblSummaryReadyToReimburse.Text = udtFormatter.formatMoney(dblReady.ToString, False)
        lblSummaryVoided.Text = udtFormatter.formatMoney(dblVoided.ToString, False)
        lblSummaryReimbursed.Text = udtFormatter.formatMoney(dblReimbursed.ToString, False)
        lblSummarySuspended.Text = udtFormatter.formatMoney(dblSuspended.ToString, False)
        'CRE13-018 Change Voucher Amount to 1 Dollar [End][Chris YIM]

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        lblSummaryHCVSCHN.Text = SchemeClaimModel.HCVSCHN
        'lblSummaryIncompleteRMB.Text = udtFormatter.formatMoneyRMB(dblIncompleteRMB.ToString, False)
        lblSummaryPendingComfirmRMB.Text = udtFormatter.formatMoneyRMB(dblPendingConfirmRMB.ToString, False)
        lblSummaryPendingVRAcctValidateRMB.Text = udtFormatter.formatMoneyRMB(dblPendingValidationRMB.ToString, False)
        lblSummaryReadyToReimburseRMB.Text = udtFormatter.formatMoneyRMB(dblReadyRMB.ToString, False)
        lblSummaryVoidedRMB.Text = udtFormatter.formatMoneyRMB(dblVoidedRMB.ToString, False)
        lblSummaryReimbursedRMB.Text = udtFormatter.formatMoneyRMB(dblReimbursedRMB.ToString, False)
        lblSummarySuspendedRMB.Text = udtFormatter.formatMoneyRMB(dblSuspendedRMB.ToString, False)
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        ' CRE20-0XX (HA Scheme) [Start][Winnie]
        lblSummarySSSCMC.Text = SchemeClaimModel.SSSCMC
        lblSummaryPendingComfirmRMB_SSSCMC.Text = udtFormatter.formatMoneyRMB(dblPendingConfirmRMB_SSSCMC.ToString, False)
        lblSummaryPendingVRAcctValidateRMB_SSSCMC.Text = udtFormatter.formatMoneyRMB(dblPendingValidationRMB_SSSCMC.ToString, False)
        lblSummaryReadyToReimburseRMB_SSSCMC.Text = udtFormatter.formatMoneyRMB(dblReadyRMB_SSSCMC.ToString, False)
        lblSummaryVoidedRMB_SSSCMC.Text = udtFormatter.formatMoneyRMB(dblVoidedRMB_SSSCMC.ToString, False)
        lblSummaryReimbursedRMB_SSSCMC.Text = udtFormatter.formatMoneyRMB(dblReimbursedRMB_SSSCMC.ToString, False)
        lblSummarySuspendedRMB_SSSCMC.Text = udtFormatter.formatMoneyRMB(dblSuspendedRMB_SSSCMC.ToString, False)
        ' CRE20-0XX (HA Scheme) [End][Winnie]

        'lblManualReimbursed.Text = dblManualReimbursedClaim.ToString("#,##0")
    End Sub

    Protected Sub ibtnBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMsgBox.Visible = False
        udcInfoMsgBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00010, AuditLogDescription.BackClick)

        MultiViewClaimTranManagement.ActiveViewIndex = ViewIndex.InputSearch
    End Sub

    Protected Sub ibtnModify_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMsgBox.Visible = False
        udcInfoMsgBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00015, AuditLogDescription.ModifyClick)

        MultiViewDetailAction.ActiveViewIndex = ViewIndexDetailAction.Modify

        txtVoidReason.Text = String.Empty
        imgAlertVoidReason.Visible = False

        Me.udcClaimTranEnquiry.ChangeModifyMode()
    End Sub

    Protected Sub ibtnModifyCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00020, AuditLogDescription.ModifyCancelClick)

        MultiViewDetailAction.ActiveViewIndex = ViewIndexDetailAction.Button
        Dim strTransactionID As String = CType(Session(SESS_EHSTransaction), EHSTransactionModel).TransactionID
        Session(SESS_EHSTransaction) = Nothing

        Me.BuildDetail(strTransactionID)
        Me.udcClaimTranEnquiry.ChangeViewMode()

        HandleRedirectButtons()     ' CRE11-024-02 HCVS

        If Me.ibtnDetailBack.Visible Then
            Me.SetFocus(Me.ibtnDetailBack)
        Else
            Me.SetFocus(Me.ibtnReturnBtn)
        End If
    End Sub

    Protected Sub ibtnModifyNext_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00016, AuditLogDescription.ModifyNextClick)

        Me.UpdateDetail()
        Me.udcMsgBox.Clear()
        If Not Me.udcClaimTranEnquiry.Validate(Me.udcMsgBox) Then
            Me.udcMsgBox.BuildMessageBox(Me.ValidationFail)
            Exit Sub
        End If



        MultiViewDetailAction.ActiveViewIndex = ViewIndexDetailAction.ModifyConfirm

        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC
        Dim udtEHSTransaction As EHSTransactionModel = Session(SESS_EHSTransaction)


        Me.ibtnModifyConfirmSave.Visible = False
        Me.ibtnModifyConfirmConfirm.Visible = False

        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            ' Service Provider
            'If udtEHSTransaction.ServiceDate >= New Date(2012, 1, 1) And (udtEHSTransaction.CoPaymentFee = String.Empty Or udtEHSTransaction.TransactionAdditionFields.Count = 0) Then
            '    ' Defer
            '    Me.ibtnModifyConfirmSave.Visible = True
            'ElseIf udtEHSTransaction.ServiceDate < New Date(2012, 1, 1) And (udtEHSTransaction.TransactionAdditionFields.Count = 0) Then
            If Me.udcClaimTranEnquiry.IsIncomplete(udtEHSTransaction) Then
                ' Defer
                Me.ibtnModifyConfirmSave.Visible = True
            Else
                Me.ibtnModifyConfirmConfirm.Visible = True
            End If
        Else
            ' Data Entry
            Me.ibtnModifyConfirmSave.Visible = True
        End If

        Me.udcClaimTranEnquiry.ChangeConfirmDetailMode()

        HandleRedirectButtons()     ' CRE11-024-02 HCVS

        If Me.ibtnModifyConfirmSave.Visible Then
            Me.SetFocus(Me.ibtnModifyConfirmSave)
        Else
            Me.SetFocus(Me.ibtnModifyConfirmConfirm)
        End If
    End Sub

    Protected Sub ibtnModifyConfirmBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00017, AuditLogDescription.ModifyBackClick)

        MultiViewDetailAction.ActiveViewIndex = ViewIndexDetailAction.Modify

        Me.udcClaimTranEnquiry.ChangeModifyMode()

        HandleRedirectButtons()     ' CRE11-024-02 HCVS
    End Sub

    Protected Sub ibtnModifySave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00018, AuditLogDescription.ModifySaveClick)

        SaveAndConfirmModification()
    End Sub

    Protected Sub ibtnModifyConfirmSaveConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00019, AuditLogDescription.ModifyConfirmClick)

        SaveAndConfirmModification()
    End Sub

    Protected Sub SaveAndConfirmModification()

        MultiViewDetailAction.ActiveViewIndex = ViewIndexDetailAction.CompleteModify

        Me.udcClaimTranEnquiry.ChangeViewMode()

        udcMsgBox.Visible = False
        udcInfoMsgBox.Visible = False

        Dim udtEHSTransaction As EHSTransactionModel = Session(SESS_EHSTransaction)


        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC
        Dim strAC_ID As String = String.Empty
        ' Bind Scheme
        Dim udtServiceProvider As ServiceProviderModel = Nothing
        Dim udtDataEntry As DataEntryUserModel = Nothing
        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            udtServiceProvider = udtUserAC
            strAC_ID = udtServiceProvider.SPID
        Else
            udtDataEntry = udtUserAC
            strAC_ID = udtDataEntry.DataEntryAccount
        End If

        udtEHSTransaction.UpdateBy = strAC_ID
        udtEHSTransaction.UpdateDate = Now()
        Dim udtDB As New Common.DataAccess.Database

        udtDB.BeginTransaction()

        Try

            Me.udtEHSTransactionBLL.UpdateEHSTransaction(udtDB, udtEHSTransaction)
            udtEHSTransaction = Me.udtEHSTransactionBLL.LoadClaimTran(udtEHSTransaction.TransactionID, False, False, udtDB)

            udtEHSTransaction.UpdateBy = strAC_ID
            udtEHSTransaction.UpdateDate = Now()

            ' Save transaction 
            ' --------------------------------------------------------------------------------
            If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                ' if SP
                ' --------------------------------------------------------------------------------
                'If udtEHSTransaction.ServiceDate >= New Date(2012, 1, 1) And (udtEHSTransaction.CoPaymentFee = String.Empty Or udtEHSTransaction.TransactionAdditionFields.Count = 0) Then
                '    ' Defer
                '    udcInfoMsgBox.AddMessage("020301", "I", "00003")
                'ElseIf udtEHSTransaction.ServiceDate < New Date(2012, 1, 1) And (udtEHSTransaction.TransactionAdditionFields.Count = 0) Then
                If Me.udcClaimTranEnquiry.IsIncomplete(udtEHSTransaction) Then
                    ' Defer
                    udcInfoMsgBox.AddMessage("020301", "I", "00003")
                Else

                    ' Complete Information
                    'If udtEHSTransaction.TempVoucherAccID = String.Empty Then
                    '    ' Valicated Account
                    '    Me.udtEHSTransactionBLL.UpdateEHSTransactionStatus(udtEHSTransaction.TransactionID, EHSTransactionModel.TransRecordStatusClass.Active, udtEHSTransaction.UpdateBy, udtEHSTransaction.UpdateDate, udtEHSTransaction.TSMP)
                    'Else
                    '    ' Temp Account
                    '    Me.udtEHSTransactionBLL.UpdateEHSTransactionStatus(udtEHSTransaction.TransactionID, EHSTransactionModel.TransRecordStatusClass.PendingVRValidate, udtEHSTransaction.UpdateBy, udtEHSTransaction.UpdateDate, udtEHSTransaction.TSMP)
                    'End If

                    Me.udtEHSTransactionBLL.UpdateEHSTransactionStatus(udtEHSTransaction.TransactionID, EHSTransactionModel.TransRecordStatusClass.Pending, udtEHSTransaction.UpdateBy, udtEHSTransaction.UpdateDate, udtEHSTransaction.TSMP, udtDB)
                    'Dim udtRecordConfirmationBLL As New RecordConfirmationBLL
                    'udtRecordConfirmationBLL.UpdateTransactionStatus(udtEHSTransaction.TransactionID, udtEHSTransaction.UpdateDate, udtEHSTransaction.UpdateBy, EHSTransactionModel.TransRecordStatusClass.Pending, udtEHSTransaction.TSMP, udtDB)

                    ' Confirm transaction
                    Dim udtRecordConfirmationBLL As New RecordConfirmationBLL
                    'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    'Dim dtmClaimConfirmationDate As DateTime = udtRecordConfirmationBLL.ConfirmTransaction(udtEHSTransaction.ServiceProviderID, udtEHSTransaction.TransactionID, udtDB)
                    Dim dtmClaimConfirmationDate As DateTime = udtRecordConfirmationBLL.ConfirmTransaction(udtEHSTransaction.ServiceProviderID, udtEHSTransaction.TransactionID, Me.SubPlatform, udtDB)
                    'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

                    udcInfoMsgBox.AddMessage("020301", "I", "00004") ' Claim completed and confirmed. Please refer to the following information. (TBC)
                End If

            Else
                ' if Data Entry
                ' --------------------------------------------------------------------------------
                'If udtEHSTransaction.ServiceDate >= New Date(2012, 1, 1) And (udtEHSTransaction.CoPaymentFee = String.Empty Or udtEHSTransaction.TransactionAdditionFields.Count = 0) Then
                '    ' Defer
                '    udcInfoMsgBox.AddMessage("020301", "I", "00003")
                'ElseIf udtEHSTransaction.ServiceDate < New Date(2012, 1, 1) And (udtEHSTransaction.TransactionAdditionFields.Count = 0) Then
                If Me.udcClaimTranEnquiry.IsIncomplete(udtEHSTransaction) Then
                    ' Defer
                    udcInfoMsgBox.AddMessage("020301", "I", "00003")
                Else
                    ' Complete Information, pending SP to confirm
                    Me.udtEHSTransactionBLL.UpdateEHSTransactionStatus(udtEHSTransaction.TransactionID, EHSTransactionModel.TransRecordStatusClass.Pending, udtEHSTransaction.UpdateBy, udtEHSTransaction.UpdateDate, udtEHSTransaction.TSMP, udtDB)

                    'Dim udtRecordConfirmationBLL As New RecordConfirmationBLL
                    'udtRecordConfirmationBLL.UpdateTransactionStatus(udtEHSTransaction.TransactionID, udtEHSTransaction.UpdateDate, udtEHSTransaction.UpdateBy, EHSTransactionModel.TransRecordStatusClass.Pending, udtEHSTransaction.TSMP, udtDB)
                    udcInfoMsgBox.AddMessage("020301", "I", "00005") ' Claim completed. Please refer to the following information. (TBC)
                End If

            End If
            udtDB.CommitTransaction()
        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex
        End Try

        ' Reload Claim
        udtEHSTransaction = Me.udtEHSTransactionBLL.LoadClaimTran(udtEHSTransaction.TransactionID)
        BuildDetail(udtEHSTransaction.TransactionID)

        udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Complete
        udcInfoMsgBox.BuildMessageBox()

        HandleRedirectButtons()     ' CRE11-024-02 HCVS
    End Sub


    Protected Sub ibtnCompleteModifyReturn_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)


        udcMsgBox.Visible = False
        udcInfoMsgBox.Visible = False

        ibtnReturn_Click(sender, e)
        'MultiViewDetailAction.ActiveViewIndex = ViewIndexDetailAction.Button
        'MultiViewClaimTranManagement.ActiveViewIndex = ViewIndex.TransactionList
    End Sub


    '

    Protected Sub ibtnDetailBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMsgBox.Visible = False
        udcInfoMsgBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00011, AuditLogDescription.DetailBackClick)

        udcClaimTranEnquiry.Clear()

        MultiViewClaimTranManagement.ActiveViewIndex = ViewIndex.TransactionList

    End Sub

    Protected Sub ibtnVoid_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMsgBox.Visible = False
        udcInfoMsgBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00013, AuditLogDescription.VoidClick)

        MultiViewDetailAction.ActiveViewIndex = ViewIndexDetailAction.Void

        txtVoidReason.Text = String.Empty
        imgAlertVoidReason.Visible = False

    End Sub

    Protected Sub ibtnVoidConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMsgBox.Visible = False
        udcInfoMsgBox.Visible = False

        imgAlertVoidReason.Visible = False

        Dim udtEHSTransaction As EHSTransactionModel = Nothing
        udtEHSTransaction = Session(SESS_EHSTransaction)

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Transaction No", udtEHSTransaction.TransactionID)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00007, AuditLogDescription.VoidTransaction)

        If txtVoidReason.Text.Trim = String.Empty Then
            udcMsgBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002))
            imgAlertVoidReason.Visible = True
        End If

        If udcMsgBox.GetCodeTable.Rows.Count <> 0 Then
            udcMsgBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationFail, udtAuditLogEntry, LogID.LOG00006, AuditLogDescription.VoidTransactionFail)

            Return
        End If

        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC

        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            Dim udtSP As ServiceProviderModel = udtUserAC
            udtEHSTransaction.VoidUser = udtSP.SPID
            udtEHSTransaction.VoidByDataEntry = String.Empty
        Else
            Dim udtDataEntry As DataEntryUserModel = udtUserAC
            udtEHSTransaction.VoidUser = udtDataEntry.SPID
            udtEHSTransaction.VoidByDataEntry = udtDataEntry.DataEntryAccount
        End If

        udtEHSTransaction.VoidReason = txtVoidReason.Text.Trim

        Try
            If udtTransactionMaintenanceBLL.OnVoid(udtEHSTransaction) = False Then
                udcMsgBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, MsgCode.MSG00011))
                udtAuditLogEntry.AddDescripton("Transaction No", udtEHSTransaction.TransactionID)
                udtAuditLogEntry.AddDescripton("StackTrace", "udtTransactionMaintenanceBLL.OnVoid(udtEHSTransaction) = False")
                udcMsgBox.BuildMessageBox(ErrorMessageBoxHeaderKey.UpdateFail, udtAuditLogEntry, LogID.LOG00006, AuditLogDescription.VoidTransactionFail)

                Return

            End If

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                udcMsgBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, eSQL.Message))
                udtAuditLogEntry.AddDescripton("Transaction No", udtEHSTransaction.TransactionID)
                udtAuditLogEntry.AddDescripton("StackTrace", "SqlException")
                udcMsgBox.BuildMessageBox(ErrorMessageBoxHeaderKey.UpdateFail, udtAuditLogEntry, LogID.LOG00006, AuditLogDescription.VoidTransactionFail)

                Return
            Else
                Throw eSQL
            End If

        Catch ex As Exception
            Throw ex
        End Try

        udtEHSTransaction = udtEHSTransactionBLL.LoadClaimTran(udtEHSTransaction.TransactionID)

        lblCompleteReferenceNo.Text = udtFormatter.formatSystemNumber(udtEHSTransaction.VoidTranNo.Trim)
        lblCompleteVoidDate.Text = udtFormatter.formatDateTime(udtEHSTransaction.VoidDate)

        Select Case udtEHSTransaction.EHSAcct.AccountSource
            Case EHSAccount.EHSAccountModel.SysAccountSource.SpecialAccount
                udcInfoMsgBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00002, _
                    New String() {"%r", "%s"}, New Object() {udtFormatter.formatSystemNumber(udtEHSTransaction.EHSAcct.OriginalAccID.Trim), _
                    udtFormatter.formatSystemNumber(udtEHSTransaction.VoidTranNo.Trim)})

            Case Else
                If udtEHSTransaction.EHSAcct.RecordStatus = VRAcctValidatedStatus.Deleted Then
                    If Not IsNothing(udtEHSTransaction.EHSAcct.OriginalAccID) _
                            AndAlso udtEHSTransaction.EHSAcct.OriginalAccID <> String.Empty Then
                        udcInfoMsgBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001, _
                            "%s", udtFormatter.formatSystemNumber(udtEHSTransaction.VoidTranNo.Trim))
                    Else
                        udcInfoMsgBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00002, _
                            New String() {"%r", "%s"}, New Object() {udtFormatter.formatSystemNumber(udtEHSTransaction.EHSAcct.VoucherAccID.Trim), _
                            udtFormatter.formatSystemNumber(udtEHSTransaction.VoidTranNo.Trim)})
                    End If

                Else
                    udcInfoMsgBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001, _
                        "%s", udtFormatter.formatSystemNumber(udtEHSTransaction.VoidTranNo.Trim))
                End If

        End Select

        udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Complete
        udcInfoMsgBox.BuildMessageBox()

        udtAuditLogEntry.AddDescripton("Transaction No", udtEHSTransaction.TransactionID)
        udtAuditLogEntry.AddDescripton("Void Reason", udtEHSTransaction.VoidReason)
        udtAuditLogEntry.AddDescripton("Void Transaction No", udtEHSTransaction.VoidTranNo)
        udtAuditLogEntry.AddDescripton("Void Transaction Date", udtEHSTransaction.VoidDate)
        udtAuditLogEntry.WriteEndLog(LogID.LOG00005, AuditLogDescription.VoidTransactionSuccessful)

        MultiViewClaimTranManagement.ActiveViewIndex = ViewIndex.CompleteVoid

        HandleRedirectButtons()     ' CRE11-024-02 HCVS

    End Sub

    Protected Sub ibtnVoidCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMsgBox.Visible = False
        udcInfoMsgBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00012, AuditLogDescription.CancelConfirmVoidClick)

        MultiViewDetailAction.ActiveViewIndex = ViewIndexDetailAction.Button

        HandleRedirectButtons()     ' CRE11-024-02 HCVS

    End Sub

    '

    Protected Sub ibtnReturn_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMsgBox.Visible = False
        udcInfoMsgBox.Visible = False

        Session(SESS_HideDetailBackButton) = "N"

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00009, AuditLogDescription.ReturnClick)

        Try
            Dim udtSearchCriteria As SearchCriteria = Session(SESS_SearchCriteria)
            Dim dtTransaction As DataTable = GetTransaction(udtSearchCriteria)

            If dtTransaction.Rows.Count = 0 Then
                udcInfoMsgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001))
                udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Information
                udcInfoMsgBox.BuildMessageBox()

                udtAuditLogEntry.AddDescripton("No of Record", "0")
                udtAuditLogEntry.WriteEndLog(LogID.LOG00002, AuditLogDescription.SearchSuccessful)

                MultiViewClaimTranManagement.ActiveViewIndex = ViewIndex.InputSearch

            Else
                Session(SESS_TransactionDataTable) = dtTransaction
                GridViewDataBind(gvTranList, dtTransaction, "Transaction_Dtm", "ASC", False)

                Dim blnBuildRecordSummary As Boolean = Session(SESS_BuildRecordSummary)

                If blnBuildRecordSummary Then
                    BuildRecordSummary(dtTransaction)
                    panRecordSummary.Visible = True
                Else
                    panRecordSummary.Visible = False
                End If

                SwitchGridViewLanguage()

                SwitchTableRecordSummaryLanguage()

                MultiViewClaimTranManagement.ActiveViewIndex = ViewIndex.TransactionList

                udtAuditLogEntry.AddDescripton("No of Record", dtTransaction.Rows.Count)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00002, AuditLogDescription.SearchSuccessful)

            End If

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                udcMsgBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, eSQL.Message))
                udcMsgBox.BuildMessageBox(ErrorMessageBoxHeaderKey.SearchFail, udtAuditLogEntry, LogID.LOG00003, AuditLogDescription.SearchFail)
            Else
                udtAuditLogEntry.AddDescripton("StackTrace", "Unknown SqlException")
                udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditLogDescription.SearchFail)
                Throw eSQL
            End If

        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception")
            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditLogDescription.SearchFail)
            Throw ex
        End Try

    End Sub

    '

    Private Sub gvTranList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTranList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Transaction No.
            Dim lbtn_transactionNum As LinkButton = e.Row.FindControl("lbtn_transactionNum")
            Dim strTransactionNo As String = lbtn_transactionNum.Text.Trim
            lbtn_transactionNum.Text = udtFormatter.formatSystemNumber(strTransactionNo)

            ' Transaction Time
            Dim lblTranListTranDtm As Label = e.Row.FindControl("lblTranListTranDtm")
            lblTranListTranDtm.Text = udtFormatter.formatDateTime(lblTranListTranDtm.Text.Trim, "EN-US")

            ' Transaction Time (Chi)
            Dim lblTranListTranDtm_Chi As Label = e.Row.FindControl("lblTranListTranDtm_Chi")
            lblTranListTranDtm_Chi.Text = udtFormatter.formatDateTime(lblTranListTranDtm.Text.Trim, "ZH-TW")

            ' Identity Document No. + Remark
            Dim lblTranListHKID As Label = e.Row.FindControl("lblTranListHKID")
            Dim hfDocCode As HiddenField = e.Row.FindControl("hfDocCode")
            Dim hfTranListIDNo1 As HiddenField = e.Row.FindControl("hfTranListIDNo1")
            Dim hfTranListIDNo2 As HiddenField = e.Row.FindControl("hfTranListIDNo2")

            ' Name (Chi)
            Dim lblTranListCname As Label = e.Row.FindControl("lblTranListCname")


            ' Account Type (Eng / Chi)
            Dim lblTranListAccountType As Label = e.Row.FindControl("lblTranListAccountType")
            Dim lblTranListAccountType_Chi As Label = e.Row.FindControl("lblTranListAccountType_Chi")
            Dim lblTranListAccountType_CN As Label = e.Row.FindControl("lblTranListAccountType_CN")

            ' CRE13-001 EHAPP [Start][Karl]
            ' ----------------------------------------------------------------------------------------
            'Claim Amount
            Dim lblTotalAmount As Label
            lblTotalAmount = CType(e.Row.FindControl("lblTotalAmount"), Label)

            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim dr As DataRow = CType(e.Row.DataItem, System.Data.DataRowView).Row
                Dim strTotalAmount As String
                If IsDBNull(dr.Item("Total_Claim_Amount")) = True Then
                    strTotalAmount = Me.GetGlobalResourceObject("Text", "ServiceFeeN/A")
                Else
                    'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Chris YIM]
                    ' ----------------------------------------------------------------------------------------
                    'strTotalAmount = CDbl(dr.Item("Total_Claim_Amount")).ToString("#,##0")
                    strTotalAmount = udtFormatter.formatMoney(dr.Item("Total_Claim_Amount").ToString, False)
                    'CRE13-018 Change Voucher Amount to 1 Dollar [End][Chris YIM]
                End If
                lblTotalAmount.Text = strTotalAmount
            End If

            ' CRE13-001 EHAPP [End][Karl]

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim lblTotalAmountRMB As Label
            lblTotalAmountRMB = CType(e.Row.FindControl("lblTotalAmountRMB"), Label)

            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim dr As DataRow = CType(e.Row.DataItem, System.Data.DataRowView).Row
                Dim strTotalAmountRMB As String
                If IsDBNull(dr.Item("Total_Claim_Amount_RMB")) = True Then
                    strTotalAmountRMB = Me.GetGlobalResourceObject("Text", "ServiceFeeN/A")
                Else
                    strTotalAmountRMB = udtFormatter.formatMoneyRMB(dr.Item("Total_Claim_Amount_RMB").ToString, False)
                End If
                lblTotalAmountRMB.Text = strTotalAmountRMB
            End If
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            ' Status (Eng / Chi)
            Dim lblTranListTranStatusEng As Label = e.Row.FindControl("lblTranListTranStatusEng")
            Dim lblTranListTranStatusChi As Label = e.Row.FindControl("lblTranListTranStatusChi")
            Dim lblTranListTranStatusCN As Label = e.Row.FindControl("lblTranListTranStatusCN")
            Dim hfTranListTranStatusEng As HiddenField = e.Row.FindControl("hfTranListTranStatusEng")

            Dim lblInvalidation As Label = e.Row.FindControl("lblInvalidation")

            Status.GetDescriptionFromDBCode(ClaimTransStatus.ClassCode, hfTranListTranStatusEng.Value.Trim, _
                    lblTranListTranStatusEng.Text, lblTranListTranStatusChi.Text, lblTranListTranStatusCN.Text)

            ' Reimbursment Method
            Dim hfTranListManualReimburse As HiddenField = e.Row.FindControl("hfTranListManualReimburse")
            Dim udtStaticDataBLL As StaticData.StaticDataBLL = New StaticData.StaticDataBLL
            Dim udtStaticDataModel As StaticData.StaticDataModel

            ' CRE13-001 EHAPP [Start][Karl]
            ' ----------------------------------------------------------------------------------------
            Dim udtSchemeClaimBLL As New SchemeClaimBLL
            Dim udtSchemeClaimModel As SchemeClaimModel

            Dim blnReimbursementAvailable As Boolean = False

            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim dr As DataRow = CType(e.Row.DataItem, System.Data.DataRowView).Row

                'INT14-0017 (Fix HSCP Claim Trans Management search for expired scheme) [ Start][Karl]
                'udtSchemeClaimModel = udtSchemeClaimBLL.getEffectiveSchemeClaim(dr.Item("Scheme_Code"))
                udtSchemeClaimModel = udtSchemeClaimBLL.getAllDistinctSchemeClaim.Filter(dr.Item("Scheme_Code"))
                'INT14-0017 (Fix HSCP Claim Trans Management search for expired scheme) [End][Karl]

                If udtSchemeClaimModel.ReimbursementMode = EnumReimbursementMode.All _
                        OrElse udtSchemeClaimModel.ReimbursementMode = EnumReimbursementMode.FirstAuthAndSecondAuth Then
                    blnReimbursementAvailable = True
                End If

            End If

            'If hfTranListManualReimburse.Value.Trim() = "Y"  Then
            If hfTranListManualReimburse.Value.Trim() = "Y" AndAlso blnReimbursementAvailable = True Then
                ' CRE13-001 EHAPP [End][Karl]
                udtStaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("ReimbursementMethod", "O") ' O-Outsidehs I-In EHS
                lblTranListTranStatusEng.Text = lblTranListTranStatusEng.Text + "<br>(" + udtStaticDataModel.DataValue.ToString.Trim() + ")"
                lblTranListTranStatusChi.Text = lblTranListTranStatusChi.Text + "<br>(" + udtStaticDataModel.DataValueChi.ToString.Trim() + ")"
                lblTranListTranStatusCN.Text = lblTranListTranStatusCN.Text + "<br>(" + udtStaticDataModel.DataValueCN.ToString.Trim() + ")"
            End If

            'For Remarked transaction
            ' All account related information shows N/A
            If lblInvalidation.Text.Trim = EHSTransactionModel.InvalidationStatusClass.Invalidated Then
                Dim lblDocCode As Label = e.Row.FindControl("lblDocCode")
                Dim lblTranListEname As Label = e.Row.FindControl("lblTranListEname")

                If LCase(Session("language")) = TradChinese Then
                    lblDocCode.Text = Me.GetGlobalResourceObject("Text", "ChiN/A")
                    lblTranListHKID.Text = Me.GetGlobalResourceObject("Text", "ChiN/A")
                    lblTranListEname.Text = Me.GetGlobalResourceObject("Text", "ChiN/A")
                ElseIf LCase(Session("language")) = SimpChinese Then
                    lblDocCode.Text = Me.GetGlobalResourceObject("Text", "CNN/A")
                    lblTranListHKID.Text = Me.GetGlobalResourceObject("Text", "CNN/A")
                    lblTranListEname.Text = Me.GetGlobalResourceObject("Text", "CNN/A")
                Else
                    lblDocCode.Text = Me.GetGlobalResourceObject("Text", "EngN/A")
                    lblTranListHKID.Text = Me.GetGlobalResourceObject("Text", "EngN/A")
                    lblTranListEname.Text = Me.GetGlobalResourceObject("Text", "EngN/A")
                End If
                lblTranListEname.Visible = True
                lblTranListCname.Visible = False

                lblTranListAccountType.Text = Me.GetGlobalResourceObject("Text", "EngN/A")
                lblTranListAccountType_Chi.Text = Me.GetGlobalResourceObject("Text", "ChiN/A")
                lblTranListAccountType_CN.Text = Me.GetGlobalResourceObject("Text", "CNN/A")

                Dim strInvalidationStatus_Eng As String = String.Empty
                Dim strInvalidationStatus_Chi As String = String.Empty
                Dim strInvalidationStatus_CN As String = String.Empty
                Status.GetDescriptionFromDBCode(EHSTransactionModel.InvalidationStatusClass.ClassCode, lblInvalidation.Text.Trim, _
                                                    strInvalidationStatus_Eng, strInvalidationStatus_Chi, strInvalidationStatus_CN)

                lblTranListTranStatusEng.Text = lblTranListTranStatusEng.Text + " (" + strInvalidationStatus_Eng + ")"
                lblTranListTranStatusChi.Text = lblTranListTranStatusChi.Text + " (" + strInvalidationStatus_Chi + ")"
                lblTranListTranStatusCN.Text = lblTranListTranStatusCN.Text + " (" + strInvalidationStatus_CN + ")"

            Else
                lblTranListHKID.Text = udtFormatter.FormatDocIdentityNoForDisplay(hfDocCode.Value.Trim, hfTranListIDNo1.Value.Trim, True, hfTranListIDNo2.Value.Trim)
                lblTranListCname.Text = udtFormatter.formatChineseName(lblTranListCname.Text.Trim)
                Status.GetDescriptionFromDBCode(EHealthAccountType.ClassCode, lblTranListAccountType.Text.Trim, _
                                                    lblTranListAccountType.Text, lblTranListAccountType_Chi.Text, lblTranListAccountType_CN.Text)

            End If

            ' Bank Account No.
            Dim lblTranListBankAcct As Label = e.Row.FindControl("lblTranListBankAcct")
            lblTranListBankAcct.Text = udtFormatter.maskBankAccount(lblTranListBankAcct.Text.Trim)

            ' Other Information (Eng / Chi)
            Dim lblTranListOtherInformation As Label = e.Row.FindControl("lblTranListOtherInformation")
            Dim lblTranListOtherInformationChi As Label = e.Row.FindControl("lblTranListOtherInformationChi")
            Dim lblTranListOtherInformationCN As Label = e.Row.FindControl("lblTranListOtherInformationCN")

            Dim hfTranListInformationCode As HiddenField = e.Row.FindControl("hfTranListInformationCode")
            Dim hfTranListInformationCodeChi As HiddenField = e.Row.FindControl("hfTranListInformationCodeChi")
            Dim hfTranListInformationCodeCN As HiddenField = e.Row.FindControl("hfTranListInformationCodeCN")

            lblTranListOtherInformation.ToolTip = hfTranListInformationCode.Value
            lblTranListOtherInformationChi.ToolTip = hfTranListInformationCodeChi.Value
            lblTranListOtherInformationCN.ToolTip = hfTranListInformationCodeCN.Value

            ' Data Entry Account (Eng / Chi)
            Dim lblTranListDataEntry_By As Label = e.Row.FindControl("lblTranListDataEntry_By")
            Dim lblTranListDataEntry_By_Chi As Label = e.Row.FindControl("lblTranListDataEntry_By_Chi")
            Dim lblTranListDataEntry_By_CN As Label = e.Row.FindControl("lblTranListDataEntry_By_CN")

            If lblTranListDataEntry_By.Text.Trim.Equals(String.Empty) Then
                lblTranListDataEntry_By.Text = Me.GetGlobalResourceObject("Text", "EngN/A")
                lblTranListDataEntry_By_Chi.Text = Me.GetGlobalResourceObject("Text", "ChiN/A")
                lblTranListDataEntry_By_CN.Text = Me.GetGlobalResourceObject("Text", "CNN/A")
            End If

            Dim lblIsUpload As Label = e.Row.FindControl("lblIsUpload")
            Dim lblTranListVia As Label = e.Row.FindControl("lblTranListVia")
            Dim lblTranListVia_Chi As Label = e.Row.FindControl("lblTranListVia_Chi")

            If lblIsUpload.Text = "Y" Then
                lblTranListVia.Visible = True
                lblTranListVia_Chi.Visible = True
                lblTranListDataEntry_By.ForeColor = Drawing.Color.Brown
                lblTranListDataEntry_By_Chi.ForeColor = Drawing.Color.Brown
            End If


        End If
    End Sub

    Private Sub gvTranList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTranList.RowCommand
        If TypeOf e.CommandSource Is LinkButton Then
            Dim strTransactionNo As String = e.CommandArgument.ToString
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
            ' -----------------------------------------------------------------------------------------
            Dim udtEHSTransaction As EHSTransactionModel = (New EHSTransactionBLL).LoadClaimTran(strTransactionNo)
            Session(SESS_EHSTransaction) = udtEHSTransaction
            Session(SESS_EHSTransactionOriginal) = (New EHSTransactionBLL).LoadClaimTran(strTransactionNo)

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
            udtAuditLogEntry.WriteLog(LogID.LOG00004, AuditLogDescription.ViewDetail)

            MultiViewClaimTranManagement.ActiveViewIndex = ViewIndex.Detail
            MultiViewDetailAction.ActiveViewIndex = ViewIndexDetailAction.Button

            BuildDetail(strTransactionNo)
        End If
    End Sub

    Private Sub gvTranList_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTranList.RowCreated
        Dim udtSortedGVHeaderList As SortedGridviewHeaderModelCollection = New SortedGridviewHeaderModelCollection
        Dim strImageURL As String = Me.GetGlobalResourceObject("ImageURL", "infoicon")

        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(1, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(5, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(6, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))

        ' Add the subsidy legend only if in non-CN platform
        If Me.SubPlatform <> EnumHCSPSubPlatform.CN Then
            udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(16, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
            udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(17, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
            udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(18, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
        End If

        GirdViewRowCreated(sender, e, udtSortedGVHeaderList)
    End Sub

    Private Sub gvTranList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTranList.PageIndexChanging
        Me.GridViewPageIndexChangingHandler(sender, e, SESS_TransactionDataTable)
    End Sub

    Private Sub gvTranList_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvTranList.PreRender
        Me.GridViewPreRenderHandler(sender, e, SESS_TransactionDataTable)
    End Sub

    Private Sub gvTranList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvTranList.Sorting
        Me.GridViewSortingHandler(sender, e, SESS_TransactionDataTable)
    End Sub

    '

    Public Overrides Sub GridViewHeaderImage_Click(ByVal sender As Object, ByVal e As SortedGridviewHeaderModel.GridViewHeaderImageEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        Select Case e.intColumn
            Case 1
                udtAuditLogEntry.AddDescripton("Type", "Scheme")
                udtAuditLogEntry.WriteLog(LogID.LOG00014, AuditLogDescription.ShowLegend)

                popupSchemeNameHelp.Show()
                udcSchemeLegend.ShowFilteredSubsidy = True
                udcSchemeLegend.ShowSubsidy = False
                udcSchemeLegend.SchemeLegendSubPlatform = Me.SubPlatform
                udcSchemeLegend.BindSchemeClaim(Session("language"))

            Case 16, 17, 18
                udtAuditLogEntry.AddDescripton("Type", "OtherInfo")
                udtAuditLogEntry.WriteLog(LogID.LOG00014, AuditLogDescription.ShowLegend)

                popupSchemeNameHelp.Show()
                udcSchemeLegend.ShowFilteredSubsidy = True
                udcSchemeLegend.ShowScheme = False
                udcSchemeLegend.SchemeLegendSubPlatform = Me.SubPlatform
                udcSchemeLegend.BindSchemeClaim(Session("language"))

            Case 5, 6
                udtAuditLogEntry.AddDescripton("Type", "DocType")
                udtAuditLogEntry.WriteLog(LogID.LOG00014, AuditLogDescription.ShowLegend)

                popupDocTypeHelp.Show()
                udcDocTypeLegend.DocTypeLegendSubPlatform = Me.SubPlatform
                udcDocTypeLegend.BindDocType(Session("language"))

        End Select

    End Sub

    '

    Protected Sub ibtnCloseDocTypeHelp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    End Sub

    Protected Sub ibtnCloseSchemeNameHelp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    End Sub

    '

    Public Sub udcClaimTranEnquiry_VaccineLegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcSchemeLegend.ShowScheme = False
        udcSchemeLegend.BindSchemeClaim(Session("language"), Me.SubPlatform)
        popupSchemeNameHelp.Show()
    End Sub

    ''' <summary>
    ''' CRE11-004
    ''' Handle working data on view change, clear working data if no use
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MultiViewClaimTranManagement_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MultiViewClaimTranManagement.ActiveViewChanged
        Select Case MultiViewClaimTranManagement.ActiveViewIndex
            Case ViewIndex.InputSearch
                Me.ClearWorkingData()
            Case ViewIndex.TransactionList
                Me.ClearWorkingData()
            Case ViewIndex.Detail
                ' Do Nothing (Keep working data)
            Case ViewIndex.CompleteVoid
                Me.ClearWorkingData()
        End Select
    End Sub

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Account which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Dim udtEHSTransaction As EHSTransactionModel = GetEHSTransaction()
        If Not IsNothing(udtEHSTransaction) Then
            Return udtEHSTransaction.EHSAcct
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
        Return Session(SESS_EHSTransaction)
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
        Dim udtEHSTransaction As EHSTransactionModel = GetEHSTransaction()
        If Not IsNothing(udtEHSTransaction) Then
            Return udtEHSTransaction.DocCode
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' CRE11-004
    '''  Clear all working data
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub ClearWorkingData()
        MyBase.ClearWorkingData()

        Session(SESS_EHSTransaction) = Nothing

        ' INT12-0012 HCSP Record Confirmation, CTM error on grid and RCH code input problem [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Me.udcClaimTranEnquiry.Clear()
        ' INT12-0012 HCSP Record Confirmation, CTM error on grid and RCH code input problem [End][Koala]
    End Sub

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    ' -----------------------------------------------------------------------------------------
    Private Sub HandleRedirectAction()
        Dim udtRedirectParameterBLL As New RedirectParameterBLL
        Dim udtRedirectParameter As RedirectParameterModel = udtRedirectParameterBLL.GetFromSession()
        If IsNothing(udtRedirectParameter) Then Return

        udtRedirectParameterBLL.RemoveFromSession()
        udtRedirectParameterBLL.WriteAuditLog(FunctionCode, Me.Page, udtRedirectParameter)

        If udtRedirectParameter.ActionList.Contains(RedirectParameterModel.EnumRedirectAction.Search) Then
            If udtRedirectParameter.ActionList.Contains(RedirectParameterModel.EnumRedirectAction.ViewDetail) Then
                Search(udtRedirectParameter.SearchCriteria, True)
            Else
                Search(udtRedirectParameter.SearchCriteria, False)
            End If
        End If
    End Sub

    Private Sub BuildRedirectButton(ByVal btn As CustomControls.CustomImageButton, ByVal objSearchCriteria As RedirectParameter.SearchCriteriaCollection)
        btn.SourceFunctionCode = CType(Me.Page, BasePage).FunctionCode

        If getReturnTargetFunctCode() <> String.Empty Then
            btn.TargetFunctionCode = getReturnTargetFunctCode()
            Dim menu As New MenuBLL()
            Dim strReturnBtnName As String = String.Empty           ' CRE11-024-02 [Tony]

            strReturnBtnName = menu.GetSystemResourceObjectName_ReturnBtn(btn.TargetFunctionCode)
            If strReturnBtnName <> String.Empty Then
                btn.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", strReturnBtnName)
                btn.AlternateText = Me.GetGlobalResourceObject("AlternateText", strReturnBtnName)
            End If
            btn.TargetUrl = RedirectHandler.AppendPageKeyToURL((New BLL.MenuBLL).GetURL(getReturnTargetFunctCode()))
        End If

        btn.Build()

        btn.ConstructNewRedirectParameter()
        If getReturnTargetContainsSearch() Then
            btn.RedirectParameter.ActionList.Add(RedirectParameterModel.EnumRedirectAction.Search)
        End If
        If getReturnTargetContainsViewDetail() Then
            btn.RedirectParameter.ActionList.Add(RedirectParameterModel.EnumRedirectAction.ViewDetail)
        End If

        btn.RedirectParameter.SearchCriteria = objSearchCriteria

    End Sub

    Private Sub ibtnReturnBtn_Click(ByVal sender As System.Web.UI.WebControls.ImageButton, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnReturnBtn.Click
        '_udtAuditLogEntry.WriteLog(AuditLogDesc.ManagementClick_ID, AuditLogDesc.ManagementClick)

        ' Get Target Function Code
        Dim strTargetFunctCode As String = getReturnTargetFunctCode()

        ' Remove Session Variable
        Dim udtRedirectParameterBLL As New RedirectParameterBLL
        Dim udtRedirectParameter As RedirectParameterModel = udtRedirectParameterBLL.GetFromSession()
        If Not IsNothing(udtRedirectParameter) Then udtRedirectParameterBLL.RemoveFromSession()

        Dim btn As CustomControls.CustomImageButton = sender.Parent
        btn.TargetUrl = RedirectHandler.AppendPageKeyToURL((New BLL.MenuBLL).GetURL(strTargetFunctCode))
        btn.Redirect()
    End Sub


    Private Sub ibtnReturnBtnCompleteModify_Click(ByVal sender As System.Web.UI.WebControls.ImageButton, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnReturnBtnCompleteModify.Click
        '_udtAuditLogEntry.WriteLog(AuditLogDesc.ManagementClick_ID, AuditLogDesc.ManagementClick)

        ' Get Target Function Code
        Dim strTargetFunctCode As String = getReturnTargetFunctCode()

        ' Remove Session Variable
        Dim udtRedirectParameterBLL As New RedirectParameterBLL
        Dim udtRedirectParameter As RedirectParameterModel = udtRedirectParameterBLL.GetFromSession()
        If Not IsNothing(udtRedirectParameter) Then udtRedirectParameterBLL.RemoveFromSession()

        Dim btn As CustomControls.CustomImageButton = sender.Parent
        btn.TargetUrl = RedirectHandler.AppendPageKeyToURL((New BLL.MenuBLL).GetURL(strTargetFunctCode))
        btn.Redirect()
    End Sub

    Private Sub ibtnReturnBtnCompleteVoid_Click(ByVal sender As System.Web.UI.WebControls.ImageButton, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnReturnBtnCompleteVoid.Click
        '_udtAuditLogEntry.WriteLog(AuditLogDesc.ManagementClick_ID, AuditLogDesc.ManagementClick)

        ' Get Target Function Code
        Dim strTargetFunctCode As String = getReturnTargetFunctCode()

        ' Remove Session Variable
        Dim udtRedirectParameterBLL As New RedirectParameterBLL
        Dim udtRedirectParameter As RedirectParameterModel = udtRedirectParameterBLL.GetFromSession()
        If Not IsNothing(udtRedirectParameter) Then udtRedirectParameterBLL.RemoveFromSession()

        Dim btn As CustomControls.CustomImageButton = sender.Parent
        btn.TargetUrl = RedirectHandler.AppendPageKeyToURL((New BLL.MenuBLL).GetURL(strTargetFunctCode))
        btn.Redirect()
    End Sub

    Private Function getReturnTargetFunctCode() As String
        ' Returns the redirect function code of the return target function
        ' Returns String.Empty if redirect parameter, or the source function code is not found; else, returns the function code
        Dim udtRedirectParameterBLL As New RedirectParameterBLL
        Dim udtRedirectParameter As RedirectParameterModel = udtRedirectParameterBLL.GetReturnFromSession()
        If IsNothing(udtRedirectParameter) OrElse IsNothing(udtRedirectParameter.SourceFunctionCode) Then
            Return String.Empty
        Else
            Return udtRedirectParameter.TargetFunctionCode
        End If
    End Function

    Private Function getReturnTargetContainsSearch() As Boolean
        ' Returns if the return target function constains a Search action in its ActionList
        Dim udtRedirectParameterBLL As New RedirectParameterBLL
        Dim udtRedirectParameter As RedirectParameterModel = udtRedirectParameterBLL.GetReturnFromSession()
        If IsNothing(udtRedirectParameter) OrElse IsNothing(udtRedirectParameter.ActionList) Then
            Return False
        Else
            Return udtRedirectParameter.ActionList.Contains(RedirectParameterModel.EnumRedirectAction.Search)
        End If
    End Function

    Private Function getReturnTargetContainsViewDetail() As Boolean
        ' Returns if the return target function constains a ViewDetail action in its ActionList
        Dim udtRedirectParameterBLL As New RedirectParameterBLL
        Dim udtRedirectParameter As RedirectParameterModel = udtRedirectParameterBLL.GetReturnFromSession()
        If IsNothing(udtRedirectParameter) OrElse IsNothing(udtRedirectParameter.ActionList) Then
            Return False
        Else
            Return udtRedirectParameter.ActionList.Contains(RedirectParameterModel.EnumRedirectAction.ViewDetail)
        End If
    End Function

    Private Function getReturnTargetSearchCriteria() As Common.Component.RedirectParameter.SearchCriteriaCollection
        ' Returns the search criteria for the return target function 
        Dim udtRedirectParameterBLL As New RedirectParameterBLL
        Dim udtRedirectParameter As RedirectParameterModel = udtRedirectParameterBLL.GetReturnFromSession()
        If IsNothing(udtRedirectParameter) OrElse IsNothing(udtRedirectParameter.SearchCriteria) Then
            Return Nothing
        Else
            Return udtRedirectParameter.SearchCriteria
        End If
    End Function

    Private Function getReturnTargetTransactionID() As String
        ' Returns the search criteria for the return target function 
        ' Returns String.Emtpy if the redirect parameter, its return parameter, or its return parameter's search criteria is not found;
        ' otherwise, returns the Transaction ID
        Dim strTmp As String = String.Empty
        Dim udtRedirectParameterBLL As New RedirectParameterBLL
        Dim udtRedirectParameter As RedirectParameterModel = udtRedirectParameterBLL.GetReturnFromSession()
        If IsNothing(udtRedirectParameter) OrElse IsNothing(udtRedirectParameter.SearchCriteria) Then
            Return String.Empty
        Else
            If udtRedirectParameter.SearchCriteria.TryGetValue(ClaimTransactionMaintenance.SEARCH_PARAM_TRAN_ID, strTmp) Then
                Return strTmp
            Else
                Return String.Empty
            End If
        End If
    End Function
    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

#Region "Page Web Method"
    <WebMethod()> _
    <System.Web.Script.Services.ScriptMethod()> _
    Public Shared Function GetReasonForVisitL1(ByVal knownCategoryValues As String, ByVal category As String, ByVal contextKey As String) As CascadingDropDownNameValue()

        Dim udtReasonforVisitBLL As ReasonForVisitBLL = New ReasonForVisitBLL
        Dim dtReasonForVisit As DataTable

        dtReasonForVisit = udtReasonforVisitBLL.getReasonForVisitL1(category)

        Dim lst As New List(Of CascadingDropDownNameValue)
        For Each dr As DataRow In dtReasonForVisit.Rows
            If contextKey.ToUpper = English.ToUpper Then
                lst.Add(New CascadingDropDownNameValue(dr("Reason_L1"), dr("Reason_L1_Code")))
            ElseIf contextKey.ToUpper = SimpChinese.ToUpper Then
                lst.Add(New CascadingDropDownNameValue(dr("Reason_L1_CN"), dr("Reason_L1_Code")))
            Else
                lst.Add(New CascadingDropDownNameValue(dr("Reason_L1_Chi"), dr("Reason_L1_Code")))
            End If

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
            If contextKey.ToUpper = English.ToUpper Then
                lst.Add(New CascadingDropDownNameValue(dr("Reason_L2"), dr("Reason_L2_Code")))
            ElseIf contextKey.ToUpper = SimpChinese.ToUpper Then
                lst.Add(New CascadingDropDownNameValue(dr("Reason_L2_CN"), dr("Reason_L2_Code")))
            Else
                lst.Add(New CascadingDropDownNameValue(dr("Reason_L2_Chi"), dr("Reason_L2_Code")))
            End If

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
#End Region

#Region "Setup Search Criteria"

    Public Const SEARCH_PARAM_RECORD_STATUS As String = "RECORD_STATUS"
    Public Const SEARCH_PARAM_DATE_FROM As String = "DATE_FROM"
    Public Const SEARCH_PARAM_DATE_TO As String = "DATE_TO"
    Public Const SEARCH_PARAM_TRAN_ID As String = "TRAN_ID"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strRecordStatus">Claim Transaction Record Status (Common.Componemt.ClaimTransStatus)</param>
    ''' <param name="dtmFrom"></param>
    ''' <param name="dtmTo"></param>
    ''' <param name="strTranID">Claim Transaction ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function BuildSearchCriteria(ByVal strRecordStatus As String, ByVal dtmFrom As DateTime, ByVal dtmTo As DateTime, ByVal strTranID As String) As RedirectParameter.SearchCriteriaCollection
        Dim clln As New RedirectParameter.SearchCriteriaCollection
        clln.Add(SEARCH_PARAM_RECORD_STATUS, strRecordStatus)
        clln.Add(SEARCH_PARAM_DATE_FROM, dtmFrom)
        clln.Add(SEARCH_PARAM_DATE_TO, dtmTo)
        clln.Add(SEARCH_PARAM_TRAN_ID, strTranID)

        Return clln
    End Function
#End Region

    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    ' Popup legend again if legend data changed
    Private Sub udcSchemeLegend_DataChanged() Handles udcSchemeLegend.DataChanged
        popupSchemeNameHelp.Show()
    End Sub

    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

    Private Sub calExtTransactionDateFrom_Load(sender As Object, e As EventArgs) Handles calExtTransactionDateFrom.Load
        Dim selectedLang As String
        Dim chineseTodayDateFormat As String
        Dim udtSubPlatformBLL As New SubPlatformBLL

        selectedLang = LCase(Session("language"))
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Select Case selectedLang
        '    Case English
        '        Me.calExtTransactionDateFrom.TodaysDateFormat = "d MMMM, yyyy"
        '        Me.calExtTransactionDateFrom.DaysModeTitleFormat = "MMMM, yyyy"
        '    Case TradChinese, SimpChinese
        '        chineseTodayDateFormat = CStr(Today.Year) + "年" + CStr(Today.Month) + "月" + CStr(Today.Day) & "日"
        '        Me.calExtTransactionDateFrom.TodaysDateFormat = chineseTodayDateFormat
        '        Me.calExtTransactionDateFrom.DaysModeTitleFormat = "MMMM, yyyy"
        '    Case Else
        '        Me.calExtTransactionDateFrom.TodaysDateFormat = "dd-MM-yyyy"
        '        Me.calExtTransactionDateFrom.DaysModeTitleFormat = "MMMM, yyyy"
        'End Select

        Select Case selectedLang
            Case English
                Me.calExtTransactionDateFrom.TodaysDateFormat = "d MMMM, yyyy"
                Me.calExtTransactionDateFrom.DaysModeTitleFormat = "MMMM, yyyy"
                Me.calExtTransactionDateFrom.Format = udtFormatter.EnterDateFormat(udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
            Case TradChinese
                chineseTodayDateFormat = CStr(Today.Year) + "年" + CStr(Today.Month) + "月" + CStr(Today.Day) & "日"
                Me.calExtTransactionDateFrom.TodaysDateFormat = chineseTodayDateFormat
                Me.calExtTransactionDateFrom.DaysModeTitleFormat = "MMMM, yyyy"
                Me.calExtTransactionDateFrom.Format = udtFormatter.EnterDateFormat(udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
            Case SimpChinese
                chineseTodayDateFormat = CStr(Today.Year) + "年" + CStr(Today.Month) + "月" + CStr(Today.Day) & "日"
                Me.calExtTransactionDateFrom.TodaysDateFormat = chineseTodayDateFormat
                Me.calExtTransactionDateFrom.DaysModeTitleFormat = "MMMM, yyyy"
                Me.calExtTransactionDateFrom.Format = udtFormatter.EnterDateFormat(udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
            Case Else
                Me.calExtTransactionDateFrom.TodaysDateFormat = "dd-MM-yyyy"
                Me.calExtTransactionDateFrom.DaysModeTitleFormat = "MMMM, yyyy"
                Me.calExtTransactionDateFrom.Format = udtFormatter.EnterDateFormat(udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
        End Select
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

    End Sub

    Private Sub calExtTranDateTo_Load(sender As Object, e As EventArgs) Handles calExtTranDateTo.Load
        Dim selectedLang As String
        Dim chineseTodayDateFormat As String

        selectedLang = LCase(Session("language"))
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Select Case selectedLang
        '    Case English
        '        Me.calExtTranDateTo.TodaysDateFormat = "d MMMM, yyyy"
        '        Me.calExtTranDateTo.DaysModeTitleFormat = "MMMM, yyyy"
        '    Case TradChinese, SimpChinese
        '        chineseTodayDateFormat = CStr(Today.Year) + "年" + CStr(Today.Month) + "月" + CStr(Today.Day) & "日"
        '        Me.calExtTranDateTo.TodaysDateFormat = chineseTodayDateFormat
        '        Me.calExtTranDateTo.DaysModeTitleFormat = "MMMM, yyyy"
        '    Case Else
        '        Me.calExtTranDateTo.TodaysDateFormat = "dd-MM-yyyy"
        '        Me.calExtTranDateTo.DaysModeTitleFormat = "MMMM, yyyy"
        'End Select

        Select Case selectedLang
            Case English
                Me.calExtTranDateTo.TodaysDateFormat = "d MMMM, yyyy"
                Me.calExtTranDateTo.DaysModeTitleFormat = "MMMM, yyyy"
                Me.calExtTranDateTo.Format = "dd-MM-yyyy"
            Case TradChinese
                chineseTodayDateFormat = CStr(Today.Year) + "年" + CStr(Today.Month) + "月" + CStr(Today.Day) & "日"
                Me.calExtTranDateTo.TodaysDateFormat = chineseTodayDateFormat
                Me.calExtTranDateTo.DaysModeTitleFormat = "MMMM, yyyy"
                Me.calExtTranDateTo.Format = "dd-MM-yyyy"
            Case SimpChinese
                chineseTodayDateFormat = CStr(Today.Year) + "年" + CStr(Today.Month) + "月" + CStr(Today.Day) & "日"
                Me.calExtTranDateTo.TodaysDateFormat = chineseTodayDateFormat
                Me.calExtTranDateTo.DaysModeTitleFormat = "MMMM, yyyy"
                Me.calExtTranDateTo.Format = "yyyy-MM-dd"
            Case Else
                Me.calExtTranDateTo.TodaysDateFormat = "dd-MM-yyyy"
                Me.calExtTranDateTo.DaysModeTitleFormat = "MMMM, yyyy"
                Me.calExtTranDateTo.Format = "dd-MM-yyyy"
        End Select
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

    End Sub

    'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub FilterPracticeList(ByRef dtPracticeList As DataTable, ByVal PracticeList As PracticeModelCollection)
        Dim IsPracticeSchemeCoexist As Boolean
        Dim IsMatchPractice As Boolean
        Dim udtPracticeModel As PracticeModel

        Dim drPracticeList As DataRow
        Dim drRemoveRow As DataRow = dtPracticeList.NewRow()

        For intPracticeList As Integer = 1 To PracticeList.Count
            IsPracticeSchemeCoexist = False
            If intPracticeList <= PracticeList.Count Then
                udtPracticeModel = PracticeList.GetValueList(intPracticeList - 1)
                If udtPracticeModel.PracticeSchemeInfoList.Count > 0 Then
                    IsPracticeSchemeCoexist = True
                End If

                If Not IsPracticeSchemeCoexist Then
                    PracticeList.Remove(udtPracticeModel)
                    intPracticeList -= 1
                End If
            End If
        Next

        For intDtPracticeList As Integer = 1 To dtPracticeList.Rows.Count
            IsMatchPractice = False
            If intDtPracticeList <= dtPracticeList.Rows.Count Then
                drPracticeList = dtPracticeList.Rows(intDtPracticeList - 1)
                For intPracticeList As Integer = 1 To PracticeList.Count
                    udtPracticeModel = PracticeList.GetValueList(intPracticeList - 1)
                    If drPracticeList.Item("PracticeID") = udtPracticeModel.DisplaySeq Then
                        IsMatchPractice = True
                    End If
                Next

                If Not IsMatchPractice Then
                    dtPracticeList.Rows.Remove(drPracticeList)
                    intDtPracticeList -= 1
                End If
            End If
        Next

    End Sub
    'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

    'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub FilterDEPracticeList(ByRef dtPracticeList As DataTable, ByVal PracticeList As ArrayList)
        Dim IsPracticeSchemeCoexist As Boolean
        Dim IsMatchPractice As Boolean
        Dim udtPracticeModel As PracticeModel

        Dim drPracticeList As DataRow
        Dim drRemoveRow As DataRow = dtPracticeList.NewRow()

        For intPracticeList As Integer = 1 To PracticeList.Count
            IsPracticeSchemeCoexist = False
            If intPracticeList <= PracticeList.Count Then
                udtPracticeModel = PracticeList.Item(intPracticeList - 1)
                If udtPracticeModel.PracticeSchemeInfoList.Count > 0 Then
                    IsPracticeSchemeCoexist = True
                End If

                If Not IsPracticeSchemeCoexist Then
                    PracticeList.Remove(udtPracticeModel)
                    intPracticeList -= 1
                End If
            End If
        Next

        For intDtPracticeList As Integer = 1 To dtPracticeList.Rows.Count
            IsMatchPractice = False
            If intDtPracticeList <= dtPracticeList.Rows.Count Then
                drPracticeList = dtPracticeList.Rows(intDtPracticeList - 1)
                For intPracticeList As Integer = 1 To PracticeList.Count
                    udtPracticeModel = PracticeList.Item(intPracticeList - 1)
                    If drPracticeList.Item("PracticeID") = udtPracticeModel.DisplaySeq Then
                        IsMatchPractice = True
                    End If
                Next

                If Not IsMatchPractice Then
                    dtPracticeList.Rows.Remove(drPracticeList)
                    intDtPracticeList -= 1
                End If
            End If
        Next

    End Sub
    'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

End Class