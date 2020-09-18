Imports Common.ComFunction
Imports Common.Component.UserAC
Imports Common.Component.ServiceProvider
Imports Common.Component.DataEntryUser
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.Scheme
Imports Common.Component.SchemeInformation
Imports Common.ComObject
Imports Common.Component
Imports HCSP.Component.FunctionInformation
Imports HCSP.BasePage
Imports SSODataType
Imports SSODAL
Imports SSOUtil
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates

Partial Public Class MasterPage
    Inherits System.Web.UI.MasterPage

    Dim strLastTimeCheck As String = "LastTimeCheck"
    Dim strLastCheckCount As String = "LastCheckCount"
    Dim udcInboxBll As New Common.Component.Inbox.InboxBLL
    Dim intNewMsgCount As Integer
    Dim udcGeneralF As New Common.ComFunction.GeneralFunction
    Dim ToleranceInMins As Integer

    Private Const SESS_DataEntryAccount As String = "DataEntryAccount"

    'For PPIEPR SSO
    Private strPPIePRlink As String

    '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

    Private strRedirectorlink As String
    Private _FunctCodeCommon As String = Common.Component.FunctCode.FUNT029901

    '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

    Private strLocalSSOAppId As String = ""
    Private arrRelatedSSOAppIds As String() = Nothing
    Private strSSOAccessDeniedPageUrl As String = ""
    Private strSSOApplicationErrorPageUrl As String = ""
    Private strSSOError As String = ""

    ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Private udcSessionHandler As New BLL.SessionHandler
    ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

#Region "Properties"

    ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
    Protected ReadOnly Property PageLanguage() As String
        Get
            If DirectCast(Me.Page, BasePage).SubPlatform = EnumHCSPSubPlatform.CN Then
                Return "lang=""zh"""
            Else
                Return String.Empty
            End If
        End Get
    End Property
    ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

#End Region

#Region "SSO related Sub/Functions"
    'For PPIEPR SSO
    Private Sub loadConfig()
        strSSOApplicationErrorPageUrl = SSOUtil.SSOAppConfigMgr.getSSOApplicationErrorPageUrl()

        strSSOAccessDeniedPageUrl = SSOUtil.SSOAppConfigMgr.getSSOAccessDeniedPageUrl()
        If strSSOAccessDeniedPageUrl Is Nothing Or strSSOAccessDeniedPageUrl.Trim() = "" Then
            strSSOError = "SSO_ACCESS_DENIED_PAGE_URL_NOT_DEFINED"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOError)
            'SSOInterfacingLib.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOError))
            Response.Redirect(strSSOApplicationErrorPageUrl)

        End If

        strLocalSSOAppId = SSOUtil.SSOAppConfigMgr.getSSOAppId()
        If strLocalSSOAppId Is Nothing Or strLocalSSOAppId.Trim() = "" Then
            strSSOError = "SSO_LOCAL_SSO_APP_ID_NOT_DEFINED"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOError)
            'SSOInterfacingLib.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOError))
            Response.Redirect(strSSOApplicationErrorPageUrl)
        End If


        arrRelatedSSOAppIds = SSOUtil.SSOAppConfigMgr.getSSORelatedAppIds()
        If arrRelatedSSOAppIds Is Nothing Then
            strSSOError = "SSO_RELATED_SSO_APP_IDS_NOT_DEFINED"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOError)
            'SSOInterfacingLib.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOError))
            Response.Redirect(strSSOApplicationErrorPageUrl)
        End If

        Dim strLocalSSOMgr As String = SSOUtil.SSOAppConfigMgr.getSSOMgrUrl(strLocalSSOAppId)
        Dim strSSOPreLoadUrl As String = SSOUtil.SSOAppConfigMgr.getSSOPreLoadUrl(arrRelatedSSOAppIds(0))
        Dim strSSOLink As String = strLocalSSOMgr + "?SSOTargetSiteSSOAppId=" + arrRelatedSSOAppIds(0)
        strPPIePRlink = strSSOPreLoadUrl + "?SSO_Artifact_Generator_Url=" + strSSOLink

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        strRedirectorlink = "../SSOModule/Redirector.aspx" + "?SSO_Artifact_Generator_Url=" + strSSOLink + "&Target_Url=" + strSSOPreLoadUrl

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

    End Sub

