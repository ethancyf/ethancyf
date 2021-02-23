Imports Common.ComFunction
Imports Common.Component.EHSAccount
'Imports Common.Component.VoucherRecipientAccount
Imports HCSP.BLL
Imports Common.Component
Imports Common.ComObject
Imports Common.Validation
Imports Common.Format
Imports Common.Component.DataEntryUser
Imports Common.Component.ServiceProvider
Imports Common.Component.UserAC
Imports Common.Component.DocType
Imports Common.Component.Scheme
Imports Common.Component.Practice

Imports SP = HCSP

Namespace Text

    Partial Public Class EHSAccountCreationV1
        Inherits EHSAccountCreationBase

        Public Const FunctCode As String = Common.Component.FunctCode.FUNT020202
        Public Const EHSClaim As String = "EHSClaimV1.aspx"

        'Text only version
        Private Const TextConfirmCancelBox As Integer = 5
        Private Const TextConfirmModifyBox As Integer = 6
        Private Const TextConfirmOnlyBox As Integer = 7
        Private Const TextSelectPractice As Integer = 8
        Private Const TextConfirmDeclare As Integer = 9
        Private Const TextInputTips As Integer = 10
        Private Const Step1b3 As Integer = 15

        Private _udtEHSAccount As EHSAccountModel
        Private _udtEHSClaimBLL As New EHSClaimBLL()
        'Private _udtClaimRulesBLL As New ClaimRules.ClaimRulesBLL()
        Private _systemMessage As SystemMessage
        Private _udtFormatter As Formatter
        Private _validator As Validator = New Validator
        Private _udtEHSAccountBLL As EHSAccountBLL
        Private _udtPracticeAcctBLL As PracticeBankAcctBLL = New PracticeBankAcctBLL
        'Private _udtSessionHandler As BLL.SessionHandler = New BLL.SessionHandler
        Private _strValidationFail As String = "ValidationFail"
        Private _udtUserAC As UserACModel = New UserACModel()
        Private _udtSP As ServiceProviderModel = Nothing
        Private _udtAuditLogEntry As New AuditLogEntry(FunctCode, Me)

        Private bUnchangeInputUI As Boolean = True

        Private Class SessionName
            'Using in Enter Account detail screen
            'If PressedNext is true, screen will go to Confirm Detail "Step1b2" Page
            Public Const PressedNext As String = "PressedNext"

        End Class

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If MyBase.IsPageRefreshed Then
                Return
            End If

            'Get Current USer Account for check Session Expired
            Me._udtUserAC = UserACBLL.GetUserAC

            'Initialize MasterPage
            CType(Me.Master.FindControl(ClaimVoucherMaster.ControlName.lblTitle), Label).Text = Me.GetGlobalResourceObject("Text", "EVoucherSystem")
            CType(Me.Master.FindControl(ClaimVoucherMaster.ControlName.lblSubTitle), Label).Text = Me.GetGlobalResourceObject("AlternateText", "VRACreationBanner")

            Dim udtSelectedPracticeDisplay As PracticeDisplayModel = Nothing
            Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = Nothing
            Me._udtEHSAccount = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)

            If Not Page.IsPostBack AndAlso _udtEHSAccount Is Nothing Then
                ' normal flow, EHSAccount will no be null, return to EHS Account Search
                Me._udtSessionHandler.AccountCreationProceedClaimRemoveFromSession()

                '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

                RedirectHandler.ToURL(EHSClaim)

                '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End
                Return
            End If

            Me.BuildMenuItem()

            If Not Me.IsPostBack Then
                EHSAccountCreationBase.AuditLogPageLoad(_udtAuditLogEntry)

                'Get the Current Practice ------------------------------------------------------------
                udtSelectedPracticeDisplay = Me._udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)

                Dim udtSmartIDContent As SmartIDContentModel = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode)

                If Not udtSmartIDContent Is Nothing AndAlso udtSmartIDContent.IsReadSmartID Then
                    '==================================================================== Code for SmartID ============================================================================
                    Select Case udtSmartIDContent.SmartIDReadStatus
                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                        ' ----------------------------------------------------------------------------------------
                        Case SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_DiffDOI_LargerDOI, _
                             SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_NotCreateBySmartID_SameDOB, _
                             SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOIDOB_CreateBySmartID_WithoutGender_SameName
                            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                            ' Different Detail w/ Newer DOI
                            ' Different Detail w/ Same DOI & DOB, Previous Account Not Created By Smart IC
                            ' Different Detail w/ Same DOI & DOB, Previous Account Created By Smart IC with Same Name but Diff Gender
                            ' - Compare Value and Create Account
                            udtSmartIDContent.EHSValidatedAccount = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
                            Me._udtSessionHandler.SmartIDContentSaveToSession(FunctCode, udtSmartIDContent)

                            Me.mvAccountCreation.ActiveViewIndex = Step1b3

                            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                            ' ----------------------------------------------------------------------------------------
                        Case SmartIDHandler.SmartIDResultStatus.DocTypeNotExist, _
                             SmartIDHandler.SmartIDResultStatus.EHSAccountNotfound, _
                             SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_NotCreateBySmartID, _
                             SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_DiffDOIDOB, _
                             SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB_WithoutGender_SameName
                            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                            ' Temp Account Diff Detail Found | No Account
                            ' - Create Account
                            'MyBase.SessionHandler.EHSAccountSaveToSession(udtSmartIDContent.EHSAccount, FunctCode)

                            Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1a1

                        Case SmartIDHandler.SmartIDResultStatus.TempAccountExist_SameDetail
                            ' Same Detail Temp Account Located
                            'MyBase.SessionHandler.EHSAccountSaveToSession(udtSmartIDContent.EHSAccount, FunctCode)
                            Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1a2

                    End Select
                    '===================================================================================================================================================================
                Else
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

            Else
                Me.StepRenderLanguage(Me._udtEHSAccount)
            End If
        End Sub

        Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)

            ' Page refresh, return to EHSClaim-> Search Account
            If MyBase.IsPageRefreshed Then
                'Me._udtSessionHandler.AccountCreationProceedClaimRemoveFromSession()
                'Me._udtSessionHandler.EHSAccountRemoveFromSession(FunctCode)
                Me._udtSessionHandler.EHSClaimSessionRemove(FunctCode)

                '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

                RedirectHandler.ToURL(EHSClaim)

                '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End
            End If

            MyBase.OnPreRender(e)
        End Sub

        Protected Overrides Sub StepRenderLanguage(ByVal udtEHSAccount As EHSAccountModel)
            CType(Me.Master.FindControl(ClaimVoucherMaster.ControlName.LblClaimVoucherStep), Label).Text = Me.GetStepText()

            Select Case mvAccountCreation.ActiveViewIndex
                Case ActiveViewIndex.Step1a1
                    'Hight Light Time Line
                    Me.SetupStep1a1(udtEHSAccount)

                Case ActiveViewIndex.Step1a2
                    'Hight Light Time Line
                    Me.SetupStep1a2(udtEHSAccount)

                Case ActiveViewIndex.Step1b1
                    'Hight Light Time Line
                    Me.SetupStep1b1(udtEHSAccount, True)

                Case ActiveViewIndex.Step1b2
                    'Hight Light Time Line
                    Me.SetupStep1b2(udtEHSAccount)

                Case Step1b3
                    '==================================================================== Code for SmartID ============================================================================
                    'Setup Smart IC Compare Value
                    Me.SetupStep1b3(Me._udtEHSAccount, True)
                    '==================================================================================================================================================================

                Case ActiveViewIndex.Step1c
                    'Hight Light Time Line
                    Me.SetupStep1c(udtEHSAccount, False)

                Case TextSelectPractice
                    CType(Me.Master.FindControl(ClaimVoucherMaster.ControlName.LblClaimVoucherStep), Label).Text = String.Empty
                    SetupSelectPractice()
                    Me.SetupSetp1b2DocumentTypeContent(udtEHSAccount, True)

                Case TextInputTips
                    CType(Me.Master.FindControl(ClaimVoucherMaster.ControlName.LblClaimVoucherStep), Label).Text = String.Empty
                    Me.ucInputTipsControl.LoadTip()
                    Me.SetupSetp1b2DocumentTypeContent(udtEHSAccount, True)
                    Me.SetupSetp1b3DocumentTypeContent(udtEHSAccount, True, MyBase.SessionHandler.SmartIDContentGetFormSession(FunctCode))

                Case TextConfirmCancelBox
                    Me.SetupSetp1b2DocumentTypeContent(udtEHSAccount, True)
                    Me.SetupSetp1b3DocumentTypeContent(udtEHSAccount, True, MyBase.SessionHandler.SmartIDContentGetFormSession(FunctCode))

                Case TextConfirmDeclare
                    SetupStepConfirmDeclaration()
            End Select

        End Sub

        Private Sub EnableConfirmButton(ByVal enable As Boolean, ByVal confirmButton As Button)
            'If enable Then
            '    confirmButton.Enabled = enable
            '    confirmButton.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmBtn")
            'Else
            '    confirmButton.Enabled = enable
            '    confirmButton.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmDisableBtn")
            'End If
            confirmButton.Enabled = enable

        End Sub

        Private Sub mvAccountCreation_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mvAccountCreation.ActiveViewChanged
            If MyBase.IsPageRefreshed Then
                Return
            End If

            Dim strHightLight As String = "highlightTimelineLast"
            Dim strUnhightLight As String = "unhighlightTimelineLast"
            Me._udtEHSAccount = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
            CType(Me.Master.FindControl(ClaimVoucherMaster.ControlName.LblClaimVoucherStep), Label).Text = Me.GetStepText()

            ' Construct the Menu
            Me.BuildMenuItem()

            Select Case mvAccountCreation.ActiveViewIndex
                Case ActiveViewIndex.Step1a1 'Get consent (Show Limited information Only)
                    Me.SetupStep1a1(Me._udtEHSAccount)

                Case ActiveViewIndex.Step1a2 'Get Exisiting Account Consent (Show all information Only)
                    'Hight Light Time Line
                    Me.SetupStep1a2(Me._udtEHSAccount)

                Case ActiveViewIndex.Step1b1 'Enter account detail
                    'Hight Light Time Line
                    Me.SetupStep1b1(Me._udtEHSAccount, True)
                    Me.Step1b1ResetErrorImage()

                Case ActiveViewIndex.Step1b2 'confirm Detail
                    'Hight Light Time Line
                    Me.SetupStep1b2(Me._udtEHSAccount)
                    'Me.chkStep1b2Declare.Checked = False
                    'Me.EnableConfirmButton(False, Me.btnStep1b2Confirm)

                Case Step1b3    ' Setup Smart IC Compare Value
                    '==================================================================== Code for SmartID ============================================================================
                    Me.SetupStep1b3(Me._udtEHSAccount, True)
                    '==================================================================================================================================================================

                Case ActiveViewIndex.Step1c 'complete account creation
                    'Hight Light Time Line
                    Me.SetupStep1c(Me._udtEHSAccount, True)

                Case TextSelectPractice
                    ' Hide Message Box
                    Me.HideMessageBox()

                    SetupSelectPractice()
                    Me.SetupSetp1b2DocumentTypeContent(Me._udtEHSAccount, True)

                Case TextInputTips
                    ' Hide Message Box
                    Me.HideMessageBox()

                    Me.ucInputTipsControl.LoadTip()
                    Me.SetupSetp1b2DocumentTypeContent(Me._udtEHSAccount, True)
                    Me.SetupSetp1b3DocumentTypeContent(Me._udtEHSAccount, True, Me.SessionHandler.SmartIDContentGetFormSession(FunctCode))

                Case TextConfirmDeclare
                    SetupStepConfirmDeclaration()

                Case TextConfirmCancelBox
                    Me.SetupSetp1b2DocumentTypeContent(Me._udtEHSAccount, True)
                    Me.SetupSetp1b3DocumentTypeContent(Me._udtEHSAccount, True, MyBase.SessionHandler.SmartIDContentGetFormSession(FunctCode))

            End Select

        End Sub

