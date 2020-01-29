' ID    | Validated rules                                               | HCSP Platform | HCSP Platform | Back office   | Upload Claim
'       |                                                               | Claim         | Rectification | Claim         | (HKMA)
' ------------------------------------------------------------------------------------------------------------------------------------
' 1     | service date outside subsidy・s claim period                  | Block         | N/A           | Block         | Reject
' 2     | service date before SP / SP scheme enrol and effective date   | Block         | N/A           | Block         | Reject
' 3     | service date later then permit to remain until of ID235B      | Block         | N/A           | Block         | Reject
' 4     | Exceed day-back limit                                         | Block         | N/A           | Block         | Reject
' 5     | Validated eHealth account with status not active              | Block         | N/A           | Block         | Reject
' ------------------------------------------------------------------------------------------------------------------------------------
' 6     | SP & Practice with status not active                          | Block         | N/A           | Block         | Reject
' 7     | Patient・s age outside document・s accepted age               | Block         | N/A           | Block         | Reject
' 8     | No available subsidies                                        | Block         | N/A           | Block         | Reject
' 9     | Eligibility Rules for Scheme                                  | Block         | N/A           | Block         | Reject
' 10    | Eligibility Rules for Subsidy                                 | Block         | N/A           | Block         | Reject
' ------------------------------------------------------------------------------------------------------------------------------------
' 11    | Eligibility age for Category                                  | Block         | N/A           | Block         | Reject
' 12    | 1 Dose & 2 Dose of CIVSS between 25 days and 28 days          | Block         | N/A           | Block         | Reject
' 13    | 1 Dose & 2 Dose of CIVSS less then 24 days                    | Block         | N/A           | Block         | Reject
' 14    | 1 Dose & 2 Dose of HSIVSS less then 21 days                   | Block         | N/A           | Block         | Reject
' 15    | 1st dose later then that of 2nd dose of CIVSS                 | Block         | N/A           | Block         | Reject
' ------------------------------------------------------------------------------------------------------------------------------------
' 16    | 1st dose later then that of 2nd dose of HSIVSS                | Block         | N/A           | Block         | Reject
' 17    | Pre-primary school certificate warning                        | Block         | N/A           | Block         | Reject
' 18    | TSW warning                                                   | Block         | N/A           | Block         | Reject
' 19    |                                                               | Block         | N/A           | Block         | Reject
' 20    |                                                               | Block         | N/A           | Block         | Reject
' ------------------------------------------------------------------------------------------------------------------------------------

Imports Common.ComObject
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.DocType
Imports Common.Component.EHSClaimVaccine
Imports Common.Component.ClaimCategory
Imports Common.Component.SchemeDetails
Imports Common.Component.ClaimRules
Imports Common.Component.ClaimRules.ClaimRulesBLL
Imports Common.Component.InputPicker
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.VoucherInfo

Namespace Component.EHSClaim.EHSClaimBLL


    Public Class EHSClaimBLL

        Private Const cFunctionCode As String = "990000"

        Private _EHSClaimValidationBLL As New EHSClaimValidationBLL()
        Private _SchemeClaimBLL As New SchemeClaimBLL()
        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        ' Remove non reference object
        'Private _udtOutsideClaimRuleCheckBLL As New OutsideClaimValidation.OutsideClaimRuleCheckBLL()
        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]
        Private _udtClaimCategoryBLL As New ClaimCategoryBLL()
        Private _udtFormatter As New Format.Formatter()
        Private _udtGeneralFunction As New Common.ComFunction.GeneralFunction
        Private _udtValidator As New Validation.Validator
        Private _udtEHSClaimValidationRuleCollection As New EHSClaimValidationRuleModelCollection()
        Private _udtEHSClaimValidationRuleCollectionResult As New EHSClaimValidationRuleModelCollection()
        'Private Shared _enumClaimAction As Integer


#Region "Object Class"





        <Serializable()> Enum RuleID
            InnerDoseBlock
            InnerDoseWarning
            DoseSeqBlock
            DoseSeqWarning
            DosePeriodBlock
            DosePeriodWarning
            ClaimRuleEligibilityBlock
            ClaimRuleEligibilityWarning
            Eligibility
            CategoryElibility
            DoseSeq
            DosePeriod
            PreprimarySchoolCertificate
            TSWChecking
            ServiceDateClaimPeriodChecking
            ServiceDateSPChecking
            ServiceDateID235BChecking
            DayBackLimitChecking
            ACStatusSuspend
            ACStatusDecease
            SPInactive
            PracticeInactive
            SPSuspend
            SPDelist
            SPSchemeInactive
            PracticeSuspend
            PracticeDelist
            PracticeSchemeDelist
            DayBackLimitAndSPDateChecking
            AvailableSubsidyNoVoucher
            'AvailableSubsidyNoVaccine
            AvailableSubsidyVaccineSameTaken
            AvailableSubsidyVaccineExceedEntitlement
            AvailableSubsidyVaccineNotEntitled
            DayBackMinDate
            DocExceedEC
            DocExceedOther
            EligibilityDB
            CatEligibilityDB
            ServiceDateFuture
            PracNotEnrolScheme
            PracJoinedProf
            SchemeSubsidy
            ' CRE11-007
            DeathDecease
            ServiceDateDecease
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            SubsidizeSeqDateBlock
            SubsidizeSeqDateWarning
            CrossSeasonIntervalBlock
            CrossSeasonIntervalWarning
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]          
            'CRE15-004 (TIV and QIV) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            SameVaccineDoseForNewTran
            SubsidyNotProvideService
            'CRE15-004 (TIV and QIV) [End][Chris YIM]
            'CRE16-017 (Block EHCP make voucher claim for themselves) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            SelfClaim
            'CRE16-017 (Block EHCP make voucher claim for themselves) [End][Chris YIM]

            ' CRE19-003 (Opt voucher capping) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            AvailableSubsidyNoVoucherQuota
            ' CRE19-003 (Opt voucher capping) [End][Winnie]
            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            DHCServiceNotProvided
            VoucherExceedDHCMaxClaimAmt
            ' CRE19-006 (DHC) [End][Winnie]

            None
        End Enum

        <Serializable()> Public Class RuleIDText

            Public Const None As String = ""
            Public Const InnerDoseBlock As String = "InnerDose"
            Public Const InnerDoseWarning As String = "InnerDose"
            Public Const DoseSeqBlock As String = "DoseSEQ"
            Public Const DoseSeqWarning As String = "DoseSEQ"
            Public Const DosePeriodBlock As String = "DosePeriod"
            Public Const DosePeriodWarning As String = "DosePeriod"
            Public Const ClaimRuleEligibilityBlock As String = "ClaimRule"
            Public Const ClaimRuleEligibilityWarning As String = "ClaimRule"
            Public Const Eligibility As String = "Eligibility"
            Public Const CategoryElibility As String = "CatEligibility"
            Public Const DoseSeq As String = "DoseSEQ"
            Public Const DosePeriod As String = "DosePeriod"
            Public Const PreprimarySchoolCertificate As String = "PreSchool"
            Public Const TSWChecking As String = "TSW"
            Public Const ServiceDateClaimPeriodChecking As String = "ServiceDtPeriod"
            Public Const ServiceDateSPChecking As String = "ServiceDateSP"
            Public Const ServiceDateID235BChecking As String = "ServiceDateID235B"
            Public Const DayBackLimitChecking As String = "DayBack"
            ' Public Const AccountStatusChecking As String = "AccountStatus"
            Public Const SPStatusChecking As String = "SPStatus"
            Public Const DayBackLimitAndSPDateChecking As String = "DayBackLimit"
            Public Const AvailableSubsidyChecking As String = "AvailableSubsidy"
        End Class

        <Serializable()> Public Class ValidationResults
            Public BlockResults As New RuleResultList()
            Public WarningResults As New RuleResultList()
            Public WarningIndicatorResults As New RuleResultList()
            Public RejectResults As New RuleResultList()
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
            Public DeclarationResults As New RuleResultList()
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Endt][Koala]
        End Class

        Public Const VariableDelimiter = "||"

        ' ValidationResuls
        '                   -> RuleResultList (Block)
        '                   -> RuleResultList (Warning)
        '                   -> RuleResultList (WarningIndicator)
        '                   -> RuleResultList (Reject)
        '                   -> RuleResultList (Declaration)

        <Serializable()> Public Class RuleResultList
            'Public HandleMethod As String
            Public RuleResults As New List(Of RuleResult)

            Public Sub Merge(ByVal udtRuleResultList As RuleResultList)
                For Each udtRuleResult As RuleResult In udtRuleResultList.RuleResults
                    Me.RuleResults.Add(udtRuleResult)
                Next
            End Sub


        End Class

        ' RuleResultList
        '               -> Block
        '               -> List of RuleResult

        <Serializable()> Public Class RuleResult
            Private _enumRuleID As RuleID
            Private _strValidationRuleID As String
            Private _objSystemMessage As SystemMessage
            Private _strMessageVariableName As String
            Private _strMessageVariableValue As String
            Private _strMessageVariableNameChi As String
            Private _strMessageVariableValueChi As String
            Private _strMessageDescription As String
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
            Private _strMessageDescriptionChi As String
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]
            Private _strSchemeCode As String
            Private _strSchemeSeq As String
            Private _strSubsidizeCode As String
            Private _strRuleGroup1 As String
            Private _strRuleGroup2 As String
            Private _strWarnIndicatorCode As String

            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
            Private _alMessageVariableName As New ArrayList
            Private _alMessageVariableValue As New ArrayList
            Private _alMessageVariableNameChi As New ArrayList
            Private _alMessageVariableValueChi As New ArrayList
            'Private _alMessageVariableName As ArrayList
            'Private _alMessageVariableValue As ArrayList
            'Private _alMessageVariableNameChi As ArrayList
            'Private _alMessageVariableValueChi As ArrayList
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]

            ReadOnly Property ValidationRuleID() As String
                Get
                    Return Me._strValidationRuleID
                End Get
            End Property
            ReadOnly Property RuleID() As RuleID
                Get
                    Return Me._enumRuleID
                End Get
            End Property

            'ReadOnly Property ErrorMessage() As SystemMessage
            '    Get
            '        Return Me._objSystemMessage
            '    End Get
            'End Property

            Public Property ErrorMessage() As SystemMessage
                Get
                    Return Me._objSystemMessage
                End Get
                Set(ByVal value As SystemMessage)
                    Me._objSystemMessage = value
                End Set
            End Property

            ReadOnly Property MessageVariableName() As String
                Get
                    Return Me._strMessageVariableName
                End Get
            End Property
            ReadOnly Property MessageVariableValue() As String
                Get
                    Return Me._strMessageVariableValue
                End Get
            End Property
            ReadOnly Property MessageVariableNameChi() As String
                Get
                    Return Me._strMessageVariableNameChi
                End Get
            End Property
            ReadOnly Property MessageVariableValueChi() As String
                Get
                    Return Me._strMessageVariableValueChi
                End Get
            End Property

            ReadOnly Property MessageVariableNameArrayList() As ArrayList
                Get
                    Return Me._alMessageVariableName
                End Get
            End Property
            ReadOnly Property MessageVariableValueArrayList() As ArrayList
                Get
                    Return Me._alMessageVariableValue
                End Get
            End Property
            ReadOnly Property MessageVariableNameChiArrayList() As ArrayList
                Get
                    Return Me._alMessageVariableNameChi
                End Get
            End Property
            ReadOnly Property MessageVariableValueChiArrayList() As ArrayList
                Get
                    Return Me._alMessageVariableValueChi
                End Get
            End Property

            'ReadOnly Property MessageDescription() As String
            '    Get
            '        Return Me._strMessageDescription
            '    End Get
            'End Property


            Public Property MessageDescription() As String
                Get
                    Return Me._strMessageDescription
                End Get
                Set(ByVal value As String)
                    Me._strMessageDescription = value
                End Set
            End Property

            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
            Public Property MessageDescriptionChi() As String
                Get
                    Return Me._strMessageDescriptionChi
                End Get
                Set(ByVal value As String)
                    Me._strMessageDescriptionChi = value
                End Set
            End Property
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]

            ReadOnly Property SchemeCode() As String
                Get
                    Return Me._strSchemeCode
                End Get
            End Property
            ReadOnly Property SchemeSeq() As String
                Get
                    Return Me._strSchemeSeq
                End Get
            End Property
            ReadOnly Property SubsidizeCode() As String
                Get
                    Return Me._strSubsidizeCode
                End Get
            End Property
            ReadOnly Property RuleGroup1() As String
                Get
                    Return Me._strRuleGroup1
                End Get
            End Property
            ReadOnly Property RuleGroup2() As String
                Get
                    Return Me._strRuleGroup2
                End Get
            End Property
            Public Property WarnIndicatorCode() As String
                Get
                    Return Me._strWarnIndicatorCode
                End Get
                Set(ByVal value As String)
                    Me._strWarnIndicatorCode = value
                End Set
            End Property



            'Public Sub New(ByVal enumRuleID As RuleID, ByVal strMessageVariableName As String, ByVal strMessageVariableValue As String, ByVal strMessageVariableNameChi As String, ByVal strMessageVariableValueChi As String, ByVal strSchemeCode As String, ByVal strSchemeSeq As String, ByVal strSubsidizeCode As String)

            '    Dim udtClaimValidationRule As EHSClaimValidationRuleModel = GetValidationRule(_enumClaimAction, enumRuleID)
            '    Dim udtSystemMessage As New SystemMessage(udtClaimValidationRule.Function_Code, udtClaimValidationRule.Severity_Code, udtClaimValidationRule.Message_Code)

            '    Me._enumRuleID = enumRuleID
            '    Me._objSystemMessage = udtSystemMessage

            '    Me._strMessageVariableName = strMessageVariableName
            '    Me._strMessageVariableValue = strMessageVariableValue
            '    Me._strMessageVariableNameChi = strMessageVariableNameChi
            '    Me._strMessageVariableValueChi = strMessageVariableValueChi
            '    Me._strSchemeCode = strSchemeCode
            '    Me._strSchemeSeq = strSchemeSeq
            '    Me._strSubsidizeCode = strSubsidizeCode

            '    Me._alMessageVariableName = ReturnMessageVariableAsArrayList(strMessageVariableName)
            '    Me._alMessageVariableValue = ReturnMessageVariableAsArrayList(strMessageVariableValue)
            '    Me._alMessageVariableNameChi = ReturnMessageVariableAsArrayList(strMessageVariableNameChi)
            '    Me._alMessageVariableValueChi = ReturnMessageVariableAsArrayList(strMessageVariableValueChi)

            '    Me._strMessageDescription = ReturnVariableFeedMessage(Me.objSystemMessage, Me._alMessageVariableName, Me._alMessageVariableValue, Me._alMessageVariableNameChi, Me._alMessageVariableValueChi)

            'End Sub

            Public Sub New(ByVal enumRuleID As RuleID, _
                           Optional ByVal strMessageVariableName As String = Nothing, _
                           Optional ByVal strMessageVariableValue As String = Nothing, _
                           Optional ByVal strMessageVariableNameChi As String = Nothing, _
                           Optional ByVal strMessageVariableValueChi As String = Nothing, _
                           Optional ByVal strSchemeCode As String = Nothing, _
                           Optional ByVal strSchemeSeq As String = Nothing, _
                           Optional ByVal strSubsidizeCode As String = Nothing, _
                           Optional ByVal strRuleGroup1 As String = Nothing, _
                           Optional ByVal strRuleGroup2 As String = Nothing)

                Me._enumRuleID = enumRuleID
                Dim udtEHSClaimBLL As New EHSClaimBLL()
                Me._strValidationRuleID = udtEHSClaimBLL.GetRuleIDString(enumRuleID)

                Me._strMessageVariableName = strMessageVariableName
                Me._strMessageVariableValue = strMessageVariableValue
                Me._strMessageVariableNameChi = strMessageVariableNameChi
                Me._strMessageVariableValueChi = strMessageVariableValueChi
                Me._strSchemeCode = strSchemeCode
                Me._strSchemeSeq = strSchemeSeq
                Me._strSubsidizeCode = strSubsidizeCode
                Me._strRuleGroup1 = strRuleGroup1
                Me._strRuleGroup2 = strRuleGroup2

                ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
                If strMessageVariableName IsNot Nothing Then
                    Me._alMessageVariableName = ReturnMessageVariableAsArrayList(strMessageVariableName)
                    Me._alMessageVariableValue = ReturnMessageVariableAsArrayList(strMessageVariableValue)
                    Me._alMessageVariableNameChi = ReturnMessageVariableAsArrayList(strMessageVariableNameChi)
                    Me._alMessageVariableValueChi = ReturnMessageVariableAsArrayList(strMessageVariableValueChi)
                End If
                ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]

                'Me._strMessageDescription = ReturnVariableFeedMessage(objSystemMessage, Me._alMessageVariableName, Me._alMessageVariableValue, Me._alMessageVariableNameChi, Me._alMessageVariableValueChi)

            End Sub

            Public Sub New(ByVal enumRuleID As RuleID, ByVal objSystemMessage As SystemMessage, Optional ByVal strMessageVariableName As String = Nothing, Optional ByVal strMessageVariableValue As String = Nothing, Optional ByVal strMessageVariableNameChi As String = Nothing, Optional ByVal strMessageVariableValueChi As String = Nothing)
                Me._enumRuleID = enumRuleID
                Dim udtEHSClaimBLL As New EHSClaimBLL()
                Me._strValidationRuleID = udtEHSClaimBLL.GetRuleIDString(enumRuleID)

                Me._objSystemMessage = objSystemMessage
                Me._strMessageVariableName = strMessageVariableName
                Me._strMessageVariableValue = strMessageVariableValue
                Me._strMessageVariableNameChi = strMessageVariableNameChi
                Me._strMessageVariableValueChi = strMessageVariableValueChi

                Me._alMessageVariableName = ReturnMessageVariableAsArrayList(strMessageVariableName)
                Me._alMessageVariableValue = ReturnMessageVariableAsArrayList(strMessageVariableValue)
                Me._alMessageVariableNameChi = ReturnMessageVariableAsArrayList(strMessageVariableNameChi)
                Me._alMessageVariableValueChi = ReturnMessageVariableAsArrayList(strMessageVariableValueChi)

                Me._strMessageDescription = ReturnVariableFeedMessage(objSystemMessage, Me._alMessageVariableName, Me._alMessageVariableValue, Me._alMessageVariableNameChi, Me._alMessageVariableValueChi)

            End Sub

            Public Sub New(ByVal enumRuleID As RuleID, ByVal objSystemMessage As SystemMessage, ByVal alMessageVariableName As ArrayList, ByVal alMessageVariableValue As ArrayList, ByVal alMessageVariableNameChi As ArrayList, ByVal alMessageVariableValueChi As ArrayList)
                Me._enumRuleID = enumRuleID
                Dim udtEHSClaimBLL As New EHSClaimBLL()
                Me._strValidationRuleID = udtEHSClaimBLL.GetRuleIDString(enumRuleID)

                Me._objSystemMessage = objSystemMessage
                Me._alMessageVariableName = alMessageVariableName
                Me._alMessageVariableValue = alMessageVariableValue
                Me._alMessageVariableNameChi = alMessageVariableNameChi
                Me._alMessageVariableValueChi = alMessageVariableValueChi

                Me._strMessageVariableName = ReturnMessageVariableAsString(alMessageVariableName)
                Me._strMessageVariableValue = ReturnMessageVariableAsString(alMessageVariableValue)
                Me._strMessageVariableNameChi = ReturnMessageVariableAsString(alMessageVariableNameChi)
                Me._strMessageVariableValueChi = ReturnMessageVariableAsString(alMessageVariableValueChi)

                Me._strMessageDescription = ReturnVariableFeedMessage(objSystemMessage, Me._alMessageVariableName, Me._alMessageVariableValue, Me._alMessageVariableNameChi, Me._alMessageVariableValueChi)
            End Sub


            'Public Sub New(ByVal enumRuleID As RuleID, ByVal objSystemMessage As SystemMessage)
            '    Me._enumRuleID = enumRuleID
            '    Me._objSystemMessage = objSystemMessage
            'End Sub

            Public Sub New(ByVal objSystemMessage As SystemMessage, Optional ByVal strMessageVariableName As String = Nothing, Optional ByVal strMessageVariableValue As String = Nothing, Optional ByVal strMessageVariableNameChi As String = Nothing, Optional ByVal strMessageVariableValueChi As String = Nothing)
                Me.New(EHSClaimBLL.RuleID.None, objSystemMessage, strMessageVariableName, strMessageVariableValue, strMessageVariableNameChi, strMessageVariableValueChi)

                'Me.New(EHSClaimBLL.RuleID.None, objSystemMessage, ReturnMessageVariableAsArrayList(strMessageVariableName), ReturnMessageVariableAsArrayList(strMessageVariableValue), ReturnMessageVariableAsArrayList(strMessageVariableNameChi), ReturnMessageVariableAsArrayList(strMessageVariableValueChi))
            End Sub
        End Class



        ' RuleResult
        '           -> Rule ID
        '           -> SystemMessage

        <Serializable()> Enum ClaimAction
            HCSPClaim
            HCSPRectification
            HCVUClaim
            UploadClaim
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            UploadStudent
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
            UploadPrecheck ' For checking the subsidy's dose and mark remark on precheck report
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]
        End Enum

        <Serializable()> Enum HandlingMethod
            Block
            Warning
            WarningIndicator
            Reject
        End Enum

