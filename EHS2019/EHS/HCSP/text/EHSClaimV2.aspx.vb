Imports Common
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
Imports Common.Component.InputPicker
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.Scheme
Imports Common.Component.ServiceProvider
Imports Common.Component.StaticData
Imports Common.Component.UserAC
Imports Common.Component.VoucherScheme
Imports Common.Format
Imports Common.Validation
Imports HCSP.BLL
Imports System.Web.Security.AntiXss
Imports Common.WebService.Interface


Partial Public Class EHSClaimV2
    Inherits EHSClaimBasePageV2

    ' Public Member
    Public Const ValidatedServiceDate As String = "ValidatedServiceDate"
    Public Const EHSClaim As String = "EHSClaimV1.aspx"

    ' Private Member
    Private Const ConfirmationNormalCSSClass = "tableText"
    Private Const ConfirmationTitleCSSClass = "tableTitle"
    Private Const ConfirmationBorderedCSSClass = "tableRemark"
    Private Const ConfirmationBorderedHighlightCSSClass = "tableRemarkHighlight"
    Private Const ConfirmationUnderlineCSSClass = "tableHeader"
    Private Const SelectedPerConditionValueAttribute As String = "SelectedPerCondition"

    Private _udtClaimVoucherBLL As BLL.ClaimVoucherBLL = New BLL.ClaimVoucherBLL()
    Private _udtEHSClaimBLL As BLL.EHSClaimBLL = New BLL.EHSClaimBLL()
    Private _udtEHSAccountBll As EHSAccountBLL = New EHSAccountBLL()
    Private _udtEHSTransactionBLL As EHSTransactionBLL = New EHSTransactionBLL()
    Private _udtPracticeAcctBLL As BLL.PracticeBankAcctBLL = New BLL.PracticeBankAcctBLL()
    Private _udtPracticeBankAccBLL As New BLL.PracticeBankAcctBLL()
    Private _udtStaticDataBLL As New Common.Component.StaticData.StaticDataBLL()
    Private _udtClaimCategoryBLL As New ClaimCategoryBLL()
    Private _udtSchemeClaimBLL As New SchemeClaimBLL()

    Private _udtSP As ServiceProviderModel = Nothing
    Private _udtDataEntryModel As DataEntryUserModel = Nothing
    Private _udtVoucherScheme As VoucherSchemeModel = New VoucherSchemeModel()
    Private _udtUserAC As UserACModel = New UserACModel()

    Private _udtEHSTransaction As EHSTransactionModel
    Private _udtCommfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction()
    Private _strValidationFail As String = "ValidationFail"
    Private _udtFormatter As Common.Format.Formatter = New Common.Format.Formatter()
    Private _udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

    Private _blnConcurrentUpdate As Boolean = False

#Region "Private Class"

    Private Class VaccinationRecordPopupStatusClass
        Public Const Active As String = "A"
        Public Const Closed As String = "C"
    End Class

    Private Class VS
        Public Const VaccinationRecordPopupStatus As String = "VaccinationRecordPopupStatus"
    End Class

    Private Class SESS
        Public Const ViewBeforeVaccinationRecord As String = "EHSClaimV2_ViewBeforeVaccinationRecord"
    End Class

    'Active View Index
    Private Class ActiveViewIndex
        Public Const ViewTranDetail As Integer = 0
        Public Const ConfirmDetail As Integer = 1
        Public Const CompleteClaim As Integer = 2
        Public Const ServiceDate As Integer = 3
        Public Const Category As Integer = 4
        Public Const MedicalCondition As Integer = 5
        Public Const RCHCode As Integer = 6
        Public Const Vaccine As Integer = 7
        Public Const InternalError As Integer = 8
        Public Const ConfirmBox As Integer = 9
        Public Const PrintOption As Integer = 10
        Public Const AddHocPrint As Integer = 11
        Public Const Remark As Integer = 12
        Public Const SelectPractice As Integer = 13
        Public Const SelectScheme As Integer = 14
        Public Const VaccinationRecord As Integer = 15
        'CRE15-005-03 (New PIDVSS scheme) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Const DocumentaryProof As Integer = 16
        'CRE15-005-03 (New PIDVSS scheme) [End][Chris YIM]
        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Const PlaceVaccination As Integer = 17
        'CRE16-002 (Revamp VSS) [End][Chris YIM]
        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Const RecipientConditionHelp As Integer = 18
        Public Const SubsidizeDisabledDetail As Integer = 19
        'CRE16-026 (Add PCV13) [End][Chris YIM]

    End Class

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Class ClinicType
        Public Const Clinic As String = "C"
        Public Const NonClinic As String = "N"
    End Class

    Private Class PlaceOfVaccinationOptions
        Public Const OTHERS As String = "OTHERS"
    End Class

    Public Class PID_DOCUMENTARYPROOF
        Public Const PID_INSTITUTION_CERT As String = "PID_I_CERT"
    End Class

    Public Class RCH_TYPE
        Public Const ALL As String = ""
        Public Const PID As String = "I"
    End Class
    'CRE16-002 (Revamp VSS) [End][Chris YIM]

    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Class HighRiskOption
        Public Const HIGHRISK As String = "HIGHRISK"
        Public Const NOHIGHRISK As String = "NOHIGHRISK"
    End Class
    'CRE16-026 (Add PCV13) [End][Chris YIM]
#End Region

    ' Confirmation Display Style
    Protected Enum ConfirmationStyle
        NotSet
        Normal
        Bordered
        BorderedUnderline
        BorderedUnderlineHighlight
    End Enum


    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        ' Concurrent Browser Checking
        If Not Page.IsPostBack Then
            ' get the token from session / create a new one
            Dim strCurrentToken As String = SessionHandler.EHSClaimTokenNumberGetFromSession(FunctionCode)
            If String.IsNullOrEmpty(strCurrentToken) Then
                ' Assign a new token
                Me.EHSClaimTokenNumAssign()
            Else
                ' From EHSClaimV1, Get the Token from there
                BrowserTokenHiddenField.Value = strCurrentToken
            End If
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        ' Page Setup
        CType(Me.Master.FindControl(ClaimVoucherMaster.ControlName.LblClaimVoucherStep), Label).Text = Me.GetStepText()

        'Get Current USer Account for check Session Expired
        Me._udtUserAC = UserACBLL.GetUserAC

        If Not Me.IsPostBack Then
            'Get Current User Account
            MyBase.GetCurrentUserAccount(Me._udtSP, Me._udtDataEntryModel, True)

            'Step 1 of Page Load : if no Selected Practice in session, go to Practice selection Page
            Dim udtSelectedPracticeDisplay As BLL.PracticeDisplayModel = SessionHandler.PracticeDisplayGetFromSession(Me.FunctionCode)

            If MyBase.SessionHandler.AccountCreationProceedClaimGetFromSession() Then

                'EHSClaimBasePage.AuditLogPageLoad(FunctCode, True, True)
                EHSClaimBasePage.AuditLogEnterClaimDetailLoaded(_udtAuditLogEntry)

                'Me.EHSClaimTokenNumAssign()

                'Step 5 of Page Load : if is come from Account creation and user pressed "proceed to claim" -> got to Enter Claim Detail
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.ServiceDate

                ' Remove account creation's session
                SessionHandler.AccountCreationProceedClaimRemoveFromSession()
            Else
                'EHSClaimBasePage.AuditLogPageLoad(Me.FunctionCode, True, False)
                ' Validated Account Redirected from Version1
                EHSClaimBasePage.AuditLogEnterClaimDetailLoaded(_udtAuditLogEntry)

                '' If All Practice/ Scheme/ Doc Type is presented, go to Step1
                'Dim udtSelectedPractice = SessionHandler.PracticeDisplayGetFromSession(Me.FunctionCode)
                'Dim udtSelectedScheme = SessionHandler.SchemeSelectedGetFromSession(Me.FunctionCode)
                'Dim udtSelectedDocType = SessionHandler.DocumentTypeSelectedGetFromSession(Me.FunctionCode)

                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.ServiceDate

                'If udtSelectedPractice Is Nothing Then
                '    Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SelectPractice
                'ElseIf udtSelectedScheme Is Nothing Then
                '    Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SelectScheme
                'ElseIf udtSelectedDocType Is Nothing Then
                '    Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SelectDocType
                'Else
                '    Me.Step1Clear()
                '    Me.ClearEHSClaimState()
                '    Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
                'End If

            End If
        Else
            ' Handle Page Post Back 
            Select Case Me.mvEHSClaim.ActiveViewIndex

                Case ActiveViewIndex.CompleteClaim
                    Me.SetupCompleteClaim(SessionHandler.EHSAccountGetFromSession(FunctionCode), False)

                Case ActiveViewIndex.ConfirmDetail
                    Me.SetupStepConfirmClaim(SessionHandler.EHSAccountGetFromSession(FunctionCode), False)

                Case ActiveViewIndex.Remark
                    ' Build Dynamic Control
                    Me.ShowVaccineRemarkText()

                Case ActiveViewIndex.ViewTranDetail
                    ' Build Dynamic Control
                    Me.ShowTransactionDetail()

                Case ActiveViewIndex.ConfirmBox
                    ' Build Dynamic Control
                    Me.ShowConfirmation()

                Case ActiveViewIndex.ServiceDate
                    Me.SetupEnterClaimDetail()
                    Me.SetupServiceDate(False)

                Case ActiveViewIndex.Category
                    Me.SetupEnterClaimDetail()
                    Me.SetupCategory(False)

                Case ActiveViewIndex.MedicalCondition
                    Me.SetupEnterClaimDetail()
                    Me.SetupMedicalCondition(False)

                Case ActiveViewIndex.RCHCode
                    Me.SetupEnterClaimDetail()
                    Me.SetupRCHCode(False)

                Case ActiveViewIndex.Vaccine
                    Me.SetupEnterClaimDetail()
                    'CRE15-003 System-generated Form [Start][Philip Chau]
                    MyBase.SessionHandler.EHSClaimTempTransactionIDRemoveFromSession()
                    MyBase.SessionHandler.EHSClaimStep3ShowLastestTransactionIDSaveToSession(False)
                    'CRE15-003 System-generated Form [End][Philip Chau]
                    'Me.SetupVaccine(True)

                Case ActiveViewIndex.SelectPractice
                    Me.SetupSelectPractice()

                Case ActiveViewIndex.SelectScheme
                    Me.SetupSelectScheme(False)

                Case ActiveViewIndex.InternalError
                    Me.SetupInternalError()

                Case ActiveViewIndex.VaccinationRecord
                    Me.SetupEnterClaimDetail()
                    Me.SetupVaccinationRecord()

                    'CRE15-005-03 (New PIDVSS scheme) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                Case ActiveViewIndex.DocumentaryProof
                    Me.SetupEnterClaimDetail()
                    Me.SetupDocumentaryProof(False)
                    'CRE15-005-03 (New PIDVSS scheme) [End][Chris YIM]
                    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                Case ActiveViewIndex.PlaceVaccination
                    Me.SetupEnterClaimDetail()
                    Me.SetupPlaceVaccination(False)
                    'CRE16-002 (Revamp VSS) [End][Chris YIM]

                    'CRE16-026 (Add PCV13) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                Case ActiveViewIndex.RecipientConditionHelp
                    Me.ShowRecipientConditionHelpText()

                Case ActiveViewIndex.SubsidizeDisabledDetail
                    Me.ShowSubsidizeDisabledRemarkText(SessionHandler.SubsidizeDisabledDetailKeyGetFromSession(FunctionCode))
                    'CRE16-026 (Add PCV13) [End][Chris YIM]
            End Select

            ' Build Dynamic Control
            Dim udtClaimVaccine As EHSClaimVaccineModel = SessionHandler.EHSClaimVaccineGetFromSession()
            If Not udtClaimVaccine Is Nothing Then
                Me.SetupVaccine(True)
            End If

        End If

    End Sub

    Private Sub mvEHSClaim_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mvEHSClaim.ActiveViewChanged

        ' Build Menu Item
        Me.BuildMenuItem()

        ' Hide All Error/Info Message when View Changed
        Me.ClearMessageBox()

        CType(Me.Master.FindControl(ClaimVoucherMaster.ControlName.LblClaimVoucherStep), Label).Text = Me.GetStepText()

        Select Case mvEHSClaim.ActiveViewIndex

            Case ActiveViewIndex.ConfirmDetail
                Me.SetupStepConfirmClaim(SessionHandler.EHSAccountGetFromSession(FunctionCode), True)

            Case ActiveViewIndex.CompleteClaim
                Me.CategoryClear()
                Me.ServiceDateClean(False)
                Me.MedicalConditionClear()
                Me.VaccineClear()
                Me.SetupCompleteClaim(SessionHandler.EHSAccountGetFromSession(FunctionCode), True)

            Case ActiveViewIndex.ServiceDate
                Me.SetupEnterClaimDetail()
                Me.SetupServiceDate(True)

            Case ActiveViewIndex.Category
                Me.SetupEnterClaimDetail()
                Me.SetupCategory(True)

            Case ActiveViewIndex.MedicalCondition
                Me.SetupEnterClaimDetail()
                Me.SetupMedicalCondition(True)

            Case ActiveViewIndex.RCHCode
                Me.SetupEnterClaimDetail()
                Me.SetupRCHCode(True)

            Case ActiveViewIndex.Vaccine
                Me.ClearWarningRules(MyBase.SessionHandler.EligibleResultGetFromSession())
                Me.SetupEnterClaimDetail()
                Me.SetupVaccine(True)
                'CRE15-003 System-generated Form [Start][Philip Chau]
                MyBase.SessionHandler.EHSClaimTempTransactionIDRemoveFromSession()
                MyBase.SessionHandler.EHSClaimStep3ShowLastestTransactionIDSaveToSession(False)
                'CRE15-003 System-generated Form [End][Philip Chau]

            Case ActiveViewIndex.InternalError
                Me.SetupInternalError()

                'Case ActiveViewIndex.ViewTranDetail
                'Case ActiveViewIndex.ConfirmBox
                'Case ActiveViewIndex.Remark

            Case ActiveViewIndex.PrintOption

            Case ActiveViewIndex.AddHocPrint

            Case ActiveViewIndex.SelectPractice
                Me.SetupSelectPractice()

            Case ActiveViewIndex.SelectScheme
                Me.SetupSelectScheme(True)

            Case ActiveViewIndex.VaccinationRecord

                'CRE15-005-03 (New PIDVSS scheme) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
            Case ActiveViewIndex.DocumentaryProof
                Me.SetupEnterClaimDetail()
                Me.SetupDocumentaryProof(True)
                'CRE15-005-03 (New PIDVSS scheme) [End][Chris YIM]
                'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
            Case ActiveViewIndex.PlaceVaccination
                Me.SetupEnterClaimDetail()
                Me.SetupPlaceVaccination(True)
                'CRE16-002 (Revamp VSS) [End][Chris YIM]
        End Select

    End Sub

#Region "Enter Service Date"

    Protected Sub btnServiceDateCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnServiceDateCancel.Click
        EHSClaimBasePage.AuditLogTextOnlyVersionEnterServiceDateCancelClick(_udtAuditLogEntry)

        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearServiceDateError()

        CancelAction()
    End Sub

    Protected Sub btnServiceDateNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnServiceDateNext.Click
        EHSClaimBasePage.AuditLogTextOnlyVersionEnterServiceDateNextClick(_udtAuditLogEntry)

        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        'BLLs
        Dim udtValidator As Validator = New Validator

        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(Me.FunctionCode)
        Dim udtEHSAccount As EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(Me.FunctionCode)
        Dim udtPersonalInformation As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode)
        Dim udtSubPlatformBLL As New SubPlatformBLL
        Dim strServiceDate As String = Me._udtFormatter.formatInputDate(Me.txtServiceDate.Text, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtSchemeClaimBLL As SchemeClaimBLL = New SchemeClaimBLL()
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

        Dim udtSystemMessage As SystemMessage = Nothing
        Dim udtSchemeClaims As SchemeClaimModelCollection = Nothing
        Dim udtClaimCategorys As ClaimCategoryModelCollection = Nothing
        Dim isValid As Boolean = True

        'Service DateBack parameters
        Dim strClaimDayLimit As String = String.Empty
        Dim strMinDate As String = String.Empty
        Dim strAllowDateBack As String = String.Empty
        Dim strOriServiceDate As String = Nothing
        Dim intDayLimit As Integer
        Dim dtmMinDate As DateTime

        Me._udtEHSTransaction = MyBase.SessionHandler.EHSTransactionGetFromSession(Me.FunctionCode)

        'Initi Error image
        ClearServiceDateError()

        '-------------------------------------------------------------------------------------------------
        'Service Date Validation
        '-------------------------------------------------------------------------------------------------
        'Check Service Date Format
        udtSystemMessage = udtValidator.chkServiceDate(strServiceDate)
        If Not udtSystemMessage Is Nothing Then
            isValid = False
            Me.lblServiceDateError.Visible = True
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
                    Me.lblServiceDateError.Visible = True
                    ' Me.udcMsgBoxErr.AddMessage(udtSystemMessage, "%s", strClaimDayLimit)
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
            udtSchemeClaims = Me.GetAvailableScheme()
            udtSchemeClaim = udtSchemeClaims.Filter(udtSchemeClaim.SchemeCode)

            If Not Me._udtEHSTransaction Is Nothing Then
                strOriServiceDate = Me._udtFormatter.convertDate(Me._udtEHSTransaction.ServiceDate)
            End If

            isValid = (New SchemeClaimBLL).IsServiceDateWithinClaimPeriod(udtSchemeClaim.SchemeCode, Me._udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English))


            If isValid Then

                Me._udtEHSTransaction = Me._udtEHSClaimBLL.ConstructNewEHSTransaction(udtSchemeClaim, MyBase.SessionHandler.EHSAccountGetFromSession(Me.FunctionCode), _
                                                                                            MyBase.SessionHandler.PracticeDisplayGetFromSession(Me.FunctionCode), _
                                                                                            Me._udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English), _
                                                                                            GetExtRefStatus(MyBase.SessionHandler.EHSAccountGetFromSession(Me.FunctionCode), udtSchemeClaim.SchemeCode), _
                                                                                            GetDHVaccineRefStatus(MyBase.SessionHandler.EHSAccountGetFromSession(Me.FunctionCode), udtSchemeClaim.SchemeCode))

                'Me._udtEHSTransaction.ServiceDate = Me._udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English)

                'Case of back to service date page from confirm vaccine page
                strServiceDate = Me._udtFormatter.convertDate(Me._udtEHSTransaction.ServiceDate)
                If Not strServiceDate.Equals(strOriServiceDate) Then
                    MyBase.SessionHandler.ClaimCategoryRemoveFromSession(FunctionCode)
                    MyBase.SessionHandler.EHSClaimVaccineRemoveFromSession()
                    Me.VaccineClear()
                End If

                ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
                ' --------------------------------------------------------------------------------------
                Select Case udtSchemeClaim.ControlType
                    Case SchemeClaimModel.EnumControlType.CIVSS, SchemeClaimModel.EnumControlType.EVSS, SchemeClaimModel.EnumControlType.HSIVSS, _
                        SchemeClaimModel.EnumControlType.RVP, SchemeClaimModel.EnumControlType.PIDVSS, SchemeClaimModel.EnumControlType.VSS, _
                        SchemeClaimModel.EnumControlType.ENHVSSO, SchemeClaimModel.EnumControlType.PPP

                        Dim udtInputPicker As InputPickerModel = New InputPickerModel
                        Dim needCategorySelection As Boolean = False

                        'Show radio button list of "Category"
                        If udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.HSIVSS OrElse _
                            udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.RVP OrElse _
                            udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.VSS OrElse _
                            udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.ENHVSSO OrElse _
                            udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.PPP Then

                            needCategorySelection = True

                        End If

                        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                        ' -----------------------------------------------------------------------------------------
                        udtClaimCategorys = Me._udtClaimCategoryBLL.getDistinctCategoryByScheme(udtSchemeClaim, udtPersonalInformation, Me._udtEHSTransaction.ServiceDate)
                        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                        'If RVP, check category whether is enabled.
                        If udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.RVP Then
                            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
                            Dim strEnableClaimCategory As String = Nothing
                            Dim udtClaimCategory As ClaimCategoryModel = Nothing
                            Dim udtEHSClaimVaccine As EHSClaimVaccineModel = Nothing

                            udtGeneralFunction.getSytemParameterByParameterNameSchemeCode("RVPEnableClaimCategory", strEnableClaimCategory, String.Empty, SchemeClaimModel.RVP)

                            If strEnableClaimCategory = "N" Then
                                needCategorySelection = False

                                udtClaimCategory = udtClaimCategorys.FilterByCategoryCode(udtSchemeClaim.SchemeCode, "RESIDENT")

                                'CRE16-026 (Add PCV13) [Start][Chris YIM]
                                '-----------------------------------------------------------------------------------------
                                'Add CategoryCode
                                udtInputPicker.CategoryCode = udtClaimCategory.CategoryCode
                                'CRE16-026 (Add PCV13) [End][Chris YIM]

                                udtEHSClaimVaccine = Me._udtEHSClaimBLL.SearchEHSClaimVaccine(udtSchemeClaim, udtEHSAccount.SearchDocCode, udtEHSAccount, Me._udtEHSTransaction.ServiceDate, True, GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), udtInputPicker)

                                If Me.CheckVaccineAvail(udtEHSClaimVaccine) Then
                                    MyBase.SessionHandler.ClaimCategorySaveToSession(udtClaimCategory, Me.FunctionCode)

                                    'Create Category 
                                    Me._udtEHSTransaction.CategoryCode = udtClaimCategory.CategoryCode

                                    MyBase.SessionHandler.EHSTransactionSaveToSession(Me._udtEHSTransaction, Me.FunctionCode)
                                    MyBase.SessionHandler.SchemeSelectedSaveToSession(udtSchemeClaim, Me.FunctionCode)

                                    Dim udtSelectedPracticeDisplay As PracticeDisplayModel = SessionHandler.PracticeDisplayGetFromSession(FunctionCode)
                                    Dim udtConvertedSchemeCode As String = udtSchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(udtSchemeClaim.SchemeCode)
                                    SessionHandler.NonClinicSettingSaveToSession(SessionSP.PracticeList(udtSelectedPracticeDisplay.PracticeID).PracticeSchemeInfoList.Filter(udtConvertedSchemeCode).IsNonClinic, FunctionCode)

                                    Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.RCHCode
                                Else
                                    isValid = False
                                    Me.udcMsgBoxErr.AddMessage("990000", "E", "00107")
                                End If

                            End If
                        End If

                        If isValid Then
                            If needCategorySelection Then
                                If udtClaimCategorys.Count > 0 Then
                                    Dim udtSelectedPracticeDisplay As PracticeDisplayModel = SessionHandler.PracticeDisplayGetFromSession(FunctionCode)
                                    Dim udtConvertedSchemeCode As String = udtSchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(udtSchemeClaim.SchemeCode)

                                    MyBase.SessionHandler.SchemeSelectedSaveToSession(udtSchemeClaim, Me.FunctionCode)
                                    MyBase.SessionHandler.EHSTransactionSaveToSession(Me._udtEHSTransaction, Me.FunctionCode)
                                    SessionHandler.NonClinicSettingSaveToSession(SessionSP.PracticeList(udtSelectedPracticeDisplay.PracticeID).PracticeSchemeInfoList.Filter(udtConvertedSchemeCode).IsNonClinic, FunctionCode)

                                    GoToView(ActiveViewIndex.Category)
                                Else
                                    isValid = False
                                    'No category for recipient Show Message 
                                    Me.udcMsgBoxErr.AddMessage("990000", "E", "00106")
                                End If
                            Else

                                If udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.CIVSS OrElse _
                                    udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.EVSS OrElse _
                                    udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.PIDVSS Then

                                    Dim udtEHSClaimVaccine As EHSClaimVaccineModel = Nothing
                                    udtEHSClaimVaccine = Me._udtEHSClaimBLL.SearchEHSClaimVaccine(udtSchemeClaim, udtEHSAccount.SearchDocCode, udtEHSAccount, Me._udtEHSTransaction.ServiceDate, True, GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), Nothing)

                                    If Me.CheckVaccineAvail(udtEHSClaimVaccine) Then
                                        ' No category
                                        MyBase.SessionHandler.SchemeSelectedSaveToSession(udtSchemeClaim, Me.FunctionCode)
                                        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                                        '-----------------------------------------------------------------------------------------
                                        Dim udtSelectedPracticeDisplay As PracticeDisplayModel = SessionHandler.PracticeDisplayGetFromSession(FunctionCode)
                                        Dim udtConvertedSchemeCode As String = udtSchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(udtSchemeClaim.SchemeCode)
                                        SessionHandler.NonClinicSettingSaveToSession(SessionSP.PracticeList(udtSelectedPracticeDisplay.PracticeID).PracticeSchemeInfoList.Filter(udtConvertedSchemeCode).IsNonClinic, FunctionCode)
                                        'CRE16-002 (Revamp VSS) [End][Chris YIM]
                                        MyBase.SessionHandler.EHSTransactionSaveToSession(Me._udtEHSTransaction, Me.FunctionCode)

                                        Select Case udtSchemeClaim.ControlType
                                            Case SchemeClaimModel.EnumControlType.CIVSS, SchemeClaimModel.EnumControlType.EVSS
                                                GoToView(ActiveViewIndex.Vaccine)

                                            Case SchemeClaimModel.EnumControlType.PIDVSS
                                                GoToView(ActiveViewIndex.DocumentaryProof)

                                        End Select

                                    Else
                                        isValid = False
                                        Me.udcMsgBoxErr.AddMessage("990000", "E", "00107")
                                    End If
                                Else
                                    ' No category
                                    MyBase.SessionHandler.SchemeSelectedSaveToSession(udtSchemeClaim, Me.FunctionCode)
                                    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                                    '-----------------------------------------------------------------------------------------
                                    Dim udtSelectedPracticeDisplay As PracticeDisplayModel = SessionHandler.PracticeDisplayGetFromSession(FunctionCode)
                                    Dim udtConvertedSchemeCode As String = udtSchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(udtSchemeClaim.SchemeCode)
                                    SessionHandler.NonClinicSettingSaveToSession(SessionSP.PracticeList(udtSelectedPracticeDisplay.PracticeID).PracticeSchemeInfoList.Filter(udtConvertedSchemeCode).IsNonClinic, FunctionCode)
                                    'CRE16-002 (Revamp VSS) [End][Chris YIM]
                                    MyBase.SessionHandler.EHSTransactionSaveToSession(Me._udtEHSTransaction, Me.FunctionCode)

                                    GoToView(ActiveViewIndex.Vaccine)
                                End If
                            End If
                        End If

                    Case Else
                        Throw New Exception("EHSClaim V2 is only for HSIVSS and RVP, user may use more than one browser.")

                End Select
                ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

                Me.udcMsgBoxInfo.BuildMessageBox()
                Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail)
            Else
                Me.udcMsgBoxErr.AddMessage("990000", "E", "00243")

                Me.udcMsgBoxInfo.BuildMessageBox()
                Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail)
            End If
        Else
            Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail)
        End If

    End Sub

    Protected Sub btnServiceDateViewDetail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnServiceDateViewDetail.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearServiceDateError()

        Me.ShowTransactionDetail()
    End Sub

    ''' <summary>
    ''' Click "View Vaccination Record"
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnServiceDateViewVaccinationRecord_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnServiceDateViewVaccinationRecord.Click
        EHSClaimBasePage.AuditLogVaccinationRecordClick(_udtAuditLogEntry)

        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearServiceDateError()

        ShowVaccinationRecord(True)
    End Sub

    Private Sub SetupServiceDate(ByVal activeViewChanged As Boolean)
        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode)

        If Not IsNothing(udtSchemeClaim) AndAlso IsNothing(udtSchemeClaim.SubsidizeGroupClaimList) Then
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            ' Retrieve the subsidize list again
            udtSchemeClaim = (New SchemeClaimBLL).getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(udtSchemeClaim.SchemeCode)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        End If

        Dim udtVaccinationBLL As New VaccinationBLL


        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' -------------------------------------------------------------------------------------
        If Not IsNothing(udtSchemeClaim) _
                AndAlso udtVaccinationBLL.SchemeContainVaccine(udtSchemeClaim) _
                AndAlso (VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N _
                    Or VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CIMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N) Then

            If CheckShowVaccinationRecord() Then Return
            btnServiceDateViewVaccinationRecord.Visible = True
        Else
            btnServiceDateViewVaccinationRecord.Visible = False
        End If
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        Dim udtEHSTransaction As EHSTransactionModel = MyBase.SessionHandler.EHSTransactionGetFromSession(Me.FunctionCode)

        'Render Language
        Me.lblServiceDateText.Text = Me.GetGlobalResourceObject("Text", "ServiceDate")

        If activeViewChanged Then
            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            If Not udtEHSTransaction Is Nothing AndAlso udtEHSTransaction.ServiceDate <> DateTime.MinValue Then
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim udtSubPlatformBLL As New SubPlatformBLL

                'Me.txtServiceDate.Text = Me._udtFormatter.formatEnterDate(udtEHSTransaction.ServiceDate)
                Me.txtServiceDate.Text = Me._udtFormatter.formatInputTextDate(udtEHSTransaction.ServiceDate, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))


            ElseIf Me.txtServiceDate.Text.Trim().Equals(String.Empty) Then
                Dim udtSubPlatformBLL As New SubPlatformBLL
                'Me.txtServiceDate.Text = Me._udtFormatter.formatEnterDate(_udtCommfunct.GetSystemDateTime())
                Me.txtServiceDate.Text = Me._udtFormatter.formatInputTextDate(_udtCommfunct.GetSystemDateTime(), udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))

                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            End If
            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]
        End If

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        If Not IsNothing(udtSchemeClaim) Then
            Dim udtSystemMessage As SystemMessage =  CheckValidHKICInScheme(udtSchemeClaim.SchemeCode)

            If Not udtSystemMessage Is Nothing Then

                Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
                Me.udcMsgBoxErr.BuildMessageBox()

                Me.btnServiceDateNext.Enabled = False
            Else
                Me.btnServiceDateNext.Enabled = True
            End If
        End If
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

    End Sub

    Private Sub ServiceDateClean(ByVal blnCleanSession As Boolean)
        If blnCleanSession Then
            MyBase.SessionHandler.EHSClaimSessionRemove(Me.FunctionCode)
        End If

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtSubPlatformBLL As New SubPlatformBLL

        'Me.txtServiceDate.Text = Me._udtFormatter.formatEnterDate(_udtCommfunct.GetSystemDateTime())
        Me.txtServiceDate.Text = Me._udtFormatter.formatInputTextDate(_udtCommfunct.GetSystemDateTime(), udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Koala]

        ClearServiceDateError()
    End Sub

    Private Sub ClearServiceDateError()

        Me.lblServiceDateError.Visible = False

    End Sub

#End Region