#Region "Step 1a1 Get Account Creation Consent "

        Private Sub btnstep1a1CreateAccount_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnstep1a1CreateAccount.Click
            If MyBase.IsPageRefreshed Then
                Return
            End If

            Dim udtDataEntry As DataEntryUserModel = Nothing
            Dim udtEHSAccount As EHSAccountModel = Nothing
            Dim strDocCode As String = String.Empty
            Dim udtScheme As Scheme.SchemeClaimModel
            Dim udtSmartIDContent As BLL.SmartIDContentModel = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode)

            Me._udtSessionHandler.ExceedDocTypeLimitRemoveFromSession()
            Me._udtSessionHandler.CurrentUserGetFromSession(Me._udtSP, udtDataEntry)

            EHSAccountCreationBase.AuditLogStep1a1CreateAccountStart(_udtAuditLogEntry)

            Me._udtEHSAccount = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)

            If Not udtSmartIDContent Is Nothing Then

                '==================================================================== Code for SmartID ============================================================================
                Select Case udtSmartIDContent.SmartIDReadStatus

                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    Case SmartIDHandler.SmartIDResultStatus.DocTypeNotExist, _
                         SmartIDHandler.SmartIDResultStatus.EHSAccountNotfound, _
                         SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_DiffDOIDOB, _
                         SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_NotCreateBySmartID,
                         SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB_WithoutGender_SameName
                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                        ' HKBC holder already in system
                        ' - Alert user 
                        ' Account Not Found | Temp Account Exists with Diff DOI &/ DOB | Temp Account Exists w/ Diff Detail
                        ' - Create Account                        
                        Me.mvAccountCreation.ActiveViewIndex = Step1b3
                        EHSAccountCreationBase.AuditLogStep1a1CreateAccountEndBySmartID(_udtAuditLogEntry, udtSmartIDContent, "Go to Step1b3 (Confirm Account)")

                    Case Else
                        EHSAccountCreationBase.AuditLogStep1a1CreateAccountEndBySmartID(_udtAuditLogEntry, udtSmartIDContent, "Action is not Expected in this page.")
                        'No case Unit Now
                        Me.BackToClaim(False)
                End Select
                '==================================================================================================================================================================

            Else
                '----------------------------------------------------------------
                '   Normal Case
                '----------------------------------------------------------------
                '1) DB have the record which is same document Identity No but different DOB 
                '2) For CIVSS Only, crosse search for HKID -> HKBC OR HKBC -> HKID
                If Me._udtSessionHandler.NotMatchAccountExistGetFromSession() OrElse _
                     Not Me._udtEHSAccount.SearchDocCode.Trim().Equals(Me._udtEHSAccount.EHSPersonalInformationList(0).DocCode.Trim()) Then
                    EHSAccountCreationBase.AuditLogStep1a1CreateAccountEnd(_udtAuditLogEntry, "Doc No. found + [DOB not match OR Doc Code not match]")

                    udtScheme = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
                    If Me._udtEHSAccount.SearchDocCode = DocType.DocTypeModel.DocTypeCode.EC Then
                        With Me._udtEHSAccount.EHSPersonalInformationList(0)
                            If .ECAge.HasValue Then
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

                bUnchangeInputUI = True
                Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1b1
            End If



        End Sub

        Private Sub btnstep1a1Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnstep1a1Cancel.Click
            If MyBase.IsPageRefreshed Then
                Return
            End If

            Me.BackToClaim(False)
        End Sub

        Protected Overrides Sub SetupStep1a1(ByVal udtEHSAccount As EHSAccountModel)
            Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)
            Me._udtFormatter = New Formatter()
            Dim udtDocTypeBLL As DocType.DocTypeBLL = New DocType.DocTypeBLL()
            Dim udtDocTypeModelList As DocType.DocTypeModelCollection
            Dim strDocumentTypeFullName As String
            Dim strDocIdentityDesc As String
            Dim strDocCode As String = Me._udtEHSAccount.SearchDocCode.Trim()

            Dim udtSmartIDContent As BLL.SmartIDContentModel = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode)
            Dim enumSmartIDStatus As SmartIDHandler.SmartIDResultStatus = SmartIDHandler.SmartIDResultStatus.Empty

            ''Control init
            'Me.lblstep1a1HeaderText.Text = Me.GetGlobalResourceObject("Text", "VRACreationTempAcctCreate")
            'Me.btnstep1a1Confirm.Visible = False
            Me.lblstep1a1DocumentType.BackColor = Drawing.Color.Transparent

            If Not udtEHSAccount Is Nothing Then
                'Get Documnet type full name
                udtDocTypeModelList = udtDocTypeBLL.getAllDocType()

                ' CRE20-0022 (Immu record) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                strDocumentTypeFullName = udtDocTypeModelList.Filter(strDocCode).DocName(Me._udtSessionHandler.Language)
                strDocIdentityDesc = udtDocTypeModelList.Filter(strDocCode).DocIdentityDesc(Me._udtSessionHandler.Language)
                ' CRE20-0022 (Immu record) [End][Chris YIM]

                '==================================================================== Code for SmartID ============================================================================
                If Not udtSmartIDContent Is Nothing AndAlso udtSmartIDContent.IsReadSmartID Then
                    Me.lblstep1a1DOBText.Visible = False
                    Me.lblstep1a1DOB.Visible = False
                    enumSmartIDStatus = udtSmartIDContent.SmartIDReadStatus

                Else
                    Me.lblstep1a1DOBText.Visible = True
                    Me.lblstep1a1DOBText.Text = Me.GetGlobalResourceObject("Text", "DOB")

                    Me.lblstep1a1DOB.Visible = True
                    Me.lblstep1a1DOB.Text = Me._udtFormatter.formatDOB(udtEHSAccountPersonalInfo.DOB, udtEHSAccountPersonalInfo.ExactDOB, Me._udtSessionHandler.Language, udtEHSAccountPersonalInfo.ECAge, udtEHSAccountPersonalInfo.ECDateOfRegistration)

                End If
                '===================================================================================================================================================================

                If Me._udtEHSAccount.IsNew() Then
                    If Not Me._udtSessionHandler.NotMatchAccountExistGetFromSession() Then
                        'No reocrd found -> Create New Account
                        Me.lblstep1a1DisclaimerNotice.Text = Me.GetGlobalResourceObject("Text", "CreateTempAccNotExistDisclaim")
                    Else
                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                        ' ----------------------------------------------------------------------------------------
                        Select Case enumSmartIDStatus
                            Case SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_NotCreateBySmartID, _
                                enumSmartIDStatus = SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_DiffDOIDOB, _
                                enumSmartIDStatus = SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB_WithoutGender_SameName
                                ' Temp Account not created by Smart ID
                                ' Temp Account Created By Smart ID w/ Diff DOI &/ DOB
                                ' Temp Account Created By Smart ID w/ Same DOI &/ DOB & Name but Diff Gender
                                Me.lblstep1a1DisclaimerNotice.Text = Me.GetGlobalResourceObject("Text", "TempACWithDiffInfo")
                            Case Else
                                'Record found but DOB/Detail not Match -> Create New Temp account
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
                            ' Temp Account not created by Smart ID
                            ' Temp Account Created By Smart ID w/ Diff DOI &/ DOB
                            ' Temp Account Created By Smart ID w/ Same DOI &/ DOB & Name but Diff Gender
                            Me.lblstep1a1DisclaimerNotice.Text = Me.GetGlobalResourceObject("Text", "TempACWithDiffInfo")

                        Case Else
                            'Search Code = Personal Information DocCode -> go to Step 1b1 (get Consent with all information), Coding in page load
                            If Not Me._udtEHSAccount.EHSPersonalInformationList(0).DocCode.Trim().Equals(strDocCode) Then
                                If Me._udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                                    Me.lblstep1a1DisclaimerNotice.Text = Me.GetGlobalResourceObject("Text", "ValidatedAccDiffDocTypeDisclaim").ToString()
                                ElseIf Me._udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.TemporaryAccount OrElse Me._udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.SpecialAccount Then
                                    Me.lblstep1a1DisclaimerNotice.Text = Me.GetGlobalResourceObject("Text", "CreateTempAccDiffDocTypeDisclaim").ToString()
                                End If
                            End If
                    End Select
                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
                End If
                '----------------------------------------------------------------------------------------------------------------------------------

                Me.lblstep1a1DocumentType.Text = strDocumentTypeFullName
                Me.lblstep1a1HKIDText.Text = strDocIdentityDesc

                Select Case strDocCode
                    Case DocType.DocTypeModel.DocTypeCode.HKIC, DocType.DocTypeModel.DocTypeCode.EC
                        'Me.lblstep1a1HKIDText.Text = Me.GetGlobalResourceObject("Text", "HKID")
                        Me.lblstep1a1HKID.Text = Me._udtFormatter.formatHKID(udtEHSAccountPersonalInfo.IdentityNum, False)

                    Case DocType.DocTypeModel.DocTypeCode.HKBC, DocType.DocTypeModel.DocTypeCode.CCIC, DocType.DocTypeModel.DocTypeCode.ROP140 ' CRE20-0022 (Immu record)[Martin]
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
                        'Me.lblstep1a1HKIDText.Text = Me.GetGlobalResourceObject("Text", "NoOfEntry")
                        Me.lblstep1a1HKID.Text = Me._udtFormatter.FormatDocIdentityNoForDisplay(udtEHSAccountPersonalInfo.DocCode, udtEHSAccountPersonalInfo.IdentityNum, False, udtEHSAccountPersonalInfo.AdoptionPrefixNum)

                    Case DocTypeModel.DocTypeCode.DI, DocTypeModel.DocTypeCode.PASS ' CRE20-0022 (Immu record)[Martin]
                        'Me.lblstep1a1HKIDText.Text = Me.GetGlobalResourceObject("Text", "IdentityDocNo")
                        Me.lblstep1a1HKID.Text = Me._udtFormatter.FormatDocIdentityNoForDisplay(udtEHSAccountPersonalInfo.DocCode, udtEHSAccountPersonalInfo.IdentityNum, False)
                        'Me.lblstep1a1HKID.Text = udtEHSAccountPersonalInfo.IdentityNum
                End Select

                Select Case udtEHSAccountPersonalInfo.DocCode
                    Case DocType.DocTypeModel.DocTypeCode.HKIC, _
                         DocType.DocTypeModel.DocTypeCode.HKBC, _
                         DocType.DocTypeModel.DocTypeCode.EC, _
                         DocType.DocTypeModel.DocTypeCode.CCIC, _
                         DocType.DocTypeModel.DocTypeCode.ROP140 ' CRE20-0022 (Immu record)[Martin]
                        Me.lblstep1a1HKID.Text = Me._udtFormatter.formatHKID(udtEHSAccountPersonalInfo.IdentityNum, False)
                    Case DocType.DocTypeModel.DocTypeCode.ADOPC
                        Me.lblstep1a1HKID.Text = udtEHSAccountPersonalInfo.AdoptionField
                    Case Else
                        Me.lblstep1a1HKID.Text = Me._udtFormatter.FormatDocIdentityNoForDisplay(udtEHSAccountPersonalInfo.DocCode, udtEHSAccountPersonalInfo.IdentityNum, False)
                        'Me.lblstep1a1HKID.Text = udtEHSAccountPersonalInfo.IdentityNum
                End Select

            Else
                Me.lblstep1a1DocumentType.Text = String.Empty
                Me.lblstep1a1HKID.Text = String.Empty
                Me.lblstep1a1DOB.Text = String.Empty
            End If
            'Me.Step1a1RenderLanguage(udtEHSAccount)
        End Sub

