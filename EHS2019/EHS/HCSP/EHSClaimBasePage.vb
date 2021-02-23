Imports Common.Component.EHSAccount
Imports Common.Component.Scheme
Imports Common.Component.EHSTransaction
Imports Common.Component.ClaimRules
Imports Common.Component.ClaimRules.ClaimRulesBLL
Imports Common.ComObject
Imports Common.Component
Imports Common.Format
Imports HCSP.BLL

Public MustInherit Class EHSClaimBasePage
    Inherits TextOnlyBasePage

    Protected Class RuleGroupCode
        Public Const ClaimRuleFirstDose As String = "1STDOSE"
        Public Const ClaimRuleSecondDose As String = "2NDDOSE"
    End Class

    Protected Class PrintOptionValue

        Public Const FullChi As String = "FullChi"
        Public Const FullEng As String = "FullEng"
        Public Const CondensedChi As String = "CondensedChi"
        Public Const CondensedEng As String = "CondensedEng"

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        'Public Const Chi As String = "Chi"
        'Public Const Eng As String = "Eng"

        Public Const FullSimpChi As String = "FullSimpChi"
        Public Const CondensedSimpChi As String = "CondensedSimpChi"
        'CRE13-019-02 Extend HCVS to China [End][Winnie]
    End Class

    'CRE13-019-02 Extend HCVS to China [Start][Winnie]
    Protected Class PrintOptionLanguage
        Public Const TradChinese As String = "ZH"
        Public Const SimpChinese As String = "CN"
        Public Const English As String = "EN"        
    End Class
    'CRE13-019-02 Extend HCVS to China [End][Winnie]

    Public Class ActiveViewIndex
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

        'Select Scheme
        Public Const SelectScheme As Integer = 5

        'Select Doc Type
        Public Const SelectDocType As Integer = 6

        'EHS Claim Error
        Public Const InternalError As Integer = 7

        'Confirmation Box
        Public Const ConfirmBox As Integer = 8

        'Change Print Option
        Public Const PrintOption As Integer = 9

        ' Adhoc Print
        Public Const AdHocPrint As Integer = 10

        ' Remark View
        Public Const Remark As Integer = 11

        ' Input Tips View
        Public Const InputTip As Integer = 12

        ' Change Reason For Visit View
        Public Const ReasonForVisit As Integer = 13

        ' Change Category
        Public Const Category As Integer = 14

        ' Change PerConditions
        'Public Const PerConditions As Integer = 15

        ' Vaccination Record
        Public Const VaccinationRecord As Integer = 15

    End Class

    Private _udtSessionHandler As BLL.SessionHandler = New BLL.SessionHandler()
#Region "Property"

    Protected MustOverride ReadOnly Property FunctionCode() As String

#End Region

#Region "Claim Step1 Search account"

    Protected MustOverride Function Step1SearchValdiation(ByVal udtAuditLogEntry As AuditLogEntry) As Boolean
    Protected MustOverride Sub SetupStep1(ByVal createPopupPractice As Boolean, ByVal activeViewChange As Boolean)
    Protected MustOverride Sub Step1Clear(ByVal blnRetainDocType As Boolean)

#End Region

#Region "Step 2a Enter Claim detail Events"

    Protected MustOverride Sub SetupStep2a(ByVal udtEHSAccount As EHSAccountModel, ByVal activeViewChange As Boolean, ByVal setDefaultScheme As Boolean)
    Protected MustOverride Sub SetupStep2aSchemeAvailableForClaim(ByVal isAvailable As Boolean, ByVal udtEHSAccount As EHSAccountModel, ByVal udtSchemeClaim As SchemeClaimModel, ByVal activeViewChange As Boolean, Optional ByVal udtClaimCategorys As ClaimCategory.ClaimCategoryModelCollection = Nothing)
    Protected MustOverride Sub SetupStep2aClaimContent(ByVal udtSchemeClaim As SchemeClaimModel, ByVal udtEHSAccount As EHSAccountModel, ByVal activeViewChange As Boolean, Optional ByVal udtClaimCategorys As ClaimCategory.ClaimCategoryModelCollection = Nothing)
    Protected MustOverride Sub Step2aClear()

#End Region