#End Region

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Dim udcGeneralFun = New Common.ComFunction.GeneralFunction()
        Me.basetag.Attributes("href") = udcGeneralFun.getPageBasePath()

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        'Ctrl N in Claim Account Creation
        'Problem: Call redirect before pagekey checking in content page load event
        'Solution: Put in master page init event (before content page load event)

        Me.CheckConcurrentAccessForHttpGet()
        'Me.CheckConcurrentAccessForHttpPost()

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If panMenu.Visible Then

            ibtnMenu.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ExpandedMenuImg")
            ibtnMenu.AlternateText = Me.GetGlobalResourceObject("ToolTip", "ExpandedMenuImg")

            BuildMenu()
        Else
            ibtnMenu.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "CollapsedMenuImage")
            ibtnMenu.AlternateText = Me.GetGlobalResourceObject("ToolTip", "CollapsedMenuImage")
        End If

        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
        ' Hide Inbox in China Platform
        If DirectCast(Me.Page, BasePage).SubPlatform = EnumHCSPSubPlatform.CN Then
            btnInbox.Visible = False
            lnkbtnTradChinese.Visible = False
            lnkbtnEnglish.Visible = False
        End If
        ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        'Me.CheckConcurrentAccessForHttpGet()
        Me.CheckConcurrentAccessForHttpPost()

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

        Dim strSPName As String = String.Empty

        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        lnkBtnPrivacyPolicy.Text = Me.GetGlobalResourceObject("Text", "PrivacyPolicy")
        lnkBtnDisclaimer.Text = Me.GetGlobalResourceObject("Text", "ImportantNotices")
        lnkBtnSysMaint.Text = Me.GetGlobalResourceObject("Text", "SysMaint")

        setLangageStyle()

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim hlCommonStyleCSS As HtmlLink = New HtmlLink
        Dim hlMenuStyleCSS As HtmlLink = New HtmlLink
        If DirectCast(Me.Page, BasePage).SubPlatform = EnumHCSPSubPlatform.CN Then
            hlCommonStyleCSS.Href = "CSS/CommonStyle_cn.css"
            hlMenuStyleCSS.Href = "CSS/MenuStyle_cn.css"
        Else
            hlCommonStyleCSS.Href = "CSS/CommonStyle.css"
            hlMenuStyleCSS.Href = "CSS/MenuStyle.css"
        End If

        ' CRE15-006 Rename of eHS [Start][Lawrence]
        lblAppEnvironment.Text = (New GeneralFunction).getSystemParameter("AppEnvironment")

        If lblAppEnvironment.Text.ToLower = "production" Then lblAppEnvironment.Text = String.Empty

        Select Case Session("language")
            Case CultureLanguage.TradChinese, CultureLanguage.SimpChinese
                tdAppEnvironment.Attributes("class") = "AppEnvironmentZH"
            Case Else
                tdAppEnvironment.Attributes("class") = "AppEnvironment"
        End Select
        ' CRE15-006 Rename of eHS [End][Lawrence]

        'Add CommonStyle CSS File
        hlCommonStyleCSS.Attributes.Add("rel", "stylesheet")
        hlCommonStyleCSS.Attributes.Add("type", "text/css")
        Me.Page.Header.Controls.Add(hlCommonStyleCSS)

        'Add MenuStyle CSS File
        hlMenuStyleCSS.Attributes.Add("rel", "stylesheet")
        hlMenuStyleCSS.Attributes.Add("type", "text/css")
        Me.Page.Header.Controls.Add(hlMenuStyleCSS)
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC()
        Dim udtSP As ServiceProviderModel

        Me.lblWelcome.Text = Me.GetGlobalResourceObject("Text", "Username")

        ' Check Access Right
        Dim udtFunctInfoBLL As New FunctionInformationBLL()
        If Not udtFunctInfoBLL.ChkAccessRight(udtUserAC) Then
            Throw New Exception("Access denied")
        End If

        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            Me.ibtnChangeSP.Visible = False
            Me.btnInbox.Visible = True
            udtSP = CType(udtUserAC, ServiceProviderModel)

            If Session("language") = CultureLanguage.TradChinese OrElse Session("language") = CultureLanguage.SimpChinese Then
                If udtSP.ChineseName.Trim.Equals(String.Empty) Then
                    strSPName = udtSP.EnglishName
                Else
                    strSPName = udtSP.ChineseName
                End If
            Else
                strSPName = udtSP.EnglishName
            End If

            Me.lblLoginName.Text = strSPName

        Else

            Dim udtDataEntryUser As DataEntryUserModel = CType(udtUserAC, DataEntryUserModel)
            Me.btnInbox.Visible = False

            Me.lblLoginName.Text = udtDataEntryUser.DataEntryAccount

            If Session("language") = CultureLanguage.TradChinese OrElse Session("language") = CultureLanguage.SimpChinese Then
                If udtDataEntryUser.SPChiName.Trim.Equals(String.Empty) Then
                    strSPName = udtDataEntryUser.SPEngName
                Else
                    strSPName = udtDataEntryUser.SPChiName
                End If
            Else
                strSPName = udtDataEntryUser.SPEngName
            End If

            Me.lblLoginName.Text &= " (" & Me.GetGlobalResourceObject("Text", "for") & " " & strSPName & ")"

        End If

        If Session("language") = CultureLanguage.TradChinese OrElse Session("language") = CultureLanguage.SimpChinese Then
            Me.lblLoginName.CssClass = "bannerTextChi"
        Else
            Me.lblLoginName.CssClass = "bannerText"
        End If

        Me.tblBanner.Style.Item("background-image") = "url(" + Me.GetGlobalResourceObject("ImageUrl", "Banner").ToString + ")"

        If Not IsPostBack Then
            Dim strHomePage As String = String.Empty
            Dim strCurrentPage As String = String.Empty

            strHomePage = "home.aspx"
            strCurrentPage = Request.ServerVariables("URL").ToLower

            If Not strCurrentPage.Equals(String.Empty) Then
                strCurrentPage = strCurrentPage.Substring(strCurrentPage.LastIndexOf("/") + 1)
            End If

            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

            If strHomePage = strCurrentPage Or RedirectHandler.AppendPageKeyToURL(strHomePage).ToLower = strCurrentPage Then
                panMenu.Visible = True
            Else
                panMenu.Visible = False
            End If

            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

            preventMultiImgClick(Page.ClientScript, Me.ibtnSSOConfirmOK)

        End If


        Dim udtMenuBLL As BLL.MenuBLL = New BLL.MenuBLL

        If panMenu.Visible Then
            Me.BuildMenu()
        End If

        'Logic to update the inbox button
        udtUserAC = UserACBLL.GetUserAC

        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            Dim udtServiceProvider As ServiceProviderModel
            udtServiceProvider = CType(UserACBLL.GetUserAC, ServiceProviderModel)

            Dim strvalue As String = String.Empty
            udcGeneralF.getSystemParameter("InboxRefreshMinute", strvalue, String.Empty)
            ToleranceInMins = CInt(strvalue)

            Dim dtmCurrentTime As DateTime = Now

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
                intNewMsgCount = udcInboxBll.GetNewMessageCount(udtServiceProvider.SPID, CType(Session(Me.strLastTimeCheck), DateTime), dtmCurrentTime)
                Session(Me.strLastTimeCheck) = dtmCurrentTime
                intAccumNewMsgCount = Convert.ToInt32(Session(Me.strLastCheckCount)) + intNewMsgCount
                Session(Me.strLastCheckCount) = intAccumNewMsgCount
                If intAccumNewMsgCount > 0 Then
                    blnAlertOn = True
                End If
            End If

            If blnAlertOn Then
                'Update the inbox button image and alternate text
                Me.btnInbox.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "InboxRedBtn")
                Dim strNewMsgText As String
                strNewMsgText = HttpContext.GetGlobalResourceObject("AlternateText", "InboxRedBtn")
                Me.btnInbox.AlternateText = strNewMsgText.Replace("%s", Convert.ToInt32(Session(Me.strLastCheckCount)))
            Else
                Me.btnInbox.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "InboxBtn")
                Me.btnInbox.AlternateText = HttpContext.GetGlobalResourceObject("AlternateText", "InboxBtn")
            End If
        End If

        ' Wait Cursor Panel Script
        ScriptManager.RegisterStartupScript(Page, Me.GetType, "ModalUpdProg", Me.GetWaitCursorPanelScript(), True)

        ' Timeout Warning Script
        'ScriptManager.RegisterStartupScript(Page, Me.GetType, "TimeoutWarning", Me.GetTimeoutWarningScript(), True)

        Dim selectedValue As String = LCase(Session("language"))

        ' Link Javascript
        Dim strPrivacyPolicyLink As String = String.Empty

        Select Case Session("language")
            Case CultureLanguage.English
                udcGeneralF.getSystemParameter("PrivacyPolicyLink", strPrivacyPolicyLink, String.Empty)
            Case CultureLanguage.TradChinese
                udcGeneralF.getSystemParameter("PrivacyPolicyLink_CHI", strPrivacyPolicyLink, String.Empty)
            Case CultureLanguage.SimpChinese
                udcGeneralF.getSystemParameter("PrivacyPolicyLink_CN", strPrivacyPolicyLink, String.Empty)
            Case Else
                Throw New Exception(String.Format("Unexpected value (Session(language)={0})", Session("language")))
        End Select

        Me.lnkBtnPrivacyPolicy.OnClientClick = "javascript:openNewHTML('" + strPrivacyPolicyLink + "');return false;"

        Dim strDisclaimerPolicyLink As String = String.Empty

        Select Case Session("language")
            Case CultureLanguage.English
                udcGeneralF.getSystemParameter("DisclaimerLink", strDisclaimerPolicyLink, String.Empty)
            Case CultureLanguage.TradChinese
                udcGeneralF.getSystemParameter("DisclaimerLink_CHI", strDisclaimerPolicyLink, String.Empty)
            Case CultureLanguage.SimpChinese
                udcGeneralF.getSystemParameter("DisclaimerLink_CN", strDisclaimerPolicyLink, String.Empty)
            Case Else
                Throw New Exception(String.Format("Unexpected value (Session(language)={0})", Session("language")))
        End Select

        Me.lnkBtnDisclaimer.OnClientClick = "javascript:openNewHTML('" + strDisclaimerPolicyLink + "');return false;"

        Dim strSysMaintLink As String = String.Empty

        Select Case Session("language")
            Case CultureLanguage.English
                udcGeneralF.getSystemParameter("SysMaintLink", strSysMaintLink, String.Empty)
            Case CultureLanguage.TradChinese
                udcGeneralF.getSystemParameter("SysMaintLink_CHI", strSysMaintLink, String.Empty)
            Case CultureLanguage.SimpChinese
                udcGeneralF.getSystemParameter("SysMaintLink_CN", strSysMaintLink, String.Empty)
            Case Else
                Throw New Exception(String.Format("Unexpected value (Session(language)={0})", Session("language")))
        End Select

        Me.lnkBtnSysMaint.OnClientClick = "javascript:openNewHTML('" + strSysMaintLink + "');return false;"

        'For PPIEPR SSO ------------------------------------------------------------------------------------

        Dim strEnableSingleLogon As String = "N"
        udcGeneralF.getSystemParameter("EnableSSO_to_PPIePR", strEnableSingleLogon, String.Empty)

        If strEnableSingleLogon.Trim.ToUpper = "Y" Then
            'Enable SSO
            If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                'Common Users
                If (Not IsNothing(Session("SP_CommonUser"))) AndAlso Session("SP_CommonUser") = True Then

                    loadConfig()

                    ibtnPPIePR.Visible = True
                    'Redirection
                    'ibtnPPIePR.Attributes.Add("onclick", "perform_SSO()")
                Else
                    'Non-Common users
                    ibtnPPIePR.Visible = False
                End If
            Else
                'data entry account
                ibtnPPIePR.Visible = False
            End If
        Else
            'Disable SSO
            ibtnPPIePR.Visible = False
        End If

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        SetupClock()
        SetupTimeoutReminder()
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Koala]

    End Sub

    Private Sub gvMenu_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMenu.RowCommand
        If e.CommandName = "Redirect" Then
            Dim udtSessionHandler As New BLL.SessionHandler
            udtSessionHandler.ClearVREClaim()

            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

            RedirectHandler.ToURL(e.CommandArgument)

            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

        End If


    End Sub

    Private Sub gvMenu_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMenu.RowDataBound
        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC()



        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRow = CType(e.Row.DataItem, System.Data.DataRowView).Row

            'CRE13-021 Upgrade server to 2008 [Start][Karl]
            'Dim lnkbtnMenuItem As LinkButton = e.Row.FindControl("lnkbtnMenuItem")
            'Dim lnkbtnMenuChiItem As LinkButton = e.Row.FindControl("lnkbtnMenuChiItem")
            Dim lnkbtnMenuItemReal As LinkButton = e.Row.FindControl("lnkbtnMenuItemReal")
            Dim lnkbtnMenuChiItemReal As LinkButton = e.Row.FindControl("lnkbtnMenuChiItemReal")
            Dim lblbtnMenuItem As Label = e.Row.FindControl("lblbtnMenuItem")
            Dim lblbtnMenuChiItem As Label = e.Row.FindControl("lblbtnMenuChiItem")

            'CRE13-021 Upgrade server to 2008 [End][Karl]

            Dim strMenuRole As String = CStr(dr.Item("Role"))
            Dim dtmEffectiveDate As DateTime = CType(dr.Item("Effective_Date"), DateTime)

            Dim ibtnMenuItem As ImageButton = e.Row.FindControl("ibtnMenuItem")

            Dim strMenuPage As String = String.Empty
            Dim strCurrentPage As String = String.Empty

            'CRE13-021 Upgrade server to 2008 [Start][Karl]
            'strMenuPage = lnkbtnMenuItem.CommandArgument.Replace("~", String.Empty).ToLower
            strMenuPage = lnkbtnMenuItemReal.CommandArgument.Replace("~", String.Empty).ToLower
            'CRE13-021 Upgrade server to 2008 [End][Karl]
            strCurrentPage = Request.ServerVariables("URL").ToLower
            ibtnMenuItem.OnClientClick = "return false;"

            If Not strMenuPage.Equals(String.Empty) And Not strCurrentPage.Equals(String.Empty) Then
                If Not dr.Item("PopUp") Is DBNull.Value AndAlso dr.Item("PopUp") = "N" Then
                    If strMenuPage.IndexOf("/") = 0 Then
                        strMenuPage = strMenuPage.Substring(strMenuPage.LastIndexOf("/"))
                    End If
                Else
                    'CRE13-021 Upgrade server to 2008 [Start][Karl]
                    'lnkbtnMenuItem.OnClientClick = "javascript:openNewWin('" & lnkbtnMenuItem.CommandArgument & "'); return false;"
                    'lnkbtnMenuChiItem.OnClientClick = "javascript:openNewWin('" & lnkbtnMenuChiItem.CommandArgument & "'); return false;"
                    lnkbtnMenuItemReal.OnClientClick = "javascript:openNewWin('" & lnkbtnMenuItemReal.CommandArgument & "'); return false;"
                    lnkbtnMenuChiItemReal.OnClientClick = "javascript:openNewWin('" & lnkbtnMenuChiItemReal.CommandArgument & "'); return false;"
                    'CRE13-021 Upgrade server to 2008 [End][Karl]
                End If
                strCurrentPage = strCurrentPage.Substring(strCurrentPage.LastIndexOf("/"))
            End If

            If strMenuPage = strCurrentPage And Not strMenuPage.Equals(String.Empty) Then
                e.Row.CssClass = "menuSelect"
                'CRE13-021 Upgrade server to 2008 [Start][Karl]
                'lnkbtnMenuItem.CssClass = "menuSelect"
                'lnkbtnMenuChiItem.CssClass = "menuSelect"
                lblbtnMenuItem.CssClass = "menuSelect"
                lblbtnMenuChiItem.CssClass = "menuSelect"
                'CRE13-021 Upgrade server to 2008 [End][Karl]                
            Else
                e.Row.CssClass = "menuUnSelect"
                'CRE13-021 Upgrade server to 2008 [Start][Karl]    
                lblbtnMenuItem.CssClass = "menuUnSelect"
                lblbtnMenuChiItem.CssClass = "menuUnSelect"
                'lnkbtnMenuItem.CssClass = "menuUnSelect"
                'lnkbtnMenuChiItem.CssClass = "menuUnSelect"
                'CRE13-021 Upgrade server to 2008 [End][Karl]
            End If

            Dim strMenuItemClientID As String = String.Empty

            'CRE13-021 Upgrade server to 2008 [Start][Karl]
            Dim ctrlMenuItemClientIDReal As Web.UI.Control
            Dim strURL As String = String.Empty
            'CRE13-021 Upgrade server to 2008 [End][Karl]

            If Session("language") = "zh-tw" Then
                'CRE13-021 Upgrade server to 2008 [Start][Karl]
                'lnkbtnMenuItem.Visible = False
                'lnkbtnMenuChiItem.Visible = True
                'strMenuItemClientID = lnkbtnMenuChiItem.ClientID
                lblbtnMenuItem.Visible = False
                lblbtnMenuChiItem.Visible = True
                'CRE15-006 (Rename of eHS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                lnkbtnMenuItemReal.Visible = False
                lnkbtnMenuChiItemReal.Visible = True
                'CRE15-006 (Rename of eHS) [End][Chris YIM]
                strMenuItemClientID = lblbtnMenuChiItem.ClientID
                ctrlMenuItemClientIDReal = lnkbtnMenuChiItemReal
                strURL = lnkbtnMenuChiItemReal.CommandArgument
                'CRE13-021 Upgrade server to 2008 [End][Karl]

            ElseIf Session("language") = "zh-cn" Then
                lblbtnMenuItem.Visible = False
                lblbtnMenuChiItem.Visible = True
                'CRE15-006 (Rename of eHS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                lnkbtnMenuItemReal.Visible = False
                lnkbtnMenuChiItemReal.Visible = True
                'CRE15-006 (Rename of eHS) [End][Chris YIM]
                strMenuItemClientID = lblbtnMenuChiItem.ClientID
                ctrlMenuItemClientIDReal = lnkbtnMenuChiItemReal
                strURL = lnkbtnMenuChiItemReal.CommandArgument

                lblbtnMenuChiItem.Text = dr("Menu_Name_CN")

            Else
                'CRE13-021 Upgrade server to 2008 [Start][Karl]
                'lnkbtnMenuItem.Visible = True
                'lnkbtnMenuChiItem.Visible = False
                'strMenuItemClientID = lnkbtnMenuItem.ClientID
                lblbtnMenuItem.Visible = True
                lblbtnMenuChiItem.Visible = False
                'CRE15-006 (Rename of eHS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                lnkbtnMenuItemReal.Visible = True
                lnkbtnMenuChiItemReal.Visible = False
                'CRE15-006 (Rename of eHS) [End][Chris YIM]
                strMenuItemClientID = lblbtnMenuItem.ClientID
                ctrlMenuItemClientIDReal = lnkbtnMenuItemReal
                strURL = lnkbtnMenuItemReal.CommandArgument
                'CRE13-021 Upgrade server to 2008 [End][Karl]
            End If

            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

            If RedirectHandler.IsTurnOnConcurrentBrowserHandling Then
                ibtnMenuItem.PostBackUrl = ibtnMenuItem.CommandArgument
            End If

            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

            If strMenuPage <> strCurrentPage Then
                e.Row.Attributes.Add("onmouseover", "this.className = 'menuSelect';document.getElementById('" + strMenuItemClientID + "').className = 'menuSelect';")
                e.Row.Attributes.Add("onmouseout", "this.className = 'menuUnSelect';document.getElementById('" + strMenuItemClientID + "').className = 'menuUnSelect';")
            End If

            'CRE13-021 Upgrade server to 2008 [Start][Karl]
            'e.Row.Attributes.Add("onclick", "document.getElementById('" + strMenuItemClientIDReal + "').click();")
            If Not strMenuPage.Equals(String.Empty) And Not strCurrentPage.Equals(String.Empty) Then
                If Not dr.Item("PopUp") Is DBNull.Value AndAlso dr.Item("PopUp") = "N" Then
                    e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(ctrlMenuItemClientIDReal, String.Empty))
                Else
                    e.Row.Attributes.Add("onclick", "javascript:openNewWin('" & strURL & "'); return false;")
                End If
            End If
            'CRE13-021 Upgrade server to 2008 [End][Karl]

            If Not strMenuRole.Equals("A") Then
                If strMenuRole = udtUserAC.UserType Then
                    e.Row.Visible = True
                Else
                    e.Row.Visible = False
                End If
            End If

            ' Filter the menu item by the SP enrolled practice scheme
            If e.Row.Visible Then
                Dim udtSP As ServiceProviderModel = Nothing
                Dim aryDataEntryPracticeList As ArrayList = Nothing

                If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                    udtSP = udtUserAC
                Else
                    Dim udtDataEntry As DataEntryUserModel = udtUserAC
                    udtSP = udtDataEntry.ServiceProvider
                    aryDataEntryPracticeList = udtDataEntry.PracticeList
                End If

                ' Check the Menu Visible if entitle the premitted scheme
                Dim strMenuSchemeList As String = CStr(dr("Scheme_Code")).Trim.ToUpper
                e.Row.Visible = CheckMenuVisible(strMenuSchemeList, udtSP, aryDataEntryPracticeList)

            End If
        End If
    End Sub

    'CRE13-021 Upgrade server to 2008 [End][Karl]
    Private Sub BuildMenu()
        Dim udtMenuBLL As BLL.MenuBLL = New BLL.MenuBLL
        Dim dt As New DataTable
        dt = udtMenuBLL.GetMenuItem(HCSPMenuType.Menu, DirectCast(Me.Page, BasePage).SubPlatform)

        Dim strFAQURL As String = String.Empty
        Dim strContactUsURL As String = String.Empty
        Dim strHost As String = Request.Url.ToString.Substring(0, Request.Url.ToString.IndexOf(Request.Path))
        Dim strURL As String = String.Empty

        For Each dr As DataRow In dt.Rows
            strURL = dr.Item("URL").ToString
            If strURL.Contains("|||") Then
                Dim aryURL As String() = strURL.Split(New String() {"|||"}, StringSplitOptions.None)

                Select Case Session("language")
                    Case CultureLanguage.English
                        dr.Item("URL") = aryURL(0)
                    Case CultureLanguage.TradChinese
                        If aryURL.Length > 1 Then
                            dr.Item("URL") = aryURL(1)
                        Else
                            ' Default to English if not set
                            dr.Item("URL") = aryURL(0)
                        End If
                    Case CultureLanguage.SimpChinese
                        If aryURL.Length > 2 Then
                            dr.Item("URL") = aryURL(2)
                        Else
                            ' Default to English if not set
                            dr.Item("URL") = aryURL(0)
                        End If
                End Select

                Dim strURLLink As String = strHost '& Request.ApplicationPath
                If strURLLink.Substring(strURLLink.Length - 1, 1) <> "/" Then
                    strURLLink = strURLLink & "/" & dr.Item("URL")
                Else
                    strURLLink = strURLLink & dr.Item("URL")
                End If
                dr.Item("URL") = strURLLink

            End If
        Next
        gvMenu.DataSource = dt
        gvMenu.DataBind()
    End Sub

    'Show which langage is selected
    Private Sub setLangageStyle()
        Dim selectedValue As String

        selectedValue = Session("language")
        Select Case selectedValue
            Case CultureLanguage.English
                lnkbtnEnglish.CssClass = "languageSelectedText"
                lnkbtnTradChinese.CssClass = "languageText"
                lnkbtnSimpChinese.CssClass = "languageText"
                lnkbtnEnglish.Enabled = False
                lnkbtnTradChinese.Enabled = True
                lnkbtnSimpChinese.Enabled = True
            Case CultureLanguage.TradChinese
                lnkbtnEnglish.CssClass = "languageText"
                lnkbtnTradChinese.CssClass = "languageSelectedText"
                lnkbtnSimpChinese.CssClass = "languageText"
                lnkbtnEnglish.Enabled = True
                lnkbtnTradChinese.Enabled = False
                lnkbtnSimpChinese.Enabled = True
            Case CultureLanguage.SimpChinese
                lnkbtnEnglish.CssClass = "languageText"
                lnkbtnTradChinese.CssClass = "languageText"
                lnkbtnSimpChinese.CssClass = "languageSelectedText"
                lnkbtnEnglish.Enabled = True
                lnkbtnTradChinese.Enabled = True
                lnkbtnSimpChinese.Enabled = False
            Case Else
                lnkbtnEnglish.CssClass = "languageSelectedText"
                lnkbtnTradChinese.CssClass = "languageText"
                lnkbtnSimpChinese.CssClass = "languageText"
                lnkbtnEnglish.Enabled = False
                lnkbtnTradChinese.Enabled = True
                lnkbtnSimpChinese.Enabled = True
        End Select
    End Sub

    Private Function CheckMenuVisible(ByVal strMenuSchemeList As String, ByVal udtSP As ServiceProviderModel, ByVal aryDataEntryPracticeList As ArrayList) As Boolean
        If strMenuSchemeList = "ALL" Then Return True

        ' Get all SP Scheme
        Dim udtSchemeList As SchemeInformationModelCollection = udtSP.SchemeInfoList

        ' Get all Practice Scheme
        Dim udtFilterPracticeSchemeList As New PracticeSchemeInfoModelCollection

        For Each udtPractice As PracticeModel In udtSP.PracticeList.Values
            If Not IsNothing(aryDataEntryPracticeList) Then
                If Not aryDataEntryPracticeList.Contains(udtPractice.DisplaySeq) Then Continue For
            End If

            For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                Dim strPracticeSchemeStatus As String = udtPracticeScheme.RecordStatus
                Dim strSchemeStatus As String = udtSchemeList.Filter(udtPracticeScheme.SchemeCode).RecordStatus

                If (strPracticeSchemeStatus = PracticeSchemeInfoMaintenanceDisplayStatus.Active _
                            OrElse strPracticeSchemeStatus = PracticeSchemeInfoMaintenanceDisplayStatus.ActivePendingSuspend _
                            OrElse strPracticeSchemeStatus = PracticeSchemeInfoMaintenanceDisplayStatus.ActivePendingDelist) _
                        AndAlso (strSchemeStatus = SchemeInformationMaintenanceDisplayStatus.Active _
                            OrElse strSchemeStatus = SchemeInformationMaintenanceDisplayStatus.ActivePendingSuspend _
                            OrElse strSchemeStatus = SchemeInformationMaintenanceDisplayStatus.ActivePendingDelist) Then
                    udtFilterPracticeSchemeList.Add(udtPracticeScheme)
                End If

            Next

        Next

        Dim udtSchemeClaimBLL As New SchemeClaimBLL
        Dim udtSchemeClaimList As SchemeClaimModelCollection = udtSchemeClaimBLL.ConvertSchemeClaimCodeFromSchemeEnrol(udtFilterPracticeSchemeList)

        For Each udtSchemeClaim As SchemeClaimModel In udtSchemeClaimList
            For Each strMenuScheme As String In strMenuSchemeList.Split(",")
                If strMenuScheme.Trim = udtSchemeClaim.SchemeCode Then
                    Return True
                End If
            Next
        Next

        Return False

    End Function