#Region "Select Category"

    Protected Sub btnCategoryCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCategoryCancel.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearCategoryError()

        Me.CancelAction()
    End Sub

    Protected Sub btnCategoryBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCategoryBack.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearCategoryError()

        Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.ServiceDate
    End Sub

    Protected Sub btnCategoryNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCategoryNext.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(Me.FunctionCode)
        Dim udtClaimCategory As ClaimCategoryModel = Nothing
        Dim udtSessionClaimCategory As ClaimCategoryModel = Nothing
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = Nothing

        Me._udtEHSTransaction = MyBase.SessionHandler.EHSTransactionGetFromSession(Me.FunctionCode)

        Dim isValid As Boolean = True
        ClearCategoryError()

        If String.IsNullOrEmpty(Me.rbCategory.SelectedValue) Then
            isValid = False
            Me.lblCategoryError.Visible = True
            Me.SessionHandler.ClaimCategoryRemoveFromSession(Me.FunctionCode)
            Me.udcMsgBoxErr.AddMessage("990000", "E", "00238")
        End If

        If isValid Then
            Dim udtEHSAccount As EHSAccount.EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(Me.FunctionCode)
            Dim udtPersonalInformation As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode)

            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            Dim udtClaimCategorys As ClaimCategoryModelCollection = Me._udtClaimCategoryBLL.getDistinctCategoryByScheme(udtSchemeClaim, udtPersonalInformation, Me._udtEHSTransaction.ServiceDate)
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            udtClaimCategory = udtClaimCategorys.FilterByCategoryCode(udtSchemeClaim.SchemeCode, Me.rbCategory.SelectedValue)
            udtSessionClaimCategory = MyBase.SessionHandler.ClaimCategoryGetFromSession(Me.FunctionCode)
            'CRE16-026 (Add PCV13) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim udtInputPicker As InputPickerModel = New InputPickerModel()

            If udtSessionClaimCategory Is Nothing Then
                'First time select category

                'Add CategoryCode
                udtInputPicker.CategoryCode = udtClaimCategory.CategoryCode

                udtEHSClaimVaccine = Me._udtEHSClaimBLL.SearchEHSClaimVaccine(udtSchemeClaim, udtEHSAccount.SearchDocCode, udtEHSAccount, Me._udtEHSTransaction.ServiceDate, True, GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), udtInputPicker)
            ElseIf Not udtSessionClaimCategory.CategoryCode.Equals(Me.rbCategory.SelectedValue) Then
                'Changed

                'Add CategoryCode
                udtInputPicker.CategoryCode = udtClaimCategory.CategoryCode

                udtEHSClaimVaccine = Me._udtEHSClaimBLL.SearchEHSClaimVaccine(udtSchemeClaim, udtEHSAccount.SearchDocCode, udtEHSAccount, Me._udtEHSTransaction.ServiceDate, True, GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), udtInputPicker)
            Else
                'No change -> Must have value
                udtEHSClaimVaccine = MyBase.SessionHandler.EHSClaimVaccineGetFromSession()
            End If


            If Not Me.CheckVaccineAvail(udtEHSClaimVaccine) Then
                isValid = False
                ' Display Error Box instead of Info Box for no available subsidy or not eligible
                Me.udcMsgBoxErr.AddMessage("990000", "E", "00107")
            Else
                MyBase.SessionHandler.EHSClaimVaccineSaveToSession(udtEHSClaimVaccine)
            End If
            'CRE16-026 (Add PCV13) [End][Chris YIM]
        End If

        If isValid Then
            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            If Not Me._udtEHSTransaction Is Nothing Then
                'Case of back to Select Category page from confirm vaccine page
                If Not Me._udtEHSTransaction.CategoryCode.Equals(udtClaimCategory.CategoryCode) Then
                    ' No need to remove vaccine as the vaccine object is updated in the above checking
                    Me.VaccineClear()

                    'Remove Transaction Addition Fields When category changed 
                    _udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()
                End If
            End If

            '-------------------------------------------------
            'Nothing in first adding TransactionAdditionalField
            '-------------------------------------------------
            If Me._udtEHSTransaction.TransactionAdditionFields Is Nothing Then
                Me._udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()
            End If

            '-------------------------------------------------
            'Remove Addition Fields : Non Clinic Setting
            '-------------------------------------------------
            Dim udtTransactAdditionfield As TransactionAdditionalFieldModel

            Select Case udtSchemeClaim.ControlType
                Case SchemeClaimModel.EnumControlType.VSS
                    If Me._udtEHSTransaction.TransactionAdditionFields Is Nothing Then
                        Me._udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()
                    Else
                        udtTransactAdditionfield = Me._udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ClinicType)
                        If Not udtTransactAdditionfield Is Nothing Then
                            Me._udtEHSTransaction.TransactionAdditionFields.Remove(udtTransactAdditionfield)
                        End If
                    End If

            End Select

            '-------------------------------------------------
            'Set up Addition Fields : Non Clinic Setting
            '-------------------------------------------------
            Select Case udtSchemeClaim.ControlType
                Case SchemeClaimModel.EnumControlType.VSS
                    If Not Me._udtEHSTransaction.TransactionAdditionFields Is Nothing Then
                        udtTransactAdditionfield = New TransactionAdditionalFieldModel()

                        udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
                        udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
                        udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode.Trim()
                        udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ClinicType
                        udtTransactAdditionfield.AdditionalFieldValueCode = IIf(SessionHandler.NonClinicSettingGetFromSession(FunctionCode), ClinicType.NonClinic, ClinicType.Clinic)
                        udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing

                        Me._udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
                        MyBase.SessionHandler.EHSTransactionSaveToSession(Me._udtEHSTransaction, Me.FunctionCode)
                    End If
            End Select

            Me._udtEHSTransaction.CategoryCode = udtClaimCategory.CategoryCode
            MyBase.SessionHandler.EHSTransactionSaveToSession(Me._udtEHSTransaction, Me.FunctionCode)

            MyBase.SessionHandler.ClaimCategorySaveToSession(udtClaimCategory, Me.FunctionCode)

            Select Case udtSchemeClaim.ControlType
                Case SchemeClaimModel.EnumControlType.HSIVSS
                    If udtClaimCategory.IsMedicalCondition = "Y" Then
                        GoToView(ActiveViewIndex.MedicalCondition)
                    Else
                        Me.MedicalConditionClear()
                        GoToView(ActiveViewIndex.Vaccine)
                    End If

                Case SchemeClaimModel.EnumControlType.RVP
                    GoToView(ActiveViewIndex.RCHCode)

                Case SchemeClaimModel.EnumControlType.VSS
                    Select Case udtClaimCategory.CategoryCode
                        Case CategoryCode.VSS_PW, CategoryCode.VSS_CHILD, CategoryCode.VSS_ELDER, CategoryCode.VSS_ADULT
                            If SessionHandler.NonClinicSettingGetFromSession(FunctionCode) Then
                                GoToView(ActiveViewIndex.PlaceVaccination)
                            Else
                                GoToView(ActiveViewIndex.Vaccine)
                            End If

                        Case CategoryCode.VSS_PID, CategoryCode.VSS_DA
                            GoToView(ActiveViewIndex.DocumentaryProof)

                    End Select

                    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
                    ' --------------------------------------------------------------------------------------
                Case SchemeClaimModel.EnumControlType.ENHVSSO
                    Select Case udtClaimCategory.CategoryCode
                        Case CategoryCode.EVSSO_CHILD
                            GoToView(ActiveViewIndex.PlaceVaccination)

                        Case Else
                            Throw New Exception(String.Format("Invalid Category Code:({0})", udtClaimCategory.CategoryCode))

                    End Select
                    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

            End Select
            'CRE16-002 (Revamp VSS) [End][Chris YIM]
        End If


        If Not isValid Then
            ' Remove Session value incase NO Category is selected
            '   or the Category DONT have any available vaccine
            MyBase.SessionHandler.ClaimCategoryRemoveFromSession(FunctionCode)
            MyBase.SessionHandler.EHSClaimVaccineRemoveFromSession()

        End If

        Me.udcMsgBoxErr.BuildMessageBox(_strValidationFail)
        Me.udcMsgBoxInfo.BuildMessageBox()

    End Sub

    Protected Sub btnCategoryViewDetail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCategoryViewDetail.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearCategoryError()

        Me.ShowTransactionDetail()
    End Sub

    ''' <summary>
    ''' Click "View Vaccination Record"
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 
    Protected Sub btnCategoryViewVaccinationRecord_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCategoryViewVaccinationRecord.Click
        EHSClaimBasePage.AuditLogVaccinationRecordClick(_udtAuditLogEntry)

        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearCategoryError()

        ShowVaccinationRecord(True)
    End Sub

    Private Sub SetupCategory(ByVal activeViewChanged As Boolean)
        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(Me.FunctionCode)
        Dim udtEHSAccount As EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(Me.FunctionCode)
        Dim udtPersonalInformation As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode)

        Dim strSelectedCategoryCode As String = Me.rbCategory.SelectedValue
        Dim udtTransactionAdditionField As TransactionAdditionalFieldModel = Nothing
        Dim udtClaimCategorys As ClaimCategoryModelCollection = Nothing
        Dim dtCategory As DataTable = Nothing

        Me._udtEHSTransaction = MyBase.SessionHandler.EHSTransactionGetFromSession(Me.FunctionCode)

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        udtClaimCategorys = Me._udtClaimCategoryBLL.getDistinctCategoryByScheme(udtSchemeClaim, udtPersonalInformation, Me._udtEHSTransaction.ServiceDate)
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

        udtClaimCategorys = udtClaimCategorys.FilterOutBySubsidizeItemCodeReturnCollection(SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19) ' CRE20-0022 (Immu record)[Martin]

        dtCategory = ClaimCategoryBLL.ConvertCategoryToDatatable(udtClaimCategorys, "text")

        ' --------------------------------------------------------------------------------------------------
        ' Vaccination Record
        ' --------------------------------------------------------------------------------------------------
        Dim udtVaccinationBLL As New VaccinationBLL

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        If Not IsNothing(udtSchemeClaim) _
                AndAlso udtVaccinationBLL.SchemeContainVaccine(udtSchemeClaim) _
                AndAlso (VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N _
                    Or VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CIMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N) Then

            btnCategoryViewVaccinationRecord.Visible = True
        Else
            btnCategoryViewVaccinationRecord.Visible = False
        End If
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        '--------------------------------------------------------------------------------------------------
        'Setup Category Radiobutton list
        '--------------------------------------------------------------------------------------------------
        Me.rbCategory.DataSource = dtCategory
        Me.rbCategory.DataValueField = ClaimCategoryModel._Category_Code
        If Me.SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
            Me.rbCategory.DataTextField = ClaimCategoryModel._Category_Name_Chi
        Else
            Me.rbCategory.DataTextField = ClaimCategoryModel._Category_Name
        End If

        Me.rbCategory.DataBind()

        If udtClaimCategorys.Count = 1 Then
            Me.rbCategory.SelectedIndex = 0
        Else
            If Not String.IsNullOrEmpty(strSelectedCategoryCode) Then
                Dim listItem As ListItem = Me.rbCategory.Items.FindByValue(strSelectedCategoryCode)
                If Not listItem Is Nothing Then
                    Me.rbCategory.SelectedValue = strSelectedCategoryCode
                End If

            End If
        End If

        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        If activeViewChanged Then

            If Not Me._udtEHSTransaction.CategoryCode Is Nothing Then

                strSelectedCategoryCode = Me._udtEHSTransaction.CategoryCode

                Dim listItem As ListItem = Me.rbCategory.Items.FindByValue(strSelectedCategoryCode)

                If Not listItem Is Nothing Then
                    Me.rbCategory.SelectedValue = strSelectedCategoryCode
                End If

            End If
        End If
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

    End Sub

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub SetupDocumentaryProof(ByVal activeViewChanged As Boolean)
        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(Me.FunctionCode)
        Dim udtSelectedCategory As ClaimCategoryModel = Nothing
        If udtSchemeClaim.SchemeCode = SchemeClaimModel.EnumControlType.VSS.ToString.Trim Then
            udtSelectedCategory = SessionHandler.ClaimCategoryGetFromSession(FunctionCode)
        End If

        Dim strSelectedDocumentaryProofCode As String = Me.rbDocumentaryProof.SelectedValue

        ' --------------------------------------------------------------------------------------------------
        ' Vaccination Record
        ' --------------------------------------------------------------------------------------------------
        Dim udtVaccinationBLL As New VaccinationBLL

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        If Not IsNothing(udtSchemeClaim) _
                AndAlso udtVaccinationBLL.SchemeContainVaccine(udtSchemeClaim) _
                AndAlso (VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N _
                    Or VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CIMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N) Then

            btnDocumentaryProofViewVaccinationRecord.Visible = True
        Else
            btnDocumentaryProofViewVaccinationRecord.Visible = False
        End If
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        '--------------------------------------------------------------------------------------------------
        'Setup Category Radiobutton list
        '--------------------------------------------------------------------------------------------------
        'Data Source
        Dim udtStaticDataBLL As New StaticDataBLL
        Dim dtDocumentaryProof As DataTable = Nothing

        Select Case udtSchemeClaim.SchemeCode
            Case SchemeClaimModel.EnumControlType.PIDVSS.ToString.Trim
                dtDocumentaryProof = udtStaticDataBLL.GetStaticDataList("PIDVSS_DOCUMENTARYPROOF")
            Case SchemeClaimModel.EnumControlType.VSS.ToString.Trim
                Select Case udtSelectedCategory.CategoryCode
                    Case CategoryCode.VSS_PID
                        dtDocumentaryProof = udtStaticDataBLL.GetStaticDataList("VSSPID_DOCUMENTARYPROOF")
                    Case CategoryCode.VSS_DA
                        ' dtDocumentaryProof = udtStaticDataBLL.GetStaticDataList("VSSDA_DOCUMENTARYPROOF")
                        'CRE20-009 VSS Da with CSSA - text version [Start][Nichole]
                        dtDocumentaryProof = udtStaticDataBLL.GetStaticDataListFilter("VSSDA_DOCUMENTARYPROOF", txtServiceDate.Text)
                        'CRE20-009 VSS Da with CSSA - text version [End][Nichole]
                End Select
        End Select

        Me.rbDocumentaryProof.DataSource = dtDocumentaryProof
        Me.rbDocumentaryProof.DataValueField = StaticDataModel.Item_No
        If Me.SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
            Me.rbDocumentaryProof.DataTextField = StaticDataModel.Data_Value_Chi
        Else
            Me.rbDocumentaryProof.DataTextField = StaticDataModel.Data_Value
        End If

        Dim intDocumentaryProofOption As Integer = dtDocumentaryProof.Rows.Count

        If intDocumentaryProofOption = 1 Then
            rbDocumentaryProof.Visible = False
            chkDocumentaryProof.Visible = True

            If SessionHandler.Language = CultureLanguage.TradChinese Then
                Me.chkDocumentaryProof.Text = dtDocumentaryProof.Rows(0)(StaticDataModel.Data_Value_Chi)
            Else
                Me.chkDocumentaryProof.Text = dtDocumentaryProof.Rows(0)(StaticDataModel.Data_Value)
            End If

            Me.hfDocumentaryProof.Value = dtDocumentaryProof.Rows(0)(StaticDataModel.Item_No)
        Else
            Me.rbDocumentaryProof.DataBind()

            rbDocumentaryProof.Visible = True
            chkDocumentaryProof.Visible = False



            If Not String.IsNullOrEmpty(strSelectedDocumentaryProofCode) Then
                Dim listItem As ListItem = Me.rbDocumentaryProof.Items.FindByValue(strSelectedDocumentaryProofCode)
                If Not listItem Is Nothing Then
                    Me.rbDocumentaryProof.SelectedValue = strSelectedDocumentaryProofCode
                End If

            End If
        End If

        '--------------------------------------------------------------------------------------------------
        'Retain Documentary Proof Radiobutton list
        '--------------------------------------------------------------------------------------------------
        If activeViewChanged Then
            Dim udtTransactionAdditionField As TransactionAdditionalFieldModel = Nothing

            Me._udtEHSTransaction = MyBase.SessionHandler.EHSTransactionGetFromSession(Me.FunctionCode)

            If Not Me._udtEHSTransaction.TransactionAdditionFields Is Nothing Then
                udtTransactionAdditionField = Me._udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.DocumentaryProof)

                'If first time come to this page, udtTransactionAdditionField will be nothing 
                If Not udtTransactionAdditionField Is Nothing Then
                    If intDocumentaryProofOption = 1 Then
                        Me.chkDocumentaryProof.Checked = True
                    Else
                        strSelectedDocumentaryProofCode = udtTransactionAdditionField.AdditionalFieldValueCode

                        Dim listItem As ListItem = Me.rbDocumentaryProof.Items.FindByValue(strSelectedDocumentaryProofCode)

                        If Not listItem Is Nothing Then
                            Me.rbDocumentaryProof.SelectedValue = strSelectedDocumentaryProofCode
                        End If
                    End If

                End If
            End If
        End If

    End Sub
    'CRE16-002 (Revamp VSS) [End][Chris YIM]

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub SetupPlaceVaccination(ByVal activeViewChanged As Boolean)
        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(Me.FunctionCode)

        ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [Start][Winnie]
        Dim strSelectedPlaceVaccination As String = AntiXssEncoder.HtmlEncode(Me.rbPlaceVaccination.SelectedValue, True)
        ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [End][Winnie]

        If activeViewChanged Then
            If Not SessionHandler.PlaceVaccinationGetFromSession(FunctionCode, udtSchemeClaim.SchemeCode) Is Nothing Then
                strSelectedPlaceVaccination = SessionHandler.PlaceVaccinationGetFromSession(FunctionCode, udtSchemeClaim.SchemeCode)
            End If
        End If

        ' --------------------------------------------------------------------------------------------------
        ' Vaccination Record
        ' --------------------------------------------------------------------------------------------------
        Dim udtVaccinationBLL As New VaccinationBLL

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        If Not IsNothing(udtSchemeClaim) _
                AndAlso udtVaccinationBLL.SchemeContainVaccine(udtSchemeClaim) _
                AndAlso (VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N _
                    Or VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CIMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N) Then

            btnPlaceVaccinationViewVaccinationRecord.Visible = True
        Else
            btnPlaceVaccinationViewVaccinationRecord.Visible = False
        End If
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        '--------------------------------------------------------------------------------------------------
        'Setup Category Radiobutton list
        '--------------------------------------------------------------------------------------------------
        Dim udtStaticDataBLL As New StaticDataBLL
        Dim dtPlaceVaccination As DataTable = Nothing

        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Select Case udtSchemeClaim.ControlType
            Case SchemeClaimModel.EnumControlType.VSS
                dtPlaceVaccination = udtStaticDataBLL.GetStaticDataList("VSS_PLACEOFVACCINATION")

            Case SchemeClaimModel.EnumControlType.ENHVSSO
                dtPlaceVaccination = udtStaticDataBLL.GetStaticDataList("ENHVSSO_PLACEOFVACCINATION")

        End Select
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        Me.rbPlaceVaccination.DataSource = dtPlaceVaccination
        Me.rbPlaceVaccination.DataValueField = StaticDataModel.Item_No
        If Me.SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
            Me.rbPlaceVaccination.DataTextField = StaticDataModel.Data_Value_Chi
        Else
            Me.rbPlaceVaccination.DataTextField = StaticDataModel.Data_Value
        End If

        Me.rbPlaceVaccination.DataBind()

        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        panPlaceVaccinationOther.Visible = False

        For Each lstItem As ListItem In rbPlaceVaccination.Items
            If lstItem.Value = PlaceOfVaccinationOptions.OTHERS Then
                panPlaceVaccinationOther.Visible = True
                Exit For
            End If
        Next
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        If dtPlaceVaccination.Rows.Count = 1 Then
            Me.rbPlaceVaccination.SelectedIndex = 0
        Else
            If Not String.IsNullOrEmpty(strSelectedPlaceVaccination) Then
                Dim listItem As ListItem = Me.rbPlaceVaccination.Items.FindByValue(strSelectedPlaceVaccination)
                If Not listItem Is Nothing Then
                    Me.rbPlaceVaccination.SelectedValue = strSelectedPlaceVaccination

                End If
            End If
        End If

        If activeViewChanged Then
            If rbPlaceVaccination.SelectedValue = PlaceOfVaccinationOptions.OTHERS Then
                txtPlaceVaccinationOther.Enabled = True
                lblPlaceVaccinationOther.Enabled = True

                If Not SessionHandler.PlaceVaccinationOtherGetFromSession(FunctionCode, udtSchemeClaim.SchemeCode) Is Nothing Then
                    txtPlaceVaccinationOther.Text = SessionHandler.PlaceVaccinationOtherGetFromSession(FunctionCode, udtSchemeClaim.SchemeCode)
                Else
                    txtPlaceVaccinationOther.Text = String.Empty
                End If
            End If

            Dim udtTransactionAdditionField As TransactionAdditionalFieldModel = Nothing

            Me._udtEHSTransaction = MyBase.SessionHandler.EHSTransactionGetFromSession(Me.FunctionCode)

            If Not Me._udtEHSTransaction.TransactionAdditionFields Is Nothing Then
                udtTransactionAdditionField = Me._udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.PlaceVaccination)

                'If first time come to this page, udtTransactionAdditionField may be nothing 
                If Not udtTransactionAdditionField Is Nothing Then
                    strSelectedPlaceVaccination = udtTransactionAdditionField.AdditionalFieldValueCode

                    Dim listItem As ListItem = Me.rbPlaceVaccination.Items.FindByValue(strSelectedPlaceVaccination)

                    If Not listItem Is Nothing Then
                        Me.rbPlaceVaccination.SelectedValue = strSelectedPlaceVaccination

                        If rbPlaceVaccination.SelectedValue = PlaceOfVaccinationOptions.OTHERS Then
                            txtPlaceVaccinationOther.Enabled = True
                            lblPlaceVaccinationOther.Enabled = True
                            txtPlaceVaccinationOther.Text = udtTransactionAdditionField.AdditionalFieldValueDesc
                        Else
                            txtPlaceVaccinationOther.Enabled = False
                            lblPlaceVaccinationOther.Enabled = False
                            txtPlaceVaccinationOther.Text = String.Empty
                        End If

                    End If

                End If
            End If

        End If

    End Sub
    'CRE16-002 (Revamp VSS) [End][Chris YIM]

    Private Sub CategoryClear()
        Me.rbCategory.Items.Clear()

        ClearCategoryError()
    End Sub

    Private Sub ClearCategoryError()

        Me.lblCategoryError.Visible = False

    End Sub

    'CRE15-005-03 (New PIDVSS scheme) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub DocumentaryProofClear()
        Me.rbDocumentaryProof.Items.Clear()

        ClearDocumentaryProofError()
    End Sub

    Private Sub ClearDocumentaryProofError()

        Me.lblDocumentaryProofError.Visible = False

    End Sub
    'CRE15-005-03 (New PIDVSS scheme) [End][Chris YIM]

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub PlaceVaccinationClear()
        Me.rbPlaceVaccination.Items.Clear()

        ClearDocumentaryProofError()
    End Sub

    Private Sub ClearPlaceVaccinationError()

        Me.lblPlaceVaccinationError.Visible = False

    End Sub
    'CRE16-002 (Revamp VSS) [End][Chris YIM]
#End Region

#Region "Enter RCH Code"

    Protected Sub btnRCHCodeCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRCHCodeCancel.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearRCHCodeError()

        CancelAction()
    End Sub

    Protected Sub btnRCHCodeBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRCHCodeBack.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearRCHCodeError()

        Dim udtSchemeClaim As SchemeClaimModel = SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
        Dim strEnableClaimCategory As String = Nothing

        udtGeneralFunction.getSytemParameterByParameterNameSchemeCode("RVPEnableClaimCategory", strEnableClaimCategory, String.Empty, SchemeClaimModel.RVP)

        Select Case udtSchemeClaim.SchemeCode
            Case SchemeClaimModel.EnumControlType.RVP.ToString.Trim
                If strEnableClaimCategory = "Y" Then
                    GoToView(ActiveViewIndex.Category)
                Else
                    GoToView(ActiveViewIndex.ServiceDate)
                End If
            Case SchemeClaimModel.EnumControlType.VSS.ToString.Trim
                GoToView(ActiveViewIndex.DocumentaryProof)
        End Select

    End Sub

    Protected Sub btnRCHCodeNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRCHCodeNext.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(Me.FunctionCode)
        Dim strRCHCDesc As String = Nothing
        Dim udtSystemMessage As SystemMessage = Nothing

        ClearRCHCodeError()

        Select Case udtSchemeClaim.SchemeCode
            'RVP
            Case SchemeClaimModel.EnumControlType.RVP.ToString.Trim
                strRCHCDesc = Me.lookUpRCHCode(Me.txtRCHCode.Text.Trim, RCH_TYPE.ALL)

                If String.IsNullOrEmpty(txtRCHCode.Text.Trim()) Then
                    Me.lblRCHCodeError.Visible = True

                    Me.SessionHandler.RVPRCHCodeRemoveFromSession(Me.FunctionCode())
                    ' Please input "RCH code".
                    udtSystemMessage = New SystemMessage("990000", "E", "00198")

                    Me.udcMsgBoxErr.AddMessage(udtSystemMessage)

                ElseIf String.IsNullOrEmpty(strRCHCDesc) Then
                    Me.lblRCHCodeError.Visible = True

                    Me.SessionHandler.RVPRCHCodeRemoveFromSession(Me.FunctionCode())
                    ' "RCH Code" is invalid.
                    udtSystemMessage = New SystemMessage("990000", "E", "00219")

                    Me.udcMsgBoxErr.AddMessage(udtSystemMessage)

                Else
                    Dim udtTransactAdditionfield As TransactionAdditionalFieldModel
                    Me._udtEHSTransaction = MyBase.SessionHandler.EHSTransactionGetFromSession(Me.FunctionCode)

                    Me.SessionHandler.RVPRCHCodeSaveToSession(Me.txtRCHCode.Text.Trim(), Me.FunctionCode)

                    '-------------------------------------------------
                    'Remove Addition Fields : RCH Code
                    '-------------------------------------------------
                    If Me._udtEHSTransaction.TransactionAdditionFields Is Nothing Then
                        Me._udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()
                    Else
                        udtTransactAdditionfield = Me._udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.RCHCode)
                        If Not udtTransactAdditionfield Is Nothing Then
                            Me._udtEHSTransaction.TransactionAdditionFields.Remove(udtTransactAdditionfield)
                        End If
                    End If

                    '-------------------------------------------------
                    'Set up Addition Fields : RCHCode
                    '-------------------------------------------------
                    udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                    udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
                    udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
                    udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode.Trim()
                    udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.RCHCode
                    udtTransactAdditionfield.AdditionalFieldValueCode = Me.txtRCHCode.Text.Trim
                    udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty

                    Me._udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
                    MyBase.SessionHandler.EHSTransactionSaveToSession(Me._udtEHSTransaction, Me.FunctionCode)

                    GoToView(ActiveViewIndex.Vaccine)
                End If

                'VSS
            Case SchemeClaimModel.EnumControlType.VSS.ToString.Trim
                strRCHCDesc = Me.lookUpRCHCode(Me.txtRCHCode.Text.Trim, RCH_TYPE.PID)

                If String.IsNullOrEmpty(txtRCHCode.Text.Trim()) Then
                    Me.lblRCHCodeError.Visible = True

                    Me.SessionHandler.PIDInstitutionCodeRemoveFromSession(Me.FunctionCode())
                    ' Please input "PID Institution Code".
                    udtSystemMessage = New SystemMessage("990000", "E", "00387")

                    Me.udcMsgBoxErr.AddMessage(udtSystemMessage)

                ElseIf String.IsNullOrEmpty(strRCHCDesc) Then
                    Me.lblRCHCodeError.Visible = True

                    Me.SessionHandler.PIDInstitutionCodeRemoveFromSession(Me.FunctionCode())
                    ' "PID Institution Code" is invalid.
                    udtSystemMessage = New SystemMessage("990000", "E", "00388")

                    Me.udcMsgBoxErr.AddMessage(udtSystemMessage)

                Else
                    Dim udtTransactAdditionfield As TransactionAdditionalFieldModel
                    Me._udtEHSTransaction = MyBase.SessionHandler.EHSTransactionGetFromSession(Me.FunctionCode)

                    Me.SessionHandler.PIDInstitutionCodeSaveToSession(Me.txtRCHCode.Text.Trim(), Me.FunctionCode())

                    '-------------------------------------------------
                    'Remove Addition Fields : Category
                    '-------------------------------------------------
                    If Me._udtEHSTransaction.TransactionAdditionFields Is Nothing Then
                        Me._udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()
                    Else
                        udtTransactAdditionfield = Me._udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.PIDInstitutionCode)
                        If Not udtTransactAdditionfield Is Nothing Then
                            Me._udtEHSTransaction.TransactionAdditionFields.Remove(udtTransactAdditionfield)
                        End If
                    End If

                    '-------------------------------------------------
                    'Set up Addition Fields : RCHCode
                    '-------------------------------------------------
                    udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                    udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
                    udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
                    udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode.Trim()
                    udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.PIDInstitutionCode
                    udtTransactAdditionfield.AdditionalFieldValueCode = Me.txtRCHCode.Text.Trim
                    udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing

                    Me._udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

                    MyBase.SessionHandler.EHSTransactionSaveToSession(Me._udtEHSTransaction, Me.FunctionCode)

                    If SessionHandler.NonClinicSettingGetFromSession(FunctionCode) Then
                        GoToView(ActiveViewIndex.PlaceVaccination)
                    Else
                        GoToView(ActiveViewIndex.Vaccine)
                    End If

                End If

        End Select

        Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail)

    End Sub

    Protected Sub btnRCHCodeViewDetail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRCHCodeViewDetail.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearRCHCodeError()

        Me.ShowTransactionDetail()
    End Sub

    ''' <summary>
    ''' Click "View Vaccination Record"
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 
    Protected Sub btnRCHCodeViewVaccinationRecord_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRCHCodeViewVaccinationRecord.Click
        EHSClaimBasePage.AuditLogVaccinationRecordClick(_udtAuditLogEntry)

        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearRCHCodeError()

        ShowVaccinationRecord(True)
    End Sub

    Private Sub SetupRCHCode(ByVal activeViewChanged As Boolean)
        ' --------------------------------------------------------------------------------------------------
        ' Vaccination Record
        ' --------------------------------------------------------------------------------------------------
        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
        Dim udtVaccinationBLL As New VaccinationBLL

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        If Not IsNothing(udtSchemeClaim) _
                AndAlso udtVaccinationBLL.SchemeContainVaccine(udtSchemeClaim) _
                AndAlso (VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N _
                    Or VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CIMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N) Then

            btnRCHCodeViewVaccinationRecord.Visible = True
        Else
            btnRCHCodeViewVaccinationRecord.Visible = False
        End If
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        ' Translation
        Select Case udtSchemeClaim.SchemeCode
            Case SchemeClaimModel.EnumControlType.RVP.ToString.Trim
                Me.lblRCHCodeText.Text = Me.GetGlobalResourceObject("Text", "RCHCode")

            Case SchemeClaimModel.EnumControlType.VSS.ToString.Trim
                Me.lblRCHCodeText.Text = Me.GetGlobalResourceObject("Text", "PIDInstitutionCode")
        End Select

        ' RCH Code TextBox
        If activeViewChanged Then
            Select Case udtSchemeClaim.SchemeCode
                Case SchemeClaimModel.EnumControlType.RVP.ToString.Trim
                    If Not MyBase.SessionHandler.RVPRCHCodeGetFromSession(Me.FunctionCode) Is Nothing Then
                        Me.txtRCHCode.Text = MyBase.SessionHandler.RVPRCHCodeGetFromSession(Me.FunctionCode)
                    End If

                Case SchemeClaimModel.EnumControlType.VSS.ToString.Trim
                    Me.txtRCHCode.Text = MyBase.SessionHandler.PIDInstitutionCodeGetFromSession(Me.FunctionCode)

            End Select
        End If
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

    End Sub

    Private Sub RCHCodeClear()
        Me.txtRCHCode.Text = String.Empty

        ClearRCHCodeError()
    End Sub

    Private Sub ClearRCHCodeError()

        Me.lblRCHCodeError.Visible = False

    End Sub

    Private Function lookUpRCHCode(ByVal strRCHCode As String, ByVal strRCHType As String) As String
        Dim udtRVPHomeListBLL As New Common.Component.RVPHomeList.RVPHomeListBLL()
        Dim dtResult As DataTable = udtRVPHomeListBLL.getRVPHomeListActiveByCode(strRCHCode, strRCHType)
        Dim strRCHName As String = String.Empty
        Dim strRCHNameChi As String = String.Empty

        If dtResult.Rows.Count > 0 Then

            Me.txtRCHCode.Text = dtResult.Rows(0)("RCH_Code").ToString().Trim().ToUpper()

            strRCHName = dtResult.Rows(0)("Homename_Eng").ToString().Trim()
            If Not dtResult.Rows(0).IsNull("Homename_Chi") Then
                strRCHNameChi = strRCHName
            Else
                strRCHNameChi = dtResult.Rows(0)("Homename_Chi").ToString().Trim()
            End If

            Return String.Format("{0}|||{1}", strRCHName, strRCHNameChi)
        Else
            Return Nothing
        End If
    End Function

#End Region

#Region "Select Medical Condition"

    Protected Sub btnMedicalConditionCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMedicalConditionCancel.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearMedicalConditionError()

        CancelAction()
    End Sub

    Protected Sub btnMedicalConditionBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMedicalConditionBack.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearMedicalConditionError()

        Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Category
    End Sub

    Protected Sub btnMedicalConditionNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMedicalConditionNext.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearMedicalConditionError()

        If String.IsNullOrEmpty(Me.rbMedicalCondition.SelectedValue) Then
            Me.udcMsgBoxErr.AddMessage("990000", "E", "00196")

        Else
            Me._udtEHSTransaction = MyBase.SessionHandler.EHSTransactionGetFromSession(Me.FunctionCode)

            Dim udtTransactAdditionfield As TransactionAdditionalFieldModel
            Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(Me.FunctionCode)
            Dim udtEHSAccount As EHSAccount.EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(Me.FunctionCode)
            Dim udtPersonalInformation As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode)

            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            Dim udtClaimCategorys As ClaimCategoryModelCollection = Me._udtClaimCategoryBLL.getDistinctCategoryByScheme(udtSchemeClaim, udtPersonalInformation, Me._udtEHSTransaction.ServiceDate)
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            '-------------------------------------------------
            'Remove Addition Fields : PreCondition
            '-------------------------------------------------
            If Me._udtEHSTransaction.TransactionAdditionFields Is Nothing Then
                Me._udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()
            Else
                udtTransactAdditionfield = Me._udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID("PreCondition")
                If Not udtTransactAdditionfield Is Nothing Then
                    Me._udtEHSTransaction.TransactionAdditionFields.Remove(udtTransactAdditionfield)
                End If
            End If

            '-------------------------------------------------
            'Set up Transaction Model Addition Fields : PreCondition
            '-------------------------------------------------
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()

            udtTransactAdditionfield.AdditionalFieldID = "PreCondition"
            udtTransactAdditionfield.AdditionalFieldValueCode = Me.rbMedicalCondition.SelectedValue
            udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
            udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            'udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SchemeSeq
            udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
            udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode.Trim()
            Me._udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

            MyBase.SessionHandler.EHSTransactionSaveToSession(Me._udtEHSTransaction, Me.FunctionCode)

            GoToView(ActiveViewIndex.Vaccine)
        End If

        Me.udcMsgBoxErr.BuildMessageBox(_strValidationFail)
    End Sub

    Protected Sub btnMedicalConditionViewDetail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMedicalConditionViewDetail.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearMedicalConditionError()

        Me.ShowTransactionDetail()
    End Sub

    ''' <summary>
    ''' Click "View Vaccination Record"
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 
    Protected Sub btnMedicalConditionViewVaccinationRecord_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMedicalConditionViewVaccinationRecord.Click
        EHSClaimBasePage.AuditLogVaccinationRecordClick(_udtAuditLogEntry)

        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearMedicalConditionError()

        ShowVaccinationRecord(True)
    End Sub

    Private Sub SetupMedicalCondition(ByVal activeViewChanged As Boolean)
        Dim udtEHSTransaction As EHSTransactionModel = MyBase.SessionHandler.EHSTransactionGetFromSession(Me.FunctionCode)
        Dim dtPreCondition As DataTable = Me._udtStaticDataBLL.GetStaticDataList("PreCondition")
        Dim strSelectedMedicalCondition As String = Me.rbMedicalCondition.SelectedValue
        Dim udtTransactionAdditionField As TransactionAdditionalFieldModel = Nothing
        Dim listItem As ListItem

        ' --------------------------------------------------------------------------------------------------
        ' Vaccination Record
        ' --------------------------------------------------------------------------------------------------
        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
        Dim udtVaccinationBLL As New VaccinationBLL

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        If Not IsNothing(udtSchemeClaim) _
                AndAlso udtVaccinationBLL.SchemeContainVaccine(udtSchemeClaim) _
                AndAlso (VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N _
                    Or VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CIMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N) Then

            btnMedicalConditionViewVaccinationRecord.Visible = True
        Else
            btnMedicalConditionViewVaccinationRecord.Visible = False
        End If
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        '--------------------------------------------------------------------------------------------------
        'Setup Category Radiobutton list
        '--------------------------------------------------------------------------------------------------
        Me.rbMedicalCondition.DataSource = dtPreCondition
        Me.rbMedicalCondition.DataValueField = Common.Component.StaticData.StaticDataModel.Item_No
        If Me.SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
            Me.rbMedicalCondition.DataTextField = Common.Component.StaticData.StaticDataModel.Data_Value_Chi
        Else
            Me.rbMedicalCondition.DataTextField = Common.Component.StaticData.StaticDataModel.Data_Value
        End If

        Me.rbMedicalCondition.DataBind()

        If dtPreCondition.Rows.Count = 1 Then
            Me.rbCategory.SelectedIndex = 0
        Else
            If Not String.IsNullOrEmpty(strSelectedMedicalCondition) Then
                listItem = Me.rbMedicalCondition.Items.FindByValue(strSelectedMedicalCondition)
                If Not listItem Is Nothing Then
                    Me.rbMedicalCondition.SelectedValue = strSelectedMedicalCondition
                End If
            End If
        End If

        If activeViewChanged Then

            If Not udtEHSTransaction.TransactionAdditionFields Is Nothing Then

                udtTransactionAdditionField = udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID("PreCondition")

                'If first time come to this page, udtTransactionAdditionField will be nothing 
                If Not udtTransactionAdditionField Is Nothing Then
                    strSelectedMedicalCondition = udtTransactionAdditionField.AdditionalFieldValueCode

                    listItem = Me.rbCategory.Items.FindByValue(strSelectedMedicalCondition)

                    If Not listItem Is Nothing Then
                        Me.rbMedicalCondition.SelectedValue = strSelectedMedicalCondition
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub MedicalConditionClear()
        ClearMedicalConditionError()

        Me.rbMedicalCondition.Items.Clear()
    End Sub

    Private Sub ClearMedicalConditionError()

        Me.lblMedicalConditionError.Visible = False

    End Sub

#End Region

