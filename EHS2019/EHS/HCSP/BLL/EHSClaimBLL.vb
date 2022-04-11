Imports System.Data.SqlClient
Imports System.Threading
Imports Common.ComFunction
Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.ClaimRules
Imports Common.Component.DataEntryUser
Imports Common.Component.DocType
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.EHSTransaction
Imports Common.Component.InputPicker
Imports Common.Component.Practice
Imports Common.Component.Scheme
Imports Common.Component.SchemeDetails
Imports Common.Component.ServiceProvider
Imports Common.Component.UserAC
Imports Common.DataAccess
Imports Common.Format
Imports Common.WebService.Interface
Imports HCSP.EHSClaimVaccineModel
Imports Common.Component.HAServicePatient


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

#Region "Check Rules"


        Public Function CheckClaimCategoryEligibilityForEnterClaim(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel, ByVal dtmServiceDate As Date, _
            ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, ByVal strCategoryCode As String, ByRef udtEligibilityRuleResult As ClaimRulesBLL.EligibleResult) As Common.ComObject.SystemMessage

            Dim strFunctCode As String = "990000"
            Dim strSeverity As String = "E"
            Dim strMsgCode As String = String.Empty

            For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
                If udtEHSClaimSubsidize.Selected Then
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                    ' -----------------------------------------------------------------------------------------
                    'If udtEHSPersonalInfo.DocCode = DocTypeModel.DocTypeCode.EC AndAlso udtEHSPersonalInfo.ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                    '    udtEligibilityRuleResult = Me._udtClaimRulesBLL.CheckCategoryEligibilityByCategory(udtEHSClaimSubsidize.SchemeCode, udtEHSClaimSubsidize.SchemeSeq, udtEHSClaimSubsidize.SubsidizeCode, strCategoryCode, udtEHSPersonalInfo.ECDateOfRegistration.Value.AddYears(-udtEHSPersonalInfo.ECAge.Value), "Y", dtmServiceDate, udtEHSPersonalInfo.Gender)
                    'Else
                    '    udtEligibilityRuleResult = Me._udtClaimRulesBLL.CheckCategoryEligibilityByCategory(udtEHSClaimSubsidize.SchemeCode, udtEHSClaimSubsidize.SchemeSeq, udtEHSClaimSubsidize.SubsidizeCode, strCategoryCode, udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, dtmServiceDate, udtEHSPersonalInfo.Gender)
                    'End If

                    udtEligibilityRuleResult = Me._udtClaimRulesBLL.CheckCategoryEligibilityByCategory(udtEHSClaimSubsidize.SchemeCode, udtEHSClaimSubsidize.SchemeSeq, udtEHSClaimSubsidize.SubsidizeCode, strCategoryCode, udtEHSPersonalInfo, dtmServiceDate)
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
                End If
            Next

            If Not udtEligibilityRuleResult.IsEligible Then
                strMsgCode = "00106"
            End If

            If Not strMsgCode.Equals(String.Empty) Then
                Return New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                Return Nothing
            End If

        End Function

        Public Function CheckEligibilityForEnterClaim(ByVal udtSchemeClaimWithSubsidize As SchemeClaimModel, ByVal dtmServiceDate As Date, _
                    ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, ByVal udtTranBenefitList As TransactionDetailModelCollection, ByRef udtEligibilityRuleResult As ClaimRulesBLL.EligibleResult) As Common.ComObject.SystemMessage

            Dim strFunctCode As String = "990000"
            Dim strSeverity As String = "E"
            Dim strMsgCode As String = String.Empty

            ' In Enter Claim Detail, service date can be change, use the servicedate for validation
            ' -------------------------------------------------------------------------------
            ' 1. Check Active SchemeClaim
            ' -------------------------------------------------------------------------------
            Dim udtServiceSchemeClaimModel As SchemeClaimModel = Nothing
            strMsgCode = Me._udtClaimRulesBLL.CheckSchemeClaimModelByServiceDate(dtmServiceDate, udtSchemeClaimWithSubsidize, udtServiceSchemeClaimModel)

            ' -------------------------------------------------------------------------------
            ' 2. Retrieve Benifit for Exception Handling & Claim Rule Checking
            ' -------------------------------------------------------------------------------
            If strMsgCode = "" Then

                Dim udtEHSTransactionBLL As New EHSTransactionBLL()

                If udtTranBenefitList Is Nothing Then
                    ' CRE20-0022 (Immu record) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                    udtTranBenefitList = udtEHSTransactionBLL.getTransactionDetailBenefit(udtEHSPersonalInfo.DocCode, _
                                                                                          udtEHSPersonalInfo.IdentityNum, _
                                                                                          EHSTransactionBLL.Source.GetFromDB)
                    ' CRE20-0022 (Immu record) [End][Chris YIM]
                End If


                ' -------------------------------------------------------------------------------
                ' 3. Check Eligibility Rule for Prompt Message
                ' -------------------------------------------------------------------------------

                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                'If udtEHSPersonalInfo.DocCode = DocTypeModel.DocTypeCode.EC AndAlso udtEHSPersonalInfo.ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                '    udtEligibilityRuleResult = Me._udtClaimRulesBLL.CheckEligibilityAny(udtServiceSchemeClaimModel, udtEHSPersonalInfo.ECDateOfRegistration.Value.AddYears(-udtEHSPersonalInfo.ECAge.Value), _
                '        "Y", dtmServiceDate, udtEHSPersonalInfo.Gender, udtTranBenefitList)
                'Else
                'udtEligibilityRuleResult = Me._udtClaimRulesBLL.CheckEligibilityAny(udtServiceSchemeClaimModel, udtEHSPersonalInfo.DOB, _
                '    udtEHSPersonalInfo.ExactDOB, dtmServiceDate, udtEHSPersonalInfo.Gender, udtTranBenefitList)
                'End If
                udtEligibilityRuleResult = Me._udtClaimRulesBLL.CheckEligibilityAny(udtServiceSchemeClaimModel, udtEHSPersonalInfo, _
                                                                                    dtmServiceDate, udtTranBenefitList)
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
            End If


            If strMsgCode = "" AndAlso Not udtEligibilityRuleResult.IsEligible Then
                strMsgCode = "00106"
            End If

            If Not strMsgCode.Equals(String.Empty) Then
                Return New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                Return Nothing
            End If

        End Function

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Function CheckEligibilityForIVRSEnterClaim(ByVal udtSchemeClaimWithSubsidize As SchemeClaimModel, ByVal dtmServiceDate As Date, _
                    ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, ByVal udtTranBenefitList As TransactionDetailModelCollection, ByRef udtEligibilityRuleResult As ClaimRulesBLL.EligibleResult) As Common.ComObject.SystemMessage

            Dim strFunctCode As String = "990000"
            Dim strSeverity As String = "E"
            Dim strMsgCode As String = String.Empty

            ' In Enter Claim Detail, service date can be change, use the servicedate for validation
            ' -------------------------------------------------------------------------------
            ' 1. Check Active SchemeClaim
            ' -------------------------------------------------------------------------------
            Dim udtServiceSchemeClaimModel As SchemeClaimModel = Nothing
            strMsgCode = Me._udtClaimRulesBLL.CheckSchemeClaimModelByServiceDate(dtmServiceDate, udtSchemeClaimWithSubsidize, udtServiceSchemeClaimModel)

            ' -------------------------------------------------------------------------------
            ' 2. Retrieve Benifit for Exception Handling & Claim Rule Checking
            ' -------------------------------------------------------------------------------
            If strMsgCode = "" Then

                Dim udtEHSTransactionBLL As New EHSTransactionBLL()

                If udtTranBenefitList Is Nothing Then
                    ' CRE20-0022 (Immu record) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                    udtTranBenefitList = udtEHSTransactionBLL.getTransactionDetailBenefitForIVRS(udtEHSPersonalInfo.DocCode, _
                                                                                                udtEHSPersonalInfo.IdentityNum, _
                                                                                                EHSTransactionBLL.Source.GetFromDB)
                    ' CRE20-0022 (Immu record) [End][Chris YIM]
                End If


                ' -------------------------------------------------------------------------------
                ' 3. Check Eligibility Rule for Prompt Message
                ' -------------------------------------------------------------------------------
                udtEligibilityRuleResult = Me._udtClaimRulesBLL.CheckEligibilityAny(udtServiceSchemeClaimModel, udtEHSPersonalInfo, _
                                                                                    dtmServiceDate, udtTranBenefitList)

            End If

            If strMsgCode = "" AndAlso Not udtEligibilityRuleResult.IsEligible Then
                strMsgCode = "00106"
            End If

            If Not strMsgCode.Equals(String.Empty) Then
                Return New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                Return Nothing
            End If

        End Function
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        ' Modify to able to checking with multiple scheme seq subsidy

        Public Function CheckClaimRuleForEnterClaim(ByVal dtmServiceDate As Date, ByVal udtEHSAccount As EHSAccountModel, ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, _
            ByVal udtEHSClaimVaccineModel As EHSClaimVaccineModel, ByVal udtAllTransactionBenefit As TransactionDetailModelCollection, ByRef udtClaimRuleResult As ClaimRules.ClaimRulesBLL.ClaimRuleResult, _
            ByVal udtInputPicker As InputPickerModel) As Common.ComObject.SystemMessage

            Dim strMsgCode As String = String.Empty
            Dim strFunctCode As String = "990000"
            Dim strSeverity As String = "E"

            Dim lstIntSchemeSeq As New List(Of Integer)
            Dim lstStrSubsidizeCode As New List(Of String)
            Dim lstStrSubsidizeItemCode As New List(Of String)
            Dim lstStrAvailableCode As New List(Of String)

            'CRE16-026 (Add PCV13) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim udtInputVaccineCollection As New InputVaccineModelCollection
            'CRE16-026 (Add PCV13) [End][Chris YIM]

            '------------------------------------------------------
            ' Retrieve and concat the current claiming vaccination
            '------------------------------------------------------
            For Each udtEHSClaimSubsidizeModel As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccineModel.SubsidizeList
                If udtEHSClaimSubsidizeModel.Selected Then
                    If udtEHSClaimSubsidizeModel.SubsidizeDetailList.Count = 1 Then
                        lstIntSchemeSeq.Add(udtEHSClaimSubsidizeModel.SchemeSeq)
                        lstStrSubsidizeCode.Add(udtEHSClaimSubsidizeModel.SubsidizeCode.Trim())
                        lstStrSubsidizeItemCode.Add(udtEHSClaimSubsidizeModel.SubsidizeDetailList(0).SubsidizeItemCode.Trim())
                        lstStrAvailableCode.Add(udtEHSClaimSubsidizeModel.SubsidizeDetailList(0).AvailableItemCode.Trim())

                        udtInputVaccineCollection.Add(New InputVaccineModel(udtEHSClaimSubsidizeModel.SchemeCode, udtEHSClaimSubsidizeModel.SchemeSeq, _
                                                                            udtEHSClaimSubsidizeModel.SubsidizeCode.Trim(), _
                                                                            udtEHSClaimSubsidizeModel.SubsidizeItemCode.Trim(), _
                                                                            udtEHSClaimSubsidizeModel.SubsidizeDetailList(0).AvailableItemCode.Trim(),
                                                                            udtEHSClaimSubsidizeModel.DisplaySeq, udtEHSClaimSubsidizeModel.SubsidizeDisplayCode))
                    Else
                        For Each udtEHSClaimSubsidizeDetailModel As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimSubsidizeModel.SubsidizeDetailList
                            If udtEHSClaimSubsidizeDetailModel.Selected Then
                                lstIntSchemeSeq.Add(udtEHSClaimSubsidizeModel.SchemeSeq)
                                lstStrSubsidizeCode.Add(udtEHSClaimSubsidizeModel.SubsidizeCode.Trim())
                                lstStrSubsidizeItemCode.Add(udtEHSClaimSubsidizeDetailModel.SubsidizeItemCode.Trim())
                                lstStrAvailableCode.Add(udtEHSClaimSubsidizeDetailModel.AvailableItemCode.Trim())

                                udtInputVaccineCollection.Add(New InputVaccineModel(udtEHSClaimSubsidizeModel.SchemeCode, udtEHSClaimSubsidizeModel.SchemeSeq, _
                                                    udtEHSClaimSubsidizeModel.SubsidizeCode.Trim(), _
                                                    udtEHSClaimSubsidizeModel.SubsidizeItemCode.Trim(), _
                                                    udtEHSClaimSubsidizeDetailModel.AvailableItemCode.Trim(),
                                                    udtEHSClaimSubsidizeModel.DisplaySeq, udtEHSClaimSubsidizeModel.SubsidizeDisplayCode))

                                Exit For
                            End If
                        Next
                    End If
                End If
            Next

            'CRE16-026 (Add PCV13) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            udtInputPicker.EHSClaimVaccine = udtInputVaccineCollection
            'CRE16-026 (Add PCV13) [End][Chris YIM]


            Dim blnInvalidScheme As Boolean = False


            '------------------------------------------------------
            ' Check the Newly Selected Transaction Same (Eqv Dose)
            '------------------------------------------------------

            'CRE15-004 (TIV and QIV) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'Only use for function "CheckSameVaccineForNewTran"
            Dim lstStrResSystemMessage As New List(Of String)
            Dim blnHasResSystemMessage As Boolean = False

            'Dim blnSameVaccineDoseError As Boolean = Me._udtClaimRulesBLL.CheckSameVaccineForNewTran(udtEHSClaimVaccineModel.SchemeCode, dtmServiceDate, lstIntSchemeSeq, lstStrSubsidizeCode, lstStrSubsidizeItemCode, lstStrAvailableCode)
            Dim blnSameVaccineDoseError As Boolean = Me._udtClaimRulesBLL.CheckSameVaccineForNewTran(udtEHSClaimVaccineModel.SchemeCode, dtmServiceDate, lstIntSchemeSeq, lstStrSubsidizeCode, lstStrSubsidizeItemCode, lstStrAvailableCode, lstStrResSystemMessage)
            If blnSameVaccineDoseError Then
                strMsgCode = "00242"

                If lstStrResSystemMessage.Count > 0 Then
                    blnHasResSystemMessage = True
                End If
            End If
            'CRE15-004 (TIV and QIV) [End][Chris YIM]

            Dim lstClaimResultNewTran As List(Of ClaimRulesBLL.ClaimRuleResult) = Nothing
            Dim lstClaimResult As List(Of ClaimRulesBLL.ClaimRuleResult) = Nothing

            '------------------------------------------------------
            ' Check the Newly Selected Transaction against the Newly Selected Transaction
            '------------------------------------------------------
            If strMsgCode = "" Then
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                lstClaimResultNewTran = Me._udtClaimRulesBLL.CheckClaimRuleForNewTran(udtEHSPersonalInfo, _
                    dtmServiceDate, udtEHSClaimVaccineModel.SchemeCode, lstIntSchemeSeq, lstStrSubsidizeCode, _
                    lstStrSubsidizeItemCode, lstStrAvailableCode, blnInvalidScheme)
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
            End If

            If strMsgCode = "" Then
                If blnInvalidScheme Then
                    strMsgCode = "00105"
                End If
            End If

            '------------------------------------------------------
            ' Check available subsidize for claim
            '------------------------------------------------------
            If strMsgCode = "" Then
                Dim udtSchemeClaim As Scheme.SchemeClaimModel = Nothing '(New Scheme.SchemeClaimBLL).getAllEffectiveSchemeClaim_WithSubsidizeGroup(dtmServiceDate).FilterKey(udtEHSClaimVaccineModel.SchemeCode, udtEHSClaimVaccineModel.SchemeSeq)
                For i As Integer = 0 To lstStrSubsidizeCode.Count - 1

                    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                    ' Remove SchemeSeq
                    'udtSchemeClaim = (New Scheme.SchemeClaimBLL).getAllEffectiveSchemeClaim_WithSubsidizeGroup(dtmServiceDate).FilterKey(udtEHSClaimVaccineModel.SchemeCode, lstIntSchemeSeq(i))
                    udtSchemeClaim = (New Scheme.SchemeClaimBLL).getAllEffectiveSchemeClaim_WithSubsidizeGroup(dtmServiceDate).Filter(udtEHSClaimVaccineModel.SchemeCode)
                    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

                    If Not CheckVaccineAvailableBenefitBySubsidizeForEnterClaim(udtSchemeClaim, _
                                                                                lstIntSchemeSeq(i), _
                                                                                lstStrSubsidizeCode(i), _
                                                                                udtEHSPersonalInfo, _
                                                                                dtmServiceDate, _
                                                                                udtAllTransactionBenefit, _
                                                                                udtInputPicker
                                                                                ) Then
                        ' No available subsidize
                        strMsgCode = "00255"
                        Exit For
                    End If
                Next
            End If

            '------------------------------------------------------
            ' Check the Newly Selected Transaction against the database Transaction
            '------------------------------------------------------
            If strMsgCode = "" Then
                ' Get All vaccination transaction (Benefit)
                Dim udtAllTransactionVaccinte As EHSTransaction.TransactionDetailVaccineModelCollection = udtAllTransactionBenefit

                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                lstClaimResult = Me._udtClaimRulesBLL.CheckClaimRuleForClaim(udtEHSPersonalInfo, dtmServiceDate, _
                    udtEHSClaimVaccineModel.SchemeCode, lstIntSchemeSeq, lstStrSubsidizeCode, lstStrSubsidizeItemCode, lstStrAvailableCode, _
                    udtAllTransactionVaccinte, blnInvalidScheme, udtInputPicker)
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                strMsgCode = CustomHandleSystemMessage(lstClaimResult, udtInputVaccineCollection, lstStrResSystemMessage, blnHasResSystemMessage)

            End If

            If strMsgCode = "" Then
                If blnInvalidScheme Then
                    strMsgCode = "00105"
                End If
            End If

            ' -------------------------------------
            ' Check with Block Case
            ' -------------------------------------
            If strMsgCode = "" Then
                For Each udtClaimResult As ClaimRulesBLL.ClaimRuleResult In lstClaimResultNewTran
                    If udtClaimResult.IsBlock Then

                        If Not udtClaimResult.RelatedClaimRule Is Nothing Then
                            strFunctCode = udtClaimResult.RelatedClaimRule.FunctionCode
                            strSeverity = udtClaimResult.RelatedClaimRule.SeverityCode
                            strMsgCode = udtClaimResult.RelatedClaimRule.MessageCode
                            Exit For
                        End If
                    End If
                Next
            End If

            ' -------------------------------------
            ' Check with Block Case
            ' -------------------------------------
            If strMsgCode = "" Then
                For Each udtClaimResult As ClaimRulesBLL.ClaimRuleResult In lstClaimResult
                    If udtClaimResult.IsBlock Then

                        If Not udtClaimResult.RelatedClaimRule Is Nothing Then
                            strFunctCode = udtClaimResult.RelatedClaimRule.FunctionCode
                            strSeverity = udtClaimResult.RelatedClaimRule.SeverityCode
                            strMsgCode = udtClaimResult.RelatedClaimRule.MessageCode

                            ' CRE20-0022 (Immu record) [Start][Chris YIM]
                            ' ---------------------------------------------------------------------------------------------------------
                            udtClaimRuleResult = udtClaimResult
                            ' CRE20-0022 (Immu record) [End][Chris YIM]

                            Exit For
                        End If
                    End If
                Next
            End If

            ' -------------------------------------
            ' Check with Warning Case
            ' -------------------------------------
            If strMsgCode = "" Then
                For Each udtClaimResult As ClaimRulesBLL.ClaimRuleResult In lstClaimResultNewTran
                    If Not udtClaimResult.IsBlock AndAlso (udtClaimResult.HandleMethod = ClaimRulesBLL.HandleMethodENum.Declaration OrElse udtClaimResult.HandleMethod = ClaimRulesBLL.HandleMethodENum.Warning) Then
                        udtClaimRuleResult = udtClaimResult
                        Exit For
                    End If
                Next
            End If

            ' -------------------------------------
            ' Check with Warning Case
            ' -------------------------------------
            If strMsgCode = "" Then
                For Each udtClaimResult As ClaimRulesBLL.ClaimRuleResult In lstClaimResult
                    If Not udtClaimResult.IsBlock AndAlso (udtClaimResult.HandleMethod = ClaimRulesBLL.HandleMethodENum.Declaration OrElse udtClaimResult.HandleMethod = ClaimRulesBLL.HandleMethodENum.Warning) Then
                        udtClaimRuleResult = udtClaimResult
                        Exit For
                    End If
                Next
            End If

            If Not strMsgCode.Equals(String.Empty) Then
                'CRE15-004 (TIV and QIV) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Return New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
                Dim sm As SystemMessage = New SystemMessage(strFunctCode, strSeverity, strMsgCode)
                'Dim strRes As String

                If blnHasResSystemMessage Then
                    'strRes = sm.ConcatMessage(lstStrResSystemMessage, Thread.CurrentThread.CurrentUICulture.Name.ToLower)
                    For idx As Integer = 0 To lstStrResSystemMessage.Count - 1
                        sm.AddReplaceMessage("%s" + (idx + 1).ToString, lstStrResSystemMessage(idx))
                    Next
                End If

                Return sm
                'CRE15-004 (TIV and QIV) [End][Chris YIM]
            Else
                Return Nothing
            End If
        End Function
        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

        Public Function CheckClaimRuleForEnterClaimCOVID19(ByVal dtmServiceDate As Date, ByVal udtEHSAccount As EHSAccountModel, ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, _
            ByVal udtEHSClaimVaccineModel As EHSClaimVaccineModel, ByVal udtAllTransactionBenefit As TransactionDetailModelCollection, ByRef udtClaimRuleResultList As List(Of ClaimRulesBLL.ClaimRuleResult), _
            ByVal udtInputPicker As InputPickerModel) As Common.ComObject.SystemMessage

            Dim strMsgCode As String = String.Empty
            Dim strFunctCode As String = "990000"
            Dim strSeverity As String = "E"

            Dim lstIntSchemeSeq As New List(Of Integer)
            Dim lstStrSubsidizeCode As New List(Of String)
            Dim lstStrSubsidizeItemCode As New List(Of String)
            Dim lstStrAvailableCode As New List(Of String)

            'CRE16-026 (Add PCV13) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim udtInputVaccineCollection As New InputVaccineModelCollection
            'CRE16-026 (Add PCV13) [End][Chris YIM]

            '------------------------------------------------------
            ' Retrieve and concat the current claiming vaccination
            '------------------------------------------------------
            For Each udtEHSClaimSubsidizeModel As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccineModel.SubsidizeList
                If udtEHSClaimSubsidizeModel.Selected Then
                    If udtEHSClaimSubsidizeModel.SubsidizeDetailList.Count = 1 Then
                        lstIntSchemeSeq.Add(udtEHSClaimSubsidizeModel.SchemeSeq)
                        lstStrSubsidizeCode.Add(udtEHSClaimSubsidizeModel.SubsidizeCode.Trim())
                        lstStrSubsidizeItemCode.Add(udtEHSClaimSubsidizeModel.SubsidizeDetailList(0).SubsidizeItemCode.Trim())
                        lstStrAvailableCode.Add(udtEHSClaimSubsidizeModel.SubsidizeDetailList(0).AvailableItemCode.Trim())

                        udtInputVaccineCollection.Add(New InputVaccineModel(udtEHSClaimSubsidizeModel.SchemeCode, udtEHSClaimSubsidizeModel.SchemeSeq, _
                                                                            udtEHSClaimSubsidizeModel.SubsidizeCode.Trim(), _
                                                                            udtEHSClaimSubsidizeModel.SubsidizeItemCode.Trim(), _
                                                                            udtEHSClaimSubsidizeModel.SubsidizeDetailList(0).AvailableItemCode.Trim(),
                                                                            udtEHSClaimSubsidizeModel.DisplaySeq, udtEHSClaimSubsidizeModel.SubsidizeDisplayCode))
                    Else
                        For Each udtEHSClaimSubsidizeDetailModel As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimSubsidizeModel.SubsidizeDetailList
                            If udtEHSClaimSubsidizeDetailModel.Selected Then
                                lstIntSchemeSeq.Add(udtEHSClaimSubsidizeModel.SchemeSeq)
                                lstStrSubsidizeCode.Add(udtEHSClaimSubsidizeModel.SubsidizeCode.Trim())
                                lstStrSubsidizeItemCode.Add(udtEHSClaimSubsidizeDetailModel.SubsidizeItemCode.Trim())
                                lstStrAvailableCode.Add(udtEHSClaimSubsidizeDetailModel.AvailableItemCode.Trim())

                                udtInputVaccineCollection.Add(New InputVaccineModel(udtEHSClaimSubsidizeModel.SchemeCode, udtEHSClaimSubsidizeModel.SchemeSeq, _
                                                    udtEHSClaimSubsidizeModel.SubsidizeCode.Trim(), _
                                                    udtEHSClaimSubsidizeModel.SubsidizeItemCode.Trim(), _
                                                    udtEHSClaimSubsidizeDetailModel.AvailableItemCode.Trim(),
                                                    udtEHSClaimSubsidizeModel.DisplaySeq, udtEHSClaimSubsidizeModel.SubsidizeDisplayCode))

                                Exit For
                            End If
                        Next
                    End If
                End If
            Next

            'CRE16-026 (Add PCV13) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            udtInputPicker.EHSClaimVaccine = udtInputVaccineCollection
            'CRE16-026 (Add PCV13) [End][Chris YIM]


            Dim blnInvalidScheme As Boolean = False


            '------------------------------------------------------
            ' Check the Newly Selected Transaction Same (Eqv Dose)
            '------------------------------------------------------

            'CRE15-004 (TIV and QIV) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'Only use for function "CheckSameVaccineForNewTran"
            Dim lstStrResSystemMessage As New List(Of String)
            Dim blnHasResSystemMessage As Boolean = False

            'Dim blnSameVaccineDoseError As Boolean = Me._udtClaimRulesBLL.CheckSameVaccineForNewTran(udtEHSClaimVaccineModel.SchemeCode, dtmServiceDate, lstIntSchemeSeq, lstStrSubsidizeCode, lstStrSubsidizeItemCode, lstStrAvailableCode)
            Dim blnSameVaccineDoseError As Boolean = Me._udtClaimRulesBLL.CheckSameVaccineForNewTran(udtEHSClaimVaccineModel.SchemeCode, dtmServiceDate, lstIntSchemeSeq, lstStrSubsidizeCode, lstStrSubsidizeItemCode, lstStrAvailableCode, lstStrResSystemMessage)
            If blnSameVaccineDoseError Then
                strMsgCode = "00242"

                If lstStrResSystemMessage.Count > 0 Then
                    blnHasResSystemMessage = True
                End If
            End If
            'CRE15-004 (TIV and QIV) [End][Chris YIM]

            Dim lstClaimResultNewTran As List(Of ClaimRulesBLL.ClaimRuleResult) = Nothing
            Dim lstClaimResult As List(Of ClaimRulesBLL.ClaimRuleResult) = Nothing

            '------------------------------------------------------
            ' Check the Newly Selected Transaction against the Newly Selected Transaction
            '------------------------------------------------------
            If strMsgCode = "" Then
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                lstClaimResultNewTran = Me._udtClaimRulesBLL.CheckClaimRuleForNewTran(udtEHSPersonalInfo, _
                    dtmServiceDate, udtEHSClaimVaccineModel.SchemeCode, lstIntSchemeSeq, lstStrSubsidizeCode, _
                    lstStrSubsidizeItemCode, lstStrAvailableCode, blnInvalidScheme)
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
            End If

            If strMsgCode = "" Then
                If blnInvalidScheme Then
                    strMsgCode = "00105"
                End If
            End If

            '------------------------------------------------------
            ' Check available subsidize for claim
            '------------------------------------------------------
            If strMsgCode = "" Then
                Dim udtSchemeClaim As Scheme.SchemeClaimModel = Nothing '(New Scheme.SchemeClaimBLL).getAllEffectiveSchemeClaim_WithSubsidizeGroup(dtmServiceDate).FilterKey(udtEHSClaimVaccineModel.SchemeCode, udtEHSClaimVaccineModel.SchemeSeq)
                For i As Integer = 0 To lstStrSubsidizeCode.Count - 1

                    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                    ' Remove SchemeSeq
                    'udtSchemeClaim = (New Scheme.SchemeClaimBLL).getAllEffectiveSchemeClaim_WithSubsidizeGroup(dtmServiceDate).FilterKey(udtEHSClaimVaccineModel.SchemeCode, lstIntSchemeSeq(i))
                    udtSchemeClaim = (New Scheme.SchemeClaimBLL).getAllEffectiveSchemeClaim_WithSubsidizeGroup(dtmServiceDate).Filter(udtEHSClaimVaccineModel.SchemeCode)
                    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

                    If Not CheckVaccineAvailableBenefitBySubsidizeForEnterClaim(udtSchemeClaim, _
                                                                                lstIntSchemeSeq(i), _
                                                                                lstStrSubsidizeCode(i), _
                                                                                udtEHSPersonalInfo, _
                                                                                dtmServiceDate, _
                                                                                udtAllTransactionBenefit, _
                                                                                udtInputPicker
                                                                                ) Then
                        ' No available subsidize
                        strMsgCode = "00255"
                        Exit For
                    End If
                Next
            End If

            '------------------------------------------------------
            ' Check the Newly Selected Transaction against the database Transaction
            '------------------------------------------------------
            If strMsgCode = "" Then
                ' Get All vaccination transaction (Benefit)
                Dim udtAllTransactionVaccinte As EHSTransaction.TransactionDetailVaccineModelCollection = udtAllTransactionBenefit

                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                lstClaimResult = Me._udtClaimRulesBLL.CheckClaimRuleForClaim(udtEHSPersonalInfo, dtmServiceDate, _
                    udtEHSClaimVaccineModel.SchemeCode, lstIntSchemeSeq, lstStrSubsidizeCode, lstStrSubsidizeItemCode, lstStrAvailableCode, _
                    udtAllTransactionVaccinte, blnInvalidScheme, udtInputPicker)
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                strMsgCode = CustomHandleSystemMessage(lstClaimResult, udtInputVaccineCollection, lstStrResSystemMessage, blnHasResSystemMessage)

            End If

            If strMsgCode = "" Then
                If blnInvalidScheme Then
                    strMsgCode = "00105"
                End If
            End If

            ' -------------------------------------
            ' Check with Block Case
            ' -------------------------------------
            If strMsgCode = "" Then
                For Each udtClaimResult As ClaimRulesBLL.ClaimRuleResult In lstClaimResultNewTran
                    If udtClaimResult.IsBlock Then

                        If Not udtClaimResult.RelatedClaimRule Is Nothing Then
                            strFunctCode = udtClaimResult.RelatedClaimRule.FunctionCode
                            strSeverity = udtClaimResult.RelatedClaimRule.SeverityCode
                            strMsgCode = udtClaimResult.RelatedClaimRule.MessageCode
                            Exit For
                        End If
                    End If
                Next
            End If

            ' -------------------------------------
            ' Check with Block Case
            ' -------------------------------------
            If strMsgCode = "" Then
                Dim udtClaimResultBlock As ClaimRulesBLL.ClaimRuleResult = Nothing
                Dim udtClaimResultBlockPopup As ClaimRulesBLL.ClaimRuleResult = Nothing
                Dim blnError As Boolean = False
                Dim blnErrorPopup As Boolean = False

                For Each udtClaimResult As ClaimRulesBLL.ClaimRuleResult In lstClaimResult
                    If udtClaimResult.IsBlock Then
                        Select Case udtClaimResult.HandleMethod
                            Case ClaimRulesBLL.HandleMethodENum.Block
                                blnError = True
                                If udtClaimResultBlock Is Nothing Then
                                    udtClaimResultBlock = udtClaimResult
                                End If
                            Case ClaimRulesBLL.HandleMethodENum.BlockPopup
                                blnErrorPopup = True
                                If udtClaimResultBlockPopup Is Nothing Then
                                    udtClaimResultBlockPopup = udtClaimResult
                                End If
                        End Select
                    End If
                Next

                If blnErrorPopup Then
                    If udtClaimResultBlockPopup IsNot Nothing Then
                        Dim udtPopupSystemMessageList As List(Of SystemMessage) = TryCast(udtClaimResultBlockPopup.ResultParam("SystemMessage"), List(Of SystemMessage))
                        strFunctCode = udtPopupSystemMessageList(0).FunctionCode
                        strSeverity = udtPopupSystemMessageList(0).SeverityCode
                        strMsgCode = udtPopupSystemMessageList(0).MessageCode

                        udtClaimRuleResultList.Add(udtClaimResultBlockPopup)
                    End If
                Else
                    If udtClaimResultBlock IsNot Nothing Then
                        strFunctCode = udtClaimResultBlock.RelatedClaimRule.FunctionCode
                        strSeverity = udtClaimResultBlock.RelatedClaimRule.SeverityCode
                        strMsgCode = udtClaimResultBlock.RelatedClaimRule.MessageCode

                        udtClaimRuleResultList.Add(udtClaimResultBlock)
                    End If
                End If

            End If

            ' -------------------------------------
            ' Check with Warning Case
            ' -------------------------------------
            If strMsgCode = "" Then
                For Each udtClaimResult As ClaimRulesBLL.ClaimRuleResult In lstClaimResultNewTran
                    If Not udtClaimResult.IsBlock AndAlso (udtClaimResult.HandleMethod = ClaimRulesBLL.HandleMethodENum.Declaration OrElse udtClaimResult.HandleMethod = ClaimRulesBLL.HandleMethodENum.Warning) Then
                        'udtClaimRuleResult = udtClaimResult
                        udtClaimRuleResultList.Add(udtClaimResult)
                        Exit For
                    End If
                Next
            End If

            ' -------------------------------------
            ' Check with Warning Case
            ' -------------------------------------
            If strMsgCode = "" Then
                For Each udtClaimResult As ClaimRulesBLL.ClaimRuleResult In lstClaimResult
                    If Not udtClaimResult.IsBlock AndAlso _
                        (udtClaimResult.HandleMethod = ClaimRulesBLL.HandleMethodENum.Declaration OrElse _
                         udtClaimResult.HandleMethod = ClaimRulesBLL.HandleMethodENum.Warning OrElse _
                         udtClaimResult.HandleMethod = ClaimRulesBLL.HandleMethodENum.WarningReason) Then
                        'udtClaimRuleResult = udtClaimResult
                        udtClaimRuleResultList = lstClaimResult
                        Exit For
                    End If
                Next
            End If

            If Not strMsgCode.Equals(String.Empty) Then
                'CRE15-004 (TIV and QIV) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Return New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
                Dim sm As SystemMessage = New SystemMessage(strFunctCode, strSeverity, strMsgCode)
                'Dim strRes As String

                If blnHasResSystemMessage Then
                    'strRes = sm.ConcatMessage(lstStrResSystemMessage, Thread.CurrentThread.CurrentUICulture.Name.ToLower)
                    For idx As Integer = 0 To lstStrResSystemMessage.Count - 1
                        sm.AddReplaceMessage("%s" + (idx + 1).ToString, lstStrResSystemMessage(idx))
                    Next
                End If

                Return sm
                'CRE15-004 (TIV and QIV) [End][Chris YIM]
            Else
                Return Nothing
            End If
        End Function

        Public Function CheckExceedDocumentLimitForEnterClaim(ByVal strSchemeCode As String, ByVal dtmServiceDate As Date, ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel) As Common.ComObject.SystemMessage

            ' -------------------------------------------------------------------------------
            ' Check Document Limit
            ' -------------------------------------------------------------------------------
            Dim strFunctCode As String = "990000"
            Dim strSeverity As String = "E"
            Dim strMsgCode As String = String.Empty

            If strMsgCode.Trim() = "" Then
                If Me._udtClaimRulesBLL.CheckExceedDocumentLimit(strSchemeCode, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, dtmServiceDate) Then
                    If udtEHSPersonalInfo.DocCode = DocTypeModel.DocTypeCode.EC Then
                        strMsgCode = "00185"
                    Else
                        strMsgCode = "00213"
                    End If
                End If
            End If

            If Not strMsgCode.Equals(String.Empty) Then '
                Return New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                Return Nothing
            End If

        End Function

        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        ' Add SchemeSeq to subsidy level
        Private Function CheckVaccineAvailableBenefitBySubsidizeForEnterClaim(ByVal udtSchemeClaim As SchemeClaimModel, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String, _
                                                                            ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, _
                                                                            ByVal dtmServiceDate As DateTime, _
                                                                            ByVal udtEHSTransactionBenefitList As TransactionDetailModelCollection, _
                                                                            ByVal udtInputPicker As InputPickerModel) As Boolean 'Common.ComObject.SystemMessage

            Dim udtSubsidizeGroupClaim As Scheme.SubsidizeGroupClaimModel = udtSchemeClaim.SubsidizeGroupClaimList.Filter(udtSchemeClaim.SchemeCode, _
                                                                                                                        intSchemeSeq, _
                                                                                                                        strSubsidizeCode)

            Dim udtSubsidizeItemDetailList As SubsidizeItemDetailsModelCollection = Me._udtSchemeDetailBLL.getSubsidizeItemDetails(udtSubsidizeGroupClaim.SubsidizeItemCode)

            If Not (New ClaimRules.ClaimRulesBLL).chkVaccineAvailableBenefitBySubsidize(udtSubsidizeGroupClaim, udtSubsidizeItemDetailList, _
                                                                            udtEHSTransactionBenefitList, _
                                                                            udtEHSPersonalInfo, _
                                                                            dtmServiceDate, _
                                                                            Nothing, udtInputPicker) Then

                Return False
                'Return New Common.ComObject.SystemMessage("990000", "E", "00255")
            Else
                Return True
                'Return Nothing
            End If
        End Function
        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]