#End Region
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
        ''' <summary>
        ''' Validate claim creation
        ''' </summary>
        ''' <param name="udtEHSTransaction">Current claiming transaction model</param>
        ''' <param name="udtHAVaccineResult">HA vaccine result and vaccination records (ignore this parameter if cllnTranVaccineBenefit is provided)</param>
        ''' <param name="udtDHVaccineResult">HA vaccine result and vaccination records (ignore this parameter if cllnTranVaccineBenefit is provided)</param>
        ''' <param name="udtAuditLogEntry"></param>
        ''' <param name="udtCurrentTransactionModelCollection">Unknown ussage parameter</param>
        ''' <param name="udtTransactionBenefitDetailList">All vaccination record (eHS + HA + DH)</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidateClaimCreation(ByVal enumClaimAction As ClaimAction, _
                                              ByVal udtEHSTransaction As EHSTransactionModel, _
                                              ByVal udtHAVaccineResult As Common.WebService.Interface.HAVaccineResult, _
                                              ByVal udtDHVaccineResult As Common.WebService.Interface.DHVaccineResult, _
                                              ByVal udtAuditLogEntry As Common.ComObject.AuditLogEntry, _
                                              Optional ByVal udtCurrentTransactionModelCollection As EHSTransactionModelCollection = Nothing, _
                                              Optional ByVal udtTransactionBenefitDetailList As TransactionDetailVaccineModelCollection = Nothing) As ValidationResults
            'Public Function ValidateClaimCreation(ByVal enumClaimAction As ClaimAction, _
            '                                      ByVal udtEHSTransaction As EHSTransactionModel, _
            '                                      ByVal udtHAVaccineResult As Common.WebService.Interface.HAVaccineResult, _
            '                                      ByVal udtDHVaccineResult As Common.WebService.Interface.DHVaccineResult, _
            '                                      ByVal udtAuditLogEntry As Common.ComObject.AuditLogEntry, _
            '                                      Optional ByVal udtCurrentTransactionModelCollection As EHSTransactionModelCollection = Nothing) As ValidationResults
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
            ' To Do: Validate the input parameter
            _udtEHSClaimValidationRuleCollection = EHSClaimValidationBLL.GetValidationRuleCache()
            Dim udtValidationResults As New ValidationResults()

            ' ------------------------------------------------------------------------------- 
            ' ------------------------------------------------------------------------------- 

            Dim htDuplicateCheck As New Hashtable

            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]

            ' Check the checking result
            Dim udtRuleResultList As RuleResultList = Me.ValidateClaimCreationCore(udtEHSTransaction, udtHAVaccineResult, udtDHVaccineResult, udtAuditLogEntry, udtCurrentTransactionModelCollection, udtTransactionBenefitDetailList)
            'Dim udtRuleResultList As RuleResultList = Me.ValidateClaimCreationCore(udtEHSTransaction, udtHAVaccineResult, udtDHVaccineResult, udtAuditLogEntry, udtCurrentTransactionModelCollection)
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]

            For Each udtRuleResult As RuleResult In udtRuleResultList.RuleResults

                'Dim strDuplicateCheckKey As String = udtRuleResult.ErrorMessage.FunctionCode.ToString() + udtRuleResult.ErrorMessage.SeverityCode.ToString() + udtRuleResult.ErrorMessage.MessageCode.ToString()

                'CRE16-025 (Lowering voucher eligibility age) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim udtClaimValidationRule As EHSClaimValidationRuleModel
                'If udtRuleResult.SchemeCode = Nothing Then
                '    udtClaimValidationRule = Me.GetValidationRule(enumClaimAction, udtRuleResult.RuleID)
                If udtRuleResult.RuleGroup1 = Nothing Then
                    udtClaimValidationRule = Me.GetValidationRule(enumClaimAction, udtRuleResult.RuleID, udtRuleResult.SchemeCode)
                Else
                    udtClaimValidationRule = Me.GetValidationRule(enumClaimAction, udtRuleResult.RuleID, udtRuleResult.SchemeCode, udtRuleResult.SchemeSeq, udtRuleResult.SubsidizeCode, udtRuleResult.RuleGroup1, udtRuleResult.RuleGroup2)
                End If
                'CRE16-025 (Lowering voucher eligibility age) [End][Chris YIM]

                If Not udtClaimValidationRule Is Nothing Then
                    udtRuleResult.ErrorMessage = New SystemMessage(udtClaimValidationRule.Function_Code.Trim(), udtClaimValidationRule.Severity_Code.Trim(), udtClaimValidationRule.Message_Code.Trim())
                    Dim strDuplicateCheckKey As String = ""
                    'If Not udtClaimValidationRule.Scheme_Code = Nothing And Not udtClaimValidationRule.Scheme_Seq = Nothing And Not udtClaimValidationRule.Subsidize_Code = Nothing Then
                    '    strDuplicateCheckKey = udtRuleResult.RuleID.ToString().Trim() + "-" + udtClaimValidationRule.Scheme_Code + udtClaimValidationRule.Scheme_Seq + udtClaimValidationRule.Subsidize_Code
                    'Else
                    '    strDuplicateCheckKey = udtRuleResult.RuleID.ToString().Trim()
                    'End If

                    'strDuplicateCheckKey = udtClaimValidationRule.Function_Code.ToString().Trim() + udtClaimValidationRule.Severity_Code.ToString().Trim() + udtClaimValidationRule.Message_Code.ToString().Trim()
                    strDuplicateCheckKey = udtClaimValidationRule.HandlingMethod.Trim() + udtClaimValidationRule.Function_Code.ToString().Trim() + udtClaimValidationRule.Severity_Code.ToString().Trim() + udtClaimValidationRule.Message_Code.ToString().Trim()
                    'CRE15-004 (TIV and QIV) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    If Not udtRuleResult.MessageVariableNameArrayList Is Nothing Then
                        If udtRuleResult.MessageVariableNameArrayList.Count > 0 Then
                            strDuplicateCheckKey = strDuplicateCheckKey + "_" + udtRuleResult.MessageVariableValue
                        End If
                    End If

                    If Not udtRuleResult.MessageVariableNameChiArrayList Is Nothing Then
                        If udtRuleResult.MessageVariableNameChiArrayList.Count > 0 Then
                            strDuplicateCheckKey = strDuplicateCheckKey + "_" + udtRuleResult.MessageVariableValueChi
                        End If
                    End If
                    'CRE15-004 (TIV and QIV) [End][Chris YIM]

                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
                    udtRuleResult.MessageDescription = ReturnVariableFeedMessage(udtRuleResult.ErrorMessage, udtRuleResult.MessageVariableNameArrayList, udtRuleResult.MessageVariableValueArrayList, udtRuleResult.MessageVariableNameChiArrayList, udtRuleResult.MessageVariableValueChiArrayList, EnumLanguage.EN)
                    udtRuleResult.MessageDescriptionChi = ReturnVariableFeedMessage(udtRuleResult.ErrorMessage, udtRuleResult.MessageVariableNameArrayList, udtRuleResult.MessageVariableValueArrayList, udtRuleResult.MessageVariableNameChiArrayList, udtRuleResult.MessageVariableValueChiArrayList, EnumLanguage.TC)
                    'udtRuleResult.MessageDescription = ReturnVariableFeedMessage(udtRuleResult.ErrorMessage, udtRuleResult.MessageVariableNameArrayList, udtRuleResult.MessageVariableValueArrayList, udtRuleResult.MessageVariableNameChiArrayList, udtRuleResult.MessageVariableValueChiArrayList)
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]

                    If Not htDuplicateCheck.ContainsKey(strDuplicateCheckKey) Then
                        If Not udtClaimValidationRule.WarnIndicator_Code Is Nothing Then
                            udtRuleResult.WarnIndicatorCode = udtClaimValidationRule.WarnIndicator_Code.Trim()
                        End If
                        Select Case udtClaimValidationRule.HandlingMethod.Trim()
                            Case HandleMethodClass.Block
                                udtValidationResults.BlockResults.RuleResults.Add(udtRuleResult)
                            Case HandleMethodClass.Warning
                                udtValidationResults.WarningResults.RuleResults.Add(udtRuleResult)
                            Case HandleMethodClass.Declaration
                                udtValidationResults.DeclarationResults.RuleResults.Add(udtRuleResult)
                        End Select

                        'Select Case enumClaimAction
                        '    Case ClaimAction.HCSPClaim

                        '    Case ClaimAction.HCSPRectification
                        '    Case ClaimAction.HCVUClaim

                        '        Select Case udtRuleResult.RuleID

                        '            Case RuleID.DoseSeq, RuleID.DosePeriod, RuleID.ServiceDateClaimPeriodChecking, RuleID.DoseSeqBlock, _
                        '                RuleID.DoseSeqWarning, RuleID.DocumentAcceptedAge
                        '                udtValidationResults.BlockResults.RuleResults.Add(udtRuleResult)

                        '            Case RuleID.AccountStatusChecking, RuleID.InnerDoseBlock, _
                        '                    RuleID.InnerDoseWarning, RuleID.PreprimarySchoolCertificate, RuleID.ServiceDateID235BChecking, _
                        '                    RuleID.SPStatusChecking, RuleID.TSWChecking, _
                        '                    RuleID.DosePeriodBlock, RuleID.DosePeriodWarning, RuleID.Eligibility, RuleID.ServiceDateSPChecking, _
                        '                    RuleID.DayBackLimitChecking, RuleID.AvailableSubsidyChecking, RuleID.CategoryElibility
                        '                udtValidationResults.WarningResults.RuleResults.Add(udtRuleResult)

                        '        End Select

                        '    Case ClaimAction.UploadClaim

                        'End Select

                        htDuplicateCheck.Add(strDuplicateCheckKey, "")
                    End If

                End If

            Next

            Return udtValidationResults

        End Function

        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
        ''' <summary>
        ''' Validate claim creation
        ''' </summary>
        ''' <param name="udtEHSTransaction">Current claiming transaction model</param>
        ''' <param name="udtHAVaccineResult">HA vaccine result and vaccination records (ignore this parameter if cllnTranVaccineBenefit is provided)</param>
        ''' <param name="udtDHVaccineResult">HA vaccine result and vaccination records (ignore this parameter if cllnTranVaccineBenefit is provided)</param>
        ''' <param name="udtAuditLogEntry"></param>
        ''' <param name="udtCurrentTransactionModelCollection">Unknown ussage parameter</param>
        ''' <param name="udtTransactionBenefitDetailList">All vaccination record (eHS + HA + DH)</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidateClaimCreationCore(ByVal udtEHSTransaction As EHSTransactionModel, _
                                                  ByVal udtHAVaccineResult As Common.WebService.Interface.HAVaccineResult, _
                                                  ByVal udtDHVaccineResult As Common.WebService.Interface.DHVaccineResult, _
                                                  ByVal udtAuditLogEntry As Common.ComObject.AuditLogEntry, _
                                                  Optional ByVal udtCurrentTransactionModelCollection As EHSTransactionModelCollection = Nothing, _
                                                  Optional ByVal udtTransactionBenefitDetailList As TransactionDetailVaccineModelCollection = Nothing) As RuleResultList
            'Public Function ValidateClaimCreationCore(ByVal udtEHSTransaction As EHSTransactionModel, _
            '                                      ByVal udtHAVaccineResult As Common.WebService.Interface.HAVaccineResult, _
            '                                      ByVal udtDHVaccineResult As Common.WebService.Interface.DHVaccineResult, _
            '                                      ByVal udtAuditLogEntry As Common.ComObject.AuditLogEntry, _
            '                                      Optional ByVal udtCurrentTransactionModelCollection As EHSTransactionModelCollection = Nothing) As RuleResultList
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
            Dim blnIsEligible As Boolean = True
            Dim udtRuleResultList As New RuleResultList()
            udtRuleResultList.RuleResults = New List(Of RuleResult)
            Dim intCurrentTransactionClaimedVoucher As Integer = 0

            ' ------------------------------------------------------------------------------------------------------------------------------------
            ' Init
            ' ------------------------------------------------------------------------------------------------------------------------------------
            Dim udtServiceProvider As ServiceProvider.ServiceProviderModel = getServiceProvider(udtEHSTransaction.ServiceProviderID, udtEHSTransaction.ServiceDate)
            Dim udtPractice As Practice.PracticeModel = getPractice(udtEHSTransaction.ServiceProviderID, udtEHSTransaction.PracticeID, udtEHSTransaction.ServiceDate)
            Dim udtProfessionalBLL As New Common.Component.Professional.ProfessionalBLL()
            Dim udtProfessionalModelCollection As Common.Component.Professional.ProfessionalModelCollection = udtProfessionalBLL.GetProfessinalListFromPermanentBySPID(udtServiceProvider.SPID, New Common.DataAccess.Database())

            Dim strSPID As String = udtEHSTransaction.ServiceProviderID
            Dim strSchemeCode As String = udtEHSTransaction.SchemeCode
            Dim strCategoryCode As String = String.Empty

            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            If Not udtEHSTransaction.CategoryCode Is Nothing Then
                strCategoryCode = udtEHSTransaction.CategoryCode
            End If
            'CRE16-002 (Revamp VSS) [End][Chris YIM]

            Dim udtEHSAccount As EHSAccountModel = udtEHSTransaction.EHSAcct
            Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSTransaction.DocCode)

            Dim udtSchemeClaimBLL As New SchemeClaimBLL()
            Dim udtSchemeClaimModel As SchemeClaimModel = Nothing
            udtSchemeClaimModel = Me._SchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSTransaction.SchemeCode, udtEHSTransaction.ServiceDate.AddDays(1).AddMinutes(-1))

            Dim udtSchemeInfoModelCollection As Common.Component.SchemeInformation.SchemeInformationModelCollection = udtServiceProvider.SchemeInfoList


            'Dim strSchemeCodeEnrol As String = udtSchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(udtSchemeClaimModel.SchemeCode)
            'Dim udtSchemeInfoModel As Common.Component.SchemeInformation.SchemeInformationModel = udtServiceProvider.SchemeInfoList().Filter(strSchemeCodeEnrol)

            Dim udtSchemeInfoModel As New Common.Component.SchemeInformation.SchemeInformationModel
            For Each udtScheme As SchemeInformation.SchemeInformationModel In udtSchemeInfoModelCollection.Values
                If udtScheme.SchemeCode.Trim.Contains(udtEHSTransaction.SchemeCode.Trim) Then
                    udtSchemeInfoModel = udtScheme
                End If
            Next

            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            Dim udtEHSTransactionBLL As New EHSTransactionBLL()

            '----------------------------------------------------------------
            ' a1. If has inputted benefit, use it. If not, get benefit again
            '----------------------------------------------------------------
            If udtTransactionBenefitDetailList Is Nothing Then

                udtTransactionBenefitDetailList = udtEHSTransactionBLL.getTransactionDetailVaccine(udtEHSTransaction.DocCode, udtEHSAccount.getPersonalInformation(udtEHSTransaction.DocCode).IdentityNum)

                Dim objVaccinationBLL As New VaccinationBLL

                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
                ' ----------------------------------------------------------
                If objVaccinationBLL.SchemeContainVaccine(udtSchemeClaimModel) Then
                    If Not udtHAVaccineResult Is Nothing Then
                        udtTransactionBenefitDetailList.JoinVaccineList(udtEHSAccount.getPersonalInformation(udtEHSTransaction.DocCode), udtHAVaccineResult.SinglePatient.VaccineList, udtAuditLogEntry, strSchemeCode)
                    End If

                    If Not udtDHVaccineResult Is Nothing Then
                        udtTransactionBenefitDetailList.JoinVaccineList(udtEHSAccount.getPersonalInformation(udtEHSTransaction.DocCode), udtDHVaccineResult.SingleClient.VaccineRecordList, udtAuditLogEntry, strSchemeCode)
                    End If

                End If
                ' CRE18-001(CIMS Vaccination Sharing) [End][Chris YIM]
            End If

            '-----------------------------------------------------------------------------
            ' a2. Convert to the "TransactionDetailModelCollection" for general purpose
            '-----------------------------------------------------------------------------
            ' Convert "TransactionDetailVaccineModelCollection" to the "TransactionDetailModelCollection"
            Dim udtTransDetailBenefitList As TransactionDetailModelCollection = udtTransactionBenefitDetailList

            If _SchemeClaimBLL.ConvertControlTypeFromSchemeClaimCode(udtEHSTransaction.SchemeCode) = SchemeClaimModel.EnumControlType.EHAPP Then
                udtTransDetailBenefitList = udtEHSTransactionBLL.getTransactionDetailBenefit(udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, udtEHSTransaction.SchemeCode)
            End If

            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]

            If Not udtCurrentTransactionModelCollection Is Nothing Then
                For Each udtEHSTransactionModel As EHSTransactionModel In udtCurrentTransactionModelCollection
                    If udtEHSTransactionModel.VoucherClaim > 0 Then
                        intCurrentTransactionClaimedVoucher = intCurrentTransactionClaimedVoucher + udtEHSTransactionModel.VoucherClaim
                    End If
                    For Each udtCurrentTransactionDetailVaccineModel As TransactionDetailModel In udtEHSTransactionModel.TransactionDetails
                        udtTransactionBenefitDetailList.Add(udtCurrentTransactionDetailVaccineModel)
                    Next
                Next
            End If
            'udtTransactionBenefitDetailList = udtEHSTransactionBLL.getTransactionDetailBenefit(udtEHSTransaction.DocCode, udtEHSAccount.getPersonalInformation(udtEHSTransaction.DocCode).IdentityNum, udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SchemeSeq)

            Dim udtRuleResult As RuleResult = Nothing
            Dim udtTempClaimRuleList As RuleResultList = Nothing


            ' ------------------------------------------------------------------------------------------------------------------------------------
            ' Rule added for external interface
            ' ------------------------------------------------------------------------------------------------------------------------------------

            ' Check Practice Enroled scheme
            udtRuleResult = Me.CheckPracticeEnrolScheme(udtPractice, udtEHSTransaction.SchemeCode)
            Dim blnIsExternalInterfaceCheckValid As Boolean = True
            Dim blnIsPracticeEnrolledSchemeValid As Boolean = True
            If Not udtRuleResult Is Nothing Then
                udtRuleResultList.RuleResults.Add(udtRuleResult)
                blnIsPracticeEnrolledSchemeValid = False
                blnIsExternalInterfaceCheckValid = False
            End If

            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            If _SchemeClaimBLL.ConvertControlTypeFromSchemeClaimCode(udtEHSTransaction.SchemeCode) = SchemeClaimModel.EnumControlType.VOUCHER OrElse _
                _SchemeClaimBLL.ConvertControlTypeFromSchemeClaimCode(udtEHSTransaction.SchemeCode) = SchemeClaimModel.EnumControlType.VOUCHERCHINA Then
                udtRuleResult = Me.CheckPracticeJoinedProf(udtPractice, udtEHSTransaction.ServiceType)
                If Not udtRuleResult Is Nothing Then
                    udtRuleResultList.RuleResults.Add(udtRuleResult)
                    blnIsExternalInterfaceCheckValid = False
                End If
            End If

            If Not _SchemeClaimBLL.ConvertControlTypeFromSchemeClaimCode(udtEHSTransaction.SchemeCode) = SchemeClaimModel.EnumControlType.VOUCHER AndAlso _
               Not _SchemeClaimBLL.ConvertControlTypeFromSchemeClaimCode(udtEHSTransaction.SchemeCode) = SchemeClaimModel.EnumControlType.VOUCHERCHINA Then
                udtRuleResult = Me.CheckSchemeSubsidy(udtEHSTransaction)
                If Not udtRuleResult Is Nothing Then
                    udtRuleResultList.RuleResults.Add(udtRuleResult)
                    blnIsExternalInterfaceCheckValid = False
                End If
            End If
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

            If blnIsExternalInterfaceCheckValid = False Then
                Return udtRuleResultList
            End If
            ' ------------------------------------------------------------------------------------------------------------------------------------
            ' Rule 1: 
            ' ------------------------------------------------------------------------------------------------------------------------------------
            udtRuleResult = Me.CheckServiceDateClaimPeriod(udtEHSTransaction, udtSchemeClaimModel)
            If Not udtRuleResult Is Nothing Then
                udtRuleResultList.RuleResults.Add(udtRuleResult)
            End If

            ' ------------------------------------------------------------------------------------------------------------------------------------
            ' Rule 1.1: 
            ' ------------------------------------------------------------------------------------------------------------------------------------
            ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]
            ' -----------------------------------------------------------------------------------------
            udtRuleResult = Me.CheckServiceDateClaimPeriod(udtEHSTransaction, udtSchemeClaimModel)
            If Not udtRuleResult Is Nothing Then
                udtRuleResultList.RuleResults.Add(udtRuleResult)
            End If
            ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

            ' ------------------------------------------------------------------------------------------------------------------------------------
            ' Rule 2, 4: 
            ' ------------------------------------------------------------------------------------------------------------------------------------
            If blnIsPracticeEnrolledSchemeValid = True Then
                udtTempClaimRuleList = Me.CheckServiceDateSP(udtEHSTransaction, udtServiceProvider, udtSchemeInfoModelCollection, udtPractice, udtEHSTransaction.TransactionDetails)
                If Not udtTempClaimRuleList Is Nothing Then
                    udtRuleResultList.Merge(udtTempClaimRuleList)
                End If
            End If
            'udtTempClaimRuleList = Me.CheckDayBackLimitAndSPServiceDate(udtEHSTransaction, udtSchemeClaimModel, udtSchemeInfoModel, udtServiceProvider, udtSchemeInfoModelCollection)
            'If Not udtTempClaimRuleList Is Nothing Then
            '    udtRuleResultList.Merge(udtTempClaimRuleList)
            'End If


            ' ------------------------------------------------------------------------------------------------------------------------------------
            ' Rule 3: 
            ' ------------------------------------------------------------------------------------------------------------------------------------
            udtRuleResult = Me.CheckServiceDateID235B(udtEHSTransaction, udtEHSPersonalInfo)
            If Not udtRuleResult Is Nothing Then
                udtRuleResultList.RuleResults.Add(udtRuleResult)
            End If
            ' ------------------------------------------------------------------------------------------------------------------------------------
            ' Rule 4: 
            ' ------------------------------------------------------------------------------------------------------------------------------------
            udtTempClaimRuleList = Me.CheckDayBackLimit(udtEHSTransaction, udtSchemeClaimModel, udtSchemeInfoModel, udtServiceProvider, udtSchemeInfoModelCollection)
            If Not udtTempClaimRuleList Is Nothing Then
                udtRuleResultList.Merge(udtTempClaimRuleList)
            End If
            ' ------------------------------------------------------------------------------------------------------------------------------------
            ' Rule 5: 
            ' ------------------------------------------------------------------------------------------------------------------------------------
            udtRuleResult = Me.CheckAccountStatus(udtEHSAccount)
            If Not udtRuleResult Is Nothing Then
                udtRuleResultList.RuleResults.Add(udtRuleResult)
            End If

            ' ------------------------------------------------------------------------------------------------------------------------------------
            ' Rule 5.5: 
            ' ------------------------------------------------------------------------------------------------------------------------------------
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            'udtRuleResult = Me.CheckServiceDateDecease(udtEHSTransaction, udtEHSAccount)
            udtRuleResult = Me.CheckServiceDateDecease(udtEHSTransaction, udtEHSPersonalInfo)
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            If Not udtRuleResult Is Nothing Then
                udtRuleResultList.RuleResults.Add(udtRuleResult)
            End If

            'CRE16-017 (Block EHCP make voucher claim for themselves) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            ' ------------------------------------------------------------------------------------------------------------------------------------
            ' Rule 6: 
            ' ------------------------------------------------------------------------------------------------------------------------------------
            udtRuleResult = Me.IsSPClaimForThemselves(udtServiceProvider.HKID, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, udtSchemeClaimModel.AvailableHCSPSubPlatform)
            If Not udtRuleResult Is Nothing Then
                udtRuleResultList.RuleResults.Add(udtRuleResult)
            End If
            'CRE16-017 (Block EHCP make voucher claim for themselves) [End][Chris YIM]


            'CRE15-004 (TIV and QIV) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            ' ------------------------------------------------------------------------------------------------------------------------------------
            ' Rule 6.1: 
            ' ------------------------------------------------------------------------------------------------------------------------------------
            udtRuleResult = Me.CheckSPStatus(udtEHSTransaction, udtServiceProvider, udtSchemeInfoModelCollection, udtEHSTransaction.TransactionDetails)
            If blnIsPracticeEnrolledSchemeValid = True Then
                If Not udtRuleResult Is Nothing Then
                    udtRuleResultList.RuleResults.Add(udtRuleResult)
                End If
            End If

            ' ------------------------------------------------------------------------------------------------------------------------------------
            ' Rule 6.2: 
            ' ------------------------------------------------------------------------------------------------------------------------------------
            udtTempClaimRuleList = Me.CheckPracticeStatus(udtEHSTransaction, udtPractice, udtSchemeInfoModelCollection, udtEHSTransaction.TransactionDetails)
            If blnIsPracticeEnrolledSchemeValid = True Then
                If Not udtTempClaimRuleList Is Nothing Then
                    If udtTempClaimRuleList.RuleResults.Count > 0 Then
                        udtRuleResultList.Merge(udtTempClaimRuleList)
                    End If
                End If
            End If
            'CRE15-004 (TIV and QIV) [End][Chris YIM]

            ' ------------------------------------------------------------------------------------------------------------------------------------
            ' Rule 7: 
            ' ------------------------------------------------------------------------------------------------------------------------------------
            udtRuleResult = Me.CheckDocumentAcceptedAge(udtEHSTransaction, udtEHSPersonalInfo, udtSchemeClaimModel)
            If Not udtRuleResult Is Nothing Then
                udtRuleResultList.RuleResults.Add(udtRuleResult)
            End If

            ' ------------------------------------------------------------------------------------------------------------------------------------
            ' Rule 9, 10, 17: Eligibility
            ' ------------------------------------------------------------------------------------------------------------------------------------
            ' If Not udtEHSTransaction.SchemeCode = SchemeClaimModel.HCVS Then
            udtTempClaimRuleList = Me.CheckEligibility(udtEHSTransaction, udtSchemeClaimModel, udtEHSTransaction.TransactionDetails, udtTransactionBenefitDetailList, udtEHSPersonalInfo)
            If Not udtTempClaimRuleList Is Nothing Then
                If udtTempClaimRuleList.RuleResults.Count > 0 Then
                    blnIsEligible = False
                End If
                udtRuleResultList.Merge(udtTempClaimRuleList)
            End If
            'End If


            ' ------------------------------------------------------------------------------------------------------------------------------------
            ' Eligibility (Voucher Only)
            ' ------------------------------------------------------------------------------------------------------------------------------------
            'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            udtTempClaimRuleList = Me.CheckEligibilityVoucherOnly(udtEHSTransaction, udtSchemeClaimModel, udtEHSPersonalInfo)
            If Not udtTempClaimRuleList Is Nothing Then
                If udtTempClaimRuleList.RuleResults.Count > 0 Then
                    blnIsEligible = False
                End If
                udtRuleResultList.Merge(udtTempClaimRuleList)
            End If
            'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

            ' ------------------------------------------------------------------------------------------------------------------------------------
            ' Rule 11: Category Eligibility
            ' ------------------------------------------------------------------------------------------------------------------------------------           
            udtTempClaimRuleList = Me.CheckCategoryEligibility(udtEHSTransaction, udtSchemeClaimModel, udtEHSTransaction.TransactionDetails, udtTransactionBenefitDetailList, udtEHSPersonalInfo, strCategoryCode)
            If Not udtTempClaimRuleList Is Nothing Then
                If udtTempClaimRuleList.RuleResults.Count > 0 Then
                    blnIsEligible = False
                End If
                udtRuleResultList.Merge(udtTempClaimRuleList)
            End If

            ' ------------------------------------------------------------------------------------------------------------------------------------
            ' Rule 8: Check Available Subsidy, it must be after check eligibility
            ' ------------------------------------------------------------------------------------------------------------------------------------
            udtTempClaimRuleList = CheckAvailableSubsidyNew(strCategoryCode, udtEHSTransaction, udtEHSPersonalInfo, udtSchemeClaimModel, udtEHSAccount, udtEHSTransaction.TransactionDetails, udtTransDetailBenefitList, blnIsEligible, intCurrentTransactionClaimedVoucher, udtPractice)

            If Not udtTempClaimRuleList Is Nothing Then
                If udtTempClaimRuleList.RuleResults.Count > 0 Then
                    blnIsEligible = False
                End If
                udtRuleResultList.Merge(udtTempClaimRuleList)
            End If

            'CRE15-004 (TIV and QIV) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'Rule : SubsidyNotProvideService
            udtTempClaimRuleList = CheckAvailableSubsidy(udtEHSTransaction, udtServiceProvider)

            If Not udtTempClaimRuleList Is Nothing Then
                udtRuleResultList.Merge(udtTempClaimRuleList)
            End If

            'Rule : SameVaccineDose
            udtRuleResult = CheckSameVaccineForNewTran(udtEHSTransaction)

            If Not udtRuleResult Is Nothing Then
                udtRuleResultList.RuleResults.Add(udtRuleResult)
            End If
            'CRE15-004 (TIV and QIV) [End][Chris YIM]

            'CRE16-026 (Add PCV13) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            ' ------------------------------------------------------------------------------------------------------------------------------------
            ' Rule 18: TSW warning
            ' ------------------------------------------------------------------------------------------------------------------------------------
            udtRuleResult = Me.CheckTSW(udtSchemeClaimModel, strSPID, udtEHSPersonalInfo.IdentityNum)
            If Not udtRuleResult Is Nothing Then
                udtRuleResultList.RuleResults.Add(udtRuleResult)
            End If

            Dim EnumControlType As SchemeClaimModel.EnumControlType = _SchemeClaimBLL.ConvertControlTypeFromSchemeClaimCode(udtEHSTransaction.SchemeCode)
            Dim udtInputPicker As New InputPickerModel
            Dim udtInputVaccineCollection As New InputVaccineModelCollection

            '------------------------------------------------------------------------------
            ' 1. Retrieve and concat the current claiming vaccination from TransactionDetail
            '------------------------------------------------------------------------------
            'Get Active SchemeClaim with SubsidizeGroupClaim (By ServiceDate)
            Dim udtServiceSchemeClaimModel As SchemeClaimModel = _SchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSTransaction.SchemeCode, udtEHSTransaction.ServiceDate.AddDays(1).AddMinutes(-1))

            For i As Integer = 0 To udtEHSTransaction.TransactionDetails.Count - 1
                Dim udtTransactionDetail As TransactionDetailModel = udtEHSTransaction.TransactionDetails(i)

                Dim udtSubsidizeGroupClaim As SubsidizeGroupClaimModel = udtServiceSchemeClaimModel.SubsidizeGroupClaimList.Filter(udtTransactionDetail.SchemeCode, udtTransactionDetail.SchemeSeq, udtTransactionDetail.SubsidizeCode)

                udtInputVaccineCollection.Add(New InputVaccineModel(udtTransactionDetail.SchemeCode, udtTransactionDetail.SchemeSeq, _
                                                                    udtTransactionDetail.SubsidizeCode.Trim(), _
                                                                    udtTransactionDetail.SubsidizeItemCode.Trim(), _
                                                                    udtTransactionDetail.AvailableItemCode.Trim(), _
                                                                    udtSubsidizeGroupClaim.DisplaySeq, udtSubsidizeGroupClaim.DisplayCodeForClaim))
            Next

            If udtInputVaccineCollection.Count > 0 Then
                udtInputPicker.EHSClaimVaccine = udtInputVaccineCollection
            End If

            '-------------------------------------------------------------------
            ' 2. Collect RCHCode
            '-------------------------------------------------------------------
            Dim strRCHCode As String = String.Empty

            If Not IsNothing(udtEHSTransaction.TransactionAdditionFields) AndAlso Not IsNothing(udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.RCHCode)) Then
                strRCHCode = udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.RCHCode).AdditionalFieldValueCode
            End If

            udtInputPicker.RCHCode = strRCHCode

            '-------------------------------------------------------------------
            ' 3. Collect High Risk Option
            '-------------------------------------------------------------------
            udtInputPicker.HighRisk = udtEHSTransaction.HighRisk


            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
            ' --------------------------------------------------------------------------------------
            '-------------------------------------------------------------------
            ' 4. Collect "School Code"
            '-------------------------------------------------------------------
            Dim strSchoolCode As String = String.Empty

            If Not IsNothing(udtEHSTransaction.TransactionAdditionFields) AndAlso _
                Not IsNothing(udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.SchoolCode)) Then

                strSchoolCode = udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.SchoolCode).AdditionalFieldValueCode

            End If

            udtInputPicker.SchoolCode = strSchoolCode
            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

            ' Check Claim Rule
            ' ------------------------------------------------------------------------------------------------------------------------------------
            ' Rule : Claim Rule SUBSIDIZESEQDATE - check inputted transaction against inputted transaction
            ' ------------------------------------------------------------------------------------------------------------------------------------
            If Not EnumControlType = SchemeClaimModel.EnumControlType.VOUCHER AndAlso Not EnumControlType = SchemeClaimModel.EnumControlType.VOUCHERCHINA Then
                ' Check to New Transaction in UI
                For i As Integer = 0 To udtEHSTransaction.TransactionDetails.Count - 1
                    Dim udtCurrentTransactionDetailList As New TransactionDetailModelCollection()

                    If i + 1 <= udtEHSTransaction.TransactionDetails.Count - 1 Then
                        For j As Integer = i + 1 To udtEHSTransaction.TransactionDetails.Count - 1
                            If Not (udtEHSTransaction.TransactionDetails(i).SchemeCode = udtEHSTransaction.TransactionDetails(j).SchemeCode And _
                                udtEHSTransaction.TransactionDetails(i).SchemeSeq = udtEHSTransaction.TransactionDetails(j).SchemeSeq And _
                                udtEHSTransaction.TransactionDetails(i).SubsidizeCode = udtEHSTransaction.TransactionDetails(j).SubsidizeCode) Then
                                udtEHSTransaction.TransactionDetails(j).ServiceReceiveDtm = udtEHSTransaction.ServiceDate
                                udtCurrentTransactionDetailList.Add(udtEHSTransaction.TransactionDetails(j))
                            End If
                        Next
                    End If

                    If udtCurrentTransactionDetailList.Count > 0 Then
                        udtTempClaimRuleList = Me.CheckClaimRules(udtEHSTransaction, udtSchemeClaimModel, udtEHSTransaction.TransactionDetails, udtCurrentTransactionDetailList, udtEHSPersonalInfo, udtInputPicker, ClaimRuleModel.RuleTypeClass.SUBSIDIZESEQDATE)
                        If Not udtTempClaimRuleList Is Nothing Then
                            udtRuleResultList.Merge(udtTempClaimRuleList)
                        End If
                    End If
                Next

            End If
            ' ------------------------------------------------------------------------------------------------------------------------------------
            ' Rule : Claim Rule INNERDOSE - check inputted transaction against inputted transaction
            ' ------------------------------------------------------------------------------------------------------------------------------------
            If Not EnumControlType = SchemeClaimModel.EnumControlType.VOUCHER AndAlso Not EnumControlType = SchemeClaimModel.EnumControlType.VOUCHERCHINA Then
                udtTempClaimRuleList = Me.CheckClaimRules(udtEHSTransaction, udtSchemeClaimModel, udtEHSTransaction.TransactionDetails, udtEHSTransaction.TransactionDetails, udtEHSPersonalInfo, udtInputPicker, ClaimRuleModel.RuleTypeClass.INNERDOSE)
                If Not udtTempClaimRuleList Is Nothing Then
                    udtRuleResultList.Merge(udtTempClaimRuleList)
                End If
            End If

            ' ------------------------------------------------------------------------------------------------------------------------------------
            ' Rule : ALL Claim Rules - check inputted transaction against transaction in Database
            ' ------------------------------------------------------------------------------------------------------------------------------------
            If Not EnumControlType = SchemeClaimModel.EnumControlType.VOUCHER AndAlso Not EnumControlType = SchemeClaimModel.EnumControlType.VOUCHERCHINA Then
                ' Check transaction in Database
                udtTempClaimRuleList = Me.CheckClaimRules(udtEHSTransaction, udtSchemeClaimModel, udtEHSTransaction.TransactionDetails, udtTransactionBenefitDetailList, udtEHSPersonalInfo, udtInputPicker)
                If Not udtTempClaimRuleList Is Nothing Then
                    udtRuleResultList.Merge(udtTempClaimRuleList)
                End If
            End If

            'CRE16-026 (Add PCV13) [End][Chris YIM]

            ' ------------------------------------------------------------------------------------------------------------------------------------
            ' Return
            ' ------------------------------------------------------------------------------------------------------------------------------------

            Return udtRuleResultList
        End Function

