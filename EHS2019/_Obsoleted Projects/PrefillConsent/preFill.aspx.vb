Imports Common.ComFunction
Imports Common.Validation
Imports Common.ComObject
Imports System.Globalization
Imports System.Threading
'Imports Common.Component.VoucherRecipientAccount
Imports Common.DataAccess
'Imports VoucherBalanceEnquiry.Controller
Imports Common.Format
Imports Common.Component.VoucherScheme
Imports Common.Component
Imports Common.Component.Scheme
Imports Common.Component.DocType
Imports Common.Component.EHSAccount

Partial Public Class preFill
    Inherits BasePage

    Private udcGeneralF As New Common.ComFunction.GeneralFunction
    Private _udtEHSAccount As EHSAccountModel
    Private udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel
    Private formatter As Common.Format.Formatter = New Common.Format.Formatter()
    Private validator As Common.Validation.Validator = New Common.Validation.Validator
    Private GeneralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

    Private _strValidationFail As String = "ValidationFail"
    Private _systemMessage As SystemMessage
    Private _udtSessionHandler As BLL.SessionHandler = New BLL.SessionHandler
    Private bEmptyCCCode As Boolean


    Private Sub login_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        Dim udcGeneralFun = New Common.ComFunction.GeneralFunction()
        Me.basetag.Attributes("href") = udcGeneralFun.getPageBasePath()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            FunctionCode = Common.Component.FunctCode.FUNT050101

            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00004, "Pre-Fill Consent Page Load")

            If IsNothing(Session("PreFillMain")) Then
                Response.Redirect("~/main.aspx")
            End If

            'ibtnCompleteClose.Attributes.Add("OnClick", "javascript:window.opener='X';window.open('','_parent','');window.close(); return false;")
            'ibtnCompleteClose.Attributes.Add("OnClick", "javascript:closeWin();")
            mvPreFill.ActiveViewIndex = 0
            udcStep1DocumentTypeRadioButtonGroup.SelectedValue = String.Empty
            SetProcessCss(1)

            '_udtEHSAccount = New EHSAccountModel
        End If

        Dim SM As Common.ComObject.SystemMessage
        SM = New Common.ComObject.SystemMessage(FunctionCode, "Q", Common.Component.MsgCode.MSG00001)

        Me.lblMsg.Text = SM.GetMessage

        Select Case mvPreFill.ActiveViewIndex
            Case 0
                BuildDocType()

                setInputDocumentType()

            Case 1
                BuildDocType()

                setInputDocumentType()

                setReadOnlyDocumentType()

            Case 2
                lblCompleteSubmissionDate.Text = formatter.convertDateTime(_udtSessionHandler.PreFillSubmitTimeGetFormSession(FunctionCode))
                'setToDoLabel()

        End Select

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

        ibtnCompleteClose.OnClientClick = "showConfirm(this); return false;"

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

    Private Sub SetProcessCss(ByVal iStep As Integer)
        panStep1.CssClass = "unhighlightTimeline"
        panStep2.CssClass = "unhighlightTimeline"
        panStep3.CssClass = "unhighlightTimeline"

        Select Case iStep
            Case 1
                panStep1.CssClass = "highlightTimeline"
            Case 2
                panStep2.CssClass = "highlightTimeline"
            Case 3
                panStep3.CssClass = "highlightTimeline"
        End Select

    End Sub

    Private Sub ReRenderPage()

        lnkBtnPrivacyPolicy.Text = Me.GetGlobalResourceObject("Text", "PrivacyPolicy")
        lnkBtnDisclaimer.Text = Me.GetGlobalResourceObject("Text", "ImportantNotices")
        lnkBtnSysMaint.Text = Me.GetGlobalResourceObject("Text", "SysMaint")

    End Sub

    Private Sub main_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If IsPostBack Then
            'udcMsgBoxErr.Clear()

            Dim controlID As String = Page.Request.Params.Get(PostBackEventTarget)
            If controlID.Equals(SelectTradChinese) OrElse controlID.Equals(SelectEnglish) OrElse _
                controlID.Equals(_SelectTradChinese) OrElse controlID.Equals(_SelectEnglish) Then
                ReRenderPage()
            End If

            If mvPreFill.ActiveViewIndex = 0 Then
                If Me.udcMsgBoxErr.Visible Then
                    'validateInputPage()
                End If
            End If
        End If

        setToDoLabel()
    End Sub

    'For Fill Form

    Private Sub udcStep1DocumentTypeRadioButtonGroup_CheckedChanged(ByVal sender As Object, ByVal documentTypeRadioButtonGroupArgs As CustomControls.DocumentTypeRadioButtonGroup.DocumentTypeRadioButtonGroupArgs) Handles udcStep1DocumentTypeRadioButtonGroup.CheckedChanged
        Me.udcStep1b1InputDocumentType.ClearControl()
        setInputDocumentType()

        BuildDocType()

        'udcMsgBoxErr.Visible = False
        udcMsgBoxErr.Clear()
        _udtSessionHandler.PreFillChineseRemoveFromSession(FunctionCode)
        _udtSessionHandler.PreFillPressConfirmRemoveFromSession(FunctionCode)

        ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [Start][Winnie] Step 0
        _udtSessionHandler.CCCodeRemoveFromSession(FunctionCode)
        ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [End][Winnie] Step 0
    End Sub

    Private Sub BuildDocType()
        Dim udtDocTypeBLL As New DocTypeBLL()
        Dim udtDocTypeModelList As DocTypeModelCollection = udtDocTypeBLL.getAllDocType()
        Dim udtDocTypeModelNew As DocTypeModelCollection = New DocTypeModelCollection
        Dim udtSchemeDocTypeList As SchemeDocTypeModelCollection = udtDocTypeBLL.getSchemeDocTypeByScheme(SchemeClaimModel.CIVSS)

        For Each udtDocTypeModel As DocTypeModel In udtDocTypeModelList
            If udtDocTypeModel.DocCode.Trim.ToUpper() = DocTypeModel.DocTypeCode.HKBC Then
                udtDocTypeModelNew.Add(udtDocTypeModel)
                Exit For
            End If
        Next

        For Each udtDocTypeModel As DocTypeModel In udtDocTypeModelList
            If udtDocTypeModel.DocCode.Trim.ToUpper() <> DocTypeModel.DocTypeCode.HKBC Then
                For Each udtSchemeDocTypeModel As SchemeDocTypeModel In udtSchemeDocTypeList
                    If udtDocTypeModel.DocCode.Trim.ToUpper() = udtSchemeDocTypeModel.DocCode.Trim.ToUpper() Then
                        udtDocTypeModelNew.Add(udtDocTypeModel)
                        Exit For
                    End If
                Next
            End If

        Next

        udcStep1DocumentTypeRadioButtonGroup.Scheme = SchemeClaimModel.CIVSS
        udcStep1DocumentTypeRadioButtonGroup.ShowLegend = False
        'udcStep1DocumentTypeRadioButtonGroup.Build()
        udcStep1DocumentTypeRadioButtonGroup.BuildSpecifiedDocType(udtDocTypeModelNew)

    End Sub

    Private Sub setInputDocumentType()
        Me.udcStep1b1InputDocumentType.ClearControl()
        Me.udcStep1b1InputDocumentType.DocType = udcStep1DocumentTypeRadioButtonGroup.SelectedValue
        Me.udcStep1b1InputDocumentType.Mode = ucInputDocTypeBase.BuildMode.Creation
        Me.udcStep1b1InputDocumentType.EHSAccount = _udtEHSAccount
        Me.udcStep1b1InputDocumentType.FillValue = True
        Me.udcStep1b1InputDocumentType.Built()

        setInputDocText()
    End Sub

    Private Sub setInputDocText()
        Me.btnInputTips.Visible = False

        Select Case udcStep1DocumentTypeRadioButtonGroup.SelectedValue
            Case DocType.DocTypeModel.DocTypeCode.HKIC
                'input Tips
                Me.lblInputInfoText.Text = Me.GetGlobalResourceObject("Text", "EnterVRAInfo")
                Me.btnInputTips.Visible = True
                Me.btnInputTips.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "HelpBtn")
                Me.btnInputTips.AlternateText = Me.GetGlobalResourceObject("AlternateText", "HelpBtn")
                Me.btnInputTips.OnClientClick = "showHKIDHelp();return false;"

            Case DocType.DocTypeModel.DocTypeCode.EC
                'Input Tips 
                Me.lblInputInfoText.Text = Me.GetGlobalResourceObject("Text", "EnterVRAInfoEC")
            Case DocType.DocTypeModel.DocTypeCode.HKBC
                'Input Tips 
                Me.lblInputInfoText.Text = Me.GetGlobalResourceObject("Text", "EnterEHSAccountInfoHKBC")
                Me.btnInputTips.Visible = True
                Me.btnInputTips.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "HelpBtn")
                Me.btnInputTips.AlternateText = Me.GetGlobalResourceObject("AlternateText", "HelpBtn")
                Me.btnInputTips.OnClientClick = "showHKBCHelp();return false;"

            Case DocType.DocTypeModel.DocTypeCode.ADOPC
                'Input Tips 
                Me.lblInputInfoText.Text = Me.GetGlobalResourceObject("Text", "EnterVRAInfoADOPC")
                Me.btnInputTips.Visible = True
                Me.btnInputTips.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "HelpBtn")
                Me.btnInputTips.AlternateText = Me.GetGlobalResourceObject("AlternateText", "HelpBtn")
                Me.btnInputTips.OnClientClick = "showADOPCHelp();return false;"

            Case DocType.DocTypeModel.DocTypeCode.DI
                'Input Tips 
                Me.lblInputInfoText.Text = Me.GetGlobalResourceObject("Text", "EnterVRAInfoDI")
                Me.btnInputTips.Visible = True
                Me.btnInputTips.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "HelpBtn")
                Me.btnInputTips.AlternateText = Me.GetGlobalResourceObject("AlternateText", "HelpBtn")
                Me.btnInputTips.OnClientClick = "showDIHelp();return false;"

            Case DocType.DocTypeModel.DocTypeCode.ID235B
                'Input Tips 
                Me.lblInputInfoText.Text = Me.GetGlobalResourceObject("Text", "EnterVRAInfoID235B")
            Case DocType.DocTypeModel.DocTypeCode.REPMT
                'Input Tips 
                Me.lblInputInfoText.Text = Me.GetGlobalResourceObject("Text", "EnterVRAInfoREPMT")
                Me.btnInputTips.Visible = True
                Me.btnInputTips.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "HelpBtn")
                Me.btnInputTips.AlternateText = Me.GetGlobalResourceObject("AlternateText", "HelpBtn")
                Me.btnInputTips.OnClientClick = "showREPMTHelp();return false;"

            Case DocType.DocTypeModel.DocTypeCode.VISA
                'Input Tips 
                Me.lblInputInfoText.Text = Me.GetGlobalResourceObject("Text", "EnterVRAInfoVISA")
                Me.btnInputTips.Visible = True
                Me.btnInputTips.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "HelpBtn")
                Me.btnInputTips.AlternateText = Me.GetGlobalResourceObject("AlternateText", "HelpBtn")
                Me.btnInputTips.OnClientClick = "showVISAHelp();return false;"

        End Select
    End Sub

    Private Sub btnNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnNext.Click
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Doc Type", udcStep1DocumentTypeRadioButtonGroup.SelectedValue)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00005, "Step 1 Start", "")

        'Dim objAuditLogInfo As AuditLogInfo = New AuditLogInfo(Nothing, Nothing, _udtEHSAccount.AccountSourceString, _udtEHSAccount.VoucherAccID, _udtEHSAccount.EHSPersonalInformationList(0).DocCode, _udtEHSAccount.EHSPersonalInformationList(0).IdentityNum)
        udtAuditLogEntry.WriteLog(LogID.LOG00006, "Step 1 Validate Input Doc")

        logInputControl()

        _udtSessionHandler.PreFillPressConfirmSaveToSession(FunctionCode, "Y")

        If udcStep1DocumentTypeRadioButtonGroup.SelectedValue = DocTypeModel.DocTypeCode.HKIC Then
            Dim udcInputHKIC As ucInputHKID = Me.udcStep1b1InputDocumentType.GetHKICControl()
            udcInputHKIC.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

            If Me.NeedPopupChineseNameDialog() Then
                Me.bEmptyCCCode = False

                Me.udcStep1b1InputDocumentType_SelectChineseName_HKIC(udcInputHKIC, False, Nothing, Nothing)
                'isValid = False
                If Not Me.bEmptyCCCode Then
                    '_udtSessionHandler.PreFillPressConfirmRemoveFromSession(FunctionCode)
                    'udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Step 1 Input Validate Fail", "")
                    Exit Sub
                End If
            End If

            If udcInputHKIC.CCCodeIsEmpty() Then
                Me.udcChooseCCCode.Clean()
                _udtSessionHandler.CCCodeRemoveFromSession(FunctionCode)
            End If

        End If

        If Not validateInputPage() Then
            ' CRE11-004
            'udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Step 1 Input Validate Fail", "")
            _udtSessionHandler.PreFillPressConfirmRemoveFromSession(FunctionCode)
            Exit Sub
        End If
        '_udtSessionHandler.PreFillPressConfirmSaveToSession(FunctionCode, "")
        '_udtSessionHandler.PreFillChineseRemoveFromSession(FunctionCode)
        _udtSessionHandler.PreFillPressConfirmRemoveFromSession(FunctionCode)

        mvPreFill.ActiveViewIndex = 1


        udtAuditLogEntry.WriteLog(LogID.LOG00007, "Step 1 Set ReadOnly Doc")
        setReadOnlyDocumentType()

        SetProcessCss(2)

        _udtSessionHandler.EHSAccountSaveToSession(_udtEHSAccount, FunctionCode)

        udtEHSAccountPersonalInfo = _udtEHSAccount.EHSPersonalInformationList(0)
        udtAuditLogEntry.AddDescripton("Doc Type", udtEHSAccountPersonalInfo.DocCode)
        udtAuditLogEntry.AddDescripton("Identity No.", udtEHSAccountPersonalInfo.IdentityNum)
        If udtEHSAccountPersonalInfo.DocCode = DocType.DocTypeModel.DocTypeCode.ADOPC Then
            udtAuditLogEntry.AddDescripton("Prefix No.", udtEHSAccountPersonalInfo.AdoptionPrefixNum)
        End If
        udtAuditLogEntry.AddDescripton("Date of Birth", udtEHSAccountPersonalInfo.DOB.ToString("dd-MM-yyyy"))

        ' CRE11-004
        Dim objAuditLogInfo As New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, udtEHSAccountPersonalInfo.DocCode, udtEHSAccountPersonalInfo.IdentityNum)
        ' End CRE11-004

        udtAuditLogEntry.WriteEndLog(LogID.LOG00008, "Step 1 End", objAuditLogInfo)
    End Sub

    Private Sub btnFillBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnFillBack.Click
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        udtAuditLogEntry.WriteLog(LogID.LOG00010, "Step 1 Back to Main Page")

        _udtSessionHandler.PreFillChineseRemoveFromSession(FunctionCode)
        _udtSessionHandler.PreFillPressConfirmRemoveFromSession(FunctionCode)
        Me.Response.Redirect("~/main.aspx")
    End Sub

