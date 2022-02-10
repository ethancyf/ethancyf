Imports Common.Component.EHSAccount
Imports Common.Component.EHSClaimVaccine
Imports Common.Component.EHSTransaction
Imports Common.Component.Practice
Imports Common.Component.Scheme
Imports Common.Component.COVID19


Namespace BLL
    Public Class SessionHandlerBLL

        Public Class SessionName
            'Language
            Public Const SESS_LANGUAGE As String = "language"

            'EHSTransaction
            Public Const SESS_EHSTransaction As String = "SESS_EHSTRANSACTION"
            Public Const SESS_EHSTransaction_Without_Transaction_Detail As String = "SESS_EHSTRANSACTION_WITHOUT_TRANSACTION_DETAIL"
            Public Const SESS_EHSClaimVaccine As String = "SESS_EHSCLAIMVACCINE"

            'EHSTransaction Reprint
            Public Const SESS_EHSClaimPrintOutFunctionCode As String = "SESS_EHSCLAIM_PRINTOUT_FUNCTIONCODE"
            Public Const SESS_CLAIMCOVID19_VaccinationCard As String = "SESS_CLAIMCOVID19_VACCINATIONCARD"
            Public Const SESS_CLAIMCOVID19_ValidReprint As String = "SESS_CLAIMCOVID19_VALIDREPRINT"
            Public Const SESS_ClaimCOVID19_DischargeRecord As String = "SESS_CLAIMCOVID19_DISCHARGERECORD"
            Public Const SESS_ClaimCOVID19_MedicalExemptionRecord As String = "SESS_CLAIMCOVID19_MEDICALEXEMPTIONRECORD"
            Public Const SESS_ClaimCOVID19_MedicalExemptionRecordFull As String = "SESS_CLAIMCOVID19_MEDICALEXEMPTIONRECORDFULL"

            'Scheme
            Public Const SESS_SchemeClaim As String = "SESS_SCHEMECLAIM"
            Public Const SESS_SelectedScheme As String = "SESS_SELECTEDSCHEMECLAIM"

            'Practice
            Public Const SESS_PracticeBankListForClaim As String = "SESS_PRACTICEBANKLISTFORCLAIM"
            Public Const SESS_PracticeBankForClaim As String = "SESS_PRACTICEBANKFORCLAIM"

            'Category
            Public Const SESS_ClaimCategory As String = "SESS_CLAIMCATEGORY"

            'RVP
            Public Const SESS_RVPRCHCode As String = "SESS_RVHRCHCode"

            'Vaccination Record
            Public Const SESS_CMSVaccineResult As String = "SESS_CMSVaccineResult"
            Public Const SESS_CIMSVaccineResult As String = "SESS_CIMSVaccineResult"

            'VSS
            Public Const SESS_NonClinicSetting As String = "SESS_NONCLINICSETTING"
            Public Const SESS_ChangeSchemeInPractice As String = "SESS_CHANGESCHEMEINPRACTICE"
            Public Const SESS_DocumentaryProofForPID As String = "SESS_DOCUMENTARYPROOFFORPID"
            Public Const SESS_PIDInstitutionCode As String = "SESS_PIDINSTITUTIONCODE"
            Public Const SESS_PlaceOfVaccination As String = "SESS_PLACEOFVACCINATION"
            Public Const SESS_PlaceOfVaccinationOther As String = "SESS_PLACEOFVACCINATIONOTHER"
            Public Const SESS_HighRisk As String = "SESS_HIGHRISK"

            'PPP
            Public Const SESS_SchoolCode As String = "SESS_SCHOOLCODE"

            ' CRE20-003 (Batch Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            ' EHSAccount
            Public Const SESS_EHSAccount As String = "SESS_EHSACCOUNT"
            ' CRE20-003 (Batch Upload) [End][Chris YIM]

            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Public Const SESS_ClaimCOVID19 As String = "SESS_CLAIMCOVID19"
            Public Const SESS_ClaimCOVID19_Booth As String = "SESS_CLAIMCOVID19_Booth"
            Public Const SESS_ClaimCOVID19_Category As String = "SESS_CLAIMCOVID19_Category"
            Public Const SESS_ClaimCOVID19_VaccineLotNo As String = "SESS_CLAIMCOVID19_VaccineLotNo"
            ' CRE20-0022 (Immu record) [End][Chris YIM]

            ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Public Const SESS_HAPatient As String = "SESS_HA_PATIENT"
            Public Const SESS_NewClaim As String = "SESS_NEW_CLAIM"
            ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            '------------------------------------------------------------------------------------------
            'SmartIC Content Session
            '------------------------------------------------------------------------------------------
            Public Const SESS_SmartIDContent As String = "SESS_SMARTIDCONTENT"
            Public Const SESS_IDEASComboClient As String = "SESS_IDEASComboClient"
            Public Const SESS_IDEASComboVersion As String = "SESS_IDEASComboVersion"
            ' CRE20-0022 (Immu record) [End][Chris YIM]

        End Class