#End Region

#Region "Class"

        <Serializable()> _
        Public Class SearchAccountStatus
            Public NotMatchAccountExist As Boolean
            Public ExceedDocTypeLimit As Boolean
            Public TempAccountNotMatchDOBFound As Boolean
            Public TempAccountInputDetailDiffFound As Boolean
            Public OnlyInvalidAccountFound As Boolean

            Public Sub New()
                NotMatchAccountExist = False
                ExceedDocTypeLimit = False
                TempAccountNotMatchDOBFound = False
                TempAccountInputDetailDiffFound = False
                OnlyInvalidAccountFound = False
            End Sub

        End Class

#End Region

#Region "Search EHSAccount Function For Screen"

        ''' <summary>
        ''' Search EHS Account In EHSClaimSearch (For EC Case Age on Date of Registration)
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="strDocCode"></param>
        ''' <param name="strIdentityNum"></param>
        ''' <param name="intAge"></param>
        ''' <param name="dtmDOR"></param>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="udtEligibleResult"></param>
        ''' <param name="blnNoMatchRecordFound"></param>
        ''' <param name="blnExceedDocTypeLimit"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SearchEHSAccount(ByVal strSchemeCode As String, ByVal strDocCode As String, ByVal strIdentityNum As String, _
            ByVal intAge As Integer, ByVal dtmDOR As Date, ByRef udtEHSAccount As EHSAccountModel, ByRef udtEligibleResult As ClaimRulesBLL.EligibleResult, _
            ByRef udtSearchAccountStatus As EHSClaimBLL.SearchAccountStatus, ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, _
            ByVal strFunctionCode As String, ByVal enumClaimMode As ClaimMode) As SystemMessage

            ' -------------------------------------------------------------------------------
            ' Init
            ' -------------------------------------------------------------------------------
            strIdentityNum = Me._udtFormater.formatDocumentIdentityNumber(strDocCode, strIdentityNum)

            Dim dtmCurrentDateTime As DateTime = Me._udtCommonGenFunc.GetSystemDateTime()
            Dim dtmCurrentDate As Date = dtmCurrentDateTime.Date

            ' Indicate the DocCode of EHS Account of Database
            Dim strSearchDocCode As String = String.Empty

            Dim strFunctCode As String = "990000"
            Dim strSeverity As String = "E"
            Dim strMsgCode As String = String.Empty

            Dim sm As Common.ComObject.SystemMessage = Nothing
            Dim udtCurrEHSPersonalInfoModel As EHSAccountModel.EHSPersonalInformationModel = Nothing

            ' -------------------------------------------------------------------------------
            ' 1. Check Active SchemeClaim
            ' -------------------------------------------------------------------------------
            Dim udtSchemeClaimBLL As New SchemeClaimBLL()
            Dim udtSchemeClaimModel As SchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(strSchemeCode, dtmCurrentDateTime)

            ' CRE20-0023 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            udtSchemeClaimModel.SubsidizeGroupClaimList = udtSchemeClaimBLL.FilterSubsidizeGroupClaim(udtSchemeClaimModel, enumClaimMode)
            ' CRE20-0023 (Immu record) [End][Chris YIM]

            If udtSchemeClaimModel Is Nothing OrElse udtSchemeClaimModel.SubsidizeGroupClaimList.Count = 0 Then
                strMsgCode = "00105"
            End If

            ' -------------------------------------------------------------------------------
            ' 2. Service Date Back
            ' -------------------------------------------------------------------------------
            Dim dtmServiceDateBack As Date = dtmCurrentDate
            Dim strAllowDateBack As String = String.Empty
            Dim strDummy As String = String.Empty
            Dim strClaimDayLimit As String = String.Empty
            Dim strMinDate As String = String.Empty

            Dim udtGenFunct As New Common.ComFunction.GeneralFunction()
            udtGenFunct.getSystemParameter("DateBackClaimAllow", strAllowDateBack, strDummy)

            If strAllowDateBack = String.Empty Then
                strAllowDateBack = "N"
            End If

            If strAllowDateBack = "Y" Then
                udtGenFunct.getSystemParameter("DateBackClaimDayLimit", strClaimDayLimit, strDummy)
                udtGenFunct.getSystemParameter("DateBackClaimMinDate", strMinDate, strDummy)
                Dim intDayLimit As Integer = CInt(strClaimDayLimit)
                Dim dtmMinDate As DateTime = Convert.ToDateTime(strMinDate)

                dtmServiceDateBack = dtmCurrentDate.AddDays(-intDayLimit + 1)
                If dtmServiceDateBack < dtmMinDate Then dtmServiceDateBack = dtmMinDate
                If dtmServiceDateBack < udtSchemeClaimModel.ClaimPeriodFrom Then dtmServiceDateBack = udtSchemeClaimModel.ClaimPeriodFrom
            Else
                dtmServiceDateBack = dtmCurrentDate
            End If

            ' -------------------------------------------------------------------------------
            ' CRE11-007
            ' 3. Recipient Deceased
            ' Check Active Death Record
            ' If dead, return "(document id name) is invalid"
            ' -------------------------------------------------------------------------------
            If strMsgCode.Trim() = "" Then
                If (New DocTypeBLL).getDocTypeByAvailable(DocTypeBLL.EnumAvailable.DeathRecordAvailable).Filter(strDocCode) IsNot Nothing Then
                    If (New eHealthAccountDeathRecord.eHealthAccountDeathRecordBLL).GetDeathRecordEntry(strIdentityNum).IsDead() Then
                        strMsgCode = (New Common.Validation.Validator).GetMessageForIdentityNoIsNoLongerValid(strDocCode).MessageCode
                    End If
                End If
            End If

            ' -------------------------------------------------------------------------------
            ' 4. Block EHCP make voucher claim for themselves
            ' Check Inputted Doc. No. with SP's HKID
            ' If matches, return "HCPs are not allowed to make claims for himself/herself"
            ' -------------------------------------------------------------------------------

            'CRE16-017 (Block EHCP make voucher claim for themselves) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            If strMsgCode.Trim() = "" Then
                Dim udtSP As ServiceProviderModel = UserACBLL.GetServiceProvider()
                strMsgCode = Me._udtClaimRulesBLL.IsSPClaimForThemselves(udtSP.HKID, strDocCode, strIdentityNum, udtSchemeClaimModel.AvailableHCSPSubPlatform)
            End If
            'CRE16-017 (Block EHCP make voucher claim for themselves) [End][Chris YIM]

            ' -------------------------------------------------------------------------------
            ' 5. Check HKIC VS EC
            ' -------------------------------------------------------------------------------
            If strMsgCode.Trim() = "" Then
                strMsgCode = Me._udtClaimRulesBLL.chkEHSAccountHKICVsEC(EHSAccountModel.SysAccountSource.ValidateAccount, strDocCode, strIdentityNum)
            End If
            If strMsgCode.Trim() = "" Then
                strMsgCode = Me._udtClaimRulesBLL.chkEHSAccountHKICVsEC(EHSAccountModel.SysAccountSource.TemporaryAccount, strDocCode, strIdentityNum)
            End If
            If strMsgCode.Trim() = "" Then
                strMsgCode = Me._udtClaimRulesBLL.chkEHSAccountHKICVsEC(EHSAccountModel.SysAccountSource.SpecialAccount, strDocCode, strIdentityNum)
            End If

            ' -------------------------------------------------------------------------------
            ' 6. Adoption Checking (Check the Adoption Detail when account is searched
            ' -------------------------------------------------------------------------------
            ' Nil For EC Case

            ' -------------------------------------------------------------------------------
            ' 7. Search Validate Account, Check Account Status (DOB Match)
            ' -------------------------------------------------------------------------------            
            Me.SearchValidatedAccount(strDocCode, strIdentityNum, udtEHSAccount, strSearchDocCode)
            If Not udtEHSAccount Is Nothing AndAlso strMsgCode = String.Empty Then
                ' Validate Account Found
                If udtEHSAccount.RecordStatus.Equals(EHSAccountModel.ValidatedAccountRecordStatusClass.Suspended) AndAlso chkSuspendAccountClaimAllow(enumClaimMode) = False Then
                    strMsgCode = "00108"

                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                    ' -----------------------------------------------------------------------------------------
                ElseIf udtEHSAccount.RecordStatus.Equals(EHSAccountModel.ValidatedAccountRecordStatusClass.Terminated) Then
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
                    strMsgCode = "00109"
                ElseIf udtEHSAccount.RecordStatus.Equals(EHSAccountModel.ValidatedAccountRecordStatusClass.Active) OrElse _
                      (udtEHSAccount.RecordStatus.Equals(EHSAccountModel.ValidatedAccountRecordStatusClass.Suspended) AndAlso chkSuspendAccountClaimAllow(enumClaimMode) = True) Then
                    ' Check DOB Match
                    udtCurrEHSPersonalInfoModel = udtEHSAccount.EHSPersonalInformationList.Filter(strSearchDocCode)
                    If Not Me.chkEHSAccountInputDOBMatch(udtCurrEHSPersonalInfoModel, intAge, dtmDOR) Then
                        'DOB Not Match
                        strMsgCode = "00110"
                    End If

                End If
            End If

            ' -------------------------------------------------------------------------------
            ' 8. Search Temporary Account, Check Account Status
            ' -------------------------------------------------------------------------------
            Dim blnOnlyInValidAccountFound As Boolean = False
            Dim blnTempAccountNotMatchDOBFound As Boolean = False

            If enumClaimMode = ClaimMode.COVID19 Then
                'Claim COVID19
                If Not udtEHSAccount Is Nothing AndAlso _
                    (udtEHSAccount.RecordStatus = "A" OrElse (udtEHSAccount.RecordStatus = "S" AndAlso chkSuspendAccountClaimAllow(enumClaimMode) = True)) AndAlso _
                    (udtEHSAccount.SearchDocCode = strDocCode) Then
                    ' Use Validated Account directly
                Else
                    ' CRE20-023-67 (COVID19 - Prefill Personal Info) [Start][Winnie SUEN]
                    '--------------------------------------------------------------------------
                    'udtEHSAccount = Me.ConstructEHSTemporaryVoucherAccount(strIdentityNum, strDocCode, intAge, dtmDOR, strSchemeCode)

                    ' Get latest temp account with COVID19 tx
                    Me.SearchTemporaryAccountWithCOVID19Claim(strDocCode, strIdentityNum, Nothing, Nothing, strSchemeCode, udtEHSAccount,
                                                              udtSearchAccountStatus, intAge, dtmDOR, Nothing, strSearchDocCode, udtEHSPersonalInfo)

                    udtSearchAccountStatus.NotMatchAccountExist = udtSearchAccountStatus.OnlyInvalidAccountFound OrElse udtSearchAccountStatus.TempAccountNotMatchDOBFound
                    ' CRE20-023-67 (COVID19 - Prefill Personal Info) [End][Winnie SUEN]
                End If

            Else
                'Normal claim
                If udtEHSAccount Is Nothing AndAlso strMsgCode = String.Empty Then
                    Me.SearchTemporaryAccount(strDocCode, strIdentityNum, Nothing, Nothing, udtEHSAccount, _
                        udtSearchAccountStatus, intAge, dtmDOR, Nothing, strSearchDocCode, udtEHSPersonalInfo)
                End If

                udtSearchAccountStatus.NotMatchAccountExist = udtSearchAccountStatus.OnlyInvalidAccountFound OrElse udtSearchAccountStatus.TempAccountNotMatchDOBFound
            End If

            ' -------------------------------------------------------------------------------
            ' 9. Create New Account
            ' -------------------------------------------------------------------------------
            If udtEHSAccount Is Nothing And strMsgCode.Trim().Equals(String.Empty) Then
                udtEHSAccount = Me.ConstructEHSTemporaryVoucherAccount(strIdentityNum, strDocCode, intAge, dtmDOR, strSchemeCode)
            End If

            ' [2009Oct22] Performance Tuning
            Dim udtEHSTransactionBLL As New EHSTransactionBLL()
            Dim udtTranBenefitList As TransactionDetailModelCollection = Nothing

            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            ' Transaction Checking will be only apply on Vaccine scheme
            If udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVaccine Then
                ' Retrieve the Transaction Detail (may have mutil SubsidizeCodes)
                udtTranBenefitList = udtEHSTransactionBLL.getTransactionDetailBenefit(strDocCode, strIdentityNum)
            End If
            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

            ' -------------------------------------------------------------------------------
            ' 10. Check Eligible
            ' -------------------------------------------------------------------------------
            If strMsgCode.Trim() = "" Then
                Dim udtSP As ServiceProviderModel = UserACBLL.GetServiceProvider()
                Dim udtSelectedPractice As PracticeDisplayModel = (New SessionHandler).PracticeDisplayGetFromSession(strFunctionCode)
                Dim udtPractice As PracticeModel = Nothing
                If Not IsNothing(udtSelectedPractice) Then
                    udtPractice = udtSP.PracticeList(udtSelectedPractice.PracticeID)
                End If

                ' CRE16-002 Revamp VSS [Start][Lawrence]
                Dim strGender As String = String.Empty

                If Not IsNothing(udtEHSPersonalInfo) Then
                    strGender = udtEHSPersonalInfo.Gender
                End If
                ' CRE16-002 Revamp VSS [End][Lawrence]

                udtEligibleResult = Me._udtClaimRulesBLL.CheckEligibilityFromEHSClaimSearch(udtSchemeClaimModel, strIdentityNum, strDocCode.Trim(), intAge, dtmDOR, dtmCurrentDate, strGender, udtTranBenefitList, udtPractice)
                If Not udtEligibleResult.IsEligible Then
                    strMsgCode = "00106"
                End If
            End If

            ' -------------------------------------------------------------------------------
            ' 11. Check Document Limit
            ' -------------------------------------------------------------------------------
            If strMsgCode.Trim() = "" Then
                udtSearchAccountStatus.ExceedDocTypeLimit = Me._udtClaimRulesBLL.CheckExceedDocumentLimitFromEHSClaimSearch(strSchemeCode, strDocCode, intAge, dtmDOR, dtmCurrentDate)
                If udtSearchAccountStatus.ExceedDocTypeLimit Then
                    strMsgCode = "00185"
                End If
            End If

            ' -------------------------------------------------------------------------------
            ' Return
            ' -------------------------------------------------------------------------------

            If Not udtEHSAccount Is Nothing Then
                ' For UI Setting
                udtEHSAccount.SetSearchDocCode(strDocCode.Trim())
            End If


            If Not strMsgCode.Equals(String.Empty) Then
                Return New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                Return Nothing
            End If

        End Function

        ''' <summary>
        ''' Search EHS Account In EHSClaimSearch
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="strDocCode"></param>
        ''' <param name="strIdentityNum"></param>
        ''' <param name="strDOB"></param>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="udtEligibleResult"></param>
        ''' <param name="udtEHSPersonalInfo">Provide if from Vaccination Record Enquiry</param>
        ''' <param name="strAdoptionPrefixNum"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SearchEHSAccount(ByVal strSchemeCode As String, _
                                         ByVal strDocCode As String, _
                                         ByVal strIdentityNum As String, _
                                         ByVal strDOB As String, _
                                         ByRef udtEHSAccount As EHSAccountModel, _
                                         ByRef udtEligibleResult As ClaimRulesBLL.EligibleResult, _
                                         ByRef udtSearchAccountStatus As SearchAccountStatus, _
                                         ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, _
                                         ByVal strAdoptionPrefixNum As String, _
                                         ByVal strFunctionCode As String, _
                                         ByVal enumClaimMode As ClaimMode) As SystemMessage

            ' -------------------------------------------------------------------------------
            ' Init
            ' -------------------------------------------------------------------------------
            strIdentityNum = Me._udtFormater.formatDocumentIdentityNumber(strDocCode, strIdentityNum)
            Dim dtmCurrentDateTime As Date = Me._udtCommonGenFunc.GetSystemDateTime()
            Dim dtmCurrentDate As Date = dtmCurrentDateTime.Date


            ' Indicate the DocCode of EHS Account of Database
            Dim strSearchDocCode As String = String.Empty
            Dim strFunctCode As String = "990000"
            Dim strSeverity As String = "E"
            Dim strMsgCode As String = String.Empty

            Dim sm As Common.ComObject.SystemMessage = Nothing
            Dim udtCurrEHSPersonalInfoModel As EHSAccountModel.EHSPersonalInformationModel = Nothing

            Dim dtmDOB As Date = Nothing
            Dim strExactDOB As String = Nothing
            Me._udtCommonGenFunc.chkDOBtype(strDOB, dtmDOB, strExactDOB)

            ' -------------------------------------------------------------------------------
            ' 1. Check Active SchemeClaim
            ' -------------------------------------------------------------------------------
            Dim udtSchemeClaimBLL As New SchemeClaimBLL()
            Dim udtSchemeClaimModel As SchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(strSchemeCode, dtmCurrentDateTime)

            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            udtSchemeClaimModel.SubsidizeGroupClaimList = udtSchemeClaimBLL.FilterSubsidizeGroupClaim(udtSchemeClaimModel, enumClaimMode)
            ' CRE20-0022 (Immu record) [End][Chris YIM]

            If udtSchemeClaimModel Is Nothing OrElse udtSchemeClaimModel.SubsidizeGroupClaimList.Count = 0 Then
                strMsgCode = "00105"
            End If

            ' -------------------------------------------------------------------------------
            ' 2. Service Date Back
            ' -------------------------------------------------------------------------------

            Dim dtmServiceDateBack As Date = dtmCurrentDate
            Dim strAllowDateBack As String = String.Empty
            Dim strDummy As String = String.Empty
            Dim strClaimDayLimit As String = String.Empty
            Dim strMinDate As String = String.Empty

            Dim udtGenFunct As New Common.ComFunction.GeneralFunction()
            udtGenFunct.getSystemParameter("DateBackClaimAllow", strAllowDateBack, strDummy)

            If strAllowDateBack = String.Empty Then
                strAllowDateBack = "N"
            End If

            If strAllowDateBack = "Y" Then
                udtGenFunct.getSystemParameter("DateBackClaimDayLimit", strClaimDayLimit, strDummy)
                udtGenFunct.getSystemParameter("DateBackClaimMinDate", strMinDate, strDummy)
                Dim intDayLimit As Integer = CInt(strClaimDayLimit)
                Dim dtmMinDate As DateTime = Convert.ToDateTime(strMinDate)

                dtmServiceDateBack = dtmCurrentDate.AddDays(-intDayLimit)
                If dtmServiceDateBack < dtmMinDate Then dtmServiceDateBack = dtmMinDate
                If dtmServiceDateBack < udtSchemeClaimModel.ClaimPeriodFrom Then dtmServiceDateBack = udtSchemeClaimModel.ClaimPeriodFrom
            Else
                dtmServiceDateBack = dtmCurrentDate
            End If

            ' -------------------------------------------------------------------------------
            ' CRE11-007
            ' 3. Recipient Deceased
            ' Check Active Death Record
            ' If dead, return "(document id name) is invalid"
            ' -------------------------------------------------------------------------------
            If strMsgCode.Trim() = "" Then
                If (New DocTypeBLL).getDocTypeByAvailable(DocTypeBLL.EnumAvailable.DeathRecordAvailable).Filter(strDocCode) IsNot Nothing Then
                    If (New eHealthAccountDeathRecord.eHealthAccountDeathRecordBLL).GetDeathRecordEntry(strIdentityNum).IsDead() Then
                        strMsgCode = (New Common.Validation.Validator).GetMessageForIdentityNoIsNoLongerValid(strDocCode).MessageCode
                    End If
                End If
            End If

            ' -------------------------------------------------------------------------------
            ' 4. Block EHCP make voucher claim for themselves
            ' Check Inputted Doc. No. with SP's HKID
            ' If matches, return "HCPs are not allowed to make claims for himself/herself"
            ' -------------------------------------------------------------------------------

            'CRE16-017 (Block EHCP make voucher claim for themselves) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            If strMsgCode.Trim() = "" Then
                Dim udtSP As ServiceProviderModel = UserACBLL.GetServiceProvider()
                strMsgCode = Me._udtClaimRulesBLL.IsSPClaimForThemselves(udtSP.HKID, strDocCode, strIdentityNum, udtSchemeClaimModel.AvailableHCSPSubPlatform)
            End If
            'CRE16-017 (Block EHCP make voucher claim for themselves) [End][Chris YIM]

            ' -------------------------------------------------------------------------------
            ' 5. Check HKIC VS EC
            ' -------------------------------------------------------------------------------
            If strMsgCode.Trim() = "" Then
                strMsgCode = Me._udtClaimRulesBLL.chkEHSAccountHKICVsEC(EHSAccountModel.SysAccountSource.ValidateAccount, strDocCode, strIdentityNum)
            End If
            If strMsgCode.Trim() = "" Then
                strMsgCode = Me._udtClaimRulesBLL.chkEHSAccountHKICVsEC(EHSAccountModel.SysAccountSource.TemporaryAccount, strDocCode, strIdentityNum)
            End If
            If strMsgCode.Trim() = "" Then
                strMsgCode = Me._udtClaimRulesBLL.chkEHSAccountHKICVsEC(EHSAccountModel.SysAccountSource.SpecialAccount, strDocCode, strIdentityNum)
            End If

            ' -------------------------------------------------------------------------------
            ' 6. Adoption Checking (Check the Adoption Detail when account is searched
            ' -------------------------------------------------------------------------------
            If strMsgCode.Trim() = "" AndAlso strDocCode = DocTypeModel.DocTypeCode.ADOPC Then
                strMsgCode = Me._udtClaimRulesBLL.chkEHSAdoptionCertDetail(strDocCode, strIdentityNum, strAdoptionPrefixNum, String.Empty, EHSAccountModel.SysAccountSource.ValidateAccount)
            End If

            If strMsgCode.Trim() = "" AndAlso strDocCode = DocTypeModel.DocTypeCode.ADOPC Then
                strMsgCode = Me._udtClaimRulesBLL.chkEHSAdoptionCertDetail(strDocCode, strIdentityNum, strAdoptionPrefixNum, String.Empty, EHSAccountModel.SysAccountSource.TemporaryAccount)
            End If

            If strMsgCode.Trim() = "" AndAlso strDocCode = DocTypeModel.DocTypeCode.ADOPC Then
                strMsgCode = Me._udtClaimRulesBLL.chkEHSAdoptionCertDetail(strDocCode, strIdentityNum, strAdoptionPrefixNum, String.Empty, EHSAccountModel.SysAccountSource.SpecialAccount)
            End If

            ' -------------------------------------------------------------------------------
            ' 7. Search Validate Account, Check Account Status (DOB Match)
            ' -------------------------------------------------------------------------------
            Me.SearchValidatedAccount(strDocCode, strIdentityNum, udtEHSAccount, strSearchDocCode)
            If Not udtEHSAccount Is Nothing AndAlso strMsgCode = String.Empty Then
                ' Validate Account Found
                If udtEHSAccount.RecordStatus.Equals(EHSAccountModel.ValidatedAccountRecordStatusClass.Suspended) AndAlso chkSuspendAccountClaimAllow(enumClaimMode) = False Then
                    strMsgCode = "00108"
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                    ' -----------------------------------------------------------------------------------------
                ElseIf udtEHSAccount.RecordStatus.Equals(EHSAccountModel.ValidatedAccountRecordStatusClass.Terminated) Then
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
                    strMsgCode = "00109"
                ElseIf udtEHSAccount.RecordStatus.Equals(EHSAccountModel.ValidatedAccountRecordStatusClass.Active) OrElse _
                       (udtEHSAccount.RecordStatus.Equals(EHSAccountModel.ValidatedAccountRecordStatusClass.Suspended) AndAlso chkSuspendAccountClaimAllow(enumClaimMode) = True) Then
                    ' Check DOB Match
                    udtCurrEHSPersonalInfoModel = udtEHSAccount.EHSPersonalInformationList.Filter(strSearchDocCode)
                    If Not Me.chkEHSAccountInputDOBMatch(udtCurrEHSPersonalInfoModel, dtmDOB, strExactDOB) Then
                        'DOB Not Match
                        Select Case strDocCode
                            Case DocTypeModel.DocTypeCode.ADOPC
                                strMsgCode = "00222"
                            Case DocTypeModel.DocTypeCode.DI, DocTypeModel.DocTypeCode.OW, DocTypeModel.DocTypeCode.TW, DocTypeModel.DocTypeCode.PASS, _
                                DocTypeModel.DocTypeCode.MEP, DocTypeModel.DocTypeCode.TWMTP, DocTypeModel.DocTypeCode.TWPAR, DocTypeModel.DocTypeCode.TWVTD, _
                                DocTypeModel.DocTypeCode.TWNS, DocTypeModel.DocTypeCode.MD, DocTypeModel.DocTypeCode.MP, DocTypeModel.DocTypeCode.TD, _
                                DocTypeModel.DocTypeCode.CEEP, DocTypeModel.DocTypeCode.ISSHK, DocTypeModel.DocTypeCode.ET, DocTypeModel.DocTypeCode.RFNo8, _
                                DocTypeModel.DocTypeCode.DS
                                ' CRE20-0022 (Immu record) [Martin]
                                strMsgCode = "00223"
                            Case DocTypeModel.DocTypeCode.EC, DocTypeModel.DocTypeCode.CCIC, DocTypeModel.DocTypeCode.ROP140 ' CRE20-0022 (Immu record) [Martin]
                                strMsgCode = "00110"
                            Case DocTypeModel.DocTypeCode.HKBC
                                strMsgCode = "00224"
                            Case DocTypeModel.DocTypeCode.HKIC
                                strMsgCode = "00110"
                            Case DocTypeModel.DocTypeCode.ID235B
                                strMsgCode = "00225"
                            Case DocTypeModel.DocTypeCode.REPMT
                                strMsgCode = "00226"
                            Case DocTypeModel.DocTypeCode.VISA
                                strMsgCode = "00227"
                            Case Else
                                strMsgCode = "00110"
                        End Select
                    End If

                End If
            End If

            ' -------------------------------------------------------------------------------
            ' 8. Search Temporary Account, Check Account Status
            ' -------------------------------------------------------------------------------
            If enumClaimMode = ClaimMode.COVID19 Then
                'Claim COVID19

                If Not udtEHSAccount Is Nothing AndAlso _
                    (udtEHSAccount.RecordStatus = "A" OrElse (udtEHSAccount.RecordStatus = "S" AndAlso chkSuspendAccountClaimAllow(enumClaimMode) = True)) AndAlso _
                    (udtEHSAccount.SearchDocCode = strDocCode) Then
                    ' Use Validated Account directly
                Else
                    ' CRE20-023-67 (COVID19 - Prefill Personal Info) [Start][Winnie SUEN]
                    '--------------------------------------------------------------------------
                    'udtEHSAccount = Me.ConstructEHSTemporaryVoucherAccount(strIdentityNum, strDocCode, strExactDOB, dtmDOB, strSchemeCode, strAdoptionPrefixNum, udtEHSPersonalInfo)
                    ' Get latest temp account with COVID19 tx
                    Me.SearchTemporaryAccountWithCOVID19Claim(strDocCode, strIdentityNum, dtmDOB, strExactDOB, strSchemeCode, udtEHSAccount, _
                                                              udtSearchAccountStatus, Nothing, Nothing, strAdoptionPrefixNum, strSearchDocCode, _
                                                              udtEHSPersonalInfo)

                    udtSearchAccountStatus.NotMatchAccountExist = udtSearchAccountStatus.OnlyInvalidAccountFound OrElse udtSearchAccountStatus.TempAccountNotMatchDOBFound
                    ' CRE20-023-67 (COVID19 - Prefill Personal Info) [End][Winnie SUEN]
                End If

            Else
                'Normal claim
                If udtEHSAccount Is Nothing AndAlso strMsgCode = String.Empty Then
                    Me.SearchTemporaryAccount(strDocCode, strIdentityNum, dtmDOB, strExactDOB, udtEHSAccount, _
                        udtSearchAccountStatus, Nothing, Nothing, strAdoptionPrefixNum, strSearchDocCode, _
                        udtEHSPersonalInfo)
                End If

                udtSearchAccountStatus.NotMatchAccountExist = udtSearchAccountStatus.OnlyInvalidAccountFound OrElse udtSearchAccountStatus.TempAccountNotMatchDOBFound
            End If

            ' -------------------------------------------------------------------------------
            ' 9. Create New Account
            ' -------------------------------------------------------------------------------
            If udtEHSAccount Is Nothing And strMsgCode.Trim().Equals(String.Empty) Then
                udtEHSAccount = Me.ConstructEHSTemporaryVoucherAccount(strIdentityNum, strDocCode, strExactDOB, dtmDOB, strSchemeCode, strAdoptionPrefixNum, udtEHSPersonalInfo)
            End If


            ' [2009Oct22] Performance Tuning
            Dim udtEHSTransactionBLL As New EHSTransactionBLL()
            Dim udtTranBenefitList As TransactionDetailModelCollection = Nothing

            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            ' Transaction Checking will be only apply on Vaccine scheme
            If udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVaccine Then
                ' Retrieve the Transaction Detail (may have mutil SubsidizeCodes)
                udtTranBenefitList = udtEHSTransactionBLL.getTransactionDetailBenefit(strDocCode, strIdentityNum)
            End If
            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

            ' -------------------------------------------------------------------------------
            ' 10. Check Eligible
            ' -------------------------------------------------------------------------------
            If strMsgCode.Trim() = "" Then
                Dim udtSP As ServiceProviderModel = UserACBLL.GetServiceProvider()
                Dim udtSelectedPractice As PracticeDisplayModel = (New SessionHandler).PracticeDisplayGetFromSession(strFunctionCode)
                Dim udtPractice As PracticeModel = Nothing
                If Not IsNothing(udtSelectedPractice) Then
                    udtPractice = udtSP.PracticeList(udtSelectedPractice.PracticeID)
                End If

                ' CRE16-002 Revamp VSS [Start][Lawrence]
                Dim strGender As String = String.Empty

                If Not IsNothing(udtEHSPersonalInfo) Then
                    strGender = udtEHSPersonalInfo.Gender
                End If
                ' CRE16-002 Revamp VSS [End][Lawrence]

                udtEligibleResult = Me._udtClaimRulesBLL.CheckEligibilityFromEHSClaimSearch(udtSchemeClaimModel, strIdentityNum, strDocCode.Trim(), dtmDOB, strExactDOB, dtmCurrentDate, strGender, udtTranBenefitList, udtPractice)
                If Not udtEligibleResult.IsEligible Then
                    strMsgCode = "00106"
                End If
            End If

            ' -------------------------------------------------------------------------------
            ' 11. Check Document Limit
            ' -------------------------------------------------------------------------------
            If strMsgCode.Trim() = "" Then
                udtSearchAccountStatus.ExceedDocTypeLimit = Me._udtClaimRulesBLL.CheckExceedDocumentLimitFromEHSClaimSearch(strSchemeCode, strDocCode, dtmDOB, strExactDOB, dtmCurrentDate)
                If udtSearchAccountStatus.ExceedDocTypeLimit Then
                    If strDocCode = DocTypeModel.DocTypeCode.EC Then
                        strMsgCode = "00185"
                    Else
                        strMsgCode = "00213"
                    End If
                End If
            End If

            ' -------------------------------------------------------------------------------
            ' Return
            ' -------------------------------------------------------------------------------

            If Not udtEHSAccount Is Nothing Then
                ' For UI Setting
                udtEHSAccount.SetSearchDocCode(strDocCode.Trim())
            End If

            If Not strMsgCode.Equals(String.Empty) Then
                Return New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                Return Nothing
            End If
        End Function

        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        '''' <summary>
        '''' Search EHS Account with EligibleResult by EHS Account No
        '''' </summary>
        '''' <param name="strSchemeCode"></param>
        '''' <param name="strVRAcctID"></param>
        '''' <param name="udtEHSAccount"></param>
        '''' <param name="udtEligibleResult"></param>
        '''' <param name="blnExceedDocTypeLimit"></param>
        '''' <param name="strAdoptionPrefixNum"></param>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Public Function SearchTemporaryAccount(ByVal strSchemeCode As String, ByVal strVRAcctID As String, _
        '    ByRef udtEHSAccount As EHSAccountModel, ByRef udtEligibleResult As ClaimRulesBLL.EligibleResult, _
        '    ByRef blnExceedDocTypeLimit As Boolean, Optional ByVal strAdoptionPrefixNum As String = "") As Common.ComObject.SystemMessage

        '    Dim udtEHSAccountInformation As EHSAccountModel.EHSPersonalInformationModel
        '    Dim strMsgCode As String = String.Empty
        '    Dim udtSchemeClaimBLL As New SchemeClaimBLL()
        '    Dim dtmCurrentDateTime As Date = Me._udtCommonGenFunc.GetSystemDateTime()
        '    Dim udtSchemeClaimModel As SchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(strSchemeCode, dtmCurrentDateTime)
        '    Dim strFunctCode As String = "990000"
        '    Dim strSeverity As String = "E"
        '    Dim udtEHSAccountBll As EHSAccountBLL = New EHSAccountBLL()
        '    Dim strIdentityNum As String
        '    Dim strDocCode As String
        '    ' -------------------------------------------------------------------------------
        '    ' Service Date Back
        '    ' -------------------------------------------------------------------------------
        '    Dim dtmServiceDateBack As Date
        '    Dim strAllowDateBack As String = String.Empty
        '    Dim strDummy As String = String.Empty
        '    Dim strClaimDayLimit As String = String.Empty
        '    Dim strMinDate As String = String.Empty

        '    Dim udtGenFunct As New Common.ComFunction.GeneralFunction()
        '    udtGenFunct.getSystemParameter("DateBackClaimAllow", strAllowDateBack, strDummy)

        '    If strAllowDateBack = String.Empty Then
        '        strAllowDateBack = "N"
        '    End If

        '    If strAllowDateBack = "Y" Then
        '        udtGenFunct.getSystemParameter("DateBackClaimDayLimit", strClaimDayLimit, strDummy)
        '        udtGenFunct.getSystemParameter("DateBackClaimMinDate", strMinDate, strDummy)
        '        Dim intDayLimit As Integer = CInt(strClaimDayLimit)
        '        Dim dtmMinDate As DateTime = Convert.ToDateTime(strMinDate)

        '        dtmServiceDateBack = dtmCurrentDateTime.AddDays(-intDayLimit)
        '        If dtmServiceDateBack < dtmMinDate Then dtmServiceDateBack = dtmMinDate
        '        If dtmServiceDateBack < udtSchemeClaimModel.ClaimPeriodFrom Then dtmServiceDateBack = udtSchemeClaimModel.ClaimPeriodFrom
        '    Else
        '        dtmServiceDateBack = dtmCurrentDateTime
        '    End If

        '    ' -------------------------------------------------------------------------------
        '    ' 1. Search Temporary Account, Check Account Status
        '    ' -------------------------------------------------------------------------------
        '    'Me.SearchTemporaryAccount(strDocCode, strIdentityNum, udtEHSAccountInformation.DOB, udtEHSAccountInformation.ExactDOB, udtEHSAccount, _
        '    'blnTempAccountNotMatchDOBFound, blnOnlyInValidAccountFound, Nothing, Nothing, strAdoptionPrefixNum, udtEHSAccount.SearchDocCode)
        '    udtEHSAccount = udtEHSAccountBll.LoadTempEHSAccountByVRID(strVRAcctID)
        '    udtEHSAccountInformation = udtEHSAccount.EHSPersonalInformationList(0)
        '    udtEHSAccount.SetSearchDocCode(udtEHSAccountInformation.DocCode)
        '    strIdentityNum = udtEHSAccountInformation.IdentityNum
        '    strDocCode = udtEHSAccountInformation.DocCode

        '    ' [2009Oct22] Performance Tuning
        '    Dim udtEHSTransactionBLL As New EHSTransactionBLL()
        '    Dim udtTranBenefitList As TransactionDetailModelCollection = Nothing

        '    ' Transaction Checking will be only apply on Vaccine scheme
        '    If udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVaccine Then
        '        ' Retrieve the Transaction Detail related to the current Scheme (may have mutil SubsidizeCodes)
        '        udtTranBenefitList = udtEHSTransactionBLL.getTransactionDetailBenefit(strDocCode, strIdentityNum, udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SchemeSeq)
        '    End If

        '    ' -------------------------------------------------------------------------------
        '    ' 3. Check Eligible
        '    ' -------------------------------------------------------------------------------
        '    If strMsgCode.Trim() = "" Then
        '        udtEligibleResult = Me._udtClaimRulesBLL.CheckEligibilityFromEHSClaimSearch(udtSchemeClaimModel, strIdentityNum, strDocCode.Trim(), udtEHSAccountInformation.DOB, udtEHSAccountInformation.ExactDOB, dtmCurrentDateTime, udtTranBenefitList)
        '        If Not udtEligibleResult.IsEligible Then
        '            strMsgCode = "00106"
        '        End If
        '    End If

        '    ' -------------------------------------------------------------------------------
        '    ' 4. Check Document Limit
        '    ' -------------------------------------------------------------------------------
        '    If strMsgCode.Trim() = "" Then
        '        blnExceedDocTypeLimit = Me._udtClaimRulesBLL.CheckExceedDocumentLimitFromEHSClaimSearch(strSchemeCode, strDocCode, udtEHSAccountInformation.DOB, udtEHSAccountInformation.ExactDOB, dtmCurrentDateTime)
        '        If blnExceedDocTypeLimit Then
        '            If strDocCode = DocTypeModel.DocTypeCode.EC Then
        '                strMsgCode = "00185"
        '            Else
        '                strMsgCode = "00213"
        '            End If
        '        End If
        '    End If

        '    ' -------------------------------------------------------------------------------
        '    ' 5. Check Benefit
        '    ' -------------------------------------------------------------------------------
        '    If strMsgCode.Trim() = "" Then
        '        If udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVaccine Then
        '            If Not Me._udtClaimRulesBLL.chkVaccineAvailableBenefit(strDocCode, strIdentityNum, udtSchemeClaimModel, udtTranBenefitList, udtEHSAccountInformation.DOB, udtEHSAccountInformation.ExactDOB, dtmCurrentDateTime, dtmServiceDateBack) Then
        '                strMsgCode = "00195"
        '            End If
        '        Else
        '            Dim intAvailable As Integer = Me._udtEHSTransactionBLL.getAvailableVoucher(dtmCurrentDateTime, strDocCode, strIdentityNum, udtEHSAccountInformation.DOB, udtEHSAccountInformation.ExactDOB, udtSchemeClaimModel)
        '            udtEHSAccount.AvailableVoucher = intAvailable
        '            If intAvailable <= 0 Then
        '                strMsgCode = "00107"
        '            End If
        '        End If
        '    End If

        '    ' -------------------------------------------------------------------------------
        '    ' Return
        '    ' -------------------------------------------------------------------------------
        '    If Not udtEHSAccount Is Nothing Then
        '        ' For UI Setting
        '        udtEHSAccount.SetSearchDocCode(strDocCode.Trim())
        '    End If

        '    If Not strMsgCode.Equals(String.Empty) Then
        '        Return New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
        '    Else
        '        Return Nothing
        '    End If

        'End Function
        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

        Private Sub SearchValidatedAccount(ByVal strDocCode As String, ByVal strIdentityNum As String, ByRef udtEHSAccount As EHSAccountModel, ByRef strSearchDocCode As String)

            strSearchDocCode = strDocCode.Trim()
            ' Load Validated Account
            udtEHSAccount = Me._udtEHSAccountBLL.LoadEHSAccountByIdentity(strIdentityNum, strDocCode)

            ' Load Validated Account with HKIC / BirthCert if not Found (HKIC <==> BirthCert)
            If udtEHSAccount Is Nothing AndAlso strDocCode.Trim().ToUpper() = DocTypeModel.DocTypeCode.HKIC Then

                strSearchDocCode = DocTypeModel.DocTypeCode.HKBC
                udtEHSAccount = Me._udtEHSAccountBLL.LoadEHSAccountByIdentity(strIdentityNum, strSearchDocCode)
            ElseIf udtEHSAccount Is Nothing AndAlso strDocCode.Trim().ToUpper() = DocTypeModel.DocTypeCode.HKBC Then

                strSearchDocCode = DocTypeModel.DocTypeCode.HKIC
                udtEHSAccount = Me._udtEHSAccountBLL.LoadEHSAccountByIdentity(strIdentityNum, strSearchDocCode)
            End If

        End Sub

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Public Sub SearchTemporaryAccount(ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal strDOB As String, _
            ByRef udtEHSAccount As EHSAccountModel, ByRef udtSearchAccountStatus As SearchAccountStatus, ByVal udtEHSPersonalInfo As EHSPersonalInformationModel)

            ' -------------------------------------------------------------------------------
            ' Init
            ' -------------------------------------------------------------------------------
            strIdentityNum = Me._udtFormater.formatDocumentIdentityNumber(strDocCode, strIdentityNum)
            Dim dtmCurrentDateTime As Date = Me._udtCommonGenFunc.GetSystemDateTime()
            Dim dtmCurrentDate As Date = dtmCurrentDateTime.Date

            ' Indicate the DocCode of EHS Account of Database
            Dim strSearchDocCode As String = String.Empty

            Dim dtmDOB As Date = Nothing
            Dim strExactDOB As String = Nothing
            Me._udtCommonGenFunc.chkDOBtype(strDOB, dtmDOB, strExactDOB)

            SearchTemporaryAccount(strDocCode, udtEHSPersonalInfo.IdentityNum, dtmDOB, strExactDOB, udtEHSAccount, _
                                udtSearchAccountStatus, Nothing, Nothing, "", strSearchDocCode, _
                                udtEHSPersonalInfo)

            If Not udtEHSAccount Is Nothing Then
                ' For UI Setting
                udtEHSAccount.SetSearchDocCode(strDocCode.Trim())
            End If
        End Sub
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]


        Private Sub SearchTemporaryAccount(ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal dtmDOB As Date, ByVal strExactDOB As String, _
            ByRef udtEHSAccount As EHSAccountModel, ByRef udtSearchAccountStatus As SearchAccountStatus, _
            ByVal intAge As Nullable(Of Integer), ByVal dtmDOR As Nullable(Of Date), ByVal strAdoptionPrefixNum As String, ByRef strSearchDocCode As String, _
            ByVal udtEHSPersonalInfo As EHSPersonalInformationModel)

            Dim udtEHSAccountModelList As EHSAccountModelCollection = Me._udtEHSAccountBLL.LoadTempEHSAccountByIdentity(strIdentityNum, strDocCode)
            Dim udtTempEHSAccount As EHSAccountModel = Nothing
            Dim blnInvalidTempAccountFound As Boolean = False
            strSearchDocCode = strDocCode.Trim()

            ' -----------------------------------------------------------------------------------
            ' Temporary Account
            ' -----------------------------------------------------------------------------------
            For Each udtCurEHSAccount As EHSAccountModel In udtEHSAccountModelList
                ' --- Checking 1: For Claim or For Validate only ----
                If udtCurEHSAccount.AccountPurpose <> AccountPurposeClass.ForClaim AndAlso _
                        udtCurEHSAccount.AccountPurpose <> AccountPurposeClass.ForValidate Then
                    Continue For
                End If

                ' --- Checking 2: Pending For Confirmation Or Pending For Verify ----
                If udtCurEHSAccount.RecordStatus <> TempAccountRecordStatusClass.PendingConfirmation AndAlso _
                        udtCurEHSAccount.RecordStatus <> TempAccountRecordStatusClass.PendingVerify Then
                    If udtCurEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.InValid Then
                        ' Any Invalid Record Found!
                        blnInvalidTempAccountFound = True
                    End If

                    Continue For
                End If

                ' ---- Checking 3: Not X Account ----
                If Not IsNothing(udtCurEHSAccount.OriginalAccID) AndAlso udtCurEHSAccount.OriginalAccID.Trim <> String.Empty Then
                    ' X Account Found: Invalid Record Found
                    blnInvalidTempAccountFound = True

                    Continue For
                End If

                ' ---- Checking 4: Match DOB ----
                If intAge.HasValue AndAlso dtmDOR.HasValue Then
                    ' EC Case Report Age on Date of Registration
                    If Not chkEHSAccountInputDOBMatch(udtCurEHSAccount.EHSPersonalInformationList(0), intAge.Value, dtmDOR.Value) Then
                        ' Any Temp Account Not Match DOB Found
                        udtSearchAccountStatus.TempAccountNotMatchDOBFound = True

                        Continue For
                    End If

                    If Not ChkEHSAccountInputDetailMatch(udtEHSPersonalInfo, udtCurEHSAccount.EHSPersonalInformationList(0)) Then
                        udtSearchAccountStatus.TempAccountInputDetailDiffFound = True

                        Continue For
                    End If

                    If udtTempEHSAccount Is Nothing Then
                        udtTempEHSAccount = udtCurEHSAccount
                    Else
                        If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                            udtTempEHSAccount = udtCurEHSAccount
                        End If
                    End If

                Else
                    If Not chkEHSAccountInputDOBMatch(udtCurEHSAccount.EHSPersonalInformationList(0), dtmDOB, strExactDOB) Then
                        ' Any Temp Account Not Match DOB Found
                        udtSearchAccountStatus.TempAccountNotMatchDOBFound = True

                        Continue For
                    End If

                    If Not ChkEHSAccountInputDetailMatch(udtEHSPersonalInfo, udtCurEHSAccount.EHSPersonalInformationList(0)) Then
                        udtSearchAccountStatus.TempAccountInputDetailDiffFound = True

                        Continue For
                    End If

                    If udtTempEHSAccount Is Nothing Then
                        udtTempEHSAccount = udtCurEHSAccount
                    Else
                        '==================================================================== Code for SmartID ============================================================================
                        If udtTempEHSAccount.EHSPersonalInformationList(0).DocCode.Trim.Equals(DocType.DocTypeModel.DocTypeCode.HKIC) AndAlso _
                           udtCurEHSAccount.EHSPersonalInformationList(0).DocCode.Trim.Equals(DocType.DocTypeModel.DocTypeCode.HKIC) Then

                            If udtTempEHSAccount.EHSPersonalInformationList(0).CreateBySmartID Then
                                If udtCurEHSAccount.EHSPersonalInformationList(0).CreateBySmartID Then

                                    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
                                    ' ---------------------------------------------------------------------------------------------------------
                                    ' Get latest version
                                    Dim strExistSmartIDVer As String = Replace(udtTempEHSAccount.EHSPersonalInformationList(0).SmartIDVer, "C", "")
                                    Dim strCurrentSmartIDVer As String = Replace(udtCurEHSAccount.EHSPersonalInformationList(0).SmartIDVer, "C", "")
                                    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

                                    If SmartIDHandler.IsExistingOldSmartIDVersion(strExistSmartIDVer, strCurrentSmartIDVer) Then
                                        udtTempEHSAccount = udtCurEHSAccount

                                    ElseIf SmartIDHandler.CompareVersion(strExistSmartIDVer, strCurrentSmartIDVer, "=") Then
                                        ' Get latest create date when same version
                                        If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                                            udtTempEHSAccount = udtCurEHSAccount
                                        End If
                                    End If

                                End If
                            Else
                                If udtCurEHSAccount.EHSPersonalInformationList(0).CreateBySmartID Then
                                    udtTempEHSAccount = udtCurEHSAccount
                                End If
                            End If
                        Else
                            If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                                udtTempEHSAccount = udtCurEHSAccount
                            End If
                        End If
                        '==================================================================================================================================================================

                        'If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                        '    udtTempEHSAccount = udtCurEHSAccount
                        'End If
                    End If

                End If

            Next

            ' -----------------------------------------------------------------------------------
            ' Special Account
            ' -----------------------------------------------------------------------------------
            udtEHSAccountModelList = Me._udtEHSAccountBLL.LoadSpecialEHSAccountByIdentity(strIdentityNum, strDocCode)
            For Each udtCurEHSAccount As EHSAccountModel In udtEHSAccountModelList
                ' --- Checking 1: For Claim or For Validate only ----
                If udtCurEHSAccount.AccountPurpose <> EHSAccountModel.AccountPurposeClass.ForClaim _
                        AndAlso udtCurEHSAccount.AccountPurpose <> EHSAccountModel.AccountPurposeClass.ForValidate Then
                    Continue For
                End If

                ' --- Checking 2: Pending For Confirmation Or Pending For Verify ----
                If udtCurEHSAccount.RecordStatus <> TempAccountRecordStatusClass.PendingConfirmation AndAlso _
                        udtCurEHSAccount.RecordStatus <> TempAccountRecordStatusClass.PendingVerify Then
                    If udtCurEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.InValid Then
                        ' Any Invalid Record Found!
                        blnInvalidTempAccountFound = True
                    End If

                    Continue For
                End If

                ' ---- Checking 3: Not X Account ----
                If Not IsNothing(udtCurEHSAccount.TempVouhcerAccID) AndAlso udtCurEHSAccount.TempVouhcerAccID.Trim <> String.Empty Then
                    ' X Account Found: Invalid Record Found
                    blnInvalidTempAccountFound = True

                    Continue For
                End If

                ' ---- Checking 4: Match DOB ----
                If intAge.HasValue AndAlso dtmDOR.HasValue Then
                    ' Actually special account won't have EC

                    ' EC Case Report Age on Date of Registration
                    If Not chkEHSAccountInputDOBMatch(udtCurEHSAccount.EHSPersonalInformationList(0), intAge.Value, dtmDOR.Value) Then
                        ' Any Temp Account Not Match DOB Found
                        udtSearchAccountStatus.TempAccountNotMatchDOBFound = True

                        Continue For
                    End If

                    If Not ChkEHSAccountInputDetailMatch(udtEHSPersonalInfo, udtCurEHSAccount.EHSPersonalInformationList(0)) Then
                        udtSearchAccountStatus.TempAccountInputDetailDiffFound = True

                        Continue For
                    End If

                    If udtTempEHSAccount Is Nothing Then
                        udtTempEHSAccount = udtCurEHSAccount
                    Else
                        If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                            udtTempEHSAccount = udtCurEHSAccount
                        End If
                    End If

                Else
                    If Not chkEHSAccountInputDOBMatch(udtCurEHSAccount.EHSPersonalInformationList(0), dtmDOB, strExactDOB) Then
                        ' Any Temp Account Not Match DOB Found
                        udtSearchAccountStatus.TempAccountNotMatchDOBFound = True

                        Continue For
                    End If

                    If Not ChkEHSAccountInputDetailMatch(udtEHSPersonalInfo, udtCurEHSAccount.EHSPersonalInformationList(0)) Then
                        udtSearchAccountStatus.TempAccountInputDetailDiffFound = True

                        Continue For
                    End If

                    If udtTempEHSAccount Is Nothing Then
                        udtTempEHSAccount = udtCurEHSAccount
                    Else
                        '==================================================================== Code for SmartID ============================================================================
                        If udtTempEHSAccount.EHSPersonalInformationList(0).DocCode.Trim.Equals(DocType.DocTypeModel.DocTypeCode.HKIC) AndAlso _
                           udtCurEHSAccount.EHSPersonalInformationList(0).DocCode.Trim.Equals(DocType.DocTypeModel.DocTypeCode.HKIC) Then

                            If udtTempEHSAccount.EHSPersonalInformationList(0).CreateBySmartID Then
                                If udtCurEHSAccount.EHSPersonalInformationList(0).CreateBySmartID Then

                                    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
                                    ' ---------------------------------------------------------------------------------------------------------
                                    ' Get latest version
                                    Dim strExistSmartIDVer As String = Replace(udtTempEHSAccount.EHSPersonalInformationList(0).SmartIDVer, "C", "")
                                    Dim strCurrentSmartIDVer As String = Replace(udtCurEHSAccount.EHSPersonalInformationList(0).SmartIDVer, "C", "")
                                    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

                                    If SmartIDHandler.IsExistingOldSmartIDVersion(strExistSmartIDVer, strCurrentSmartIDVer) Then
                                        udtTempEHSAccount = udtCurEHSAccount

                                    ElseIf SmartIDHandler.CompareVersion(strExistSmartIDVer, strCurrentSmartIDVer, "=") Then
                                        ' Get latest create date when same version
                                        If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                                            udtTempEHSAccount = udtCurEHSAccount
                                        End If
                                    End If

                                End If
                            Else
                                If udtCurEHSAccount.EHSPersonalInformationList(0).CreateBySmartID Then
                                    udtTempEHSAccount = udtCurEHSAccount
                                End If
                            End If
                        Else
                            If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                                udtTempEHSAccount = udtCurEHSAccount
                            End If
                        End If
                        '==================================================================================================================================================================

                        'If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                        '    udtTempEHSAccount = udtCurEHSAccount
                        'End If
                    End If

                End If

            Next

            If Not udtTempEHSAccount Is Nothing Then
                udtEHSAccount = udtTempEHSAccount
            End If

            ' -----------------------------------------------------------
            ' Load Temporary Account with HKIC / BirthCert if not Found (HKIC <==> BirthCert)
            ' -----------------------------------------------------------
            If udtEHSAccount Is Nothing Then
                ' -----------------------------------------------------------------------------------
                ' Temporary Account
                ' -----------------------------------------------------------------------------------
                udtEHSAccountModelList = Nothing
                udtTempEHSAccount = Nothing

                If strDocCode = DocTypeModel.DocTypeCode.HKIC Then
                    strSearchDocCode = DocTypeModel.DocTypeCode.HKBC
                    udtEHSAccountModelList = Me._udtEHSAccountBLL.LoadTempEHSAccountByIdentity(strIdentityNum, strSearchDocCode)
                ElseIf strDocCode = DocTypeModel.DocTypeCode.HKBC Then
                    strSearchDocCode = DocTypeModel.DocTypeCode.HKIC
                    udtEHSAccountModelList = Me._udtEHSAccountBLL.LoadTempEHSAccountByIdentity(strIdentityNum, strSearchDocCode)
                End If
                If Not udtEHSAccountModelList Is Nothing AndAlso udtEHSAccountModelList.Count > 0 Then
                    For Each udtCurEHSAccount As EHSAccountModel In udtEHSAccountModelList
                        ' Temporary Account
                        If (udtCurEHSAccount.AccountPurpose = EHSAccountModel.AccountPurposeClass.ForClaim Or _
                            udtCurEHSAccount.AccountPurpose = EHSAccountModel.AccountPurposeClass.ForValidate) Then

                            ' Pending For Confirmation Or Pending For Verify
                            If (udtCurEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingConfirmation Or _
                                udtCurEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingVerify) Then

                                ' Not X Account
                                If (udtCurEHSAccount.OriginalAccID Is Nothing OrElse udtCurEHSAccount.OriginalAccID.Trim() = "") Then

                                    ' Match DOB
                                    If intAge.HasValue AndAlso dtmDOR.HasValue Then
                                        'EC Case Report Age on Date of Registration
                                        If Me.chkEHSAccountInputDOBMatch(udtCurEHSAccount.EHSPersonalInformationList(0), intAge.Value, dtmDOR.Value) Then

                                            If udtTempEHSAccount Is Nothing Then
                                                udtTempEHSAccount = udtCurEHSAccount
                                            Else
                                                If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                                                    udtTempEHSAccount = udtCurEHSAccount
                                                End If
                                            End If
                                        Else
                                            ' Any Temp Account Not Match DOB Found
                                            udtSearchAccountStatus.TempAccountNotMatchDOBFound = True
                                        End If
                                    Else
                                        If Me.chkEHSAccountInputDOBMatch(udtCurEHSAccount.EHSPersonalInformationList(0), dtmDOB, strExactDOB) Then
                                            If Not ChkEHSAccountInputDetailMatch(udtEHSPersonalInfo, udtCurEHSAccount.EHSPersonalInformationList(0)) Then
                                                udtSearchAccountStatus.TempAccountInputDetailDiffFound = True

                                                Continue For
                                            End If

                                            If udtTempEHSAccount Is Nothing Then
                                                udtTempEHSAccount = udtCurEHSAccount
                                            Else
                                                '==================================================================== Code for SmartID ============================================================================
                                                If udtTempEHSAccount.EHSPersonalInformationList(0).DocCode.Trim.Equals(DocType.DocTypeModel.DocTypeCode.HKIC) AndAlso _
                                                   udtCurEHSAccount.EHSPersonalInformationList(0).DocCode.Trim.Equals(DocType.DocTypeModel.DocTypeCode.HKIC) Then

                                                    If udtTempEHSAccount.EHSPersonalInformationList(0).CreateBySmartID Then
                                                        If udtCurEHSAccount.EHSPersonalInformationList(0).CreateBySmartID Then

                                                            ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
                                                            ' ---------------------------------------------------------------------------------------------------------
                                                            ' Get latest version
                                                            Dim strExistSmartIDVer As String = Replace(udtTempEHSAccount.EHSPersonalInformationList(0).SmartIDVer, "C", "")
                                                            Dim strCurrentSmartIDVer As String = Replace(udtCurEHSAccount.EHSPersonalInformationList(0).SmartIDVer, "C", "")
                                                            ' CRE19-028 (IDEAS Combo) [End][Chris YIM]

                                                            If SmartIDHandler.IsExistingOldSmartIDVersion(strExistSmartIDVer, strCurrentSmartIDVer) Then
                                                                udtTempEHSAccount = udtCurEHSAccount

                                                            ElseIf SmartIDHandler.CompareVersion(strExistSmartIDVer, strCurrentSmartIDVer, "=") Then
                                                                ' Get latest create date when same version
                                                                If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                                                                    udtTempEHSAccount = udtCurEHSAccount
                                                                End If
                                                            End If

                                                        End If
                                                    Else
                                                        If udtCurEHSAccount.EHSPersonalInformationList(0).CreateBySmartID Then
                                                            udtTempEHSAccount = udtCurEHSAccount
                                                        End If
                                                    End If
                                                Else
                                                    If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                                                        udtTempEHSAccount = udtCurEHSAccount
                                                    End If
                                                End If
                                                '==================================================================================================================================================================

                                                'If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                                                '    udtTempEHSAccount = udtCurEHSAccount
                                                'End If
                                            End If
                                        Else
                                            ' Any Temp Account Not Match DOB Found
                                            udtSearchAccountStatus.TempAccountNotMatchDOBFound = True
                                        End If
                                    End If
                                Else
                                    ' X Account Found: Invalid Record Found
                                    blnInvalidTempAccountFound = True
                                End If
                            ElseIf udtCurEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.InValid Then
                                ' Any Invalid Record Found!
                                blnInvalidTempAccountFound = True
                            End If
                        End If
                    Next
                End If
                ' -----------------------------------------------------------------------------------
                ' Special Account
                ' -----------------------------------------------------------------------------------

                If strDocCode = DocTypeModel.DocTypeCode.HKIC Then
                    strSearchDocCode = DocTypeModel.DocTypeCode.HKBC
                    udtEHSAccountModelList = Me._udtEHSAccountBLL.LoadSpecialEHSAccountByIdentity(strIdentityNum, strSearchDocCode)
                ElseIf strDocCode = DocTypeModel.DocTypeCode.HKBC Then
                    strSearchDocCode = DocTypeModel.DocTypeCode.HKIC
                    udtEHSAccountModelList = Me._udtEHSAccountBLL.LoadSpecialEHSAccountByIdentity(strIdentityNum, strSearchDocCode)
                End If

                If Not udtEHSAccountModelList Is Nothing AndAlso udtEHSAccountModelList.Count > 0 Then
                    For Each udtCurEHSAccount As EHSAccountModel In udtEHSAccountModelList
                        ' Special Account
                        If (udtCurEHSAccount.AccountPurpose = EHSAccountModel.AccountPurposeClass.ForClaim Or _
                            udtCurEHSAccount.AccountPurpose = EHSAccountModel.AccountPurposeClass.ForValidate) Then

                            ' Special Account must Confiremd
                            ' Pending For Confirmation Or Pending For Verify
                            If (udtCurEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingConfirmation Or _
                                udtCurEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingVerify) Then

                                ' Not X Account
                                If (udtCurEHSAccount.TempVouhcerAccID Is Nothing OrElse udtCurEHSAccount.TempVouhcerAccID.Trim() = "") Then
                                    ' Match DOB
                                    If intAge.HasValue AndAlso dtmDOR.HasValue Then
                                        'EC Case Report Age on Date of Registration
                                        If Me.chkEHSAccountInputDOBMatch(udtCurEHSAccount.EHSPersonalInformationList(0), intAge.Value, dtmDOR.Value) Then

                                            If udtTempEHSAccount Is Nothing Then
                                                udtTempEHSAccount = udtCurEHSAccount
                                            Else
                                                If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                                                    udtTempEHSAccount = udtCurEHSAccount
                                                End If
                                            End If
                                        Else
                                            ' Any Temp Account Not Match DOB Found
                                            udtSearchAccountStatus.TempAccountNotMatchDOBFound = True
                                        End If
                                    Else
                                        If Me.chkEHSAccountInputDOBMatch(udtCurEHSAccount.EHSPersonalInformationList(0), dtmDOB, strExactDOB) Then
                                            If Not ChkEHSAccountInputDetailMatch(udtEHSPersonalInfo, udtCurEHSAccount.EHSPersonalInformationList(0)) Then
                                                udtSearchAccountStatus.TempAccountInputDetailDiffFound = True

                                                Continue For
                                            End If

                                            If udtTempEHSAccount Is Nothing Then
                                                udtTempEHSAccount = udtCurEHSAccount
                                            Else
                                                '==================================================================== Code for SmartID ============================================================================
                                                If udtTempEHSAccount.EHSPersonalInformationList(0).DocCode.Trim.Equals(DocType.DocTypeModel.DocTypeCode.HKIC) AndAlso _
                                                   udtCurEHSAccount.EHSPersonalInformationList(0).DocCode.Trim.Equals(DocType.DocTypeModel.DocTypeCode.HKIC) Then

                                                    If udtTempEHSAccount.EHSPersonalInformationList(0).CreateBySmartID Then
                                                        If udtCurEHSAccount.EHSPersonalInformationList(0).CreateBySmartID Then

                                                            ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
                                                            ' ---------------------------------------------------------------------------------------------------------
                                                            ' Get latest version
                                                            Dim strExistSmartIDVer As String = Replace(udtTempEHSAccount.EHSPersonalInformationList(0).SmartIDVer, "C", "")
                                                            Dim strCurrentSmartIDVer As String = Replace(udtCurEHSAccount.EHSPersonalInformationList(0).SmartIDVer, "C", "")
                                                            ' CRE19-028 (IDEAS Combo) [End][Chris YIM]

                                                            If SmartIDHandler.IsExistingOldSmartIDVersion(strExistSmartIDVer, strCurrentSmartIDVer) Then
                                                                udtTempEHSAccount = udtCurEHSAccount

                                                            ElseIf SmartIDHandler.CompareVersion(strExistSmartIDVer, strCurrentSmartIDVer, "=") Then
                                                                ' Get latest create date when same version
                                                                If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                                                                    udtTempEHSAccount = udtCurEHSAccount
                                                                End If
                                                            End If

                                                        End If
                                                    Else
                                                        If udtCurEHSAccount.EHSPersonalInformationList(0).CreateBySmartID Then
                                                            udtTempEHSAccount = udtCurEHSAccount
                                                        End If
                                                    End If
                                                Else
                                                    If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                                                        udtTempEHSAccount = udtCurEHSAccount
                                                    End If
                                                End If
                                                '==================================================================================================================================================================

                                                'If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                                                '    udtTempEHSAccount = udtCurEHSAccount
                                                'End If
                                            End If
                                        Else
                                            ' Any Temp Account Not Match DOB Found
                                            udtSearchAccountStatus.TempAccountNotMatchDOBFound = True
                                        End If
                                    End If
                                Else
                                    ' X Account Record Found!
                                    blnInvalidTempAccountFound = True
                                End If

                            ElseIf udtCurEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.InValid Then
                                ' Any Invalid Record Found!
                                blnInvalidTempAccountFound = True
                            End If
                        End If
                    Next
                End If

                If Not udtTempEHSAccount Is Nothing Then
                    udtEHSAccount = udtTempEHSAccount
                End If
            End If

            If udtTempEHSAccount Is Nothing Then
                ' Not Match Record Found & Invalid Record Found!
                If blnInvalidTempAccountFound Then
                    udtSearchAccountStatus.OnlyInvalidAccountFound = True
                End If
            End If

        End Sub

        '==================================================================== Code for SmartID ============================================================================
        Public Function SearchEHSAccountSmartID(ByVal strSchemeCode As String, ByVal strDocCode As String, ByVal strIdentityNum As String, _
                            ByVal strDOB As String, ByRef udtEHSAccount As EHSAccountModel, ByVal udtEHSAccountSmartID As EHSAccountModel, _
                            ByRef enumSmartIDReadStatus As SmartIDHandler.SmartIDResultStatus, ByRef udtEligibleResult As ClaimRulesBLL.EligibleResult, _
                            ByRef blnNoMatchRecordFound As Boolean, ByRef blnExceedDocTypeLimit As Boolean, ByVal strFunctionCode As String, ByVal blnCheckSelfClaim As Boolean, _
                            ByVal enumClaimMode As ClaimMode) As Common.ComObject.SystemMessage

            ' -------------------------------------------------------------------------------
            ' Init
            ' -------------------------------------------------------------------------------
            strIdentityNum = Me._udtFormater.formatDocumentIdentityNumber(strDocCode, strIdentityNum)
            Dim dtmCurrentDateTime As Date = Me._udtCommonGenFunc.GetSystemDateTime()
            Dim dtmCurrentDate As Date = dtmCurrentDateTime.Date


            ' Indicate the DocCode of EHS Account of Database
            Dim strSearchDocCode As String = String.Empty
            Dim strFunctCode As String = "990000"
            Dim strSeverity As String = "E"
            Dim strMsgCode As String = String.Empty

            Dim sm As Common.ComObject.SystemMessage = Nothing
            Dim udtCurrEHSPersonalInfoModel As EHSAccountModel.EHSPersonalInformationModel = Nothing

            Dim dtmDOB As Date = Nothing
            Dim strExactDOB As String = Nothing
            Me._udtCommonGenFunc.chkDOBtype(strDOB, dtmDOB, strExactDOB)

            ' -------------------------------------------------------------------------------
            ' 1. Check Active SchemeClaim
            ' -------------------------------------------------------------------------------
            Dim udtSchemeClaimBLL As New SchemeClaimBLL()
            Dim udtSchemeClaimModel As SchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(strSchemeCode, dtmCurrentDateTime)

            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            udtSchemeClaimModel.SubsidizeGroupClaimList = udtSchemeClaimBLL.FilterSubsidizeGroupClaim(udtSchemeClaimModel, enumClaimMode)
            ' CRE20-0022 (Immu record) [End][Chris YIM]

            If udtSchemeClaimModel Is Nothing OrElse udtSchemeClaimModel.SubsidizeGroupClaimList.Count = 0 Then
                strMsgCode = "00105"
            End If

            ' -------------------------------------------------------------------------------
            ' Service Date Back
            ' -------------------------------------------------------------------------------

            Dim dtmServiceDateBack As Date = dtmCurrentDate
            Dim strAllowDateBack As String = String.Empty
            Dim strDummy As String = String.Empty
            Dim strClaimDayLimit As String = String.Empty
            Dim strMinDate As String = String.Empty

            Dim udtGenFunct As New Common.ComFunction.GeneralFunction()

            udtGenFunct.getSystemParameter("DateBackClaimAllow", strAllowDateBack, strDummy)

            If strAllowDateBack = String.Empty Then
                strAllowDateBack = "N"
            End If

            If strAllowDateBack = "Y" Then
                udtGenFunct.getSystemParameter("DateBackClaimDayLimit", strClaimDayLimit, strDummy)
                udtGenFunct.getSystemParameter("DateBackClaimMinDate", strMinDate, strDummy)
                Dim intDayLimit As Integer = CInt(strClaimDayLimit)
                Dim dtmMinDate As DateTime = Convert.ToDateTime(strMinDate)

                dtmServiceDateBack = dtmCurrentDate.AddDays(-intDayLimit)
                If dtmServiceDateBack < dtmMinDate Then dtmServiceDateBack = dtmMinDate
                If dtmServiceDateBack < udtSchemeClaimModel.ClaimPeriodFrom Then dtmServiceDateBack = udtSchemeClaimModel.ClaimPeriodFrom
            Else
                dtmServiceDateBack = dtmCurrentDate
            End If

            ' -------------------------------------------------------------------------------
            ' INT16-0030 Block Smart ID search for deceased recipient
            ' 3. Recipient Deceased
            ' Check Active Death Record
            ' If dead, return "(document id name) is invalid"
            ' -------------------------------------------------------------------------------
            If strMsgCode.Trim() = "" Then
                If (New DocTypeBLL).getDocTypeByAvailable(DocTypeBLL.EnumAvailable.DeathRecordAvailable).Filter(strDocCode) IsNot Nothing Then
                    If (New eHealthAccountDeathRecord.eHealthAccountDeathRecordBLL).GetDeathRecordEntry(strIdentityNum).IsDead() Then
                        strMsgCode = (New Common.Validation.Validator).GetMessageForIdentityNoIsNoLongerValid(strDocCode).MessageCode
                    End If
                End If
            End If

            ' -------------------------------------------------------------------------------
            ' 4. Block EHCP make voucher claim for themselves
            ' Check Inputted Doc. No. with SP's HKID
            ' If matches, return "HCPs are not allowed to make claims for himself/herself"
            ' -------------------------------------------------------------------------------

            'CRE16-017 (Block EHCP make voucher claim for themselves) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            ' Ignore the checking from VRE
            If strMsgCode.Trim() = "" AndAlso blnCheckSelfClaim Then
                Dim udtSP As ServiceProviderModel = UserACBLL.GetServiceProvider()
                strMsgCode = Me._udtClaimRulesBLL.IsSPClaimForThemselves(udtSP.HKID, strDocCode, strIdentityNum, udtSchemeClaimModel.AvailableHCSPSubPlatform)
            End If
            'CRE16-017 (Block EHCP make voucher claim for themselves) [End][Chris YIM]

            ' -------------------------------------------------------------------------------
            ' 5. Check HKIC VS EC
            ' -------------------------------------------------------------------------------
            If strMsgCode.Trim() = "" Then
                strMsgCode = Me._udtClaimRulesBLL.chkEHSAccountHKICVsEC(EHSAccountModel.SysAccountSource.ValidateAccount, strDocCode, strIdentityNum)
            End If
            If strMsgCode.Trim() = "" Then
                strMsgCode = Me._udtClaimRulesBLL.chkEHSAccountHKICVsEC(EHSAccountModel.SysAccountSource.TemporaryAccount, strDocCode, strIdentityNum)
            End If
            If strMsgCode.Trim() = "" Then
                strMsgCode = Me._udtClaimRulesBLL.chkEHSAccountHKICVsEC(EHSAccountModel.SysAccountSource.SpecialAccount, strDocCode, strIdentityNum)
            End If

            ' -------------------------------------------------------------------------------
            ' 6. Search Validate Account, Check Account Status (DOB Match)
            ' -------------------------------------------------------------------------------
            Me.SearchValidatedAccount(strDocCode, strIdentityNum, udtEHSAccount, strSearchDocCode)

            If strMsgCode = String.Empty Then
                enumSmartIDReadStatus = BLL.SmartIDHandler.CheckSmartIDCardStatus(udtEHSAccountSmartID, udtEHSAccount)

                Select Case enumSmartIDReadStatus

                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    'Case BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_NotCreateBySmartID_NotSameDOB
                    Case BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_DiffDOB
                        strMsgCode = "00247"
                    Case BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOIDOB_CreateBySmartID_WithGender
                        'Case BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_CreateBySmartID
                        strMsgCode = "00248"
                    Case BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOIDOB_CreateBySmartID_WithoutGender_DiffName
                        strMsgCode = "00248"
                    Case BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_DiffDOI_SmallerDOI
                        strMsgCode = "00249"
                    Case BLL.SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB_WithGender, _
                        BLL.SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB_WithoutGender_DiffName
                        'Case BLL.SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB
                        strMsgCode = "00250"
                End Select
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
            End If


            If Not udtEHSAccount Is Nothing AndAlso strMsgCode = String.Empty Then
                ' Validate Account Found
                If udtEHSAccount.RecordStatus.Equals(EHSAccountModel.ValidatedAccountRecordStatusClass.Suspended) AndAlso chkSuspendAccountClaimAllow(enumClaimMode) = False Then
                    strMsgCode = "00108"
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                    ' -----------------------------------------------------------------------------------------
                ElseIf udtEHSAccount.RecordStatus.Equals(EHSAccountModel.ValidatedAccountRecordStatusClass.Terminated) Then
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
                    strMsgCode = "00109"
                ElseIf udtEHSAccount.RecordStatus.Equals(EHSAccountModel.ValidatedAccountRecordStatusClass.Active) OrElse _
                       (udtEHSAccount.RecordStatus.Equals(EHSAccountModel.ValidatedAccountRecordStatusClass.Suspended) AndAlso chkSuspendAccountClaimAllow(enumClaimMode) = True) Then
                    ' [INT11-0023] 
                    If enumSmartIDReadStatus = SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_DiffDOI_LargerDOI Then
                        ' Reading card with Larger DOI, bypass DOB checking
                    Else
                        ' Check DOB Match
                        udtCurrEHSPersonalInfoModel = udtEHSAccount.EHSPersonalInformationList.Filter(strSearchDocCode)
                        If Not Me.chkEHSAccountInputDOBMatch(udtCurrEHSPersonalInfoModel, dtmDOB, strExactDOB) Then
                            'DOB Not Match
                            Select Case strDocCode
                                Case DocTypeModel.DocTypeCode.ADOPC
                                    strMsgCode = "00222"
                                Case DocTypeModel.DocTypeCode.DI, DocTypeModel.DocTypeCode.OW, DocTypeModel.DocTypeCode.TW, DocTypeModel.DocTypeCode.PASS, _
                                    DocTypeModel.DocTypeCode.MEP, DocTypeModel.DocTypeCode.TWMTP, DocTypeModel.DocTypeCode.TWPAR, DocTypeModel.DocTypeCode.TWVTD, _
                                    DocTypeModel.DocTypeCode.TWNS, DocTypeModel.DocTypeCode.MD, DocTypeModel.DocTypeCode.MP, DocTypeModel.DocTypeCode.TD, _
                                    DocTypeModel.DocTypeCode.CEEP, DocTypeModel.DocTypeCode.ISSHK, DocTypeModel.DocTypeCode.ET, DocTypeModel.DocTypeCode.RFNo8, _
                                    DocTypeModel.DocTypeCode.DS
                                    strMsgCode = "00223"
                                Case DocTypeModel.DocTypeCode.EC, DocTypeModel.DocTypeCode.CCIC, DocTypeModel.DocTypeCode.ROP140 ' CRE20-0022 (Immu record) [Martin]
                                    strMsgCode = "00110"
                                Case DocTypeModel.DocTypeCode.HKBC
                                    strMsgCode = "00224"
                                Case DocTypeModel.DocTypeCode.HKIC
                                    strMsgCode = "00110"
                                Case DocTypeModel.DocTypeCode.ID235B
                                    strMsgCode = "00225"
                                Case DocTypeModel.DocTypeCode.REPMT
                                    strMsgCode = "00226"
                                Case DocTypeModel.DocTypeCode.VISA
                                    strMsgCode = "00227"
                                Case Else
                                    strMsgCode = "00110"
                            End Select
                        End If
                    End If
                End If
            End If

            ' -------------------------------------------------------------------------------
            ' 7. Search Temporary Account, Check Account Status
            ' -------------------------------------------------------------------------------
            Dim blnOnlyInValidAccountFound As Boolean = False
            Dim blnTempAccountNotMatchDOBFound As Boolean = False

            ' INT13-0021 - Fix use HKBC on smart IC claim incorrectly [Start][Koala]
            ' -------------------------------------------------------------------------------------
            If (udtEHSAccount Is Nothing Or enumSmartIDReadStatus = SmartIDHandler.SmartIDResultStatus.DocTypeNotExist) AndAlso strMsgCode = String.Empty Then
                ' INT13-0021 - Fix use HKBC on smart IC claim incorrectly [End][Koala]

                ' CRE20-023-67 (COVID19 - Prefill Personal Info) [Start][Winnie SUEN]
                '--------------------------------------------------------------------------
                If enumClaimMode = ClaimMode.COVID19 Then
                    Me.SearchTemporaryAccountSmartIDWithCOVID19Claim(strDocCode, strIdentityNum, dtmDOB, strExactDOB, strSchemeCode, _
                                                                     udtEHSAccount, udtEHSAccountSmartID, enumSmartIDReadStatus, _
                                                                     blnTempAccountNotMatchDOBFound, blnOnlyInValidAccountFound, strSearchDocCode)
                Else
                    'Normal claim
                    Me.SearchTemporaryAccountSmartID(strDocCode, strIdentityNum, dtmDOB, strExactDOB, udtEHSAccount, udtEHSAccountSmartID, _
                         enumSmartIDReadStatus, blnTempAccountNotMatchDOBFound, blnOnlyInValidAccountFound, strSearchDocCode)
                End If
                ' CRE20-023-67 (COVID19 - Prefill Personal Info) [End][Winnie SUEN]
            End If

            blnNoMatchRecordFound = blnOnlyInValidAccountFound Or blnTempAccountNotMatchDOBFound

            If strMsgCode = String.Empty Then
                'enumSmartIDReadStatus = BLL.SmartIDHandler.CheckSmartIDCardStatus(udtEHSAccountSmartID, udtEHSAccount)

                Select Case enumSmartIDReadStatus
                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    'Case BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_NotCreateBySmartID_NotSameDOB
                    Case BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_DiffDOB
                        strMsgCode = "00247"
                    Case BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOIDOB_CreateBySmartID_WithGender
                        'Case BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_CreateBySmartID
                        strMsgCode = "00248"
                    Case BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOIDOB_CreateBySmartID_WithoutGender_DiffName
                        strMsgCode = "00248"
                    Case BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_DiffDOI_SmallerDOI
                        strMsgCode = "00249"
                    Case BLL.SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB_WithGender, _
                        BLL.SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB_WithoutGender_DiffName
                        'Case BLL.SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB
                        strMsgCode = "00250"
                End Select
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
            End If

            ' -------------------------------------------------------------------------------
            ' 8. Create New Account
            ' -------------------------------------------------------------------------------
            If udtEHSAccount Is Nothing And strMsgCode.Trim().Equals(String.Empty) Then
                udtEHSAccount = Me.ConstructEHSTemporaryVoucherAccount(strIdentityNum, strDocCode, strExactDOB, dtmDOB, strSchemeCode, String.Empty)
            End If


            ' [2009Oct22] Performance Tuning
            Dim udtEHSTransactionBLL As New EHSTransactionBLL()
            Dim udtTranBenefitList As TransactionDetailModelCollection = Nothing

            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            ' Transaction Checking will be only apply on Vaccine scheme
            If udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVaccine Then
                ' Retrieve the Transaction Detail related to the current Scheme (may have mutil SubsidizeCodes)
                udtTranBenefitList = udtEHSTransactionBLL.getTransactionDetailBenefit(strDocCode, strIdentityNum)
            End If
            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

            ' -------------------------------------------------------------------------------
            ' 9. Check Eligible
            ' -------------------------------------------------------------------------------
            If strMsgCode.Trim() = "" Then
                Dim udtSP As ServiceProviderModel = UserACBLL.GetServiceProvider()
                Dim udtSelectedPractice As PracticeDisplayModel = (New SessionHandler).PracticeDisplayGetFromSession(strFunctionCode)
                Dim udtPractice As PracticeModel = Nothing
                If Not IsNothing(udtSelectedPractice) Then
                    udtPractice = udtSP.PracticeList(udtSelectedPractice.PracticeID)
                End If

                udtEligibleResult = Me._udtClaimRulesBLL.CheckEligibilityFromEHSClaimSearch(udtSchemeClaimModel, strIdentityNum, strDocCode.Trim(), dtmDOB, strExactDOB, dtmCurrentDate, String.Empty, udtTranBenefitList, udtPractice)
                If Not udtEligibleResult.IsEligible Then
                    strMsgCode = "00106"
                End If
            End If

            ' -------------------------------------------------------------------------------
            ' 10. Check Document Limit
            ' -------------------------------------------------------------------------------
            If strMsgCode.Trim() = "" Then
                blnExceedDocTypeLimit = Me._udtClaimRulesBLL.CheckExceedDocumentLimitFromEHSClaimSearch(strSchemeCode, strDocCode, dtmDOB, strExactDOB, dtmCurrentDate)
                If blnExceedDocTypeLimit Then
                    If strDocCode = DocTypeModel.DocTypeCode.EC Then
                        strMsgCode = "00185"
                    Else
                        strMsgCode = "00213"
                    End If
                End If
            End If

            ' -------------------------------------------------------------------------------
            ' Return
            ' -------------------------------------------------------------------------------

            If Not udtEHSAccount Is Nothing Then
                ' For UI Setting
                udtEHSAccount.SetSearchDocCode(strDocCode.Trim())
            End If

            If Not strMsgCode.Equals(String.Empty) Then
                Return New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                Return Nothing
            End If
        End Function


        Private Sub SearchTemporaryAccountSmartID(ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal dtmDOB As Date, ByVal strExactDOB As String, _
                    ByRef udtEHSAccount As EHSAccountModel, ByVal udtEHSAccountSmartID As EHSAccountModel, ByRef enumSmartIDReadStatus As SmartIDHandler.SmartIDResultStatus, _
                    ByRef blnTempAccountNotMatchDOBFound As Boolean, ByRef blnOnlyInValidAccountFound As Boolean, _
                     ByRef strSearchDocCode As String)

            Dim udtEHSAccountModelList As EHSAccountModelCollection
            Dim blnInvalidTempAccountFound As Boolean = False
            Dim udtResultEHSAccount As EHSAccountModel = Nothing

            strSearchDocCode = strDocCode.Trim()
            ' -----------------------------------------------------------------------------------
            ' Temporary Account
            ' -----------------------------------------------------------------------------------
            udtEHSAccountModelList = Me._udtEHSAccountBLL.LoadTempEHSAccountByIdentity(strIdentityNum, strDocCode)
            Me.FilterSmartIDEHSAccount(udtEHSAccountModelList, udtEHSAccountSmartID, dtmDOB, strExactDOB, enumSmartIDReadStatus, blnInvalidTempAccountFound, blnTempAccountNotMatchDOBFound, udtResultEHSAccount)

            ' -----------------------------------------------------------------------------------
            ' Special Account
            ' -----------------------------------------------------------------------------------
            udtEHSAccountModelList = Me._udtEHSAccountBLL.LoadSpecialEHSAccountByIdentity(strIdentityNum, strDocCode)
            Me.FilterSmartIDEHSAccount(udtEHSAccountModelList, udtEHSAccountSmartID, dtmDOB, strExactDOB, enumSmartIDReadStatus, blnInvalidTempAccountFound, blnTempAccountNotMatchDOBFound, udtResultEHSAccount)

            If Not udtResultEHSAccount Is Nothing Then
                udtEHSAccount = udtResultEHSAccount
            End If

            ' ----------------------------------------------------------------------------------------------------------------------
            ' Load Temporary Account with HKIC / BirthCert if not Found (HKIC <==> BirthCert)
            ' ----------------------------------------------------------------------------------------------------------------------
            If udtEHSAccount Is Nothing Then
                ' -----------------------------------------------------------------------------------
                ' Temporary Account
                ' -----------------------------------------------------------------------------------
                udtEHSAccountModelList = Nothing
                udtResultEHSAccount = Nothing

                If strDocCode = DocTypeModel.DocTypeCode.HKIC Then
                    strSearchDocCode = DocTypeModel.DocTypeCode.HKBC
                    udtEHSAccountModelList = Me._udtEHSAccountBLL.LoadTempEHSAccountByIdentity(strIdentityNum, strSearchDocCode)
                ElseIf strDocCode = DocTypeModel.DocTypeCode.HKBC Then
                    strSearchDocCode = DocTypeModel.DocTypeCode.HKIC
                    udtEHSAccountModelList = Me._udtEHSAccountBLL.LoadTempEHSAccountByIdentity(strIdentityNum, strSearchDocCode)
                End If
                Me.FilterSmartIDEHSAccount(udtEHSAccountModelList, udtEHSAccountSmartID, dtmDOB, strExactDOB, enumSmartIDReadStatus, blnInvalidTempAccountFound, blnTempAccountNotMatchDOBFound, udtResultEHSAccount)

                ' -----------------------------------------------------------------------------------
                ' Special Account
                ' -----------------------------------------------------------------------------------
                If strDocCode = DocTypeModel.DocTypeCode.HKIC Then
                    strSearchDocCode = DocTypeModel.DocTypeCode.HKBC
                    udtEHSAccountModelList = Me._udtEHSAccountBLL.LoadSpecialEHSAccountByIdentity(strIdentityNum, strSearchDocCode)
                ElseIf strDocCode = DocTypeModel.DocTypeCode.HKBC Then
                    strSearchDocCode = DocTypeModel.DocTypeCode.HKIC
                    udtEHSAccountModelList = Me._udtEHSAccountBLL.LoadSpecialEHSAccountByIdentity(strIdentityNum, strSearchDocCode)
                End If
                Me.FilterSmartIDEHSAccount(udtEHSAccountModelList, udtEHSAccountSmartID, dtmDOB, strExactDOB, enumSmartIDReadStatus, blnInvalidTempAccountFound, blnTempAccountNotMatchDOBFound, udtResultEHSAccount)

                If Not udtResultEHSAccount Is Nothing Then
                    udtEHSAccount = udtResultEHSAccount
                End If
            End If

            If udtResultEHSAccount Is Nothing Then
                ' Not Match Record Found & Invalid Record Found!
                If blnInvalidTempAccountFound Then
                    blnOnlyInValidAccountFound = True
                End If
            End If

        End Sub

        ' CRE20-023-67 (COVID19 - Prefill Personal Info) [Start][Winnie SUEN]
        '--------------------------------------------------------------------------
        ''' <summary>
        ''' Only temp account with COVID19 transaction will be returned
        ''' </summary>
        ''' <param name="strDocCode"></param>
        ''' <param name="strIdentityNum"></param>
        ''' <param name="dtmDOB"></param>
        ''' <param name="strExactDOB"></param>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="udtSearchAccountStatus"></param>
        ''' <param name="intAge"></param>
        ''' <param name="dtmDOR"></param>
        ''' <param name="strAdoptionPrefixNum"></param>
        ''' <param name="strSearchDocCode"></param>
        ''' <param name="udtEHSPersonalInfo"></param>
        ''' <remarks></remarks>
        Private Sub SearchTemporaryAccountWithCOVID19Claim(ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal dtmDOB As Date, ByVal strExactDOB As String, ByVal strSchemeCode As String, _
                                                           ByRef udtEHSAccount As EHSAccountModel, ByRef udtSearchAccountStatus As SearchAccountStatus, _
                                                           ByVal intAge As Nullable(Of Integer), ByVal dtmDOR As Nullable(Of Date), ByVal strAdoptionPrefixNum As String, ByRef strSearchDocCode As String, _
                                                           ByVal udtEHSPersonalInfo As EHSPersonalInformationModel)

            ' -----------------------------------------------------------------------------------
            ' Search Temporary Account with COVID19 transaction (C19 & MEC)
            ' -----------------------------------------------------------------------------------
            Dim udtEHSAccountModelList As New EHSAccountModelCollection
            Dim udtC19EHSAccountModelList As EHSAccountModelCollection = Me._udtEHSAccountBLL.LoadTempEHSAccountByIdentityNumSubsidize(strIdentityNum, strDocCode, SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19)
            Dim udtMECEHSAccountModelList As EHSAccountModelCollection = Me._udtEHSAccountBLL.LoadTempEHSAccountByIdentityNumSubsidize(strIdentityNum, strDocCode, SubsidizeGroupClaimModel.SubsidizeItemCodeClass.MEC)

            udtEHSAccountModelList.AddRange(udtC19EHSAccountModelList)
            udtEHSAccountModelList.AddRange(udtMECEHSAccountModelList)

            Dim udtTempEHSAccount As EHSAccountModel = Nothing
            Dim blnInvalidTempAccountFound As Boolean = False
            strSearchDocCode = strDocCode.Trim()

            ' -----------------------------------------------------------------------------------
            ' Temporary Account
            ' -----------------------------------------------------------------------------------
            For Each udtCurEHSAccount As EHSAccountModel In udtEHSAccountModelList
                ' --- Checking 1: For Claim or For Validate only ----
                If udtCurEHSAccount.AccountPurpose <> AccountPurposeClass.ForClaim AndAlso _
                        udtCurEHSAccount.AccountPurpose <> AccountPurposeClass.ForValidate Then
                    Continue For
                End If

                ' --- Checking 2: Pending For Confirmation Or Pending For Verify ----
                If udtCurEHSAccount.RecordStatus <> TempAccountRecordStatusClass.PendingConfirmation AndAlso _
                        udtCurEHSAccount.RecordStatus <> TempAccountRecordStatusClass.PendingVerify Then
                    If udtCurEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.InValid Then
                        ' Any Invalid Record Found!
                        blnInvalidTempAccountFound = True
                    End If

                    Continue For
                End If

                ' ---- Checking 3: Not X Account ----
                If Not IsNothing(udtCurEHSAccount.OriginalAccID) AndAlso udtCurEHSAccount.OriginalAccID.Trim <> String.Empty Then
                    ' X Account Found: Invalid Record Found
                    blnInvalidTempAccountFound = True

                    Continue For
                End If

                ' ---- Checking 4: With COVID19 Transaction ----
                If IsNothing(udtCurEHSAccount.TransactionID) OrElse udtCurEHSAccount.TransactionID.Trim = String.Empty Then
                    ' No COVID19 Tx Found: Invalid Record Found
                    blnInvalidTempAccountFound = True

                    Continue For
                End If

                ' ---- Checking 5: Match DOB ----
                If intAge.HasValue AndAlso dtmDOR.HasValue Then
                    ' EC Case Report Age on Date of Registration
                    If Not chkEHSAccountInputDOBMatch(udtCurEHSAccount.EHSPersonalInformationList(0), intAge.Value, dtmDOR.Value) Then
                        ' Any Temp Account Not Match DOB Found
                        udtSearchAccountStatus.TempAccountNotMatchDOBFound = True

                        Continue For
                    End If

                    If Not ChkEHSAccountInputDetailMatch(udtEHSPersonalInfo, udtCurEHSAccount.EHSPersonalInformationList(0)) Then
                        udtSearchAccountStatus.TempAccountInputDetailDiffFound = True

                        Continue For
                    End If

                Else
                    If Not chkEHSAccountInputDOBMatch(udtCurEHSAccount.EHSPersonalInformationList(0), dtmDOB, strExactDOB) Then
                        ' Any Temp Account Not Match DOB Found
                        udtSearchAccountStatus.TempAccountNotMatchDOBFound = True

                        Continue For
                    End If

                    If Not ChkEHSAccountInputDetailMatch(udtEHSPersonalInfo, udtCurEHSAccount.EHSPersonalInformationList(0)) Then
                        udtSearchAccountStatus.TempAccountInputDetailDiffFound = True

                        Continue For
                    End If

                End If

                ' ---- Checking 6: Check Latest Account ----
                Dim blnUseCurrentAccount As Boolean = False

                If udtTempEHSAccount Is Nothing Then
                    udtTempEHSAccount = udtCurEHSAccount
                    Continue For
                End If

                'Priority: Personal Info Latest Update Dtm
                If udtTempEHSAccount.EHSPersonalInformationList(0).UpdateDtm < udtCurEHSAccount.EHSPersonalInformationList(0).UpdateDtm Then
                    blnUseCurrentAccount = True
                End If

                '==================================================================================================================================================================

                If blnUseCurrentAccount Then
                    udtTempEHSAccount = udtCurEHSAccount
                End If

            Next

            ' -----------------------------------------------------------------------------------
            ' Special Account    (Skipped)
            ' -----------------------------------------------------------------------------------

            ' -----------------------------------------------------------------------------------
            ' Load Temporary Account with HKIC / BirthCert if not Found (HKIC <==> BirthCert)    (Skipped)
            ' -----------------------------------------------------------------------------------

            If Not udtTempEHSAccount Is Nothing Then
                udtEHSAccount = udtTempEHSAccount
            End If

            If udtTempEHSAccount Is Nothing Then
                ' Not Match Record Found & Invalid Record Found!
                If blnInvalidTempAccountFound Then
                    udtSearchAccountStatus.OnlyInvalidAccountFound = True
                End If
            End If

        End Sub
        ' CRE20-023-67 (COVID19 - Prefill Personal Info) [End][Winnie SUEN]


        ' CRE20-023-67 (COVID19 - Prefill Personal Info) [Start][Winnie SUEN]
        '--------------------------------------------------------------------------
        Private Sub SearchTemporaryAccountSmartIDWithCOVID19Claim(ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal dtmDOB As Date, ByVal strExactDOB As String, ByVal strSchemeCode As String, _
                                                                  ByRef udtEHSAccount As EHSAccountModel, ByVal udtEHSAccountSmartID As EHSAccountModel, ByRef enumSmartIDReadStatus As SmartIDHandler.SmartIDResultStatus, _
                                                                  ByRef blnTempAccountNotMatchDOBFound As Boolean, ByRef blnOnlyInValidAccountFound As Boolean, _
                                                                  ByRef strSearchDocCode As String)

            Dim udtEHSAccountModelList As New EHSAccountModelCollection
            Dim blnInvalidTempAccountFound As Boolean = False
            Dim udtResultEHSAccount As EHSAccountModel = Nothing
            
            strSearchDocCode = strDocCode.Trim()
            ' -----------------------------------------------------------------------------------
            ' Search Temporary Account with COVID19 transaction (C19 & MEC)
            ' -----------------------------------------------------------------------------------
            Dim udtC19EHSAccountModelList As EHSAccountModelCollection = Me._udtEHSAccountBLL.LoadTempEHSAccountByIdentityNumSubsidize(strIdentityNum, strDocCode, SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19)
            Dim udtMECEHSAccountModelList As EHSAccountModelCollection = Me._udtEHSAccountBLL.LoadTempEHSAccountByIdentityNumSubsidize(strIdentityNum, strDocCode, SubsidizeGroupClaimModel.SubsidizeItemCodeClass.MEC)

            udtEHSAccountModelList.AddRange(udtC19EHSAccountModelList)
            udtEHSAccountModelList.AddRange(udtMECEHSAccountModelList)

            Me.FilterSmartIDEHSAccountForCOVID19(udtEHSAccountModelList, udtEHSAccountSmartID, dtmDOB, strExactDOB, enumSmartIDReadStatus, blnInvalidTempAccountFound, blnTempAccountNotMatchDOBFound, udtResultEHSAccount)

            If Not udtResultEHSAccount Is Nothing Then
                udtEHSAccount = udtResultEHSAccount
            End If

            ' -----------------------------------------------------------------------------------
            ' Special Account   (Skipped)
            ' -----------------------------------------------------------------------------------

            ' ----------------------------------------------------------------------------------------------------------------------
            ' Load Temporary Account with HKIC / BirthCert if not Found (HKIC <==> BirthCert)   (Skipped)
            ' ----------------------------------------------------------------------------------------------------------------------

            If udtResultEHSAccount Is Nothing Then
                ' Not Match Record Found & Invalid Record Found!
                If blnInvalidTempAccountFound Then
                    blnOnlyInValidAccountFound = True
                End If
            End If

        End Sub
        ' CRE20-023-67 (COVID19 - Prefill Personal Info) [End][Winnie SUEN]


        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        'Public Function SearchEHSAccountSmartIDRectification(ByVal strIdentityNum As String, ByVal udtEHSAccountExist As EHSAccountModel, ByVal udtEHSAccountSmartID As EHSAccountModel, ByRef udtEligibleResult As ClaimRulesBLL.EligibleResult) As Common.ComObject.SystemMessage
        Public Function SearchEHSAccountSmartIDRectification(ByVal strIdentityNum As String, ByVal udtEHSAccountExist As EHSAccountModel, ByVal udtEHSAccountSmartID As EHSAccountModel, ByRef udtEligibleResult As ClaimRulesBLL.EligibleResult, ByRef enumSmartIDReadStatus As SmartIDHandler.SmartIDResultStatus) As Common.ComObject.SystemMessage
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

            ' -------------------------------------------------------------------------------
            ' Init
            ' -------------------------------------------------------------------------------
            Dim strDocCode As String = DocType.DocTypeModel.DocTypeCode.HKIC

            Dim strSearchDocCode As String = String.Empty
            Dim strFunctCode As String = "990000"
            Dim strSeverity As String = "E"
            Dim strMsgCode As String = String.Empty

            Dim sm As Common.ComObject.SystemMessage = Nothing

            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            'Dim enumSmartIDReadStatus As SmartIDHandler.SmartIDResultStatus
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

            Dim udtEHSAccount As EHSAccountModel

            strIdentityNum = Me._udtFormater.formatDocumentIdentityNumber(strDocCode, strIdentityNum)


            ' -------------------------------------------------------------------------------
            ' 1. Search whether Validate Account exist
            ' -------------------------------------------------------------------------------
            udtEHSAccount = Me._udtEHSAccountBLL.LoadEHSAccountByIdentity(strIdentityNum, strDocCode)
            If Not udtEHSAccount Is Nothing AndAlso strMsgCode = String.Empty Then
                ' Validate Account Found

                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                If udtEHSAccount.RecordStatus.Equals(EHSAccountModel.ValidatedAccountRecordStatusClass.Terminated) Then
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
                    strMsgCode = "00109"
                End If

                '---------------------------------------------------------------------------------------
                ' Check Smart ID Scenario
                '---------------------------------------------------------------------------------------
                If strMsgCode = String.Empty Then

                    enumSmartIDReadStatus = BLL.SmartIDHandler.CheckSmartIDCardStatus(udtEHSAccountSmartID, udtEHSAccount)

                    Select Case enumSmartIDReadStatus
                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                        ' ----------------------------------------------------------------------------------------
                        'Case BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_NotCreateBySmartID_NotSameDOB
                        Case BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_DiffDOB
                            strMsgCode = "00247"
                        Case BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOIDOB_CreateBySmartID_WithGender
                            'Case BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_CreateBySmartID
                            strMsgCode = "00248"
                        Case BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOIDOB_CreateBySmartID_WithoutGender_DiffName
                            strMsgCode = "00248"
                            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                        Case BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_DiffDOI_SmallerDOI
                            strMsgCode = "00249"
                    End Select
                End If
            End If

            ' -------------------------------------------------------------------------------
            ' 2. Search whether Temporary account exist
            ' -------------------------------------------------------------------------------
            If strMsgCode = String.Empty Then

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Only use the selected temp account to compare rather than loop all temp acct
                'Dim udtEHSAccountModelList As EHSAccountModelCollection
                'udtEHSAccountModelList = Me._udtEHSAccountBLL.LoadTempEHSAccountByIdentity(strIdentityNum, strDocCode)

                'For Each udtCurEHSAccountModel As EHSAccountModel In udtEHSAccountModelList
                '    enumSmartIDReadStatus = BLL.SmartIDHandler.CheckSmartIDCardStatus(udtEHSAccountSmartID, udtCurEHSAccountModel)

                '    Select Case enumSmartIDReadStatus
                '        Case BLL.SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB
                '            strMsgCode = "00250"
                '            Exit For

                '    End Select
                'Next

                enumSmartIDReadStatus = BLL.SmartIDHandler.CheckSmartIDCardStatus(udtEHSAccountSmartID, udtEHSAccountExist)

                Select Case enumSmartIDReadStatus
                    Case BLL.SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB_WithGender, _
                        BLL.SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB_WithoutGender_DiffName
                        'Case BLL.SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB
                        strMsgCode = "00250"
                End Select
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
            End If

            If strMsgCode = String.Empty Then
                Dim udtClaimRulesBLL As New ClaimRules.ClaimRulesBLL
                udtEHSAccountSmartID.CreateDtm = udtEHSAccountExist.CreateDtm
                ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                sm = udtClaimRulesBLL.CheckRectifyEHSAccount(udtEHSAccountSmartID.SchemeCode, strDocCode, _
                                                             udtEHSAccountSmartID, udtEligibleResult, Nothing, _
                                                             ClaimRulesBLL.Eligiblity.Check, ClaimRulesBLL.Unique.Include_Self_EHSAccount)
                ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]
            End If

            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            'If Not IsNothing(sm) Then
            If IsNothing(sm) Then
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
                If Not strMsgCode.Equals(String.Empty) Then
                    sm = New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
                End If
            End If

            Return sm
        End Function

        Private Sub FilterSmartIDEHSAccount(ByVal udtEHSAccountModelList As EHSAccountModelCollection, ByVal udtEHSAccountSmartID As EHSAccountModel, ByVal dtmDOB As Date, ByVal strExactDOB As String, _
                ByRef enumSmartIDReadStatus As SmartIDHandler.SmartIDResultStatus, ByRef blnInvalidTempAccountFound As Boolean, ByRef blnTempAccountNotMatchDOBFound As Boolean, ByRef udtTempEHSAccount As EHSAccountModel)

            Dim enumTempSmartIDReadStatus As SmartIDHandler.SmartIDResultStatus
            Dim blnCheckedBySmartID As Boolean = False
            Dim blnCheckEHSAccount As Boolean = False

            If Not udtEHSAccountModelList Is Nothing AndAlso udtEHSAccountModelList.Count > 0 Then


                For Each udtCurEHSAccount As EHSAccountModel In udtEHSAccountModelList
                    ' Temporary Account
                    If (udtCurEHSAccount.AccountPurpose = EHSAccountModel.AccountPurposeClass.ForClaim Or _
                        udtCurEHSAccount.AccountPurpose = EHSAccountModel.AccountPurposeClass.ForValidate) Then

                        ' Pending For Confirmation Or Pending For Verify
                        If (udtCurEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingConfirmation Or _
                            udtCurEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingVerify) Then

                            ' Not X Account
                            If (udtCurEHSAccount.OriginalAccID Is Nothing OrElse udtCurEHSAccount.OriginalAccID.Trim() = "") Then

                                enumTempSmartIDReadStatus = BLL.SmartIDHandler.CheckSmartIDCardStatus(udtEHSAccountSmartID, udtCurEHSAccount)

                                If Not blnCheckedBySmartID Then
                                    If Not IsNothing(udtCurEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKIC)) Then
                                        blnCheckedBySmartID = udtCurEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKIC).CreateBySmartID
                                    Else
                                        blnCheckEHSAccount = True
                                    End If
                                End If

                                Select Case enumTempSmartIDReadStatus
                                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                                    ' ----------------------------------------------------------------------------------------
                                    Case BLL.SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB_WithGender, _
                                        BLL.SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB_WithoutGender_DiffName
                                        'Case BLL.SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB
                                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
                                        'Block case
                                        enumSmartIDReadStatus = enumTempSmartIDReadStatus
                                        blnCheckEHSAccount = False

                                        Return

                                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                                        ' ----------------------------------------------------------------------------------------
                                    Case BLL.SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB_WithoutGender_SameName
                                        'Account is created by smartID, TempEHSAccount can be override
                                        blnCheckEHSAccount = True
                                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                                    Case BLL.SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_DiffDOIDOB
                                        'Account is created by smartID, TempEHSAccount can be override
                                        blnCheckEHSAccount = True

                                    Case BLL.SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_NotCreateBySmartID
                                        'If one of the EHS Account is created by smartID, system will not check again
                                        If Not blnCheckedBySmartID Then
                                            blnCheckEHSAccount = True
                                        Else
                                            blnCheckEHSAccount = False
                                        End If

                                    Case BLL.SmartIDHandler.SmartIDResultStatus.TempAccountExist_SameDetail

                                        If udtCurEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKIC).CreateBySmartID Then
                                            'enumSmartIDReadStatus = enumTempSmartIDReadStatus
                                            blnCheckEHSAccount = True
                                        Else
                                            If Not blnCheckedBySmartID Then
                                                'enumSmartIDReadStatus = enumTempSmartIDReadStatus
                                                blnCheckEHSAccount = True
                                            Else
                                                blnCheckEHSAccount = False
                                            End If

                                        End If
                                End Select

                                If blnCheckEHSAccount Then

                                    If udtTempEHSAccount Is Nothing Then
                                        enumSmartIDReadStatus = enumTempSmartIDReadStatus
                                        udtTempEHSAccount = udtCurEHSAccount
                                    Else
                                        If udtTempEHSAccount.EHSPersonalInformationList(0).DocCode.Trim.Equals(DocType.DocTypeModel.DocTypeCode.HKIC) AndAlso _
                                           udtCurEHSAccount.EHSPersonalInformationList(0).DocCode.Trim.Equals(DocType.DocTypeModel.DocTypeCode.HKIC) Then

                                            If udtTempEHSAccount.EHSPersonalInformationList(0).CreateBySmartID Then
                                                If udtCurEHSAccount.EHSPersonalInformationList(0).CreateBySmartID Then

                                                    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
                                                    ' ---------------------------------------------------------------------------------------------------------
                                                    ' Get latest version
                                                    Dim strExistSmartIDVer As String = Replace(udtTempEHSAccount.EHSPersonalInformationList(0).SmartIDVer, "C", "")
                                                    Dim strCurrentSmartIDVer As String = Replace(udtCurEHSAccount.EHSPersonalInformationList(0).SmartIDVer, "C", "")
                                                    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

                                                    If SmartIDHandler.IsExistingOldSmartIDVersion(strExistSmartIDVer, strCurrentSmartIDVer) Then
                                                        enumSmartIDReadStatus = enumTempSmartIDReadStatus
                                                        udtTempEHSAccount = udtCurEHSAccount

                                                    ElseIf SmartIDHandler.CompareVersion(strExistSmartIDVer, strCurrentSmartIDVer, "=") Then
                                                        ' Get latest create date when same version
                                                        If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                                                            enumSmartIDReadStatus = enumTempSmartIDReadStatus
                                                            udtTempEHSAccount = udtCurEHSAccount
                                                        End If
                                                    End If

                                                End If
                                            Else
                                                If udtCurEHSAccount.EHSPersonalInformationList(0).CreateBySmartID Then
                                                    enumSmartIDReadStatus = enumTempSmartIDReadStatus
                                                    udtTempEHSAccount = udtCurEHSAccount
                                                Else
                                                    If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                                                        enumSmartIDReadStatus = enumTempSmartIDReadStatus
                                                        udtTempEHSAccount = udtCurEHSAccount
                                                    End If
                                                End If
                                            End If
                                        Else
                                            If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                                                enumSmartIDReadStatus = enumTempSmartIDReadStatus
                                                udtTempEHSAccount = udtCurEHSAccount
                                            End If
                                        End If

                                        'If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                                        '    enumSmartIDReadStatus = enumTempSmartIDReadStatus
                                        '    udtTempEHSAccount = udtCurEHSAccount
                                        'End If
                                    End If

                                    If Not Me.chkEHSAccountInputDOBMatch(udtCurEHSAccount.EHSPersonalInformationList(0), dtmDOB, strExactDOB) Then
                                        ' Any Temp Account Not Match DOB Found
                                        blnTempAccountNotMatchDOBFound = True
                                    End If

                                End If
                            Else
                                ' X Account Found: Invalid Record Found
                                blnInvalidTempAccountFound = True
                            End If
                        ElseIf udtCurEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.InValid Then
                            ' Any Invalid Record Found!
                            blnInvalidTempAccountFound = True
                        End If
                    End If
                Next
            Else
                Return
            End If
        End Sub

        ' CRE20-023-67 (COVID19 - Prefill Personal Info) [Start][Winnie SUEN]
        '--------------------------------------------------------------------------
        Private Sub FilterSmartIDEHSAccountForCOVID19(ByVal udtEHSAccountModelList As EHSAccountModelCollection, ByVal udtEHSAccountSmartID As EHSAccountModel, ByVal dtmDOB As Date, ByVal strExactDOB As String, _
                                                      ByRef enumSmartIDReadStatus As SmartIDHandler.SmartIDResultStatus, ByRef blnInvalidTempAccountFound As Boolean, ByRef blnTempAccountNotMatchDOBFound As Boolean, ByRef udtTempEHSAccount As EHSAccountModel)

            Dim enumTempSmartIDReadStatus As SmartIDHandler.SmartIDResultStatus
            'Dim blnCheckedBySmartID As Boolean = False
            Dim blnCheckEHSAccount As Boolean = True

            If Not udtEHSAccountModelList Is Nothing AndAlso udtEHSAccountModelList.Count > 0 Then


                For Each udtCurEHSAccount As EHSAccountModel In udtEHSAccountModelList

                    ' Temporary Account

                    ' --- Checking 1: For Claim or For Validate only ----
                    If (udtCurEHSAccount.AccountPurpose <> EHSAccountModel.AccountPurposeClass.ForClaim AndAlso _
                        udtCurEHSAccount.AccountPurpose <> EHSAccountModel.AccountPurposeClass.ForValidate) Then
                        Continue For
                    End If

                    ' --- Checking 2: Pending For Confirmation Or Pending For Verify ----
                    If (udtCurEHSAccount.RecordStatus <> EHSAccountModel.TempAccountRecordStatusClass.PendingConfirmation AndAlso _
                        udtCurEHSAccount.RecordStatus <> EHSAccountModel.TempAccountRecordStatusClass.PendingVerify) Then
                        If udtCurEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.InValid Then
                            ' Any Invalid Record Found!
                            blnInvalidTempAccountFound = True
                        End If

                        Continue For
                    End If

                    ' ---- Checking 3: Not X Account ----
                    If Not IsNothing(udtCurEHSAccount.OriginalAccID) AndAlso udtCurEHSAccount.OriginalAccID.Trim <> String.Empty Then
                        ' X Account Found: Invalid Record Found
                        blnInvalidTempAccountFound = True

                        Continue For
                    End If

                    ' ---- Checking 4: With COVID19 Transaction ----
                    If IsNothing(udtCurEHSAccount.TransactionID) OrElse udtCurEHSAccount.TransactionID.Trim = String.Empty Then
                        ' No COVID19 Tx Found: Invalid Record Found
                        blnInvalidTempAccountFound = True

                        Continue For
                    End If

                    ' ---- Checking 5: Match Smart ID Status ----
                    enumTempSmartIDReadStatus = BLL.SmartIDHandler.CheckSmartIDCardStatus(udtEHSAccountSmartID, udtCurEHSAccount)

                    'If Not blnCheckedBySmartID Then
                    '    If Not IsNothing(udtCurEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKIC)) Then
                    '        blnCheckedBySmartID = udtCurEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKIC).CreateBySmartID
                    '    Else
                    '        blnCheckEHSAccount = True
                    '    End If
                    'End If

                    Select Case enumTempSmartIDReadStatus

                        Case BLL.SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB_WithGender, _
                            BLL.SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB_WithoutGender_DiffName
                            ' Account created by Smart IC + Same DOI + Diff Info => Invalid Card (Block Case)
                            enumSmartIDReadStatus = enumTempSmartIDReadStatus
                            'blnCheckEHSAccount = False

                            Return

                            'Case BLL.SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB_WithoutGender_SameName
                            '    'Account is created by smartID, TempEHSAccount can be override
                            '    blnCheckEHSAccount = True

                            'Case BLL.SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_DiffDOIDOB
                            '    'Account is created by smartID, TempEHSAccount can be override
                            '    blnCheckEHSAccount = True

                            'Case BLL.SmartIDHandler.SmartIDResultStatus.TempAccountExist_DiffDetail_NotCreateBySmartID
                            '    'If one of the EHS Account is created by smartID, system will not check again
                            '    If Not blnCheckedBySmartID Then
                            '        blnCheckEHSAccount = True
                            '    Else
                            '        blnCheckEHSAccount = False
                            '    End If

                            'Case BLL.SmartIDHandler.SmartIDResultStatus.TempAccountExist_SameDetail

                            '    If udtCurEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKIC).CreateBySmartID Then
                            '        blnCheckEHSAccount = True
                            '    Else
                            '        If Not blnCheckedBySmartID Then
                            '            blnCheckEHSAccount = True
                            '        Else
                            '            blnCheckEHSAccount = False
                            '        End If

                            '    End If
                    End Select

                    ' ---- Checking 6: Match DOB ----
                    If blnCheckEHSAccount Then
                        If Not Me.chkEHSAccountInputDOBMatch(udtCurEHSAccount.EHSPersonalInformationList(0), dtmDOB, strExactDOB) Then
                            ' Any Temp Account Not Match DOB Found
                            blnTempAccountNotMatchDOBFound = True
                            'Continue For   ' Use the temp account for prefill purpose even DOB not match
                        End If
                    End If

                    ' ---- Checking 7: Check Latest Account ----
                    Dim blnUseCurrentAccount As Boolean = False

                    If blnCheckEHSAccount Then
                        If udtTempEHSAccount Is Nothing Then
                            blnUseCurrentAccount = True
                            enumSmartIDReadStatus = enumTempSmartIDReadStatus
                            udtTempEHSAccount = udtCurEHSAccount
                            Continue For
                        End If
                    Else
                        Continue For
                    End If

                    'Priority: Personal Info Latest Update Dtm
                    If udtTempEHSAccount.EHSPersonalInformationList(0).UpdateDtm < udtCurEHSAccount.EHSPersonalInformationList(0).UpdateDtm Then
                        blnUseCurrentAccount = True
                    End If

                    If blnUseCurrentAccount Then
                        enumSmartIDReadStatus = enumTempSmartIDReadStatus
                        udtTempEHSAccount = udtCurEHSAccount
                    End If

                Next
            Else
                Return
            End If
        End Sub
        '==================================================================================================================================================================
        ' CRE20-023-67 (COVID19 - Prefill Personal Info) [End][Winnie SUEN]
#End Region

#Region "Insert / Update EHS Account / EHSTransaction"

        ''' <summary>
        ''' Create Temporary EHS Account, Save to Database
        ''' </summary>
        ''' <param name="udtSP"></param>
        ''' <param name="udtDataEntry"></param>
        ''' <param name="udtEHSAccount"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateTemporaryEHSAccount(ByVal udtSP As ServiceProviderModel, _
                                                  ByVal udtDataEntry As DataEntryUserModel, _
                                                  ByVal udtEHSAccount As EHSAccountModel, _
                                                  Optional ByVal udtDB As Database = Nothing) As Common.ComObject.SystemMessage

            Dim udtErrorMsg As Common.ComObject.SystemMessage = Nothing
            udtEHSAccount.AccountPurpose = EHSAccountModel.AccountPurposeClass.ForClaim

            If udtDataEntry Is Nothing Then
                udtEHSAccount.DataEntryBy = String.Empty
                udtEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingVerify
                udtEHSAccount.EHSPersonalInformationList(0).DataEntryBy = String.Empty
            Else
                udtEHSAccount.DataEntryBy = udtDataEntry.DataEntryAccount
                udtEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingConfirmation
                udtEHSAccount.EHSPersonalInformationList(0).DataEntryBy = udtDataEntry.DataEntryAccount
            End If

            ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
            If Not (New DocTypeBLL).getAllDocType.Filter(udtEHSAccount.EHSPersonalInformationList(0).DocCode).IMMDorManualValidationAvailable Then
                udtEHSAccount.RecordStatus = TempAccountRecordStatusClass.NotForImmDValidation
            End If
            ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]

            udtEHSAccount.CreateSPID = udtSP.SPID
            udtEHSAccount.CreateBy = udtSP.SPID
            udtEHSAccount.EHSPersonalInformationList(0).RecordStatus = "N"
            udtEHSAccount.EHSPersonalInformationList(0).CreateBy = udtSP.SPID
            ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            udtEHSAccount.SubsidizeWriteOff_CreateReason = eHASubsidizeWriteOff_CreateReason.PersonalInfoCreation
            ' CRE13-006 - HCVS Ceiling [End][Tommy L]

            ' UI Handle
            'udtEHSAccount.CreateSPPracticeDisplaySeq = intPracticeID

            Dim blnLocalDB As Boolean

            If udtDB Is Nothing Then
                udtDB = New Database()
                blnLocalDB = True
            Else
                blnLocalDB = False
            End If

            Try
                If blnLocalDB = True Then
                    udtDB.BeginTransaction()
                End If

                udtErrorMsg = Me._udtEHSAccountBLL.InsertEHSAccount(udtDB, udtEHSAccount)

                If udtErrorMsg Is Nothing Then
                    If blnLocalDB = True Then
                        udtDB.CommitTransaction()
                    End If
                Else
                    If blnLocalDB = True Then
                        udtDB.RollBackTranscation()
                    End If
                End If
                Return udtErrorMsg

            Catch eSQL As SqlException
                If blnLocalDB = True Then
                    udtDB.RollBackTranscation()
                End If
                Throw eSQL
            Catch ex As Exception
                If blnLocalDB = True Then
                    udtDB.RollBackTranscation()
                End If
                Throw
            End Try

            Return udtErrorMsg
        End Function

        Public Function CreateEHSTransaction(ByVal udtEHSTransactionModel As EHSTransactionModel, ByVal udtEHSAccountModel As EHSAccountModel, _
            ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, ByVal udtSchemeClaimModel As SchemeClaimModel, _
            ByVal enumAppSource As EHSTransactionModel.AppSourceEnum, Optional ByVal blnAutoConfirm As Boolean = False) As Common.ComObject.SystemMessage

            ' 1. Claim By Validated Account:
            '   -> Insert Transaction, no need to modify the Validated Account

            ' 2. Claim By Newly Create Temp Acocunt (Saved to Database and retrieve to use)

            ' 3. Claim by Confirm Temporary Account, same SP and Temporary Account have no Transaction, append to end.

            ' 4. Claim by Confirm Temporary Account, same SP and create by Dataentry and Temporary Account have no Transaction, append to end and auto confirm.

            ' ----------------------------------------------------------------------------------------------
            ' If Auto Confirm, update Record_Status = Pending Validation, TSMP of udtEHSAccount will be checked
            ' ----------------------------------------------------------------------------------------------
            ' ----------------------------------------------------------------------------------------------
            ' If Other Temporary EHS Account Sceniro, Check TSMP of udtEHSAccount to protect the Temporary EHS Account still exist
            ' ----------------------------------------------------------------------------------------------

            Dim udtDB As New Database()
            Try
                udtDB.BeginTransaction()

                Dim udtSystemMessage As Common.ComObject.SystemMessage = Me._udtEHSTransactionBLL.InsertEHSTransaction(udtDB, udtEHSTransactionModel, udtEHSAccountModel, udtEHSPersonalInfo, udtSchemeClaimModel, enumAppSource)

                If udtSystemMessage Is Nothing Then
                    If blnAutoConfirm Then
                        Me._udtEHSAccountBLL.UpdateTempEHSAccountConfirm(udtDB, udtEHSAccountModel, udtEHSTransactionModel.CreateBy, DateTime.Now)
                    Else

                        If udtEHSAccountModel.AccountSource = EHSAccountModel.SysAccountSource.TemporaryAccount Then
                            Me._udtEHSAccountBLL.CheckTempEHSAccountTSMP(udtDB, udtEHSAccountModel.VoucherAccID, udtEHSAccountModel.TSMP)
                        End If
                    End If
                End If

                If udtSystemMessage Is Nothing Then
                    udtDB.CommitTransaction()
                Else
                    udtDB.RollBackTranscation()
                End If

                Return udtSystemMessage

            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                Throw eSQL
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try
        End Function

        ''' <summary>
        ''' Claim with Validated Account or Temporary Account
        ''' </summary>
        ''' <param name="udtEHSTransactionModel"></param>
        ''' <param name="udtEHSAccountModel"></param>
        ''' <param name="udtEHSPersonalInfo"></param>
        ''' <param name="udtSchemeClaimModel"></param>
        ''' <param name="blnAutoConfirm"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateEHSTransaction(ByVal udtEHSTransactionModel As EHSTransactionModel, ByVal udtEHSAccountModel As EHSAccountModel, _
            ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, ByVal udtSchemeClaimModel As SchemeClaimModel, _
            Optional ByVal blnAutoConfirm As Boolean = False) As Common.ComObject.SystemMessage

            ' 1. Claim By Validated Account:
            '   -> Insert Transaction, no need to modify the Validated Account

            ' 2. Claim By Newly Create Temp Acocunt (Saved to Database and retrieve to use)

            ' 3. Claim by Confirm Temporary Account, same SP and Temporary Account have no Transaction, append to end.

            ' 4. Claim by Confirm Temporary Account, same SP and create by Dataentry and Temporary Account have no Transaction, append to end and auto confirm.

            ' ----------------------------------------------------------------------------------------------
            ' If Auto Confirm, update Record_Status = Pending Validation, TSMP of udtEHSAccount will be checked
            ' ----------------------------------------------------------------------------------------------
            ' ----------------------------------------------------------------------------------------------
            ' If Other Temporary EHS Account Sceniro, Check TSMP of udtEHSAccount to protect the Temporary EHS Account still exist
            ' ----------------------------------------------------------------------------------------------

            Dim udtDB As New Database()
            Try
                udtDB.BeginTransaction()

                Dim udtSystemMessage As Common.ComObject.SystemMessage = Me._udtEHSTransactionBLL.InsertEHSTransaction(udtDB, udtEHSTransactionModel, udtEHSAccountModel, udtEHSPersonalInfo, udtSchemeClaimModel)

                If udtSystemMessage Is Nothing Then
                    If blnAutoConfirm Then
                        Me._udtEHSAccountBLL.UpdateTempEHSAccountConfirm(udtDB, udtEHSAccountModel, udtEHSTransactionModel.CreateBy, DateTime.Now)
                    Else

                        If udtEHSAccountModel.AccountSource = EHSAccountModel.SysAccountSource.TemporaryAccount Then
                            Me._udtEHSAccountBLL.CheckTempEHSAccountTSMP(udtDB, udtEHSAccountModel.VoucherAccID, udtEHSAccountModel.TSMP)
                        End If
                    End If
                End If

                If udtSystemMessage Is Nothing Then
                    udtDB.CommitTransaction()
                Else
                    udtDB.RollBackTranscation()
                End If

                Return udtSystemMessage

            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                Throw eSQL
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try
        End Function

        Public Function CreateXEHSAccountEHSTransaction(ByVal udtSP As ServiceProviderModel, ByVal udtDataEntry As DataEntryUserModel, _
            ByVal udtEHSTransactionModel As EHSTransactionModel, ByVal udtEHSAccountModel As EHSAccountModel, _
            ByVal udtSchemeClaimModel As SchemeClaimModel, ByVal enumAppSource As EHSTransactionModel.AppSourceEnum) As Common.ComObject.SystemMessage

            ' 3. Claim By X Account, [Insert X Account & Insert Transaction]

            udtEHSTransactionModel.TempVoucherAccID = udtEHSAccountModel.VoucherAccID

            If udtDataEntry Is Nothing Then
                udtEHSAccountModel.DataEntryBy = String.Empty
                udtEHSAccountModel.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingVerify
                udtEHSAccountModel.EHSPersonalInformationList(0).DataEntryBy = String.Empty
            Else
                udtEHSAccountModel.DataEntryBy = udtDataEntry.DataEntryAccount
                udtEHSAccountModel.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingConfirmation
                udtEHSAccountModel.EHSPersonalInformationList(0).DataEntryBy = udtDataEntry.DataEntryAccount
            End If

            ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
            If Not (New DocTypeBLL).getAllDocType.Filter(udtEHSAccountModel.EHSPersonalInformationList(0).DocCode).IMMDorManualValidationAvailable Then
                udtEHSAccountModel.RecordStatus = TempAccountRecordStatusClass.NotForImmDValidation
            End If
            ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]

            udtEHSAccountModel.CreateSPID = udtSP.SPID
            udtEHSAccountModel.CreateBy = udtSP.SPID
            udtEHSAccountModel.EHSPersonalInformationList(0).RecordStatus = "N"
            udtEHSAccountModel.EHSPersonalInformationList(0).CreateBy = udtSP.SPID

            'X Account Practice ID and Scheme Code must same as transaction
            udtEHSAccountModel.CreateSPPracticeDisplaySeq = udtEHSTransactionModel.PracticeID
            udtEHSAccountModel.SchemeCode = udtEHSTransactionModel.SchemeCode

            Dim udtDB As New Database()
            Try
                udtDB.BeginTransaction()
                'CRE13-006 HCVS Ceiling [Start][Karl]
                'Dim udtErrorMsg As Common.ComObject.SystemMessage = Me._udtEHSTransactionBLL.InsertEHSTransaction(udtDB, udtEHSTransactionModel, udtEHSAccountModel, udtEHSAccountModel.EHSPersonalInformationList(0), udtSchemeClaimModel, enumAppSource)

                'If udtErrorMsg Is Nothing Then
                '    udtErrorMsg = Me._udtEHSAccountBLL.InsertEHSAccount(udtDB, udtEHSAccountModel)
                'End If


                Dim udtErrorMsg As Common.ComObject.SystemMessage = Me._udtEHSAccountBLL.InsertEHSAccount(udtDB, udtEHSAccountModel)

                If udtErrorMsg Is Nothing Then
                    udtErrorMsg = Me._udtEHSTransactionBLL.InsertEHSTransaction(udtDB, udtEHSTransactionModel, udtEHSAccountModel, udtEHSAccountModel.EHSPersonalInformationList(0), udtSchemeClaimModel, enumAppSource)
                End If
                'CRE13-006 HCVS Ceiling [Start][End]

                If udtErrorMsg Is Nothing Then
                    udtDB.CommitTransaction()
                Else
                    udtDB.RollBackTranscation()
                End If
                Return udtErrorMsg

            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                Throw eSQL
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try

        End Function

        ''' <summary>
        ''' Claim with X Account, Insert X Account + Insert Transaction
        ''' </summary>
        ''' <param name="udtEHSTransactionModel"></param>
        ''' <param name="udtEHSAccountModel"></param>
        ''' <param name="udtSchemeClaimModel"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateXEHSAccountEHSTransaction(ByVal udtSP As ServiceProviderModel, ByVal udtDataEntry As DataEntryUserModel, _
            ByVal udtEHSTransactionModel As EHSTransactionModel, ByVal udtEHSAccountModel As EHSAccountModel, _
            ByVal udtSchemeClaimModel As SchemeClaimModel) As Common.ComObject.SystemMessage

            ' 3. Claim By X Account, [Insert X Account & Insert Transaction]

            udtEHSTransactionModel.TempVoucherAccID = udtEHSAccountModel.VoucherAccID

            If udtDataEntry Is Nothing Then
                udtEHSAccountModel.DataEntryBy = String.Empty
                udtEHSAccountModel.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingVerify
                udtEHSAccountModel.EHSPersonalInformationList(0).DataEntryBy = String.Empty
            Else
                udtEHSAccountModel.DataEntryBy = udtDataEntry.DataEntryAccount
                udtEHSAccountModel.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingConfirmation
                udtEHSAccountModel.EHSPersonalInformationList(0).DataEntryBy = udtDataEntry.DataEntryAccount
            End If

            ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
            If Not (New DocTypeBLL).getAllDocType.Filter(udtEHSAccountModel.EHSPersonalInformationList(0).DocCode).IMMDorManualValidationAvailable Then
                udtEHSAccountModel.RecordStatus = TempAccountRecordStatusClass.NotForImmDValidation
            End If
            ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]

            udtEHSAccountModel.CreateSPID = udtSP.SPID
            udtEHSAccountModel.CreateBy = udtSP.SPID
            udtEHSAccountModel.EHSPersonalInformationList(0).RecordStatus = "N"
            udtEHSAccountModel.EHSPersonalInformationList(0).CreateBy = udtSP.SPID

            'X Account Practice ID and Scheme Code must same as transaction
            udtEHSAccountModel.CreateSPPracticeDisplaySeq = udtEHSTransactionModel.PracticeID
            udtEHSAccountModel.SchemeCode = udtEHSTransactionModel.SchemeCode

            Dim udtDB As New Database()
            Try
                udtDB.BeginTransaction()

                'CRE13-006 HCVS Ceiling [Start][Karl]
                'Dim udtErrorMsg As Common.ComObject.SystemMessage = Me._udtEHSTransactionBLL.InsertEHSTransaction(udtDB, udtEHSTransactionModel, udtEHSAccountModel, udtEHSAccountModel.EHSPersonalInformationList(0), udtSchemeClaimModel)

                'If udtErrorMsg Is Nothing Then
                '    udtErrorMsg = Me._udtEHSAccountBLL.InsertEHSAccount(udtDB, udtEHSAccountModel)
                'End If

                Dim udtErrorMsg As Common.ComObject.SystemMessage = Me._udtEHSAccountBLL.InsertEHSAccount(udtDB, udtEHSAccountModel)

                If udtErrorMsg Is Nothing Then
                    udtErrorMsg = Me._udtEHSTransactionBLL.InsertEHSTransaction(udtDB, udtEHSTransactionModel, udtEHSAccountModel, udtEHSAccountModel.EHSPersonalInformationList(0), udtSchemeClaimModel)
                End If

                'CRE13-006 HCVS Ceiling [Start][End]

                If udtErrorMsg Is Nothing Then
                    udtDB.CommitTransaction()
                Else
                    udtDB.RollBackTranscation()
                End If
                Return udtErrorMsg

            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                Throw eSQL
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try

        End Function

        Public Function CreateRectifyAccount(ByVal udtSP As ServiceProviderModel, _
                                             ByVal udtDataEntry As DataEntryUserModel, _
                                             ByVal udtEHSXAccount As EHSAccountModel, _
                                             ByVal udtEHSNewAccount As EHSAccountModel, _
                                             Optional ByVal udtDB As Database = Nothing) As Common.ComObject.SystemMessage
            'Private Sub UpdateTempEHSAccountRectify(ByVal udtEHSAccount As EHSAccountModel, ByVal strUpdateBy As String, ByVal dtmUpdate As DateTime)
            Dim udtSubsidizeWriteOffBLL As New SubsidizeWriteOffBLL
            'CRE13-006 HCVS Ceiling [End][Karl]
            '' Rectify X Account, remove X Account and make a new EHSAccount

            If udtDataEntry Is Nothing Then
                udtEHSNewAccount.DataEntryBy = String.Empty
                udtEHSNewAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingVerify
                udtEHSNewAccount.EHSPersonalInformationList(0).DataEntryBy = String.Empty

                udtEHSNewAccount.CreateBy = udtSP.SPID
                udtEHSNewAccount.EHSPersonalInformationList(0).CreateBy = udtSP.SPID
                udtEHSNewAccount.CreateSPID = udtSP.SPID
            Else
                udtEHSNewAccount.DataEntryBy = udtDataEntry.DataEntryAccount
                udtEHSNewAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingConfirmation
                udtEHSNewAccount.EHSPersonalInformationList(0).DataEntryBy = udtDataEntry.DataEntryAccount

                udtEHSNewAccount.CreateBy = udtDataEntry.SPID
                udtEHSNewAccount.EHSPersonalInformationList(0).CreateBy = udtDataEntry.SPID
                udtEHSNewAccount.CreateSPID = udtDataEntry.SPID
            End If

            ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
            If Not (New DocTypeBLL).getAllDocType.Filter(udtEHSNewAccount.EHSPersonalInformationList(0).DocCode).IMMDorManualValidationAvailable Then
                udtEHSNewAccount.RecordStatus = TempAccountRecordStatusClass.NotForImmDValidation
            End If
            ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]

            Dim blnLocalDB As Boolean

            If udtDB Is Nothing Then
                udtDB = New Database()
                blnLocalDB = True
            Else
                blnLocalDB = False
            End If

            Try
                If blnLocalDB = True Then
                    udtDB.BeginTransaction()
                End If

                ' Since X Account removed, checking will ignore X Account
                Me._udtEHSAccountBLL.UpdateTempEHSAccountRecordStatus(udtDB, udtEHSXAccount, udtSP.SPID, EHSAccountModel.TempAccountRecordStatusClass.Removed, DateTime.Now)

                'CRE13-006 HCVS Ceiling [Start][Karl]
                Me._udtEHSTransactionBLL.UpdateTransactionWithNewTemporaryAccount(udtDB, udtEHSXAccount.TransactionID, udtEHSXAccount.VoucherAccID, udtEHSNewAccount.VoucherAccID, _
                udtEHSNewAccount.EHSPersonalInformationList(0).DocCode, udtEHSNewAccount.EHSPersonalInformationList(0).IdentityNum, udtEHSNewAccount.CreateBy)
                'CRE13-006 HCVS Ceiling [Start][Karl]

                Dim udtErrorMsg As Common.ComObject.SystemMessage = Me._udtEHSAccountBLL.InsertEHSAccount(udtDB, udtEHSNewAccount)

                If udtErrorMsg Is Nothing Then
                    'handle write off
                    Call udtSubsidizeWriteOffBLL.DeleteAccountWriteOff(udtEHSXAccount.EHSPersonalInformationList, eHASubsidizeWriteOff_CreateReason.PersonalInfoAmend, udtDB)
                End If

                If udtErrorMsg Is Nothing Then
                    If blnLocalDB = True Then
                        udtDB.CommitTransaction()
                    End If
                Else
                    If blnLocalDB = True Then
                        udtDB.RollBackTranscation()
                    End If
                End If
                Return udtErrorMsg

            Catch eSQL As SqlException
                If blnLocalDB = True Then
                    udtDB.RollBackTranscation()
                End If
                Throw eSQL
            Catch ex As Exception
                If blnLocalDB = True Then
                    udtDB.RollBackTranscation()
                End If
                Throw
            End Try
        End Function

        '==================================================================== Code for SmartID ============================================================================
        Public Function CreateAmendEHSAccount(ByVal udtSP As ServiceProviderModel, ByVal udtDataEntry As DataEntryUserModel, _
                    ByVal udtEHSAccountModel_Validated As EHSAccountModel, ByRef udtEHSAccountModel_Amend As EHSAccountModel, _
                    ByVal strSchemeCode As String, ByVal intPracticeDisplaySeq As Integer, ByVal blnNeedImmDValidation As Boolean) As EHSAccountModel

            '-----  Amend EHS Account Step -----'
            '   1. Create 2 new temp EHSAccount in table "TempVoucherAccount", 
            '   2. Create 2 temp EHSPersonalInformation in table "TempPersonalInformation"
            '       => One Temp EHSAccount account purpose = 'O' (Original Record)
            '          with original personal information
            '       => Another Temp EHSAccount account purpose = 'A' (Amendment Record)
            '          with amending personal information
            '   3. if the creation is mabe by SP
            '       => Add one record in table 'PersonalInfoAmendHistory'
            '       => if the amendment is no need to send to ImmD for verification
            '           => Call the 'ValidateAccountEHSModelWithoutImmDValidation' to merge
            '              the Temp EHSAccount account purpose = 'A' to validated EHSAccount
            ' --------------------------------------------------------

            Dim udtGF As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
            Dim strDocCode As String = udtEHSAccountModel_Amend.EHSPersonalInformationList(0).DocCode.Trim
            Dim udtEHSAccountTemp_O As EHSAccountModel
            Dim udtEHSAccountTemp_A As EHSAccountModel
            Dim udtEHSAccountBLL As EHSAccountBLL = New EHSAccountBLL()
            'Dim udtErrorMsg As Common.ComObject.SystemMessage = Nothing

            Dim blnCreateDE As Boolean = False

            udtEHSAccountTemp_O = udtEHSAccountModel_Validated.CloneDataForAmendmentOld(strDocCode, False)
            udtEHSAccountTemp_A = udtEHSAccountModel_Amend.CloneDataForAmendment(strDocCode, False)

            udtEHSAccountTemp_O.VoucherAccID = udtGF.generateSystemNum("C")
            udtEHSAccountTemp_A.VoucherAccID = udtGF.generateSystemNum("C")

            udtEHSAccountTemp_A.OriginalAmendAccID = udtEHSAccountTemp_O.VoucherAccID
            udtEHSAccountTemp_A.ValidatedAccID = udtEHSAccountModel_Validated.VoucherAccID

            If udtDataEntry Is Nothing Then
                udtEHSAccountTemp_O.DataEntryBy = String.Empty
                udtEHSAccountTemp_A.DataEntryBy = String.Empty

                udtEHSAccountTemp_O.EHSPersonalInformationList(0).DataEntryBy = String.Empty
                udtEHSAccountTemp_A.EHSPersonalInformationList(0).DataEntryBy = String.Empty

            Else
                udtEHSAccountTemp_O.DataEntryBy = udtDataEntry.DataEntryAccount
                udtEHSAccountTemp_A.DataEntryBy = udtDataEntry.DataEntryAccount

                udtEHSAccountTemp_O.EHSPersonalInformationList(0).DataEntryBy = udtDataEntry.DataEntryAccount
                udtEHSAccountTemp_A.EHSPersonalInformationList(0).DataEntryBy = udtDataEntry.DataEntryAccount

                udtEHSAccountTemp_O.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingConfirmation
                udtEHSAccountTemp_A.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingConfirmation

                blnCreateDE = True
            End If

            udtEHSAccountTemp_O.CreateSPID = udtSP.SPID
            udtEHSAccountTemp_A.CreateSPID = udtSP.SPID

            udtEHSAccountTemp_O.CreateBy = udtSP.SPID
            udtEHSAccountTemp_A.CreateBy = udtSP.SPID

            udtEHSAccountTemp_O.getPersonalInformation(strDocCode).CreateBy = udtSP.SPID
            udtEHSAccountTemp_A.getPersonalInformation(strDocCode).CreateBy = udtSP.SPID

            udtEHSAccountTemp_O.CreateSPPracticeDisplaySeq = intPracticeDisplaySeq
            udtEHSAccountTemp_A.CreateSPPracticeDisplaySeq = intPracticeDisplaySeq

            udtEHSAccountTemp_O.SchemeCode = strSchemeCode
            udtEHSAccountTemp_A.SchemeCode = strSchemeCode

            Dim udtDB As New Database()
            Try
                udtDB.BeginTransaction()

                Me._udtEHSAccountBLL.InsertAmendmentEHSAccount(udtDB, strDocCode, udtEHSAccountTemp_O, udtEHSAccountTemp_A)

                If Not blnCreateDE Then
                    Dim strHistoryRecordStatus As String = "V"
                    Dim strNeedImmDVerify As String = "Y"
                    _udtEHSAccountBLL.InsertPersonalInfoAmendHistory(udtDB, udtEHSAccountTemp_A, udtSP.SPID, strHistoryRecordStatus, strNeedImmDVerify)

                    If Not blnNeedImmDValidation Then
                        Dim udtImmDBLL As New ImmD.ImmDBLL
                        udtImmDBLL.ValidateAccountEHSModelWithoutImmDValidation(udtEHSAccountTemp_A, udtSP.SPID, False, udtDB)
                    End If
                End If

                udtDB.CommitTransaction()

            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                Throw eSQL
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try

            udtEHSAccountModel_Amend = udtEHSAccountBLL.LoadTempEHSAccountByVRID(udtEHSAccountTemp_A.VoucherAccID)
            udtEHSAccountModel_Amend.SetSearchDocCode(udtEHSAccountModel_Amend.EHSPersonalInformationList(0).DocCode)

            If blnNeedImmDValidation Then
                Return udtEHSAccountBLL.LoadTempEHSAccountByVRID(udtEHSAccountTemp_A.VoucherAccID)
            Else
                If blnCreateDE Then
                    Return udtEHSAccountBLL.LoadTempEHSAccountByVRID(udtEHSAccountTemp_A.VoucherAccID)
                Else
                    Return udtEHSAccountBLL.LoadEHSAccountByVRID(udtEHSAccountModel_Validated.VoucherAccID)
                End If

            End If
            'Return udtEHSAccountTemp_A
        End Function

        Public Function CreateAmendEHSAccountEHSTransaction(ByVal udtSP As ServiceProviderModel, ByVal udtDataEntry As DataEntryUserModel, _
            ByVal udtEHSTransactionModel As EHSTransactionModel, ByVal udtEHSAccountModel_Validated As EHSAccountModel, ByRef udtEHSAccountModel_Amend As EHSAccountModel, _
            ByVal udtSchemeClaimModel As SchemeClaimModel, ByVal enumAppSource As EHSTransactionModel.AppSourceEnum) As Common.ComObject.SystemMessage

            '-----  Amend EHS Account Step -----'
            '   1. Create 2 new temp EHSAccount in table "TempVoucherAccount", 
            '   2. Create 2 temp EHSPersonalInformation in table "TempPersonalInformation"
            '       => One Temp EHSAccount account purpose = 'O' (Original Record)
            '          with original personal information
            '       => Another Temp EHSAccount account purpose = 'A' (Amendment Record)
            '          with amending personal information
            '   3. if the creation is mabe by SP
            '       => Add one record in table 'PersonalInfoAmendHistory'
            '       => if the amendment is no need to send to ImmD for verification
            '           => Call the 'ValidateAccountEHSModelWithoutImmDValidation' to merge
            '              the Temp EHSAccount account purpose = 'A' to validated EHSAccount
            ' --------------------------------------------------------

            Dim udtGF As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
            Dim strDocCode As String = udtEHSAccountModel_Amend.EHSPersonalInformationList(0).DocCode.Trim
            Dim udtEHSAccountTemp_O As EHSAccountModel
            Dim udtEHSAccountTemp_A As EHSAccountModel
            Dim udtEHSAccountBLL As EHSAccountBLL = New EHSAccountBLL()
            Dim udtErrorMsg As Common.ComObject.SystemMessage

            Dim blnCreateDE As Boolean = False
            Dim blnNeedImmDValidation As Boolean = False

            udtEHSAccountTemp_O = udtEHSAccountModel_Validated.CloneDataForAmendmentOld(strDocCode, False)
            udtEHSAccountTemp_A = udtEHSAccountModel_Amend.CloneDataForAmendment(strDocCode, False)

            udtEHSAccountTemp_O.VoucherAccID = udtGF.generateSystemNum("C")
            udtEHSAccountTemp_A.VoucherAccID = udtGF.generateSystemNum("C")

            udtEHSAccountTemp_A.OriginalAmendAccID = udtEHSAccountTemp_O.VoucherAccID
            udtEHSAccountTemp_A.ValidatedAccID = udtEHSAccountModel_Validated.VoucherAccID

            udtEHSTransactionModel.TempVoucherAccID = udtEHSAccountTemp_A.VoucherAccID

            If udtDataEntry Is Nothing Then
                udtEHSAccountTemp_O.DataEntryBy = String.Empty
                udtEHSAccountTemp_A.DataEntryBy = String.Empty

                udtEHSAccountTemp_O.EHSPersonalInformationList(0).DataEntryBy = String.Empty
                udtEHSAccountTemp_A.EHSPersonalInformationList(0).DataEntryBy = String.Empty
            Else
                udtEHSAccountTemp_O.DataEntryBy = udtDataEntry.DataEntryAccount
                udtEHSAccountTemp_A.DataEntryBy = udtDataEntry.DataEntryAccount

                udtEHSAccountTemp_O.EHSPersonalInformationList(0).DataEntryBy = udtDataEntry.DataEntryAccount
                udtEHSAccountTemp_A.EHSPersonalInformationList(0).DataEntryBy = udtDataEntry.DataEntryAccount

                udtEHSAccountTemp_O.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingConfirmation
                udtEHSAccountTemp_A.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingConfirmation

                blnCreateDE = True
            End If

            udtEHSAccountTemp_O.CreateSPID = udtSP.SPID
            udtEHSAccountTemp_A.CreateSPID = udtSP.SPID

            udtEHSAccountTemp_O.CreateBy = udtSP.SPID
            udtEHSAccountTemp_A.CreateBy = udtSP.SPID

            udtEHSAccountTemp_O.getPersonalInformation(strDocCode).CreateBy = udtSP.SPID
            udtEHSAccountTemp_A.getPersonalInformation(strDocCode).CreateBy = udtSP.SPID

            udtEHSAccountTemp_O.CreateSPPracticeDisplaySeq = udtEHSTransactionModel.PracticeID
            udtEHSAccountTemp_A.CreateSPPracticeDisplaySeq = udtEHSTransactionModel.PracticeID

            udtEHSAccountTemp_O.SchemeCode = udtEHSTransactionModel.SchemeCode
            udtEHSAccountTemp_A.SchemeCode = udtEHSTransactionModel.SchemeCode

            If udtEHSAccountTemp_A.EHSPersonalInformationList(0).DateofIssue.Value > udtEHSAccountTemp_O.EHSPersonalInformationList(0).DateofIssue.Value Then
                blnNeedImmDValidation = True
            End If

            Dim udtDB As New Database()
            Try
                udtDB.BeginTransaction()

                ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
                ' -----------------------------------------------------------------------------------------
                Me._udtEHSAccountBLL.InsertAmendmentEHSAccount(udtDB, strDocCode, udtEHSAccountTemp_O, udtEHSAccountTemp_A)
                ' CRE13-006 - HCVS Ceiling [End][Tommy L]
                udtErrorMsg = Me._udtEHSTransactionBLL.InsertEHSTransaction(udtDB, udtEHSTransactionModel, udtEHSAccountTemp_A, udtEHSAccountModel_Amend.EHSPersonalInformationList(0), udtSchemeClaimModel, enumAppSource)

                If udtErrorMsg Is Nothing Then
                    ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
                    ' -----------------------------------------------------------------------------------------
                    'Me._udtEHSAccountBLL.InsertAmendmentEHSAccount(udtDB, strDocCode, udtEHSAccountTemp_O, udtEHSAccountTemp_A)
                    ' CRE13-006 - HCVS Ceiling [End][Tommy L]

                    If Not blnCreateDE Then
                        Dim strHistoryRecordStatus As String = "V"
                        Dim strNeedImmDVerify As String = "Y"
                        _udtEHSAccountBLL.InsertPersonalInfoAmendHistory(udtDB, udtEHSAccountTemp_A, udtSP.SPID, strHistoryRecordStatus, strNeedImmDVerify)

                        If Not blnNeedImmDValidation Then
                            Dim udtImmDBLL As New ImmD.ImmDBLL
                            udtImmDBLL.ValidateAccountEHSModelWithoutImmDValidation(udtEHSAccountTemp_A, udtSP.SPID, False, udtDB)
                        End If
                    End If

                End If

                If udtErrorMsg Is Nothing Then
                    udtDB.CommitTransaction()
                Else
                    udtDB.RollBackTranscation()
                End If

            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                Throw eSQL
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try

            'I-CRE17-005 (Performance Tuning) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            udtEHSAccountModel_Amend = udtEHSAccountBLL.LoadTempEHSAccountByVRID(udtEHSAccountTemp_A.VoucherAccID)

            If Not udtEHSAccountModel_Amend Is Nothing Then
                udtEHSAccountModel_Amend.SetSearchDocCode(udtEHSAccountModel_Amend.EHSPersonalInformationList(0).DocCode)
            End If
            'I-CRE17-005 (Performance Tuning) [End][Chris YIM]

            Return udtErrorMsg
        End Function
        '==================================================================================================================================================================
#End Region

#Region "Support Function"

        'CRE15-003 System-generated Form [Start][Philip Chau]
        ''' <summary>
        ''' Check if the prefix of the temporay transaction ID matches the prefix of the transaction ID
        ''' </summary>
        ''' <param name="strTransactionIDPrefix"></param>
        ''' <param name="udtEHSTransaction"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function chkIsPrefixAndTransactionIDTheSame(ByVal strTransactionIDPrefix As String, ByVal udtEHSTransaction As EHSTransactionModel) As Boolean
            Dim udtFormatter As Formatter = New Formatter()
            Return String.IsNullOrEmpty(strTransactionIDPrefix) Or udtFormatter.formatSystemNumber(udtEHSTransaction.TransactionID).Contains(strTransactionIDPrefix)
        End Function
        'CRE15-003 System-generated Form [End][Philip Chau]

        ''' <summary>
        ''' Checking the input of EC Age on Date of Registration Equal to Searched EHS Account
        ''' </summary>
        ''' <param name="udtEHSPersonalInformation"></param>
        ''' <param name="intAge"></param>
        ''' <param name="dtmDOR"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function chkEHSAccountInputDOBMatch(ByRef udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel, ByVal intAge As Integer, ByVal dtmDOR As Date) As Boolean

            If (udtEHSPersonalInformation.ECAge.HasValue AndAlso udtEHSPersonalInformation.ECDateOfRegistration.HasValue) Then
                If udtEHSPersonalInformation.ECAge.Value = intAge AndAlso udtEHSPersonalInformation.ECDateOfRegistration.Value.Equals(dtmDOR) Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' Checking the input of Date Of Birth Equal to Searched EHS Account
        ''' </summary>
        ''' <param name="udtEHSPersonalInformation"></param>
        ''' <param name="dtmDOB"></param>
        ''' <param name="strExactDOB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function chkEHSAccountInputDOBMatch(ByRef udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel, ByVal dtmDOB As Date, ByVal strExactDOB As String) As Boolean

            If (udtEHSPersonalInformation.DOB.Equals(dtmDOB) AndAlso strExactDOB = "Y" AndAlso _
                (udtEHSPersonalInformation.ExactDOB = "V" OrElse udtEHSPersonalInformation.ExactDOB = "Y" OrElse udtEHSPersonalInformation.ExactDOB = "R")) _
                OrElse _
                (udtEHSPersonalInformation.DOB.Equals(dtmDOB) AndAlso strExactDOB = "M" AndAlso _
                (udtEHSPersonalInformation.ExactDOB = "U" OrElse udtEHSPersonalInformation.ExactDOB = "M")) _
                OrElse _
                (udtEHSPersonalInformation.DOB.Equals(dtmDOB) AndAlso strExactDOB = "D" AndAlso _
                (udtEHSPersonalInformation.ExactDOB = "T" OrElse udtEHSPersonalInformation.ExactDOB = "D")) Then
                Return True
            Else
                Return False
            End If

        End Function

        Private Function ChkEHSAccountInputDetailMatch(ByVal udtEHSPersonalInfoPass As EHSPersonalInformationModel, ByVal udtEHSPersonalInfoDB As EHSPersonalInformationModel) As Boolean
            ' If no EHSPersonalInformation is passed, consider it is matched
            If IsNothing(udtEHSPersonalInfoPass) Then Return True

            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
            ' ----------------------------------------------------------
            ' Compare (1) Exact DOB; (2) Gender; (3) Name in English (4) If EC, Serial No
            If udtEHSPersonalInfoPass.ExactDOB <> udtEHSPersonalInfoDB.ExactDOB Then Return False
            If udtEHSPersonalInfoPass.Gender <> udtEHSPersonalInfoDB.Gender Then Return False
            If udtEHSPersonalInfoPass.EName <> udtEHSPersonalInfoDB.EName Then Return False
            If udtEHSPersonalInfoPass.DocCode = DocTypeModel.DocTypeCode.EC Then
                If udtEHSPersonalInfoPass.ECSerialNoNotProvided <> udtEHSPersonalInfoDB.ECSerialNoNotProvided Then Return False
                If udtEHSPersonalInfoPass.ECSerialNo <> udtEHSPersonalInfoDB.ECSerialNo Then Return False
            End If
            ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

            Return True

        End Function

        Public Function chkEHSTransactionIncomplete(ByVal udtEHSTransaction As EHSTransactionModel) As Boolean
            Dim objEHSClaimBLL As New Common.Component.EHSClaim.EHSClaimBLL.EHSClaimBLL
            Return objEHSClaimBLL.CheckTransactionIncomplete(udtEHSTransaction)
        End Function

        ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [Start][Winnie]
        Public Function CheckDuplicateClaim(ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, ByVal udtEHSTransaction As EHSTransactionModel) As Boolean
            Dim intCheckMinute As Integer = 0
            Dim strCheckMinute As String = String.Empty
            Dim udtGeneralFunction As New GeneralFunction
            Dim blnCheckPractice As Boolean = False

            udtGeneralFunction.getSystemParameter("DuplicateVoucherClaimPopupAlertCheckMinute", strCheckMinute, String.Empty, udtEHSTransaction.SchemeCode)

            If strCheckMinute = String.Empty Then
                Return False
            End If

            If Not Integer.TryParse(strCheckMinute, intCheckMinute) Then
                Return False
            End If

            If intCheckMinute = 0 Then
                Return False
            End If

            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ' HCVS: no need to check practice
            ' HCVSCHN & HCVSDHC: Check duplicate claim in "Same Pracitce" of the SP;
            Select Case udtEHSTransaction.SchemeCode
                Case SchemeClaimModel.HCVS
                    blnCheckPractice = False

                Case SchemeClaimModel.HCVSDHC, SchemeClaimModel.HCVSCHN
                    blnCheckPractice = True
            End Select
            ' CRE19-006 (DHC) [End][Winnie]

            Dim blnRes As Boolean = Me._udtEHSTransactionBLL.CheckDuplicateClaim(udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, udtEHSTransaction, intCheckMinute, blnCheckPractice)

            Return blnRes
        End Function
        ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [End][Winnie]

        Public Function chkSuspendAccountClaimAllow(ByVal stClaimMode As String) As Boolean
            Dim blResult As Boolean = False
            Dim udtGenFunct As New Common.ComFunction.GeneralFunction()
            Dim strAllowSuspendAccountClaim As String = udtGenFunct.getSystemParameter("SuspendAccountClaimAllow")

            If strAllowSuspendAccountClaim = "Y" And stClaimMode = ClaimMode.COVID19 Then
                blResult = True
            End If

            Return blResult

        End Function

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
        ''' <param name="udtAllTransactionVaccineBenefit"></param>
        ''' <param name="udtInputPicker"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SearchEHSClaimVaccine(ByVal udtSchemeClaimModel As SchemeClaimModel, ByVal strDocCode As String, ByVal udtEHSAccountModel As EHSAccountModel, _
                                                    ByVal dtmServiceDate As Date, ByVal blnDynamicControl As Boolean, _
                                                    ByVal udtAllTransactionVaccineBenefit As TransactionDetailVaccineModelCollection, _
                                                    ByVal udtInputPicker As InputPickerModel) As EHSClaimVaccineModel

            Dim udtSchemeClaimBLL As New SchemeClaimBLL()
            Dim udtSchemeClaimModelPrevious As SchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtSchemeClaimModel.SchemeCode, dtmServiceDate.Date.AddDays(1).AddMinutes(-1))
            If Not udtSchemeClaimModelPrevious Is Nothing Then
                udtSchemeClaimModel = udtSchemeClaimModelPrevious
            End If

            Dim udtEHSClaimVaccineModel As New EHSClaimVaccineModel(udtSchemeClaimModel.SchemeCode)

            ' Auto Selected If only 1 Entry found
            Dim intEntryCount As Integer = 0

            If udtInputPicker Is Nothing Then
                udtInputPicker = New InputPickerModel
                udtInputPicker.CategoryCode = String.Empty
            End If

            '---------------------------------
            ' Vaccine->Subsidize
            '---------------------------------
            ' By the selected service date, determine whether the subsidize is available for the patient
            For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtSchemeClaimModel.SubsidizeGroupClaimList
                Dim blnNoVaccineAvailable As Boolean = False

                If blnDynamicControl Then
                    ' Out of Service Period, Or Not Eligible
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                    ' -----------------------------------------------------------------------------------------
                    If udtSubsidizeGroupClaim.LastServiceDtm < dtmServiceDate OrElse Not Me._udtClaimRulesBLL.CheckEligibilityPerSubsidize(udtSubsidizeGroupClaim, udtEHSAccountModel.getPersonalInformation(strDocCode), dtmServiceDate, udtAllTransactionVaccineBenefit).IsEligible Then
                        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
                        Continue For
                    End If
                End If

                If udtInputPicker.CategoryCode.Trim() <> "" Then
                    ' If Category is passed, Check Exist of Category
                    Dim udtClaimCategoryBLL As New ClaimCategory.ClaimCategoryBLL()
                    If udtClaimCategoryBLL.getAllCategoryCache().Filter(udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, udtInputPicker.CategoryCode) Is Nothing Then
                        Continue For
                    Else
                        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                        ' -----------------------------------------------------------------------------------------
                        ' Category Exist, Check for Category Eligibility for the Subsidize
                        'Dim udtEligibilityRuleResult As ClaimRulesBLL.EligibleResult = Me._udtClaimRulesBLL.CheckCategoryEligibilityByCategory(udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, udtInputPicker.CategoryCode, udtEHSAccountModel.getPersonalInformation(strDocCode).DOB, udtEHSAccountModel.getPersonalInformation(strDocCode).ExactDOB, dtmServiceDate, udtEHSAccountModel.getPersonalInformation(strDocCode).Gender)
                        Dim udtEligibilityRuleResult As ClaimRulesBLL.EligibleResult = Me._udtClaimRulesBLL.CheckCategoryEligibilityByCategory(udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, udtInputPicker.CategoryCode, udtEHSAccountModel.getPersonalInformation(strDocCode), dtmServiceDate)
                        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                        If Not udtEligibilityRuleResult.IsEligible Then
                            Continue For
                        End If
                    End If
                End If

                ' Vaccine Fee
                Dim udtSubsidizeFeeVaccineModel As SubsidizeFeeModel = udtSubsidizeGroupClaim.SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVaccine, dtmServiceDate)
                Dim udtSubsidizeFeeInjectionModel As SubsidizeFeeModel = udtSubsidizeGroupClaim.SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeInjection, dtmServiceDate)
                Dim udtSubsidizeFeeSubsidizeModel As SubsidizeFeeModel = udtSubsidizeGroupClaim.SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeSubsidize, dtmServiceDate)

                Dim dblAmount As Double = Nothing
                Dim dblVaccineFee As Double = Nothing
                Dim dblInjectionFee As Double = Nothing
                Dim dblSubsidizeFee As Double = Nothing
                Dim blnVaccineFeeVisible As Boolean = Nothing
                Dim blnInjectionFeeVisible As Boolean = Nothing
                Dim blnSubsidizeFeeVisible As Boolean = Nothing

                If Not udtSubsidizeFeeVaccineModel Is Nothing Then
                    dblAmount = dblAmount + udtSubsidizeFeeVaccineModel.SubsidizeFee
                    dblVaccineFee = udtSubsidizeFeeVaccineModel.SubsidizeFee
                    blnVaccineFeeVisible = udtSubsidizeFeeVaccineModel.SubsidizeFeeVisible
                Else
                    dblVaccineFee = Nothing
                    blnVaccineFeeVisible = False
                End If

                If Not udtSubsidizeFeeInjectionModel Is Nothing Then
                    dblAmount = dblAmount + udtSubsidizeFeeInjectionModel.SubsidizeFee
                    dblInjectionFee = udtSubsidizeFeeInjectionModel.SubsidizeFee
                    blnInjectionFeeVisible = udtSubsidizeFeeInjectionModel.SubsidizeFeeVisible
                Else
                    dblInjectionFee = Nothing
                    blnInjectionFeeVisible = False
                End If

                If Not udtSubsidizeFeeSubsidizeModel Is Nothing Then
                    dblAmount = dblAmount + udtSubsidizeFeeSubsidizeModel.SubsidizeFee
                    dblVaccineFee = udtSubsidizeFeeSubsidizeModel.SubsidizeFee
                    blnVaccineFeeVisible = udtSubsidizeFeeSubsidizeModel.SubsidizeFeeVisible
                Else
                    dblSubsidizeFee = Nothing
                    blnSubsidizeFeeVisible = False
                End If

                Dim udtEHSClaimSubsidizeModel As New EHSClaimVaccineModel.EHSClaimSubsidizeModel( _
                    udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, _
                    udtSubsidizeGroupClaim.DisplaySeq, udtSubsidizeGroupClaim.SubsidizeItemCode, _
                    udtSubsidizeGroupClaim.DisplayCodeForClaim, udtSubsidizeGroupClaim.SubsidizeItemDesc, udtSubsidizeGroupClaim.SubsidizeItemDescChi, _
                    dblAmount, _
                    dblVaccineFee, blnVaccineFeeVisible, _
                    dblInjectionFee, blnInjectionFeeVisible, _
                    dblSubsidizeFee, blnSubsidizeFeeVisible, _
                    udtSubsidizeGroupClaim.HighRiskOption _
                    )

                Dim strRemark As String = String.Empty
                Dim strRemarkChi As String = String.Empty
                Dim strRemarkCN As String = String.Empty

                Dim providerUS As New System.Globalization.CultureInfo("en-us")
                Dim providerTW As New System.Globalization.CultureInfo("zh-tw")
                Dim providerCN As New System.Globalization.CultureInfo("zh-cn")

                If Not udtSubsidizeFeeVaccineModel Is Nothing AndAlso udtSubsidizeFeeVaccineModel.SubsidizeFeeVisible Then
                    strRemark = HttpContext.GetGlobalResourceObject("Text", udtSubsidizeFeeVaccineModel.SubsidizeFeeTypeDisplayResource, providerUS) + ": $" + udtSubsidizeFeeVaccineModel.SubsidizeFee.ToString()
                    strRemarkChi = HttpContext.GetGlobalResourceObject("Text", udtSubsidizeFeeVaccineModel.SubsidizeFeeTypeDisplayResource, providerTW) + ": $" + udtSubsidizeFeeVaccineModel.SubsidizeFee.ToString()
                    strRemarkCN = HttpContext.GetGlobalResourceObject("Text", udtSubsidizeFeeVaccineModel.SubsidizeFeeTypeDisplayResource, providerCN) + ": $" + udtSubsidizeFeeVaccineModel.SubsidizeFee.ToString()
                End If

                If Not udtSubsidizeFeeInjectionModel Is Nothing AndAlso udtSubsidizeFeeInjectionModel.SubsidizeFeeVisible Then
                    If strRemark.Trim() <> "" Then strRemark = strRemark + ", "
                    If strRemarkChi.Trim() <> "" Then strRemarkChi = strRemarkChi + ", "
                    If strRemarkCN.Trim() <> "" Then strRemarkCN = strRemarkCN + ", "
                    strRemark = strRemark + HttpContext.GetGlobalResourceObject("Text", udtSubsidizeFeeInjectionModel.SubsidizeFeeTypeDisplayResource, providerUS) + ": $" + udtSubsidizeFeeInjectionModel.SubsidizeFee.ToString()
                    strRemarkChi = strRemarkChi + HttpContext.GetGlobalResourceObject("Text", udtSubsidizeFeeInjectionModel.SubsidizeFeeTypeDisplayResource, providerTW) + ": $" + udtSubsidizeFeeInjectionModel.SubsidizeFee.ToString()
                    strRemarkCN = strRemarkCN + HttpContext.GetGlobalResourceObject("Text", udtSubsidizeFeeInjectionModel.SubsidizeFeeTypeDisplayResource, providerCN) + ": $" + udtSubsidizeFeeInjectionModel.SubsidizeFee.ToString()
                End If

                If Not udtSubsidizeFeeSubsidizeModel Is Nothing AndAlso udtSubsidizeFeeSubsidizeModel.SubsidizeFeeVisible Then
                    If strRemark.Trim() <> "" Then strRemark = strRemark + ", "
                    If strRemarkChi.Trim() <> "" Then strRemarkChi = strRemarkChi + ", "
                    If strRemarkCN.Trim() <> "" Then strRemarkCN = strRemarkCN + ", "
                    strRemark = strRemark + HttpContext.GetGlobalResourceObject("Text", udtSubsidizeFeeSubsidizeModel.SubsidizeFeeTypeDisplayResource, providerUS) + ": $" + udtSubsidizeFeeSubsidizeModel.SubsidizeFee.ToString()
                    strRemarkChi = strRemarkChi + HttpContext.GetGlobalResourceObject("Text", udtSubsidizeFeeSubsidizeModel.SubsidizeFeeTypeDisplayResource, providerTW) + ": $" + udtSubsidizeFeeSubsidizeModel.SubsidizeFee.ToString()
                    strRemarkCN = strRemarkCN + HttpContext.GetGlobalResourceObject("Text", udtSubsidizeFeeSubsidizeModel.SubsidizeFeeTypeDisplayResource, providerCN) + ": $" + udtSubsidizeFeeSubsidizeModel.SubsidizeFee.ToString()
                End If

                udtEHSClaimSubsidizeModel.Remark = strRemark
                udtEHSClaimSubsidizeModel.RemarkChi = strRemarkChi
                udtEHSClaimSubsidizeModel.RemarkCN = strRemarkCN


                ' Check available benefit
                Dim arrCheckDateList As New List(Of Date)
                arrCheckDateList.Add(dtmServiceDate)

                Dim dicDoseRuleResult As Dictionary(Of String, ClaimRulesBLL.DoseRuleResult) = Nothing
                ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                Dim udtSubsidizeItemDetailList As SubsidizeItemDetailsModelCollection = Me._udtSchemeDetailBLL.getSubsidizeItemDetails(udtEHSClaimSubsidizeModel.SubsidizeItemCode)
                Dim udtSubsidizeGroupClaimItemDetailsList As SubsidizeGroupClaimItemDetailsModelCollection = Me._udtSchemeDetailBLL.getSubsidizeGroupClaimItemDetails(udtEHSClaimSubsidizeModel.SchemeCode, udtEHSClaimSubsidizeModel.SchemeSeq, udtEHSClaimSubsidizeModel.SubsidizeCode, udtEHSClaimSubsidizeModel.SubsidizeItemCode)

                Dim udtResSubsidizeItemDetailList As New SubsidizeItemDetailsModelCollection

                For Each udtSubsidizeItemDetail As SubsidizeItemDetailsModel In udtSubsidizeItemDetailList
                    For Each udtSubsidizeGroupClaimItemDetail As SubsidizeGroupClaimItemDetailsModel In udtSubsidizeGroupClaimItemDetailsList
                        If udtSubsidizeItemDetail.SubsidizeItemCode = udtSubsidizeGroupClaimItemDetail.SubsidizeItemCode And _
                            udtSubsidizeItemDetail.AvailableItemCode = udtSubsidizeGroupClaimItemDetail.AvailableItemCode Then

                            udtResSubsidizeItemDetailList.Add(New SubsidizeItemDetailsModel(udtSubsidizeItemDetail))

                            Continue For

                        End If
                    Next
                Next

                blnNoVaccineAvailable = Me._udtClaimRulesBLL.chkVaccineAvailableBenefitBySubsidize(udtSubsidizeGroupClaim, udtResSubsidizeItemDetailList, _
                                                                                udtAllTransactionVaccineBenefit, _
                                                                                udtEHSAccountModel.getPersonalInformation(strDocCode), _
                                                                                dtmServiceDate, _
                                                                                dicDoseRuleResult, _
                                                                                udtInputPicker)

                ' Checking For Each SubsidizeItemCode: Eg. HSIV->1STDOSE, 2NDDOSE, ONLYDOSE
                '--------------------------------------------------------
                ' Vaccine -> Subsidze -> SubsidizeDetail
                '--------------------------------------------------------
                For Each udtSubsidizeItemDetail As SubsidizeItemDetailsModel In udtResSubsidizeItemDetailList

                    Dim udtDoseRuleResult As ClaimRulesBLL.DoseRuleResult = Nothing
                    If Not dicDoseRuleResult Is Nothing AndAlso dicDoseRuleResult.ContainsKey(udtSubsidizeItemDetail.AvailableItemCode) Then
                        udtDoseRuleResult = dicDoseRuleResult(udtSubsidizeItemDetail.AvailableItemCode)
                    End If

                    If Not udtDoseRuleResult Is Nothing AndAlso udtDoseRuleResult.IsMatch Then

                        Dim blnAvailable As Boolean = True
                        Dim blnHide As Boolean = False

                        Select Case udtDoseRuleResult.HandlingMethod
                            Case ClaimRulesBLL.DoseRuleHandlingMethod.ALL
                                blnAvailable = True
                                blnHide = False
                            Case ClaimRulesBLL.DoseRuleHandlingMethod.READONLY
                                blnAvailable = False
                                blnHide = False
                            Case ClaimRulesBLL.DoseRuleHandlingMethod.HIDE
                                blnAvailable = False
                                blnHide = True
                            Case Else

                        End Select

                        Dim udtEHSClaimSubsidizeDetailModel As New EHSClaimVaccineModel.EHSClaimSubidizeDetailModel( _
                                                udtSubsidizeItemDetail.SubsidizeItemCode, udtSubsidizeItemDetail.AvailableItemCode, udtSubsidizeItemDetail.DisplaySeq, _
                                                udtSubsidizeItemDetail.AvailableItemDesc, udtSubsidizeItemDetail.AvailableItemDescChi, udtSubsidizeItemDetail.AvailableItemDescCN, _
                                                udtSubsidizeItemDetail.AvailableItemNum, blnAvailable, False, blnHide)

                        If Not blnAvailable Then
                            udtEHSClaimSubsidizeDetailModel.SubsidizeItemDisabledRemark = udtDoseRuleResult
                        End If

                        udtEHSClaimSubsidizeModel.Add(udtEHSClaimSubsidizeDetailModel)
                    End If
                Next


                ' If Vaccine->Subsidze contain at least 1 SubsidizeDetail 
                If Not udtEHSClaimSubsidizeModel.SubsidizeDetailList Is Nothing AndAlso udtEHSClaimSubsidizeModel.SubsidizeDetailList.Count > 0 Then

                    udtEHSClaimVaccineModel.Add(udtEHSClaimSubsidizeModel)
                    intEntryCount = intEntryCount + 1
                End If

            Next
            ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

            If intEntryCount = 1 Then
                udtEHSClaimVaccineModel.SubsidizeList(0).Selected = True
            End If

            Return udtEHSClaimVaccineModel
        End Function

        Public Function ConstructEHSClaimVaccineModel(ByVal strSchemeCode As String, ByRef udtEHSTransactionModel As EHSTransactionModel) As EHSClaimVaccineModel

            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            ' [SchemeClaim]
            'Dim udtSchemeClaimModel As SchemeClaimModel = Me._udtSchemeClaimBLL.getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(udtEHSTransactionModel.SchemeCode)
            ' [SchemeVaccineDetail]: Vaccine Fee
            'Dim udtSchemeVaccineDetailList As SchemeVaccineDetailModelCollection = Me._udtSchemeDetailBLL.getSchemeVaccineDetail(udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SchemeSeq)

            ' <EHSClaimVaccine>
            Dim udtEHSClaimVaccineModel As New EHSClaimVaccineModel(udtEHSTransactionModel.SchemeCode)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

            ' Loop TransactionDetail
            For Each udtTransactionDetail As TransactionDetailModel In udtEHSTransactionModel.TransactionDetails
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]
                ' Retrieve correct scheme info by each TransactionDetail
                ' [SchemeClaim]
                Dim udtSchemeClaimModel = Me._udtSchemeClaimBLL.getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(udtTransactionDetail.SchemeCode)
                ' [SchemeVaccineDetail]: Vaccine Fee
                'udtSchemeVaccineDetailList = Me._udtSchemeDetailBLL.getSchemeVaccineDetail(udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SchemeSeq)
                ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

                ' SubsidizeItem
                Dim udtSubsidizeGroupClaim As SubsidizeGroupClaimModel = udtSchemeClaimModel.SubsidizeGroupClaimList.Filter(udtSchemeClaimModel.SchemeCode, udtTransactionDetail.SchemeSeq, udtTransactionDetail.SubsidizeCode)
                ' Vaccine Fee
                'Dim udtSchemeVaccineDetailModel As SchemeVaccineDetailModel = udtSchemeVaccineDetailList.Filter(udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode)
                Dim udtSubsidizeFeeVaccineModel As SubsidizeFeeModel = udtSubsidizeGroupClaim.SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVaccine, udtEHSTransactionModel.ServiceDate)
                Dim udtSubsidizeFeeInjectionModel As SubsidizeFeeModel = udtSubsidizeGroupClaim.SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeInjection, udtEHSTransactionModel.ServiceDate)
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

                'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim udtSubsidizeFeeSubsidizeModel As SubsidizeFeeModel = udtSubsidizeGroupClaim.SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeSubsidize, udtEHSTransactionModel.ServiceDate)

                Dim strRemark As String = String.Empty
                Dim strRemarkChi As String = String.Empty
                Dim strRemarkCN As String = String.Empty

                Dim providerUS As New System.Globalization.CultureInfo("en-us")
                Dim providerTW As New System.Globalization.CultureInfo("zh-tw")
                Dim providerCN As New System.Globalization.CultureInfo("zh-cn")

                If Not udtSubsidizeFeeVaccineModel Is Nothing AndAlso udtSubsidizeFeeVaccineModel.SubsidizeFeeVisible Then
                    strRemark = HttpContext.GetGlobalResourceObject("Text", udtSubsidizeFeeVaccineModel.SubsidizeFeeTypeDisplayResource, providerUS) + ": $" + udtSubsidizeFeeVaccineModel.SubsidizeFee.ToString()
                    strRemarkChi = HttpContext.GetGlobalResourceObject("Text", udtSubsidizeFeeVaccineModel.SubsidizeFeeTypeDisplayResource, providerTW) + ": $" + udtSubsidizeFeeVaccineModel.SubsidizeFee.ToString()
                    strRemarkCN = HttpContext.GetGlobalResourceObject("Text", udtSubsidizeFeeVaccineModel.SubsidizeFeeTypeDisplayResource, providerCN) + ": $" + udtSubsidizeFeeVaccineModel.SubsidizeFee.ToString()
                End If

                If Not udtSubsidizeFeeInjectionModel Is Nothing AndAlso udtSubsidizeFeeInjectionModel.SubsidizeFeeVisible Then
                    If strRemark.Trim() <> "" Then strRemark = strRemark + ", "
                    If strRemarkChi.Trim() <> "" Then strRemarkChi = strRemarkChi + ", "
                    If strRemarkCN.Trim() <> "" Then strRemarkCN = strRemarkCN + ", "
                    strRemark = strRemark + HttpContext.GetGlobalResourceObject("Text", udtSubsidizeFeeInjectionModel.SubsidizeFeeTypeDisplayResource, providerUS) + ": $" + udtSubsidizeFeeInjectionModel.SubsidizeFee.ToString()
                    strRemarkChi = strRemarkChi + HttpContext.GetGlobalResourceObject("Text", udtSubsidizeFeeInjectionModel.SubsidizeFeeTypeDisplayResource, providerTW) + ": $" + udtSubsidizeFeeInjectionModel.SubsidizeFee.ToString()
                    strRemarkCN = strRemarkCN + HttpContext.GetGlobalResourceObject("Text", udtSubsidizeFeeInjectionModel.SubsidizeFeeTypeDisplayResource, providerCN) + ": $" + udtSubsidizeFeeInjectionModel.SubsidizeFee.ToString()
                End If

                If Not udtSubsidizeFeeSubsidizeModel Is Nothing AndAlso udtSubsidizeFeeSubsidizeModel.SubsidizeFeeVisible Then
                    If strRemark.Trim() <> "" Then strRemark = strRemark + ", "
                    If strRemarkChi.Trim() <> "" Then strRemarkChi = strRemarkChi + ", "
                    If strRemarkCN.Trim() <> "" Then strRemarkCN = strRemarkCN + ", "
                    strRemark = strRemark + HttpContext.GetGlobalResourceObject("Text", udtSubsidizeFeeSubsidizeModel.SubsidizeFeeTypeDisplayResource, providerUS) + ": $" + udtSubsidizeFeeSubsidizeModel.SubsidizeFee.ToString()
                    strRemarkChi = strRemarkChi + HttpContext.GetGlobalResourceObject("Text", udtSubsidizeFeeSubsidizeModel.SubsidizeFeeTypeDisplayResource, providerTW) + ": $" + udtSubsidizeFeeSubsidizeModel.SubsidizeFee.ToString()
                    strRemarkCN = strRemarkCN + HttpContext.GetGlobalResourceObject("Text", udtSubsidizeFeeSubsidizeModel.SubsidizeFeeTypeDisplayResource, providerCN) + ": $" + udtSubsidizeFeeSubsidizeModel.SubsidizeFee.ToString()
                End If

                ' Vaccine->Subsidize
                Dim dblAmount As Double = Nothing
                Dim dblVaccineFee As Double = Nothing
                Dim dblInjectionFee As Double = Nothing
                Dim dblSubsidizeFee As Double = Nothing
                Dim blnVaccineFeeVisible As Boolean = Nothing
                Dim blnInjectionFeeVisible As Boolean = Nothing
                Dim blnSubsidizeFeeVisible As Boolean = Nothing

                If Not udtSubsidizeFeeVaccineModel Is Nothing Then
                    dblAmount = dblAmount + udtSubsidizeFeeVaccineModel.SubsidizeFee
                    dblVaccineFee = udtSubsidizeFeeVaccineModel.SubsidizeFee
                    blnVaccineFeeVisible = udtSubsidizeFeeVaccineModel.SubsidizeFeeVisible
                Else
                    dblVaccineFee = Nothing
                    blnVaccineFeeVisible = False
                End If

                If Not udtSubsidizeFeeInjectionModel Is Nothing Then
                    dblAmount = dblAmount + udtSubsidizeFeeInjectionModel.SubsidizeFee
                    dblInjectionFee = udtSubsidizeFeeInjectionModel.SubsidizeFee
                    blnInjectionFeeVisible = udtSubsidizeFeeInjectionModel.SubsidizeFeeVisible
                Else
                    dblInjectionFee = Nothing
                    blnInjectionFeeVisible = False
                End If

                If Not udtSubsidizeFeeSubsidizeModel Is Nothing Then
                    dblAmount = dblAmount + udtSubsidizeFeeSubsidizeModel.SubsidizeFee
                    dblVaccineFee = udtSubsidizeFeeSubsidizeModel.SubsidizeFee
                    blnVaccineFeeVisible = udtSubsidizeFeeSubsidizeModel.SubsidizeFeeVisible
                Else
                    dblSubsidizeFee = Nothing
                    blnSubsidizeFeeVisible = False
                End If

                'CRE16-026 (Add PCV13) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim udtEHSClaimSubsidizeModel As New EHSClaimVaccineModel.EHSClaimSubsidizeModel( _
                    udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, _
                    udtSubsidizeGroupClaim.DisplaySeq, udtSubsidizeGroupClaim.SubsidizeItemCode, _
                    udtSubsidizeGroupClaim.DisplayCodeForClaim, udtSubsidizeGroupClaim.SubsidizeItemDesc, udtSubsidizeGroupClaim.SubsidizeItemDescChi, _
                    dblAmount, _
                    dblVaccineFee, blnVaccineFeeVisible, _
                    dblInjectionFee, blnInjectionFeeVisible, _
                    dblSubsidizeFee, blnSubsidizeFeeVisible, _
                    udtSubsidizeGroupClaim.HighRiskOption _
                    )
                'CRE16-026 (Add PCV13) [End][Chris YIM]

                'CRE16-002 (Revamp VSS) [End][Chris YIM]

                udtEHSClaimSubsidizeModel.Remark = strRemark
                udtEHSClaimSubsidizeModel.RemarkChi = strRemarkChi
                udtEHSClaimSubsidizeModel.RemarkCN = strRemarkCN

                ' Vaccine->Subsidze->SubsidizeDetail
                'CRE16-026 (Add PCV13) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim udtEHSClaimSubsidizeDetailModel As New EHSClaimVaccineModel.EHSClaimSubidizeDetailModel( _
                        udtTransactionDetail.SubsidizeItemCode, udtTransactionDetail.AvailableItemCode, 1, _
                        udtTransactionDetail.AvailableItemDesc, udtTransactionDetail.AvailableItemDescChi, udtTransactionDetail.AvailableItemDescCN, udtTransactionDetail.Unit, True, True, False)
                'CRE16-026 (Add PCV13) [End][Chris YIM]
                udtEHSClaimSubsidizeModel.Add(udtEHSClaimSubsidizeDetailModel)
                udtEHSClaimSubsidizeModel.Selected = True

                udtEHSClaimVaccineModel.Add(udtEHSClaimSubsidizeModel)
            Next

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
        Public Function ConstructEHSTemporaryVoucherAccount(ByVal strIdentityNum As String, ByVal strDocCode As String, ByVal strExactDOB As String, ByVal dtmDOB As DateTime, ByVal strSchemeCode As String, ByVal strAdoptionPrefixNum As String, Optional ByVal udtEHSPersonalInfo As EHSPersonalInformationModel = Nothing) As EHSAccountModel
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

            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
            ' ----------------------------------------------------------
            If strDocCode = DocTypeModel.DocTypeCode.EC AndAlso Not udtEHSPersonalInfo Is Nothing Then
                udtEHSAccount.EHSPersonalInformationList(0).ECSerialNoNotProvided = udtEHSPersonalInfo.ECSerialNoNotProvided
                udtEHSAccount.EHSPersonalInformationList(0).ECSerialNo = udtEHSPersonalInfo.ECSerialNo
            End If
            ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

            Return udtEHSAccount

        End Function

        ''' <summary>
        ''' Create new Temporary Account from Validated Account when (1) from Vaccination Record Enquiry; (2) Validated Account found but different document type
        ''' </summary>
        ''' <param name="strIdentityNum"></param>
        ''' <param name="strDocCode"></param>
        ''' <param name="strExactDOB"></param>
        ''' <param name="dtmDOB"></param>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="strAdoptionPrefixNum"></param>
        ''' <param name="strENameFirstName"></param>
        ''' <param name="strENameSurName"></param>
        ''' <param name="strGender"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <author>tmk791</author>
        Public Function ConstructEHSTemporaryVoucherAccount(ByVal strIdentityNum As String, ByVal strDocCode As String, ByVal strExactDOB As String, ByVal dtmDOB As DateTime, ByVal strSchemeCode As String, ByVal strAdoptionPrefixNum As String, ByVal strENameFirstName As String, ByVal strENameSurName As String, ByVal strGender As String) As EHSAccountModel
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
            udtEHSAccount.EHSPersonalInformationList(0).ENameFirstName = strENameFirstName
            udtEHSAccount.EHSPersonalInformationList(0).ENameSurName = strENameSurName
            udtEHSAccount.EHSPersonalInformationList(0).Gender = strGender

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


        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        ' Create transaction with service date for calculate the Voucher Before Redeem

        ''' <summary>
        ''' Construct EHS Transaction Model
        ''' </summary>
        ''' <param name="udtSchemeClaimModel"></param>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="udtPracticeDisplayModel"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ConstructNewEHSTransaction(ByVal udtSchemeClaimModel As SchemeClaimModel, ByVal udtEHSAccount As EHSAccountModel, ByVal udtPracticeDisplayModel As PracticeDisplayModel, ByVal dtmServiceDate As DateTime) As EHSTransactionModel
            Return ConstructNewEHSTransaction(udtSchemeClaimModel, udtEHSAccount, udtPracticeDisplayModel, dtmServiceDate, Nothing, Nothing)
        End Function

        ''' <summary>
        ''' Construct EHS Transaction Model
        ''' </summary>
        ''' <param name="udtSchemeClaimModel"></param>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="udtPracticeDisplayModel"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ConstructNewEHSTransaction(ByVal udtSchemeClaimModel As SchemeClaimModel, _
                                                   ByVal udtEHSAccount As EHSAccountModel, _
                                                   ByVal udtPracticeDisplayModel As PracticeDisplayModel, _
                                                   ByVal dtmServiceDate As DateTime, _
                                                   ByVal udtHAVaccineRefStatus As EHSTransactionModel.ExtRefStatusClass, _
                                                   ByVal udtDHVaccineRefStatus As EHSTransactionModel.ExtRefStatusClass) As EHSTransactionModel
            Dim udtEHSTran As New EHSTransactionModel()

            If Not udtEHSAccount.IsNew() Then
                If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                    udtEHSTran.VoucherAccID = udtEHSAccount.VoucherAccID
                Else
                    udtEHSTran.TempVoucherAccID = udtEHSAccount.VoucherAccID
                End If
            End If
            udtEHSTran.SchemeCode = udtSchemeClaimModel.SchemeCode
            udtEHSTran.ServiceDate = dtmServiceDate
            udtEHSTran.ServiceType = udtPracticeDisplayModel.ServiceCategoryCode
            udtEHSTran.ServiceProviderID = udtPracticeDisplayModel.SPID
            udtEHSTran.PracticeID = udtPracticeDisplayModel.PracticeID
            udtEHSTran.BankAccountID = udtPracticeDisplayModel.BankAcctID
            udtEHSTran.BankAccountNo = udtPracticeDisplayModel.BankAccountNo
            udtEHSTran.BankAccountOwner = udtPracticeDisplayModel.BankAccHolder
            udtEHSTran.ClaimAmount = 0

            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
            ' ----------------------------------------------------------
            ' Set External Reference Status (e.g. Vaccination record from CMS)
            udtHAVaccineRefStatus = EHSTransactionModel.ExtRefStatusClass.AmendExtRefStatus(udtSchemeClaimModel, udtHAVaccineRefStatus, VaccinationBLL.VaccineRecordProvider.HA)
            If udtHAVaccineRefStatus Is Nothing Then
                ' No Ext Ref Status that mean Vaccination record turn off
                udtEHSTran.HAVaccineRefStatus = Nothing
            Else
                ' Ext Ref Status exist, record status code
                udtEHSTran.HAVaccineRefStatus = udtHAVaccineRefStatus.Code
            End If

            udtDHVaccineRefStatus = EHSTransactionModel.ExtRefStatusClass.AmendExtRefStatus(udtSchemeClaimModel, udtDHVaccineRefStatus, VaccinationBLL.VaccineRecordProvider.DH)
            If udtDHVaccineRefStatus Is Nothing Then
                ' No Ext Ref Status that mean Vaccination record turn off
                udtEHSTran.DHVaccineRefStatus = Nothing
            Else
                ' Ext Ref Status exist, record status code
                udtEHSTran.DHVaccineRefStatus = udtDHVaccineRefStatus.Code
            End If
            ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]


            ' Get the correct SchemeClaim & subsidize by service date
            Dim udtSchemeClaimTemp As SchemeClaimModel = (New SchemeClaimBLL).getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtSchemeClaimModel.SchemeCode, dtmServiceDate.Date.AddDays(1).AddMinutes(-1))

            If udtSchemeClaimTemp.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVoucher Then

                udtEHSTran.PerVoucherValue = udtSchemeClaimTemp.SubsidizeGroupClaimList(0).SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVoucher, dtmServiceDate).SubsidizeFee

                ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                udtEHSTran.VoucherBeforeRedeem = udtEHSAccount.VoucherInfo.GetAvailableVoucher()
                ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

            End If

            Return udtEHSTran

        End Function


        ' Transaction Detail ------------------------------

        ''' <summary>
        ''' Construct the EHS Transaction Detail Model for Vaccination
        ''' use servicedate to retrieve the active scheme and active subsidize
        ''' </summary>
        ''' <param name="udtSP"></param>
        ''' <param name="udtDataEntry"></param>
        ''' <param name="udtEHSTransactionModel"></param>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="udtEHSClaimVaccineModel"></param>
        ''' <remarks></remarks>
        Public Sub ConstructEHSTransactionDetails(ByVal udtSP As ServiceProviderModel, ByVal udtDataEntry As DataEntryUserModel, _
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

                udtEHSTransactionModel.CreateBy = udtSP.SPID
                udtEHSTransactionModel.UpdateBy = udtSP.SPID
                udtEHSTransactionModel.DataEntryBy = String.Empty
            Else

                If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                    udtEHSTransactionModel.VoucherAccID = udtEHSAccount.VoucherAccID
                Else
                    udtEHSTransactionModel.TempVoucherAccID = udtEHSAccount.VoucherAccID
                End If
                udtEHSTransactionModel.RecordStatus = EHSTransactionModel.TransRecordStatusClass.Pending


                udtEHSTransactionModel.CreateBy = udtSP.SPID
                udtEHSTransactionModel.UpdateBy = udtSP.SPID
                udtEHSTransactionModel.DataEntryBy = udtDataEntry.DataEntryAccount
            End If

            udtEHSTransactionModel.TransactionDtm = Now
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            If New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(udtEHSTransactionModel.SchemeCode) = SchemeClaimModel.EnumControlType.VOUCHER OrElse _
                New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(udtEHSTransactionModel.SchemeCode) = SchemeClaimModel.EnumControlType.VOUCHERCHINA Then
                Throw New Exception(String.Format("Invalid scheme code ({0})", udtEHSTransactionModel.SchemeCode))
            End If
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

            udtEHSTransactionModel.DocCode = udtEHSAccount.SearchDocCode
            'udtEHSTransactionModel.CategoryCode =
            udtEHSTransactionModel.TransactionDetails = New TransactionDetailModelCollection()

            ' TransactionDetail
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
                    'Dim udtSchemeClaimModel As SchemeClaimModel = Me._udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSClaimVaccineModel.SchemeCode, udtEHSTransactionModel.ServiceDate)
                    'If udtSchemeClaimModel Is Nothing Then
                    '    udtSchemeClaimModel = Me._udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSClaimVaccineModel.SchemeCode, udtEHSTransactionModel.ServiceDate.AddDays(1).AddMinutes(-1))
                    'End If

                    If udtSubsidize.SubsidizeDetailList.Count = 1 Then

                        Dim udtEHSTransactionDetail As New EHSTransaction.TransactionDetailModel()
                        udtEHSTransactionDetail.SchemeCode = udtEHSClaimVaccineModel.SchemeCode
                        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
                        ' -----------------------------------------------------------------------------------------
                        ' After modification, EHSClaimVaccineModel contain SchemeSeq, so direct use the SchemeSeq
                        udtEHSTransactionDetail.SchemeSeq = udtSubsidize.SchemeSeq
                        'udtEHSTransactionDetail.SchemeSeq = udtSchemeClaimModel.SubsidizeGroupClaimList.Filter(udtEHSClaimVaccineModel.SchemeCode, udtSubsidize.SubsidizeCode).SchemeSeq
                        'udtEHSTransactionDetail.SchemeSeq = udtEHSClaimVaccineModel.SchemeSeq
                        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

                        udtEHSTransactionDetail.SubsidizeCode = udtSubsidize.SubsidizeCode
                        udtEHSTransactionDetail.SubsidizeItemCode = udtSubsidize.SubsidizeItemCode
                        udtEHSTransactionDetail.AvailableItemCode = udtSubsidize.SubsidizeDetailList(0).AvailableItemCode
                        udtEHSTransactionDetail.Unit = 1
                        udtEHSTransactionDetail.PerUnitValue = udtSubsidize.Amount
                        udtEHSTransactionDetail.TotalAmount = udtEHSTransactionDetail.Unit.Value * udtEHSTransactionDetail.PerUnitValue.Value
                        udtEHSTransactionDetail.Remark = ""
                        udtEHSTransactionModel.TransactionDetails.Add(udtEHSTransactionDetail)

                        dblClaimAmount = dblClaimAmount + udtEHSTransactionDetail.TotalAmount.Value

                    ElseIf udtSubsidize.SubsidizeDetailList.Count > 1 Then
                        For Each udtSubsidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtSubsidize.SubsidizeDetailList
                            If udtSubsidizeDetail.Selected Then
                                Dim udtEHSTransactionDetail As New EHSTransaction.TransactionDetailModel()
                                udtEHSTransactionDetail.SchemeCode = udtEHSClaimVaccineModel.SchemeCode
                                ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
                                ' -----------------------------------------------------------------------------------------
                                ' After modification, EHSClaimVaccineModel contain SchemeSeq, so direct use the SchemeSeq
                                udtEHSTransactionDetail.SchemeSeq = udtSubsidize.SchemeSeq
                                'udtEHSTransactionDetail.SchemeSeq = udtSchemeClaimModel.SubsidizeGroupClaimList.Filter(udtEHSClaimVaccineModel.SchemeCode, udtSubsidize.SubsidizeCode).SchemeSeq
                                'udtEHSTransactionDetail.SchemeSeq = udtEHSClaimVaccineModel.SchemeSeq
                                ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

                                udtEHSTransactionDetail.SubsidizeCode = udtSubsidize.SubsidizeCode
                                udtEHSTransactionDetail.SubsidizeItemCode = udtSubsidize.SubsidizeItemCode
                                udtEHSTransactionDetail.AvailableItemCode = udtSubsidizeDetail.AvailableItemCode
                                udtEHSTransactionDetail.Unit = 1
                                udtEHSTransactionDetail.PerUnitValue = udtSubsidize.Amount
                                udtEHSTransactionDetail.TotalAmount = udtEHSTransactionDetail.Unit.Value * udtEHSTransactionDetail.PerUnitValue.Value
                                udtEHSTransactionDetail.Remark = ""
                                udtEHSTransactionModel.TransactionDetails.Add(udtEHSTransactionDetail)

                                dblClaimAmount = dblClaimAmount + udtEHSTransactionDetail.TotalAmount.Value
                            End If
                        Next
                    End If
                End If
            Next

            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            ' Update SchemeSeq/Code after confirm selected claim
            If udtEHSTransactionModel.TransactionAdditionFields IsNot Nothing AndAlso udtEHSTransactionModel.TransactionAdditionFields.Count > 0 Then
                Dim udtLatestSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel = udtEHSClaimVaccineModel.GetLatestSelectedSubsidize()
                For Each udtField As EHSTransaction.TransactionAdditionalFieldModel In udtEHSTransactionModel.TransactionAdditionFields
                    udtField.SchemeSeq = udtLatestSubsidize.SchemeSeq
                    udtField.SubsidizeCode = udtLatestSubsidize.SubsidizeCode
                Next
            End If
            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

            udtEHSTransactionModel.ClaimAmount = dblClaimAmount
        End Sub

        ''' <summary>
        ''' Construct EHS Transaction Detail For Voucher
        ''' </summary>
        ''' <param name="udtSP"></param>
        ''' <param name="udtDataEntry"></param>
        ''' <param name="udtEHSTransactionModel"></param>
        ''' <param name="udtEHSAccount"></param>
        ''' <remarks></remarks>
        Public Sub ConstructEHSTransactionDetails(ByVal udtSP As ServiceProviderModel, ByVal udtDataEntry As DataEntryUserModel, _
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

                udtEHSTransactionModel.CreateBy = udtSP.SPID
                udtEHSTransactionModel.UpdateBy = udtSP.SPID
                udtEHSTransactionModel.DataEntryBy = String.Empty
            Else

                If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                    udtEHSTransactionModel.VoucherAccID = udtEHSAccount.VoucherAccID
                Else
                    udtEHSTransactionModel.TempVoucherAccID = udtEHSAccount.VoucherAccID
                End If
                udtEHSTransactionModel.RecordStatus = EHSTransactionModel.TransRecordStatusClass.Pending
                udtEHSTransactionModel.CreateBy = udtSP.SPID
                udtEHSTransactionModel.UpdateBy = udtSP.SPID
                udtEHSTransactionModel.DataEntryBy = udtDataEntry.DataEntryAccount
            End If

            udtEHSTransactionModel.TransactionDtm = Now
            'If udtEHSTransactionModel.SchemeCode = SchemeClaimModel.HCVS _
            '        And (udtEHSTransactionModel.CoPaymentFee = String.Empty Or udtEHSTransactionModel.TransactionAdditionFields.Count = 0) _
            '        And udtEHSTransactionModel.ServiceDate >= New Date(2012, 1, 1) Then
            If Me.chkEHSTransactionIncomplete(udtEHSTransactionModel) Then
                udtEHSTransactionModel.RecordStatus = EHSTransactionModel.TransRecordStatusClass.Incomplete
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

            'Dim udtSchemeClaimModel As SchemeClaimModel = Me._udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSTransactionModel.SchemeCode, udtEHSTransactionModel.ServiceDate)
            'If udtSchemeClaimModel Is Nothing Then
            '    udtSchemeClaimModel = Me._udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSTransactionModel.SchemeCode, udtEHSTransactionModel.ServiceDate.AddDays(1).AddMinutes(-1))
            'End If

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
            'CRE13-019-02 Extend HCVS to China [Start][Karl]
            If udtEHSTransactionModel.ExchangeRate.HasValue = True AndAlso udtEHSTransactionModel.VoucherClaimRMB.HasValue = True Then
                udtEHSTransactionDetail.ExchangeRate_Value = udtEHSTransactionModel.ExchangeRate
                udtEHSTransactionDetail.TotalAmountRMB = udtEHSTransactionModel.VoucherClaimRMB
            End If
            'CRE13-019-02 Extend HCVS to China [End][Karl]
            udtEHSTransactionModel.TransactionDetails.Add(udtEHSTransactionDetail)

            udtEHSTransactionModel.VoucherAfterRedeem = udtEHSTransactionModel.VoucherBeforeRedeem - udtEHSTransactionModel.VoucherClaim
            udtEHSTransactionModel.ClaimAmount = udtEHSTransactionDetail.TotalAmount


        End Sub

        ''' <summary>
        ''' Construct EHS Transaction Detail For SSSCMC
        ''' </summary>
        ''' <param name="udtSP"></param>
        ''' <param name="udtDataEntry"></param>
        ''' <param name="udtEHSTransactionModel"></param>
        ''' <param name="udtEHSAccount"></param>
        ''' <remarks></remarks>
        Public Sub ConstructEHSTransactionDetail_SSSCMC(ByVal udtSP As ServiceProviderModel, _
                                                        ByVal udtDataEntry As DataEntryUserModel, _
                                                        ByRef udtEHSTransactionModel As EHSTransactionModel, _
                                                        ByVal udtEHSAccount As EHSAccountModel,
                                                        ByVal dtHAPatient As DataTable)

            Dim udtSchemeClaimModel As SchemeClaimModel = Me._udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSTransactionModel.SchemeCode, udtEHSTransactionModel.ServiceDate.AddDays(1).AddMinutes(-1))

            ' VoucherTransaction
            If udtDataEntry Is Nothing Then
                If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                    udtEHSTransactionModel.RecordStatus = udtSchemeClaimModel.ConfirmedTransactionStatus
                    udtEHSTransactionModel.VoucherAccID = udtEHSAccount.VoucherAccID
                Else
                    udtEHSTransactionModel.RecordStatus = EHSTransactionModel.TransRecordStatusClass.PendingVRValidate
                    udtEHSTransactionModel.TempVoucherAccID = udtEHSAccount.VoucherAccID
                End If

                udtEHSTransactionModel.CreateBy = udtSP.SPID
                udtEHSTransactionModel.UpdateBy = udtSP.SPID
                udtEHSTransactionModel.DataEntryBy = String.Empty
            Else

                If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                    udtEHSTransactionModel.VoucherAccID = udtEHSAccount.VoucherAccID
                Else
                    udtEHSTransactionModel.TempVoucherAccID = udtEHSAccount.VoucherAccID
                End If
                udtEHSTransactionModel.RecordStatus = EHSTransactionModel.TransRecordStatusClass.Pending
                udtEHSTransactionModel.CreateBy = udtSP.SPID
                udtEHSTransactionModel.UpdateBy = udtSP.SPID
                udtEHSTransactionModel.DataEntryBy = udtDataEntry.DataEntryAccount
            End If

            udtEHSTransactionModel.TransactionDtm = Now
            udtEHSTransactionModel.DocCode = udtEHSAccount.SearchDocCode

            'Sub-Patient Type
            Dim strSubsidizeCode As String = String.Empty

            Select Case dtHAPatient.Rows(0)("Patient_Type").ToString.Trim
                Case "A"
                    strSubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.HAS_A
                Case "B"
                    strSubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.HAS_B
                Case Else
                    Throw New Exception(String.Format("Invalid Patient Type({0}) is found in DB table HAServicePatient.", dtHAPatient.Rows(0)("Patient_Type").ToString.Trim))
            End Select

            udtEHSTransactionModel.TransactionDetails = New TransactionDetailModelCollection()

            ' ------------------------------------------------------------------------
            ' Construct the Detail usign the Active Scheme & Subsidize By Service date 
            ' ------------------------------------------------------------------------
            Dim udtEHSTransactionDetail As New EHSTransaction.TransactionDetailModel()
            udtEHSTransactionDetail.SchemeCode = udtSchemeClaimModel.SchemeCode
            udtEHSTransactionDetail.SchemeSeq = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SchemeSeq
            udtEHSTransactionDetail.SubsidizeCode = strSubsidizeCode
            udtEHSTransactionDetail.SubsidizeItemCode = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeItemCode

            Dim udtSubsidizeItemDetailList As SubsidizeItemDetailsModelCollection = Me._udtSchemeDetailBLL.getSubsidizeItemDetails(udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeItemCode)
            udtEHSTransactionDetail.AvailableItemCode = udtSubsidizeItemDetailList(0).AvailableItemCode
            udtEHSTransactionDetail.Unit = Nothing
            udtEHSTransactionDetail.PerUnitValue = Nothing
            udtEHSTransactionDetail.TotalAmount = Nothing
            udtEHSTransactionDetail.Remark = String.Empty
            udtEHSTransactionDetail.ExchangeRate_Value = udtEHSTransactionModel.ExchangeRate
            udtEHSTransactionDetail.TotalAmountRMB = udtEHSTransactionModel.VoucherClaimRMB

            udtEHSTransactionModel.TransactionDetails.Add(udtEHSTransactionDetail)

            udtEHSTransactionModel.VoucherBeforeRedeem = 0
            udtEHSTransactionModel.VoucherAfterRedeem = 0
            udtEHSTransactionModel.ClaimAmount = Nothing

        End Sub

        ''' <summary>
        ''' Construct EHS Transaction Detail For COVID19MEC
        ''' </summary>
        ''' <param name="udtSP"></param>
        ''' <param name="udtDataEntry"></param>
        ''' <param name="udtEHSTransactionModel"></param>
        ''' <param name="udtEHSAccount"></param>
        ''' <remarks></remarks>
        Public Sub ConstructEHSTransactionDetail_MEC(ByVal udtSP As ServiceProviderModel, _
                                                     ByVal udtDataEntry As DataEntryUserModel, _
                                                     ByRef udtEHSTransactionModel As EHSTransactionModel, _
                                                     ByVal udtEHSAccount As EHSAccountModel)

            Dim udtSchemeClaimModel As SchemeClaimModel = Me._udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSTransactionModel.SchemeCode, udtEHSTransactionModel.ServiceDate.AddDays(1).AddMinutes(-1))

            ' VoucherTransaction
            If udtDataEntry Is Nothing Then
                If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                    udtEHSTransactionModel.RecordStatus = udtSchemeClaimModel.ConfirmedTransactionStatus
                    udtEHSTransactionModel.VoucherAccID = udtEHSAccount.VoucherAccID
                Else
                    udtEHSTransactionModel.RecordStatus = EHSTransactionModel.TransRecordStatusClass.PendingVRValidate
                    udtEHSTransactionModel.TempVoucherAccID = udtEHSAccount.VoucherAccID
                End If

                udtEHSTransactionModel.CreateBy = udtSP.SPID
                udtEHSTransactionModel.UpdateBy = udtSP.SPID
                udtEHSTransactionModel.DataEntryBy = String.Empty
            Else

                If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                    udtEHSTransactionModel.VoucherAccID = udtEHSAccount.VoucherAccID
                Else
                    udtEHSTransactionModel.TempVoucherAccID = udtEHSAccount.VoucherAccID
                End If
                udtEHSTransactionModel.RecordStatus = EHSTransactionModel.TransRecordStatusClass.Pending
                udtEHSTransactionModel.CreateBy = udtSP.SPID
                udtEHSTransactionModel.UpdateBy = udtSP.SPID
                udtEHSTransactionModel.DataEntryBy = udtDataEntry.DataEntryAccount
            End If

            udtEHSTransactionModel.TransactionDtm = Now
            udtEHSTransactionModel.DocCode = udtEHSAccount.SearchDocCode

            udtEHSTransactionModel.TransactionDetails = New TransactionDetailModelCollection()

            ' ------------------------------------------------------------------------
            ' Construct the Detail usign the Active Scheme & Subsidize By Service date 
            ' ------------------------------------------------------------------------
            Dim udtEHSTransactionDetail As New EHSTransaction.TransactionDetailModel()
            udtEHSTransactionDetail.SchemeCode = udtSchemeClaimModel.SchemeCode
            udtEHSTransactionDetail.SchemeSeq = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SchemeSeq
            udtEHSTransactionDetail.SubsidizeCode = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeCode
            udtEHSTransactionDetail.SubsidizeItemCode = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeItemCode

            Dim udtSubsidizeItemDetailList As SubsidizeItemDetailsModelCollection = Me._udtSchemeDetailBLL.getSubsidizeItemDetails(udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeItemCode)
            udtEHSTransactionDetail.AvailableItemCode = udtSubsidizeItemDetailList(0).AvailableItemCode
            udtEHSTransactionDetail.Unit = Nothing
            udtEHSTransactionDetail.PerUnitValue = Nothing
            udtEHSTransactionDetail.TotalAmount = Nothing
            udtEHSTransactionDetail.Remark = String.Empty
            udtEHSTransactionDetail.ExchangeRate_Value = udtEHSTransactionModel.ExchangeRate
            udtEHSTransactionDetail.TotalAmountRMB = udtEHSTransactionModel.VoucherClaimRMB

            udtEHSTransactionModel.TransactionDetails.Add(udtEHSTransactionDetail)

            udtEHSTransactionModel.VoucherBeforeRedeem = 0
            udtEHSTransactionModel.VoucherAfterRedeem = 0
            udtEHSTransactionModel.ClaimAmount = Nothing

        End Sub


        ' CRE13-001 - EHAPP [Start][Tommy L]
        ' -------------------------------------------------------------------------------------
        ''' <summary>
        ''' Construct EHS Transaction Detail For Registration
        ''' </summary>
        ''' <param name="udtSP"></param>
        ''' <param name="udtDataEntry"></param>
        ''' <param name="udtEHSTransactionModel"></param>
        ''' <param name="udtEHSAccount"></param>
        ''' <remarks></remarks>
        Public Sub ConstructEHSTransDetail_Registration(ByVal udtSP As ServiceProviderModel, ByVal udtDataEntry As DataEntryUserModel, _
                                                        ByRef udtEHSTransactionModel As EHSTransactionModel, ByVal udtEHSAccount As EHSAccountModel)

            Dim udtSchemeClaimModel As SchemeClaimModel = Me._udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSTransactionModel.SchemeCode, udtEHSTransactionModel.ServiceDate.AddDays(1).AddMinutes(-1))
            Dim udtSubsidizeItemDetailList As SubsidizeItemDetailsModelCollection = Me._udtSchemeDetailBLL.getSubsidizeItemDetails(udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeItemCode)

            If udtDataEntry Is Nothing Then
                If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                    udtEHSTransactionModel.RecordStatus = udtSchemeClaimModel.ConfirmedTransactionStatus
                    udtEHSTransactionModel.VoucherAccID = udtEHSAccount.VoucherAccID
                Else
                    udtEHSTransactionModel.RecordStatus = EHSTransactionModel.TransRecordStatusClass.PendingVRValidate
                    udtEHSTransactionModel.TempVoucherAccID = udtEHSAccount.VoucherAccID
                End If

                udtEHSTransactionModel.CreateBy = udtSP.SPID
                udtEHSTransactionModel.UpdateBy = udtSP.SPID
                udtEHSTransactionModel.DataEntryBy = String.Empty
            Else

                If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                    udtEHSTransactionModel.VoucherAccID = udtEHSAccount.VoucherAccID
                Else
                    udtEHSTransactionModel.TempVoucherAccID = udtEHSAccount.VoucherAccID
                End If
                udtEHSTransactionModel.RecordStatus = EHSTransactionModel.TransRecordStatusClass.Pending


                udtEHSTransactionModel.CreateBy = udtSP.SPID
                udtEHSTransactionModel.UpdateBy = udtSP.SPID
                udtEHSTransactionModel.DataEntryBy = udtDataEntry.DataEntryAccount
            End If

            udtEHSTransactionModel.TransactionDtm = Now

            If New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(udtEHSTransactionModel.SchemeCode) <> SchemeClaimModel.EnumControlType.EHAPP Then
                Throw New Exception(String.Format("Invalid scheme code ({0})", udtEHSTransactionModel.SchemeCode))
            End If

            udtEHSTransactionModel.DocCode = udtEHSAccount.SearchDocCode
            udtEHSTransactionModel.TransactionDetails = New TransactionDetailModelCollection()

            ' TransactionDetail
            Dim udtEHSTransactionDetail As New EHSTransaction.TransactionDetailModel()

            udtEHSTransactionDetail.SchemeCode = udtSchemeClaimModel.SchemeCode
            udtEHSTransactionDetail.SchemeSeq = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SchemeSeq
            udtEHSTransactionDetail.SubsidizeCode = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeCode
            udtEHSTransactionDetail.SubsidizeItemCode = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeItemCode
            udtEHSTransactionDetail.AvailableItemCode = udtSubsidizeItemDetailList(0).AvailableItemCode
            udtEHSTransactionDetail.Unit = 1
            udtEHSTransactionDetail.PerUnitValue = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeNA).SubsidizeFee
            If udtEHSTransactionDetail.PerUnitValue.HasValue Then
                udtEHSTransactionDetail.TotalAmount = udtEHSTransactionDetail.PerUnitValue.Value * udtEHSTransactionDetail.Unit.Value
            Else
                udtEHSTransactionDetail.TotalAmount = Nothing
            End If
            udtEHSTransactionDetail.Remark = ""

            udtEHSTransactionModel.TransactionDetails.Add(udtEHSTransactionDetail)

            ' VoucherTransaction
            udtEHSTransactionModel.VoucherBeforeRedeem = 0
            udtEHSTransactionModel.VoucherAfterRedeem = 0
            udtEHSTransactionModel.ClaimAmount = udtEHSTransactionDetail.TotalAmount
        End Sub
        ' CRE13-001 - EHAPP [End][Tommy L]