#Region "Set value to Confirm Page"
    Private Sub setReadOnlyDocumentType()
        Me.udcStep1a2ReadOnlyDocumnetType.ClearControl()
        setStepConfirmValue()
        Me.udcStep1a2ReadOnlyDocumnetType.DocumentType = udcStep1DocumentTypeRadioButtonGroup.SelectedValue

        Me.udcStep1a2ReadOnlyDocumnetType.EHSAccount = _udtEHSAccount
        Me.udcStep1a2ReadOnlyDocumnetType.Vertical = True
        Me.udcStep1a2ReadOnlyDocumnetType.MaskIdentityNo = False
        Me.udcStep1a2ReadOnlyDocumnetType.ShowAccountRefNo = False
        Me.udcStep1a2ReadOnlyDocumnetType.ShowTempAccountNotice = True
        Me.udcStep1a2ReadOnlyDocumnetType.ShowAccountCreationDate = False
        Me.udcStep1a2ReadOnlyDocumnetType.Built()
    End Sub

    Private Sub setStepConfirmValue()
        _udtEHSAccount = New EHSAccountModel
        udtEHSAccountPersonalInfo = _udtEHSAccount.EHSPersonalInformationList(0)

        Select Case udcStep1DocumentTypeRadioButtonGroup.SelectedValue
            Case DocTypeModel.DocTypeCode.HKBC
                setHKBC()

            Case DocTypeModel.DocTypeCode.HKIC
                setHKIC()

            Case DocTypeModel.DocTypeCode.DI
                setDI()

            Case DocTypeModel.DocTypeCode.ID235B
                setID235B()

            Case DocTypeModel.DocTypeCode.REPMT
                setREPMT()

            Case DocTypeModel.DocTypeCode.VISA
                setVISA()

            Case DocTypeModel.DocTypeCode.ADOPC
                setADOPC()

        End Select

    End Sub

    Private Sub setHKBC()
        Dim udcInputHKBC As ucInputHKBC = Me.udcStep1b1InputDocumentType.GetHKBCControl()
        udcInputHKBC.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

        Dim dtDOB As DateTime
        Dim strDOBtype As String = udcInputHKBC.IsExactDOB

        GeneralFunction.chkDOBtype(udcInputHKBC.DOB, dtDOB, strDOBtype)

        udtEHSAccountPersonalInfo.ENameSurName = udcInputHKBC.ENameSurName
        udtEHSAccountPersonalInfo.ENameFirstName = udcInputHKBC.ENameFirstName
        udtEHSAccountPersonalInfo.Gender = udcInputHKBC.Gender
        udtEHSAccountPersonalInfo.OtherInfo = udcInputHKBC.DOBInWord
        udtEHSAccountPersonalInfo.DocCode = DocType.DocTypeModel.DocTypeCode.HKBC
        udtEHSAccountPersonalInfo.ExactDOB = udcInputHKBC.IsExactDOB
        udtEHSAccountPersonalInfo.SetDOBTypeSelected(True)

        udtEHSAccountPersonalInfo.IdentityNum = udcInputHKBC.RegistrationNo.Replace("(", "").Replace(")", "") & ""
        udtEHSAccountPersonalInfo.DOB = dtDOB
    End Sub

    Private Sub setHKIC()
        Dim strHKIDIssueDate As String = Nothing
        Dim dtHKIDIssueDate As DateTime

        Dim udcInputHKIC As ucInputHKID = Me.udcStep1b1InputDocumentType.GetHKICControl()
        udcInputHKIC.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

        Dim dtDOB As DateTime
        Dim strDOBtype As String = String.Empty

        GeneralFunction.chkDOBtype(udcInputHKIC.DOB, dtDOB, strDOBtype)

        udtEHSAccountPersonalInfo.ENameSurName = udcInputHKIC.ENameSurName
        udtEHSAccountPersonalInfo.ENameFirstName = udcInputHKIC.ENameFirstName

        udtEHSAccountPersonalInfo.CCCode1 = String.Format("{0}{1}", udcInputHKIC.CCCode1, Me.udcChooseCCCode.SelectedCCCodeTail1)
        udtEHSAccountPersonalInfo.CCCode2 = String.Format("{0}{1}", udcInputHKIC.CCCode2, Me.udcChooseCCCode.SelectedCCCodeTail2)
        udtEHSAccountPersonalInfo.CCCode3 = String.Format("{0}{1}", udcInputHKIC.CCCode3, Me.udcChooseCCCode.SelectedCCCodeTail3)
        udtEHSAccountPersonalInfo.CCCode4 = String.Format("{0}{1}", udcInputHKIC.CCCode4, Me.udcChooseCCCode.SelectedCCCodeTail4)
        udtEHSAccountPersonalInfo.CCCode5 = String.Format("{0}{1}", udcInputHKIC.CCCode5, Me.udcChooseCCCode.SelectedCCCodeTail5)
        udtEHSAccountPersonalInfo.CCCode6 = String.Format("{0}{1}", udcInputHKIC.CCCode6, Me.udcChooseCCCode.SelectedCCCodeTail6)

        ''Retervie Chinese Name from Choose
        udcInputHKIC.CCCode1 = udtEHSAccountPersonalInfo.CCCode1
        udcInputHKIC.CCCode2 = udtEHSAccountPersonalInfo.CCCode2
        udcInputHKIC.CCCode3 = udtEHSAccountPersonalInfo.CCCode3
        udcInputHKIC.CCCode4 = udtEHSAccountPersonalInfo.CCCode4
        udcInputHKIC.CCCode5 = udtEHSAccountPersonalInfo.CCCode5
        udcInputHKIC.CCCode6 = udtEHSAccountPersonalInfo.CCCode6
        udcInputHKIC.SetCName()
        udtEHSAccountPersonalInfo.CName = udcInputHKIC.CName

        _udtSessionHandler.PreFillChineseSaveToSession(FunctionCode, udcInputHKIC.CName)

        dtHKIDIssueDate = formatter.convertHKIDIssueDateStringToDate(formatter.formatHKIDIssueDateBeforeValidate(udcInputHKIC.HKIDIssuseDate))
        udtEHSAccountPersonalInfo.Gender = udcInputHKIC.Gender
        udtEHSAccountPersonalInfo.DateofIssue = dtHKIDIssueDate
        udtEHSAccountPersonalInfo.DocCode = DocType.DocTypeModel.DocTypeCode.HKIC
        udtEHSAccountPersonalInfo.IdentityNum = udcInputHKIC.HKID.Replace("(", "").Replace(")", "") & ""
        udtEHSAccountPersonalInfo.ExactDOB = strDOBtype
        udtEHSAccountPersonalInfo.DOB = dtDOB

    End Sub

    Private Sub setDI()
        Dim dtDateOfIssue As DateTime

        Dim udcInputDI As ucInputDI = Me.udcStep1b1InputDocumentType.GetDIControl()
        udcInputDI.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

        Dim dtDOB As DateTime
        Dim strDOBtype As String = String.Empty

        GeneralFunction.chkDOBtype(udcInputDI.DOB, dtDOB, strDOBtype)

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'dtDateOfIssue = formatter.convertDate(formatter.formatDateBeforValidation_DDMMYYYY(formatter.formatDate(udcInputDI.DateOfIssue)), Session("language"))
        dtDateOfIssue = formatter.convertDate(formatter.formatDateBeforValidation_DDMMYYYY(formatter.formatInputDate(udcInputDI.DateOfIssue)), Session("language"))
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        udtEHSAccountPersonalInfo.ENameSurName = udcInputDI.ENameSurName
        udtEHSAccountPersonalInfo.ENameFirstName = udcInputDI.ENameFirstName
        udtEHSAccountPersonalInfo.Gender = udcInputDI.Gender
        udtEHSAccountPersonalInfo.DocCode = DocType.DocTypeModel.DocTypeCode.DI
        udtEHSAccountPersonalInfo.DateofIssue = dtDateOfIssue
        udtEHSAccountPersonalInfo.SetDOBTypeSelected(True)
        udtEHSAccountPersonalInfo.IdentityNum = udcInputDI.TravelDocNo
        udtEHSAccountPersonalInfo.ExactDOB = strDOBtype
        udtEHSAccountPersonalInfo.DOB = dtDOB

    End Sub

    Private Sub setID235B()
        Dim dtmPermitRemain As DateTime

        Dim udcInputID235B As ucInputID235B = Me.udcStep1b1InputDocumentType.GetID235BControl()
        udcInputID235B.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

        Dim dtDOB As DateTime
        Dim strDOBtype As String = String.Empty

        GeneralFunction.chkDOBtype(udcInputID235B.DateOfBirth, dtDOB, strDOBtype)

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'dtmPermitRemain = formatter.convertDate(formatter.formatDateBeforValidation_DDMMYYYY(formatter.formatDate(udcInputID235B.PermitRemain)), Common.Component.CultureLanguage.English)
        dtmPermitRemain = formatter.convertDate(formatter.formatDateBeforValidation_DDMMYYYY(formatter.formatInputDate(udcInputID235B.PermitRemain)), Common.Component.CultureLanguage.English)
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        udtEHSAccountPersonalInfo.ENameSurName = udcInputID235B.ENameSurName
        udtEHSAccountPersonalInfo.ENameFirstName = udcInputID235B.ENameFirstName
        udtEHSAccountPersonalInfo.Gender = udcInputID235B.Gender
        udtEHSAccountPersonalInfo.DocCode = DocType.DocTypeModel.DocTypeCode.ID235B
        udtEHSAccountPersonalInfo.PermitToRemainUntil = dtmPermitRemain
        udtEHSAccountPersonalInfo.SetDOBTypeSelected(True)
        udtEHSAccountPersonalInfo.IdentityNum = udcInputID235B.BirthEntryNo
        udtEHSAccountPersonalInfo.ExactDOB = strDOBtype
        udtEHSAccountPersonalInfo.DOB = dtDOB

    End Sub

    Private Sub setREPMT()
        Dim dtDateOfIssue As DateTime

        Dim udcInputReentryPermit As ucInputReentryPermit = Me.udcStep1b1InputDocumentType.GetREPMTControl()
        udcInputReentryPermit.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

        Dim dtDOB As DateTime
        Dim strDOBtype As String = String.Empty

        GeneralFunction.chkDOBtype(udcInputReentryPermit.DateOfBirth, dtDOB, strDOBtype)

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'dtDateOfIssue = formatter.convertDate(formatter.formatDateBeforValidation_DDMMYYYY(formatter.formatDate(udcInputReentryPermit.DateOfIssue)), Session("language"))
        dtDateOfIssue = formatter.convertDate(formatter.formatDateBeforValidation_DDMMYYYY(formatter.formatInputDate(udcInputReentryPermit.DateOfIssue)), Session("language"))
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        udtEHSAccountPersonalInfo.ENameSurName = udcInputReentryPermit.ENameSurName
        udtEHSAccountPersonalInfo.ENameFirstName = udcInputReentryPermit.ENameFirstName
        udtEHSAccountPersonalInfo.Gender = udcInputReentryPermit.Gender
        udtEHSAccountPersonalInfo.DocCode = DocType.DocTypeModel.DocTypeCode.REPMT
        udtEHSAccountPersonalInfo.DateofIssue = dtDateOfIssue
        udtEHSAccountPersonalInfo.SetDOBTypeSelected(True)
        udtEHSAccountPersonalInfo.IdentityNum = udcInputReentryPermit.REPMTNo.Replace("(", "").Replace(")", "") & ""
        udtEHSAccountPersonalInfo.ExactDOB = strDOBtype
        udtEHSAccountPersonalInfo.DOB = dtDOB

    End Sub

    Private Sub setVISA()
        Dim udcInputVISA As ucInputVISA = Me.udcStep1b1InputDocumentType.GetVISAControl()
        udcInputVISA.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

        Dim dtDOB As DateTime
        Dim strDOBtype As String = String.Empty

        GeneralFunction.chkDOBtype(udcInputVISA.DOB, dtDOB, strDOBtype)

        udtEHSAccountPersonalInfo.ENameSurName = udcInputVISA.ENameSurName
        udtEHSAccountPersonalInfo.ENameFirstName = udcInputVISA.ENameFirstName
        udtEHSAccountPersonalInfo.Gender = udcInputVISA.Gender
        udtEHSAccountPersonalInfo.DocCode = DocType.DocTypeModel.DocTypeCode.VISA
        udtEHSAccountPersonalInfo.Foreign_Passport_No = udcInputVISA.PassportNo
        udtEHSAccountPersonalInfo.SetDOBTypeSelected(True)
        udtEHSAccountPersonalInfo.IdentityNum = udcInputVISA.VISANo.Replace("(", "").Replace(")", "") & ""
        udtEHSAccountPersonalInfo.ExactDOB = strDOBtype
        udtEHSAccountPersonalInfo.DOB = dtDOB


    End Sub

    Private Sub setADOPC()
        Dim udcInputAdoption As ucInputAdoption = Me.udcStep1b1InputDocumentType.GetADOPCControl()
        udcInputAdoption.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

        Dim dtDOB As DateTime
        Dim strDOBtype As String = udcInputAdoption.IsExactDOB

        GeneralFunction.chkDOBtype(udcInputAdoption.DOB, dtDOB, strDOBtype)

        udtEHSAccountPersonalInfo.ENameSurName = udcInputAdoption.ENameSurName
        udtEHSAccountPersonalInfo.ENameFirstName = udcInputAdoption.ENameFirstName
        udtEHSAccountPersonalInfo.Gender = udcInputAdoption.Gender
        udtEHSAccountPersonalInfo.OtherInfo = udcInputAdoption.DOBInWord
        udtEHSAccountPersonalInfo.DocCode = DocType.DocTypeModel.DocTypeCode.ADOPC
        udtEHSAccountPersonalInfo.SetDOBTypeSelected(True)
        udtEHSAccountPersonalInfo.ExactDOB = udcInputAdoption.IsExactDOB

        udtEHSAccountPersonalInfo.IdentityNum = udcInputAdoption.IdentityNo
        udtEHSAccountPersonalInfo.AdoptionPrefixNum = udcInputAdoption.PerfixNo
        udtEHSAccountPersonalInfo.DOB = dtDOB

    End Sub

