Imports Common.ComFunction
Imports Common.Validation
Imports Common.ComObject
Imports System.Globalization
Imports System.Threading
Imports Common.Component.VoucherRecipientAccount
Imports Common.DataAccess
'Imports VoucherBalanceEnquiry.Controller
Imports Common.Format
Imports Common.Component.VoucherScheme
Imports Common.Component

Partial Public Class main
    Inherits BasePage

    Private udcGeneralF As New Common.ComFunction.GeneralFunction
    Private _strValidationFail As String = "ValidationFail"
    Private _udtSessionHandler As BLL.SessionHandler = New BLL.SessionHandler

    Private Sub login_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        Dim udcGeneralFun = New Common.ComFunction.GeneralFunction()
        Me.basetag.Attributes("href") = udcGeneralFun.getPageBasePath()

        Dim strSelectedLang As String
        'strSelectedLang = IIf(IsNothing(Request.QueryString("lang")), "", Request.QueryString("lang")).ToString().Trim.ToUpper
        'If strSelectedLang = "ZH" Then
        '    Session("language") = TradChinese
        'ElseIf strSelectedLang = "EN" Then
        '    Session("language") = English
        'End If
        strSelectedLang = LCase(Session("language"))
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            FunctionCode = Common.Component.FunctCode.FUNT050101

            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, "Home Page Load")

            Session("PreFillMain") = Nothing
            _udtSessionHandler.PreFillChineseRemoveFromSession(FunctionCode)
            _udtSessionHandler.PreFillPressConfirmRemoveFromSession(FunctionCode)
            _udtSessionHandler.PreFillConsentIDRemoveFromSession(FunctionCode)
            _udtSessionHandler.PreFillSubmitTimeRemoveFromSession(FunctionCode)
            _udtSessionHandler.EHSAccountRemoveFromSession(FunctionCode)

            ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [Start][Winnie] Step 0
            _udtSessionHandler.CCCodeRemoveFromSession(FunctionCode)
            ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [End][Winnie] Step 0

            'CRE13-032 End of Support of XP and IE6 [Start][Karl]
            'ibtnClose.Attributes.Add("OnClick", "javascript:window.opener='X';window.open('','_parent','');window.close(); return false;")
            'CRE13-032 End of Support of XP and IE6 [End][Karl]
        End If

        txtProceedImageUrl.Text = Me.GetGlobalResourceObject("ImageUrl", "ProceedBtn")
        txtProceedDisableImageUrl.Text = Me.GetGlobalResourceObject("ImageUrl", "ProceedDisableBtn")

        SetLangage()

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

        tblBanner.Style.Item("background-image") = "url(" + Me.GetGlobalResourceObject("ImageUrl", "IndexBanner") + ")"
        Me.PageTitle.InnerText = Me.GetGlobalResourceObject("Title", "PreFillConsent")

        Dim selectedLang As String
        selectedLang = LCase(Session("language"))

        'Dim strImportantDoc As String = "importantDoc.aspx"

        'Me.ibtnInfo.OnClientClick = "javascript:openNewWin('" + strImportantDoc + "');return false;"

        'lblDownload.Text = GetGlobalResourceObject("Text", "Acrobat") & ", " & GetGlobalResourceObject("Text", "EHealthSCS") & ", " & GetGlobalResourceObject("Text", "HKSCS")
        lblDownload.Text = GetGlobalResourceObject("Text", "PreFillSoftware") & " " & GetGlobalResourceObject("Text", "Acrobat") & ", " & GetGlobalResourceObject("Text", "PreFillSCS")
        'Me.ibtnFloopy.OnClientClick = "javascript:openNewWin('DownloadArea/EN/downloadArea.htm');return false;"
        Me.ibtnFloopy.OnClientClick = "javascript:openNewWin('" & GetGlobalResourceObject("Text", "PreFillDownload") & "');return false;"

        Dim strPrivacyPolicyLink As String = String.Empty

        Me.chkAgreeDisclaimer.Checked = False

        If selectedLang.Equals(English) Then
            udcGeneralF.getSystemParameter("PrivacyPolicyLink", strPrivacyPolicyLink, String.Empty)
        ElseIf selectedLang.Equals(TradChinese) Then
            udcGeneralF.getSystemParameter("PrivacyPolicyLink_CHI", strPrivacyPolicyLink, String.Empty)
        End If

        Me.lnkBtnPrivacyPolicy.OnClientClick = "javascript:openNewWin('" + strPrivacyPolicyLink + "');return false;"

        Dim strDisclaimerLink As String = String.Empty

        If selectedLang.Equals(English) Then
            udcGeneralF.getSystemParameter("DisclaimerLink", strDisclaimerLink, String.Empty)
        ElseIf selectedLang.Equals(TradChinese) Then
            udcGeneralF.getSystemParameter("DisclaimerLink_CHI", strDisclaimerLink, String.Empty)
        End If

        Me.lnkBtnDisclaimer.OnClientClick = "javascript:openNewWin('" + strDisclaimerLink + "');return false;"

        Dim strFAQsLink As String = String.Empty

        If selectedLang.Equals(English) Then
            udcGeneralF.getSystemParameter("FAQsLink", strFAQsLink, String.Empty)
        ElseIf selectedLang.Equals(TradChinese) Then
            udcGeneralF.getSystemParameter("FAQsLink_CHI", strFAQsLink, String.Empty)
        End If

        Me.ibtnFAQ.OnClientClick = "javascript:openNewWin('" + strFAQsLink + "?view=Public');return false;"

        Dim strContactUsLink As String = String.Empty

        If selectedLang.Equals(English) Then
            udcGeneralF.getSystemParameter("ContactUsVOLink", strContactUsLink, String.Empty)
        ElseIf selectedLang.Equals(TradChinese) Then
            udcGeneralF.getSystemParameter("ContactUsVOLink_CHI", strContactUsLink, String.Empty)
        End If

        Me.ibtnContactUs.OnClientClick = "javascript:openNewWin('" + strContactUsLink + "');return false;"

        Dim strSysMaintLink As String = String.Empty

        If selectedLang.Equals(English) Then
            udcGeneralF.getSystemParameter("SysMaintLink", strSysMaintLink, String.Empty)
        ElseIf selectedLang.Equals(TradChinese) Then
            udcGeneralF.getSystemParameter("SysMaintLink_CHI", strSysMaintLink, String.Empty)
        End If
        Me.lnkBtnSysMaint.OnClientClick = "javascript:openNewWin('" + strSysMaintLink + "');return false;"

        ' CRE15-006 Rename eHS - Change Online Demo to Easy Guide [Start][Winnie]
        Dim strEasyGuide As String = Me.GetGlobalResourceObject("Url", "PreFillEasyGuideUrl")
        Me.ibtnEasyGuide.OnClientClick = "javascript:openNewWin('" + strEasyGuide + "');return false;"
        ' CRE15-006 Rename eHS - Change Online Demo to Easy Guide [End][Winnie]

    End Sub

    Private Sub SetLangage()
        Dim selectedValue As String

        selectedValue = Session("language")

        Select Case selectedValue
            Case English
                lnkbtnEnglish.CssClass = "languageSelectedText"
                lnkbtnTradChinese.CssClass = "languageText"
                lnkbtnSimpChinese.CssClass = "languageText"
                lnkbtnEnglish.Enabled = False
                lnkbtnTradChinese.Enabled = True
                lnkbtnSimpChinese.Enabled = True
            Case TradChinese
                lnkbtnEnglish.CssClass = "languageText"
                lnkbtnTradChinese.CssClass = "languageSelectedText"
                lnkbtnSimpChinese.CssClass = "languageText"
                lnkbtnEnglish.Enabled = True
                lnkbtnTradChinese.Enabled = False
                lnkbtnSimpChinese.Enabled = True
            Case SimpChinese
                lnkbtnEnglish.CssClass = "languageText"
                lnkbtnTradChinese.CssClass = "languageText"
                lnkbtnSimpChinese.CssClass = "languageSelectedText"
                lnkbtnEnglish.Enabled = True
                lnkbtnTradChinese.Enabled = True
                lnkbtnSimpChinese.Enabled = False
            Case Else
                lnkbtnEnglish.CssClass = "languageText"
                lnkbtnTradChinese.CssClass = "languageSelectedText"
                lnkbtnSimpChinese.CssClass = "languageText"
                lnkbtnEnglish.Enabled = True
                lnkbtnTradChinese.Enabled = False
                lnkbtnSimpChinese.Enabled = True
        End Select
    End Sub

    Private Sub ReRenderPage()

        lnkBtnPrivacyPolicy.Text = Me.GetGlobalResourceObject("Text", "PrivacyPolicy")
        lnkBtnDisclaimer.Text = Me.GetGlobalResourceObject("Text", "ImportantNotices")
        lnkBtnSysMaint.Text = Me.GetGlobalResourceObject("Text", "SysMaint")

    End Sub

    Private Sub main_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If IsPostBack Then
            Dim controlID As String = Page.Request.Params.Get(PostBackEventTarget)
            If controlID.Equals(SelectTradChinese) OrElse controlID.Equals(SelectEnglish) OrElse _
                controlID.Equals(_SelectTradChinese) OrElse controlID.Equals(_SelectEnglish) Then
                ReRenderPage()
            End If
        End If
    End Sub

    Private Sub ibtnProceed_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnProceed.Click
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        If Me.CaptchaControl.UserValidated Then
            udtAuditLogEntry.WriteLog(LogID.LOG00002, "Go to Pre-Fill Consent Page")
            Session("PreFillMain") = "Pre Fill Main Page"
            Me.Response.Redirect("~/preFill.aspx")
        Else
            Dim errorCode As String
            errorCode = CaptchaControl.ErrorMessage
            Me.imgCaptchaAlert.Visible = True
            Me.udcMessageBox.AddMessage("990000", "E", errorCode)

            'Me.udcMessageBox.BuildMessageBox(_strValidationFail)
            ' CRE11-004
            udtAuditLogEntry.AddDescripton("Message Text", errorCode)
            Me.udcMessageBox.BuildMessageBox(_strValidationFail, udtAuditLogEntry, LogID.LOG00003, "Main Page Validate fail")

        End If
    End Sub

    Private Sub ibtnInfo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnInfo.Click
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00001, "Load Information Page")

        Dim strSelectedLanguage As String
        strSelectedLanguage = LCase(Session("language"))

        popupDocTypeHelp.Show()
        udcDocTypeLegend.BindDocType(strSelectedLanguage)
    End Sub

    Protected Sub ibtnCloseDocTypeHelp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCloseDocTypeHelp.Click
        popupDocTypeHelp.Hide()
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
    End Sub
    'CRE13-032 End of Support of XP and IE6 [Start][Karl]
    Private Sub ibtnExit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnExit.Click
        Me.Response.Redirect("~/thankyou.aspx")
    End Sub
    'CRE13-032 End of Support of XP and IE6 [End][Karl]
End Class