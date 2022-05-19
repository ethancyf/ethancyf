Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.UserAC
Imports System.Web.WebPages

Partial Public Class DownloadArea
    Inherits BasePage

#Region "Properties"

    Protected ReadOnly Property PageLanguage() As String
        Get
            If Me.SubPlatform = EnumHCSPSubPlatform.CN Then
                Return "lang=""zh"""
            Else
                Return String.Empty
            End If
        End Get
    End Property

#End Region

#Region "Page Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Page.Title = Me.GetGlobalResourceObject("Title", "DownloadAreaBanner")

        Dim strSelectedLang As String = IIf(IsNothing(Session("language")), CultureLanguage.English, Session("language")).ToString().Trim

        trEngVersion.Visible = False
        trChiVersion.Visible = False

        trEngSmartIDCardReader.Visible = False
        trChiSmartIDCardReader.Visible = False

        Select Case strSelectedLang
            Case CultureLanguage.English
                trEngVersion.Visible = True
            Case CultureLanguage.TradChinese
                trChiVersion.Visible = True
            Case Else
                trEngVersion.Visible = True
        End Select

        InitControlAlways()

        ' Show the link of SmartID Card Reader if login
        If UserACBLL.Exist Then
            trEngSmartIDCardReader.Visible = True
            trChiSmartIDCardReader.Visible = True
        End If


    End Sub

    Private Sub InitControlAlways()
        tdHeader.Style("background-image") = String.Format("url({0})", Me.GetGlobalResourceObject("ImageUrl", "Banner"))

        Dim udtGeneralFunction As New GeneralFunction

        ' Link Javascript
        Dim strPrivacyPolicyLink As String = String.Empty

        Select Case Session("language")
            Case CultureLanguage.English
                udtGeneralFunction.getSystemParameter("PrivacyPolicyLink", strPrivacyPolicyLink, String.Empty)
            Case CultureLanguage.TradChinese
                udtGeneralFunction.getSystemParameter("PrivacyPolicyLink_CHI", strPrivacyPolicyLink, String.Empty)
            Case CultureLanguage.SimpChinese
                udtGeneralFunction.getSystemParameter("PrivacyPolicyLink_CN", strPrivacyPolicyLink, String.Empty)
            Case Else
                Throw New Exception(String.Format("Unexpected value (Session(language)={0})", Session("language")))
        End Select

        lnkBtnPrivacyPolicy.OnClientClick = "javascript:openNewHTML('" + strPrivacyPolicyLink + "');return false;"

        Dim strDisclaimerPolicyLink As String = String.Empty

        Select Case Session("language")
            Case CultureLanguage.English
                udtGeneralFunction.getSystemParameter("DisclaimerLink", strDisclaimerPolicyLink, String.Empty)
            Case CultureLanguage.TradChinese
                udtGeneralFunction.getSystemParameter("DisclaimerLink_CHI", strDisclaimerPolicyLink, String.Empty)
            Case CultureLanguage.SimpChinese
                udtGeneralFunction.getSystemParameter("DisclaimerLink_CN", strDisclaimerPolicyLink, String.Empty)
            Case Else
                Throw New Exception(String.Format("Unexpected value (Session(language)={0})", Session("language")))
        End Select

        lnkBtnDisclaimer.OnClientClick = "javascript:openNewHTML('" + strDisclaimerPolicyLink + "');return false;"

        Dim strSysMaintLink As String = String.Empty

        Select Case Session("language")
            Case CultureLanguage.English
                udtGeneralFunction.getSystemParameter("SysMaintLink", strSysMaintLink, String.Empty)
            Case CultureLanguage.TradChinese
                udtGeneralFunction.getSystemParameter("SysMaintLink_CHI", strSysMaintLink, String.Empty)
            Case CultureLanguage.SimpChinese
                udtGeneralFunction.getSystemParameter("SysMaintLink_CN", strSysMaintLink, String.Empty)
            Case Else
                Throw New Exception(String.Format("Unexpected value (Session(language)={0})", Session("language")))
        End Select

        lnkBtnSysMaint.OnClientClick = "javascript:openNewHTML('" + strSysMaintLink + "');return false;"

        lblAppEnvironment.Text = (New GeneralFunction).getSystemParameter("AppEnvironment")

        If lblAppEnvironment.Text.ToLower = "production" Then lblAppEnvironment.Text = String.Empty

        Select Case Session("language")
            Case CultureLanguage.TradChinese, CultureLanguage.SimpChinese
                tdHeader.Attributes("class") = "AppEnvironmentZH"
            Case Else
                tdHeader.Attributes("class") = "AppEnvironment"
        End Select

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
        Return Nothing
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

#End Region

End Class