#Region "Select Vaccine"

    Protected Sub btnVaccineNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVaccineNext.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearVaccineError()
        Me.ConfirmVaccineSubmit(False)

    End Sub

    Protected Sub btnVaccineBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVaccineBack.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearVaccineError()

        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(Me.FunctionCode)

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Select Case udtSchemeClaim.ControlType
            Case SchemeClaimModel.EnumControlType.RVP

                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.RCHCode

            Case SchemeClaimModel.EnumControlType.HSIVSS
                Me._udtEHSTransaction = MyBase.SessionHandler.EHSTransactionGetFromSession(Me.FunctionCode)
                Dim udtTransactionAdditionalField As TransactionAdditionalFieldModel = Me._udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID("PreCondition")

                If Not udtTransactionAdditionalField Is Nothing Then
                    Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.MedicalCondition
                Else
                    Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Category
                End If
            Case SchemeClaimModel.EnumControlType.CIVSS, SchemeClaimModel.EnumControlType.EVSS
                mvEHSClaim.ActiveViewIndex = ActiveViewIndex.ServiceDate
                'CRE15-005-03 (New PIDVSS scheme) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
            Case SchemeClaimModel.EnumControlType.PIDVSS
                mvEHSClaim.ActiveViewIndex = ActiveViewIndex.DocumentaryProof
                'CRE15-005-03 (New PIDVSS scheme) [End][Chris YIM]

            Case SchemeClaimModel.EnumControlType.VSS

                Dim udtSelectedCategory As ClaimCategoryModel = SessionHandler.ClaimCategoryGetFromSession(FunctionCode)
                Select Case udtSelectedCategory.CategoryCode
                    Case CategoryCode.VSS_PW, CategoryCode.VSS_CHILD, CategoryCode.VSS_ELDER, CategoryCode.VSS_ADULT
                        If SessionHandler.NonClinicSettingGetFromSession(FunctionCode) Then
                            GoToView(ActiveViewIndex.PlaceVaccination)
                        Else
                            GoToView(ActiveViewIndex.Category)
                        End If

                    Case CategoryCode.VSS_PID, CategoryCode.VSS_DA
                        If SessionHandler.NonClinicSettingGetFromSession(FunctionCode) Then
                            GoToView(ActiveViewIndex.PlaceVaccination)
                        Else
                            GoToView(ActiveViewIndex.DocumentaryProof)
                        End If

                End Select

                ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
                ' --------------------------------------------------------------------------------------
            Case SchemeClaimModel.EnumControlType.ENHVSSO

                Dim udtSelectedCategory As ClaimCategoryModel = SessionHandler.ClaimCategoryGetFromSession(FunctionCode)
                Select Case udtSelectedCategory.CategoryCode
                    Case CategoryCode.EVSSO_CHILD
                        GoToView(ActiveViewIndex.PlaceVaccination)

                End Select
                ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        End Select
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

    End Sub

    Protected Sub btnVaccineCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVaccineCancel.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearVaccineError()

        Me.CancelAction()
    End Sub

    Protected Sub btnVaccineViewDetail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVaccineViewDetail.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearVaccineError()

        Me.ShowTransactionDetail()
    End Sub

    ''' <summary>
    ''' Click "View Vaccination Record" from Service Date view
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnVaccineViewVaccinationRecord_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVaccineViewVaccinationRecord.Click
        EHSClaimBasePage.AuditLogVaccinationRecordClick(_udtAuditLogEntry)

        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearVaccineError()

        ShowVaccinationRecord(True)
    End Sub

    Private Sub udcVaccineClaimVaccineInputText_RemarkClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles udcVaccineClaimVaccineInputText.RemarkClicked
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearVaccineError()

        ShowVaccineRemarkText()
    End Sub

    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub lBtnRecipientConditionHelp_Click(sender As Object, e As EventArgs) Handles lBtnRecipientConditionHelp.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearVaccineError()

        ShowRecipientConditionHelpText()
    End Sub
    'CRE16-026 (Add PCV13) [End][Chris YIM]

    Private Sub SetupVaccine(ByVal activeViewChanged As Boolean)
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = MyBase.SessionHandler.EHSClaimVaccineGetFromSession()
        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(Me.FunctionCode)
        Dim udtEHSTransaction As EHSTransactionModel = MyBase.SessionHandler.EHSTransactionGetFromSession(Me.FunctionCode)
        Dim udtEHSAccount As EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(Me.FunctionCode)
        Dim udtClaimCategory As ClaimCategoryModel = MyBase.SessionHandler.ClaimCategoryGetFromSession(Me.FunctionCode)
        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtInputPicker As InputPickerModel = Nothing

        ' --------------------------------------------------------------------------------------------------
        ' Vaccination Record
        ' --------------------------------------------------------------------------------------------------
        Dim udtVaccinationBLL As New VaccinationBLL

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        If Not IsNothing(udtSchemeClaim) _
                AndAlso udtVaccinationBLL.SchemeContainVaccine(udtSchemeClaim) _
                AndAlso (VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N _
                    Or VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CIMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N) Then

            btnVaccineViewVaccinationRecord.Visible = True
        Else
            btnVaccineViewVaccinationRecord.Visible = False
        End If
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' --------------------------------------------------------------------------------------------------
        ' UI Display Text
        ' --------------------------------------------------------------------------------------------------
        Me.udcVaccineClaimVaccineInputText.VaccineText = Me.GetGlobalResourceObject("Text", "Vaccine")
        Me.udcVaccineClaimVaccineInputText.DoseText = Me.GetGlobalResourceObject("Text", "Dose")
        Me.udcVaccineClaimVaccineInputText.AmountText = IIf(udtEHSTransaction.SchemeCode = SchemeClaimModel.RVP, HttpContext.GetGlobalResourceObject("Text", "InjectionCost"), HttpContext.GetGlobalResourceObject("Text", "SubsidyAmount"))
        Me.udcVaccineClaimVaccineInputText.RemarksText = Me.GetGlobalResourceObject("Text", "Remarks")
        Me.udcVaccineClaimVaccineInputText.TotalAmount = IIf(udtEHSTransaction.SchemeCode = SchemeClaimModel.RVP, HttpContext.GetGlobalResourceObject("Text", "TotalInjectionCost"), HttpContext.GetGlobalResourceObject("Text", "TotalSubsidyAmount"))
        Me.udcVaccineClaimVaccineInputText.NAText = Me.GetGlobalResourceObject("Text", "N/A")
        Me.udcVaccineClaimVaccineInputText.IsSupportedDevice = True
        Me.udcVaccineClaimVaccineInputText.SubsidizeDisableDetail = Me.GetGlobalResourceObject("Text", "Detail")

        'if not Vaccine -> get the vaccination list
        If udtEHSClaimVaccine Is Nothing OrElse Not udtEHSClaimVaccine.SchemeCode.Equals(udtSchemeClaim.SchemeCode) Then
            udtInputPicker = New InputPickerModel

            'Add CategoryCode
            udtInputPicker.CategoryCode = String.Empty
            If Not IsNothing(udtClaimCategory) Then udtInputPicker.CategoryCode = udtClaimCategory.CategoryCode

            udtEHSClaimVaccine = Me._udtEHSClaimBLL.SearchEHSClaimVaccine(udtSchemeClaim, udtEHSAccount.SearchDocCode, udtEHSAccount, udtEHSTransaction.ServiceDate, True, GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), udtInputPicker)
            MyBase.SessionHandler.EHSClaimVaccineSaveToSession(udtEHSClaimVaccine)
        End If

        '-----------------------------------------------------------------------------------------------------------------------------------
        'If practice has enrolled the scheme and has provided service under that scheme, the subsidize will add in the pool for display.
        '-----------------------------------------------------------------------------------------------------------------------------------
        Dim intSelectedPracticeDisplay As Integer = MyBase.SessionHandler.PracticeDisplayGetFromSession(Me.FunctionCode).PracticeID
        MyBase.SessionHandler.CurrentUserGetFromSession(_udtSP, _udtDataEntryModel)

        Dim udtResEHSClaimSubsidizeModelCollection As New EHSClaimVaccineModel.EHSClaimSubsidizeModelCollection

        If Not udtEHSClaimVaccine Is Nothing AndAlso _
            Not _udtSP.PracticeList(intSelectedPracticeDisplay) Is Nothing AndAlso _
            Not _udtSP.PracticeList(intSelectedPracticeDisplay).PracticeSchemeInfoList Is Nothing Then
            For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
                Dim blnRes As Boolean = False

                For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In _udtSP.PracticeList(intSelectedPracticeDisplay).PracticeSchemeInfoList.Values
                    If udtPracticeSchemeInfo.SubsidizeCode = udtEHSClaimSubsidize.SubsidizeCode Then
                        If udtPracticeSchemeInfo.ProvideService Then
                            blnRes = True
                        End If
                    End If
                Next

                If blnRes Then
                    udtResEHSClaimSubsidizeModelCollection.Add(udtEHSClaimSubsidize)
                End If
            Next

            udtEHSClaimVaccine.SubsidizeList = udtResEHSClaimSubsidizeModelCollection
        End If

        '------------------
        'Bulid Vaccine
        '------------------
        Me.udcVaccineClaimVaccineInputText.Build(udtEHSClaimVaccine, Not activeViewChanged)

        If Not udtEHSClaimVaccine Is Nothing AndAlso udtEHSClaimVaccine.SubsidizeList.Count > 0 Then
            AddHandler Me.udcVaccineClaimVaccineInputText.SubsidizeDisabledRemarkClicked, AddressOf udcClaimVaccineInputText_SubsidizeDisabledRemarkClicked

            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
            ' --------------------------------------------------------------------------------------
            Select Case udtSchemeClaim.ControlType
                Case SchemeClaimModel.EnumControlType.VSS, SchemeClaimModel.EnumControlType.RVP
                    'If turned on high risk option, the high risk option is shown.
                    If HighRiskOptionShown(udtEHSTransaction, udtEHSClaimVaccine) = SubsidizeGroupClaimModel.HighRiskOptionClass.ShowForInput Then
                        panVSSRecipientCondition.Visible = True
                        BindRecipientCondition()

                        'Recipient Condition
                        Me.rblRecipientCondition.Enabled = False

                        If udtEHSClaimVaccine.IsSelectedSubsidizeWithHighRisk Then
                            Me.rblRecipientCondition.Enabled = True
                            SetHighRisk(SessionHandler.HighRiskGetFromSession(FunctionCode))
                        End If

                        AddHandler Me.udcVaccineClaimVaccineInputText.SubsidizeCheckboxClickedEnableRecipientCondition, AddressOf udcClaimVaccineInputText_SubsidizeCheckboxClickedEnableRecipientCondition
                    Else
                        panVSSRecipientCondition.Visible = False
                    End If

                Case Else
                    panVSSRecipientCondition.Visible = False

            End Select
            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        End If
        'CRE16-026 (Add PCV13) [End][Chris YIM]

    End Sub

    Private Sub VaccineClear()

        ClearVaccineError()

        udcVaccineClaimVaccineInputText.Clear()

    End Sub

    Private Sub ClearVaccineError()
        Me.udcVaccineClaimVaccineInputText.SetDoseErrorImage(False)
        lblRecipientConditionError.Visible = False
    End Sub

    Private Sub ConfirmVaccineSubmit(ByVal blnIsConfirmed As Boolean)
        Dim udtEHSAccount As EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(FunctionCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel
        Dim udtDataEntryUser As DataEntryUserModel = Nothing
        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(Me.FunctionCode)
        Me._udtEHSTransaction = MyBase.SessionHandler.EHSTransactionGetFromSession(Me.FunctionCode)

        Dim isValid As Boolean = True
        Dim isTSWCase As Boolean = False

        ' Audit Log System Time
        EHSClaimBasePage.AuditLogEnterClaimDetailStart(_udtAuditLogEntry, blnIsConfirmed, MyBase.SessionHandler.SmartIDContentGetFormSession(FunctionCode))

        ' Get SP
        MyBase.SessionHandler.CurrentUserGetFromSession(Me._udtSP, udtDataEntryUser)

        ' Check Service Date VS Permit to Remain
        If isValid Then
            udtEHSPersonalInfo = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode.Trim())
            If udtEHSPersonalInfo.DocCode = DocTypeModel.DocTypeCode.ID235B AndAlso udtEHSPersonalInfo.PermitToRemainUntil.HasValue Then
                Dim udtSystemMessage As SystemMessage = MyBase.Validator.ChkServiceDatePermitToRemainUntil(_udtEHSTransaction.ServiceDate, udtEHSPersonalInfo.PermitToRemainUntil.Value)
                If Not udtSystemMessage Is Nothing Then
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
                End If
            End If
        End If

        ' Scheme Code Specific Validation
        If isValid Then

            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
            ' --------------------------------------------------------------------------------------
            Select Case udtSchemeClaim.ControlType
                Case SchemeClaimModel.EnumControlType.CIVSS
                    isValid = CIVSSValidation(blnIsConfirmed, _udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.EVSS
                    isValid = EVSSValidation(blnIsConfirmed, _udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.HSIVSS
                    isValid = Me.HSIVSSValidation(blnIsConfirmed, Me._udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.RVP
                    isValid = Me.RVPValidation(blnIsConfirmed, Me._udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.PIDVSS
                    isValid = Me.PIDVSSValidation(blnIsConfirmed, _udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.VSS
                    isValid = Me.VSSValidation(blnIsConfirmed, _udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.ENHVSSO
                    isValid = Me.ENHVSSOValidation(blnIsConfirmed, _udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.PPP
                    isValid = Me.PPPValidation(blnIsConfirmed, _udtEHSTransaction)
            End Select
            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        End If

        ' Finalize Transaction object and go to Confirm claim
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

            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            ' Post Validation Scheme Specific Function
            Select Case udtSchemeClaim.ControlType
                Case SchemeClaimModel.EnumControlType.VOUCHER ', SchemeClaimModel.EnumControlType.VOUCHERCHINA
                    ' TSW Checking
                    If udtSchemeClaim.TSWCheckingEnable Then
                        isTSWCase = Me._udtEHSClaimBLL.chkIsTSWCase(Me._udtSP.SPID, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).IdentityNum)
                    End If

                Case SchemeClaimModel.EnumControlType.RVP
                    ' Save RCH Code to Session
                    Dim udtTransactionAdditionalField As EHSTransaction.TransactionAdditionalFieldModel = _udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.RCHCode)
                    SessionHandler.RVPRCHCodeSaveToSession(FunctionCode, udtTransactionAdditionalField.AdditionalFieldValueCode)

                    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                Case SchemeClaimModel.EnumControlType.VSS
                    ' Save RCH Code to Session
                    Dim udtTransactionAdditionalField As EHSTransaction.TransactionAdditionalFieldModel = _udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.PIDInstitutionCode)
                    If Not udtTransactionAdditionalField Is Nothing Then
                        SessionHandler.PIDInstitutionCodeSaveToSession(udtTransactionAdditionalField.AdditionalFieldValueCode, FunctionCode)
                    End If

                    udtTransactionAdditionalField = _udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.PlaceVaccination)
                    If Not udtTransactionAdditionalField Is Nothing Then
                        Me.SessionHandler.PlaceVaccinationSaveToSession(udtTransactionAdditionalField.AdditionalFieldValueCode, Me.FunctionCode(), udtTransactionAdditionalField.SchemeCode)

                        If udtTransactionAdditionalField.AdditionalFieldValueCode = PlaceOfVaccinationOptions.OTHERS Then
                            Me.SessionHandler.PlaceVaccinationOtherSaveToSession(udtTransactionAdditionalField.AdditionalFieldValueDesc, Me.FunctionCode(), udtTransactionAdditionalField.SchemeCode)
                        Else
                            Me.SessionHandler.PlaceVaccinationOtherRemoveFromSession(Me.FunctionCode())
                        End If
                    End If
                    'CRE16-002 (Revamp VSS) [End][Chris YIM]
            End Select

            ' Construct Transaction Detail
            'CRE13-019-02 Extend HCVS to China [Start][Karl]
            If udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.VOUCHER Then 'OrElse udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.VOUCHERCHINA Then
                'CRE13-019-02 Extend HCVS to China [End][Karl]
                Me._udtEHSClaimBLL.ConstructEHSTransactionDetails(Me._udtSP, udtDataEntryUser, Me._udtEHSTransaction, udtEHSAccount)
            Else
                'CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim dicVaccineRef As Dictionary(Of String, String) = EHSTransactionModel.GetVaccineRef(GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), Me._udtEHSTransaction)
                Me._udtEHSTransaction.EHSVaccineResult = dicVaccineRef(EHSTransactionModel.VaccineRefType.EHS)
                Me._udtEHSTransaction.HAVaccineResult = dicVaccineRef(EHSTransactionModel.VaccineRefType.HA)
                Me._udtEHSTransaction.DHVaccineResult = dicVaccineRef(EHSTransactionModel.VaccineRefType.DH)
                'CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

                Me._udtEHSClaimBLL.ConstructEHSTransactionDetails(Me._udtSP, udtDataEntryUser, Me._udtEHSTransaction, udtEHSAccount, MyBase.SessionHandler.EHSClaimVaccineGetFromSession())
            End If
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

            MyBase.SessionHandler.EHSTransactionSaveToSession(Me._udtEHSTransaction, FunctionCode)


            'Audit Log Success
            EHSClaimBasePage.AuditLogEnterClaimDetailPassed(_udtAuditLogEntry, Me._udtEHSTransaction, blnIsConfirmed, isTSWCase)

            GoToView(ActiveViewIndex.ConfirmDetail)
        Else

            ' Audlt Log Failed
            EHSClaimBasePage.AuditLogShowClaimRulePopupBox(_udtAuditLogEntry)
        End If

        Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail)
    End Sub

#End Region

#Region "Select Print Option"

    Protected Sub btnPrintOptionSelectionBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintOptionSelectionBack.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        BackToPreviousView()
    End Sub

    Protected Sub btnPrintOptionSelectionSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintOptionSelectionSelect.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        Dim strSelectedPrintingOption As String = Me.udtPrintOptionSelection.getSelection()
        Dim udtSysMsgPrintOption As SystemMessage = MyBase.UpdateUserPrintOption(strSelectedPrintingOption)

        If udtSysMsgPrintOption Is Nothing Then
            ' Audit Log
            EHSClaimBasePage.AuditLogPrintOptionSelected(_udtAuditLogEntry, strSelectedPrintingOption)

            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            Me.hfCurrentPrintOption.Value = strSelectedPrintingOption
            'CRE13-019-02 Extend HCVS to China [End][Winnie]

            ' Return to the confirm claim page
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.ConfirmDetail
        Else
            Me.udcMsgBoxErr.AddMessage(udtSysMsgPrintOption)
            Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail, _udtAuditLogEntry, Common.Component.LogID.LOG00022, "Print Option Selection Fail")
        End If

    End Sub

    Protected Sub RefreshConfirmDetailPrintOptionControl()
        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
        Dim udtEHSTransaction As EHSTransactionModel = SessionHandler.EHSTransactionGetFromSession(FunctionCode)

        If udtSchemeClaim Is Nothing Then
            Throw New ArgumentNullException("SchemeClaimModel")
        End If

        If udtEHSTransaction Is Nothing Then
            Throw New ArgumentNullException("EHSTransactionModel")
        End If


        ' CRE13-001 - EHAPP [Start][Koala]
        ' -------------------------------------------------------------------------------------
        Dim udtSchemeClaimTemp As SchemeClaimModel = (New SchemeClaimBLL).getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtSchemeClaim.SchemeCode, udtEHSTransaction.ServiceDate.Date.AddDays(1).AddMinutes(-1))
        Dim udtSubsidizeClln As SubsidizeGroupClaimModelCollection = udtSchemeClaimTemp.SubsidizeGroupClaimList.FilterBySchemeCodeAndSubsidizeCode(udtEHSTransaction.TransactionDetails(0).SchemeCode, udtEHSTransaction.TransactionDetails(0).SubsidizeCode)
        ' CRE13-001 - EHAPP [End][Koala]

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        Dim strPrintOption As String = String.Empty

        Dim blnConsentFormAvailability As Boolean = False
        Dim blnPrintOptionAvailability As Boolean = False
        Dim slConsentFormAvailableLang As String() = Nothing
        Dim strConsentFormAvailableVersion As String = Nothing

        Me.hfCurrentPrintOption.Value = Nothing

        blnConsentFormAvailability = udtSubsidizeClln(0).ConsentFormAvailable
        blnPrintOptionAvailability = udtSubsidizeClln(0).PrintOptionAvailable
        slConsentFormAvailableLang = udtSubsidizeClln(0).ConsentFormAvailableLang
        strConsentFormAvailableVersion = udtSubsidizeClln(0).ConsentFormAvailableVersion
        'CRE13-019-02 Extend HCVS to China [End][Winnie]

        ' INT13-0022 - Fix some special handling on HCSP text only version [Start][Koala]
        ' -------------------------------------------------------------------------------------
        ' Check the browser is from Mobile device, hide the printing stuff if is mobile
        ' if the first subsidize need print form -> this scheme need to print form 
        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        'If udtSubsidizeClln(0).ConsentFormAvailable Then
        If blnConsentFormAvailability Then
            'CRE13-019-02 Extend HCVS to China [End][Winnie]
            ' INT13-0022 - Fix some special handling on HCSP text only version [End][Koala]
            ' Allow User to Change Print Option
            Me.panConfirmDetailPrintClaimConsentForm.Visible = True

            strPrintOption = MyBase.GetCurrentUserPrintOption()

            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            If blnPrintOptionAvailability Then
                'Set up print Option
                Me.btnConfirmDetailChangePrintOption.Visible = True
                Me.udtPrintOptionSelection.setPrintOption(strConsentFormAvailableVersion)
            Else
                Me.btnConfirmDetailChangePrintOption.Visible = False
            End If

            Dim udtConsentFormPrintOption As New HCSP.BLL.ConsentFormPrintOptionBLL
            strPrintOption = udtConsentFormPrintOption.GetCurrentPrintOption(blnPrintOptionAvailability, strConsentFormAvailableVersion, MyBase.GetCurrentUserPrintOption())

            Me.PrintClaimConsentFormLanguageSetup(slConsentFormAvailableLang)
            'CRE13-019-02 Extend HCVS to China [End][Winnie]    

            If strPrintOption = Common.Component.PrintFormOptionValue.PreprintForm Then
                ' Ad-hoc Printing
                Me.panlblConfirmDetailPrintConsent.Visible = False
                Me.panConfirmDetailPerprintFormNotice.Visible = True
                Me.btnConfirmDetailAdhocPrintConsentForm.Visible = IIf(udtSubsidizeClln(0).AdhocPrintAvailable, True, False)

                ' Ad-hoc Printing not need to print
                Me.btnConfirmDetailConfirm.Visible = True

            Else
                ' Consent Full / Condensed Printing
                Me.panlblConfirmDetailPrintConsent.Visible = True
                Me.panConfirmDetailPerprintFormNotice.Visible = False
                Me.btnConfirmDetailAdhocPrintConsentForm.Visible = False

                'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                'Me.rbConfirmDetailPrintClaimConsentFormLanguage.Items.FindByValue("Chi").Text = Me.GetGlobalResourceObject("Text", "Chinese")
                'Me.rbConfirmDetailPrintClaimConsentFormLanguage.Items.FindByValue("Eng").Text = Me.GetGlobalResourceObject("Text", "English")
                'CRE13-019-02 Extend HCVS to China [End][Winnie]

                Me.btnConfirmDetailPrintClaimConsentForm.Text = IIf(strPrintOption = PrintFormOptionValue.PrintConsentOnly, Me.GetGlobalResourceObject("AlternateText", "PrintConsentOnly"), Me.GetGlobalResourceObject("AlternateText", "PrintStatementAndConsent"))

                ' Check the user has printed the document
                Me.btnConfirmDetailConfirm.Visible = IIf(udtEHSTransaction.PrintedConsentForm, True, False)

            End If

        Else
            ' Not Supported Device (I.e. Mobile Device) or the Scheme is Not available for print
            Me.panConfirmDetailPrintClaimConsentForm.Visible = False

            Me.btnConfirmDetailAdhocPrintConsentForm.Visible = False

            Me.btnConfirmDetailConfirm.Visible = True
        End If

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]            
        Me.hfCurrentPrintOption.Value = strPrintOption
        'CRE13-019-02 Extend HCVS to China [End][Winnie]   
    End Sub

    'CRE13-019-02 Extend HCVS to China [Start][Winnie]
    Private Sub PrintClaimConsentFormLanguageSetup(ByVal slConsentFormAvailableLang As String())

        Dim strSelectedLang As String = Me.rbConfirmDetailPrintClaimConsentFormLanguage.SelectedValue

        Me.rbConfirmDetailPrintClaimConsentFormLanguage.Visible = False
        Me.rbConfirmDetailPrintClaimConsentFormLanguage.Items.Clear()

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

                Me.rbConfirmDetailPrintClaimConsentFormLanguage.Items.Add(item)
            Next

            'If only 1 language is available, not display
            If Me.rbConfirmDetailPrintClaimConsentFormLanguage.Items.Count > 1 Then
                Me.rbConfirmDetailPrintClaimConsentFormLanguage.Visible = True
            End If

            'Default the first item
            If Me.rbConfirmDetailPrintClaimConsentFormLanguage.SelectedIndex = -1 Then
                Me.rbConfirmDetailPrintClaimConsentFormLanguage.SelectedIndex = 0
            End If

        End If
    End Sub
    'CRE13-019-02 Extend HCVS to China [End][Winnie]

    Protected Sub ResetEHSTransactionPrintState()
        Dim udtEHSTransaction As EHSTransactionModel = SessionHandler.EHSTransactionGetFromSession(FunctionCode)
        If Not udtEHSTransaction Is Nothing Then
            udtEHSTransaction.PrintedConsentForm = False

            SessionHandler.EHSTransactionSaveToSession(udtEHSTransaction, FunctionCode)
        End If

    End Sub

#End Region

#Region "Select AddHoc Print"

    Protected Sub btnAddHocPrintSelectionBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddHocPrintSelectionBack.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        BackToPreviousView()
    End Sub

    Protected Sub btnAddHocPrintSelection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddHocPrintSelection.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If


        ' check selection
        Dim enumPrintOption As PrintoutOption


        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        'If rbPrintFullChi.Checked Then
        '    enumPrintOption = PrintoutOption.ChineseFull
        'ElseIf rbPrintFull.Checked Then
        '    enumPrintOption = PrintoutOption.EnglishFull
        'ElseIf rbPrintCondencedChi.Checked Then
        '    enumPrintOption = PrintoutOption.ChineseCondensed
        'ElseIf rbPrintCondenced.Checked Then
        '    enumPrintOption = PrintoutOption.EnglishCondensed
        'End If

        Dim strPrintOptionSelectedLang As String = Nothing
        Dim strPrintOptionSelectedVersion As String = Nothing

        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode)

        Dim slConsentFormAvailableLang As String() = Nothing

        slConsentFormAvailableLang = udtSchemeClaim.SubsidizeGroupClaimList(0).ConsentFormAvailableLang

        'CRE15-003 System-generated Form [Start][Philip Chau]
        MyBase.SessionHandler.EHSClaimTempTransactionIDSaveToSession(_udtCommfunct.generateTemporaryTransactionNumber(udtSchemeClaim.SchemeCode.Trim()))
        'CRE15-003 System-generated Form [End][Philip Chau]

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
                    enumPrintOption = PrintoutOption.ChineseCondensed
                ElseIf strPrintOptionSelectedVersion = PrintFormOptionValue.PrintPurposeAndConsent Then
                    enumPrintOption = PrintoutOption.ChineseFull
                End If
            Case PrintOptionLanguage.English
                If strPrintOptionSelectedVersion = PrintFormOptionValue.PrintConsentOnly Then
                    enumPrintOption = PrintoutOption.EnglishCondensed
                ElseIf strPrintOptionSelectedVersion = PrintFormOptionValue.PrintPurposeAndConsent Then
                    enumPrintOption = PrintoutOption.EnglishFull
                End If
            Case PrintOptionLanguage.SimpChinese
                If strPrintOptionSelectedVersion = PrintFormOptionValue.PrintConsentOnly Then
                    enumPrintOption = PrintoutOption.SimpChineseCondensed
                ElseIf strPrintOptionSelectedVersion = PrintFormOptionValue.PrintPurposeAndConsent Then
                    enumPrintOption = PrintoutOption.SimpChineseFull
                End If
        End Select
        'CRE13-019-02 Extend HCVS to China [End][Winnie]

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        'If rbPrintFullChi.Checked OrElse rbPrintFull.Checked OrElse rbPrintCondencedChi.Checked OrElse rbPrintCondenced.Checked Then
        If Not strPrintOptionSelectedLang Is Nothing AndAlso Not strPrintOptionSelectedVersion Is Nothing Then
            'CRE13-019-02 Extend HCVS to China [End][Winnie]
            ' Prompt a popup windows for the printout and return to the confirm claim page
            MyBase.GeneratePrintout(enumPrintOption, True)

            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.ConfirmDetail
        End If

    End Sub

#End Region

#Region "Internal Error"

    Protected Sub btnViewInternalErrorReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewInternalErrorReturn.Click

        BackToSearhAccount()

    End Sub

    Protected Overrides Sub SetupInternalError()
        ' EHSClaim Transaction
        Dim udtEHSTransaction As EHSTransactionModel = SessionHandler.EHSTransactionGetFromSession(FunctionCode)

        If MyBase.IsConcurrentBrowser Then
            'Multiple browser with same session detected, the current action aborted.
            Me.udcMsgBoxInfo.AddMessage(New SystemMessage("990000", "I", "00023"))
            Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Information
            Me.udcMsgBoxInfo.BuildMessageBox()
            Me.udcMsgBoxInfo.Visible = True

            Me.btnViewInternalErrorReturn.Visible = False
            Throw New Exception("Concurrent Browser Detected in Text/EHSClaimV2")
        ElseIf (Not udtEHSTransaction Is Nothing) AndAlso _
                (Not String.IsNullOrEmpty(udtEHSTransaction.TransactionID) OrElse Not udtEHSTransaction.IsNew) Then
            'The current update is aborted since the request has been submit more than once or the record(s) is updated by other. Please verify your action has been complete or not.
            Dim udtSysMsgDuplicateRequest As SystemMessage = New SystemMessage("990000", "I", "00022")
            Me.udcMsgBoxInfo.AddMessage(udtSysMsgDuplicateRequest)
            Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Information
            Me.udcMsgBoxInfo.BuildMessageBox()
            Me.udcMsgBoxInfo.Visible = True
        End If

    End Sub

#End Region

#Region "Confirmation Box"

    ' Event
    Protected Sub btnConfirmBoxBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirmBoxBack.Click
        EHSClaimBasePage.AuditLogTextOnlyVersionConfirmBoxBackClick(_udtAuditLogEntry)

        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ' Clear Confirmation Session
        MyBase.SessionHandler.EHSClaimConfirmMessageGetFromSession(FunctionCode)

        ' Return to the Previous View
        Me.BackToPreviousView()

    End Sub

    Protected Sub btnConfirmBoxConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirmBoxConfirm.Click
        EHSClaimBasePage.AuditLogTextOnlyVersionConfirmBoxConfirmClick(_udtAuditLogEntry)

        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        'Get Previous View
        Dim intViewIndex As Integer = SessionHandler.EHSClaimStepsGetFromSession(FunctionCode)
        Dim objDisplayMessage As Object = SessionHandler.EHSClaimConfirmMessageGetFromSession(FunctionCode)

        Select Case intViewIndex
            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            Case ActiveViewIndex.ServiceDate, ActiveViewIndex.Category, ActiveViewIndex.RCHCode, ActiveViewIndex.MedicalCondition, ActiveViewIndex.VaccinationRecord, ActiveViewIndex.DocumentaryProof, ActiveViewIndex.PlaceVaccination
                'CRE16-002 (Revamp VSS) [End][Chris YIM]

                ' Only Cancel Operation Provided
                ' Return to Search Account
                BackToSearhAccount()

            Case ActiveViewIndex.Vaccine
                Dim udtRuleResultCollection As RuleResultCollection = MyBase.SessionHandler.EligibleResultGetFromSession()
                Dim blnIsConfirmSubmitClaim As Boolean = False

                If Not udtRuleResultCollection Is Nothing Then
                    For Each udtRuleResult As RuleResult In udtRuleResultCollection.Values
                        If udtRuleResult.PromptConfirmed = False Then
                            blnIsConfirmSubmitClaim = True
                            Exit For
                        End If
                    Next
                End If

                If blnIsConfirmSubmitClaim Then
                    ' Handle Confirmation Prompt from Step2a
                    Me.ConfirmVaccineSubmit(True)
                Else
                    ' Handle Confrim Cancel
                    SessionHandler.EHSClaimSessionRemove(Me.FunctionCode)
                    Me.BackToSearhAccount()
                End If

            Case ActiveViewIndex.ConfirmDetail
                ' Confirm: Confirm Claim Detail -> Submit the Claim
                ConfirmClaimSubmit()

        End Select

    End Sub

    ' Helper Method
    Protected Sub ShowConfirmation()
        Dim obj As Object = SessionHandler.EHSClaimConfirmMessageGetFromSession(FunctionCode)
        If obj Is Nothing Then
            Throw New ArgumentNullException("ConfirmationMessage")
        End If

        ShowConfirmation(ConfirmationStyle.NotSet, obj)
    End Sub

    Protected Sub ShowConfirmation(ByVal udtConfirmationStyle As ConfirmationStyle, ByVal objDisplayMessage As Object)


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

        End Select


        ' Set Confirmation Message        
        If TypeOf objDisplayMessage Is String Then
            If objDisplayMessage.Equals("ProvidedInfoTrueClaimSP") Then
                Me.lblConfirmOnlyBoxTitle.Text = Me.GetGlobalResourceObject("Text", "Declaration")
            Else
                Me.lblConfirmOnlyBoxTitle.Text = Me.GetGlobalResourceObject("Text", "ConfirmBoxTitle")
            End If

            Me.lblConfirmBoxMessage.Text = Me.GetGlobalResourceObject("Text", CType(objDisplayMessage, String))

        ElseIf TypeOf objDisplayMessage Is ClaimRuleResult Then
            Me.lblConfirmOnlyBoxTitle.Text = Me.GetGlobalResourceObject("Text", "ConfirmBoxText")

            Me.lblConfirmBoxMessage.Text = Me.Step2aPromptClaimRule(CType(objDisplayMessage, ClaimRuleResult))
        End If


        ' Update Session Index if the value is different
        If Me.mvEHSClaim.ActiveViewIndex <> ActiveViewIndex.ConfirmBox Then
            MyBase.SessionHandler.EHSClaimConfirmMessageSaveToSession(Me.FunctionCode, objDisplayMessage)
            MyBase.SessionHandler.EHSClaimStepsSaveToSession(FunctionCode, Me.mvEHSClaim.ActiveViewIndex)
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.ConfirmBox
        End If

    End Sub

#End Region

#Region "Remark"

    ' Event
    Protected Sub btnViewRemarkBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewRemarkBack.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        BackToPreviousView()

    End Sub

    ' Helper Method
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

                'CRE16-026 (Add PCV13) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                If udtEHSClaimSubsidizeItem.SchemeCode <> SchemeClaimModel.RVP Then
                    ' Subsidize Remark (Regular Text)
                    tr = New TableRow()
                    td = New TableCell()
                    td.CssClass = "tableText"
                    td.Text = IIf(SessionHandler.Language() = CultureLanguage.English, FormatSubsidyRemark(udtEHSClaimSubsidizeItem.Remark), FormatSubsidyRemark(udtEHSClaimSubsidizeItem.RemarkChi))
                    tr.Controls.Add(td)
                    tblRemark.Controls.Add(tr)
                End If
                'CRE16-026 (Add PCV13) [End][Chris YIM]

                ' Subsidize Admount Header (Bold)
                tr = New TableRow()
                td = New TableCell()
                td.CssClass = "tableTitle"
                'CRE16-026 (Add PCV13) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                td.Text = IIf(udtEHSClaimSubsidizeItem.SchemeCode = SchemeClaimModel.RVP, HttpContext.GetGlobalResourceObject("Text", "InjectionCost"), HttpContext.GetGlobalResourceObject("Text", "SubsidyAmount"))
                'CRE16-026 (Add PCV13) [End][Chris YIM]
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

    Private Sub ShowRecipientConditionHelpText()

        Dim strInnerHTML As String = String.Empty
        Dim strRemark() As String = Split(HttpContext.GetGlobalResourceObject("Text", "RecipientConditionPopupContent", New System.Globalization.CultureInfo(CType(SessionHandler.Language, String))), "|")
        If strRemark.Length > 0 Then
            For i As Integer = 0 To strRemark.Length - 1
                strInnerHTML = strInnerHTML + "<li style=""position:relative;left:18px;line-height:20px"">" + strRemark(i) + "</li>"
            Next
        End If

        lblRecipientConditionHeading.Text = HttpContext.GetGlobalResourceObject("Text", "RecipientConditionPopupHeading", New System.Globalization.CultureInfo(CType(SessionHandler.Language, String)))
        divRecipientConditionHelpTitle.InnerHtml = HttpContext.GetGlobalResourceObject("Text", "RecipientConditionPopupTitle", New System.Globalization.CultureInfo(CType(SessionHandler.Language, String)))
        divRecipientConditionHelpContent.InnerHtml = strInnerHTML

        ' Update Session Index if the value is different
        If Me.mvEHSClaim.ActiveViewIndex <> ActiveViewIndex.RecipientConditionHelp Then
            MyBase.SessionHandler.EHSClaimStepsSaveToSession(FunctionCode, Me.mvEHSClaim.ActiveViewIndex)
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.RecipientConditionHelp
        End If

    End Sub

    Protected Sub btnViewRecipientConditionBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewRecipientConditionBack.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        BackToPreviousView()

    End Sub

    Private Sub ShowSubsidizeDisabledRemarkText(ByVal strKey As String)
        Dim strInnerHTML As String = String.Empty

        '1. Heading
        lblSubsidizeDisabledDetailHeading.Text = HttpContext.GetGlobalResourceObject("Text", "NotEligibleDetail", New System.Globalization.CultureInfo(CType(SessionHandler.Language, String)))

        ''2. Title
        'Dim strRemark() As String = Split(strKey, "|")
        'Dim udtSchemeClaim As SchemeClaimModel = Nothing
        'Dim udtSubsidizeGroupClaim As SubsidizeGroupClaimModel = Nothing
        'Dim strSchemeCode As String = String.Empty
        'Dim strSchemeSeq As String = String.Empty
        'Dim strSubsidizeCode As String = String.Empty

        ''Default wordings is "subsidy"
        'Dim strReplace = HttpContext.GetGlobalResourceObject("Text", "Vaccine", New System.Globalization.CultureInfo(CType(Me.SessionHandler.Language, String)))
        'If Me.SessionHandler.Language = Common.Component.CultureLanguage.English Then
        '    strReplace = LCase(strReplace)
        'End If

        'If strRemark.Length > 0 Then
        '    strSchemeCode = strRemark(0)
        '    strSchemeSeq = strRemark(1)
        '    strSubsidizeCode = strRemark(2)

        '    udtSchemeClaim = _udtSchemeClaimBLL.getEffectiveSchemeClaimWithSubsidize(strSchemeCode)

        '    If Not udtSchemeClaim Is Nothing Then
        '        udtSubsidizeGroupClaim = udtSchemeClaim.SubsidizeGroupClaimList.Filter(strSchemeCode, CInt(strSchemeSeq), strSubsidizeCode)
        '    End If

        '    If Not udtSubsidizeGroupClaim Is Nothing Then
        '        Select Case CType(SessionHandler.Language, String)
        '            Case Common.Component.CultureLanguage.English
        '                strReplace = udtSubsidizeGroupClaim.DisplayCodeForClaim
        '            Case Common.Component.CultureLanguage.TradChinese
        '                strReplace = udtSubsidizeGroupClaim.LegendDescForClaimChi
        '            Case Common.Component.CultureLanguage.SimpChinese
        '                strReplace = udtSubsidizeGroupClaim.LegendDescForClaimCN
        '        End Select
        '    End If

        'End If

        'divSubsidizeDisabledDetailTitle.InnerHtml = Replace(HttpContext.GetGlobalResourceObject("Text", "SubsidizeDisabledDetailsTitle", New System.Globalization.CultureInfo(CType(Me.SessionHandler.Language, String))), "%s", strReplace)

        '3. Content
        Dim strContent() As String = Split(HttpContext.GetGlobalResourceObject("Text", strKey, New System.Globalization.CultureInfo(CType(SessionHandler.Language, String))), "|")
        If strContent.Length > 0 Then
            For i As Integer = 0 To strContent.Length - 1
                strInnerHTML = strInnerHTML + "<li style=""position:relative;left:18px;line-height:20px"">" + strContent(i) + "</li>"
            Next
        End If

        divSubsidizeDisabledDetailContent.InnerHtml = strInnerHTML

        ' Update Session Index if the value is different
        If Me.mvEHSClaim.ActiveViewIndex <> ActiveViewIndex.SubsidizeDisabledDetail Then
            MyBase.SessionHandler.EHSClaimStepsSaveToSession(FunctionCode, Me.mvEHSClaim.ActiveViewIndex)
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SubsidizeDisabledDetail
        End If

    End Sub

    Protected Sub btnViewSubsidizeDisabledDetailBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewSubsidizeDisabledDetailBack.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        BackToPreviousView()

    End Sub

#End Region