#Region "Supporting Validate Function"
        ' 18 TSW warning 
        Private Function CheckTSW(ByVal udtSchemeClaimModel As SchemeClaimModel, ByVal strSPID As String, ByVal strHKID As String) As RuleResult
            If udtSchemeClaimModel.TSWCheckingEnable Then
                If Me._EHSClaimValidationBLL.chkIsTSWCase(strSPID, strHKID) Then
                    'Dim udtSystemMessage As New SystemMessage(cFunctionCode, "E", "00266")
                    'Return New RuleResult(RuleID.TSWChecking, udtSystemMessage)
                    Return New RuleResult(RuleID.TSWChecking)
                End If
            End If
            Return Nothing
        End Function

        '1 service date outside subsidy・s claim period    
        Private Function CheckServiceDateClaimPeriod(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtSchemeClaimModel As SchemeClaimModel) As RuleResult

            'Dim udtSystemMessage As New SystemMessage(cFunctionCode, "E", "00237")
            If udtSchemeClaimModel Is Nothing Then
                'If True Then
                '    Dim udtEHSClaimValidationRuleCollection As EHSClaimValidationRuleModelCollection = _udtEHSClaimValidationRuleCollection.GetEHSClaimValidationRuleByRuleID("ServiceDtPeriod")
                '    _udtEHSClaimValidationRuleCollectionResult = _udtEHSClaimValidationRuleCollectionResult.Merge(udtEHSClaimValidationRuleCollection)
                'Return New RuleResult(RuleID.ServiceDateClaimPeriodChecking, udtSystemMessage)
                Return New RuleResult(RuleID.ServiceDateClaimPeriodChecking)
            End If

            ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]
            ' -----------------------------------------------------------------------------------------
            If Not Profession.ProfessionBLL.GetProfessionListByServiceCategoryCode(udtEHSTransaction.ServiceType).IsClaimPeriod(udtEHSTransaction.ServiceDate) Then
                Return New RuleResult(RuleID.ServiceDateClaimPeriodChecking)
            End If
            ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

            For Each udtTransactionDetail As EHSTransaction.TransactionDetailModel In udtEHSTransaction.TransactionDetails
                Dim udtSubsidizeGroupClaimModel As Component.Scheme.SubsidizeGroupClaimModel = udtSchemeClaimModel.SubsidizeGroupClaimList.Filter(udtTransactionDetail.SchemeCode, udtTransactionDetail.SchemeSeq, udtTransactionDetail.SubsidizeCode)
                If udtEHSTransaction.ServiceDate.Date > udtSubsidizeGroupClaimModel.LastServiceDtm.Date Or _
                   udtEHSTransaction.ServiceDate.Date < udtSubsidizeGroupClaimModel.ClaimPeriodFrom.Date Then
                    'Return New RuleResult(RuleID.ServiceDateClaimPeriodChecking, udtSystemMessage)
                    Return New RuleResult(RuleID.ServiceDateClaimPeriodChecking)
                End If
                Exit For
            Next
            Return Nothing
        End Function

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        ' service date is after date of death    
        Private Function CheckServiceDateDecease(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel) As RuleResult
            'If Not udtEHSAccount.DeathRecord.IsDead Then Return Nothing
            If Not udtEHSPersonalInfo.Deceased Then Return Nothing

            'If udtEHSAccount.DeathRecord.IsDead(udtEHSTransaction.ServiceDate) Then
            If udtEHSPersonalInfo.IsDeceasedAsAt(EHSPersonalInformationModel.DODCalMethodClass.FIRSTDAYOFMONTHYEAR, udtEHSTransaction.ServiceDate) Then
                Return New RuleResult(RuleID.ServiceDateDecease)
            End If

            Return Nothing
        End Function
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

        ' 2  service date before SP / SP scheme enrol and effective date 
        Private Function CheckServiceDateSP(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtServiceProvider As ServiceProvider.ServiceProviderModel, ByVal udtSchemeInfoModelCollection As Common.Component.SchemeInformation.SchemeInformationModelCollection, ByVal udtPractice As Practice.PracticeModel, ByVal udtEHSTransactionDetails As TransactionDetailModelCollection) As RuleResultList

            Dim udtSchemeInfoModel As New Common.Component.SchemeInformation.SchemeInformationModel

            Dim udtRuleResultList As New RuleResultList()

            Dim dtmMinDate As New DateTime()
            Dim blnHasError As Boolean

            ' The date is a future date
            If DateDiff("d", New DateTime(Now.Year, Now.Month, Now.Day), udtEHSTransaction.ServiceDate) > 0 Then
                'Dim udtSystemMessage As New SystemMessage(cFunctionCode, "E", "00121")
                'udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.ServiceDateSPChecking, udtSystemMessage))
                udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.ServiceDateFuture))
            End If




            ' SP Effectice date
            If udtEHSTransaction.ServiceDate.Date < udtServiceProvider.EffectiveDtm.Value.Date Then
                'Dim udtSystemMessage As New SystemMessage(cFunctionCode, "E", "00263")
                blnHasError = True
            End If
            dtmMinDate = udtServiceProvider.EffectiveDtm.Value.Date

            ' No need to check SP scheme date. Now check practice scheme date
            'For Each udtScheme As SchemeInformation.SchemeInformationModel In udtSchemeInfoModelCollection.Values
            '    Dim strSchemeCodeEnrol As String = _SchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(udtEHSTransaction.SchemeCode.Trim())
            '    If udtScheme.SchemeCode.Trim() = strSchemeCodeEnrol.Trim() Then
            '        udtSchemeInfoModel = udtScheme
            '    End If
            'Next
            'If Not udtSchemeInfoModel Is Nothing Then
            '    If udtEHSTransaction.ServiceDate.Date < udtSchemeInfoModel.EffectiveDtm.Value.Date Then
            '        blnHasError = True
            '        If dtmMinDate.Date < udtSchemeInfoModel.EffectiveDtm.Value.Date Then
            '            dtmMinDate = udtSchemeInfoModel.EffectiveDtm.Value.Date
            '        End If
            '    End If
            'End If


            ' Check the practice scheme date   
            Dim strSchemeCode As String = ""
            For Each udtTransactionDetailModel As EHSTransaction.TransactionDetailModel In udtEHSTransactionDetails
                ' Check DayBack Date for min date of scheme
                Dim strDayBackMinDate As String = ""
                _udtGeneralFunction.getSystemParameter("DateBackClaimMinDate", strDayBackMinDate, String.Empty, udtTransactionDetailModel.SchemeCode)
                Dim dtmDayBackMinDate As DateTime = Convert.ToDateTime(strDayBackMinDate)

                If udtEHSTransaction.ServiceDate.Date < dtmDayBackMinDate.Date Then
                    blnHasError = True
                    If dtmDayBackMinDate.Date < dtmMinDate.Date Then
                        dtmMinDate = dtmDayBackMinDate.Date
                    End If
                End If


                Dim strSchemeCodeEnrol As String = _SchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(udtTransactionDetailModel.SchemeCode.Trim())

                Dim udtPracticeSchemeCollection As PracticeSchemeInfo.PracticeSchemeInfoModelCollection = udtPractice.PracticeSchemeInfoList.Filter(strSchemeCodeEnrol.Trim())
                Dim udtPracticeScheme As PracticeSchemeInfo.PracticeSchemeInfoModel = udtPracticeSchemeCollection.GetByIndex(0)

                ' if the claiming scheme for that practice is delisted

                Dim dtmPracticeSchemeEnrol As DateTime
                'If dtmPracticeSchemeEnrol > udtPracticeSchemeInfoModel.EffectiveDtm Then
                dtmPracticeSchemeEnrol = udtPracticeScheme.EffectiveDtm.Value.Date
                'End If

                If udtEHSTransaction.ServiceDate.Date < dtmPracticeSchemeEnrol.Date Then
                    If dtmMinDate.Date < dtmPracticeSchemeEnrol.Date Then
                        dtmMinDate = dtmPracticeSchemeEnrol.Date

                        blnHasError = True
                    End If
                End If
            Next

            If blnHasError Then
                'Dim udtSystemMessageMinDate As New SystemMessage(cFunctionCode, "E", "00150")
                'udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.DayBackLimitChecking, udtSystemMessageMinDate, "%s", _udtFormatter.formatDate(dtmMinDate, "en"), "%s", _udtFormatter.formatDate(dtmMinDate, "zh-tw")))
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.ServiceDateSPChecking, "%s", _udtFormatter.formatDate(dtmMinDate, "en"), "%s", _udtFormatter.formatDate(dtmMinDate, "zh-tw")))
                udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.ServiceDateSPChecking, "%s", _udtFormatter.formatDisplayDate(dtmMinDate, CultureLanguage.English), "%s", _udtFormatter.formatDisplayDate(dtmMinDate, CultureLanguage.TradChinese)))
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            End If

            Return udtRuleResultList
        End Function


        ' 3. Claim's service date later then permit to remain until of ID235B
        Private Function CheckServiceDateID235B(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel) As RuleResult
            If udtEHSPersonalInfo.DocCode = DocType.DocTypeModel.DocTypeCode.ID235B Then
                'If udtEHSTransaction.ServiceDate > udtEHSPersonalInfo.PermitToRemainUntil Then
                '    Dim udtSystemMessage As New SystemMessage(cFunctionCode, "E", "00262")
                '    Return New RuleResult(RuleID.ServiceDateID235BChecking, udtSystemMessage)
                'End If

                Dim udtSystemMessage As SystemMessage = _udtValidator.ChkServiceDatePermitToRemainUntil(udtEHSTransaction.ServiceDate, udtEHSPersonalInfo.PermitToRemainUntil) 'msg 00230
                If Not udtSystemMessage Is Nothing Then
                    'Return New RuleResult(RuleID.ServiceDateID235BChecking, udtSystemMessage)
                    Return New RuleResult(RuleID.ServiceDateID235BChecking)
                End If
            End If
            Return Nothing
        End Function

        ' 4 Check Day Back Limit
        Private Function CheckDayBackLimit(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtSchemeClaim As Scheme.SchemeClaimModel, ByVal udtSchemeInfoModel As SchemeInformation.SchemeInformationModel, ByVal udtServiceProvider As ServiceProvider.ServiceProviderModel, ByVal udtSchemeInfoModelCollection As Common.Component.SchemeInformation.SchemeInformationModelCollection) As RuleResultList
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction



            Dim udtRuleResultList As New RuleResultList()



            Dim strAllowDateBack As String = ""
            Dim strClaimDayLimit As String = ""
            Dim strMinDate As String = ""

            udtGeneralFunction.getSystemParameter("DateBackClaimAllow", strAllowDateBack, String.Empty, udtSchemeClaim.SchemeCode)
            If strAllowDateBack = "Y" Then

                udtGeneralFunction.getSystemParameter("DateBackClaimDayLimit", strClaimDayLimit, String.Empty, udtSchemeClaim.SchemeCode)
                udtGeneralFunction.getSystemParameter("DateBackClaimMinDate", strMinDate, String.Empty, udtSchemeClaim.SchemeCode)

                Dim intDayLimit As Integer = CInt(strClaimDayLimit)
                Dim dtmMinDate As DateTime = Convert.ToDateTime(strMinDate)

                ' To Do: ServiceDate should not before SP.SchemeInformation
                'Dim udtSystemMessage As SystemMessage = _udtValidator.chkDateBackClaimServiceDate(udtEHSTransaction.ServiceDate.ToString(), intDayLimit, dtmMinDate)
                If udtEHSTransaction.ServiceDate.AddDays(intDayLimit) <= DateTime.Now.Date Then

                    'Dim udtSystemMessage As New SystemMessage(cFunctionCode, "E", "00149")
                    'udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.DayBackLimitChecking, udtSystemMessage, "%s", strClaimDayLimit, "%s", strClaimDayLimit))
                    udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.DayBackLimitChecking, "%n", strClaimDayLimit, "%n", strClaimDayLimit))
                End If
            End If

            Return udtRuleResultList
        End Function

        ' 4 Exceed day-back limit          
        'Private Function CheckDayBackLimit(ByVal udtEHSTransaction As EHSTransactionModel) As RuleResult
        '    Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
        '    Dim strParm_Value1 As String = ""
        '    Dim strParm_Value2 As String = ""
        '    udtGeneralFunction.getSystemParameter("DateBackClaimDayLimit", strParm_Value1, strParm_Value2)
        '    If DateDiff(DateInterval.Day, udtEHSTransaction.ServiceDate, Date.Today) > CInt(strParm_Value1) Then
        '        'Dim udtSystemMessage As New SystemMessage(cFunctionCode, "E", "00264")
        '        Dim udtSystemMessage As New SystemMessage(cFunctionCode, "E", "00264")
        '        Return New RuleResult(RuleID.DayBackLimitChecking, udtSystemMessage)
        '    End If

        '    Return Nothing
        'End Function

        'Private Function CheckDayBackLimitAndSPServiceDate(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtSchemeClaim As Scheme.SchemeClaimModel, ByVal udtSchemeInfoModel As SchemeInformation.SchemeInformationModel, ByVal udtServiceProvider As ServiceProvider.ServiceProviderModel, ByVal udtSchemeInfoModelCollection As Common.Component.SchemeInformation.SchemeInformationModelCollection) As RuleResultList
        '    Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction



        '    Dim udtRuleResultList As New RuleResultList()

        '    ' The date is a future date
        '    If DateDiff("d", New DateTime(Now.Year, Now.Month, Now.Day), udtEHSTransaction.ServiceDate) > 0 Then
        '        Dim udtSystemMessage As New SystemMessage(cFunctionCode, "E", "00121")
        '        udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.DayBackLimitAndSPDateChecking, udtSystemMessage))
        '    End If


        '    Dim strAllowDateBack As String = ""
        '    Dim strClaimDayLimit As String = ""
        '    Dim strMinDate As String = ""

        '    udtGeneralFunction.getSystemParameter("DateBackClaimAllow", strAllowDateBack, String.Empty, udtSchemeClaim.SchemeCode)
        '    If strAllowDateBack = "Y" Then

        '        udtGeneralFunction.getSystemParameter("DateBackClaimDayLimit", strClaimDayLimit, String.Empty, udtSchemeClaim.SchemeCode)
        '        udtGeneralFunction.getSystemParameter("DateBackClaimMinDate", strMinDate, String.Empty, udtSchemeClaim.SchemeCode)

        '        Dim intDayLimit As Integer = CInt(strClaimDayLimit)
        '        Dim dtmMinDate As DateTime = Convert.ToDateTime(strMinDate)

        '        ' To Do: ServiceDate should not before SP.SchemeInformation

        '        ' ServiceDate should not before SP Enrol / Effective date
        '        If udtServiceProvider.EffectiveDtm.HasValue AndAlso udtServiceProvider.EffectiveDtm.Value > dtmMinDate Then
        '            dtmMinDate = udtServiceProvider.EffectiveDtm.Value.Date
        '        End If

        '        ' ServiceDate should not before SP Scheme Enrol / Effective date
        '        If Not udtSchemeInfoModel Is Nothing AndAlso udtSchemeInfoModel.EffectiveDtm.HasValue AndAlso udtSchemeInfoModel.EffectiveDtm.Value > dtmMinDate Then
        '            dtmMinDate = udtSchemeInfoModel.EffectiveDtm.Value.Date
        '        End If

        '        Dim udtSystemMessage As SystemMessage = _udtValidator.chkDateBackClaimServiceDate(udtEHSTransaction.ServiceDate.ToString(), intDayLimit, dtmMinDate)

        '        If Not udtSystemMessage Is Nothing Then
        '            If udtSystemMessage.MessageCode = "00149" Then
        '                udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.DayBackLimitAndSPDateChecking, udtSystemMessage, "%s", strClaimDayLimit, "%s", strClaimDayLimit))
        '            ElseIf udtSystemMessage.MessageCode = "00150" Then
        '                udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.DayBackLimitAndSPDateChecking, udtSystemMessage, "%s", _udtFormatter.formatDate(dtmMinDate, "en"), "%s", _udtFormatter.formatDate(dtmMinDate, "zh-tw")))
        '            Else
        '                udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.DayBackLimitAndSPDateChecking, udtSystemMessage))
        '            End If
        '        End If
        '    Else
        '    End If

        '    Return udtRuleResultList
        'End Function

        ' 5 Validated eHealth account with status not active 
        Private Function CheckAccountStatus(ByVal udtEHSAccount As EHSAccountModel) As RuleResult
            ' CRE11-007
            ' Death Record Entry override account status
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            'If udtEHSAccount.DeathRecord.IsDead Then
            If udtEHSAccount.Deceased Then
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
                Return New RuleResult(RuleID.DeathDecease)
            End If

            If udtEHSAccount.RecordStatus.Equals(EHSAccountModel.ValidatedAccountRecordStatusClass.Suspended) Then
                'Dim udtSystemMessage As New SystemMessage(cFunctionCode, "E", "00108")
                'Return New RuleResult(RuleID.AccountStatusChecking, udtSystemMessage)
                Return New RuleResult(RuleID.ACStatusSuspend)

                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
            ElseIf udtEHSAccount.RecordStatus.Equals(EHSAccountModel.ValidatedAccountRecordStatusClass.Terminated) Then
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
                Return New RuleResult(RuleID.ACStatusDecease) ' <- Terminate Status
            End If

            Return Nothing
        End Function

        ' 6 SP make claims for themselves 
        Private Function IsSPClaimForThemselves(ByVal strSPHKID As String, ByVal strRecipientDocCode As String, _
                                                ByVal strRecipientIdentityNum As String, _
                                                ByVal enumSubPlatform As SchemeClaimModel.EnumAvailableHCSPSubPlatform) As RuleResult
            Dim udtClaimRulesBLL As New ClaimRulesBLL()

            If udtClaimRulesBLL.IsSPClaimForThemselves(strSPHKID, strRecipientDocCode, strRecipientIdentityNum, enumSubPlatform) <> String.Empty Then
                Return New RuleResult(RuleID.SelfClaim)
            End If

            Return Nothing
        End Function

        ' 6.1 SP with status not active      
        Private Function CheckSPStatus(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtServiceProvider As ServiceProvider.ServiceProviderModel, ByVal udtSchemeInfoModelCollection As Common.Component.SchemeInformation.SchemeInformationModelCollection, ByVal udtEHSTransactionDetails As EHSTransaction.TransactionDetailModelCollection) As RuleResult
            'If Not udtServiceProvider.RecordStatus = ServiceProviderStatus.Active Then
            '    'Dim udtSystemMessage As New SystemMessage(cFunctionCode, "E", "00260")
            '    'Return New RuleResult(RuleID.SPStatusChecking, udtSystemMessage)
            '    Return New RuleResult(RuleID.SPInactive)
            'End If
            Dim Formatter As New Format.Formatter

            If udtServiceProvider.RecordStatus = ServiceProviderStatus.Delisted Then

                'Dim dtmSPDelist As DateTime
                'If Not udtSchemeInfoModelCollection Is Nothing And Not udtSchemeInfoModelCollection.Values Is Nothing Then
                '    Dim udtScheme As SchemeInformation.SchemeInformationModel = udtSchemeInfoModelCollection.GetByIndex(0)
                '    dtmSPDelist = udtScheme.DelistDtm
                '    For Each udtSchemeLoop As SchemeInformation.SchemeInformationModel In udtSchemeInfoModelCollection.Values
                '        If udtSchemeLoop.DelistDtm > dtmSPDelist Then
                '            dtmSPDelist = udtSchemeLoop.DelistDtm
                '        End If
                '    Next
                '    Return New RuleResult(RuleID.SPDelist, "%date", Formatter.formatDate(dtmSPDelist, "en"), "%date", Formatter.formatDate(dtmSPDelist, "zh-tw"), Nothing, Nothing, Nothing, Nothing)
                'Else
                '    Return New RuleResult(RuleID.SPDelist)
                'End If

                Return New RuleResult(RuleID.SPDelist)
            ElseIf udtServiceProvider.RecordStatus = ServiceProviderStatus.Suspended Then
                Return New RuleResult(RuleID.SPSuspend)
                'ElseIf Not udtServiceProvider.RecordStatus = ServiceProviderStatus.Active Then
                '    Return New RuleResult(RuleID.SPInactive)

                'CRE15-004 (TIV and QIV) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Else

                '    For Each udtTransactionDetailModel As EHSTransaction.TransactionDetailModel In udtEHSTransactionDetails
                '        Dim strSchemeCodeEnrol As String = _SchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(udtTransactionDetailModel.SchemeCode.Trim())
                '        Dim udtSPScheme As SchemeInformation.SchemeInformationModel = udtServiceProvider.SchemeInfoList.Filter(strSchemeCodeEnrol.Trim())
                '        If udtSPScheme.DelistDtm.HasValue Or udtSPScheme.RecordStatus = "W" Or udtSPScheme.RecordStatus = "V" Or udtSPScheme.RecordStatus = "I" Or udtSPScheme.RecordStatus = "X" Or udtSPScheme.RecordStatus = "Y" Then
                '            Return New RuleResult(RuleID.SPSchemeInactive)
                '        End If
                '    Next
                'CRE15-004 (TIV and QIV) [End][Chris YIM]
            End If

            Return Nothing
        End Function

        ' 6.2 Practice with status not active      
        Private Function CheckPracticeStatus(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtPractice As Practice.PracticeModel, ByVal udtSchemeInfoModelCollection As Common.Component.SchemeInformation.SchemeInformationModelCollection, ByVal udtEHSTransactionDetails As EHSTransaction.TransactionDetailModelCollection) As RuleResultList

            'CRE15-004 (TIV and QIV) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim udtRuleResultList As New RuleResultList()
            'CRE15-004 (TIV and QIV) [End][Chris YIM]

            Dim Formatter As New Format.Formatter

            'If Not udtPractice.RecordStatus = PracticeStatus.Active Then
            '    'Dim udtSystemMessage As New SystemMessage(cFunctionCode, "E", "00261")
            '    'Return New RuleResult(RuleID.SPStatusChecking, udtSystemMessage)
            '    Return New RuleResult(RuleID.PracticeInactive)
            'End If

            If udtPractice.RecordStatus = PracticeStatus.Delisted Then
                ' no need to give delist datetime in message
                'Dim dtmPracticeDelist As DateTime
                'Dim udtPracticeScheme As PracticeSchemeInfo.PracticeSchemeInfoModel = udtPractice.PracticeSchemeInfoList.GetByIndex(0)
                'dtmPracticeDelist = udtPracticeScheme.DelistDtm
                'Dim udtPracticeSchemeInfoCollection As PracticeSchemeInfo.PracticeSchemeInfoModelCollection = udtPractice.PracticeSchemeInfoList
                'If Not udtPracticeSchemeInfoCollection Is Nothing And Not udtPracticeSchemeInfoCollection.Values Is Nothing Then
                '    For Each udtPracticeSchemeInfoLoop As PracticeSchemeInfo.PracticeSchemeInfoModel In udtPracticeSchemeInfoCollection.Values
                '        If udtPracticeSchemeInfoLoop.DelistDtm > dtmPracticeDelist Then
                '            dtmPracticeDelist = udtPracticeSchemeInfoLoop.DelistDtm
                '        End If
                '    Next
                '    ' Return New RuleResult(RuleID.PracticeDelist, "%date", Formatter.formatDate(dtmPracticeDelist, "en"), "%date", Formatter.formatDate(dtmPracticeDelist, "zh-tw"), Nothing, Nothing, Nothing, Nothing)
                'Else
                '    Return New RuleResult(RuleID.PracticeDelist)
                'End If

                'Return New RuleResult(RuleID.PracticeDelist)
                udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.PracticeDelist))
                'CRE15-004 (TIV and QIV) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.PracticeSchemeDelist))
                'CRE15-004 (TIV and QIV) [End][Chris YIM]
            ElseIf udtPractice.RecordStatus = PracticeStatus.Suspended Then
                'Return New RuleResult(RuleID.PracticeSuspend)
                udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.PracticeSuspend))
            Else
                ' Check if the claiming scheme is delisted                 
                For Each udtTransactionDetailModel As EHSTransaction.TransactionDetailModel In udtEHSTransactionDetails
                    Dim strSchemeCodeEnrol As String = _SchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(udtTransactionDetailModel.SchemeCode.Trim())
                    Dim udtPracticeSchemeCollection As PracticeSchemeInfo.PracticeSchemeInfoModelCollection = udtPractice.PracticeSchemeInfoList.Filter(strSchemeCodeEnrol.Trim())
                    Dim udtPracticeScheme As PracticeSchemeInfo.PracticeSchemeInfoModel = udtPracticeSchemeCollection.GetByIndex(0)

                    ' if the claiming scheme for that practice is delisted
                    '                    If udtPracticeScheme.DelistDtm.HasValue Or udtPracticeScheme.RecordStatus = "W" Or udtPracticeScheme.RecordStatus = "V" Or udtPracticeScheme.RecordStatus = "I" Or udtPracticeScheme.RecordStatus = "X" Or udtPracticeScheme.RecordStatus = "Y" Then
                    If udtPracticeScheme.DelistDtm.HasValue Or udtPracticeScheme.RecordStatus = "W" Or udtPracticeScheme.RecordStatus = "V" Or udtPracticeScheme.RecordStatus = "I" Or udtPracticeScheme.RecordStatus = "XP" Or udtPracticeScheme.RecordStatus = "YP" Then
                        udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.PracticeSchemeDelist))
                    End If
                Next
            End If

            'udtServiceProvider.UpdateDtm
            'udtPractice.UpdateDtm

            'CRE15-004 (TIV and QIV) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            If udtRuleResultList.RuleResults.Count > 0 Then
                Return udtRuleResultList
            Else
                Return Nothing
            End If
            'CRE15-004 (TIV and QIV) [End][Chris YIM]

        End Function

        ' 7 Patient・s age outside document・s accepted age   

        Private Function CheckDocumentAcceptedAge(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, ByVal udtSchemeClaimModel As SchemeClaimModel) As RuleResult
            Dim blnExceedLimit As Boolean = False
            Dim udtDocTypeBLL As New DocTypeBLL()

            Dim udtDocTypeList As DocTypeModelCollection = udtDocTypeBLL.getAllDocType()

            If udtDocTypeList.Count > 0 Then
                Dim udtDocTypeModel As DocTypeModel = udtDocTypeList.Filter(udtEHSTransaction.DocCode)
                If Not udtDocTypeModel Is Nothing Then
                    If udtDocTypeModel.AgeUpperLimit.HasValue OrElse udtDocTypeModel.AgeLowerLimit.HasValue Then
                        Dim dtmPassDOB As Date = EHSClaimValidationBLL.ConvertDateOfBirthByCalMethod(udtDocTypeModel.AgeCalMethod, udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB)

                        If udtDocTypeModel.AgeLowerLimit.HasValue Then
                            Dim intPassValue As Integer = EHSClaimValidationBLL.ConvertPassValueByCalUnit(udtDocTypeModel.AgeLowerLimitUnit, dtmPassDOB, udtEHSTransaction.ServiceDate)
                            blnExceedLimit = blnExceedLimit Or (intPassValue < udtDocTypeModel.AgeLowerLimit.Value)
                        End If

                        If udtDocTypeModel.AgeUpperLimit.HasValue Then
                            Dim intPassValue As Integer = EHSClaimValidationBLL.ConvertPassValueByCalUnit(udtDocTypeModel.AgeUpperLimitUnit, dtmPassDOB, udtEHSTransaction.ServiceDate)
                            blnExceedLimit = blnExceedLimit Or (intPassValue >= udtDocTypeModel.AgeUpperLimit.Value)
                        End If
                    End If

                End If
            End If

            If blnExceedLimit Then
                ' CRE17-018-07 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
                ' --------------------------------------------------------------------------------------
                If udtEHSPersonalInfo.DocCode = DocTypeModel.DocTypeCode.EC Then
                    Return New RuleResult(RuleID.DocExceedEC, Nothing, Nothing, Nothing, Nothing, _
                                          udtEHSTransaction.SchemeCode, _
                                          udtEHSTransaction.TransactionDetails(0).SchemeSeq, _
                                          udtEHSTransaction.TransactionDetails(0).SubsidizeCode)
                Else
                    Return New RuleResult(RuleID.DocExceedOther, Nothing, Nothing, Nothing, Nothing, _
                                          udtEHSTransaction.SchemeCode, _
                                          udtEHSTransaction.TransactionDetails(0).SchemeSeq, _
                                          udtEHSTransaction.TransactionDetails(0).SubsidizeCode)
                End If
                ' CRE17-018-07 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

            End If

            Return Nothing
        End Function

        ' 8 No available subsidies    
        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ' Add input parm - [udtPractice]
        Private Function CheckAvailableSubsidyNew(ByVal strCategoryCode As String, ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, ByVal udtSchemeClaimModel As SchemeClaimModel, ByVal udtEHSAccount As EHSAccountModel, ByVal udtClaimingTransactionDetails As TransactionDetailModelCollection, ByVal udtBenefitTransactionDetailList As TransactionDetailModelCollection, ByVal blnIsEligible As Boolean, ByVal intCurrentTransactionClaimedVoucher As Integer, ByVal udtPractice As Practice.PracticeModel) As RuleResultList

            Dim udtRuleResultList As New RuleResultList()

            If _SchemeClaimBLL.ConvertControlTypeFromSchemeClaimCode(udtEHSTransaction.SchemeCode) = SchemeClaimModel.EnumControlType.VOUCHER Then
                ' don't check voucher entitlement if eligible
                If blnIsEligible Then
                    If CheckAvailableVoucher(strCategoryCode, udtEHSTransaction, udtEHSPersonalInfo, udtSchemeClaimModel, udtEHSAccount, udtEHSTransaction.TransactionDetails, udtBenefitTransactionDetailList, intCurrentTransactionClaimedVoucher) = False Then
                        udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.AvailableSubsidyNoVoucher))
                    End If

                    ' CRE19-003 (Opt voucher capping) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    ' Quota Checking, bypass checking when dead
                    Dim blnIsDead As Boolean = False

                    If udtEHSPersonalInfo.Deceased = True Then
                        'Dead, check date of death
                        Dim dtmDOD As Date = udtEHSPersonalInfo.LogicalDOD(EHSAccountModel.EHSPersonalInformationModel.DODCalMethodClass.LASTDAYOFMONTHYEAR)

                        If dtmDOD < udtEHSTransaction.ServiceDate Then
                            blnIsDead = True
                        End If
                    End If

                    If Not blnIsDead Then
                        If CheckAvailableVoucherQuota(udtEHSTransaction, udtEHSPersonalInfo, udtSchemeClaimModel, intCurrentTransactionClaimedVoucher) = False Then

                            Dim strMsg_en = String.Format(HttpContext.GetGlobalResourceObject("Text", "ProfessionQuota", New System.Globalization.CultureInfo(CultureLanguage.English)) _
                                                          , HttpContext.GetGlobalResourceObject("Text", udtEHSTransaction.ServiceType.Trim, New System.Globalization.CultureInfo(CultureLanguage.English)))

                            Dim strMsg_tc = String.Format(HttpContext.GetGlobalResourceObject("Text", "ProfessionQuota", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)) _
                                                          , HttpContext.GetGlobalResourceObject("Text", udtEHSTransaction.ServiceType.Trim, New System.Globalization.CultureInfo(CultureLanguage.TradChinese)))

                            Dim udtRuleResult As RuleResult = New RuleResult(RuleID.AvailableSubsidyNoVoucherQuota, "%en", strMsg_en, "%tc", strMsg_tc)

                            udtRuleResultList.RuleResults.Add(udtRuleResult)
                        End If
                    End If
                    ' CRE19-003 (Opt voucher capping) [End][Winnie]

                    ' CRE19-006 (DHC) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    ' DHC-related Services
                    If CheckProvideDHCService(udtEHSTransaction, udtPractice) = False Then
                        Dim udtRuleResult As RuleResult = New RuleResult(RuleID.DHCServiceNotProvided)
                        udtRuleResultList.RuleResults.Add(udtRuleResult)
                    End If

                    Dim intMaxClaimAmt As Integer
                    If CheckExceedDHCMaxClaimAmt(udtEHSTransaction, intMaxClaimAmt) = False Then
                        Dim udtRuleResult As RuleResult = New RuleResult(RuleID.VoucherExceedDHCMaxClaimAmt, "%s", (New Common.Format.Formatter).formatMoney(intMaxClaimAmt.ToString(), True))
                        udtRuleResultList.RuleResults.Add(udtRuleResult)
                    End If
                    ' CRE19-006 (DHC) [End][Winnie]

                End If

                ' CRE13-001 - EHAPP [Start][Tommy L]
                ' -------------------------------------------------------------------------------------
            ElseIf _SchemeClaimBLL.ConvertControlTypeFromSchemeClaimCode(udtEHSTransaction.SchemeCode) = SchemeClaimModel.EnumControlType.VOUCHERCHINA Then
                ' Same as EnumControlType.VOUCHER
                If blnIsEligible Then
                    If CheckAvailableVoucher(strCategoryCode, udtEHSTransaction, udtEHSPersonalInfo, udtSchemeClaimModel, udtEHSAccount, udtEHSTransaction.TransactionDetails, udtBenefitTransactionDetailList, intCurrentTransactionClaimedVoucher) = False Then
                        'Dim udtSystemMessage As New SystemMessage(cFunctionCode, "E", "00107")
                        'udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.AvailableSubsidyChecking, udtSystemMessage))
                        udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.AvailableSubsidyNoVoucher))
                    Else
                        ' additional checking for EnumControlType.VOUCHERCHINA
                        If CheckAvailableVoucherRMB(strCategoryCode, udtEHSTransaction, udtEHSPersonalInfo, udtSchemeClaimModel, udtEHSAccount, udtEHSTransaction.TransactionDetails, udtBenefitTransactionDetailList, intCurrentTransactionClaimedVoucher) = False Then
                            'Dim udtSystemMessage As New SystemMessage(cFunctionCode, "E", "00107")
                            'udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.AvailableSubsidyChecking, udtSystemMessage))
                            udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.AvailableSubsidyNoVoucher))
                        End If

                    End If

                End If

            ElseIf _SchemeClaimBLL.ConvertControlTypeFromSchemeClaimCode(udtEHSTransaction.SchemeCode) = SchemeClaimModel.EnumControlType.EHAPP Then

                If blnIsEligible Then
                    If Not CheckAvailableSubsidizeItem_EHAPP(udtSchemeClaimModel, udtBenefitTransactionDetailList) Then
                        udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.AvailableSubsidyNoVoucher))
                    End If
                End If
                ' CRE13-001 - EHAPP [End][Tommy L]

            Else
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                ' Move to ClaimRule
                'If Me.IsSameVaccineEntitlementTaken(strCategoryCode, udtEHSTransaction, udtEHSPersonalInfo, udtSchemeClaimModel, udtEHSAccount, udtEHSClaimVaccine, udtEHSTransaction.TransactionDetails, udtBenefitTransactionDetailList) = True Then
                '    'Dim udtSystemMessage As New SystemMessage(cFunctionCode, "E", "00107")
                '    'udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.AvailableSubsidyChecking, udtSystemMessage))
                '    udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.AvailableSubsidyVaccineSameTaken))
                'End If
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

                If Me.IsClaimingVaccineExceedEntitlement(udtEHSTransaction, udtEHSPersonalInfo, udtSchemeClaimModel, udtEHSAccount, udtEHSTransaction.TransactionDetails, udtBenefitTransactionDetailList) = True Then
                    'Dim udtSystemMessage As New SystemMessage(cFunctionCode, "E", "00107")
                    'udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.AvailableSubsidyChecking, udtSystemMessage))
                    udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.AvailableSubsidyVaccineExceedEntitlement))
                End If

                If blnIsEligible Then
                    If Me.IsClaimingVaccineNotEntitled(udtEHSTransaction, udtEHSPersonalInfo, udtSchemeClaimModel, udtEHSAccount, udtEHSTransaction.TransactionDetails, udtBenefitTransactionDetailList) = True Then
                        udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.AvailableSubsidyVaccineNotEntitled))
                    End If
                End If
            End If

            Return udtRuleResultList
        End Function


        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Private Function CheckAvailableVoucher(ByVal strCategoryCode As String, _
                                               ByVal udtEHSTransaction As EHSTransactionModel, _
                                               ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, _
                                               ByVal udtSchemeClaimModel As SchemeClaimModel, _
                                               ByVal udtEHSAccount As EHSAccountModel, _
                                               ByVal udtClaimingTransactionDetails As TransactionDetailModelCollection, _
                                               ByVal udtBenefitTransactionDetailList As TransactionDetailModelCollection, _
                                               ByVal intCurrentTransactionClaimedVoucher As Integer) As Boolean

            ' 1. Get remaining Subsidize (if no remaining subsidize: No available Subsidy)
            ' 2. Compare remaining subsidize vs claiming subsidize (if no remaining subsidize for claiming subsidize)
            ' Check Available Subsidy (Voucher):

            Dim udtVoucherInfo As New VoucherInfoModel(VoucherInfoModel.AvailableVoucher.Include, VoucherInfoModel.AvailableQuota.None)

            udtVoucherInfo.GetInfo(udtEHSTransaction.ServiceDate, udtSchemeClaimModel, udtEHSPersonalInfo)

            Dim intAvailableVoucher As Integer = udtVoucherInfo.GetAvailableVoucher()

            If intAvailableVoucher > 0 Then
                intAvailableVoucher = intAvailableVoucher - intCurrentTransactionClaimedVoucher
            Else
                Return False
            End If

            If intAvailableVoucher > 0 Then
                If (intAvailableVoucher - udtEHSTransaction.VoucherClaim) < 0 Then
                    Return False
                Else
                    Return True
                End If
            Else
                Return False
            End If

        End Function
        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Private Function CheckAvailableVoucherRMB(ByVal strCategoryCode As String, _
                                                  ByVal udtEHSTransaction As EHSTransactionModel, _
                                                  ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, _
                                                  ByVal udtSchemeClaimModel As SchemeClaimModel, _
                                                  ByVal udtEHSAccount As EHSAccountModel, _
                                                  ByVal udtClaimingTransactionDetails As TransactionDetailModelCollection, _
                                                  ByVal udtBenefitTransactionDetailList As TransactionDetailModelCollection, _
                                                  ByVal intCurrentTransactionClaimedVoucher As Integer) As Boolean
            ' 1. Get remaining Subsidize (if no remaining subsidize: No available Subsidy)
            ' 2. Compare remaining subsidize vs claiming subsidize (if no remaining subsidize for claiming subsidize)
            ' Check Available Subsidy (Voucher):

            Dim udtVoucherInfo As New VoucherInfoModel(VoucherInfoModel.AvailableVoucher.Include, VoucherInfoModel.AvailableQuota.None)

            udtVoucherInfo.GetInfo(udtEHSTransaction.ServiceDate, udtSchemeClaimModel, udtEHSPersonalInfo)

            Dim intAvailableVoucher As Integer = udtVoucherInfo.GetAvailableVoucher()

            Dim dblAvailableVoucherRMB As Decimal

            If intAvailableVoucher > 0 Then
                intAvailableVoucher = intAvailableVoucher - intCurrentTransactionClaimedVoucher
                dblAvailableVoucherRMB = (New ExchangeRate.ExchangeRateBLL).CalculateHKDtoRMB(intAvailableVoucher, udtEHSTransaction.ExchangeRate)
            Else
                Return False
            End If

            If dblAvailableVoucherRMB > 0 Then
                If (dblAvailableVoucherRMB - udtEHSTransaction.VoucherClaimRMB) < 0 Then
                    Return False
                Else
                    Return True
                End If
            Else
                Return False
            End If

        End Function
        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

        ' CRE19-003 (Opt voucher capping) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Private Function CheckAvailableVoucherQuota(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, ByVal udtSchemeClaimModel As SchemeClaimModel, ByVal intCurrentTransactionClaimedVoucher As Integer) As Boolean
            ' 1. Get remaining Subsidize (if no remaining subsidize: No available Subsidy quota)
            ' 2. Compare remaining subsidize quota vs claiming subsidize (if no remaining subsidize quota for claiming subsidize)

            ' Check Available Voucher Quota
            Dim udtVoucherInfo As New VoucherInfoModel()

            Dim udtVoucherQuota As VoucherQuotaModel = udtVoucherInfo.GetVoucherQuota(udtEHSTransaction.ServiceDate, udtSchemeClaimModel, udtEHSPersonalInfo, udtEHSTransaction.ServiceType)
            
            If Not udtVoucherQuota Is Nothing Then
                Dim intAvailableVoucherQuota As Integer = udtVoucherQuota.AvailableQuota

                If intAvailableVoucherQuota > 0 Then
                    intAvailableVoucherQuota = intAvailableVoucherQuota - intCurrentTransactionClaimedVoucher
                Else
                    Return False
                End If

                If intAvailableVoucherQuota > 0 Then
                    If (intAvailableVoucherQuota - udtEHSTransaction.VoucherClaim) < 0 Then
                        Return False
                    Else
                        Return True
                    End If
                Else
                    Return False
                End If

            Else
                Return True
            End If

        End Function
        ' CRE19-003 (Opt voucher capping) [End][Winnie]

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Private Function CheckProvideDHCService(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtPractice As Practice.PracticeModel) As Boolean
            Dim blnValid As Boolean = False

            If udtEHSTransaction.DHCService = YesNo.Yes Then
                Dim udtProfessional As Professional.ProfessionalModel = udtPractice.Professional
                Dim udtProfessionalBLL As New Professional.ProfessionalBLL

                If udtProfessionalBLL.CheckDHCSPMapping(udtProfessional.ServiceCategoryCode, udtProfessional.RegistrationCode) Then
                    blnValid = True
                End If
            Else
                blnValid = True
            End If

            Return blnValid
        End Function

        Private Function CheckExceedDHCMaxClaimAmt(ByVal udtEHSTransaction As EHSTransactionModel, ByRef intMaxClaimAmt As Integer) As Boolean
            Dim blnValid As Boolean = False
            Dim blnCheckDHCMaxClaimAmt As Boolean = False

            If udtEHSTransaction.SchemeCode.Trim = SchemeClaimModel.HCVS Then
                If udtEHSTransaction.DHCService = YesNo.Yes Then
                    blnCheckDHCMaxClaimAmt = True
                End If

            ElseIf udtEHSTransaction.SchemeCode.Trim = SchemeClaimModel.HCVSDHC Then
                blnCheckDHCMaxClaimAmt = True
            End If

            If blnCheckDHCMaxClaimAmt Then
                Dim udtProfessionDHC As Profession.ProfessionDHCModel = Nothing

                udtProfessionDHC = Profession.ProfessionBLL.GetProfessionDHCByServiceCategoryCode(udtEHSTransaction.ServiceType)

                If Not udtProfessionDHC Is Nothing Then

                    ' Check Voucher claim amount + Net Service Fee > DHC Max Claim
                    If udtEHSTransaction.VoucherClaim + udtEHSTransaction.TransactionAdditionFields.CoPaymentFee.Value > udtProfessionDHC.MaxClaimAmt Then
                        intMaxClaimAmt = udtProfessionDHC.MaxClaimAmt
                        blnValid = False

                    Else
                        blnValid = True
                    End If

                Else
                    blnValid = True
                End If

            Else
                blnValid = True
            End If

            Return blnValid
        End Function
        ' CRE19-006 (DHC) [End][Winnie]

        ' CRE13-001 - EHAPP [Start][Tommy L]
        ' -------------------------------------------------------------------------------------
        Private Function CheckAvailableSubsidizeItem_EHAPP(ByVal udtSchemeClaimModel As SchemeClaimModel, ByVal udtBenefitTransactionDetailList As TransactionDetailModelCollection) As Boolean
            Dim udtSchemeDetailBLL As New SchemeDetailBLL

            Dim udtSubsidizeGroupClaim As SubsidizeGroupClaimModel = udtSchemeClaimModel.SubsidizeGroupClaimList(0)
            Dim udtSubsidizeItemDetailList As SubsidizeItemDetailsModelCollection = udtSchemeDetailBLL.getSubsidizeItemDetails(udtSubsidizeGroupClaim.SubsidizeItemCode)
            Dim udtSubsidizeItemDetail As SubsidizeItemDetailsModel = udtSubsidizeItemDetailList(0)

            Dim udtTransDetailBenefitListByAvailItem As TransactionDetailModelCollection

            Dim intNumSubsidize_Total As Integer
            Dim intNumSubsidize_Used As Integer

            intNumSubsidize_Total = udtSubsidizeItemDetail.AvailableItemNum

            intNumSubsidize_Used = 0
            udtTransDetailBenefitListByAvailItem = udtBenefitTransactionDetailList.FilterBySubsidizeItemDetail(udtSchemeClaimModel.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, udtSubsidizeGroupClaim.SubsidizeItemCode, udtSubsidizeItemDetail.AvailableItemCode)
            For Each udtTransDetailBenefitByAvailItem As TransactionDetailModel In udtTransDetailBenefitListByAvailItem
                intNumSubsidize_Used += udtTransDetailBenefitByAvailItem.Unit.Value
            Next

            If intNumSubsidize_Total > intNumSubsidize_Used Then
                ' Subsidies is Available
                Return True
            Else
                ' No Available Subsidies
                Return False
            End If
        End Function
        ' CRE13-001 - EHAPP [End][Tommy L]

        Private Function CheckPracticeEnrolScheme(ByVal udtPractice As Practice.PracticeModel, ByVal strSchemeCode As String)
            ' Check if that practice enrolled that shcme
            Dim strSchemeCodeEnrol As String = _SchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(strSchemeCode.Trim())
            Dim udtPracticeSchemeCollection As PracticeSchemeInfo.PracticeSchemeInfoModelCollection = udtPractice.PracticeSchemeInfoList.Filter(strSchemeCodeEnrol.Trim())
            If udtPracticeSchemeCollection Is Nothing OrElse udtPracticeSchemeCollection.Count = 0 Then
                Return New RuleResult(RuleID.PracNotEnrolScheme)
            End If

            Return Nothing
        End Function

        Private Function CheckPracticeJoinedProf(ByVal udtPractice As Practice.PracticeModel, ByVal strTransServiceType As String)
            '' Check if SP + Practice joined the profession 
            Dim blnProfCodeExist = False

            ' Dim udtProfessional As Common.Component.Professional.ProfessionalModel = udtProfessionalModelCollection.Item(udtPractice.ProfessionalSeq)
            If Not udtPractice.Professional Is Nothing AndAlso udtPractice.Professional.ServiceCategoryCode = strTransServiceType Then
                blnProfCodeExist = True
            End If

            If blnProfCodeExist = False Then
                Return New RuleResult(RuleID.PracJoinedProf)
            End If

            Return Nothing
        End Function

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Private Function CheckSchemeSubsidy(ByVal udtEHSTransaction As EHSTransactionModel)

            ' Check if it is a valid subsidy item
            If Not _SchemeClaimBLL.ConvertControlTypeFromSchemeClaimCode(udtEHSTransaction.SchemeCode) = SchemeClaimModel.EnumControlType.VOUCHER AndAlso _
                Not _SchemeClaimBLL.ConvertControlTypeFromSchemeClaimCode(udtEHSTransaction.SchemeCode) = SchemeClaimModel.EnumControlType.VOUCHERCHINA Then
                'Dim udtSchemeDetailBLL As New SchemeDetailBLL()
                Dim udtSchemeClaimBLL As New SchemeClaimBLL()
                If udtEHSTransaction.TransactionDetails Is Nothing OrElse udtEHSTransaction.TransactionDetails.Count <= 0 Then
                    Return New RuleResult(RuleID.SchemeSubsidy)
                Else
                    For Each udtTransDetail As EHSTransaction.TransactionDetailModel In udtEHSTransaction.TransactionDetails
                        'Dim udtSchemeVaccineDetailModel = udtSchemeDetailBLL.getSchemeVaccineDetail(udtEHSTransaction.SchemeCode, udtTransDetail.SchemeSeq, udtTransDetail.SubsidizeCode)
                        'If udtSchemeVaccineDetailModel Is Nothing Then
                        Dim udtSubsidizeFeeModel = udtSchemeClaimBLL.getAllSubsidizeFee().Filter(udtEHSTransaction.SchemeCode, udtTransDetail.SchemeSeq, udtTransDetail.SubsidizeCode, udtEHSTransaction.ServiceDate)
                        If udtSubsidizeFeeModel Is Nothing Then
                            Return New RuleResult(RuleID.SchemeSubsidy)
                        End If
                    Next
                End If
            End If

            Return Nothing
        End Function
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        'Private Function CheckAvailableSubsidy(ByVal strCategoryCode As String, ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, ByVal udtSchemeClaimModel As SchemeClaimModel, ByVal udtEHSAccount As EHSAccountModel, ByVal udtEHSClaimVaccine As EHSClaimVaccine.EHSClaimVaccineModel, ByVal udtClaimingTransactionDetails As TransactionDetailModelCollection, ByVal udtBenefitTransactionDetailList As TransactionDetailModelCollection) As RuleResult

        '    ' 1. Get remaining Subsidize (if no remaining subsidize: No available Subsidy)
        '    ' 2. Compare remaining subsidize vs claiming subsidize (if no remaining subsidize for claiming subsidize)


        '    ' Check Available Subsidy (Voucher):

        '    If udtEHSTransaction.SchemeCode = Scheme.SchemeClaimModel.HCVS Then
        '        Dim udtEHSTransactionBLL As New EHSTransactionBLL()
        '        If Not udtEHSAccount.AvailableVoucher.HasValue Then
        '            udtEHSAccount.AvailableVoucher = udtEHSTransactionBLL.getAvailableVoucher(udtSchemeClaimModel, udtEHSPersonalInfo)
        '        End If

        '        ' To Do: AvailableVoucher - Claiming # of Voucher <=0                              

        '        If (udtEHSAccount.AvailableVoucher.Value - udtEHSTransaction.VoucherClaim) < 0 Then
        '            'Dim udtSystemMessage As New SystemMessage(cFunctionCode, "E", "00107")
        '            'Return New RuleResult(RuleID.AvailableSubsidyChecking, udtSystemMessage)
        '            Return New RuleResult(RuleID.AvailableSubsidyNoVoucher)
        '        End If
        '    End If


        '    ' Check Available Subsidy (Vaccine):
        '    ' -----------------------------------
        '    'If udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVaccine Then
        '    If udtEHSTransaction.SchemeCode = Scheme.SchemeClaimModel.EVSS Or udtEHSTransaction.SchemeCode = Scheme.SchemeClaimModel.CIVSS Or udtEHSTransaction.SchemeCode = Scheme.SchemeClaimModel.HSIVSS Or udtEHSTransaction.SchemeCode = Scheme.SchemeClaimModel.RVP Then

        '        ' 1. Compare claiming information vs claimed information 
        '        ' 2. Compare claiming information vs Is granted ?
        '        ' 1. Used Benefit: 
        '        ' 2. By Service date, compile the subsidize list is available for the patient

        '        'For Case 2:

        '        ' Loop Though claiming subsidy for checking (in TransactionDetail of udtEHSTransaction)
        '        ' Claimed -> SchemeCode Scheme Seq, SubsidizeCode -> SubsidizeItemCode: CIV, AvailableItemCode: 1STDOSE

        '        ' 
        '        ' Check Granted?
        '        ' -> EligibilityChecking + Service Checking
        '        ' 
        '        ' Get Vaccine Dose for the specific subisidize
        '        ' If claiming Dose, do not exist in the Vaccine Dose List
        '        ' Else ,exist,  Check Dose Rule Whether dose, granted ? 

        '        'getSubsidizeItemDetails (Check granted Subsidize, SubidizeItemCode + Available Code



        '        Dim SchemeDetailBLL As New SchemeDetailBLL()
        '        Dim ClaimRulesBLL As New ClaimRulesBLL()
        '        For i As Integer = 0 To udtClaimingTransactionDetails.Count - 1

        '            Dim udtRelatedTransactionDetails = getRelatedTransactionDetailsList(udtClaimingTransactionDetails(i), udtBenefitTransactionDetailList)


        '            Dim blnHasAvailableSubsidy = False

        '            Dim udtSubsidizeItemDetailList As SubsidizeItemDetailsModelCollection = SchemeDetailBLL.getSubsidizeItemDetails(udtClaimingTransactionDetails(i).SubsidizeItemCode)
        '            If Not udtSubsidizeItemDetailList Is Nothing Then
        '                For Each udtSubsidizeItemDetail As SubsidizeItemDetailsModel In udtSubsidizeItemDetailList
        '                    If udtClaimingTransactionDetails(i).AvailableItemCode = udtSubsidizeItemDetail.AvailableItemCode Then
        '                        ' Check Dose Rule
        '                        Dim udtDoseRuleResult As ClaimRulesBLL.DoseRuleResult = ClaimRulesBLL.CheckSubsidizeItemDetailRuleByDose(udtRelatedTransactionDetails, _
        '            udtClaimingTransactionDetails(i).SchemeCode, udtClaimingTransactionDetails(i).SchemeSeq, udtClaimingTransactionDetails(i).SubsidizeCode, _
        '            udtClaimingTransactionDetails(i).SubsidizeItemCode, udtClaimingTransactionDetails(i).AvailableItemCode, udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, udtEHSTransaction.ServiceDate)

        '                        If udtDoseRuleResult.IsMatch AndAlso (udtDoseRuleResult.HandlingMethod = ClaimRulesBLL.DoseRuleHandlingMethod.ALL) Then
        '                            blnHasAvailableSubsidy = True
        '                        End If

        '                    End If
        '                Next
        '            End If






        '            If Not udtRelatedTransactionDetails Is Nothing Then
        '                For Each udtRelatedTransactionDetail As TransactionDetailModel In udtRelatedTransactionDetails
        '                    'If udtRelatedTransactionDetail.SubsidizeCode.Trim() = udtClaimingTransactionDetails(i).SubsidizeCode.Trim() And _
        '                    '   udtRelatedTransactionDetail.SchemeSeq = udtClaimingTransactionDetails(i).SchemeSeq And _
        '                    '   udtRelatedTransactionDetail.SchemeCode.Trim() = udtClaimingTransactionDetails(i).SchemeCode.Trim() Then
        '                    If udtRelatedTransactionDetail.SubsidizeItemCode.Trim() = udtClaimingTransactionDetails(i).SubsidizeItemCode.Trim() And _
        '                        udtRelatedTransactionDetail.AvailableItemCode.Trim() = udtClaimingTransactionDetails(i).AvailableItemCode.Trim() Then
        '                        ' Vaccine Claimed

        '                        'udtRelatedTransactionDetail.DOB
        '                        'udtRelatedTransactionDetail.ExactDOB

        '                        blnHasAvailableSubsidy = False
        '                    End If
        '                    'End If
        '                Next
        '            End If

        '            If blnHasAvailableSubsidy = False Then
        '                'Dim udtSystemMessage As New SystemMessage(cFunctionCode, "E", "00107")
        '                'Return New RuleResult(RuleID.AvailableSubsidyChecking, udtSystemMessage)
        '                Return New RuleResult(RuleID.AvailableSubsidyNoVaccine)

        '            End If

        '        Next




        '        'Dim udtClaimCategorys As ClaimCategoryModelCollection = Me._udtClaimCategoryBLL.getDistinctCategoryByScheme(udtSchemeClaimModel, udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, udtEHSTransaction.ServiceDate)
        '        'Dim udtClaimCategory As New ClaimCategoryModel()

        '        'Dim needCreateVaccine As Boolean = False
        '        'Dim noCategorys As Boolean = True

        '        'Dim notAvailableForClaim As Boolean = True
        '        'Dim isEligibleForClaim As Boolean = True

        '        'If strCategoryCode.Trim() <> "" Then
        '        '    'If udtSchemeClaimModel.SchemeCode.Trim().Equals(SchemeClaimModel.HSIVSS) OrElse udtSchemeClaimModel.SchemeCode.Trim().Equals(SchemeClaimModel.RVP) Then

        '        '    Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
        '        '    Dim strEnableClaimCategory As String = String.Empty

        '        '    'If scheme is HSIVSS or RVP, retrieve Claim Category
        '        '    udtClaimCategorys = _udtClaimCategoryBLL.getDistinctCategoryByScheme(udtSchemeClaimModel, udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, udtEHSTransaction.ServiceDate)

        '        '    'Assing Claim Category List to HINSS and RVP control

        '        '    If udtSchemeClaimModel.SchemeCode.Equals(SchemeClaimModel.RVP) Then
        '        '        udtGeneralFunction = New Common.ComFunction.GeneralFunction
        '        '        udtGeneralFunction.getSytemParameterByParameterNameSchemeCode("RVPEnableClaimCategory", strEnableClaimCategory, String.Empty, SchemeClaimModel.RVP)
        '        '    End If

        '        '    If strEnableClaimCategory = "Y" OrElse udtSchemeClaimModel.SchemeCode.Trim().Equals(SchemeClaimModel.HSIVSS) Then

        '        '        '----------------------------------------------------------------------
        '        '        'Check Claim Category list
        '        '        '----------------------------------------------------------------------
        '        '        If Not udtClaimCategorys Is Nothing AndAlso udtClaimCategorys.Count > 0 Then
        '        '            noCategorys = False
        '        '        Else
        '        '            Dim udtSystemMessage As New SystemMessage(cFunctionCode, "E", "00106")
        '        '            Return New RuleResult(RuleID.AvailableSubsidyChecking, udtSystemMessage)
        '        '            isEligibleForClaim = False
        '        '        End If

        '        '    ElseIf strEnableClaimCategory = "N" Then
        '        '        'For RVP ONLY
        '        '        udtClaimCategory = udtClaimCategorys.FilterByCategoryCode(udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SchemeSeq, "RESIDENT")
        '        '        noCategorys = False
        '        '    End If

        '        '    If Not udtEHSClaimVaccine Is Nothing AndAlso Not noCategorys AndAlso Not udtClaimCategory Is Nothing Then
        '        '        If Not udtEHSClaimVaccine.SubsidizeList Is Nothing Then
        '        '            'Check if vaccine is avaliable for the recipient -> change "noAvailableForClaim" to false
        '        '            For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
        '        '                If udtEHSClaimSubsidize.Available Then
        '        '                    notAvailableForClaim = False
        '        '                    Exit For
        '        '                End If
        '        '            Next
        '        '        Else
        '        '            udtEHSClaimVaccine = Nothing
        '        '            notAvailableForClaim = True
        '        '        End If

        '        '    ElseIf Not noCategorys Then
        '        '        notAvailableForClaim = False

        '        '    Else
        '        '        notAvailableForClaim = True
        '        '    End If
        '        'Else
        '        '    'For EVSS and CIVSS

        '        '    noCategorys = False
        '        '    'Default
        '        '    '----------------------------------------------------------------------
        '        '    'Check Vaccine is available for Claim
        '        '    '----------------------------------------------------------------------
        '        '    If Not udtEHSClaimVaccine Is Nothing Then
        '        '        If Not udtEHSClaimVaccine.SubsidizeList Is Nothing Then
        '        '            'Check if vaccine is avaliable for the recipient -> change "noAvailableForClaim" to false
        '        '            For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
        '        '                If udtEHSClaimSubsidize.Available Then
        '        '                    notAvailableForClaim = False
        '        '                    Exit For
        '        '                End If
        '        '            Next
        '        '        Else
        '        '            udtEHSClaimVaccine = Nothing
        '        '            ' No available subsidize for Claim
        '        '            ' Case 1: Not Eligiblity
        '        '            ' Case 2: Out of period
        '        '            ' Case 3: The subsidizes is used
        '        '            For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtSchemeClaimModel.SubsidizeGroupClaimList
        '        '                If udtSubsidizeGroupClaim.LastServiceDtm >= udtEHSTransaction.ServiceDate Then
        '        '                    isEligibleForClaim = False
        '        '                End If
        '        '            Next
        '        '        End If
        '        '    Else
        '        '        ' No available subsidize for Claim
        '        '        ' Case 1: Not Eligiblity
        '        '        ' Case 2: Out of period
        '        '        ' Case 3: The subsidizes is used
        '        '        For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtSchemeClaimModel.SubsidizeGroupClaimList
        '        '            If udtSubsidizeGroupClaim.LastServiceDtm >= udtEHSTransaction.ServiceDate Then
        '        '                isEligibleForClaim = False
        '        '            End If
        '        '        Next
        '        '    End If
        '        'End If


        '        'If notAvailableForClaim Then
        '        '    'If isEligibleForClaim Then
        '        '    '    Dim udtSystemMessage As New SystemMessage(cFunctionCode, "E", "00107")
        '        '    '    Return New RuleResult(RuleID.AvailableSubsidyChecking, udtSystemMessage)
        '        '    'Else
        '        '    Dim udtSystemMessage As New SystemMessage(cFunctionCode, "E", "00106")
        '        '    Return New RuleResult(RuleID.AvailableSubsidyChecking, udtSystemMessage)
        '        '    'End If
        '        'End If
        '    End If

        'End Function


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="udtSchemeClaimModel">Selected Scheme</param>
        ''' <param name="udtClaimingTransactionDetails">Selected Vaccine to be claim</param>
        ''' <param name="udtBenefitTransactionDetailList">Claimed Vaccine</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function CheckClaimRules(ByVal udtEHSClaimingTransaction As EHSTransactionModel, ByVal udtSchemeClaimModel As SchemeClaimModel, _
                                         ByVal udtClaimingTransactionDetails As TransactionDetailModelCollection, _
                                         ByVal udtBenefitTransactionDetailList As TransactionDetailModelCollection, _
                                         ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, _
                                         ByVal udtInputPicker As InputPickerModel, _
                                         Optional ByVal strRuleType As String = "") As RuleResultList

            Dim Formatter As New Format.Formatter
            Dim udtRuleResultList As New RuleResultList()
            Dim udtClaimRuleBLL As New ClaimRulesBLL

            Dim lstClaimRuleResult As New List(Of ClaimRuleResult)

            'CRE16-026 (Add PCV13) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            ' ------------------------------------------------------------------------------- 
            ' Loop for each Vaccine taken and check with Matched ClaimRuleResult
            ' ------------------------------------------------------------------------------- 
            For i As Integer = 0 To udtClaimingTransactionDetails.Count - 1

                Dim udtClaimRulesBLL As New ClaimRules.ClaimRulesBLL()

                Dim udtClaimRulesList As ClaimRules.ClaimRuleModelCollection = Nothing

                If strRuleType <> String.Empty Then
                    'Obtain Claim Rules for the specific [Scheme_Code, Scheme_Seq, Subsidize_Code] + Type (INNERDOSE)
                    udtClaimRulesList = udtClaimRulesBLL.getAllClaimRuleCache().Filter(udtSchemeClaimModel.SchemeCode, udtClaimingTransactionDetails(i).SchemeSeq, udtClaimingTransactionDetails(i).SubsidizeCode).Filter(strRuleType)
                Else
                    'Obtain Claim Rules for the specific [Scheme_Code, Scheme_Seq, Subsidize_Code]
                    udtClaimRulesList = udtClaimRulesBLL.getAllClaimRuleCache().Filter(udtSchemeClaimModel.SchemeCode, udtClaimingTransactionDetails(i).SchemeSeq, udtClaimingTransactionDetails(i).SubsidizeCode)
                End If

                If Not udtClaimRulesList Is Nothing Then
                    '-------------------------------------------------------------------
                    ' 1. Per Selected [Vaccine + Dose]
                    '-------------------------------------------------------------------
                    Dim udtRelatedTransactionDetails = getRelatedTransactionDetailsList(udtClaimingTransactionDetails(i), udtBenefitTransactionDetailList)

                    '-------------------------------------------------------------------
                    ' 2. Check with Equivalent Dose of Previous Season related Transaction (equivalent dose of previous season from EqvSubsidizePrevSeasonMap)
                    '-------------------------------------------------------------------
                    Dim udtTranDetailPrevSeason = getRelatedTransactionDetailsPrevSeasonList(udtClaimingTransactionDetails(i), udtBenefitTransactionDetailList)
                    Dim udtTranDetailNextSeason = getRelatedTransactionDetailsNextSeasonList(udtClaimingTransactionDetails(i), udtBenefitTransactionDetailList)

                    ' Group the related ClaimRules (same GroupID: And, Different GroupID: Or)
                    Dim lstClaimRuleForSubsidize As SortedList(Of String, ClaimRules.ClaimRuleModelCollection) = ClaimRulesBLL.ConvertClaimRule(udtClaimRulesList)

                    ' Check For Each ClaimRules Group
                    For Each udtGroupedClaimRuleList As ClaimRuleModelCollection In lstClaimRuleForSubsidize.Values

                        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                        ' -----------------------------------------------------------------------------------------
                        'Go to check Claim Rules
                        Dim udtCurrentClaimRuleResult As ClaimRulesBLL.ClaimRuleResult = udtClaimRuleBLL.CheckClaimRuleByRuleGroup(udtEHSClaimingTransaction.ServiceDate, udtEHSPersonalInfo, udtClaimingTransactionDetails(i).AvailableItemCode, udtRelatedTransactionDetails, udtTranDetailPrevSeason, udtTranDetailNextSeason, udtGroupedClaimRuleList, udtBenefitTransactionDetailList, udtInputPicker)
                        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                        'Only return Matched Result
                        If udtCurrentClaimRuleResult.IsMatched Then
                            lstClaimRuleResult.Add(udtCurrentClaimRuleResult)
                        End If
                    Next
                End If
            Next

            For Each udtClaimRuleResult As ClaimRuleResult In lstClaimRuleResult

                Dim udtSystemMessage As New SystemMessage(udtClaimRuleResult.RelatedClaimRule.FunctionCode, udtClaimRuleResult.RelatedClaimRule.SeverityCode, udtClaimRuleResult.RelatedClaimRule.MessageCode)
                Dim intCaseNo As Integer = 0

                Dim strReplaceTextEng As String = String.Empty
                Dim strReplaceTextTC As String = String.Empty
                Dim strMessageEng As String = String.Empty
                Dim strMessageTC As String = String.Empty

                '-----------------------------------
                'Classify type of system message
                '-----------------------------------
                'Case 1
                ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
                If Not udtClaimRuleResult.RelatedClaimRule Is Nothing Then
                    If udtClaimRuleResult.ResultParam.Count > 0 Then
                        intCaseNo = 1
                    End If
                End If
                'If Not udtClaimRuleResult.RelatedClaimRule Is Nothing Then
                '    If udtClaimRuleResult.dtmDoseDate.HasValue Then
                '        intCaseNo = 1
                '        strReplaceTextEng = "%date"
                '        strReplaceTextTC = "%date"
                '        strMessageEng = Formatter.formatDisplayDate(udtClaimRuleResult.dtmDoseDate.Value, CultureLanguage.English)
                '        strMessageTC = Formatter.formatDisplayDate(udtClaimRuleResult.dtmDoseDate.Value, CultureLanguage.TradChinese)
                '    End If
                'End If
                ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]

                'Case 2
                If Not udtSystemMessage.FunctionCode Is Nothing AndAlso Not udtSystemMessage.SeverityCode Is Nothing AndAlso Not udtSystemMessage.MessageCode Is Nothing Then
                    Select Case (udtSystemMessage.FunctionCode.ToString + "-" + udtSystemMessage.SeverityCode.ToString + "-" + udtSystemMessage.MessageCode.ToString)
                        Case "990000-E-00242"
                            intCaseNo = 2
                            strReplaceTextEng = "%en"
                            strReplaceTextTC = "%tc"

                            Dim blnFormerDisplay As Boolean = False
                            Dim blnLatterDisplay As Boolean = False
                            Dim lstStrResSystemMessage As New List(Of String)

                            For Each udtInputVaccine As InputVaccineModel In udtInputPicker.EHSClaimVaccine.Values
                                If Not blnFormerDisplay AndAlso udtInputVaccine.SubsidizeCode = udtClaimRuleResult.RelatedClaimRule.SubsidizeCode.Trim Then
                                    lstStrResSystemMessage.Add(udtInputVaccine.DisplayCodeForClaim)
                                    blnFormerDisplay = True
                                End If

                                If Not blnLatterDisplay AndAlso udtInputVaccine.SubsidizeCode = udtClaimRuleResult.RelatedClaimRule.CompareValue.Trim Then
                                    lstStrResSystemMessage.Add(udtInputVaccine.DisplayCodeForClaim)
                                    blnLatterDisplay = True
                                End If
                            Next

                            strMessageEng = lstStrResSystemMessage(0) + " " + HttpContext.GetGlobalResourceObject("Text", "ConjunctionAnd", New System.Globalization.CultureInfo(CultureLanguage.English)) + " " + lstStrResSystemMessage(1)
                            strMessageTC = lstStrResSystemMessage(0) + " " + HttpContext.GetGlobalResourceObject("Text", "ConjunctionAnd", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)) + " " + lstStrResSystemMessage(1)
                    End Select
                End If

                '-----------------------------------
                'Create system message (By Case No.)
                '-----------------------------------
                Select Case intCaseNo
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
                    Case 1
                        Dim udtRuleResult As RuleResult = New RuleResult(ReturnRuleID(udtClaimRuleResult.RuleType, udtClaimRuleResult.HandleMethod), _
                                           Nothing, Nothing, _
                                           Nothing, Nothing, _
                                           udtClaimRuleResult.RelatedClaimRule.SchemeCode, udtClaimRuleResult.RelatedClaimRule.SchemeSeq, _
                                           udtClaimRuleResult.RelatedClaimRule.SubsidizeCode, udtClaimRuleResult.RelatedClaimRule.RuleGroup)


                        For Each strKey As String In udtClaimRuleResult.ResultParam.Keys

                            strReplaceTextEng = strKey
                            strReplaceTextTC = strKey

                            If TypeOf udtClaimRuleResult.ResultParam(strKey) Is Date Then
                                strMessageEng = Formatter.formatDisplayDate(udtClaimRuleResult.ResultParam(strKey), CultureLanguage.English)
                                strMessageTC = Formatter.formatDisplayDate(udtClaimRuleResult.ResultParam(strKey), CultureLanguage.TradChinese)
                            Else
                                strMessageEng = udtClaimRuleResult.ResultParam(strKey)
                                strMessageTC = udtClaimRuleResult.ResultParam(strKey)
                            End If
                            udtRuleResult.MessageVariableNameArrayList.Add(strReplaceTextEng)
                            udtRuleResult.MessageVariableValueArrayList.Add(strMessageEng)

                            udtRuleResult.MessageVariableNameChiArrayList.Add(strReplaceTextTC)
                            udtRuleResult.MessageVariableValueChiArrayList.Add(strMessageTC)
                        Next
                        udtRuleResultList.RuleResults.Add(udtRuleResult)
                    Case 2
                        udtRuleResultList.RuleResults.Add( _
                            New RuleResult(ReturnRuleID(udtClaimRuleResult.RuleType, udtClaimRuleResult.HandleMethod), _
                                           strReplaceTextEng, strMessageEng, _
                                           strReplaceTextTC, strMessageTC, _
                                           udtClaimRuleResult.RelatedClaimRule.SchemeCode, udtClaimRuleResult.RelatedClaimRule.SchemeSeq, _
                                           udtClaimRuleResult.RelatedClaimRule.SubsidizeCode, udtClaimRuleResult.RelatedClaimRule.RuleGroup))
                        'Case 1, 2
                        '    udtRuleResultList.RuleResults.Add( _
                        '        New RuleResult(ReturnRuleID(udtClaimRuleResult.RuleType, udtClaimRuleResult.HandleMethod), _
                        '                       strReplaceTextEng, strMessageEng, _
                        '                       strReplaceTextTC, strMessageTC, _
                        '                       udtClaimRuleResult.RelatedClaimRule.SchemeCode, udtClaimRuleResult.RelatedClaimRule.SchemeSeq, _
                        '                       udtClaimRuleResult.RelatedClaimRule.SubsidizeCode, udtClaimRuleResult.RelatedClaimRule.RuleGroup))

                        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
                    Case Else
                        udtRuleResultList.RuleResults.Add( _
                            New RuleResult(ReturnRuleID(udtClaimRuleResult.RuleType, udtClaimRuleResult.HandleMethod), _
                                           Nothing, Nothing, _
                                           Nothing, Nothing, _
                                           udtClaimRuleResult.RelatedClaimRule.SchemeCode, udtClaimRuleResult.RelatedClaimRule.SchemeSeq, _
                                           udtClaimRuleResult.RelatedClaimRule.SubsidizeCode, udtClaimRuleResult.RelatedClaimRule.RuleGroup))

                End Select
            Next
            'CRE16-026 (Add PCV13) [End][Chris YIM]

            Return udtRuleResultList
        End Function
        Private Function ReturnRuleID(ByVal strRuleType As String, ByVal intHandleMethod As Integer) As Integer

            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            If intHandleMethod = HandleMethodENum.Block Then
                Select Case strRuleType
                    Case ClaimRuleModel.RuleTypeClass.INNERDOSE
                        Return RuleID.InnerDoseBlock
                    Case ClaimRuleModel.RuleTypeClass.DOSEPERIOD
                        Return RuleID.DosePeriodBlock
                    Case ClaimRuleModel.RuleTypeClass.DOSESEQ
                        Return RuleID.DoseSeqBlock
                    Case ClaimRuleModel.RuleTypeClass.Eligibility
                        Return RuleID.ClaimRuleEligibilityBlock
                    Case ClaimRuleModel.RuleTypeClass.SUBSIDIZESEQDATE
                        Return RuleID.SubsidizeSeqDateBlock
                    Case ClaimRuleModel.RuleTypeClass.CROSSSEASONINTERVAL
                        Return RuleID.CrossSeasonIntervalBlock
                    Case Else
                        Return RuleID.ClaimRuleEligibilityBlock
                End Select
            Else
                Select Case strRuleType
                    Case ClaimRuleModel.RuleTypeClass.INNERDOSE
                        Return RuleID.InnerDoseWarning
                    Case ClaimRuleModel.RuleTypeClass.DOSEPERIOD
                        Return RuleID.DosePeriodWarning
                    Case ClaimRuleModel.RuleTypeClass.DOSESEQ
                        Return RuleID.DoseSeqWarning
                    Case ClaimRuleModel.RuleTypeClass.Eligibility
                        Return RuleID.ClaimRuleEligibilityWarning
                    Case ClaimRuleModel.RuleTypeClass.SUBSIDIZESEQDATE
                        Return RuleID.SubsidizeSeqDateWarning
                    Case ClaimRuleModel.RuleTypeClass.CROSSSEASONINTERVAL
                        Return RuleID.CrossSeasonIntervalWarning
                    Case Else
                        Return RuleID.ClaimRuleEligibilityWarning
                End Select
            End If
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

            Return Nothing
        End Function

        'Private Function ValidateDosePeriod(ByVal udtEHSTransaction As EHSTransactionModel) As RuleResult
        '    ' Check Dose Period
        '    Dim udtSchemeDetailBLL As New Common.Component.SchemeDetails.SchemeDetailBLL()
        '    For Each udtTransactionDetail As EHSTransaction.TransactionDetailModel In udtEHSTransaction.TransactionDetails

        '        Dim udtSchemeDosePeriod As Component.SchemeDetails.SchemeDosePeriodModel
        '        udtSchemeDosePeriod = udtSchemeDetailBLL.getAllSchemeDosePeriod.Filter(udtEHSTransaction.SchemeCode, udtTransactionDetail.SchemeSeq, udtTransactionDetail.SubsidizeCode, udtTransactionDetail.AvailableItemCode)

        '        If Not udtSchemeDosePeriod Is Nothing Then
        '            If udtEHSTransaction.ServiceDate > udtSchemeDosePeriod.ToDtm Then
        '                'Dim udtSystemMessage As New SystemMessage(cFunctionCode, "E", "00243")
        '                'Return New RuleResult(RuleID.SPStatusChecking, udtSystemMessage)
        '                Return New RuleResult(RuleID.DosePeriod)
        '            End If
        '        End If
        '    Next

        '    Return Nothing
        'End Function

        Private Function CheckEligibility(ByVal udtEHSClaimingTransaction As EHSTransactionModel, ByVal udtSchemeClaimModel As SchemeClaimModel, ByVal udtClaimingTransactionDetails As TransactionDetailModelCollection, ByVal udtBenefitTransactionDetailList As TransactionDetailModelCollection, ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel) As RuleResultList
            ' CheckEligibilityPerSubsidize
            Dim udtClaimRulesBLL As New ClaimRulesBLL

            Dim udtRuleResultList As New RuleResultList()

            Dim blnIsEligible As Boolean

            For i As Integer = 0 To udtClaimingTransactionDetails.Count - 1

                blnIsEligible = False

                ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
                ' -----------------------------------------------------------------------------------------
                Dim udtCurrentEligibleResult As EligibleResult = Nothing
                Dim udtEligibilityRuleList As EligibilityRuleModelCollection = ClaimRulesBLL.getAllEligibilityRuleCache().Filter(udtSchemeClaimModel.SchemeCode, udtClaimingTransactionDetails(i).SchemeSeq, udtClaimingTransactionDetails(i).SubsidizeCode)
                ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

                ' Group EligibilityRule By RuleGroupCode
                Dim lstEligibilityRuleList As SortedList(Of String, EligibilityRuleModelCollection) = ClaimRulesBLL.ConvertEligibilityRule(udtEligibilityRuleList)

                For Each udtGroupedEligibilityRuleList As EligibilityRuleModelCollection In lstEligibilityRuleList.Values
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                    ' -----------------------------------------------------------------------------------------
                    Dim udtEligibleResult As EligibleResult = ClaimRulesBLL.CheckEligibleByRuleGroup(udtEHSPersonalInfo, udtEHSClaimingTransaction.ServiceDate, udtGroupedEligibilityRuleList)
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                    If udtEligibleResult.IsMatched Then
                        If udtEligibleResult.HandleMethod = HandleMethodENum.Exception Then
                            '-------------------------------------------------------------------
                            ' Exception Handling, Check with Exception Rule
                            '-------------------------------------------------------------------
                            Dim udtEligibilityExceptionRuleList As EligibilityExceptionRuleModelCollection = ClaimRulesBLL.getAllEligibilityExceptionRuleCache().Filter(udtEligibleResult.SchemeCode, udtEligibleResult.SchemeSeq, udtEligibleResult.SubsidizeCode, udtEligibleResult.RuleGroupCode)
                            Dim lstEligibilityExceptionRuleList As SortedList(Of String, EligibilityExceptionRuleModelCollection) = ClaimRulesBLL.ConvertEligibilityExceptionRule(udtEligibilityExceptionRuleList)

                            Dim blnCurrentExceptionEligible As Boolean = False
                            Dim udtCurrentExceptionEligibleResult As EligibleResult = Nothing

                            ' Return Used Benefits Transaction Merged with RelatedTransaction 
                            'Dim udtRelatedTransactionDetails = getRelatedTransactionDetailsList(udtClaimingTransactionDetails(i), udtBenefitTransactionDetailList)

                            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
                            ' -----------------------------------------------------------------------------------------
                            ' Filter the specific TransactionDetail for current subsidize
                            Dim udtSubsidizeTransactionDetailList As TransactionDetailModelCollection = udtBenefitTransactionDetailList.FilterBySubsidize(udtSchemeClaimModel.SchemeCode, udtClaimingTransactionDetails(i).SchemeSeq, udtClaimingTransactionDetails(i).SubsidizeCode)
                            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

                            '-------------------------------------------------------------------
                            ' Check with Exact Match Transaction
                            '-------------------------------------------------------------------
                            For Each udtGroupedEligibilityExceptionRuleList As EligibilityExceptionRuleModelCollection In lstEligibilityExceptionRuleList.Values
                                Dim udtExceptionEligibleResult As EligibleResult = CheckEligibleExceptionByRuleGroup(udtSubsidizeTransactionDetailList, udtEHSClaimingTransaction.ServiceDate, udtGroupedEligibilityExceptionRuleList)
                                If udtExceptionEligibleResult.IsMatched And udtExceptionEligibleResult.IsEligible Then
                                    udtCurrentEligibleResult = udtExceptionEligibleResult
                                    blnIsEligible = True

                                    If Not udtCurrentEligibleResult Is Nothing AndAlso Not udtCurrentEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                                        'Dim udtSystemMessage As New SystemMessage(udtCurrentEligibleResult.RelatedEligibleExceptionRule.FunctionCode, udtCurrentEligibleResult.RelatedEligibleExceptionRule.SeverityCode, udtCurrentEligibleResult.RelatedEligibleExceptionRule.MessageCode)
                                        'udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.Eligibility, udtSystemMessage))
                                        'Add With schemecode
                                        udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.EligibilityDB, Nothing, Nothing, Nothing, Nothing, udtCurrentEligibleResult.RelatedEligibleExceptionRule.SchemeCode, udtCurrentEligibleResult.RelatedEligibleExceptionRule.SchemeSeq, udtCurrentEligibleResult.RelatedEligibleExceptionRule.SubsidizeCode, udtCurrentEligibleResult.RelatedEligibleExceptionRule.RuleGroupCode, udtCurrentEligibleResult.RelatedEligibleExceptionRule.ExceptionGroupCode))
                                    End If

                                    Exit For
                                End If
                            Next

                            '-------------------------------------------------------------------
                            ' Check with Equivalent Dose related Transaction (equivalent dose from EqvSubsidizeMap)
                            '-------------------------------------------------------------------
                            '-------------------------------------------------------------------
                            ' Remove this Equivalent Dose For Exception Rule
                            '-------------------------------------------------------------------

                            'Dim udtEqvSubsidizeMapList As EqvSubsidizeMapModelCollection = Me._udtSchemeDetailBLL.getALLEqvSubsidizeMap().GetUniqueEqvMappingBySubsidize(udtSubsidizeGroupClaimModel.SchemeCode, udtSubsidizeGroupClaimModel.SchemeSeq, udtSubsidizeGroupClaimModel.SubsidizeCode)

                            'Dim udtMergeTransactionDetailList As New TransactionDetailModelCollection()

                            'For Each udtEqvSubsidizeMapModel As EqvSubsidizeMapModel In udtEqvSubsidizeMapList
                            '    udtSubsidizeTransactionDetailList = udtTransactionDetailList.FilterBySubsidize(udtEqvSubsidizeMapModel.EqvSchemeCode, udtEqvSubsidizeMapModel.EqvSchemeSeq, udtEqvSubsidizeMapModel.EqvSubsidizeCode)
                            '    For Each udtTransactionDetail As TransactionDetailModel In udtSubsidizeTransactionDetailList
                            '        udtMergeTransactionDetailList.Add(New TransactionDetailModel(udtTransactionDetail))
                            '    Next
                            'Next

                            'For Each udtGroupedEligibilityExceptionRuleList As EligibilityExceptionRuleModelCollection In lstEligibilityExceptionRuleList.Values
                            '    Dim udtExceptionEligibleResult As EligibleResult = Me.CheckEligibleExceptionByRuleGroup(udtSubsidizeTransactionDetailList, dtmServiceDate, udtGroupedEligibilityExceptionRuleList)
                            '    If udtExceptionEligibleResult.IsMatched And udtExceptionEligibleResult.IsEligible Then
                            '        udtCurrentEligibleResult = udtExceptionEligibleResult
                            '        Exit For
                            '    End If
                            'Next

                        ElseIf udtEligibleResult.IsEligible AndAlso udtEligibleResult.HandleMethod = HandleMethodENum.Normal Then
                            ' Match and Eligible with Normal Handling
                            udtCurrentEligibleResult = udtEligibleResult
                            blnIsEligible = True
                            Exit For
                        ElseIf udtEligibleResult.IsEligible Then
                            ' Match and Eligible with Other Handling
                            udtCurrentEligibleResult = udtEligibleResult
                            blnIsEligible = True
                            Exit For
                        End If
                    Else
                        ' Result Not Match: Ignore   

                    End If


                Next

                '-------------------------------------------------------------------
                ' Check Claim Rule with Benefit for the Type = 'Eligibility
                '-------------------------------------------------------------------

                If blnIsEligible Then

                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                    ' -----------------------------------------------------------------------------------------
                    Dim udtClaimRuleResult As ClaimRuleResult = udtClaimRulesBLL.CheckClaimRulesEligibilty(udtSchemeClaimModel.SchemeCode, udtClaimingTransactionDetails(i).SchemeSeq, udtClaimingTransactionDetails(i).SubsidizeCode, udtEHSClaimingTransaction.ServiceDate, udtEHSPersonalInfo, udtBenefitTransactionDetailList)
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                    If udtClaimRuleResult.IsMatched AndAlso udtClaimRuleResult.IsBlock Then
                        blnIsEligible = False
                    End If

                End If


                If Not blnIsEligible Then ' loop thru trans detail, if NOT any of the rule is matched 

                    If Not udtCurrentEligibleResult Is Nothing AndAlso Not udtCurrentEligibleResult.RelatedEligibleRule Is Nothing AndAlso Not udtCurrentEligibleResult.RelatedEligibleRule.MessageCode Is Nothing Then
                        'Dim udtSystemMessage As New SystemMessage(udtCurrentEligibleResult.RelatedEligibleRule.FunctionCode, udtCurrentEligibleResult.RelatedEligibleRule.SeverityCode, udtCurrentEligibleResult.RelatedEligibleRule.MessageCode)
                        'udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.Eligibility, udtSystemMessage))
                        'Add With seqcode
                        udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.EligibilityDB, Nothing, Nothing, Nothing, Nothing, udtCurrentEligibleResult.RelatedEligibleRule.SchemeCode, udtCurrentEligibleResult.RelatedEligibleRule.SchemeSeq, udtCurrentEligibleResult.RelatedEligibleRule.SubsidizeCode, udtCurrentEligibleResult.RelatedEligibleRule.RuleGroupCode))
                    End If

                    If Not udtCurrentEligibleResult Is Nothing AndAlso Not udtCurrentEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                        udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.EligibilityDB, Nothing, Nothing, Nothing, Nothing, udtCurrentEligibleResult.RelatedEligibleExceptionRule.SchemeCode, udtCurrentEligibleResult.RelatedEligibleExceptionRule.SchemeSeq, udtCurrentEligibleResult.RelatedEligibleExceptionRule.SubsidizeCode, udtCurrentEligibleResult.RelatedEligibleExceptionRule.RuleGroupCode))
                    End If

                    'Dim udtDefaultSystemMessage As New SystemMessage(cFunctionCode, "E", "00106")
                    'udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.Eligibility, udtDefaultSystemMessage))

                    'CRE16-025 (Lowering voucher eligibility age) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    'udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.Eligibility))

                    udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.Eligibility, Nothing, Nothing, Nothing, Nothing, _
                                                                     udtEHSClaimingTransaction.SchemeCode, _
                                                                     udtClaimingTransactionDetails(i).SchemeSeq, _
                                                                     udtClaimingTransactionDetails(i).SubsidizeCode))
                    'CRE16-025 (Lowering voucher eligibility age) [End][Chris YIM]

                End If
            Next

            Return udtRuleResultList
        End Function

        'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Function CheckEligibilityVoucherOnly(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtSchemeClaim As SchemeClaimModel, ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel) As RuleResultList
            Dim udtClaimRulesBLL As New ClaimRulesBLL
            Dim udtRuleResultList As New RuleResultList()
            Dim blnEligible As Boolean = False

            If udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeType = Common.Component.Scheme.SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVoucher Then
                blnEligible = udtClaimRulesBLL.CheckEligibleForClaimVoucherPerSeason(udtSchemeClaim.SchemeCode, udtEHSPersonalInfo, udtEHSTransaction.ServiceDate, True, Nothing)

                If Not blnEligible Then

                    udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.Eligibility, Nothing, Nothing, Nothing, Nothing, _
                                                 udtEHSTransaction.SchemeCode, _
                                                 udtEHSTransaction.TransactionDetails(0).SchemeSeq, _
                                                 udtEHSTransaction.TransactionDetails(0).SubsidizeCode))
                End If
            End If

            Return udtRuleResultList
        End Function
        'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

        Public Function CheckCategoryEligibility(ByVal udtEHSClaimingTransaction As EHSTransactionModel, ByVal udtSchemeClaimModel As SchemeClaimModel, ByVal udtClaimingTransactionDetails As TransactionDetailModelCollection, ByVal udtBenefitTransactionDetailList As TransactionDetailModelCollection, ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, ByVal strCategoryCode As String) As RuleResultList
            'CheckCategoryEligibilityByCategory

            Dim udtRuleResultList As New RuleResultList()

            Dim udtCurrentEligibleResult As EligibleResult = Nothing
            Dim blnIsEligible As Boolean
            Dim blnAnyRuleNotEligible As Boolean
            'Dim udtRuleResultList As New RuleResultList()

            Dim udtClaimCategoryBLL As New ClaimCategoryBLL()
            For i As Integer = 0 To udtClaimingTransactionDetails.Count - 1

                blnIsEligible = False

                ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
                ' -----------------------------------------------------------------------------------------
                Dim udtClaimCateogryEligibilityList As ClaimCategoryEligibilityModelCollection = udtClaimCategoryBLL.getCategoryEligibilityCache().Filter(udtSchemeClaimModel.SchemeCode, udtClaimingTransactionDetails(i).SchemeSeq, udtClaimingTransactionDetails(i).SubsidizeCode, strCategoryCode)
                ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

                ' Group ClaimCateogryEligibility By RuleGroupCode
                Dim lstClaimCateogryEligibilityList As SortedList(Of String, ClaimCategoryEligibilityModelCollection) = ClaimRulesBLL.ConvertClaimCateogryEligibility(udtClaimCateogryEligibilityList)

                If lstClaimCateogryEligibilityList Is Nothing OrElse lstClaimCateogryEligibilityList.Count = 0 Then
                    blnIsEligible = True
                End If

                For Each udtGroupedClaimCateogryEligibilityList As ClaimCategoryEligibilityModelCollection In lstClaimCateogryEligibilityList.Values

                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                    ' -----------------------------------------------------------------------------------------
                    Dim udtEligibleResult As EligibleResult = ClaimRulesBLL.CheckCategoryEligibilityByGroup(udtEHSPersonalInfo, udtEHSClaimingTransaction.ServiceDate, _
                                                                                                            udtGroupedClaimCateogryEligibilityList)
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                    If udtEligibleResult.IsMatched Then
                        If udtEligibleResult.IsEligible AndAlso udtEligibleResult.HandleMethod = HandleMethodENum.Normal Then
                            ' Match and Eligible with Normal Handling
                            udtCurrentEligibleResult = udtEligibleResult
                        ElseIf udtEligibleResult.IsEligible Then
                            ' Match and Eligible with Other Handling
                            udtCurrentEligibleResult = udtEligibleResult
                        End If
                    Else
                        ' Result Not Match: Ignore
                    End If

                    If Not udtCurrentEligibleResult Is Nothing AndAlso udtCurrentEligibleResult.IsEligible Then
                        'Return udtCurrentEligibleResult
                        blnIsEligible = True
                    End If

                    If Not udtCurrentEligibleResult Is Nothing AndAlso Not udtCurrentEligibleResult.RelatedClaimCategoryEligibilityModel Is Nothing AndAlso Not udtCurrentEligibleResult.RelatedClaimCategoryEligibilityModel.MessageCode Is Nothing Then

                        'Dim udtSystemMessage As New SystemMessage(udtCurrentEligibleResult.RelatedClaimCategoryEligibilityModel.FunctionCode, udtCurrentEligibleResult.RelatedClaimCategoryEligibilityModel.SeverityCode, udtCurrentEligibleResult.RelatedClaimCategoryEligibilityModel.MessageCode)
                        'udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.CategoryElibility, udtSystemMessage))
                        'add with schemecode
                        udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.CatEligibilityDB, Nothing, Nothing, Nothing, Nothing, udtCurrentEligibleResult.RelatedClaimCategoryEligibilityModel.SchemeCode, udtCurrentEligibleResult.RelatedClaimCategoryEligibilityModel.SchemeSeq, udtCurrentEligibleResult.RelatedClaimCategoryEligibilityModel.SubsidizeCode, udtCurrentEligibleResult.RelatedClaimCategoryEligibilityModel.RuleGroupCode))
                    End If

                Next

                If blnIsEligible = False Then
                    blnAnyRuleNotEligible = True
                End If
            Next

            If blnAnyRuleNotEligible Then
                'Dim udtSystemMessage As New SystemMessage(cFunctionCode, "E", "00106")
                'udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.CategoryElibility, udtSystemMessage))
                udtRuleResultList.RuleResults.Add(New RuleResult(RuleID.CategoryElibility))
            End If

            Return udtRuleResultList
        End Function

        'CRE15-004 (TIV and QIV) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Function CheckSameVaccineForNewTran(ByVal udtEHSTransaction As EHSTransactionModel) As RuleResult
            Dim udtClaimRulesBLL As New ClaimRulesBLL
            Dim udtRuleResult As RuleResult = Nothing

            Dim lstIntSchemeSeq As New List(Of Integer)
            Dim lstStrSubsidizeCode As New List(Of String)
            Dim lstStrSubsidizeItemCode As New List(Of String)
            Dim lstStrAvailableCode As New List(Of String)
            Dim lstStrResSystemMessage As New List(Of String)

            For Each udtTransactionDetailModel As TransactionDetailModel In udtEHSTransaction.TransactionDetails
                lstIntSchemeSeq.Add(udtTransactionDetailModel.SchemeSeq)
                lstStrSubsidizeCode.Add(udtTransactionDetailModel.SubsidizeCode)
                lstStrSubsidizeItemCode.Add(udtTransactionDetailModel.SubsidizeItemCode)
                lstStrAvailableCode.Add(udtTransactionDetailModel.AvailableItemCode)
            Next

            Dim blnSameVaccineDoseError As Boolean = udtClaimRulesBLL.CheckSameVaccineForNewTran(udtEHSTransaction.SchemeCode, udtEHSTransaction.ServiceDate, lstIntSchemeSeq, lstStrSubsidizeCode, lstStrSubsidizeItemCode, lstStrAvailableCode, lstStrResSystemMessage)

            If blnSameVaccineDoseError Then
                Dim strMessageEng As String = String.Empty
                Dim strMessageChi As String = String.Empty

                If lstStrResSystemMessage.Count > 0 Then
                    strMessageEng = lstStrResSystemMessage(0) + " " + HttpContext.GetGlobalResourceObject("Text", "ConjunctionAnd", New System.Globalization.CultureInfo(CultureLanguage.English)) + " " + lstStrResSystemMessage(1)
                    strMessageChi = lstStrResSystemMessage(0) + " " + HttpContext.GetGlobalResourceObject("Text", "ConjunctionAnd", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)) + " " + lstStrResSystemMessage(1)
                End If

                udtRuleResult = New RuleResult(RuleID.SameVaccineDoseForNewTran, "%en", strMessageEng, "%tc", strMessageChi)

                Return udtRuleResult
            Else
                Return udtRuleResult
            End If

        End Function
        'CRE15-004 (TIV and QIV) [End][Chris YIM]

        'CRE15-004 (TIV and QIV) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Function CheckAvailableSubsidy(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtSP As ServiceProvider.ServiceProviderModel) As RuleResultList
            Dim udtClaimRulesBLL As New ClaimRulesBLL
            Dim udtRuleResultlist As New RuleResultList

            Dim lstIntSchemeSeq As New List(Of Integer)
            Dim lstStrSubsidizeCode As New List(Of String)
            'Dim lstStrSubsidizeItemCode As New List(Of String)
            'Dim lstStrAvailableCode As New List(Of String)
            'Dim lstStrResSystemMessage As New List(Of String)

            For Each udtTransactionDetailModel As TransactionDetailModel In udtEHSTransaction.TransactionDetails
                lstIntSchemeSeq.Add(udtTransactionDetailModel.SchemeSeq)
                lstStrSubsidizeCode.Add(udtTransactionDetailModel.SubsidizeCode)
                'lstStrSubsidizeItemCode.Add(udtTransactionDetailModel.SubsidizeItemCode)
                'lstStrAvailableCode.Add(udtTransactionDetailModel.AvailableItemCode)
            Next

            Dim udtPracticeSchemeInfoModelCollection As PracticeSchemeInfo.PracticeSchemeInfoModelCollection = udtSP.PracticeList(udtEHSTransaction.PracticeID).PracticeSchemeInfoList

            Dim udtSchemeClaimBLL As New SchemeClaimBLL
            Dim udtSchemeClaimModel As SchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSTransaction.SchemeCode, udtEHSTransaction.ServiceDate)

            'Dim blnSameVaccineDoseError As Boolean = udtClaimRulesBLL.CheckSameVaccineForNewTran(udtEHSTransaction.SchemeCode, udtEHSTransaction.ServiceDate, lstIntSchemeSeq, lstStrSubsidizeCode, lstStrSubsidizeItemCode, lstStrAvailableCode, lstStrResSystemMessage)

            For idx As Integer = 0 To lstStrSubsidizeCode.Count - 1
                Dim blnSubsidyNotProvideService As Boolean = True

                For Each udtPracticeSchemeInfo As PracticeSchemeInfo.PracticeSchemeInfoModel In udtPracticeSchemeInfoModelCollection.Values
                    If udtPracticeSchemeInfo.SubsidizeCode = lstStrSubsidizeCode(idx) Then
                        If udtPracticeSchemeInfo.ProvideService Then
                            blnSubsidyNotProvideService = False
                        End If
                    End If
                Next

                If blnSubsidyNotProvideService Then
                    Dim strMessageEng As String = String.Empty

                    strMessageEng = udtSchemeClaimModel.SubsidizeGroupClaimList.Filter(udtEHSTransaction.SchemeCode, lstIntSchemeSeq(idx), lstStrSubsidizeCode(idx)).DisplayCodeForClaim

                    udtRuleResultlist.RuleResults.Add(New RuleResult(RuleID.SubsidyNotProvideService, "%en", strMessageEng))
                End If

            Next

            If udtRuleResultlist.RuleResults.Count > 0 Then
                Return udtRuleResultlist
            Else
                Return Nothing
            End If

        End Function
        'CRE15-004 (TIV and QIV) [End][Chris YIM]

