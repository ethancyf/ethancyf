Partial Public Class thankyou
    Inherits BasePage

    Private udcGeneralF As New Common.ComFunction.GeneralFunction
    Private strLanguage As String = String.Empty

    Private Sub login_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim udcGeneralFun = New Common.ComFunction.GeneralFunction()
        Me.basetag.Attributes("href") = udcGeneralFun.getPageBasePath()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
  
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        SetLangage()

        tblBanner.Style.Item("background-image") = "url(" + Me.GetGlobalResourceObject("ImageUrl", "IndexBanner") + ")"
        Me.PageTitle.InnerText = Me.GetGlobalResourceObject("Title", "VoucherBalanceEnquiry")

        Dim selectedLang As String
        selectedLang = LCase(Session("language"))

        Dim strPrivacyPolicyLink As String = String.Empty

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

        Dim strSysMaintLink As String = String.Empty

        If selectedLang.Equals(English) Then
            udcGeneralF.getSystemParameter("SysMaintLink", strSysMaintLink, String.Empty)
        ElseIf selectedLang.Equals(TradChinese) Then
            udcGeneralF.getSystemParameter("SysMaintLink_CHI", strSysMaintLink, String.Empty)
        End If
        Me.lnkBtnSysMaint.OnClientClick = "javascript:openNewWin('" + strSysMaintLink + "');return false;"

        strLanguage = Session("language")
        Session.RemoveAll()
        Session("language") = strLanguage
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

    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Return Nothing
    End Function

    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        Return Nothing
    End Function

    Public Overrides Function GetDocCode() As String
        Return Nothing
    End Function
   
    Public Overrides Sub ClearWorkingData()
        MyBase.ClearWorkingData()
    End Sub
End Class