Imports Common.Component
Imports Common.Component.Practice
Imports Common.Component.Scheme
Imports Common.Component.EHSAccount
Imports Common.Component.DataEntryUser
Imports Common.Component.ServiceProvider
Imports Common.Component.EHSTransaction
Imports Common.Component.ClaimRules.ClaimRulesBLL

Namespace BLL

    Public Class SessionHandler

        Public Class SessionName
            Public Const SESS_LANGUAGE As String = "language"
            Public Const SESS_EHSAccount As String = "SESS_EHSACCOUNT"
            Public Const SESS_SelectedPracticeDsiplay As String = "SESS_SELECTEDPRACTICEDISPLAY"
            Public Const SESS_SelectedPracticeDsiplayList As String = "SESS_SELECTEDPRACTICEDISPLAYLIST"
            Public Const SESS_ConfirmedPracticePopup As String = "SESS_CONFIRMEDPRACTICEPOPUP"
            Public Const SESS_AccountCreationProceedClaim As String = "SESS_ACCOUNTCREATIONPROCEEDCLAIM"
            Public Const SESS_AccountCreationComeFromClaim As String = "SESS_ACCOUNTCREATIONCOMEFROMCLAIM"
            Public Const SESS_SchemeSelected As String = "SESS_SCHEMESELECTED"
            Public Const SESS_SchemeSubsidizeList As String = "SESS_SCHEMESUBSIDIZELIST"
            Public Const SESS_DocumentTypeSelected As String = "SESS_DOCUMENTTYPESELECTED"


            Public Const SESS_ServiceProvider As String = "SESS_SERVICEPROVIDER"
            Public Const SESS_DataEntry As String = "SESS_DATAENTRY"
            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Public Const SESS_NonClinicSetting As String = "SESS_NONCLINICSETTING"
            Public Const SESS_ChangeSchemeInPractice As String = "SESS_CHANGESCHEMEINPRACTICE"
            Public Const SESS_ClaimForSamePatient As String = "SESS_CLAIMFORSAMEPATIENT"
            Public Const SESS_EligibleResultVSSReminder As String = "SESS_ELIGIBLERESULTVSSREMINDER"
            Public Const SESS_DocumentaryProofForPID As String = "SESS_DOCUMENTARYPROOFFORPID"
            Public Const SESS_PIDInstitutionCode As String = "SESS_PIDINSTITUTIONCODE"
            Public Const SESS_PlaceOfVaccination As String = "SESS_PLACEOFVACCINATION"
            Public Const SESS_PlaceOfVaccinationOther As String = "SESS_PLACEOFVACCINATIONOTHER"
            'CRE16-002 (Revamp VSS) [End][Chris YIM]
            'CRE16-026 (Add PCV13) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Public Const SESS_HighRisk As String = "SESS_HIGHRISK"
            Public Const SESS_SubsidizeDisabledDetailKey As String = "SESS_SUBSIDIZEDISABLEDDETAILKEY"
            'CRE16-026 (Add PCV13) [End][Chris YIM]

            'CRe20-009 VSS DA with CSSA [Start][Nichole]
            Public Const SESS_DocumentaryProof As String = "SESS_DOCUMENTARYPROOF"
            'CRE20-009 VSS DA with CSSA [End][Nichole]

            '------------------------------------------------------------------------------------------
            'EHS Claim Session
            '------------------------------------------------------------------------------------------
            Public Const SESS_NotMatchTempAccountExist As String = "SESS_NOTMATCHTEMPACCOUNTEXIST"
            Public Const SESS_ExceedDocTypeLimit As String = "SESS_EXCEEDDOCTYPELIMIT"
            Public Const SESS_EHSEnterClaimDetailSearchRCH As String = "SESS_EHSCLAIMENTERCLAIMDETAILSEARCHRCH"
            Public Const SESS_EHSEnterClaimDetailSearchOutreach As String = "SESS_EHSCLAIMENTERCLAIMDETAILSEARCHOUTREACH"
            Public Const SESS_RuleResults As String = "SESS_RULERESULTS"

            Public Const SESS_EHSClaimSteps As String = "SESS_EHSCLAIMSTEPS"
            Public Const SESS_EHSClaimConfirmMessage As String = "SESS_EHSCLAIMCONFIRMMESSAGE"

            Public Const SESS_EHSClaimVaccine As String = "SESS_EHSCLAIMVACCINE"
            Public Const SESS_EHSTransaction As String = "SESS_EHSTRANSACTION"
            Public Const SESS_EHSTransaction_Orginal As String = "SESS_EHSTRANSACTION_ORGINAL"
            Public Const SESS_EHSTransactions As String = "SESS_EHSTRANSACTIONS"
            Public Const SESS_EHSTransactions_Page As String = "SESS_EHSTRANSACTIONS_PAGE"
            Public Const SESS_EHSTransactions_SearchByNo As String = "SESS_EHSTRANSACTIONS_SEARCHBYNO"

            Public Const SESS_CCCode1 As String = "SESS_CHOOSECCCODE_CCCODE1"
            Public Const SESS_CCCode2 As String = "SESS_CHOOSECCCODE_CCCODE2"
            Public Const SESS_CCCode3 As String = "SESS_CHOOSECCCODE_CCCODE3"
            Public Const SESS_CCCode4 As String = "SESS_CHOOSECCCODE_CCCODE4"
            Public Const SESS_CCCode5 As String = "SESS_CHOOSECCCODE_CCCODE5"
            Public Const SESS_CCCode6 As String = "SESS_CHOOSECCCODE_CCCODE6"

            Public Const SESS_EnterClaimToken As String = "SESS_ENTERCLAIMTOKEN"
            Public Const SESS_EHSClaimPrintOutFunctionCode As String = "SESS_EHSCLAIM_PRINTOUT_FUNCTIONCODE"

            Public Const SESS_RVPRCHCode As String = "SESS_RVHRCHCode"
            Public Const SESS_OutreachCode As String = "SESS_OUTREACHCODE"
            Public Const SESS_ClaimCategory As String = "SESS_CLAIMCATEGORY"

            Public Const SESS_IsMobileDevice As String = "SESS_ISMOBILEDEVICE"
            Public Const SESS_CMSVaccineResult As String = "SESS_CMSVaccineResult"
            Public Const SESS_CIMSVaccineResult As String = "SESS_CIMSVaccineResult"

            'CRE15-003 System-generated Form [Start][Philip Chau]
            Public Const SESS_EHSClaimTempTransactionID As String = "SESS_EHSClaimTempTransactionID"
            Public Const SESS_EHSClaimStep3ShowLastestTransactionID As String = "SESS_EHSClaimStep3ShowLastestTransactionID"
            'CRE15-003 System-generated Form [End][Philip Chau]

            ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [Start][Winnie]
            Public Const SESS_NoticedDuplicateClaimAlert As String = "SESS_NOTICEDDUPLICATECLAIMALERT"
            ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [End][Winnie]

            '------------------------------------------------------------------------------------------
            'Void EHS Claim Session
            '------------------------------------------------------------------------------------------
            Public Const SESS_SelectedSearchType As String = "SESS_SELECTEDSEARCHTYPE"
            Public Const SESS_eHSAccDocType As String = "SESS_EHSACCDOCTYPE"
            Public Const SESS_eHSAccDocNum As String = "SESS_EHSACCDOCNUM"


            '------------------------------------------------------------------------------------------
            'SmartIC Content Session
            '------------------------------------------------------------------------------------------
            Public Const SESS_SmartIDContent As String = "SESS_SMARTIDCONTENT"
            Public Const SESS_PilotRunSmartID As String = "SESS_PILOTRUNSMARTID"

            ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Public Const SESS_IDEASComboClient As String = "SESS_IDEASComboClient"
            Public Const SESS_IDEASComboVersion As String = "SESS_IDEASComboVersion"
            ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

            Public Const SESS_ForceToReadSmartIC As String = "SESS_ForceToReadSmartIC"

            '------------------------------------------------------------------------------------------
            'VaccinationRecordEnquiry
            '------------------------------------------------------------------------------------------
            Public Const SESS_FromVaccinationRecordEnquiry As String = "SESS_FROMVACCINATIONRECORDENQUIRY"
            Public Const SESS_VREEHSAccount As String = "SESS_VREEHSACCOUNT"
            Public Const SESS_SearchAccountStatus As String = "SESS_SEARCHACCOUNTSTATUS"

            Public Const SESS_ExtRefStatus As String = "SESS_EXTREFSTATUS"
            Public Const SESS_DHExtRefStatus As String = "SESS_DHEXTREFSTATUS"

            '------------------------------------------------------------------------------------------
            ' Popup Blocker 
            '------------------------------------------------------------------------------------------
            ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Public Const SESS_PopupBlockerShow As String = "SESS_POPUPBLOCKERSHOW"
            ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

            'CRE20-006 DHC integration [Start][Nichole]
            'declare the session for store the artifact parameter value
            Public Const SESS_DHCArtifact As String = "SESS_DHCARTIFACT"
            Public Const SESS_DHCClientInfo As String = "SESS_DHCClientINFO"
            'CRE20-006 DHC integration [End][Nichole]
            '------------------------------------------------------------------------------------------
            ' OCSSS - HKIC Symbol
            '------------------------------------------------------------------------------------------
            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Public Const SESS_HKIC_Symbol As String = "SESS_HKIC_SYMBOL"
            Public Const SESS_OCSSS_Ref_Status As String = "SESS_OCSSS_REF_STATUS"
            Public Const SESS_UIDisplayHKICSymbol As String = "SESS_UIDISPLAYHKICSYMBOL"
            ' CRE17-010 (OCSSS integration) [End][Chris YIM]

            ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Public Const SESS_BatchUploadScheme As String = "SESS_BATCHUPLOADSCHEME"
            ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

            ' CRE20-0XX (HA Scheme) [Start][Winnie]
            Public Const SESS_SchemeSelectedForPractice As String = "SESS_SCHEME_SELECTED_FOR_PRACTICE"
            Public Const SESS_HAPatient As String = "SESS_HA_PATIENT"
            ' CRE20-0XX (HA Scheme) [End][Winnie]

            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Public Const SESS_ClaimCOVID19 As String = "SESS_CLAIMCOVID19"
            Public Const SESS_ClaimCOVID19_Booth As String = "SESS_CLAIMCOVID19_BOOTH"
            Public Const SESS_ClaimCOVID19_Category As String = "SESS_CLAIMCOVID19_CATEGORY"
            Public Const SESS_ClaimCOVID19_MainCategory As String = "SESS_CLAIMCOVID19_MAINCATEGORY"
            Public Const SESS_ClaimCOVID19_SubCategory As String = "SESS_CLAIMCOVID19_SUBCATEGORY"
            Public Const SESS_ClaimCOVID19_VaccineBrand As String = "SESS_CLAIMCOVID19_VACCINEBRAND"
            Public Const SESS_ClaimCOVID19_VaccineLotNo As String = "SESS_CLAIMCOVID19_VACCINELOTNO"
            Public Const SESS_ClaimCOVID19_Dose As String = "SESS_CLAIMCOVID19_DOSE"
            Public Const SESS_ClaimCOVID19_VaccineRecord As String = "SESS_CLAIMCOVID19_VACCINERECORD" ' CRE20-0022 (Immu record) [End][Martin]
            Public Const SESS_ClaimCOVID19_DischargeRecord As String = "SESS_CLAIMCOVID19_DISCHARGERECORD"
            Public Const SESS_ClaimCOVID19_DischargeReminder As String = "SESS_CLAIMCOVID19_DISCHARGEREMINDER"
            Public Const SESS_ClaimCOVID19_DischargeDemographicReminder As String = "SESS_CLAIMCOVID19_DISCHARGEDEMOGRAPHICREMINDER"
            Public Const SESS_ClaimCOVID19_DischargeOverrideReason As String = "SESS_CLAIMCOVID19_DISCHARGEOVERRIDEREASON"
            Public Const SESS_ClaimFunctCode As String = "SESS_CLAIM_FUNCTCODE" 'CRE20-006 DHC Integration [End][Nichole]
            Public Const SESS_ClaimCOVID19_SchemeSelected As String = "SESS_CLAIMCOVID19_SCHEMESELECTED"
            Public Const SESS_CLAIMCOVID19_VaccinationCard As String = "SESS_CLAIMCOVID19_VACCINATIONCARD"
            Public Const SESS_CLAIMCOVID19_CarryForward As String = "SESS_CLAIMCOVID19_CARRYFORWARD"
            ' CRE20-0022 (Immu record) [End][Chris YIM]

        End Class

        Public Function Language() As String
            Return CType(HttpContext.Current.Session(SessionName.SESS_LANGUAGE), String)
        End Function


