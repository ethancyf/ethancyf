Imports HCSP.BLL
Imports Common.ComFunction
Imports Common.Component
Imports Common.ComObject
Imports Common.Validation
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.EHSTransaction
Imports Common.Component.Practice
Imports Common.Format
Imports Common.Component.DataEntryUser
Imports Common.Component.ServiceProvider
Imports Common.Component.UserAC
Imports Common.Component.DocType
Imports Common.Component.StaticData
Imports Common.ComFunction.ParameterFunction

Partial Public Class EHSAccountCreationV1
    Inherits BasePage

    Public FunctCode As String = String.Empty   'CRE20-0xx for COVID19 claim [Nichole]
    Public Const EHSClaim As String = "EHSClaimV1.aspx"

    Private _udtEHSAccount As EHSAccountModel
    Private _udtEHSClaimBLL As New EHSClaimBLL()
    Private _udtClaimRulesBLL As New ClaimRules.ClaimRulesBLL()
    Private _systemMessage As SystemMessage
    Private _udtFormatter As Formatter
    Private _validator As Validator = New Validator
    Private _udtEHSAccountBLL As EHSAccountBLL
    Private _udtPracticeAcctBLL As PracticeBankAcctBLL = New PracticeBankAcctBLL
    Private _udtSessionHandler As BLL.SessionHandler = New BLL.SessionHandler
    Private _strValidationFail As String = "ValidationFail"
    Private _udtUserAC As UserACModel = New UserACModel()
    Private _udtSP As ServiceProviderModel = Nothing
    Private _blnIsRequireHandlePageRefresh As Boolean = False
    Private _udtAuditLogEntry As AuditLogEntry

    Private Class PrintOptionValue
        Public Const Chi As String = "Chi"
        Public Const Eng As String = "Eng"
    End Class

    Private Class SessionName
        'Using in Enter Account detail screen
        'If PressedNext is true, screen will go to Confirm Detail "Step1b2" Page
        Public Const PressedNext As String = "PressedNext"

    End Class

    Private Class ActiveViewIndex
        'Get Account Creation Consent
        Public Const Step1a1 As Integer = 0

        'Get Get Existing Account Consent
        Public Const Step1a2 As Integer = 1

        'Enter Detail
        Public Const Step1b1 As Integer = 2

        'Confirm Detail
        Public Const Step1b2 As Integer = 3

        'Complete Account Creation
        Public Const Step1c As Integer = 4

        'Smart IC Consent
        Public Const Step1b3 As Integer = 5
    End Class
    'CRE20-0xx Immu Record [Start][Nichole]
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        If Me.ClaimMode = ClaimMode.COVID19 Then
            If _udtSessionHandler.Language = CultureLanguage.TradChinese Then
                Me.imgHeaderClaimVoucher.AlternateText = HttpContext.GetGlobalResourceObject("AlternateText", "ClaimCOVID19Banner", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))
                Me.imgHeaderClaimVoucher.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "ClaimCOVID19Banner", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))
                Me.lblClaimVoucherStep1b.Text = HttpContext.GetGlobalResourceObject("Text", "ClaimStep3Complete", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))

            ElseIf _udtSessionHandler.Language = CultureLanguage.SimpChinese Then
                Me.imgHeaderClaimVoucher.AlternateText = HttpContext.GetGlobalResourceObject("AlternateText", "ClaimCOVID19Banner", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese))
                Me.imgHeaderClaimVoucher.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "ClaimCOVID19Banner", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese))
                Me.lblClaimVoucherStep1b.Text = HttpContext.GetGlobalResourceObject("Text", "ClaimStep3Complete", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese))
            Else
                Me.imgHeaderClaimVoucher.AlternateText = HttpContext.GetGlobalResourceObject("AlternateText", "ClaimCOVID19Banner", New System.Globalization.CultureInfo(CultureLanguage.English))
                Me.imgHeaderClaimVoucher.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "ClaimCOVID19Banner", New System.Globalization.CultureInfo(CultureLanguage.English))
                Me.lblClaimVoucherStep1b.Text = HttpContext.GetGlobalResourceObject("Text", "ClaimStep3Complete", New System.Globalization.CultureInfo(CultureLanguage.English))

            End If
        End If
    End Sub
    'CRE20-0xx Immu Record [End][Nichole]

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        FunctCode = Me.ClaimFunctCode

        _udtAuditLogEntry = New AuditLogEntry(FunctCode, Me)

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        If Not IsPostBack Then
            ' Init the Active View in Page Load only
            Me.mvAccountCreation.ActiveViewIndex = 0

          
        End If

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

        Dim udtSelectedPracticeDisplay As PracticeDisplayModel = Nothing
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = Nothing
        Dim udtSmartIDContent As SmartIDContentModel = Nothing

        Me._udtEHSAccount = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)

        Dim udtSearchAccountStatus As EHSClaimBLL.SearchAccountStatus = _udtSessionHandler.SearchAccountStatusGetFormSession

        'Get Current USer Account for check Session Expired
        Me._udtUserAC = UserACBLL.GetUserAC

        If Not Me.IsPostBack Then
            EHSAccountCreationBase.AuditLogPageLoad(_udtAuditLogEntry)

            'Get the Current Practice ------------------------------------------------------------
            udtSelectedPracticeDisplay = Me._udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)

            If Me._udtSessionHandler.AccountCreationComeFromClaimGetFromSession() Then

                udtSmartIDContent = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode)
                If Not udtSmartIDContent Is Nothing AndAlso udtSmartIDContent.IsReadSmartID Then
                    '==================================================================== Code for SmartID ============================================================================
                    '--------------------------------------------------------------------------------------------------
                    'SmartID Case
                    '--------------------------------------------------------------------------------------------------
                    Select Case udtSmartIDContent.SmartIDReadStatus

                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                        ' ----------------------------------------------------------------------------------------
                        Case SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_DiffDOI_LargerDOI, _
                                SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_NotCreateBySmartID_SameDOB, _
                                SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOIDOB_CreateBySmartID_WithoutGender_SameName
                            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
                            udtSmartIDContent.EHSValidatedAccount = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
                            Me._udtSessionHandler.SmartIDContentSaveToSession(FunctCode, udtSmartIDContent)

                            Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1b3

                            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                            ' ----------------------------------------------------------------------------------------
                        Case SmartIDHandler.SmartIDResultStatus.DocTypeNotExist, _
                                SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_NotCreateBySmartID, _
                                SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_DiffDOIDOB, _
                                SmartIDHandler.SmartIDResultStatus.DocTypeNotExist, _
                                SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB_WithoutGender_SameName
                            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
                            Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1a1

                        Case SmartIDHandler.SmartIDResultStatus.TempAccountExist_SameDetail
                            If Me.ClaimMode = ClaimMode.COVID19 Then
                                ' Create new account
                                mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1a1
                            Else
                                If _udtEHSAccount.IsNew Then
                                    ' Create new account
                                    mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1a1
                                Else
                                    mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1a2
                                End If
                            End If
                    End Select
                    '==================================================================================================================================================================

                ElseIf _udtSessionHandler.FromVaccinationRecordEnquiryGetFromSession Then
                    '--------------------------------------------------------------------------------------------------
                    ' From Vaccination Record Enquiry
                    '--------------------------------------------------------------------------------------------------
                    If _udtEHSAccount.IsNew Then
                        'Go to create new account
                        mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1a1

                    Else
                        If _udtEHSAccount.SearchDocCode <> _udtEHSAccount.EHSPersonalInformationList(0).DocCode Then
                            'Doc Code not same, Go to create new account
                            mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1a1

                        Else
                            'EC Account with 5-key matched
                            '    => 1.Doc no.   2.Eng. name     3.DOB   4.Gender    5.Serial no.; OR

                            'Other Account(e.g HKIC,HKBC,VISA,...) with 4-key matched
                            '    => 1.Doc no.   2.Eng. name     3.DOB   4.Gender

                            'Go to confirm temporary account
                            mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1a2

                        End If

                    End If

                Else

                    '--------------------------------------------------------------------------------------------------
                    'Normal Case same as before
                    '--------------------------------------------------------------------------------------------------
                    If Me._udtEHSAccount.IsNew() Then
                        'Create new Temppary Account
                        Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1a1
                    Else
                        'Account is existing in databse
                        If Not Me._udtEHSAccount.SearchDocCode.Trim().Equals(Me._udtEHSAccount.EHSPersonalInformationList(0).DocCode.Trim()) Then
                            'Search DocCode != PersonalInfo DocCode -> show limited information to get consent ("Back", "Create Accoint", Check Case -> "Confirm")
                            Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1a1
                        Else
                            'show all information to get consent ("Back", "Confirm", "Modify")
                            Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1a2
                        End If
                    End If

                End If

                Me._udtSessionHandler.AccountCreationComeFromClaimRemoveFromSession()
            Else
                Me.BackToClaim(False)
            End If

            preventMultiImgClick(Me.ClientScript, Me.btnStep1b2Confirm)

        Else
            Me.StepRenderLanguage(Me._udtEHSAccount)
        End If

        If Me._blnIsRequireHandlePageRefresh Then

            Me.BackToClaim(False)
        End If

        'CRE20-xxx COVID-19 [Start][Nichole] 
        If Not IsPostBack Then
            If Me.ClaimMode = ClaimMode.COVID19 Then
                lblVRACreationStep1c.Visible = False
                ' lblClaimVoucherStep1b.Visible = False

                If Not udtSmartIDContent Is Nothing AndAlso udtSmartIDContent.IsReadSmartID Then
                    '-------------------------------
                    '--- smart IC
                    If Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1a2 Then
                        '-Validate smart IC
                        Me.btnStep1a2Confirm_Click(Nothing, Nothing)
                    ElseIf Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1a1 Then
                        Me.btnstep1a1CreateAccount_Click(Nothing, Nothing)
                    End If
                Else
                    '------------------------------
                    '--- manual input

                    If Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1a2 Then
                        Me._udtSessionHandler.ExceedDocTypeLimitRemoveFromSession()
                        Me._udtSessionHandler.NotMatchAccountExistRemoveFromSession()
                        Me._udtSessionHandler.CCCodeRemoveFromSession(FunctCode)
                        Me._udtSessionHandler.AccountCreationProceedClaimSaveToSession(True)

                        Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1a1
                    Else

                    End If

                    Me.btnstep1a1CreateAccount_Click(Nothing, Nothing)
                End If
            End If
        End If

        'CRE20-xxx COVID-19 [End][Nichole]

        Me.ModalPopupExtenderConfirmSelectPractice.PopupDragHandleControlID = Me.ucNoticePopUpConfirmSelectPractice.Header.ClientID

    End Sub

    Private Sub StepRenderLanguage(ByVal udtEHSAccount As EHSAccountModel)

        Select Case mvAccountCreation.ActiveViewIndex
            Case ActiveViewIndex.Step1a1
                'Hight Light Time Line
                Me.SetupStep1a1(udtEHSAccount)

            Case ActiveViewIndex.Step1a2
                'Hight Light Time Line
                Me.SetupStep1a2(udtEHSAccount)

            Case ActiveViewIndex.Step1b1
                'Hight Light Time Line
                Me.SetupStep1b1(udtEHSAccount, True, False)

            Case ActiveViewIndex.Step1b2
                'Hight Light Time Line
                Me.SetupStep1b2(udtEHSAccount)
                Me.EnableConfirmButton(Me.chkStep1b2Declare.Checked, Me.btnStep1b2Confirm)

            Case ActiveViewIndex.Step1c
                'Hight Light Time Line
                Me.SetupStep1c(udtEHSAccount, False)

            Case ActiveViewIndex.Step1b3
                '==================================================================== Code for SmartID ============================================================================
                Me.SetupStep1b3(udtEHSAccount, False)
                '==================================================================================================================================================================
        End Select

    End Sub

    Protected Sub mvAccountCreation_ActiveViewChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mvAccountCreation.ActiveViewChanged
        Dim strHightLight As String = "highlightTimelineLast"
        Dim strUnhightLight As String = "unhighlightTimelineLast"
        Me._udtEHSAccount = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)

        If Me._udtEHSAccount Is Nothing Then
            Me.BackToClaim(False)
            Return
        End If

        Me._udtSessionHandler.EHSClaimStepsSaveToSession(FunctCode, Me.mvAccountCreation.ActiveViewIndex)

        'Reset Time Line
        Me.panAccountCreationTimelineStep1a.CssClass = strUnhightLight
        Me.panAccountCreationTimelineStep1b.CssClass = strUnhightLight
        Me.panAccountCreationTimelineStep1c.CssClass = strUnhightLight

        Select Case mvAccountCreation.ActiveViewIndex
            Case ActiveViewIndex.Step1a1 'Get consent (Show Limited information Only)
                'Hight Light Time Line
                Me.panAccountCreationTimelineStep1a.CssClass = strHightLight
                Me.SetupStep1a1(Me._udtEHSAccount)

            Case ActiveViewIndex.Step1a2 'Get Exisiting Account Consent (Show all information Only)
                'Hight Light Time Line
                Me.panAccountCreationTimelineStep1a.CssClass = strHightLight
                Me.SetupStep1a2(Me._udtEHSAccount)

            Case ActiveViewIndex.Step1b1 'Enter account detail
                'Hight Light Time Line
                Me.panAccountCreationTimelineStep1b.CssClass = strHightLight
                Me.SetupStep1b1(Me._udtEHSAccount, True, True)

            Case ActiveViewIndex.Step1b2 'confirm Detail
                'Hight Light Time Line
                Me.panAccountCreationTimelineStep1b.CssClass = strHightLight
                Me.SetupStep1b2(Me._udtEHSAccount)
                Me.Step1b1ResetErrorImage()
                Me.chkStep1b2Declare.Checked = False
                Me.EnableConfirmButton(False, Me.btnStep1b2Confirm)

            Case ActiveViewIndex.Step1c 'complete account creation
                'Hight Light Time Line
                Me.panAccountCreationTimelineStep1c.CssClass = strHightLight
                Me.SetupStep1c(Me._udtEHSAccount, True)

            Case ActiveViewIndex.Step1b3
                '==================================================================== Code for SmartID ============================================================================
                'Hight Light Time Line
                Me.panAccountCreationTimelineStep1b.CssClass = strHightLight
                Me.SetupStep1b3(Me._udtEHSAccount, True)
                '==================================================================================================================================================================
        End Select
    End Sub

    Private Sub EnableConfirmButton(ByVal enable As Boolean, ByVal confirmButton As ImageButton)
        If enable Then
            confirmButton.Enabled = enable
            confirmButton.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmBtn")
        Else
            confirmButton.Enabled = enable
            confirmButton.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmDisableBtn")
        End If
    End Sub