#End Region

#Region "Step 1a2 Get Get Existing Account Consent "

        Private Sub btnStep1a2Back_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStep1a2Back.Click
            If MyBase.IsPageRefreshed Then
                Return
            End If

            Me.BackToClaim(False)
        End Sub

        Private Sub btnStep1a2Confirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStep1a2Confirm.Click
            If MyBase.IsPageRefreshed Then
                Return
            End If

            Me._udtSessionHandler.AccountCreationProceedClaimSaveToSession(True)

            EHSAccountCreationBase.AuditLogStep1a2ConfirmAccount(_udtAuditLogEntry, FunctCode)

            Me.BackToClaim(True)
        End Sub

        Private Sub btnStep1a2Modify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStep1a2Modify.Click
            If MyBase.IsPageRefreshed Then
                Return
            End If

            EHSAccountCreationBase.AuditLogStep1a2ModifyAccount(_udtAuditLogEntry, FunctCode)

            Me.mvAccountCreation.ActiveViewIndex = TextConfirmModifyBox
        End Sub

        Private Sub btnConfirmModifyBoxYes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirmModifyBoxYes.Click
            If MyBase.IsPageRefreshed Then
                Return
            End If

            ' Confirm Modify EHSAccount
            EHSAccountCreationBase.AuditLogStep1a2ConfirmModifyAccount(_udtAuditLogEntry, FunctCode)

            Dim udtSmartIDContent As BLL.SmartIDContentModel = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode)
            Dim udtScheme As Scheme.SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

            If Not udtSmartIDContent Is Nothing Then

                '==================================================================== Code for SmartID ============================================================================
                Me.mvAccountCreation.ActiveViewIndex = Step1b3
                EHSAccountCreationBase.AuditLogStep1a2ConfirmModifyAccountBySmartID(_udtAuditLogEntry, FunctCode, udtSmartIDContent)
                '==================================================================================================================================================================

            Else
                Me._udtEHSAccount = Me._udtEHSAccount.CloneData()
                Me._udtEHSAccount.SchemeCode = udtScheme.SchemeCode

                bUnchangeInputUI = False
                Me._udtSessionHandler.EHSAccountSaveToSession(Me._udtEHSAccount, FunctCode)
                Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1b1

            End If
        End Sub

        Private Sub btnConfirmModifyBoxNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirmModifyBoxNo.Click
            If MyBase.IsPageRefreshed Then
                Return
            End If

            Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1a2
        End Sub

        Protected Overrides Sub SetupStep1a2(ByVal udtEHSAccount As EHSAccountModel)
            Dim strDocCode As String = udtEHSAccount.EHSPersonalInformationList(0).DocCode.Trim()
            Dim udtSmartIDContent As BLL.SmartIDContentModel = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode)
            Me.lblStep1a2TempAccDisclaimerText.Text = Me.GetGlobalResourceObject("Text", "ConfirmTempAccDisclaim")

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

            Me.udcStep1a2ReadOnlyDocumnetType.TextOnlyVersion = True
            Me.udcStep1a2ReadOnlyDocumnetType.DocumentType = strDocCode
            Me.udcStep1a2ReadOnlyDocumnetType.EHSAccount = udtEHSAccount
            Me.udcStep1a2ReadOnlyDocumnetType.Vertical = True
            Me.udcStep1a2ReadOnlyDocumnetType.MaskIdentityNo = True
            Me.udcStep1a2ReadOnlyDocumnetType.ShowAccountRefNo = True
            Me.udcStep1a2ReadOnlyDocumnetType.ShowTempAccountNotice = True
            Me.udcStep1a2ReadOnlyDocumnetType.ShowAccountCreationDate = False
            Me.udcStep1a2ReadOnlyDocumnetType.TextOnlyVersion = True

            If Not udtSmartIDContent Is Nothing _
                    AndAlso udtSmartIDContent.IsReadSmartID _
                    AndAlso SmartIDShowRealID() Then
                Me.udcStep1a2ReadOnlyDocumnetType.MaskIdentityNo = False
            End If

            Me.udcStep1a2ReadOnlyDocumnetType.Built()
        End Sub

#End Region