#Region "Step 2a Validation for Enter Claim detail"

    Protected MustOverride Function Step2aHCVSValidation(ByVal blnIsConfirmed As Boolean, ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
    'CRE13-019-02 Extend HCVS to China [Start][Karl]
    Protected MustOverride Function Step2aHCVSChinaValidation(ByVal blnIsConfirmed As Boolean, ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
    'CRE13-019-02 Extend HCVS to China [End][Karl]

    Protected Function Step2aPromptClaimRule(ByVal udtClaimRuleResult As ClaimRuleResult) As String
        Dim strText As String = String.Empty
        Dim udtFormatter As Common.Format.Formatter = New Common.Format.Formatter()
        'Rule Detail: service date is over 24 days and less then 28 date, prompt warning message

        If Not udtClaimRuleResult.RelatedClaimRule Is Nothing Then
            strText = Me.GetGlobalResourceObject("Text", udtClaimRuleResult.RelatedClaimRule.ObjectName)
            If udtClaimRuleResult.dtmDoseDate.HasValue Then
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim udtSubPlatformBLL As New SubPlatformBLL
                'strText = strText.Replace("%date", udtFormatter.formatDate(udtClaimRuleResult.dtmDoseDate.Value))
                strText = strText.Replace("%date", udtFormatter.formatDisplayDate(udtClaimRuleResult.dtmDoseDate.Value, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform)))
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            Else
                strText = strText.Replace("%date", String.Empty)
            End If
        End If
        Return strText
    End Function

#End Region

#Region "Step 2b Confirm Claim detail"

    Protected MustOverride Sub SetupStep2b(ByVal udtEHSAccount As EHSAccountModel, ByVal blnInitAll As Boolean)
    Protected MustOverride Sub Step2bClear()

#End Region

#Region "Step 3 Complete claim"

    Protected MustOverride Sub SetupStep3(ByVal udtEHSAccount As EHSAccountModel, ByVal activeViewChange As Boolean)
    Protected MustOverride Sub Step3Clear()

#End Region

#Region "Step of Select Practice"

    Protected MustOverride Sub SetupSelectPractice()

#End Region

#Region "Step of Internal Error "

    Protected MustOverride Sub SetupInternalError()

#End Region

#Region "Other functions"

    '-----------------------------------------------------------------------------------------------------------------------------
    'Clear Claim for new patient
    '-----------------------------------------------------------------------------------------------------------------------------
    Protected Sub Clear(ByVal strFunctCode As String)
        Me.Step2aClear()
        Me.Step2bClear()
        Me.Step3Clear()
        Me._udtSessionHandler.EHSClaimSessionRemove(strFunctCode)
    End Sub

    Protected Function RuleResultKey(ByVal strActiveViewIndex As String, ByVal enumRuleType As RuleTypeENum) As String
        Return String.Format("{0}_{1}", strActiveViewIndex, enumRuleType)
    End Function

    Protected Sub ClearWarningRules(ByRef udtRuleResults As RuleResultCollection)
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

    Protected Sub ShowChangePracticeButton(ByVal imageButton As Button)
        Dim udtPracticeDisplays As BLL.PracticeDisplayModelCollection = Me._udtSessionHandler.PracticeDisplayListGetFromSession()
        If udtPracticeDisplays.Count > 1 Then
            imageButton.Visible = True
        Else
            imageButton.Visible = False
        End If

    End Sub

    '-----------------------------------------------------------------------------------------------------------------------------
    ' concurrent Browser checking
    '-----------------------------------------------------------------------------------------------------------------------------
    Protected Sub EHSClaimTokenNumAssign(ByVal hfEHSClaimTokenNum As HiddenField, ByVal functCode As String)
        hfEHSClaimTokenNum.Value = Me._udtSessionHandler.EHSClaimTokenNumberSaveToSession(functCode)
    End Sub

    Protected Function EHSClaimTokenNumValidation(ByVal strBrowserToken As String, ByVal functCode As String) As Boolean

        Dim strTurnOnChecking As String = String.Empty
        Dim udtGeneralF As New Common.ComFunction.GeneralFunction

        udtGeneralF.getSystemParameter("ConcurrentBrowserChecking", strTurnOnChecking, String.Empty)

        If strTurnOnChecking.Trim.Equals("N") Then
            Return True
        Else
            If strBrowserToken.Trim().ToString() <> Me._udtSessionHandler.EHSClaimTokenNumberGetFromSession(functCode) Then
                Return False
            Else
                Return True
            End If
        End If

    End Function

    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    ' Obsolete non reference function
    'Public Shared Function CompareVaccince(ByVal udtOrgVaccine As EHSClaimVaccineModel, ByVal udtNewVaccine As EHSClaimVaccineModel) As Boolean
    '    Dim isSame As Boolean = True
    '    Dim udtSubsidizeMatched As EHSClaimVaccineModel.EHSClaimSubsidizeModel = Nothing
    '    Dim udtSubsidizDetailMatched As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel = Nothing

    '    If udtOrgVaccine.SubsidizeList.Count <> udtNewVaccine.SubsidizeList.Count OrElse Not udtOrgVaccine.SchemeCode.Trim().Equals(udtNewVaccine.SchemeCode.Trim) Then
    '        isSame = False
    '    Else
    '        For Each udtOrgSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtOrgVaccine.SubsidizeList

    '            udtSubsidizeMatched = udtNewVaccine.SubsidizeList.Filter(udtOrgSubsidize.SubsidizeCode)
    '            If udtSubsidizeMatched Is Nothing Then
    '                isSame = False
    '                Exit For
    '            Else

    '                If udtSubsidizeMatched.Available = udtOrgSubsidize.Available Then
    '                    If udtOrgSubsidize.SubsidizeDetailList.Count <> udtSubsidizeMatched.SubsidizeDetailList.Count Then
    '                        isSame = False
    '                    Else
    '                        For Each udtOrgSubsidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtOrgSubsidize.SubsidizeDetailList

    '                            udtSubsidizDetailMatched = udtSubsidizeMatched.SubsidizeDetailList.Filter(udtOrgSubsidizeDetail.AvailableItemCode)
    '                            If udtSubsidizDetailMatched Is Nothing Then
    '                                isSame = False
    '                                Exit For
    '                            Else
    '                                If udtSubsidizDetailMatched.Available <> udtOrgSubsidizeDetail.Available Then
    '                                    isSame = False
    '                                    Exit For
    '                                End If
    '                            End If
    '                        Next
    '                    End If

    '                Else
    '                    isSame = False
    '                    Exit For
    '                End If

    '                If isSame = False Then
    '                    Exit For
    '                End If

    '            End If

    '        Next
    '    End If
    '    Return isSame
    'End Function
    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

    Public Shared Function UpdateVaccince(ByVal udtOrgVaccine As EHSClaimVaccineModel, ByVal udtNewVaccine As EHSClaimVaccineModel) As EHSClaimVaccineModel
        Dim isMatch As Boolean = False
        Dim udtSubsidizeMatched As EHSClaimVaccineModel.EHSClaimSubsidizeModel = Nothing
        Dim udtSubsidizDetailMatched As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel = Nothing


        For Each udtOrgSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtOrgVaccine.SubsidizeList

            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            udtSubsidizeMatched = udtNewVaccine.SubsidizeList.Filter(udtOrgSubsidize)
            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

            If Not udtSubsidizeMatched Is Nothing Then

                If udtSubsidizeMatched.Selected Then

                    isMatch = True

                    udtSubsidizeMatched.Selected = udtSubsidizeMatched.Selected

                    For Each udtOrgSubsidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtOrgSubsidize.SubsidizeDetailList

                        udtSubsidizDetailMatched = udtSubsidizeMatched.SubsidizeDetailList.FilterBySubsidizeItemCode(udtOrgSubsidizeDetail.SubsidizeItemCode)
                        If Not udtSubsidizDetailMatched Is Nothing Then
                            udtSubsidizDetailMatched.Selected = udtOrgSubsidizeDetail.Selected
                        End If
                    Next

                End If

            End If
        Next

        If isMatch Then
            Return udtNewVaccine
        Else
            Return Nothing
        End If

    End Function

    '-----------------------------------------------------------------------------------------------------------------------------
    'SmartID Functions
    '-----------------------------------------------------------------------------------------------------------------------------
    Public Shared Function SmartIDCardFaceDataValidation(ByVal udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel) As SystemMessage
        Dim udtSystemMessage As SystemMessage = Nothing
        Dim formatter As Common.Format.Formatter = New Common.Format.Formatter
        Dim validator As Common.Validation.Validator = New Common.Validation.Validator
        Dim isValid As Boolean = True
        Dim udtCommfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
        Dim strSmartIDIssue As String = String.Empty
        '---------------------------------------------------------------------------------------------------------------
        ' Check Identity No
        '---------------------------------------------------------------------------------------------------------------
        udtSystemMessage = validator.chkIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, udtEHSPersonalInformation.IdentityNum, String.Empty)
        If Not udtSystemMessage Is Nothing Then
            isValid = False
            udtSystemMessage = New SystemMessage("990000", "E", "00244")
        End If

        '---------------------------------------------------------------------------------------------------------------
        ' Check Date of Issue
        '---------------------------------------------------------------------------------------------------------------
        If isValid Then

            udtSystemMessage = validator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.HKIC, formatter.formatDOI(DocType.DocTypeModel.DocTypeCode.HKIC, udtEHSPersonalInformation.DateofIssue), udtEHSPersonalInformation.DOB)
            If Not udtSystemMessage Is Nothing Then
                isValid = False
                udtSystemMessage = New SystemMessage("990000", "E", "00245")
            End If
        End If

        If isValid Then
            udtCommfunct.getSystemParameter("SmartIDIssueDate", strSmartIDIssue, String.Empty, "ALL")
            If udtEHSPersonalInformation.DateofIssue < CDate(strSmartIDIssue) Then
                isValid = False
                udtSystemMessage = New SystemMessage("990000", "E", "00245")
            End If
        End If

        '---------------------------------------------------------------------------------------------------------------
        ' Check Date of Brith
        '---------------------------------------------------------------------------------------------------------------
        If isValid Then

            Dim strDOB As String = formatter.formatDOB(udtEHSPersonalInformation.DOB, udtEHSPersonalInformation.ExactDOB, CultureLanguage.English, Nothing, Nothing)
            udtSystemMessage = validator.chkDOB(DocType.DocTypeModel.DocTypeCode.HKIC, strDOB)
            If Not udtSystemMessage Is Nothing Then
                isValid = False
                udtSystemMessage = New SystemMessage("990000", "E", "00246")
            End If
        End If

        Return udtSystemMessage
    End Function

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

    Public Shared Function ParseSearchAccountResult(ByVal udtEHSAccount As EHSAccountModel, ByVal udtSearchAccountStatus As BLL.EHSClaimBLL.SearchAccountStatus) As String
        Select Case udtEHSAccount.AccountSource
            Case EHSAccountModel.SysAccountSource.ValidateAccount
                If Not IsNothing(udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode)) Then Return "Validated account with same doc code found"

                Return "Validated account with different doc code found"

            Case EHSAccountModel.SysAccountSource.TemporaryAccount
                If udtSearchAccountStatus.NotMatchAccountExist Then Return "Temporary account with different details found"

                If udtEHSAccount.IsNew Then Return "No account found"

                Return "Temporary account with same details found"

            Case EHSAccountModel.SysAccountSource.SpecialAccount
                If udtSearchAccountStatus.NotMatchAccountExist Then Return "Special account with different details found"

                Return "Special account with same details found"

        End Select

        Return "Unhandled case"

    End Function

#End Region

#Region "Property"

    Public ReadOnly Property SessionHandler() As BLL.SessionHandler
        Get
            Return Me._udtSessionHandler
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


#End Region

#Region "Page Load"

    'Page Load : LOG00001
    Public Shared Sub AuditLogPageLoad(ByVal udtAuditLogEntry As AuditLogEntry, ByVal selectedPractice As Boolean, ByVal comeFromAccountCreation As Boolean)
        If selectedPractice Then
            udtAuditLogEntry.AddDescripton("Practice Selected", "True")
        Else
            udtAuditLogEntry.AddDescripton("Practice Selected", "False")
        End If

        If comeFromAccountCreation Then
            udtAuditLogEntry.AddDescripton("Come From Account Creation", "True")
        End If

        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00001, "Claim Page Loaded")
    End Sub

#End Region