#End Region

#Region "Other Supporting Function"

        Public Function calTotalAmount(ByVal strVoucherRedeem As String, ByVal intVoucherValue As Integer) As Integer
            Dim intRes As Integer = 0
            Dim intVoucherRedeem As Integer
            Try
                intVoucherRedeem = CInt(strVoucherRedeem)
                If intVoucherRedeem > 0 Then
                    intRes = intVoucherRedeem * intVoucherValue
                Else
                    intRes = 0
                End If
            Catch ex As Exception
                intRes = 0
            End Try
            Return intRes
        End Function

        Public Function chkIsTSWCase(ByVal strSPID As String, ByVal strHKID As String) As Boolean
            Dim blnRes As Boolean = False
            Dim udtdb As Database = New Database()
            Dim dt As DataTable = New DataTable()

            Dim udtFormatter As New Common.Format.Formatter()
            strHKID = udtFormatter.formatDocumentIdentityNumber(Common.Component.DocType.DocTypeModel.DocTypeCode.HKIC, strHKID.Trim())

            Try
                Dim prams() As SqlParameter = { _
                    udtdb.MakeInParam("@GP_SPID", SqlDbType.Char, 8, strSPID), _
                    udtdb.MakeInParam("@HKID", SqlDbType.Char, 9, strHKID)}

                udtdb.RunProc("proc_TSWPatientMapping_bySPIDHKID", prams, dt)

                If dt.Rows(0).Item(0) > 0 Then
                    blnRes = True
                Else
                    blnRes = False
                End If
            Catch ex As Exception
                Throw
            End Try
            Return blnRes
        End Function

        ''' <summary>
        ''' Check whether any subsidizes inside the scheme is Vaccine type
        ''' </summary>
        ''' <param name="udtSubsidizeList"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <author>tmk791</author>
        Public Function SchemeContainVaccine(ByVal udtSubsidizeList As SubsidizeGroupClaimModelCollection) As Boolean
            For Each udtSubsidize As SubsidizeGroupClaimModel In udtSubsidizeList
                If udtSubsidize.SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVaccine Then Return True
            Next

            Return False

        End Function

        Public Function CustomHandleSystemMessage(ByVal lstClaimResult As List(Of ClaimRulesBLL.ClaimRuleResult), _
                                                  ByVal udtInputVaccineCollection As InputVaccineModelCollection, _
                                                  ByRef lstStrResSystemMessage As List(Of String), _
                                                  ByRef blnHasResSystemMessage As Boolean) As String
            Dim strMsgCode As String = String.Empty

            For Each udtClaimResult As ClaimRulesBLL.ClaimRuleResult In lstClaimResult
                Select Case udtClaimResult.RelatedClaimRule.MessageCode
                    Case "00242"
                        'If occurs, stop checking and show the warning message
                        strMsgCode = "00242"

                        Dim blnFormerDisplay As Boolean = False
                        Dim blnLatterDisplay As Boolean = False

                        For Each udtInputVaccine As InputVaccineModel In udtInputVaccineCollection.Values
                            If Not blnFormerDisplay AndAlso udtInputVaccine.SubsidizeCode = udtClaimResult.RelatedClaimRule.SubsidizeCode.Trim Then
                                lstStrResSystemMessage.Add(udtInputVaccine.DisplayCodeForClaim)
                                blnFormerDisplay = True
                            End If

                            If Not blnLatterDisplay AndAlso udtInputVaccine.SubsidizeCode = udtClaimResult.RelatedClaimRule.CompareValue.Trim Then
                                lstStrResSystemMessage.Add(udtInputVaccine.DisplayCodeForClaim)
                                blnLatterDisplay = True
                            End If
                        Next

                        blnHasResSystemMessage = True

                        Exit For
                    Case Else
                        'Nothing to do
                End Select
            Next

            Return strMsgCode

        End Function

