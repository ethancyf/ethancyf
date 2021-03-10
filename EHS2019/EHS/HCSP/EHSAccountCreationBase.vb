Imports Common.Component.EHSAccount
Imports Common.Component.Scheme
Imports Common.Component
Imports Common.ComObject
Imports Common.Validation

Public MustInherit Class EHSAccountCreationBase
    Inherits TextOnlyBasePage

    Protected Class ActiveViewIndex
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

    End Class

    Public _udtSessionHandler As BLL.SessionHandler = New BLL.SessionHandler()
    Public _udtClaimRulesBLL As New ClaimRules.ClaimRulesBLL()

    Protected MustOverride Sub StepRenderLanguage(ByVal udtEHSAccount As EHSAccountModel)

#Region "Step1a1 Get Account Creation Consent "

    Protected MustOverride Sub SetupStep1a1(ByVal udtEHSAccount As EHSAccountModel)
#End Region

#Region "Step1a2 Get Get Existing Account Consent "

    Protected MustOverride Sub SetupStep1a2(ByVal udtEHSAccount As EHSAccountModel)

#End Region

#Region "Step 1b1 Enter Detail"

    Protected MustOverride Sub SetupStep1b1(ByVal udtEHSAccount As EHSAccountModel, ByVal createPopupPractice As Boolean)

#End Region

#Region "Step lbl Enter Account Detial Valiation"

    Protected MustOverride Function Step1b1HKICValdiation(ByRef udtEHSAccount As EHSAccountModel) As Boolean
    Protected MustOverride Function Step1b1ECValdiation(ByRef udtEHSAccount As EHSAccountModel) As Boolean
    Protected MustOverride Function Step1b1HKBCValidation(ByRef udtEHSAccount As EHSAccountModel) As Boolean
    Protected MustOverride Function Step1b1DIValidation(ByRef udtEHSAccount As EHSAccountModel) As Boolean
    Protected MustOverride Function Step1b1ID235BValidation(ByRef udtEHSAccount As EHSAccountModel) As Boolean
    Protected MustOverride Function Step1b1RepmtValidation(ByRef udtEHSAccount As EHSAccountModel) As Boolean
    Protected MustOverride Function Step1b1VISAValidation(ByRef udtEHSAccount As EHSAccountModel) As Boolean
    Protected MustOverride Function Step1b1AdoptionValidation(ByRef udtEHSAccount As EHSAccountModel) As Boolean

    Protected Function EHSAccountBasicValidation(ByVal strDocCode As String, ByVal udtEHSAccount As EHSAccountModel) As SystemMessage
        Return Me._udtClaimRulesBLL.CheckCreateEHSAccount(udtEHSAccount.SchemeCode, strDocCode, udtEHSAccount, ClaimRules.ClaimRulesBLL.Eligiblity.Check)
    End Function

#End Region

#Region "Step 1b2 Events 'Confirm Detail"

    Protected MustOverride Sub SetupStep1b2(ByVal udtEHSAccount As EHSAccountModel)

#End Region

#Region "Step 1c Events confirm Account Creation"

    Protected MustOverride Sub SetupStep1c(ByVal udtEHSAccount As EHSAccountModel, ByVal activeViewChange As Boolean)

#End Region

#Region "Property"

    Public ReadOnly Property SessionHandler() As BLL.SessionHandler
        Get
            Return Me._udtSessionHandler
        End Get
    End Property

    Public ReadOnly Property ClaimRulesBLL() As ClaimRules.ClaimRulesBLL
        Get
            Return Me._udtClaimRulesBLL
        End Get
    End Property

#End Region

#Region "Page Load"
    'Page Load : LOG00024
    Public Shared Sub AuditLogPageLoad(ByVal udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00024, "Account Creation Page Loaded")
    End Sub
#End Region

#Region "Get Consent 1a1"
    'Get Consent Create Account : Create Account Start Log : LOG00025
    Public Shared Sub AuditLogStep1a1CreateAccountStart(ByVal udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00025, "Create Acccount Pressed")
    End Sub

    'Get Consent Create Account : Create Account End Log : LOG00054
    Public Shared Sub AuditLogStep1a1CreateAccountEnd(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strDescription As String)
        udtAuditLogEntry.AddDescripton("Result case", strDescription)
        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00054, "Create Acccount")
    End Sub

    'Get Consent Create Account : Create Account End Log : LOG00055
    Public Shared Sub AuditLogStep1a1CreateAccountEndBySmartID(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtSmartIDContent As BLL.SmartIDContentModel, ByVal strDescription As String)
        udtAuditLogEntry.AddDescripton("Smart ID Read Status", udtSmartIDContent.SmartIDReadStatus.ToString())
        udtAuditLogEntry.AddDescripton("Result case", strDescription)
        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00055, "Create Acccount By SmartID")
    End Sub