#End Region

#Region "Supporting Retrieve Function"

        Private Function getServiceProvider(ByVal strSPID As String, ByVal dtServiceDate As DateTime) As ServiceProvider.ServiceProviderModel
            Dim udtServiceProviderBLL As New ServiceProvider.ServiceProviderBLL()
            Dim udtDB As New Common.DataAccess.Database()
            'Dim udtServiceProvider As ServiceProvider.ServiceProviderModel = udtServiceProviderBLL.GetServiceProviderBySPID(udtDB, strSPID)
            Dim udtServiceProvider As ServiceProvider.ServiceProviderModel = udtServiceProviderBLL.GetServiceProviderBySPID(udtDB, strSPID, True, dtServiceDate.Date)

            Return udtServiceProvider
        End Function

        Private Function getPractice(ByVal strSPID As String, ByVal intPracticeID As Integer, ByVal dtServiceDate As DateTime) As Practice.PracticeModel
            Dim udtServiceProvider As ServiceProvider.ServiceProviderModel = getServiceProvider(strSPID, dtServiceDate)
            Dim udtPracticeCollection As Practice.PracticeModelCollection = udtServiceProvider.PracticeList
            Dim udtPractice As Practice.PracticeModel = udtPracticeCollection(intPracticeID)

            Return udtPractice
        End Function

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Public Function getRelatedTransactionDetailsListBySubsidize(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String, ByVal udtTransactionDetailAllBenefits As TransactionDetailModelCollection, Optional ByVal blnPreviousSeason As Boolean = False) As TransactionDetailModelCollection

            If udtTransactionDetailAllBenefits Is Nothing Then
                udtTransactionDetailAllBenefits = New TransactionDetailModelCollection()
            End If

            'CRE16-026 (Add PCV13) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim udtClaimRulesBLL As New ClaimRulesBLL

            ' Check with Equivalent Dose related Transaction (equivalent dose from EqvSubsidizeMap)
            Dim strSubsidizeItemCode As String = (New SubsidizeBLL).GetSubsidizeItemBySubsidize(strSubsidizeCode)

            Dim udtTranDetailBenefitForSelectedSubsidize As TransactionDetailModelCollection = udtClaimRulesBLL.FilterTransactionDetailListByEqvSubsidizeMap( _
                strSchemeCode, intSchemeSeq, strSubsidizeItemCode, udtTransactionDetailAllBenefits)
            'CRE16-026 (Add PCV13) [End][Chris YIM]


            If blnPreviousSeason Then
                ' Check with Previous Season related Transaction
                '-------------------------------------------------------------------
                ' Check with Equivalent Dose related Transaction (equivalent dose from EqvSubsidizePrevSeasonMap)
                '-------------------------------------------------------------------

                ' Get all scheme subsidies available item which treat as previous season available item
                Dim udtEqvSubsidizePrevSeasonMapList As EqvSubsidizePrevSeasonMapModelCollection = (New SchemeDetailBLL).getALLEqvSubsidizePrevSeasonMap().Filter( _
                                                                                                                                                            strSchemeCode, intSchemeSeq, strSubsidizeItemCode)

                ' Loop all equivalent dose, if exist in used benefit (TransactionDetail),
                ' that mean current season vaccine was injected in previous season
                For Each udtEqvSubsidizePrevSeasonMapModel As EqvSubsidizePrevSeasonMapModel In udtEqvSubsidizePrevSeasonMapList
                    Dim udtEquMergeTranDetailList As TransactionDetailModelCollection = udtTransactionDetailAllBenefits.FilterBySubsidizeItemDetail( _
                        udtEqvSubsidizePrevSeasonMapModel.EqvSchemeCode, udtEqvSubsidizePrevSeasonMapModel.EqvSchemeSeq, udtEqvSubsidizePrevSeasonMapModel.EqvSubsidizeItemCode)

                    If udtEquMergeTranDetailList IsNot Nothing AndAlso _
                        udtEquMergeTranDetailList.Count > 0 Then

                        For Each udtTranDetail As TransactionDetailModel In udtEquMergeTranDetailList
                            udtTranDetailBenefitForSelectedSubsidize.Add(New TransactionDetailModel(udtTranDetail))
                        Next

                    End If
                Next

            End If

            Return udtTranDetailBenefitForSelectedSubsidize

        End Function

        Public Function getRelatedTransactionDetailsList(ByVal udtSelectedTransactionDetailModel As TransactionDetailModel, ByVal udtTransactionDetailAllBenefits As TransactionDetailModelCollection, Optional ByVal blnPreviousSeason As Boolean = False) As TransactionDetailModelCollection

            Return Me.getRelatedTransactionDetailsListBySubsidize(udtSelectedTransactionDetailModel.SchemeCode, udtSelectedTransactionDetailModel.SchemeSeq, udtSelectedTransactionDetailModel.SubsidizeCode, udtTransactionDetailAllBenefits, blnPreviousSeason)

        End Function

        Public Function getRelatedTransactionDetailsPrevSeasonListBySubsidize(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String, ByVal udtTransactionDetailAllBenefits As TransactionDetailModelCollection) As TransactionDetailModelCollection

            If udtTransactionDetailAllBenefits Is Nothing Then
                udtTransactionDetailAllBenefits = New TransactionDetailModelCollection()
            End If

            Dim udtTranDetailBenefitForSelectedSubsidize As New TransactionDetailModelCollection

            ' Retrieve Eqv Subsidize mapping table
            Dim SchemeDetailBLL As New SchemeDetailBLL()

            ' Check with Previous Season related Transaction
            '-------------------------------------------------------------------
            ' Check with Equivalent Dose related Transaction (equivalent dose from EqvSubsidizePrevSeasonMap)
            '-------------------------------------------------------------------

            ' Get all scheme subsidies available item which treat as previous season available item
            Dim strSubsidizeItemCode As String = (New SubsidizeBLL).GetSubsidizeItemBySubsidize(strSubsidizeCode)
            Dim udtEqvSubsidizePrevSeasonMapList As EqvSubsidizePrevSeasonMapModelCollection = SchemeDetailBLL.getALLEqvSubsidizePrevSeasonMap().Filter(strSchemeCode, intSchemeSeq, strSubsidizeItemCode)

            ' Loop all equivalent dose, if exist in used benefit (TransactionDetail),
            For Each udtEqvSubsidizePrevSeasonMapModel As EqvSubsidizePrevSeasonMapModel In udtEqvSubsidizePrevSeasonMapList
                Dim udtEquMergeTranDetailList As TransactionDetailModelCollection = udtTransactionDetailAllBenefits.FilterBySubsidizeItemDetail( _
                    udtEqvSubsidizePrevSeasonMapModel.EqvSchemeCode, udtEqvSubsidizePrevSeasonMapModel.EqvSchemeSeq, udtEqvSubsidizePrevSeasonMapModel.EqvSubsidizeItemCode)

                If udtEquMergeTranDetailList IsNot Nothing AndAlso udtEquMergeTranDetailList.Count > 0 Then

                    For Each udtTranDetail As TransactionDetailModel In udtEquMergeTranDetailList
                        udtTranDetailBenefitForSelectedSubsidize.Add(New TransactionDetailModel(udtTranDetail))
                    Next

                End If
            Next

            Return udtTranDetailBenefitForSelectedSubsidize

        End Function

        Public Function getRelatedTransactionDetailsPrevSeasonList(ByVal udtSelectedTransactionDetailModel As TransactionDetailModel, ByVal udtTransactionDetailAllBenefits As TransactionDetailModelCollection) As TransactionDetailModelCollection

            Return Me.getRelatedTransactionDetailsPrevSeasonListBySubsidize(udtSelectedTransactionDetailModel.SchemeCode, udtSelectedTransactionDetailModel.SchemeSeq, udtSelectedTransactionDetailModel.SubsidizeCode, udtTransactionDetailAllBenefits)

        End Function

        Public Function getRelatedTransactionDetailsNextSeasonListBySubsidize(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String, ByVal udtTransactionDetailAllBenefits As TransactionDetailModelCollection) As TransactionDetailModelCollection

            If udtTransactionDetailAllBenefits Is Nothing Then
                udtTransactionDetailAllBenefits = New TransactionDetailModelCollection()
            End If

            Dim udtTranDetailBenefitForSelectedSubsidize As New TransactionDetailModelCollection

            ' Retrieve Eqv Subsidize mapping table
            Dim SchemeDetailBLL As New SchemeDetailBLL()

            ' Check with Previous Season related Transaction
            '-------------------------------------------------------------------
            ' Check with Equivalent Dose related Transaction (equivalent dose from EqvSubsidizePrevSeasonMap)
            '-------------------------------------------------------------------

            ' Get all scheme subsidies available item which treat as previous season available item
            Dim strSubsidizeItemCode As String = (New SubsidizeBLL).GetSubsidizeItemBySubsidize(strSubsidizeCode)
            Dim udtEqvSubsidizePrevSeasonMapList As EqvSubsidizePrevSeasonMapModelCollection = SchemeDetailBLL.getALLEqvSubsidizePrevSeasonMap().FilterByEqv(strSchemeCode, intSchemeSeq, strSubsidizeItemCode)

            ' Loop all equivalent dose, if exist in used benefit (TransactionDetail),
            For Each udtEqvSubsidizePrevSeasonMapModel As EqvSubsidizePrevSeasonMapModel In udtEqvSubsidizePrevSeasonMapList
                Dim udtEquMergeTranDetailList As TransactionDetailModelCollection = udtTransactionDetailAllBenefits.FilterBySubsidizeItemDetail( _
                    udtEqvSubsidizePrevSeasonMapModel.SchemeCode, udtEqvSubsidizePrevSeasonMapModel.SchemeSeq, udtEqvSubsidizePrevSeasonMapModel.EqvSubsidizeItemCode)

                If udtEquMergeTranDetailList IsNot Nothing AndAlso udtEquMergeTranDetailList.Count > 0 Then

                    For Each udtTranDetail As TransactionDetailModel In udtEquMergeTranDetailList
                        udtTranDetailBenefitForSelectedSubsidize.Add(New TransactionDetailModel(udtTranDetail))
                    Next

                End If
            Next

            Return udtTranDetailBenefitForSelectedSubsidize

        End Function

        Public Function getRelatedTransactionDetailsNextSeasonList(ByVal udtSelectedTransactionDetailModel As TransactionDetailModel, ByVal udtTransactionDetailAllBenefits As TransactionDetailModelCollection) As TransactionDetailModelCollection

            Return Me.getRelatedTransactionDetailsNextSeasonListBySubsidize(udtSelectedTransactionDetailModel.SchemeCode, udtSelectedTransactionDetailModel.SchemeSeq, udtSelectedTransactionDetailModel.SubsidizeCode, udtTransactionDetailAllBenefits)

        End Function
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        Public Shared Function ReturnMessageVariableAsArrayList(ByVal strMessageVariable As String) As ArrayList

            If Not strMessageVariable Is Nothing AndAlso strMessageVariable.Trim().Length > 0 Then
                Dim alMessageVariable As New ArrayList
                Dim arrayMessageVariable() As String = strMessageVariable.Split(EHSClaimBLL.VariableDelimiter)
                For i As Int32 = 0 To arrayMessageVariable.Length - 1
                    alMessageVariable.Add(arrayMessageVariable(i))
                Next

                'alMessageVariable.AddRange(Split(strMessageVariable, EHSClaimBLL.VariableDelimiter))

                Return alMessageVariable
            Else
                Return Nothing
            End If

        End Function


        Public Shared Function ReturnMessageVariableAsString(ByVal alMessageVariable As ArrayList) As String

            Dim strMessageVariable As String = Nothing

            If Not alMessageVariable Is Nothing AndAlso alMessageVariable.Count > 0 Then
                For i As Int32 = 0 To alMessageVariable.Count - 1
                    strMessageVariable = alMessageVariable(i) + EHSClaimBLL.VariableDelimiter
                Next

                strMessageVariable = strMessageVariable.Substring(0, strMessageVariable.Length - EHSClaimBLL.VariableDelimiter.Length)

            End If

            Return strMessageVariable
        End Function

        Public Shared Function ReturnVariableFeedMessage(ByVal objSystemMessage As SystemMessage, ByVal alMessageVariableName As ArrayList, ByVal alMessageVariableValue As ArrayList, ByVal alMessageVariableNameChi As ArrayList, ByVal alMessageVariableValueChi As ArrayList) As String
            Dim strResult As String
            strResult = objSystemMessage.GetMessage()

            If Not strResult Is Nothing Then
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
                If Not HttpRuntime.Cache Is Nothing AndAlso Not HttpRuntime.Cache("language") Is Nothing AndAlso (HttpRuntime.Cache("language").ToString().Trim.ToUpper = "ZH-TW" Or HttpRuntime.Cache("language").ToString().Trim.ToUpper = "zh_HK") Then
                    strResult = ReturnVariableFeedMessage(objSystemMessage, alMessageVariableName, alMessageVariableValue, alMessageVariableNameChi, alMessageVariableValueChi, EnumLanguage.TC)

                    'If Not HttpContext.Current.Session Is Nothing AndAlso Not HttpContext.Current.Session("language") Is Nothing AndAlso (HttpContext.Current.Session("language").ToString().Trim.ToUpper = "ZH-TW" Or HttpContext.Current.Session("language").ToString().Trim.ToUpper = "zh_HK") Then
                    ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
                    'If Not alMessageVariableNameChi Is Nothing Then
                    '    For i As Int32 = 0 To alMessageVariableNameChi.Count - 1
                    '        Dim strTempVariableNameChi As String = alMessageVariableNameChi(i)
                    '        Dim strTempVariableValueChi As String = alMessageVariableValueChi(i)
                    '        If strTempVariableNameChi.Trim().Length > 0 And strTempVariableValueChi.Trim().Length > 0 Then
                    '            strResult = strResult.Replace(strTempVariableNameChi, strTempVariableValueChi)
                    '        End If
                    '    Next
                    'End If
                Else
                    strResult = ReturnVariableFeedMessage(objSystemMessage, alMessageVariableName, alMessageVariableValue, alMessageVariableNameChi, alMessageVariableValueChi, EnumLanguage.EN)
                    'If Not alMessageVariableName Is Nothing Then
                    '    For i As Int32 = 0 To alMessageVariableName.Count - 1
                    '        Dim strTempVariableName As String = alMessageVariableName(i)
                    '        Dim strTempVariableValue As String = alMessageVariableValue(i)
                    '        If strTempVariableName.Trim().Length > 0 And strTempVariableValue.Trim().Length > 0 Then
                    '            strResult = strResult.Replace(strTempVariableName, strTempVariableValue)
                    '        End If
                    '    Next
                    'End If
                End If
            End If
            Return strResult
        End Function

        Public Shared Function ReturnVariableFeedMessage(ByVal objSystemMessage As SystemMessage, ByVal alMessageVariableName As ArrayList, ByVal alMessageVariableValue As ArrayList, ByVal alMessageVariableNameChi As ArrayList, ByVal alMessageVariableValueChi As ArrayList, ByVal enumLang As EnumLanguage) As String
            Dim strResult As String
            strResult = objSystemMessage.GetMessage(enumLang)

            If Not strResult Is Nothing Then
                If enumLang = EnumLanguage.EN Then
                    ' English

                    If Not alMessageVariableName Is Nothing Then
                        For i As Int32 = 0 To alMessageVariableName.Count - 1
                            Dim strTempVariableName As String = alMessageVariableName(i)
                            Dim strTempVariableValue As String = alMessageVariableValue(i)
                            If strTempVariableName.Trim().Length > 0 And strTempVariableValue.Trim().Length > 0 Then
                                strResult = strResult.Replace(strTempVariableName, strTempVariableValue)
                            End If
                        Next
                    End If
                Else
                    ' Traditional Chinese
                    If Not alMessageVariableNameChi Is Nothing Then
                        For i As Int32 = 0 To alMessageVariableNameChi.Count - 1
                            Dim strTempVariableNameChi As String = alMessageVariableNameChi(i)
                            Dim strTempVariableValueChi As String = alMessageVariableValueChi(i)
                            If strTempVariableNameChi.Trim().Length > 0 And strTempVariableValueChi.Trim().Length > 0 Then
                                strResult = strResult.Replace(strTempVariableNameChi, strTempVariableValueChi)
                            End If
                        Next
                    End If
                End If

            End If
            Return strResult
        End Function

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        Public Function GetVaccineEntitlementBySubsidize(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String, ByVal strSubsidizeItemCode As String, _
            ByVal dtmServiceDate As DateTime, ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, _
            ByVal udtBenefitTransactionDetailList As TransactionDetailModelCollection) As SubsidizeGroupClaimItemDetailsModelCollection
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            Dim SchemeDetailBLL As New SchemeDetailBLL()
            Dim ClaimRulesBLL As New ClaimRulesBLL()
            Dim udtAvailableSubsidizeGroupClaimItemDetaillList As New SubsidizeGroupClaimItemDetailsModelCollection()

            '' Should include Previous season
            Dim udtSubsidizeGroupClaimItemDetailList As SubsidizeGroupClaimItemDetailsModelCollection = SchemeDetailBLL.getSubsidizeGroupClaimItemDetails(strSchemeCode, intSchemeSeq, strSubsidizeCode, strSubsidizeItemCode)

            If Not udtSubsidizeGroupClaimItemDetailList Is Nothing Then
                For Each udtSubsidizeGroupClaimItemDetail As SubsidizeGroupClaimItemDetailsModel In udtSubsidizeGroupClaimItemDetailList
                    ' Check Dose Rule

                    'CRE16-026 (Add PCV13) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                    ' -----------------------------------------------------------------------------------------
                    Dim udtDoseRuleResult As ClaimRulesBLL.DoseRuleResult = ClaimRulesBLL.CheckSubsidizeItemDetailRuleByDose(udtBenefitTransactionDetailList, _
                    strSchemeCode, intSchemeSeq, strSubsidizeCode, strSubsidizeItemCode, udtSubsidizeGroupClaimItemDetail.AvailableItemCode, _
                    udtEHSPersonalInfo, dtmServiceDate.Date, Nothing)
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                    If udtDoseRuleResult.IsMatch AndAlso _
                        (udtDoseRuleResult.HandlingMethod = ClaimRulesBLL.DoseRuleHandlingMethod.ALL Or _
                         udtDoseRuleResult.HandlingMethod = ClaimRulesBLL.DoseRuleHandlingMethod.READONLY Or _
                            udtDoseRuleResult.HandlingMethod = ClaimRulesBLL.DoseRuleHandlingMethod.HIDE) Then
                        udtAvailableSubsidizeGroupClaimItemDetaillList.Add(udtSubsidizeGroupClaimItemDetail)
                    End If
                    'CRE16-026 (Add PCV13) [End][Chris YIM]

                Next
            End If
            Return udtAvailableSubsidizeGroupClaimItemDetaillList
        End Function

        Public Function GetVaccineEntitlement(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, ByVal udtClaimingTransactionDetails As TransactionDetailModelCollection, ByVal udtBenefitTransactionDetailList As TransactionDetailModelCollection, ByVal udtSchemeClaimModel As SchemeClaimModel) As SubsidizeGroupClaimItemDetailsModelCollection

            Dim SchemeDetailBLL As New SchemeDetailBLL()
            Dim ClaimRulesBLL As New ClaimRulesBLL()
            Dim udtAvailableSubsidizeGroupClaimItemDetaillList As New SubsidizeGroupClaimItemDetailsModelCollection()

            For i As Integer = 0 To udtClaimingTransactionDetails.Count - 1

                Dim strSchemeCode As String = udtClaimingTransactionDetails(i).SchemeCode
                Dim intSchemeSeq As Integer = udtClaimingTransactionDetails(i).SchemeSeq
                Dim strSubsidizeCode As String = udtClaimingTransactionDetails(i).SubsidizeCode
                Dim strSubsidizeItemCode As String = udtClaimingTransactionDetails(i).SubsidizeItemCode

                Dim udtTranRelatedBenefit As TransactionDetailModelCollection = Me.getRelatedTransactionDetailsListBySubsidize(strSchemeCode, intSchemeSeq, strSubsidizeCode, udtBenefitTransactionDetailList, True)

                Dim udtSubsidizeGroupClaimItemDetailList As SubsidizeGroupClaimItemDetailsModelCollection = SchemeDetailBLL.getSubsidizeGroupClaimItemDetails(strSchemeCode, intSchemeSeq, strSubsidizeCode, strSubsidizeItemCode)

                If Not udtSubsidizeGroupClaimItemDetailList Is Nothing Then
                    For Each udtSubsidizeGroupClaimItemDetail As SubsidizeGroupClaimItemDetailsModel In udtSubsidizeGroupClaimItemDetailList
                        ' Check Dose Rule

                        'CRE16-026 (Add PCV13) [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                        ' -----------------------------------------------------------------------------------------
                        Dim udtDoseRuleResult As ClaimRulesBLL.DoseRuleResult = ClaimRulesBLL.CheckSubsidizeItemDetailRuleByDose(udtTranRelatedBenefit, _
                        udtSchemeClaimModel.SchemeCode, udtClaimingTransactionDetails(i).SchemeSeq, udtClaimingTransactionDetails(i).SubsidizeCode, _
                         udtClaimingTransactionDetails(i).SubsidizeItemCode, udtSubsidizeGroupClaimItemDetail.AvailableItemCode, udtEHSPersonalInfo, udtEHSTransaction.ServiceDate, Nothing)
                        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                        If udtDoseRuleResult.IsMatch AndAlso (udtDoseRuleResult.HandlingMethod = ClaimRulesBLL.DoseRuleHandlingMethod.READONLY Or udtDoseRuleResult.HandlingMethod = ClaimRulesBLL.DoseRuleHandlingMethod.ALL) Then
                            udtAvailableSubsidizeGroupClaimItemDetaillList.Add(udtSubsidizeGroupClaimItemDetail)
                        End If
                        'CRE16-026 (Add PCV13) [End][Chris YIM]

                    Next
                End If
            Next
            Return udtAvailableSubsidizeGroupClaimItemDetaillList

        End Function

        Public Function IsClaimingVaccineNotEntitled(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, ByVal udtSchemeClaimModel As SchemeClaimModel, ByVal udtEHSAccount As EHSAccountModel, ByVal udtClaimingTransactionDetails As TransactionDetailModelCollection, ByVal udtBenefitTransactionDetailList As TransactionDetailModelCollection) As Boolean

            Dim blnIsClaimingNotEntitled As Boolean = False
            For i As Integer = 0 To udtClaimingTransactionDetails.Count - 1

                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                'Dim udtAvailableSubsidizeGroupClaimItemDetailList As SubsidizeGroupClaimItemDetailsModelCollection = Me.GetVaccineEntitlementBySubsidize(udtClaimingTransactionDetails(i).SchemeCode, udtClaimingTransactionDetails(i).SchemeSeq, udtClaimingTransactionDetails(i).SubsidizeCode, udtClaimingTransactionDetails(i).SubsidizeItemCode, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, udtBenefitTransactionDetailList)
                Dim udtAvailableSubsidizeGroupClaimItemDetailList As SubsidizeGroupClaimItemDetailsModelCollection = Me.GetVaccineEntitlementBySubsidize(udtClaimingTransactionDetails(i).SchemeCode, udtClaimingTransactionDetails(i).SchemeSeq, udtClaimingTransactionDetails(i).SubsidizeCode, udtClaimingTransactionDetails(i).SubsidizeItemCode, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo, udtBenefitTransactionDetailList)
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                If udtAvailableSubsidizeGroupClaimItemDetailList.Filter(udtClaimingTransactionDetails(i).SubsidizeItemCode, udtClaimingTransactionDetails(i).AvailableItemCode).Count <= 0 Then
                    blnIsClaimingNotEntitled = True
                End If
            Next

            Return blnIsClaimingNotEntitled

        End Function

        Public Function IsClaimingVaccineExceedEntitlement(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, ByVal udtSchemeClaimModel As SchemeClaimModel, ByVal udtEHSAccount As EHSAccountModel, ByVal udtClaimingTransactionDetails As TransactionDetailModelCollection, ByVal udtBenefitTransactionDetailList As TransactionDetailModelCollection) As Boolean

            Dim udtAvailableSubsidizeGroupClaimItemDetailList As SubsidizeGroupClaimItemDetailsModelCollection = Me.GetVaccineEntitlement(udtEHSTransaction, udtEHSPersonalInfo, udtClaimingTransactionDetails, udtBenefitTransactionDetailList, udtSchemeClaimModel)

            Dim iNumOfAvailableEntitlement As Int32 = 0
            Dim iNumOfUsedEntitlement As Int32 = 0
            Dim blnIsClaimingVaccineExceedEntitlement = False

            For i As Integer = 0 To udtClaimingTransactionDetails.Count - 1

                If Me.IsClaimingVaccineExceedEntitlementPerSubsidy(udtAvailableSubsidizeGroupClaimItemDetailList, udtEHSTransaction, udtEHSPersonalInfo, udtSchemeClaimModel, udtEHSAccount, udtClaimingTransactionDetails(i), udtBenefitTransactionDetailList) Then
                    blnIsClaimingVaccineExceedEntitlement = True
                    Exit For
                End If
            Next

            Return blnIsClaimingVaccineExceedEntitlement

        End Function


        ''' <summary>
        ''' Check Entitlement all used per subsidize
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="intSchemeSeq"></param>
        ''' <param name="strSubsidizeCode"></param>
        ''' <param name="strSubsidizeItemCode"></param>
        ''' <param name="udtEntitlementSubsidizeItemDetailList"></param>
        ''' <param name="udtBenefitTransactionDetailList"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsAllVaccineEntitlementUsedPerSubsidy(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, _
            ByVal strSubsidizeCode As String, ByVal strSubsidizeItemCode As String, _
            ByVal udtEntitlementSubsidizeGroupClaimItemDetailList As SubsidizeGroupClaimItemDetailsModelCollection, _
            ByVal udtBenefitTransactionDetailList As TransactionDetailModelCollection) As Boolean

            ' SubsidizeCode <-> SubsidizeItemCode (one to one mapping)
            Dim iNumOfAvailableEntitlement As Int32 = Me.GetNumberOfEntitlementBySubsidize(udtEntitlementSubsidizeGroupClaimItemDetailList, strSubsidizeItemCode)
            Dim iNumOfUsedEntitlement As Int32 = Me.GetNumberOfClaimedVaccineBySubsidize(udtBenefitTransactionDetailList, strSchemeCode, intSchemeSeq, strSubsidizeCode)
            Dim blnIsAllVaccineEntitlementUsed = False

            If iNumOfAvailableEntitlement <= iNumOfUsedEntitlement Then
                blnIsAllVaccineEntitlementUsed = True
            End If
            Return blnIsAllVaccineEntitlementUsed

        End Function

        ''' <summary>
        ''' Selecting Dose, Exceed (# of Entitlement less than # of claimed Dose + claiming Dose)
        ''' </summary>
        ''' <param name="udtEntitlementSubsidizeItemDetailList"></param>
        ''' <param name="udtEHSTransaction"></param>
        ''' <param name="udtEHSPersonalInfo"></param>
        ''' <param name="udtSchemeClaimModel"></param>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="udtClaimingTransactionDetail"></param>
        ''' <param name="udtBenefitTransactionDetailList"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function IsClaimingVaccineExceedEntitlementPerSubsidy(ByVal udtEntitlementSubsidizeGroupClaimItemDetailList As SubsidizeGroupClaimItemDetailsModelCollection, _
            ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, _
            ByVal udtSchemeClaimModel As SchemeClaimModel, ByVal udtEHSAccount As EHSAccountModel, _
            ByVal udtClaimingTransactionDetail As TransactionDetailModel, ByVal udtBenefitTransactionDetailList As TransactionDetailModelCollection) As Boolean
            ' Entitlement for the specific Subsidize [SubsidizeCode <-> SubsidizeItemCode]

            Dim blnVaccineExceedEntitlement = False

            Dim iNumOfAvailableEntitlement As Int32 = Me.GetNumberOfEntitlementBySubsidize(udtEntitlementSubsidizeGroupClaimItemDetailList, udtClaimingTransactionDetail.SubsidizeItemCode)
            Dim iNumOfUsedEntitlement As Int32 = Me.GetNumberOfClaimedVaccineBySubsidize(udtBenefitTransactionDetailList, udtClaimingTransactionDetail.SchemeCode, udtClaimingTransactionDetail.SchemeSeq, udtClaimingTransactionDetail.SubsidizeCode)

            ' Assume in UI, only 1 Dose per subsidize is allow to be selected, if not, further enhancement is needed
            If iNumOfAvailableEntitlement < iNumOfUsedEntitlement + 1 Then
                blnVaccineExceedEntitlement = True
            End If
            Return blnVaccineExceedEntitlement

        End Function

        ''' <summary>
        ''' Get Number of Entitlement for the subsidize (SubsidizeCode -- SubsidizeItemCode [One-One])
        ''' </summary>
        ''' <param name="udtEntitlementSubsidizeItemDetailList">Entitlement for subsidies under the same scheme (need to filter by subsidizeItemCode)</param>
        ''' <param name="strSubsidizeItemCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetNumberOfEntitlementBySubsidize(ByVal udtEntitlementSubsidizeGroupClaimItemDetailList As SubsidizeGroupClaimItemDetailsModelCollection, ByVal strSubsidizeItemCode As String) As Integer
            Dim udtAvailableSubsidizeGroupClaimItemDetailList As SubsidizeGroupClaimItemDetailsModelCollection = udtEntitlementSubsidizeGroupClaimItemDetailList.Filter(strSubsidizeItemCode)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            'Return udtAvailableSubsidizeGroupClaimItemDetailList.Count
            Dim intSubsidizeEntitlement As Integer = 0
            For Each udtAvailableSubsidizeGroupClaimItemDetailModel As SubsidizeGroupClaimItemDetailsModel In udtAvailableSubsidizeGroupClaimItemDetailList
                intSubsidizeEntitlement += udtAvailableSubsidizeGroupClaimItemDetailModel.AvailableItemNum
            Next
            Return intSubsidizeEntitlement
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        End Function

        ''' <summary>
        ''' Get Number of claimed vaccine for the subsidize (Eqv should be take care also)
        ''' </summary>
        ''' <param name="udtBenefitTransactionDetailList"></param>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="intSchemeSeq"></param>
        ''' <param name="strSubsidizeCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetNumberOfClaimedVaccineBySubsidize(ByVal udtBenefitTransactionDetailList As TransactionDetailModelCollection, ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String) As Integer

            ' Count also the Eqv Mapping for the subsidize <-> SubsidizeItemCode
            Dim udtBenefitTransactionDetailListBySubsidy As TransactionDetailModelCollection = Me.getRelatedTransactionDetailsListBySubsidize(strSchemeCode, intSchemeSeq, strSubsidizeCode, udtBenefitTransactionDetailList)
            Return udtBenefitTransactionDetailListBySubsidy.Count

        End Function

        'CRE16-025 (Lowering voucher eligibility age) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Private Function GetValidationRule(ByVal intClaimActionType As Integer, ByVal intRuleID As Integer, ByVal strSchemeCode As String) As EHSClaimValidationRuleModel
            Dim strRuleID As String = GetRuleIDString(intRuleID)

            Dim udtEHSClaimValidationRuleCollection As New EHSClaimValidationRuleModelCollection()
            udtEHSClaimValidationRuleCollection = _udtEHSClaimValidationRuleCollection.GetEHSClaimValidationRuleByRuleID(GetClaimActionType(intClaimActionType), strRuleID, strSchemeCode)

            If Not udtEHSClaimValidationRuleCollection Is Nothing AndAlso udtEHSClaimValidationRuleCollection.Count > 0 Then
                Return udtEHSClaimValidationRuleCollection(0)
            Else
                Return Nothing
            End If

        End Function
        'CRE16-025 (Lowering voucher eligibility age) [End][Chris YIM]

        Private Function GetValidationRule(ByVal intClaimActionType As Integer, ByVal intRuleID As Integer, ByVal strSchemeCode As String, ByVal strSchemeSeq As String, ByVal strSubsidizeCode As String, ByVal strRuleGroup1 As String, ByVal strRuleGroup2 As String) As EHSClaimValidationRuleModel
            Dim strRuleID As String = GetRuleIDString(intRuleID)
            'Dim strClaimActionType As String = GetClaimActionType(intClaimActionType)
            Dim udtEHSClaimValidationRuleCollection As New EHSClaimValidationRuleModelCollection()
            udtEHSClaimValidationRuleCollection = _udtEHSClaimValidationRuleCollection.GetEHSClaimValidationRuleByRuleID(GetClaimActionType(intClaimActionType), strRuleID, strSchemeCode, strSchemeSeq, strSubsidizeCode, strRuleGroup1, strRuleGroup2)

            If Not udtEHSClaimValidationRuleCollection Is Nothing AndAlso udtEHSClaimValidationRuleCollection.Count > 0 Then
                Return udtEHSClaimValidationRuleCollection(0)
            Else
                Return Nothing
            End If

        End Function

        Private Function GetClaimActionType(ByVal intClaimActionType As Integer) As String
            Dim strActionType As String = ""
            Select Case intClaimActionType
                Case ClaimAction.HCVUClaim
                    strActionType = "HCVUClaim"
                Case ClaimAction.HCSPClaim
                    strActionType = "HCSPClaim"
                Case ClaimAction.HCSPRectification
                    strActionType = "HCSPRectification"
                Case ClaimAction.UploadClaim
                    strActionType = "UploadClaim"
                    ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
                Case ClaimAction.UploadStudent
                    strActionType = "UploadStudent"
                    ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
                Case ClaimAction.UploadPrecheck
                    strActionType = "UploadPrecheck"
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]
            End Select
            Return strActionType

        End Function

        Private Function GetRuleIDString(ByVal intRuleID As Integer) As String

            Dim strRule As String = ""

            Select Case intRuleID
                Case RuleID.ServiceDateClaimPeriodChecking
                    strRule = "ServiceDtPeriod"
                Case RuleID.ServiceDateSPChecking
                    strRule = "ServiceDateSP"
                Case RuleID.ServiceDateFuture
                    strRule = "ServiceDateFuture"
                Case RuleID.DayBackMinDate
                    strRule = "DayBackMinDate"
                Case RuleID.ServiceDateID235BChecking
                    strRule = "ServiceDateID235B"
                Case RuleID.DayBackLimitChecking
                    strRule = "DayBackLimit"
                Case RuleID.ACStatusSuspend
                    strRule = "ACStatusSuspend"
                Case RuleID.ACStatusDecease
                    strRule = "ACStatusDecease"
                Case RuleID.SPSuspend
                    strRule = "SPSuspend"
                Case RuleID.SPDelist
                    strRule = "SPDelist"
                Case RuleID.SPSchemeInactive
                    strRule = "SPSchemeInactive"
                Case RuleID.PracticeSuspend
                    strRule = "PracticeSuspend"
                Case RuleID.PracticeDelist
                    strRule = "PracticeDelist"
                Case RuleID.PracticeSchemeDelist
                    strRule = "PracticeSchemeDelist"
                Case RuleID.DocExceedEC
                    strRule = "DocExceedEC"
                Case RuleID.DocExceedOther
                    strRule = "DocExceedOther"
                Case RuleID.AvailableSubsidyNoVoucher
                    strRule = "NoAvailVoucher"
                    'Case RuleID.AvailableSubsidyNoVaccine
                    'strRule = "NoAvailVaccine"
                Case RuleID.AvailableSubsidyVaccineSameTaken
                    strRule = "VaccineTaken"
                Case RuleID.AvailableSubsidyVaccineExceedEntitlement
                    strRule = "VaccineExceed"
                Case RuleID.AvailableSubsidyVaccineNotEntitled
                    strRule = "VaccineNotEntitled"
                Case RuleID.Eligibility
                    strRule = "Eligibility"
                Case RuleID.EligibilityDB
                    strRule = "EligibilityDB"
                Case RuleID.CategoryElibility
                    strRule = "CatEligibility"
                Case RuleID.CatEligibilityDB
                    strRule = "CatEligibilityDB"
                Case RuleID.PreprimarySchoolCertificate
                    strRule = "PreSchool"
                Case RuleID.TSWChecking
                    strRule = "TSW"
                Case RuleID.SPInactive
                    strRule = "SPInactive"
                Case RuleID.PracticeInactive
                    strRule = "PracticeInactive"
                Case RuleID.InnerDoseBlock
                    strRule = "ClaimRule"
                Case RuleID.InnerDoseWarning
                    strRule = "ClaimRule"
                Case RuleID.DoseSeqBlock
                    strRule = "ClaimRule"
                Case RuleID.DoseSeqWarning
                    strRule = "ClaimRule"
                Case RuleID.DosePeriodBlock
                    strRule = "ClaimRule"
                Case RuleID.DosePeriodWarning
                    strRule = "ClaimRule"
                Case RuleID.ClaimRuleEligibilityBlock
                    strRule = "ClaimRule"
                Case RuleID.ClaimRuleEligibilityWarning
                    strRule = "ClaimRule"
                Case RuleID.DoseSeq
                    strRule = "ClaimRule"
                Case RuleID.DosePeriod
                    strRule = "ClaimRule"
                Case RuleID.PracNotEnrolScheme
                    strRule = "PracNotEnrolScheme"
                Case RuleID.PracJoinedProf
                    strRule = "PracJoinedProf"
                Case RuleID.SchemeSubsidy
                    strRule = "SchemeSubsidy"

                    ' CRE11-007
                Case RuleID.DeathDecease
                    strRule = "DeathDecease"
                Case RuleID.ServiceDateDecease
                    strRule = "ServiceDateDecease"

                    'Case RuleID.InnerDoseBlock
                    '    strRule = "InnerDose"
                    'Case RuleID.InnerDoseWarning
                    '    strRule = "InnerDose"
                    'Case RuleID.DoseSeqBlock
                    '    strRule = "DoseSeq"
                    'Case RuleID.DoseSeqWarning
                    '    strRule = "DoseSeq"
                    'Case RuleID.DosePeriodBlock
                    '    strRule = "DosePeriod"
                    'Case RuleID.DosePeriodWarning
                    '    strRule = "DosePeriod"
                    'Case RuleID.ClaimRuleEligibilityBlock
                    '    strRule = "ClaimRule"
                    'Case RuleID.ClaimRuleEligibilityWarning
                    '    strRule = "ClaimRule"
                    'Case RuleID.DoseSeq
                    '    strRule = "DoseSeq"
                    'Case RuleID.DosePeriod
                    '    strRule = "DosePeriod"

                    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                Case RuleID.SubsidizeSeqDateBlock
                    strRule = "ClaimRule"
                Case RuleID.SubsidizeSeqDateWarning
                    strRule = "ClaimRule"
                Case RuleID.CrossSeasonIntervalBlock
                    strRule = "ClaimRule"
                Case RuleID.CrossSeasonIntervalWarning
                    strRule = "ClaimRule"
                    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

                    'CRE15-004 (TIV and QIV) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                Case RuleID.SameVaccineDoseForNewTran
                    strRule = "SameVaccineDose"
                Case RuleID.SubsidyNotProvideService
                    strRule = "SubsidyNotProvided"
                    'CRE15-004 (TIV and QIV) [End][Chris YIM]

                    'CRE16-017 (Block EHCP make voucher claim for themselves) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                Case RuleID.SelfClaim
                    strRule = "SelfClaim"
                    'CRE16-017 (Block EHCP make voucher claim for themselves) [End][Chris YIM]

                    ' CRE19-003 (Opt voucher capping) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                Case RuleID.AvailableSubsidyNoVoucherQuota
                    strRule = "NoAvailVoucherQuota"
                    ' CRE19-003 (Opt voucher capping) [End][Winnie]

                    ' CRE19-006 (DHC) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                Case RuleID.DHCServiceNotProvided
                    strRule = "NotProvidedDHC"
                Case RuleID.VoucherExceedDHCMaxClaimAmt
                    strRule = "ExceedDHCMaxAmt"

                    ' CRE19-006 (DHC) [End][Winnie]
                Case RuleID.None
                    strRule = ""
            End Select

            Return strRule
        End Function

