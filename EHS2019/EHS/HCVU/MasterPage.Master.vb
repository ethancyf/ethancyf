Imports Common.ComFunction
Imports Common.Component.UserAC
Imports Common.Component.HCVUUser
Imports HCVU.Component.Menu
Imports Common.Component.AccessRight
Imports HCVU.Component.FunctionInformation
Imports HCVU.BLL
Imports Common.ComObject
Imports Common.Component

Partial Public Class MasterPage
    Inherits System.Web.UI.MasterPage

    Dim strLastTimeCheck As String = "LastTimeCheck"
    Dim strLastCheckCount As String = "LastCheckCount"
    Dim udcInboxBll As New Common.Component.Inbox.InboxBLL
    Dim intNewMsgCount As Integer
    Dim udcGeneralF As New Common.ComFunction.GeneralFunction
    Dim ToleranceInMins As Integer

    ' CRE19-026 (HCVS hotline service) [Start][Winnie]
    ' ------------------------------------------------------------------------
    Private strRedirectorlink As String
    Private _FunctCodeCommon As String = Common.Component.FunctCode.FUNT029901

    Public Class PageURL
        Public Const Login As String = "~/login.aspx"
        Public Const Inbox As String = "~/Home/Inbox.aspx"
        Public Const Home As String = "~/Home/home.aspx"
        Public Const ImproperAccess As String = "~/ImproperAccess.aspx"
    End Class
    ' CRE19-026 (HCVS hotline service) [End][Winnie]

    ' CRE12-014 Relax 500 row limit in back office platform [Start][Twinsen]
    Public ReadOnly Property ContentTemplate() As ContentPlaceHolder
        Get
            Return Me.ContentPlaceHolder2
        End Get
    End Property

    Public ReadOnly Property UpdatePanelTemplate() As UpdatePanel
        Get
            Return Me.UpdatePanel1
        End Get
    End Property
    ' CRE12-014 Relax 500 row limit in back office platform [End][Twinsen]

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        Me.basetag.Attributes("href") = udcGeneralF.getPageBasePath()

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        'Ctrl N in Claim Account Creation
        'Problem: Call redirect before pagekey checking in content page load event
        'Solution: Put in master page init event (before content page load event)

        Me.CheckConcurrentAccessForHttpGet()
        'Me.CheckConcurrentAccessForHttpPost()

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        'Me.CheckConcurrentAccessForHttpGet()
        Me.CheckConcurrentAccessForHttpPost()

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
        Select Case DirectCast(Me.Page, BasePage).SubPlatform
            Case EnumHCVUSubPlatform.CC
                tblBanner.Style.Item("background-image") = "url(" + Me.GetGlobalResourceObject("ImageUrl", "BannerCallCentre").ToString + ")"
            Case EnumHCVUSubPlatform.VC
                tblBanner.Style.Item("background-image") = "url(" + Me.GetGlobalResourceObject("ImageUrl", "BannerVaccinationCentre").ToString + ")"
            Case Else
                tblBanner.Style.Item("background-image") = "url(" + Me.GetGlobalResourceObject("ImageUrl", "Banner") + ")"
        End Select
        ' CRE19-026 (HCVS hotline service) [End][Winnie]

        Dim strvalue As String = String.Empty
        udcGeneralF.getSystemParameter("InboxRefreshMinute", strvalue, String.Empty)
        ToleranceInMins = CInt(strvalue)

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL
        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

        Dim udtFunctionInformationBLL As New FunctionInformationBLL
        Dim strFunctionCode As String = udtFunctionInformationBLL.GetFunctionCode()
        If Not udtHCVUUser.AccessRightCollection.Item(strFunctionCode).Allow Then
            Throw New Exception("Access denied")
        End If

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        ' -------------------------------------------------------------
        If udtHCVUUser.AccessRightCollection.Item(FunctCode.FUNT010801).Allow Then
            ibtnChangePW.Visible = True
        Else
            ibtnChangePW.Visible = False
        End If
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Koala]

        ' CRE15-006 Rename of eHS [Start][Lawrence]
        lblAppEnvironment.Text = (New GeneralFunction).getSystemParameter("AppEnvironment")

        If lblAppEnvironment.Text.ToLower = "production" Then lblAppEnvironment.Text = String.Empty
        ' CRE15-006 Rename of eHS [End][Lawrence]

        If Me.ibtnHome.Visible Then
            Me.lblWelcome.Text = "&nbsp;" & Me.GetGlobalResourceObject("Text", "Welcome")
        End If
        Me.lblLoginName.Text = udtHCVUUser.UserName

        Dim udtMenuController As New MenuControlBLL

        ' Clear Then Menu Item Before Build
        'Me.caMenu.Items.Clear()
        'Diable viewstate of the menu
        'caMenu.EnableViewState = False

        ' CRE20-0023 (Immu record) [Start][Winnie SUEN]
        Dim blnHideDisabledMenuItem As Boolean = False
        If Session("HCVU_UserType") = "HCSPUser" Then
            blnHideDisabledMenuItem = True
        End If

        udtMenuController.BuildMenu(Me.ulMenu, DirectCast(Me.Page, BasePage).SubPlatform, blnHideDisabledMenuItem)
        ' CRE20-0023 (Immu record) [End][Winnie SUEN]

        ' Logic To Check New Message In Inbox:
        ' Check New Message Between Last Retrive Time & Current Time
        ' If message Counter > 0, Set Alert
        ' Clear the Alert only when Enter Inbox

        ' If User Click InBox: Set Previous Check Time = Entry InBox Time

        Dim dtmCurrentTime As DateTime = Now 'udcGeneralF.GetSystemDateTime()

        If Session(Me.strLastTimeCheck) Is Nothing Then Session(Me.strLastTimeCheck) = dtmCurrentTime
        If Session(Me.strLastCheckCount) Is Nothing Then Session(Me.strLastCheckCount) = 0


        Dim tsDiff As New TimeSpan
        tsDiff = dtmCurrentTime - CType(Session(Me.strLastTimeCheck), DateTime)

        Dim blnAlertOn As Boolean = False

        Dim intAccumNewMsgCount As Integer = 0

        If Convert.ToInt32(Session(Me.strLastCheckCount)) > 0 Then
            blnAlertOn = True
        End If

        If (Math.Abs(tsDiff.TotalMinutes) > ToleranceInMins) Then
            intNewMsgCount = udcInboxBll.GetNewMessageCount(udtHCVUUser.UserID, CType(Session(Me.strLastTimeCheck), DateTime), dtmCurrentTime)
            Session(Me.strLastTimeCheck) = dtmCurrentTime
            'Session(Me.strLastCheckCount) = 
            intAccumNewMsgCount = Convert.ToInt32(Session(Me.strLastCheckCount)) + intNewMsgCount
            Session(Me.strLastCheckCount) = intAccumNewMsgCount
            If intAccumNewMsgCount > 0 Then
                blnAlertOn = True
            End If
        End If

        If blnAlertOn Then
            'Update the inbox button image and alternate text
            Me.ibtnInbox.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "InboxRedBtn")
            Dim strNewMsgText As String
            strNewMsgText = HttpContext.GetGlobalResourceObject("AlternateText", "InboxRedBtn")
            Me.ibtnInbox.AlternateText = strNewMsgText.Replace("%s", Convert.ToInt32(Session(Me.strLastCheckCount)))
        Else
            Me.ibtnInbox.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "InboxBtn")
            Me.ibtnInbox.AlternateText = HttpContext.GetGlobalResourceObject("AlternateText", "InboxBtn")
        End If

        ' CRE20-023-29 (Immu record) [Start][Winnie]
        Select Case DirectCast(Me.Page, BasePage).SubPlatform
            Case EnumHCVUSubPlatform.CC
                Me.ibtnInbox.Visible = False

            Case EnumHCVUSubPlatform.VC
                Me.ibtnInbox.Visible = False

            Case Else
                Me.ibtnInbox.Visible = True
        End Select

        'If Session("HCVU_UserType") = "HCSPUser" Then
        '    Me.ibtnInbox.Visible = False
        'Else
        '    Me.ibtnInbox.Visible = True
        'End If
        ' CRE20-023-29 (Immu record) [End][Winnie]

        ' Wait Cursor Panel Script
        ScriptManager.RegisterStartupScript(Page, Me.GetType, "ModalUpdProg", Me.GetWaitCursorPanelScript(), True)

        'Dim strScript As String = ""
        'strScript &= "<script type='text/javascript'>" & vbCrLf
        'strScript &= "function MenuClick(url){" & vbCrLf
        'strScript &= "$find('" & Me.ModalPopupExtender1.ClientID & "').show();" & vbCrLf
        'strScript &= "setTimeout(" & Chr(34) & "document.getElementById('" & Me.pnlPleaseWait.ClientID & "').style.visibility='visible'" & Chr(34) & ", 1000);" & vbCrLf
        ''strScript &= "setTimeout(" & Chr(34) & "window.location = '" & Chr(34) & " + url + " & Chr(34) & "'" & Chr(34) & ", 1);" & vbCrLf
        'strScript &= "window.location = url" & vbCrLf
        'strScript &= "}" & vbCrLf
        'strScript &= "</script>"

        'Me.Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "Menu_Click", strScript)

        Dim strHost As String = Request.Url.ToString.Substring(0, Request.Url.ToString.IndexOf(Request.Path))
        Dim strPrivacyPolicyLink As String = strHost & Request.ApplicationPath
        If strPrivacyPolicyLink.Substring(strPrivacyPolicyLink.Length - 1, 1) <> "/" Then
            strPrivacyPolicyLink = strPrivacyPolicyLink & "/PrivacyPolicy/PrivacyPolicy.htm"
        Else
            strPrivacyPolicyLink = strPrivacyPolicyLink & "PrivacyPolicy/PrivacyPolicy.htm"
        End If

        Me.lnkBtnPrivacyPolicy.OnClientClick = "javascript:openNewHTML('" + strPrivacyPolicyLink + "');return false;"

        Dim strDisclaimerPolicyLink As String = strHost & Request.ApplicationPath
        If strDisclaimerPolicyLink.Substring(strDisclaimerPolicyLink.Length - 1, 1) <> "/" Then
            strDisclaimerPolicyLink = strDisclaimerPolicyLink & "/Disclaimer/Disclaimer.htm"
        Else
            strDisclaimerPolicyLink = strDisclaimerPolicyLink & "Disclaimer/Disclaimer.htm"
        End If

        Me.lnkBtnDisclaimer.OnClientClick = "javascript:openNewHTML('" + strDisclaimerPolicyLink + "');return false;"

        Dim strSysMaintLink As String = strHost & Request.ApplicationPath
        If strSysMaintLink.Substring(strSysMaintLink.Length - 1, 1) <> "/" Then
            strSysMaintLink = strSysMaintLink & "/SystemMaint/SysMaint.htm"
        Else
            strSysMaintLink = strSysMaintLink & "SystemMaint/SysMaint.htm"
        End If
        Me.lnkBtnSysMaint.OnClientClick = "javascript:openNewHTML('" + strSysMaintLink + "');return false;"

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        SetupClock()
        SetupTimeoutReminder()
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Koala]
    End Sub

    Private Sub ibtnLogout_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnLogout.Click

        Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT010001)
        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL
        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

        udtAuditLogEntry.AddDescripton("User_ID", udtHCVUUser.UserID)
        udtAuditLogEntry.WriteLog(LogID.LOG00006, "Logout")

        Dim selectedValue As String
        selectedValue = Session("language")
        'I-CRE16-006 (Capture detail client browser and OS information) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim blnSESSUUA As Boolean = CommonSessionHandler.AddedUndefinedUserAgent
        'I-CRE16-006 (Capture detail client browser and OS information) [End][Chris YIM]

        Dim strUserType As String = Session("HCVU_UserType")

        Session.RemoveAll()

        Session("language") = selectedValue
        'I-CRE16-006 (Capture detail client browser and OS information) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        CommonSessionHandler.AddedUndefinedUserAgent = blnSESSUUA
        'I-CRE16-006 (Capture detail client browser and OS information) [End][Chris YIM]

        If strUserType = "HCSPUser" Then
            Response.Redirect("~/SPlogin.aspx")
        Else
            Response.Redirect("~/login.aspx")
        End If

    End Sub

    Private Sub ibtnHome_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnHome.Click
        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
        ' ------------------------------------------------------------------------
        'Response.Redirect("~/Home/home.aspx")
        RedirectHandler.ToURL(PageURL.Home)
        ' CRE19-026 (HCVS hotline service) [End][Winnie]
    End Sub

    Protected Sub ibtnInbox_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnInbox.Click
        'Session(Me.strLastTimeCheck) = DateTime.Now.AddMinutes(-100)
        'Reset the session variables when inbox page is clicked
        Session(Me.strLastTimeCheck) = Now  'udcGeneralF.GetSystemDateTime
        Session(Me.strLastCheckCount) = 0

        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
        ' ------------------------------------------------------------------------
        'Response.Redirect("~/Home/Inbox.aspx")
        RedirectHandler.ToURL(PageURL.Inbox)
        ' CRE19-026 (HCVS hotline service) [End][Winnie]
    End Sub

    Private Function GetWaitCursorPanelScript() As String

        Dim strPleaseWaitScript As New StringBuilder()
        strPleaseWaitScript.Append("function ModalUpdProgInitialize(sender, args) {")
        strPleaseWaitScript.Append("var upd = $find('" & Me.UpdateProgress1.ClientID & "');")

        ' Clear the end handler and re-add
        strPleaseWaitScript.Append("upd._pageRequestManager.remove_endRequest(upd._endRequestHandlerDelegate);")

        strPleaseWaitScript.Append("upd._endRequestHandlerDelegate = Function.createDelegate(upd, ModalUpdProgEndRequest);")
        strPleaseWaitScript.Append("upd._startDelegate = Function.createDelegate(upd, ModalUpdProgStartRequest);")
        strPleaseWaitScript.Append("upd._pageRequestManager.add_endRequest(upd._endRequestHandlerDelegate);}")
        strPleaseWaitScript.Append("function ModalUpdProgStartRequest() {")
        strPleaseWaitScript.Append("$('#" + Me.pnlPleaseWait.ClientID + "').show();")

        ' Bring "Please Wait" Image to foreground and block click action in background
        strPleaseWaitScript.Append("if (this._pageRequestManager.get_isInAsyncPostBack()) {")
        strPleaseWaitScript.Append("document.getElementById('" & Me.pnlPleaseWait.ClientID & "').style.visibility='hidden';")
        strPleaseWaitScript.Append("this._timerCookie = setTimeout(ShowPleaseWait, 2000);}}")
        strPleaseWaitScript.Append("function ShowPleaseWait() {")

        strPleaseWaitScript.Append("document.getElementById('" & Me.pnlPleaseWait.ClientID & "').style.visibility='visible';")
        strPleaseWaitScript.Append("document.getElementById('" & Me.pnlPleaseWait.ClientID & "').style.height = document.documentElement.clientHeight + document.documentElement.scrollTop;")
        strPleaseWaitScript.Append("}")
        strPleaseWaitScript.Append("function ModalUpdProgEndRequest(sender, arg) {")

        strPleaseWaitScript.Append("document.getElementById('" & Me.pnlPleaseWait.ClientID & "').style.visibility='hidden';")
        strPleaseWaitScript.Append("window.clearTimeout(this._timerCookie);")
        strPleaseWaitScript.Append("if (this._timerCookie) {")
        strPleaseWaitScript.Append("document.getElementById('" & Me.pnlPleaseWait.ClientID & "').style.visibility='hidden';")
        strPleaseWaitScript.Append("window.clearTimeout(this._timerCookie);")
        strPleaseWaitScript.Append("this._timerCookie = null;}}")
        strPleaseWaitScript.Append("Sys.Application.add_load(ModalUpdProgInitialize);")

        Return strPleaseWaitScript.ToString()

    End Function

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    Private Sub SetupClock()
        Dim dtm As DateTime = (New Common.ComFunction.GeneralFunction).GetSystemDateTime()
        Dim strFormat As String = (New Common.Format.Formatter).DisplayClockFormat()
        Dim strFormatTime As String = (New Common.Format.Formatter).DisplayClockTimeFormat()
        Dim strDtm As String = dtm.ToString(strFormat)
        Me.lblClock.Text = strDtm
        Me.Page.ClientScript.RegisterStartupScript(Me.GetType(), "SetupClock", String.Format("Clock.StartClock('{0}','{1}','{2}');", _
                                                                                                    Me.lblClock.ClientID, _
                                                                                                    strFormat, _
                                                                                                    strFormatTime), True)

    End Sub

    Private Sub SetupTimeoutReminder()

        ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
        ' -------------------------------------------------------------------------------
        'Me.Page.ClientScript.RegisterStartupScript(Me.GetType, "SetupTimeoutReminder", String.Format("javascript: StartTimeoutReminder('{0}','{1}','{2}','{3}','{4}');", _
        '                                               New String() {(Session.Timeout * 60).ToString, _
        '                                                            Me.udcGeneralF.GetTimeoutReminderDisplayTime(), _
        '                                                            String.Empty, _
        '                                                            Me.ModalPopupExtenderTimeoutReminder.BehaviorID, _
        '                                                            Me.ucNoticePopUpTimeoutReminder.MessageLabel.ClientID}), True)

        Me.Page.ClientScript.RegisterStartupScript(Me.GetType, "SetupTimeoutReminder", String.Format("javascript: StartTimeoutReminder('{0}','{1}','{2}','{3}','{4}');", _
                                                New String() {(Session.Timeout * 60).ToString, _
                                                              Me.udcGeneralF.GetTimeoutReminderDisplayTime(), _
                                                              String.Empty, _
                                                              Me.panTimeoutReminder.ClientID, _
                                                              Me.ucNoticePopUpTimeoutReminder.MessageLabel.ClientID}), True)


        Me.ucNoticePopUpTimeoutReminder.ButtonOK.Attributes.Add("onclick", "ReminderOK_Click();return false;")
        'Me.ModalPopupExtenderTimeoutReminder.OkControlID = Me.ucNoticePopUpTimeoutReminder.ButtonOK.ClientID
        'Me.ModalPopupExtenderTimeoutReminder.PopupDragHandleControlID = Me.ucNoticePopUpTimeoutReminder.Header.ClientID
        ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]

    End Sub
    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Koala]

    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Koala]
    ' -------------------------------------------------------------
    Protected Sub ibtnChangePW_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtMenuBLL As New MenuBLL

        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
        ' ------------------------------------------------------------------------
        'Response.Redirect(udtMenuBLL.GetURLByFunctionCode(FunctCode.FUNT010801))
        RedirectHandler.ToURL(udtMenuBLL.GetURLByFunctionCode(FunctCode.FUNT010801))
        ' CRE19-026 (HCVS hotline service) [End][Winnie]
    End Sub
    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Koala]

    ' CRE19-026 (HCVS hotline service) [Start][Winnie]
    ' ------------------------------------------------------------------------
    Protected Sub lbtnMenuItem_Click(ByVal sender As Object, ByVal e As CommandEventArgs)
        RedirectHandler.ToURL(e.CommandArgument)
    End Sub
    ' CRE19-026 (HCVS hotline service) [End][Winnie]