#Region "Audit Log Search"
    ' CRE11-004
    'Search Account : Search Account Start: LOG00002
    Public Shared Sub AuditLogSearchAccountStart(ByRef udtAuditLogEntry As AuditLogEntry, ByVal strSchemeCode As String, ByVal strDoctype As String, ByVal strHKICSymbol As String, ByVal strDocNo As String, ByVal strDOB As String, ByVal strSearchFrom As String)
        udtAuditLogEntry.AddDescripton("Scheme Code", strSchemeCode)
        udtAuditLogEntry.AddDescripton("Doc Code", strDoctype)
        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        If strDoctype = DocType.DocTypeModel.DocTypeCode.HKIC Then udtAuditLogEntry.AddDescripton("HKIC Symbol", strHKICSymbol)
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]
        udtAuditLogEntry.AddDescripton("Doc No.", strDocNo)
        udtAuditLogEntry.AddDescripton("DOB", strDOB)
        udtAuditLogEntry.AddDescripton("Search From", strSearchFrom)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00002, "Search EHSAccount Start", New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, _
                                                                                                                strDoctype, strDocNo))
    End Sub

    ' INT18-XXX (Refine auditlog) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    'Search Account : Search Account Start: LOG00003
    Public Shared Sub AuditLogSearchAccountInfo(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strSchemeCode As String, ByVal strDoctype As String, ByVal strHKICSymbol As String, ByVal strHKID As String, ByVal strDOB As String, ByVal strAge As String, ByVal strDateOfReg As String)
        udtAuditLogEntry.AddDescripton("Scheme Code", strSchemeCode)
        udtAuditLogEntry.AddDescripton("Search Doc Code", strDoctype)
        If strDoctype = DocType.DocTypeModel.DocTypeCode.HKIC Then udtAuditLogEntry.AddDescripton("HKIC Symbol", strHKICSymbol)
        udtAuditLogEntry.AddDescripton("Doc No.", strHKID)
        udtAuditLogEntry.AddDescripton("DOB", strDOB)
        If Not String.IsNullOrEmpty(strAge) Then
            udtAuditLogEntry.AddDescripton("Age", strAge)
        End If
        If Not String.IsNullOrEmpty(strDateOfReg) Then
            udtAuditLogEntry.AddDescripton("Date Of Reg", strDateOfReg)
        End If
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00003, "Search Information Start", New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, _
                                                                                                                strDoctype, (New Formatter).formatDocumentIdentityNumber(strDoctype, strHKID)))
    End Sub
    ' INT18-XXX (Refine auditlog) [End][Chris YIM]

    ' INT18-XXX (Refine auditlog) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    'Search Account : Search Account Start: LOG00088
    Public Shared Sub AuditLogSearchAccountInfoEnd(ByVal udtAuditLogEntry As AuditLogEntry, ByVal blnValid As Boolean)
        udtAuditLogEntry.AddDescripton("Valid", IIf(blnValid, "True", "False"))
        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00088, "Search Information Complete")
    End Sub
    ' INT18-XXX (Refine auditlog) [End][Chris YIM]

    ' INT18-XXX (Refine auditlog) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    'Search Account : Search Account Start: LOG00089
    Public Shared Sub AuditLogSearchOCSSSStart(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strDoctype As String, ByVal strHKICSymbol As String, ByVal strHKID As String)
        udtAuditLogEntry.AddDescripton("Doc No.", strHKID)
        If strDoctype = DocType.DocTypeModel.DocTypeCode.HKIC Then udtAuditLogEntry.AddDescripton("HKIC Symbol", strHKICSymbol)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00089, "Search OCSSS Start", New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, _
                                                                                                              strDoctype, (New Formatter).formatDocumentIdentityNumber(strDoctype, strHKID)))
    End Sub
    ' INT18-XXX (Refine auditlog) [End][Chris YIM]

    ' INT18-XXX (Refine auditlog) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    'Search Account : Search Account Start: LOG00090
    Public Shared Sub AuditLogSearchOCSSSEnd(ByVal udtAuditLogEntry As AuditLogEntry, ByVal blnValid As Boolean)
        udtAuditLogEntry.AddDescripton("Eligible", IIf(blnValid, "True", "False"))
        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00090, "Search OCSSS Complete")
    End Sub
    ' INT18-XXX (Refine auditlog) [End][Chris YIM]

    'Search Account : Search Account Complete : LOG00004
    Public Shared Sub AuditLogSearchAccountComplete(ByRef udtAuditLogEntry As AuditLogEntry, ByVal strSchemeCode As String, ByVal strSearchDocCode As String, ByVal strHKICSymbol As String, ByVal udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel, ByVal strSearchResult As String)
        udtAuditLogEntry.AddDescripton("Scheme Code", strSchemeCode)
        udtAuditLogEntry.AddDescripton("Search Doc Code", strSearchDocCode)
        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        If strSearchDocCode = DocType.DocTypeModel.DocTypeCode.HKIC Then udtAuditLogEntry.AddDescripton("HKIC Symbol", strHKICSymbol)
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]
        udtAuditLogEntry.AddDescripton("Account Doc Code", udtEHSPersonalInfo.DocCode)
        udtAuditLogEntry.AddDescripton("Doc No.", udtEHSPersonalInfo.IdentityNum)
        udtAuditLogEntry.AddDescripton("DOB", udtEHSPersonalInfo.DOB)
        udtAuditLogEntry.AddDescripton("Search Result", strSearchResult)
        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00004, "Search EHSAccount Complete")
    End Sub

    'Search Account : Search Account Failed : LOG00005

    'Search Account : Validated Account Found: LOG00043
    Public Shared Sub AuditLogValidatedAccounhtFound(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSAccount As EHSAccount.EHSAccountModel)
        udtAuditLogEntry.AddDescripton("VoucherAccID", udtEHSAccount.VoucherAccID)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00043, "Validated Account Found")
    End Sub

    'Search Account : Change Scheme: LOG00045
    Public Shared Sub AuditLogSearchAccountChangeScheme(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strPreviousScheme As String, ByVal strCurrentScheme As String)
        udtAuditLogEntry.AddDescripton("Previous Scheme:", strPreviousScheme)
        udtAuditLogEntry.AddDescripton("Current Scheme:", strCurrentScheme)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00045, "Search Account - Change Scheme")
    End Sub

    'Search Account : Search Account - Select Document Type: LOG00079
    Public Shared Sub AuditLogSearchAccountSelectDocumentType(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strDocumentName As String)
        udtAuditLogEntry.AddDescripton("Document Type:", strDocumentName)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00079, "Search Account - Select Document Type")
    End Sub
#End Region

