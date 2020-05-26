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
Imports Common.Component.VoucherInfo

Namespace Text
    Partial Public Class main
        Inherits TextOnlyBasePage

        ' FunctionCode = FunctCode.FUNT030102

#Region "Private Classes"

        Private Class ViewIndex
            Public Const Search As Integer = 0
            Public Const Result As Integer = 1
            Public Const ReminderObsoleteOS As Integer = 2
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

            FunctionCode = FunctCode.FUNT030102

            Dim udtAuditLogRedirect As New AuditLogEntry(FunctionCode)
            udtAuditLogRedirect.WriteLog(LogID.LOG00010, "Voucher Balance Enquiry load (Obsoleted)")

            Dim strRedirectLink As String = ConfigurationManager.AppSettings("RedirectLinkChi")
            HttpContext.Current.Response.Redirect(strRedirectLink)

            Return

            udtFormatter = New Formatter

            Dim strlink As String = String.Empty
            If Not HttpContext.Current.Request.CurrentExecutionFilePath.Equals(HttpContext.Current.Request.Path) Then
                If HttpContext.Current.Session("language") Is Nothing Then
                    strlink = "~/text/en/invalidlink.aspx"
                Else
                    If HttpContext.Current.Session("language") = "zh-tw" Then
                        strlink = "~/text/zh/invalidlink.aspx"
                    Else
                        strlink = "~/text/en/invalidlink.aspx"
                    End If
                End If
                Response.Redirect(strlink)
            End If

            If Not IsPostBack Then
                FunctionCode = FunctCode.FUNT030102
                Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode)
                udtAuditLogEntry.WriteLog(LogID.LOG00010, "Voucher Balance Enquiry load (Obsoleted)")

                Me.ResetAlertLabel()

                pnlHKIDDOB.Visible = True
                pnlECDOB.Visible = False

                Me.rboDOB.Checked = True
                Me.rboECAge.Checked = False

                txtPatsDOB.ReadOnly = False
                txtPatsDOB.BackColor = Nothing

                Me.txtPatsAge.Text = String.Empty
                Me.txtPatsAge.ReadOnly = True
                Me.txtPatsAge.BackColor = Drawing.Color.WhiteSmoke

                Me.txtRegisterOnDay.Text = String.Empty
                Me.txtRegisterOnDay.ReadOnly = True
                Me.txtRegisterOnDay.BackColor = Drawing.Color.WhiteSmoke

                ddlRegisterOnMonth.SelectedIndex = 0
                ddlRegisterOnMonth.Enabled = False

                Me.txtRegisterOnYear.Text = String.Empty
                Me.txtRegisterOnYear.ReadOnly = True
                Me.txtRegisterOnYear.BackColor = Drawing.Color.WhiteSmoke

                Me.txtRegisterOnDayChi.Text = String.Empty
                Me.txtRegisterOnDayChi.ReadOnly = True
                Me.txtRegisterOnDayChi.BackColor = Drawing.Color.WhiteSmoke

                ddlRegisterOnMonthChi.SelectedIndex = 0
                ddlRegisterOnMonthChi.Enabled = False

                Me.txtRegisterOnYearChi.Text = String.Empty
                Me.txtRegisterOnYearChi.ReadOnly = True
                Me.txtRegisterOnYearChi.BackColor = Drawing.Color.WhiteSmoke

                If UCase(CStr(Session("language"))).Equals("ZH-TW") Then
                    pnlEngECAge.Visible = False
                    pnlChiECAge.Visible = True
                Else
                    pnlEngECAge.Visible = True
                    pnlChiECAge.Visible = False
                End If

                lblPatSearchDOBHint.Text = Me.GetGlobalResourceObject("Text", "ECDOBHint") + " " + Me.GetGlobalResourceObject("Text", "ECDOBHint2")
            End If

            SetControlFocus(Me.txtPatsHKID)

            SetLangage()

            Me.PageTitle.Text = Me.GetGlobalResourceObject("Title", "VoucherBalanceEnquiry")
            If ViewState("CaptchaControlAlert") = "1" Then
                Me.CaptchaControl.Text = Me.GetGlobalResourceObject("Text", "TextOnlyCaptcha") & "&nbsp;<span style='color: red'>*</span>"
            Else
                Me.CaptchaControl.Text = Me.GetGlobalResourceObject("Text", "TextOnlyCaptcha")
            End If

            If Me.rboHKIDType.SelectedValue.Trim.Equals("Y") Then
                pnlHKIDDOB.Visible = True
                pnlECDOB.Visible = False

                lblPatSearchDOBHint.Text = Me.GetGlobalResourceObject("Text", "ECDOBHint") + " " + Me.GetGlobalResourceObject("Text", "ECDOBHint2")
                lblPatsDOBText.Text = Me.GetGlobalResourceObject("Text", "DOBLongForm")
            Else
                pnlHKIDDOB.Visible = False
                pnlECDOB.Visible = True
                lblPatsDOBText.Text = Me.GetGlobalResourceObject("Text", "DOBYOB")

                If Me.rboDOB.Checked Then
                    lblPatSearchDOBHint.Text = Me.GetGlobalResourceObject("Text", "ECDOBHint") + " " + Me.GetGlobalResourceObject("Text", "ECDOBHint2")
                ElseIf Me.rboECAge.Checked Then
                    lblPatSearchDOBHint.Text = Me.GetGlobalResourceObject("Text", "ECDORegisterAgeHint")
                End If
            End If

            ' Disallow public using WinXP
            Dim enumObsoleteOS As ObsoletedOSHandler.Result = Nothing

            ObsoletedOSHandler.HandleObsoleteOS(CommonSessionHandler.OS, Nothing, ObsoletedOSHandler.Version.TextOnly, _
                                                Me.FunctionCode, LogID.LOG00005, Nothing, enumObsoleteOS)

            If enumObsoleteOS = ObsoletedOSHandler.Result.WARNING Then
                MultiViewEnquiry.ActiveViewIndex = ViewIndex.ReminderObsoleteOS
            End If


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

        Private Sub ReRenderPage()

            If UCase(CStr(Session("language"))).Equals("ZH-TW") Then
                pnlEngECAge.Visible = False
                pnlChiECAge.Visible = True
                txtRegisterOnDayChi.Text = txtRegisterOnDay.Text.Trim
                txtRegisterOnYearChi.Text = txtRegisterOnYear.Text.Trim
                ddlRegisterOnMonthChi.SelectedValue = ddlRegisterOnMonth.SelectedValue.Trim

                lblDisplayDOB.Visible = False
                lblDisplayDOB_Chi.Visible = True
            Else
                pnlEngECAge.Visible = True
                pnlChiECAge.Visible = False
                txtRegisterOnDay.Text = txtRegisterOnDayChi.Text.Trim
                txtRegisterOnYear.Text = txtRegisterOnYearChi.Text.Trim
                ddlRegisterOnMonth.SelectedValue = ddlRegisterOnMonthChi.SelectedValue.Trim

                lblDisplayDOB_Chi.Visible = False
                lblDisplayDOB.Visible = True
            End If

            lblECYear.Text = Me.GetGlobalResourceObject("Text", "Year")
            lblECMonth.Text = Me.GetGlobalResourceObject("Text", "Month")
            lblECDay.Text = Me.GetGlobalResourceObject("Text", "Day")

            If Me.rboHKIDType.SelectedValue.Trim.Equals("Y") Then
                lblPatSearchDOBHint.Text = Me.GetGlobalResourceObject("Text", "ECDOBHint") + " " + Me.GetGlobalResourceObject("Text", "ECDOBHint2")
            Else
                If Me.rboDOB.Checked Then
                    lblPatSearchDOBHint.Text = Me.GetGlobalResourceObject("Text", "ECDOBHint") + " " + Me.GetGlobalResourceObject("Text", "ECDOBHint2")
                ElseIf Me.rboECAge.Checked Then
                    lblPatSearchDOBHint.Text = Me.GetGlobalResourceObject("Text", "ECDORegisterAgeHint")
                End If
            End If

            ' Disallow public using WinXP
            ' ----------------------------------------------------------
            Me.lblReminderObsoleteOSContent.Text = Me.GetGlobalResourceObject("Text", "ReminderWindowsVersion")

            Me.btnReminderObsoleteOSOK.Text = Me.GetGlobalResourceObject("AlternateText", "OKBtn")

            ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
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
            ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

        End Sub

        Private Sub SetLangage()
            Dim selectedValue As String

            selectedValue = Session("language")

            Select Case selectedValue
                Case English
                    lnkbtnEnglish.Enabled = False
                    lnkbtnTradChinese.Enabled = True

                Case TradChinese
                    lnkbtnEnglish.Enabled = True
                    lnkbtnTradChinese.Enabled = False

                Case Else
                    lnkbtnEnglish.Enabled = True
                    lnkbtnTradChinese.Enabled = False

            End Select
        End Sub

