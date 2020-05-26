Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Format
Imports Common.Validation
Imports System.Globalization
Imports System.Threading
Imports VoucherBalanceEnquiry.Component.VoucherAccEnquiryFailRecord
Imports VoucherBalanceEnquiry.ucNoticePopUp
Imports Common.Component.VoucherInfo

Partial Public Class main
    Inherits BasePage

    ' FunctionCode = FunctCode.FUNT030101

#Region "Private Classes"

    Private Class ViewIndex
        Public Const Search As Integer = 0
        Public Const Result As Integer = 1
    End Class

#End Region

#Region "Fields"

    Private udtEHSAccountBLL As New EHSAccountBLL
    Private udtEHSTransactionBLL As New EHSTransactionBLL
    Private udtFormatter As New Formatter
    Private udtGeneralFunction As New GeneralFunction
    Private udtSchemeClaimBLL As New SchemeClaimBLL
    Private udtValidator As New Validator
    Private udtVoucherAccEnquiryFailRecordBLL As New VoucherAccEnquiryFailRecordBLL

#End Region

#Region "Constants"

    Private Const HCVS As String = "HCVS"
    Private Const EHCVS As String = "EHCVS"

#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        FunctionCode = FunctCode.FUNT030101

        Dim udtAuditLogRedirect As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogRedirect.WriteLog(LogID.LOG00010, "Voucher Balance Enquiry load (Obsoleted)")

        Dim strRedirectLink As String = ConfigurationManager.AppSettings("RedirectLinkChi")
        HttpContext.Current.Response.Redirect(strRedirectLink)

        Return

        If Not IsPostBack Then
            FunctionCode = FunctCode.FUNT030101

            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00010, "Voucher Balance Enquiry load (Obsoleted)")

            ResetAlertImage()

            Me.pnlECDOB.Visible = False
            Me.pnlHKIDDOB.Visible = True
            imgHKIDSample.Visible = True
            lblPatSearchHKIDHint.Visible = False
            lblPatSearchDOBHint.Visible = False
            lblPatSearchDOBHint1.Visible = False
            lblECDAgeTip.Visible = False

            rboDOB.Checked = True
            rboECAge.Checked = False

            txtPatsDOB.Attributes.Remove("readonly")
            txtPatsDOB.BackColor = Nothing

            txtPatsAge.BackColor = Drawing.Color.WhiteSmoke
            txtPatsAge.Attributes.Add("readonly", "readonly")
            txtPatsAge.Text = String.Empty

            txtRegisterOnDay.BackColor = Drawing.Color.WhiteSmoke
            txtRegisterOnDay.Attributes.Add("readonly", "readonly")
            txtRegisterOnDay.Text = String.Empty

            txtRegisterOnYear.BackColor = Drawing.Color.WhiteSmoke
            txtRegisterOnYear.Attributes.Add("readonly", "readonly")
            txtRegisterOnYear.Text = String.Empty

            Me.ddlRegisterOnMonth.Enabled = False

            txtRegisterOnDayChi.BackColor = Drawing.Color.WhiteSmoke
            txtRegisterOnDayChi.Attributes.Add("readonly", "readonly")
            txtRegisterOnDayChi.Text = String.Empty

            txtRegisterOnYearChi.BackColor = Drawing.Color.WhiteSmoke
            txtRegisterOnYearChi.Attributes.Add("readonly", "readonly")
            txtRegisterOnYearChi.Text = String.Empty

            Me.ddlRegisterOnMonthChi.Enabled = False

            lblPatSearchDOBHint.Style.Item("DISPLAY") = "block"
            lblPatSearchDOBHint1.Style.Item("DISPLAY") = "block"
            lblECDAgeTip.Style.Item("DISPLAY") = "none"

            If UCase(CStr(Session("language"))).Equals("ZH-TW") Then
                pnlEngECAge.Style.Item("DISPLAY") = "none"
                pnlChiECAge.Style.Item("DISPLAY") = "block"
            Else
                pnlEngECAge.Style.Item("DISPLAY") = "block"
                pnlChiECAge.Style.Item("DISPLAY") = "none"
            End If

        End If

        SetControlFocus(Me.txtPatsHKID)

        SetLangage()

        lblAppEnvironment.Text = (New GeneralFunction).getSystemParameter("AppEnvironment")

        If lblAppEnvironment.Text.ToLower = "production" Then lblAppEnvironment.Text = String.Empty

        Select Case Session("language")
            Case CultureLanguage.TradChinese, CultureLanguage.SimpChinese
                tdAppEnvironment.Attributes("class") = "AppEnvironmentZH"
            Case Else
                tdAppEnvironment.Attributes("class") = "AppEnvironment"
        End Select

        If Me.rboHKIDType.SelectedValue.Equals("Y") Then
            Me.pnlECDOB.Visible = False
            Me.pnlHKIDDOB.Visible = True
            imgHKIDSample.Visible = True
            lblPatSearchHKIDHint.Visible = False
            lblPatSearchDOBHint.Visible = False
            lblPatSearchDOBHint1.Visible = False
            lblECDAgeTip.Visible = False

            lblPatsDOBText.Text = Me.GetGlobalResourceObject("Text", "DOBLongForm")

        Else
            Me.pnlECDOB.Visible = True
            Me.pnlHKIDDOB.Visible = False
            imgHKIDSample.Visible = False
            lblPatSearchHKIDHint.Visible = True
            lblPatSearchDOBHint.Visible = True
            lblPatSearchDOBHint1.Visible = True
            lblECDAgeTip.Visible = True

            lblPatsDOBText.Text = Me.GetGlobalResourceObject("Text", "DOBYOB")

            If Me.rboDOB.Checked Then
                lblPatSearchDOBHint.Style.Item("DISPLAY") = "block"
                lblPatSearchDOBHint1.Style.Item("DISPLAY") = "block"
                lblECDAgeTip.Style.Item("DISPLAY") = "none"

                rboDOB.Checked = True
                rboECAge.Checked = False

                txtPatsDOB.Attributes.Remove("readonly")
                txtPatsDOB.BackColor = Nothing

                txtPatsAge.BackColor = Drawing.Color.WhiteSmoke
                txtPatsAge.Attributes.Add("readonly", "readonly")
                txtPatsAge.Text = String.Empty

                txtRegisterOnDay.BackColor = Drawing.Color.WhiteSmoke
                txtRegisterOnDay.Attributes.Add("readonly", "readonly")
                txtRegisterOnDay.Text = String.Empty

                txtRegisterOnYear.BackColor = Drawing.Color.WhiteSmoke
                txtRegisterOnYear.Attributes.Add("readonly", "readonly")
                txtRegisterOnYear.Text = String.Empty

                ddlRegisterOnMonth.Enabled = False
                ddlRegisterOnMonth.SelectedIndex = 0

                txtRegisterOnDayChi.BackColor = Drawing.Color.WhiteSmoke
                txtRegisterOnDayChi.Attributes.Add("readonly", "readonly")
                txtRegisterOnDayChi.Text = String.Empty

                txtRegisterOnYearChi.BackColor = Drawing.Color.WhiteSmoke
                txtRegisterOnYearChi.Attributes.Add("readonly", "readonly")
                txtRegisterOnYearChi.Text = String.Empty

                ddlRegisterOnMonthChi.Enabled = False
                ddlRegisterOnMonthChi.SelectedIndex = 0


            ElseIf Me.rboECAge.Checked Then
                lblPatSearchDOBHint.Style.Item("DISPLAY") = "none"
                lblPatSearchDOBHint1.Style.Item("DISPLAY") = "none"
                lblECDAgeTip.Style.Item("DISPLAY") = "block"

                rboDOB.Checked = False
                rboECAge.Checked = True

                txtPatsDOB.BackColor = Drawing.Color.WhiteSmoke
                txtPatsDOB.Attributes.Add("readonly", "readonly")
                txtPatsDOB.Text = String.Empty

                txtPatsAge.Attributes.Remove("readonly")
                txtPatsAge.BackColor = Nothing

                txtRegisterOnDay.Attributes.Remove("readonly")
                txtRegisterOnDay.BackColor = Nothing

                txtRegisterOnYear.Attributes.Remove("readonly")
                txtRegisterOnYear.BackColor = Nothing

                ddlRegisterOnMonth.Enabled = True

                txtRegisterOnDayChi.Attributes.Remove("readonly")
                txtRegisterOnDayChi.BackColor = Nothing

                txtRegisterOnYearChi.Attributes.Remove("readonly")
                txtRegisterOnYearChi.BackColor = Nothing

                ddlRegisterOnMonth.Enabled = True

            End If
        End If



        tblBanner.Style.Item("background-image") = "url(" + Me.GetGlobalResourceObject("ImageUrl", "IndexBanner") + ")"
        Me.PageTitle.InnerText = Me.GetGlobalResourceObject("Title", "VoucherBalanceEnquiry")

        Dim selectedLang As String
        selectedLang = LCase(Session("language"))


        Dim strPrivacyPolicyLink As String = String.Empty

        If selectedLang.Equals(English) Then
            udtGeneralFunction.getSystemParameter("PrivacyPolicyLink", strPrivacyPolicyLink, String.Empty)
        ElseIf selectedLang.Equals(TradChinese) Then
            udtGeneralFunction.getSystemParameter("PrivacyPolicyLink_CHI", strPrivacyPolicyLink, String.Empty)
        End If

        Me.lnkBtnPrivacyPolicy.OnClientClick = "javascript:openNewWin('" + strPrivacyPolicyLink + "');return false;"

        Dim strDisclaimerLink As String = String.Empty

        If selectedLang.Equals(English) Then
            udtGeneralFunction.getSystemParameter("DisclaimerLink", strDisclaimerLink, String.Empty)
        ElseIf selectedLang.Equals(TradChinese) Then
            udtGeneralFunction.getSystemParameter("DisclaimerLink_CHI", strDisclaimerLink, String.Empty)
        End If

        Me.lnkBtnDisclaimer.OnClientClick = "javascript:openNewWin('" + strDisclaimerLink + "');return false;"

        Dim strFAQsLink As String = String.Empty

        If selectedLang.Equals(English) Then
            udtGeneralFunction.getSystemParameter("FAQsLink_HCVR", strFAQsLink, String.Empty)
        ElseIf selectedLang.Equals(TradChinese) Then
            udtGeneralFunction.getSystemParameter("FAQsLink_HCVR_CHI", strFAQsLink, String.Empty)
        End If

        Me.ibtnFAQ.OnClientClick = "javascript:openNewWin('" + strFAQsLink + "');return false;"

        Dim strContactUsLink As String = String.Empty

        If selectedLang.Equals(English) Then
            udtGeneralFunction.getSystemParameter("ContactUsSPLink", strContactUsLink, String.Empty)
        ElseIf selectedLang.Equals(TradChinese) Then
            udtGeneralFunction.getSystemParameter("ContactUsSPLink_CHI", strContactUsLink, String.Empty)
        End If

        Me.ibtnContactUs.OnClientClick = "javascript:openNewWin('" + strContactUsLink + "');return false;"

        Dim strSysMaintLink As String = String.Empty

        If selectedLang.Equals(English) Then
            udtGeneralFunction.getSystemParameter("SysMaintLink", strSysMaintLink, String.Empty)
        ElseIf selectedLang.Equals(TradChinese) Then
            udtGeneralFunction.getSystemParameter("SysMaintLink_CHI", strSysMaintLink, String.Empty)
        End If
        Me.lnkBtnSysMaint.OnClientClick = "javascript:openNewWin('" + strSysMaintLink + "');return false;"

        Dim strEasyGuide As String = Me.GetGlobalResourceObject("Url", "HCVREasyGuideUrl")
        Me.ibtnEasyGuide.OnClientClick = "javascript:openNewWin('" + strEasyGuide + "');return false;"

        lblPatSearchHKIDHint.Text = Me.GetGlobalResourceObject("Text", "HKICHint")

        ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
        ' ----------------------------------------------------------
        ObsoletedOSHandler.HandleObsoleteOS(CommonSessionHandler.OS, ModalPopupExtenderReminderWindowsVersion, ObsoletedOSHandler.Version.Full, _
                                            Me.FunctionCode, LogID.LOG00005, Me, Nothing)
        ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

    End Sub

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.basetag.Attributes("href") = udtGeneralFunction.getPageBasePath()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If IsPostBack Then
            Dim controlID As String = Page.Request.Params.Get(PostBackEventTarget)
            If controlID.Equals(SelectTradChinese) OrElse controlID.Equals(SelectEnglish) OrElse _
                controlID.Equals(_SelectTradChinese) OrElse controlID.Equals(_SelectEnglish) Then
                ReRenderPage()
            End If
        End If
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

        lblPatSearchDOBHint.Text = Me.GetGlobalResourceObject("Text", "ECDOBHint")
        lblPatSearchDOBHint1.Text = Me.GetGlobalResourceObject("Text", "ECDOBHint2")
        lblECDAgeTip.Text = Me.GetGlobalResourceObject("Text", "ECDORegisterAgeHint")

        If UCase(CStr(Session("language"))).Equals("ZH-TW") Then
            pnlEngECAge.Style.Item("DISPLAY") = "none"
            pnlChiECAge.Style.Item("DISPLAY") = "block"
            txtRegisterOnDayChi.Text = txtRegisterOnDay.Text.Trim
            txtRegisterOnYearChi.Text = txtRegisterOnYear.Text.Trim
            ddlRegisterOnMonthChi.SelectedValue = ddlRegisterOnMonth.SelectedValue.Trim

            lblDisplayDOB.Visible = False
            lblDisplayDOB_Chi.Visible = True
        Else
            pnlEngECAge.Style.Item("DISPLAY") = "block"
            pnlChiECAge.Style.Item("DISPLAY") = "none"
            txtRegisterOnDay.Text = txtRegisterOnDayChi.Text.Trim
            txtRegisterOnYear.Text = txtRegisterOnYearChi.Text.Trim
            ddlRegisterOnMonth.SelectedValue = ddlRegisterOnMonthChi.SelectedValue.Trim

            lblDisplayDOB_Chi.Visible = False
            lblDisplayDOB.Visible = True
        End If

        lblECYear.Text = Me.GetGlobalResourceObject("Text", "Year")
        lblECMonth.Text = Me.GetGlobalResourceObject("Text", "Month")
        lblECDay.Text = Me.GetGlobalResourceObject("Text", "Day")

        imgHKIDSample.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "HKICSampleImg")
        imgHKIDSample.AlternateText = Me.GetGlobalResourceObject("AlternateText", "HKICSampleImg")

        ' CRE19-003 (Opt voucher capping) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        If UCase(CStr(Session("language"))).Equals("ZH-TW") Then
            lblDisplayAvailableQuotaText.Visible = False
            lblDisplayAvailableQuotaUpto.Visible = False

            lblDisplayAvailableQuotaText_Chi.Visible = True
            lblDisplayAvailableQuotaUpto_Chi.Visible = True

            lblAnnualAllotmentText.Visible = False
            lblAnnualAllotmentText_Chi.Visible = True

            lblForfeitVoucherText.Visible = False
            lblForfeitVoucherText_Chi.Visible = True

            lblForfeitVoucherNote.Visible = False
            lblForfeitVoucherNote_Chi.Visible = True

            lblMaximumVoucherAmountText.Visible = False
            lblMaximumVoucherAmountText_Chi.Visible = True

            lblMaximumVoucherAmountNote.Visible = False
            lblMaximumVoucherAmountNote_Chi.Visible = True
        Else
            lblDisplayAvailableQuotaText.Visible = True
            lblDisplayAvailableQuotaUpto.Visible = True

            lblDisplayAvailableQuotaText_Chi.Visible = False
            lblDisplayAvailableQuotaUpto_Chi.Visible = False

            lblAnnualAllotmentText.Visible = True
            lblAnnualAllotmentText_Chi.Visible = False

            lblForfeitVoucherText.Visible = True
            lblForfeitVoucherText_Chi.Visible = False

            lblForfeitVoucherNote.Visible = True
            lblForfeitVoucherNote_Chi.Visible = False

            lblMaximumVoucherAmountText.Visible = True
            lblMaximumVoucherAmountText_Chi.Visible = False

            lblMaximumVoucherAmountNote.Visible = True
            lblMaximumVoucherAmountNote_Chi.Visible = False
        End If
        ' CRE19-003 (Opt voucher capping) [End][Winnie]

        If Me.imgHKIDSymbolSample.Visible Then
            Me.imgHKIDSymbolSample.Style.Remove("display")
        Else
            Me.imgHKIDSymbolSample.Style.Add("display", "none")
        End If

    End Sub