#Region "Current User Service Provider/Data Entry"
        Public Sub CurrentUserSaveToSession(ByVal udtSP As ServiceProviderModel, ByVal udtDataEntry As DataEntryUserModel)
            HttpContext.Current.Session(SessionName.SESS_ServiceProvider) = udtSP
            If Not udtDataEntry Is Nothing Then
                HttpContext.Current.Session(SessionName.SESS_DataEntry) = udtDataEntry
            End If
        End Sub

        Public Sub CurrentUserGetFromSession(ByRef udtSP As ServiceProviderModel, ByRef udtDataEntry As DataEntryUserModel)
            udtSP = CType(HttpContext.Current.Session(SessionName.SESS_ServiceProvider), ServiceProviderModel)
            udtDataEntry = CType(HttpContext.Current.Session(SessionName.SESS_DataEntry), DataEntryUserModel)
        End Sub

        Public Sub CurrentUserRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_ServiceProvider)
            HttpContext.Current.Session.Remove(SessionName.SESS_DataEntry)
        End Sub

        '==================================================================== Code for SmartID ============================================================================
        Public Sub CurrentPilotRunSmartIDSaveToSession(ByVal strFunctCode As String, ByVal dt As DataTable)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctCode, SessionName.SESS_PilotRunSmartID)) = dt
        End Sub

        Public Function CurrentPilotRunSmartIDGetFromSession(ByVal strFunctCode As String) As DataTable
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctCode, SessionName.SESS_PilotRunSmartID)), DataTable)
        End Function

        Public Sub CurrentPilotRunSmartIDRemoveFromSession(ByVal strFunctCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctCode, SessionName.SESS_PilotRunSmartID))
        End Sub
        '==================================================================================================================================================================
#End Region

#Region "EHS Claim"
        Public Sub EHSClaimSessionRemove(ByVal strFunctCode As String)
            'Status
            Me.AccountCreationProceedClaimRemoveFromSession()
            Me.AccountCreationComeFromClaimRemoveFromSession()
            Me.NotMatchAccountExistRemoveFromSession()
            Me.ExceedDocTypeLimitRemoveFromSession()

            'Account
            Me.EHSAccountRemoveFromSession(strFunctCode)
            Me.CCCodeRemoveFromSession(strFunctCode)

            'Transaction
            Me.EHSClaimVaccineRemoveFromSession()
            Me.EHSTransactionRemoveFromSession(strFunctCode)
            Me.EHSTransactionListRemoveFromSession(strFunctCode)

            'During Enter Claim Detail
            Me.EHSEnterClaimDetailSearchRCHRemoveFromSession()

            'Eligible Result
            Me.EligibleResultRemoveFromSession()

            '' Clear the Token Number since search action will update the related Session
            'Me.EHSClaimTokenNumberRemoveFromSession(strFunctCode)

            ' Clear printout function code
            Me.EHSClaimPrintoutFunctionCodeRemoveFromSession()

            'Scheme Claim Collection
            Me.SchemeSubsidizeListRemoveFromSession(strFunctCode)

            Me.ClaimCategoryRemoveFromSession(strFunctCode)

            Me.SmartIDContentRemoveFormSession(strFunctCode)
            Me.CurrentPilotRunSmartIDRemoveFromSession(strFunctCode)

            ' CMS vaccination result
            Me.CMSVaccineResultRemoveFromSession(strFunctCode)
            Me.CIMSVaccineResultRemoveFromSession(strFunctCode)

        End Sub

#End Region

        '-----------------------------------------------------------------------------------------------------------------------------------------------------------------
        'only For EHS Claim
        '-----------------------------------------------------------------------------------------------------------------------------------------------------------------

#Region "Search Account"
        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Sub HKICSymbolSaveToSession(ByVal strFunctionCode As String, ByVal strHKICSymbol As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_HKIC_Symbol)) = strHKICSymbol
        End Sub

        Public Function HKICSymbolGetFormSession(ByVal strFunctionCode As String) As String
            Return HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_HKIC_Symbol))
        End Function

        Public Sub HKICSymbolRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_HKIC_Symbol))
        End Sub

        Public Sub OCSSSRefStatusSaveToSession(ByVal strFunctionCode As String, ByVal strOCSSSRefStatus As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_OCSSS_Ref_Status)) = strOCSSSRefStatus
        End Sub

        Public Function OCSSSRefStatusGetFormSession(ByVal strFunctionCode As String) As String
            Return HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_OCSSS_Ref_Status))
        End Function

        Public Sub OCSSSRefStatusRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_OCSSS_Ref_Status))
        End Sub

        Public Sub UIDisplayHKICSymbolSaveToSession(ByVal strFunctionCode As String, ByVal blnDisplay As Boolean)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_UIDisplayHKICSymbol)) = blnDisplay
        End Sub

        Public Function UIDisplayHKICSymbolGetFormSession(ByVal strFunctionCode As String) As Boolean
            Return HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_UIDisplayHKICSymbol))
        End Function

        Public Sub UIDisplayHKICSymbolRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_UIDisplayHKICSymbol))
        End Sub
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]
#End Region