#End Region

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Public Function CheckTransactionIncomplete(ByVal udtEHSTransaction As EHSTransactionModel) As Boolean

            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
            Dim isIncomplete As Boolean = False

            If udtEHSTransaction IsNot Nothing Then
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                Select Case _SchemeClaimBLL.ConvertControlTypeFromSchemeClaimCode(udtEHSTransaction.SchemeCode)
                    'CRE13-019-02 Extend HCVS to China [Start][Karl]
                    Case SchemeClaimModel.EnumControlType.VOUCHER, SchemeClaimModel.EnumControlType.VOUCHERCHINA
                        'CRE13-019-02 Extend HCVS to China [End][Karl]

                        If udtEHSTransaction.TransactionAdditionFields IsNot Nothing Then

                            'No CoPaymentFee and No Reason For Visit
                            If udtEHSTransaction.TransactionAdditionFields.Count = 0 Then
                                isIncomplete = True
                            End If

                            'CRE13-019-02 Extend HCVS to China [Start][Karl]
                            'No Principal Reason For Visit

                            If udtGeneralFunction.IsCoPaymentFeeEnabled(udtEHSTransaction.ServiceDate) Then
                                'No CoPaymentFee 

                                If _SchemeClaimBLL.ConvertControlTypeFromSchemeClaimCode(udtEHSTransaction.SchemeCode) = SchemeClaimModel.EnumControlType.VOUCHER Then

                                    If Not HasCoPaymentFee(udtEHSTransaction) Or Not HasPrincipalReasonForVisit(udtEHSTransaction) Then
                                        isIncomplete = True
                                    End If

                                ElseIf _SchemeClaimBLL.ConvertControlTypeFromSchemeClaimCode(udtEHSTransaction.SchemeCode) = SchemeClaimModel.EnumControlType.VOUCHERCHINA Then
                                    If Not HasCoPaymentFeeRMB(udtEHSTransaction) Then
                                        isIncomplete = True
                                    End If
                                End If

                                ' Else
                                'No Principal Reason For Visit
                                'If Not HasPrincipalReasonForVisit(udtEHSTransaction) Then
                                '    isIncomplete = True
                                'End If
                            End If

                            'CRE13-019-02 Extend HCVS to China [End][Karl]
                        Else
                            'No CoPaymentFee and No Reason For Visit
                            isIncomplete = True
                        End If
                    Case Else
                        'Do Nothing
                End Select
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
            End If

            Return isIncomplete

        End Function

        'CRE13-019-02 Extend HCVS to China [Start][Karl]
        Public Function HasCoPaymentFeeRMB(ByVal udtEHSTransaction As EHSTransactionModel) As Boolean
            Dim btnHasCoPaymentFee As Boolean = False
            If udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.CoPaymentFeeRMB) IsNot Nothing Then
                btnHasCoPaymentFee = True
            End If
            Return btnHasCoPaymentFee
        End Function
        'CRE13-019-02 Extend HCVS to China [End][Karl]

        Public Function HasCoPaymentFee(ByVal udtEHSTransaction As EHSTransactionModel) As Boolean
            Dim btnHasCoPaymentFee As Boolean = False
            If udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.CoPaymentFee) IsNot Nothing Then
                btnHasCoPaymentFee = True
            End If
            Return btnHasCoPaymentFee
        End Function

        Public Function HasPrincipalReasonForVisit(ByVal udtEHSTransaction As EHSTransactionModel) As Boolean
            Dim btnHasPrincipalReasonForVisit As Boolean = False

            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            'If udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1(0)) IsNot Nothing And udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL2(0)) IsNot Nothing Then
            If udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1(0)) IsNot Nothing Then
                ' CRE19-006 (DHC) [End][Winnie]
                btnHasPrincipalReasonForVisit = True
            End If
            Return btnHasPrincipalReasonForVisit
        End Function

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Koala]

        'CRE13-006 HCVS Ceiling [Start][Karl]
        Public Function IsBackSeasonClaim(ByVal dtmServiceDate As DateTime, ByVal strSchemeCode As String, Optional ByRef intServiceDateSchemeSeq As Integer = 0) As Boolean
            Dim intCurrentSchemeSeq As Integer
            Dim udtSchemeClaimBLL As New SchemeClaimBLL
            Dim udtSubsidizeGroupClaimList As SubsidizeGroupClaimModelCollection

            udtSubsidizeGroupClaimList = udtSchemeClaimBLL.getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(strSchemeCode).SubsidizeGroupClaimList.Filter(dtmServiceDate)
            intServiceDateSchemeSeq = udtSubsidizeGroupClaimList(0).SchemeSeq
            udtSubsidizeGroupClaimList = udtSchemeClaimBLL.getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(strSchemeCode).SubsidizeGroupClaimList.Filter(Now)
            intCurrentSchemeSeq = udtSubsidizeGroupClaimList(0).SchemeSeq

            If intCurrentSchemeSeq > intServiceDateSchemeSeq Then
                Return True
            Else
                Return False
            End If

        End Function
        'CRE13-006 HCVS Ceiling [End][Karl]

    End Class

End Namespace