#Region "Audit Log Step2a"
    'Enter Claim Detail : Enter Claim Detail Loaded : LOG00042
    Public Shared Sub AuditLogEnterClaimDetailLoaded(ByVal udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00042, "Enter Claim Detail")
    End Sub

    'Enter Claim Detail : Start Enter Claim Detail : LOG00010
    Public Shared Sub AuditLogEnterClaimDetailStart(ByRef udtAuditLogEntry As AuditLogEntry, ByVal blnIsConfirmed As Boolean, ByVal udtSmartIDContent As BLL.SmartIDContentModel)
        If blnIsConfirmed Then
            udtAuditLogEntry.AddDescripton("Popup box Confirm", "True")
        Else
            udtAuditLogEntry.AddDescripton("Popup box Confirm", "False")
        End If

        If Not udtSmartIDContent Is Nothing AndAlso udtSmartIDContent.IsReadSmartID Then
            udtAuditLogEntry.AddDescripton("Is Read by Smart ID Case", "True")
        Else
            udtAuditLogEntry.AddDescripton("Is Read by Smart ID Case", "False")
        End If

        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00010, "Enter Claim Detail Start")
    End Sub

    'Enter Claim Detail : Passed Enter Claim Detail : LOG00011
    Public Shared Sub AuditLogEnterClaimDetailPassed(ByRef udtAuditLogEntry As AuditLogEntry, ByVal udtEHSTransaction As EHSTransactionModel, ByVal blnIsConfirmed As Boolean, ByVal isTSWCase As Boolean)
        If blnIsConfirmed Then
            udtAuditLogEntry.AddDescripton("Popup box Confirm", "True")
        Else
            udtAuditLogEntry.AddDescripton("Popup box Confirm", "False")
        End If

        udtAuditLogEntry.AddDescripton("Doc Code", udtEHSTransaction.DocCode)
        udtAuditLogEntry.AddDescripton("Create By", udtEHSTransaction.CreateBy)
        udtAuditLogEntry.AddDescripton("Update By", udtEHSTransaction.UpdateBy)
        udtAuditLogEntry.AddDescripton("Date Entry By", udtEHSTransaction.DataEntryBy)
        udtAuditLogEntry.AddDescripton("Service Date", udtEHSTransaction.ServiceDate)

        If isTSWCase Then
            udtAuditLogEntry.AddDescripton("Is TSW Case", "True")
        Else
            udtAuditLogEntry.AddDescripton("Is TSW Case", "False")
        End If

        Dim enumControlType As SchemeClaimModel.EnumControlType = New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(udtEHSTransaction.SchemeCode.Trim())

        Select Case enumControlType
            Case SchemeClaimModel.EnumControlType.VOUCHER
                udtAuditLogEntry = AuditLogHCVS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.VOUCHERCHINA
                udtAuditLogEntry = AuditLogHCVSChina(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.EVSS
                udtAuditLogEntry = AuditLogEVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.CIVSS
                udtAuditLogEntry = AuditLogCIVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.HSIVSS
                udtAuditLogEntry = AuditLogHSIVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.RVP
                udtAuditLogEntry = AuditLogRVP(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.EHAPP
                udtAuditLogEntry = AuditLogEHAPP(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.PIDVSS
                udtAuditLogEntry = AuditLogPIDVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.VSS
                udtAuditLogEntry = AuditLogVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.ENHVSSO
                udtAuditLogEntry = AuditLogENHVSSO(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.PPP
                udtAuditLogEntry = AuditLogPPP(udtAuditLogEntry, udtEHSTransaction)

                ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
            Case SchemeClaimModel.EnumControlType.SSSCMC
                udtAuditLogEntry = AuditLogSSSCMC(udtAuditLogEntry, udtEHSTransaction)
                ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

                ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
                ' --------------------------------------------------------------------------------------
            Case SchemeClaimModel.EnumControlType.COVID19, SchemeClaimModel.EnumControlType.COVID19CBD, SchemeClaimModel.EnumControlType.COVID19RVP
                udtAuditLogEntry = AuditLogCOVID19(udtAuditLogEntry, udtEHSTransaction)
                ' CRE20-0022 (Immu record) [End][Winnie SUEN]

            Case Else
                Throw New Exception(String.Format("No available input control for scheme({0}).", enumControlType.ToString))

        End Select

        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00011, "Enter Claim Detail Complete")
    End Sub

    ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [Start][Winnie]
    'Enter Claim Detail : Passed Enter Claim Detail : LOG00012
    Public Shared Sub AuditLogShowClaimRulePopupBox(ByRef udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00012, "Show Claim Rule/Eligibility Popup Box")
    End Sub

    Public Shared Sub AuditLogShowDuplicateClaimAlert(ByVal udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00085, "Show Duplicate Claim Alert")
    End Sub

    Public Shared Sub AuditLogDuplicateClaimAlertProceedClick(ByVal udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00086, "Duplicate Claim Alert - Proceed Click")
    End Sub

    Public Shared Sub AuditLogDuplicateClaimAlertNotProceedClick(ByVal udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00087, "Duplicate Claim Alert - Not proceed Click")
    End Sub

    Public Shared Sub AuditLogTextOnlyVersionShowDuplicateClaimAlert(ByVal udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00089, "Show Duplicate Claim Alert")
    End Sub

    Public Shared Sub AuditLogTextOnlyVersionDuplicateClaimAlertProceedClick(ByVal udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00090, "Duplicate Claim Alert - Proceed Click")
    End Sub

    Public Shared Sub AuditLogTextOnlyVersionDuplicateClaimAlertBackClick(ByVal udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00091, "Duplicate Claim Alert - Back Click")
    End Sub
    ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [End][Winnie]

    'Enter Claim Detail : Enter Claim Detail Failed : LOG00013

    'Enter Claim Detail : Change Scheme: LOG00046
    Public Shared Sub AuditLogEnterClaimDetailChangeScheme(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strPreviousScheme As String, ByVal strCurrentScheme As String)
        udtAuditLogEntry.AddDescripton("Previous Scheme:", strPreviousScheme)
        udtAuditLogEntry.AddDescripton("Current Scheme:", strCurrentScheme)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00046, "Enter Claim Detail - Change Scheme")

    End Sub

    ' Open RCH Window : LOG00076
    Public Shared Sub AuditLogOpenRCHWindow(ByVal udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteLog(LogID.LOG00076, "Open RCH Window")
    End Sub

    ' Search RCH : LOG00077
    Public Shared Sub AuditLogSelectRCH(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strRCHCode As String)
        udtAuditLogEntry.AddDescripton("RCH Code", strRCHCode)
        udtAuditLogEntry.WriteLog(LogID.LOG00077, "Select RCH")
    End Sub

    ' Cancel Search RCH : LOG00078
    Public Shared Sub AuditLogCancelSearchRCH(ByVal udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteLog(LogID.LOG00078, "Cancel Search RCH")
    End Sub


    'Enter Claim Detail - Cancel - Yes Click : LOG00083
    Public Shared Sub AuditLogEnterClaimDetailCancelYes(ByVal udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00083, "Enter Claim Detail - Cancel - Yes Click")
    End Sub

    Public Shared Sub AuditLogTextOnlyVersionEnterServiceDateCancelClick(ByVal udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00084, "Enter Service Date - Cancel Click")
    End Sub

    Public Shared Sub AuditLogTextOnlyVersionConfirmBoxConfirmClick(ByVal udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00085, "Confirm Box - Confirm Click")
    End Sub

    Public Shared Sub AuditLogTextOnlyVersionConfirmBoxBackClick(ByVal udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00086, "Confirm Box - Back Click")
    End Sub

    Public Shared Sub AuditLogTextOnlyVersionEnterServiceDateNextClick(ByVal udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00087, "Enter Service Date - Next Click")
    End Sub
#End Region

#Region "Audit Log Step2b"
    'Enter Claim Detail : Confirm Claim Detail Start : LOG00014
    Public Shared Sub AuditLogConfirmClaimDetailStart(ByRef udtAuditLogEntry As AuditLogEntry, ByVal udtSmartIDContent As BLL.SmartIDContentModel)
        If Not udtSmartIDContent Is Nothing AndAlso udtSmartIDContent.IsReadSmartID Then
            udtAuditLogEntry.AddDescripton("Is Read by Smart ID Case", "True")
        Else
            udtAuditLogEntry.AddDescripton("Is Read by Smart ID Case", "False")
        End If

        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00014, "Confirm Claim Detail Start")
    End Sub

    'Confirm Claim Detail : Confirm Claim Detail Passed : LOG00015
    Public Shared Sub AuditLogConfirmClaimDetailPassed(ByRef udtAuditLogEntry As AuditLogEntry, ByVal strFuncCode As String, ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtSmartIDContent As BLL.SmartIDContentModel, ByVal blnCreateAdment As Boolean)

        udtAuditLogEntry.AddDescripton("Doc Code", udtEHSTransaction.DocCode)
        udtAuditLogEntry.AddDescripton("Create By", udtEHSTransaction.CreateBy)
        udtAuditLogEntry.AddDescripton("Update By", udtEHSTransaction.UpdateBy)
        udtAuditLogEntry.AddDescripton("Date Entry By", udtEHSTransaction.DataEntryBy)

        If udtEHSTransaction.CreateBySmartID Then
            udtAuditLogEntry.AddDescripton("Is Read by Smart ID Case", "True")
            udtAuditLogEntry.AddDescripton("Smart ID Read Status", udtSmartIDContent.SmartIDReadStatus.ToString())
            If blnCreateAdment Then
                udtAuditLogEntry.AddDescripton("Create Admentment Account", "True")
            Else
                udtAuditLogEntry.AddDescripton("Create Admentment Account", "True")
            End If
        Else
            udtAuditLogEntry.AddDescripton("Is Read by Smart ID Case", "False")
        End If

        If udtEHSTransaction.PrintedConsentForm Then
            udtAuditLogEntry.AddDescripton("Printed Consent Form", "True")
        Else
            udtAuditLogEntry.AddDescripton("Printed Consent Form", "False")
        End If

        udtAuditLogEntry.AddDescripton("Service Date", udtEHSTransaction.ServiceDate)

        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Dim enumControlType As SchemeClaimModel.EnumControlType = New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(udtEHSTransaction.SchemeCode.Trim())

        Select Case enumControlType
            Case SchemeClaimModel.EnumControlType.VOUCHER
                udtAuditLogEntry = AuditLogHCVS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.VOUCHERCHINA
                udtAuditLogEntry = AuditLogHCVSChina(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.EVSS
                udtAuditLogEntry = AuditLogEVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.CIVSS
                udtAuditLogEntry = AuditLogCIVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.HSIVSS
                udtAuditLogEntry = AuditLogHSIVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.RVP
                udtAuditLogEntry = AuditLogRVP(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.EHAPP
                udtAuditLogEntry = AuditLogEHAPP(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.PIDVSS
                udtAuditLogEntry = AuditLogPIDVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.VSS
                udtAuditLogEntry = AuditLogVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.ENHVSSO
                udtAuditLogEntry = AuditLogENHVSSO(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.PPP
                udtAuditLogEntry = AuditLogPPP(udtAuditLogEntry, udtEHSTransaction)

                ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
            Case SchemeClaimModel.EnumControlType.SSSCMC
                udtAuditLogEntry = AuditLogSSSCMC(udtAuditLogEntry, udtEHSTransaction)
                ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

                ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
                ' --------------------------------------------------------------------------------------
            Case SchemeClaimModel.EnumControlType.COVID19, SchemeClaimModel.EnumControlType.COVID19CBD, SchemeClaimModel.EnumControlType.COVID19RVP
                udtAuditLogEntry = AuditLogCOVID19(udtAuditLogEntry, udtEHSTransaction)
                ' CRE20-0022 (Immu record) [End][Winnie SUEN]

            Case Else
                Throw New Exception(String.Format("No available input control for scheme({0}).", enumControlType.ToString))

        End Select
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00015, "Confirm Claim Detail Complete")
    End Sub

    'Confirm Claim Detail : Confirm Claim Failed : LOG00016
    Public Shared Sub AuditLogConfirmClaimDetailFailed(ByRef udtAuditLogEntry As AuditLogEntry, ByVal strFuncCode As String)
        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00016, "Confirm Claim Detail Failed")
    End Sub

#End Region

#Region "Complete Claim"

    'Complete Claim : Complete Claim : LOG00017
    Public Shared Sub AuditLogCompleteClaim(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSTransaction As EHSTransactionModel)
        udtAuditLogEntry.AddDescripton("Transaction No", udtEHSTransaction.TransactionID)
        udtAuditLogEntry.AddDescripton("Transaction Date", udtEHSTransaction.TransactionDtm)
        udtAuditLogEntry.AddDescripton("Doc Code", udtEHSTransaction.DocCode)
        udtAuditLogEntry.AddDescripton("Create By", udtEHSTransaction.CreateBy)
        udtAuditLogEntry.AddDescripton("Update By", udtEHSTransaction.UpdateBy)
        udtAuditLogEntry.AddDescripton("Date Entry By", udtEHSTransaction.DataEntryBy)
        udtAuditLogEntry.AddDescripton("Service Date", udtEHSTransaction.ServiceDate)
        If udtEHSTransaction.CreateBySmartID Then
            udtAuditLogEntry.AddDescripton("Is Read by Smart ID Case", "True")
        Else
            udtAuditLogEntry.AddDescripton("Is Read by Smart ID Case", "False")
        End If

        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Dim enumControlType As SchemeClaimModel.EnumControlType = New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(udtEHSTransaction.SchemeCode.Trim())

        Select Case enumControlType
            Case SchemeClaimModel.EnumControlType.VOUCHER
                udtAuditLogEntry = AuditLogHCVS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.VOUCHERCHINA
                udtAuditLogEntry = AuditLogHCVSChina(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.EVSS
                udtAuditLogEntry = AuditLogEVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.CIVSS
                udtAuditLogEntry = AuditLogCIVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.HSIVSS
                udtAuditLogEntry = AuditLogHSIVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.RVP
                udtAuditLogEntry = AuditLogRVP(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.EHAPP
                udtAuditLogEntry = AuditLogEHAPP(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.PIDVSS
                udtAuditLogEntry = AuditLogPIDVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.VSS
                udtAuditLogEntry = AuditLogVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.ENHVSSO
                udtAuditLogEntry = AuditLogENHVSSO(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.PPP
                udtAuditLogEntry = AuditLogPPP(udtAuditLogEntry, udtEHSTransaction)

                ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
            Case SchemeClaimModel.EnumControlType.SSSCMC
                udtAuditLogEntry = AuditLogSSSCMC(udtAuditLogEntry, udtEHSTransaction)
                ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

                ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
                ' --------------------------------------------------------------------------------------
            Case SchemeClaimModel.EnumControlType.COVID19, SchemeClaimModel.EnumControlType.COVID19CBD, SchemeClaimModel.EnumControlType.COVID19RVP
                udtAuditLogEntry = AuditLogCOVID19(udtAuditLogEntry, udtEHSTransaction)
                ' CRE20-0022 (Immu record) [End][Winnie SUEN]

            Case Else
                Throw New Exception(String.Format("No available input control for scheme({0}).", enumControlType.ToString))

        End Select
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        'CRE15-003 System-generated Form [Start][Philip Chau]
        Dim udtEHSClaimBLL As EHSClaimBLL = New EHSClaimBLL
        Dim _udtSessionHandler As SessionHandler = New SessionHandler
        Dim strPrefixStatus As String = String.Empty
        Dim strTransactionIDPrefix As String = _udtSessionHandler.EHSClaimTempTransactionIDGetFromSession()
        If udtEHSTransaction.PrintedConsentForm And Not String.IsNullOrEmpty(strTransactionIDPrefix) Then
            If udtEHSClaimBLL.chkIsPrefixAndTransactionIDTheSame(_udtSessionHandler.EHSClaimTempTransactionIDGetFromSession(), udtEHSTransaction) Then
                strPrefixStatus = Common.Component.YesNo.No
            Else
                strPrefixStatus = Common.Component.YesNo.Yes
            End If
            udtAuditLogEntry.AddDescripton("Consent Form Transaction No. Prefix Outdated", strPrefixStatus)
        End If
        'CRE15-003 System-generated Form [End][Philip Chau]

        ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [Start][Winnie]
        If _udtSessionHandler.NoticedDuplicateClaimAlertGetFromSession = YesNo.Yes Then
            udtAuditLogEntry.AddDescripton("Confirmed Duplicate Claim Alert", "True")
        Else
            udtAuditLogEntry.AddDescripton("Confirmed Duplicate Claim Alert", "False")
        End If
        ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [End][Winnie]

        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00017, "Complete Claim")
    End Sub

    'Complete Claim : Claim for Same patient : LOG00018
    Public Shared Sub AuditLogClaimForSamePatient(ByVal udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00018, "Claim For Same Patient")
    End Sub

    'Complete Claim : Next Claim : LOG00044
    Public Shared Sub AuditLogNextClaim(ByVal udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00044, "Next Claim")
    End Sub

#End Region

#Region "Selected Practice"

    'Selected Practice : LOG00019
    Public Shared Sub AuditLogPracticeSelected(ByVal udtAuditLogEntry As AuditLogEntry, ByVal isPopupBoxSelection As Boolean, ByVal udtPracticeDisplay As BLL.PracticeDisplayModel, ByVal udtSchemeClaim As SchemeClaimModel, ByVal SchemeAvailForEHSAccount As Boolean)
        If isPopupBoxSelection Then
            udtAuditLogEntry.AddDescripton("Popup Box Selection", "True")

        Else
            udtAuditLogEntry.AddDescripton("Popup Box Selection", "False")
        End If

        'Scheme Changed
        If Not udtSchemeClaim Is Nothing Then
            udtAuditLogEntry.AddDescripton("Scheme Changed", "True")
            udtAuditLogEntry.AddDescripton("Scheme Code", udtSchemeClaim.SchemeCode)
            udtAuditLogEntry.AddDescripton("Scheme Desc", udtSchemeClaim.SchemeDesc)
        Else
            udtAuditLogEntry.AddDescripton("Scheme Changed", "False")
            udtAuditLogEntry.AddDescripton("Scheme Code", "N/A")
            udtAuditLogEntry.AddDescripton("Scheme Desc", "N/A")
        End If

        If SchemeAvailForEHSAccount Then
            udtAuditLogEntry.AddDescripton("Available Scheme for current EHSAccount", "True")
        Else
            udtAuditLogEntry.AddDescripton("Available Scheme for current EHSAccount", "False")
        End If

        udtAuditLogEntry.AddDescripton("Practice Name", udtPracticeDisplay.PracticeName)
        udtAuditLogEntry.AddDescripton("Bank Account No.", udtPracticeDisplay.BankAccountNo)
        udtAuditLogEntry.AddDescripton("Practice ID", udtPracticeDisplay.PracticeID)

        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00019, "Claim Practice Selected")
    End Sub

#End Region

#Region "Select Print Option "

    'Select Print Option : LOG00020
    Public Shared Sub AuditLogSelectPrintOption(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strCurrentPrintOption As String)
        udtAuditLogEntry.AddDescripton("Current Print Option", strCurrentPrintOption)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00020, "Select Print Option")
    End Sub

    'Select Print Option : Complete : LOG00021
    Public Shared Sub AuditLogPrintOptionSelected(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strSelectedPrintOption As String)
        udtAuditLogEntry.AddDescripton("Selected Print Option", strSelectedPrintOption)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00021, "Print Option Selected")
    End Sub

    'Select Print Option : Fail : LOG00022
#End Region

#Region "Print Form"

    'Print Form : Printed: LOG00023
    Public Shared Sub AuditLogPrintFrom(ByVal udtAuditLogEntry As AuditLogEntry, ByVal isAdHocPrint As Boolean, ByVal strPrintFormURL As String, ByVal strLanguage As String, ByVal strTransactionIDPrefix As String)

        If isAdHocPrint Then
            udtAuditLogEntry.AddDescripton("Adhoc Print", "True")
        Else
            udtAuditLogEntry.AddDescripton("Adhoc Print", "False")
        End If

        udtAuditLogEntry.AddDescripton("Form URL", strPrintFormURL)
        udtAuditLogEntry.AddDescripton("Language", strLanguage)

        'CRE15-003 System-generated Form [Start][Philip Chau]
        If Not String.IsNullOrEmpty(strTransactionIDPrefix) Then
            udtAuditLogEntry.AddDescripton("Transaction No. Prefix", strTransactionIDPrefix)
        End If
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00023, "Form Printed")
        'CRE15-003 System-generated Form [End][Philip Chau]
    End Sub

    ' CRE11-021 log the missed essential information [Start]
    ' -----------------------------------------------------------------------------------------
    'Print Form : Adhoc Print Click : LOG00080
    Public Shared Sub AuditLogAdhocPrintClick(ByVal udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00080, "Adhoc Print Click")
    End Sub

    'Print Form : Adhoc Print Cancel Click : LOG00081
    Public Shared Sub AuditLogAdhocPrintCancelClick(ByVal udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00081, "Adhoc Print Cancel Click")
    End Sub
    ' CRE11-021 log the missed essential information [End]

#End Region

    '=========================================================================Audit Log for smart ID=========================================================================
#Region "Read Smart IC"

    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    'Search Account : Read Samrt ID: LOG00047
    Public Shared Sub AuditLogReadSamrtID(ByRef udtAuditLogEntry As AuditLogEntry, ByVal strSchemeCode As String, ByVal strIdeasVersion As String, ByVal blnIsNewSmartIC As Nullable(Of Boolean))
        Dim strNewSmartIC As String = String.Empty

        If Not blnIsNewSmartIC Is Nothing Then
            strNewSmartIC = IIf(blnIsNewSmartIC, YesNo.Yes, YesNo.No)
        End If

        If Not IsNothing(strSchemeCode) Then udtAuditLogEntry.AddDescripton("Scheme Code", strSchemeCode)
        udtAuditLogEntry.AddDescripton("New Card", strNewSmartIC)
        udtAuditLogEntry.AddDescripton("IDEAS Version", strIdeasVersion)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00047, "Click 'Read and Search Card' and Token Request")
    End Sub
    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    'Search Account : Connect Ideas Complelet: LOG00048
    'Public Shared Sub AuditLogConnectIdeasComplete(ByRef udtAuditLogEntry As AuditLogEntry, ByVal strSchemeCode As String, ByVal ideasTokenResponse As IdeasRM.TokenResponse, ByVal strDemoVersion As String)
    Public Shared Sub AuditLogConnectIdeasComplete(ByRef udtAuditLogEntry As AuditLogEntry, ByVal strSchemeCode As String, ByVal ideasTokenResponse As IdeasRM.TokenResponse, ByVal strDemoVersion As String, ByVal strIdeasVersion As String)
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
        If Not IsNothing(strSchemeCode) Then udtAuditLogEntry.AddDescripton("Scheme Code", strSchemeCode)
        udtAuditLogEntry.AddDescripton("Ideas Artifact Receiver URL", ideasTokenResponse.IdeasArtifactReceiverURL)
        udtAuditLogEntry.AddDescripton("Ideas MAURL", ideasTokenResponse.IdeasMAURL)
        udtAuditLogEntry.AddDescripton("Demo Version", strDemoVersion)
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        udtAuditLogEntry.AddDescripton("IDEAS Version", strIdeasVersion)
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00048, "Click 'Read and Search Card' and Token Request Complete")
    End Sub

    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Shared Sub AuditLogConnectIdeasComboComplete(ByRef udtAuditLogEntry As AuditLogEntry, ByVal strSchemeCode As String, ByVal ideasTokenResponse As IdeasRM.TokenResponse, ByVal strDemoVersion As String, ByVal strIdeasVersion As String)
        If Not IsNothing(strSchemeCode) Then udtAuditLogEntry.AddDescripton("Scheme Code", strSchemeCode)
        udtAuditLogEntry.AddDescripton("Ideas Broker URL", ideasTokenResponse.BrokerURL)
        udtAuditLogEntry.AddDescripton("Demo Version", strDemoVersion)
        udtAuditLogEntry.AddDescripton("IDEAS Version", strIdeasVersion)
        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00048, "Click 'Read and Search Card' and Token Request Complete")
    End Sub
    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    'Search Account : Connect Ideas Fail: LOG00049
    'Public Shared Sub AuditLogConnectIdeasFail(ByRef udtAuditLogEntry As AuditLogEntry, ByVal strSchemeCode As String, ByVal ideasTokenResponse As IdeasRM.TokenResponse, ByVal strDemoVersion As String)
    Public Shared Sub AuditLogConnectIdeasFail(ByRef udtAuditLogEntry As AuditLogEntry, ByVal strSchemeCode As String, ByVal ideasTokenResponse As IdeasRM.TokenResponse, ByVal strDemoVersion As String, ByVal strIdeasVersion As String)
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
        udtAuditLogEntry.AddDescripton("Ideas Error Code", ideasTokenResponse.ErrorCode)
        udtAuditLogEntry.AddDescripton("Ideas Error Detail", ideasTokenResponse.ErrorDetail)
        udtAuditLogEntry.AddDescripton("Ideas Error Message", ideasTokenResponse.ErrorMessage)
        udtAuditLogEntry.AddDescripton("Demo Version", strDemoVersion)
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        udtAuditLogEntry.AddDescripton("IDEAS Version", strIdeasVersion)
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
        'udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00049, "Click 'Read and Search Card' and Token Request Fail")
    End Sub

    Public Shared Sub AuditLogTickSmartIDDeclarationBox(ByVal udtAuditLogEntry As AuditLogEntry, ByVal blnChecked As Boolean)
        If blnChecked Then
            udtAuditLogEntry.AddDescripton("Checked", "Y")
        Else
            udtAuditLogEntry.AddDescripton("Checked", "N")
        End If

        If udtAuditLogEntry.FunctionCode.Trim.Equals(Common.Component.FunctCode.FUNT020401) Then
            ' Function: eHealth Account Rectification
            udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00055, "Tick Smart IC Declaration")
        Else
            ' Function:  Full version Claim / Text only version Claim
            udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00060, "Tick Smart IC Declaration")

        End If

    End Sub