#Region "Temp Account record is exist in DataBase but DOB Not match (Detail not match) (For EHS Claim Only) (Can be Validated/Tempoary Account)"
        Public Sub NotMatchAccountExistSaveToSession(ByVal blnNotMatchAccountExist As Boolean)
            HttpContext.Current.Session(SessionName.SESS_NotMatchTempAccountExist) = blnNotMatchAccountExist
        End Sub

        Public Function NotMatchAccountExistGetFromSession() As Boolean
            If HttpContext.Current.Session(SessionName.SESS_NotMatchTempAccountExist) Is Nothing Then
                Return False
            Else
                Return CType(HttpContext.Current.Session(SessionName.SESS_NotMatchTempAccountExist), Boolean)
            End If
        End Function

        Public Sub NotMatchAccountExistRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_NotMatchTempAccountExist)
        End Sub
#End Region

#Region "Cross Search: Sreach By HKID -> Return HKBC Personal Informtion but the recipient age is exceed DocType Limit"
        Public Sub ExceedDocTypeLimitSaveToSession(ByVal blnExceedDocTypeLimit As Boolean)
            HttpContext.Current.Session(SessionName.SESS_ExceedDocTypeLimit) = blnExceedDocTypeLimit
        End Sub

        Public Function ExceedDocTypeLimitGetFromSession() As Boolean
            If HttpContext.Current.Session(SessionName.SESS_ExceedDocTypeLimit) Is Nothing Then
                Return False
            Else
                Return CType(HttpContext.Current.Session(SessionName.SESS_ExceedDocTypeLimit), Boolean)
            End If
        End Function

        Public Sub ExceedDocTypeLimitRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_ExceedDocTypeLimit)
        End Sub
#End Region

#Region "Account Creation Status (For EHS Claim Only)"
        Public Sub AccountCreationProceedClaimSaveToSession(ByVal isAccountCreationProceedClaim As Boolean)
            HttpContext.Current.Session(SessionName.SESS_AccountCreationProceedClaim) = isAccountCreationProceedClaim
        End Sub

        Public Function AccountCreationProceedClaimGetFromSession() As Boolean
            If HttpContext.Current.Session(SessionName.SESS_AccountCreationProceedClaim) Is Nothing Then
                Return False
            Else
                Return CType(HttpContext.Current.Session(SessionName.SESS_AccountCreationProceedClaim), Boolean)
            End If
        End Function

        Public Sub AccountCreationProceedClaimRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_AccountCreationProceedClaim)
        End Sub
#End Region

#Region "Account Creation Come From Claim (For EHS Claim Only)"
        Public Sub AccountCreationComeFromClaimSaveToSession(ByVal isAccountCreationComeFromClaim As Boolean)
            HttpContext.Current.Session(SessionName.SESS_AccountCreationComeFromClaim) = isAccountCreationComeFromClaim
        End Sub

        Public Function AccountCreationComeFromClaimGetFromSession() As Boolean
            If HttpContext.Current.Session(SessionName.SESS_AccountCreationComeFromClaim) Is Nothing Then
                Return False
            Else
                Return CType(HttpContext.Current.Session(SessionName.SESS_AccountCreationComeFromClaim), Boolean)
            End If
        End Function

        Public Sub AccountCreationComeFromClaimRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_AccountCreationComeFromClaim)
        End Sub
#End Region

#Region "EHS Claim Vaccine (For EHS Claim Only)"
        Public Sub EHSClaimVaccineSaveToSession(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel)
            HttpContext.Current.Session(SessionName.SESS_EHSClaimVaccine) = udtEHSClaimVaccine
        End Sub

        Public Function EHSClaimVaccineGetFromSession() As EHSClaimVaccineModel
            If HttpContext.Current.Session(SessionName.SESS_EHSClaimVaccine) Is Nothing Then
                Return Nothing
            Else
                Return CType(HttpContext.Current.Session(SessionName.SESS_EHSClaimVaccine), EHSClaimVaccineModel)
            End If
        End Function

        Public Sub EHSClaimVaccineRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_EHSClaimVaccine)
        End Sub
#End Region

#Region "CCCode control"
        Public Sub CCCodeSaveToSession(ByVal strFunctionCode As String, ByVal strCCCodeSessionName As String, ByVal strCCCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, strCCCodeSessionName)) = strCCCode
        End Sub

        Public Function CCCodeGetFormSession(ByVal strFunctionCode As String, ByVal strCCCodeSessionName As String) As String
            Return HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, strCCCodeSessionName))
        End Function

        Public Sub CCCodeRemoveFromSession(ByVal strFunctionCode As String, ByVal strCCCodeSessionName As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, strCCCodeSessionName))
        End Sub

        Public Sub CCCodeRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CCCode1))
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CCCode2))
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CCCode3))
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CCCode4))
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CCCode5))
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CCCode6))
        End Sub
#End Region

#Region "Seach RCH during enter claim Detail"
        Public Sub EHSEnterClaimDetailSearchRCHSaveToSession(ByVal isSearchingRCH As Boolean)
            HttpContext.Current.Session(SessionName.SESS_EHSEnterClaimDetailSearchRCH) = isSearchingRCH
        End Sub

        Public Function EHSEnterClaimDetailSearchRCHGetFromSession() As Boolean
            If HttpContext.Current.Session(SessionName.SESS_EHSEnterClaimDetailSearchRCH) Is Nothing Then
                Return False
            Else
                Return CType(HttpContext.Current.Session(SessionName.SESS_EHSEnterClaimDetailSearchRCH), Boolean)
            End If
        End Function

        Public Sub EHSEnterClaimDetailSearchRCHRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_EHSEnterClaimDetailSearchRCH)
        End Sub
#End Region

#Region "Seach Outreach List during enter claim Detail"
        Public Sub EHSEnterClaimDetailSearchOutreachSaveToSession(ByVal blnSearchOutreach As Boolean)
            HttpContext.Current.Session(SessionName.SESS_EHSEnterClaimDetailSearchOutreach) = blnSearchOutreach
        End Sub

        Public Function EHSEnterClaimDetailSearchOutreachGetFromSession() As Boolean
            If HttpContext.Current.Session(SessionName.SESS_EHSEnterClaimDetailSearchOutreach) Is Nothing Then
                Return False
            Else
                Return CType(HttpContext.Current.Session(SessionName.SESS_EHSEnterClaimDetailSearchOutreach), Boolean)
            End If
        End Function

        Public Sub EHSEnterClaimDetailSearchOutreachRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_EHSEnterClaimDetailSearchOutreach)
        End Sub

        Public Sub OutreachCodeSaveToSession(ByVal strFunctionCode As String, ByVal strOutreachCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_OutreachCode)) = strOutreachCode.Trim()
        End Sub

        Public Function OutreachCodeGetFromSession(ByVal strFunctionCode As String) As String
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_OutreachCode)), String)
        End Function

        Public Sub OutreachCodeRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_OutreachCode))
        End Sub

#End Region

#Region "Eligible Rules"
        Public Sub EligibleResultSaveToSession(ByVal udtRuleResults As RuleResultCollection)
            HttpContext.Current.Session(SessionName.SESS_RuleResults) = udtRuleResults
        End Sub

        Public Function EligibleResultGetFromSession() As RuleResultCollection
            Return CType(HttpContext.Current.Session(SessionName.SESS_RuleResults), RuleResultCollection)
        End Function

        Public Sub EligibleResultRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_RuleResults)
        End Sub
#End Region

#Region "EHS Claim Steps"
        Public Sub EHSClaimStepsSaveToSession(ByVal strFunctCode As String, ByVal intActiveViewIndex As Integer)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctCode, SessionName.SESS_EHSClaimSteps)) = intActiveViewIndex
        End Sub

        Public Function EHSClaimStepsGetFromSession(ByVal strFunctCode As String) As Integer
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctCode, SessionName.SESS_EHSClaimSteps)), Integer)
        End Function

        Public Sub EHSClaimStepsRemoveFromSession(ByVal strFunctCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctCode, SessionName.SESS_EHSClaimSteps))
        End Sub
#End Region

        'CRE20-009 VSS DA With CSSA - store the documentary proof [Start][Nichole]
#Region "EHS Documentary Proof"
        Public Sub EHSDocProofSaveToSession(ByVal strFunctCode As String, ByVal strDocProof As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctCode, SessionName.SESS_DocumentaryProof)) = strDocProof
        End Sub

        Public Function EHSDocProofGetFromSession(ByVal strFunctCode As String) As String
            Return HttpContext.Current.Session(String.Format("{0}_{1}", strFunctCode, SessionName.SESS_DocumentaryProof))
        End Function

        Public Sub EHSDocProofRemoveFromSession(ByVal strFunctCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctCode, SessionName.SESS_DocumentaryProof))
        End Sub
#End Region
        'CRE20-009 VSS DA With CSSA - store the documentary proof [End][Nichole]

