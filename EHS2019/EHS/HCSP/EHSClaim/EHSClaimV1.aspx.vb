Imports AjaxControlToolkit
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.ClaimCategory
Imports Common.Component.ClaimRules
Imports Common.Component.ClaimRules.ClaimRulesBLL
Imports Common.Component.DataEntryUser
Imports Common.Component.DocType
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.EHSTransaction
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.Scheme
Imports Common.Component.SchemeDetails
Imports Common.Component.ServiceProvider
Imports Common.Component.UserAC
Imports Common.Component.VoucherScheme
Imports Common.Component.InputPicker
Imports Common.Component.ReasonForVisit
Imports Common.Format
Imports Common.OCSSS
Imports Common.Validation
Imports Common.WebService.Interface
Imports HCSP.BLL
Imports HCSP.UIControl.EHCClaimText
Imports System.Net
Imports System.Web.Services
Imports Common.Component.VoucherInfo
Imports Common.Component.HAServicePatient
Imports System.Linq

<System.Web.Script.Services.ScriptService()> _
Partial Public Class EHSClaimV1
    Inherits BasePageWithGridView

    Private _udtClaimVoucherBLL As BLL.ClaimVoucherBLL = New BLL.ClaimVoucherBLL()
    Private _udtEHSClaimBLL As BLL.EHSClaimBLL = New BLL.EHSClaimBLL()
    Private _udtEHSAccountBll As EHSAccountBLL = New EHSAccountBLL()
    Private _udtEHSTransactionBLL As EHSTransactionBLL = New EHSTransactionBLL()
    Private _udtPracticeAcctBLL As BLL.PracticeBankAcctBLL = New BLL.PracticeBankAcctBLL()
    Private _udtSessionHandler As New SessionHandler
    Private _udtSP As ServiceProviderModel = Nothing
    Private _udtUserAC As UserACModel = New UserACModel()
    Private _udtSystemMessage As SystemMessage
    Private _udtEHSTransaction As EHSTransactionModel
    Private _udtGeneralFunction As New GeneralFunction
    Private _strValidationFail As String = "ValidationFail"
    Private _udtFormatter As New Formatter
    Private _blnIsRequireHandlePageRefresh As Boolean = False
    Private _udtClaimCategoryBLL As ClaimCategoryBLL = New ClaimCategoryBLL()
    Private _udtSchemeClaimBLL As New SchemeClaimBLL
    Private _udtAuditLogEntry As AuditLogEntry = Nothing

    Public FunctCode As String = String.Empty   'CRE20-0xx for COVID19 claim [Nichole]
    Public Const ValidatedServiceDate As String = "ValidatedServiceDate"

    Private _blnConcurrentUpdate As Boolean = False

#Region "Private Class"
    Private Class PrintOptionValue
        Public Const FullChi As String = "FullChi"
        Public Const FullEng As String = "FullEng"
        Public Const CondensedChi As String = "CondensedChi"
        Public Const CondensedEng As String = "CondensedEng"

    End Class

    Protected Class PrintOptionLanguage
        Public Const TradChinese As String = "ZH"
        Public Const SimpChinese As String = "CN"
        Public Const English As String = "EN"
    End Class

    Private Class ActiveViewIndex
        'Search Account
        Public Const Step1 As Integer = 0

        'Enter Claim Details
        Public Const Step2a As Integer = 1

        'Confirm Claim Details
        Public Const Step2b As Integer = 2

        'Conplete Claim
        Public Const Step3 As Integer = 3

        'Select Practice
        Public Const SelectPractice As Integer = 4

        'EHS Claim Error
        Public Const EHSClaimError As Integer = 5
    End Class

    Private Class PopupStatusClass
        Public Const Active As String = "A"
        Public Const Closed As String = "C"
    End Class

    Private Class VS
        Public Const VaccinationRecordPopupStatus As String = "VaccinationRecordPopupStatus"
        Public Const VaccinationRecordProviderPopupStatus As String = "VaccinationRecordProviderPopupStatus"
    End Class

#End Region

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        'AddHandler ucIDEASCombo.ShowResult, AddressOf ShowResult
    End Sub

    Private Sub ShowResult()
        'If Not Me.ReadSmartID(udtSmartIDContent) Then
        '    EHSClaimBasePage.AuditLogPageLoad(New AuditLogEntry(FunctionCode, Me), True, False)
        '    'Step 6 of Page Load : Practice Selected -> go to search page
        '    Me.Clear()
        '    Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
        'End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'CRE20-0xx Immue record [Start][Nichole]
        FunctCode = Me.ClaimFunctCode
		'CRE20-0xx Immue record [End][Nichole]
		   
        _udtAuditLogEntry = New AuditLogEntry(FunctCode, Me)

        ' Get Current User Account for check Session Expired
        _udtUserAC = UserACBLL.GetUserAC

        MyBase.FunctionCode = FunctCode

        If Not IsPostBack Then
            Dim udtSmartIDContent As SmartIDContentModel = _udtSessionHandler.SmartIDContentGetFormSession(FunctCode)

            ' Get Current User Account
            Dim udtDataEntry As DataEntryUserModel = Nothing
            GetCurrentUserAccount(_udtSP, udtDataEntry, True)

            SetupSchemeLogo()

            'Step 1 of Page Load : if no Selected Practice in session, go to Practice selection Page
            Dim udtSelectedPracticeDisplay As PracticeDisplayModel = _udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)

            If udtSelectedPracticeDisplay Is Nothing Then
                PageLoadPracticeSetup(_udtSP, udtDataEntry)

            Else
                If Me._udtSessionHandler.AccountCreationProceedClaimGetFromSession() Then
                    ' To Handle Concurrent Browser:
                    Me.EHSClaimTokenNumAssign()
                    'EHSClaimBasePage.AuditLogPageLoad(FunctCode, True, True)
                    EHSClaimBasePage.AuditLogEnterClaimDetailLoaded(New AuditLogEntry(FunctionCode, Me))

                    'Step 5 of Page Load : if is come from Account creation and user pressed "proceed to claim" -> go to Enter Claim Detail
                    Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step2a
                    'remove session
                    Me._udtSessionHandler.AccountCreationProceedClaimRemoveFromSession()

                ElseIf CheckFromVaccinationRecordEnquiry() Then
                    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                    ' ----------------------------------------------------------
                    Me._udtSessionHandler.HKICSymbolRemoveFromSession(FunctCode)
                    Me._udtSessionHandler.OCSSSRefStatusRemoveFromSession(FunctionCode)
                    Me._udtSessionHandler.UIDisplayHKICSymbolRemoveFromSession(FunctCode)
                    ' CRE17-010 (OCSSS integration) [End][Chris YIM]

                    EHSClaimBasePage.AuditLogPageLoad(New AuditLogEntry(FunctionCode, Me), True, False)

                    Me.EHSClaimTokenNumAssign()

                    mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1

                    ' If criteria are fulfilled, auto search EHS Account
                    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                    ' ----------------------------------------------------------
                    ' 1. Scheme is selected; AND
                    ' 2a. Scheme do not check OCSSS; OR
                    ' 2b. (i)Scheme check OCSSS AND (ii)DocTypc == HKIC AND (iii)Residential Status have value

                    If Not IsNothing(_udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)) Then
                        If Not Common.OCSSS.OCSSSServiceBLL.EnableHKICSymbolInput(_udtSessionHandler.SchemeSelectedGetFromSession(FunctCode).SchemeCode) Or _
                            Not _udtSessionHandler.EHSAccountGetFromSession(FunctCode).SearchDocCode = DocTypeModel.DocTypeCode.HKIC Then

                            udcClaimSearch_SearchButtonClick(Nothing, Nothing)
                        Else
                            If Not IsNothing(_udtSessionHandler.HKICSymbolGetFormSession(FunctCode)) Then
                                udcClaimSearch_SearchButtonClick(Nothing, Nothing)
                            End If
                        End If
                    End If

                    ' CRE17-010 (OCSSS integration) [End][Chris YIM]

                Else
                    '==================================================================== Code for SmartID ============================================================================
                    If Not Me.ReadSmartID(udtSmartIDContent) Then
                        EHSClaimBasePage.AuditLogPageLoad(New AuditLogEntry(FunctionCode, Me), True, False)
                        'Step 6 of Page Load : Practice Selected -> go to search page
                        Me.Clear()
                        Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
                    End If
                    '==================================================================================================================================================================
                End If

            End If

            preventMultiImgClick(Me.ClientScript, Me.btnStep2bConfirm)
            If Not IsNothing(Me.udcClaimSearch.FindControl("btnShortIdentityNoSmartID")) Then
                preventMultiImgClick(Me.ClientScript, Me.udcClaimSearch.FindControl("btnShortIdentityNoSmartID"))
            End If

            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            If Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1 Then
                Me._udtSessionHandler.HKICSymbolRemoveFromSession(FunctCode)
                Me._udtSessionHandler.OCSSSRefStatusRemoveFromSession(FunctionCode)
                Me._udtSessionHandler.UIDisplayHKICSymbolRemoveFromSession(FunctCode)
            End If
            ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        Else
            ' Page is posted back
            Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)

            'Step1
            Select Case Me.mvEHSClaim.ActiveViewIndex
                Case ActiveViewIndex.Step1
                    're-gen all with controls
                    Me.SetupStep1(True, False)

                Case ActiveViewIndex.Step2a
                    Me.SetupStep2a(udtEHSAccount, True, True, False)
                    If ViewState(VS.VaccinationRecordPopupStatus) <> PopupStatusClass.Active Then
                        Me._ScriptManager.SetFocus(Me.btnStep2aClaim)
                    End If

                    If udcMsgBoxInfo.Visible OrElse udcMsgBoxErr.Visible Then
                        Dim strJS As String = String.Empty
                        Dim guidText As String = Guid.NewGuid().ToString()

                        strJS += "setTimeout('window.scrollTo(0,210)', 0);"
                        strJS += "RemoveUsedBlockScript('" & guidText & "');"

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", strJS, True)
                    End If

                Case ActiveViewIndex.Step2b
                    Me.SetupStep2b(udtEHSAccount, False)
                    'Me._ScriptManager.SetFocus(Me.chkStep2bDeclareClaim)
                    'Me._ScriptManager.SetFocus(Me.btnStep2bConfirm)
                    'Me._ScriptManager.SetFocus(Me.btnStep2bBack)

                Case ActiveViewIndex.Step3
                    Me.SetupStep3(udtEHSAccount, False)

                    'CRE20-0022 (Immu record) [Start][Raiman]
                    'Focus Printed page for covid19 reprint button if IsClaimCOVID19 is true
                    If ClaimMode <> ClaimMode.COVID19 Then
                        Me._ScriptManager.SetFocus(Me.btnStep3NextClaim)
                    End If
                    'CRE20-0022 (Immu record) [End][Raiman]

                Case ActiveViewIndex.SelectPractice
                    Me.SetupSelectPractice()

                Case ActiveViewIndex.EHSClaimError
                    Me.SetupInternalError()
            End Select
        End If

        If _blnIsRequireHandlePageRefresh Then

            'Sometimes is Multiple browser case
            If Me.EHSClaimTokenNumValidation(True) Then
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
                Me._udtSessionHandler.EHSClaimSessionRemove(FunctCode)
            End If
        End If

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Me.ModalPopupConfirmCancel.PopupDragHandleControlID = Me.ucNoticePopUpConfirm.Header.ClientID
        Me.ModalPopupConfirmCancel.CancelControlID = Me.ucNoticePopUpConfirm.ButtonCancel.ClientID
        Me.ModalPopupExclamationConfirmationBox.PopupDragHandleControlID = Me.ucNoticePopUpExclamationConfirm.Header.ClientID
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Koala]

        ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [Start][Winnie]
        Me.ModelPopupDuplicateClaimAlert.PopupDragHandleControlID = Me.ucNoticePopUpDuplicateClaimAlert.Header.ClientID
        ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [End][Winnie]
    End Sub

    Private Sub EHSClaim_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Response.Cache.SetNoStore()
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetExpires(Now.AddDays(-1))

    End Sub

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        SetupTimeline()

        If Me.ClaimMode = ClaimMode.COVID19 Then

            Me.imgHeaderClaimVoucher.AlternateText = HttpContext.GetGlobalResourceObject("AlternateText", "ClaimCOVID19Banner")
            Me.imgHeaderClaimVoucher.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "ClaimCOVID19Banner")

            Me.lblStep2aClaimInfo.Text = Me.GetGlobalResourceObject("Text", "VaccineInfo")
            Me.lblStep2aServiceDateText.Text = HttpContext.GetGlobalResourceObject("Text", "InjectionDate")
            Me.btnStep2aClaim.AlternateText = HttpContext.GetGlobalResourceObject("AlternateText", "SaveDisableBtn")
            Me.btnStep2aClaim.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "SaveDisableBtn")

            Me.lblStep2bClaimInfo.Text = Me.GetGlobalResourceObject("Text", "VaccineInfo")
            Me.lblStep2bServiceDateText.Text = HttpContext.GetGlobalResourceObject("Text", "InjectionDate")

            Me.lblStep3ClaimInfo.Text = HttpContext.GetGlobalResourceObject("Text", "VaccineInfo")
            Me.lblStep3ServiceDateText.Text = HttpContext.GetGlobalResourceObject("Text", "InjectionDate")
            Me.btnStep3NextClaim.AlternateText = HttpContext.GetGlobalResourceObject("AlternateText", "NextRecipientBtn")
            Me.btnStep3NextClaim.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "NextRecipientBtn")

        Else

            Me.imgHeaderClaimVoucher.AlternateText = HttpContext.GetGlobalResourceObject("AlternateText", "ClaimVoucherBanner")
            Me.imgHeaderClaimVoucher.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "ClaimVoucherBanner")

            Me.lblStep2aClaimInfo.Text = HttpContext.GetGlobalResourceObject("Text", "ClaimInfo")
            Me.lblStep2aServiceDateText.Text = HttpContext.GetGlobalResourceObject("Text", "ServiceDate")
            Me.btnStep2aClaim.AlternateText = HttpContext.GetGlobalResourceObject("AlternateText", "ClaimBtn")
            If btnStep2aClaim.Enabled Then
                Me.btnStep2aClaim.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "ClaimBtn")
            Else
                Me.btnStep2aClaim.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "ClaimDisableBtn")
            End If

            Me.lblStep2bClaimInfo.Text = Me.GetGlobalResourceObject("Text", "ClaimInfo")
            Me.lblStep2bServiceDateText.Text = HttpContext.GetGlobalResourceObject("Text", "ServiceDate")

            Me.lblStep3ClaimInfo.Text = HttpContext.GetGlobalResourceObject("Text", "ClaimInfo")
            Me.lblStep3ServiceDateText.Text = HttpContext.GetGlobalResourceObject("Text", "ServiceDate")
            Me.btnStep3NextClaim.AlternateText = HttpContext.GetGlobalResourceObject("AlternateText", "NextClaimBtn")
            Me.btnStep3NextClaim.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "NextClaimBtn")

        End If

    End Sub

    Protected Sub mvEHSClaim_ActiveViewChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Dim udtFormatter As Formatter = New Formatter()
        Dim udtEHSTransaction As EHSTransactionModel = Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)
        Dim strHightLight As String = "highlightTimelineLast"
        Dim strUnhightLight As String = "unhighlightTimelineLast"
        Dim strDummy As String = String.Empty
        Dim strClaimDayLimit As String = String.Empty
        Me.udcMsgBoxErr.Clear()
        Me.udcMsgBoxInfo.Clear()

        Me._udtSessionHandler.EHSClaimStepsSaveToSession(FunctCode, Me.mvEHSClaim.ActiveViewIndex)

        'Reset Time Line
        Me.panClaimValidatedTimeline.Visible = True
        Me.panClaimValidatedTimelineStep1.CssClass = strUnhightLight
        Me.panClaimValidatedTimelineStep2.CssClass = strUnhightLight
        Me.panClaimValidatedTimelineStep3.CssClass = strUnhightLight

        Select Case Me.mvEHSClaim.ActiveViewIndex
            Case ActiveViewIndex.Step1
                Me.panClaimValidatedTimelineStep1.CssClass = strHightLight
                Me.EHSClaimTokenNumAssign()

                If _udtSessionHandler.SmartIDContentGetFormSession(FunctCode) Is Nothing Then
                    If _udtSessionHandler.ForceToReadSmartICGetFormSession Is Nothing Then
                        _udtSessionHandler.ForceToReadSmartICSaveToSession(True)
                    End If
                End If

                're-gen all with controls
                Me.udcStep1DocumentTypeRadioButtonGroup.SelectedValue = String.Empty
                Me.Step1Reset(False)
                Me.SetupStep1(True, True)

            Case ActiveViewIndex.Step2a
                Me.panClaimValidatedTimelineStep2.CssClass = strHightLight

                Dim dtmServiceDate As Date = Me._udtGeneralFunction.GetSystemDateTime().Date

                'setup Service date
                If udtEHSTransaction Is Nothing Then
                    Dim udtSubPlatformBLL As New SubPlatformBLL

                    Me.txtStep2aServiceDate.Text = udtFormatter.formatInputTextDate(dtmServiceDate, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
                    Me.txtStep2aServiceDate.Attributes(ValidatedServiceDate) = udtFormatter.formatInputDate(Me.txtStep2aServiceDate.Text, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))

                    Me._udtGeneralFunction.getSystemParameter("DateBackClaimDayLimit", strClaimDayLimit, strDummy)
                    Me.Step2aCalendarExtenderServiceDate.Format = udtFormatter.EnterDateFormat(udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
                    Me.Step2aCalendarExtenderServiceDate.StartDate = DateAdd(DateInterval.Day, (CInt(strClaimDayLimit) - 1) * -1, dtmServiceDate)
                    Me.Step2aCalendarExtenderServiceDate.EndDate = dtmServiceDate

                End If

                _udtSessionHandler.EligibleResultVSSReminderRemoveFromSession()

                Me.SetupStep2a(udtEHSAccount, True, False, True)

                'After Me.SetupStep2a(udtEHSAccount, True, False, True), SchemeSchemeModel is not nothing
                Me.Step2aCleanSchemeErrorImage()

                Me._ScriptManager.SetFocus(Me.btnStep2aClaim)

                If ClaimMode = Common.Component.ClaimMode.COVID19 Then
                    'Set the scroll height in load enter claim datail page, for view the claim in one page
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "setTimeout('window.scrollTo(0,240)', 0);", True)

                    'Generate JavaScript Vaccine Lot No. mapping
                    Dim udtSelectedPracticeDisplay As PracticeDisplayModel = Me._udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)
                    Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
                    Dim udtSP As Common.Component.ServiceProvider.ServiceProviderModel = Nothing
                    Dim udtDataEntry As Common.Component.DataEntryUser.DataEntryUserModel = Nothing
                    Me._udtSessionHandler.CurrentUserGetFromSession(udtSP, udtDataEntry)

                    Dim strVaccineLotNoMappingJavaScript As String = String.Empty

                    If udtSchemeClaim IsNot Nothing Then
                        _udtSessionHandler.ClaimCOVID19MainCategoryRemoveFromSession(FunctCode)
                        _udtSessionHandler.ClaimCOVID19SubCategoryRemoveFromSession(FunctCode)

                        Select Case udtSchemeClaim.ControlType
                            Case SchemeClaimModel.EnumControlType.COVID19, SchemeClaimModel.EnumControlType.COVID19RVP, SchemeClaimModel.EnumControlType.COVID19OR
                                strVaccineLotNoMappingJavaScript = Me.GetVaccineLotNoMappingForCentreJavaScript(udtSP.SPID, udtSelectedPracticeDisplay.PracticeID, dtmServiceDate)

                                '_udtSessionHandler.ClaimCOVID19VaccineLotNoRemoveFromSession(FunctCode)

                                If udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.COVID19OR Then
                                    'Convert sub-category -> Dictionary -> Json
                                    Dim udtCOVID19BLL As New COVID19.COVID19BLL
                                    Dim strSubCategoryMappingJavaScript As String = String.Empty
                                    Dim strCategoryJson As String = String.Empty
                                    Dim strCategoryTextEngJson As String = String.Empty
                                    Dim strCategoryTextChiJson As String = String.Empty

                                    udtCOVID19BLL.GenerateCategoryJson(strCategoryJson, strCategoryTextEngJson, strCategoryTextChiJson)

                                    'Generate JavaScript
                                    strSubCategoryMappingJavaScript = udtCOVID19BLL.GenerateCategoryMappingJavaScript(strCategoryJson, strCategoryTextEngJson, strCategoryTextChiJson)
                                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "COVID19_SubCategory_Mapping", strSubCategoryMappingJavaScript, True)

                                End If

                            Case SchemeClaimModel.EnumControlType.VSS
                                strVaccineLotNoMappingJavaScript = Me.GetVaccineLotNoMappingForPrivateJavaScript(dtmServiceDate)

                                '_udtSessionHandler.ClaimCOVID19VaccineLotNoRemoveFromSession(FunctCode)

                                'Convert sub-category -> Dictionary -> Json
                                Dim udtCOVID19BLL As New COVID19.COVID19BLL
                                Dim strSubCategoryMappingJavaScript As String = String.Empty
                                Dim strCategoryJson As String = String.Empty
                                Dim strCategoryTextEngJson As String = String.Empty
                                Dim strCategoryTextChiJson As String = String.Empty

                                udtCOVID19BLL.GenerateCategoryJson(strCategoryJson, strCategoryTextEngJson, strCategoryTextChiJson)

                                'Generate JavaScript
                                strSubCategoryMappingJavaScript = udtCOVID19BLL.GenerateCategoryMappingJavaScript(strCategoryJson, strCategoryTextEngJson, strCategoryTextChiJson)
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "COVID19_SubCategory_Mapping", strSubCategoryMappingJavaScript, True)

                            Case SchemeClaimModel.EnumControlType.RVP
                                strVaccineLotNoMappingJavaScript = Me.GetVaccineLotNoMappingForRCHJavaScript(dtmServiceDate)

                                '_udtSessionHandler.ClaimCOVID19VaccineLotNoRemoveFromSession(FunctCode)

                                'Convert sub-category -> Dictionary -> Json
                                Dim udtCOVID19BLL As New COVID19.COVID19BLL
                                Dim strSubCategoryMappingJavaScript As String = String.Empty
                                Dim strCategoryJson As String = String.Empty
                                Dim strCategoryTextEngJson As String = String.Empty
                                Dim strCategoryTextChiJson As String = String.Empty

                                udtCOVID19BLL.GenerateCategoryJson(strCategoryJson, strCategoryTextEngJson, strCategoryTextChiJson)

                                'Generate JavaScript
                                strSubCategoryMappingJavaScript = udtCOVID19BLL.GenerateCategoryMappingJavaScript(strCategoryJson, strCategoryTextEngJson, strCategoryTextChiJson)
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "COVID19_SubCategory_Mapping", strSubCategoryMappingJavaScript, True)

                        End Select

                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "COVID19_Vaccine_LotNo_Mapping", strVaccineLotNoMappingJavaScript, True)
                    End If


                End If

            Case ActiveViewIndex.Step2b
                Me.panClaimValidatedTimelineStep2.CssClass = strHightLight
                Me.SetupStep2b(udtEHSAccount, True)

                Me._ScriptManager.SetFocus(Me.btnStep2bBack)
                'Me._ScriptManager.SetFocus(Me.chkStep2bDeclareClaim)
                'Me._ScriptManager.SetFocus(Me.btnStep2bConfirm)

            Case ActiveViewIndex.Step3
                _udtSessionHandler.EligibleResultVSSReminderRemoveFromSession()
                _udtSessionHandler.ClaimCOVID19MainCategoryRemoveFromSession(FunctCode)
                _udtSessionHandler.ClaimCOVID19SubCategoryRemoveFromSession(FunctCode)

                Me.panClaimValidatedTimelineStep3.CssClass = strHightLight
                Me.SetupStep3(udtEHSAccount, True)
                Me._ScriptManager.SetFocus(Me.udcMsgBoxInfo)

            Case ActiveViewIndex.SelectPractice
                Me.panClaimValidatedTimeline.Visible = False
                Me.SetupSelectPractice()

            Case ActiveViewIndex.EHSClaimError
                Me.panClaimValidatedTimeline.Visible = False
                Me.SetupInternalError()

        End Select
    End Sub

    Private Sub SetupAvailSchemeDropDownList(ByVal practiceSchemeInfoList As PracticeSchemeInfoModelCollection, ByVal schemeDropDown As DropDownList)
        Dim schemeListItem As ListItem

        For Each practiceSchemeInfo As PracticeSchemeInfoModel In practiceSchemeInfoList.Values
            schemeListItem = New ListItem
            schemeListItem.Value = practiceSchemeInfo.SchemeCode
        Next
    End Sub

    '==================================================================== Code for SmartID ============================================================================
    Private Function ReadSmartID(ByVal udtSmartIDContent As BLL.SmartIDContentModel) As Boolean
        If IsNothing(udtSmartIDContent) OrElse udtSmartIDContent.IsReadSmartID = False OrElse udtSmartIDContent.IsEndOfReadSmartID Then Return False

        Dim isReadingSmartID As Boolean = False

        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(FunctCode, Me)
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        Dim ideasBLL As BLL.IdeasBLL = New BLL.IdeasBLL
        Dim strIdeasVersion As String = ideasBLL.ConvertIdeasVersion(udtSmartIDContent.IdeasVersion)

        'Write Start Audit log
        EHSClaimBasePage.AuditLogRedirectFormIDEAS(udtAuditLogEntry, udtSchemeClaim.SchemeCode, strIdeasVersion)

        isReadingSmartID = True
        udtSmartIDContent.IsEndOfReadSmartID = True
        Me._udtSessionHandler.SmartIDContentSaveToSession(FunctCode, udtSmartIDContent)
        udtSmartIDContent = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode)

        '--------------------------------------------------------------------------------------------------------------------------------------------------
        ' Smart ID Form Ideas
        '--------------------------------------------------------------------------------------------------------------------------------------------------
        Dim ideasHelper As IdeasRM.IHelper = IdeasRM.HelpFactory.createHelper()

        '--------------------------------------------------------------------------------------------------------------------------------------------------
        ' Get CFD
        '--------------------------------------------------------------------------------------------------------------------------------------------------
        Dim udtAuditLogEntry_GetCFD As AuditLogEntry = New AuditLogEntry(FunctCode, Me)

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

        If strArtifact Is Nothing OrElse strArtifact = String.Empty Then
            '----------------------------- Error Handling -----------------------------------------------

            ' Error100 - 113
            If Not Request.QueryString("status") Is Nothing Then
                Dim strErrorCode As String = Request.QueryString("status").Trim()
                Dim strErrorMsg As String = IdeasRM.ErrorMessageMapper.MapMAStatus(strErrorCode)
                If Not strErrorMsg Is Nothing Then
                    _udtSessionHandler.ForceToReadSmartICSaveToSession(False)

                    Me.Clear()
                    Me.udcStep1DocumentTypeRadioButtonGroup.SelectedValue = DocTypeModel.DocTypeCode.HKIC
                    Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
                    Me.udcMsgBoxErr.AddMessageDesc(FunctCode, strErrorCode, strErrorMsg)

                    'Write End Audit log
                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    'EHSClaimBasePage.AuditLogGetCFDFail(udtAuditLogEntry_GetCFD, ideasBLL.Artifact, strErrorCode, strErrorMsg)
                    EHSClaimBasePage.AuditLogGetCFDFail(udtAuditLogEntry_GetCFD, ideasBLL.Artifact, strErrorCode, strErrorMsg, strIdeasVersion)
                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
                    Me.udcMsgBoxErr.BuildMessageDescBox("SmartIDActionFail", udtAuditLogEntry_GetCFD, Common.Component.LogID.LOG00063, "Get CFD Fail")

                    isValid = False

                End If
            End If
        End If

        If isValid Then

            If ideasSamlResponse.StatusCode.Equals("samlp:Success") Then
                EHSClaimBasePage.AuditLogGetCFDComplete(udtAuditLogEntry_GetCFD, ideasBLL.Artifact)

                '[Dim udtPersonalInfo As EHSAccountModel.EHSPersonalInformationModel
                Dim udtEHSAccountExist As EHSAccountModel = Nothing
                Dim blnNotMatchAccountExist As Boolean = False
                Dim blnExceedDocTypeLimit As Boolean = False
                Dim udtEligibleResult As EligibleResult = Nothing
                Dim goToCreation As Boolean = True
                Dim strError As String = String.Empty

                Try
                    If udtSmartIDContent.IsDemonVersion Then
                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                        ' ----------------------------------------------------------------------------------------
                        udtSmartIDContent.EHSAccount = SmartIDDummyCase.GetDummyEHSAccount(udtSchemeClaim, udtSmartIDContent.IdeasVersion)
                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
                        udtSmartIDContent.EHSAccount.EHSPersonalInformationList(0).CName = BLL.VoucherAccountMaintenanceBLL.GetCName(udtSmartIDContent.EHSAccount.EHSPersonalInformationList(0))

                    Else
                        Dim udtCFD As IdeasRM.CardFaceData
                        udtCFD = ideasSamlResponse.CardFaceDate()
                        If IsNothing(udtCFD) Then
                            strError = "ideasSamlResponse.CardFaceDate() is nothing"
                        End If
                        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                        udtSmartIDContent.EHSAccount = ideasBLL.GetCardFaceDataEHSAccount(udtCFD, udtSchemeClaim, FunctCode, udtSmartIDContent)
                        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	
                    End If
                Catch ex As Exception
                    udtSmartIDContent.EHSAccount = Nothing
                    strError = ex.Message
                End Try

                Dim udtAuditlogEntry_Search As AuditLogEntry = New AuditLogEntry(FunctCode, Me)
                Dim strHKICNo As String = String.Empty

                If Not udtSmartIDContent.EHSAccount Is Nothing Then
                    strHKICNo = udtSmartIDContent.EHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).IdentityNum.Trim
                End If

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                EHSClaimBasePage.AuditLogSearchNvaliatedACwithCFD(udtAuditlogEntry_Search, udtSchemeClaim.SchemeCode, strHKICNo, strError, strIdeasVersion, "Claim")
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                If Not udtSmartIDContent.EHSAccount Is Nothing Then

                    udtPersonalInfoSmartID = udtSmartIDContent.EHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC)

                    '------------------------------------------------------------------------------------------------------
                    'Card Face Data Validation
                    '------------------------------------------------------------------------------------------------------
                    Me._udtSystemMessage = EHSClaimBasePage.SmartIDCardFaceDataValidation(udtPersonalInfoSmartID)
                    If Not Me._udtSystemMessage Is Nothing Then
                        isValid = False
                        If Not udtPersonalInfoSmartID.IdentityNum Is Nothing Then udtAuditlogEntry_Search.AddDescripton("HKID", udtPersonalInfoSmartID.IdentityNum)
                        If udtPersonalInfoSmartID.DateofIssue.HasValue Then udtAuditlogEntry_Search.AddDescripton("DOI", udtPersonalInfoSmartID.DateofIssue)
                        udtAuditlogEntry_Search.AddDescripton("DOB", udtPersonalInfoSmartID.DOB)

                        Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                    End If

                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    SearchSmartID(udtSmartIDContent, isValid, udtAuditlogEntry_Search)
                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
                Else
                    '---------------------------------------------------------------------------------------------------------------
                    ' udtSmartIDContent.EHSAccount is nothing, crad face data may not be able to return 
                    '---------------------------------------------------------------------------------------------------------------
                    _udtSessionHandler.ForceToReadSmartICSaveToSession(False)

                    Me.Clear()
                    Me.udcStep1DocumentTypeRadioButtonGroup.SelectedValue = DocTypeModel.DocTypeCode.HKIC
                    Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
                    Me.udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00253"))
                    isValid = False

                End If

                If udcMsgBoxErr.GetCodeTable.Rows.Count > 0 Then
                    Me.udcMsgBoxErr.BuildMessageBox(_strValidationFail, udtAuditlogEntry_Search, Common.Component.LogID.LOG00052, "Search & validate account with CFD Fail", _
                        New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocTypeModel.DocTypeCode.HKIC, (New Formatter).formatDocumentIdentityNumber(DocTypeModel.DocTypeCode.HKIC, strHKICNo)))
                End If
            Else
                '---------------------------------------------------------------------------------------------------------------
                ' ideasSamlResponse.StatusCode is not "samlp:Success"
                '---------------------------------------------------------------------------------------------------------------
                _udtSessionHandler.ForceToReadSmartICSaveToSession(False)

                Me.Clear()
                Me.udcStep1DocumentTypeRadioButtonGroup.SelectedValue = DocTypeModel.DocTypeCode.HKIC
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
                Me.udcMsgBoxErr.AddMessageDesc(FunctCode, ideasSamlResponse.StatusMessage, ideasSamlResponse.StatusDetail)

                'Me.udcMsgBoxErr.BuildMessageDescBox("SmartIDActionFail")

                'Write End Audit log
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                'EHSClaimBasePage.AuditLogGetCFDFail(udtAuditLogEntry_GetCFD, ideasBLL.Artifact, ideasSamlResponse.StatusMessage, ideasSamlResponse.StatusDetail)
                EHSClaimBasePage.AuditLogGetCFDFail(udtAuditLogEntry_GetCFD, ideasBLL.Artifact, ideasSamlResponse.StatusMessage, ideasSamlResponse.StatusDetail, strIdeasVersion)
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
                Me.udcMsgBoxErr.BuildMessageDescBox("SmartIDActionFail", udtAuditLogEntry_GetCFD, Common.Component.LogID.LOG00063, "Get CFD Fail")

                isValid = False

            End If
        End If

        Return isReadingSmartID
    End Function

    Private Sub PageLoadPracticeSetup(ByVal udtSP As ServiceProviderModel, ByVal udtDataEntry As DataEntryUserModel)
        EHSClaimBasePage.AuditLogPageLoad(New AuditLogEntry(FunctionCode, Me), False, False)

        Dim udtPracticeDisplayList As PracticeDisplayModelCollection = Nothing

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Dim udtFilterPracticeList As New PracticeModelCollection

        If Not IsNothing(udtDataEntry) Then
            'Data Entry
            For Each udtPractice As PracticeModel In udtSP.PracticeList.Values
                ' Practice Scheme Info List Filter by COVID-19
                Dim udtFilterPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = _udtSchemeClaimBLL.FilterPracticeSchemeInfo(udtDataEntry.ServiceProvider.PracticeList, udtPractice.DisplaySeq, Me.ClaimMode())

                ' Get valid schemes
                Dim udtFilterSchemeClaimList As SchemeClaimModelCollection = _udtSchemeClaimBLL.searchValidClaimPeriodSchemeClaimByPracticeSchemeInfoSubsidizeCode( _
                    udtFilterPracticeSchemeInfoList, _udtSP.SchemeInfoList)

                udtFilterSchemeClaimList = udtFilterSchemeClaimList.FilterWithoutReadonly()

                udtFilterSchemeClaimList = udtFilterSchemeClaimList.FilterByHCSPSubPlatform(Me.SubPlatform)

                'If udtFilterPracticeSchemeInfoList.Count > 0 Then
                If udtFilterSchemeClaimList.Count > 0 Then
                    udtFilterPracticeList.Add(New PracticeModel(udtPractice))
                End If
            Next

            udtPracticeDisplayList = (New PracticeBankAcctBLL).getActivePracticeWithAvailableScheme(udtDataEntry.ServiceProvider.SPID, _
                udtDataEntry.DataEntryAccount, udtFilterPracticeList, udtDataEntry.ServiceProvider.SchemeInfoList)
        Else
            'Service Provider
            For Each udtPractice As PracticeModel In udtSP.PracticeList.Values
                ' Practice Scheme Info List Filter by COVID-19
                Dim udtFilterPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = _udtSchemeClaimBLL.FilterPracticeSchemeInfo(udtSP.PracticeList, udtPractice.DisplaySeq, Me.ClaimMode())

                ' Get valid schemes
                Dim udtFilterSchemeClaimList As SchemeClaimModelCollection = _udtSchemeClaimBLL.searchValidClaimPeriodSchemeClaimByPracticeSchemeInfoSubsidizeCode( _
                    udtFilterPracticeSchemeInfoList, _udtSP.SchemeInfoList)

                udtFilterSchemeClaimList = udtFilterSchemeClaimList.FilterWithoutReadonly()

                udtFilterSchemeClaimList = udtFilterSchemeClaimList.FilterByHCSPSubPlatform(Me.SubPlatform)

                'If udtFilterPracticeSchemeInfoList.Count > 0 Then
                If udtFilterSchemeClaimList.Count > 0 Then
                    udtFilterPracticeList.Add(New PracticeModel(udtPractice))
                End If
            Next

            udtPracticeDisplayList = (New PracticeBankAcctBLL).getActivePracticeWithAvailableScheme(udtSP.SPID, udtFilterPracticeList, udtSP.SchemeInfoList)
        End If
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        ' Save practice display list before any action
        Me._udtSessionHandler.PracticeDisplayListSaveToSession(udtPracticeDisplayList, FunctCode)

        If udtPracticeDisplayList Is Nothing OrElse udtPracticeDisplayList.Count = 0 Then
            'Step 2 of Page Load : SP have no avaiable practice -> go to vEHSClaimError
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.EHSClaimError

        ElseIf udtPracticeDisplayList IsNot Nothing AndAlso Not udtPracticeDisplayList.HasPracticeAvailableForClaim() Then
            ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]
            ' -----------------------------------------------------------------------------------------
            ' Step 2 of Page Load :Practice is active but not available for claim -> go to vEHSClaimError
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.EHSClaimError

            ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]
        ElseIf udtPracticeDisplayList.Count = 1 Then
            'Step 3 of Page Load :  Only 1 Active Practice, Auto Select -> go to Search Account Page (Step1)
            Me._udtSessionHandler.PracticeDisplaySaveToSession(udtPracticeDisplayList(0), FunctCode)

            ' Clear all sessions
            Dim udtEHSAccount As EHSAccountModel = Nothing
            Dim udtSmartIDContent As SmartIDContentModel = Nothing

            Dim blnFromVaccinationRecordEnquiry As Boolean = CheckFromVaccinationRecordEnquiry()

            If blnFromVaccinationRecordEnquiry Then
                udtEHSAccount = _udtSessionHandler.EHSAccountGetFromSession(FunctCode)
                udtSmartIDContent = _udtSessionHandler.SmartIDContentGetFormSession(FunctCode)
            End If

            Clear()

            If blnFromVaccinationRecordEnquiry Then
                _udtSessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctCode)
                _udtSessionHandler.SmartIDContentSaveToSession(FunctCode, udtSmartIDContent)
            End If

            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1

        Else
            'Step 4 of Page Load : More then 1 Active Practice, go to Select Practice -> go to Select Practice Page
            Me._udtSessionHandler.PracticeDisplayListSaveToSession(udtPracticeDisplayList, FunctCode)
            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.SelectPractice

        End If

    End Sub
    '==================================================================================================================================================================

#Region "Popup box events"

    '---------------------------------------------------------------------------------------------------------
    'Confirmation Box
    '---------------------------------------------------------------------------------------------------------
    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    Private Sub ucNoticePopUpConfirm_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUpConfirm.ButtonClick
        Select Case e
            Case ucNoticePopUp.enumButtonClick.OK
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Koala]
                ' -----------------------------------------------------------------------------------------
                EHSClaimBasePage.AuditLogEnterClaimDetailCancelYes(_udtAuditLogEntry)
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Koala]
                Me.Clear()
                _udtSessionHandler.ClearVREClaim()
                _udtSessionHandler.ClaimCOVID19MainCategoryRemoveFromSession(FunctCode)
                _udtSessionHandler.ClaimCOVID19SubCategoryRemoveFromSession(FunctCode)
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
            Case Else
                ' Do nothing
        End Select

    End Sub

    'Protected Sub btnPopupConfirmCancelConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPopupConfirmCancelConfirm.Click
    '    Me.Clear()
    '    _udtSessionHandler.ClearVREClaim()
    '    Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
    'End Sub

    'Private Sub btnPopupConfirmCancelCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPopupConfirmCancelCancel.Click

    'End Sub
    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Koala]

    ''' <summary>
    ''' Select Practice in popup box
    ''' </summary>
    ''' <param name="strPracticeName"></param>
    ''' <param name="strBankAcctNo"></param>
    ''' <param name="intBankAccountDisplaySeq"></param>
    ''' <param name="strSchemeCode"></param>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub udcPopupPracticeRadioButtonGroup_PracticeSelected(ByVal strPracticeName As String, ByVal strBankAcctNo As String, ByVal intBankAccountDisplaySeq As Integer, ByVal strSchemeCode As String, ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcPopupPracticeRadioButtonGroup.PracticeSelected
        ' Get current user account
        Dim udtDataEntryModel As DataEntryUserModel = Nothing
        GetCurrentUserAccount(_udtSP, udtDataEntryModel, False)

        ' Get old and new selected Practice
        Dim udtOldPracticeDisplay As PracticeDisplayModel = _udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)
        Dim udtNewPracticeDisplay As PracticeDisplayModel = _udtSessionHandler.PracticeDisplayListGetFromSession(FunctCode).Filter(intBankAccountDisplaySeq)

        ' Save the new selected Practice to session
        _udtSessionHandler.PracticeDisplaySaveToSession(udtNewPracticeDisplay, FunctCode)

        ' Save the new selected Scheme to session
        _udtSessionHandler.SchemeSelectedForPracticeSaveToSession(strSchemeCode, FunctCode)

        'Init Carry Forward
        _udtSessionHandler.ClaimCOVID19CarryForwordSaveToSession(False, FunctCode)

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ' Audit log
        Dim udtSchemeClaim As SchemeClaimModel = _udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        Dim udtCOVID19BLL As New COVID19.COVID19BLL

        If udtSchemeClaim IsNot Nothing Then
            If ClaimMode = Common.Component.ClaimMode.COVID19 AndAlso udtSchemeClaim.SchemeCode = SchemeClaimModel.COVID19CVC Then
                EHSClaimBasePage.AddDescriptionCentreBooth(udtAuditLogEntry, udtOldPracticeDisplay.SPID, udtOldPracticeDisplay.PracticeID)
            End If
        End If
        EHSClaimBasePage.AuditLogPracticeSelected(udtAuditLogEntry, True, udtNewPracticeDisplay, udtSchemeClaim, False)
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        Select Case mvEHSClaim.ActiveViewIndex
            Case ActiveViewIndex.Step1
                Me.udcMsgBoxErr.Clear()
                Me.udcMsgBoxInfo.Clear()

                'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                If udtNewPracticeDisplay.PracticeID <> udtOldPracticeDisplay.PracticeID Then
                    'Me._udtSessionHandler.DocumentaryProofForPIDRemoveFromSession(FunctCode)
                    Me._udtSessionHandler.PIDInstitutionCodeRemoveFromSession(FunctCode)
                    Me._udtSessionHandler.PlaceVaccinationRemoveFromSession(FunctCode)
                    Me._udtSessionHandler.PlaceVaccinationOtherRemoveFromSession(FunctCode)
                    Me._udtSessionHandler.ClaimCategoryRemoveFromSession(FunctCode)
                    Me._udtSessionHandler.ForceToReadSmartICSaveToSession(True)
                End If
                'CRE16-002 (Revamp VSS) [End][Chris YIM]

                SetupStep1(False, True)

                Return

            Case ActiveViewIndex.Step2a
                Dim udtEHSAccount As EHSAccountModel = _udtSessionHandler.EHSAccountGetFromSession(FunctCode)
                Dim udtSchemeClaimUpdated As SchemeClaimModel = Nothing
                Dim isValid As Boolean = False

                'Get the orignal Scheme
                'Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

                If udtNewPracticeDisplay.PracticeID <> udtOldPracticeDisplay.PracticeID Then

                    ' Clear Session
                    Me._udtSessionHandler.EHSClaimVaccineRemoveFromSession()
                    Me._udtSessionHandler.PIDInstitutionCodeRemoveFromSession(FunctCode)
                    Me._udtSessionHandler.PlaceVaccinationRemoveFromSession(FunctCode)
                    Me._udtSessionHandler.PlaceVaccinationOtherRemoveFromSession(FunctCode)
                    Me._udtSessionHandler.ClaimCategoryRemoveFromSession(FunctCode)
                    'Me._udtSessionHandler.ExceedDocTypeLimitRemoveFromSession()

                    Me.udcMsgBoxErr.Clear()
                    Me.udcMsgBoxInfo.Clear()

                    If Not udtSchemeClaim Is Nothing Then
                        Select Case udtSchemeClaim.ControlType
                            Case SchemeClaimModel.EnumControlType.RVP
                                If Me.ClaimMode = Common.Component.ClaimMode.COVID19 Then
                                    _udtSessionHandler.ClaimCOVID19DoseRemoveFromSession(FunctionCode)

                                    Dim udcInputRVPCOVID19 As ucInputRVPCOVID19 = Me.udcStep2aInputEHSClaim.GetRVPCOVID19Control()
                                    If Not udcInputRVPCOVID19 Is Nothing Then
                                        If udcInputRVPCOVID19.VaccineBrandDDL.Items.Count <= 1 Then
                                            udcInputRVPCOVID19.ClearVaccineAndLotNo()
                                        End If
                                    End If

                                    Me.chkStep2aDeclareClaim.Checked = False
                                Else
                                    Dim udcInputRVP As ucInputRVP = Me.udcStep2aInputEHSClaim.GetRVPControl()

                                    If udcInputRVP IsNot Nothing Then udcInputRVP.ClearClaimDetail()

                                End If

                            Case SchemeClaimModel.EnumControlType.VSS
                                If Me.ClaimMode = Common.Component.ClaimMode.COVID19 Then
                                    _udtSessionHandler.ClaimCOVID19DoseRemoveFromSession(FunctionCode)

                                    Dim udcInputVSSCOVID19 As ucInputVSSCOVID19 = Me.udcStep2aInputEHSClaim.GetVSSCOVID19Control()
                                    If Not udcInputVSSCOVID19 Is Nothing Then
                                        If udcInputVSSCOVID19.VaccineBrandDDL.Items.Count <= 1 Then
                                            udcInputVSSCOVID19.ClearVaccineAndLotNo()
                                        End If
                                    End If

                                    Me.chkStep2aDeclareClaim.Checked = False
                                Else
                                    Dim udcInputVSS As ucInputVSS = Me.udcStep2aInputEHSClaim.GetVSSControl()

                                    If udcInputVSS IsNot Nothing Then
                                        udcInputVSS.ClearClaimDetail()
                                    End If

                                End If

                            Case SchemeClaimModel.EnumControlType.ENHVSSO
                                Dim udcInputENHVSSO As ucInputENHVSSO = Me.udcStep2aInputEHSClaim.GetENHVSSOControl()

                                If udcInputENHVSSO IsNot Nothing Then udcInputENHVSSO.ClearClaimDetail()

                                ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
                                ' --------------------------------------------------------------------------------------
                            Case SchemeClaimModel.EnumControlType.COVID19
                                '_udtSessionHandler.ClaimCOVID19CategoryRemoveFromSession(FunctionCode)
                                '_udtSessionHandler.ClaimCOVID19VaccineBrandSaveToSession(udcInputCOVID19.VaccineBrand, FunctionCode)
                                '_udtSessionHandler.ClaimCOVID19VaccineLotNoSaveToSession(udcInputCOVID19.VaccineLotNo, FunctionCode)
                                _udtSessionHandler.ClaimCOVID19DoseRemoveFromSession(FunctionCode)

                                Dim udcInputCOVID19 As ucInputCOVID19 = Me.udcStep2aInputEHSClaim.GetCOVID19Control()
                                If Not udcInputCOVID19 Is Nothing Then
                                    If udcInputCOVID19.VaccineBrandDDL.Items.Count <= 1 Then
                                        udcInputCOVID19.ClearVaccineAndLotNo()
                                    End If
                                End If

                                Me.chkStep2aDeclareClaim.Checked = False
                                ' CRE20-0022 (Immu record) [End][Winnie SUEN]

                            Case SchemeClaimModel.EnumControlType.COVID19RVP
                                '_udtSessionHandler.ClaimCOVID19CategoryRemoveFromSession(FunctionCode)
                                '_udtSessionHandler.ClaimCOVID19VaccineBrandSaveToSession(udcInputCOVID19.VaccineBrand, FunctionCode)
                                '_udtSessionHandler.ClaimCOVID19VaccineLotNoSaveToSession(udcInputCOVID19.VaccineLotNo, FunctionCode)
                                _udtSessionHandler.ClaimCOVID19DoseRemoveFromSession(FunctionCode)

                                Dim udcInputCOVID19RVP As ucInputCOVID19RVP = Me.udcStep2aInputEHSClaim.GetCOVID19RVPControl()
                                If Not udcInputCOVID19RVP Is Nothing Then
                                    If udcInputCOVID19RVP.VaccineBrandDDL.Items.Count <= 1 Then
                                        udcInputCOVID19RVP.ClearVaccineAndLotNo()
                                    End If
                                End If

                                Me.chkStep2aDeclareClaim.Checked = False
                                ' CRE20-0022 (Immu record) [End][Winnie SUEN]

                            Case SchemeClaimModel.EnumControlType.COVID19OR
                                '_udtSessionHandler.ClaimCOVID19CategoryRemoveFromSession(FunctionCode)
                                '_udtSessionHandler.ClaimCOVID19VaccineBrandSaveToSession(udcInputCOVID19.VaccineBrand, FunctionCode)
                                '_udtSessionHandler.ClaimCOVID19VaccineLotNoSaveToSession(udcInputCOVID19.VaccineLotNo, FunctionCode)
                                _udtSessionHandler.ClaimCOVID19DoseRemoveFromSession(FunctionCode)

                                Dim udcInputCOVID19OR As ucInputCOVID19OR = Me.udcStep2aInputEHSClaim.GetCOVID19ORControl()
                                If Not udcInputCOVID19OR Is Nothing Then
                                    If udcInputCOVID19OR.VaccineBrandDDL.Items.Count <= 1 Then
                                        udcInputCOVID19OR.ClearVaccineAndLotNo()
                                    End If
                                End If

                                Me.chkStep2aDeclareClaim.Checked = False

                        End Select
                    End If

                    ' CRE19-003 (Opt voucher capping) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------'
                    ' Profession is changed, need to recalculate quota
                    If Not udtOldPracticeDisplay.ServiceCategoryCode.Equals(udtNewPracticeDisplay.ServiceCategoryCode) Then

                        ' Clear AvailableVoucher to trigger recalculation
                        udtEHSAccount.VoucherInfo = Nothing
                    End If
                    ' CRE19-003 (Opt voucher capping) [End][Winnie]

                    isValid = Me.CheckSchemeAvailableForEHSAccount(udtEHSAccount, udtNewPracticeDisplay, udtSchemeClaim)
                End If

                'Log
                udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
                EHSClaimBasePage.AddDescriptionCentreBooth(udtAuditLogEntry, udtOldPracticeDisplay.SPID, udtOldPracticeDisplay.PracticeID)

                If isValid Then

                    ''add the true boolean if practice ExceedDocTypeLimit checked in CheckSchemeAvailableForEHSAccount => 
                    'Me._udtSessionHandler.ExceedDocTypeLimitSaveToSession(True)

                    'Log Practice Selection
                    EHSClaimBasePage.AuditLogPracticeSelected(udtAuditLogEntry, True, udtNewPracticeDisplay, udtSchemeClaim, True)

                    ' Scheme Available in new Practice
                    If Not udtOldPracticeDisplay.PracticeID.Equals(intBankAccountDisplaySeq) Then

                        Me._udtSessionHandler.EHSTransactionRemoveFromSession(FunctCode)

                        '' Default Scheme = False to keep retain the Scheme
                        Me.udcStep2aInputEHSClaim.ResetSchemeType()

                        Me.SetupStep2a(udtEHSAccount, False, False, False)
                        Me.Step2aCleanSchemeErrorImage()

                        ' CRE20-0022 (Immu record) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                        If Me.ClaimMode = ClaimMode.COVID19 Then
                            If Me.ddlStep2aScheme.Visible Then
                                Me.ddlStep2aScheme.SelectedValue = udtSchemeClaim.SchemeCode
                            End If
                        End If
                        ' CRE20-0022 (Immu record) [End][Chris YIM]
                    End If

                Else

                    'Log Practice Selection
                    EHSClaimBasePage.AuditLogPracticeSelected(udtAuditLogEntry, True, udtNewPracticeDisplay, udtSchemeClaim, False)

                    ' Scheme not Available in new Practice
                    If Not udtOldPracticeDisplay.PracticeID.Equals(intBankAccountDisplaySeq) Then
                        Me._udtSessionHandler.SchemeSelectedRemoveFromSession(FunctCode)
                        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                        Me._udtSessionHandler.NonClinicSettingRemoveFromSession(FunctCode)
                        'CRE16-002 (Revamp VSS) [End][Chris YIM]
                        Me._udtSessionHandler.EHSTransactionRemoveFromSession(FunctCode)
                        Me.ddlStep2aScheme.Items.Clear()
                    End If
                    Me.SetupStep2a(udtEHSAccount, False, True, False)
                    Me.Step2aCleanSchemeErrorImage()
                End If

                'After SetupStep2a -> Scheme may be changed by default
                'Scheme can be empty, if the current practice is no scheme for recipient
                udtSchemeClaimUpdated = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
                If Not udtSchemeClaimUpdated Is Nothing Then
                    Dim udtConvertedSchemeCode As String = _udtSchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(udtSchemeClaimUpdated.SchemeCode)
                    _udtSessionHandler.NonClinicSettingSaveToSession(_udtSP.PracticeList(udtNewPracticeDisplay.PracticeID).PracticeSchemeInfoList.Filter(udtConvertedSchemeCode).IsNonClinic, FunctCode)

                    If udtSchemeClaim Is Nothing OrElse Not udtSchemeClaim.SchemeCode.Equals(udtSchemeClaimUpdated.SchemeCode) Then
                        Me.SetupStep2aClaimContent(udtSchemeClaimUpdated, udtEHSAccount)
                    End If

                    ' CRE20-0022 (Immu record) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                    Me.SetupServiceDate(udtSchemeClaimUpdated.SchemeCode)
                    ' CRE20-0022 (Immu record) [End][Chris YIM]
                End If

                If Not udcMsgBoxErr.Visible Then
                    If udcMsgBoxErr.GetCodeTable.Rows.Count <> 0 Then Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail)
                End If

                Me.SetupSchemeLogo()

                If Not udtSchemeClaim Is Nothing Then
                    Select Case udtSchemeClaim.ControlType
                        Case SchemeClaimModel.EnumControlType.HSIVSS
                            Dim udcInputHSIVSS As ucInputHSIVSS = Me.udcStep2aInputEHSClaim.GetHSIVSSControl()
                            If Not udcInputHSIVSS Is Nothing Then
                                If String.IsNullOrEmpty(udcInputHSIVSS.Category) Then
                                    Me.SetConfirmButtonEnable(Me.btnStep2aClaim, False)
                                End If
                            End If

                        Case SchemeClaimModel.EnumControlType.RVP
                            If ClaimMode = Common.Component.ClaimMode.COVID19 Then
                                Dim udcInputRVPCOVID19 As ucInputRVPCOVID19 = Me.udcStep2aInputEHSClaim.GetRVPCOVID19Control()
                                If Not udcInputRVPCOVID19 Is Nothing Then
                                    If udcInputRVPCOVID19.VaccineBrandDDL.Items.Count > 1 Then
                                        udcInputRVPCOVID19.ClearVaccineAndLotNo()
                                    End If
                                    _udtSessionHandler.ClaimCOVID19VaccineBrandRemoveFromSession(FunctionCode)
                                    _udtSessionHandler.ClaimCOVID19VaccineLotNoRemoveFromSession(FunctionCode)

                                    'Set Claim button
                                    If String.IsNullOrEmpty(udcInputRVPCOVID19.CategoryForCOVID19) Then
                                        Me.SetConfirmButtonEnable(Me.btnStep2aClaim, False)
                                    End If
                                End If

                            Else
                                Dim udcInputRVP As ucInputRVP = Me.udcStep2aInputEHSClaim.GetRVPControl()
                                If Not udcInputRVP Is Nothing Then
                                    If String.IsNullOrEmpty(udcInputRVP.Category) Then
                                        Me.SetConfirmButtonEnable(Me.btnStep2aClaim, False)
                                    End If
                                End If
                            End If

                        Case SchemeClaimModel.EnumControlType.VSS
                            If ClaimMode = Common.Component.ClaimMode.COVID19 Then
                                Dim udcInputVSSCOVID19 As ucInputVSSCOVID19 = Me.udcStep2aInputEHSClaim.GetVSSCOVID19Control()
                                If Not udcInputVSSCOVID19 Is Nothing Then
                                    If udcInputVSSCOVID19.VaccineBrandDDL.Items.Count > 1 Then
                                        udcInputVSSCOVID19.ClearVaccineAndLotNo()
                                    End If
                                    _udtSessionHandler.ClaimCOVID19VaccineBrandRemoveFromSession(FunctionCode)
                                    _udtSessionHandler.ClaimCOVID19VaccineLotNoRemoveFromSession(FunctionCode)

                                    'Set Claim button
                                    If String.IsNullOrEmpty(udcInputVSSCOVID19.CategoryForCOVID19) Then
                                        Me.SetConfirmButtonEnable(Me.btnStep2aClaim, False)
                                    End If
                                End If

                            Else
                                Dim udcInputVSS As ucInputVSS = Me.udcStep2aInputEHSClaim.GetVSSControl()
                                If Not udcInputVSS Is Nothing Then
                                    If String.IsNullOrEmpty(udcInputVSS.Category) Then
                                        Me.SetConfirmButtonEnable(Me.btnStep2aClaim, False)
                                    End If
                                End If

                            End If

                        Case SchemeClaimModel.EnumControlType.ENHVSSO
                            Dim udcInputENHVSSO As ucInputENHVSSO = Me.udcStep2aInputEHSClaim.GetENHVSSOControl()
                            If Not udcInputENHVSSO Is Nothing Then
                                If String.IsNullOrEmpty(udcInputENHVSSO.Category) Then
                                    Me.SetConfirmButtonEnable(Me.btnStep2aClaim, False)
                                End If
                            End If

                            ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
                            ' --------------------------------------------------------------------------------------
                        Case SchemeClaimModel.EnumControlType.COVID19
                            Dim udcInputCOVID19 As ucInputCOVID19 = Me.udcStep2aInputEHSClaim.GetCOVID19Control()
                            If Not udcInputCOVID19 Is Nothing Then
                                If udcInputCOVID19.VaccineBrandDDL.Items.Count > 1 Then
                                    udcInputCOVID19.ClearVaccineAndLotNo()
                                End If
                                _udtSessionHandler.ClaimCOVID19VaccineBrandRemoveFromSession(FunctionCode)
                                _udtSessionHandler.ClaimCOVID19VaccineLotNoRemoveFromSession(FunctionCode)

                                'Set Claim button
                                If String.IsNullOrEmpty(udcInputCOVID19.CategoryForCOVID19) Then
                                    Me.SetConfirmButtonEnable(Me.btnStep2aClaim, False)
                                End If
                            End If
                            ' CRE20-0022 (Immu record) [End][Winnie SUEN]

                        Case SchemeClaimModel.EnumControlType.COVID19RVP
                            Dim udcInputCOVID19RVP As ucInputCOVID19RVP = Me.udcStep2aInputEHSClaim.GetCOVID19RVPControl()
                            If Not udcInputCOVID19RVP Is Nothing Then
                                If udcInputCOVID19RVP.VaccineBrandDDL.Items.Count > 1 Then
                                    udcInputCOVID19RVP.ClearVaccineAndLotNo()
                                End If
                                _udtSessionHandler.ClaimCOVID19VaccineBrandRemoveFromSession(FunctionCode)
                                _udtSessionHandler.ClaimCOVID19VaccineLotNoRemoveFromSession(FunctionCode)

                                'Set Claim button
                                If String.IsNullOrEmpty(udcInputCOVID19RVP.CategoryForCOVID19) Then
                                    Me.SetConfirmButtonEnable(Me.btnStep2aClaim, False)
                                End If
                            End If

                        Case SchemeClaimModel.EnumControlType.COVID19OR
                            Dim udcInputCOVID19OR As ucInputCOVID19OR = Me.udcStep2aInputEHSClaim.GetCOVID19ORControl()
                            If Not udcInputCOVID19OR Is Nothing Then
                                If udcInputCOVID19OR.VaccineBrandDDL.Items.Count > 1 Then
                                    udcInputCOVID19OR.ClearVaccineAndLotNo()
                                End If
                                _udtSessionHandler.ClaimCOVID19VaccineBrandRemoveFromSession(FunctionCode)
                                _udtSessionHandler.ClaimCOVID19VaccineLotNoRemoveFromSession(FunctionCode)

                                'Set Claim button
                                If String.IsNullOrEmpty(udcInputCOVID19OR.CategoryForCOVID19) Then
                                    Me.SetConfirmButtonEnable(Me.btnStep2aClaim, False)
                                End If
                            End If

                    End Select

                End If

                If ClaimMode = Common.Component.ClaimMode.COVID19 Then
                    Dim udtFormatter As New Formatter

                    Dim strServiceDate As String = udtFormatter.formatInputDate(Me.txtStep2aServiceDate.Text, (New SubPlatformBLL).GetDateFormatLocale(Me.SubPlatform))

                    Dim dtmServiceDate As DateTime = udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English)

                    'Get Vaccine Brand & Lot No.
                    Dim strVaccineLotNoMappingJavaScript As String = String.Empty

                    Dim udtNewSchemeClaim As SchemeClaimModel = Nothing

                    If udtSchemeClaim Is Nothing Then
                        udtNewSchemeClaim = udtSchemeClaimUpdated
                    Else

                        If udtSchemeClaimUpdated Is Nothing OrElse udtSchemeClaim.SchemeCode <> udtSchemeClaimUpdated.SchemeCode Then
                            udtNewSchemeClaim = udtSchemeClaimUpdated
                        Else
                            udtNewSchemeClaim = udtSchemeClaim
                        End If

                    End If

                    If udtNewSchemeClaim IsNot Nothing Then
                        Select Case udtNewSchemeClaim.ControlType
                            Case SchemeClaimModel.EnumControlType.COVID19, SchemeClaimModel.EnumControlType.COVID19RVP, SchemeClaimModel.EnumControlType.COVID19OR
                                strVaccineLotNoMappingJavaScript = Me.GetVaccineLotNoMappingForCentreJavaScript(udtNewPracticeDisplay.SPID, udtNewPracticeDisplay.PracticeID, dtmServiceDate)

                                If udtNewSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.COVID19OR Then
                                    Dim udcInputCOVID19OR As ucInputCOVID19OR = Me.udcStep2aInputEHSClaim.GetCOVID19ORControl()
                                    If Not udcInputCOVID19OR Is Nothing Then
                                        If udcInputCOVID19OR.VaccineBrandDDL.Items.Count > 1 Then
                                            udcInputCOVID19OR.ClearVaccineAndLotNo()
                                        End If
                                    End If

                                    'Convert sub-category -> Dictionary -> Json
                                    Dim strSubCategoryMappingJavaScript As String = String.Empty
                                    Dim strCategoryJson As String = String.Empty
                                    Dim strCategoryTextEngJson As String = String.Empty
                                    Dim strCategoryTextChiJson As String = String.Empty

                                    udtCOVID19BLL.GenerateCategoryJson(strCategoryJson, strCategoryTextEngJson, strCategoryTextChiJson)

                                    'Generate JavaScript
                                    strSubCategoryMappingJavaScript = udtCOVID19BLL.GenerateCategoryMappingJavaScript(strCategoryJson, strCategoryTextEngJson, strCategoryTextChiJson)
                                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "COVID19_SubCategory_Mapping", strSubCategoryMappingJavaScript, True)
                                End If

                                'Carry Forword: once times only
                                If _udtSessionHandler.ClaimCOVID19CarryForwordGetFromSession(FunctCode) = False Then
                                    _udtSessionHandler.ClaimCOVID19CarryForwordSaveToSession(True, FunctCode)
                                End If

                            Case SchemeClaimModel.EnumControlType.VSS
                                strVaccineLotNoMappingJavaScript = Me.GetVaccineLotNoMappingForPrivateJavaScript(dtmServiceDate)

                                Dim udcInputVSSCOVID19 As ucInputVSSCOVID19 = Me.udcStep2aInputEHSClaim.GetVSSCOVID19Control()
                                If Not udcInputVSSCOVID19 Is Nothing Then
                                    If udcInputVSSCOVID19.VaccineBrandDDL.Items.Count > 1 Then
                                        udcInputVSSCOVID19.ClearVaccineAndLotNo()
                                    End If
                                End If

                                'Convert sub-category -> Dictionary -> Json
                                Dim strSubCategoryMappingJavaScript As String = String.Empty
                                Dim strCategoryJson As String = String.Empty
                                Dim strCategoryTextEngJson As String = String.Empty
                                Dim strCategoryTextChiJson As String = String.Empty

                                udtCOVID19BLL.GenerateCategoryJson(strCategoryJson, strCategoryTextEngJson, strCategoryTextChiJson)

                                'Generate JavaScript
                                strSubCategoryMappingJavaScript = udtCOVID19BLL.GenerateCategoryMappingJavaScript(strCategoryJson, strCategoryTextEngJson, strCategoryTextChiJson)
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "COVID19_SubCategory_Mapping", strSubCategoryMappingJavaScript, True)

                                'Carry Forword: once times only
                                If _udtSessionHandler.ClaimCOVID19CarryForwordGetFromSession(FunctCode) = False Then
                                    _udtSessionHandler.ClaimCOVID19CarryForwordSaveToSession(True, FunctCode)
                                End If

                            Case SchemeClaimModel.EnumControlType.RVP
                                strVaccineLotNoMappingJavaScript = Me.GetVaccineLotNoMappingForRCHJavaScript(dtmServiceDate)

                                Dim udcInputRVPCOVID19 As ucInputRVPCOVID19 = Me.udcStep2aInputEHSClaim.GetRVPCOVID19Control()
                                If Not udcInputRVPCOVID19 Is Nothing Then
                                    If udcInputRVPCOVID19.VaccineBrandDDL.Items.Count > 1 Then
                                        udcInputRVPCOVID19.ClearVaccineAndLotNo()
                                    End If
                                End If

                                'Convert sub-category -> Dictionary -> Json
                                Dim strSubCategoryMappingJavaScript As String = String.Empty
                                Dim strCategoryJson As String = String.Empty
                                Dim strCategoryTextEngJson As String = String.Empty
                                Dim strCategoryTextChiJson As String = String.Empty

                                udtCOVID19BLL.GenerateCategoryJson(strCategoryJson, strCategoryTextEngJson, strCategoryTextChiJson)

                                'Generate JavaScript
                                strSubCategoryMappingJavaScript = udtCOVID19BLL.GenerateCategoryMappingJavaScript(strCategoryJson, strCategoryTextEngJson, strCategoryTextChiJson)
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "COVID19_SubCategory_Mapping", strSubCategoryMappingJavaScript, True)

                                'Carry Forword: once times only
                                If _udtSessionHandler.ClaimCOVID19CarryForwordGetFromSession(FunctCode) = False Then
                                    _udtSessionHandler.ClaimCOVID19CarryForwordSaveToSession(True, FunctCode)
                                End If
                        End Select

                    End If

                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "COVID19_Vaccine_LotNo_Mapping", strVaccineLotNoMappingJavaScript, True)

                End If
        End Select

    End Sub

    ' CRE20-0XX (HA Scheme) [Start][Winnie]
    Private Sub udcPopupPracticeRadioButtonGroup_SchemeSelected() Handles udcPopupPracticeRadioButtonGroup.SchemeSelected
        ModalPopupPracticeSelection.Show()
    End Sub
    ' CRE20-0XX (HA Scheme) [End][Winnie]

    '---------------------------------------------------------------------------------------------------------
    'Select Print Option
    '---------------------------------------------------------------------------------------------------------
    Private Sub btnModalPopupPrintOptionSelection_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPopupPrintOptionSelectionSelect.Click
        Dim strSelectedPrintingOption As String = Me.udtPrintOptionSelection.getSelection()
        Dim udtDataEntryUser As DataEntryUserModel = Nothing
        Dim blnErr As Boolean = False
        Dim systemMessage As SystemMessage = Nothing
        Dim validator As Common.Validation.Validator = New Common.Validation.Validator
        Dim udtEHSTransaction As EHSTransactionModel = Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        systemMessage = validator.chkSelectedPrintFormOption(strSelectedPrintingOption)
        If Not systemMessage Is Nothing Then
            blnErr = True
            Me.udcMsgBoxErr.AddMessage(systemMessage)
        End If

        If Not blnErr Then
            Me._udtSessionHandler.CurrentUserGetFromSession(Me._udtSP, udtDataEntryUser)

            If udtDataEntryUser Is Nothing Then

                'if Print Option changed
                If strSelectedPrintingOption <> Me._udtSP.PrintOption Then
                    'Assign Print Option to Servive Provider
                    Me._udtSP.PrintOption = strSelectedPrintingOption
                    Me._udtClaimVoucherBLL.updatePrintOption(Me._udtSP.SPID, String.Empty, Me._udtSP.PrintOption)

                    'Save to Session
                    'Me._udtSessionHandler.CurrentUserSaveToSession(Me._udtSP, Nothing)

                    If Session("ChangePrintOptionPage") = "2a" Then
                        Me.ChangePrintFormControl(Me._udtSP.PrintOption, udtSchemeClaim, Me.panStep2aAdhocPrint, Me.panlblStep2aPrintConsent, _
                                                  Me.panStep2aPerprintFormNotice, Me.btnStep2aPrintAdhocClaimConsentForm, Me.btnStep2aPrintClaimConsentForm)
                    Else
                        Me.ChangePrintFormControl(Me._udtSP.PrintOption, udtSchemeClaim, Me.panStep2bAdhocPrint, Me.panlblStep2bPrintConsent, _
                                                  Me.panStep2bPerprintFormNotice, Me.btnStep2bPrintAdhocClaimConsentForm, Me.btnStep2bPrintClaimConsentForm)
                    End If

                    Me.DisableConfirmDeclareCheckBox(Me._udtSP.PrintOption)

                    If udtEHSTransaction IsNot Nothing Then
                        udtEHSTransaction.PrintedConsentForm = False
                        'Me._udtSessionHandler.EHSTransactionSaveToSession(udtEHSTransaction, FunctCode)
                    End If

                End If
            Else
                'if Print Option changed
                If strSelectedPrintingOption <> udtDataEntryUser.PrintOption Then
                    udtDataEntryUser.PrintOption = strSelectedPrintingOption
                    Me._udtClaimVoucherBLL.updatePrintOption(Me._udtSP.SPID, udtDataEntryUser.DataEntryAccount, udtDataEntryUser.PrintOption)

                    'Save to Session
                    'Me._udtSessionHandler.CurrentUserSaveToSession(Me._udtSP, udtDataEntryUser)

                    If Session("ChangePrintOptionPage") = "2a" Then
                        Me.ChangePrintFormControl(udtDataEntryUser.PrintOption, udtSchemeClaim, Me.panStep2aAdhocPrint, Me.panlblStep2aPrintConsent, _
                                                  Me.panStep2aPerprintFormNotice, Me.btnStep2aPrintAdhocClaimConsentForm, Me.btnStep2aPrintClaimConsentForm)
                    Else
                        Me.ChangePrintFormControl(udtDataEntryUser.PrintOption, udtSchemeClaim, Me.panStep2bAdhocPrint, Me.panlblStep2bPrintConsent, _
                                                  Me.panStep2bPerprintFormNotice, Me.btnStep2bPrintAdhocClaimConsentForm, Me.btnStep2bPrintClaimConsentForm)
                    End If

                    Me.DisableConfirmDeclareCheckBox(udtDataEntryUser.PrintOption)

                    If udtEHSTransaction IsNot Nothing Then
                        udtEHSTransaction.PrintedConsentForm = False
                        'Me._udtSessionHandler.EHSTransactionSaveToSession(udtEHSTransaction, FunctCode)
                    End If

                End If
            End If

            Me.hfStep2aCurrentPrintOption.Value = strSelectedPrintingOption
            Me.hfCurrentPrintOption.Value = strSelectedPrintingOption

            EHSClaimBasePage.AuditLogPrintOptionSelected(New AuditLogEntry(FunctionCode, Me), strSelectedPrintingOption)
        Else
            Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail, New AuditLogEntry(FunctCode, Me), Common.Component.LogID.LOG00022, "Print Option Selection Fail")
        End If
    End Sub

    Private Sub btnPopupPrintOptionSelectionCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPopupPrintOptionSelectionCancel.Click

    End Sub

    '---------------------------------------------------------------------------------------------------------
    'Check Dose less than 4 weeks
    '---------------------------------------------------------------------------------------------------------
    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    'Protected Sub btnPopupExclamationConfirmationBoxConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPopupExclamationConfirmationBoxConfirm.Click
    Private Sub ucNoticePopUpExclamationConfirm_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUpExclamationConfirm.ButtonClick
        Select Case e
            Case ucNoticePopUp.enumButtonClick.OK
                ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [Start][Winnie]
                'ExclamationConfirm()
                Me.Step2aClaimSubmit(True)
                ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [End][Winnie]
            Case Else
                ' Do Nothing
        End Select
    End Sub

    '---------------------------------------------------------------------------------------------------------
    'Search RCH
    '---------------------------------------------------------------------------------------------------------
    Private Sub btnPopupRVPHomeListSearchCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPopupRVPHomeListSearchCancel.Click
        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(FunctCode, Me)
        EHSClaimBasePage.AuditLogCancelSearchRCH(udtAuditLogEntry)

        Me._udtSessionHandler.EHSEnterClaimDetailSearchRCHRemoveFromSession()
        Me.ModalPopupExtenderRVPHomeListSearch.Hide()
    End Sub

    Private Sub udcRVPHomeListSearch_RCHSelectedChanged(ByVal blnSelected As Boolean, ByVal sender As Object) Handles udcRVPHomeListSearch.RCHSelectedChanged
        If blnSelected Then
            Me.btnPopupRVPHomeListSearchSelect.Enabled = True
            Me.btnPopupRVPHomeListSearchSelect.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectBtn")
        Else
            Me.btnPopupRVPHomeListSearchSelect.Enabled = False
            Me.btnPopupRVPHomeListSearchSelect.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectDisableBtn")
        End If
    End Sub

    '---------------------------------------------------------------------------------------------------------
    'Search Outreach
    '---------------------------------------------------------------------------------------------------------
    Private Sub btnPopupOutreachListSearchCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPopupOutreachListSearchCancel.Click
        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(FunctCode, Me)
        EHSClaimBasePage.AuditLogCancelSearchOutreach(udtAuditLogEntry)

        Me._udtSessionHandler.EHSEnterClaimDetailSearchOutreachRemoveFromSession()
        Me.ModalPopupExtenderOutreachListSearch.Hide()
    End Sub

    Private Sub udcOutreachListSearch_OutreachSelectedChanged(ByVal blnSelected As Boolean, ByVal sender As Object) Handles udcOutreachSearch.SelectedChanged
        If blnSelected Then
            Me.btnPopupOutreachListSearchSelect.Enabled = True
            Me.btnPopupOutreachListSearchSelect.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectBtn")
        Else
            Me.btnPopupOutreachListSearchSelect.Enabled = False
            Me.btnPopupOutreachListSearchSelect.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectDisableBtn")
        End If
    End Sub

    '---------------------------------------------------------------------------------------------------------
    'Scheme Legend
    '---------------------------------------------------------------------------------------------------------
    Private Sub btnSchemeLegnedClose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSchemeLegnedClose.Click
        Me.ModalPopupExtenderSchemeLegned.Hide()
    End Sub

    '---------------------------------------------------------------------------------------------------------
    'Adhoc Print Selection
    '---------------------------------------------------------------------------------------------------------
    Private Sub btnAddHocPrintSelection_Click1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAddHocPrintSelection.Click

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        Dim udtEHSTransaction As EHSTransactionModel = Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)

        Dim strCurrentPrintOption As String = Me.hfCurrentPrintOption.Value

        Dim strPrintOptionSelectedValue As String = Me.rbStep2bPrintClaimConsentFormLanguage.SelectedValue

        Dim imgbtn As ImageButton = CType(sender, ImageButton)

        If imgbtn.ID = "btnAddHocPrintSelection" AndAlso mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step2a Then
            'Reset Error Image
            If Me.ClaimMode = ClaimMode.COVID19 Then
                Me.udcMsgBoxErr.Clear()

                Me.imgStep2aContactNoError.Visible = False
                Me.imgStep2aDeclareClaimError.Visible = False
                Me.Step2aCleanSchemeErrorImage()
            End If

            '---------------------------------------------------------------------------------------------------------------------------------

            Dim udtFormatter As New Formatter
            Dim udtValidator As New Validator
            'Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
            Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
            Dim udtPracticeDisplay As BLL.PracticeDisplayModel = Me._udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)

            Dim udtDataEntryUser As DataEntryUserModel = Nothing
            Me._udtSessionHandler.CurrentUserGetFromSession(Me._udtSP, udtDataEntryUser)

            Dim udtSubPlatformBLL As New SubPlatformBLL
            Dim strServiceDate As String = udtFormatter.formatInputDate(Me.txtStep2aServiceDate.Text, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))

            Dim isValid As Boolean = True

            Me._udtSystemMessage = udtValidator.chkServiceDate(strServiceDate)

            If Not Me._udtSystemMessage Is Nothing Then
                isValid = False
                Me.imgStep2aServiceDateError.Visible = True
                Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
            Else
                Me.txtStep2aServiceDate.Text = udtFormatter.formatInputTextDate(strServiceDate, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
            End If

            udtSchemeClaim = Me._udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtSchemeClaim.SchemeCode, udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English))

            'Construct EHS Transaction
            Dim udtNewEHSTransaction As EHSTransactionModel = Me._udtEHSClaimBLL.ConstructNewEHSTransaction(udtSchemeClaim, _
                                                                                  udtEHSAccount, _
                                                                                  udtPracticeDisplay, _
                                                                                  udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English), _
                                                                                  GetExtRefStatus(udtEHSAccount, udtSchemeClaim), _
                                                                                  GetDHVaccineRefStatus(udtEHSAccount, udtSchemeClaim))

            If isValid Then

                If Not udtSchemeClaim Is Nothing Then
                    Select Case udtSchemeClaim.ControlType

                        Case SchemeClaimModel.EnumControlType.VSS
                            Dim udcInputVSS As ucInputVSS = Me.udcStep2aInputEHSClaim.GetVSSControl

                            udcInputVSS.SetDoseErrorImage(False)

                            isValid = udcInputVSS.Validate(True, Me.udcMsgBoxErr)

                            If isValid Then
                                udcInputVSS.Save(udtNewEHSTransaction, Me._udtSessionHandler.EHSClaimVaccineGetFromSession())
                            End If

                            ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
                            ' --------------------------------------------------------------------------------------
                        Case SchemeClaimModel.EnumControlType.COVID19

                            Dim udcInputCOVID19 As ucInputCOVID19 = Me.udcStep2aInputEHSClaim.GetCOVID19Control

                            udcInputCOVID19.SetDoseErrorImage(False)

                            isValid = udcInputCOVID19.Validate(True, Me.udcMsgBoxErr)

                            If isValid Then
                                udcInputCOVID19.Save(udtNewEHSTransaction, Me._udtSessionHandler.EHSClaimVaccineGetFromSession())
                            End If
                            ' CRE20-0022 (Immu record) [End][Winnie SUEN]
                    End Select
                End If

                'isValid = Me.Step2aVSSValidation(False, udtNewEHSTransaction)
            End If

            If isValid Then
                'Dim dicVaccineRef As Dictionary(Of String, String) = EHSTransactionModel.GetVaccineRef(GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), udtNewEHSTransaction)
                'Me._udtEHSTransaction.EHSVaccineResult = dicVaccineRef(EHSTransactionModel.VaccineRefType.EHS)
                'Me._udtEHSTransaction.HAVaccineResult = dicVaccineRef(EHSTransactionModel.VaccineRefType.HA)
                'Me._udtEHSTransaction.DHVaccineResult = dicVaccineRef(EHSTransactionModel.VaccineRefType.DH)

                Me._udtEHSClaimBLL.ConstructEHSTransactionDetails(Me._udtSP, udtDataEntryUser, udtNewEHSTransaction, udtEHSAccount, Me._udtSessionHandler.EHSClaimVaccineGetFromSession())

                udtEHSTransaction = udtNewEHSTransaction

            Else
                Dim errorMessageCodeTable As DataTable = Me.udcMsgBoxErr.GetCodeTable
                If errorMessageCodeTable.Rows.Count > 0 Then
                    Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail, udtAuditLogEntry, Common.Component.LogID.LOG00013, String.Format("Enter Claim Detail - Print Consent Form Failed", FunctCode))
                End If

                Return
            End If

            strCurrentPrintOption = Me.hfStep2aCurrentPrintOption.Value
            strPrintOptionSelectedValue = Me.rbStep2aPrintClaimConsentFormLanguage.SelectedValue
        End If
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        Dim strPrintDateTime As String = String.Format("DH_HCV103{0}{1}{2}{3}{4}{5}{6}", Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, Now.Second, Now.Millisecond)

        If udtEHSTransaction IsNot Nothing Then
            udtEHSTransaction.PrintedConsentForm = True
            ''Set the transaction is printed consent Form
            'Me._udtSessionHandler.EHSTransactionSaveToSession(udtEHSTransaction, FunctCode)
        End If

        Dim strPrintOptionSelectedLang As String = Nothing
        Dim strPrintOptionSelectedVersion As String = Nothing

        Dim slConsentFormAvailableLang As String() = Nothing
        Dim intPlatform As Integer = SubPlatform()

        Select Case intPlatform
            Case EnumHCSPSubPlatform.HK
                slConsentFormAvailableLang = udtSchemeClaim.SubsidizeGroupClaimList(0).ConsentFormAvailableLang

            Case EnumHCSPSubPlatform.CN
                slConsentFormAvailableLang = udtSchemeClaim.SubsidizeGroupClaimList(0).ConsentFormAvailableLang_CN
        End Select


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

        'Set the transaction is printed consent Form
        Me._udtSessionHandler.EHSTransactionSaveToSession(udtEHSTransaction, FunctCode)

        ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
        ' --------------------------------------------------------------------------------------
        'Save the current function code to session (will be removed in the printout form)
        Me._udtSessionHandler.EHSClaimPrintoutFunctionCodeSaveToSession(FunctCode)
        ' CRE20-0022 (Immu record) [End][Winnie SUEN]

        _udtSessionHandler.EHSClaimTempTransactionIDSaveToSession(Me._udtGeneralFunction.generateTemporaryTransactionNumber(udtSchemeClaim.SchemeCode.Trim()))

        'CRE15-003 System-generated Form [Start][Philip Chau]
        Select Case strPrintOptionSelectedLang
            Case PrintOptionLanguage.TradChinese 'PrintOptionValue.Chi
                If strPrintOptionSelectedVersion = PrintFormOptionValue.PrintConsentOnly Then
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/EHSClaimCondensedForm_CHI_RV.aspx?TID=" + strPrintDateTime + "');", True)
                    EHSClaimBasePage.AuditLogPrintFrom(udtAuditLogEntry, True, "EHSClaimCondensedForm_CHI_RV", strPrintOptionSelectedLang, _udtSessionHandler.EHSClaimTempTransactionIDGetFromSession())
                ElseIf strPrintOptionSelectedVersion = PrintFormOptionValue.PrintPurposeAndConsent Then
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/EHSClaimForm_CHI_RV.aspx?TID=" + strPrintDateTime + "');", True)
                    EHSClaimBasePage.AuditLogPrintFrom(udtAuditLogEntry, True, "EHSClaimForm_CHI_RV", strPrintOptionSelectedLang, _udtSessionHandler.EHSClaimTempTransactionIDGetFromSession())
                End If
            Case PrintOptionLanguage.English ' PrintOptionValue.Eng
                If strPrintOptionSelectedVersion = PrintFormOptionValue.PrintConsentOnly Then
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/EHSClaimCondensedForm_RV.aspx?TID=" + strPrintDateTime + "');", True)
                    EHSClaimBasePage.AuditLogPrintFrom(udtAuditLogEntry, True, "EHSClaimCondensedForm_RV", strPrintOptionSelectedLang, _udtSessionHandler.EHSClaimTempTransactionIDGetFromSession())
                ElseIf strPrintOptionSelectedVersion = PrintFormOptionValue.PrintPurposeAndConsent Then
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/EHSClaimForm_RV.aspx?TID=" + strPrintDateTime + "');", True)
                    EHSClaimBasePage.AuditLogPrintFrom(udtAuditLogEntry, True, "EHSClaimForm_RV", strPrintOptionSelectedLang, _udtSessionHandler.EHSClaimTempTransactionIDGetFromSession())
                End If
            Case PrintOptionLanguage.SimpChinese
                If strPrintOptionSelectedVersion = PrintFormOptionValue.PrintConsentOnly Then
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/EHSClaimCondensedForm_CN_RV.aspx?TID=" + strPrintDateTime + "');", True)
                    EHSClaimBasePage.AuditLogPrintFrom(udtAuditLogEntry, True, "EHSClaimCondensedForm_CN_RV", strPrintOptionSelectedLang, _udtSessionHandler.EHSClaimTempTransactionIDGetFromSession())
                ElseIf strPrintOptionSelectedVersion = PrintFormOptionValue.PrintPurposeAndConsent Then
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/EHSClaimForm_CN_RV.aspx?TID=" + strPrintDateTime + "');", True)
                    EHSClaimBasePage.AuditLogPrintFrom(udtAuditLogEntry, True, "EHSClaimForm_CN_RV", strPrintOptionSelectedLang, _udtSessionHandler.EHSClaimTempTransactionIDGetFromSession())
                End If
        End Select
        'CRE15-003 System-generated Form [End][Philip Chau]
        'CRE13-019-02 Extend HCVS to China [End][Winnie]

        Me.ModalPopupExtenderAddHocPrintSelection.Hide()
    End Sub

    Private Sub btnAddHocPrintSelectionCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAddHocPrintSelectionCancel.Click
        EHSClaimBasePage.AuditLogAdhocPrintCancelClick(_udtAuditLogEntry)
    End Sub

    Private Sub btnPopupRVPHomeListSearchSelect_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPopupRVPHomeListSearchSelect.Click
        Dim strRCHCode As String = Me.udcRVPHomeListSearch.getSelectedCode()

        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(FunctCode, Me)
        EHSClaimBasePage.AuditLogSelectRCH(udtAuditLogEntry, strRCHCode)

        Dim udtSelectedScheme As SchemeClaimModel = _udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        Select Case udtSelectedScheme.SchemeCode.Trim.ToUpper
            Case SchemeClaimModel.RVP
                If Not strRCHCode Is Nothing AndAlso strRCHCode.Trim() <> "" Then
                    Me._udtSessionHandler.RVPRCHCodeSaveToSession(FunctCode, strRCHCode.Trim())
                End If

                If ClaimMode = Common.Component.ClaimMode.COVID19 Then
                    CType(Me.udcStep2aInputEHSClaim.GetRVPCOVID19Control(), ucInputRVPCOVID19).SetRCHCode(strRCHCode)
                Else
                    CType(Me.udcStep2aInputEHSClaim.GetRVPControl(), ucInputRVP).SetRCHCode(strRCHCode)
                End If

            Case SchemeClaimModel.VSS
                CType(Me.udcStep2aInputEHSClaim.GetVSSControl(), ucInputVSS).SetPIDCode(strRCHCode)

            Case SchemeClaimModel.COVID19RVP
                If Not strRCHCode Is Nothing AndAlso strRCHCode.Trim() <> "" Then
                    Me._udtSessionHandler.RVPRCHCodeSaveToSession(FunctCode, strRCHCode.Trim())
                End If

                'CType(Me.udcStep2aInputEHSClaim.GetCOVID19RVPControl(), ucInputCOVID19RVP).SetRCHCode(strRCHCode, udcMsgBoxInfo)
                CType(Me.udcStep2aInputEHSClaim.GetCOVID19RVPControl(), ucInputCOVID19RVP).SetRCHCode(strRCHCode)

                Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Information
                Me.udcMsgBoxInfo.BuildMessageBox()

            Case Else
                'Nothing to do
        End Select

        Me._udtSessionHandler.EHSEnterClaimDetailSearchRCHRemoveFromSession()
        Me.ModalPopupExtenderRVPHomeListSearch.Hide()
    End Sub

    Private Sub udcInputEHSClaim_RCHCodeTextChanged(ByVal strSchemeCode As String, ByVal sender As Object, ByVal e As System.EventArgs)
        Dim udtSelectedScheme As SchemeClaimModel = _udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        Select Case udtSelectedScheme.SchemeCode
            Case SchemeClaimModel.EnumControlType.COVID19RVP.ToString.Trim
                'Dim udcInputCOVID19RVP As ucInputCOVID19RVP = Me.udcStep2aInputEHSClaim.GetCOVID19RVPControl()

                'udcInputCOVID19RVP.SearchVaccineLotNoByRCHCode(udcMsgBoxInfo)

                'Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Information
                'Me.udcMsgBoxInfo.BuildMessageBox()
            Case Else
                'Nothing to do
        End Select

    End Sub

    Private Sub btnPopupOutreachListSearchSelect_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPopupOutreachListSearchSelect.Click
        Dim strOutreachCode As String = Me.udcOutreachSearch.GetSelectedCode()

        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(FunctCode, Me)
        EHSClaimBasePage.AuditLogSelectOutreach(udtAuditLogEntry, strOutreachCode)

        Dim udtSelectedScheme As SchemeClaimModel = _udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        Select Case udtSelectedScheme.SchemeCode.Trim.ToUpper
            Case SchemeClaimModel.COVID19OR
                If Not strOutreachCode Is Nothing AndAlso strOutreachCode.Trim() <> "" Then
                    Me._udtSessionHandler.OutreachCodeSaveToSession(FunctCode, strOutreachCode.Trim())
                End If

                CType(Me.udcStep2aInputEHSClaim.GetCOVID19ORControl(), ucInputCOVID19OR).SetOutreachCode(strOutreachCode)

            Case SchemeClaimModel.RVP
                If Not strOutreachCode Is Nothing AndAlso strOutreachCode.Trim() <> "" Then
                    Me._udtSessionHandler.OutreachCodeSaveToSession(FunctCode, strOutreachCode.Trim())
                End If

                CType(Me.udcStep2aInputEHSClaim.GetRVPCOVID19Control(), ucInputRVPCOVID19).SetOutreachCode(strOutreachCode)


            Case Else
                'Nothing to do
        End Select

        Me._udtSessionHandler.EHSEnterClaimDetailSearchOutreachRemoveFromSession()
        Me.ModalPopupExtenderOutreachListSearch.Hide()
    End Sub

    ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [Start][Winnie]
    Private Sub ucNoticePopUpDuplicateClaimAlert_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUpDuplicateClaimAlert.ButtonClick
        Select Case e
            Case ucNoticePopUp.enumButtonClick.OK
                EHSClaimBasePage.AuditLogDuplicateClaimAlertProceedClick(_udtAuditLogEntry)
                _udtSessionHandler.NoticedDuplicateClaimAlertSaveToSession(YesNo.Yes)
                Me.Step2aClaimSubmit(True)

            Case Else
                EHSClaimBasePage.AuditLogDuplicateClaimAlertNotProceedClick(_udtAuditLogEntry)
        End Select
    End Sub
    ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [End][Winnie]

    '---------------------------------------------------------------------------------------------------------
    'Scheme DocType Legend
    '---------------------------------------------------------------------------------------------------------
    Private Sub btnSchemeDocTypeLegnedClose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSchemeDocTypeLegnedClose.Click
        Me.ModalPopupExtenderSchemeDocTypeLegend.Hide()
    End Sub

    '==================================================================== Code for SmartID ============================================================================
    'Protected Sub btnPopupSmartIDDeclarationBoxConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPopupSmartIDDeclarationBoxConfirm.Click
    '    Me.RedirectToIdeas(True)
    '    Me.ModalPopupExtenderSmartIDDeclaration.Hide()
    'End Sub

    'Protected Sub btnPopupSmartIDDeclarationBoxCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPopupSmartIDDeclarationBoxCancel.Click

    'End Sub
    '==================================================================================================================================================================
#End Region

#Region "Step 1 Search Account Events"

    ' Events

    Private Sub ddlStep1Scheme_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlStep1Scheme.SelectedIndexChanged
        ' Handle concurrent browser
        If Not EHSClaimTokenNumValidation() Then Return


        'PS: Setup Me.udcStep1DocumentTypeRadioButtonGroup was done by function SetupStep1 
        'But Me.udcClaimSearch must built by this function. If not do that, some unexpected UI Error will show
        ' tmk791: Hi previous developer, I don't know what you are talking about


        ' Audit Log - Change Scheme
        Dim udtPreviousSelectedScheme As SchemeClaimModel = _udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
        Dim strPreviousSelectedScheme As String = String.Empty
        If Not IsNothing(udtPreviousSelectedScheme) Then strPreviousSelectedScheme = udtPreviousSelectedScheme.SchemeCode.Trim

        EHSClaimBasePage.AuditLogSearchAccountChangeScheme(New AuditLogEntry(FunctionCode, Me), strPreviousSelectedScheme, ddlStep1Scheme.SelectedValue)

        Dim blnRetainDocType As Boolean = False

        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        panNonClinicSettingStep1.Visible = False

        If ddlStep1Scheme.SelectedValue = String.Empty Then
            ' ----- No scheme selected -----

            ' Remove session
            _udtSessionHandler.SchemeSelectedRemoveFromSession(FunctCode)
            _udtSessionHandler.NonClinicSettingRemoveFromSession(FunctCode)

        Else
            ' ----- Scheme selected -----

            ' CRE20-0023 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Dim udtSelectedPracticeDisplay As PracticeDisplayModel = _udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)

            ' Practice Scheme Info List Filter by COVID-19
            Dim udtFilterPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = _udtSchemeClaimBLL.FilterPracticeSchemeInfo(_udtSP.PracticeList, udtSelectedPracticeDisplay.PracticeID, Me.ClaimMode())

            ' Get valid schemes
            Dim udtSchemeClaimList As SchemeClaimModelCollection = _udtSchemeClaimBLL.searchValidClaimPeriodSchemeClaimByPracticeSchemeInfoSubsidizeCode( _
                                                                    udtFilterPracticeSchemeInfoList, _udtSP.SchemeInfoList)

            udtSchemeClaimList = udtSchemeClaimList.FilterWithoutReadonly()

            udtSchemeClaimList = udtSchemeClaimList.FilterByHCSPSubPlatform(Me.SubPlatform)

            ' Save the selected scheme to session
            'Dim udtSchemeClaim As SchemeClaimModel = (New SchemeClaimBLL).getValidClaimPeriodSchemeClaimBySchemeCode(ddlStep1Scheme.SelectedValue)
            Dim udtSchemeClaim As SchemeClaimModel = udtSchemeClaimList.Filter(ddlStep1Scheme.SelectedValue.Trim)

            _udtSessionHandler.SchemeSelectedSaveToSession(udtSchemeClaim, FunctCode)
            ' CRE20-0022 (Immu record) [End][Chris YIM]

            If Not udtSelectedPracticeDisplay Is Nothing Then
                Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = _udtSP.PracticeList(udtSelectedPracticeDisplay.PracticeID).PracticeSchemeInfoList.Filter(ddlStep1Scheme.SelectedValue)

                If Not udtPracticeSchemeInfoList Is Nothing AndAlso udtPracticeSchemeInfoList.Count > 0 Then
                    If udtPracticeSchemeInfoList.IsNonClinic Then
                        panNonClinicSettingStep1.Visible = True

                        If _udtSessionHandler.Language = CultureLanguage.TradChinese Then
                            lblNonClinicSettingStep1.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)))
                        ElseIf _udtSessionHandler.Language = CultureLanguage.SimpChinese Then
                            lblNonClinicSettingStep1.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese)))
                        Else
                            lblNonClinicSettingStep1.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.English)))
                        End If
                    End If
                End If
            End If

            _udtSessionHandler.NonClinicSettingSaveToSession(panNonClinicSettingStep1.Visible, FunctCode)

            Dim udtSchemeDocTypeList As SchemeDocTypeModelCollection = (New DocTypeBLL).getSchemeDocTypeByScheme(ddlStep1Scheme.SelectedValue)
            blnRetainDocType = RetainDocType(udtSchemeDocTypeList, udcStep1DocumentTypeRadioButtonGroup.SelectedValue, False)

        End If
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

        _udtSessionHandler.ForceToReadSmartICSaveToSession(True)
        Step1Reset(blnRetainDocType)
        SetupStep1(False, False)

    End Sub

    Private Sub udcStep1DocumentTypeRadioButtonGroup_CheckedChanged(ByVal sender As Object, ByVal e As CustomControls.DocumentTypeRadioButtonGroup.DocumentTypeRadioButtonGroupArgs) Handles udcStep1DocumentTypeRadioButtonGroup.CheckedChanged
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        EHSClaimBasePage.AuditLogSearchAccountSelectDocumentType(_udtAuditLogEntry, udcStep1DocumentTypeRadioButtonGroup.SelectedValue)
        ' CRE11-021 log the missed essential information [End]

        ' Handle concurrent browser
        If Not EHSClaimTokenNumValidation() Then Return

        Step1Reset(False)
        SetupStep1(False, False)

    End Sub

    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub udcStep1DocumentTypeRadioButtonGroup_LegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcStep1DocumentTypeRadioButtonGroup.LegendClicked
        ' Handle concurrent browser
        If Not EHSClaimTokenNumValidation() Then Return

        udcSchemeDocTypeLegend.Build(_udtSessionHandler.Language, (New DocTypeBLL).getAllDocType())

        ModalPopupExtenderSchemeDocTypeLegend.Show()

    End Sub
    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

    Private Sub btnStep1ChangePractice_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnStep1ChangePractice.Click, btnStep2aChangePractice.Click

        ' To Handle Concurrent Browser
        If Not Me.EHSClaimTokenNumValidation() Then
            Return
        End If

        Me.ModalPopupPracticeSelection.Show()

        'Reset All Document type and fields to emtpy 
        'Me.Step1Reset(False)
    End Sub

    Private Sub udcClaimSearch_HKICSymbolListClick(ByVal sender As System.Object, ByVal e As EventArgs) Handles udcClaimSearch.HKICSymbolListClick
        _udtSessionHandler.HKICSymbolSaveToSession(FunctCode, Me.udcClaimSearch.HKICSymbolValue)
    End Sub

    Private Sub udcClaimSearch_CancelButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcClaimSearch.CancelButtonClick
        udcMsgBoxErr.Clear()
        udcMsgBoxInfo.Clear()

        ' CRE14-007 - Fix VRE display for same doc no. in HKIC and EC [Start][Lawrence]
        _udtSessionHandler.ClearVREClaim()
        '_udtSessionHandler.FromVaccinationRecordEnquiryRemoveFromSession()
        ' CRE14-007 - Fix VRE display for same doc no. in HKIC and EC [End][Lawrence]

        udcClaimSearch.CleanField()

        Dim strDocCode As String = udcStep1DocumentTypeRadioButtonGroup.SelectedValue
        Dim udtSchemeClaim As SchemeClaimModel = _udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        If Not IsNothing(udtSchemeClaim) AndAlso Not IsDocumentAcceptedForScheme(strDocCode, udtSchemeClaim.SchemeCode) Then
            udcStep1DocumentTypeRadioButtonGroup.SelectedValue = String.Empty
        End If

        SetupStep1(False, False)

    End Sub

    ' INT20-0021 (Add auditlog for click UpdateNow & Fix GetEHSVaccine web service ) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub udcClaimSearch_UpdateNowClick(ByVal sender As System.Object, ByVal e As EventArgs) Handles udcClaimSearch.UpdateNowLinkButtonClick
        Dim udtAuditLogEntry As New AuditLogEntry(FunctCode, Me)

        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00088, "Click Update Now for software of reading Smart ID card")

    End Sub
    ' INT20-0021 (Add auditlog for click UpdateNow & Fix GetEHSVaccine web service ) [End][Chris YIM]	

    ' INT20-0021 (Add auditlog for click UpdateNow & Fix GetEHSVaccine web service ) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub udcClaimSearch_HereClick(ByVal sender As System.Object, ByVal e As EventArgs) Handles udcClaimSearch.HereLinkButtonClick
        Dim udtAuditLogEntry As New AuditLogEntry(FunctCode, Me)

        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00089, "Click HERE for software of reading Smart ID card")

    End Sub
    ' INT20-0021 (Add auditlog for click UpdateNow & Fix GetEHSVaccine web service ) [End][Chris YIM]	

    '==================================================================== Code for SmartID ============================================================================
    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub udcClaimSearch_ReadSmartIDButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcClaimSearch.ReadSmartIDButtonClick
        Me._udtSessionHandler.UIDisplayHKICSymbolSaveToSession(FunctCode, Me.udcClaimSearch.UIDisplayHKICSymbol)
        Me._udtSessionHandler.OCSSSRefStatusRemoveFromSession(FunctCode)
        If ClaimMode = Common.Component.ClaimMode.COVID19 Then
            _udtSessionHandler.ClaimCOVID19CarryForwordSaveToSession(False, FunctCode)
            _udtSessionHandler.ClaimCOVID19VaccineBrandRemoveFromSession(FunctCode)
            _udtSessionHandler.ClaimCOVID19VaccineLotNoRemoveFromSession(FunctCode)
        End If

        Me.RedirectToIdeas(False, IdeasBLL.EnumIdeasVersion.Two)

        'Carry Forword: once times only
        If _udtSessionHandler.ClaimCOVID19CarryForwordGetFromSession(FunctCode) = False Then
            _udtSessionHandler.ClaimCOVID19CarryForwordSaveToSession(True, FunctCode)
        End If

    End Sub
    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub udcClaimSearch_ReadOldSmartIDButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcClaimSearch.ReadOldSmartIDButtonClick
        Me._udtSessionHandler.UIDisplayHKICSymbolSaveToSession(FunctCode, Me.udcClaimSearch.UIDisplayHKICSymbol)
        Me._udtSessionHandler.OCSSSRefStatusRemoveFromSession(FunctCode)
        If ClaimMode = Common.Component.ClaimMode.COVID19 Then
            _udtSessionHandler.ClaimCOVID19CarryForwordSaveToSession(False, FunctCode)
            _udtSessionHandler.ClaimCOVID19VaccineBrandRemoveFromSession(FunctCode)
            _udtSessionHandler.ClaimCOVID19VaccineLotNoRemoveFromSession(FunctCode)
        End If

        Me.RedirectToIdeas(False, IdeasBLL.EnumIdeasVersion.One)

        'Carry Forword: once times only
        If _udtSessionHandler.ClaimCOVID19CarryForwordGetFromSession(FunctCode) = False Then
            _udtSessionHandler.ClaimCOVID19CarryForwordSaveToSession(True, FunctCode)
        End If

    End Sub
    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub udcClaimSearch_ReadNewSmartIDComboButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcClaimSearch.ReadNewSmartIDComboButtonClick
        Me._udtSessionHandler.UIDisplayHKICSymbolSaveToSession(FunctCode, Me.udcClaimSearch.UIDisplayHKICSymbol)
        Me._udtSessionHandler.OCSSSRefStatusRemoveFromSession(FunctCode)
        If ClaimMode = Common.Component.ClaimMode.COVID19 Then
            _udtSessionHandler.ClaimCOVID19CarryForwordSaveToSession(False, FunctCode)
            _udtSessionHandler.ClaimCOVID19VaccineBrandRemoveFromSession(FunctCode)
            _udtSessionHandler.ClaimCOVID19VaccineLotNoRemoveFromSession(FunctCode)
        End If

        Me.RedirectToIdeasCombo(IdeasBLL.EnumIdeasVersion.Combo)

        'Carry Forword: once times only
        If _udtSessionHandler.ClaimCOVID19CarryForwordGetFromSession(FunctCode) = False Then
            _udtSessionHandler.ClaimCOVID19CarryForwordSaveToSession(True, FunctCode)
        End If

    End Sub
    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

    '==================================================================================================================================================================

    Private Sub udcClaimSearch_SearchButtonClick(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcClaimSearch.SearchButtonClick
        ' To Handle Concurrent Browser
        If Not Me.EHSClaimTokenNumValidation() Then
            Return
        End If

        Dim udtAuditLogEntry As New AuditLogEntry(FunctCode, Me)

        ' -- Prepare the selected value --
        ' (1) Scheme
        Dim udtSchemeClaim As SchemeClaimModel = _udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        If IsNothing(udtSchemeClaim) Then Throw New Exception("EHSClaimV1.udcClaimSearch_SearchButtonClick: udtSchemeClaim is nothing")

        Dim strSchemeCode As String = String.Empty
        strSchemeCode = udtSchemeClaim.SchemeCode.Trim

        ' (2) Document
        Dim strSearchDocCode As String = udcStep1DocumentTypeRadioButtonGroup.SelectedValue

        Me.udcClaimSearch.SetProperty(strSearchDocCode)

        ' Check document is accepted for the current scheme
        If Not IsDocumentAcceptedForScheme(strSearchDocCode, strSchemeCode) Then
            udcMsgBoxErr.AddMessage(FunctCode, SeverityCode.SEVE, MsgCode.MSG00001)
            udcMsgBoxErr.BuildMessageBox(_strValidationFail, udtAuditLogEntry, LogID.LOG00005, "Search Account Failed", _
                New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, strSearchDocCode, (New Formatter).formatDocumentIdentityNumber(strSearchDocCode, SearchGetDocumentNo(strSearchDocCode))))
            Return
        End If

        ' (3) EHSAccount from Vaccination Record Enquiry
        Dim udtVREEHSAccount As EHSAccountModel = Nothing
        Dim udtVREEHSPersonalInfo As EHSPersonalInformationModel = Nothing

        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Dim udtSmartIDContent As BLL.SmartIDContentModel = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode)
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

        Dim strSearchFrom As String = "Claim"

        If CheckFromVaccinationRecordEnquiry() Then
            udtVREEHSAccount = _udtSessionHandler.VREEHSAccountGetFromSession
            udtVREEHSPersonalInfo = udtVREEHSAccount.EHSPersonalInformationList(0)

            ' Fill the scheme code as the account is searched in Vaccination Record Enquiry, no scheme selection is required
            udtVREEHSAccount.SchemeCode = strSchemeCode

            _udtSessionHandler.VREEHSAccountSaveToSession(udtVREEHSAccount)

            If udtVREEHSPersonalInfo.ENameSurName = String.Empty Then udtVREEHSAccount = Nothing

            strSearchFrom = "VRE"

            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            If Not IsNothing(udtSmartIDContent) Then
                strSearchFrom += "SmartIC"
                strSearchFrom += "_IDEAS" + IdeasBLL.ConvertIdeasVersion(udtSmartIDContent.IdeasVersion)
            End If
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
        End If

        Dim udtSearchAccountStatus As New EHSClaimBLL.SearchAccountStatus

        Dim udtEHSAccount As EHSAccountModel = Nothing
        Dim isValid As Boolean = True
        Dim strIdentityNo As String = String.Empty
        Dim strIdentityNoPrefix As String = String.Empty
        Dim strDOB As String = String.Empty
        Dim strECAge As String = String.Empty

        Dim udtEligibleResult As EligibleResult = Nothing


        'Init controls
        Me.udcMsgBoxErr.Clear()
        Me.udcMsgBoxInfo.Clear()
        Me.udcClaimSearch.SetHKICError(False)
        Me.udcClaimSearch.SetECError(False)
        Me.udcClaimSearch.SetADOPCError(False)
        Me.udcClaimSearch.SetSearchShortError(False)
        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Me._udtSessionHandler.OCSSSRefStatusRemoveFromSession(FunctCode)
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ' Read Smart IC (From VRE)
        If CheckFromVaccinationRecordEnquiry() AndAlso Not udtSmartIDContent Is Nothing Then
            ' As different DOB with Validate Acct is not allowed by Manual Input
            ' Change Search process, similar to click Read Smart IC in claim page

            Dim udtAuditLogEntry_Search As New AuditLogEntry(FunctCode, Me)
            EHSClaimBasePage.AuditLogSearchNvaliatedACwithCFD(udtAuditLogEntry_Search, udtSchemeClaim.SchemeCode, SearchGetDocumentNo(strSearchDocCode), String.Empty, IdeasBLL.ConvertIdeasVersion(udtSmartIDContent.IdeasVersion), strSearchFrom)

            Me._udtSessionHandler.UIDisplayHKICSymbolSaveToSession(FunctCode, Me.udcClaimSearch.UIDisplayHKICSymbol)

            SearchSmartID(udtSmartIDContent, isValid, udtAuditLogEntry_Search)

            Me.udcMsgBoxErr.BuildMessageBox(_strValidationFail, udtAuditLogEntry_Search, Common.Component.LogID.LOG00052, "Search & validate account with CFD Fail", _
                New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocTypeModel.DocTypeCode.HKIC, (New Formatter).formatDocumentIdentityNumber(strSearchDocCode, SearchGetDocumentNo(strSearchDocCode))))

            Return
        End If
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

        ' Set doc code to make claim search control can determine which identity no user entered
        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        EHSClaimBasePage.AuditLogSearchAccountStart(udtAuditLogEntry, strSchemeCode, strSearchDocCode, _udtSessionHandler.HKICSymbolGetFormSession(FunctCode), _
                                                    (New Formatter).formatDocumentIdentityNumber(strSearchDocCode, SearchGetDocumentNo(strSearchDocCode)), _
                                                    Me.udcClaimSearch.DOB, strSearchFrom)
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        Select Case strSearchDocCode
            Case DocTypeModel.DocTypeCode.HKIC

                'Searh HKIC case fields validation -----------------------------------------------------------
                isValid = Me.Step1SearchValdiation(strSearchDocCode, strIdentityNo, strDOB, strIdentityNoPrefix)

                If isValid Then

                    'Log Enter Info
                    EHSClaimBasePage.AuditLogSearchAccountInfo(New AuditLogEntry(FunctionCode, Me), strSchemeCode, strSearchDocCode, _
                                                               _udtSessionHandler.HKICSymbolGetFormSession(FunctCode), strIdentityNo, strDOB, Nothing, Nothing)

                    strIdentityNo = strIdentityNo.Replace("(", String.Empty).Replace(")", String.Empty)

                    _udtSessionHandler.UIDisplayHKICSymbolSaveToSession(FunctCode, Me.udcClaimSearch.UIDisplayHKICSymbol)

                    ' ----------------------------------------------
                    ' 1. Search account in EHS 
                    ' ----------------------------------------------
                    ' Manual Input
                    _udtSystemMessage = _udtEHSClaimBLL.SearchEHSAccount(udtSchemeClaim.SchemeCode.Trim(), _
                                                                         udcStep1DocumentTypeRadioButtonGroup.SelectedValue.Trim(), _
                                                                         strIdentityNo, _
                                                                         strDOB, _
                                                                         udtEHSAccount, _
                                                                         udtEligibleResult, _
                                                                         udtSearchAccountStatus, _
                                                                         udtVREEHSPersonalInfo, _
                                                                         String.Empty, _
                                                                         FunctionCode, _
                                                                         Me.ClaimMode)

                    ' INT18-XXX (Refine auditlog) [Start][Chris YIM]
                    ' --------------------------------------------------------------------------------------
                    'Log Enter Info Complete
                    EHSClaimBasePage.AuditLogSearchAccountInfoEnd(New AuditLogEntry(FunctionCode, Me), isValid)
                    ' INT18-XXX (Refine auditlog) [End][Chris YIM]

                    ' ----------------------------------------------
                    ' 2. Call OCSSS to check HKIC if input is shown
                    ' ----------------------------------------------
                    If Me._udtSystemMessage Is Nothing Then
                        ' HKIC must be formated in 9 characters e.g. " A1234567" or "CD1234567"
                        If Me.udcClaimSearch.UIDisplayHKICSymbol Then
                            ' INT18-XXX (Refine auditlog) [Start][Chris YIM]
                            ' --------------------------------------------------------------------------------------
                            'Log Enter Info
                            EHSClaimBasePage.AuditLogSearchOCSSSStart(New AuditLogEntry(FunctionCode, Me), strSearchDocCode, _
                                                                       _udtSessionHandler.HKICSymbolGetFormSession(FunctCode), strIdentityNo)

                            CheckHKIDByOCSSS((New Formatter).formatDocumentIdentityNumber(DocTypeModel.DocTypeCode.HKIC, strIdentityNo), Me._udtSP.SPID, strSchemeCode)

                            If Me._udtSystemMessage Is Nothing Then
                                EHSClaimBasePage.AuditLogSearchOCSSSEnd(New AuditLogEntry(FunctionCode, Me), True)
                            Else
                                EHSClaimBasePage.AuditLogSearchOCSSSEnd(New AuditLogEntry(FunctionCode, Me), False)
                            End If

                            ' INT18-XXX (Refine auditlog) [End][Chris YIM]
                        Else
                            Me._udtSessionHandler.HKICSymbolRemoveFromSession(FunctCode)
                        End If
                    End If

                    ' ----------------------------------------------
                    ' Search account error issue
                    ' ----------------------------------------------
                    If Not Me._udtSystemMessage Is Nothing Then
                        'Validation Failed
                        isValid = False

                        Select Case Me._udtSystemMessage.MessageCode
                            Case "00142", "00141"
                                Me.udcClaimSearch.SetHKICNoError(True)
                            Case "00110"
                                Me.udcClaimSearch.SetHKICDOBError(True)
                        End Select

                        Me.udcMsgBoxErr.AddMessage(_udtSystemMessage)
                    Else
                        'Validation Success

                        'Store residential status in model
                        If udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC) Is Nothing And _
                           Not udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKBC) Is Nothing Then

                            udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKBC).HKICSymbol = String.Empty
                        Else
                            udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).HKICSymbol = _udtSessionHandler.HKICSymbolGetFormSession(FunctCode)
                        End If
                    End If
                End If

                ' CRE20-0022 (Immu record) [Start][Martin]
            Case DocTypeModel.DocTypeCode.HKBC, _
                DocTypeModel.DocTypeCode.DI, _
                DocTypeModel.DocTypeCode.REPMT, _
                DocTypeModel.DocTypeCode.ID235B, _
                DocTypeModel.DocTypeCode.VISA, _
                DocTypeModel.DocTypeCode.ADOPC, _
                DocTypeModel.DocTypeCode.OW, _
                DocTypeModel.DocTypeCode.RFNo8, _
                DocTypeModel.DocTypeCode.CCIC, _
                DocTypeModel.DocTypeCode.ROP140, _
                DocTypeModel.DocTypeCode.PASS
                ' CRE20-0022 (Immu record) [End][Martin]

                ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                ' ----------------------------------------------------------
                Me._udtSessionHandler.HKICSymbolRemoveFromSession(FunctCode)
                ' CRE17-010 (OCSSS integration) [End][Chris YIM]

                'Searh HKIC case fields validation -----------------------------------------------------------
                isValid = Me.Step1SearchValdiation(strSearchDocCode, strIdentityNo, strDOB, strIdentityNoPrefix)
                If isValid Then
                    'Log Enter Info
                    EHSClaimBasePage.AuditLogSearchAccountInfo(New AuditLogEntry(FunctionCode, Me), strSchemeCode, strSearchDocCode, Nothing, strIdentityNo, strDOB, Nothing, Nothing)

                    'strIdentityNo = strIdentityNo.Replace("(", String.Empty).Replace(")", String.Empty)

                    Dim strAdoptionPrefixNum As String = String.Empty

                    If strSearchDocCode = DocTypeModel.DocTypeCode.ADOPC Then
                        strAdoptionPrefixNum = strIdentityNoPrefix
                    End If

                    _udtSystemMessage = _udtEHSClaimBLL.SearchEHSAccount(udtSchemeClaim.SchemeCode.Trim, _
                        udcStep1DocumentTypeRadioButtonGroup.SelectedValue.Trim, strIdentityNo, strDOB, udtEHSAccount, udtEligibleResult, _
                        udtSearchAccountStatus, udtVREEHSPersonalInfo, strAdoptionPrefixNum, FunctionCode, Me.ClaimMode)

                    If Not Me._udtSystemMessage Is Nothing Then
                        isValid = False

                        If Me._udtSystemMessage.MessageCode = "00142" Or Me._udtSystemMessage.MessageCode = "00141" Then
                            Me.udcClaimSearch.SetSearchShortIdentityNoError(True)
                        ElseIf Me._udtSystemMessage.MessageCode = "00110" Then
                            Me.udcClaimSearch.SetSearchShortDOBError(True)
                        End If

                        Me.udcMsgBoxErr.AddMessage(_udtSystemMessage)
                        'Else
                        '    If Not udtEHSAccount.EHSPersonalInformationList(0).DocCode.Equals(strSearchDocCode) Then

                    End If

                    ' INT18-XXX (Refine auditlog) [Start][Chris YIM]
                    ' --------------------------------------------------------------------------------------
                    'Log Enter Info Complete
                    EHSClaimBasePage.AuditLogSearchAccountInfoEnd(New AuditLogEntry(FunctionCode, Me), isValid)
                    ' INT18-XXX (Refine auditlog) [End][Chris YIM]

                End If

            Case DocTypeModel.DocTypeCode.EC
                Dim dtmDateOfReg As DateTime

                ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                ' ----------------------------------------------------------
                Me._udtSessionHandler.HKICSymbolRemoveFromSession(FunctCode)
                ' CRE17-010 (OCSSS integration) [End][Chris YIM]

                'Searh EC case fields validation -----------------------------------------------------------
                isValid = Me.Step1SearchECValdiation(strIdentityNo, strDOB, strECAge, dtmDateOfReg)

                If isValid Then
                    'Log Enter Info
                    EHSClaimBasePage.AuditLogSearchAccountInfo(New AuditLogEntry(FunctionCode, Me), strSchemeCode, strSearchDocCode, Nothing, strIdentityNo, strDOB, strECAge, dtmDateOfReg)

                    strIdentityNo = strIdentityNo.Replace("(", String.Empty).Replace(")", String.Empty)

                    If Not strECAge Is Nothing AndAlso Not strECAge.Trim().Equals(String.Empty) Then
                        ' Age 99 on DD MMM YYYY
                        _udtSystemMessage = _udtEHSClaimBLL.SearchEHSAccount(udtSchemeClaim.SchemeCode.Trim, _
                            udcStep1DocumentTypeRadioButtonGroup.SelectedValue.Trim, strIdentityNo, strECAge, dtmDateOfReg, udtEHSAccount, _
                            udtEligibleResult, udtSearchAccountStatus, udtVREEHSPersonalInfo, FunctionCode, Me.ClaimMode)
                    Else
                        ' DOB
                        _udtSystemMessage = _udtEHSClaimBLL.SearchEHSAccount(udtSchemeClaim.SchemeCode.Trim, _
                            udcStep1DocumentTypeRadioButtonGroup.SelectedValue.Trim, strIdentityNo, strDOB, udtEHSAccount, udtEligibleResult, _
                            udtSearchAccountStatus, udtVREEHSPersonalInfo, String.Empty, FunctionCode, Me.ClaimMode)

                    End If

                    If Not Me._udtSystemMessage Is Nothing Then
                        isValid = False

                        If Me._udtSystemMessage.MessageCode = "00142" Or Me._udtSystemMessage.MessageCode = "00141" Then
                            Me.udcClaimSearch.SetECHKIDError(True)
                        ElseIf Me._udtSystemMessage.MessageCode = "00110" Then
                            If Me.udcClaimSearch.ECDOBSelected Then
                                Me.udcClaimSearch.SetECDOBError(True)
                            Else
                                Me.udcClaimSearch.SetECDOAAgeError(True)
                                Me.udcClaimSearch.SetECDOAError(True)
                            End If
                        End If

                        Me.udcMsgBoxErr.AddMessage(_udtSystemMessage)
                    End If

                    ' INT18-XXX (Refine auditlog) [Start][Chris YIM]
                    ' --------------------------------------------------------------------------------------
                    'Log Enter Info Complete
                    EHSClaimBasePage.AuditLogSearchAccountInfoEnd(New AuditLogEntry(FunctionCode, Me), isValid)
                    ' INT18-XXX (Refine auditlog) [End][Chris YIM]
                End If

        End Select

        If isValid Then
            Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = Nothing
            Dim blnGoToCreation As Boolean = True
            Dim strRuleResultKey As String = String.Empty

            If udtEligibleResult Is Nothing Then
                Me._udtSessionHandler.EligibleResultRemoveFromSession()
            Else
                Dim udtRuleResults As RuleResultCollection = New RuleResultCollection()

                udtEligibleResult.PromptConfirmed = True
                'Key = 1_G0002 -> not need prompt confirm popup dox -> reminder in step2a
                strRuleResultKey = Me.RuleResultKey(ActiveViewIndex.Step1, udtEligibleResult.RuleType)

                udtRuleResults.Add(strRuleResultKey, udtEligibleResult)
                Me._udtSessionHandler.EligibleResultSaveToSession(udtRuleResults)
            End If


            Me._udtSessionHandler.NotMatchAccountExistSaveToSession(udtSearchAccountStatus.NotMatchAccountExist)
            Me._udtSessionHandler.ExceedDocTypeLimitSaveToSession(udtSearchAccountStatus.ExceedDocTypeLimit)
            udtEHSAccount.SetSearchDocCode(strSearchDocCode)

            Me._udtSessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctCode)
            _udtSessionHandler.SearchAccountStatusSaveToSession(udtSearchAccountStatus)

            If ClaimMode = Common.Component.ClaimMode.COVID19 Then
                _udtSessionHandler.ClaimCOVID19CarryForwordSaveToSession(False, FunctCode)
                _udtSessionHandler.ClaimCOVID19VaccineBrandRemoveFromSession(FunctCode)
                _udtSessionHandler.ClaimCOVID19VaccineLotNoRemoveFromSession(FunctCode)
            End If

            ' ===========================================================================================
            ' Redirect to page
            ' ===========================================================================================

            ' Account validated + Search DocCode = PersonalInfo DocCode -> Go to Claim
            If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                udtEHSAccountPersonalInfo = udtEHSAccount.EHSPersonalInformationList.Filter(strSearchDocCode)
                If Not udtEHSAccountPersonalInfo Is Nothing Then

                    ' To Handle Concurrent Browser:
                    Me.EHSClaimTokenNumAssign()
                    Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step2a
                    blnGoToCreation = False
                End If
            End If

            ' CRE20-0023 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            'Log
            Dim udtPracticeDisplay As PracticeDisplayModel = _udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)

            If ClaimMode = Common.Component.ClaimMode.COVID19 AndAlso udtSchemeClaim.SchemeCode = SchemeClaimModel.COVID19CVC Then
                EHSClaimBasePage.AddDescriptionCentreBooth(udtAuditLogEntry, udtPracticeDisplay.SPID, udtPracticeDisplay.PracticeID)
            End If

            EHSClaimBasePage.AuditLogSearchAccountComplete(udtAuditLogEntry, strSchemeCode, strSearchDocCode, _
                                                           _udtSessionHandler.HKICSymbolGetFormSession(FunctCode), udtEHSAccount.EHSPersonalInformationList(0), _
                                                           EHSClaimBasePage.ParseSearchAccountResult(udtEHSAccount, udtSearchAccountStatus))
            ' CRE20-0023 (Immu record) [End][Chris YIM]


            If blnGoToCreation = False AndAlso Not udtEHSAccount Is Nothing AndAlso udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                EHSClaimBasePage.AuditLogValidatedAccounhtFound(udtAuditLogEntry, udtEHSAccount)
            End If

            If blnGoToCreation Then
                Me._udtSessionHandler.AccountCreationComeFromClaimSaveToSession(True)

                '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

                RedirectHandler.ToURL("EHSAccountCreationV1.aspx")

                '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

            Else
                EHSClaimBasePage.AuditLogEnterClaimDetailLoaded(New AuditLogEntry(FunctionCode, Me))

                'Carry Forword: once times only
                If _udtSessionHandler.ClaimCOVID19CarryForwordGetFromSession(FunctCode) = False Then
                    _udtSessionHandler.ClaimCOVID19CarryForwordSaveToSession(True, FunctCode)
                End If

            End If

        Else
            Me.udcMsgBoxErr.BuildMessageBox(_strValidationFail, udtAuditLogEntry, Common.Component.LogID.LOG00005, "Search Account Failed", _
                New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, strSearchDocCode, (New Formatter).formatDocumentIdentityNumber(strSearchDocCode, SearchGetDocumentNo(strSearchDocCode))))
        End If

        'Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step2a
    End Sub

    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Private Sub SearchSmartID(ByVal udtSmartIDContent As SmartIDContentModel, ByVal isValid As Boolean, ByVal udtAuditlogEntry As AuditLogEntry)
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
        Dim udtPersonalInfoSmartID As EHSAccountModel.EHSPersonalInformationModel
        Dim udtEHSAccountExist As EHSAccountModel = Nothing
        Dim blnNotMatchAccountExist As Boolean = False
        Dim blnExceedDocTypeLimit As Boolean = False
        Dim udtEligibleResult As EligibleResult = Nothing
        Dim goToCreation As Boolean = True
        Dim strError As String = String.Empty

        udtPersonalInfoSmartID = udtSmartIDContent.EHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC)

        If isValid Then

            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            ' ----------------------------------------------
            ' 1. Search account in EHS 
            ' ----------------------------------------------
            Me._udtSystemMessage = Me._udtEHSClaimBLL.SearchEHSAccountSmartID(udtSchemeClaim.SchemeCode.Trim(), _
                                                                              DocTypeModel.DocTypeCode.HKIC, _
                                                                              udtPersonalInfoSmartID.IdentityNum, _
                                                                              Me._udtFormatter.formatDOB(udtPersonalInfoSmartID.DOB, udtPersonalInfoSmartID.ExactDOB, Common.Component.CultureLanguage.English, Nothing, Nothing),
                                                                              udtEHSAccountExist, _
                                                                              udtSmartIDContent.EHSAccount, _
                                                                              udtSmartIDContent.SmartIDReadStatus, _
                                                                              udtEligibleResult, _
                                                                              blnNotMatchAccountExist, _
                                                                              blnExceedDocTypeLimit, _
                                                                              FunctionCode, _
                                                                              True,
                                                                              Me.ClaimMode)

            ' ----------------------------------------------
            ' 2. Call OCSSS to check HKIC if input is shown
            ' ----------------------------------------------
            If Me._udtSystemMessage Is Nothing Then
                ' HKIC must be formated in 9 characters e.g. " A1234567" or "CD1234567"

                If Me._udtSessionHandler.UIDisplayHKICSymbolGetFormSession(FunctCode) Then

                    ' INT18-XXX (Refine auditlog) [Start][Chris YIM]
                    ' --------------------------------------------------------------------------------------
                    'Log Enter Info
                    EHSClaimBasePage.AuditLogSearchOCSSSStart(New AuditLogEntry(FunctionCode, Me), DocTypeModel.DocTypeCode.HKIC, _
                                                               _udtSessionHandler.HKICSymbolGetFormSession(FunctCode), udtPersonalInfoSmartID.IdentityNum)

                    CheckHKIDByOCSSS(udtPersonalInfoSmartID.IdentityNum, _udtSP.SPID, udtSchemeClaim.SchemeCode.Trim())

                    If Me._udtSystemMessage Is Nothing Then
                        EHSClaimBasePage.AuditLogSearchOCSSSEnd(New AuditLogEntry(FunctionCode, Me), True)
                    Else
                        EHSClaimBasePage.AuditLogSearchOCSSSEnd(New AuditLogEntry(FunctionCode, Me), False)
                    End If

                    ' INT18-XXX (Refine auditlog) [End][Chris YIM]
                Else
                    Me._udtSessionHandler.HKICSymbolRemoveFromSession(FunctCode)
                End If
            End If

            ' ----------------------------------------------
            ' Search Account Error Issue
            ' ----------------------------------------------
            If Not Me._udtSystemMessage Is Nothing Then
                isValid = False

                Select Case Me._udtSystemMessage.MessageCode
                    Case "00141", "00142"
                        Me.udcClaimSearch.SetHKICNoError(True)
                End Select

                'Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
            Else
                'Validation Success

                'Store residential status in model
                ' INT18-0018 (Fix read smart IC with HKBC account) [Start][Koala]
                If udtEHSAccountExist.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC) IsNot Nothing Then
                    udtEHSAccountExist.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).HKICSymbol = _udtSessionHandler.HKICSymbolGetFormSession(FunctCode)
                End If
                ' INT18-0018 (Fix read smart IC with HKBC account) [End][Koala]
            End If
            ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        End If

        If isValid Then
            Dim strRuleResultKey As String = String.Empty

            If udtEligibleResult Is Nothing Then
                Me._udtSessionHandler.EligibleResultRemoveFromSession()
            Else
                Dim udtRuleResults As RuleResultCollection = New RuleResultCollection()

                udtEligibleResult.PromptConfirmed = True
                'Key = 1_G0002 -> not need prompt confirm popup dox -> reminder in step2a
                strRuleResultKey = Me.RuleResultKey(ActiveViewIndex.Step1, udtEligibleResult.RuleType)

                udtRuleResults.Add(strRuleResultKey, udtEligibleResult)
                Me._udtSessionHandler.EligibleResultSaveToSession(udtRuleResults)
            End If

            udtEHSAccountExist.SetSearchDocCode(DocTypeModel.DocTypeCode.HKIC)

            'Only one case go to Claim directly -> Account validated && Search DocCode = PersonalInfo DocCode 
            'udtSmartIDContent.SmartIDReadStatus = BLL.SmartIDHandler.CheckSmartIDCardStatus(udtSmartIDContent.EHSAccount, udtEHSAccountExist)
            Me._udtSessionHandler.SmartIDContentSaveToSession(FunctCode, udtSmartIDContent)
            Me._udtSessionHandler.NotMatchAccountExistSaveToSession(blnNotMatchAccountExist)
            Me._udtSessionHandler.ExceedDocTypeLimitSaveToSession(blnExceedDocTypeLimit)
            Me._udtSessionHandler.EHSAccountSaveToSession(udtEHSAccountExist, FunctCode)

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
                            Me._udtSystemMessage = New Common.ComObject.SystemMessage("990001", Common.Component.SeverityCode.SEVD, eSQL.Message)
                            Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                        Else
                            Throw eSQL
                        End If
                    End Try

                    If Me._udtSystemMessage Is Nothing Then
                        'udtEHSAccountExist = Me._udtEHSAccountBll.LoadEHSAccountByVRID(udtPersonalInfoSmartID.VoucherAccID)
                        'udtEHSAccountExist.SetSearchDocCode(udtEHSAccountExist.EHSPersonalInformationList(0).DocCode)
                        udtEHSAccountExist = Me._udtEHSAccountBll.LoadEHSAccountByIdentity(udtEHSAccountExist.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).IdentityNum, DocTypeModel.DocTypeCode.HKIC)
                        udtEHSAccountExist.SetSearchDocCode(DocTypeModel.DocTypeCode.HKIC)
                        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                        ' ----------------------------------------------------------
                        udtEHSAccountExist.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).HKICSymbol = _udtSessionHandler.HKICSymbolGetFormSession(FunctCode)
                        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

                        Me._udtSessionHandler.EHSAccountSaveToSession(udtEHSAccountExist, FunctCode)
                    End If

            End Select
        Else
            goToCreation = False
        End If

        If goToCreation Then

            EHSClaimBasePage.AuditLogSearchNvaliatedACwithCFDComplete(udtAuditlogEntry, udtSchemeClaim.SchemeCode, udtSmartIDContent, True)
            Me._udtSessionHandler.AccountCreationComeFromClaimSaveToSession(True)

            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

            RedirectHandler.ToURL("EHSAccountCreationV1.aspx")

            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

        Else
            If Not Me._udtSystemMessage Is Nothing Then
                '---------------------------------------------------------------------------------------------------------------
                ' Block Case 
                '---------------------------------------------------------------------------------------------------------------
                If Not CheckFromVaccinationRecordEnquiry() Then
                    Me.Clear()
                    Me.udcStep1DocumentTypeRadioButtonGroup.SelectedValue = DocTypeModel.DocTypeCode.HKIC
                End If

                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1

                'Remove duplicate alert
                Dim dtCode As DataTable = Me.udcMsgBoxErr.GetCodeTable
                Dim drCode() As DataRow = dtCode.Select(String.Format("msgCode='{0}-{1}-{2}'", Me._udtSystemMessage.FunctionCode, Me._udtSystemMessage.SeverityCode, Me._udtSystemMessage.MessageCode))

                If drCode.Length = 0 Then
                    Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                End If

                isValid = False

            Else
                ' To Handle Concurrent Browser:
                EHSClaimBasePage.AuditLogSearchNvaliatedACwithCFDComplete(udtAuditlogEntry, udtSchemeClaim.SchemeCode, udtSmartIDContent, False)
                Me.EHSClaimTokenNumAssign()
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------                
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step2a
                EHSClaimBasePage.AuditLogEnterClaimDetailLoaded(New AuditLogEntry(FunctionCode, Me))
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
            End If

        End If
    End Sub
    ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Private Sub btnStep1ReadSmartIDTips_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnStep1ReadSmartIDTips.Click

        ' To Handle Concurrent Browser
        If Not Me.EHSClaimTokenNumValidation() Then
            Return
        End If

        Dim strReadSmartIDTipsUrl As String = Me.GetGlobalResourceObject("Url", "HCSPSmartIDCardUserGuideUrl")
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "ReadSmartIDTips", "javascript:openNewWin('" + ResolveClientUrl(strReadSmartIDTipsUrl) + "');", True)
    End Sub
    ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

    Private Sub btnStep1InputTips_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnStep1InputTips.Click

        ' To Handle Concurrent Browser
        If Not Me.EHSClaimTokenNumValidation() Then
            Return
        End If

        ScriptManager.RegisterStartupScript(Me, Page.GetType, "DocumentSmaple", String.Format("javascript:show{0}Help('{1}');", Me.udcStep1DocumentTypeRadioButtonGroup.SelectedValue.Trim.Replace("/", ""), Session("language")), True)
    End Sub

    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Private Sub udcClaimSearch_HKICSymbolHelpClick(ByVal sender As Object, ByVal e As EventArgs) Handles udcClaimSearch.HKICSymbolHelpClick

        ' To Handle Concurrent Browser
        If Not Me.EHSClaimTokenNumValidation() Then
            Return
        End If

        ScriptManager.RegisterStartupScript(Me, Page.GetType, "DocumentSmaple", String.Format("javascript:show{0}Help('{1}');", Me.udcStep1DocumentTypeRadioButtonGroup.SelectedValue.Trim.Replace("/", ""), Session("language")), True)
    End Sub
    ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

    ' Functions

    Private Function Step1SearchValdiation(ByVal strDocCode As String, ByRef strDocumentNo As String, ByRef strDOB As String, ByRef strDocumentNoPrefix As String) As Boolean
        Dim isValid As Boolean = True
        Dim formatter As Formatter = New Formatter()
        Dim validator As Common.Validation.Validator = New Common.Validation.Validator
        Dim udtDocTypeBLL As New DocTypeBLL ' CRE11-007
        Dim udtEHSClaimBLL As New EHSClaimBLL ' CRE11-007

        Me.udcClaimSearch.SetProperty(strDocCode)

        Me._udtSystemMessage = validator.chkIdentityNumber(strDocCode, SearchGetDocumentNo(strDocCode).ToUpper(), Me.udcClaimSearch.IdentityNoPrefix)

        If Not Me._udtSystemMessage Is Nothing Then
            isValid = False
            Select Case strDocCode.Trim()
                Case DocTypeModel.DocTypeCode.HKIC
                    Me.udcClaimSearch.SetHKICNoError(True)
                Case DocTypeModel.DocTypeCode.EC
                    Me.udcClaimSearch.SetECHKIDError(True)
                Case DocTypeModel.DocTypeCode.ADOPC
                    Me.udcClaimSearch.SetADOPCIdentityNoError(True)
                Case Else
                    Me.udcClaimSearch.SetSearchShortIdentityNoError(True)
            End Select

            Me.udcMsgBoxErr.AddMessage(_udtSystemMessage)
        Else
            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            strDocumentNo = SearchGetDocumentNo(strDocCode)
            ' CRE20-0022 (Immu record) [End][Chris YIM]
            strDocumentNoPrefix = Me.udcClaimSearch.IdentityNoPrefix
        End If

        ' Validate DOB
        Me._udtSystemMessage = validator.chkDOB(strDocCode, Me.udcClaimSearch.DOB)
        If Not Me._udtSystemMessage Is Nothing Then
            isValid = False
            Select Case strDocCode.Trim()
                Case DocTypeModel.DocTypeCode.HKIC
                    Me.udcClaimSearch.SetHKICDOBError(True)
                Case DocTypeModel.DocTypeCode.EC
                    Me.udcClaimSearch.SetECDOBError(True)
                Case DocTypeModel.DocTypeCode.ADOPC
                    Me.udcClaimSearch.SetADOPCDOBError(True)
                Case Else
                    Me.udcClaimSearch.SetSearchShortDOBError(True)
            End Select

            'Me.udcClaimSearch.SetSearchShortDOBError(True)
            Me.udcMsgBoxErr.AddMessage(_udtSystemMessage)
        Else
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim udtSubPlatformBLL As New SubPlatformBLL
            'strDOB = formatter.formatDate(Me.udcClaimSearch.DOB)
            strDOB = formatter.formatInputDate(Me.udcClaimSearch.DOB)
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            Me.udcClaimSearch.SetSearchShortDOB(strDOB)
        End If

        ' -------------------------------------------------------------------------------
        ' CRE11-007
        ' Check Active Death Record
        ' If dead, return "(document id name) is invalid"
        ' -------------------------------------------------------------------------------
        If isValid Then
            If udtDocTypeBLL.getDocTypeByAvailable(DocTypeBLL.EnumAvailable.DeathRecordAvailable).Filter(strDocCode) IsNot Nothing Then
                If (New eHealthAccountDeathRecord.eHealthAccountDeathRecordBLL).GetDeathRecordEntry(SearchGetDocumentNo(strDocCode).ToUpper()).IsDead() Then
                    isValid = False
                    Me._udtSystemMessage = validator.GetMessageForIdentityNoIsNoLongerValid(strDocCode)
                    Me.udcMsgBoxErr.AddMessage(_udtSystemMessage)
                End If
            End If
        End If

        Return isValid
    End Function

    Private Function SearchGetDocumentNo(ByVal strDocCode As String) As String
        Dim strDocumentNo As String = String.Empty

        Select Case strDocCode
            Case DocTypeModel.DocTypeCode.PASS, DocTypeModel.DocTypeCode.OW
                strDocumentNo = Me.udcClaimSearch.RawIdentityNo
            Case Else
                strDocumentNo = Me.udcClaimSearch.IdentityNo
        End Select

        Return strDocumentNo

    End Function


    Private Function Step1SearchECValdiation(ByRef strHKID As String, ByRef strDOB As String, ByRef strDOAge As String, ByRef dtmDateOfReg As DateTime) As Boolean
        Dim isValid As Boolean = True
        Dim formatter As Formatter = New Formatter()
        Dim validator As Common.Validation.Validator = New Common.Validation.Validator
        Dim strDateOfReg As String = String.Empty
        Dim udtDocTypeBLL As New DocTypeBLL ' CRE11-007

        Me.udcClaimSearch.SetProperty(DocTypeModel.DocTypeCode.EC)

        Me._udtSystemMessage = validator.chkHKID(Me.udcClaimSearch.IdentityNo.ToUpper())

        If Not Me._udtSystemMessage Is Nothing Then
            isValid = False
            Me.udcClaimSearch.SetECHKIDError(True)
            Me.udcMsgBoxErr.AddMessage(_udtSystemMessage)
        Else
            strHKID = Me.udcClaimSearch.IdentityNo
        End If

        ' Validate DOB
        If Me.udcClaimSearch.ECDOBSelected Then
            Me._udtSystemMessage = validator.chkDOB(DocTypeModel.DocTypeCode.EC, Me.udcClaimSearch.DOB)
            If Not Me._udtSystemMessage Is Nothing Then
                isValid = False
                Me.udcClaimSearch.SetECDOBError(True)
                Me.udcMsgBoxErr.AddMessage(_udtSystemMessage)
            Else
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'strDOB = formatter.formatDate(Me.udcClaimSearch.DOB)
                strDOB = formatter.formatInputDate(Me.udcClaimSearch.DOB)
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            End If
        Else
            Dim strDOADay As String = Me.udcClaimSearch.ECDOADay
            Dim strDOAMonth As String = Me.udcClaimSearch.ECDOAMonth
            Dim strDOAYear As String = Me.udcClaimSearch.ECDOAYear


            Me._udtSystemMessage = validator.chkECAge(Me.udcClaimSearch.ECAge)
            If Not Me._udtSystemMessage Is Nothing Then
                isValid = False
                Me.udcClaimSearch.SetECDOAAgeError(True)
                Me.udcMsgBoxErr.AddMessage(_udtSystemMessage)
            Else
                strDOAge = Me.udcClaimSearch.ECAge
            End If

            ' validate Date of Age
            Me._udtSystemMessage = validator.chkECDOAge(strDOADay, strDOAMonth, strDOAYear)
            If Not Me._udtSystemMessage Is Nothing Then
                isValid = False
                Me.udcClaimSearch.SetECDOAError(True)
                Me.udcMsgBoxErr.AddMessage(_udtSystemMessage)
            Else
                strDateOfReg = String.Format("{0:00}-{1}-{2}", Convert.ToInt32(strDOADay), strDOAMonth, strDOAYear)

                dtmDateOfReg = CDate(formatter.convertDate(strDateOfReg, Me._udtSessionHandler.Language))
            End If

            ' validate Age + Date of Age if Within Age
            If isValid Then
                Me._udtSystemMessage = validator.chkECAgeAndDOAge(Me.udcClaimSearch.ECAge, strDOADay, strDOAMonth, strDOAYear)
                If Not Me._udtSystemMessage Is Nothing Then
                    isValid = False
                    Me.udcClaimSearch.SetECDOAAgeError(True)
                    Me.udcClaimSearch.SetECDOAError(True)
                    Me.udcMsgBoxErr.AddMessage(_udtSystemMessage)
                Else
                    strDOB = (CDate(formatter.convertDate(strDateOfReg, "E")).Year - Convert.ToInt32(strDOAge)).ToString()
                End If
            End If
        End If

        ' -------------------------------------------------------------------------------
        ' CRE11-007
        ' Check Active Death Record
        ' If dead, return "(document id name) is invalid"
        ' -------------------------------------------------------------------------------
        If isValid Then
            If udtDocTypeBLL.getDocTypeByAvailable(DocTypeBLL.EnumAvailable.DeathRecordAvailable).Filter(DocType.DocTypeModel.DocTypeCode.EC) IsNot Nothing Then
                If (New eHealthAccountDeathRecord.eHealthAccountDeathRecordBLL).GetDeathRecordEntry(Me.udcClaimSearch.IdentityNo.ToUpper()).IsDead() Then
                    isValid = False
                    Me._udtSystemMessage = validator.GetMessageForIdentityNoIsNoLongerValid(DocType.DocTypeModel.DocTypeCode.EC)
                    Me.udcMsgBoxErr.AddMessage(_udtSystemMessage)
                End If
            End If
        End If

        Return isValid
    End Function

    Private Function IsDocumentAcceptedForScheme(ByVal strDocCode As String, ByVal strSchemeCode As String) As Boolean
        For Each udtSchemeDocType As SchemeDocTypeModel In (New DocTypeBLL).getSchemeDocTypeByScheme(strSchemeCode)
            If udtSchemeDocType.DocCode.Trim = strDocCode.Trim Then Return True
        Next

        Return False

    End Function

    ' Setup

    Private Sub SetupStep1(ByVal blnCreatePopupPractice As Boolean, ByVal blnSetScheme As Boolean)
        Dim blnFromVaccinationRecordEnquiry As Boolean = CheckFromVaccinationRecordEnquiry()

        ' Handle concurrent browser
        If Not EHSClaimTokenNumValidation() Then Return

        ' Get user account from session
        Dim udtDataEntry As DataEntryUserModel = Nothing
        GetCurrentUserAccount(_udtSP, udtDataEntry, False)

        ' Clear all sessions
        Dim udtEHSAccount As EHSAccountModel = Nothing
        Dim udtSmartIDContent As SmartIDContentModel = Nothing

        If blnFromVaccinationRecordEnquiry Then
            udtEHSAccount = _udtSessionHandler.EHSAccountGetFromSession(FunctCode)
            udtSmartIDContent = _udtSessionHandler.SmartIDContentGetFormSession(FunctCode)
        End If

        Clear()

        If blnFromVaccinationRecordEnquiry Then
            _udtSessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctCode)
            _udtSessionHandler.SmartIDContentSaveToSession(FunctCode, udtSmartIDContent)
        End If

        If IsNothing(udtSmartIDContent) Then udtSmartIDContent = New SmartIDContentModel()

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        _udtEHSTransactionBLL.ClearSessionTransactionDetailBenefit()
        _udtEHSTransactionBLL.ClearSessionTransactionDetailVaccine()
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        ' Get selected Practice
        Dim udtSelectedPracticeDisplay As PracticeDisplayModel = _udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)

        ' Set label: Selected Practice
        If _udtSessionHandler.Language = CultureLanguage.TradChinese OrElse _udtSessionHandler.Language = CultureLanguage.SimpChinese Then
            Me.lblStep1Practice.Text = String.Format("{0} ({1})", udtSelectedPracticeDisplay.PracticeNameChi, udtSelectedPracticeDisplay.PracticeID)
            Me.lblStep1Practice.CssClass = "tableTextChi"
        Else
            Me.lblStep1Practice.Text = String.Format("{0} ({1})", udtSelectedPracticeDisplay.PracticeName, udtSelectedPracticeDisplay.PracticeID)
            Me.lblStep1Practice.CssClass = "tableText"
        End If

        ' Show/Hide the change practice button (depends on No. of Practices)
        Me.ShowChangePracticeButton(Me.btnStep1ChangePractice)

        ' Set up practice selection popup box
        If blnCreatePopupPractice Then
            Dim udtPracticeDisplayList As PracticeDisplayModelCollection = _udtSessionHandler.PracticeDisplayListGetFromSession(FunctCode)

            udcPopupPracticeRadioButtonGroup.VerticalScrollBar = True
            ' CRE20-0XX (HA Scheme) [Start][Winnie]
            udcPopupPracticeRadioButtonGroup.SchemeSelection = IIf(Me.SubPlatform = EnumHCSPSubPlatform.CN, True, False)
            udcPopupPracticeRadioButtonGroup.SelectedScheme = Me._udtSessionHandler.SchemeSelectedForPracticeGetFromSession(FunctCode)
            ' CRE20-0XX (HA Scheme) [End][Winnie]
            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            udcPopupPracticeRadioButtonGroup.BuildRadioButtonGroup(udtPracticeDisplayList, _udtSP.PracticeList, _
                _udtSP.SchemeInfoList, _udtSessionHandler.Language, PracticeRadioButtonGroup.DisplayMode.BankAccount, Me.ClaimMode)
            ' CRE20-0022 (Immu record) [End][Chris YIM]
        End If

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ' Practice Scheme Info List Filter by COVID-19
        Dim udtFilterPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = _udtSchemeClaimBLL.FilterPracticeSchemeInfo(_udtSP.PracticeList, udtSelectedPracticeDisplay.PracticeID, Me.ClaimMode())

        ' --- Build Scheme Drop Down List ---
        ' Get valid schemes
        Dim udtSchemeClaimList As SchemeClaimModelCollection = _udtSchemeClaimBLL.searchValidClaimPeriodSchemeClaimByPracticeSchemeInfoSubsidizeCode( _
            udtFilterPracticeSchemeInfoList, _udtSP.SchemeInfoList)
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        udtSchemeClaimList = udtSchemeClaimList.FilterWithoutReadonly()
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        udtSchemeClaimList = udtSchemeClaimList.FilterByHCSPSubPlatform(Me.SubPlatform)

        Dim udtSelectedSchemeClaim As SchemeClaimModel = Nothing

        If udtSchemeClaimList.Count > 1 Then
            ' More then one scheme for selected practice

            ' Show the scheme dropdown list
            DropDownListBindScheme(ddlStep1Scheme, udtSchemeClaimList, _udtSessionHandler.Language, True)
            ddlStep1Scheme.Visible = True

            ' Hide the fixed scheme label
            lblStep1SchemeSelectedText.Visible = False

            ' Retrieve the previously selected scheme
            udtSelectedSchemeClaim = _udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

            If IsNothing(udtSelectedSchemeClaim) Then

                ' Unselect the Document Type
                udcStep1DocumentTypeRadioButtonGroup.SelectedValue = String.Empty

            Else
                ' Scheme is previously selected
                If blnSetScheme Then
                    udtSelectedSchemeClaim = udtSchemeClaimList.Filter(udtSelectedSchemeClaim.SchemeCode)

                    If IsNothing(udtSelectedSchemeClaim) Then
                        ddlStep1Scheme.SelectedValue = String.Empty
                        Step1Reset(False)

                    Else
                        ddlStep1Scheme.SelectedValue = udtSelectedSchemeClaim.SchemeCode

                        Dim udtConvertedSchemeCode As String = _udtSchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(udtSelectedSchemeClaim.SchemeCode)
                        ' CRE20-0022 (Immu record) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                        _udtSessionHandler.NonClinicSettingSaveToSession(udtFilterPracticeSchemeInfoList.Filter(udtConvertedSchemeCode).IsNonClinic, FunctCode)
                        ' CRE20-0022 (Immu record) [End][Chris YIM]

                    End If
                End If
            End If

            ' --- Build Document Type ---

            udcStep1DocumentTypeRadioButtonGroup.Scheme = ddlStep1Scheme.SelectedValue

            udcStep1DocumentTypeRadioButtonGroup.HCSPSubPlatform = Me.SubPlatform

            If Me.SubPlatform = EnumHCSPSubPlatform.CN Then
                udcStep1DocumentTypeRadioButtonGroup.ShowLegend = False
            End If

            udcStep1DocumentTypeRadioButtonGroup.Build(CustomControls.DocumentTypeRadioButtonGroup.FilterDocCode.Scheme)

        Else
            ' Only 1 scheme, auto assign
            udtSelectedSchemeClaim = udtSchemeClaimList(0)

            ' Hide the dropdown list
            ddlStep1Scheme.Visible = False

            ' Show the fixed label
            lblStep1SchemeSelectedText.Visible = True

            If _udtSessionHandler.Language = CultureLanguage.TradChinese Then
                lblStep1SchemeSelectedText.Text = udtSelectedSchemeClaim.SchemeDescChi
            ElseIf _udtSessionHandler.Language = CultureLanguage.SimpChinese Then
                lblStep1SchemeSelectedText.Text = udtSelectedSchemeClaim.SchemeDescCN
            Else
                lblStep1SchemeSelectedText.Text = udtSelectedSchemeClaim.SchemeDesc
            End If

            ' Save the selected scheme to session
            _udtSessionHandler.SchemeSelectedSaveToSession(udtSelectedSchemeClaim, FunctCode)

            Dim udtConvertedSchemeCode As String = _udtSchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(udtSelectedSchemeClaim.SchemeCode)

            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            _udtSessionHandler.NonClinicSettingSaveToSession(udtFilterPracticeSchemeInfoList.Filter(udtConvertedSchemeCode).IsNonClinic, FunctCode)
            ' CRE20-0022 (Immu record) [End][Chris YIM]

        End If

        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        panNonClinicSettingStep1.Visible = False

        If Not udtSelectedSchemeClaim Is Nothing Then
            If _udtSessionHandler.NonClinicSettingGetFromSession(FunctCode) Then
                panNonClinicSettingStep1.Visible = True

                If _udtSessionHandler.Language = CultureLanguage.TradChinese Then
                    lblNonClinicSettingStep1.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)))
                ElseIf _udtSessionHandler.Language = CultureLanguage.SimpChinese Then
                    lblNonClinicSettingStep1.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese)))
                Else
                    lblNonClinicSettingStep1.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.English)))
                End If
            Else
                panNonClinicSettingStep1.Visible = False
            End If
        End If
        'CRE16-002 (Revamp VSS) [End][Chris YIM]


        ' --- Build Document Type + Search (depends on From Vaccination Record Enquiry) ---

        If Not blnFromVaccinationRecordEnquiry Then
            ' Not from Vaccination Record Enquiry - Normal case

            ' --- Build Document Type ---

            If Not IsNothing(udtSelectedSchemeClaim) Then
                udcStep1DocumentTypeRadioButtonGroup.Scheme = udtSelectedSchemeClaim.SchemeCode
            Else
                udcStep1DocumentTypeRadioButtonGroup.Scheme = String.Empty
                udcStep1DocumentTypeRadioButtonGroup.SelectedValue = String.Empty
            End If

            udcStep1DocumentTypeRadioButtonGroup.HCSPSubPlatform = Me.SubPlatform

            If Me.SubPlatform = EnumHCSPSubPlatform.CN Then
                udcStep1DocumentTypeRadioButtonGroup.ShowLegend = False
            End If

            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            If udtSelectedSchemeClaim Is Nothing Then
                udcStep1DocumentTypeRadioButtonGroup.Build(CustomControls.DocumentTypeRadioButtonGroup.FilterDocCode.VaccinationRecordEnquriySearch)
            Else
                If udtSelectedSchemeClaim.SchemeCode = SchemeClaimModel.VSS OrElse udtSelectedSchemeClaim.SchemeCode = SchemeClaimModel.RVP Then
                    If ClaimMode = Common.Component.ClaimMode.COVID19 Then
                        udcStep1DocumentTypeRadioButtonGroup.Build(CustomControls.DocumentTypeRadioButtonGroup.FilterDocCode.VSS_COVID19)
                    Else
                        udcStep1DocumentTypeRadioButtonGroup.Build(CustomControls.DocumentTypeRadioButtonGroup.FilterDocCode.VSS)
                    End If
                Else
                    udcStep1DocumentTypeRadioButtonGroup.Build(CustomControls.DocumentTypeRadioButtonGroup.FilterDocCode.Scheme)
                End If
            End If
            ' CRE20-0022 (Immu record) [End][Chris YIM]

            ' --- Build Search ---

            Dim strDocCode As String = udcStep1DocumentTypeRadioButtonGroup.SelectedValue

            ' Search message

            ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Dim strIDEASComboClientInstalled As String = IIf(_udtSessionHandler.IDEASComboClientGetFormSession() Is Nothing, YesNo.No, _udtSessionHandler.IDEASComboClientGetFormSession())
            Dim blnIDEASComboClientInstalled As Boolean = IIf(strIDEASComboClientInstalled = YesNo.Yes, True, False)
            Dim blnIDEASComboClientForceToUse As Boolean = IIf(_udtGeneralFunction.getSystemParameter("SmartID_IDEAS_Combo_Force_To_Use") = YesNo.Yes, True, False)

            ' SmartID Tips button
            If SmartIDHandler.EnableSmartID AndAlso strDocCode = DocTypeModel.DocTypeCode.HKIC Then
                lblSearchECAcctInputSearch.Text = Me.GetGlobalResourceObject("Text", "InputHKICSearchAccount")

                If blnIDEASComboClientInstalled Or blnIDEASComboClientForceToUse Then
                    btnStep1ReadSmartIDTips.Visible = False
                Else
                    btnStep1ReadSmartIDTips.Visible = True
                End If

            Else
                lblSearchECAcctInputSearch.Text = Me.GetGlobalResourceObject("Text", "InputECSearchAccount")

                btnStep1ReadSmartIDTips.Visible = False

            End If

            ' Help button
            Dim blnHelpAvailable As Boolean = False

            If strDocCode <> String.Empty Then
                blnHelpAvailable = (New DocTypeBLL).getAllDocType().Filter(strDocCode).HelpAvailable() = "Y"
            End If

            If blnHelpAvailable Then
                btnStep1InputTips.Visible = True
                btnStep1InputTips.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "HelpBtn")
                btnStep1InputTips.AlternateText = Me.GetGlobalResourceObject("AlternateText", "HelpBtn")
            Else
                btnStep1InputTips.Visible = False
            End If

            ' Search
            udcClaimSearch.SchemeSelected = Not IsNothing(udtSelectedSchemeClaim)
            udcClaimSearch.ShowInputTips = Not blnHelpAvailable
            If ClaimMode = Common.Component.ClaimMode.COVID19 Then
                udcClaimSearch.UIEnableHKICSymbol = False
            Else
                udcClaimSearch.UIEnableHKICSymbol = True
            End If

            If udcClaimSearch.SchemeSelected Then
                udcClaimSearch.SchemeCode = udtSelectedSchemeClaim.SchemeCode
            Else
                udcClaimSearch.SchemeCode = String.Empty
            End If

            udcClaimSearch.ForceReadSmartIDCard = False

            If udtSelectedSchemeClaim IsNot Nothing Then
                If Me.ClaimMode = Common.Component.ClaimMode.COVID19 And udtSelectedSchemeClaim.SchemeCode = SchemeClaimModel.VSS Then
                    If _udtSessionHandler.ForceToReadSmartICGetFormSession Then
                        'udcClaimSearch.ForceReadSmartIDCard = True
                    End If
                End If
            End If

            udcClaimSearch.IDEASComboClientInstalled = blnIDEASComboClientInstalled
            udcClaimSearch.IDEASComboClientForceToUse = blnIDEASComboClientForceToUse

            udcClaimSearch.Build(strDocCode)
            ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

        Else
            ' From Vaccination Record Enquiry
            ' - Document Type is fixed
            ' - Document No. and DOB is fixed
            ' - Cancel button is shown

            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            'Dim udtEHSPersonalInfo As EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)

            Dim udtEHSPersonalInfo As New EHSPersonalInformationModel
            If Not udtSmartIDContent.EHSAccount Is Nothing Then
                udtEHSPersonalInfo = udtSmartIDContent.EHSAccount.EHSPersonalInformationList(0)
            Else
                udtEHSPersonalInfo = udtEHSAccount.EHSPersonalInformationList(0)
            End If
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

            ' --- Build Document Type ---

            ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
            udcStep1DocumentTypeRadioButtonGroup.HCSPSubPlatform = Me.SubPlatform

            If Me.SubPlatform = EnumHCSPSubPlatform.CN Then
                udcStep1DocumentTypeRadioButtonGroup.ShowLegend = False
            End If
            ' CRE13-019-02 Extend HCVS to China [End][Lawrence]
            udcStep1DocumentTypeRadioButtonGroup.BuildWithFixedDocument(udtEHSPersonalInfo.DocCode)

            ' --- Build Search ---

            ' Search message
            lblSearchECAcctInputSearch.Text = Me.GetGlobalResourceObject("Text", "InputECSearchAccount")

            ' Help button
            Dim blnHelpAvailable As Boolean = (New DocTypeBLL).getAllDocType().Filter(udtEHSPersonalInfo.DocCode).HelpAvailable() = "Y"

            If blnHelpAvailable Then
                btnStep1InputTips.Visible = True
                btnStep1InputTips.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "HelpBtn")
                btnStep1InputTips.AlternateText = Me.GetGlobalResourceObject("AlternateText", "HelpBtn")
            Else
                btnStep1InputTips.Visible = False
            End If

            ' Search
            udcClaimSearch.SchemeSelected = Not IsNothing(udtSelectedSchemeClaim)
            udcClaimSearch.ShowInputTips = Not blnHelpAvailable
            If ClaimMode = Common.Component.ClaimMode.COVID19 Then
                udcClaimSearch.UIEnableHKICSymbol = False
            Else
                udcClaimSearch.UIEnableHKICSymbol = True
            End If

            If udcClaimSearch.SchemeSelected Then
                udcClaimSearch.SchemeCode = udtSelectedSchemeClaim.SchemeCode
            Else
                udcClaimSearch.SchemeCode = String.Empty
            End If

            udcClaimSearch.Build(udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo)

        End If

        ' Setup the scheme logo
        SetupSchemeLogo()

    End Sub

    Private Sub Step1Reset(ByVal blnRetainDocType As Boolean)

        If Not blnRetainDocType Then
            Me.udcClaimSearch.CleanField()
        Else
            Me.udcClaimSearch.SetHKICError(False)
            Me.udcClaimSearch.SetECError(False)
            Me.udcClaimSearch.SetADOPCError(False)
            Me.udcClaimSearch.SetSearchShortError(False)
        End If

        Me.udcMsgBoxInfo.BuildMessageBox()
        Me.udcMsgBoxErr.BuildMessageBox()
    End Sub

#End Region

#Region "Step 2a Enter Claim detail Events"

    '-------------------------------------------------------------------------------------------------------------------
    'Events
    '-------------------------------------------------------------------------------------------------------------------
    Private Sub btnStep2aClaim_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnStep2aClaim.Click

        If Me._blnIsRequireHandlePageRefresh Then
            Return
        End If

        Me.Step2aClaimSubmit(False)
    End Sub

    Private Sub btnStep2aCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnStep2aCancel.Click
        If Me._blnIsRequireHandlePageRefresh Then
            Return
        End If

        Me.ModalPopupConfirmCancel.Show()
    End Sub

    Private Sub udcStep2aInputEHSClaim_ClaimControlEventFired(ByVal strSchemeName As String, ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcStep2aInputEHSClaim.ClaimControlEventFired
        If Me._blnIsRequireHandlePageRefresh Then
            Return
        End If

        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(FunctCode, Me)
        EHSClaimBasePage.AuditLogOpenRCHWindow(udtAuditLogEntry)

        Me._udtSessionHandler.EHSEnterClaimDetailSearchRCHSaveToSession(True)
        Me.udcRVPHomeListSearch.BindRVPHomeList(Nothing)
        Me.udcRVPHomeListSearch.ClearFilter()

        Me.btnPopupRVPHomeListSearchSelect.Enabled = False
        Me.btnPopupRVPHomeListSearchSelect.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectDisableBtn")

        Me.ModalPopupExtenderRVPHomeListSearch.Show()
    End Sub

    Private Sub udcStep2aInputEHSClaim_OutreachTypeChanged(ByVal strSchemeCode As String, ByVal sender As Object, ByVal e As System.EventArgs) Handles udcStep2aInputEHSClaim.OutreachTypeChange
        If Me._blnIsRequireHandlePageRefresh Then
            Return
        End If

        Select Case strSchemeCode
            Case SchemeClaimModel.RVP
                Dim udcInputRVPCOVID19 As ucInputRVPCOVID19 = udcStep2aInputEHSClaim.GetRVPCOVID19Control

                If udcInputRVPCOVID19 IsNot Nothing Then
                    Select Case udcInputRVPCOVID19.TypeOfOutreach
                        Case "RCH"
                            'trStep2aContactNo.Style.Add("display", "none")
                            'trStep2aDeclareJoineHRSS.Style.Add("display", "none")

                        Case "O"
                            trStep2aContactNo.Style.Remove("display")
                            trStep2aDeclareJoineHRSS.Style.Remove("display")

                        Case Else
                            trStep2aContactNo.Style.Add("display", "none")
                            trStep2aDeclareJoineHRSS.Style.Add("display", "none")

                    End Select

                End If
        End Select

    End Sub

    Private Sub udcStep2aInputEHSClaim_OutreachListSearchClicked(ByVal strSchemeName As String, ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcStep2aInputEHSClaim.OutreachListSearchClicked
        If Me._blnIsRequireHandlePageRefresh Then
            Return
        End If

        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(FunctCode, Me)
        EHSClaimBasePage.AuditLogShowOutreachPopup(udtAuditLogEntry)

        Me._udtSessionHandler.EHSEnterClaimDetailSearchOutreachSaveToSession(True)
        Me.udcOutreachSearch.BindOutreachList(Nothing)
        Me.udcOutreachSearch.ClearFilter()

        Me.btnPopupOutreachListSearchSelect.Enabled = False
        Me.btnPopupOutreachListSearchSelect.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectDisableBtn")

        Me.ModalPopupExtenderOutreachListSearch.Show()
    End Sub


    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub udcInputEHSClaim_SubsidizeDisabledRemarkClick(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me._blnIsRequireHandlePageRefresh Then
            Return
        End If

        Dim linkBtn As LinkButton = CType(sender, LinkButton)

        '1. Heading
        lblpanSubsidizeDisabledDetailsHeading.Text = HttpContext.GetGlobalResourceObject("Text", "NotEligibleDetail", New System.Globalization.CultureInfo(CType(Me._udtSessionHandler.Language, String)))

        ''2. Title
        'Dim strRemark() As String = Split(linkBtn.Attributes.Item("remark"), "|")
        'Dim udtSchemeClaim As SchemeClaimModel = Nothing
        'Dim udtSubsidizeGroupClaim As SubsidizeGroupClaimModel = Nothing
        'Dim strSchemeCode As String = String.Empty
        'Dim strSchemeSeq As String = String.Empty
        'Dim strSubsidizeCode As String = String.Empty

        ''Default wordings is "subsidy"
        'Dim strReplace = HttpContext.GetGlobalResourceObject("Text", "Vaccine", New System.Globalization.CultureInfo(CType(Me._udtSessionHandler.Language, String)))
        'If Me._udtSessionHandler.Language = Common.Component.CultureLanguage.English Then
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
        '        Select Case CType(_udtSessionHandler.Language, String)
        '            Case Common.Component.CultureLanguage.English
        '                strReplace = udtSubsidizeGroupClaim.DisplayCodeForClaim
        '            Case Common.Component.CultureLanguage.TradChinese
        '                strReplace = udtSubsidizeGroupClaim.LegendDescForClaimChi
        '            Case Common.Component.CultureLanguage.SimpChinese
        '                strReplace = udtSubsidizeGroupClaim.LegendDescForClaimCN
        '        End Select
        '    End If

        'End If

        'divSubsidizeDisabledDetailsTitle.InnerHtml = Replace(HttpContext.GetGlobalResourceObject("Text", "SubsidizeDisabledDetailsTitle", New System.Globalization.CultureInfo(CType(Me._udtSessionHandler.Language, String))), "%s", strReplace)

        '3. Content
        Dim strInnerHTML As String = String.Empty
        Dim strContent() As String = Split(HttpContext.GetGlobalResourceObject("Text", linkBtn.Attributes.Item("remark"), New System.Globalization.CultureInfo(CType(Me._udtSessionHandler.Language, String))), "|")
        If strContent.Length > 0 Then
            For i As Integer = 0 To strContent.Length - 1
                strInnerHTML = strInnerHTML + "<li style=""position:relative;left:10px;line-height:24px"">" + strContent(i) + "</li>"
            Next
        End If

        divSubsidizeDisabledDetailsContent.InnerHtml = strInnerHTML

        Me.ModalPopupExtenderSubsidizeDisabledRemark.Show()
    End Sub
    'CRE16-026 (Add PCV13) [End][Chris YIM]

    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub udcInputEHSClaim_RecipientConditionHelpClick(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        If Me._blnIsRequireHandlePageRefresh Then
            Return
        End If

        Dim ImgBtn As ImageButton = CType(sender, ImageButton)

        Dim strInnerHTML As String = String.Empty
        Dim strRemark() As String = Split(HttpContext.GetGlobalResourceObject("Text", "RecipientConditionPopupContent", New System.Globalization.CultureInfo(CType(Me._udtSessionHandler.Language, String))), "|")
        If strRemark.Length > 0 Then
            For i As Integer = 0 To strRemark.Length - 1
                strInnerHTML = strInnerHTML + "<li style=""position:relative;left:10px;line-height:24px"">" + strRemark(i) + "</li>"
            Next
        End If

        lblpanRecipientConditionHelpHeading.Text = HttpContext.GetGlobalResourceObject("Text", "RecipientConditionPopupHeading", New System.Globalization.CultureInfo(CType(Me._udtSessionHandler.Language, String)))
        divRecipientConditionHelpTitle.InnerHtml = HttpContext.GetGlobalResourceObject("Text", "RecipientConditionPopupTitle", New System.Globalization.CultureInfo(CType(Me._udtSessionHandler.Language, String)))
        divRecipientConditionHelpContent.InnerHtml = strInnerHTML

        Me.ModalPopupExtenderRecipientConditionHelp.Show()
    End Sub
    'CRE16-026 (Add PCV13) [End][Chris YIM]

    Private Sub udcInputEHSClaim_VaccineLegendClick(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        If Me._blnIsRequireHandlePageRefresh Then
            Return
        End If

        Me.udcSchemeLegend.ShowScheme = False
        Me.udcSchemeLegend.BindSchemeClaim(Me._udtSessionHandler.Language)
        Me.ModalPopupExtenderSchemeLegned.Show()
    End Sub

    Private Sub ddlStep2aScheme_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlStep2aScheme.SelectedIndexChanged
        If Me._blnIsRequireHandlePageRefresh Then
            Return
        End If

        ' Audit Log - Change Scheme
        Dim udtPreviousSelectedScheme As SchemeClaimModel = _udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
        Dim strPreviousSelectedScheme As String = String.Empty
        If Not udtPreviousSelectedScheme Is Nothing Then
            strPreviousSelectedScheme = udtPreviousSelectedScheme.SchemeCode.Trim()
        End If

        If strPreviousSelectedScheme <> ddlStep2aScheme.SelectedValue Then
            EHSClaimBasePage.AuditLogEnterClaimDetailChangeScheme(New AuditLogEntry(FunctionCode, Me), strPreviousSelectedScheme, ddlStep2aScheme.SelectedValue)
        End If

        Dim udtSchemeClaimBLL As SchemeClaimBLL = New SchemeClaimBLL()
        Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = Nothing
        Dim udtSchemeClaim As SchemeClaimModel = Nothing
        Dim udtPracticeDisplays As BLL.PracticeDisplayModelCollection = Nothing
        Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Dim udtSelectedPracticeDisplay As BLL.PracticeDisplayModel = Nothing

        'Initi Error image
        Me.imgStep2aServiceDateError.Visible = False

        udtSelectedPracticeDisplay = Me._udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)

        udtSchemeClaimModelCollection = Me._udtSessionHandler.SchemeSubsidizeListGetFromSession(FunctCode)

        'udtSchemeClaimModelCollection = udtSchemeClaimBLL.searchValidClaimPeriodSchemeClaimByPracticeSchemeInfoSubsidizeCode(Me._udtSP.PracticeList(udtSelectedPracticeDisplay.PracticeID).PracticeSchemeInfoList)

        'Init Carry Forward
        _udtSessionHandler.ClaimCOVID19CarryForwordSaveToSession(False, FunctCode)

        Me.udcMsgBoxErr.Clear()
        Me.udcMsgBoxInfo.Clear()

        'Save the selected scheme to session
        'udtSchemeClaim = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimBySchemeCode(Me.ddlStep2aScheme.SelectedValue.Trim())
        'udtSchemeClaim = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(Me.ddlStep2aScheme.SelectedValue.Trim())
        '
        udtSchemeClaim = udtSchemeClaimModelCollection.Filter(Me.ddlStep2aScheme.SelectedValue.Trim())
        Me._udtSessionHandler.SchemeSelectedSaveToSession(udtSchemeClaim, FunctCode)
        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode)
        Dim udtEHSTransactionBLL As New EHSTransactionBLL()
        Dim udtTranBenefitList As TransactionDetailModelCollection = Nothing

        ' Transaction Checking will be only apply on Vaccine scheme
        If udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVaccine Then
            ' Retrieve the Transaction Detail (may have mutil SubsidizeCodes)
            udtTranBenefitList = udtEHSTransactionBLL.getTransactionDetailBenefit(udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.IdentityNum)
        End If
        ' -------------------------------------------------------------------------------
        ' Re-Check Eligible
        ' -------------------------------------------------------------------------------
        Dim udtFormatter As New Formatter
        Dim udtSubPlatformBLL As New SubPlatformBLL()
        Dim _udtClaimRulesBLL As New ClaimRulesBLL()
        Dim udtEligibleResult As ClaimRulesBLL.EligibleResult
        Dim strRuleResultKey As String = String.Empty

        Dim udtSP As ServiceProviderModel = UserACBLL.GetServiceProvider()
        Dim udtSelectedPractice As PracticeDisplayModel = (New SessionHandler).PracticeDisplayGetFromSession(FunctionCode)
        Dim udtPractice As PracticeModel = Nothing
        If Not IsNothing(udtSelectedPractice) Then
            udtPractice = udtSP.PracticeList(udtSelectedPractice.PracticeID)
        End If
        Dim udtFilterSchemeClaim As SchemeClaimModel = New SchemeClaimModel(udtSchemeClaim)

        Dim strServiceDate As String = udtFormatter.formatInputDate(Me.txtStep2aServiceDate.Text, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
        Dim dtmServiceDate As DateTime = udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English)

        'Check Eligibility with all available subsidizes
        Dim udtRuleResultsForCheck As RuleResultCollection = New RuleResultCollection()

        For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtFilterSchemeClaim.SubsidizeGroupClaimList
            Dim udtSchemeClaimForCheck As SchemeClaimModel = New SchemeClaimModel(udtFilterSchemeClaim)
            Dim udtSubsidizeGroupClaimListForCheck As New SubsidizeGroupClaimModelCollection

            udtSubsidizeGroupClaimListForCheck.Add(udtSubsidizeGroupClaim)

            If Not udtSubsidizeGroupClaimListForCheck Is Nothing AndAlso udtSubsidizeGroupClaimListForCheck.Count > 0 Then
                udtSchemeClaimForCheck.SubsidizeGroupClaimList = udtSubsidizeGroupClaimListForCheck
            End If

            udtEligibleResult = _udtClaimRulesBLL.CheckEligibilityFromEHSClaimSearch(udtSchemeClaimForCheck, udtEHSPersonalInformation.IdentityNum, udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.DOB, udtEHSPersonalInformation.ExactDOB, dtmServiceDate, udtEHSPersonalInformation.Gender, udtTranBenefitList, udtPractice)

            If Not udtEligibleResult Is Nothing Then
                udtEligibleResult.PromptConfirmed = True
                'Key = 1_G0002
                strRuleResultKey = Me.RuleResultKey(udtSubsidizeGroupClaim.SubsidizeCode, udtEligibleResult.RuleType)

                udtRuleResultsForCheck.Add(strRuleResultKey, udtEligibleResult)
            End If
        Next

        If Not udtRuleResultsForCheck Is Nothing AndAlso udtRuleResultsForCheck.Count > 0 Then
            Me._udtSessionHandler.EligibleResultVSSReminderSaveToSession(udtRuleResultsForCheck)
        End If
        ' -------------------------------------------------------------------------------

        Dim udtConvertedSchemeCode As String = udtSchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(udtSchemeClaim.SchemeCode)
        Me._udtSessionHandler.NonClinicSettingSaveToSession(_udtSP.PracticeList(udtSelectedPracticeDisplay.PracticeID).PracticeSchemeInfoList.Filter(udtConvertedSchemeCode).IsNonClinic, FunctCode)

        If _udtSessionHandler.NonClinicSettingGetFromSession(FunctCode) Then
            panNonClinicSettingStep2a.Visible = True

            If _udtSessionHandler.Language = CultureLanguage.TradChinese Then
                lblNonClinicSettingStep2a.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)))
            ElseIf _udtSessionHandler.Language = CultureLanguage.SimpChinese Then
                lblNonClinicSettingStep2a.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese)))
            Else
                lblNonClinicSettingStep2a.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.English)))
            End If
        Else
            panNonClinicSettingStep2a.Visible = False
        End If

        _udtSessionHandler.ChangeSchemeInPracticeSaveToSession(True, FunctCode)
        'CRE16-002 (Revamp VSS) [End][Chris YIM]
        Me._udtSessionHandler.EHSClaimVaccineRemoveFromSession()
        Me._udtSessionHandler.EHSTransactionRemoveFromSession(FunctCode)
        Me._udtSessionHandler.ClaimCategoryRemoveFromSession(FunctCode)

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        If Me.ClaimMode = ClaimMode.COVID19 Then
            Me._udtSessionHandler.ClaimCOVID19CategoryRemoveFromSession(FunctCode)
            'Me._udtSessionHandler.ClaimCOVID19DoseRemoveFromSession(FunctionCode)

            Me.chkStep2aDeclareClaim.Checked = False

            If Not udtSchemeClaim Is Nothing Then
                Select Case udtSchemeClaim.ControlType
                    Case SchemeClaimModel.EnumControlType.RVP
                        Dim udcInputRVPCOVID19 As ucInputRVPCOVID19 = Me.udcStep2aInputEHSClaim.GetRVPCOVID19Control()
                        If Not udcInputRVPCOVID19 Is Nothing Then
                            If udcInputRVPCOVID19.VaccineBrandDDL.Items.Count <= 1 Then
                                udcInputRVPCOVID19.ClearVaccineAndLotNo()
                            End If
                        End If

                    Case SchemeClaimModel.EnumControlType.VSS
                        Dim udcInputVSSCOVID19 As ucInputVSSCOVID19 = Me.udcStep2aInputEHSClaim.GetVSSCOVID19Control()
                        If Not udcInputVSSCOVID19 Is Nothing Then
                            If udcInputVSSCOVID19.VaccineBrandDDL.Items.Count <= 1 Then
                                udcInputVSSCOVID19.ClearVaccineAndLotNo()
                            End If
                        End If

                    Case SchemeClaimModel.EnumControlType.COVID19
                        Dim udcInputCOVID19 As ucInputCOVID19 = Me.udcStep2aInputEHSClaim.GetCOVID19Control()
                        If Not udcInputCOVID19 Is Nothing Then
                            If udcInputCOVID19.VaccineBrandDDL.Items.Count <= 1 Then
                                udcInputCOVID19.ClearVaccineAndLotNo()
                            End If
                        End If

                    Case SchemeClaimModel.EnumControlType.COVID19RVP
                        Dim udcInputCOVID19RVP As ucInputCOVID19RVP = Me.udcStep2aInputEHSClaim.GetCOVID19RVPControl()
                        If Not udcInputCOVID19RVP Is Nothing Then
                            If udcInputCOVID19RVP.VaccineBrandDDL.Items.Count <= 1 Then
                                udcInputCOVID19RVP.ClearVaccineAndLotNo()
                            End If
                        End If

                    Case SchemeClaimModel.EnumControlType.COVID19OR
                        Dim udcInputCOVID19OR As ucInputCOVID19OR = Me.udcStep2aInputEHSClaim.GetCOVID19ORControl()
                        If Not udcInputCOVID19OR Is Nothing Then
                            If udcInputCOVID19OR.VaccineBrandDDL.Items.Count <= 1 Then
                                udcInputCOVID19OR.ClearVaccineAndLotNo()
                            End If
                        End If

                End Select
            End If
        End If
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        Me.SetupStep2aClaimContent(udtSchemeClaim, udtEHSAccount)

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        If Me.ClaimMode = ClaimMode.COVID19 Then
            Dim udtCOVID19BLL As New COVID19.COVID19BLL

            'Clear selected vaccine & lot no.
            If Not udtSchemeClaim Is Nothing Then
                Select Case udtSchemeClaim.ControlType
                    Case SchemeClaimModel.EnumControlType.RVP
                        If ClaimMode = Common.Component.ClaimMode.COVID19 Then
                            Dim udcInputRVPCOVID19 As ucInputRVPCOVID19 = Me.udcStep2aInputEHSClaim.GetRVPCOVID19Control()
                            If Not udcInputRVPCOVID19 Is Nothing Then
                                If udcInputRVPCOVID19.VaccineBrandDDL.Items.Count > 1 Then
                                    udcInputRVPCOVID19.ClearVaccineAndLotNo()
                                End If
                                _udtSessionHandler.ClaimCOVID19VaccineBrandRemoveFromSession(FunctionCode)
                                _udtSessionHandler.ClaimCOVID19VaccineLotNoRemoveFromSession(FunctionCode)

                            End If
                        End If

                    Case SchemeClaimModel.EnumControlType.VSS
                        If ClaimMode = Common.Component.ClaimMode.COVID19 Then
                            Dim udcInputVSSCOVID19 As ucInputVSSCOVID19 = Me.udcStep2aInputEHSClaim.GetVSSCOVID19Control()
                            If Not udcInputVSSCOVID19 Is Nothing Then
                                If udcInputVSSCOVID19.VaccineBrandDDL.Items.Count > 1 Then
                                    udcInputVSSCOVID19.ClearVaccineAndLotNo()
                                End If
                                _udtSessionHandler.ClaimCOVID19VaccineBrandRemoveFromSession(FunctionCode)
                                _udtSessionHandler.ClaimCOVID19VaccineLotNoRemoveFromSession(FunctionCode)

                            End If
                        End If

                    Case SchemeClaimModel.EnumControlType.COVID19
                        Dim udcInputCOVID19 As ucInputCOVID19 = Me.udcStep2aInputEHSClaim.GetCOVID19Control()
                        If Not udcInputCOVID19 Is Nothing Then
                            If udcInputCOVID19.VaccineBrandDDL.Items.Count > 1 Then
                                udcInputCOVID19.ClearVaccineAndLotNo()
                            End If
                            _udtSessionHandler.ClaimCOVID19VaccineBrandRemoveFromSession(FunctionCode)
                            _udtSessionHandler.ClaimCOVID19VaccineLotNoRemoveFromSession(FunctionCode)

                        End If

                    Case SchemeClaimModel.EnumControlType.COVID19RVP
                        Dim udcInputCOVID19RVP As ucInputCOVID19RVP = Me.udcStep2aInputEHSClaim.GetCOVID19RVPControl()
                        If Not udcInputCOVID19RVP Is Nothing Then
                            If udcInputCOVID19RVP.VaccineBrandDDL.Items.Count > 1 Then
                                udcInputCOVID19RVP.ClearVaccineAndLotNo()
                            End If
                            _udtSessionHandler.ClaimCOVID19VaccineBrandRemoveFromSession(FunctionCode)
                            _udtSessionHandler.ClaimCOVID19VaccineLotNoRemoveFromSession(FunctionCode)

                        End If

                    Case SchemeClaimModel.EnumControlType.COVID19OR
                        Dim udcInputCOVID19OR As ucInputCOVID19OR = Me.udcStep2aInputEHSClaim.GetCOVID19ORControl()
                        If Not udcInputCOVID19OR Is Nothing Then
                            If udcInputCOVID19OR.VaccineBrandDDL.Items.Count > 1 Then
                                udcInputCOVID19OR.ClearVaccineAndLotNo()
                            End If
                            _udtSessionHandler.ClaimCOVID19VaccineBrandRemoveFromSession(FunctionCode)
                            _udtSessionHandler.ClaimCOVID19VaccineLotNoRemoveFromSession(FunctionCode)

                        End If

                End Select

            End If

            'Re-assign mapping: lot no., category
            Dim strVaccineLotNoMappingJavaScript As String = String.Empty

            Select Case udtSchemeClaim.SchemeCode.Trim.ToUpper
                Case SchemeClaimModel.COVID19CVC, SchemeClaimModel.COVID19DH, SchemeClaimModel.COVID19OR, SchemeClaimModel.COVID19SR
                    strVaccineLotNoMappingJavaScript = Me.GetVaccineLotNoMappingForCentreJavaScript(udtSelectedPracticeDisplay.SPID, udtSelectedPracticeDisplay.PracticeID, dtmServiceDate)

                    If udtSchemeClaim.SchemeCode.Trim.ToUpper = SchemeClaimModel.COVID19OR Then
                        'Convert sub-category -> Dictionary -> Json
                        Dim strSubCategoryMappingJavaScript As String = String.Empty
                        Dim strCategoryJson As String = String.Empty
                        Dim strCategoryTextEngJson As String = String.Empty
                        Dim strCategoryTextChiJson As String = String.Empty

                        udtCOVID19BLL.GenerateCategoryJson(strCategoryJson, strCategoryTextEngJson, strCategoryTextChiJson)

                        'Generate JavaScript
                        strSubCategoryMappingJavaScript = udtCOVID19BLL.GenerateCategoryMappingJavaScript(strCategoryJson, strCategoryTextEngJson, strCategoryTextChiJson)
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "COVID19_SubCategory_Mapping", strSubCategoryMappingJavaScript, True)

                    End If

                    'Carry Forword: once times only
                    If _udtSessionHandler.ClaimCOVID19CarryForwordGetFromSession(FunctCode) = False Then
                        _udtSessionHandler.ClaimCOVID19CarryForwordSaveToSession(True, FunctCode)
                    End If

                Case SchemeClaimModel.VSS
                    strVaccineLotNoMappingJavaScript = Me.GetVaccineLotNoMappingForPrivateJavaScript(dtmServiceDate)

                    'Convert sub-category -> Dictionary -> Json
                    Dim strSubCategoryMappingJavaScript As String = String.Empty
                    Dim strCategoryJson As String = String.Empty
                    Dim strCategoryTextEngJson As String = String.Empty
                    Dim strCategoryTextChiJson As String = String.Empty

                    udtCOVID19BLL.GenerateCategoryJson(strCategoryJson, strCategoryTextEngJson, strCategoryTextChiJson)

                    'Generate JavaScript
                    strSubCategoryMappingJavaScript = udtCOVID19BLL.GenerateCategoryMappingJavaScript(strCategoryJson, strCategoryTextEngJson, strCategoryTextChiJson)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "COVID19_SubCategory_Mapping", strSubCategoryMappingJavaScript, True)

                    'Carry Forword: once times only
                    If _udtSessionHandler.ClaimCOVID19CarryForwordGetFromSession(FunctCode) = False Then
                        _udtSessionHandler.ClaimCOVID19CarryForwordSaveToSession(True, FunctCode)
                    End If

                Case SchemeClaimModel.RVP
                    strVaccineLotNoMappingJavaScript = Me.GetVaccineLotNoMappingForRCHJavaScript(dtmServiceDate)

                    'Convert sub-category -> Dictionary -> Json
                    Dim strSubCategoryMappingJavaScript As String = String.Empty
                    Dim strCategoryJson As String = String.Empty
                    Dim strCategoryTextEngJson As String = String.Empty
                    Dim strCategoryTextChiJson As String = String.Empty

                    udtCOVID19BLL.GenerateCategoryJson(strCategoryJson, strCategoryTextEngJson, strCategoryTextChiJson)

                    'Generate JavaScript
                    strSubCategoryMappingJavaScript = udtCOVID19BLL.GenerateCategoryMappingJavaScript(strCategoryJson, strCategoryTextEngJson, strCategoryTextChiJson)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "COVID19_SubCategory_Mapping", strSubCategoryMappingJavaScript, True)

                    'Carry Forword: once times only
                    If _udtSessionHandler.ClaimCOVID19CarryForwordGetFromSession(FunctCode) = False Then
                        _udtSessionHandler.ClaimCOVID19CarryForwordSaveToSession(True, FunctCode)
                    End If

            End Select

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "COVID19_Vaccine_LotNo_Mapping", strVaccineLotNoMappingJavaScript, True)

        End If

        Me.SetupServiceDate(udtSchemeClaim.SchemeCode)
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        Me.Step2aCleanSchemeErrorImage()

        Me.SetupSchemeLogo()

        Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail)

    End Sub

    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub udcStep2aInputEHSClaim_CategorySelected(ByVal sender As Object, ByVal e As System.EventArgs) Handles udcStep2aInputEHSClaim.CategorySelected
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
        Dim udtFormatter As Formatter = New Formatter
        Dim strCategory As String = String.Empty

        ' Reset Error Message when Category changed
        Me.udcMsgBoxErr.Clear()
        Me.udcMsgBoxInfo.Clear()

        Select Case udtSchemeClaim.ControlType
            Case SchemeClaimModel.EnumControlType.HSIVSS
                Dim udcInputHSIVSS As ucInputHSIVSS = Me.udcStep2aInputEHSClaim.GetHSIVSSControl()
                strCategory = udcInputHSIVSS.Category
            Case SchemeClaimModel.EnumControlType.RVP
                If ClaimMode = Common.Component.ClaimMode.COVID19 Then
                    Dim udcInputRVPCOVID19 As ucInputRVPCOVID19 = Me.udcStep2aInputEHSClaim.GetRVPCOVID19Control()
                    strCategory = udcInputRVPCOVID19.CategoryForCOVID19
                Else
                    Dim udcInputRVP As ucInputRVP = Me.udcStep2aInputEHSClaim.GetRVPControl()
                    strCategory = udcInputRVP.Category
                End If

            Case SchemeClaimModel.EnumControlType.VSS
                If Me.ClaimMode = Common.Component.ClaimMode.COVID19 Then
                    Dim udcInputVSSCOVID19 As ucInputVSSCOVID19 = Me.udcStep2aInputEHSClaim.GetVSSCOVID19Control()
                    strCategory = udcInputVSSCOVID19.CategoryForCOVID19
                Else
                    Dim udcInputVSS As ucInputVSS = Me.udcStep2aInputEHSClaim.GetVSSControl()
                    strCategory = udcInputVSS.Category

                End If

                ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
                ' --------------------------------------------------------------------------------------
            Case SchemeClaimModel.EnumControlType.ENHVSSO
                Dim udcInputENHVSSO As ucInputENHVSSO = Me.udcStep2aInputEHSClaim.GetENHVSSOControl()
                strCategory = udcInputENHVSSO.Category

                'Case SchemeClaimModel.EnumControlType.PPP
                '    Dim udcInputPPP As ucInputPPP = Me.udcStep2aInputEHSClaim.GetPPPControl()
                '    strCategory = udcInputPPP.Category
                ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

                ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
                ' --------------------------------------------------------------------------------------
            Case SchemeClaimModel.EnumControlType.COVID19
                Dim udcInputCOVID19 As ucInputCOVID19 = Me.udcStep2aInputEHSClaim.GetCOVID19Control()
                strCategory = udcInputCOVID19.CategoryForCOVID19
                ' CRE20-0022 (Immu record) [End][Winnie SUEN]

            Case SchemeClaimModel.EnumControlType.COVID19RVP
                Dim udcInputCOVID19RVP As ucInputCOVID19RVP = Me.udcStep2aInputEHSClaim.GetCOVID19RVPControl()
                strCategory = udcInputCOVID19RVP.CategoryForCOVID19

            Case SchemeClaimModel.EnumControlType.COVID19OR
                Dim udcInputCOVID19OR As ucInputCOVID19OR = Me.udcStep2aInputEHSClaim.GetCOVID19ORControl()
                strCategory = udcInputCOVID19OR.CategoryForCOVID19
        End Select


        If String.IsNullOrEmpty(strCategory) Then
            Me.SetConfirmButtonEnable(Me.btnStep2aClaim, False)
            Me._udtSessionHandler.ClaimCategoryRemoveFromSession(FunctCode)
        Else

            Dim udtSubPlatformBLL As New SubPlatformBLL
            Dim strServiceDate As String = udtFormatter.formatInputDate(Me.txtStep2aServiceDate.Text, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
            Dim dtmServiceDate As DateTime = udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English)
            Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
            Dim udtPersonalInformation As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode)
            Dim udtClaimCategorys As ClaimCategoryModelCollection

            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            udtClaimCategorys = Me._udtClaimCategoryBLL.getDistinctCategoryByScheme(udtSchemeClaim, udtPersonalInformation, dtmServiceDate)
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            'Update Session Variable
            Me._udtSessionHandler.ClaimCategorySaveToSession(udtClaimCategorys.FilterByCategoryCode(udtSchemeClaim.SchemeCode, strCategory), FunctCode)
            Me._udtSessionHandler.EHSClaimVaccineRemoveFromSession()

            Me.udcStep2aInputEHSClaim.ResetSchemeType()

            Me.SetupStep2aClaimContent(udtSchemeClaim, udtEHSAccount)

        End If

        Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail)
    End Sub
    'CRE16-026 (Add PCV13) [End][Chris YIM]

    Protected Sub txtStep2aServiceDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
        Dim udtEHSAccount As EHSAccountModel
        Dim udtClaimCategory As ClaimCategoryModel

        Dim udtFormatter As Formatter = New Formatter
        Dim udtValidator As Validator = New Validator
        Dim udtSubPlatformBLL As New SubPlatformBLL
        Dim strServiceDate As String = udtFormatter.formatInputDate(Me.txtStep2aServiceDate.Text, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))

        Dim strClaimDayLimit As String = String.Empty
        Dim strMinDate As String = String.Empty
        Dim strAllowDateBack As String = String.Empty
        Dim intDayLimit As Integer
        Dim dtmMinDate As DateTime
        Dim isValid As Boolean = True
        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(FunctCode, Me)

        EHSClaimBasePage.AuditLogChangeServiceDateStart(udtAuditLogEntry, Me.txtStep2aServiceDate.Text)

        'Initi Error image
        Me.imgStep2aServiceDateError.Visible = False
        ' Build Message box clear previous validation result, so clear the error icon as well
        Step2aCleanSchemeErrorImage()
        Me.udcMsgBoxErr.Clear()

        'Check Service Date Format
        Me._udtSystemMessage = udtValidator.chkServiceDate(strServiceDate)
        If Not Me._udtSystemMessage Is Nothing Then
            isValid = False

            Me.txtStep2aServiceDate.Text = udtFormatter.formatInputTextDate(Me.txtStep2aServiceDate.Attributes(ValidatedServiceDate), udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
            Me.imgStep2aServiceDateError.Visible = True
            Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
        End If

        If isValid Then
            'udtSchemeClaim

            'Check Service Date Back
            Me._udtGeneralFunction.getSystemParameter("DateBackClaimAllow", strAllowDateBack, String.Empty, udtSchemeClaim.SchemeCode)
            If strAllowDateBack = "Y" Then

                Me._udtGeneralFunction.getSystemParameter("DateBackClaimDayLimit", strClaimDayLimit, String.Empty, udtSchemeClaim.SchemeCode)
                Me._udtGeneralFunction.getSystemParameter("DateBackClaimMinDate", strMinDate, String.Empty, udtSchemeClaim.SchemeCode)

                intDayLimit = CInt(strClaimDayLimit)
                dtmMinDate = Convert.ToDateTime(strMinDate)

                Me._udtSystemMessage = udtValidator.chkDateBackClaimServiceDate(strServiceDate, intDayLimit, dtmMinDate)
                If Not Me._udtSystemMessage Is Nothing Then
                    isValid = False
                    Me.imgStep2aServiceDateError.Visible = True
                    Me.txtStep2aServiceDate.Text = udtFormatter.formatInputTextDate(Me.txtStep2aServiceDate.Attributes(ValidatedServiceDate), udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))

                    If Me._udtSystemMessage.MessageCode = "00149" Then
                        Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage, "%s", strClaimDayLimit)
                    ElseIf Me._udtSystemMessage.MessageCode = "00150" Then
                        Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage, "%s", udtFormatter.formatDisplayDate(dtmMinDate, Me._udtSessionHandler.Language))
                    Else
                        Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                    End If

                End If

            End If
        End If

        If isValid Then
            Dim strPreviousValdatedServiceDate As String = Me.txtStep2aServiceDate.Attributes(ValidatedServiceDate)
            Me.txtStep2aServiceDate.Attributes(ValidatedServiceDate) = strServiceDate
            Me.txtStep2aServiceDate.Text = udtFormatter.formatInputTextDate(strServiceDate, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))

            udtEHSAccount = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)

            ' Clear AvailableVoucher to trigger recalculation
            udtEHSAccount.VoucherInfo = Nothing

            If udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.VOUCHER) OrElse udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.VOUCHERCHINA) Then
                ' Voucher Scheme, No change
                Me.SetupStep2aClaimContent(Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode), udtEHSAccount)


            ElseIf udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.EHAPP) Then
                ' EHAPP, No change
                Me.SetupStep2aClaimContent(Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode), udtEHSAccount)

            Else
                ' Vaccine Scheme, re-render vaccine control
                Dim udtEHSClaimVaccine As EHSClaimVaccineModel
                Dim dtmServiceDate As DateTime = udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English)
                Dim udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode)
                Dim udtSP As ServiceProviderModel = UserACBLL.GetServiceProvider()
                Dim udtSelectedPractice As PracticeDisplayModel = (New SessionHandler).PracticeDisplayGetFromSession(FunctionCode)
                Dim udtInputPicker As InputPickerModel = New InputPickerModel

                ' Load the new SubsidizeGroupClaim list after service date changed
                Dim udtCurrentSchemeClaimList As SchemeClaimModelCollection = Me._udtSessionHandler.SchemeSubsidizeListGetFromSession(FunctCode)
                Dim udtUpdatedSchemeClaimList As SchemeClaimModelCollection = Nothing

                ' CRE20-0022 (Immu record) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                ' Practice Scheme Info List Filter by COVID-19
                Dim udtFilterPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = _udtSchemeClaimBLL.FilterPracticeSchemeInfo(_udtSP.PracticeList, udtSelectedPractice.PracticeID, Me.ClaimMode())

                ' Get all available Scheme with SubsidizeGroupClaim list (Filter by Provided Service in BO & Service date) 
                udtUpdatedSchemeClaimList = _udtSchemeClaimBLL.searchValidClaimPeriodSchemeClaimByPracticeSchemeInfoSubsidizeCode(udtFilterPracticeSchemeInfoList, _
                                                                                                                                  Me._udtSP.SchemeInfoList, _
                                                                                                                                  dtmServiceDate)
                ' CRE20-0022 (Immu record) [End][Chris YIM]

                ' Get all available SubsidizeGroupClaim List (Filter by Eligibility Rule)
                udtUpdatedSchemeClaimList = _udtSchemeClaimBLL.searchEligibleAndExceedDocumentClaimScheme(udtEHSAccount, udtEHSAccount.SearchDocCode, udtUpdatedSchemeClaimList)

                ' Re-new the SubsidizeGroupClaim list
                For Each udtCurrentSchemeClaim As SchemeClaimModel In udtCurrentSchemeClaimList
                    Dim udtUpdatedSchemeClaim As SchemeClaimModel = udtUpdatedSchemeClaimList.Filter(udtCurrentSchemeClaim.SchemeCode)

                    If Not udtUpdatedSchemeClaim Is Nothing Then
                        udtCurrentSchemeClaim.SubsidizeGroupClaimList = udtUpdatedSchemeClaim.SubsidizeGroupClaimList
                    End If

                Next

                Dim udtNewSchemeClaim As SchemeClaimModel = udtCurrentSchemeClaimList.Filter(udtSchemeClaim.SchemeCode)

                ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
                ' --------------------------------------------------------------------------------------
                If udtNewSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.RVP) OrElse _
                    udtNewSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.HSIVSS) OrElse _
                    udtNewSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.VSS) OrElse _
                    udtNewSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.ENHVSSO) OrElse _
                    udtNewSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.PPP) OrElse _
                    udtNewSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.COVID19) OrElse _
                    udtNewSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.COVID19CBD) OrElse _
                    udtNewSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.COVID19RVP) OrElse _
                    udtNewSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.COVID19OR) _
                Then ' CRE20-0022 (Immu record) [End][Winnie SUEN]

                    Dim udtEHSTransactionBLL As New EHSTransactionBLL()
                    Dim udtTranBenefitList As TransactionDetailModelCollection = Nothing

                    ' Transaction Checking will be only apply on Vaccine scheme
                    If udtNewSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVaccine Then
                        ' Retrieve the Transaction Detail (may have mutil SubsidizeCodes)
                        udtTranBenefitList = udtEHSTransactionBLL.getTransactionDetailBenefit(udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.IdentityNum)
                    End If

                    ' Re-generate the category list by new SubsidizeGroupClaim list (e.g VSS & RVP)
                    Dim udtClaimCategorys As ClaimCategoryModelCollection = Me._udtClaimCategoryBLL.getDistinctCategoryByScheme(udtNewSchemeClaim, udtEHSPersonalInformation, dtmServiceDate)

                    udtClaimCategory = Me._udtSessionHandler.ClaimCategoryGetFromSession(FunctCode)

                    ' CRE20-0022 (Immu record) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                    ' Save selected option to session
                    If Not udtNewSchemeClaim Is Nothing Then
                        Select Case udtNewSchemeClaim.ControlType
                            Case SchemeClaimModel.EnumControlType.VSS
                                If Me.ClaimMode = ClaimMode.COVID19 Then
                                    Dim udcInputVSSCOVID19 As ucInputVSSCOVID19 = Me.udcStep2aInputEHSClaim.GetVSSCOVID19Control()

                                    If udcInputVSSCOVID19 IsNot Nothing Then
                                        _udtSessionHandler.ClaimCOVID19CategorySaveToSession(udcInputVSSCOVID19.CategoryForCOVID19, FunctionCode)
                                        _udtSessionHandler.ClaimCOVID19VaccineBrandSaveToSession(udcInputVSSCOVID19.VaccineBrand, FunctionCode)
                                        _udtSessionHandler.ClaimCOVID19VaccineLotNoSaveToSession(udcInputVSSCOVID19.VaccineLotNo, FunctionCode)
                                    End If
                                End If

                            Case SchemeClaimModel.EnumControlType.RVP
                                If Me.ClaimMode = ClaimMode.COVID19 Then
                                    Dim udcInputRVPCOVID19 As ucInputRVPCOVID19 = Me.udcStep2aInputEHSClaim.GetRVPCOVID19Control()

                                    If udcInputRVPCOVID19 IsNot Nothing Then
                                        _udtSessionHandler.ClaimCOVID19CategorySaveToSession(udcInputRVPCOVID19.CategoryForCOVID19, FunctionCode)
                                        _udtSessionHandler.ClaimCOVID19VaccineBrandSaveToSession(udcInputRVPCOVID19.VaccineBrand, FunctionCode)
                                        _udtSessionHandler.ClaimCOVID19VaccineLotNoSaveToSession(udcInputRVPCOVID19.VaccineLotNo, FunctionCode)
                                    End If
                                End If

                            Case SchemeClaimModel.EnumControlType.COVID19
                                Dim udcInputCOVID19 As ucInputCOVID19 = Me.udcStep2aInputEHSClaim.GetCOVID19Control()

                                If udcInputCOVID19 IsNot Nothing Then
                                    _udtSessionHandler.ClaimCOVID19CategorySaveToSession(udcInputCOVID19.CategoryForCOVID19, FunctionCode)
                                    _udtSessionHandler.ClaimCOVID19VaccineBrandSaveToSession(udcInputCOVID19.VaccineBrand, FunctionCode)
                                    _udtSessionHandler.ClaimCOVID19VaccineLotNoSaveToSession(udcInputCOVID19.VaccineLotNo, FunctionCode)
                                End If

                            Case SchemeClaimModel.EnumControlType.COVID19RVP
                                Dim udcInputCOVID19RVP As ucInputCOVID19RVP = Me.udcStep2aInputEHSClaim.GetCOVID19RVPControl()

                                If udcInputCOVID19RVP IsNot Nothing Then
                                    _udtSessionHandler.ClaimCOVID19CategorySaveToSession(udcInputCOVID19RVP.CategoryForCOVID19, FunctionCode)
                                    _udtSessionHandler.ClaimCOVID19VaccineBrandSaveToSession(udcInputCOVID19RVP.VaccineBrand, FunctionCode)
                                    _udtSessionHandler.ClaimCOVID19VaccineLotNoSaveToSession(udcInputCOVID19RVP.VaccineLotNo, FunctionCode)
                                End If

                            Case SchemeClaimModel.EnumControlType.COVID19OR
                                Dim udcInputCOVID19OR As ucInputCOVID19OR = Me.udcStep2aInputEHSClaim.GetCOVID19ORControl()

                                If udcInputCOVID19OR IsNot Nothing Then
                                    _udtSessionHandler.ClaimCOVID19CategorySaveToSession(udcInputCOVID19OR.CategoryForCOVID19, FunctionCode)
                                    _udtSessionHandler.ClaimCOVID19VaccineBrandSaveToSession(udcInputCOVID19OR.VaccineBrand, FunctionCode)
                                    _udtSessionHandler.ClaimCOVID19VaccineLotNoSaveToSession(udcInputCOVID19OR.VaccineLotNo, FunctionCode)
                                End If

                        End Select
                    End If
                    ' CRE20-0022 (Immu record) [End][Chris YIM]

                    ' -------------------------------------------------------------------------------
                    ' Re-Check Eligible
                    ' -------------------------------------------------------------------------------
                    Dim _udtClaimRulesBLL As New ClaimRulesBLL()
                    Dim udtEligibleResult As ClaimRulesBLL.EligibleResult
                    Dim strRuleResultKey As String = String.Empty


                    Dim udtPractice As PracticeModel = Nothing
                    Dim udtFilterSchemeClaim As SchemeClaimModel = New SchemeClaimModel(udtNewSchemeClaim)
                    If Not IsNothing(udtSelectedPractice) Then
                        udtPractice = udtSP.PracticeList(udtSelectedPractice.PracticeID)
                    End If

                    'Check Eligibility with all available subsidizes
                    Dim udtRuleResultsForCheck As RuleResultCollection = New RuleResultCollection()
                    For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtFilterSchemeClaim.SubsidizeGroupClaimList
                        Dim udtSchemeClaimForCheck As SchemeClaimModel = New SchemeClaimModel(udtFilterSchemeClaim)
                        Dim udtSubsidizeGroupClaimListForCheck As New SubsidizeGroupClaimModelCollection

                        udtSubsidizeGroupClaimListForCheck.Add(udtSubsidizeGroupClaim)

                        If Not udtSubsidizeGroupClaimListForCheck Is Nothing AndAlso udtSubsidizeGroupClaimListForCheck.Count > 0 Then
                            udtSchemeClaimForCheck.SubsidizeGroupClaimList = udtSubsidizeGroupClaimListForCheck
                        End If

                        udtEligibleResult = _udtClaimRulesBLL.CheckEligibilityFromEHSClaimSearch(udtSchemeClaimForCheck, udtEHSPersonalInformation.IdentityNum, udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.DOB, udtEHSPersonalInformation.ExactDOB, dtmServiceDate, udtEHSPersonalInformation.Gender, udtTranBenefitList, udtPractice)

                        If Not udtEligibleResult Is Nothing Then
                            udtEligibleResult.PromptConfirmed = True
                            'Key = 1_G0002
                            strRuleResultKey = Me.RuleResultKey(udtSubsidizeGroupClaim.SubsidizeCode, udtEligibleResult.RuleType)

                            udtRuleResultsForCheck.Add(strRuleResultKey, udtEligibleResult)
                        End If
                    Next

                    If Not udtRuleResultsForCheck Is Nothing AndAlso udtRuleResultsForCheck.Count > 0 Then
                        Me._udtSessionHandler.EligibleResultVSSReminderSaveToSession(udtRuleResultsForCheck)
                    End If

                    'Update Eligibility Result
                    If Not _udtSessionHandler.ClaimCategoryGetFromSession(FunctionCode) Is Nothing Then
                        Dim udtFilterClaimCategorys As ClaimCategoryModelCollection = Me._udtClaimCategoryBLL.getAllCategoryCache()

                        Dim udtFilterSubsidizeGroupClaimList As New SubsidizeGroupClaimModelCollection

                        For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtFilterSchemeClaim.SubsidizeGroupClaimList
                            Dim udtFilterClaimCategoryList As ClaimCategoryModelCollection = udtFilterClaimCategorys.Filter(udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode)

                            For Each udtFilterClaimCategory As ClaimCategoryModel In udtFilterClaimCategoryList
                                If udtFilterClaimCategory.CategoryCode = udtClaimCategory.CategoryCode Then
                                    udtFilterSubsidizeGroupClaimList.Add(udtSubsidizeGroupClaim)
                                End If
                            Next

                        Next

                        If Not udtFilterSubsidizeGroupClaimList Is Nothing AndAlso udtFilterSubsidizeGroupClaimList.Count > 0 Then
                            udtFilterSchemeClaim.SubsidizeGroupClaimList = udtFilterSubsidizeGroupClaimList
                        End If
                    End If

                    udtEligibleResult = _udtClaimRulesBLL.CheckEligibilityFromEHSClaimSearch(udtFilterSchemeClaim, udtEHSPersonalInformation.IdentityNum, udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.DOB, udtEHSPersonalInformation.ExactDOB, dtmServiceDate, udtEHSPersonalInformation.Gender, udtTranBenefitList, udtPractice)

                    If udtEligibleResult Is Nothing Then
                        Me._udtSessionHandler.EligibleResultRemoveFromSession()
                    Else
                        Dim udtRuleResults As RuleResultCollection = New RuleResultCollection()

                        udtEligibleResult.PromptConfirmed = True
                        'Key = 1_G0002 -> not need prompt confirm popup dox -> reminder in step2a
                        strRuleResultKey = Me.RuleResultKey(ActiveViewIndex.Step1, udtEligibleResult.RuleType)

                        udtRuleResults.Add(strRuleResultKey, udtEligibleResult)
                        Me._udtSessionHandler.EligibleResultSaveToSession(udtRuleResults)
                    End If

                    If udtClaimCategorys.Count = 1 Then
                        udtClaimCategory = udtClaimCategorys(0)
                        Me._udtSessionHandler.ClaimCategorySaveToSession(udtClaimCategory, FunctCode)

                    ElseIf Not udtClaimCategory Is Nothing AndAlso udtClaimCategorys.Count > 1 Then
                        If udtClaimCategorys.FilterByCategoryCode(udtNewSchemeClaim.SchemeCode, udtClaimCategory.CategoryCode) Is Nothing Then
                            udtClaimCategory = Nothing
                            Me._udtSessionHandler.ClaimCategoryRemoveFromSession(FunctCode)
                        End If

                    ElseIf udtClaimCategorys.Count = 0 Then
                        udtClaimCategory = Nothing
                        Me._udtSessionHandler.ClaimCategoryRemoveFromSession(FunctCode)
                    End If

                    'strServiceDate is Changed
                    If Not udtClaimCategory Is Nothing AndAlso (Not strServiceDate.Equals(strPreviousValdatedServiceDate) OrElse IsNothing(sender)) Then
                        'Update session of SchemeClaim model list
                        Me._udtSessionHandler.SchemeSubsidizeListSaveToSession(udtCurrentSchemeClaimList, FunctCode)

                        'Update session of SchemeClaim model
                        Me._udtSessionHandler.SchemeSelectedSaveToSession(udtNewSchemeClaim, FunctCode)

                        'Add CategoryCode
                        udtInputPicker.CategoryCode = udtClaimCategory.CategoryCode

                        udtEHSClaimVaccine = Me._udtEHSClaimBLL.SearchEHSClaimVaccine(udtNewSchemeClaim, udtEHSAccount.SearchDocCode, udtEHSAccount, dtmServiceDate, True, GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), udtInputPicker)

                        Me._udtSessionHandler.EHSClaimVaccineSaveToSession(udtEHSClaimVaccine)
                        Me.udcStep2aInputEHSClaim.ResetSchemeType()
                        Me.SetupStep2aClaimContent(Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode), udtEHSAccount)

                    Else
                        Me.udcStep2aInputEHSClaim.ResetSchemeType()
                        Me.SetupStep2aClaimContent(Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode), udtEHSAccount)

                    End If

                    Select Case udtNewSchemeClaim.SchemeCode
                        Case SchemeClaimModel.EnumControlType.VSS.ToString.Trim
                            If Me.udcStep2aInputEHSClaim.AvaliableForClaim Then
                                Dim udcInputVSS As ucInputVSS = CType(Me.udcStep2aInputEHSClaim.GetVSSControl(), ucInputVSS)

                                If Not udcInputVSS Is Nothing Then
                                    udcInputVSS.SetClaimDetailError(False)
                                    udcInputVSS.FillClaimDetail(False)

                                End If

                            End If

                        Case SchemeClaimModel.EnumControlType.ENHVSSO.ToString.Trim
                            If Me.udcStep2aInputEHSClaim.AvaliableForClaim Then
                                Dim udcInputENHVSSO As ucInputENHVSSO = CType(Me.udcStep2aInputEHSClaim.GetENHVSSOControl(), ucInputENHVSSO)

                                If Not udcInputENHVSSO Is Nothing Then
                                    udcInputENHVSSO.FillClaimDetail(False)

                                End If

                            End If

                    End Select

                Else
                    ' Vaccine scheme without cateogry (e.g. CIVSS & EVSS)
                    If Not strServiceDate.Equals(strPreviousValdatedServiceDate) OrElse IsNothing(sender) Then
                        udtEHSClaimVaccine = Me._udtEHSClaimBLL.SearchEHSClaimVaccine(udtNewSchemeClaim, udtEHSAccount.SearchDocCode, udtEHSAccount, dtmServiceDate, True, GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), Nothing)

                        Me._udtSessionHandler.EHSClaimVaccineSaveToSession(udtEHSClaimVaccine)
                        Me.udcStep2aInputEHSClaim.ResetSchemeType()
                        Me.SetupStep2aClaimContent(Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode), udtEHSAccount)

                    End If
                End If

                Dim strVaccineLotNoMappingJavaScript As String = String.Empty

                If ClaimMode = Common.Component.ClaimMode.COVID19 Then
                    'Get Vaccine Brand & Lot No.
                    Select Case udtNewSchemeClaim.ControlType
                        Case SchemeClaimModel.EnumControlType.COVID19, SchemeClaimModel.EnumControlType.COVID19RVP, SchemeClaimModel.EnumControlType.COVID19OR
                            strVaccineLotNoMappingJavaScript = Me.GetVaccineLotNoMappingForCentreJavaScript(udtSelectedPractice.SPID, udtSelectedPractice.PracticeID, dtmServiceDate)

                        Case SchemeClaimModel.EnumControlType.VSS
                            strVaccineLotNoMappingJavaScript = Me.GetVaccineLotNoMappingForPrivateJavaScript(dtmServiceDate)

                        Case SchemeClaimModel.EnumControlType.RVP
                            strVaccineLotNoMappingJavaScript = Me.GetVaccineLotNoMappingForRCHJavaScript(dtmServiceDate)

                    End Select

                End If

                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "COVID19_Vaccine_LotNo_Mapping", strVaccineLotNoMappingJavaScript, True)

            End If
        Else
            Me.udcMsgBoxInfo.Clear()
        End If

        ' CRE20-0022 (Immu record) [Start][Martin Tang]
        Me.chkStep2aDeclareClaim.Checked = False
        ' CRE20-0022 (Immu record) [End][Martin Tang]
        Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail, udtAuditLogEntry, Common.Component.LogID.LOG00066, "Change ServiceDate")

        EHSClaimBasePage.AuditLogChangeServiceDateEnd(udtAuditLogEntry, Me.txtStep2aServiceDate.Text)

    End Sub

#End Region

#Region "Step 2a Setup"

    Private Sub SetupStep2a(ByVal udtEHSAccount As EHSAccountModel, ByVal createPopupPractice As Boolean, ByVal setDefaultScheme As Boolean, ByVal activeViewChanged As Boolean)
        If Me._udtSessionHandler.EHSClaimStepsGetFromSession(FunctCode) <> ActiveViewIndex.Step2a Then
            Me._blnIsRequireHandlePageRefresh = True
            Return
        End If

        'CRE15-003 System-generated Form [Start][Philip Chau]
        _udtSessionHandler.EHSClaimTempTransactionIDRemoveFromSession()
        _udtSessionHandler.EHSClaimStep3ShowLastestTransactionIDSaveToSession(False)
        'CRE15-003 System-generated Form [End][Philip Chau]

        ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [Start][Winnie]
        _udtSessionHandler.NoticedDuplicateClaimAlertRemoveFromSession()
        ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [End][Winnie]

        Dim udtSelectedPracticeDisplay As BLL.PracticeDisplayModel = Nothing
        Dim udtDataEntry As DataEntryUserModel = Nothing
        Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = Me._udtSessionHandler.SchemeSubsidizeListGetFromSession(FunctCode)
        Dim udtSchemeClaimBLL As SchemeClaimBLL = New SchemeClaimBLL()
        Dim udtSchemeClaim As SchemeClaimModel = Nothing

        Dim strDummy As String = String.Empty
        Dim strAllowDateBack As String = String.Empty
        Dim strSearchDocCode As String

        ' To Handle Concurrent Browser
        If Not Me.EHSClaimTokenNumValidation() Then
            Return
        End If

        strSearchDocCode = udtEHSAccount.SearchDocCode
        Me.GetCurrentUserAccount(Me._udtSP, udtDataEntry, False)

        'Default set invisible
        Me.panStep2aReminder.Visible = False

        'Display selected Practice
        udtSelectedPracticeDisplay = Me._udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)
        If Me._udtSessionHandler.Language = Common.Component.CultureLanguage.TradChinese OrElse Me._udtSessionHandler.Language = Common.Component.CultureLanguage.SimpChinese Then
            Me.lblStep2aPractice.Text = String.Format("{0} ({1})", udtSelectedPracticeDisplay.PracticeNameChi, udtSelectedPracticeDisplay.PracticeID)
            Me.lblStep2aPractice.CssClass = "tableTextChi"
        Else
            Me.lblStep2aPractice.Text = String.Format("{0} ({1})", udtSelectedPracticeDisplay.PracticeName, udtSelectedPracticeDisplay.PracticeID)
            Me.lblStep2aPractice.CssClass = "tableText"
        End If

        If udtSchemeClaimModelCollection Is Nothing Then
            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            ' Practice Scheme Info List Filter by COVID-19
            Dim udtFilterPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = _udtSchemeClaimBLL.FilterPracticeSchemeInfo(_udtSP.PracticeList, udtSelectedPracticeDisplay.PracticeID, Me.ClaimMode())
            ' CRE20-0022 (Immu record) [End][Chris YIM]

            'Get all available Scheme
            udtSchemeClaimModelCollection = udtSchemeClaimBLL.searchValidClaimPeriodSchemeClaimByPracticeSchemeInfoSubsidizeCode(udtFilterPracticeSchemeInfoList, Me._udtSP.SchemeInfoList)
            'Get all Eligible Scheme form available List
            udtSchemeClaimModelCollection = udtSchemeClaimBLL.searchEligibleAndExceedDocumentClaimScheme(udtEHSAccount, udtEHSAccount.SearchDocCode, udtSchemeClaimModelCollection)

            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
            ' --------------------------------------------------------------------------------------
            udtSchemeClaimModelCollection = udtSchemeClaimModelCollection.FilterWithoutReadonly()
            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

            ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
            udtSchemeClaimModelCollection = udtSchemeClaimModelCollection.FilterByHCSPSubPlatform(Me.SubPlatform)
            ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

            Me._udtSessionHandler.SchemeSubsidizeListSaveToSession(udtSchemeClaimModelCollection, FunctCode)
        End If

        'Invisibility Button
        Me.ShowChangePracticeButton(Me.btnStep2aChangePractice)

        'Display Seach RCH Model Popup
        If Me._udtSessionHandler.EHSEnterClaimDetailSearchRCHGetFromSession() Then
            Me.ModalPopupExtenderRVPHomeListSearch.Show()
        Else
            Me.ModalPopupExtenderRVPHomeListSearch.Hide()
        End If

        'Display Seach Outreach Popup
        If Me._udtSessionHandler.EHSEnterClaimDetailSearchOutreachGetFromSession() Then
            Me.ModalPopupExtenderOutreachListSearch.Show()
        Else
            Me.ModalPopupExtenderOutreachListSearch.Hide()
        End If

        If udtSchemeClaimModelCollection Is Nothing OrElse udtSchemeClaimModelCollection.Count = 0 Then
            'No Scheme for the the recipient 
            Me.SetupStep2aSchemeAvailableForClaim(False, udtEHSAccount, Nothing, createPopupPractice)

        Else
            'if not Visible here some contorl may not to enable for edit
            'CRE13-024 Notice in HCSP Claim Screen [Start][Karl]
            'Me.panStep2aClaimDetail.Visible = True
            Me.panStep2aClaimDetaila.Visible = True
            Me.panStep2aClaimDetailb.Visible = True
            'CRE13-024 Notice in HCSP Claim Screen [End][Karl]

            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Me.panNonClinicSettingStep2a.Visible = False
            'CRE16-002 (Revamp VSS) [End][Chris YIM]

            If udtSchemeClaimModelCollection.Count > 1 Then
                Me.DropDownListBindScheme(Me.ddlStep2aScheme, udtSchemeClaimModelCollection, Me._udtSessionHandler.Language, False)

                'show the scheme dropdown list
                Me.ddlStep2aScheme.Visible = True

                'invisiable the scheme label
                Me.lblStep2aSchemeSelectedText.Visible = False

                'May not have value
                udtSchemeClaim = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

                If Not udtSchemeClaim Is Nothing Then
                    'After user Selected other scheme from drop down list
                    'Session scheme is not equals to the selected scheme -> need update session scheme
                    If setDefaultScheme Then

                        ' Audit Log - Change Scheme
                        Dim strPreviousSelectedScheme As String = udtSchemeClaim.SchemeCode.Trim()

                        ' CRE20-0022 (Immu record) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                        If Me.ClaimMode = ClaimMode.COVID19 Then
                            _udtSessionHandler.ClaimCOVID19SchemeSelectedSaveToSession(udtSchemeClaim, FunctionCode)

                            udtSchemeClaim = udtSchemeClaimModelCollection.Filter(udtSchemeClaim.SchemeCode)
                        Else
                            If Not Me.ddlStep2aScheme.SelectedValue.Trim().Equals(udtSchemeClaim.SchemeCode) Then
                                udtSchemeClaim = udtSchemeClaimModelCollection.Filter(Me.ddlStep2aScheme.SelectedValue.Trim())
                            Else
                                udtSchemeClaim = udtSchemeClaimModelCollection.Filter(udtSchemeClaim.SchemeCode)
                            End If
                        End If
                        ' CRE20-0022 (Immu record) [End][Chris YIM]

                        Me._udtSessionHandler.SchemeSelectedSaveToSession(udtSchemeClaim, FunctCode)
                        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                        If Not udtSchemeClaim Is Nothing Then

                            Dim udtConvertedSchemeCode As String = udtSchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(udtSchemeClaim.SchemeCode)
                            Me._udtSessionHandler.NonClinicSettingSaveToSession(_udtSP.PracticeList(udtSelectedPracticeDisplay.PracticeID).PracticeSchemeInfoList.Filter(udtConvertedSchemeCode).IsNonClinic, FunctCode)

                            If _udtSessionHandler.NonClinicSettingGetFromSession(FunctCode) Then
                                panNonClinicSettingStep2a.Visible = True

                                If _udtSessionHandler.Language = CultureLanguage.TradChinese Then
                                    lblNonClinicSettingStep2a.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)))
                                ElseIf _udtSessionHandler.Language = CultureLanguage.SimpChinese Then
                                    lblNonClinicSettingStep2a.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese)))
                                Else
                                    lblNonClinicSettingStep2a.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.English)))
                                End If
                            Else
                                panNonClinicSettingStep2a.Visible = False
                            End If

                        End If
                        'CRE16-002 (Revamp VSS) [End][Chris YIM]

                        ' Audit Log - Change Scheme
                        If Not udtSchemeClaim Is Nothing AndAlso strPreviousSelectedScheme <> udtSchemeClaim.SchemeCode Then
                            EHSClaimBasePage.AuditLogEnterClaimDetailChangeScheme(New AuditLogEntry(FunctionCode, Me), strPreviousSelectedScheme, udtSchemeClaim.SchemeCode)
                        End If

                    Else
                        ' Put the SchemeCode with Subsidize to Session 
                        Dim udtSchemeClaimWithSubsidizeCode As SchemeClaimModel = udtSchemeClaimModelCollection.Filter(udtSchemeClaim.SchemeCode)

                        If Not udtSchemeClaimWithSubsidizeCode Is Nothing Then
                            Me._udtSessionHandler.SchemeSelectedSaveToSession(udtSchemeClaimWithSubsidizeCode, FunctCode)
                            udtSchemeClaim = udtSchemeClaimWithSubsidizeCode

                            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                            '-----------------------------------------------------------------------------------------
                            Dim udtConvertedSchemeCode As String = udtSchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(udtSchemeClaim.SchemeCode)
                            Me._udtSessionHandler.NonClinicSettingSaveToSession(_udtSP.PracticeList(udtSelectedPracticeDisplay.PracticeID).PracticeSchemeInfoList.Filter(udtConvertedSchemeCode).IsNonClinic, FunctCode)

                            If Me._udtSessionHandler.NonClinicSettingGetFromSession(FunctionCode) Then
                                panNonClinicSettingStep2a.Visible = True

                                If _udtSessionHandler.Language = CultureLanguage.TradChinese Then
                                    lblNonClinicSettingStep2a.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)))
                                ElseIf _udtSessionHandler.Language = CultureLanguage.SimpChinese Then
                                    lblNonClinicSettingStep2a.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese)))
                                Else
                                    lblNonClinicSettingStep2a.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.English)))
                                End If
                            Else
                                panNonClinicSettingStep2a.Visible = False
                            End If
                            'CRE16-002 (Revamp VSS) [End][Chris YIM]
                        End If
                    End If
                End If

                'if still have no selected Scheme 
                'Scheme can be nothing -> only one case -> After Practice changed, scheme will remove from session
                'if Practice Changed, Eligible schemes may not exist in udtSchemeClaimModelCollection
                If udtSchemeClaim Is Nothing Then
                    'As Step2a(Enter Claim Detail cannot no default Scheme) -> Set Default Scheme Select
                    udtSchemeClaim = udtSchemeClaimModelCollection(0)
                    Me._udtSessionHandler.SchemeSelectedSaveToSession(udtSchemeClaim, FunctCode)

                    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    Dim udtConvertedSchemeCode As String = udtSchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(udtSchemeClaim.SchemeCode)
                    Me._udtSessionHandler.NonClinicSettingSaveToSession(_udtSP.PracticeList(udtSelectedPracticeDisplay.PracticeID).PracticeSchemeInfoList.Filter(udtConvertedSchemeCode).IsNonClinic, FunctCode)
                    'CRE16-002 (Revamp VSS) [End][Chris YIM]
                End If

                If Me.ClaimMode <> ClaimMode.COVID19 OrElse activeViewChanged Then
                    Me.ddlStep2aScheme.SelectedValue = udtSchemeClaim.SchemeCode
                End If

            ElseIf udtSchemeClaimModelCollection.Count = 1 Then
                'Show Label for only 1 Scheme, Auto Assign
                'invisiable the scheme Drop Down List
                Me.ddlStep2aScheme.Visible = False
                Me.ddlStep2aScheme.Items.Clear()

                'show the scheme Label with default scheme
                Me.lblStep2aSchemeSelectedText.Visible = True

                If Me._udtSessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                    Me.lblStep2aSchemeSelectedText.Text = udtSchemeClaimModelCollection(0).SchemeDescChi
                ElseIf Me._udtSessionHandler.Language = Common.Component.CultureLanguage.SimpChinese Then
                    Me.lblStep2aSchemeSelectedText.Text = udtSchemeClaimModelCollection(0).SchemeDescCN
                Else
                    Me.lblStep2aSchemeSelectedText.Text = udtSchemeClaimModelCollection(0).SchemeDesc
                End If

                'Save the Default Scheme to session
                Me._udtSessionHandler.SchemeSelectedSaveToSession(udtSchemeClaimModelCollection(0), FunctCode)
                udtSchemeClaim = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

                'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim udtConvertedSchemeCode As String = udtSchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(udtSchemeClaim.SchemeCode)
                Me._udtSessionHandler.NonClinicSettingSaveToSession(_udtSP.PracticeList(udtSelectedPracticeDisplay.PracticeID).PracticeSchemeInfoList.Filter(udtConvertedSchemeCode).IsNonClinic, FunctCode)

                If Not udtSchemeClaim Is Nothing Then
                    If _udtSessionHandler.NonClinicSettingGetFromSession(FunctCode) Then
                        panNonClinicSettingStep2a.Visible = True

                        If _udtSessionHandler.Language = CultureLanguage.TradChinese Then
                            lblNonClinicSettingStep2a.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)))
                        ElseIf _udtSessionHandler.Language = CultureLanguage.SimpChinese Then
                            lblNonClinicSettingStep2a.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese)))
                        Else
                            lblNonClinicSettingStep2a.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.English)))
                        End If
                    Else
                        panNonClinicSettingStep2a.Visible = False
                    End If
                End If
                'CRE16-002 (Revamp VSS) [End][Chris YIM]
            End If

            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            SetupServiceDate(udtSchemeClaim.SchemeCode)
            ' CRE20-0022 (Immu record) [End][Chris YIM]

            ' --------------------------------------------------------------------------------------------
            ' TSW Checking
            ' --------------------------------------------------------------------------------------------
            If udtSchemeClaim.TSWCheckingEnable Then
                If Me._udtEHSClaimBLL.chkIsTSWCase(Me._udtSP.SPID, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).IdentityNum) Then
                    'Show TSW panel
                    Me.panStep2aReminder.Visible = True
                    Me.lblStep2aReminder.Text = Me.GetGlobalResourceObject("Text", "TSWRemind")
                End If
            End If

            Select Case udtSchemeClaim.SchemeCode
                ' CRE20-0022 (Immu record) [Start][Winnie SUEN]                
                Case SchemeClaimModel.EnumControlType.VSS.ToString.Trim, _
                    SchemeClaimModel.EnumControlType.ENHVSSO.ToString.Trim, _
                    SchemeClaimModel.EnumControlType.PPP.ToString.Trim, _
                    SchemeClaimModel.EnumControlType.COVID19.ToString.Trim, _
                    SchemeClaimModel.EnumControlType.COVID19CBD.ToString.Trim, _
                    SchemeClaimModel.EnumControlType.COVID19RVP.ToString.Trim, _
                    SchemeClaimModel.EnumControlType.COVID19OR.ToString.Trim
                    ' CRE20-0022 (Immu record) [End][Winnie SUEN]
                    'Nothing to do

                Case Else
                    'Check CIVSS Eligible Result, if child is over 6 years and the first dose is avalible for the child
                    Dim strRuleResults As RuleResultCollection
                    strRuleResults = Me._udtSessionHandler.EligibleResultGetFromSession()
                    For Each udtRuleResult As RuleResult In strRuleResults.Values

                        If udtRuleResult.RuleType = RuleTypeENum.EligibleResult AndAlso udtRuleResult.HandleMethod = HandleMethodENum.Declaration AndAlso udtRuleResult.PromptConfirmed Then

                            If udtRuleResult.SchemeCode.Trim() = CType(Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode), SchemeClaimModel).SchemeCode.Trim Then

                                If CType(udtRuleResult, EligibleResult).IsEligible Then

                                    Dim udtEligibleResult As EligibleResult = CType(udtRuleResult, EligibleResult)

                                    Dim strObjectName2 As String = String.Empty

                                    If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                                        strObjectName2 = udtEligibleResult.RelatedEligibleRule.ObjectName2

                                    ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                                        strObjectName2 = udtEligibleResult.RelatedEligibleExceptionRule.ObjectName2
                                    End If

                                    If Not String.IsNullOrEmpty(strObjectName2) Then
                                        Me.panStep2aReminder.Visible = True
                                        Me.lblStep2aReminder.Text = Me.GetGlobalResourceObject("Text", strObjectName2)
                                    Else
                                        Me.panStep2aReminder.Visible = False
                                    End If
                                End If
                            End If
                        End If
                    Next
            End Select

            Me.SetupStep2aSchemeAvailableForClaim(True, udtEHSAccount, udtSchemeClaim, createPopupPractice)
        End If

        'If activeViewChanged Then
        If udcMsgBoxErr.GetCodeTable.Rows.Count <> 0 Then Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail)
        'End If

        ' --------------------------------------------------------------------------------------------
        ' Vaccination Record
        ' --------------------------------------------------------------------------------------------
        udtSchemeClaim = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        Dim udtVaccinationBLL As New VaccinationBLL

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' -------------------------------------------------------------------------------------
        If Not IsNothing(udtSchemeClaim) _
                AndAlso udtVaccinationBLL.SchemeContainVaccine(udtSchemeClaim) _
                AndAlso (VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N _
                    Or VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CIMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N) Then

            ' CRE20-0022 (Immu record) [Start][Martin Tang]
            ' ---------------------------------------------------------------------------------------------------------
            If Me.ClaimMode = ClaimMode.COVID19 Then
                'In COVID19 mode
                If ViewState(VS.VaccinationRecordPopupStatus) = PopupStatusClass.Active Then
                    'If popup is shown, keep to display
                    CheckShowVaccinationRecord()
                Else
                    'If popup not shown, check connection status to determine the popup whether display
                    If IsVaccinationRecordPopUp(udtEHSAccount, udtSchemeClaim.SchemeCode) Then
                        CheckShowVaccinationRecord()
                    End If

                End If
            Else
                'In normal claim mode, always display the popup
                CheckShowVaccinationRecord()
            End If
            ' CRE20-0022 (Immu record) [End][Martin Tang]

            ibtnVaccinationRecord.Visible = True
        Else
            ibtnVaccinationRecord.Visible = False
        End If
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        'CRE13-024 Notice in HCSP Claim Screen [Start][Karl]
        If Not IsNothing(udtSchemeClaim) Then
            Call SetupStep2aSchemeNotice(udtSchemeClaim)
        End If
        'CRE13-024 Notice in HCSP Claim Screen [End][Karl]

        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        _udtSessionHandler.ClaimForSamePatientRemoveFromSession(FunctCode)
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

    End Sub

    'CRE13-024 Notice in HCSP Claim Screen [Start][Karl]
    Private Sub SetupStep2aSchemeNotice(ByVal udtSchemeClaim As SchemeClaimModel)
        Dim udtSchemeNotice As SchemeNoticeModel = Nothing
        Dim udtSchemeNoticeBLL As New SchemeNoticeBLL
        Dim dtmCurrentDateTime As DateTime = (New Common.ComFunction.GeneralFunction).GetSystemDateTime

        Me.panNotice.Visible = False

        udtSchemeNotice = udtSchemeNoticeBLL.getSchemeNoticeWithinDisplayPeriod(udtSchemeClaim.SchemeCode)

        If Not IsNothing(udtSchemeNotice) Then
            Me.panNotice.Visible = True

            If udtSchemeNotice.WithinNewPeriod(dtmCurrentDateTime) = True Then
                Me.panNoticeNew.Visible = True
            Else
                Me.panNoticeNew.Visible = False
            End If

            If Session("language").ToString().Trim.ToUpper = TradChinese.ToUpper Then
                Me.ltlNoticeContent.Text = udtSchemeNotice.HTMLContentChi
            ElseIf Session("language").ToString().Trim.ToUpper = SimpChinese.ToUpper Then
                Me.ltlNoticeContent.Text = udtSchemeNotice.HTMLContentCN
            Else
                'default english
                Me.ltlNoticeContent.Text = udtSchemeNotice.HTMLContent
            End If
        End If

    End Sub

    'CRE13-024 Notice in HCSP Claim Screen [Start][Karl]

    Private Sub SetupStep2aSchemeAvailableForClaim(ByVal isAvailable As Boolean, ByVal udtEHSAccount As EHSAccountModel, ByVal udtSchemeClaim As SchemeClaimModel, ByVal createPopupPractice As Boolean)

        Me.udcMsgBoxInfo.Clear()
        Dim udtEHSTransaction As EHSTransactionModel
        Dim udtFormatter As Formatter

        If isAvailable Then
            'Init
            udtEHSTransaction = Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)
            udtFormatter = New Formatter

            'Show claim detail
            Me.panStep2aClaimDetaila.Visible = True
            Me.panStep2aClaimDetailb.Visible = True

            'Setup Service date
            If Not udtEHSTransaction Is Nothing AndAlso Not createPopupPractice Then
                Dim udtSubPlatformBLL As New SubPlatformBLL

                Me.txtStep2aServiceDate.Text = udtFormatter.formatInputTextDate(udtEHSTransaction.ServiceDate, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
                Me.Step2aCalendarExtenderServiceDate.Format = udtFormatter.EnterDateFormat(udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
            End If

            Me.lblStep2aServiceDateText.Text = Me.GetGlobalResourceObject("Text", "ServiceDate")
            Me.lblStep2aSchemeText.Text = Me.GetGlobalResourceObject("Text", "Scheme")

            Me.SetupStep2aClaimContent(udtSchemeClaim, udtEHSAccount)
        Else
            Me.panStep2aClaimDetaila.Visible = False
            Me.panStep2aClaimDetailb.Visible = False

            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Me.panStep2aVaccinationRecord.Visible = False
            Me.panStep2aRecipinetContactInfo.Visible = False
            Me.panStep2aDeclareClaim.Visible = False
            Me.panStep2aPrintClaimConsentForm.Visible = False
            Me.panStep2aPerprintFormNotice.Visible = False
            ' CRE20-0022 (Immu record) [End][Chris YIM]

            Me.udcStep2aInputEHSClaim.Clear()
            Me.SetClaimButtonEnable(Me.btnStep2aClaim, False)

            Me.udcMsgBoxInfo.AddMessage(New SystemMessage("990000", "I", "00020"))
            Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Information
            Me.udcMsgBoxInfo.BuildMessageBox()
        End If

        If createPopupPractice Then
            Dim udtPracticeDisplays As BLL.PracticeDisplayModelCollection = Nothing
            udtPracticeDisplays = Me._udtSessionHandler.PracticeDisplayListGetFromSession(FunctCode)
            Me.udcPopupPracticeRadioButtonGroup.VerticalScrollBar = True
            ' CRE20-0XX (HA Scheme) [Start][Winnie]
            Me.udcPopupPracticeRadioButtonGroup.SchemeSelection = IIf(Me.SubPlatform = EnumHCSPSubPlatform.CN, True, False)
            Me.udcPopupPracticeRadioButtonGroup.SelectedScheme = Me._udtSessionHandler.SchemeSelectedForPracticeGetFromSession(FunctCode)
            ' CRE20-0XX (HA Scheme) [End][Winnie]
            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Me.udcPopupPracticeRadioButtonGroup.BuildRadioButtonGroup(udtPracticeDisplays, _
                                                                      Me._udtSP.PracticeList, _
                                                                      Me._udtSP.SchemeInfoList, _
                                                                      Me._udtSessionHandler.Language, _
                                                                      PracticeRadioButtonGroup.DisplayMode.BankAccount, _
                                                                      Me.ClaimMode)
            ' CRE20-0022 (Immu record) [End][Chris YIM]

            If Not udtEHSAccount Is Nothing Then
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
                ' CRE18-005 (OCSSS Popup) [Start][Chris YIM]
                ' --------------------------------------------------------------------------------------
                'Me.udcStep2aReadOnlyDocumnetType.TableTitleWidth = 185
                Me.udcStep2aReadOnlyDocumnetType.TableTitleWidth = 205
                ' CRE18-005 (OCSSS Popup) [End][Chris YIM]


                Dim udtSmartIDContent As BLL.SmartIDContentModel = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode)
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
        End If
    End Sub

    Private Sub SetupStep2aClaimContent(ByVal udtSchemeClaim As SchemeClaimModel, ByVal udtEHSAccount As EHSAccountModel)
        Dim notAvailableForClaim As Boolean = True
        Dim isEligibleForClaim As Boolean = True
        Dim noCategorys As Boolean = True
        Dim blnCOVID19ForClaim As Boolean = False

        Dim udtFormatter As Formatter = New Formatter

        Dim udtSubPlatformBLL As New SubPlatformBLL
        Dim strServiceDate As String = udtFormatter.formatInputDate(Me.txtStep2aServiceDate.Text, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))

        Dim dtmServiceDate As DateTime
        Dim udtClaimCategory As ClaimCategoryModel = Me._udtSessionHandler.ClaimCategoryGetFromSession(FunctCode)
        Dim blnInClaimPeriod As Boolean = False
        Dim blnWithoutConversionRate As Boolean = False
        Dim blnNoAvailableQuota As Boolean = False

        Dim udtInputPicker As InputPickerModel = New InputPickerModel

        Dim udtSelectedPracticeDisplay As PracticeDisplayModel = Me._udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)

        'Prepare PracticeSchemeInfoCollection for compare value of provider service
        Dim intSelectedPracticeDisplay As Integer = udtSelectedPracticeDisplay.PracticeID

        Dim udtSP As Common.Component.ServiceProvider.ServiceProviderModel = Nothing
        Dim udtDataEntry As Common.Component.DataEntryUser.DataEntryUserModel = Nothing
        Me._udtSessionHandler.CurrentUserGetFromSession(udtSP, udtDataEntry)
        Dim udtLatestC19Vaccine As TransactionDetailVaccineModel = Nothing
        Dim udtLatestC19Transaction As EHSTransactionModel = Nothing

        '----------------------------------------------------------------------------
        ' Prepare value for proceed
        '----------------------------------------------------------------------------

        udtInputPicker.SPID = udtSP.SPID
        udtInputPicker.PracticeDisplaySeq = intSelectedPracticeDisplay

        strServiceDate = udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English)

        If Not DateTime.TryParse(strServiceDate, dtmServiceDate) OrElse Not IsValidServiceDate(Me.txtStep2aServiceDate.Text) Then
            dtmServiceDate = udtFormatter.convertDate(Me.txtStep2aServiceDate.Attributes(ValidatedServiceDate), Common.Component.CultureLanguage.English)
        End If

        '----------------------------------------------------------------------------
        ' Retrieve the available subsidy by scheme
        '----------------------------------------------------------------------------

        If Not udtSchemeClaim Is Nothing Then
            blnInClaimPeriod = _udtSchemeClaimBLL.IsServiceDateWithinClaimPeriod(udtSchemeClaim.SchemeCode, dtmServiceDate)

            If blnInClaimPeriod Then

                Select Case udtSchemeClaim.ControlType

                    Case SchemeClaimModel.EnumControlType.VOUCHER, SchemeClaimModel.EnumControlType.VOUCHERCHINA
                        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                        If udtEHSAccount.VoucherInfo Is Nothing Then
                            ' Recheck available voucher when service date change (Handle date back to last year)

                            Dim udtVoucherInfo As New VoucherInfoModel(VoucherInfoModel.AvailableVoucher.Include, _
                                                                       VoucherInfoModel.AvailableQuota.Include)

                            udtVoucherInfo.GetInfo(dtmServiceDate, _
                                                    Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode), _
                                                    udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode), _
                                                    udtSelectedPracticeDisplay.ServiceCategoryCode)

                            udtEHSAccount.VoucherInfo = udtVoucherInfo

                            Me._udtSessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctCode)
                        End If
                        ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [End][Winnie]

                        If udtEHSAccount.VoucherInfo.GetAvailableVoucher > 0 Then
                            notAvailableForClaim = False

                            ' CRE19-003 (Opt voucher capping) [Start][Winnie]
                            ' ----------------------------------------------------------------------------------------
                            Dim udtVoucherQuota As VoucherQuotaModel = udtEHSAccount.VoucherInfo.VoucherQuotalist.FilterByProfCodeEffectiveDtm(udtSelectedPracticeDisplay.ServiceCategoryCode, dtmServiceDate)

                            If Not udtVoucherQuota Is Nothing Then
                                If udtVoucherQuota.AvailableQuota <= 0 Then
                                    notAvailableForClaim = True
                                    blnNoAvailableQuota = True

                                    Me._udtSystemMessage = New SystemMessage("990000", "E", "00424")
                                    Dim strMsg_en = String.Format(HttpContext.GetGlobalResourceObject("Text", "ProfessionQuota", New System.Globalization.CultureInfo(CultureLanguage.English)) _
                                                                  , HttpContext.GetGlobalResourceObject("Text", udtVoucherQuota.ProfCode, New System.Globalization.CultureInfo(CultureLanguage.English)))

                                    Dim strMsg_tc = String.Format(HttpContext.GetGlobalResourceObject("Text", "ProfessionQuota", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)) _
                                                                  , HttpContext.GetGlobalResourceObject("Text", udtVoucherQuota.ProfCode, New System.Globalization.CultureInfo(CultureLanguage.TradChinese)))

                                    _udtSystemMessage.AddReplaceMessage("%en", strMsg_en)
                                    _udtSystemMessage.AddReplaceMessage("%tc", strMsg_tc)
                                End If
                            End If
                            ' CRE19-003 (Opt voucher capping) [End][Winnie]
                        End If

                        If udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.VOUCHERCHINA Then
                            'Check is there any effective exchange rate

                            Dim decExchangeRate As Decimal = 0
                            Dim udtExchangeRateBLL As ExchangeRate.ExchangeRateBLL = New ExchangeRate.ExchangeRateBLL()
                            Dim lstrServiceDate As String

                            lstrServiceDate = udtFormatter.convertDate(Me.txtStep2aServiceDate.Attributes(ValidatedServiceDate), Common.Component.CultureLanguage.English)

                            If String.IsNullOrEmpty(lstrServiceDate) = False Then
                                decExchangeRate = udtExchangeRateBLL.GetExchangeRateValue(CDate(lstrServiceDate))
                                If decExchangeRate <= 0 Then
                                    notAvailableForClaim = True
                                    blnWithoutConversionRate = True
                                    Me._udtSystemMessage = New SystemMessage(FunctCode, "E", "00002") 'Voucher conversion rate is invalid. Contact the Department of Health if assistance is required.                                    
                                End If
                            End If
                        End If
                        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

                        ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
                        ' --------------------------------------------------------------------------------------
                    Case SchemeClaimModel.EnumControlType.EVSS, SchemeClaimModel.EnumControlType.CIVSS, _
                        SchemeClaimModel.EnumControlType.HSIVSS, SchemeClaimModel.EnumControlType.RVP, _
                        SchemeClaimModel.EnumControlType.PIDVSS, SchemeClaimModel.EnumControlType.VSS, _
                        SchemeClaimModel.EnumControlType.ENHVSSO, SchemeClaimModel.EnumControlType.PPP, _
                        SchemeClaimModel.EnumControlType.COVID19, SchemeClaimModel.EnumControlType.COVID19CBD, _
                        SchemeClaimModel.EnumControlType.COVID19RVP, SchemeClaimModel.EnumControlType.COVID19OR
                        ' CRE20-0022 (Immu record) [End][Winnie SUEN]

                        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = Nothing
                        Dim needCreateVaccine As Boolean = False

                        udtEHSClaimVaccine = Me._udtSessionHandler.EHSClaimVaccineGetFromSession()

                        'if not Vaccine -> get the vaccination list
                        If udtEHSClaimVaccine Is Nothing OrElse Not udtEHSClaimVaccine.SchemeCode.Equals(udtSchemeClaim.SchemeCode) Then
                            needCreateVaccine = True
                        End If

                        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
                        ' --------------------------------------------------------------------------------------
                        'Search available subsidy of Vaccine with different scheme 
                        Select Case udtSchemeClaim.ControlType

                            ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
                            Case SchemeClaimModel.EnumControlType.HSIVSS, SchemeClaimModel.EnumControlType.RVP, _
                                SchemeClaimModel.EnumControlType.VSS, SchemeClaimModel.EnumControlType.ENHVSSO, SchemeClaimModel.EnumControlType.PPP, _
                                SchemeClaimModel.EnumControlType.COVID19, SchemeClaimModel.EnumControlType.COVID19CBD, _
                                SchemeClaimModel.EnumControlType.COVID19RVP, SchemeClaimModel.EnumControlType.COVID19OR
                                ' CRE20-0022 (Immu record) [End][Winnie SUEN]

                                If ClaimMode = Common.Component.ClaimMode.COVID19 Then
                                    'Get Vaccine Lot No. mapping

                                    Select Case udtSchemeClaim.ControlType
                                        Case SchemeClaimModel.EnumControlType.VSS
                                            Dim drVaccineLotNo() As DataRow = Me.GetCOVID19VaccineLotMappingForPrivate(dtmServiceDate)

                                            If drVaccineLotNo IsNot Nothing AndAlso drVaccineLotNo.Length > 0 Then
                                                blnCOVID19ForClaim = True
                                            End If

                                        Case SchemeClaimModel.EnumControlType.RVP
                                            Dim drVaccineLotNo() As DataRow = Me.GetCOVID19VaccineLotMappingForRCH(dtmServiceDate)

                                            If drVaccineLotNo IsNot Nothing AndAlso drVaccineLotNo.Length > 0 Then
                                                blnCOVID19ForClaim = True
                                            End If

                                        Case SchemeClaimModel.EnumControlType.COVID19, SchemeClaimModel.EnumControlType.COVID19RVP, _
                                             SchemeClaimModel.EnumControlType.COVID19OR

                                            Dim drVaccineLotNo() As DataRow = Me.GetCOVID19VaccineLotMappingForCentre(udtSP.SPID, udtSelectedPracticeDisplay.PracticeID, dtmServiceDate)

                                            If drVaccineLotNo IsNot Nothing AndAlso drVaccineLotNo.Length > 0 Then
                                                blnCOVID19ForClaim = True
                                            End If

                                        Case SchemeClaimModel.EnumControlType.COVID19CBD
                                            Dim drVaccineLotNo() As DataRow = Me.GetCOVID19VaccineLotMapping(udtSP.SPID, udtSelectedPracticeDisplay.PracticeID, dtmServiceDate)

                                            If drVaccineLotNo IsNot Nothing AndAlso drVaccineLotNo.Length > 0 Then
                                                blnCOVID19ForClaim = True
                                            End If

                                    End Select

                                End If

                                '--------------------
                                ' With Category
                                '--------------------
                                Dim udtClaimCategorys As ClaimCategoryModelCollection
                                Dim udtPersonalInformation As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode)
                                Dim strEnableClaimCategory As String = String.Empty

                                '--------------------------------------
                                'Part 1: Retrieve Claim Category
                                '--------------------------------------
                                'udtClaimCategorys = Me._udtClaimCategoryBLL.getDistinctCategoryByScheme(udtNewSchemeClaimUpdated, udtPersonalInformation, dtmServiceDate)
                                udtClaimCategorys = Me._udtClaimCategoryBLL.getDistinctCategoryByScheme(udtSchemeClaim, udtPersonalInformation, dtmServiceDate)

                                'Check Claim Category list
                                If Not udtClaimCategorys Is Nothing AndAlso udtClaimCategorys.Count > 0 Then
                                    noCategorys = False
                                    If udtClaimCategorys.Count = 1 Then
                                        udtClaimCategory = udtClaimCategorys(0)
                                        Me._udtSessionHandler.ClaimCategorySaveToSession(udtClaimCategory, FunctCode)
                                    End If
                                Else
                                    isEligibleForClaim = False
                                    Me._udtSessionHandler.ClaimCategoryRemoveFromSession(FunctCode)
                                End If

                                'Assign Claim Category List to control
                                Me.udcStep2aInputEHSClaim.ClaimCategorys = udtClaimCategorys

                                '--------------------------------------
                                'Part 2.1: Search Vaccine (RVP,HSIVSS)
                                '--------------------------------------
                                If udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.RVP) AndAlso Me.ClaimMode = Common.Component.ClaimMode.All Then
                                    _udtGeneralFunction.getSytemParameterByParameterNameSchemeCode("RVPEnableClaimCategory", strEnableClaimCategory, String.Empty, SchemeClaimModel.RVP)
                                End If

                                If strEnableClaimCategory = "Y" OrElse udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.HSIVSS) Then

                                    If Not udtClaimCategory Is Nothing AndAlso udtClaimCategory.SchemeCode = udtSchemeClaim.SchemeCode.Trim() Then
                                        'Category has been selected
                                        If needCreateVaccine Then
                                            'Add CategoryCode
                                            udtInputPicker.CategoryCode = udtClaimCategory.CategoryCode

                                            udtEHSClaimVaccine = Me._udtEHSClaimBLL.SearchEHSClaimVaccine(udtSchemeClaim, udtEHSAccount.SearchDocCode, udtEHSAccount, dtmServiceDate, True, GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), udtInputPicker)
                                        End If

                                    Else
                                        'Category not selected or categrory is not for this scheme
                                        If udtClaimCategorys.Count = 1 Then
                                            udtClaimCategory = udtClaimCategorys(0)
                                            Me._udtSessionHandler.ClaimCategorySaveToSession(udtClaimCategory, FunctCode)

                                            If needCreateVaccine Then
                                                'Add CategoryCode
                                                udtInputPicker.CategoryCode = udtClaimCategory.CategoryCode

                                                udtEHSClaimVaccine = Me._udtEHSClaimBLL.SearchEHSClaimVaccine(udtSchemeClaim, udtEHSAccount.SearchDocCode, udtEHSAccount, dtmServiceDate, True, GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), udtInputPicker)
                                            End If

                                        Else
                                            'Scheme Change 
                                            '1) Remove category
                                            '2) no vaccine
                                            Me._udtSessionHandler.ClaimCategoryRemoveFromSession(FunctCode)
                                            Me._udtSessionHandler.EHSClaimVaccineRemoveFromSession()
                                            udtEHSClaimVaccine = Nothing
                                        End If

                                    End If

                                ElseIf strEnableClaimCategory = "N" Then
                                    'For RVP, the category "Resident" is shown only when the system parameter "RVPEnableClaimCategory" is "N"
                                    udtClaimCategory = udtClaimCategorys.FilterByCategoryCode(udtSchemeClaim.SchemeCode, "RESIDENT")

                                    Me._udtSessionHandler.ClaimCategorySaveToSession(udtClaimCategory, FunctCode)

                                    If needCreateVaccine Then
                                        'Add CategoryCode
                                        udtInputPicker.CategoryCode = udtClaimCategory.CategoryCode

                                        udtEHSClaimVaccine = Me._udtEHSClaimBLL.SearchEHSClaimVaccine(udtSchemeClaim, udtEHSAccount.SearchDocCode, udtEHSAccount, dtmServiceDate, True, GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), udtInputPicker)
                                    End If

                                    noCategorys = False
                                End If

                                '--------------------------------------
                                'Part 2.2: Search Vaccine (VSS, ENHVSSO, PPP)
                                '--------------------------------------
                                If (udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.VSS) AndAlso Me.ClaimMode = Common.Component.ClaimMode.All) OrElse _
                                   udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.ENHVSSO) OrElse _
                                   udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.PPP) _
                                   Then

                                    If Not udtClaimCategory Is Nothing AndAlso udtClaimCategory.SchemeCode = udtSchemeClaim.SchemeCode.Trim() Then
                                        'Category has been selected
                                        If needCreateVaccine Then
                                            udtInputPicker.CategoryCode = udtClaimCategory.CategoryCode

                                            udtEHSClaimVaccine = Me._udtEHSClaimBLL.SearchEHSClaimVaccine(udtSchemeClaim, udtEHSAccount.SearchDocCode, udtEHSAccount, dtmServiceDate, True, GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), udtInputPicker)

                                        End If

                                    Else
                                        'Category not selected or categrory is not for this scheme
                                        If udtClaimCategorys.Count = 1 Then
                                            udtClaimCategory = udtClaimCategorys(0)
                                            Me._udtSessionHandler.ClaimCategorySaveToSession(udtClaimCategory, FunctCode)

                                            If needCreateVaccine Then
                                                udtInputPicker.CategoryCode = udtClaimCategory.CategoryCode

                                                udtEHSClaimVaccine = Me._udtEHSClaimBLL.SearchEHSClaimVaccine(udtSchemeClaim, udtEHSAccount.SearchDocCode, udtEHSAccount, dtmServiceDate, True, GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), udtInputPicker)
                                            End If

                                        Else
                                            'Scheme Change 
                                            '1) Remove category
                                            '2) no vaccine
                                            Me._udtSessionHandler.ClaimCategoryRemoveFromSession(FunctCode)
                                            Me._udtSessionHandler.EHSClaimVaccineRemoveFromSession()
                                            udtEHSClaimVaccine = Nothing
                                        End If

                                    End If

                                End If

                                '--------------------------------------
                                'Part 2.3: Search Vaccine (COVID19)
                                '--------------------------------------
                                ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
                                If udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.COVID19) OrElse _
                                   udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.COVID19CBD) OrElse _
                                   udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.COVID19RVP) OrElse _
                                   udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.COVID19OR) OrElse _
                                   (udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.VSS) AndAlso Me.ClaimMode = Common.Component.ClaimMode.COVID19) OrElse _
                                   (udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.RVP) AndAlso Me.ClaimMode = Common.Component.ClaimMode.COVID19) _
                                   Then     ' CRE20-0022 (Immu record) [End][Winnie SUEN]

                                    Dim udtTranDetailVaccineList As TransactionDetailVaccineModelCollection = GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode)
                                    Dim udtC19VaccineList As TransactionDetailVaccineModelCollection = udtTranDetailVaccineList.FilterIncludeBySubsidizeItemCode(SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19)

                                    For Each udtC19Vaccine As TransactionDetailVaccineModel In udtC19VaccineList
                                        'Find the latest COVID19 transaction in EHS
                                        If udtC19Vaccine.AvailableItemCode.Trim.ToUpper = "1STDOSE" Then
                                            If udtLatestC19Vaccine Is Nothing Then
                                                udtLatestC19Vaccine = udtC19Vaccine
                                            Else
                                                If udtC19Vaccine.ServiceReceiveDtm > udtLatestC19Vaccine.ServiceReceiveDtm Then
                                                    udtLatestC19Vaccine = udtC19Vaccine
                                                End If
                                            End If
                                        End If
                                    Next

                                    'Only allow EHS Transaction, not include CMS / CIMS record
                                    If udtLatestC19Vaccine IsNot Nothing AndAlso udtLatestC19Vaccine.TransactionID <> String.Empty Then
                                        udtLatestC19Transaction = _udtEHSTransactionBLL.LoadClaimTran(udtLatestC19Vaccine.TransactionID.Trim)
                                        udtInputPicker.LatestC19Transaction = udtLatestC19Transaction
                                    End If

                                    udtEHSClaimVaccine = Me._udtEHSClaimBLL.SearchEHSClaimVaccine(udtSchemeClaim, udtEHSAccount.SearchDocCode, udtEHSAccount, dtmServiceDate, True, GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), udtInputPicker)

                                    Dim udtFilterSubsidizeList As New EHSClaimVaccineModel.EHSClaimSubsidizeModelCollection

                                    For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
                                        If udtEHSClaimSubsidize.SubsidizeItemCode.ToUpper.Trim = SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19 Then
                                            udtFilterSubsidizeList.Add(udtEHSClaimSubsidize)
                                        End If
                                    Next

                                    udtEHSClaimVaccine.SubsidizeList = udtFilterSubsidizeList

                                End If

                                '--------------------------------------------------------------------------------------------------------------------------------
                                ' Part 3: If practice has enrolled the scheme and has provided service under that scheme, the subsidize will add in the pool for display.
                                '--------------------------------------------------------------------------------------------------------------------------------
                                If Not udtEHSClaimVaccine Is Nothing AndAlso _
                                    Not udtEHSClaimVaccine.SubsidizeList Is Nothing AndAlso _
                                    Not udtSP.PracticeList(intSelectedPracticeDisplay) Is Nothing AndAlso _
                                    Not udtSP.PracticeList(intSelectedPracticeDisplay).PracticeSchemeInfoList Is Nothing Then
                                    Dim udtResEHSClaimSubsidizeModelCollection As New EHSClaimVaccineModel.EHSClaimSubsidizeModelCollection

                                    For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
                                        Dim blnRes As Boolean = False

                                        For Each udtPracticeSchemeInfo As PracticeSchemeInfo.PracticeSchemeInfoModel In udtSP.PracticeList(intSelectedPracticeDisplay).PracticeSchemeInfoList.Values
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
                                'CRE16-026 (Add PCV13) [End][Chris YIM]

                                '------------------------------------------------------------
                                ' Part 4: Determine whether it is available for claim
                                '------------------------------------------------------------
                                If Not udtEHSClaimVaccine Is Nothing AndAlso Not noCategorys AndAlso (Not udtClaimCategory Is Nothing OrElse Me.ClaimMode = ClaimMode.COVID19) Then
                                    If Not udtEHSClaimVaccine.SubsidizeList Is Nothing Then
                                        'Check if vaccine is avaliable for the recipient -> change "notAvailableForClaim" to false
                                        For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
                                            If udtEHSClaimSubsidize.Available Then
                                                notAvailableForClaim = False
                                                Exit For
                                            End If
                                        Next
                                    Else
                                        udtEHSClaimVaccine = Nothing
                                        notAvailableForClaim = True
                                    End If
                                ElseIf Not noCategorys Then
                                    notAvailableForClaim = False
                                Else
                                    notAvailableForClaim = True
                                End If

                            Case Else
                                '--------------------
                                ' Without Category
                                '--------------------
                                'For EVSS and CIVSS


                                noCategorys = False
                                'Default
                                '--------------------------------------
                                'Part 1: Search Vaccine
                                '--------------------------------------
                                If needCreateVaccine Then
                                    udtEHSClaimVaccine = Me._udtEHSClaimBLL.SearchEHSClaimVaccine(udtSchemeClaim, udtEHSAccount.SearchDocCode, udtEHSAccount, dtmServiceDate, True, GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), Nothing)
                                End If

                                '--------------------------------------------------------------------------------------------------------------------------------
                                ' Part 2: If practice has enrolled the scheme and has provided service under that scheme, the subsidize will add in the pool for display.
                                '--------------------------------------------------------------------------------------------------------------------------------
                                If Not udtEHSClaimVaccine Is Nothing AndAlso _
                                    Not udtSP.PracticeList(intSelectedPracticeDisplay) Is Nothing AndAlso _
                                    Not udtSP.PracticeList(intSelectedPracticeDisplay).PracticeSchemeInfoList Is Nothing Then
                                    Dim udtResEHSClaimSubsidizeModelCollection As New EHSClaimVaccineModel.EHSClaimSubsidizeModelCollection

                                    For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
                                        Dim blnRes As Boolean = False

                                        For Each udtPracticeSchemeInfo As PracticeSchemeInfo.PracticeSchemeInfoModel In udtSP.PracticeList(intSelectedPracticeDisplay).PracticeSchemeInfoList.Values
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
                                'CRE16-026 (Add PCV13) [End][Chris YIM]

                                '----------------------------------------------------------------------
                                'Part 3: Check Vaccine is available for Claim
                                '----------------------------------------------------------------------
                                If Not udtEHSClaimVaccine Is Nothing Then
                                    If Not udtEHSClaimVaccine.SubsidizeList Is Nothing Then
                                        'Check if no vaccine is avaliable for the recipient -> change "noAvailableForClaim" to false
                                        For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
                                            If udtEHSClaimSubsidize.Available Then
                                                notAvailableForClaim = False
                                                Exit For
                                            End If
                                        Next
                                    Else
                                        udtEHSClaimVaccine = Nothing
                                        ' No available subsidize for Claim
                                        ' Case 1: Not Eligiblity
                                        ' Case 2: Out of period
                                        ' Case 3: The subsidizes is used
                                        For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtSchemeClaim.SubsidizeGroupClaimList
                                            If udtSubsidizeGroupClaim.LastServiceDtm >= dtmServiceDate Then
                                                isEligibleForClaim = False
                                            End If
                                        Next
                                    End If
                                Else
                                    ' No available subsidize for Claim
                                    ' Case 1: Not Eligiblity
                                    ' Case 2: Out of period
                                    ' Case 3: The subsidizes is used
                                    For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtSchemeClaim.SubsidizeGroupClaimList
                                        If udtSubsidizeGroupClaim.LastServiceDtm >= dtmServiceDate Then
                                            isEligibleForClaim = False
                                        End If
                                    Next
                                End If
                        End Select

                        Me._udtSessionHandler.EHSClaimVaccineSaveToSession(udtEHSClaimVaccine)

                        Me.udcStep2aInputEHSClaim.EHSClaimVaccine = udtEHSClaimVaccine

                        AddHandler Me.udcStep2aInputEHSClaim.VaccineLegendClicked, AddressOf udcInputEHSClaim_VaccineLegendClick
                        AddHandler Me.udcStep2aInputEHSClaim.SubsidizeDisabledRemarkClicked, AddressOf udcInputEHSClaim_SubsidizeDisabledRemarkClick
                        AddHandler Me.udcStep2aInputEHSClaim.RecipientConditionHelpClicked, AddressOf udcInputEHSClaim_RecipientConditionHelpClick
                        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]
                        If udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.COVID19RVP Then
                            AddHandler Me.udcStep2aInputEHSClaim.RCHCodeTextChanged, AddressOf udcInputEHSClaim_RCHCodeTextChanged
                        End If

                        ' CRE13-001 - EHAPP [Start][Tommy L]
                        ' -------------------------------------------------------------------------------------
                    Case SchemeClaimModel.EnumControlType.EHAPP

                        Dim udtClaimRulesBLL As New ClaimRulesBLL
                        Dim udtSchemeClaimBLL As New SchemeClaimBLL
                        Dim udtSchemeDetailBLL As New SchemeDetailBLL
                        Dim udtEHSTransactionBLL As New EHSTransactionBLL

                        Dim udtSchemeClaim_ValidPeriod_EHAPP As SchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtSchemeClaim.SchemeCode, dtmServiceDate.Date.AddDays(1).AddMinutes(-1))

                        If udtSchemeClaim_ValidPeriod_EHAPP Is Nothing Then

                            ' Scheme and Subsidy is not Available

                        Else

                            Dim udtEHSPersonalInformation As EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
                            Dim udtTransDetailBenefitList As TransactionDetailModelCollection = udtEHSTransactionBLL.getTransactionDetailBenefit(udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.IdentityNum, udtSchemeClaim_ValidPeriod_EHAPP.SchemeCode)

                            Dim udtTransDetailBenefitListByAvailItem As TransactionDetailModelCollection
                            Dim udtSubsidizeItemDetailList As SubsidizeItemDetailsModelCollection

                            Dim intNumSubsidize_Total As Integer
                            Dim intNumSubsidize_Used As Integer

                            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                            ' -----------------------------------------------------------------------------------------
                            'Dim dtmDOB As Date
                            'Dim strExactDOB As String

                            'If udtEHSPersonalInformation.ExactDOB = EHSAccount.EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                            '    dtmDOB = CType(udtEHSPersonalInformation.ECDateOfRegistration, Date).AddYears(-CType(udtEHSPersonalInformation.ECAge, Integer))
                            '    strExactDOB = "Y"
                            'Else
                            '    dtmDOB = udtEHSPersonalInformation.DOB
                            '    strExactDOB = udtEHSPersonalInformation.ExactDOB
                            'End If

                            ' Check Eligibility
                            'If udtClaimRulesBLL.CheckEligibilityAny(udtSchemeClaim_ValidPeriod_EHAPP, dtmDOB, strExactDOB, dtmServiceDate, udtEHSPersonalInformation.Gender).IsEligible Then
                            If udtClaimRulesBLL.CheckEligibilityAny(udtSchemeClaim_ValidPeriod_EHAPP, udtEHSPersonalInformation, dtmServiceDate).IsEligible Then
                                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                                ' Eligible Case
                                For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtSchemeClaim_ValidPeriod_EHAPP.SubsidizeGroupClaimList

                                    If Not notAvailableForClaim Then
                                        ' Subsidies is Available
                                        Exit For
                                    End If

                                    ' Future subsidize group claim is filtered
                                    If udtSubsidizeGroupClaim.ClaimPeriodFrom > dtmServiceDate Then
                                        ' Subsidies is not Available
                                        Continue For
                                    End If

                                    '
                                    udtSubsidizeItemDetailList = udtSchemeDetailBLL.getSubsidizeItemDetails(udtSubsidizeGroupClaim.SubsidizeItemCode)
                                    For Each udtSubsidizeItemDetail As SubsidizeItemDetailsModel In udtSubsidizeItemDetailList

                                        intNumSubsidize_Total = udtSubsidizeItemDetail.AvailableItemNum

                                        intNumSubsidize_Used = 0
                                        udtTransDetailBenefitListByAvailItem = udtTransDetailBenefitList.FilterBySubsidizeItemDetail(udtSchemeClaim_ValidPeriod_EHAPP.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, udtSubsidizeGroupClaim.SubsidizeItemCode, udtSubsidizeItemDetail.AvailableItemCode)
                                        For Each udtTransDetailBenefitByAvailItem As TransactionDetailModel In udtTransDetailBenefitListByAvailItem
                                            intNumSubsidize_Used += udtTransDetailBenefitByAvailItem.Unit.Value
                                        Next

                                        If intNumSubsidize_Total > intNumSubsidize_Used Then
                                            ' Subsidies is Available
                                            notAvailableForClaim = False
                                            isEligibleForClaim = True
                                            Exit For
                                        Else
                                            ' No Available Subsidies
                                        End If

                                    Next

                                Next

                            Else

                                ' Not Eligible Case
                                notAvailableForClaim = True
                                isEligibleForClaim = False

                            End If

                        End If
                        ' CRE13-001 - EHAPP [End][Tommy L]

                        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                    Case SchemeClaimModel.EnumControlType.SSSCMC
                        Dim blnValid As Boolean = True
                        Dim udtPersonalInformation As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)

                        ' Check Patient whether is on list
                        If (New ClaimRulesBLL).CheckIsHAPatient(udtSchemeClaim.SchemeCode, udtEHSAccount.SearchDocCode, udtPersonalInformation.IdentityNum) <> String.Empty Then
                            blnValid = False
                        End If

                        ' Check Patient whether has available subsidy
                        If Not _udtEHSTransactionBLL.getAvailableSubsidizeItem_SSSCMC(udtPersonalInformation, udtSchemeClaim.SubsidizeGroupClaimList) > 0 Then
                            blnValid = False
                        End If

                        If blnValid Then
                            notAvailableForClaim = False

                            'Sub-Patient Type
                            Dim dtHAPatient As DataTable = (New HAServicePatientBLL).getHAServicePatientByIdentityNum(udtPersonalInformation.DocCode, udtPersonalInformation.IdentityNum)

                            If dtHAPatient.Rows.Count = 0 Then
                                Throw New Exception(String.Format("Document No.({0}) of Document type({1}) is not found in DB table HAServicePatient.", _
                                                                  udtPersonalInformation.IdentityNum, _
                                                                  udtPersonalInformation.DocCode))
                            End If

                            Me._udtSessionHandler.HAPatientSaveToSession(dtHAPatient)

                        End If

                        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

                End Select

                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
            End If

            '' CRE20-0023 (Immu record) [Start][Chris YIM]
            '' ---------------------------------------------------------------------------------------------------------
            '' Temporary handling to block document (Passport, One-way Permit) to claim COVID19 under VSS scheme
            'If udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.VSS AndAlso Me.ClaimMode = ClaimMode.COVID19 Then
            '    If udtEHSAccount.SearchDocCode IsNot Nothing AndAlso _
            '        (udtEHSAccount.SearchDocCode = DocTypeModel.DocTypeCode.OW OrElse udtEHSAccount.SearchDocCode = DocTypeModel.DocTypeCode.PASS) Then
            '        If udtLatestC19Vaccine Is Nothing Then
            '            notAvailableForClaim = True
            '            isEligibleForClaim = False
            '        End If
            '    End If
            'End If
            '' CRE20-0023 (Immu record) [End][Chris YIM]

            If blnInClaimPeriod Then

                If notAvailableForClaim Then

                    If isEligibleForClaim Then
                        'Me._udtSystemMessage = New SystemMessage("990000", "I", "00019")
                        'Me.udcMsgBoxInfo.AddMessage(New SystemMessage("990000", "E", "00019"))

                        ' 2010-05-08 Fix
                        Dim udtVaccinationBLL As New VaccinationBLL

                        ' CRE13-001 - EHAPP [Start][Koala]
                        ' -------------------------------------------------------------------------------------
                        If udtVaccinationBLL.SchemeContainVaccine(udtSchemeClaim) Then
                            If Me.ClaimMode = Common.Component.ClaimMode.COVID19 Then
                                Me._udtSystemMessage = New SystemMessage("990000", "E", "00474")
                            Else
                                Me._udtSystemMessage = New SystemMessage("990000", "E", "00255")
                            End If
                        Else
                            ' CRE19-003 (Opt voucher capping) [Start][Winnie]
                            ' ----------------------------------------------------------------------------------------
                            If blnWithoutConversionRate = False AndAlso blnNoAvailableQuota = False Then
                                ' CRE19-003 (Opt voucher capping) [End][Winnie]
                                Me._udtSystemMessage = New SystemMessage("990000", "E", "00107")
                            End If
                        End If
                        ' CRE13-001 - EHAPP [End][Koala]
                    Else
                        Me._udtSystemMessage = New SystemMessage("990000", "E", "00106")
                        'Me.udcMsgBoxInfo.AddMessage(New SystemMessage("990000", "E", "00106"))
                    End If

                    ' 2010-05-08 Fix
                    ' Display Error Box instead of Info Box for no available subsidy or not eligible
                    ' The Error Message Box build in after step
                    Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                    'Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail)

                    'Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Information
                    'Me.udcMsgBoxInfo.AddMessage(Me._udtSystemMessage)
                    'Me.udcMsgBoxInfo.BuildMessageBox()

                    'disable proceed claim control
                    Me.SetClaimButtonEnable(Me.btnStep2aClaim, False)

                    Me.udcStep2aInputEHSClaim.AvaliableForClaim = False
                Else
                    'Me.udcMsgBoxErr.Clear()
                    'Me.udcMsgBoxInfo.Clear()
                    'enable proceed claim control
                    Me.SetClaimButtonEnable(Me.btnStep2aClaim, True)

                    Me.udcStep2aInputEHSClaim.AvaliableForClaim = True

                End If

            Else
                ' CRE13-001 - EHAPP [Start][Koala]
                ' -------------------------------------------------------------------------------------
                Me._udtSystemMessage = New SystemMessage("990000", "E", "00243")
                Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)

                Me.SetClaimButtonEnable(Me.btnStep2aClaim, False)
                Me.udcStep2aInputEHSClaim.AvaliableForClaim = False
                ' CRE13-001 - EHAPP [End][Koala]
            End If

            '----------------------------------------------------------
            ' Find the latest EHS transaction for carry forward
            '----------------------------------------------------------
            Dim udtEHSTransactionLatestVaccine As EHSTransactionModel = Nothing
            Dim udtTranDetailLatestVaccine As TransactionDetailVaccineModel = Nothing

            If (udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.VSS OrElse _
                udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.RVP OrElse _
                udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.COVID19 OrElse _
                udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.COVID19RVP OrElse _
                udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.COVID19OR) _
                AndAlso ClaimMode = Common.Component.ClaimMode.COVID19 Then

                If udtLatestC19Vaccine IsNot Nothing Then
                    udtTranDetailLatestVaccine = New TransactionDetailVaccineModel(udtLatestC19Vaccine)

                    If udtLatestC19Transaction IsNot Nothing Then
                        udtEHSTransactionLatestVaccine = udtLatestC19Transaction
                    End If
                End If
            End If

            '----------------------------------------------------------
            ' Build input control
            '----------------------------------------------------------
            Me.udcStep2aInputEHSClaim.SetRebuildRequired()
            Me.udcStep2aInputEHSClaim.CurrentPractice = Me._udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)
            Me.udcStep2aInputEHSClaim.SchemeType = udtSchemeClaim.SchemeCode.Trim()
            Me.udcStep2aInputEHSClaim.EHSAccount = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
            Me.udcStep2aInputEHSClaim.EHSTransaction = Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)
            Me.udcStep2aInputEHSClaim.EHSTransactionLatestVaccineRecord = udtEHSTransactionLatestVaccine
            Me.udcStep2aInputEHSClaim.TranDetailLatestVaccineRecord = udtTranDetailLatestVaccine
            Me.udcStep2aInputEHSClaim.TableTitleWidth = 205
            Me.udcStep2aInputEHSClaim.ServiceDate = dtmServiceDate
            Me.udcStep2aInputEHSClaim.NonClinic = Me._udtSessionHandler.NonClinicSettingGetFromSession(FunctCode)
            Me.udcStep2aInputEHSClaim.ClaimMode = Me.ClaimMode
            Me.udcStep2aInputEHSClaim.Built()

            Select Case udtSchemeClaim.ControlType
                ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
                Case SchemeClaimModel.EnumControlType.HSIVSS, SchemeClaimModel.EnumControlType.RVP, SchemeClaimModel.EnumControlType.VSS, _
                    SchemeClaimModel.EnumControlType.ENHVSSO, SchemeClaimModel.EnumControlType.PPP, SchemeClaimModel.EnumControlType.COVID19, _
                    SchemeClaimModel.EnumControlType.COVID19RVP, SchemeClaimModel.EnumControlType.COVID19OR
                    ' CRE20-0022 (Immu record) [End][Winnie SUEN]

                    ' CRE20-0022 (Immu record) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                    If Me.ClaimMode = ClaimMode.COVID19 Then
                        Me.SetClaimButtonEnable(Me.btnStep2aClaim, False)

                        If notAvailableForClaim OrElse noCategorys OrElse Not blnInClaimPeriod OrElse Not blnCOVID19ForClaim Then
                            Me.chkStep2aDeclareClaim.Enabled = False
                        Else
                            Me.chkStep2aDeclareClaim.Enabled = True
                        End If

                        'If udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.RVP Then
                        '    panStep2aDeclareJoineHRSS.Visible = False
                        'Else
                        panStep2aDeclareJoineHRSS.Visible = False

                        If udtEHSAccount.SearchDocCode IsNot Nothing Then
                            Dim intAge As Integer

                            If Not Integer.TryParse(_udtGeneralFunction.GetSystemParameterParmValue1("AgeLimitForJoinEHRSS"), intAge) Then
                                Throw New Exception(String.Format("Invalid value({0}) is not a integer in DB table SystemParameter(AgeLimitForJoinEHRSS).", intAge))
                            End If

                            If Not CompareEligibleRuleByAge(dtmServiceDate, udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode), _
                                                            intAge, "<", "Y", "DAY3") Then

                                Select Case udtEHSAccount.SearchDocCode
                                    Case DocTypeModel.DocTypeCode.HKIC, DocTypeModel.DocTypeCode.EC, DocTypeModel.DocTypeCode.OW
                                        panStep2aDeclareJoineHRSS.Visible = True
                                        chkStep2aDeclareJoineHRSS.Enabled = True
                                        trStep2aDeclareJoineHRSS.Style.Remove("display")

                                        'Scheme & recipient option
                                        If udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.COVID19RVP OrElse _
                                           udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.RVP Then
                                            trStep2aDeclareJoineHRSS.Style.Add("display", "none")

                                            'COVID19RVP
                                            Dim udcInputCOVID19RVP As ucInputCOVID19RVP = Me.udcStep2aInputEHSClaim.GetCOVID19RVPControl()

                                            If udcInputCOVID19RVP IsNot Nothing Then
                                                If udcInputCOVID19RVP.RecipientType IsNot Nothing AndAlso _
                                                    udcInputCOVID19RVP.RecipientType <> RECIPIENT_TYPE.RESIDENT AndAlso _
                                                    udcInputCOVID19RVP.RecipientType <> String.Empty Then

                                                    trStep2aDeclareJoineHRSS.Style.Remove("display")
                                                End If
                                            End If

                                            'RVPCOVID19
                                            Dim udcInputRVPCOVID19 As ucInputRVPCOVID19 = Me.udcStep2aInputEHSClaim.GetRVPCOVID19Control()

                                            If udcInputRVPCOVID19 IsNot Nothing Then
                                                If udcInputRVPCOVID19.TypeOfOutreach = TYPE_OF_OUTREACH.RCH Then
                                                    If udcInputRVPCOVID19.RecipientType IsNot Nothing AndAlso _
                                                        udcInputRVPCOVID19.RecipientType <> RECIPIENT_TYPE.RESIDENT AndAlso _
                                                        udcInputRVPCOVID19.RecipientType <> String.Empty Then

                                                        trStep2aDeclareJoineHRSS.Style.Remove("display")
                                                    End If

                                                Else
                                                    trStep2aDeclareJoineHRSS.Style.Remove("display")

                                                End If

                                            End If
                                        End If

                                        'Carry Forward: Join eHealth
                                        If udtTranDetailLatestVaccine IsNot Nothing Then
                                            chkStep2aDeclareJoineHRSS.Enabled = False
                                            If udtEHSTransactionLatestVaccine IsNot Nothing Then

                                                If udtEHSTransactionLatestVaccine.TransactionAdditionFields IsNot Nothing AndAlso _
                                                   udtEHSTransactionLatestVaccine.TransactionAdditionFields.JoinEHRSS IsNot Nothing Then

                                                    Select Case udtEHSTransactionLatestVaccine.SchemeCode.Trim.ToUpper
                                                        Case SchemeClaimModel.COVID19CVC, SchemeClaimModel.COVID19DH, SchemeClaimModel.COVID19RVP, _
                                                             SchemeClaimModel.COVID19OR, SchemeClaimModel.COVID19SR, SchemeClaimModel.VSS, SchemeClaimModel.RVP

                                                            If udtEHSTransactionLatestVaccine.TransactionAdditionFields.JoinEHRSS = YesNo.Yes Then
                                                                panStep2aDeclareJoineHRSS.Visible = False
                                                                'If _udtSessionHandler.ClaimCOVID19CarryForwordGetFromSession(FunctCode) = False Then
                                                                '    chkStep2aDeclareJoineHRSS.Checked = True
                                                                'End If
                                                            Else
                                                                chkStep2aDeclareJoineHRSS.Enabled = True
                                                                'chkStep2aDeclareJoineHRSS.Checked = False

                                                            End If

                                                        Case Else
                                                            chkStep2aDeclareJoineHRSS.Enabled = True
                                                            'chkStep2aDeclareJoineHRSS.Checked = False
                                                    End Select

                                                Else
                                                    chkStep2aDeclareJoineHRSS.Enabled = True
                                                    'chkStep2aDeclareJoineHRSS.Checked = False

                                                End If
                                            Else
                                                chkStep2aDeclareJoineHRSS.Enabled = True
                                                'chkStep2aDeclareJoineHRSS.Checked = False

                                            End If
                                        End If

                                    Case Else
                                        'Nothing to do
                                End Select

                            End If

                        End If

                        'End If

                        If Not blnCOVID19ForClaim Then
                            udcMsgBoxInfo.AddMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00049)
                            Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Information
                            Me.udcMsgBoxInfo.BuildMessageBox()
                        End If

                    Else
                        If udtClaimCategory Is Nothing OrElse notAvailableForClaim OrElse noCategorys OrElse Not blnInClaimPeriod Then
                            Me.SetClaimButtonEnable(Me.btnStep2aClaim, False)
                        Else
                            Me.SetClaimButtonEnable(Me.btnStep2aClaim, True)
                        End If

                    End If
                    ' CRE20-0022 (Immu record) [End][Chris YIM]
            End Select

            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            If Me.ClaimMode = ClaimMode.COVID19 Then
                panStep2aVaccinationRecord.Visible = True
                panStep2aRecipinetContactInfo.Visible = True
                panStep2aPrintClaimConsentForm.Visible = False
                panStep2aDeclareClaim.Visible = True

                'Bind the vaccination record table in enter claim page
                Dim dtVaccineRecord As DataTable = TransactionDetailListToCOVID19DataTable(GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode))
                BuildCOVID19VaccinationRecordGrid(dtVaccineRecord)

                '----------------------------------------------------------
                'Generate JavaScript Function
                '----------------------------------------------------------
                'Category & Sub-Category
                Dim strCategoryFunctionJavaScript As String = String.Empty

                strCategoryFunctionJavaScript = (New COVID19.COVID19BLL).GenerateCategoryJavaScript(Session("language"))

                ScriptManager.RegisterStartupScript(Me, Page.GetType(), "COVID19_Category_Function", strCategoryFunctionJavaScript, True)

                'Vaccine Brand & Lot No.
                Dim strVaccineLotNoFunctionJavaScript As String = (New COVID19.COVID19BLL).GenerateVaccineLotNoJavaScript()

                ScriptManager.RegisterStartupScript(Me, Page.GetType(), "COVID19_Vaccine_LotNo_Function", strVaccineLotNoFunctionJavaScript, True)

                'Generate JavaScript Declaimer checkbox
                BuildbtnStep2aClaimEventForCOVID19()

                'Generate JavaScript for recipient Type
                Dim strRecipientTypeJavaScript As String = (New COVID19.COVID19BLL).GenerateRecipientTypeJavaScript()

                ScriptManager.RegisterStartupScript(Me, Page.GetType(), "COVID19_Recipient_Function", strRecipientTypeJavaScript, True)

                '----------------------------------------------------------

                If _udtSessionHandler.ClaimCOVID19SchemeSelectedGetFromSession(FunctionCode) IsNot Nothing Then
                    Dim udtSelectedSchemeClaim As SchemeClaimModel = _udtSessionHandler.ClaimCOVID19SchemeSelectedGetFromSession(FunctionCode)
                    Select Case udtSelectedSchemeClaim.ControlType
                        Case SchemeClaimModel.EnumControlType.VSS
                            If Me.ClaimMode = ClaimMode.COVID19 Then
                                Dim udcInputVSSCOVID19 As ucInputVSSCOVID19 = Me.udcStep2aInputEHSClaim.GetVSSCOVID19Control()

                                If udcInputVSSCOVID19 IsNot Nothing Then
                                    '_udtSessionHandler.ClaimCOVID19CategorySaveToSession(udcInputVSSCOVID19.CategoryForCOVID19, FunctionCode)
                                    _udtSessionHandler.ClaimCOVID19VaccineBrandSaveToSession(udcInputVSSCOVID19.VaccineBrand, FunctionCode)
                                    _udtSessionHandler.ClaimCOVID19VaccineLotNoSaveToSession(udcInputVSSCOVID19.VaccineLotNo, FunctionCode)
                                End If
                            End If

                        Case SchemeClaimModel.EnumControlType.COVID19
                            Dim udcInputCOVID19 As ucInputCOVID19 = Me.udcStep2aInputEHSClaim.GetCOVID19Control()

                            If udcInputCOVID19 IsNot Nothing Then
                                '_udtSessionHandler.ClaimCOVID19CategorySaveToSession(udcInputCOVID19.CategoryForCOVID19, FunctionCode)
                                _udtSessionHandler.ClaimCOVID19VaccineBrandSaveToSession(udcInputCOVID19.VaccineBrand, FunctionCode)
                                _udtSessionHandler.ClaimCOVID19VaccineLotNoSaveToSession(udcInputCOVID19.VaccineLotNo, FunctionCode)
                            End If

                        Case SchemeClaimModel.EnumControlType.COVID19RVP
                            Dim udcInputCOVID19RVP As ucInputCOVID19RVP = Me.udcStep2aInputEHSClaim.GetCOVID19RVPControl()

                            If udcInputCOVID19RVP IsNot Nothing Then
                                '_udtSessionHandler.ClaimCOVID19CategorySaveToSession(udcInputCOVID19.CategoryForCOVID19, FunctionCode)
                                _udtSessionHandler.ClaimCOVID19VaccineBrandSaveToSession(udcInputCOVID19RVP.VaccineBrand, FunctionCode)
                                _udtSessionHandler.ClaimCOVID19VaccineLotNoSaveToSession(udcInputCOVID19RVP.VaccineLotNo, FunctionCode)
                            End If

                        Case SchemeClaimModel.EnumControlType.RVP
                            If Me.ClaimMode = ClaimMode.COVID19 Then
                                Dim udcInputCOVID19RVP As ucInputRVPCOVID19 = Me.udcStep2aInputEHSClaim.GetRVPCOVID19Control()

                                If udcInputCOVID19RVP IsNot Nothing Then
                                    '_udtSessionHandler.ClaimCOVID19CategorySaveToSession(udcInputCOVID19.CategoryForCOVID19, FunctionCode)
                                    _udtSessionHandler.ClaimCOVID19VaccineBrandSaveToSession(udcInputCOVID19RVP.VaccineBrand, FunctionCode)
                                    _udtSessionHandler.ClaimCOVID19VaccineLotNoSaveToSession(udcInputCOVID19RVP.VaccineLotNo, FunctionCode)
                                End If
                            End If


                        Case SchemeClaimModel.EnumControlType.COVID19OR
                            Dim udcInputCOVID19OR As ucInputCOVID19OR = Me.udcStep2aInputEHSClaim.GetCOVID19ORControl()

                            If udcInputCOVID19OR IsNot Nothing Then
                                '_udtSessionHandler.ClaimCOVID19CategorySaveToSession(udcInputCOVID19.CategoryForCOVID19, FunctionCode)
                                _udtSessionHandler.ClaimCOVID19VaccineBrandSaveToSession(udcInputCOVID19OR.VaccineBrand, FunctionCode)
                                _udtSessionHandler.ClaimCOVID19VaccineLotNoSaveToSession(udcInputCOVID19OR.VaccineLotNo, FunctionCode)
                            End If

                    End Select
                End If

                'Contact No.
                If udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.VSS OrElse _
                   udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.RVP OrElse _
                   udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.COVID19OR OrElse _
                   udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.COVID19RVP Then

                    trStep2aContactNo.Visible = True
                    trStep2aContactNo.Style.Remove("display")
                    lblStep2aContactNoRecommendation.Visible = True
                    txtStep2aContactNo.Enabled = True

                    If udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.COVID19RVP OrElse _
                       udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.RVP Then

                        trStep2aContactNo.Style.Add("display", "none")

                        Dim udcInputCOVID19RVP As ucInputCOVID19RVP = Me.udcStep2aInputEHSClaim.GetCOVID19RVPControl()

                        If udcInputCOVID19RVP IsNot Nothing Then
                            If udcInputCOVID19RVP.RecipientType IsNot Nothing AndAlso _
                                udcInputCOVID19RVP.RecipientType <> RECIPIENT_TYPE.RESIDENT AndAlso _
                                udcInputCOVID19RVP.RecipientType <> String.Empty Then

                                trStep2aContactNo.Style.Remove("display")
                            End If
                        End If

                        Dim udcInputRVPCOVID19 As ucInputRVPCOVID19 = Me.udcStep2aInputEHSClaim.GetRVPCOVID19Control()

                        If udcInputRVPCOVID19 IsNot Nothing Then
                            If udcInputRVPCOVID19.TypeOfOutreach = TYPE_OF_OUTREACH.RCH Then
                                If udcInputRVPCOVID19.RecipientType IsNot Nothing AndAlso _
                                    udcInputRVPCOVID19.RecipientType <> RECIPIENT_TYPE.RESIDENT AndAlso _
                                    udcInputRVPCOVID19.RecipientType <> String.Empty Then

                                    trStep2aContactNo.Style.Remove("display")
                                End If

                            Else
                                trStep2aContactNo.Style.Remove("display")

                            End If

                        End If

                    End If

                    'Carry Forward: Contact no.
                    If udtTranDetailLatestVaccine IsNot Nothing Then
                        txtStep2aContactNo.Enabled = False
                        If udtEHSTransactionLatestVaccine IsNot Nothing Then
                            If udtEHSTransactionLatestVaccine.TransactionAdditionFields IsNot Nothing AndAlso _
                                udtEHSTransactionLatestVaccine.TransactionAdditionFields.ContactNo IsNot Nothing AndAlso _
                                udtEHSTransactionLatestVaccine.TransactionAdditionFields.ContactNo <> String.Empty Then

                                Select Case udtEHSTransactionLatestVaccine.SchemeCode.Trim.ToUpper
                                    Case SchemeClaimModel.VSS, SchemeClaimModel.RVP, SchemeClaimModel.COVID19OR, SchemeClaimModel.COVID19RVP
                                        If _udtSessionHandler.ClaimCOVID19CarryForwordGetFromSession(FunctCode) = False Then
                                            txtStep2aContactNo.Text = udtEHSTransactionLatestVaccine.TransactionAdditionFields.ContactNo
                                        End If
                                    Case Else
                                        txtStep2aContactNo.Enabled = True

                                End Select

                            Else
                                txtStep2aContactNo.Enabled = True

                            End If
                        Else
                            txtStep2aContactNo.Enabled = True

                        End If
                    End If

                Else
                    trStep2aContactNo.Visible = False
                    lblStep2aContactNoRecommendation.Visible = False
                    txtStep2aContactNo.Text = String.Empty
                    chkStep2aMobile.Checked = False
                End If

            Else
                panStep2aVaccinationRecord.Visible = False
                panStep2aRecipinetContactInfo.Visible = False
                panStep2aPrintClaimConsentForm.Visible = False
                panStep2aDeclareClaim.Visible = False

                CheckValidHKICInScheme(udtSchemeClaim.SchemeCode)

            End If

            ' CRE20-0022 (Immu record) [End][Chris YIM]

        End If

    End Sub

    Private Sub Step2aClear()
        Me.imgStep2aServiceDateError.Visible = False
        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Me.imgStep2aContactNoError.Visible = False
        Me.txtStep2aContactNo.Text = String.Empty
        Me.imgStep2aDeclareClaimError.Visible = False
        Me.chkStep2aDeclareClaim.Checked = False
        Me.chkStep2aDeclareJoineHRSS.Checked = False
        Me.chkStep2aMobile.Checked = False
        Me.txtStep2aRemark.Text = String.Empty
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        Me.udcStep2aInputEHSClaim.Clear()
        Me.udcStep2aReadOnlyDocumnetType.Clear()
    End Sub

    ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [Start][Winnie]
    Protected Sub Step2aClaimSubmit(ByVal blnIsConfirmed As Boolean)
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
        Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Dim udtPracticeDisplay As BLL.PracticeDisplayModel = Me._udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)
        Dim udtFormatter As Formatter = New Formatter()
        Dim udtValidator As Validator = New Validator()
        Dim isValid As Boolean = True
        Dim strServiceDate As String = String.Empty
        Dim udtDataEntryUser As DataEntryUserModel = Nothing
        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(FunctCode, Me)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = Nothing
        Dim isTSWCase As Boolean = False

        EHSClaimBasePage.AuditLogEnterClaimDetailStart(udtAuditLogEntry, blnIsConfirmed, Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode))

        ' To Handle Concurrent Browser
        If Not Me.EHSClaimTokenNumValidation() Then
            Return
        End If

        'Reset Error Image
        Me.imgStep2aServiceDateError.Visible = False
        Me.ModalPopupExclamationConfirmationBox.Hide()

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        If Me.ClaimMode = ClaimMode.COVID19 Then
            Me.imgStep2aContactNoError.Visible = False
            Me.imgStep2aDeclareClaimError.Visible = False

            Me.Step2aCleanSchemeErrorImage()
        End If
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        Me._udtSessionHandler.CurrentUserGetFromSession(Me._udtSP, udtDataEntryUser)

        '------------------------------------------------ Check Service Date ---------------------------------------------------

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtSubPlatformBLL As New SubPlatformBLL
        'strServiceDate = udtFormatter.formatInputDate(Me.txtStep2aServiceDate.Text)
        strServiceDate = udtFormatter.formatInputDate(Me.txtStep2aServiceDate.Text, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        Me._udtSystemMessage = udtValidator.chkServiceDate(strServiceDate)

        If Not Me._udtSystemMessage Is Nothing Then
            isValid = False
            Me.imgStep2aServiceDateError.Visible = True
            Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
        Else
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'Me.txtStep2aServiceDate.Text = strServiceDate
            Me.txtStep2aServiceDate.Text = udtFormatter.formatInputTextDate(strServiceDate, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
        End If

        If Not blnIsConfirmed Then
            '-------------------------------------- Check Service Date VS Permit to Remain ---------------------------------------
            If isValid Then
                udtEHSPersonalInfo = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode.Trim())
                If udtEHSPersonalInfo.DocCode = DocTypeModel.DocTypeCode.ID235B AndAlso udtEHSPersonalInfo.PermitToRemainUntil.HasValue Then
                    Me._udtSystemMessage = udtValidator.ChkServiceDatePermitToRemainUntil(udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English), udtEHSPersonalInfo.PermitToRemainUntil.Value)
                    If Not Me._udtSystemMessage Is Nothing Then
                        isValid = False
                        Me.imgStep2aServiceDateError.Visible = True
                        Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                    End If
                End If
            End If

            If isValid Then
                Dim strAllowDateBack As String = String.Empty
                Dim strClaimDayLimit As String = String.Empty
                Dim strMinDate As String = String.Empty
                Dim intDayLimit As Integer
                Dim dtmMinDate As DateTime

                Me._udtGeneralFunction.getSystemParameter("DateBackClaimAllow", strAllowDateBack, String.Empty, udtSchemeClaim.SchemeCode)
                If strAllowDateBack = "Y" Then

                    Me._udtGeneralFunction.getSystemParameter("DateBackClaimDayLimit", strClaimDayLimit, String.Empty, udtSchemeClaim.SchemeCode)
                    Me._udtGeneralFunction.getSystemParameter("DateBackClaimMinDate", strMinDate, String.Empty, udtSchemeClaim.SchemeCode)

                    intDayLimit = CInt(strClaimDayLimit)
                    dtmMinDate = Convert.ToDateTime(strMinDate)

                    ' To Do: ServiceDate should not before SP.SchemeInformation

                    Dim udtSchemeClaimBLL As New SchemeClaimBLL()
                    Dim strSchemeCodeEnrol As String = udtSchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(udtSchemeClaim.SchemeCode)
                    Dim udtSchemeInfoModel As Common.Component.SchemeInformation.SchemeInformationModel = Me._udtSP.SchemeInfoList().Filter(strSchemeCodeEnrol)


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

                    ' ServiceDate should not before SP Scheme Enrol / Effective date
                    If Not udtSchemeInfoModel Is Nothing AndAlso udtSchemeInfoModel.EffectiveDtm.HasValue AndAlso udtSchemeInfoModel.EffectiveDtm.Value > dtmMinDate Then
                        dtmMinDate = udtSchemeInfoModel.EffectiveDtm.Value.Date
                    End If

                    Me._udtSystemMessage = udtValidator.chkDateBackClaimServiceDate(strServiceDate, intDayLimit, dtmMinDate)

                    If Not Me._udtSystemMessage Is Nothing Then
                        isValid = False
                        Me.imgStep2aServiceDateError.Visible = True

                        If Me._udtSystemMessage.MessageCode = "00149" Then
                            Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage, "%s", strClaimDayLimit)
                        ElseIf Me._udtSystemMessage.MessageCode = "00150" Then
                            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                            '-----------------------------------------------------------------------------------------
                            'Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage, "%s", udtFormatter.formatDate(dtmMinDate, Me._udtSessionHandler.Language))
                            Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage, "%s", udtFormatter.formatDisplayDate(dtmMinDate, Me._udtSessionHandler.Language))
                            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
                        Else
                            Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                        End If
                    End If
                End If
            End If
            '-------------------------------------------------------------------------------------------------------------------------
        End If

        If isValid Then

            ' INT13-0010 - HCSP back date claim previous year voucher problem [Start][Koala]
            ' -------------------------------------------------------------------------------------
            udtSchemeClaim = Me._udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtSchemeClaim.SchemeCode, udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English))
            Me._udtSessionHandler.SchemeSelectedSaveToSession(udtSchemeClaim, FunctCode)
            ' INT13-0010 - HCSP back date claim previous year voucher problem [End][Koala]

            'Construct EHS Transaction
            Me._udtEHSTransaction = Me._udtEHSClaimBLL.ConstructNewEHSTransaction(udtSchemeClaim, _
                                                                                  udtEHSAccount, _
                                                                                  udtPracticeDisplay, _
                                                                                  udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English), _
                                                                                  GetExtRefStatus(udtEHSAccount, udtSchemeClaim), _
                                                                                  GetDHVaccineRefStatus(udtEHSAccount, udtSchemeClaim))

            Select Case udtSchemeClaim.ControlType
                Case SchemeClaimModel.EnumControlType.VOUCHER
                    isValid = Me.Step2aHCVSValidation(blnIsConfirmed, Me._udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.VOUCHERCHINA
                    isValid = Me.Step2aHCVSChinaValidation(blnIsConfirmed, Me._udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.EVSS
                    isValid = Me.Step2aEVSSValidation(blnIsConfirmed, Me._udtEHSTransaction.ServiceDate)

                Case SchemeClaimModel.EnumControlType.CIVSS
                    isValid = Me.Step2aCIVSSValidation(blnIsConfirmed, Me._udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.RVP
                    If Me.ClaimMode = Common.Component.ClaimMode.COVID19 Then
                        isValid = Me.Step2aRVPCOVID19Validation(blnIsConfirmed, Me._udtEHSTransaction)
                    Else
                        isValid = Me.Step2aRVPValidation(blnIsConfirmed, Me._udtEHSTransaction)
                    End If

                Case SchemeClaimModel.EnumControlType.HSIVSS
                    isValid = Me.Step2aHSIVSSValidation(blnIsConfirmed, Me._udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.EHAPP
                    isValid = Me.Step2aEHAPPValidation(Me._udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.PIDVSS
                    isValid = Me.Step2aPIDVSSValidation(blnIsConfirmed, Me._udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.VSS
                    If Me.ClaimMode = Common.Component.ClaimMode.COVID19 Then
                        isValid = Me.Step2aVSSCOVID19Validation(blnIsConfirmed, Me._udtEHSTransaction)
                    Else
                        isValid = Me.Step2aVSSValidation(blnIsConfirmed, Me._udtEHSTransaction)
                    End If

                    ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
                    ' --------------------------------------------------------------------------------------
                Case SchemeClaimModel.EnumControlType.COVID19
                    isValid = Me.Step2aCOVID19Validation(blnIsConfirmed, Me._udtEHSTransaction)
                    ' CRE20-0022 (Immu record) [End][Winnie SUEN]

                Case SchemeClaimModel.EnumControlType.COVID19RVP
                    isValid = Me.Step2aCOVID19RVPValidation(blnIsConfirmed, Me._udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.COVID19OR
                    isValid = Me.Step2aCOVID19ORValidation(blnIsConfirmed, Me._udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.ENHVSSO
                    isValid = Me.Step2aENHVSSOValidation(blnIsConfirmed, Me._udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.PPP
                    isValid = Me.Step2aPPPValidation(blnIsConfirmed, Me._udtEHSTransaction)

                    ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                Case SchemeClaimModel.EnumControlType.SSSCMC
                    isValid = Me.Step2aSSSCMCValidation(blnIsConfirmed, Me._udtEHSTransaction)
                    ' CRE20-015 (Special Support Scheme) [End][Chris YIM]


                Case Else
                    Throw New Exception(String.Format("No available input control for scheme({0}).", udtSchemeClaim.ControlType.ToString))

            End Select

        End If

        If isValid Then
            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            If Common.OCSSS.OCSSSServiceBLL.EnableHKICSymbolInput(udtSchemeClaim.SchemeCode) Then
                Me._udtEHSTransaction.HKICSymbol = _udtSessionHandler.HKICSymbolGetFormSession(FunctCode)
                Me._udtEHSTransaction.OCSSSRefStatus = _udtSessionHandler.OCSSSRefStatusGetFormSession(FunctCode)
            Else
                Me._udtEHSTransaction.HKICSymbol = String.Empty
                Me._udtEHSTransaction.OCSSSRefStatus = String.Empty
            End If
            ' CRE17-010 (OCSSS integration) [End][Chris YIM]

            If udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.VOUCHER OrElse udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.VOUCHERCHINA Then
                Me._udtEHSTransaction.ServiceDate = udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English)
                Me._udtEHSClaimBLL.ConstructEHSTransactionDetails(Me._udtSP, udtDataEntryUser, Me._udtEHSTransaction, udtEHSAccount)

            ElseIf udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.EHAPP Then
                Me._udtEHSClaimBLL.ConstructEHSTransDetail_Registration(Me._udtSP, udtDataEntryUser, Me._udtEHSTransaction, udtEHSAccount)

                ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
            ElseIf udtSchemeClaim.ControlType = SchemeClaimModel.EnumControlType.SSSCMC Then
                Me._udtEHSClaimBLL.ConstructEHSTransactionDetail_SSSCMC(Me._udtSP, udtDataEntryUser, Me._udtEHSTransaction, udtEHSAccount, Me._udtSessionHandler.HAPatientGetFromSession())
                ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

            Else
                Dim dicVaccineRef As Dictionary(Of String, String) = EHSTransactionModel.GetVaccineRef(GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), Me._udtEHSTransaction)
                Me._udtEHSTransaction.EHSVaccineResult = dicVaccineRef(EHSTransactionModel.VaccineRefType.EHS)
                Me._udtEHSTransaction.HAVaccineResult = dicVaccineRef(EHSTransactionModel.VaccineRefType.HA)
                Me._udtEHSTransaction.DHVaccineResult = dicVaccineRef(EHSTransactionModel.VaccineRefType.DH)

                Me._udtEHSClaimBLL.ConstructEHSTransactionDetails(Me._udtSP, udtDataEntryUser, Me._udtEHSTransaction, udtEHSAccount, Me._udtSessionHandler.EHSClaimVaccineGetFromSession())

            End If


            Me._udtSessionHandler.EHSTransactionSaveToSession(Me._udtEHSTransaction, FunctCode)


            ' --------------------------------------------------------------------------------------------
            ' TSW Checking
            ' --------------------------------------------------------------------------------------------
            If udtSchemeClaim.TSWCheckingEnable Then
                isTSWCase = Me._udtEHSClaimBLL.chkIsTSWCase(Me._udtSP.SPID, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).IdentityNum)
            End If

            'Log
            If ClaimMode = Common.Component.ClaimMode.COVID19 AndAlso udtSchemeClaim.SchemeCode = SchemeClaimModel.COVID19CVC Then
                EHSClaimBasePage.AddDescriptionCentreBooth(udtAuditLogEntry, Me._udtEHSTransaction.ServiceProviderID, Me._udtEHSTransaction.PracticeID)
            End If

            EHSClaimBasePage.AuditLogEnterClaimDetailPassed(udtAuditLogEntry, Me._udtEHSTransaction, blnIsConfirmed, isTSWCase)

            If Not Me.IsSkipClaimCompletePage Then
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step2b
            Else

                Step2bClaimSubmit()
                ' CRE20-0022 (Immu record) [Start][Martin Tang]
                Me.chkStep2aDeclareClaim.Checked = False
                ' CRE20-0022 (Immu record) [End][Martin Tang]
            End If

        Else
            Me._udtSessionHandler.EHSTransactionRemoveFromSession(FunctCode)

            Dim errorMessageCodeTable As DataTable = Me.udcMsgBoxErr.GetCodeTable
            If errorMessageCodeTable.Rows.Count > 0 Then
                Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail, udtAuditLogEntry, Common.Component.LogID.LOG00013, String.Format("Enter Claim Detail Failed", FunctCode))
            End If
        End If
    End Sub
    ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [End][Winnie]
#End Region

#Region "Step 2a Scheme Error Reset"

    Private Sub Step2aCleanSchemeErrorImage()
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        'Scheme can be nothing, if SP changed practice which is no available for the recipient 
        If Not udtSchemeClaim Is Nothing Then
            Select Case udtSchemeClaim.ControlType
                Case SchemeClaimModel.EnumControlType.VOUCHER
                    Me.Step2aCleanHCVSError()

                Case SchemeClaimModel.EnumControlType.VOUCHERCHINA
                    Me.Step2aCleanHCVSChinaError()

                Case SchemeClaimModel.EnumControlType.CIVSS
                    Me.Step2aCleanCIVSSError()

                Case SchemeClaimModel.EnumControlType.EVSS
                    Me.Step2aCleanEVSSError()

                Case SchemeClaimModel.EnumControlType.HSIVSS
                    Me.Step2aCleanHSIVSSError()

                Case SchemeClaimModel.EnumControlType.RVP
                    If Me.ClaimMode = Common.Component.ClaimMode.COVID19 Then
                        Me.Step2aCleanRVPCOVID19Error()
                    Else
                        Me.Step2aCleanRVPError()
                    End If

                Case SchemeClaimModel.EnumControlType.EHAPP
                    Me.Step2aCleanEHAPPError()

                Case SchemeClaimModel.EnumControlType.PIDVSS
                    Me.Step2aCleanPIDVSSError()

                Case SchemeClaimModel.EnumControlType.VSS
                    If Me.ClaimMode = Common.Component.ClaimMode.COVID19 Then
                        Me.Step2aCleanVSSCOVID19Error()
                    Else
                        Me.Step2aCleanVSSError()
                    End If

                Case SchemeClaimModel.EnumControlType.ENHVSSO
                    Me.Step2aCleanENHVSSOError()

                Case SchemeClaimModel.EnumControlType.PPP
                    Me.Step2aCleanPPPError()

                    ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                Case SchemeClaimModel.EnumControlType.SSSCMC
                    Me.Step2aCleanSSSCMCError()
                    ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

                    ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
                    ' --------------------------------------------------------------------------------------
                Case SchemeClaimModel.EnumControlType.COVID19
                    Me.Step2aCleanCOVID19Error()
                    ' CRE20-0022 (Immu record) [End][Winnie SUEN]

                Case SchemeClaimModel.EnumControlType.COVID19RVP
                    Me.Step2aCleanCOVID19RVPError()

                Case SchemeClaimModel.EnumControlType.COVID19OR
                    Me.Step2aCleanCOVID19ORError()

                Case Else
                    Throw New Exception(String.Format("No available input control for scheme({0}).", udtSchemeClaim.ControlType.ToString))

            End Select


        End If
    End Sub

    Private Sub Step2aCleanHCVSError()
        Dim udcInputHCVS As ucInputHCVS = Me.udcStep2aInputEHSClaim.GetHCVSControl()
        If Not udcInputHCVS Is Nothing Then
            'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
            'udcInputHCVS.SetVoucherRedeemError(False)
            'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
            udcInputHCVS.SetReasonForVisitError(False)
        End If
    End Sub
    'CRE13-019-02 Extend HCVS to China [Start][Karl]
    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
    Private Sub Step2aCleanHCVSChinaError()
        'CRE13-019-02 Extend HCVS to China [End][Karl]
        Dim udcInputVoucherChina As ucInputHCVSChina = Me.udcStep2aInputEHSClaim.GetHCVSChinaControl()
        If Not udcInputVoucherChina Is Nothing Then
            udcInputVoucherChina.SetReasonForVisitError(False)
        End If
    End Sub
    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

    Private Sub Step2aCleanCIVSSError()
        Dim udcInputCIVSS As ucInputCIVSS = Me.udcStep2aInputEHSClaim.GetCIVSSControl()
        If Not udcInputCIVSS Is Nothing Then
            udcInputCIVSS.SetDoseErrorImage(False)
        End If
    End Sub

    Private Sub Step2aCleanEVSSError()
        Dim udcInputEVSS As ucInputEVSS = Me.udcStep2aInputEHSClaim.GetEVSSControl()
        If Not udcInputEVSS Is Nothing Then
            udcInputEVSS.SetDoseErrorImage(False)
        End If
    End Sub

    Private Sub Step2aCleanHSIVSSError()
        Dim udcInputHSIVSS As ucInputHSIVSS = Me.udcStep2aInputEHSClaim.GetHSIVSSControl()
        If Not udcInputHSIVSS Is Nothing Then
            udcInputHSIVSS.SetPreConditionError(False)
            udcInputHSIVSS.SetDoseErrorImage(False)
        End If
    End Sub

    Private Sub Step2aCleanRVPError()
        Dim udcInputRVP As ucInputRVP = Me.udcStep2aInputEHSClaim.GetRVPControl()
        If Not udcInputRVP Is Nothing Then
            udcInputRVP.SetRCHCodeError(False)
            udcInputRVP.SetDoseErrorImage(False)
        End If
    End Sub

    ' CRE20-0023 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub Step2aCleanRVPCOVID19Error()
        Dim udcInputRVPCOVID19 As ucInputRVPCOVID19 = Me.udcStep2aInputEHSClaim.GetRVPCOVID19Control()
        If Not udcInputRVPCOVID19 Is Nothing Then
            udcInputRVPCOVID19.SetCategoryForCOVID19Error(False)
            udcInputRVPCOVID19.SetOutreachTypeError(False)
            udcInputRVPCOVID19.SetRecipientTypeError(False)
            udcInputRVPCOVID19.SetRCHCodeError(False)
            udcInputRVPCOVID19.SetOutreachCodeError(False)
            udcInputRVPCOVID19.SetMainCategoryForCOVID19Error(False)
            udcInputRVPCOVID19.SetSubCategoryForCOVID19Error(False)
            udcInputRVPCOVID19.SetVaccineBrandError(False)
            udcInputRVPCOVID19.SetVaccineLotNoError(False)
            udcInputRVPCOVID19.SetDoseForCOVID19Error(False)
        End If
    End Sub
    ' CRE20-0023 (Immu record) [End][Chris YIM]

    ' CRE13-001 - EHAPP [Start][Tommy L]
    ' -------------------------------------------------------------------------------------
    Private Sub Step2aCleanEHAPPError()
        Dim udcInputEHAPP As ucInputEHAPP = Me.udcStep2aInputEHSClaim.GetEHAPPControl()
        If Not udcInputEHAPP Is Nothing Then
            udcInputEHAPP.SetAllAlertVisible(False)
        End If
    End Sub
    ' CRE13-001 - EHAPP [End][Tommy L]

    'CRE15-005-03 (New PIDVSS scheme) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub Step2aCleanPIDVSSError()
        Dim udcInputPIDVSS As ucInputPIDVSS = Me.udcStep2aInputEHSClaim.GetPIDVSSControl()
        If Not udcInputPIDVSS Is Nothing Then
            udcInputPIDVSS.SetDocumentaryProofError(False)
            udcInputPIDVSS.SetDoseErrorImage(False)
        End If
    End Sub
    'CRE15-005-03 (New PIDVSS scheme) [End][Chris YIM]

    Private Sub Step2aCleanVSSError()
        Dim udcInputVSS As ucInputVSS = Me.udcStep2aInputEHSClaim.GetVSSControl()
        If Not udcInputVSS Is Nothing Then
            udcInputVSS.SetPlaceOfVaccinationError(False)
            udcInputVSS.SetDoseErrorImage(False)
            udcInputVSS.SetRecipientConditionError(False)
        End If
    End Sub

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub Step2aCleanVSSCOVID19Error()
        Dim udcInputVSSCOVID19 As ucInputVSSCOVID19 = Me.udcStep2aInputEHSClaim.GetVSSCOVID19Control()
        If Not udcInputVSSCOVID19 Is Nothing Then
            'udcInputVSSCOVID19.SetBoothError(False)
            udcInputVSSCOVID19.SetCategoryForCOVID19Error(False)
            udcInputVSSCOVID19.SetMainCategoryForCOVID19Error(False)
            udcInputVSSCOVID19.SetSubCategoryForCOVID19Error(False)
            udcInputVSSCOVID19.SetVaccineBrandError(False)
            udcInputVSSCOVID19.SetVaccineLotNoError(False)
            udcInputVSSCOVID19.SetDoseForCOVID19Error(False)
        End If
    End Sub
    ' CRE20-0022 (Immu record) [End][Chris YIM]

    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Private Sub Step2aCleanENHVSSOError()
        Dim udcInputENHVSSO As ucInputENHVSSO = Me.udcStep2aInputEHSClaim.GetENHVSSOControl()
        If Not udcInputENHVSSO Is Nothing Then
            udcInputENHVSSO.SetPlaceOfVaccinationError(False)
            udcInputENHVSSO.SetDoseErrorImage(False)
        End If
    End Sub
    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Private Sub Step2aCleanPPPError()
        'Dim udcInputPPP As ucInputPPP = Me.udcStep2aInputEHSClaim.GetPPPControl()
        'If Not udcInputPPP Is Nothing Then
        '    udcInputPPP.SetDoseErrorImage(False)
        'End If
    End Sub
    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

    ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub Step2aCleanSSSCMCError()
        Dim udcInputSSSCMC As ucInputSSSCMC = Me.udcStep2aInputEHSClaim.GetSSSCMCControl()
        If Not udcInputSSSCMC Is Nothing Then
            udcInputSSSCMC.SetError(False)
        End If
    End Sub
    ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

    ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
    ' --------------------------------------------------------------------------------------
    Private Sub Step2aCleanCOVID19Error()
        Dim udcInputCOVID19 As ucInputCOVID19 = Me.udcStep2aInputEHSClaim.GetCOVID19Control()
        If Not udcInputCOVID19 Is Nothing Then
            udcInputCOVID19.SetBoothError(False)
            udcInputCOVID19.SetCategoryForCOVID19Error(False)
            udcInputCOVID19.SetVaccineBrandError(False)
            udcInputCOVID19.SetVaccineLotNoError(False)
            udcInputCOVID19.SetDoseForCOVID19Error(False)
        End If
    End Sub
    ' CRE20-0022 (Immu record) [End][Winnie SUEN]

    Private Sub Step2aCleanCOVID19RVPError()
        Dim udcInputCOVID19RVP As ucInputCOVID19RVP = Me.udcStep2aInputEHSClaim.GetCOVID19RVPControl()
        If Not udcInputCOVID19RVP Is Nothing Then
            'udcInputCOVID19RVP.SetBoothError(False)
            udcInputCOVID19RVP.SetCategoryForCOVID19Error(False)
            udcInputCOVID19RVP.SetRecipientTypeError(False)
            udcInputCOVID19RVP.SetRCHCodeError(False)
            udcInputCOVID19RVP.SetVaccineBrandError(False)
            udcInputCOVID19RVP.SetVaccineLotNoError(False)
            udcInputCOVID19RVP.SetDoseForCOVID19Error(False)
        End If
    End Sub

    Private Sub Step2aCleanCOVID19ORError()
        Dim udcInputCOVID19OR As ucInputCOVID19OR = Me.udcStep2aInputEHSClaim.GetCOVID19ORControl()
        If Not udcInputCOVID19OR Is Nothing Then
            'udcInputCOVID19OR.SetBoothError(False)
            udcInputCOVID19OR.SetCategoryForCOVID19Error(False)
            udcInputCOVID19OR.SetOutreachCodeError(False)
            udcInputCOVID19OR.SetMainCategoryForCOVID19Error(False)
            udcInputCOVID19OR.SetSubCategoryForCOVID19Error(False)
            udcInputCOVID19OR.SetVaccineBrandError(False)
            udcInputCOVID19OR.SetVaccineLotNoError(False)
            udcInputCOVID19OR.SetDoseForCOVID19Error(False)
        End If
    End Sub
#End Region

#Region "Step 2a Validation for Enter Claim detail"

    ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [Start][Winnie]
    ' 1. Add Eligible Warning Checking
    ' 2. Add Duplicate Claim Checking
    Private Function Step2aHCVSValidation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As EHSTransactionModel) As Boolean

        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = True
        Dim strDOB As String = String.Empty
        Dim udtEligibleResult As EligibleResult = Nothing

        Dim udcInputHCVS As ucInputHCVS = Me.udcStep2aInputEHSClaim.GetHCVSControl()
        Dim udtValidator As Validator = New Validator()

        Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
        Dim udtSelectedPracticeDisplay As PracticeDisplayModel = Me._udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)

        Dim intAvailableVoucher As Integer = 0

        udcInputHCVS.SetReasonForVisitError(False)

        Me.udcMsgBoxErr.Clear()

        'INT17-021 (Tune performance on voucher) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'If isValid Then
        '    '----------------------------------------
        '    ' Concurrent Update Checking
        '    '----------------------------------------
        '    If udtEHSPersonalInfo.DocCode.Trim() = DocTypeModel.DocTypeCode.EC AndAlso udtEHSPersonalInfo.ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
        '        ' EC
        '        intAvailableVoucher = Me._udtEHSTransactionBLL.getAvailableVoucher(udtEHSTransaction.ServiceDate, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, udtEHSPersonalInfo.ECAge, udtEHSPersonalInfo.ECDateOfRegistration, udtSchemeClaim)
        '    Else
        '        ' HKIC
        '        intAvailableVoucher = Me._udtEHSTransactionBLL.getAvailableVoucher(udtEHSTransaction.ServiceDate, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, udtSchemeClaim)
        '    End If

        '    If udtEHSAccount.AvailableVoucher <> intAvailableVoucher Then
        '        isValid = False
        '        Me._blnConcurrentUpdate = True

        '        Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.EHSClaimError
        '        Return False
        '    End If

        'End If

        intAvailableVoucher = udtEHSTransaction.VoucherBeforeRedeem
        'INT17-021 (Tune performance on voucher) [End][Chris YIM]


        If Not checkByConfirmationBox Then

            ' -----------------------------------------------
            ' UI Input Validation
            '------------------------------------------------
            If Not udcInputHCVS.Validate(True, Me.udcMsgBoxErr) Then
                isValid = False
            End If

            If isValid Then
                ' --------------------------------------------------------------
                ' Check Eligibility:
                ' --------------------------------------------------------------
                If udtEHSPersonalInfo.DocCode = DocTypeModel.DocTypeCode.EC AndAlso udtEHSPersonalInfo.ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                    strDOB = _udtFormatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, udtEHSPersonalInfo.ECAge, udtEHSPersonalInfo.ECDateOfRegistration)
                Else
                    strDOB = _udtFormatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, Nothing, Nothing)
                End If

                Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckEligibilityForEnterClaim(udtSchemeClaim, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo, Nothing, udtEligibleResult)

                If Not Me._udtSystemMessage Is Nothing Then
                    ' If Check Eligibility Block Show Error
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                End If
            End If

            If isValid Then
                ' --------------------------------------------------------------
                ' Check Document Limit:
                ' --------------------------------------------------------------
                Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckExceedDocumentLimitForEnterClaim(udtSchemeClaim.SchemeCode, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo)
                If Not Me._udtSystemMessage Is Nothing Then
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
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
                Dim udtVoucherQuota As VoucherQuotaModel = udtEHSAccount.VoucherInfo.VoucherQuotalist.FilterByProfCodeEffectiveDtm(udtSelectedPracticeDisplay.ServiceCategoryCode, udtEHSTransaction.ServiceDate)

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
                    Me.ModelPopupDuplicateClaimAlert.Show()
                    EHSClaimBasePage.AuditLogShowDuplicateClaimAlert(_udtAuditLogEntry)
                End If
            End If
        End If

        If isValid Then
            'Dim udtTransactAdditionfield As TransactionAdditionalFieldModel
            ' -----------------------------------------------
            ' Set up Transaction Model: Addition Fields
            '------------------------------------------------
            udtEHSTransaction.VoucherClaim = udcInputHCVS.VoucherRedeem
            udtEHSTransaction.UIInput = udcInputHCVS.UIInput
            'udtEHSTransaction.CoPaymentFee = udcInputHCVS.CoPaymentFee

            udcInputHCVS.Save(udtEHSTransaction)

        End If

        Return isValid
    End Function
    ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [End][Winnie]


    ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [Start][Winnie]
    ' 1. Add Eligible Warning Checking
    ' 2. Add Duplicate Claim Checking
    'CRE13-019-02 Extend HCVS to China [Start][Karl]
    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
    Private Function Step2aHCVSChinaValidation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
        'CRE13-019-02 Extend HCVS to China [End][Karl]

        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = True
        Dim strDOB As String = String.Empty
        Dim udtEligibleResult As EligibleResult = Nothing

        Dim udcInputHCVSChina As ucInputHCVSChina = Me.udcStep2aInputEHSClaim.GetHCVSChinaControl()

        Dim udtValidator As Validator = New Validator()

        Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        Dim intAvailableVoucher As Integer = 0

        udcInputHCVSChina.SetVoucherRedeemError(False)

        Me.udcMsgBoxErr.Clear()

        'INT17-021 (Tune performance on voucher) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'If isValid Then
        '    '----------------------------------------
        '    ' Concurrent Update Checking
        '    '----------------------------------------
        '    If udtEHSPersonalInfo.DocCode.Trim() = DocTypeModel.DocTypeCode.EC AndAlso udtEHSPersonalInfo.ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
        '        ' EC
        '        intAvailableVoucher = Me._udtEHSTransactionBLL.getAvailableVoucher(udtEHSTransaction.ServiceDate, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, udtEHSPersonalInfo.ECAge, udtEHSPersonalInfo.ECDateOfRegistration, udtSchemeClaim)
        '    Else
        '        ' HKIC
        '        intAvailableVoucher = Me._udtEHSTransactionBLL.getAvailableVoucher(udtEHSTransaction.ServiceDate, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, udtSchemeClaim)
        '    End If

        '    If udtEHSAccount.AvailableVoucher <> intAvailableVoucher Then
        '        isValid = False
        '        Me._blnConcurrentUpdate = True

        '        Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.EHSClaimError
        '        Return False
        '    End If

        'End If

        intAvailableVoucher = udtEHSTransaction.VoucherBeforeRedeem
        'INT17-021 (Tune performance on voucher) [End][Chris YIM]


        If Not checkByConfirmationBox Then

            ' -----------------------------------------------
            ' UI Input Validation
            '------------------------------------------------
            If Not udcInputHCVSChina.Validate(True, Me.udcMsgBoxErr) Then
                isValid = False
            End If

            If isValid Then
                ' --------------------------------------------------------------
                ' Check Eligibility:
                ' --------------------------------------------------------------
                If udtEHSPersonalInfo.DocCode = DocTypeModel.DocTypeCode.EC AndAlso udtEHSPersonalInfo.ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                    strDOB = _udtFormatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, udtEHSPersonalInfo.ECAge, udtEHSPersonalInfo.ECDateOfRegistration)
                Else
                    strDOB = _udtFormatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, Nothing, Nothing)
                End If

                Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckEligibilityForEnterClaim(udtSchemeClaim, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo, Nothing, udtEligibleResult)

                If Not Me._udtSystemMessage Is Nothing Then
                    ' If Check Eligibility Block Show Error
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                End If
            End If

            If isValid Then
                ' --------------------------------------------------------------
                ' Check Document Limit:
                ' --------------------------------------------------------------
                Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckExceedDocumentLimitForEnterClaim(udtSchemeClaim.SchemeCode, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo)
                If Not Me._udtSystemMessage Is Nothing Then
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                End If
            End If

            If isValid Then
                ' --------------------------------------------------------------
                ' Check Benefit:
                ' --------------------------------------------------------------
                If intAvailableVoucher <= 0 OrElse intAvailableVoucher < udcInputHCVSChina.VoucherRedeem Then
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage("990000", "E", "00123")
                End If
            End If

            If isValid Then
                '----------------------------------------
                ' Duplicate Claim Checking
                '----------------------------------------
                udtEHSTransaction.VoucherClaimRMB = udcInputHCVSChina.VoucherRedeemRMB
                If Me._udtEHSClaimBLL.CheckDuplicateClaim(udtEHSPersonalInfo, udtEHSTransaction) = True Then
                    isValid = False
                    Me.ModelPopupDuplicateClaimAlert.Show()
                    EHSClaimBasePage.AuditLogShowDuplicateClaimAlert(_udtAuditLogEntry)
                End If
            End If
        End If

        If isValid Then

            udtEHSTransaction.VoucherClaim = udcInputHCVSChina.VoucherRedeem
            'CRE13-019-02 Extend HCVS to China [Start][Karl]
            udtEHSTransaction.ExchangeRate = udcInputHCVSChina.ExchangeRate
            udtEHSTransaction.VoucherClaimRMB = udcInputHCVSChina.VoucherRedeemRMB
            'CRE13-019-02 Extend HCVS to China [End][Karl]
            udtEHSTransaction.UIInput = udcInputHCVSChina.UIInput

            udcInputHCVSChina.Save(udtEHSTransaction)
        End If

        Return isValid
    End Function
    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
    ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [End][Winnie]

    Private Function Step2aEVSSValidation(ByVal checkByConfirmationBox As Boolean, ByVal dtmServiceDate As DateTime) As Boolean

        ' ---------------------------------------------
        ' Init
        '----------------------------------------------

        Dim isValid As Boolean = True
        Dim noVaccineSelected As Boolean = True
        Dim noDoseSelected As Boolean = True
        Dim strDOB As String

        Dim udtSchemeDetailBLL As New Common.Component.SchemeDetails.SchemeDetailBLL()
        Dim udcInputEVSS As ucInputEVSS = Me.udcStep2aInputEHSClaim.GetEVSSControl()
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = Me._udtSessionHandler.EHSClaimVaccineGetFromSession()
        udtEHSClaimVaccine = udcInputEVSS.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)

        Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        ' For Eligible & Claim Rule Warning Checking
        Dim udtEligibleResult As EligibleResult = Nothing
        Dim udtClaimRuleResult As ClaimRuleResult = Nothing
        Dim udtRuleResults As RuleResultCollection = Nothing
        Dim strText As String = String.Empty
        Dim strKey As String = String.Empty
        Dim udtRuleResult As RuleResult = Nothing
        Dim udtValidator As Validator = New Validator()

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtInputPicker As New InputPickerModel()
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        udcInputEVSS.SetDoseErrorImage(False)

        udtRuleResults = Me._udtSessionHandler.EligibleResultGetFromSession()

        If Not checkByConfirmationBox Then

            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            'Me.ClearWarningRules(udtRuleResults, SchemeClaimModel.EVSS)
            Me.ClearWarningRules(udtRuleResults)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

            ' -----------------------------------------------
            ' UI Input Validation
            '------------------------------------------------
            isValid = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, Me.udcMsgBoxErr)
            If Not isValid Then
                udcInputEVSS.SetDoseErrorImage(True)
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
            '                    Me._udtSystemMessage = udtValidator.chkServiceDateSubsidizeDoseLastServiceDate(dtmServiceDate, udtSchemeDetailBLL.getAllSchemeDosePeriod.Filter(udtSchemeClaim.SchemeCode, udtSchemeClaim.SchemeSeq, udtEHSClaimVaccineSubsidize.SubsidizeCode, udtEHSClaimSubidizeDetailModel.AvailableItemCode))
            '                    If Not Me._udtSystemMessage Is Nothing Then
            '                        isValid = False
            '                        Me.imgStep2aServiceDateError.Visible = True
            '                        Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
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

                Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckEligibilityForEnterClaim(udtSchemeClaim, dtmServiceDate, udtEHSPersonalInfo, GetVaccinationRecord(udtEHSAccount, udtSchemeClaim.SchemeCode), udtEligibleResult)

                If Not Me._udtSystemMessage Is Nothing Then
                    ' If Check Eligibility Block Show Error
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                End If

                ' --------------------------------------------------------------
                ' Check Document Limit:
                ' --------------------------------------------------------------
                If isValid Then
                    Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckExceedDocumentLimitForEnterClaim(udtSchemeClaim.SchemeCode, dtmServiceDate, udtEHSPersonalInfo)
                    If Not Me._udtSystemMessage Is Nothing Then
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                    End If
                End If

                ' --------------------------------------------------------------
                ' Check Claim Rules:
                ' --------------------------------------------------------------
                If isValid Then
                    Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckClaimRuleForEnterClaim(dtmServiceDate, udtEHSAccount, udtEHSPersonalInfo, Me._udtSessionHandler.EHSClaimVaccineGetFromSession(), GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), udtClaimRuleResult, udtInputPicker)
                    If Not Me._udtSystemMessage Is Nothing Then
                        ' If Check Claim Rules Block Show Error
                        isValid = False

                        HandleSystemMessage(Me._udtSystemMessage, udtClaimRuleResult)
                        udcInputEVSS.SetDoseErrorImage(True)

                    End If
                End If
            End If

            If isValid Then
                udtRuleResults = Me._udtSessionHandler.EligibleResultGetFromSession()

                If udtRuleResults Is Nothing Then
                    udtRuleResults = New RuleResultCollection()
                End If

                ' --------------------------------------------------------------
                ' Eligibility Warning / Declaration
                ' --------------------------------------------------------------
                If udtEligibleResult.IsEligible AndAlso _
                    (udtEligibleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtEligibleResult.HandleMethod = HandleMethodENum.Warning) Then

                    udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Step2a, udtEligibleResult.RuleType), udtEligibleResult)

                    'strText = Me.GetGlobalResourceObject("Text", "DeclaraChildOverSixYearsOldPromptMessage")
                    If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                        strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleRule.ObjectName.Trim())
                    ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                        strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleExceptionRule.ObjectName.Trim())
                    End If

                    Me.ucNoticePopUpExclamationConfirm.MessageText = strText
                    Me.ModalPopupExclamationConfirmationBox.Show()
                    EHSClaimBasePage.AuditLogShowClaimRulePopupBox(_udtAuditLogEntry)

                    isValid = False
                End If

                ' --------------------------------------------------------------
                ' Claim Rules Warning / Declaration
                ' --------------------------------------------------------------
                If Not udtClaimRuleResult Is Nothing AndAlso Not udtClaimRuleResult.IsBlock AndAlso _
                    (udtClaimRuleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtClaimRuleResult.HandleMethod = HandleMethodENum.Warning) Then

                    udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Step2a, udtClaimRuleResult.RuleType), udtClaimRuleResult)

                    'not Popup prompt before
                    If strText.Equals(String.Empty) AndAlso isValid Then
                        'Get the prompt message from ClaimRule
                        strText = Me.Step2aPromptClaimRule(udtClaimRuleResult)
                        Me.ucNoticePopUpExclamationConfirm.MessageText = strText
                        Me.ModalPopupExclamationConfirmationBox.Show()
                        EHSClaimBasePage.AuditLogShowClaimRulePopupBox(_udtAuditLogEntry)

                    End If

                    isValid = False
                End If

                Me._udtSessionHandler.EligibleResultSaveToSession(udtRuleResults)
            End If
        Else
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            'isValid = Me.RemoveRulesAfterConfirm(SchemeClaimModel.EVSS, udtRuleResults, False)
            isValid = Me.RemoveRulesAfterConfirm(udtRuleResults, False)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        End If

        Return isValid
    End Function

    Private Function Step2aCIVSSValidation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As EHSTransactionModel) As Boolean

        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = False
        Dim strDOB As String

        Dim udtSchemeDetailBLL As New Common.Component.SchemeDetails.SchemeDetailBLL()
        Dim udcInputCIVSS As ucInputCIVSS = Me.udcStep2aInputEHSClaim.GetCIVSSControl()
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = Me._udtSessionHandler.EHSClaimVaccineGetFromSession()
        Dim udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel = udtEHSClaimVaccine.SubsidizeList(0)

        Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        ' For Eligible & Claim Rule Warning Checking
        Dim udtEligibleResult As EligibleResult = Nothing
        Dim udtClaimRuleResult As ClaimRuleResult = Nothing
        Dim udtRuleResults As RuleResultCollection = Nothing
        Dim strText As String = String.Empty
        Dim strKey As String = String.Empty
        Dim udtRuleResult As RuleResult = Nothing
        Dim udtValidator As Validator = New Validator()

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtInputPicker As New InputPickerModel()
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        udcInputCIVSS.SetDoseErrorImage(False)

        udtRuleResults = Me._udtSessionHandler.EligibleResultGetFromSession()

        If Not checkByConfirmationBox Then

            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            'Me.ClearWarningRules(udtRuleResults, SchemeClaimModel.CIVSS)
            Me.ClearWarningRules(udtRuleResults)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

            ' -----------------------------------------------
            ' UI Input Validation
            '------------------------------------------------
            udtEHSClaimVaccine = udcInputCIVSS.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
            isValid = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, Me.udcMsgBoxErr)
            If Not isValid Then
                udcInputCIVSS.SetDoseErrorImage(True)
            End If

            ' ------------------------------------------------------------------
            ' Check Last Service Date of SubsidizeGroupClaim
            ' ------------------------------------------------------------------
            If isValid Then
                isValid = Me.CheckLastServiceDate(udtEHSTransaction.ServiceDate, udtSchemeClaim, udtEHSClaimVaccine)
            End If

            '' ------------------------------------------------------------------
            '' Check Dose Period
            '' ------------------------------------------------------------------
            'If isValid Then
            '    For Each udtEHSClaimVaccineSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
            '        If isValid AndAlso udtEHSClaimVaccineSubsidize.Selected Then

            '            For Each udtEHSClaimSubidizeDetailModel As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimVaccineSubsidize.SubsidizeDetailList
            '                If isValid AndAlso udtEHSClaimSubidizeDetailModel.Selected Then
            '                    Me._udtSystemMessage = udtValidator.chkServiceDateSubsidizeDoseLastServiceDate(udtEHSTransaction.ServiceDate, udtSchemeDetailBLL.getAllSchemeDosePeriod.Filter(udtSchemeClaim.SchemeCode, udtSchemeClaim.SchemeSeq, udtEHSClaimVaccineSubsidize.SubsidizeCode, udtEHSClaimSubidizeDetailModel.AvailableItemCode))
            '                    If Not Me._udtSystemMessage Is Nothing Then
            '                        isValid = False
            '                        Me.imgStep2aServiceDateError.Visible = True
            '                        Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
            '                    End If
            '                End If
            '            Next
            '        End If
            '    Next
            'End If

            ' To Do:
            ' Generic CheckClaimRulesFunction:
            ' will return ClaimResult with appropriate Status for Popup
            ' According to GroupCode / ClaimRulesCode -> Show different Popup

            'Check is Eligible for claim -> Child is 6 years old+ and the first does is avaliable = not Eligible

            If isValid Then
                ' --------------------------------------------------------------
                ' Check Eligibility:
                ' --------------------------------------------------------------
                If udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DocCode = DocTypeModel.DocTypeCode.EC AndAlso udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                    strDOB = _udtFormatter.formatDOB(udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DOB, udtEHSPersonalInfo.ExactDOB, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ECAge, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ECDateOfRegistration)
                Else
                    strDOB = _udtFormatter.formatDOB(udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DOB, udtEHSPersonalInfo.ExactDOB, Nothing, Nothing)
                End If

                Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckEligibilityForEnterClaim(udtSchemeClaim, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo, GetVaccinationRecord(udtEHSAccount, udtSchemeClaim.SchemeCode), udtEligibleResult)

                If Not Me._udtSystemMessage Is Nothing Then
                    ' If Check Eligibility Block Show Error
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                End If

                ' --------------------------------------------------------------
                ' Check Document Limit:
                ' --------------------------------------------------------------
                If isValid Then
                    Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckExceedDocumentLimitForEnterClaim(udtSchemeClaim.SchemeCode, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo)
                    If Not Me._udtSystemMessage Is Nothing Then
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                    End If
                End If

                ' --------------------------------------------------------------
                ' Check Claim Rules:
                ' --------------------------------------------------------------
                If isValid Then
                    Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckClaimRuleForEnterClaim(udtEHSTransaction.ServiceDate, udtEHSAccount, udtEHSPersonalInfo, Me._udtSessionHandler.EHSClaimVaccineGetFromSession(), GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), udtClaimRuleResult, udtInputPicker)
                    If Not Me._udtSystemMessage Is Nothing Then
                        ' If Check Claim Rules Block Show Error
                        isValid = False

                        HandleSystemMessage(Me._udtSystemMessage, udtClaimRuleResult)
                        udcInputCIVSS.SetDoseErrorImage(True)

                    End If
                End If
            End If

            If isValid Then
                udtRuleResults = Me._udtSessionHandler.EligibleResultGetFromSession()

                If udtRuleResults Is Nothing Then
                    udtRuleResults = New RuleResultCollection()
                End If

                ' --------------------------------------------------------------
                ' Eligibility Warning / Declaration
                ' --------------------------------------------------------------
                If udtEligibleResult.IsEligible AndAlso _
                    (udtEligibleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtEligibleResult.HandleMethod = HandleMethodENum.Warning) Then

                    udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Step2a, udtEligibleResult.RuleType), udtEligibleResult)

                    'strText = Me.GetGlobalResourceObject("Text", "DeclaraChildOverSixYearsOldPromptMessage")
                    If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                        strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleRule.ObjectName.Trim())
                    ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                        strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleExceptionRule.ObjectName.Trim())
                    End If

                    Me.ucNoticePopUpExclamationConfirm.MessageText = strText
                    'Me.lblPopupExclamationConfirmationBoxMessage.Text = strText
                    Me.ModalPopupExclamationConfirmationBox.Show()
                    EHSClaimBasePage.AuditLogShowClaimRulePopupBox(_udtAuditLogEntry)

                    isValid = False
                End If

                ' --------------------------------------------------------------
                ' Claim Rules Warning / Declaration
                ' --------------------------------------------------------------
                If Not udtClaimRuleResult Is Nothing AndAlso Not udtClaimRuleResult.IsBlock AndAlso _
                    (udtClaimRuleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtClaimRuleResult.HandleMethod = HandleMethodENum.Warning) Then

                    udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Step2a, udtClaimRuleResult.RuleType), udtClaimRuleResult)

                    'not Popup prompt before
                    If strText.Equals(String.Empty) AndAlso isValid Then
                        'Get the prompt message from ClaimRule
                        strText = Me.Step2aPromptClaimRule(udtClaimRuleResult)
                        Me.ucNoticePopUpExclamationConfirm.MessageText = strText
                        Me.ModalPopupExclamationConfirmationBox.Show()
                        EHSClaimBasePage.AuditLogShowClaimRulePopupBox(_udtAuditLogEntry)

                    End If

                    isValid = False
                End If

                Me._udtSessionHandler.EligibleResultSaveToSession(udtRuleResults)
            End If
        Else
            strKey = Me.RuleResultKey(ActiveViewIndex.Step2a, RuleTypeENum.EligibleResult)
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
            'isValid = Me.RemoveRulesAfterConfirm(SchemeClaimModel.CIVSS, udtRuleResults, False)
            isValid = Me.RemoveRulesAfterConfirm(udtRuleResults, False)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        End If

        Return isValid
    End Function

    Private Function Step2aHSIVSSValidation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As EHSTransactionModel) As Boolean

        ' ---------------------------------------------
        ' Init
        '----------------------------------------------

        Dim isValid As Boolean = True
        Dim noDoseSelected As Boolean = True
        Dim dtmServiceDate As Date = udtEHSTransaction.ServiceDate
        Dim strDOB As String

        Dim udtSchemeDetailBLL As New Common.Component.SchemeDetails.SchemeDetailBLL()
        Dim udcInputHSIVSS As ucInputHSIVSS = Me.udcStep2aInputEHSClaim.GetHSIVSSControl()
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = Me._udtSessionHandler.EHSClaimVaccineGetFromSession()
        Dim udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel = udtEHSClaimVaccine.SubsidizeList(0)

        Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
        Dim udtClaimCategory As ClaimCategoryModel = Me._udtSessionHandler.ClaimCategoryGetFromSession(FunctCode)

        ' For Eligible & Claim Rule Warning Checking
        Dim udtEligibleResult As EligibleResult = Nothing
        Dim udtClaimRuleResult As ClaimRuleResult = Nothing
        Dim udtRuleResults As RuleResultCollection = Nothing
        Dim strText As String = String.Empty
        Dim strKey As String = String.Empty
        Dim udtRuleResult As RuleResult = Nothing
        Dim udtValidator As Validator = New Validator()

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtInputPicker As New InputPickerModel()
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        'Init Controls
        udcInputHSIVSS.SetPreConditionError(False)
        udcInputHSIVSS.SetDoseErrorImage(False)

        udtRuleResults = Me._udtSessionHandler.EligibleResultGetFromSession()

        If Not checkByConfirmationBox Then

            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            'Me.ClearWarningRules(udtRuleResults, SchemeClaimModel.HSIVSS)
            Me.ClearWarningRules(udtRuleResults)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

            ' -----------------------------------------------
            ' UI Input Validation
            '------------------------------------------------
            udtEHSClaimVaccine = udcInputHSIVSS.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
            isValid = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, Me.udcMsgBoxErr)
            If Not isValid Then
                udcInputHSIVSS.SetDoseErrorImage(True)
            End If

            If String.IsNullOrEmpty(udcInputHSIVSS.Category) Then
                isValid = False
                udcInputHSIVSS.SetCategoryError(True)
                Me._udtSystemMessage = New SystemMessage("990000", "E", "00238")
                Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
            Else
                If udtClaimCategory.IsMedicalCondition = "Y" AndAlso String.IsNullOrEmpty(udcInputHSIVSS.PreCondition) Then
                    isValid = False
                    udcInputHSIVSS.SetPreConditionError(True)
                    Me._udtSystemMessage = New SystemMessage("990000", "E", "00196")
                    Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
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
            '                    Me._udtSystemMessage = udtValidator.chkServiceDateSubsidizeDoseLastServiceDate(dtmServiceDate, udtSchemeDetailBLL.getAllSchemeDosePeriod.Filter(udtSchemeClaim.SchemeCode, udtSchemeClaim.SchemeSeq, udtEHSClaimVaccineSubsidize.SubsidizeCode, udtEHSClaimSubidizeDetailModel.AvailableItemCode))
            '                    If Not Me._udtSystemMessage Is Nothing Then
            '                        isValid = False
            '                        Me.imgStep2aServiceDateError.Visible = True
            '                        Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
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

                Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckEligibilityForEnterClaim(udtSchemeClaim, dtmServiceDate, udtEHSPersonalInfo, GetVaccinationRecord(udtEHSAccount, udtSchemeClaim.SchemeCode), udtEligibleResult)
                If Not Me._udtSystemMessage Is Nothing Then
                    ' If Check Eligibility Block Show Error
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                End If

                ' --------------------------------------------------------------
                ' Check Document Limit:
                ' --------------------------------------------------------------
                If isValid Then
                    Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckExceedDocumentLimitForEnterClaim(udtSchemeClaim.SchemeCode, dtmServiceDate, udtEHSPersonalInfo)
                    If Not Me._udtSystemMessage Is Nothing Then
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                    End If
                End If

                ' --------------------------------------------------------------
                ' Check Claim Rules:
                ' --------------------------------------------------------------
                If isValid Then
                    Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckClaimCategoryEligibilityForEnterClaim(Me._udtSessionHandler.EHSClaimVaccineGetFromSession(), dtmServiceDate, udtEHSPersonalInfo, udtClaimCategory.CategoryCode, udtEligibleResult)
                    If Not Me._udtSystemMessage Is Nothing Then
                        ' If Check Claim Rules Block Show Error
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                    End If
                End If

                If isValid Then
                    Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckClaimRuleForEnterClaim(dtmServiceDate, udtEHSAccount, udtEHSPersonalInfo, Me._udtSessionHandler.EHSClaimVaccineGetFromSession(), GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), udtClaimRuleResult, udtInputPicker)
                    If Not Me._udtSystemMessage Is Nothing Then
                        ' If Check Claim Rules Block Show Error
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                    End If
                End If
            End If

            If isValid Then
                udtRuleResults = Me._udtSessionHandler.EligibleResultGetFromSession()

                If udtRuleResults Is Nothing Then
                    udtRuleResults = New RuleResultCollection()
                End If

                ' --------------------------------------------------------------
                ' Eligibility Warning / Declaration
                ' --------------------------------------------------------------
                If udtEligibleResult.IsEligible AndAlso _
                    (udtEligibleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtEligibleResult.HandleMethod = HandleMethodENum.Warning) Then

                    udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Step2a, udtEligibleResult.RuleType), udtEligibleResult)

                    'strText = Me.GetGlobalResourceObject("Text", "DeclaraChildOverSixYearsOldPromptMessage")
                    If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                        strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleRule.ObjectName.Trim())
                    ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                        strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleExceptionRule.ObjectName.Trim())
                    ElseIf Not udtEligibleResult.RelatedClaimCategoryEligibilityModel Is Nothing Then
                        strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedClaimCategoryEligibilityModel.ObjectName.Trim())
                    End If

                    Me.ucNoticePopUpExclamationConfirm.MessageText = strText
                    Me.ModalPopupExclamationConfirmationBox.Show()
                    EHSClaimBasePage.AuditLogShowClaimRulePopupBox(_udtAuditLogEntry)

                    isValid = False
                End If

                ' --------------------------------------------------------------
                ' Claim Rules Warning / Declaration
                ' --------------------------------------------------------------
                If Not udtClaimRuleResult Is Nothing AndAlso Not udtClaimRuleResult.IsBlock AndAlso _
                    (udtClaimRuleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtClaimRuleResult.HandleMethod = HandleMethodENum.Warning) Then

                    udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Step2a, udtClaimRuleResult.RuleType), udtClaimRuleResult)

                    'not Popup prompt before
                    If strText.Equals(String.Empty) AndAlso isValid Then
                        'Get the prompt message from ClaimRule
                        strText = Me.Step2aPromptClaimRule(udtClaimRuleResult)
                        Me.ucNoticePopUpExclamationConfirm.MessageText = strText
                        Me.ModalPopupExclamationConfirmationBox.Show()
                        EHSClaimBasePage.AuditLogShowClaimRulePopupBox(_udtAuditLogEntry)

                    End If

                    isValid = False
                End If

                Me._udtSessionHandler.EligibleResultSaveToSession(udtRuleResults)
            End If
        Else
            strKey = Me.RuleResultKey(ActiveViewIndex.Step2a, RuleTypeENum.EligibleResult)
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
            'isValid = Me.RemoveRulesAfterConfirm(SchemeClaimModel.HSIVSS, udtRuleResults, False)
            isValid = Me.RemoveRulesAfterConfirm(udtRuleResults, False)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        End If

        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        If isValid Then

            Dim udtTransactAdditionfield As TransactionAdditionalFieldModel
            udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()

            ' -----------------------------------------------
            ' Get Latest SchemeSeq Selected
            '------------------------------------------------
            Dim udtSubsidizeLatest As EHSClaimVaccineModel.EHSClaimSubsidizeModel = udtEHSClaimVaccine.GetLatestSelectedSubsidize

            '-------------------------------------------------
            ' Set up Transaction Model Addition Fields : Category
            '-------------------------------------------------
            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            udtEHSTransaction.CategoryCode = udcInputHSIVSS.Category
            'CRE16-002 (Revamp VSS) [End][Chris YIM]



            ' -----------------------------------------------
            ' Set up Transaction Model Addition Fields : PreCondition
            '------------------------------------------------
            If udtClaimCategory.IsMedicalCondition = "Y" Then
                udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                udtTransactAdditionfield.AdditionalFieldID = "PreCondition"
                udtTransactAdditionfield.AdditionalFieldValueCode = udcInputHSIVSS.PreCondition
                udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
                udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
                udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode.Trim()
                udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
            End If

        End If
        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

        Return isValid
    End Function

    Private Function Step2aRVPValidation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As EHSTransactionModel) As Boolean

        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = True
        Dim noVaccineSelected As Boolean = True
        Dim noDoseSelected As Boolean = True
        Dim dtmServiceDate As Date = udtEHSTransaction.ServiceDate
        Dim strDOB As String

        Dim udtSchemeDetailBLL As New Common.Component.SchemeDetails.SchemeDetailBLL()
        Dim udcInputRVP As ucInputRVP = Me.udcStep2aInputEHSClaim.GetRVPControl()
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = Me._udtSessionHandler.EHSClaimVaccineGetFromSession()
        Dim udtGeneralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
        udtEHSClaimVaccine = udcInputRVP.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)

        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
        Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)

        Dim udtValidator As Validator = New Validator()

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtInputPicker As New InputPickerModel()
        udtInputPicker.RCHCode = udcInputRVP.RCHCode
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        ' For Eligible & Claim Rule Warning Checking
        Dim udtEligibleResult As EligibleResult = Nothing
        Dim udtClaimRuleResult As ClaimRuleResult = Nothing
        Dim udtRuleResults As RuleResultCollection = Nothing
        Dim strText As String = String.Empty
        Dim strKey As String = String.Empty
        Dim strEnableClaimCategory As String = Nothing

        udtGeneralFunction.getSytemParameterByParameterNameSchemeCode("RVPEnableClaimCategory", strEnableClaimCategory, String.Empty, SchemeClaimModel.RVP)

        Dim udtRuleResult As RuleResult = Nothing
        udtRuleResults = Me._udtSessionHandler.EligibleResultGetFromSession()

        'Init Controls
        udcInputRVP.SetRCHCodeError(False)
        udcInputRVP.SetCategoryError(False)
        udcInputRVP.SetDoseErrorImage(False)

        If Not checkByConfirmationBox Then

            Me.ClearWarningRules(udtRuleResults)

            ' -----------------------------------------------
            ' UI Input Validation
            '------------------------------------------------

            'Claim Detial Part & Vaccine Part
            'CRE16-026 (Add PCV13) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            isValid = udcInputRVP.Validate(True, Me.udcMsgBoxErr, strEnableClaimCategory)

            If isValid Then
                Dim strHighRisk As String = String.Empty
                Select Case udcInputRVP.HighRiskOptionShown
                    Case SubsidizeGroupClaimModel.HighRiskOptionClass.ShowForInput 'Manual Input
                        If udcInputRVP.HighRiskOptionEnabled Then
                            strHighRisk = udcInputRVP.HighRisk()
                        End If
                    Case SubsidizeGroupClaimModel.HighRiskOptionClass.HideButForceHighRisk 'Auto Input
                        strHighRisk = YesNo.Yes
                    Case SubsidizeGroupClaimModel.HighRiskOptionClass.HideWithoutInput 'No Input
                        strHighRisk = String.Empty
                    Case Else
                        strHighRisk = String.Empty
                End Select

                udtInputPicker.HighRisk = strHighRisk
            End If
            'CRE16-026 (Add PCV13) [End][Chris YIM]

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
            '                    Me._udtSystemMessage = udtValidator.chkServiceDateSubsidizeDoseLastServiceDate(dtmServiceDate, udtSchemeDetailBLL.getAllSchemeDosePeriod.Filter(udtSchemeClaim.SchemeCode, udtSchemeClaim.SchemeSeq, udtEHSClaimVaccineSubsidize.SubsidizeCode, udtEHSClaimSubidizeDetailModel.AvailableItemCode))
            '                    If Not Me._udtSystemMessage Is Nothing Then
            '                        isValid = False
            '                        Me.imgStep2aServiceDateError.Visible = True
            '                        Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
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

                Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckEligibilityForEnterClaim(udtSchemeClaim, dtmServiceDate, udtEHSPersonalInfo, GetVaccinationRecord(udtEHSAccount, udtSchemeClaim.SchemeCode), udtEligibleResult)

                If Not Me._udtSystemMessage Is Nothing Then
                    ' If Check Eligibility Block Show Error
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                End If

                ' --------------------------------------------------------------
                ' Check Document Limit:
                ' --------------------------------------------------------------
                If isValid Then
                    Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckExceedDocumentLimitForEnterClaim(udtSchemeClaim.SchemeCode, dtmServiceDate, udtEHSPersonalInfo)
                    If Not Me._udtSystemMessage Is Nothing Then
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                    End If
                End If

                ' --------------------------------------------------------------
                ' Check Claim Rules:
                ' --------------------------------------------------------------
                If isValid Then
                    ' CRE14-021 IV for Southern Hemsiphere Vaccine under RVP [Start][Lawrence]
                    Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckClaimRuleForEnterClaim(dtmServiceDate, udtEHSAccount, udtEHSPersonalInfo, Me._udtSessionHandler.EHSClaimVaccineGetFromSession(), GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), udtClaimRuleResult, udtInputPicker)
                    ' CRE14-021 IV for Southern Hemsiphere Vaccine under RVP [End][Lawrence]

                    If Not Me._udtSystemMessage Is Nothing Then
                        ' If Check Claim Rules Block Show Error
                        isValid = False

                        HandleSystemMessage(Me._udtSystemMessage, udtClaimRuleResult)
                        udcInputRVP.SetDoseErrorImage(True)

                    End If
                End If
            End If

            If isValid Then
                udtRuleResults = Me._udtSessionHandler.EligibleResultGetFromSession()

                If udtRuleResults Is Nothing Then
                    udtRuleResults = New RuleResultCollection()
                End If

                ' --------------------------------------------------------------
                ' Eligibility Warning / Declaration
                ' --------------------------------------------------------------
                If udtEligibleResult.IsEligible AndAlso _
                    (udtEligibleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtEligibleResult.HandleMethod = HandleMethodENum.Warning) Then

                    udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Step2a, udtEligibleResult.RuleType), udtEligibleResult)

                    'strText = Me.GetGlobalResourceObject("Text", "DeclaraChildOverSixYearsOldPromptMessage")
                    If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                        strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleRule.ObjectName.Trim())
                    ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                        strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleExceptionRule.ObjectName.Trim())
                    ElseIf Not udtEligibleResult.RelatedClaimCategoryEligibilityModel Is Nothing Then
                        strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedClaimCategoryEligibilityModel.ObjectName.Trim())
                    End If

                    Me.ucNoticePopUpExclamationConfirm.MessageText = strText
                    Me.ModalPopupExclamationConfirmationBox.Show()

                    _udtAuditLogEntry.AddDescripton("Message", strText)
                    EHSClaimBasePage.AuditLogShowClaimRulePopupBox(_udtAuditLogEntry)

                    isValid = False
                End If

                ' --------------------------------------------------------------
                ' Claim Rules Warning / Declaration
                ' --------------------------------------------------------------
                If Not udtClaimRuleResult Is Nothing AndAlso Not udtClaimRuleResult.IsBlock AndAlso _
                    (udtClaimRuleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtClaimRuleResult.HandleMethod = HandleMethodENum.Warning) Then

                    udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Step2a, udtClaimRuleResult.RuleType), udtClaimRuleResult)

                    'not Popup prompt before
                    If strText.Equals(String.Empty) AndAlso isValid Then
                        'Get the prompt message from ClaimRule
                        strText = Me.Step2aPromptClaimRule(udtClaimRuleResult)
                        Me.ucNoticePopUpExclamationConfirm.MessageText = strText
                        Me.ModalPopupExclamationConfirmationBox.Show()

                        _udtAuditLogEntry.AddDescripton("Message", strText)
                        EHSClaimBasePage.AuditLogShowClaimRulePopupBox(_udtAuditLogEntry)

                    End If

                    isValid = False
                End If

                Me._udtSessionHandler.EligibleResultSaveToSession(udtRuleResults)
            End If
        Else
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            'isValid = Me.RemoveRulesAfterConfirm(SchemeClaimModel.RVP, udtRuleResults, False)
            isValid = Me.RemoveRulesAfterConfirm(udtRuleResults, False)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        End If

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        If isValid Then
            udcInputRVP.Save(udtEHSTransaction, udtEHSClaimVaccine, strEnableClaimCategory)
        End If
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        Return isValid

    End Function

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Private Function Step2aRVPCOVID19Validation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = False
        Dim strDOB As String

        Dim udtSchemeDetailBLL As New Common.Component.SchemeDetails.SchemeDetailBLL()
        Dim udtValidator As Validator = New Validator()

        Dim udcInputRVPCOVID19 As ucInputRVPCOVID19 = Me.udcStep2aInputEHSClaim.GetRVPCOVID19Control
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = Me._udtSessionHandler.EHSClaimVaccineGetFromSession()
        Dim udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel = udtEHSClaimVaccine.SubsidizeList(0)

        Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        ' For Eligible & Claim Rule Warning Checking
        Dim udtEligibleResult As EligibleResult = Nothing
        Dim udtClaimRuleResultList As New List(Of ClaimRuleResult)
        Dim udtRuleResults As RuleResultCollection = Nothing
        Dim strHeader As String = String.Empty
        Dim strText As String = String.Empty
        Dim strKey As String = String.Empty
        Dim udtRuleResult As RuleResult = Nothing

        Dim udtInputPicker As New InputPickerModel()
        udtInputPicker.ServiceDate = udtEHSTransaction.ServiceDate
        udtInputPicker.SPID = udtEHSTransaction.ServiceProviderID
        udtInputPicker.PracticeDisplaySeq = udtEHSTransaction.PracticeID

        udcInputRVPCOVID19.SetDoseErrorImage(False)

        udtRuleResults = Me._udtSessionHandler.EligibleResultGetFromSession()

        If Not checkByConfirmationBox Then

            Me.ClearWarningRules(udtRuleResults)

            ' -----------------------------------------------
            ' UI Input Validation
            '------------------------------------------------

            'Claim Detial Part & Vaccine Part
            isValid = udcInputRVPCOVID19.Validate(True, Me.udcMsgBoxErr)

            If panStep2aRecipinetContactInfo.Visible Then
                If Me.txtStep2aContactNo.Enabled AndAlso _
                    ((udcInputRVPCOVID19.TypeOfOutreach = TYPE_OF_OUTREACH.RCH AndAlso _
                        udcInputRVPCOVID19.RecipientType <> RECIPIENT_TYPE.RESIDENT AndAlso _
                        udcInputRVPCOVID19.RecipientType <> String.Empty) OrElse _
                    udcInputRVPCOVID19.TypeOfOutreach = TYPE_OF_OUTREACH.OTHER) Then

                    'If String.IsNullOrEmpty(Me.txtStep2aContactNo.Text) AndAlso Me.chkStep2aMobile.Checked Then
                    If String.IsNullOrEmpty(Me.txtStep2aContactNo.Text) Then
                        isValid = False

                        imgStep2aContactNoError.Visible = True

                        Dim udtMsg As SystemMessage = New SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00463)
                        If Me.udcMsgBoxErr IsNot Nothing Then Me.udcMsgBoxErr.AddMessage(udtMsg, _
                                                                                        New String() {"%en", "%tc", "%sc"}, _
                                                                                        New String() {HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.English)), _
                                                                                                      HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)), _
                                                                                                      HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese))})
                    End If

                    If Not String.IsNullOrEmpty(Me.txtStep2aContactNo.Text) Then
                        If Not Regex.IsMatch(Me.txtStep2aContactNo.Text, "^[2-9]\d{7}$") Then
                            isValid = False

                            imgStep2aContactNoError.Visible = True

                            Dim udtMsg As SystemMessage = New SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00466)
                            If Me.udcMsgBoxErr IsNot Nothing Then Me.udcMsgBoxErr.AddMessage(udtMsg, _
                                                                                        New String() {"%en", "%tc", "%sc"}, _
                                                                                        New String() {HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.English)), _
                                                                                                      HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)), _
                                                                                                      HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese))})
                        End If

                    End If

                End If

            End If

            'If Not Me.chkStep2aDeclareClaim.Checked Then
            '    isValid = False

            '    imgStep2aDeclareClaimError.Visible = True

            '    Dim udtMsg As SystemMessage = New SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00367)
            '    If Me.udcMsgBoxErr IsNot Nothing Then Me.udcMsgBoxErr.AddMessage(udtMsg, "%s", "Verification Checklist")
            'End If

        End If

        'Mark selected vaccine & dose in EHSClaimVaccineModel model
        If isValid OrElse checkByConfirmationBox Then
            udtInputPicker.Brand = udcInputRVPCOVID19.VaccineBrand
            udtInputPicker.VaccinationRecord = GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode)

            Dim udtLatestC19Vaccine As TransactionDetailVaccineModel = Nothing
            Dim udtLatestC19Transaction As EHSTransactionModel = Nothing
            Dim udtTranDetailVaccineList As TransactionDetailVaccineModelCollection = GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode)
            Dim udtC19VaccineList As TransactionDetailVaccineModelCollection = udtTranDetailVaccineList.FilterIncludeBySubsidizeItemCode(SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19)

            For Each udtC19Vaccine As TransactionDetailVaccineModel In udtC19VaccineList
                'Find the latest COVID19 transaction in EHS
                If udtC19Vaccine.AvailableItemCode.Trim.ToUpper = "1STDOSE" Then
                    If udtLatestC19Vaccine Is Nothing Then
                        udtLatestC19Vaccine = udtC19Vaccine
                    Else
                        If udtC19Vaccine.ServiceReceiveDtm > udtLatestC19Vaccine.ServiceReceiveDtm Then
                            udtLatestC19Vaccine = udtC19Vaccine
                        End If
                    End If
                End If
            Next

            'Only allow EHS Transaction, not include CMS / CIMS record
            If udtLatestC19Vaccine IsNot Nothing AndAlso udtLatestC19Vaccine.TransactionID <> String.Empty Then
                udtLatestC19Transaction = _udtEHSTransactionBLL.LoadClaimTran(udtLatestC19Vaccine.TransactionID.Trim)
                udtInputPicker.LatestC19Transaction = udtLatestC19Transaction
            End If

            udcInputRVPCOVID19.Selection()
        End If

        If Not checkByConfirmationBox Then
            ' ------------------------------------------------------------------
            ' Check Last Service Date of SubsidizeGroupClaim
            ' ------------------------------------------------------------------
            If isValid Then
                isValid = Me.CheckLastServiceDate(udtEHSTransaction.ServiceDate, udtSchemeClaim, udtEHSClaimVaccine)
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

                Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckEligibilityForEnterClaim(udtSchemeClaim, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo, GetVaccinationRecord(udtEHSAccount, udtSchemeClaim.SchemeCode), udtEligibleResult)

                If Not Me._udtSystemMessage Is Nothing Then
                    ' If Check Eligibility Block Show Error
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                End If

                ' --------------------------------------------------------------
                ' Check Document Limit:
                ' --------------------------------------------------------------
                If isValid Then
                    Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckExceedDocumentLimitForEnterClaim(udtSchemeClaim.SchemeCode, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo)
                    If Not Me._udtSystemMessage Is Nothing Then
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                    End If
                End If

                ' --------------------------------------------------------------
                ' Check Claim Rules:
                ' --------------------------------------------------------------
                If isValid Then
                    Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckClaimRuleForEnterClaimCOVID19(udtEHSTransaction.ServiceDate, _
                                                                                                 udtEHSAccount, _
                                                                                                 udtEHSPersonalInfo, _
                                                                                                 Me._udtSessionHandler.EHSClaimVaccineGetFromSession(), _
                                                                                                 GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), _
                                                                                                 udtClaimRuleResultList, _
                                                                                                 udtInputPicker)
                    If Not Me._udtSystemMessage Is Nothing Then
                        ' If Check Claim Rules Block Show Error
                        isValid = False

                        For Each udtClaimRuleResult As ClaimRuleResult In udtClaimRuleResultList
                            HandleSystemMessage(Me._udtSystemMessage, udtClaimRuleResult)
                        Next

                        udcInputRVPCOVID19.SetDoseErrorImage(True)

                        If _udtSystemMessage.FunctionCode = "990000" AndAlso _udtSystemMessage.SeverityCode = "E" AndAlso _udtSystemMessage.MessageCode = "00464" Then
                            udcInputRVPCOVID19.SetVaccineBrandError(True)
                        End If
                    End If
                End If
            End If

            If isValid Then
                udtRuleResults = Me._udtSessionHandler.EligibleResultGetFromSession()

                If udtRuleResults Is Nothing Then
                    udtRuleResults = New RuleResultCollection()
                End If

                ' --------------------------------------------------------------
                ' Eligibility Warning / Declaration
                ' --------------------------------------------------------------
                If udtEligibleResult.IsEligible AndAlso _
                    (udtEligibleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtEligibleResult.HandleMethod = HandleMethodENum.Warning) Then

                    'Dim drClaimCategory As DataRow = (New ClaimCategoryBLL).getCategoryDesc(MyBase.EHSTransaction.CategoryCode)
                    Dim strCategoryCode As String = String.Empty
                    Dim strCategoryCodeForRule As String = String.Empty
                    Dim strCategoryCodeForExceptionRule As String = String.Empty
                    Dim udtClaimCategoryModelCollection As ClaimCategoryModelCollection = (New ClaimCategoryBLL).getAllCategoryCache
                    For Each udtClaimCategoryModel As ClaimCategoryModel In udtClaimCategoryModelCollection
                        If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                            If udtClaimCategoryModel.SchemeCode = udtEligibleResult.RelatedEligibleRule.SchemeCode And _
                                udtClaimCategoryModel.SchemeSeq = udtEligibleResult.RelatedEligibleRule.SchemeSeq And _
                                udtClaimCategoryModel.SubsidizeCode = udtEligibleResult.RelatedEligibleRule.SubsidizeCode Then
                                strCategoryCodeForRule = udtClaimCategoryModel.CategoryCode
                            End If
                        ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                            If udtClaimCategoryModel.SchemeCode = udtEligibleResult.RelatedEligibleExceptionRule.SchemeCode And _
                                udtClaimCategoryModel.SchemeSeq = udtEligibleResult.RelatedEligibleExceptionRule.SchemeSeq And _
                                udtClaimCategoryModel.SubsidizeCode = udtEligibleResult.RelatedEligibleExceptionRule.SubsidizeCode Then
                                strCategoryCodeForExceptionRule = udtClaimCategoryModel.CategoryCode
                            End If
                        End If
                    Next

                    If strCategoryCodeForRule <> String.Empty And strCategoryCodeForExceptionRule <> String.Empty And _
                        strCategoryCodeForRule = strCategoryCodeForExceptionRule Then
                        strCategoryCode = strCategoryCodeForRule
                    End If

                    If strCategoryCodeForRule <> String.Empty Then
                        strCategoryCode = strCategoryCodeForRule
                    End If

                    If strCategoryCodeForExceptionRule <> String.Empty Then
                        strCategoryCode = strCategoryCodeForExceptionRule
                    End If

                    If strCategoryCode = udcInputRVPCOVID19.CategoryForCOVID19 Then
                        udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Step2a, udtEligibleResult.RuleType), udtEligibleResult)

                        If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                            strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleRule.ObjectName.Trim())
                        ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                            strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleExceptionRule.ObjectName.Trim())
                        End If

                        Me.ucNoticePopUpExclamationConfirm.MessageText = strText

                        Me.ModalPopupExclamationConfirmationBox.Show()

                        _udtAuditLogEntry.AddDescripton("Message", strText)
                        EHSClaimBasePage.AuditLogShowClaimRulePopupBox(_udtAuditLogEntry)

                        isValid = False
                    End If

                End If

                ' --------------------------------------------------------------
                ' Claim Rules Warning / Declaration
                ' --------------------------------------------------------------
                If udtClaimRuleResultList IsNot Nothing AndAlso udtClaimRuleResultList.Count > 0 Then
                    Dim blnError As Boolean = False
                    Dim blnWarning As Boolean = False

                    For Each udtClaimRuleResult As ClaimRuleResult In udtClaimRuleResultList
                        If udtClaimRuleResult.IsBlock Then
                            blnError = True
                        End If

                        If Not udtClaimRuleResult.IsBlock AndAlso _
                            (udtClaimRuleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtClaimRuleResult.HandleMethod = HandleMethodENum.Warning) Then
                            blnWarning = True
                        End If
                    Next

                    If Not blnError AndAlso blnWarning Then
                        udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Step2a, udtClaimRuleResultList(0).RuleType), udtClaimRuleResultList(0))

                        'not Popup prompt before
                        If strText.Equals(String.Empty) AndAlso isValid Then
                            Dim strHTMLList As String = Me.Step2aPromptContent(udtClaimRuleResultList)

                            'Get the prompt message from ClaimRule
                            If udtClaimRuleResultList.Count = 1 Then
                                strHeader = Me.Step2aPromptHeader(udtClaimRuleResultList(0))
                            End If

                            If strHeader <> String.Empty Then
                                Me.ucNoticePopUpExclamationConfirm.CustomHeaderText = strHeader
                            End If

                            Me.ucNoticePopUpExclamationConfirm.MessageText = strHTMLList

                            Me.ModalPopupExclamationConfirmationBox.Show()

                            _udtAuditLogEntry.AddDescripton("Message", strText)
                            EHSClaimBasePage.AuditLogShowClaimRulePopupBox(_udtAuditLogEntry)

                        End If

                        isValid = False
                    End If
                End If

                Me._udtSessionHandler.EligibleResultSaveToSession(udtRuleResults)
            End If
        Else
            strKey = Me.RuleResultKey(ActiveViewIndex.Step2a, RuleTypeENum.EligibleResult)
            udtRuleResult = udtRuleResults.Item(strKey)

            If Not udtRuleResult Is Nothing Then

                'Should have 2 rule in this collection
                'first : After sreach this account -> rule added and auto make to confirmed
                'Second : After press "Next" in Enter claim detail -> rule added but not confirmed
                If Not udtRuleResult.PromptConfirmed Then
                    udtEHSTransaction.PreSchool = "Y"
                End If
            End If

            isValid = Me.RemoveRulesAfterConfirm(udtRuleResults, False)
        End If

        If isValid Then
            udcInputRVPCOVID19.Save(udtEHSTransaction, udtEHSClaimVaccine)

            'Clear old vaccination record for vaccination card
            _udtSessionHandler.ClaimCOVID19VaccinationCardRemoveFromSession(FunctionCode)

            'Find the nearest vaccination record for vaccination card
            Dim udtVaccinationRecordList As TransactionDetailVaccineModelCollection = Nothing
            udtVaccinationRecordList = GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode).FilterIncludeBySubsidizeItemCode(SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19)

            If udtVaccinationRecordList.Count > 0 Then
                Dim udtVaccinationRecord As TransactionDetailVaccineModel = udtVaccinationRecordList.FilterFindNearestRecord()

                If udtEHSClaimVaccine.GetSelectedDoseForCOVID19.AvailableItemCode.Trim().ToUpper() <> udtVaccinationRecord.AvailableItemCode.Trim.Trim().ToUpper() Then
                    'If udtInputPicker.Brand.Trim = udtVaccinationRecord.VaccineBrand.Trim.Trim().ToUpper() Then
                    _udtSessionHandler.ClaimCOVID19VaccinationCardSaveToSession(udtVaccinationRecord, FunctionCode)
                    'End If

                End If

            End If

            If Me.ClaimMode = ClaimMode.COVID19 AndAlso panStep2aRecipinetContactInfo.Visible Then
                Dim udtTransactAdditionfield As TransactionAdditionalFieldModel

                If Me._udtEHSTransaction.TransactionAdditionFields(0) IsNot Nothing Then
                    'Contact No.
                    Dim strContactNo As String = String.Empty

                    If ((udcInputRVPCOVID19.TypeOfOutreach = TYPE_OF_OUTREACH.RCH AndAlso udcInputRVPCOVID19.RecipientType <> RECIPIENT_TYPE.RESIDENT AndAlso _
                        udcInputRVPCOVID19.RecipientType <> String.Empty) _
                        OrElse _
                        udcInputRVPCOVID19.TypeOfOutreach = TYPE_OF_OUTREACH.OTHER) Then

                        strContactNo = txtStep2aContactNo.Text.Trim

                    End If

                    udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                    udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ContactNo
                    udtTransactAdditionfield.AdditionalFieldValueCode = strContactNo
                    udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
                    udtTransactAdditionfield.SchemeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeCode
                    udtTransactAdditionfield.SchemeSeq = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeSeq
                    udtTransactAdditionfield.SubsidizeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SubsidizeCode
                    Me._udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

                    ''Mobile
                    'udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                    'udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.Mobile
                    'udtTransactAdditionfield.AdditionalFieldValueCode = IIf(Me.chkStep2aMobile.Checked, YesNo.Yes, YesNo.No)
                    'udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
                    'udtTransactAdditionfield.SchemeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeCode
                    'udtTransactAdditionfield.SchemeSeq = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeSeq
                    'udtTransactAdditionfield.SubsidizeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SubsidizeCode
                    'Me._udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

                    'Remarks
                    udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                    udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.Remarks
                    udtTransactAdditionfield.AdditionalFieldValueCode = String.Empty
                    udtTransactAdditionfield.AdditionalFieldValueDesc = txtStep2aRemark.Text.Trim
                    udtTransactAdditionfield.SchemeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeCode
                    udtTransactAdditionfield.SchemeSeq = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeSeq
                    udtTransactAdditionfield.SubsidizeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SubsidizeCode
                    Me._udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

                    'JoinEHRSS
                    Dim strJoinEHRSS As String = String.Empty

                    If panStep2aDeclareJoineHRSS.Visible AndAlso _
                        ((udcInputRVPCOVID19.TypeOfOutreach = TYPE_OF_OUTREACH.RCH AndAlso udcInputRVPCOVID19.RecipientType <> RECIPIENT_TYPE.RESIDENT AndAlso _
                        udcInputRVPCOVID19.RecipientType <> String.Empty) _
                        OrElse _
                        udcInputRVPCOVID19.TypeOfOutreach = TYPE_OF_OUTREACH.OTHER) Then

                        strJoinEHRSS = IIf(chkStep2aDeclareJoineHRSS.Checked, YesNo.Yes, YesNo.No)

                    End If

                    udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                    udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.JoinEHRSS
                    udtTransactAdditionfield.AdditionalFieldValueCode = strJoinEHRSS
                    udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
                    udtTransactAdditionfield.SchemeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeCode
                    udtTransactAdditionfield.SchemeSeq = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeSeq
                    udtTransactAdditionfield.SubsidizeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SubsidizeCode
                    Me._udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

                End If
            End If

        End If

        Return isValid
    End Function
    ' CRE20-0022 (Immu record) [End][Chris YIM]

    ' CRE13-001 - EHAPP [Start][Tommy L]
    ' -------------------------------------------------------------------------------------
    Private Function Step2aEHAPPValidation(ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
        Dim isValid As Boolean
        Dim udtEligibleResult As EligibleResult = Nothing

        Dim udtValidator As New Validator

        Dim strDOB As String = String.Empty
        Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        Dim udcInputEHAPP As ucInputEHAPP = Me.udcStep2aInputEHSClaim.GetEHAPPControl()

        ' -----------------------------------------------
        ' UI Input Validation
        '------------------------------------------------
        isValid = udcInputEHAPP.Validate(Me.udcMsgBoxErr)

        ' ------------------------------------------------------------------
        ' Check Last Service Date of SubsidizeGroupClaim
        ' ------------------------------------------------------------------
        If isValid Then
            Me._udtSystemMessage = udtValidator.chkServiceDataSubsidizeGroupLastServiceData(udtEHSTransaction.ServiceDate, udtSchemeClaim.SubsidizeGroupClaimList(0))
            If Not Me._udtSystemMessage Is Nothing Then
                isValid = False
                Me.imgStep2aServiceDateError.Visible = True
                Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
            End If
        End If

        If isValid Then
            ' --------------------------------------------------------------
            ' Check Eligibility:
            ' --------------------------------------------------------------
            If udtEHSPersonalInfo.DocCode = DocTypeModel.DocTypeCode.EC AndAlso udtEHSPersonalInfo.ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                strDOB = _udtFormatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, udtEHSPersonalInfo.ECAge, udtEHSPersonalInfo.ECDateOfRegistration)
            Else
                strDOB = _udtFormatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, Nothing, Nothing)
            End If

            Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckEligibilityForEnterClaim(udtSchemeClaim, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo, Nothing, udtEligibleResult)

            If Not Me._udtSystemMessage Is Nothing Then
                ' If Check Eligibility Block Show Error
                isValid = False
                Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
            End If
        End If

        If isValid Then
            ' --------------------------------------------------------------
            ' Check Document Limit:
            ' --------------------------------------------------------------
            Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckExceedDocumentLimitForEnterClaim(udtSchemeClaim.SchemeCode, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo)
            If Not Me._udtSystemMessage Is Nothing Then
                isValid = False
                Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
            End If
        End If

        If isValid Then
            ' --------------------------------------------------------------
            ' Check Benefit:
            ' --------------------------------------------------------------
            ' INT13-0012 - Fix EHAPP concurrent claim checking [Start][Tommy L]
            ' -------------------------------------------------------------------------------------
            Dim udtEHSTransactionBLL As New EHSTransactionBLL

            If udtEHSTransactionBLL.getAvailableSubsidizeItem_Registration(udtEHSPersonalInfo, udtSchemeClaim.SubsidizeGroupClaimList(0)) > 0 Then
                ' Subsidies for EHAPP Registration is Available
            Else
                ' No Available Subsidies for EHAPP Registration
                isValid = False
                Me.udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00107"))
            End If
            ' INT13-0012 - Fix EHAPP concurrent claim checking [End][Tommy L]

        End If

        If isValid Then
            udcInputEHAPP.Save(udtEHSTransaction)
        End If

        Return isValid
    End Function
    ' CRE13-001 - EHAPP [End][Tommy L]

    'CRE15-005-03 (New PIDVSS scheme) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Function Step2aPIDVSSValidation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = False
        Dim strDOB As String

        Dim udtSchemeDetailBLL As New Common.Component.SchemeDetails.SchemeDetailBLL()
        Dim udcInputPIDVSS As ucInputPIDVSS = Me.udcStep2aInputEHSClaim.GetPIDVSSControl
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = Me._udtSessionHandler.EHSClaimVaccineGetFromSession()
        Dim udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel = udtEHSClaimVaccine.SubsidizeList(0)

        Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        ' For Eligible & Claim Rule Warning Checking
        Dim udtEligibleResult As EligibleResult = Nothing
        Dim udtClaimRuleResult As ClaimRuleResult = Nothing
        Dim udtRuleResults As RuleResultCollection = Nothing
        Dim strText As String = String.Empty
        Dim strKey As String = String.Empty
        Dim udtRuleResult As RuleResult = Nothing
        Dim udtValidator As Validator = New Validator()

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtInputPicker As New InputPickerModel()
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        udcInputPIDVSS.SetDoseErrorImage(False)

        udtRuleResults = Me._udtSessionHandler.EligibleResultGetFromSession()

        If Not checkByConfirmationBox Then

            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            'Me.ClearWarningRules(udtRuleResults, SchemeClaimModel.CIVSS)
            Me.ClearWarningRules(udtRuleResults)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

            ' -----------------------------------------------
            ' UI Input Validation
            '------------------------------------------------
            Dim isValidDetail, isValidVaccineSelection As Boolean

            'Claim Detial Part
            isValidDetail = udcInputPIDVSS.Validate(True, Me.udcMsgBoxErr)

            'Select Vaccine Part
            udtEHSClaimVaccine = udcInputPIDVSS.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
            isValidVaccineSelection = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, Me.udcMsgBoxErr)
            If Not isValidVaccineSelection Then
                udcInputPIDVSS.SetDoseErrorImage(True)
            End If

            'Combine Result
            isValid = isValidDetail And isValidVaccineSelection

            ' ------------------------------------------------------------------
            ' Check Last Service Date of SubsidizeGroupClaim
            ' ------------------------------------------------------------------
            If isValid Then
                isValid = Me.CheckLastServiceDate(udtEHSTransaction.ServiceDate, udtSchemeClaim, udtEHSClaimVaccine)
            End If

            '' ------------------------------------------------------------------
            '' Check Dose Period
            '' ------------------------------------------------------------------
            'If isValid Then
            '    For Each udtEHSClaimVaccineSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
            '        If isValid AndAlso udtEHSClaimVaccineSubsidize.Selected Then

            '            For Each udtEHSClaimSubidizeDetailModel As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimVaccineSubsidize.SubsidizeDetailList
            '                If isValid AndAlso udtEHSClaimSubidizeDetailModel.Selected Then
            '                    Me._udtSystemMessage = udtValidator.chkServiceDateSubsidizeDoseLastServiceDate(udtEHSTransaction.ServiceDate, udtSchemeDetailBLL.getAllSchemeDosePeriod.Filter(udtSchemeClaim.SchemeCode, udtSchemeClaim.SchemeSeq, udtEHSClaimVaccineSubsidize.SubsidizeCode, udtEHSClaimSubidizeDetailModel.AvailableItemCode))
            '                    If Not Me._udtSystemMessage Is Nothing Then
            '                        isValid = False
            '                        Me.imgStep2aServiceDateError.Visible = True
            '                        Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
            '                    End If
            '                End If
            '            Next
            '        End If
            '    Next
            'End If

            ' To Do:
            ' Generic CheckClaimRulesFunction:
            ' will return ClaimResult with appropriate Status for Popup
            ' According to GroupCode / ClaimRulesCode -> Show different Popup

            'Check is Eligible for claim -> Child is 6 years old+ and the first does is avaliable = not Eligible

            If isValid Then
                ' --------------------------------------------------------------
                ' Check Eligibility:
                ' --------------------------------------------------------------
                If udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DocCode = DocTypeModel.DocTypeCode.EC AndAlso udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                    strDOB = _udtFormatter.formatDOB(udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DOB, udtEHSPersonalInfo.ExactDOB, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ECAge, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ECDateOfRegistration)
                Else
                    strDOB = _udtFormatter.formatDOB(udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DOB, udtEHSPersonalInfo.ExactDOB, Nothing, Nothing)
                End If

                Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckEligibilityForEnterClaim(udtSchemeClaim, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo, GetVaccinationRecord(udtEHSAccount, udtSchemeClaim.SchemeCode), udtEligibleResult)

                If Not Me._udtSystemMessage Is Nothing Then
                    ' If Check Eligibility Block Show Error
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                End If

                ' --------------------------------------------------------------
                ' Check Document Limit:
                ' --------------------------------------------------------------
                If isValid Then
                    Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckExceedDocumentLimitForEnterClaim(udtSchemeClaim.SchemeCode, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo)
                    If Not Me._udtSystemMessage Is Nothing Then
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                    End If
                End If

                ' --------------------------------------------------------------
                ' Check Claim Rules:
                ' --------------------------------------------------------------
                If isValid Then
                    Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckClaimRuleForEnterClaim(udtEHSTransaction.ServiceDate, udtEHSAccount, udtEHSPersonalInfo, Me._udtSessionHandler.EHSClaimVaccineGetFromSession(), GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), udtClaimRuleResult, udtInputPicker)
                    If Not Me._udtSystemMessage Is Nothing Then
                        ' If Check Claim Rules Block Show Error
                        isValid = False

                        HandleSystemMessage(Me._udtSystemMessage, udtClaimRuleResult)
                        udcInputPIDVSS.SetDoseErrorImage(True)

                    End If
                End If
            End If

            If isValid Then
                udtRuleResults = Me._udtSessionHandler.EligibleResultGetFromSession()

                If udtRuleResults Is Nothing Then
                    udtRuleResults = New RuleResultCollection()
                End If

                ' --------------------------------------------------------------
                ' Eligibility Warning / Declaration
                ' --------------------------------------------------------------
                If udtEligibleResult.IsEligible AndAlso _
                    (udtEligibleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtEligibleResult.HandleMethod = HandleMethodENum.Warning) Then

                    udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Step2a, udtEligibleResult.RuleType), udtEligibleResult)

                    'strText = Me.GetGlobalResourceObject("Text", "DeclaraChildOverSixYearsOldPromptMessage")
                    If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                        strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleRule.ObjectName.Trim())
                    ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                        strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleExceptionRule.ObjectName.Trim())
                    End If

                    Me.ucNoticePopUpExclamationConfirm.MessageText = strText
                    'Me.lblPopupExclamationConfirmationBoxMessage.Text = strText
                    Me.ModalPopupExclamationConfirmationBox.Show()
                    EHSClaimBasePage.AuditLogShowClaimRulePopupBox(_udtAuditLogEntry)

                    isValid = False
                End If

                ' --------------------------------------------------------------
                ' Claim Rules Warning / Declaration
                ' --------------------------------------------------------------
                If Not udtClaimRuleResult Is Nothing AndAlso Not udtClaimRuleResult.IsBlock AndAlso _
                    (udtClaimRuleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtClaimRuleResult.HandleMethod = HandleMethodENum.Warning) Then

                    udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Step2a, udtClaimRuleResult.RuleType), udtClaimRuleResult)

                    'not Popup prompt before
                    If strText.Equals(String.Empty) AndAlso isValid Then
                        'Get the prompt message from ClaimRule
                        strText = Me.Step2aPromptClaimRule(udtClaimRuleResult)
                        Me.ucNoticePopUpExclamationConfirm.MessageText = strText
                        Me.ModalPopupExclamationConfirmationBox.Show()
                        EHSClaimBasePage.AuditLogShowClaimRulePopupBox(_udtAuditLogEntry)

                    End If

                    isValid = False
                End If

                Me._udtSessionHandler.EligibleResultSaveToSession(udtRuleResults)
            End If
        Else
            strKey = Me.RuleResultKey(ActiveViewIndex.Step2a, RuleTypeENum.EligibleResult)
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
            'isValid = Me.RemoveRulesAfterConfirm(SchemeClaimModel.CIVSS, udtRuleResults, False)
            isValid = Me.RemoveRulesAfterConfirm(udtRuleResults, False)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        End If

        If isValid Then
            udcInputPIDVSS.Save(udtEHSTransaction, udtEHSClaimVaccine)
        End If

        Return isValid
    End Function
    'CRE15-005-03 (New PIDVSS scheme) [End][Chris YIM]

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Function Step2aVSSValidation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = False
        Dim strDOB As String

        Dim udtSchemeDetailBLL As New Common.Component.SchemeDetails.SchemeDetailBLL()
        Dim udtValidator As Validator = New Validator()

        Dim udcInputVSS As ucInputVSS = Me.udcStep2aInputEHSClaim.GetVSSControl
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = Me._udtSessionHandler.EHSClaimVaccineGetFromSession()
        Dim udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel = udtEHSClaimVaccine.SubsidizeList(0)

        Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        ' For Eligible & Claim Rule Warning Checking
        Dim udtEligibleResult As EligibleResult = Nothing
        Dim udtClaimRuleResult As ClaimRuleResult = Nothing
        Dim udtRuleResults As RuleResultCollection = Nothing
        Dim strText As String = String.Empty
        Dim strKey As String = String.Empty
        Dim udtRuleResult As RuleResult = Nothing

        Dim udtInputPicker As New InputPickerModel()
        udtInputPicker.ServiceDate = udtEHSTransaction.ServiceDate
        ' CRE20-014 (Gov SIV 2020/21) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        udtInputPicker.SPID = udtEHSTransaction.ServiceProviderID
        ' CRE20-014 (Gov SIV 2020/21) [End][Chris YIM]

        udcInputVSS.SetDoseErrorImage(False)

        udtRuleResults = Me._udtSessionHandler.EligibleResultGetFromSession()

        If Not checkByConfirmationBox Then

            Me.ClearWarningRules(udtRuleResults)

            ' -----------------------------------------------
            ' UI Input Validation
            '------------------------------------------------

            'Claim Detial Part & Vaccine Part
            isValid = udcInputVSS.Validate(True, Me.udcMsgBoxErr)

            If Me.ClaimMode <> ClaimMode.COVID19 Then
                If isValid Then
                    Dim strHighRisk As String = String.Empty
                    Select Case udcInputVSS.HighRiskOptionShown
                        Case SubsidizeGroupClaimModel.HighRiskOptionClass.ShowForInput 'Manual Input
                            If udcInputVSS.HighRiskOptionEnabled Then
                                strHighRisk = udcInputVSS.HighRisk()
                            End If
                        Case SubsidizeGroupClaimModel.HighRiskOptionClass.HideButForceHighRisk 'Auto Input
                            strHighRisk = YesNo.Yes
                        Case SubsidizeGroupClaimModel.HighRiskOptionClass.HideWithoutInput 'No Input
                            strHighRisk = String.Empty
                        Case Else
                            strHighRisk = String.Empty
                    End Select

                    udtInputPicker.HighRisk = strHighRisk
                End If

            End If

            ' ------------------------------------------------------------------
            ' Check Last Service Date of SubsidizeGroupClaim
            ' ------------------------------------------------------------------
            If isValid Then
                isValid = Me.CheckLastServiceDate(udtEHSTransaction.ServiceDate, udtSchemeClaim, udtEHSClaimVaccine)
            End If

            '' ------------------------------------------------------------------
            '' Check Dose Period
            '' ------------------------------------------------------------------
            'If isValid Then
            '    For Each udtEHSClaimVaccineSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
            '        If isValid AndAlso udtEHSClaimVaccineSubsidize.Selected Then

            '            For Each udtEHSClaimSubidizeDetailModel As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimVaccineSubsidize.SubsidizeDetailList
            '                If isValid AndAlso udtEHSClaimSubidizeDetailModel.Selected Then
            '                    Me._udtSystemMessage = udtValidator.chkServiceDateSubsidizeDoseLastServiceDate(udtEHSTransaction.ServiceDate, udtSchemeDetailBLL.getAllSchemeDosePeriod.Filter(udtSchemeClaim.SchemeCode, udtSchemeClaim.SchemeSeq, udtEHSClaimVaccineSubsidize.SubsidizeCode, udtEHSClaimSubidizeDetailModel.AvailableItemCode))
            '                    If Not Me._udtSystemMessage Is Nothing Then
            '                        isValid = False
            '                        Me.imgStep2aServiceDateError.Visible = True
            '                        Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
            '                    End If
            '                End If
            '            Next
            '        End If
            '    Next
            'End If

            ' To Do:
            ' Generic CheckClaimRulesFunction:
            ' will return ClaimResult with appropriate Status for Popup
            ' According to GroupCode / ClaimRulesCode -> Show different Popup

            'Check is Eligible for claim -> Child is 6 years old+ and the first does is avaliable = not Eligible

            If isValid Then
                ' --------------------------------------------------------------
                ' Check Eligibility:
                ' --------------------------------------------------------------
                If udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DocCode = DocTypeModel.DocTypeCode.EC AndAlso udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                    strDOB = _udtFormatter.formatDOB(udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DOB, udtEHSPersonalInfo.ExactDOB, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ECAge, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ECDateOfRegistration)
                Else
                    strDOB = _udtFormatter.formatDOB(udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DOB, udtEHSPersonalInfo.ExactDOB, Nothing, Nothing)
                End If

                Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckEligibilityForEnterClaim(udtSchemeClaim, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo, GetVaccinationRecord(udtEHSAccount, udtSchemeClaim.SchemeCode), udtEligibleResult)

                If Not Me._udtSystemMessage Is Nothing Then
                    ' If Check Eligibility Block Show Error
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                End If

                ' --------------------------------------------------------------
                ' Check Document Limit:
                ' --------------------------------------------------------------
                If isValid Then
                    Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckExceedDocumentLimitForEnterClaim(udtSchemeClaim.SchemeCode, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo)
                    If Not Me._udtSystemMessage Is Nothing Then
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                    End If
                End If

                ' --------------------------------------------------------------
                ' Check Claim Rules:
                ' --------------------------------------------------------------
                If isValid Then
                    Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckClaimRuleForEnterClaim(udtEHSTransaction.ServiceDate, udtEHSAccount, udtEHSPersonalInfo, Me._udtSessionHandler.EHSClaimVaccineGetFromSession(), GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), udtClaimRuleResult, udtInputPicker)
                    If Not Me._udtSystemMessage Is Nothing Then
                        ' If Check Claim Rules Block Show Error
                        isValid = False

                        HandleSystemMessage(Me._udtSystemMessage, udtClaimRuleResult)
                        udcInputVSS.SetDoseErrorImage(True)

                    End If
                End If
            End If

            If isValid Then
                udtRuleResults = Me._udtSessionHandler.EligibleResultGetFromSession()

                If udtRuleResults Is Nothing Then
                    udtRuleResults = New RuleResultCollection()
                End If

                ' --------------------------------------------------------------
                ' Eligibility Warning / Declaration
                ' --------------------------------------------------------------
                If udtEligibleResult.IsEligible AndAlso _
                    (udtEligibleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtEligibleResult.HandleMethod = HandleMethodENum.Warning) Then

                    'Dim drClaimCategory As DataRow = (New ClaimCategoryBLL).getCategoryDesc(MyBase.EHSTransaction.CategoryCode)
                    Dim strCategoryCode As String = String.Empty
                    Dim strCategoryCodeForRule As String = String.Empty
                    Dim strCategoryCodeForExceptionRule As String = String.Empty
                    Dim udtClaimCategoryModelCollection As ClaimCategoryModelCollection = (New ClaimCategoryBLL).getAllCategoryCache
                    For Each udtClaimCategoryModel As ClaimCategoryModel In udtClaimCategoryModelCollection
                        If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                            If udtClaimCategoryModel.SchemeCode = udtEligibleResult.RelatedEligibleRule.SchemeCode And _
                                udtClaimCategoryModel.SchemeSeq = udtEligibleResult.RelatedEligibleRule.SchemeSeq And _
                                udtClaimCategoryModel.SubsidizeCode = udtEligibleResult.RelatedEligibleRule.SubsidizeCode Then
                                strCategoryCodeForRule = udtClaimCategoryModel.CategoryCode
                            End If
                        ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                            If udtClaimCategoryModel.SchemeCode = udtEligibleResult.RelatedEligibleExceptionRule.SchemeCode And _
                                udtClaimCategoryModel.SchemeSeq = udtEligibleResult.RelatedEligibleExceptionRule.SchemeSeq And _
                                udtClaimCategoryModel.SubsidizeCode = udtEligibleResult.RelatedEligibleExceptionRule.SubsidizeCode Then
                                strCategoryCodeForExceptionRule = udtClaimCategoryModel.CategoryCode
                            End If
                        End If
                    Next

                    If strCategoryCodeForRule <> String.Empty And strCategoryCodeForExceptionRule <> String.Empty And _
                        strCategoryCodeForRule = strCategoryCodeForExceptionRule Then
                        strCategoryCode = strCategoryCodeForRule
                    End If

                    If strCategoryCodeForRule <> String.Empty Then
                        strCategoryCode = strCategoryCodeForRule
                    End If

                    If strCategoryCodeForExceptionRule <> String.Empty Then
                        strCategoryCode = strCategoryCodeForExceptionRule
                    End If

                    If strCategoryCode = udcInputVSS.Category Then
                        udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Step2a, udtEligibleResult.RuleType), udtEligibleResult)

                        If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                            strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleRule.ObjectName.Trim())
                        ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                            strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleExceptionRule.ObjectName.Trim())
                        End If

                        Me.ucNoticePopUpExclamationConfirm.MessageText = strText

                        Me.ModalPopupExclamationConfirmationBox.Show()

                        _udtAuditLogEntry.AddDescripton("Message", strText)
                        EHSClaimBasePage.AuditLogShowClaimRulePopupBox(_udtAuditLogEntry)

                        isValid = False
                    End If

                End If

                ' --------------------------------------------------------------
                ' Claim Rules Warning / Declaration
                ' --------------------------------------------------------------
                If Not udtClaimRuleResult Is Nothing AndAlso Not udtClaimRuleResult.IsBlock AndAlso _
                    (udtClaimRuleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtClaimRuleResult.HandleMethod = HandleMethodENum.Warning) Then

                    udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Step2a, udtClaimRuleResult.RuleType), udtClaimRuleResult)

                    'not Popup prompt before
                    If strText.Equals(String.Empty) AndAlso isValid Then
                        'Get the prompt message from ClaimRule
                        strText = Me.Step2aPromptClaimRule(udtClaimRuleResult)
                        Me.ucNoticePopUpExclamationConfirm.MessageText = strText
                        Me.ModalPopupExclamationConfirmationBox.Show()

                        _udtAuditLogEntry.AddDescripton("Message", strText)
                        EHSClaimBasePage.AuditLogShowClaimRulePopupBox(_udtAuditLogEntry)

                    End If

                    isValid = False
                End If

                Me._udtSessionHandler.EligibleResultSaveToSession(udtRuleResults)
            End If
        Else
            strKey = Me.RuleResultKey(ActiveViewIndex.Step2a, RuleTypeENum.EligibleResult)
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
            'isValid = Me.RemoveRulesAfterConfirm(SchemeClaimModel.CIVSS, udtRuleResults, False)
            isValid = Me.RemoveRulesAfterConfirm(udtRuleResults, False)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        End If

        If isValid Then
            udcInputVSS.Save(udtEHSTransaction, udtEHSClaimVaccine)
        End If

        Return isValid
    End Function
    'CRE16-002 (Revamp VSS) [End][Chris YIM]

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Private Function Step2aVSSCOVID19Validation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = False
        Dim strDOB As String

        Dim udtSchemeDetailBLL As New Common.Component.SchemeDetails.SchemeDetailBLL()
        Dim udtValidator As Validator = New Validator()

        Dim udcInputVSSCOVID19 As ucInputVSSCOVID19 = Me.udcStep2aInputEHSClaim.GetVSSCOVID19Control
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = Me._udtSessionHandler.EHSClaimVaccineGetFromSession()
        Dim udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel = udtEHSClaimVaccine.SubsidizeList(0)

        Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        ' For Eligible & Claim Rule Warning Checking
        Dim udtEligibleResult As EligibleResult = Nothing
        Dim udtClaimRuleResultList As New List(Of ClaimRuleResult)
        Dim udtRuleResults As RuleResultCollection = Nothing
        Dim strHeader As String = String.Empty
        Dim strText As String = String.Empty
        Dim strKey As String = String.Empty
        Dim udtRuleResult As RuleResult = Nothing

        Dim udtInputPicker As New InputPickerModel()
        udtInputPicker.ServiceDate = udtEHSTransaction.ServiceDate
        udtInputPicker.SPID = udtEHSTransaction.ServiceProviderID
        udtInputPicker.PracticeDisplaySeq = udtEHSTransaction.PracticeID

        udcInputVSSCOVID19.SetDoseErrorImage(False)

        udtRuleResults = Me._udtSessionHandler.EligibleResultGetFromSession()

        If Not checkByConfirmationBox Then

            Me.ClearWarningRules(udtRuleResults)

            ' -----------------------------------------------
            ' UI Input Validation
            '------------------------------------------------

            'Claim Detial Part & Vaccine Part
            isValid = udcInputVSSCOVID19.Validate(True, Me.udcMsgBoxErr)

            If panStep2aRecipinetContactInfo.Visible Then
                If Me.txtStep2aContactNo.Enabled Then
                    'If String.IsNullOrEmpty(Me.txtStep2aContactNo.Text) AndAlso Me.chkStep2aMobile.Checked Then
                    If String.IsNullOrEmpty(Me.txtStep2aContactNo.Text) Then
                        isValid = False

                        imgStep2aContactNoError.Visible = True

                        Dim udtMsg As SystemMessage = New SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00463)
                        If Me.udcMsgBoxErr IsNot Nothing Then Me.udcMsgBoxErr.AddMessage(udtMsg, _
                                                                                        New String() {"%en", "%tc", "%sc"}, _
                                                                                        New String() {HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.English)), _
                                                                                                      HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)), _
                                                                                                      HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese))})
                    End If

                    If Not String.IsNullOrEmpty(Me.txtStep2aContactNo.Text) Then
                        If Not Regex.IsMatch(Me.txtStep2aContactNo.Text, "^[2-9]\d{7}$") Then
                            isValid = False

                            imgStep2aContactNoError.Visible = True

                            Dim udtMsg As SystemMessage = New SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00466)
                            If Me.udcMsgBoxErr IsNot Nothing Then Me.udcMsgBoxErr.AddMessage(udtMsg, _
                                                                                        New String() {"%en", "%tc", "%sc"}, _
                                                                                        New String() {HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.English)), _
                                                                                                      HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)), _
                                                                                                      HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese))})
                        End If

                    End If

                End If

            End If

            'If Not Me.chkStep2aDeclareClaim.Checked Then
            '    isValid = False

            '    imgStep2aDeclareClaimError.Visible = True

            '    Dim udtMsg As SystemMessage = New SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00367)
            '    If Me.udcMsgBoxErr IsNot Nothing Then Me.udcMsgBoxErr.AddMessage(udtMsg, "%s", "Verification Checklist")
            'End If

        End If

        'Mark selected vaccine & dose in EHSClaimVaccineModel model
        If isValid OrElse checkByConfirmationBox Then
            udtInputPicker.Brand = udcInputVSSCOVID19.VaccineBrand
            udtInputPicker.VaccinationRecord = GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode)

            Dim udtLatestC19Vaccine As TransactionDetailVaccineModel = Nothing
            Dim udtLatestC19Transaction As EHSTransactionModel = Nothing
            Dim udtTranDetailVaccineList As TransactionDetailVaccineModelCollection = GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode)
            Dim udtC19VaccineList As TransactionDetailVaccineModelCollection = udtTranDetailVaccineList.FilterIncludeBySubsidizeItemCode(SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19)

            For Each udtC19Vaccine As TransactionDetailVaccineModel In udtC19VaccineList
                'Find the latest COVID19 transaction in EHS
                If udtC19Vaccine.AvailableItemCode.Trim.ToUpper = "1STDOSE" Then
                    If udtLatestC19Vaccine Is Nothing Then
                        udtLatestC19Vaccine = udtC19Vaccine
                    Else
                        If udtC19Vaccine.ServiceReceiveDtm > udtLatestC19Vaccine.ServiceReceiveDtm Then
                            udtLatestC19Vaccine = udtC19Vaccine
                        End If
                    End If
                End If
            Next

            'Only allow EHS Transaction, not include CMS / CIMS record
            If udtLatestC19Vaccine IsNot Nothing AndAlso udtLatestC19Vaccine.TransactionID <> String.Empty Then
                udtLatestC19Transaction = _udtEHSTransactionBLL.LoadClaimTran(udtLatestC19Vaccine.TransactionID.Trim)
                udtInputPicker.LatestC19Transaction = udtLatestC19Transaction
            End If

            udcInputVSSCOVID19.Selection()
        End If

        If Not checkByConfirmationBox Then
            ' ------------------------------------------------------------------
            ' Check Last Service Date of SubsidizeGroupClaim
            ' ------------------------------------------------------------------
            If isValid Then
                isValid = Me.CheckLastServiceDate(udtEHSTransaction.ServiceDate, udtSchemeClaim, udtEHSClaimVaccine)
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

                Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckEligibilityForEnterClaim(udtSchemeClaim, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo, GetVaccinationRecord(udtEHSAccount, udtSchemeClaim.SchemeCode), udtEligibleResult)

                If Not Me._udtSystemMessage Is Nothing Then
                    ' If Check Eligibility Block Show Error
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                End If

                ' --------------------------------------------------------------
                ' Check Document Limit:
                ' --------------------------------------------------------------
                If isValid Then
                    Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckExceedDocumentLimitForEnterClaim(udtSchemeClaim.SchemeCode, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo)
                    If Not Me._udtSystemMessage Is Nothing Then
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                    End If
                End If

                ' --------------------------------------------------------------
                ' Check Claim Rules:
                ' --------------------------------------------------------------
                If isValid Then
                    Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckClaimRuleForEnterClaimCOVID19(udtEHSTransaction.ServiceDate, _
                                                                                                 udtEHSAccount, _
                                                                                                 udtEHSPersonalInfo, _
                                                                                                 Me._udtSessionHandler.EHSClaimVaccineGetFromSession(), _
                                                                                                 GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), _
                                                                                                 udtClaimRuleResultList, _
                                                                                                 udtInputPicker)
                    If Not Me._udtSystemMessage Is Nothing Then
                        ' If Check Claim Rules Block Show Error
                        isValid = False

                        For Each udtClaimRuleResult As ClaimRuleResult In udtClaimRuleResultList
                            HandleSystemMessage(Me._udtSystemMessage, udtClaimRuleResult)
                        Next

                        udcInputVSSCOVID19.SetDoseErrorImage(True)

                        If _udtSystemMessage.FunctionCode = "990000" AndAlso _udtSystemMessage.SeverityCode = "E" AndAlso _udtSystemMessage.MessageCode = "00464" Then
                            udcInputVSSCOVID19.SetVaccineBrandError(True)
                        End If
                    End If
                End If
            End If

            If isValid Then
                udtRuleResults = Me._udtSessionHandler.EligibleResultGetFromSession()

                If udtRuleResults Is Nothing Then
                    udtRuleResults = New RuleResultCollection()
                End If

                ' --------------------------------------------------------------
                ' Eligibility Warning / Declaration
                ' --------------------------------------------------------------
                If udtEligibleResult.IsEligible AndAlso _
                    (udtEligibleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtEligibleResult.HandleMethod = HandleMethodENum.Warning) Then

                    'Dim drClaimCategory As DataRow = (New ClaimCategoryBLL).getCategoryDesc(MyBase.EHSTransaction.CategoryCode)
                    Dim strCategoryCode As String = String.Empty
                    Dim strCategoryCodeForRule As String = String.Empty
                    Dim strCategoryCodeForExceptionRule As String = String.Empty
                    Dim udtClaimCategoryModelCollection As ClaimCategoryModelCollection = (New ClaimCategoryBLL).getAllCategoryCache
                    For Each udtClaimCategoryModel As ClaimCategoryModel In udtClaimCategoryModelCollection
                        If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                            If udtClaimCategoryModel.SchemeCode = udtEligibleResult.RelatedEligibleRule.SchemeCode And _
                                udtClaimCategoryModel.SchemeSeq = udtEligibleResult.RelatedEligibleRule.SchemeSeq And _
                                udtClaimCategoryModel.SubsidizeCode = udtEligibleResult.RelatedEligibleRule.SubsidizeCode Then
                                strCategoryCodeForRule = udtClaimCategoryModel.CategoryCode
                            End If
                        ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                            If udtClaimCategoryModel.SchemeCode = udtEligibleResult.RelatedEligibleExceptionRule.SchemeCode And _
                                udtClaimCategoryModel.SchemeSeq = udtEligibleResult.RelatedEligibleExceptionRule.SchemeSeq And _
                                udtClaimCategoryModel.SubsidizeCode = udtEligibleResult.RelatedEligibleExceptionRule.SubsidizeCode Then
                                strCategoryCodeForExceptionRule = udtClaimCategoryModel.CategoryCode
                            End If
                        End If
                    Next

                    If strCategoryCodeForRule <> String.Empty And strCategoryCodeForExceptionRule <> String.Empty And _
                        strCategoryCodeForRule = strCategoryCodeForExceptionRule Then
                        strCategoryCode = strCategoryCodeForRule
                    End If

                    If strCategoryCodeForRule <> String.Empty Then
                        strCategoryCode = strCategoryCodeForRule
                    End If

                    If strCategoryCodeForExceptionRule <> String.Empty Then
                        strCategoryCode = strCategoryCodeForExceptionRule
                    End If

                    If strCategoryCode = udcInputVSSCOVID19.CategoryForCOVID19 Then
                        udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Step2a, udtEligibleResult.RuleType), udtEligibleResult)

                        If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                            strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleRule.ObjectName.Trim())
                        ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                            strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleExceptionRule.ObjectName.Trim())
                        End If

                        Me.ucNoticePopUpExclamationConfirm.MessageText = strText

                        Me.ModalPopupExclamationConfirmationBox.Show()

                        _udtAuditLogEntry.AddDescripton("Message", strText)
                        EHSClaimBasePage.AuditLogShowClaimRulePopupBox(_udtAuditLogEntry)

                        isValid = False
                    End If

                End If

                ' --------------------------------------------------------------
                ' Claim Rules Warning / Declaration
                ' --------------------------------------------------------------
                If udtClaimRuleResultList IsNot Nothing AndAlso udtClaimRuleResultList.Count > 0 Then
                    Dim blnError As Boolean = False
                    Dim blnWarning As Boolean = False

                    For Each udtClaimRuleResult As ClaimRuleResult In udtClaimRuleResultList
                        If udtClaimRuleResult.IsBlock Then
                            blnError = True
                        End If

                        If Not udtClaimRuleResult.IsBlock AndAlso _
                            (udtClaimRuleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtClaimRuleResult.HandleMethod = HandleMethodENum.Warning) Then
                            blnWarning = True
                        End If
                    Next

                    If Not blnError AndAlso blnWarning Then
                        udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Step2a, udtClaimRuleResultList(0).RuleType), udtClaimRuleResultList(0))

                        'not Popup prompt before
                        If strText.Equals(String.Empty) AndAlso isValid Then
                            Dim strHTMLList As String = Me.Step2aPromptContent(udtClaimRuleResultList)

                            'Get the prompt message from ClaimRule
                            If udtClaimRuleResultList.Count = 1 Then
                                strHeader = Me.Step2aPromptHeader(udtClaimRuleResultList(0))
                            End If

                            If strHeader <> String.Empty Then
                                Me.ucNoticePopUpExclamationConfirm.CustomHeaderText = strHeader
                            End If

                            Me.ucNoticePopUpExclamationConfirm.MessageText = strHTMLList

                        	Me.ModalPopupExclamationConfirmationBox.Show()

                        	_udtAuditLogEntry.AddDescripton("Message", strText)
                        	EHSClaimBasePage.AuditLogShowClaimRulePopupBox(_udtAuditLogEntry)

                   		End If

                    	isValid = False
                	End If
                
				End If
				
                Me._udtSessionHandler.EligibleResultSaveToSession(udtRuleResults)
            End If
        Else
            strKey = Me.RuleResultKey(ActiveViewIndex.Step2a, RuleTypeENum.EligibleResult)
            udtRuleResult = udtRuleResults.Item(strKey)

            If Not udtRuleResult Is Nothing Then

                'Should have 2 rule in this collection
                'first : After sreach this account -> rule added and auto make to confirmed
                'Second : After press "Next" in Enter claim detail -> rule added but not confirmed
                If Not udtRuleResult.PromptConfirmed Then
                    udtEHSTransaction.PreSchool = "Y"
                End If
            End If

            isValid = Me.RemoveRulesAfterConfirm(udtRuleResults, False)
        End If

        If isValid Then
            udcInputVSSCOVID19.Save(udtEHSTransaction, udtEHSClaimVaccine)

            'Clear old vaccination record for vaccination card
            _udtSessionHandler.ClaimCOVID19VaccinationCardRemoveFromSession(FunctionCode)

            'Find the nearest vaccination record for vaccination card
            Dim udtVaccinationRecordList As TransactionDetailVaccineModelCollection = Nothing
            udtVaccinationRecordList = GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode).FilterIncludeBySubsidizeItemCode(SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19)

            If udtVaccinationRecordList.Count > 0 Then
                Dim udtVaccinationRecord As TransactionDetailVaccineModel = udtVaccinationRecordList.FilterFindNearestRecord()

                If udtEHSClaimVaccine.GetSelectedDoseForCOVID19.AvailableItemCode.Trim().ToUpper() <> udtVaccinationRecord.AvailableItemCode.Trim.Trim().ToUpper() Then
                    'If udtInputPicker.Brand.Trim = udtVaccinationRecord.VaccineBrand.Trim.Trim().ToUpper() Then
                    _udtSessionHandler.ClaimCOVID19VaccinationCardSaveToSession(udtVaccinationRecord, FunctionCode)
                    'End If

                End If

            End If

            If Me.ClaimMode = ClaimMode.COVID19 AndAlso panStep2aRecipinetContactInfo.Visible Then
                Dim udtTransactAdditionfield As TransactionAdditionalFieldModel

                If Me._udtEHSTransaction.TransactionAdditionFields(0) IsNot Nothing Then
                    'Contact No.
                    udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                    udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ContactNo
                    udtTransactAdditionfield.AdditionalFieldValueCode = txtStep2aContactNo.Text.Trim
                    udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
                    udtTransactAdditionfield.SchemeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeCode
                    udtTransactAdditionfield.SchemeSeq = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeSeq
                    udtTransactAdditionfield.SubsidizeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SubsidizeCode
                    Me._udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

                    ''Mobile
                    'udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                    'udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.Mobile
                    'udtTransactAdditionfield.AdditionalFieldValueCode = IIf(Me.chkStep2aMobile.Checked, YesNo.Yes, YesNo.No)
                    'udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
                    'udtTransactAdditionfield.SchemeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeCode
                    'udtTransactAdditionfield.SchemeSeq = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeSeq
                    'udtTransactAdditionfield.SubsidizeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SubsidizeCode
                    'Me._udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

                    'Remarks
                    udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                    udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.Remarks
                    udtTransactAdditionfield.AdditionalFieldValueCode = String.Empty
                    udtTransactAdditionfield.AdditionalFieldValueDesc = txtStep2aRemark.Text.Trim
                    udtTransactAdditionfield.SchemeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeCode
                    udtTransactAdditionfield.SchemeSeq = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeSeq
                    udtTransactAdditionfield.SubsidizeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SubsidizeCode
                    Me._udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

                    'JoinEHRSS
                    Dim strJoinEHRSS As String = String.Empty

                    If panStep2aDeclareJoineHRSS.Visible Then
                        strJoinEHRSS = IIf(chkStep2aDeclareJoineHRSS.Checked, YesNo.Yes, YesNo.No)
                    End If

                    udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                    udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.JoinEHRSS
                    udtTransactAdditionfield.AdditionalFieldValueCode = strJoinEHRSS
                    udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
                    udtTransactAdditionfield.SchemeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeCode
                    udtTransactAdditionfield.SchemeSeq = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeSeq
                    udtTransactAdditionfield.SubsidizeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SubsidizeCode
                    Me._udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
                End If
            End If

        End If

        Return isValid
    End Function
    ' CRE20-0022 (Immu record) [End][Chris YIM]

    ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
    ' --------------------------------------------------------------------------------------
    Private Function Step2aCOVID19Validation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = False
        Dim strDOB As String

        Dim udtSchemeDetailBLL As New Common.Component.SchemeDetails.SchemeDetailBLL()
        Dim udtValidator As Validator = New Validator()

        Dim udcInputCOVID19 As ucInputCOVID19 = Me.udcStep2aInputEHSClaim.GetCOVID19Control
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = Me._udtSessionHandler.EHSClaimVaccineGetFromSession()
        Dim udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel = udtEHSClaimVaccine.SubsidizeList(0)

        Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        ' For Eligible & Claim Rule Warning Checking
        Dim udtEligibleResult As EligibleResult = Nothing
        Dim udtClaimRuleResultList As New List(Of ClaimRuleResult)
        Dim udtRuleResults As RuleResultCollection = Nothing
        Dim strHeader As String = String.Empty
        Dim strText As String = String.Empty
        Dim strKey As String = String.Empty
        Dim udtRuleResult As RuleResult = Nothing

        Dim udtInputPicker As New InputPickerModel()
        udtInputPicker.ServiceDate = udtEHSTransaction.ServiceDate
        udtInputPicker.SPID = udtEHSTransaction.ServiceProviderID

        udcInputCOVID19.SetDoseErrorImage(False)

        udtRuleResults = Me._udtSessionHandler.EligibleResultGetFromSession()

        If Not checkByConfirmationBox Then

            Me.ClearWarningRules(udtRuleResults)

            ' -----------------------------------------------
            ' UI Input Validation
            '------------------------------------------------

            'Claim Detial Part & Vaccine Part
            isValid = udcInputCOVID19.Validate(True, Me.udcMsgBoxErr)

            If panStep2aRecipinetContactInfo.Visible Then
                If String.IsNullOrEmpty(Me.txtStep2aContactNo.Text) AndAlso Me.chkStep2aMobile.Checked Then
                    isValid = False

                    imgStep2aContactNoError.Visible = True

                    Dim udtMsg As SystemMessage = New SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00463)
                    If Me.udcMsgBoxErr IsNot Nothing Then Me.udcMsgBoxErr.AddMessage(udtMsg, _
                                                                                    New String() {"%en", "%tc", "%sc"}, _
                                                                                    New String() {HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.English)), _
                                                                                                  HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)), _
                                                                                                  HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese))})
                End If

                If Not String.IsNullOrEmpty(Me.txtStep2aContactNo.Text) Then
                    If Not Regex.IsMatch(Me.txtStep2aContactNo.Text, "^[2-9]\d{7}$") Then
                        isValid = False

                        imgStep2aContactNoError.Visible = True

                        Dim udtMsg As SystemMessage = New SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00466)
                        If Me.udcMsgBoxErr IsNot Nothing Then Me.udcMsgBoxErr.AddMessage(udtMsg, _
                                                                                    New String() {"%en", "%tc", "%sc"}, _
                                                                                    New String() {HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.English)), _
                                                                                                  HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)), _
                                                                                                  HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese))})
                    End If

                End If

            End If

            'If Not Me.chkStep2aDeclareClaim.Checked Then
            '    isValid = False

            '    imgStep2aDeclareClaimError.Visible = True

            '    Dim udtMsg As SystemMessage = New SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00367)
            '    If Me.udcMsgBoxErr IsNot Nothing Then Me.udcMsgBoxErr.AddMessage(udtMsg, "%s", "Verification Checklist")
            'End If

        End If

        'Mark selected vaccine & dose in EHSClaimVaccineModel model
        If isValid OrElse checkByConfirmationBox Then
            udtInputPicker.Brand = udcInputCOVID19.VaccineBrand
            udtInputPicker.VaccinationRecord = GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode)
            udcInputCOVID19.Selection()
        End If

        If Not checkByConfirmationBox Then
            ' ------------------------------------------------------------------
            ' Check Last Service Date of SubsidizeGroupClaim
            ' ------------------------------------------------------------------
            If isValid Then
                isValid = Me.CheckLastServiceDate(udtEHSTransaction.ServiceDate, udtSchemeClaim, udtEHSClaimVaccine)
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

                Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckEligibilityForEnterClaim(udtSchemeClaim, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo, GetVaccinationRecord(udtEHSAccount, udtSchemeClaim.SchemeCode), udtEligibleResult)

                If Not Me._udtSystemMessage Is Nothing Then
                    ' If Check Eligibility Block Show Error
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                End If

                ' --------------------------------------------------------------
                ' Check Document Limit:
                ' --------------------------------------------------------------
                If isValid Then
                    Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckExceedDocumentLimitForEnterClaim(udtSchemeClaim.SchemeCode, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo)
                    If Not Me._udtSystemMessage Is Nothing Then
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                    End If
                End If

                ' --------------------------------------------------------------
                ' Check Claim Rules:
                ' --------------------------------------------------------------
                If isValid Then
                    Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckClaimRuleForEnterClaimCOVID19(udtEHSTransaction.ServiceDate, _
                                                                                                 udtEHSAccount, _
                                                                                                 udtEHSPersonalInfo, _
                                                                                                 Me._udtSessionHandler.EHSClaimVaccineGetFromSession(), _
                                                                                                 GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), _
                                                                                                 udtClaimRuleResultList, _
                                                                                                 udtInputPicker)
                    If Not Me._udtSystemMessage Is Nothing Then
                        ' If Check Claim Rules Block Show Error
                        isValid = False

                        For Each udtClaimRuleResult As ClaimRuleResult In udtClaimRuleResultList
                            HandleSystemMessage(Me._udtSystemMessage, udtClaimRuleResult)
                        Next

                        udcInputCOVID19.SetDoseErrorImage(True)

                        If _udtSystemMessage.FunctionCode = "990000" AndAlso _udtSystemMessage.SeverityCode = "E" AndAlso _udtSystemMessage.MessageCode = "00464" Then
                            udcInputCOVID19.SetVaccineBrandError(True)
                        End If
                    End If
                End If
            End If

            If isValid Then
                udtRuleResults = Me._udtSessionHandler.EligibleResultGetFromSession()

                If udtRuleResults Is Nothing Then
                    udtRuleResults = New RuleResultCollection()
                End If

                ' --------------------------------------------------------------
                ' Eligibility Warning / Declaration
                ' --------------------------------------------------------------
                If udtEligibleResult.IsEligible AndAlso _
                    (udtEligibleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtEligibleResult.HandleMethod = HandleMethodENum.Warning) Then

                    'Dim drClaimCategory As DataRow = (New ClaimCategoryBLL).getCategoryDesc(MyBase.EHSTransaction.CategoryCode)
                    Dim strCategoryCode As String = String.Empty
                    Dim strCategoryCodeForRule As String = String.Empty
                    Dim strCategoryCodeForExceptionRule As String = String.Empty
                    Dim udtClaimCategoryModelCollection As ClaimCategoryModelCollection = (New ClaimCategoryBLL).getAllCategoryCache
                    For Each udtClaimCategoryModel As ClaimCategoryModel In udtClaimCategoryModelCollection
                        If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                            If udtClaimCategoryModel.SchemeCode = udtEligibleResult.RelatedEligibleRule.SchemeCode And _
                                udtClaimCategoryModel.SchemeSeq = udtEligibleResult.RelatedEligibleRule.SchemeSeq And _
                                udtClaimCategoryModel.SubsidizeCode = udtEligibleResult.RelatedEligibleRule.SubsidizeCode Then
                                strCategoryCodeForRule = udtClaimCategoryModel.CategoryCode
                            End If
                        ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                            If udtClaimCategoryModel.SchemeCode = udtEligibleResult.RelatedEligibleExceptionRule.SchemeCode And _
                                udtClaimCategoryModel.SchemeSeq = udtEligibleResult.RelatedEligibleExceptionRule.SchemeSeq And _
                                udtClaimCategoryModel.SubsidizeCode = udtEligibleResult.RelatedEligibleExceptionRule.SubsidizeCode Then
                                strCategoryCodeForExceptionRule = udtClaimCategoryModel.CategoryCode
                            End If
                        End If
                    Next

                    If strCategoryCodeForRule <> String.Empty And strCategoryCodeForExceptionRule <> String.Empty And _
                        strCategoryCodeForRule = strCategoryCodeForExceptionRule Then
                        strCategoryCode = strCategoryCodeForRule
                    End If

                    If strCategoryCodeForRule <> String.Empty Then
                        strCategoryCode = strCategoryCodeForRule
                    End If

                    If strCategoryCodeForExceptionRule <> String.Empty Then
                        strCategoryCode = strCategoryCodeForExceptionRule
                    End If

                    If strCategoryCode = udcInputCOVID19.CategoryForCOVID19 Then
                        udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Step2a, udtEligibleResult.RuleType), udtEligibleResult)

                        If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                            strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleRule.ObjectName.Trim())
                        ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                            strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleExceptionRule.ObjectName.Trim())
                        End If

                        Me.ucNoticePopUpExclamationConfirm.MessageText = strText

                        Me.ModalPopupExclamationConfirmationBox.Show()

                        _udtAuditLogEntry.AddDescripton("Message", strText)
                        EHSClaimBasePage.AuditLogShowClaimRulePopupBox(_udtAuditLogEntry)

                        isValid = False
                    End If

                End If

                ' --------------------------------------------------------------
                ' Claim Rules Warning / Declaration
                ' --------------------------------------------------------------
                If udtClaimRuleResultList IsNot Nothing AndAlso udtClaimRuleResultList.Count > 0 Then
                    Dim blnError As Boolean = False
                    Dim blnWarning As Boolean = False

                    For Each udtClaimRuleResult As ClaimRuleResult In udtClaimRuleResultList
                        If udtClaimRuleResult.IsBlock Then
                            blnError = True
                        End If

                        If Not udtClaimRuleResult.IsBlock AndAlso _
                            (udtClaimRuleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtClaimRuleResult.HandleMethod = HandleMethodENum.Warning) Then
                            blnWarning = True
                        End If
                    Next

                    If Not blnError AndAlso blnWarning Then
                        udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Step2a, udtClaimRuleResultList(0).RuleType), udtClaimRuleResultList(0))

                        'not Popup prompt before
                        If strText.Equals(String.Empty) AndAlso isValid Then
                            Dim strHTMLList As String = Me.Step2aPromptContent(udtClaimRuleResultList)

                            'Get the prompt message from ClaimRule
                            If udtClaimRuleResultList.Count = 1 Then
                                strHeader = Me.Step2aPromptHeader(udtClaimRuleResultList(0))
                            End If

                            If strHeader <> String.Empty Then
                                Me.ucNoticePopUpExclamationConfirm.CustomHeaderText = strHeader
                            End If

                            Me.ucNoticePopUpExclamationConfirm.MessageText = strHTMLList

                        	Me.ModalPopupExclamationConfirmationBox.Show()

                        	_udtAuditLogEntry.AddDescripton("Message", strText)
                        	EHSClaimBasePage.AuditLogShowClaimRulePopupBox(_udtAuditLogEntry)

                    	End If

                    	isValid = False
                	End If
                	
                End If	

                Me._udtSessionHandler.EligibleResultSaveToSession(udtRuleResults)
            End If
        Else
            strKey = Me.RuleResultKey(ActiveViewIndex.Step2a, RuleTypeENum.EligibleResult)
            udtRuleResult = udtRuleResults.Item(strKey)

            If Not udtRuleResult Is Nothing Then

                'Should have 2 rule in this collection
                'first : After sreach this account -> rule added and auto make to confirmed
                'Second : After press "Next" in Enter claim detail -> rule added but not confirmed
                If Not udtRuleResult.PromptConfirmed Then
                    udtEHSTransaction.PreSchool = "Y"
                End If
            End If

            isValid = Me.RemoveRulesAfterConfirm(udtRuleResults, False)
        End If

        If isValid Then
            udcInputCOVID19.Save(udtEHSTransaction, udtEHSClaimVaccine)

            'Clear old vaccination record for vaccination card
            _udtSessionHandler.ClaimCOVID19VaccinationCardRemoveFromSession(FunctionCode)

            'Find the nearest vaccination record for vaccination card
            Dim udtVaccinationRecordList As TransactionDetailVaccineModelCollection = Nothing
            udtVaccinationRecordList = GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode).FilterIncludeBySubsidizeItemCode(SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19)

            If udtVaccinationRecordList.Count > 0 Then
                Dim udtVaccinationRecord As TransactionDetailVaccineModel = udtVaccinationRecordList.FilterFindNearestRecord()

                If udtEHSClaimVaccine.GetSelectedDoseForCOVID19.AvailableItemCode.Trim().ToUpper() <> udtVaccinationRecord.AvailableItemCode.Trim.Trim().ToUpper() Then
                    'If udtInputPicker.Brand.Trim = udtVaccinationRecord.VaccineBrand.Trim.Trim().ToUpper() Then
                    _udtSessionHandler.ClaimCOVID19VaccinationCardSaveToSession(udtVaccinationRecord, FunctionCode)
                    'End If

                End If

            End If

            If Me.ClaimMode = ClaimMode.COVID19 AndAlso panStep2aRecipinetContactInfo.Visible Then
                Dim udtTransactAdditionfield As TransactionAdditionalFieldModel

                If Me._udtEHSTransaction.TransactionAdditionFields(0) IsNot Nothing Then
                    ''Contact No.
                    'udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                    'udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ContactNo
                    'udtTransactAdditionfield.AdditionalFieldValueCode = txtStep2aContactNo.Text.Trim
                    'udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
                    'udtTransactAdditionfield.SchemeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeCode
                    'udtTransactAdditionfield.SchemeSeq = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeSeq
                    'udtTransactAdditionfield.SubsidizeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SubsidizeCode
                    'Me._udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

                    ''Mobile
                    'udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                    'udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.Mobile
                    'udtTransactAdditionfield.AdditionalFieldValueCode = IIf(Me.chkStep2aMobile.Checked, YesNo.Yes, YesNo.No)
                    'udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
                    'udtTransactAdditionfield.SchemeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeCode
                    'udtTransactAdditionfield.SchemeSeq = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeSeq
                    'udtTransactAdditionfield.SubsidizeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SubsidizeCode
                    'Me._udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

                    'Remarks
                    udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                    udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.Remarks
                    udtTransactAdditionfield.AdditionalFieldValueCode = String.Empty
                    udtTransactAdditionfield.AdditionalFieldValueDesc = txtStep2aRemark.Text.Trim
                    udtTransactAdditionfield.SchemeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeCode
                    udtTransactAdditionfield.SchemeSeq = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeSeq
                    udtTransactAdditionfield.SubsidizeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SubsidizeCode
                    Me._udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

                    'JoinEHRSS
                    Dim strJoinEHRSS As String = String.Empty

                    If panStep2aDeclareJoineHRSS.Visible Then
                        strJoinEHRSS = IIf(chkStep2aDeclareJoineHRSS.Checked, YesNo.Yes, YesNo.No)
                    End If

                    udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                    udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.JoinEHRSS
                    udtTransactAdditionfield.AdditionalFieldValueCode = strJoinEHRSS
                    udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
                    udtTransactAdditionfield.SchemeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeCode
                    udtTransactAdditionfield.SchemeSeq = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeSeq
                    udtTransactAdditionfield.SubsidizeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SubsidizeCode
                    Me._udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
                End If
            End If

        End If

        Return isValid
    End Function
    ' CRE20-0022 (Immu record) [End][Winnie SUEN]

    ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
    ' --------------------------------------------------------------------------------------
    Private Function Step2aCOVID19RVPValidation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = False
        Dim strDOB As String

        Dim udtSchemeDetailBLL As New Common.Component.SchemeDetails.SchemeDetailBLL()
        Dim udtValidator As Validator = New Validator()

        Dim udcInputCOVID19RVP As ucInputCOVID19RVP = Me.udcStep2aInputEHSClaim.GetCOVID19RVPControl
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = Me._udtSessionHandler.EHSClaimVaccineGetFromSession()
        Dim udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel = udtEHSClaimVaccine.SubsidizeList(0)

        Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        ' For Eligible & Claim Rule Warning Checking
        Dim udtEligibleResult As EligibleResult = Nothing
        Dim udtClaimRuleResultList As New List(Of ClaimRuleResult)
        Dim udtRuleResults As RuleResultCollection = Nothing
        Dim strHeader As String = String.Empty
        Dim strText As String = String.Empty
        Dim strKey As String = String.Empty
        Dim udtRuleResult As RuleResult = Nothing

        Dim udtInputPicker As New InputPickerModel()
        udtInputPicker.ServiceDate = udtEHSTransaction.ServiceDate
        udtInputPicker.SPID = udtEHSTransaction.ServiceProviderID

        udcInputCOVID19RVP.SetRCHCodeError(False)
        udcInputCOVID19RVP.SetDoseErrorImage(False)

        udtRuleResults = Me._udtSessionHandler.EligibleResultGetFromSession()

        If Not checkByConfirmationBox Then

            Me.ClearWarningRules(udtRuleResults)

            ' -----------------------------------------------
            ' UI Input Validation
            '------------------------------------------------

            'Claim Detial Part & Vaccine Part
            isValid = udcInputCOVID19RVP.Validate(True, Me.udcMsgBoxErr)

            If panStep2aRecipinetContactInfo.Visible Then
                If Me.txtStep2aContactNo.Enabled AndAlso _
                    udcInputCOVID19RVP.RecipientType <> RECIPIENT_TYPE.RESIDENT AndAlso _
                    udcInputCOVID19RVP.RecipientType <> String.Empty Then

                    'If String.IsNullOrEmpty(Me.txtStep2aContactNo.Text) AndAlso Me.chkStep2aMobile.Checked Then
                    If String.IsNullOrEmpty(Me.txtStep2aContactNo.Text) Then
                        isValid = False

                        imgStep2aContactNoError.Visible = True

                        Dim udtMsg As SystemMessage = New SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00463)
                        If Me.udcMsgBoxErr IsNot Nothing Then Me.udcMsgBoxErr.AddMessage(udtMsg, _
                                                                                        New String() {"%en", "%tc", "%sc"}, _
                                                                                        New String() {HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.English)), _
                                                                                                      HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)), _
                                                                                                      HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese))})
                    End If

                    If Not String.IsNullOrEmpty(Me.txtStep2aContactNo.Text) AndAlso udcInputCOVID19RVP.RecipientType <> RECIPIENT_TYPE.RESIDENT _
                        AndAlso udcInputCOVID19RVP.RecipientType <> String.Empty Then

                        If Not Regex.IsMatch(Me.txtStep2aContactNo.Text, "^[2-9]\d{7}$") Then
                            isValid = False

                            imgStep2aContactNoError.Visible = True

                            Dim udtMsg As SystemMessage = New SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00466)
                            If Me.udcMsgBoxErr IsNot Nothing Then Me.udcMsgBoxErr.AddMessage(udtMsg, _
                                                                                        New String() {"%en", "%tc", "%sc"}, _
                                                                                        New String() {HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.English)), _
                                                                                                      HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)), _
                                                                                                      HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese))})
                        End If

                    End If
                End If
            End If

            'If Not Me.chkStep2aDeclareClaim.Checked Then
            '    isValid = False

            '    imgStep2aDeclareClaimError.Visible = True

            '    Dim udtMsg As SystemMessage = New SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00367)
            '    If Me.udcMsgBoxErr IsNot Nothing Then Me.udcMsgBoxErr.AddMessage(udtMsg, "%s", "Verification Checklist")
            'End If

        End If

        'Mark selected vaccine & dose in EHSClaimVaccineModel model
        If isValid OrElse checkByConfirmationBox Then
            udtInputPicker.Brand = udcInputCOVID19RVP.VaccineBrand
            udtInputPicker.VaccinationRecord = GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode)
            udcInputCOVID19RVP.Selection()
        End If

        If Not checkByConfirmationBox Then
            ' ------------------------------------------------------------------
            ' Check Last Service Date of SubsidizeGroupClaim
            ' ------------------------------------------------------------------
            If isValid Then
                isValid = Me.CheckLastServiceDate(udtEHSTransaction.ServiceDate, udtSchemeClaim, udtEHSClaimVaccine)
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

                Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckEligibilityForEnterClaim(udtSchemeClaim, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo, GetVaccinationRecord(udtEHSAccount, udtSchemeClaim.SchemeCode), udtEligibleResult)

                If Not Me._udtSystemMessage Is Nothing Then
                    ' If Check Eligibility Block Show Error
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                End If

                ' --------------------------------------------------------------
                ' Check Document Limit:
                ' --------------------------------------------------------------
                If isValid Then
                    Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckExceedDocumentLimitForEnterClaim(udtSchemeClaim.SchemeCode, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo)
                    If Not Me._udtSystemMessage Is Nothing Then
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                    End If
                End If

                ' --------------------------------------------------------------
                ' Check Claim Rules:
                ' --------------------------------------------------------------
                If isValid Then
                    Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckClaimRuleForEnterClaimCOVID19(udtEHSTransaction.ServiceDate, _
                                                                                                 udtEHSAccount, _
                                                                                                 udtEHSPersonalInfo, _
                                                                                                 Me._udtSessionHandler.EHSClaimVaccineGetFromSession(), _
                                                                                                 GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), _
                                                                                                 udtClaimRuleResultList, _
                                                                                                 udtInputPicker)
                    If Not Me._udtSystemMessage Is Nothing Then
                        ' If Check Claim Rules Block Show Error
                        isValid = False

                        For Each udtClaimRuleResult As ClaimRuleResult In udtClaimRuleResultList
                            HandleSystemMessage(Me._udtSystemMessage, udtClaimRuleResult)
                        Next

                        udcInputCOVID19RVP.SetDoseErrorImage(True)

                        If _udtSystemMessage.FunctionCode = "990000" AndAlso _udtSystemMessage.SeverityCode = "E" AndAlso _udtSystemMessage.MessageCode = "00464" Then
                            udcInputCOVID19RVP.SetVaccineBrandError(True)
                        End If
                    End If
                End If
            End If

            If isValid Then
                udtRuleResults = Me._udtSessionHandler.EligibleResultGetFromSession()

                If udtRuleResults Is Nothing Then
                    udtRuleResults = New RuleResultCollection()
                End If

                ' --------------------------------------------------------------
                ' Eligibility Warning / Declaration
                ' --------------------------------------------------------------
                If udtEligibleResult.IsEligible AndAlso _
                    (udtEligibleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtEligibleResult.HandleMethod = HandleMethodENum.Warning) Then

                    'Dim drClaimCategory As DataRow = (New ClaimCategoryBLL).getCategoryDesc(MyBase.EHSTransaction.CategoryCode)
                    Dim strCategoryCode As String = String.Empty
                    Dim strCategoryCodeForRule As String = String.Empty
                    Dim strCategoryCodeForExceptionRule As String = String.Empty
                    Dim udtClaimCategoryModelCollection As ClaimCategoryModelCollection = (New ClaimCategoryBLL).getAllCategoryCache
                    For Each udtClaimCategoryModel As ClaimCategoryModel In udtClaimCategoryModelCollection
                        If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                            If udtClaimCategoryModel.SchemeCode = udtEligibleResult.RelatedEligibleRule.SchemeCode And _
                                udtClaimCategoryModel.SchemeSeq = udtEligibleResult.RelatedEligibleRule.SchemeSeq And _
                                udtClaimCategoryModel.SubsidizeCode = udtEligibleResult.RelatedEligibleRule.SubsidizeCode Then
                                strCategoryCodeForRule = udtClaimCategoryModel.CategoryCode
                            End If
                        ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                            If udtClaimCategoryModel.SchemeCode = udtEligibleResult.RelatedEligibleExceptionRule.SchemeCode And _
                                udtClaimCategoryModel.SchemeSeq = udtEligibleResult.RelatedEligibleExceptionRule.SchemeSeq And _
                                udtClaimCategoryModel.SubsidizeCode = udtEligibleResult.RelatedEligibleExceptionRule.SubsidizeCode Then
                                strCategoryCodeForExceptionRule = udtClaimCategoryModel.CategoryCode
                            End If
                        End If
                    Next

                    If strCategoryCodeForRule <> String.Empty And strCategoryCodeForExceptionRule <> String.Empty And _
                        strCategoryCodeForRule = strCategoryCodeForExceptionRule Then
                        strCategoryCode = strCategoryCodeForRule
                    End If

                    If strCategoryCodeForRule <> String.Empty Then
                        strCategoryCode = strCategoryCodeForRule
                    End If

                    If strCategoryCodeForExceptionRule <> String.Empty Then
                        strCategoryCode = strCategoryCodeForExceptionRule
                    End If

                    If strCategoryCode = udcInputCOVID19RVP.CategoryForCOVID19 Then
                        udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Step2a, udtEligibleResult.RuleType), udtEligibleResult)

                        If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                            strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleRule.ObjectName.Trim())
                        ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                            strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleExceptionRule.ObjectName.Trim())
                        End If

                        Me.ucNoticePopUpExclamationConfirm.MessageText = strText

                        Me.ModalPopupExclamationConfirmationBox.Show()

                        _udtAuditLogEntry.AddDescripton("Message", strText)
                        EHSClaimBasePage.AuditLogShowClaimRulePopupBox(_udtAuditLogEntry)

                        isValid = False
                    End If

                End If

                ' --------------------------------------------------------------
                ' Claim Rules Warning / Declaration
                ' --------------------------------------------------------------
                If udtClaimRuleResultList IsNot Nothing AndAlso udtClaimRuleResultList.Count > 0 Then
                    Dim blnError As Boolean = False
                    Dim blnWarning As Boolean = False

                    For Each udtClaimRuleResult As ClaimRuleResult In udtClaimRuleResultList
                        If udtClaimRuleResult.IsBlock Then
                            blnError = True
                        End If

                        If Not udtClaimRuleResult.IsBlock AndAlso _
                            (udtClaimRuleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtClaimRuleResult.HandleMethod = HandleMethodENum.Warning) Then
                            blnWarning = True
                        End If
                    Next

                    If Not blnError AndAlso blnWarning Then

                        udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Step2a, udtClaimRuleResultList(0).RuleType), udtClaimRuleResultList(0))

                        'not Popup prompt before
                        If strText.Equals(String.Empty) AndAlso isValid Then
                            Dim strHTMLList As String = Me.Step2aPromptContent(udtClaimRuleResultList)

                            'Get the prompt message from ClaimRule
                            If udtClaimRuleResultList.Count = 1 Then
                                strHeader = Me.Step2aPromptHeader(udtClaimRuleResultList(0))
                            End If

                            If strHeader <> String.Empty Then
                                Me.ucNoticePopUpExclamationConfirm.CustomHeaderText = strHeader
                            End If

                            Me.ucNoticePopUpExclamationConfirm.MessageText = strHTMLList

                            Me.ModalPopupExclamationConfirmationBox.Show()

                            _udtAuditLogEntry.AddDescripton("Message", strText)
                            EHSClaimBasePage.AuditLogShowClaimRulePopupBox(_udtAuditLogEntry)

                        End If

                        isValid = False
                    End If

                End If

                Me._udtSessionHandler.EligibleResultSaveToSession(udtRuleResults)
            End If
        Else
            strKey = Me.RuleResultKey(ActiveViewIndex.Step2a, RuleTypeENum.EligibleResult)
            udtRuleResult = udtRuleResults.Item(strKey)

            If Not udtRuleResult Is Nothing Then

                'Should have 2 rule in this collection
                'first : After sreach this account -> rule added and auto make to confirmed
                'Second : After press "Next" in Enter claim detail -> rule added but not confirmed
                If Not udtRuleResult.PromptConfirmed Then
                    udtEHSTransaction.PreSchool = "Y"
                End If
            End If

            isValid = Me.RemoveRulesAfterConfirm(udtRuleResults, False)
        End If

        If isValid Then
            udcInputCOVID19RVP.Save(udtEHSTransaction, udtEHSClaimVaccine)

            'Clear old vaccination record for vaccination card
            _udtSessionHandler.ClaimCOVID19VaccinationCardRemoveFromSession(FunctionCode)

            'Find the nearest vaccination record for vaccination card
            Dim udtVaccinationRecordList As TransactionDetailVaccineModelCollection = Nothing
            udtVaccinationRecordList = GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode).FilterIncludeBySubsidizeItemCode(SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19)

            If udtVaccinationRecordList.Count > 0 Then
                Dim udtVaccinationRecord As TransactionDetailVaccineModel = udtVaccinationRecordList.FilterFindNearestRecord()

                If udtEHSClaimVaccine.GetSelectedDoseForCOVID19.AvailableItemCode.Trim().ToUpper() <> udtVaccinationRecord.AvailableItemCode.Trim.Trim().ToUpper() Then
                    'If udtInputPicker.Brand.Trim = udtVaccinationRecord.VaccineBrand.Trim.Trim().ToUpper() Then
                    _udtSessionHandler.ClaimCOVID19VaccinationCardSaveToSession(udtVaccinationRecord, FunctionCode)
                    'End If

                End If

            End If

            If Me.ClaimMode = ClaimMode.COVID19 AndAlso panStep2aRecipinetContactInfo.Visible Then
                Dim udtTransactAdditionfield As TransactionAdditionalFieldModel

                If Me._udtEHSTransaction.TransactionAdditionFields(0) IsNot Nothing Then
                    'Contact No.
                    Dim strContactNo As String = String.Empty

                    If udcInputCOVID19RVP.RecipientType <> RECIPIENT_TYPE.RESIDENT AndAlso udcInputCOVID19RVP.RecipientType <> String.Empty Then
                        strContactNo = txtStep2aContactNo.Text.Trim
                    End If

                    udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                    udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ContactNo
                    udtTransactAdditionfield.AdditionalFieldValueCode = strContactNo
                    udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
                    udtTransactAdditionfield.SchemeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeCode
                    udtTransactAdditionfield.SchemeSeq = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeSeq
                    udtTransactAdditionfield.SubsidizeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SubsidizeCode
                    Me._udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

                    ''Mobile
                    'udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                    'udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.Mobile
                    'udtTransactAdditionfield.AdditionalFieldValueCode = IIf(Me.chkStep2aMobile.Checked, YesNo.Yes, YesNo.No)
                    'udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
                    'udtTransactAdditionfield.SchemeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeCode
                    'udtTransactAdditionfield.SchemeSeq = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeSeq
                    'udtTransactAdditionfield.SubsidizeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SubsidizeCode
                    'Me._udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

                    'Remarks
                    udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                    udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.Remarks
                    udtTransactAdditionfield.AdditionalFieldValueCode = String.Empty
                    udtTransactAdditionfield.AdditionalFieldValueDesc = txtStep2aRemark.Text.Trim
                    udtTransactAdditionfield.SchemeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeCode
                    udtTransactAdditionfield.SchemeSeq = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeSeq
                    udtTransactAdditionfield.SubsidizeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SubsidizeCode
                    Me._udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

                    'JoinEHRSS
                    Dim strJoinEHRSS As String = String.Empty

                    If panStep2aDeclareJoineHRSS.Visible AndAlso _
                        udcInputCOVID19RVP.RecipientType <> RECIPIENT_TYPE.RESIDENT AndAlso _
                        udcInputCOVID19RVP.RecipientType <> String.Empty Then

                        strJoinEHRSS = IIf(chkStep2aDeclareJoineHRSS.Checked, YesNo.Yes, YesNo.No)

                    End If

                    udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                    udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.JoinEHRSS
                    udtTransactAdditionfield.AdditionalFieldValueCode = strJoinEHRSS
                    udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
                    udtTransactAdditionfield.SchemeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeCode
                    udtTransactAdditionfield.SchemeSeq = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeSeq
                    udtTransactAdditionfield.SubsidizeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SubsidizeCode
                    Me._udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

                End If
            End If

        End If

        Return isValid
    End Function
    ' CRE20-0022 (Immu record) [End][Chris YIM]

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Private Function Step2aCOVID19ORValidation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = False
        Dim strDOB As String

        Dim udtSchemeDetailBLL As New Common.Component.SchemeDetails.SchemeDetailBLL()
        Dim udtValidator As Validator = New Validator()

        Dim udcInputCOVID19OR As ucInputCOVID19OR = Me.udcStep2aInputEHSClaim.GetCOVID19ORControl
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = Me._udtSessionHandler.EHSClaimVaccineGetFromSession()
        Dim udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel = udtEHSClaimVaccine.SubsidizeList(0)

        Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        ' For Eligible & Claim Rule Warning Checking
        Dim udtEligibleResult As EligibleResult = Nothing
        Dim udtClaimRuleResultList As New List(Of ClaimRuleResult)
        Dim udtRuleResults As RuleResultCollection = Nothing
        Dim strHeader As String = String.Empty
        Dim strText As String = String.Empty
        Dim strKey As String = String.Empty
        Dim udtRuleResult As RuleResult = Nothing

        Dim udtInputPicker As New InputPickerModel()
        udtInputPicker.ServiceDate = udtEHSTransaction.ServiceDate
        udtInputPicker.SPID = udtEHSTransaction.ServiceProviderID

        udcInputCOVID19OR.SetDoseErrorImage(False)

        udtRuleResults = Me._udtSessionHandler.EligibleResultGetFromSession()

        If Not checkByConfirmationBox Then

            Me.ClearWarningRules(udtRuleResults)

            ' -----------------------------------------------
            ' UI Input Validation
            '------------------------------------------------

            'Claim Detial Part & Vaccine Part
            isValid = udcInputCOVID19OR.Validate(True, Me.udcMsgBoxErr)

            If panStep2aRecipinetContactInfo.Visible Then
                If Me.txtStep2aContactNo.Enabled Then
                    'If String.IsNullOrEmpty(Me.txtStep2aContactNo.Text) AndAlso Me.chkStep2aMobile.Checked Then
                    If String.IsNullOrEmpty(Me.txtStep2aContactNo.Text) Then
                        isValid = False

                        imgStep2aContactNoError.Visible = True

                        Dim udtMsg As SystemMessage = New SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00463)
                        If Me.udcMsgBoxErr IsNot Nothing Then Me.udcMsgBoxErr.AddMessage(udtMsg, _
                                                                                        New String() {"%en", "%tc", "%sc"}, _
                                                                                        New String() {HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.English)), _
                                                                                                      HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)), _
                                                                                                      HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese))})
                    End If

                    If Not String.IsNullOrEmpty(Me.txtStep2aContactNo.Text) Then
                        If Not Regex.IsMatch(Me.txtStep2aContactNo.Text, "^[2-9]\d{7}$") Then
                            isValid = False

                            imgStep2aContactNoError.Visible = True

                            Dim udtMsg As SystemMessage = New SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00466)
                            If Me.udcMsgBoxErr IsNot Nothing Then Me.udcMsgBoxErr.AddMessage(udtMsg, _
                                                                                        New String() {"%en", "%tc", "%sc"}, _
                                                                                        New String() {HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.English)), _
                                                                                                      HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)), _
                                                                                                      HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese))})
                        End If

                    End If

                End If

            End If

            'If Not Me.chkStep2aDeclareClaim.Checked Then
            '    isValid = False

            '    imgStep2aDeclareClaimError.Visible = True

            '    Dim udtMsg As SystemMessage = New SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00367)
            '    If Me.udcMsgBoxErr IsNot Nothing Then Me.udcMsgBoxErr.AddMessage(udtMsg, "%s", "Verification Checklist")
            'End If

        End If

        'Mark selected vaccine & dose in EHSClaimVaccineModel model
        If isValid OrElse checkByConfirmationBox Then
            udtInputPicker.Brand = udcInputCOVID19OR.VaccineBrand
            udtInputPicker.VaccinationRecord = GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode)

            Dim udtLatestC19Vaccine As TransactionDetailVaccineModel = Nothing
            Dim udtLatestC19Transaction As EHSTransactionModel = Nothing
            Dim udtTranDetailVaccineList As TransactionDetailVaccineModelCollection = GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode)
            Dim udtC19VaccineList As TransactionDetailVaccineModelCollection = udtTranDetailVaccineList.FilterIncludeBySubsidizeItemCode(SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19)

            For Each udtC19Vaccine As TransactionDetailVaccineModel In udtC19VaccineList
                'Find the latest COVID19 transaction in EHS
                If udtC19Vaccine.AvailableItemCode.Trim.ToUpper = "1STDOSE" Then
                    If udtLatestC19Vaccine Is Nothing Then
                        udtLatestC19Vaccine = udtC19Vaccine
                    Else
                        If udtC19Vaccine.ServiceReceiveDtm > udtLatestC19Vaccine.ServiceReceiveDtm Then
                            udtLatestC19Vaccine = udtC19Vaccine
                        End If
                    End If
                End If
            Next

            'Only allow EHS Transaction, not include CMS / CIMS record
            If udtLatestC19Vaccine IsNot Nothing AndAlso udtLatestC19Vaccine.TransactionID <> String.Empty Then
                udtLatestC19Transaction = _udtEHSTransactionBLL.LoadClaimTran(udtLatestC19Vaccine.TransactionID.Trim)
                udtInputPicker.LatestC19Transaction = udtLatestC19Transaction
            End If

            udcInputCOVID19OR.Selection()
        End If

        If Not checkByConfirmationBox Then
            ' ------------------------------------------------------------------
            ' Check Last Service Date of SubsidizeGroupClaim
            ' ------------------------------------------------------------------
            If isValid Then
                isValid = Me.CheckLastServiceDate(udtEHSTransaction.ServiceDate, udtSchemeClaim, udtEHSClaimVaccine)
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

                Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckEligibilityForEnterClaim(udtSchemeClaim, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo, GetVaccinationRecord(udtEHSAccount, udtSchemeClaim.SchemeCode), udtEligibleResult)

                If Not Me._udtSystemMessage Is Nothing Then
                    ' If Check Eligibility Block Show Error
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                End If

                ' --------------------------------------------------------------
                ' Check Document Limit:
                ' --------------------------------------------------------------
                If isValid Then
                    Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckExceedDocumentLimitForEnterClaim(udtSchemeClaim.SchemeCode, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo)
                    If Not Me._udtSystemMessage Is Nothing Then
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                    End If
                End If

                ' --------------------------------------------------------------
                ' Check Claim Rules:
                ' --------------------------------------------------------------
                If isValid Then
                    Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckClaimRuleForEnterClaimCOVID19(udtEHSTransaction.ServiceDate, _
                                                                                                 udtEHSAccount, _
                                                                                                 udtEHSPersonalInfo, _
                                                                                                 Me._udtSessionHandler.EHSClaimVaccineGetFromSession(), _
                                                                                                 GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), _
                                                                                                 udtClaimRuleResultList, _
                                                                                                 udtInputPicker)
                    If Not Me._udtSystemMessage Is Nothing Then
                        ' If Check Claim Rules Block Show Error
                        isValid = False

                        For Each udtClaimRuleResult As ClaimRuleResult In udtClaimRuleResultList
                            HandleSystemMessage(Me._udtSystemMessage, udtClaimRuleResult)
                        Next
                                             
                        udcInputCOVID19OR.SetDoseErrorImage(True)

                        If _udtSystemMessage.FunctionCode = "990000" AndAlso _udtSystemMessage.SeverityCode = "E" AndAlso _udtSystemMessage.MessageCode = "00464" Then
                            udcInputCOVID19OR.SetVaccineBrandError(True)
                        End If
                    End If
                End If
            End If

            If isValid Then
                udtRuleResults = Me._udtSessionHandler.EligibleResultGetFromSession()

                If udtRuleResults Is Nothing Then
                    udtRuleResults = New RuleResultCollection()
                End If

                ' --------------------------------------------------------------
                ' Eligibility Warning / Declaration
                ' --------------------------------------------------------------
                If udtEligibleResult.IsEligible AndAlso _
                    (udtEligibleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtEligibleResult.HandleMethod = HandleMethodENum.Warning) Then

                    'Dim drClaimCategory As DataRow = (New ClaimCategoryBLL).getCategoryDesc(MyBase.EHSTransaction.CategoryCode)
                    Dim strCategoryCode As String = String.Empty
                    Dim strCategoryCodeForRule As String = String.Empty
                    Dim strCategoryCodeForExceptionRule As String = String.Empty
                    Dim udtClaimCategoryModelCollection As ClaimCategoryModelCollection = (New ClaimCategoryBLL).getAllCategoryCache
                    For Each udtClaimCategoryModel As ClaimCategoryModel In udtClaimCategoryModelCollection
                        If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                            If udtClaimCategoryModel.SchemeCode = udtEligibleResult.RelatedEligibleRule.SchemeCode And _
                                udtClaimCategoryModel.SchemeSeq = udtEligibleResult.RelatedEligibleRule.SchemeSeq And _
                                udtClaimCategoryModel.SubsidizeCode = udtEligibleResult.RelatedEligibleRule.SubsidizeCode Then
                                strCategoryCodeForRule = udtClaimCategoryModel.CategoryCode
                            End If
                        ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                            If udtClaimCategoryModel.SchemeCode = udtEligibleResult.RelatedEligibleExceptionRule.SchemeCode And _
                                udtClaimCategoryModel.SchemeSeq = udtEligibleResult.RelatedEligibleExceptionRule.SchemeSeq And _
                                udtClaimCategoryModel.SubsidizeCode = udtEligibleResult.RelatedEligibleExceptionRule.SubsidizeCode Then
                                strCategoryCodeForExceptionRule = udtClaimCategoryModel.CategoryCode
                            End If
                        End If
                    Next

                    If strCategoryCodeForRule <> String.Empty And strCategoryCodeForExceptionRule <> String.Empty And _
                        strCategoryCodeForRule = strCategoryCodeForExceptionRule Then
                        strCategoryCode = strCategoryCodeForRule
                    End If

                    If strCategoryCodeForRule <> String.Empty Then
                        strCategoryCode = strCategoryCodeForRule
                    End If

                    If strCategoryCodeForExceptionRule <> String.Empty Then
                        strCategoryCode = strCategoryCodeForExceptionRule
                    End If

                    If strCategoryCode = udcInputCOVID19OR.CategoryForCOVID19 Then
                        udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Step2a, udtEligibleResult.RuleType), udtEligibleResult)

                        If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                            strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleRule.ObjectName.Trim())
                        ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                            strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleExceptionRule.ObjectName.Trim())
                        End If

                        Me.ucNoticePopUpExclamationConfirm.MessageText = strText

                        Me.ModalPopupExclamationConfirmationBox.Show()

                        _udtAuditLogEntry.AddDescripton("Message", strText)
                        EHSClaimBasePage.AuditLogShowClaimRulePopupBox(_udtAuditLogEntry)

                        isValid = False
                    End If

                End If

                ' --------------------------------------------------------------
                ' Claim Rules Warning / Declaration
                ' --------------------------------------------------------------
                If udtClaimRuleResultList IsNot Nothing AndAlso udtClaimRuleResultList.Count > 0 Then
                    Dim blnError As Boolean = False
                    Dim blnWarning As Boolean = False

                    For Each udtClaimRuleResult As ClaimRuleResult In udtClaimRuleResultList
                        If udtClaimRuleResult.IsBlock Then
                            blnError = True
                        End If

                        If Not udtClaimRuleResult.IsBlock AndAlso _
                            (udtClaimRuleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtClaimRuleResult.HandleMethod = HandleMethodENum.Warning) Then
                            blnWarning = True
                        End If
                    Next

                    If Not blnError AndAlso blnWarning Then
                        udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Step2a, udtClaimRuleResultList(0).RuleType), udtClaimRuleResultList(0))

	                    'not Popup prompt before
	                    If strText.Equals(String.Empty) AndAlso isValid Then
                            Dim strHTMLList As String = Me.Step2aPromptContent(udtClaimRuleResultList)

                            'Get the prompt message from ClaimRule
                            If udtClaimRuleResultList.Count = 1 Then
                                strHeader = Me.Step2aPromptHeader(udtClaimRuleResultList(0))
                            End If
	
	                        If strHeader <> String.Empty Then
	                            Me.ucNoticePopUpExclamationConfirm.CustomHeaderText = strHeader
	                        End If
	
                            Me.ucNoticePopUpExclamationConfirm.MessageText = strHTMLList
	
	                        Me.ModalPopupExclamationConfirmationBox.Show()
	
	                        _udtAuditLogEntry.AddDescripton("Message", strText)
	                        EHSClaimBasePage.AuditLogShowClaimRulePopupBox(_udtAuditLogEntry)
	
	                    End If
	
	                    isValid = False
                    End If

                End If

                Me._udtSessionHandler.EligibleResultSaveToSession(udtRuleResults)
            End If
        Else
            strKey = Me.RuleResultKey(ActiveViewIndex.Step2a, RuleTypeENum.EligibleResult)
            udtRuleResult = udtRuleResults.Item(strKey)

            If Not udtRuleResult Is Nothing Then

                'Should have 2 rule in this collection
                'first : After sreach this account -> rule added and auto make to confirmed
                'Second : After press "Next" in Enter claim detail -> rule added but not confirmed
                If Not udtRuleResult.PromptConfirmed Then
                    udtEHSTransaction.PreSchool = "Y"
                End If
            End If

            isValid = Me.RemoveRulesAfterConfirm(udtRuleResults, False)
        End If

        If isValid Then
            udcInputCOVID19OR.Save(udtEHSTransaction, udtEHSClaimVaccine)

            'Clear old vaccination record for vaccination card
            _udtSessionHandler.ClaimCOVID19VaccinationCardRemoveFromSession(FunctionCode)

            'Find the nearest vaccination record for vaccination card
            Dim udtVaccinationRecordList As TransactionDetailVaccineModelCollection = Nothing
            udtVaccinationRecordList = GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode).FilterIncludeBySubsidizeItemCode(SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19)

            If udtVaccinationRecordList.Count > 0 Then
                Dim udtVaccinationRecord As TransactionDetailVaccineModel = udtVaccinationRecordList.FilterFindNearestRecord()

                If udtEHSClaimVaccine.GetSelectedDoseForCOVID19.AvailableItemCode.Trim().ToUpper() <> udtVaccinationRecord.AvailableItemCode.Trim.Trim().ToUpper() Then
                    'If udtInputPicker.Brand.Trim = udtVaccinationRecord.VaccineBrand.Trim.Trim().ToUpper() Then
                    _udtSessionHandler.ClaimCOVID19VaccinationCardSaveToSession(udtVaccinationRecord, FunctionCode)
                    'End If

                End If

            End If

            If Me.ClaimMode = ClaimMode.COVID19 AndAlso panStep2aRecipinetContactInfo.Visible Then
                Dim udtTransactAdditionfield As TransactionAdditionalFieldModel

                If Me._udtEHSTransaction.TransactionAdditionFields(0) IsNot Nothing Then
                    'Contact No.
                    udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                    udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ContactNo
                    udtTransactAdditionfield.AdditionalFieldValueCode = txtStep2aContactNo.Text.Trim
                    udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
                    udtTransactAdditionfield.SchemeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeCode
                    udtTransactAdditionfield.SchemeSeq = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeSeq
                    udtTransactAdditionfield.SubsidizeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SubsidizeCode
                    Me._udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

                    ''Mobile
                    'udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                    'udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.Mobile
                    'udtTransactAdditionfield.AdditionalFieldValueCode = IIf(Me.chkStep2aMobile.Checked, YesNo.Yes, YesNo.No)
                    'udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
                    'udtTransactAdditionfield.SchemeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeCode
                    'udtTransactAdditionfield.SchemeSeq = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeSeq
                    'udtTransactAdditionfield.SubsidizeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SubsidizeCode
                    'Me._udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

                    'Remarks
                    udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                    udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.Remarks
                    udtTransactAdditionfield.AdditionalFieldValueCode = String.Empty
                    udtTransactAdditionfield.AdditionalFieldValueDesc = txtStep2aRemark.Text.Trim
                    udtTransactAdditionfield.SchemeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeCode
                    udtTransactAdditionfield.SchemeSeq = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeSeq
                    udtTransactAdditionfield.SubsidizeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SubsidizeCode
                    Me._udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

                    'JoinEHRSS
                    Dim strJoinEHRSS As String = String.Empty

                    If panStep2aDeclareJoineHRSS.Visible Then
                        strJoinEHRSS = IIf(chkStep2aDeclareJoineHRSS.Checked, YesNo.Yes, YesNo.No)
                    End If

                    udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                    udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.JoinEHRSS
                    udtTransactAdditionfield.AdditionalFieldValueCode = strJoinEHRSS
                    udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
                    udtTransactAdditionfield.SchemeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeCode
                    udtTransactAdditionfield.SchemeSeq = Me._udtEHSTransaction.TransactionAdditionFields(0).SchemeSeq
                    udtTransactAdditionfield.SubsidizeCode = Me._udtEHSTransaction.TransactionAdditionFields(0).SubsidizeCode
                    Me._udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
                End If
            End If

        End If

        Return isValid
    End Function
    ' CRE20-0022 (Immu record) [End][Chris YIM]

    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
    ' -----------------------------------------------------------------------------------------
    Private Function Step2aENHVSSOValidation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = False
        Dim strDOB As String

        Dim udtSchemeDetailBLL As New Common.Component.SchemeDetails.SchemeDetailBLL()
        Dim udtValidator As Validator = New Validator()

        Dim udcInputENHVSSO As ucInputENHVSSO = Me.udcStep2aInputEHSClaim.GetENHVSSOControl
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = Me._udtSessionHandler.EHSClaimVaccineGetFromSession()
        Dim udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel = udtEHSClaimVaccine.SubsidizeList(0)

        Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        ' For Eligible & Claim Rule Warning Checking
        Dim udtEligibleResult As EligibleResult = Nothing
        Dim udtClaimRuleResult As ClaimRuleResult = Nothing
        Dim udtRuleResults As RuleResultCollection = Nothing
        Dim strText As String = String.Empty
        Dim strKey As String = String.Empty
        Dim udtRuleResult As RuleResult = Nothing

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtInputPicker As New InputPickerModel()
        udtInputPicker.ServiceDate = udtEHSTransaction.ServiceDate
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        udcInputENHVSSO.SetDoseErrorImage(False)

        udtRuleResults = Me._udtSessionHandler.EligibleResultGetFromSession()

        If Not checkByConfirmationBox Then

            Me.ClearWarningRules(udtRuleResults)

            ' -----------------------------------------------
            ' UI Input Validation
            '------------------------------------------------

            'Claim Detial Part & Vaccine Part
            isValid = udcInputENHVSSO.Validate(True, Me.udcMsgBoxErr)

            ' ------------------------------------------------------------------
            ' Check Last Service Date of SubsidizeGroupClaim
            ' ------------------------------------------------------------------
            If isValid Then
                isValid = Me.CheckLastServiceDate(udtEHSTransaction.ServiceDate, udtSchemeClaim, udtEHSClaimVaccine)
            End If

            '' ------------------------------------------------------------------
            '' Check Dose Period
            '' ------------------------------------------------------------------
            'If isValid Then
            '    For Each udtEHSClaimVaccineSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
            '        If isValid AndAlso udtEHSClaimVaccineSubsidize.Selected Then

            '            For Each udtEHSClaimSubidizeDetailModel As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimVaccineSubsidize.SubsidizeDetailList
            '                If isValid AndAlso udtEHSClaimSubidizeDetailModel.Selected Then
            '                    Me._udtSystemMessage = udtValidator.chkServiceDateSubsidizeDoseLastServiceDate(udtEHSTransaction.ServiceDate, udtSchemeDetailBLL.getAllSchemeDosePeriod.Filter(udtSchemeClaim.SchemeCode, udtSchemeClaim.SchemeSeq, udtEHSClaimVaccineSubsidize.SubsidizeCode, udtEHSClaimSubidizeDetailModel.AvailableItemCode))
            '                    If Not Me._udtSystemMessage Is Nothing Then
            '                        isValid = False
            '                        Me.imgStep2aServiceDateError.Visible = True
            '                        Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
            '                    End If
            '                End If
            '            Next
            '        End If
            '    Next
            'End If

            ' To Do:
            ' Generic CheckClaimRulesFunction:
            ' will return ClaimResult with appropriate Status for Popup
            ' According to GroupCode / ClaimRulesCode -> Show different Popup

            'Check is Eligible for claim -> Child is 6 years old+ and the first does is avaliable = not Eligible

            If isValid Then
                ' --------------------------------------------------------------
                ' Check Eligibility:
                ' --------------------------------------------------------------
                If udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DocCode = DocTypeModel.DocTypeCode.EC AndAlso udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                    strDOB = _udtFormatter.formatDOB(udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DOB, udtEHSPersonalInfo.ExactDOB, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ECAge, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ECDateOfRegistration)
                Else
                    strDOB = _udtFormatter.formatDOB(udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DOB, udtEHSPersonalInfo.ExactDOB, Nothing, Nothing)
                End If

                Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckEligibilityForEnterClaim(udtSchemeClaim, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo, GetVaccinationRecord(udtEHSAccount, udtSchemeClaim.SchemeCode), udtEligibleResult)

                If Not Me._udtSystemMessage Is Nothing Then
                    ' If Check Eligibility Block Show Error
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                End If

                ' --------------------------------------------------------------
                ' Check Document Limit:
                ' --------------------------------------------------------------
                If isValid Then
                    Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckExceedDocumentLimitForEnterClaim(udtSchemeClaim.SchemeCode, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo)
                    If Not Me._udtSystemMessage Is Nothing Then
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                    End If
                End If

                ' --------------------------------------------------------------
                ' Check Claim Rules:
                ' --------------------------------------------------------------
                If isValid Then
                    Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckClaimRuleForEnterClaim(udtEHSTransaction.ServiceDate, udtEHSAccount, udtEHSPersonalInfo, Me._udtSessionHandler.EHSClaimVaccineGetFromSession(), GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), udtClaimRuleResult, udtInputPicker)
                    If Not Me._udtSystemMessage Is Nothing Then
                        ' If Check Claim Rules Block Show Error
                        isValid = False

                        HandleSystemMessage(Me._udtSystemMessage, udtClaimRuleResult)
                        udcInputENHVSSO.SetDoseErrorImage(True)

                    End If
                End If
            End If

            If isValid Then
                udtRuleResults = Me._udtSessionHandler.EligibleResultGetFromSession()

                If udtRuleResults Is Nothing Then
                    udtRuleResults = New RuleResultCollection()
                End If

                ' --------------------------------------------------------------
                ' Eligibility Warning / Declaration
                ' --------------------------------------------------------------
                If udtEligibleResult.IsEligible AndAlso _
                    (udtEligibleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtEligibleResult.HandleMethod = HandleMethodENum.Warning) Then

                    'Dim drClaimCategory As DataRow = (New ClaimCategoryBLL).getCategoryDesc(MyBase.EHSTransaction.CategoryCode)
                    Dim strCategoryCode As String = String.Empty
                    Dim strCategoryCodeForRule As String = String.Empty
                    Dim strCategoryCodeForExceptionRule As String = String.Empty
                    Dim udtClaimCategoryModelCollection As ClaimCategoryModelCollection = (New ClaimCategoryBLL).getAllCategoryCache
                    For Each udtClaimCategoryModel As ClaimCategoryModel In udtClaimCategoryModelCollection
                        If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                            If udtClaimCategoryModel.SchemeCode = udtEligibleResult.RelatedEligibleRule.SchemeCode And _
                                udtClaimCategoryModel.SchemeSeq = udtEligibleResult.RelatedEligibleRule.SchemeSeq And _
                                udtClaimCategoryModel.SubsidizeCode = udtEligibleResult.RelatedEligibleRule.SubsidizeCode Then
                                strCategoryCodeForRule = udtClaimCategoryModel.CategoryCode
                            End If
                        ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                            If udtClaimCategoryModel.SchemeCode = udtEligibleResult.RelatedEligibleExceptionRule.SchemeCode And _
                                udtClaimCategoryModel.SchemeSeq = udtEligibleResult.RelatedEligibleExceptionRule.SchemeSeq And _
                                udtClaimCategoryModel.SubsidizeCode = udtEligibleResult.RelatedEligibleExceptionRule.SubsidizeCode Then
                                strCategoryCodeForExceptionRule = udtClaimCategoryModel.CategoryCode
                            End If
                        End If
                    Next

                    If strCategoryCodeForRule <> String.Empty And strCategoryCodeForExceptionRule <> String.Empty And _
                        strCategoryCodeForRule = strCategoryCodeForExceptionRule Then
                        strCategoryCode = strCategoryCodeForRule
                    End If

                    If strCategoryCodeForRule <> String.Empty Then
                        strCategoryCode = strCategoryCodeForRule
                    End If

                    If strCategoryCodeForExceptionRule <> String.Empty Then
                        strCategoryCode = strCategoryCodeForExceptionRule
                    End If

                    If strCategoryCode = udcInputENHVSSO.Category Then
                        udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Step2a, udtEligibleResult.RuleType), udtEligibleResult)

                        If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                            strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleRule.ObjectName.Trim())
                        ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                            strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleExceptionRule.ObjectName.Trim())
                        End If

                        Me.ucNoticePopUpExclamationConfirm.MessageText = strText

                        Me.ModalPopupExclamationConfirmationBox.Show()
                        EHSClaimBasePage.AuditLogShowClaimRulePopupBox(_udtAuditLogEntry)

                        isValid = False
                    End If

                End If

                ' --------------------------------------------------------------
                ' Claim Rules Warning / Declaration
                ' --------------------------------------------------------------
                If Not udtClaimRuleResult Is Nothing AndAlso Not udtClaimRuleResult.IsBlock AndAlso _
                    (udtClaimRuleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtClaimRuleResult.HandleMethod = HandleMethodENum.Warning) Then

                    udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Step2a, udtClaimRuleResult.RuleType), udtClaimRuleResult)

                    'not Popup prompt before
                    If strText.Equals(String.Empty) AndAlso isValid Then
                        'Get the prompt message from ClaimRule
                        strText = Me.Step2aPromptClaimRule(udtClaimRuleResult)
                        Me.ucNoticePopUpExclamationConfirm.MessageText = strText
                        Me.ModalPopupExclamationConfirmationBox.Show()
                        EHSClaimBasePage.AuditLogShowClaimRulePopupBox(_udtAuditLogEntry)

                    End If

                    isValid = False
                End If

                Me._udtSessionHandler.EligibleResultSaveToSession(udtRuleResults)
            End If
        Else
            strKey = Me.RuleResultKey(ActiveViewIndex.Step2a, RuleTypeENum.EligibleResult)
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
            'isValid = Me.RemoveRulesAfterConfirm(SchemeClaimModel.CIVSS, udtRuleResults, False)
            isValid = Me.RemoveRulesAfterConfirm(udtRuleResults, False)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        End If

        If isValid Then
            udcInputENHVSSO.Save(udtEHSTransaction, udtEHSClaimVaccine)
        End If

        Return isValid
    End Function
    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
    ' -----------------------------------------------------------------------------------------
    Private Function Step2aPPPValidation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = False
        Dim strDOB As String

        Dim udtSchemeDetailBLL As New Common.Component.SchemeDetails.SchemeDetailBLL()
        Dim udtValidator As Validator = New Validator()

        Dim udcInputVSS As ucInputVSS = Me.udcStep2aInputEHSClaim.GetVSSControl
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = Me._udtSessionHandler.EHSClaimVaccineGetFromSession()
        Dim udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel = udtEHSClaimVaccine.SubsidizeList(0)

        Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        ' For Eligible & Claim Rule Warning Checking
        Dim udtEligibleResult As EligibleResult = Nothing
        Dim udtClaimRuleResult As ClaimRuleResult = Nothing
        Dim udtRuleResults As RuleResultCollection = Nothing
        Dim strText As String = String.Empty
        Dim strKey As String = String.Empty
        Dim udtRuleResult As RuleResult = Nothing

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtInputPicker As New InputPickerModel()
        udtInputPicker.ServiceDate = udtEHSTransaction.ServiceDate
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        udcInputVSS.SetDoseErrorImage(False)

        udtRuleResults = Me._udtSessionHandler.EligibleResultGetFromSession()

        If Not checkByConfirmationBox Then

            Me.ClearWarningRules(udtRuleResults)

            ' -----------------------------------------------
            ' UI Input Validation
            '------------------------------------------------

            'Claim Detial Part & Vaccine Part
            isValid = udcInputVSS.Validate(True, Me.udcMsgBoxErr)

            'CRE16-026 (Add PCV13) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            If isValid Then
                Dim strHighRisk As String = String.Empty
                Select Case udcInputVSS.HighRiskOptionShown
                    Case SubsidizeGroupClaimModel.HighRiskOptionClass.ShowForInput 'Manual Input
                        If udcInputVSS.HighRiskOptionEnabled Then
                            strHighRisk = udcInputVSS.HighRisk()
                        End If
                    Case SubsidizeGroupClaimModel.HighRiskOptionClass.HideButForceHighRisk 'Auto Input
                        strHighRisk = YesNo.Yes
                    Case SubsidizeGroupClaimModel.HighRiskOptionClass.HideWithoutInput 'No Input
                        strHighRisk = String.Empty
                    Case Else
                        strHighRisk = String.Empty
                End Select

                udtInputPicker.HighRisk = strHighRisk
            End If
            'CRE16-026 (Add PCV13) [End][Chris YIM]

            ' ------------------------------------------------------------------
            ' Check Last Service Date of SubsidizeGroupClaim
            ' ------------------------------------------------------------------
            If isValid Then
                isValid = Me.CheckLastServiceDate(udtEHSTransaction.ServiceDate, udtSchemeClaim, udtEHSClaimVaccine)
            End If

            '' ------------------------------------------------------------------
            '' Check Dose Period
            '' ------------------------------------------------------------------
            'If isValid Then
            '    For Each udtEHSClaimVaccineSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
            '        If isValid AndAlso udtEHSClaimVaccineSubsidize.Selected Then

            '            For Each udtEHSClaimSubidizeDetailModel As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimVaccineSubsidize.SubsidizeDetailList
            '                If isValid AndAlso udtEHSClaimSubidizeDetailModel.Selected Then
            '                    Me._udtSystemMessage = udtValidator.chkServiceDateSubsidizeDoseLastServiceDate(udtEHSTransaction.ServiceDate, udtSchemeDetailBLL.getAllSchemeDosePeriod.Filter(udtSchemeClaim.SchemeCode, udtSchemeClaim.SchemeSeq, udtEHSClaimVaccineSubsidize.SubsidizeCode, udtEHSClaimSubidizeDetailModel.AvailableItemCode))
            '                    If Not Me._udtSystemMessage Is Nothing Then
            '                        isValid = False
            '                        Me.imgStep2aServiceDateError.Visible = True
            '                        Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
            '                    End If
            '                End If
            '            Next
            '        End If
            '    Next
            'End If

            ' To Do:
            ' Generic CheckClaimRulesFunction:
            ' will return ClaimResult with appropriate Status for Popup
            ' According to GroupCode / ClaimRulesCode -> Show different Popup

            'Check is Eligible for claim -> Child is 6 years old+ and the first does is avaliable = not Eligible

            If isValid Then
                ' --------------------------------------------------------------
                ' Check Eligibility:
                ' --------------------------------------------------------------
                If udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DocCode = DocTypeModel.DocTypeCode.EC AndAlso udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                    strDOB = _udtFormatter.formatDOB(udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DOB, udtEHSPersonalInfo.ExactDOB, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ECAge, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ECDateOfRegistration)
                Else
                    strDOB = _udtFormatter.formatDOB(udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DOB, udtEHSPersonalInfo.ExactDOB, Nothing, Nothing)
                End If

                Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckEligibilityForEnterClaim(udtSchemeClaim, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo, GetVaccinationRecord(udtEHSAccount, udtSchemeClaim.SchemeCode), udtEligibleResult)

                If Not Me._udtSystemMessage Is Nothing Then
                    ' If Check Eligibility Block Show Error
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                End If

                ' --------------------------------------------------------------
                ' Check Document Limit:
                ' --------------------------------------------------------------
                If isValid Then
                    Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckExceedDocumentLimitForEnterClaim(udtSchemeClaim.SchemeCode, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo)
                    If Not Me._udtSystemMessage Is Nothing Then
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                    End If
                End If

                ' --------------------------------------------------------------
                ' Check Claim Rules:
                ' --------------------------------------------------------------
                If isValid Then
                    Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckClaimRuleForEnterClaim(udtEHSTransaction.ServiceDate, udtEHSAccount, udtEHSPersonalInfo, Me._udtSessionHandler.EHSClaimVaccineGetFromSession(), GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), udtClaimRuleResult, udtInputPicker)
                    If Not Me._udtSystemMessage Is Nothing Then
                        ' If Check Claim Rules Block Show Error
                        isValid = False

                        HandleSystemMessage(Me._udtSystemMessage, udtClaimRuleResult)
                        udcInputVSS.SetDoseErrorImage(True)

                    End If
                End If
            End If

            If isValid Then
                udtRuleResults = Me._udtSessionHandler.EligibleResultGetFromSession()

                If udtRuleResults Is Nothing Then
                    udtRuleResults = New RuleResultCollection()
                End If

                ' --------------------------------------------------------------
                ' Eligibility Warning / Declaration
                ' --------------------------------------------------------------
                If udtEligibleResult.IsEligible AndAlso _
                    (udtEligibleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtEligibleResult.HandleMethod = HandleMethodENum.Warning) Then

                    'Dim drClaimCategory As DataRow = (New ClaimCategoryBLL).getCategoryDesc(MyBase.EHSTransaction.CategoryCode)
                    Dim strCategoryCode As String = String.Empty
                    Dim strCategoryCodeForRule As String = String.Empty
                    Dim strCategoryCodeForExceptionRule As String = String.Empty
                    Dim udtClaimCategoryModelCollection As ClaimCategoryModelCollection = (New ClaimCategoryBLL).getAllCategoryCache
                    For Each udtClaimCategoryModel As ClaimCategoryModel In udtClaimCategoryModelCollection
                        If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                            If udtClaimCategoryModel.SchemeCode = udtEligibleResult.RelatedEligibleRule.SchemeCode And _
                                udtClaimCategoryModel.SchemeSeq = udtEligibleResult.RelatedEligibleRule.SchemeSeq And _
                                udtClaimCategoryModel.SubsidizeCode = udtEligibleResult.RelatedEligibleRule.SubsidizeCode Then
                                strCategoryCodeForRule = udtClaimCategoryModel.CategoryCode
                            End If
                        ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                            If udtClaimCategoryModel.SchemeCode = udtEligibleResult.RelatedEligibleExceptionRule.SchemeCode And _
                                udtClaimCategoryModel.SchemeSeq = udtEligibleResult.RelatedEligibleExceptionRule.SchemeSeq And _
                                udtClaimCategoryModel.SubsidizeCode = udtEligibleResult.RelatedEligibleExceptionRule.SubsidizeCode Then
                                strCategoryCodeForExceptionRule = udtClaimCategoryModel.CategoryCode
                            End If
                        End If
                    Next

                    If strCategoryCodeForRule <> String.Empty And strCategoryCodeForExceptionRule <> String.Empty And _
                        strCategoryCodeForRule = strCategoryCodeForExceptionRule Then
                        strCategoryCode = strCategoryCodeForRule
                    End If

                    If strCategoryCodeForRule <> String.Empty Then
                        strCategoryCode = strCategoryCodeForRule
                    End If

                    If strCategoryCodeForExceptionRule <> String.Empty Then
                        strCategoryCode = strCategoryCodeForExceptionRule
                    End If

                    If strCategoryCode = udcInputVSS.Category Then
                        udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Step2a, udtEligibleResult.RuleType), udtEligibleResult)

                        If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                            strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleRule.ObjectName.Trim())
                        ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                            strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleExceptionRule.ObjectName.Trim())
                        End If

                        Me.ucNoticePopUpExclamationConfirm.MessageText = strText

                        Me.ModalPopupExclamationConfirmationBox.Show()
                        EHSClaimBasePage.AuditLogShowClaimRulePopupBox(_udtAuditLogEntry)

                        isValid = False
                    End If

                End If

                ' --------------------------------------------------------------
                ' Claim Rules Warning / Declaration
                ' --------------------------------------------------------------
                If Not udtClaimRuleResult Is Nothing AndAlso Not udtClaimRuleResult.IsBlock AndAlso _
                    (udtClaimRuleResult.HandleMethod = HandleMethodENum.Declaration OrElse udtClaimRuleResult.HandleMethod = HandleMethodENum.Warning) Then

                    udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Step2a, udtClaimRuleResult.RuleType), udtClaimRuleResult)

                    'not Popup prompt before
                    If strText.Equals(String.Empty) AndAlso isValid Then
                        'Get the prompt message from ClaimRule
                        strText = Me.Step2aPromptClaimRule(udtClaimRuleResult)
                        Me.ucNoticePopUpExclamationConfirm.MessageText = strText
                        Me.ModalPopupExclamationConfirmationBox.Show()
                        EHSClaimBasePage.AuditLogShowClaimRulePopupBox(_udtAuditLogEntry)

                    End If

                    isValid = False
                End If

                Me._udtSessionHandler.EligibleResultSaveToSession(udtRuleResults)
            End If
        Else
            strKey = Me.RuleResultKey(ActiveViewIndex.Step2a, RuleTypeENum.EligibleResult)
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
            'isValid = Me.RemoveRulesAfterConfirm(SchemeClaimModel.CIVSS, udtRuleResults, False)
            isValid = Me.RemoveRulesAfterConfirm(udtRuleResults, False)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        End If

        If isValid Then
            udcInputVSS.Save(udtEHSTransaction, udtEHSClaimVaccine)
        End If

        Return isValid
    End Function
    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

    ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Function Step2aSSSCMCValidation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As EHSTransactionModel) As Boolean

        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = True
        Dim strDOB As String = String.Empty
        Dim udtEligibleResult As EligibleResult = Nothing

        Dim udcInputSSSCMC As ucInputSSSCMC = Me.udcStep2aInputEHSClaim.GetSSSCMCControl()

        Dim udtValidator As Validator = New Validator()

        Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        Dim decAvailableAmount As Decimal = 0

        udcInputSSSCMC.SetError(False)

        Me.udcMsgBoxErr.Clear()

        If Not checkByConfirmationBox Then

            ' -----------------------------------------------
            ' UI Input Validation
            '------------------------------------------------
            If Not udcInputSSSCMC.Validate(True, Me.udcMsgBoxErr) Then
                isValid = False
            End If

            If isValid Then
                ' --------------------------------------------------------------
                ' Check Eligibility:
                ' --------------------------------------------------------------
                If udtEHSPersonalInfo.DocCode = DocTypeModel.DocTypeCode.EC AndAlso udtEHSPersonalInfo.ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                    strDOB = _udtFormatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, udtEHSPersonalInfo.ECAge, udtEHSPersonalInfo.ECDateOfRegistration)
                Else
                    strDOB = _udtFormatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, Nothing, Nothing)
                End If

                Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckEligibilityForEnterClaim(udtSchemeClaim, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo, Nothing, udtEligibleResult)

                If Not Me._udtSystemMessage Is Nothing Then
                    ' If Check Eligibility Block Show Error
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                End If
            End If

            If isValid Then
                ' --------------------------------------------------------------
                ' Check Document Limit:
                ' --------------------------------------------------------------
                Me._udtSystemMessage = Me._udtEHSClaimBLL.CheckExceedDocumentLimitForEnterClaim(udtSchemeClaim.SchemeCode, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo)
                If Not Me._udtSystemMessage Is Nothing Then
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                End If
            End If

            If isValid Then
                ' --------------------------------------------------------------
                ' Check Benefit:
                ' --------------------------------------------------------------
                Dim udtEHSTransactionBLL As New EHSTransactionBLL

                decAvailableAmount = udtEHSTransactionBLL.getAvailableSubsidizeItem_SSSCMC(udtEHSPersonalInfo, udtSchemeClaim.SubsidizeGroupClaimList)

                If decAvailableAmount > 0 AndAlso decAvailableAmount >= udcInputSSSCMC.UsedRMB Then
                    ' Subsidies for SSSCMC is available
                Else
                    ' No available subsidies for SSSCMC
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00107"))
                End If

            End If

        End If

        If isValid Then
            udtEHSTransaction.VoucherBeforeRedeem = Nothing
            udtEHSTransaction.VoucherAfterRedeem = Nothing
            udtEHSTransaction.VoucherClaim = Nothing
            udtEHSTransaction.ExchangeRate = udcInputSSSCMC.ExchangeRate
            udtEHSTransaction.VoucherClaimRMB = udcInputSSSCMC.UsedRMB

            udcInputSSSCMC.Save(udtEHSTransaction)

        End If

        Return isValid
    End Function
    ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

    Private Function Step2aPromptClaimRule(ByVal udtClaimRuleResult As ClaimRuleResult, Optional ByRef arrRemark As List(Of String) = Nothing) As String
        Dim strText As String = String.Empty
        Dim strRemark As String = String.Empty

        'Rule Detail: service date is over 24 days and less then 28 date, prompt warning message
        If Not udtClaimRuleResult.RelatedClaimRule Is Nothing Then
            strText = Me.GetGlobalResourceObject("Text", udtClaimRuleResult.RelatedClaimRule.ObjectName)

            Dim strContentRemark() As String = Split(strText, "|")

            If strContentRemark.Length > 1 Then
                strText = strContentRemark(0)
                strRemark = strContentRemark(1)
                If arrRemark IsNot Nothing AndAlso Not arrRemark.Contains(strRemark) AndAlso strRemark <> String.Empty Then
                    arrRemark.Add(strRemark)
                End If
            Else
                strText = strContentRemark(0)
            End If

            If udtClaimRuleResult.dtmDoseDate.HasValue Then
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim udtSubPlatformBLL As New SubPlatformBLL

                'strText = strText.Replace("%date", _udtFormatter.formatDate(udtClaimRuleResult.dtmDoseDate.Value))
                strText = strText.Replace("%date", _udtFormatter.formatDisplayDate(udtClaimRuleResult.dtmDoseDate.Value, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform)))
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            Else
                strText = strText.Replace("%date", String.Empty)
            End If

            If udtClaimRuleResult.ResultParam.Count > 0 Then
                For Each kvp As KeyValuePair(Of String, Object) In udtClaimRuleResult.ResultParam
                    strText = strText.Replace(kvp.Key, kvp.Value)
                Next
            End If

        End If

        Return strText
    End Function

    Private Function Step2aPromptHeader(ByVal udtClaimRuleResult As ClaimRuleResult) As String
        Dim strText As String = String.Empty

        Select Case udtClaimRuleResult.RelatedClaimRule.MessageCode
            Case MsgCode.MSG00467
                strText = GetGlobalResourceObject("Text", "COVID19NoRecordFound")
            Case MsgCode.MSG00468
                strText = GetGlobalResourceObject("Text", "COVID19LaterRecommendedSchedule")
        End Select

        Return strText

    End Function

    Private Function Step2aPromptContent(ByVal udtClaimRuleResultList As List(Of ClaimRuleResult)) As String
        Dim strText As String = String.Empty
        Dim strRemark As String = String.Empty
        Dim lstRemark As New List(Of String)
        Dim strStartHTML As String = "<ul style='padding-left:15px;margin-top:5px;margin-bottom:0px'><li style='padding-bottom:10px'>"
        Dim strEndHTML As String = "</li></ul>"
        Dim strRes As String = String.Empty

        'Get the prompt message from ClaimRule
        If udtClaimRuleResultList.Count > 1 Then
            Dim intCt As Integer = 1
            For Each udtClaimRuleResult As ClaimRuleResult In udtClaimRuleResultList

                If strText = String.Empty Then
                    strText = Me.Step2aPromptClaimRule(udtClaimRuleResult, lstRemark)
                Else
                    strText = strText + "</li><li style='padding-bottom:10px'>" + Me.Step2aPromptClaimRule(udtClaimRuleResult, lstRemark)
                End If

                intCt = intCt + 1

            Next

            For intRemark As Integer = 0 To lstRemark.Count - 1
                'If strRemark = String.Empty Then
                '    strRemark = lstRemark(intRemark)
                'Else
                strRemark = strRemark + "</li><li style='padding-bottom:10px'>" + lstRemark(intRemark)
                'End If

                intRemark = intRemark + 1
            Next

            If strRemark <> String.Empty Then
                strRes = strStartHTML + strText + strRemark + strEndHTML
            Else
                strRes = strStartHTML + strText + strEndHTML
            End If

        Else
            strText = Me.Step2aPromptClaimRule(udtClaimRuleResultList(0), lstRemark)

            For intRemark As Integer = 0 To lstRemark.Count - 1
                'If strRemark = String.Empty Then
                '    strRemark = lstRemark(intRemark)
                'Else
                strRemark = strRemark + "</li><li style='padding-bottom:10px'>" + lstRemark(intRemark)
                'End If

                intRemark = intRemark + 1
            Next

            If strRemark <> String.Empty Then
                strRes = strStartHTML + strText + strRemark + strEndHTML
            Else
                strRes = strText
            End If
        End If

        Return strRes

    End Function


    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
    ' Remove input parameter: strSchemeCode
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
                Me.ucNoticePopUpExclamationConfirm.MessageText = Me.Step2aPromptClaimRule(udtRuleResult)
                Me.ModalPopupExclamationConfirmationBox.Show()
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

    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
    ' Remove input parameter: strSchemeCode
    'Private Sub ClearWarningRules(ByRef udtRuleResults As RuleResultCollection, ByVal strSchemeCode As String)
    Private Sub ClearWarningRules(ByRef udtRuleResults As RuleResultCollection)
        'Reset EligibleResult Collection in Session
        If Not udtRuleResults Is Nothing AndAlso udtRuleResults.Count > 0 Then
            Me._udtSessionHandler.EligibleResultRemoveFromSession()

            Dim strKey As String = Me.RuleResultKey(ActiveViewIndex.Step1, RuleTypeENum.EligibleResult)
            Dim udtRuleResult As RuleResult = udtRuleResults.Item(strKey)

            'if Eligible Search Result is existing in session 
            If Not udtRuleResult Is Nothing Then
                udtRuleResults = New RuleResultCollection()
                udtRuleResults.Add(strKey, udtRuleResult)
                Me._udtSessionHandler.EligibleResultSaveToSession(udtRuleResults)
            End If
        End If
    End Sub
    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

#End Region

#Region "Step 2b Confirm Claim detail"

    Protected Sub btnStep2bBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnStep2bBack.Click
        If Me._blnIsRequireHandlePageRefresh Then
            Return
        End If
        Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step2a
        Me._udtSessionHandler.EHSTransactionRemoveFromSession(FunctCode)
    End Sub

    Protected Sub btnStep2bConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnStep2bConfirm.Click
        If Me._blnIsRequireHandlePageRefresh Then
            Return
        End If

        ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [Start][Winnie]
        Me.Step2bClaimSubmit()
        ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [End][Winnie]
    End Sub

    '------------------------------------------------------------------------------------------
    'Event For change print Option
    '------------------------------------------------------------------------------------------
    Private Sub btnStep2bChangePrintOption_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnStep2aChangePrintOption.Click, btnStep2bChangePrintOption.Click
        If Me._blnIsRequireHandlePageRefresh Then
            Return
        End If

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Dim imgbtn As ImageButton = CType(sender, ImageButton)

        If imgbtn.ID = "btnStep2aChangePrintOption" Then
            Session("ChangePrintOptionPage") = "2a"
        End If

        If imgbtn.ID = "btnStep2bChangePrintOption" Then
            Session("ChangePrintOptionPage") = "2b"
        End If
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        Dim udtDataEntryUser As DataEntryUserModel = Nothing

        Me._udtSessionHandler.CurrentUserGetFromSession(Me._udtSP, udtDataEntryUser)
        If udtDataEntryUser Is Nothing Then
            Me.udtPrintOptionSelection.setSelectedValue(Me._udtSP.PrintOption)
            EHSClaimBasePage.AuditLogSelectPrintOption(New AuditLogEntry(FunctionCode, Me), Me._udtSP.PrintOption)
        Else
            Me.udtPrintOptionSelection.setSelectedValue(udtDataEntryUser.PrintOption)
            EHSClaimBasePage.AuditLogSelectPrintOption(New AuditLogEntry(FunctionCode, Me), udtDataEntryUser.PrintOption)
        End If

        Me.udtPrintOptionSelection.setTitle(Me.GetGlobalResourceObject("Text", "SelectPrintFormOption"))

        Me.ModalPopupPrintOptionSelection.Show()
    End Sub

    '---------------------------------------------------------------------------------------------------------
    'Print from event
    '---------------------------------------------------------------------------------------------------------
    Private Sub btnStep2bPrintClaimConsentForm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnStep2aPrintClaimConsentForm.Click, btnStep2bPrintClaimConsentForm.Click
        If Me._blnIsRequireHandlePageRefresh Then
            Return
        End If

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        Dim udtEHSTransaction As EHSTransactionModel = Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)

        Dim strCurrentPrintOption As String = Me.hfCurrentPrintOption.Value

        Dim strPrintOptionSelectedValue As String = Me.rbStep2bPrintClaimConsentFormLanguage.SelectedValue

        Dim imgbtn As ImageButton = CType(sender, ImageButton)

        If imgbtn.ID = "btnStep2aPrintClaimConsentForm" Then
            'Reset Error Image
            If Me.ClaimMode = ClaimMode.COVID19 Then
                Me.udcMsgBoxErr.Clear()

                Me.imgStep2aContactNoError.Visible = False
                Me.imgStep2aDeclareClaimError.Visible = False
                Me.Step2aCleanSchemeErrorImage()
            End If

            '---------------------------------------------------------------------------------------------------------------------------------

            Dim udtFormatter As New Formatter
            Dim udtValidator As New Validator
            'Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
            Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
            Dim udtPracticeDisplay As BLL.PracticeDisplayModel = Me._udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)

            Dim udtDataEntryUser As DataEntryUserModel = Nothing
            Me._udtSessionHandler.CurrentUserGetFromSession(Me._udtSP, udtDataEntryUser)

            Dim udtSubPlatformBLL As New SubPlatformBLL
            Dim strServiceDate As String = udtFormatter.formatInputDate(Me.txtStep2aServiceDate.Text, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))

            Dim isValid As Boolean = True

            Me._udtSystemMessage = udtValidator.chkServiceDate(strServiceDate)

            If Not Me._udtSystemMessage Is Nothing Then
                isValid = False
                Me.imgStep2aServiceDateError.Visible = True
                Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
            Else
                Me.txtStep2aServiceDate.Text = udtFormatter.formatInputTextDate(strServiceDate, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
            End If

            udtSchemeClaim = Me._udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtSchemeClaim.SchemeCode, udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English))

            'Construct EHS Transaction
            Dim udtNewEHSTransaction As EHSTransactionModel = Me._udtEHSClaimBLL.ConstructNewEHSTransaction(udtSchemeClaim, _
                                                                                  udtEHSAccount, _
                                                                                  udtPracticeDisplay, _
                                                                                  udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English), _
                                                                                  GetExtRefStatus(udtEHSAccount, udtSchemeClaim), _
                                                                                  GetDHVaccineRefStatus(udtEHSAccount, udtSchemeClaim))

            If isValid Then

                If Not udtSchemeClaim Is Nothing Then
                    Select Case udtSchemeClaim.ControlType

                        Case SchemeClaimModel.EnumControlType.VSS
                            Dim udcInputVSS As ucInputVSS = Me.udcStep2aInputEHSClaim.GetVSSControl

                            udcInputVSS.SetDoseErrorImage(False)

                            isValid = udcInputVSS.Validate(True, Me.udcMsgBoxErr)

                            If isValid Then
                                udcInputVSS.Save(udtNewEHSTransaction, Me._udtSessionHandler.EHSClaimVaccineGetFromSession())
                            End If

                            ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
                            ' --------------------------------------------------------------------------------------
                        Case SchemeClaimModel.EnumControlType.COVID19

                            Dim udcInputCOVID19 As ucInputCOVID19 = Me.udcStep2aInputEHSClaim.GetCOVID19Control

                            udcInputCOVID19.SetDoseErrorImage(False)

                            isValid = udcInputCOVID19.Validate(True, Me.udcMsgBoxErr)

                            If isValid Then
                                udcInputCOVID19.Save(udtNewEHSTransaction, Me._udtSessionHandler.EHSClaimVaccineGetFromSession())
                            End If
                            ' CRE20-0022 (Immu record) [End][Winnie SUEN]
                    End Select
                End If

                'isValid = Me.Step2aVSSValidation(False, udtNewEHSTransaction)
            End If

            If isValid Then
                'Dim dicVaccineRef As Dictionary(Of String, String) = EHSTransactionModel.GetVaccineRef(GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), udtNewEHSTransaction)
                'Me._udtEHSTransaction.EHSVaccineResult = dicVaccineRef(EHSTransactionModel.VaccineRefType.EHS)
                'Me._udtEHSTransaction.HAVaccineResult = dicVaccineRef(EHSTransactionModel.VaccineRefType.HA)
                'Me._udtEHSTransaction.DHVaccineResult = dicVaccineRef(EHSTransactionModel.VaccineRefType.DH)

                Me._udtEHSClaimBLL.ConstructEHSTransactionDetails(Me._udtSP, udtDataEntryUser, udtNewEHSTransaction, udtEHSAccount, Me._udtSessionHandler.EHSClaimVaccineGetFromSession())

                udtEHSTransaction = udtNewEHSTransaction

            Else
                Dim errorMessageCodeTable As DataTable = Me.udcMsgBoxErr.GetCodeTable
                If errorMessageCodeTable.Rows.Count > 0 Then
                    Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail, udtAuditLogEntry, Common.Component.LogID.LOG00013, String.Format("Enter Claim Detail - Print Consent Form Failed", FunctCode))
                End If

                Return
            End If

            strCurrentPrintOption = Me.hfStep2aCurrentPrintOption.Value
            strPrintOptionSelectedValue = Me.rbStep2aPrintClaimConsentFormLanguage.SelectedValue
        End If
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        Dim strPrintDateTime As String = String.Format("DH_HCV103{0}{1}{2}{3}{4}{5}{6}", Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, Now.Second, Now.Millisecond)
        udtEHSTransaction.PrintedConsentForm = True

        'Set the transaction is printed consent Form
        Me._udtSessionHandler.EHSTransactionSaveToSession(udtEHSTransaction, FunctCode)

        ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
        ' --------------------------------------------------------------------------------------
        'Save the current function code to session (will be removed in the printout form)
        Me._udtSessionHandler.EHSClaimPrintoutFunctionCodeSaveToSession(FunctCode)
        ' CRE20-0022 (Immu record) [End][Winnie SUEN]

        _udtSessionHandler.EHSClaimTempTransactionIDSaveToSession(Me._udtGeneralFunction.generateTemporaryTransactionNumber(udtSchemeClaim.SchemeCode.Trim()))

        Select Case strPrintOptionSelectedValue
            Case PrintOptionLanguage.TradChinese 'PrintOptionValue.Chi
                If strCurrentPrintOption = Common.Component.PrintFormOptionValue.PrintPurposeAndConsent Then
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/EHSClaimForm_CHI_RV.aspx?TID=" + strPrintDateTime + "');", True)
                    EHSClaimBasePage.AuditLogPrintFrom(udtAuditLogEntry, False, "EHSClaimForm_CHI_RV", strPrintOptionSelectedValue, _udtSessionHandler.EHSClaimTempTransactionIDGetFromSession())

                ElseIf strCurrentPrintOption = Common.Component.PrintFormOptionValue.PrintConsentOnly Then
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/EHSClaimCondensedForm_CHI_RV.aspx?TID=" + strPrintDateTime + "');", True)
                    EHSClaimBasePage.AuditLogPrintFrom(udtAuditLogEntry, False, "EHSClaimCondensedForm_CHI_RV", strPrintOptionSelectedValue, _udtSessionHandler.EHSClaimTempTransactionIDGetFromSession())

                End If
            Case PrintOptionLanguage.English 'PrintOptionValue.Eng
                If strCurrentPrintOption = Common.Component.PrintFormOptionValue.PrintPurposeAndConsent Then
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/EHSClaimForm_RV.aspx?TID=" + strPrintDateTime + "');", True)
                    EHSClaimBasePage.AuditLogPrintFrom(udtAuditLogEntry, False, "EHSClaimForm_RV", strPrintOptionSelectedValue, _udtSessionHandler.EHSClaimTempTransactionIDGetFromSession())

                ElseIf strCurrentPrintOption = Common.Component.PrintFormOptionValue.PrintConsentOnly Then
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/EHSClaimCondensedForm_RV.aspx?TID=" + strPrintDateTime + "');", True)
                    EHSClaimBasePage.AuditLogPrintFrom(udtAuditLogEntry, False, "EHSClaimCondensedForm_RV", strPrintOptionSelectedValue, _udtSessionHandler.EHSClaimTempTransactionIDGetFromSession())

                End If

                'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            Case PrintOptionLanguage.SimpChinese
                If strCurrentPrintOption = Common.Component.PrintFormOptionValue.PrintPurposeAndConsent Then
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/EHSClaimForm_CN_RV.aspx?TID=" + strPrintDateTime + "');", True)
                    EHSClaimBasePage.AuditLogPrintFrom(udtAuditLogEntry, False, "EHSClaimForm_CN_RV", strPrintOptionSelectedValue, _udtSessionHandler.EHSClaimTempTransactionIDGetFromSession())

                ElseIf strCurrentPrintOption = Common.Component.PrintFormOptionValue.PrintConsentOnly Then
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/EHSClaimCondensedForm_CN_RV.aspx?TID=" + strPrintDateTime + "');", True)
                    EHSClaimBasePage.AuditLogPrintFrom(udtAuditLogEntry, False, "EHSClaimCondensedForm_CN_RV", strPrintOptionSelectedValue, _udtSessionHandler.EHSClaimTempTransactionIDGetFromSession())

                End If
                'CRE13-019-02 Extend HCVS to China [End][Winnie]
        End Select

        If imgbtn.ID = "btnStep2aPrintClaimConsentForm" Then
            Call Set2aDeclareCheckboxEnable(True)
        Else
            Call Set2bDeclareCheckboxEnable(True)
        End If

    End Sub

    Private Sub btnStep2bPrintAdhocClaimConsentForm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnStep2aPrintAdhocClaimConsentForm.Click, btnStep2bPrintAdhocClaimConsentForm.Click
        If Me._blnIsRequireHandlePageRefresh Then
            Return
        End If

        'Setup AdhocPrintOption
        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        Dim slConsentFormAvailableLang As String() = Nothing
        Dim strConsentFormAvailableVersion As String = Nothing

        Dim intPlatform As Integer = SubPlatform()

        Select Case intPlatform
            Case EnumHCSPSubPlatform.HK
                slConsentFormAvailableLang = udtSchemeClaim.SubsidizeGroupClaimList(0).ConsentFormAvailableLang
                strConsentFormAvailableVersion = udtSchemeClaim.SubsidizeGroupClaimList(0).ConsentFormAvailableVersion

            Case EnumHCSPSubPlatform.CN
                slConsentFormAvailableLang = udtSchemeClaim.SubsidizeGroupClaimList(0).ConsentFormAvailableLang_CN
                strConsentFormAvailableVersion = udtSchemeClaim.SubsidizeGroupClaimList(0).ConsentFormAvailableVersion_CN
        End Select

        Me.dvAdhocPrintFull.Visible = False
        Me.dvAdhocPrintCondense.Visible = False

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
                    Me.dvAdhocPrintFull.Visible = True
                    Me.dvAdhocPrintCondense.Visible = True

                    If slConsentFormAvailableLang IsNot Nothing Then Me.rbAdhocPrintCondenseLang1.Checked = True

                Case PrintFormAvailableVersion.Full
                    Me.dvAdhocPrintFull.Visible = True
                    If slConsentFormAvailableLang IsNot Nothing Then Me.rbAdhocPrintFullLang1.Checked = True

                Case PrintFormAvailableVersion.Condense
                    Me.dvAdhocPrintCondense.Visible = True
                    If slConsentFormAvailableLang IsNot Nothing Then Me.rbAdhocPrintCondenseLang1.Checked = True

            End Select

        End If

        'CRE13-019-02 Extend HCVS to China [End][Winnie]

        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        EHSClaimBasePage.AuditLogAdhocPrintClick(_udtAuditLogEntry)
        ' CRE11-021 log the missed essential information [End]

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        'Me.rbPrintCondencedChi.Checked = True

        'Me.rbPrintCondenced.Checked = False
        'Me.rbPrintCondencedChi.Checked = True
        'Me.rbPrintFull.Checked = False
        'Me.rbPrintFullChi.Checked = False
        'CRE13-019-02 Extend HCVS to China [End][Winnie]

        'CRE15-003 System-generated Form [Start][Philip Chau]
        _udtSessionHandler.EHSClaimTempTransactionIDSaveToSession(Me._udtGeneralFunction.generateTemporaryTransactionNumber(udtSchemeClaim.SchemeCode.Trim()))
        'CRE15-003 System-generated Form [End][Philip Chau]
        Me.ModalPopupExtenderAddHocPrintSelection.Show()
    End Sub

    Private Sub chkStep2bDeclareClaim_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkStep2bDeclareClaim.CheckedChanged
        Me.SetConfirmButtonEnable(Me.btnStep2bConfirm, Me.chkStep2bDeclareClaim.Checked)
    End Sub

    Private Sub udcStep2bReadOnlyEHSClaim_VaccineLegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcStep2bReadOnlyEHSClaim.VaccineLegendClicked
        If Me._blnIsRequireHandlePageRefresh Then
            Return
        End If

        Me.udcSchemeLegend.ShowScheme = False
        Me.udcSchemeLegend.BindSchemeClaim(Me._udtSessionHandler.Language)
        Me.ModalPopupExtenderSchemeLegned.Show()
    End Sub

    '---------------------------------------------------------------------------------------------------------
    'Setup form event
    '---------------------------------------------------------------------------------------------------------
    Private Sub SetupStep2b(ByVal udtEHSAccount As EHSAccountModel, ByVal blnInitAll As Boolean)
        If Me._udtSessionHandler.EHSClaimStepsGetFromSession(FunctCode) <> ActiveViewIndex.Step2b Then
            Me._blnIsRequireHandlePageRefresh = True
            Return
        End If

        Dim udtDataEntryUser As DataEntryUserModel = Nothing
        Dim udtFormatter As Formatter = New Formatter()
        Dim udtEHSTransaction As EHSTransactionModel = Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)
        Dim udtSelectedPracticeDisplay As BLL.PracticeDisplayModel = Me._udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
        Dim blnCOVID19 As Boolean = False
        Dim strPrintOption As String = String.Empty
        Dim blnClaimDeclarationAvailability As Boolean = True
        Dim blnConsentFormAvailability As Boolean = False
        Dim blnPrintOptionAvailability As Boolean = False
        Dim slConsentFormAvailableLang As String() = Nothing
        Dim strConsentFormAvailableVersion As String = Nothing
        Dim intPlatform As Integer = SubPlatform()

        Me.hfCurrentPrintOption.Value = Nothing

        ' To Handle Concurrent Browser
        If Not Me.EHSClaimTokenNumValidation() Then
            Return
        End If

        'Clear all control which in Enter Claim input, for viewstate problem
        Me.udcStep2aInputEHSClaim.Clear()

        'Setup Claim Declaration Availability
        blnClaimDeclarationAvailability = SetClaimDeclarationAvailability(udtSchemeClaim, udtEHSTransaction)

        Select Case intPlatform
            Case EnumHCSPSubPlatform.HK
                blnConsentFormAvailability = udtSchemeClaim.SubsidizeGroupClaimList(0).ConsentFormAvailable
                blnPrintOptionAvailability = udtSchemeClaim.SubsidizeGroupClaimList(0).PrintOptionAvailable
                slConsentFormAvailableLang = udtSchemeClaim.SubsidizeGroupClaimList(0).ConsentFormAvailableLang
                strConsentFormAvailableVersion = udtSchemeClaim.SubsidizeGroupClaimList(0).ConsentFormAvailableVersion

            Case EnumHCSPSubPlatform.CN
                blnConsentFormAvailability = udtSchemeClaim.SubsidizeGroupClaimList(0).ConsentFormAvailable_CN
                blnPrintOptionAvailability = udtSchemeClaim.SubsidizeGroupClaimList(0).PrintOptionAvailable_CN
                slConsentFormAvailableLang = udtSchemeClaim.SubsidizeGroupClaimList(0).ConsentFormAvailableLang_CN
                strConsentFormAvailableVersion = udtSchemeClaim.SubsidizeGroupClaimList(0).ConsentFormAvailableVersion_CN
        End Select

        Me.SetConfirmButtonEnable(Me.btnStep2bConfirm, Me.chkStep2bDeclareClaim.Checked)

        'if the first subsidize need print form -> this scheme need to print form 

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        'If udtSchemeClaim.SubsidizeGroupClaimList(0).ConsentFormAvailable Then
        If blnConsentFormAvailability Then
            'CRE13-019-02 Extend HCVS to China [End][Winnie]
            Me.panStep2bPrintClaimConsentForm.Visible = True

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

            Me.PrintClaimConsentFormLanguageSetup(slConsentFormAvailableLang, rbStep2bPrintClaimConsentFormLanguage)
            'CRE13-019-02 Extend HCVS to China [End][Winnie]            

            Me.ChangePrintFormControl(strPrintOption, udtSchemeClaim, Me.panStep2bAdhocPrint, Me.panlblStep2bPrintConsent, _
                                      Me.panStep2bPerprintFormNotice, Me.btnStep2bPrintAdhocClaimConsentForm, Me.btnStep2bPrintClaimConsentForm)
        Else
            strPrintOption = Common.Component.PrintFormOptionValue.PreprintForm
            Me.panStep2bPrintClaimConsentForm.Visible = False
        End If

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]            
        Me.hfCurrentPrintOption.Value = strPrintOption
        'CRE13-019-02 Extend HCVS to China [End][Winnie]            

        ' CRE13-001 - EHAPP [Start][Tommy L]
        ' -------------------------------------------------------------------------------------
        If blnInitAll AndAlso blnClaimDeclarationAvailability Then
            ' CRE13-001 - EHAPP [End][Tommy L]
            Me.DisableConfirmDeclareCheckBox(strPrintOption)
            Me.chkStep2bDeclareClaim.Checked = False
        End If

        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        If _udtSessionHandler.NonClinicSettingGetFromSession(FunctCode) Then
            panNonClinicSettingStep2b.Visible = True
            If _udtSessionHandler.Language = CultureLanguage.TradChinese Then
                lblNonClinicSettingStep2b.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)))
            ElseIf _udtSessionHandler.Language = CultureLanguage.SimpChinese Then
                lblNonClinicSettingStep2b.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese)))
            Else
                lblNonClinicSettingStep2b.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.English)))
            End If
        Else
            panNonClinicSettingStep2b.Visible = False
        End If
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

        'Setup Lable Value 
        If Me._udtSessionHandler.Language() = Common.Component.CultureLanguage.TradChinese Then
            Me.lblStep2bScheme.Text = udtSchemeClaim.SchemeDescChi
            Me.lblStep2bServiceType.Text = udtEHSTransaction.ServiceTypeDesc_Chi
            Me.lblStep2bPractice.Text = String.Format("{0} ({1})", udtSelectedPracticeDisplay.PracticeNameChi, udtSelectedPracticeDisplay.PracticeID)
            Me.lblStep2bPractice.CssClass = "tableTextChi"
        ElseIf Me._udtSessionHandler.Language() = Common.Component.CultureLanguage.SimpChinese Then
            Me.lblStep2bScheme.Text = udtSchemeClaim.SchemeDescCN
            Me.lblStep2bServiceType.Text = udtEHSTransaction.ServiceTypeDesc_CN
            Me.lblStep2bPractice.Text = String.Format("{0} ({1})", udtSelectedPracticeDisplay.PracticeNameChi, udtSelectedPracticeDisplay.PracticeID)
            Me.lblStep2bPractice.CssClass = "tableTextChi"
        Else
            Me.lblStep2bScheme.Text = udtSchemeClaim.SchemeDesc
            Me.lblStep2bServiceType.Text = udtEHSTransaction.ServiceTypeDesc
            Me.lblStep2bPractice.Text = String.Format("{0} ({1})", udtSelectedPracticeDisplay.PracticeName, udtSelectedPracticeDisplay.PracticeID)
            Me.lblStep2bPractice.CssClass = "tableText"
        End If

        Me.lblStep2bBankAcct.Text = udtFormatter.maskBankAccount(udtEHSTransaction.BankAccountNo)
        Me.lblStep2bServiceDate.Text = udtFormatter.formatDisplayDate(udtEHSTransaction.ServiceDate, Me._udtSessionHandler.Language())

        If udtEHSTransaction.TransactionDetails.FilterBySubsidizeItemDetail(SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19).Count > 0 Then
            blnCOVID19 = True
        End If

        'Setup Personal information 
        Me.udcStep2bReadOnlyDocumnetType.DocumentType = udtEHSAccount.SearchDocCode
        Me.udcStep2bReadOnlyDocumnetType.EHSAccount = udtEHSAccount
        Me.udcStep2bReadOnlyDocumnetType.Vertical = False
        Me.udcStep2bReadOnlyDocumnetType.MaskIdentityNo = True
        Me.udcStep2bReadOnlyDocumnetType.ShowAccountRefNo = False
        Me.udcStep2bReadOnlyDocumnetType.ShowTempAccountNotice = False
        Me.udcStep2bReadOnlyDocumnetType.ShowAccountCreationDate = False
        Me.udcStep2bReadOnlyDocumnetType.TableTitleWidth = 205
        Me.udcStep2bReadOnlyDocumnetType.SetEnableToShowHKICSymbol = True
        Me.udcStep2bReadOnlyDocumnetType.Built()

        'setup Transaction detail
        Me.udcStep2bReadOnlyEHSClaim.EHSClaimVaccine = Me._udtSessionHandler.EHSClaimVaccineGetFromSession()
        Me.udcStep2bReadOnlyEHSClaim.EHSTransaction = udtEHSTransaction
        Me.udcStep2bReadOnlyEHSClaim.SchemeCode = udtSchemeClaim.SchemeCode
        Me.udcStep2bReadOnlyEHSClaim.Mode = ucReadOnlyEHSClaim.ReadOnlyEHSClaimMode.Normal
        Me.udcStep2bReadOnlyEHSClaim.TableTitleWidth = 205
        Me.udcStep2bReadOnlyEHSClaim.ClaimMode = IIf(blnCOVID19, Common.Component.ClaimMode.COVID19, Common.Component.ClaimMode.All)
        Me.udcStep2bReadOnlyEHSClaim.Built()

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ' Display setting for COVID-19
        If blnCOVID19 Then
            panStep2bRecipinetContactInfo.Visible = True
            trStep2bBankAcct.Style.Add("display", "none")
            trStep2bServiceType.Style.Add("display", "none")
            lblStep2bContactNoNotAbleSMS.Visible = False
            'Contact No. & Mobile
            If udtEHSTransaction.TransactionAdditionFields.ContactNo IsNot Nothing AndAlso udtEHSTransaction.TransactionAdditionFields.ContactNo <> String.Empty Then
                trStep2bContactNo.Visible = True

                'If udtEHSTransaction.TransactionAdditionFields.ContactNo <> String.Empty Then
                lblStep2bContactNo.Text = udtEHSTransaction.TransactionAdditionFields.ContactNo

                If udtEHSTransaction.TransactionAdditionFields.Mobile IsNot Nothing And udtEHSTransaction.TransactionAdditionFields.Mobile = YesNo.Yes Then
                    lblStep2bContactNo.Text = lblStep2bContactNo.Text & " (" & GetGlobalResourceObject("Text", "Mobile") & ")"
                End If

                Select Case Left(udtEHSTransaction.TransactionAdditionFields.ContactNo, 1)
                    Case "2", "3"
                        lblStep2bContactNoNotAbleSMS.Visible = True
                    Case Else
                        lblStep2bContactNoNotAbleSMS.Visible = False
                End Select

                'Else
                '    lblStep2bContactNo.Text = GetGlobalResourceObject("Text", "NotProvided")
                'End If
            Else
                trStep2bContactNo.Visible = False
            End If

            'Remarks
            If udtEHSTransaction.TransactionAdditionFields.Remarks IsNot Nothing And udtEHSTransaction.TransactionAdditionFields.Remarks <> String.Empty Then
                lblStep2bRemark.Text = udtEHSTransaction.TransactionAdditionFields.Remarks
            Else
                lblStep2bRemark.Text = GetGlobalResourceObject("Text", "NotProvided")
            End If

            'Join EHRSS
            If (udtEHSTransaction.SchemeCode.Trim.ToUpper() = SchemeClaimModel.COVID19CVC OrElse _
                udtEHSTransaction.SchemeCode.Trim.ToUpper() = SchemeClaimModel.COVID19DH OrElse _
                udtEHSTransaction.SchemeCode.Trim.ToUpper() = SchemeClaimModel.COVID19RVP OrElse _
                udtEHSTransaction.SchemeCode.Trim.ToUpper() = SchemeClaimModel.COVID19OR OrElse _
                udtEHSTransaction.SchemeCode.Trim.ToUpper() = SchemeClaimModel.COVID19SR OrElse _
                udtEHSTransaction.SchemeCode.Trim.ToUpper() = SchemeClaimModel.VSS OrElse _
                udtEHSTransaction.SchemeCode.Trim.ToUpper() = SchemeClaimModel.RVP) AndAlso _
               (udtEHSAccount.SearchDocCode = DocTypeModel.DocTypeCode.HKIC OrElse _
                udtEHSAccount.SearchDocCode = DocTypeModel.DocTypeCode.EC OrElse _
                udtEHSAccount.SearchDocCode = DocTypeModel.DocTypeCode.OW) Then

                panStep2bJoinEHRSS.Visible = True
                If udtEHSTransaction.TransactionAdditionFields.JoinEHRSS IsNot Nothing And udtEHSTransaction.TransactionAdditionFields.JoinEHRSS <> String.Empty Then
                    lblStep2bJoinEHRSS.Text = IIf(udtEHSTransaction.TransactionAdditionFields.JoinEHRSS = YesNo.Yes, _
                                               GetGlobalResourceObject("Text", "Yes"), _
                                               GetGlobalResourceObject("Text", "No"))

                Else
                    lblStep2bJoinEHRSS.Text = GetGlobalResourceObject("Text", "NA")
                End If
            Else
                panStep2bJoinEHRSS.Visible = False
            End If

            'Override the print option, disable the declare checkbox and enable the confirm button
            If udtEHSTransaction.SchemeCode = SchemeClaimModel.VSS Or udtEHSTransaction.SchemeCode = SchemeClaimModel.RVP Then
                Me.panStep2bPrintClaimConsentForm.Visible = False
                Me.trStep2bDeclareClaim.Visible = False
                Me.chkStep2bDeclareClaim.Checked = True
                Me.SetConfirmButtonEnable(Me.btnStep2bConfirm, Me.chkStep2bDeclareClaim.Checked)
            End If

        Else
            panStep2bRecipinetContactInfo.Visible = False
            trStep2bBankAcct.Style.Remove("display")
            trStep2bServiceType.Style.Remove("display")
        End If

        ' CRE20-0022 (Immu record) [End][Chris YIM]

    End Sub

    Private Sub Step2bClear()
        Me.udcStep2bReadOnlyDocumnetType.Clear()
        Me.udcStep2bReadOnlyEHSClaim.Clear()
    End Sub

    ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [Start][Winnie]
    Protected Sub Step2bClaimSubmit()
        If Me._blnIsRequireHandlePageRefresh Then
            Return
        End If

        Dim udtDataEntryUser As DataEntryUserModel = Nothing
        Dim udtFormatter As Formatter = New Formatter()
        Dim udtEHSTransaction As EHSTransactionModel = Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)
        Dim udtSelectedPracticeDisplay As BLL.PracticeDisplayModel = Me._udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = Me._udtSessionHandler.EHSClaimVaccineGetFromSession()
        Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Dim udtTransactionBLL As EHSTransactionBLL = New EHSTransactionBLL()
        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(FunctCode, Me)
        Dim udtSmartIDContent As BLL.SmartIDContentModel = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode)
        Dim udtEHSPersonalInfo As EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode.Trim())

        Dim isValid As Boolean = True
        Dim strOrignalEHSAccountID As String = Nothing
        Dim blnVaccineType As Boolean = True
        Dim isCreateBySmartID As Boolean = False
        Dim blnCreateAdment As Boolean = False

        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Dim strSmartIDVersion As String = String.Empty
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

        EHSClaimBasePage.AuditLogConfirmClaimDetailStart(udtAuditLogEntry, udtSmartIDContent)

        ' To Handle Concurrent Browser
        If Not Me.EHSClaimTokenNumValidation() Then
            Return
        End If

        Me._udtSessionHandler.CurrentUserGetFromSession(Me._udtSP, udtDataEntryUser)

        '---------------------------------------------------------------------
        ' Case 1. Double Submit and redirect to Concurrent update Error Page
        '---------------------------------------------------------------------
        If isValid Then
            If (Not udtEHSTransaction.TransactionID Is Nothing AndAlso Not udtEHSTransaction.TransactionID.Trim().Equals(String.Empty)) OrElse (Not udtEHSTransaction.IsNew) Then
                isValid = False
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.EHSClaimError
            End If
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
                strSmartIDVersion = udtSmartIDContent.SmartIDVer()
                udtEHSTransaction.SmartIDVer = strSmartIDVersion
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

            End If
            '===================================================================================================================================================================

            udtEHSTransaction.TransactionID = Me._udtGeneralFunction.generateTransactionNumber(udtSchemeClaim.SchemeCode.Trim())

            If Not udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.SpecialAccount Then
                    strOrignalEHSAccountID = udtEHSAccount.OriginalAccID

                ElseIf (Not udtEHSAccount.TransactionID Is Nothing AndAlso Not udtEHSAccount.TransactionID.Equals(String.Empty)) _
                    OrElse (Not udtEHSAccount.CreateSPID Is Nothing AndAlso Not udtEHSAccount.CreateSPID.Equals(Me._udtSP.SPID)) Then
                    'If udtEHSAccount is created by smart ID, CreateSPID is nothing
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
                    udtEHSAccount.VoucherAccID = Me._udtGeneralFunction.generateSystemNum("X")

                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    If isCreateBySmartID Then
                        udtEHSAccount.EHSPersonalInformationList(0).CreateBySmartID = True
                        udtEHSAccount.EHSPersonalInformationList(0).SmartIDVer = strSmartIDVersion
                    Else
                        udtEHSAccount.EHSPersonalInformationList(0).CreateBySmartID = False
                        udtEHSAccount.EHSPersonalInformationList(0).SmartIDVer = String.Empty
                    End If
                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
                End If

                If udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVaccine Then
                    Me._udtEHSClaimBLL.ConstructEHSTransactionDetails(Me._udtSP, udtDataEntryUser, udtEHSTransaction, udtEHSAccount, udtEHSClaimVaccine)

                ElseIf udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeRegistration Then
                    blnVaccineType = False
                    Me._udtEHSClaimBLL.ConstructEHSTransDetail_Registration(Me._udtSP, udtDataEntryUser, udtEHSTransaction, udtEHSAccount)

                    ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                ElseIf udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeType_HAService Then
                    blnVaccineType = False
                    Me._udtEHSClaimBLL.ConstructEHSTransactionDetail_SSSCMC(Me._udtSP, udtDataEntryUser, udtEHSTransaction, udtEHSAccount, Me._udtSessionHandler.HAPatientGetFromSession())
                    ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

                Else
                    blnVaccineType = False
                    Me._udtEHSClaimBLL.ConstructEHSTransactionDetails(Me._udtSP, udtDataEntryUser, udtEHSTransaction, udtEHSAccount)
                End If

                Try
                    If blnCreateAdment Then
                        Me._udtSystemMessage = Me._udtEHSClaimBLL.CreateAmendEHSAccountEHSTransaction(Me._udtSP, udtDataEntryUser, udtEHSTransaction, udtSmartIDContent.EHSValidatedAccount, udtSmartIDContent.EHSAccount, udtSchemeClaim, EHSTransactionModel.AppSourceEnum.WEB_FULL)
                    Else
                        Me._udtSystemMessage = Me._udtEHSClaimBLL.CreateXEHSAccountEHSTransaction(Me._udtSP, udtDataEntryUser, udtEHSTransaction, udtEHSAccount, udtSchemeClaim)
                    End If
                Catch eSql As SqlClient.SqlException
                    If eSql.Number = 50000 Then
                        '----------------------------------------
                        ' Case 1 Checking
                        '----------------------------------------
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage(New SystemMessage("990001", "D", eSql.Message))
                        'Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.EHSClaimError
                    Else
                        Throw eSql
                    End If
                End Try

                'I-CRE17-005 (Performance Tuning) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                If Me._udtSystemMessage Is Nothing Then
                    If blnCreateAdment Then
                        'udtSmartIDContent.EHSAccount is retrieved form database after Admentment Record was created 
                        Me._udtSessionHandler.EHSAccountSaveToSession(udtSmartIDContent.EHSAccount, FunctCode)
                        Me._udtSessionHandler.SmartIDContentSaveToSession(FunctCode, udtSmartIDContent)
                    End If
                End If
                'I-CRE17-005 (Performance Tuning) [End][Chris YIM]

            Else

                If udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVaccine Then
                    Me._udtEHSClaimBLL.ConstructEHSTransactionDetails(Me._udtSP, udtDataEntryUser, udtEHSTransaction, udtEHSAccount, udtEHSClaimVaccine)
                    ' CRE13-001 - EHAPP [Start][Tommy L]
                    ' -------------------------------------------------------------------------------------
                ElseIf udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeRegistration Then
                    blnVaccineType = False
                    Me._udtEHSClaimBLL.ConstructEHSTransDetail_Registration(Me._udtSP, udtDataEntryUser, udtEHSTransaction, udtEHSAccount)
                    ' CRE13-001 - EHAPP [End][Tommy L]

                    ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                ElseIf udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeType_HAService Then
                    blnVaccineType = False
                    Me._udtEHSClaimBLL.ConstructEHSTransactionDetail_SSSCMC(Me._udtSP, udtDataEntryUser, udtEHSTransaction, udtEHSAccount, Me._udtSessionHandler.HAPatientGetFromSession())
                    ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

                Else
                    blnVaccineType = False
                    Me._udtEHSClaimBLL.ConstructEHSTransactionDetails(Me._udtSP, udtDataEntryUser, udtEHSTransaction, udtEHSAccount)
                End If

                Try

                    Dim blnAutoConfirm As Boolean = False
                    ' ---------------------------------------------------------------------------------------------------------------------------------------
                    ' 1. Temporary EHS Account Create By DataEntry of Same SPID, and the record status = Pending Confirmation, no transaction follow and once SP reuse this account, Auto Confirm
                    ' ---------------------------------------------------------------------------------------------------------------------------------------
                    ' ---------------------------------------------------------------------------------------------------------------------------------------
                    ' 2. Other Scenior will not auto confirm the EHSAccount
                    ' ---------------------------------------------------------------------------------------------------------------------------------------
                    If udtDataEntryUser Is Nothing AndAlso udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.TemporaryAccount _
                        AndAlso udtEHSAccount.CreateSPID.Trim().Equals(Me._udtSP.SPID.Trim()) AndAlso udtEHSAccount.DataEntryBy.Trim() <> "" _
                        AndAlso udtEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingConfirmation Then
                        blnAutoConfirm = True
                    End If

                    Me._udtSystemMessage = Me._udtEHSClaimBLL.CreateEHSTransaction(udtEHSTransaction, udtEHSAccount, udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode), udtSchemeClaim, blnAutoConfirm)

                Catch eSql As SqlClient.SqlException
                    If eSql.Number = 50000 Then
                        isValid = False
                        Me.udcMsgBoxErr.AddMessage(New SystemMessage("990001", "D", eSql.Message))
                        'Throw eSql
                    Else
                        Throw eSql
                    End If
                End Try
            End If

            'I-CRE17-005 (Performance Tuning) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            If Not Me._udtSystemMessage Is Nothing Then
                isValid = False
                Select Case Me._udtSystemMessage.MessageCode
                    Case MsgCode.MSG00197
                        Me._blnConcurrentUpdate = True
                        Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.EHSClaimError
                    Case Else
                        Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                End Select

            End If
            'I-CRE17-005 (Performance Tuning) [End][Chris YIM]

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
                            Me._udtSessionHandler.SmartIDContentSaveToSession(FunctCode, udtSmartIDContent)
                    End Select
                End If

                'if Success
                udtEHSTransaction = udtTransactionBLL.LoadEHSTransaction(udtEHSTransaction.TransactionID)
                Me._udtSessionHandler.EHSTransactionSaveToSession(udtEHSTransaction, FunctCode)

                If blnVaccineType Then
                    udtEHSClaimVaccine = Me._udtEHSClaimBLL.ConstructEHSClaimVaccineModel(udtEHSTransaction.SchemeCode, udtEHSTransaction)
                    Me._udtSessionHandler.EHSClaimVaccineSaveToSession(udtEHSClaimVaccine)
                End If

                'Log
                If ClaimMode = Common.Component.ClaimMode.COVID19 AndAlso udtSchemeClaim.SchemeCode = SchemeClaimModel.COVID19CVC Then
                    EHSClaimBasePage.AddDescriptionCentreBooth(udtAuditLogEntry, udtEHSTransaction.ServiceProviderID, udtEHSTransaction.PracticeID)
                End If

                EHSClaimBasePage.AuditLogConfirmClaimDetailPassed(udtAuditLogEntry, FunctCode, udtEHSTransaction, udtSmartIDContent, blnCreateAdment)

                If Me.IsSkipClaimCompletePage Then
                    btnStep3NextClaim_Click(Nothing, Nothing)
                Else
                    Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step3

                    'CRE20-0022 (Immu record) [Start][Raiman]
                    'Print Consent form when SP process to show detail page.
                    If Me.ClaimMode = ClaimMode.COVID19 Then
                        Step3ClaimSubmitPrintConsentForm()
                    End If
                    'CRE20-0022 (Immu record) [End][Raiman]
                End If

            Else
                EHSClaimBasePage.AuditLogConfirmClaimDetailFailed(udtAuditLogEntry, FunctCode)
                Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail)
            End If

        End If
    End Sub
    ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [End][Winnie]
#End Region

#Region "Step 3 Complete claim"

    Protected Sub btnStep3NextClaim_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnStep3NextClaim.Click
        ' To Handle Concurrent Browser
        If Not Me.EHSClaimTokenNumValidation() Then
            Return
        End If

        If Me._blnIsRequireHandlePageRefresh Then
            Return
        End If

        ' Log Next Claim Clicked
        EHSClaimBasePage.AuditLogNextClaim(New AuditLogEntry(FunctionCode, Me))

        _udtSessionHandler.ClearVREClaim()
        Me.Clear()

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Me._udtSessionHandler.HKICSymbolRemoveFromSession(FunctionCode)
        Me._udtSessionHandler.OCSSSRefStatusRemoveFromSession(FunctCode)
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        Me._udtSessionHandler.ForceToReadSmartICRemoveFormSession()

        Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
    End Sub

    Protected Sub btnStep3Reprint_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnStep3Reprint.Click
        ' To Handle Concurrent Browser
        If Not Me.EHSClaimTokenNumValidation() Then
            Return
        End If

        If Me._blnIsRequireHandlePageRefresh Then
            Return
        End If

        Step3ClaimSubmitPrintConsentForm()
    End Sub

    Private Sub btnStep3ClaimForSamePatient_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnStep3ClaimForSamePatient.Click
        ' To Handle Concurrent Browser
        If Not Me.EHSClaimTokenNumValidation() Then
            Return
        End If

        If Me._blnIsRequireHandlePageRefresh Then
            Return
        End If

        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
        Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtFormatter As Formatter = New Formatter()
        Dim strDOB As String = udtFormatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, Me._udtSessionHandler.Language(), udtEHSPersonalInfo.ECAge, udtEHSPersonalInfo.ECDateOfRegistration)
        Dim udtSmartIDContent As BLL.SmartIDContentModel = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode)
        Dim blnNotMatchAccountExist As Boolean
        Dim blnExceedDocTypeLimit As Boolean
        Dim udtEligibleResult As EligibleResult = Nothing

        'Search the updated EHS Account Again
        Dim udtSearchAccountStatus As New EHSClaimBLL.SearchAccountStatus

        ' CRE20-0022 (Immu record) [Start][Martin]
        Select Case udtEHSAccount.SearchDocCode
            Case DocTypeModel.DocTypeCode.HKIC, _
                DocTypeModel.DocTypeCode.HKBC, _
                DocTypeModel.DocTypeCode.ADOPC, _
                DocTypeModel.DocTypeCode.REPMT, _
                DocTypeModel.DocTypeCode.ID235B, _
                DocTypeModel.DocTypeCode.VISA, _
                DocTypeModel.DocTypeCode.DI, _
                DocTypeModel.DocTypeCode.CCIC, _
                DocTypeModel.DocTypeCode.ROP140, _
                DocTypeModel.DocTypeCode.PASS
                ' CRE20-0022 (Immu record) [End][Martin]

                If udtEHSAccount.SearchDocCode = DocTypeModel.DocTypeCode.ADOPC Then
                    _udtSystemMessage = _udtEHSClaimBLL.SearchEHSAccount(udtSchemeClaim.SchemeCode.Trim, udtEHSAccount.SearchDocCode, _
                        udtEHSPersonalInfo.IdentityNum, strDOB, udtEHSAccount, udtEligibleResult, udtSearchAccountStatus, udtEHSPersonalInfo, _
                        udtEHSPersonalInfo.AdoptionPrefixNum, FunctionCode, Me.ClaimMode)

                Else
                    If Not udtSmartIDContent Is Nothing AndAlso udtSmartIDContent.IsReadSmartID Then
                        '==================================================================== Code for SmartID ============================================================================
                        'Select Case udtSmartIDContent.SmartIDReadStatus
                        '    Case BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_SameDetail, _
                        '            BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_NotCreateBySmartID_SameDOB_NoCCCode

                        '        Me._udtSystemMessage = Me._udtEHSClaimBLL.SearchEHSAccount(udtSchemeClaim.SchemeCode.Trim(), udtEHSAccount.SearchDocCode, udtEHSPersonalInfo.IdentityNum, strDOB, udtEHSAccount, udtEligibleResult, blnNotMatchAccountExist, blnExceedDocTypeLimit)
                        '    Case Else
                        '        Me._udtSystemMessage = Me._udtEHSClaimBLL.SearchTemporaryAccount(udtSchemeClaim.SchemeCode.Trim(), udtEHSAccount.VoucherAccID, udtEHSAccount, udtEligibleResult, blnExceedDocTypeLimit)
                        'End Select

                        'udtEHSAccount.SetSearchDocCode(DocTypeModel.DocTypeCode.HKIC)
                        Dim blnKeepSmartIDContent As Boolean = False

                        If udtSmartIDContent.IsCreateAmendEHSAccount Then
                            If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                                udtSmartIDContent.IsCreateAmendEHSAccount = False
                            ElseIf udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.TemporaryAccount Then
                                blnKeepSmartIDContent = True

                            End If
                        End If

                        ' CRE20-0023 (Immu record) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                        Me._udtSystemMessage = Me._udtEHSClaimBLL.SearchEHSAccountSmartID(udtSchemeClaim.SchemeCode.Trim(), _
                                                                                          DocTypeModel.DocTypeCode.HKIC, _
                                                                                          udtEHSPersonalInfo.IdentityNum, _
                                                                                          strDOB, _
                                                                                          udtEHSAccount, _
                                                                                          udtSmartIDContent.EHSAccount, _
                                                                                          udtSmartIDContent.SmartIDReadStatus, _
                                                                                          udtEligibleResult, _
                                                                                          blnNotMatchAccountExist, _
                                                                                          blnExceedDocTypeLimit, _
                                                                                          FunctionCode, _
                                                                                          True, _
                                                                                          Me.ClaimMode)
                        ' CRE20-0023 (Immu record) [End][Chris YIM]

                        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                        ' ----------------------------------------------------------
                        ' Restore the HKIC Symbol
                        udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).HKICSymbol = _udtSessionHandler.HKICSymbolGetFormSession(FunctCode)
                        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

                        If blnKeepSmartIDContent Then
                            udtEHSAccount = udtSmartIDContent.EHSAccount
                        Else
                            udtSmartIDContent.EHSAccount = udtEHSAccount
                        End If

                        Me._udtSessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctCode)
                        Me._udtSessionHandler.SmartIDContentSaveToSession(FunctCode, udtSmartIDContent)

                        'Me._udtSystemMessage  is not nothing : systemmessage stored the complete transaction information message
                        'Me._udtSystemMessage = Nothing
                        '===================================================================================================================================================================

                    Else

                        _udtSystemMessage = _udtEHSClaimBLL.SearchEHSAccount(udtSchemeClaim.SchemeCode.Trim, udtEHSAccount.SearchDocCode, _
                            udtEHSPersonalInfo.IdentityNum, strDOB, udtEHSAccount, udtEligibleResult, udtSearchAccountStatus, udtEHSPersonalInfo, _
                            String.Empty, FunctionCode, Me.ClaimMode)

                        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                        ' -------------------------------------------------------------------------------
                        ' If selected Doc Code is different the one in udtEHSAccount, Search Temporary Account again, Check Account Status
                        ' -------------------------------------------------------------------------------
                        Dim udtEHSAccountPersonalInfo As EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSPersonalInfo.DocCode)
                        If udtEHSAccountPersonalInfo Is Nothing Then

                            _udtEHSClaimBLL.SearchTemporaryAccount(udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, strDOB, udtEHSAccount, _
                                udtSearchAccountStatus, udtEHSPersonalInfo)

                            udtSearchAccountStatus.NotMatchAccountExist = udtSearchAccountStatus.OnlyInvalidAccountFound OrElse udtSearchAccountStatus.TempAccountNotMatchDOBFound

                            Me._udtSessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctCode)
                            _udtSessionHandler.SearchAccountStatusSaveToSession(udtSearchAccountStatus)
                        End If
                        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

                        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                        ' ----------------------------------------------------------
                        ' Restore the HKIC Symbol 
                        If udtEHSAccount.SearchDocCode = DocTypeModel.DocTypeCode.HKIC Then
                            udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).HKICSymbol = _udtSessionHandler.HKICSymbolGetFormSession(FunctCode)
                            Me._udtSessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctCode)
                        End If
                        ' CRE17-010 (OCSSS integration) [End][Chris YIM]
                    End If
                End If

            Case DocTypeModel.DocTypeCode.EC

                If udtEHSPersonalInfo.ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration AndAlso udtEHSPersonalInfo.ECAge.HasValue Then
                    _udtSystemMessage = _udtEHSClaimBLL.SearchEHSAccount(udtSchemeClaim.SchemeCode.Trim, _
                                            udtEHSAccount.SearchDocCode, udtEHSPersonalInfo.IdentityNum, udtEHSPersonalInfo.ECAge.Value, _
                                            udtEHSPersonalInfo.ECDateOfRegistration.Value, udtEHSAccount, udtEligibleResult, _
                                            udtSearchAccountStatus, udtEHSPersonalInfo, FunctionCode, Me.ClaimMode)

                Else
                    _udtSystemMessage = _udtEHSClaimBLL.SearchEHSAccount(udtSchemeClaim.SchemeCode.Trim, udtEHSAccount.SearchDocCode, _
                        udtEHSPersonalInfo.IdentityNum, strDOB, udtEHSAccount, udtEligibleResult, udtSearchAccountStatus, udtEHSPersonalInfo, _
                        String.Empty, FunctionCode, Me.ClaimMode)

                End If

        End Select


        ' -------------------------------------------------------------------------------
        ' 9. Benefit Error 
        ' -------------------------------------------------------------------------------
        If Not Me._udtSystemMessage Is Nothing Then
            If Me._udtSystemMessage.MessageCode = "00195" Then
                Me._udtSystemMessage = Nothing
            ElseIf Me._udtSystemMessage.MessageCode = "00107" Or Me._udtSystemMessage.MessageCode = "00255" Then
                Me._udtSystemMessage = Nothing
            End If
        End If

        'Account with transaction voided, before click this button
        If Not udtEHSAccount.IsNew AndAlso Me._udtSystemMessage Is Nothing Then

            If udtEligibleResult Is Nothing Then
                Me._udtSessionHandler.EligibleResultRemoveFromSession()
            Else
                'Remove all rules from session since some rules may produced by enter claim detail
                Me._udtSessionHandler.EligibleResultRemoveFromSession()

                'CIVSS rule: Key should be same as step1 <- to be implement if more then one rules return
                Dim udtRuleResults As RuleResultCollection = New RuleResultCollection()
                udtEligibleResult.PromptConfirmed = True
                udtRuleResults.Add(Me.RuleResultKey(ActiveViewIndex.Step1, RuleTypeENum.EligibleResult), udtEligibleResult)

                Me._udtSessionHandler.EligibleResultSaveToSession(udtRuleResults)
            End If

            Me._udtSessionHandler.EHSTransactionRemoveFromSession(FunctCode)
            Me._udtSessionHandler.EHSClaimVaccineRemoveFromSession()
            Me._udtSessionHandler.SchemeSubsidizeListRemoveFromSession(FunctCode)
            Me._udtSessionHandler.ClaimCategoryRemoveFromSession(FunctCode)
            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            _udtEHSTransactionBLL.ClearSessionTransactionDetailBenefit()
            _udtEHSTransactionBLL.ClearSessionTransactionDetailVaccine()
            ' CRE20-0022 (Immu record) [End][Chris YIM]

            Me._udtSessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctCode)
            Me._udtSessionHandler.ClaimForSamePatientSaveToSession(True, FunctCode)

            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step2a

            EHSClaimBasePage.AuditLogClaimForSamePatient(New AuditLogEntry(FunctionCode, Me))
            EHSClaimBasePage.AuditLogEnterClaimDetailLoaded(New AuditLogEntry(FunctionCode, Me))
        Else
            Me._udtSessionHandler.EHSTransactionRemoveFromSession(FunctCode)
            Me._udtSessionHandler.EHSClaimVaccineRemoveFromSession()
            Me._udtSessionHandler.EHSAccountRemoveFromSession(FunctCode)
            Me._udtSessionHandler.SchemeSubsidizeListRemoveFromSession(FunctCode)

            Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.EHSClaimError
        End If

    End Sub

    Private Sub udcStep3ReadOnlyEHSClaim_VaccineLegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcStep3ReadOnlyEHSClaim.VaccineLegendClicked
        If Me._blnIsRequireHandlePageRefresh Then
            Return
        End If

        Me.udcSchemeLegend.ShowScheme = False
        Me.udcSchemeLegend.BindSchemeClaim(Me._udtSessionHandler.Language)
        Me.ModalPopupExtenderSchemeLegned.Show()

    End Sub

    Private Sub SetupStep3(ByVal udtEHSAccount As EHSAccountModel, ByVal activeViewChange As Boolean)
        If Me._udtSessionHandler.EHSClaimStepsGetFromSession(FunctCode) <> ActiveViewIndex.Step3 Then
            Me._blnIsRequireHandlePageRefresh = True
            Return
        End If

        Dim udtFormatter As Formatter = New Formatter()
        Dim udtEHSTransaction As EHSTransactionModel = Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)
        Dim udtSelectedPracticeDisplay As BLL.PracticeDisplayModel = Me._udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
        Dim blnCOVID19 As Boolean = False

        If Not udtEHSTransaction.PrintedConsentForm Then
            _udtSessionHandler.EHSClaimTempTransactionIDRemoveFromSession()
        End If

        Dim strTransactionIDPrefix As String = _udtSessionHandler.EHSClaimTempTransactionIDGetFromSession()
        Dim strSchemeSetupModelSetupType As String = SchemeSetupModel.SetupType_ClaimCompletionMessage
        If Me._udtEHSClaimBLL.chkIsPrefixAndTransactionIDTheSame(strTransactionIDPrefix, udtEHSTransaction) Then
            Step3HandleTransactionPrefixMisMatch(strTransactionIDPrefix, True)
        Else
            Step3HandleTransactionPrefixMisMatch(strTransactionIDPrefix, False)
            strSchemeSetupModelSetupType = SchemeSetupModel.SetupType_ClaimCompletionMessageOutdateTxNo
        End If

        ' Display setting for COVID-19
        If udtEHSTransaction.TransactionDetails.FilterBySubsidizeItemDetail(SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19).Count > 0 Then
            blnCOVID19 = True
            panStep3RecipinetContactInfo.Visible = True
            trStep3TransactionStatus.Style.Add("display", "none")
            trStep3BankAcct.Style.Add("display", "none")
            trStep3ServiceType.Style.Add("display", "none")
            btnStep3ClaimForSamePatient.Visible = False
            btnStep3Reprint.Visible = True

            'Contact No. & Mobile
            If udtEHSTransaction.TransactionAdditionFields.ContactNo IsNot Nothing AndAlso udtEHSTransaction.TransactionAdditionFields.ContactNo <> String.Empty Then
                trStep3ContactNo.Visible = True

                'If udtEHSTransaction.TransactionAdditionFields.ContactNo <> String.Empty Then
                lblStep3ContactNo.Text = udtEHSTransaction.TransactionAdditionFields.ContactNo

                If udtEHSTransaction.TransactionAdditionFields.Mobile IsNot Nothing And udtEHSTransaction.TransactionAdditionFields.Mobile = YesNo.Yes Then
                    lblStep3ContactNo.Text = lblStep2bContactNo.Text & " (" & GetGlobalResourceObject("Text", "Mobile") & ")"
                End If
                'Else
                '    lblStep3ContactNo.Text = GetGlobalResourceObject("Text", "NotProvided")
                'End If
            Else
                trStep3ContactNo.Visible = False
            End If

            'Remarks
            If udtEHSTransaction.TransactionAdditionFields.Remarks IsNot Nothing And udtEHSTransaction.TransactionAdditionFields.Remarks <> String.Empty Then
                lblStep3Remark.Text = udtEHSTransaction.TransactionAdditionFields.Remarks
            Else
                lblStep3Remark.Text = GetGlobalResourceObject("Text", "NotProvided")
            End If

            'Join EHRSS
            If (udtEHSTransaction.SchemeCode.Trim.ToUpper() = SchemeClaimModel.COVID19CVC OrElse _
                udtEHSTransaction.SchemeCode.Trim.ToUpper() = SchemeClaimModel.COVID19DH OrElse _
                udtEHSTransaction.SchemeCode.Trim.ToUpper() = SchemeClaimModel.COVID19RVP OrElse _
                udtEHSTransaction.SchemeCode.Trim.ToUpper() = SchemeClaimModel.COVID19OR OrElse _
                udtEHSTransaction.SchemeCode.Trim.ToUpper() = SchemeClaimModel.COVID19SR OrElse _
                udtEHSTransaction.SchemeCode.Trim.ToUpper() = SchemeClaimModel.VSS OrElse _
                udtEHSTransaction.SchemeCode.Trim.ToUpper() = SchemeClaimModel.RVP) AndAlso _
               (udtEHSAccount.SearchDocCode = DocTypeModel.DocTypeCode.HKIC OrElse _
                udtEHSAccount.SearchDocCode = DocTypeModel.DocTypeCode.EC OrElse _
                udtEHSAccount.SearchDocCode = DocTypeModel.DocTypeCode.OW) Then

                panStep3JoinEHRSS.Visible = True
                If udtEHSTransaction.TransactionAdditionFields.JoinEHRSS IsNot Nothing And udtEHSTransaction.TransactionAdditionFields.JoinEHRSS <> String.Empty Then
                    lblStep3JoinEHRSS.Text = IIf(udtEHSTransaction.TransactionAdditionFields.JoinEHRSS = YesNo.Yes, _
                                               GetGlobalResourceObject("Text", "Yes"), _
                                               GetGlobalResourceObject("Text", "No"))

                Else
                    lblStep3JoinEHRSS.Text = GetGlobalResourceObject("Text", "NA")
                End If
            Else
                panStep3JoinEHRSS.Visible = False
            End If


        Else
            panStep3RecipinetContactInfo.Visible = False
            trStep3TransactionStatus.Style.Remove("display")
            trStep3BankAcct.Style.Remove("display")
            trStep3ServiceType.Style.Remove("display")
            btnStep3ClaimForSamePatient.Visible = True
            btnStep3Reprint.Visible = False
        End If

        ' CRE20-0023 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Dim udtSchemeSetupModel As SchemeSetupModel = Nothing

        If ClaimMode = Common.Component.ClaimMode.COVID19 Then
            Select Case udtSchemeClaim.SchemeCode
                Case SchemeClaimModel.VSS, SchemeClaimModel.RVP
                    udtSchemeSetupModel = SchemeSetupBLL.GetSchemeSetupByKey(SchemeClaimModel.COVID19CVC, udtEHSTransaction.RecordStatus.ToCharArray()(0), strSchemeSetupModelSetupType)
                Case Else
                    udtSchemeSetupModel = SchemeSetupBLL.GetSchemeSetupByKey(udtEHSTransaction.SchemeCode, udtEHSTransaction.RecordStatus.ToCharArray()(0), strSchemeSetupModelSetupType)
            End Select
        Else
            udtSchemeSetupModel = SchemeSetupBLL.GetSchemeSetupByKey(udtEHSTransaction.SchemeCode, udtEHSTransaction.RecordStatus.ToCharArray()(0), strSchemeSetupModelSetupType)
        End If

        Me._udtSystemMessage = CType(SchemeSetupBLL.InterpretSetupValueByType(udtSchemeSetupModel), SystemMessage)

        Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Complete
        Me.udcMsgBoxInfo.AddMessage(Me._udtSystemMessage)
        Me.udcMsgBoxInfo.BuildMessageBox()
        ' CRE20-0023 (Immu record) [End][Chris YIM]

        'Normal Fields 

        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        If _udtSessionHandler.NonClinicSettingGetFromSession(FunctCode) Then
            panNonClinicSettingStep3.Visible = True
            If _udtSessionHandler.Language = CultureLanguage.TradChinese Then
                lblNonClinicSettingStep3.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)))
            ElseIf _udtSessionHandler.Language = CultureLanguage.SimpChinese Then
                lblNonClinicSettingStep3.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese)))
            Else
                lblNonClinicSettingStep3.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.English)))
            End If
        Else
            panNonClinicSettingStep3.Visible = False
        End If
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------
        Dim strTransactionStatus As String = String.Empty
        Dim strTransactionStatusChi As String = String.Empty
        Dim strTransactionStatusCN As String = String.Empty
        Status.GetDescriptionFromDBCode(ClaimTransStatus.ClassCode, udtEHSTransaction.RecordStatus, strTransactionStatus, strTransactionStatusChi, strTransactionStatusCN)

        If Me._udtSessionHandler.Language() = Common.Component.CultureLanguage.TradChinese Then
            Me.lblStep3Scheme.Text = udtSchemeClaim.SchemeDescChi
            Me.lblStep3ServiceType.Text = udtEHSTransaction.ServiceTypeDesc_Chi
            Me.lblStep3Practice.Text = String.Format("{0} ({1})", udtSelectedPracticeDisplay.PracticeNameChi, udtSelectedPracticeDisplay.PracticeID)
            Me.lblStep3Practice.CssClass = "tableTextChi"
            Me.lblTransactionStatus.Text = strTransactionStatusChi
        ElseIf Me._udtSessionHandler.Language() = Common.Component.CultureLanguage.SimpChinese Then
            Me.lblStep3Scheme.Text = udtSchemeClaim.SchemeDescCN
            Me.lblStep3ServiceType.Text = udtEHSTransaction.ServiceTypeDesc_CN
            Me.lblStep3Practice.Text = String.Format("{0} ({1})", udtSelectedPracticeDisplay.PracticeNameChi, udtSelectedPracticeDisplay.PracticeID)
            Me.lblStep3Practice.CssClass = "tableTextChi"
            Me.lblTransactionStatus.Text = strTransactionStatusCN
        Else
            Me.lblStep3Scheme.Text = udtSchemeClaim.SchemeDesc
            Me.lblStep3ServiceType.Text = udtEHSTransaction.ServiceTypeDesc
            Me.lblStep3Practice.Text = String.Format("{0} ({1})", udtSelectedPracticeDisplay.PracticeName, udtSelectedPracticeDisplay.PracticeID)
            Me.lblStep3Practice.CssClass = "tableText"
            Me.lblTransactionStatus.Text = strTransactionStatus
        End If
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
        ' -----------------------------------------------------------------------------------------
        If Me._udtSessionHandler.Language() = Common.Component.CultureLanguage.TradChinese OrElse Me._udtSessionHandler.Language() = CultureLanguage.SimpChinese Then
            If udtEHSTransaction.RecordStatus.ToUpper.Equals(ClaimTransStatus.Incomplete.ToUpper) Then
                Me.lblTransactionStatus.CssClass = "tableTextAlertChi"
            Else
                Me.lblTransactionStatus.CssClass = "tableTextChi"
            End If
        Else
            If udtEHSTransaction.RecordStatus.ToUpper.Equals(ClaimTransStatus.Incomplete.ToUpper) Then
                Me.lblTransactionStatus.CssClass = "tableTextAlert"
            Else
                Me.lblTransactionStatus.CssClass = "tableText"
            End If
        End If
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]


        lblStep3TransNum.Text = udtFormatter.formatSystemNumber(udtEHSTransaction.TransactionID)

        Me.lblStep3TransDate.Text = udtFormatter.formatDateTime(udtEHSTransaction.TransactionDtm)
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtSubPlatformBLL As New SubPlatformBLL
        'Me.lblStep3ServiceDate.Text = udtFormatter.formatDate(udtEHSTransaction.ServiceDate)
        Me.lblStep3ServiceDate.Text = udtFormatter.formatDisplayDate(udtEHSTransaction.ServiceDate, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        Me.lblStep3BankAcct.Text = udtFormatter.maskBankAccount(udtEHSTransaction.BankAccountNo)

        'Setup Personal information 
        Me.udcStep3ReadOnlyDocumnetType.DocumentType = udtEHSAccount.SearchDocCode
        Me.udcStep3ReadOnlyDocumnetType.EHSAccount = udtEHSAccount
        Me.udcStep3ReadOnlyDocumnetType.Vertical = False
        Me.udcStep3ReadOnlyDocumnetType.MaskIdentityNo = True
        Me.udcStep3ReadOnlyDocumnetType.ShowAccountRefNo = False
        Me.udcStep3ReadOnlyDocumnetType.ShowTempAccountNotice = False
        Me.udcStep3ReadOnlyDocumnetType.ShowAccountCreationDate = False
        Me.udcStep3ReadOnlyDocumnetType.TableTitleWidth = 205
        Me.udcStep3ReadOnlyDocumnetType.SetEnableToShowHKICSymbol = True
        Me.udcStep3ReadOnlyDocumnetType.Built()

        'setup Transaction detail
        Me.udcStep3ReadOnlyEHSClaim.EHSClaimVaccine = Me._udtSessionHandler.EHSClaimVaccineGetFromSession()
        Me.udcStep3ReadOnlyEHSClaim.EHSTransaction = udtEHSTransaction
        Me.udcStep3ReadOnlyEHSClaim.SchemeCode = udtSchemeClaim.SchemeCode
        Me.udcStep3ReadOnlyEHSClaim.Mode = ucReadOnlyEHSClaim.ReadOnlyEHSClaimMode.Complete
        Me.udcStep3ReadOnlyEHSClaim.TableTitleWidth = 205
        Me.udcStep3ReadOnlyEHSClaim.ClaimMode = IIf(blnCOVID19, Common.Component.ClaimMode.COVID19, Common.Component.ClaimMode.All)
        Me.udcStep3ReadOnlyEHSClaim.Built()

        Me.Step2aClear()
        Me.Step2bClear()

        If activeViewChange Then
            EHSClaimBasePage.AuditLogCompleteClaim(New AuditLogEntry(FunctionCode, Me), udtEHSTransaction)
        End If

    End Sub

    'CRE15-003 System-generated Form [Start][Philip Chau]
    Private Sub HandleButtonClicked(ByVal blnClicked As Boolean)
        If blnClicked Then
            lblStep3TransactionIDUpdateNoticeBeforeViewLatest.Visible = False
            lblStep3TransactionIDUpdateNoticeAfterViewLatest.Visible = True
            lblLatestTransactionID.Visible = True
            ibtnStep3ViewLatestTransactionID.Visible = False
            lblHTMLRightPointArrow.Visible = True
        Else
            lblStep3TransactionIDUpdateNoticeBeforeViewLatest.Visible = True
            lblStep3TransactionIDUpdateNoticeAfterViewLatest.Visible = False
            lblLatestTransactionID.Visible = False
            ibtnStep3ViewLatestTransactionID.Visible = True
            lblHTMLRightPointArrow.Visible = False
        End If
    End Sub


    Private Sub btnStep3ViewLatestTransactionID_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnStep3ViewLatestTransactionID.Click
        HandleButtonClicked(True)
        _udtSessionHandler.EHSClaimStep3ShowLastestTransactionIDSaveToSession(True)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("View latest transaction no.", lblStep3TransNum.Text)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00084, "Show Transaction ID")
    End Sub

    Private Sub Step3HandleTransactionPrefixMisMatch(ByVal strTmpTranactionIDPrefix As String, ByVal blnViewedLatestTransactioNo As Boolean)
        Dim udtFormatter As Formatter = New Formatter()
        Dim udtEHSTransaction As EHSTransactionModel = Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)

        lblLatestTransactionID.Text = udtFormatter.formatSystemNumber(udtEHSTransaction.TransactionID)
        lblStep3PrefixTransNum.Text = strTmpTranactionIDPrefix
        If blnViewedLatestTransactioNo Then
            lblStep3TransNum.Visible = True
            lblStep3PrefixTransNum.Visible = False

            lblStep3TransactionIDUpdateNoticeBeforeViewLatest.Visible = False
            lblStep3TransactionIDUpdateNoticeAfterViewLatest.Visible = False
            lblLatestTransactionID.Visible = False
            ibtnStep3ViewLatestTransactionID.Visible = False
            lblHTMLRightPointArrow.Visible = False
        Else
            lblStep3TransNum.Visible = False
            lblStep3PrefixTransNum.Visible = True
            lblStep3TransactionIDUpdateNoticeBeforeViewLatest.Visible = True
            lblHTMLRightPointArrow.Visible = False

            If _udtSessionHandler.EHSClaimStep3ShowLastestTransactionIDGetFromSession() Then
                HandleButtonClicked(True)
            Else
                HandleButtonClicked(False)
            End If
        End If
    End Sub
    'CRE15-003 System-generated Form [End][Philip Chau]

    Private Sub Step3Clear()
        Me.udcStep3ReadOnlyDocumnetType.Clear()
        Me.udcStep3ReadOnlyEHSClaim.Clear()
    End Sub



    'CRE20-0022 (Immu record) [Start][Raiman]
    Protected Sub Step3ClaimSubmitPrintConsentForm()

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        Dim udtEHSTransaction As EHSTransactionModel = Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)

        'HardCoded for print Covid19 Vaccination '
        Dim strCurrentPrintOption As String = Common.Component.PrintFormOptionValue.PrintPurposeAndConsent

        Dim strPrintOptionSelectedValue As String = PrintOptionLanguage.English

        Dim strPrintDateTime As String = String.Format("DH_HCV103{0}{1}{2}{3}{4}{5}{6}", Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, Now.Second, Now.Millisecond)
        udtEHSTransaction.PrintedConsentForm = True

        'Set the transaction is printed consent Form
        Me._udtSessionHandler.EHSTransactionSaveToSession(udtEHSTransaction, FunctCode)

        ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
        ' --------------------------------------------------------------------------------------
        'Save the current function code to session (will be removed in the printout form)
        Me._udtSessionHandler.EHSClaimPrintoutFunctionCodeSaveToSession(FunctCode)
        ' CRE20-0022 (Immu record) [End][Winnie SUEN]

        _udtSessionHandler.EHSClaimTempTransactionIDSaveToSession(Me._udtGeneralFunction.generateTemporaryTransactionNumber(udtSchemeClaim.SchemeCode.Trim()))

        'Log
        If ClaimMode = Common.Component.ClaimMode.COVID19 AndAlso udtSchemeClaim.SchemeCode = SchemeClaimModel.COVID19CVC Then
            EHSClaimBasePage.AddDescriptionCentreBooth(udtAuditLogEntry, udtEHSTransaction.ServiceProviderID, udtEHSTransaction.PracticeID)
        End If

        Select Case strPrintOptionSelectedValue
            Case PrintOptionLanguage.TradChinese 'PrintOptionValue.Chi
                If strCurrentPrintOption = Common.Component.PrintFormOptionValue.PrintPurposeAndConsent Then
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/EHSClaimForm_CHI_RV.aspx?TID=" + strPrintDateTime + "');", True)
                    EHSClaimBasePage.AuditLogPrintFrom(udtAuditLogEntry, False, "EHSClaimForm_CHI_RV", strPrintOptionSelectedValue, _udtSessionHandler.EHSClaimTempTransactionIDGetFromSession())

                ElseIf strCurrentPrintOption = Common.Component.PrintFormOptionValue.PrintConsentOnly Then
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/EHSClaimCondensedForm_CHI_RV.aspx?TID=" + strPrintDateTime + "');", True)
                    EHSClaimBasePage.AuditLogPrintFrom(udtAuditLogEntry, False, "EHSClaimCondensedForm_CHI_RV", strPrintOptionSelectedValue, _udtSessionHandler.EHSClaimTempTransactionIDGetFromSession())

                End If
            Case PrintOptionLanguage.English 'PrintOptionValue.Eng
                If strCurrentPrintOption = Common.Component.PrintFormOptionValue.PrintPurposeAndConsent Then
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/EHSClaimForm_RV.aspx?TID=" + strPrintDateTime + "');", True)
                    EHSClaimBasePage.AuditLogPrintFrom(udtAuditLogEntry, False, "EHSClaimForm_RV", strPrintOptionSelectedValue, _udtSessionHandler.EHSClaimTempTransactionIDGetFromSession())

                ElseIf strCurrentPrintOption = Common.Component.PrintFormOptionValue.PrintConsentOnly Then
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/EHSClaimCondensedForm_RV.aspx?TID=" + strPrintDateTime + "');", True)
                    EHSClaimBasePage.AuditLogPrintFrom(udtAuditLogEntry, False, "EHSClaimCondensedForm_RV", strPrintOptionSelectedValue, _udtSessionHandler.EHSClaimTempTransactionIDGetFromSession())

                End If

                'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            Case PrintOptionLanguage.SimpChinese
                If strCurrentPrintOption = Common.Component.PrintFormOptionValue.PrintPurposeAndConsent Then
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/EHSClaimForm_CN_RV.aspx?TID=" + strPrintDateTime + "');", True)
                    EHSClaimBasePage.AuditLogPrintFrom(udtAuditLogEntry, False, "EHSClaimForm_CN_RV", strPrintOptionSelectedValue, _udtSessionHandler.EHSClaimTempTransactionIDGetFromSession())

                ElseIf strCurrentPrintOption = Common.Component.PrintFormOptionValue.PrintConsentOnly Then
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/EHSClaimCondensedForm_CN_RV.aspx?TID=" + strPrintDateTime + "');", True)
                    EHSClaimBasePage.AuditLogPrintFrom(udtAuditLogEntry, False, "EHSClaimCondensedForm_CN_RV", strPrintOptionSelectedValue, _udtSessionHandler.EHSClaimTempTransactionIDGetFromSession())

                End If
                'CRE13-019-02 Extend HCVS to China [End][Winnie]
        End Select

    End Sub
    'CRE20-0022 (Immu record) [End][Raiman]




#End Region

#Region "Step of Select Practice"

    'Select Practice in MuiltView
    Protected Sub PracticeRadioButtonGroup_PracticeSelected(ByVal strPracticeName As String, ByVal strBankAcctNo As String, ByVal intBankAccountDisplaySeq As Integer, ByVal strSchemeCode As String, ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles PracticeRadioButtonGroup.PracticeSelected
        If Me._blnIsRequireHandlePageRefresh Then
            Return
        End If

        Dim formatter As Formatter = New Formatter()
        Dim udtDataEntryModel As DataEntryUserModel = Nothing
        Dim strSelectedPractice As String = String.Format("{0} ({1})", strPracticeName, intBankAccountDisplaySeq)
        Dim udtPracticeDisplays As BLL.PracticeDisplayModelCollection = Nothing
        Dim udtPracticeDisplay As BLL.PracticeDisplayModel

        Me.GetCurrentUserAccount(Me._udtSP, udtDataEntryModel, False)

        udtPracticeDisplays = Me._udtSessionHandler.PracticeDisplayListGetFromSession(FunctCode)

        udtPracticeDisplay = udtPracticeDisplays.Filter(intBankAccountDisplaySeq)

        Me._udtSessionHandler.PracticeDisplaySaveToSession(udtPracticeDisplay, FunctCode)

        ' Save the new selected Scheme to session
        _udtSessionHandler.SchemeSelectedForPracticeSaveToSession(strSchemeCode, FunctCode)

        'Log Practice Selection
        EHSClaimBasePage.AuditLogPracticeSelected(New AuditLogEntry(FunctionCode, Me), False, udtPracticeDisplay, Nothing, False)

        Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
    End Sub

    'Setup Practice Selection
    Private Sub SetupSelectPractice()
        Dim udtPracticeDisplays As BLL.PracticeDisplayModelCollection = Me._udtSessionHandler.PracticeDisplayListGetFromSession(FunctCode)
        Dim udtDataEntryUser As DataEntryUserModel = Nothing

        Me._udtSessionHandler.CurrentUserGetFromSession(Me._udtSP, udtDataEntryUser)

        'Build practice Selection List
        Me.PracticeRadioButtonGroup.VerticalScrollBar = False
        ' CRE20-0XX (HA Scheme) [Start][Winnie]
        Me.PracticeRadioButtonGroup.SchemeSelection = IIf(Me.SubPlatform = EnumHCSPSubPlatform.CN, True, False)
        Me.PracticeRadioButtonGroup.SelectedScheme = Me._udtSessionHandler.SchemeSelectedForPracticeGetFromSession(FunctCode)
        ' CRE20-0XX (HA Scheme) [End][Winnie]

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Me.PracticeRadioButtonGroup.BuildRadioButtonGroup(udtPracticeDisplays, _
                                                          Me._udtSP.PracticeList, _
                                                          Me._udtSP.SchemeInfoList, _
                                                          Me._udtSessionHandler.Language, _
                                                          PracticeRadioButtonGroup.DisplayMode.BankAccount, _
                                                          Me.ClaimMode)
        ' CRE20-0022 (Immu record) [End][Chris YIM]
    End Sub

#End Region

#Region "Step of Internal Error "

    Private Sub SetupInternalError()
        If Me._udtSessionHandler.EHSClaimStepsGetFromSession(FunctCode) <> ActiveViewIndex.EHSClaimError Then
            Me._blnIsRequireHandlePageRefresh = True
            Return
        End If

        Dim udtEHSaccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Dim udtPracticeDisplay As BLL.PracticeDisplayModel = Me._udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)

        ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtPracticeDisplayList As PracticeDisplayModelCollection = Me._udtSessionHandler.PracticeDisplayListGetFromSession(FunctCode)
        ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

        Dim udtEHSTransaction As EHSTransactionModel = Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)
        Me.btnInternalErrorBack.Visible = True
        Me.btnInternalErrorBack.AlternateText = Me.GetGlobalResourceObject("AlternateText", "BackBtn")
        Me.btnInternalErrorBack.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "BackBtn")

        If Not Me.EHSClaimTokenNumValidation(False) Then

            Me._udtSystemMessage = New Common.ComObject.SystemMessage("990000", "I", "00023")
            Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Information
            Me.udcMsgBoxInfo.AddMessage(Me._udtSystemMessage)

            'Force user reload EHSClaim page
            Me.btnInternalErrorBack.Visible = False

            Throw New Exception("Concurrent Browser Detected in EHSClaimV1")

        ElseIf udtPracticeDisplayList IsNot Nothing AndAlso udtPracticeDisplayList.Count > 0 AndAlso Not udtPracticeDisplayList.HasPracticeAvailableForClaim Then

            ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]
            ' -----------------------------------------------------------------------------------------
            ' Practice is active but not available for claim
            'No Active Practice for the current SP
            Me._udtSystemMessage = New Common.ComObject.SystemMessage("020202", "I", "00006") ' No available scheme. No claim action can be taken.
            Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Information
            Me.udcMsgBoxInfo.AddMessage(Me._udtSystemMessage)

            'Force user reload EHSClaim page
            Me.btnInternalErrorBack.Visible = False
            ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]
        ElseIf udtPracticeDisplay Is Nothing OrElse udtPracticeDisplayList.Count = 0 Then
            'No Active Practice for the current SP
            Me._udtSystemMessage = New Common.ComObject.SystemMessage("020202", "I", "00003")
            Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Information
            Me.udcMsgBoxInfo.AddMessage(Me._udtSystemMessage)

            'Force user reload EHSClaim page
            Me.btnInternalErrorBack.Visible = False

        ElseIf Me._blnConcurrentUpdate = True OrElse udtEHSaccount Is Nothing Then

            ' Case of  Me._blnConcurrentUpdate = true
            Me._udtSessionHandler.EHSAccountRemoveFromSession(FunctCode)

            If udtEHSTransaction.TransactionDetails.FilterBySubsidizeItemDetail(SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19).Count > 0 Then
                Me._udtSystemMessage = New Common.ComObject.SystemMessage("990000", "I", "00017")
            Else
                Me._udtSystemMessage = New Common.ComObject.SystemMessage("020202", "I", "00005")
            End If

            Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Information
            Me.udcMsgBoxInfo.AddMessage(Me._udtSystemMessage)


        ElseIf (Not udtEHSTransaction Is Nothing) AndAlso _
            ( _
                ( _
                    Not udtEHSTransaction.TransactionID Is Nothing AndAlso _
                    Not udtEHSTransaction.TransactionID.Trim().Equals(String.Empty)) _
                OrElse _
                    (Not udtEHSTransaction.IsNew) _
            ) Then
            'No Active Practice for the current SP
            Me._udtSystemMessage = New Common.ComObject.SystemMessage("990000", "I", "00022")
            Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Information
            Me.udcMsgBoxInfo.AddMessage(Me._udtSystemMessage)

        End If

        Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Information
        Me.udcMsgBoxInfo.BuildMessageBox()

    End Sub

    Private Sub btnInternalErrorBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnInternalErrorBack.Click

        Me._udtSessionHandler.EHSClaimSessionRemove(FunctCode)
        Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
    End Sub

#End Region

#Region "Vaccination Record"

    Protected Sub ibtnVaccinationRecord_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        EHSClaimBasePage.AuditLogVaccinationRecordClick(New AuditLogEntry(FunctionCode, Me))

        ShowVaccinationRecord(True)
        txtStep2aServiceDate_TextChanged(Nothing, Nothing)
    End Sub

    Private Sub CheckShowVaccinationRecord()
        If CheckFromVaccinationRecordEnquiry() Then
            ViewState(VS.VaccinationRecordPopupStatus) = PopupStatusClass.Closed
            _udtSessionHandler.FromVaccinationRecordEnquiryRemoveFromSession()
        End If

        Select Case ViewState(VS.VaccinationRecordPopupStatus)
            Case Nothing
                EHSClaimBasePage.AuditLogForceShowVaccinationRecord(New AuditLogEntry(FunctionCode, Me))

                ShowVaccinationRecord(False)

            Case PopupStatusClass.Active
                Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
                If IsNothing(udtEHSAccount) Then Return

                ModalPopupExtenderVaccinationRecord.Show()
                udcVaccinationRecord.BuildEHSAccount(udtEHSAccount)

        End Select

        If ViewState(VS.VaccinationRecordProviderPopupStatus) = PopupStatusClass.Active Then
            ucVaccinationRecordProvider.Build()
            ModalPopupExtenderVaccinationRecordProvider.Show()
        End If

    End Sub

    ''' <summary>
    ''' Build and display vaccination record (EHS, HA CMS)
    ''' </summary>
    ''' <param name="blnForceRefresh">If true, HA CMS vaccination record will be re-enquiry from CMS again, otherwise cached vaccination record will be used, if no cached then re-enquirty again</param>
    ''' <remarks></remarks>
    Private Sub ShowVaccinationRecord(ByVal blnForceRefresh As Boolean)
        ModalPopupExtenderVaccinationRecord.Show()
        ViewState(VS.VaccinationRecordPopupStatus) = PopupStatusClass.Active

        Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        If blnForceRefresh Then
            udcVaccinationRecord.Build(udtEHSAccount, _
                                       New AuditLogEntry(FunctCode, Me), _
                                       IIf(Me.ClaimMode() = ClaimMode.COVID19, ucVaccinationRecord.RecordType.COVID19, ucVaccinationRecord.RecordType.WithoutCOVID19))
        Else
            udcVaccinationRecord.Build(udtEHSAccount, _
                                       Me._udtSessionHandler.CMSVaccineResultGetFromSession(FunctCode), _
                                       Me._udtSessionHandler.CIMSVaccineResultGetFromSession(FunctCode), _
                                       New AuditLogEntry(FunctCode, Me), _
                                       IIf(Me.ClaimMode() = ClaimMode.COVID19, ucVaccinationRecord.RecordType.COVID19, ucVaccinationRecord.RecordType.WithoutCOVID19))
        End If
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        Me._udtSessionHandler.CMSVaccineResultSaveToSession(udcVaccinationRecord.HAVaccineResult, FunctCode)
        Me._udtSessionHandler.CIMSVaccineResultSaveToSession(udcVaccinationRecord.DHVaccineResult, FunctCode)
    End Sub

    '

    Protected Sub ibtnInfo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ViewState(VS.VaccinationRecordProviderPopupStatus) = PopupStatusClass.Active
        ucVaccinationRecordProvider.Build()
        ModalPopupExtenderVaccinationRecordProvider.Show()
    End Sub

    Protected Sub ibtnVaccinationRecordProviderClose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ViewState.Remove(VS.VaccinationRecordProviderPopupStatus)
        ModalPopupExtenderVaccinationRecordProvider.Hide()
    End Sub

    Protected Sub btnVaccinationRecordClose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        EHSClaimBasePage.AuditLogVaccinationRecordCloseClick(New AuditLogEntry(FunctionCode, Me))

        ViewState(VS.VaccinationRecordPopupStatus) = PopupStatusClass.Closed
        ModalPopupExtenderVaccinationRecord.Hide()
        Me._ScriptManager.SetFocus(Me.btnStep2aClaim)

    End Sub

    '

    Private Function CheckFromVaccinationRecordEnquiry() As Boolean
        Return _udtSessionHandler.FromVaccinationRecordEnquiryGetFromSession
    End Function



    ' CRE20-0022 (Immu record) [Start][Martin]
    Private Sub BuildCOVID19VaccinationRecordGrid(ByRef dtVaccineRecord As DataTable)

        Me._udtSessionHandler.ClaimCOVID19VaccineRecordSaveToSession(dtVaccineRecord)
        If dtVaccineRecord.Rows.Count > 0 Then
            gvCVaccinationRecord.DataSource = dtVaccineRecord
            gvCVaccinationRecord.DataBind()
            panNoVaccinationRecord.Visible = False
            gvCVaccinationRecord.Visible = True
        Else
            panNoVaccinationRecord.Visible = True
            gvCVaccinationRecord.Visible = False
            gvCVaccinationRecord.Dispose()
        End If

    End Sub

    Protected Sub gvCVaccinationRecord_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Dim dr As DataRow = CType(e.Row.DataItem, System.Data.DataRowView).Row
            Dim udtFormatter As New Formatter
            Dim blnIsChinese As Boolean = (LCase(Session("language")) = CultureLanguage.TradChinese)
            Dim strLanguage As String = Session("language").ToString.Trim.ToLower

            ' Injection Date
            Dim lblCInjectionDate As Label = e.Row.FindControl("lblCInjectionDate")
            Select Case strLanguage
                Case CultureLanguage.TradChinese
                    lblCInjectionDate.Text = udtFormatter.formatDisplayDate(CDate(lblCInjectionDate.Text.Trim), CultureLanguage.TradChinese)
                Case CultureLanguage.SimpChinese
                    lblCInjectionDate.Text = udtFormatter.formatDisplayDate(CDate(lblCInjectionDate.Text.Trim), CultureLanguage.SimpChinese)
                Case CultureLanguage.English
                    lblCInjectionDate.Text = udtFormatter.formatDisplayDate(CDate(lblCInjectionDate.Text.Trim), CultureLanguage.English)
                Case Else
                    lblCInjectionDate.Text = udtFormatter.formatDisplayDate(CDate(lblCInjectionDate.Text.Trim), CultureLanguage.English)
            End Select

            ' Vaccination
            Dim lblGVaccination As Label = e.Row.FindControl("lblCVaccination")
            Dim lblGVaccinationChi As Label = e.Row.FindControl("lblCVaccinationChi")

            lblGVaccination.Visible = Not blnIsChinese
            lblGVaccinationChi.Visible = blnIsChinese

            ' Dose
            Dim lblGDose As Label = e.Row.FindControl("lblCDose")
            Dim lblGDoseChi As Label = e.Row.FindControl("lblCDoseChi")

            lblGDose.Visible = False
            lblGDoseChi.Visible = False

            Select Case strLanguage
                Case CultureLanguage.TradChinese
                    lblGDoseChi.Visible = True
                    lblGDoseChi.Text = dr("AvailableItemDescChi")
                Case CultureLanguage.SimpChinese
                    lblGDoseChi.Visible = True
                    lblGDoseChi.Text = dr("AvailableItemDescCN")
                Case CultureLanguage.English
                    lblGDose.Visible = True
                    lblGDose.Text = dr("AvailableItemDesc")
                Case Else
                    lblGDose.Visible = True
                    lblGDose.Text = dr("AvailableItemDesc")
            End Select

            ' Information Provider
            Dim lblGProvider As Label = e.Row.FindControl("lblCProvider")

            If lblGProvider.Text.ToUpper.Trim = TransactionDetailVaccineModel.ProviderClass.Private OrElse _
               lblGProvider.Text.ToUpper.Trim = TransactionDetailVaccineModel.ProviderClass.RVP Then
                'Enrolled Doctors (eHS(S))
                lblGProvider.Text = GetGlobalResourceObject("Text", "eHSSubsidize")
            Else
                'Hospital Authority, Department of Health, Residential Care Home (eHS(S))
                Select Case strLanguage
                    Case CultureLanguage.TradChinese
                        Status.GetDescriptionFromDBCode(TransactionDetailVaccineModel.ProviderClass.ClassCode, lblGProvider.Text.Trim, String.Empty, lblGProvider.Text, String.Empty)
                    Case CultureLanguage.SimpChinese
                        Status.GetDescriptionFromDBCode(TransactionDetailVaccineModel.ProviderClass.ClassCode, lblGProvider.Text.Trim, String.Empty, String.Empty, lblGProvider.Text)
                    Case CultureLanguage.English
                        Status.GetDescriptionFromDBCode(TransactionDetailVaccineModel.ProviderClass.ClassCode, lblGProvider.Text.Trim, lblGProvider.Text, String.Empty, String.Empty)
                    Case Else
                        Status.GetDescriptionFromDBCode(TransactionDetailVaccineModel.ProviderClass.ClassCode, lblGProvider.Text.Trim, lblGProvider.Text, String.Empty, String.Empty)
                End Select
            End If

            ' Remarks
            Dim lblGRemark As Label = e.Row.FindControl("lblCRemark")
            Dim strDaysApartText As String
            strDaysApartText = HttpContext.GetGlobalResourceObject("AlternateText", "DaysApart")
            lblGRemark.Text = strDaysApartText.Replace("%s", Convert.ToInt32(lblGRemark.Text))

            ' CRE20-0022 (Immu record) [End][Chris YIM]
        End If
    End Sub

    Protected Sub gvCVaccinationRecord_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.GridViewPreRenderHandler(sender, e, SessionHandler.SessionName.SESS_ClaimCOVID19_VaccineRecord)
    End Sub

    Protected Sub gvCVaccinationRecord_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        Me.GridViewSortingHandler(sender, e, SessionHandler.SessionName.SESS_ClaimCOVID19_VaccineRecord)
    End Sub

    Private Function TransactionDetailListToCOVID19DataTable(ByVal udtTranDetailVaccineList As TransactionDetailVaccineModelCollection) As DataTable
        Dim dtVaccineRecord As New DataTable

        ' Columns
        With dtVaccineRecord.Columns
            .Add("ServiceReceiveDtm", GetType(Date))
            .Add("SubsidizeDesc", GetType(String))
            .Add("SubsidizeDescChi", GetType(String))
            .Add("AvailableItemDesc", GetType(String))
            .Add("AvailableItemDescChi", GetType(String))
            .Add("AvailableItemDescCN", GetType(String))
            .Add("Provider", GetType(String))
            .Add("Remark", GetType(String))
        End With

        ' Convert each TransactionDetailModel to datarow
        For Each udtTranDetailVaccine As TransactionDetailVaccineModel In udtTranDetailVaccineList
            If udtTranDetailVaccine.SubsidizeItemCode = SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19 Then
                Dim drVaccineRecord As DataRow = dtVaccineRecord.NewRow

                drVaccineRecord("ServiceReceiveDtm") = udtTranDetailVaccine.ServiceReceiveDtm
                drVaccineRecord("SubsidizeDesc") = udtTranDetailVaccine.SubsidizeDesc
                drVaccineRecord("SubsidizeDescChi") = udtTranDetailVaccine.SubsidizeDescChi
                drVaccineRecord("AvailableItemDesc") = udtTranDetailVaccine.AvailableItemDesc
                drVaccineRecord("AvailableItemDescChi") = udtTranDetailVaccine.AvailableItemDescChi
                drVaccineRecord("AvailableItemDescCN") = udtTranDetailVaccine.AvailableItemDescCN

                If udtTranDetailVaccine.SchemeCode = Common.Component.Scheme.SchemeClaimModel.RVP Then
                    drVaccineRecord("Provider") = TransactionDetailVaccineModel.ProviderClass.RVP
                Else
                    drVaccineRecord("Provider") = udtTranDetailVaccine.Provider
                End If

                drVaccineRecord("Remark") = DateDiff(DateInterval.DayOfYear, udtTranDetailVaccine.ServiceReceiveDtm, Now)
                dtVaccineRecord.Rows.Add(drVaccineRecord)
            End If
        Next

        ' Sort the datatable
        Dim dtResult As DataTable = dtVaccineRecord.Clone

        For Each dr As DataRow In dtVaccineRecord.Select(String.Empty, "ServiceReceiveDtm DESC")
            dtResult.ImportRow(dr)
        Next

        Return dtResult

    End Function
    ' CRE20-0022 (Immu record) [End][Martin]


#End Region

#Region "Other functions"

    '-----------------------------------------------------------------------------------------------------------------------------
    'For change Scheme Logo
    '-----------------------------------------------------------------------------------------------------------------------------
    Private Sub DropDownListBindScheme(ByVal dropDownList As DropDownList, ByVal udtSchemeClaimModelCollection As SchemeClaimModelCollection, ByVal language As String, ByVal includeEmptyValue As Boolean)
        Dim listItem As ListItem
        Dim isFillSelectedValue As Boolean = False
        Dim dropDownListSelectedValue As String = dropDownList.SelectedValue

        dropDownList.Items.Clear()
        If includeEmptyValue Then
            dropDownList.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "EHSClaimPleaseSelect"), String.Empty))
        End If

        For Each schemeClaimModel As SchemeClaimModel In udtSchemeClaimModelCollection
            listItem = New ListItem
            If language = Common.Component.CultureLanguage.TradChinese Then
                listItem.Text = schemeClaimModel.SchemeDescChi
            ElseIf language = Common.Component.CultureLanguage.SimpChinese Then
                listItem.Text = schemeClaimModel.SchemeDescCN
            Else
                listItem.Text = schemeClaimModel.SchemeDesc
            End If
            listItem.Value = schemeClaimModel.SchemeCode

            'The selected scheme code may not include in new drop down list
            If schemeClaimModel.SchemeCode.Trim().Equals(dropDownListSelectedValue) Then
                isFillSelectedValue = True
            End If

            dropDownList.Items.Add(listItem)
        Next

        'if the selected scheme code is existing in the new drop down list
        If isFillSelectedValue Then
            If Not dropDownListSelectedValue Is Nothing AndAlso Not dropDownListSelectedValue.Equals(String.Empty) Then
                dropDownList.SelectedValue = dropDownListSelectedValue
            End If
        End If

    End Sub

    '-----------------------------------------------------------------------------------------------------------------------------
    'For change Scheme Logo
    '-----------------------------------------------------------------------------------------------------------------------------
    Private Sub SetupSchemeLogo()
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
        If udtSchemeClaim Is Nothing Then
            Me.imgSchemeLogo.Visible = False
        Else
            Me.imgSchemeLogo.Visible = True
            Me.imgSchemeLogo.ImageUrl = Me.GetGlobalResourceObject("ImageURL", String.Format("SchemeLogo_{0}", udtSchemeClaim.SchemeCode.ToUpper().Trim()))

        End If

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        If Me.ClaimMode = Common.Component.ClaimMode.COVID19 Then
            Me.imgSchemeLogo.Visible = False
        End If
        ' CRE20-0022 (Immu record) [End][Chris YIM]
    End Sub

    Private Sub SetupTimeline()
        Dim strStep3Resource As String = String.Empty

        If Me.IsSkipClaimCompletePage Then
            lblClaimValidatedStep3.Text = String.Empty
        Else
            If Me.ClaimMode = ClaimMode.COVID19 Then
                strStep3Resource = "ClaimStep3Complete"
            Else
                strStep3Resource = "ClaimStep3"
            End If

            lblClaimValidatedStep3.Text = HttpContext.GetGlobalResourceObject("Text", strStep3Resource)

        End If

    End Sub


    ''' <summary>
    ''' Get current user account
    ''' </summary>
    ''' <param name="udtSP"></param>
    ''' <param name="udtDataEntry"></param>
    ''' <param name="blnGetFromDatabase">True: Get from database and save to session; False: Get from session</param>
    ''' <remarks></remarks>
    Private Sub GetCurrentUserAccount(ByRef udtSP As ServiceProviderModel, ByRef udtDataEntry As DataEntryUserModel, ByVal blnGetFromDatabase As Boolean)
        ' Get Current User Account
        _udtUserAC = UserACBLL.GetUserAC

        If blnGetFromDatabase Then
            If _udtUserAC.UserType = SPAcctType.ServiceProvider Then
                udtSP = _udtUserAC

                ' Get the latest SP from database
                udtSP = (New ClaimVoucherBLL).loadSP(udtSP.SPID, Me.SubPlatform)

                ' Set Data Entry to Nothing
                udtDataEntry = Nothing

            ElseIf _udtUserAC.UserType = SPAcctType.DataEntryAcct Then
                udtDataEntry = _udtUserAC

                ' Get the latest Data Entry Account from database
                udtDataEntry = (New DataEntryAcctBLL).LoadDataEntry(udtDataEntry.SPID, udtDataEntry.DataEntryAccount)

                ' Get the latest SP from database 
                udtSP = (New ClaimVoucherBLL).loadSP(udtDataEntry.SPID, Me.SubPlatform)
                udtDataEntry.ServiceProvider = udtSP

            End If

            ' Save to session
            _udtSessionHandler.CurrentUserSaveToSession(udtSP, udtDataEntry)

        Else
            ' Get from session
            _udtSessionHandler.CurrentUserGetFromSession(udtSP, udtDataEntry)

        End If

    End Sub

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

    ' CRE13-001 - EHAPP [Start][Tommy L]
    ' -------------------------------------------------------------------------------------
    'Setup Claim Declaration Availability by visibility setting
    Private Function SetClaimDeclarationAvailability(ByVal udtSchemeClaim As SchemeClaimModel, ByVal udtEHSTransaction As EHSTransactionModel) As Boolean
        Dim blnClaimDeclarationAvailability As Boolean = False
        Dim udtSubsidizeGroupClaimModel As SubsidizeGroupClaimModel

        For Each udtTransactionDetailModel As TransactionDetailModel In udtEHSTransaction.TransactionDetails

            ' CRE13-001 - EHAPP [Start][Koala]
            ' -------------------------------------------------------------------------------------
            ' Get the correct SchemeClaim & subsidize by service date
            Dim udtSchemeClaimTemp As SchemeClaimModel = (New SchemeClaimBLL).getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtSchemeClaim.SchemeCode, udtEHSTransaction.ServiceDate.Date.AddDays(1).AddMinutes(-1))
            ' CRE13-001 - EHAPP [End][Koala]

            udtSubsidizeGroupClaimModel = udtSchemeClaimTemp.SubsidizeGroupClaimList.Filter(udtTransactionDetailModel.SchemeCode, udtTransactionDetailModel.SchemeSeq, udtTransactionDetailModel.SubsidizeCode)

            If udtSubsidizeGroupClaimModel.ClaimDeclarationAvailable Then
                blnClaimDeclarationAvailability = True
                Exit For
            End If
        Next

        If blnClaimDeclarationAvailability Then
            Me.trStep2bDeclareClaim.Visible = True
            Return True
        Else
            Me.chkStep2bDeclareClaim.Checked = True
            Me.trStep2bDeclareClaim.Visible = False
            Me.btnStep2bConfirm.Enabled = True
            Return False
        End If
    End Function
    ' CRE13-001 - EHAPP [End][Tommy L]

    '-----------------------------------------------------------------------------------------------------------------------------
    'For enable or disable Buttons
    '-----------------------------------------------------------------------------------------------------------------------------
    Private Sub SetSaveButtonEnable(ByVal btnSave As ImageButton, ByVal blnEnable As Boolean)
        btnSave.Enabled = blnEnable
        If blnEnable Then
            btnSave.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "SaveBtn")
        Else
            btnSave.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "SaveDisableBtn")
        End If
        btnSave.AlternateText = Me.GetGlobalResourceObject("AlternateText", "SaveBtn")
    End Sub

    Private Sub SetConfirmButtonEnable(ByVal btnConfirm As ImageButton, ByVal blnEnable As Boolean)
        btnConfirm.Enabled = blnEnable
        If blnEnable Then
            btnConfirm.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "ConfirmBtn")
        Else
            btnConfirm.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "ConfirmDisableBtn")
        End If
        btnConfirm.AlternateText = Me.GetGlobalResourceObject("AlternateText", "ConfirmBtn")
    End Sub

    Private Sub SetClaimButtonEnable(ByVal btnClaim As ImageButton, ByVal blnEnable As Boolean)
        btnClaim.Enabled = blnEnable
        If blnEnable Then
            btnClaim.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "ClaimBtn")
        Else
            btnClaim.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "ClaimDisableBtn")
        End If
        btnClaim.AlternateText = Me.GetGlobalResourceObject("AlternateText", "ClaimBtn")
    End Sub

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub Set2aDeclareCheckboxEnable(ByVal blnEnable As Boolean)
        If blnEnable = True Then
            Me.chkStep2aDeclareClaim.Enabled = True
            Me.chkStep2aDeclareClaim.ForeColor = Drawing.Color.Black
        Else
            Me.chkStep2aDeclareClaim.Enabled = False
            Me.chkStep2aDeclareClaim.ForeColor = Drawing.Color.Gray
        End If
    End Sub
    ' CRE20-0022 (Immu record) [End][Chris YIM]

    Private Sub Set2bDeclareCheckboxEnable(ByVal blnEnable As Boolean)
        If blnEnable = True Then
            Me.chkStep2bDeclareClaim.Enabled = True
            Me.chkStep2bDeclareClaim.ForeColor = Drawing.Color.Black
        Else
            Me.chkStep2bDeclareClaim.Enabled = False
            Me.chkStep2bDeclareClaim.ForeColor = Drawing.Color.Gray
        End If
    End Sub

    'CRE13-032 End of Support of XP and IE6 [End][Karl]
    '-----------------------------------------------------------------------------------------------------------------------------
    'For Print Option 
    '-----------------------------------------------------------------------------------------------------------------------------
    Public Sub ChangePrintFormControl(ByVal strPrintOption As String, _
                                      ByVal udtSchemeClaim As SchemeClaimModel, _
                                      ByVal panAdhocPrint As Panel, _
                                      ByVal panlblPrintConsent As Panel, _
                                      ByVal panPerprintFormNotice As Panel, _
                                      ByVal btnPrintAdhocClaimConsentForm As ImageButton, _
                                      ByVal btnPrintClaimConsentForm As ImageButton)

        panAdhocPrint.Visible = False

        'Chnage Print Option controls (for consent and creation form)
        If strPrintOption = Common.Component.PrintFormOptionValue.PreprintForm Then
            panlblPrintConsent.Visible = False
            panPerprintFormNotice.Visible = True

            If udtSchemeClaim.SubsidizeGroupClaimList(0).AdhocPrintAvailable Then
                'Setup Print consent from image button 
                btnPrintAdhocClaimConsentForm.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "VRAPrintClaimConsentFormBtn")
                btnPrintAdhocClaimConsentForm.AlternateText = Me.GetGlobalResourceObject("AlternateText", "VRAPrintClaimConsentFormBtn")

                panAdhocPrint.Visible = True
            End If

        Else
            panlblPrintConsent.Visible = True
            panPerprintFormNotice.Visible = False

            If strPrintOption = Common.Component.PrintFormOptionValue.PrintConsentOnly Then
                btnPrintClaimConsentForm.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "PrintConsentOnly")
                btnPrintClaimConsentForm.AlternateText = Me.GetGlobalResourceObject("AlternateText", "PrintConsentOnly")
            Else
                btnPrintClaimConsentForm.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "PrintStatementAndConsent")
                btnPrintClaimConsentForm.AlternateText = Me.GetGlobalResourceObject("AlternateText", "PrintStatementAndConsent")
            End If

        End If

    End Sub

    'CRE13-019-02 Extend HCVS to China [Start][Winnie]
    Private Sub PrintClaimConsentFormLanguageSetup(ByVal slConsentFormAvailableLang As String(), ByVal rblConsentFormLanguage As RadioButtonList)

        Dim strSelectedLang As String = rblConsentFormLanguage.SelectedValue

        rblConsentFormLanguage.Visible = False
        rblConsentFormLanguage.Items.Clear()

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

                rblConsentFormLanguage.Items.Add(item)
            Next

            'If only 1 language is available, not display
            If rblConsentFormLanguage.Items.Count > 1 Then
                rblConsentFormLanguage.Visible = True
            End If

            'Default the first item
            If rblConsentFormLanguage.SelectedIndex = -1 Then
                rblConsentFormLanguage.SelectedIndex = 0
            End If

        End If
    End Sub
    'CRE13-019-02 Extend HCVS to China [End][Winnie]

    Private Sub DisableConfirmDeclareCheckBox(ByVal strPrintOption As String)
        'init declaration check box control
        Me.chkStep2bDeclareClaim.Checked = False

        Me.SetConfirmButtonEnable(Me.btnStep2bConfirm, False)

        If strPrintOption <> Common.Component.PrintFormOptionValue.PreprintForm Then
            'CRE13-032 End of Support of XP and IE6 [Start][Karl]
            'Me.chkStep2bDeclareClaim.Enabled = False
            Call Set2bDeclareCheckboxEnable(False)
            'CRE13-032 End of Support of XP and IE6 [End][Karl]
        Else
            'CRE13-032 End of Support of XP and IE6 [Start][Karl]
            'Me.chkStep2bDeclareClaim.Enabled = True
            Call Set2bDeclareCheckboxEnable(True)
            'CRE13-032 End of Support of XP and IE6 [End][Karl]
        End If
    End Sub

    Private Sub DisableStep2aDeclareCheckBox(ByVal strPrintOption As String)
        'init declaration check box control
        Me.chkStep2aDeclareClaim.Checked = False

        If strPrintOption <> Common.Component.PrintFormOptionValue.PreprintForm Then
            Call Set2aDeclareCheckboxEnable(False)
        Else
            Call Set2aDeclareCheckboxEnable(True)
        End If
    End Sub

    ''' <summary>
    ''' Clear Claim for new patient
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Clear()
        Me.Step2aClear()
        Me.Step2bClear()
        Me.Step3Clear()
        Me._udtSessionHandler.EHSClaimSessionRemove(FunctCode)

        Me._udtSessionHandler.HighRiskRemoveFromSession(FunctCode)

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Me._udtSessionHandler.ClaimCOVID19CategoryRemoveFromSession(FunctCode)
        Me._udtSessionHandler.ClaimCOVID19DoseRemoveFromSession(FunctionCode)
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        ViewState(VS.VaccinationRecordPopupStatus) = Nothing

    End Sub

    ''' <summary>
    ''' Show Change Practice button
    ''' </summary>
    ''' <param name="imageButton"></param>
    ''' <remarks></remarks>
    Private Sub ShowChangePracticeButton(ByVal imageButton As ImageButton)
        Dim udtPracticeDisplayList As PracticeDisplayModelCollection = _udtSessionHandler.PracticeDisplayListGetFromSession(FunctCode)

        If udtPracticeDisplayList.Count > 1 Then
            imageButton.Visible = True
        Else
            imageButton.Visible = False
        End If

    End Sub

    Private Function RuleResultKey(ByVal strActiveViewIndex As String, ByVal enumRuleType As RuleTypeENum) As String

        Return String.Format("{0}_{1}", strActiveViewIndex, enumRuleType)
    End Function

    '-----------------------------------------------------------------------------------------------------------------------------
    ' concurrent Browser checking
    '-----------------------------------------------------------------------------------------------------------------------------
    Private Sub EHSClaimTokenNumAssign()
        Me.hfEHSClaimTokenNum.Value = Me._udtSessionHandler.EHSClaimTokenNumberSaveToSession(FunctCode)
    End Sub

    Private Function EHSClaimTokenNumValidation(Optional ByVal blnRedirect As Boolean = True) As Boolean

        Dim strTurnOnChecking As String = String.Empty
        Dim udtGeneralF As New Common.ComFunction.GeneralFunction

        udtGeneralF.getSystemParameter("ConcurrentBrowserChecking", strTurnOnChecking, String.Empty)

        If strTurnOnChecking.Trim.Equals("N") Then
            Return True
        Else
            If Me.hfEHSClaimTokenNum.Value.Trim().ToString() <> Me._udtSessionHandler.EHSClaimTokenNumberGetFromSession(FunctCode) Then
                If blnRedirect Then
                    Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.EHSClaimError
                End If
                Return False
            Else
                Return True
            End If
        End If

    End Function

    '-----------------------------------------------------------------------------------------------------------------------------
    ' Check the Scheme exists in a specific Practice, for retain scheme selection after changing practice
    '-----------------------------------------------------------------------------------------------------------------------------
    Private Function CheckSchemeAvailableForEHSAccount(ByVal udtEHSAccount As EHSAccountModel, ByVal udtPracticeDisplay As BLL.PracticeDisplayModel, ByVal udtSchemeClaimModel As SchemeClaimModel) As Boolean

        ' Get SP info
        Dim udtDataEntry As DataEntryUserModel = Nothing
        ' Check Scheme Available
        Dim udtSchemeClaimBLL As SchemeClaimBLL = New SchemeClaimBLL()
        Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = Nothing

        Me.GetCurrentUserAccount(Me._udtSP, udtDataEntry, False)

        'If udtSchemeClaimModel Is Nothing Then
        '    ' Not Available
        '    Return False
        'End If

        If Not udtPracticeDisplay Is Nothing Then
            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            ' Practice Scheme Info List Filter by COVID-19
            Dim udtFilterPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = _udtSchemeClaimBLL.FilterPracticeSchemeInfo(Me._udtSP.PracticeList, udtPracticeDisplay.PracticeID, Me.ClaimMode)

            'Must Seach Again if practice Changed
            'Get all available Scheme
            udtSchemeClaimModelCollection = udtSchemeClaimBLL.searchValidClaimPeriodSchemeClaimByPracticeSchemeInfoSubsidizeCode(udtFilterPracticeSchemeInfoList, Me._udtSP.SchemeInfoList)
            ' CRE20-0022 (Immu record) [End][Chris YIM]

            'Get all Eligible Scheme form available List
            udtSchemeClaimModelCollection = udtSchemeClaimBLL.searchEligibleAndExceedDocumentClaimScheme(udtEHSAccount, udtEHSAccount.SearchDocCode, udtSchemeClaimModelCollection)

            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
            ' --------------------------------------------------------------------------------------
            udtSchemeClaimModelCollection = udtSchemeClaimModelCollection.FilterWithoutReadonly()
            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

            ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
            udtSchemeClaimModelCollection = udtSchemeClaimModelCollection.FilterByHCSPSubPlatform(Me.SubPlatform)
            ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

            If Not udtSchemeClaimModelCollection Is Nothing AndAlso udtSchemeClaimModelCollection.Count > 0 Then

                'Retain pervious scheme, if SchemeClaim not is nothing 
                If udtSchemeClaimModel Is Nothing Then
                    udtSchemeClaimModel = udtSchemeClaimModelCollection(0)
                End If

                Me._udtSessionHandler.SchemeSelectedSaveToSession(udtSchemeClaimModel, FunctCode)

                Me._udtSessionHandler.SchemeSubsidizeListSaveToSession(udtSchemeClaimModelCollection, FunctCode)
            Else
                Me._udtSessionHandler.SchemeSubsidizeListRemoveFromSession(FunctCode)
            End If

        Else
            udtSchemeClaimModelCollection = Me._udtSessionHandler.SchemeSubsidizeListGetFromSession(FunctCode)
        End If

        ''Get all available Scheme
        'udtSchemeClaimModelCollection = udtSchemeClaimBLL.searchValidClaimPeriodSchemeClaimByPracticeSchemeInfoSubsidizeCode(Me._udtSP.PracticeList(udtPracticeDisplay.PracticeID).PracticeSchemeInfoList)
        ''Get all Eligible Scheme form available List
        'udtSchemeClaimModelCollection = udtSchemeClaimBLL.searchEligibleClaimScheme(udtEHSAccount, udtEHSAccount.SearchDocCode, udtSchemeClaimModelCollection)

        If udtSchemeClaimModel Is Nothing Then
            ' Not Available
            Return False
        End If

        If udtSchemeClaimModelCollection.Filter(udtSchemeClaimModel.SchemeCode) Is Nothing Then
            ' Not available
            Return False
        Else
            ' Available
            Return True
        End If

    End Function

    Private Function IsValidServiceDate(ByVal strInputServiceDate As String) As Boolean

        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
        Dim udtSystemMessage As SystemMessage

        Dim udtValidator As Validator = New Validator
        Dim strClaimDayLimit As String = String.Empty
        Dim strMinDate As String = String.Empty
        Dim strAllowDateBack As String = String.Empty
        Dim intDayLimit As Integer
        Dim dtmMinDate As DateTime

        'Check Service Date Format
        udtSystemMessage = udtValidator.chkServiceDate(strInputServiceDate)
        If Not udtSystemMessage Is Nothing Then
            Return False
        End If

        'Check Service Date Back
        Me._udtGeneralFunction.getSystemParameter("DateBackClaimAllow", strAllowDateBack, String.Empty, udtSchemeClaim.SchemeCode)
        If strAllowDateBack = "Y" Then
            Me._udtGeneralFunction.getSystemParameter("DateBackClaimDayLimit", strClaimDayLimit, String.Empty, udtSchemeClaim.SchemeCode)
            Me._udtGeneralFunction.getSystemParameter("DateBackClaimMinDate", strMinDate, String.Empty, udtSchemeClaim.SchemeCode)

            intDayLimit = CInt(strClaimDayLimit)
            dtmMinDate = Convert.ToDateTime(strMinDate)

            udtSystemMessage = udtValidator.chkDateBackClaimServiceDate(strInputServiceDate, intDayLimit, dtmMinDate)
            If Not udtSystemMessage Is Nothing Then
                Return False
            End If

        End If

        Return True

    End Function

    Private Function RetainDocType(ByVal udtSchemeDocTypeList As SchemeDocTypeModelCollection, ByVal strPerviousCodeCode As String, ByVal blnBuildControl As Boolean) As Boolean
        Dim blnRetainDocType As Boolean = False

        For Each udtSchemeDocType As SchemeDocTypeModel In udtSchemeDocTypeList
            If strPerviousCodeCode.Equals(udtSchemeDocType.DocCode) AndAlso Me.udcClaimSearch.IsEmpty(strPerviousCodeCode) Then
                blnRetainDocType = True
                Exit For
            End If
        Next

        If Not blnRetainDocType Then
            Me.udcStep1DocumentTypeRadioButtonGroup.SelectedValue = Nothing
        End If

        Me.udcStep1DocumentTypeRadioButtonGroup.SchemeDocTypeList = udtSchemeDocTypeList
        Me.udcStep1DocumentTypeRadioButtonGroup.Scheme = Me.ddlStep1Scheme.SelectedValue

        If blnBuildControl Then
            udcStep1DocumentTypeRadioButtonGroup.HCSPSubPlatform = Me.SubPlatform

            If Me.SubPlatform = EnumHCSPSubPlatform.CN Then
                udcStep1DocumentTypeRadioButtonGroup.ShowLegend = False
            End If

            ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Me.udcStep1DocumentTypeRadioButtonGroup.Build(CustomControls.DocumentTypeRadioButtonGroup.FilterDocCode.VaccinationRecordEnquriySearch)
            ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

        End If

        Return blnRetainDocType
    End Function

    '==================================================================== Code for SmartID ============================================================================
    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub RedirectToIdeas(ByVal blnShowPopup As Boolean, ByVal enumIDEASVersion As IdeasBLL.EnumIdeasVersion)
        Dim udtSmarIDContent As BLL.SmartIDContentModel = New BLL.SmartIDContentModel
        Dim ideasHelper As IdeasRM.IHelper = IdeasRM.HelpFactory.createHelper()
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
        Dim ideasTokenResponse As IdeasRM.TokenResponse = Nothing
        Dim isDemoVersion As String = String.Empty
        Dim strLang As String = String.Empty
        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(FunctCode, Me)

        If Session("language") = TradChinese Then
            strLang = "zh_HK"
        ElseIf Session("language") = SimpChinese Then
            strLang = "zh_HK"
        Else
            strLang = "en_US"
        End If

        ' Remove Card Setting Read From SystemParameters
        Dim strRemoveCard As String = String.Empty
        Me._udtGeneralFunction.getSystemParameter("SmartID_RemoveCard", strRemoveCard, String.Empty)
        If strRemoveCard = String.Empty Then
            strRemoveCard = "Y"
        End If

        Dim eIdeasVersion As IdeasBLL.EnumIdeasVersion = IdeasBLL.GetIdeasVersion(enumIDEASVersion)
        Dim strIdeasVersion As String = IdeasBLL.ConvertIdeasVersion(eIdeasVersion)
        udtSmarIDContent.IdeasVersion = eIdeasVersion

        EHSClaimBasePage.AuditLogReadSamrtID(udtAuditLogEntry, udtSchemeClaim.SchemeCode, strIdeasVersion, Nothing)

        ' Enforce HCSP accept server cert for connecting IDEAS Testing server
        ServicePointManager.ServerCertificateValidationCallback = New Security.RemoteCertificateValidationCallback(AddressOf (New IdeasBLL).ValidateCertificate)

        ' Get Token From Ideas, input: the return URL from Ideas to eHS
        ideasTokenResponse = IdeasBLL.GetToken(eIdeasVersion, Me.Page.Request.Url.GetLeftPart(UriPartial.Path), strLang, strRemoveCard)

        If Not ideasTokenResponse.ErrorCode Is Nothing Then
            Me.udcMsgBoxErr.AddMessageDesc(FunctCode, ideasTokenResponse.ErrorCode, ideasTokenResponse.ErrorMessage)

            If isDemoVersion.Equals("Y") Then
                EHSClaimBasePage.AuditLogConnectIdeasFail(udtAuditLogEntry, udtSchemeClaim.SchemeCode, ideasTokenResponse, "Y", strIdeasVersion)
            Else
                EHSClaimBasePage.AuditLogConnectIdeasFail(udtAuditLogEntry, udtSchemeClaim.SchemeCode, ideasTokenResponse, "N", strIdeasVersion)
            End If

            Me.udcMsgBoxErr.BuildMessageDescBox("SmartIDActionFail", udtAuditLogEntry, Common.Component.LogID.LOG00049, "Click 'Read and Search Card' and Token Request Fail")

        Else
            isDemoVersion = ConfigurationManager.AppSettings("SmartIDDemoVersion")
            udtSmarIDContent.IsReadSmartID = True
            udtSmarIDContent.TokenResponse = ideasTokenResponse

            If isDemoVersion.Equals("Y") Then
                udtSmarIDContent.IsDemonVersion = True

                Me._udtSessionHandler.SmartIDContentSaveToSession(FunctCode, udtSmarIDContent)

                EHSClaimBasePage.AuditLogConnectIdeasComplete(udtAuditLogEntry, udtSchemeClaim.SchemeCode, ideasTokenResponse, "Y", strIdeasVersion)

                RedirectHandler.ToURL(ConfigurationManager.AppSettings("SmartIDTestRedirectPage").ToString().Replace("@", "&"))

            Else
                udtSmarIDContent.IsDemonVersion = False

                Me._udtSessionHandler.SmartIDContentSaveToSession(FunctCode, udtSmarIDContent)

                EHSClaimBasePage.AuditLogConnectIdeasComplete(udtAuditLogEntry, udtSchemeClaim.SchemeCode, ideasTokenResponse, "N", strIdeasVersion)

                ' Redirect to Ideas, no need to add page key
                Response.Redirect(ideasTokenResponse.IdeasMAURL)

            End If
        End If
    End Sub
    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub RedirectToIdeasCombo(ByVal enumIDEASVersion As IdeasBLL.EnumIdeasVersion)
        Dim udtSmarIDContent As BLL.SmartIDContentModel = New BLL.SmartIDContentModel
        Dim ideasHelper As IdeasRM.IHelper = IdeasRM.HelpFactory.createHelper()
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
        Dim ideasTokenResponse As IdeasRM.TokenResponse = Nothing
        Dim isDemoVersion As String = String.Empty
        Dim strLang As String = String.Empty
        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(FunctCode, Me)

        If Session("language") = TradChinese Then
            strLang = "zh_HK"
        ElseIf Session("language") = SimpChinese Then
            strLang = "zh_HK"
        Else
            strLang = "en_US"
        End If

        ' Remove Card Setting Read From SystemParameters
        Dim strRemoveCard As String = String.Empty
        Me._udtGeneralFunction.getSystemParameter("SmartID_RemoveCard", strRemoveCard, String.Empty)
        If strRemoveCard = String.Empty Then
            strRemoveCard = "Y"
        End If

        Dim eIdeasVersion As IdeasBLL.EnumIdeasVersion = IdeasBLL.GetIdeasVersion(enumIDEASVersion)
        Dim strIdeasVersion As String = IdeasBLL.ConvertIdeasVersion(eIdeasVersion)
        udtSmarIDContent.IdeasVersion = eIdeasVersion

        EHSClaimBasePage.AuditLogReadSamrtID(udtAuditLogEntry, udtSchemeClaim.SchemeCode, strIdeasVersion, Nothing)

        ' Enforce HCSP accept server cert for connecting IDEAS Testing server
        ServicePointManager.ServerCertificateValidationCallback = New Security.RemoteCertificateValidationCallback(AddressOf (New IdeasBLL).ValidateCertificate)

        ' Get Token From Ideas, input: the return URL from Ideas to eHS
        Select Case eIdeasVersion
            Case IdeasBLL.EnumIdeasVersion.One, IdeasBLL.EnumIdeasVersion.Two, IdeasBLL.EnumIdeasVersion.TwoGender
                ideasTokenResponse = IdeasBLL.GetToken(eIdeasVersion, Me.Page.Request.Url.GetLeftPart(UriPartial.Path), strLang, strRemoveCard)

            Case IdeasBLL.EnumIdeasVersion.Combo, IdeasBLL.EnumIdeasVersion.ComboGender
                Dim strPageName As String = New IO.FileInfo(Me.Request.Url.LocalPath).Name
                Dim strComboReturnURL As String = Me.Page.Request.Url.GetLeftPart(UriPartial.Path)
                Dim strFolderName As String = "/EHSClaim"

                strComboReturnURL = strComboReturnURL.Replace(strFolderName + "/" + strPageName, "/IDEASComboReader/IDEASComboReader.aspx")
                ideasTokenResponse = IdeasBLL.GetToken(eIdeasVersion, strComboReturnURL, strLang, strRemoveCard)

        End Select

        If Not ideasTokenResponse.ErrorCode Is Nothing Then
            Me.udcMsgBoxErr.AddMessageDesc(FunctCode, ideasTokenResponse.ErrorCode, ideasTokenResponse.ErrorMessage)

            If isDemoVersion.Equals("Y") Then
                EHSClaimBasePage.AuditLogConnectIdeasFail(udtAuditLogEntry, udtSchemeClaim.SchemeCode, ideasTokenResponse, "Y", strIdeasVersion)
            Else
                EHSClaimBasePage.AuditLogConnectIdeasFail(udtAuditLogEntry, udtSchemeClaim.SchemeCode, ideasTokenResponse, "N", strIdeasVersion)
            End If

            Me.udcMsgBoxErr.BuildMessageDescBox("SmartIDActionFail", udtAuditLogEntry, Common.Component.LogID.LOG00049, "Click 'Read and Search Card' and Token Request Fail")

        Else
            isDemoVersion = ConfigurationManager.AppSettings("SmartIDDemoVersion")
            udtSmarIDContent.IsReadSmartID = True
            udtSmarIDContent.TokenResponse = ideasTokenResponse

            If isDemoVersion.Equals("Y") Then
                udtSmarIDContent.IsDemonVersion = True

                Me._udtSessionHandler.SmartIDContentSaveToSession(FunctCode, udtSmarIDContent)

                EHSClaimBasePage.AuditLogConnectIdeasComboComplete(udtAuditLogEntry, udtSchemeClaim.SchemeCode, ideasTokenResponse, "Y", strIdeasVersion)
                'CRE20-0xx Immu record for testing smart IC card [Start][Nichole]
                ReadDemoSmartID(udtSmarIDContent)
                'CRE20-0xx Immu record for testing smart IC card [End][Nichole]
                'RedirectHandler.ToURL(ConfigurationManager.AppSettings("SmartIDTestRedirectPage").ToString().Replace("@", "&"))

            Else
                udtSmarIDContent.IsDemonVersion = False

                Me._udtSessionHandler.SmartIDContentSaveToSession(FunctCode, udtSmarIDContent)

                EHSClaimBasePage.AuditLogConnectIdeasComboComplete(udtAuditLogEntry, udtSchemeClaim.SchemeCode, ideasTokenResponse, "N", strIdeasVersion)

                ' Prompt the popup include iframe to show IDEAS Combo UI
                ucIDEASCombo.ReadSmartIC(IdeasBLL.EnumIdeasVersion.Combo, ideasTokenResponse, FunctCode)

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


    ''' <summary>
    ''' Get EHS Vaccination record and CMS Vaccination record, and Join together by current claiming scheme (no cache)
    ''' </summary>
    ''' <param name="udtEHSAccount"></param>
    ''' <param name="strSchemeCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetVaccinationRecord(ByVal udtEHSAccount As EHSAccountModel, Optional ByVal strSchemeCode As String = "") As TransactionDetailVaccineModelCollection

        Dim udtTranDetailVaccineList As TransactionDetailVaccineModelCollection = Nothing

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Dim udtVaccineResultBag As New VaccineResultCollection
        udtVaccineResultBag = _udtEHSClaimBLL.GetVaccinationRecord(udtEHSAccount, udtTranDetailVaccineList, _
                                                         FunctCode, _udtAuditLogEntry, _
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

    ' CRE20-0022 (Immu record) [Start][Martin Tang]
    Public Function IsVaccinationRecordPopUp(ByVal udtEHSAccount As EHSAccountModel, Optional ByVal strSchemeCode As String = "") As Boolean

        Dim isValid As Boolean = False
        Dim udtTranDetailVaccineList As TransactionDetailVaccineModelCollection = Nothing
        Dim udtVaccineResultBag As New VaccineResultCollection

        udtVaccineResultBag = _udtEHSClaimBLL.GetVaccinationRecord(udtEHSAccount,
                                                                   udtTranDetailVaccineList, _
                                                                   FunctCode, _
                                                                   _udtAuditLogEntry, _
                                                                   strSchemeCode)


        'udtVaccineResultBag.
        'udtSession.CMSVaccineResultGetFromSession(strFunctionCode)

        For Each udtTranDetailVaccine As TransactionDetailVaccineModel In udtTranDetailVaccineList
            If udtTranDetailVaccine.RecordType = TransactionDetailVaccineModel.RecordTypeClass.DemographicNotMatch Then
                isValid = True
                Exit For
            End If
        Next

        If VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N Then
            Select Case udtVaccineResultBag.HAReturnStatus
                Case VaccinationBLL.EnumVaccinationRecordReturnStatus.DemographicNotMatch
                    isValid = True
                Case VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail
                    isValid = True
            End Select
        End If

        If VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CIMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N Then
            Select Case udtVaccineResultBag.DHReturnStatus
                Case VaccinationBLL.EnumVaccinationRecordReturnStatus.OK
                    If udtVaccineResultBag.DHVaccineResult.SingleClient.ReturnClientCIMSCode = DHTransaction.DHClientModel.ReturnCIMSCode.AllDemographicMatch_PartialRecord Then
                        If udtVaccineResultBag.DHVaccineResult.GetNoOfValidVaccine > 0 Then
                            isValid = True
                        End If
                    End If
                Case VaccinationBLL.EnumVaccinationRecordReturnStatus.DemographicNotMatch
                    isValid = True
                Case VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail
                    isValid = True
            End Select
        End If

        Return isValid
    End Function
    ' CRE20-0022 (Immu record) [End][Martin Tang]


    Public Function GetVaccinationRecordFromSession(ByVal udtEHSAccount As EHSAccountModel, Optional ByVal strSchemeCode As String = "") As TransactionDetailVaccineModelCollection
        Dim udtVaccinationBLL As New VaccinationBLL
        Dim udtEHSTransactionBLL As New EHSTransactionBLL
        Dim udtSession As New BLL.SessionHandler

        Dim htRecordSummary As Hashtable = Nothing
        Dim udtTranDetailVaccineList As TransactionDetailVaccineModelCollection = Nothing
        Dim udtHAVaccineResult As Common.WebService.Interface.HAVaccineResult = New Common.WebService.Interface.HAVaccineResult(Common.WebService.Interface.HAVaccineResult.enumReturnCode.Error)
        Dim udtDHVaccineResult As Common.WebService.Interface.DHVaccineResult = New Common.WebService.Interface.DHVaccineResult(Common.WebService.Interface.DHVaccineResult.enumReturnCode.UnexpectedError)
        Dim udtHAVaccineResultSession As Common.WebService.Interface.HAVaccineResult = udtSession.CMSVaccineResultGetFromSession(FunctCode)
        Dim udtDHVaccineResultSession As Common.WebService.Interface.DHVaccineResult = udtSession.CIMSVaccineResultGetFromSession(FunctCode)

        If Me.CheckFromVaccinationRecordEnquiry Then
            If udtSession.CMSVaccineResultGetFromSession(Common.Component.FunctCode.FUNT020801) IsNot Nothing Then
                udtHAVaccineResultSession = udtSession.CMSVaccineResultGetFromSession(Common.Component.FunctCode.FUNT020801)
                udtSession.CMSVaccineResultSaveToSession(udtHAVaccineResultSession, FunctCode)
                udtSession.CMSVaccineResultRemoveFromSession(Common.Component.FunctCode.FUNT020801)
            End If

            If udtSession.CIMSVaccineResultGetFromSession(Common.Component.FunctCode.FUNT020801) IsNot Nothing Then
                udtDHVaccineResultSession = udtSession.CIMSVaccineResultGetFromSession(Common.Component.FunctCode.FUNT020801)
                udtSession.CIMSVaccineResultSaveToSession(udtDHVaccineResultSession, FunctCode)
                udtSession.CIMSVaccineResultRemoveFromSession(Common.Component.FunctCode.FUNT020801)
            End If
        End If

        Dim udtVaccineResultBag As New VaccineResultCollection
        udtVaccineResultBag.DHVaccineResult = udtDHVaccineResult
        udtVaccineResultBag.HAVaccineResult = udtHAVaccineResult

        Dim udtVaccineResultBagSession As New VaccineResultCollection
        udtVaccineResultBagSession.DHVaccineResult = udtDHVaccineResultSession
        udtVaccineResultBagSession.HAVaccineResult = udtHAVaccineResultSession

        udtVaccinationBLL.GetVaccinationRecord(udtEHSAccount, udtTranDetailVaccineList, udtVaccineResultBag, htRecordSummary, New AuditLogEntry(FunctCode, Me), strSchemeCode, udtVaccineResultBagSession)

        udtSession.CMSVaccineResultSaveToSession(udtVaccineResultBag.HAVaccineResult, FunctCode)
        udtSession.CIMSVaccineResultSaveToSession(udtVaccineResultBag.DHVaccineResult, FunctCode)

        udtTranDetailVaccineList.Sort(TransactionDetailVaccineModelCollection.enumSortBy.ServiceDate, SortDirection.Descending)

        Return udtTranDetailVaccineList
    End Function
    ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

    Private Function GetExtRefStatus(ByVal udtEHSAccount As EHSAccountModel, ByVal udtSchemeClaimModel As SchemeClaimModel) As EHSTransactionModel.ExtRefStatusClass
        Dim udtExtRefStatus As EHSTransactionModel.ExtRefStatusClass = (New SessionHandler).ExtRefStatusGetFromSession()
        If udtExtRefStatus Is Nothing Then
            ' CRE13-001 - EHAPP [Start][Koala]
            ' -------------------------------------------------------------------------------------
            If (New VaccinationBLL).SchemeContainVaccine(udtSchemeClaimModel) Then
                GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaimModel.SchemeCode)
                ' INT13-0021 - Fix use HKBC on smart IC claim incorrectly [Start][Koala]
                ' -------------------------------------------------------------------------------------
                udtExtRefStatus = New EHSTransactionModel.ExtRefStatusClass((New SessionHandler).CMSVaccineResultGetFromSession(FunctCode), udtEHSAccount.SearchDocCode)
                'udtExtRefStatus = New EHSTransactionModel.ExtRefStatusClass((New SessionHandler).CMSVaccineResultGetFromSession(FunctCode), udtEHSAccount.EHSPersonalInformationList(0).DocCode)
                ' INT13-0021 - Fix use HKBC on smart IC claim incorrectly [End][Koala]
            Else
                udtExtRefStatus = New EHSTransactionModel.ExtRefStatusClass()
            End If
            ' CRE13-001 - EHAPP [End][Koala]
        End If

        Return udtExtRefStatus
    End Function

    Private Function GetDHVaccineRefStatus(ByVal udtEHSAccount As EHSAccountModel, ByVal udtSchemeClaimModel As SchemeClaimModel) As EHSTransactionModel.ExtRefStatusClass
        Dim udtDHVaccineRefStatus As EHSTransactionModel.ExtRefStatusClass = (New SessionHandler).DHExtRefStatusGetFromSession()
        If udtDHVaccineRefStatus Is Nothing Then

            If (New VaccinationBLL).SchemeContainVaccine(udtSchemeClaimModel) Then
                GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaimModel.SchemeCode)

                udtDHVaccineRefStatus = New EHSTransactionModel.ExtRefStatusClass((New SessionHandler).CIMSVaccineResultGetFromSession(FunctCode), udtEHSAccount.SearchDocCode)
            Else
                udtDHVaccineRefStatus = New EHSTransactionModel.ExtRefStatusClass()
            End If

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
                ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
                ' -----------------------------------------------------------------------------------------
                Me._udtSystemMessage = udtValidator.chkServiceDataSubsidizeGroupLastServiceData(dtmServicedate, udtSchemeClaim.SubsidizeGroupClaimList.Filter(udtSchemeClaim.SchemeCode, udtEHSClaimVaccineSubsidize.SchemeSeq, udtEHSClaimVaccineSubsidize.SubsidizeCode))
                ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]
                If Not Me._udtSystemMessage Is Nothing Then
                    isValid = False
                    Me.imgStep2aServiceDateError.Visible = True
                    Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                End If
            End If
        Next

        Return isValid
    End Function

    Protected Overrides Sub CreateChildControls()
        MyBase.CreateChildControls()

        If String.IsNullOrEmpty(FunctCode) Then
            FunctCode = Me.ClaimFunctCode
        End If

        If Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode) Is Nothing Then Exit Sub
        If Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode) Is Nothing Then Exit Sub
        If mvEHSClaim.ActiveViewIndex <> 1 Then Exit Sub

        'Carry Forword: once times only
        If _udtSessionHandler.ClaimCOVID19CarryForwordGetFromSession(FunctCode) = False Then
            _udtSessionHandler.ClaimCOVID19CarryForwordSaveToSession(True, FunctCode)
        End If

        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        Me.udcStep2aInputEHSClaim.CurrentPractice = Me._udtSessionHandler.PracticeDisplayGetFromSession(FunctCode)
        Me.udcStep2aInputEHSClaim.SchemeType = udtSchemeClaim.SchemeCode.Trim()
        Me.udcStep2aInputEHSClaim.EHSAccount = Me._udtSessionHandler.EHSAccountGetFromSession(FunctCode)
        Me.udcStep2aInputEHSClaim.EHSTransaction = Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)
        'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
        'Me.udcStep2aInputEHSClaim.TableTitleWidth = 160
        Me.udcStep2aInputEHSClaim.TableTitleWidth = 205
        'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        'Me.udcStep2aInputEHSClaim.ServiceDate = Now()
        If Me.txtStep2aServiceDate.Attributes(ValidatedServiceDate) Is Nothing Then
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim udtSubPlatformBLL As New SubPlatformBLL
            'Me.txtStep2aServiceDate.Text = (New Formatter).formatEnterDate(Me._udtGeneralFunction.GetSystemDateTime())
            Me.txtStep2aServiceDate.Text = (New Formatter).formatInputTextDate(Me._udtGeneralFunction.GetSystemDateTime(), udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            Me.txtStep2aServiceDate.Attributes(ValidatedServiceDate) = _udtFormatter.formatInputDate(Me.txtStep2aServiceDate.Text, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
        End If
        Me.udcStep2aInputEHSClaim.ServiceDate = (New Formatter).convertDate(Me.txtStep2aServiceDate.Attributes(ValidatedServiceDate), Common.Component.CultureLanguage.English)
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        If Not udtSchemeClaim.SubsidizeGroupClaimList Is Nothing AndAlso _
            udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVaccine Then

            Me.udcStep2aInputEHSClaim.ClaimCategorys = Me._udtClaimCategoryBLL.getDistinctCategoryByScheme(Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode), _
                                                                                                           Me.udcStep2aInputEHSClaim.EHSAccount.getPersonalInformation(Me.udcStep2aInputEHSClaim.EHSAccount.SearchDocCode), _
                                                                                                           Me.udcStep2aInputEHSClaim.ServiceDate)
        End If

        Me.udcStep2aInputEHSClaim.ClaimMode = Me.ClaimMode

        Me.udcStep2aInputEHSClaim.BuiltSchemeControlOnly(False, True)

    End Sub

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

        GetCurrentUserAccount(udtSP, udtDataEntry, False)

        Return udtSP
    End Function

#End Region

#Region "Page Web Method"
    <WebMethod()> _
    <System.Web.Script.Services.ScriptMethod()> _
    Public Shared Function GetReasonForVisitL1(ByVal knownCategoryValues As String, ByVal category As String, ByVal contextKey As String) As CascadingDropDownNameValue()

        Dim udtReasonforVisitBLL As ReasonForVisitBLL = New ReasonForVisitBLL
        Dim dtReasonForVisit As DataTable

        dtReasonForVisit = udtReasonforVisitBLL.getReasonForVisitL1(category)

        Dim lst As New List(Of CascadingDropDownNameValue)
        For Each dr As DataRow In dtReasonForVisit.Rows
            Dim strChiColName As String = String.Empty

            Select Case contextKey.ToUpper
                Case English.ToUpper
                    strChiColName = "Reason_L1"
                Case TradChinese.ToUpper
                    strChiColName = "Reason_L1_Chi"
                Case SimpChinese.ToUpper
                    strChiColName = "Reason_L1_CN"
                Case Else
                    Throw New Exception(String.Format("EHSClaimV1.GetReasonForVisitL1: Unexpected value (contextKey={0})", contextKey))
            End Select

            lst.Add(New CascadingDropDownNameValue(dr(strChiColName), dr("Reason_L1_Code")))

        Next

        Return lst.ToArray
    End Function

    <WebMethod()> _
    <System.Web.Script.Services.ScriptMethod()> _
    Public Shared Function GetReasonForVisitL2(ByVal knownCategoryValues As String, ByVal category As String, ByVal contextKey As String) As CascadingDropDownNameValue()

        Dim udtReasonforVisitBLL As ReasonForVisitBLL = New ReasonForVisitBLL
        Dim dtReasonForVisit As DataTable
        Dim kv As StringDictionary

        Dim arrCategoryValues() As String = knownCategoryValues.Split(New Char() {";"}, StringSplitOptions.RemoveEmptyEntries)
        If arrCategoryValues.Length = 1 Then
            kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Else
            kv = CascadingDropDown.ParseKnownCategoryValuesString(arrCategoryValues(arrCategoryValues.Length - 1) + ";")
        End If

        dtReasonForVisit = udtReasonforVisitBLL.getReasonForVisitL2(category, kv(category))

        Dim lst As New List(Of CascadingDropDownNameValue)
        For Each dr As DataRow In dtReasonForVisit.Rows
            Dim strChiColName As String = String.Empty

            Select Case contextKey.ToUpper
                Case English.ToUpper
                    strChiColName = "Reason_L2"
                Case TradChinese.ToUpper
                    strChiColName = "Reason_L2_Chi"
                Case SimpChinese.ToUpper
                    strChiColName = "Reason_L2_CN"
                Case Else
                    Throw New Exception(String.Format("EHSClaimV1.GetReasonForVisitL2: Unexpected value (contextKey={0})", contextKey))
            End Select

            lst.Add(New CascadingDropDownNameValue(dr(strChiColName), dr("Reason_L2_Code")))

        Next

        Return lst.ToArray
    End Function

    Private Function CovertReasonForVisitToArray(ByVal dtReasonForVisit As DataTable) As CascadingDropDownNameValue()
        Dim lst As New List(Of CascadingDropDownNameValue)
        For Each dr As DataRow In dtReasonForVisit.Rows
            lst.Add(New CascadingDropDownNameValue(dr("Reason_L2"), dr("Reason_L2_Code")))
        Next

        Return lst.ToArray
    End Function
#End Region

    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    ' Popup legend again if legend data changed
    Private Sub udcSchemeLegend_DataChanged() Handles udcSchemeLegend.DataChanged
        Me.ModalPopupExtenderSchemeLegned.Show()
    End Sub

    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

    Private Sub Step2aCalendarExtenderServiceDate_Load(sender As Object, e As EventArgs) Handles Step2aCalendarExtenderServiceDate.Load
        Dim selectedLang As String
        Dim chineseTodayDateFormat As String

        selectedLang = LCase(Session("language"))
        Select Case selectedLang
            Case English
                Me.Step2aCalendarExtenderServiceDate.TodaysDateFormat = "d MMMM, yyyy"
                Me.Step2aCalendarExtenderServiceDate.DaysModeTitleFormat = "MMMM, yyyy"
            Case TradChinese, SimpChinese
                chineseTodayDateFormat = CStr(Today.Year) + "年" + CStr(Today.Month) + "月" + CStr(Today.Day) & "日"
                Me.Step2aCalendarExtenderServiceDate.TodaysDateFormat = chineseTodayDateFormat
                Me.Step2aCalendarExtenderServiceDate.DaysModeTitleFormat = "MMMM, yyyy"
            Case Else
                Me.Step2aCalendarExtenderServiceDate.TodaysDateFormat = "dd-MM-yyyy"
                Me.Step2aCalendarExtenderServiceDate.DaysModeTitleFormat = "MMMM, yyyy"
        End Select
    End Sub

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub HandleSystemMessage(ByVal sm As SystemMessage, ByVal udtClaimRuleResult As ClaimRuleResult)
        Dim lstStrIdx As List(Of String) = Nothing
        Dim lstStrReplaceMessage As List(Of String) = Nothing

        sm.GetReplaceMessage("%s1", lstStrIdx, lstStrReplaceMessage)

        If lstStrIdx Is Nothing Then
            If udtClaimRuleResult IsNot Nothing Then
                Select Case (sm.FunctionCode.ToString + "-" + sm.SeverityCode.ToString + "-" + sm.MessageCode.ToString)
                    Case "990000-E-00461"
                        'Me.udcMsgBoxErr.AddMessage(sm, "%DaysApart", udtClaimRuleResult.ResultParam("%DaysApart"))
                        Me.udcMsgBoxErr.AddMessage(sm)
                    Case Else
                        Me.udcMsgBoxErr.AddMessage(sm)
                End Select
            Else
                Me.udcMsgBoxErr.AddMessage(sm)
            End If

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
    ' CRE20-0022 (Immu record) [End][Chris YIM]

    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Private Sub CheckHKIDByOCSSS(ByVal strIdentityNo As String, ByVal strSPID As String, ByVal strSchemeCode As String)
        Dim udtOCSSSResult As OCSSSResult = Nothing

        Try
            udtOCSSSResult = (New OCSSSServiceBLL).IsEligible(strIdentityNo, _udtSessionHandler.HKICSymbolGetFormSession(FunctCode), strSPID, strSchemeCode)
        Catch ex As Exception
            Throw
        End Try

        _udtSessionHandler.OCSSSRefStatusSaveToSession(FunctCode, udtOCSSSResult.OCSSSStatus)

        'Validation
        If udtOCSSSResult.ConnectionStatus = OCSSSResult.OCSSSConnection.Success Then
            'Arise warning if HKIC no. is invalid
            If udtOCSSSResult.EligibleResult = OCSSSResult.OCSSSEligibleResult.Invalid Then
                _udtSystemMessage = New SystemMessage("990000", "E", "00420")
            End If
        End If

    End Sub
    ' CRE17-010 (OCSSS integration) [End][Chris YIM]

    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Private Sub CheckValidHKICInScheme(ByVal strSchemeCode)
        Dim udtEHSAccount As EHSAccountModel = _udtSessionHandler.EHSAccountGetFromSession(FunctCode)

        If Not udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode) Is Nothing AndAlso _
            udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode).DocCode = DocTypeModel.DocTypeCode.HKIC Then

            If Common.OCSSS.OCSSSServiceBLL.EnableHKICSymbolInput(strSchemeCode) And _
                _udtSessionHandler.HKICSymbolGetFormSession(FunctCode) Is Nothing Then

                Me._udtSystemMessage = New SystemMessage("990000", "E", "00422")
                Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)

                Me.SetClaimButtonEnable(Me.btnStep2aClaim, False)
                Me.udcStep2aInputEHSClaim.AvaliableForClaim = False
            End If
        End If

    End Sub
    ' CRE17-010 (OCSSS integration) [End][Chris YIM]

    ' CRE20-0022 (Immu record) [Start][Martin]
    Private Sub BuildbtnStep2aClaimEventForCOVID19()
        Dim jsBuildbtnStep2aClaimEventForCOVID19 As String = ""
        Dim strImageEnable As String = Me.GetGlobalResourceObject("ImageURL", "SaveBtn")
        Dim strImageDisable As String = Me.GetGlobalResourceObject("ImageURL", "SaveDisableBtn")

        jsBuildbtnStep2aClaimEventForCOVID19 += "$(""[id$='chkStep2aDeclareClaim']"").change(function () {"

        jsBuildbtnStep2aClaimEventForCOVID19 += "if ($(""[id$='chkStep2aDeclareClaim']"").is("":checked"")) {"
        jsBuildbtnStep2aClaimEventForCOVID19 += "$(""[id$='btnStep2aClaim']"").attr('disabled', false);"
        jsBuildbtnStep2aClaimEventForCOVID19 += "$(""[id$='btnStep2aClaim']"").attr('src','" & strImageEnable.Replace("~", "..") & "');"
        jsBuildbtnStep2aClaimEventForCOVID19 += "console.log(""checked V1"");"
        jsBuildbtnStep2aClaimEventForCOVID19 += "}"
        jsBuildbtnStep2aClaimEventForCOVID19 += "else {"
        jsBuildbtnStep2aClaimEventForCOVID19 += "$(""[id$='btnStep2aClaim']"").attr('disabled', true);"
        jsBuildbtnStep2aClaimEventForCOVID19 += "$(""[id$='btnStep2aClaim']"").attr('src','" & strImageDisable.Replace("~", "..") & "');"
        jsBuildbtnStep2aClaimEventForCOVID19 += "console.log(""unchecked V1"");"
        jsBuildbtnStep2aClaimEventForCOVID19 += "}"
        jsBuildbtnStep2aClaimEventForCOVID19 += "});"
        jsBuildbtnStep2aClaimEventForCOVID19 += "$(""[id$='chkStep2aDeclareClaim']"").change();"
        Dim guidText As String = Guid.NewGuid().ToString()
        jsBuildbtnStep2aClaimEventForCOVID19 += "RemoveUsedBlockScript('" & guidText & "');"

        ScriptManager.RegisterStartupScript(Me, Page.GetType(), "scriptStep2aClaim", jsBuildbtnStep2aClaimEventForCOVID19, True)
    End Sub
    ' CRE20-0022 (Immu record) [End][Martin]


    'CRE20-0xx For testing the smart IC card (demo version) [Start][Nichole]
    Private Function ReadDemoSmartID(ByVal udtSmartIDContent As BLL.SmartIDContentModel) As Boolean
        If IsNothing(udtSmartIDContent) OrElse udtSmartIDContent.IsReadSmartID = False OrElse udtSmartIDContent.IsEndOfReadSmartID Then Return False

        Dim isReadingSmartID As Boolean = False

        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(FunctCode, Me)
        Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)

        Dim ideasBLL As BLL.IdeasBLL = New BLL.IdeasBLL
        Dim strIdeasVersion As String = ideasBLL.ConvertIdeasVersion(udtSmartIDContent.IdeasVersion)


        isReadingSmartID = True
        udtSmartIDContent.IsEndOfReadSmartID = True
        Me._udtSessionHandler.SmartIDContentSaveToSession(FunctCode, udtSmartIDContent)
        udtSmartIDContent = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode)

        '--------------------------------------------------------------------------------------------------------------------------------------------------
        ' Smart ID Form Ideas
        '--------------------------------------------------------------------------------------------------------------------------------------------------
        Dim ideasHelper As IdeasRM.IHelper = IdeasRM.HelpFactory.createHelper()

        '--------------------------------------------------------------------------------------------------------------------------------------------------
        ' Get CFD
        '--------------------------------------------------------------------------------------------------------------------------------------------------
        Dim udtAuditLogEntry_GetCFD As AuditLogEntry = New AuditLogEntry(FunctCode, Me)
        Dim strArtifact As String = String.Empty

        If udtSmartIDContent.IdeasVersion = BLL.IdeasBLL.EnumIdeasVersion.Combo Or _
            udtSmartIDContent.IdeasVersion = BLL.IdeasBLL.EnumIdeasVersion.ComboGender Then

            strArtifact = udtSmartIDContent.Artifact
        Else
            strArtifact = ideasBLL.Artifact

        End If

        EHSClaimBasePage.AuditLogGetCFD(udtAuditLogEntry_GetCFD, strArtifact)
        Dim udtPersonalInfoSmartID As EHSAccountModel.EHSPersonalInformationModel
        Dim isValid As Boolean = True

        If isValid Then

            EHSClaimBasePage.AuditLogGetCFDComplete(udtAuditLogEntry_GetCFD, ideasBLL.Artifact)

            Dim udtEHSAccountExist As EHSAccountModel = Nothing
            Dim blnNotMatchAccountExist As Boolean = False
            Dim blnExceedDocTypeLimit As Boolean = False
            Dim udtEligibleResult As EligibleResult = Nothing
            Dim goToCreation As Boolean = True
            Dim strError As String = String.Empty

            Try
                ' dummy account for smart id
                ' ----------------------------------------------------------------------------------------
                udtSmartIDContent.EHSAccount = SmartIDDummyCase.GetDummyEHSAccount(udtSchemeClaim, udtSmartIDContent.IdeasVersion)
                udtSmartIDContent.EHSAccount.EHSPersonalInformationList(0).CName = BLL.VoucherAccountMaintenanceBLL.GetCName(udtSmartIDContent.EHSAccount.EHSPersonalInformationList(0))
                'udtSmartIDContent.EHSAccount.EHSPersonalInformationList(0).DateofIssue = Convert.ToDateTime("2016-07-28")

            Catch ex As Exception
                udtSmartIDContent.EHSAccount = Nothing
                strError = ex.Message
            End Try

            Dim udtAuditlogEntry_Search As AuditLogEntry = New AuditLogEntry(FunctCode, Me)
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
                Me._udtSystemMessage = EHSClaimBasePage.SmartIDCardFaceDataValidation(udtPersonalInfoSmartID)
                If Not Me._udtSystemMessage Is Nothing Then
                    isValid = False
                    If Not udtPersonalInfoSmartID.IdentityNum Is Nothing Then udtAuditlogEntry_Search.AddDescripton("HKID", udtPersonalInfoSmartID.IdentityNum)
                    If udtPersonalInfoSmartID.DateofIssue.HasValue Then udtAuditlogEntry_Search.AddDescripton("DOI", udtPersonalInfoSmartID.DateofIssue)
                    udtAuditlogEntry_Search.AddDescripton("DOB", udtPersonalInfoSmartID.DOB)

                    Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
                End If

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                SearchSmartID(udtSmartIDContent, isValid, udtAuditlogEntry_Search)
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
            Else
                '---------------------------------------------------------------------------------------------------------------
                ' udtSmartIDContent.EHSAccount is nothing, crad face data may not be able to return 
                '---------------------------------------------------------------------------------------------------------------
                Me.Clear()
                Me.udcStep1DocumentTypeRadioButtonGroup.SelectedValue = DocTypeModel.DocTypeCode.HKIC
                Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
                Me.udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00253"))
                isValid = False
            End If

            If udcMsgBoxErr.GetCodeTable.Rows.Count > 0 Then
                Me.udcMsgBoxErr.BuildMessageBox(_strValidationFail, udtAuditlogEntry_Search, Common.Component.LogID.LOG00052, "Search & validate account with CFD Fail", _
                    New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocTypeModel.DocTypeCode.HKIC, (New Formatter).formatDocumentIdentityNumber(DocTypeModel.DocTypeCode.HKIC, strHKICNo)))
            End If

        End If

        Return isReadingSmartID
    End Function
    'CRE20-0xx For testing the smart IC card (demo version) [End][Nichole]

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub SetupServiceDate(ByVal strSchemeCode As String)
        Dim strAllowDateBack As String = String.Empty
        Dim strDummy As String = String.Empty

        'Service date Textbox setup 
        Me._udtGeneralFunction.getSystemParameter("DateBackClaimAllow", strAllowDateBack, strDummy, strSchemeCode)

        'Override the DB settings
        If strAllowDateBack = String.Empty Then
            strAllowDateBack = YesNo.No
        End If

        If (strSchemeCode = SchemeClaimModel.VSS OrElse strSchemeCode = SchemeClaimModel.RVP) AndAlso Me.ClaimMode = Common.Component.ClaimMode.COVID19 Then
            strAllowDateBack = YesNo.No
        End If

        If strAllowDateBack = YesNo.Yes Then
            'Show Service Date TextBox
            Me.txtStep2aServiceDate.ForeColor = Drawing.Color.Black
            Me.txtStep2aServiceDate.Style.Remove("Display")
            Me.btnStep2aServiceDateCal.Visible = True
            Me.lblStep2aServiceDate.Visible = False

        Else
            ' Set Service Date as Today
            Dim udtFormatter As New Formatter
            Me.txtStep2aServiceDate.Text = udtFormatter.formatInputTextDate(Me._udtGeneralFunction.GetSystemDateTime(), (New SubPlatformBLL).GetDateFormatLocale(Me.SubPlatform))

            'Hide Service Date TextBox
            Me.txtStep2aServiceDate.ForeColor = Drawing.Color.DimGray
            Me.txtStep2aServiceDate.Style.Add("Display", "none")
            Me.btnStep2aServiceDateCal.Visible = False

            If Me.SubPlatform = EnumHCSPSubPlatform.CN Then
                Me.lblStep2aServiceDate.Text = Me.txtStep2aServiceDate.Text
            Else
                Me.lblStep2aServiceDate.Text = udtFormatter.formatDisplayDate(Me.txtStep2aServiceDate.Text, Me._udtSessionHandler.Language())
            End If

            Me.lblStep2aServiceDate.Visible = True
        End If
    End Sub
    ' CRE20-0022 (Immu record) [End][Chris YIM]

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Function GetCOVID19VaccineLotMapping(ByVal strSPID As String, ByVal intPracticeID As Integer, ByVal dtmServiceDate As Date) As DataRow()

        Dim udtCOVID19BLL As New COVID19.COVID19BLL

        'CRE20-023 Fix the filter on the Lot Mapping table[Start][Nichole]
        Dim dtVaccineLotNo As DataTable = udtCOVID19BLL.GetCOVID19VaccineLotMapping(strSPID, intPracticeID, dtmServiceDate)

        Dim drVaccineLotNo() As DataRow = Nothing

        If dtVaccineLotNo.Rows.Count > 0 Then
            ' drVaccineLotNo = udtCOVID19BLL.FilterActiveVaccineLotNoByServiceDate(dtVaccineLotNo, dtmServiceDate)
            drVaccineLotNo = dtVaccineLotNo.Select
        End If

        'CRE20-023 Fix the filter on the Lot Mapping table [End][Nichole]
        Return drVaccineLotNo

    End Function
    ' CRE20-0022 (Immu record) [End][Chris YIM]

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Function GetCOVID19VaccineLotMappingForPrivate(ByVal dtmServiceDate As Date) As DataRow()

        Dim udtCOVID19BLL As New COVID19.COVID19BLL

        Dim dtVaccineLotNo As DataTable = udtCOVID19BLL.GetCOVID19VaccineLotMappingForPrivate(String.Empty, Nothing, dtmServiceDate)
        Dim drVaccineLotNo() As DataRow = Nothing

        If dtVaccineLotNo.Rows.Count > 0 Then
            'CRE20-023 Fix the Lot Mapping table filter [Start][Nichole]
            ' drVaccineLotNo = udtCOVID19BLL.FilterActiveVaccineLotNoByServiceDate(dtVaccineLotNo, dtmServiceDate)
            drVaccineLotNo = dtVaccineLotNo.Select
            'CRE20-023 Fix the Lot Mapping table filter [End][Nichole]
        End If

        Return drVaccineLotNo

    End Function
    ' CRE20-0022 (Immu record) [End][Chris YIM]

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Function GetCOVID19VaccineLotMappingForRCH(ByVal dtmServiceDate As Date) As DataRow()

        Dim udtCOVID19BLL As New COVID19.COVID19BLL
        'CRE20-023 Fix the Lot Mapping table filter [Start][Nichole]
        Dim dtVaccineLotNo As DataTable = udtCOVID19BLL.GetCOVID19VaccineLotMappingForRCH(String.Empty, Nothing, dtmServiceDate)
        Dim drVaccineLotNo() As DataRow = Nothing

        If dtVaccineLotNo.Rows.Count > 0 Then
            'drVaccineLotNo = udtCOVID19BLL.FilterActiveVaccineLotNoByServiceDate(dtVaccineLotNo, dtmServiceDate)
            drVaccineLotNo = dtVaccineLotNo.Select
        End If
        'CRE20-023 Fix the Lot Mapping table filter [End][Nichole]
        Return drVaccineLotNo

    End Function
    ' CRE20-0022 (Immu record) [End][Chris YIM]


    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Function GetCOVID19VaccineLotMappingForCentre(ByVal strSPID As String, ByVal intPracticeID As Integer, ByVal dtmServiceDate As Date) As DataRow()

        Dim udtCOVID19BLL As New COVID19.COVID19BLL
        'CRE20-023 Fix the Lot Mapping table filter [Start][Nichole]
        Dim dtVaccineLotNo As DataTable = udtCOVID19BLL.GetCOVID19VaccineLotMappingForCentre(strSPID, intPracticeID, dtmServiceDate)
        Dim drVaccineLotNo() As DataRow = Nothing

        If dtVaccineLotNo.Rows.Count > 0 Then
            drVaccineLotNo = dtVaccineLotNo.Select
            'drVaccineLotNo = udtCOVID19BLL.FilterActiveVaccineLotNoByServiceDate(dtVaccineLotNo, dtmServiceDate)
        End If
        'CRE20-023 Fix the Lot Mapping table filter [End][Nichole]
        Return drVaccineLotNo

    End Function
    ' CRE20-0022 (Immu record) [End][Chris YIM]

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Function GetVaccineLotNoMappingForCentreJavaScript(ByVal strSPID As String, ByVal intPracticeID As Integer, ByVal dtmServiceDate As Date) As String
        Dim udtCOVID19BLL As New COVID19.COVID19BLL

        Dim strVaccineLotNoMappingJavaScript As String = String.Empty

        'Get Vaccine Lot No. mapping
        Dim drVaccineLotNo() As DataRow = Me.GetCOVID19VaccineLotMappingForCentre(strSPID, intPracticeID, dtmServiceDate)

        If drVaccineLotNo IsNot Nothing AndAlso drVaccineLotNo.Length > 0 Then
            'Convert DataRow -> Dictionary(Brand_ID, Vaccine_Lot_No) -> Json
            Dim strVaccineLotNoJson As String = udtCOVID19BLL.GenerateVaccineLotNoJson(drVaccineLotNo)

            'Generate JavaScript
            strVaccineLotNoMappingJavaScript = udtCOVID19BLL.GenerateVaccineLotNoMappingJavaScript(strVaccineLotNoJson)
        End If

        Return strVaccineLotNoMappingJavaScript

    End Function
    ' CRE20-0022 (Immu record) [End][Chris YIM]

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Function GetVaccineLotNoMappingForPrivateJavaScript(ByVal dtmServiceDate As Date) As String
        Dim udtCOVID19BLL As New COVID19.COVID19BLL

        Dim strVaccineLotNoMappingJavaScript As String = String.Empty

        'Get Vaccine Lot No. mapping
        Dim drVaccineLotNo() As DataRow = Me.GetCOVID19VaccineLotMappingForPrivate(dtmServiceDate)

        If drVaccineLotNo IsNot Nothing AndAlso drVaccineLotNo.Length > 0 Then
            'Convert DataRow -> Dictionary(Brand_ID, Vaccine_Lot_No) -> Json
            Dim strVaccineLotNoJson As String = udtCOVID19BLL.GenerateVaccineLotNoJson(drVaccineLotNo)

            'Generate JavaScript
            strVaccineLotNoMappingJavaScript = udtCOVID19BLL.GenerateVaccineLotNoMappingJavaScript(strVaccineLotNoJson)
        End If

        Return strVaccineLotNoMappingJavaScript

    End Function
    ' CRE20-0022 (Immu record) [End][Chris YIM]

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Function GetVaccineLotNoMappingForRCHJavaScript(ByVal dtmServiceDate As Date) As String
        Dim udtCOVID19BLL As New COVID19.COVID19BLL

        Dim strVaccineLotNoMappingJavaScript As String = String.Empty

        'Get Vaccine Lot No. mapping
        Dim drVaccineLotNo() As DataRow = Me.GetCOVID19VaccineLotMappingForRCH(dtmServiceDate)

        If drVaccineLotNo IsNot Nothing AndAlso drVaccineLotNo.Length > 0 Then
            'Convert DataRow -> Dictionary(Brand_ID, Vaccine_Lot_No) -> Json
            Dim strVaccineLotNoJson As String = udtCOVID19BLL.GenerateVaccineLotNoJson(drVaccineLotNo)

            'Generate JavaScript
            strVaccineLotNoMappingJavaScript = udtCOVID19BLL.GenerateVaccineLotNoMappingJavaScript(strVaccineLotNoJson)
        End If

        Return strVaccineLotNoMappingJavaScript

    End Function
    ' CRE20-0022 (Immu record) [End][Chris YIM]
End Class