#End Region

#Region "Redirect From IDEAS"

    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    'From IDEAS  : Redirect from IDEAS : LOG00064
    'Public Shared Sub AuditLogRedirectFormIDEAS(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strSchemeCode As String)
    Public Shared Sub AuditLogRedirectFormIDEAS(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strSchemeCode As String, ByVal strIdeasVersion As String)
        udtAuditLogEntry.AddDescripton("Scheme Code", strSchemeCode)
        udtAuditLogEntry.AddDescripton("IDEAS Version", strIdeasVersion)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00064, "Redirect from IDEAS after Token Request")
    End Sub
    ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

#End Region

#Region "Search & validate account with CFD"

    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Public Shared Sub AuditLogSearchNvaliatedACwithCFD(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strSchemeCode As String, ByVal strHKIC As String, ByVal strError As String, ByVal strIdeasVersion As String, ByVal strSearchFrom As String)
        udtAuditLogEntry.AddDescripton("Scheme Code", strSchemeCode)
        udtAuditLogEntry.AddDescripton("HKIC No.", strHKIC)
        udtAuditLogEntry.AddDescripton("Error", strError)
        udtAuditLogEntry.AddDescripton("IDEAS Version", strIdeasVersion)
        udtAuditLogEntry.AddDescripton("Search From", strSearchFrom)
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00050, "Search & validate account with CFD", New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocType.DocTypeModel.DocTypeCode.HKIC, (New Formatter).formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, strHKIC)))
    End Sub

    'From IDEAS  : Redirect from IDEAS Complete : LOG00051
    Public Shared Sub AuditLogSearchNvaliatedACwithCFDComplete(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strSchemeCode As String, ByVal udtSmartIDContent As BLL.SmartIDContentModel, ByVal blnGoToCreation As Boolean)
        udtAuditLogEntry.AddDescripton("Scheme Code", strSchemeCode)

        If blnGoToCreation Then
            udtAuditLogEntry.AddDescripton("Go to Account Creation", "True")
        Else
            udtAuditLogEntry.AddDescripton("Go to Account Creation", "False")
        End If

        udtAuditLogEntry.AddDescripton("Smart IC Type", udtSmartIDContent.SmartIDReadStatus.ToString())

        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        udtAuditLogEntry.AddDescripton("IDEAS Version", IdeasBLL.ConvertIdeasVersion(udtSmartIDContent.IdeasVersion))
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

        udtAuditLogEntry.AddDescripton("CFD", "->")
        EHSAccountCreationBase.AuditLogHKIC(udtAuditLogEntry, udtSmartIDContent.EHSAccount)

        If Not udtSmartIDContent.EHSValidatedAccount Is Nothing Then
            udtAuditLogEntry.AddDescripton("Validated EHS Account", "->")
            EHSAccountCreationBase.AuditLogHKIC(udtAuditLogEntry, udtSmartIDContent.EHSValidatedAccount)
        End If

        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00051, "Search & validate account with CFD Complete")
    End Sub

    'From IDEAS  : Redirect from IDEAS Fail : LOG00052
    Public Shared Sub AuditLogSearchNvaliatedACwithCFDFail(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strSchemeCode As String, ByVal strStepLocation As String, ByVal strErrorCode As String, ByVal strErrorMessage As String)
        udtAuditLogEntry.AddDescripton("Scheme Code", strSchemeCode)
        udtAuditLogEntry.AddDescripton("Steps Location", strStepLocation)
        udtAuditLogEntry.AddDescripton("Error Code", strErrorCode)
        udtAuditLogEntry.AddDescripton("Error Message", strErrorMessage)

        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00052, "Search & validate account with CFD Fail")
    End Sub

