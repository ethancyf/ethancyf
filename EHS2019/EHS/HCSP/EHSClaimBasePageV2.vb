Imports Common.Component.ServiceProvider
Imports Common.Component.DataEntryUser
Imports Common.Component.UserAC
Imports Common.Component.EHSAccount
Imports Common.Component.Scheme
Imports Common.Component.EHSTransaction
Imports Common.Component.ClaimRules
Imports Common.Component.ClaimRules.ClaimRulesBLL
Imports Common.ComObject
Imports Common.Component
Imports HCSP.BLL

Public MustInherit Class EHSClaimBasePageV2
    Inherits TextOnlyBasePage

    Protected Class RuleGroupCode
        Public Const ClaimRuleFirstDose As String = "1STDOSE"
        Public Const ClaimRuleSecondDose As String = "2NDDOSE"
    End Class

    Protected Enum PrintoutOption
        ChineseFull
        ChineseCondensed
        EnglishFull
        EnglishCondensed
        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        SimpChineseFull
        SimpChineseCondensed
        'CRE13-019-02 Extend HCVS to China [End][Winnie]
    End Enum

    'CRE13-019-02 Extend HCVS to China [Start][Winnie]
    Protected Class PrintOptionLanguage
        Public Const TradChinese As String = "ZH"
        Public Const SimpChinese As String = "CN"
        Public Const English As String = "EN"
    End Class
    'CRE13-019-02 Extend HCVS to China [End][Winnie]

    ' Helper Class
    Private _udtSessionHandler As BLL.SessionHandler = New BLL.SessionHandler()
    Private _udtFormatter As Common.Format.Formatter
    Private _udtValidator As Common.Validation.Validator
    Private _udtStaticDataBLL As StaticData.StaticDataBLL

    ' Attribute
    Private _blnIsConcurrentBrowser As Boolean = False

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Master Page Related
        If Not Me.ClaimMasterPage Is Nothing Then
            ' Set Master Page Title
            CType(Me.Master.FindControl(ClaimVoucherMaster.ControlName.lblTitle), Label).Text = PageTitle
            ' Set Master Page SubTitle
            CType(Me.Master.FindControl(ClaimVoucherMaster.ControlName.lblSubTitle), Label).Text = PageSubTitle
            ' Set Master Page Step
            CType(Me.Master.FindControl(ClaimVoucherMaster.ControlName.LblClaimVoucherStep), Label).Text = PageStepTitle

            ' Construct the Menu
            Me.BuildMenuItem()
        End If


        ' Concurrent Browser Related
        _blnIsConcurrentBrowser = CheckIsConcurrentBrowser()

    End Sub

#Region "Step 2a Validation for Enter Claim detail"

    Protected Overridable Function CIVSSValidation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
    End Function

    Protected Overridable Function EVSSValidation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
    End Function

    Protected Overridable Function HSIVSSValidation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
    End Function

    Protected Overridable Function RVPValidation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
    End Function

    'CRE15-005-03 (New PIDVSS scheme) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Protected Overridable Function PIDVSSValidation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
    End Function
    'CRE15-005-03 (New PIDVSS scheme) [End][Chris YIM]

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Protected Overridable Function VSSValidation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
    End Function
    'CRE16-002 (Revamp VSS) [End][Chris YIM]

    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Protected Overridable Function ENHVSSOValidation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
    End Function
    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Protected Overridable Function PPPValidation(ByVal checkByConfirmationBox As Boolean, ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
    End Function
    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

    Protected Function Step2aPromptClaimRule(ByVal udtClaimRuleResult As ClaimRuleResult) As String
        Dim strText As String = String.Empty
        Dim udtFormatter As Common.Format.Formatter = New Common.Format.Formatter()
        'Rule Detail: service date is over 24 days and less then 28 date, prompt warning message

        If Not udtClaimRuleResult.RelatedClaimRule Is Nothing Then
            strText = Me.GetGlobalResourceObject("Text", udtClaimRuleResult.RelatedClaimRule.ObjectName)

            If udtClaimRuleResult.dtmDoseDate.HasValue Then
                Dim udtSubPlatformBLL As New SubPlatformBLL
                strText = strText.Replace("%date", udtFormatter.formatDisplayDate(udtClaimRuleResult.dtmDoseDate.Value, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform)))
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

#End Region