#End Region

#Region "Get Vaccine"
        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Function GetVaccinationRecord(ByVal udtEHSAccount As EHSAccountModel, ByRef udtTranDetailVaccineList As TransactionDetailVaccineModelCollection, _
                                             ByVal strFunctionCode As String, ByVal udtAuditLogEntry As AuditLogEntry, _
                                             Optional ByVal strSchemeCode As String = "") As VaccineResultCollection

            Dim udtVaccinationBLL As New VaccinationBLL
            Dim udtEHSTransactionBLL As New EHSTransactionBLL
            Dim udtSession As New BLL.SessionHandler

            Dim htRecordSummary As Hashtable = Nothing
            'HA CMS
            Dim udtHAVaccineResult As Common.WebService.Interface.HAVaccineResult = New HAVaccineResult(HAVaccineResult.enumReturnCode.Error)
            Dim udtHAVaccineResultSession As Common.WebService.Interface.HAVaccineResult = Nothing

            udtHAVaccineResultSession = udtSession.CMSVaccineResultGetFromSession(strFunctionCode)
            If udtHAVaccineResultSession IsNot Nothing Then
                If udtHAVaccineResultSession.ReturnCode <> Common.WebService.Interface.HAVaccineResult.enumReturnCode.SuccessWithData Then
                    udtHAVaccineResultSession = Nothing
                End If
            End If

            'DH CIMS
            Dim udtDHVaccineResult As Common.WebService.Interface.DHVaccineResult = New DHVaccineResult(DHVaccineResult.enumReturnCode.UnexpectedError)
            Dim udtDHVaccineResultSession As Common.WebService.Interface.DHVaccineResult = Nothing

            udtDHVaccineResultSession = udtSession.CIMSVaccineResultGetFromSession(strFunctionCode)
            If udtDHVaccineResultSession IsNot Nothing Then
                If udtDHVaccineResultSession.ReturnCode <> DHVaccineResult.enumReturnCode.Success Then
                    udtDHVaccineResultSession = Nothing
                End If
            End If

            Dim udtVaccineResultBag As New VaccineResultCollection
            udtVaccineResultBag.DHVaccineResult = udtDHVaccineResult
            udtVaccineResultBag.HAVaccineResult = udtHAVaccineResult

            Dim udtVaccineResultBagSession As New VaccineResultCollection
            udtVaccineResultBagSession.DHVaccineResult = udtDHVaccineResultSession
            udtVaccineResultBagSession.HAVaccineResult = udtHAVaccineResultSession

            ' Try to enquiry CMS latest record and eHS record 
            udtVaccinationBLL.GetVaccinationRecord(udtEHSAccount, udtTranDetailVaccineList, udtVaccineResultBag, htRecordSummary, udtAuditLogEntry, strSchemeCode, udtVaccineResultBagSession)

            If udtVaccineResultBag.HAReturnStatus <> VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail Then
                ' if Success, then use latest record for bar
                udtSession.CMSVaccineResultSaveToSession(udtVaccineResultBag.HAVaccineResult, strFunctionCode)
                udtTranDetailVaccineList.Sort(TransactionDetailVaccineModelCollection.enumSortBy.ServiceDate, SortDirection.Descending)
            End If

            If udtVaccineResultBag.DHReturnStatus <> VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail Then
                ' if Success, then use latest record for bar
                udtSession.CIMSVaccineResultSaveToSession(udtVaccineResultBag.DHVaccineResult, strFunctionCode)
                udtTranDetailVaccineList.Sort(TransactionDetailVaccineModelCollection.enumSortBy.ServiceDate, SortDirection.Descending)
            End If

            Return udtVaccineResultBag
        End Function
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

#End Region

    End Class
End Namespace