#Region "Script Function"

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
        'strPleaseWaitScript.Append("$('#" + Me.pnlPleaseWait.ClientID + "').show();")

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

    Private Function GetTimeoutWarningScript() As String

        Dim intTimeoutMillSec As Integer = 18 * 60 * 1000
        Dim strScript As String = String.Empty

        Dim formater As New Common.Format.Formatter()
        strScript = strScript + "function ShowTimeoutWarning(){"
        strScript = strScript + "window.focus();"
        strScript = strScript + "alert('Session will be expired soon! [" + DateTime.Now.AddMilliseconds(intTimeoutMillSec).ToString(formater.DisplayDateTimeFormat) + "] Popup set: " + (intTimeoutMillSec / 1000).ToString() + " Seconds');"
        strScript = strScript + "}"
        strScript = strScript + "var timeoutID;"
        strScript = strScript + "if (timeoutID!=null){"
        strScript = strScript + "clearTimeout(timeoutID);"
        strScript = strScript + "}"
        strScript = strScript + "timeoutID = setTimeout('ShowTimeoutWarning()', '" + intTimeoutMillSec.ToString() + "');"
        Return strScript.Trim()

    End Function
#End Region

#Region "Click Event"

    Protected Sub ibtnMenu_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE13-026 - Repository [Start][Lawrence]
        Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT029901)
        udtAuditLogEntry.AddDescripton("Current function code", DirectCast(Me.Page, BasePage).FunctionCode)
        udtAuditLogEntry.AddDescripton("Old Status", IIf(panMenu.Visible, "Expand", "Collapse"))
        udtAuditLogEntry.AddDescripton("New Status", IIf(panMenu.Visible, "Collapse", "Expand"))
        udtAuditLogEntry.WriteLog(LogID.LOG00010, "Menu Expand/Collapse click")
        ' CRE13-026 - Repository [End][Lawrence]

        panMenu.Visible = Not panMenu.Visible
    End Sub

    Protected Sub btnInbox_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnInbox.Click
        ' CRE13-026 - Repository [Start][Lawrence]
        Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT029901)
        udtAuditLogEntry.AddDescripton("Current function code", DirectCast(Me.Page, BasePage).FunctionCode)
        udtAuditLogEntry.WriteLog(LogID.LOG00002, "Inbox click")
        ' CRE13-026 - Repository [End][Lawrence]

        Session(Me.strLastTimeCheck) = Now
        Session(Me.strLastCheckCount) = 0

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        RedirectHandler.ToURL("~/Home/Inbox.aspx")

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

    End Sub

    Protected Sub ibtnHome_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnHome.Click
        ' CRE13-026 - Repository [Start][Lawrence]
        Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT029901)
        udtAuditLogEntry.AddDescripton("Current function code", DirectCast(Me.Page, BasePage).FunctionCode)
        udtAuditLogEntry.WriteLog(LogID.LOG00001, "Home click")
        ' CRE13-026 - Repository [End][Lawrence]

        'Session.Abandon()

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        RedirectHandler.ToURL("~/Home/home.aspx")

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

    End Sub

    Protected Sub ibtnLogout_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnLogout.Click
        ' CRE13-026 - Repository [Start][Lawrence]
        Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT029901)
        udtAuditLogEntry.AddDescripton("Current function code", DirectCast(Me.Page, BasePage).FunctionCode)
        udtAuditLogEntry.WriteLog(LogID.LOG00003, "Logout click")
        ' CRE13-026 - Repository [End][Lawrence]

        Logout()
        Response.Redirect("~/login.aspx")
    End Sub

    Public Sub Logout()

        Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT020001)
        Dim udtUserAC As UserACModel
        Dim strLogID As String
        Dim strAuditLogDesc As String
        udtUserAC = UserACBLL.GetUserAC
        Dim udtSP As ServiceProviderModel
        'Me.lblWelcome.Text = Me.GetGlobalResourceObject("Text", "Username")
        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            'Me.ibtnChangeSP.Visible = False
            udtSP = CType(udtUserAC, ServiceProviderModel)
            'Me.lblLoginName.Text = udtSP.EnglishName
            udtAuditLogEntry.AddDescripton("SP_ID", udtSP.SPID)
            strLogID = LogID.LOG00010
            strAuditLogDesc = "Service Provider "
        Else
            Dim udtDataEntryUser As DataEntryUserModel
            udtDataEntryUser = CType(udtUserAC, DataEntryUserModel)
            udtAuditLogEntry.AddDescripton("SPID", udtDataEntryUser.SPID)
            udtAuditLogEntry.AddDescripton("Data_Entry_Account", udtDataEntryUser.DataEntryAccount)
            strLogID = LogID.LOG00011
            strAuditLogDesc = "Data Entry Account "
        End If

        udtAuditLogEntry.WriteLog(strLogID, strAuditLogDesc & "Logout")

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

    End Sub

    Protected Sub ibtnChangeSP_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnChangeSP.Click
        ' CRE13-026 - Repository [Start][Lawrence]
        Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT029901)
        udtAuditLogEntry.AddDescripton("Current function code", DirectCast(Me.Page, BasePage).FunctionCode)
        udtAuditLogEntry.WriteLog(LogID.LOG00004, "Change Service Provider click")
        ' CRE13-026 - Repository [End][Lawrence]

        Dim strDataEntryAcct As String = ""

        Dim udtUserAC As UserACModel
        udtUserAC = UserACBLL.GetUserAC

        If udtUserAC.UserType = SPAcctType.DataEntryAcct Then
            Dim udtDataEntryUser As DataEntryUserModel
            udtDataEntryUser = CType(udtUserAC, DataEntryUserModel)
            strDataEntryAcct = udtDataEntryUser.DataEntryAccount
        End If

        ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
        ' ----------------------------------------------------------
        HandleSessionVariable()
        ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

        Session(SESS_DataEntryAccount) = strDataEntryAcct

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

        Response.Redirect("~/login.aspx")
    End Sub

    'For PPIEPR SSO
    Protected Sub ibtnPPIePR_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnPPIePR.Click
        Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT021101)
        Dim strLogID As String
        Dim strAuditLogDesc As String
        Dim udtUserAC As UserACModel

        udtUserAC = UserACBLL.GetUserAC
        Dim udtSP As ServiceProviderModel
        udtSP = CType(udtUserAC, ServiceProviderModel)

        udtAuditLogEntry.AddDescripton("SP_ID", udtSP.SPID)
        strLogID = LogID.LOG00001
        strAuditLogDesc = "Click PPI-ePR Logon button"
        udtAuditLogEntry.WriteLog(strLogID, strAuditLogDesc)

        ModalPopupExtenderSSOConfirm.Show()
    End Sub

    'For PPIEPR SSO
    Protected Sub ibtnSSOConfirmOK_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT021101)
        Dim strLogID As String
        Dim strAuditLogDesc As String
        Dim udtUserAC As UserACModel

        udtUserAC = UserACBLL.GetUserAC
        Dim udtSP As ServiceProviderModel
        udtSP = CType(udtUserAC, ServiceProviderModel)

        'Confirm Redirect

        'Add audit log
        udtAuditLogEntry.AddDescripton("SP_ID", udtSP.SPID)
        strLogID = LogID.LOG00002
        strAuditLogDesc = "SSO (PPI-ePR) starts"
        udtAuditLogEntry.WriteLog(strLogID, strAuditLogDesc)

        System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_Language") = Session("language")

        'Enhancement-----------------------------------------------------------------------------------------------------------
        Dim strSSOErrCode As String = String.Empty
        Dim strLogDescription As String = String.Empty
        Dim blnWSfail As Boolean = True
        Dim arrSSORedirect As String() = Nothing

        '----------------------------------------
        ' Retrieve Authentication Ticket from DB 
        '----------------------------------------
        Dim objSSOAuthen As SSOAuthen = Nothing
        Dim objSSOAuthenApp As SSOAuthenApp = Nothing

        If Not Session("SSO_SSOAuthen") Is Nothing Then
            objSSOAuthen = CType(Session("SSO_SSOAuthen"), SSOAuthen)
        Else
            strSSOErrCode = "SSO_NULL_SESSION_VARIABLE_SSOAUTHEN"
        End If

        If strSSOErrCode = "" Then
            objSSOAuthenApp = SSOAuthenticationDAL.getInstance().getSSOAuthenApp(objSSOAuthen.SystemDtm, objSSOAuthen.AuthenTicket)

            If objSSOAuthenApp Is Nothing Then
                strSSOErrCode = "SSO_FAILED_TO_RETRIEVE_SSOAUTHENAPP_FROM_DB"
            End If

            ' Retrieve a redirect ticket generated from relying application
            If strSSOErrCode = "" Then

                Dim arrWsUrl As String() = Nothing
                Dim strWsUrlList As String = String.Empty
                Dim intSSOWSIdPTimeoutInSec As Integer = 0

                Dim objSSOIdPWebServices As SSOLib.SSOIdPWebServices.SSOIdPWebServices = Nothing
                strWsUrlList = SSOUtil.SSOAppConfigMgr.getSSOSPWSUrl(arrRelatedSSOAppIds(0))

                If strWsUrlList.Trim() = "" OrElse strWsUrlList Is Nothing Then
                    strSSOErrCode = "SSO_IDP_WS_URL_NOT_DEFINED"
                End If

                Dim callback As New RemoteCertificateValidationCallback(AddressOf ValidateCertificate)
                System.Net.ServicePointManager.ServerCertificateValidationCallback = callback

                arrWsUrl = strWsUrlList.Split(New Char() {","c})
                '-----------------------------------------------------------------------
                'For Failover, loop through possible web service url until it is successful to cal web service to get redirect ticket 
                '-----------------------------------------------------------------------
                For intCounter As Integer = 0 To arrWsUrl.Length - 1
                    Try
                        objSSOIdPWebServices = New SSOLib.SSOIdPWebServices.SSOIdPWebServices()
                        objSSOIdPWebServices.Url = arrWsUrl(intCounter).Trim()
                        If Int32.TryParse(SSOUtil.SSOAppConfigMgr.getSSOWSSPTimeoutInSec(strLocalSSOAppId), intSSOWSIdPTimeoutInSec) Then
                            objSSOIdPWebServices.Timeout = intSSOWSIdPTimeoutInSec * 1000
                        End If

                        'Write SSO audit log
                        strLogDescription = "SSO call Replying App web service for retrieving redirect ticket (Start)"
                        strLogDescription = strLogDescription + "<SP_ID:" + udtSP.SPID + ">"
                        SSOHelper.WriteSSOAuditLogToDB(LogID.LOG00025, LogType.Information, strLogDescription)

                        '----------------------------------------------------
                        'Call replying app's web service to retrieve redirect ticket
                        '----------------------------------------------------
                        'Secured Auth Ticket + Relay App ID (eHS)
                        arrSSORedirect = objSSOIdPWebServices.getSSORedirect(objSSOAuthenApp.RelySignedAuthenTicket, strLocalSSOAppId)

                        'Write SSO audit log
                        strLogDescription = "SSO call Replying App web service for retrieving redirect ticket (Complete)"
                        strLogDescription = strLogDescription + "<RedirectResult:" + IIf(IsNothing(arrSSORedirect(0)), "", arrSSORedirect(0)) + ">"
                        strLogDescription = strLogDescription + "<RedirectTicket:" + IIf(IsNothing(arrSSORedirect(1)), "", arrSSORedirect(1)) + ">"
                        strLogDescription = strLogDescription + "<ErrorCode:" + IIf(IsNothing(arrSSORedirect(2)), "", arrSSORedirect(2)) + ">"
                        SSOHelper.WriteSSOAuditLogToDB(LogID.LOG00026, LogType.Information, strLogDescription)

                        blnWSfail = False

                        'leave the loop if web service call is completed succesfully
                        Exit For

                    Catch ex As Exception

                        blnWSfail = True
                        strLogDescription = "SSO call Replying App web service for retrieving redirect ticket (Exception)"
                        strLogDescription = strLogDescription + "<Exception:" + ex.Message + ">"
                        strLogDescription = strLogDescription + "<SP_ID:" + udtSP.SPID + ">"
                        SSOHelper.WriteSSOAuditLogToDB(LogID.LOG00027, LogType.SysException, strLogDescription)
                    End Try
                Next
            End If
        End If

        If blnWSfail = True Then
            'Return to Home Page
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)

            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

            RedirectHandler.ToURL("~/Home/home.aspx")

            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End
        End If

        If strSSOErrCode <> "" Then
            strLogDescription = "Fail to redirect to replying app  (Invalid Internal settings / error)"
            strLogDescription = strLogDescription + "<ErrorCode:" + strSSOErrCode + ">"
            strLogDescription = strLogDescription + "<SP_ID:" + udtSP.SPID + ">"
            SSOHelper.WriteSSOAuditLogToDB(LogID.LOG00028, LogType.SysFail, strLogDescription)

            'Return to Home Page 
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)

            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

            RedirectHandler.ToURL("~/Home/home.aspx")

            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End
        End If

        '-------------------------------------------------
        ' 4 --> if success:     Cotinue SSO process
        '   --> if fail:        Return User to Home Page (with error message)
        '-------------------------------------------------
        If Not blnWSfail Then
            If Not IsNothing(arrSSORedirect) AndAlso Not IsNothing(arrSSORedirect(0)) AndAlso arrSSORedirect(0) = "S" AndAlso Not IsNothing(arrSSORedirect(1)) Then

                System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_RedirectTicket") = arrSSORedirect(1)

                '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

                'Redirect --> Continue SSO

                'Since logout EHS, no need to append PageKey for Concurrent Browser Handling
                Response.Redirect(strRedirectorlink)

                '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

            Else
                'Write log
                If Not IsNothing(arrSSORedirect) AndAlso Not IsNothing(arrSSORedirect(0)) AndAlso arrSSORedirect(0) = "F" Then

                    strSSOErrCode = arrSSORedirect(2)
                    If IsNothing(strSSOErrCode) Then
                        strSSOErrCode = ""
                    End If

                    strLogDescription = "Fail to redirect to replying app (Rejected when retrieving redirect ticket)"
                    strLogDescription = strLogDescription + "<SP_ID:" + udtSP.SPID + ">"
                    strLogDescription = strLogDescription + "<ErrorCode:" + strSSOErrCode + ">"
                    SSOHelper.WriteSSOAuditLogToDB(LogID.LOG00029, LogType.Information, strLogDescription)
                Else
                    strLogDescription = "Fail to redirect to replying app (Invalid return when retrieving redirect ticket)"
                    strLogDescription = strLogDescription + "<SP_ID:" + udtSP.SPID + ">"
                    SSOHelper.WriteSSOAuditLogToDB(LogID.LOG00030, LogType.SysFail, strLogDescription)
                End If

                'Return to Home Page 
                SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)

                '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

                RedirectHandler.ToURL("~/Home/home.aspx")

                '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

            End If
        End If


        '-----------------------------------------------------------------------------------------------------------

    End Sub

    'For PPIEPR SSO
    Protected Sub ibtnSSOConfirmCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT021101)
        Dim strLogID As String
        Dim strAuditLogDesc As String
        Dim udtUserAC As UserACModel

        udtUserAC = UserACBLL.GetUserAC
        Dim udtSP As ServiceProviderModel
        udtSP = CType(udtUserAC, ServiceProviderModel)

        udtAuditLogEntry.AddDescripton("SP_ID", udtSP.SPID)
        strLogID = LogID.LOG00003
        strAuditLogDesc = "Click 'Cancel' to abort Single Logon (PPI-ePR)"
        udtAuditLogEntry.WriteLog(strLogID, strAuditLogDesc)

        ModalPopupExtenderSSOConfirm.Hide()
    End Sub