#End Region

        Private Sub ResetAlertLabel()
            lblPatsHKIDAlert.Visible = False
            lblPatsDOBAlert.Visible = False
            lblHKIDDOBAlert.Visible = False
            lblPatsECAgeAlert.Visible = False
            lblCaptchaControlAlert.Visible = False
        End Sub

        '

        Private Sub btnSearchPat_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchPat.Click
            Dim udtDocTypeBLL As New DocType.DocTypeBLL

            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode)
            udtAuditLogEntry.AddDescripton("Identity No", txtPatsHKID.Text.Trim)
            udtAuditLogEntry.AddDescripton("Is HKID", rboHKIDType.SelectedValue.Trim)
            If rboHKIDType.SelectedValue = "Y" Then
                udtAuditLogEntry.AddDescripton("DOB", txtHKIDDOB.Text.Trim)
            Else
                udtAuditLogEntry.AddDescripton("DOB", txtPatsDOB.Text.Trim)
            End If

            udtAuditLogEntry.AddDescripton("Age", txtPatsAge.Text.Trim)

            udcTextOnlyInfoMessageBox.Visible = False
            udcTextOnlyMessageBox.Visible = False

            If CStr(Session("language")).ToUpper = "ZH-TW" Then
                udtAuditLogEntry.AddDescripton("DOR Year", txtRegisterOnYearChi.Text.Trim)
                udtAuditLogEntry.AddDescripton("DOR Month", ddlRegisterOnMonthChi.SelectedValue)
                udtAuditLogEntry.AddDescripton("DOR Day", txtRegisterOnDayChi.Text.Trim)
            Else
                udtAuditLogEntry.AddDescripton("DOR Year", txtRegisterOnYear.Text.Trim)
                udtAuditLogEntry.AddDescripton("DOR Month", ddlRegisterOnMonth.SelectedValue)
                udtAuditLogEntry.AddDescripton("DOR Day", txtRegisterOnDay.Text.Trim)
            End If

            Dim objAuditLogInfo As AuditLogInfo = New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, IIf(rboHKIDType.SelectedValue = "Y", DocTypeCode.HKIC, DocTypeCode.EC), udtFormatter.formatHKIDInternal(txtPatsHKID.Text))
            udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Search", objAuditLogInfo)

            ResetAlertLabel()
            Dim strDocCode As String = IIf(rboHKIDType.SelectedValue = "Y", DocTypeCode.HKIC, DocTypeCode.EC) ' CRE11-007

            Dim udtSystemMessage As SystemMessage = Nothing

            If udtValidator.IsEmpty(txtPatsHKID.Text.Trim) Then
                udcTextOnlyMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00001)
                lblPatsHKIDAlert.Visible = True

            Else
                udtSystemMessage = udtValidator.chkHKID(udtFormatter.formatHKIDInternal(txtPatsHKID.Text))

                ' -------------------------------------------------------------------------------
                ' Check Active Death Record
                ' If dead, return "(document id name) is invalid"
                ' -------------------------------------------------------------------------------
                If udtSystemMessage Is Nothing Then
                    If udtDocTypeBLL.getDocTypeByAvailable(DocType.DocTypeBLL.EnumAvailable.DeathRecordAvailable).Filter(strDocCode) IsNot Nothing Then
                        If (New eHealthAccountDeathRecord.eHealthAccountDeathRecordBLL).GetDeathRecordEntry(txtPatsHKID.Text).IsDead() Then
                            udcTextOnlyInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
                            udcTextOnlyInfoMessageBox.BuildMessageBox()

                            Return
                        End If
                    End If
                End If

                If Not udtSystemMessage Is Nothing Then
                    udcTextOnlyMessageBox.AddMessage(udtSystemMessage)
                    lblPatsHKIDAlert.Visible = True
                End If
            End If

            If rboHKIDType.SelectedValue = "Y" Then
                ' Hong Kong Identity Card
                udtSystemMessage = udtValidator.chkDOB(DocType.DocTypeModel.DocTypeCode.HKIC, udtFormatter.formatInputDate(txtHKIDDOB.Text.Trim))


                If Not IsNothing(udtSystemMessage) Then
                    udcTextOnlyMessageBox.AddMessage(udtSystemMessage)
                    lblHKIDDOBAlert.Visible = True
                End If

            Else
                ' Certificate of Exemption
                If rboDOB.Checked Then

                    udtSystemMessage = udtValidator.chkDOB(DocType.DocTypeModel.DocTypeCode.EC, udtFormatter.formatInputDate(txtPatsDOB.Text.Trim))

                    If Not IsNothing(udtSystemMessage) Then
                        udcTextOnlyMessageBox.AddMessage(udtSystemMessage)
                        lblPatsDOBAlert.Visible = True
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
                        udcTextOnlyMessageBox.AddMessage(udtSystemMessage)
                        lblPatsECAgeAlert.Visible = True

                    Else
                        udtSystemMessage = udtValidator.chkECDOAge(txtRegisterOnDay.Text.Trim, ddlRegisterOnMonth.SelectedValue.Trim, txtRegisterOnYear.Text.Trim)

                        If Not IsNothing(udtSystemMessage) Then
                            udcTextOnlyMessageBox.AddMessage(udtSystemMessage)
                            lblPatsECAgeAlert.Visible = True
                        Else
                            udtSystemMessage = udtValidator.chkECAgeAndDOAge(txtPatsAge.Text.Trim, txtRegisterOnDay.Text.Trim, ddlRegisterOnMonth.SelectedValue.Trim, Me.txtRegisterOnYear.Text.Trim)
                            If Not IsNothing(udtSystemMessage) Then
                                udcTextOnlyMessageBox.AddMessage(udtSystemMessage)
                                lblPatsECAgeAlert.Visible = True
                            End If
                        End If

                    End If
                End If
            End If

            ' Captcha checking
            If Not CaptchaControl.UserValidated Then
                udcTextOnlyMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, CaptchaControl.ErrorMessage)

                hfCaptchaControlAlert.Value = "1"
                ViewState("CaptchaControlAlert") = "1"
                CaptchaControl.Text = Me.GetGlobalResourceObject("Text", "TextOnlyCaptcha") & "&nbsp;<span style='color: red'>*</span>"
            End If

            ' Get the EHS Account
            Dim strIdentityNo As String = udtFormatter.formatHKIDInternal(txtPatsHKID.Text)

            Dim udtEHSAccount As EHSAccountModel = udtEHSAccountBLL.LoadEHSAccountByIdentity(strIdentityNo, strDocCode)

            If udcTextOnlyMessageBox.GetCodeTable.Rows.Count <> 0 Then
                If Not IsNothing(udtEHSAccount) Then
                    udtVoucherAccEnquiryFailRecordBLL.UpdateFailCount(udtEHSAccount.VoucherAccID)
                End If

                udcTextOnlyMessageBox.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00004, "Search fail", objAuditLogInfo)

                Return
            End If

            If IsNothing(udtEHSAccount) Then
                udcTextOnlyInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
                udcTextOnlyInfoMessageBox.BuildMessageBox()
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("EHSAccountModel Is Nothing: Could not find EHS Account with Identity No. {0} and Document Code {1}", strIdentityNo, strDocCode))
                udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search complete. No record found", objAuditLogInfo)

                Return
            End If

            Dim strDOB As String = String.Empty
            Dim strAge As String = String.Empty
            Dim dtmDOR As Date = Nothing

            Dim udtEHSPersonalInformation As EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(strDocCode)

            If IsNothing(udtEHSPersonalInformation) Then
                udcTextOnlyInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
                udcTextOnlyInfoMessageBox.BuildMessageBox()
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("EHSPersonalInformationModel Is Nothing: Could not find Personal Information with Document Type {0} for Voucher Account {1}", strDocCode, udtEHSAccount.VoucherAccID))
                udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search complete. No record found", objAuditLogInfo)

                Return
            End If

            ' Check if the patient is eligible for HCVS (Aged 70 or above)
            Dim udtClaimRuleBLL As New Common.Component.ClaimRules.ClaimRulesBLL
            Dim udtResult As Common.Component.ClaimRules.ClaimRulesBLL.EligibleResult

            udtResult = udtClaimRuleBLL.CheckEligibilityFromHCVR(HCVS, udtEHSPersonalInformation, udtGeneralFunction.GetSystemDateTime().Date)

            If Not udtResult.IsEligible Then
                udcTextOnlyInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
                udcTextOnlyInfoMessageBox.BuildMessageBox()
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Not udtResult.IsEligible: EHSPersonalInformationModel is not eligible for Document Type {0} and Voucher Account {1}", strDocCode, udtEHSAccount.VoucherAccID))
                udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search complete. No record found", objAuditLogInfo)

                Return
            End If

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
                Dim udtVoucherInfo As New VoucherInfoModel(VoucherInfoModel.AvailableVoucher.Include, VoucherInfoModel.AvailableQuota.Include)

                udtVoucherInfo.GetInfo(udtSchemeC, udtEHSPersonalInformation)

                udtVoucherInfo.FunctionCode = FunctionCode

                Dim intAvailableVoucher As Integer = udtVoucherInfo.GetAvailableVoucher()

                If intAvailableVoucher >= 0 Then
                    ' Nothing to do
                Else
                    ' Handling negative number of available voucher
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("AvailableVoucher:" + intAvailableVoucher.ToString()))
                    Me.udcTextOnlyInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00033)
                    Me.udcTextOnlyInfoMessageBox.BuildMessageBox()

                    intAvailableVoucher = 0
                End If

                Dim udtVoucherQuota As New VoucherQuotaModel
                Dim udtVoucherQuotaList As VoucherQuotaModelCollection = udtVoucherInfo.VoucherQuotalist

                If udtVoucherQuotaList.Count > 0 Then
                    ' Display first record
                    udtVoucherQuota = udtVoucherQuotaList.Item(0)

                    Dim intAvailableQuota As Integer = IIf(udtVoucherQuota.AvailableQuota > 0, udtVoucherQuota.AvailableQuota, 0)
                    '
                    lblDisplayAvailableQuotaText.Text = String.Format(HttpContext.GetGlobalResourceObject("Text", "ProfessionQuota", New CultureInfo(CultureLanguage.English)) _
                                                                      , HttpContext.GetGlobalResourceObject("Text", udtVoucherQuota.ProfCode, New CultureInfo(CultureLanguage.English)))

                    lblDisplayAvailableQuotaText_Chi.Text = String.Format(HttpContext.GetGlobalResourceObject("Text", "ProfessionQuota", New CultureInfo(CultureLanguage.TradChinese)) _
                                                      , HttpContext.GetGlobalResourceObject("Text", udtVoucherQuota.ProfCode, New CultureInfo(CultureLanguage.TradChinese)))


                    lblDisplayAvailableQuota.Text = (New Common.Format.Formatter).formatMoney(intAvailableQuota.ToString, True)

                    lblDisplayAvailableQuotaUpto.Text = String.Format(HttpContext.GetGlobalResourceObject("Text", "Upto", New CultureInfo(CultureLanguage.English)) _
                                                                      , udtVoucherQuota.PeriodEndDtm.ToString("dd-MM-yyyy"))

                    lblDisplayAvailableQuotaUpto_Chi.Text = String.Format(HttpContext.GetGlobalResourceObject("Text", "Upto", New CultureInfo(CultureLanguage.TradChinese)) _
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
                hlFullVersion.Visible = False

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

                ResetTextBoxValue()

                If Me.Request.Browser.IsMobileDevice Then

                    btnComplete.Visible = False

                Else

                    btnComplete.Visible = True

                End If

                ' CRE18-008 Add note to HCVR [Start][Koala]
                ' Handle voucher note
                If strDocCode = DocTypeCode.HKIC Then
                    Me.lblNoteText.Visible = True
                    Me.lblNoteValue.Visible = True
                Else
                    Me.lblNoteText.Visible = False
                    Me.lblNoteValue.Visible = False
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

                    udcTextOnlyInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
                    udcTextOnlyInfoMessageBox.BuildMessageBox()

                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("udtEHSAccount.RecordStatus = VRAcctStatus.Terminated: EHS Account Terminated"))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search complete. No record found", objAuditLogInfo)

                Else
                    If IsMatchDOB AndAlso udtEHSAccount.PublicEnquiryStatus <> VRAcctEnquiryStatus.Available Then
                        udcTextOnlyInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
                        udcTextOnlyInfoMessageBox.BuildMessageBox()

                        udtVoucherAccEnquiryFailRecordBLL.UpdateFailCount(udtEHSAccount.VoucherAccID)

                        udtAuditLogEntry.AddDescripton("EHS Account Public Enquiry Status", udtEHSAccount.PublicEnquiryStatus)
                        udtAuditLogEntry.AddDescripton("StackTrace", String.Format("udtEHSAccount.PublicEnquiryStatus <> VRAcctEnquiryStatus.Available: EHS Account is not available to enquire"))
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search complete. No record found", objAuditLogInfo)

                    Else
                        udcTextOnlyInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
                        udcTextOnlyInfoMessageBox.BuildMessageBox()

                        udtVoucherAccEnquiryFailRecordBLL.UpdateFailCount(udtEHSAccount.VoucherAccID)

                        udtAuditLogEntry.AddDescripton("StackTrace", String.Format("IsMatchDOB = False: The DOB does not match the supplied Identity No."))
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search complete. No record found", objAuditLogInfo)
                    End If
                End If

            End If

        End Sub

        '

        Private Sub ResetTextBoxValue()
            Me.txtPatsHKID.Text = ""
            Me.txtPatsDOB.Text = ""
            txtPatsAge.Text = String.Empty
            txtRegisterOnDay.Text = String.Empty
            ddlRegisterOnMonth.SelectedIndex = 0
            txtRegisterOnYear.Text = String.Empty
            txtRegisterOnYearChi.Text = String.Empty
            ddlRegisterOnMonthChi.SelectedIndex = 0
            txtRegisterOnDayChi.Text = String.Empty
        End Sub

        Protected Sub rboDOB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rboDOB.CheckedChanged
            If rboDOB.Checked Then
                Me.rboECAge.Checked = False

                txtPatsDOB.ReadOnly = False
                txtPatsDOB.BackColor = Nothing

                Me.txtPatsAge.Text = String.Empty
                Me.txtPatsAge.ReadOnly = True
                Me.txtPatsAge.BackColor = Drawing.Color.WhiteSmoke

                Me.txtRegisterOnDay.Text = String.Empty
                Me.txtRegisterOnDay.ReadOnly = True
                Me.txtRegisterOnDay.BackColor = Drawing.Color.WhiteSmoke

                ddlRegisterOnMonth.SelectedIndex = 0
                ddlRegisterOnMonth.Enabled = False

                Me.txtRegisterOnYear.Text = String.Empty
                Me.txtRegisterOnYear.ReadOnly = True
                Me.txtRegisterOnYear.BackColor = Drawing.Color.WhiteSmoke

                Me.txtRegisterOnDayChi.Text = String.Empty
                Me.txtRegisterOnDayChi.ReadOnly = True
                Me.txtRegisterOnDayChi.BackColor = Drawing.Color.WhiteSmoke

                ddlRegisterOnMonthChi.SelectedIndex = 0
                ddlRegisterOnMonthChi.Enabled = False

                Me.txtRegisterOnYearChi.Text = String.Empty
                Me.txtRegisterOnYearChi.ReadOnly = True
                Me.txtRegisterOnYearChi.BackColor = Drawing.Color.WhiteSmoke

                lblPatSearchDOBHint.Text = Me.GetGlobalResourceObject("Text", "ECDOBHint") + " " + Me.GetGlobalResourceObject("Text", "ECDOBHint2")
            Else
                txtPatsDOB.Text = String.Empty
                txtPatsDOB.ReadOnly = True
                txtPatsDOB.BackColor = Drawing.Color.WhiteSmoke
            End If
        End Sub

        Protected Sub rboECAge_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rboECAge.CheckedChanged
            If rboECAge.Checked Then
                Me.rboDOB.Checked = False

                Me.txtPatsDOB.Text = String.Empty
                Me.txtPatsDOB.ReadOnly = True
                Me.txtPatsDOB.BackColor = Drawing.Color.WhiteSmoke

                Me.txtPatsAge.ReadOnly = False
                Me.txtPatsAge.BackColor = Nothing

                Me.txtRegisterOnDay.ReadOnly = False
                Me.txtRegisterOnDay.BackColor = Nothing

                ddlRegisterOnMonth.Enabled = True

                Me.txtRegisterOnYear.ReadOnly = False
                Me.txtRegisterOnYear.BackColor = Nothing

                Me.txtRegisterOnDayChi.ReadOnly = False
                Me.txtRegisterOnDayChi.BackColor = Nothing

                ddlRegisterOnMonthChi.Enabled = True

                Me.txtRegisterOnYearChi.ReadOnly = False
                Me.txtRegisterOnYearChi.BackColor = Nothing

                lblPatSearchDOBHint.Text = Me.GetGlobalResourceObject("Text", "ECDORegisterAgeHint")
            Else
                Me.txtRegisterOnDay.Text = String.Empty
                Me.txtRegisterOnDay.ReadOnly = True
                Me.txtRegisterOnDay.BackColor = Drawing.Color.WhiteSmoke

                ddlRegisterOnMonth.SelectedIndex = 0
                ddlRegisterOnMonth.Enabled = False

                Me.txtRegisterOnYear.Text = String.Empty
                Me.txtRegisterOnYear.ReadOnly = True
                Me.txtRegisterOnYear.BackColor = Drawing.Color.WhiteSmoke

                Me.txtRegisterOnDayChi.Text = String.Empty
                Me.txtRegisterOnDayChi.ReadOnly = True
                Me.txtRegisterOnDayChi.BackColor = Drawing.Color.WhiteSmoke

                ddlRegisterOnMonthChi.SelectedIndex = 0
                ddlRegisterOnMonthChi.Enabled = False

                Me.txtRegisterOnYearChi.Text = String.Empty
                Me.txtRegisterOnYearChi.ReadOnly = True
                Me.txtRegisterOnYearChi.BackColor = Drawing.Color.WhiteSmoke

                lblPatSearchDOBHint.Text = Me.GetGlobalResourceObject("Text", "ECDOBHint") + " " + Me.GetGlobalResourceObject("Text", "ECDOBHint2")
            End If
        End Sub

        Private Sub btnComplete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnComplete.Click

            Me.Response.Redirect("~/text/thankyou.aspx")

        End Sub


        Private Sub btnObsoleteWindowsVersionOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReminderObsoleteOSOK.Click

            Dim udtAuditLogEntry As New AuditLogEntry(Me.FunctionCode)
            udtAuditLogEntry.WriteLog(LogID.LOG00006, "Reminder - Obsolete Windows Version - OK Click")

            MultiViewEnquiry.ActiveViewIndex = ViewIndex.Search
            ReRenderPage()
        End Sub


    End Class

End Namespace