#Region "Claim Transaction"
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

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Sub EHSTransactionWithoutTransactionDetailSaveToSession(ByVal udtEHSTransaction As EHSTransactionModel, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSTransaction_Without_Transaction_Detail)) = udtEHSTransaction
        End Sub

        Public Function EHSTransactionWithoutTransactionDetailGetFromSession(ByVal strFunctionCode As String) As EHSTransactionModel
            If HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSTransaction_Without_Transaction_Detail)) Is Nothing Then
                Return Nothing
            Else
                Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSTransaction_Without_Transaction_Detail)), EHSTransactionModel)
            End If
        End Function

        Public Sub EHSTransactionWithoutTransactionDetailRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSTransaction_Without_Transaction_Detail))
        End Sub
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]
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

#Region "Claim Vaccination Transaction"
        Public Sub EHSClaimVaccineSaveToSession(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSClaimVaccine)) = udtEHSClaimVaccine
        End Sub

        Public Function EHSClaimVaccineGetFromSession(ByVal strFunctionCode As String) As EHSClaimVaccineModel

            If HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSClaimVaccine)) Is Nothing Then
                Return Nothing
            Else
                Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSClaimVaccine)), EHSClaimVaccineModel)
            End If
        End Function

        Public Sub EHSClaimVaccineRemoveFromSession(ByVal strFunctionCode As String)

            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSClaimVaccine))
        End Sub
#End Region

#Region "Language"

        Public Function Language()
            Return CType(HttpContext.Current.Session(SessionName.SESS_LANGUAGE), String)
        End Function

#End Region

#Region "Scheme Collection with Subsidize"

        Public Sub SchemeListSaveToSession(ByVal udtSchemeClaim As SchemeClaimModelCollection, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SchemeClaim)) = udtSchemeClaim
        End Sub

        Public Function SchemeListGetFromSession(ByVal strFunctionCode As String) As SchemeClaimModelCollection
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SchemeClaim)), SchemeClaimModelCollection)
        End Function

        Public Sub SchemeListRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SchemeClaim))
        End Sub

        Public Sub SelectSchemeSaveToSession(ByVal udtSchemeClaim As SchemeClaimModel, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SelectedScheme)) = udtSchemeClaim
        End Sub

        Public Function SelectSchemeGetFromSession(ByVal strFunctionCode As String) As SchemeClaimModel
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SelectedScheme)), SchemeClaimModel)
        End Function

        Public Sub SelectSchemeRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SelectedScheme))
        End Sub

#End Region

#Region "Practice Bank for Create Claim"
        Public Sub PracticeDisplayListSaveToSession(ByVal udtPracticeDisplayList As PracticeBLL.PracticeDisplayModelCollection, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_PracticeBankListForClaim)) = udtPracticeDisplayList
        End Sub

        Public Function PracticeDisplayListGetFromSession(ByVal strFunctionCode As String) As PracticeBLL.PracticeDisplayModelCollection
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_PracticeBankListForClaim)), PracticeBLL.PracticeDisplayModelCollection)
        End Function

        Public Sub PracticeDisplayListRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_PracticeBankListForClaim))
        End Sub

        Public Sub PracticeDisplaySaveToSession(ByVal udtPracticeDisplayList As PracticeBLL.PracticeDisplayModel, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_PracticeBankForClaim)) = udtPracticeDisplayList
        End Sub

        Public Function PracticeDisplayGetFromSession(ByVal strFunctionCode As String) As PracticeBLL.PracticeDisplayModel
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_PracticeBankForClaim)), PracticeBLL.PracticeDisplayModel)
        End Function

        Public Sub PracticeDisplayRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_PracticeBankForClaim))
        End Sub