#Region "Step 1b1 Enter Detail"

        Private Sub btnStep1b1Next_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStep1b1Next.Click
            If MyBase.IsPageRefreshed Then
                Return
            End If

            Dim practiceDisplay As PracticeDisplayModel = Me._udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)
            Dim blnProceed As Boolean = True
            Me._udtEHSAccount = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
            Me._udtEHSAccount.CreateSPPracticeDisplaySeq = practiceDisplay.PracticeID

            'Dim udcInputHKIC As ucInputHKID = Me.udcStep1b1InputDocumentType.GetHKICControl()
            EHSAccountCreationBase.AuditLogStep1b1Start(_udtAuditLogEntry)

            Select Case Me._udtEHSAccount.SearchDocCode
                Case DocTypeModel.DocTypeCode.HKIC
                    'If udcInputHKIC.CCCodeIsEmpty() Then
                    '    udcInputHKIC.SetCName(String.Empty)
                    '    Me.udcChooseCCCode.Clean()
                    'Else
                    '    'Check for select chinese name/CCCode 
                    '    'if cccode is changed (different between session value and input box), or incorrect CCCode
                    '    If Me.NeedPopupChineseNameDialog() Then
                    '        Me.udcStep1b1InputDocumentType_SelectChineseName_HKIC(udcInputHKIC, Nothing, Nothing)
                    '        blnProceed = False
                    '    End If
                    'End If

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
            End Select

            If blnProceed Then
                Me._udtSessionHandler.EHSAccountSaveToSession(Me._udtEHSAccount, FunctCode)
                Me.udcStep1b1InputDocumentType.ActiveViewChanged = True

                EHSAccountCreationBase.AuditLogStep1b1Complete(_udtAuditLogEntry, Me._udtEHSAccount)
                Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1b2
            End If
            Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail, _udtAuditLogEntry, Common.Component.LogID.LOG00033, "Enter Detail Failed")
        End Sub

        Private Sub btnStep1b1Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStep1b1Cancel.Click
            If MyBase.IsPageRefreshed Then
                Return
            End If

            Me.mvAccountCreation.ActiveViewIndex = TextConfirmCancelBox
            Me.udcMsgBoxErr.Clear()
        End Sub

        Private Sub btnStep1b1ChangePractice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStep1b1ChangePractice.Click
            If MyBase.IsPageRefreshed Then
                Return
            End If

            Me.udcStep1b1InputDocumentType.ActiveViewChanged = False
            Me.mvAccountCreation.ActiveViewIndex = TextSelectPractice
        End Sub

        Protected Overrides Sub SetupStep1b1(ByVal udtEHSAccount As EHSAccountModel, ByVal activeViewChanged As Boolean)
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

            If Me._udtSessionHandler.Language() = CultureLanguage.TradChinese Then
                Me.lblStep1b1CurrentPractice.Text = String.Format("{0} ({1})", practiceDisplay.PracticeNameChi, practiceDisplay.PracticeID)
                Me.lblStep1b1CurrentPractice.CssClass = "tableTextChi"
            Else
                Me.lblStep1b1CurrentPractice.Text = String.Format("{0} ({1})", practiceDisplay.PracticeName, practiceDisplay.PracticeID)
                Me.lblStep1b1CurrentPractice.CssClass = "tableText"
            End If

            'Invisibility Button
            Me.ShowChangePracticeButton(Me.btnStep1b1ChangePractice)

            Dim udtDocTypeBLL As New DocTypeBLL()

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
            End Select

            Me.SetupSetp1b2DocumentTypeContent(udtEHSAccount, activeViewChanged)
            ''Format: Practice Name (Practice display Seq/Practice ID) [Practice Address]
            'If activeViewChanged Then

            '    'Set up practice selection popup Box
            '    udtPracticeDisplays = Me._udtSessionHandler.PracticeDisplayListGetFromSession()
            '    'Me.udcPracticeRadioButtonGroup.BuildRadioButtonGroup(udtPracticeDisplays, Me._udtSP.PracticeList, Me._udtSessionHandler.Language, PracticeRadioButtonGroup.DisplayMode.Address)

            '    Me.udcStep1b1InputDocumentType.EHSAccount = udtEHSAccount
            '    Me.udcStep1b1InputDocumentType.DocType = udtEHSAccount.SearchDocCode
            '    Me.udcStep1b1InputDocumentType.Mode = ucInputDocTypeBase.BuildMode.Creation
            '    Me.udcStep1b1InputDocumentType.FillValue = True
            '    Me.udcStep1b1InputDocumentType.SchemeClaim = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
            '    Me.udcStep1b1InputDocumentType.TextOnlyVersion = True
            '    'Me.udcStep1b1InputDocumentType.ActiveViewChanged = createPopupPractice
            '    Me.udcStep1b1InputDocumentType.ActiveViewChanged = activeViewChanged
            '    Me.udcStep1b1InputDocumentType.Built()

            'End If
        End Sub

        Private Sub SetupSetp1b2DocumentTypeContent(ByVal udtEHSAccount As EHSAccountModel, ByVal activeViewChanged As Boolean)
            If activeViewChanged Then

                Me.udcStep1b1InputDocumentType.EHSAccount = udtEHSAccount
                Me.udcStep1b1InputDocumentType.DocType = udtEHSAccount.SearchDocCode
                Me.udcStep1b1InputDocumentType.Mode = ucInputDocTypeBase.BuildMode.Creation
                Me.udcStep1b1InputDocumentType.FillValue = True
                Me.udcStep1b1InputDocumentType.SchemeClaim = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
                Me.udcStep1b1InputDocumentType.TextOnlyVersion = True
                Me.udcStep1b1InputDocumentType.AuditLogEntry = _udtAuditLogEntry

                'Me.udcStep1b1InputDocumentType.ActiveViewChanged = createPopupPractice
                'Me.udcStep1b1InputDocumentType.ActiveViewChanged = activeViewChanged
                Me.udcStep1b1InputDocumentType.Built()

            End If
        End Sub

        Private Sub btnConfirmCancelBoxCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirmCancelBoxCancel.Click
            If MyBase.IsPageRefreshed Then
                Return
            End If

            If Not Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode) Is Nothing Then
                '==================================================================== Code for SmartID ============================================================================
                Me.mvAccountCreation.ActiveViewIndex = Step1b3
                '==================================================================================================================================================================

            Else
                bUnchangeInputUI = True
                Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1b1

            End If
        End Sub

        Private Sub btnConfirmCancelBoxConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirmCancelBoxConfirm.Click
            If MyBase.IsPageRefreshed Then
                Return
            End If

            Me._udtSessionHandler.EHSClaimSessionRemove(FunctCode)

            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

            RedirectHandler.ToURL(EHSClaim)

            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End
        End Sub

        Private Sub lnkDocIDTips_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDocIDTips.Click
            If MyBase.IsPageRefreshed Then
                Return
            End If

            Me._udtEHSAccount = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
            Select Case Me._udtEHSAccount.SearchDocCode
                Case DocTypeModel.DocTypeCode.HKIC
                    Me.ucInputTipsControl.LoadTip(ucInputTips.InputTipsType.InputHKIC)
                Case DocTypeModel.DocTypeCode.EC
                    Me.ucInputTipsControl.LoadTip(ucInputTips.InputTipsType.InputEC)
                Case DocTypeModel.DocTypeCode.HKBC
                    Me.ucInputTipsControl.LoadTip(ucInputTips.InputTipsType.InputHKBC)
                Case DocTypeModel.DocTypeCode.DI
                    Me.ucInputTipsControl.LoadTip(ucInputTips.InputTipsType.InputDI)
                Case DocTypeModel.DocTypeCode.ID235B
                    Me.ucInputTipsControl.LoadTip(ucInputTips.InputTipsType.InputID235B)
                Case DocTypeModel.DocTypeCode.REPMT
                    Me.ucInputTipsControl.LoadTip(ucInputTips.InputTipsType.InputReentry)
                Case DocTypeModel.DocTypeCode.VISA
                    Me.ucInputTipsControl.LoadTip(ucInputTips.InputTipsType.InputVisa)
                Case DocTypeModel.DocTypeCode.ADOPC
                    Me.ucInputTipsControl.LoadTip(ucInputTips.InputTipsType.InputAdoption)
            End Select

            Me.udcStep1b1InputDocumentType.ActiveViewChanged = False
            Me.udcStep1b3InputDocumentType.ActiveViewChanged = False
            Me.mvAccountCreation.ActiveViewIndex = TextInputTips

        End Sub

        '-----------------------------------------------------------------------------------------------------------------------------
        'Reset DocType Error Image
        '-----------------------------------------------------------------------------------------------------------------------------
        Private Sub Step1b1ResetErrorImage()
            Me._udtEHSAccount = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
            Dim ucInputDocType As ucInputDocTypeBase = Nothing

            Select Case Me._udtEHSAccount.SearchDocCode
                Case DocTypeModel.DocTypeCode.HKIC
                    ucInputDocType = Me.udcStep1b1InputDocumentType.GetHKICTextControl()

                Case DocTypeModel.DocTypeCode.EC
                    ucInputDocType = Me.udcStep1b1InputDocumentType.GetECTextControl()

                Case DocTypeModel.DocTypeCode.HKBC
                    ucInputDocType = Me.udcStep1b1InputDocumentType.GetHKBCTextControl()

                Case DocTypeModel.DocTypeCode.DI
                    ucInputDocType = Me.udcStep1b1InputDocumentType.GetDITextControl()

                Case DocTypeModel.DocTypeCode.ID235B
                    ucInputDocType = Me.udcStep1b1InputDocumentType.GetID235BTextControl()

                Case DocTypeModel.DocTypeCode.REPMT
                    ucInputDocType = Me.udcStep1b1InputDocumentType.GetREPMTTextControl()

                Case DocTypeModel.DocTypeCode.VISA
                    ucInputDocType = Me.udcStep1b1InputDocumentType.GetVISATextControl()

                Case DocTypeModel.DocTypeCode.ADOPC
                    ucInputDocType = Me.udcStep1b1InputDocumentType.GetADOPCTextControl()
            End Select

            If Not ucInputDocType Is Nothing Then
                ucInputDocType.SetErrorImage(ucInputDocTypeBase.BuildMode.Creation, False)
            End If
        End Sub

#End Region