#Region "View Transaction Details"

    ' Event
    Protected Sub btnViewTranDetailReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewTranDetailReturn.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        Me.BackToPreviousView()

        If Not IsNothing(Session(SESS.ViewBeforeVaccinationRecord)) Then
            Dim intViewIndex As Integer = Session(SESS.ViewBeforeVaccinationRecord)
            MyBase.SessionHandler.EHSClaimStepsSaveToSession(FunctionCode, intViewIndex)
            Session.Remove(SESS.ViewBeforeVaccinationRecord)
        End If

    End Sub

    ' Helper Method
    Protected Sub ShowTransactionDetail()
        Dim udtEHSAccount As EHSAccountModel = SessionHandler.EHSAccountGetFromSession(FunctionCode)
        Dim udtSchemeClaim As SchemeClaimModel = SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
        Dim udtSelectedPracticeDisplay As BLL.PracticeDisplayModel = SessionHandler.PracticeDisplayGetFromSession(FunctionCode)
        Dim udtEHSTransaction As EHSTransactionModel = SessionHandler.EHSTransactionGetFromSession(FunctionCode)
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = SessionHandler.EHSClaimVaccineGetFromSession()
        Dim StaticDataBLL As StaticData.StaticDataBLL = New StaticData.StaticDataBLL()
        Dim strLanguage As String = SessionHandler.Language()
        Dim udtTransactionAdditionalField As TransactionAdditionalFieldModel = Nothing
        Dim udtStaticDataModel As StaticData.StaticDataModel
        Dim udtGeneralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
        Dim strEnableCategory As String = String.Empty

        udtGeneralFunction.getSytemParameterByParameterNameSchemeCode("RVPEnableClaimCategory", strEnableCategory, String.Empty, SchemeClaimModel.RVP)

        If udtEHSAccount Is Nothing Then
            Throw New ArgumentNullException("EHSAccountModel")
        End If

        ' Global Translation
        lblViewTranDetailAcctInfo.Text = Me.GetGlobalResourceObject("Text", "AccountInfo")
        lblViewTranDetailClaimInfo.Text = Me.GetGlobalResourceObject("Text", "ClaimInfo")
        lblViewTranDetailSchemeText.Text = Me.GetGlobalResourceObject("Text", "Scheme")
        lblViewNonClinicSetting.Text = Me.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting")
        lblViewTranDetailPracticeText.Text = Me.GetGlobalResourceObject("Text", "Practice")
        btnViewTranDetailReturn.Text = Me.GetGlobalResourceObject("AlternateText", "ReturnBtn")
        lblViewTranDetailServiceDateText.Text = Me.GetGlobalResourceObject("Text", "ServiceDate")

        Me.lblViewTranDetailCategoryText.Text = Me.GetGlobalResourceObject("Text", "Category")
        Me.lblViewTranDetailPreConditionsText.Text = Me.GetGlobalResourceObject("Text", "PreConditions")
        Me.lblViewTranDetailRCHCodeText.Text = Me.GetGlobalResourceObject("Text", "RCHCode")
        Me.lblViewTranDetailRCHNameText.Text = Me.GetGlobalResourceObject("Text", "RCHName")

        ' --- Setup Page's Info --- 
        ' Doc Info
        Me.udcViewTranDetailOnlyDocumnetType.TextOnlyVersion = True
        ' INT13-0021 - Fix use HKBC on smart IC claim incorrectly [Start][Koala]
        ' -------------------------------------------------------------------------------------
        Me.udcViewTranDetailOnlyDocumnetType.DocumentType = udtEHSAccount.SearchDocCode
        'Me.udcViewTranDetailOnlyDocumnetType.DocumentType = udtEHSAccount.EHSPersonalInformationList(0).DocCode.Trim()  'ToDo: SearchDocCode?
        ' INT13-0021 - Fix use HKBC on smart IC claim incorrectly [End][Koala]
        Me.udcViewTranDetailOnlyDocumnetType.EHSAccount = udtEHSAccount
        Me.udcViewTranDetailOnlyDocumnetType.Vertical = False
        Me.udcViewTranDetailOnlyDocumnetType.MaskIdentityNo = True
        Me.udcViewTranDetailOnlyDocumnetType.ShowAccountRefNo = False
        Me.udcViewTranDetailOnlyDocumnetType.ShowTempAccountNotice = False
        Me.udcViewTranDetailOnlyDocumnetType.ShowAccountCreationDate = False

        Dim udtSmartIDContent As BLL.SmartIDContentModel = MyBase.SessionHandler.SmartIDContentGetFormSession(FunctionCode)

        If Not udtSmartIDContent Is Nothing _
              AndAlso udtSmartIDContent.IsReadSmartID _
              AndAlso Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.ServiceDate _
              AndAlso udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount _
              AndAlso SmartIDShowRealID() Then
            udcViewTranDetailOnlyDocumnetType.IsSmartID = True
        Else
            udcViewTranDetailOnlyDocumnetType.IsSmartID = False
        End If

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        udcViewTranDetailOnlyDocumnetType.SetEnableToShowHKICSymbol = True
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        Me.udcViewTranDetailOnlyDocumnetType.Built()

        ' -----------------------------------------------------------------------------------
        ' Scheme
        ' -----------------------------------------------------------------------------------
        If Not udtSchemeClaim Is Nothing Then
            tblViewTranDetailScheme.Visible = True
            lblViewTranDetailScheme.Text = IIf(strLanguage = CultureLanguage.English, udtSchemeClaim.SchemeDesc, udtSchemeClaim.SchemeDescChi)

            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            If SessionHandler.NonClinicSettingGetFromSession(FunctionCode) Then
                trNonClinicSetting.Style.Add("display", "initial")

                If strLanguage = CultureLanguage.TradChinese Then
                    lblViewNonClinicSetting.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)))
                ElseIf strLanguage = CultureLanguage.SimpChinese Then
                    lblViewNonClinicSetting.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese)))
                Else
                    lblViewNonClinicSetting.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.English)))
                End If
            Else
                trNonClinicSetting.Style.Add("display", "none")
            End If
            'CRE16-002 (Revamp VSS) [End][Chris YIM]
        Else
            tblViewTranDetailScheme.Visible = False
        End If

        ' -----------------------------------------------------------------------------------
        ' Practice
        ' -----------------------------------------------------------------------------------
        If Not udtSelectedPracticeDisplay Is Nothing Then
            tblViewTranDetailPractice.Visible = True
            lblViewTranDetailPractice.Text = String.Format("{0} ({1})", udtSelectedPracticeDisplay.PracticeName, udtSelectedPracticeDisplay.PracticeID)
        Else
            tblViewTranDetailPractice.Visible = False
        End If

        ' -----------------------------------------------------------------------------------
        ' Service Date
        ' -----------------------------------------------------------------------------------
        If Not udtEHSTransaction Is Nothing Then
            tblViewTranDetailServiceDate.Visible = True
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim udtSubPlatformBLL As New SubPlatformBLL
            'lblViewTranDetailServiceDate.Text = Formatter.formatDate(udtEHSTransaction.ServiceDate)
            lblViewTranDetailServiceDate.Text = Formatter.formatDisplayDate(udtEHSTransaction.ServiceDate, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
        Else
            tblViewTranDetailServiceDate.Visible = False
        End If

        ' -----------------------------------------------------------------------------------
        'Category
        ' -----------------------------------------------------------------------------------
        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        If strEnableCategory = "Y" Then
            If Not udtEHSTransaction Is Nothing AndAlso Not udtEHSTransaction.CategoryCode Is Nothing Then
                tblViewTranDetailCategory.Visible = True

                Dim udtClaimCategoryBLL As New ClaimCategoryBLL()

                Dim drClaimCategory As DataRow = udtClaimCategoryBLL.getCategoryDesc(udtEHSTransaction.CategoryCode)

                If Not drClaimCategory Is Nothing Then
                    If strLanguage = Common.Component.CultureLanguage.TradChinese Then
                        lblViewTranDetailCategory.Text = drClaimCategory(ClaimCategoryModel._Category_Name_Chi)
                    Else
                        lblViewTranDetailCategory.Text = drClaimCategory(ClaimCategoryModel._Category_Name)
                    End If
                Else
                    tblViewTranDetailCategory.Visible = False
                End If
            Else
                tblViewTranDetailCategory.Visible = False
            End If
        Else
            tblViewTranDetailCategory.Visible = False
        End If
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

        ' -----------------------------------------------------------------------------------
        ' PreConditions
        ' -----------------------------------------------------------------------------------
        If Not udtEHSTransaction Is Nothing AndAlso Not udtEHSTransaction.TransactionAdditionFields Is Nothing Then
            tblViewTranDetailPreConditions.Visible = True

            udtTransactionAdditionalField = udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID("PreCondition")
            If Not udtTransactionAdditionalField Is Nothing Then

                udtStaticDataModel = StaticDataBLL.GetStaticDataByColumnNameItemNo("PreCondition", udtTransactionAdditionalField.AdditionalFieldValueCode)

                If strLanguage = Common.Component.CultureLanguage.TradChinese Then
                    Me.lblViewTranDetailPreConditions.Text = udtStaticDataModel.DataValueChi.ToString().Trim()
                Else
                    Me.lblViewTranDetailPreConditions.Text = udtStaticDataModel.DataValue.ToString().Trim()
                End If
            Else
                tblViewTranDetailPreConditions.Visible = False
            End If
        Else
            tblViewTranDetailPreConditions.Visible = False
        End If
        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        ' -----------------------------------------------------------------------------------
        ' RCHCode
        ' -----------------------------------------------------------------------------------
        If Not udtEHSTransaction Is Nothing AndAlso Not udtEHSTransaction.TransactionAdditionFields Is Nothing Then
            tblViewTranDetailRCHCode.Visible = True

            Select Case udtSchemeClaim.SchemeCode
                Case SchemeClaimModel.EnumControlType.RVP.ToString.Trim
                    udtTransactionAdditionalField = udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.RCHCode)
                Case SchemeClaimModel.EnumControlType.VSS.ToString.Trim
                    udtTransactionAdditionalField = udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.PIDInstitutionCode)
            End Select

            If Not udtTransactionAdditionalField Is Nothing Then

                Dim udtRVPHomeListBLL As New Common.Component.RVPHomeList.RVPHomeListBLL()
                Dim dtResult As DataTable = udtRVPHomeListBLL.getRVPHomeListActiveByCode(udtTransactionAdditionalField.AdditionalFieldValueCode)

                Select Case udtSchemeClaim.SchemeCode
                    Case SchemeClaimModel.EnumControlType.RVP.ToString.Trim
                        Me.lblViewTranDetailRCHCodeText.Text = GetGlobalResourceObject("Text", "RCHCode")
                        Me.lblViewTranDetailRCHNameText.Text = GetGlobalResourceObject("Text", "RCHName")
                    Case SchemeClaimModel.EnumControlType.VSS.ToString.Trim
                        Me.lblViewTranDetailRCHCodeText.Text = GetGlobalResourceObject("Text", "PIDInstitutionCode")
                        Me.lblViewTranDetailRCHNameText.Text = GetGlobalResourceObject("Text", "PIDInstitutionName")
                End Select

                Me.lblViewTranDetailRCHCode.Text = udtTransactionAdditionalField.AdditionalFieldValueCode
                Me.lblViewTranDetailRCHName.Text = IIf(strLanguage = CultureLanguage.English, dtResult.Rows(0)("Homename_Eng"), dtResult.Rows(0)("Homename_Chi"))
            Else
                tblViewTranDetailRCHCode.Visible = False
            End If
        Else
            tblViewTranDetailRCHCode.Visible = False
        End If
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        ' -----------------------------------------------------------------------------------
        ' Type of Documentary Proof
        ' -----------------------------------------------------------------------------------
        If Not udtEHSTransaction Is Nothing AndAlso Not udtEHSTransaction.TransactionAdditionFields Is Nothing Then
            tblViewTranDetailDocumentaryProof.Visible = True

            udtTransactionAdditionalField = udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.DocumentaryProof)

            udtStaticDataModel = Nothing

            If Not udtTransactionAdditionalField Is Nothing AndAlso udtTransactionAdditionalField.AdditionalFieldValueCode <> String.Empty Then

                Select Case udtSchemeClaim.SchemeCode
                    Case SchemeClaimModel.EnumControlType.PIDVSS.ToString.Trim
                        udtStaticDataModel = StaticDataBLL.GetStaticDataByColumnNameItemNo("PIDVSS_DOCUMENTARYPROOF", udtTransactionAdditionalField.AdditionalFieldValueCode.ToString)
                    Case SchemeClaimModel.EnumControlType.VSS.ToString.Trim
                        Dim udtSelectedCategory As ClaimCategoryModel = SessionHandler.ClaimCategoryGetFromSession(FunctionCode)

                        If Not udtSelectedCategory Is Nothing Then
                            Select Case udtSelectedCategory.CategoryCode
                                Case CategoryCode.VSS_PID
                                    udtStaticDataModel = StaticDataBLL.GetStaticDataByColumnNameItemNo("VSSPID_DOCUMENTARYPROOF", udtTransactionAdditionalField.AdditionalFieldValueCode.ToString)
                                Case CategoryCode.VSS_DA
                                    udtStaticDataModel = StaticDataBLL.GetStaticDataByColumnNameItemNo("VSSDA_DOCUMENTARYPROOF", udtTransactionAdditionalField.AdditionalFieldValueCode.ToString)
                            End Select
                        End If
                End Select

                If Not udtStaticDataModel Is Nothing Then
                    If strLanguage = Common.Component.CultureLanguage.TradChinese Then
                        Me.lblViewTranDetailDocumentaryProof.Text = udtStaticDataModel.DataValueChi.ToString().Trim()
                    Else
                        Me.lblViewTranDetailDocumentaryProof.Text = udtStaticDataModel.DataValue.ToString().Trim()
                    End If

                End If
            Else
                tblViewTranDetailDocumentaryProof.Visible = False
            End If
        Else
            tblViewTranDetailDocumentaryProof.Visible = False
        End If
        'CRE16-002 (Revamp VSS) [End][Chris YIM]


        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        ' -----------------------------------------------------------------------------------
        ' Place of Vaccination
        ' -----------------------------------------------------------------------------------
        If Not udtEHSTransaction Is Nothing AndAlso Not udtEHSTransaction.TransactionAdditionFields Is Nothing Then
            tblViewTranDetailPlaceVaccination.Visible = True

            udtTransactionAdditionalField = udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.PlaceVaccination)

            udtStaticDataModel = Nothing

            If Not udtTransactionAdditionalField Is Nothing Then
                Select Case udtSchemeClaim.SchemeCode
                    Case SchemeClaimModel.EnumControlType.VSS.ToString.Trim
                        udtStaticDataModel = StaticDataBLL.GetStaticDataByColumnNameItemNo("VSS_PLACEOFVACCINATION", udtTransactionAdditionalField.AdditionalFieldValueCode.ToString)

                        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
                        ' --------------------------------------------------------------------------------------
                    Case SchemeClaimModel.EnumControlType.ENHVSSO.ToString.Trim
                        udtStaticDataModel = StaticDataBLL.GetStaticDataByColumnNameItemNo("ENHVSSO_PLACEOFVACCINATION", udtTransactionAdditionalField.AdditionalFieldValueCode.ToString)
                        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

                End Select

                If Not udtStaticDataModel Is Nothing Then
                    If strLanguage = Common.Component.CultureLanguage.TradChinese Then
                        Me.lblViewTranDetailPlaceVaccination.Text = udtStaticDataModel.DataValueChi.ToString().Trim()
                    Else
                        Me.lblViewTranDetailPlaceVaccination.Text = udtStaticDataModel.DataValue.ToString().Trim()
                    End If

                    If udtTransactionAdditionalField.AdditionalFieldValueCode = PlaceOfVaccinationOptions.OTHERS Then
                        lblViewTranDetailPlaceVaccination.Text = lblViewTranDetailPlaceVaccination.Text + " - " + udtTransactionAdditionalField.AdditionalFieldValueDesc
                    End If
                End If
            Else
                tblViewTranDetailPlaceVaccination.Visible = False
            End If
        Else
            tblViewTranDetailPlaceVaccination.Visible = False
        End If
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

        ' -----------------------------------------------------------------------------------
        ' EHS Claim Vaccine
        ' -----------------------------------------------------------------------------------
        If Not udtEHSClaimVaccine Is Nothing AndAlso CheckVaccineSelected(udtEHSClaimVaccine) AndAlso _
            Not udtEHSTransaction Is Nothing AndAlso Not udtSchemeClaim Is Nothing Then

            udcViewTranDetailVaccineReadOnly.Visible = True

            udcViewTranDetailVaccineReadOnly.ShowRemark = False
            udcViewTranDetailVaccineReadOnly.VaccineText = Me.GetGlobalResourceObject("Text", "Vaccine")
            udcViewTranDetailVaccineReadOnly.DoseText = Me.GetGlobalResourceObject("Text", "Dose")
            'CRE16-026 (Add PCV13) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            udcViewTranDetailVaccineReadOnly.AmountText = IIf(udtEHSTransaction.SchemeCode = SchemeClaimModel.RVP, HttpContext.GetGlobalResourceObject("Text", "InjectionCost"), HttpContext.GetGlobalResourceObject("Text", "SubsidyAmount"))
            udcViewTranDetailVaccineReadOnly.RemarksText = Me.GetGlobalResourceObject("Text", "Remarks")
            udcViewTranDetailVaccineReadOnly.TotalAmount = IIf(udtEHSTransaction.SchemeCode = SchemeClaimModel.RVP, HttpContext.GetGlobalResourceObject("Text", "TotalInjectionCost"), HttpContext.GetGlobalResourceObject("Text", "TotalSubsidyAmount"))
            'CRE16-026 (Add PCV13) [End][Chris YIM]
            udcViewTranDetailVaccineReadOnly.NAText = Me.GetGlobalResourceObject("Text", "N/A")

            udcViewTranDetailVaccineReadOnly.Build(udtEHSClaimVaccine)

        Else
            udcViewTranDetailVaccineReadOnly.Visible = False
        End If


        ' Update Session Index if the value is different
        If Me.mvEHSClaim.ActiveViewIndex <> ActiveViewIndex.ViewTranDetail Then
            MyBase.SessionHandler.EHSClaimStepsSaveToSession(FunctionCode, Me.mvEHSClaim.ActiveViewIndex)
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.ViewTranDetail
        End If

    End Sub

#End Region