#End Region

    Private Sub ResetAlertImage()
        ImgPatsHKIDAlert.Visible = False
        imgPatsDOBAlert.Visible = False
        imgCaptchaControlAlert.Visible = False
        imgECDORAlert.Visible = False
        imgHKIDDOBAlert.Visible = False
    End Sub

    Protected Sub ibtnComplete_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.Response.Redirect("~/thankyou.aspx")
    End Sub


    Private Sub ibtnSearchPat_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSearchPat.Click
        Dim udtDocTypeBLL As New DocType.DocTypeBLL

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Identity No", txtPatsHKID.Text)
        udtAuditLogEntry.AddDescripton("Is HKID", rboHKIDType.SelectedValue)

        If rboHKIDType.SelectedValue = "Y" Then
            udtAuditLogEntry.AddDescripton("DOB", txtHKIDDOB.Text)
        Else
            udtAuditLogEntry.AddDescripton("DOB", txtPatsDOB.Text)
        End If

        udtAuditLogEntry.AddDescripton("Age", txtPatsAge.Text)

        udcInfoMessageBox.Visible = False
        udcMessageBox.Visible = False

        If CStr(Session("language")).ToUpper = "ZH-TW" Then
            udtAuditLogEntry.AddDescripton("DOR Year", txtRegisterOnYearChi.Text)
            udtAuditLogEntry.AddDescripton("DOR Month", ddlRegisterOnMonthChi.SelectedValue)
            udtAuditLogEntry.AddDescripton("DOR Day", txtRegisterOnDayChi.Text)
        Else
            udtAuditLogEntry.AddDescripton("DOR Year", txtRegisterOnYear.Text)
            udtAuditLogEntry.AddDescripton("DOR Month", ddlRegisterOnMonth.SelectedValue)
            udtAuditLogEntry.AddDescripton("DOR Day", txtRegisterOnDay.Text)
        End If

        Dim objAuditLogInfo As AuditLogInfo = New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, IIf(rboHKIDType.SelectedValue = "Y", DocTypeCode.HKIC, DocTypeCode.EC), udtFormatter.formatHKIDInternal(txtPatsHKID.Text))
        udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Search", objAuditLogInfo)

        ResetAlertImage()

        Dim udtSystemMessage As SystemMessage = Nothing
        Dim strDocCode As String = IIf(rboHKIDType.SelectedValue = "Y", DocTypeCode.HKIC, DocTypeCode.EC) ' CRE11-007

        If udtValidator.IsEmpty(txtPatsHKID.Text.Trim) Then
            udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00001)
            ImgPatsHKIDAlert.Visible = True

        Else
            udtSystemMessage = udtValidator.chkHKID(udtFormatter.formatHKIDInternal(txtPatsHKID.Text))

            ' -------------------------------------------------------------------------------
            ' Check Active Death Record
            ' If dead, return "(document id name) is invalid"
            ' -------------------------------------------------------------------------------
            If udtSystemMessage Is Nothing Then
                If udtDocTypeBLL.getDocTypeByAvailable(DocType.DocTypeBLL.EnumAvailable.DeathRecordAvailable).Filter(strDocCode) IsNot Nothing Then
                    If (New eHealthAccountDeathRecord.eHealthAccountDeathRecordBLL).GetDeathRecordEntry(txtPatsHKID.Text).IsDead() Then
                        udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
                        udcInfoMessageBox.BuildMessageBox()

                        Return
                    End If
                End If
            End If

            If Not IsNothing(udtSystemMessage) Then
                udcMessageBox.AddMessage(udtSystemMessage)
                ImgPatsHKIDAlert.Visible = True

            End If
        End If

        If rboHKIDType.SelectedValue = "Y" Then
            ' Hong Kong Identity Card

            udtSystemMessage = udtValidator.chkDOB(DocType.DocTypeModel.DocTypeCode.HKIC, udtFormatter.formatInputDate(txtHKIDDOB.Text.Trim))

            If Not IsNothing(udtSystemMessage) Then
                udcMessageBox.AddMessage(udtSystemMessage)
                imgHKIDDOBAlert.Visible = True
            End If

        Else
            ' Certificate of Exemption
            If rboDOB.Checked Then

                udtSystemMessage = udtValidator.chkDOB(DocType.DocTypeModel.DocTypeCode.EC, udtFormatter.formatInputDate(txtPatsDOB.Text.Trim))

                If Not udtSystemMessage Is Nothing Then
                    udcMessageBox.AddMessage(udtSystemMessage)
                    imgPatsDOBAlert.Visible = True
                End If

            ElseIf rboECAge.Checked Then
                If CStr(Session("language")).ToUpper = "ZH-TW" Then
                    txtRegisterOnDay.Text = txtRegisterOnDayChi.Text.Trim
                    txtRegisterOnYear.Text = txtRegisterOnYearChi.Text.Trim
                    ddlRegisterOnMonth.SelectedValue = ddlRegisterOnMonthChi.SelectedValue.Trim
                Else
                    txtRegisterOnDayChi.Text = txtRegisterOnDay.Text.Trim
                    txtRegisterOnYearChi.Text = txtRegisterOnYear.Text.Trim
                    ddlRegisterOnMonthChi.SelectedValue = ddlRegisterOnMonth.SelectedValue.Trim
                End If

                udtSystemMessage = udtValidator.chkECAge(txtPatsAge.Text.Trim)

                If Not IsNothing(udtSystemMessage) Then
                    udcMessageBox.AddMessage(udtSystemMessage)
                    imgECDORAlert.Visible = True

                Else
                    udtSystemMessage = udtValidator.chkECDOAge(txtRegisterOnDay.Text.Trim, ddlRegisterOnMonth.SelectedValue.Trim, txtRegisterOnYear.Text.Trim)
                    If Not IsNothing(udtSystemMessage) Then
                        udcMessageBox.AddMessage(udtSystemMessage)
                        imgECDORAlert.Visible = True
                    Else
                        udtSystemMessage = udtValidator.chkECAgeAndDOAge(txtPatsAge.Text.Trim, txtRegisterOnDay.Text.Trim, ddlRegisterOnMonth.SelectedValue.Trim, txtRegisterOnYear.Text.Trim)
                        If Not IsNothing(udtSystemMessage) Then
                            udcMessageBox.AddMessage(udtSystemMessage)
                            imgECDORAlert.Visible = True
                        End If
                    End If

                End If

            End If
        End If

        ' Captcha checking
        If Not CaptchaControl.UserValidated Then
            udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, CaptchaControl.ErrorMessage)
            imgCaptchaControlAlert.Visible = True
        End If

        ' Get the EHS Account
        Dim strIdentityNo As String = udtFormatter.formatHKIDInternal(txtPatsHKID.Text)

        Dim udtEHSAccount As EHSAccountModel = udtEHSAccountBLL.LoadEHSAccountByIdentity(strIdentityNo, strDocCode)

        If udcMessageBox.GetCodeTable.Rows.Count <> 0 Then
            If Not IsNothing(udtEHSAccount) Then
                udtVoucherAccEnquiryFailRecordBLL.UpdateFailCount(udtEHSAccount.VoucherAccID)
            End If

            udcMessageBox.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00004, "Search fail", objAuditLogInfo)

            Return
        End If

        If IsNothing(udtEHSAccount) Then
            udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
            udcInfoMessageBox.BuildMessageBox()
            udtAuditLogEntry.AddDescripton("StackTrace", String.Format("EHSAccountModel Is Nothing: Could not find EHS Account with Identity No. {0} and Document Code {1}", strIdentityNo, strDocCode))
            udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search complete. No record found", objAuditLogInfo)

            Return
        End If

        Dim udtEHSPersonalInformation As EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(strDocCode)

        If IsNothing(udtEHSPersonalInformation) Then
            udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
            udcInfoMessageBox.BuildMessageBox()
            udtAuditLogEntry.AddDescripton("StackTrace", String.Format("EHSPersonalInformationModel Is Nothing: Could not find Personal Information with Document Type {0} for Voucher Account {1}", strDocCode, udtEHSAccount.VoucherAccID))
            udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search complete. No record found", objAuditLogInfo)

            Return
        End If

        ' Check if the patient is eligible for HCVS (Aged 70 or above)
        Dim udtClaimRuleBLL As New Common.Component.ClaimRules.ClaimRulesBLL
        Dim udtResult As Common.Component.ClaimRules.ClaimRulesBLL.EligibleResult


        udtResult = udtClaimRuleBLL.CheckEligibilityFromHCVR(HCVS, udtEHSPersonalInformation, udtGeneralFunction.GetSystemDateTime().Date)

        If Not udtResult.IsEligible Then
            udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
            udcInfoMessageBox.BuildMessageBox()
            udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Not udtResult.IsEligible: EHSPersonalInformationModel is not eligible for Document Type {0} and Voucher Account {1}", strDocCode, udtEHSAccount.VoucherAccID))
            udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search complete. No record found", objAuditLogInfo)

            Return
        End If

        Dim strDOB As String = String.Empty
        Dim strAge As String = String.Empty
        Dim dtmDOR As Date = Nothing

        If rboHKIDType.SelectedValue = "Y" Then

            strDOB = udtFormatter.formatInputDate(txtHKIDDOB.Text.Trim)

        Else
            If rboDOB.Checked Then

                strDOB = udtFormatter.formatInputDate(txtPatsDOB.Text.Trim)

            ElseIf rboECAge.Checked Then
                strAge = txtPatsAge.Text.Trim
                dtmDOR = New Date(CInt(txtRegisterOnYear.Text.Trim), CInt(ddlRegisterOnMonth.SelectedValue.Trim), CInt(txtRegisterOnDay.Text.Trim))
            End If

        End If

        Dim IsMatchDOB As Boolean = False

        Select Case udtEHSPersonalInformation.ExactDOB
            Case ExactDOBClass.ExactYear, ExactDOBClass.ReportedYear, ExactDOBClass.ManualExactYear
                If udtEHSPersonalInformation.DOB.ToString(udtFormatter.EnterDateFormat) = "01-01-" + strDOB Then
                    IsMatchDOB = True
                End If

            Case ExactDOBClass.ExactMonth, ExactDOBClass.ManualExactMonth
                If udtEHSPersonalInformation.DOB.ToString(udtFormatter.EnterDateFormat) = "01-" + strDOB Then
                    IsMatchDOB = True
                End If

            Case ExactDOBClass.ExactDate, ExactDOBClass.ManualExactDate
                If udtEHSPersonalInformation.DOB.ToString(udtFormatter.EnterDateFormat) = strDOB Then
                    IsMatchDOB = True
                End If

            Case ExactDOBClass.AgeAndRegistration
                ' Check whether the Age and DOR are matched
                If strAge <> String.Empty AndAlso Not IsNothing(dtmDOR) Then
                    If CInt(udtEHSPersonalInformation.ECAge) = CInt(strAge) AndAlso udtEHSPersonalInformation.ECDateOfRegistration.Equals(dtmDOR) Then
                        IsMatchDOB = True
                    End If
                End If

        End Select


        If udtEHSAccount.PublicEnquiryStatus = VRAcctEnquiryStatus.Available AndAlso IsMatchDOB AndAlso udtEHSAccount.RecordStatus <> VRAcctStatus.Terminated Then

            ' Get the number of available vouchers for this VR
            Dim udtSchemeCList As SchemeClaimModelCollection = udtSchemeClaimBLL.getAllEffectiveSchemeClaim_WithSubsidizeGroup()
            Dim udtSchemeC As SchemeClaimModel = udtSchemeCList.Filter(HCVS)
            Dim udtSubsidizeGroupC As SubsidizeGroupClaimModel = udtSchemeC.SubsidizeGroupClaimList.Filter(HCVS, EHCVS)
            ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Dim udtVoucherInfo As New VoucherInfoModel(VoucherInfoModel.AvailableVoucher.Include, _
                                                       VoucherInfoModel.AvailableQuota.Include)

            udtVoucherInfo.getinfo(udtSchemeC, udtEHSPersonalInformation)

            udtVoucherInfo.FunctionCode = FunctionCode

            Dim intAvailableVoucher As Integer = udtVoucherInfo.GetAvailableVoucher()

            If intAvailableVoucher >= 0 Then
                ' Nothing to do
            Else
                ' Handling negative number of available voucher
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("AvailableVoucher:" + intAvailableVoucher.ToString()))
                udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00033)
                udcInfoMessageBox.BuildMessageBox()

                intAvailableVoucher = 0
            End If

            Dim udtVoucherQuota As New VoucherQuotaModel
            Dim udtVoucherQuotaList As VoucherQuotaModelCollection = udtVoucherInfo.VoucherQuotalist

            If udtVoucherQuotaList.Count > 0 Then
                ' Display first record
                udtVoucherQuota = udtVoucherQuotaList.Item(0)

                Dim intAvailableQuota As Integer = IIf(udtVoucherQuota.AvailableQuota > 0, udtVoucherQuota.AvailableQuota, 0)

                Me.lblDisplayAvailableQuotaText.Text = String.Format(HttpContext.GetGlobalResourceObject("Text", "ProfessionQuota", New CultureInfo(CultureLanguage.English)) _
                                                                  , "<br>" & HttpContext.GetGlobalResourceObject("Text", udtVoucherQuota.ProfCode, New CultureInfo(CultureLanguage.English)))

                Me.lblDisplayAvailableQuotaText_Chi.Text = String.Format(HttpContext.GetGlobalResourceObject("Text", "ProfessionQuota", New CultureInfo(CultureLanguage.TradChinese)) _
                                                  , HttpContext.GetGlobalResourceObject("Text", udtVoucherQuota.ProfCode, New CultureInfo(CultureLanguage.TradChinese)))

                Me.lblDisplayAvailableQuota.Text = (New Common.Format.Formatter).formatMoney(intAvailableQuota.ToString, True)

                Me.lblDisplayAvailableQuotaUpto.Text = String.Format(HttpContext.GetGlobalResourceObject("Text", "Upto", New CultureInfo(CultureLanguage.English)) _
                                                                  , udtVoucherQuota.PeriodEndDtm.ToString("dd-MM-yyyy"))

                Me.lblDisplayAvailableQuotaUpto_Chi.Text = String.Format(HttpContext.GetGlobalResourceObject("Text", "Upto", New CultureInfo(CultureLanguage.TradChinese)) _
                                                                      , udtVoucherQuota.PeriodEndDtm.ToString("dd-MM-yyyy"))

                Me.lblMaximumVoucherAmountText.Text = String.Format(HttpContext.GetGlobalResourceObject("Text", "MaximumVoucherAmount", New CultureInfo(CultureLanguage.English)), _
                                                                    HttpContext.GetGlobalResourceObject("Text", udtVoucherQuota.ProfCode, New CultureInfo(CultureLanguage.English)))

                Me.lblMaximumVoucherAmountText_Chi.Text = String.Format(HttpContext.GetGlobalResourceObject("Text", "MaximumVoucherAmount", New CultureInfo(CultureLanguage.TradChinese)), _
                                                                        HttpContext.GetGlobalResourceObject("Text", udtVoucherQuota.ProfCode, New CultureInfo(CultureLanguage.TradChinese)))

                Me.lblMaximumVoucherAmountNote.Text = String.Format(HttpContext.GetGlobalResourceObject("Text", "MaximumVoucherAmountNote", New CultureInfo(CultureLanguage.English)), _
                                                                    HttpContext.GetGlobalResourceObject("Text", udtVoucherQuota.ProfCode, New CultureInfo(CultureLanguage.English)))

                Me.lblMaximumVoucherAmountNote_Chi.Text = String.Format(HttpContext.GetGlobalResourceObject("Text", "MaximumVoucherAmountNote", New CultureInfo(CultureLanguage.TradChinese)), _
                                                                        HttpContext.GetGlobalResourceObject("Text", udtVoucherQuota.ProfCode, New CultureInfo(CultureLanguage.TradChinese)))

                Dim intMaxUsableBalance As Integer = udtVoucherInfo.GetMaxUsableBalance(udtVoucherQuota.ProfCode)

                Me.lblMaximumVoucherAmount.Text = udtFormatter.formatMoney(IIf(intMaxUsableBalance > 0, intMaxUsableBalance, 0), True)

                If UCase(CStr(Session("language"))).Equals("ZH-TW") Then
                    lblDisplayAvailableQuotaText.Visible = False
                    lblDisplayAvailableQuotaUpto.Visible = False

                    lblDisplayAvailableQuotaText_Chi.Visible = True
                    lblDisplayAvailableQuotaUpto_Chi.Visible = True

                    lblMaximumVoucherAmountText.Visible = False
                    lblMaximumVoucherAmountNote.Visible = False

                    lblMaximumVoucherAmountText_Chi.Visible = True
                    lblMaximumVoucherAmountNote_Chi.Visible = True
                Else
                    lblDisplayAvailableQuotaText.Visible = True
                    lblDisplayAvailableQuotaUpto.Visible = True

                    lblDisplayAvailableQuotaText_Chi.Visible = False
                    lblDisplayAvailableQuotaUpto_Chi.Visible = False

                    lblMaximumVoucherAmountText.Visible = True
                    lblMaximumVoucherAmountNote.Visible = True

                    lblMaximumVoucherAmountText_Chi.Visible = False
                    lblMaximumVoucherAmountNote_Chi.Visible = False
                End If

                trMaximumVoucherAmount.Style.Item("display") = ""
                tblQuotaReference.Style.Item("display") = ""
            Else
                trMaximumVoucherAmount.Style.Item("display") = "none"
                tblQuotaReference.Style.Item("display") = "none"
            End If


            MultiViewEnquiry.ActiveViewIndex = ViewIndex.Result
            lnkTextOnlyVesion.Visible = False

            ' Result View
            lblDisplayHKID.Text = udtFormatter.FormatDocIdentityNoForDisplay(udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.IdentityNum, True)
            lblDisplayDOB.Text = udtFormatter.formatDOB(udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.DOB, _
                                                        udtEHSPersonalInformation.ExactDOB, String.Empty, udtEHSPersonalInformation.ECAge, _
                                                        udtEHSPersonalInformation.ECDateOfRegistration, "")
            lblDisplayDOB_Chi.Text = udtFormatter.formatDOB(udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.DOB, _
                                                        udtEHSPersonalInformation.ExactDOB, "ZH-TW", udtEHSPersonalInformation.ECAge, _
                                                        udtEHSPersonalInformation.ECDateOfRegistration, "")

            If CStr(Session("language")).ToUpper = "ZH-TW" Then
                lblDisplayDOB.Visible = False
                lblDisplayDOB_Chi.Visible = True
            Else
                lblDisplayDOB_Chi.Visible = False
                lblDisplayDOB.Visible = True
            End If


            Dim udtSubsidizeFeeModel As SubsidizeFeeModel = udtSubsidizeGroupC.SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVoucher)

            Dim dblVoucherValue As Double = intAvailableVoucher * udtSubsidizeFeeModel.SubsidizeFee.Value

            lblDisplayVoucherValue.Text = udtFormatter.formatMoney(dblVoucherValue.ToString, True)

            ' CRE18-008 Add note to HCVR [Start][Koala]
            ' Handle voucher note
            If strDocCode = DocTypeCode.HKIC Then
                Me.lblNoteText.Visible = True
                Me.lblNoteValue.Visible = True
                Me.imgHKIDSymbolSample.Visible = True
                Me.imgHKIDSymbolSample.Style.Remove("display")
            Else
                Me.lblNoteText.Visible = False
                Me.lblNoteValue.Visible = False
                Me.imgHKIDSymbolSample.Visible = False
                Me.imgHKIDSymbolSample.Style.Add("display", "none")
            End If
            ' CRE18-008 Add note to HCVR [End][Koala]

            lblAnnualAllotmentText.Text = HttpContext.GetGlobalResourceObject("Text", "AnnualVoucherAllotment", New CultureInfo(CultureLanguage.English))
            lblAnnualAllotmentText_Chi.Text = HttpContext.GetGlobalResourceObject("Text", "AnnualVoucherAllotment", New CultureInfo(CultureLanguage.TradChinese))

            If CStr(Session("language")).ToUpper = "ZH-TW" Then
                lblAnnualAllotmentText.Visible = False
                lblAnnualAllotmentText_Chi.Visible = True

            Else
                lblAnnualAllotmentText.Visible = True
                lblAnnualAllotmentText_Chi.Visible = False

            End If

            lblAnnualAllotment.Text = udtFormatter.formatMoney(udtVoucherInfo.GetNextDepositAmount.ToString, True)

            lblForfeitVoucherText.Text = HttpContext.GetGlobalResourceObject("Text", "ForfeitVoucher", New CultureInfo(CultureLanguage.English))
            lblForfeitVoucherText_Chi.Text = HttpContext.GetGlobalResourceObject("Text", "ForfeitVoucher", New CultureInfo(CultureLanguage.TradChinese))

            lblForfeitVoucherNote.Text = String.Format(HttpContext.GetGlobalResourceObject("Text", "ForfeitVoucherNote", New CultureInfo(CultureLanguage.English)), _
                                                       udtFormatter.formatMoney(udtVoucherInfo.GetNextCappingAmount.ToString, True))
            lblForfeitVoucherNote_Chi.Text = String.Format(HttpContext.GetGlobalResourceObject("Text", "ForfeitVoucherNote", New CultureInfo(CultureLanguage.TradChinese)), _
                                                       udtFormatter.formatMoney(udtVoucherInfo.GetNextCappingAmount.ToString, True))

            If CStr(Session("language")).ToUpper = "ZH-TW" Then
                lblForfeitVoucherText.Visible = False
                lblForfeitVoucherText_Chi.Visible = True

                lblForfeitVoucherNote.Visible = False
                lblForfeitVoucherNote_Chi.Visible = True
            Else
                lblForfeitVoucherText.Visible = True
                lblForfeitVoucherText_Chi.Visible = False

                lblForfeitVoucherNote.Visible = True
                lblForfeitVoucherNote_Chi.Visible = False
            End If

            If udtVoucherInfo.GetNextForfeitAmount > 0 Then
                lblForfeitVoucher.Text = udtFormatter.formatMoney(udtVoucherInfo.GetNextForfeitAmount.ToString, True)
                lblForfeitVoucher.Style.Add("color", "red")
            Else
                lblForfeitVoucher.Text = udtFormatter.formatMoney("0", True)
                lblForfeitVoucher.Style.Remove("color")
            End If
            ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search successful", objAuditLogInfo)

        Else

            If IsMatchDOB AndAlso udtEHSAccount.RecordStatus = VRAcctStatus.Terminated Then
                udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
                udcInfoMessageBox.BuildMessageBox()

                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("udtEHSAccount.RecordStatus = VRAcctStatus.Terminated: EHS Account Terminated"))
                udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search complete. No record found", objAuditLogInfo)

            Else
                If IsMatchDOB AndAlso udtEHSAccount.PublicEnquiryStatus <> VRAcctEnquiryStatus.Available Then
                    udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
                    udcInfoMessageBox.BuildMessageBox()

                    udtVoucherAccEnquiryFailRecordBLL.UpdateFailCount(udtEHSAccount.VoucherAccID)

                    udtAuditLogEntry.AddDescripton("EHS Account Public Enquiry Status", udtEHSAccount.PublicEnquiryStatus)
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("udtEHSAccount.PublicEnquiryStatus <> VRAcctEnquiryStatus.Available: EHS Account is not available to enquire"))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search complete. No record found", objAuditLogInfo)

                Else
                    udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
                    udcInfoMessageBox.BuildMessageBox()

                    udtVoucherAccEnquiryFailRecordBLL.UpdateFailCount(udtEHSAccount.VoucherAccID)

                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("IsMatchDOB = False: The DOB does not match the supplied Identity No."))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search complete. No record found", objAuditLogInfo)

                End If
            End If

        End If

    End Sub

    ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Protected Sub ibtnReminderWindowsVersion_OK_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim udtAuditLogEntry As New AuditLogEntry(Me.FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00006, "Reminder - Obsolete Windows Version - OK Click")

        Me.ModalPopupExtenderReminderWindowsVersion.Hide()
    End Sub
    ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

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
End Class