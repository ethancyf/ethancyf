Imports Common.ComFunction
Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.ClaimRules
Imports Common.Component.DataEntryUser
Imports Common.Component.DocType
Imports Common.Component.EHSAccount
Imports Common.Component.EHSClaimVaccine
Imports Common.Component.EHSTransaction
Imports Common.Component.InputPicker
Imports Common.Component.Practice
Imports Common.Component.Scheme
Imports Common.Component.SchemeDetails
Imports Common.Component.ServiceProvider
Imports Common.Component.VoucherInfo
Imports Common.DataAccess
Imports Common.Format
Imports Common.WebService.Interface
Imports System.Data.SqlClient

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

#Region "Insert / Update EHS Account / EHSTransaction"

        ' CRE20-003 (Batch Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Create Temporary EHS Account, Save to Database
        ''' </summary>
        ''' <param name="strSPID"></param>
        ''' <param name="udtEHSAccount"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateTemporaryEHSAccount(ByVal strSPID As String, ByVal udtEHSAccount As EHSAccountModel) As Common.ComObject.SystemMessage
            Dim udtEHSAccountBLL As New EHSAccountBLL
            Dim udtErrorMsg As Common.ComObject.SystemMessage = Nothing

            udtEHSAccount.AccountPurpose = EHSAccountModel.AccountPurposeClass.ForClaim

            udtEHSAccount.DataEntryBy = String.Empty
            udtEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingVerify
            udtEHSAccount.EHSPersonalInformationList(0).DataEntryBy = String.Empty

            ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
            If Not (New DocTypeBLL).getAllDocType.Filter(udtEHSAccount.EHSPersonalInformationList(0).DocCode).IMMDorManualValidationAvailable Then
                udtEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.NotForImmDValidation
            End If
            ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]

            udtEHSAccount.CreateSPID = strSPID
            udtEHSAccount.CreateBy = strSPID
            udtEHSAccount.EHSPersonalInformationList(0).RecordStatus = "N"
            udtEHSAccount.EHSPersonalInformationList(0).CreateBy = strSPID
            udtEHSAccount.SubsidizeWriteOff_CreateReason = eHASubsidizeWriteOff_CreateReason.PersonalInfoCreation

            Dim udtDB As New Database()

            Try
                udtDB.BeginTransaction()

                udtErrorMsg = udtEHSAccountBLL.InsertEHSAccount(udtDB, udtEHSAccount)

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

            Return udtErrorMsg
        End Function
        ' CRE20-003 (Batch Upload) [End][Chris YIM]


        ' CRE20-003 (Batch Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Function CreateRectifyAccount(ByVal strSPID As String, _
                                             ByVal udtEHSXAccount As EHSAccountModel, _
                                             ByVal udtEHSNewAccount As EHSAccountModel) As Common.ComObject.SystemMessage

            Dim udtEHSAccountBLL As New EHSAccountBLL
            Dim udtEHSTransactionBLL As New EHSTransactionBLL
            Dim udtSubsidizeWriteOffBLL As New SubsidizeWriteOffBLL

            ' Rectify X Account, remove X Account and make a new EHSAccount
            udtEHSNewAccount.DataEntryBy = String.Empty
            udtEHSNewAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingVerify
            udtEHSNewAccount.EHSPersonalInformationList(0).DataEntryBy = String.Empty

            ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
            If Not (New DocTypeBLL).getAllDocType.Filter(udtEHSNewAccount.EHSPersonalInformationList(0).DocCode).IMMDorManualValidationAvailable Then
                udtEHSNewAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.NotForImmDValidation
            End If
            ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]

            udtEHSNewAccount.CreateBy = strSPID
            udtEHSNewAccount.EHSPersonalInformationList(0).CreateBy = strSPID
            udtEHSNewAccount.CreateSPID = strSPID

            Dim udtDB As New Database()
            Dim udtErrorMsg As Common.ComObject.SystemMessage = Nothing

            Try
                udtDB.BeginTransaction()

                ' Since X Account removed, checking will ignore X Account
                udtEHSAccountBLL.UpdateTempEHSAccountRecordStatus(udtDB, udtEHSXAccount, strSPID, EHSAccountModel.TempAccountRecordStatusClass.Removed, DateTime.Now)


                udtEHSTransactionBLL.UpdateTransactionWithNewTemporaryAccount(udtDB, _
                                                                              udtEHSXAccount.TransactionID, _
                                                                              udtEHSXAccount.VoucherAccID, _
                                                                              udtEHSNewAccount.VoucherAccID, _
                                                                              udtEHSNewAccount.EHSPersonalInformationList(0).DocCode, _
                                                                              udtEHSNewAccount.EHSPersonalInformationList(0).IdentityNum, _
                                                                              udtEHSNewAccount.CreateBy)


                udtErrorMsg = udtEHSAccountBLL.InsertEHSAccount(udtDB, udtEHSNewAccount)

                If udtErrorMsg Is Nothing Then
                    'Handle write-off
                    Call udtSubsidizeWriteOffBLL.DeleteAccountWriteOff(udtEHSXAccount.EHSPersonalInformationList, eHASubsidizeWriteOff_CreateReason.PersonalInfoAmend, udtDB)
                End If

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
        ' CRE20-003 (Batch Upload) [End][Chris YIM]

        ''' <summary>
        ''' Claim with Validated Account
        ''' </summary>
        ''' <param name="udtEHSTransactionModel"></param>
        ''' <param name="udtEHSAccountModel"></param>
        ''' <param name="udtEHSPersonalInfo"></param>
        ''' <param name="udtSchemeClaimModel"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateEHSTransaction(ByVal udtEHSTransactionModel As EHSTransactionModel, ByVal udtEHSAccountModel As EHSAccountModel, _
            ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, ByVal udtSchemeClaimModel As SchemeClaimModel) As Common.ComObject.SystemMessage


            Dim udtDB As New Database()
            Try
                udtDB.BeginTransaction()

                Dim udtSystemMessage As Common.ComObject.SystemMessage = Me._udtEHSTransactionBLL.InsertEHSTransactionWithoutChecking(udtEHSTransactionModel, udtEHSAccountModel, _
                                                                        udtEHSPersonalInfo, udtSchemeClaimModel)

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
        ''' <param name="udtInputPicker"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SearchEHSClaimVaccine(ByVal udtSchemeClaimModel As SchemeClaimModel, ByVal strDocCode As String, ByVal udtEHSAccountModel As EHSAccountModel, _
                                              ByVal dtmServiceDate As Date, ByVal blnDynamicControl As Boolean, _
                                              ByVal udtInputPicker As InputPickerModel) As EHSClaimVaccineModel

            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            'Dim udtEHSClaimVaccineModel As New EHSClaimVaccineModel(udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SchemeSeq)
            Dim udtEHSClaimVaccineModel As New EHSClaimVaccineModel(udtSchemeClaimModel.SchemeCode)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            ' Modify to retrieve all used subsidy
            ' Recipient used benefit
            Dim udtTransactionDetailList As TransactionDetailModelCollection = Me._udtEHSTransactionBLL.getTransactionDetailBenefit(strDocCode, udtEHSAccountModel.getPersonalInformation(strDocCode).IdentityNum)
            'Dim udtTransactionDetailList As TransactionDetailModelCollection = Me._udtEHSTransactionBLL.getTransactionDetailBenefit(strDocCode, udtEHSAccountModel.getPersonalInformation(strDocCode).IdentityNum, udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SchemeSeq)
            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

            ' Auto Selected If only 1 Entry found
            Dim intEntryCount As Integer = 0

            'CRE16-026 (Add PCV13) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            If udtInputPicker Is Nothing Then
                udtInputPicker = New InputPickerModel
                udtInputPicker.CategoryCode = String.Empty
            End If

            '---------------------------------
            ' Vaccine->Subsidize
            '---------------------------------
            ' By the selected service date, determine whether the subsidize is available for the patient
            For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtSchemeClaimModel.SubsidizeGroupClaimList

                ' ''TO:Do Remove checking about the period
                'If blnDynamicControl Then
                '    ' Out of Service Period, Or Not Eligible
                '    If udtSubsidizeGroupClaim.LastServiceDtm < dtmServiceDate OrElse Not Me._udtClaimRulesBLL.CheckEligibilityPerSubsidize(udtSubsidizeGroupClaim, udtEHSAccountModel.getPersonalInformation(strDocCode).DOB, udtEHSAccountModel.getPersonalInformation(strDocCode).ExactDOB, dtmServiceDate, udtTransactionDetailList).IsEligible Then
                '        Continue For
                '    End If
                'End If

                If udtInputPicker.CategoryCode.Trim() <> "" Then
                    ' If Category is passed, Check Exist of Category
                    Dim udtClaimCategoryBLL As New ClaimCategory.ClaimCategoryBLL()
                    If udtClaimCategoryBLL.getAllCategoryCache().Filter(udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, udtInputPicker.CategoryCode) Is Nothing Then
                        Continue For
                    Else
                        ' Category Exist, Check for Category Eligibility for the Subsidize
                        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                        ' -----------------------------------------------------------------------------------------
                        Dim udtEligibilityRuleResult As ClaimRulesBLL.EligibleResult = Me._udtClaimRulesBLL.CheckCategoryEligibilityByCategory(udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, udtInputPicker.CategoryCode, udtEHSAccountModel.getPersonalInformation(strDocCode), dtmServiceDate)
                        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                        If Not udtEligibilityRuleResult.IsEligible Then
                            'Continue For
                        End If
                    End If
                End If

                ' Vaccine Fee
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                'Dim udtSchemeVaccineDetailModel As SchemeVaccineDetailModel = Me._udtSchemeDetailBLL.getSchemeVaccineDetail(udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode)
                Dim udtSubsidizeFeeVaccineModel As SubsidizeFeeModel = udtSubsidizeGroupClaim.SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVaccine, dtmServiceDate)
                Dim udtSubsidizeFeeInjectionModel As SubsidizeFeeModel = udtSubsidizeGroupClaim.SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeInjection, dtmServiceDate)

                'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
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

                Dim udtSubsidizeGroupClaimItemDetailList As SubsidizeGroupClaimItemDetailsModelCollection = Me._udtSchemeDetailBLL.getSubsidizeGroupClaimItemDetails(udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, udtSubsidizeGroupClaim.SubsidizeItemCode)

                Dim strRemark As String = String.Empty
                Dim strRemarkChi As String = String.Empty
                Dim strRemarkCN As String = String.Empty
                Dim arrStrAvailableCode As New List(Of String)

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
                'CRE16-002 (Revamp VSS) [End][Chris YIM]

                ' Checking For Each SubsidizeItemCode: Eg. HSIV->1STDOSE, 2NDDOSE, ONLYDOSE

                ' Vaccine->Subsidze->SubsidizeDetail
                For Each udtSubsidizeGroupClaimItemDetail As SubsidizeGroupClaimItemDetailsModel In udtSubsidizeGroupClaimItemDetailList
                    'For Each udtSubsidizeItemDetail As SubsidizeItemDetailsModel In udtSubsidizeItemDetailList

                    ''Dim blnDisplayOnly As Boolean = False
                    ''Dim blnSkipSubsidizeItemDetail As Boolean = False
                    ' '' To Do: Here Will Use the Transaction For The Scheme Instead of the Transaction for Subsidize, Any Problem ????!!

                    ''Dim udtDoseRuleResult As ClaimRulesBLL.DoseRuleResult = Me._udtClaimRulesBLL.CheckSubsidizeItemDetailRuleByDose(udtTransactionDetailList, _
                    ''    udtSchemeClaimModel.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, _
                    ''    udtSubsidizeGroupClaimItemDetail.SubsidizeItemCode, udtSubsidizeGroupClaimItemDetail.AvailableItemCode, udtEHSAccountModel.getPersonalInformation(strDocCode).DOB, udtEHSAccountModel.getPersonalInformation(strDocCode).ExactDOB, dtmServiceDate)

                    ' ''If udtDoseRuleResult.IsMatch AndAlso (udtDoseRuleResult.HandlingMethod = ClaimRulesBLL.DoseRuleHandlingMethod.ALL Or udtDoseRuleResult.HandlingMethod = ClaimRulesBLL.DoseRuleHandlingMethod.DISPLAY) Then
                    ' ''    If udtDoseRuleResult.HandlingMethod = ClaimRulesBLL.DoseRuleHandlingMethod.DISPLAY Then
                    ' ''        blnDisplayOnly = True
                    ' ''    End If
                    ' ''Else
                    ' ''    ' Not for Select
                    ' ''    'Continue For
                    ' ''    blnSkipSubsidizeItemDetail = True
                    ' ''End If

                    ' ''-------------------------------------------------------------------
                    ' '' Check with Exact Match Transaction
                    ' ''-------------------------------------------------------------------
                    ''Dim udtSearchedTranDetailList As TransactionDetailModelCollection = udtTransactionDetailList.FilterBySubsidizeItemDetail( _
                    ''    udtSchemeClaimModel.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, _
                    ''    udtSubsidizeGroupClaimItemDetail.SubsidizeItemCode, udtSubsidizeGroupClaimItemDetail.AvailableItemCode)
                    ' ''-------------------------------------------------------------------
                    ' '' Check with Equivalent Dose related Transaction (equivalent dose from EqvSubsidizeMap)
                    ' ''-------------------------------------------------------------------

                    ' '' Dose: SchemeCode, SchemeSeq, SubsidizeCode, SubsidizeItemCode, AvailableItemCode <=> Eqv * 5
                    ' ''CRE16-026 (Add PCV13) [Start][Chris YIM]
                    ' ''-----------------------------------------------------------------------------------------
                    ''Dim udtEqvSubsidizeMapList As EqvSubsidizeMapModelCollection = Me._udtSchemeDetailBLL.getALLEqvSubsidizeMap().Filter( _
                    ''    udtSchemeClaimModel.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaimItemDetail.SubsidizeItemCode)
                    ' ''CRE16-026 (Add PCV13) [End][Chris YIM]

                    ' '' Merge the Transaction
                    ''For Each udtEqvSubsidizeMapModel As EqvSubsidizeMapModel In udtEqvSubsidizeMapList
                    ''    Dim udtEquMergeTranDetailList As TransactionDetailModelCollection = udtTransactionDetailList.FilterBySubsidizeItemDetail( _
                    ''        udtEqvSubsidizeMapModel.EqvSchemeCode, udtEqvSubsidizeMapModel.EqvSchemeSeq, udtEqvSubsidizeMapModel.EqvSubsidizeItemCode)

                    ''    For Each udtTranDetail As TransactionDetailModel In udtEquMergeTranDetailList
                    ''        ' CRE16-002 Revamp VSS [Start][Lawrence]
                    ''        ' Avoid duplicate
                    ''        If Not udtSearchedTranDetailList.Contains(udtTranDetail) Then
                    ''            udtSearchedTranDetailList.Add(New TransactionDetailModel(udtTranDetail))
                    ''        End If
                    ''        ' CRE16-002 Revamp VSS [End][Lawrence]
                    ''    Next
                    ''Next

                    ' Back Office:
                    ' Display the available dose for selection without consider the SubsidizeItemDetailsRule (Dose Rule)
                    Dim blnReceived As Boolean = False
                    Dim blnAvailable As Boolean = True

                    ''If Not blnSkipSubsidizeItemDetail Then
                    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                    Dim udtEHSClaimSubsidizeDetailModel As New EHSClaimVaccineModel.EHSClaimSubidizeDetailModel( _
                                            udtSubsidizeGroupClaimItemDetail.SubsidizeItemCode, udtSubsidizeGroupClaimItemDetail.AvailableItemCode, udtSubsidizeGroupClaimItemDetail.DisplaySeq, _
                                            udtSubsidizeGroupClaimItemDetail.AvailableItemDesc, udtSubsidizeGroupClaimItemDetail.AvailableItemDescChi, udtSubsidizeGroupClaimItemDetail.AvailableItemNum, _
                                            udtSubsidizeGroupClaimItemDetail.InternalUse, blnAvailable, blnReceived, False)
                    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

                    'If udtSearchedTranDetailList.Count > 0 Then
                    '    udtEHSClaimSubsidizeDetailModel.DoseDate = udtSearchedTranDetailList(0).ServiceReceiveDtm
                    'End If
                    udtEHSClaimSubsidizeModel.Add(udtEHSClaimSubsidizeDetailModel)
                    ''End If
                Next

                udtEHSClaimSubsidizeModel.Remark = strRemark
                udtEHSClaimSubsidizeModel.RemarkChi = strRemarkChi

                ' If Vaccine->Subsidze contain at least 1 SubsidizeDetail 
                If Not udtEHSClaimSubsidizeModel.SubsidizeDetailList Is Nothing AndAlso udtEHSClaimSubsidizeModel.SubsidizeDetailList.Count > 0 Then
                    udtEHSClaimVaccineModel.Add(udtEHSClaimSubsidizeModel)
                    intEntryCount = intEntryCount + 1
                End If

                'CRE16-026 (Add PCV13) [End][Chris YIM]
            Next

            If intEntryCount = 1 Then
                udtEHSClaimVaccineModel.SubsidizeList(0).Selected = True
            End If

            Return udtEHSClaimVaccineModel
        End Function

#End Region

#Region "Create Model"

        ' Transaction Detail ------------------------------

        ''' <summary>
        ''' Construct transacton details for voucher scheme
        ''' </summary>
        ''' <param name="udtEHSTransactionModel"></param>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="strBOUserID"></param>
        ''' <remarks></remarks>
        Public Sub ConstructEHSTransactionDetails(ByRef udtEHSTransactionModel As EHSTransactionModel, ByVal udtEHSAccount As EHSAccountModel, ByVal strBOUserID As String)

            Dim udtSchemeClaimModel As SchemeClaimModel = Me._udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSTransactionModel.SchemeCode, udtEHSTransactionModel.ServiceDate.AddDays(1).AddMinutes(-1))

            ' VoucherTransaction
            udtEHSTransactionModel.CreateBy = strBOUserID
            udtEHSTransactionModel.UpdateBy = strBOUserID
            udtEHSTransactionModel.DataEntryBy = String.Empty
            udtEHSTransactionModel.RecordStatus = udtSchemeClaimModel.ConfirmedTransactionStatus
            udtEHSTransactionModel.VoucherAccID = udtEHSAccount.VoucherAccID
            udtEHSTransactionModel.ManualReimburse = True
            udtEHSTransactionModel.DocCode = udtEHSAccount.SearchDocCode
            udtEHSTransactionModel.TransactionDetails = New TransactionDetailModelCollection()

            Dim udtEHSTransactionDetail As New EHSTransaction.TransactionDetailModel()
            Dim udtEHSTransactionBLL As New EHSTransactionBLL

            ' ------------------------------------------------------------------------
            ' Construct the Detail using the Active Scheme & Subsidize By Service date 
            ' ------------------------------------------------------------------------
            udtEHSTransactionDetail.SchemeCode = udtSchemeClaimModel.SchemeCode
            udtEHSTransactionDetail.SchemeSeq = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SchemeSeq
            udtEHSTransactionDetail.SubsidizeCode = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeCode
            udtEHSTransactionDetail.SubsidizeItemCode = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeItemCode

            Dim udtSubsidizeItemDetailList As SubsidizeItemDetailsModelCollection = Me._udtSchemeDetailBLL.getSubsidizeItemDetails(udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeItemCode)
            udtEHSTransactionDetail.AvailableItemCode = udtSubsidizeItemDetailList(0).AvailableItemCode
            udtEHSTransactionDetail.Unit = udtEHSTransactionModel.VoucherClaim
            udtEHSTransactionDetail.PerUnitValue = udtEHSTransactionModel.PerVoucherValue
            udtEHSTransactionDetail.TotalAmount = udtEHSTransactionDetail.Unit.Value * udtEHSTransactionDetail.PerUnitValue.Value
            udtEHSTransactionDetail.Remark = ""

            If udtEHSTransactionModel.ExchangeRate.HasValue = True Then
                udtEHSTransactionDetail.ExchangeRate_Value = udtEHSTransactionModel.ExchangeRate
                udtEHSTransactionDetail.TotalAmountRMB = udtEHSTransactionModel.VoucherClaimRMB
            End If

            udtEHSTransactionModel.TransactionDetails.Add(udtEHSTransactionDetail)

            ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Dim udtVoucherInfo As New VoucherInfoModel(VoucherInfoModel.AvailableVoucher.Include, _
                                                       VoucherInfoModel.AvailableQuota.None)

            udtVoucherInfo.GetInfo(udtEHSTransactionModel.ServiceDate, udtSchemeClaimModel, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode))

            udtEHSTransactionModel.VoucherBeforeRedeem = udtVoucherInfo.GetAvailableVoucher()
            ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

            udtEHSTransactionModel.VoucherAfterRedeem = udtEHSTransactionModel.VoucherBeforeRedeem - udtEHSTransactionModel.VoucherClaim
            udtEHSTransactionModel.ClaimAmount = udtEHSTransactionDetail.TotalAmount

        End Sub

        ''' <summary>
        ''' Construct transaction details for vaccination scheme
        ''' </summary>
        ''' <param name="udtEHSTransactionModel"></param>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="udtEHSClaimVaccineModel"></param>
        ''' <param name="strBOUserID"></param>
        ''' <remarks></remarks>
        Public Sub ConstructEHSTransactionDetails(ByRef udtEHSTransactionModel As EHSTransactionModel, ByVal udtEHSAccount As EHSAccountModel, ByRef udtEHSClaimVaccineModel As EHSClaimVaccineModel, ByVal strBOUserID As String)

            ' CRE13-001 - EHAPP [Start][Tommy L]
            ' -------------------------------------------------------------------------------------
            Dim udtSchemeClaimModel As SchemeClaimModel = Me._udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSClaimVaccineModel.SchemeCode, udtEHSTransactionModel.ServiceDate.AddDays(1).AddMinutes(-1))
            ' CRE13-001 - EHAPP [End][Tommy L]

            Dim dblClaimAmount As Double = 0

            udtEHSTransactionModel.CreateBy = strBOUserID
            udtEHSTransactionModel.UpdateBy = strBOUserID
            udtEHSTransactionModel.DataEntryBy = String.Empty

            ' CRE13-001 - EHAPP [Start][Tommy L]
            ' -------------------------------------------------------------------------------------
            'udtEHSTransactionModel.RecordStatus = EHSTransactionModel.TransRecordStatusClass.Active
            udtEHSTransactionModel.RecordStatus = udtSchemeClaimModel.ConfirmedTransactionStatus
            ' CRE13-001 - EHAPP [End][Tommy L]
            udtEHSTransactionModel.VoucherAccID = udtEHSAccount.VoucherAccID

            udtEHSTransactionModel.ManualReimburse = True

            udtEHSTransactionModel.DocCode = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DocCode
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
                        udtEHSTransactionDetail.SchemeSeq = udtSubsidize.SchemeSeq 'udtSchemeClaimModel.SubsidizeGroupClaimList.Filter(udtEHSClaimVaccineModel.SchemeCode, udtSubsidize.SubsidizeCode).SchemeSeq
                        'udtEHSTransactionDetail.SchemeSeq = udtEHSClaimVaccineModel.SchemeSeq
                        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

                        udtEHSTransactionDetail.SubsidizeCode = udtSubsidize.SubsidizeCode
                        udtEHSTransactionDetail.SubsidizeItemCode = udtSubsidize.SubsidizeItemCode
                        udtEHSTransactionDetail.AvailableItemCode = udtSubsidize.SubsidizeDetailList(0).AvailableItemCode
                        udtEHSTransactionDetail.AvailableItemDesc = udtSubsidize.SubsidizeDetailList(0).AvailableItemDesc
                        udtEHSTransactionDetail.AvailableItemDescChi = udtSubsidize.SubsidizeDetailList(0).AvailableItemDescChi
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
                                udtEHSTransactionDetail.SchemeSeq = udtSubsidize.SchemeSeq 'udtSchemeClaimModel.SubsidizeGroupClaimList.Filter(udtEHSClaimVaccineModel.SchemeCode, udtSubsidize.SubsidizeCode).SchemeSeq
                                'udtEHSTransactionDetail.SchemeSeq = udtEHSClaimVaccineModel.SchemeSeq
                                ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

                                udtEHSTransactionDetail.SubsidizeCode = udtSubsidize.SubsidizeCode
                                udtEHSTransactionDetail.SubsidizeItemCode = udtSubsidize.SubsidizeItemCode
                                udtEHSTransactionDetail.AvailableItemCode = udtSubsidizeDetail.AvailableItemCode
                                udtEHSTransactionDetail.AvailableItemDesc = udtSubsidizeDetail.AvailableItemDesc
                                udtEHSTransactionDetail.AvailableItemDescChi = udtSubsidizeDetail.AvailableItemDescChi
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

            udtEHSTransactionModel.ClaimAmount = dblClaimAmount
        End Sub

        ' CRE13-001 - EHAPP [Start][Tommy L]
        ' -------------------------------------------------------------------------------------
        ''' <summary>
        ''' Construct EHS Transaction Detail for Subsidize Type - "Registration"
        ''' </summary>
        ''' <param name="udtEHSTransactionModel"></param>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="strBOUserID"></param>
        ''' <remarks></remarks>
        Public Sub ConstructEHSTransDetail_Registration(ByRef udtEHSTransactionModel As EHSTransactionModel, ByVal udtEHSAccount As EHSAccountModel, ByVal strBOUserID As String)
            Dim udtSchemeClaimModel As SchemeClaimModel = Me._udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSTransactionModel.SchemeCode, udtEHSTransactionModel.ServiceDate.AddDays(1).AddMinutes(-1))

            ' VoucherTransaction
            udtEHSTransactionModel.CreateBy = strBOUserID
            udtEHSTransactionModel.UpdateBy = strBOUserID
            udtEHSTransactionModel.DataEntryBy = String.Empty

            udtEHSTransactionModel.RecordStatus = udtSchemeClaimModel.ConfirmedTransactionStatus
            udtEHSTransactionModel.VoucherAccID = udtEHSAccount.VoucherAccID

            udtEHSTransactionModel.ManualReimburse = True

            udtEHSTransactionModel.DocCode = udtEHSAccount.SearchDocCode
            udtEHSTransactionModel.TransactionDetails = New TransactionDetailModelCollection()

            ' TransactionDetail
            Dim udtEHSTransactionDetail As New EHSTransaction.TransactionDetailModel()

            udtEHSTransactionDetail.SchemeCode = udtSchemeClaimModel.SchemeCode
            udtEHSTransactionDetail.SchemeSeq = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SchemeSeq
            udtEHSTransactionDetail.SubsidizeCode = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeCode
            udtEHSTransactionDetail.SubsidizeItemCode = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeItemCode

            Dim udtSubsidizeItemDetailList As SubsidizeItemDetailsModelCollection = Me._udtSchemeDetailBLL.getSubsidizeItemDetails(udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeItemCode)

            udtEHSTransactionDetail.AvailableItemCode = udtSubsidizeItemDetailList(0).AvailableItemCode
            udtEHSTransactionDetail.Unit = 1
            udtEHSTransactionDetail.PerUnitValue = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeNA).SubsidizeFee
            If udtEHSTransactionDetail.PerUnitValue.HasValue Then
                udtEHSTransactionDetail.TotalAmount = udtEHSTransactionDetail.Unit.Value * udtEHSTransactionDetail.PerUnitValue.Value
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

        Public Function CheckServiceDateClaimPeriod(ByVal strSchemeCode As String, ByVal dtmServiceDate As Date) As Common.ComObject.SystemMessage
            Dim udtResSM As Common.ComObject.SystemMessage = Nothing

            Dim udtSchemeClaimBLL As New SchemeClaimBLL()
            Dim udtSchemeClaimModel As SchemeClaimModel = Nothing

            udtSchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(strSchemeCode, dtmServiceDate)

            Dim udtSystemMessage As New Common.ComObject.SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00258)

            If udtSchemeClaimModel Is Nothing Then
                udtResSM = udtSystemMessage
            Else
                If IsNothing(udtSchemeClaimModel.SubsidizeGroupClaimList) Then
                    udtResSM = udtSystemMessage
                Else
                    If udtSchemeClaimModel.SubsidizeGroupClaimList.Count = 0 Then
                        udtResSM = udtSystemMessage
                    End If

                End If
            End If
            Return udtResSM
        End Function

#End Region

#Region "Get Vaccine"
        ' CRE20-003 (Batch Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Function GetVaccinationRecord(ByVal udtEHSAccount As EHSAccountModel, ByRef udtTranDetailVaccineList As TransactionDetailVaccineModelCollection, _
                                             ByVal strFunctionCode As String, ByVal udtAuditLogEntry As AuditLogEntry, _
                                             Optional ByVal strSchemeCode As String = "") As VaccineResultCollection

            Dim udtVaccinationBLL As New VaccinationBLL
            Dim udtEHSTransactionBLL As New EHSTransactionBLL
            Dim udtSession As New HCVU.BLL.SessionHandlerBLL

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

            udtSession.CMSVaccineResultSaveToSession(udtVaccineResultBag.HAVaccineResult, strFunctionCode)
            udtSession.CIMSVaccineResultSaveToSession(udtVaccineResultBag.DHVaccineResult, strFunctionCode)

            Return udtVaccineResultBag
        End Function
        ' CRE20-003 (Batch Upload) [End][Chris YIM]

#End Region


    End Class
End Namespace