#Region "Confirm Claim Validation"

    Protected Overrides Function CIVSSValidation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As Common.Component.EHSTransaction.EHSTransactionModel) As Boolean
        Dim systemMessage As SystemMessage
        Dim isValid As Boolean = True
        Dim noVaccineSelected As Boolean = True
        Dim noDoseSelected As Boolean = True
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = MyBase.SessionHandler.EHSClaimVaccineGetFromSession()
        Dim udtGeneralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

        Dim dtmServiceDate As Date = udtEHSTransaction.ServiceDate
        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(Me.FunctionCode)
        Dim udtEHSAccount As EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(Me.FunctionCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtValidator As Validator = New Validator()
        Dim udtSchemeDetailBLL As New Common.Component.SchemeDetails.SchemeDetailBLL()

        Dim strDOB As String
        Dim udtEligibleResult As EligibleResult
        Dim udtClaimRuleResult As ClaimRuleResult

        Dim udtRuleResults As RuleResultCollection = MyBase.SessionHandler.EligibleResultGetFromSession()
        Dim udtRuleResult As RuleResult = Nothing

        Dim strText As String = String.Empty
        Dim strKey As String = String.Empty

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtInputPicker As New InputPickerModel()
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        ' Hide Error Message
        Me.udcVaccineClaimVaccineInputText.SetDoseErrorImage(False)

        If Not checkByConfirmationBox Then

            Me.ClearWarningRules(udtRuleResults)


            ' -----------------------------------------------
            ' UI Input Validation
            '------------------------------------------------
            udtEHSClaimVaccine = Me.udcVaccineClaimVaccineInputText.SetEHSVaccineDoseSelected(udtEHSClaimVaccine)
            isValid = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, Me.udcMsgBoxErr)
            If Not isValid Then
                Me.udcVaccineClaimVaccineInputText.SetDoseErrorImage(True)
            End If

            ' ------------------------------------------------------------------
            ' Check Last Service Date of SubsidizeGroupClaim
            ' ------------------------------------------------------------------
            If isValid Then
                isValid = Me.CheckLastServiceDate(dtmServiceDate, udtSchemeClaim, udtEHSClaimVaccine)
            End If

            '' ------------------------------------------------------------------
            '' Check Dose Period
            '' ------------------------------------------------------------------
            'If isValid Then
            '    For Each udtEHSClaimVaccineSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
            '        If isValid AndAlso udtEHSClaimVaccineSubsidize.Selected Then

            '            For Each udtEHSClaimSubidizeDetailModel As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimVaccineSubsidize.SubsidizeDetailList
            '                If isValid AndAlso udtEHSClaimSubidizeDetailModel.Selected Then
            '                    systemMessage = udtValidator.chkServiceDateSubsidizeDoseLastServiceDate(dtmServiceDate, udtSchemeDetailBLL.getAllSchemeDosePeriod.Filter(udtSchemeClaim.SchemeCode, udtSchemeClaim.SchemeSeq, udtEHSClaimVaccineSubsidize.SubsidizeCode, udtEHSClaimSubidizeDetailModel.AvailableItemCode))
            '                    If Not systemMessage Is Nothing Then
            '                        isValid = False
            '                        Me.udcMsgBoxErr.AddMessage(systemMessage)
            '                    End If
            '                End If
            '            Next
            '        End If
            '    Next
            'End If

            If isValid Then
                ' --------------------------------------------------------------
                ' Check Eligibility:
                ' --------------------------------------------------------------
                If udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DocCode = DocTypeModel.DocTypeCode.EC AndAlso udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                    strDOB = _udtFormatter.formatDOB(udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DOB, udtEHSPersonalInfo.ExactDOB, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ECAge, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ECDateOfRegistration)
                Else
                    strDOB = _udtFormatter.formatDOB(udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DOB, udtEHSPersonalInfo.ExactDOB, Nothing, Nothing)
                End If

                systemMessage = Me._udtEHSClaimBLL.CheckEligibilityForEnterClaim(udtSchemeClaim, dtmServiceDate, udtEHSPersonalInfo, GetVaccinationRecord(udtEHSAccount, udtSchemeClaim.SchemeCode), udtEligibleResult)

                If Not systemMessage Is Nothing Then
                    ' If Check Eligibility Block Show Error
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(systemMessage)
                End If

                ' --------------------------------------------------------------
                ' Check Document Limit:
                ' --------------------------------------------------------------
                If isValid Then
                    systemMessage = Me._udtEHSClaimBLL.CheckExceedDocumentLimitForEnterClaim(udtSchemeClaim.SchemeCode, dtmServiceDate, udtEHSPersonalInfo)
                    If Not systemMessage Is Nothing Then
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage(systemMessage)
                    End If
                End If


                ' --------------------------------------------------------------
                ' Check Claim Rules:
                ' --------------------------------------------------------------
                If isValid Then
                    systemMessage = Me._udtEHSClaimBLL.CheckClaimRuleForEnterClaim(dtmServiceDate, udtEHSAccount, udtEHSPersonalInfo, MyBase.SessionHandler.EHSClaimVaccineGetFromSession(), GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), udtClaimRuleResult, udtInputPicker)
                    If Not systemMessage Is Nothing Then
                        ' If Check Claim Rules Block Show Error
                        isValid = False
                        'CRE15-004 (TIV and QIV) [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                        'Me.udcMsgBoxErr.AddMessage(systemMessage)
                        HandleSystemMessage(systemMessage)
                        Me.udcVaccineClaimVaccineInputText.SetDoseErrorImage(True)
                        'CRE15-004 (TIV and QIV) [End][Chris YIM]
                    End If
                End If
            End If

            If isValid Then
                udtRuleResults = MyBase.SessionHandler.EligibleResultGetFromSession()

                If udtRuleResults Is Nothing Then
                    udtRuleResults = New RuleResultCollection()
                End If

                ' --------------------------------------------------------------
                ' Eligibility Warning / Declaration 
                ' --------------------------------------------------------------
                If udtEligibleResult.IsEligible AndAlso _
                    (udtEligibleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtEligibleResult.HandleMethod = HandleMethodENum.Warning) Then

                    Dim strResourceName As String = String.Empty

                    udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Vaccine, udtEligibleResult.RuleType), udtEligibleResult)

                    If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                        strResourceName = udtEligibleResult.RelatedEligibleRule.ObjectName.Trim()
                    ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                        strResourceName = udtEligibleResult.RelatedEligibleExceptionRule.ObjectName.Trim()
                    ElseIf Not udtEligibleResult.RelatedClaimCategoryEligibilityModel Is Nothing Then
                        strResourceName = udtEligibleResult.RelatedClaimCategoryEligibilityModel.ObjectName.Trim()
                    End If

                    'Set confirm Message
                    Me.ShowConfirmation(ConfirmationStyle.BorderedUnderlineHighlight, strResourceName)
                    isValid = False
                End If

                ' --------------------------------------------------------------
                ' Claim Rules Warning / Declaration
                ' --------------------------------------------------------------
                If Not udtClaimRuleResult Is Nothing AndAlso Not udtClaimRuleResult.IsBlock AndAlso _
                    (udtClaimRuleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtClaimRuleResult.HandleMethod = HandleMethodENum.Warning) Then

                    udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Vaccine, udtClaimRuleResult.RuleType), udtClaimRuleResult)

                    'not Popup prompt defore
                    If strText.Equals(String.Empty) AndAlso isValid Then
                        'Get the prompt message from ClaimRule
                        Me.ShowConfirmation(ConfirmationStyle.Bordered, udtClaimRuleResult)
                    End If

                    isValid = False
                End If

                MyBase.SessionHandler.EligibleResultSaveToSession(udtRuleResults)
            End If
        Else
            strKey = Me.RuleResultKey(ActiveViewIndex.Vaccine, RuleTypeENum.EligibleResult)
            udtRuleResult = udtRuleResults.Item(strKey)

            If Not udtRuleResult Is Nothing Then

                'Should have 2 rule in this collection
                'first : After sreach this account -> rule added and auto make to confirmed
                'Second : After press "Next" in Enter claim detail -> rule added but not confirmed
                If Not udtRuleResult.PromptConfirmed Then
                    udtEHSTransaction.PreSchool = "Y"
                End If
            End If
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            isValid = Me.RemoveRulesAfterConfirm(udtRuleResults, False)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        End If

        Return isValid
    End Function

    Protected Overrides Function EVSSValidation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As Common.Component.EHSTransaction.EHSTransactionModel) As Boolean
        Dim systemMessage As SystemMessage
        Dim isValid As Boolean = True
        Dim noVaccineSelected As Boolean = True
        Dim noDoseSelected As Boolean = True
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = MyBase.SessionHandler.EHSClaimVaccineGetFromSession()
        Dim udtGeneralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

        Dim dtmServiceDate As Date = udtEHSTransaction.ServiceDate
        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(Me.FunctionCode)
        Dim udtEHSAccount As EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(Me.FunctionCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtValidator As Validator = New Validator()
        Dim udtSchemeDetailBLL As New Common.Component.SchemeDetails.SchemeDetailBLL()

        Dim strDOB As String
        Dim udtEligibleResult As EligibleResult
        Dim udtClaimRuleResult As ClaimRuleResult

        Dim udtRuleResults As RuleResultCollection = MyBase.SessionHandler.EligibleResultGetFromSession()
        Dim udtRuleResult As RuleResult = Nothing

        Dim strText As String = String.Empty
        Dim strKey As String = String.Empty

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtInputPicker As New InputPickerModel()
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        ' Hide Error Message
        Me.udcVaccineClaimVaccineInputText.SetDoseErrorImage(False)

        If Not checkByConfirmationBox Then

            Me.ClearWarningRules(udtRuleResults)


            ' -----------------------------------------------
            ' UI Input Validation
            '------------------------------------------------
            udtEHSClaimVaccine = Me.udcVaccineClaimVaccineInputText.SetEHSVaccineDoseSelected(udtEHSClaimVaccine)
            isValid = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, Me.udcMsgBoxErr)
            If Not isValid Then
                Me.udcVaccineClaimVaccineInputText.SetDoseErrorImage(True)
            End If


            ' ------------------------------------------------------------------
            ' Check Last Service Date of SubsidizeGroupClaim
            ' ------------------------------------------------------------------
            If isValid Then
                isValid = Me.CheckLastServiceDate(dtmServiceDate, udtSchemeClaim, udtEHSClaimVaccine)
            End If

            '' ------------------------------------------------------------------
            '' Check Dose Period
            '' ------------------------------------------------------------------
            'If isValid Then
            '    For Each udtEHSClaimVaccineSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
            '        If isValid AndAlso udtEHSClaimVaccineSubsidize.Selected Then

            '            For Each udtEHSClaimSubidizeDetailModel As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimVaccineSubsidize.SubsidizeDetailList
            '                If isValid AndAlso udtEHSClaimSubidizeDetailModel.Selected Then
            '                    systemMessage = udtValidator.chkServiceDateSubsidizeDoseLastServiceDate(dtmServiceDate, udtSchemeDetailBLL.getAllSchemeDosePeriod.Filter(udtSchemeClaim.SchemeCode, udtSchemeClaim.SchemeSeq, udtEHSClaimVaccineSubsidize.SubsidizeCode, udtEHSClaimSubidizeDetailModel.AvailableItemCode))
            '                    If Not systemMessage Is Nothing Then
            '                        isValid = False
            '                        Me.udcMsgBoxErr.AddMessage(systemMessage)
            '                    End If
            '                End If
            '            Next
            '        End If
            '    Next
            'End If

            If isValid Then
                ' --------------------------------------------------------------
                ' Check Eligibility:
                ' --------------------------------------------------------------
                If udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DocCode = DocTypeModel.DocTypeCode.EC AndAlso udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                    strDOB = _udtFormatter.formatDOB(udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DOB, udtEHSPersonalInfo.ExactDOB, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ECAge, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ECDateOfRegistration)
                Else
                    strDOB = _udtFormatter.formatDOB(udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DOB, udtEHSPersonalInfo.ExactDOB, Nothing, Nothing)
                End If

                systemMessage = Me._udtEHSClaimBLL.CheckEligibilityForEnterClaim(udtSchemeClaim, dtmServiceDate, udtEHSPersonalInfo, GetVaccinationRecord(udtEHSAccount, udtSchemeClaim.SchemeCode), udtEligibleResult)

                If Not systemMessage Is Nothing Then
                    ' If Check Eligibility Block Show Error
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(systemMessage)
                End If

                ' --------------------------------------------------------------
                ' Check Document Limit:
                ' --------------------------------------------------------------
                If isValid Then
                    systemMessage = Me._udtEHSClaimBLL.CheckExceedDocumentLimitForEnterClaim(udtSchemeClaim.SchemeCode, dtmServiceDate, udtEHSPersonalInfo)
                    If Not systemMessage Is Nothing Then
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage(systemMessage)
                    End If
                End If


                ' --------------------------------------------------------------
                ' Check Claim Rules:
                ' --------------------------------------------------------------
                If isValid Then
                    systemMessage = Me._udtEHSClaimBLL.CheckClaimRuleForEnterClaim(dtmServiceDate, udtEHSAccount, udtEHSPersonalInfo, MyBase.SessionHandler.EHSClaimVaccineGetFromSession(), GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), udtClaimRuleResult, udtInputPicker)
                    If Not systemMessage Is Nothing Then
                        ' If Check Claim Rules Block Show Error
                        isValid = False
                        'CRE15-004 (TIV and QIV) [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                        'Me.udcMsgBoxErr.AddMessage(systemMessage)
                        HandleSystemMessage(systemMessage)
                        Me.udcVaccineClaimVaccineInputText.SetDoseErrorImage(True)
                        'CRE15-004 (TIV and QIV) [End][Chris YIM]
                    End If
                End If
            End If

            If isValid Then
                udtRuleResults = MyBase.SessionHandler.EligibleResultGetFromSession()

                If udtRuleResults Is Nothing Then
                    udtRuleResults = New RuleResultCollection()
                End If

                ' --------------------------------------------------------------
                ' Eligibility Warning / Declaration 
                ' --------------------------------------------------------------
                If udtEligibleResult.IsEligible AndAlso _
                    (udtEligibleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtEligibleResult.HandleMethod = HandleMethodENum.Warning) Then

                    Dim strResourceName As String = String.Empty

                    udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Vaccine, udtEligibleResult.RuleType), udtEligibleResult)

                    If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                        strResourceName = udtEligibleResult.RelatedEligibleRule.ObjectName.Trim()
                    ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                        strResourceName = udtEligibleResult.RelatedEligibleExceptionRule.ObjectName.Trim()
                    ElseIf Not udtEligibleResult.RelatedClaimCategoryEligibilityModel Is Nothing Then
                        strResourceName = udtEligibleResult.RelatedClaimCategoryEligibilityModel.ObjectName.Trim()
                    End If

                    'Set confirm Message
                    Me.ShowConfirmation(ConfirmationStyle.Bordered, strResourceName)
                    isValid = False
                End If

                ' --------------------------------------------------------------
                ' Claim Rules Warning / Declaration
                ' --------------------------------------------------------------
                If Not udtClaimRuleResult Is Nothing AndAlso Not udtClaimRuleResult.IsBlock AndAlso _
                    (udtClaimRuleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtClaimRuleResult.HandleMethod = HandleMethodENum.Warning) Then

                    udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Vaccine, udtClaimRuleResult.RuleType), udtClaimRuleResult)

                    'not Popup prompt defore
                    If strText.Equals(String.Empty) AndAlso isValid Then
                        'Get the prompt message from ClaimRule
                        Me.ShowConfirmation(ConfirmationStyle.Bordered, udtClaimRuleResult)
                    End If

                    isValid = False
                End If

                MyBase.SessionHandler.EligibleResultSaveToSession(udtRuleResults)
            End If
        Else
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            isValid = Me.RemoveRulesAfterConfirm(udtRuleResults, False)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        End If

        Return isValid
    End Function

    Protected Overrides Function HSIVSSValidation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As Common.Component.EHSTransaction.EHSTransactionModel) As Boolean
        ' ---------------------------------------------
        ' Init
        '----------------------------------------------

        Dim isValid As Boolean = True
        Dim noDoseSelected As Boolean = True
        Dim dtmServiceDate As Date = udtEHSTransaction.ServiceDate
        Dim strDOB As String

        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = MyBase.SessionHandler.EHSClaimVaccineGetFromSession()
        Dim udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel = udtEHSClaimVaccine.SubsidizeList(0)

        Dim udtEHSAccount As EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(Me.FunctionCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(Me.FunctionCode)
        Dim udtClaimCategory As ClaimCategoryModel = MyBase.SessionHandler.ClaimCategoryGetFromSession(Me.FunctionCode)

        ' For Eligible & Claim Rule Warning Checking
        Dim udtEligibleResult As EligibleResult
        Dim udtClaimRuleResult As ClaimRuleResult = Nothing
        Dim udtRuleResults As RuleResultCollection
        Dim strText As String = String.Empty
        Dim strKey As String = String.Empty
        Dim udtRuleResult As RuleResult = Nothing
        Dim udtValidator As Validator = New Validator()
        Dim udtSystemMessage As SystemMessage = Nothing
        Dim udtSchemeDetailBLL As New Common.Component.SchemeDetails.SchemeDetailBLL()

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtInputPicker As New InputPickerModel()
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        'Init Controls
        Me.udcVaccineClaimVaccineInputText.SetDoseErrorImage(False)

        udtRuleResults = MyBase.SessionHandler.EligibleResultGetFromSession()

        If Not checkByConfirmationBox Then

            Me.ClearWarningRules(udtRuleResults)

            ' -----------------------------------------------
            ' UI Input Validation
            '------------------------------------------------
            udtEHSClaimVaccine = Me.udcVaccineClaimVaccineInputText.SetEHSVaccineDoseSelected(udtEHSClaimVaccine)
            isValid = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, Me.udcMsgBoxErr)
            If Not isValid Then
                Me.udcVaccineClaimVaccineInputText.SetDoseErrorImage(True)
            End If

            ' ------------------------------------------------------------------
            ' Check Last Service Date of SubsidizeGroupClaim
            ' ------------------------------------------------------------------
            If isValid Then
                isValid = Me.CheckLastServiceDate(dtmServiceDate, udtSchemeClaim, udtEHSClaimVaccine)
            End If

            '' ------------------------------------------------------------------
            '' Check Dose Period
            '' ------------------------------------------------------------------
            'If isValid Then
            '    For Each udtEHSClaimVaccineSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
            '        If isValid AndAlso udtEHSClaimVaccineSubsidize.Selected Then

            '            For Each udtEHSClaimSubidizeDetailModel As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimVaccineSubsidize.SubsidizeDetailList
            '                If isValid AndAlso udtEHSClaimSubidizeDetailModel.Selected Then
            '                    udtSystemMessage = udtValidator.chkServiceDateSubsidizeDoseLastServiceDate(dtmServiceDate, udtSchemeDetailBLL.getAllSchemeDosePeriod.Filter(udtSchemeClaim.SchemeCode, udtSchemeClaim.SchemeSeq, udtEHSClaimVaccineSubsidize.SubsidizeCode, udtEHSClaimSubidizeDetailModel.AvailableItemCode))
            '                    If Not udtSystemMessage Is Nothing Then
            '                        isValid = False
            '                        Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
            '                    End If
            '                End If
            '            Next
            '        End If
            '    Next
            'End If

            If isValid Then
                ' --------------------------------------------------------------
                ' Check Eligibility:
                ' --------------------------------------------------------------
                If udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DocCode = DocTypeModel.DocTypeCode.EC AndAlso udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                    strDOB = _udtFormatter.formatDOB(udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DOB, udtEHSPersonalInfo.ExactDOB, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ECAge, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ECDateOfRegistration)
                Else
                    strDOB = _udtFormatter.formatDOB(udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DOB, udtEHSPersonalInfo.ExactDOB, Nothing, Nothing)
                End If

                udtSystemMessage = Me._udtEHSClaimBLL.CheckEligibilityForEnterClaim(udtSchemeClaim, dtmServiceDate, udtEHSPersonalInfo, GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), udtEligibleResult)
                If Not udtSystemMessage Is Nothing Then
                    ' If Check Eligibility Block Show Error
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
                End If

                ' --------------------------------------------------------------
                ' Check Document Limit:
                ' --------------------------------------------------------------
                If isValid Then
                    udtSystemMessage = Me._udtEHSClaimBLL.CheckExceedDocumentLimitForEnterClaim(udtSchemeClaim.SchemeCode, dtmServiceDate, udtEHSPersonalInfo)
                    If Not udtSystemMessage Is Nothing Then
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
                    End If
                End If

                ' --------------------------------------------------------------
                ' Check Claim Rules:
                ' --------------------------------------------------------------
                If isValid Then
                    udtSystemMessage = Me._udtEHSClaimBLL.CheckClaimCategoryEligibilityForEnterClaim(MyBase.SessionHandler.EHSClaimVaccineGetFromSession(), dtmServiceDate, udtEHSPersonalInfo, udtClaimCategory.CategoryCode, udtEligibleResult)
                    If Not udtSystemMessage Is Nothing Then
                        ' If Check Claim Rules Block Show Error
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
                    End If
                End If

                If isValid Then
                    udtSystemMessage = Me._udtEHSClaimBLL.CheckClaimRuleForEnterClaim(dtmServiceDate, udtEHSAccount, udtEHSPersonalInfo, MyBase.SessionHandler.EHSClaimVaccineGetFromSession(), GetVaccinationRecord(udtEHSAccount, udtSchemeClaim.SchemeCode), udtClaimRuleResult, udtInputPicker)
                    If Not udtSystemMessage Is Nothing Then
                        ' If Check Claim Rules Block Show Error
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
                    End If
                End If
            End If

            If isValid Then
                udtRuleResults = MyBase.SessionHandler.EligibleResultGetFromSession()

                If udtRuleResults Is Nothing Then
                    udtRuleResults = New RuleResultCollection()
                End If

                ' --------------------------------------------------------------
                ' Eligibility Warning / Declaration (HSIVSS contain no Eligibility Warning)
                ' --------------------------------------------------------------
                If udtEligibleResult.IsEligible AndAlso _
                    (udtEligibleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtEligibleResult.HandleMethod = HandleMethodENum.Warning) Then

                    Dim strResourceName As String = String.Empty

                    udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Vaccine, udtEligibleResult.RuleType), udtEligibleResult)

                    If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                        strResourceName = udtEligibleResult.RelatedEligibleRule.ObjectName.Trim()
                    ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                        strResourceName = udtEligibleResult.RelatedEligibleExceptionRule.ObjectName.Trim()
                    ElseIf Not udtEligibleResult.RelatedClaimCategoryEligibilityModel Is Nothing Then
                        strResourceName = udtEligibleResult.RelatedClaimCategoryEligibilityModel.ObjectName.Trim()
                    End If

                    'Set confirm Message
                    Me.ShowConfirmation(ConfirmationStyle.Bordered, strResourceName)
                    isValid = False
                End If

                ' --------------------------------------------------------------
                ' Claim Rules Warning / Declaration
                ' --------------------------------------------------------------
                If Not udtClaimRuleResult Is Nothing AndAlso Not udtClaimRuleResult.IsBlock AndAlso _
                    (udtClaimRuleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtClaimRuleResult.HandleMethod = HandleMethodENum.Warning) Then

                    udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Vaccine, udtClaimRuleResult.RuleType), udtClaimRuleResult)

                    'not Popup prompt defore
                    If strText.Equals(String.Empty) AndAlso isValid Then
                        'Get the prompt message from ClaimRule
                        Me.ShowConfirmation(ConfirmationStyle.Bordered, udtClaimRuleResult)
                    End If

                    isValid = False
                End If

                MyBase.SessionHandler.EligibleResultSaveToSession(udtRuleResults)
            End If
        Else

            strKey = Me.RuleResultKey(ActiveViewIndex.Vaccine, RuleTypeENum.EligibleResult)
            udtRuleResult = udtRuleResults.Item(strKey)

            If Not udtRuleResult Is Nothing Then

                'Should have 2 rule in this collection
                'first : After sreach this account -> rule added and auto make to confirmed
                'Second : After press "Next" in Enter claim detail -> rule added but not confirmed
                If Not udtRuleResult.PromptConfirmed Then
                    udtEHSTransaction.PreSchool = "Y"
                End If
            End If
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            isValid = Me.RemoveRulesAfterConfirm(udtRuleResults, False)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        End If

        Return isValid
    End Function

    Protected Overrides Function RVPValidation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As Common.Component.EHSTransaction.EHSTransactionModel) As Boolean
        Dim systemMessage As SystemMessage
        Dim isValid As Boolean = True
        Dim noVaccineSelected As Boolean = True
        Dim noDoseSelected As Boolean = True
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = MyBase.SessionHandler.EHSClaimVaccineGetFromSession()
        Dim udtGeneralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

        Dim dtmServiceDate As Date = udtEHSTransaction.ServiceDate
        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(Me.FunctionCode)
        Dim udtEHSAccount As EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(Me.FunctionCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtValidator As Validator = New Validator()
        Dim udtSchemeDetailBLL As New Common.Component.SchemeDetails.SchemeDetailBLL()

        Dim strDOB As String
        Dim udtEligibleResult As EligibleResult
        Dim udtClaimRuleResult As ClaimRuleResult = Nothing
        Dim udtRuleResults As RuleResultCollection
        Dim strText As String = String.Empty
        Dim strKey As String = String.Empty
        Dim strEnableClaimCategory As String = Nothing

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        udtEHSTransaction.HighRisk = HighRisk()
        Dim udtInputPicker As New InputPickerModel()
        udtInputPicker.RCHCode = udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID("RHCCode").AdditionalFieldValueCode.Trim
        udtInputPicker.HighRisk = udtEHSTransaction.HighRisk
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        udtGeneralFunction.getSytemParameterByParameterNameSchemeCode("RVPEnableClaimCategory", strEnableClaimCategory, String.Empty, SchemeClaimModel.RVP)

        Dim udtRuleResult As RuleResult = Nothing
        udtRuleResults = MyBase.SessionHandler.EligibleResultGetFromSession()

        ' Hide Error Message
        Me.udcVaccineClaimVaccineInputText.SetDoseErrorImage(False)

        If Not checkByConfirmationBox Then

            Me.ClearWarningRules(udtRuleResults)

            ' -----------------------------------------------
            ' UI Input Validation
            '------------------------------------------------
            'CRE16-026 (Add PCV13) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'Check High Risk option if turned on 
            If HighRiskOptionShown(udtEHSTransaction, udtEHSClaimVaccine) = SubsidizeGroupClaimModel.HighRiskOptionClass.ShowForInput Then
                'Check Recipient Condition whether it is contained invalid setting
                Dim blnManualInput As Boolean = False
                Dim blnAutoInput As Boolean = False
                For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
                    If udtEHSClaimSubsidize.HighRiskOption = SubsidizeGroupClaimModel.HighRiskOptionClass.ShowForInput Then
                        blnManualInput = True
                    End If
                    If udtEHSClaimSubsidize.HighRiskOption = SubsidizeGroupClaimModel.HighRiskOptionClass.HideButForceHighRisk Then
                        blnAutoInput = True
                    End If
                Next

                If blnManualInput And blnAutoInput Then
                    Throw New Exception("Subsidies are included different High Risk Option [M] and [A] at the same time.")
                End If

                'Check Recipient Condition whether it is nothing to input
                If Me.rblRecipientCondition.Enabled Then
                    isValid = ValidateRecipientCondition(Me.udcMsgBoxErr)
                End If
            End If
            'CRE16-026 (Add PCV13) [End][Chris YIM]

            'Select Vaccine Part
            If isValid Then
                udtEHSClaimVaccine = Me.udcVaccineClaimVaccineInputText.SetEHSVaccineDoseSelected(udtEHSClaimVaccine)
                isValid = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, Me.udcMsgBoxErr)
                If Not isValid Then
                    Me.udcVaccineClaimVaccineInputText.SetDoseErrorImage(True)
                End If
            End If

            ' ------------------------------------------------------------------
            ' Check Last Service Date of SubsidizeGroupClaim
            ' ------------------------------------------------------------------
            If isValid Then
                isValid = Me.CheckLastServiceDate(dtmServiceDate, udtSchemeClaim, udtEHSClaimVaccine)
            End If

            '' ------------------------------------------------------------------
            '' Check Dose Period
            '' ------------------------------------------------------------------
            'If isValid Then
            '    For Each udtEHSClaimVaccineSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
            '        If isValid AndAlso udtEHSClaimVaccineSubsidize.Selected Then

            '            For Each udtEHSClaimSubidizeDetailModel As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimVaccineSubsidize.SubsidizeDetailList
            '                If isValid AndAlso udtEHSClaimSubidizeDetailModel.Selected Then
            '                    systemMessage = udtValidator.chkServiceDateSubsidizeDoseLastServiceDate(dtmServiceDate, udtSchemeDetailBLL.getAllSchemeDosePeriod.Filter(udtSchemeClaim.SchemeCode, udtSchemeClaim.SchemeSeq, udtEHSClaimVaccineSubsidize.SubsidizeCode, udtEHSClaimSubidizeDetailModel.AvailableItemCode))
            '                    If Not systemMessage Is Nothing Then
            '                        isValid = False
            '                        Me.udcMsgBoxErr.AddMessage(systemMessage)
            '                    End If
            '                End If
            '            Next
            '        End If
            '    Next
            'End If

            If isValid Then
                ' --------------------------------------------------------------
                ' Check Eligibility:
                ' --------------------------------------------------------------
                If udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DocCode = DocTypeModel.DocTypeCode.EC AndAlso udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                    strDOB = _udtFormatter.formatDOB(udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DOB, udtEHSPersonalInfo.ExactDOB, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ECAge, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ECDateOfRegistration)
                Else
                    strDOB = _udtFormatter.formatDOB(udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DOB, udtEHSPersonalInfo.ExactDOB, Nothing, Nothing)
                End If

                systemMessage = Me._udtEHSClaimBLL.CheckEligibilityForEnterClaim(udtSchemeClaim, dtmServiceDate, udtEHSPersonalInfo, GetVaccinationRecord(udtEHSAccount, udtSchemeClaim.SchemeCode), udtEligibleResult)

                If Not systemMessage Is Nothing Then
                    ' If Check Eligibility Block Show Error
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(systemMessage)
                End If

                ' --------------------------------------------------------------
                ' Check Document Limit:
                ' --------------------------------------------------------------
                If isValid Then
                    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                    systemMessage = Me._udtEHSClaimBLL.CheckExceedDocumentLimitForEnterClaim(udtSchemeClaim.SchemeCode, dtmServiceDate, udtEHSPersonalInfo)
                    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
                    If Not systemMessage Is Nothing Then
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage(systemMessage)
                    End If
                End If


                ' --------------------------------------------------------------
                ' Check Claim Rules:
                ' --------------------------------------------------------------
                'CRE16-026 (Add PCV13) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                If isValid Then
                    ' CRE14-021 IV for Southern Hemsiphere Vaccine under RVP [Start][Lawrence]
                    systemMessage = Me._udtEHSClaimBLL.CheckClaimRuleForEnterClaim(dtmServiceDate, udtEHSAccount, udtEHSPersonalInfo, MyBase.SessionHandler.EHSClaimVaccineGetFromSession(), GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), udtClaimRuleResult, udtInputPicker)
                    ' CRE14-021 IV for Southern Hemsiphere Vaccine under RVP [End][Lawrence]

                    If Not systemMessage Is Nothing Then
                        ' If Check Claim Rules Block Show Error
                        isValid = False
                        ' CRE15-010-03 (Add TIV under RVP) [Start][Lawrence]
                        HandleSystemMessage(systemMessage)
                        Me.udcVaccineClaimVaccineInputText.SetDoseErrorImage(True)
                        ' CRE15-010-03 (Add TIV under RVP) [End][Lawrence]
                    End If
                End If
                'CRE16-026 (Add PCV13) [End][Chris YIM]
            End If

            If isValid Then
                udtRuleResults = MyBase.SessionHandler.EligibleResultGetFromSession()

                If udtRuleResults Is Nothing Then
                    udtRuleResults = New RuleResultCollection()
                End If

                ' --------------------------------------------------------------
                ' Eligibility Warning / Declaration 
                ' --------------------------------------------------------------
                If udtEligibleResult.IsEligible AndAlso _
                    (udtEligibleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtEligibleResult.HandleMethod = HandleMethodENum.Warning) Then

                    Dim strResourceName As String = String.Empty

                    udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Vaccine, udtEligibleResult.RuleType), udtEligibleResult)

                    If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                        strResourceName = udtEligibleResult.RelatedEligibleRule.ObjectName.Trim()
                    ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                        strResourceName = udtEligibleResult.RelatedEligibleExceptionRule.ObjectName.Trim()
                    ElseIf Not udtEligibleResult.RelatedClaimCategoryEligibilityModel Is Nothing Then
                        strResourceName = udtEligibleResult.RelatedClaimCategoryEligibilityModel.ObjectName.Trim()
                    End If

                    'Set confirm Message
                    Me.ShowConfirmation(ConfirmationStyle.Bordered, strResourceName)
                    isValid = False
                End If

                ' --------------------------------------------------------------
                ' Claim Rules Warning / Declaration
                ' --------------------------------------------------------------
                If Not udtClaimRuleResult Is Nothing AndAlso Not udtClaimRuleResult.IsBlock AndAlso _
                    (udtClaimRuleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtClaimRuleResult.HandleMethod = HandleMethodENum.Warning) Then

                    udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Vaccine, udtClaimRuleResult.RuleType), udtClaimRuleResult)

                    'not Popup prompt defore
                    If strText.Equals(String.Empty) AndAlso isValid Then
                        'Get the prompt message from ClaimRule
                        Me.ShowConfirmation(ConfirmationStyle.Bordered, udtClaimRuleResult)
                    End If

                    isValid = False
                End If

                MyBase.SessionHandler.EligibleResultSaveToSession(udtRuleResults)
            End If
        Else
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            isValid = Me.RemoveRulesAfterConfirm(udtRuleResults, False)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        End If

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Save the High Risk Value
        If isValid Then
            Select Case HighRiskOptionShown(udtEHSTransaction, udtEHSClaimVaccine)
                Case SubsidizeGroupClaimModel.HighRiskOptionClass.ShowForInput 'Manual Input
                    If Me.rblRecipientCondition.Enabled Then
                        udtEHSTransaction.HighRisk = Me.HighRisk()
                    Else
                        udtEHSTransaction.HighRisk = String.Empty
                    End If
                Case SubsidizeGroupClaimModel.HighRiskOptionClass.HideButForceHighRisk 'Auto Input
                    udtEHSTransaction.HighRisk = String.Empty
                Case SubsidizeGroupClaimModel.HighRiskOptionClass.HideWithoutInput 'No Input
                    udtEHSTransaction.HighRisk = String.Empty
                Case Else
                    udtEHSTransaction.HighRisk = String.Empty
            End Select
        End If
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        Return isValid
    End Function

    'CRE15-005-03 (New PIDVSS scheme) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Protected Overrides Function PIDVSSValidation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As Common.Component.EHSTransaction.EHSTransactionModel) As Boolean
        Dim systemMessage As SystemMessage
        Dim isValid As Boolean = True
        Dim noVaccineSelected As Boolean = True
        Dim noDoseSelected As Boolean = True
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = MyBase.SessionHandler.EHSClaimVaccineGetFromSession()
        Dim udtGeneralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

        Dim dtmServiceDate As Date = udtEHSTransaction.ServiceDate
        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(Me.FunctionCode)
        Dim udtEHSAccount As EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(Me.FunctionCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtValidator As Validator = New Validator()
        Dim udtSchemeDetailBLL As New Common.Component.SchemeDetails.SchemeDetailBLL()

        Dim strDOB As String
        Dim udtEligibleResult As EligibleResult
        Dim udtClaimRuleResult As ClaimRuleResult

        Dim udtRuleResults As RuleResultCollection = MyBase.SessionHandler.EligibleResultGetFromSession()
        Dim udtRuleResult As RuleResult = Nothing

        Dim strText As String = String.Empty
        Dim strKey As String = String.Empty

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtInputPicker As New InputPickerModel()
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        ' Hide Error Message
        Me.udcVaccineClaimVaccineInputText.SetDoseErrorImage(False)

        If Not checkByConfirmationBox Then

            Me.ClearWarningRules(udtRuleResults)


            ' -----------------------------------------------
            ' UI Input Validation
            '------------------------------------------------
            udtEHSClaimVaccine = Me.udcVaccineClaimVaccineInputText.SetEHSVaccineDoseSelected(udtEHSClaimVaccine)
            isValid = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, Me.udcMsgBoxErr)
            If Not isValid Then
                Me.udcVaccineClaimVaccineInputText.SetDoseErrorImage(True)
            End If

            ' ------------------------------------------------------------------
            ' Check Last Service Date of SubsidizeGroupClaim
            ' ------------------------------------------------------------------
            If isValid Then
                isValid = Me.CheckLastServiceDate(dtmServiceDate, udtSchemeClaim, udtEHSClaimVaccine)
            End If

            '' ------------------------------------------------------------------
            '' Check Dose Period
            '' ------------------------------------------------------------------
            'If isValid Then
            '    For Each udtEHSClaimVaccineSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
            '        If isValid AndAlso udtEHSClaimVaccineSubsidize.Selected Then

            '            For Each udtEHSClaimSubidizeDetailModel As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimVaccineSubsidize.SubsidizeDetailList
            '                If isValid AndAlso udtEHSClaimSubidizeDetailModel.Selected Then
            '                    systemMessage = udtValidator.chkServiceDateSubsidizeDoseLastServiceDate(dtmServiceDate, udtSchemeDetailBLL.getAllSchemeDosePeriod.Filter(udtSchemeClaim.SchemeCode, udtSchemeClaim.SchemeSeq, udtEHSClaimVaccineSubsidize.SubsidizeCode, udtEHSClaimSubidizeDetailModel.AvailableItemCode))
            '                    If Not systemMessage Is Nothing Then
            '                        isValid = False
            '                        Me.udcMsgBoxErr.AddMessage(systemMessage)
            '                    End If
            '                End If
            '            Next
            '        End If
            '    Next
            'End If

            If isValid Then
                ' --------------------------------------------------------------
                ' Check Eligibility:
                ' --------------------------------------------------------------
                If udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DocCode = DocTypeModel.DocTypeCode.EC AndAlso udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                    strDOB = _udtFormatter.formatDOB(udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DOB, udtEHSPersonalInfo.ExactDOB, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ECAge, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ECDateOfRegistration)
                Else
                    strDOB = _udtFormatter.formatDOB(udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DOB, udtEHSPersonalInfo.ExactDOB, Nothing, Nothing)
                End If

                systemMessage = Me._udtEHSClaimBLL.CheckEligibilityForEnterClaim(udtSchemeClaim, dtmServiceDate, udtEHSPersonalInfo, GetVaccinationRecord(udtEHSAccount, udtSchemeClaim.SchemeCode), udtEligibleResult)

                If Not systemMessage Is Nothing Then
                    ' If Check Eligibility Block Show Error
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(systemMessage)
                End If

                ' --------------------------------------------------------------
                ' Check Document Limit:
                ' --------------------------------------------------------------
                If isValid Then
                    systemMessage = Me._udtEHSClaimBLL.CheckExceedDocumentLimitForEnterClaim(udtSchemeClaim.SchemeCode, dtmServiceDate, udtEHSPersonalInfo)
                    If Not systemMessage Is Nothing Then
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage(systemMessage)
                    End If
                End If


                ' --------------------------------------------------------------
                ' Check Claim Rules:
                ' --------------------------------------------------------------
                If isValid Then
                    systemMessage = Me._udtEHSClaimBLL.CheckClaimRuleForEnterClaim(dtmServiceDate, udtEHSAccount, udtEHSPersonalInfo, MyBase.SessionHandler.EHSClaimVaccineGetFromSession(), GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), udtClaimRuleResult, udtInputPicker)
                    If Not systemMessage Is Nothing Then
                        ' If Check Claim Rules Block Show Error
                        isValid = False
                        'CRE15-004 (TIV and QIV) [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                        'Me.udcMsgBoxErr.AddMessage(systemMessage)
                        HandleSystemMessage(systemMessage)
                        Me.udcVaccineClaimVaccineInputText.SetDoseErrorImage(True)
                        'CRE15-004 (TIV and QIV) [End][Chris YIM]
                    End If
                End If
            End If

            If isValid Then
                udtRuleResults = MyBase.SessionHandler.EligibleResultGetFromSession()

                If udtRuleResults Is Nothing Then
                    udtRuleResults = New RuleResultCollection()
                End If

                ' --------------------------------------------------------------
                ' Eligibility Warning / Declaration 
                ' --------------------------------------------------------------
                If udtEligibleResult.IsEligible AndAlso _
                    (udtEligibleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtEligibleResult.HandleMethod = HandleMethodENum.Warning) Then

                    Dim strResourceName As String = String.Empty

                    udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Vaccine, udtEligibleResult.RuleType), udtEligibleResult)

                    If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                        strResourceName = udtEligibleResult.RelatedEligibleRule.ObjectName.Trim()
                    ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                        strResourceName = udtEligibleResult.RelatedEligibleExceptionRule.ObjectName.Trim()
                    ElseIf Not udtEligibleResult.RelatedClaimCategoryEligibilityModel Is Nothing Then
                        strResourceName = udtEligibleResult.RelatedClaimCategoryEligibilityModel.ObjectName.Trim()
                    End If

                    'Set confirm Message
                    Me.ShowConfirmation(ConfirmationStyle.BorderedUnderlineHighlight, strResourceName)
                    isValid = False
                End If

                ' --------------------------------------------------------------
                ' Claim Rules Warning / Declaration
                ' --------------------------------------------------------------
                If Not udtClaimRuleResult Is Nothing AndAlso Not udtClaimRuleResult.IsBlock AndAlso _
                    (udtClaimRuleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtClaimRuleResult.HandleMethod = HandleMethodENum.Warning) Then

                    udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Vaccine, udtClaimRuleResult.RuleType), udtClaimRuleResult)

                    'not Popup prompt defore
                    If strText.Equals(String.Empty) AndAlso isValid Then
                        'Get the prompt message from ClaimRule
                        Me.ShowConfirmation(ConfirmationStyle.Bordered, udtClaimRuleResult)
                    End If

                    isValid = False
                End If

                MyBase.SessionHandler.EligibleResultSaveToSession(udtRuleResults)
            End If
        Else
            strKey = Me.RuleResultKey(ActiveViewIndex.Vaccine, RuleTypeENum.EligibleResult)
            udtRuleResult = udtRuleResults.Item(strKey)

            If Not udtRuleResult Is Nothing Then

                'Should have 2 rule in this collection
                'first : After sreach this account -> rule added and auto make to confirmed
                'Second : After press "Next" in Enter claim detail -> rule added but not confirmed
                If Not udtRuleResult.PromptConfirmed Then
                    udtEHSTransaction.PreSchool = "Y"
                End If
            End If
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            isValid = Me.RemoveRulesAfterConfirm(udtRuleResults, False)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        End If

        Return isValid
    End Function
    'CRE15-005-03 (New PIDVSS scheme) [End][Chris YIM]

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Protected Overrides Function VSSValidation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As Common.Component.EHSTransaction.EHSTransactionModel) As Boolean
        Dim systemMessage As SystemMessage
        Dim isValid As Boolean = True
        Dim noVaccineSelected As Boolean = True
        Dim noDoseSelected As Boolean = True
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = MyBase.SessionHandler.EHSClaimVaccineGetFromSession()
        Dim udtGeneralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

        Dim dtmServiceDate As Date = udtEHSTransaction.ServiceDate
        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(Me.FunctionCode)
        Dim udtClaimCategory As ClaimCategoryModel = MyBase.SessionHandler.ClaimCategoryGetFromSession(Me.FunctionCode)
        Dim udtEHSAccount As EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(Me.FunctionCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtValidator As Validator = New Validator()
        Dim udtSchemeDetailBLL As New Common.Component.SchemeDetails.SchemeDetailBLL()

        Dim strDOB As String
        Dim udtEligibleResult As EligibleResult
        Dim udtClaimRuleResult As ClaimRuleResult

        Dim udtRuleResults As RuleResultCollection = MyBase.SessionHandler.EligibleResultGetFromSession()
        Dim udtRuleResult As RuleResult = Nothing

        Dim strText As String = String.Empty
        Dim strKey As String = String.Empty

        udtEHSTransaction.HighRisk = HighRisk()
        Dim udtInputPicker As New InputPickerModel()
        udtInputPicker.HighRisk = udtEHSTransaction.HighRisk
        ' CRE20-014 (Gov SIV 2020/21) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        udtInputPicker.SPID = udtEHSTransaction.ServiceProviderID
        ' CRE20-014 (Gov SIV 2020/21) [End][Chris YIM]

        ' Hide Error Message
        Me.udcVaccineClaimVaccineInputText.SetDoseErrorImage(False)

        If Not checkByConfirmationBox Then

            Me.ClearWarningRules(udtRuleResults)

            ' -----------------------------------------------
            ' UI Input Validation
            '------------------------------------------------
            'CRE16-026 (Add PCV13) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'Check High Risk option if turned on 
            If HighRiskOptionShown(udtEHSTransaction, udtEHSClaimVaccine) = SubsidizeGroupClaimModel.HighRiskOptionClass.ShowForInput Then
                'Check Recipient Condition whether it is contained invalid setting
                Dim blnManualInput As Boolean = False
                Dim blnAutoInput As Boolean = False
                For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
                    If udtEHSClaimSubsidize.HighRiskOption = SubsidizeGroupClaimModel.HighRiskOptionClass.ShowForInput Then
                        blnManualInput = True
                    End If
                    If udtEHSClaimSubsidize.HighRiskOption = SubsidizeGroupClaimModel.HighRiskOptionClass.HideButForceHighRisk Then
                        blnAutoInput = True
                    End If
                Next

                If blnManualInput And blnAutoInput Then
                    Throw New Exception("Subsidies are included different High Risk Option [M] and [A] at the same time.")
                End If

                'Check Recipient Condition whether it is nothing to input
                If Me.rblRecipientCondition.Enabled Then
                    isValid = ValidateRecipientCondition(Me.udcMsgBoxErr)
                End If
            End If
            'CRE16-026 (Add PCV13) [End][Chris YIM]

            'Select Vaccine Part
            If isValid Then
                udtEHSClaimVaccine = Me.udcVaccineClaimVaccineInputText.SetEHSVaccineDoseSelected(udtEHSClaimVaccine)
                isValid = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, Me.udcMsgBoxErr)
                If Not isValid Then
                    Me.udcVaccineClaimVaccineInputText.SetDoseErrorImage(True)
                End If
            End If


            ' ------------------------------------------------------------------
            ' Check Last Service Date of SubsidizeGroupClaim
            ' ------------------------------------------------------------------
            If isValid Then
                isValid = Me.CheckLastServiceDate(dtmServiceDate, udtSchemeClaim, udtEHSClaimVaccine)
            End If

            '' ------------------------------------------------------------------
            '' Check Dose Period
            '' ------------------------------------------------------------------
            'If isValid Then
            '    For Each udtEHSClaimVaccineSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
            '        If isValid AndAlso udtEHSClaimVaccineSubsidize.Selected Then

            '            For Each udtEHSClaimSubidizeDetailModel As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimVaccineSubsidize.SubsidizeDetailList
            '                If isValid AndAlso udtEHSClaimSubidizeDetailModel.Selected Then
            '                    systemMessage = udtValidator.chkServiceDateSubsidizeDoseLastServiceDate(dtmServiceDate, udtSchemeDetailBLL.getAllSchemeDosePeriod.Filter(udtSchemeClaim.SchemeCode, udtSchemeClaim.SchemeSeq, udtEHSClaimVaccineSubsidize.SubsidizeCode, udtEHSClaimSubidizeDetailModel.AvailableItemCode))
            '                    If Not systemMessage Is Nothing Then
            '                        isValid = False
            '                        Me.udcMsgBoxErr.AddMessage(systemMessage)
            '                    End If
            '                End If
            '            Next
            '        End If
            '    Next
            'End If

            If isValid Then
                ' --------------------------------------------------------------
                ' Check Eligibility:
                ' --------------------------------------------------------------
                If udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DocCode = DocTypeModel.DocTypeCode.EC AndAlso udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                    strDOB = _udtFormatter.formatDOB(udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DOB, udtEHSPersonalInfo.ExactDOB, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ECAge, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ECDateOfRegistration)
                Else
                    strDOB = _udtFormatter.formatDOB(udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DOB, udtEHSPersonalInfo.ExactDOB, Nothing, Nothing)
                End If

                systemMessage = Me._udtEHSClaimBLL.CheckEligibilityForEnterClaim(udtSchemeClaim, dtmServiceDate, udtEHSPersonalInfo, GetVaccinationRecord(udtEHSAccount, udtSchemeClaim.SchemeCode), udtEligibleResult)

                If Not systemMessage Is Nothing Then
                    ' If Check Eligibility Block Show Error
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(systemMessage)
                End If

                ' --------------------------------------------------------------
                ' Check Document Limit:
                ' --------------------------------------------------------------
                If isValid Then
                    systemMessage = Me._udtEHSClaimBLL.CheckExceedDocumentLimitForEnterClaim(udtSchemeClaim.SchemeCode, dtmServiceDate, udtEHSPersonalInfo)
                    If Not systemMessage Is Nothing Then
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage(systemMessage)
                    End If
                End If


                ' --------------------------------------------------------------
                ' Check Claim Rules:
                ' --------------------------------------------------------------
                If isValid Then
                    systemMessage = Me._udtEHSClaimBLL.CheckClaimRuleForEnterClaim(dtmServiceDate, udtEHSAccount, udtEHSPersonalInfo, MyBase.SessionHandler.EHSClaimVaccineGetFromSession(), GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), udtClaimRuleResult, udtInputPicker)
                    If Not systemMessage Is Nothing Then
                        ' If Check Claim Rules Block Show Error
                        isValid = False
                        'CRE15-004 (TIV and QIV) [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                        'Me.udcMsgBoxErr.AddMessage(systemMessage)
                        HandleSystemMessage(systemMessage)
                        Me.udcVaccineClaimVaccineInputText.SetDoseErrorImage(True)
                        'CRE15-004 (TIV and QIV) [End][Chris YIM]
                    End If
                End If
            End If

            If isValid Then
                udtRuleResults = MyBase.SessionHandler.EligibleResultGetFromSession()

                If udtRuleResults Is Nothing Then
                    udtRuleResults = New RuleResultCollection()
                End If

                ' --------------------------------------------------------------
                ' Eligibility Warning / Declaration 
                ' --------------------------------------------------------------
                If udtEligibleResult.IsEligible AndAlso _
                    (udtEligibleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtEligibleResult.HandleMethod = HandleMethodENum.Warning) Then

                    Dim udtClaimCategorysForCheck As ClaimCategoryModelCollection = New ClaimCategoryBLL().getAllCategoryCache()

                    Dim udtFilterClaimCategoryList As ClaimCategoryModelCollection = udtClaimCategorysForCheck.Filter(udtEligibleResult.SchemeCode, udtEligibleResult.SchemeSeq, udtEligibleResult.SubsidizeCode)

                    If udtFilterClaimCategoryList(0).CategoryCode = udtClaimCategory.CategoryCode Then

                        Dim strResourceName As String = String.Empty

                        udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Vaccine, udtEligibleResult.RuleType), udtEligibleResult)

                        If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                            strResourceName = udtEligibleResult.RelatedEligibleRule.ObjectName.Trim()
                        ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                            strResourceName = udtEligibleResult.RelatedEligibleExceptionRule.ObjectName.Trim()
                        ElseIf Not udtEligibleResult.RelatedClaimCategoryEligibilityModel Is Nothing Then
                            strResourceName = udtEligibleResult.RelatedClaimCategoryEligibilityModel.ObjectName.Trim()
                        End If

                        'Set confirm Message
                        Me.ShowConfirmation(ConfirmationStyle.BorderedUnderlineHighlight, strResourceName)
                        isValid = False
                    End If
                End If

                ' --------------------------------------------------------------
                ' Claim Rules Warning / Declaration
                ' --------------------------------------------------------------
                If Not udtClaimRuleResult Is Nothing AndAlso Not udtClaimRuleResult.IsBlock AndAlso _
                    (udtClaimRuleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtClaimRuleResult.HandleMethod = HandleMethodENum.Warning) Then

                    udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Vaccine, udtClaimRuleResult.RuleType), udtClaimRuleResult)

                    'not Popup prompt defore
                    If strText.Equals(String.Empty) AndAlso isValid Then
                        'Get the prompt message from ClaimRule
                        Me.ShowConfirmation(ConfirmationStyle.Bordered, udtClaimRuleResult)
                    End If

                    isValid = False
                End If

                MyBase.SessionHandler.EligibleResultSaveToSession(udtRuleResults)
            End If
        Else
            strKey = Me.RuleResultKey(ActiveViewIndex.Vaccine, RuleTypeENum.EligibleResult)
            udtRuleResult = udtRuleResults.Item(strKey)

            If Not udtRuleResult Is Nothing Then

                'Should have 2 rule in this collection
                'first : After sreach this account -> rule added and auto make to confirmed
                'Second : After press "Next" in Enter claim detail -> rule added but not confirmed
                If Not udtRuleResult.PromptConfirmed Then
                    udtEHSTransaction.PreSchool = "Y"
                End If
            End If
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            isValid = Me.RemoveRulesAfterConfirm(udtRuleResults, False)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        End If

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Save the High Risk Value
        If isValid Then
            Select Case HighRiskOptionShown(udtEHSTransaction, udtEHSClaimVaccine)
                Case SubsidizeGroupClaimModel.HighRiskOptionClass.ShowForInput 'Manual Input
                    If Me.rblRecipientCondition.Enabled Then
                        udtEHSTransaction.HighRisk = Me.HighRisk()
                    Else
                        udtEHSTransaction.HighRisk = String.Empty
                    End If
                Case SubsidizeGroupClaimModel.HighRiskOptionClass.HideButForceHighRisk 'Auto Input
                    udtEHSTransaction.HighRisk = String.Empty
                Case SubsidizeGroupClaimModel.HighRiskOptionClass.HideWithoutInput 'No Input
                    udtEHSTransaction.HighRisk = String.Empty
                Case Else
                    udtEHSTransaction.HighRisk = String.Empty
            End Select
        End If
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        Return isValid
    End Function
    'CRE16-002 (Revamp VSS) [End][Chris YIM]

    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Protected Overrides Function ENHVSSOValidation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As Common.Component.EHSTransaction.EHSTransactionModel) As Boolean
        Dim systemMessage As SystemMessage
        Dim isValid As Boolean = True
        Dim noVaccineSelected As Boolean = True
        Dim noDoseSelected As Boolean = True
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = MyBase.SessionHandler.EHSClaimVaccineGetFromSession()
        Dim udtGeneralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

        Dim dtmServiceDate As Date = udtEHSTransaction.ServiceDate
        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(Me.FunctionCode)
        Dim udtClaimCategory As ClaimCategoryModel = MyBase.SessionHandler.ClaimCategoryGetFromSession(Me.FunctionCode)
        Dim udtEHSAccount As EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(Me.FunctionCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtValidator As Validator = New Validator()
        Dim udtSchemeDetailBLL As New Common.Component.SchemeDetails.SchemeDetailBLL()

        Dim strDOB As String
        Dim udtEligibleResult As EligibleResult
        Dim udtClaimRuleResult As ClaimRuleResult

        Dim udtRuleResults As RuleResultCollection = MyBase.SessionHandler.EligibleResultGetFromSession()
        Dim udtRuleResult As RuleResult = Nothing

        Dim strText As String = String.Empty
        Dim strKey As String = String.Empty

        Dim udtInputPicker As New InputPickerModel()

        ' Hide Error Message
        Me.udcVaccineClaimVaccineInputText.SetDoseErrorImage(False)

        If Not checkByConfirmationBox Then

            Me.ClearWarningRules(udtRuleResults)

            ' -----------------------------------------------
            ' UI Input Validation
            '------------------------------------------------
            'Select Vaccine Part
            If isValid Then
                udtEHSClaimVaccine = Me.udcVaccineClaimVaccineInputText.SetEHSVaccineDoseSelected(udtEHSClaimVaccine)
                isValid = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, Me.udcMsgBoxErr)

                If Not isValid Then
                    Me.udcVaccineClaimVaccineInputText.SetDoseErrorImage(True)
                End If

            End If

            ' ------------------------------------------------------------------
            ' Check Last Service Date of SubsidizeGroupClaim
            ' ------------------------------------------------------------------
            If isValid Then
                isValid = Me.CheckLastServiceDate(dtmServiceDate, udtSchemeClaim, udtEHSClaimVaccine)
            End If

            If isValid Then
                ' --------------------------------------------------------------
                ' Check Eligibility:
                ' --------------------------------------------------------------
                If udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DocCode = DocTypeModel.DocTypeCode.EC AndAlso udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                    strDOB = _udtFormatter.formatDOB(udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DOB, udtEHSPersonalInfo.ExactDOB, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ECAge, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ECDateOfRegistration)
                Else
                    strDOB = _udtFormatter.formatDOB(udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DOB, udtEHSPersonalInfo.ExactDOB, Nothing, Nothing)
                End If

                systemMessage = Me._udtEHSClaimBLL.CheckEligibilityForEnterClaim(udtSchemeClaim, dtmServiceDate, udtEHSPersonalInfo, GetVaccinationRecord(udtEHSAccount, udtSchemeClaim.SchemeCode), udtEligibleResult)

                If Not systemMessage Is Nothing Then
                    ' If Check Eligibility Block Show Error
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(systemMessage)
                End If

                ' --------------------------------------------------------------
                ' Check Document Limit:
                ' --------------------------------------------------------------
                If isValid Then
                    systemMessage = Me._udtEHSClaimBLL.CheckExceedDocumentLimitForEnterClaim(udtSchemeClaim.SchemeCode, dtmServiceDate, udtEHSPersonalInfo)
                    If Not systemMessage Is Nothing Then
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage(systemMessage)
                    End If
                End If

                ' --------------------------------------------------------------
                ' Check Claim Rules:
                ' --------------------------------------------------------------
                If isValid Then
                    systemMessage = Me._udtEHSClaimBLL.CheckClaimRuleForEnterClaim(dtmServiceDate, udtEHSAccount, udtEHSPersonalInfo, MyBase.SessionHandler.EHSClaimVaccineGetFromSession(), GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), udtClaimRuleResult, udtInputPicker)
                    If Not systemMessage Is Nothing Then
                        ' If Check Claim Rules Block Show Error
                        isValid = False
                        'CRE15-004 (TIV and QIV) [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                        'Me.udcMsgBoxErr.AddMessage(systemMessage)
                        HandleSystemMessage(systemMessage)
                        Me.udcVaccineClaimVaccineInputText.SetDoseErrorImage(True)
                        'CRE15-004 (TIV and QIV) [End][Chris YIM]
                    End If
                End If
            End If

            If isValid Then
                udtRuleResults = MyBase.SessionHandler.EligibleResultGetFromSession()

                If udtRuleResults Is Nothing Then
                    udtRuleResults = New RuleResultCollection()
                End If

                ' --------------------------------------------------------------
                ' Eligibility Warning / Declaration 
                ' --------------------------------------------------------------
                If udtEligibleResult.IsEligible AndAlso _
                    (udtEligibleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtEligibleResult.HandleMethod = HandleMethodENum.Warning) Then

                    Dim udtClaimCategorysForCheck As ClaimCategoryModelCollection = New ClaimCategoryBLL().getAllCategoryCache()

                    Dim udtFilterClaimCategoryList As ClaimCategoryModelCollection = udtClaimCategorysForCheck.Filter(udtEligibleResult.SchemeCode, udtEligibleResult.SchemeSeq, udtEligibleResult.SubsidizeCode)

                    If udtFilterClaimCategoryList(0).CategoryCode = udtClaimCategory.CategoryCode Then

                        Dim strResourceName As String = String.Empty

                        udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Vaccine, udtEligibleResult.RuleType), udtEligibleResult)

                        If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                            strResourceName = udtEligibleResult.RelatedEligibleRule.ObjectName.Trim()
                        ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                            strResourceName = udtEligibleResult.RelatedEligibleExceptionRule.ObjectName.Trim()
                        ElseIf Not udtEligibleResult.RelatedClaimCategoryEligibilityModel Is Nothing Then
                            strResourceName = udtEligibleResult.RelatedClaimCategoryEligibilityModel.ObjectName.Trim()
                        End If

                        'Set confirm Message
                        Me.ShowConfirmation(ConfirmationStyle.BorderedUnderlineHighlight, strResourceName)
                        isValid = False
                    End If
                End If

                ' --------------------------------------------------------------
                ' Claim Rules Warning / Declaration
                ' --------------------------------------------------------------
                If Not udtClaimRuleResult Is Nothing AndAlso Not udtClaimRuleResult.IsBlock AndAlso _
                    (udtClaimRuleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtClaimRuleResult.HandleMethod = HandleMethodENum.Warning) Then

                    udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Vaccine, udtClaimRuleResult.RuleType), udtClaimRuleResult)

                    'not Popup prompt defore
                    If strText.Equals(String.Empty) AndAlso isValid Then
                        'Get the prompt message from ClaimRule
                        Me.ShowConfirmation(ConfirmationStyle.Bordered, udtClaimRuleResult)
                    End If

                    isValid = False
                End If

                MyBase.SessionHandler.EligibleResultSaveToSession(udtRuleResults)
            End If
        Else
            strKey = Me.RuleResultKey(ActiveViewIndex.Vaccine, RuleTypeENum.EligibleResult)
            udtRuleResult = udtRuleResults.Item(strKey)

            If Not udtRuleResult Is Nothing Then

                'Should have 2 rule in this collection
                'first : After sreach this account -> rule added and auto make to confirmed
                'Second : After press "Next" in Enter claim detail -> rule added but not confirmed
                If Not udtRuleResult.PromptConfirmed Then
                    udtEHSTransaction.PreSchool = "Y"
                End If
            End If
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            isValid = Me.RemoveRulesAfterConfirm(udtRuleResults, False)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        End If

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Save the High Risk Value
        udtEHSTransaction.HighRisk = String.Empty
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        Return isValid
    End Function
    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Protected Overrides Function PPPValidation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As Common.Component.EHSTransaction.EHSTransactionModel) As Boolean
        Dim systemMessage As SystemMessage
        Dim isValid As Boolean = True
        Dim noVaccineSelected As Boolean = True
        Dim noDoseSelected As Boolean = True
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = MyBase.SessionHandler.EHSClaimVaccineGetFromSession()
        Dim udtGeneralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

        Dim dtmServiceDate As Date = udtEHSTransaction.ServiceDate
        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(Me.FunctionCode)
        Dim udtClaimCategory As ClaimCategoryModel = MyBase.SessionHandler.ClaimCategoryGetFromSession(Me.FunctionCode)
        Dim udtEHSAccount As EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(Me.FunctionCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtValidator As Validator = New Validator()
        Dim udtSchemeDetailBLL As New Common.Component.SchemeDetails.SchemeDetailBLL()

        Dim strDOB As String
        Dim udtEligibleResult As EligibleResult
        Dim udtClaimRuleResult As ClaimRuleResult

        Dim udtRuleResults As RuleResultCollection = MyBase.SessionHandler.EligibleResultGetFromSession()
        Dim udtRuleResult As RuleResult = Nothing

        Dim strText As String = String.Empty
        Dim strKey As String = String.Empty

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        udtEHSTransaction.HighRisk = HighRisk()
        Dim udtInputPicker As New InputPickerModel()
        udtInputPicker.HighRisk = udtEHSTransaction.HighRisk
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        ' Hide Error Message
        Me.udcVaccineClaimVaccineInputText.SetDoseErrorImage(False)

        If Not checkByConfirmationBox Then

            Me.ClearWarningRules(udtRuleResults)

            ' -----------------------------------------------
            ' UI Input Validation
            '------------------------------------------------
            'CRE16-026 (Add PCV13) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'Check High Risk option if turned on 
            If HighRiskOptionShown(udtEHSTransaction, udtEHSClaimVaccine) = SubsidizeGroupClaimModel.HighRiskOptionClass.ShowForInput Then
                'Check Recipient Condition whether it is contained invalid setting
                Dim blnManualInput As Boolean = False
                Dim blnAutoInput As Boolean = False
                For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
                    If udtEHSClaimSubsidize.HighRiskOption = SubsidizeGroupClaimModel.HighRiskOptionClass.ShowForInput Then
                        blnManualInput = True
                    End If
                    If udtEHSClaimSubsidize.HighRiskOption = SubsidizeGroupClaimModel.HighRiskOptionClass.HideButForceHighRisk Then
                        blnAutoInput = True
                    End If
                Next

                If blnManualInput And blnAutoInput Then
                    Throw New Exception("Subsidies are included different High Risk Option [M] and [A] at the same time.")
                End If

                'Check Recipient Condition whether it is nothing to input
                If Me.rblRecipientCondition.Enabled Then
                    isValid = ValidateRecipientCondition(Me.udcMsgBoxErr)
                End If
            End If
            'CRE16-026 (Add PCV13) [End][Chris YIM]

            'Select Vaccine Part
            If isValid Then
                udtEHSClaimVaccine = Me.udcVaccineClaimVaccineInputText.SetEHSVaccineDoseSelected(udtEHSClaimVaccine)
                isValid = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, Me.udcMsgBoxErr)
                If Not isValid Then
                    Me.udcVaccineClaimVaccineInputText.SetDoseErrorImage(True)
                End If
            End If


            ' ------------------------------------------------------------------
            ' Check Last Service Date of SubsidizeGroupClaim
            ' ------------------------------------------------------------------
            If isValid Then
                isValid = Me.CheckLastServiceDate(dtmServiceDate, udtSchemeClaim, udtEHSClaimVaccine)
            End If

            '' ------------------------------------------------------------------
            '' Check Dose Period
            '' ------------------------------------------------------------------
            'If isValid Then
            '    For Each udtEHSClaimVaccineSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
            '        If isValid AndAlso udtEHSClaimVaccineSubsidize.Selected Then

            '            For Each udtEHSClaimSubidizeDetailModel As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimVaccineSubsidize.SubsidizeDetailList
            '                If isValid AndAlso udtEHSClaimSubidizeDetailModel.Selected Then
            '                    systemMessage = udtValidator.chkServiceDateSubsidizeDoseLastServiceDate(dtmServiceDate, udtSchemeDetailBLL.getAllSchemeDosePeriod.Filter(udtSchemeClaim.SchemeCode, udtSchemeClaim.SchemeSeq, udtEHSClaimVaccineSubsidize.SubsidizeCode, udtEHSClaimSubidizeDetailModel.AvailableItemCode))
            '                    If Not systemMessage Is Nothing Then
            '                        isValid = False
            '                        Me.udcMsgBoxErr.AddMessage(systemMessage)
            '                    End If
            '                End If
            '            Next
            '        End If
            '    Next
            'End If

            If isValid Then
                ' --------------------------------------------------------------
                ' Check Eligibility:
                ' --------------------------------------------------------------
                If udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DocCode = DocTypeModel.DocTypeCode.EC AndAlso udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                    strDOB = _udtFormatter.formatDOB(udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DOB, udtEHSPersonalInfo.ExactDOB, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ECAge, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ECDateOfRegistration)
                Else
                    strDOB = _udtFormatter.formatDOB(udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DOB, udtEHSPersonalInfo.ExactDOB, Nothing, Nothing)
                End If

                systemMessage = Me._udtEHSClaimBLL.CheckEligibilityForEnterClaim(udtSchemeClaim, dtmServiceDate, udtEHSPersonalInfo, GetVaccinationRecord(udtEHSAccount, udtSchemeClaim.SchemeCode), udtEligibleResult)

                If Not systemMessage Is Nothing Then
                    ' If Check Eligibility Block Show Error
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(systemMessage)
                End If

                ' --------------------------------------------------------------
                ' Check Document Limit:
                ' --------------------------------------------------------------
                If isValid Then
                    systemMessage = Me._udtEHSClaimBLL.CheckExceedDocumentLimitForEnterClaim(udtSchemeClaim.SchemeCode, dtmServiceDate, udtEHSPersonalInfo)
                    If Not systemMessage Is Nothing Then
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage(systemMessage)
                    End If
                End If


                ' --------------------------------------------------------------
                ' Check Claim Rules:
                ' --------------------------------------------------------------
                If isValid Then
                    systemMessage = Me._udtEHSClaimBLL.CheckClaimRuleForEnterClaim(dtmServiceDate, udtEHSAccount, udtEHSPersonalInfo, MyBase.SessionHandler.EHSClaimVaccineGetFromSession(), GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), udtClaimRuleResult, udtInputPicker)
                    If Not systemMessage Is Nothing Then
                        ' If Check Claim Rules Block Show Error
                        isValid = False
                        'CRE15-004 (TIV and QIV) [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                        'Me.udcMsgBoxErr.AddMessage(systemMessage)
                        HandleSystemMessage(systemMessage)
                        Me.udcVaccineClaimVaccineInputText.SetDoseErrorImage(True)
                        'CRE15-004 (TIV and QIV) [End][Chris YIM]
                    End If
                End If
            End If

            If isValid Then
                udtRuleResults = MyBase.SessionHandler.EligibleResultGetFromSession()

                If udtRuleResults Is Nothing Then
                    udtRuleResults = New RuleResultCollection()
                End If

                ' --------------------------------------------------------------
                ' Eligibility Warning / Declaration 
                ' --------------------------------------------------------------
                If udtEligibleResult.IsEligible AndAlso _
                    (udtEligibleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtEligibleResult.HandleMethod = HandleMethodENum.Warning) Then

                    Dim udtClaimCategorysForCheck As ClaimCategoryModelCollection = New ClaimCategoryBLL().getAllCategoryCache()

                    Dim udtFilterClaimCategoryList As ClaimCategoryModelCollection = udtClaimCategorysForCheck.Filter(udtEligibleResult.SchemeCode, udtEligibleResult.SchemeSeq, udtEligibleResult.SubsidizeCode)

                    If udtFilterClaimCategoryList(0).CategoryCode = udtClaimCategory.CategoryCode Then

                        Dim strResourceName As String = String.Empty

                        udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Vaccine, udtEligibleResult.RuleType), udtEligibleResult)

                        If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                            strResourceName = udtEligibleResult.RelatedEligibleRule.ObjectName.Trim()
                        ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                            strResourceName = udtEligibleResult.RelatedEligibleExceptionRule.ObjectName.Trim()
                        ElseIf Not udtEligibleResult.RelatedClaimCategoryEligibilityModel Is Nothing Then
                            strResourceName = udtEligibleResult.RelatedClaimCategoryEligibilityModel.ObjectName.Trim()
                        End If

                        'Set confirm Message
                        Me.ShowConfirmation(ConfirmationStyle.BorderedUnderlineHighlight, strResourceName)
                        isValid = False
                    End If
                End If

                ' --------------------------------------------------------------
                ' Claim Rules Warning / Declaration
                ' --------------------------------------------------------------
                If Not udtClaimRuleResult Is Nothing AndAlso Not udtClaimRuleResult.IsBlock AndAlso _
                    (udtClaimRuleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtClaimRuleResult.HandleMethod = HandleMethodENum.Warning) Then

                    udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Vaccine, udtClaimRuleResult.RuleType), udtClaimRuleResult)

                    'not Popup prompt defore
                    If strText.Equals(String.Empty) AndAlso isValid Then
                        'Get the prompt message from ClaimRule
                        Me.ShowConfirmation(ConfirmationStyle.Bordered, udtClaimRuleResult)
                    End If

                    isValid = False
                End If

                MyBase.SessionHandler.EligibleResultSaveToSession(udtRuleResults)
            End If
        Else
            strKey = Me.RuleResultKey(ActiveViewIndex.Vaccine, RuleTypeENum.EligibleResult)
            udtRuleResult = udtRuleResults.Item(strKey)

            If Not udtRuleResult Is Nothing Then

                'Should have 2 rule in this collection
                'first : After sreach this account -> rule added and auto make to confirmed
                'Second : After press "Next" in Enter claim detail -> rule added but not confirmed
                If Not udtRuleResult.PromptConfirmed Then
                    udtEHSTransaction.PreSchool = "Y"
                End If
            End If
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            isValid = Me.RemoveRulesAfterConfirm(udtRuleResults, False)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        End If

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Save the High Risk Value
        If isValid Then
            Select Case HighRiskOptionShown(udtEHSTransaction, udtEHSClaimVaccine)
                Case SubsidizeGroupClaimModel.HighRiskOptionClass.ShowForInput 'Manual Input
                    If Me.rblRecipientCondition.Enabled Then
                        udtEHSTransaction.HighRisk = Me.HighRisk()
                    Else
                        udtEHSTransaction.HighRisk = String.Empty
                    End If
                Case SubsidizeGroupClaimModel.HighRiskOptionClass.HideButForceHighRisk 'Auto Input
                    udtEHSTransaction.HighRisk = String.Empty
                Case SubsidizeGroupClaimModel.HighRiskOptionClass.HideWithoutInput 'No Input
                    udtEHSTransaction.HighRisk = String.Empty
                Case Else
                    udtEHSTransaction.HighRisk = String.Empty
            End Select
        End If
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        Return isValid
    End Function
    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