#End Region

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

        Public Sub ChangeSchemeInPracticeSaveToSession(ByVal blnChangeSchemeInPractice As String, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ChangeSchemeInPractice)) = blnChangeSchemeInPractice
        End Sub

        Public Function ChangeSchemeInPracticeGetFromSession(ByVal strFunctionCode As String) As Boolean
            Return HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ChangeSchemeInPractice))
        End Function

        Public Sub ChangeSchemeInPracticeRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ChangeSchemeInPractice))
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

        Public Sub PlaceVaccinationSaveToSession(ByVal strPlaceVaccination As String, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_PlaceOfVaccination)) = strPlaceVaccination
        End Sub

        Public Function PlaceVaccinationGetFromSession(ByVal strFunctionCode As String) As String
            Return HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_PlaceOfVaccination))
        End Function

        Public Sub PlaceVaccinationRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_PlaceOfVaccination))
        End Sub

#End Region

#Region "Place of Vaccination Other"

        Public Sub PlaceVaccinationOtherSaveToSession(ByVal strPlaceVaccinationOther As String, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_PlaceOfVaccinationOther)) = strPlaceVaccinationOther
        End Sub

        Public Function PlaceVaccinationOtherGetFromSession(ByVal strFunctionCode As String) As String
            Return HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_PlaceOfVaccinationOther))
        End Function

        Public Sub PlaceVaccinationOtherRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_PlaceOfVaccinationOther))
        End Sub

#End Region

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

#Region "School Code"
        Public Sub SchoolCodeSaveToSession(ByVal strSchoolCode As String, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SchoolCode)) = strSchoolCode
        End Sub

        Public Function SchoolCodeGetFromSession(ByVal strFunctionCode As String) As String
            Return HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SchoolCode))
        End Function

        Public Sub SchoolCodeRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SchoolCode))
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

#Region "RCH Code"
        Public Sub RVPRCHCodeSaveToSession(ByVal strFunctionCode As String, ByVal strRCHCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_RVPRCHCode)) = strRCHCode.Trim()
        End Sub

        Public Function RVPRCHCodeGetFromSession(ByVal strFunctionCode As String) As String
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_RVPRCHCode)), String)
        End Function

        Public Sub RVPRCHCodeRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_RVPRCHCode))
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
        'CRE18-004 [Start][Marco CHOI]
        Public Sub CIMSVaccineResultSaveToSession(ByVal udtVaccineResult As Common.WebService.Interface.DHVaccineResult, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CIMSVaccineResult)) = udtVaccineResult
        End Sub

        Public Function CIMSVaccineResultGetFromSession(ByVal strFunctionCode As String) As Common.WebService.Interface.DHVaccineResult
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CIMSVaccineResult)), Common.WebService.Interface.DHVaccineResult)
        End Function

        Public Sub CIMSVaccineResultRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CIMSVaccineResult))
        End Sub
        'CRE18-004 [End]  [Marco CHOI]
#End Region

        ' CRE20-003 (Batch Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
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
#End Region
        ' CRE20-003 (Batch Upload) [End][Chris YIM]

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

        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
#Region "Make New Claim Transaction"
        Public Sub NewClaimTransactionSaveToSession(ByVal blnNewClaim As Boolean)
            HttpContext.Current.Session(SessionName.SESS_NewClaim) = blnNewClaim
        End Sub

        Public Function NewClaimTransactionGetFromSession() As Boolean
            If HttpContext.Current.Session(SessionName.SESS_NewClaim) Is Nothing Then
                Return False
            Else
                Return HttpContext.Current.Session(SessionName.SESS_NewClaim)
            End If
        End Function

        Public Sub NewClaimTransactionRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_NewClaim)
        End Sub
#End Region
        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]


