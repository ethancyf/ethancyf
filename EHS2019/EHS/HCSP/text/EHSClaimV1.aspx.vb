Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.ClaimCategory
Imports Common.Component.ClaimRules
Imports Common.Component.ClaimRules.ClaimRulesBLL
Imports Common.Component.DataEntryUser
Imports Common.Component.DocType
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.Scheme
Imports Common.Component.ServiceProvider
Imports Common.Component.UserAC
Imports Common.Component.VoucherScheme
Imports Common.Format
Imports Common.OCSSS
Imports Common.Validation
Imports Common.WebService.Interface
Imports HCSP.BLL
Imports System.Net
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.VoucherInfo
Imports Common.Component.Profession
Imports IdeasRM

Namespace Text

    Partial Public Class EHSClaimV1
        Inherits EHSClaimBasePage

#Region "Private Class"

        Private Class VaccinationRecordPopupStatusClass
            Public Const Active As String = "A"
            Public Const Closed As String = "C"
        End Class

        Private Class VS
            Public Const VaccinationRecordPopupStatus As String = "VaccinationRecordPopupStatus"
        End Class

#End Region

#Region "Private Member"
        ' Private Member
        Private _udtClaimVoucherBLL As BLL.ClaimVoucherBLL = New BLL.ClaimVoucherBLL()
        Private _udtEHSClaimBLL As BLL.EHSClaimBLL = New BLL.EHSClaimBLL()
        Private _udtEHSAccountBll As EHSAccountBLL = New EHSAccountBLL()
        Private _udtEHSTransactionBLL As EHSTransactionBLL = New EHSTransactionBLL()
        Private _udtGeneralFunction As New GeneralFunction

        Private _udtPracticeAcctBLL As BLL.PracticeBankAcctBLL = New BLL.PracticeBankAcctBLL()
        Private _udtPracticeBankAccBLL As New BLL.PracticeBankAcctBLL()
        Private _udtStaticDataBLL As New Common.Component.StaticData.StaticDataBLL()
        Private _udtClaimCategoryBLL As New ClaimCategoryBLL()

        Private _udtSP As ServiceProviderModel = Nothing
        Private _udtDataEntryModel As DataEntryUserModel = Nothing
        Private _udtVoucherScheme As VoucherSchemeModel = New VoucherSchemeModel()
        Private _udtUserAC As UserACModel = New UserACModel()

        Private _udtEHSTransaction As EHSTransactionModel
        Private _udtCommfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction()
        Private _strValidationFail As String = "ValidationFail"
        Private _udtFormatter As Common.Format.Formatter = New Common.Format.Formatter()

        Private _blnIsRequireHandlePageRefresh As Boolean = False
        Private _blnConcurrentUpdate As Boolean = False
        Private _blnIsConcurrentBrowserDetected As Boolean = False

        Private _udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

#End Region

#Region "Constant"
        Public Const ValidatedServiceDate As String = "ValidatedServiceDate"
        Public Const EHSClaimV2 As String = "EHSClaimV2.aspx"

        Private Const ConfirmationNormalCSSClass = "tableText"
        Private Const ConfirmationTitleCSSClass = "tableTitle"
        Private Const ConfirmationBorderedCSSClass = "tableRemark"
        Private Const ConfirmationBorderedHighlightCSSClass = "tableRemarkHighlight"
        Private Const ConfirmationUnderlineCSSClass = "tableHeader"
        Private Const SelectedPerConditionValueAttribute As String = "SelectedPerCondition"

        ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [Start][Winnie]
        Private Const DuplicateClaimAlertMessage As String = "DuplicateClaimAlertMessage"
        ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [End][Winnie]

        ' Confirmation Display Style
        Protected Enum ConfirmationStyle
            NotSet
            Normal
            Bordered
            BorderedUnderline
            BorderedUnderlineHighlight
            Underline
        End Enum

#End Region

#Region "Property"

        Protected Overrides ReadOnly Property FunctionCode() As String
            Get
                Return Common.Component.FunctCode.FUNT020202
            End Get
        End Property

        Protected ReadOnly Property SessionSP() As ServiceProviderModel
            Get
                Me.GetCurrentUserAccount(Me._udtSP, Me._udtDataEntryModel, False)

                Return _udtSP
            End Get
        End Property

#End Region

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
            ' -----------------------------------------------------------------------------------------

            Me.udcReasonForVisit.FunctionCode = Me.FunctionCode

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            Dim udtPracticeDisplays As BLL.PracticeDisplayModelCollection
            Dim masterPage As ClaimVoucherMaster = CType(Me.Master, ClaimVoucherMaster)
            Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode)

            Dim udtSmartIDContent As BLL.SmartIDContentModel = Nothing

            'Get Current USer Account for check Session Expired
            Me._udtUserAC = UserACBLL.GetUserAC

            ' Set Title
            CType(Me.Master.FindControl(ClaimVoucherMaster.ControlName.lblTitle), Label).Text = Me.GetGlobalResourceObject("Text", "EVoucherSystem")
            ' Set SubTitle
            CType(Me.Master.FindControl(ClaimVoucherMaster.ControlName.lblSubTitle), Label).Text = Me.GetGlobalResourceObject("Text", "ClaimVoucher")
            ' Set Step
            CType(Me.Master.FindControl(ClaimVoucherMaster.ControlName.LblClaimVoucherStep), Label).Text = Me.GetStepText()

            AddHandler masterPage.MenuChanged, AddressOf MasterPage_MenuChanged

            ' Construct the Menu
            Me.BuildMenuItem(masterPage)

            If Not IsPostBack Then
                udtSmartIDContent = MyBase.SessionHandler.SmartIDContentGetFormSession(FunctionCode)

                'Get Current User Account
                Me.GetCurrentUserAccount(Me._udtSP, Me._udtDataEntryModel, True)

                'Step 1 of Page Load : if no Selected Practice in session, go to Practice selection Page
                Dim udtSelectedPracticeDisplay As BLL.PracticeDisplayModel = SessionHandler.PracticeDisplayGetFromSession(FunctionCode)
                If udtSelectedPracticeDisplay Is Nothing Then

                    EHSClaimBasePage.AuditLogPageLoad(_udtAuditLogEntry, False, False)

                    If Not Me._udtDataEntryModel Is Nothing Then
                        udtPracticeDisplays = Me._udtPracticeBankAccBLL.getActivePracticeWithAvailableScheme(Me._udtSP.SPID, Me._udtDataEntryModel.DataEntryAccount, Me._udtSP.PracticeList, Me._udtSP.SchemeInfoList)
                    Else
                        udtPracticeDisplays = Me._udtPracticeBankAccBLL.getActivePracticeWithAvailableScheme(Me._udtSP.SPID, Me._udtSP.PracticeList, Me._udtSP.SchemeInfoList)
                    End If

                    ' CRE13-001 - EHAPP [Start][Tommy L]
                    ' -------------------------------------------------------------------------------------
                    udtPracticeDisplays = udtPracticeDisplays.FilterByTextOnlyAvailable(Me._udtSP)
                    ' CRE13-001 - EHAPP [End][Tommy L]

                    ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]
                    ' -----------------------------------------------------------------------------------------
                    ' Save practice display list before any action
                    Me.SessionHandler.PracticeDisplayListSaveToSession(udtPracticeDisplays)
                    ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

                    If udtPracticeDisplays Is Nothing OrElse udtPracticeDisplays.Count = 0 Then
                        ' SP have no avaiable practice -> go to vEHSClaimError
                        Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.InternalError
                    ElseIf udtPracticeDisplays IsNot Nothing AndAlso Not udtPracticeDisplays.HasPracticeAvailableForClaim() Then
                        ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]
                        ' -----------------------------------------------------------------------------------------
                        ' Step 1 of Page Load :Practice is active but not available for claim -> go to vEHSClaimError
                        Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.InternalError

                        ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]
                    ElseIf udtPracticeDisplays.Count = 1 Then
                        'Step 3 of Page Load :  Only 1 Active Practice, Auto Select -> go to Search Account Page (Step1)
                        Me.SessionHandler.PracticeDisplaySaveToSession(udtPracticeDisplays(0), FunctionCode)
                        Me.SessionHandler.PracticeDisplayListSaveToSession(udtPracticeDisplays)
                        Me.Clear()
                        Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SelectScheme
                    Else
                        ' go to Select Practice -> go to Select Practice Page
                        SessionHandler.PracticeDisplayListSaveToSession(udtPracticeDisplays)
                        Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SelectPractice
                    End If
                Else
                    If SessionHandler.AccountCreationProceedClaimGetFromSession() Then

                        'EHSClaimBasePage.AuditLogPageLoad(FunctionCode, True, True)
                        EHSClaimBasePage.AuditLogEnterClaimDetailLoaded(_udtAuditLogEntry)

                        Me.EHSClaimTokenNumAssign(Me.hfEHSClaimTokenNum, FunctionCode)

                        Me.ResetStep2aServiceDate()

                        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
                        ' --------------------------------------------------------------------------------------
                        Select Case udtSchemeClaim.ControlType
                            Case SchemeClaimModel.EnumControlType.CIVSS, SchemeClaimModel.EnumControlType.EVSS, SchemeClaimModel.EnumControlType.RVP, _
                                 SchemeClaimModel.EnumControlType.HSIVSS, SchemeClaimModel.EnumControlType.PIDVSS, SchemeClaimModel.EnumControlType.VSS, _
                                 SchemeClaimModel.EnumControlType.ENHVSSO, SchemeClaimModel.EnumControlType.PPP

                                Me.GoToVersion2()

                            Case Else
                                'Step 5 of Page Load : if is come from Account creation and user pressed "proceed to claim" -> got to Enter Claim Detail
                                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step2a
                        End Select
                        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

                        ' Default service date in here: avoid change view and service date being reset
                        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                        Dim udtSubPlatformBLL As New SubPlatformBLL
                        'txtStep2aServiceDate.Text = DateTime.Today.ToString(_udtFormatter.EnterDateFormat)
                        txtStep2aServiceDate.Text = DateTime.Today.ToString(_udtFormatter.EnterDateFormat(udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform)))
                        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

                        ' Remove account creation's session
                        SessionHandler.AccountCreationProceedClaimRemoveFromSession()
                    Else
                        EHSClaimBasePage.AuditLogPageLoad(_udtAuditLogEntry, True, False)

                        ' If All Practice/ Scheme/ Doc Type is presented, go to Step1
                        Dim udtSelectedPractice = SessionHandler.PracticeDisplayGetFromSession(FunctionCode)
                        Dim udtSelectedScheme = SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
                        Dim udtSelectedDocType = SessionHandler.DocumentTypeSelectedGetFromSession(FunctionCode)

                        If udtSelectedPractice Is Nothing Then
                            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SelectPractice
                        ElseIf udtSelectedScheme Is Nothing Then
                            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SelectScheme
                        ElseIf udtSelectedDocType Is Nothing Then
                            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SelectDocType
                        Else

                            If Not Me.ReadSmartID(udtSmartIDContent) Then
                                Me.Step1Clear(False)
                                Me.ClearEHSClaimState()
                                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
                            End If
                        End If

                    End If

                End If

                ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                ' ----------------------------------------------------------
                If Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1 Then
                    Me.SessionHandler.HKICSymbolRemoveFromSession(FunctionCode)
                    Me.SessionHandler.OCSSSRefStatusRemoveFromSession(FunctionCode)
                    Me.SessionHandler.UIDisplayHKICSymbolRemoveFromSession(FunctionCode)
                End If
                ' CRE17-010 (OCSSS integration) [End][Chris YIM]

            Else
                ' Check the Page is valid to load
                Dim blnIsValid As Boolean = Me.ValidateViewStatus()
                If blnIsValid = True Then
                    Select Case Me.mvEHSClaim.ActiveViewIndex
                        Case ActiveViewIndex.Step1
                            Me.SetupStep1(True, False)

                        Case ActiveViewIndex.Step2a
                            Dim udtEHSAccount As EHSAccountModel = SessionHandler.EHSAccountGetFromSession(FunctionCode)
                            Me.SetupStep2a(udtEHSAccount, True, False)
                        Case ActiveViewIndex.Step2b
                            Dim udtEHSAccount As EHSAccountModel = SessionHandler.EHSAccountGetFromSession(FunctionCode)
                            Me.SetupStep2b(udtEHSAccount, False)
                            ' Generate Dynamic Control
                            Me.Step2aBuildInputEHSClaimControl()

                        Case ActiveViewIndex.Step3
                            Dim udtEHSAccount As EHSAccountModel = SessionHandler.EHSAccountGetFromSession(FunctionCode)
                            Me.SetupStep3(udtEHSAccount, False)

                        Case ActiveViewIndex.SelectPractice
                            Me.SetupSelectPractice()
                            ' Generate Dynamic Control
                            Me.Step2aBuildInputEHSClaimControl()

                        Case ActiveViewIndex.SelectScheme
                            Me.SetupSelectScheme(False)

                            Dim udtClaimCategory As ClaimCategoryModel = MyBase.SessionHandler.ClaimCategoryGetFromSession(FunctionCode)

                            If Not udtSchemeClaim Is Nothing Then
                                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                                'If claim category is nothing -> this page is come from claim for same patient -> claim for same patient delete Claim category
                                If udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.RVP) OrElse udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.HSIVSS) Then
                                    If Not udtClaimCategory Is Nothing Then
                                        ' Generate Dynamic Control
                                        Me.Step2aBuildInputEHSClaimControl()
                                    End If
                                Else
                                    'HCVS, CIVSS
                                    Me.Step2aBuildInputEHSClaimControl()
                                End If
                                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
                            End If


                        Case ActiveViewIndex.SelectDocType
                            Me.SetupSelectDocumentType()

                        Case ActiveViewIndex.InternalError
                            Me.SetupInternalError()

                        Case ActiveViewIndex.ConfirmBox
                            Me.SetupConfirmBox()
                            ' Generate Dynamic Control
                            Me.Step2aBuildInputEHSClaimControl()

                        Case ActiveViewIndex.InputTip
                            Me.SetupInputTips()
                            ' Generate Dynamic Control
                            Me.Step2aBuildInputEHSClaimControl()

                        Case ActiveViewIndex.Remark
                            Me.ShowVaccineRemarkText()
                            ' Generate Dynamic Control
                            Me.Step2aBuildInputEHSClaimControl()

                        Case ActiveViewIndex.ReasonForVisit
                            Me.SetupReasonForVisit()
                            ' Generate Dynamic Control
                            Me.Step2aBuildInputEHSClaimControl()

                        Case ActiveViewIndex.Category
                            Me.SetupCategory(Nothing)

                            ' Generate Dynamic Control when User Choosed Change Category
                            Dim udtClaimCategory As ClaimCategoryModel = MyBase.SessionHandler.ClaimCategoryGetFromSession(FunctionCode)

                            If Not udtSchemeClaim Is Nothing Then
                                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                                If udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.RVP) OrElse udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.HSIVSS) Then
                                    If Not udtClaimCategory Is Nothing Then
                                        ' Generate Dynamic Control
                                        Me.Step2aBuildInputEHSClaimControl()
                                    End If
                                Else
                                    'HCVS, CIVSS
                                    Me.Step2aBuildInputEHSClaimControl()
                                End If
                                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
                            End If

                            'Case ActiveViewIndex.PerConditions
                            '    Me.SetupPerConditons(Nothing)

                        Case ActiveViewIndex.VaccinationRecord
                            Me.SetupVaccinationRecord()

                            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                        Case ActiveViewIndex.PrintOption
                            ' Generate Dynamic Control
                            Me.Step2aBuildInputEHSClaimControl()
                            'CRE13-019-02 Extend HCVS to China [End][Winnie]
                    End Select
                Else
                    Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SelectPractice

                End If

            End If

            ' Translation 
            RenderControlText()

        End Sub

        Protected Sub MasterPage_MenuChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Me.SessionHandler.EHSClaimSessionRemove(FunctionCode)
        End Sub

        Protected Sub mvEHSClaim_ActiveViewChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mvEHSClaim.ActiveViewChanged
            Dim masterPage As ClaimVoucherMaster = CType(Me.Master, ClaimVoucherMaster)
            ' Set Step (Handle Change step)
            CType(Me.Master.FindControl(ClaimVoucherMaster.ControlName.LblClaimVoucherStep), Label).Text = Me.GetStepText()

            ' Construct the Menu
            Me.BuildMenuItem(masterPage)

            ' Hide Message Box
            ' Me.HideMessageBox()
            Me.ClearMessageBox()

            Dim udtEHSAccount As EHSAccountModel = SessionHandler.EHSAccountGetFromSession(FunctionCode)
            Select Case Me.mvEHSClaim.ActiveViewIndex
                Case ActiveViewIndex.Step1
                    If hfEHSClaimTokenNum.Value Is Nothing OrElse hfEHSClaimTokenNum.Value.Equals(String.Empty) Then
                        Me.EHSClaimTokenNumAssign(Me.hfEHSClaimTokenNum, FunctionCode)
                    End If

                    Me.SetupStep1(True, True)
                    Me.ResetStep1()

                Case ActiveViewIndex.Step2a
                    ' Hide Error Message
                    Me.ClearWarningRules(MyBase.SessionHandler.EligibleResultGetFromSession())

                    Me.udcStep2aInputEHSClaim.ClearErrorMessage()
                    Me.lblStep2aServiceDateError.Visible = False
                    Me.SetupStep2a(udtEHSAccount, True, False)

                Case ActiveViewIndex.Step2b
                    Me.SetupStep2b(udtEHSAccount, True)
                    ' Generate Dynamic Control
                    Me.Step2aBuildInputEHSClaimControl()

                Case ActiveViewIndex.Step3
                    Me.SetupStep3(udtEHSAccount, True)

                Case ActiveViewIndex.SelectPractice
                    ' Check Have move than 1 Practice, If only 1 Practice, Auto Select the Practice and move to Select Scheme
                    Dim udtAvailablePractice As BLL.PracticeDisplayModelCollection = GetAvailablePractice()
                    Me.StepSelectPracticeClear()
                    If udtAvailablePractice Is Nothing OrElse udtAvailablePractice.Count = 0 Then
                        ' No Practice Available
                        Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.InternalError
                    ElseIf udtAvailablePractice.Count = 1 Then
                        ' Only 1 Practice, Auto Select
                        SessionHandler.PracticeDisplaySaveToSession(udtAvailablePractice(0), FunctionCode)
                        SessionHandler.PracticeDisplayListSaveToSession(udtAvailablePractice)
                        Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SelectScheme
                    Else
                        ' More than 1 Practice, Let user to Choose
                        Me.SetupSelectPractice()
                    End If
                    ' Generate Dynamic Control
                    Me.Step2aBuildInputEHSClaimControl()

                Case ActiveViewIndex.SelectScheme
                    ' Check Have move than 1 Scheme, If only 1 Scheme, Auto Select the scheme and move to Select DocType
                    ' CRE13-001 - EHAPP [Start][Tommy L]
                    ' -------------------------------------------------------------------------------------
                    'Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = GetAvailableScheme()
                    Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = GetAvailableScheme().FilterByTextOnlyAvailable(True)
                    ' CRE13-001 - EHAPP [End][Tommy L]
                    Dim udtSchemeClaim As SchemeClaimModel
                    Dim udtClaimCategory As ClaimCategoryModel

                    Me.StepSelectSchemeClear()
                    If udtSchemeClaimModelCollection Is Nothing OrElse udtSchemeClaimModelCollection.Count = 0 Then
                        ' No Scheme Available
                        Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.InternalError

                    ElseIf udtSchemeClaimModelCollection.Count = 1 Then
                        ' Only 1 Scheme, Auto Select
                        Dim udtSchemeClaimModel As SchemeClaimModel = udtSchemeClaimModelCollection(0)
                        SessionHandler.SchemeSelectedSaveToSession(udtSchemeClaimModel, FunctionCode)
                        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                        Dim udtSchemeClaimBLL As SchemeClaimBLL = New SchemeClaimBLL()
                        Dim udtSelectedPracticeDisplay As PracticeDisplayModel = SessionHandler.PracticeDisplayGetFromSession(FunctionCode)
                        Dim udtConvertedSchemeCode As String = udtSchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(udtSchemeClaimModel.SchemeCode)
                        SessionHandler.NonClinicSettingSaveToSession(SessionSP.PracticeList(udtSelectedPracticeDisplay.PracticeID).PracticeSchemeInfoList.Filter(udtConvertedSchemeCode).IsNonClinic, FunctionCode)
                        'CRE16-002 (Revamp VSS) [End][Chris YIM]

                        ' Handle Retain the Selected Document Type 
                        Dim strSessionDocumentType As String = SessionHandler.DocumentTypeSelectedGetFromSession(FunctionCode)
                        If String.IsNullOrEmpty(strSessionDocumentType) OrElse Not Me.IsSchemeAvailableInDocumentType(udtSchemeClaimModel, strSessionDocumentType) Then
                            ' default major doc
                            Dim strDefaultDocTypeCode As String = GetDefaultDocumnetTypeByScheme(udtSchemeClaimModel.SchemeCode.Trim())

                            If String.IsNullOrEmpty(strDefaultDocTypeCode) Then
                                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SelectDocType
                            Else
                                ' Default the major Document Type, Auto Select it and go to Step1
                                SessionHandler.DocumentTypeSelectedSaveToSession(strDefaultDocTypeCode, FunctionCode)

                                If Not SessionHandler.EHSAccountGetFromSession(FunctionCode) Is Nothing Then
                                    ' Account exists, return to enter claim detail
                                    Me.SetStep2aInputEHSClaimControlReadFromSession(True)
                                    Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step2a
                                Else
                                    Me.Step1Clear(False)
                                    Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
                                End If

                            End If
                        Else
                            If Not SessionHandler.EHSAccountGetFromSession(FunctionCode) Is Nothing Then
                                ' Account exists, return to enter claim detail
                                Me.SetStep2aInputEHSClaimControlReadFromSession(True)
                                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step2a
                            Else
                                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
                            End If
                        End If
                    Else
                        ' More than 1 Scheme, Let user to Choose
                        Me.SetupSelectScheme(True)
                    End If

                    udtClaimCategory = MyBase.SessionHandler.ClaimCategoryGetFromSession(FunctionCode)
                    udtSchemeClaim = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode)

                    If Not udtSchemeClaim Is Nothing Then
                        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                        'If claim category is nothing -> this page is come from claim for same patient -> claim for same patient delete Claim category
                        If udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.RVP) OrElse udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.HSIVSS) Then
                            If Not udtClaimCategory Is Nothing Then
                                ' Generate Dynamic Control
                                Me.Step2aBuildInputEHSClaimControl()
                            End If
                        Else
                            'HCVS, CIVSS
                            Me.Step2aBuildInputEHSClaimControl()
                        End If
                        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
                    End If

                Case ActiveViewIndex.SelectDocType
                    ' Check Have more than 1 Document Type, If only 1 Document Type, Auto Select the Document Type and move to Step1 (Search Account)
                    Dim udtSchemeDocTypeModelCollection As SchemeDocTypeModelCollection = GetAvailableDocumentType()
                    If udtSchemeDocTypeModelCollection Is Nothing OrElse udtSchemeDocTypeModelCollection.Count = 0 Then
                        ' No Document Type available
                        Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.InternalError
                    ElseIf udtSchemeDocTypeModelCollection.Count = 1 Then
                        ' Only 1 Document Type, Auto Select
                        SessionHandler.DocumentTypeSelectedSaveToSession(udtSchemeDocTypeModelCollection(0).DocCode.Trim(), FunctionCode)
                        Me.Step1Clear(False)
                        Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
                    Else
                        ' Move than 1 Document Type, Let User to Choose
                        Me.SetupSelectDocumentType()
                    End If

                Case ActiveViewIndex.InternalError
                    Me.SetupInternalError()

                Case ActiveViewIndex.ConfirmBox
                    SetupConfirmBox()
                    ' Generate Dynamic Control
                    Me.Step2aBuildInputEHSClaimControl()

                Case ActiveViewIndex.InputTip
                    ' Generate Dynamic Control
                    Me.Step2aBuildInputEHSClaimControl()

                Case ActiveViewIndex.Remark
                    ' Generate Dynamic Control
                    Me.Step2aBuildInputEHSClaimControl()

                Case ActiveViewIndex.ReasonForVisit
                    Me.SetupReasonForVisit()

                    Me.Step2aBuildInputEHSClaimControl()

                Case ActiveViewIndex.Category
                    Dim udtClaimCategory As ClaimCategoryModel = MyBase.SessionHandler.ClaimCategoryGetFromSession(FunctionCode)
                    If udtClaimCategory Is Nothing Then
                        Me.SetupCategory(Nothing)
                    Else
                        Me.SetupCategory(udtClaimCategory.CategoryCode)
                    End If

                Case ActiveViewIndex.VaccinationRecord
                    Me.SetupVaccinationRecord()

                    'Case ActiveViewIndex.PerConditions
                    '    Me.SetupPerConditons(Me.rbPreConditions.Attributes(SelectedPerConditionValueAttribute))
            End Select

            ' Translation 
            RenderControlText()

        End Sub

        Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)

            ' handle if page is refreshed
            If _blnIsRequireHandlePageRefresh = True Then
                HandlePageRefreshed()
            End If

            MyBase.OnPreRender(e)
        End Sub

        '==================================================================== Code for SmartID ============================================================================
        Private Function ReadSmartID(ByVal udtSmartIDContent As BLL.SmartIDContentModel) As Boolean
            If IsNothing(udtSmartIDContent) OrElse udtSmartIDContent.IsReadSmartID = False OrElse udtSmartIDContent.IsEndOfReadSmartID Then Return False

            Dim blnIsReadingSmartID As Boolean = False
            Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode)

            Dim ideasBLL As BLL.IdeasBLL = New BLL.IdeasBLL
            Dim strIdeasVersion As String = ideasBLL.ConvertIdeasVersion(udtSmartIDContent.IdeasVersion)

            'Write Start Audit log
            EHSClaimBasePage.AuditLogRedirectFormIDEAS(_udtAuditLogEntry, udtSchemeClaim.SchemeCode, strIdeasVersion)

            MyBase.EHSClaimTokenNumAssign(Me.hfEHSClaimTokenNum, FunctionCode)

            blnIsReadingSmartID = True
            udtSmartIDContent.IsEndOfReadSmartID = True
            '--------------------------------------------------------------------------------------------------------------------------------------------------
            ' Smart ID Form Ideas
            '--------------------------------------------------------------------------------------------------------------------------------------------------                
            Dim ideasHelper As IdeasRM.IHelper = IdeasRM.HelpFactory.createHelper()

            '--------------------------------------------------------------------------------------------------------------------------------------------------
            ' Get CFD
            '--------------------------------------------------------------------------------------------------------------------------------------------------
            Dim udtAuditLogEntry_GetCFD As AuditLogEntry = _udtAuditLogEntry

            ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Dim ideasSamlResponse As IdeasRM.IdeasResponse = Nothing
            Dim strArtifact As String = String.Empty

            If udtSmartIDContent.IdeasVersion = BLL.IdeasBLL.EnumIdeasVersion.Combo Or _
                udtSmartIDContent.IdeasVersion = BLL.IdeasBLL.EnumIdeasVersion.ComboGender Then

                strArtifact = udtSmartIDContent.Artifact
                ideasSamlResponse = udtSmartIDContent.IdeasSamlResponse
            Else
                strArtifact = ideasBLL.Artifact
                ideasSamlResponse = ideasHelper.getCardFaceData(udtSmartIDContent.TokenResponse, strArtifact, strIdeasVersion)

            End If

            EHSClaimBasePage.AuditLogGetCFD(udtAuditLogEntry_GetCFD, strArtifact)

            ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

            Dim udtPersonalInfoSmartID As EHSAccountModel.EHSPersonalInformationModel
            Dim isValid As Boolean = True

            Dim udtSystemMessage As SystemMessage = Nothing

            If strArtifact Is Nothing OrElse strArtifact = String.Empty Then
                '------------------------------------------------------------------------------------------------------
                'Error Handling : Err100 - 113
                '------------------------------------------------------------------------------------------------------
                If Not Request.QueryString("status") Is Nothing Then
                    Dim strErrorCode As String = Request.QueryString("status").Trim()
                    Dim strErrorMsg As String = IdeasRM.ErrorMessageMapper.MapMAStatus(strErrorCode)
                    If Not strErrorMsg Is Nothing Then

                        Me.Clear()
                        Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
                        Me.udcMsgBoxErr.AddMessageDesc(FunctionCode, strErrorCode, strErrorMsg)

                        EHSClaimBasePage.AuditLogGetCFDFail(udtAuditLogEntry_GetCFD, strArtifact, strErrorCode, strErrorMsg, strIdeasVersion)

                        Me.udcMsgBoxErr.BuildMessageDescBox("SmartIDActionFail", udtAuditLogEntry_GetCFD, Common.Component.LogID.LOG00063, "Get CFD Fail")
                        isValid = False
                    End If
                End If
            End If

            If isValid Then

                If ideasSamlResponse.StatusCode.Equals("samlp:Success") Then
                    EHSClaimBasePage.AuditLogGetCFDComplete(udtAuditLogEntry_GetCFD, strArtifact)

                    Dim udtEHSAccountExist As EHSAccountModel = Nothing
                    Dim blnNotMatchAccountExist As Boolean = False
                    Dim blnExceedDocTypeLimit As Boolean = False
                    Dim udtEligibleResult As EligibleResult = Nothing
                    Dim goToCreation As Boolean = True
                    Dim strError As String = String.Empty

                    Try
                        If udtSmartIDContent.IsDemonVersion Then

                            udtSmartIDContent.EHSAccount = SmartIDDummyCase.GetDummyEHSAccount(udtSchemeClaim, udtSmartIDContent.IdeasVersion)

                            udtSmartIDContent.EHSAccount.EHSPersonalInformationList(0).CName = BLL.VoucherAccountMaintenanceBLL.GetCName(udtSmartIDContent.EHSAccount.EHSPersonalInformationList(0))

                        Else
                            Dim udtCFD As IdeasRM.CardFaceData = ideasSamlResponse.CardFaceDate()

                            If IsNothing(udtCFD) Then
                                strError = "ideasSamlResponse.CardFaceDate() is nothing"
                            End If

                            ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
                            ' ---------------------------------------------------------------------------------------------------------
                            udtSmartIDContent.EHSAccount = ideasBLL.GetCardFaceDataEHSAccount(udtCFD, udtSchemeClaim, FunctionCode, udtSmartIDContent)
                            ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

                        End If
                    Catch ex As Exception
                        udtSmartIDContent.EHSAccount = Nothing
                        strError = ex.Message
                    End Try

                    Dim udtAuditlogEntry_Search As AuditLogEntry = _udtAuditLogEntry
                    Dim strHKICNo As String = String.Empty

                    If Not udtSmartIDContent.EHSAccount Is Nothing Then
                        strHKICNo = udtSmartIDContent.EHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).IdentityNum.Trim
                    End If

                    EHSClaimBasePage.AuditLogSearchNvaliatedACwithCFD(udtAuditlogEntry_Search, udtSchemeClaim.SchemeCode, strHKICNo, strError, strIdeasVersion, "Claim")

                    If Not udtSmartIDContent.EHSAccount Is Nothing Then
                        udtPersonalInfoSmartID = udtSmartIDContent.EHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC)

                        '------------------------------------------------------------------------------------------------------
                        'Card Face Data Validation
                        '------------------------------------------------------------------------------------------------------
                        udtSystemMessage = EHSClaimBasePage.SmartIDCardFaceDataValidation(udtPersonalInfoSmartID)
                        If Not udtSystemMessage Is Nothing Then
                            isValid = False
                            If Not udtPersonalInfoSmartID.IdentityNum Is Nothing Then udtAuditlogEntry_Search.AddDescripton("HKID", udtPersonalInfoSmartID.IdentityNum)
                            If udtPersonalInfoSmartID.DateofIssue.HasValue Then udtAuditlogEntry_Search.AddDescripton("DOI", udtPersonalInfoSmartID.DateofIssue)
                            udtAuditlogEntry_Search.AddDescripton("DOB", udtPersonalInfoSmartID.DOB)

                            Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
                        End If

                        If isValid Then

                            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                            ' ----------------------------------------------------------
                            ' ----------------------------------------------
                            ' 1. Search account in EHS 
                            ' ----------------------------------------------
                            udtSystemMessage = Me._udtEHSClaimBLL.SearchEHSAccountSmartID(udtSchemeClaim.SchemeCode.Trim(), DocTypeModel.DocTypeCode.HKIC, udtPersonalInfoSmartID.IdentityNum, _
                                            Me._udtFormatter.formatDOB(udtPersonalInfoSmartID.DOB, udtPersonalInfoSmartID.ExactDOB, Common.Component.CultureLanguage.English, Nothing, Nothing), _
                                            udtEHSAccountExist, udtSmartIDContent.EHSAccount, udtSmartIDContent.SmartIDReadStatus, udtEligibleResult, blnNotMatchAccountExist, blnExceedDocTypeLimit, _
                                            FunctionCode, True, ClaimMode.All)

                            ' ----------------------------------------------
                            ' 2. Call OCSSS to check HKIC if input is shown
                            ' ----------------------------------------------
                            If udtSystemMessage Is Nothing Then
                                ' HKIC must be formated in 9 characters e.g. " A1234567" or "CD1234567"
                                If SessionHandler.UIDisplayHKICSymbolGetFormSession(FunctionCode) Then

                                    ' INT18-XXX (Refine auditlog) [Start][Chris YIM]
                                    ' --------------------------------------------------------------------------------------
                                    'Log Enter Info
                                    EHSClaimBasePage.AuditLogSearchOCSSSStart(_udtAuditLogEntry, udtPersonalInfoSmartID.DocCode, _
                                                                               SessionHandler.HKICSymbolGetFormSession(FunctionCode), udtPersonalInfoSmartID.IdentityNum)

                                    udtSystemMessage = CheckHKIDByOCSSS(udtPersonalInfoSmartID.IdentityNum, _udtSP.SPID, udtSchemeClaim.SchemeCode.Trim())

                                    If udtSystemMessage Is Nothing Then
                                        EHSClaimBasePage.AuditLogSearchOCSSSEnd(New AuditLogEntry(FunctionCode, Me), True)
                                    Else
                                        EHSClaimBasePage.AuditLogSearchOCSSSEnd(New AuditLogEntry(FunctionCode, Me), False)
                                    End If

                                    ' INT18-XXX (Refine auditlog) [End][Chris YIM]
                                Else
                                    SessionHandler.HKICSymbolRemoveFromSession(FunctionCode)
                                End If
                            End If

                            ' ----------------------------------------------
                            ' Search Account Error Issue
                            ' ----------------------------------------------
                            If Not udtSystemMessage Is Nothing Then
                                isValid = False

                                Select Case udtSystemMessage.MessageCode
                                    Case "00141", "00142"
                                        Me.udcStep1ClaimSearch.SetHKICNoError(True)
                                End Select

                                Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
                            Else
                                'Validation Success

                                'Store residential status in model
                                ' INT18-0018 (Fix read smart IC with HKBC account) [Start][Koala]
                                If udtEHSAccountExist.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC) IsNot Nothing Then
                                    udtEHSAccountExist.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).HKICSymbol = SessionHandler.HKICSymbolGetFormSession(FunctionCode)
                                End If
                                ' INT18-0018 (Fix read smart IC with HKBC account) [End][Koala]
                            End If
                            ' CRE17-010 (OCSSS integration) [End][Chris YIM]

                        End If

                        If isValid Then
                            Dim strRuleResultKey As String = String.Empty

                            If udtEligibleResult Is Nothing Then
                                MyBase.SessionHandler.EligibleResultRemoveFromSession()
                            Else
                                Dim udtRuleResults As RuleResultCollection = New RuleResultCollection()

                                udtEligibleResult.PromptConfirmed = True
                                'Key = 1_G0002 -> not need prompt confirm popup dox -> reminder in step2a
                                strRuleResultKey = Me.RuleResultKey(ActiveViewIndex.Step1, udtEligibleResult.RuleType)

                                udtRuleResults.Add(strRuleResultKey, udtEligibleResult)
                                MyBase.SessionHandler.EligibleResultSaveToSession(udtRuleResults)
                            End If

                            udtEHSAccountExist.SetSearchDocCode(DocTypeModel.DocTypeCode.HKIC)

                            'Only one case go to Claim directly -> Account validated && Search DocCode = PersonalInfo DocCode 
                            'udtSmartIDContent.SmartIDReadStatus = BLL.SmartIDHandler.CheckSmartIDCardStatus(udtSmartIDContent.EHSAccount, udtEHSAccountExist)
                            MyBase.SessionHandler.SmartIDContentSaveToSession(FunctionCode, udtSmartIDContent)
                            MyBase.SessionHandler.NotMatchAccountExistSaveToSession(blnNotMatchAccountExist)
                            MyBase.SessionHandler.ExceedDocTypeLimitSaveToSession(blnExceedDocTypeLimit)
                            MyBase.SessionHandler.EHSAccountSaveToSession(udtEHSAccountExist, FunctionCode)

                            Select Case udtSmartIDContent.SmartIDReadStatus
                                Case BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_SameDetail

                                    goToCreation = False

                                Case BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_NotCreateBySmartID_SameDOB_NoCCCode

                                    goToCreation = False
                                    udtPersonalInfoSmartID.VoucherAccID = udtEHSAccountExist.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).VoucherAccID
                                    udtPersonalInfoSmartID.Gender = udtEHSAccountExist.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).Gender
                                    udtPersonalInfoSmartID.TSMP = udtEHSAccountExist.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).TSMP
                                    Try
                                        Me._udtEHSAccountBll.UpdateEHSAccountNameBySmartIC(udtPersonalInfoSmartID, Me._udtSP.SPID)
                                    Catch eSQL As SqlClient.SqlException
                                        If eSQL.Number = 50000 Then
                                            udtSystemMessage = New Common.ComObject.SystemMessage("990001", SeverityCode.SEVD, eSQL.Message)
                                            Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
                                        Else
                                            Throw eSQL
                                        End If
                                    End Try

                                    If udtSystemMessage Is Nothing Then
                                        'udtEHSAccountExist = Me._udtEHSAccountBll.LoadEHSAccountByVRID(udtPersonalInfoSmartID.VoucherAccID)
                                        'udtEHSAccountExist.SetSearchDocCode(udtEHSAccountExist.EHSPersonalInformationList(0).DocCode)
                                        udtEHSAccountExist = Me._udtEHSAccountBll.LoadEHSAccountByIdentity(udtEHSAccountExist.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).IdentityNum, DocTypeModel.DocTypeCode.HKIC)
                                        udtEHSAccountExist.SetSearchDocCode(DocTypeModel.DocTypeCode.HKIC)

                                        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                                        ' ----------------------------------------------------------
                                        udtEHSAccountExist.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).HKICSymbol = SessionHandler.HKICSymbolGetFormSession(FunctionCode)
                                        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

                                        MyBase.SessionHandler.EHSAccountSaveToSession(udtEHSAccountExist, FunctionCode)
                                    End If
                            End Select
                        Else
                            goToCreation = False
                        End If

                        If goToCreation Then

                            EHSClaimBasePage.AuditLogSearchNvaliatedACwithCFDComplete(udtAuditlogEntry_Search, udtSchemeClaim.SchemeCode, udtSmartIDContent, True)
                            MyBase.SessionHandler.AccountCreationComeFromClaimSaveToSession(True)

                            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

                            RedirectHandler.ToURL("EHSAccountCreationV1.aspx")

                            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End
                        Else

                            If Not udtSystemMessage Is Nothing Then
                                '---------------------------------------------------------------------------------------------------------------
                                ' Block Case 
                                '---------------------------------------------------------------------------------------------------------------
                                Me.Clear()
                                'Me.udcStep1DocumentTypeRadioButtonGroup.SelectedValue = DocTypeModel.DocTypeCode.HKIC
                                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
                                Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
                            Else
                                ' To Handle Concurrent Browser:
                                EHSClaimBasePage.AuditLogSearchNvaliatedACwithCFDComplete(udtAuditlogEntry_Search, udtSchemeClaim.SchemeCode, udtSmartIDContent, False)

                                ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
                                ' --------------------------------------------------------------------------------------
                                Select Case udtSchemeClaim.ControlType
                                    Case SchemeClaimModel.EnumControlType.CIVSS, SchemeClaimModel.EnumControlType.EVSS, SchemeClaimModel.EnumControlType.HSIVSS, _
                                         SchemeClaimModel.EnumControlType.RVP, SchemeClaimModel.EnumControlType.PIDVSS, SchemeClaimModel.EnumControlType.VSS, _
                                         SchemeClaimModel.EnumControlType.ENHVSSO, SchemeClaimModel.EnumControlType.PPP

                                        Me.GoToVersion2()

                                    Case Else
                                        Me.ResetStep2aServiceDate()
                                        Me.SetStep2aInputEHSClaimControlReadFromSession(True)
                                        Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step2a
                                        EHSClaimBasePage.AuditLogEnterClaimDetailLoaded(_udtAuditLogEntry)

                                        ' Default service date in here:
                                        Dim udtSubPlatformBLL As New SubPlatformBLL

                                        txtStep2aServiceDate.Text = DateTime.Today.ToString(Me._udtFormatter.EnterDateFormat(udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform)))

                                End Select
                                ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]
                            End If
                        End If
                    Else
                        '---------------------------------------------------------------------------------------------------------------
                        ' udtSmartIDContent.EHSAccount is nothing, crad face data may not be able to return 
                        '---------------------------------------------------------------------------------------------------------------
                        Me.Clear()
                        Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
                        Me.udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00253"))
                    End If

                    Me.udcMsgBoxErr.BuildMessageBox(_strValidationFail, udtAuditlogEntry_Search, Common.Component.LogID.LOG00052, "Search & validate account with CFD Fail")
                Else

                    '---------------------------------------------------------------------------------------------------------------
                    ' ideasSamlResponse.StatusCode is not "samlp:Success"
                    '---------------------------------------------------------------------------------------------------------------
                    Me.Clear()
                    Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
                    Me.udcMsgBoxErr.AddMessageDesc(FunctionCode, ideasSamlResponse.StatusMessage, ideasSamlResponse.StatusDetail)

                    'Write End Audit log
                    EHSClaimBasePage.AuditLogGetCFDFail(udtAuditLogEntry_GetCFD, strArtifact, ideasSamlResponse.StatusMessage, ideasSamlResponse.StatusDetail, strIdeasVersion)

                    Me.udcMsgBoxErr.BuildMessageDescBox("SmartIDActionFail", udtAuditLogEntry_GetCFD, Common.Component.LogID.LOG00063, "Get CFD Fail")

                End If
            End If

            Return blnIsReadingSmartID
        End Function
        '==================================================================================================================================================================

#Region "Language"

        Private Sub RenderControlText()

            Select Case Me.mvEHSClaim.ActiveViewIndex
                Case ActiveViewIndex.Step1
                    'lblStep1SearchAccountText.Text = Me.GetGlobalResourceObject("Text", "SearchAccount")
                    lblStep1SearchAccountText.Text = Me.GetGlobalResourceObject("Text", "SearchTempVRAcct")
                    lblStep1PracticeText.Text = Me.GetGlobalResourceObject("Text", "Practice")
                    lblStep1SchemeText.Text = Me.GetGlobalResourceObject("Text", "Scheme")
                    lblStep1DocTypeText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")

                    ' btnStep1ChangePractice.Text = Me.GetGlobalResourceObject("AlternateText", "ChangePracticeBtn")
                    btnStep1ChangePractice.Text = Me.GetGlobalResourceObject("AlternateText", "Change")
                    ' btnStep1ChangeScheme.Text = Me.GetGlobalResourceObject("AlternateText", "ChangeSchemeBtn")
                    btnStep1ChangeScheme.Text = Me.GetGlobalResourceObject("AlternateText", "Change")
                    ' btnStep1ChangeDocType.Text = Me.GetGlobalResourceObject("AlternateText", "ChangeDocumentTypeBtn")
                    btnStep1ChangeDocType.Text = Me.GetGlobalResourceObject("AlternateText", "Change")
                    btnStep1GO.Text = Me.GetGlobalResourceObject("AlternateText", "SearchBtn")

                Case ActiveViewIndex.SelectPractice
                    lblStepSelectPracticePracticeText.Text = Me.GetGlobalResourceObject("Text", "SelectPractice")

                    btnStepSelectPracticeGO.Text = Me.GetGlobalResourceObject("AlternateText", "SelectBtn")

                Case ActiveViewIndex.SelectScheme
                    lblStepSelectSchemePracticeText.Text = Me.GetGlobalResourceObject("Text", "Practice")
                    lblStepSelectSchemeScheme.Text = Me.GetGlobalResourceObject("Text", "SelectScheme")

                    ' btnStepSelectSchemeChangePractice.Text = Me.GetGlobalResourceObject("AlternateText", "ChangePracticeBtn")
                    btnStepSelectSchemeChangePractice.Text = Me.GetGlobalResourceObject("AlternateText", "Change")
                    btnStepSelectSchemeGO.Text = Me.GetGlobalResourceObject("AlternateText", "SelectBtn")

                Case ActiveViewIndex.SelectDocType
                    lblStepSelectDocTypePracticeText.Text = Me.GetGlobalResourceObject("Text", "Practice")
                    lblStepSelectDocTypeSchemeText.Text = Me.GetGlobalResourceObject("Text", "Scheme")
                    lblStepSelectDocTypeDocTypeText.Text = Me.GetGlobalResourceObject("Text", "SelectDocType")

                    ' btnStepSelectDocTypeChangePractice.Text = Me.GetGlobalResourceObject("AlternateText", "ChangePracticeBtn")
                    btnStepSelectDocTypeChangePractice.Text = Me.GetGlobalResourceObject("AlternateText", "Change")
                    ' btnStepSelectDocTypeChangeScheme.Text = Me.GetGlobalResourceObject("AlternateText", "ChangeSchemeBtn")
                    btnStepSelectDocTypeChangeScheme.Text = Me.GetGlobalResourceObject("AlternateText", "Change")
                    btnStepSelectDocTypeSelect.Text = Me.GetGlobalResourceObject("AlternateText", "SelectBtn")

            End Select

        End Sub

#End Region

#Region "Step of Step 1 Select Residential Status"
        Private Sub udcClaimSearch_HKICSymbolListClick(ByVal sender As System.Object, ByVal e As EventArgs) Handles udcStep1ClaimSearch.HKICSymbolListClick
            SessionHandler.HKICSymbolSaveToSession(FunctionCode, Me.udcStep1ClaimSearch.HKICSymbol)

            ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Dim strIDEASComboClientInstalled As String = IIf(SessionHandler.IDEASComboClientGetFormSession() Is Nothing, YesNo.No, SessionHandler.IDEASComboClientGetFormSession())
            Dim blnIDEASComboClientInstalled As Boolean = IIf(strIDEASComboClientInstalled = YesNo.Yes, True, False)

            If Not btnStep1GO.Enabled Then
                btnStep1GO.Enabled = True
            End If

            If blnIDEASComboClientInstalled Then
                If Not btnStep1ReadNewSmartIDCombo.Enabled Then
                    btnStep1ReadNewSmartIDCombo.Enabled = True
                End If

            Else
                If Not btnStep1ReadOldSmartID.Enabled Then
                    btnStep1ReadOldSmartID.Enabled = True
                End If

                If Not btnStep1ReadNewSmartID.Enabled Then
                    btnStep1ReadNewSmartID.Enabled = True
                End If

            End If
            ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

        End Sub
#End Region

#Region "Step of Step 1 Search account"

        'Event
        Protected Sub btnStep1GO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStep1GO.Click
            'If (MyBase.IsPageRefreshed AndAlso Not SessionHandler.EHSAccountGetFromSession(FunctionCode) Is Nothing) Then
            '    _blnIsRequireHandlePageRefresh = True
            '    Return
            'End If

            ' To Handle Concurrent Browser
            If Not Me.EHSClaimTokenNumValidation(hfEHSClaimTokenNum.Value, FunctionCode) Then
                _blnIsConcurrentBrowserDetected = True
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.InternalError
                Return
            End If

            Dim udtEHSAccount As EHSAccountModel = Nothing
            Dim udtEligibleResult As EligibleResult = Nothing
            Dim udtSchemeClaim As SchemeClaimModel = SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
            Dim strSearchDocCode As String = SessionHandler.DocumentTypeSelectedGetFromSession(FunctionCode)
            Dim strSchemeCode As String = String.Empty
            If Not udtSchemeClaim Is Nothing Then
                strSchemeCode = udtSchemeClaim.SchemeCode.Trim()
            End If

            ' CRE11-004
            ' Set doc code to make claim search control can determine which identity no user entered
            Me.udcStep1ClaimSearch.SetProperty(strSearchDocCode)
            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            EHSClaimBasePage.AuditLogSearchAccountStart(_udtAuditLogEntry, strSchemeCode, strSearchDocCode, SessionHandler.HKICSymbolGetFormSession(FunctionCode), _
                                                        (New Formatter).formatDocumentIdentityNumber(strSearchDocCode, Me.udcStep1ClaimSearch.IdentityNo), _
                                                        Me.udcStep1ClaimSearch.DOB, "Claim")

            Me.SessionHandler.OCSSSRefStatusRemoveFromSession(FunctionCode)
            ' CRE17-010 (OCSSS integration) [End][Chris YIM]

            ' Validate user input
            Dim udtSearchAccountStatus As New BLL.EHSClaimBLL.SearchAccountStatus

            Dim isValid As Boolean = Me.Step1SearchValdiation(_udtAuditLogEntry)
            If isValid Then
                ' Handle Different Document Type 
                Select Case strSearchDocCode
                    Case DocTypeModel.DocTypeCode.HKIC, _
                        DocTypeModel.DocTypeCode.HKBC, _
                        DocTypeModel.DocTypeCode.DI, _
                        DocTypeModel.DocTypeCode.REPMT, _
                        DocTypeModel.DocTypeCode.ID235B, _
                        DocTypeModel.DocTypeCode.VISA, _
                        DocTypeModel.DocTypeCode.ADOPC

                        ' Method Out some objects... validation put here...
                        ' Validation Searh HKIC case fields 
                        If isValid Then

                            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                            ' ----------------------------------------------------------
                            Dim strHKICSymbol As String = String.Empty
                            If strSearchDocCode = DocTypeModel.DocTypeCode.HKIC Then
                                strHKICSymbol = Me.udcStep1ClaimSearch.HKICSymbol
                            End If
                            ' CRE17-010 (OCSSS integration) [End][Chris YIM]
                            Dim strIdentityNo As String = Me.udcStep1ClaimSearch.IdentityNo
                            Dim strIdentityNoPrefix As String = String.Empty
                            Dim strDOB As String = Me._udtFormatter.formatInputDate(Me.udcStep1ClaimSearch.DOB)


                            If Not Me.udcStep1ClaimSearch.IdentityNoPrefix Is Nothing Then
                                strIdentityNoPrefix = Me.udcStep1ClaimSearch.IdentityNoPrefix.ToUpper()
                            End If

                            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                            ' ----------------------------------------------------------
                            'Log Enter Info
                            EHSClaimBasePage.AuditLogSearchAccountInfo(_udtAuditLogEntry, strSchemeCode, strSearchDocCode, _
                                                                       SessionHandler.HKICSymbolGetFormSession(FunctionCode), strIdentityNo, strDOB, _
                                                                       Nothing, Nothing)

                            strIdentityNo = strIdentityNo.Replace("(", String.Empty).Replace(")", String.Empty)

                            Dim udtSystemMessage As SystemMessage = Nothing
                            ' ----------------------------------------------
                            ' 1. Search account in EHS 
                            ' ----------------------------------------------
                            If strSearchDocCode = DocTypeModel.DocTypeCode.ADOPC Then
                                udtSystemMessage = _udtEHSClaimBLL.SearchEHSAccount(strSchemeCode, strSearchDocCode, strIdentityNo, strDOB, _
                                    udtEHSAccount, udtEligibleResult, udtSearchAccountStatus, Nothing, strIdentityNoPrefix, FunctionCode, ClaimMode.All)
                            Else
                                udtSystemMessage = Me._udtEHSClaimBLL.SearchEHSAccount(strSchemeCode, strSearchDocCode, strIdentityNo, strDOB, _
                                    udtEHSAccount, udtEligibleResult, udtSearchAccountStatus, Nothing, String.Empty, FunctionCode, ClaimMode.All)
                            End If

                            ' INT18-XXX (Refine auditlog) [Start][Chris YIM]
                            ' --------------------------------------------------------------------------------------
                            'Log Enter Info Complete
                            EHSClaimBasePage.AuditLogSearchAccountInfoEnd(_udtAuditLogEntry, isValid)
                            ' INT18-XXX (Refine auditlog) [End][Chris YIM]

                            ' ----------------------------------------------
                            ' 2. Call OCSSS to check HKIC if input is shown
                            ' ----------------------------------------------
                            If udtSystemMessage Is Nothing Then
                                ' HKIC must be formated in 9 characters e.g. " A1234567" or "CD1234567"
                                If Me.udcStep1ClaimSearch.UIDisplayHKICSymbol Then
                                    ' INT18-XXX (Refine auditlog) [Start][Chris YIM]
                                    ' --------------------------------------------------------------------------------------
                                    'Log Enter Info
                                    EHSClaimBasePage.AuditLogSearchOCSSSStart(_udtAuditLogEntry, strSearchDocCode, _
                                                                               SessionHandler.HKICSymbolGetFormSession(FunctionCode), strIdentityNo)

                                    udtSystemMessage = CheckHKIDByOCSSS((New Formatter).formatDocumentIdentityNumber(DocTypeModel.DocTypeCode.HKIC, strIdentityNo), _udtSP.SPID, strSchemeCode)

                                    If udtSystemMessage Is Nothing Then
                                        EHSClaimBasePage.AuditLogSearchOCSSSEnd(_udtAuditLogEntry, True)
                                    Else
                                        EHSClaimBasePage.AuditLogSearchOCSSSEnd(_udtAuditLogEntry, False)
                                    End If

                                    ' INT18-XXX (Refine auditlog) [End][Chris YIM]
                                Else
                                    SessionHandler.HKICSymbolRemoveFromSession(FunctionCode)
                                End If
                            End If

                            ' ----------------------------------------------
                            ' Search account error issue
                            ' ----------------------------------------------
                            If Not udtSystemMessage Is Nothing Then
                                ' Error
                                isValid = False

                                If udtSystemMessage.MessageCode = "00142" Or udtSystemMessage.MessageCode = "00141" Then
                                    Me.udcStep1ClaimSearch.SetSearchShortIdentityNoError(True)
                                ElseIf udtSystemMessage.MessageCode = "00110" Then
                                    Me.udcStep1ClaimSearch.SetSearchShortDOBError(True)
                                End If

                                ' Show Error
                                Me.ShowError(New SystemMessage() {udtSystemMessage}, _udtAuditLogEntry, LogID.LOG00005, "Search Account Failed", _
                                    New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, strSearchDocCode, (New Formatter).formatDocumentIdentityNumber(strSearchDocCode, strIdentityNo)))
                            Else
                                'Validation Success

                                'Store residential status in model
                                If strSearchDocCode = DocTypeModel.DocTypeCode.HKIC Then
                                    If udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC) Is Nothing And _
                                        Not udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKBC) Is Nothing Then

                                        udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKBC).HKICSymbol = String.Empty
                                    Else
                                        udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).HKICSymbol = SessionHandler.HKICSymbolGetFormSession(FunctionCode)
                                    End If
                                End If
                            End If
                            ' CRE17-010 (OCSSS integration) [End][Chris YIM]
                        End If

                    Case DocTypeModel.DocTypeCode.EC
                        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                        ' ----------------------------------------------------------
                        Me.SessionHandler.HKICSymbolRemoveFromSession(FunctionCode)
                        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

                        If isValid Then
                            Dim strIdentityNo As String = Me.udcStep1ClaimSearch.IdentityNo.Replace("(", String.Empty).Replace(")", String.Empty)
                            Dim strECAge As String = Me.udcStep1ClaimSearch.ECAge()

                            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                            '-----------------------------------------------------------------------------------------
                            'Dim strDOB As String = _udtFormatter.formatDate(Me.udcStep1ClaimSearch.DOB)
                            Dim strDOB As String = _udtFormatter.formatInputDate(Me.udcStep1ClaimSearch.DOB)
                            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

                            'Log Enter Info
                            EHSClaimBasePage.AuditLogSearchAccountInfo(_udtAuditLogEntry, strSchemeCode, strSearchDocCode, Nothing, strIdentityNo, strDOB, strECAge, strDOB)

                            Dim udtSysMsgValidateEC As SystemMessage = Nothing
                            If Me.udcStep1ClaimSearch.ECDOBSelected Then
                                udtSysMsgValidateEC = _udtEHSClaimBLL.SearchEHSAccount(strSchemeCode, strSearchDocCode, strIdentityNo, strDOB, _
                                    udtEHSAccount, udtEligibleResult, udtSearchAccountStatus, Nothing, String.Empty, FunctionCode, ClaimMode.All)

                            Else
                                Dim strDOADay As String = udcStep1ClaimSearch.ECDOADay
                                Dim strDOAMonth As String = udcStep1ClaimSearch.ECDOAMonth
                                Dim strDOAYear As String = udcStep1ClaimSearch.ECDOAYear
                                Dim strDateOfReg = String.Format("{0:00}-{1}-{2}", Convert.ToInt32(strDOADay), strDOAMonth, strDOAYear)
                                Dim dtmDateOfReg As DateTime = CDate(_udtFormatter.convertDate(strDateOfReg, SessionHandler.Language))

                                udtSysMsgValidateEC = _udtEHSClaimBLL.SearchEHSAccount(strSchemeCode, strSearchDocCode, strIdentityNo, _
                                    strECAge, dtmDateOfReg, udtEHSAccount, udtEligibleResult, udtSearchAccountStatus, Nothing, FunctionCode, ClaimMode.All)
                            End If


                            If Not udtSysMsgValidateEC Is Nothing Then
                                ' Error
                                isValid = False

                                If udtSysMsgValidateEC.MessageCode = "00142" Or udtSysMsgValidateEC.MessageCode = "00141" Then
                                    Me.udcStep1ClaimSearch.SetECHKIDError(True)
                                ElseIf udtSysMsgValidateEC.MessageCode = "00110" Then
                                    If Me.udcStep1ClaimSearch.ECDOBSelected Then
                                        Me.udcStep1ClaimSearch.SetECDOBError(True)
                                    Else
                                        Me.udcStep1ClaimSearch.SetECDOAAgeError(True)
                                        Me.udcStep1ClaimSearch.SetECDOAError(True)
                                    End If
                                End If

                                Me.ShowError(New SystemMessage() {udtSysMsgValidateEC}, _udtAuditLogEntry, LogID.LOG00005, "Search Account Failed", _
                                    New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, strSearchDocCode, (New Formatter).formatDocumentIdentityNumber(strSearchDocCode, strIdentityNo)))

                            End If

                        End If
                End Select
            End If

            If isValid Then
                Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = Nothing
                Dim goToCreation As Boolean = True
                Dim blnIsGoToVersion2 As Boolean = False

                If udtEligibleResult Is Nothing Then
                    SessionHandler.EligibleResultRemoveFromSession()
                Else
                    Dim udtRuleResults As RuleResultCollection = New RuleResultCollection()
                    'Key = 1_G0002 -> not need prompt confirm popup dox -> reminder in step2a
                    udtEligibleResult.PromptConfirmed = True
                    udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Step1, udtEligibleResult.RuleType), udtEligibleResult)
                    SessionHandler.EligibleResultSaveToSession(udtRuleResults)
                End If

                SessionHandler.NotMatchAccountExistSaveToSession(udtSearchAccountStatus.NotMatchAccountExist)
                SessionHandler.ExceedDocTypeLimitSaveToSession(udtSearchAccountStatus.ExceedDocTypeLimit)

                udtEHSAccount.SetSearchDocCode(strSearchDocCode)
                SessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctionCode)

                'Only one case go to Claim directly -> Account validated && Search DocCode = PersonalInfo DocCode 
                If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                    udtEHSAccountPersonalInfo = udtEHSAccount.EHSPersonalInformationList.Filter(strSearchDocCode)
                    If Not udtEHSAccountPersonalInfo Is Nothing Then

                        ' go to enter claim detail
                        Me.EHSClaimTokenNumAssign(Me.hfEHSClaimTokenNum, FunctionCode)

                        ' Check to go to Version2

                        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
                        ' --------------------------------------------------------------------------------------
                        Select Case udtSchemeClaim.ControlType
                            Case SchemeClaimModel.EnumControlType.HSIVSS, SchemeClaimModel.EnumControlType.RVP, SchemeClaimModel.EnumControlType.EVSS, _
                                SchemeClaimModel.EnumControlType.CIVSS, SchemeClaimModel.EnumControlType.PIDVSS, SchemeClaimModel.EnumControlType.VSS, _
                                SchemeClaimModel.EnumControlType.ENHVSSO, SchemeClaimModel.EnumControlType.PPP

                                goToCreation = False
                                blnIsGoToVersion2 = True

                            Case Else
                                Me.ResetStep2aServiceDate()
                                Me.SetStep2aInputEHSClaimControlReadFromSession(True)
                                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step2a

                                ' Default service date in here:
                                Dim udtSubPlatformBLL As New SubPlatformBLL

                                txtStep2aServiceDate.Text = DateTime.Today.ToString(_udtFormatter.EnterDateFormat(udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform)))

                                goToCreation = False
                        End Select
                        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

                    End If
                End If

                ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                ' ----------------------------------------------------------
                EHSClaimBasePage.AuditLogSearchAccountComplete(_udtAuditLogEntry, strSchemeCode, strSearchDocCode, _
                                                               SessionHandler.HKICSymbolGetFormSession(FunctionCode), udtEHSAccount.EHSPersonalInformationList(0), _
                                                               EHSClaimBasePage.ParseSearchAccountResult(udtEHSAccount, udtSearchAccountStatus))
                ' CRE17-010 (OCSSS integration) [End][Chris YIM]

                If goToCreation = False AndAlso Not udtEHSAccount Is Nothing AndAlso udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                    EHSClaimBasePage.AuditLogValidatedAccounhtFound(_udtAuditLogEntry, udtEHSAccount)
                End If

                If goToCreation Then

                    '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

                    RedirectHandler.ToURL(ClaimVoucherMaster.ChildPage.EHSAccountCreation)

                    '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

                ElseIf blnIsGoToVersion2 Then
                    GoToVersion2()
                Else
                    EHSClaimBasePage.AuditLogEnterClaimDetailLoaded(_udtAuditLogEntry)
                End If
            End If

        End Sub

        ' Go to select Practice Step
        Protected Sub btnStep1PracticeStep_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStep1ChangePractice.Click

            ' To Handle Concurrent Browser
            If Not Me.EHSClaimTokenNumValidation(hfEHSClaimTokenNum.Value, FunctionCode) Then
                _blnIsConcurrentBrowserDetected = True
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.InternalError
                Return
            End If

            ' Move to Step Select Practice
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SelectPractice

        End Sub

        ' Go to Select Scheme Step
        Protected Sub btnStep1SchemeStep_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStep1ChangeScheme.Click

            ' To Handle Concurrent Browser
            If Not Me.EHSClaimTokenNumValidation(hfEHSClaimTokenNum.Value, FunctionCode) Then
                _blnIsConcurrentBrowserDetected = True
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.InternalError
                Return
            End If

            ' Move to Step Select Practice
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SelectScheme

        End Sub

        ' Go To Select Document Type Step
        Protected Sub btnStep1DocTypeStep_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStep1ChangeDocType.Click

            ' To Handle Concurrent Browser
            If Not Me.EHSClaimTokenNumValidation(hfEHSClaimTokenNum.Value, FunctionCode) Then
                _blnIsConcurrentBrowserDetected = True
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.InternalError
                Return
            End If

            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Dim strDocumentType As String = SessionHandler.DocumentTypeSelectedGetFromSession(FunctionCode)
            If strDocumentType = DocTypeModel.DocTypeCode.HKIC Then
                Me.udcStep1ClaimSearch.ClearHKICSymbolSelection()
            End If
            ' CRE17-010 (OCSSS integration) [End][Chris YIM]

            ' Move to Step Select Document Type
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SelectDocType

        End Sub

        Protected Sub udcStep1ClaimSearch_ShowInputTips(ByVal type As ucInputTips.InputTipsType) Handles udcStep1ClaimSearch.ShowInputTipsClick

            ' To Handle Concurrent Browser
            If Not Me.EHSClaimTokenNumValidation(hfEHSClaimTokenNum.Value, FunctionCode) Then
                _blnIsConcurrentBrowserDetected = True
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.InternalError
                Return
            End If

            ShowInputTips(type)
        End Sub

        ' Search Account Validation
        Protected Overrides Function Step1SearchValdiation(ByVal udtAuditLogEntry As AuditLogEntry) As Boolean
            ' Assume Valid
            Dim blnIsValid As Boolean = True
            Dim validator As Common.Validation.Validator = New Common.Validation.Validator
            Dim strDocCode As String = SessionHandler.DocumentTypeSelectedGetFromSession(FunctionCode).Trim()

            ' Store all validation result
            Dim udtSysMessages As New List(Of SystemMessage)

            ' Clear Info/Error MessageBox/Error Indicator
            Me.ResetStep1()
            Me.ClearMessageBox()

            ' Fill class variable from control's value
            udcStep1ClaimSearch.SetProperty(strDocCode)

            Select Case strDocCode
                Case DocTypeModel.DocTypeCode.HKIC, _
                     DocTypeModel.DocTypeCode.HKBC, _
                     DocTypeModel.DocTypeCode.DI, _
                     DocTypeModel.DocTypeCode.REPMT, _
                     DocTypeModel.DocTypeCode.ID235B, _
                     DocTypeModel.DocTypeCode.VISA, _
                     DocTypeModel.DocTypeCode.ADOPC

                    ' Validate ID
                    Dim udtSysMsgValidateID As SystemMessage = validator.chkIdentityNumber(strDocCode, udcStep1ClaimSearch.IdentityNo.ToUpper(), udcStep1ClaimSearch.IdentityNoPrefix)
                    If Not udtSysMsgValidateID Is Nothing Then
                        ' Erorr
                        blnIsValid = False

                        Select Case strDocCode
                            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                            ' ----------------------------------------------------------
                            Case DocTypeModel.DocTypeCode.HKIC
                                udcStep1ClaimSearch.SetHKICNoError(True)
                                ' CRE17-010 (OCSSS integration) [End][Chris YIM]
                            Case DocTypeModel.DocTypeCode.ADOPC
                                udcStep1ClaimSearch.SetADOPCIdentityNoError(True)
                            Case Else
                                udcStep1ClaimSearch.SetSearchShortIdentityNoError(True)
                        End Select

                        udtSysMessages.Add(udtSysMsgValidateID)
                    End If

                    ' Validate DOB
                    Dim udtSysMsgValidateDOB As SystemMessage = validator.chkDOB(strDocCode, udcStep1ClaimSearch.DOB)
                    If Not udtSysMsgValidateDOB Is Nothing Then
                        ' Error
                        blnIsValid = False

                        Select Case strDocCode
                            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                            ' ----------------------------------------------------------
                            Case DocTypeModel.DocTypeCode.HKIC
                                udcStep1ClaimSearch.SetHKICDOBError(True)
                                ' CRE17-010 (OCSSS integration) [End][Chris YIM]
                            Case DocTypeModel.DocTypeCode.ADOPC
                                udcStep1ClaimSearch.SetADOPCDOBError(True)
                            Case Else
                                udcStep1ClaimSearch.SetSearchShortDOBError(True)
                        End Select

                        udtSysMessages.Add(udtSysMsgValidateDOB)

                    End If


                Case DocTypeModel.DocTypeCode.EC

                    Dim strDateOfReg As String = String.Empty

                    ' Validate ID
                    Dim udtSysMsgValidateID As SystemMessage = validator.chkHKID(udcStep1ClaimSearch.IdentityNo.ToUpper())
                    If Not udtSysMsgValidateID Is Nothing Then
                        ' Error
                        blnIsValid = False

                        udcStep1ClaimSearch.SetECHKIDError(True)

                        udtSysMessages.Add(udtSysMsgValidateID)
                    End If

                    ' Validate DOB
                    If udcStep1ClaimSearch.ECDOBSelected Then
                        Dim udtSysMsgValidateDOB As SystemMessage = validator.chkDOB(strDocCode, udcStep1ClaimSearch.DOB)
                        If Not udtSysMsgValidateDOB Is Nothing Then
                            ' Error
                            blnIsValid = False

                            udcStep1ClaimSearch.SetECDOBError(True)

                            udtSysMessages.Add(udtSysMsgValidateDOB)
                        End If
                    Else
                        ' Validate EC Age
                        Dim udtSysMsgValidateECAge As SystemMessage = validator.chkECAge(udcStep1ClaimSearch.ECAge)
                        If Not udtSysMsgValidateECAge Is Nothing Then
                            ' Error
                            blnIsValid = False

                            udcStep1ClaimSearch.SetECDOAAgeError(True)

                            udtSysMessages.Add(udtSysMsgValidateECAge)
                        End If

                        ' Validate Date of Age
                        Dim strDOADay As String = udcStep1ClaimSearch.ECDOADay
                        Dim strDOAMonth As String = udcStep1ClaimSearch.ECDOAMonth
                        Dim strDOAYear As String = udcStep1ClaimSearch.ECDOAYear
                        Dim udtSysMsgValidateDateOfAge As SystemMessage = validator.chkECDOAge(strDOADay, strDOAMonth, strDOAYear)
                        If Not udtSysMsgValidateDateOfAge Is Nothing Then
                            ' Error
                            blnIsValid = False

                            udcStep1ClaimSearch.SetECDOAError(True)

                            udtSysMessages.Add(udtSysMsgValidateDateOfAge)
                        End If

                        ' Validate Age + Date of Age if Within Age
                        If blnIsValid Then
                            Dim udtSysMsgValidateECAgeAndDOAge As SystemMessage = validator.chkECAgeAndDOAge(udcStep1ClaimSearch.ECAge, strDOADay, strDOAMonth, strDOAYear)
                            If Not udtSysMsgValidateECAgeAndDOAge Is Nothing Then
                                ' Error
                                blnIsValid = False

                                udcStep1ClaimSearch.SetECDOAAgeError(True)
                                udcStep1ClaimSearch.SetECDOAError(True)

                                udtSysMessages.Add(udtSysMsgValidateECAgeAndDOAge)
                            End If
                        End If
                    End If
            End Select

            If udtSysMessages.Count > 0 Then
                Me.ShowError(udtSysMessages.ToArray(), udtAuditLogEntry, Common.Component.LogID.LOG00005, "Search Account Failed", _
                            New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, strDocCode, (New Formatter).formatDocumentIdentityNumber(strDocCode, udcStep1ClaimSearch.IdentityNo)))
            End If

            Return blnIsValid

        End Function

        '==================================================================== Code for SmartID ============================================================================
        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Private Sub btnStep1ReadSmartID_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStep1ReadNewSmartID.Click
            SessionHandler.UIDisplayHKICSymbolSaveToSession(FunctionCode, Me.udcStep1ClaimSearch.UIDisplayHKICSymbol)
            SessionHandler.OCSSSRefStatusRemoveFromSession(FunctionCode)

            Me.RedirectToIdeas(IdeasBLL.EnumIdeasVersion.Two)

        End Sub
        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Private Sub btnStep1ReadOldSmartID_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStep1ReadOldSmartID.Click
            SessionHandler.UIDisplayHKICSymbolSaveToSession(FunctionCode, Me.udcStep1ClaimSearch.UIDisplayHKICSymbol)
            SessionHandler.OCSSSRefStatusRemoveFromSession(FunctionCode)

            Me.RedirectToIdeas(IdeasBLL.EnumIdeasVersion.One)
        End Sub
        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Private Sub btnStep1ReadOldSmartIDCombo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStep1ReadNewSmartIDCombo.Click
            SessionHandler.UIDisplayHKICSymbolSaveToSession(FunctionCode, Me.udcStep1ClaimSearch.UIDisplayHKICSymbol)
            SessionHandler.OCSSSRefStatusRemoveFromSession(FunctionCode)

            Me.RedirectToIdeasCombo(IdeasBLL.EnumIdeasVersion.Combo)
        End Sub
        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

        '==================================================================================================================================================================

        Protected Overrides Sub SetupStep1(ByVal createPopupPractice As Boolean, ByVal activeViewChange As Boolean)
            Dim udtDataEntry As DataEntryUserModel = Nothing
            Dim udtSchemeClaimBLL As SchemeClaimBLL = New SchemeClaimBLL()
            Dim udtSmartIDContent As BLL.SmartIDContentModel = New BLL.SmartIDContentModel()
            Dim udtSelectedPracticeDisplay As BLL.PracticeDisplayModel = MyBase.SessionHandler.PracticeDisplayGetFromSession(FunctionCode)
            Dim udtSchemeClaim As SchemeClaimModel = SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
            MyBase.SessionHandler.CurrentUserGetFromSession(Me._udtSP, udtDataEntry)

            ' Show Practice
            Me.FillPracticeText(lblStep1Practice)

            ' Show Scheme
            Me.FillSchemeText(lblStep1Scheme)

            ' Show Document Type
            Me.FillDocumentTypeText(lblStep1DocType)

            ' Hide Change Practice button when only 1 practice available
            Dim udtPracticeDisplayModelCollection As BLL.PracticeDisplayModelCollection = GetAvailablePractice()
            Me.btnStep1ChangePractice.Visible = (Not udtPracticeDisplayModelCollection Is Nothing AndAlso udtPracticeDisplayModelCollection.Count > 1)

            ' Hide Change Scheme button when only 1 practice available
            ' CRE13-001 - EHAPP [Start][Tommy L]
            ' -------------------------------------------------------------------------------------
            'Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = GetAvailableScheme()
            Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = GetAvailableScheme().FilterByTextOnlyAvailable(True)
            ' CRE13-001 - EHAPP [End][Tommy L]
            Me.btnStep1ChangeScheme.Visible = (Not udtSchemeClaimModelCollection Is Nothing AndAlso udtSchemeClaimModelCollection.Count > 1)

            ' Hide Change Document Type button when only 1 practice available
            Dim udtSchemeDocTypeModelCollection As SchemeDocTypeModelCollection = GetAvailableDocumentType()
            Me.btnStep1ChangeDocType.Visible = (Not udtSchemeDocTypeModelCollection Is Nothing AndAlso udtSchemeDocTypeModelCollection.Count > 1)

            ' Hide Error Message
            If activeViewChange Then
                udcStep1ClaimSearch.SetECError(False)
                udcStep1ClaimSearch.SetADOPCError(False)
                udcStep1ClaimSearch.SetSearchShortError(False)
            End If

            'Setup Document Type Selection
            Dim strDocumentTyepCode = SessionHandler.DocumentTypeSelectedGetFromSession(FunctionCode)

            Me.udcStep1ClaimSearch.SchemeCode = udtSchemeClaim.SchemeCode

            If strDocumentTyepCode = DocTypeModel.DocTypeCode.HKIC Then
                Me.udcStep1ClaimSearch.UIEnableHKICSymbol = True
            End If
            Me.udcStep1ClaimSearch.Build(strDocumentTyepCode)
            Me.btnStep1GO.Enabled = True

            '==================================================================== Code for SmartID ============================================================================
            ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Dim strIDEASComboClientInstalled As String = IIf(SessionHandler.IDEASComboClientGetFormSession() Is Nothing, YesNo.No, SessionHandler.IDEASComboClientGetFormSession())
            Dim blnIDEASComboClientInstalled As Boolean = IIf(strIDEASComboClientInstalled = YesNo.Yes, True, False)
            Dim blnIDEASComboClientForceToUse As Boolean = IIf((New GeneralFunction).getSystemParameter("SmartID_IDEAS_Combo_Force_To_Use") = YesNo.Yes, True, False)

            btnStep1ReadOldSmartID.Enabled = False
            btnStep1ReadNewSmartID.Enabled = False
            btnStep1ReadNewSmartIDCombo.Enabled = False

            btnStep1ReadOldSmartID.Style.Add("display", "none")
            btnStep1ReadNewSmartID.Style.Add("display", "none")
            btnStep1ReadNewSmartIDCombo.Style.Add("display", "none")

            lblReadCardAndSearchNA.Visible = False

            trSmartIDSoftwareAvailableDownload.Visible = False
            trSmartID.Visible = False
            trSmartIDSoftwareNotInstalled.Visible = False
            trSmartIDCombo.Visible = False
            trSmartIDNoService.Visible = False

            If strDocumentTyepCode.Equals(DocType.DocTypeModel.DocTypeCode.HKIC) Then
                If BLL.SmartIDHandler.EnableSmartID AndAlso MyBase.IsSupportedDevice Then
                    If BLL.SmartIDHandler.TurnOnSmartID Then

                        If blnIDEASComboClientForceToUse Then
                            ' Combo Client Only Period
                            If blnIDEASComboClientInstalled Then
                                trSmartIDCombo.Visible = True

                                btnStep1ReadNewSmartIDCombo.Enabled = True
                                btnStep1ReadNewSmartIDCombo.Style.Remove("display")
                                btnStep1ReadNewSmartIDCombo.Text = Me.GetGlobalResourceObject("Text", "ReadSmartIDAndSearch")

                            Else
                                trSmartIDCombo.Visible = True

                                ' Disable "Read Smart ID And Search" if the client is not installed
                                btnStep1ReadNewSmartIDCombo.Enabled = False
                                btnStep1ReadNewSmartIDCombo.Style.Remove("display")
                                btnStep1ReadNewSmartIDCombo.Text = Me.GetGlobalResourceObject("Text", "ReadSmartIDAndSearch")
                                trSmartIDSoftwareNotInstalled.Visible = True

                            End If

                        Else
                            ' Transition Period
                            If blnIDEASComboClientInstalled Then
                                trSmartIDCombo.Visible = True

                                btnStep1ReadNewSmartIDCombo.Enabled = True
                                btnStep1ReadNewSmartIDCombo.Style.Remove("display")
                                btnStep1ReadNewSmartIDCombo.Text = Me.GetGlobalResourceObject("Text", "ReadSmartIDAndSearch")

                            Else
                                trSmartIDSoftwareAvailableDownload.Visible = True
                                trSmartID.Visible = True

                                btnStep1ReadOldSmartID.Enabled = True
                                btnStep1ReadOldSmartID.Style.Remove("display")
                                btnStep1ReadOldSmartID.Text = Me.GetGlobalResourceObject("AlternateText", "ReadOldCardAndSearchBtn")

                                btnStep1ReadNewSmartID.Enabled = True
                                btnStep1ReadNewSmartID.Style.Remove("display")
                                btnStep1ReadNewSmartID.Text = Me.GetGlobalResourceObject("AlternateText", "ReadNewCardAndSearchBtn")

                            End If

                        End If

                    Else
                        trSmartIDNoService.Visible = True
                        lblReadCardAndSearchNA.Visible = True
                        lblReadCardAndSearchNA.Text = Me.GetGlobalResourceObject("Text", "ReadCardAndSearchTextOnlyNA")

                    End If

                End If
            End If


            '==================================================================================================================================================================
            ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            If strDocumentTyepCode = DocTypeModel.DocTypeCode.HKIC Then
                If Me.udcStep1ClaimSearch.UIEnableHKICSymbol And Me.udcStep1ClaimSearch.UIDisplayHKICSymbol Then
                    If Me.udcStep1ClaimSearch.HKICSymbolSelectedValue = String.Empty Then
                        Me.btnStep1GO.Enabled = False

                        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                        If blnIDEASComboClientInstalled Then
                            Me.btnStep1ReadNewSmartIDCombo.Enabled = False
                        Else
                            Me.btnStep1ReadOldSmartID.Enabled = False
                            Me.btnStep1ReadNewSmartID.Enabled = False
                        End If
                        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

                    End If
                End If
            End If
            ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        End Sub

        Protected Overrides Sub Step1Clear(ByVal blnRetainDocType As Boolean)

            If Not blnRetainDocType Then
                Me.udcStep1ClaimSearch.CleanField()
            Else
                Me.udcStep1ClaimSearch.SetECError(False)
                Me.udcStep1ClaimSearch.SetADOPCError(False)
                Me.udcStep1ClaimSearch.SetSearchShortError(False)
            End If
        End Sub

        Private Sub ResetStep1()
            Me.udcStep1ClaimSearch.SetADOPCError(False)
            Me.udcStep1ClaimSearch.SetECError(False)
            Me.udcStep1ClaimSearch.SetSearchShortError(False)
        End Sub

#End Region


#Region "Step of Step 2a Enter Claim detail"

        '-------------------------------------------------------------------------------------------------------------------
        '   Function
        '-------------------------------------------------------------------------------------------------------------------
        Protected Overrides Sub SetupStep2a(ByVal udtEHSAccount As EHSAccountModel, ByVal activeViewChange As Boolean, ByVal setDefaultScheme As Boolean)

            If Me.IsPageRefreshed Then
                Me._blnIsRequireHandlePageRefresh = True
                Return
            End If

            'CRE15-003 System-generated Form [Start][Philip Chau]
            MyBase.SessionHandler.EHSClaimTempTransactionIDRemoveFromSession()
            MyBase.SessionHandler.EHSClaimStep3ShowLastestTransactionIDSaveToSession(False)
            'CRE15-003 System-generated Form [End][Philip Chau]

            ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [Start][Winnie]
            MyBase.SessionHandler.NoticedDuplicateClaimAlertRemoveFromSession()
            ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [End][Winnie]

            Dim udtSelectedPracticeDisplay As BLL.PracticeDisplayModel = Nothing
            Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = Nothing
            Dim udtSchemeClaimBLL As SchemeClaimBLL = New SchemeClaimBLL()
            Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
            Dim strDummy As String = String.Empty
            Dim strAllowDateBack As String = String.Empty
            Dim strSearchDocCode As String = udtEHSAccount.SearchDocCode
            Dim strRuleResults As RuleResultCollection
            Dim isValid As Boolean = True
            Dim udtClaimCategorys As ClaimCategoryModelCollection = Nothing
            Dim udtFormatter As Formatter = New Formatter

            ' To Handle Concurrent Browser
            If Not Me.EHSClaimTokenNumValidation(hfEHSClaimTokenNum.Value, FunctionCode) Then
                _blnIsConcurrentBrowserDetected = True
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.InternalError
                Return
            End If



            ' CRE13-001 - EHAPP [Start][Koala]
            ' -------------------------------------------------------------------------------------
            If Not IsNothing(udtSchemeClaim) AndAlso (New VaccinationBLL).SchemeContainVaccine(udtSchemeClaim) Then
                If CheckShowVaccinationRecord() Then Return
                btnStep2aViewVaccinationRecord.Visible = True
            Else
                btnStep2aViewVaccinationRecord.Visible = False
            End If
            ' CRE13-001 - EHAPP [End][Koala]

            'Default set Reminder invisible
            Me.tblStep2aReminderContainer.Visible = False

            ' Default Show Claim button on every scheme
            Me.btnStep2aClaim.Visible = True

            'display selected Practice
            udtSelectedPracticeDisplay = MyBase.SessionHandler.PracticeDisplayGetFromSession(FunctionCode)
            If MyBase.SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                Me.lblStep2aPractice.Text = String.Format("{0} ({1})", udtSelectedPracticeDisplay.PracticeNameChi, udtSelectedPracticeDisplay.PracticeID)
                Me.lblStep2aPractice.CssClass = "tableTextChi"
            Else
                Me.lblStep2aPractice.Text = String.Format("{0} ({1})", udtSelectedPracticeDisplay.PracticeName, udtSelectedPracticeDisplay.PracticeID)
                Me.lblStep2aPractice.CssClass = "tableText"
            End If


            ''Get all available Scheme <- for SP
            'udtSchemeClaimModelCollection = udtSchemeClaimBLL.searchValidClaimPeriodSchemeClaimByPracticeSchemeInfoSubsidizeCode(SessionSP.PracticeList(udtSelectedPracticeDisplay.PracticeID).PracticeSchemeInfoList)
            ''Get all Eligible Scheme form available List <- for EHS Account
            'udtSchemeClaimModelCollection = udtSchemeClaimBLL.searchEligibleClaimScheme(udtEHSAccount, udtEHSAccount.SearchDocCode, udtSchemeClaimModelCollection)

            'Invisibility Button
            MyBase.ShowChangePracticeButton(Me.btnStep2aChangePractice)

            ' Enable the Claim button
            Me.btnStep2aClaim.Enabled = True

            ' Hide Change Scheme button when only 1 practice available
            ' CRE13-001 - EHAPP [Start][Tommy L]
            ' -------------------------------------------------------------------------------------
            'udtSchemeClaimModelCollection = GetAvailableScheme()
            udtSchemeClaimModelCollection = GetAvailableScheme().FilterByTextOnlyAvailable(True)
            ' CRE13-001 - EHAPP [End][Tommy L]
            Me.btnStep2aChangeScheme.Visible = (Not udtSchemeClaimModelCollection Is Nothing AndAlso udtSchemeClaimModelCollection.Count > 1)

            If udtSchemeClaimModelCollection Is Nothing OrElse udtSchemeClaimModelCollection.Count = 0 Then
                'No Scheme for the the recipient 
                Me.SetupStep2aSchemeAvailableForClaim(False, udtEHSAccount, Nothing, activeViewChange)
                isValid = False
            Else
                'if not Visible here some contorl may not to enable for edit
                Me.panStep2aClaimDetail.Visible = True

                'Must have value <- not same as Full version <- Scheme claim was seleted in "Select Scheme"
                udtSchemeClaim = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode)

                ' Get scheme with Subsidize to Session and save to session
                udtSchemeClaim = udtSchemeClaimModelCollection.Filter(udtSchemeClaim.SchemeCode)

                MyBase.SessionHandler.SchemeSelectedSaveToSession(udtSchemeClaim, FunctionCode)

                If MyBase.SessionHandler.Language() = Common.Component.CultureLanguage.TradChinese Then
                    Me.lblStep2aSchemeSelectedText.Text = udtSchemeClaim.SchemeDescChi
                Else
                    Me.lblStep2aSchemeSelectedText.Text = udtSchemeClaim.SchemeDesc
                End If
            End If


            If isValid Then

                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                Select Case udtSchemeClaim.ControlType
                    Case SchemeClaimModel.EnumControlType.HSIVSS, SchemeClaimModel.EnumControlType.RVP

                        'if Category is not selected, got to Category Page
                        If MyBase.SessionHandler.ClaimCategoryGetFromSession(FunctionCode) Is Nothing Then

                            Dim udtPersonalInformation As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode)
                            Dim strServiceDate As String
                            Dim dtmServiceDate As DateTime
                            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
                            Dim strEnableClaimCategory As String = Nothing

                            udtGeneralFunction.getSytemParameterByParameterNameSchemeCode("RVPEnableClaimCategory", strEnableClaimCategory, String.Empty, SchemeClaimModel.RVP)

                            If String.IsNullOrEmpty(Me.txtStep2aServiceDate.Attributes(ValidatedServiceDate)) Then
                                dtmServiceDate = DateTime.Now()
                            Else
                                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                                '-----------------------------------------------------------------------------------------
                                Dim udtSubPlatformBLL As New SubPlatformBLL
                                'strServiceDate = udtFormatter.formatInputDate(Me.txtStep2aServiceDate.Attributes(ValidatedServiceDate))
                                strServiceDate = udtFormatter.formatInputDate(Me.txtStep2aServiceDate.Attributes(ValidatedServiceDate), udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
                                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

                                dtmServiceDate = udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English)
                            End If

                            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                            ' -----------------------------------------------------------------------------------------
                            'If scheme is HSIVSS or RVP, retrieve Claim Category
                            udtClaimCategorys = Me._udtClaimCategoryBLL.getDistinctCategoryByScheme(udtSchemeClaim, udtPersonalInformation, dtmServiceDate)
                            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                            If strEnableClaimCategory = "Y" Then
                                If udtClaimCategorys.Count > 1 Then
                                    Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Category
                                    Return
                                Else
                                    MyBase.SessionHandler.ClaimCategorySaveToSession(udtClaimCategorys(0), FunctionCode)
                                End If
                            Else
                                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                                ' Remove SchemeSeq
                                If udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.RVP) Then
                                    'MyBase.SessionHandler.ClaimCategorySaveToSession(udtClaimCategorys.FilterByCategoryCode(udtSchemeClaim.SchemeCode, udtSchemeClaim.SchemeSeq, "RESIDENT"), FunctionCode)
                                    MyBase.SessionHandler.ClaimCategorySaveToSession(udtClaimCategorys.FilterByCategoryCode(udtSchemeClaim.SchemeCode, "RESIDENT"), FunctionCode)
                                Else
                                    Throw New Exception("Enabled the Systeme Parametes of Claim Category for RVP and scheme HSIVSS is available for claim")
                                End If
                                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
                            End If

                        End If
                End Select
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

                ' CRE19-006 (DHC) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                'Service date Textbox setup 
                'Me._udtCommfunct.getSystemParameter("DateBackClaimAllow", strAllowDateBack, strDummy)
                Me._udtCommfunct.getSystemParameter("DateBackClaimAllow", strAllowDateBack, strDummy, udtSchemeClaim.SchemeCode)

                If strAllowDateBack = String.Empty Then
                    strAllowDateBack = "N"
                End If

                If strAllowDateBack = "Y" Then
                    'Show Service Date TextBox
                    Me.txtStep2aServiceDate.ForeColor = Drawing.Color.Black
                    Me.txtStep2aServiceDate.Style.Remove("Display")
                    Me.lblStep2aServiceDate.Visible = False

                Else
                    ' Set Service Date as Today
                    Me.txtStep2aServiceDate.Text = udtFormatter.formatInputTextDate(Me._udtGeneralFunction.GetSystemDateTime(), (New SubPlatformBLL).GetDateFormatLocale(Me.SubPlatform))

                    'Hide Service Date TextBox
                    Me.txtStep2aServiceDate.ForeColor = Drawing.Color.DimGray
                    Me.txtStep2aServiceDate.Style.Add("Display", "none")

                    Me.lblStep2aServiceDate.Text = Me.txtStep2aServiceDate.Text

                    Me.lblStep2aServiceDate.Visible = True
                End If
                ' CRE19-006 (DHC) [End][Winnie]

                ' --------------------------------------------------------------------------------------------
                ' TSW Checking
                ' --------------------------------------------------------------------------------------------
                If udtSchemeClaim.TSWCheckingEnable Then
                    If Me._udtEHSClaimBLL.chkIsTSWCase(Me._udtSP.SPID, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).IdentityNum) Then
                        'Show TSW panel
                        Me.tblStep2aReminderContainer.Visible = True
                        Me.lblStep2aReminder.Text = Me.GetGlobalResourceObject("Text", "TSWRemind")
                    End If
                End If

                'Check CIVSS Eligible Result, if child is over 6 years and the first dose is avalible for the child
                strRuleResults = MyBase.SessionHandler.EligibleResultGetFromSession()
                For Each udtRuleResult As RuleResult In strRuleResults.Values

                    If udtRuleResult.RuleType = RuleTypeENum.EligibleResult AndAlso udtRuleResult.HandleMethod = HandleMethodENum.Declaration AndAlso udtRuleResult.PromptConfirmed Then

                        If udtRuleResult.SchemeCode.Trim() = udtSchemeClaim.SchemeCode.Trim() Then
                            If CType(udtRuleResult, EligibleResult).IsEligible Then
                                Dim udtEligibleResult As EligibleResult = CType(udtRuleResult, EligibleResult)

                                Dim strObjectName2 As String = String.Empty

                                If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                                    strObjectName2 = udtEligibleResult.RelatedEligibleRule.ObjectName2

                                ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                                    strObjectName2 = udtEligibleResult.RelatedEligibleExceptionRule.ObjectName2
                                End If

                                If Not String.IsNullOrEmpty(strObjectName2) Then
                                    Me.tblStep2aReminderContainer.Visible = True
                                    Me.lblStep2aReminder.Text = Me.GetGlobalResourceObject("Text", strObjectName2)
                                End If
                            End If
                        End If
                    End If

                Next

                Me.SetupStep2aSchemeAvailableForClaim(True, udtEHSAccount, udtSchemeClaim, activeViewChange, udtClaimCategorys)

            End If
        End Sub

        Protected Overrides Sub SetupStep2aSchemeAvailableForClaim(ByVal isAvailable As Boolean, ByVal udtEHSAccount As EHSAccountModel, _
            ByVal udtSchemeClaim As SchemeClaimModel, ByVal activeViewChange As Boolean, Optional ByVal udtClaimCategorys As ClaimCategoryModelCollection = Nothing)

            Dim udtPracticeDisplays As BLL.PracticeDisplayModelCollection = Nothing
            Dim udtEHSTransaction As EHSTransactionModel
            Dim udtFormatter As Formatter

            If isAvailable Then
                'Init
                udtEHSTransaction = MyBase.SessionHandler.EHSTransactionGetFromSession(FunctionCode)
                udtFormatter = New Formatter

                'Show claim detail
                Me.panStep2aClaimDetail.Visible = True

                'Setup Service date
                If Not udtEHSTransaction Is Nothing AndAlso Not activeViewChange Then
                    'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    Dim udtSubPlatformBLL As New SubPlatformBLL

                    'Me.txtStep2aServiceDate.Text = udtFormatter.formatEnterDate(udtEHSTransaction.ServiceDate)
                    Me.txtStep2aServiceDate.Text = udtFormatter.formatInputTextDate(udtEHSTransaction.ServiceDate, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
                    'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

                End If

                Me.lblStep2aServiceDateText.Text = Me.GetGlobalResourceObject("Text", "ServiceDate")
                Me.lblStep2aSchemeText.Text = Me.GetGlobalResourceObject("Text", "Scheme")

                Me.SetupStep2aClaimContent(udtSchemeClaim, udtEHSAccount, activeViewChange, udtClaimCategorys)
            Else
                Me.panStep2aClaimDetail.Visible = False

                Me.udcStep2aInputEHSClaim.Clear()

                Me.udcMsgBoxInfo.AddMessage(New SystemMessage("990000", "I", "00020"))
                Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Information
                Me.udcMsgBoxInfo.BuildMessageBox()

                ' No more scheme available, disable the claim button
                Me.btnStep2aClaim.Enabled = False

            End If

            If activeViewChange Then
                Me.udcStep2aReadOnlyDocumnetType.TextOnlyVersion = True
                ' INT13-0021 - Fix use HKBC on smart IC claim incorrectly [Start][Koala]
                ' -------------------------------------------------------------------------------------
                Me.udcStep2aReadOnlyDocumnetType.DocumentType = udtEHSAccount.SearchDocCode
                'Me.udcStep2aReadOnlyDocumnetType.DocumentType = udtEHSAccount.EHSPersonalInformationList(0).DocCode.Trim()
                ' INT13-0021 - Fix use HKBC on smart IC claim incorrectly [End][Koala]
                Me.udcStep2aReadOnlyDocumnetType.EHSAccount = udtEHSAccount
                Me.udcStep2aReadOnlyDocumnetType.Vertical = False
                Me.udcStep2aReadOnlyDocumnetType.MaskIdentityNo = True
                Me.udcStep2aReadOnlyDocumnetType.ShowAccountRefNo = False
                Me.udcStep2aReadOnlyDocumnetType.ShowTempAccountNotice = False
                Me.udcStep2aReadOnlyDocumnetType.ShowAccountCreationDate = False

                Dim udtSmartIDContent As BLL.SmartIDContentModel = MyBase.SessionHandler.SmartIDContentGetFormSession(FunctionCode)
                If Not udtSmartIDContent Is Nothing _
                        AndAlso udtSmartIDContent.IsReadSmartID _
                        AndAlso udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount _
                        AndAlso SmartIDShowRealID() Then
                    udcStep2aReadOnlyDocumnetType.IsSmartID = True

                Else
                    udcStep2aReadOnlyDocumnetType.IsSmartID = False

                End If

                ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                ' ----------------------------------------------------------
                Me.udcStep2aReadOnlyDocumnetType.SetEnableToShowHKICSymbol = True
                ' CRE17-010 (OCSSS integration) [End][Chris YIM]
                Me.udcStep2aReadOnlyDocumnetType.Built()
            End If

        End Sub

        Protected Overrides Sub SetupStep2aClaimContent(ByVal udtSchemeClaim As SchemeClaimModel, ByVal udtEHSAccount As EHSAccountModel, ByVal activeViewChange As Boolean, Optional ByVal udtClaimCategorys As ClaimCategoryModelCollection = Nothing)

            ' CRE18-0XX (Opt voucher capping) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Dim blnNoAvailableQuota As Boolean = False

            ' when active view changed (go to next step), load value from session
            'Dim isSchemeAvailableForClaim As Boolean = Me.Step2aBuildInputEHSClaimControl(udtSchemeClaim, udtEHSAccount, udtClaimCategorys)
            Dim isSchemeAvailableForClaim As Boolean = Me.Step2aBuildInputEHSClaimControl(udtSchemeClaim, udtEHSAccount, udtClaimCategorys, blnNoAvailableQuota)
            ' CRE18-0XX (Opt voucher capping) [End][Winnie]

            If isSchemeAvailableForClaim Then
                Me.udcMsgBoxInfo.Clear()
                'enable proceed claim control
                'Me.txtStep2aServiceDate.Enabled = True
                Me.btnStep2aClaim.Enabled = True

                ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                ' ----------------------------------------------------------
                Dim udtSystemMessage As SystemMessage = CheckValidHKICInScheme(udtSchemeClaim.SchemeCode)

                If Not udtSystemMessage Is Nothing Then
                    Me.udcStep2aInputEHSClaim.AvaliableForClaim = False

                    Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
                    Me.udcMsgBoxErr.BuildMessageBox()

                    Me.btnStep2aClaim.Enabled = False
                End If
                ' CRE17-010 (OCSSS integration) [End][Chris YIM]

            Else
                ' No available scheme notification
                Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Information

                ' CRE19-003 (Opt voucher capping) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                If blnNoAvailableQuota Then
                    ' No Available Quota
                    Dim udtSM As SystemMessage = New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00045)


                    Dim udtSelectedPracticeDisplay As PracticeDisplayModel = Me.SessionHandler.PracticeDisplayGetFromSession(FunctionCode)

                    Dim strMsg_en = String.Format(HttpContext.GetGlobalResourceObject("Text", "ProfessionQuota", New System.Globalization.CultureInfo(CultureLanguage.English)) _
                                                  , HttpContext.GetGlobalResourceObject("Text", udtSelectedPracticeDisplay.ServiceCategoryCode.Trim, New System.Globalization.CultureInfo(CultureLanguage.English)))

                    Dim strMsg_tc = String.Format(HttpContext.GetGlobalResourceObject("Text", "ProfessionQuota", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)) _
                                                  , HttpContext.GetGlobalResourceObject("Text", udtSelectedPracticeDisplay.ServiceCategoryCode.Trim, New System.Globalization.CultureInfo(CultureLanguage.TradChinese)))

                    udtSM.AddReplaceMessage("%en", strMsg_en)
                    udtSM.AddReplaceMessage("%tc", strMsg_tc)

                    Me.udcMsgBoxInfo.AddMessage(udtSM)
                Else
                    ' No available subsidy in the selected scheme
                    Me.udcMsgBoxInfo.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00019))
                End If
                ' CRE19-003 (Opt voucher capping) [End][Winnie]

                Me.udcMsgBoxInfo.BuildMessageBox()

                'disable proceed claim control
                'Me.txtStep2aServiceDate.Enabled = False
                Me.btnStep2aClaim.Enabled = False
            End If

        End Sub

        ' Return Boolean flag indicate the Scheme is available or not:  Return True if available
        Private Function Step2aBuildInputEHSClaimControl() As Boolean

            Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
            Dim udtEHSAccount As EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(FunctionCode)

            Return Step2aBuildInputEHSClaimControl(udtSchemeClaim, udtEHSAccount)

        End Function

        ' Return Boolean flag indicate the Scheme is available or not
        ' CRE19-003 (Opt voucher capping) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        'Private Function Step2aBuildInputEHSClaimControl(ByVal udtSchemeClaim As SchemeClaimModel, ByVal udtEHSAccount As EHSAccountModel, Optional ByVal udtClaimCategorys As ClaimCategoryModelCollection = Nothing) As Boolean
        Private Function Step2aBuildInputEHSClaimControl(ByVal udtSchemeClaim As SchemeClaimModel, ByVal udtEHSAccount As EHSAccountModel, Optional ByVal udtClaimCategorys As ClaimCategoryModelCollection = Nothing, Optional ByRef blnNoAvailableQuota As Boolean = False) As Boolean
            ' CRE19-003 (Opt voucher capping) [End][Winnie]

            Dim blnIsAvailable As Boolean = False
            Dim strServiceDate As String
            Dim dtmServiceDate As Date
            'udtClaimCategory Must have value
            Dim udtClaimCategory As ClaimCategoryModel = Me.SessionHandler.ClaimCategoryGetFromSession(FunctionCode)

            ' CRE19-003 (Opt voucher capping) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Dim udtSelectedPracticeDisplay As PracticeDisplayModel = Me.SessionHandler.PracticeDisplayGetFromSession(FunctionCode)
            ' CRE19-003 (Opt voucher capping) [End][Winnie]

            'INT16-0028 (Fix the Service Date validation in Claim functions) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim udtValidator As New Validator
            Dim sm As SystemMessage = Nothing
            'INT16-0028 (Fix the Service Date validation in Claim functions) [End][Chris YIM]

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
            ' -----------------------------------------------------------------------------------------

            ' Skip service date if invalid format (validation will handled in service date text changed event)
            Dim udtSubPlatformBLL As New SubPlatformBLL
            strServiceDate = _udtFormatter.formatInputDate(Me.txtStep2aServiceDate.Text, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))

            Try
                'INT16-0028 (Fix the Service Date validation in Claim functions) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Check Service Date
                sm = udtValidator.chkServiceDate(strServiceDate)

                'Check Back Date
                If sm Is Nothing Then
                    Dim strAllowDateBack As String = String.Empty
                    Dim strClaimDayLimit As String = String.Empty
                    Dim strMinDate As String = String.Empty
                    Dim intDayLimit As Integer
                    Dim dtmMinDate As DateTime

                    Me._udtCommfunct.getSystemParameter("DateBackClaimDayLimit", strClaimDayLimit, String.Empty, udtSchemeClaim.SchemeCode)
                    Me._udtCommfunct.getSystemParameter("DateBackClaimMinDate", strMinDate, String.Empty, udtSchemeClaim.SchemeCode)

                    intDayLimit = CInt(strClaimDayLimit)
                    dtmMinDate = Convert.ToDateTime(strMinDate)

                    sm = udtValidator.chkDateBackClaimServiceDate(strServiceDate, intDayLimit, dtmMinDate)
                End If

                If sm Is Nothing Then
                    dtmServiceDate = _udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English)
                Else
                    dtmServiceDate = (New GeneralFunction).GetSystemDateTime.Date
                End If
                'INT16-0028 (Fix the Service Date validation in Claim functions) [End][Chris YIM]

            Catch ex As Exception
                ' Do Nothing
            End Try

            Dim udtTransaction As EHSTransactionModel = Nothing
            If IsNothing(MyBase.SessionHandler.EHSTransactionGetFromSession(FunctionCode)) Then
                udtTransaction = New EHSTransactionModel
                udtTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()
                MyBase.SessionHandler.EHSTransactionSaveToSession(udtTransaction, FunctionCode)
            Else
                udtTransaction = MyBase.SessionHandler.EHSTransactionGetFromSession(FunctionCode)
            End If

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            ' Build control only when the scheme have subsidize available
            If Not udtSchemeClaim Is Nothing AndAlso Not udtSchemeClaim.SubsidizeGroupClaimList Is Nothing AndAlso Not udtEHSAccount Is Nothing Then

                Select Case udtSchemeClaim.ControlType

                    Case SchemeClaimModel.EnumControlType.VOUCHER ', SchemeClaimModel.EnumControlType.VOUCHERCHINA

                        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                        If udtEHSAccount.VoucherInfo Is Nothing Then

                            ' Recheck available voucher when service date change (Handle date back to last year)
                            Dim udtVoucherInfo As New VoucherInfoModel(VoucherInfoModel.AvailableVoucher.Include, _
                                                                       VoucherInfoModel.AvailableQuota.Include)

                            udtVoucherInfo.GetInfo(dtmServiceDate, MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode), _
                                                   udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode), _
                                                   udtSelectedPracticeDisplay.ServiceCategoryCode)

                            udtEHSAccount.VoucherInfo = udtVoucherInfo

                            MyBase.SessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctionCode)
                        End If

                        If udtEHSAccount.VoucherInfo.GetAvailableVoucher() > 0 Then
                            ' CRE19-003 (Opt voucher capping) [Start][Winnie]
                            ' ----------------------------------------------------------------------------------------
                            Dim udtVoucherQuota As VoucherQuotaModel = udtEHSAccount.VoucherInfo.VoucherQuotalist.FilterByProfCodeEffectiveDtm(udtSelectedPracticeDisplay.ServiceCategoryCode, dtmServiceDate)

                            If Not udtVoucherQuota Is Nothing Then
                                If udtVoucherQuota.AvailableQuota > 0 Then
                                    blnIsAvailable = True
                                Else
                                    blnIsAvailable = False
                                    blnNoAvailableQuota = True
                                End If
                            Else
                                blnIsAvailable = True
                            End If
                            ' CRE19-003 (Opt voucher capping) [End][Winnie]
                        End If
                        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

                    Case SchemeClaimModel.EnumControlType.EHAPP
                        ' Nothing to do

                End Select


                ' INT13-0022 - Fix some special handling on HCSP text only version [Start][Koala]
                ' -------------------------------------------------------------------------------------
                Me.udcStep2aInputEHSClaim.IsSupportedDevice = True
                'Me.udcStep2aInputEHSClaim.IsSupportedDevice = MyBase.IsSupportedDevice
                ' INT13-0022 - Fix some special handling on HCSP text only version [End][Koala]
                Me.udcStep2aInputEHSClaim.AvaliableForClaim = blnIsAvailable
                Me.udcStep2aInputEHSClaim.TextOnlyVersion = True
                Me.udcStep2aInputEHSClaim.CurrentPractice = MyBase.SessionHandler.PracticeDisplayGetFromSession(FunctionCode)
                Me.udcStep2aInputEHSClaim.SchemeType = udtSchemeClaim.SchemeCode.Trim()
                Me.udcStep2aInputEHSClaim.EHSAccount = udtEHSAccount

                ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
                ' -----------------------------------------------------------------------------------------

                Me.udcStep2aInputEHSClaim.EHSTransaction = udtTransaction
                Me.udcStep2aInputEHSClaim.SetRebuildRequired()

                ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

                Me.udcStep2aInputEHSClaim.ServiceDate = dtmServiceDate
                Me.udcStep2aInputEHSClaim.Built()

            End If

            Return blnIsAvailable

        End Function

        Private Sub SetStep2aInputEHSClaimControlReadFromSession(ByVal blnIsReadFromSession As Boolean)
            Me.udcStep2aInputEHSClaim.ActiveViewChanged = blnIsReadFromSession
        End Sub

        Private Sub ResetStep2aServiceDate()
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim udtSubPlatformBLL As New SubPlatformBLL

            'Me.txtStep2aServiceDate.Text = Me._udtFormatter.formatEnterDate(Me._udtCommfunct.GetSystemDateTime())
            Me.txtStep2aServiceDate.Text = Me._udtFormatter.formatInputTextDate(Me._udtCommfunct.GetSystemDateTime(), udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            Me.txtStep2aServiceDate.Attributes(ValidatedServiceDate) = Me.txtStep2aServiceDate.Text
        End Sub

        Private Sub LoadServiceDateIntoUdcInputHCVS()
            Dim udcInputHCVS As UIControl.EHCClaimText.ucInputHCVS = Me.udcStep2aInputEHSClaim.GetHCVSControl()
            If udcInputHCVS IsNot Nothing Then
                udcInputHCVS.ServiceDate = Me.txtStep2aServiceDate.Text
            End If
        End Sub

#End Region


#Region "Event of Step 2a Enter Claim detail"

        ''' <summary>
        ''' Click "View Vaccination Record"
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnStep2aViewVaccinationRecord_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStep2aViewVaccinationRecord.Click
            If (MyBase.IsPageRefreshed) Then
                _blnIsRequireHandlePageRefresh = True
                Return
            End If

            ' To Handle Concurrent Browser
            If Not Me.EHSClaimTokenNumValidation(hfEHSClaimTokenNum.Value, FunctionCode) Then
                _blnIsConcurrentBrowserDetected = True
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.InternalError
                Return
            End If

            ShowVaccinationRecord(True)
        End Sub

        '-------------------------------------------------------------------------------------------------------------------
        '   Events
        '-------------------------------------------------------------------------------------------------------------------
        Protected Sub btnStep2aCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStep2aCancel.Click
            If (MyBase.IsPageRefreshed) Then
                _blnIsRequireHandlePageRefresh = True
                Return
            End If

            Me.SetStep2aInputEHSClaimControlReadFromSession(True)
            Me.ShowConfirmMessageBox(ConfirmationStyle.Bordered, "CancelAlert")
        End Sub

        Protected Sub btnStep2aClaim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStep2aClaim.Click
            If (MyBase.IsPageRefreshed) Then
                _blnIsRequireHandlePageRefresh = True
                Return
            End If

            ' To Handle Concurrent Browser
            If Not Me.EHSClaimTokenNumValidation(hfEHSClaimTokenNum.Value, FunctionCode) Then
                _blnIsConcurrentBrowserDetected = True
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.InternalError
                Return
            End If

            Me.SetStep2aInputEHSClaimControlReadFromSession(True)
            Step2aClaimSubmit(False)
        End Sub

        Private Sub btnStep2aChangePractice_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnStep2aChangePractice.Click
            If (MyBase.IsPageRefreshed) Then
                _blnIsRequireHandlePageRefresh = True
                Return
            End If

            Me.SetStep2aInputEHSClaimControlReadFromSession(True)

            ' Go to change practice view
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SelectPractice

        End Sub

        Private Sub btnStep2aChangeScheme_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnStep2aChangeScheme.Click
            If (MyBase.IsPageRefreshed) Then
                _blnIsRequireHandlePageRefresh = True
                Return
            End If

            Me.SetStep2aInputEHSClaimControlReadFromSession(True)

            ' Go to change scheme view
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SelectScheme

        End Sub

        Protected Sub udcStep2aInputEHSClaim_RemarkClicked(ByVal sender As Object, ByVal e As EventArgs) Handles udcStep2aInputEHSClaim.VaccineRemarkClicked
            If (MyBase.IsPageRefreshed) Then
                _blnIsRequireHandlePageRefresh = True
                Return
            End If

            Me.SetStep2aInputEHSClaimControlReadFromSession(True)

            ShowVaccineRemarkText()

        End Sub

        Private Sub udcStep2aInputEHSClaim_CategorySelected(ByVal sender As Object, ByVal e As System.EventArgs) Handles udcStep2aInputEHSClaim.CategorySelected
            Dim udtFormatter As Formatter = New Formatter
            Dim udtValidator As Validator = New Validator
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim udtSubPlatformBLL As New SubPlatformBLL
            'Dim strServiceDate As String = udtFormatter.formatInputDate(Me.txtStep2aServiceDate.Text)
            Dim strServiceDate As String = udtFormatter.formatInputDate(Me.txtStep2aServiceDate.Text, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
            Dim strClaimDayLimit As String = String.Empty
            Dim strMinDate As String = String.Empty
            Dim strAllowDateBack As String = String.Empty
            Dim intDayLimit As Integer
            Dim dtmMinDate As DateTime
            Dim isValid As Boolean = True
            Dim udtSystemMessage As SystemMessage
            'Initi Error image
            Me.lblStep2aServiceDateError.Visible = False


            'Check Service Date Format
            udtSystemMessage = udtValidator.chkServiceDate(strServiceDate)
            If Not udtSystemMessage Is Nothing Then
                isValid = False
                Me.lblStep2aServiceDateError.Visible = True
                Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
            End If

            If isValid Then
                'Check Service Date Back
                Me._udtCommfunct.getSystemParameter("DateBackClaimAllow", strAllowDateBack, String.Empty, udtSchemeClaim.SchemeCode)
                If strAllowDateBack = "Y" Then

                    Me._udtCommfunct.getSystemParameter("DateBackClaimDayLimit", strClaimDayLimit, String.Empty, udtSchemeClaim.SchemeCode)
                    Me._udtCommfunct.getSystemParameter("DateBackClaimMinDate", strMinDate, String.Empty, udtSchemeClaim.SchemeCode)

                    intDayLimit = CInt(strClaimDayLimit)
                    dtmMinDate = Convert.ToDateTime(strMinDate)

                    udtSystemMessage = udtValidator.chkDateBackClaimServiceDate(strServiceDate, intDayLimit, dtmMinDate)
                    If Not udtSystemMessage Is Nothing Then
                        isValid = False
                        Me.lblStep2aServiceDateError.Visible = True

                        ' Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
                        Select Case udtSystemMessage.MessageCode
                            Case "00149"
                                Me.udcMsgBoxErr.AddMessage(udtSystemMessage, New String() {"%s"}, New String() {strClaimDayLimit})
                            Case "00150"
                                Me.udcMsgBoxErr.AddMessage(udtSystemMessage, New String() {"%s"}, New String() {strMinDate})
                            Case Else
                                Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
                        End Select

                    End If

                End If
            End If

            If isValid Then
                Me.txtStep2aServiceDate.Text = strServiceDate
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Category

            End If

            Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail)

        End Sub

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------

        Protected Sub udcStep2aInputEHSClaim_SelectReasonForVisitClick(ByVal sender As Object, ByVal e As EventArgs) Handles udcStep2aInputEHSClaim.SelectReasonForVisitClicked
            Me.udcMsgBoxErr.Clear()
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
            If udtGeneralFunction.IsCoPaymentFeeEnabled(Me.udcStep2aInputEHSClaim.ServiceDate) Then
                Me.udcReasonForVisit.Mode(UIControl.EHCClaimText.ucReasonForVisit.EnumMode.AfterCopaymentFeeEnabled)
            Else
                Me.udcReasonForVisit.Mode(UIControl.EHCClaimText.ucReasonForVisit.EnumMode.BeforeCopaymentFeeEnabled)
            End If
            Me.SetupReasonForVisit()
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.ReasonForVisit
        End Sub

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        Protected Sub Step2aClaimSubmit(ByVal blnIsConfirmed As Boolean)
            Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
            Dim udtEHSAccount As EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(FunctionCode)
            Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
            Dim udtPracticeDisplay As BLL.PracticeDisplayModel = MyBase.SessionHandler.PracticeDisplayGetFromSession(FunctionCode)
            Dim udtValidator As Validator = New Validator()
            Dim strServiceDate As String = String.Empty
            Dim udtSystemMessage As SystemMessage
            Dim isValid As Boolean = True
            Dim udtFormatter As Formatter = New Formatter()
            Dim isTSWCase As Boolean = False

            EHSClaimBasePage.AuditLogEnterClaimDetailStart(_udtAuditLogEntry, blnIsConfirmed, MyBase.SessionHandler.SmartIDContentGetFormSession(FunctionCode))

            ' To Handle Concurrent Browser
            If Me._blnIsConcurrentBrowserDetected Then
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.InternalError
                Return
            End If

            ' Get SP
            MyBase.SessionHandler.CurrentUserGetFromSession(Me._udtSP, Me._udtDataEntryModel)

            'Reset Error Image
            Me.lblStep2aServiceDateError.Visible = False
            Me.udcStep2aInputEHSClaim.ClearErrorMessage()

            '------------------------------------------------ Check Service Date ---------------------------------------------------
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim udtSubPlatformBLL As New SubPlatformBLL
            'strServiceDate = _udtFormatter.formatInputDate(Me.txtStep2aServiceDate.Text)
            strServiceDate = _udtFormatter.formatInputDate(Me.txtStep2aServiceDate.Text, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            udtSystemMessage = udtValidator.chkServiceDate(strServiceDate)

            If Not udtSystemMessage Is Nothing Then
                ' Error
                isValid = False
                Me.lblStep2aServiceDateError.Visible = True
                Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
            Else
                Me.txtStep2aServiceDate.Text = strServiceDate
            End If

            If isValid Then
                ' Check Back Date Case
                Dim strAllowDateBack As String = String.Empty
                Dim strClaimDayLimit As String = String.Empty
                Dim strMinDate As String = String.Empty
                Dim intDayLimit As Integer
                Dim dtmMinDate As DateTime

                Me._udtCommfunct.getSystemParameter("DateBackClaimAllow", strAllowDateBack, String.Empty, udtSchemeClaim.SchemeCode)
                If strAllowDateBack = "Y" Then

                    Me._udtCommfunct.getSystemParameter("DateBackClaimDayLimit", strClaimDayLimit, String.Empty, udtSchemeClaim.SchemeCode)
                    Me._udtCommfunct.getSystemParameter("DateBackClaimMinDate", strMinDate, String.Empty, udtSchemeClaim.SchemeCode)

                    intDayLimit = CInt(strClaimDayLimit)
                    dtmMinDate = Convert.ToDateTime(strMinDate)

                    ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]
                    ' -----------------------------------------------------------------------------------------
                    ' Profession Claim block
                    If udtPracticeDisplay.Profession.ClaimPeriodFrom.HasValue AndAlso udtPracticeDisplay.Profession.ClaimPeriodFrom > dtmMinDate Then
                        dtmMinDate = udtPracticeDisplay.Profession.ClaimPeriodFrom.Value.Date
                    End If
                    ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

                    ' ServiceDate should not before SP Enrol / Effective date
                    If Me._udtSP.EffectiveDtm.HasValue AndAlso Me._udtSP.EffectiveDtm.Value > dtmMinDate Then
                        dtmMinDate = Me._udtSP.EffectiveDtm.Value.Date
                    End If

                    udtSystemMessage = udtValidator.chkDateBackClaimServiceDate(strServiceDate, intDayLimit, dtmMinDate)
                    If Not udtSystemMessage Is Nothing Then
                        ' Error
                        isValid = False
                        Me.lblStep2aServiceDateError.Visible = True

                        Select Case udtSystemMessage.MessageCode
                            Case "00149"
                                Me.udcMsgBoxErr.AddMessage(udtSystemMessage, New String() {"%s"}, New String() {strClaimDayLimit})
                            Case "00150"
                                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                                '-----------------------------------------------------------------------------------------
                                udtSubPlatformBLL = New SubPlatformBLL
                                'Me.udcMsgBoxErr.AddMessage(udtSystemMessage, New String() {"%s"}, New String() {udtFormatter.formatDate(dtmMinDate, Me.SessionHandler.Language)})
                                Me.udcMsgBoxErr.AddMessage(udtSystemMessage, New String() {"%s"}, New String() {udtFormatter.formatDisplayDate(dtmMinDate, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))})
                                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
                            Case Else
                                Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
                        End Select
                    End If
                End If
            End If


            '-------------------------------------- Check Service Date VS Permit to Remain ---------------------------------------
            If isValid Then
                udtEHSPersonalInfo = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode.Trim())
                If udtEHSPersonalInfo.DocCode = DocTypeModel.DocTypeCode.ID235B AndAlso udtEHSPersonalInfo.PermitToRemainUntil.HasValue Then
                    udtSystemMessage = udtValidator.ChkServiceDatePermitToRemainUntil(udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English), udtEHSPersonalInfo.PermitToRemainUntil.Value)
                    If Not udtSystemMessage Is Nothing Then
                        isValid = False
                        Me.lblStep2aServiceDateError.Visible = True
                        Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
                    End If
                End If
            End If

            '-------------------------------------------------------------------------------------------------------------------------

            ' Scheme Code Specific Validation
            If isValid Then
                'Construct EHS Transaction
                Me._udtEHSTransaction = Me._udtEHSClaimBLL.ConstructNewEHSTransaction(udtSchemeClaim, udtEHSAccount, udtPracticeDisplay, _udtFormatter.convertDate(strServiceDate, CultureLanguage.English))
                Me._udtEHSTransaction.ServiceDate = _udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English)

                ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [Start][Winnie]
                Select Case udtSchemeClaim.ControlType
                    Case SchemeClaimModel.EnumControlType.VOUCHER
                        isValid = Me.Step2aHCVSValidation(blnIsConfirmed, Me._udtEHSTransaction)

                    Case SchemeClaimModel.EnumControlType.VOUCHERCHINA
                        'No text version

                    Case SchemeClaimModel.EnumControlType.EHAPP
                        'No text version

                End Select
                ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [End][Winnie]

                Me.txtStep2aServiceDate.Attributes(ValidatedServiceDate) = strServiceDate
            End If

            ' If all valid, Finalize Transaction object and go to Confirm claim
            If isValid Then
                ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                ' ----------------------------------------------------------
                If Common.OCSSS.OCSSSServiceBLL.EnableHKICSymbolInput(udtSchemeClaim.SchemeCode) Then
                    Me._udtEHSTransaction.HKICSymbol = SessionHandler.HKICSymbolGetFormSession(FunctionCode)
                    Me._udtEHSTransaction.OCSSSRefStatus = SessionHandler.OCSSSRefStatusGetFormSession(FunctionCode)
                Else
                    Me._udtEHSTransaction.HKICSymbol = String.Empty
                    Me._udtEHSTransaction.OCSSSRefStatus = String.Empty
                End If
                ' CRE17-010 (OCSSS integration) [End][Chris YIM]

                ' Construct Transaction
                If udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.VOUCHER Then

                    Me._udtEHSClaimBLL.ConstructEHSTransactionDetails(Me._udtSP, Me._udtDataEntryModel, Me._udtEHSTransaction, udtEHSAccount)

                ElseIf udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.EHAPP Then
                    ' Nothing
                Else
                    Me._udtEHSClaimBLL.ConstructEHSTransactionDetails(Me._udtSP, Me._udtDataEntryModel, Me._udtEHSTransaction, udtEHSAccount, MyBase.SessionHandler.EHSClaimVaccineGetFromSession())
                End If

                MyBase.SessionHandler.EHSTransactionSaveToSession(Me._udtEHSTransaction, FunctionCode)

                ' --------------------------------------------------------------------------------------------
                ' TSW Checking
                ' --------------------------------------------------------------------------------------------
                If udtSchemeClaim.TSWCheckingEnable Then
                    isTSWCase = Me._udtEHSClaimBLL.chkIsTSWCase(Me._udtSP.SPID, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).IdentityNum)
                End If

                'Case of HCVS
                EHSClaimBasePage.AuditLogEnterClaimDetailPassed(_udtAuditLogEntry, Me._udtEHSTransaction, blnIsConfirmed, isTSWCase)

                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step2b
            Else

                Dim errorMessageCodeTable As DataTable = Me.udcMsgBoxErr.GetCodeTable

                If errorMessageCodeTable.Rows.Count > 0 Then
                    Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail, _udtAuditLogEntry, Common.Component.LogID.LOG00015, String.Format("Enter Claim Detail Failed", FunctionCode))
                End If

            End If

        End Sub

        Protected Overrides Sub Step2aClear()
            Me.udcStep2aInputEHSClaim.Clear()
            Me.udcReasonForVisit.Clear()
            Me.udcStep2aReadOnlyDocumnetType.Clear()
        End Sub

#End Region


#Region "Step 2a Validation for Enter Claim detail"
        '-------------------------------------------------------------------------------------------------------------------
        '   Validation
        '-------------------------------------------------------------------------------------------------------------------
        Protected Overrides Function Step2aHCVSValidation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
            Dim udtEHSAccount As EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(FunctionCode)
            Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
            Dim udcInputHCVS As UIControl.EHCClaimText.ucInputHCVS = Me.udcStep2aInputEHSClaim.GetHCVSControl()
            Dim udtValidator As Validator = New Validator()
            Dim udtEligibleResult As EligibleResult
            Dim isValid As Boolean = True
            Dim strDOB As String = String.Empty
            Dim systemMessage As SystemMessage
            Dim strMsgParam As String = String.Empty
            Dim udtSchemeClaim As SchemeClaimBLL = New SchemeClaimBLL()
            Dim udtSchemeClaimModel As SchemeClaimModel = udtSchemeClaim.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSTransaction.SchemeCode, udtEHSTransaction.ServiceDate)
            If udtSchemeClaimModel Is Nothing Then
                udtSchemeClaimModel = udtSchemeClaim.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSTransaction.SchemeCode, udtEHSTransaction.ServiceDate.AddDays(1).AddMinutes(-1))
            End If

            ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Dim udtVoucherInfo As New VoucherInfoModel(VoucherInfoModel.AvailableVoucher.Include, _
                                                       VoucherInfoModel.AvailableQuota.Include)
            Dim intAvailableVoucher As Integer = 0

            If isValid Then
                '----------------------------------------
                ' Concurrent Update Checking
                '----------------------------------------
                udtVoucherInfo.GetInfo(udtEHSTransaction.ServiceDate, udtSchemeClaimModel, udtEHSPersonalInfo, udtEHSTransaction.ServiceType)

                intAvailableVoucher = udtVoucherInfo.GetAvailableVoucher()

                If udtEHSAccount.VoucherInfo.GetAvailableVoucher() <> intAvailableVoucher Then
                    isValid = False
                    Me._blnConcurrentUpdate = True

                    Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.InternalError
                    Return False
                End If

            End If
            ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            udtEHSTransaction.DHCService = udcInputHCVS.DHCService()
            ' CRE19-006 (DHC) [End][Winnie]

            If Not checkByConfirmationBox Then

                ' -----------------------------------------------
                ' UI Input Validation
                '------------------------------------------------

                ' CRE19-006 (DHC) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                If Not udcInputHCVS.Validate(True, Me.udcMsgBoxErr) Then
                    isValid = False
                End If
                ' CRE19-006 (DHC) [End][Winnie]


                If isValid Then
                    ' --------------------------------------------------------------
                    ' Check Eligibility:
                    ' --------------------------------------------------------------
                    If udtEHSPersonalInfo.DocCode = DocTypeModel.DocTypeCode.EC AndAlso udtEHSPersonalInfo.ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                        strDOB = _udtFormatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, udtEHSPersonalInfo.ECAge, udtEHSPersonalInfo.ECDateOfRegistration)
                    Else
                        strDOB = _udtFormatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, Nothing, Nothing)
                    End If

                    systemMessage = Me._udtEHSClaimBLL.CheckEligibilityForEnterClaim(udtSchemeClaimModel, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo, Nothing, udtEligibleResult)

                    If Not systemMessage Is Nothing Then
                        ' If Check Eligibility Block Show Error
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage(systemMessage)
                    End If
                End If

                If isValid Then
                    ' --------------------------------------------------------------
                    ' Check Document Limit:
                    ' --------------------------------------------------------------
                    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                    'systemMessage = Me._udtEHSClaimBLL.CheckExceedDocumentLimitForEnterClaim(SchemeClaimModel.HCVS, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo)
                    systemMessage = Me._udtEHSClaimBLL.CheckExceedDocumentLimitForEnterClaim(udtEHSTransaction.SchemeCode, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo)
                    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
                    If Not systemMessage Is Nothing Then
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage(systemMessage)
                    End If
                End If

                If isValid Then
                    ' --------------------------------------------------------------
                    ' Check Benefit:
                    ' --------------------------------------------------------------
                    If intAvailableVoucher <= 0 OrElse intAvailableVoucher < udcInputHCVS.VoucherRedeem Then
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage("990000", "E", "00123")
                    End If
                End If

                ' CRE19-003 (Opt voucher capping) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                If isValid Then
                    ' --------------------------------------------------------------
                    ' Check Benefit (Quota for profession):
                    ' --------------------------------------------------------------

                    ' CRE19-003 (Opt voucher capping) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    Dim udtVoucherQuota As VoucherQuotaModel = udtVoucherInfo.VoucherQuotalist.FilterByProfCodeEffectiveDtm(udtEHSTransaction.ServiceType, udtEHSTransaction.ServiceDate)

                    If Not udtVoucherQuota Is Nothing Then
                        Dim intAvailableVoucherQuota As Integer = udtVoucherQuota.AvailableQuota
                        If intAvailableVoucherQuota <= 0 OrElse intAvailableVoucherQuota < udcInputHCVS.VoucherRedeem Then
                            isValid = False

                            Dim udtSM As SystemMessage = New SystemMessage("990000", "E", "00425")
                            Dim strMsg_en = String.Format(HttpContext.GetGlobalResourceObject("Text", "ProfessionQuota", New System.Globalization.CultureInfo(CultureLanguage.English)) _
                                                          , HttpContext.GetGlobalResourceObject("Text", udtVoucherQuota.ProfCode, New System.Globalization.CultureInfo(CultureLanguage.English)))

                            Dim strMsg_tc = String.Format(HttpContext.GetGlobalResourceObject("Text", "ProfessionQuota", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)) _
                                                          , HttpContext.GetGlobalResourceObject("Text", udtVoucherQuota.ProfCode, New System.Globalization.CultureInfo(CultureLanguage.TradChinese)))

                            udtSM.AddReplaceMessage("%en", strMsg_en)
                            udtSM.AddReplaceMessage("%tc", strMsg_tc)
                            Me.udcMsgBoxErr.AddMessage(udtSM)
                        End If
                    End If

                End If
                ' CRE19-003 (Opt voucher capping) [End][Winnie]

                If isValid Then
                    '----------------------------------------
                    ' Duplicate Claim Checking 
                    '----------------------------------------
                    udtEHSTransaction.VoucherClaim = udcInputHCVS.VoucherRedeem
                    If Me._udtEHSClaimBLL.CheckDuplicateClaim(udtEHSPersonalInfo, udtEHSTransaction) = True Then
                        isValid = False
                        Me.ShowConfirmMessageBox(ConfirmationStyle.Underline, DuplicateClaimAlertMessage)
                        EHSClaimBasePage.AuditLogTextOnlyVersionShowDuplicateClaimAlert(_udtAuditLogEntry)
                    End If
                End If
            End If
            ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [End][Winnie]

            If isValid Then
                udtEHSTransaction.VoucherClaim = udcInputHCVS.VoucherRedeem
                udtEHSTransaction.UIInput = udcInputHCVS.UIInput

                ' VisitReason Move to AdditionalField

                udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()

                ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
                ' -----------------------------------------------------------------------------------------

                Dim udtTransactAdditionfield As TransactionAdditionalFieldModel = New TransactionAdditionalFieldModel()

                If Me._udtGeneralFunction.IsCoPaymentFeeEnabled(udtEHSTransaction.ServiceDate) Then
                    If udcInputHCVS.CoPaymentFee.Trim <> String.Empty Then
                        udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                        udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.CoPaymentFee
                        'INT14-0012 (Fix Net Service Fee Changed lead-zero input and DB value) [Start][Karl]
                        udtTransactAdditionfield.AdditionalFieldValueCode = CInt(udcInputHCVS.CoPaymentFee)
                        'udtTransactAdditionfield.AdditionalFieldValueCode = udcInputHCVS.CoPaymentFee
                        'INT14-0012 (Fix Net Service Fee Changed lead-zero input and DB value) [End][Karl]
                        udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
                        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                        'udtTransactAdditionfield.SchemeCode = SchemeClaimModel.HCVS
                        'udtTransactAdditionfield.SchemeSeq = udtSchemeClaimModel.SchemeSeq
                        udtTransactAdditionfield.SchemeCode = udtSchemeClaimModel.SchemeCode
                        udtTransactAdditionfield.SchemeSeq = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SchemeSeq
                        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
                        udtTransactAdditionfield.SubsidizeCode = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeCode
                        udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
                    End If
                End If

                Dim strL1ValueCode As String = String.Empty
                Dim strL2ValueCode As String = String.Empty

                For i As Integer = 0 To TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1.Length - 1
                    Select Case i
                        Case 0
                            strL1ValueCode = udcInputHCVS.ReasonForVisitFirst
                            strL2ValueCode = udcInputHCVS.ReasonForVisitSecond
                        Case 1
                            strL1ValueCode = udcInputHCVS.ReasonForVisitFirst_S1
                            strL2ValueCode = udcInputHCVS.ReasonForVisitSecond_S1
                        Case 2
                            strL1ValueCode = udcInputHCVS.ReasonForVisitFirst_S2
                            strL2ValueCode = udcInputHCVS.ReasonForVisitSecond_S2
                        Case 3
                            strL1ValueCode = udcInputHCVS.ReasonForVisitFirst_S3
                            strL2ValueCode = udcInputHCVS.ReasonForVisitSecond_S3
                    End Select

                    ' Reason For Visit Level1
                    If strL1ValueCode.Trim <> String.Empty Then
                        udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                        udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1(i)
                        udtTransactAdditionfield.AdditionalFieldValueCode = strL1ValueCode
                        udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
                        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                        'udtTransactAdditionfield.SchemeCode = SchemeClaimModel.HCVS
                        'udtTransactAdditionfield.SchemeSeq = udtSchemeClaimModel.SchemeSeq
                        udtTransactAdditionfield.SchemeCode = udtSchemeClaimModel.SchemeCode
                        udtTransactAdditionfield.SchemeSeq = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SchemeSeq
                        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
                        udtTransactAdditionfield.SubsidizeCode = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeCode
                        udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

                        ' Reason For Visit Level2
                        ' CRE19-006 (DHC) [Start][Winnie]
                        ' ----------------------------------------------------------------------------------------
                        If strL2ValueCode <> String.Empty Then
                            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL2(i)
                            udtTransactAdditionfield.AdditionalFieldValueCode = strL2ValueCode
                            udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
                            udtTransactAdditionfield.SchemeCode = udtSchemeClaimModel.SchemeCode
                            udtTransactAdditionfield.SchemeSeq = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SchemeSeq
                            udtTransactAdditionfield.SubsidizeCode = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeCode
                            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
                        End If
                        ' CRE19-006 (DHC) [End][Winnie]
                    End If
                Next

                'CRE20-006 DHC integration [Start][Nichole]
                If udcInputHCVS.DHCCheckboxEnable Then
                    udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                    udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.DHCDistrictCode
                    udtTransactAdditionfield.AdditionalFieldValueCode = udcInputHCVS.DistrictCodeRadioBtn
                    udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
                    udtTransactAdditionfield.SchemeCode = udtSchemeClaimModel.SchemeCode
                    udtTransactAdditionfield.SchemeSeq = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SchemeSeq
                    udtTransactAdditionfield.SubsidizeCode = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeCode
                    udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
                End If
                'CRE20-006 DHC integration [End][Nichole]
            End If
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            Return isValid
        End Function

        'CRE13-019-02 Extend HCVS to China [Start][Karl]
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Protected Overrides Function Step2aHCVSChinaValidation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As EHSTransactionModel) As Boolean

            'Dim udtEHSAccount As EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(FunctionCode)
            'Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
            'Dim udcInputVoucherSlim As UIControl.EHCClaimText.ucInputVoucherSlim = Me.udcStep2aInputEHSClaim.GetVoucherSlimControl()
            'Dim udtValidator As Validator = New Validator()
            'Dim udtEligibleResult As EligibleResult
            'Dim isValid As Boolean = True
            'Dim strDOB As String = String.Empty
            'Dim systemMessage As SystemMessage
            'Dim strMsgParam As String = String.Empty
            'Dim udtSchemeClaim As SchemeClaimBLL = New SchemeClaimBLL()
            'Dim udtSchemeClaimModel As SchemeClaimModel = udtSchemeClaim.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSTransaction.SchemeCode, udtEHSTransaction.ServiceDate)
            'If udtSchemeClaimModel Is Nothing Then
            '    udtSchemeClaimModel = udtSchemeClaim.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSTransaction.SchemeCode, udtEHSTransaction.ServiceDate.AddDays(1).AddMinutes(-1))
            'End If

            'Dim udtEHSTransactionBLL As New EHSTransactionBLL()

            'Dim intAvailableVoucher As Integer = 0

            '' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [Start][Winnie]
            'If Not checkByConfirmationBox Then
            '    ' To Do: Use Available Voucher For Checking
            '    'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
            '    'systemMessage = udtValidator.chkVoucherRedeem(udcInputVoucherSlim.VoucherRedeem, udtEHSAccount.AvailableVoucher())
            '    systemMessage = udtValidator.chkVoucherRedeem(udcInputVoucherSlim.VoucherRedeem, udtEHSAccount.AvailableVoucher(), udtEHSTransaction.ServiceDate)
            '    'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]

            '    If Not systemMessage Is Nothing Then
            '        udcInputVoucherSlim.SetVoucherredeemError(True)
            '        isValid = False
            '        Me.udcMsgBoxErr.AddMessage(systemMessage)
            '    End If

            '    If isValid Then
            '        ' --------------------------------------------------------------
            '        ' Check Eligibility:
            '        ' --------------------------------------------------------------
            '        If udtEHSPersonalInfo.DocCode = DocTypeModel.DocTypeCode.EC AndAlso udtEHSPersonalInfo.ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
            '            strDOB = _udtFormatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, udtEHSPersonalInfo.ECAge, udtEHSPersonalInfo.ECDateOfRegistration)
            '        Else
            '            strDOB = _udtFormatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, Nothing, Nothing)
            '        End If

            '        systemMessage = Me._udtEHSClaimBLL.CheckEligibilityForEnterClaim(udtSchemeClaimModel, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo, Nothing, udtEligibleResult)

            '        If Not systemMessage Is Nothing Then
            '            ' If Check Eligibility Block Show Error
            '            isValid = False
            '            Me.udcMsgBoxErr.AddMessage(systemMessage)
            '        End If
            '    End If

            '    If isValid Then
            '        ' --------------------------------------------------------------
            '        ' Check Document Limit:
            '        ' --------------------------------------------------------------
            '        systemMessage = Me._udtEHSClaimBLL.CheckExceedDocumentLimitForEnterClaim(udtEHSTransaction.SchemeCode, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo)
            '        If Not systemMessage Is Nothing Then
            '            isValid = False
            '            Me.udcMsgBoxErr.AddMessage(systemMessage)
            '        End If
            '    End If

            '    If isValid Then
            '        ' --------------------------------------------------------------
            '        ' Check Benefit:
            '        ' --------------------------------------------------------------
            '        If udtEHSPersonalInfo.DocCode.Trim() = DocTypeModel.DocTypeCode.EC AndAlso udtEHSPersonalInfo.ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
            '            ' EC
            '            intAvailableVoucher = udtEHSTransactionBLL.getAvailableVoucher(udtEHSTransaction.ServiceDate, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, udtEHSPersonalInfo.ECAge, udtEHSPersonalInfo.ECDateOfRegistration, udtSchemeClaimModel)
            '        Else
            '            ' HKIC
            '            intAvailableVoucher = udtEHSTransactionBLL.getAvailableVoucher(udtEHSTransaction.ServiceDate, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, udtSchemeClaimModel)
            '        End If

            '        If intAvailableVoucher <= 0 OrElse intAvailableVoucher < udcInputVoucherSlim.VoucherRedeem Then
            '            isValid = False
            '            Me.udcMsgBoxErr.AddMessage("990000", "E", "00123")
            '        End If
            '    End If

            '    If isValid Then
            '        '----------------------------------------
            '        ' Duplicate Claim Checking 
            '        '----------------------------------------
            '        udtEHSTransaction.VoucherClaim = udcInputVoucherSlim.VoucherRedeem
            '        If Me._udtEHSClaimBLL.CheckDuplicateClaim(udtEHSPersonalInfo, udtEHSTransaction) = True Then
            '            isValid = False
            '            Me.ShowConfirmMessageBox(ConfirmationStyle.Underline, DuplicateClaimAlertMessage)
            '            EHSClaimBasePage.AuditLogTextOnlyVersionShowDuplicateClaimAlert(_udtAuditLogEntry)
            '        End If
            '    End If
            'End If
            '' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [End][Winnie]

            'If isValid Then

            '    udtEHSTransaction.VoucherClaim = udcInputVoucherSlim.VoucherRedeem
            '    udtEHSTransaction.UIInput = udcInputVoucherSlim.UIInput

            'End If

            'Return isValid
            ''CRE13-019-02 Extend HCVS to China [End][Karl]
        End Function
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

#End Region


#Region "Step of Step 2b Confirm Claim detail"

        '-------------------------------------------------------------------------------------------------------------------
        'Events
        '-------------------------------------------------------------------------------------------------------------------
        Protected Sub btnStep2bBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStep2bBack.Click
            If (MyBase.IsPageRefreshed) Then
                _blnIsRequireHandlePageRefresh = True
                Return
            End If

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
            ' -----------------------------------------------------------------------------------------

            'SessionHandler.EHSTransactionRemoveFromSession(FunctionCode)

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            Me.SetStep2aInputEHSClaimControlReadFromSession(True)
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step2a
        End Sub

        Protected Sub btnStep2bConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStep2bConfirm.Click
            If (MyBase.IsPageRefreshed) Then
                _blnIsRequireHandlePageRefresh = True
                Return
            End If

            ' To Handle Concurrent Browser
            If Not Me.EHSClaimTokenNumValidation(hfEHSClaimTokenNum.Value, FunctionCode) Then
                _blnIsConcurrentBrowserDetected = True
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.InternalError
                Return
            End If

            Dim isValid As Boolean = Step2bValidate()

            ' Check the input is correct. if correct, go to confrim declaration page
            If isValid Then
                ' Go to Declaration page
                Me.ShowConfirmMessageBox(ConfirmationStyle.BorderedUnderlineHighlight, "ProvidedInfoTrueClaimSP")

            ElseIf Me.udcMsgBoxErr.GetCodeTable().Rows.Count > 0 Then
                Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail)
            Else
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.InternalError
            End If

        End Sub

        Protected Sub udcStep2bReadOnlyEHSClaim_VaccineRemarkClicked(ByVal sender As Object, ByVal e As EventArgs) Handles udcStep2bReadOnlyEHSClaim.VaccineRemarkClicked
            If (MyBase.IsPageRefreshed) Then
                _blnIsRequireHandlePageRefresh = True
                Return
            End If

            ShowVaccineRemarkText()
        End Sub

        Protected Sub btnStep2bPrintClaimConsentForm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnStep2bPrintClaimConsentForm.Click
            If (MyBase.IsPageRefreshed) Then
                _blnIsRequireHandlePageRefresh = True
                Return
            End If

            Dim udtEHSTransaction As EHSTransactionModel = SessionHandler.EHSTransactionGetFromSession(FunctionCode)
            Dim strPrintOptionSelectedValue As String = Me.rbStep2bPrintClaimConsentFormLanguage.SelectedValue

            'CRE15-003 System-generated Form [Start][Philip Chau]
            Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
            'CRE15-003 System-generated Form [End][Philip Chau]

            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            'Dim strCurrentUserPrintOption As String = Me.GetCurrentUserPrintOption()
            Dim strCurrentPrintOption As String = Me.hfCurrentPrintOption.Value
            'CRE13-019-02 Extend HCVS to China [End][Winnie]

            Dim strPrintDateTime As String = String.Format("DH_HCV103{0}{1}{2}{3}{4}{5}{6}", Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, Now.Second, Now.Millisecond)
            udtEHSTransaction.PrintedConsentForm = True

            'Set the transaction is printed consent Form
            SessionHandler.EHSTransactionSaveToSession(udtEHSTransaction, FunctionCode)

            'Save the current function code to session (will be removed in the printout form)
            SessionHandler.EHSClaimPrintoutFunctionCodeSaveToSession(FunctionCode)

            'CRE15-003 System-generated Form [Start][Philip Chau]
            MyBase.SessionHandler.EHSClaimTempTransactionIDSaveToSession(Me._udtGeneralFunction.generateTemporaryTransactionNumber(udtSchemeClaim.SchemeCode.Trim()))
            'CRE15-003 System-generated Form [End][Philip Chau]

            Select Case strPrintOptionSelectedValue
                'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                'Case PrintOptionValue.Chi
                Case PrintOptionLanguage.TradChinese
                    If strCurrentPrintOption = Common.Component.PrintFormOptionValue.PrintPurposeAndConsent Then
                        PrintPrintout(PrintOptionValue.FullChi, False)
                    ElseIf strCurrentPrintOption = Common.Component.PrintFormOptionValue.PrintConsentOnly Then
                        PrintPrintout(PrintOptionValue.CondensedChi, False)
                    End If

                    'Case PrintOptionValue.Eng
                Case PrintOptionLanguage.English
                    If strCurrentPrintOption = Common.Component.PrintFormOptionValue.PrintPurposeAndConsent Then
                        PrintPrintout(PrintOptionValue.FullEng, False)
                    ElseIf strCurrentPrintOption = Common.Component.PrintFormOptionValue.PrintConsentOnly Then
                        PrintPrintout(PrintOptionValue.CondensedEng, False)
                    End If

                Case PrintOptionLanguage.SimpChinese
                    If strCurrentPrintOption = Common.Component.PrintFormOptionValue.PrintPurposeAndConsent Then
                        PrintPrintout(PrintOptionValue.FullSimpChi, False)
                    ElseIf strCurrentPrintOption = Common.Component.PrintFormOptionValue.PrintConsentOnly Then
                        PrintPrintout(PrintOptionValue.CondensedSimpChi, False)
                    End If
                    'CRE13-019-02 Extend HCVS to China [End][Winnie]
            End Select

        End Sub

        Protected Sub btnStep2bAdhocPrintConsentForm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnStep2bAdhocPrintConsentForm.Click
            If (MyBase.IsPageRefreshed) Then
                _blnIsRequireHandlePageRefresh = True
                Return
            End If
            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            'Me.rbPrintFullChi.Checked = False
            'Me.rbPrintFull.Checked = False
            'Me.rbPrintCondencedChi.Checked = True
            'Me.rbPrintCondenced.Checked = False

            Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode)

            Dim slConsentFormAvailableLang As String() = Nothing
            Dim strConsentFormAvailableVersion As String = Nothing

            slConsentFormAvailableLang = udtSchemeClaim.SubsidizeGroupClaimList(0).ConsentFormAvailableLang
            strConsentFormAvailableVersion = udtSchemeClaim.SubsidizeGroupClaimList(0).ConsentFormAvailableVersion

            Me.tbAdhocPrintFull.Visible = False
            Me.tbAdhocPrintCondense.Visible = False

            Dim strLanguageText As String = ""

            Dim count As Integer = 1

            'initial language setting
            Me.rbAdhocPrintFullLang1.Visible = False
            Me.rbAdhocPrintFullLang2.Visible = False
            Me.rbAdhocPrintFullLang3.Visible = False
            Me.rbAdhocPrintCondenseLang1.Visible = False
            Me.rbAdhocPrintCondenseLang2.Visible = False
            Me.rbAdhocPrintCondenseLang3.Visible = False

            Me.rbAdhocPrintFullLang1.Checked = False
            Me.rbAdhocPrintFullLang2.Checked = False
            Me.rbAdhocPrintFullLang3.Checked = False
            Me.rbAdhocPrintCondenseLang1.Checked = False
            Me.rbAdhocPrintCondenseLang2.Checked = False
            Me.rbAdhocPrintCondenseLang3.Checked = False

            If strConsentFormAvailableVersion IsNot Nothing Then
                For Each language As String In slConsentFormAvailableLang
                    Select Case language
                        Case PrintOptionLanguage.English
                            strLanguageText = Me.GetGlobalResourceObject("Text", "English")
                        Case PrintOptionLanguage.TradChinese
                            strLanguageText = Me.GetGlobalResourceObject("Text", "Chinese")
                        Case PrintOptionLanguage.SimpChinese
                            strLanguageText = Me.GetGlobalResourceObject("Text", "SimpChinese")
                    End Select

                    If count = 1 Then
                        Me.rbAdhocPrintFullLang1.Visible = True
                        Me.rbAdhocPrintCondenseLang1.Visible = True

                        Me.rbAdhocPrintFullLang1.Text = strLanguageText
                        Me.rbAdhocPrintCondenseLang1.Text = strLanguageText

                    ElseIf count = 2 Then
                        Me.rbAdhocPrintFullLang2.Visible = True
                        Me.rbAdhocPrintCondenseLang2.Visible = True

                        Me.rbAdhocPrintFullLang2.Text = strLanguageText
                        Me.rbAdhocPrintCondenseLang2.Text = strLanguageText
                    ElseIf count = 3 Then
                        Me.rbAdhocPrintFullLang3.Visible = True
                        Me.rbAdhocPrintCondenseLang3.Visible = True

                        Me.rbAdhocPrintFullLang3.Text = strLanguageText
                        Me.rbAdhocPrintCondenseLang3.Text = strLanguageText
                    End If

                    count += 1

                Next

                'Default the first item
                Select Case strConsentFormAvailableVersion
                    Case PrintFormAvailableVersion.Both
                        Me.tbAdhocPrintFull.Visible = True
                        Me.tbAdhocPrintCondense.Visible = True

                        If slConsentFormAvailableLang IsNot Nothing Then Me.rbAdhocPrintCondenseLang1.Checked = True

                    Case PrintFormAvailableVersion.Full
                        Me.tbAdhocPrintFull.Visible = True
                        If slConsentFormAvailableLang IsNot Nothing Then Me.rbAdhocPrintFullLang1.Checked = True

                    Case PrintFormAvailableVersion.Condense
                        Me.tbAdhocPrintCondense.Visible = True
                        If slConsentFormAvailableLang IsNot Nothing Then Me.rbAdhocPrintCondenseLang1.Checked = True

                End Select

            End If
            'CRE13-019-02 Extend HCVS to China [End][Winnie]

            ' Go to select ad-hoc printing page
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.AdHocPrint

        End Sub

        Protected Sub btnStep2bChangePrintOption_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnStep2bChangePrintOption.Click
            If (MyBase.IsPageRefreshed) Then
                _blnIsRequireHandlePageRefresh = True
                Return
            End If

            ' Get the current user's print option
            SessionHandler.CurrentUserGetFromSession(Me._udtSP, Me._udtDataEntryModel)

            If Me._udtDataEntryModel Is Nothing Then
                Me.udtPrintOptionSelection.setSelectedValue(Me._udtSP.PrintOption)
                EHSClaimBasePage.AuditLogSelectPrintOption(_udtAuditLogEntry, Me._udtSP.PrintOption)
            Else
                Me.udtPrintOptionSelection.setSelectedValue(Me._udtDataEntryModel.PrintOption)
                EHSClaimBasePage.AuditLogSelectPrintOption(_udtAuditLogEntry, Me._udtDataEntryModel.PrintOption)
            End If

            Me.udtPrintOptionSelection.setTitle(Me.GetGlobalResourceObject("Text", "SelectPrintFormOption"))

            ' Go to Change Print option page
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.PrintOption

        End Sub

        '-------------------------------------------------------------------------------------------------------------------
        'Functions
        '-------------------------------------------------------------------------------------------------------------------
        Protected Overrides Sub SetupStep2b(ByVal udtEHSAccount As EHSAccountModel, ByVal blnInitAll As Boolean)
            Dim udtEHSTransaction As EHSTransactionModel = MyBase.SessionHandler.EHSTransactionGetFromSession(FunctionCode)
            Dim udtSelectedPracticeDisplay As BLL.PracticeDisplayModel = MyBase.SessionHandler.PracticeDisplayGetFromSession(FunctionCode)
            Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
            Dim strPrintOption As String = String.Empty

            'ToDo: The following code cause problem in Keeping the state. uncomment first, and check
            ''Clear all control which in Enter Claim input, for viewstate problem
            '' Me.udcStep2aInputEHSClaim.Clear()

            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            Dim blnConsentFormAvailability As Boolean = False
            Dim blnPrintOptionAvailability As Boolean = False
            Dim slConsentFormAvailableLang As String() = Nothing
            Dim strConsentFormAvailableVersion As String = Nothing

            Me.hfCurrentPrintOption.Value = Nothing

            blnConsentFormAvailability = udtSchemeClaim.SubsidizeGroupClaimList(0).ConsentFormAvailable
            blnPrintOptionAvailability = udtSchemeClaim.SubsidizeGroupClaimList(0).PrintOptionAvailable
            slConsentFormAvailableLang = udtSchemeClaim.SubsidizeGroupClaimList(0).ConsentFormAvailableLang
            strConsentFormAvailableVersion = udtSchemeClaim.SubsidizeGroupClaimList(0).ConsentFormAvailableVersion
            'CRE13-019-02 Extend HCVS to China [End][Winnie]

            ' To Handle Concurrent Browser
            If Not Me.EHSClaimTokenNumValidation(hfEHSClaimTokenNum.Value, FunctionCode) Then
                _blnIsConcurrentBrowserDetected = True
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.InternalError
                Return
            End If

            MyBase.SessionHandler.CurrentUserGetFromSession(Me._udtSP, Me._udtDataEntryModel)

            ' INT13-0022 - Fix some special handling on HCSP text only version [Start][Koala]
            ' -------------------------------------------------------------------------------------
            ' Check the browser is from Mobile device, hide the printing stuff if is mobile
            ' if the first subsidize need print form -> this scheme need to print form 
            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            'If udtSchemeClaim.SubsidizeGroupClaimList(0).ConsentFormAvailable Then
            If blnConsentFormAvailability Then
                'CRE13-019-02 Extend HCVS to China [End][Winnie]
                'If MyBase.IsSupportedDevice AndAlso udtSchemeClaim.SubsidizeGroupClaimList(0).ConsentFormAvailable Then
                ' INT13-0022 - Fix some special handling on HCSP text only version [End][Koala]
                Me.panStep2bPrintClaimConsentForm.Visible = True

                'Set up print Option
                If Me._udtDataEntryModel Is Nothing Then
                    strPrintOption = Me._udtSP.PrintOption
                Else
                    strPrintOption = Me._udtDataEntryModel.PrintOption
                End If

                'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                If blnPrintOptionAvailability Then
                    'Set up print Option
                    Me.btnStep2bChangePrintOption.Visible = True
                    Me.udtPrintOptionSelection.setPrintOption(strConsentFormAvailableVersion)
                Else
                    Me.btnStep2bChangePrintOption.Visible = False
                End If

                Dim udtConsentFormPrintOption As New HCSP.BLL.ConsentFormPrintOptionBLL
                strPrintOption = udtConsentFormPrintOption.GetCurrentPrintOption(blnPrintOptionAvailability, strConsentFormAvailableVersion, Me.GetCurrentUserPrintOption())

                Me.PrintClaimConsentFormLanguageSetup(slConsentFormAvailableLang)
                'CRE13-019-02 Extend HCVS to China [End][Winnie]    

                Me.ChangePrintFormControl(strPrintOption, udtSchemeClaim)
                Me.HandlePrintOptionChanged()

            Else
                'Not available for print
                Me.panStep2bPrintClaimConsentForm.Visible = False
                Me.btnStep2bAdhocPrintConsentForm.Visible = False
                Me.btnStep2bConfirm.Visible = True
            End If

            'CRE13-019-02 Extend HCVS to China [Start][Winnie]            
            Me.hfCurrentPrintOption.Value = strPrintOption
            'CRE13-019-02 Extend HCVS to China [End][Winnie]     

            ' Hide Error label
            lblStep2bPrintFormError.Visible = False

            'Setup Lable Value 
            If MyBase.SessionHandler.Language() = CultureLanguage.TradChinese Then
                Me.lblStep2bScheme.Text = udtSchemeClaim.SchemeDescChi
                Me.lblStep2bServiceType.Text = udtEHSTransaction.ServiceTypeDesc_Chi
                Me.lblStep2bPractice.Text = String.Format("{0} ({1})", udtSelectedPracticeDisplay.PracticeNameChi, udtSelectedPracticeDisplay.PracticeID)
                Me.lblStep2bPractice.CssClass = "tableTextChi"
            Else
                Me.lblStep2bScheme.Text = udtSchemeClaim.SchemeDesc
                Me.lblStep2bServiceType.Text = udtEHSTransaction.ServiceTypeDesc
                Me.lblStep2bPractice.Text = String.Format("{0} ({1})", udtSelectedPracticeDisplay.PracticeName, udtSelectedPracticeDisplay.PracticeID)
                Me.lblStep2bPractice.CssClass = "tableText"
            End If
            Me.lblStep2bBankAcct.Text = _udtFormatter.maskBankAccount(udtEHSTransaction.BankAccountNo)
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim udtSubPlatformBLL As New SubPlatformBLL
            'Me.lblStep2bServiceDate.Text = _udtFormatter.formatDate(udtEHSTransaction.ServiceDate, MyBase.SessionHandler.Language())
            Me.lblStep2bServiceDate.Text = _udtFormatter.formatInputTextDate(udtEHSTransaction.ServiceDate, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            'Setup Personal information 
            Me.udcStep2bReadOnlyDocumnetType.TextOnlyVersion = True
            ' INT13-0021 - Fix use HKBC on smart IC claim incorrectly [Start][Koala]
            ' -------------------------------------------------------------------------------------
            Me.udcStep2bReadOnlyDocumnetType.DocumentType = udtEHSAccount.SearchDocCode
            'Me.udcStep2bReadOnlyDocumnetType.DocumentType = udtEHSAccount.EHSPersonalInformationList(0).DocCode.Trim()
            ' INT13-0021 - Fix use HKBC on smart IC claim incorrectly [End][Koala]
            Me.udcStep2bReadOnlyDocumnetType.EHSAccount = udtEHSAccount
            Me.udcStep2bReadOnlyDocumnetType.Vertical = False
            Me.udcStep2bReadOnlyDocumnetType.MaskIdentityNo = True
            Me.udcStep2bReadOnlyDocumnetType.ShowAccountRefNo = False
            Me.udcStep2bReadOnlyDocumnetType.ShowTempAccountNotice = False
            Me.udcStep2bReadOnlyDocumnetType.ShowAccountCreationDate = False
            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Me.udcStep2bReadOnlyDocumnetType.SetEnableToShowHKICSymbol = True
            ' CRE17-010 (OCSSS integration) [End][Chris YIM]
            Me.udcStep2bReadOnlyDocumnetType.Built()

            'setup Transaction detail
            Me.udcStep2bReadOnlyEHSClaim.TextOnlyVersion = True
            Me.udcStep2bReadOnlyEHSClaim.EHSClaimVaccine = MyBase.SessionHandler.EHSClaimVaccineGetFromSession()
            Me.udcStep2bReadOnlyEHSClaim.EHSTransaction = udtEHSTransaction
            Me.udcStep2bReadOnlyEHSClaim.SchemeCode = udtSchemeClaim.SchemeCode
            Me.udcStep2bReadOnlyEHSClaim.Mode = ucReadOnlyEHSClaim.ReadOnlyEHSClaimMode.Normal
            Me.udcStep2bReadOnlyEHSClaim.Built()
        End Sub

        Protected Overrides Sub Step2bClear()

        End Sub

        Private Function Step2bValidate() As Boolean
            Dim udtEHSTransaction As EHSTransactionModel = MyBase.SessionHandler.EHSTransactionGetFromSession(FunctionCode)
            Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
            Dim udtEHSAccount As EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(FunctionCode)
            Dim udtTransactionBLL As EHSTransactionBLL = New EHSTransactionBLL()
            Dim isValid As Boolean = True

            ' To Handle Concurrent Browser
            If Me._blnIsConcurrentBrowserDetected Then
                Return False
            End If

            '----------------------------------------
            ' Case 2 Checking
            '----------------------------------------
            If Not udtEHSTransaction.IsNew OrElse Not String.IsNullOrEmpty(udtEHSTransaction.TransactionID) Then
                ' Check the Transaction have been Saved or not
                ' When the Transaction contain TransactoinID, it should have been saved into DB. should not appear in this step
                Return False
            End If

            '----------------------------------------
            ' Case 1 Checking
            '----------------------------------------
            If isValid Then
                If udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVoucher Then
                    ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                    Dim udtVoucherInfo As New VoucherInfoModel(VoucherInfoModel.AvailableVoucher.Include, _
                                                               VoucherInfoModel.AvailableQuota.None)

                    udtVoucherInfo.GetInfo(udtEHSTransaction.ServiceDate, MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode), _
                                           udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode))

                    Dim intLatestAvailableVoucher As Integer = udtVoucherInfo.GetAvailableVoucher()

                    If udtEHSAccount.VoucherInfo.GetAvailableVoucher <> intLatestAvailableVoucher Then
                        isValid = False
                        Me._blnConcurrentUpdate = True
                    End If
                    ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

                End If
            End If

            MyBase.SessionHandler.CurrentUserGetFromSession(Me._udtSP, Me._udtDataEntryModel)

            ' Check the browser is from Mobile device
            ' if the first subsidize need print form -> this scheme need to print form 
            ' INT13-0022 - Fix some special handling on HCSP text only version [Start][Koala]
            ' -------------------------------------------------------------------------------------
            If Not IsPrePrintDocument() _
                AndAlso udtSchemeClaim.SubsidizeGroupClaimList(0).ConsentFormAvailable Then
                ' check the printform have been printed
                If udtEHSTransaction.PrintedConsentForm = False Then
                    ' Error
                    isValid = False
                    lblStep2bPrintFormError.Visible = True
                    Me.udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00148"))
                End If
            End If
            ' INT13-0022 - Fix some special handling on HCSP text only version [End][Koala]

            Return isValid

        End Function

        Private Sub Step2bClaimSubmit()
            Dim systemMessage As SystemMessage = Nothing
            Dim udtFormatter As Formatter = New Formatter()
            Dim udtEHSTransaction As EHSTransactionModel = MyBase.SessionHandler.EHSTransactionGetFromSession(FunctionCode)
            Dim udtSelectedPracticeDisplay As BLL.PracticeDisplayModel = MyBase.SessionHandler.PracticeDisplayGetFromSession(FunctionCode)
            Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
            Dim udtEHSClaimVaccine As EHSClaimVaccineModel = MyBase.SessionHandler.EHSClaimVaccineGetFromSession()
            Dim udtEHSAccount As EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(FunctionCode)
            Dim udtTransactionBLL As EHSTransactionBLL = New EHSTransactionBLL()
            Dim udtSmartIDContent As BLL.SmartIDContentModel = MyBase.SessionHandler.SmartIDContentGetFormSession(FunctionCode)
            Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode.Trim())

            Dim strOrignalEHSAccountID As String = Nothing
            Dim blnVaccineType As Boolean = True
            Dim blnIsRecordOutdated As Boolean = False
            Dim isCreateBySmartID As Boolean = False
            Dim blnCreateAdment As Boolean = False
            Dim isValid As Boolean = True

            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Dim strSmartIDVer As String = String.Empty
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

            EHSClaimBasePage.AuditLogConfirmClaimDetailStart(_udtAuditLogEntry, MyBase.SessionHandler.SmartIDContentGetFormSession(FunctionCode))

            If isValid Then
                isValid = Step2bValidate()
            End If


            If isValid Then

                '==================================================================== Code for SmartID ============================================================================
                '----------------------------------------
                ' Check if is create by smart IC
                '----------------------------------------
                If Not udtSmartIDContent Is Nothing AndAlso udtSmartIDContent.IsReadSmartID Then
                    isCreateBySmartID = True
                    udtEHSTransaction.CreateBySmartID = True

                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    strSmartIDVer = udtSmartIDContent.SmartIDVer
                    udtEHSTransaction.SmartIDVer = strSmartIDVer
                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
                End If
                '===================================================================================================================================================================

                udtEHSTransaction.TransactionID = Me._udtCommfunct.generateTransactionNumber(udtSchemeClaim.SchemeCode.Trim())

                If Not udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                    If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.SpecialAccount Then
                        strOrignalEHSAccountID = udtEHSAccount.OriginalAccID

                    ElseIf (Not udtEHSAccount.TransactionID Is Nothing AndAlso Not udtEHSAccount.TransactionID.Equals(String.Empty)) _
                        OrElse (Not udtEHSAccount.CreateSPID Is Nothing AndAlso Not udtEHSAccount.CreateSPID.Equals(Me._udtSP.SPID)) Then

                        strOrignalEHSAccountID = udtEHSAccount.VoucherAccID
                    End If
                End If

                'case of X account 
                '1: Account is special account
                '2: Account with transaction 
                '3: Account is created by other Service Provider
                If Not strOrignalEHSAccountID Is Nothing OrElse (Not udtSmartIDContent Is Nothing AndAlso udtSmartIDContent.IsCreateAmendEHSAccount) Then

                    If Not udtSmartIDContent Is Nothing AndAlso udtSmartIDContent.IsCreateAmendEHSAccount Then
                        '==================================================================== Code for SmartID ============================================================================
                        '--------------------------------------------------------------------------------------------------------------------------------------
                        ' Case of create by SmartID -> validated Acocunt change personal particular -> need assign value to create a C account 
                        '--------------------------------------------------------------------------------------------------------------------------------------

                        'Select Case udtSmartIDContent.SmartIDReadStatus
                        '    Case BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_DiffDOI_LargerDOI, _
                        '                BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_NotCreateBySmartID_SameDOB
                        blnCreateAdment = True
                        'End Select
                        '===================================================================================================================================================================
                    Else
                        udtEHSAccount = udtEHSAccount.CloneData()
                        udtEHSAccount.OriginalAccID = strOrignalEHSAccountID
                        udtEHSAccount.VoucherAccID = Me._udtCommfunct.generateSystemNum("X")

                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                        ' ----------------------------------------------------------------------------------------
                        If isCreateBySmartID Then
                            udtEHSAccount.EHSPersonalInformationList(0).CreateBySmartID = True
                            udtEHSAccount.EHSPersonalInformationList(0).SmartIDVer = strSmartIDVer
                        Else
                            udtEHSAccount.EHSPersonalInformationList(0).CreateBySmartID = False
                            udtEHSAccount.EHSPersonalInformationList(0).SmartIDVer = String.Empty
                        End If
                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
                    End If

                    If udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVaccine Then
                        Me._udtEHSClaimBLL.ConstructEHSTransactionDetails(Me._udtSP, Me._udtDataEntryModel, udtEHSTransaction, udtEHSAccount, udtEHSClaimVaccine)
                        ' CRE13-001 - EHAPP [Start][Tommy L]
                        ' -------------------------------------------------------------------------------------
                    ElseIf udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeRegistration Then
                        blnVaccineType = False
                        Me._udtEHSClaimBLL.ConstructEHSTransDetail_Registration(Me._udtSP, Me._udtDataEntryModel, udtEHSTransaction, udtEHSAccount)
                        ' CRE13-001 - EHAPP [End][Tommy L]
                    Else
                        blnVaccineType = False
                        Me._udtEHSClaimBLL.ConstructEHSTransactionDetails(Me._udtSP, Me._udtDataEntryModel, udtEHSTransaction, udtEHSAccount)
                    End If

                    Try
                        If blnCreateAdment Then
                            systemMessage = Me._udtEHSClaimBLL.CreateAmendEHSAccountEHSTransaction(Me._udtSP, Me._udtDataEntryModel, udtEHSTransaction, udtSmartIDContent.EHSValidatedAccount, udtSmartIDContent.EHSAccount, udtSchemeClaim, EHSTransactionModel.AppSourceEnum.WEB_TEXT)
                        Else
                            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                            '-----------------------------------------------------------------------------------------
                            systemMessage = Me._udtEHSClaimBLL.CreateXEHSAccountEHSTransaction(Me._udtSP, Me._udtDataEntryModel, udtEHSTransaction, udtEHSAccount, udtSchemeClaim, EHSTransactionModel.AppSourceEnum.WEB_TEXT)
                            'CRE16-002 (Revamp VSS) [End][Chris YIM]
                        End If

                    Catch eSql As SqlClient.SqlException
                        If eSql.Number = 50000 Then
                            isValid = False
                            Me.udcMsgBoxErr.AddMessage(New SystemMessage("990001", "D", eSql.Message))
                        Else
                            Throw eSql
                        End If
                    End Try

                    If Not systemMessage Is Nothing Then
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage(systemMessage)
                        'The current update is aborted since the request has been submit more than once or the record(s) is updated by other.
                        If systemMessage.MessageCode = "00197" Then
                            blnIsRecordOutdated = True
                        End If
                    Else
                        If blnCreateAdment Then
                            'udtSmartIDContent.EHSAccount is retrieved form database after Admentment Record was created 
                            MyBase.SessionHandler.EHSAccountSaveToSession(udtSmartIDContent.EHSAccount, FunctionCode)
                            MyBase.SessionHandler.SmartIDContentSaveToSession(FunctionCode, udtSmartIDContent)
                        End If
                    End If

                Else

                    If udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVaccine Then
                        Me._udtEHSClaimBLL.ConstructEHSTransactionDetails(Me._udtSP, Me._udtDataEntryModel, udtEHSTransaction, udtEHSAccount, udtEHSClaimVaccine)
                        ' CRE13-001 - EHAPP [Start][Tommy L]
                        ' -------------------------------------------------------------------------------------
                    ElseIf udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeRegistration Then
                        blnVaccineType = False
                        Me._udtEHSClaimBLL.ConstructEHSTransDetail_Registration(Me._udtSP, Me._udtDataEntryModel, udtEHSTransaction, udtEHSAccount)
                        ' CRE13-001 - EHAPP [End][Tommy L]
                    Else
                        blnVaccineType = False
                        Me._udtEHSClaimBLL.ConstructEHSTransactionDetails(Me._udtSP, Me._udtDataEntryModel, udtEHSTransaction, udtEHSAccount)
                    End If

                    Try

                        Dim blnAutoConfirm As Boolean = False
                        ' ---------------------------------------------------------------------------------------------------------------------------------------
                        ' 1. Temporary EHS Account Create By DataEntry of Same SPID, and the record status = Pending Confirmation, no transaction follow and once SP reuse this account, Auto Confirm
                        ' ---------------------------------------------------------------------------------------------------------------------------------------
                        ' ---------------------------------------------------------------------------------------------------------------------------------------
                        ' 2. Other Scenior will not auto confirm the EHSAccount
                        ' ---------------------------------------------------------------------------------------------------------------------------------------
                        If Me._udtDataEntryModel Is Nothing AndAlso udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.TemporaryAccount _
                            AndAlso udtEHSAccount.CreateSPID.Trim().Equals(Me._udtSP.SPID.Trim()) AndAlso udtEHSAccount.DataEntryBy.Trim() <> "" _
                            AndAlso udtEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingConfirmation Then
                            blnAutoConfirm = True
                        End If
                        systemMessage = Me._udtEHSClaimBLL.CreateEHSTransaction(udtEHSTransaction, udtEHSAccount, udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode), udtSchemeClaim, EHSTransactionModel.AppSourceEnum.WEB_TEXT, blnAutoConfirm)
                        ' systemMessage = Me._udtEHSClaimBLL.CreateEHSTransaction(udtEHSTransaction, udtEHSAccount, udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode), udtSchemeClaim)

                        If Not systemMessage Is Nothing Then
                            isValid = False
                            Me.udcMsgBoxErr.AddMessage(systemMessage)

                            'The current update is aborted since the request has been submit more than once or the record(s) is updated by other.
                            If systemMessage.MessageCode = "00197" Then
                                blnIsRecordOutdated = True
                            End If
                        End If
                    Catch eSql As SqlClient.SqlException
                        If eSql.Number = 50000 Then
                            isValid = False
                            Me.udcMsgBoxErr.AddMessage(New SystemMessage("990001", "D", eSql.Message))
                        Else
                            Throw eSql
                        End If
                    End Try

                End If
            End If

            If isValid Then
                If Not udtSmartIDContent Is Nothing AndAlso udtSmartIDContent.IsReadSmartID Then
                    Select Case udtSmartIDContent.SmartIDReadStatus
                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                        ' ----------------------------------------------------------------------------------------
                        Case BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_DiffDOI_LargerDOI, _
                                    BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_NotCreateBySmartID_SameDOB, _
                                    BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOIDOB_CreateBySmartID_WithoutGender_SameName
                            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                            udtSmartIDContent.IsCreateAmendEHSAccount = True
                            MyBase.SessionHandler.SmartIDContentSaveToSession(FunctionCode, udtSmartIDContent)
                    End Select
                End If

                'if Success
                udtEHSTransaction = udtTransactionBLL.LoadEHSTransaction(udtEHSTransaction.TransactionID)
                MyBase.SessionHandler.EHSTransactionSaveToSession(udtEHSTransaction, FunctionCode)

                If blnVaccineType Then
                    udtEHSClaimVaccine = Me._udtEHSClaimBLL.ConstructEHSClaimVaccineModel(udtEHSTransaction.SchemeCode, udtEHSTransaction)
                    MyBase.SessionHandler.EHSClaimVaccineSaveToSession(udtEHSClaimVaccine)
                End If

                EHSClaimBasePage.AuditLogConfirmClaimDetailPassed(_udtAuditLogEntry, FunctionCode, udtEHSTransaction, udtSmartIDContent, blnCreateAdment)

                Me.Step2aClear()
                Me.Step2bClear()

                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step3
            ElseIf blnIsRecordOutdated = False AndAlso Me.udcMsgBoxErr.GetCodeTable().Rows.Count > 0 Then
                ' Return To Step2b when error / validation fail
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step2b

                EHSClaimBasePage.AuditLogConfirmClaimDetailFailed(_udtAuditLogEntry, FunctionCode)
                Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail)
            Else
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.InternalError
            End If
        End Sub

#End Region


#Region "Step of Step 3 Complete claim"

        '-------------------------------------------------------------------------------------------------------------------
        'Events
        '-------------------------------------------------------------------------------------------------------------------
        Protected Sub btnStep3NextClaim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStep3NextClaim.Click
            If (MyBase.IsPageRefreshed) Then
                _blnIsRequireHandlePageRefresh = True
                Return
            End If

            ' Log Next Claim Clicked
            EHSClaimBasePage.AuditLogNextClaim(_udtAuditLogEntry)

            ' Clear previous account and go to step1
            Me.Clear()

            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Me.SessionHandler.HKICSymbolRemoveFromSession(FunctionCode)
            Me.SessionHandler.OCSSSRefStatusRemoveFromSession(FunctionCode)
            ' CRE17-010 (OCSSS integration) [End][Chris YIM]

            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
        End Sub

        Protected Sub btnStep3ClaimForSamePatient_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStep3ClaimForSamePatient.Click
            If (MyBase.IsPageRefreshed) Then
                _blnIsRequireHandlePageRefresh = True
                Return
            End If

            Dim udtSchemeClaim As SchemeClaimModel = SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
            Dim udtEHSAccount As EHSAccountModel = SessionHandler.EHSAccountGetFromSession(FunctionCode)
            Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
            Dim strDOB As String = _udtFormatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, SessionHandler.Language(), udtEHSPersonalInfo.ECAge, udtEHSPersonalInfo.ECDateOfRegistration)
            Dim udtSmartIDContent As BLL.SmartIDContentModel = MyBase.SessionHandler.SmartIDContentGetFormSession(FunctionCode)

            Dim blnNotMatchAccountExist As Boolean
            Dim blnExceedDocTypeLimit As Boolean
            Dim udtEligibleResult As EligibleResult = Nothing
            Dim udtSystemMessage As SystemMessage = Nothing

            'Search the updated EHS Account Again
            Dim udtSearchAccountStatus As New BLL.EHSClaimBLL.SearchAccountStatus

            Select Case udtEHSAccount.SearchDocCode
                Case DocTypeModel.DocTypeCode.HKIC, _
                    DocTypeModel.DocTypeCode.HKBC, _
                    DocTypeModel.DocTypeCode.ADOPC, _
                    DocTypeModel.DocTypeCode.REPMT, _
                    DocTypeModel.DocTypeCode.ID235B, _
                    DocTypeModel.DocTypeCode.VISA, _
                    DocTypeModel.DocTypeCode.DI

                    If udtEHSAccount.SearchDocCode = DocTypeModel.DocTypeCode.ADOPC Then
                        udtSystemMessage = _udtEHSClaimBLL.SearchEHSAccount(udtSchemeClaim.SchemeCode.Trim, udtEHSAccount.SearchDocCode, _
                            udtEHSPersonalInfo.IdentityNum, strDOB, udtEHSAccount, udtEligibleResult, udtSearchAccountStatus, Nothing, _
                            udtEHSPersonalInfo.AdoptionPrefixNum, FunctionCode, ClaimMode.All)
                    Else
                        If Not udtSmartIDContent Is Nothing AndAlso udtSmartIDContent.IsReadSmartID Then

                            '==================================================================== Code for SmartID ============================================================================
                            'Select Case udtSmartIDContent.SmartIDReadStatus
                            '    Case BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_SameDetail, _
                            '            BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_NotCreateBySmartID_SameDOB_NoCCCode
                            '        udtSystemMessage = Me._udtEHSClaimBLL.SearchEHSAccount(udtSchemeClaim.SchemeCode.Trim(), udtEHSAccount.SearchDocCode, udtEHSPersonalInfo.IdentityNum, strDOB, udtEHSAccount, udtEligibleResult, blnNotMatchAccountExist, blnExceedDocTypeLimit)
                            '    Case Else
                            '        udtSystemMessage = Me._udtEHSClaimBLL.SearchTemporaryAccount(udtSchemeClaim.SchemeCode.Trim(), udtEHSAccount.VoucherAccID, udtEHSAccount, udtEligibleResult, blnExceedDocTypeLimit)
                            'End Select


                            'udtEHSAccount.SetSearchDocCode(DocTypeModel.DocTypeCode.HKIC)
                            'udtSmartIDContent.EHSAccount = udtEHSAccount


                            Dim blnKeepSmartIDContent As Boolean = False

                            If udtSmartIDContent.IsCreateAmendEHSAccount Then
                                If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                                    udtSmartIDContent.IsCreateAmendEHSAccount = False
                                ElseIf udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.TemporaryAccount Then
                                    blnKeepSmartIDContent = True

                                End If
                            End If
                            'CRE16-017 (Block EHCP make voucher claim for themselves) [Start][Chris YIM]
                            '-----------------------------------------------------------------------------------------
                            udtSystemMessage = Me._udtEHSClaimBLL.SearchEHSAccountSmartID(udtSchemeClaim.SchemeCode.Trim(), DocTypeModel.DocTypeCode.HKIC, udtEHSPersonalInfo.IdentityNum, _
                                                strDOB, udtEHSAccount, udtSmartIDContent.EHSAccount, udtSmartIDContent.SmartIDReadStatus, udtEligibleResult, blnNotMatchAccountExist, blnExceedDocTypeLimit, _
                                                FunctionCode, True, ClaimMode.All)
                            'CRE16-017 (Block EHCP make voucher claim for themselves) [End][Chris YIM]

                            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                            ' ----------------------------------------------------------
                            ' Restore the HKIC Symbol
                            udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).HKICSymbol = SessionHandler.HKICSymbolGetFormSession(FunctionCode)
                            ' CRE17-010 (OCSSS integration) [End][Chris YIM]

                            If blnKeepSmartIDContent Then
                                udtEHSAccount = udtSmartIDContent.EHSAccount
                            Else
                                udtSmartIDContent.EHSAccount = udtEHSAccount
                            End If

                            MyBase.SessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctionCode)
                            MyBase.SessionHandler.SmartIDContentSaveToSession(FunctionCode, udtSmartIDContent)

                            'Me._udtSystemMessage  is not nothign : systemmessage stored the complete transaction information message
                            '===================================================================================================================================================================
                        Else
                            udtSystemMessage = _udtEHSClaimBLL.SearchEHSAccount(udtSchemeClaim.SchemeCode.Trim, udtEHSAccount.SearchDocCode, _
                                udtEHSPersonalInfo.IdentityNum, strDOB, udtEHSAccount, udtEligibleResult, udtSearchAccountStatus, Nothing, _
                                String.Empty, FunctionCode, ClaimMode.All)

                            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                            ' -------------------------------------------------------------------------------
                            ' If selected Doc Code is different the one in udtEHSAccount, Search Temporary Account again, Check Account Status
                            ' -------------------------------------------------------------------------------
                            Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSPersonalInfo.DocCode)
                            If udtEHSAccountPersonalInfo Is Nothing Then

                                _udtEHSClaimBLL.SearchTemporaryAccount(udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, strDOB, udtEHSAccount, _
                                    udtSearchAccountStatus, udtEHSPersonalInfo)

                                udtSearchAccountStatus.NotMatchAccountExist = udtSearchAccountStatus.OnlyInvalidAccountFound OrElse udtSearchAccountStatus.TempAccountNotMatchDOBFound

                                SessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctionCode)
                                SessionHandler.SearchAccountStatusSaveToSession(udtSearchAccountStatus)
                            End If
                            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

                            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                            ' ----------------------------------------------------------
                            ' Restore the HKIC Symbol 
                            If udtEHSAccount.SearchDocCode = DocTypeModel.DocTypeCode.HKIC Then
                                udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).HKICSymbol = SessionHandler.HKICSymbolGetFormSession(FunctionCode)
                                SessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctionCode)
                            End If
                            ' CRE17-010 (OCSSS integration) [End][Chris YIM]
                        End If
                    End If

                Case DocTypeModel.DocTypeCode.EC

                    If udtEHSPersonalInfo.ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration AndAlso udtEHSPersonalInfo.ECAge.HasValue Then
                        udtSystemMessage = _udtEHSClaimBLL.SearchEHSAccount(udtSchemeClaim.SchemeCode.Trim, udtEHSAccount.SearchDocCode, _
                            udtEHSPersonalInfo.IdentityNum, udtEHSPersonalInfo.ECAge.Value, udtEHSPersonalInfo.ECDateOfRegistration.Value, _
                            udtEHSAccount, udtEligibleResult, udtSearchAccountStatus, Nothing, FunctionCode, ClaimMode.All)

                    Else
                        udtSystemMessage = _udtEHSClaimBLL.SearchEHSAccount(udtSchemeClaim.SchemeCode.Trim, udtEHSAccount.SearchDocCode, _
                            udtEHSPersonalInfo.IdentityNum, strDOB, udtEHSAccount, udtEligibleResult, udtSearchAccountStatus, Nothing, _
                            String.Empty, FunctionCode, ClaimMode.All)

                    End If

            End Select

            ' -------------------------------------------------------------------------------
            ' 9. Benefit Error 
            ' -------------------------------------------------------------------------------
            If Not udtSystemMessage Is Nothing Then
                If udtSystemMessage.MessageCode = "00195" Then
                    udtSystemMessage = Nothing
                ElseIf udtSystemMessage.MessageCode = "00107" Then
                    udtSystemMessage = Nothing
                End If
            End If

            If Not udtEHSAccount.IsNew AndAlso udtSystemMessage Is Nothing Then

                If udtEligibleResult Is Nothing Then
                    SessionHandler.EligibleResultRemoveFromSession()
                Else
                    'Remove all rules from session since some rules may produced by enter claim detail
                    SessionHandler.EligibleResultRemoveFromSession()

                    'CIVSS rule: Key should be same as step1 <- to be implement if more then one rules return
                    Dim udtRuleResults As RuleResultCollection = New RuleResultCollection()
                    udtEligibleResult.PromptConfirmed = True
                    udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Step1, RuleTypeENum.EligibleResult), udtEligibleResult)

                    SessionHandler.EligibleResultSaveToSession(udtRuleResults)
                End If

                MyBase.SessionHandler.EHSTransactionRemoveFromSession(FunctionCode)
                MyBase.SessionHandler.EHSClaimVaccineRemoveFromSession()
                MyBase.SessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctionCode)
                MyBase.SessionHandler.ClaimCategoryRemoveFromSession(FunctionCode)
                Me.ResetStep2aServiceDate()

                Me.StepSelectCategoryClean()

                ' Save step for hidding the cancel button in "select scheme" page
                MyBase.SessionHandler.EHSClaimStepsSaveToSession(FunctionCode, Me.mvEHSClaim.ActiveViewIndex)

                ' Move to Select Scheme
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SelectScheme

                ' Default service date in here: avoid change view and service date being reset
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim udtSubPlatformBLL As New SubPlatformBLL
                'txtStep2aServiceDate.Text = DateTime.Today.ToString(_udtFormatter.EnterDateFormat)
                txtStep2aServiceDate.Text = DateTime.Today.ToString(_udtFormatter.EnterDateFormat(udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform)))
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]


                EHSClaimBasePage.AuditLogClaimForSamePatient(_udtAuditLogEntry)
                EHSClaimBasePage.AuditLogEnterClaimDetailLoaded(_udtAuditLogEntry)
            Else
                SessionHandler.EHSTransactionRemoveFromSession(FunctionCode)
                SessionHandler.EHSClaimVaccineRemoveFromSession()
                Me.SessionHandler.EHSAccountRemoveFromSession(FunctionCode)
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.InternalError
            End If
        End Sub

        Protected Sub udcStep3ReadOnlyEHSClaim_VaccineRemarkClicked(ByVal sender As Object, ByVal e As EventArgs) Handles udcStep3ReadOnlyEHSClaim.VaccineRemarkClicked
            If (MyBase.IsPageRefreshed) Then
                _blnIsRequireHandlePageRefresh = True
                Return
            End If

            ShowVaccineRemarkText()

        End Sub

        '-------------------------------------------------------------------------------------------------------------------
        'Functions
        '-------------------------------------------------------------------------------------------------------------------
        Protected Overrides Sub SetupStep3(ByVal udtEHSAccount As EHSAccountModel, ByVal activeViewChange As Boolean)
            Dim udtEHSTransaction As EHSTransactionModel = SessionHandler.EHSTransactionGetFromSession(FunctionCode)
            Dim udtSelectedPracticeDisplay As BLL.PracticeDisplayModel = SessionHandler.PracticeDisplayGetFromSession(FunctionCode)
            Dim udtSchemeClaim As SchemeClaimModel = SessionHandler.SchemeSelectedGetFromSession(FunctionCode)

            ' Message disappeared when change language
            'If activeViewChange Then
            ' Show complete transcaction message when change view into claim complete view
            Dim udtSysMsgClaimComplete As SystemMessage
            'udtSysMsgClaimComplete = New Common.ComObject.SystemMessage("020202", "I", "00002")

            Dim udtUserAC As UserACModel = UserACBLL.GetUserAC
            Dim strAC_ID As String = String.Empty

            Dim udtServiceProvider As ServiceProviderModel = Nothing
            Dim udtDataEntry As DataEntryUserModel = Nothing
            If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                udtServiceProvider = udtUserAC
                strAC_ID = udtServiceProvider.SPID
            Else
                udtDataEntry = udtUserAC
                strAC_ID = udtDataEntry.DataEntryAccount
            End If

            'CRE15-003 System-generated Form [Start][Philip Chau]
            If Not udtEHSTransaction.PrintedConsentForm Then
                MyBase.SessionHandler.EHSClaimTempTransactionIDRemoveFromSession()
            End If

            Dim udtFormatter As Formatter = New Formatter()
            Dim strTransactionIDPrefix As String = MyBase.SessionHandler.EHSClaimTempTransactionIDGetFromSession()
            Dim blnPrefixAndTransactionIDTheSame As Boolean = Me._udtEHSClaimBLL.chkIsPrefixAndTransactionIDTheSame(strTransactionIDPrefix, udtEHSTransaction)
            If blnPrefixAndTransactionIDTheSame Then
                Step3HandleTransactionPrefixMisMatch(strTransactionIDPrefix, True)
            Else
                Step3HandleTransactionPrefixMisMatch(strTransactionIDPrefix, False)
            End If

            'CRE15-003 System-generated Form [End][Philip Chau]
            ' Save transaction 
            ' --------------------------------------------------------------------------------
            'If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            ' if SP
            ' --------------------------------------------------------------------------------
            'CRE15-003 System-generated Form [Start][Philip Chau]
            If udtEHSTransaction.RecordStatus = EHSTransactionModel.TransRecordStatusClass.Incomplete Then
                ' Defer
                If blnPrefixAndTransactionIDTheSame Then
                    udtSysMsgClaimComplete = New Common.ComObject.SystemMessage("020202", "I", "00007")
                Else
                    udtSysMsgClaimComplete = New Common.ComObject.SystemMessage("020202", "I", "00009")
                End If

            Else

                ' Complete Information
                'Me.udtEHSTransactionBLL.UpdateEHSTransactionStatus(udtEHSTransaction.TransactionID, EHSTransactionModel.TransRecordStatusClass.Pending, udtEHSTransaction.UpdateBy, udtEHSTransaction.UpdateDate, udtEHSTransaction.TSMP)

                ' Confirm transaction
                'Dim udtRecordConfirmationBLL As New RecordConfirmationBLL
                'Dim dtmClaimConfirmationDate As DateTime = udtRecordConfirmationBLL.ConfirmTransaction(udtEHSTransaction.ServiceProviderID, udtEHSTransaction.TransactionID)
                If blnPrefixAndTransactionIDTheSame Then
                    udtSysMsgClaimComplete = New Common.ComObject.SystemMessage("020202", "I", "00002")
                Else
                    udtSysMsgClaimComplete = New Common.ComObject.SystemMessage("020202", "I", "00008") ' Claim completed and confirmed. Please refer to the following information. (TBC)
                End If
            End If
            'Else
            '    ' if Data Entry
            '    ' --------------------------------------------------------------------------------

            '    If udtEHSTransaction.RecordStatus = EHSTransactionModel.TransRecordStatusClass.Incomplete Then
            '        ' Defer
            '        udtSysMsgClaimComplete = New Common.ComObject.SystemMessage("020202", "I", "00007")
            '    Else
            '        ' Complete Information, pending SP to confirm
            '        'Me.udtEHSTransactionBLL.UpdateEHSTransactionStatus(udtEHSTransaction.TransactionID, EHSTransactionModel.TransRecordStatusClass.Pending, udtEHSTransaction.UpdateBy, udtEHSTransaction.UpdateDate, udtEHSTransaction.TSMP)

            '        udtSysMsgClaimComplete = New Common.ComObject.SystemMessage("020202", "I", "00002")  ' Claim completed. Please refer to the following information. (TBC)
            '    End If

            'End If
            'CRE15-003 System-generated Form [End][Philip Chau]
            Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Complete
            Me.udcMsgBoxInfo.AddMessage(udtSysMsgClaimComplete, New String() {"%s"}, New String() {_udtFormatter.formatSystemNumber(udtEHSTransaction.TransactionID)})
            Me.udcMsgBoxInfo.BuildMessageBox()
            'End If

            'Normal Fields 
            If SessionHandler.Language() = CultureLanguage.TradChinese Then
                Me.lblStep3Scheme.Text = udtSchemeClaim.SchemeDescChi
                Me.lblStep3ServiceType.Text = udtEHSTransaction.ServiceTypeDesc_Chi
                Me.lblStep3Practice.Text = String.Format("{0} ({1})", udtSelectedPracticeDisplay.PracticeNameChi, udtSelectedPracticeDisplay.PracticeID)
                Me.lblStep3Practice.CssClass = "tableTextChi"
            Else
                Me.lblStep3Scheme.Text = udtSchemeClaim.SchemeDesc
                Me.lblStep3ServiceType.Text = udtEHSTransaction.ServiceTypeDesc
                Me.lblStep3Practice.Text = String.Format("{0} ({1})", udtSelectedPracticeDisplay.PracticeName, udtSelectedPracticeDisplay.PracticeID)
                Me.lblStep3Practice.CssClass = "tableText"
            End If

            Me.lblStep3TransStatus.Text = GetTransactionStatusText(udtEHSTransaction.RecordStatus)
            FormatTransactionStatus(Me.lblStep3TransStatus, udtEHSTransaction.RecordStatus)

            Me.lblStep3TransNum.Text = _udtFormatter.formatSystemNumber(udtEHSTransaction.TransactionID)
            Me.lblStep3TransDate.Text = _udtFormatter.formatDateTime(udtEHSTransaction.TransactionDtm)

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim udtSubPlatformBLL As New SubPlatformBLL
            'Me.lblStep3ServiceDate.Text = _udtFormatter.formatDate(udtEHSTransaction.ServiceDate)
            Me.lblStep3ServiceDate.Text = _udtFormatter.formatDisplayDate(udtEHSTransaction.ServiceDate, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]


            Me.lblStep3BankAcct.Text = _udtFormatter.maskBankAccount(udtEHSTransaction.BankAccountNo)

            'Setup Personal information 
            Me.udcStep3ReadOnlyDocumnetType.TextOnlyVersion = True
            Me.udcStep3ReadOnlyDocumnetType.DocumentType = udtEHSAccount.SearchDocCode
            Me.udcStep3ReadOnlyDocumnetType.EHSAccount = udtEHSAccount
            Me.udcStep3ReadOnlyDocumnetType.Vertical = True
            Me.udcStep3ReadOnlyDocumnetType.MaskIdentityNo = True
            Me.udcStep3ReadOnlyDocumnetType.ShowAccountRefNo = False
            Me.udcStep3ReadOnlyDocumnetType.ShowTempAccountNotice = False
            Me.udcStep3ReadOnlyDocumnetType.ShowAccountCreationDate = False
            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Me.udcStep3ReadOnlyDocumnetType.SetEnableToShowHKICSymbol = True
            ' CRE17-010 (OCSSS integration) [End][Chris YIM]
            Me.udcStep3ReadOnlyDocumnetType.Built()

            'setup Transaction detail
            Me.udcStep3ReadOnlyEHSClaim.TextOnlyVersion = True
            Me.udcStep3ReadOnlyEHSClaim.EHSClaimVaccine = SessionHandler.EHSClaimVaccineGetFromSession()
            Me.udcStep3ReadOnlyEHSClaim.EHSTransaction = udtEHSTransaction
            Me.udcStep3ReadOnlyEHSClaim.SchemeCode = udtSchemeClaim.SchemeCode
            Me.udcStep3ReadOnlyEHSClaim.Mode = ucReadOnlyEHSClaim.ReadOnlyEHSClaimMode.Complete
            Me.udcStep3ReadOnlyEHSClaim.Built()

            If activeViewChange Then
                EHSClaimBasePage.AuditLogCompleteClaim(_udtAuditLogEntry, udtEHSTransaction)
            End If
        End Sub

        Private Sub FormatTransactionStatus(ByRef lblTransactionStatus As Label, ByVal strTransactionStatus As String)
            If strTransactionStatus = Common.Component.ClaimTransStatus.Incomplete Then
                lblTransactionStatus.CssClass = "tableTextForInComplete"
            Else
                lblTransactionStatus.CssClass = "tableText"
            End If
        End Sub

        Private Function GetTransactionStatusText(ByVal strRecordStatus As String) As String
            Dim strTransactionStatusText As String
            Select Case strRecordStatus
                Case Common.Component.ClaimTransStatus.Incomplete
                    strTransactionStatusText = Me.GetGlobalResourceObject("Text", "Incomplete")
                Case Common.Component.ClaimTransStatus.Active
                    strTransactionStatusText = Me.GetGlobalResourceObject("Text", "ReadytoReimburse")
                Case Common.Component.ClaimTransStatus.Inactive
                    strTransactionStatusText = Me.GetGlobalResourceObject("Text", "Voided")
                Case Common.Component.ClaimTransStatus.Pending
                    strTransactionStatusText = Me.GetGlobalResourceObject("Text", "PendingConfirmation")
                Case Common.Component.ClaimTransStatus.PendingVRValidate
                    strTransactionStatusText = Me.GetGlobalResourceObject("Text", "PendingVoucherAccountValidation")
                Case Common.Component.ClaimTransStatus.Reimbursed
                    strTransactionStatusText = Me.GetGlobalResourceObject("Text", "Reimbursed")
                Case Common.Component.ClaimTransStatus.Suspended
                    strTransactionStatusText = Me.GetGlobalResourceObject("Text", "ClaimSuspended")
                Case Common.Component.ClaimTransStatus.ManualReimbursedClaim
                    strTransactionStatusText = Me.GetGlobalResourceObject("Text", "ManualReimbursedClaim")
                    ' CRE13-001 - EHAPP [Start][Tommy L]
                    ' -------------------------------------------------------------------------------------
                Case Common.Component.ClaimTransStatus.Joined
                    strTransactionStatusText = Me.GetGlobalResourceObject("Text", "Joined")
                    ' CRE13-001 - EHAPP [End][Tommy L]
                Case Else
                    strTransactionStatusText = "Unclassified"
            End Select

            Return strTransactionStatusText
        End Function

        'CRE15-003 System-generated Form [Start][Philip Chau]
        Private Sub btnStep3ViewLatestTransactionID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStep3ViewLatestTransactionID.Click
            HandleButtonClicked(True)
            MyBase.SessionHandler.EHSClaimStep3ShowLastestTransactionIDSaveToSession(True)
            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode(), Me)
            udtAuditLogEntry.AddDescripton("View latest transaction no.", lblStep3TransNum.Text)
            udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00088, "Show Transaction ID")
        End Sub

        Private Sub HandleButtonClicked(ByVal blnclicked As Boolean)
            If blnclicked Then
                lblStep3TransactionIDUpdateNoticeBeforeViewLatest.Visible = False
                lblStep3TransactionIDUpdateNoticeAfterViewLatest.Visible = True
                lblLatestTransactionID.Visible = True
                btnStep3ViewLatestTransactionID.Visible = False
                lblHTMLRightPointArrow.Visible = True
            Else
                lblStep3TransactionIDUpdateNoticeBeforeViewLatest.Visible = True
                lblStep3TransactionIDUpdateNoticeAfterViewLatest.Visible = False
                lblLatestTransactionID.Visible = False
                btnStep3ViewLatestTransactionID.Visible = True
                lblHTMLRightPointArrow.Visible = False
            End If
        End Sub

        Private Sub Step3HandleTransactionPrefixMisMatch(ByVal strTmpTranactionIDPrefix As String, ByVal blnViewedLatestTransactioNo As Boolean)
            Dim udtFormatter As Formatter = New Formatter()
            Dim udtEHSTransaction As EHSTransactionModel = MyBase.SessionHandler.EHSTransactionGetFromSession(FunctionCode())

            lblLatestTransactionID.Text = udtFormatter.formatSystemNumber(udtEHSTransaction.TransactionID)
            lblStep3PrefixTransNum.Text = strTmpTranactionIDPrefix

            If blnViewedLatestTransactioNo Then
                lblStep3TransNum.Visible = True
                lblStep3PrefixTransNum.Visible = False

                lblStep3TransactionIDUpdateNoticeBeforeViewLatest.Visible = False
                lblStep3TransactionIDUpdateNoticeAfterViewLatest.Visible = False
                lblLatestTransactionID.Visible = False
                btnStep3ViewLatestTransactionID.Visible = False
                lblHTMLRightPointArrow.Visible = False
            Else
                lblStep3TransNum.Visible = False
                lblStep3PrefixTransNum.Visible = True
                lblStep3TransactionIDUpdateNoticeBeforeViewLatest.Visible = True
                lblHTMLRightPointArrow.Visible = False

                If MyBase.SessionHandler.EHSClaimStep3ShowLastestTransactionIDGetFromSession() Then
                    HandleButtonClicked(True)
                Else
                    HandleButtonClicked(False)
                End If
            End If

        End Sub
        'CRE15-003 System-generated Form [End][Philip Chau]

        Protected Overrides Sub Step3Clear()
            Me.udcStep3ReadOnlyDocumnetType.Clear()
            Me.udcStep3ReadOnlyEHSClaim.Clear()
        End Sub

#End Region


#Region "Step of Select Practice"

        ''' <summary>
        ''' Check the User can go to next step or not
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function StepSelectPracticeValdiation() As Boolean
            ' Check practice is selected in session
            Dim blnIsValid As Boolean = False
            Dim udtSessionPractice As BLL.PracticeDisplayModel = SessionHandler.PracticeDisplayGetFromSession(FunctionCode)

            If udtSessionPractice Is Nothing Then
                ' Error
                blnIsValid = False

                Dim udtSysMessagePractice As SystemMessage = New SystemMessage(FunctionCode, "E", "00017")
                Me.ShowError(udtSysMessagePractice)
            Else
                blnIsValid = True
            End If

            Return blnIsValid
        End Function

        Protected Overrides Sub SetupSelectPractice()

            Dim udtPracticeDisplays As BLL.PracticeDisplayModelCollection = SessionHandler.PracticeDisplayListGetFromSession()


            SessionHandler.CurrentUserGetFromSession(Me._udtSP, Me._udtDataEntryModel)

            'Build practice Selection List
            ucSelectPracticeRadioButtonGroupText.MaskBankAccountNo = True
            ucSelectPracticeRadioButtonGroupText.BuildRadioButtonGroup(True, udtPracticeDisplays, Me._udtSP.PracticeList, SessionHandler.Language, PracticeRadioButtonGroup.DisplayMode.BankAccount)

            ' Check Session Object exists, and select
            Dim udtSessionPracticeDisplay As BLL.PracticeDisplayModel = SessionHandler.PracticeDisplayGetFromSession(FunctionCode)
            If Not udtSessionPracticeDisplay Is Nothing AndAlso String.IsNullOrEmpty(ucSelectPracticeRadioButtonGroupText.SelectedValue) Then
                ucSelectPracticeRadioButtonGroupText.SelectedValue = udtSessionPracticeDisplay.PracticeID
            End If

            If Me.SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                Me.ucSelectPracticeRadioButtonGroupText.CssClass = "tableTextChi"
            Else
                Me.ucSelectPracticeRadioButtonGroupText.CssClass = "tableText"
            End If

        End Sub

        Private Sub btnStepSelectPracticeGO_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnStepSelectPracticeGO.Click
            Dim sessionPractice As BLL.PracticeDisplayModel = SessionHandler.PracticeDisplayGetFromSession(FunctionCode)
            Dim udtEHSAccount As EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(FunctionCode)
            Dim strPracticeId As String = ucSelectPracticeRadioButtonGroupText.SelectedValue.Trim()
            Dim intPracticeId As Integer
            Dim comeFromStep2a As Boolean = IIf(udtEHSAccount Is Nothing, False, True)
            Dim blnGoToVersion2 As Boolean = False
            Dim udtPracticeDisplays As BLL.PracticeDisplayModelCollection

            If Not String.IsNullOrEmpty(strPracticeId) Then
                udtPracticeDisplays = SessionHandler.PracticeDisplayListGetFromSession()
                intPracticeId = Convert.ToInt32(strPracticeId)

                ' Check the selected practice is same as the one in session or not.
                If Not sessionPractice Is Nothing AndAlso sessionPractice.PracticeID.ToString() <> strPracticeId Then
                    ' Handle Practice Updated: Selected Practice different from Session's Practice
                    Dim blnIsSchemeExists As Boolean = False
                    Dim udtSchemeClaimBLL As SchemeClaimBLL = New SchemeClaimBLL()

                    Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = Nothing
                    Dim udtSessionScheme As SchemeClaimModel = SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
                    Dim udtSelectedPracticeDisplay As BLL.PracticeDisplayModel = udtPracticeDisplays.Filter(intPracticeId)

                    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    Me.SessionHandler.PIDInstitutionCodeRemoveFromSession(FunctionCode)
                    Me.SessionHandler.PlaceVaccinationRemoveFromSession(FunctionCode)
                    Me.SessionHandler.PlaceVaccinationOtherRemoveFromSession(FunctionCode)
                    'CRE16-002 (Revamp VSS) [End][Chris YIM]

                    ' if no scheme is selected, no need to retain
                    ' Check the scheme is available in the newly selected practice
                    ' Get all available Scheme <- for the new SP
                    udtSchemeClaimModelCollection = udtSchemeClaimBLL.searchValidClaimPeriodSchemeClaimByPracticeSchemeInfoSubsidizeCode(SessionSP.PracticeList(intPracticeId).PracticeSchemeInfoList, Me._udtSP.SchemeInfoList)

                    If Not udtEHSAccount Is Nothing Then '<- come from Step2a

                        ' Get all Eligible Scheme form available List <- for EHS Account
                        udtSchemeClaimModelCollection = udtSchemeClaimBLL.searchEligibleAndExceedDocumentClaimScheme(udtEHSAccount, udtEHSAccount.SearchDocCode, udtSchemeClaimModelCollection)

                        Me.SessionHandler.SchemeSubsidizeListSaveToSession(udtSchemeClaimModelCollection, FunctionCode)
                    End If

                    If Not udtSessionScheme Is Nothing AndAlso Not udtSchemeClaimModelCollection Is Nothing AndAlso udtSchemeClaimModelCollection.Count > 0 Then
                        'INT21-0010 DHC district selection issue - Clear the radiobutton [Start][Nichole]
                        Dim udcInputHCVS As UIControl.EHCClaimText.ucInputHCVS = Me.udcStep2aInputEHSClaim.GetHCVSControl()
                        If Not udcInputHCVS Is Nothing Then
                            udcInputHCVS.DHCDistrictRBL.Items.Clear()
                            udcInputHCVS.DHCDistrictRBL.SelectedValue = Nothing
                            udcInputHCVS.DHCDistrictRBL.SelectedIndex = -1
                            udcInputHCVS.DHCDistrictRBL.ClearSelection()
                            udcInputHCVS.DHCDistrictCHK.Checked = Nothing
                        End If
                        'INT21-0010 DHC district selection issue - Clear the radiobutton [End][Nichole]

                        ' Check the Session's Scheme exists in the new practice's scheme list
                        For Each udtSchemeClaimModel As SchemeClaimModel In udtSchemeClaimModelCollection
                            If udtSchemeClaimModel.SchemeCode = udtSessionScheme.SchemeCode Then
                                blnIsSchemeExists = True
                                SessionHandler.SchemeSelectedSaveToSession(udtSchemeClaimModel, FunctionCode)

                                ' No need to go to Version 2

                                'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                                '-----------------------------------------------------------------------------------------
                                Dim udtConvertedSchemeCode As String = udtSchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(udtSchemeClaimModel.SchemeCode)
                                SessionHandler.NonClinicSettingSaveToSession(SessionSP.PracticeList(udtSelectedPracticeDisplay.PracticeID).PracticeSchemeInfoList.Filter(udtConvertedSchemeCode).IsNonClinic, FunctionCode)
                                'CRE16-002 (Revamp VSS) [End][Chris YIM]

                            End If
                        Next
                    End If

                    If comeFromStep2a Then

                        Me.udcStep2aInputEHSClaim.ResetSchemeType()
                        Me.SessionHandler.EHSClaimVaccineRemoveFromSession()

                        If blnIsSchemeExists = False Then

                            ' Previous Selected Scheme not exists in the new Practice, Clear the input claim control 
                            Step2aClear()
                            ' Scheme Updated: Remove Transaction
                            SessionHandler.EHSTransactionRemoveFromSession(FunctionCode)
                            MyBase.SessionHandler.ClaimCategoryRemoveFromSession(FunctionCode)
                            Me.StepSelectCategoryClean()

                            ' Default the first Scheme if the Session Scheme doesnt exists in new practice scheme list
                            If Not udtSchemeClaimModelCollection Is Nothing AndAlso udtSchemeClaimModelCollection.Count > 0 Then
                                ' Auto select the first Scheme

                                Dim udtSchemeClaimModelDefault As SchemeClaimModel = udtSchemeClaimModelCollection(0)

                                ' From Step2a case, the document type should be same, no need to clear
                                SessionHandler.SchemeSelectedSaveToSession(udtSchemeClaimModelDefault, FunctionCode)

                                Dim udtConvertedSchemeCode As String = udtSchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(udtSchemeClaimModelDefault.SchemeCode)
                                SessionHandler.NonClinicSettingSaveToSession(SessionSP.PracticeList(udtSelectedPracticeDisplay.PracticeID).PracticeSchemeInfoList.Filter(udtConvertedSchemeCode).IsNonClinic, FunctionCode)

                                ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
                                ' --------------------------------------------------------------------------------------
                                Select Case udtSchemeClaimModelDefault.ControlType
                                    Case SchemeClaimModel.EnumControlType.CIVSS, SchemeClaimModel.EnumControlType.EVSS, SchemeClaimModel.EnumControlType.HSIVSS, _
                                         SchemeClaimModel.EnumControlType.RVP, SchemeClaimModel.EnumControlType.PIDVSS, SchemeClaimModel.EnumControlType.VSS, _
                                         SchemeClaimModel.EnumControlType.ENHVSSO, SchemeClaimModel.EnumControlType.PPP

                                        blnGoToVersion2 = True

                                    Case Else
                                        blnGoToVersion2 = False

                                End Select
                                ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

                                'Log Practice Selection
                                EHSClaimBasePage.AuditLogPracticeSelected(_udtAuditLogEntry, True, udtSelectedPracticeDisplay, udtSchemeClaimModelCollection(0), True)
                            Else

                                'Log Practice Selection
                                EHSClaimBasePage.AuditLogPracticeSelected(_udtAuditLogEntry, True, udtSelectedPracticeDisplay, Nothing, False)

                                ' No Scheme in new practice
                                SessionHandler.SchemeSelectedRemoveFromSession(FunctionCode)

                                'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                                '-----------------------------------------------------------------------------------------
                                SessionHandler.NonClinicSettingRemoveFromSession(FunctionCode)
                                'CRE16-002 (Revamp VSS) [End][Chris YIM]
                            End If

                        End If

                        ' CRE19-003 (Opt voucher capping) [Start][Winnie]
                        ' ----------------------------------------------------------------------------------------
                        ' Profession is changed, need to recalculate quota
                        If Not sessionPractice.ServiceCategoryCode.Equals(udtSelectedPracticeDisplay.ServiceCategoryCode) Then

                            ' Clear AvailableVoucher to trigger recalculation
                            udtEHSAccount.VoucherInfo = Nothing
                        End If
                        ' CRE19-003 (Opt voucher capping) [End][Winnie]
                    Else

                        If blnIsSchemeExists = False Then

                            'Log Practice Selection
                            EHSClaimBasePage.AuditLogPracticeSelected(_udtAuditLogEntry, False, udtSelectedPracticeDisplay, Nothing, False)

                            'Scheme Not exist user must select scheme again
                            MyBase.SessionHandler.SchemeSelectedRemoveFromSession(FunctionCode)
                            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                            '-----------------------------------------------------------------------------------------
                            SessionHandler.NonClinicSettingRemoveFromSession(FunctionCode)
                            'CRE16-002 (Revamp VSS) [End][Chris YIM]
                            MyBase.SessionHandler.ClaimCategoryRemoveFromSession(FunctionCode)
                            Me.StepSelectCategoryClean()

                            If Not Me.IsSchemeAvailableInDocumentType(udtSchemeClaimModelCollection(0), MyBase.SessionHandler.DocumentTypeSelectedGetFromSession(FunctionCode)) Then

                                ' No Scheme in new practice
                                MyBase.SessionHandler.DocumentTypeSelectedRemoveFromSession(FunctionCode)

                            End If
                        Else
                            ' keep the selected scheme if the scheme exists in the newly selected practice

                            'Log Practice Selection
                            EHSClaimBasePage.AuditLogPracticeSelected(_udtAuditLogEntry, False, udtSelectedPracticeDisplay, udtSessionScheme, False)
                        End If

                        ' Remove Session following by this step
                        SessionHandler.EHSClaimSessionRemove(FunctionCode)
                    End If
                End If

                ' save practice to session
                SessionHandler.PracticeDisplaySaveToSession(udtPracticeDisplays.Filter(intPracticeId), FunctionCode)
            End If

            ' Move to next step when practice is valid
            If StepSelectPracticeValdiation() Then

                ' Check if EHSAccount have been already searched.
                If Not SessionHandler.EHSAccountGetFromSession(FunctionCode) Is Nothing Then
                    If blnGoToVersion2 Then
                        ' go to new version claim
                        GoToVersion2()
                    Else
                        ' Account exists, return to enter claim detail
                        Me.SetStep2aInputEHSClaimControlReadFromSession(True)
                        Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step2a
                    End If
                Else
                    ' go to next step, select scheme
                    Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SelectScheme
                End If
            End If

        End Sub

        Private Sub StepSelectPracticeClear()
            ucSelectPracticeRadioButtonGroupText.Controls.Clear()
        End Sub

#End Region


#Region "Step of Select Scheme"

        ''' <summary>
        ''' Go To Select Practice Step
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub btnStepSelectSchemeChangePractice_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnStepSelectSchemeChangePractice.Click
            ' Move to Step Select Practice
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SelectPractice

        End Sub

        Protected Sub btnStepSelectSchemeCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStepSelectSchemeCancel.Click
            SessionHandler.EHSClaimSessionRemove(FunctionCode)
            SessionHandler.EHSClaimConfirmMessageGetFromSession(FunctionCode)
            SessionHandler.EHSClaimStepsRemoveFromSession(FunctionCode)

            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
        End Sub

        Private Sub btnStepSelectSchemeGO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStepSelectSchemeGO.Click
            Dim strScheme As String = ucSchemeRadioButtonGroupText.SelectedValue.Trim()
            Dim udtSessionSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
            Dim udtSchemeClaimBLL As SchemeClaimBLL = New SchemeClaimBLL()
            Dim udtEHSAccount As EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(FunctionCode)
            Dim blnRetainDocType As Boolean = False
            Dim strPerviousCodeCode As String = SessionHandler.DocumentTypeSelectedGetFromSession(FunctionCode)

            ' Audit Log - Change Scheme
            Dim strPreviousSelectedScheme As String = String.Empty
            If Not udtSessionSchemeClaim Is Nothing Then
                strPreviousSelectedScheme = udtSessionSchemeClaim.SchemeCode.Trim()
            End If
            If udtEHSAccount Is Nothing Then
                ' from step1
                EHSClaimBasePage.AuditLogSearchAccountChangeScheme(_udtAuditLogEntry, strPreviousSelectedScheme, strScheme)
            Else
                ' from step2a
                EHSClaimBasePage.AuditLogEnterClaimDetailChangeScheme(_udtAuditLogEntry, strPreviousSelectedScheme, strScheme)
            End If


            If Not String.IsNullOrEmpty(strScheme) Then

                If udtEHSAccount Is Nothing Then
                    ' Check the selected scheme is same as the one in session or not.
                    If udtSessionSchemeClaim Is Nothing OrElse udtSessionSchemeClaim.SchemeCode.Trim() <> strScheme OrElse String.IsNullOrEmpty(strPerviousCodeCode) Then

                        Dim strDefaultDocTypeCode As String = GetDefaultDocumnetTypeByScheme(strScheme)
                        Dim udtSchemeDocTypeList As SchemeDocTypeModelCollection
                        Dim udtDocTypeBLL As New DocTypeBLL()

                        If Not strPerviousCodeCode Is Nothing Then
                            udtSchemeDocTypeList = udtDocTypeBLL.getSchemeDocTypeByScheme(strScheme)

                            For Each udtSchemeDocType As SchemeDocTypeModel In udtSchemeDocTypeList
                                If strPerviousCodeCode.Equals(udtSchemeDocType.DocCode) AndAlso Me.udcStep1ClaimSearch.IsEmpty(strPerviousCodeCode) Then
                                    blnRetainDocType = True
                                    Exit For
                                End If
                            Next
                        End If

                        If Not blnRetainDocType AndAlso Not String.IsNullOrEmpty(strDefaultDocTypeCode) Then
                            ' Default the major Document Type, Auto Select it and go to Step1
                            SessionHandler.DocumentTypeSelectedSaveToSession(strDefaultDocTypeCode, FunctionCode)
                        End If
                    Else

                        blnRetainDocType = True

                        ' Same Scheme. not need to update the corresponding DocType
                    End If
                Else
                    ' Will go back to Step2a
                    ' Check New scheme diff from scheme in session, clear the step2a control if changed
                    If udtSessionSchemeClaim Is Nothing OrElse udtSessionSchemeClaim.SchemeCode.Trim() <> strScheme Then
                        Me.Step2aClear()
                        ' Scheme Updated: Remove Transaction
                        MyBase.SessionHandler.EHSTransactionRemoveFromSession(FunctionCode)
                        MyBase.SessionHandler.EHSClaimVaccineRemoveFromSession()
                        MyBase.SessionHandler.ClaimCategoryRemoveFromSession(FunctionCode)
                        MyBase.SessionHandler.EHSClaimStepsRemoveFromSession(FunctionCode)
                        Me.StepSelectCategoryClean()
                    End If
                End If


                ' save the updated scheme to session
                SessionHandler.SchemeSelectedSaveToSession(udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(strScheme), FunctionCode)

                ' save the non clinic setting when the scheme is updated
                'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim udtConvertedSchemeCode As String = udtSchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(strScheme)
                Dim udtSelectedPracticeDisplay As PracticeDisplayModel = SessionHandler.PracticeDisplayGetFromSession(FunctionCode)

                SessionHandler.NonClinicSettingSaveToSession(SessionSP.PracticeList(udtSelectedPracticeDisplay.PracticeID).PracticeSchemeInfoList.Filter(udtConvertedSchemeCode).IsNonClinic, FunctionCode)

                SessionHandler.ChangeSchemeInPracticeSaveToSession(True, FunctionCode)
                'CRE16-002 (Revamp VSS) [End][Chris YIM]
            End If

            ' Move to next step when scheme is valid
            If StepSelectSchemeValdiation() Then

                ' Step recorded in "claim for same patient" case
                MyBase.SessionHandler.EHSClaimStepsRemoveFromSession(FunctionCode)

                udtSessionSchemeClaim = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode)

                ' Check it's come from Step2a
                If Not SessionHandler.EHSAccountGetFromSession(FunctionCode) Is Nothing Then
                    ' Account exists, return to enter claim detail
                    Me.SetStep2aInputEHSClaimControlReadFromSession(True)

                    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
                    ' --------------------------------------------------------------------------------------
                    Select Case udtSessionSchemeClaim.ControlType
                        Case SchemeClaimModel.EnumControlType.CIVSS, SchemeClaimModel.EnumControlType.EVSS, SchemeClaimModel.EnumControlType.HSIVSS, _
                             SchemeClaimModel.EnumControlType.RVP, SchemeClaimModel.EnumControlType.PIDVSS, SchemeClaimModel.EnumControlType.VSS, _
                             SchemeClaimModel.EnumControlType.ENHVSSO, SchemeClaimModel.EnumControlType.PPP
                            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

                            Dim udtSubPlatformBLL As New SubPlatformBLL

                            Dim strServiceDate As String = Me._udtFormatter.formatInputDate(Me.txtStep2aServiceDate.Attributes(ValidatedServiceDate), udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))

                            Dim dtmServiceDate As DateTime = Me._udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English)

                            Me._udtEHSTransaction = Me._udtEHSClaimBLL.ConstructNewEHSTransaction(udtSessionSchemeClaim, MyBase.SessionHandler.EHSAccountGetFromSession(FunctionCode), MyBase.SessionHandler.PracticeDisplayGetFromSession(FunctionCode), dtmServiceDate)

                            MyBase.SessionHandler.EHSTransactionSaveToSession(Me._udtEHSTransaction, FunctionCode)

                            Me.GoToVersion2()
                        Case Else
                            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step2a

                    End Select


                ElseIf Not String.IsNullOrEmpty(SessionHandler.DocumentTypeSelectedGetFromSession(FunctionCode)) Then

                    Me.Step1Clear(blnRetainDocType)
                    Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
                Else
                    Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SelectDocType
                End If
            End If

        End Sub

        ''' <summary>
        ''' Check the User can go to next step or not
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function StepSelectSchemeValdiation() As Boolean
            ' Check scheme is selected in session
            Dim blnIsValid As Boolean = False
            Dim udtSessionClaim As SchemeClaimModel = SessionHandler.SchemeSelectedGetFromSession(FunctionCode)

            If udtSessionClaim Is Nothing Then
                ' Error
                blnIsValid = False

                Dim udtSysMessageScheme As SystemMessage = New SystemMessage(FunctionCode, "E", "00018")
                Me.ShowError(udtSysMessageScheme)
            Else
                blnIsValid = True
            End If

            Return blnIsValid

        End Function

        Protected Sub SetupSelectScheme(ByVal activeViewChanged As Boolean)
            Dim intPeriousPage As Integer = MyBase.SessionHandler.EHSClaimStepsGetFromSession(FunctionCode)
            Me.btnStepSelectSchemeCancel.Text = Me.GetGlobalResourceObject("AlternateText", "CancelBtn")

            If intPeriousPage = ActiveViewIndex.Step3 Then
                Me.btnStepSelectSchemeCancel.Visible = True
            Else
                Me.btnStepSelectSchemeCancel.Visible = False
            End If

            ' Show Practice
            Me.FillPracticeText(lblStepSelectSchemePractice)

            ' Hide Change Practice button when only 1 practice available
            Dim udtPracticeDisplayModelCollection As BLL.PracticeDisplayModelCollection = GetAvailablePractice()
            Me.btnStepSelectSchemeChangePractice.Visible = (Not udtPracticeDisplayModelCollection Is Nothing AndAlso udtPracticeDisplayModelCollection.Count > 1)

            'Build practice Selection List
            Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = GetAvailableScheme()
            ' CRE16-026 (Add PCV13) [Start][Winnie]
            'ucSchemeRadioButtonGroupText.PopulateScheme(True, udtSchemeClaimModelCollection, SessionHandler.Language, FunctionCode, Me._udtSP, True)
            ucSchemeRadioButtonGroupText.PopulateScheme(True, udtSchemeClaimModelCollection, SessionHandler.Language, FunctionCode, Me._udtSP)
            ' CRE16-026 (Add PCV13) [End][Winnie]

            ' Check Session Object exists, and select
            Dim udtSessionClaimModel As SchemeClaimModel = SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
            If activeViewChanged AndAlso Not udtSessionClaimModel Is Nothing Then
                ucSchemeRadioButtonGroupText.SelectedValue = udtSessionClaimModel.SchemeCode
            End If

        End Sub

        Private Sub StepSelectSchemeClear()
            ucSchemeRadioButtonGroupText.Items.Clear()
        End Sub

#End Region


#Region "Step of Select Document Type"

        ''' <summary>
        ''' Check the User can go to next step or not
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function StepSelectDocumentTypeValdiation() As Boolean
            ' Check document type is selected in session
            Dim blnIsValid As Boolean = False
            Dim strDocTypeCode As String = SessionHandler.DocumentTypeSelectedGetFromSession(FunctionCode)

            If String.IsNullOrEmpty(strDocTypeCode) Then
                ' Error
                blnIsValid = False

                Dim udtSysMessageScheme As SystemMessage = New SystemMessage(FunctionCode, "E", "00019")
                Me.ShowError(udtSysMessageScheme)
            Else
                blnIsValid = True
            End If

            Return blnIsValid
        End Function

        Protected Sub SetupSelectDocumentType()
            Dim udtSchemeClaimBLL As SchemeClaimBLL = New SchemeClaimBLL()
            Dim udtSchemeClaim As SchemeClaimModel = SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
            If udtSchemeClaim Is Nothing Then
                Throw New Exception("Missing Scheme in Select Document Type View")
            End If

            ' Show Practice
            Me.FillPracticeText(lblStepSelectDocTypePractice)

            ' Show Scheme
            Me.FillSchemeText(lblStepSelectDocTypeScheme)

            ' Hide Change Practice button when only 1 practice available
            Dim udtPracticeDisplayModelCollection As BLL.PracticeDisplayModelCollection = GetAvailablePractice()
            Me.btnStepSelectDocTypeChangePractice.Visible = (Not udtPracticeDisplayModelCollection Is Nothing AndAlso udtPracticeDisplayModelCollection.Count > 1)

            ' Hide Change Scheme button when only 1 practice available
            Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = GetAvailableScheme()
            Me.btnStepSelectDocTypeChangeScheme.Visible = (Not udtSchemeClaimModelCollection Is Nothing AndAlso udtSchemeClaimModelCollection.Count > 1)

            'Setup Document Type Selection
            ucDocumentTypeRadioButtonGroupText.Scheme = udtSchemeClaim.SchemeCode
            ucDocumentTypeRadioButtonGroupText.EnableFilterDocCode = CustomControls.DocumentTypeRadioButtonGroupText.FilterDocCode.VaccinationRecordEnquriySearch
            ucDocumentTypeRadioButtonGroupText.Build()

            ' Check Session Object exists, if exists and the item is in the scheme, select the item
            Dim strDocTypeCode As String = SessionHandler.DocumentTypeSelectedGetFromSession(FunctionCode)
            If Not String.IsNullOrEmpty(strDocTypeCode) Then 'AndAlso ucDocumentTypeRadioButtonGroupText.ContainValue(strDocTypeCode) Then
                ucDocumentTypeRadioButtonGroupText.SelectedValue = strDocTypeCode
            End If

        End Sub

        Private Sub btnStepSelectDocTypeGO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStepSelectDocTypeSelect.Click
            Dim strDocTypeCode As String = ucDocumentTypeRadioButtonGroupText.SelectedValue.Trim()
            Dim strOrgDocType As String = MyBase.SessionHandler.DocumentTypeSelectedGetFromSession(FunctionCode)
            Dim blnRetainDocType As Boolean = False

            If String.IsNullOrEmpty(strDocTypeCode) Then
                'Remove selected document type from session, if no scheme selected
                SessionHandler.DocumentTypeSelectedRemoveFromSession(FunctionCode)
            Else
                If strOrgDocType.Equals(strDocTypeCode) Then
                    blnRetainDocType = True
                End If

                'Save the selected document type to session
                SessionHandler.DocumentTypeSelectedSaveToSession(strDocTypeCode, FunctionCode)
            End If

            ' Move to next step when Document Type is selected
            If StepSelectDocumentTypeValdiation() Then
                Me.Step1Clear(blnRetainDocType)
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
            End If

        End Sub

        Private Sub StepSelectDocumentTypeClear()
            ucDocumentTypeRadioButtonGroupText.Controls.Clear()
        End Sub

        ''' <summary>
        ''' Go To Select Practice Step
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub btnStepSelectDocTypeChangePractice_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnStepSelectDocTypeChangePractice.Click
            ' Move to Step Select Practice
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SelectPractice

        End Sub

        ''' <summary>
        ''' Go To Select Scheme Step
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub btnStepSelectDocTypeChangeScheme_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStepSelectDocTypeChangeScheme.Click
            ' Move to Step Select Practice
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SelectScheme

        End Sub

#End Region


#Region "Step of Internal Error "

        ' Event
        Private Sub btnViewInternalErrorReturn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnViewInternalErrorReturn.Click
            ' Clear all session and reset the page
            Me.ClearAll()
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SelectPractice
        End Sub


        ' Function
        Protected Overrides Sub SetupInternalError()
            ' Practice Info
            Dim udtPracticeDisplay As BLL.PracticeDisplayModel = SessionHandler.PracticeDisplayGetFromSession(FunctionCode)
            Dim udtAvailablePractice As BLL.PracticeDisplayModelCollection = GetAvailablePractice()

            ' Scheme Info
            Dim udtSchemeClaim As SchemeClaimModel = Nothing
            Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = Nothing
            If Not udtPracticeDisplay Is Nothing Then
                udtSchemeClaim = SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
                udtSchemeClaimModelCollection = GetAvailableScheme()
            End If

            ' Document Type Info
            Dim udtSchemeDocTypeModelCollection As SchemeDocTypeModelCollection = Nothing
            If Not udtSchemeClaim Is Nothing Then
                udtSchemeDocTypeModelCollection = GetAvailableDocumentType()
            End If

            ' EHSClaim Transaction
            Dim udtEHSTransaction As EHSTransactionModel = SessionHandler.EHSTransactionGetFromSession(FunctionCode)

            ' Default Allow return button to go back to Select Practice page
            Me.btnViewInternalErrorReturn.Visible = True

            'EHS Account 
            Dim udtEHSaccount As EHSAccountModel = Me.SessionHandler.EHSAccountGetFromSession(FunctionCode)
            If Me._blnIsConcurrentBrowserDetected OrElse Not Me.EHSClaimTokenNumValidation(hfEHSClaimTokenNum.Value, FunctionCode) Then
                'Multiple browser with same session detected, the current action aborted.
                Me.btnViewInternalErrorReturn.Visible = False
                Dim udtSysMessageConcurrentBrowser As SystemMessage = New SystemMessage("990000", "I", "00023")
                Me.udcMsgBoxInfo.AddMessage(udtSysMessageConcurrentBrowser)
                Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Information
                Me.udcMsgBoxInfo.BuildMessageBox()
                Me.udcMsgBoxInfo.Visible = True
                Throw New Exception("Concurrent Browser Detected in Text/EHSClaimV1")

            ElseIf udtAvailablePractice IsNot Nothing AndAlso udtAvailablePractice.Count > 0 AndAlso Not udtAvailablePractice.HasPracticeAvailableForClaim Then

                ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]
                ' -----------------------------------------------------------------------------------------
                'No Active Practice found. No claim action can be taken.
                Me.btnViewInternalErrorReturn.Visible = False
                Dim udtSysMessageNoPractice As SystemMessage = New SystemMessage("020202", "I", "00006") ' No available scheme. No claim action can be taken.
                Me.udcMsgBoxInfo.AddMessage(udtSysMessageNoPractice)
                Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Information
                Me.udcMsgBoxInfo.BuildMessageBox()
                Me.udcMsgBoxInfo.Visible = True
                ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

            ElseIf udtPracticeDisplay Is Nothing OrElse udtAvailablePractice Is Nothing OrElse udtAvailablePractice.Count = 0 Then
                'No Active Practice found. No claim action can be taken.
                Me.btnViewInternalErrorReturn.Visible = False
                Dim udtSysMessageNoPractice As SystemMessage = New SystemMessage("020202", "I", "00003")
                Me.udcMsgBoxInfo.AddMessage(udtSysMessageNoPractice)
                Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Information
                Me.udcMsgBoxInfo.BuildMessageBox()
                Me.udcMsgBoxInfo.Visible = True

            ElseIf udtSchemeClaimModelCollection Is Nothing OrElse udtSchemeClaimModelCollection.Count = 0 Then
                'There is no applicable scheme for the eHealth Account in the selected practice.
                Dim udtSysMessageNoScheme As SystemMessage = New SystemMessage("990000", "I", "00020")
                Me.udcMsgBoxInfo.AddMessage(udtSysMessageNoScheme)
                Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Information
                Me.udcMsgBoxInfo.BuildMessageBox()
                Me.udcMsgBoxInfo.Visible = True

            ElseIf Not udtSchemeClaim Is Nothing AndAlso (udtSchemeDocTypeModelCollection Is Nothing OrElse udtSchemeDocTypeModelCollection.Count = 0) Then
                'The scheme setting is invalid. Contact the Department of Health if assistance is required.
                Dim udtSysMessageSchemeError As SystemMessage = New SystemMessage("020202", "E", "00004")

                Me.udcMsgBoxInfo.AddMessage(udtSysMessageSchemeError)
                Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Information
                Me.udcMsgBoxInfo.BuildMessageBox()
                Me.udcMsgBoxInfo.Visible = True


            ElseIf Me._blnConcurrentUpdate = True OrElse udtEHSaccount Is Nothing Then
                'The eHealth Account information is updated by others. The current process may be aborted. Please check and verify.
                Dim udtSysMessageRecordOutdated = New SystemMessage("020202", "I", "00005")

                Me.udcMsgBoxInfo.AddMessage(udtSysMessageRecordOutdated)
                Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Information
                Me.udcMsgBoxInfo.BuildMessageBox()
                Me.udcMsgBoxInfo.Visible = True

            ElseIf (Not udtEHSTransaction Is Nothing) AndAlso _
                        ( _
                            ( _
                                Not udtEHSTransaction.TransactionID Is Nothing AndAlso _
                                Not udtEHSTransaction.TransactionID.Trim().Equals(String.Empty) _
                            ) _
                            OrElse _
                                (Not udtEHSTransaction.IsNew) _
                    ) _
            Then
                'The current update is aborted since the request has been submit more than once or the record(s) is updated by other. Please verify your action has been complete or not.
                Dim udtSysMsgDuplicateRequest As SystemMessage = New SystemMessage("990000", "I", "00022")
                Me.udcMsgBoxInfo.AddMessage(udtSysMsgDuplicateRequest)
                Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Information
                Me.udcMsgBoxInfo.BuildMessageBox()
                Me.udcMsgBoxInfo.Visible = True
            End If

        End Sub

#End Region


#Region "Step of Confirm Box"

        ' Event
        Private Sub btnConfirmBoxBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirmBoxBack.Click
            ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [Start][Winnie]
            Dim intActiveViewSource As Integer = SessionHandler.EHSClaimStepsGetFromSession(FunctionCode)
            Dim objConfirmMessage As Object = SessionHandler.EHSClaimConfirmMessageGetFromSession(FunctionCode)

            If intActiveViewSource = ActiveViewIndex.Step2a AndAlso objConfirmMessage.Equals(DuplicateClaimAlertMessage) Then
                EHSClaimBasePage.AuditLogTextOnlyVersionDuplicateClaimAlertBackClick(_udtAuditLogEntry)
            End If
            ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [End][Winnie]

            ClearConfirmBox()

            ' Cancel Clicked
            Dim intView As Integer = MyBase.SessionHandler.EHSClaimStepsGetFromSession(FunctionCode)

            ' Clear Session Value
            MyBase.SessionHandler.EHSClaimStepsRemoveFromSession(FunctionCode)

            ' Move to the previous View
            Me.mvEHSClaim.ActiveViewIndex = intView

        End Sub

        Private Sub btnConfirmBoxConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirmBoxConfirm.Click
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            'ClearConfirmBox()

            Dim intActiveViewSource As Integer = SessionHandler.EHSClaimStepsGetFromSession(FunctionCode)
            Dim objConfirmMessage As Object = SessionHandler.EHSClaimConfirmMessageGetFromSession(FunctionCode)

            ' Clear Session Value
            'MyBase.SessionHandler.EHSTransactionRemoveFromSession(FunctionCode)
            MyBase.SessionHandler.EHSClaimConfirmMessageGetFromSession(FunctionCode)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
            MyBase.SessionHandler.EHSClaimStepsRemoveFromSession(FunctionCode)

            Select Case intActiveViewSource
                Case ActiveViewIndex.Step2a
                    ' Construct the dynamic control
                    Dim udtEHSAccount As EHSAccountModel = SessionHandler.EHSAccountGetFromSession(FunctionCode)
                    Me.SetupStep2a(udtEHSAccount, True, False)

                    Dim udtRuleResultCollection As RuleResultCollection = MyBase.SessionHandler.EligibleResultGetFromSession()
                    Dim blnConfirmByClaimRule As Boolean = False
                    Dim blnConfirmByDupClaim As Boolean = False

                    If Not udtRuleResultCollection Is Nothing Then
                        For Each udtRuleResult As RuleResult In udtRuleResultCollection.Values
                            If udtRuleResult.PromptConfirmed = False Then
                                blnConfirmByClaimRule = True
                                Exit For
                            End If
                        Next
                    End If

                    ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [Start][Winnie]
                    If Not blnConfirmByClaimRule Then
                        If objConfirmMessage.Equals(DuplicateClaimAlertMessage) Then
                            SessionHandler.NoticedDuplicateClaimAlertSaveToSession(YesNo.Yes)
                            EHSClaimBasePage.AuditLogTextOnlyVersionDuplicateClaimAlertProceedClick(_udtAuditLogEntry)
                            blnConfirmByDupClaim = True
                        End If
                    End If

                    If blnConfirmByClaimRule OrElse blnConfirmByDupClaim Then
                        ' Handle Confirmation Prompt from Step2a
                        Step2aClaimSubmit(True)

                        ' Handle 2 Confirmation Popup. retain the View Session
                        If Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.ConfirmBox Then
                            MyBase.SessionHandler.EHSClaimStepsSaveToSession(FunctionCode, ActiveViewIndex.Step2a)
                        End If

                    Else
                        ' Handle Confrim Cancel
                        SessionHandler.EHSClaimSessionRemove(FunctionCode)
                        Me.Step2aClear()
                        Me.Step1Clear(False)
                        Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1

                    End If
                    ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [End][Winnie]

                Case ActiveViewIndex.Step2b
                    Step2bClaimSubmit()

                Case ActiveViewIndex.VaccinationRecord
                    ' Handle Confirm Cancel
                    SessionHandler.EHSClaimSessionRemove(FunctionCode)
                    Me.Step2aClear()
                    Me.Step1Clear(False)
                    Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1

            End Select
        End Sub

        Private Sub SetupConfirmBox()
            ' Set Confirmation Message
            Dim obj As Object = SessionHandler.EHSClaimConfirmMessageGetFromSession(FunctionCode)
            If TypeOf obj Is String Then

                ' Title
                ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [Start][Winnie]
                If obj.Equals("ProvidedInfoTrueClaimSP") Then
                    Me.lblConfirmOnlyBoxTitle.Text = Me.GetGlobalResourceObject("Text", "Declaration")
                    Me.btnConfirmBoxConfirm.Text = Me.GetGlobalResourceObject("AlternateText", "ConfirmBtn")
                ElseIf obj.Equals(DuplicateClaimAlertMessage) Then
                    Me.lblConfirmOnlyBoxTitle.Text = Me.GetGlobalResourceObject("Text", "DuplicateClaimAlertTitle")
                    Me.btnConfirmBoxConfirm.Text = Me.GetGlobalResourceObject("AlternateText", "ProceedBtn")
                Else
                    Me.lblConfirmOnlyBoxTitle.Text = Me.GetGlobalResourceObject("Text", "ConfirmBoxTitle")
                    Me.btnConfirmBoxConfirm.Text = Me.GetGlobalResourceObject("AlternateText", "ConfirmBtn")
                End If
                ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [End][Winnie]

                ' Message
                Me.lblConfirmBoxMessage.Text = Me.GetGlobalResourceObject("Text", CType(obj, String))
            ElseIf TypeOf obj Is ClaimRuleResult Then
                ' Title
                Me.lblConfirmOnlyBoxTitle.Text = Me.GetGlobalResourceObject("Text", "ConfirmBoxTitle")

                ' Message
                Me.lblConfirmBoxMessage.Text = Me.Step2aPromptClaimRule(CType(obj, ClaimRuleResult))
            End If

        End Sub

        Private Sub ClearConfirmBox()
            SessionHandler.EHSClaimConfirmMessageRemoveFromSession(FunctionCode)
            Me.lblConfirmBoxMessage.Text = String.Empty
        End Sub

#End Region


#Region "Step of Print Option"

        'Evnet
        Private Sub btnPrintOptionSelectionBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrintOptionSelectionBack.Click
            ' Cancel Clicked, return to the confirm claim page
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step2b

        End Sub

        Private Sub btnPrintOptionSelectionSelect_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrintOptionSelectionSelect.Click
            Dim strSelectedPrintingOption As String = Me.udtPrintOptionSelection.getSelection()
            Dim validator As Common.Validation.Validator = New Common.Validation.Validator()
            Dim udtEHSTransaction As EHSTransactionModel = SessionHandler.EHSTransactionGetFromSession(FunctionCode)
            Dim udtSchemeClaim As SchemeClaimModel = SessionHandler.SchemeSelectedGetFromSession(FunctionCode)

            Dim blnIsValid As Boolean = True
            Dim udtSysMsgPrintOption As SystemMessage = validator.chkSelectedPrintFormOption(strSelectedPrintingOption)
            If Not udtSysMsgPrintOption Is Nothing Then
                ' Error
                blnIsValid = False
                ShowError(udtSysMsgPrintOption)
            End If

            If blnIsValid Then
                SessionHandler.CurrentUserGetFromSession(Me._udtSP, Me._udtDataEntryModel)

                If Me._udtDataEntryModel Is Nothing Then
                    'if Print Option changed
                    If strSelectedPrintingOption <> Me._udtSP.PrintOption Then
                        'Assign Print Option to Servive Provider
                        Me._udtSP.PrintOption = strSelectedPrintingOption
                        Me._udtClaimVoucherBLL.updatePrintOption(Me._udtSP.SPID, String.Empty, Me._udtSP.PrintOption)

                        'Save to Session
                        SessionHandler.CurrentUserSaveToSession(Me._udtSP, Nothing)
                        Me.ChangePrintFormControl(Me._udtSP.PrintOption, udtSchemeClaim)
                        udtEHSTransaction.PrintedConsentForm = False
                        SessionHandler.EHSTransactionSaveToSession(udtEHSTransaction, FunctionCode)
                        Me.HandlePrintOptionChanged()
                    End If
                Else
                    'if Print Option changed
                    If strSelectedPrintingOption <> Me._udtDataEntryModel.PrintOption Then
                        Me._udtDataEntryModel.PrintOption = strSelectedPrintingOption

                        'CRE15-003 Print Tx No. Prefix into Consent Form [Start][Winnie]
                        'Me._udtClaimVoucherBLL.updatePrintOption(Me._udtSP.SPID, Me._udtDataEntryModel.DataEntryAccount, Me._udtSP.PrintOption)
                        Me._udtClaimVoucherBLL.updatePrintOption(Me._udtSP.SPID, Me._udtDataEntryModel.DataEntryAccount, Me._udtDataEntryModel.PrintOption)
                        'CRE15-003 Print Tx No. Prefix into Consent Form [End][Winnie]

                        'Save to Session
                        SessionHandler.CurrentUserSaveToSession(Me._udtSP, Me._udtDataEntryModel)
                        Me.ChangePrintFormControl(Me._udtDataEntryModel.PrintOption, udtSchemeClaim)
                        udtEHSTransaction.PrintedConsentForm = False
                        SessionHandler.EHSTransactionSaveToSession(udtEHSTransaction, FunctionCode)
                        Me.HandlePrintOptionChanged()
                    End If
                End If
                EHSClaimBasePage.AuditLogPrintOptionSelected(_udtAuditLogEntry, strSelectedPrintingOption)

                'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                Me.hfCurrentPrintOption.Value = strSelectedPrintingOption
                'CRE13-019-02 Extend HCVS to China [End][Winnie]

                ' return to the confirm claim page
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step2b
            Else
                'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                Me.udcMsgBoxErr.AddMessage(udtSysMsgPrintOption)
                'CRE13-019-02 Extend HCVS to China [End][Winnie]
                Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail, _udtAuditLogEntry, Common.Component.LogID.LOG00022, "Print Option Selection Fail")
            End If

        End Sub

#End Region


#Region "Step of Adhoc Print"
        ' Event
        Private Sub btnAddHocPrintSelectionBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddHocPrintSelectionBack.Click
            ' Cancel Clicked, to back to confirm cliam page 
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step2b

        End Sub

        Private Sub btnAddHocPrintSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddHocPrintSelection.Click
            ' check selection
            Dim strPrintOption As String = String.Empty

            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            'If rbPrintFullChi.Checked Then
            '    strPrintOption = PrintOptionValue.FullChi
            'ElseIf rbPrintFull.Checked Then
            '    strPrintOption = PrintOptionValue.FullEng
            'ElseIf rbPrintCondencedChi.Checked Then
            '    strPrintOption = PrintOptionValue.CondensedChi
            'ElseIf rbPrintCondenced.Checked Then
            '    strPrintOption = PrintOptionValue.CondensedEng
            'End If

            Dim strPrintOptionSelectedLang As String = Nothing
            Dim strPrintOptionSelectedVersion As String = Nothing

            Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode)

            Dim slConsentFormAvailableLang As String() = Nothing

            slConsentFormAvailableLang = udtSchemeClaim.SubsidizeGroupClaimList(0).ConsentFormAvailableLang


            If Me.rbAdhocPrintCondenseLang1.Checked Then
                strPrintOptionSelectedLang = slConsentFormAvailableLang(0)
                strPrintOptionSelectedVersion = PrintFormOptionValue.PrintConsentOnly
            ElseIf Me.rbAdhocPrintCondenseLang2.Checked Then
                strPrintOptionSelectedLang = slConsentFormAvailableLang(1)
                strPrintOptionSelectedVersion = PrintFormOptionValue.PrintConsentOnly
            ElseIf Me.rbAdhocPrintCondenseLang3.Checked Then
                strPrintOptionSelectedLang = slConsentFormAvailableLang(2)
                strPrintOptionSelectedVersion = PrintFormOptionValue.PrintConsentOnly
            ElseIf Me.rbAdhocPrintFullLang1.Checked Then
                strPrintOptionSelectedLang = slConsentFormAvailableLang(0)
                strPrintOptionSelectedVersion = PrintFormOptionValue.PrintPurposeAndConsent
            ElseIf Me.rbAdhocPrintFullLang2.Checked Then
                strPrintOptionSelectedLang = slConsentFormAvailableLang(1)
                strPrintOptionSelectedVersion = PrintFormOptionValue.PrintPurposeAndConsent
            ElseIf Me.rbAdhocPrintFullLang3.Checked Then
                strPrintOptionSelectedLang = slConsentFormAvailableLang(2)
                strPrintOptionSelectedVersion = PrintFormOptionValue.PrintPurposeAndConsent
            End If

            Select Case strPrintOptionSelectedLang
                Case PrintOptionLanguage.TradChinese
                    If strPrintOptionSelectedVersion = PrintFormOptionValue.PrintConsentOnly Then
                        strPrintOption = PrintOptionValue.CondensedChi
                    ElseIf strPrintOptionSelectedVersion = PrintFormOptionValue.PrintPurposeAndConsent Then
                        strPrintOption = PrintOptionValue.FullChi
                    End If
                Case PrintOptionLanguage.English
                    If strPrintOptionSelectedVersion = PrintFormOptionValue.PrintConsentOnly Then
                        strPrintOption = PrintOptionValue.CondensedEng
                    ElseIf strPrintOptionSelectedVersion = PrintFormOptionValue.PrintPurposeAndConsent Then
                        strPrintOption = PrintOptionValue.FullEng
                    End If
                Case PrintOptionLanguage.SimpChinese
                    If strPrintOptionSelectedVersion = PrintFormOptionValue.PrintConsentOnly Then
                        strPrintOption = PrintOptionValue.CondensedSimpChi
                    ElseIf strPrintOptionSelectedVersion = PrintFormOptionValue.PrintPurposeAndConsent Then
                        strPrintOption = PrintOptionValue.FullSimpChi
                    End If
            End Select
            'CRE13-019-02 Extend HCVS to China [End][Winnie]

            If Not String.IsNullOrEmpty(strPrintOption) Then
                ' Prompt a popup windows for the printout and return to the confirm claim page
                PrintPrintout(strPrintOption, True)

                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step2b
            End If

        End Sub

#End Region


#Region "Step of Remark"

        ' Event
        Private Sub btnViewRemarkReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewRemarkReturn.Click
            ' Cancel Clicked, move to the previous View
            Dim intView As Integer = MyBase.SessionHandler.EHSClaimStepsGetFromSession(FunctionCode)

            MyBase.SessionHandler.EHSClaimStepsRemoveFromSession(FunctionCode)

            Me.mvEHSClaim.ActiveViewIndex = intView

        End Sub

#End Region


#Region "Step of Input Tip"

        ' Event
        Private Sub btnInputTipReturn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnInputTipReturn.Click
            ' Return to previous view
            Dim intViewIndex As Integer = MyBase.SessionHandler.EHSClaimStepsGetFromSession(FunctionCode)

            MyBase.SessionHandler.EHSClaimStepsRemoveFromSession(FunctionCode)

            Me.mvEHSClaim.ActiveViewIndex = intViewIndex

        End Sub

        ' Function
        Private Sub SetupInputTips()
            Me.ucInputTipsControl.LoadTip()
        End Sub

#End Region


#Region "Step of Reason For Visit"

        ' Event
        Private Sub udcReasonForVisit_ReturnButtonClick(ByVal sender As Object, ByVal e As EventArgs) Handles udcReasonForVisit.ReturnButtonClick
            Me.SetStep2aInputEHSClaimControlReadFromSession(True)
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step2a
        End Sub

        Private Sub udcReasonForVisit_ConfirmButtonClick(ByVal sender As Object, ByVal e As EventArgs) Handles udcReasonForVisit.ConfirmButtonClick
            Me.SetStep2aInputEHSClaimControlReadFromSession(True)
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step2a
        End Sub

        ' Function
        Private Sub SetupReasonForVisit()

            Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
            Dim udtEHSAccount As EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(FunctionCode)

            ' CRE19-003 (Opt voucher capping) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Dim udtSelectedPracticeDisplay As PracticeDisplayModel = MyBase.SessionHandler.PracticeDisplayGetFromSession(FunctionCode)
            ' CRE19-003 (Opt voucher capping) [End][Winnie]

            Dim blnIsAvailable As Boolean = False

            ' Build control only when the scheme have subsidize available
            If Not udtSchemeClaim Is Nothing AndAlso Not udtSchemeClaim.SubsidizeGroupClaimList Is Nothing AndAlso Not udtEHSAccount Is Nothing Then

                Select Case udtSchemeClaim.ControlType

                    Case SchemeClaimModel.EnumControlType.VOUCHER ', SchemeClaimModel.EnumControlType.VOUCHERCHINA
                        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                        If udtEHSAccount.VoucherInfo Is Nothing Then
                            Dim udtVoucherInfo As New VoucherInfoModel(VoucherInfoModel.AvailableVoucher.Include, _
                                                                       VoucherInfoModel.AvailableQuota.Include)

                            udtVoucherInfo.GetInfo(MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode), _
                                                   udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode), _
                                                   udtSelectedPracticeDisplay.ServiceCategoryCode)

                            udtEHSAccount.VoucherInfo = udtVoucherInfo

                            MyBase.SessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctionCode)
                        End If

                        If udtEHSAccount.VoucherInfo.GetAvailableVoucher() > 0 Then
                            ' CRE19-003 (Opt voucher capping) [Start][Winnie]
                            ' ----------------------------------------------------------------------------------------
                            Dim udtVoucherQuota As VoucherQuotaModel = udtEHSAccount.VoucherInfo.VoucherQuotalist.Filter(udtSelectedPracticeDisplay.ServiceCategoryCode)

                            If Not udtVoucherQuota Is Nothing Then
                                If udtVoucherQuota.AvailableQuota > 0 Then
                                    blnIsAvailable = True
                                End If
                            Else
                                blnIsAvailable = True
                            End If
                            ' CRE19-003 (Opt voucher capping) [End][Winnie]                            
                        End If
                        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]
                End Select

            End If

            If blnIsAvailable Then
                ' INT13-0022 - Fix some special handling on HCSP text only version [Start][Koala]
                ' -------------------------------------------------------------------------------------
                Me.udcReasonForVisit.IsSupportedDevice = True
                ' INT13-0022 - Fix some special handling on HCSP text only version [End][Koala]
                Me.udcReasonForVisit.AvaliableForClaim = blnIsAvailable
                Me.udcReasonForVisit.CurrentPractice = MyBase.SessionHandler.PracticeDisplayGetFromSession(FunctionCode)
                Me.udcReasonForVisit.EHSAccount = udtEHSAccount
                Me.udcReasonForVisit.EHSTransaction = MyBase.SessionHandler.EHSTransactionGetFromSession(FunctionCode)
                Me.udcReasonForVisit.Build()
            Else
                Me.ClearReasonForVisit()
            End If

        End Sub

        Private Sub ClearReasonForVisit()
            Me.udcReasonForVisit.Clear()
        End Sub

#End Region


#Region "Step of Category"

        '' Event
        'Protected Sub btnSelectCategoryBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectCategoryBack.Click
        '    Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step2a
        'End Sub

        Protected Sub btnSelectCategoryConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectCategoryConfirm.Click

            If String.IsNullOrEmpty(Me.rbCategory.SelectedValue) Then
                Me.SessionHandler.ClaimCategoryRemoveFromSession(FunctionCode)
                Me.udcMsgBoxErr.AddMessage("990000", "E", "00238")
                Me.udcMsgBoxErr.BuildMessageBox(_strValidationFail)
            Else
                Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
                Dim udtEHSAccount As EHSAccount.EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(FunctionCode)
                Dim udtPersonalInformation As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode)
                Dim udtClaimCategorys As ClaimCategoryModelCollection
                Dim udtClaimCategory As ClaimCategoryModel
                Dim dtmServiceDate As DateTime

                If Me.txtStep2aServiceDate.Text.Equals(String.Empty) Then
                    dtmServiceDate = DateTime.Now()
                Else
                    dtmServiceDate = Me._udtFormatter.convertDate(Me.txtStep2aServiceDate.Text, Common.Component.CultureLanguage.English)
                End If

                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                udtClaimCategorys = Me._udtClaimCategoryBLL.getDistinctCategoryByScheme(udtSchemeClaim, udtPersonalInformation, dtmServiceDate)
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                udtClaimCategory = udtClaimCategorys.FilterByCategoryCode(udtSchemeClaim.SchemeCode, Me.rbCategory.SelectedValue)

                Dim udtSessionClaimCategory As ClaimCategoryModel = SessionHandler.ClaimCategoryGetFromSession(FunctionCode)
                If udtSessionClaimCategory Is Nothing OrElse udtClaimCategory Is Nothing OrElse udtSessionClaimCategory.CategoryCode <> udtClaimCategory.CategoryCode Then
                    ' Force to rebuild the Input Claim Control when the Category is updated
                    udcStep2aInputEHSClaim.SetRebuildRequired()
                End If

                MyBase.SessionHandler.ClaimCategorySaveToSession(udtClaimCategory, FunctionCode)
                MyBase.SessionHandler.EHSClaimVaccineRemoveFromSession()

                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step2a
            End If

        End Sub

        ' Function
        Private Sub SetupCategory(ByVal strSelectedValue As String)
            Dim dtCategory As DataTable
            Dim listItem As ListItem
            Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
            Dim dtmServiceDate As DateTime
            Dim udtClaimCategorys As ClaimCategoryModelCollection
            Dim udtClaimCategory As ClaimCategoryModel = MyBase.SessionHandler.ClaimCategoryGetFromSession(FunctionCode)
            Dim udtEHSAccount As EHSAccount.EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(FunctionCode)
            Dim udtPersonalInformation As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode)

            If Me.txtStep2aServiceDate.Text.Equals(String.Empty) Then
                dtmServiceDate = DateTime.Now()
            Else
                dtmServiceDate = Me._udtFormatter.convertDate(Me.txtStep2aServiceDate.Text, Common.Component.CultureLanguage.English)
            End If

            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            udtClaimCategorys = Me._udtClaimCategoryBLL.getDistinctCategoryByScheme(udtSchemeClaim, udtPersonalInformation, dtmServiceDate)
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            dtCategory = ClaimCategoryBLL.ConvertCategoryToDatatable(udtClaimCategorys)

            If String.IsNullOrEmpty(strSelectedValue) Then
                strSelectedValue = Me.rbCategory.SelectedValue
            End If

            'Select Case udtSchemeClaim.SchemeCode.Trim()
            '    Case SchemeClaimModel.HSIVSS
            '        dtCategory = Me._udtStaticDataBLL.GetStaticDataList("Role")

            '    Case SchemeClaimModel.RVP
            '        dtCategory = Me._udtStaticDataBLL.GetStaticDataList("RVPRole")
            'End Select

            Me.rbCategory.DataSource = dtCategory

            Me.rbCategory.DataValueField = ClaimCategoryModel._Category_Code

            If Me.SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                Me.rbCategory.DataTextField = ClaimCategoryModel._Category_Name_Chi
            Else
                Me.rbCategory.DataTextField = ClaimCategoryModel._Category_Name
            End If

            Me.rbCategory.DataBind()

            listItem = Me.rbCategory.Items.FindByValue(strSelectedValue)
            If Not listItem Is Nothing Then
                Me.rbCategory.SelectedValue = strSelectedValue
            End If

            'If Me.SessionHandler.ClaimCategoryGetFromSession(FunctionCode) Is Nothing Then
            '    Me.btnSelectCategoryBack.Visible = False
            'Else
            '    Me.btnSelectCategoryBack.Visible = True
            '    Me.btnSelectCategoryBack.Text = Me.GetGlobalResourceObject("AlternateText", "BackBtn")
            'End If

        End Sub

        Private Sub StepSelectCategoryClean()
            Me.rbCategory.Items.Clear()
        End Sub

#End Region

#Region "Vaccination Record"

        Protected Sub btnVRContinue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVRContinue.Click
            ViewState(VS.VaccinationRecordPopupStatus) = VaccinationRecordPopupStatusClass.Closed
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step2a

        End Sub

        Protected Sub btnVRReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVRReturn.Click
            If (MyBase.IsPageRefreshed) Then
                _blnIsRequireHandlePageRefresh = True
                Return
            End If

            ' To Handle Concurrent Browser
            If Not Me.EHSClaimTokenNumValidation(hfEHSClaimTokenNum.Value, FunctionCode) Then
                _blnIsConcurrentBrowserDetected = True
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.InternalError
                Return
            End If

            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step2a
        End Sub

        '

        Private Sub SetupVaccinationRecord()
            Dim udtEHSAccount As EHSAccountModel = SessionHandler.EHSAccountGetFromSession(FunctionCode)

            Me.udcVaccinationRecordReadOnlyDocumnetType.Clear()

            Me.udcVaccinationRecordReadOnlyDocumnetType.TextOnlyVersion = True
            ' INT13-0021 - Fix use HKBC on smart IC claim incorrectly [Start][Koala]
            ' -------------------------------------------------------------------------------------
            Me.udcVaccinationRecordReadOnlyDocumnetType.DocumentType = udtEHSAccount.SearchDocCode
            'Me.udcVaccinationRecordReadOnlyDocumnetType.DocumentType = udtEHSAccount.EHSPersonalInformationList(0).DocCode.Trim()
            ' INT13-0021 - Fix use HKBC on smart IC claim incorrectly [End][Koala]
            Me.udcVaccinationRecordReadOnlyDocumnetType.EHSAccount = udtEHSAccount
            Me.udcVaccinationRecordReadOnlyDocumnetType.Vertical = False
            Me.udcVaccinationRecordReadOnlyDocumnetType.MaskIdentityNo = True
            Me.udcVaccinationRecordReadOnlyDocumnetType.ShowAccountRefNo = False
            Me.udcVaccinationRecordReadOnlyDocumnetType.ShowTempAccountNotice = False
            Me.udcVaccinationRecordReadOnlyDocumnetType.ShowAccountCreationDate = False

            Dim udtSmartIDContent As BLL.SmartIDContentModel = MyBase.SessionHandler.SmartIDContentGetFormSession(FunctionCode)
            If Not udtSmartIDContent Is Nothing _
                    AndAlso udtSmartIDContent.IsReadSmartID _
                    AndAlso udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount _
                    AndAlso SmartIDShowRealID() Then
                udcVaccinationRecordReadOnlyDocumnetType.IsSmartID = True

            Else
                udcVaccinationRecordReadOnlyDocumnetType.IsSmartID = False

            End If

            Me.udcVaccinationRecordReadOnlyDocumnetType.Built()

            udcVaccinationRecord.RebuildVaccinationRecordGrid()

        End Sub

        Private Sub ShowVaccinationRecord(ByVal blnRecall As Boolean)
            mvEHSClaim.ActiveViewIndex = ActiveViewIndex.VaccinationRecord

            Dim udtEHSAccount As EHSAccountModel = SessionHandler.EHSAccountGetFromSession(FunctionCode)
            ' INT13-0022 - Fix some special handling on HCSP text only version [Start][Koala]
            ' -------------------------------------------------------------------------------------
            Dim udtSystemMessageList As List(Of SystemMessage) = udcVaccinationRecord.Build(udtEHSAccount, _udtAuditLogEntry, True)
            ' INT13-0022 - Fix some special handling on HCSP text only version [End][Koala]

            Me.udcVaccinationRecordReadOnlyDocumnetType.Clear()

            Me.udcVaccinationRecordReadOnlyDocumnetType.TextOnlyVersion = True
            ' INT13-0021 - Fix use HKBC on smart IC claim incorrectly [Start][Koala]
            ' -------------------------------------------------------------------------------------
            Me.udcVaccinationRecordReadOnlyDocumnetType.DocumentType = udtEHSAccount.SearchDocCode
            'Me.udcVaccinationRecordReadOnlyDocumnetType.DocumentType = udtEHSAccount.EHSPersonalInformationList(0).DocCode.Trim()
            ' INT13-0021 - Fix use HKBC on smart IC claim incorrectly [End][Koala]
            Me.udcVaccinationRecordReadOnlyDocumnetType.EHSAccount = udtEHSAccount
            Me.udcVaccinationRecordReadOnlyDocumnetType.Vertical = False
            Me.udcVaccinationRecordReadOnlyDocumnetType.MaskIdentityNo = True
            Me.udcVaccinationRecordReadOnlyDocumnetType.ShowAccountRefNo = False
            Me.udcVaccinationRecordReadOnlyDocumnetType.ShowTempAccountNotice = False
            Me.udcVaccinationRecordReadOnlyDocumnetType.ShowAccountCreationDate = False

            Dim udtSmartIDContent As BLL.SmartIDContentModel = MyBase.SessionHandler.SmartIDContentGetFormSession(FunctionCode)
            If Not udtSmartIDContent Is Nothing _
                    AndAlso udtSmartIDContent.IsReadSmartID _
                    AndAlso udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount _
                    AndAlso SmartIDShowRealID() Then
                udcVaccinationRecordReadOnlyDocumnetType.IsSmartID = True

            Else
                udcVaccinationRecordReadOnlyDocumnetType.IsSmartID = False

            End If

            Me.udcVaccinationRecordReadOnlyDocumnetType.Built()

            btnVRContinue.Visible = Not blnRecall
            btnVRReturn.Visible = blnRecall

            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
            ' ----------------------------------------------------------
            ' Build system message
            For Each udtSystemMessage As SystemMessage In udtSystemMessageList
                If Not IsNothing(udtSystemMessage) Then
                    If udtSystemMessage.SeverityCode = SeverityCode.SEVI Then
                        udcMsgBoxInfo.AddMessage(udtSystemMessage)
                        udcMsgBoxInfo.BuildMessageBox()
                    Else
                        Dim lstIdx As New List(Of String)
                        Dim lstReplaceMessage As New List(Of String)
                        udtSystemMessage.GetReplaceMessage(String.Empty, lstIdx, lstReplaceMessage)

                        udcMsgBoxErr.AddMessage(udtSystemMessage, New String() {lstIdx(0), lstIdx(1), lstIdx(2), lstIdx(3)}, New String() {lstReplaceMessage(0), lstReplaceMessage(1), lstReplaceMessage(2), lstReplaceMessage(3)})
                        udcMsgBoxErr.BuildMessageBox("ConnectionFail")
                    End If
                End If
            Next
            ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        End Sub

        Private Function CheckShowVaccinationRecord() As Boolean
            Select Case ViewState(VS.VaccinationRecordPopupStatus)
                Case Nothing
                    ShowVaccinationRecord(False)
                    Return True
            End Select

        End Function

#End Region

#Region "Common Function"

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

        Public Sub ShowError(ByVal systemMessages As SystemMessage(), ByVal udtAuditLogEntry As AuditLogEntry, ByVal strLogID As String, ByVal strLogMessage As String)
            ShowError(systemMessages, udtAuditLogEntry, strLogID, strLogMessage, Nothing)
        End Sub


        Public Sub ShowError(ByVal systemMessages As SystemMessage(), ByVal udtAuditLogEntry As AuditLogEntry, ByVal strLogID As String, ByVal strLogMessage As String, ByVal objAuditLogInfo As AuditLogInfo)
            For Each systemMessage As SystemMessage In systemMessages
                Me.udcMsgBoxErr.AddMessage(systemMessage)
            Next
            'Me.udcMsgBoxErr.Visible = True

            If Not udtAuditLogEntry Is Nothing Then
                Me.udcMsgBoxErr.BuildMessageBox(_strValidationFail, udtAuditLogEntry, strLogID, strLogMessage, objAuditLogInfo)
            Else
                Me.udcMsgBoxErr.BuildMessageBox(_strValidationFail)
            End If
        End Sub

        Public Sub ShowError(ByVal systemMessages As SystemMessage())
            Me.ShowError(systemMessages, Nothing, Nothing, Nothing)
        End Sub

        Public Sub ShowInfoMessage(ByVal systemMessage As SystemMessage)
            Me.udcMsgBoxInfo.AddMessage(systemMessage)
            Me.udcMsgBoxInfo.BuildMessageBox()
            Me.udcMsgBoxInfo.Visible = True

        End Sub

#End Region

        ' Build Menu
        Private Sub BuildMenuItem(ByVal masterPage As ClaimVoucherMaster)
            ' Menu not apply in the claim process
            Dim udtEHSAccount As EHSAccountModel = SessionHandler.EHSAccountGetFromSession(FunctionCode)
            If Not Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.ConfirmBox AndAlso (udtEHSAccount Is Nothing OrElse Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step3) Then
                masterPage.BuildMenu(FunctionCode, SessionHandler.Language())
            Else
                masterPage.ClearMenu()
            End If

        End Sub

        '-------------------------------------------------------------------------------------------------------------------
        ' Get current user Account from database and save to session
        '-------------------------------------------------------------------------------------------------------------------
        Private Sub GetCurrentUserAccount(ByRef passedSP As ServiceProviderModel, ByRef passedDataEntry As DataEntryUserModel, ByVal getFormDataBase As Boolean)
            'Get Current USer Account
            Me._udtUserAC = UserACBLL.GetUserAC

            If getFormDataBase Then

                If Me._udtUserAC.UserType = Common.Component.SPAcctType.ServiceProvider Then
                    'Get SP from form database
                    passedSP = CType(_udtUserAC, ServiceProviderModel)
                    passedSP = Me._udtClaimVoucherBLL.loadSP(passedSP.SPID)

                    passedDataEntry = Nothing

                ElseIf Me._udtUserAC.UserType = Common.Component.SPAcctType.DataEntryAcct Then
                    passedDataEntry = CType(_udtUserAC, DataEntryUserModel)

                    'Get the latest data entry account mordel form database
                    Dim udtDataEntryAcctBLL As BLL.DataEntryAcctBLL = New BLL.DataEntryAcctBLL
                    passedDataEntry = udtDataEntryAcctBLL.LoadDataEntry(passedDataEntry.SPID, passedDataEntry.DataEntryAccount)

                    'Get the latest service provider account mordel 
                    passedSP = Me._udtClaimVoucherBLL.loadSP(passedDataEntry.SPID)
                End If

                SessionHandler.CurrentUserSaveToSession(passedSP, passedDataEntry)
            Else
                SessionHandler.CurrentUserGetFromSession(passedSP, passedDataEntry)
            End If

        End Sub

        '-------------------------------------------------------------------------------------------------------------------
        ' Clear Claim for new patient
        '-------------------------------------------------------------------------------------------------------------------
        Private Sub Clear()

            Me.Step1Clear(False)
            Me.ClearEHSClaimState()

            Me.StepSelectPracticeClear()
            Me.StepSelectSchemeClear()
            Me.StepSelectDocumentTypeClear()

        End Sub

        Private Sub ClearEHSClaimState()
            Me.Step2aClear()
            Me.Step2bClear()
            Me.Step3Clear()

            MyBase.SessionHandler.EHSClaimSessionRemove(FunctionCode)

        End Sub

        Private Sub ClearAll()
            Me.Clear()

            SessionHandler.PracticeDisplayRemoveFromSession(FunctionCode)
            SessionHandler.SchemeSelectedRemoveFromSession(FunctionCode)
            SessionHandler.DocumentTypeSelectedRemoveFromSession(FunctionCode)
            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            SessionHandler.NonClinicSettingRemoveFromSession(FunctionCode)
            SessionHandler.ChangeSchemeInPracticeRemoveFromSession(FunctionCode)
            'CRE16-002 (Revamp VSS) [End][Chris YIM]
        End Sub

        '-------------------------------------------------------------------------------------------------------------------
        ' Fill in Session's Select Practice into a label
        '-------------------------------------------------------------------------------------------------------------------
        Private Sub FillPracticeText(ByRef ctrlLabel As Label)

            Dim udtPracticeDisplay As BLL.PracticeDisplayModel = SessionHandler.PracticeDisplayGetFromSession(FunctionCode)
            If udtPracticeDisplay Is Nothing Then
                ' Throw New Exception("Practice Empty!")
                ctrlLabel.Text = String.Empty
            Else
                Dim strPracticeName As String = IIf(String.IsNullOrEmpty(udtPracticeDisplay.PracticeNameChi) OrElse SessionHandler.Language = CultureLanguage.English, udtPracticeDisplay.PracticeName, udtPracticeDisplay.PracticeNameChi)
                ctrlLabel.Text = String.Format("{0} ({1})", strPracticeName, udtPracticeDisplay.PracticeID)
            End If

            If MyBase.SessionHandler.Language() = Common.Component.CultureLanguage.TradChinese Then
                ctrlLabel.CssClass = "tableTextChi"
            Else
                ctrlLabel.CssClass = "tableText"
            End If

        End Sub

        '-------------------------------------------------------------------------------------------------------------------
        ' Fill in Session's Select Scheme into a label
        '-------------------------------------------------------------------------------------------------------------------
        Private Sub FillSchemeText(ByRef ctrlLabel As Label)

            Dim udtSchemeClaim As SchemeClaimModel = SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
            If udtSchemeClaim Is Nothing Then
                ' Throw New Exception("Scheme Empty!")
                ctrlLabel.Text = String.Empty
            Else
                'ctrlLabel.Text = IIf(SessionHandler.Language = CultureLanguage.English, udtSchemeClaim.SchemeDesc, udtSchemeClaim.SchemeDescChi)
                If SessionHandler.Language = CultureLanguage.TradChinese Then
                    ctrlLabel.Text = udtSchemeClaim.SchemeDescChi
                ElseIf SessionHandler.Language = CultureLanguage.SimpChinese Then
                    ctrlLabel.Text = udtSchemeClaim.SchemeDescCN
                Else
                    ctrlLabel.Text = udtSchemeClaim.SchemeDesc
                End If

                If SessionHandler.NonClinicSettingGetFromSession(FunctionCode) Then
                    panNonClinicSetting.Visible = True

                    If SessionHandler.Language = CultureLanguage.TradChinese Then
                        ctrlLabel.Text = ctrlLabel.Text + String.Format("<br>({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)))
                    ElseIf SessionHandler.Language = CultureLanguage.SimpChinese Then
                        ctrlLabel.Text = ctrlLabel.Text + String.Format("<br>({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese)))
                    Else
                        ctrlLabel.Text = ctrlLabel.Text + String.Format("<br>({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.English)))
                    End If
                Else
                    panNonClinicSetting.Visible = False
                End If
            End If

        End Sub

        '-------------------------------------------------------------------------------------------------------------------
        ' Fill in Session's Select Document Type into a label
        '-------------------------------------------------------------------------------------------------------------------
        Private Sub FillDocumentTypeText(ByRef ctrlLabel As Label)

            Dim strDocumentType As String = SessionHandler.DocumentTypeSelectedGetFromSession(FunctionCode)
            If String.IsNullOrEmpty(strDocumentType) Then
                ' Throw New Exception("Document Type Empty!")
                ctrlLabel.Text = String.Empty
            Else
                Dim udtDocTypeBLL As New DocTypeBLL()
                Dim udtDocType As DocType.DocTypeModel = udtDocTypeBLL.getAllDocType().Filter(strDocumentType)

                ctrlLabel.Text = IIf(SessionHandler.Language = CultureLanguage.English, udtDocType.DocName, udtDocType.DocNameChi)
            End If

        End Sub

        Private Function ValidateViewStatus() As Boolean
            Dim blnIsValid As Boolean = True
            ' Handle Client side page back
            Select Case Me.mvEHSClaim.ActiveViewIndex
                Case ActiveViewIndex.SelectScheme
                    If SessionHandler.PracticeDisplayGetFromSession(FunctionCode) Is Nothing Then
                        blnIsValid = False
                    End If

                Case ActiveViewIndex.SelectDocType
                    If SessionHandler.PracticeDisplayGetFromSession(FunctionCode) Is Nothing OrElse _
                       SessionHandler.SchemeSelectedGetFromSession(FunctionCode) Is Nothing Then
                        blnIsValid = False
                    End If

                Case ActiveViewIndex.Step1
                    If SessionHandler.PracticeDisplayGetFromSession(FunctionCode) Is Nothing OrElse _
                       SessionHandler.SchemeSelectedGetFromSession(FunctionCode) Is Nothing OrElse _
                       SessionHandler.DocumentTypeSelectedGetFromSession(FunctionCode) Is Nothing Then
                        blnIsValid = False
                    End If

                Case ActiveViewIndex.Step2a
                    If SessionHandler.EHSAccountGetFromSession(FunctionCode) Is Nothing OrElse _
                        SessionHandler.PracticeDisplayGetFromSession(FunctionCode) Is Nothing Then
                        ' Change to a practice without eligibily scheme
                        ' OrElse SessionHandler.SchemeSelectedGetFromSession(FunctionCode) Is Nothing 
                        blnIsValid = False
                    End If

                Case ActiveViewIndex.Step2b, ActiveViewIndex.PrintOption, ActiveViewIndex.AdHocPrint, ActiveViewIndex.Step3
                    If SessionHandler.EHSAccountGetFromSession(FunctionCode) Is Nothing OrElse _
                       SessionHandler.EHSTransactionGetFromSession(FunctionCode) Is Nothing OrElse _
                       SessionHandler.PracticeDisplayGetFromSession(FunctionCode) Is Nothing OrElse _
                       SessionHandler.SchemeSelectedGetFromSession(FunctionCode) Is Nothing Then
                        blnIsValid = False
                    End If

            End Select

            Return blnIsValid
        End Function

        Private Function IsSchemeAvailableInDocumentType(ByVal udtSchemeClaim As SchemeClaimModel, ByVal strDocumentType As String) As Boolean

            Dim blnIsValid As Boolean = False

            If Not udtSchemeClaim Is Nothing AndAlso Not strDocumentType Is Nothing Then
                Dim udtDocTypeBLL As New DocTypeBLL()
                Dim udtSchemeDocTypeModelCollection As SchemeDocTypeModelCollection = udtDocTypeBLL.getSchemeDocTypeByScheme(udtSchemeClaim.SchemeCode.Trim())

                If Not udtSchemeDocTypeModelCollection Is Nothing Then
                    For Each udtSchemeDocTypeModel As SchemeDocTypeModel In udtSchemeDocTypeModelCollection
                        If udtSchemeDocTypeModel.DocCode.Trim() = strDocumentType.Trim() Then
                            blnIsValid = True
                            Exit For
                        End If
                    Next
                End If
            End If

            Return blnIsValid

        End Function

        ' Return the Default Document Type by Scheme
        Private Function GetDefaultDocumnetTypeByScheme(ByVal strScheme As String) As String
            Dim strDocTypeCode As String = String.Empty

            Dim udtDocTypeBLL As New DocTypeBLL()
            Dim udtSchemeDocTypeList As SchemeDocTypeModelCollection = udtDocTypeBLL.getSchemeDocTypeByScheme(strScheme)
            For Each udtSchemeDocType As SchemeDocTypeModel In udtSchemeDocTypeList
                If udtSchemeDocType.IsMajorDoc Then
                    strDocTypeCode = udtSchemeDocType.DocCode.Trim()
                    Exit For
                End If
            Next

            Return strDocTypeCode
        End Function

        ' Return the number of Practice Available for the SP
        Private Function GetAvailablePractice() As BLL.PracticeDisplayModelCollection
            Return Me.SessionHandler.PracticeDisplayListGetFromSession()
            'Me.GetCurrentUserAccount(Me._udtSP, Me._udtDataEntryModel, False)

            'If Not Me._udtDataEntryModel Is Nothing Then
            '    Return Me._udtPracticeBankAccBLL.getActivePractice(Me._udtSP.SPID, Me._udtDataEntryModel.DataEntryAccount)
            'Else
            '    Return Me._udtPracticeBankAccBLL.getActivePracticeWithAvailableScheme(Me._udtSP.SPID, Me._udtSP.PracticeList)
            'End If

        End Function

        ' Return the number of Scheme Available for the Selected Practice
        Private Function GetAvailableScheme() As SchemeClaimModelCollection

            Me.GetCurrentUserAccount(Me._udtSP, Me._udtDataEntryModel, False)

            ' Check EHSAccount Exists
            Dim udtSchemeClaimBLL As SchemeClaimBLL = New SchemeClaimBLL()
            Dim udtSelectedPracticeDisplay As BLL.PracticeDisplayModel = MyBase.SessionHandler.PracticeDisplayGetFromSession(FunctionCode)
            Dim udtEHSAccount As EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(FunctionCode)
            If Not udtSelectedPracticeDisplay Is Nothing Then
                If udtEHSAccount Is Nothing Then
                    ' Return the Scheme List filtered by Practice only
                    Return udtSchemeClaimBLL.searchValidClaimPeriodSchemeClaimByPracticeSchemeInfoSubsidizeCode(Me._udtSP.PracticeList(udtSelectedPracticeDisplay.PracticeID).PracticeSchemeInfoList, Me._udtSP.SchemeInfoList)
                Else
                    ' Return the Scheme List filtered by EHSAccount
                    Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = Me.SessionHandler.SchemeSubsidizeListGetFromSession(FunctionCode)

                    If udtSchemeClaimModelCollection Is Nothing Then

                        'Get all available Scheme <- for SP
                        udtSchemeClaimModelCollection = udtSchemeClaimBLL.searchValidClaimPeriodSchemeClaimByPracticeSchemeInfoSubsidizeCode(SessionSP.PracticeList(udtSelectedPracticeDisplay.PracticeID).PracticeSchemeInfoList, Me._udtSP.SchemeInfoList)
                        'Get all Eligible Scheme form available List <- for EHS Account
                        udtSchemeClaimModelCollection = udtSchemeClaimBLL.searchEligibleAndExceedDocumentClaimScheme(udtEHSAccount, udtEHSAccount.SearchDocCode, udtSchemeClaimModelCollection)

                        Me.SessionHandler.SchemeSubsidizeListSaveToSession(udtSchemeClaimModelCollection, FunctionCode)
                    End If

                    Return udtSchemeClaimModelCollection
                End If
            Else
                Return New SchemeClaimModelCollection()
            End If

        End Function

        ' Return the number of Document Type Available for the Selected Practice
        Private Function GetAvailableDocumentType() As SchemeDocTypeModelCollection

            Dim udtSchemeClaim As SchemeClaimModel = SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
            If Not udtSchemeClaim Is Nothing Then
                Dim udtDocTypeBLL As New DocTypeBLL()
                Return udtDocTypeBLL.getSchemeDocTypeByScheme(udtSchemeClaim.SchemeCode.Trim())
            Else
                Return Nothing
            End If

        End Function

        Private Function GetCurrentUserPrintOption() As String
            Dim udtDataEnteryUser As DataEntryUserModel = Nothing
            Dim strPrintOption As String = Nothing
            Me.GetCurrentUserAccount(Me._udtSP, udtDataEnteryUser, False)

            If udtDataEnteryUser Is Nothing Then
                strPrintOption = Me._udtSP.PrintOption
            Else
                strPrintOption = udtDataEnteryUser.PrintOption
            End If

            Return strPrintOption
        End Function

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        ' Remove input parameter - strSchemeCode
        'Private Function RemoveRulesAfterConfirm(ByVal strSchemeCode As String, ByRef udtRuleResults As RuleResultCollection, ByVal isValid As Boolean) As Boolean
        Private Function RemoveRulesAfterConfirm(ByRef udtRuleResults As RuleResultCollection, ByVal isValid As Boolean) As Boolean
            ' --------------------------------------------------------------
            ' Eligibility rule
            ' --------------------------------------------------------------
            Dim strKey As String = Me.RuleResultKey(ActiveViewIndex.Step2a, RuleTypeENum.EligibleResult)
            Dim udtRuleResult As RuleResult = udtRuleResults.Item(strKey)

            If Not udtRuleResult Is Nothing Then

                'Should have 2 rule in this collection
                'first : After sreach this account -> rule added and auto make to confirmed
                'Second : After press "Next" in Enter claim detail -> rule added but not confirmed
                If Not udtRuleResult.PromptConfirmed Then
                    udtRuleResults.Remove(strKey)
                    isValid = True
                End If
            End If

            ' --------------------------------------------------------------
            ' Claim rule
            ' --------------------------------------------------------------
            strKey = Me.RuleResultKey(ActiveViewIndex.Step2a, RuleTypeENum.ClaimRuleResult)
            udtRuleResult = udtRuleResults.Item(strKey)
            If isValid Then
                If Not udtRuleResult Is Nothing Then
                    Me.ShowConfirmMessageBox(ConfirmationStyle.Bordered, udtRuleResult)
                    EHSClaimBasePage.AuditLogShowClaimRulePopupBox(_udtAuditLogEntry)
                    isValid = False
                End If
            Else

                If Not udtRuleResult Is Nothing Then
                    If Not udtRuleResult.PromptConfirmed Then
                        udtRuleResults.Remove(strKey)
                        isValid = True
                    End If
                End If
            End If
            Return isValid

        End Function
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        Private Sub ShowConfirmMessageBox(ByVal udtConfirmationStyle As ConfirmationStyle, ByRef objDisplayMessage As Object)

            ' Update CSS Class
            Select Case udtConfirmationStyle
                Case ConfirmationStyle.Normal
                    lblConfirmOnlyBoxTitle.CssClass = ConfirmationTitleCSSClass
                    tblConfirmBoxContainer.Attributes("class") = ConfirmationNormalCSSClass

                Case ConfirmationStyle.Bordered
                    lblConfirmOnlyBoxTitle.CssClass = ConfirmationTitleCSSClass
                    tblConfirmBoxContainer.Attributes("class") = ConfirmationBorderedCSSClass

                Case ConfirmationStyle.BorderedUnderline
                    lblConfirmOnlyBoxTitle.CssClass = ConfirmationUnderlineCSSClass
                    tblConfirmBoxContainer.Attributes("class") = ConfirmationBorderedCSSClass

                Case ConfirmationStyle.BorderedUnderlineHighlight
                    lblConfirmOnlyBoxTitle.CssClass = ConfirmationUnderlineCSSClass
                    tblConfirmBoxContainer.Attributes("class") = ConfirmationBorderedHighlightCSSClass

                    ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [Start][Winnie]
                Case ConfirmationStyle.Underline
                    lblConfirmOnlyBoxTitle.CssClass = ConfirmationUnderlineCSSClass
                    tblConfirmBoxContainer.Attributes("class") = ConfirmationNormalCSSClass
                    ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [End][Winnie]
            End Select

            ' Save Object to Session
            MyBase.SessionHandler.EHSClaimConfirmMessageSaveToSession(FunctionCode, objDisplayMessage)

            ' SetupConfirmBox is invoked in ActiveChangedEvent and Page_Load of the page
            If Me.mvEHSClaim.ActiveViewIndex <> ActiveViewIndex.ConfirmBox Then
                MyBase.SessionHandler.EHSClaimStepsSaveToSession(FunctionCode, Me.mvEHSClaim.ActiveViewIndex)
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.ConfirmBox
            Else
                Me.SetupConfirmBox()
            End If

        End Sub

        Private Sub ShowVaccineRemarkText()

            Dim tblRemark As Table = New Table
            Dim tr As TableRow
            Dim td As TableCell

            ' Table Property
            tblRemark.CellPadding = 0
            tblRemark.CellSpacing = 0
            tblRemark.CssClass = "textVersionTable"

            Dim udtEHSClaimVaccine As EHSClaimVaccineModel = MyBase.SessionHandler.EHSClaimVaccineGetFromSession()
            If Not udtEHSClaimVaccine Is Nothing AndAlso Not udtEHSClaimVaccine.SubsidizeList Is Nothing Then
                For Each udtEHSClaimSubsidizeItem As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList

                    If tblRemark.Controls.Count > 0 Then
                        ' Add Seperator
                        tr = New TableRow()
                        td = New TableCell()
                        td.Controls.Add(New HtmlControls.HtmlGenericControl("HR"))
                        tr.Controls.Add(td)
                        tblRemark.Controls.Add(tr)
                    End If

                    ' Subsidy Title (Bold)
                    tr = New TableRow()
                    td = New TableCell()
                    td.CssClass = "tableTitle"
                    td.Text = Me.GetGlobalResourceObject("Text", "Vaccine")
                    tr.Controls.Add(td)
                    tblRemark.Controls.Add(tr)

                    ' Subsidize Code (Regular Text)
                    tr = New TableRow()
                    td = New TableCell()
                    td.CssClass = "tableText"
                    td.Text = udtEHSClaimSubsidizeItem.SubsidizeDisplayCode
                    tr.Controls.Add(td)
                    tblRemark.Controls.Add(tr)

                    ' Subsidize Remark (Regular Text)
                    tr = New TableRow()
                    td = New TableCell()
                    td.CssClass = "tableText"
                    td.Text = IIf(SessionHandler.Language() = CultureLanguage.English, FormatSubsidyRemark(udtEHSClaimSubsidizeItem.Remark), FormatSubsidyRemark(udtEHSClaimSubsidizeItem.RemarkChi))
                    tr.Controls.Add(td)
                    tblRemark.Controls.Add(tr)

                    ' Subsidize Admount Header (Bold)
                    tr = New TableRow()
                    td = New TableCell()
                    td.CssClass = "tableTitle"
                    td.Text = Me.GetGlobalResourceObject("Text", "Amount")
                    tr.Controls.Add(td)
                    tblRemark.Controls.Add(tr)

                    ' Subsidize Admount (Regular Text)
                    tr = New TableRow()
                    td = New TableCell()
                    td.CssClass = "tableText"
                    td.Text = String.Format("${0}", Convert.ToInt32(udtEHSClaimSubsidizeItem.Amount))
                    tr.Controls.Add(td)
                    tblRemark.Controls.Add(tr)
                Next
            End If

            ' Put the dynamic control into the remark place holder
            Me.plRemarkContainer.Controls.Add(tblRemark)

            ' Update Session Index if the value is different
            If Me.mvEHSClaim.ActiveViewIndex <> ActiveViewIndex.Remark Then
                MyBase.SessionHandler.EHSClaimStepsSaveToSession(FunctionCode, Me.mvEHSClaim.ActiveViewIndex)
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Remark
            End If

        End Sub

        Private Function FormatSubsidyRemark(ByVal strSubsidizeItemRemark As String) As String
            ' Separator According to "SearchEHSClaimVaccine" in "EHSClaimBLL"
            Dim strRemarks As String() = strSubsidizeItemRemark.Replace("<br>", "|").Split("|")
            Dim strResult As StringBuilder = New StringBuilder()

            For Each strRemark As String In strRemarks
                If strResult.Length > 0 Then
                    strResult.AppendFormat("<br/>")
                End If

                If strRemark.Length > 0 Then
                    If strRemark.StartsWith("(") AndAlso strRemark.EndsWith(")") Then
                        strResult.Append(strRemark)
                    Else
                        strResult.AppendFormat("({0})", strRemark)
                    End If
                End If
            Next

            Return strResult.ToString()

        End Function

        Private Sub ShowInputTips(ByVal type As ucInputTips.InputTipsType)
            MyBase.SessionHandler.EHSClaimStepsSaveToSession(FunctionCode, Me.mvEHSClaim.ActiveViewIndex)
            Me.ucInputTipsControl.LoadTip(type)
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.InputTip

        End Sub

        Private Function GetStepText() As String
            Select Case Me.mvEHSClaim.ActiveViewIndex
                Case ActiveViewIndex.Step1
                    Return Me.GetGlobalResourceObject("Text", "ClaimStep1")

                Case ActiveViewIndex.Step2a, ActiveViewIndex.Step2b, ActiveViewIndex.ReasonForVisit
                    Return Me.GetGlobalResourceObject("Text", "ClaimStep2")

                Case ActiveViewIndex.Step3
                    Return Me.GetGlobalResourceObject("Text", "ClaimStep3")

                Case ActiveViewIndex.ConfirmBox
                    Dim objConfirmMessage As Object = SessionHandler.EHSClaimConfirmMessageGetFromSession(FunctionCode)
                    If Not objConfirmMessage Is Nothing Then
                        ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [Start][Winnie]
                        If objConfirmMessage.Equals("ProvidedInfoTrueClaimSP") OrElse
                            objConfirmMessage.Equals(DuplicateClaimAlertMessage) Then
                            Return Me.GetGlobalResourceObject("Text", "ClaimStep2")
                        End If
                        ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [End][Winnie]

                    End If

            End Select

            Return String.Empty
        End Function

        '-----------------------------------------------------------------------------------------------------------------------------
        'For Print Option 
        '-----------------------------------------------------------------------------------------------------------------------------
        Public Sub ChangePrintFormControl(ByVal strPrintOption As String, ByVal udtSchemeClaim As SchemeClaimModel)
            'Dim strChinese As String = Me.GetGlobalResourceObject("Text", "Chinese")
            'Dim strEnglish As String = Me.GetGlobalResourceObject("Text", "English")

            ' Default Hide Ad-hoc print button
            Me.btnStep2bAdhocPrintConsentForm.Visible = False

            'Chnage Print Option controls (for consent and creation form)
            If strPrintOption = Common.Component.PrintFormOptionValue.PreprintForm Then
                Me.panlblStep2bPrintConsent.Visible = False
                Me.panStep2bPerprintFormNotice.Visible = True

                If udtSchemeClaim.SubsidizeGroupClaimList(0).AdhocPrintAvailable Then
                    Me.btnStep2bAdhocPrintConsentForm.Visible = True
                End If
            Else
                'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                'Me.rbStep2bPrintClaimConsentFormLanguage.Items.FindByValue(PrintOptionValue.Chi).Text = strChinese
                'Me.rbStep2bPrintClaimConsentFormLanguage.Items.FindByValue(PrintOptionValue.Eng).Text = strEnglish
                'CRE13-019-02 Extend HCVS to China [End][Winnie]

                Me.panlblStep2bPrintConsent.Visible = True
                Me.panStep2bPerprintFormNotice.Visible = False

                If strPrintOption = Common.Component.PrintFormOptionValue.PrintConsentOnly Then
                    Me.btnStep2bPrintClaimConsentForm.Text = Me.GetGlobalResourceObject("AlternateText", "PrintConsentOnly")
                Else
                    Me.btnStep2bPrintClaimConsentForm.Text = Me.GetGlobalResourceObject("AlternateText", "PrintStatementAndConsent")
                End If
            End If
        End Sub

        Private Sub HandlePrintOptionChanged()
            ' INT13-0022 - Fix some special handling on HCSP text only version [Start][Koala]
            ' -------------------------------------------------------------------------------------
            If Me.IsPrePrintDocument() Then
                Me.btnStep2bConfirm.Visible = True
            Else
                ' Check the user has printed the document
                Dim udtEHSTransaction As EHSTransactionModel = SessionHandler.EHSTransactionGetFromSession(FunctionCode)
                If udtEHSTransaction Is Nothing OrElse udtEHSTransaction.PrintedConsentForm = False Then
                    Me.btnStep2bConfirm.Visible = False
                ElseIf udtEHSTransaction.PrintedConsentForm = True Then
                    Me.btnStep2bConfirm.Visible = True
                End If
            End If
            ' INT13-0022 - Fix some special handling on HCSP text only version [End][Koala]
        End Sub

        Private Function IsPrePrintDocument() As Boolean

            SessionHandler.CurrentUserGetFromSession(Me._udtSP, Me._udtDataEntryModel)

            Dim strSelectedPrintOption As String = String.Empty
            If Me._udtDataEntryModel Is Nothing Then
                strSelectedPrintOption = _udtSP.PrintOption
            Else
                strSelectedPrintOption = Me._udtDataEntryModel.PrintOption
            End If

            If strSelectedPrintOption = Common.Component.PrintFormOptionValue.PreprintForm Then
                Return True
            Else
                Return False
            End If

        End Function

        Private Sub PrintPrintout(ByVal strPrintOptionValue As String, ByVal isAdHocPrint As Boolean)

            Dim udtEHSTransaction As EHSTransactionModel = SessionHandler.EHSTransactionGetFromSession(FunctionCode)
            Dim strPrintDateTime As String = String.Format("DH_HCV103{0}{1}{2}{3}{4}{5}{6}", Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, Now.Second, Now.Millisecond)
            udtEHSTransaction.PrintedConsentForm = True
            Me.btnStep2bConfirm.Visible = True

            'Set the transaction is printed consent Form
            SessionHandler.EHSTransactionSaveToSession(udtEHSTransaction, FunctionCode)

            'Save the current function code to session (will be removed in the printout form)
            SessionHandler.EHSClaimPrintoutFunctionCodeSaveToSession(FunctionCode)

            'CRE15-003 System-generated Form [Start][Philip Chau]
            Select Case strPrintOptionValue
                Case PrintOptionValue.CondensedChi
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/EHSClaimCondensedForm_CHI_RV.aspx?TID=" + strPrintDateTime + "');", True)
                    EHSClaimBasePage.AuditLogPrintFrom(_udtAuditLogEntry, isAdHocPrint, "EHSClaimCondensedForm_CHI_RV", "CHI", MyBase.SessionHandler.EHSClaimTempTransactionIDGetFromSession())
                Case PrintOptionValue.CondensedEng
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/EHSClaimCondensedForm_RV.aspx?TID=" + strPrintDateTime + "');", True)
                    EHSClaimBasePage.AuditLogPrintFrom(_udtAuditLogEntry, isAdHocPrint, "EHSClaimCondensedForm_RV", "ENG", MyBase.SessionHandler.EHSClaimTempTransactionIDGetFromSession())
                Case PrintOptionValue.FullChi
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/EHSClaimForm_CHI_RV.aspx?TID=" + strPrintDateTime + "');", True)
                    EHSClaimBasePage.AuditLogPrintFrom(_udtAuditLogEntry, isAdHocPrint, "EHSClaimForm_CHI_RV", "CHI", MyBase.SessionHandler.EHSClaimTempTransactionIDGetFromSession())
                Case PrintOptionValue.FullEng
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/EHSClaimForm_RV.aspx?TID=" + strPrintDateTime + "');", True)
                    EHSClaimBasePage.AuditLogPrintFrom(_udtAuditLogEntry, isAdHocPrint, "EHSClaimForm_RV", "ENG", MyBase.SessionHandler.EHSClaimTempTransactionIDGetFromSession())

                    'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                Case PrintOptionValue.CondensedSimpChi
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/EHSClaimCondensedForm_CN_RV.aspx?TID=" + strPrintDateTime + "');", True)
                    EHSClaimBasePage.AuditLogPrintFrom(_udtAuditLogEntry, isAdHocPrint, "EHSClaimCondensedForm_CN_RV", PrintOptionLanguage.SimpChinese, MyBase.SessionHandler.EHSClaimTempTransactionIDGetFromSession())
                Case PrintOptionValue.FullSimpChi
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/EHSClaimForm_CN_RV.aspx?TID=" + strPrintDateTime + "');", True)
                    EHSClaimBasePage.AuditLogPrintFrom(_udtAuditLogEntry, isAdHocPrint, "EHSClaimForm_CN_RV", PrintOptionLanguage.SimpChinese, MyBase.SessionHandler.EHSClaimTempTransactionIDGetFromSession())
                    'CRE13-019-02 Extend HCVS to China [End][Winnie]
            End Select
            'CRE15-003 System-generated Form [End][Philip Chau]
        End Sub

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        Private Sub PrintClaimConsentFormLanguageSetup(ByVal slConsentFormAvailableLang As String())

            Dim strSelectedLang As String = Me.rbStep2bPrintClaimConsentFormLanguage.SelectedValue

            Me.rbStep2bPrintClaimConsentFormLanguage.Visible = False
            Me.rbStep2bPrintClaimConsentFormLanguage.Items.Clear()

            If slConsentFormAvailableLang IsNot Nothing Then
                For Each language As String In slConsentFormAvailableLang
                    Dim item As New ListItem

                    Select Case language
                        Case PrintOptionLanguage.English
                            item.Text = Me.GetGlobalResourceObject("Text", "English")
                        Case PrintOptionLanguage.TradChinese
                            item.Text = Me.GetGlobalResourceObject("Text", "Chinese")
                        Case PrintOptionLanguage.SimpChinese
                            item.Text = Me.GetGlobalResourceObject("Text", "SimpChinese")
                    End Select

                    item.Value = language

                    If item.Value = strSelectedLang Then
                        item.Selected = True
                    End If

                    Me.rbStep2bPrintClaimConsentFormLanguage.Items.Add(item)
                Next

                'If only 1 language is available, not display
                If Me.rbStep2bPrintClaimConsentFormLanguage.Items.Count > 1 Then
                    Me.rbStep2bPrintClaimConsentFormLanguage.Visible = True
                End If

                'Default the first item
                If Me.rbStep2bPrintClaimConsentFormLanguage.SelectedIndex = -1 Then
                    Me.rbStep2bPrintClaimConsentFormLanguage.SelectedIndex = 0
                End If

            End If
        End Sub
        'CRE13-019-02 Extend HCVS to China [End][Winnie]

        ' Handle Page Refresh
        Private Sub HandlePageRefreshed()

            ' Remove all claim related session
            Me.Clear()

            Dim objSessionPractice As Object = SessionHandler.PracticeDisplayGetFromSession(FunctionCode)
            Dim objSessionScheme As Object = SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
            Dim objSessionDocumentType As Object = SessionHandler.DocumentTypeSelectedGetFromSession(FunctionCode)

            If Not objSessionPractice Is Nothing AndAlso _
               Not objSessionScheme Is Nothing AndAlso _
               Not objSessionDocumentType Is Nothing Then
                ' Have Practice+Scheme+DocType
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
            ElseIf Not objSessionPractice Is Nothing AndAlso _
                   Not objSessionScheme Is Nothing Then
                ' Have Practice+Scheme
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SelectDocType
            ElseIf Not Not objSessionPractice Is Nothing Then
                ' Have Practice
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SelectScheme
            Else
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SelectPractice
            End If

        End Sub

        Private Sub GoToVersion2()

            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

            RedirectHandler.ToURL(EHSClaimV2)

            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

        End Sub

        '==================================================================== Code for SmartID ============================================================================
        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Private Sub RedirectToIdeas(ByVal enumIDEASVersion As IdeasBLL.EnumIdeasVersion)

            Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
            Dim eIdeasVersion As IdeasBLL.EnumIdeasVersion = IdeasBLL.GetIdeasVersion(enumIDEASVersion)
            Dim strIdeasVersion As String = IdeasBLL.ConvertIdeasVersion(eIdeasVersion)

            EHSClaimBasePage.AuditLogReadSamrtID(_udtAuditLogEntry, udtSchemeClaim.SchemeCode, strIdeasVersion, Nothing)

            ' (1) Language
            Dim strLang As String = String.Empty

            If LCase(Session("language")) = CultureLanguage.TradChinese Then
                strLang = "zh_HK"
            Else
                strLang = "en_US"
            End If

            ' (2) Remove Card Setting
            Dim strRemoveCard As String = String.Empty

            Dim udtGeneralFunction As New GeneralFunction
            udtGeneralFunction.getSystemParameter("SmartID_RemoveCard", strRemoveCard, String.Empty)
            If strRemoveCard = String.Empty Then
                strRemoveCard = "Y"
            End If

            ' (3) IDEAS token
            Dim ideasHelper As IHelper = HelpFactory.createHelper()
            Dim ideasTokenResponse As TokenResponse = Nothing

            ' Enforce HCSP accept server cert for connecting IDEAS Testing server
            ServicePointManager.ServerCertificateValidationCallback = New Security.RemoteCertificateValidationCallback(AddressOf (New BLL.IdeasBLL).ValidateCertificate)

            ' Get Token From Ideas, input: the return URL from Ideas to eHS
            ideasTokenResponse = IdeasBLL.GetToken(eIdeasVersion, Me.Page.Request.Url.GetLeftPart(UriPartial.Path), strLang, strRemoveCard)

            Dim isDemoVersion As String = ConfigurationManager.AppSettings("SmartIDDemoVersion")

            If Not ideasTokenResponse.ErrorCode Is Nothing And Not isDemoVersion.Equals("Y") Then
                Me.udcMsgBoxErr.AddMessageDesc(FunctionCode, ideasTokenResponse.ErrorCode, ideasTokenResponse.ErrorMessage)

                EHSClaimBasePage.AuditLogConnectIdeasFail(_udtAuditLogEntry, udtSchemeClaim.SchemeCode, ideasTokenResponse, "N", strIdeasVersion)

                Me.udcMsgBoxErr.BuildMessageDescBox("SmartIDActionFail", _udtAuditLogEntry, Common.Component.LogID.LOG00049, "Click 'Read and Search Card' and Token Request Fail")

            Else
                Dim udtSessionHandler As New SessionHandler
                Dim udtSmarIDContent As New SmartIDContentModel
                udtSmarIDContent.IsReadSmartID = True
                udtSmarIDContent.TokenResponse = ideasTokenResponse
                udtSmarIDContent.IdeasVersion = eIdeasVersion

                If isDemoVersion.Equals("Y") Then
                    udtSmarIDContent.IsDemonVersion = True

                    MyBase.SessionHandler.SmartIDContentSaveToSession(FunctionCode, udtSmarIDContent)

                    EHSClaimBasePage.AuditLogConnectIdeasComplete(_udtAuditLogEntry, udtSchemeClaim.SchemeCode, ideasTokenResponse, "Y", strIdeasVersion)

                    RedirectHandler.ToURL(ConfigurationManager.AppSettings("SmartIDTestRedirectPageTextOnlyVersion").ToString().Replace("@", "&"))

                Else
                    udtSmarIDContent.IsDemonVersion = False

                    MyBase.SessionHandler.SmartIDContentSaveToSession(FunctionCode, udtSmarIDContent)

                    EHSClaimBasePage.AuditLogConnectIdeasComplete(_udtAuditLogEntry, udtSchemeClaim.SchemeCode, ideasTokenResponse, "N", strIdeasVersion)

                    ' Redirect to Ideas, no need to add page key
                    Response.Redirect(ideasTokenResponse.IdeasMAURL)

                End If
            End If
        End Sub
        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Private Sub RedirectToIdeasCombo(ByVal enumIDEASVersion As IdeasBLL.EnumIdeasVersion)

            Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
            Dim eIdeasVersion As IdeasBLL.EnumIdeasVersion = IdeasBLL.GetIdeasVersion(enumIDEASVersion)
            Dim strIdeasVersion As String = IdeasBLL.ConvertIdeasVersion(eIdeasVersion)

            EHSClaimBasePage.AuditLogReadSamrtID(_udtAuditLogEntry, udtSchemeClaim.SchemeCode, strIdeasVersion, Nothing)

            ' (1) Language
            Dim strLang As String = String.Empty

            If LCase(Session("language")) = CultureLanguage.TradChinese Then
                strLang = "zh_HK"
            Else
                strLang = "en_US"
            End If

            ' (2) Remove Card Setting
            Dim strRemoveCard As String = String.Empty

            Dim udtGeneralFunction As New GeneralFunction
            udtGeneralFunction.getSystemParameter("SmartID_RemoveCard", strRemoveCard, String.Empty)
            If strRemoveCard = String.Empty Then
                strRemoveCard = "Y"
            End If

            ' (3) IDEAS token
            Dim ideasHelper As IHelper = HelpFactory.createHelper()
            Dim ideasTokenResponse As TokenResponse = Nothing

            ' Enforce HCSP accept server cert for connecting IDEAS Testing server
            ServicePointManager.ServerCertificateValidationCallback = New Security.RemoteCertificateValidationCallback(AddressOf (New IdeasBLL).ValidateCertificate)

            ' Get Token From Ideas, input: the return URL from Ideas to eHS
            Select Case eIdeasVersion
                Case IdeasBLL.EnumIdeasVersion.One, IdeasBLL.EnumIdeasVersion.Two, IdeasBLL.EnumIdeasVersion.TwoGender
                    ideasTokenResponse = IdeasBLL.GetToken(eIdeasVersion, Me.Page.Request.Url.GetLeftPart(UriPartial.Path), strLang, strRemoveCard)

                Case IdeasBLL.EnumIdeasVersion.Combo, IdeasBLL.EnumIdeasVersion.ComboGender
                    Dim strPageName As String = New IO.FileInfo(Me.Request.Url.LocalPath).Name
                    Dim strComboReturnURL As String = Me.Page.Request.Url.GetLeftPart(UriPartial.Path)
                    Dim strFolderName As String = "/text"

                    strComboReturnURL = strComboReturnURL.Replace(strFolderName + "/" + strPageName, "/IDEASComboReader/IDEASComboReader.aspx")
                    ideasTokenResponse = IdeasBLL.GetToken(eIdeasVersion, strComboReturnURL, strLang, strRemoveCard)

            End Select

            Dim isDemoVersion As String = ConfigurationManager.AppSettings("SmartIDDemoVersion")

            If Not ideasTokenResponse.ErrorCode Is Nothing And Not isDemoVersion.Equals("Y") Then
                udcMsgBoxErr.AddMessageDesc(FunctionCode, ideasTokenResponse.ErrorCode, ideasTokenResponse.ErrorMessage)

                EHSClaimBasePage.AuditLogConnectIdeasFail(_udtAuditLogEntry, udtSchemeClaim.SchemeCode, ideasTokenResponse, "N", strIdeasVersion)

                Me.udcMsgBoxErr.BuildMessageDescBox("SmartIDActionFail", _udtAuditLogEntry, Common.Component.LogID.LOG00050, "Click 'Read Smart ID Card' and Token Request Fail")

            Else
                Dim udtSessionHandler As New SessionHandler
                Dim udtSmarIDContent As New SmartIDContentModel
                udtSmarIDContent.IsReadSmartID = True
                udtSmarIDContent.TokenResponse = ideasTokenResponse
                udtSmarIDContent.IdeasVersion = eIdeasVersion

                If isDemoVersion.Equals("Y") Then
                    udtSmarIDContent.IsDemonVersion = True

                    MyBase.SessionHandler.SmartIDContentSaveToSession(FunctionCode, udtSmarIDContent)

                    EHSClaimBasePage.AuditLogConnectIdeasComplete(_udtAuditLogEntry, udtSchemeClaim.SchemeCode, ideasTokenResponse, "Y", strIdeasVersion)

                    RedirectHandler.ToURL(ConfigurationManager.AppSettings("SmartIDTestRedirectPageTextOnlyVersion").ToString().Replace("@", "&"))

                Else
                    udtSmarIDContent.IsDemonVersion = False

                    MyBase.SessionHandler.SmartIDContentSaveToSession(FunctionCode, udtSmarIDContent)

                    EHSClaimBasePage.AuditLogConnectIdeasComplete(_udtAuditLogEntry, udtSchemeClaim.SchemeCode, ideasTokenResponse, "N", strIdeasVersion)

                    ' Prompt the popup include iframe to show IDEAS Combo UI
                    ucIDEASCombo.ReadSmartIC(IdeasBLL.EnumIdeasVersion.Combo, ideasTokenResponse, FunctionCode)

                End If
            End If
        End Sub
        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

        Private Function SmartIDShowRealID() As Boolean
            Dim udtGeneralFunction As New GeneralFunction
            Dim strParmValue As String = String.Empty
            udtGeneralFunction.getSystemParameter("SmartIDShowRealID", strParmValue, String.Empty)
            Return strParmValue.Trim = "Y"
        End Function
        '==================================================================================================================================================================

#End Region

        Public Function GetVaccinationRecord(ByVal udtEHSAccount As EHSAccountModel, Optional ByVal strSchemeCode As String = "") As TransactionDetailVaccineModelCollection

            Dim udtTranDetailVaccineList As TransactionDetailVaccineModelCollection = Nothing

            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Dim udtVaccineResultBag As New VaccineResultCollection
            udtVaccineResultBag = _udtEHSClaimBLL.GetVaccinationRecord(udtEHSAccount, udtTranDetailVaccineList, _
                                                             FunctionCode, _udtAuditLogEntry, _
                                                             strSchemeCode)

            If udtVaccineResultBag.HAReturnStatus = VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail Or _
                udtVaccineResultBag.DHReturnStatus = VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail Then
                ' if fail to enquiry latest record, then use previous cached record
                Return GetVaccinationRecordFromSession(udtEHSAccount, strSchemeCode)
            End If
            ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

            Return udtTranDetailVaccineList
        End Function

        Public Function GetVaccinationRecordFromSession(ByVal udtEHSAccount As EHSAccountModel, Optional ByVal strSchemeCode As String = "") As TransactionDetailVaccineModelCollection
            Dim udtVaccinationBLL As New VaccinationBLL
            Dim udtEHSTransactionBLL As New EHSTransactionBLL
            Dim udtSession As New BLL.SessionHandler

            Dim htRecordSummary As Hashtable = Nothing
            Dim udtTranDetailVaccineList As TransactionDetailVaccineModelCollection = Nothing
            Dim udtHAVaccineResult As Common.WebService.Interface.HAVaccineResult = Nothing
            Dim udtHAVaccineResultSession As Common.WebService.Interface.HAVaccineResult = udtSession.CMSVaccineResultGetFromSession(FunctionCode)
            Dim udtDHVaccineResult As Common.WebService.Interface.DHVaccineResult = Nothing
            Dim udtDHVaccineResultSession As Common.WebService.Interface.DHVaccineResult = udtSession.CIMSVaccineResultGetFromSession(FunctionCode)

            Dim udtVaccineResultBag As New VaccineResultCollection
            udtVaccineResultBag.DHVaccineResult = udtDHVaccineResult
            udtVaccineResultBag.HAVaccineResult = udtHAVaccineResult

            Dim udtVaccineResultBagSession As New VaccineResultCollection
            udtVaccineResultBagSession.DHVaccineResult = udtDHVaccineResultSession
            udtVaccineResultBagSession.HAVaccineResult = udtHAVaccineResultSession

            udtVaccinationBLL.GetVaccinationRecord(udtEHSAccount, udtTranDetailVaccineList, udtVaccineResultBag, htRecordSummary, _udtAuditLogEntry, strSchemeCode, udtVaccineResultBagSession)

            udtSession.CMSVaccineResultSaveToSession(udtVaccineResultBag.HAVaccineResult, FunctionCode)
            udtSession.CIMSVaccineResultSaveToSession(udtVaccineResultBag.DHVaccineResult, FunctionCode)

            udtTranDetailVaccineList.Sort(TransactionDetailVaccineModelCollection.enumSortBy.ServiceDate, SortDirection.Descending)

            Return udtTranDetailVaccineList
        End Function

#Region "Implement IWorkingData (CRE11-004)"

        Public Overrides Function GetDocCode() As String
            If GetEHSAccount() Is Nothing Then Return Nothing
            If GetEHSAccount.SearchDocCode = String.Empty Then Return Nothing
            Return GetEHSAccount.SearchDocCode
        End Function

        Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
            Return MyBase.SessionHandler.EHSAccountGetFromSession(FunctionCode)
        End Function

        Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
            Return MyBase.SessionHandler.EHSTransactionGetFromSession(FunctionCode)
        End Function

        Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
            Dim udtSP As ServiceProviderModel = Nothing
            Dim udtDataEntry As DataEntryUserModel = Nothing

            GetCurrentUserAccount(udtSP, udtDataEntry, False)

            Return udtSP
        End Function

#End Region

        Private Sub txtStep2aServiceDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStep2aServiceDate.TextChanged
            '------------------------------------------------ Check Service Date ---------------------------------------------------
            Dim udtSystemMessage As SystemMessage
            Dim udtValidator As Validator = New Validator()
            Dim strServiceDate As String = String.Empty

            udcMsgBoxErr.Clear()
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim udtSubPlatformBLL As New SubPlatformBLL
            'strServiceDate = _udtFormatter.formatInputDate(Me.txtStep2aServiceDate.Text)
            strServiceDate = _udtFormatter.formatInputDate(Me.txtStep2aServiceDate.Text, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]



            udtSystemMessage = udtValidator.chkServiceDate(strServiceDate)

            If Not udtSystemMessage Is Nothing Then
                ' Error
                Me.lblStep2aServiceDateError.Visible = True
                Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
                ' Error override information message

                ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Koala]
                ' -----------------------------------------------------------------------------------------
                Me.udcMsgBoxInfo.Clear()
                ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Koala]
            Else
                Me.lblStep2aServiceDateError.Visible = False
                Me.txtStep2aServiceDate.Text = strServiceDate
            End If

            Dim errorMessageCodeTable As DataTable = Me.udcMsgBoxErr.GetCodeTable
            If errorMessageCodeTable.Rows.Count > 0 Then
                Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail, _udtAuditLogEntry, Common.Component.LogID.LOG00015, String.Format("Enter Claim Detail Failed", FunctionCode))
            End If

            If Me.lblStep2aServiceDateError.Visible Then Exit Sub

            ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [Start][Winnie]
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
            Dim strConvertedServiceDate As String = _udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English)

            Dim udcInputHCVS As UIControl.EHCClaimText.ucInputHCVS = Me.udcStep2aInputEHSClaim.GetHCVSControl()

            If Not udtGeneralFunction.IsCoPaymentFeeEnabled(strConvertedServiceDate) Then
                CType(udcInputHCVS.FindControl("txtCoPaymentFee"), TextBox).Text = String.Empty
                Me.SessionHandler.EHSTransactionGetFromSession(FunctionCode).TransactionAdditionFields.RemoveReasonForVisit(3)
                Me.SessionHandler.EHSTransactionGetFromSession(FunctionCode).TransactionAdditionFields.RemoveReasonForVisit(2)
                Me.SessionHandler.EHSTransactionGetFromSession(FunctionCode).TransactionAdditionFields.RemoveReasonForVisit(1)
            End If

            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ' Clear DHC-related Services
            If udtGeneralFunction.IsDHCServiceEffective(strConvertedServiceDate) = False Then
                CType(udcInputHCVS.FindControl("chkDHCRelatedService"), CheckBox).Checked = False
                Me.SessionHandler.EHSTransactionGetFromSession(FunctionCode).DHCService = String.Empty
            End If
            ' CRE19-006 (DHC) [End][Winnie]

            ' Clear AvailableVoucher to trigger recalculation
            Dim udtEHSAccount As EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(FunctionCode)
            udtEHSAccount.VoucherInfo = Nothing

            ' CRE19-003 (Opt voucher capping) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            'Step2aBuildInputEHSClaimControl()
            Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
            SetupStep2aClaimContent(udtSchemeClaim, udtEHSAccount, False)
            ' CRE19-003 (Opt voucher capping) [End][Winnie]

            ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [End][Winnie]

        End Sub

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Function CheckHKIDByOCSSS(ByVal strIdentityNo As String, ByVal strSPID As String, ByVal strSchemeCode As String) As SystemMessage
            Dim udtOCSSSResult As OCSSSResult = Nothing
            'Call OCSSS to check HKIC
            Try
                udtOCSSSResult = (New OCSSSServiceBLL).IsEligible(strIdentityNo, SessionHandler.HKICSymbolGetFormSession(FunctionCode), strSPID, strSchemeCode)
            Catch ex As Exception
                Throw
            End Try

            SessionHandler.OCSSSRefStatusSaveToSession(FunctionCode, udtOCSSSResult.OCSSSStatus)

            Dim udtSystemMessage As SystemMessage = Nothing

            'Validation
            If udtOCSSSResult.ConnectionStatus = OCSSSResult.OCSSSConnection.Success Then
                'Arise warning if HKIC no. is invalid
                If udtOCSSSResult.EligibleResult = OCSSSResult.OCSSSEligibleResult.Invalid Then
                    udtSystemMessage = New SystemMessage("990000", "E", "00420")
                End If
            End If

            Return udtSystemMessage

        End Function
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Function CheckValidHKICInScheme(ByVal strSchemeCode) As SystemMessage
            Dim udtEHSAccount As EHSAccountModel = SessionHandler.EHSAccountGetFromSession(FunctionCode)
            Dim udtSystemMessage As SystemMessage = Nothing

            If Not udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode) Is Nothing AndAlso _
                udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode).DocCode = DocTypeModel.DocTypeCode.HKIC Then

                If Common.OCSSS.OCSSSServiceBLL.EnableHKICSymbolInput(strSchemeCode) And _
                    SessionHandler.HKICSymbolGetFormSession(FunctionCode) Is Nothing Then

                    udtSystemMessage = New SystemMessage("990000", "E", "00422")
                End If
            End If

            Return udtSystemMessage

        End Function
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        ' INT20-0021 (Add auditlog for click UpdateNow & Fix GetEHSVaccine web service ) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Private Sub lbtnUpdateNow_Click(sender As Object, e As EventArgs) Handles lbtnUpdateNow.Click

            ScriptManager.RegisterStartupScript(Me, Page.GetType, "UpdateNow", String.Format("javascript:showUpdateNow('{0}');", Session("language")), True)

            _udtAuditLogEntry.WriteLog(LogID.LOG00092, "Click Update Now for software of reading Smart ID card")

        End Sub
        ' INT20-0021 (Add auditlog for click UpdateNow & Fix GetEHSVaccine web service ) [End][Chris YIM]	

        ' INT20-0021 (Add auditlog for click UpdateNow & Fix GetEHSVaccine web service ) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Private Sub lbtnHere_Click(sender As Object, e As EventArgs) Handles lbtnSmartIDSoftwareNotInstalled2.Click

            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Here", String.Format("javascript:showUpdateNow('{0}');", Session("language")), True)

            _udtAuditLogEntry.WriteLog(LogID.LOG00093, "Click HERE for software of reading Smart ID card")

        End Sub
        ' INT20-0021 (Add auditlog for click UpdateNow & Fix GetEHSVaccine web service ) [End][Chris YIM]	

    End Class



End Namespace