#End Region

#Region "step2b Confirm Details"

    ' Event
    Protected Sub btnConfirmDetailBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirmDetailBack.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearConfirmDetailError()

        ' Reset Printing Status when back to enter detail
        ResetEHSTransactionPrintState()

        ' Always back to Vaccine page
        Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Vaccine
    End Sub

    Protected Sub btnConfirmDetailConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirmDetailConfirm.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearConfirmDetailError()

        ' Check Concurrent Browser
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        ' CRE13-001 - EHAPP [Start][Koala]
        ' -------------------------------------------------------------------------------------
        Dim udtEHSTransaction As EHSTransactionModel = MyBase.SessionHandler.EHSTransactionGetFromSession(FunctionCode)
        Dim udtSchemeClaim As SchemeClaimModel = (New SchemeClaimBLL).getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSTransaction.SchemeCode, udtEHSTransaction.ServiceDate.Date.AddDays(1).AddMinutes(-1))
        MyBase.SessionHandler.SchemeSelectedSaveToSession(udtSchemeClaim, FunctionCode)
        ' CRE13-001 - EHAPP [End][Koala]

        Dim blnIsValid As Boolean = ValidateConfirmClaimDetail()

        ' Check the input is correct. if correct, go to confrim declaration page
        If IsValid Then
            ShowConfirmation(ConfirmationStyle.BorderedUnderlineHighlight, "ProvidedInfoTrueClaimSP")

        ElseIf Me.udcMsgBoxErr.GetCodeTable().Rows.Count > 0 Then
            Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail)

        Else
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.InternalError

        End If

    End Sub

    Protected Sub btnConfirmDetailPrintClaimConsentForm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirmDetailPrintClaimConsentForm.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        Dim strPrintOptionSelectedValue As String = Me.rbConfirmDetailPrintClaimConsentFormLanguage.SelectedValue



        'CRE15-003 System-generated Form [Start][Philip Chau]
        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode())
        MyBase.SessionHandler.EHSClaimTempTransactionIDSaveToSession(_udtCommfunct.generateTemporaryTransactionNumber(udtSchemeClaim.SchemeCode.Trim()))
        'CRE15-003 System-generated Form [End][Philip Chau]

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        'Select Case strPrintOptionSelectedValue
        '    Case "Chi"
        '        MyBase.GeneratePrintout(CultureLanguage.TradChinese)
        '    Case "Eng"
        '        MyBase.GeneratePrintout(CultureLanguage.English)
        'End Select

        Dim strCurrentPrintOption As String = Me.hfCurrentPrintOption.Value
        MyBase.GeneratePrintout(strPrintOptionSelectedValue, strCurrentPrintOption)
        'CRE13-019-02 Extend HCVS to China [End][Winnie]

        ' Refresh Printout Control
        RefreshConfirmDetailPrintOptionControl()

    End Sub

    Protected Sub btnConfirmDetailChangePrintOption_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirmDetailChangePrintOption.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearConfirmDetailError()

        ' Get the current user's print option
        Dim strCurrentPrintOption As String = MyBase.GetCurrentUserPrintOption()
        Me.udtPrintOptionSelection.setTitle(Me.GetGlobalResourceObject("Text", "SelectPrintFormOption"))
        Me.udtPrintOptionSelection.setSelectedValue(strCurrentPrintOption)

        ' Audit Log
        EHSClaimBasePage.AuditLogSelectPrintOption(_udtAuditLogEntry, strCurrentPrintOption)

        ' Go to Change Print option page
        GoToView(ActiveViewIndex.PrintOption)

    End Sub

    Protected Sub btnConfirmDetailAdhocPrintConsentForm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirmDetailAdhocPrintConsentForm.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearConfirmDetailError()

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        ' Default Select Chinese Condensed Version
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
        GoToView(ActiveViewIndex.AddHocPrint)

    End Sub

    Private Sub udcConfirmDetailReadOnlyEHSClaim_VaccineRemarkClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles udcConfirmDetailReadOnlyEHSClaim.VaccineRemarkClicked
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearConfirmDetailError()

        ' Activate Remark Page and Show Remark
        ShowVaccineRemarkText()
    End Sub

    ' Helper Method
    Protected Overrides Sub SetupStepConfirmClaim(ByVal udtEHSAccount As Common.Component.EHSAccount.EHSAccountModel, ByVal blnInitAll As Boolean)
        MyBase.SetupStepConfirmClaim(udtEHSAccount, blnInitAll)

        Dim udtSchemeClaim As SchemeClaimModel = SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
        Dim udtSelectedPracticeDisplay As BLL.PracticeDisplayModel = SessionHandler.PracticeDisplayGetFromSession(FunctionCode)
        Dim udtEHSTransaction As EHSTransactionModel = SessionHandler.EHSTransactionGetFromSession(FunctionCode)
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = SessionHandler.EHSClaimVaccineGetFromSession()
        Dim strLanguage As String = SessionHandler.Language()

        panConfirmDetailRecipientCondition.Visible = False


        'CRE20-009 session to handle the type of doucmentary proof  [Start][Nichole]
        SessionHandler.EHSDocProofSaveToSession(FunctionCode, rbDocumentaryProof.SelectedValue)
        'CRE20-009 session to handle the type of doucmentary proof  [end][Nichole]

        If udtEHSAccount Is Nothing Then
            Throw New ArgumentNullException("EHSAccountModel")
        End If

        ' Global Translation
        lblConfirmDetailAcctInfo.Text = Me.GetGlobalResourceObject("Text", "AccountInfo")
        lblConfirmDetailClaimInfo.Text = Me.GetGlobalResourceObject("Text", "ConfirmClaimInformation")
        lblConfirmDetailSchemeText.Text = Me.GetGlobalResourceObject("Text", "Scheme")
        lblConfirmDetailServiceDateText.Text = Me.GetGlobalResourceObject("Text", "ServiceDate")
        lblConfirmDetailPracticeText.Text = Me.GetGlobalResourceObject("Text", "Practice")
        lblConfirmDetailBankAcctText.Text = Me.GetGlobalResourceObject("Text", "BankAccountNo")
        lblConfirmDetailServiceTypeText.Text = Me.GetGlobalResourceObject("Text", "ServiceType")
        btnConfirmDetailBack.Text = Me.GetGlobalResourceObject("AlternateText", "BackBtn")
        btnConfirmDetailConfirm.Text = Me.GetGlobalResourceObject("AlternateText", "ConfirmBtn")
        btnConfirmDetailAdhocPrintConsentForm.Text = Me.GetGlobalResourceObject("AlternateText", "VRAPrintClaimConsentFormBtn")
        lblConfirmDetailRecipientConditionText.Text = Me.GetGlobalResourceObject("Text", "RecipientCondition")

        ' --- Setup Page's Info --- 
        ' Doc Info
        Me.udcConfirmDetailReadOnlyDocumnetType.TextOnlyVersion = True
        ' INT13-0021 - Fix use HKBC on smart IC claim incorrectly [Start][Koala]
        ' -------------------------------------------------------------------------------------
        Me.udcConfirmDetailReadOnlyDocumnetType.DocumentType = udtEHSAccount.SearchDocCode
        'Me.udcConfirmDetailReadOnlyDocumnetType.DocumentType = udtEHSAccount.EHSPersonalInformationList(0).DocCode.Trim()  'ToDo: SearchDocCode?
        ' INT13-0021 - Fix use HKBC on smart IC claim incorrectly [End][Koala]
        Me.udcConfirmDetailReadOnlyDocumnetType.EHSAccount = udtEHSAccount
        Me.udcConfirmDetailReadOnlyDocumnetType.Vertical = False
        Me.udcConfirmDetailReadOnlyDocumnetType.MaskIdentityNo = True
        Me.udcConfirmDetailReadOnlyDocumnetType.ShowAccountRefNo = False
        Me.udcConfirmDetailReadOnlyDocumnetType.ShowTempAccountNotice = False
        Me.udcConfirmDetailReadOnlyDocumnetType.ShowAccountCreationDate = False
        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Me.udcConfirmDetailReadOnlyDocumnetType.SetEnableToShowHKICSymbol = True
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]
        Me.udcConfirmDetailReadOnlyDocumnetType.Built()

        ' Scheme
        lblConfirmDetailScheme.Text = IIf(strLanguage = CultureLanguage.English, udtSchemeClaim.SchemeDesc, udtSchemeClaim.SchemeDescChi)

        ' Non Clinic Setting
        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtTransactionAdditionField As TransactionAdditionalFieldModel = udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ClinicType)
        If Not udtTransactionAdditionField Is Nothing Then
            If udtTransactionAdditionField.AdditionalFieldValueCode = ClinicType.NonClinic Then
                Me.trConfirmNonClinicSetting.Style.Add("display", "initial")
                Me.lblConfirmNonClinicSetting.Text = String.Format("({0})", GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting"))
                'If strLanguage = CultureLanguage.TradChinese Then

                'Else

                'End If
            Else
                Me.trConfirmNonClinicSetting.Style.Add("display", "none")
            End If
        End If
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

        ' Service Date
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtSubPlatformBLL As New SubPlatformBLL
        'lblConfirmDetailServiceDate.Text = Formatter.formatDate(udtEHSTransaction.ServiceDate)
        lblConfirmDetailServiceDate.Text = Formatter.formatDisplayDate(udtEHSTransaction.ServiceDate, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]


        ' Practice
        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        ' Fix practice english name display font
        If strLanguage = Common.Component.CultureLanguage.TradChinese Then
            Me.lblConfirmDetailPractice.CssClass = "tableTextChi"
        Else
            Me.lblConfirmDetailPractice.CssClass = "tableText"
        End If
        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]
        lblConfirmDetailPractice.Text = String.Format("{0} ({1})", udtSelectedPracticeDisplay.PracticeName, udtSelectedPracticeDisplay.PracticeID)

        ' Bank Account
        lblConfirmDetailBankAcct.Text = Formatter.maskBankAccount(udtEHSTransaction.BankAccountNo)

        ' Service Type
        lblConfirmDetailServiceType.Text = IIf(strLanguage = CultureLanguage.English, udtEHSTransaction.ServiceTypeDesc, udtEHSTransaction.ServiceTypeDesc_Chi)

        ' EHS Claim Vaccine
        Me.udcConfirmDetailReadOnlyEHSClaim.TextOnlyVersion = True
        Me.udcConfirmDetailReadOnlyEHSClaim.EHSClaimVaccine = udtEHSClaimVaccine
        Me.udcConfirmDetailReadOnlyEHSClaim.EHSTransaction = udtEHSTransaction
        Me.udcConfirmDetailReadOnlyEHSClaim.SchemeCode = udtSchemeClaim.SchemeCode
        Me.udcConfirmDetailReadOnlyEHSClaim.Mode = ucReadOnlyEHSClaim.ReadOnlyEHSClaimMode.Normal
        Me.udcConfirmDetailReadOnlyEHSClaim.Built()

        ' Recipient Condition
        setupDisplayRecipientCondition(udtEHSTransaction, panConfirmDetailRecipientCondition, lblConfirmDetailRecipientCondition)

        ' Update Printout Control
        RefreshConfirmDetailPrintOptionControl()
    End Sub

    Private Function ValidateConfirmClaimDetail() As Boolean
        Dim udtEHSTransaction As EHSTransactionModel = MyBase.SessionHandler.EHSTransactionGetFromSession(FunctionCode)
        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode)

        Dim udtEHSAccount As EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(FunctionCode)
        Dim isValid As Boolean = True

        If udtEHSTransaction Is Nothing Then
            Throw New ArgumentNullException("EHSTransactionModel")
        End If
        If udtSchemeClaim Is Nothing Then
            Throw New ArgumentNullException("SchemeClaimModel")
        End If
        If udtEHSAccount Is Nothing Then
            Throw New ArgumentNullException("EHSAccountModel")
        End If

        ' Check Concurrent Browser
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
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
                Dim udtVoucherinfo As New VoucherInfo.VoucherInfoModel(VoucherInfo.VoucherInfoModel.AvailableVoucher.Include, _
                                                                       VoucherInfo.VoucherInfoModel.AvailableQuota.None)

                udtVoucherinfo.GetInfo(MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode), udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode))

                If udtEHSAccount.VoucherInfo.GetAvailableVoucher() <> udtVoucherinfo.GetAvailableVoucher() Then
                    ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]
                    isValid = False
                    Me._blnConcurrentUpdate = True
                End If
            End If
        End If

        ' INT13-0022 - Fix some special handling on HCSP text only version [Start][Koala]
        ' -------------------------------------------------------------------------------------
        ' Check the browser is from Mobile device
        ' if the first subsidize need print form -> this scheme need to print form 
        If Not IsPrePrintDocument() AndAlso udtSchemeClaim.SubsidizeGroupClaimList(0).ConsentFormAvailable Then
            ' check the printform have been printed
            If udtEHSTransaction.PrintedConsentForm = False Then
                isValid = False
                lblConfirmDetailPrintFormError.Visible = True

                Me.udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00148"))
            End If
        End If
        ' INT13-0022 - Fix some special handling on HCSP text only version [End][Koala]

        Return isValid

    End Function

    Private Sub ConfirmClaimSubmit()

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        Dim systemMessage As SystemMessage = Nothing
        Dim udtEHSTransaction As EHSTransactionModel = MyBase.SessionHandler.EHSTransactionGetFromSession(FunctionCode)
        Dim udtSelectedPracticeDisplay As BLL.PracticeDisplayModel = MyBase.SessionHandler.PracticeDisplayGetFromSession(FunctionCode)
        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = MyBase.SessionHandler.EHSClaimVaccineGetFromSession()
        Dim udtEHSAccount As EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(FunctionCode)
        Dim udtSmartIDContent As BLL.SmartIDContentModel = MyBase.SessionHandler.SmartIDContentGetFormSession(FunctionCode)

        Dim strOrignalEHSAccountID As String = Nothing
        Dim blnIsVaccineType As Boolean = True
        Dim blnIsRecordOutdated As Boolean = False
        Dim isCreateBySmartID As Boolean = False
        Dim blnCreateAdment As Boolean = False

        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Dim strSmartIDVer As String = String.Empty
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

        ' Aduit Log: Start Confirm Claim Detail
        EHSClaimBasePage.AuditLogConfirmClaimDetailStart(_udtAuditLogEntry, MyBase.SessionHandler.SmartIDContentGetFormSession(FunctionCode))

        ' Basic Data Validation
        Dim isValid As Boolean = ValidateConfirmClaimDetail()

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

            MyBase.GetCurrentUserAccount(Me._udtSP, Me._udtDataEntryModel, False)
            udtEHSTransaction.TransactionID = Me._udtCommfunct.generateTransactionNumber(udtSchemeClaim.SchemeCode.Trim())

            If Not udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.SpecialAccount Then
                    strOrignalEHSAccountID = udtEHSAccount.OriginalAccID

                ElseIf (Not udtEHSAccount.TransactionID Is Nothing AndAlso Not udtEHSAccount.TransactionID.Equals(String.Empty)) _
                    OrElse (Not udtEHSAccount.CreateSPID Is Nothing AndAlso Not udtEHSAccount.CreateSPID.Equals(Me._udtSP.SPID)) Then

                    strOrignalEHSAccountID = udtEHSAccount.VoucherAccID
                End If
            End If

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
                    'case of X account 
                    '1: Account is special account
                    '2: Account with transaction 
                    '3: Account is created by other Service Provider
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
                    blnIsVaccineType = False
                    Me._udtEHSClaimBLL.ConstructEHSTransDetail_Registration(Me._udtSP, Me._udtDataEntryModel, udtEHSTransaction, udtEHSAccount)
                    ' CRE13-001 - EHAPP [End][Tommy L]
                Else
                    blnIsVaccineType = False
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
                    blnIsVaccineType = False
                    Me._udtEHSClaimBLL.ConstructEHSTransDetail_Registration(Me._udtSP, Me._udtDataEntryModel, udtEHSTransaction, udtEHSAccount)
                    ' CRE13-001 - EHAPP [End][Tommy L]
                Else
                    blnIsVaccineType = False
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
            udtEHSTransaction = _udtEHSTransactionBLL.LoadEHSTransaction(udtEHSTransaction.TransactionID)
            MyBase.SessionHandler.EHSTransactionSaveToSession(udtEHSTransaction, FunctionCode)

            If blnIsVaccineType Then
                udtEHSClaimVaccine = Me._udtEHSClaimBLL.ConstructEHSClaimVaccineModel(udtEHSTransaction.SchemeCode, udtEHSTransaction)
                MyBase.SessionHandler.EHSClaimVaccineSaveToSession(udtEHSClaimVaccine)
            End If

            ' Audit Log: Success
            EHSClaimBasePage.AuditLogConfirmClaimDetailPassed(_udtAuditLogEntry, FunctionCode, udtEHSTransaction, udtSmartIDContent, blnCreateAdment)

            ' Move to Complete Claim
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.CompleteClaim


        ElseIf blnIsRecordOutdated = False AndAlso Me.udcMsgBoxErr.GetCodeTable().Rows.Count > 0 Then
            ' Audit Log: Failed
            EHSClaimBasePage.AuditLogConfirmClaimDetailFailed(_udtAuditLogEntry, FunctionCode)

            ' Avoid Showing the Error Message in Confirmation Page
            If Me.mvEHSClaim.ActiveViewIndex <> ActiveViewIndex.ConfirmDetail Then
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.ConfirmDetail
            End If

            ' Build Error Message
            Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail)
        Else
            ' Unknown Error
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.InternalError
        End If

    End Sub

    Private Sub ClearConfirmDetailError()

        lblConfirmDetailPrintFormError.Visible = False
        ClaimPrintOptionError.Visible = False

    End Sub

#End Region

