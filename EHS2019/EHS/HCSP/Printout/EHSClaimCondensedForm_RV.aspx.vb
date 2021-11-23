Imports Common.Component.Printout
Imports Common.Component.Scheme
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.ServiceProvider
Imports Common.Component.ClaimRules
Imports Common.Component.ClaimRules.ClaimRulesBLL
Imports Common.Component
' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
' -----------------------------------------------------------------------------------------
'Imports ConsentFormEHS
Imports HCSP.PrintOut.ConsentFormInformation
' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

Imports HCSP.PrintOut
Imports HCSP.PrintOut.Common

Partial Public Class EHSClaimCondensedForm_RV
    Inherits BasePrintoutForm

    Public Const FunctCode As String = Common.Component.FunctCode.FUNT020201

    Dim _udtSessionHandler As BLL.SessionHandler = New BLL.SessionHandler()
    Dim _udtPrintoutBLL As PrintoutBLL = New PrintoutBLL

    Dim _udtClaimRules As ClaimRulesBLL = New ClaimRulesBLL()
    Dim _udtPrintoutHelper As PrintoutHelper = New PrintoutHelper()

    Public Overrides ReadOnly Property Language() As String
        Get
            Return ConsentFormInformationModel.LanguageClassInternal.English
        End Get
    End Property

    Public Overrides ReadOnly Property FormStyle() As String
        Get
            Return ConsentFormInformationModel.FormStyleClass.Condensed
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        LoadReport()

    End Sub

    Overrides Function GetReport() As GrapeCity.ActiveReports.SectionReport
        Dim objReport As GrapeCity.ActiveReports.SectionReport = Nothing
        Dim strFunctCode As String = FunctCode
        Dim strSessionFunctCode As String = _udtSessionHandler.EHSClaimPrintoutFunctionCodeGetFromSession()
        If Not String.IsNullOrEmpty(strSessionFunctCode) Then
            strFunctCode = strSessionFunctCode
        End If

        ' Get required object from session
        Dim udtSchemeClaim As SchemeClaimModel = _udtSessionHandler.SchemeSelectedGetFromSession(strFunctCode)
        Dim udtEHSAccount As EHSAccountModel = _udtSessionHandler.EHSAccountGetFromSession(strFunctCode)
        Dim udtEHSTransaction As EHSTransactionModel = _udtSessionHandler.EHSTransactionGetFromSession(strFunctCode)
        Dim udtSmartIDContent As BLL.SmartIDContentModel = Me._udtSessionHandler.SmartIDContentGetFormSession(strFunctCode)
        Dim udtVaccinationRecord As COVID19.VaccinationCardRecordModel = _udtSessionHandler.ClaimCOVID19VaccinationCardGetFromSession(strFunctCode)
        Dim udtDischargeResult As COVID19.DischargeResultModel = _udtSessionHandler.ClaimCOVID19DischargeRecordGetFromSession(strFunctCode)
        Dim blnDischarge As Boolean = False
        Dim blnNonLocalRecoveredHistory1stDose As Boolean = False
        Dim blnNonLocalRecoveredHistory2ndDose As Boolean = False
        Dim udtSP As ServiceProviderModel = Nothing
        _udtSessionHandler.CurrentUserGetFromSession(udtSP, Nothing)

        If udtDischargeResult IsNot Nothing AndAlso _
            (udtDischargeResult.DemographicResult = COVID19.DischargeResultModel.Result.ExactMatch OrElse _
            udtDischargeResult.DemographicResult = COVID19.DischargeResultModel.Result.PartialMatch) Then
            blnDischarge = True
        End If

        
        ' Create the report instance
        Select Case udtSchemeClaim.SchemeCode
            Case SchemeClaimModel.CIVSS
                If Not udtSP Is Nothing AndAlso _
                   Not udtEHSTransaction Is Nothing AndAlso _
                   Not udtEHSAccount Is Nothing AndAlso _
                   Not udtSchemeClaim Is Nothing Then
                    objReport = New CIVSSConsentForm.CIVSSConsentCondensedForm(udtEHSTransaction, udtSchemeClaim, udtEHSAccount, udtSP, udtSmartIDContent)
                End If

            Case SchemeClaimModel.EVSS
                If Not udtSP Is Nothing AndAlso _
                   Not udtEHSTransaction Is Nothing AndAlso _
                   Not udtEHSAccount Is Nothing AndAlso _
                   Not udtSchemeClaim Is Nothing Then
                    objReport = New EVSSConsentForm.EVSSConsentCondensedForm(udtEHSTransaction, udtSchemeClaim, udtEHSAccount, udtSP, udtSmartIDContent)
                End If

                ' [CRP12-003] Fix the program so that the old consent form not to be shown in HCVS [Start][Tommy]

                'Handled By FGS : ConsentFormEHS.ConsentFormInformationBLL.GetReport(BulidConsentFormInformation(strFunctCode))
                'Case SchemeClaimModel.HCVS
                '    If Not udtSP Is Nothing AndAlso _
                '       Not udtEHSTransaction Is Nothing AndAlso _
                '       Not udtEHSAccount Is Nothing Then
                '        objReport = New PrintOut.VoucherConsentForm.VoucherConsentCondensedForm(udtEHSTransaction, udtSP, udtEHSAccount, udtSmartIDContent)
                '    End If

                ' [CRP12-003] Fix the program so that the old consent form not to be shown in HCVS [End][Tommy]

            Case SchemeClaimModel.HSIVSS
                If Not udtSP Is Nothing AndAlso _
                   Not udtEHSTransaction Is Nothing AndAlso _
                   Not udtEHSAccount Is Nothing AndAlso _
                   Not udtSchemeClaim Is Nothing Then

                    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    Dim strCategoryCode As String = udtEHSTransaction.CategoryCode
                    'CRE16-002 (Revamp VSS) [End][Chris YIM]
                    Dim udtSchemeCategoryDescriptionMapping As PrintoutBLL.GetSchemeCategoryDescriptionResourceMappingResult = _udtPrintoutBLL.GetSchemeCategoryDescriptionResourceMapping(udtEHSTransaction.TransactionDetails(0).SchemeCode, udtEHSTransaction.TransactionDetails(0).SchemeSeq, strCategoryCode, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode), udtEHSTransaction.ServiceDate)

                    If udtSchemeCategoryDescriptionMapping.IsAdult Then
                        objReport = New HSIVSSConsentForm.HSIVSSConsentCondensedFormAdult(udtEHSTransaction, udtSchemeClaim, udtEHSAccount, udtSP, udtSchemeCategoryDescriptionMapping.SystemResource, udtSmartIDContent)
                    Else
                        objReport = New HSIVSSConsentForm.HSIVSSConsentCondensedFormChild(udtEHSTransaction, udtSchemeClaim, udtEHSAccount, udtSP, udtSchemeCategoryDescriptionMapping.SystemResource, udtSmartIDContent)
                    End If

                End If

            Case SchemeClaimModel.RVP
                ' ToDo: RVP Printout

            Case SchemeClaimModel.VSS
                If udtEHSTransaction.TransactionDetails.FilterBySubsidizeItemDetail(SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19).Count > 0 Then
                    If Not IsNothing(udtSP) _
                            AndAlso Not IsNothing(udtEHSTransaction) _
                            AndAlso Not IsNothing(udtEHSAccount) _
                            AndAlso Not IsNothing(udtSchemeClaim) Then

                        objReport = New COVID19.PrintOut.Covid19VaccinationCard.Covid19VaccinationCard(udtEHSTransaction, udtEHSAccount, udtVaccinationRecord, _
                                                                                                       blnDischarge)
                    End If

                Else
                    If Not IsNothing(udtSP) _
                            AndAlso Not IsNothing(udtEHSTransaction) _
                            AndAlso Not IsNothing(udtEHSAccount) _
                            AndAlso Not IsNothing(udtSchemeClaim) Then

                        If udtEHSTransaction.CategoryCode = CategoryCode.VSS_CHILD Then
                            objReport = New VSSConsentForm.VSSConsentCondensedForm_C(udtEHSTransaction, udtSchemeClaim, udtEHSAccount, udtSP, udtSmartIDContent)
                        Else
                            objReport = New VSSConsentForm.VSSConsentCondensedForm(udtEHSTransaction, udtSchemeClaim, udtEHSAccount, udtSP, udtSmartIDContent)
                        End If

                    End If

                End If

                ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
                ' --------------------------------------------------------------------------------------
            Case SchemeClaimModel.ENHVSSO
                If Not IsNothing(udtSP) _
                        AndAlso Not IsNothing(udtEHSTransaction) _
                        AndAlso Not IsNothing(udtEHSAccount) _
                        AndAlso Not IsNothing(udtSchemeClaim) Then

                    'If udtEHSTransaction.CategoryCode = CategoryCode.EVSSO_CHILD Then
                    '    objReport = New VSSConsentForm.VSSConsentForm_C(udtEHSTransaction, udtSchemeClaim, udtEHSAccount, udtSP, udtSmartIDContent)
                    'Else
                    '    objReport = New VSSConsentForm.VSSConsentForm(udtEHSTransaction, udtSchemeClaim, udtEHSAccount, udtSP, udtSmartIDContent)
                    'End If

                    objReport = New VSSConsentForm.VSSConsentCondensedForm_C(udtEHSTransaction, udtSchemeClaim, udtEHSAccount, udtSP, udtSmartIDContent)

                End If
                ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

                'CRE20-022 (Immu record)  [Start][Raiman] 
            Case SchemeClaimModel.COVID19CVC, SchemeClaimModel.COVID19RVP, _
                SchemeClaimModel.COVID19DH, SchemeClaimModel.COVID19OR, SchemeClaimModel.COVID19SR, SchemeClaimModel.COVID19SB
                If Not IsNothing(udtSP) _
                        AndAlso Not IsNothing(udtEHSTransaction) _
                        AndAlso Not IsNothing(udtEHSAccount) _
                        AndAlso Not IsNothing(udtSchemeClaim) Then

                    objReport = New COVID19.PrintOut.Covid19VaccinationCard.Covid19VaccinationCard(udtEHSTransaction, udtEHSAccount, udtVaccinationRecord, _
                                                                                                   blnDischarge)
                End If
                'CRE20-022 (Immu record)  [End][Raiman] 

            Case SchemeClaimModel.RVP
                If udtEHSTransaction.TransactionDetails.FilterBySubsidizeItemDetail(SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19).Count > 0 Then
                    If Not IsNothing(udtSP) _
                            AndAlso Not IsNothing(udtEHSTransaction) _
                            AndAlso Not IsNothing(udtEHSAccount) _
                            AndAlso Not IsNothing(udtSchemeClaim) Then
                        objReport = New COVID19.PrintOut.Covid19VaccinationCard.Covid19VaccinationCard(udtEHSTransaction, udtEHSAccount, udtVaccinationRecord, _
                                                                                                       blnDischarge)

                    End If
                End If

        End Select

        Return objReport
    End Function

End Class