#End Region

#Region "Get CFD"

    'Get CFD : Start : 00061
    Public Shared Sub AuditLogGetCFD(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strArtifact As String)
        udtAuditLogEntry.AddDescripton("Artifact", strArtifact)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00061, "Get CFD")
    End Sub

    'Get CFD Complete: 00062
    Public Shared Sub AuditLogGetCFDComplete(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strArtifact As String)
        udtAuditLogEntry.AddDescripton("Artifact", strArtifact)
        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00062, "Get CFD Complete")
    End Sub

    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    'Get CFD Fail: 00063
    'Public Shared Sub AuditLogGetCFDFail(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strArtifact As String, ByVal strErrorCode As String, ByVal strErrorMsg As String)
    Public Shared Sub AuditLogGetCFDFail(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strArtifact As String, ByVal strErrorCode As String, ByVal strErrorMsg As String, ByVal strIdeasVersion As String)
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
        udtAuditLogEntry.AddDescripton("Artifact", strArtifact)
        udtAuditLogEntry.AddDescripton("Error Code", strErrorCode)
        udtAuditLogEntry.AddDescripton("Error Message", strErrorMsg)
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        udtAuditLogEntry.AddDescripton("IDEAS Version", strIdeasVersion)
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
        'udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00063, "Get CFD Fail")
    End Sub