#Region "Concurrent Access Checking"
    ' CRE19-026 (HCVS hotline service) [Start][Winnie]
    ' ------------------------------------------------------------------------
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
        If Not Session(BasePage.SESS_PageKey) Is Nothing Then
            If Not strCurrentPageKey = String.Empty AndAlso strCurrentPageKey.ToString = Session(BasePage.SESS_PageKey).ToString() Then
                RenewPageKey()
            Else
                RedirectToInvalidAccessErrorPage()
            End If
        End If
    End Sub

    Public Sub RenewPageKey()
        KeyGenerator.RenewSessionPageKey()
        Me.PageKey.Text = Session(BasePage.SESS_PageKey).ToString()
    End Sub

    Public Sub RedirectToInvalidAccessErrorPage()

        Dim udtAuditLogEntry As AuditLogEntry = Nothing
        If TypeOf Me.Parent.Page Is Common.ComInterface.IWorkingData Then
            udtAuditLogEntry = New AuditLogEntry(_FunctCodeCommon, Me.Parent.Page)
        Else
            udtAuditLogEntry = New AuditLogEntry(_FunctCodeCommon)
        End If

        udtAuditLogEntry.AddDescripton("PageKey", Me.PageKey.Text)
        udtAuditLogEntry.WriteLog(LogID.LOG00001, "Redirect to invalid access error page")

        Response.Redirect(PageURL.ImproperAccess)
    End Sub
    ' CRE19-026 (HCVS hotline service) [End][Winnie]

#End Region

End Class