#End Region

#Region "Get Consent 1a2"
    'Get Consent Confirm Account : Confirmed Account : LOG00026
    Public Shared Sub AuditLogStep1a2ConfirmAccount(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strFuncCode As String)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00026, "Confirmed Account")
    End Sub

    'Get Consent Confirm Account : Modify : LOG00027
    Public Shared Sub AuditLogStep1a2ModifyAccount(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strFuncCode As String)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00027, "Modify Account")
    End Sub

    'Get Consent Confirm Account : Confirm Modify : LOG00028
    Public Shared Sub AuditLogStep1a2ConfirmModifyAccount(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strFuncCode As String)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00028, "Confirm Modify Account")
    End Sub

    'Get Consent Confirm Account : Confirm Modify : LOG00056
    Public Shared Sub AuditLogStep1a2ConfirmModifyAccountBySmartID(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strFuncCode As String, ByVal udtSmartIDContent As BLL.SmartIDContentModel)
        udtAuditLogEntry.AddDescripton("Smart ID Read Status", udtSmartIDContent.SmartIDReadStatus.ToString())
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00056, "Confirm Modify Account")
    End Sub


#End Region

#Region "Enter Detail 1b1"
    'Enter Detail : Enter Detail Start : LOG00031
    Public Shared Sub AuditLogStep1b1Start(ByVal udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00031, "Enter Detail Start")
    End Sub

    'Enter Detail : Enter Detail Start Complete : LOG00032
    Public Shared Sub AuditLogStep1b1Complete(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSAccount As EHSAccount.EHSAccountModel)
        Dim strDocCode As String = udtEHSAccount.EHSPersonalInformationList(0).DocCode

        Select Case strDocCode
            Case DocType.DocTypeModel.DocTypeCode.HKIC
                udtAuditLogEntry = AuditLogHKIC(udtAuditLogEntry, udtEHSAccount)
            Case DocType.DocTypeModel.DocTypeCode.EC
                udtAuditLogEntry = AuditLogEC(udtAuditLogEntry, udtEHSAccount)
            Case DocType.DocTypeModel.DocTypeCode.DI
                udtAuditLogEntry = AuditLogDI(udtAuditLogEntry, udtEHSAccount)
            Case DocType.DocTypeModel.DocTypeCode.HKBC
                udtAuditLogEntry = AuditLogHKBC(udtAuditLogEntry, udtEHSAccount)
            Case DocType.DocTypeModel.DocTypeCode.REPMT
                udtAuditLogEntry = AuditLogREPMT(udtAuditLogEntry, udtEHSAccount)
            Case DocType.DocTypeModel.DocTypeCode.ID235B
                udtAuditLogEntry = AuditLogID235B(udtAuditLogEntry, udtEHSAccount)
            Case DocType.DocTypeModel.DocTypeCode.VISA
                udtAuditLogEntry = AuditLogVISA(udtAuditLogEntry, udtEHSAccount)
            Case DocType.DocTypeModel.DocTypeCode.ADOPC
                udtAuditLogEntry = AuditLogADOPC(udtAuditLogEntry, udtEHSAccount)
                ' CRE20-0022 (Immu record) [Start][Martin]
            Case DocType.DocTypeModel.DocTypeCode.CCIC
                udtAuditLogEntry = AuditLogCCIC(udtAuditLogEntry, udtEHSAccount)
            Case DocType.DocTypeModel.DocTypeCode.ROP140
                udtAuditLogEntry = AuditLogROP140(udtAuditLogEntry, udtEHSAccount)
            Case DocType.DocTypeModel.DocTypeCode.PASS
                udtAuditLogEntry = AuditLogPASS(udtAuditLogEntry, udtEHSAccount)
                ' CRE20-0022 (Immu record) [End][Martin]

        End Select

        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00032, "Enter Detail Complete")
    End Sub

    'Enter Detail : Enter Detail Failed : LOG00033

    'Enter Detail : Prompt CCCode Box : LOG00034
    Public Shared Sub AuditLogStep1b1PromptCCCode(ByVal udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00034, "Prompt CCCode Box")
    End Sub