#End Region

    '==================================================================================================================================================

    ' 00065, 00066
    ' Change Service Date
    Public Shared Sub AuditLogChangeServiceDateStart(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strServiceDate As String)
        udtAuditLogEntry.AddDescripton("InputServiceDate", strServiceDate)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00065, "Change ServiceDate Start")
    End Sub

    Public Shared Sub AuditLogChangeServiceDateEnd(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strServiceDate As String)
        udtAuditLogEntry.AddDescripton("ServiceDateAtEnd", strServiceDate)
        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00067, "Change ServiceDate End")
    End Sub

#Region "Scheme Audit Log"
    'HCVS
    Public Shared Function AuditLogHCVS(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSTransaction As EHSTransactionModel) As AuditLogEntry
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        udtAuditLogEntry.AddDescripton("Scheme", udtEHSTransaction.SchemeCode.Trim)
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        udtAuditLogEntry.AddDescripton("VoucherRedeem", udtEHSTransaction.VoucherClaim)

        For Each udtTransactAdditionfield As TransactionAdditionalFieldModel In udtEHSTransaction.TransactionAdditionFields
            udtAuditLogEntry.AddDescripton(udtTransactAdditionfield.AdditionalFieldID, udtTransactAdditionfield.AdditionalFieldValueCode)
        Next

        Return udtAuditLogEntry
    End Function
    'CRE13-019-02 Extend Voucher to China [Start][Karl]    
    Public Shared Function AuditLogHCVSChina(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSTransaction As EHSTransactionModel) As AuditLogEntry
        Dim udtExchangeRate As New ExchangeRate.ExchangeRateBLL
        Dim udtSchemeClaim As SchemeClaimModel
        Dim udtSchemeClaimBLL As SchemeClaimBLL = New SchemeClaimBLL()
        Dim dblSubsidizeFee As Double

        udtSchemeClaim = udtSchemeClaimBLL.getAllDistinctSchemeClaim_WithEffectiveSubsidizeGroup(udtEHSTransaction.ServiceDate).Filter(udtEHSTransaction.SchemeCode)
        If Not udtSchemeClaim Is Nothing Then
            dblSubsidizeFee = (udtSchemeClaim.SubsidizeGroupClaimList.Filter(udtEHSTransaction.ServiceDate))(0).SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVoucher, udtEHSTransaction.ServiceDate).SubsidizeFee
        End If

        udtAuditLogEntry.AddDescripton("Scheme", udtEHSTransaction.SchemeCode.Trim)

        udtAuditLogEntry.AddDescripton("VoucherRedeem", udtEHSTransaction.VoucherClaim)
        udtAuditLogEntry.AddDescripton("ExchangeRate", udtEHSTransaction.ExchangeRate)

        udtAuditLogEntry.AddDescripton("VoucherRedeemHKD", udtEHSTransaction.VoucherClaim * dblSubsidizeFee)
        udtAuditLogEntry.AddDescripton("VoucherRedeemRMB", udtEHSTransaction.VoucherClaimRMB * dblSubsidizeFee)

        For Each udtTransactAdditionfield As TransactionAdditionalFieldModel In udtEHSTransaction.TransactionAdditionFields
            udtAuditLogEntry.AddDescripton(udtTransactAdditionfield.AdditionalFieldID, udtTransactAdditionfield.AdditionalFieldValueCode)
        Next

        Return udtAuditLogEntry
    End Function
    'CRE13-019-02 Extend Voucher to China [End][Karl]
    'EVSS
    Public Shared Function AuditLogEVSS(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSTransaction As EHSTransactionModel) As AuditLogEntry
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        udtAuditLogEntry.AddDescripton("Scheme", udtEHSTransaction.SchemeCode.Trim)
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        For Each udtEHSTransactionDetail As TransactionDetailModel In udtEHSTransaction.TransactionDetails
            udtAuditLogEntry.AddDescripton("Subsidize Code", udtEHSTransactionDetail.SubsidizeCode.Trim)
            udtAuditLogEntry.AddDescripton("Total Amount", udtEHSTransactionDetail.TotalAmount)
        Next

        Return udtAuditLogEntry
    End Function

    'CIVSS
    Public Shared Function AuditLogCIVSS(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSTransaction As EHSTransactionModel) As AuditLogEntry
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        udtAuditLogEntry.AddDescripton("Scheme", udtEHSTransaction.SchemeCode.Trim)
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        For Each udtEHSTransactionDetail As TransactionDetailModel In udtEHSTransaction.TransactionDetails
            udtAuditLogEntry.AddDescripton("Subsidize Code", udtEHSTransactionDetail.SubsidizeCode.Trim)
            udtAuditLogEntry.AddDescripton("Total Amount", udtEHSTransactionDetail.TotalAmount)
        Next

        Return udtAuditLogEntry
    End Function

    'HSIVIVSS
    Public Shared Function AuditLogHSIVSS(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSTransaction As EHSTransactionModel) As AuditLogEntry
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        udtAuditLogEntry.AddDescripton("Scheme", udtEHSTransaction.SchemeCode.Trim)
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        For Each udtEHSTransactionDetail As TransactionDetailModel In udtEHSTransaction.TransactionDetails
            udtAuditLogEntry.AddDescripton("Subsidize Code", udtEHSTransactionDetail.SubsidizeCode.Trim)
            udtAuditLogEntry.AddDescripton("Total Amount", udtEHSTransactionDetail.TotalAmount)
        Next

        For Each udtTransactAdditionfield As TransactionAdditionalFieldModel In udtEHSTransaction.TransactionAdditionFields
            udtAuditLogEntry.AddDescripton("Pre Condition", udtTransactAdditionfield.AdditionalFieldID)
            udtAuditLogEntry.AddDescripton("Value Code", udtTransactAdditionfield.AdditionalFieldValueCode)
            udtAuditLogEntry.AddDescripton("Value Desc", udtTransactAdditionfield.AdditionalFieldValueDesc)
        Next

        Return udtAuditLogEntry
    End Function

    'RVP
    Public Shared Function AuditLogRVP(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSTransaction As EHSTransactionModel) As AuditLogEntry
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        udtAuditLogEntry.AddDescripton("Scheme", udtEHSTransaction.SchemeCode.Trim)
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        For Each udtEHSTransactionDetail As TransactionDetailModel In udtEHSTransaction.TransactionDetails
            udtAuditLogEntry.AddDescripton("Subsidize Code", udtEHSTransactionDetail.SubsidizeCode.Trim)
            udtAuditLogEntry.AddDescripton("Total Amount", udtEHSTransactionDetail.TotalAmount)
        Next

        For Each udtTransactAdditionfield As TransactionAdditionalFieldModel In udtEHSTransaction.TransactionAdditionFields
            udtAuditLogEntry.AddDescripton("RHC Code", udtTransactAdditionfield.AdditionalFieldID)
            udtAuditLogEntry.AddDescripton("Value Code", udtTransactAdditionfield.AdditionalFieldValueCode)
            udtAuditLogEntry.AddDescripton("Value Desc", udtTransactAdditionfield.AdditionalFieldValueDesc)
        Next

        Return udtAuditLogEntry
    End Function

    ' CRE13-001 - EHAPP [Start][Tommy L]
    ' -------------------------------------------------------------------------------------
    'EHAPP
    Public Shared Function AuditLogEHAPP(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSTransaction As EHSTransactionModel) As AuditLogEntry
        udtAuditLogEntry.AddDescripton("Scheme", udtEHSTransaction.SchemeCode.Trim)
        udtAuditLogEntry.AddDescripton("Subsidize Code", udtEHSTransaction.TransactionDetails(0).SubsidizeCode.Trim)
        udtAuditLogEntry.AddDescripton(udtEHSTransaction.TransactionAdditionFields(0).AdditionalFieldID, udtEHSTransaction.TransactionAdditionFields(0).AdditionalFieldValueCode)

        Return udtAuditLogEntry
    End Function
    ' CRE13-001 - EHAPP [End][Tommy L]

    'CRE15-005-03 (New PIDVSS scheme) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    'PIDVSS
    Public Shared Function AuditLogPIDVSS(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSTransaction As EHSTransactionModel) As AuditLogEntry
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        udtAuditLogEntry.AddDescripton("Scheme", udtEHSTransaction.SchemeCode.Trim)
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        For Each udtEHSTransactionDetail As TransactionDetailModel In udtEHSTransaction.TransactionDetails
            udtAuditLogEntry.AddDescripton("Subsidize Code", udtEHSTransactionDetail.SubsidizeCode.Trim)
            udtAuditLogEntry.AddDescripton("Total Amount", udtEHSTransactionDetail.TotalAmount)
        Next

        If Not udtEHSTransaction Is Nothing AndAlso Not udtEHSTransaction.TransactionAdditionFields Is Nothing Then
            For Each udtTransactAdditionfield As TransactionAdditionalFieldModel In udtEHSTransaction.TransactionAdditionFields
                udtAuditLogEntry.AddDescripton(udtTransactAdditionfield.AdditionalFieldID, udtTransactAdditionfield.AdditionalFieldValueCode)
            Next
        End If

        Return udtAuditLogEntry
    End Function
    'CRE15-005-03 (New PIDVSS scheme) [End][Chris YIM]

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    'VSS
    Public Shared Function AuditLogVSS(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSTransaction As EHSTransactionModel) As AuditLogEntry
        udtAuditLogEntry.AddDescripton("Scheme", udtEHSTransaction.SchemeCode.Trim)
        udtAuditLogEntry.AddDescripton("Category", udtEHSTransaction.CategoryCode)

        For Each udtEHSTransactionDetail As TransactionDetailModel In udtEHSTransaction.TransactionDetails
            udtAuditLogEntry.AddDescripton("Subsidize Code", udtEHSTransactionDetail.SubsidizeCode.Trim)
            udtAuditLogEntry.AddDescripton("Total Amount", udtEHSTransactionDetail.TotalAmount)
        Next

        If Not udtEHSTransaction Is Nothing AndAlso Not udtEHSTransaction.TransactionAdditionFields Is Nothing Then
            For Each udtTransactAdditionfield As TransactionAdditionalFieldModel In udtEHSTransaction.TransactionAdditionFields
                'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                If udtTransactAdditionfield.AdditionalFieldValueDesc Is Nothing OrElse udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty Then
                    udtAuditLogEntry.AddDescripton(udtTransactAdditionfield.AdditionalFieldID, udtTransactAdditionfield.AdditionalFieldValueCode)
                Else
                    udtAuditLogEntry.AddDescripton(udtTransactAdditionfield.AdditionalFieldID, udtTransactAdditionfield.AdditionalFieldValueCode + " - " + udtTransactAdditionfield.AdditionalFieldValueDesc)
                End If
                'CRE16-002 (Revamp VSS) [End][Chris YIM]
            Next
        End If

        Return udtAuditLogEntry
    End Function
    'CRE16-002 (Revamp VSS) [End][Chris YIM]

    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Public Shared Function AuditLogENHVSSO(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSTransaction As EHSTransactionModel) As AuditLogEntry
        udtAuditLogEntry.AddDescripton("Scheme", udtEHSTransaction.SchemeCode.Trim)
        udtAuditLogEntry.AddDescripton("Category", udtEHSTransaction.CategoryCode)

        For Each udtEHSTransactionDetail As TransactionDetailModel In udtEHSTransaction.TransactionDetails
            udtAuditLogEntry.AddDescripton("Subsidize Code", udtEHSTransactionDetail.SubsidizeCode.Trim)
            udtAuditLogEntry.AddDescripton("Total Amount", udtEHSTransactionDetail.TotalAmount)
        Next

        If Not udtEHSTransaction Is Nothing AndAlso Not udtEHSTransaction.TransactionAdditionFields Is Nothing Then
            For Each udtTransactAdditionfield As TransactionAdditionalFieldModel In udtEHSTransaction.TransactionAdditionFields
                If udtTransactAdditionfield.AdditionalFieldValueDesc Is Nothing OrElse udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty Then
                    udtAuditLogEntry.AddDescripton(udtTransactAdditionfield.AdditionalFieldID, udtTransactAdditionfield.AdditionalFieldValueCode)
                Else
                    udtAuditLogEntry.AddDescripton(udtTransactAdditionfield.AdditionalFieldID, udtTransactAdditionfield.AdditionalFieldValueCode + " - " + udtTransactAdditionfield.AdditionalFieldValueDesc)
                End If

            Next
        End If

        Return udtAuditLogEntry
    End Function
    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Public Shared Function AuditLogPPP(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSTransaction As EHSTransactionModel) As AuditLogEntry
        udtAuditLogEntry.AddDescripton("Scheme", udtEHSTransaction.SchemeCode.Trim)
        udtAuditLogEntry.AddDescripton("Category", udtEHSTransaction.CategoryCode)

        For Each udtEHSTransactionDetail As TransactionDetailModel In udtEHSTransaction.TransactionDetails
            udtAuditLogEntry.AddDescripton("Subsidize Code", udtEHSTransactionDetail.SubsidizeCode.Trim)
            udtAuditLogEntry.AddDescripton("Total Amount", udtEHSTransactionDetail.TotalAmount)
        Next

        If Not udtEHSTransaction Is Nothing AndAlso Not udtEHSTransaction.TransactionAdditionFields Is Nothing Then
            For Each udtTransactAdditionfield As TransactionAdditionalFieldModel In udtEHSTransaction.TransactionAdditionFields
                If udtTransactAdditionfield.AdditionalFieldValueDesc Is Nothing OrElse udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty Then
                    udtAuditLogEntry.AddDescripton(udtTransactAdditionfield.AdditionalFieldID, udtTransactAdditionfield.AdditionalFieldValueCode)
                Else
                    udtAuditLogEntry.AddDescripton(udtTransactAdditionfield.AdditionalFieldID, udtTransactAdditionfield.AdditionalFieldValueCode + " - " + udtTransactAdditionfield.AdditionalFieldValueDesc)
                End If

            Next
        End If

        Return udtAuditLogEntry
    End Function
    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

    ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Shared Function AuditLogSSSCMC(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSTransaction As EHSTransactionModel) As AuditLogEntry
        udtAuditLogEntry.AddDescripton("Scheme", udtEHSTransaction.SchemeCode.Trim)

        udtAuditLogEntry.AddDescripton("ExchangeRate", udtEHSTransaction.ExchangeRate)

        udtAuditLogEntry.AddDescripton("VoucherRedeemRMB", udtEHSTransaction.VoucherClaimRMB)

        For Each udtTransactAdditionfield As TransactionAdditionalFieldModel In udtEHSTransaction.TransactionAdditionFields
            udtAuditLogEntry.AddDescripton(udtTransactAdditionfield.AdditionalFieldID, udtTransactAdditionfield.AdditionalFieldValueCode)
        Next

        Return udtAuditLogEntry
    End Function
    ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

    ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
    ' --------------------------------------------------------------------------------------
    'COVID19
    Public Shared Function AuditLogCOVID19(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSTransaction As EHSTransactionModel) As AuditLogEntry
        udtAuditLogEntry.AddDescripton("Scheme", udtEHSTransaction.SchemeCode.Trim)
        udtAuditLogEntry.AddDescripton("Category", udtEHSTransaction.CategoryCode)

        For Each udtEHSTransactionDetail As TransactionDetailModel In udtEHSTransaction.TransactionDetails
            udtAuditLogEntry.AddDescripton("Subsidize Code", udtEHSTransactionDetail.SubsidizeCode.Trim)
            udtAuditLogEntry.AddDescripton("Total Amount", udtEHSTransactionDetail.TotalAmount)
        Next

        'Transaction Addition Fields
        If Not udtEHSTransaction Is Nothing AndAlso Not udtEHSTransaction.TransactionAdditionFields Is Nothing Then
            For Each udtTransactAdditionfield As TransactionAdditionalFieldModel In udtEHSTransaction.TransactionAdditionFields

                If udtTransactAdditionfield.AdditionalFieldValueDesc Is Nothing OrElse udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty Then
                    udtAuditLogEntry.AddDescripton(udtTransactAdditionfield.AdditionalFieldID, udtTransactAdditionfield.AdditionalFieldValueCode)
                Else
                    udtAuditLogEntry.AddDescripton(udtTransactAdditionfield.AdditionalFieldID, udtTransactAdditionfield.AdditionalFieldValueCode + " - " + udtTransactAdditionfield.AdditionalFieldValueDesc)
                End If
            Next
        End If

        Return udtAuditLogEntry
    End Function
    ' CRE20-0022 (Immu record) [End][Winnie SUEN]

#End Region

#Region "Vaccination Record"

    Public Shared Sub AuditLogVaccinationRecordClick(ByVal udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteLog(LogID.LOG00072, "Vaccination record button click")
    End Sub

    Public Shared Sub AuditLogForceShowVaccinationRecord(ByVal udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteLog(LogID.LOG00073, "Force show vaccination record")
    End Sub

    Public Shared Sub AuditLogVaccinationRecordCloseClick(ByVal udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteLog(LogID.LOG00074, "Vaccination record close button click")
    End Sub

    Public Shared Sub AuditLogVaccinationRecordViewDetailClick(ByVal udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteLog(LogID.LOG00075, "Vaccination record view details button click")
    End Sub

#End Region

End Class