#Region "Step lbl Enter Account Detial Valiation"

        Protected Overrides Function Step1b1HKICValdiation(ByRef udtEHSAccount As EHSAccountModel) As Boolean
            Dim isValid As Boolean = True
            Dim strHKIDIssueDate As String = Nothing
            Dim dtHKIDIssueDate As DateTime
            Dim udcInputHKIC As UIControl.DocTypeText.ucInputHKID = Me.udcStep1b1InputDocumentType.GetHKICTextControl()
            udcInputHKIC.SetErrorImage(ucInputDocTypeBase.BuildMode.Creation, False)
            udcInputHKIC.SetProperty(ucInputDocTypeBase.BuildMode.Creation)
            Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode)

            Me._systemMessage = Me._validator.chkEngName(udcInputHKIC.ENameSurName, udcInputHKIC.ENameFirstName)
            If Not Me._systemMessage Is Nothing Then
                isValid = False
                udcInputHKIC.SetENameError(True)
                Me.udcMsgBoxErr.AddMessage(_systemMessage)
            End If

            'Me._systemMessage = Me._validator.chkCCCode_UsingDDL(String.Format("{0}{1}", udcInputHKIC.CCCode1, Me.udcChooseCCCode.SelectedCCCodeTail1), _
            '    String.Format("{0}{1}", udcInputHKIC.CCCode2, Me.udcChooseCCCode.SelectedCCCodeTail2), _
            '    String.Format("{0}{1}", udcInputHKIC.CCCode3, Me.udcChooseCCCode.SelectedCCCodeTail3), _
            '    String.Format("{0}{1}", udcInputHKIC.CCCode4, Me.udcChooseCCCode.SelectedCCCodeTail4), _
            '    String.Format("{0}{1}", udcInputHKIC.CCCode5, Me.udcChooseCCCode.SelectedCCCodeTail5), _
            '    String.Format("{0}{1}", udcInputHKIC.CCCode6, Me.udcChooseCCCode.SelectedCCCodeTail6))
            'If Not Me._systemMessage Is Nothing Then
            '    isValid = False
            '    udcInputHKIC.SetCCCodeError(True)
            '    Me.udcMsgBoxErr.AddMessage(_systemMessage)
            'End If

            Me._systemMessage = Me._validator.chkGender(udcInputHKIC.Gender)
            If Not Me._systemMessage Is Nothing Then
                isValid = False
                udcInputHKIC.SetGenderError(True)
                Me.udcMsgBoxErr.AddMessage(_systemMessage)
            End If

            strHKIDIssueDate = Me._udtFormatter.formatHKIDIssueDateBeforeValidate(udcInputHKIC.HKIDIssuseDate)
            'Me._systemMessage = Me._validator.chkHKIDIssueDate(strHKIDIssueDate)
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

                'udtEHSAccountPersonalInfo.CCCode1 = String.Format("{0}{1}", udcInputHKIC.CCCode1, Me.udcChooseCCCode.SelectedCCCodeTail1)
                'udtEHSAccountPersonalInfo.CCCode2 = String.Format("{0}{1}", udcInputHKIC.CCCode2, Me.udcChooseCCCode.SelectedCCCodeTail2)
                'udtEHSAccountPersonalInfo.CCCode3 = String.Format("{0}{1}", udcInputHKIC.CCCode3, Me.udcChooseCCCode.SelectedCCCodeTail3)
                'udtEHSAccountPersonalInfo.CCCode4 = String.Format("{0}{1}", udcInputHKIC.CCCode4, Me.udcChooseCCCode.SelectedCCCodeTail4)
                'udtEHSAccountPersonalInfo.CCCode5 = String.Format("{0}{1}", udcInputHKIC.CCCode5, Me.udcChooseCCCode.SelectedCCCodeTail5)
                'udtEHSAccountPersonalInfo.CCCode6 = String.Format("{0}{1}", udcInputHKIC.CCCode6, Me.udcChooseCCCode.SelectedCCCodeTail6)

                ''Retervie Chinese Name from Choose
                'udcInputHKIC.CCCode1 = udtEHSAccountPersonalInfo.CCCode1
                'udcInputHKIC.CCCode2 = udtEHSAccountPersonalInfo.CCCode2
                'udcInputHKIC.CCCode3 = udtEHSAccountPersonalInfo.CCCode3
                'udcInputHKIC.CCCode4 = udtEHSAccountPersonalInfo.CCCode4
                'udcInputHKIC.CCCode5 = udtEHSAccountPersonalInfo.CCCode5
                'udcInputHKIC.CCCode6 = udtEHSAccountPersonalInfo.CCCode6
                'udcInputHKIC.SetCName()
                'udtEHSAccountPersonalInfo.CName = udcInputHKIC.CName
                udtEHSAccountPersonalInfo.CName = String.Empty

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

        Protected Overrides Function Step1b1ECValdiation(ByRef udtEHSAccount As EHSAccountModel) As Boolean
            Dim udcInputEC As UIControl.DocTypeText.ucInputEC = Me.udcStep1b1InputDocumentType.GetECTextControl()
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
                udcInputEC.SetSurnameError(True)
                udcInputEC.SetGivenNameError(True)
                Me.udcMsgBoxErr.AddMessage(_systemMessage)
            End If

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
                udtEHSAccountPersonalInfo.CName = String.Empty
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

        Protected Overrides Function Step1b1HKBCValidation(ByRef udtEHSAccount As EHSAccountModel) As Boolean
            Dim isValid As Boolean = True

            Dim udcInputHKBC As UIControl.DocTypeText.ucInputHKBC = Me.udcStep1b1InputDocumentType.GetHKBCTextControl()
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
                ' CRE16-012 Removal of DOB InWord [Start][Winnie]
                udtEHSAccountPersonalInfo.OtherInfo = String.Empty 'udcInputHKBC.DOBInWord
                ' CRE16-012 Removal of DOB InWord [End][Winnie]
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

        Protected Overrides Function Step1b1DIValidation(ByRef udtEHSAccount As EHSAccountModel) As Boolean
            Dim isValid As Boolean = True
            Dim udcInputDI As UIControl.DocTypeText.ucInputDI = Me.udcStep1b1InputDocumentType.GetDITextControl()
            Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)
            Dim dtDateOfIssue As DateTime
            Dim strDOI As String = String.Empty
            udcInputDI.SetErrorImage(ucInputDocTypeBase.BuildMode.Creation, False)
            udcInputDI.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

            Me._systemMessage = Me._validator.chkEngName(udcInputDI.ENameSurName, udcInputDI.ENameFirstName)
            If Not Me._systemMessage Is Nothing Then
                isValid = False
                udcInputDI.SetSurnameError(True)
                udcInputDI.SetGivenNameError(True)
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

        Protected Overrides Function Step1b1ID235BValidation(ByRef udtEHSAccount As EHSAccountModel) As Boolean
            Dim isValid As Boolean = True
            Dim udcInputID235B As UIControl.DocTypeText.ucInputID235B = Me.udcStep1b1InputDocumentType.GetID235BTextControl()
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

        Protected Overrides Function Step1b1RepmtValidation(ByRef udtEHSAccount As EHSAccountModel) As Boolean
            Dim isValid As Boolean = True
            Dim udcInputReentryPermit As UIControl.DocTypeText.ucInputReentryPermit = Me.udcStep1b1InputDocumentType.GetREPMTTextControl()
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

                Me._systemMessage = Me.EHSAccountBasicValidation(DocTypeModel.DocTypeCode.ID235B, udtEHSAccount)
                If Not Me._systemMessage Is Nothing Then
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
                End If
            End If

            Return isValid
        End Function

        Protected Overrides Function Step1b1VISAValidation(ByRef udtEHSAccount As EHSAccountModel) As Boolean
            Dim isValid As Boolean = True
            Dim udcInputVISA As UIControl.DocTypeText.ucInputVISA = Me.udcStep1b1InputDocumentType.GetVISATextControl()
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

        Protected Overrides Function Step1b1AdoptionValidation(ByRef udtEHSAccount As EHSAccountModel) As Boolean
            Dim isValid As Boolean = True
            Dim udcInputAdoption As UIControl.DocTypeText.ucInputAdoption = Me.udcStep1b1InputDocumentType.GetADOPCTextControl()
            udcInputAdoption.SetErrorImage(ucInputDocTypeBase.BuildMode.Creation, False)
            udcInputAdoption.SetProperty(ucInputDocTypeBase.BuildMode.Creation)

            Me._systemMessage = Me._validator.chkEngName(udcInputAdoption.ENameSurName, udcInputAdoption.ENameFirstName)
            If Not Me._systemMessage Is Nothing Then
                isValid = False
                udcInputAdoption.SetGivenNameError(True)
                udcInputAdoption.SetSurnameError(True)
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
                ' CRE16-012 Removal of DOB InWord [Start][Winnie]
                udtEHSAccountPersonalInfo.OtherInfo = String.Empty 'udcInputAdoption.DOBInWord
                ' CRE16-012 Removal of DOB InWord [End][Winnie]
                udtEHSAccountPersonalInfo.ExactDOB = udcInputAdoption.IsExactDOB
                udtEHSAccountPersonalInfo.SetDOBTypeSelected(True)

                Me._systemMessage = Me.EHSAccountBasicValidation(DocType.DocTypeModel.DocTypeCode.ADOPC, udtEHSAccount)
                If Not Me._systemMessage Is Nothing Then
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(Me._systemMessage)
                End If
            End If

            Return isValid
        End Function

        Private Function EHSAccountBasicValidation(ByVal strDocCode As String, ByVal udtEHSAccount As EHSAccountModel) As SystemMessage
            Return Me._udtClaimRulesBLL.CheckCreateEHSAccount(udtEHSAccount.SchemeCode, strDocCode, udtEHSAccount, ClaimRules.ClaimRulesBLL.Eligiblity.Check)
        End Function

#End Region

#Region "Step 1b2 Events 'Confirm Detail"

        'Private Sub chkStep1b2Declare_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkStep1b2Declare.CheckedChanged
        '    Me.EnableConfirmButton(Me.chkStep1b2Declare.Checked, Me.btnStep1b2Confirm)
        'End Sub

        Private Sub btnStep1b2Back_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStep1b2Back.Click
            If MyBase.IsPageRefreshed Then
                Return
            End If

            bUnchangeInputUI = True
            Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1b1
        End Sub

        Private Sub btnStep1b2Confirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStep1b2Confirm.Click
            If MyBase.IsPageRefreshed Then
                Return
            End If

            Me.mvAccountCreation.ActiveViewIndex = TextConfirmDeclare
        End Sub

        Private Sub btnStep1b2Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStep1b2Cancel.Click
            If MyBase.IsPageRefreshed Then
                Return
            End If

            Me.mvAccountCreation.ActiveViewIndex = TextConfirmCancelBox
        End Sub

        Private Sub ConfirmDeclareAcct()
            Dim practiceDisplay As PracticeDisplayModel = Me._udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)
            Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
            Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
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
                Me._udtSessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctCode)
                EHSAccountCreationBase.AuditLogStep1b2Complete(_udtAuditLogEntry, udtEHSAccount)
                Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1c
            Else
                ' To Do: Display Error!
                Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
                Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail, _udtAuditLogEntry, Common.Component.LogID.LOG00031, "Confirm Detail Failed")
            End If
        End Sub

        Protected Overrides Sub SetupStep1b2(ByVal udtEHSAccount As EHSAccountModel)
            Me.btnStep1b2Cancel.Visible = False
            Me.btnStep1b2Back.Visible = True

            If Not udtEHSAccount Is Nothing Then
                Me.udcStep1b2ReadOnlyDocumnetType.TextOnlyVersion = True
                Me.udcStep1b2ReadOnlyDocumnetType.DocumentType = udtEHSAccount.SearchDocCode
                Me.udcStep1b2ReadOnlyDocumnetType.EHSAccount = udtEHSAccount
                Me.udcStep1b2ReadOnlyDocumnetType.Vertical = True
                Me.udcStep1b2ReadOnlyDocumnetType.MaskIdentityNo = False
                Me.udcStep1b2ReadOnlyDocumnetType.ShowAccountRefNo = False
                Me.udcStep1b2ReadOnlyDocumnetType.ShowTempAccountNotice = False
                Me.udcStep1b2ReadOnlyDocumnetType.ShowAccountCreationDate = False
                Me.udcStep1b2ReadOnlyDocumnetType.TextOnlyVersion = True
                Me.udcStep1b2ReadOnlyDocumnetType.Built()
            End If
        End Sub

        Private Sub Step1b2Confirm()
            Dim practiceDisplay As PracticeDisplayModel = Me._udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)
            Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
            Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
            Dim udtDataEntry As DataEntryUserModel = Nothing
            Dim isValid As Boolean = False
            Dim strVoucherAccountID As String = commfunct.generateSystemNum("C")
            Dim udtSmartIDContent As SmartIDContentModel = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode)

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
                If udtEHSAccount.EHSPersonalInformationList(0).DocCode = DocTypeModel.DocTypeCode.HKIC Then
                    udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).HKICSymbol = SessionHandler.HKICSymbolGetFormSession(FunctCode)
                End If
                ' CRE17-010 (OCSSS integration) [End][Chris YIM]
                Me._udtSessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctCode)
                EHSAccountCreationBase.AuditLogStep1b2Complete(_udtAuditLogEntry, udtEHSAccount)
                Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1c
            Else
                ' To Do: Display Error!
                Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
                Me.udcMsgBoxErr.BuildMessageBox()
            End If
        End Sub

