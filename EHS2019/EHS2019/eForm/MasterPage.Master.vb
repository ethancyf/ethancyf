Imports Common.ComFunction
Imports Common.Component

Partial Public Class MasterPage
    Inherits System.Web.UI.MasterPage

    Private Const TradChinese As String = "zh-tw"
    Private Const SimpChinese As String = "zh-cn"
    Private Const English As String = "en-us"

    Private udcGeneralF As New Common.ComFunction.GeneralFunction

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        Me.basetag.Attributes("href") = udcGeneralF.getPageBasePath()
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        setLangageStyle()

        tblBanner.Style.Item("background-image") = "url(" + Me.GetGlobalResourceObject("ImageUrl", "IndexBanner") + ")"
        lnkBtnDocRead.Text = Me.GetGlobalResourceObject("Text", "DocumentToRead")
        lnkBtnFAQs.Text = Me.GetGlobalResourceObject("Text", "Faqs")
        lnkBtnContactUs.Text = Me.GetGlobalResourceObject("Text", "ContactUs")
        lnkBtnPrivacyPolicy.Text = Me.GetGlobalResourceObject("Text", "PrivacyPolicy")
        lnkBtnDisclaimer.Text = Me.GetGlobalResourceObject("Text", "ImportantNotices")
        lnkBtnSysMaint.Text = Me.GetGlobalResourceObject("Text", "SysMaint")

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

        Dim selectedValue As String

        selectedValue = LCase(Session("language"))

        Dim strHost As String = Request.Url.ToString.Substring(0, Request.Url.ToString.IndexOf(Request.Path))
        Dim strLink As String = strHost & Request.ApplicationPath
        If strLink.Substring(strLink.Length - 1, 1) <> "/" Then
            If selectedValue.Equals(English) Then
                strLink = strLink & "/DocToRead/DocToRead.htm"
            ElseIf selectedValue.Equals(TradChinese) Then
                strLink = strLink & "/DocToRead/DocToRead_CHI.htm"
            End If

        Else
            If selectedValue.Equals(English) Then
                strLink = strLink & "DocToRead/DocToRead.htm"
            ElseIf selectedValue.Equals(TradChinese) Then
                strLink = strLink & "DocToRead/DocToRead_CHI.htm"
            End If
        End If

        Me.lnkBtnDocRead.OnClientClick = "javascript:window.openNewWin('" + strLink + "');return false;"

        'Dim strContactUsLink As String = strHost & Request.ApplicationPath
        'If strContactUsLink.Substring(strContactUsLink.Length - 1, 1) <> "/" Then
        '    If selectedValue.Equals(English) Then
        '        strContactUsLink = strContactUsLink & "/ContactUs/contactUs.htm"
        '    ElseIf selectedValue.Equals(TradChinese) Then
        '        strContactUsLink = strContactUsLink & "/ContactUs/contactUs.htm"
        '    End If

        'Else
        '    If selectedValue.Equals(English) Then
        '        strContactUsLink = strContactUsLink & "ContactUs/contactUs.htm"
        '    ElseIf selectedValue.Equals(TradChinese) Then
        '        strContactUsLink = strContactUsLink & "ContactUs/contactUs.htm"
        '    End If
        'End If

        Dim strFAQsLink As String = String.Empty

        If selectedValue.Equals(English) Then
            udcGeneralF.getSystemParameter("FAQsLink_EFORM", strFAQsLink, String.Empty)
        ElseIf selectedValue.Equals(TradChinese) Then
            udcGeneralF.getSystemParameter("FAQsLink_EFORM_CHI", strFAQsLink, String.Empty)
        End If

        Me.lnkBtnFAQs.OnClientClick = "javascript:window.openNewWin('" + strFAQsLink + "');return false;"

        Dim strContactUsLink As String = String.Empty

        If selectedValue.Equals(English) Then
            udcGeneralF.getSystemParameter("ContactUsLink", strContactUsLink, String.Empty)
        ElseIf selectedValue.Equals(TradChinese) Then
            udcGeneralF.getSystemParameter("ContactUsLink_CHI", strContactUsLink, String.Empty)
        End If

        Me.lnkBtnContactUs.OnClientClick = "javascript:window.openNewWin('" + strContactUsLink + "');return false;"

        'Dim strPrivacyPolicyLink As String = strHost & Request.ApplicationPath
        'If strPrivacyPolicyLink.Substring(strPrivacyPolicyLink.Length - 1, 1) <> "/" Then
        '    If selectedValue.Equals(English) Then
        '        strPrivacyPolicyLink = strPrivacyPolicyLink & "/PrivacyPolicy/PrivacyPolicy.htm"
        '    ElseIf selectedValue.Equals(TradChinese) Then
        '        strPrivacyPolicyLink = strPrivacyPolicyLink & "/PrivacyPolicy/PrivacyPolicy.htm#C"
        '    End If
        'Else
        '    If selectedValue.Equals(English) Then
        '        strPrivacyPolicyLink = strPrivacyPolicyLink & "PrivacyPolicy/PrivacyPolicy.htm"
        '    ElseIf selectedValue.Equals(TradChinese) Then
        '        strPrivacyPolicyLink = strPrivacyPolicyLink & "PrivacyPolicy/PrivacyPolicy.htm#C"
        '    End If
        'End If

        Dim strPrivacyPolicyLink As String = String.Empty

        If selectedValue.Equals(English) Then
            udcGeneralF.getSystemParameter("PrivacyPolicyLink", strPrivacyPolicyLink, String.Empty)
        ElseIf selectedValue.Equals(TradChinese) Then
            udcGeneralF.getSystemParameter("PrivacyPolicyLink_CHI", strPrivacyPolicyLink, String.Empty)
        End If
        Me.lnkBtnPrivacyPolicy.OnClientClick = "javascript:openNewWin('" + strPrivacyPolicyLink + "');return false;"

        'Dim strDisclaimerPolicyLink As String = strHost & Request.ApplicationPath
        'If strDisclaimerPolicyLink.Substring(strDisclaimerPolicyLink.Length - 1, 1) <> "/" Then
        '    strDisclaimerPolicyLink = strDisclaimerPolicyLink & "/Disclaimer/Disclaimer.htm"
        'Else
        '    strDisclaimerPolicyLink = strDisclaimerPolicyLink & "Disclaimer/Disclaimer.htm"
        'End If

        Dim strDisclaimerPolicyLink As String = String.Empty

        If selectedValue.Equals(English) Then
            udcGeneralF.getSystemParameter("DisclaimerLink", strDisclaimerPolicyLink, String.Empty)
        ElseIf selectedValue.Equals(TradChinese) Then
            udcGeneralF.getSystemParameter("DisclaimerLink_CHI", strDisclaimerPolicyLink, String.Empty)
        End If
        Me.lnkBtnDisclaimer.OnClientClick = "javascript:openNewWin('" + strDisclaimerPolicyLink + "');return false;"

        Dim strSysMaintLink As String = String.Empty

        If selectedValue.Equals(English) Then
            udcGeneralF.getSystemParameter("SysMaintLink", strSysMaintLink, String.Empty)
        ElseIf selectedValue.Equals(TradChinese) Then
            udcGeneralF.getSystemParameter("SysMaintLink_CHI", strSysMaintLink, String.Empty)
        End If
        Me.lnkBtnSysMaint.OnClientClick = "javascript:openNewWin('" + strSysMaintLink + "');return false;"

        ' Wait Cursor Panel Script
        ScriptManager.RegisterStartupScript(Page, Me.GetType, "ModalUpdProg", Me.GetWaitCursorPanelScript(), True)

    End Sub

    'Show which langage is selected
    Private Sub setLangageStyle()
        Dim selectedValue As String

        selectedValue = Session("language")

        Select Case selectedValue
            Case English
                lnkbtnEnglish.CssClass = "languageSelectedText"
                lnkbtnTradChinese.CssClass = "languageText"
                lnkbtnSimpleChinese.CssClass = "languageText"
                lnkbtnEnglish.Enabled = False
                lnkbtnTradChinese.Enabled = True
                lnkbtnSimpleChinese.Enabled = True
            Case TradChinese
                lnkbtnEnglish.CssClass = "languageText"
                lnkbtnTradChinese.CssClass = "languageSelectedText"
                lnkbtnSimpleChinese.CssClass = "languageText"
                lnkbtnEnglish.Enabled = True
                lnkbtnTradChinese.Enabled = False
                lnkbtnSimpleChinese.Enabled = True
            Case SimpChinese
                lnkbtnEnglish.CssClass = "languageText"
                lnkbtnTradChinese.CssClass = "languageText"
                lnkbtnSimpleChinese.CssClass = "languageSelectedText"
                lnkbtnEnglish.Enabled = True
                lnkbtnTradChinese.Enabled = True
                lnkbtnSimpleChinese.Enabled = False
            Case Else
                lnkbtnEnglish.CssClass = "languageSelectedText"
                lnkbtnTradChinese.CssClass = "languageText"
                lnkbtnSimpleChinese.CssClass = "languageText"
                lnkbtnEnglish.Enabled = False
                lnkbtnTradChinese.Enabled = True
                lnkbtnSimpleChinese.Enabled = True
        End Select
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
        strPleaseWaitScript.Append("document.getElementById('" + Me.pnlPleaseWait.ClientID + "').style.visibility='hidden';")
        strPleaseWaitScript.Append("if (this._pageRequestManager.get_isInAsyncPostBack()) {")
        strPleaseWaitScript.Append("$find('" & Me.ModalPopupExtender1.ClientID & "').show();")
        strPleaseWaitScript.Append("document.getElementById('" & Me.ModalPopupExtender1.ClientID & "_backgroundElement').style.height = document.documentElement.clientHeight + document.documentElement.scrollTop;")
        strPleaseWaitScript.Append("setTimeout(""document.getElementById('" + Me.pnlPleaseWait.ClientID + "').style.visibility='visible'"", 2000);}")
        strPleaseWaitScript.Append("this._timerCookie = null;}")
        strPleaseWaitScript.Append("function ModalUpdProgEndRequest(sender, arg) {")
        strPleaseWaitScript.Append("document.getElementById('" + Me.pnlPleaseWait.ClientID + "').style.visibility='hidden';")
        strPleaseWaitScript.Append(" $find('" & ModalPopupExtender1.ClientID & "').hide();")
        strPleaseWaitScript.Append("if (this._timerCookie) {")
        strPleaseWaitScript.Append("window.clearTimeout(this._timerCookie);")
        strPleaseWaitScript.Append("this._timerCookie = null;}}")
        strPleaseWaitScript.Append("Sys.Application.add_load(ModalUpdProgInitialize);")

        Return strPleaseWaitScript.ToString()

    End Function
End Class