#End Region

#Region "Enter Detail 1b2"
    'Confirm Detail : Confirm Detail Start : LOG00035
    Public Shared Sub AuditLogStep1b2Start(ByVal udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00035, "Confirm Detail Start")
    End Sub

    'Confirm Detail : Confirm Detail Complete : LOG00036
    Public Shared Sub AuditLogStep1b2Complete(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSAccount As EHSAccount.EHSAccountModel)
        Dim strDocCode As String = udtEHSAccount.EHSPersonalInformationList(0).DocCode

        Select Case strDocCode
            Case DocType.DocTypeModel.DocTypeCode.HKIC
                udtAuditLogEntry = AuditLogHKIC(udtAuditLogEntry, udtEHSAccount)
            Case DocType.DocTypeModel.DocTypeCode.EC
                udtAuditLogEntry = AuditLogEC(udtAuditLogEntry, udtEHSAccount)
            Case DocType.DocTypeModel.DocTypeCode.DI
                udtAuditLogEntry = AuditLogDI(udtAuditLogEntry, udtEHSAccount)
            Case DocType.DocTypeModel.DocTypeCode.HKBC
                udtAuditLogEntry = AuditLogHKBC(udtAuditLogEntry, udtEHSAccount)
            Case DocType.DocTypeModel.DocTypeCode.REPMT
                udtAuditLogEntry = AuditLogREPMT(udtAuditLogEntry, udtEHSAccount)
            Case DocType.DocTypeModel.DocTypeCode.ID235B
                udtAuditLogEntry = AuditLogID235B(udtAuditLogEntry, udtEHSAccount)
            Case DocType.DocTypeModel.DocTypeCode.VISA
                udtAuditLogEntry = AuditLogVISA(udtAuditLogEntry, udtEHSAccount)
            Case DocType.DocTypeModel.DocTypeCode.ADOPC
                udtAuditLogEntry = AuditLogADOPC(udtAuditLogEntry, udtEHSAccount)
                ' CRE20-0022 (Immu record) [Start][Martin]
            Case DocType.DocTypeModel.DocTypeCode.CCIC
                udtAuditLogEntry = AuditLogCCIC(udtAuditLogEntry, udtEHSAccount)
            Case DocType.DocTypeModel.DocTypeCode.ROP140
                udtAuditLogEntry = AuditLogROP140(udtAuditLogEntry, udtEHSAccount)
            Case DocType.DocTypeModel.DocTypeCode.PASS
                udtAuditLogEntry = AuditLogPASS(udtAuditLogEntry, udtEHSAccount)
                ' CRE20-0022 (Immu record) [End][Martin]
        End Select

        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00036, "Confirm Detail Complete")
    End Sub

    'Confirm Detail : Confirm Detail Declare Click : LOG00082
    Public Shared Sub AuditLogStep1b2DeclareClick(ByVal udtAuditLogEntry As AuditLogEntry, ByVal blnChecked As Boolean)
        udtAuditLogEntry.AddDescripton("Checked", blnChecked)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00082, "Confirm Detail Declare Click")
    End Sub

    'Confirm Detail : Confirm Detail Failed : LOG00037
#End Region