#End Region


    Private Function ValidateCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
        'Return True to force the certificate to be accepted.
        Return True
    End Function

    Private Sub preventMultiImgClick(ByVal cs As ClientScriptManager, ByVal ibtn As ImageButton)
        Dim strScript As String = "if (this.style.cursor != 'wait') { this.style.cursor = 'wait'; return true; } else { this.disabled = true; return false; }"

        ibtn.Attributes.Add("onclick", strScript)

    End Sub

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

        Dim udtAuditLogEntry As AuditLogEntry = Nothing
        If TypeOf Me.Parent.Page Is Common.ComInterface.IWorkingData Then
            udtAuditLogEntry = New AuditLogEntry(Me._FunctCodeCommon, Me.Parent.Page)
        Else
            udtAuditLogEntry = New AuditLogEntry(Me._FunctCodeCommon)
        End If

        udtAuditLogEntry.AddDescripton("PageKey", Me.PageKey.Text)
        udtAuditLogEntry.WriteLog(LogID.LOG00001, "Redirect to invalid access error page")

        Dim strlink As String
        If Session("language") = "zh-tw" Then
            strlink = "~/zh/ImproperAccess.aspx"
        Else
            strlink = "~/en/ImproperAccess.aspx"
        End If
        Response.Redirect(strlink)
    End Sub

    '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

    ' CRE11-021 log the missed essential information [Start]
    ' -----------------------------------------------------------------------------------------
    Private Sub Language_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkbtnEnglish.Click, lnkbtnTradChinese.Click
        Dim udtAuditLogEntry As AuditLogEntry = Nothing
        If TypeOf Me.Parent.Page Is Common.ComInterface.IWorkingData Then
            udtAuditLogEntry = New AuditLogEntry(Me._FunctCodeCommon, Me.Parent.Page)
        Else
            udtAuditLogEntry = New AuditLogEntry(Me._FunctCodeCommon)
        End If

        udtAuditLogEntry.AddDescripton("Language", Session("language"))
        udtAuditLogEntry.WriteLog(LogID.LOG00006, "Change Language")
    End Sub

    ' CRE11-021 log the missed essential information [End]

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

    Private Function GetLanguageFolderName() As String
        Dim strLangFolder As String = String.Empty
        If Not HttpContext.Current.Session Is Nothing AndAlso Not HttpContext.Current.Session("language") Is Nothing Then
            If HttpContext.Current.Session("language") = "zh-tw" Then
                strLangFolder = "zh"
            Else
                strLangFolder = "en"
            End If
        ElseIf Request.UserLanguages.Length > 0 Then
            Dim strLanguage As String = Request.UserLanguages(0)
            If strLanguage.IndexOf("zh") = 0 Then
                strLangFolder = "zh"
            Else
                strLangFolder = "en"
            End If
        Else
            strLangFolder = "en"
        End If

        Return strLangFolder
    End Function

    Private Sub SetupTimeoutReminder()
        ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
        ' -------------------------------------------------------------------------------
        'Me.Page.ClientScript.RegisterStartupScript(Me.GetType, "SetupTimeoutReminder", String.Format("javascript: StartTimeoutReminder('{0}','{1}','{2}','{3}','{4}');", _
        '                                        New String() {(Session.Timeout * 60).ToString, _
        '                                                    Me.udcGeneralF.GetTimeoutReminderDisplayTime(), _
        '                                                    GetLanguageFolderName(), _
        '                                                    Me.ModalPopupExtenderTimeoutReminder.BehaviorID, _
        '                                                    Me.ucNoticePopUpTimeoutReminder.MessageLabel.ClientID}), True)

        Me.Page.ClientScript.RegisterStartupScript(Me.GetType, "SetupTimeoutReminder", String.Format("javascript: StartTimeoutReminder('{0}','{1}','{2}','{3}','{4}');", _
                                                New String() {(Session.Timeout * 60).ToString, _
                                                              Me.udcGeneralF.GetTimeoutReminderDisplayTime(), _
                                                              GetLanguageFolderName(), _
                                                              Me.panTimeoutReminder.ClientID, _
                                                              Me.ucNoticePopUpTimeoutReminder.MessageLabel.ClientID}), True)

        Me.ucNoticePopUpTimeoutReminder.ButtonOK.Attributes.Add("onclick", "ReminderOK_Click();return false;")

        'Me.ModalPopupExtenderTimeoutReminder.OkControlID = Me.ucNoticePopUpTimeoutReminder.ButtonOK.ClientID
        'Me.ModalPopupExtenderTimeoutReminder.PopupDragHandleControlID = Me.ucNoticePopUpTimeoutReminder.Header.ClientID
        ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]

        ' INT20-0012 (Fix double postback on claim) [Start][Koala]
        ' ----------------------------------------------------------
        ucNoticePopUpTimeoutReminder.MessageText = HttpContext.GetGlobalResourceObject("Text", "TimeoutReminderMessage")
        ' INT20-0012 (Fix double postback on claim) [End][Koala]
    End Sub
    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Koala]

    ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Private Sub HandleSessionVariable()
        Dim Cache1 As String = Nothing
        Dim Cache2 As Boolean = Nothing
        Dim Cache3 As Boolean = Nothing
        Dim Cache4 As Boolean = False

        '1. language
        If Not Session("language") Is Nothing Then
            Cache1 = Session("language")
        End If

        '2. Undefined User Agent
        Cache2 = CommonSessionHandler.AddedUndefinedUserAgent

        '3. Popup for remind obsoleted OS
        Cache3 = CommonSessionHandler.ReminderForWindowsVersion

        '4. Popup for enable to allow popup
        Cache4 = udcSessionHandler.PopupBlockerGetFromSession()

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

        '4. Popup for enable to allow popup
        udcSessionHandler.PopupBlockerSaveToSession(Cache4)

    End Sub
    ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

End Class