#End Region

#Region "Validate Input Page"
    Private Function validateInputPage() As Boolean
        'udcMsgBoxErr.Visible = False
        udcMsgBoxErr.Clear()

        Select Case udcStep1DocumentTypeRadioButtonGroup.SelectedValue
            Case DocTypeModel.DocTypeCode.HKIC
                If Not validateInputHKIC() Then
                    Return False
                End If

            Case DocTypeModel.DocTypeCode.HKBC
                If Not validateInputHKBC() Then
                    Return False
                End If

            Case DocTypeModel.DocTypeCode.DI
                If Not validateInputDI() Then
                    Return False
                End If

            Case DocTypeModel.DocTypeCode.ID235B
                If Not validateInputID235B() Then
                    Return False
                End If

            Case DocTypeModel.DocTypeCode.REPMT
                If Not validateInputREPMT() Then
                    Return False
                End If

            Case DocTypeModel.DocTypeCode.VISA
                If Not validateInputVISA() Then
                    Return False
                End If

            Case DocTypeModel.DocTypeCode.ADOPC
                If Not validateInputADOPC() Then
                    Return False
                End If

        End Select

        Return True
    End Function

    Private Function validateInputHKBC() As Boolean
        Dim isValid As Boolean = True
        Dim udcInputHKBC As ucInputHKBC = Me.udcStep1b1InputDocumentType.GetHKBCControl()
        udcInputHKBC.SetProperty(ucInputDocTypeBase.BuildMode.Creation)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        'Me._systemMessage = validator.chkRegNo(udcInputHKBC.RegistrationNo)
        Me._systemMessage = validator.chkIdentityNumber(DocTypeModel.DocTypeCode.HKBC, udcInputHKBC.RegistrationNo, String.Empty)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputHKBC.SetRegNoError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        Me._systemMessage = validator.chkEngName(udcInputHKBC.ENameSurName, udcInputHKBC.ENameFirstName)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputHKBC.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        Me._systemMessage = validator.chkGender(udcInputHKBC.Gender)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputHKBC.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        Me._systemMessage = validator.chkDOBType(udcInputHKBC.IsExactDOB)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputHKBC.SetDOBTypeError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        Else
            If udcInputHKBC.DOBInWordCase Then
                If udcInputHKBC.DOBInWord Is Nothing OrElse udcInputHKBC.DOBInWord = String.Empty Then
                    isValid = False
                    Me._systemMessage = New SystemMessage("990000", "E", "00160")
                    udcInputHKBC.SetDOBTypeError(True)
                    Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
                End If

                'Me._systemMessage = validator.chkDOB_New(udcInputHKBC.DOB)
                Me._systemMessage = validator.chkDOB_PreFill(DocTypeModel.DocTypeCode.HKBC, udcInputHKBC.DOB, udcInputHKBC.IsExactDOB)
                If Not Me._systemMessage Is Nothing Then
                    isValid = False
                    udcInputHKBC.SetDOBTypeError(True)
                    Me.udcMsgBoxErr.AddMessage(_systemMessage)
                End If
            Else
                'Me._systemMessage = validator.chkDOB_New(udcInputHKBC.DOB)
                Me._systemMessage = validator.chkDOB_PreFill(DocTypeModel.DocTypeCode.HKBC, udcInputHKBC.DOB, udcInputHKBC.IsExactDOB)
                If Not Me._systemMessage Is Nothing Then
                    isValid = False
                    udcInputHKBC.SetDOBError(True)
                    Me.udcMsgBoxErr.AddMessage(_systemMessage)
                End If
            End If
        End If

        If Not isValid Then
            ' CRE11-004
            Dim objAuditLogInfo As New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocType.DocTypeModel.DocTypeCode.HKBC, udcInputHKBC.RegistrationNo)
            Me.udcMsgBoxErr.BuildMessageBox(_strValidationFail, udtAuditLogEntry, Common.Component.LogID.LOG00009, "Step 1 Input Validate Fail", objAuditLogInfo)
        End If

        Return isValid
    End Function

    Private Function validateInputHKIC() As Boolean
        Dim isValid As Boolean = True
        Dim strHKIDIssueDate As String = Nothing
        Dim udcInputHKIC As ucInputHKID = Me.udcStep1b1InputDocumentType.GetHKICControl()
        udcInputHKIC.SetProperty(ucInputDocTypeBase.BuildMode.Creation)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        'If Me.NeedPopupChineseNameDialog() Then
        '    Me.bEmptyCCCode = False
        '    Me.udcStep1b1InputDocumentType_SelectChineseName_HKIC(udcInputHKIC, Nothing, Nothing)
        '    'isValid = False
        '    If Not Me.bEmptyCCCode Then
        '        '_udtSessionHandler.PreFillPressConfirmRemoveFromSession(FunctionCode)
        '        Return False
        '    End If
        'End If

        '_udtSessionHandler.PreFillPressConfirmRemoveFromSession(FunctionCode)

        'If udcInputHKIC.CCCodeIsEmpty() Then
        '    Me.udcChooseCCCode.Clean()
        'End If

        Dim dtDOB As DateTime
        Dim strDOBtype As String = String.Empty

        GeneralFunction.chkDOBtype(udcInputHKIC.DOB, dtDOB, strDOBtype)

        Me._systemMessage = validator.chkEngName(udcInputHKIC.ENameSurName, udcInputHKIC.ENameFirstName)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputHKIC.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        'If udcInputHKIC.CCCodeIsEmpty() Then

        '    'No CCCode
        '    udcInputHKIC.SetCName(String.Empty)

        '    'display error
        '    isValid = False
        '    Me._systemMessage = New SystemMessage("990000", "E", "00143")
        '    udcInputHKIC.SetCCCodeError(True)
        '    Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        '    'Else
        '    '    Me.udcChooseCCCode.CCCode1 = udcInputHKIC.CCCode1
        '    '    Me.udcChooseCCCode.CCCode2 = udcInputHKIC.CCCode2
        '    '    Me.udcChooseCCCode.CCCode3 = udcInputHKIC.CCCode3
        '    '    Me.udcChooseCCCode.CCCode4 = udcInputHKIC.CCCode4
        '    '    Me.udcChooseCCCode.CCCode5 = udcInputHKIC.CCCode5
        '    '    Me.udcChooseCCCode.CCCode6 = udcInputHKIC.CCCode6

        '    '    'Bind related chinese words into Drop Down List
        '    '    If Me.udcChooseCCCode.BindCCCode() Then
        '    '        isValid = False
        '    '        Me._systemMessage = New Common.ComObject.SystemMessage("990000", "E", "00039")
        '    '        Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        '    '        udcInputHKIC.SetCCCodeError(True)
        '    '    End If
        'End If

        'If Me.NeedPopupChineseNameDialog() Then
        '    Me.udcStep1b1InputDocumentType_SelectChineseName_HKIC(udcInputHKIC, Nothing, Nothing)
        '    isValid = False
        'End If

        Me._systemMessage = validator.chkCCCode_UsingDDL(String.Format("{0}{1}", udcInputHKIC.CCCode1, Me.udcChooseCCCode.SelectedCCCodeTail1), _
            String.Format("{0}{1}", udcInputHKIC.CCCode2, Me.udcChooseCCCode.SelectedCCCodeTail2), _
            String.Format("{0}{1}", udcInputHKIC.CCCode3, Me.udcChooseCCCode.SelectedCCCodeTail3), _
            String.Format("{0}{1}", udcInputHKIC.CCCode4, Me.udcChooseCCCode.SelectedCCCodeTail4), _
            String.Format("{0}{1}", udcInputHKIC.CCCode5, Me.udcChooseCCCode.SelectedCCCodeTail5), _
            String.Format("{0}{1}", udcInputHKIC.CCCode6, Me.udcChooseCCCode.SelectedCCCodeTail6))
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputHKIC.SetCCCodeError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        Else
        End If

        Me._systemMessage = validator.chkGender(udcInputHKIC.Gender)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputHKIC.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        strHKIDIssueDate = formatter.formatHKIDIssueDateBeforeValidate(udcInputHKIC.HKIDIssuseDate)

        'Me._systemMessage = validator.chkDOB_New(udcInputHKIC.DOB)
        Me._systemMessage = validator.chkDOB_PreFill(DocTypeModel.DocTypeCode.HKIC, udcInputHKIC.DOB, "")
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputHKIC.SetDOBError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)

            If strHKIDIssueDate = "" Then
                isValid = False
                udcInputHKIC.SetHKIDIssueDateError(True)
                Me.udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00081"))
            End If
        Else
            'Me._systemMessage = validator.chkHKIDIssueDate(strHKIDIssueDate)
            Me._systemMessage = validator.chkDataOfIssue(DocTypeModel.DocTypeCode.HKIC, strHKIDIssueDate, dtDOB)
            If Not Me._systemMessage Is Nothing Then
                isValid = False
                udcInputHKIC.SetHKIDIssueDateError(True)
                Me.udcMsgBoxErr.AddMessage(_systemMessage)
            End If
        End If

        'Me._systemMessage = validator.chkHKID(udcInputHKIC.HKID)
        Me._systemMessage = validator.chkIdentityNumber(DocTypeModel.DocTypeCode.HKIC, udcInputHKIC.HKID, String.Empty)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputHKIC.SetHKIDError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        If Not isValid Then
            ' CRE11-004
            Dim objAuditLogInfo As New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocType.DocTypeModel.DocTypeCode.HKIC, udcInputHKIC.HKID)
            Me.udcMsgBoxErr.BuildMessageBox(_strValidationFail, udtAuditLogEntry, Common.Component.LogID.LOG00009, "Step 1 Input Validate Fail", objAuditLogInfo)
        End If

        Return isValid
    End Function

    Private Function validateInputDI() As Boolean
        Dim isValid As Boolean = True
        Dim strDOI As String = String.Empty
        Dim udcInputDI As ucInputDI = Me.udcStep1b1InputDocumentType.GetDIControl()
        udcInputDI.SetProperty(ucInputDocTypeBase.BuildMode.Creation)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        Dim dtDOB As DateTime
        Dim strDOBtype As String = String.Empty

        GeneralFunction.chkDOBtype(udcInputDI.DOB, dtDOB, strDOBtype)

        'Me._systemMessage = validator.chkDocumentNo(udcInputDI.TravelDocNo)
        Me._systemMessage = validator.chkIdentityNumber(DocTypeModel.DocTypeCode.DI, udcInputDI.TravelDocNo, String.Empty)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputDI.SetTDError(True)
            Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        End If

        Me._systemMessage = validator.chkEngName(udcInputDI.ENameSurName, udcInputDI.ENameFirstName)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputDI.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        Me._systemMessage = validator.chkGender(udcInputDI.Gender)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputDI.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        End If

        strDOI = formatter.formatDateBeforValidation_DDMMYYYY(udcInputDI.DateOfIssue)

        'Me._systemMessage = validator.chkDOB_New(udcInputDI.DOB)
        Me._systemMessage = validator.chkDOB_PreFill(DocTypeModel.DocTypeCode.DI, udcInputDI.DOB, "")
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputDI.SetDOBError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)

            If strDOI = "" Then
                isValid = False
                udcInputDI.SetDOIError(True)
                Me.udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00081"))
            End If

        Else
            Me._systemMessage = validator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.DI, strDOI, dtDOB)
            If Not Me._systemMessage Is Nothing Then
                isValid = False
                udcInputDI.SetDOIError(True)
                Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
            End If

        End If

        If Not isValid Then
            Dim objAuditLogInfo As New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocType.DocTypeModel.DocTypeCode.DI, udcInputDI.TravelDocNo)
            Me.udcMsgBoxErr.BuildMessageBox(_strValidationFail, udtAuditLogEntry, Common.Component.LogID.LOG00009, "Step 1 Input Validate Fail", objAuditLogInfo)
        End If

        Return isValid
    End Function

    Private Function validateInputID235B() As Boolean
        Dim isValid As Boolean = True
        Dim udcInputID235B As ucInputID235B = Me.udcStep1b1InputDocumentType.GetID235BControl()
        udcInputID235B.SetProperty(ucInputDocTypeBase.BuildMode.Creation)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim strPermitRemain As String = String.Empty

        Dim dtDOB As DateTime
        Dim strDOBtype As String = String.Empty

        GeneralFunction.chkDOBtype(udcInputID235B.DateOfBirth, dtDOB, strDOBtype)

        'Me._systemMessage = validator.chkBirthEntryNo(udcInputID235B.BirthEntryNo)
        Me._systemMessage = validator.chkIdentityNumber(DocTypeModel.DocTypeCode.ID235B, udcInputID235B.BirthEntryNo, String.Empty)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputID235B.SetBirthEntryNoError(True)
            Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        End If

        Me._systemMessage = validator.chkEngName(udcInputID235B.ENameSurName, udcInputID235B.ENameFirstName)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputID235B.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        Me._systemMessage = validator.chkGender(udcInputID235B.Gender)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputID235B.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        End If

        strPermitRemain = formatter.formatDateBeforValidation_DDMMYYYY(udcInputID235B.PermitRemain)

        'Me._systemMessage = validator.chkDOB_New(udcInputID235B.DateOfBirth, DocTypeModel.DocTypeCode.ID235B)
        Me._systemMessage = validator.chkDOB_PreFill(DocTypeModel.DocTypeCode.ID235B, udcInputID235B.DateOfBirth, "")
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputID235B.SetDOBError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)

            If strPermitRemain = "" Then
                isValid = False
                udcInputID235B.SetPermitRemainError(True)
                Me.udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00218"))
            End If

        Else
            Me._systemMessage = validator.chkPremitToRemainUntil(strPermitRemain, dtDOB, DocType.DocTypeModel.DocTypeCode.ID235B, False)
            If Not Me._systemMessage Is Nothing Then
                isValid = False
                udcInputID235B.SetPermitRemainError(True)
                Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
            End If
        End If

        'strPermitRemain = formatter.formatDateBeforValidation_DDMMYYYY(udcInputID235B.PermitRemain)
        'If Not validator.isValidPermitToRemainUntilFormat(strPermitRemain) Then
        '    isValid = False
        '    udcInputID235B.SetPermitRemainError(True)
        '    Me.udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00188"))
        'End If

        If Not isValid Then
            Dim objAuditLogInfo As New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocType.DocTypeModel.DocTypeCode.ID235B, udcInputID235B.PermitRemain)
            Me.udcMsgBoxErr.BuildMessageBox(_strValidationFail, udtAuditLogEntry, Common.Component.LogID.LOG00009, "Step 1 Input Validate Fail", objAuditLogInfo)
        End If

        Return isValid
    End Function

    Private Function validateInputREPMT() As Boolean
        Dim isValid As Boolean = True
        Dim udcInputReentryPermit As ucInputReentryPermit = Me.udcStep1b1InputDocumentType.GetREPMTControl()
        udcInputReentryPermit.SetProperty(ucInputDocTypeBase.BuildMode.Creation)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        Dim strDOI As String = String.Empty

        Dim dtDOB As DateTime
        Dim strDOBtype As String = String.Empty

        GeneralFunction.chkDOBtype(udcInputReentryPermit.DateOfBirth, dtDOB, strDOBtype)

        'Me._systemMessage = validator.chkReEntryNo(udcInputReentryPermit.REPMTNo)
        Me._systemMessage = validator.chkIdentityNumber(DocTypeModel.DocTypeCode.REPMT, udcInputReentryPermit.REPMTNo, String.Empty)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputReentryPermit.SetREPMTNoError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        Me._systemMessage = validator.chkEngName(udcInputReentryPermit.ENameSurName, udcInputReentryPermit.ENameFirstName)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputReentryPermit.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        Me._systemMessage = validator.chkGender(udcInputReentryPermit.Gender)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputReentryPermit.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        End If

        strDOI = formatter.formatDateBeforValidation_DDMMYYYY(udcInputReentryPermit.DateOfIssue)

        'Me._systemMessage = validator.chkDOB_New(udcInputReentryPermit.DateOfBirth)
        Me._systemMessage = validator.chkDOB_PreFill(DocTypeModel.DocTypeCode.REPMT, udcInputReentryPermit.DateOfBirth, "")
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputReentryPermit.SetDOBError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)

            If strDOI = "" Then
                isValid = False
                udcInputReentryPermit.SetDOIError(True)
                Me.udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00081"))
            End If
        Else

            Me._systemMessage = validator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.REPMT, strDOI, dtDOB)
            If Not Me._systemMessage Is Nothing Then
                isValid = False
                udcInputReentryPermit.SetDOIError(True)
                Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
            End If

        End If

        If Not isValid Then
            Dim objAuditLogInfo As New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocType.DocTypeModel.DocTypeCode.REPMT, udcInputReentryPermit.REPMTNo)
            Me.udcMsgBoxErr.BuildMessageBox(_strValidationFail, udtAuditLogEntry, Common.Component.LogID.LOG00009, "Step 1 Input Validate Fail", objAuditLogInfo)
        End If

        Return isValid
    End Function

    Private Function validateInputVISA() As Boolean
        Dim isValid As Boolean = True
        Dim udcInputVISA As ucInputVISA = Me.udcStep1b1InputDocumentType.GetVISAControl()
        udcInputVISA.SetProperty(ucInputDocTypeBase.BuildMode.Creation)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        'Me._systemMessage = validator.chkVisaNo(udcInputVISA.VISANo)
        Me._systemMessage = validator.chkIdentityNumber(DocTypeModel.DocTypeCode.VISA, udcInputVISA.VISANo, String.Empty)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputVISA.SetVISANoError(True)
            Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        End If

        Me._systemMessage = validator.chkEngName(udcInputVISA.ENameSurName, udcInputVISA.ENameFirstName)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputVISA.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        Me._systemMessage = validator.chkGender(udcInputVISA.Gender)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputVISA.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        End If

        Me._systemMessage = validator.chkPassportNo(udcInputVISA.PassportNo)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputVISA.SetPassportNoError(True)
            Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        End If

        'If udcInputVISA.PassportNo.Equals(String.Empty) Then
        '    isValid = False
        '    udcInputVISA.SetPassportNoError(True)
        '    Me.udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00199"))
        'End If

        'Me._systemMessage = validator.chkDOB_New(udcInputVISA.DOB, DocTypeModel.DocTypeCode.VISA)
        Me._systemMessage = validator.chkDOB_PreFill(DocTypeModel.DocTypeCode.VISA, udcInputVISA.DOB, "")
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputVISA.SetDOBError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        If Not isValid Then
            Dim objAuditLogInfo As New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocType.DocTypeModel.DocTypeCode.VISA, udcInputVISA.VISANo)
            Me.udcMsgBoxErr.BuildMessageBox(_strValidationFail, udtAuditLogEntry, Common.Component.LogID.LOG00009, "Step 1 Input Validate Fail", objAuditLogInfo)
        End If

        Return isValid
    End Function

    Private Function validateInputADOPC() As Boolean
        Dim isValid As Boolean = True
        Dim udcInputAdoption As ucInputAdoption = Me.udcStep1b1InputDocumentType.GetADOPCControl()
        udcInputAdoption.SetProperty(ucInputDocTypeBase.BuildMode.Creation)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        'Me._systemMessage = validator.chkNoOfEntry(udcInputAdoption.PerfixNo & "/" & udcInputAdoption.IdentityNo)
        Me._systemMessage = validator.chkIdentityNumber(DocTypeModel.DocTypeCode.ADOPC, udcInputAdoption.IdentityNo, udcInputAdoption.PerfixNo)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputAdoption.SetEntryNoError(True)
            Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        End If

        Me._systemMessage = validator.chkEngName(udcInputAdoption.ENameSurName, udcInputAdoption.ENameFirstName)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputAdoption.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        Me._systemMessage = validator.chkGender(udcInputAdoption.Gender)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputAdoption.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        End If

        Me._systemMessage = validator.chkDOBType(udcInputAdoption.IsExactDOB)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputAdoption.SetDOBInWordError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        Else
            If udcInputAdoption.DOBInWordCase Then
                If udcInputAdoption.DOBInWord Is Nothing OrElse udcInputAdoption.DOBInWord = String.Empty Then
                    isValid = False
                    Me._systemMessage = New SystemMessage("990000", "E", "00160")
                    udcInputAdoption.SetDOBInWordError(True)
                    Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
                End If

                'Me._systemMessage = validator.chkDOB_New(udcInputAdoption.DOB)
                Me._systemMessage = validator.chkDOB_PreFill(DocTypeModel.DocTypeCode.ADOPC, udcInputAdoption.DOB, udcInputAdoption.IsExactDOB)
                If Not Me._systemMessage Is Nothing Then
                    isValid = False
                    udcInputAdoption.SetDOBInWordError(True)
                    Me.udcMsgBoxErr.AddMessage(_systemMessage)
                End If
            Else
                'Me._systemMessage = validator.chkDOB_New(udcInputAdoption.DOB)
                Me._systemMessage = validator.chkDOB_PreFill(DocTypeModel.DocTypeCode.ADOPC, udcInputAdoption.DOB, udcInputAdoption.IsExactDOB)
                If Not Me._systemMessage Is Nothing Then
                    isValid = False
                    udcInputAdoption.SetDOBError(True)
                    Me.udcMsgBoxErr.AddMessage(_systemMessage)
                End If
            End If
        End If

        If Not isValid Then
            Dim objAuditLogInfo As New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocType.DocTypeModel.DocTypeCode.ADOPC, udcInputAdoption.IdentityNo)
            Me.udcMsgBoxErr.BuildMessageBox(_strValidationFail, udtAuditLogEntry, Common.Component.LogID.LOG00009, "Step 1 Input Validate Fail", objAuditLogInfo)
        End If

        Return isValid
    End Function

