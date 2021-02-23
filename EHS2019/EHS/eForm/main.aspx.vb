Imports Common.Component.Scheme
Imports Common.ComObject
Imports Common.Component
Imports Common.ComFunction

Partial Public Class main
    Inherits BasePage

    Private GeneralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
    Private udtSchemeEFormBLL As SchemeEFormBLL = New SchemeEFormBLL

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        lnkBtnDocToRead.Text = Me.GetGlobalResourceObject("Text", "DocumentToRead")

        txtDownloadImageUrl.Text = Me.GetGlobalResourceObject("ImageUrl", "DownloadBtn")
        txtDownloadDisableImageUrl.Text = Me.GetGlobalResourceObject("ImageUrl", "DownloadDisableBtn")

        Dim selectedValue As String = Nothing
        If Not IsPostBack Then
            ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Dim blnReminderForWindowsVersion As Boolean = Nothing

            'Backup Session Variables
            If Not Session Is Nothing Then
                If Not Session("language") Is Nothing Then
                    selectedValue = Session("language")
                End If

                blnReminderForWindowsVersion = CommonSessionHandler.ReminderForWindowsVersion
            End If

            Session.RemoveAll()

            'Restore Session Variables
            If Not selectedValue Is Nothing Then
                Session("language") = selectedValue
            End If

            CommonSessionHandler.ReminderForWindowsVersion = blnReminderForWindowsVersion

            Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT020101, Me) ''Begin Writing Audit Log
            udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00075, "Enrolment Main Page Loaded")

            FunctionCode = FunctCode.FUNT020101
            ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]
        End If

        ' I-CRE16-003 Fix XSS [Start][Lawrence]
        Dim strRequestUrl As String = AntiXss.AntiXssEncoder.HtmlEncode(Request.Url.ToString, True)
        Dim strHost As String = strRequestUrl.Substring(0, strRequestUrl.IndexOf(Request.Path))
        ' I-CRE16-003 Fix XSS [End][Lawrence]

        Dim strLink As String = strHost & Request.ApplicationPath
        If strLink.Substring(strLink.Length - 1, 1) <> "/" Then
            strLink = strLink & "/Disclaimer.aspx"
        Else
            strLink = strLink & "Disclaimer.aspx"
        End If

        ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
        ' ----------------------------------------------------------
        'Add javascript to get the selected structure address
        Dim openNewWindowScript As New StringBuilder
        openNewWindowScript.Append("<Script type='text/javascript'>")
        openNewWindowScript.Append("function goNewWin() {")
        openNewWindowScript.Append("document.getElementById('" & btnHiddenOnlineEnrol.ClientID & "').click();")
        openNewWindowScript.Append("}")
        openNewWindowScript.Append("</Script>")
        ClientScript.RegisterStartupScript(Me.GetType(), "OpenNewWindows", openNewWindowScript.ToString())
        ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

        Dim selectedLanguageValue As String
        selectedLanguageValue = LCase(Session("language"))

        Dim strLinkDocToRead As String = strHost & Request.ApplicationPath
        If strLinkDocToRead.Substring(strLinkDocToRead.Length - 1, 1) <> "/" Then
            If selectedLanguageValue.Equals("en-us") Then
                strLinkDocToRead = strLinkDocToRead & "/DocToRead/DocToRead.htm"
            ElseIf selectedLanguageValue.Equals("zh-tw") Then
                strLinkDocToRead = strLinkDocToRead & "/DocToRead/DocToRead_CHI.htm"

            End If
        Else
            If selectedLanguageValue.Equals("en-us") Then
                strLinkDocToRead = strLinkDocToRead & "DocToRead/DocToRead.htm"
            ElseIf selectedLanguageValue.Equals("zh-tw") Then
                strLinkDocToRead = strLinkDocToRead & "DocToRead/DocToRead_CHI.htm"

            End If
        End If

        Me.lnkBtnDocToRead.OnClientClick = "javascript:window.openNewWin('" + strLinkDocToRead + "');return false;"
        Me.ibtnDocToRead.OnClientClick = "javascript:window.openNewWin('" + strLinkDocToRead + "');return false;"

        ' CRE15-006 Rename eHS - Change Online Demo to Easy Guide [Start][Winnie]
        Dim strEasyGuide As String = Me.GetGlobalResourceObject("Url", "eFormEasyGuideUrl")
        ibtnEasyGuide.OnClientClick = "javascript:window.openNewWin('" + strEasyGuide + "');return false;"
        ' CRE15-006 Rename eHS - Change Online Demo to Easy Guide [End][Winnie]

        Dim lnkBtnDocToReadMaster As LinkButton = Master.FindControl("lnkBtnDocRead")
        lnkBtnDocToReadMaster.Visible = False

        SetLangage()

        ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
        ' ----------------------------------------------------------
        ObsoletedOSHandler.HandleObsoleteOS(CommonSessionHandler.OS, ModalPopupExtenderReminderWindowsVersion, ObsoletedOSHandler.Version.Full, _
                                            Me.FunctionCode, LogID.LOG00076, Me, Nothing)
        ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

        'CRE20-018 Stop Token Sharing [Start][Nichole]
        Dim udtGeneralFunction As New GeneralFunction
        Dim streHRSSToken As String = udtGeneralFunction.getSystemParameter("eHRSS_Token")
        If streHRSSToken = YesNo.No Then
            trEHRSSConsentForm.Visible = False
        End If
        'CRE20-018 Stop Token Sharing [End][Nichole]

    End Sub

    Private Sub SetLangage()
        Dim selectedValue As String

        selectedValue = Session("language")

        Dim lnkbtnEnglish As LinkButton
        Dim lnkbtnTradChinese As LinkButton
        Dim lnkbtnSimpChinese As LinkButton

        lnkbtnEnglish = Me.Master.FindControl("lnkbtnEnglish")
        lnkbtnTradChinese = Me.Master.FindControl("lnkbtnTradChinese")
        lnkbtnSimpChinese = Me.Master.FindControl("lnkbtnSimpleChinese")

        Select Case selectedValue
            Case English
                Me.Master.FindControl("")
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
                lnkbtnEnglish.CssClass = "languageSelectedText"
                lnkbtnTradChinese.CssClass = "languageText"
                lnkbtnSimpChinese.CssClass = "languageText"
                lnkbtnEnglish.Enabled = False
                lnkbtnTradChinese.Enabled = True
                lnkbtnSimpChinese.Enabled = True
        End Select
    End Sub

    Protected Sub lnkBtnDocToRead_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Protected Sub ibtnReminderWindowsVersion_OK_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim udtAuditLogEntry As New AuditLogEntry(Me.FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00077, "Reminder - Obsolete Windows Version - OK Click")

        Me.ModalPopupExtenderReminderWindowsVersion.Hide()
    End Sub
    ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

#Region "Implement IWorkingData (CRE11-004)"

    Public Overrides Function GetDocCode() As String
        Return Nothing
    End Function

    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Return Nothing
    End Function

    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        Return Nothing
    End Function

#End Region

    Private Sub btnHiddenOnlineEnrol_Click(sender As Object, e As EventArgs) Handles btnHiddenOnlineEnrol.Click
        Dim udteFormBLL As New eFormBLL
        udteFormBLL.ClearRedirectPageSession()

        Session(eFormBLL.SESS_Disclaimer) = "Y"
        Response.Redirect("~/Disclaimer.aspx")
    End Sub
End Class