#Region "EHS Claim Confirm Message"
        Public Sub EHSClaimConfirmMessageSaveToSession(ByVal strFunctCode As String, ByVal obj As Object)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctCode, SessionName.SESS_EHSClaimConfirmMessage)) = obj
        End Sub

        Public Function EHSClaimConfirmMessageGetFromSession(ByVal strFunctCode As String) As Object
            Return HttpContext.Current.Session(String.Format("{0}_{1}", strFunctCode, SessionName.SESS_EHSClaimConfirmMessage))
        End Function

        Public Sub EHSClaimConfirmMessageRemoveFromSession(ByVal strFunctCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctCode, SessionName.SESS_EHSClaimConfirmMessage))
        End Sub
#End Region

#Region "EHS Claim Printout Step"

        Public Sub EHSClaimPrintoutFunctionCodeSaveToSession(ByVal strFunctCode As String)
            HttpContext.Current.Session(String.Format("{0}", SessionName.SESS_EHSClaimPrintOutFunctionCode)) = strFunctCode
        End Sub

        Public Function EHSClaimPrintoutFunctionCodeGetFromSession() As String
            Return HttpContext.Current.Session(String.Format("{0}", SessionName.SESS_EHSClaimPrintOutFunctionCode))
        End Function

        Public Sub EHSClaimPrintoutFunctionCodeRemoveFromSession()
            HttpContext.Current.Session.Remove(String.Format("{0}", SessionName.SESS_EHSClaimPrintOutFunctionCode))
        End Sub

#End Region

#Region "Scheme Collection with Subsidize"

        Public Sub SchemeSubsidizeListSaveToSession(ByVal udtSchemeClaim As SchemeClaimModelCollection, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SchemeSubsidizeList)) = udtSchemeClaim
        End Sub

        Public Function SchemeSubsidizeListGetFromSession(ByVal strFunctionCode As String) As SchemeClaimModelCollection
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SchemeSubsidizeList)), SchemeClaimModelCollection)
        End Function

        Public Sub SchemeSubsidizeListRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SchemeSubsidizeList))
        End Sub

#End Region

#Region "Claim Category"

        Public Sub ClaimCategorySaveToSession(ByVal udtClaimCategory As Common.Component.ClaimCategory.ClaimCategoryModel, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCategory)) = udtClaimCategory
        End Sub

        Public Function ClaimCategoryGetFromSession(ByVal strFunctionCode As String) As Common.Component.ClaimCategory.ClaimCategoryModel
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCategory)), Common.Component.ClaimCategory.ClaimCategoryModel)
        End Function

        Public Sub ClaimCategoryRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCategory))
        End Sub

#End Region

#Region "External vaccination source (CMS Vaccination)"
        Public Sub CMSVaccineResultSaveToSession(ByVal udtVaccineResult As Common.WebService.Interface.HAVaccineResult, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CMSVaccineResult)) = udtVaccineResult
        End Sub

        Public Function CMSVaccineResultGetFromSession(ByVal strFunctionCode As String) As Common.WebService.Interface.HAVaccineResult
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CMSVaccineResult)), Common.WebService.Interface.HAVaccineResult)
        End Function

        Public Sub CMSVaccineResultRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CMSVaccineResult))
        End Sub
#End Region

#Region "External vaccination source (CIMS Vaccination)"
        Public Sub CIMSVaccineResultSaveToSession(ByVal udtVaccineResult As Common.WebService.Interface.DHVaccineResult, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CIMSVaccineResult)) = udtVaccineResult
        End Sub

        Public Function CIMSVaccineResultGetFromSession(ByVal strFunctionCode As String) As Common.WebService.Interface.DHVaccineResult
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CIMSVaccineResult)), Common.WebService.Interface.DHVaccineResult)
        End Function

        Public Sub CIMSVaccineResultRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CIMSVaccineResult))
        End Sub
#End Region

        'CRE15-003 System-generated Form [Start][Philip Chau]
#Region "EHSClaimV1 temporary TransactionID"

        Public Sub EHSClaimTempTransactionIDSaveToSession(ByVal strTranactionID As String)
            HttpContext.Current.Session(SessionName.SESS_EHSClaimTempTransactionID) = strTranactionID
        End Sub

        Public Function EHSClaimTempTransactionIDGetFromSession() As String
            Dim strResult As String = String.Empty
            If HttpContext.Current.Session(SessionName.SESS_EHSClaimTempTransactionID) IsNot Nothing Then
                strResult = CType(HttpContext.Current.Session(SessionName.SESS_EHSClaimTempTransactionID), String)
            End If
            Return strResult
        End Function

        Public Sub EHSClaimTempTransactionIDRemoveFromSession()
            If HttpContext.Current.Session(SessionName.SESS_EHSClaimTempTransactionID) IsNot Nothing Then
                HttpContext.Current.Session.Remove(SessionName.SESS_EHSClaimTempTransactionID)
            End If
        End Sub

        Public Sub EHSClaimStep3ShowLastestTransactionIDSaveToSession(ByVal blnShowLatestTranactionID As Boolean)
            HttpContext.Current.Session(SessionName.SESS_EHSClaimStep3ShowLastestTransactionID) = blnShowLatestTranactionID
        End Sub

        Public Function EHSClaimStep3ShowLastestTransactionIDGetFromSession() As Boolean
            Dim blnResult As Boolean
            If HttpContext.Current.Session(SessionName.SESS_EHSClaimStep3ShowLastestTransactionID) IsNot Nothing Then
                blnResult = CType(HttpContext.Current.Session(SessionName.SESS_EHSClaimStep3ShowLastestTransactionID), Boolean)
            End If
            Return blnResult
        End Function

        Public Sub EHSClaimStep3ShowLastestTransactionIDRemoveFromSession()
            If HttpContext.Current.Session(SessionName.SESS_EHSClaimStep3ShowLastestTransactionID) IsNot Nothing Then
                HttpContext.Current.Session.Remove(SessionName.SESS_EHSClaimStep3ShowLastestTransactionID)
            End If
        End Sub

#End Region
        'CRE15-003 System-generated Form [End][Philip Chau]

        '-----------------------------------------------------------------------------------------------------------------------------------------------------------------
        'only For text only version Void EHS Claim
        '-----------------------------------------------------------------------------------------------------------------------------------------------------------------
#Region "Pre- filled record is differe doc type "
        Public Sub TextOnlyVersionSearchTypeSaveToSession(ByVal enumSearchType As VoidClaimSearch.SearchType)
            HttpContext.Current.Session(SessionName.SESS_SelectedSearchType) = enumSearchType
        End Sub

        Public Function TextOnlyVersionSearchTypeGetFromSession() As VoidClaimSearch.SearchType
            If HttpContext.Current.Session(SessionName.SESS_SelectedSearchType) Is Nothing Then
                Return VoidClaimSearch.SearchType.Empty
            Else
                Return CType(HttpContext.Current.Session(SessionName.SESS_SelectedSearchType), VoidClaimSearch.SearchType)
            End If
        End Function

        Public Sub TextOnlyVersionSearchTypeRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_SelectedSearchType)
        End Sub
#End Region

#Region "Is Mobile Device"
        Public Sub IsMobileDeviceSaveToSession(ByVal isMobileDevice As Boolean)
            HttpContext.Current.Session(SessionName.SESS_IsMobileDevice) = isMobileDevice
        End Sub

        Public Function IsMobileDeviceGetFromSession() As Object
            Dim isMobileDevice As Object = HttpContext.Current.Session(SessionName.SESS_IsMobileDevice)
            If Not isMobileDevice Is Nothing Then
                Return isMobileDevice
            Else
                Return Nothing
            End If
        End Function

        Public Sub IsMobileDeviceRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_IsMobileDevice)
        End Sub
#End Region

#Region "Void Transaction by searching eHealth Account"
        Public Sub eHSAccDocTypeAndDocNumSaveToSession(ByVal strDocType As String, ByVal strDocNum As String, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_eHSAccDocType)) = strDocType
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_eHSAccDocNum)) = strDocNum
        End Sub

        Public Sub eHSAccDocTypeAndDocNumGetFromSession(ByRef strDocType As String, ByRef strDocNum As String, ByVal strFunctionCode As String)
            If IsNothing(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_eHSAccDocType))) Then
                strDocType = String.Empty
            Else
                strDocType = CStr(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_eHSAccDocType)))
            End If

            If IsNothing(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_eHSAccDocNum))) Then
                strDocNum = String.Empty
            Else
                strDocNum = CStr(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_eHSAccDocNum)))
            End If

        End Sub

        Public Sub eHSAccDocTypeAndDocNumRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_eHSAccDocType))
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_eHSAccDocNum))
        End Sub
#End Region

        '-----------------------------------------------------------------------------------------------------------------------------------------------------------------
        'Permanent Session
        '-----------------------------------------------------------------------------------------------------------------------------------------------------------------
