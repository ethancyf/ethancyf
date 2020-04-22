Imports Common.Component.EHSTransaction
Imports Common.Component.EHSClaimVaccine
Imports Common.Component.Scheme
Imports Common.Component.Practice

Namespace BLL
    Public Class SessionHandlerBLL


        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Public Class SessionName
            'Language
            Public Const SESS_LANGUAGE As String = "language"

            'EHSTransaction
            Public Const SESS_EHSTransaction As String = "SESS_EHSTRANSACTION"
            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Public Const SESS_EHSTransaction_Without_Transaction_Detail As String = "SESS_EHSTRANSACTION_WITHOUT_TRANSACTION_DETAIL"
            ' CRE17-010 (OCSSS integration) [End][Chris YIM]
            Public Const SESS_EHSClaimVaccine As String = "SESS_EHSCLAIMVACCINE"

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

        End Class
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

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
        'CRE16-026 (Add PCV13) [End][Chris YIM]


        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
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
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]


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


    End Class
End Namespace


