Imports Common.ComFunction
Imports Common.Component.Scheme
Imports Common.DataAccess
Imports Common.Format
Imports System.Data.SqlClient
Imports System.Data
Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.EHSTransaction
Imports Common.Component.Practice
Imports Common.Component.SchemeDetails



Namespace BLL

    Public Class StudentFileTranBLL

        ''' <summary>
        ''' Construct EHS Transaction Model
        ''' </summary>
        ''' <param name="udtSchemeClaimModel"></param>
        ''' <param name="udtSubsidizeGroupClaim"></param>
        ''' <param name="udtPracticeModel"></param>
        ''' <param name="udtStudent"></param>
        ''' <param name="udtAccount"></param>
        ''' <param name="cllnTranVaccine"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ConstructNewEHSTransaction(ByVal udtSchemeClaimModel As SchemeClaimModel, _
                                                   ByVal udtSubsidizeGroupClaim As Scheme.SubsidizeGroupClaimModel, _
                                                   ByVal udtClaimCategory As ClaimCategory.ClaimCategoryModel, _
                                                   ByVal udtPracticeModel As PracticeModel, _
                                                   ByVal udtStudent As StudentModel, _
                                                   ByVal dtmServiceDate As DateTime, _
                                                   ByVal strDose As String, _
                                                   ByVal udtAccount As EHSAccount.EHSAccountModel, _
                                                   ByVal cllnTranVaccine As TransactionDetailVaccineModelCollection, _
                                                   ByVal udtVaccineEntitle As BLL.VaccineEntitleModel) As EHSTransactionModel
            Dim udtEHSTran As New EHSTransactionModel()


            ' Account information
            udtEHSTran.DocCode = udtStudent.PersonalInformation.DocCode
            If udtAccount.AccountSource = SysAccountSource.ValidateAccount Then
                udtEHSTran.VoucherAccID = udtAccount.VoucherAccID
            Else
                udtEHSTran.TempVoucherAccID = udtAccount.VoucherAccID
            End If
            udtEHSTran.EHSAcct = udtAccount

            ' Service Provider & Practice
            udtEHSTran.ServiceType = udtPracticeModel.Professional.ServiceCategoryCode
            udtEHSTran.ServiceProviderID = udtPracticeModel.SPID
            udtEHSTran.DataEntryBy = String.Empty
            udtEHSTran.PracticeID = udtPracticeModel.DisplaySeq
            udtEHSTran.BankAccountID = udtPracticeModel.BankAcct.DisplaySeq
            udtEHSTran.BankAccountNo = udtPracticeModel.BankAcct.BankAcctNo
            udtEHSTran.BankAccountOwner = udtPracticeModel.BankAcct.BankAcctOwner

            ' Claim, Scheme & subsidies
            udtEHSTran.SchemeCode = udtSchemeClaimModel.SchemeCode
            udtEHSTran.CategoryCode = udtClaimCategory.CategoryCode
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
            udtEHSTran.ServiceDate = dtmServiceDate
            'udtEHSTran.ServiceDate = udtStudent.ServiceReceviceDate
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
            udtEHSTran.ClaimAmount = 0

            ' Vaccine Ref Status (e.g. Patient match, with vaccination record returned from different parties)
            udtEHSTran.HAVaccineRefStatus = udtStudent.HAVaccineRefStatus
            udtEHSTran.DHVaccineRefStatus = udtStudent.DHVaccineRefStatus

            ' Vaccine Result (e.g. Injected 23vPPV or PCV13 previously in different parties)
            Dim dicVaccineRef As Dictionary(Of String, String) = EHSTransactionModel.GetVaccineRef(cllnTranVaccine, udtEHSTran)
            udtEHSTran.EHSVaccineResult = dicVaccineRef(EHSTransactionModel.VaccineRefType.EHS)
            udtEHSTran.HAVaccineResult = dicVaccineRef(EHSTransactionModel.VaccineRefType.HA)
            udtEHSTran.DHVaccineResult = dicVaccineRef(EHSTransactionModel.VaccineRefType.DH)

            ' Mise information
            ' udtEHSTran.SourceApp <-- SourceApp will be specified when call insert transaction function
            udtEHSTran.TransactionDtm = Now()

            ' CRE19-001 (VSS 2019) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            'udtEHSTran.RecordStatus = udtSchemeClaimModel.ConfirmedTransactionStatus

            If udtAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                udtEHSTran.RecordStatus = udtSchemeClaimModel.ConfirmedTransactionStatus
            Else
                udtEHSTran.RecordStatus = EHSTransactionModel.TransRecordStatusClass.PendingVRValidate
            End If

            ' Transaction created by SP
            udtEHSTran.CreateBy = udtStudent.ServiceProviderID 'udtStudent.ClaimUploadBy
            udtEHSTran.UpdateBy = udtStudent.ServiceProviderID 'udtStudent.ClaimUploadBy
            ' CRE19-001 (VSS 2019) [End][Winnie]

            ConstructEHSTransactionDetails(udtSchemeClaimModel, udtSubsidizeGroupClaim, udtStudent, strDose, udtEHSTran, udtVaccineEntitle)

            Return udtEHSTran

        End Function


        ' Transaction Detail ------------------------------

        ''' <summary>
        ''' Construct the EHS Transaction Detail Model for Vaccination PPP
        ''' </summary>
        ''' <param name="udtSchemeClaimModel"></param>
        ''' <param name="udtSubsidizeGroupClaim"></param>
        ''' <param name="udtStudent"></param>
        ''' <param name="udtEHSTransactionModel"></param>
        ''' <remarks></remarks>
        Public Shared Sub ConstructEHSTransactionDetails(ByVal udtSchemeClaimModel As SchemeClaimModel, _
                                                  ByVal udtSubsidizeGroupClaim As Scheme.SubsidizeGroupClaimModel, _
                                                  ByVal udtStudent As StudentModel, _
                                                  ByVal strDose As String, _
                                                  ByRef udtEHSTransactionModel As EHSTransactionModel, _
                                                  ByVal udtVaccineEntitle As BLL.VaccineEntitleModel)

            ' Total claim amount
            Dim dblClaimAmount As Double = 0
            For Each udtSubsidizeFee As SubsidizeFeeModel In udtSubsidizeGroupClaim.SubsidizeFeeList.Filter(udtStudent.ServiceReceviceDate)
                dblClaimAmount += udtSubsidizeFee.SubsidizeFee
            Next

            ' Available Item Code
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
            Dim strAvailalbeItemCode As String = strDose
            'Dim strAvailalbeItemCode As String = udtStudent.Dose
            If strDose = SubsidizeItemDetailsModel.DoseCode.FirstDOSE Then
                ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
                If udtVaccineEntitle Is Nothing Then
                    If udtStudent.EntitleOnlyDose Then
                        strAvailalbeItemCode = SubsidizeItemDetailsModel.DoseCode.ONLYDOSE
                    End If
                Else
                    If udtVaccineEntitle.EntitleOnlyDose Then
                        strAvailalbeItemCode = SubsidizeItemDetailsModel.DoseCode.ONLYDOSE
                    End If
                End If

            End If

            ' Voucher Transaction 
            udtEHSTransactionModel.ClaimAmount = dblClaimAmount

            ' Transaction Details
            Dim udtEHSTransactionDetail As New EHSTransaction.TransactionDetailModel()
            udtEHSTransactionDetail.SchemeCode = udtSubsidizeGroupClaim.SchemeCode
            udtEHSTransactionDetail.SchemeSeq = udtSubsidizeGroupClaim.SchemeSeq
            udtEHSTransactionDetail.SubsidizeCode = udtSubsidizeGroupClaim.SubsidizeCode
            udtEHSTransactionDetail.SubsidizeItemCode = udtSubsidizeGroupClaim.SubsidizeItemCode
            udtEHSTransactionDetail.AvailableItemCode = strAvailalbeItemCode
            udtEHSTransactionDetail.Unit = 1
            udtEHSTransactionDetail.PerUnitValue = dblClaimAmount
            udtEHSTransactionDetail.TotalAmount = dblClaimAmount
            udtEHSTransactionDetail.Remark = String.Empty
            udtEHSTransactionModel.TransactionDetails = New EHSTransaction.TransactionDetailModelCollection
            udtEHSTransactionModel.TransactionDetails.Add(udtEHSTransactionDetail)

            ' Transaction Additional Field
            Dim udtTransactionAdditionalFieldModel As New EHSTransaction.TransactionAdditionalFieldModel()
            udtTransactionAdditionalFieldModel.SchemeCode = udtSubsidizeGroupClaim.SchemeCode
            udtTransactionAdditionalFieldModel.SchemeSeq = udtSubsidizeGroupClaim.SchemeSeq
            udtTransactionAdditionalFieldModel.SubsidizeCode = udtSubsidizeGroupClaim.SubsidizeCode
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
            If udtSubsidizeGroupClaim.SchemeCode = SchemeClaimModel.RVP Then
                udtTransactionAdditionalFieldModel.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.RCHCode
            Else
                udtTransactionAdditionalFieldModel.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.SchoolCode
            End If
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
            udtTransactionAdditionalFieldModel.AdditionalFieldValueCode = udtStudent.SchoolCode
            udtEHSTransactionModel.TransactionAdditionFields = New EHSTransaction.TransactionAdditionalFieldModelCollection
            udtEHSTransactionModel.TransactionAdditionFields.Add(udtTransactionAdditionalFieldModel)
        End Sub

    End Class
End Namespace