#Region "Modal Popup Extender"
    '------------------------------------------------------------------------------------------------------------------------------------------------------------
    'Confirm Cancel
    '------------------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub btnPopupConfirmCancelConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPopupConfirmCancelConfirm.Click
        Me.BackToClaim(False)
    End Sub

    Private Sub btnPopupConfirmCancelCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPopupConfirmCancelCancel.Click

    End Sub

    '------------------------------------------------------------------------------------------------------------------------------------------------------------
    'Confirm Modify Temporay Account
    '------------------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub btnPopupConfirmModifyYes_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPopupConfirmModifyYes.Click
        'Me._udtEHSAccount = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Dim udtScheme As Scheme.SchemeClaimModel
        Dim systemMessage As SystemMessage
        Dim udtSmartIDContent As BLL.SmartIDContentModel = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode)

        udtScheme = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        If Not udtSmartIDContent Is Nothing Then
            '==================================================================== Code for SmartID ============================================================================
            Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1b3
            EHSAccountCreationBase.AuditLogStep1a2ConfirmModifyAccountBySmartID(_udtAuditLogEntry, FunctCode, udtSmartIDContent)
            '==================================================================================================================================================================
        Else
            Me._udtEHSAccount = Me._udtEHSAccount.CloneData()
            Me._udtEHSAccount.SchemeCode = udtScheme.SchemeCode

            'For CCCode is not nothing only--------------------------------------------------------------------------------------------
            'Save CCCode to session. If account detial is no change, Chinese Name Popup box will not display and pass cccode validation
            '---------------------------------------------------------------------------------------------------------------------------
            If Me._udtEHSAccount.EHSPersonalInformationList(0).DocCode.Trim() = DocTypeModel.DocTypeCode.HKIC Then
                Me.udcChooseCCCode.CCCode1 = Me._udtEHSAccount.EHSPersonalInformationList(0).CCCode1
                Me.udcChooseCCCode.CCCode2 = Me._udtEHSAccount.EHSPersonalInformationList(0).CCCode2
                Me.udcChooseCCCode.CCCode3 = Me._udtEHSAccount.EHSPersonalInformationList(0).CCCode3
                Me.udcChooseCCCode.CCCode4 = Me._udtEHSAccount.EHSPersonalInformationList(0).CCCode4
                Me.udcChooseCCCode.CCCode5 = Me._udtEHSAccount.EHSPersonalInformationList(0).CCCode5
                Me.udcChooseCCCode.CCCode6 = Me._udtEHSAccount.EHSPersonalInformationList(0).CCCode6
                systemMessage = Me.udcChooseCCCode.BindCCCode()

                If systemMessage Is Nothing Then
                    Me._udtEHSAccount.EHSPersonalInformationList(0).CName = Me.udcChooseCCCode.GetChineseName(FunctCode, True)
                End If

            End If

            EHSAccountCreationBase.AuditLogStep1a2ConfirmModifyAccount(_udtAuditLogEntry, FunctCode)

            Me._udtSessionHandler.EHSAccountSaveToSession(Me._udtEHSAccount, FunctCode)
            Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1b1
        End If
    End Sub

    Private Sub btnPopupConfirmModifyNo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPopupConfirmModifyNo.Click

    End Sub

    '------------------------------------------------------------------------------------------------------------------------------------------------------------
    'CCCode Selection
    '------------------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub udcChooseCCCode_Cancel(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcChooseCCCode.Cancel
        Dim udtAuditLogEntry As New AuditLogEntry(FunctCode, Me)
        EHSAccountCreationBase.AuditLogStep1b1PromptCCCodeCancel(udtAuditLogEntry)
    End Sub

    Private Sub udcChooseCCCode_Confirm(ByVal strDocCode As String, ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcChooseCCCode.Confirm
        Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Dim strChineseName As String = String.Empty

        Select Case strDocCode
            Case DocTypeModel.DocTypeCode.HKIC
                Dim udcInputHKIC As ucInputHKID = Me.udcStep1b1InputDocumentType.GetHKICControl

                udcInputHKIC.SetProperty(ucInputDocTypeBase.BuildMode.Creation)
                Me.udcChooseCCCode.CCCode1 = udcInputHKIC.CCCode1
                Me.udcChooseCCCode.CCCode2 = udcInputHKIC.CCCode2
                Me.udcChooseCCCode.CCCode3 = udcInputHKIC.CCCode3
                Me.udcChooseCCCode.CCCode4 = udcInputHKIC.CCCode4
                Me.udcChooseCCCode.CCCode5 = udcInputHKIC.CCCode5
                Me.udcChooseCCCode.CCCode6 = udcInputHKIC.CCCode6

                'Get Chinese From drop down list, and Save CCCode in Session
                strChineseName = Me.udcChooseCCCode.GetChineseName(FunctCode, True)
                udcInputHKIC.SetCName(strChineseName)

            Case DocTypeModel.DocTypeCode.ROP140
                Dim udcInputROP140 As ucInputROP140 = Me.udcStep1b1InputDocumentType.GetROP140Control

                udcInputROP140.SetProperty(ucInputDocTypeBase.BuildMode.Creation)
                Me.udcChooseCCCode.CCCode1 = udcInputROP140.CCCode1
                Me.udcChooseCCCode.CCCode2 = udcInputROP140.CCCode2
                Me.udcChooseCCCode.CCCode3 = udcInputROP140.CCCode3
                Me.udcChooseCCCode.CCCode4 = udcInputROP140.CCCode4
                Me.udcChooseCCCode.CCCode5 = udcInputROP140.CCCode5
                Me.udcChooseCCCode.CCCode6 = udcInputROP140.CCCode6

                'Get Chinese From drop down list, and Save CCCode in Session
                strChineseName = Me.udcChooseCCCode.GetChineseName(FunctCode, True)
                udcInputROP140.SetCName(strChineseName)

        End Select

        Dim udtAuditLogEntry As New AuditLogEntry(FunctCode, Me)
        udtAuditLogEntry.AddDescripton("Chinese Name", strChineseName)

        EHSAccountCreationBase.AuditLogStep1b1PromptCCCodeConfirm(udtAuditLogEntry)

        udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode).CName = strChineseName
        Me._udtSessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctCode)

        Me.ModalPopupExtenderChooseCCCode.Hide()

        Dim blnPressedNext As Boolean = False

        If Not IsNothing(Me.Session(SessionName.PressedNext)) Then
            'Since CCCode is incorrect and user pressed "Next" Button in step1b1 (Enter Account Detail) page
            blnPressedNext = CType(Me.Session(SessionName.PressedNext), Boolean)

            If blnPressedNext Then
                Me.Session.Remove(SessionName.PressedNext)
                Me.btnStep1b1Next_Click(Nothing, Nothing)
            End If

        End If

    End Sub

    '------------------------------------------------------------------------------------------------------------------------------------------------------------
    'Practice Selection
    '------------------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub udcPracticeRadioButtonGroup_PracticeSelected(ByVal strPracticeName As String, ByVal strBankAcctNo As String, ByVal intBankAccountDisplaySeqas As Integer, ByVal strSchemeCode As String, ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcPracticeRadioButtonGroup.PracticeSelected
        Dim isValidForCreation As Boolean = True
        Dim udtPracticeDisplays As BLL.PracticeDisplayModelCollection = Nothing
        Dim udtSchemeClaimModelCollection As Scheme.SchemeClaimModelCollection = Nothing
        Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Dim udtPracticedisplay As BLL.PracticeDisplayModel = Me._udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)
        Dim udtSelectedPracticedisplay As BLL.PracticeDisplayModel = Nothing
        Dim udtSchemeClaimBLL As Scheme.SchemeClaimBLL = New Scheme.SchemeClaimBLL()
        Dim udtSchemeClaim As Scheme.SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        ' CRE20-023  (Immu record) [Start][Raiman]
        'Get sender(which is image button created by PracticeRadioButtonGroup.vb) contain attributes. and pass attribute value to popup ok button for calling PracticeRadioButtonGroup_PracticeSelected sub again.
        Dim selectPracticeImageButton As ImageButton = CType(sender, ImageButton)
        Dim IsContainCovid19Scheme As Boolean = CType(selectPracticeImageButton.Attributes("blnShowPopUp"), Boolean)

        If (Me.ClaimMode = Common.Component.ClaimMode.COVID19 AndAlso Not _udtSessionHandler.ConfirmedPracticePopUpGetFromSession AndAlso IsContainCovid19Scheme) Then
            ModalPopupExtenderConfirmSelectPractice.Show()
            ModalPopupPracticeSelection.Show()

            ucNoticePopUpConfirmSelectPractice.ButtonOK.Attributes("DataTextField") = selectPracticeImageButton.Attributes("DataTextField")
            ucNoticePopUpConfirmSelectPractice.ButtonOK.Attributes("DataValueField") = selectPracticeImageButton.Attributes("DataValueField")
            ucNoticePopUpConfirmSelectPractice.ButtonOK.Attributes("PracticeDisplaySeq") = selectPracticeImageButton.Attributes("PracticeDisplaySeq")
            ucNoticePopUpConfirmSelectPractice.ButtonOK.Attributes("PracticeDisplayText") = selectPracticeImageButton.Attributes("PracticeDisplayText")
            ucNoticePopUpConfirmSelectPractice.ButtonOK.Attributes("blnShowPopUp") = selectPracticeImageButton.Attributes("blnShowPopUp")
            ucNoticePopUpConfirmSelectPractice.ButtonOK.Attributes("PopupCallFrom") = udcPracticeRadioButtonGroup.ClientID

            ucNoticePopUpConfirmSelectPractice.MessageText = String.Format(HttpContext.GetGlobalResourceObject("Text", "SelectPracticePopup"), selectPracticeImageButton.Attributes("PracticeDisplayText"))

        Else
            ' CRE20-023  (Immu record) [End][Raiman]



            udtPracticeDisplays = Me._udtSessionHandler.PracticeDisplayListGetFromSession(FunctCode)
            udtSelectedPracticedisplay = udtPracticeDisplays.Filter(intBankAccountDisplaySeqas)
            Me._udtSessionHandler.PracticeDisplaySaveToSession(udtSelectedPracticedisplay, FunctCode)

            'this session for always show select practice popup.
            _udtSessionHandler.ConfirmedPracticePopUpSaveToSession(False)

            ' Save the new selected Scheme to session
            _udtSessionHandler.SchemeSelectedForPracticeSaveToSession(strSchemeCode, FunctCode)

            Select Case Me.mvAccountCreation.ActiveViewIndex
                Case ActiveViewIndex.Step1b1
                    Dim udtAuditLogEntry As New AuditLogEntry(FunctCode, Me)

                    'if Selected practice is not same as the current practice
                    If Not udtPracticedisplay.PracticeID.Equals(intBankAccountDisplaySeqas) Then

                        ' CRE20-0022 (Immu record) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                        ' Practice Scheme Info List Filter by COVID-19
                        Dim udtFilterPracticeSchemeInfoList As PracticeSchemeInfo.PracticeSchemeInfoModelCollection = (New Scheme.SchemeClaimBLL).FilterPracticeSchemeInfo(Me._udtSP.PracticeList, intBankAccountDisplaySeqas, Me.ClaimMode)

                        udtSchemeClaimModelCollection = udtSchemeClaimBLL.searchValidClaimPeriodSchemeClaimByPracticeSchemeInfoSubsidizeCode(udtFilterPracticeSchemeInfoList, Me._udtSP.SchemeInfoList)
                        ' CRE20-0022 (Immu record) [End][Chris YIM]

                        udtSchemeClaim = udtSchemeClaimModelCollection.Filter(udtSchemeClaim.SchemeCode)

                        'Practice do not have this scheme for creation account -> show pop up message message
                        If udtSchemeClaim Is Nothing Then
                            EHSAccountCreationBase.AuditLogPracticeSelected(udtAuditLogEntry, True, udtSelectedPracticedisplay, Nothing, False)
                            Me._udtSessionHandler.SchemeSelectedRemoveFromSession(FunctCode)

                            isValidForCreation = False
                        Else
                            EHSAccountCreationBase.AuditLogPracticeSelected(udtAuditLogEntry, True, udtSelectedPracticedisplay, udtSchemeClaim, True)
                        End If

                        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                        Me._udtSessionHandler.NonClinicSettingRemoveFromSession(FunctCode)
                        'Me._udtSessionHandler.DocumentaryProofForPIDRemoveFromSession(FunctCode)
                        Me._udtSessionHandler.PIDInstitutionCodeRemoveFromSession(FunctCode)
                        Me._udtSessionHandler.PlaceVaccinationRemoveFromSession(FunctCode)
                        Me._udtSessionHandler.PlaceVaccinationOtherRemoveFromSession(FunctCode)
                        Me._udtSessionHandler.ClaimCategoryRemoveFromSession(FunctCode)
                        'CRE16-002 (Revamp VSS) [End][Chris YIM]
                    Else
                        EHSAccountCreationBase.AuditLogPracticeSelected(udtAuditLogEntry, True, udtSelectedPracticedisplay, udtSchemeClaim, True)
                    End If

                    'case 1 : SP selected the same practice -> procced account normally
                    'case 2 : SP selected the diff practice and have same scheme -> procced account normally
                    If isValidForCreation Then


                        Me.SetupStep1b1(udtEHSAccount, False, False)
                    Else
                        Me.lblPopupConfirmOnlyMessage.Text = Me.GetGlobalResourceObject("Text", "PracticeNoAvailSchemeForEHSAccountCreateion")
                        Me.ModalPopupConfirmOnly.Show()
                    End If
            End Select


            ' CRE20-023  (Immu record) [Start][Raiman]
        End If
        ' CRE20-023  (Immu record) [End][Raiman]

    End Sub

    ' CRE20-0XX (HA Scheme) [Start][Winnie]
    Private Sub udcPracticeRadioButtonGroup_SchemeSelected() Handles udcPracticeRadioButtonGroup.SchemeSelected
        Me.ModalPopupPracticeSelection.Show()
    End Sub
    ' CRE20-0XX (HA Scheme) [End][Winnie]

    '------------------------------------------------------------------------------------------------------------------------------------------------------------
    'Confirm only Popup Box
    '------------------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub btnPopupConfirmOnlyConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPopupConfirmOnlyConfirm.Click
        Me.BackToClaim(False)
    End Sub


    Private Sub ucNoticePopUpConfirmSelectPractice_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUpConfirmSelectPractice.ButtonClick
        Select Case e
            Case ucNoticePopUp.enumButtonClick.OK
                Dim imageButton As ImageButton = New ImageButton
                _udtSessionHandler.ConfirmedPracticePopUpSaveToSession(True)
                imageButton.Attributes("DataTextField") = ucNoticePopUpConfirmSelectPractice.ButtonOK.Attributes("DataTextField")
                imageButton.Attributes("DataValueField") = ucNoticePopUpConfirmSelectPractice.ButtonOK.Attributes("DataValueField")
                imageButton.Attributes("PracticeDisplaySeq") = ucNoticePopUpConfirmSelectPractice.ButtonOK.Attributes("PracticeDisplaySeq")
                imageButton.Attributes("PracticeDisplayText") = ucNoticePopUpConfirmSelectPractice.ButtonOK.Attributes("PracticeDisplayText")
                imageButton.Attributes("blnShowPopUp") = ucNoticePopUpConfirmSelectPractice.ButtonOK.Attributes("blnShowPopUp")

                udcPracticeRadioButtonGroup.btnPracticeSelection_click(imageButton, Nothing)
            Case Else
                If (ucNoticePopUpConfirmSelectPractice.ButtonOK.Attributes("PopupCallFrom") = udcPracticeRadioButtonGroup.ClientID) Then
                    ModalPopupPracticeSelection.Show()
                End If
                ' Do nothing
        End Select

    End Sub

#End Region