#Region "step3 Complete Claim"

    Protected Sub btnCompleteClaimNextClaim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCompleteClaimNextClaim.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ' Log Next Claim Clicked
        EHSClaimBasePage.AuditLogNextClaim(_udtAuditLogEntry)

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Me.SessionHandler.HKICSymbolRemoveFromSession(FunctionCode)
        Me.SessionHandler.OCSSSRefStatusRemoveFromSession(FunctionCode)
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        Me.BackToSearhAccount()
    End Sub

    Protected Sub btnCompleteClaimClaimForSamePatient_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCompleteClaimClaimForSamePatient.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        Dim udtSchemeClaim As SchemeClaimModel = SessionHandler.SchemeSelectedGetFromSession(Me.FunctionCode)
        Dim udtEHSAccount As EHSAccountModel = SessionHandler.EHSAccountGetFromSession(Me.FunctionCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim strDOB As String = _udtFormatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, SessionHandler.Language(), udtEHSPersonalInfo.ECAge, udtEHSPersonalInfo.ECDateOfRegistration)
        Dim udtSmartIDContent As BLL.SmartIDContentModel = MyBase.SessionHandler.SmartIDContentGetFormSession(FunctionCode)

        Dim blnNotMatchAccountExist As Boolean
        Dim blnExceedDocTypeLimit As Boolean
        Dim udtEligibleResult As EligibleResult = Nothing
        Dim udtSystemMessage As SystemMessage = Nothing

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
                        'udtSystemMessage = Me._udtEHSClaimBLL.SearchTemporaryAccount(udtSchemeClaim.SchemeCode.Trim(), udtEHSAccount.VoucherAccID, udtEHSAccount, udtEligibleResult, blnExceedDocTypeLimit)
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
                                            strDOB, udtEHSAccount, udtSmartIDContent.EHSAccount, udtSmartIDContent.SmartIDReadStatus, udtEligibleResult, _
                                            blnNotMatchAccountExist, blnExceedDocTypeLimit, FunctionCode, True, ClaimMode.All)
                        'CRE16-017 (Block EHCP make voucher claim for themselves) [End][Chris YIM]

                        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                        ' ----------------------------------------------------------
                        udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).HKICSymbol = SessionHandler.HKICSymbolGetFormSession(FunctionCode)
                        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

                        If blnKeepSmartIDContent Then
                            udtEHSAccount = udtSmartIDContent.EHSAccount
                        Else
                            'Account Created for amendment, use for claim next time
                            udtSmartIDContent.EHSAccount = udtEHSAccount
                        End If

                        MyBase.SessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctionCode)
                        MyBase.SessionHandler.SmartIDContentSaveToSession(FunctionCode, udtSmartIDContent)

                        'Me._udtSystemMessage  is not nothign : systemmessage stored the complete transaction information message
                        udtSystemMessage = Nothing
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

                            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                            ' ----------------------------------------------------------
                            If udtEHSPersonalInfo.DocCode = DocTypeModel.DocTypeCode.HKIC Then
                                udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).HKICSymbol = SessionHandler.HKICSymbolGetFormSession(FunctionCode)
                            End If
                            ' CRE17-010 (OCSSS integration) [End][Chris YIM]

                            SessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctionCode)
                            SessionHandler.SearchAccountStatusSaveToSession(udtSearchAccountStatus)
                        End If
                        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

                    End If
                End If

            Case DocTypeModel.DocTypeCode.EC

                If udtEHSPersonalInfo.ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration AndAlso udtEHSPersonalInfo.ECAge.HasValue Then
                    udtSystemMessage = _udtEHSClaimBLL.SearchEHSAccount(udtSchemeClaim.SchemeCode.Trim, udtEHSAccount.SearchDocCode, _
                        udtEHSPersonalInfo.IdentityNum, udtEHSPersonalInfo.ECAge.Value, udtEHSPersonalInfo.ECDateOfRegistration.Value, _
                        udtEHSAccount, udtEligibleResult, udtSearchAccountStatus, Nothing, FunctionCode, ClaimMode.All)

                Else
                    udtSystemMessage = _udtEHSClaimBLL.SearchEHSAccount(udtSchemeClaim.SchemeCode.Trim, udtEHSAccount.SearchDocCode, _
                        udtEHSPersonalInfo.IdentityNum, strDOB, udtEHSAccount, udtEligibleResult, udtSearchAccountStatus, Nothing, String.Empty, FunctionCode, ClaimMode.All)

                End If
        End Select

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
                udtRuleResults.Add(Me.RuleResultKey(EHSClaimBasePage.ActiveViewIndex.Step1, RuleTypeENum.EligibleResult), udtEligibleResult)

                SessionHandler.EligibleResultSaveToSession(udtRuleResults)
            End If

            MyBase.SessionHandler.EHSTransactionRemoveFromSession(Me.FunctionCode)
            MyBase.SessionHandler.EHSClaimVaccineRemoveFromSession()
            Me.VaccineClear()
            MyBase.SessionHandler.EHSAccountSaveToSession(udtEHSAccount, Me.FunctionCode)
            MyBase.SessionHandler.ClaimCategoryRemoveFromSession(Me.FunctionCode)

            ' Move to Select Scheme
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SelectScheme

            EHSClaimBasePage.AuditLogClaimForSamePatient(_udtAuditLogEntry)
            EHSClaimBasePage.AuditLogEnterClaimDetailLoaded(_udtAuditLogEntry)
        Else
            SessionHandler.EHSTransactionRemoveFromSession(Me.FunctionCode)
            SessionHandler.EHSClaimVaccineRemoveFromSession()
            Me.VaccineClear()
            Me.SessionHandler.EHSAccountRemoveFromSession(Me.FunctionCode)
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.InternalError
        End If

    End Sub

    Protected Overrides Sub SetupCompleteClaim(ByVal udtEHSAccount As Common.Component.EHSAccount.EHSAccountModel, ByVal activeViewChange As Boolean)
        MyBase.SetupCompleteClaim(udtEHSAccount, activeViewChange)

        Dim udtSchemeClaim As SchemeClaimModel = SessionHandler.SchemeSelectedGetFromSession(Me.FunctionCode)
        Dim udtSelectedPracticeDisplay As BLL.PracticeDisplayModel = SessionHandler.PracticeDisplayGetFromSession(Me.FunctionCode)
        Dim udtEHSTransaction As EHSTransactionModel = SessionHandler.EHSTransactionGetFromSession(Me.FunctionCode)
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = SessionHandler.EHSClaimVaccineGetFromSession()
        Dim udtFormatter As Formatter = New Formatter()
        Dim strLanguage As String = SessionHandler.Language()

        If udtEHSAccount Is Nothing Then
            Throw New ArgumentNullException("EHSAccountModel")
        End If

        ' Audit Log Complete Claim
        If activeViewChange Then
            EHSClaimBasePage.AuditLogCompleteClaim(_udtAuditLogEntry, udtEHSTransaction)
        End If

        'CRE15-003 System-generated Form [Start][Philip Chau]
        If Not udtEHSTransaction.PrintedConsentForm Then
            MyBase.SessionHandler.EHSClaimTempTransactionIDRemoveFromSession()
        End If

        Dim strTransactionIDPrefix As String = MyBase.SessionHandler.EHSClaimTempTransactionIDGetFromSession()
        Dim strSchemeSetupModelSetupType As String = SchemeSetupModel.SetupType_ClaimCompletionMessage
        Dim blnPrefixAndTransactionIDTheSame As Boolean = Me._udtEHSClaimBLL.chkIsPrefixAndTransactionIDTheSame(strTransactionIDPrefix, udtEHSTransaction)
        If blnPrefixAndTransactionIDTheSame Then
            Step3HandleTransactionPrefixMisMatch(strTransactionIDPrefix, True)
        Else
            Step3HandleTransactionPrefixMisMatch(strTransactionIDPrefix, False)
            strSchemeSetupModelSetupType = SchemeSetupModel.SetupType_ClaimCompletionMessageOutdateTxNo
        End If
        'CRE15-003 System-generated Form [End][Philip Chau]


        Dim udtSysMsgClaimComplete As SystemMessage
        If blnPrefixAndTransactionIDTheSame Then
            udtSysMsgClaimComplete = New Common.ComObject.SystemMessage("020202", "I", "00002")
        Else
            udtSysMsgClaimComplete = New Common.ComObject.SystemMessage("020202", "I", "00008")
        End If
        Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Complete
        Me.udcMsgBoxInfo.AddMessage(udtSysMsgClaimComplete, New String() {"%s"}, New String() {_udtFormatter.formatSystemNumber(udtEHSTransaction.TransactionID)})
        Me.udcMsgBoxInfo.BuildMessageBox()

        ' Global Translation
        Me.lblCompleteClaimAccInfo.Text = Me.GetGlobalResourceObject("Text", "AccountInfo")
        Me.lblCompleteClaimClaimInfo.Text = Me.GetGlobalResourceObject("Text", "ClaimInfo")
        Me.lblCompleteClaimTransNumText.Text = Me.GetGlobalResourceObject("Text", "TransactionNo")
        Me.lblCompleteClaimTransDateText.Text = Me.GetGlobalResourceObject("Text", "TransactionDate")
        Me.lblCompleteClaimSchemeText.Text = Me.GetGlobalResourceObject("Text", "Scheme")
        Me.lblCompleteClaimServiceDateText.Text = Me.GetGlobalResourceObject("Text", "ServiceDate")
        Me.lblCompleteClaimPracticeText.Text = Me.GetGlobalResourceObject("Text", "Practice")
        Me.lblCompleteClaimBankAccText.Text = Me.GetGlobalResourceObject("Text", "BankAccountNo")
        Me.lblCompleteClaimServiceTypeText.Text = Me.GetGlobalResourceObject("Text", "ServiceType")
        Me.btnCompleteClaimNextClaim.Text = Me.GetGlobalResourceObject("AlternateText", "NextClaimBtn")
        Me.btnCompleteClaimClaimForSamePatient.Text = Me.GetGlobalResourceObject("AlternateText", "ClaimForSamePatientBtn")
        lblCompleteRecipientConditionText.Text = Me.GetGlobalResourceObject("Text", "RecipientCondition")

        ' Doc Info
        Me.udcCompleteClaimReadOnlyDocumnetType.TextOnlyVersion = True
        ' INT13-0021 - Fix use HKBC on smart IC claim incorrectly [Start][Koala]
        ' -------------------------------------------------------------------------------------
        Me.udcCompleteClaimReadOnlyDocumnetType.DocumentType = udtEHSAccount.SearchDocCode
        'Me.udcCompleteClaimReadOnlyDocumnetType.DocumentType = udtEHSAccount.EHSPersonalInformationList(0).DocCode.Trim()  'ToDo: SearchDocCode?
        ' INT13-0021 - Fix use HKBC on smart IC claim incorrectly [End][Koala]
        Me.udcCompleteClaimReadOnlyDocumnetType.EHSAccount = udtEHSAccount
        Me.udcCompleteClaimReadOnlyDocumnetType.Vertical = False
        Me.udcCompleteClaimReadOnlyDocumnetType.MaskIdentityNo = True
        Me.udcCompleteClaimReadOnlyDocumnetType.ShowAccountRefNo = False
        Me.udcCompleteClaimReadOnlyDocumnetType.ShowTempAccountNotice = False
        Me.udcCompleteClaimReadOnlyDocumnetType.ShowAccountCreationDate = False
        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Me.udcCompleteClaimReadOnlyDocumnetType.SetEnableToShowHKICSymbol = True
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]
        Me.udcCompleteClaimReadOnlyDocumnetType.Built()

        'setup Transaction detail
        Me.udcCompleteClaimReadOnlyEHSClaim.TextOnlyVersion = True
        Me.udcCompleteClaimReadOnlyEHSClaim.EHSClaimVaccine = SessionHandler.EHSClaimVaccineGetFromSession()
        Me.udcCompleteClaimReadOnlyEHSClaim.EHSTransaction = udtEHSTransaction
        Me.udcCompleteClaimReadOnlyEHSClaim.SchemeCode = udtSchemeClaim.SchemeCode
        Me.udcCompleteClaimReadOnlyEHSClaim.Mode = ucReadOnlyEHSClaim.ReadOnlyEHSClaimMode.Complete
        Me.udcCompleteClaimReadOnlyEHSClaim.Built()

        'Normal Fields 
        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        ' Fix practice english name display font
        If strLanguage = CultureLanguage.TradChinese Then
            Me.lblCompleteClaimScheme.Text = udtSchemeClaim.SchemeDescChi
            Me.lblCompleteClaimServiceType.Text = udtEHSTransaction.ServiceTypeDesc_Chi
            Me.lblCompleteClaimPractice.Text = String.Format("{0} ({1})", udtSelectedPracticeDisplay.PracticeNameChi, udtSelectedPracticeDisplay.PracticeID)
            Me.lblCompleteClaimPractice.CssClass = "tableTextChi"
        Else
            Me.lblCompleteClaimScheme.Text = udtSchemeClaim.SchemeDesc
            Me.lblCompleteClaimServiceType.Text = udtEHSTransaction.ServiceTypeDesc
            Me.lblCompleteClaimPractice.Text = String.Format("{0} ({1})", udtSelectedPracticeDisplay.PracticeName, udtSelectedPracticeDisplay.PracticeID)
            Me.lblCompleteClaimPractice.CssClass = "tableText"
        End If
        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtTransactionAdditionField As TransactionAdditionalFieldModel = udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ClinicType)
        If Not udtTransactionAdditionField Is Nothing Then
            If udtTransactionAdditionField.AdditionalFieldValueCode = ClinicType.NonClinic Then
                Me.trCompleteNonClinicSetting.Style.Add("display", "initial")
                Me.lblCompleteNonClinicSetting.Text = String.Format("({0})", GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting"))
                'If strLanguage = CultureLanguage.TradChinese Then

                'Else

                'End If
            Else
                Me.trCompleteNonClinicSetting.Style.Add("display", "none")
            End If
        End If
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

        Me.lblCompleteClaimTransNum.Text = _udtFormatter.formatSystemNumber(udtEHSTransaction.TransactionID)
        Me.lblCompleteClaimTransDate.Text = _udtFormatter.formatDateTime(udtEHSTransaction.TransactionDtm)
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtSubPlatformBLL As New SubPlatformBLL
        'Me.lblCompleteClaimServiceDate.Text = _udtFormatter.formatDate(udtEHSTransaction.ServiceDate)
        Me.lblCompleteClaimServiceDate.Text = _udtFormatter.formatDisplayDate(udtEHSTransaction.ServiceDate, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
        Me.lblCompleteClaimBankAcc.Text = _udtFormatter.maskBankAccount(udtEHSTransaction.BankAccountNo)

        ' Recipient Condition
        setupDisplayRecipientCondition(udtEHSTransaction, panCompleteRecipientCondition, lblCompleteRecipientCondition)

    End Sub

    Protected Overrides Sub SetupCompleteClaimClear()
        MyBase.SetupCompleteClaimClear()
    End Sub

    Private Sub udcCompleteClaimReadOnlyEHSClaim_VaccineRemarkClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles udcCompleteClaimReadOnlyEHSClaim.VaccineRemarkClicked
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ' Activate Remark Page and Show Remark
        ShowVaccineRemarkText()

    End Sub

#End Region

#Region "Common function"

    ' Menu Item
    Protected Overrides Function IsMenuItemVisible() As Boolean
        Return SessionHandler.EHSAccountGetFromSession(FunctionCode) Is Nothing OrElse Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.CompleteClaim
    End Function

    Private Sub CancelAction()

        ' Show Cancel Operation's Confirmation 
        ShowConfirmation(ConfirmationStyle.Bordered, "CancelAlert")

    End Sub

    Private Sub ClearClaimSession()

        ' Clear All Claim Related Session (For Returning to Search Account)
        SessionHandler.EHSClaimSessionRemove(FunctionCode)
        SessionHandler.EHSClaimConfirmMessageGetFromSession(FunctionCode)
        SessionHandler.EHSClaimStepsRemoveFromSession(FunctionCode)

    End Sub

    Private Sub BackToSearhAccount()

        ' Clear Session Before Return to Search Account
        ClearClaimSession()

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        RedirectHandler.ToURL(EHSClaim)

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End
    End Sub

    Private Sub BackToStep2a()

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        RedirectHandler.ToURL(EHSClaim)

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End
    End Sub

    Private Sub GoToView(ByVal intActiveViewIndex As Integer)

        SessionHandler.EHSClaimStepsSaveToSession(FunctionCode, Me.mvEHSClaim.ActiveViewIndex)

        Me.mvEHSClaim.ActiveViewIndex = intActiveViewIndex

    End Sub

    Private Sub BackToPreviousView()

        ' Get Previous View
        Dim intViewIndex As Integer = SessionHandler.EHSClaimStepsGetFromSession(FunctionCode)

        SessionHandler.EHSClaimStepsRemoveFromSession(FunctionCode)

        Me.mvEHSClaim.ActiveViewIndex = intViewIndex

    End Sub

    ' Clear Error / Information Message
    Private Sub ClearMessageBox()
        Me.udcMsgBoxErr.Clear()
        Me.udcMsgBoxInfo.Clear()
    End Sub

    ' Return the number of Scheme Available for the Selected Practice
    Private Function GetAvailableScheme() As SchemeClaimModelCollection

        Me.GetCurrentUserAccount(Me._udtSP, Me._udtDataEntryModel, False)

        ' Check EHSAccount Exists
        Dim udtSchemeClaimBLL As SchemeClaimBLL = New SchemeClaimBLL()
        Dim udtSelectedPracticeDisplay As BLL.PracticeDisplayModel = MyBase.SessionHandler.PracticeDisplayGetFromSession(Me.FunctionCode)
        Dim udtEHSAccount As EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(Me.FunctionCode)
        If Not udtSelectedPracticeDisplay Is Nothing Then
            If udtEHSAccount Is Nothing Then
                ' Return the Scheme List filtered by Practice only
                Return udtSchemeClaimBLL.searchValidClaimPeriodSchemeClaimByPracticeSchemeInfoSubsidizeCode(Me._udtSP.PracticeList(udtSelectedPracticeDisplay.PracticeID).PracticeSchemeInfoList, Me._udtSP.SchemeInfoList)
            Else
                ' Return the Scheme List filtered by EHSAccount
                Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = Me.SessionHandler.SchemeSubsidizeListGetFromSession(Me.FunctionCode)

                If udtSchemeClaimModelCollection Is Nothing Then

                    'Get all available Scheme <- for SP
                    udtSchemeClaimModelCollection = udtSchemeClaimBLL.searchValidClaimPeriodSchemeClaimByPracticeSchemeInfoSubsidizeCode(SessionSP.PracticeList(udtSelectedPracticeDisplay.PracticeID).PracticeSchemeInfoList, Me._udtSP.SchemeInfoList)
                    'Get all Eligible Scheme form available List <- for EHS Account
                    udtSchemeClaimModelCollection = udtSchemeClaimBLL.searchEligibleAndExceedDocumentClaimScheme(udtEHSAccount, udtEHSAccount.SearchDocCode, udtSchemeClaimModelCollection)

                    Me.SessionHandler.SchemeSubsidizeListSaveToSession(udtSchemeClaimModelCollection, Me.FunctionCode)
                End If

                Return udtSchemeClaimModelCollection
            End If
        Else
            Return New SchemeClaimModelCollection()
        End If

    End Function

    Private Sub ClearWarningRules(ByRef udtRuleResults As RuleResultCollection)
        'Reset EligibleResult Collection in Session
        If Not udtRuleResults Is Nothing AndAlso udtRuleResults.Count > 0 Then
            MyBase.SessionHandler.EligibleResultRemoveFromSession()

            Dim strKey As String = Me.RuleResultKey(EHSClaimBasePage.ActiveViewIndex.Step1, RuleTypeENum.EligibleResult)
            Dim udtRuleResult As RuleResult = udtRuleResults.Item(strKey)

            'if Eligible Search Result is existing in session 
            If Not udtRuleResult Is Nothing Then
                udtRuleResults = New RuleResultCollection()
                udtRuleResults.Add(strKey, udtRuleResult)
                MyBase.SessionHandler.EligibleResultSaveToSession(udtRuleResults)
            End If
        End If
    End Sub

    Private Function RuleResultKey(ByVal strActiveViewIndex As String, ByVal enumRuleType As RuleTypeENum) As String
        Return String.Format("{0}_{1}", strActiveViewIndex, enumRuleType)
    End Function

    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
    ' Remove input parameter - strSchemeCode
    'Private Function RemoveRulesAfterConfirm(ByVal strSchemeCode As String, ByRef udtRuleResults As RuleResultCollection, ByVal isValid As Boolean) As Boolean
    Private Function RemoveRulesAfterConfirm(ByRef udtRuleResults As RuleResultCollection, ByVal isValid As Boolean) As Boolean
        ' --------------------------------------------------------------
        ' Eligibility rule
        ' --------------------------------------------------------------
        Dim strKey As String = Me.RuleResultKey(ActiveViewIndex.Vaccine, RuleTypeENum.EligibleResult)
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
        strKey = Me.RuleResultKey(ActiveViewIndex.Vaccine, RuleTypeENum.ClaimRuleResult)
        udtRuleResult = udtRuleResults.Item(strKey)
        If isValid Then
            If Not udtRuleResult Is Nothing Then
                Me.ShowConfirmation(ConfirmationStyle.Bordered, udtRuleResult)
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

    Private Sub SetupEnterClaimDetail()
        Dim strAccInfoText As String = Me.GetGlobalResourceObject("Text", "AccountInfo")

        Me.lblServiceDateAccInfoText.Text = strAccInfoText
        Me.lblCategoryAccInfoText.Text = strAccInfoText
        Me.lblMedicalConditionAccInfoText.Text = strAccInfoText
        Me.lblECHCodeAccInfoText.Text = strAccInfoText
        Me.lblVaccineAccInfoText.Text = strAccInfoText
        Me.lblVaccinationRecordAccInfoText.Text = strAccInfoText
        'CRE15-005-03 (New PIDVSS scheme) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Me.lblDocumentaryProofAccInfoText.Text = strAccInfoText
        'CRE15-005-03 (New PIDVSS scheme) [End][Chris YIM]
        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Me.lblPlaceVaccinationAccInfoText.Text = strAccInfoText
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

        Dim strAccInfoEnameText As String = Me.GetGlobalResourceObject("Text", "Name")

        Me.lblServiceDateAccInfoENameText.Text = strAccInfoEnameText
        Me.lblCategoryAccInfoENameText.Text = strAccInfoEnameText
        Me.lblMedicalConditionAccInfoENameText.Text = strAccInfoEnameText
        Me.lblECHCodeAccInfoENameText.Text = strAccInfoEnameText
        Me.lblVaccineAccInfoENameText.Text = strAccInfoEnameText
        Me.lblVaccinationRecordAccInfoENameText.Text = strAccInfoEnameText
        'CRE15-005-03 (New PIDVSS scheme) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Me.lblDocumentaryProofAccInfoENameText.Text = strAccInfoEnameText
        'CRE15-005-03 (New PIDVSS scheme) [End][Chris YIM]
        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Me.lblPlaceVaccinationAccInfoENameText.Text = strAccInfoEnameText
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

        Dim udtEHSAccout As EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(Me.FunctionCode)
        Dim strEHSAccountEName As String = udtEHSAccout.getPersonalInformation(udtEHSAccout.SearchDocCode).EName

        Me.lblServiceDateAccInfoEName.Text = strEHSAccountEName
        Me.lblCategoryAccInfoEName.Text = strEHSAccountEName
        Me.lblMedicalConditionAccInfoEName.Text = strEHSAccountEName
        Me.lblECHCodeAccInfoEName.Text = strEHSAccountEName
        Me.lblVaccineAccInfoEName.Text = strEHSAccountEName
        Me.lblVaccinationRecordAccInfoEName.Text = strEHSAccountEName
        'CRE15-005-03 (New PIDVSS scheme) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Me.lblDocumentaryProofAccInfoEName.Text = strEHSAccountEName
        'CRE15-005-03 (New PIDVSS scheme) [End][Chris YIM]
        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Me.lblPlaceVaccinationAccInfoEName.Text = strEHSAccountEName
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

        Dim strViewDetailBtnText As String = Me.GetGlobalResourceObject("AlternateText", "ViewDetailsBtn")

        Me.btnServiceDateViewDetail.Text = strViewDetailBtnText
        Me.btnCategoryViewDetail.Text = strViewDetailBtnText
        Me.btnMedicalConditionViewDetail.Text = strViewDetailBtnText
        Me.btnRCHCodeViewDetail.Text = strViewDetailBtnText
        Me.btnVaccineViewDetail.Text = strViewDetailBtnText
        Me.btnVaccinationRecordViewDetail.Text = strViewDetailBtnText
        'CRE15-005-03 (New PIDVSS scheme) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Me.btnDocumentaryProofViewDetail.Text = strViewDetailBtnText
        'CRE15-005-03 (New PIDVSS scheme) [End][Chris YIM]
        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Me.btnPlaceVaccinationViewDetail.Text = strViewDetailBtnText
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

        Dim strViewVaccinationRecordBtnText As String = Me.GetGlobalResourceObject("AlternateText", "ViewVaccinationRecordBtn")

        Me.btnServiceDateViewVaccinationRecord.Text = strViewVaccinationRecordBtnText
        Me.btnCategoryViewVaccinationRecord.Text = strViewVaccinationRecordBtnText
        Me.btnMedicalConditionViewVaccinationRecord.Text = strViewVaccinationRecordBtnText
        Me.btnRCHCodeViewVaccinationRecord.Text = strViewVaccinationRecordBtnText
        Me.btnVaccineViewVaccinationRecord.Text = strViewVaccinationRecordBtnText
        'CRE15-005-03 (New PIDVSS scheme) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Me.btnDocumentaryProofViewVaccinationRecord.Text = strViewVaccinationRecordBtnText
        'CRE15-005-03 (New PIDVSS scheme) [End][Chris YIM]
        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Me.btnPlaceVaccinationViewVaccinationRecord.Text = strViewVaccinationRecordBtnText
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

    End Sub

    Private Function GetStepText() As String
        Select Case Me.mvEHSClaim.ActiveViewIndex
            'Case ActiveViewIndex.Step1
            '    Return Me.GetGlobalResourceObject("Text", "ClaimStep1")
            Case ActiveViewIndex.ServiceDate, ActiveViewIndex.Category, ActiveViewIndex.MedicalCondition, ActiveViewIndex.RCHCode, ActiveViewIndex.Vaccine, ActiveViewIndex.ConfirmDetail
                Return Me.GetGlobalResourceObject("Text", "ClaimStep2")
            Case ActiveViewIndex.CompleteClaim
                Return Me.GetGlobalResourceObject("Text", "ClaimStep3")

            Case ActiveViewIndex.ConfirmBox
                Dim objConfirmMessage As Object = SessionHandler.EHSClaimConfirmMessageGetFromSession(FunctionCode)
                If Not objConfirmMessage Is Nothing AndAlso objConfirmMessage.Equals("ProvidedInfoTrueClaimSP") Then
                    Return Me.GetGlobalResourceObject("Text", "ClaimStep2")
                End If

            Case ActiveViewIndex.VaccinationRecord
                Return Me.GetGlobalResourceObject("Text", "VaccinationRecord")

        End Select

        Return String.Empty
    End Function

    Private Sub FillPracticeText(ByRef ctrlLabel As Label)

        Dim udtPracticeDisplay As BLL.PracticeDisplayModel = SessionHandler.PracticeDisplayGetFromSession(Me.FunctionCode)
        If udtPracticeDisplay Is Nothing Then
            ' Throw New Exception("Practice Empty!")
            ctrlLabel.Text = String.Empty
        Else
            Dim strPracticeName As String = IIf(String.IsNullOrEmpty(udtPracticeDisplay.PracticeNameChi) OrElse SessionHandler.Language = CultureLanguage.English, udtPracticeDisplay.PracticeName, udtPracticeDisplay.PracticeNameChi)
            ctrlLabel.Text = String.Format("{0} ({1})", strPracticeName, udtPracticeDisplay.PracticeID)
        End If

    End Sub

    Private Function CheckVaccineAvail(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel) As Boolean
        Dim notAvailableForClaim As Boolean = True

        If Not udtEHSClaimVaccine Is Nothing Then

            If Not udtEHSClaimVaccine.SubsidizeList Is Nothing Then
                'Check if no vaccine is avaliable for the recipient -> change "noAvailableForClaim" to false
                For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
                    If udtEHSClaimSubsidize.Available Then
                        notAvailableForClaim = False
                        Exit For
                    End If
                Next
            End If
        End If

        Return Not notAvailableForClaim
    End Function

    Private Function CheckVaccineSelected(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel) As Boolean
        Dim isVaccineSelected As Boolean = False

        If Not udtEHSClaimVaccine Is Nothing AndAlso Not udtEHSClaimVaccine.SubsidizeList Is Nothing Then
            For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
                If udtEHSClaimSubsidize.Selected Then
                    isVaccineSelected = True
                    Exit For
                End If
            Next

        End If

        Return isVaccineSelected
    End Function

    Private Sub HandlePageRefreshed()

        ' Clear All EHSClaim Session and Return to Search Account
        Me.ClearClaimSession()

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        RedirectHandler.ToURL(EHSClaim)

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

    End Sub

    Protected Overrides Sub HandleConcurrentBrowser()
        'MyBase.HandleConcurrentBrowser()

        Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.InternalError

    End Sub

#End Region

#Region "Property"

    Protected Overrides ReadOnly Property FunctionCode() As String
        Get
            Return Common.Component.FunctCode.FUNT020202
        End Get
    End Property

    Protected Overrides ReadOnly Property BrowserTokenHiddenField() As System.Web.UI.WebControls.HiddenField
        Get
            Return hfEHSClaimTokenNum
        End Get
    End Property

    Protected ReadOnly Property SessionSP() As ServiceProviderModel
        Get
            Me.GetCurrentUserAccount(Me._udtSP, Me._udtDataEntryModel, False)

            Return _udtSP
        End Get
    End Property
#End Region

#Region "Select Practice"

    Protected Sub btnStepSelectPracticeSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStepSelectPracticeSelect.Click

        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        Dim udtPractice As BLL.PracticeDisplayModel = MyBase.SessionHandler.PracticeDisplayGetFromSession(Me.FunctionCode)
        Dim udtEHSAccount As EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(Me.FunctionCode)
        Dim strPracticeId As String = ucSelectPracticeRadioButtonGroupText.SelectedValue.Trim()
        Dim intPracticeId As Integer
        Dim udtPracticeDisplays As BLL.PracticeDisplayModelCollection
        Dim keepInClaimV2 As Boolean = True
        Dim udtSchemeClaims As SchemeClaimModelCollection = Nothing

        If String.IsNullOrEmpty(strPracticeId) Then
            MyBase.SessionHandler.PracticeDisplayRemoveFromSession(Me.FunctionCode)

        Else
            udtPracticeDisplays = SessionHandler.PracticeDisplayListGetFromSession()
            intPracticeId = Convert.ToInt32(strPracticeId)

            'If Practice Changed
            If Not udtPractice Is Nothing AndAlso udtPractice.PracticeID <> strPracticeId Then
                Dim udtSchemeClaimBLL As SchemeClaimBLL = New SchemeClaimBLL()
                Dim udtSchemeClaim As SchemeClaimModel = SessionHandler.SchemeSelectedGetFromSession(Me.FunctionCode)

                Dim blnIsSchemeExists As Boolean = False

                'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                SessionHandler.NonClinicSettingRemoveFromSession(FunctionCode)
                SessionHandler.PIDInstitutionCodeRemoveFromSession(FunctionCode)
                SessionHandler.PlaceVaccinationRemoveFromSession(FunctionCode)
                SessionHandler.PlaceVaccinationOtherRemoveFromSession(FunctionCode)
                'CRE16-002 (Revamp VSS) [End][Chris YIM]

                udtSchemeClaims = udtSchemeClaimBLL.searchValidClaimPeriodSchemeClaimByPracticeSchemeInfoSubsidizeCode(SessionSP.PracticeList(intPracticeId).PracticeSchemeInfoList, Me._udtSP.SchemeInfoList)
                ' Get all Eligible Scheme form available List <- for EHS Account
                udtSchemeClaims = udtSchemeClaimBLL.searchEligibleAndExceedDocumentClaimScheme(udtEHSAccount, udtEHSAccount.SearchDocCode, udtSchemeClaims)

                Me.SessionHandler.SchemeSubsidizeListSaveToSession(udtSchemeClaims, Me.FunctionCode)

                If Not udtSchemeClaims Is Nothing Then
                    If udtSchemeClaims.Count = 1 Then
                        MyBase.SessionHandler.SchemeSelectedSaveToSession(udtSchemeClaims(0), Me.FunctionCode)
                        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                        Dim udtConvertedSchemeCode As String = udtSchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(udtSchemeClaims(0).SchemeCode)
                        SessionHandler.NonClinicSettingSaveToSession(SessionSP.PracticeList(intPracticeId).PracticeSchemeInfoList.Filter(udtConvertedSchemeCode).IsNonClinic, FunctionCode)
                        'CRE16-002 (Revamp VSS) [End][Chris YIM]

                        If Not udtSchemeClaim Is Nothing AndAlso udtSchemeClaims(0).SchemeCode = udtSchemeClaim.SchemeCode Then
                            blnIsSchemeExists = True
                        End If

                        Select Case udtSchemeClaims(0).ControlType
                            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                            '-----------------------------------------------------------------------------------------
                            Case SchemeClaimModel.EnumControlType.VOUCHER ',SchemeClaimModel.EnumControlType.VOUCHERCHINA
                                'CRE16-002 (Revamp VSS) [End][Chris YIM]

                                keepInClaimV2 = False
                                MyBase.SessionHandler.AccountCreationProceedClaimSaveToSession(True)
                        End Select

                    ElseIf udtSchemeClaims.Count > 0 Then
                        If Not udtSchemeClaim Is Nothing Then

                            ' Check the Session's Scheme exists in the new practice's scheme list
                            For Each udtSchemeClaimModel As SchemeClaimModel In udtSchemeClaims
                                If udtSchemeClaimModel.SchemeCode = udtSchemeClaim.SchemeCode Then
                                    blnIsSchemeExists = True
                                    MyBase.SessionHandler.SchemeSelectedSaveToSession(udtSchemeClaimModel, Me.FunctionCode)
                                    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                                    '-----------------------------------------------------------------------------------------
                                    Dim udtConvertedSchemeCode As String = udtSchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(udtSchemeClaimModel.SchemeCode)
                                    SessionHandler.NonClinicSettingSaveToSession(SessionSP.PracticeList(intPracticeId).PracticeSchemeInfoList.Filter(udtConvertedSchemeCode).IsNonClinic, FunctionCode)
                                    'CRE16-002 (Revamp VSS) [End][Chris YIM]
                                End If
                            Next
                        End If
                    Else
                        'No scheme for recipient
                        keepInClaimV2 = True
                        blnIsSchemeExists = False
                    End If
                End If

                MyBase.SessionHandler.EHSClaimVaccineRemoveFromSession()
                Me.VaccineClear()

                If Not blnIsSchemeExists OrElse Not keepInClaimV2 Then
                    ' Scheme Updated: Remove Transaction
                    MyBase.SessionHandler.EHSTransactionRemoveFromSession(Me.FunctionCode)
                    MyBase.SessionHandler.ClaimCategoryRemoveFromSession(Me.FunctionCode)

                    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    If udtSchemeClaims.Count > 1 Then
                        MyBase.SessionHandler.SchemeSelectedRemoveFromSession(Me.FunctionCode)
                    End If
                    'CRE16-002 (Revamp VSS) [End][Chris YIM]
                End If

            End If

            ' save practice to session
            MyBase.SessionHandler.PracticeDisplaySaveToSession(udtPracticeDisplays.Filter(intPracticeId), Me.FunctionCode)

            ' Audlt Log Practice Selection            
            Dim udtSessionScheme As Scheme.SchemeClaimModel = SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
            EHSClaimBasePage.AuditLogPracticeSelected(_udtAuditLogEntry, True, SessionHandler.PracticeDisplayGetFromSession(FunctionCode), udtSessionScheme, Not udtSessionScheme Is Nothing)

        End If

        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        If keepInClaimV2 Then
            If StepSelectPracticeValdiation() Then

                If Not udtSchemeClaims Is Nothing AndAlso udtSchemeClaims.Count = 1 Then
                    Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.ServiceDate
                Else
                    ' go to next step, select scheme
                    Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SelectScheme
                End If

            End If
        Else
            If MyBase.SessionHandler.AccountCreationProceedClaimGetFromSession() Then
                ' If voucher, back to V1
                Me.BackToStep2a()
            End If
        End If
        'CRE16-002 (Revamp VSS) [End][Chris YIM]


    End Sub

    Private Sub SetupSelectPractice()
        Me.lblStepSelectPracticePracticeText.Text = Me.GetGlobalResourceObject("Text", "SelectPractice")
        Me.btnStepSelectPracticeSelect.Text = Me.GetGlobalResourceObject("AlternateText", "SelectBtn")

        Dim udtPracticeDisplays As BLL.PracticeDisplayModelCollection = SessionHandler.PracticeDisplayListGetFromSession()

        SessionHandler.CurrentUserGetFromSession(Me._udtSP, Me._udtDataEntryModel)

        'Build practice Selection List
        ucSelectPracticeRadioButtonGroupText.MaskBankAccountNo = True
        ucSelectPracticeRadioButtonGroupText.BuildRadioButtonGroup(True, udtPracticeDisplays, Me._udtSP.PracticeList, SessionHandler.Language, PracticeRadioButtonGroup.DisplayMode.BankAccount)

        ' Check Session Object exists, and select
        Dim udtSessionPracticeDisplay As BLL.PracticeDisplayModel = SessionHandler.PracticeDisplayGetFromSession(Me.FunctionCode)
        If Not udtSessionPracticeDisplay Is Nothing AndAlso String.IsNullOrEmpty(ucSelectPracticeRadioButtonGroupText.SelectedValue) Then
            ucSelectPracticeRadioButtonGroupText.SelectedValue = udtSessionPracticeDisplay.PracticeID
        End If

        If Me.SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
            Me.ucSelectPracticeRadioButtonGroupText.CssClass = "tableTextChi"
        Else
            Me.ucSelectPracticeRadioButtonGroupText.CssClass = "tableText"
        End If

    End Sub

    Private Function GetAvailablePractice() As BLL.PracticeDisplayModelCollection
        Return Me.SessionHandler.PracticeDisplayListGetFromSession()
    End Function

    Private Sub StepSelectPracticeClear()
        ucSelectPracticeRadioButtonGroupText.Controls.Clear()
    End Sub

    Protected Function StepSelectPracticeValdiation() As Boolean
        ' Check practice is selected in session
        Dim blnIsValid As Boolean = True
        Dim udtSessionPractice As BLL.PracticeDisplayModel = SessionHandler.PracticeDisplayGetFromSession(Me.FunctionCode)

        If udtSessionPractice Is Nothing Then
            blnIsValid = False
            Me.udcMsgBoxErr.AddMessage(New SystemMessage(Me.FunctionCode, "E", "00017"))
        End If

        Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail)

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

    ''' Check the User can go to next step or not
    Protected Function StepSelectSchemeValdiation() As Boolean
        ' Check scheme is selected in session
        Dim blnIsValid As Boolean = True
        Dim udtSessionClaim As SchemeClaimModel = SessionHandler.SchemeSelectedGetFromSession(Me.FunctionCode)

        If udtSessionClaim Is Nothing Then
            ' Error
            blnIsValid = False

            Dim udtSysMessageScheme As SystemMessage = New SystemMessage(Me.FunctionCode, "E", "00018")
            Me.udcMsgBoxErr.AddMessage(udtSysMessageScheme)
        End If
        Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail)
        Return blnIsValid

    End Function


#End Region

#Region "Select Scheme"

    Protected Sub btnStepSelectSchemeChangePractice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStepSelectSchemeChangePractice.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SelectPractice
    End Sub

    Protected Sub btnStepSelectSchemeSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStepSelectSchemeSelect.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        Dim strScheme As String = ucSchemeRadioButtonGroupText.SelectedValue.Trim()
        Dim udtSessionSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(Me.FunctionCode)
        Dim udtSchemeClaimBLL As SchemeClaimBLL = New SchemeClaimBLL()
        Dim udtEHSAccount As EHSAccountModel = MyBase.SessionHandler.EHSAccountGetFromSession(Me.FunctionCode)

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
                If udtSessionSchemeClaim Is Nothing OrElse udtSessionSchemeClaim.SchemeCode.Trim() <> strScheme OrElse String.IsNullOrEmpty(SessionHandler.DocumentTypeSelectedGetFromSession(Me.FunctionCode)) Then

                    Dim strDefaultDocTypeCode = GetDefaultDocumnetTypeByScheme(strScheme)

                    If Not String.IsNullOrEmpty(strDefaultDocTypeCode) Then
                        ' Default the major Document Type, Auto Select it and go to Step1
                        SessionHandler.DocumentTypeSelectedSaveToSession(strDefaultDocTypeCode, Me.FunctionCode)
                    End If
                Else
                    ' Same Scheme. not need to update the corresponding DocType
                End If
            Else
                ' Will go back to Step2a
                ' Check New scheme diff from scheme in session, clear the step2a control if changed
                If udtSessionSchemeClaim Is Nothing OrElse udtSessionSchemeClaim.SchemeCode.Trim() <> strScheme Then
                    ' Scheme Updated: Remove Transaction
                    MyBase.SessionHandler.EHSTransactionRemoveFromSession(Me.FunctionCode)
                    MyBase.SessionHandler.EHSClaimVaccineRemoveFromSession()
                    Me.VaccineClear()
                    MyBase.SessionHandler.ClaimCategoryRemoveFromSession(Me.FunctionCode)
                End If
            End If

            ' save the updated scheme to session
            SessionHandler.SchemeSelectedSaveToSession(udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(strScheme), Me.FunctionCode)
            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim udtSelectedPracticeDisplay As PracticeDisplayModel = SessionHandler.PracticeDisplayGetFromSession(FunctionCode)
            Dim udtConvertedSchemeCode As String = udtSchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(strScheme)
            SessionHandler.NonClinicSettingSaveToSession(SessionSP.PracticeList(udtSelectedPracticeDisplay.PracticeID).PracticeSchemeInfoList.Filter(udtConvertedSchemeCode).IsNonClinic, FunctionCode)
            'CRE16-002 (Revamp VSS) [End][Chris YIM]
        End If

        ' Move to next step when scheme is valid
        If StepSelectSchemeValdiation() Then
            udtSessionSchemeClaim = MyBase.SessionHandler.SchemeSelectedGetFromSession(Me.FunctionCode)

            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
            ' --------------------------------------------------------------------------------------
            Select Case udtSessionSchemeClaim.ControlType
                Case SchemeClaimModel.EnumControlType.RVP, SchemeClaimModel.EnumControlType.HSIVSS, SchemeClaimModel.EnumControlType.EVSS, _
                    SchemeClaimModel.EnumControlType.CIVSS, SchemeClaimModel.EnumControlType.PIDVSS, SchemeClaimModel.EnumControlType.VSS, _
                    SchemeClaimModel.EnumControlType.ENHVSSO

                    Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.ServiceDate

                Case Else
                    MyBase.SessionHandler.AccountCreationProceedClaimSaveToSession(True)
                    Me.BackToStep2a()

            End Select
            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]
        End If

    End Sub

    Protected Sub btnStepSelectSchemeBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStepSelectSchemeBack.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        Me.BackToSearhAccount()
    End Sub

    Private Sub SetupSelectScheme(ByVal activeViewChanged As Boolean)
        ' Translation
        Me.lblStepSelectSchemePracticeText.Text = Me.GetGlobalResourceObject("Text", "Practice")
        Me.btnStepSelectSchemeChangePractice.Text = Me.GetGlobalResourceObject("AlternateText", "Change")
        Me.lblStepSelectSchemeScheme.Text = Me.GetGlobalResourceObject("Text", "SelectScheme")
        Me.btnStepSelectSchemeSelect.Text = Me.GetGlobalResourceObject("AlternateText", "SelectBtn")
        Me.btnStepSelectSchemeBack.Text = Me.GetGlobalResourceObject("AlternateText", "BackBtn")

        ' Show Practice
        Me.FillPracticeText(lblStepSelectSchemePractice)

        ' Hide Change Practice button when only 1 practice available
        Dim udtPracticeDisplayModelCollection As BLL.PracticeDisplayModelCollection = GetAvailablePractice()
        Me.btnStepSelectSchemeChangePractice.Visible = (Not udtPracticeDisplayModelCollection Is Nothing AndAlso udtPracticeDisplayModelCollection.Count > 1)

        'Build practice Selection List
        Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = GetAvailableScheme()
        If Not udtSchemeClaimModelCollection Is Nothing And udtSchemeClaimModelCollection.Count > 0 Then
            ' Have Scheme to Select
            ucSchemeRadioButtonGroupText.PopulateScheme(True, udtSchemeClaimModelCollection, SessionHandler.Language, FunctionCode, Me._udtSP)

            lblStepSelectSchemeScheme.Visible = True
            btnStepSelectSchemeSelect.Visible = True
        Else
            ' There is no applicable scheme for the eHealth Account in the selected practice.
            ucSchemeRadioButtonGroupText.Items.Clear()

            Dim udtSysMessageNoScheme As SystemMessage = New SystemMessage("990000", "I", "00020")
            Me.udcMsgBoxInfo.AddMessage(udtSysMessageNoScheme)
            Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Information
            Me.udcMsgBoxInfo.BuildMessageBox()

            lblStepSelectSchemeScheme.Visible = False
            btnStepSelectSchemeSelect.Visible = False
        End If

        ' Check Session Object exists, and select
        Dim udtSessionClaimModel As SchemeClaimModel = SessionHandler.SchemeSelectedGetFromSession(Me.FunctionCode)
        If activeViewChanged AndAlso Not udtSessionClaimModel Is Nothing Then

            Dim listItem As ListItem = ucSchemeRadioButtonGroupText.Items.FindByValue(udtSessionClaimModel.SchemeCode)

            If Not listItem Is Nothing Then
                ucSchemeRadioButtonGroupText.SelectedValue = udtSessionClaimModel.SchemeCode
            End If
        End If
    End Sub

    Private Sub StepSelectSchemeClear()
        ucSchemeRadioButtonGroupText.Items.Clear()
    End Sub


#End Region

#Region "Vaccination Record"

    Protected Sub btnVaccinationRecordViewDetail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVaccinationRecordViewDetail.Click
        EHSClaimBasePage.AuditLogVaccinationRecordViewDetailClick(_udtAuditLogEntry)

        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        Session(SESS.ViewBeforeVaccinationRecord) = SessionHandler.EHSClaimStepsGetFromSession(FunctionCode)

        Me.ShowTransactionDetail()
    End Sub

    '

    Protected Sub btnVRContinue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVRContinue.Click
        EHSClaimBasePage.AuditLogVaccinationRecordCloseClick(_udtAuditLogEntry)

        ViewState(VS.VaccinationRecordPopupStatus) = VaccinationRecordPopupStatusClass.Closed
        Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.ServiceDate

    End Sub

    Protected Sub btnVRReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVRReturn.Click
        EHSClaimBasePage.AuditLogVaccinationRecordCloseClick(_udtAuditLogEntry)

        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        Me.BackToPreviousView()
    End Sub

    '

    Private Sub SetupVaccinationRecord()
        udcVaccinationRecord.RebuildVaccinationRecordGrid()
    End Sub


    ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Private Sub ShowVaccinationRecord(ByVal blnRecall As Boolean)
        If blnRecall Then MyBase.SessionHandler.EHSClaimStepsSaveToSession(FunctionCode, mvEHSClaim.ActiveViewIndex)

        mvEHSClaim.ActiveViewIndex = ActiveViewIndex.VaccinationRecord

        Dim udtEHSAccount As EHSAccountModel = SessionHandler.EHSAccountGetFromSession(FunctionCode)

        GetVaccinationRecordFromSession(udtEHSAccount, "")
        Dim udtHAVaccineResultSession As Common.WebService.Interface.HAVaccineResult = Nothing
        Dim udtDHVaccineResultSession As Common.WebService.Interface.DHVaccineResult = Nothing
        If Not blnRecall Then ' Force re-enquiry HA CMS vaccination record for build UI
            udtHAVaccineResultSession = SessionHandler.CMSVaccineResultGetFromSession(FunctionCode)
        End If
        If Not blnRecall Then
            udtDHVaccineResultSession = SessionHandler.CIMSVaccineResultGetFromSession(FunctionCode)
        End If

        Dim udtSystemMessageList As List(Of SystemMessage) = udcVaccinationRecord.Build(udtEHSAccount, udtHAVaccineResultSession, udtDHVaccineResultSession, _udtAuditLogEntry, True)

        If blnRecall Then ' If Force re-enquiry then cache the reason for futher use
            SessionHandler.CMSVaccineResultSaveToSession(udcVaccinationRecord.HAVaccineResult, FunctionCode)
            SessionHandler.CIMSVaccineResultSaveToSession(udcVaccinationRecord.DHVaccineResult, FunctionCode)
        End If

        btnVRContinue.Visible = Not blnRecall
        btnVRReturn.Visible = blnRecall

        ' Build system message
        For Each udtSystemMessage As SystemMessage In udtSystemMessageList
            If Not IsNothing(udtSystemMessage) Then
                If udtSystemMessage.SeverityCode = SeverityCode.SEVI Then
                    udcMsgBoxInfo.AddMessage(udtSystemMessage)
                    udcMsgBoxInfo.BuildMessageBox()
                Else
                    udcMsgBoxErr.AddMessage(udtSystemMessage)
                    udcMsgBoxErr.BuildMessageBox("ConnectionFail")
                End If
            End If
        Next

    End Sub
    ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

    Private Function CheckShowVaccinationRecord() As Boolean
        Select Case ViewState(VS.VaccinationRecordPopupStatus)
            Case Nothing
                EHSClaimBasePage.AuditLogForceShowVaccinationRecord(_udtAuditLogEntry)

                ShowVaccinationRecord(False)
                Return True
        End Select

    End Function

#End Region

#Region "Supporting Functions"

    Private Function SmartIDShowRealID() As Boolean
        Dim udtGeneralFunction As New GeneralFunction
        Dim strParmValue As String = String.Empty
        udtGeneralFunction.getSystemParameter("SmartIDShowRealID", strParmValue, String.Empty)
        Return strParmValue.Trim = "Y"
    End Function

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

    ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Public Function GetVaccinationRecordFromSession(ByVal udtEHSAccount As EHSAccountModel, Optional ByVal strSchemeCode As String = "") As TransactionDetailVaccineModelCollection
        Dim udtVaccinationBLL As New VaccinationBLL
        Dim udtEHSTransactionBLL As New EHSTransactionBLL
        Dim udtSession As New BLL.SessionHandler

        Dim htRecordSummary As Hashtable = Nothing
        Dim udtTranDetailVaccineList As TransactionDetailVaccineModelCollection = Nothing
        Dim udtHAVaccineResult As Common.WebService.Interface.HAVaccineResult = Nothing
        Dim udtDHVaccineResult As Common.WebService.Interface.DHVaccineResult = Nothing
        Dim udtHAVaccineResultSession As Common.WebService.Interface.HAVaccineResult = udtSession.CMSVaccineResultGetFromSession(FunctionCode)
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
    ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]


    Private Function GetExtRefStatus(ByVal udtEHSAccount As EHSAccountModel, Optional ByVal strSchemeCode As String = "") As EHSTransactionModel.ExtRefStatusClass
        Dim udtExtRefStatus As EHSTransactionModel.ExtRefStatusClass = MyBase.SessionHandler.ExtRefStatusGetFromSession()
        If udtExtRefStatus Is Nothing Then
            GetVaccinationRecordFromSession(udtEHSAccount, strSchemeCode)
            ' INT13-0021 - Fix use HKBC on smart IC claim incorrectly [Start][Koala]
            ' -------------------------------------------------------------------------------------
            udtExtRefStatus = New EHSTransactionModel.ExtRefStatusClass(MyBase.SessionHandler.CMSVaccineResultGetFromSession(FunctionCode), udtEHSAccount.SearchDocCode)
            'udtExtRefStatus = New EHSTransactionModel.ExtRefStatusClass(MyBase.SessionHandler.CMSVaccineResultGetFromSession(FunctionCode), udtEHSAccount.EHSPersonalInformationList(0).DocCode)
            ' INT13-0021 - Fix use HKBC on smart IC claim incorrectly [End][Koala]
        End If

        Return udtExtRefStatus
    End Function

    Private Function GetDHVaccineRefStatus(ByVal udtEHSAccount As EHSAccountModel, Optional ByVal strSchemeCode As String = "") As EHSTransactionModel.ExtRefStatusClass
        Dim udtDHVaccineRefStatus As EHSTransactionModel.ExtRefStatusClass = MyBase.SessionHandler.DHExtRefStatusGetFromSession()
        If udtDHVaccineRefStatus Is Nothing Then
            GetVaccinationRecordFromSession(udtEHSAccount, strSchemeCode)

            udtDHVaccineRefStatus = New EHSTransactionModel.ExtRefStatusClass(MyBase.SessionHandler.CIMSVaccineResultGetFromSession(FunctionCode), udtEHSAccount.SearchDocCode)
        End If

        Return udtDHVaccineRefStatus
    End Function

    Private Function CheckLastServiceDate(ByVal dtmServicedate As Date, ByVal udtSchemeClaim As SchemeClaimModel, ByVal udtEHSClaimVaccine As EHSClaimVaccineModel) As Boolean

        ' For changing the ServiceDate to Previous season or not current season
        ' The EHSClaimVaccineModel will be rendered according to the service date
        ' The checking of last Service Date will be applied base on the the servicedate also 
        ' => active udtSchemeClaim on the selected servicedate

        Dim udtValidator As New Validator()
        Dim isValid As Boolean = True
        Dim SystemMessage As SystemMessage = Nothing

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        'If udtEHSClaimVaccine.SchemeSeq <> udtSchemeClaim.SchemeSeq OrElse Not udtEHSClaimVaccine.SchemeCode.Equals(udtSchemeClaim.SchemeCode) Then
        If Not udtEHSClaimVaccine.SchemeCode.Equals(udtSchemeClaim.SchemeCode) Then
            Dim udtSchemeClaimBLL As New SchemeClaimBLL()
            Dim udtSchemeClaimModelPrevious As SchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtSchemeClaim.SchemeCode, dtmServicedate.Date.AddDays(1).AddMinutes(-1))
            If Not udtSchemeClaimModelPrevious Is Nothing Then
                udtSchemeClaim = udtSchemeClaimModelPrevious
            End If
        End If
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        For Each udtEHSClaimVaccineSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
            If isValid AndAlso udtEHSClaimVaccineSubsidize.Selected Then

                ' CRE13-001 - EHAPP [Start][Koala]
                ' -------------------------------------------------------------------------------------
                ' Get the correct SchemeClaim & subsidize by service date
                Dim udtSchemeClaimTemp As SchemeClaimModel = (New SchemeClaimBLL).getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtSchemeClaim.SchemeCode, dtmServicedate.Date.AddDays(1).AddMinutes(-1))
                ' CRE13-001 - EHAPP [End][Koala]

                ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
                ' -----------------------------------------------------------------------------------------
                SystemMessage = udtValidator.chkServiceDataSubsidizeGroupLastServiceData(dtmServicedate, udtSchemeClaimTemp.SubsidizeGroupClaimList.Filter(udtSchemeClaim.SchemeCode, udtEHSClaimVaccineSubsidize.SchemeSeq, udtEHSClaimVaccineSubsidize.SubsidizeCode))
                ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]
                If Not SystemMessage Is Nothing Then
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(SystemMessage)
                End If
            End If
        Next

        Return isValid
    End Function

    'CRE15-003 System-generated Form [Start][Philip Chau]
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
            lblCompleteClaimTransNum.Visible = True
            lblStep3PrefixTransNum.Visible = False

            lblStep3TransactionIDUpdateNoticeBeforeViewLatest.Visible = False
            lblStep3TransactionIDUpdateNoticeAfterViewLatest.Visible = False
            lblLatestTransactionID.Visible = False
            btnStep3ViewLatestTransactionID.Visible = False
            lblHTMLRightPointArrow.Visible = False
        Else
            lblCompleteClaimTransNum.Visible = False
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

    'CRE15-004 (TIV and QIV) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub HandleSystemMessage(ByVal sm As SystemMessage)
        Dim lstStrIdx As List(Of String) = Nothing
        Dim lstStrReplaceMessage As List(Of String) = Nothing

        sm.GetReplaceMessage("%s1", lstStrIdx, lstStrReplaceMessage)

        If lstStrIdx Is Nothing Then
            Me.udcMsgBoxErr.AddMessage(sm)
        Else
            sm.GetReplaceMessage(String.Empty, lstStrIdx, lstStrReplaceMessage)

            Select Case (sm.FunctionCode.ToString + "-" + sm.SeverityCode.ToString + "-" + sm.MessageCode.ToString)
                Case "990000-E-00242"
                    Dim strMessageEng As String
                    Dim strMessageTC As String
                    Dim strMessageSC As String

                    strMessageEng = lstStrReplaceMessage(0) + " " + HttpContext.GetGlobalResourceObject("Text", "ConjunctionAnd", New System.Globalization.CultureInfo(CultureLanguage.English)) + " " + lstStrReplaceMessage(1)
                    strMessageTC = lstStrReplaceMessage(0) + " " + HttpContext.GetGlobalResourceObject("Text", "ConjunctionAnd", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)) + " " + lstStrReplaceMessage(1)
                    strMessageSC = lstStrReplaceMessage(0) + " " + HttpContext.GetGlobalResourceObject("Text", "ConjunctionAnd", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese)) + " " + lstStrReplaceMessage(1)
                    Me.udcMsgBoxErr.AddMessage(sm, New String() {"%en", "%tc", "%sc"}, New String() {strMessageEng, strMessageTC, strMessageSC})
            End Select
        End If
    End Sub
    'CRE15-004 (TIV and QIV) [End][Chris YIM]

    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Sub BindRecipientCondition()
        Dim udtStaticDataBLL As New StaticDataBLL

        Dim udtStaticDataModelCollection As StaticDataModelCollection
        'Dim strSelectedRecipientConditionOption As String = String.Empty
        Dim lstCheckboxListResult As List(Of Boolean) = Nothing
        Dim blnCheckedRecipientConditionOption As Boolean = False
        Dim listItem As ListItem
        Dim blnEnableRecipientCondition As Boolean = False

        udtStaticDataModelCollection = udtStaticDataBLL.GetStaticDataListByColumnName("VSS_RECIPIENTCONDITION")

        If udtStaticDataModelCollection.Count > 0 Then
            If udtStaticDataModelCollection.Count > 1 Then
                Me.rblRecipientCondition.Visible = True
                Me.chkRecipientCondition.Visible = False

                ' Save the User Input before clear it
                If Me.rblRecipientCondition.SelectedIndex > -1 AndAlso String.IsNullOrEmpty(Me.rblRecipientCondition.SelectedValue) = False Then
                    lstCheckboxListResult = New List(Of Boolean)

                    'strSelectedRecipientConditionOption = cblRecipientCondition.SelectedValue
                    For i As Integer = 0 To rblRecipientCondition.Items.Count - 1
                        lstCheckboxListResult.Add(rblRecipientCondition.Items(i).Selected)
                    Next
                End If

                rblRecipientCondition.Items.Clear()

                'rblRecipientCondition.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "NotApplicable"), String.Empty))

                For Each udtStaticData As StaticDataModel In udtStaticDataModelCollection
                    listItem = New ListItem

                    If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
                        listItem.Text = udtStaticData.DataValueChi.ToString
                    ElseIf MyBase.SessionHandler.Language = CultureLanguage.SimpChinese Then
                        listItem.Text = udtStaticData.DataValueCN.ToString
                    Else
                        listItem.Text = udtStaticData.DataValue.ToString
                    End If

                    listItem.Value = udtStaticData.ItemNo

                    rblRecipientCondition.Items.Add(listItem)
                Next

                ' Restore the User Input after clear it
                'If Not String.IsNullOrEmpty(strSelectedRecipientConditionOption) Then
                '    cblRecipientCondition.SelectedValue = strSelectedRecipientConditionOption
                'End If
                If Not lstCheckboxListResult Is Nothing AndAlso lstCheckboxListResult.Count > 0 Then
                    For i As Integer = 0 To rblRecipientCondition.Items.Count - 1
                        rblRecipientCondition.Items(i).Selected = lstCheckboxListResult.Item(i)
                    Next
                End If

            Else
                Me.rblRecipientCondition.Visible = False
                Me.chkRecipientCondition.Visible = True

                ' Save the User Input before clear it
                If Me.chkRecipientCondition.Checked Then
                    blnCheckedRecipientConditionOption = True
                End If

                Dim udtStaticData As StaticDataModel = udtStaticDataModelCollection.Item(0)

                If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
                    chkRecipientCondition.Text = udtStaticData.DataValueChi.ToString
                ElseIf MyBase.SessionHandler.Language = CultureLanguage.SimpChinese Then
                    chkRecipientCondition.Text = udtStaticData.DataValueCN.ToString
                Else
                    chkRecipientCondition.Text = udtStaticData.DataValue.ToString
                End If

                ' Restore the User Input after clear it
                chkRecipientCondition.Checked = blnCheckedRecipientConditionOption

            End If
        End If

    End Sub
    'CRE16-026 (Add PCV13) [End][Chris YIM]

    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Function HighRiskOptionShown(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtEHSClaimVacine As EHSClaimVaccineModel) As String

        Dim strShowHighRiskOption As String = SubsidizeGroupClaimModel.HighRiskOptionClass.HideWithoutInput

        'Check High Risk Option MinDate
        Dim strMinDate As String = String.Empty
        Dim dtmMinDate As Date
        Me._udtCommfunct.getSystemParameter("HighRiskOptionDateBackClaimMinDate", strMinDate, String.Empty, udtEHSTransaction.SchemeCode)

        dtmMinDate = Convert.ToDateTime(strMinDate)

        If udtEHSTransaction.ServiceDate < dtmMinDate Then
            Return strShowHighRiskOption
        End If

        'Check subsidize whether show the High Risk Option
        For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVacine.SubsidizeList
            Select Case udtEHSClaimSubsidize.HighRiskOption
                Case SubsidizeGroupClaimModel.HighRiskOptionClass.ShowForInput
                    strShowHighRiskOption = SubsidizeGroupClaimModel.HighRiskOptionClass.ShowForInput
                    Exit For
                Case SubsidizeGroupClaimModel.HighRiskOptionClass.HideButForceHighRisk
                    strShowHighRiskOption = SubsidizeGroupClaimModel.HighRiskOptionClass.HideButForceHighRisk
            End Select
        Next

        Return strShowHighRiskOption

    End Function
    'CRE16-026 (Add PCV13) [End][Chris YIM]

    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Sub SetHighRisk(ByVal strHighRisk As String)
        If strHighRisk = String.Empty Then Return

        If Me.chkRecipientCondition.Visible Then
            Me.chkRecipientCondition.Checked = IIf(strHighRisk = YesNo.Yes, True, False)
        End If

        If Me.rblRecipientCondition.Visible Then
            Dim strSelectedValue As String = String.Empty

            Select Case strHighRisk
                Case YesNo.Yes
                    strSelectedValue = HighRiskOption.HIGHRISK
                Case YesNo.No
                    strSelectedValue = HighRiskOption.NOHIGHRISK
            End Select

            For i As Integer = 0 To Me.rblRecipientCondition.Items.Count - 1
                If Me.rblRecipientCondition.Items(i).Value = strSelectedValue Then
                    Me.rblRecipientCondition.Items(i).Selected = True
                Else
                    Me.rblRecipientCondition.Items(i).Selected = False
                End If
            Next
        End If
    End Sub
    'CRE16-026 (Add PCV13) [End][Chris YIM]

    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Protected Sub udcClaimVaccineInputText_SubsidizeDisabledRemarkClicked(ByVal sender As Object, ByVal e As EventArgs)
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearVaccineError()

        Dim linkBtn As LinkButton = CType(sender, LinkButton)

        SessionHandler.SubsidizeDisabledDetailKeySaveToSession(linkBtn.Attributes.Item("remark"), FunctionCode)

        ShowSubsidizeDisabledRemarkText(linkBtn.Attributes.Item("remark"))

    End Sub
    'CRE16-026 (Add PCV13) [End][Chris YIM]

    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Protected Sub udcClaimVaccineInputText_SubsidizeCheckboxClickedEnableRecipientCondition(ByVal blnEnabled As Boolean)
        Me.rblRecipientCondition.Enabled = blnEnabled

        If Me.rblRecipientCondition.Enabled Then
            'Enabled
            SetHighRisk(SessionHandler.HighRiskGetFromSession(FunctionCode))
        Else
            'Disabled
            Me.rblRecipientCondition.SelectedIndex = -1
            Me.rblRecipientCondition.SelectedValue = Nothing
            Me.rblRecipientCondition.ClearSelection()

            Me.lblRecipientConditionError.Visible = False

        End If
    End Sub
    'CRE16-026 (Add PCV13) [End][Chris YIM]

    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Function ValidateRecipientCondition(ByVal objMsgBox As CustomControls.TextOnlyMessageBox) As Boolean
        Dim blnResult As Boolean = True

        Me.lblRecipientConditionError.Visible = False

        If String.IsNullOrEmpty(HighRisk()) = True Then
            blnResult = False
            Me.lblRecipientConditionError.Visible = True
            objMsgBox.AddMessage(New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00397)) ' Please select "Recipient Condition"
        End If

        Return blnResult
    End Function
    'CRE16-026 (Add PCV13) [End][Chris YIM]

    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Function HighRisk() As String
        If Me.rblRecipientCondition.Visible Then
            If Me.rblRecipientCondition.SelectedItem Is Nothing Then
                Return String.Empty
            Else
                If Me.rblRecipientCondition.SelectedItem.Value = HighRiskOption.HIGHRISK Then
                    Return YesNo.Yes
                ElseIf Me.rblRecipientCondition.SelectedItem.Value = HighRiskOption.NOHIGHRISK Then
                    Return YesNo.No
                End If

                Return String.Empty
            End If
        Else
            Return IIf(Me.chkRecipientCondition.Checked, YesNo.Yes, YesNo.No)
        End If
    End Function
    'CRE16-026 (Add PCV13) [End][Chris YIM]

    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Sub setupDisplayRecipientCondition(ByVal udtEHSTransaction As EHSTransactionModel, ByRef panRecipientCondition As Panel, ByRef lblRecipientCondition As Label)
        Dim strRecipientCondition As String = String.Empty

        If Not udtEHSTransaction.HighRisk Is Nothing Then
            Select Case udtEHSTransaction.HighRisk
                Case YesNo.Yes
                    strRecipientCondition = HighRiskOption.HIGHRISK
                Case YesNo.No
                    strRecipientCondition = HighRiskOption.NOHIGHRISK
            End Select
        End If

        'Display Recipient Condition
        If strRecipientCondition <> String.Empty Then

            panRecipientCondition.Visible = True

            Dim udtStaticDataModel As StaticDataModel = (New StaticDataBLL).GetStaticDataByColumnNameItemNo("VSS_RECIPIENTCONDITION", strRecipientCondition)

            Select Case SessionHandler.Language
                Case CultureLanguage.TradChinese
                    lblRecipientCondition.Text = udtStaticDataModel.DataValueChi
                Case CultureLanguage.SimpChinese
                    lblRecipientCondition.Text = udtStaticDataModel.DataValueCN
                Case Else
                    lblRecipientCondition.Text = udtStaticDataModel.DataValue
            End Select

        End If

    End Sub
    'CRE16-026 (Add PCV13) [End][Chris YIM]
#End Region

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

#Region "Event"
    Protected Sub btnStep3ViewLatestTransactionID_Click(sender As Object, e As EventArgs) Handles btnStep3ViewLatestTransactionID.Click
        HandleButtonClicked(True)
        MyBase.SessionHandler.EHSClaimStep3ShowLastestTransactionIDSaveToSession(True)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("View latest transaction no.", lblCompleteClaimTransNum.Text)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00088, "Show Transaction ID")
    End Sub

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub btnDocumentaryProofNext_Click(sender As Object, e As EventArgs) Handles btnDocumentaryProofNext.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(Me.FunctionCode)
        Dim udtSelectedCategory As ClaimCategoryModel = Nothing
        Dim strDocumentaryProof As String = String.Empty

        If udtSchemeClaim.SchemeCode = SchemeClaimModel.EnumControlType.VSS.ToString.Trim Then
            udtSelectedCategory = SessionHandler.ClaimCategoryGetFromSession(FunctionCode)
        End If

        Dim isValid As Boolean = True
        ClearDocumentaryProofError()


        If rbDocumentaryProof.Visible Then
            If String.IsNullOrEmpty(Me.rbDocumentaryProof.SelectedValue) Then
                isValid = False
                Me.lblDocumentaryProofError.Visible = True
                Me.udcMsgBoxErr.AddMessage("990000", "E", "00360")
            End If
            strDocumentaryProof = Me.rbDocumentaryProof.SelectedValue
        End If

        'CRE20-009 Validation on the CSSA & Annex checkbox [Start][Nichole]
        If rbDocumentaryProof.Visible Then
            If Me.rbDocumentaryProof.SelectedValue = ucInputVSSDA.VSS_DOCUMENTARYPROOF.VSS_CSSA_CERT Or Me.rbDocumentaryProof.SelectedValue = ucInputVSSDA.VSS_DOCUMENTARYPROOF.VSS_ANNEX_PAGE Then
                If Not chkDocProofCSSA.Checked Then
                    isValid = False
                    Me.lblVSSDAConfirmCSSAError.Visible = True
                    Me.udcMsgBoxErr.AddMessage("990000", "E", "00453")
                Else
                    Me.lblVSSDAConfirmCSSAError.Visible = False
                End If
            End If
        End If


        If rbDocumentaryProof.Visible Then
            If Me.rbDocumentaryProof.SelectedValue = ucInputVSSDA.VSS_DOCUMENTARYPROOF.VSS_CSSA_CERT Or Me.rbDocumentaryProof.SelectedValue = ucInputVSSDA.VSS_DOCUMENTARYPROOF.VSS_ANNEX_PAGE Then
                If Not chkDocProofAnnex.Checked Then
                    isValid = False
                    Me.lblVSSDAConfirmAnnexError.Visible = True
                    Me.udcMsgBoxErr.AddMessage("990000", "E", "00454")
                Else
                    Me.lblVSSDAConfirmAnnexError.Visible = False
                End If
            End If
        End If
        'CRE20-009 Validation on the CSSA & Annex checkbox [End][Nichole]


        If chkDocumentaryProof.Visible Then
            If Not chkDocumentaryProof.Checked Then
                isValid = False
                Me.lblDocumentaryProofError.Visible = True
                Me.udcMsgBoxErr.AddMessage("990000", "E", "00360")
            End If
            strDocumentaryProof = hfDocumentaryProof.Value
        End If

        If isValid Then
            Me._udtEHSTransaction = MyBase.SessionHandler.EHSTransactionGetFromSession(Me.FunctionCode)

            Dim udtTransactAdditionfield As TransactionAdditionalFieldModel

            '-------------------------------------------------
            'Nothing in first adding TransactionAdditionalField
            '-------------------------------------------------
            If Me._udtEHSTransaction.TransactionAdditionFields Is Nothing Then
                Me._udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()
            End If

            '---------------------------------------------------
            'If exist, remove Addition Fields : PlaceVaccination
            '---------------------------------------------------
            If Not Me._udtEHSTransaction.TransactionAdditionFields Is Nothing Then
                udtTransactAdditionfield = _udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.DocumentaryProof)
                If Not udtTransactAdditionfield Is Nothing Then
                    _udtEHSTransaction.TransactionAdditionFields.Remove(udtTransactAdditionfield)
                End If
            End If

            '-------------------------------------------------
            'Set up Addition Fields : PlaceVaccination
            '-------------------------------------------------
            If Not Me._udtEHSTransaction.TransactionAdditionFields Is Nothing Then
                udtTransactAdditionfield = New TransactionAdditionalFieldModel()

                udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
                udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
                udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode.Trim()
                udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.DocumentaryProof
                udtTransactAdditionfield.AdditionalFieldValueCode = strDocumentaryProof
                udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing

                Me._udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

                MyBase.SessionHandler.EHSTransactionSaveToSession(Me._udtEHSTransaction, Me.FunctionCode)
            End If

            Select Case udtSchemeClaim.SchemeCode
                Case SchemeClaimModel.EnumControlType.PIDVSS.ToString.Trim
                    GoToView(ActiveViewIndex.Vaccine)
                Case SchemeClaimModel.EnumControlType.VSS.ToString.Trim
                    Select Case udtSelectedCategory.CategoryCode
                        Case CategoryCode.VSS_PID
                            If Me.rbDocumentaryProof.SelectedItem.Value = PID_DOCUMENTARYPROOF.PID_INSTITUTION_CERT Then
                                GoToView(ActiveViewIndex.RCHCode)
                            Else
                                If SessionHandler.NonClinicSettingGetFromSession(FunctionCode) Then
                                    GoToView(ActiveViewIndex.PlaceVaccination)
                                Else
                                    GoToView(ActiveViewIndex.Vaccine)
                                End If
                            End If

                        Case CategoryCode.VSS_DA
                            If SessionHandler.NonClinicSettingGetFromSession(FunctionCode) Then
                                GoToView(ActiveViewIndex.PlaceVaccination)
                            Else
                                GoToView(ActiveViewIndex.Vaccine)
                            End If

                    End Select

            End Select
        End If

        Me.udcMsgBoxErr.BuildMessageBox(_strValidationFail)
        Me.udcMsgBoxInfo.BuildMessageBox()

    End Sub
    'CRE16-002 (Revamp VSS) [End][Chris YIM]

    'CRE15-005-03 (New PIDVSS scheme) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub btnDocumentaryProofBack_Click(sender As Object, e As EventArgs) Handles btnDocumentaryProofBack.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearDocumentaryProofError()

        Dim udtSchemeClaim As SchemeClaimModel = SessionHandler.SchemeSelectedGetFromSession(FunctionCode)
        Dim udtSelectedCategory As ClaimCategoryModel = SessionHandler.ClaimCategoryGetFromSession(FunctionCode)
        Select Case udtSchemeClaim.SchemeCode
            Case SchemeClaimModel.EnumControlType.PIDVSS.ToString.Trim
                GoToView(ActiveViewIndex.ServiceDate)
            Case SchemeClaimModel.EnumControlType.VSS.ToString.Trim
                Select Case udtSelectedCategory.CategoryCode
                    Case CategoryCode.VSS_PID, CategoryCode.VSS_DA
                        GoToView(ActiveViewIndex.Category)
                End Select
        End Select


    End Sub
    'CRE15-005-03 (New PIDVSS scheme) [End][Chris YIM]

    'CRE15-005-03 (New PIDVSS scheme) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub btnDocumentaryProofCancel_Click(sender As Object, e As EventArgs) Handles btnDocumentaryProofCancel.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearDocumentaryProofError()

        Me.CancelAction()
    End Sub
    'CRE15-005-03 (New PIDVSS scheme) [End][Chris YIM]

    'CRE15-005-03 (New PIDVSS scheme) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub btnDocumentaryProofViewDetail_Click(sender As Object, e As EventArgs) Handles btnDocumentaryProofViewDetail.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearDocumentaryProofError()

        Me.ShowTransactionDetail()
    End Sub
    'CRE15-005-03 (New PIDVSS scheme) [End][Chris YIM]

    'CRE15-005-03 (New PIDVSS scheme) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub btnDocumentaryProofViewVaccinationRecord_Click(sender As Object, e As EventArgs) Handles btnDocumentaryProofViewVaccinationRecord.Click
        EHSClaimBasePage.AuditLogVaccinationRecordClick(_udtAuditLogEntry)

        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearDocumentaryProofError()

        ShowVaccinationRecord(True)
    End Sub
    'CRE15-005-03 (New PIDVSS scheme) [End][Chris YIM]

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub btnPlaceVaccinationNext_Click(sender As Object, e As EventArgs) Handles btnPlaceVaccinationNext.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(Me.FunctionCode)
        'Dim udtClaimCategory As ClaimCategoryModel = Nothing
        'Dim udtSessionClaimCategory As ClaimCategoryModel = Nothing
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = Nothing

        Dim isValid As Boolean = True
        ClearPlaceVaccinationError()

        If String.IsNullOrEmpty(Me.rbPlaceVaccination.SelectedValue) Then
            isValid = False
            Me.lblPlaceVaccinationError.Visible = True
            Me.udcMsgBoxErr.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00385) ' Please select "Place Of Vaccination"
        End If

        If Me.rbPlaceVaccination.SelectedValue = PlaceOfVaccinationOptions.OTHERS Then
            If Me.txtPlaceVaccinationOther.Text = String.Empty Then
                isValid = False
                Me.lblPlaceVaccinationError.Visible = True
                Me.udcMsgBoxErr.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00386) ' Please input "Place Of Vaccination"
            End If

            'Transform to byte (4 byte for each char)
            Dim bytChiName As Byte() = System.Text.Encoding.UTF32.GetBytes(Me.txtPlaceVaccinationOther.Text)
            If bytChiName.Length > 1020 Then 'Maximum 255 char
                isValid = False

                Me.lblPlaceVaccinationError.Visible = True
                Me.udcMsgBoxErr.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00390) ' "Place Of Vaccination" is invalid
            End If
        End If

        If isValid Then
            Me._udtEHSTransaction = MyBase.SessionHandler.EHSTransactionGetFromSession(Me.FunctionCode)

            Dim udtTransactAdditionfield As TransactionAdditionalFieldModel

            '-------------------------------------------------
            'Nothing in first adding TransactionAdditionalField
            '-------------------------------------------------
            If Me._udtEHSTransaction.TransactionAdditionFields Is Nothing Then
                Me._udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()
            End If

            '---------------------------------------------------
            'If exist, remove Addition Fields : PlaceVaccination
            '---------------------------------------------------
            If Not Me._udtEHSTransaction.TransactionAdditionFields Is Nothing Then
                udtTransactAdditionfield = _udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.PlaceVaccination)
                If Not udtTransactAdditionfield Is Nothing Then
                    _udtEHSTransaction.TransactionAdditionFields.Remove(udtTransactAdditionfield)
                End If
            End If

            '-------------------------------------------------
            'Set up Addition Fields : PlaceVaccination
            '-------------------------------------------------
            If Not Me._udtEHSTransaction.TransactionAdditionFields Is Nothing Then
                udtTransactAdditionfield = New TransactionAdditionalFieldModel()

                udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
                udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
                udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode.Trim()
                udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.PlaceVaccination
                udtTransactAdditionfield.AdditionalFieldValueCode = Me.rbPlaceVaccination.SelectedItem.Value
                udtTransactAdditionfield.AdditionalFieldValueDesc = IIf(Me.rbPlaceVaccination.SelectedItem.Value = PlaceOfVaccinationOptions.OTHERS, txtPlaceVaccinationOther.Text.Trim, Nothing)

                Me.SessionHandler.PlaceVaccinationSaveToSession(Me.rbPlaceVaccination.SelectedItem.Value, Me.FunctionCode(), udtSchemeClaim.SchemeCode)

                If Me.rbPlaceVaccination.SelectedItem.Value = PlaceOfVaccinationOptions.OTHERS Then
                    Me.SessionHandler.PlaceVaccinationOtherSaveToSession(Me.txtPlaceVaccinationOther.Text.Trim, Me.FunctionCode(), udtSchemeClaim.SchemeCode)
                Else
                    Me.SessionHandler.PlaceVaccinationOtherRemoveFromSession(Me.FunctionCode())
                End If

                Me._udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
                MyBase.SessionHandler.EHSTransactionSaveToSession(Me._udtEHSTransaction, Me.FunctionCode)
            End If

            Select Case udtSchemeClaim.SchemeCode
                Case SchemeClaimModel.EnumControlType.VSS.ToString.Trim, SchemeClaimModel.EnumControlType.ENHVSSO.ToString.Trim
                    GoToView(ActiveViewIndex.Vaccine)
            End Select
        End If

        Me.udcMsgBoxErr.BuildMessageBox(_strValidationFail)
        Me.udcMsgBoxInfo.BuildMessageBox()

    End Sub
    'CRE16-002 (Revamp VSS) [End][Chris YIM]

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub rbPlaceVaccination_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbPlaceVaccination.SelectedIndexChanged
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        Dim udtEHSTrans As EHSTransactionModel = SessionHandler.EHSTransactionGetFromSession(FunctionCode)

        Dim udtRadioButtonList As RadioButtonList = CType(sender, RadioButtonList)
        If udtRadioButtonList.SelectedValue = PlaceOfVaccinationOptions.OTHERS Then
            lblPlaceVaccinationOther.Enabled = True
            txtPlaceVaccinationOther.Enabled = True

            If Not SessionHandler.PlaceVaccinationOtherGetFromSession(FunctionCode, udtEHSTrans.SchemeCode) Is Nothing Then
                txtPlaceVaccinationOther.Text = SessionHandler.PlaceVaccinationOtherGetFromSession(FunctionCode, udtEHSTrans.SchemeCode)
            End If
        Else
            lblPlaceVaccinationOther.Enabled = False
            txtPlaceVaccinationOther.Enabled = False
            txtPlaceVaccinationOther.Text = String.Empty
        End If

    End Sub
    'CRE16-002 (Revamp VSS) [End][Chris YIM]

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub btnPlaceVaccinationBack_Click(sender As Object, e As EventArgs) Handles btnPlaceVaccinationBack.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearPlaceVaccinationError()

        Dim udtEHSTrans As EHSTransactionModel = SessionHandler.EHSTransactionGetFromSession(FunctionCode)

        Dim udtTransactionAdditionField As TransactionAdditionalFieldModel = udtEHSTrans.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.DocumentaryProof)
        Dim blnPIDCert As Boolean = False

        If Not udtTransactionAdditionField Is Nothing AndAlso udtTransactionAdditionField.AdditionalFieldValueCode = PID_DOCUMENTARYPROOF.PID_INSTITUTION_CERT Then
            blnPIDCert = True
        End If


        Dim udtSelectedCategory As ClaimCategoryModel = SessionHandler.ClaimCategoryGetFromSession(FunctionCode)

        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Select Case udtSelectedCategory.CategoryCode
            Case CategoryCode.VSS_PW, CategoryCode.VSS_CHILD, CategoryCode.VSS_ELDER, CategoryCode.VSS_ADULT, CategoryCode.EVSSO_CHILD
                GoToView(ActiveViewIndex.Category)

            Case CategoryCode.VSS_PID
                If blnPIDCert Then
                    GoToView(ActiveViewIndex.RCHCode)
                Else
                    GoToView(ActiveViewIndex.DocumentaryProof)
                End If

            Case CategoryCode.VSS_DA
                GoToView(ActiveViewIndex.DocumentaryProof)
        End Select
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]


    End Sub
    'CRE16-002 (Revamp VSS) [End][Chris YIM]

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub btnPlaceVaccinationCancel_Click(sender As Object, e As EventArgs) Handles btnPlaceVaccinationCancel.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearPlaceVaccinationError()

        Me.CancelAction()
    End Sub
    'CRE16-002 (Revamp VSS) [End][Chris YIM]

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub btnPlaceVaccinationViewDetail_Click(sender As Object, e As EventArgs) Handles btnPlaceVaccinationViewDetail.Click
        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearPlaceVaccinationError()

        Me.ShowTransactionDetail()
    End Sub
    'CRE16-002 (Revamp VSS) [End][Chris YIM]

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub btnPlaceVaccinationViewVaccinationRecord_Click(sender As Object, e As EventArgs) Handles btnPlaceVaccinationViewVaccinationRecord.Click
        EHSClaimBasePage.AuditLogVaccinationRecordClick(_udtAuditLogEntry)

        If MyBase.IsConcurrentBrowser Then
            HandleConcurrentBrowser()
            Return
        End If

        If MyBase.IsPageRefreshed Then
            Me.HandlePageRefreshed()
            Return
        End If

        ClearPlaceVaccinationError()

        ShowVaccinationRecord(True)
    End Sub
    'CRE16-002 (Revamp VSS) [End][Chris YIM]

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

#End Region


    Private Sub rbDocumentaryProof_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbDocumentaryProof.SelectedIndexChanged
        'CRE20-009 The CSSA and Annex checkbox and label setting [Start][Nichole]
        If rbDocumentaryProof.SelectedValue = ucInputVSSDA.VSS_DOCUMENTARYPROOF.VSS_CSSA_CERT Or rbDocumentaryProof.SelectedValue = ucInputVSSDA.VSS_DOCUMENTARYPROOF.VSS_ANNEX_PAGE Then
            'chkDocProofCSSA.Visible = True
            'chkDocProofAnnex.Visible = True
            'lblDocProofAnnex.Visible = True
            'lblDocProofCSSA.Visible = True
            panVSSDAConfirm.Visible = True
        Else
            'chkDocProofCSSA.Visible = False
            'chkDocProofAnnex.Visible = False
            'lblDocProofAnnex.Visible = False
            'lblDocProofCSSA.Visible = False
            panVSSDAConfirm.Visible = False
        End If

        'CRE20-009 The CSSA and Annex checkbox and label setting [End][Nichole]
    End Sub
End Class