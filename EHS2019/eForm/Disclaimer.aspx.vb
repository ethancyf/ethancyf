Imports Common.ComObject
Imports Common.Component
Imports Common.ComFunction

Partial Public Class Disclaimer
    'Inherits System.Web.UI.Page
    Inherits BasePage

    ' CRE17-015-02 (Disallow public using WinXP) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Private udtEFormBLL As eFormBLL = New eFormBLL
    ' CRE17-015-02 (Disallow public using WinXP) [End][Chris YIM]

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        txtProceedImageUrl.Text = Me.GetGlobalResourceObject("ImageUrl", "ProceedBtn")
        txtProceedDisableImageUrl.Text = Me.GetGlobalResourceObject("ImageUrl", "ProceedDisableBtn")
        lnkBtnDocToRead.Text = Me.GetGlobalResourceObject("Text", "DocumentToRead")

        If Not IsPostBack Then
            ' CRE17-015-02 (Disallow public using WinXP) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Dim strAbleToAccessThisPage As String = String.Empty
            strAbleToAccessThisPage = Session(eFormBLL.SESS_Disclaimer)
            udtEFormBLL.ClearRedirectPageSession()

            If IsNothing(strAbleToAccessThisPage) OrElse Not strAbleToAccessThisPage.Trim.Equals("Y") Then
                Response.Redirect("~/main.aspx")
            End If
            ' CRE17-015-02 (Disallow public using WinXP) [End][Chris YIM]

            ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT020101, Me) ''Begin Writing Audit Log
            udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00078, "Enrolment Disclaimer Page Loaded")

            FunctionCode = FunctCode.FUNT020101
            ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

            ibtnClose.Attributes.Add("OnClick", "javascript:window.close();")
        End If

        If chkAgreeDisclaimer.Checked Then
            ibtnProceed.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProceedBtn")
            ibtnProceed.Enabled = True
        Else
            ibtnProceed.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProceedDisableBtn")
            ibtnProceed.Enabled = False
        End If

        Dim selectedValue As String
        selectedValue = LCase(Session("language"))

        Dim strHost As String = Request.Url.ToString.Substring(0, Request.Url.ToString.IndexOf(Request.Path))
        Dim strLinkDocToRead As String = strHost & Request.ApplicationPath
        If strLinkDocToRead.Substring(strLinkDocToRead.Length - 1, 1) <> "/" Then
            If selectedValue.Equals("en-us") Then
                strLinkDocToRead = strLinkDocToRead & "/DocToRead/DocToRead.htm"
            ElseIf selectedValue.Equals("zh-tw") Then
                strLinkDocToRead = strLinkDocToRead & "/DocToRead/DocToRead_CHI.htm"
            End If

        Else
            If selectedValue.Equals("en-us") Then
                strLinkDocToRead = strLinkDocToRead & "DocToRead/DocToRead.htm"
            ElseIf selectedValue.Equals("zh-tw") Then
                strLinkDocToRead = strLinkDocToRead & "DocToRead/DocToRead_CHI.htm"
            End If

        End If

        Me.lnkBtnDocToRead.OnClientClick = "javascript:window.openNewWin('" + strLinkDocToRead + "');return false;"
        Me.ibtnDocToRead.OnClientClick = "javascript:window.openNewWin('" + strLinkDocToRead + "');return false;"

        Dim lnkBtnDocToReadMaster As LinkButton = Master.FindControl("lnkBtnDocRead")
        lnkBtnDocToReadMaster.Visible = False

        Dim lnkBtnContactUs As LinkButton = New LinkButton
        lnkBtnContactUs = Master.FindControl("lnkBtnContactUs")
        lnkBtnContactUs.Visible = False

        ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
        ' ----------------------------------------------------------
        ObsoletedOSHandler.HandleObsoleteOS(CommonSessionHandler.OS, ModalPopupExtenderReminderWindowsVersion, ObsoletedOSHandler.Version.Full, _
                                            Me.FunctionCode, LogID.LOG00079, Me, Nothing)
        ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]
    End Sub

    Protected Sub ibtnProceed_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        msgBox.Visible = False
        imgCaptchaAlert.Visible = False
        imgAgreeDisclaimerAlert.Visible = False

        If CaptchaControl1.UserValidated Then
        Else
            Dim errorCode As String
            errorCode = CaptchaControl1.ErrorMessage
            imgCaptchaAlert.Visible = True
            'SM = New Common.ComObject.SystemMessage("99000", "E", errorCode)
            msgBox.AddMessage("990000", "E", errorCode)
        End If

        If Not Me.chkAgreeDisclaimer.Checked Then
            imgAgreeDisclaimerAlert.Visible = True
            'SM = New Common.ComObject.SystemMessage("020101", "E", "00001")
            msgBox.AddMessage("020101", "E", "00001")
        End If

        If msgBox.GetCodeTable.Rows.Count = 0 Then
            Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT020101, Me) ''Begin Writing Audit Log
            udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00001, "Checked the disclaimer and Click Proceed Button")

            msgBox.Visible = False
            'Response.Redirect("~/eForm.aspx")
            Dim selectedValue As String
            selectedValue = Session("language")

            ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Dim blnReminderForWindowsVersion As Boolean = CommonSessionHandler.ReminderForWindowsVersion
            ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

            Session.RemoveAll()
            Session("language") = selectedValue

            ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
            ' ----------------------------------------------------------
            CommonSessionHandler.ReminderForWindowsVersion = blnReminderForWindowsVersion
            ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

            'ibtnProceed.PostBackUrl = "~/eForm.aspx"
            'Session("FromDisclaimer") = "Y"

            Session(eFormBLL.SESS_PersonalParticular) = "Y"
            Response.Redirect("~/PersonalPacticulars.aspx")
        Else
            msgBox.BuildMessageBox("ValidationFail")
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If IsPostBack Then
            Dim controlID As String = Page.Request.Params.Get(PostBackEventTarget)
            If controlID.Equals(SelectTradChinese) OrElse controlID.Equals(SelectEnglish) Then
                If Me.chkAgreeDisclaimer.Checked Then
                    Me.ibtnProceed.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ProceedBtn")
                Else
                    Me.ibtnProceed.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ProceedDisableBtn")
                End If
            End If
        End If
    End Sub

    ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Protected Sub ibtnReminderWindowsVersion_OK_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim udtAuditLogEntry As New AuditLogEntry(Me.FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00080, "Reminder - Obsolete Windows Version - OK Click")

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
End Class