#Region "Enter Detail 1b3"
    'Confirm Detail : Confirm SmartID Detail Start : LOG00057
    Public Shared Sub AuditLogStep1b3Start(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtSmartIDContent As BLL.SmartIDContentModel)
        udtAuditLogEntry.AddDescripton("Smart ID Read Status", udtSmartIDContent.SmartIDReadStatus.ToString())
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00057, "Confirm SmartID Detail Start")
    End Sub

    'Confirm Detail : Confirm SmartID Detail Complete : LOG00058
    Public Shared Sub AuditLogStep1b3Complete(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSAccount As EHSAccount.EHSAccountModel, ByVal udtSmartIDContent As BLL.SmartIDContentModel, ByVal blnProccedToClaim As Boolean)
        udtAuditLogEntry.AddDescripton("Smart ID Read Status", udtSmartIDContent.SmartIDReadStatus.ToString())
        If blnProccedToClaim Then
            udtAuditLogEntry.AddDescripton("Procced To Claim", "True")
        Else
            udtAuditLogEntry.AddDescripton("Procced To Claim", "False")
        End If

        udtAuditLogEntry.AddDescripton("Session EHS Account", "---")
        udtAuditLogEntry = AuditLogHKIC(udtAuditLogEntry, udtEHSAccount)

        udtAuditLogEntry.AddDescripton("Card Face Data", "---")
        udtAuditLogEntry = AuditLogHKIC(udtAuditLogEntry, udtSmartIDContent.EHSAccount)

        If Not udtSmartIDContent.EHSValidatedAccount Is Nothing Then
            udtAuditLogEntry.AddDescripton("Validated EHS Account", "---")
            udtAuditLogEntry = AuditLogHKIC(udtAuditLogEntry, udtSmartIDContent.EHSValidatedAccount)
        End If

        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00058, "Confirm Detail Complete")
    End Sub

    'Confirm Detail : Confirm SmartID Detail Failed : LOG00059
#End Region

#Region "Complete Account Creation 1c"
    'Complete Account Creation : Complete Account Creation : LOG00038
    Public Shared Sub AuditLogStep1cComplete(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSAccount As EHSAccount.EHSAccountModel)
        udtAuditLogEntry.AddDescripton("Temp Account ID", udtEHSAccount.VoucherAccID)
        udtAuditLogEntry.AddDescripton("Creation Date", udtEHSAccount.CreateDtm)
        udtAuditLogEntry.AddDescripton("Created SP ID", udtEHSAccount.CreateSPID)
        udtAuditLogEntry.AddDescripton("Created By", udtEHSAccount.CreateBy)

        Select Case udtEHSAccount.SearchDocCode
            Case DocType.DocTypeModel.DocTypeCode.HKIC
                udtAuditLogEntry = AuditLogHKIC(udtAuditLogEntry, udtEHSAccount)
            Case DocType.DocTypeModel.DocTypeCode.EC
                udtAuditLogEntry = AuditLogEC(udtAuditLogEntry, udtEHSAccount)
            Case DocType.DocTypeModel.DocTypeCode.DI
                udtAuditLogEntry = AuditLogDI(udtAuditLogEntry, udtEHSAccount)
            Case DocType.DocTypeModel.DocTypeCode.HKBC
                udtAuditLogEntry = AuditLogHKBC(udtAuditLogEntry, udtEHSAccount)
            Case DocType.DocTypeModel.DocTypeCode.REPMT
                udtAuditLogEntry = AuditLogREPMT(udtAuditLogEntry, udtEHSAccount)
            Case DocType.DocTypeModel.DocTypeCode.ID235B
                udtAuditLogEntry = AuditLogID235B(udtAuditLogEntry, udtEHSAccount)
            Case DocType.DocTypeModel.DocTypeCode.VISA
                udtAuditLogEntry = AuditLogVISA(udtAuditLogEntry, udtEHSAccount)
            Case DocType.DocTypeModel.DocTypeCode.ADOPC
                udtAuditLogEntry = AuditLogADOPC(udtAuditLogEntry, udtEHSAccount)
                ' CRE20-0022 (Immu record) [Start][Martin]
            Case DocType.DocTypeModel.DocTypeCode.CCIC
                udtAuditLogEntry = AuditLogCCIC(udtAuditLogEntry, udtEHSAccount)
            Case DocType.DocTypeModel.DocTypeCode.ROP140
                udtAuditLogEntry = AuditLogROP140(udtAuditLogEntry, udtEHSAccount)
            Case DocType.DocTypeModel.DocTypeCode.PASS
                udtAuditLogEntry = AuditLogPASS(udtAuditLogEntry, udtEHSAccount)
                ' CRE20-0022 (Immu record) [End][Martin]
        End Select


        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00038, "Complete Account Creation")
    End Sub

    'Complete Account Creation : Create new account : LOG00039
    Public Shared Sub AuditLogStep1cCreateNewAccount(ByVal udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00039, "Create new account")
    End Sub

    'Complete Account Creation : Procced to claim : LOG00040
    Public Shared Sub AuditLogStep1cProccedToclaim(ByVal udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00040, "Procced to claim")
    End Sub

#End Region

#Region "Selected Practice"

    'Selected Practice : LOG00041
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

        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00041, "Account Creation Practice Selected")
    End Sub