#Region "Step 1a1 Get Consent"

    'Private Sub Step1a1RenderLanguage(ByVal udtEHSAccount As EHSAccountModel)
    '    Dim udtDocTypeBLL As New DocTypeBLL()
    '    Dim udtDocTypeModelList As DocTypeModelCollection = udtDocTypeBLL.getAllDocType()

    '    If Me._udtSessionHandler.Language() = Common.Component.CultureLanguage.TradChinese Then
    '        Me.lblstep1a1DocumentType.Text = udtDocTypeModelList.Filter(udtEHSAccount).DocNameChi
    '    Else
    '        Me.lblstep1a1DocumentType.Text = udtDocTypeModelList.Filter(udtEHSAccount.SearchDocCode).DocName
    '    End If
    'End Sub

    Protected Sub btnstep1a1CreateAccount_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnstep1a1CreateAccount.Click
        Dim udtDataEntry As DataEntryUserModel = Nothing
        Dim udtEHSAccount As EHSAccountModel = Nothing
        Dim strDocCode As String = String.Empty
        Dim udtScheme As Scheme.SchemeClaimModel
        Dim udtSmartIDContent As BLL.SmartIDContentModel = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode)

        Me._udtSessionHandler.ExceedDocTypeLimitRemoveFromSession()
        Me._udtSessionHandler.CurrentUserGetFromSession(Me._udtSP, udtDataEntry)

        EHSAccountCreationBase.AuditLogStep1a1CreateAccountStart(_udtAuditLogEntry)

        Me._udtEHSAccount = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)

        Dim udtVREEHSAccount As EHSAccountModel = _udtSessionHandler.VREEHSAccountGetFromSession

        If Not IsNothing(udtVREEHSAccount) Then
            _udtEHSAccount = udtVREEHSAccount
            _udtSessionHandler.EHSAccountSaveToSession(_udtEHSAccount, FunctCode)
        End If

        If Not udtSmartIDContent Is Nothing Then
            '==================================================================== Code for SmartID ============================================================================
            Select Case udtSmartIDContent.SmartIDReadStatus

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Case SmartIDHandler.SmartIDResultStatus.DocTypeNotExist, _
                        SmartIDHandler.SmartIDResultStatus.EHSAccountNotfound, _
                        SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_DiffDOIDOB, _
                        SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_NotCreateBySmartID, _
                        SmartIDHandler.SmartIDResultStatus.TempAccountExist_SameDetail, _
                        SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB_WithoutGender_SameName
                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                    Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1b3
                    EHSAccountCreationBase.AuditLogStep1a1CreateAccountEndBySmartID(_udtAuditLogEntry, udtSmartIDContent, "Go to Step1b3 (Confirm Account)")
                Case Else
                    EHSAccountCreationBase.AuditLogStep1a1CreateAccountEndBySmartID(_udtAuditLogEntry, udtSmartIDContent, "Action is not Expected in this page.")
                    'No case Unit Now
                    Me.BackToClaim(False)
            End Select
            '===================================================================================================================================================================

        ElseIf _udtSessionHandler.FromVaccinationRecordEnquiryGetFromSession Then
            If Not _udtEHSAccount.IsNew Then
                ' Validated account found but different document type --> Clone new
                With Me._udtEHSAccount.EHSPersonalInformationList(0)
                    udtScheme = _udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

                    udtEHSAccount = _udtEHSClaimBLL.ConstructEHSTemporaryVoucherAccount(.IdentityNum, _udtEHSAccount.SearchDocCode, .ExactDOB, .DOB, _
                                        udtScheme.SchemeCode, .AdoptionPrefixNum, .ENameFirstName, .ENameSurName, .Gender)
                    udtEHSAccount.SetSearchDocCode(_udtEHSAccount.SearchDocCode)

                    _udtSessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctCode)

                End With

            End If

            Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1b1

        Else

            '1) DB have the record which is same document Identity No but different DOB 
            '2) For CIVSS Only, crosse search for HKID -> HKBC OR HKBC -> HKID
            If Me._udtSessionHandler.NotMatchAccountExistGetFromSession() OrElse _
                 Not Me._udtEHSAccount.SearchDocCode.Trim().Equals(Me._udtEHSAccount.EHSPersonalInformationList(0).DocCode.Trim()) Then
                EHSAccountCreationBase.AuditLogStep1a1CreateAccountEnd(_udtAuditLogEntry, "Doc No. found + [DOB not match OR Doc Code not match]")

                udtScheme = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
                If Me._udtEHSAccount.SearchDocCode = DocType.DocTypeModel.DocTypeCode.EC Then
                    With Me._udtEHSAccount.EHSPersonalInformationList(0)
                        If .ECAge.HasValue Then
                            ' Age on Date of Registration
                            udtEHSAccount = Me._udtEHSClaimBLL.ConstructEHSTemporaryVoucherAccount(.IdentityNum, Me._udtEHSAccount.SearchDocCode, .ECAge.Value, .ECDateOfRegistration.Value, udtScheme.SchemeCode)
                            udtEHSAccount.SetSearchDocCode(Me._udtEHSAccount.SearchDocCode)
                        Else
                            udtEHSAccount = Me._udtEHSClaimBLL.ConstructEHSTemporaryVoucherAccount(.IdentityNum, Me._udtEHSAccount.SearchDocCode, .ExactDOB, .DOB, udtScheme.SchemeCode, .AdoptionPrefixNum)
                            udtEHSAccount.SetSearchDocCode(Me._udtEHSAccount.SearchDocCode)
                        End If
                    End With
                Else
                    With Me._udtEHSAccount.EHSPersonalInformationList(0)
                        udtEHSAccount = Me._udtEHSClaimBLL.ConstructEHSTemporaryVoucherAccount(.IdentityNum, Me._udtEHSAccount.SearchDocCode, .ExactDOB, .DOB, udtScheme.SchemeCode, .AdoptionPrefixNum)
                        udtEHSAccount.SetSearchDocCode(Me._udtEHSAccount.SearchDocCode)
                    End With

                End If

                Me._udtSessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctCode)
            Else
                EHSAccountCreationBase.AuditLogStep1a1CreateAccountEnd(_udtAuditLogEntry, "Doc No. not found")
            End If

            Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1b1
        End If
    End Sub

    Protected Sub btnStep1aBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnstep1a1Back.Click
        Me.BackToClaim(False)
    End Sub

    Private Sub SetupStep1a1(ByVal udtEHSAccount As EHSAccountModel)
        If Me._udtSessionHandler.EHSClaimStepsGetFromSession(FunctCode) <> ActiveViewIndex.Step1a1 Then
            Me._blnIsRequireHandlePageRefresh = True
            Return
        End If

        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)
        Me._udtFormatter = New Formatter()
        Dim udtDocTypeBLL As DocType.DocTypeBLL = New DocType.DocTypeBLL()
        Dim udtDocTypeModelList As DocType.DocTypeModelCollection
        Dim strDocumentTypeFullName As String
        Dim strDocIdentityDesc As String
        Dim strDocCode As String = Me._udtEHSAccount.SearchDocCode.Trim()
        Dim udtSmartIDContent As BLL.SmartIDContentModel = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode)
        Dim enumSmartIDStatus As SmartIDHandler.SmartIDResultStatus = SmartIDHandler.SmartIDResultStatus.Empty

        'Control init
        Me.lblstep1a1HeaderText.Text = Me.GetGlobalResourceObject("Text", "VRACreationTempAcctCreate")
        'Me.btnstep1a1Confirm.Visible = False
        Me.lblstep1a1DocumentType.BackColor = Drawing.Color.Transparent

        ' If from Vaccination Record Enquiry
        Dim udtVREEHSAccount As EHSAccountModel = _udtSessionHandler.VREEHSAccountGetFromSession

        If Not IsNothing(udtVREEHSAccount) AndAlso udtVREEHSAccount.EHSPersonalInformationList(0).ENameSurName <> String.Empty AndAlso IsNothing(udtSmartIDContent) Then
            ' Fill Name in English and Gender
            lblstep1a1ENameText.Visible = True
            lblstep1a1EName.Visible = True
            lblstep1a1GenderText.Visible = True
            lblstep1a1Gender.Visible = True
            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
            ' ----------------------------------------------------------
            Trstep1a1ECSerialNo.Visible = False
            ' CRE18-001(CIMS Vaccination Sharing) [End][Koala CHENG]

            Dim udtVREEHSPersonalInfo As EHSPersonalInformationModel = udtVREEHSAccount.EHSPersonalInformationList(0)

            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
            ' ----------------------------------------------------------
            If udtVREEHSPersonalInfo.DocCode = DocType.DocTypeModel.DocTypeCode.EC Then
                ' EC Serial No.
                If udtEHSAccountPersonalInfo.DocCode = DocType.DocTypeModel.DocTypeCode.EC Then
                    If udtEHSAccountPersonalInfo.ECSerialNoNotProvided = False Then
                        lblstep1a1ECSerialNo.Text = udtEHSAccountPersonalInfo.ECSerialNo
                    Else
                        lblstep1a1ECSerialNo.Text = Me.GetGlobalResourceObject("Text", "NotProvided")
                    End If
                    Trstep1a1ECSerialNo.Visible = True
                Else
                    Trstep1a1ECSerialNo.Visible = False
                End If
            End If
            ' CRE18-001(CIMS Vaccination Sharing) [End][Koala CHENG]

            ' Name in English
            lblstep1a1ENameText.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
            lblstep1a1EName.Text = _udtFormatter.formatEnglishName(udtVREEHSPersonalInfo.ENameSurName, udtVREEHSPersonalInfo.ENameFirstName)

            ' Gender
            lblstep1a1GenderText.Text = Me.GetGlobalResourceObject("Text", "Gender")

            If udtVREEHSPersonalInfo.Gender = "M" Then
                lblstep1a1Gender.Text = Me.GetGlobalResourceObject("Text", "GenderMale")
            Else
                lblstep1a1Gender.Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
            End If

        Else
            lblstep1a1ENameText.Visible = False
            lblstep1a1EName.Visible = False
            lblstep1a1GenderText.Visible = False
            lblstep1a1Gender.Visible = False
            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
            ' ----------------------------------------------------------
            Trstep1a1ECSerialNo.Visible = False
            ' CRE18-001(CIMS Vaccination Sharing) [End][Koala CHENG]
        End If

        If Not udtEHSAccount Is Nothing Then
            'Get Documnet type full name
            udtDocTypeModelList = udtDocTypeBLL.getAllDocType()

            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            strDocumentTypeFullName = udtDocTypeModelList.Filter(strDocCode).DocName(Me._udtSessionHandler.Language)
            strDocIdentityDesc = udtDocTypeModelList.Filter(strDocCode).DocIdentityDesc(Me._udtSessionHandler.Language)
            ' CRE20-0022 (Immu record) [End][Chris YIM]

            Me.lblstep1a1DocumentType.Text = strDocumentTypeFullName
            Me.lblstep1a1HKIDText.Text = strDocIdentityDesc

            Select Case strDocCode
                Case DocType.DocTypeModel.DocTypeCode.HKIC, DocType.DocTypeModel.DocTypeCode.EC
                    Me.lblstep1a1HKID.Text = Me._udtFormatter.formatHKID(udtEHSAccountPersonalInfo.IdentityNum, False)
                Case DocType.DocTypeModel.DocTypeCode.HKBC, DocTypeModel.DocTypeCode.CCIC, DocTypeModel.DocTypeCode.ROP140 ' CRE20-0022 (Immu record) [Martin]
                    'Me.lblstep1a1HKIDText.Text = Me.GetGlobalResourceObject("Text", "RegistrationNo")
                    Me.lblstep1a1HKID.Text = Me._udtFormatter.formatHKID(udtEHSAccountPersonalInfo.IdentityNum, False)

                Case DocTypeModel.DocTypeCode.REPMT
                    'Me.lblstep1a1HKIDText.Text = Me.GetGlobalResourceObject("Text", "ReentryPermitNo")
                    Me.lblstep1a1HKID.Text = Me._udtFormatter.FormatDocIdentityNoForDisplay(udtEHSAccountPersonalInfo.DocCode, udtEHSAccountPersonalInfo.IdentityNum, False)

                Case DocTypeModel.DocTypeCode.VISA
                    'Me.lblstep1a1HKIDText.Text = Me.GetGlobalResourceObject("Text", "VisaRefNo")
                    Me.lblstep1a1HKID.Text = Me._udtFormatter.FormatDocIdentityNoForDisplay(udtEHSAccountPersonalInfo.DocCode, udtEHSAccountPersonalInfo.IdentityNum, False)

                Case DocTypeModel.DocTypeCode.ID235B
                    'Me.lblstep1a1HKIDText.Text = Me.GetGlobalResourceObject("Text", "BirthEntryNo")
                    Me.lblstep1a1HKID.Text = Me._udtFormatter.FormatDocIdentityNoForDisplay(udtEHSAccountPersonalInfo.DocCode, udtEHSAccountPersonalInfo.IdentityNum, False)

                Case DocType.DocTypeModel.DocTypeCode.ADOPC
                    ' Me.lblstep1a1HKIDText.Text = Me.GetGlobalResourceObject("Text", "NoOfEntry")
                    Me.lblstep1a1HKID.Text = Me._udtFormatter.FormatDocIdentityNoForDisplay(udtEHSAccountPersonalInfo.DocCode, udtEHSAccountPersonalInfo.IdentityNum, False, udtEHSAccountPersonalInfo.AdoptionPrefixNum)

                Case DocTypeModel.DocTypeCode.DI, DocTypeModel.DocTypeCode.PASS ' CRE20-0022 (Immu record) [Martin]
                    'Me.lblstep1a1HKIDText.Text = Me.GetGlobalResourceObject("Text", "IdentityDocNo")
                    Me.lblstep1a1HKID.Text = Me._udtFormatter.FormatDocIdentityNoForDisplay(udtEHSAccountPersonalInfo.DocCode, udtEHSAccountPersonalInfo.IdentityNum, False)
                    'Me.lblstep1a1HKID.Text = udtEHSAccountPersonalInfo.IdentityNum
            End Select


            If Not udtSmartIDContent Is Nothing AndAlso udtSmartIDContent.IsReadSmartID Then
                '==================================================================== Code for SmartID ============================================================================
                '---------------------------------------------------------------------------------------------------------------------------------------------
                ' If come form smart ID, DOB will not show on consent page
                '---------------------------------------------------------------------------------------------------------------------------------------------
                Me.lblstep1a1DOBText.Visible = False
                Me.lblstep1a1DOB.Visible = False
                enumSmartIDStatus = udtSmartIDContent.SmartIDReadStatus
                '==================================================================================================================================================================
            Else
                Me.lblstep1a1DOBText.Visible = True
                Me.lblstep1a1DOBText.Text = Me.GetGlobalResourceObject("Text", "DOB")

                Me.lblstep1a1DOB.Visible = True
                Me.lblstep1a1DOB.Text = Me._udtFormatter.formatDOB(udtEHSAccountPersonalInfo.DOB, udtEHSAccountPersonalInfo.ExactDOB, Me._udtSessionHandler.Language, _
                udtEHSAccountPersonalInfo.ECAge, udtEHSAccountPersonalInfo.ECDateOfRegistration)

                ' CRE16-012 Removal of DOB InWord [Start][Winnie]
                'If Not IsNothing(udtVREEHSAccount) Then
                'Dim udtVREEHSPersonalInfo As EHSPersonalInformationModel = udtVREEHSAccount.EHSPersonalInformationList(0)

                '' Add "in word" to Date of Birth (HKBC and ADOPC)
                'If udtVREEHSPersonalInfo.DocCode = DocTypeModel.DocTypeCode.HKBC OrElse udtVREEHSPersonalInfo.DocCode = DocTypeModel.DocTypeCode.ADOPC Then
                '    If udtVREEHSPersonalInfo.ExactDOB = "T" OrElse udtVREEHSPersonalInfo.ExactDOB = "U" OrElse udtVREEHSPersonalInfo.ExactDOB = "V" Then
                '        Dim udtStaticDataModel As StaticDataModel = (New StaticDataBLL).GetStaticDataByColumnNameItemNo("DOBInWordType", udtVREEHSPersonalInfo.OtherInfo)

                '        Select Case LCase(_udtSessionHandler.Language)
                '            Case CultureLanguage.English
                '                lblstep1a1DOB.Text = udtStaticDataModel.DataValue.ToString.Trim + " " + lblstep1a1DOB.Text
                '            Case CultureLanguage.TradChinese
                '                lblstep1a1DOB.Text = udtStaticDataModel.DataValueChi.ToString.Trim + " " + lblstep1a1DOB.Text
                '            Case CultureLanguage.SimpChinese
                '                lblstep1a1DOB.Text = udtStaticDataModel.DataValueCN.Trim + " " + lblstep1a1DOB.Text
                '        End Select

                '    End If
                'End If
                'End If
                ' CRE16-012 Removal of DOB InWord [End][Winnie]
            End If

            '==================================================================== Code for SmartID ============================================================================
            '---------------------------------------------------------------------------------------------------------------------------------------------
            ' Consent message will be different, if read by smart ID
            '---------------------------------------------------------------------------------------------------------------------------------------------
            If Me._udtEHSAccount.IsNew() Then

                If Not Me._udtSessionHandler.NotMatchAccountExistGetFromSession() OrElse enumSmartIDStatus = SmartIDHandler.SmartIDResultStatus.EHSAccountNotfound Then
                    Dim udtSearchAccountStatus As EHSClaimBLL.SearchAccountStatus = _udtSessionHandler.SearchAccountStatusGetFormSession

                    If Not IsNothing(udtSearchAccountStatus) AndAlso udtSearchAccountStatus.TempAccountInputDetailDiffFound Then
                        ' [A temporary eHealth account with the same Identity Document No. is found in the system. Please conduct another search or create a new eHealth account.]
                        Me.lblstep1a1DisclaimerNotice.Text = Me.GetGlobalResourceObject("Text", "CreateTempAccExistDisclaim")

                    Else
                        ' No account found -> Create New Account
                        ' [There is no record of this account. Please obtain consent from the applicant to provide personal information to create a temporary account.]
                        Me.lblstep1a1DisclaimerNotice.Text = Me.GetGlobalResourceObject("Text", "CreateTempAccNotExistDisclaim")

                    End If

                Else
                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    Select Case enumSmartIDStatus
                        Case SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_NotCreateBySmartID, _
                            SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_DiffDOIDOB, _
                            SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB_WithoutGender_SameName
                            Me.lblstep1a1DisclaimerNotice.Text = Me.GetGlobalResourceObject("Text", "TempACWithDiffInfo")

                        Case Else
                            'Record found but DOB/Detail not Match -> Create New Temp account
                            '[A temporary eHealth account with the same Identity Document No. is found in the system. Please conduct another search or create a new eHealth account.]
                            Me.lblstep1a1DisclaimerNotice.Text = Me.GetGlobalResourceObject("Text", "CreateTempAccExistDisclaim")
                    End Select
                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]                    
                End If

            Else
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Select Case enumSmartIDStatus
                    Case SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_NotCreateBySmartID, _
                        SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_DiffDOIDOB, _
                        SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB_WithoutGender_SameName
                        Me.lblstep1a1DisclaimerNotice.Text = Me.GetGlobalResourceObject("Text", "TempACWithDiffInfo")

                    Case Else
                        'Search Code = Personal Information DocCode -> go to Step 1b1 (get Consent with all information), Coding in page load
                        If Not Me._udtEHSAccount.EHSPersonalInformationList(0).DocCode.Trim().Equals(strDocCode) Then

                            If Me._udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                                Me.lblstep1a1DisclaimerNotice.Text = Me.GetGlobalResourceObject("Text", "ValidatedAccDiffDocTypeDisclaim").ToString()
                            ElseIf Me._udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.TemporaryAccount OrElse Me._udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.SpecialAccount Then
                                Me.lblstep1a1DisclaimerNotice.Text = Me.GetGlobalResourceObject("Text", "CreateTempAccDiffDocTypeDisclaim").ToString()
                            End If

                            'End If
                        End If
                End Select
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]                
            End If
            '==================================================================================================================================================================

        Else
            Me.lblstep1a1DocumentType.Text = String.Empty
            Me.lblstep1a1HKID.Text = String.Empty
            Me.lblstep1a1DOB.Text = String.Empty
        End If
        'Me.Step1a1RenderLanguage(udtEHSAccount)
    End Sub

#End Region

#Region "Step 1a2 Existing Account Consent"

    Protected Sub btnStep1a2Back_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnStep1a2Cancel.Click
        Me.BackToClaim(False)
    End Sub

    Protected Sub btnStep1a2Confirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnStep1a2Confirm.Click
        EHSAccountCreationBase.AuditLogStep1a2ConfirmAccount(_udtAuditLogEntry, FunctCode)
        Me.BackToClaim(True)
    End Sub

    Protected Sub btnStep1a2Modify_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnStep1a2Modify.Click
        EHSAccountCreationBase.AuditLogStep1a2ModifyAccount(_udtAuditLogEntry, FunctCode)

        Me.ModalPopupConfirmModify.Show()
    End Sub

    Private Sub SetupStep1a2(ByVal udtEHSAccount As EHSAccountModel)
        If Me._udtSessionHandler.EHSClaimStepsGetFromSession(FunctCode) <> ActiveViewIndex.Step1a2 Then
            Me._blnIsRequireHandlePageRefresh = True
            Return
        End If

        Dim strDocCode As String = udtEHSAccount.EHSPersonalInformationList(0).DocCode.Trim()
        Dim udtSmartIDContent As BLL.SmartIDContentModel = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode)

        '==================================================================== Code for SmartID ============================================================================
        If Not udtSmartIDContent Is Nothing Then

            ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Select Case udtSmartIDContent.SmartIDReadStatus
                Case SmartIDHandler.SmartIDResultStatus.TempAccountExist_SameDetail

                    If udtSmartIDContent.IdeasVersion = IdeasBLL.EnumIdeasVersion.TwoGender Or udtSmartIDContent.IdeasVersion = IdeasBLL.EnumIdeasVersion.ComboGender Then
                        'A temporary eHealth (Subsidies) Account with the same input information is found in the System. Please verify all information against those on the recipient's presented document.
                        Me.lblStep1a2TempAccDisclaimerText.Text = Me.GetGlobalResourceObject("Text", "ConfirmTempAccDisclaim")

                        udtSmartIDContent.HighLightGender = False
                    Else
                        'A temporary eHealth (Subsidies) Account with same HKIC No. has been located in the System. Please compare gender information with that shown on the recipient's Hong Kong Identity Card.
                        Me.lblStep1a2TempAccDisclaimerText.Text = Me.GetGlobalResourceObject("Text", "TempACWithSameInfo")

                        udtSmartIDContent.HighLightGender = True
                    End If
            End Select
            ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	
        Else
            Me.lblStep1a2TempAccDisclaimerText.Text = Me.GetGlobalResourceObject("Text", "ConfirmTempAccDisclaim")
        End If
        Me.udcStep1a2ReadOnlyDocumnetType.SmartIDContent = udtSmartIDContent
        '==================================================================================================================================================================

        Me.udcStep1a2ReadOnlyDocumnetType.DocumentType = strDocCode
        Me.udcStep1a2ReadOnlyDocumnetType.EHSAccount = udtEHSAccount
        Me.udcStep1a2ReadOnlyDocumnetType.Vertical = True
        Me.udcStep1a2ReadOnlyDocumnetType.MaskIdentityNo = True
        Me.udcStep1a2ReadOnlyDocumnetType.ShowAccountRefNo = True
        Me.udcStep1a2ReadOnlyDocumnetType.ShowTempAccountNotice = True
        Me.udcStep1a2ReadOnlyDocumnetType.ShowAccountCreationDate = False
        Me.udcStep1a2ReadOnlyDocumnetType.TableTitleWidth = 200

        If Not udtSmartIDContent Is Nothing _
                AndAlso udtSmartIDContent.IsReadSmartID _
                AndAlso SmartIDShowRealID() Then
            Me.udcStep1a2ReadOnlyDocumnetType.MaskIdentityNo = False
        End If

        Me.udcStep1a2ReadOnlyDocumnetType.Built()

        ' Show button (if using 4-key to search + matched record + no other information need to confirm -> Hide Modify button)
        If _udtSessionHandler.FromVaccinationRecordEnquiryGetFromSession AndAlso (New VaccinationBLL).DocumentContainOtherInformation(strDocCode) = False Then
            btnStep1a2Modify.Visible = False
        Else
            btnStep1a2Modify.Visible = True
        End If

        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ' Read Smart ID with gender, no other information need to confirm -> Hide Modify button
        If Not udtSmartIDContent Is Nothing AndAlso udtSmartIDContent.IsReadSmartID Then
            If udtSmartIDContent.IdeasVersion = IdeasBLL.EnumIdeasVersion.TwoGender Or udtSmartIDContent.IdeasVersion = IdeasBLL.EnumIdeasVersion.ComboGender Then
                btnStep1a2Modify.Visible = False
            End If
        End If
        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	
    End Sub

#End Region