#Region "Transaction"
        Public Sub EHSTransactionSaveToSession(ByVal udtEHSClaimVaccine As EHSTransactionModel, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSTransaction)) = udtEHSClaimVaccine
        End Sub

        Public Function EHSTransactionGetFromSession(ByVal strFunctionCode As String) As EHSTransactionModel
            If HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSTransaction)) Is Nothing Then
                Return Nothing
            Else
                Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSTransaction)), EHSTransactionModel)
            End If
        End Function

        Public Sub EHSTransactionRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSTransaction))
        End Sub

        Public Sub EHSTransactionOrginalSaveToSession(ByVal udtEHSClaimVaccine As EHSTransactionModel, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSTransaction_Orginal)) = udtEHSClaimVaccine
        End Sub

        Public Function EHSTransactionOrginalGetFromSession(ByVal strFunctionCode As String) As EHSTransactionModel
            If HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSTransaction_Orginal)) Is Nothing Then
                Return Nothing
            Else
                Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSTransaction_Orginal)), EHSTransactionModel)
            End If
        End Function

        Public Sub EHSTransactionOrginalRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSTransaction))
        End Sub
#End Region

#Region "Transaction"
        Public Sub EHSTransactionListSaveToSession(ByVal udtEHSClaimVaccines As EHSTransactionModelCollection, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSTransactions)) = udtEHSClaimVaccines
        End Sub

        Public Function EHSTransactionListGetFromSession(ByVal strFunctionCode As String) As EHSTransactionModelCollection
            If HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSTransactions)) Is Nothing Then
                Return Nothing
            Else
                Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSTransactions)), EHSTransactionModelCollection)
            End If
        End Function

        Public Sub EHSTransactionListRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSTransactions))
        End Sub

        Public Sub EHSTransactionListPageSaveToSession(ByVal strPageParameter As String, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSTransactions_Page)) = strPageParameter
        End Sub

        Public Function EHSTransactionListPageGetFromSession(ByVal strFunctionCode As String) As String
            If HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSTransactions_Page)) Is Nothing Then
                Return Nothing
            Else
                Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSTransactions_Page)), String)
            End If
        End Function

        Public Sub EHSTransactionListPageRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSTransactions_Page))
        End Sub

        Public Sub EHSTransactionSearchByNoSaveToSession(ByVal isSearchByNo As Boolean, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSTransactions_SearchByNo)) = isSearchByNo
        End Sub

        Public Function EHSTransactionSearchByNoGetFromSession(ByVal strFunctionCode As String) As Boolean
            If HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSTransactions_SearchByNo)) Is Nothing Then
                Return Nothing
            Else
                Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSTransactions_SearchByNo)), Boolean)
            End If
        End Function

        Public Sub EHSTransactionSearchByNoRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSTransactions_SearchByNo))
        End Sub
#End Region

#Region "Practice Display Collection"
        Public Sub PracticeDisplayListSaveToSession(ByVal udtPracticeDisplay As PracticeDisplayModelCollection)
            HttpContext.Current.Session(SessionName.SESS_SelectedPracticeDsiplayList) = udtPracticeDisplay
        End Sub

        Public Function PracticeDisplayListGetFromSession() As PracticeDisplayModelCollection
            Return CType(HttpContext.Current.Session(SessionName.SESS_SelectedPracticeDsiplayList), PracticeDisplayModelCollection)
        End Function

        Public Sub PracticeDisplayListRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_SelectedPracticeDsiplayList)
        End Sub

        Public Sub PracticeDisplayListSaveToSession(ByVal udtPracticeDisplay As PracticeDisplayModelCollection, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SelectedPracticeDsiplayList)) = udtPracticeDisplay
        End Sub

        Public Function PracticeDisplayListGetFromSession(ByVal strFunctionCode As String) As PracticeDisplayModelCollection
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SelectedPracticeDsiplayList)), PracticeDisplayModelCollection)
        End Function

        Public Sub PracticeDisplayListRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SelectedPracticeDsiplayList))
        End Sub

#End Region

#Region "Selected Practice Display"
        Public Sub PracticeDisplaySaveToSession(ByVal udtPracticeDisplay As PracticeDisplayModel, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SelectedPracticeDsiplay)) = udtPracticeDisplay
        End Sub

        Public Function PracticeDisplayGetFromSession(ByVal strFunctionCode As String) As PracticeDisplayModel
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SelectedPracticeDsiplay)), PracticeDisplayModel)
        End Function

        Public Sub PracticeDisplayRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SelectedPracticeDsiplay))
        End Sub

        Public Sub ConfirmedPracticePopUpSaveToSession(ByVal isConfirmedPracticePopUp As Boolean)
            HttpContext.Current.Session(SessionName.SESS_ConfirmedPracticePopup) = isConfirmedPracticePopUp
        End Sub

        Public Function ConfirmedPracticePopUpGetFromSession() As Boolean
            Return CType(HttpContext.Current.Session(SessionName.SESS_ConfirmedPracticePopup), Boolean)
        End Function

        Public Sub ConfirmedPracticePopUpRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_ConfirmedPracticePopup)
        End Sub

#End Region

#Region "eHealth Account"

        Public Sub EHSAccountSaveToSession(ByVal udtEHSAccount As EHSAccountModel, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSAccount)) = udtEHSAccount
        End Sub

        Public Function EHSAccountGetFromSession(ByVal strFunctionCode As String) As EHSAccountModel
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSAccount)), EHSAccountModel)
        End Function

        Public Sub EHSAccountRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSAccount))
        End Sub

        '

        Public Sub SearchAccountStatusSaveToSession(ByVal udtSearchAccountStatus As EHSClaimBLL.SearchAccountStatus)
            HttpContext.Current.Session(SessionName.SESS_SearchAccountStatus) = udtSearchAccountStatus
        End Sub

        Public Function SearchAccountStatusGetFormSession() As EHSClaimBLL.SearchAccountStatus
            Return HttpContext.Current.Session(SessionName.SESS_SearchAccountStatus)
        End Function

        Public Sub SearchAccountStatusRemoveFormSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_SearchAccountStatus)
        End Sub

#End Region

        ' CRE20-0XX (HA Scheme) [Start][Winnie]
#Region "Scheme Selected for Practice Selection"

        Public Sub SchemeSelectedForPracticeSaveToSession(ByVal strSchemeCode As String, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SchemeSelectedForPractice)) = strSchemeCode
        End Sub

        Public Function SchemeSelectedForPracticeGetFromSession(ByVal strFunctionCode As String) As String
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SchemeSelectedForPractice)), String)
        End Function

        Public Sub SchemeSelectedForPracticeRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SchemeSelectedForPractice))
        End Sub
        ' CRE20-0XX (HA Scheme) [End][Winnie]
#End Region


#Region "Scheme Selected"

        Public Sub SchemeSelectedSaveToSession(ByVal udtSchemeClaim As SchemeClaimModel, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SchemeSelected)) = udtSchemeClaim
        End Sub

        Public Function SchemeSelectedGetFromSession(ByVal strFunctionCode As String) As SchemeClaimModel
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SchemeSelected)), SchemeClaimModel)
        End Function

        Public Sub SchemeSelectedRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SchemeSelected))
        End Sub

#End Region

        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
#Region "Scheme Claim List"
        Public Sub SchemeClaimListSaveToSession(ByVal udtSchemeClaimList As SchemeClaimModelCollection, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_BatchUploadScheme)) = udtSchemeClaimList
        End Sub

        Public Function SchemeClaimListGetFromSession(ByVal strFunctionCode As String) As SchemeClaimModelCollection
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_BatchUploadScheme)), SchemeClaimModelCollection)
        End Function

        Public Sub SchemeClaimListRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_BatchUploadScheme))
        End Sub

#End Region
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
#Region "Non Clinic Setting"

        Public Sub NonClinicSettingSaveToSession(ByVal blnNonClinicSetting As Boolean, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_NonClinicSetting)) = blnNonClinicSetting
        End Sub

        Public Function NonClinicSettingGetFromSession(ByVal strFunctionCode As String) As Boolean
            Return HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_NonClinicSetting))
        End Function

        Public Sub NonClinicSettingRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_NonClinicSetting))
        End Sub

#End Region

#Region "Change Scheme In Practice"

        Public Sub ChangeSchemeInPracticeSaveToSession(ByVal blnChangeSchemeInPractice As Boolean, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ChangeSchemeInPractice)) = blnChangeSchemeInPractice
        End Sub

        Public Function ChangeSchemeInPracticeGetFromSession(ByVal strFunctionCode As String) As Boolean
            Return HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ChangeSchemeInPractice))
        End Function

        Public Sub ChangeSchemeInPracticeRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ChangeSchemeInPractice))
        End Sub

#End Region

#Region "Claim For Same Patient"

        Public Sub ClaimForSamePatientSaveToSession(ByVal blnClaimForSamePatient As Boolean, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimForSamePatient)) = blnClaimForSamePatient
        End Sub

        Public Function ClaimForSamePatientGetFromSession(ByVal strFunctionCode As String) As Boolean
            Return HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimForSamePatient))
        End Function

        Public Sub ClaimForSamePatientRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimForSamePatient))
        End Sub

