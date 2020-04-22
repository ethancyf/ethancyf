Imports Common.ComObject
Imports System.Threading
Imports System.Globalization
Imports Common.Component.UserAC
Imports Common.Component
Imports Common.Component.ServiceProvider
Imports common.Component.DataEntryUser
Imports Common.ComInterface

Partial Public Class ClaimVoucherMaster
    Inherits System.Web.UI.MasterPage
    Implements IWorkingData

    Public Class FullVersionPage
        Public Const Login As String = "~/login.aspx"

        Public Const Home As String = "~/Home/home.aspx"
        Public Const Inbox As String = "~/Home/Inbox.aspx"

        Public Const ChangePassword As String = "~/ChangePassword/LoginChangePassword.aspx"

        Public Const EHSRectification As String = "~/EHSRectification/EHSRectification.aspx"

        ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        'Public Const RequestChangePassword As String = "~/ForgotPassword/RequestChangePassword.aspx"
        Public Const DataEntryRecoverLogin As String = "~/RecoverLogin/DataEntryRecoverLogin.aspx"
        Public Const RecoverLogin As String = "~/RecoverLogin/RecoverLogin.aspx"        
        ' CRE16-004 (Enable SP to unlock account) [End][Winnie]

        Public Const RecordConfirmation As String = "~/RecordConfirmation/RecordConfirmation.aspx"
    End Class

    Public Class ChildPage

        Public Const Home As String = "~/text/home.aspx"
        Public Const Login As String = "~/text/login.aspx"
        Public Const ChangePassword As String = "~/text/loginchangepassword.aspx"

        Public Const EHSClaim As String = "~/text/EHSClaimV1.aspx"
        Public Const EHSAccountCreation As String = "~/text/EHSAccountCreationV1.aspx"

        Public Const SelectBankAccount = "~/Text/SelectBankAccount.aspx"
        Public Const SearchAccount = "~/Text/ClaimVoucherSearchAccount.aspx"
        Public Const ConfirmAccount = "~/Text/ClaimVoucherConfirmAccount.aspx"
        Public Const EnterDetail = "~/Text/ClaimVoucherEnterDetail.aspx"
        Public Const SearchTransation = "~/Text/VoidClaimSearch.aspx"
        Public Const SelectTransation = "~/Text/VoidClaimSelectTransaction.aspx"
        Public Const ConfirmTransaction = "~/Text/VoidClaimConfirmTransaction.aspx"
    End Class

    Public Class ControlName
        Public Const panMenu As String = "panMenu"
        Public Const lblTitle As String = "lblTitle"
        Public Const lblSubTitle As String = "lblSubTitle"
        Public Const UDCInfoMessageBox As String = "udcInfoMsgBox"
        Public Const UDCMessageBoxError As String = "udcMsgBoxErr"
        Public Const LblClaimVoucherStep As String = "lblClaimVoucherStep"
    End Class

    Public Event MenuChanged(ByVal sender As Object, ByVal e As EventArgs)

    '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

    Private _FunctCodeCommon As String = Common.Component.FunctCode.FUNT029901

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Me.CheckConcurrentAccessForHttpGet()

    End Sub

    '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        Me.CheckConcurrentAccessForHttpPost()

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

        Dim udtUserAC As UserACModel = New UserACModel
        udtUserAC = UserACBLL.GetUserAC
        ' 2009-07-08: Check Access Right for Both SP & DataEntry
        'if loign user have no permit to access this page, throw exception
        Dim udtFunctInfoBLL As New Component.FunctionInformation.FunctionInformationBLL
        If Not udtFunctInfoBLL.ChkAccessRight(udtUserAC) Then
            Throw New Exception("Access denied")
        End If

        ''MessageBox Set up
        'If Not IsPostBack Then
        '    HideMessageBox()
        'End If
        'Language Set up
        Me.SetLangage()
        Me.lnkbtnLogout.Text = Me.GetGlobalResourceObject("AlternateText", "LogoutBtn")

        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetExpires(DateTime.Now.AddDays(-1))
        Response.Cache.SetNoServerCaching()

    End Sub

    Public Shared Sub ShowError(ByVal errorLabel As Label, ByVal systemMessage As SystemMessage, ByVal errorMessageBox As CustomControls.TextOnlyMessageBox, ByVal strHeaderKey As String, ByVal udtAuditLogEntry As AuditLogEntry, ByVal strLogID As String, ByVal strLogDesc As String)
        If Not errorLabel Is Nothing Then
            errorLabel.Visible = True
        End If


        errorMessageBox.AddMessage(systemMessage)

        If udtAuditLogEntry Is Nothing Then
            errorMessageBox.BuildMessageBox(strHeaderKey)
        Else
            errorMessageBox.BuildMessageBox(strHeaderKey, udtAuditLogEntry, strLogID, strLogDesc)
        End If
        errorMessageBox.Visible = True

    End Sub

    Private Sub SetLangage()
        Dim selectedValue As String = Session("language")
        Select Case selectedValue
            Case Common.Component.CultureLanguage.English
                lnkbtnEnglish.Visible = False
                Me.lblCurrentLanguageEnglish.Visible = True

                lnkbtnTradChinese.Visible = True
                Me.lblCurrentLanguageTradChinese.Visible = False
            Case Common.Component.CultureLanguage.TradChinese
                lnkbtnEnglish.Visible = True
                Me.lblCurrentLanguageEnglish.Visible = False

                lnkbtnTradChinese.Visible = False
                Me.lblCurrentLanguageTradChinese.Visible = True
        End Select
    End Sub

    Private Sub lnkbtnLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkbtnLogout.Click

        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC
        Dim strLogSPID As String = ""
        Dim strLogDataEntryAccount As String = ""
        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry("020004")

        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            Dim udtServiceProvider As ServiceProviderModel
            udtServiceProvider = CType(udtUserAC, ServiceProviderModel)
            udtAuditLogEntry.AddDescripton("User Account type", "Service Provider")
            udtAuditLogEntry.AddDescripton("SPID/User Name", udtServiceProvider.SPID)
            strLogSPID = udtServiceProvider.SPID
        Else
            Dim udtDataEntryUser As DataEntryUserModel
            udtDataEntryUser = CType(udtUserAC, DataEntryUserModel)
            udtAuditLogEntry.AddDescripton("User Account type", "Data Entry")
            udtAuditLogEntry.AddDescripton("SPID/Username", udtDataEntryUser.SPID)
            udtAuditLogEntry.AddDescripton("Data Entry User name", udtDataEntryUser.DataEntryAccount)
            strLogSPID = udtDataEntryUser.SPID
            strLogDataEntryAccount = udtDataEntryUser.DataEntryAccount
        End If
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00008, "Logout", strLogSPID, strLogDataEntryAccount)

        ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
        ' ----------------------------------------------------------
        HandleSessionVariable()
        ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

        Dim strSessionID As String = HttpContext.Current.Session.SessionID
        If strSessionID Is Nothing Then
            strSessionID = String.Empty
        End If

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        If Not RedirectHandler.IsTurnOnConcurrentBrowserHandling Then

            Dim udtLoginBLL As New BLL.LoginBLL()
            udtLoginBLL.ClearLoginSession(strSessionID)

        End If

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

        Me.Response.Redirect(ChildPage.Login)
    End Sub

    Public Sub ClearMenu()
        Me.panMenu.Controls.Clear()
    End Sub

    'Public Sub BuildMenu(ByVal currentPageFunctionCode As String, ByVal strLanguage As String, Optional ByVal udtUserAC As UserACModel = Nothing)
    '    ClaimVoucherMaster.BuildMenu(currentPageFunctionCode, Me.panMenu, strLanguage, udtUserAC)
    'End Sub

    Public Sub BuildMenu(ByVal currentPageFunctionCode As String, ByVal strLanguage As String, Optional ByVal udtUserAC As UserACModel = Nothing)
        Dim udtMenuBLL As BLL.MenuBLL = New BLL.MenuBLL()
        Dim dt As New DataTable
        Dim dtAvaliableMenu As New DataTable
        Dim strfunction As String
        Dim blnBuildMenu As Boolean = True
        dt = udtMenuBLL.GetMenuItem(Common.Component.HCSPMenuType.TextOnlyVersionMenu)
        Me.panMenu.Controls.Clear()

        Dim table As Table = New Table
        table.CellPadding = 0
        table.CellSpacing = 0

        For Each dr As DataRow In dt.Rows
            strfunction = dr("Function_Code").ToString().Trim()

            If udtUserAC Is Nothing Then
                blnBuildMenu = True
            Else
                Dim strFunctionURL As String = String.Empty
                blnBuildMenu = CheckAvalidableFunction(udtUserAC, strfunction, strFunctionURL)
            End If

            If blnBuildMenu Then

                If strfunction <> currentPageFunctionCode.Trim() Then

                    Dim tableRow As TableRow
                    Dim tableCell As TableCell

                    Dim lbtNmenu As Button
                    lbtNmenu = New Button
                    lbtNmenu.ID = String.Format("btnMenu{0}", dr("Menu_Name").ToString())
                    lbtNmenu.SkinID = "TextOnlyVersionLinkButton"
                    lbtNmenu.Attributes("UrlLocation") = dr("URL").ToString
                    If strLanguage.ToUpper.Equals(Common.Component.CultureLanguage.TradChinese.ToUpper()) Then
                        lbtNmenu.Text = dr("Menu_Name_Chi").ToString
                    Else
                        lbtNmenu.Text = dr("Menu_Name").ToString
                    End If

                    AddHandler lbtNmenu.Click, AddressOf lbtNmenu_Click

                    tableCell = New TableCell
                    tableCell.Controls.Add(lbtNmenu)

                    tableRow = New TableRow
                    tableRow.Cells.Add(tableCell)

                    table.Rows.Add(tableRow)
                    Me.panMenu.Controls.Add(table)
                End If
            End If
        Next

    End Sub

    Protected Sub lbtNmenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim lbtNmenu As Button = CType(sender, Button)
        RaiseEvent MenuChanged(sender, e)

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        RedirectHandler.ToURL(lbtNmenu.Attributes("UrlLocation"))

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End
    End Sub

    Public Shared Function CheckAvalidableFunction(ByVal udtUserAC As UserACModel, ByVal strFunctionCode As String, ByRef strFunctionURL As String) As Boolean
        Dim bShowByScheme As Boolean = False
        Dim udtSchemeInfoBLL As New Common.Component.SchemeInformation.SchemeInformationBLL
        Dim udtSchemeInfoList As SchemeInformation.SchemeInformationModelCollection
        Dim udtMenuBLL As BLL.MenuBLL = New BLL.MenuBLL()
        Dim dtMenu As DataTable = udtMenuBLL.GetMenuItem(Common.Component.HCSPMenuType.TextOnlyVersionMenu)
        Dim drFunction As DataRow = Nothing

        For Each dr As DataRow In dtMenu.Rows
            If dr("Function_Code") = strFunctionCode Then
                drFunction = dr
                strFunctionURL = drFunction("URL").ToString().Trim()
            End If
        Next

        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            Dim udtSP As ServiceProviderModel
            udtSP = CType(udtUserAC, ServiceProviderModel)
            udtSchemeInfoList = udtSchemeInfoBLL.GetSchemeInfoListPermanent(udtSP.SPID, New Common.DataAccess.Database)
        Else
            Dim udtDataEntryUser As DataEntryUserModel
            udtDataEntryUser = CType(udtUserAC, DataEntryUserModel)
            udtSchemeInfoList = udtSchemeInfoBLL.GetSchemeInfoListPermanent(udtDataEntryUser.SPID, New Common.DataAccess.Database)
        End If

        Return CheckMenuVisible(drFunction, udtSchemeInfoList)

    End Function

    Private Shared Function CheckMenuVisible(ByVal drMenuItem As DataRow, ByVal udtSchemeInfoList As SchemeInformation.SchemeInformationModelCollection) As Boolean

        ' Check The Login User Entitle Scheme (Scheme Enrol) -> (Scheme Claim)
        '   VS the permitted scheme of the Menu item

        Dim blnShowMenu As Boolean = False

        Dim udtSchemeClaimBLL As New Scheme.SchemeClaimBLL()
        Dim lstStrSchemeClaimCode As List(Of String) = udtSchemeClaimBLL.ConvertSchemeClaimCodeFromSchemeEnrol(udtSchemeInfoList)

        Dim strMenuSchemeCodeList As String = CStr(drMenuItem("Scheme_Code")).ToString().Trim().ToUpper()

        For Each strSchemeClaimCode As String In lstStrSchemeClaimCode
            ' Scheme_Code Contains 'ALL' refer to any
            Dim arrStrMenuSchemeCode As String() = strMenuSchemeCodeList.Split(",")

            For Each strMenuSchemeCode As String In arrStrMenuSchemeCode
                If strMenuSchemeCode.Trim().ToUpper().Equals("ALL") OrElse strMenuSchemeCode.Trim().ToUpper().Equals(strSchemeClaimCode.Trim()) Then
                    blnShowMenu = True
                    Exit For
                End If
            Next
        Next
        Return blnShowMenu
    End Function