#Region "Step 1b1 Enter Detail"

    Protected Sub btnStep1b1Next_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnStep1b1Next.Click
        Dim practiceDisplay As PracticeDisplayModel = Me._udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)
        Dim blnProceed As Boolean = True
        Dim systemMessage As SystemMessage
        Me._udtEHSAccount = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Me._udtEHSAccount.CreateSPPracticeDisplaySeq = practiceDisplay.PracticeID

        EHSAccountCreationBase.AuditLogStep1b1Start(_udtAuditLogEntry)

        Select Case Me._udtEHSAccount.SearchDocCode
            Case DocTypeModel.DocTypeCode.HKIC
                Dim udcInputHKIC As ucInputHKID = Me.udcStep1b1InputDocumentType.GetHKICControl()

                If udcInputHKIC.CCCodeIsEmpty() Then
                    udcInputHKIC.SetCName(String.Empty)
                    Me.udcChooseCCCode.Clean()
                    Me._udtSessionHandler.CCCodeRemoveFromSession(FunctCode)
                Else
                    udcInputHKIC.SetErrorImage(ucInputDocTypeBase.BuildMode.Creation, False)
                    udcInputHKIC.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

                    If udcInputHKIC.IsValidCCCodeInput() Then
                        'Check for select chinese name/CCCode 
                        'if cccode is changed (different between session value and input box), or incorrect CCCode
                        If Me.NeedPopupChineseNameDialog(DocTypeModel.DocTypeCode.HKIC) Then

                            systemMessage = Me.Step1b1ShowCCCodeSelection(udcInputHKIC, DocTypeModel.DocTypeCode.HKIC, True)
                            If Not systemMessage Is Nothing Then
                                Me.udcMsgBoxErr.AddMessage(systemMessage)
                            Else
                                EHSAccountCreationBase.AuditLogStep1b1PromptCCCode(_udtAuditLogEntry)

                            End If
                            blnProceed = False
                        End If

                    Else
                        Me.udcMsgBoxErr.AddMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00039)
                        udcInputHKIC.SetCCCodeError(True)
                        blnProceed = False

                    End If

                End If

                If blnProceed Then
                    'Fields checking for HKID only
                    blnProceed = Me.Step1b1HKICValdiation(Me._udtEHSAccount)
                End If
            Case DocTypeModel.DocTypeCode.EC
                'Fields checking for EC only
                blnProceed = Me.Step1b1ECValdiation(Me._udtEHSAccount)

            Case DocTypeModel.DocTypeCode.HKBC
                'Fields checking for HKBC only
                blnProceed = Me.Step1b1HKBCValidation(Me._udtEHSAccount)

            Case DocTypeModel.DocTypeCode.DI
                'Fields checking for DI only
                blnProceed = Me.Step1b1DIValidation(Me._udtEHSAccount)

            Case DocTypeModel.DocTypeCode.ID235B
                'Fields checking for ID235B only
                blnProceed = Me.Step1b1ID235BValidation(Me._udtEHSAccount)

            Case DocTypeModel.DocTypeCode.REPMT
                'Fields checking for REPMT only
                blnProceed = Me.Step1b1RepmtValidation(Me._udtEHSAccount)

            Case DocTypeModel.DocTypeCode.VISA
                'Fields checking for VISA only
                blnProceed = Me.Step1b1VISAValidation(Me._udtEHSAccount)

            Case DocTypeModel.DocTypeCode.ADOPC
                'Fields checking for Adoption only
                blnProceed = Me.Step1b1AdoptionValidation(Me._udtEHSAccount)

            Case DocTypeModel.DocTypeCode.OW
                'Fields checking for One-Way Permit only
                blnProceed = Me.Step1b1OWValidation(Me._udtEHSAccount)

            Case DocTypeModel.DocTypeCode.TW
                'Fields checking for Two-Way Permit only
                blnProceed = Me.Step1b1TWValidation(Me._udtEHSAccount)

            Case DocTypeModel.DocTypeCode.RFNo8
                'Fields checking for RFNo8 only
                blnProceed = Me.Step1b1RFNo8Validation(Me._udtEHSAccount)

                ' CRE20-0022 (Immu record) [Start][Martin]
            Case DocTypeModel.DocTypeCode.CCIC
                'Fields checking for CCIC only
                blnProceed = Me.Step1b1CCICValidation(Me._udtEHSAccount)
            Case DocTypeModel.DocTypeCode.ROP140
                Dim udcInputROP140 As ucInputROP140 = Me.udcStep1b1InputDocumentType.GetROP140Control()

                If udcInputROP140.CCCodeIsEmpty() Then
                    udcInputROP140.SetCName(String.Empty)
                    Me.udcChooseCCCode.Clean()
                    Me._udtSessionHandler.CCCodeRemoveFromSession(FunctCode)
                Else
                    udcInputROP140.SetErrorImage(ucInputDocTypeBase.BuildMode.Creation, False)
                    udcInputROP140.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

                    If udcInputROP140.IsValidCCCodeInput() Then
                        'Check for select chinese name/CCCode 
                        'if cccode is changed (different between session value and input box), or incorrect CCCode
                        If Me.NeedPopupChineseNameDialog(DocTypeModel.DocTypeCode.ROP140) Then

                            systemMessage = Me.Step1b1ShowCCCodeSelection(udcInputROP140, DocTypeModel.DocTypeCode.ROP140, True)
                            If Not systemMessage Is Nothing Then
                                Me.udcMsgBoxErr.AddMessage(systemMessage)
                            Else
                                EHSAccountCreationBase.AuditLogStep1b1PromptCCCode(_udtAuditLogEntry)

                            End If
                            blnProceed = False
                        End If

                    Else
                        Me.udcMsgBoxErr.AddMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00039)
                        udcInputROP140.SetCCCodeError(True)
                        blnProceed = False

                    End If

                End If

                If blnProceed Then
                    'Fields checking for ROP140 only
                    blnProceed = Me.Step1b1ROP140Validation(Me._udtEHSAccount)
                End If

            Case DocTypeModel.DocTypeCode.PASS
                'Fields checking for PASS only
                blnProceed = Me.Step1b1PASSValidation(Me._udtEHSAccount)
                ' CRE20-0022 (Immu record) [End][Martin]

        End Select

        If blnProceed Then
            Me._udtSessionHandler.EHSAccountSaveToSession(Me._udtEHSAccount, FunctCode)

            EHSAccountCreationBase.AuditLogStep1b1Complete(_udtAuditLogEntry, Me._udtEHSAccount)

            Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1b2
        End If

        Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail, _udtAuditLogEntry, Common.Component.LogID.LOG00033, "Enter Detail Failed")

    End Sub

    Protected Sub btnStep1b1Cancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) _
        Handles btnStep1b1Cancel.Click
        Me.ModalPopupConfirmCancel.Show()
    End Sub

    Private Sub btnStep1b1ChangePractice_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnStep1b1ChangePractice.Click
        Me.ModalPopupPracticeSelection.Show()
    End Sub

    Private Sub btnStep1b1InputTips_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnStep1b1InputTips.Click

        Me._udtEHSAccount = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)


        ScriptManager.RegisterStartupScript(Me, Page.GetType, "DocumentSmaple", String.Format("javascript:show{0}Help('{1}');", Me._udtEHSAccount.SearchDocCode.Replace("/", ""), Session("language")), True)

        'Dim a As String = DocType.DocTypeModel.DocTypeCode.HKIC
        'a = DocType.DocTypeModel.HKBirthCert
        'a = DocType.DocTypeModel.AdoptionCert
        'a = DocType.DocTypeModel.DocOfIdentity
        'a = DocType.DocTypeModel.ReEntryPermit


    End Sub

    Private Sub SetupStep1b1(ByVal udtEHSAccount As EHSAccountModel, ByVal createPopupPractice As Boolean, ByVal activeViewChanged As Boolean)
        If Me._udtSessionHandler.EHSClaimStepsGetFromSession(FunctCode) <> ActiveViewIndex.Step1b1 Then
            Me._blnIsRequireHandlePageRefresh = True
            Return
        End If

        Dim udtSelectedPracticeDisplay As BLL.PracticeDisplayModel = Nothing
        Dim udtPracticeDisplays As BLL.PracticeDisplayModelCollection = Nothing
        Dim udtDataEntry As DataEntryUserModel = Nothing
        Me._udtSessionHandler.CurrentUserGetFromSession(Me._udtSP, udtDataEntry)

        'Show the current practice detail
        Dim practiceDisplay As PracticeDisplayModel = Me._udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)
        Dim strAreaCode As String = String.Empty


        If practiceDisplay.AddressCode.HasValue Then
            strAreaCode = practiceDisplay.AddressCode.Value
        End If

        Me._udtFormatter = New Formatter()

        'Set Current Practice Value 
        udtSelectedPracticeDisplay = Me._udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)

        If Me._udtSessionHandler.Language() = CultureLanguage.TradChinese OrElse Me._udtSessionHandler.Language() = CultureLanguage.SimpChinese Then
            Me.lblStep1b1CurrentPractice.Text = String.Format("{0} ({1})", practiceDisplay.PracticeNameChi, practiceDisplay.PracticeID)
            Me.lblStep1b1CurrentPractice.CssClass = "tableTextChi"
        Else
            Me.lblStep1b1CurrentPractice.Text = String.Format("{0} ({1})", practiceDisplay.PracticeName, practiceDisplay.PracticeID)
            Me.lblStep1b1CurrentPractice.CssClass = "tableText"
        End If
        'Me._udtFormatter.formatAddress(practiceDisplay.Room, practiceDisplay.Floor, practiceDisplay.Block, practiceDisplay.Building, practiceDisplay.District, strAreaCode))

        Me.imgDocTips.Visible = False
        Me.btnStep1b1InputTips.Visible = False

        'Invisibility Button
        Me.ShowChangePracticeButton(Me.btnStep1b1ChangePractice)

        Dim udtDocTypeBLL As New DocTypeBLL()
        Dim strHelpAvauilable As String = udtDocTypeBLL.getAllDocType().Filter(udtEHSAccount.SearchDocCode).HelpAvailable
        If strHelpAvauilable.ToUpper() = "Y" Then
            Me.btnStep1b1InputTips.Visible = True
            Me.btnStep1b1InputTips.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "HelpBtn")
            Me.btnStep1b1InputTips.AlternateText = Me.GetGlobalResourceObject("AlternateText", "HelpBtn")
        Else
            Me.btnStep1b1InputTips.Visible = False
        End If


        Select Case udtEHSAccount.SearchDocCode
            Case DocType.DocTypeModel.DocTypeCode.HKIC
                'input Tips
                Me.lblStep1b1InputInfoText.Text = Me.GetGlobalResourceObject("Text", "EnterVRAInfo")

                ''Image Button
                'Me.btnStep1b1InputTips.Visible = True
                'Me.btnStep1b1InputTips.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "HelpBtn")
                'Me.btnStep1b1InputTips.AlternateText = Me.GetGlobalResourceObject("AlternateText", "HelpBtn")

            Case DocType.DocTypeModel.DocTypeCode.EC
                'Input Tips 
                Me.lblStep1b1InputInfoText.Text = Me.GetGlobalResourceObject("Text", "EnterVRAInfoEC")
            Case DocType.DocTypeModel.DocTypeCode.HKBC
                'Input Tips 
                Me.lblStep1b1InputInfoText.Text = Me.GetGlobalResourceObject("Text", "EnterEHSAccountInfoHKBC")

                ''Image Button
                'Me.btnStep1b1InputTips.Visible = True
                'Me.btnStep1b1InputTips.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "HelpBtn")
                'Me.btnStep1b1InputTips.AlternateText = Me.GetGlobalResourceObject("AlternateText", "HelpBtn")

                'Me.imgDocTips.Visible = True
                'Me.imgDocTips.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "HKBCSampleImg")
            Case DocType.DocTypeModel.DocTypeCode.ADOPC
                'Input Tips 
                Me.lblStep1b1InputInfoText.Text = Me.GetGlobalResourceObject("Text", "EnterVRAInfoADOPC")
            Case DocType.DocTypeModel.DocTypeCode.DI
                'Input Tips 
                Me.lblStep1b1InputInfoText.Text = Me.GetGlobalResourceObject("Text", "EnterVRAInfoDI")
            Case DocType.DocTypeModel.DocTypeCode.ID235B
                'Input Tips 
                Me.lblStep1b1InputInfoText.Text = Me.GetGlobalResourceObject("Text", "EnterVRAInfoID235B")
            Case DocType.DocTypeModel.DocTypeCode.REPMT
                'Input Tips 
                Me.lblStep1b1InputInfoText.Text = Me.GetGlobalResourceObject("Text", "EnterVRAInfoREPMT")
            Case DocType.DocTypeModel.DocTypeCode.VISA
                'Input Tips 
                Me.lblStep1b1InputInfoText.Text = Me.GetGlobalResourceObject("Text", "EnterVRAInfoVISA")

                ' CRE20-0022 (Immu record) [Start][Martin]
            Case DocType.DocTypeModel.DocTypeCode.CCIC
                'Input Tips 
                Me.lblStep1b1InputInfoText.Text = Me.GetGlobalResourceObject("Text", "EnterVRAInfoCCIC")

            Case DocType.DocTypeModel.DocTypeCode.ROP140
                'Input Tips 
                Me.lblStep1b1InputInfoText.Text = Me.GetGlobalResourceObject("Text", "EnterVRAInfoROP140")

            Case DocType.DocTypeModel.DocTypeCode.PASS
                'Input Tips 
                Me.lblStep1b1InputInfoText.Text = Me.GetGlobalResourceObject("Text", "EnterVRAInfoPASS")

            Case DocType.DocTypeModel.DocTypeCode.OW
                'Input Tips 
                Me.lblStep1b1InputInfoText.Text = Me.GetGlobalResourceObject("Text", "EnterVRAInfoOW")

            Case DocType.DocTypeModel.DocTypeCode.TW
                'Input Tips 
                Me.lblStep1b1InputInfoText.Text = Me.GetGlobalResourceObject("Text", "EnterVRAInfoTW")
                ' CRE20-0022 (Immu record) [End][Martin]
        End Select

        'Format: Practice Name (Practice display Seq/Practice ID) [Practice Address]
        If createPopupPractice Then

            'Set up practice selection popup Box
            udtPracticeDisplays = Me._udtSessionHandler.PracticeDisplayListGetFromSession(FunctCode)
            Me.udcPracticeRadioButtonGroup.VerticalScrollBar = True
            ' CRE20-0XX (HA Scheme) [Start][Winnie]
            Me.udcPracticeRadioButtonGroup.SchemeSelection = IIf(Me.SubPlatform = EnumHCSPSubPlatform.CN, True, False)
            Me.udcPracticeRadioButtonGroup.SelectedScheme = Me._udtSessionHandler.SchemeSelectedForPracticeGetFromSession(FunctCode)
            ' CRE20-0XX (HA Scheme) [End][Winnie]
            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Me.udcPracticeRadioButtonGroup.BuildRadioButtonGroup(udtPracticeDisplays, _
                                                                 Me._udtSP.PracticeList, _
                                                                 Me._udtSP.SchemeInfoList, _
                                                                 Me._udtSessionHandler.Language, _
                                                                 PracticeRadioButtonGroup.DisplayMode.Address, _
                                                                 Me.ClaimMode)
            ' CRE20-0022 (Immu record) [End][Chris YIM]
            Me.udcStep1b1InputDocumentType.EHSAccount = udtEHSAccount
            Me.udcStep1b1InputDocumentType.DocType = udtEHSAccount.SearchDocCode
            Me.udcStep1b1InputDocumentType.Mode = ucInputDocTypeBase.BuildMode.Creation
            Me.udcStep1b1InputDocumentType.FillValue = True
            Me.udcStep1b1InputDocumentType.ActiveViewChanged = activeViewChanged
            Me.udcStep1b1InputDocumentType.SchemeClaim = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
            Me.udcStep1b1InputDocumentType.AuditLogEntry = _udtAuditLogEntry
            Me.udcStep1b1InputDocumentType.FixEnglishNameGender = Not IsNothing(_udtSessionHandler.VREEHSAccountGetFromSession) _
                AndAlso _udtSessionHandler.VREEHSAccountGetFromSession.EHSPersonalInformationList(0).ENameSurName <> String.Empty

            Me.udcStep1b1InputDocumentType.Built()

        End If
    End Sub

    Private Sub Step1b1ResetErrorImage()
        Me._udtEHSAccount = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Dim ucInputDocType As ucInputDocTypeBase = Nothing

        Select Case Me._udtEHSAccount.SearchDocCode
            Case DocTypeModel.DocTypeCode.HKIC
                ucInputDocType = Me.udcStep1b1InputDocumentType.GetHKICControl()

            Case DocTypeModel.DocTypeCode.EC
                ucInputDocType = Me.udcStep1b1InputDocumentType.GetECControl()
            Case DocTypeModel.DocTypeCode.HKBC
                ucInputDocType = Me.udcStep1b1InputDocumentType.GetHKBCControl()

            Case DocTypeModel.DocTypeCode.DI
                ucInputDocType = Me.udcStep1b1InputDocumentType.GetDIControl()

            Case DocTypeModel.DocTypeCode.ID235B
                ucInputDocType = Me.udcStep1b1InputDocumentType.GetID235BControl()

            Case DocTypeModel.DocTypeCode.REPMT
                ucInputDocType = Me.udcStep1b1InputDocumentType.GetREPMTControl()

            Case DocTypeModel.DocTypeCode.VISA
                ucInputDocType = Me.udcStep1b1InputDocumentType.GetVISAControl()

            Case DocTypeModel.DocTypeCode.ADOPC
                ucInputDocType = Me.udcStep1b1InputDocumentType.GetADOPCControl()

            Case DocTypeModel.DocTypeCode.OW
                ucInputDocType = Me.udcStep1b1InputDocumentType.GetOWControl()

                ' CRE20-0022 (Immu record) [Start][Martin]
            Case DocTypeModel.DocTypeCode.TW
                ucInputDocType = Me.udcStep1b1InputDocumentType.GetTWControl()

            Case DocTypeModel.DocTypeCode.CCIC
                ucInputDocType = Me.udcStep1b1InputDocumentType.GetCCICControl()

            Case DocTypeModel.DocTypeCode.ROP140
                ucInputDocType = Me.udcStep1b1InputDocumentType.GetROP140Control()

            Case DocTypeModel.DocTypeCode.PASS
                ucInputDocType = Me.udcStep1b1InputDocumentType.GetPASSControl()
                ' CRE20-0022 (Immu record) [End][Martin]
        End Select

        If Not ucInputDocType Is Nothing Then
            ucInputDocType.SetErrorImage(ucInputDocTypeBase.BuildMode.Creation, False)
        End If
    End Sub

#End Region