#End Region

#Region "Step 1b3 SmartIC Confirmation"

        '' Event
        'Private Sub btnStep1b3PrintChangeForm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnStep1b3PrintChangeForm.Click
        '    ' Validateion
        '    If ValidateStep1b3Input() Then

        '        ' Handle Print form
        '        Dim udcSmartIDContent As BLL.SmartIDContentModel = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode)
        '        Dim strPrintDateTime As String = String.Format("DH_HCV103{0}{1}{2}{3}{4}{5}{6}", Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, Now.Second, Now.Millisecond)
        '        udcSmartIDContent.IsPrintedChangeForm = True
        '        Me._udtSessionHandler.SmartIDContentSaveToSession(FunctCode, udcSmartIDContent)
        '        Me._udtSessionHandler.EHSClaimPrintoutFunctionCodeSaveToSession(FunctCode)

        '        ScriptManager.RegisterStartupScript(Me, Page.GetType, "ChangeFormScript", "javascript:openNewWin('../Printout/VoucherAccountChangeForm_VR.aspx?TID=" + strPrintDateTime + "');", True)

        '        Me.btnStep1b3Confirm.Visible = True
        '        Me.btnStep1b3Confirm.Text = Me.GetGlobalResourceObject("AlternateText", "ConfirmBtn")
        '    End If

        'End Sub

        Private Sub btnStep1b3Cancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnStep1b3Cancel.Click
            ' Reset Error icon
            ClearStep1b3Error()

            ' Cancel Account Creation
            Me.mvAccountCreation.ActiveViewIndex = TextConfirmCancelBox
            Me.udcMsgBoxErr.Clear()
            Me.udcMsgBoxInfo.Clear()
        End Sub

        Private Sub btnStep1b3Confirm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnStep1b3Confirm.Click
            ' Validation
            If ValidateStep1b3Input() Then
                ' Validate input
                Dim udtSmartIDContent As BLL.SmartIDContentModel = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode)
                Select Case udtSmartIDContent.SmartIDReadStatus
                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    Case SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_DiffDOI_LargerDOI, _
                            SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_NotCreateBySmartID_SameDOB, _
                            SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOIDOB_CreateBySmartID_WithoutGender_SameName
                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
                        Me.Step1b3Confirm()
                    Case Else
                        ' Goto Confirm Declaration
                        Me.mvAccountCreation.ActiveViewIndex = TextConfirmDeclare
                End Select

            End If

        End Sub

        Private Sub udcStep1b3InputDocumentType_SmartIDGenderTips(ByVal sender As Object, ByVal e As System.EventArgs) Handles udcStep1b3InputDocumentType.SmartIDGenderTips

            Me.udcStep1b3InputDocumentType.ActiveViewChanged = False
            Me.ucInputTipsControl.LoadTip(ucInputTips.InputTipsType.InputSmartIDGender)
            Me.mvAccountCreation.ActiveViewIndex = TextInputTips

        End Sub

        ' Method
        Private Sub SetupStep1b3(ByVal udtEHSAccount As EHSAccountModel, ByVal activeViewChanged As Boolean)

            If MyBase.IsPageRefreshed Then
                Return
            End If

            If activeViewChanged Then
                ClearStep1b3Error()
            End If

            Dim udcSmartIDContent As BLL.SmartIDContentModel = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode)
            Dim isShowSmartIDDiff As Boolean = False

            Me.SetupSetp1b3DocumentTypeContent(udtEHSAccount, activeViewChanged, udcSmartIDContent)

            Select Case udcSmartIDContent.SmartIDReadStatus
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Case SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_DiffDOI_LargerDOI, _
                        SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_NotCreateBySmartID_SameDOB, _
                        SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOIDOB_CreateBySmartID_WithoutGender_SameName
                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
                    Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Information
                    Me.udcMsgBoxInfo.AddMessage(New SystemMessage("990000", "I", "00024"))
                    Me.udcMsgBoxInfo.BuildMessageBox()
                    'Me.tblStep1b3Reminder.Visible = True

                    'Me.panStep1b3PrintChangeForm.Visible = True
                    'Me.btnStep1b3PrintChangeForm.Text = Me.GetGlobalResourceObject("AlternateText", "PrintStatementAndChangeAcctDetail")
                    'Me.rbStep1b3PrintClaimConsentFormLanguage.Items.FindByValue("Chi").Text = Me.GetGlobalResourceObject("Text", "Chinese")
                    'Me.rbStep1b3PrintClaimConsentFormLanguage.Items.FindByValue("Eng").Text = Me.GetGlobalResourceObject("Text", "English")

                    'isShowSmartIDDiff = True
                    'If udcSmartIDContent.IsPrintedChangeForm Then
                    'Else
                    '    Me.btnStep1b3Confirm.Visible = False
                    'End If
                    Me.btnStep1b3Confirm.Visible = True
                    Me.btnStep1b3Confirm.Text = Me.GetGlobalResourceObject("AlternateText", "ConfirmBtn")

                Case Else
                    'Me.tblStep1b3Reminder.Visible = False
                    'Me.panStep1b3PrintChangeForm.Visible = False

                    isShowSmartIDDiff = False
                    Me.btnStep1b3Confirm.Visible = True
                    Me.btnStep1b3Confirm.Text = Me.GetGlobalResourceObject("AlternateText", "ConfirmBtn")
            End Select

        End Sub

        Private Sub SetupSetp1b3DocumentTypeContent(ByVal udtEHSAccount As EHSAccountModel, ByVal activeViewChanged As Boolean, ByVal udcSmartIDContent As BLL.SmartIDContentModel)
            If activeViewChanged Then

                Me.udcStep1b3InputDocumentType.TextOnlyVersion = True
                Me.udcStep1b3InputDocumentType.EHSAccount = udtEHSAccount
                Me.udcStep1b3InputDocumentType.DocType = DocType.DocTypeModel.DocTypeCode.HKIC
                Me.udcStep1b3InputDocumentType.Mode = ucInputDocTypeBase.BuildMode.Creation
                Me.udcStep1b3InputDocumentType.FillValue = True
                Me.udcStep1b3InputDocumentType.SchemeClaim = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
                Me.udcStep1b3InputDocumentType.SmartIDContent = udcSmartIDContent
                Me.udcStep1b3InputDocumentType.Built()

            End If
        End Sub

        Private Sub SetStep1b3GenderFromControl()
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

                    Dim udcInputHKIDSmartID As UIControl.DocTypeText.ucInputHKIDSmartID = Me.udcStep1b3InputDocumentType.GetHKICSmartIDTextControl()
                    udcInputHKIDSmartID.SetProperty(ucInputDocTypeBase.BuildMode.Creation)
                    strGender = udcInputHKIDSmartID.Gender
                    isShowSmartIDDiff = True

                Case Else
                    Dim udcInputHKIDSmartIDSignal As UIControl.DocTypeText.ucInputHKIDSmartIDSignal = Me.udcStep1b3InputDocumentType.GetHKICSmartIDSignalTextControl()
                    udcInputHKIDSmartIDSignal.SetProperty(ucInputDocTypeBase.BuildMode.Creation)
                    strGender = udcInputHKIDSmartIDSignal.Gender
                    isShowSmartIDDiff = False
            End Select

            If Not String.IsNullOrEmpty(strGender) Then
                udcSmartIDContent.EHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).Gender = strGender
                Me._udtSessionHandler.SmartIDContentSaveToSession(FunctCode, udcSmartIDContent)
            End If

        End Sub

        Private Function ValidateStep1b3Input() As Boolean
            Dim isvalid As Boolean = True

            ' Reset Error icon
            Me.ClearStep1b3Error()

            ' Set value into model
            Me.SetStep1b3GenderFromControl()

            ' Validate input
            Dim udtSmartIDContent As BLL.SmartIDContentModel = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode)
            Dim gender As String = udtSmartIDContent.EHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).Gender
            Me._udtEHSAccount = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
            If String.IsNullOrEmpty(gender) Then
                SetStep1b3GenderErrorVisible(True)

                Me.udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00034"))     'Please select "Gender".
                isvalid = False
            Else
                'udtSmartIDContent.EHSAccount records save to database
                udtSmartIDContent.EHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKIC).Gender = gender
                Me._udtSessionHandler.SmartIDContentSaveToSession(FunctCode, udtSmartIDContent)

                'HKIC Control value assigned by Me._udtEHSAccount
                ' INT13-0021 - Fix use HKBC on smart IC claim incorrectly [Start][Koala]
                ' -------------------------------------------------------------------------------------
                If Me._udtEHSAccount.getPersonalInformation(DocType.DocTypeModel.DocTypeCode.HKIC) IsNot Nothing Then
                    Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKIC).Gender = gender
                End If
                ' INT13-0021 - Fix use HKBC on smart IC claim incorrectly [End][Koala]
                Me._udtSessionHandler.EHSAccountSaveToSession(Me._udtEHSAccount, FunctCode)
            End If

            'If isvalid Then
            '    'IsPrintedChangeForm default is true, 
            '    'if is case of change personal particual, IsPrintedChangeForm default is false <- After Printed form value will change to true
            '    If udtSmartIDContent.IsPrintedChangeForm Then
            '        Me.udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00144"))
            '    End If
            'End If

            If Not isvalid Then
                Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail)
            Else
                Me.udcStep1b3InputDocumentType.ActiveViewChanged = True
            End If

            Return isvalid
        End Function

        Private Sub ClearStep1b3Error()

            SetStep1b3GenderErrorVisible(False)

        End Sub

        Private Sub SetStep1b3GenderErrorVisible(ByVal visible As Boolean)

            Dim ucInputHKIDSmartIDControl As UIControl.DocTypeText.ucInputHKIDSmartID = Me.udcStep1b3InputDocumentType.GetHKICSmartIDTextControl()
            If Not ucInputHKIDSmartIDControl Is Nothing Then
                ucInputHKIDSmartIDControl.SetGenderSmartIDError(visible)
            End If

            Dim ucInputHKICSmartIDSignalControl As UIControl.DocTypeText.ucInputHKIDSmartIDSignal = Me.udcStep1b3InputDocumentType.GetHKICSmartIDSignalTextControl()
            If Not ucInputHKICSmartIDSignalControl Is Nothing Then
                ucInputHKICSmartIDSignalControl.SetGenderSmartIDError(visible)
            End If

        End Sub

        Private Sub Step1b3Confirm()
            Dim udtSmartIDContent As SmartIDContentModel = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode)
            Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
            Dim practiceDisplay As PracticeDisplayModel = Me._udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)
            Dim commFunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
            Dim udtSchemeClaim As Scheme.SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
            Dim udtDataEntry As DataEntryUserModel = Nothing
            Dim isValid As Boolean = False
            Dim udtSystemMessage As Common.ComObject.SystemMessage = Nothing

            Dim blnNeedImmDValidation As Boolean = False

            Me._udtEHSAccountBLL = New EHSAccountBLL()
            Me._udtSessionHandler.CurrentUserGetFromSession(Me._udtSP, udtDataEntry)

            EHSAccountCreationBase.AuditLogStep1b3Start(_udtAuditLogEntry, udtSmartIDContent)

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

                    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                    ' ----------------------------------------------------------
                    If udtSmartIDContent.EHSAccount.SearchDocCode = DocTypeModel.DocTypeCode.HKIC Then
                        udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).HKICSymbol = SessionHandler.HKICSymbolGetFormSession(FunctCode)
                    End If
                    ' CRE17-010 (OCSSS integration) [End][Chris YIM]

                    'udtSmartIDContent.EHSAccount = Me._udtEHSAccountBLL.LoadTempEHSAccountByVRID(udtEHSAccount.VoucherAccID)
                    ' INT13-0021 - Fix use HKBC on smart IC claim incorrectly [Start][Koala]
                    ' -------------------------------------------------------------------------------------
                    udtEHSAccount.SetSearchDocCode(udtSmartIDContent.EHSAccount.SearchDocCode)
                    udtSmartIDContent.EHSAccount = udtEHSAccount
                    'udtSmartIDContent.EHSAccount.SetSearchDocCode(udtSmartIDContent.EHSAccount.EHSPersonalInformationList(0).DocCode)
                    ' INT13-0021 - Fix use HKBC on smart IC claim incorrectly [End][Koala]
                    Me._udtSessionHandler.SmartIDContentSaveToSession(FunctCode, udtSmartIDContent)
                    Me._udtSessionHandler.EHSAccountSaveToSession(udtSmartIDContent.EHSAccount, FunctCode)
                    EHSAccountCreationBase.AuditLogStep1b3Complete(_udtAuditLogEntry, udtEHSAccount, udtSmartIDContent, True)

                    Me.BackToClaim(True)
                Case Else
                    Dim strVoucherAccountID As String = commFunct.generateSystemNum("C")

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

                    If udtSystemMessage Is Nothing Then
                        udtSmartIDContent.EHSAccount = Me._udtEHSAccountBLL.LoadTempEHSAccountByVRID(strVoucherAccountID)
                        udtSmartIDContent.EHSAccount.SetSearchDocCode(udtSmartIDContent.EHSAccount.EHSPersonalInformationList(0).DocCode)

                        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                        ' ----------------------------------------------------------
                        If udtSmartIDContent.EHSAccount.EHSPersonalInformationList(0).DocCode = DocTypeModel.DocTypeCode.HKIC Then
                            udtSmartIDContent.EHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).HKICSymbol = SessionHandler.HKICSymbolGetFormSession(FunctCode)
                        End If
                        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

                        Me._udtSessionHandler.EHSAccountSaveToSession(udtSmartIDContent.EHSAccount, FunctCode)

                        udtEHSAccount = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)

                        EHSAccountCreationBase.AuditLogStep1b3Complete(_udtAuditLogEntry, udtEHSAccount, udtSmartIDContent, False)
                        Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1c
                    Else
                        ' To Do: Display Error!
                        Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
                        Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail, _udtAuditLogEntry, Common.Component.LogID.LOG00059, "Confirm SmartID Detail Failed")
                    End If

            End Select


        End Sub