#End Region

#Region "VSS Children Reminder"

        Public Sub EligibleResultVSSReminderSaveToSession(ByVal udtRuleResults As RuleResultCollection)
            HttpContext.Current.Session(SessionName.SESS_EligibleResultVSSReminder) = udtRuleResults
        End Sub

        Public Function EligibleResultVSSReminderGetFromSession() As RuleResultCollection
            Return CType(HttpContext.Current.Session(SessionName.SESS_EligibleResultVSSReminder), RuleResultCollection)
        End Function

        Public Sub EligibleResultVSSReminderRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_EligibleResultVSSReminder)
        End Sub

#End Region

#Region "PID Institution Code"

        Public Sub PIDInstitutionCodeSaveToSession(ByVal strPIDInstitutionCode As String, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_PIDInstitutionCode)) = strPIDInstitutionCode
        End Sub

        Public Function PIDInstitutionCodeGetFromSession(ByVal strFunctionCode As String) As String
            Return HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_PIDInstitutionCode))
        End Function

        Public Sub PIDInstitutionCodeRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_PIDInstitutionCode))
        End Sub

#End Region

#Region "Place of Vaccination"

        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Public Sub PlaceVaccinationSaveToSession(ByVal strPlaceVaccination As String, ByVal strFunctionCode As String, ByVal strSchemeCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}_{2}", strFunctionCode, strSchemeCode, SessionName.SESS_PlaceOfVaccination)) = strPlaceVaccination
        End Sub

        Public Function PlaceVaccinationGetFromSession(ByVal strFunctionCode As String, ByVal strSchemeCode As String) As String
            Return HttpContext.Current.Session(String.Format("{0}_{1}_{2}", strFunctionCode, strSchemeCode, SessionName.SESS_PlaceOfVaccination))
        End Function

        Public Sub PlaceVaccinationRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}_{2}", strFunctionCode, SchemeClaimModel.VSS, SessionName.SESS_PlaceOfVaccination))
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}_{2}", strFunctionCode, SchemeClaimModel.ENHVSSO, SessionName.SESS_PlaceOfVaccination))
        End Sub
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

#End Region

#Region "Place of Vaccination Other"

        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Public Sub PlaceVaccinationOtherSaveToSession(ByVal strPlaceVaccinationOther As String, ByVal strFunctionCode As String, ByVal strSchemeCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}_{2}", strFunctionCode, strSchemeCode, SessionName.SESS_PlaceOfVaccinationOther)) = strPlaceVaccinationOther
        End Sub

        Public Function PlaceVaccinationOtherGetFromSession(ByVal strFunctionCode As String, ByVal strSchemeCode As String) As String
            Return HttpContext.Current.Session(String.Format("{0}_{1}_{2}", strFunctionCode, strSchemeCode, SessionName.SESS_PlaceOfVaccinationOther))
        End Function

        Public Sub PlaceVaccinationOtherRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}_{2}", strFunctionCode, SchemeClaimModel.VSS, SessionName.SESS_PlaceOfVaccinationOther))
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}_{2}", strFunctionCode, SchemeClaimModel.ENHVSSO, SessionName.SESS_PlaceOfVaccinationOther))
        End Sub
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

#End Region
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
#Region "HighRisk"
        Public Sub HighRiskSaveToSession(ByVal strHighRisk As String, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_HighRisk)) = strHighRisk
        End Sub

        Public Function HighRiskGetFromSession(ByVal strFunctionCode As String) As String
            If HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_HighRisk)) Is Nothing Then
                Return String.Empty
            Else
                Return HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_HighRisk))
            End If
        End Function

        Public Sub HighRiskRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_HighRisk))
        End Sub
#End Region

#Region "SubsidizeDisabledDetail"
        Public Sub SubsidizeDisabledDetailKeySaveToSession(ByVal strKey As String, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SubsidizeDisabledDetailKey)) = strKey
        End Sub

        Public Function SubsidizeDisabledDetailKeyGetFromSession(ByVal strFunctionCode As String) As String
            If HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SubsidizeDisabledDetailKey)) Is Nothing Then
                Return String.Empty
            Else
                Return HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SubsidizeDisabledDetailKey))
            End If
        End Function

        Public Sub SubsidizeDisabledDetailKeyRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SubsidizeDisabledDetailKey))
        End Sub
#End Region

        'CRE16-026 (Add PCV13) [End][Chris YIM]


#Region "Document Type Selected"

        Public Sub DocumentTypeSelectedSaveToSession(ByVal strDocumentTypeCode As String, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_DocumentTypeSelected)) = strDocumentTypeCode
        End Sub

        Public Function DocumentTypeSelectedGetFromSession(ByVal strFunctionCode As String) As String
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_DocumentTypeSelected)), String)
        End Function

        Public Sub DocumentTypeSelectedRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_DocumentTypeSelected))
        End Sub

#End Region

#Region "Concurrent Browser"

        Public Function EHSClaimTokenNumberSaveToSession(ByVal strFunctionCode As String) As String

            'Dim intMin As Integer = 14
            'Dim intMax As Integer = 18
            'Dim rand As New Random()
            'Dim Builder As StringBuilder = New StringBuilder()
            ''Dim intCodeLength, i, j As Integer
            'Dim intCodeLength = rand.Next(intMin, intMax)

            'For j As Integer = 0 To intCodeLength - 1
            '    Dim i = rand.Next(0, 2)
            '    If i = 0 Then
            '        Builder.Append(rand.Next(0, 10))
            '    Else
            '        Builder.Append(Convert.ToChar((rand.Next(0, 26) + 65)))
            '    End If
            'Next

            Dim strResult As String = Common.ComFunction.GeneralFunction.generateBrowserTokenNum()
            HttpContext.Current.Session(String.Format("{0}", SessionName.SESS_EnterClaimToken)) = strResult
            Return strResult

        End Function

        Public Function EHSClaimTokenNumberGetFromSession(ByVal strFunctionCode As String) As String
            Return CType(HttpContext.Current.Session(String.Format("{0}", SessionName.SESS_EnterClaimToken)), String)
        End Function

        Public Sub EHSClaimTokenNumberRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}", SessionName.SESS_EnterClaimToken))
        End Sub
#End Region

#Region "Popup Blocker"
        ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Sub PopupBlockerSaveToSession(ByVal blnShow As Boolean)
            HttpContext.Current.Session(String.Format("{0}", SessionName.SESS_PopupBlockerShow)) = blnShow
        End Sub

        Public Function PopupBlockerGetFromSession() As Boolean
            If IsNothing(HttpContext.Current.Session(String.Format("{0}", SessionName.SESS_PopupBlockerShow))) Then
                HttpContext.Current.Session(String.Format("{0}", SessionName.SESS_PopupBlockerShow)) = False
            End If

            Return CType(HttpContext.Current.Session(String.Format("{0}", SessionName.SESS_PopupBlockerShow)), Boolean)
        End Function

        Public Sub PopupBlockerRemoveFromSession()
            HttpContext.Current.Session.Remove(String.Format("{0}", SessionName.SESS_PopupBlockerShow))
        End Sub
        ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

#End Region

#Region "DHC service"
        'CRE20-006 DHC Claim service [Start][Nichole]

        Public Sub ArtifactSaveToSession(ByVal strFunctionCode As String, ByVal strArtifact As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_DHCArtifact)) = strArtifact
        End Sub
        Public Sub ArtifactRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_DHCArtifact))
        End Sub

        Public Function ArtifactGetFromSession(ByVal strFunctionCode As String) As String
            Return HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_DHCArtifact))
        End Function

        Public Sub DHCInfoSaveToSession(ByVal strFunctionCode As String, ByVal udtDHCInfo As DHCClaim.DHCClaimBLL.DHCPersonalInformationModel)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_DHCClientInfo)) = udtDHCInfo
        End Sub

        Public Function DHCInfoGetFromSession(ByVal strFunctionCode As String) As DHCClaim.DHCClaimBLL.DHCPersonalInformationModel
            Return HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_DHCClientInfo))
        End Function
        'CRE20-006 DHC Claim Service [End][Nichole]