#Region "Step 2b Confirm Claim detail"

    Protected Overridable Sub SetupStepConfirmClaim(ByVal udtEHSAccount As EHSAccountModel, ByVal blnInitAll As Boolean)

    End Sub

    Protected Overridable Sub StepConfirmClaimClear()

    End Sub

#End Region

#Region "Step 3 Complete claim"

    Protected Overridable Sub SetupCompleteClaim(ByVal udtEHSAccount As EHSAccountModel, ByVal activeViewChange As Boolean)

    End Sub

    Protected Overridable Sub SetupCompleteClaimClear()

    End Sub

#End Region

#Region "Step Internal Error "

    Protected Overridable Sub SetupInternalError()

    End Sub

#End Region

#Region "Step Printout"

    'CRE13-019-02 Extend HCVS to China [Start][Winnie]
    'Rewrite function
    ' Take Current User Print Option
    'Protected Sub GeneratePrintout(ByVal strLanguage As String)

    '    Dim strCurrentUserPrintOption As String = Me.GetCurrentUserPrintOption()

    '    If strLanguage = CultureLanguage.English Then

    '        If strCurrentUserPrintOption = Common.Component.PrintFormOptionValue.PrintPurposeAndConsent Then
    '            GeneratePrintout(PrintoutOption.EnglishFull, False)
    '        ElseIf strCurrentUserPrintOption = Common.Component.PrintFormOptionValue.PrintConsentOnly Then
    '            GeneratePrintout(PrintoutOption.EnglishCondensed, False)
    '        End If

    '    ElseIf strLanguage = CultureLanguage.TradChinese Then

    '        If strCurrentUserPrintOption = Common.Component.PrintFormOptionValue.PrintPurposeAndConsent Then
    '            GeneratePrintout(PrintoutOption.ChineseFull, False)
    '        ElseIf strCurrentUserPrintOption = Common.Component.PrintFormOptionValue.PrintConsentOnly Then
    '            GeneratePrintout(PrintoutOption.ChineseCondensed, False)
    '        End If

    '    End If

    'End Sub

    Protected Sub GeneratePrintout(ByVal strLanguage As String, ByVal strCurrentPrintOption As String)

        If strLanguage = PrintOptionLanguage.English Then

            If strCurrentPrintOption = PrintFormOptionValue.PrintPurposeAndConsent Then
                GeneratePrintout(PrintoutOption.EnglishFull, False)
            ElseIf strCurrentPrintOption = PrintFormOptionValue.PrintConsentOnly Then
                GeneratePrintout(PrintoutOption.EnglishCondensed, False)
            End If

        ElseIf strLanguage = PrintOptionLanguage.TradChinese Then

            If strCurrentPrintOption = PrintFormOptionValue.PrintPurposeAndConsent Then
                GeneratePrintout(PrintoutOption.ChineseFull, False)
            ElseIf strCurrentPrintOption = PrintFormOptionValue.PrintConsentOnly Then
                GeneratePrintout(PrintoutOption.ChineseCondensed, False)
            End If

        ElseIf strLanguage = PrintOptionLanguage.SimpChinese Then

            If strCurrentPrintOption = PrintFormOptionValue.PrintPurposeAndConsent Then
                GeneratePrintout(PrintoutOption.SimpChineseFull, False)
            ElseIf strCurrentPrintOption = PrintFormOptionValue.PrintConsentOnly Then
                GeneratePrintout(PrintoutOption.SimpChineseCondensed, False)
            End If
        End If

    End Sub
    'CRE13-019-02 Extend HCVS to China [End][Winnie]

    ' Generate Printout by Print Option
    Protected Overridable Sub GeneratePrintout(ByVal printOption As PrintoutOption, ByVal isAdHocPrint As Boolean)

        Dim strPrintDateTime As String = String.Format("DH_HCV103{0}{1}{2}{3}{4}{5}{6}", Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, Now.Second, Now.Millisecond)
        Dim udtEHSTransaction As EHSTransactionModel = SessionHandler.EHSTransactionGetFromSession(FunctionCode)

        'Set Transaction Print Status
        udtEHSTransaction.PrintedConsentForm = True

        'Set the transaction is printed consent Form
        SessionHandler.EHSTransactionSaveToSession(udtEHSTransaction, FunctionCode)

        'Save the current function code to session (will be removed in the printout form)
        SessionHandler.EHSClaimPrintoutFunctionCodeSaveToSession(FunctionCode)

        'CRE15-003 System-generated Form [Start][Philip Chau]
        Select Case printOption
            Case PrintoutOption.EnglishCondensed
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/EHSClaimCondensedForm_RV.aspx?TID=" + strPrintDateTime + "');", True)
                EHSClaimBasePage.AuditLogPrintFrom(New AuditLogEntry(FunctionCode, Me), isAdHocPrint, "EHSClaimCondensedForm_RV", "ENG", _udtSessionHandler.EHSClaimTempTransactionIDGetFromSession())

            Case PrintoutOption.EnglishFull
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/EHSClaimForm_RV.aspx?TID=" + strPrintDateTime + "');", True)
                EHSClaimBasePage.AuditLogPrintFrom(New AuditLogEntry(FunctionCode, Me), isAdHocPrint, "EHSClaimForm_RV", "ENG", _udtSessionHandler.EHSClaimTempTransactionIDGetFromSession())

            Case PrintoutOption.ChineseCondensed
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/EHSClaimCondensedForm_CHI_RV.aspx?TID=" + strPrintDateTime + "');", True)
                EHSClaimBasePage.AuditLogPrintFrom(New AuditLogEntry(FunctionCode, Me), isAdHocPrint, "EHSClaimCondensedForm_CHI_RV", "CHI", _udtSessionHandler.EHSClaimTempTransactionIDGetFromSession())

            Case PrintoutOption.ChineseFull
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/EHSClaimForm_CHI_RV.aspx?TID=" + strPrintDateTime + "');", True)
                EHSClaimBasePage.AuditLogPrintFrom(New AuditLogEntry(FunctionCode, Me), isAdHocPrint, "EHSClaimForm_CHI_RV", "CHI", _udtSessionHandler.EHSClaimTempTransactionIDGetFromSession())

                'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            Case PrintoutOption.SimpChineseCondensed
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/EHSClaimCondensedForm_CN_RV.aspx?TID=" + strPrintDateTime + "');", True)
                EHSClaimBasePage.AuditLogPrintFrom(New AuditLogEntry(FunctionCode, Me), isAdHocPrint, "EHSClaimCondensedForm_CN_RV", PrintOptionLanguage.SimpChinese, _udtSessionHandler.EHSClaimTempTransactionIDGetFromSession())

            Case PrintoutOption.SimpChineseFull
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/EHSClaimForm_CN_RV.aspx?TID=" + strPrintDateTime + "');", True)
                EHSClaimBasePage.AuditLogPrintFrom(New AuditLogEntry(FunctionCode, Me), isAdHocPrint, "EHSClaimForm_CN_RV", PrintOptionLanguage.SimpChinese, _udtSessionHandler.EHSClaimTempTransactionIDGetFromSession())
                'CRE13-019-02 Extend HCVS to China [End][Winnie]
        End Select
        'CRE15-003 System-generated Form [End][Philip Chau]
    End Sub

    ' Get the Print Option of the SP/Data Entry User
    Protected Function GetCurrentUserPrintOption() As String
        Dim strPrintOption As String = Nothing
        Dim udtSP As ServiceProviderModel = Nothing
        Dim udtDataEntryUser As DataEntryUserModel = Nothing
        Me.GetCurrentUserAccount(udtSP, udtDataEntryUser, False)

        If udtDataEntryUser Is Nothing Then
            strPrintOption = udtSP.PrintOption
        Else
            strPrintOption = udtDataEntryUser.PrintOption
        End If

        Return strPrintOption
    End Function

    ' Get the Print Option. Return True if User use AdHoc Printing
    Protected Function IsPrePrintDocument() As Boolean
        Dim udtSP As ServiceProviderModel = Nothing
        Dim udtDataEntryModel As DataEntryUserModel = Nothing
        Dim strSelectedPrintOption As String = String.Empty

        SessionHandler.CurrentUserGetFromSession(udtSP, udtDataEntryModel)
        If udtDataEntryModel Is Nothing Then
            strSelectedPrintOption = udtSP.PrintOption
        Else
            strSelectedPrintOption = udtDataEntryModel.PrintOption
        End If

        If strSelectedPrintOption = Common.Component.PrintFormOptionValue.PreprintForm Then
            Return True
        Else
            Return False
        End If

    End Function

    ' Update Print Option for the Current User Account
    Protected Function UpdateUserPrintOption(ByVal strPrintOption As String) As SystemMessage
        ' Check Input is Valid
        Dim udtSysMsgPrintOption As SystemMessage = Validator.chkSelectedPrintFormOption(strPrintOption)
        If udtSysMsgPrintOption Is Nothing Then

            Dim udtClaimVoucherBLL As BLL.ClaimVoucherBLL = New BLL.ClaimVoucherBLL()
            Dim udtEHSTransaction As EHSTransactionModel = SessionHandler.EHSTransactionGetFromSession(FunctionCode)
            Dim udtSP As ServiceProviderModel = Nothing
            Dim udtDataEntryUser As DataEntryUserModel = Nothing
            Me.GetCurrentUserAccount(udtSP, udtDataEntryUser, False)

            ' Update Print Option
            If GetCurrentUserPrintOption() <> strPrintOption Then
                'CRE15-003 Print Tx No. Prefix into Consent Form [Start][Winnie]
                'If Not udtSP Is Nothing Then
                If udtDataEntryUser Is Nothing Then
                    ' Assign Print Option to Servive Provider
                    udtSP.PrintOption = strPrintOption
                    udtClaimVoucherBLL.updatePrintOption(udtSP.SPID, String.Empty, udtSP.PrintOption)
                    SessionHandler.CurrentUserSaveToSession(udtSP, Nothing)

                    'ElseIf Not udtDataEntryUser Is Nothing Then
                Else
                    ' Assign Print Option to Data Entry User
                    udtDataEntryUser.PrintOption = strPrintOption
                    udtClaimVoucherBLL.updatePrintOption(udtSP.SPID, udtDataEntryUser.DataEntryAccount, udtDataEntryUser.PrintOption)
                    SessionHandler.CurrentUserSaveToSession(udtSP, udtDataEntryUser)
                End If
                'CRE15-003 Print Tx No. Prefix into Consent Form [End][Winnie]

                HandlePrintOptionChanged()
            End If

        End If

        Return udtSysMsgPrintOption

    End Function

    ' Method to Handle Print Option Changed
    Protected Overridable Sub HandlePrintOptionChanged()

        ' Reset Print Status when Print Option have been Updated
        Dim udtEHSTransaction As EHSTransactionModel = SessionHandler.EHSTransactionGetFromSession(FunctionCode)
        udtEHSTransaction.PrintedConsentForm = False
        SessionHandler.EHSTransactionSaveToSession(udtEHSTransaction, FunctionCode)

    End Sub

