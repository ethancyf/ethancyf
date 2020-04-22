Imports System.Data.SqlClient
Imports Common.DataAccess
Imports Common.Format
Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.Scheme
Imports Common.Component.SchemeDetails
Imports Common.Component.DocType
Imports Common.Component.ClaimRules
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.EHSTransaction
Imports Common.Component.ServiceProvider
Imports Common.Component.DataEntryUser
Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.WebService.Interface

Namespace BLL

    Public Class EHSClaimBLL

#Region "Private Member"
        Private _udtDocTypeBLL As New DocTypeBLL()
        Private _udtSchemeClaimBLL As New SchemeClaimBLL()
        Private _udtSchemeDetailBLL As New SchemeDetailBLL()
        Private _udtEHSAccountBLL As New EHSAccountBLL()
        Private _udtEHSTransactionBLL As New EHSTransactionBLL()
        Private _udtClaimRulesBLL As New ClaimRulesBLL()
        Private _udtFormater As New Formatter()
        Private _udtCommonGenFunc As New GeneralFunction()
#End Region

#Region "EHSVaccine FOR EHSClaim UI"

        ''' <summary>
        ''' Retrieve the EHSClaimVaccine Model for EHSClaim Screen Enter Claim Detail
        ''' DynamicControl:
        ''' ---> True
        ''' ---------> Vaccine Control is generated dynamically base on the changed service date
        ''' ---------> The vaccine will be base on the specific pass in service date only
        ''' -- False 
        ''' ---------> Vaccine Control is generated once base on the first input service date
        ''' ---------> The vaccine will be base on all possible value of the service date (passed in Claim + Subsidize Model)
        ''' </summary>
        ''' <param name="udtSchemeClaimModel"></param>
        ''' <param name="strDocCode"></param>
        ''' <param name="udtEHSAccountModel"></param>
        ''' <param name="dtmServiceDate"></param>
        ''' <param name="blnDynamicControl"></param>
        ''' <param name="strCategoryCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SearchEHSClaimVaccine(ByVal udtSchemeClaimModel As SchemeClaimModel, ByVal strDocCode As String, ByVal udtEHSAccountModel As EHSAccountModel, _
                                                    ByVal dtmServiceDate As Date, ByVal blnDynamicControl As Boolean, _
                                                    ByVal udtAllTransactionVaccineBenefit As TransactionDetailVaccineModelCollection, Optional ByVal strCategoryCode As String = "") As EHSClaimVaccineModel

            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]

            'If dtmServiceDate.Date < udtSchemeClaimModel.ClaimPeriodFrom.Date Then
            Dim udtSchemeClaimBLL As New SchemeClaimBLL()
            Dim udtSchemeClaimModelPrevious As SchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtSchemeClaimModel.SchemeCode, dtmServiceDate.Date.AddDays(1).AddMinutes(-1))
            If Not udtSchemeClaimModelPrevious Is Nothing Then
                udtSchemeClaimModel = udtSchemeClaimModelPrevious
            End If
            'End If

            'Dim udtEHSClaimVaccineModel As New EHSClaimVaccineModel(udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SchemeSeq)
            Dim udtEHSClaimVaccineModel As New EHSClaimVaccineModel(udtSchemeClaimModel.SchemeCode)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

            ' Auto Selected If only 1 Entry found
            Dim intEntryCount As Integer = 0


            ' Vaccine->Subsidize

            ' By the selected service date, determine whether the subsidize is available for the patient
            For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtSchemeClaimModel.SubsidizeGroupClaimList
                Dim blnNoVaccineAvailable As Boolean = False


                If blnDynamicControl Then
                    ' Out of Service Period, Or Not Eligible
                    If udtSubsidizeGroupClaim.LastServiceDtm < dtmServiceDate OrElse Not Me._udtClaimRulesBLL.CheckEligibilityPerSubsidize(udtSubsidizeGroupClaim, udtEHSAccountModel.getPersonalInformation(strDocCode).DOB, udtEHSAccountModel.getPersonalInformation(strDocCode).ExactDOB, dtmServiceDate, udtAllTransactionVaccineBenefit).IsEligible Then
                        Continue For
                    End If
                End If

                If strCategoryCode.Trim() <> "" Then
                    ' If Category is passed, Check Exist of Category
                    Dim udtClaimCategoryBLL As New ClaimCategory.ClaimCategoryBLL()
                    If udtClaimCategoryBLL.getAllCategoryCache().Filter(udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, strCategoryCode) Is Nothing Then
                        Continue For
                    Else
                        ' Category Exist, Check for Category Eligibility for the Subsidize
                        Dim udtEligibilityRuleResult As ClaimRulesBLL.EligibleResult = Me._udtClaimRulesBLL.CheckCategoryEligibilityByCategory(udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, strCategoryCode, udtEHSAccountModel.getPersonalInformation(strDocCode).DOB, udtEHSAccountModel.getPersonalInformation(strDocCode).ExactDOB, dtmServiceDate)
                        If Not udtEligibilityRuleResult.IsEligible Then
                            Continue For
                        End If
                    End If
                End If

                ' Check available benefit
                Dim arrCheckDateList As New List(Of Date)
                arrCheckDateList.Add(dtmServiceDate)
                blnNoVaccineAvailable = Not Me._udtClaimRulesBLL.chkVaccineAvailableBenefitBySubsidize(udtSubsidizeGroupClaim, arrCheckDateList, strDocCode, udtEHSAccountModel.EHSPersonalInformationList(0).IdentityNum, _
                                                                                udtSchemeClaimModel, udtAllTransactionVaccineBenefit, _
                                                                                udtEHSAccountModel.EHSPersonalInformationList(0).DOB, _
                                                                                udtEHSAccountModel.EHSPersonalInformationList(0).ExactDOB, Now, dtmServiceDate)

                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                ' Vaccine Fee
                'Dim udtSchemeVaccineDetailModel As SchemeVaccineDetailModel = Me._udtSchemeDetailBLL.getSchemeVaccineDetail(udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode)
                Dim udtSubsidizeFeeVaccineModel As SubsidizeFeeModel = udtSubsidizeGroupClaim.SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVaccine, dtmServiceDate)
                Dim udtSubsidizeFeeInjectionModel As SubsidizeFeeModel = udtSubsidizeGroupClaim.SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeInjection, dtmServiceDate)

                ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
                ' -----------------------------------------------------------------------------------------
                ' CRE13-001 - EHAPP [Start][Tommy L]
                ' -------------------------------------------------------------------------------------
                'Dim udtEHSClaimSubsidizeModel As New EHSClaimVaccineModel.EHSClaimSubsidizeModel( _
                '   udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, _
                '    udtSubsidizeGroupClaim.DisplaySeq, udtSubsidizeGroupClaim.SubsidizeItemCode, _
                '    IIf(UploadClaimBLL.UseParticularSubsidyCode, udtSubsidizeGroupClaim.DisplayCodeForClaim, udtSubsidizeGroupClaim.SubsidizeDisplayCode), _
                '    udtSubsidizeGroupClaim.SubsidizeItemDesc, udtSubsidizeGroupClaim.SubsidizeItemDescChi, _
                '    udtSubsidizeFeeVaccineModel.SubsidizeFee + udtSubsidizeFeeInjectionModel.SubsidizeFee, udtSubsidizeFeeVaccineModel.SubsidizeFee, udtSubsidizeFeeVaccineModel.SubsidizeFeeVisible, _
                '    udtSubsidizeFeeInjectionModel.SubsidizeFee, udtSubsidizeFeeInjectionModel.SubsidizeFeeVisible)
                Dim udtEHSClaimSubsidizeModel As New EHSClaimVaccineModel.EHSClaimSubsidizeModel( _
                    udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, _
                    udtSubsidizeGroupClaim.DisplaySeq, udtSubsidizeGroupClaim.SubsidizeItemCode, _
                    IIf(UploadClaimBLL.UseParticularSubsidyCode, udtSubsidizeGroupClaim.DisplayCodeForClaim, udtSubsidizeGroupClaim.SubsidizeDisplayCode), _
                    udtSubsidizeGroupClaim.SubsidizeItemDesc, udtSubsidizeGroupClaim.SubsidizeItemDescChi, _
                    udtSubsidizeFeeVaccineModel.SubsidizeFee.Value + udtSubsidizeFeeInjectionModel.SubsidizeFee.Value, udtSubsidizeFeeVaccineModel.SubsidizeFee.Value, udtSubsidizeFeeVaccineModel.SubsidizeFeeVisible, _
                    udtSubsidizeFeeInjectionModel.SubsidizeFee, udtSubsidizeFeeInjectionModel.SubsidizeFeeVisible)
                ' CRE13-001 - EHAPP [End][Tommy L]
                ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

                Dim udtSubsidizeItemDetailList As SubsidizeItemDetailsModelCollection = Me._udtSchemeDetailBLL.getSubsidizeItemDetails(udtEHSClaimSubsidizeModel.SubsidizeItemCode)


                Dim strRemark As String = String.Empty
                Dim strRemarkChi As String = String.Empty
                Dim arrStrAvailableCode As New List(Of String)

                Dim providerUS As New System.Globalization.CultureInfo("en-us")
                Dim providerTW As New System.Globalization.CultureInfo("zh-tw")

                If udtSubsidizeFeeVaccineModel.SubsidizeFeeVisible Then
                    strRemark = HttpContext.GetGlobalResourceObject("Text", udtSubsidizeFeeVaccineModel.SubsidizeFeeTypeDisplayResource, providerUS) + ": $" + udtSubsidizeFeeVaccineModel.SubsidizeFee.ToString()
                    strRemarkChi = HttpContext.GetGlobalResourceObject("Text", udtSubsidizeFeeVaccineModel.SubsidizeFeeTypeDisplayResource, providerTW) + ": $" + udtSubsidizeFeeVaccineModel.SubsidizeFee.ToString()
                End If

                If udtSubsidizeFeeInjectionModel.SubsidizeFeeVisible Then
                    If strRemark.Trim() <> "" Then strRemark = strRemark + ", "
                    If strRemarkChi.Trim() <> "" Then strRemarkChi = strRemarkChi + ", "
                    strRemark = strRemark + HttpContext.GetGlobalResourceObject("Text", udtSubsidizeFeeInjectionModel.SubsidizeFeeTypeDisplayResource, providerUS) + ": $" + udtSubsidizeFeeInjectionModel.SubsidizeFee.ToString()
                    strRemarkChi = strRemarkChi + HttpContext.GetGlobalResourceObject("Text", udtSubsidizeFeeInjectionModel.SubsidizeFeeTypeDisplayResource, providerTW) + ": $" + udtSubsidizeFeeInjectionModel.SubsidizeFee.ToString()
                End If
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

                ' Checking For Each SubsidizeItemCode: Eg. HSIV->1STDOSE, 2NDDOSE, ONLYDOSE


                ' Vaccine->Subsidze->SubsidizeDetail
                For Each udtSubsidizeItemDetail As SubsidizeItemDetailsModel In udtSubsidizeItemDetailList

                    Dim blnDisplayOnly As Boolean = False
                    Dim blnSkipSubsidizeItemDetail As Boolean = False
                    ' To Do: Here Will Use the Transaction For The Scheme Instead of the Transaction for Subsidize, Any Problem ????!!

                    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
                    ' -----------------------------------------------------------------------------------------
                    Dim udtDoseRuleResult As ClaimRulesBLL.DoseRuleResult = Me._udtClaimRulesBLL.CheckSubsidizeItemDetailRuleByDose(udtAllTransactionVaccineBenefit, _
                        udtSchemeClaimModel.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, _
                        udtSubsidizeItemDetail.SubsidizeItemCode, udtSubsidizeItemDetail.AvailableItemCode, udtEHSAccountModel.getPersonalInformation(strDocCode).DOB, udtEHSAccountModel.getPersonalInformation(strDocCode).ExactDOB, dtmServiceDate)
                    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

                    If udtDoseRuleResult.IsMatch AndAlso (udtDoseRuleResult.HandlingMethod = ClaimRulesBLL.DoseRuleHandlingMethod.ALL Or udtDoseRuleResult.HandlingMethod = ClaimRulesBLL.DoseRuleHandlingMethod.DISPLAY) Then
                        If udtDoseRuleResult.HandlingMethod = ClaimRulesBLL.DoseRuleHandlingMethod.DISPLAY Then
                            blnDisplayOnly = True
                        End If
                    Else
                        ' Not for Select
                        'Continue For
                        blnSkipSubsidizeItemDetail = True
                    End If

                    '-------------------------------------------------------------------
                    ' Check with Exact Match Transaction
                    '-------------------------------------------------------------------

                    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
                    ' -----------------------------------------------------------------------------------------
                    Dim udtSearchedTranDetailList As TransactionDetailModelCollection = udtAllTransactionVaccineBenefit.FilterBySubsidizeItemDetail( _
                        udtSchemeClaimModel.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, _
                        udtSubsidizeItemDetail.SubsidizeItemCode, udtSubsidizeItemDetail.AvailableItemCode)
                    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

                    '-------------------------------------------------------------------
                    ' Check with Equivalent Dose related Transaction (equivalent dose from EqvSubsidizeMap)
                    '-------------------------------------------------------------------


                    ' Dose: SchemeCode, SchemeSeq, SubsidizeCode, SubsidizeItemCode, AvailableItemCode <=> Eqv * 5
                    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
                    ' -----------------------------------------------------------------------------------------
                    Dim udtEqvSubsidizeMapList As EqvSubsidizeMapModelCollection = Me._udtSchemeDetailBLL.getALLEqvSubsidizeMap().GetUniqueEqvMappingByDose( _
                        udtSchemeClaimModel.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, _
                        udtSubsidizeItemDetail.SubsidizeItemCode, udtSubsidizeItemDetail.AvailableItemCode)
                    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

                    ' Merge the Transaction
                    For Each udtEqvSubsidizeMapModel As EqvSubsidizeMapModel In udtEqvSubsidizeMapList
                        Dim udtEquMergeTranDetailList As TransactionDetailModelCollection = udtAllTransactionVaccineBenefit.FilterBySubsidizeItemDetail( _
                            udtEqvSubsidizeMapModel.EqvSchemeCode, udtEqvSubsidizeMapModel.EqvSchemeSeq, udtEqvSubsidizeMapModel.EqvSubsidizeCode, _
                            udtEqvSubsidizeMapModel.EqvSubsidizeItemCode, udtEqvSubsidizeMapModel.EqvAvailableItemCode)

                        For Each udtTranDetail As TransactionDetailModel In udtEquMergeTranDetailList
                            udtSearchedTranDetailList.Add(New TransactionDetailModel(udtTranDetail))
                        Next
                    Next


                    ' Config the dose display 
                    ' -------------------------------------------------------
                    ' 1. For Display the Dose but not available for Selection
                    ' 2. if usage exceed the available entitlement
                    ' 3. if used
                    Dim blnReceived As Boolean = False
                    Dim blnAvailable As Boolean = True
                    If blnDisplayOnly Or udtSearchedTranDetailList.Count > 0 Or blnNoVaccineAvailable Then
                        blnReceived = True
                        blnAvailable = False
                    End If

                    ' For Display the Dose but not available for Selection
                    If blnDisplayOnly Then
                        blnReceived = True
                        blnAvailable = False
                    End If

                    ' If # of Entitlement <= Claimed
                    If blnNoVaccineAvailable Then
                        blnReceived = True
                        blnAvailable = False
                    End If

                    If Not blnSkipSubsidizeItemDetail Then
                        Dim udtEHSClaimSubsidizeDetailModel As New EHSClaimVaccineModel.EHSClaimSubidizeDetailModel( _
                                                udtSubsidizeItemDetail.SubsidizeItemCode, udtSubsidizeItemDetail.AvailableItemCode, udtSubsidizeItemDetail.DisplaySeq, _
                                                udtSubsidizeItemDetail.AvailableItemDesc, udtSubsidizeItemDetail.AvailableItemDescChi, blnAvailable, blnReceived, False)

                        If udtSearchedTranDetailList.Count > 0 Then
                            udtEHSClaimSubsidizeDetailModel.DoseDate = udtSearchedTranDetailList(0).ServiceReceiveDtm
                        End If
                        udtEHSClaimSubsidizeModel.Add(udtEHSClaimSubsidizeDetailModel)
                    End If
                Next


                ' *************************************************************************************
                ' Obsolete on eVaccination Record Enhancement
                ' *************************************************************************************
                'Dim udtUsedTranDetailList As New TransactionDetailModelCollection
                'If udtSubsidizeGroupClaim.SubsidizeItemCode = "PV" Then
                '    udtUsedTranDetailList = udtAllTransactionVaccineBenefit
                'End If

                'If udtUsedTranDetailList.Count > 0 Then
                '    udtUsedTranDetailList.Sort(TransactionDetailModelCollection.enumSortBy.ServiceDate, SortDirection.Descending)
                '    For Each udtTranDetail As TransactionDetailModel In udtUsedTranDetailList
                '        If udtTranDetail.SubsidizeItemCode = "PV" Then Continue For
                '        'If Not arrStrAvailableCode.Contains(udtTranDetail.AvailableItemCode.Trim().ToUpper()) Then
                '        arrStrAvailableCode.Add(udtTranDetail.AvailableItemCode.Trim())

                '        ' TODO: Double check claim dose injected remark
                '        'Dim udtTakenSubsidizeItemDetail As SubsidizeItemDetailsModel = udtSubsidizeItemDetail
                '        'Dim udtTakenSubsidizeItemDetail As SubsidizeItemDetailsModel = udtSubsidizeItemDetailList.Filter(udtTranDetail.SubsidizeItemCode, udtTranDetail.AvailableItemCode.Trim())

                '        If strRemark.Trim() <> "" Then strRemark = strRemark + "<br>"
                '        If strRemarkChi.Trim() <> "" Then strRemarkChi = strRemarkChi + "<br>"

                '        ' TODO: Different available item name in Vaccintaion Record popup and Claim page remark
                '        Dim strAvailableItemDesc As String
                '        Dim strAvailableItemDescChi As String

                '        If udtTranDetail.AvailableItemCode.Trim = "N/A" Or udtTranDetail.AvailableItemCode.Trim = "ONLYDOSE" Then
                '            strAvailableItemDesc = "Injection"
                '            strAvailableItemDescChi = "ª`®g"
                '        Else
                '            strAvailableItemDesc = udtTranDetail.AvailableItemDesc.Trim()
                '            strAvailableItemDescChi = udtTranDetail.AvailableItemDescChi.Trim()
                '        End If
                '        If udtTranDetail.AvailableItemCode.Trim().ToUpper() = SubsidizeItemDetailsModel.DoseCode.VACCINE.Trim().ToUpper() Or udtTranDetail.AvailableItemCode.Trim().ToUpper() = SubsidizeItemDetailsModel.DoseCode.ONLYDOSE.Trim().ToUpper() Then
                '            strRemark = strRemark + "(" + strAvailableItemDesc + HttpContext.GetGlobalResourceObject("Text", "On", providerUS) + Me._udtFormater.formatDate(udtTranDetail.ServiceReceiveDtm, "en-us") + ")"
                '            strRemarkChi = strRemarkChi + "(" + strAvailableItemDescChi + HttpContext.GetGlobalResourceObject("Text", "On", providerTW) + Me._udtFormater.formatDate(udtTranDetail.ServiceReceiveDtm, "zh-tw") + ")"
                '        Else
                '            strRemark = strRemark + "(" + strAvailableItemDesc + HttpContext.GetGlobalResourceObject("Text", "On", providerUS) + Me._udtFormater.formatDate(udtTranDetail.ServiceReceiveDtm, "en-us") + ")"
                '            strRemarkChi = strRemarkChi + "(" + strAvailableItemDescChi + HttpContext.GetGlobalResourceObject("Text", "On", providerTW) + Me._udtFormater.formatDate(udtTranDetail.ServiceReceiveDtm, "zh-tw") + ")"
                '        End If
                '        'End If
                '    Next
                'End If


                udtEHSClaimSubsidizeModel.Remark = strRemark
                udtEHSClaimSubsidizeModel.RemarkChi = strRemarkChi

                ' If Vaccine->Subsidze contain at least 1 SubsidizeDetail 
                If Not udtEHSClaimSubsidizeModel.SubsidizeDetailList Is Nothing AndAlso udtEHSClaimSubsidizeModel.SubsidizeDetailList.Count > 0 Then
                    udtEHSClaimVaccineModel.Add(udtEHSClaimSubsidizeModel)
                    intEntryCount = intEntryCount + 1
                End If
            Next

            If intEntryCount = 1 Then
                udtEHSClaimVaccineModel.SubsidizeList(0).Selected = True
            End If

            Return udtEHSClaimVaccineModel
        End Function

        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        ' Obsolete non reference function
        'Public Function ConstructEHSClaimVaccineModel(ByVal strSchemeCode As String, ByRef udtEHSTransactionModel As EHSTransactionModel) As EHSClaimVaccineModel

        '    ' [SchemeClaim]
        '    Dim udtSchemeClaimModel As SchemeClaimModel = Me._udtSchemeClaimBLL.getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(udtEHSTransactionModel.SchemeCode, udtEHSTransactionModel.TransactionDetails(0).SchemeSeq)
        '    ' [SchemeVaccineDetail]: Vaccine Fee
        '    Dim udtSchemeVaccineDetailList As SchemeVaccineDetailModelCollection = Me._udtSchemeDetailBLL.getSchemeVaccineDetail(udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SchemeSeq)

        '    ' <EHSClaimVaccine>
        '    Dim udtEHSClaimVaccineModel As New EHSClaimVaccineModel(udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SchemeSeq)

        '    ' Loop TransactionDetail
        '    For Each udtTransactionDetail As TransactionDetailModel In udtEHSTransactionModel.TransactionDetails

        '        ' SubsidizeItem
        '        Dim udtSubsidizeGroupClaim As SubsidizeGroupClaimModel = udtSchemeClaimModel.SubsidizeGroupClaimList.Filter(udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SchemeSeq, udtTransactionDetail.SubsidizeCode)
        '        ' Vaccine Fee
        '        Dim udtSchemeVaccineDetailModel As SchemeVaccineDetailModel = udtSchemeVaccineDetailList.Filter(udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode)

        '        Dim strRemark As String = String.Empty
        '        Dim strRemarkChi As String = String.Empty

        '        Dim providerUS As New System.Globalization.CultureInfo("en-us")
        '        Dim providerTW As New System.Globalization.CultureInfo("zh-tw")

        '        If udtSchemeVaccineDetailModel.VaccineFeeDisplayEnabled AndAlso udtSchemeVaccineDetailModel.VaccineFee.HasValue Then
        '            strRemark = HttpContext.GetGlobalResourceObject("Text", "VaccineCost", providerUS) + ": $" + udtSchemeVaccineDetailModel.VaccineFee.ToString()
        '            strRemarkChi = HttpContext.GetGlobalResourceObject("Text", "VaccineCost", providerTW) + ": $" + udtSchemeVaccineDetailModel.VaccineFee.ToString()
        '        End If

        '        If udtSchemeVaccineDetailModel.InjectionFeeDisplayEnabled AndAlso udtSchemeVaccineDetailModel.InjectionFee.HasValue Then
        '            If strRemark.Trim() <> "" Then strRemark = strRemark + ", "
        '            If strRemarkChi.Trim() <> "" Then strRemarkChi = strRemarkChi + ", "
        '            strRemark = strRemark + HttpContext.GetGlobalResourceObject("Text", "InjectionCost", providerUS) + ": $" + udtSchemeVaccineDetailModel.InjectionFee.ToString()
        '            strRemarkChi = strRemarkChi + HttpContext.GetGlobalResourceObject("Text", "InjectionCost", providerTW) + ": $" + udtSchemeVaccineDetailModel.InjectionFee.ToString()
        '        End If

        '        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        '        ' -----------------------------------------------------------------------------------------
        '        ' Vaccine->Subsidize
        '        Dim udtEHSClaimSubsidizeModel As New EHSClaimVaccineModel.EHSClaimSubsidizeModel( _
        '            udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, _
        '            udtSubsidizeGroupClaim.DisplaySeq, udtSubsidizeGroupClaim.SubsidizeItemCode, _
        '            udtSubsidizeGroupClaim.DisplayCodeForClaim, udtSubsidizeGroupClaim.SubsidizeItemDesc, udtSubsidizeGroupClaim.SubsidizeItemDescChi, _
        '            udtSubsidizeGroupClaim.SubsidizeValue, udtSchemeVaccineDetailModel.VaccineFee, udtSchemeVaccineDetailModel.VaccineFeeDisplayEnabled, _
        '            udtSchemeVaccineDetailModel.InjectionFee, udtSchemeVaccineDetailModel.InjectionFeeDisplayEnabled)
        '        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

        '        udtEHSClaimSubsidizeModel.Remark = strRemark
        '        udtEHSClaimSubsidizeModel.RemarkChi = strRemarkChi

        '        ' Vaccine->Subsidze->SubsidizeDetail
        '        Dim udtEHSClaimSubsidizeDetailModel As New EHSClaimVaccineModel.EHSClaimSubidizeDetailModel( _
        '                udtTransactionDetail.SubsidizeItemCode, udtTransactionDetail.AvailableItemCode, 1, _
        '                udtTransactionDetail.AvailableItemDesc, udtTransactionDetail.AvailableItemDescChi, True, False, True)

        '        udtEHSClaimSubsidizeModel.Add(udtEHSClaimSubsidizeDetailModel)
        '        udtEHSClaimSubsidizeModel.Selected = True

        '        udtEHSClaimVaccineModel.Add(udtEHSClaimSubsidizeModel)
        '    Next

        '    Return udtEHSClaimVaccineModel
        'End Function
        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]


        Public Function ConstructEHSClaimVaccineModel2(ByVal udtSchemeClaimModel As SchemeClaimModel, ByVal strDocCode As String, ByVal udtEHSAccountModel As EHSAccountModel, ByVal dtmServiceDate As Date, ByVal blnDynamicControl As Boolean, Optional ByVal strCategoryCode As String = "") As EHSClaimVaccineModel

            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            'Dim udtEHSClaimVaccineModel As New EHSClaimVaccineModel(udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SchemeSeq)
            Dim udtEHSClaimVaccineModel As New EHSClaimVaccineModel(udtSchemeClaimModel.SchemeCode)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

            ' Recipient used benefit
            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            Dim udtTransactionDetailList As TransactionDetailModelCollection = Me._udtEHSTransactionBLL.getTransactionDetailBenefit(strDocCode, udtEHSAccountModel.getPersonalInformation(strDocCode).IdentityNum)
            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

            ' Auto Selected If only 1 Entry found
            Dim intEntryCount As Integer = 0

            ' Vaccine->Subsidize

            ' By the selected service date, determine whether the subsidize is available for the patient
            For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtSchemeClaimModel.SubsidizeGroupClaimList

                ' ''TO:Do Remove checking about the period
                'If blnDynamicControl Then
                '    ' Out of Service Period, Or Not Eligible
                '    If udtSubsidizeGroupClaim.LastServiceDtm < dtmServiceDate OrElse Not Me._udtClaimRulesBLL.CheckEligibilityPerSubsidize(udtSubsidizeGroupClaim, udtEHSAccountModel.getPersonalInformation(strDocCode).DOB, udtEHSAccountModel.getPersonalInformation(strDocCode).ExactDOB, dtmServiceDate, udtTransactionDetailList).IsEligible Then
                '        Continue For
                '    End If
                'End If

                If strCategoryCode.Trim() <> "" Then
                    ' If Category is passed, Check Exist of Category
                    Dim udtClaimCategoryBLL As New ClaimCategory.ClaimCategoryBLL()
                    If udtClaimCategoryBLL.getAllCategoryCache().Filter(udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, strCategoryCode) Is Nothing Then
                        Continue For
                    Else
                        ' Category Exist, Check for Category Eligibility for the Subsidize
                        Dim udtEligibilityRuleResult As ClaimRulesBLL.EligibleResult = Me._udtClaimRulesBLL.CheckCategoryEligibilityByCategory(udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, strCategoryCode, udtEHSAccountModel.getPersonalInformation(strDocCode).DOB, udtEHSAccountModel.getPersonalInformation(strDocCode).ExactDOB, dtmServiceDate)
                        If Not udtEligibilityRuleResult.IsEligible Then
                            Continue For
                        End If
                    End If
                End If

                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                ' Vaccine Fee
                'Dim udtSchemeVaccineDetailModel As SchemeVaccineDetailModel = Me._udtSchemeDetailBLL.getSchemeVaccineDetail(udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode)
                Dim udtSubsidizeFeeVaccineModel As SubsidizeFeeModel = udtSubsidizeGroupClaim.SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVaccine, dtmServiceDate)
                Dim udtSubsidizeFeeInjectionModel As SubsidizeFeeModel = udtSubsidizeGroupClaim.SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeInjection, dtmServiceDate)

                ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
                ' -----------------------------------------------------------------------------------------
                ' CRE13-001 - EHAPP [Start][Tommy L]
                ' -------------------------------------------------------------------------------------
                'Dim udtEHSClaimSubsidizeModel As New EHSClaimVaccineModel.EHSClaimSubsidizeModel( _
                '   udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, _
                '   udtSubsidizeGroupClaim.DisplaySeq, udtSubsidizeGroupClaim.SubsidizeItemCode, _
                '   IIf(UploadClaimBLL.UseParticularSubsidyCode, udtSubsidizeGroupClaim.DisplayCodeForClaim, udtSubsidizeGroupClaim.SubsidizeDisplayCode), _
                '   udtSubsidizeGroupClaim.SubsidizeItemDesc, udtSubsidizeGroupClaim.SubsidizeItemDescChi, _
                '   udtSubsidizeFeeVaccineModel.SubsidizeFee + udtSubsidizeFeeInjectionModel.SubsidizeFee, udtSubsidizeFeeVaccineModel.SubsidizeFee, udtSubsidizeFeeVaccineModel.SubsidizeFeeVisible, _
                '   udtSubsidizeFeeInjectionModel.SubsidizeFee, udtSubsidizeFeeInjectionModel.SubsidizeFeeVisible)
                Dim udtEHSClaimSubsidizeModel As New EHSClaimVaccineModel.EHSClaimSubsidizeModel( _
                    udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, _
                    udtSubsidizeGroupClaim.DisplaySeq, udtSubsidizeGroupClaim.SubsidizeItemCode, _
                    IIf(UploadClaimBLL.UseParticularSubsidyCode, udtSubsidizeGroupClaim.DisplayCodeForClaim, udtSubsidizeGroupClaim.SubsidizeDisplayCode), _
                    udtSubsidizeGroupClaim.SubsidizeItemDesc, udtSubsidizeGroupClaim.SubsidizeItemDescChi, _
                    udtSubsidizeFeeVaccineModel.SubsidizeFee.Value + udtSubsidizeFeeInjectionModel.SubsidizeFee.Value, udtSubsidizeFeeVaccineModel.SubsidizeFee.Value, udtSubsidizeFeeVaccineModel.SubsidizeFeeVisible, _
                    udtSubsidizeFeeInjectionModel.SubsidizeFee, udtSubsidizeFeeInjectionModel.SubsidizeFeeVisible)
                ' CRE13-001 - EHAPP [End][Tommy L]
                ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

                Dim udtSubsidizeItemDetailList As SubsidizeItemDetailsModelCollection = Me._udtSchemeDetailBLL.getSubsidizeItemDetails(udtEHSClaimSubsidizeModel.SubsidizeItemCode)

                Dim strRemark As String = String.Empty
                Dim strRemarkChi As String = String.Empty
                Dim arrStrAvailableCode As New List(Of String)

                Dim providerUS As New System.Globalization.CultureInfo("en-us")
                Dim providerTW As New System.Globalization.CultureInfo("zh-tw")

                If udtSubsidizeFeeVaccineModel.SubsidizeFeeVisible Then
                    strRemark = HttpContext.GetGlobalResourceObject("Text", udtSubsidizeFeeVaccineModel.SubsidizeFeeTypeDisplayResource, providerUS) + ": $" + udtSubsidizeFeeVaccineModel.SubsidizeFee.ToString()
                    strRemarkChi = HttpContext.GetGlobalResourceObject("Text", udtSubsidizeFeeVaccineModel.SubsidizeFeeTypeDisplayResource, providerTW) + ": $" + udtSubsidizeFeeVaccineModel.SubsidizeFee.ToString()
                End If

                If udtSubsidizeFeeInjectionModel.SubsidizeFeeVisible Then
                    If strRemark.Trim() <> "" Then strRemark = strRemark + ", "
                    If strRemarkChi.Trim() <> "" Then strRemarkChi = strRemarkChi + ", "
                    strRemark = strRemark + HttpContext.GetGlobalResourceObject("Text", udtSubsidizeFeeInjectionModel.SubsidizeFeeTypeDisplayResource, providerUS) + ": $" + udtSubsidizeFeeInjectionModel.SubsidizeFee.ToString()
                    strRemarkChi = strRemarkChi + HttpContext.GetGlobalResourceObject("Text", udtSubsidizeFeeInjectionModel.SubsidizeFeeTypeDisplayResource, providerTW) + ": $" + udtSubsidizeFeeInjectionModel.SubsidizeFee.ToString()
                End If
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

                ' Checking For Each SubsidizeItemCode: Eg. HSIV->1STDOSE, 2NDDOSE, ONLYDOSE

                ' Vaccine->Subsidze->SubsidizeDetail
                For Each udtSubsidizeItemDetail As SubsidizeItemDetailsModel In udtSubsidizeItemDetailList

                    Dim blnDisplayOnly As Boolean = False
                    Dim blnSkipSubsidizeItemDetail As Boolean = False
                    ' To Do: Here Will Use the Transaction For The Scheme Instead of the Transaction for Subsidize, Any Problem ????!!

                    Dim udtDoseRuleResult As ClaimRulesBLL.DoseRuleResult = Me._udtClaimRulesBLL.CheckSubsidizeItemDetailRuleByDose(udtTransactionDetailList, _
                        udtSchemeClaimModel.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, _
                        udtSubsidizeItemDetail.SubsidizeItemCode, udtSubsidizeItemDetail.AvailableItemCode, udtEHSAccountModel.getPersonalInformation(strDocCode).DOB, udtEHSAccountModel.getPersonalInformation(strDocCode).ExactDOB, dtmServiceDate)

                    If udtDoseRuleResult.IsMatch AndAlso (udtDoseRuleResult.HandlingMethod = ClaimRulesBLL.DoseRuleHandlingMethod.ALL Or udtDoseRuleResult.HandlingMethod = ClaimRulesBLL.DoseRuleHandlingMethod.DISPLAY) Then
                        If udtDoseRuleResult.HandlingMethod = ClaimRulesBLL.DoseRuleHandlingMethod.DISPLAY Then
                            blnDisplayOnly = True
                        End If
                    Else
                        ' Not for Select
                        'Continue For
                        'blnSkipSubsidizeItemDetail = True
                    End If

                    '-------------------------------------------------------------------
                    ' Check with Exact Match Transaction
                    '-------------------------------------------------------------------
                    Dim udtSearchedTranDetailList As TransactionDetailModelCollection = udtTransactionDetailList.FilterBySubsidizeItemDetail( _
                        udtSchemeClaimModel.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, _
                        udtSubsidizeItemDetail.SubsidizeItemCode, udtSubsidizeItemDetail.AvailableItemCode)
                    '-------------------------------------------------------------------
                    ' Check with Equivalent Dose related Transaction (equivalent dose from EqvSubsidizeMap)
                    '-------------------------------------------------------------------

                    ' Dose: SchemeCode, SchemeSeq, SubsidizeCode, SubsidizeItemCode, AvailableItemCode <=> Eqv * 5
                    Dim udtEqvSubsidizeMapList As EqvSubsidizeMapModelCollection = Me._udtSchemeDetailBLL.getALLEqvSubsidizeMap().GetUniqueEqvMappingByDose( _
                        udtSchemeClaimModel.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, _
                        udtSubsidizeItemDetail.SubsidizeItemCode, udtSubsidizeItemDetail.AvailableItemCode)

                    ' Merge the Transaction
                    For Each udtEqvSubsidizeMapModel As EqvSubsidizeMapModel In udtEqvSubsidizeMapList
                        Dim udtEquMergeTranDetailList As TransactionDetailModelCollection = udtTransactionDetailList.FilterBySubsidizeItemDetail( _
                            udtEqvSubsidizeMapModel.EqvSchemeCode, udtEqvSubsidizeMapModel.EqvSchemeSeq, udtEqvSubsidizeMapModel.EqvSubsidizeCode, _
                            udtEqvSubsidizeMapModel.EqvSubsidizeItemCode, udtEqvSubsidizeMapModel.EqvAvailableItemCode)

                        For Each udtTranDetail As TransactionDetailModel In udtEquMergeTranDetailList
                            udtSearchedTranDetailList.Add(New TransactionDetailModel(udtTranDetail))
                        Next
                    Next

                    Dim blnReceived As Boolean = False
                    Dim blnAvailable As Boolean = True
                    If udtSearchedTranDetailList.Count > 0 Then
                        blnReceived = True
                        blnAvailable = False

                        If Not arrStrAvailableCode.Contains(udtSearchedTranDetailList(0).AvailableItemCode.Trim().ToUpper()) Then
                            arrStrAvailableCode.Add(udtSearchedTranDetailList(0).AvailableItemCode.Trim())

                            Dim udtTakenSubsidizeItemDetail As SubsidizeItemDetailsModel = udtSubsidizeItemDetailList.Filter(udtSearchedTranDetailList(0).SubsidizeItemCode, udtSearchedTranDetailList(0).AvailableItemCode.Trim())

                            For Each udtSearchedTranDetail As TransactionDetailModel In udtSearchedTranDetailList
                                If strRemark.Trim() <> "" Then strRemark = strRemark + "<br>"
                                If strRemarkChi.Trim() <> "" Then strRemarkChi = strRemarkChi + "<br>"

                                If udtSearchedTranDetailList(0).AvailableItemCode.Trim().ToUpper() = SubsidizeItemDetailsModel.DoseCode.VACCINE.Trim().ToUpper() Or udtSearchedTranDetailList(0).AvailableItemCode.Trim().ToUpper() = SubsidizeItemDetailsModel.DoseCode.ONLYDOSE.Trim().ToUpper() Then
                                    strRemark = strRemark + "(" + udtTakenSubsidizeItemDetail.AvailableItemDesc.Trim() + HttpContext.GetGlobalResourceObject("Text", "On", providerUS) + Me._udtFormater.formatDate(udtSearchedTranDetail.ServiceReceiveDtm, "en-us") + ")"
                                    strRemarkChi = strRemarkChi + "(" + udtTakenSubsidizeItemDetail.AvailableItemDescChi.Trim() + HttpContext.GetGlobalResourceObject("Text", "On", providerTW) + Me._udtFormater.formatDate(udtSearchedTranDetail.ServiceReceiveDtm, "zh-tw") + ")"
                                Else
                                    strRemark = strRemark + "(" + udtTakenSubsidizeItemDetail.AvailableItemDesc.Trim() + HttpContext.GetGlobalResourceObject("Text", "On", providerUS) + Me._udtFormater.formatDate(udtSearchedTranDetail.ServiceReceiveDtm, "en-us") + ")"
                                    strRemarkChi = strRemarkChi + "(" + udtTakenSubsidizeItemDetail.AvailableItemDescChi.Trim() + HttpContext.GetGlobalResourceObject("Text", "On", providerTW) + Me._udtFormater.formatDate(udtSearchedTranDetail.ServiceReceiveDtm, "zh-tw") + ")"
                                End If
                            Next

                        End If
                    End If

                    ' For Display the Dose but not available for Selection
                    If blnDisplayOnly Then
                        blnReceived = True
                        blnAvailable = False
                    End If

                    If Not blnSkipSubsidizeItemDetail Then
                        Dim udtEHSClaimSubsidizeDetailModel As New EHSClaimVaccineModel.EHSClaimSubidizeDetailModel( _
                                                udtSubsidizeItemDetail.SubsidizeItemCode, udtSubsidizeItemDetail.AvailableItemCode, udtSubsidizeItemDetail.DisplaySeq, _
                                                udtSubsidizeItemDetail.AvailableItemDesc, udtSubsidizeItemDetail.AvailableItemDescChi, blnAvailable, blnReceived, False)

                        If udtSearchedTranDetailList.Count > 0 Then
                            udtEHSClaimSubsidizeDetailModel.DoseDate = udtSearchedTranDetailList(0).ServiceReceiveDtm
                        End If
                        udtEHSClaimSubsidizeModel.Add(udtEHSClaimSubsidizeDetailModel)
                    End If
                Next

                udtEHSClaimSubsidizeModel.Remark = strRemark
                udtEHSClaimSubsidizeModel.RemarkChi = strRemarkChi

                ' If Vaccine->Subsidze contain at least 1 SubsidizeDetail 
                If Not udtEHSClaimSubsidizeModel.SubsidizeDetailList Is Nothing AndAlso udtEHSClaimSubsidizeModel.SubsidizeDetailList.Count > 0 Then
                    udtEHSClaimVaccineModel.Add(udtEHSClaimSubsidizeModel)
                    intEntryCount = intEntryCount + 1
                End If
            Next

            If intEntryCount = 1 Then
                udtEHSClaimVaccineModel.SubsidizeList(0).Selected = True
            End If

            Return udtEHSClaimVaccineModel
        End Function