#End Region

#Region "Logging on input control"
    Private Sub logInputControl()
        'udcMsgBoxErr.Visible = False
        'udcMsgBoxErr.Clear()

        Select Case udcStep1DocumentTypeRadioButtonGroup.SelectedValue
            Case DocTypeModel.DocTypeCode.HKIC
                logInputHKIC()

            Case DocTypeModel.DocTypeCode.HKBC
                logInputHKBC()

            Case DocTypeModel.DocTypeCode.DI
                logInputDI()

            Case DocTypeModel.DocTypeCode.ID235B
                logInputID235B()

            Case DocTypeModel.DocTypeCode.REPMT
                logInputREPMT()

            Case DocTypeModel.DocTypeCode.VISA
                logInputVISA()

            Case DocTypeModel.DocTypeCode.ADOPC
                logInputADOPC()

        End Select

    End Sub

    Private Sub logInputHKBC()
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim udcInputHKBC As ucInputHKBC = Me.udcStep1b1InputDocumentType.GetHKBCControl()
        udcInputHKBC.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

        udtAuditLogEntry.AddDescripton("Input Control", udcInputHKBC.RegistrationNo & _
        "||" & udcInputHKBC.ENameSurName & _
        "||" & udcInputHKBC.ENameFirstName & _
        "||" & udcInputHKBC.Gender & _
        "||" & udcInputHKBC.IsExactDOB & _
        "||" & udcInputHKBC.DOB & _
        "||" & udcInputHKBC.DOBInWord _
        )

        ' CRE11-004   
        'Dim strIdentityNumFullTemp = udcInputHKBC.RegistrationNo.Trim.ToUpper.Replace("-", "").Replace("(", "").Replace(")", "")
        'strIdentityNumFullTemp = formatter.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKBC, strIdentityNumFullTemp)
        'Dim objAuditLogInfo As New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocType.DocTypeModel.DocTypeCode.HKBC, strIdentityNumFullTemp)
        'udtAuditLogEntry.WriteLog(LogID.LOG00020, "Step 1 Input Control", objAuditLogInfo)
        udtAuditLogEntry.WriteLog(LogID.LOG00020, "Step 1 Input Control")
        'End CRE11-004   

    End Sub

    Private Sub logInputHKIC()
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim udcInputHKIC As ucInputHKID = Me.udcStep1b1InputDocumentType.GetHKICControl()
        udcInputHKIC.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

        udtAuditLogEntry.AddDescripton("Input Control", udcInputHKIC.HKID & _
        "||" & udcInputHKIC.ENameSurName & _
        "||" & udcInputHKIC.ENameFirstName & _
        "||" & udcInputHKIC.Gender & _
        "||" & udcInputHKIC.DOB & _
        "||" & udcInputHKIC.HKIDIssuseDate & _
        "||" & udcInputHKIC.CCCode1 & _
        "||" & udcInputHKIC.CCCode2 & _
        "||" & udcInputHKIC.CCCode3 & _
        "||" & udcInputHKIC.CCCode4 & _
        "||" & udcInputHKIC.CCCode5 & _
        "||" & udcInputHKIC.CCCode6 _
        )

        ' CRE11-004   
        'Dim strIdentityNumFullTemp = udcInputHKIC.HKID.Trim.ToUpper.Replace("-", "").Replace("(", "").Replace(")", "")
        'strIdentityNumFullTemp = formatter.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, strIdentityNumFullTemp)
        'Dim objAuditLogInfo As New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocType.DocTypeModel.DocTypeCode.HKIC, strIdentityNumFullTemp)
        'udtAuditLogEntry.WriteLog(LogID.LOG00020, "Step 1 Input Control", objAuditLogInfo)
        udtAuditLogEntry.WriteLog(LogID.LOG00020, "Step 1 Input Control")
        'End CRE11-004   

    End Sub

    Private Sub logInputDI()
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode)
        Dim udcInputDI As ucInputDI = Me.udcStep1b1InputDocumentType.GetDIControl()
        udcInputDI.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

        udtAuditLogEntry.AddDescripton("Input Control", udcInputDI.TravelDocNo & _
        "||" & udcInputDI.ENameSurName & _
        "||" & udcInputDI.ENameFirstName & _
        "||" & udcInputDI.Gender & _
        "||" & udcInputDI.DOB & _
        "||" & udcInputDI.DateOfIssue _
        )

        ' CRE11-004   
        'Dim strIdentityNumFullTemp = udcInputDI.TravelDocNo.Trim.ToUpper.Replace("-", "").Replace("(", "").Replace(")", "")
        'strIdentityNumFullTemp = formatter.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.DI, strIdentityNumFullTemp)
        'Dim objAuditLogInfo As New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocType.DocTypeModel.DocTypeCode.DI, strIdentityNumFullTemp)
        'udtAuditLogEntry.WriteLog(LogID.LOG00020, "Step 1 Input Control", objAuditLogInfo)
        udtAuditLogEntry.WriteLog(LogID.LOG00020, "Step 1 Input Control")
        'End CRE11-004   

    End Sub

    Private Sub logInputID235B()
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim udcInputID235B As ucInputID235B = Me.udcStep1b1InputDocumentType.GetID235BControl()
        udcInputID235B.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

        udtAuditLogEntry.AddDescripton("Input Control", udcInputID235B.BirthEntryNo & _
        "||" & udcInputID235B.ENameSurName & _
        "||" & udcInputID235B.ENameFirstName & _
        "||" & udcInputID235B.Gender & _
        "||" & udcInputID235B.DateOfBirth & _
        "||" & udcInputID235B.PermitRemain _
        )

        ' CRE11-004   
        'Dim strIdentityNumFullTemp = udcInputID235B.BirthEntryNo.Trim.ToUpper.Replace("-", "").Replace("(", "").Replace(")", "")
        'strIdentityNumFullTemp = formatter.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.ID235B, strIdentityNumFullTemp)
        'Dim objAuditLogInfo As New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocType.DocTypeModel.DocTypeCode.ID235B, formatter.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.ID235B, udcInputID235B.BirthEntryNo))
        'udtAuditLogEntry.WriteLog(LogID.LOG00020, "Step 1 Input Control", objAuditLogInfo)
        udtAuditLogEntry.WriteLog(LogID.LOG00020, "Step 1 Input Control")
        'End CRE11-004   

    End Sub

    Private Sub logInputREPMT()
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim udcInputReentryPermit As ucInputReentryPermit = Me.udcStep1b1InputDocumentType.GetREPMTControl()
        udcInputReentryPermit.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

        udtAuditLogEntry.AddDescripton("Input Control", udcInputReentryPermit.REPMTNo & _
        "||" & udcInputReentryPermit.ENameSurName & _
        "||" & udcInputReentryPermit.ENameFirstName & _
        "||" & udcInputReentryPermit.Gender & _
        "||" & udcInputReentryPermit.DateOfBirth & _
        "||" & udcInputReentryPermit.DateOfIssue _
        )

        ' CRE11-004   
        'Dim strIdentityNumFullTemp = udcInputReentryPermit.REPMTNo.Trim.ToUpper.Replace("-", "").Replace("(", "").Replace(")", "")
        'strIdentityNumFullTemp = formatter.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.REPMT, strIdentityNumFullTemp)
        'Dim objAuditLogInfo As New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocType.DocTypeModel.DocTypeCode.REPMT, strIdentityNumFullTemp)
        'udtAuditLogEntry.WriteLog(LogID.LOG00020, "Step 1 Input Control", objAuditLogInfo)
        udtAuditLogEntry.WriteLog(LogID.LOG00020, "Step 1 Input Control")
        'End CRE11-004   

    End Sub

    Private Sub logInputVISA()
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim udcInputVISA As ucInputVISA = Me.udcStep1b1InputDocumentType.GetVISAControl()
        udcInputVISA.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

        udtAuditLogEntry.AddDescripton("Input Control", udcInputVISA.VISANo & _
        "||" & udcInputVISA.ENameSurName & _
        "||" & udcInputVISA.ENameFirstName & _
        "||" & udcInputVISA.Gender & _
        "||" & udcInputVISA.PassportNo & _
        "||" & udcInputVISA.DOB _
        )

        ' CRE11-004   
        'Dim strIdentityNumFullTemp = udcInputVISA.VISANo.Trim.ToUpper.Replace("-", "").Replace("(", "").Replace(")", "")
        'strIdentityNumFullTemp = formatter.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.VISA, strIdentityNumFullTemp)
        'Dim objAuditLogInfo As New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocType.DocTypeModel.DocTypeCode.VISA, strIdentityNumFullTemp)
        'udtAuditLogEntry.WriteLog(LogID.LOG00020, "Step 1 Input Control", objAuditLogInfo)
        udtAuditLogEntry.WriteLog(LogID.LOG00020, "Step 1 Input Control")
        'End CRE11-004   

    End Sub

    Private Sub logInputADOPC()
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim udcInputAdoption As ucInputAdoption = Me.udcStep1b1InputDocumentType.GetADOPCControl()
        udcInputAdoption.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

        udtAuditLogEntry.AddDescripton("Input Control", udcInputAdoption.IdentityNo & _
        "||" & udcInputAdoption.PerfixNo & _
        "||" & udcInputAdoption.ENameSurName & _
        "||" & udcInputAdoption.ENameFirstName & _
        "||" & udcInputAdoption.Gender & _
        "||" & udcInputAdoption.IsExactDOB & _
        "||" & udcInputAdoption.DOB & _
        "||" & udcInputAdoption.DOBInWord _
        )

        ' CRE11-004   
        'Dim strIdentityNumFullTemp = udcInputAdoption.IdentityNo.Trim.ToUpper.Replace("-", "").Replace("(", "").Replace(")", "")
        'strIdentityNumFullTemp = formatter.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.DI, strIdentityNumFullTemp)
        'Dim objAuditLogInfo As New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocType.DocTypeModel.DocTypeCode.ADOPC, strIdentityNumFullTemp)
        'udtAuditLogEntry.WriteLog(LogID.LOG00020, "Step 1 Input Control", objAuditLogInfo)
        udtAuditLogEntry.WriteLog(LogID.LOG00020, "Step 1 Input Control")
        'End CRE11-004   

    End Sub