#Region "Step lbl Enter Account Detial Valiation"

    'For HKID
    Private Function Step1b1HKICValdiation(ByRef udtEHSAccount As EHSAccountModel) As Boolean
        Dim isValid As Boolean = True
        Dim strHKIDIssueDate As String = Nothing
        Dim dtHKIDIssueDate As DateTime
        Dim udcInputHKIC As ucInputHKID = Me.udcStep1b1InputDocumentType.GetHKICControl()
        udcInputHKIC.SetErrorImage(ucInputDocTypeBase.BuildMode.Creation, False)
        udcInputHKIC.SetProperty(ucInputDocTypeBase.BuildMode.Creation)
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode)

        Me._systemMessage = Me._validator.chkEngName(udcInputHKIC.ENameSurName, udcInputHKIC.ENameFirstName)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputHKIC.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        Me._systemMessage = Me._validator.chkCCCode_UsingDDL(String.Format("{0}{1}", udcInputHKIC.CCCode1, Me.udcChooseCCCode.SelectedCCCodeTail1), _
            String.Format("{0}{1}", udcInputHKIC.CCCode2, Me.udcChooseCCCode.SelectedCCCodeTail2), _
            String.Format("{0}{1}", udcInputHKIC.CCCode3, Me.udcChooseCCCode.SelectedCCCodeTail3), _
            String.Format("{0}{1}", udcInputHKIC.CCCode4, Me.udcChooseCCCode.SelectedCCCodeTail4), _
            String.Format("{0}{1}", udcInputHKIC.CCCode5, Me.udcChooseCCCode.SelectedCCCodeTail5), _
            String.Format("{0}{1}", udcInputHKIC.CCCode6, Me.udcChooseCCCode.SelectedCCCodeTail6))
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputHKIC.SetCCCodeError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        Me._systemMessage = Me._validator.chkGender(udcInputHKIC.Gender)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputHKIC.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        strHKIDIssueDate = Me._udtFormatter.formatHKIDIssueDateBeforeValidate(udcInputHKIC.HKIDIssuseDate)
        Me._systemMessage = Me._validator.chkHKIDIssueDate(strHKIDIssueDate, udtEHSAccountPersonalInfo.DOB)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputHKIC.SetHKIDIssueDateError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        Else
            dtHKIDIssueDate = Me._udtFormatter.convertHKIDIssueDateStringToDate(strHKIDIssueDate)
        End If

        If isValid Then

            udtEHSAccountPersonalInfo.ENameSurName = udcInputHKIC.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputHKIC.ENameFirstName

            udtEHSAccountPersonalInfo.CCCode1 = String.Format("{0}{1}", udcInputHKIC.CCCode1, Me.udcChooseCCCode.SelectedCCCodeTail1)
            udtEHSAccountPersonalInfo.CCCode2 = String.Format("{0}{1}", udcInputHKIC.CCCode2, Me.udcChooseCCCode.SelectedCCCodeTail2)
            udtEHSAccountPersonalInfo.CCCode3 = String.Format("{0}{1}", udcInputHKIC.CCCode3, Me.udcChooseCCCode.SelectedCCCodeTail3)
            udtEHSAccountPersonalInfo.CCCode4 = String.Format("{0}{1}", udcInputHKIC.CCCode4, Me.udcChooseCCCode.SelectedCCCodeTail4)
            udtEHSAccountPersonalInfo.CCCode5 = String.Format("{0}{1}", udcInputHKIC.CCCode5, Me.udcChooseCCCode.SelectedCCCodeTail5)
            udtEHSAccountPersonalInfo.CCCode6 = String.Format("{0}{1}", udcInputHKIC.CCCode6, Me.udcChooseCCCode.SelectedCCCodeTail6)

            'Retervie Chinese Name from Choose
            udcInputHKIC.CCCode1 = udtEHSAccountPersonalInfo.CCCode1
            udcInputHKIC.CCCode2 = udtEHSAccountPersonalInfo.CCCode2
            udcInputHKIC.CCCode3 = udtEHSAccountPersonalInfo.CCCode3
            udcInputHKIC.CCCode4 = udtEHSAccountPersonalInfo.CCCode4
            udcInputHKIC.CCCode5 = udtEHSAccountPersonalInfo.CCCode5
            udcInputHKIC.CCCode6 = udtEHSAccountPersonalInfo.CCCode6
            udcInputHKIC.SetCName()
            udtEHSAccountPersonalInfo.CName = udcInputHKIC.CName

            udtEHSAccountPersonalInfo.Gender = udcInputHKIC.Gender
            udtEHSAccountPersonalInfo.DateofIssue = dtHKIDIssueDate
            udtEHSAccountPersonalInfo.DocCode = DocType.DocTypeModel.DocTypeCode.HKIC

            Me._systemMessage = Me.EHSAccountBasicValidation(DocTypeModel.DocTypeCode.HKIC, udtEHSAccount)
            If Not Me._systemMessage Is Nothing Then
                isValid = False
                Me.udcMsgBoxErr.AddMessage(_systemMessage)
            End If
        End If

        Return isValid
    End Function

    'For EC
    Private Function Step1b1ECValdiation(ByRef udtEHSAccount As EHSAccountModel) As Boolean
        Dim udcInputEC As ucInputEC = Me.udcStep1b1InputDocumentType.GetECControl()
        udcInputEC.SetErrorImage(ucInputDocTypeBase.BuildMode.Creation, False)
        udcInputEC.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

        Dim isValid As Boolean = True
        Dim strECDateDay As String = udcInputEC.ECDateDay.Trim()
        Dim strECDateMonth As String = udcInputEC.ECDateMonth.Trim()
        Dim strECDateYear As String = udcInputEC.ECDateYear.Trim()
        Dim strECDate As String = Nothing
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode)
        Me._udtFormatter = New Formatter()

        ' Serial No.
        Me._systemMessage = Me._validator.chkSerialNo(udcInputEC.SerialNumber, udcInputEC.SerialNumberNotProvided)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputEC.SetECSerialNoError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        ' Reference
        Me._systemMessage = Me._validator.chkReferenceNo(udcInputEC.Reference, udcInputEC.ReferenceOtherFormat)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputEC.SetECReferenceError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        ' Date of Issue
        Me._systemMessage = Me._validator.chkECDate(strECDateDay, strECDateMonth, strECDateYear, udtEHSAccountPersonalInfo.DOB)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputEC.SetECDateError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        ' Name in English
        Me._systemMessage = Me._validator.chkEngName(udcInputEC.ENameSurName, udcInputEC.ENameFirstName)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputEC.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        ' CRE15-014 HA_MingLiu UTF32 [Start][Winnie]
        'Chinese Name
        Me._systemMessage = Me._validator.chkChiName(udcInputEC.CName)
        If Not IsNothing(_systemMessage) Then
            isValid = False
            udcInputEC.SetCNameError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If
        ' CRE15-014 HA_MingLiu UTF32 [End][Winnie]

        ' Gender
        Me._systemMessage = Me._validator.chkGender(udcInputEC.Gender)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputEC.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        ' Date of Birth: Age {0} On {D MMM YYYY}
        If Not udtEHSAccountPersonalInfo.ECAge.HasValue Then
            Me._systemMessage = Me._validator.chkDOBType(udcInputEC.IsExactDOB)
            If Not Me._systemMessage Is Nothing Then
                isValid = False
                udcInputEC.SetDOBTypeError(True)
                Me.udcMsgBoxErr.AddMessage(_systemMessage)
            End If
        End If

        ' Serial No. Not Provided and Reference free format is only allowed for Date of Issue < {SystemParameters: EC_DOI}
        If isValid Then
            ' Get user input Date of Issue
            If strECDateDay.Length = 1 Then
                strECDate = String.Format("0{0}-{1}-{2}", strECDateDay, strECDateMonth, strECDateYear)
            Else
                strECDate = String.Format("{0}-{1}-{2}", strECDateDay, strECDateMonth, strECDateYear)
            End If

            Dim dtmECDate As Date = Date.ParseExact(strECDate, "dd-MM-yyyy", Nothing)

            _systemMessage = _validator.chkSerialNoNotProvidedAllow(dtmECDate, udcInputEC.SerialNumberNotProvided)
            If Not IsNothing(_systemMessage) Then
                isValid = False
                udcInputEC.SetECSerialNoError(True)
                udcMsgBoxErr.AddMessage(_systemMessage)
            End If

            ' Try parse the Reference if all the previous inputs are valid
            If isValid Then
                If udcInputEC.ReferenceOtherFormat Then
                    Dim dtmECDOI As New Date(udcInputEC.ECDateYear, udcInputEC.ECDateMonth, udcInputEC.ECDateDay)
                    _validator.TryParseECReference(udcInputEC.Reference, udcInputEC.ReferenceOtherFormat, dtmECDOI)
                End If

            End If

            _systemMessage = _validator.chkReferenceOtherFormatAllow(dtmECDate, udcInputEC.ReferenceOtherFormat)
            If Not IsNothing(_systemMessage) Then
                isValid = False
                udcInputEC.SetECReferenceError(True)
                udcMsgBoxErr.AddMessage(_systemMessage)
            End If

        End If

        If isValid Then
            udtEHSAccountPersonalInfo.ENameSurName = udcInputEC.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputEC.ENameFirstName
            udtEHSAccountPersonalInfo.CName = udcInputEC.CName
            udtEHSAccountPersonalInfo.Gender = udcInputEC.Gender

            udtEHSAccountPersonalInfo.ECSerialNoNotProvided = udcInputEC.SerialNumberNotProvided
            udtEHSAccountPersonalInfo.ECSerialNo = udcInputEC.SerialNumber

            udtEHSAccountPersonalInfo.ECReferenceNoOtherFormat = udcInputEC.ReferenceOtherFormat
            'TODO: Lawrence
            'If udcInputEC.ReferenceOtherFormat Then
            '    udtEHSAccountPersonalInfo.ECReferenceNo = udcInputEC.Reference
            'Else
            '    udtEHSAccountPersonalInfo.ECReferenceNo = udcInputEC.Reference.Replace("(", String.Empty).Replace(")", String.Empty)
            'End If
            udtEHSAccountPersonalInfo.ECReferenceNo = udcInputEC.Reference


            udtEHSAccountPersonalInfo.DateofIssue = CDate(Me._udtFormatter.convertDate(strECDate, Common.Component.CultureLanguage.English))
            udtEHSAccountPersonalInfo.DocCode = DocType.DocTypeModel.DocTypeCode.EC
            udtEHSAccountPersonalInfo.SetDOBTypeSelected(True)
            If Not udtEHSAccountPersonalInfo.ECAge.HasValue Then
                udtEHSAccountPersonalInfo.ExactDOB = udcInputEC.IsExactDOB
            End If

            Me._systemMessage = Me.EHSAccountBasicValidation(DocTypeModel.DocTypeCode.EC, udtEHSAccount)
            If Not Me._systemMessage Is Nothing Then
                isValid = False
                Me.udcMsgBoxErr.AddMessage(_systemMessage)
            End If
        End If

        Return isValid
    End Function

    'For HKBC
    Private Function Step1b1HKBCValidation(ByRef udtEHSAccount As EHSAccountModel) As Boolean
        Dim isValid As Boolean = True

        Dim udcInputHKBC As ucInputHKBC = Me.udcStep1b1InputDocumentType.GetHKBCControl()
        udcInputHKBC.SetErrorImage(ucInputDocTypeBase.BuildMode.Creation, False)
        udcInputHKBC.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

        Me._systemMessage = Me._validator.chkEngName(udcInputHKBC.ENameSurName, udcInputHKBC.ENameFirstName)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputHKBC.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        Me._systemMessage = Me._validator.chkGender(udcInputHKBC.Gender)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputHKBC.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        ' CRE16-012 Removal of DOB InWord [Start][Winnie]
        'Me._systemMessage = Me._validator.chkDOBType(udcInputHKBC.IsExactDOB)
        'If Not Me._systemMessage Is Nothing Then
        '    isValid = False
        '    udcInputHKBC.SetDOBTypeError(True)
        '    Me.udcMsgBoxErr.AddMessage(_systemMessage)
        'End If

        'If udcInputHKBC.DOBInWordCase Then
        '    If udcInputHKBC.DOBInWord Is Nothing OrElse udcInputHKBC.DOBInWord = String.Empty Then
        '        isValid = False
        '        Me._systemMessage = New SystemMessage("990000", "E", "00160")
        '        udcInputHKBC.SetDOBTypeError(True)
        '        Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        '    End If
        'End If
        ' CRE16-012 Removal of DOB InWord [End][Winnie]

        If isValid Then
            Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)
            udtEHSAccountPersonalInfo.ENameSurName = udcInputHKBC.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputHKBC.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputHKBC.Gender
            udtEHSAccountPersonalInfo.OtherInfo = udcInputHKBC.DOBInWord
            udtEHSAccountPersonalInfo.DocCode = DocType.DocTypeModel.DocTypeCode.HKBC
            udtEHSAccountPersonalInfo.ExactDOB = udcInputHKBC.IsExactDOB
            udtEHSAccountPersonalInfo.SetDOBTypeSelected(True)

            Me._systemMessage = Me.EHSAccountBasicValidation(DocTypeModel.DocTypeCode.HKBC, udtEHSAccount)
            If Not Me._systemMessage Is Nothing Then
                isValid = False
                Me.udcMsgBoxErr.AddMessage(_systemMessage)
            End If

        End If

        Return isValid
    End Function

    'For Document Identity
    Private Function Step1b1DIValidation(ByRef udtEHSAccount As EHSAccountModel) As Boolean
        Dim isValid As Boolean = True
        Dim udcInputDI As ucInputDI = Me.udcStep1b1InputDocumentType.GetDIControl()
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)
        Dim dtDateOfIssue As DateTime
        Dim strDOI As String = String.Empty
        udcInputDI.SetErrorImage(ucInputDocTypeBase.BuildMode.Creation, False)
        udcInputDI.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

        Me._systemMessage = Me._validator.chkEngName(udcInputDI.ENameSurName, udcInputDI.ENameFirstName)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputDI.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        Me._systemMessage = Me._validator.chkGender(udcInputDI.Gender)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputDI.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        End If

        strDOI = Me._udtFormatter.formatDateBeforValidation_DDMMYYYY(udcInputDI.DateOfIssue)
        Me._systemMessage = Me._validator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.DI, strDOI, udtEHSAccountPersonalInfo.DOB)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputDI.SetDOIError(True)
            Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        Else
            dtDateOfIssue = Me._udtFormatter.convertDate(Me._udtFormatter.formatInputDate(strDOI), Me._udtSessionHandler.Language())
        End If

        If isValid Then
            udtEHSAccountPersonalInfo.ENameSurName = udcInputDI.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputDI.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputDI.Gender
            udtEHSAccountPersonalInfo.DocCode = DocType.DocTypeModel.DocTypeCode.DI
            udtEHSAccountPersonalInfo.DateofIssue = dtDateOfIssue
            udtEHSAccountPersonalInfo.SetDOBTypeSelected(True)

            Me._systemMessage = Me.EHSAccountBasicValidation(DocType.DocTypeModel.DocTypeCode.DI, udtEHSAccount)
            If Not Me._systemMessage Is Nothing Then
                isValid = False
                Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
            End If
        End If

        Return isValid
    End Function

    'For ID235B
    Private Function Step1b1ID235BValidation(ByRef udtEHSAccount As EHSAccountModel) As Boolean
        Dim isValid As Boolean = True
        Dim udcInputID235B As ucInputID235B = Me.udcStep1b1InputDocumentType.GetID235BControl()
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)
        Dim dtmPermitRemain As DateTime
        Dim strPermitRemain As String = String.Empty
        udcInputID235B.SetErrorImage(ucInputDocTypeBase.BuildMode.Creation, False)
        udcInputID235B.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

        Me._systemMessage = Me._validator.chkEngName(udcInputID235B.ENameSurName, udcInputID235B.ENameFirstName)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputID235B.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        Me._systemMessage = Me._validator.chkGender(udcInputID235B.Gender)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputID235B.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        End If

        strPermitRemain = Me._udtFormatter.formatDateBeforValidation_DDMMYYYY(udcInputID235B.PermitRemain)
        Me._systemMessage = Me._validator.chkPremitToRemainUntil(strPermitRemain, udtEHSAccountPersonalInfo.DOB, DocType.DocTypeModel.DocTypeCode.ID235B)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputID235B.SetPermitRemainError(True)
            Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        Else
            dtmPermitRemain = Me._udtFormatter.convertDate(Me._udtFormatter.formatInputDate(strPermitRemain), Common.Component.CultureLanguage.English)
        End If

        'Me._systemMessage = Me._validator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.ID235B, strDOI, udtEHSAccountPersonalInfo.DOB)
        'If Not Me._systemMessage Is Nothing Then
        '    isValid = False
        '    udcInputID235B.SetPermitRemainError(True)
        '    Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        'Else
        '    dtDateOfIssue = Me._udtFormatter.convertDate(strDOI, Me._udtSessionHandler.Language())
        'End If

        If isValid Then
            udtEHSAccountPersonalInfo.ENameSurName = udcInputID235B.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputID235B.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputID235B.Gender
            udtEHSAccountPersonalInfo.DocCode = DocType.DocTypeModel.DocTypeCode.ID235B
            udtEHSAccountPersonalInfo.PermitToRemainUntil = dtmPermitRemain
            udtEHSAccountPersonalInfo.SetDOBTypeSelected(True)

            Me._systemMessage = Me.EHSAccountBasicValidation(DocType.DocTypeModel.DocTypeCode.ID235B, udtEHSAccount)
            If Not Me._systemMessage Is Nothing Then
                isValid = False
                Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
            End If
        End If

        Return isValid
    End Function

    'For REPMT
    Private Function Step1b1RepmtValidation(ByRef udtEHSAccount As EHSAccountModel) As Boolean
        Dim isValid As Boolean = True
        Dim udcInputReentryPermit As ucInputReentryPermit = Me.udcStep1b1InputDocumentType.GetREPMTControl()
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)
        Dim dtDateOfIssue As DateTime
        Dim strDOI As String = String.Empty
        udcInputReentryPermit.SetErrorImage(ucInputDocTypeBase.BuildMode.Creation, False)
        udcInputReentryPermit.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

        Me._systemMessage = Me._validator.chkEngName(udcInputReentryPermit.ENameSurName, udcInputReentryPermit.ENameFirstName)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputReentryPermit.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        Me._systemMessage = Me._validator.chkGender(udcInputReentryPermit.Gender)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputReentryPermit.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        End If

        strDOI = Me._udtFormatter.formatDateBeforValidation_DDMMYYYY(udcInputReentryPermit.DateOfIssue)
        Me._systemMessage = Me._validator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.REPMT, strDOI, udtEHSAccountPersonalInfo.DOB)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputReentryPermit.SetDOIError(True)
            Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        Else
            dtDateOfIssue = Me._udtFormatter.convertDate(Me._udtFormatter.formatInputDate(strDOI), Me._udtSessionHandler.Language())
        End If

        If isValid Then
            udtEHSAccountPersonalInfo.ENameSurName = udcInputReentryPermit.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputReentryPermit.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputReentryPermit.Gender
            udtEHSAccountPersonalInfo.DocCode = DocType.DocTypeModel.DocTypeCode.REPMT
            udtEHSAccountPersonalInfo.DateofIssue = dtDateOfIssue
            udtEHSAccountPersonalInfo.SetDOBTypeSelected(True)

            Me._systemMessage = Me.EHSAccountBasicValidation(DocTypeModel.DocTypeCode.REPMT, udtEHSAccount)
            If Not Me._systemMessage Is Nothing Then
                isValid = False
                Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
            End If
        End If

        Return isValid
    End Function

    'For VISA
    Private Function Step1b1VISAValidation(ByRef udtEHSAccount As EHSAccountModel) As Boolean
        Dim isValid As Boolean = True
        Dim udcInputVISA As ucInputVISA = Me.udcStep1b1InputDocumentType.GetVISAControl()
        udcInputVISA.SetErrorImage(ucInputDocTypeBase.BuildMode.Creation, False)
        udcInputVISA.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

        Me._systemMessage = Me._validator.chkEngName(udcInputVISA.ENameSurName, udcInputVISA.ENameFirstName)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputVISA.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        Me._systemMessage = Me._validator.chkGender(udcInputVISA.Gender)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputVISA.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        End If

        Me._systemMessage = Me._validator.chkPassportNo(udcInputVISA.PassportNo)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputVISA.SetPassportNoError(True)
            Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        End If

        If isValid Then
            Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)
            udtEHSAccountPersonalInfo.ENameSurName = udcInputVISA.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputVISA.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputVISA.Gender
            udtEHSAccountPersonalInfo.DocCode = DocType.DocTypeModel.DocTypeCode.VISA
            udtEHSAccountPersonalInfo.Foreign_Passport_No = udcInputVISA.PassportNo
            udtEHSAccountPersonalInfo.SetDOBTypeSelected(True)

            Me._systemMessage = Me.EHSAccountBasicValidation(DocType.DocTypeModel.DocTypeCode.VISA, udtEHSAccount)
            If Not Me._systemMessage Is Nothing Then
                isValid = False
                Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
            End If
        End If

        Return isValid
    End Function

    'For Adoption Cert
    Private Function Step1b1AdoptionValidation(ByRef udtEHSAccount As EHSAccountModel) As Boolean
        Dim isValid As Boolean = True
        Dim udcInputAdoption As ucInputAdoption = Me.udcStep1b1InputDocumentType.GetADOPCControl()
        udcInputAdoption.SetErrorImage(ucInputDocTypeBase.BuildMode.Creation, False)
        udcInputAdoption.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

        Me._systemMessage = Me._validator.chkEngName(udcInputAdoption.ENameSurName, udcInputAdoption.ENameFirstName)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputAdoption.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        Me._systemMessage = Me._validator.chkGender(udcInputAdoption.Gender)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputAdoption.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        End If

        ' CRE16-012 Removal of DOB InWord [Start][Winnie]
        'Me._systemMessage = Me._validator.chkDOBType(udcInputAdoption.IsExactDOB)
        'If Not Me._systemMessage Is Nothing Then
        '    isValid = False
        '    udcInputAdoption.SetDOBInWordError(True)
        '    Me.udcMsgBoxErr.AddMessage(_systemMessage)
        'End If

        'If udcInputAdoption.DOBInWordCase Then
        '    If udcInputAdoption.DOBInWord Is Nothing OrElse udcInputAdoption.DOBInWord = String.Empty Then
        '        isValid = False
        '        Me._systemMessage = New SystemMessage("990000", "E", "00160")
        '        udcInputAdoption.SetDOBInWordError(True)
        '        Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        '    End If
        'End If
        ' CRE16-012 Removal of DOB InWord [End][Winnie]

        If isValid Then
            Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)
            udtEHSAccountPersonalInfo.ENameSurName = udcInputAdoption.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputAdoption.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputAdoption.Gender
            udtEHSAccountPersonalInfo.DocCode = DocType.DocTypeModel.DocTypeCode.ADOPC
            udtEHSAccountPersonalInfo.ExactDOB = udcInputAdoption.IsExactDOB
            udtEHSAccountPersonalInfo.OtherInfo = udcInputAdoption.DOBInWord
            udtEHSAccountPersonalInfo.SetDOBTypeSelected(True)

            Me._systemMessage = Me.EHSAccountBasicValidation(DocType.DocTypeModel.DocTypeCode.ADOPC, udtEHSAccount)
            If Not Me._systemMessage Is Nothing Then
                isValid = False
                Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
            End If
        End If

        Return isValid
    End Function

    'For OW
    Private Function Step1b1OWValidation(ByRef udtEHSAccount As EHSAccountModel) As Boolean
        Dim isValid As Boolean = True
        Dim udcInputOW As ucInputOW = Me.udcStep1b1InputDocumentType.GetOWControl()
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)

        udcInputOW.SetErrorImage(ucInputDocTypeBase.BuildMode.Creation, False)
        udcInputOW.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

        Me._systemMessage = Me._validator.chkEngName(udcInputOW.ENameSurName, udcInputOW.ENameFirstName)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputOW.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        Me._systemMessage = Me._validator.chkGender(udcInputOW.Gender)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputOW.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        End If

        If isValid Then
            udtEHSAccountPersonalInfo.ENameSurName = udcInputOW.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputOW.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputOW.Gender
            udtEHSAccountPersonalInfo.DocCode = DocType.DocTypeModel.DocTypeCode.OW
            udtEHSAccountPersonalInfo.SetDOBTypeSelected(True)

            Me._systemMessage = Me.EHSAccountBasicValidation(DocType.DocTypeModel.DocTypeCode.OW, udtEHSAccount)
            If Not Me._systemMessage Is Nothing Then
                isValid = False
                Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
            End If
        End If

        Return isValid
    End Function

    ' CRE20-0023 (Immu record) [Start][Martin]
    'For TW
    Private Function Step1b1TWValidation(ByRef udtEHSAccount As EHSAccountModel) As Boolean
        Dim isValid As Boolean = True
        Dim udcInputTW As ucInputTW = Me.udcStep1b1InputDocumentType.GetTWControl()
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)

        udcInputTW.SetErrorImage(ucInputDocTypeBase.BuildMode.Creation, False)
        udcInputTW.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

        Me._systemMessage = Me._validator.chkEngName(udcInputTW.ENameSurName, udcInputTW.ENameFirstName)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputTW.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        Me._systemMessage = Me._validator.chkGender(udcInputTW.Gender)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputTW.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        End If

        If isValid Then
            udtEHSAccountPersonalInfo.ENameSurName = udcInputTW.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputTW.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputTW.Gender
            udtEHSAccountPersonalInfo.DocCode = DocType.DocTypeModel.DocTypeCode.TW
            udtEHSAccountPersonalInfo.SetDOBTypeSelected(True)

            Me._systemMessage = Me.EHSAccountBasicValidation(DocType.DocTypeModel.DocTypeCode.TW, udtEHSAccount)
            If Not Me._systemMessage Is Nothing Then
                isValid = False
                Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
            End If
        End If

        Return isValid
    End Function
    ' CRE20-0023 (Immu record) [End][Martin]

    'For RFNo8
    Private Function Step1b1RFNo8Validation(ByRef udtEHSAccount As EHSAccountModel) As Boolean
        Dim isValid As Boolean = True
        Dim udcInputRFNo8 As ucInputRFNo8 = Me.udcStep1b1InputDocumentType.GetRFNo8Control()
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)

        udcInputRFNo8.SetErrorImage(ucInputDocTypeBase.BuildMode.Creation, False)
        udcInputRFNo8.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

        Me._systemMessage = Me._validator.chkEngName(udcInputRFNo8.ENameSurName, udcInputRFNo8.ENameFirstName)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputRFNo8.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        Me._systemMessage = Me._validator.chkGender(udcInputRFNo8.Gender)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputRFNo8.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        End If

        If isValid Then
            udtEHSAccountPersonalInfo.ENameSurName = udcInputRFNo8.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputRFNo8.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputRFNo8.Gender
            udtEHSAccountPersonalInfo.DocCode = DocType.DocTypeModel.DocTypeCode.RFNo8
            udtEHSAccountPersonalInfo.SetDOBTypeSelected(True)

            Me._systemMessage = Me.EHSAccountBasicValidation(DocTypeModel.DocTypeCode.RFNo8, udtEHSAccount)
            If Not Me._systemMessage Is Nothing Then
                isValid = False
                Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
            End If
        End If

        Return isValid
    End Function

    ' CRE20-0022 (Immu record) [Start][Martin]
    'For CCIC
    Private Function Step1b1CCICValidation(ByRef udtEHSAccount As EHSAccountModel) As Boolean

        Dim isValid As Boolean = True
        Dim udcInputCCIC As ucInputCCIC = Me.udcStep1b1InputDocumentType.GetCCICControl()
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)
        Dim strHKIDIssueDate As String = String.Empty
        Dim dtHKIDIssueDate As DateTime
        udcInputCCIC.SetErrorImage(ucInputDocTypeBase.BuildMode.Creation, False)
        udcInputCCIC.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

        Me._systemMessage = Me._validator.chkEngName(udcInputCCIC.ENameSurName, udcInputCCIC.ENameFirstName, DocTypeModel.DocTypeCode.CCIC)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputCCIC.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        Me._systemMessage = Me._validator.chkGender(udcInputCCIC.Gender)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputCCIC.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        End If

        strHKIDIssueDate = Me._udtFormatter.formatHKIDIssueDateBeforeValidate(udcInputCCIC.DateOfIssue)
        Me._systemMessage = Me._validator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.CCIC, strHKIDIssueDate, udtEHSAccountPersonalInfo.DOB)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputCCIC.SetDOIError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        Else
            dtHKIDIssueDate = Me._udtFormatter.convertHKIDIssueDateStringToDate(strHKIDIssueDate)
        End If


        If isValid Then
            udtEHSAccountPersonalInfo.ENameSurName = udcInputCCIC.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputCCIC.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputCCIC.Gender
            udtEHSAccountPersonalInfo.DateofIssue = dtHKIDIssueDate
            udtEHSAccountPersonalInfo.DocCode = DocType.DocTypeModel.DocTypeCode.CCIC
            udtEHSAccountPersonalInfo.SetDOBTypeSelected(True)


            Me._systemMessage = Me.EHSAccountBasicValidation(DocType.DocTypeModel.DocTypeCode.CCIC, udtEHSAccount)
            If Not Me._systemMessage Is Nothing Then
                isValid = False
                Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
            End If
        End If

        Return isValid
    End Function

    'For ROP140
    Private Function Step1b1ROP140Validation(ByRef udtEHSAccount As EHSAccountModel) As Boolean
        Dim isValid As Boolean = True
        Dim udcInputROP140 As ucInputROP140 = Me.udcStep1b1InputDocumentType.GetROP140Control()
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)
        Dim dtDateOfIssue As DateTime
        Dim strDOI As String = String.Empty
        udcInputROP140.SetErrorImage(ucInputDocTypeBase.BuildMode.Creation, False)
        udcInputROP140.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

        Me._systemMessage = Me._validator.chkEngName(udcInputROP140.ENameSurName, udcInputROP140.ENameFirstName)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputROP140.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        Me._systemMessage = Me._validator.chkCCCode_UsingDDL(String.Format("{0}{1}", udcInputROP140.CCCode1, Me.udcChooseCCCode.SelectedCCCodeTail1), _
                                                             String.Format("{0}{1}", udcInputROP140.CCCode2, Me.udcChooseCCCode.SelectedCCCodeTail2), _
                                                             String.Format("{0}{1}", udcInputROP140.CCCode3, Me.udcChooseCCCode.SelectedCCCodeTail3), _
                                                             String.Format("{0}{1}", udcInputROP140.CCCode4, Me.udcChooseCCCode.SelectedCCCodeTail4), _
                                                             String.Format("{0}{1}", udcInputROP140.CCCode5, Me.udcChooseCCCode.SelectedCCCodeTail5), _
                                                             String.Format("{0}{1}", udcInputROP140.CCCode6, Me.udcChooseCCCode.SelectedCCCodeTail6))
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputROP140.SetCCCodeError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        Me._systemMessage = Me._validator.chkGender(udcInputROP140.Gender)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputROP140.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        End If

        strDOI = Me._udtFormatter.formatDateBeforValidation_DDMMYYYY(udcInputROP140.DateOfIssue)
        Me._systemMessage = Me._validator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.ROP140, strDOI, udtEHSAccountPersonalInfo.DOB)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputROP140.SetDOIError(True)
            Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        Else
            dtDateOfIssue = Me._udtFormatter.convertDate(Me._udtFormatter.formatInputDate(strDOI), Me._udtSessionHandler.Language())
        End If

        If isValid Then
            udtEHSAccountPersonalInfo.ENameSurName = udcInputROP140.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputROP140.ENameFirstName

            udtEHSAccountPersonalInfo.CCCode1 = String.Format("{0}{1}", udcInputROP140.CCCode1, Me.udcChooseCCCode.SelectedCCCodeTail1)
            udtEHSAccountPersonalInfo.CCCode2 = String.Format("{0}{1}", udcInputROP140.CCCode2, Me.udcChooseCCCode.SelectedCCCodeTail2)
            udtEHSAccountPersonalInfo.CCCode3 = String.Format("{0}{1}", udcInputROP140.CCCode3, Me.udcChooseCCCode.SelectedCCCodeTail3)
            udtEHSAccountPersonalInfo.CCCode4 = String.Format("{0}{1}", udcInputROP140.CCCode4, Me.udcChooseCCCode.SelectedCCCodeTail4)
            udtEHSAccountPersonalInfo.CCCode5 = String.Format("{0}{1}", udcInputROP140.CCCode5, Me.udcChooseCCCode.SelectedCCCodeTail5)
            udtEHSAccountPersonalInfo.CCCode6 = String.Format("{0}{1}", udcInputROP140.CCCode6, Me.udcChooseCCCode.SelectedCCCodeTail6)

            'Retervie Chinese Name from Choose
            udcInputROP140.CCCode1 = udtEHSAccountPersonalInfo.CCCode1
            udcInputROP140.CCCode2 = udtEHSAccountPersonalInfo.CCCode2
            udcInputROP140.CCCode3 = udtEHSAccountPersonalInfo.CCCode3
            udcInputROP140.CCCode4 = udtEHSAccountPersonalInfo.CCCode4
            udcInputROP140.CCCode5 = udtEHSAccountPersonalInfo.CCCode5
            udcInputROP140.CCCode6 = udtEHSAccountPersonalInfo.CCCode6
            udcInputROP140.SetCName()
            udtEHSAccountPersonalInfo.CName = udcInputROP140.CName

            udtEHSAccountPersonalInfo.Gender = udcInputROP140.Gender
            udtEHSAccountPersonalInfo.DateofIssue = dtDateOfIssue
            udtEHSAccountPersonalInfo.DocCode = DocType.DocTypeModel.DocTypeCode.ROP140
            udtEHSAccountPersonalInfo.SetDOBTypeSelected(True)

            Me._systemMessage = Me.EHSAccountBasicValidation(DocType.DocTypeModel.DocTypeCode.ROP140, udtEHSAccount)
            If Not Me._systemMessage Is Nothing Then
                isValid = False
                Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
            End If
        End If

        Return isValid
    End Function

    'For PASS
    Private Function Step1b1PASSValidation(ByRef udtEHSAccount As EHSAccountModel) As Boolean
        Dim isValid As Boolean = True
        Dim udcInputPASS As ucInputPASS = Me.udcStep1b1InputDocumentType.GetPASSControl()
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)
        Dim strDOI As String = String.Empty
        udcInputPASS.SetErrorImage(ucInputDocTypeBase.BuildMode.Creation, False)
        udcInputPASS.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

        Me._systemMessage = Me._validator.chkEngName(udcInputPASS.ENameSurName, udcInputPASS.ENameFirstName, DocTypeModel.DocTypeCode.PASS)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputPASS.SetENameError(True)
            Me.udcMsgBoxErr.AddMessage(_systemMessage)
        End If

        Me._systemMessage = Me._validator.chkGender(udcInputPASS.Gender)
        If Not Me._systemMessage Is Nothing Then
            isValid = False
            udcInputPASS.SetGenderError(True)
            Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
        End If

        ' CRE20-023 Add Issue country/region to passport document [Start][Raiman]
        'Add Passport checking
        If udcInputPASS.PassportIssueRegion.Equals(String.Empty) Then
            Me._systemMessage = New Common.ComObject.SystemMessage("990000", "E", "00462")
            isValid = False
            udcInputPASS.SetPassportIssueRegionError(True)

            udcMsgBoxErr.AddMessage(Me._systemMessage, _
                                     New String() {"%en", "%tc", "%sc"}, _
                                     New String() {HttpContext.GetGlobalResourceObject("Text", "PassportIssueRegion", New System.Globalization.CultureInfo(CultureLanguage.English)), _
                                                   HttpContext.GetGlobalResourceObject("Text", "PassportIssueRegion", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)), _
                                                   HttpContext.GetGlobalResourceObject("Text", "PassportIssueRegion", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese)) _
                                                   })
        End If
        ' CRE20-023 Add Issue country/region to passport document [End][Raiman]


        If isValid Then
            udtEHSAccountPersonalInfo.ENameSurName = udcInputPASS.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputPASS.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputPASS.Gender
            udtEHSAccountPersonalInfo.DocCode = DocType.DocTypeModel.DocTypeCode.PASS
            udtEHSAccountPersonalInfo.SetDOBTypeSelected(True)

            ' CRE20-023 Add Issue country/region to passport document [Start][Raiman]
            udtEHSAccountPersonalInfo.PassportIssueRegion = udcInputPASS.PassportIssueRegion
            ' CRE20-023 Add Issue country/region to passport document [End][Raiman]

            Me._systemMessage = Me.EHSAccountBasicValidation(DocType.DocTypeModel.DocTypeCode.PASS, udtEHSAccount)
            If Not Me._systemMessage Is Nothing Then
                isValid = False
                Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
            End If
        End If

        Return isValid
    End Function

    ' CRE20-0022 (Immu record) [End][Martin]

    Private Function EHSAccountBasicValidation(ByVal strDocCode As String, ByVal udtEHSAccount As EHSAccountModel) As SystemMessage
        Return Me._udtClaimRulesBLL.CheckCreateEHSAccount(udtEHSAccount.SchemeCode, strDocCode, udtEHSAccount, ClaimRules.ClaimRulesBLL.Eligiblity.Check)
    End Function