#Region "Implement IWorkingData (CRE11-004)"

    Public Sub ClearWorkingData() Implements Common.ComInterface.IWorkingData.ClearWorkingData

    End Sub

    Public Function GetDocCode() As String Implements Common.ComInterface.IWorkingData.GetDocCode
        Return Nothing
    End Function

    Public Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel Implements Common.ComInterface.IWorkingData.GetEHSAccount
        Return Nothing
    End Function

    Public Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel Implements Common.ComInterface.IWorkingData.GetEHSTransaction
        Return Nothing
    End Function

    Public Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel Implements Common.ComInterface.IWorkingData.GetServiceProvider
        Return Nothing
    End Function

#End Region

    '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

    Private Sub CheckConcurrentAccessForHttpGet()
        If RedirectHandler.IsTurnOnConcurrentBrowserHandling Then
            If Not Me.Page.IsPostBack Then
                If Not Request.QueryString("PageKey") Is Nothing Then
                    CheckPageKey(Request.QueryString("PageKey").ToString)
                Else
                    RedirectToInvalidAccessErrorPage()
                End If
            End If
        End If
    End Sub

    Private Sub CheckConcurrentAccessForHttpPost()
        If RedirectHandler.IsTurnOnConcurrentBrowserHandling Then
            If Me.Page.IsPostBack Then
                If Not Me.PageKey Is Nothing Then
                    CheckPageKey(Me.PageKey.Text)
                Else
                    RedirectToInvalidAccessErrorPage()
                End If
            End If
        End If
    End Sub

    Public Sub CheckPageKey(ByVal strCurrentPageKey As String)
        If Not Session("PageKey") Is Nothing Then
            If Not strCurrentPageKey = String.Empty AndAlso strCurrentPageKey.ToString = Session("PageKey").ToString() Then
                RenewPageKey()
            Else
                RedirectToInvalidAccessErrorPage()
            End If
        End If
    End Sub

    Public Sub RenewPageKey()
        KeyGenerator.RenewSessionPageKey()
        Me.PageKey.Text = Session("PageKey").ToString()
    End Sub

    Public Sub RedirectToInvalidAccessErrorPage()

        Dim udtAuditLogEntry As New AuditLogEntry(Me._FunctCodeCommon, Me.Parent.Page)
        udtAuditLogEntry.AddDescripton("PageKey", Me.PageKey.Text)
        udtAuditLogEntry.WriteLog(LogID.LOG00001, "Redirect to invalid access error page")

        Dim strlink As String
        If Session("language") = "zh-tw" Then
            strlink = "~/text/zh/ImproperAccess.aspx"
        Else
            strlink = "~/text/en/ImproperAccess.aspx"
        End If
        Response.Redirect(strlink)
    End Sub

    '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

    ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Private Sub HandleSessionVariable()
        Dim Cache1 As String = Nothing
        Dim Cache2 As Boolean = Nothing
        Dim Cache3 As Boolean = Nothing

        '1. language
        If Not Session("language") Is Nothing Then
            Cache1 = Session("language")
        End If

        '2. Undefined User Agent
        Cache2 = CommonSessionHandler.AddedUndefinedUserAgent

        '3. Popup for remind obsoleted OS
        Cache3 = CommonSessionHandler.ReminderForWindowsVersion

        'Clear
        Session.RemoveAll()

        '1. language
        If Not Cache1 Is Nothing Then
            Session("language") = Cache1
        End If

        '2. Undefined User Agent
        CommonSessionHandler.AddedUndefinedUserAgent = Cache2

        '3. Popup for remind obsoleted OS
        CommonSessionHandler.ReminderForWindowsVersion = Cache3

    End Sub
    ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]


End Class