#End Region

#Region "Doc Type Log"
    'Hong Kong Identity Card
    Public Shared Function AuditLogHKIC(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSAccount As EHSAccount.EHSAccountModel) As AuditLogEntry

        Dim udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(DocType.DocTypeModel.DocTypeCode.HKIC)

        udtAuditLogEntry.AddDescripton("Doc Type", DocType.DocTypeModel.DocTypeCode.HKIC)
        udtAuditLogEntry.AddDescripton("HKID No.", udtEHSPersonalInfo.IdentityNum)
        udtAuditLogEntry.AddDescripton("DOB", udtEHSPersonalInfo.DOB)
        udtAuditLogEntry.AddDescripton("ExactDOB", udtEHSPersonalInfo.ExactDOB)
        udtAuditLogEntry.AddDescripton("ENameSurName", udtEHSPersonalInfo.ENameSurName)
        udtAuditLogEntry.AddDescripton("ENameFirstName", udtEHSPersonalInfo.ENameFirstName)
        If Not udtEHSPersonalInfo.CName Is Nothing Then
            udtAuditLogEntry.AddDescripton("CName", udtEHSPersonalInfo.CName)
        Else
            udtAuditLogEntry.AddDescripton("CName", String.Empty)
        End If
        If Not udtEHSPersonalInfo.CCCode1 Is Nothing Then
            udtAuditLogEntry.AddDescripton("CCCode1", udtEHSPersonalInfo.CCCode1)
        Else
            udtAuditLogEntry.AddDescripton("CCCode1", String.Empty)
        End If
        If Not udtEHSPersonalInfo.CCCode2 Is Nothing Then
            udtAuditLogEntry.AddDescripton("CCCode2", udtEHSPersonalInfo.CCCode2)
        Else
            udtAuditLogEntry.AddDescripton("CCCode2", String.Empty)
        End If
        If Not udtEHSPersonalInfo.CCCode2 Is Nothing Then
            udtAuditLogEntry.AddDescripton("CCCode3", udtEHSPersonalInfo.CCCode3)
        Else
            udtAuditLogEntry.AddDescripton("CCCode3", String.Empty)
        End If
        If Not udtEHSPersonalInfo.CCCode2 Is Nothing Then
            udtAuditLogEntry.AddDescripton("CCCode4", udtEHSPersonalInfo.CCCode4)
        Else
            udtAuditLogEntry.AddDescripton("CCCode4", String.Empty)
        End If
        If Not udtEHSPersonalInfo.CCCode2 Is Nothing Then
            udtAuditLogEntry.AddDescripton("CCCode5", udtEHSPersonalInfo.CCCode5)
        Else
            udtAuditLogEntry.AddDescripton("CCCode5", String.Empty)
        End If
        If Not udtEHSPersonalInfo.CCCode2 Is Nothing Then
            udtAuditLogEntry.AddDescripton("CCCode6", udtEHSPersonalInfo.CCCode6)
        Else
            udtAuditLogEntry.AddDescripton("CCCode6", String.Empty)
        End If
        udtAuditLogEntry.AddDescripton("Date of Issue", udtEHSPersonalInfo.DateofIssue)
        udtAuditLogEntry.AddDescripton("Gender", udtEHSPersonalInfo.Gender)
        Return udtAuditLogEntry
    End Function

    'Certificate of Exemption
    Private Shared Function AuditLogEC(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSAccount As EHSAccount.EHSAccountModel) As AuditLogEntry
        Dim udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(DocType.DocTypeModel.DocTypeCode.EC)

        udtAuditLogEntry.AddDescripton("Doc Type", DocType.DocTypeModel.DocTypeCode.EC)
        udtAuditLogEntry.AddDescripton("HKID No.", udtEHSPersonalInfo.IdentityNum)
        udtAuditLogEntry.AddDescripton("Serial No.", udtEHSPersonalInfo.ECSerialNo)
        udtAuditLogEntry.AddDescripton("Reference", udtEHSPersonalInfo.ECReferenceNo)
        udtAuditLogEntry.AddDescripton("ReferenceOtherFormat", IIf(udtEHSPersonalInfo.ECReferenceNoOtherFormat, "Y", "N"))
        udtAuditLogEntry.AddDescripton("DOB", udtEHSPersonalInfo.DOB)
        udtAuditLogEntry.AddDescripton("ExactDOB", udtEHSPersonalInfo.ExactDOB)
        udtAuditLogEntry.AddDescripton("ENameSurName", udtEHSPersonalInfo.ENameSurName)
        udtAuditLogEntry.AddDescripton("ENameFirstName", udtEHSPersonalInfo.ENameFirstName)
        If Not udtEHSPersonalInfo.CName Is Nothing Then
            udtAuditLogEntry.AddDescripton("CName", udtEHSPersonalInfo.CName)
        Else
            udtAuditLogEntry.AddDescripton("CName", String.Empty)
        End If
        If udtEHSPersonalInfo.ECAge.HasValue Then
            udtAuditLogEntry.AddDescripton("Age", udtEHSPersonalInfo.ECAge.Value)
        Else
            udtAuditLogEntry.AddDescripton("Age", String.Empty)
        End If
        If udtEHSPersonalInfo.ECDateOfRegistration.HasValue Then
            udtAuditLogEntry.AddDescripton("Date of Reg", udtEHSPersonalInfo.ECDateOfRegistration.Value)
        Else
            udtAuditLogEntry.AddDescripton("Date of Reg", String.Empty)
        End If

        udtAuditLogEntry.AddDescripton("Date of Issue", udtEHSPersonalInfo.DateofIssue)
        udtAuditLogEntry.AddDescripton("Gender", udtEHSPersonalInfo.Gender)
        Return udtAuditLogEntry
    End Function

    'Document of Identity
    Private Shared Function AuditLogDI(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSAccount As EHSAccount.EHSAccountModel) As AuditLogEntry
        Dim udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(DocType.DocTypeModel.DocTypeCode.DI)
        udtAuditLogEntry.AddDescripton("Doc Type", DocType.DocTypeModel.DocTypeCode.DI)
        udtAuditLogEntry.AddDescripton("Document No.", udtEHSPersonalInfo.IdentityNum)
        udtAuditLogEntry.AddDescripton("DOB", udtEHSPersonalInfo.DOB)
        udtAuditLogEntry.AddDescripton("ExactDOB", udtEHSPersonalInfo.ExactDOB)
        udtAuditLogEntry.AddDescripton("ENameSurName", udtEHSPersonalInfo.ENameSurName)
        udtAuditLogEntry.AddDescripton("ENameFirstName", udtEHSPersonalInfo.ENameFirstName)
        udtAuditLogEntry.AddDescripton("Date of Issue", udtEHSPersonalInfo.DateofIssue)
        udtAuditLogEntry.AddDescripton("Gender", udtEHSPersonalInfo.Gender)
        Return udtAuditLogEntry
    End Function

    'Hong Kong Birth Certificate
    Private Shared Function AuditLogHKBC(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSAccount As EHSAccount.EHSAccountModel) As AuditLogEntry
        Dim udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(DocType.DocTypeModel.DocTypeCode.HKBC)
        udtAuditLogEntry.AddDescripton("Doc Type", DocType.DocTypeModel.DocTypeCode.HKBC)
        udtAuditLogEntry.AddDescripton("Registration No.", udtEHSPersonalInfo.IdentityNum)
        udtAuditLogEntry.AddDescripton("DOB", udtEHSPersonalInfo.DOB)
        udtAuditLogEntry.AddDescripton("ExactDOB", udtEHSPersonalInfo.ExactDOB)

        If Not udtEHSPersonalInfo.OtherInfo Is Nothing Then
            udtAuditLogEntry.AddDescripton("DOB in Word", udtEHSPersonalInfo.OtherInfo)
        Else
            udtAuditLogEntry.AddDescripton("DOB in Word", String.Empty)
        End If

        udtAuditLogEntry.AddDescripton("ENameSurName", udtEHSPersonalInfo.ENameSurName)
        udtAuditLogEntry.AddDescripton("ENameFirstName", udtEHSPersonalInfo.ENameFirstName)
        udtAuditLogEntry.AddDescripton("Gender", udtEHSPersonalInfo.Gender)
        Return udtAuditLogEntry
    End Function

    'Hong Kong Re-entry Permit
    Private Shared Function AuditLogREPMT(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSAccount As EHSAccount.EHSAccountModel) As AuditLogEntry
        Dim udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(DocType.DocTypeModel.DocTypeCode.REPMT)
        udtAuditLogEntry.AddDescripton("Doc Type", DocType.DocTypeModel.DocTypeCode.REPMT)
        udtAuditLogEntry.AddDescripton("Re-entry Permit No.", udtEHSPersonalInfo.IdentityNum)
        udtAuditLogEntry.AddDescripton("DOB", udtEHSPersonalInfo.DOB)
        udtAuditLogEntry.AddDescripton("ExactDOB", udtEHSPersonalInfo.ExactDOB)
        udtAuditLogEntry.AddDescripton("ENameSurName", udtEHSPersonalInfo.ENameSurName)
        udtAuditLogEntry.AddDescripton("ENameFirstName", udtEHSPersonalInfo.ENameFirstName)
        udtAuditLogEntry.AddDescripton("Gender", udtEHSPersonalInfo.Gender)
        udtAuditLogEntry.AddDescripton("Date of Issue", udtEHSPersonalInfo.DateofIssue)
        Return udtAuditLogEntry
    End Function

    'ID 235B
    Private Shared Function AuditLogID235B(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSAccount As EHSAccount.EHSAccountModel) As AuditLogEntry
        Dim udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(DocType.DocTypeModel.DocTypeCode.ID235B)
        udtAuditLogEntry.AddDescripton("Doc Type", DocType.DocTypeModel.DocTypeCode.ID235B)
        udtAuditLogEntry.AddDescripton("Birth Entry No.", udtEHSPersonalInfo.IdentityNum)
        udtAuditLogEntry.AddDescripton("DOB", udtEHSPersonalInfo.DOB)
        udtAuditLogEntry.AddDescripton("ExactDOB", udtEHSPersonalInfo.ExactDOB)
        udtAuditLogEntry.AddDescripton("ENameSurName", udtEHSPersonalInfo.ENameSurName)
        udtAuditLogEntry.AddDescripton("ENameFirstName", udtEHSPersonalInfo.ENameFirstName)
        udtAuditLogEntry.AddDescripton("Gender", udtEHSPersonalInfo.Gender)
        udtAuditLogEntry.AddDescripton("Permitted Remain Until", udtEHSPersonalInfo.PermitToRemainUntil)
        Return udtAuditLogEntry
    End Function

    'Non-Hong Kong Travel Documents
    Private Shared Function AuditLogVISA(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSAccount As EHSAccount.EHSAccountModel) As AuditLogEntry
        Dim udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(DocType.DocTypeModel.DocTypeCode.VISA)
        udtAuditLogEntry.AddDescripton("Doc Type", DocType.DocTypeModel.DocTypeCode.VISA)
        udtAuditLogEntry.AddDescripton("Visa/Reference No.", udtEHSPersonalInfo.IdentityNum)
        udtAuditLogEntry.AddDescripton("Passport No.", udtEHSPersonalInfo.Foreign_Passport_No)
        udtAuditLogEntry.AddDescripton("DOB", udtEHSPersonalInfo.DOB)
        udtAuditLogEntry.AddDescripton("ExactDOB", udtEHSPersonalInfo.ExactDOB)
        udtAuditLogEntry.AddDescripton("ENameSurName", udtEHSPersonalInfo.ENameSurName)
        udtAuditLogEntry.AddDescripton("ENameFirstName", udtEHSPersonalInfo.ENameFirstName)
        udtAuditLogEntry.AddDescripton("Gender", udtEHSPersonalInfo.Gender)
        Return udtAuditLogEntry
    End Function

    'Certificate issued by the Births and Deaths Registry for adopted children
    Private Shared Function AuditLogADOPC(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSAccount As EHSAccount.EHSAccountModel) As AuditLogEntry
        Dim udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(DocType.DocTypeModel.DocTypeCode.ADOPC)
        udtAuditLogEntry.AddDescripton("Doc Type", DocType.DocTypeModel.DocTypeCode.ADOPC)
        udtAuditLogEntry.AddDescripton("No. of Entry", udtEHSPersonalInfo.IdentityNum)
        udtAuditLogEntry.AddDescripton("No. of Entry Prefix", udtEHSPersonalInfo.AdoptionPrefixNum)
        udtAuditLogEntry.AddDescripton("DOB", udtEHSPersonalInfo.DOB)
        udtAuditLogEntry.AddDescripton("ExactDOB", udtEHSPersonalInfo.ExactDOB)

        If Not udtEHSPersonalInfo.OtherInfo Is Nothing Then
            udtAuditLogEntry.AddDescripton("DOB in Word", udtEHSPersonalInfo.OtherInfo)
        Else
            udtAuditLogEntry.AddDescripton("DOB in Word", String.Empty)
        End If

        udtAuditLogEntry.AddDescripton("ENameSurName", udtEHSPersonalInfo.ENameSurName)
        udtAuditLogEntry.AddDescripton("ENameFirstName", udtEHSPersonalInfo.ENameFirstName)
        udtAuditLogEntry.AddDescripton("Gender", udtEHSPersonalInfo.Gender)
        Return udtAuditLogEntry
    End Function

    ' CRE20-0022 (Immu record) [Start][Martin]
    'CCIC
    Private Shared Function AuditLogCCIC(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSAccount As EHSAccount.EHSAccountModel) As AuditLogEntry
        Dim udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(DocType.DocTypeModel.DocTypeCode.CCIC)
        udtAuditLogEntry.AddDescripton("Doc Type", DocType.DocTypeModel.DocTypeCode.CCIC)
        udtAuditLogEntry.AddDescripton("Document No.", udtEHSPersonalInfo.IdentityNum)
        udtAuditLogEntry.AddDescripton("DOB", udtEHSPersonalInfo.DOB)
        udtAuditLogEntry.AddDescripton("ExactDOB", udtEHSPersonalInfo.ExactDOB)
        udtAuditLogEntry.AddDescripton("ENameSurName", udtEHSPersonalInfo.ENameSurName)
        udtAuditLogEntry.AddDescripton("ENameFirstName", udtEHSPersonalInfo.ENameFirstName)
        udtAuditLogEntry.AddDescripton("Gender", udtEHSPersonalInfo.Gender)
        udtAuditLogEntry.AddDescripton("Date of Issue", udtEHSPersonalInfo.DateofIssue)
        Return udtAuditLogEntry
    End Function


    'ROP140
    Private Shared Function AuditLogROP140(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSAccount As EHSAccount.EHSAccountModel) As AuditLogEntry
        Dim udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(DocType.DocTypeModel.DocTypeCode.ROP140)
        udtAuditLogEntry.AddDescripton("Doc Type", DocType.DocTypeModel.DocTypeCode.ROP140)
        udtAuditLogEntry.AddDescripton("Document No.", udtEHSPersonalInfo.IdentityNum)
        udtAuditLogEntry.AddDescripton("DOB", udtEHSPersonalInfo.DOB)
        udtAuditLogEntry.AddDescripton("ExactDOB", udtEHSPersonalInfo.ExactDOB)
        udtAuditLogEntry.AddDescripton("ENameSurName", udtEHSPersonalInfo.ENameSurName)
        udtAuditLogEntry.AddDescripton("ENameFirstName", udtEHSPersonalInfo.ENameFirstName)
        udtAuditLogEntry.AddDescripton("Gender", udtEHSPersonalInfo.Gender)
        udtAuditLogEntry.AddDescripton("Date of Issue", udtEHSPersonalInfo.DateofIssue)
        Return udtAuditLogEntry
    End Function

    'PASS
    Private Shared Function AuditLogPASS(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSAccount As EHSAccount.EHSAccountModel) As AuditLogEntry
        Dim udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(DocType.DocTypeModel.DocTypeCode.PASS)
        udtAuditLogEntry.AddDescripton("Doc Type", DocType.DocTypeModel.DocTypeCode.PASS)
        udtAuditLogEntry.AddDescripton("Document No.", udtEHSPersonalInfo.IdentityNum)
        udtAuditLogEntry.AddDescripton("DOB", udtEHSPersonalInfo.DOB)
        udtAuditLogEntry.AddDescripton("ExactDOB", udtEHSPersonalInfo.ExactDOB)
        udtAuditLogEntry.AddDescripton("ENameSurName", udtEHSPersonalInfo.ENameSurName)
        udtAuditLogEntry.AddDescripton("ENameFirstName", udtEHSPersonalInfo.ENameFirstName)
        udtAuditLogEntry.AddDescripton("Gender", udtEHSPersonalInfo.Gender)
        Return udtAuditLogEntry
    End Function
    ' CRE20-0022 (Immu record) [End][Martin]
#End Region


End Class