#Region "COVID-19"

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

        ' CRE20-0022 (Immu record) [Start][Raiman]
        ' ---------------------------------------------------------------------------------------------------------
#Region "COVID-19 - Vaccination Card"
        Public Sub ClaimCOVID19VaccinationCardSaveToSession(ByVal udtVaccinationCardRecord As VaccinationCardRecordModel, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CLAIMCOVID19_VaccinationCard)) = udtVaccinationCardRecord
        End Sub

        Public Function ClaimCOVID19VaccinationCardGetFromSession(ByVal strFunctionCode As String) As VaccinationCardRecordModel
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CLAIMCOVID19_VaccinationCard)), VaccinationCardRecordModel)
        End Function

        Public Sub ClaimCOVID19VaccinationCardRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CLAIMCOVID19_VaccinationCard))
        End Sub

#End Region
        ' CRE20-0022 (Immu record) [End][Raiman]

#Region "COVID-19 - Valid Reprint Vaccination Card"
        Public Sub ClaimCOVID19ValidReprintSaveToSession(ByVal blnValid As Boolean, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CLAIMCOVID19_ValidReprint)) = blnValid
        End Sub

        Public Function ClaimCOVID19ValidReprintGetFromSession(ByVal strFunctionCode As String) As Boolean
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CLAIMCOVID19_ValidReprint)), Boolean)
        End Function

        Public Sub ClaimCOVID19ValidReprintRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CLAIMCOVID19_ValidReprint))
        End Sub

#End Region

        ' CRE20-0023 (Immu record) [Start][Chris YIM]
#Region "Claim COVID-19 - Discharge Record"

        Public Sub ClaimCOVID19DischargeRecordSaveToSession(ByVal udtDischargeResult As DischargeResultModel, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_DischargeRecord)) = udtDischargeResult
        End Sub

        Public Function ClaimCOVID19DischargeRecordGetFromSession(ByVal strFunctionCode As String) As DischargeResultModel
            If HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_DischargeRecord)) Is Nothing Then
                Return Nothing
            Else
                Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_DischargeRecord)), DischargeResultModel)
            End If
        End Function

        Public Sub ClaimCOVID19DischargeRecordRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_ClaimCOVID19_DischargeRecord))
        End Sub
#End Region
        ' CRE20-0023 (Immu record) [End][Chris YIM]

        ' CRE20-0023-71 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
#Region "Claim COVID-19 - Medical Exemption Record"
        Public Sub MedicalExemptionRecordFullSaveToSession(ByVal dt As DataTable)
            HttpContext.Current.Session(SessionName.SESS_ClaimCOVID19_MedicalExemptionRecordFull) = dt
        End Sub

        Public Function MedicalExemptionRecordFullGetFromSession() As DataTable
            If HttpContext.Current.Session(SessionName.SESS_ClaimCOVID19_MedicalExemptionRecordFull) Is Nothing Then
                Return Nothing
            Else
                Return CType(HttpContext.Current.Session(SessionName.SESS_ClaimCOVID19_MedicalExemptionRecordFull), DataTable)
            End If
        End Function

        Public Sub MedicalExemptionRecordFullRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_ClaimCOVID19_MedicalExemptionRecordFull)
        End Sub

        Public Sub MedicalExemptionRecordSaveToSession(ByVal dt As DataTable)
            HttpContext.Current.Session(SessionName.SESS_ClaimCOVID19_MedicalExemptionRecord) = dt
        End Sub

        Public Function MedicalExemptionRecordGetFromSession() As DataTable
            If HttpContext.Current.Session(SessionName.SESS_ClaimCOVID19_MedicalExemptionRecord) Is Nothing Then
                Return Nothing
            Else
                Return CType(HttpContext.Current.Session(SessionName.SESS_ClaimCOVID19_MedicalExemptionRecord), DataTable)
            End If
        End Function

        Public Sub MedicalExemptionRecordRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_ClaimCOVID19_MedicalExemptionRecord)
        End Sub
#End Region
        ' CRE20-0023-71 (Immu record) [End][Chris YIM]

#End Region

#Region "SmartID"
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
#End Region

    End Class
End Namespace