#End Region

#Region "Concurrent Browser Checking"

    Protected Sub EHSClaimTokenNumAssign()
        BrowserTokenHiddenField.Value = SessionHandler.EHSClaimTokenNumberSaveToSession(FunctionCode)
    End Sub

    Protected Overridable Function CheckIsConcurrentBrowser() As Boolean
        Dim blnResult As Boolean = False

        If Not String.IsNullOrEmpty(SessionHandler.EHSClaimTokenNumberGetFromSession(FunctionCode)) Then
            ' Token Assigned Before
            ' Check the Token in this Page is same as the one in Session
            If BrowserTokenHiddenField.Value.Trim() <> SessionHandler.EHSClaimTokenNumberGetFromSession(FunctionCode) Then
                blnResult = True
            End If

        End If

        Return blnResult

    End Function

    Protected Overridable Sub HandleConcurrentBrowser()

    End Sub

#End Region

#Region "Other functions"

    Protected Sub GetCurrentUserAccount(ByRef passedSP As ServiceProviderModel, ByRef passedDataEntry As DataEntryUserModel, ByVal getFormDataBase As Boolean)
        'Get Current USer Account
        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC
        Dim udtClaimVoucherBLL As BLL.ClaimVoucherBLL = New BLL.ClaimVoucherBLL()

        If getFormDataBase Then
            If udtUserAC.UserType = Common.Component.SPAcctType.ServiceProvider Then
                'Get SP from form database
                passedSP = CType(udtUserAC, ServiceProviderModel)
                passedSP = udtClaimVoucherBLL.loadSP(passedSP.SPID)

                passedDataEntry = Nothing

            ElseIf udtUserAC.UserType = Common.Component.SPAcctType.DataEntryAcct Then
                passedDataEntry = CType(udtUserAC, DataEntryUserModel)

                'Get the latest data entry account mordel form database
                Dim udtDataEntryAcctBLL As BLL.DataEntryAcctBLL = New BLL.DataEntryAcctBLL
                passedDataEntry = udtDataEntryAcctBLL.LoadDataEntry(passedDataEntry.SPID, passedDataEntry.DataEntryAccount)

                'Get the latest service provider account mordel 
                passedSP = udtClaimVoucherBLL.loadSP(passedDataEntry.SPID)
            End If

            SessionHandler.CurrentUserSaveToSession(passedSP, passedDataEntry)
        Else
            SessionHandler.CurrentUserGetFromSession(passedSP, passedDataEntry)
        End If

    End Sub

    '-----------------------------------------------------------------------------------------------------------------------------
    'Clear Claim for new patient
    '-----------------------------------------------------------------------------------------------------------------------------
    Protected Overridable Sub Clear(ByVal strFunctCode As String)

        Me.StepConfirmClaimClear()

        Me.SetupCompleteClaimClear()

        Me.SessionHandler.EHSClaimSessionRemove(strFunctCode)

    End Sub

    Protected Function IsSupportedDevice() As Boolean
        Dim isMobileDevice As Object = Me._udtSessionHandler.IsMobileDeviceGetFromSession()

        If Not isMobileDevice Is Nothing Then
            Return CType(isMobileDevice, Boolean)
        Else
            isMobileDevice = MyBase.GetIsSupportedDevice()
            Me._udtSessionHandler.IsMobileDeviceSaveToSession(CType(isMobileDevice, Boolean))

            Return CType(isMobileDevice, Boolean)
        End If

    End Function