#End Region
#Region "Vaccination Record Enquiry"

        Public Sub FromVaccinationRecordEnquirySaveToSession(ByVal value As Boolean)
            HttpContext.Current.Session(SessionName.SESS_FromVaccinationRecordEnquiry) = value
        End Sub

        Public Function FromVaccinationRecordEnquiryGetFromSession() As Boolean
            If HttpContext.Current.Session(SessionName.SESS_FromVaccinationRecordEnquiry) Is Nothing Then
                Return False
            Else
                Return CType(HttpContext.Current.Session(SessionName.SESS_FromVaccinationRecordEnquiry), Boolean)
            End If
        End Function

        Public Sub FromVaccinationRecordEnquiryRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_FromVaccinationRecordEnquiry)
        End Sub

        '  

        Public Sub VREEHSAccountSaveToSession(ByVal value As EHSAccountModel)
            HttpContext.Current.Session(SessionName.SESS_VREEHSAccount) = value
        End Sub

        Public Function VREEHSAccountGetFromSession() As EHSAccountModel
            Return HttpContext.Current.Session(SessionName.SESS_VREEHSAccount)
        End Function

        Public Sub VREEHSAccountRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_VREEHSAccount)
        End Sub

        '

        Public Sub ExtRefStatusSaveToSession(ByVal value As EHSTransactionModel.ExtRefStatusClass)
            HttpContext.Current.Session(SessionName.SESS_ExtRefStatus) = value
        End Sub

        Public Function ExtRefStatusGetFromSession() As EHSTransactionModel.ExtRefStatusClass
            If IsNothing(HttpContext.Current.Session) Then Return Nothing

            Return HttpContext.Current.Session(SessionName.SESS_ExtRefStatus)
        End Function

        Public Sub ExtRefStatusRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_ExtRefStatus)
        End Sub

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Sub DHExtRefStatusSaveToSession(ByVal value As EHSTransactionModel.ExtRefStatusClass)
            HttpContext.Current.Session(SessionName.SESS_DHExtRefStatus) = value
        End Sub

        Public Function DHExtRefStatusGetFromSession() As EHSTransactionModel.ExtRefStatusClass
            If IsNothing(HttpContext.Current.Session) Then Return Nothing

            Return HttpContext.Current.Session(SessionName.SESS_DHExtRefStatus)
        End Function

        Public Sub DHExtRefStatusRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_DHExtRefStatus)
        End Sub
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        Public Sub ClearVREClaim()
            FromVaccinationRecordEnquiryRemoveFromSession()
            VREEHSAccountRemoveFromSession()
            SmartIDContentRemoveFormSession(ClaimFunctCodeGetFromSession)  'CRE20-0xx Immue record [Nichole]
            SmartIDContentRemoveFormSession(FunctCode.FUNT020801)
            EHSAccountRemoveFromSession(ClaimFunctCodeGetFromSession) 'CRE20-0xx Immue record [Nichole]
            EHSAccountRemoveFromSession(FunctCode.FUNT020801)
            ExtRefStatusRemoveFromSession()
        End Sub

#End Region

        Public Sub RVPRCHCodeSaveToSession(ByVal strFunctionCode As String, ByVal strRCHCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_RVPRCHCode)) = strRCHCode.Trim()
        End Sub

        Public Function RVPRCHCodeGetFromSession(ByVal strFunctionCode As String) As String
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_RVPRCHCode)), String)
        End Function

        Public Sub RVPRCHCodeRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_RVPRCHCode))
        End Sub

        '---------------------------------------------------------------------------------------
        ' SmartID
        '---------------------------------------------------------------------------------------
        'SmartIDContent Model
        Public Sub SmartIDContentSaveToSession(ByVal strFunctionCode As String, ByVal udtSmartIDContent As BLL.SmartIDContentModel)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SmartIDContent)) = udtSmartIDContent
        End Sub

        Public Function SmartIDContentGetFormSession(ByVal strFunctionCode As String) As BLL.SmartIDContentModel
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SmartIDContent)), BLL.SmartIDContentModel)
        End Function

        Public Sub SmartIDContentRemoveFormSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SmartIDContent))
        End Sub

        'SmartID Ideas Combo Client Installation Result
        Public Sub IDEASComboClientSaveToSession(ByVal strResult As String)
            HttpContext.Current.Session(String.Format("{0}", SessionName.SESS_IDEASComboClient)) = strResult
        End Sub

        Public Function IDEASComboClientGetFormSession() As String
            Return CType(HttpContext.Current.Session(String.Format("{0}", SessionName.SESS_IDEASComboClient)), String)
        End Function

        Public Sub IDEASComboClientRemoveFormSession()
            HttpContext.Current.Session.Remove(String.Format("{0}", SessionName.SESS_IDEASComboClient))
        End Sub

        'SmartID Ideas Combo Version
        Public Sub IDEASComboVersionSaveToSession(ByVal strVersion As String)
            HttpContext.Current.Session(String.Format("{0}", SessionName.SESS_IDEASComboVersion)) = strVersion
        End Sub

        Public Function IDEASComboVersionGetFormSession() As String
            Return CType(HttpContext.Current.Session(String.Format("{0}", SessionName.SESS_IDEASComboVersion)), String)
        End Function

        Public Sub IDEASComboVersionRemoveFormSession()
            HttpContext.Current.Session.Remove(String.Format("{0}", SessionName.SESS_IDEASComboVersion))
        End Sub

        'SmartID Ideas Combo Version
        Public Sub ForceToReadSmartICSaveToSession(ByVal blnForce As Boolean)
            HttpContext.Current.Session(String.Format("{0}", SessionName.SESS_ForceToReadSmartIC)) = blnForce
        End Sub

        Public Function ForceToReadSmartICGetFormSession() As Nullable(Of Boolean)
            If HttpContext.Current.Session(String.Format("{0}", SessionName.SESS_ForceToReadSmartIC)) Is Nothing Then
                Return Nothing
            Else
                Return CType(HttpContext.Current.Session(String.Format("{0}", SessionName.SESS_ForceToReadSmartIC)), Boolean)
            End If
        End Function

        Public Sub ForceToReadSmartICRemoveFormSession()
            HttpContext.Current.Session.Remove(String.Format("{0}", SessionName.SESS_ForceToReadSmartIC))
        End Sub

#Region "NoticedDuplicateClaimAlert"
        ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [Start][Winnie]
        Public Sub NoticedDuplicateClaimAlertSaveToSession(ByVal NoticedDuplicateClaimAlert As String)
            HttpContext.Current.Session(SessionName.SESS_NoticedDuplicateClaimAlert) = NoticedDuplicateClaimAlert
        End Sub

        Public Function NoticedDuplicateClaimAlertGetFromSession() As String
            Dim strResult As String = String.Empty
            If HttpContext.Current.Session(SessionName.SESS_NoticedDuplicateClaimAlert) IsNot Nothing Then
                strResult = CType(HttpContext.Current.Session(SessionName.SESS_NoticedDuplicateClaimAlert), String)
            End If
            Return strResult
        End Function

        Public Sub NoticedDuplicateClaimAlertRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_NoticedDuplicateClaimAlert)
        End Sub
        ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [End][Winnie]
#End Region

        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
#Region "HA Patient (For SSSCMC Claim Only)"
        Public Sub HAPatientSaveToSession(ByVal dtHAPatient As DataTable)
            HttpContext.Current.Session(SessionName.SESS_HAPatient) = dtHAPatient
        End Sub

        Public Function HAPatientGetFromSession() As DataTable
            If HttpContext.Current.Session(SessionName.SESS_HAPatient) Is Nothing Then
                Return Nothing
            Else
                Return CType(HttpContext.Current.Session(SessionName.SESS_HAPatient), DataTable)
            End If
        End Function

        Public Sub HAPatientRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_HAPatient)
        End Sub
#End Region
        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
#Region "Claim COVID-19"

#Region "Claim COVID-19 Selected"
        Public Sub ClaimCOVID19SaveToSession(ByVal blnClaim As Boolean)
            HttpContext.Current.Session(SessionName.SESS_ClaimCOVID19) = blnClaim
        End Sub

        Public Function ClaimCOVID19GetFromSession() As Boolean
            If HttpContext.Current.Session(SessionName.SESS_ClaimCOVID19) Is Nothing Then
                Return Nothing
            Else
                Return CType(HttpContext.Current.Session(SessionName.SESS_ClaimCOVID19), Boolean)
            End If
        End Function

        Public Sub ClaimCOVID19RemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_ClaimCOVID19)
        End Sub
#End Region


#Region "Funct Code Setting"
        'CRE20-0xx Immu record Covid-19 set funct code [Start][Nichole]
        Public Sub ClaimFunctCodeSaveToSession(ByVal strFunctCode As String)
            HttpContext.Current.Session(SessionName.SESS_ClaimFunctCode) = strFunctCode
        End Sub

        Public Function ClaimFunctCodeGetFromSession() As String
            If HttpContext.Current.Session(SessionName.SESS_ClaimFunctCode) Is Nothing Then
                Return Common.Component.FunctCode.FUNT020201
            Else
                Return CType(HttpContext.Current.Session(SessionName.SESS_ClaimFunctCode), String)
            End If
        End Function

        Public Sub ClaimFunctCodeRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_ClaimFunctCode)
        End Sub
        'CRE20-0xx Immu record Covid-19 set funct code [End][Nichole]
#End Region

#Region "Claim COVID-19 - Booth"

        Public Sub ClaimCOVID19BoothSaveToSession(ByVal strBooth As String, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_Booth)) = strBooth
        End Sub

        Public Function ClaimCOVID19BoothGetFromSession(ByVal strFunctionCode As String) As String
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_Booth)), String)
        End Function

        Public Sub ClaimCOVID19BoothRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_Booth))
        End Sub