#End Region

#Region "Step 1b1 CCCode related function/ Event "

    Private Function NeedPopupChineseNameDialog(ByVal strDocCode As String) As Boolean
        'isDiff is using for check the sessoion CCCode is same as current CCCode 
        'isDiff = true : sessoion CCCode <> current CCCode 
        Dim isDiff As Boolean = True

        Select Case strDocCode
            Case DocTypeModel.DocTypeCode.HKIC
                Dim udcInputHKIC As ucInputHKID = Me.udcStep1b1InputDocumentType.GetHKICControl()

                udcInputHKIC.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

                isDiff = Me.udcChooseCCCode.CCCodeDiff(udcInputHKIC.CCCode1, FunctCode, 1)

                If Not isDiff Then
                    isDiff = Me.udcChooseCCCode.CCCodeDiff(udcInputHKIC.CCCode2, FunctCode, 2)
                End If
                If Not isDiff Then
                    isDiff = Me.udcChooseCCCode.CCCodeDiff(udcInputHKIC.CCCode3, FunctCode, 3)
                End If
                If Not isDiff Then
                    isDiff = Me.udcChooseCCCode.CCCodeDiff(udcInputHKIC.CCCode4, FunctCode, 4)
                End If
                If Not isDiff Then
                    isDiff = Me.udcChooseCCCode.CCCodeDiff(udcInputHKIC.CCCode5, FunctCode, 5)
                End If
                If Not isDiff Then
                    isDiff = Me.udcChooseCCCode.CCCodeDiff(udcInputHKIC.CCCode6, FunctCode, 6)
                End If

            Case DocTypeModel.DocTypeCode.ROP140
                Dim udcInputROP140 As ucInputROP140 = Me.udcStep1b1InputDocumentType.GetROP140Control()

                udcInputROP140.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

                isDiff = Me.udcChooseCCCode.CCCodeDiff(udcInputROP140.CCCode1, FunctCode, 1)

                If Not isDiff Then
                    isDiff = Me.udcChooseCCCode.CCCodeDiff(udcInputROP140.CCCode2, FunctCode, 2)
                End If
                If Not isDiff Then
                    isDiff = Me.udcChooseCCCode.CCCodeDiff(udcInputROP140.CCCode3, FunctCode, 3)
                End If
                If Not isDiff Then
                    isDiff = Me.udcChooseCCCode.CCCodeDiff(udcInputROP140.CCCode4, FunctCode, 4)
                End If
                If Not isDiff Then
                    isDiff = Me.udcChooseCCCode.CCCodeDiff(udcInputROP140.CCCode5, FunctCode, 5)
                End If
                If Not isDiff Then
                    isDiff = Me.udcChooseCCCode.CCCodeDiff(udcInputROP140.CCCode6, FunctCode, 6)
                End If

        End Select

        Return isDiff

    End Function

    ' CRE20-0023 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    'For HKIC / ROP140 Case Only
    Private Sub udcStep1b1InputDocumentType_SelectChineseName_HKIC(ByVal udcInputDocumentType As ucInputDocTypeBase, _
                                                                   ByVal strDocCode As String, _
                                                                   ByVal sender As Object, _
                                                                   ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcStep1b1InputDocumentType.SelectChineseName_HKIC

        Dim systemMessage As SystemMessage = Nothing

        Dim udtAuditLogEntry As New AuditLogEntry(FunctCode, Me)

        Select Case strDocCode
            Case DocTypeModel.DocTypeCode.HKIC
                Dim udcInputHKID As ucInputHKID = CType(udcInputDocumentType, ucInputHKID)
                udcInputHKID.SetProperty(ucInputDocTypeBase.BuildMode.Creation)
                udtAuditLogEntry.AddDescripton("CCCode1", udcInputHKID.CCCode1)
                udtAuditLogEntry.AddDescripton("CCCode2", udcInputHKID.CCCode2)
                udtAuditLogEntry.AddDescripton("CCCode3", udcInputHKID.CCCode3)
                udtAuditLogEntry.AddDescripton("CCCode4", udcInputHKID.CCCode4)
                udtAuditLogEntry.AddDescripton("CCCode5", udcInputHKID.CCCode5)
                udtAuditLogEntry.AddDescripton("CCCode6", udcInputHKID.CCCode6)

                EHSAccountCreationBase.AuditLogStep1b1PromptCCCode(udtAuditLogEntry)

                systemMessage = Step1b1ShowCCCodeSelection(udcInputHKID, strDocCode, False)

            Case DocTypeModel.DocTypeCode.ROP140
                Dim udcInputROP140 As ucInputROP140 = CType(udcInputDocumentType, ucInputROP140)
                udcInputROP140.SetProperty(ucInputDocTypeBase.BuildMode.Creation)
                udtAuditLogEntry.AddDescripton("CCCode1", udcInputROP140.CCCode1)
                udtAuditLogEntry.AddDescripton("CCCode2", udcInputROP140.CCCode2)
                udtAuditLogEntry.AddDescripton("CCCode3", udcInputROP140.CCCode3)
                udtAuditLogEntry.AddDescripton("CCCode4", udcInputROP140.CCCode4)
                udtAuditLogEntry.AddDescripton("CCCode5", udcInputROP140.CCCode5)
                udtAuditLogEntry.AddDescripton("CCCode6", udcInputROP140.CCCode6)

                EHSAccountCreationBase.AuditLogStep1b1PromptCCCode(udtAuditLogEntry)

                systemMessage = Step1b1ShowCCCodeSelection(udcInputROP140, strDocCode, False)

        End Select

        If Not systemMessage Is Nothing Then
            Me.udcMsgBoxErr.AddMessage(systemMessage)
        End If

        Me.udcMsgBoxErr.BuildMessageBox(_strValidationFail)
    End Sub
    ' CRE20-0023 (Immu record) [End][Chris YIM]

    ' CRE20-0023 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Function Step1b1ShowCCCodeSelection(ByVal udcInputDocumentType As ucInputDocTypeBase, _
                                                ByVal strDocCode As String, _
                                                ByVal isPressedNext As Boolean) As SystemMessage

        Me.Session.Remove(SessionName.PressedNext)

        'User Pressed "Next" button to fire this function, if no sender passed 
        If isPressedNext Then
            Me.Session(SessionName.PressedNext) = isPressedNext
        End If

        Dim systemMessage As SystemMessage = Nothing

        Select strDocCode
            Case DocTypeModel.DocTypeCode.HKIC
                Dim udcInputHKID As ucInputHKID = CType(udcInputDocumentType, ucInputHKID)
                udcInputHKID.SetProperty(ucInputDocTypeBase.BuildMode.Creation)
                If udcInputHKID.CCCodeIsEmpty() Then

                    'No CCCode
                    udcInputHKID.SetCName(String.Empty)

                    'display error
                    systemMessage = New SystemMessage("990000", "E", "00143")
                    udcInputHKID.SetCCCodeError(True)
                    Me.udcChooseCCCode.Clean()
                Else
                    Me.udcChooseCCCode.DocCode = DocTypeModel.DocTypeCode.HKIC
                    Me.udcChooseCCCode.CCCode1 = udcInputHKID.GetCCCode(udcInputHKID.CCCode1, Me.udcChooseCCCode.getCCCodeFromSession(1, FunctCode))
                    Me.udcChooseCCCode.CCCode2 = udcInputHKID.GetCCCode(udcInputHKID.CCCode2, Me.udcChooseCCCode.getCCCodeFromSession(2, FunctCode))
                    Me.udcChooseCCCode.CCCode3 = udcInputHKID.GetCCCode(udcInputHKID.CCCode3, Me.udcChooseCCCode.getCCCodeFromSession(3, FunctCode))
                    Me.udcChooseCCCode.CCCode4 = udcInputHKID.GetCCCode(udcInputHKID.CCCode4, Me.udcChooseCCCode.getCCCodeFromSession(4, FunctCode))
                    Me.udcChooseCCCode.CCCode5 = udcInputHKID.GetCCCode(udcInputHKID.CCCode5, Me.udcChooseCCCode.getCCCodeFromSession(5, FunctCode))
                    Me.udcChooseCCCode.CCCode6 = udcInputHKID.GetCCCode(udcInputHKID.CCCode6, Me.udcChooseCCCode.getCCCodeFromSession(6, FunctCode))

                    'Bind related chinese words into Drop Down List
                    systemMessage = Me.udcChooseCCCode.BindCCCode()

                    If systemMessage Is Nothing Then
                        udcInputHKID.SetCCCodeError(False)
                        Me.ModalPopupExtenderChooseCCCode.Show()
                    Else
                        udcInputHKID.SetCCCodeError(True)
                    End If

                End If

            Case DocTypeModel.DocTypeCode.ROP140
                Dim udcInputROP140 As ucInputROP140 = CType(udcInputDocumentType, ucInputROP140)
                udcInputROP140.SetProperty(ucInputDocTypeBase.BuildMode.Creation)
                If udcInputROP140.CCCodeIsEmpty() Then

                    'No CCCode
                    udcInputROP140.SetCName(String.Empty)

                    'display error
                    systemMessage = New SystemMessage("990000", "E", "00143")
                    udcInputROP140.SetCCCodeError(True)
                    Me.udcChooseCCCode.Clean()
                Else
                    Me.udcChooseCCCode.DocCode = DocTypeModel.DocTypeCode.ROP140
                    Me.udcChooseCCCode.CCCode1 = udcInputROP140.GetCCCode(udcInputROP140.CCCode1, Me.udcChooseCCCode.getCCCodeFromSession(1, FunctCode))
                    Me.udcChooseCCCode.CCCode2 = udcInputROP140.GetCCCode(udcInputROP140.CCCode2, Me.udcChooseCCCode.getCCCodeFromSession(2, FunctCode))
                    Me.udcChooseCCCode.CCCode3 = udcInputROP140.GetCCCode(udcInputROP140.CCCode3, Me.udcChooseCCCode.getCCCodeFromSession(3, FunctCode))
                    Me.udcChooseCCCode.CCCode4 = udcInputROP140.GetCCCode(udcInputROP140.CCCode4, Me.udcChooseCCCode.getCCCodeFromSession(4, FunctCode))
                    Me.udcChooseCCCode.CCCode5 = udcInputROP140.GetCCCode(udcInputROP140.CCCode5, Me.udcChooseCCCode.getCCCodeFromSession(5, FunctCode))
                    Me.udcChooseCCCode.CCCode6 = udcInputROP140.GetCCCode(udcInputROP140.CCCode6, Me.udcChooseCCCode.getCCCodeFromSession(6, FunctCode))

                    'Bind related chinese words into Drop Down List
                    systemMessage = Me.udcChooseCCCode.BindCCCode()

                    If systemMessage Is Nothing Then
                        udcInputROP140.SetCCCodeError(False)
                        Me.ModalPopupExtenderChooseCCCode.Show()
                    Else
                        udcInputROP140.SetCCCodeError(True)
                    End If

                End If

        End Select

        Return systemMessage

    End Function
    ' CRE20-0023 (Immu record) [End][Chris YIM]