#End Region

#Region "Menu Item"
    Protected Sub BuildMenuItem()

        Dim masterPage As ClaimVoucherMaster = ClaimMasterPage
        If IsMenuItemVisible() Then
            AddHandler masterPage.MenuChanged, AddressOf MasterPage_MenuChanged
            masterPage.BuildMenu(FunctionCode, SessionHandler.Language())
        Else
            masterPage.ClearMenu()
        End If

    End Sub

    Protected Overridable Function IsMenuItemVisible() As Boolean
        Return True
    End Function

    Protected Sub MasterPage_MenuChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.SessionHandler.EHSClaimSessionRemove(FunctionCode)
    End Sub
#End Region

#Region "Property"

    ' Read Only Property
    Protected ReadOnly Property SessionHandler() As BLL.SessionHandler
        Get
            Return Me._udtSessionHandler
        End Get
    End Property

    Protected ReadOnly Property Formatter() As Common.Format.Formatter
        Get
            If _udtFormatter Is Nothing Then
                _udtFormatter = New Common.Format.Formatter
            End If

            Return _udtFormatter
        End Get
    End Property

    Protected ReadOnly Property Validator() As Common.Validation.Validator
        Get
            If _udtValidator Is Nothing Then
                _udtValidator = New Common.Validation.Validator
            End If

            Return _udtValidator
        End Get
    End Property

    Protected ReadOnly Property StaticDataBLL() As StaticData.StaticDataBLL
        Get
            If _udtStaticDataBLL Is Nothing Then
                _udtStaticDataBLL = New StaticData.StaticDataBLL
            End If

            Return _udtStaticDataBLL
        End Get
    End Property

    Protected ReadOnly Property ClaimVoucherMaster() As ClaimVoucherMaster
        Get
            If TypeOf Me.Master Is ClaimVoucherMaster Then
                Return CType(Me.Master, ClaimVoucherMaster)
            End If
            Return Nothing
        End Get
    End Property

    Protected ReadOnly Property ClaimMasterPage() As ClaimVoucherMaster
        Get
            Return CType(Me.Master, ClaimVoucherMaster)
        End Get
    End Property

    Protected MustOverride ReadOnly Property FunctionCode() As String

    Protected MustOverride ReadOnly Property BrowserTokenHiddenField() As HiddenField

    Protected Overridable ReadOnly Property PageTitle() As String
        Get
            Return Me.GetGlobalResourceObject("Text", "EVoucherSystem")
        End Get
    End Property

    Protected Overridable ReadOnly Property PageSubTitle() As String
        Get
            Return Me.GetGlobalResourceObject("Text", "ClaimVoucher")
        End Get
    End Property

    Protected Overridable ReadOnly Property PageStepTitle() As String
        Get
            Return String.Empty
        End Get
    End Property


    ' Read/Write Property
    Protected Property IsConcurrentBrowser() As Boolean
        Get
            Return _blnIsConcurrentBrowser
        End Get
        Set(ByVal value As Boolean)
            _blnIsConcurrentBrowser = value

            If value = True Then
                HandleConcurrentBrowser()
            End If
        End Set
    End Property

#End Region

End Class