#End Region

#Region "Step 1c Events confirm Account Creation"

        Private Sub btnStep1cProceedToClaim_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStep1cProceedToClaim.Click
            If MyBase.IsPageRefreshed Then
                Return
            End If

            Me._udtSessionHandler.ExceedDocTypeLimitRemoveFromSession()
            Me._udtSessionHandler.NotMatchAccountExistRemoveFromSession()
            Me._udtSessionHandler.CCCodeRemoveFromSession(FunctCode)
            Me._udtSessionHandler.AccountCreationProceedClaimSaveToSession(True)

            EHSAccountCreationBase.AuditLogStep1cProccedToclaim(_udtAuditLogEntry)

            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

            RedirectHandler.ToURL(EHSClaim)

            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

        End Sub

        Private Sub btnStep1cNextCreation_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStep1cNextCreation.Click
            If MyBase.IsPageRefreshed Then
                Return
            End If

            Me._udtSessionHandler.EHSClaimSessionRemove(FunctCode)

            EHSAccountCreationBase.AuditLogStep1cCreateNewAccount(_udtAuditLogEntry)

            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

            RedirectHandler.ToURL(EHSClaim)

            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End
        End Sub

        Protected Overrides Sub SetupStep1c(ByVal udtEHSAccount As EHSAccountModel, ByVal activeViewChange As Boolean)
            Me._systemMessage = New Common.ComObject.SystemMessage("020201", "I", "00001")
            Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Complete
            Me.udcMsgBoxInfo.AddMessage(Me._systemMessage)
            Me.udcMsgBoxInfo.BuildMessageBox()

            If Not udtEHSAccount Is Nothing Then
                Me.udcStep1cReadOnlyDocumnetType.TextOnlyVersion = True
                Me.udcStep1cReadOnlyDocumnetType.DocumentType = udtEHSAccount.SearchDocCode
                Me.udcStep1cReadOnlyDocumnetType.EHSAccount = udtEHSAccount
                Me.udcStep1cReadOnlyDocumnetType.Vertical = True
                Me.udcStep1cReadOnlyDocumnetType.MaskIdentityNo = False
                Me.udcStep1cReadOnlyDocumnetType.ShowAccountRefNo = True
                Me.udcStep1cReadOnlyDocumnetType.ShowTempAccountNotice = False
                Me.udcStep1cReadOnlyDocumnetType.ShowAccountCreationDate = True
                Me.udcStep1cReadOnlyDocumnetType.TextOnlyVersion = True

                Me.udcStep1cReadOnlyDocumnetType.Built()
            End If

            If activeViewChange Then
                EHSAccountCreationBase.AuditLogStep1cComplete(_udtAuditLogEntry, udtEHSAccount)
            End If
        End Sub

#End Region

#Region "Confirm Declaration"
        ' Event
        Private Sub btnConfirmDeclareReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirmDeclareReturn.Click
            If MyBase.IsPageRefreshed Then
                Return
            End If

            If _udtSessionHandler.SmartIDContentGetFormSession(FunctCode) Is Nothing Then
                Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1b2
            Else
                Me.mvAccountCreation.ActiveViewIndex = Step1b3
            End If

        End Sub

        Private Sub btnConfirmDeclareConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirmDeclareConfirm.Click
            If MyBase.IsPageRefreshed Then
                Return
            End If

            If _udtSessionHandler.SmartIDContentGetFormSession(FunctCode) Is Nothing Then
                ' Normal Case
                Step1b2Confirm()
            Else
                ' Smart IC Case
                Step1b3Confirm()
            End If

        End Sub

        ' Method
        Private Sub SetupStepConfirmDeclaration()

            Dim udcSmartIDContent As BLL.SmartIDContentModel = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode)
            Me.udcMsgBoxErr.Clear()
            If udcSmartIDContent Is Nothing Then
                lblConfirmDeclareMessage.Text = GetGlobalResourceObject("Text", "ProvidedInfoTrueVAForm")
            Else
                Select Case udcSmartIDContent.SmartIDReadStatus
                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    Case SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_DiffDOI_LargerDOI, _
                         SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_NotCreateBySmartID_SameDOB, _
                         SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOIDOB_CreateBySmartID_WithoutGender_SameName
                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
                        lblConfirmDeclareMessage.Text = Me.GetGlobalResourceObject("Text", "ProvidedInfoTrueVAChangeForm")

                    Case Else
                        lblConfirmDeclareMessage.Text = Me.GetGlobalResourceObject("Text", "ProvidedInfoTrueVAForm")
                End Select
            End If

        End Sub

#End Region