#End Region


#Region "Create Model"

        ' EHS Temp Voucher Account -----------------------

        ''' <summary>
        ''' Create New Temporary EHS Account Model after no record found
        ''' </summary>
        ''' <param name="strIdentityNum"></param>
        ''' <param name="strDocCode"></param>
        ''' <param name="strExactDOB"></param>
        ''' <param name="dtmDOB"></param>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="strAdoptionPrefixNum"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ConstructEHSTemporaryVoucherAccount(ByVal strIdentityNum As String, ByVal strDocCode As String, ByVal strExactDOB As String, ByVal dtmDOB As DateTime, ByVal strSchemeCode As String, ByVal strAdoptionPrefixNum As String) As EHSAccountModel
            Dim udtEHSAccount As New EHSAccountModel()

            udtEHSAccount.SchemeCode = strSchemeCode
            udtEHSAccount.EHSPersonalInformationList(0).VoucherAccID = String.Empty
            udtEHSAccount.EHSPersonalInformationList(0).IdentityNum = strIdentityNum
            udtEHSAccount.EHSPersonalInformationList(0).ExactDOB = strExactDOB
            udtEHSAccount.EHSPersonalInformationList(0).DOB = dtmDOB
            udtEHSAccount.EHSPersonalInformationList(0).DocCode = strDocCode
            If strAdoptionPrefixNum Is Nothing Then
                strAdoptionPrefixNum = String.Empty
            End If
            udtEHSAccount.EHSPersonalInformationList(0).AdoptionPrefixNum = strAdoptionPrefixNum
            udtEHSAccount.EHSPersonalInformationList(0).SetDOBTypeSelected(False)

            Return udtEHSAccount

        End Function

        ''' <summary>
        ''' Create New Temporary EHS Account Model after no record found (For EC Case Age on Date of Registration)
        ''' </summary>
        ''' <param name="strIdentityNum"></param>
        ''' <param name="strDocCode"></param>
        ''' <param name="intAge"></param>
        ''' <param name="dtmDOR"></param>
        ''' <param name="strSchemeCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ConstructEHSTemporaryVoucherAccount(ByVal strIdentityNum As String, ByVal strDocCode As String, ByVal intAge As Integer, ByVal dtmDOR As DateTime, ByVal strSchemeCode As String) As EHSAccountModel
            Dim udtEHSAccount As New EHSAccountModel()

            udtEHSAccount.SchemeCode = strSchemeCode
            udtEHSAccount.EHSPersonalInformationList(0).VoucherAccID = String.Empty
            udtEHSAccount.EHSPersonalInformationList(0).IdentityNum = strIdentityNum
            udtEHSAccount.EHSPersonalInformationList(0).ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration
            udtEHSAccount.EHSPersonalInformationList(0).ECDateOfRegistration = dtmDOR
            udtEHSAccount.EHSPersonalInformationList(0).DOB = dtmDOR.AddYears(-intAge)
            udtEHSAccount.EHSPersonalInformationList(0).ECAge = intAge
            udtEHSAccount.EHSPersonalInformationList(0).DocCode = strDocCode
            udtEHSAccount.EHSPersonalInformationList(0).SetDOBTypeSelected(False)
            Return udtEHSAccount

        End Function

        ' EHS Transaction ---------------------------------

        ''' <summary>
        ''' Construct EHS Transaction Model
        ''' </summary>
        ''' <param name="udtSchemeClaimModel"></param>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="udtPracticeDisplayModel"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ConstructNewEHSTransaction(ByVal udtSchemeClaimModel As SchemeClaimModel, ByVal udtEHSAccount As EHSAccountModel, ByVal udtPracticeDisplayModel As PracticeDisplayModel) As EHSTransactionModel
            Dim udtEHSTran As New EHSTransactionModel()

            If Not udtEHSAccount.IsNew() Then
                If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                    udtEHSTran.VoucherAccID = udtEHSAccount.VoucherAccID
                Else
                    udtEHSTran.TempVoucherAccID = udtEHSAccount.VoucherAccID
                End If
            End If
            udtEHSTran.SchemeCode = udtSchemeClaimModel.SchemeCode
            udtEHSTran.ServiceDate = DateTime.Now
            udtEHSTran.ServiceType = udtPracticeDisplayModel.ServiceCategoryCode
            udtEHSTran.ServiceProviderID = udtPracticeDisplayModel.SPID
            udtEHSTran.PracticeID = udtPracticeDisplayModel.PracticeID
            udtEHSTran.BankAccountID = udtPracticeDisplayModel.BankAcctID
            udtEHSTran.BankAccountNo = udtPracticeDisplayModel.BankAccountNo
            udtEHSTran.BankAccountOwner = udtPracticeDisplayModel.BankAccHolder
            udtEHSTran.ClaimAmount = 0

            ' Set External Reference Status (e.g. Vaccination record from CMS)
            'Dim udtExtRefStatus As EHSTransactionModel.ExtRefStatusClass = (New SessionHandler).ExtRefStatusGetFromSession()
            'If udtExtRefStatus Is Nothing Then udtExtRefStatus = New EHSTransactionModel.ExtRefStatusClass
            'udtEHSTran.ExtRefStatus = udtExtRefStatus.Code



            If udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVoucher Then

                Dim dtmCurrentDate As Date = Me._udtCommonGenFunc.GetSystemDateTime()

                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                'udtEHSTran.PerVoucherValue = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeValue
                'udtEHSTran.VoucherBeforeRedeem = Me._udtEHSTransactionBLL.getAvailableVoucher(dtmCurrentDate, udtEHSAccount.SearchDocCode, _
                'udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).IdentityNum, _
                'udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DOB, _
                'udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ExactDOB, _
                'udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SchemeSeq, udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeCode)
                udtEHSTran.PerVoucherValue = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVoucher).SubsidizeFee
                udtEHSTran.VoucherBeforeRedeem = Me._udtEHSTransactionBLL.getAvailableVoucher(dtmCurrentDate, udtEHSAccount.SearchDocCode, _
                    udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).IdentityNum, _
                    udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DOB, _
                    udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ExactDOB, _
                    udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeCode)
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
            End If

            Return udtEHSTran

        End Function

        ' Transaction Detail ------------------------------

        ''' <summary>
        ''' Construct the EHS Transaction Detail Model for Vaccination
        ''' use servicedate to retrieve the active scheme and active subsidize
        ''' </summary>
        ''' <param name="strSPID"></param>
        ''' <param name="udtDataEntry"></param>
        ''' <param name="udtEHSTransactionModel"></param>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="udtEHSClaimVaccineModel"></param>
        ''' <remarks></remarks>
        Public Sub ConstructEHSTransactionDetails(ByVal strSPID As String, ByVal udtDataEntry As DataEntryUserModel, _
            ByRef udtEHSTransactionModel As EHSTransactionModel, ByVal udtEHSAccount As EHSAccountModel, ByRef udtEHSClaimVaccineModel As EHSClaimVaccineModel)

            ' CRE13-001 - EHAPP [Start][Tommy L]
            ' -------------------------------------------------------------------------------------
            Dim udtSchemeClaimModel As SchemeClaimModel = Me._udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSClaimVaccineModel.SchemeCode, udtEHSTransactionModel.ServiceDate.AddDays(1).AddMinutes(-1))
            ' CRE13-001 - EHAPP [End][Tommy L]

            Dim dblClaimAmount As Double = 0

            ' VoucherTransaction
            If udtDataEntry Is Nothing Then
                If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                    ' CRE13-001 - EHAPP [Start][Tommy L]
                    ' -------------------------------------------------------------------------------------
                    'udtEHSTransactionModel.RecordStatus = EHSTransactionModel.TransRecordStatusClass.Active
                    udtEHSTransactionModel.RecordStatus = udtSchemeClaimModel.ConfirmedTransactionStatus
                    ' CRE13-001 - EHAPP [End][Tommy L]
                    udtEHSTransactionModel.VoucherAccID = udtEHSAccount.VoucherAccID
                Else
                    udtEHSTransactionModel.RecordStatus = EHSTransactionModel.TransRecordStatusClass.PendingVRValidate
                    udtEHSTransactionModel.TempVoucherAccID = udtEHSAccount.VoucherAccID
                End If

                udtEHSTransactionModel.CreateBy = strSPID
                udtEHSTransactionModel.UpdateBy = strSPID
                udtEHSTransactionModel.DataEntryBy = String.Empty
            Else

                If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                    udtEHSTransactionModel.VoucherAccID = udtEHSAccount.VoucherAccID
                Else
                    udtEHSTransactionModel.TempVoucherAccID = udtEHSAccount.VoucherAccID
                End If
                udtEHSTransactionModel.RecordStatus = EHSTransactionModel.TransRecordStatusClass.Pending
                udtEHSTransactionModel.CreateBy = strSPID
                udtEHSTransactionModel.UpdateBy = strSPID
                udtEHSTransactionModel.DataEntryBy = udtDataEntry.DataEntryAccount
            End If

            Dim udtEHSPersonalInformationModel As EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode)
            udtEHSTransactionModel.DocCode = udtEHSAccount.SearchDocCode
            udtEHSTransactionModel.TransactionDetails = New TransactionDetailModelCollection()

            For Each udtSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccineModel.SubsidizeList

                If udtSubsidize.Selected Then

                    ' ------------------------------------------------------------------------
                    ' Construct the Detail usign the Active Scheme & Subsidize By Service date 
                    ' ------------------------------------------------------------------------
                    ' RVP->PV 2009Oct19 , RVP->HSIV 2009Dec28 08:00 
                    ' Service Date: 2009Dec08, use 2009Dec08 23:59 to search

                    ' CRE13-001 - EHAPP [Start][Tommy L]
                    ' -------------------------------------------------------------------------------------
                    'Dim udtSchemeClaimModel As SchemeClaimModel = Me._udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSClaimVaccineModel.SchemeCode, udtEHSTransactionModel.ServiceDate.AddDays(1).AddMinutes(-1))
                    ' CRE13-001 - EHAPP [End][Tommy L]

                    If udtSubsidize.SubsidizeDetailList.Count = 1 Then

                        Dim udtEHSTransactionDetail As New EHSTransaction.TransactionDetailModel()
                        udtEHSTransactionDetail.SchemeCode = udtEHSClaimVaccineModel.SchemeCode
                        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
                        ' -----------------------------------------------------------------------------------------
                        udtEHSTransactionDetail.SchemeSeq = udtSubsidize.SchemeSeq
                        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

                        'udtEHSTransactionDetail.SchemeSeq = udtSchemeClaimModel.SubsidizeGroupClaimList.Filter(udtEHSClaimVaccineModel.SchemeCode, udtSubsidize.SubsidizeCode).SchemeSeq
                        'udtEHSTransactionDetail.SchemeSeq = udtEHSClaimVaccineModel.SchemeSeq
                        udtEHSTransactionDetail.SubsidizeCode = udtSubsidize.SubsidizeCode
                        udtEHSTransactionDetail.SubsidizeItemCode = udtSubsidize.SubsidizeItemCode
                        udtEHSTransactionDetail.AvailableItemCode = udtSubsidize.SubsidizeDetailList(0).AvailableItemCode
                        udtEHSTransactionDetail.Unit = 1
                        udtEHSTransactionDetail.PerUnitValue = udtSubsidize.Amount
                        udtEHSTransactionDetail.TotalAmount = udtEHSTransactionDetail.Unit.Value * udtEHSTransactionDetail.PerUnitValue.Value
                        udtEHSTransactionDetail.Remark = ""
                        udtEHSTransactionDetail.DOB = udtEHSPersonalInformationModel.DOB
                        udtEHSTransactionDetail.ExactDOB = udtEHSPersonalInformationModel.ExactDOB
                        udtEHSTransactionDetail.ServiceReceiveDtm = udtEHSTransactionModel.ServiceDate

                        udtEHSTransactionModel.TransactionDetails.Add(udtEHSTransactionDetail)

                        dblClaimAmount = dblClaimAmount + udtEHSTransactionDetail.TotalAmount.Value

                    ElseIf udtSubsidize.SubsidizeDetailList.Count > 1 Then
                        For Each udtSubsidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtSubsidize.SubsidizeDetailList
                            If udtSubsidizeDetail.Selected Then
                                Dim udtEHSTransactionDetail As New EHSTransaction.TransactionDetailModel()
                                udtEHSTransactionDetail.SchemeCode = udtEHSClaimVaccineModel.SchemeCode

                                ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
                                ' -----------------------------------------------------------------------------------------
                                udtEHSTransactionDetail.SchemeSeq = udtSubsidize.SchemeSeq
                                ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

                                'udtEHSTransactionDetail.SchemeSeq = udtEHSClaimVaccineModel.SchemeSeq
                                udtEHSTransactionDetail.SubsidizeCode = udtSubsidize.SubsidizeCode
                                udtEHSTransactionDetail.SubsidizeItemCode = udtSubsidize.SubsidizeItemCode
                                udtEHSTransactionDetail.AvailableItemCode = udtSubsidizeDetail.AvailableItemCode
                                udtEHSTransactionDetail.Unit = 1
                                udtEHSTransactionDetail.PerUnitValue = udtSubsidize.Amount
                                udtEHSTransactionDetail.TotalAmount = udtEHSTransactionDetail.Unit.Value * udtEHSTransactionDetail.PerUnitValue.Value
                                udtEHSTransactionDetail.Remark = ""
                                udtEHSTransactionDetail.DOB = udtEHSPersonalInformationModel.DOB
                                udtEHSTransactionDetail.ExactDOB = udtEHSPersonalInformationModel.ExactDOB
                                udtEHSTransactionDetail.ServiceReceiveDtm = udtEHSTransactionModel.ServiceDate

                                udtEHSTransactionModel.TransactionDetails.Add(udtEHSTransactionDetail)

                                dblClaimAmount = dblClaimAmount + udtEHSTransactionDetail.TotalAmount.Value
                            End If
                        Next
                    End If
                End If
            Next

            udtEHSTransactionModel.ClaimAmount = dblClaimAmount
        End Sub

        ''' <summary>
        ''' Construct EHS Transaction Detail For Voucher
        ''' </summary>
        ''' <param name="strSPID"></param>
        ''' <param name="udtDataEntry"></param>
        ''' <param name="udtEHSTransactionModel"></param>
        ''' <param name="udtEHSAccount"></param>
        ''' <remarks></remarks>
        Public Sub ConstructEHSTransactionDetails(ByVal strSPID As String, ByVal udtDataEntry As DataEntryUserModel, _
            ByRef udtEHSTransactionModel As EHSTransactionModel, ByVal udtEHSAccount As EHSAccountModel)

            ' CRE13-001 - EHAPP [Start][Tommy L]
            ' -------------------------------------------------------------------------------------
            Dim udtSchemeClaimModel As SchemeClaimModel = Me._udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSTransactionModel.SchemeCode, udtEHSTransactionModel.ServiceDate.AddDays(1).AddMinutes(-1))
            ' CRE13-001 - EHAPP [End][Tommy L]

            ' VoucherTransaction
            If udtDataEntry Is Nothing Then
                If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                    ' CRE13-001 - EHAPP [Start][Tommy L]
                    ' -------------------------------------------------------------------------------------
                    'udtEHSTransactionModel.RecordStatus = EHSTransactionModel.TransRecordStatusClass.Active
                    udtEHSTransactionModel.RecordStatus = udtSchemeClaimModel.ConfirmedTransactionStatus
                    ' CRE13-001 - EHAPP [End][Tommy L]
                    udtEHSTransactionModel.VoucherAccID = udtEHSAccount.VoucherAccID
                Else
                    udtEHSTransactionModel.RecordStatus = EHSTransactionModel.TransRecordStatusClass.PendingVRValidate
                    udtEHSTransactionModel.TempVoucherAccID = udtEHSAccount.VoucherAccID
                End If

                udtEHSTransactionModel.CreateBy = strSPID
                udtEHSTransactionModel.UpdateBy = strSPID
                udtEHSTransactionModel.DataEntryBy = String.Empty
            Else

                If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                    udtEHSTransactionModel.VoucherAccID = udtEHSAccount.VoucherAccID
                Else
                    udtEHSTransactionModel.TempVoucherAccID = udtEHSAccount.VoucherAccID
                End If
                udtEHSTransactionModel.RecordStatus = EHSTransactionModel.TransRecordStatusClass.Pending
                udtEHSTransactionModel.CreateBy = strSPID
                udtEHSTransactionModel.UpdateBy = strSPID
                udtEHSTransactionModel.DataEntryBy = udtDataEntry.DataEntryAccount
            End If

            udtEHSTransactionModel.DocCode = udtEHSAccount.SearchDocCode
            udtEHSTransactionModel.TransactionDetails = New TransactionDetailModelCollection()

            Dim udtEHSTransactionDetail As New EHSTransaction.TransactionDetailModel()

            ' ------------------------------------------------------------------------
            ' Construct the Detail usign the Active Scheme & Subsidize By Service date 
            ' ------------------------------------------------------------------------

            ' CRE13-001 - EHAPP [Start][Tommy L]
            ' -------------------------------------------------------------------------------------
            'Dim udtSchemeClaimModel As SchemeClaimModel = Me._udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSTransactionModel.SchemeCode, udtEHSTransactionModel.ServiceDate.AddDays(1).AddMinutes(-1))
            ' CRE13-001 - EHAPP [End][Tommy L]

            udtEHSTransactionDetail.SchemeCode = udtSchemeClaimModel.SchemeCode
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            'udtEHSTransactionDetail.SchemeSeq = udtSchemeClaimModel.SchemeSeq
            udtEHSTransactionDetail.SchemeSeq = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SchemeSeq
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
            udtEHSTransactionDetail.SubsidizeCode = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeCode

            udtEHSTransactionDetail.SubsidizeItemCode = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeItemCode

            Dim udtSubsidizeItemDetailList As SubsidizeItemDetailsModelCollection = Me._udtSchemeDetailBLL.getSubsidizeItemDetails(udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeItemCode)
            udtEHSTransactionDetail.AvailableItemCode = udtSubsidizeItemDetailList(0).AvailableItemCode
            udtEHSTransactionDetail.Unit = udtEHSTransactionModel.VoucherClaim
            udtEHSTransactionDetail.PerUnitValue = udtEHSTransactionModel.PerVoucherValue
            udtEHSTransactionDetail.TotalAmount = udtEHSTransactionDetail.Unit.Value * udtEHSTransactionDetail.PerUnitValue.Value
            udtEHSTransactionDetail.Remark = ""
            udtEHSTransactionModel.TransactionDetails.Add(udtEHSTransactionDetail)

            udtEHSTransactionModel.VoucherAfterRedeem = udtEHSTransactionModel.VoucherBeforeRedeem - udtEHSTransactionModel.VoucherClaim
            udtEHSTransactionModel.ClaimAmount = udtEHSTransactionDetail.TotalAmount


        End Sub
#End Region

    End Class

End Namespace