#End Region

#Region "Step 1b2 Events 'Confirm Detail"

    Protected Sub chkStep1b2Declare_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkStep1b2Declare.CheckedChanged
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        EHSAccountCreationBase.AuditLogStep1b2DeclareClick(_udtAuditLogEntry, chkStep1b2Declare.Checked)
        ' CRE11-021 log the missed essential information [End]

        Me.EnableConfirmButton(Me.chkStep1b2Declare.Checked, Me.btnStep1b2Confirm)
    End Sub

    Protected Sub btnStep1b2Back_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnStep1b2Back.Click
        Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1b1
    End Sub

    Private Sub btnStep1b2Cancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnStep1b2Cancel.Click

        Me.ModalPopupConfirmCancel.Show()

    End Sub

    Protected Sub btnStep1b2Confirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnStep1b2Confirm.Click
        Dim practiceDisplay As PracticeDisplayModel = Me._udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)
        Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
        Dim udtSmartIDContent As SmartIDContentModel = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode)
        Dim udtDataEntry As DataEntryUserModel = Nothing
        Dim isValid As Boolean = False
        Dim strVoucherAccountID As String = commfunct.generateSystemNum("C")

        EHSAccountCreationBase.AuditLogStep1b2Start(_udtAuditLogEntry)

        Me._udtSessionHandler.CurrentUserGetFromSession(Me._udtSP, udtDataEntry)

        Me._udtEHSAccountBLL = New EHSAccountBLL()
        udtEHSAccount.VoucherAccID = strVoucherAccountID
        udtEHSAccount.SchemeCode = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode).SchemeCode
        udtEHSAccount.CreateSPPracticeDisplaySeq = practiceDisplay.PracticeID

        With udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode)
            .CreateDtm = Date.Now()
            If Not udtSmartIDContent Is Nothing AndAlso udtSmartIDContent.IsReadSmartID Then
                .CreateBySmartID = True

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                .SmartIDVer = udtSmartIDContent.SmartIDVer
            Else
                .CreateBySmartID = False
                .SmartIDVer = String.Empty
            End If
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
        End With

        Dim udtSystemMessage As Common.ComObject.SystemMessage = Nothing
        Try
            udtSystemMessage = Me._udtEHSClaimBLL.CreateTemporaryEHSAccount(Me._udtSP, udtDataEntry, udtEHSAccount)
        Catch eSql As SqlClient.SqlException
            If eSql.Number = 50000 Then
                udtSystemMessage = New SystemMessage("990001", "D", eSql.Message)
            Else
                Throw eSql
            End If
        End Try

        If udtSystemMessage Is Nothing Then
            udtEHSAccount = Me._udtEHSAccountBLL.LoadTempEHSAccountByVRID(strVoucherAccountID)
            udtEHSAccount.SetSearchDocCode(udtEHSAccount.EHSPersonalInformationList(0).DocCode)

            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.EHSPersonalInformationList(0).DocCode).HKICSymbol = _udtSessionHandler.HKICSymbolGetFormSession(FunctCode)
            ' CRE17-010 (OCSSS integration) [End][Chris YIM]

            Me._udtSessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctCode)

            EHSAccountCreationBase.AuditLogStep1b2Complete(_udtAuditLogEntry, udtEHSAccount)

            'CRE20-0xx Immue record [Start][Nichole]
            If Me.ClaimMode = ClaimMode.COVID19 Then
                btnStep1cProceedToClaim_Click(Nothing, Nothing)
            Else
                Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1c
            End If
            'CRE20-00x Immu Record [End][Nichole]
        Else
            ' To Do: Display Error!
            Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
            Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail, _udtAuditLogEntry, Common.Component.LogID.LOG00031, "Confirm Detail Failed")

        End If
    End Sub

    Private Sub SetupStep1b2(ByVal udtEHSAccount As EHSAccountModel)
        If Me._udtSessionHandler.EHSClaimStepsGetFromSession(FunctCode) <> ActiveViewIndex.Step1b2 Then
            Me._blnIsRequireHandlePageRefresh = True
            Return
        End If

        Me.btnStep1b2Cancel.Visible = False
        Me.btnStep1b2Back.Visible = True
        Me.btnStep1b2Back.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "BackBtn")
        Me.btnStep1b2Back.AlternateText = Me.GetGlobalResourceObject("AlternateText", "BackBtn")

        If Not udtEHSAccount Is Nothing Then
            Me.udcStep1b2ReadOnlyDocumnetType.DocumentType = udtEHSAccount.SearchDocCode
            Me.udcStep1b2ReadOnlyDocumnetType.EHSAccount = udtEHSAccount
            Me.udcStep1b2ReadOnlyDocumnetType.Vertical = True
            Me.udcStep1b2ReadOnlyDocumnetType.MaskIdentityNo = False
            Me.udcStep1b2ReadOnlyDocumnetType.ShowAccountRefNo = False
            Me.udcStep1b2ReadOnlyDocumnetType.ShowTempAccountNotice = False
            Me.udcStep1b2ReadOnlyDocumnetType.ShowAccountCreationDate = False
            Me.udcStep1b2ReadOnlyDocumnetType.Built()
        End If
    End Sub

#End Region