#Region "Change Practice"

        Private Sub btnStepSelectPracticeGO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStepSelectPracticeGO.Click
            If MyBase.IsPageRefreshed Then
                Return
            End If

            Dim isValidForCreation As Boolean = True
            Dim strPracticeId As String = ucSelectPracticeRadioButtonGroupText.SelectedValue.Trim()
            Dim udtSchemeClaimModelCollection As Scheme.SchemeClaimModelCollection = Nothing
            Dim udtSchemeClaimBLL As Scheme.SchemeClaimBLL = New Scheme.SchemeClaimBLL()
            Dim udtDataEntryUser As DataEntryUserModel = Nothing
            Dim udtSchemeClaim As Scheme.SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
            Dim udtPracticeDisplays As BLL.PracticeDisplayModelCollection = SessionHandler.PracticeDisplayListGetFromSession()
            Dim udtSelectedPracticedisplay As BLL.PracticeDisplayModel = Nothing

            Me.SessionHandler.CurrentUserGetFromSession(Me._udtSP, udtDataEntryUser)
            'Me.SessionHandler.EHSClaimSessionRemove("")

            If String.IsNullOrEmpty(strPracticeId) Then
                'Remove related session object if no practice is selected
                SessionHandler.PracticeDisplayRemoveFromSession(FunctCode)
                SessionHandler.SchemeSelectedRemoveFromSession(FunctCode)
                'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                SessionHandler.NonClinicSettingRemoveFromSession(FunctionCode)
                'CRE16-002 (Revamp VSS) [End][Chris YIM]
                SessionHandler.DocumentTypeSelectedRemoveFromSession(FunctCode)
                'SessionHandler.EHSClaimSessionRemove(FunctCode)

                ' CRE20-0XX (HA Scheme) [Start][Winnie]
                SessionHandler.SchemeSelectedForPracticeRemoveFromSession(FunctCode)
                ' CRE20-0XX (HA Scheme) [Start][Winnie]
            Else
                ' Check the selected practice is same as the one in session or not.
                Dim sessionPractice As BLL.PracticeDisplayModel = SessionHandler.PracticeDisplayGetFromSession(FunctCode)
                Dim intPracticeId As Integer = Convert.ToInt32(strPracticeId)
                Dim udtAuditLogEntry As New AuditLogEntry(FunctCode, Me)

                udtSelectedPracticedisplay = udtPracticeDisplays.Filter(intPracticeId)

                If Not sessionPractice Is Nothing AndAlso sessionPractice.PracticeID.ToString() <> strPracticeId Then

                    Dim sessionScheme As SchemeClaimModel = SessionHandler.SchemeSelectedGetFromSession(FunctCode)

                    udtSchemeClaimModelCollection = udtSchemeClaimBLL.searchValidClaimPeriodSchemeClaimByPracticeSchemeInfoSubsidizeCode(Me._udtSP.PracticeList(intPracticeId).PracticeSchemeInfoList, Me._udtSP.SchemeInfoList)
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
                    SessionHandler.NonClinicSettingRemoveFromSession(FunctionCode)
                    'CRE16-002 (Revamp VSS) [End][Chris YIM]
                Else
                    EHSAccountCreationBase.AuditLogPracticeSelected(udtAuditLogEntry, True, udtSelectedPracticedisplay, udtSchemeClaim, True)
                End If

                ' save practice to session
                SessionHandler.PracticeDisplaySaveToSession(udtPracticeDisplays.Filter(intPracticeId), FunctCode)

                If isValidForCreation Then
                    If StepSelectPracticeValdiation() Then
                        bUnchangeInputUI = True
                        Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1b1
                    End If
                Else
                    Me.mvAccountCreation.ActiveViewIndex = TextConfirmOnlyBox
                End If

            End If
        End Sub

        Private Function StepSelectPracticeValdiation() As Boolean
            ' Check practice is selected in session
            Dim blnIsValid As Boolean = False
            Dim udtSessionPractice As BLL.PracticeDisplayModel = SessionHandler.PracticeDisplayGetFromSession(FunctCode)

            If udtSessionPractice Is Nothing Then
                ' Error
                blnIsValid = False

                Dim udtSysMessagePractice As SystemMessage = New SystemMessage(FunctCode, "E", "00017")
                Me.ShowError(udtSysMessagePractice)
            Else
                blnIsValid = True
            End If

            Return blnIsValid
        End Function

        Private Sub SetupSelectPractice()

            Dim udtPracticeDisplays As BLL.PracticeDisplayModelCollection = SessionHandler.PracticeDisplayListGetFromSession()
            Dim udtDataEntryUser As DataEntryUserModel = Nothing

            SessionHandler.CurrentUserGetFromSession(Me._udtSP, udtDataEntryUser)

            'Build practice Selection List
            ucSelectPracticeRadioButtonGroupText.MaskBankAccountNo = True
            ucSelectPracticeRadioButtonGroupText.BuildRadioButtonGroup(True, udtPracticeDisplays, Me._udtSP.PracticeList, SessionHandler.Language, PracticeRadioButtonGroup.DisplayMode.BankAccount)

            ' Check Session Object exists, and select
            Dim udtSessionPracticeDisplay As BLL.PracticeDisplayModel = SessionHandler.PracticeDisplayGetFromSession(FunctCode)
            If Not udtSessionPracticeDisplay Is Nothing AndAlso String.IsNullOrEmpty(ucSelectPracticeRadioButtonGroupText.SelectedValue) Then
                ucSelectPracticeRadioButtonGroupText.SelectedValue = udtSessionPracticeDisplay.PracticeID
            End If

            If Me._udtSessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                ucSelectPracticeRadioButtonGroupText.CssClass = "tableTextChi"
            Else
                ucSelectPracticeRadioButtonGroupText.CssClass = "tableText"
            End If

        End Sub

#End Region

#Region "Confirm Only"

        Private Sub btnConfirmOnlyBoxConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirmOnlyBoxConfirm.Click
            If MyBase.IsPageRefreshed Then
                Return
            End If

            Me._udtSessionHandler.EHSClaimSessionRemove(FunctCode)
            SessionHandler.SchemeSelectedRemoveFromSession(FunctCode)
            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            SessionHandler.NonClinicSettingRemoveFromSession(FunctionCode)
            'CRE16-002 (Revamp VSS) [End][Chris YIM]
            SessionHandler.DocumentTypeSelectedRemoveFromSession(FunctCode)

            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

            RedirectHandler.ToURL(EHSClaim)

            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End
        End Sub

#End Region

#Region "Other functions"

        '-----------------------------------------------------------------------------------------------------------------------------
        'Show Change Practice button
        '-----------------------------------------------------------------------------------------------------------------------------
        Private Sub ShowChangePracticeButton(ByVal imageButton As Button)
            Dim udtPracticeDisplays As BLL.PracticeDisplayModelCollection = Me._udtSessionHandler.PracticeDisplayListGetFromSession()
            If udtPracticeDisplays.Count > 1 Then
                imageButton.Visible = True
            Else
                imageButton.Visible = False
            End If

        End Sub

        Private Function GetStepText() As String
            Select Case Me.mvAccountCreation.ActiveViewIndex
                Case ActiveViewIndex.Step1a1, ActiveViewIndex.Step1a2
                    Return Me.GetGlobalResourceObject("Text", "EHAStep1")
                Case ActiveViewIndex.Step1b1, ActiveViewIndex.Step1b2, TextConfirmDeclare
                    Return Me.GetGlobalResourceObject("Text", "EHAStep2")
                Case ActiveViewIndex.Step1c
                    Return Me.GetGlobalResourceObject("Text", "EHAStep3")
                Case Else
                    Return String.Empty
            End Select
        End Function

        ' Build Menu
        Private Sub BuildMenuItem()
            ' Menu not apply in the claim process
            Dim udtEHSAccount As EHSAccountModel = SessionHandler.EHSAccountGetFromSession(FunctCode)
            If udtEHSAccount Is Nothing OrElse Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1c Then
                CType(Me.Master, ClaimVoucherMaster).BuildMenu(FunctCode, SessionHandler.Language())
            Else
                CType(Me.Master, ClaimVoucherMaster).ClearMenu()
            End If

        End Sub

        'Back to Claim Page
        Private Sub BackToClaim(ByVal proceedToClaim As Boolean)
            If Not proceedToClaim Then
                Me._udtSessionHandler.EHSClaimStepsRemoveFromSession(FunctCode)
                Me._udtSessionHandler.EHSClaimSessionRemove(FunctCode)
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

#End Region

#Region "Input tips"

        Private Sub btnInputTipReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInputTipBack.Click
            If MyBase.IsPageRefreshed Then
                Return
            End If

            bUnchangeInputUI = True
            Dim udtSmartIDContent As SmartIDContentModel = MyBase.SessionHandler.SmartIDContentGetFormSession(FunctCode)

            If Not udtSmartIDContent Is Nothing AndAlso udtSmartIDContent.IsReadSmartID Then
                Me.mvAccountCreation.ActiveViewIndex = Step1b3
            Else
                Me.mvAccountCreation.ActiveViewIndex = ActiveViewIndex.Step1b1
            End If
        End Sub

#End Region

#Region "Notification / Error Message Box"

        Public Sub HideMessageBox()
            Me.udcMsgBoxErr.Visible = False
            Me.udcMsgBoxInfo.Visible = False
        End Sub

        Private Sub ClearMessageBox()
            Me.udcMsgBoxErr.Clear()
            Me.udcMsgBoxInfo.Clear()
        End Sub

        Public Sub ShowError(ByVal systemMessage As SystemMessage)
            Me.ShowError(New SystemMessage() {systemMessage})

        End Sub

        Public Sub ShowError(ByVal systemMessages As SystemMessage())
            For Each systemMessage As SystemMessage In systemMessages
                Me.udcMsgBoxErr.AddMessage(systemMessage)
            Next
            Me.udcMsgBoxErr.Visible = True
            Me.udcMsgBoxErr.BuildMessageBox(_strValidationFail)

        End Sub

        Public Sub ShowInfoMessage(ByVal systemMessage As SystemMessage)
            Me.udcMsgBoxInfo.AddMessage(systemMessage)
            Me.udcMsgBoxInfo.BuildMessageBox()
            Me.udcMsgBoxInfo.Visible = True

        End Sub

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

End Namespace