#End Region

#Region "Claim COVID-19 - Category"

        Public Sub ClaimCOVID19CategorySaveToSession(ByVal strCategory As String, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_Category)) = strCategory
        End Sub

        Public Function ClaimCOVID19CategoryGetFromSession(ByVal strFunctionCode As String) As String
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_Category)), String)
        End Function

        Public Sub ClaimCOVID19CategoryRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_Category))
        End Sub

#End Region

#Region "Claim COVID-19 - Main Category"

        Public Sub ClaimCOVID19MainCategorySaveToSession(ByVal strMainCategory As String, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_MainCategory)) = strMainCategory
        End Sub

        Public Function ClaimCOVID19MainCategoryGetFromSession(ByVal strFunctionCode As String) As String
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_MainCategory)), String)
        End Function

        Public Sub ClaimCOVID19MainCategoryRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_MainCategory))
        End Sub

#End Region

#Region "Claim COVID-19 - Sub Category"

        Public Sub ClaimCOVID19SubCategorySaveToSession(ByVal strSubCategory As String, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_SubCategory)) = strSubCategory
        End Sub

        Public Function ClaimCOVID19SubCategoryGetFromSession(ByVal strFunctionCode As String) As String
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_SubCategory)), String)
        End Function

        Public Sub ClaimCOVID19SubCategoryRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_SubCategory))
        End Sub

#End Region

#Region "Claim COVID-19 - Vaccine Brand"

        Public Sub ClaimCOVID19VaccineBrandSaveToSession(ByVal strVaccineBrand As String, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_VaccineBrand)) = strVaccineBrand
        End Sub

        Public Function ClaimCOVID19VaccineBrandGetFromSession(ByVal strFunctionCode As String) As String
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_VaccineBrand)), String)
        End Function

        Public Sub ClaimCOVID19VaccineBrandRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_VaccineBrand))
        End Sub

#End Region

#Region "Claim COVID-19 - Vaccine Lot No."

        Public Sub ClaimCOVID19VaccineLotNoSaveToSession(ByVal strVaccineLotNo As String, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_VaccineLotNo)) = strVaccineLotNo
        End Sub

        Public Function ClaimCOVID19VaccineLotNoGetFromSession(ByVal strFunctionCode As String) As String
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_VaccineLotNo)), String)
        End Function

        Public Sub ClaimCOVID19VaccineLotNoRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_VaccineLotNo))
        End Sub

#End Region

#Region "Claim COVID-19 - Dose"

        Public Sub ClaimCOVID19DoseSaveToSession(ByVal strDose As String, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_Dose)) = strDose
        End Sub

        Public Function ClaimCOVID19DoseGetFromSession(ByVal strFunctionCode As String) As String
            Return HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_Dose))
        End Function

        Public Sub ClaimCOVID19DoseRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_Dose))
        End Sub

#End Region
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        ' CRE20-0022 (Immu record) [Start][Martin]
#Region "Claim COVID-19 - Vaccine Record."

        Public Sub ClaimCOVID19VaccineRecordSaveToSession(ByVal dtVaccineRecord As DataTable)
            HttpContext.Current.Session(SessionName.SESS_ClaimCOVID19_VaccineRecord) = dtVaccineRecord
        End Sub

        Public Function ClaimCOVID19VaccineRecordGetFromSession() As DataTable
            If HttpContext.Current.Session(SessionName.SESS_ClaimCOVID19_VaccineRecord) Is Nothing Then
                Return Nothing
            Else
                Return CType(HttpContext.Current.Session(SessionName.SESS_ClaimCOVID19_VaccineRecord), DataTable)
            End If
        End Function

        Public Sub ClaimCOVID19VaccineRecordRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_ClaimCOVID19_VaccineRecord)
        End Sub
#End Region
        ' CRE20-0022 (Immu record) [End][Martin]

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
#Region "Claim COVID-19 - Scheme Selected"
        Public Sub ClaimCOVID19SchemeSelectedSaveToSession(ByVal udtSchemeClaim As SchemeClaimModel, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_SchemeSelected)) = udtSchemeClaim
        End Sub

        Public Function ClaimCOVID19SchemeSelectedGetFromSession(ByVal strFunctionCode As String) As SchemeClaimModel
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_SchemeSelected)), SchemeClaimModel)
        End Function

        Public Sub ClaimCOVID19SchemeSelectedRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_SchemeSelected))
        End Sub

#End Region
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
#Region "Claim COVID-19 - Vaccination Card"
        Public Sub ClaimCOVID19VaccinationCardSaveToSession(ByVal udtEHSTransaction As TransactionDetailVaccineModel, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CLAIMCOVID19_VaccinationCard)) = udtEHSTransaction
        End Sub

        Public Function ClaimCOVID19VaccinationCardGetFromSession(ByVal strFunctionCode As String) As TransactionDetailVaccineModel
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CLAIMCOVID19_VaccinationCard)), TransactionDetailVaccineModel)
        End Function

        Public Sub ClaimCOVID19VaccinationCardRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CLAIMCOVID19_VaccinationCard))
        End Sub

#End Region
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
#Region "Claim COVID-19 - Carry Forword"
        Public Sub ClaimCOVID19CarryForwordSaveToSession(ByVal blnUsed As Boolean, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CLAIMCOVID19_CarryForward)) = blnUsed
        End Sub

        Public Function ClaimCOVID19CarryForwordGetFromSession(ByVal strFunctionCode As String) As Boolean
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CLAIMCOVID19_CarryForward)), Boolean)
        End Function

        Public Sub ClaimCOVID19CarryForwordRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CLAIMCOVID19_CarryForward))
        End Sub

#End Region
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        ' CRE20-0023 (Immu record) [Start][Chris YIM]
#Region "Claim COVID-19 - Discharge Record"

        Public Sub ClaimCOVID19DischargeRecordSaveToSession(ByVal udtDischargeResult As COVID19.DischargeResultModel, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_DischargeRecord)) = udtDischargeResult
        End Sub

        Public Function ClaimCOVID19DischargeRecordGetFromSession(ByVal strFunctionCode As String) As COVID19.DischargeResultModel
            If HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_DischargeRecord)) Is Nothing Then
                Return Nothing
            Else
                Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_DischargeRecord)), COVID19.DischargeResultModel)
            End If
        End Function

        Public Sub ClaimCOVID19DischargeRecordRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_DischargeRecord))
        End Sub
#End Region
        ' CRE20-0023 (Immu record) [End][Chris YIM]

        ' CRE20-0023 (Immu record) [Start][Chris YIM]
#Region "Claim COVID-19 - Discharge Reminder"

        Public Sub ClaimCOVID19DischargeReminderSaveToSession(ByVal blnShow As Boolean, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_DischargeReminder)) = blnShow
        End Sub

        Public Function ClaimCOVID19DischargeReminderGetFromSession(ByVal strFunctionCode As String) As Boolean
            If HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_DischargeReminder)) Is Nothing Then
                Return False
            Else
                Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_DischargeReminder)), Boolean)
            End If
        End Function

        Public Sub ClaimCOVID19DischargeReminderRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_DischargeReminder))
        End Sub
#End Region
        ' CRE20-0023 (Immu record) [End][Chris YIM]

        ' CRE20-0023 (Immu record) [Start][Chris YIM]
#Region "Claim COVID-19 - Discharge Reminder"

        Public Sub ClaimCOVID19DischargeDemographicReminderSaveToSession(ByVal blnShow As Boolean, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_DischargeDemographicReminder)) = blnShow
        End Sub

        Public Function ClaimCOVID19DischargeDemographicReminderGetFromSession(ByVal strFunctionCode As String) As Boolean
            If HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_DischargeDemographicReminder)) Is Nothing Then
                Return False
            Else
                Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_DischargeDemographicReminder)), Boolean)
            End If
        End Function

        Public Sub ClaimCOVID19DischargeDemographicReminderRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_DischargeDemographicReminder))
        End Sub
#End Region
        ' CRE20-0023 (Immu record) [End][Chris YIM]

        ' CRE20-0023 (Immu record) [Start][Chris YIM]
#Region "Claim COVID-19 - Discharge Override Reason"

        Public Sub ClaimCOVID19DischargeOverrideReasonSaveToSession(ByVal strReason As String, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_DischargeOverrideReason)) = strReason
        End Sub

        Public Function ClaimCOVID19DischargeOverrideReasonGetFromSession(ByVal strFunctionCode As String) As String
            If HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_DischargeOverrideReason)) Is Nothing Then
                Return String.Empty
            Else
                Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_DischargeOverrideReason)), String)
            End If
        End Function

        Public Sub ClaimCOVID19DischargeOverrideReasonRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_DischargeOverrideReason))
        End Sub
#End Region
        ' CRE20-0023 (Immu record) [End][Chris YIM]

#End Region


    End Class
End Namespace