#Region "Step 1b3 SmartID Account Rectify"

    Private Sub btnStep1b3Cancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnStep1b3Cancel.Click
        Me.ModalPopupConfirmCancel.Show()
    End Sub

    Private Sub btnStep1b3Confirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnStep1b3Confirm.Click
        Dim udtSmartIDContent As SmartIDContentModel = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode)
        Dim practiceDisplay As PracticeDisplayModel = Me._udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)
        Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Dim udtSchemeClaim As Scheme.SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
        Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
        Dim udtDataEntry As DataEntryUserModel = Nothing
        Dim isValid As Boolean = False
        Dim strVoucherAccountID As String = commfunct.generateSystemNum("C")
        Dim udtSystemMessage As Common.ComObject.SystemMessage = Nothing
        Dim isGoToCompleteAccountCreation As Boolean = True

        Dim blnNeedImmDValidation As Boolean = False

        Me._udtEHSAccountBLL = New EHSAccountBLL()
        EHSAccountCreationBase.AuditLogStep1b3Start(_udtAuditLogEntry, udtSmartIDContent)
        Me._udtSessionHandler.CurrentUserGetFromSession(Me._udtSP, udtDataEntry)

        Select Case udtSmartIDContent.SmartIDReadStatus
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Case SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_DiffDOI_LargerDOI, _
                    SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_NotCreateBySmartID_SameDOB, _
                    SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOIDOB_CreateBySmartID_WithoutGender_SameName

                If udtSmartIDContent.SmartIDReadStatus = SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_DiffDOI_LargerDOI Then
                    blnNeedImmDValidation = True
                    'ElseIf udtSmartIDContent.SmartIDReadStatus = SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_NotCreateBySmartID Then
                Else
                    blnNeedImmDValidation = False
                End If
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                'udtSmartIDContent.EHSAccount can be modify -> Update Gender
                'udtSmartIDContent.EHSValidatedAccount can not be modify -> for smartID Checking
                'Session EHSAccount Is the search result 
                EHSAccountCreationBase.AuditLogStep1b3Complete(_udtAuditLogEntry, udtEHSAccount, udtSmartIDContent, True)
                'udtSmartIDContent.IsCreateNewEHSAccount = True
                udtSmartIDContent.EHSValidatedAccount = udtEHSAccount
                Try
                    udtEHSAccount = Me._udtEHSClaimBLL.CreateAmendEHSAccount(Me._udtSP, udtDataEntry, udtSmartIDContent.EHSValidatedAccount, udtSmartIDContent.EHSAccount, udtSchemeClaim.SchemeCode, practiceDisplay.PracticeID, blnNeedImmDValidation)
                Catch eSQL As SqlClient.SqlException
                    If eSQL.Number = 50000 Then
                        udtSystemMessage = New SystemMessage("990001", "D", eSQL.Message)
                    Else
                        Throw eSQL
                    End If
                End Try

                udtEHSAccount.SetSearchDocCode(udtSmartIDContent.EHSAccount.SearchDocCode)
                udtSmartIDContent.EHSAccount = udtEHSAccount
                ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                ' ----------------------------------------------------------
                udtSmartIDContent.EHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).HKICSymbol = _udtSessionHandler.HKICSymbolGetFormSession(FunctCode)
                ' CRE17-010 (OCSSS integration) [End][Chris YIM]

                Me._udtSessionHandler.SmartIDContentSaveToSession(FunctCode, udtSmartIDContent)
                Me._udtSessionHandler.EHSAccountSaveToSession(udtSmartIDContent.EHSAccount, FunctCode)
                isGoToCompleteAccountCreation = False

                Me.BackToClaim(True)
            Case Else
                'udtSmartIDContent.EHSAccount Gender updated
                udtSmartIDContent.EHSAccount.VoucherAccID = strVoucherAccountID
                udtSmartIDContent.EHSAccount.SchemeCode = udtSchemeClaim.SchemeCode
                udtSmartIDContent.EHSAccount.CreateSPPracticeDisplaySeq = practiceDisplay.PracticeID

                With udtSmartIDContent.EHSAccount.EHSPersonalInformationList.Filter(udtSmartIDContent.EHSAccount.SearchDocCode)
                    .CreateDtm = Date.Now()
                End With

                Try
                    udtSystemMessage = Me._udtEHSClaimBLL.CreateTemporaryEHSAccount(Me._udtSP, udtDataEntry, udtSmartIDContent.EHSAccount)
                Catch eSql As SqlClient.SqlException
                    If eSql.Number = 50000 Then
                        udtSystemMessage = New SystemMessage("990001", "D", eSql.Message)
                    Else
                        Throw eSql
                    End If
                End Try
        End Select

        If udtSystemMessage Is Nothing AndAlso isGoToCompleteAccountCreation Then

            udtSmartIDContent.EHSAccount = Me._udtEHSAccountBLL.LoadTempEHSAccountByVRID(strVoucherAccountID)
            udtSmartIDContent.EHSAccount.SetSearchDocCode(udtSmartIDContent.EHSAccount.EHSPersonalInformationList(0).DocCode)
            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            udtSmartIDContent.EHSAccount.EHSPersonalInformationList.Filter( _
                udtSmartIDContent.EHSAccount.EHSPersonalInformationList(0).DocCode).HKICSymbol = _udtSessionHandler.HKICSymbolGetFormSession(FunctCode)
            ' CRE17-010 (OCSSS integration) [End][Chris YIM]

            Me._udtSessionHandler.EHSAccountSaveToSession(udtSmartIDContent.EHSAccount, FunctCode)
            udtEHSAccount = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)

            Select Case udtSmartIDContent.SmartIDReadStatus
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Case BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_DiffDOI_LargerDOI, _
                            BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_NotCreateBySmartID_SameDOB, _
                            BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOIDOB_CreateBySmartID_WithoutGender_SameName
                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
                    EHSAccountCreationBase.AuditLogStep1b3Complete(_udtAuditLogEntry, udtEHSAccount, udtSmartIDContent, True)
                    Me.BackToClaim(True)
                Case Else
                    EHSAccountCreationBase.AuditLogStep1b3Complete(_udtAuditLogEntry, udtEHSAccount, udtSmartIDContent, False)
                    Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1c
            End Select
        Else
            ' To Do: Display Error!
            Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
            Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail, _udtAuditLogEntry, Common.Component.LogID.LOG00058, "Confirm SmartID Detail Failed")

        End If

        'CRE20-00xx Immue record SmartIC card [Start][Nichole]
        If Me.ClaimMode = ClaimMode.COVID19 Then
            If Not udtSmartIDContent Is Nothing AndAlso udtSmartIDContent.IsReadSmartID Then

                Me.btnStep1cProceedToClaim_Click(Nothing, Nothing)
            End If
        End If
        'CRE20-00xx Immue record SmartIC card [End][Nichole]
    End Sub

    'Private Sub chkStep1b3Declare_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkStep1b3Declare.CheckedChanged
    '    Me.EnableConfirmButton(chkStep1b3Declare.Checked, Me.btnStep1b3Confirm)
    'End Sub

    Private Sub udcStep1b3InputDocumentType_SelectGender(ByVal udcInputHKID As ucInputDocTypeBase, ByVal sender As Object, ByVal e As System.EventArgs) Handles udcStep1b3InputDocumentType.SelectGender

        Dim udcSmartIDContent As BLL.SmartIDContentModel = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode)
        Dim strGender As String = String.Empty
        Dim isShowSmartIDDiff As Boolean = False

        Select Case udcSmartIDContent.SmartIDReadStatus
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Case SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_DiffDOI_LargerDOI, _
                    SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_NotCreateBySmartID_SameDOB, _
                    SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOIDOB_CreateBySmartID_WithoutGender_SameName
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
                Dim udcInputHKIDSmartID As ucInputHKIDSmartID = udcInputHKID
                udcInputHKIDSmartID.SetProperty(ucInputDocTypeBase.BuildMode.Creation)
                strGender = udcInputHKIDSmartID.Gender

                Me.panStep1b3Declare.Visible = False
                isShowSmartIDDiff = True

            Case Else
                Dim udcInputHKIDSmartIDSignal As ucInputHKIDSmartIDSignal = udcInputHKID
                udcInputHKIDSmartIDSignal.SetProperty(ucInputDocTypeBase.BuildMode.Creation)
                strGender = udcInputHKIDSmartIDSignal.Gender

                Me.panStep1b3Declare.Visible = True
                isShowSmartIDDiff = False
        End Select

        If Not String.IsNullOrEmpty(strGender) Then

            If Not isShowSmartIDDiff Then

                Me.chkStep1b3Declare.Enabled = True
                If Me.chkStep1b3Declare.Checked Then
                    Me.EnableConfirmButton(True, Me.btnStep1b3Confirm)
                Else
                    Me.EnableConfirmButton(False, Me.btnStep1b3Confirm)
                End If
            Else
                Me.EnableConfirmButton(True, Me.btnStep1b3Confirm)
            End If

            udcSmartIDContent.EHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).Gender = strGender
            Me._udtSessionHandler.SmartIDContentSaveToSession(FunctCode, udcSmartIDContent)

        Else
            Me.chkStep1b3Declare.Enabled = False
            Me.EnableConfirmButton(False, Me.btnStep1b3Confirm)
        End If

    End Sub

    'Private Sub btnStep1b3PrintChangeForm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnStep1b3PrintChangeForm.Click
    '    Dim udcSmartIDContent As BLL.SmartIDContentModel = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode)
    '    Dim strPrintDateTime As String = String.Format("DH_HCV103{0}{1}{2}{3}{4}{5}{6}", Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, Now.Second, Now.Millisecond)
    '    'Save the current function code to session (will be removed in the printout form)

    '    'udcSmartIDContent.IsPrintedChangeForm = True
    '    Me.chkStep1b3Declare.Enabled = True
    '    Me._udtSessionHandler.SmartIDContentSaveToSession(FunctCode, udcSmartIDContent)
    '    Me._udtSessionHandler.EHSClaimPrintoutFunctionCodeSaveToSession(FunctCode)

    '    ScriptManager.RegisterStartupScript(Me, Page.GetType, "ChangeFormScript", "javascript:openNewWin('../Printout/VoucherAccountChangeForm_VR.aspx?TID=" + strPrintDateTime + "');", True)

    'End Sub

    Private Sub SetupStep1b3(ByVal udtEHSAccount As EHSAccountModel, ByVal activeViewChanged As Boolean)
        If Me._udtSessionHandler.EHSClaimStepsGetFromSession(FunctCode) <> ActiveViewIndex.Step1b3 Then
            Me._blnIsRequireHandlePageRefresh = True
            Return
        End If

        Dim udcSmartIDContent As BLL.SmartIDContentModel = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode)
        Dim isShowSmartIDDiff As Boolean = False

        'Me.rbStep1b3PrintClaimConsentFormLanguage.Items.FindByValue(PrintOptionValue.Chi).Text = Me.GetGlobalResourceObject("Text", "Chinese")
        'Me.rbStep1b3PrintClaimConsentFormLanguage.Items.FindByValue(PrintOptionValue.Eng).Text = Me.GetGlobalResourceObject("Text", "English")

        'Me.lblStep1b3Consent.Text = Me.GetGlobalResourceObject("Text", "ValidatedACWithDiffInfo")
        Me.btnStep1b3Cancel.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "CancelBtn")
        Me.btnStep1b3Cancel.AlternateText = Me.GetGlobalResourceObject("AlternateText", "CancelBtn")

        Me.btnStep1b3Confirm.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmBtn")
        Me.btnStep1b3Confirm.AlternateText = Me.GetGlobalResourceObject("AlternateText", "ConfirmBtn")

        Me.udcStep1b3InputDocumentType.EHSAccount = udtEHSAccount
        Me.udcStep1b3InputDocumentType.DocType = DocType.DocTypeModel.DocTypeCode.HKIC
        Me.udcStep1b3InputDocumentType.Mode = ucInputDocTypeBase.BuildMode.Creation
        Me.udcStep1b3InputDocumentType.FillValue = True
        Me.udcStep1b3InputDocumentType.ActiveViewChanged = activeViewChanged
        Me.udcStep1b3InputDocumentType.SchemeClaim = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
        Me.udcStep1b3InputDocumentType.SmartIDContent = udcSmartIDContent
        Me.udcStep1b3InputDocumentType.FixEnglishNameGender = Not IsNothing(_udtSessionHandler.VREEHSAccountGetFromSession) _
            AndAlso _udtSessionHandler.VREEHSAccountGetFromSession.EHSPersonalInformationList(0).ENameSurName <> String.Empty
        Me.udcStep1b3InputDocumentType.Built()

        'Me.btnStep1b3PrintChangeForm.Enabled = False
        'Me.rbStep1b3PrintClaimConsentFormLanguage.Enabled = False
        'Me.chkStep1b3Declare.Enabled = False
        Me.EnableConfirmButton(False, Me.btnStep1b3Confirm)

        Select Case udcSmartIDContent.SmartIDReadStatus
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Case SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_DiffDOI_LargerDOI, _
                    SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_NotCreateBySmartID_SameDOB, _
                    SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOIDOB_CreateBySmartID_WithoutGender_SameName
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
                Me.panStep1b3Declare.Visible = False
                Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Information
                Me.udcInfoMsgBox.AddMessage(New SystemMessage("990000", "I", "00024"))
                Me.udcInfoMsgBox.BuildMessageBox()
                'Me.panStep1b3Remind.Visible = True

                isShowSmartIDDiff = True

            Case Else
                'Me.btnStep1b3PrintChangeForm.Visible = False
                'Me.rbStep1b3PrintClaimConsentFormLanguage.Visible = False
                Me.panStep1b3Declare.Visible = True
                'Me.panStep1b3Remind.Visible = False
                Me.chkStep1b3Declare.Text = Me.GetGlobalResourceObject("Text", "ProvidedInfoTrueVAForm")

                isShowSmartIDDiff = False

        End Select

        If Not String.IsNullOrEmpty(udcSmartIDContent.EHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).Gender) Then

            If Not isShowSmartIDDiff Then
                Me.chkStep1b3Declare.Enabled = True

                If Me.chkStep1b3Declare.Checked Then
                    Me.EnableConfirmButton(True, Me.btnStep1b3Confirm)
                Else
                    Me.EnableConfirmButton(False, Me.btnStep1b3Confirm)
                End If
            Else
                Me.EnableConfirmButton(True, Me.btnStep1b3Confirm)
            End If
        Else
            Me.chkStep1b3Declare.Enabled = False
            Me.EnableConfirmButton(False, Me.btnStep1b3Confirm)
        End If
    End Sub

#End Region

#Region "Step 1c Events confirm Account Creation"

    Protected Sub btnStep1cProceedToClaim_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnStep1cProceedToClaim.Click
        EHSAccountCreationBase.AuditLogStep1cProccedToclaim(_udtAuditLogEntry)

        Me.BackToClaim(True)
    End Sub

    Protected Sub btnStep1cNextCreation_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnStep1cNextCreation.Click
        EHSAccountCreationBase.AuditLogStep1cCreateNewAccount(_udtAuditLogEntry)
        Me.BackToClaim(False)

    End Sub

    Private Sub SetupStep1c(ByVal udtEHSAccount As EHSAccountModel, ByVal activeViewChanged As Boolean)
        If Me._udtSessionHandler.EHSClaimStepsGetFromSession(FunctCode) <> ActiveViewIndex.Step1c Then
            Me._blnIsRequireHandlePageRefresh = True
            Return
        End If

        Me._systemMessage = New Common.ComObject.SystemMessage(FunctCode, "I", "00001")
        Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Complete
        Me.udcInfoMsgBox.AddMessage(Me._systemMessage)
        Me.udcInfoMsgBox.BuildMessageBox()

        Dim udtSmartIDContent As BLL.SmartIDContentModel = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode)

        If Not udtEHSAccount Is Nothing Then
            '==================================================================== Code for SmartID ============================================================================
            If Not udtSmartIDContent Is Nothing Then
                udtSmartIDContent.HighLightGender = False
            End If
            Me.udcStep1cReadOnlyDocumnetType.SmartIDContent = udtSmartIDContent
            '===================================================================================================================================================================
            Me.udcStep1cReadOnlyDocumnetType.DocumentType = udtEHSAccount.SearchDocCode
            Me.udcStep1cReadOnlyDocumnetType.EHSAccount = udtEHSAccount
            Me.udcStep1cReadOnlyDocumnetType.Vertical = True
            Me.udcStep1cReadOnlyDocumnetType.MaskIdentityNo = False
            Me.udcStep1cReadOnlyDocumnetType.ShowAccountRefNo = True
            Me.udcStep1cReadOnlyDocumnetType.ShowTempAccountNotice = False
            Me.udcStep1cReadOnlyDocumnetType.ShowAccountCreationDate = True
            Me.udcStep1cReadOnlyDocumnetType.Built()
        End If

        If activeViewChanged Then
            EHSAccountCreationBase.AuditLogStep1cComplete(_udtAuditLogEntry, udtEHSAccount)
        End If

    End Sub

#End Region

#Region "Other functions"

    ''' <summary>
    ''' Show Change Practice button
    ''' </summary>
    Private Sub ShowChangePracticeButton(ByVal imageButton As ImageButton)
        Dim udtPracticeDisplays As BLL.PracticeDisplayModelCollection = Me._udtSessionHandler.PracticeDisplayListGetFromSession(FunctCode)
        If udtPracticeDisplays.Count > 1 Then
            imageButton.Visible = True
        Else
            imageButton.Visible = False
        End If

    End Sub

    ''' <summary>
    ''' Back to Claim Page
    ''' </summary>
    Private Sub BackToClaim(ByVal proceedToClaim As Boolean)
        If Not proceedToClaim Then
            Me._udtSessionHandler.EHSClaimStepsRemoveFromSession(FunctCode)
            Me._udtSessionHandler.EHSClaimSessionRemove(FunctCode)
            Me._udtSessionHandler.FromVaccinationRecordEnquiryRemoveFromSession()
            Me._udtSessionHandler.SmartIDContentRemoveFormSession(FunctCode)
            Me._udtSessionHandler.ClearVREClaim()
        Else
            Me._udtSessionHandler.ExceedDocTypeLimitRemoveFromSession()
            Me._udtSessionHandler.NotMatchAccountExistRemoveFromSession()
            Me._udtSessionHandler.CCCodeRemoveFromSession(FunctCode)
            Me._udtSessionHandler.AccountCreationProceedClaimSaveToSession(True)
        End If

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        RedirectHandler.ToURL(EHSClaim)

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End
    End Sub

    Private Function SmartIDShowRealID() As Boolean
        Dim udtGeneralFunction As New GeneralFunction
        Dim strParmValue As String = String.Empty
        udtGeneralFunction.getSystemParameter("SmartIDShowRealID", strParmValue, String.Empty)
        Return strParmValue.Trim = "Y"
    End Function

    ''==================================================================== Code for SmartID ============================================================================
    ''-----------------------------------------------------------------------------------------------------------------------------
    ''Cinstrct Inbox Msg for notify back office user about there is an amendment of validated account read from Smart IC
    ''-----------------------------------------------------------------------------------------------------------------------------
    'Private Sub ConstructMsgChangeOfParticulars(ByRef udtDB As Common.DataAccess.Database, ByRef udtMessageCollection As Common.Component.Inbox.MessageModelCollection, ByRef udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection, ByVal strACEngName As String)
    '    Dim udtGeneralF As New Common.ComFunction.GeneralFunction
    '    Dim udtHCVUUserBLL As New Common.Component.HCVUUser.HCVUUserBLL

    '    Dim udtParamFunction As New Common.ComFunction.ParameterFunction()

    '    udtMessageCollection = New Common.Component.Inbox.MessageModelCollection
    '    udtMessageReaderCollection = New Common.Component.Inbox.MessageReaderModelCollection

    '    'Get Receiver
    '    Dim dtUser As DataTable = udtHCVUUserBLL.GetHCVUUserByRoleTypeSchemeCode("99", "ALL")


    '    If dtUser.Rows.Count > 0 Then
    '        'Retrieve Message Templare
    '        Dim udtInternetMailBLL As New Common.Component.InternetMail.InternetMailBLL
    '        Dim udtMailTemplate As Common.Component.InternetMail.MailTemplateModel = udtInternetMailBLL.GetMailTemplate(udtDB, "AC_ChgInfo")
    '        Dim dtmCurrent As DateTime = udtGeneralF.GetSystemDateTime()

    '        'Construct Message & MessageReader
    '        Dim udtMessage As New Common.Component.Inbox.MessageModel
    '        udtMessage.MessageID = udtGeneralF.generateInboxMsgID

    '        Dim paramsContent As New ParameterCollection
    '        paramsContent.AddParam("Name", strACEngName)

    '        udtMessage.Subject = udtMailTemplate.MailSubjectEng
    '        udtMessage.Message = udtParamFunction.GetParsedStringByparameter(udtMailTemplate.MailBodyEng, paramsContent)

    '        udtMessage.CreateBy = "EHCVS"
    '        udtMessage.CreateDtm = dtmCurrent
    '        udtMessageCollection.Add(udtMessage)

    '        For Each dr As DataRow In dtUser.Rows
    '            Dim udtMessageReader As New Common.Component.Inbox.MessageReaderModel()
    '            udtMessageReader.MessageID = udtMessage.MessageID
    '            udtMessageReader.MessageReader = CStr(dr.Item("User_ID")).Trim
    '            udtMessageReader.UpdateBy = "EHCVS"
    '            udtMessageReader.UpdateDtm = dtmCurrent

    '            udtMessageReaderCollection.Add(udtMessageReader)
    '        Next
    '    End If
    '    '==================================================================================================================================================================

    'End Sub
#End Region

#Region "Implement IWorkingData (CRE11-004)"

    Public Overrides Function GetDocCode() As String
        If GetEHSAccount() Is Nothing Then Return Nothing
        If GetEHSAccount.SearchDocCode = String.Empty Then Return Nothing
        Return GetEHSAccount.SearchDocCode
    End Function

    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Return Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
    End Function

    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)
    End Function

    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        Dim udtSP As ServiceProviderModel = Nothing
        Dim udtDataEntry As DataEntryUserModel = Nothing

        Me._udtSessionHandler.CurrentUserGetFromSession(udtSP, udtDataEntry)

        Return udtSP
    End Function

#End Region
End Class