#End Region

#Region "For HKID"
    'For HKIC Case Only
    Private Sub udcStep1b1InputDocumentType_SelectChineseName_HKIC(ByVal udcInputHKID As ucInputHKID, ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) _
        Handles udcStep1b1InputDocumentType.SelectChineseName_HKIC

        udcStep1b1InputDocumentType_SelectChineseName_HKIC(udcInputHKID, True, sender, e)
    End Sub

    Private Sub udcStep1b1InputDocumentType_SelectChineseName_HKIC(ByVal udcInputHKID As ucInputHKID, ByVal bLogInput As Boolean, ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim isValid As Boolean = True
        Dim systemMessage As SystemMessage

        If (bLogInput) Then
            Me.logInputControl()
        End If

        udcInputHKID.SetProperty(ucInputDocTypeBase.BuildMode.Creation)
        If udcInputHKID.CCCodeIsEmpty() Then

            'No CCCode
            udcInputHKID.SetCName(String.Empty)
            _udtSessionHandler.PreFillChineseSaveToSession(FunctionCode, String.Empty)
            Me.udcChooseCCCode.Clean()
            _udtSessionHandler.CCCodeRemoveFromSession(FunctionCode)
            Me.bEmptyCCCode = True

            ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [Start][Winnie] Step 0
            'display error
            isValid = False
            systemMessage = New SystemMessage("990000", "E", "00143")
            Me.udcMsgBoxErr.AddMessage(systemMessage)
            udcInputHKID.SetCCCodeError(True)
            ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [End][Winnie] Step 0
        Else

            ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [Start][Winnie] Step 1
            'Me.udcChooseCCCode.CCCode1 = udcInputHKID.CCCode1
            'Me.udcChooseCCCode.CCCode2 = udcInputHKID.CCCode2
            'Me.udcChooseCCCode.CCCode3 = udcInputHKID.CCCode3
            'Me.udcChooseCCCode.CCCode4 = udcInputHKID.CCCode4
            'Me.udcChooseCCCode.CCCode5 = udcInputHKID.CCCode5
            'Me.udcChooseCCCode.CCCode6 = udcInputHKID.CCCode6

            Me.udcChooseCCCode.CCCode1 = udcInputHKID.GetCCCode(udcInputHKID.CCCode1, Me.udcChooseCCCode.getCCCodeFromSession(1, FunctionCode))
            Me.udcChooseCCCode.CCCode2 = udcInputHKID.GetCCCode(udcInputHKID.CCCode2, Me.udcChooseCCCode.getCCCodeFromSession(2, FunctionCode))
            Me.udcChooseCCCode.CCCode3 = udcInputHKID.GetCCCode(udcInputHKID.CCCode3, Me.udcChooseCCCode.getCCCodeFromSession(3, FunctionCode))
            Me.udcChooseCCCode.CCCode4 = udcInputHKID.GetCCCode(udcInputHKID.CCCode4, Me.udcChooseCCCode.getCCCodeFromSession(4, FunctionCode))
            Me.udcChooseCCCode.CCCode5 = udcInputHKID.GetCCCode(udcInputHKID.CCCode5, Me.udcChooseCCCode.getCCCodeFromSession(5, FunctionCode))
            Me.udcChooseCCCode.CCCode6 = udcInputHKID.GetCCCode(udcInputHKID.CCCode6, Me.udcChooseCCCode.getCCCodeFromSession(6, FunctionCode))
            ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [End][Winnie] Step 1

            'Bind related chinese words into Drop Down List
            systemMessage = Me.udcChooseCCCode.BindCCCode()
            If systemMessage Is Nothing Then
                udcInputHKID.SetCCCodeError(False)
                Me.ModalPopupExtenderChooseCCCode.Show()
            Else
                isValid = False
                Me.udcMsgBoxErr.AddMessage(systemMessage)
                udcInputHKID.SetCCCodeError(True)
                _udtSessionHandler.PreFillPressConfirmRemoveFromSession(FunctionCode)
                '_udtSessionHandler.PreFillChineseRemoveFromSession(FunctionCode)
            End If
            Me.bEmptyCCCode = False
        End If

        If Not isValid Then
            Me.udcMsgBoxErr.BuildMessageBox(_strValidationFail)
        End If
    End Sub

    Private Sub udcChooseCCCode_Cancel(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcChooseCCCode.Cancel
        _udtSessionHandler.PreFillPressConfirmRemoveFromSession(FunctionCode)
        Me.ModalPopupExtenderChooseCCCode.Hide()
    End Sub

    Private Sub udcChooseCCCode_Confirm(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcChooseCCCode.Confirm
        Dim udcInputHKIC As ucInputHKID = Me.udcStep1b1InputDocumentType.GetHKICControl
        Dim strChineseName As String
        Dim strPressConfirm As String
        udcInputHKIC.SetProperty(ucInputDocTypeBase.BuildMode.Creation)
        Me.udcChooseCCCode.CCCode1 = udcInputHKIC.CCCode1
        Me.udcChooseCCCode.CCCode2 = udcInputHKIC.CCCode2
        Me.udcChooseCCCode.CCCode3 = udcInputHKIC.CCCode3
        Me.udcChooseCCCode.CCCode4 = udcInputHKIC.CCCode4
        Me.udcChooseCCCode.CCCode5 = udcInputHKIC.CCCode5
        Me.udcChooseCCCode.CCCode6 = udcInputHKIC.CCCode6

        'Get Chinese From drop down list, and Save CCCode in Session
        strChineseName = Me.udcChooseCCCode.GetChineseName(FunctionCode, True)
        udcInputHKIC.SetCName(strChineseName)

        _udtSessionHandler.PreFillChineseSaveToSession(FunctionCode, strChineseName)

        Me.ModalPopupExtenderChooseCCCode.Hide()

        strPressConfirm = _udtSessionHandler.PreFillPressConfirmGetFormSession(FunctionCode)
        _udtSessionHandler.PreFillPressConfirmRemoveFromSession(FunctionCode)

        If strPressConfirm = "Y" Then
            btnNext_Click(Nothing, Nothing)
        End If

    End Sub

    Private Function NeedPopupChineseNameDialog() As Boolean
        'isDiff is using for check the sessoion CCCode is same as current CCCode 
        'isDiff = true : sessoion CCCode <> current CCCode 
        Dim isDiff As Boolean = True
        Dim isDiff1 As Boolean = True
        Dim isDiff2 As Boolean = True
        Dim isDiff3 As Boolean = True
        Dim isDiff4 As Boolean = True
        Dim isDiff5 As Boolean = True
        Dim isDiff6 As Boolean = True
        Dim udcInputHKIC As ucInputHKID = Me.udcStep1b1InputDocumentType.GetHKICControl()
        udcInputHKIC.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

        isDiff1 = Me.udcChooseCCCode.CCCodeDiff(udcInputHKIC.CCCode1, FunctionCode, 1)
        isDiff2 = Me.udcChooseCCCode.CCCodeDiff(udcInputHKIC.CCCode2, FunctionCode, 2)
        isDiff3 = Me.udcChooseCCCode.CCCodeDiff(udcInputHKIC.CCCode3, FunctionCode, 3)
        isDiff4 = Me.udcChooseCCCode.CCCodeDiff(udcInputHKIC.CCCode4, FunctionCode, 4)
        isDiff5 = Me.udcChooseCCCode.CCCodeDiff(udcInputHKIC.CCCode5, FunctionCode, 5)
        isDiff6 = Me.udcChooseCCCode.CCCodeDiff(udcInputHKIC.CCCode6, FunctionCode, 6)

        If isDiff1 Or isDiff2 Or isDiff3 Or isDiff4 Or isDiff5 Or isDiff6 Then
            Return True
        End If

        Return False


        'isDiff = Me.udcChooseCCCode.CCCodeDiff(udcInputHKIC.CCCode1, FunctionCode, 1)

        'If Not isDiff Then
        '    isDiff = Me.udcChooseCCCode.CCCodeDiff(udcInputHKIC.CCCode2, FunctionCode, 2)
        'End If
        'If Not isDiff Then
        '    isDiff = Me.udcChooseCCCode.CCCodeDiff(udcInputHKIC.CCCode3, FunctionCode, 3)
        'End If
        'If Not isDiff Then
        '    isDiff = Me.udcChooseCCCode.CCCodeDiff(udcInputHKIC.CCCode4, FunctionCode, 4)
        'End If
        'If Not isDiff Then
        '    isDiff = Me.udcChooseCCCode.CCCodeDiff(udcInputHKIC.CCCode5, FunctionCode, 5)
        'End If
        'If Not isDiff Then
        '    isDiff = Me.udcChooseCCCode.CCCodeDiff(udcInputHKIC.CCCode6, FunctionCode, 6)
        'End If

        'Return isDiff
    End Function

#End Region

    'For Confirm Form

    Private Sub btnConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnConfirm.Click
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        _udtEHSAccount = _udtSessionHandler.EHSAccountGetFromSession(FunctionCode)
        udtEHSAccountPersonalInfo = _udtEHSAccount.EHSPersonalInformationList(0)

        udtAuditLogEntry.AddDescripton("Doc Type", udtEHSAccountPersonalInfo.DocCode)
        udtAuditLogEntry.AddDescripton("Identity No.", udtEHSAccountPersonalInfo.IdentityNum)
        If udtEHSAccountPersonalInfo.DocCode = DocType.DocTypeModel.DocTypeCode.ADOPC Then
            udtAuditLogEntry.AddDescripton("Prefix No.", udtEHSAccountPersonalInfo.AdoptionPrefixNum)
        End If
        udtAuditLogEntry.AddDescripton("Date of Birth", udtEHSAccountPersonalInfo.DOB.ToString("dd-MM-yyyy"))

        ' CRE11-004   
        Dim objAuditLogInfo As New AuditLogInfo(Nothing, Nothing, _udtEHSAccount.AccountSourceString, _udtEHSAccount.VoucherAccID, udtEHSAccountPersonalInfo.DocCode, udtEHSAccountPersonalInfo.IdentityNum)
        'udtAuditLogEntry.WriteStartLog(LogID.LOG00011, "Step 2 Start", "")
        udtAuditLogEntry.WriteStartLog(LogID.LOG00011, "Step 2 Start", objAuditLogInfo)
        'End CRE11-004   

        mvPreFill.ActiveViewIndex = 2

        Dim dtSubmit As Date = DateTime.Now

        lblCompleteSubmissionDate.Text = formatter.convertDateTime(dtSubmit)

        _udtSessionHandler.PreFillSubmitTimeSaveToSession(FunctionCode, dtSubmit)

        Dim strPrevConsentID As String = _udtSessionHandler.PreFillConsentIDGetFormSession(FunctionCode)
        Dim sPreFill As String = ""

        If strPrevConsentID = "" Then
            udtAuditLogEntry.WriteLog(LogID.LOG00012, "Step 2 Generate Pre-Fill Consent No.", objAuditLogInfo)
            sPreFill = GeneralFunction.generatePrefillNumber()

            udtAuditLogEntry.WriteLog(LogID.LOG00013, "Step 2 Save DB Pre-Fill Consent No.: " & sPreFill, objAuditLogInfo)
            SaveDB(sPreFill)
        Else
            sPreFill = strPrevConsentID
            udtAuditLogEntry.WriteLog(LogID.LOG00021, "Step 2 Double click to generate Pre-Fill Consent No.", objAuditLogInfo)
        End If

        udtAuditLogEntry.WriteLog(LogID.LOG00014, "Step 2 Display Result Grid", objAuditLogInfo)
        ShowPrintGrid(sPreFill)
        SetProcessCss(3)

        setToDoLabel()

        _udtSessionHandler.PreFillChineseRemoveFromSession(FunctionCode)
        _udtSessionHandler.PreFillPressConfirmRemoveFromSession(FunctionCode)

        udcInfoMessageBox.AddMessage(FunctionCode, "I", "00001")
        udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
        udcInfoMessageBox.BuildMessageBox()
        ' CRE11-004  
        'udtAuditLogEntry.WriteEndLog(LogID.LOG00015, "Step 2 End", "")
        udtAuditLogEntry.WriteEndLog(LogID.LOG00015, "Step 2 End", objAuditLogInfo)
        'End CRE11-004   

        ' CRE15-004 TIV & QIV [Start][Lawrence]
        ' Upon refresh the page after submitting, redirect to main page to reset all session variables
        Session("PreFillMain") = Nothing
        ' CRE15-004 TIV & QIV [End][Lawrence]

    End Sub

    Private Sub btnConfirmBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnConfirmBack.Click
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        _udtEHSAccount = _udtSessionHandler.EHSAccountGetFromSession(FunctionCode)
        udtEHSAccountPersonalInfo = _udtEHSAccount.EHSPersonalInformationList(0)

        udtAuditLogEntry.AddDescripton("Doc Type", udtEHSAccountPersonalInfo.DocCode)
        udtAuditLogEntry.AddDescripton("Identity No.", udtEHSAccountPersonalInfo.IdentityNum)
        If udtEHSAccountPersonalInfo.DocCode = DocType.DocTypeModel.DocTypeCode.ADOPC Then
            udtAuditLogEntry.AddDescripton("Prefix No.", udtEHSAccountPersonalInfo.AdoptionPrefixNum)
        End If
        udtAuditLogEntry.AddDescripton("Date of Birth", udtEHSAccountPersonalInfo.DOB.ToString("dd-MM-yyyy"))

        ' CRE11-004   
        Dim objAuditLogInfo As New AuditLogInfo(Nothing, Nothing, _udtEHSAccount.AccountSourceString, _udtEHSAccount.VoucherAccID, udtEHSAccountPersonalInfo.DocCode, udtEHSAccountPersonalInfo.IdentityNum)
        'End CRE11-004   

        ' CRE11-004
        udtAuditLogEntry.AddDescripton("DocType", udtEHSAccountPersonalInfo.DocCode)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00016, "Back Step 1 Start", objAuditLogInfo)

        mvPreFill.ActiveViewIndex = 0

        udtAuditLogEntry.WriteLog(LogID.LOG00017, "Back Step 1 Set Input Doc", objAuditLogInfo)

        If Not _udtEHSAccount Is Nothing Then
            udtEHSAccountPersonalInfo = _udtEHSAccount.EHSPersonalInformationList(0)
            udcStep1DocumentTypeRadioButtonGroup.SelectedValue = udtEHSAccountPersonalInfo.DocCode
        End If

        setInputDocumentType()

        'If Not _udtEHSAccount Is Nothing Then
        '    setBackInputValue()
        'End If

        SetProcessCss(1)

        _udtSessionHandler.EHSAccountRemoveFromSession(FunctionCode)

        udtAuditLogEntry.WriteEndLog(LogID.LOG00018, "Back Step 1 End", objAuditLogInfo)
    End Sub

#Region "Set back value to Input Page"
    Private Sub setBackInputValue()
        Select Case udcStep1DocumentTypeRadioButtonGroup.SelectedValue
            Case DocTypeModel.DocTypeCode.HKIC
                setBackHKIC()

            Case DocTypeModel.DocTypeCode.HKBC
                setBackHKBC()

            Case DocTypeModel.DocTypeCode.DI
                setBackDI()

            Case DocTypeModel.DocTypeCode.ID235B
                setBackID235B()

            Case DocTypeModel.DocTypeCode.REPMT
                setBackREPMT()

            Case DocTypeModel.DocTypeCode.VISA
                setBackVISA()

            Case DocTypeModel.DocTypeCode.ADOPC
                setBackADOPC()

        End Select

    End Sub

    Private Sub setBackHKBC()
        'Dim udcInputHKBC As ucInputHKBC = Me.udcStep1b1InputDocumentType.GetHKBCControl()

        'udcInputHKBC.ENameSurName = udtEHSAccountPersonalInfo.ENameSurName & ""
        'udcInputHKBC.ENameFirstName = udtEHSAccountPersonalInfo.ENameFirstName & ""
        'udcInputHKBC.Gender = udtEHSAccountPersonalInfo.Gender & ""
        'udcInputHKBC.DOBInWord = udtEHSAccountPersonalInfo.OtherInfo & ""
        'udcInputHKBC.IsExactDOB = udtEHSAccountPersonalInfo.ExactDOB & ""

        'udcInputHKBC.RegistrationNo = formatter.formatHKID(udtEHSAccountPersonalInfo.IdentityNum, False)
        'udcInputHKBC.DOB = udtEHSAccountPersonalInfo.DOB.ToString("dd-MM-yyyy")
        'udcInputHKBC.SetValue(ucInputDocTypeBase.BuildMode.Creation)
    End Sub

    Private Sub setBackHKIC()
        'Dim udcInputHKIC As ucInputHKID = Me.udcStep1b1InputDocumentType.GetHKICControl()

        'udcInputHKIC.ENameSurName = udtEHSAccountPersonalInfo.ENameSurName
        'udcInputHKIC.ENameFirstName = udtEHSAccountPersonalInfo.ENameFirstName

        'udcInputHKIC.CCCode1 = udtEHSAccountPersonalInfo.CCCode1
        'udcInputHKIC.CCCode2 = udtEHSAccountPersonalInfo.CCCode2
        'udcInputHKIC.CCCode3 = udtEHSAccountPersonalInfo.CCCode3
        'udcInputHKIC.CCCode4 = udtEHSAccountPersonalInfo.CCCode4
        'udcInputHKIC.CCCode5 = udtEHSAccountPersonalInfo.CCCode5
        'udcInputHKIC.CCCode6 = udtEHSAccountPersonalInfo.CCCode6
        'udcInputHKIC.SetCName()

        'udcInputHKIC.Gender = udtEHSAccountPersonalInfo.Gender

        'udcInputHKIC.DOB = udtEHSAccountPersonalInfo.DOB.ToString("dd-MM-yyyy")
        'udcInputHKIC.HKIDIssuseDate = Convert.ToDateTime(udtEHSAccountPersonalInfo.DateofIssue).ToString("dd-MM-yy")
        'udcInputHKIC.HKID = formatter.formatHKID(udtEHSAccountPersonalInfo.IdentityNum, False)
        'udcInputHKIC.SetValue(ucInputDocTypeBase.BuildMode.Creation)

    End Sub

    Private Sub setBackDI()
        'Dim udcInputDI As ucInputDI = Me.udcStep1b1InputDocumentType.GetDIControl()

        'udcInputDI.ENameSurName = udtEHSAccountPersonalInfo.ENameSurName
        'udcInputDI.ENameFirstName = udtEHSAccountPersonalInfo.ENameFirstName
        'udcInputDI.Gender = udtEHSAccountPersonalInfo.Gender
        'udcInputDI.DOB = udtEHSAccountPersonalInfo.DOB.ToString("dd-MM-yyyy")
        'udcInputDI.DateOfIssue = Convert.ToDateTime(udtEHSAccountPersonalInfo.DateofIssue).ToString("dd-MM-yyyy")
        'udcInputDI.TravelDocNo = udtEHSAccountPersonalInfo.IdentityNum
        'udcInputDI.SetValue(ucInputDocTypeBase.BuildMode.Creation)

    End Sub

    Private Sub setBackID235B()
        'Dim udcInputID235B As ucInputID235B = Me.udcStep1b1InputDocumentType.GetID235BControl()
        'udcInputID235B.EHSPersonalInfo = udtEHSAccountPersonalInfo

        '_udtEHSAccount()

        'udcInputID235B.ENameSurName = udtEHSAccountPersonalInfo.ENameSurName
        'udcInputID235B.ENameFirstName = udtEHSAccountPersonalInfo.ENameFirstName
        'udcInputID235B.Gender = udtEHSAccountPersonalInfo.Gender
        'udcInputID235B.DateOfBirth = udtEHSAccountPersonalInfo.DOB.ToString("dd-MM-yyyy")
        'udcInputID235B.PermitRemain = Convert.ToDateTime(udtEHSAccountPersonalInfo.PermitToRemainUntil).ToString("dd-MM-yyyy")
        'udcInputID235B.BirthEntryNo = udtEHSAccountPersonalInfo.IdentityNum

        'udcInputID235B.SetValue(ucInputDocTypeBase.BuildMode.Creation)


    End Sub

    Private Sub setBackREPMT()
        'Dim udcInputReentryPermit As ucInputReentryPermit = Me.udcStep1b1InputDocumentType.GetREPMTControl()

        'udcInputReentryPermit.ENameSurName = udtEHSAccountPersonalInfo.ENameSurName
        'udcInputReentryPermit.ENameFirstName = udtEHSAccountPersonalInfo.ENameFirstName
        'udcInputReentryPermit.Gender = udtEHSAccountPersonalInfo.Gender

        'udcInputReentryPermit.SetValue(ucInputDocTypeBase.BuildMode.Creation)
    End Sub

    Private Sub setBackVISA()
        'Dim udcInputVISA As ucInputVISA = Me.udcStep1b1InputDocumentType.GetVISAControl()

        'udcInputVISA.ENameSurName = udtEHSAccountPersonalInfo.ENameSurName
        'udcInputVISA.ENameFirstName = udtEHSAccountPersonalInfo.ENameFirstName
        'udcInputVISA.Gender = udtEHSAccountPersonalInfo.Gender

        'udcInputVISA.SetValue(ucInputDocTypeBase.BuildMode.Creation)
    End Sub

    Private Sub setBackADOPC()
        Dim udcInputAdoption As ucInputAdoption = Me.udcStep1b1InputDocumentType.GetADOPCControl()

        udcInputAdoption.ENameSurName = udtEHSAccountPersonalInfo.ENameSurName
        udcInputAdoption.ENameFirstName = udtEHSAccountPersonalInfo.ENameFirstName
        udcInputAdoption.Gender = udtEHSAccountPersonalInfo.Gender

        udcInputAdoption.SetValue(ucInputDocTypeBase.BuildMode.Creation)
    End Sub


#End Region

    'For Print Form

    Private Sub ShowPrintGrid(ByVal sPreFill As String)
        Dim sFormatPreFill As String = formatter.formatSystemNumber(sPreFill)

        Dim dtSP As DataTable = New DataTable
        dtSP.Columns.Add(New DataColumn("Pre_Fill_Consent_ID"))
        dtSP.Columns.Add(New DataColumn("EnglishName"))
        dtSP.Columns.Add(New DataColumn("ChineseName"))

        Dim dr As DataRow
        dr = dtSP.NewRow

        'dr("Pre_Fill_Consent_ID") = "P09901-1-6"
        'dr("Pre_Fill_Consent_ID") = GeneralFunction.generateSystemNum("A")
        _udtSessionHandler.PreFillConsentIDSaveToSession(FunctionCode, sFormatPreFill)

        dr("Pre_Fill_Consent_ID") = sFormatPreFill
        dr("EnglishName") = udtEHSAccountPersonalInfo.ENameSurName & ", " & udtEHSAccountPersonalInfo.ENameFirstName

        If udtEHSAccountPersonalInfo.CName & "" <> "" Then
            'dr("EnglishName") = dr("EnglishName") & " (" & udtEHSAccountPersonalInfo.CName & ")"
            dr("ChineseName") = "(" & udtEHSAccountPersonalInfo.CName & ")"
        Else
            dr("ChineseName") = ""
        End If

        dtSP.Rows.Add(dr)

        Me.gvPrintOut.DataSource = dtSP
        Me.gvPrintOut.DataBind()
        Me.gvPrintOut.Visible = True
    End Sub

    Private Sub SaveDB(ByVal sPreFill As String)
        _udtEHSAccount = _udtSessionHandler.EHSAccountGetFromSession(FunctionCode)

        Dim udtPreFill As New BLL.PreFillPersonalInformationBLL()

        udtPreFill.InsertPreFillConsent(sPreFill, _udtEHSAccount.EHSPersonalInformationList(0))

    End Sub

    Private Sub gvPrintOut_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPrintOut.RowDataBound
        If e.Row.RowIndex = 0 Then
            Dim lnkBtnEngConsentForm As LinkButton = e.Row.FindControl("lnkBtnEngConsentForm")
            Dim lnkBtnChiConsentForm As LinkButton = e.Row.FindControl("lnkBtnChiConsentForm")

            lnkBtnEngConsentForm.Attributes.Add("onclick", "javascript:openNewWin('Printout/EHSPrefilledClaimForm_RV.aspx');return false;")
            lnkBtnChiConsentForm.Attributes.Add("onclick", "javascript:openNewWin('Printout/EHSPrefilledClaimForm_CHI_RV.aspx');return false;")
        End If
    End Sub

    Private Sub ibtnCompleteClose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCompleteClose.Click
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Pre-Fill Consent No.", _udtSessionHandler.PreFillConsentIDGetFormSession(FunctionCode))

        ' CRE11-004   
        Dim udtEHSAccount As EHSAccountModel = _udtSessionHandler.EHSAccountGetFromSession(FunctionCode)
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)
        Dim objAuditLogInfo As New AuditLogInfo(Nothing, Nothing, udtEHSAccount.AccountSourceString, udtEHSAccount.VoucherAccID, udtEHSAccountPersonalInfo.DocCode, udtEHSAccountPersonalInfo.IdentityNum)
        ' End CRE11-004   

        udtAuditLogEntry.WriteLog(LogID.LOG00019, "Step 3 Complete", objAuditLogInfo)

        _udtSessionHandler.PreFillConsentIDRemoveFromSession(FunctionCode)
        _udtSessionHandler.PreFillSubmitTimeRemoveFromSession(FunctionCode)
        _udtSessionHandler.EHSAccountRemoveFromSession(FunctionCode)

        'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "javascript:window.opener = top;window.close();", True)

        Me.Response.Redirect("~/thankyou.aspx")

        'panFAQ.Visible = False

        'mvPreFill.ActiveViewIndex = 3


    End Sub

    Private Sub setToDoLabel()
        Dim selectedLang As String
        selectedLang = LCase(Session("language"))

        If selectedLang.Equals(English) Then
            Me.lblThingDo.Visible = True
            Me.lblThingPlease.Visible = False
        ElseIf selectedLang.Equals(TradChinese) Then
            Me.lblThingDo.Visible = False
            Me.lblThingPlease.Visible = True
        End If
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
End Class