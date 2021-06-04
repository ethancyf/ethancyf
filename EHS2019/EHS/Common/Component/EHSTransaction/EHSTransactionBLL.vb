Imports Common.DataAccess
Imports System.Data.SqlClient
Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.Scheme
Imports Common.Component.ClaimRules
Imports Common.ComObject
Imports Common.Component.EHSAccount
Imports Common.Component.UserAC
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.ProfessionVoucherQuota
Imports Common.Component.Professional
Imports Common.Component.VoucherInfo

Namespace Component.EHSTransaction

    Public Class EHSTransactionBLL

        Private strYES = "Y"
        Private strNO = "N"

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Private Class SESS
            Public Const TransactionDetailBenefitDocCode As String = "TransactionDetailBenefitDocCode"
            Public Const TransactionDetailBenefitDocNo As String = "TransactionDetailBenefitDocNo"
            Public Const TransactionDetailBenefit As String = "TransactionDetailBenefit"

            Public Const TransactionDetailVaccineDocCode As String = "TransactionDetailVaccineDocCode"
            Public Const TransactionDetailVaccineDocNo As String = "TransactionDetailVaccineDocNo"
            Public Const TransactionDetailVaccine As String = "TransactionDetailVaccine"

        End Class
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        Private _udtFormatter As New Format.Formatter()

        Public Function LoadClaimTran(ByVal strTranID As String, Optional ByVal blnLoadOriginalAccInsteadOfInvalidAcc As Boolean = False, Optional ByVal blnShowWarning As Boolean = False, Optional ByVal udtdb As Database = Nothing) As EHSTransactionModel
            Dim dt As DataTable
            Dim udtEHSTransaction As EHSTransactionModel
            'Dim udtdb As Database
            Dim userrow As DataRow

            udtEHSTransaction = New EHSTransactionModel()
            dt = New DataTable
            If udtdb Is Nothing Then udtdb = New Database()
            Dim prams() As SqlParameter = { _
            udtdb.MakeInParam("@tran_id", SqlDbType.Char, 20, strTranID)}

            udtdb.RunProc("proc_VoucherTransaction_get_byTranID", prams, dt)

            ' Handle Mirgration Complete Show Practice Chi
            Dim strHCSPDataMirgrationCompleteTurnOn As String = String.Empty

            Dim udtGeneralFunction As New GeneralFunction
            udtGeneralFunction.getSystemParameter("HCSPDataMirgrationCompleteTurnOn", strHCSPDataMirgrationCompleteTurnOn, Nothing)

            If dt.Rows.Count > 0 Then
                userrow = dt.Rows(0)
                udtEHSTransaction.TransactionID = Trim(userrow("tranNum").ToString())
                udtEHSTransaction.TransactionDtm = userrow("tranDate")
                udtEHSTransaction.ServiceProviderName = Trim(userrow("SPName"))

                If IsDBNull(userrow("SPNameChi")) OrElse CStr(userrow("SPNameChi")).Trim = String.Empty Then
                    udtEHSTransaction.ServiceProviderNameChi = udtEHSTransaction.ServiceProviderName
                Else
                    udtEHSTransaction.ServiceProviderNameChi = CStr(userrow("SPNameChi")).Trim
                End If

                udtEHSTransaction.PracticeID = userrow("practiceID")
                udtEHSTransaction.PracticeName = Trim(userrow("PracticeName"))

                If strHCSPDataMirgrationCompleteTurnOn.Trim = "Y" Then
                    If IsDBNull(userrow("PracticeNameChi")) OrElse CStr(userrow("PracticeNameChi")).Trim = String.Empty Then
                        udtEHSTransaction.PracticeNameChi = udtEHSTransaction.PracticeName
                    Else
                        udtEHSTransaction.PracticeNameChi = Trim(userrow("PracticeNameChi"))
                    End If

                Else
                    udtEHSTransaction.PracticeNameChi = udtEHSTransaction.PracticeName

                End If

                udtEHSTransaction.BankAccountNo = Trim(userrow("bankAccountNo"))
                udtEHSTransaction.BankAccountID = userrow("BankAccountID")
                udtEHSTransaction.RecordStatus = Trim(userrow("status"))
                udtEHSTransaction.ServiceDate = userrow("serviceDate")
                udtEHSTransaction.ServiceType = userrow("serviceType")
                udtEHSTransaction.VoucherBeforeRedeem = userrow("voucherBeforeClaim")
                udtEHSTransaction.VoucherAfterRedeem = userrow("voucherAfterClaim")
                udtEHSTransaction.ServiceProviderID = userrow("SPID")
                udtEHSTransaction.TSMP = userrow("TSMP")

                If IsDBNull(userrow("totalAmount")) = True Then
                    udtEHSTransaction.ClaimAmount = Nothing
                Else
                    udtEHSTransaction.ClaimAmount = CDbl(userrow("totalAmount"))
                End If

                ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [Start][Koala]
                udtEHSTransaction.SourceApp = userrow("SourceApp")
                ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [End][Koala]
                udtEHSTransaction.DocCode = Trim(userrow("Doc_Code"))

                If IsDBNull(userrow("Authorised_status")) Then
                    udtEHSTransaction.AuthorisedStatus = String.Empty
                Else
                    udtEHSTransaction.AuthorisedStatus = userrow("Authorised_status")
                End If

                If Not IsDBNull(userrow("firstAuthorizedBy")) Then
                    udtEHSTransaction.FirstAuthorisedBy = userrow("firstAuthorizedBy").ToString()
                Else
                    udtEHSTransaction.FirstAuthorisedBy = String.Empty
                End If

                If Not IsDBNull(userrow("firstAuthorizedDate")) Then
                    udtEHSTransaction.FirstAuthorisedDate = userrow("firstAuthorizedDate")  'DateTime.ParseExact(userrow("firstAuthorizedDate"), "M/dd/yyyy HH:mm:ss", Nothing).ToString("dd MMM yyyy HH:mm")
                Else
                    udtEHSTransaction.FirstAuthorisedDate = Nothing
                End If

                If Not IsDBNull(userrow("secondAuthorizedBy")) Then
                    udtEHSTransaction.SecondAuthorisedBy = userrow("secondAuthorizedBy").ToString()
                Else
                    udtEHSTransaction.SecondAuthorisedBy = String.Empty
                End If

                If Not IsDBNull(userrow("secondAuthorizedDate")) Then
                    udtEHSTransaction.SecondAuthorisedDate = userrow("secondAuthorizedDate") 'DateTime.ParseExact(userrow("secondAuthorizedDate"), "M/dd/yyyy HH:mm:ss", Nothing).ToString("dd MMM yyyy HH:mm")
                Else
                    udtEHSTransaction.SecondAuthorisedDate = Nothing
                End If

                If Not IsDBNull(userrow("ReimbursedBy")) Then
                    udtEHSTransaction.ReimbursedBy = userrow("ReimbursedBy").ToString()
                Else
                    udtEHSTransaction.ReimbursedBy = String.Empty
                End If

                If Not IsDBNull(userrow("ReimbursedDate")) Then
                    udtEHSTransaction.ReimbursedDate = userrow("ReimbursedDate") 'DateTime.ParseExact(userrow("secondAuthorizedDate"), "M/dd/yyyy HH:mm:ss", Nothing).ToString("dd MMM yyyy HH:mm")
                Else
                    udtEHSTransaction.ReimbursedDate = Nothing
                End If

                If Not IsDBNull(userrow("Void_Transaction_ID")) Then
                    udtEHSTransaction.VoidTranNo = userrow("Void_Transaction_ID")
                Else
                    udtEHSTransaction.VoidTranNo = String.Empty
                End If

                If Not IsDBNull(userrow("Void_Dtm")) Then
                    udtEHSTransaction.VoidDate = userrow("Void_Dtm")
                Else
                    udtEHSTransaction.VoidDate = Nothing
                End If

                If Not IsDBNull(userrow("Void_Remark")) Then
                    udtEHSTransaction.VoidReason = userrow("Void_Remark")
                Else
                    udtEHSTransaction.VoidReason = String.Empty
                End If

                If Not IsDBNull(userrow("Void_By")) Then
                    udtEHSTransaction.VoidUser = userrow("Void_By")
                Else
                    udtEHSTransaction.VoidUser = String.Empty
                End If

                If Not IsDBNull(userrow("Void_By_DataEntry")) Then
                    udtEHSTransaction.VoidByDataEntry = userrow("Void_By_DataEntry")
                Else
                    udtEHSTransaction.VoidByDataEntry = String.Empty
                End If

                If Not IsDBNull(userrow("confirmed_dtm")) Then
                    udtEHSTransaction.ConfirmDate = userrow("confirmed_dtm") 'DateTime.ParseExact(userrow("secondAuthorizedDate"), "M/dd/yyyy HH:mm:ss", Nothing).ToString("dd MMM yyyy HH:mm")
                Else
                    udtEHSTransaction.ConfirmDate = Nothing
                End If

                udtEHSTransaction.Invalidation = userrow("Invalidation").ToString().Trim

                Dim vr As New EHSAccount.EHSAccountBLL

                'Handle different combinations of eHSAccount
                '1. Temp + Special + Validated ==> Validated
                '2. Temp + Special + Invalid ==> Invalid (Replaced by ***7)
                '3. Temp + Validated ==> Validated
                '4. Temp + Special ==> Special
                '5. Temp Only ==> Temp
                '6. Validated Only ==> Validated
                '***7. Temp + Special + Invalid + Load Original ==> Special
                '8. Temp + Invalid + Load Original ==> Temp 

                If Not IsDBNull(userrow("Voucher_Acc_ID")) And Not userrow("Voucher_Acc_ID").ToString.Trim.Equals(String.Empty) Then
                    'Case 1, 3, 6 ==> Validated
                    udtEHSTransaction.EHSAcct = vr.LoadEHSAccountByVRID(Trim(userrow("Voucher_Acc_ID")), udtdb)
                ElseIf Not userrow("Temp_Voucher_Acc_ID").ToString.Trim.Trim.Equals(String.Empty) AndAlso _
                        userrow("Voucher_Acc_ID").ToString.Trim.Trim.Equals(String.Empty) AndAlso _
                        userrow("Special_Acc_ID").ToString.Trim.Trim.Equals(String.Empty) AndAlso _
                        userrow("Invalid_Acc_ID").ToString.Trim.Trim.Equals(String.Empty) Then
                    'Case 5 ==> Temp
                    udtEHSTransaction.EHSAcct = vr.LoadTempEHSAccountByVRID(Trim(userrow("Temp_Voucher_Acc_ID")), udtdb)
                ElseIf Not userrow("Special_Acc_ID").ToString.Trim.Trim.Equals(String.Empty) AndAlso _
                        userrow("Voucher_Acc_ID").ToString.Trim.Trim.Equals(String.Empty) AndAlso _
                        userrow("Invalid_Acc_ID").ToString.Trim.Trim.Equals(String.Empty) Then
                    'Case 4 ==> Special
                    udtEHSTransaction.EHSAcct = vr.LoadSpecialEHSAccountByVRID(Trim(userrow("Special_Acc_ID")), udtdb)
                ElseIf Not userrow("Invalid_Acc_ID").ToString.Trim.Trim.Equals(String.Empty) Then
                    'Case 2 ==> Invalid   Or  Case 7 ==> Special   Or  Case 8 ==> Temp
                    If blnLoadOriginalAccInsteadOfInvalidAcc Then

                        ' INT20-0014 (Fix unable to open invalidated PPP transaction) [Start][Winnie]
                        ' ---------------------------------------------------------------------------
                        If Not userrow("Special_Acc_ID").ToString.Trim.Trim.Equals(String.Empty) Then
                            udtEHSTransaction.EHSAcct = vr.LoadSpecialEHSAccountByVRID(Trim(userrow("Special_Acc_ID")), udtdb)
                        Else
                            udtEHSTransaction.EHSAcct = vr.LoadTempEHSAccountByVRID(Trim(userrow("Temp_Voucher_Acc_ID")), udtdb)
                        End If
                        ' INT20-0014 (Fix unable to open invalidated PPP transaction) [End][Winnie]                        
                    Else
                        udtEHSTransaction.EHSAcct = vr.LoadInvalidEHSAccountByVRID(Trim(userrow("Invalid_Acc_ID")), udtdb)
                    End If
                Else
                    udtEHSTransaction.EHSAcct = Nothing
                End If

                If Not IsDBNull(userrow("DataEntry_by")) Then
                    udtEHSTransaction.DataEntryBy = userrow("DataEntry_by")
                Else
                    udtEHSTransaction.DataEntryBy = String.Empty
                End If

                If Trim(userrow("Void_By_HCVU")) = "Y" Then
                    udtEHSTransaction.VoidByHCVU = True
                Else
                    udtEHSTransaction.VoidByHCVU = False
                End If

                udtEHSTransaction.SchemeCode = Trim(userrow("Scheme_Code").ToString())

                udtEHSTransaction.TransactionDetails = Me.getTransactionDetail(udtEHSTransaction.TransactionID, udtdb)

                udtEHSTransaction.TransactionAdditionFields = Me.getTransactionAdditionalField(udtEHSTransaction.TransactionID, udtdb)

                udtEHSTransaction.TransactionInvalidation = Me.getTransactionInvalidation(udtEHSTransaction.TransactionID, udtdb)

                'For Voucher case only 1 TransactionDetail record only     
                ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                If udtEHSTransaction.TransactionDetails(0).Unit.HasValue Then
                    udtEHSTransaction.VoucherClaim = udtEHSTransaction.TransactionDetails(0).Unit
                Else
                    udtEHSTransaction.VoucherClaim = Nothing
                End If

                If udtEHSTransaction.TransactionDetails(0).PerUnitValue.HasValue Then
                    udtEHSTransaction.PerVoucherValue = udtEHSTransaction.TransactionDetails(0).PerUnitValue
                Else
                    udtEHSTransaction.PerVoucherValue = Nothing
                End If
                ' CRE20-015 (Special Support Scheme) [End][Chris YIM]


                If Not IsDBNull(userrow("Create_By")) Then
                    udtEHSTransaction.CreateBy = userrow("Create_by")
                Else
                    udtEHSTransaction.CreateBy = String.Empty
                End If

                If Not IsDBNull(userrow("Create_Dtm")) Then
                    udtEHSTransaction.CreateDate = userrow("Create_Dtm")
                Else
                    udtEHSTransaction.CreateDate = Nothing
                End If

                Dim blnManualReimburse As Boolean = False
                If Not IsDBNull(userrow("Manual_Reimburse")) Then
                    If CStr(userrow("Manual_Reimburse")).Trim.Equals(strYES) Then
                        blnManualReimburse = True
                    End If
                End If

                If Not IsDBNull(userrow("IsUpload")) Then
                    udtEHSTransaction.IsUpload = userrow("IsUpload")
                Else
                    udtEHSTransaction.IsUpload = "N"
                End If

                udtEHSTransaction.ManualReimburse = blnManualReimburse

                If Not IsDBNull(userrow("Create_By_SmartID")) AndAlso CStr(userrow("Create_By_SmartID")).Trim = strYES Then
                    udtEHSTransaction.CreateBySmartID = True
                Else
                    udtEHSTransaction.CreateBySmartID = False
                End If

                'Additional field by non-reimbursable claim
                Dim strCreationReason As String = String.Empty
                Dim strCreationRemarks As String = String.Empty
                Dim strPaymentMethod As String = String.Empty
                Dim strPaymentRemarks As String = String.Empty
                Dim strOverrideReason As String = String.Empty
                Dim strApprovalBy As String = String.Empty
                Dim dtmApprovalDate As Nullable(Of DateTime) = Nothing
                Dim strRejectBy As String = String.Empty
                Dim dtmRejectDate As Nullable(Of DateTime) = Nothing
                Dim byteManualReimburseTSMP As Byte() = Nothing

                If Not userrow.IsNull("Creation_Reason") Then strCreationReason = userrow("Creation_Reason").ToString().Trim()
                If Not userrow.IsNull("Creation_Remark") Then strCreationRemarks = userrow("Creation_Remark").ToString().Trim()
                If Not userrow.IsNull("Payment_Method") Then strPaymentMethod = userrow("Payment_Method").ToString().Trim()
                If Not userrow.IsNull("Payment_Remark") Then strPaymentRemarks = userrow("Payment_Remark").ToString().Trim()
                If Not userrow.IsNull("Override_Reason") Then strOverrideReason = userrow("Override_Reason").ToString().Trim()
                If Not userrow.IsNull("Approval_By") Then strApprovalBy = userrow("Approval_By").ToString().Trim()
                If Not userrow.IsNull("Approval_Dtm") Then dtmApprovalDate = Convert.ToDateTime(userrow("Approval_Dtm"))
                If Not userrow.IsNull("Reject_By") Then strRejectBy = userrow("Reject_By").ToString().Trim()
                If Not userrow.IsNull("Reject_Dtm") Then dtmRejectDate = Convert.ToDateTime(userrow("Reject_Dtm"))
                If Not userrow.IsNull("Manual_Reimburse_TSMP") Then byteManualReimburseTSMP = CType(userrow("Manual_Reimburse_TSMP"), Byte())

                udtEHSTransaction.CreationReason = strCreationReason
                udtEHSTransaction.CreationRemarks = strCreationRemarks
                udtEHSTransaction.PaymentMethod = strPaymentMethod
                udtEHSTransaction.PaymentRemarks = strPaymentRemarks
                udtEHSTransaction.OverrideReason = strOverrideReason
                udtEHSTransaction.ApprovalBy = strApprovalBy
                udtEHSTransaction.ApprovalDate = dtmApprovalDate
                udtEHSTransaction.RejectBy = strRejectBy
                udtEHSTransaction.RejectDate = dtmRejectDate
                udtEHSTransaction.ManualReimburseTSMP = byteManualReimburseTSMP

                Dim strCategoryCode As String = String.Empty
                If Not userrow.IsNull("Category_Code") Then strCategoryCode = userrow("Category_Code").ToString().Trim()
                udtEHSTransaction.CategoryCode = strCategoryCode

                Dim strHighRisk As String = String.Empty
                If Not userrow.IsNull("High_Risk") Then strHighRisk = userrow("High_Risk").ToString().Trim()
                udtEHSTransaction.HighRisk = strHighRisk

                Dim strEHSVaccineResult As String = String.Empty
                If Not userrow.IsNull("EHS_Vaccine_Ref") Then strEHSVaccineResult = userrow("EHS_Vaccine_Ref").ToString().Trim()
                udtEHSTransaction.EHSVaccineResult = strEHSVaccineResult

                Dim strHAVaccineResult As String = String.Empty
                If Not userrow.IsNull("HA_Vaccine_Ref") Then strHAVaccineResult = userrow("HA_Vaccine_Ref").ToString().Trim()
                udtEHSTransaction.HAVaccineResult = strHAVaccineResult
                'CRE16-026 (Add PCV13) [End][Chris YIM]

                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
                ' ----------------------------------------------------------
                Dim strHAVaccineRefStatus As String = String.Empty
                If Not userrow.IsNull("Ext_Ref_Status") Then strHAVaccineRefStatus = userrow("Ext_Ref_Status").ToString().Trim()
                udtEHSTransaction.HAVaccineRefStatus = strHAVaccineRefStatus

                Dim strDHVaccineResult As String = String.Empty
                If Not userrow.IsNull("DH_Vaccine_Ref") Then strDHVaccineResult = userrow("DH_Vaccine_Ref").ToString().Trim()
                udtEHSTransaction.DHVaccineResult = strDHVaccineResult

                Dim strDHVaccineRefStatus As String = String.Empty
                If Not userrow.IsNull("DH_Vaccine_Ref_Status") Then strDHVaccineRefStatus = userrow("DH_Vaccine_Ref_Status").ToString().Trim()
                udtEHSTransaction.DHVaccineRefStatus = strDHVaccineRefStatus
                ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

                ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                ' ----------------------------------------------------------
                Dim strHKICSymbol As String = String.Empty
                If Not userrow.IsNull("HKIC_Symbol") Then strHKICSymbol = userrow("HKIC_Symbol").ToString().Trim()
                udtEHSTransaction.HKICSymbol = strHKICSymbol

                Dim strOCSSSRefStatus As String = String.Empty
                If Not userrow.IsNull("OCSSS_Ref_Status") Then strOCSSSRefStatus = userrow("OCSSS_Ref_Status").ToString().Trim()
                udtEHSTransaction.OCSSSRefStatus = strOCSSSRefStatus

                If Not udtEHSTransaction.EHSAcct.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKIC) Is Nothing Then
                    udtEHSTransaction.EHSAcct.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKIC).HKICSymbol = strHKICSymbol
                End If
                ' CRE17-010 (OCSSS integration) [End][Chris YIM]

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Dim strSmartIDVer As String = String.Empty
                If Not userrow.IsNull("SmartID_Ver") Then strSmartIDVer = userrow("SmartID_Ver").ToString().Trim()
                udtEHSTransaction.SmartIDVer = strSmartIDVer
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                ' CRE19-006 (DHC) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Dim strDHCService As String = String.Empty
                If Not userrow.IsNull("DHC_Service") Then strDHCService = userrow("DHC_Service").ToString().Trim()
                udtEHSTransaction.DHCService = strDHCService
                ' CRE19-006 (DHC) [End][Winnie]

                ' 2010-08-09
                If blnShowWarning AndAlso udtEHSTransaction.ManualReimburse Then
                    udtEHSTransaction.WarningMessage = getTransactionWarningResults(udtEHSTransaction, udtdb)
                End If

            Else
                udtEHSTransaction = Nothing
            End If

            Return udtEHSTransaction
        End Function

        Public Function LoadClaimTranByPartialTranNo(ByVal strPartialTranID As String) As EHSTransactionModel
            Dim dt As DataTable
            Dim udtEHSTransaction As EHSTransactionModel
            Dim udtdb As Database
            Dim userrow As DataRow

            udtEHSTransaction = New EHSTransactionModel()
            dt = New DataTable
            udtdb = New Database()
            Dim prams() As SqlParameter = { _
            udtdb.MakeInParam("@Partial_Trans_No", SqlDbType.Char, 20, strPartialTranID)}

            udtdb.RunProc("proc_VoucherTransaction_get_byPartialTranID", prams, dt)

            ' Handle Mirgration Complete Show Practice Chi
            Dim strHCSPDataMirgrationCompleteTurnOn As String = String.Empty

            Dim udtGeneralFunction As New GeneralFunction
            udtGeneralFunction.getSystemParameter("HCSPDataMirgrationCompleteTurnOn", strHCSPDataMirgrationCompleteTurnOn, Nothing)

            If dt.Rows.Count > 0 Then
                userrow = dt.Rows(0)
                udtEHSTransaction.TransactionID = Trim(userrow("tranNum").ToString())
                udtEHSTransaction.TransactionDtm = userrow("tranDate")
                udtEHSTransaction.ServiceProviderName = Trim(userrow("SPName"))

                If IsDBNull(userrow("SPNameChi")) OrElse CStr(userrow("SPNameChi")).Trim = String.Empty Then
                    udtEHSTransaction.ServiceProviderNameChi = udtEHSTransaction.ServiceProviderName
                Else
                    udtEHSTransaction.ServiceProviderNameChi = CStr(userrow("SPNameChi")).Trim
                End If

                udtEHSTransaction.PracticeID = userrow("practiceID")
                udtEHSTransaction.PracticeName = Trim(userrow("PracticeName"))

                If strHCSPDataMirgrationCompleteTurnOn.Trim = "Y" Then
                    If IsDBNull(userrow("PracticeNameChi")) OrElse CStr(userrow("PracticeNameChi")).Trim = String.Empty Then
                        udtEHSTransaction.PracticeNameChi = udtEHSTransaction.PracticeName
                    Else
                        udtEHSTransaction.PracticeNameChi = Trim(userrow("PracticeNameChi"))
                    End If

                Else
                    udtEHSTransaction.PracticeNameChi = udtEHSTransaction.PracticeName

                End If

                'If udtEHSTransaction.PracticeNameChi.Trim().Trim() = "" Then
                '    udtEHSTransaction.PracticeNameChi = udtEHSTransaction.PracticeName
                'End If

                '' Handle Mirgration Complete Show Practice Chi
                'If strHCSPDataMirgrationCompleteTurnOn.Trim() = "Y" Then
                'Else
                '    udtEHSTransaction.PracticeNameChi = udtEHSTransaction.PracticeName
                'End If

                udtEHSTransaction.BankAccountNo = Trim(userrow("bankAccountNo"))
                udtEHSTransaction.BankAccountID = userrow("BankAccountID")
                'udtEHSTransaction.VoucherRedeem = userrow("voucherRedeem")
                'udtEHSTransaction.PerVoucherAmount = userrow("voucherAmount")
                udtEHSTransaction.RecordStatus = Trim(userrow("status"))
                udtEHSTransaction.ServiceDate = userrow("serviceDate")
                udtEHSTransaction.ServiceType = userrow("serviceType")
                udtEHSTransaction.VoucherBeforeRedeem = userrow("voucherBeforeClaim")
                udtEHSTransaction.VoucherAfterRedeem = userrow("voucherAfterClaim")
                'udtEHSTransaction.VisitReason1 = userrow("visitReason_L1")
                udtEHSTransaction.ServiceProviderID = userrow("SPID")
                udtEHSTransaction.TSMP = userrow("TSMP")
                ' CRE13-001 - EHAPP [Start][Koala]
                ' -------------------------------------------------------------------------------------
                udtEHSTransaction.ClaimAmount = CDbl(userrow("totalAmount"))
                'udtEHSTransaction.ClaimAmount = userrow("totalAmount")
                ' CRE13-001 - EHAPP [End][Koala]

                udtEHSTransaction.DocCode = Trim(userrow("Doc_Code"))

                If IsDBNull(userrow("Authorised_status")) Then
                    udtEHSTransaction.AuthorisedStatus = String.Empty
                Else
                    udtEHSTransaction.AuthorisedStatus = userrow("Authorised_status")
                End If

                If Not IsDBNull(userrow("firstAuthorizedBy")) Then
                    udtEHSTransaction.FirstAuthorisedBy = userrow("firstAuthorizedBy").ToString()
                Else
                    udtEHSTransaction.FirstAuthorisedBy = String.Empty
                End If

                If Not IsDBNull(userrow("firstAuthorizedDate")) Then
                    udtEHSTransaction.FirstAuthorisedDate = userrow("firstAuthorizedDate")  'DateTime.ParseExact(userrow("firstAuthorizedDate"), "M/dd/yyyy HH:mm:ss", Nothing).ToString("dd MMM yyyy HH:mm")
                Else
                    udtEHSTransaction.FirstAuthorisedDate = Nothing
                End If

                If Not IsDBNull(userrow("secondAuthorizedBy")) Then
                    udtEHSTransaction.SecondAuthorisedBy = userrow("secondAuthorizedBy").ToString()
                Else
                    udtEHSTransaction.SecondAuthorisedBy = String.Empty
                End If

                If Not IsDBNull(userrow("secondAuthorizedDate")) Then
                    udtEHSTransaction.SecondAuthorisedDate = userrow("secondAuthorizedDate") 'DateTime.ParseExact(userrow("secondAuthorizedDate"), "M/dd/yyyy HH:mm:ss", Nothing).ToString("dd MMM yyyy HH:mm")
                Else
                    udtEHSTransaction.SecondAuthorisedDate = Nothing
                End If

                If Not IsDBNull(userrow("ReimbursedBy")) Then
                    udtEHSTransaction.ReimbursedBy = userrow("ReimbursedBy").ToString()
                Else
                    udtEHSTransaction.ReimbursedBy = String.Empty
                End If

                If Not IsDBNull(userrow("ReimbursedDate")) Then
                    udtEHSTransaction.ReimbursedDate = userrow("ReimbursedDate") 'DateTime.ParseExact(userrow("secondAuthorizedDate"), "M/dd/yyyy HH:mm:ss", Nothing).ToString("dd MMM yyyy HH:mm")
                Else
                    udtEHSTransaction.ReimbursedDate = Nothing
                End If

                If Not IsDBNull(userrow("Void_Transaction_ID")) Then
                    udtEHSTransaction.VoidTranNo = userrow("Void_Transaction_ID")
                Else
                    udtEHSTransaction.VoidTranNo = String.Empty
                End If

                If Not IsDBNull(userrow("Void_Dtm")) Then
                    udtEHSTransaction.VoidDate = userrow("Void_Dtm")
                Else
                    udtEHSTransaction.VoidDate = Nothing
                End If

                If Not IsDBNull(userrow("Void_Remark")) Then
                    udtEHSTransaction.VoidReason = userrow("Void_Remark")
                Else
                    udtEHSTransaction.VoidReason = String.Empty
                End If

                If Not IsDBNull(userrow("Void_By")) Then
                    udtEHSTransaction.VoidUser = userrow("Void_By")
                Else
                    udtEHSTransaction.VoidUser = String.Empty
                End If

                If Not IsDBNull(userrow("Void_By_DataEntry")) Then
                    udtEHSTransaction.VoidByDataEntry = userrow("Void_By_DataEntry")
                Else
                    udtEHSTransaction.VoidByDataEntry = String.Empty
                End If

                If Not IsDBNull(userrow("confirmed_dtm")) Then
                    udtEHSTransaction.ConfirmDate = userrow("confirmed_dtm") 'DateTime.ParseExact(userrow("secondAuthorizedDate"), "M/dd/yyyy HH:mm:ss", Nothing).ToString("dd MMM yyyy HH:mm")
                Else
                    udtEHSTransaction.ConfirmDate = Nothing
                End If

                Dim vr As New EHSAccount.EHSAccountBLL

                'Handle different combinations of eHSAccount
                '1. Temp + Special + Validated ==> Validated
                '2. Temp + Special + Invalid ==> Invalid
                '3. Temp + Validated ==> Validated
                '4. Temp + Special ==> Special
                '5. Temp Only ==> Temp
                '6. Validated Only ==> Validated

                If Not IsDBNull(userrow("Voucher_Acc_ID")) And Not userrow("Voucher_Acc_ID").ToString.Trim.Equals(String.Empty) Then
                    'Case 1, 3, 6 ==> Validated
                    udtEHSTransaction.EHSAcct = vr.LoadEHSAccountByVRID(Trim(userrow("Voucher_Acc_ID")))
                ElseIf Not userrow("Temp_Voucher_Acc_ID").ToString.Trim.Trim.Equals(String.Empty) AndAlso _
                        userrow("Voucher_Acc_ID").ToString.Trim.Trim.Equals(String.Empty) AndAlso _
                        userrow("Special_Acc_ID").ToString.Trim.Trim.Equals(String.Empty) AndAlso _
                        userrow("Invalid_Acc_ID").ToString.Trim.Trim.Equals(String.Empty) Then
                    'Case 5 ==> Temp
                    udtEHSTransaction.EHSAcct = vr.LoadTempEHSAccountByVRID(Trim(userrow("Temp_Voucher_Acc_ID")))
                ElseIf Not userrow("Special_Acc_ID").ToString.Trim.Trim.Equals(String.Empty) AndAlso _
                        userrow("Voucher_Acc_ID").ToString.Trim.Trim.Equals(String.Empty) AndAlso _
                        userrow("Invalid_Acc_ID").ToString.Trim.Trim.Equals(String.Empty) Then
                    'Case 4 ==> Special
                    udtEHSTransaction.EHSAcct = vr.LoadSpecialEHSAccountByVRID(Trim(userrow("Special_Acc_ID")))
                ElseIf Not userrow("Invalid_Acc_ID").ToString.Trim.Trim.Equals(String.Empty) Then
                    'Case 2 ==> Invalid
                    udtEHSTransaction.EHSAcct = vr.LoadInvalidEHSAccountByVRID(Trim(userrow("Invalid_Acc_ID")))
                Else
                    udtEHSTransaction.EHSAcct = Nothing
                End If

                'If Not IsDBNull(userrow("visitReason_L2")) Then
                '    udtEHSTransaction.VisitReason2 = userrow("visitReason_L2")
                'Else
                '    udtEHSTransaction.VisitReason2 = String.Empty
                'End If

                If Not IsDBNull(userrow("DataEntry_by")) Then
                    udtEHSTransaction.DataEntryBy = userrow("DataEntry_by")
                Else
                    udtEHSTransaction.DataEntryBy = String.Empty
                End If

                If Trim(userrow("Void_By_HCVU")) = "Y" Then
                    udtEHSTransaction.VoidByHCVU = True
                Else
                    udtEHSTransaction.VoidByHCVU = False
                End If

                udtEHSTransaction.SchemeCode = Trim(userrow("Scheme_Code").ToString())

                udtEHSTransaction.TransactionDetails = Me.getTransactionDetail(udtEHSTransaction.TransactionID)

                udtEHSTransaction.TransactionAdditionFields = Me.getTransactionAdditionalField(udtEHSTransaction.TransactionID)

                'For Voucher case only 1 TransactionDetail record only                
                udtEHSTransaction.VoucherClaim = udtEHSTransaction.TransactionDetails(0).Unit
                udtEHSTransaction.PerVoucherValue = udtEHSTransaction.TransactionDetails(0).PerUnitValue

            Else
                udtEHSTransaction = Nothing
            End If

            Return udtEHSTransaction
        End Function

#Region "[Public] Retrieve /Search Function"

        Public Function LoadEHSTransaction(ByVal strTranID As String, Optional ByVal bWithEHSAccount As Boolean = False) As EHSTransactionModel

            Dim udtDB As New Database()

            Dim udtEHSTransactionModel As EHSTransactionModel = Me.getVoucherTransaction(strTranID, udtDB)
            udtEHSTransactionModel.TransactionDetails = Me.getTransactionDetail(strTranID, udtDB)
            udtEHSTransactionModel.TransactionAdditionFields = Me.getTransactionAdditionalField(strTranID, udtDB)
            udtEHSTransactionModel.TransactionInvalidation = Me.getTransactionInvalidation(strTranID, udtDB)

            If bWithEHSAccount Then
                Dim vr As New EHSAccount.EHSAccountBLL

                If Not udtEHSTransactionModel.VoucherAccID.Trim.Equals(String.Empty) Then
                    udtEHSTransactionModel.EHSAcct = vr.LoadEHSAccountByVRID(udtEHSTransactionModel.VoucherAccID.Trim)
                Else
                    udtEHSTransactionModel.EHSAcct = vr.LoadTempEHSAccountByVRID(udtEHSTransactionModel.TempVoucherAccID.Trim)
                End If
            End If

            udtEHSTransactionModel.WarningMessage = getTransactionWarningResults(udtEHSTransactionModel)

            Return udtEHSTransactionModel

        End Function

        ' CRE13-001 - EHAPP [Start][Koala]
        ' -------------------------------------------------------------------------------------
        ''' <summary>
        ''' For HCSP Text Only Version Claim Transaction Management to search transaction
        ''' </summary>
        ''' <param name="strDocCode"></param>
        ''' <param name="strIdentityNum"></param>
        ''' <param name="strAdoptionPrefix"></param>
        ''' <param name="dtmDOB"></param>
        ''' <param name="strExactDOB"></param>
        ''' <param name="strSPID"></param>
        ''' <param name="strDataEntry"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SearchEHSTransaction(ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal strAdoptionPrefix As String, _
           ByVal dtmDOB As Date, ByVal strExactDOB As String, ByVal strSPID As String, ByVal strDataEntry As String, ByVal enumSubPlatform As [Enum]) As EHSTransactionModelCollection
            Return Me.searchTransaction(strDocCode, strIdentityNum, strAdoptionPrefix, dtmDOB, strExactDOB, strSPID, strDataEntry, enumSubPlatform, New Database())
        End Function

        '''' <summary>
        '''' (Obsolete function since HCSP text only version claim transaction management is able to search all transaction rather than just voidable only)
        '''' </summary>
        '''' <param name="strDocCode"></param>
        '''' <param name="strIdentityNum"></param>
        '''' <param name="strAdoptionPrefix"></param>
        '''' <param name="dtmDOB"></param>
        '''' <param name="strExactDOB"></param>
        '''' <param name="strSPID"></param>
        '''' <param name="strDataEntry"></param>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Public Function SearchVoidableEHSTransaction(ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal strAdoptionPrefix As String, _
        '    ByVal dtmDOB As Date, ByVal strExactDOB As String, ByVal strSPID As String, ByVal strDataEntry As String) As EHSTransactionModelCollection
        '    Return Me.searchVoidableTransaction(strDocCode, strIdentityNum, strAdoptionPrefix, dtmDOB, strExactDOB, strSPID, strDataEntry, New Database())
        'End Function


        ''' <summary>
        ''' For HCSP Text Only Version Claim Transaction Management to search transaction
        ''' </summary>
        ''' <param name="strDocCode"></param>
        ''' <param name="strIdentityNum"></param>
        ''' <param name="intECAge"></param>
        ''' <param name="dtmDOR"></param>
        ''' <param name="strSPID"></param>
        ''' <param name="strDataEntry"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SearchEHSTransaction(ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal intECAge As Integer, _
            ByVal dtmDOR As Date, ByVal strSPID As String, ByVal strDataEntry As String, ByVal enumSubPlatform As [Enum]) As EHSTransactionModelCollection

            Return Me.searchTransaction(strDocCode, strIdentityNum, intECAge, dtmDOR, strSPID, strDataEntry, enumSubPlatform, New Database())

        End Function

        '''' <summary>
        '''' (Obsolete function since HCSP text only version claim transaction management is able to search all transaction rather than just voidable only)
        '''' </summary>
        '''' <param name="strDocCode"></param>
        '''' <param name="strIdentityNum"></param>
        '''' <param name="intECAge"></param>
        '''' <param name="dtmDOR"></param>
        '''' <param name="strSPID"></param>
        '''' <param name="strDataEntry"></param>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Public Function SearchVoidableEHSTransaction(ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal intECAge As Integer, _
        '    ByVal dtmDOR As Date, ByVal strSPID As String, ByVal strDataEntry As String) As EHSTransactionModelCollection

        '    Return Me.searchVoidableTransaction(strDocCode, strIdentityNum, intECAge, dtmDOR, strSPID, strDataEntry, New Database())

        'End Function

        ' CRE13-001 - EHAPP [End][Koala]

        ' CRE11-013 - RVP Home List Maintenance [Start][Twinsen]
        ' If RCH Code is being used in TransactionAdditionalField, return true, otherwise return false
        Public Function CheckTransactionAdditionalFieldByRCHCode(ByVal strRCHCode As String, Optional ByVal udtDB As Database = Nothing) As Boolean

            If udtDB Is Nothing Then udtDB = New Database()
            Dim dt As New DataTable()

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@RCHCode", SqlDbType.Char, 20, strRCHCode)}
                udtDB.RunProc("proc_TransactionAdditionalField_check_byRCHCode", prams, dt)

                Return CBool(dt.Rows(0)(0))

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

        End Function
        ' CRE11-013 - RVP Home List Maintenance [End][Twinsen]

        'CRE20-023 Immue Record COVID19 [Start][Martin]
        Public Function CheckTransactionAdditionalFieldByAny(ByVal strAdditionalFieldID As String, ByVal strAdditionalFieldValueCode As String, Optional ByVal udtDB As Database = Nothing) As Boolean

            If udtDB Is Nothing Then udtDB = New Database()
            Dim dt As New DataTable()

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@AdditionalFieldID", SqlDbType.VarChar, 20, IIf(strAdditionalFieldID.Trim.Equals(String.Empty), DBNull.Value, strAdditionalFieldID.Trim)), _
                                               udtDB.MakeInParam("@AdditionalFieldValueCode", SqlDbType.VarChar, 50, IIf(strAdditionalFieldValueCode.Trim.Equals(String.Empty), DBNull.Value, strAdditionalFieldValueCode.Trim))}
                udtDB.RunProc("proc_TransactionAdditionalField_check_byAny", prams, dt)

                Return CBool(dt.Rows(0)(0))

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

        End Function
        'CRE20-023 Immue Record COVID19 [End][Martin]

        '---------------------------------------------------------------------------------------------------------------
        ' For IVRS
        '---------------------------------------------------------------------------------------------------------------
        ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Lawrence]
        Public Function SearchVoidableEHSTransaction(ByVal strDocCode As String, ByVal strIdentityNum As String, _
            ByVal dtmDOB As Date, ByVal strExactDOB As String, ByVal strSPID As String) As EHSTransactionModelCollection
            Return Me.searchVoidableTransaction(strDocCode, strIdentityNum, dtmDOB, strExactDOB, strSPID, New Database())
        End Function

        Public Function SearchVoidableEHSTransaction(ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal intECAge As Integer, _
            ByVal dtmDOR As Date, ByVal strSPID As String) As EHSTransactionModelCollection
            Return Me.searchVoidableTransaction(strDocCode, strIdentityNum, intECAge, dtmDOR, strSPID)
        End Function
        ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Lawrence]

        Public Function GetPendingApprovalTransactionRowCount(ByVal strUserID As String) As Integer
            Return getTransactionManualReimbursementRowCount(strUserID, "P")
        End Function

#End Region

#Region "[Public] Insert EHS Transaction"

        ''' <summary>
        ''' Insert EHS Transaction (VoucherTransaction, TransactionDetail, TransactionAdditionalField)
        ''' </summary>
        ''' <param name="udtEHSTransactionModel"></param>
        ''' <param name="enumAppSource"></param>
        ''' <remarks></remarks>
        Public Function InsertEHSTransaction(ByVal udtEHSTransactionModel As EHSTransactionModel, ByVal udtEHSAccount As EHSAccount.EHSAccountModel, _
            ByVal udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel, ByVal udtSchemeClaimModel As Scheme.SchemeClaimModel, _
            Optional ByVal enumAppSource As EHSTransactionModel.AppSourceEnum = EHSTransactionModel.AppSourceEnum.WEB_FULL) As Common.ComObject.SystemMessage


            ' Use Service Date for Checking the corresponding SchemeClaim (SchemeSeq)

            Dim udtDB As New Database()

            Try
                udtDB.BeginTransaction()

                Return Me.InsertEHSTransaction(udtDB, udtEHSTransactionModel, udtEHSAccount, udtEHSPersonalInfo, udtSchemeClaimModel, enumAppSource)

                udtDB.CommitTransaction()

            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                Throw eSQL
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try
        End Function

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Update transaction for defer input (e.g. Co-payment fee, Reason for visit)
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <param name="udtEHSTransactionModel"></param>
        ''' <remarks></remarks>
        Public Sub UpdateEHSTransaction(ByRef udtDB As Database, ByVal udtEHSTransactionModel As EHSTransactionModel)

            ' Remove all Transaction addition Fields
            Me.RemoveTransactionAdditionalFields(udtDB, udtEHSTransactionModel.TransactionID)

            ' Insert Transaction addition Fields
            If Not udtEHSTransactionModel.TransactionAdditionFields Is Nothing AndAlso udtEHSTransactionModel.TransactionAdditionFields.Count > 0 Then
                Me.InsertTransactionAdditionalFields(udtDB, udtEHSTransactionModel.TransactionID, udtEHSTransactionModel)
            End If

        End Sub
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        ''' <summary>
        ''' Insert EHS Transaction (VoucherTransaction, TransactionDetail, TransactionAdditionalField)
        ''' DB Transaction Hold From Caller
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <param name="udtEHSTransactionModel"></param>
        ''' <param name="enumAppSource"></param>
        ''' <remarks></remarks>
        Public Function InsertEHSTransaction(ByRef udtDB As Database, ByVal udtEHSTransactionModel As EHSTransactionModel, ByVal udtEHSAccountModel As EHSAccount.EHSAccountModel, _
            ByVal udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel, ByVal udtSchemeClaimModel As Scheme.SchemeClaimModel, _
            Optional ByVal enumAppSource As EHSTransactionModel.AppSourceEnum = EHSTransactionModel.AppSourceEnum.WEB_FULL) As Common.ComObject.SystemMessage

            Dim strMsgCode As String = String.Empty
            ' Use Service Date for Checking the corresponding SchemeClaim (SchemeSeq)

            ' ----------------------------------------------------------
            ' Check Active Scheme
            ' ----------------------------------------------------------
            Dim udtSchemeClaimBLL As New SchemeClaimBLL()

            Dim udtCheckSchemeClaimModel As SchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtSchemeClaimModel.SchemeCode, udtEHSTransactionModel.ServiceDate.AddDays(1).AddMinutes(-1))
            'Dim udtCheckSchemeClaimModel As SchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtSchemeClaimModel.SchemeCode, udtEHSTransactionModel.ServiceDate)
            'If udtCheckSchemeClaimModel Is Nothing Then
            '    udtCheckSchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtSchemeClaimModel.SchemeCode, udtEHSTransactionModel.ServiceDate.AddDays(1).AddMinutes(-1))
            'End If

            If udtCheckSchemeClaimModel Is Nothing Then
                strMsgCode = "00105"
            End If

            ' ----------------------------------------------------------
            ' Check Transaction is newly create
            ' ----------------------------------------------------------
            If strMsgCode = "" Then
                ' Avoid double Post back, and resubmit the transaction
                If Not udtEHSTransactionModel.IsNew Then
                    strMsgCode = "00197"
                    'Throw New Exception("EHSTransactionBLL.InsertEHSTransaction: The EHS Transaction is retrieve from Database.")
                End If
            End If

            Select Case enumAppSource
                Case EHSTransactionModel.AppSourceEnum.IVRS
                    udtEHSTransactionModel.SourceApp = EHSTransactionModel.AppSourceClass.IVRS
                Case EHSTransactionModel.AppSourceEnum.WEB_FULL
                    udtEHSTransactionModel.SourceApp = EHSTransactionModel.AppSourceClass.WEB_FULL
                Case EHSTransactionModel.AppSourceEnum.WEB_TEXT
                    udtEHSTransactionModel.SourceApp = EHSTransactionModel.AppSourceClass.WEB_TEXT
                Case EHSTransactionModel.AppSourceEnum.ExternalWS
                    udtEHSTransactionModel.SourceApp = EHSTransactionModel.AppSourceClass.ExternalWS
            End Select

            ' ----------------------------------------------------------
            ' Check Transaction Detail 
            ' ----------------------------------------------------------
            If strMsgCode = "" Then
                If udtEHSTransactionModel.TransactionDetails.Count = 0 Then
                    strMsgCode = "00197"
                    Throw New Exception("EHSTransactionBLL.InsertEHSTransaction: EHSTransactionModel.TransactionDetails.Count = 0")
                End If
            End If

            Try
                Dim udtClaimRulesBLL As New ClaimRulesBLL()

                ' Check if Temporary Account Contain used Transaction before!
                If udtEHSAccountModel.AccountSource = EHSAccount.EHSAccountModel.SysAccountSource.TemporaryAccount Then
                    Dim prams1() As SqlParameter = {udtDB.MakeInParam("@Temp_Voucher_Acc_ID", SqlDbType.Char, 15, udtEHSTransactionModel.TempVoucherAccID)}
                    udtDB.RunProc("proc_VoucherTransaction_check", prams1)
                End If

                Dim drTSMPRow As DataRow = Me.getEHSAccountTSMP(udtDB, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum)

                Dim blnError As Boolean = False

                ' ----------------------------------------------------------
                ' Check Benefit
                ' ----------------------------------------------------------
                If strMsgCode = "" Then
                    If udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVaccine Then
                        ' Vaccine Benefit

                        blnError = udtClaimRulesBLL.chkVaccineTranBenefitUsed(udtDB, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, udtEHSTransactionModel.TransactionDetails)

                        'If claim COVID19, check vaccine lot no. whether exists
                        If udtEHSTransactionModel.TransactionDetails.FilterBySubsidizeItemDetail(SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19).Count > 0 Then
                            Dim udtCOVID19BLL As New COVID19.COVID19BLL
                            Dim strVaccineLotNo As String = udtEHSTransactionModel.TransactionAdditionFields.VaccineLotNo

                            If strVaccineLotNo IsNot Nothing AndAlso strVaccineLotNo <> String.Empty Then
                                Dim dtVaccineLot As DataTable = Nothing
                                Dim drVaccineLot() As DataRow = Nothing

                                Select Case udtEHSTransactionModel.SchemeCode.Trim
                                    Case SchemeClaimModel.COVID19CVC, SchemeClaimModel.COVID19DH, _
                                         SchemeClaimModel.COVID19RVP, SchemeClaimModel.COVID19OR, _
                                         SchemeClaimModel.COVID19SR, SchemeClaimModel.COVID19SB

                                        dtVaccineLot = udtCOVID19BLL.GetCOVID19VaccineLotMappingForCentre(udtEHSTransactionModel.ServiceProviderID, _
                                                                                                          udtEHSTransactionModel.PracticeID, _
                                                                                                          udtEHSTransactionModel.ServiceDate, _
                                                                                                          COVID19.COVID19BLL.Source.GetFromDB)

                                    Case SchemeClaimModel.VSS
                                        dtVaccineLot = udtCOVID19BLL.GetCOVID19VaccineLotMappingForPrivate(udtEHSTransactionModel.ServiceDate, _
                                                                                                           COVID19.COVID19BLL.Source.GetFromDB)

                                    Case SchemeClaimModel.RVP
                                        dtVaccineLot = udtCOVID19BLL.GetCOVID19VaccineLotMappingForRCH(udtEHSTransactionModel.ServiceDate, _
                                                                                                       COVID19.COVID19BLL.Source.GetFromDB)

                                End Select

                                drVaccineLot = dtVaccineLot.Select(String.Format("Vaccine_Lot_No='{0}'", strVaccineLotNo))

                                If drVaccineLot.Length = 0 Then
                                    blnError = True
                                End If

                            End If
                        End If

                        If blnError Then strMsgCode = "00197"

                        ' CRE13-001 - EHAPP [Start][Tommy L]
                        ' -------------------------------------------------------------------------------------
                    ElseIf udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeRegistration Then
                        ' INT13-0012 - Fix EHAPP concurrent claim checking [Start][Tommy L]
                        ' -------------------------------------------------------------------------------------
                        ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
                        ' -----------------------------------------------------------------------------------------
                        'If Me.getAvailableSubsidizeItem_Registration(udtEHSPersonalInfo, udtSchemeClaimModel.SubsidizeGroupClaimList(0)) > 0 Then
                        If Me.getAvailableSubsidizeItem_Registration(udtEHSPersonalInfo, udtSchemeClaimModel.SubsidizeGroupClaimList(0), udtDB) > 0 Then
                            ' CRE13-006 - HCVS Ceiling [End][Tommy L]
                            ' Subsidies for Registration is Available
                        Else
                            ' No Available Subsidies for Registration
                            strMsgCode = "00197"
                        End If
                        ' INT13-0012 - Fix EHAPP concurrent claim checking [End][Tommy L]
                        ' CRE13-001 - EHAPP [End][Tommy L]

                        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                    ElseIf udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeType_HAService Then
                        ' Available Subsidy 
                        Dim decAvailableSubsidy As Decimal = 0.0

                        decAvailableSubsidy = Me.getAvailableSubsidizeItem_SSSCMC(udtEHSPersonalInfo, udtSchemeClaimModel.SubsidizeGroupClaimList, udtDB)

                        '1. Compare the available subsidy between model and DB
                        '2. Check claimed amount between 0 and available subsidy
                        If (decAvailableSubsidy <> udtEHSTransactionModel.TransactionAdditionFields.SubsidyBeforeClaim) Or _
                            (decAvailableSubsidy <= 0 OrElse decAvailableSubsidy < udtEHSTransactionModel.TransactionDetails(0).TotalAmountRMB) Then

                            strMsgCode = "00197"
                        End If

                        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]
                    Else
                        ' Available Voucher 
                        Dim intAvailableVoucher As Integer = 0

                        ' CRE20-005 (Providing users' data in HCVS to eHR Patient Portal) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                        intAvailableVoucher = Me.getAvailableVoucher(udtEHSTransactionModel.ServiceDate, _
                                                                     udtSchemeClaimModel, _
                                                                     udtEHSPersonalInfo, _
                                                                     WriteOff.UpdateDB, _
                                                                     udtDB).GetAvailableVoucher()
                        ' CRE20-005 (Providing users' data in HCVS to eHR Patient Portal) [End][Chris YIM]	

                        'I-CRE17-005 (Performance Tuning) [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                        '1. Compare the available voucher between model and DB
                        '2. Check claimed amount between 0 and available voucher
                        If (intAvailableVoucher <> udtEHSTransactionModel.VoucherBeforeRedeem) Or _
                            (intAvailableVoucher <= 0 OrElse intAvailableVoucher < udtEHSTransactionModel.VoucherClaim) Then

                            strMsgCode = "00197"
                        End If
                        'I-CRE17-005 (Performance Tuning) [End][Chris YIM]
                    End If
                End If

                ' ----------------------------------------------------------
                ' Check ClaimRule
                ' ----------------------------------------------------------
                If strMsgCode = "" Then
                    Dim lstClaimResult As List(Of ClaimRulesBLL.ClaimRuleResult) = Nothing
                    Dim blnInvalidScheme As Boolean

                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                    ' -----------------------------------------------------------------------------------------
                    If udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVaccine Then
                        lstClaimResult = udtClaimRulesBLL.CheckClaimRuleForClaim(udtEHSPersonalInfo, udtEHSTransactionModel.ServiceDate, _
                                                                                 udtEHSTransactionModel.SchemeCode.Trim(), udtEHSTransactionModel.TransactionDetails, _
                                                                                 blnInvalidScheme, udtDB)
                        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
                        If blnInvalidScheme Then
                            strMsgCode = "00105"
                        Else
                            For Each udtClaimResult As ClaimRulesBLL.ClaimRuleResult In lstClaimResult
                                If udtClaimResult.IsBlock Then
                                    strMsgCode = "00197"
                                    Exit For
                                End If
                            Next
                        End If
                    End If
                End If

                ' ----------------------------------------------------------
                ' Insert
                ' ----------------------------------------------------------
                If strMsgCode = "" Then
                    Me.InsertVoucherTransaction(udtDB, udtEHSTransactionModel)

                    Me.InsertTransactionDetails(udtDB, udtEHSTransactionModel.TransactionID, udtEHSTransactionModel.TransactionDetails)

                    If Not udtEHSTransactionModel.TransactionAdditionFields Is Nothing AndAlso udtEHSTransactionModel.TransactionAdditionFields.Count > 0 Then
                        Me.InsertTransactionAdditionalFields(udtDB, udtEHSTransactionModel.TransactionID, udtEHSTransactionModel)
                    End If

                    Dim udtSubsidizeWriteOffBLL As New SubsidizeWriteOffBLL()

                    'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    udtSubsidizeWriteOffBLL.UpdateWriteOff(udtEHSTransactionModel.ServiceDate, _
                                                           udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, _
                                                           udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, _
                                                           udtEHSPersonalInfo.DOD, udtEHSPersonalInfo.ExactDOD, _
                                                           udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeCode, _
                                                           eHASubsidizeWriteOff_CreateReason.TxCreation, udtDB)
                    'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

                    If drTSMPRow Is Nothing Then
                        Me.insertEHSAccountTSMP(udtDB, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, udtEHSTransactionModel.ServiceProviderID)
                    Else
                        Me.updateEHSAccountTSMP(udtDB, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, udtEHSTransactionModel.ServiceProviderID, CType(drTSMPRow("TSMP"), Byte()))
                    End If
                End If

                If strMsgCode = "" Then
                    Return Nothing
                Else
                    Return New Common.ComObject.SystemMessage("990000", "E", strMsgCode)
                End If
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

        End Function

        ' CRE19-001 (VSS 2019 - Claim) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Insert EHS Transaction for SP (VoucherTransaction, TransactionDetail, TransactionAdditionalField)
        ''' DB Transaction Hold From Caller
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <param name="udtEHSTransactionModel"></param>
        ''' <param name="udtEHSAccountModel"></param>
        ''' <param name="udtEHSPersonalInfo"></param>
        ''' <param name="udtSchemeClaimModel"></param>
        ''' <param name="enumAppSource"></param>
        ''' <remarks></remarks>
        Public Sub InsertEHSTransactionWithoutChecking(ByRef udtDB As Database, ByVal udtEHSTransactionModel As EHSTransactionModel, ByVal udtEHSAccountModel As EHSAccount.EHSAccountModel, _
            ByVal udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel, ByVal udtSchemeClaimModel As Scheme.SchemeClaimModel, _
            Optional ByVal enumAppSource As EHSTransactionModel.AppSourceEnum = EHSTransactionModel.AppSourceEnum.WEB_FULL)

            Select Case enumAppSource
                Case EHSTransactionModel.AppSourceEnum.ExternalWS
                    udtEHSTransactionModel.SourceApp = EHSTransactionModel.AppSourceClass.ExternalWS
                Case EHSTransactionModel.AppSourceEnum.IVRS
                    udtEHSTransactionModel.SourceApp = EHSTransactionModel.AppSourceClass.IVRS
                Case EHSTransactionModel.AppSourceEnum.SFUpload
                    udtEHSTransactionModel.SourceApp = EHSTransactionModel.AppSourceClass.SFUpload
                Case EHSTransactionModel.AppSourceEnum.WEB_FULL
                    udtEHSTransactionModel.SourceApp = EHSTransactionModel.AppSourceClass.WEB_FULL
                Case EHSTransactionModel.AppSourceEnum.WEB_TEXT
                    udtEHSTransactionModel.SourceApp = EHSTransactionModel.AppSourceClass.WEB_TEXT
            End Select


            ' Check if Temporary Account Contain used Transaction before, throw ex if exists
            If udtEHSAccountModel.AccountSource = EHSAccount.EHSAccountModel.SysAccountSource.TemporaryAccount Then
                Dim prams1() As SqlParameter = {udtDB.MakeInParam("@Temp_Voucher_Acc_ID", SqlDbType.Char, 15, udtEHSTransactionModel.TempVoucherAccID)}
                udtDB.RunProc("proc_VoucherTransaction_check", prams1)
            End If

            Dim drTSMPRow As DataRow = Me.getEHSAccountTSMP(udtDB, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum)

            ' ----------------------------------------------------------
            ' Insert
            ' ----------------------------------------------------------
            Me.InsertVoucherTransaction(udtDB, udtEHSTransactionModel)

            Me.InsertTransactionDetails(udtDB, udtEHSTransactionModel.TransactionID, udtEHSTransactionModel.TransactionDetails)

            If Not udtEHSTransactionModel.TransactionAdditionFields Is Nothing AndAlso udtEHSTransactionModel.TransactionAdditionFields.Count > 0 Then
                Me.InsertTransactionAdditionalFields(udtDB, udtEHSTransactionModel.TransactionID, udtEHSTransactionModel)
            End If

            ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            If udtEHSTransactionModel.ManualReimburse Then
                If Not udtEHSTransactionModel.OverrideReason.Trim.Equals(String.Empty) AndAlso Not IsNothing(udtEHSTransactionModel.WarningMessage) Then
                    Me.InsertTransactionWarningResults(udtEHSTransactionModel)
                End If
            End If
            ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

            Dim udtSubsidizeWriteOffBLL As New SubsidizeWriteOffBLL()

            udtSubsidizeWriteOffBLL.UpdateWriteOff(udtEHSTransactionModel.ServiceDate, _
                                                    udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, _
                                                    udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, _
                                                    udtEHSPersonalInfo.DOD, udtEHSPersonalInfo.ExactDOD, _
                                                    udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeCode, _
                                                    eHASubsidizeWriteOff_CreateReason.TxCreation, udtDB)

            ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            If udtEHSTransactionModel.ManualReimburse Then
                Me.InsertManaulReimburseWithApproval(udtEHSTransactionModel, udtDB)
            End If
            ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

            If drTSMPRow Is Nothing Then
                Me.insertEHSAccountTSMP(udtDB, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, udtEHSTransactionModel.ServiceProviderID)
            Else
                Me.updateEHSAccountTSMP(udtDB, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, udtEHSTransactionModel.ServiceProviderID, CType(drTSMPRow("TSMP"), Byte()))
            End If

        End Sub
        ' CRE19-001 (VSS 2019 - Claim) [End][Winnie]

        ''' <summary>
        ''' Insert EHS Transaction for Back Office user
        ''' </summary>
        ''' <param name="udtEHSTransactionModel"></param>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="udtEHSPersonalInfo"></param>
        ''' <param name="udtSchemeClaimModel"></param>
        ''' <param name="enumAppSource"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' 
        Public Function InsertEHSTransactionWithoutChecking(ByVal udtEHSTransactionModel As EHSTransactionModel, ByVal udtEHSAccount As EHSAccount.EHSAccountModel, _
                                                          ByVal udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel, ByVal udtSchemeClaimModel As Scheme.SchemeClaimModel, _
                                                          Optional ByVal enumAppSource As EHSTransactionModel.AppSourceEnum = EHSTransactionModel.AppSourceEnum.WEB_FULL) As Common.ComObject.SystemMessage


            ' Use Service Date for Checking the corresponding SchemeClaim (SchemeSeq)

            Dim strMsgCode As String = String.Empty

            ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [Start][Koala]

            Select Case enumAppSource
                Case EHSTransactionModel.AppSourceEnum.ExternalWS
                    udtEHSTransactionModel.SourceApp = EHSTransactionModel.AppSourceClass.ExternalWS
                Case EHSTransactionModel.AppSourceEnum.IVRS
                    udtEHSTransactionModel.SourceApp = EHSTransactionModel.AppSourceClass.IVRS
                Case EHSTransactionModel.AppSourceEnum.SFUpload
                    udtEHSTransactionModel.SourceApp = EHSTransactionModel.AppSourceClass.SFUpload
                Case EHSTransactionModel.AppSourceEnum.WEB_FULL
                    udtEHSTransactionModel.SourceApp = EHSTransactionModel.AppSourceClass.WEB_FULL
                Case EHSTransactionModel.AppSourceEnum.WEB_TEXT
                    udtEHSTransactionModel.SourceApp = EHSTransactionModel.AppSourceClass.WEB_TEXT
            End Select
            'udtEHSTransactionModel.SourceApp = EHSTransactionModel.AppSourceClass.WEB_FULL
            ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [End][Koala]

            Dim udtDB As New Database()

            Try
                udtDB.BeginTransaction()

                'CRE13-006 HCVS Ceiling [Start][Karl]
                Dim drTSMPRow As DataRow = Me.getEHSAccountTSMP(udtDB, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum)
                'CRE13-006 HCVS Ceiling [End][Karl]

                If strMsgCode = "" Then
                    Me.InsertVoucherTransaction(udtDB, udtEHSTransactionModel)

                    Me.InsertTransactionDetails(udtDB, udtEHSTransactionModel.TransactionID, udtEHSTransactionModel.TransactionDetails)

                    If Not udtEHSTransactionModel.TransactionAdditionFields Is Nothing AndAlso udtEHSTransactionModel.TransactionAdditionFields.Count > 0 Then
                        Me.InsertTransactionAdditionalFields(udtDB, udtEHSTransactionModel.TransactionID, udtEHSTransactionModel)
                    End If

                    If Not udtEHSTransactionModel.OverrideReason.Trim.Equals(String.Empty) AndAlso Not IsNothing(udtEHSTransactionModel.WarningMessage) Then
                        Me.InsertTransactionWarningResults(udtEHSTransactionModel)
                    End If

                    Dim udtSubsidizeWriteOffBLL As New SubsidizeWriteOffBLL()

                    'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    udtSubsidizeWriteOffBLL.UpdateWriteOff(udtEHSTransactionModel.ServiceDate, _
                                                           udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, _
                                                           udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, _
                                                           udtEHSPersonalInfo.DOD, udtEHSPersonalInfo.ExactDOD, _
                                                           udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeCode, _
                                                           eHASubsidizeWriteOff_CreateReason.TxCreation, udtDB)
                    'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

                    Me.InsertManaulReimburse(udtEHSTransactionModel, udtDB)

                    'CRE13-006 HCVS Ceiling [Start][Karl]
                    If drTSMPRow Is Nothing Then
                        Me.insertEHSAccountTSMP(udtDB, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, udtEHSTransactionModel.CreateBy)
                    Else
                        Me.updateEHSAccountTSMP(udtDB, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, udtEHSTransactionModel.CreateBy, CType(drTSMPRow("TSMP"), Byte()))
                    End If
                    'CRE13-006 HCVS Ceiling [End][Karl]

                End If

                If strMsgCode = "" Then
                    udtDB.CommitTransaction()
                    Return Nothing
                Else
                    udtDB.CommitTransaction()
                    Return New Common.ComObject.SystemMessage("990000", "E", strMsgCode)
                End If

            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                Throw eSQL
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try
        End Function

        Public Function InsertEHSTransactionWithoutCheckingWSUpload(ByVal udtEHSTransactionModel As EHSTransactionModel, ByVal udtEHSAccount As EHSAccount.EHSAccountModel, _
                                                           ByVal udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel, ByVal udtSchemeClaimModel As Scheme.SchemeClaimModel, _
                                                           ByVal enumAppSource As EHSTransactionModel.AppSourceEnum, ByVal udtDB As Common.DataAccess.Database, Optional ByVal blnIsManualReimbursed As Boolean = True) As Common.ComObject.SystemMessage


            ' Use Service Date for Checking the corresponding SchemeClaim (SchemeSeq)

            Dim strMsgCode As String = String.Empty

            udtEHSTransactionModel.SourceApp = EHSTransactionModel.AppSourceClass.WEB_FULL

            Select Case enumAppSource
                Case EHSTransactionModel.AppSourceEnum.ExternalWS
                    udtEHSTransactionModel.SourceApp = EHSTransactionModel.AppSourceClass.ExternalWS
                Case EHSTransactionModel.AppSourceEnum.WEB_FULL
                    udtEHSTransactionModel.SourceApp = EHSTransactionModel.AppSourceClass.WEB_FULL
            End Select

            'Dim udtDB As New Database()
            If udtDB Is Nothing Then
                udtDB = New Database()
            End If

            Try
                udtDB.BeginTransaction()

                If strMsgCode = "" Then
                    Me.InsertVoucherTransaction(udtDB, udtEHSTransactionModel)

                    Me.InsertTransactionDetails(udtDB, udtEHSTransactionModel.TransactionID, udtEHSTransactionModel.TransactionDetails)

                    If Not udtEHSTransactionModel.TransactionAdditionFields Is Nothing AndAlso udtEHSTransactionModel.TransactionAdditionFields.Count > 0 Then
                        Me.InsertTransactionAdditionalFields(udtDB, udtEHSTransactionModel.TransactionID, udtEHSTransactionModel)
                    End If

                    If Not udtEHSTransactionModel.OverrideReason.Trim.Equals(String.Empty) AndAlso Not IsNothing(udtEHSTransactionModel.WarningMessage) Then
                        Me.InsertTransactionWarningResults(udtEHSTransactionModel)
                    End If

                    Dim udtSubsidizeWriteOffBLL As New SubsidizeWriteOffBLL()

                    'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    udtSubsidizeWriteOffBLL.UpdateWriteOff(udtEHSTransactionModel.ServiceDate, _
                                                           udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, _
                                                           udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, _
                                                           udtEHSPersonalInfo.DOD, udtEHSPersonalInfo.ExactDOD, _
                                                           udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeCode, _
                                                           eHASubsidizeWriteOff_CreateReason.TxCreation, udtDB)
                    'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

                    If blnIsManualReimbursed Then
                        Me.InsertManaulReimburse(udtEHSTransactionModel, udtDB)
                    End If

                End If

                If strMsgCode = "" Then
                    Return Nothing
                Else
                    Return New Common.ComObject.SystemMessage("990000", "E", strMsgCode)
                End If

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try
        End Function

        ' CRE19-001 (VSS 2019) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Public Sub InsertVoucherTranSuspendLOG(ByRef db As Database, ByVal strTranID As String, ByVal dtmUpdatetime As String, ByVal strUserID As String, ByVal strOriginalRecordStatus As String, ByVal strNewRecordStatus As String, ByVal strRemark As String)
            Try
                Dim prams() As SqlParameter = {db.MakeInParam("@transaction_id", SqlDbType.Char, 20, strTranID), _
                                                db.MakeInParam("@system_dtm", SqlDbType.DateTime, 8, dtmUpdatetime), _
                                                db.MakeInParam("@update_by", SqlDbType.VarChar, 20, strUserID), _
                                                db.MakeInParam("@original_record_status", SqlDbType.Char, 1, strOriginalRecordStatus), _
                                                db.MakeInParam("@record_status", SqlDbType.Char, 1, strNewRecordStatus), _
                                                db.MakeInParam("@remark", SqlDbType.NVarChar, 255, strRemark) _
                                                }

                db.RunProc("proc_VoucherTranSuspendLOG_add", prams)

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
            End Try

        End Sub
        ' CRE19-001 (VSS 2019) [End][Winnie]
#End Region

#Region "[Public] Update EHS Transaction"

        ''' <summary>
        ''' Update the Transaction Doc Code
        ''' </summary>        
        ''' <param name="strTransactionID"></param>
        ''' <param name="strDocCode"></param>
        ''' <param name="strUpdateBy"></param>
        ''' <param name="dtmUpdateDtm"></param>
        ''' <param name="udtDB"></param>
        ''' <remarks></remarks>  ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
        Public Sub UpdateTransactionDocCode(ByVal strTransactionID As String, ByVal strDocCode As String, ByVal strUpdateBy As String, ByVal dtmUpdateDtm As DateTime, Optional ByRef udtDB As Database = Nothing)
            If udtDB Is Nothing Then udtDB = New Database()

            Dim prams() As SqlParameter = {udtDB.MakeInParam("@transaction_id", SqlDbType.Char, 20, strTransactionID), _
                                                udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, strDocCode), _
                                                udtDB.MakeInParam("@update_by", SqlDbType.VarChar, 20, strUpdateBy), _
                                                udtDB.MakeInParam("@update_dtm", SqlDbType.DateTime, 8, dtmUpdateDtm) _
            }

            udtDB.RunProc("proc_VoucherTransaction_upd_DocCode", prams)

        End Sub
        ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]

        ''' <summary>
        ''' Update The Transaction related Temporary Account for Rectify case
        ''' Checking is added to ensure the transaction with X account Exist
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <param name="strTransactionID"></param>
        ''' <param name="strXVoucherAccID"></param>
        ''' <param name="strNewVoucherAccID"></param>
        ''' <remarks></remarks>  ' CRE13-006 HCVS Ceiling [Start][Karl]
        Private Sub UpdateTransactionWithNewTemporaryAccount(ByRef udtDB As Database, ByVal strTransactionID As String, ByVal strXVoucherAccID As String, ByVal strNewVoucherAccID As String)
            'Public Sub UpdateTransactionWithNewTemporaryAccount(ByRef udtDB As Database, ByVal strTransactionID As String, ByVal strXVoucherAccID As String, ByVal strNewVoucherAccID As String)
            ' CRE13-006 HCVS Ceiling [End][Karl]
            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Transaction_ID", EHSTransactionModel.Transaction_ID_DataType, EHSTransactionModel.Transaction_ID_DataSize, strTransactionID.Trim()), _
                udtDB.MakeInParam("@Temp_Voucher_Acc_ID", EHSAccount.EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccount.EHSAccountModel.Voucher_Acc_ID_DataSize, strNewVoucherAccID), _
                udtDB.MakeInParam("@Old_Temp_Voucher_Acc_ID", EHSAccount.EHSAccountModel.Voucher_Acc_ID_DataType, EHSAccount.EHSAccountModel.Voucher_Acc_ID_DataSize, strXVoucherAccID.Trim())}

            udtDB.RunProc("proc_VoucherTransaction_upd_ReplaceVRAccIDbyTranID", prams)

        End Sub
        ' CRE13-006 HCVS Ceiling [Start][Karl]
        ''' <summary>
        ''' Update The Transaction related Temporary Account for Rectify case
        ''' Checking is added to ensure the transaction with X account Exist
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <param name="strTransactionID"></param>
        ''' <param name="strXVoucherAccID"></param>
        ''' <param name="strNewVoucherAccID"></param>
        ''' <param name="strEHSAccountDocCode"></param>
        ''' <param name="strEHSAccountIdentityNum"></param>
        ''' <param name="strUpdatedBy"></param>
        ''' <remarks></remarks> 
        Public Sub UpdateTransactionWithNewTemporaryAccount(ByRef udtDB As Database, ByVal strTransactionID As String, ByVal strXVoucherAccID As String, ByVal strNewVoucherAccID As String, _
        ByVal strEHSAccountDocCode As String, ByVal strEHSAccountIdentityNum As String, ByVal strUpdatedBy As String)

            Dim drTSMPRow As DataRow = Me.getEHSAccountTSMP(udtDB, strEHSAccountDocCode, strEHSAccountIdentityNum)

            Call UpdateTransactionWithNewTemporaryAccount(udtDB, strTransactionID, strXVoucherAccID, strNewVoucherAccID)

            If drTSMPRow Is Nothing Then
                Me.insertEHSAccountTSMP(udtDB, strEHSAccountDocCode, strEHSAccountIdentityNum, strUpdatedBy)
            Else
                Me.updateEHSAccountTSMP(udtDB, strEHSAccountDocCode, strEHSAccountIdentityNum, strUpdatedBy, CType(drTSMPRow("TSMP"), Byte()))
            End If
        End Sub

        ' CRE13-006 HCVS Ceiling [End][Karl]
        Public Sub DeleteEHSTransactionManualReimburse(ByVal udtEHSTransaction As EHSTransactionModel, ByVal strUpdateBy As String, ByVal dtmUpdateDtm As DateTime)
            Dim udtDB As New Database()

            Try
                udtDB.BeginTransaction()
                'CRE13-006 HCVS Ceiling [Start][Karl]
                Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSTransaction.EHSAcct.EHSPersonalInformationList.Filter(udtEHSTransaction.DocCode)
                Dim drTSMPRow As DataRow = Me.getEHSAccountTSMP(udtDB, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum)
                'CRE13-006 HCVS Ceiling [End][Karl]

                Me.UpdateTransactionStatus(udtEHSTransaction, EHSTransactionModel.TransRecordStatusClass.Removed, strUpdateBy, dtmUpdateDtm, udtDB)
                Me.UpdateManualReimburseDelete(udtEHSTransaction, udtDB, strUpdateBy, dtmUpdateDtm)

                'CRE13-006 HCVS Ceiling [Start][Karl]
                If drTSMPRow Is Nothing Then
                    Me.insertEHSAccountTSMP(udtDB, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, strUpdateBy)
                Else
                    Me.updateEHSAccountTSMP(udtDB, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, strUpdateBy, CType(drTSMPRow("TSMP"), Byte()))
                End If
                'CRE13-006 HCVS Ceiling [End][Karl]

                ' INT20-0014 (Fix unable to open invalidated PPP transaction) [Start][Winnie]
                ' ---------------------------------------------------------------------------
                ' Remove [TempAccount] & [SpecialAccount] (if any)
                Dim udtEHSAccount As EHSAccountModel = udtEHSTransaction.EHSAcct
                Dim udtEHSAccountBLL As New EHSAccountBLL

                Select Case udtEHSAccount.AccountSource
                    Case EHSAccount.EHSAccountModel.SysAccountSource.TemporaryAccount
                        Dim blnErasedAmendHistroy As Boolean = False

                        '==================================================================== Code for SmartID ============================================================================
                        ' Get the temp account with account purpose = 'O'
                        Dim udtTempEHSAccount_Original As EHSAccountModel = Nothing
                        If udtEHSAccount.AccountPurpose.Trim = EHSAccountModel.AccountPurposeClass.ForAmendment AndAlso Not udtEHSAccount.OriginalAmendAccID.Trim.Equals(String.Empty) Then
                            udtTempEHSAccount_Original = udtEHSAccountBLL.LoadTempEHSAccountByVRID(udtEHSAccount.OriginalAmendAccID.Trim)
                            blnErasedAmendHistroy = True
                        End If

                        ' Also remove the temp account with account purpose = 'O'
                        If Not IsNothing(udtTempEHSAccount_Original) AndAlso udtTempEHSAccount_Original.AccountPurpose.Trim = EHSAccount.EHSAccountModel.AccountPurposeClass.ForAmendmentOld Then
                            udtEHSAccountBLL.UpdateTempEHSAccountReject(udtDB, udtTempEHSAccount_Original, udtEHSTransaction.ServiceProviderID, dtmUpdateDtm)
                        End If

                        If blnErasedAmendHistroy Then
                            ' Update PersonalInfoAmendHistory RecordStats = 'E' (Erased) and SubmitToVerify = 'N' (Doesn't verify)
                            If Not IsNothing(udtEHSAccount.ValidatedAccID) AndAlso Not udtEHSAccount.ValidatedAccID.Equals(String.Empty) Then
                                udtEHSAccountBLL.UpdatePersonalInfoAmendHistoryWithdrawAmendment(udtDB, udtEHSAccount, udtEHSTransaction.ServiceProviderID)
                            End If
                        End If
                        '==================================================================================================================================================================

                        udtEHSAccountBLL.UpdateTempEHSAccountReject(udtDB, udtEHSAccount, udtEHSTransaction.ServiceProviderID, dtmUpdateDtm)

                    Case EHSAccount.EHSAccountModel.SysAccountSource.SpecialAccount
                        udtEHSAccountBLL.UpdateSpecialEHSAccountReject(udtDB, udtEHSAccount, strUpdateBy, dtmUpdateDtm)

                End Select
                ' INT20-0014 (Fix unable to open invalidated PPP transaction) [End][Winnie]

                udtDB.CommitTransaction()
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try

        End Sub

        Public Sub UpdateEHSTransactionStatus(ByVal strTransactionID As String, ByVal strRecordStatus As String, ByVal strUpdateBy As String, ByVal dtmUpdateDtm As DateTime, ByVal byteTSMP As Byte(), Optional ByVal udtDB As Database = Nothing)

            Me.UpdateTransactionStatus(strTransactionID, strRecordStatus, strUpdateBy, dtmUpdateDtm, byteTSMP, udtDB)
        End Sub
        ' CRE13-001 EHAPP [Start][Karl]
        ' -----------------------------------------------------------------------------------------
        'Public Sub UpdateManualReimburseApprove(ByVal udtEHSTransactionModel As EHSTransactionModel, ByVal strUpdateBy As String, ByVal dtmUpdateDtm As DateTime, ByRef udtDB As Database)
        Public Sub UpdateManualReimburseApprove(ByVal udtEHSTransactionModel As EHSTransactionModel, ByVal strUpdateBy As String, ByVal dtmUpdateDtm As DateTime, ByVal udtSchemeClaimModel As SchemeClaimModel, ByRef udtDB As Database)
            ' CRE13-001 EHAPP [End][Karl]

            udtEHSTransactionModel.ApprovalBy = strUpdateBy
            udtEHSTransactionModel.ApprovalDate = dtmUpdateDtm
            udtEHSTransactionModel.RejectBy = String.Empty
            udtEHSTransactionModel.RejectDate = Nothing

            ' CRE13-001 EHAPP [Start][Karl]
            ' -----------------------------------------------------------------------------------------
            Dim strUpdateStatus As String

            'if scheme do not involves reimbursement,  update transaction status to confirm transaction status (in schemeClaim table) upon approval
            'otherwise, update the transaction to EHSTransactionModel.TransRecordStatusClass.Reimbursed
            ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
            If udtSchemeClaimModel.ReimbursementMode = SchemeClaimModel.EnumReimbursementMode.NoReimbursement Then
                strUpdateStatus = Trim(udtSchemeClaimModel.ConfirmedTransactionStatus)
            Else
                strUpdateStatus = EHSTransactionModel.TransRecordStatusClass.Reimbursed
            End If
            ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

            Me.UpdateTransactionStatus(udtEHSTransactionModel, strUpdateStatus, strUpdateBy, dtmUpdateDtm, udtDB)
            Me.UpdateManualReimburseStatus(udtEHSTransactionModel, strUpdateStatus, udtDB)
            'Me.UpdateTransactionStatus(udtEHSTransactionModel, EHSTransactionModel.TransRecordStatusClass.Reimbursed, strUpdateBy, dtmUpdateDtm, udtDB)
            'Me.UpdateManualReimburseStatus(udtEHSTransactionModel, EHSTransactionModel.TransRecordStatusClass.Reimbursed, udtDB)

            ' CRE13-001 EHAPP [End][Karl]
        End Sub

        Public Sub UpdateManualReimburseReject(ByVal udtEHSTransactionModel As EHSTransactionModel, ByVal strUpdateBy As String, ByVal dtmUpdateDtm As DateTime, ByRef udtDB As Database)
            udtEHSTransactionModel.ApprovalBy = String.Empty
            udtEHSTransactionModel.ApprovalDate = Nothing
            udtEHSTransactionModel.RejectBy = strUpdateBy
            udtEHSTransactionModel.RejectDate = dtmUpdateDtm

            'CRE13-006 HCVS Ceiling [Start][Karl]
            Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSTransactionModel.EHSAcct.EHSPersonalInformationList.Filter(udtEHSTransactionModel.DocCode)
            Dim drTSMPRow As DataRow = Me.getEHSAccountTSMP(udtDB, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum)
            'CRE13-006 HCVS Ceiling [End][Karl]

            Me.UpdateTransactionStatus(udtEHSTransactionModel, EHSTransactionModel.TransRecordStatusClass.Removed, strUpdateBy, dtmUpdateDtm, udtDB)
            Me.UpdateManualReimburseStatus(udtEHSTransactionModel, EHSTransactionModel.ManualReimbursementStatusClass.Removed, udtDB)

            'CRE13-006 HCVS Ceiling [Start][Karl]
            If drTSMPRow Is Nothing Then
                Me.insertEHSAccountTSMP(udtDB, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, strUpdateBy)
            Else
                Me.updateEHSAccountTSMP(udtDB, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, strUpdateBy, CType(drTSMPRow("TSMP"), Byte()))
            End If
            'CRE13-006 HCVS Ceiling [End][Karl]

            ' INT20-0014 (Fix unable to open invalidated PPP transaction) [Start][Winnie]
            ' ---------------------------------------------------------------------------
            ' Remove [TempAccount] & [SpecialAccount] (if any)
            Dim udtEHSAccount As EHSAccountModel = udtEHSTransactionModel.EHSAcct
            Dim udtEHSAccountBLL As New EHSAccountBLL

            Select Case udtEHSAccount.AccountSource
                Case EHSAccount.EHSAccountModel.SysAccountSource.TemporaryAccount
                    Dim blnErasedAmendHistroy As Boolean = False

                    '==================================================================== Code for SmartID ============================================================================
                    ' Get the temp account with account purpose = 'O'
                    Dim udtTempEHSAccount_Original As EHSAccountModel = Nothing
                    If udtEHSAccount.AccountPurpose.Trim = EHSAccountModel.AccountPurposeClass.ForAmendment AndAlso Not udtEHSAccount.OriginalAmendAccID.Trim.Equals(String.Empty) Then
                        udtTempEHSAccount_Original = udtEHSAccountBLL.LoadTempEHSAccountByVRID(udtEHSAccount.OriginalAmendAccID.Trim)
                        blnErasedAmendHistroy = True
                    End If

                    ' Also remove the temp account with account purpose = 'O'
                    If Not IsNothing(udtTempEHSAccount_Original) AndAlso udtTempEHSAccount_Original.AccountPurpose.Trim = EHSAccount.EHSAccountModel.AccountPurposeClass.ForAmendmentOld Then
                        udtEHSAccountBLL.UpdateTempEHSAccountReject(udtDB, udtTempEHSAccount_Original, udtEHSTransactionModel.ServiceProviderID, dtmUpdateDtm)
                    End If

                    If blnErasedAmendHistroy Then
                        ' Update PersonalInfoAmendHistory RecordStats = 'E' (Erased) and SubmitToVerify = 'N' (Doesn't verify)
                        If Not IsNothing(udtEHSAccount.ValidatedAccID) AndAlso Not udtEHSAccount.ValidatedAccID.Equals(String.Empty) Then
                            udtEHSAccountBLL.UpdatePersonalInfoAmendHistoryWithdrawAmendment(udtDB, udtEHSAccount, udtEHSTransactionModel.ServiceProviderID)
                        End If
                    End If
                    '==================================================================================================================================================================

                    udtEHSAccountBLL.UpdateTempEHSAccountReject(udtDB, udtEHSAccount, udtEHSTransactionModel.ServiceProviderID, dtmUpdateDtm)

                Case EHSAccount.EHSAccountModel.SysAccountSource.SpecialAccount
                    udtEHSAccountBLL.UpdateSpecialEHSAccountReject(udtDB, udtEHSAccount, strUpdateBy, dtmUpdateDtm)

            End Select
            ' INT20-0014 (Fix unable to open invalidated PPP transaction) [End][Winnie]
        End Sub

        Public Sub UpdateManualReimburseDelete(ByVal udtEHSTransactionModel As EHSTransactionModel, ByRef udtDB As Database, ByVal strUpdateBy As String, ByVal dtmUpdateDtm As DateTime)
            udtEHSTransactionModel.ApprovalBy = String.Empty
            udtEHSTransactionModel.ApprovalDate = Nothing
            udtEHSTransactionModel.RejectBy = String.Empty
            udtEHSTransactionModel.RejectDate = Nothing
            Me.UpdateManualReimburseStatus(udtEHSTransactionModel, EHSTransactionModel.ManualReimbursementStatusClass.Removed, udtDB)
        End Sub

#End Region

#Region "Private Retrieve Function"
        'For IVRS
        ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Lawrence]
        Private Function searchVoidableTransaction(ByVal strDocCode As String, ByVal strIRVSIdentitynum As String, _
            ByVal dtmDOB As Date, ByVal strExactDOB As String, ByVal strSPID As String, Optional ByVal udtDB As Database = Nothing) As EHSTransactionModelCollection
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Lawrence]

            Dim udtEHSTransactionList As New EHSTransactionModelCollection()
            Dim udtEHSTransactionModel As EHSTransactionModel = Nothing

            If udtDB Is Nothing Then udtDB = New Database()
            Dim dt As New DataTable()

            'strIRVSIdentitynum = Me._udtFormatter.formatDocumentIdentityNumber(strDocCode, strIRVSIdentitynum)

            Try
                ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Lawrence]
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, strDocCode), _
                    udtDB.MakeInParam("@identity", SqlDbType.VarChar, 20, strIRVSIdentitynum), _
                    udtDB.MakeInParam("@DOB", SqlDbType.DateTime, 8, dtmDOB), _
                    udtDB.MakeInParam("@Exact_DOB", SqlDbType.Char, 1, strExactDOB), _
                    udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID.Trim())}
                udtDB.RunProc("proc_VoucherTransactionVoid_get_byDocIDDOB_IVRS", prams, dt)
                ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Lawrence]

                For Each dr As DataRow In dt.Rows
                    udtEHSTransactionModel = Me.FillVoucherTransaction(dr)
                    udtEHSTransactionList.Add(udtEHSTransactionModel)
                Next

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return udtEHSTransactionList

        End Function

        'For IVRS
        ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Lawrence]
        Private Function searchVoidableTransaction(ByVal strDocCode As String, ByVal strIRVSIdentitynum As String, ByVal intECAge As Integer, _
            ByVal dtmDOR As Date, ByVal strSPID As String, Optional ByVal udtDB As Database = Nothing) As EHSTransactionModelCollection
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Lawrence]

            Dim udtEHSTransactionList As New EHSTransactionModelCollection()
            Dim udtEHSTransactionModel As EHSTransactionModel = Nothing

            If udtDB Is Nothing Then udtDB = New Database()
            Dim dt As New DataTable()

            'strIRVSIdentitynum = Me._udtFormatter.formatDocumentIdentityNumber(strDocCode, strIRVSIdentitynum)

            Try
                ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Lawrence]
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, strDocCode), _
                    udtDB.MakeInParam("@identity", SqlDbType.VarChar, 20, strIRVSIdentitynum), _
                    udtDB.MakeInParam("@EC_Age", SqlDbType.Int, 4, intECAge), _
                    udtDB.MakeInParam("@DOR", SqlDbType.DateTime, 8, dtmDOR), _
                    udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID.Trim())}
                udtDB.RunProc("proc_VoucherTransactionVoid_get_byECAgeECDOR_IVRS", prams, dt)
                ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Lawrence]

                For Each dr As DataRow In dt.Rows
                    udtEHSTransactionModel = Me.FillVoucherTransaction(dr)
                    udtEHSTransactionList.Add(udtEHSTransactionModel)
                Next

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return udtEHSTransactionList

        End Function


        ' CRE13-001 - EHAPP [Start][Koala]
        ' -------------------------------------------------------------------------------------
        ''' <summary>
        ''' For HCSP Text Only Version Claim Transaction Management to search transaction
        ''' </summary>
        ''' <param name="strDocCode"></param>
        ''' <param name="strIdentitynum"></param>
        ''' <param name="strAdoptionPrefix"></param>
        ''' <param name="dtmDOB"></param>
        ''' <param name="strExactDOB"></param>
        ''' <param name="strSPID"></param>
        ''' <param name="strDataEntryBy"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function searchTransaction(ByVal strDocCode As String, ByVal strIdentitynum As String, ByVal strAdoptionPrefix As String, _
            ByVal dtmDOB As Date, ByVal strExactDOB As String, ByVal strSPID As String, ByVal strDataEntryBy As String, ByVal enumSubPlatform As [Enum], Optional ByVal udtDB As Database = Nothing) As EHSTransactionModelCollection

            Dim udtEHSTransactionList As New EHSTransactionModelCollection()
            Dim udtEHSTransactionModel As EHSTransactionModel = Nothing

            If udtDB Is Nothing Then udtDB = New Database()
            Dim dt As New DataTable()

            strIdentitynum = Me._udtFormatter.formatDocumentIdentityNumber(strDocCode, strIdentitynum)

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim strSubPlatform As String = String.Empty
            If Not enumSubPlatform Is Nothing Then
                strSubPlatform = enumSubPlatform.ToString
            End If

            Try
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, strDocCode), _
                    udtDB.MakeInParam("@identity", SqlDbType.VarChar, 20, strIdentitynum), _
                    udtDB.MakeInParam("@Adoption_Prefix", SqlDbType.Char, 7, strAdoptionPrefix.Trim()), _
                    udtDB.MakeInParam("@DOB", SqlDbType.DateTime, 8, dtmDOB), _
                    udtDB.MakeInParam("@Exact_DOB", SqlDbType.Char, 1, strExactDOB), _
                    udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID.Trim()), _
                    udtDB.MakeInParam("@DataEntry_By", SqlDbType.VarChar, 20, strDataEntryBy.Trim()), _
                    udtDB.MakeInParam("@Available_HCSP_SubPlatform", SqlDbType.VarChar, 2, IIf(strSubPlatform.Trim.Equals(String.Empty), DBNull.Value, strSubPlatform))}
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
                udtDB.RunProc("proc_VoucherTransaction_get_byDocCodeDocIDDOB", prams, dt)

                For Each dr As DataRow In dt.Rows
                    udtEHSTransactionModel = Me.FillVoucherTransaction(dr)
                    udtEHSTransactionList.Add(udtEHSTransactionModel)
                Next

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return udtEHSTransactionList

        End Function

        ''' <summary>
        ''' For HCSP Text Only Version Claim Transaction Management to search transaction
        ''' </summary>
        ''' <param name="strDocCode"></param>
        ''' <param name="strIdentitynum"></param>
        ''' <param name="intECAge"></param>
        ''' <param name="dtmDOR"></param>
        ''' <param name="strSPID"></param>
        ''' <param name="strDataEntryBy"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function searchTransaction(ByVal strDocCode As String, ByVal strIdentitynum As String, ByVal intECAge As Integer, _
            ByVal dtmDOR As Date, ByVal strSPID As String, ByVal strDataEntryBy As String, ByVal enumSubPlatform As [Enum], Optional ByVal udtDB As Database = Nothing) As EHSTransactionModelCollection

            Dim udtEHSTransactionList As New EHSTransactionModelCollection()
            Dim udtEHSTransactionModel As EHSTransactionModel = Nothing

            If udtDB Is Nothing Then udtDB = New Database()
            Dim dt As New DataTable()

            strIdentitynum = Me._udtFormatter.formatDocumentIdentityNumber(strDocCode, strIdentitynum)

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim strSubPlatform As String = String.Empty
            If Not enumSubPlatform Is Nothing Then
                strSubPlatform = enumSubPlatform.ToString
            End If

            Try
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, strDocCode), _
                    udtDB.MakeInParam("@identity", SqlDbType.VarChar, 20, strIdentitynum), _
                    udtDB.MakeInParam("@EC_Age", SqlDbType.Int, 4, intECAge), _
                    udtDB.MakeInParam("@DOR", SqlDbType.DateTime, 8, dtmDOR), _
                    udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID.Trim()), _
                    udtDB.MakeInParam("@DataEntry_By", SqlDbType.VarChar, 20, strDataEntryBy.Trim()), _
                    udtDB.MakeInParam("@Available_HCSP_SubPlatform", SqlDbType.VarChar, 2, IIf(strSubPlatform.Trim.Equals(String.Empty), DBNull.Value, strSubPlatform))}
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
                udtDB.RunProc("proc_VoucherTransaction_get_byECAgeECDOR", prams, dt)

                For Each dr As DataRow In dt.Rows
                    udtEHSTransactionModel = Me.FillVoucherTransaction(dr)
                    udtEHSTransactionList.Add(udtEHSTransactionModel)
                Next

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return udtEHSTransactionList
        End Function

        Private Function getVoucherTransaction(ByVal strTranID As String, Optional ByVal udtDB As Database = Nothing) As EHSTransactionModel

            Dim udtEHSTransactionModel As EHSTransactionModel = Nothing
            If udtDB Is Nothing Then udtDB = New Database()
            Dim dt As New DataTable()

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@tran_id", EHSTransactionModel.Transaction_ID_DataType, EHSTransactionModel.Transaction_ID_DataSize, strTranID)}
                udtDB.RunProc("proc_VoucherTransaction_get_byTranIDv2", prams, dt)

                If dt.Rows.Count > 0 Then
                    udtEHSTransactionModel = Me.FillVoucherTransaction(dt.Rows(0))

                    Dim strPreSchool As String = String.Empty
                    If Not dt.Rows(0).IsNull("PreSchool") Then
                        strPreSchool = dt.Rows(0)("PreSchool").ToString().Trim()
                    End If
                    udtEHSTransactionModel.PreSchool = strPreSchool
                End If

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return udtEHSTransactionModel

        End Function

        Private Function FillVoucherTransaction(ByVal drSource As DataRow) As EHSTransactionModel

            Dim strVoucher_Acc_ID As String = String.Empty
            Dim strTemp_Voucher_Acc_ID As String = String.Empty
            Dim strDataEntry_By As String = String.Empty

            If Not drSource.IsNull("Voucher_Acc_ID") Then strVoucher_Acc_ID = drSource("Voucher_Acc_ID").ToString().Trim()
            If Not drSource.IsNull("Temp_Voucher_Acc_ID") Then strTemp_Voucher_Acc_ID = drSource("Temp_Voucher_Acc_ID").ToString().Trim()
            If Not drSource.IsNull("DataEntry_By") Then strDataEntry_By = drSource("DataEntry_By").ToString().Trim()

            Dim dtmConfirmed_Dtm As Nullable(Of DateTime) = Nothing
            If Not drSource.IsNull("Confirmed_Dtm") Then dtmConfirmed_Dtm = Convert.ToDateTime(drSource("Confirmed_Dtm"))

            Dim strVoid_Transaction_ID As String = String.Empty
            Dim dtmVoid_Dtm As Nullable(Of DateTime) = Nothing
            Dim strVoid_Remark As String = String.Empty
            Dim strVoid_By As String = String.Empty
            Dim strVoid_By_DataEntry As String = String.Empty

            If Not drSource.IsNull("Void_Transaction_ID") Then strVoid_Transaction_ID = drSource("Void_Transaction_ID").ToString().Trim()
            If Not drSource.IsNull("Void_Dtm") Then dtmVoid_Dtm = Convert.ToDateTime(drSource("Void_Dtm"))
            If Not drSource.IsNull("Void_Remark") Then strVoid_Remark = drSource("Void_Remark").ToString().Trim()
            If Not drSource.IsNull("Void_By") Then strVoid_By = drSource("Void_By").ToString().Trim()
            If Not drSource.IsNull("Void_By_DataEntry") Then strVoid_By_DataEntry = drSource("Void_By_DataEntry").ToString().Trim()

            Dim dblClaim_Amount As Nullable(Of Double) = Nothing
            If Not drSource.IsNull("Claim_Amount") Then dblClaim_Amount = CDbl(drSource("Claim_Amount"))

            Dim strVoid_By_HCVU As String = String.Empty
            If Not drSource.IsNull("Void_By_HCVU") Then strVoid_By_HCVU = drSource("Void_By_HCVU").ToString().Trim()

            Dim strSpecial_Acc_ID As String = String.Empty
            Dim strInvalid_Acc_ID As String = String.Empty
            Dim strAuthorised_status As String = String.Empty

            If Not drSource.IsNull("Special_Acc_ID") Then strSpecial_Acc_ID = drSource("Special_Acc_ID").ToString().Trim()
            If Not drSource.IsNull("Invalid_Acc_ID") Then strInvalid_Acc_ID = drSource("Invalid_Acc_ID").ToString().Trim()
            If Not drSource.IsNull("Authorised_status") Then strAuthorised_status = drSource("Authorised_status").ToString().Trim()

            Dim strPractice_Name As String = String.Empty
            Dim strPractice_Name_Chi As String = String.Empty
            Dim strCreateBySmartID As String = String.Empty

            If Not drSource.IsNull("Practice_Name") Then strPractice_Name = drSource("Practice_Name").ToString().Trim()
            If Not drSource.IsNull("Practice_Name_Chi") Then strPractice_Name_Chi = drSource("Practice_Name_Chi").ToString().Trim()
            If Not drSource.IsNull("Create_By_SmartID") Then strCreateBySmartID = drSource("Create_By_SmartID").ToString().Trim()

            Dim strCreationReason As String = String.Empty
            Dim strCreationRemarks As String = String.Empty
            Dim strPaymentMethod As String = String.Empty
            Dim strPaymentRemark As String = String.Empty
            Dim strOverrideReason As String = String.Empty
            Dim strApprovalBy As String = String.Empty
            Dim dtmApprovalDate As Nullable(Of DateTime) = Nothing
            Dim strRejectBy As String = String.Empty
            Dim dtmRejectDate As Nullable(Of DateTime) = Nothing
            Dim byteManualReimburseTSMP As Byte() = Nothing
            Dim strManualReimburse As String = String.Empty

            If Not drSource.IsNull("Creation_Reason") Then strCreationReason = drSource("Creation_Reason").ToString().Trim()
            If Not drSource.IsNull("Creation_Remark") Then strCreationRemarks = drSource("Creation_Remark").ToString().Trim()
            If Not drSource.IsNull("Payment_Method") Then strPaymentMethod = drSource("Payment_Method").ToString().Trim()
            If Not drSource.IsNull("Payment_Remark") Then strPaymentMethod = drSource("Payment_Remark").ToString().Trim()
            If Not drSource.IsNull("Override_Reason") Then strOverrideReason = drSource("Override_Reason").ToString().Trim()
            If Not drSource.IsNull("Approval_By") Then strApprovalBy = drSource("Approval_By").ToString().Trim()
            If Not drSource.IsNull("Approval_Dtm") Then dtmApprovalDate = Convert.ToDateTime(drSource("Approval_Dtm"))
            If Not drSource.IsNull("Reject_By") Then strRejectBy = drSource("Reject_By").ToString().Trim()
            If Not drSource.IsNull("Reject_Dtm") Then dtmRejectDate = Convert.ToDateTime(drSource("Reject_Dtm"))
            If Not drSource.IsNull("Manual_Reimburse_TSMP") Then byteManualReimburseTSMP = CType(drSource("Manual_Reimburse_TSMP"), Byte())
            If Not drSource.IsNull("Manual_Reimburse") Then strManualReimburse = drSource("Manual_Reimburse").ToString.Trim

            Dim strCategoryCode As String = String.Empty
            If Not drSource.IsNull("Category_Code") Then strCategoryCode = drSource("Category_Code").ToString().Trim()

            Dim strHighRisk As String = String.Empty
            If Not drSource.IsNull("High_Risk") Then strHighRisk = drSource("High_Risk").ToString().Trim()

            Dim strEHSVaccineResult As String = String.Empty
            If Not drSource.IsNull("EHS_Vaccine_Ref") Then strEHSVaccineResult = drSource("EHS_Vaccine_Ref").ToString().Trim()

            Dim strHAVaccineResult As String = String.Empty
            If Not drSource.IsNull("HA_Vaccine_Ref") Then strHAVaccineResult = drSource("HA_Vaccine_Ref").ToString().Trim()

            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Dim strHAVaccineRefStatus As String = String.Empty
            If Not drSource.IsNull("Ext_Ref_Status") Then strHAVaccineRefStatus = drSource("Ext_Ref_Status").ToString().Trim()

            Dim strDHVaccineResult As String = String.Empty
            If Not drSource.IsNull("DH_Vaccine_Ref") Then strDHVaccineResult = drSource("DH_Vaccine_Ref").ToString().Trim()

            Dim strDHVaccineRefStatus As String = String.Empty
            If Not drSource.IsNull("DH_Vaccine_Ref_Status") Then strDHVaccineRefStatus = drSource("DH_Vaccine_Ref_Status").ToString().Trim()
            ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Dim strHKICSymbol As String = String.Empty
            If Not drSource.IsNull("HKIC_Symbol") Then strHKICSymbol = drSource("HKIC_Symbol").ToString().Trim()

            Dim strOCSSSRefStatus As String = String.Empty
            If Not drSource.IsNull("OCSSS_Ref_Status") Then strOCSSSRefStatus = drSource("OCSSS_Ref_Status").ToString().Trim()
            ' CRE17-010 (OCSSS integration) [End][Chris YIM]

            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Dim strSmartIDVer As String = String.Empty
            If Not drSource.IsNull("SmartID_Ver") Then strSmartIDVer = drSource("SmartID_Ver").ToString().Trim()
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Dim strDHCService As String = String.Empty
            If Not drSource.IsNull("DHC_Service") Then strDHCService = drSource("DHC_Service").ToString().Trim()
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ' Add [strDHCService]
            Dim udtEHSTransactionModel As New EHSTransactionModel( _
                CStr(drSource("Transaction_ID")), _
                Convert.ToDateTime(drSource("Transaction_Dtm")), _
                strVoucher_Acc_ID, _
                strTemp_Voucher_Acc_ID, _
                CStr(drSource("Scheme_Code")), _
                CStr(drSource("Service_Receive_Dtm")), _
                CStr(drSource("Service_Type")), _
                CInt(drSource("Voucher_Before_Claim")), _
                CInt(drSource("Voucher_After_Claim")), _
                CStr(drSource("SP_ID")), _
                CInt(drSource("Practice_Display_Seq")), _
                CInt(drSource("Bank_Acc_Display_Seq")), _
                CStr(drSource("Bank_Account_No")), _
                CStr(drSource("Bank_Acc_Holder")), _
                strDataEntry_By, _
                dtmConfirmed_Dtm, _
                CStr(drSource("Consent_Form_Printed")), _
                CStr(drSource("Record_Status")), _
                strVoid_Transaction_ID, _
                dtmVoid_Dtm, _
                strVoid_Remark, _
                strVoid_By, _
                strVoid_By_DataEntry, _
                Convert.ToDateTime(drSource("Create_Dtm")), _
                CStr(drSource("Create_By")), _
                Convert.ToDateTime(drSource("Update_Dtm")), _
                CStr(drSource("Update_By")), _
                CType(drSource("TSMP"), Byte()), _
                strVoid_By_HCVU, _
                dblClaim_Amount, _
                CStr(drSource("Doc_Code")), _
                strSpecial_Acc_ID, _
                strInvalid_Acc_ID, _
                strAuthorised_status, _
                strPractice_Name, _
                strPractice_Name_Chi, _
                strCreateBySmartID, _
                strCreationReason, _
                strCreationRemarks, _
                strPaymentMethod, _
                strPaymentRemark, _
                strOverrideReason, _
                strApprovalBy, _
                dtmApprovalDate, _
                strRejectBy, _
                dtmRejectDate, _
                byteManualReimburseTSMP, _
                strManualReimburse, _
                strCategoryCode, _
                strHighRisk, _
                strEHSVaccineResult, _
                strHAVaccineResult, _
                strHAVaccineRefStatus, _
                strDHVaccineResult, _
                strDHVaccineRefStatus, _
                strHKICSymbol, _
                strOCSSSRefStatus, _
                strSmartIDVer, _
                strDHCService _
                )
            ' CRE19-006 (DHC) [End][Winnie]

            Return udtEHSTransactionModel

        End Function

        Private Function getTransactionInvalidation(ByVal strTranID As String, Optional ByVal udtDB As Database = Nothing) As TransactionInvalidationModel

            'Dim udtTransactionRemarkModelList As New TransactionRemarkModelCollection()
            Dim udtTransactionRemarkModel As TransactionInvalidationModel = Nothing
            If udtDB Is Nothing Then udtDB = New Database()
            Dim dt As New DataTable()

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Transaction_ID", TransactionInvalidationModel.Transaction_ID_DataType, TransactionInvalidationModel.Transaction_ID_DataSize, strTranID)}
                udtDB.RunProc("proc_TransactionInvalidation_get_ByTransID", prams, dt)

                If dt.Rows.Count > 0 Then
                    udtTransactionRemarkModel = Me.FillTransactionInvalidation(dt.Rows(0))
                    'For Each dr As DataRow In dt.Rows
                    'udtTransactionRemarkModel = Me.FillTransactionRemark(dr)
                    'udtTransactionRemarkModelList.Add(udtTransactionRemarkModel)
                    'Next
                End If

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return udtTransactionRemarkModel

        End Function

        Private Function FillTransactionInvalidation(ByVal drSource As DataRow) As TransactionInvalidationModel

            Dim strTransactionID As String = String.Empty
            Dim strRecordStatus As String = String.Empty
            Dim strInvalidationType As String = String.Empty
            Dim strInvalidationRemark As String = String.Empty
            Dim strCreateBy As String = String.Empty
            Dim strUpdateBy As String = String.Empty

            If Not drSource.IsNull("Transaction_ID") Then strTransactionID = drSource("Transaction_ID").ToString().Trim()
            If Not drSource.IsNull("Invalidation_Type") Then strInvalidationType = drSource("Invalidation_Type").ToString().Trim()
            If Not drSource.IsNull("Record_Status") Then strRecordStatus = drSource("Record_Status").ToString().Trim()
            If Not drSource.IsNull("Invalidation_Remark") Then strInvalidationRemark = drSource("Invalidation_Remark").ToString().Trim()
            If Not drSource.IsNull("Create_By") Then strCreateBy = drSource("Create_By").ToString().Trim()
            If Not drSource.IsNull("Update_By") Then strUpdateBy = drSource("Update_By").ToString().Trim()

            Dim udtTransactionRemarkModel As New TransactionInvalidationModel( _
            strTransactionID, _
            strInvalidationType, _
            strRecordStatus, _
            strInvalidationRemark, _
            Convert.ToDateTime(drSource("Create_Dtm")), _
            strCreateBy, _
            Convert.ToDateTime(drSource("Update_Dtm")), _
            strUpdateBy, _
            CType(drSource("TSMP"), Byte()))

            Return udtTransactionRemarkModel

        End Function

        ''' <summary>
        ''' Retrieve Transaction Details By Transaction ID
        ''' </summary>
        ''' <param name="strTranID"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getTransactionDetail(ByVal strTranID As String, Optional ByVal udtDB As Database = Nothing) As TransactionDetailModelCollection

            Dim udtTransactionDetailList As New TransactionDetailModelCollection()
            Dim udtTranDetailModel As TransactionDetailModel = Nothing

            If udtDB Is Nothing Then udtDB = New Database()
            Dim dt As New DataTable()

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@tran_id", EHSTransactionModel.Transaction_ID_DataType, EHSTransactionModel.Transaction_ID_DataSize, strTranID)}
                udtDB.RunProc("proc_TransactionDetail_get_byTranID", prams, dt)

                For Each dr As DataRow In dt.Rows
                    udtTranDetailModel = Me.FillTransactionDetail(dr)
                    udtTransactionDetailList.Add(udtTranDetailModel)
                Next

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return udtTransactionDetailList

        End Function

        Private Function FillTransactionDetail(ByVal drSource As DataRow) As TransactionDetailModel

            Dim strSubsidizeItemCode As String = String.Empty
            Dim strAvailableItemCode As String = String.Empty
            Dim strAvailableItemDesc As String = String.Empty
            Dim strAvailableItemDescChi As String = String.Empty
            Dim strAvailableItemDescCN As String = String.Empty
            Dim strRemark As String = String.Empty

            If Not drSource.IsNull("Subsidize_Item_Code") Then strSubsidizeItemCode = drSource("Subsidize_Item_Code").ToString().Trim()
            If Not drSource.IsNull("Available_Item_Code") Then strAvailableItemCode = drSource("Available_Item_Code").ToString().Trim()
            If Not drSource.IsNull("Available_Item_Desc") Then strAvailableItemDesc = drSource("Available_Item_Desc").ToString().Trim()
            If Not drSource.IsNull("Available_Item_Desc_Chi") Then strAvailableItemDescChi = drSource("Available_Item_Desc_Chi").ToString().Trim()
            If Not drSource.IsNull("Available_Item_Desc_CN") Then strAvailableItemDescCN = drSource("Available_Item_Desc_CN").ToString().Trim()
            If Not drSource.IsNull("Remark") Then strRemark = drSource("Remark").ToString().Trim()

            Dim intUnit As Nullable(Of Integer) = Nothing
            Dim dblPerUnitValue As Nullable(Of Double) = Nothing
            Dim dblTotalAmount As Nullable(Of Double) = Nothing
            'CRE13-019-02 Extend HCVS to China [Start][Karl]
            Dim dblExchangeRateValue As Nullable(Of Double) = Nothing
            Dim dblTotalAmountRMB As Nullable(Of Double) = Nothing
            If Not drSource.IsNull("ExchangeRate_Value") Then dblExchangeRateValue = CDbl(drSource("ExchangeRate_Value"))
            If Not drSource.IsNull("Total_Amount_RMB") Then dblTotalAmountRMB = CDbl(drSource("Total_Amount_RMB"))
            'CRE13-019-02 Extend HCVS to China [End][Karl]

            If Not drSource.IsNull("Unit") Then intUnit = CInt(drSource("Unit"))
            If Not drSource.IsNull("Per_Unit_Value") Then dblPerUnitValue = CDbl(drSource("Per_Unit_Value"))
            If Not drSource.IsNull("Total_Amount") Then dblTotalAmount = CDbl(drSource("Total_Amount"))

            'CRE13-019-02 Extend HCVS to China [Start][Karl]
            Dim udtTransactionDetailModel As New TransactionDetailModel( _
                drSource("Transaction_ID").ToString(), _
                drSource("Scheme_Code").ToString(), _
                CInt(drSource("Scheme_Seq")), _
                drSource("Subsidize_Code").ToString(), _
                strSubsidizeItemCode, _
                strAvailableItemCode, _
                intUnit, _
                dblPerUnitValue, _
                dblTotalAmount, _
                strRemark, _
                dblExchangeRateValue, _
                dblTotalAmountRMB, _
                strAvailableItemDesc, _
                strAvailableItemDescChi, _
                strAvailableItemDescCN)
            'CRE13-019-02 Extend HCVS to China [End][Karl]

            Return udtTransactionDetailModel

        End Function

        ''' <summary>
        ''' Retrieve Transaction Additional Fields By Transaction ID
        ''' </summary>
        ''' <param name="strTranID"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getTransactionAdditionalField(ByVal strTranID As String, Optional ByVal udtDB As Database = Nothing) As TransactionAdditionalFieldModelCollection

            Dim udtTransactionAdditionalFieldMList As New TransactionAdditionalFieldModelCollection()
            Dim udtTransactionAdditionalFieldModel As TransactionAdditionalFieldModel = Nothing

            If udtDB Is Nothing Then udtDB = New Database()
            Dim dt As New DataTable()

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@tran_id", EHSTransactionModel.Transaction_ID_DataType, EHSTransactionModel.Transaction_ID_DataSize, strTranID)}
                udtDB.RunProc("proc_TransactionAdditionalField_get_ByTranID", prams, dt)

                For Each dr As DataRow In dt.Rows

                    Dim strAdditionalFieldValueCode As String = String.Empty
                    Dim strAdditionalFieldValueDesc As String = Nothing

                    If Not dr.IsNull("AdditionalFieldValueCode") Then strAdditionalFieldValueCode = dr("AdditionalFieldValueCode").ToString().Trim()
                    If Not dr.IsNull("AdditionalFieldValueDesc") Then strAdditionalFieldValueDesc = dr("AdditionalFieldValueDesc").ToString().Trim()

                    udtTransactionAdditionalFieldModel = New TransactionAdditionalFieldModel( _
                        dr("Transaction_ID").ToString(), _
                        dr("Scheme_Code").ToString(), _
                        dr("Scheme_Seq").ToString(), _
                        dr("Subsidize_Code").ToString(), _
                        dr("AdditionalFieldID").ToString(), _
                        strAdditionalFieldValueCode, _
                        strAdditionalFieldValueDesc)

                    udtTransactionAdditionalFieldMList.Add(udtTransactionAdditionalFieldModel)
                Next

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return udtTransactionAdditionalFieldMList

        End Function

        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieve "School Code" at Transaction Additional Fields By Transaction ID
        ''' </summary>
        ''' <param name="strTranID"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetSchoolCodeByTranID(ByVal strTranID As String, Optional ByVal udtDB As Database = Nothing) As TransactionAdditionalFieldModel

            Dim udtTransactionAdditionalField As TransactionAdditionalFieldModel = Nothing

            If udtDB Is Nothing Then udtDB = New Database()
            Dim dt As New DataTable()

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@TransactionID", EHSTransactionModel.Transaction_ID_DataType, EHSTransactionModel.Transaction_ID_DataSize, strTranID)}
                udtDB.RunProc("proc_TransactionAdditionalField_get_SchoolCode_ByTranID", prams, dt)

                If dt.Rows.Count = 0 Then Return Nothing

                Dim dr As DataRow = dt.Rows(0)

                udtTransactionAdditionalField = New TransactionAdditionalFieldModel( _
                                                dr("Transaction_ID").ToString(), _
                                                dr("Scheme_Code").ToString(), _
                                                dr("Scheme_Seq").ToString(), _
                                                dr("Subsidize_Code").ToString(), _
                                                dr("AdditionalFieldID").ToString(), _
                                                IIf(dr.IsNull("AdditionalFieldValueCode"), String.Empty, dr("AdditionalFieldValueCode").ToString().Trim()), _
                                                IIf(dr.IsNull("AdditionalFieldValueDesc"), String.Empty, dr("AdditionalFieldValueDesc").ToString().Trim())
                                                )

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return udtTransactionAdditionalField

        End Function
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        Private Function getTransactionWarningResults(ByVal udtEHSTransactionModel As EHSTransactionModel, Optional ByVal udtDB As Database = Nothing) As EHSClaim.EHSClaimBLL.EHSClaimBLL.RuleResultList
            Dim udtTransactionWarningResult As EHSClaim.EHSClaimBLL.EHSClaimBLL.RuleResult
            Dim dtWarningMessage As New DataTable
            Dim udtSystemMessage As SystemMessage
            Dim udtTransactionWarningResults As New EHSClaim.EHSClaimBLL.EHSClaimBLL.RuleResultList

            If udtDB Is Nothing Then udtDB = New Database()

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@Transaction_ID", SqlDbType.Char, 20, udtEHSTransactionModel.TransactionID)}

                udtDB.RunProc("proc_ManualReimbursedClaimWarning_get_byTransactionID", prams, dtWarningMessage)

                For Each drWarningMessage As DataRow In dtWarningMessage.Rows
                    udtSystemMessage = New SystemMessage(drWarningMessage.Item("Function_Code").ToString().Trim(), _
                                                                               drWarningMessage.Item("Severity_Code").ToString().Trim(), _
                                                                               drWarningMessage.Item("Message_Code").ToString().Trim() _
                                                                           )

                    udtTransactionWarningResult = New EHSClaim.EHSClaimBLL.EHSClaimBLL.RuleResult(udtSystemMessage, drWarningMessage.Item("Message_Variable_Name").ToString().Trim(), drWarningMessage.Item("Message_Variable_Value").ToString().Trim(), drWarningMessage.Item("Message_Variable_Name_Chi").ToString().Trim(), drWarningMessage.Item("Message_Variable_Value_Chi").ToString().Trim())
                    udtTransactionWarningResults.RuleResults.Add(udtTransactionWarningResult)
                Next

                Return udtTransactionWarningResults
            Catch ex As Exception
                Throw
            End Try
        End Function

        Private Function getTransactionManualReimbursementRowCount(ByVal strUserID As String, ByVal strRecordStatus As String, Optional ByVal udtDB As Database = Nothing) As Integer
            Dim intRes As Integer = 0
            Dim dt As New DataTable

            If udtDB Is Nothing Then udtDB = New Database()

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@User_ID", SqlDbType.Char, 20, strUserID), _
                                                udtDB.MakeInParam("@Record_Status", SqlDbType.Char, 1, strRecordStatus)}

                udtDB.RunProc("proc_VoucherTransactionManualReimbursementRowCount_byStatus", prams, dt)

                If Not dt.Rows.Count = 0 Then
                    intRes = CInt(dt.Rows(0)("NoOfTran"))
                End If

                Return intRes
            Catch ex As Exception
                Throw
            End Try
        End Function

#End Region

#Region "Private Insert Function"

        Private Sub InsertVoucherTransaction(ByRef udtDB As Database, ByRef udtEHSTransaction As EHSTransactionModel)
            With udtEHSTransaction
                Dim strVoucherAccID As String = String.Empty
                Dim strTempVoucherAccID As String = String.Empty
                Dim strDataEntryBy As String = String.Empty
                Dim strCreateBySmartID As String = "N"

                If Not .VoucherAccID Is Nothing Then strVoucherAccID = .VoucherAccID.Trim()
                If Not .TempVoucherAccID Is Nothing Then strTempVoucherAccID = .TempVoucherAccID.Trim()
                If Not .DataEntryBy Is Nothing Then strDataEntryBy = .DataEntryBy.Trim()
                If .CreateBySmartID Then strCreateBySmartID = "Y"

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Dim objSmartIDVer As Object = DBNull.Value

                If .CreateBySmartID Then
                    If .SmartIDVer <> String.Empty Then
                        objSmartIDVer = .SmartIDVer.Trim()
                    End If
                End If
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                ' CRE19-006 (DHC) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Add [DHC_Service]
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Transaction_ID", EHSTransactionModel.Transaction_ID_DataType, EHSTransactionModel.Transaction_ID_DataSize, .TransactionID), _
                    udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVoucherAccID), _
                    udtDB.MakeInParam("@Temp_Voucher_Acc_ID", SqlDbType.Char, 15, strTempVoucherAccID), _
                    udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, .SchemeCode.Trim()), _
                    udtDB.MakeInParam("@Service_Receive_Dtm", SqlDbType.DateTime, 8, .ServiceDate), _
                    udtDB.MakeInParam("@Service_Type", SqlDbType.Char, 5, .ServiceType.Trim()), _
                    udtDB.MakeInParam("@Voucher_Before_Claim", SqlDbType.SmallInt, 2, .VoucherBeforeRedeem), _
                    udtDB.MakeInParam("@Voucher_After_Claim", SqlDbType.SmallInt, 2, .VoucherAfterRedeem), _
                    udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, .ServiceProviderID), _
                    udtDB.MakeInParam("@Practice_Display_Seq", SqlDbType.SmallInt, 2, .PracticeID), _
                    udtDB.MakeInParam("@Bank_Acc_Display_Seq", SqlDbType.SmallInt, 2, .BankAccountID), _
                    udtDB.MakeInParam("@Bank_Account_No", SqlDbType.VarChar, 30, .BankAccountNo), _
                    udtDB.MakeInParam("@Bank_Acc_Holder", SqlDbType.NVarChar, 100, .BankAccountOwner), _
                    udtDB.MakeInParam("@DataEntry_By", SqlDbType.VarChar, 20, strDataEntryBy), _
                    udtDB.MakeInParam("@Consent_Form_Printed", SqlDbType.Char, 1, IIf(.PrintedConsentForm, strYES, strNO)), _
                    udtDB.MakeInParam("@Record_Status", SqlDbType.Char, 1, .RecordStatus), _
                    udtDB.MakeInParam("@TSWProgram", SqlDbType.Char, 1, IIf(.TSWCase, strYES, strNO)), _
                    udtDB.MakeInParam("@Create_By", SqlDbType.VarChar, 20, .CreateBy), _
                    udtDB.MakeInParam("@Claim_Amount", SqlDbType.VarChar, 20, IIf(.ClaimAmount.HasValue, .ClaimAmount, DBNull.Value)), _
                    udtDB.MakeInParam("@SourceApp", SqlDbType.VarChar, 10, .SourceApp), _
                    udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, .DocCode), _
                    udtDB.MakeInParam("@PreSchool", SqlDbType.Char, 1, .PreSchool), _
                    udtDB.MakeInParam("@Create_By_SmartID", SqlDbType.Char, 1, strCreateBySmartID), _
                    udtDB.MakeInParam("@Manual_Reimburse", SqlDbType.Char, 1, IIf(.ManualReimburse, strYES, strNO)), _
                    udtDB.MakeInParam("@HA_Vaccine_Ref_Status", SqlDbType.VarChar, 10, IIf(.HAVaccineRefStatus Is Nothing, DBNull.Value, .HAVaccineRefStatus)), _
                    udtDB.MakeInParam("@IsUpload", SqlDbType.VarChar, 1, .IsUpload), _
                    udtDB.MakeInParam("@Category_Code", SqlDbType.VarChar, 10, IIf(String.IsNullOrEmpty(.CategoryCode), DBNull.Value, .CategoryCode)), _
                    udtDB.MakeInParam("@High_Risk", SqlDbType.Char, 1, IIf(String.IsNullOrEmpty(.HighRisk), DBNull.Value, .HighRisk)), _
                    udtDB.MakeInParam("@EHS_Vaccine_Ref", SqlDbType.VarChar, 2, IIf(String.IsNullOrEmpty(.EHSVaccineResult), DBNull.Value, .EHSVaccineResult)), _
                    udtDB.MakeInParam("@HA_Vaccine_Ref", SqlDbType.VarChar, 2, IIf(String.IsNullOrEmpty(.HAVaccineResult), DBNull.Value, .HAVaccineResult)), _
                    udtDB.MakeInParam("@DH_Vaccine_Ref", SqlDbType.VarChar, 2, IIf(String.IsNullOrEmpty(.DHVaccineResult), DBNull.Value, .DHVaccineResult)), _
                    udtDB.MakeInParam("@DH_Vaccine_Ref_Status", SqlDbType.VarChar, 10, IIf(String.IsNullOrEmpty(.DHVaccineRefStatus), DBNull.Value, .DHVaccineRefStatus)), _
                    udtDB.MakeInParam("@HKIC_Symbol", SqlDbType.Char, 1, IIf(String.IsNullOrEmpty(.HKICSymbol), DBNull.Value, .HKICSymbol)), _
                    udtDB.MakeInParam("@OCSSS_Ref_Status", SqlDbType.Char, 1, IIf(String.IsNullOrEmpty(.OCSSSRefStatus), DBNull.Value, .OCSSSRefStatus)), _
                    udtDB.MakeInParam("@SmartID_Ver", SqlDbType.VarChar, 5, objSmartIDVer), _
                    udtDB.MakeInParam("@DHC_Service", SqlDbType.Char, 1, IIf(String.IsNullOrEmpty(.DHCService), DBNull.Value, .DHCService)) _
                }
                ' CRE19-006 (DHC) [End][Winnie]

                udtDB.RunProc("proc_VoucherTransaction_add", prams)

            End With
        End Sub

        Private Sub InsertTransactionDetails(ByRef udtDB As Database, ByVal strTranID As String, ByRef udtTransactionDetails As TransactionDetailModelCollection)

            For Each udtTransactionDetail As TransactionDetailModel In udtTransactionDetails

                With udtTransactionDetail

                    ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                    Dim prams() As SqlParameter = { _
                        udtDB.MakeInParam("@Transaction_ID", EHSTransactionModel.Transaction_ID_DataType, EHSTransactionModel.Transaction_ID_DataSize, strTranID), _
                        udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, .SchemeCode.Trim()), _
                        udtDB.MakeInParam("@Scheme_Seq", SqlDbType.SmallInt, 2, .SchemeSeq), _
                        udtDB.MakeInParam("@Subsidize_Code", SqlDbType.Char, 10, .SubsidizeCode), _
                        udtDB.MakeInParam("@Subsidize_Item_Code", SqlDbType.Char, 10, .SubsidizeItemCode), _
                        udtDB.MakeInParam("@Available_item_Code", SqlDbType.Char, 20, .AvailableItemCode), _
                        udtDB.MakeInParam("@Unit", SqlDbType.Int, 2, IIf(.Unit.HasValue, .Unit, DBNull.Value)), _
                        udtDB.MakeInParam("@Per_Unit_Value", SqlDbType.Money, 4, IIf(.PerUnitValue.HasValue, .PerUnitValue, DBNull.Value)), _
                        udtDB.MakeInParam("@Remark", SqlDbType.NVarChar, 255, .Remark), _
                        udtDB.MakeInParam("@ExchangeRate_Value", SqlDbType.Decimal, 9, IIf(.ExchangeRate_Value.HasValue, .ExchangeRate_Value, DBNull.Value)), _
                        udtDB.MakeInParam("@Total_Amount_RMB", SqlDbType.Money, 4, IIf(.TotalAmountRMB.HasValue, .TotalAmountRMB, DBNull.Value))}
                    ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

                    udtDB.RunProc("proc_TransactionDetail_add", prams)
                End With
            Next
        End Sub

        Private Sub InsertTransactionAdditionalFields(ByRef udtDB As Database, ByVal strTranID As String, ByRef udtEHSTransaction As EHSTransactionModel)
            For Each udtTransactionAdditionalField As TransactionAdditionalFieldModel In udtEHSTransaction.TransactionAdditionFields
                With udtTransactionAdditionalField
                    'CRE16-003 (Disallow input Chinese Chars) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    'Allow Null in AdditionalFieldValueDesc
                    Dim prams() As SqlParameter = { _
                        udtDB.MakeInParam("@Transaction_ID", EHSTransactionModel.Transaction_ID_DataType, EHSTransactionModel.Transaction_ID_DataSize, strTranID), _
                        udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, .SchemeCode.Trim()), _
                        udtDB.MakeInParam("@Scheme_Seq", SqlDbType.SmallInt, 2, .SchemeSeq), _
                        udtDB.MakeInParam("@Subsidize_Code", SqlDbType.Char, 10, .SubsidizeCode), _
                        udtDB.MakeInParam("@AdditionalFieldID", SqlDbType.VarChar, 25, .AdditionalFieldID), _
                        udtDB.MakeInParam("@AdditionalFieldValueCode", SqlDbType.VarChar, 50, .AdditionalFieldValueCode), _
                        udtDB.MakeInParam("@AdditionalFieldValueDesc", SqlDbType.NVarChar, 255, IIf(.AdditionalFieldValueDesc Is Nothing, DBNull.Value, .AdditionalFieldValueDesc)), _
                        udtDB.MakeInParam("@Update_by", SqlDbType.VarChar, 20, udtEHSTransaction.UpdateBy)}
                    'CRE16-003 (Disallow input Chinese Chars) [End][Chris YIM]
                    udtDB.RunProc("proc_TransactionAdditionalField_add", prams)
                End With
            Next
        End Sub

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Remove transaction additional field for transaction update
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <param name="strTranID"></param>
        ''' <remarks></remarks>
        Private Sub RemoveTransactionAdditionalFields(ByRef udtDB As Database, ByVal strTranID As String)
            Dim prams() As SqlParameter = { _
                                    udtDB.MakeInParam("@Transaction_ID", EHSTransactionModel.Transaction_ID_DataType, EHSTransactionModel.Transaction_ID_DataSize, strTranID) _
                                   }

            udtDB.RunProc("proc_TransactionAdditionalField_del", prams)
        End Sub
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        Private Function InsertTransactionWarningResults(ByVal udtEHSTransactionModel As EHSTransactionModel, Optional ByRef udtDB As Common.DataAccess.Database = Nothing) As Boolean

            If IsNothing(udtDB) Then
                udtDB = New Common.DataAccess.Database
            End If

            Try
                'udtDB.BeginTransaction()
                Dim intDisplaySeq As Int32 = 0
                For Each udtWarningResult As EHSClaim.EHSClaimBLL.EHSClaimBLL.RuleResult In udtEHSTransactionModel.WarningMessage.RuleResults
                    intDisplaySeq = intDisplaySeq + 1
                    InsertTransactionWarningResult(udtEHSTransactionModel, udtWarningResult, udtDB, intDisplaySeq)
                Next

                'udtDB.CommitTransaction()
            Catch ex As Exception
                'udtDB.RollBackTranscation()
                Throw
            End Try

        End Function

        Private Function InsertTransactionWarningResult(ByVal udtEHSTransactionModel As EHSTransactionModel, ByVal udtWarningResult As EHSClaim.EHSClaimBLL.EHSClaimBLL.RuleResult, ByVal udtDB As Common.DataAccess.Database, ByVal intDisplaySeq As Int32) As Boolean

            Try
                Dim prams() As SqlParameter = { _
                                                                 udtDB.MakeInParam("@Transaction_ID", SqlDbType.Char, 20, udtEHSTransactionModel.TransactionID), _
                                                                 udtDB.MakeInParam("@Display_Seq", SqlDbType.SmallInt, 2, intDisplaySeq), _
                                                                 udtDB.MakeInParam("@Function_Code", SqlDbType.Char, 6, udtWarningResult.ErrorMessage.FunctionCode), _
                                                                 udtDB.MakeInParam("@Severity_Code", SqlDbType.Char, 1, udtWarningResult.ErrorMessage.SeverityCode), _
                                                                 udtDB.MakeInParam("@Message_Code", SqlDbType.Char, 5, udtWarningResult.ErrorMessage.MessageCode), _
                                                                 udtDB.MakeInParam("@Message_Variable_Name", SqlDbType.VarChar, 80, IIf(udtWarningResult.MessageVariableName Is Nothing, DBNull.Value, udtWarningResult.MessageVariableName)), _
                                                                 udtDB.MakeInParam("@Message_Variable_Value", SqlDbType.NVarChar, 255, IIf(udtWarningResult.MessageVariableValue Is Nothing, DBNull.Value, udtWarningResult.MessageVariableValue)), _
                                                                 udtDB.MakeInParam("@Message_Variable_Name_Chi", SqlDbType.VarChar, 80, IIf(udtWarningResult.MessageVariableNameChi Is Nothing, DBNull.Value, udtWarningResult.MessageVariableNameChi)), _
                                                                 udtDB.MakeInParam("@Message_Variable_Value_Chi", SqlDbType.NVarChar, 255, IIf(udtWarningResult.MessageVariableValueChi Is Nothing, DBNull.Value, udtWarningResult.MessageVariableValueChi)) _
                                              }

                udtDB.RunProc("proc_ManualReimbursedClaimWarning_add", prams)

                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
                Return False
            End Try
        End Function

        Private Function InsertManaulReimburse(ByVal udtEHSTransactionModel As EHSTransactionModel, ByRef udtDB As Database)

            Dim objCreationReason As Object = DBNull.Value
            Dim objCreationRemarks As Object = DBNull.Value
            Dim objOverrideReason As Object = DBNull.Value
            Dim objPaymentMethod As Object = DBNull.Value
            Dim objPaymentRemarks As Object = DBNull.Value

            With udtEHSTransactionModel
                If Not .CreationReason.Trim.Equals(String.Empty) Then objCreationReason = .CreationReason
                If Not .CreationRemarks.Trim.Equals(String.Empty) Then objCreationRemarks = .CreationRemarks
                If Not .OverrideReason.Trim.Equals(String.Empty) Then objOverrideReason = .OverrideReason
                If Not .PaymentMethod.Trim.Equals(String.Empty) Then objPaymentMethod = .PaymentMethod
                If Not .PaymentRemarks.Trim.Equals(String.Empty) Then objPaymentRemarks = .PaymentRemarks
            End With

            Try
                Dim prams() As SqlParameter = { _
                                                  udtDB.MakeInParam("@Transaction_ID", SqlDbType.Char, 20, udtEHSTransactionModel.TransactionID), _
                                                  udtDB.MakeInParam("@Creation_Reason", SqlDbType.Char, 5, objCreationReason), _
                                                  udtDB.MakeInParam("@Creation_Remark", SqlDbType.VarChar, 255, objCreationRemarks), _
                                                  udtDB.MakeInParam("@Override_Reason", SqlDbType.VarChar, 255, objOverrideReason), _
                                                  udtDB.MakeInParam("@Payment_Method", SqlDbType.Char, 5, objPaymentMethod), _
                                                  udtDB.MakeInParam("@Payment_Remark", SqlDbType.VarChar, 255, objPaymentRemarks) _
                                              }

                udtDB.RunProc("proc_ManualReimbursement_add", prams)

                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
                Return False
            End Try
        End Function

        ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Private Sub InsertManaulReimburseWithApproval(ByVal udtEHSTransactionModel As EHSTransactionModel, ByRef udtDB As Database)

            Dim objCreationReason As Object = DBNull.Value
            Dim objCreationRemarks As Object = DBNull.Value
            Dim objOverrideReason As Object = DBNull.Value
            Dim objPaymentMethod As Object = DBNull.Value
            Dim objPaymentRemarks As Object = DBNull.Value
            Dim objApprovalBy As Object = DBNull.Value
            Dim objApprovalDtm As Object = DBNull.Value
            Dim objRejectBy As Object = DBNull.Value
            Dim objRejectDtm As Object = DBNull.Value

            With udtEHSTransactionModel
                If Not .CreationReason.Trim.Equals(String.Empty) Then objCreationReason = .CreationReason
                If Not .CreationRemarks.Trim.Equals(String.Empty) Then objCreationRemarks = .CreationRemarks
                If Not .OverrideReason.Trim.Equals(String.Empty) Then objOverrideReason = .OverrideReason
                If Not .PaymentMethod.Trim.Equals(String.Empty) Then objPaymentMethod = .PaymentMethod
                If Not .PaymentRemarks.Trim.Equals(String.Empty) Then objPaymentRemarks = .PaymentRemarks
                If Not .ApprovalBy.Trim.Equals(String.Empty) Then objApprovalBy = .ApprovalBy
                If .ApprovalDate.HasValue Then objApprovalDtm = .ApprovalDate.Value
                If Not .RejectBy.Trim.Equals(String.Empty) Then objRejectBy = .RejectBy
                If .RejectDate.HasValue Then objRejectDtm = .RejectDate.Value
            End With

            Dim prams() As SqlParameter = { _
                                              udtDB.MakeInParam("@Transaction_ID", SqlDbType.Char, 20, udtEHSTransactionModel.TransactionID), _
                                              udtDB.MakeInParam("@Creation_Reason", SqlDbType.Char, 5, objCreationReason), _
                                              udtDB.MakeInParam("@Creation_Remark", SqlDbType.VarChar, 255, objCreationRemarks), _
                                              udtDB.MakeInParam("@Override_Reason", SqlDbType.VarChar, 255, objOverrideReason), _
                                              udtDB.MakeInParam("@Payment_Method", SqlDbType.Char, 5, objPaymentMethod), _
                                              udtDB.MakeInParam("@Payment_Remark", SqlDbType.VarChar, 255, objPaymentRemarks), _
                                              udtDB.MakeInParam("@Approval_By", SqlDbType.VarChar, 20, objApprovalBy), _
                                              udtDB.MakeInParam("@Approval_Dtm", SqlDbType.DateTime, 8, objApprovalDtm), _
                                              udtDB.MakeInParam("@Reject_By", SqlDbType.VarChar, 20, objRejectBy), _
                                              udtDB.MakeInParam("@Reject_Dtm", SqlDbType.DateTime, 8, objRejectDtm) _
                                          }

            udtDB.RunProc("proc_ManualReimbursement_add_With_Approval", prams)

        End Sub
        ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]


#End Region

#Region "Private Update Function"

        Private Sub UpdateTransactionStatus(ByVal strTransactionID As String, ByVal strRecordStatus As String, ByVal strUpdateBy As String, ByVal dtmUpdateDtm As DateTime, ByVal byteTSMP As Byte(), Optional ByVal udtDB As Database = Nothing)

            If udtDB Is Nothing Then udtDB = New Database()

            Dim prams() As SqlParameter = {udtDB.MakeInParam("@transaction_id", SqlDbType.Char, 20, strTransactionID), _
                                                udtDB.MakeInParam("@record_status", SqlDbType.Char, 1, strRecordStatus), _
                                                udtDB.MakeInParam("@update_by", SqlDbType.VarChar, 20, strUpdateBy), _
                                                udtDB.MakeInParam("@update_dtm", SqlDbType.DateTime, 8, dtmUpdateDtm), _
                                                udtDB.MakeInParam("@tsmp", SqlDbType.Timestamp, 8, byteTSMP)}

            udtDB.RunProc("proc_VoucherTransaction_upd_status", prams)
        End Sub

        Private Sub UpdateTransactionStatus(ByVal udtEHSTransaction As EHSTransactionModel, ByVal strRecordStatus As String, ByVal strUpdateBy As String, ByVal dtmUpdateDtm As DateTime, Optional ByVal udtDB As Database = Nothing)
            Me.UpdateTransactionStatus(udtEHSTransaction.TransactionID, strRecordStatus, strUpdateBy, dtmUpdateDtm, udtEHSTransaction.TSMP, udtDB)

            If strRecordStatus = EHSTransactionModel.TransRecordStatusClass.Inactive _
                OrElse strRecordStatus = EHSTransactionModel.TransRecordStatusClass.Removed _
                OrElse strRecordStatus = EHSTransactionModel.TransRecordStatusClass.RejectedBySP Then

                Dim udtSubsidizeWriteOffBLL As New SubsidizeWriteOffBLL()

                Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSTransaction.EHSAcct.EHSPersonalInformationList.Filter(udtEHSTransaction.DocCode)

                'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                udtSubsidizeWriteOffBLL.UpdateWriteOff(udtEHSTransaction.ServiceDate, _
                                                       udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, _
                                                       udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, _
                                                       udtEHSPersonalInfo.DOD, udtEHSPersonalInfo.ExactDOD, _
                                                       udtEHSTransaction.SchemeCode, udtEHSTransaction.TransactionDetails(0).SubsidizeCode, _
                                                       eHASubsidizeWriteOff_CreateReason.TxRemoval, udtDB)
                'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

            End If

        End Sub

        Private Sub UpdateManualReimburseStatus(ByVal udtEHSTransactionModel As EHSTransactionModel, ByVal strRecordStatus As String, ByRef udtDB As Database)
            Dim objApprovalBy As Object = DBNull.Value
            Dim objApprovalDtm As Object = DBNull.Value
            Dim objRejectBy As Object = DBNull.Value
            Dim objRejectDtm As Object = DBNull.Value

            With udtEHSTransactionModel
                If Not .ApprovalBy.Trim.Equals(String.Empty) Then objApprovalBy = .ApprovalBy
                If .ApprovalDate.HasValue Then objApprovalDtm = .ApprovalDate.Value
                If Not .RejectBy.Trim.Equals(String.Empty) Then objRejectBy = .RejectBy
                If .RejectDate.HasValue Then objRejectDtm = .RejectDate.Value
            End With

            Dim prams() As SqlParameter = {udtDB.MakeInParam("@Transaction_ID", SqlDbType.Char, 20, udtEHSTransactionModel.TransactionID), _
                                           udtDB.MakeInParam("@Record_Status", SqlDbType.Char, 1, strRecordStatus), _
                                           udtDB.MakeInParam("@Approval_By", SqlDbType.VarChar, 20, objApprovalBy), _
                                           udtDB.MakeInParam("@Approval_Dtm", SqlDbType.DateTime, 8, objApprovalDtm), _
                                           udtDB.MakeInParam("@Reject_By", SqlDbType.VarChar, 20, objRejectBy), _
                                           udtDB.MakeInParam("@Reject_Dtm", SqlDbType.DateTime, 8, objRejectDtm), _
                                           udtDB.MakeInParam("@ManualReimbursement_tsmp", SqlDbType.Timestamp, 8, udtEHSTransactionModel.ManualReimburseTSMP) _
                                           }

            udtDB.RunProc("proc_ManualReimbursement_update", prams)
        End Sub


#End Region

#Region "Benefit"

        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------


        ''' <summary>
        ''' Retrieve the Total number of voucher granted
        ''' dtmServiceDate refer to the claim service date or current date
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="strSubsidizeCode"></param>
        ''' <param name="udtEHSPersonalInformation"></param>
        ''' <param name="dtmServiceDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getTotalGrantVoucher(ByVal enumMethod As GetTotalEntitlement, ByVal strSchemeCode As String, ByVal strSubsidizeCode As String, _
                                             ByVal udtEHSPersonalInformation As EHSAccount.EHSAccountModel.EHSPersonalInformationModel, _
                                             ByVal dtmServiceDate As Date, Optional ByVal strSpecificSchemeSeqOnly As String = Nothing) As VoucherDetailModelCollection

            Dim udtClaimRulesBLL As New ClaimRulesBLL()
            Dim udtSchemeClaimBLL As New SchemeClaimBLL()
            Dim udtVoucherDetailList As New VoucherDetailModelCollection

            ' Retrieve Scheme Claim & Specific SubsidizeGroup (For All SchemeSeq)
            Dim udtSchemeClaimList As SchemeClaimModelCollection = udtSchemeClaimBLL.getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSubsidizeCode(strSchemeCode, strSubsidizeCode)

            ' To Check Eligibility, Assume The last Date Expiry Dtm is used for calculation 
            For Each udtSchemeClaim As SchemeClaimModel In udtSchemeClaimList

                ' Scheme Claim is currently effective or passed
                If udtSchemeClaim.EffectiveDtm <= dtmServiceDate Then
                    If udtSchemeClaim.SubsidizeGroupClaimList.Count > 0 Then

                        For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtSchemeClaim.SubsidizeGroupClaimList

                            If String.IsNullOrEmpty(strSpecificSchemeSeqOnly) = True Or udtSubsidizeGroupClaim.SchemeSeq = CInt(strSpecificSchemeSeqOnly) Then

                                ' Future subsidize group claim is filtered
                                If udtSubsidizeGroupClaim.ClaimPeriodFrom > dtmServiceDate Then
                                    Continue For
                                End If

                                Dim udtVoucherDetail As New VoucherDetailModel
                                udtVoucherDetail.SchemeSeq = udtSubsidizeGroupClaim.SchemeSeq
                                udtVoucherDetail.PeriodStart = udtSubsidizeGroupClaim.ClaimPeriodFrom
                                udtVoucherDetail.PeriodEnd = udtSubsidizeGroupClaim.ClaimPeriodTo
                                udtVoucherDetail.Ceiling = udtSubsidizeGroupClaim.NumSubsidizeCeiling

                                ' Filter Future Date
                                Dim dtmCheckEligible As Nullable(Of DateTime) = Nothing

                                Select Case enumMethod
                                    Case GetTotalEntitlement.ByLastDayOfClaimPeriod
                                        dtmCheckEligible = udtSubsidizeGroupClaim.ClaimPeriodTo.AddDays(-1)

                                    Case GetTotalEntitlement.ByServiceDate
                                        dtmCheckEligible = udtSubsidizeGroupClaim.ClaimPeriodTo.AddDays(-1)

                                        If udtSubsidizeGroupClaim.ClaimPeriodFrom <= dtmServiceDate And udtSubsidizeGroupClaim.ClaimPeriodTo > dtmServiceDate Then
                                            dtmCheckEligible = dtmServiceDate
                                        End If
                                End Select

                                'Requirement of getting entitlement
                                '---------------------------------------
                                '1. Recipient is alive
                                '2. Recipient is eligible at that season 
                                If udtClaimRulesBLL.CheckEligibilityPerSubsidize(udtSubsidizeGroupClaim, udtEHSPersonalInformation, dtmCheckEligible, Nothing, True).IsEligible Then
                                    udtVoucherDetail.Entitlement = udtSubsidizeGroupClaim.NumSubsidize
                                Else
                                    udtVoucherDetail.Entitlement = 0
                                End If

                                udtVoucherDetailList.Add(udtVoucherDetail)

                            End If

                        Next

                    End If
                End If

            Next

            Return udtVoucherDetailList
        End Function
        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Function getTotalGrantVoucher(ByVal strSchemeCode As String, ByVal strSubsidizeCode As String, ByVal dtmServiceDate As Date)
            ' Use dtmServiceDate 

            Dim udtClaimRulesBLL As New ClaimRulesBLL()
            Dim udtSchemeClaimBLL As New SchemeClaimBLL()
            Dim dtmCurrentDate As Date = (New GeneralFunction).GetSystemDateTime.Date

            ' Retrieve Scheme Claim & Specific SubsidizeGroup (For All SchemeSeq)
            Dim udtSchemeClaimList As SchemeClaimModelCollection = udtSchemeClaimBLL.getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSubsidizeCode(strSchemeCode, strSubsidizeCode)

            Dim intTotalGrantVoucher As Integer = 0

            ' To Check Eligibility, Assume The last Date Expiry Dtm is used for calculation 
            For Each udtSchemeClaim As SchemeClaimModel In udtSchemeClaimList

                ' Scheme Claim is currently effective or passed
                If udtSchemeClaim.EffectiveDtm <= dtmServiceDate Then
                    If udtSchemeClaim.SubsidizeGroupClaimList.Count > 0 Then
                        For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtSchemeClaim.SubsidizeGroupClaimList

                            'Future subsidize group claim is filtered
                            If udtSubsidizeGroupClaim.ClaimPeriodFrom > dtmCurrentDate Then
                                Continue For
                            End If

                            intTotalGrantVoucher = intTotalGrantVoucher + udtSubsidizeGroupClaim.NumSubsidize

                        Next

                    End If
                End If
            Next

            Return intTotalGrantVoucher

        End Function
        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieve total number of voucher used
        ''' </summary>
        ''' <param name="strDocCode"></param>
        ''' <param name="strIdentityNum"></param>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="strSubsidizeCode"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getTotalUsedVoucher(ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal strSchemeCode As String, _
            ByVal strSubsidizeCode As String, Optional ByVal udtDB As Database = Nothing) As VoucherDetailModelCollection

            Dim udtVoucherDetailList As New VoucherDetailModelCollection
            Dim dicVoucherUsage As New Dictionary(Of Integer, Integer())

            strIdentityNum = Me._udtFormatter.formatDocumentIdentityNumber(strDocCode, strIdentityNum)

            If udtDB Is Nothing Then udtDB = New Database()

            'Dim intUsedVoucher As Integer = 0

            Dim udtTransactionDetailList As TransactionDetailModelCollection = Me.getTransactionDetailVoucherBySubsidizeCodeOnly(strDocCode, strIdentityNum, strSubsidizeCode, udtDB)

            For Each udtTranDetailModel As TransactionDetailModel In udtTransactionDetailList

                If udtTranDetailModel.Unit.HasValue Then
                    Dim intVoucherAmount As Integer = 0

                    If Not udtTranDetailModel.TotalAmount Is Nothing Then
                        intVoucherAmount = CInt(udtTranDetailModel.TotalAmount)
                    End If

                    ' CRE19-006 (DHC) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    If Not dicVoucherUsage.ContainsKey(udtTranDetailModel.SchemeSeq) Then
                        Dim arrUsed(3) As Integer
                        arrUsed(0) = 0
                        arrUsed(1) = 0
                        arrUsed(2) = 0

                        If udtTranDetailModel.SchemeCode.Trim = SchemeClaimModel.HCVS Then
                            arrUsed(0) = intVoucherAmount
                        End If

                        If udtTranDetailModel.SchemeCode.Trim = SchemeClaimModel.HCVSCHN Then
                            arrUsed(1) = intVoucherAmount
                        End If

                        If udtTranDetailModel.SchemeCode.Trim = SchemeClaimModel.HCVSDHC Then
                            arrUsed(2) = intVoucherAmount
                        End If

                        dicVoucherUsage.Add(udtTranDetailModel.SchemeSeq, arrUsed)

                    Else
                        Dim arrused() As Integer = dicVoucherUsage.Item(udtTranDetailModel.SchemeSeq)

                        If udtTranDetailModel.SchemeCode.Trim = SchemeClaimModel.HCVS Then
                            arrused(0) = arrused(0) + intVoucherAmount
                        End If

                        If udtTranDetailModel.SchemeCode.Trim = SchemeClaimModel.HCVSCHN Then
                            arrused(1) = arrused(1) + intVoucherAmount
                        End If

                        If udtTranDetailModel.SchemeCode.Trim = SchemeClaimModel.HCVSDHC Then
                            arrused(2) = arrused(2) + intVoucherAmount
                        End If

                        dicVoucherUsage.Remove(udtTranDetailModel.SchemeSeq)

                        dicVoucherUsage.Add(udtTranDetailModel.SchemeSeq, arrused)

                    End If

                End If

            Next

            For Each intkey As Integer In dicVoucherUsage.Keys
                Dim udtVoucherDetail As New VoucherDetailModel

                udtVoucherDetail.SchemeSeq = intkey

                Dim arrused() As Integer = dicVoucherUsage.Item(intkey)
                udtVoucherDetail.UsedByHCVS = arrused(0)
                udtVoucherDetail.UsedByHCVSCHN = arrused(1)
                udtVoucherDetail.UsedByHCVSDHC = arrused(2)

                udtVoucherDetail.TransactionDetails = udtTransactionDetailList.FilterBySchemeSeq(intkey, strSubsidizeCode)

                udtVoucherDetailList.Add(udtVoucherDetail)
            Next
            ' CRE19-006 (DHC) [End][Winnie]

            Return udtVoucherDetailList

        End Function
        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Function getUsedVoucherByRange(ByVal udtVoucherDetailList As VoucherDetailModelCollection, _
                                              ByVal intSchemeSeqStart As Nullable(Of Integer), _
                                              ByVal intSchemeSeqEnd As Nullable(Of Integer)) As VoucherDetailModelCollection
            Dim intUsedVoucher As Integer = 0

            Dim udtResVoucherDetailList As New VoucherDetailModelCollection

            For Each udtVoucherDetail As VoucherDetailModel In udtVoucherDetailList

                If Not intSchemeSeqStart.HasValue OrElse udtVoucherDetail.SchemeSeq >= intSchemeSeqStart.Value Then
                    If Not intSchemeSeqEnd.HasValue OrElse udtVoucherDetail.SchemeSeq <= intSchemeSeqEnd.Value Then

                        udtResVoucherDetailList.Add(udtVoucherDetail)

                    End If
                End If

            Next

            Return udtResVoucherDetailList
        End Function
        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Private Function getAvailableVoucher_CurrentSeason(ByVal dtmServiceDate As Date, _
                                                          ByVal udtEHSPersonalInformation As EHSAccount.EHSAccountModel.EHSPersonalInformationModel, _
                                                          ByVal strSchemeCode As String, _
                                                          ByVal strSubsidizeCode As String, _
                                                          ByVal enumUpdateDBWriteOff As EHSAccount.WriteOff, _
                                                          Optional ByVal udtDB As Database = Nothing) As VoucherInfoModel

            Dim udtVoucherDetailList As New VoucherDetailModelCollection
            Dim udtSubsidizeWriteOffBLL As New SubsidizeWriteOffBLL()
            Dim udtVoucherRefundBLL As New VoucherRefund.VoucherRefundBLL()
            Dim udtVoucherInfo As New VoucherInfoModel

            Dim dtmCurrent As Date = (New GeneralFunction).GetSystemDateTime

            'Get Total Entitlement
            udtVoucherDetailList.Merge(Me.getTotalGrantVoucher(GetTotalEntitlement.ByServiceDate, strSchemeCode, strSubsidizeCode, udtEHSPersonalInformation, dtmCurrent), _
                                       VoucherDetailModelCollection.VoucherDetailPart.Entitlement)

            'Get Total Used 
            udtVoucherDetailList.Merge(Me.getTotalUsedVoucher(udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.IdentityNum, strSchemeCode, strSubsidizeCode, udtDB), _
                                       VoucherDetailModelCollection.VoucherDetailPart.Used)

            'Get Total Write Off 
            udtVoucherDetailList.Merge(udtSubsidizeWriteOffBLL.GetTotalWriteOff(udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.IdentityNum, _
                                                                                       udtEHSPersonalInformation.DOB, udtEHSPersonalInformation.ExactDOB, _
                                                                                       udtEHSPersonalInformation.DOD, udtEHSPersonalInformation.ExactDOD, _
                                                                                       strSchemeCode, strSubsidizeCode, _
                                                                                       eHASubsidizeWriteOff_CreateReason.TxEnquiry, _
                                                                                       enumUpdateDBWriteOff, _
                                                                                       udtDB), _
                                       VoucherDetailModelCollection.VoucherDetailPart.WriteOff)

            'Get Total Refund ***
            udtVoucherDetailList.Merge(udtVoucherRefundBLL.getTotalRefundedVoucher(udtEHSPersonalInformation.IdentityNum, dtmCurrent),
                                       VoucherDetailModelCollection.VoucherDetailPart.Refund)

            udtVoucherInfo.ServiceDate = dtmServiceDate.Date
            udtVoucherInfo.CurrentDate = dtmCurrent.Date
            udtVoucherInfo.VoucherDetailList = udtVoucherDetailList

            'Return All Voucher Usage 
            Return udtVoucherInfo

        End Function
        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieve Total Number of Voucher Used
        ''' </summary>
        ''' <param name="dtmServiceDate"></param>
        ''' <param name="udtEHSPersonalInformation"></param>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="strSubsidizeCode"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' 
        Private Function getAvailableVoucher(ByVal dtmServiceDate As Date, _
                                            ByVal udtEHSPersonalInformation As EHSAccount.EHSAccountModel.EHSPersonalInformationModel, _
                                            ByVal strSchemeCode As String, _
                                            ByVal strSubsidizeCode As String, _
                                            ByVal enumUpdateDBWriteOff As EHSAccount.WriteOff, _
                                            ByVal udtDB As Database) As VoucherInfoModel

            Dim udtEHSClaimBLL As New EHSClaim.EHSClaimBLL.EHSClaimBLL()
            Dim udtCloneEHSPersonalInformation As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = udtEHSPersonalInformation.Clone

            'Handle EC to change "Date of Registration + Age" -> DOB = YEAR(Date of Registration) - Age 
            'Update processed DOB in EHSPersonalInformation Model
            If udtCloneEHSPersonalInformation.DocCode.Trim() = DocType.DocTypeModel.DocTypeCode.EC AndAlso udtCloneEHSPersonalInformation.ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                udtCloneEHSPersonalInformation.DOB = CType(udtCloneEHSPersonalInformation.ECDateOfRegistration, Date).AddYears(-udtCloneEHSPersonalInformation.ECAge)
                udtCloneEHSPersonalInformation.ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration
            End If

            Return getAvailableVoucher_CurrentSeason(dtmServiceDate, udtCloneEHSPersonalInformation, strSchemeCode, strSubsidizeCode, enumUpdateDBWriteOff, udtDB)

        End Function
        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

        ' CRE20-005 (Providing users' data in HCVS to eHR Patient Portal) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Function getAvailableVoucher(ByVal dtmServiceDate As Date, _
                                            ByVal udtSchemeClaimModel As SchemeClaimModel, _
                                            ByVal udtEHSPersonalInformation As EHSAccount.EHSAccountModel.EHSPersonalInformationModel, _
                                            ByVal enumUpdateDBWriteOff As EHSAccount.WriteOff, _
                                            Optional ByVal udtDB As Database = Nothing) As VoucherInfoModel

            Return Me.getAvailableVoucher(dtmServiceDate, _
                                          udtEHSPersonalInformation, _
                                          udtSchemeClaimModel.SchemeCode, _
                                          udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeCode, _
                                          enumUpdateDBWriteOff, _
                                          udtDB)

        End Function
        ' CRE20-005 (Providing users' data in HCVS to eHR Patient Portal) [End][Chris YIM]	

        ' CRE19-003 (Opt voucher capping) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Public Function getVoucherQuota(ByVal dtmServiceDate As Date, _
                                            ByVal udtEHSPersonalInformation As EHSAccount.EHSAccountModel.EHSPersonalInformationModel, _
                                            ByVal strSchemeCode As String, ByVal strSubsidizeCode As String, _
                                            ByVal strProfessionCode As String, _
                                            ByVal dtmClaimPeriodTo As DateTime?, _
                                            ByVal udtTransactionDetailList As TransactionDetailModelCollection, _
                                            Optional ByVal udtDB As Database = Nothing) As VoucherQuotaModel

            ' ----------------------------------------------------------------------------------------
            ' 1. Check Eligible Date
            ' 2. Get Profession Voucher Quota
            ' 3. Get Voucher Used in period by profession
            ' 4. Count Available Quota
            ' ----------------------------------------------------------------------------------------
            Dim udtVoucherQuotaModel As VoucherQuotaModel = Nothing

            ' 1. Check Eligible Date
            Dim dtmEligibleDate As Nullable(Of DateTime) = getEligibleDate(strSchemeCode, strSubsidizeCode, udtEHSPersonalInformation, dtmServiceDate)

            ' Not Eligible on Service date -> Skip
            If dtmEligibleDate Is Nothing Then
                Return udtVoucherQuotaModel
            End If

            ' 2. Get Effective profession Quota setting
            Dim udtProfVoucherQuotaBLL As New ProfessionVoucherQuotaBLL

            Dim udtProfVoucherQuota As ProfessionVoucherQuotaModel = udtProfVoucherQuotaBLL.GetProfessionVoucherQuota(strProfessionCode, dtmServiceDate)

            If Not udtProfVoucherQuota Is Nothing Then
                Dim intStartYear As Integer
                Dim intCompareYear As Integer

                If dtmEligibleDate.Value <= udtProfVoucherQuota.EffectiveDate Then
                    ' Eligible year on or before effective year -> Use Effective year                    
                    intCompareYear = Year(udtProfVoucherQuota.EffectiveDate)

                Else
                    ' Eligible year after effective year -> Use Eligible year
                    intCompareYear = dtmEligibleDate.Value.Year
                End If

                ' Find the nearest capping start year
                intStartYear = dtmServiceDate.Year - ((dtmServiceDate.Year - intCompareYear) Mod udtProfVoucherQuota.CumulativeYear)

                udtVoucherQuotaModel = New VoucherQuotaModel()

                With udtVoucherQuotaModel
                    .ProfCode = udtProfVoucherQuota.ServiceCategoryCode

                    ' Default: Period = year start end
                    ' e.g. PeriodStartDtm = 2019/1/1; PeriodEndDtm = 2020/12/31
                    .PeriodStartDtm = New Date(intStartYear, 1, 1)
                    .PeriodEndDtm = .PeriodStartDtm.AddYears(udtProfVoucherQuota.CumulativeYear).AddDays(-1)

                    If .PeriodStartDtm < udtProfVoucherQuota.EffectiveDate Then
                        .PeriodStartDtm = udtProfVoucherQuota.EffectiveDate
                    End If

                    If .PeriodEndDtm > udtProfVoucherQuota.ExpiryDate Then
                        .PeriodEndDtm = udtProfVoucherQuota.ExpiryDate
                    End If

                    .VoucherQuotaCapping = udtProfVoucherQuota.Quota

                    ' 3. Get Voucher Used in period by profession
                    Dim dtmEndDate As DateTime

                    If dtmClaimPeriodTo.HasValue AndAlso .PeriodEndDtm > dtmClaimPeriodTo.Value Then
                        dtmEndDate = dtmClaimPeriodTo.Value
                    Else
                        dtmEndDate = .PeriodEndDtm.AddDays(1)
                    End If

                    .UsedQuota = Me.getUsedQuotaByProfession(udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.IdentityNum, strSchemeCode, strSubsidizeCode, _
                                                             .ProfCode, .PeriodStartDtm, dtmEndDate, udtTransactionDetailList, udtDB)

                    ' 4. Count Available Quota
                    .AvailableQuota = .VoucherQuotaCapping - .UsedQuota
                End With

            End If

            Return udtVoucherQuotaModel
        End Function

        Public Function getEligibleDate(ByVal strSchemeCode As String, ByVal strSubsidizeCode As String, _
                                        ByVal udtEHSPersonalInformation As EHSAccount.EHSAccountModel.EHSPersonalInformationModel, _
                                        ByVal dtmServiceDate As Date) As Nullable(Of DateTime)

            Dim udtClaimRulesBLL As New ClaimRulesBLL()
            Dim udtSchemeClaimBLL As New SchemeClaimBLL()

            ' Retrieve Scheme Claim & Specific SubsidizeGroup (For All SchemeSeq)
            Dim udtSchemeClaimList As SchemeClaimModelCollection = udtSchemeClaimBLL.getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSubsidizeCode(strSchemeCode, strSubsidizeCode)

            Dim dtmEligibleDate As Nullable(Of DateTime) = Nothing

            ' To Check Eligibility, Assume The last Date Expiry Dtm is used for calculation 
            For Each udtSchemeClaim As SchemeClaimModel In udtSchemeClaimList

                ' Scheme Claim is currently effective or passed
                If udtSchemeClaim.EffectiveDtm <= dtmServiceDate Then
                    If udtSchemeClaim.SubsidizeGroupClaimList.Count > 0 Then

                        For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtSchemeClaim.SubsidizeGroupClaimList.OrderBySchemeSeqASC()

                            ' Future subsidize group claim is filtered
                            If udtSubsidizeGroupClaim.ClaimPeriodFrom > dtmServiceDate Then
                                Continue For
                            End If

                            ' Filter Future Date
                            Dim dtmCheckEligible As Nullable(Of DateTime) = Nothing


                            dtmCheckEligible = udtSubsidizeGroupClaim.ClaimPeriodTo.AddDays(-1)

                            If udtSubsidizeGroupClaim.ClaimPeriodFrom <= dtmServiceDate And udtSubsidizeGroupClaim.ClaimPeriodTo > dtmServiceDate Then
                                dtmCheckEligible = dtmServiceDate
                            End If

                            ' Check Eligible
                            If udtClaimRulesBLL.CheckEligibilityPerSubsidize(udtSubsidizeGroupClaim, udtEHSPersonalInformation, dtmCheckEligible, Nothing, True).IsEligible Then
                                dtmEligibleDate = udtSubsidizeGroupClaim.ClaimPeriodFrom
                                Exit For
                            End If

                        Next

                    End If
                End If

            Next

            Return dtmEligibleDate
        End Function

        ''' <summary>
        ''' Retrieve total number of voucher used by profession in period
        ''' </summary>
        ''' <param name="strDocCode"></param>
        ''' <param name="strIdentityNum"></param>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="strSubsidizeCode"></param>
        ''' <param name="strProfCode"></param>
        ''' <param name="dtmStartDate"></param>
        ''' <param name="dtmEndDate"></param>
        ''' <param name="udtTransactionDetailList">
        ''' If full tx list is provided, use it for calculation, otherwise, retrieve tx in db</param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getUsedQuotaByProfession(ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal strSchemeCode As String, _
                                                 ByVal strSubsidizeCode As String, ByVal strProfCode As String, ByVal dtmStartDate As DateTime, ByVal dtmEndDate As DateTime, _
                                                 ByVal udtTransactionDetailList As TransactionDetailModelCollection, _
                                                 Optional ByVal udtDB As Database = Nothing) As Integer

            Dim intUsedVoucher As Integer = 0
            Dim udtFilteredTransaction As New TransactionDetailModelCollection

            ' Retrieve Voucher Transaction (By Scheme, Profession, Date Range)
            If udtTransactionDetailList Is Nothing Then
                If udtDB Is Nothing Then udtDB = New Database()

                strIdentityNum = Me._udtFormatter.formatDocumentIdentityNumber(strDocCode, strIdentityNum)
                udtFilteredTransaction = Me.getTransactionDetailVoucherByRangeProfession(strDocCode, strIdentityNum, strSchemeCode, strSubsidizeCode, dtmStartDate, dtmEndDate, strProfCode, udtDB)

            Else
                ' Use the retrieved transaction to calculate quota
                udtFilteredTransaction = udtTransactionDetailList.FilterByPeriodProfession(strSchemeCode, strSubsidizeCode, dtmStartDate, dtmEndDate, strProfCode)
            End If

            '' Sum the used voucher
            For Each udtTranDetailModel As TransactionDetailModel In udtFilteredTransaction
                intUsedVoucher += udtTranDetailModel.TotalAmount
            Next

            Return intUsedVoucher
        End Function
        ' CRE19-003 (Opt voucher capping) [End][Winnie]

        ''' <summary>
        ''' Retrieve Benefit (TransactionDetail) By Scheme Code, Subsidize Code, and Document Identity
        ''' </summary>
        ''' <param name="strDocCode"></param>
        ''' <param name="strIdentityNum"></param>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="intSchemeSeq"></param>
        ''' <param name="strSubsidizeCode"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getTransactionDetailBenefit(ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal strSchemeCode As String, _
                ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String, Optional ByVal udtDB As Database = Nothing) As TransactionDetailModelCollection

            Dim udtTransactionDetailList As New TransactionDetailModelCollection()
            Dim udtTranDetailModel As TransactionDetailModel = Nothing


            If udtDB Is Nothing Then udtDB = New Database()
            Dim dt As New DataTable()

            strIdentityNum = Me._udtFormatter.formatDocumentIdentityNumber(strDocCode, strIdentityNum)

            Try
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Doc_Code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocCode), _
                    udtDB.MakeInParam("@identity", EHSAccount.EHSAccountModel.IdentityNum_DataType, EHSAccount.EHSAccountModel.IdentityNum_DataSize, strIdentityNum), _
                    udtDB.MakeInParam("@Scheme_Code", SchemeClaimModel.Scheme_Code_DataType, SchemeClaimModel.Scheme_Code_DataSize, strSchemeCode), _
                    udtDB.MakeInParam("@Scheme_seq", SchemeClaimModel.Scheme_Seq_DataType, SchemeClaimModel.Scheme_Seq_DataSize, intSchemeSeq), _
                    udtDB.MakeInParam("@Subsidize_Code", SubsidizeGroupClaimModel.Subsidize_Code_DataType, SubsidizeGroupClaimModel.Subsidize_Code_DataSize, strSubsidizeCode) _
                }

                udtDB.RunProc("proc_TransactionDetail_get_byDocCodeDocIDSchemeSubsidize", prams, dt)

                For Each dr As DataRow In dt.Rows

                    udtTranDetailModel = Me.FillTransactionDetail(dr)
                    udtTranDetailModel.ServiceReceiveDtm = CDate(dr("Service_Receive_Dtm"))
                    udtTranDetailModel.DOB = CDate(dr("DOB"))
                    udtTranDetailModel.ExactDOB = CStr(dr("Exact_DOB")).Trim()
                    udtTransactionDetailList.Add(udtTranDetailModel)
                Next

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return udtTransactionDetailList

        End Function

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        ' Remove SchemeSeq
        'Public Function getTransactionDetailBenefit(ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal strSchemeCode As String, _
        '        ByVal intSchemeSeq As Integer, Optional ByVal udtDB As Database = Nothing) As TransactionDetailModelCollection
        Public Function getTransactionDetailBenefit(ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal strSchemeCode As String, _
            Optional ByVal udtDB As Database = Nothing) As TransactionDetailModelCollection

            Dim udtTransactionDetailList As New TransactionDetailModelCollection()
            Dim udtTranDetailModel As TransactionDetailModel = Nothing

            If udtDB Is Nothing Then udtDB = New Database()
            Dim dt As New DataTable()

            strIdentityNum = Me._udtFormatter.formatDocumentIdentityNumber(strDocCode, strIdentityNum)

            Try
                'Dim prams() As SqlParameter = { _
                '    udtDB.MakeInParam("@Doc_Code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocCode), _
                '    udtDB.MakeInParam("@identity", EHSAccount.EHSAccountModel.IdentityNum_DataType, EHSAccount.EHSAccountModel.IdentityNum_DataSize, strIdentityNum), _
                '    udtDB.MakeInParam("@Scheme_Code", SchemeClaimModel.Scheme_Code_DataType, SchemeClaimModel.Scheme_Code_DataSize, strSchemeCode), _
                '    udtDB.MakeInParam("@Scheme_seq", SchemeClaimModel.Scheme_Seq_DataType, SchemeClaimModel.Scheme_Seq_DataSize, intSchemeSeq) _
                '}
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Doc_Code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocCode), _
                    udtDB.MakeInParam("@identity", EHSAccount.EHSAccountModel.IdentityNum_DataType, EHSAccount.EHSAccountModel.IdentityNum_DataSize, strIdentityNum), _
                    udtDB.MakeInParam("@Scheme_Code", SchemeClaimModel.Scheme_Code_DataType, SchemeClaimModel.Scheme_Code_DataSize, strSchemeCode) _
                }

                udtDB.RunProc("proc_TransactionDetail_get_byDocCodeDocIDScheme", prams, dt)

                For Each dr As DataRow In dt.Rows

                    udtTranDetailModel = Me.FillTransactionDetail(dr)
                    udtTranDetailModel.ServiceReceiveDtm = CDate(dr("Service_Receive_Dtm"))
                    udtTranDetailModel.DOB = CDate(dr("DOB"))
                    udtTranDetailModel.ExactDOB = CStr(dr("Exact_DOB")).Trim()
                    udtTransactionDetailList.Add(udtTranDetailModel)
                Next

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return udtTransactionDetailList

        End Function
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Function getTransactionDetail_SSSCMC(ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal strSchemeCode As String, _
                                                    Optional ByVal udtDB As Database = Nothing) As TransactionDetailModelCollection

            Dim udtTransactionDetailList As New TransactionDetailModelCollection()
            Dim udtTranDetailModel As TransactionDetailModel = Nothing

            If udtDB Is Nothing Then udtDB = New Database()
            Dim dt As New DataTable()

            strIdentityNum = Me._udtFormatter.formatDocumentIdentityNumber(strDocCode, strIdentityNum)

            Try
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Doc_Code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocCode), _
                    udtDB.MakeInParam("@identity", EHSAccount.EHSAccountModel.IdentityNum_DataType, EHSAccount.EHSAccountModel.IdentityNum_DataSize, strIdentityNum), _
                    udtDB.MakeInParam("@Scheme_Code", SchemeClaimModel.Scheme_Code_DataType, SchemeClaimModel.Scheme_Code_DataSize, strSchemeCode) _
                }

                udtDB.RunProc("proc_TransactionDetail_SSSCMC_get_byDocCodeDocIDScheme", prams, dt)

                For Each dr As DataRow In dt.Rows
                    udtTranDetailModel = Me.FillTransactionDetail(dr)
                    udtTranDetailModel.ServiceReceiveDtm = CDate(dr("Service_Receive_Dtm"))
                    'udtTranDetailModel.DOB = CDate(dr("DOB"))
                    'udtTranDetailModel.ExactDOB = CStr(dr("Exact_DOB")).Trim()
                    udtTransactionDetailList.Add(udtTranDetailModel)
                Next

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return udtTransactionDetailList

        End Function
        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

        Public Function getTransactionDetailVoucher(ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal strSchemeCode As String, _
                ByVal strSubsidizeCode As String, Optional ByVal udtDB As Database = Nothing) As TransactionDetailModelCollection

            Dim udtTransactionDetailList As New TransactionDetailModelCollection()
            Dim udtTranDetailModel As TransactionDetailModel = Nothing

            Dim dt As New DataTable()

            Try

                ' CRE19-003 (Opt voucher capping) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                dt = getTransactionDetail(strDocCode, strIdentityNum, strSchemeCode, strSubsidizeCode, Nothing, Nothing, String.Empty, udtDB)
                ' CRE19-003 (Opt voucher capping) [End][Winnie]

                For Each dr As DataRow In dt.Rows

                    udtTranDetailModel = Me.FillTransactionDetail(dr)
                    'udtTranDetailModel.ServiceReceiveDtm = CDate(dr("Service_Receive_Dtm"))
                    udtTransactionDetailList.Add(udtTranDetailModel)
                Next

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return udtTransactionDetailList
        End Function

        'CRE13-019-02 Extend HCVS to China [Start][Karl]
        Public Function getTransactionDetailVoucherBySubsidizeCodeOnly(ByVal strDocCode As String, ByVal strIdentityNum As String, _
                                                                       ByVal strSubsidizeCode As String, Optional ByVal udtDB As Database = Nothing) As TransactionDetailModelCollection

            Dim udtTransactionDetailList As New TransactionDetailModelCollection()
            Dim udtTranDetailModel As TransactionDetailModel = Nothing

            Dim dt As New DataTable()

            Try

                ' CRE19-003 (Opt voucher capping) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                dt = getTransactionDetail(strDocCode, strIdentityNum, String.Empty, strSubsidizeCode, Nothing, Nothing, String.Empty, udtDB)
                ' CRE19-003 (Opt voucher capping) [End][Winnie]

                For Each dr As DataRow In dt.Rows
                    udtTranDetailModel = Me.FillTransactionDetail(dr)
                    ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Winnie]
                    ' -----------------------------------------------------------------------------------------
                    udtTranDetailModel.ServiceReceiveDtm = CDate(dr("Service_Receive_Dtm"))
                    udtTranDetailModel.ServiceType = CStr(dr("Service_Type"))
                    ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Winnie]
                    udtTransactionDetailList.Add(udtTranDetailModel)
                Next

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return udtTransactionDetailList
        End Function
        'CRE13-019-02 Extend HCVS to China [End][Karl]

        ' CRE19-003 (Opt voucher capping) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Public Function getTransactionDetailVoucherByRangeProfession(ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal strSchemeCode As String, _
                                                                     ByVal strSubsidizeCode As String, ByVal dtmPeriodFrom As DateTime, ByVal dtmPeriodTo As DateTime, _
                                                                     ByVal strServiceCategoryCode As String, Optional ByVal udtDB As Database = Nothing) As TransactionDetailModelCollection

            Dim udtTransactionDetailList As New TransactionDetailModelCollection()
            Dim udtTranDetailModel As TransactionDetailModel = Nothing

            Dim dt As New DataTable()

            Try
                dt = getTransactionDetail(strDocCode, strIdentityNum, strSchemeCode, strSubsidizeCode, dtmPeriodFrom, dtmPeriodTo, strServiceCategoryCode, udtDB)

                For Each dr As DataRow In dt.Rows

                    udtTranDetailModel = Me.FillTransactionDetail(dr)
                    udtTranDetailModel.ServiceReceiveDtm = CDate(dr("Service_Receive_Dtm"))
                    udtTransactionDetailList.Add(udtTranDetailModel)
                Next

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return udtTransactionDetailList
        End Function


        Public Function getTransactionDetail(ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal strSchemeCode As String, _
                                                    ByVal strSubsidizeCode As String, ByVal dtmPeriodFrom As DateTime?, ByVal dtmPeriodTo As DateTime?, _
                                                    ByVal strServiceCategoryCode As String, Optional ByVal udtDB As Database = Nothing) As DataTable

            Dim udtTransactionDetailList As New TransactionDetailModelCollection()
            Dim udtTranDetailModel As TransactionDetailModel = Nothing

            If udtDB Is Nothing Then udtDB = New Database()
            Dim dt As New DataTable()

            strIdentityNum = Me._udtFormatter.formatDocumentIdentityNumber(strDocCode, strIdentityNum)


            Try
                ' CRE19-003 (Opt voucher capping) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Add [Period_From], [Period_To], [Service_Category_Code]
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Doc_Code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocCode), _
                    udtDB.MakeInParam("@identity", EHSAccount.EHSAccountModel.IdentityNum_DataType, EHSAccount.EHSAccountModel.IdentityNum_DataSize, strIdentityNum), _
                    udtDB.MakeInParam("@Scheme_Code", SchemeClaimModel.Scheme_Code_DataType, SchemeClaimModel.Scheme_Code_DataSize, strSchemeCode), _
                    udtDB.MakeInParam("@Subsidize_Code", SubsidizeGroupClaimModel.Subsidize_Code_DataType, SubsidizeGroupClaimModel.Subsidize_Code_DataSize, strSubsidizeCode), _
                    udtDB.MakeInParam("@Period_From", SqlDbType.DateTime, 8, IIf(dtmPeriodFrom.HasValue, dtmPeriodFrom, DBNull.Value)), _
                    udtDB.MakeInParam("@Period_To", SqlDbType.DateTime, 8, IIf(dtmPeriodTo.HasValue, dtmPeriodTo, DBNull.Value)), _
                    udtDB.MakeInParam("@Service_Category_Code", ProfessionalModel.ServiceCategoryCodeDataType, ProfessionalModel.ServiceCategoryCodeDataSize, strServiceCategoryCode) _
                }
                ' CRE19-003 (Opt voucher capping) [End][Winnie]


                udtDB.RunProc("proc_TransactionDetail_get_byDocCodeDocIDSchemeSubsidizeAll", prams, dt)

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return dt
        End Function
        ' CRE19-003 (Opt voucher capping) [End][Winnie]

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Enum Source
            GetFromDB
            GetFromSession
            NoSession
        End Enum
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Sub ClearSessionTransactionDetailBenefit()
            HttpContext.Current.Session.Remove(SESS.TransactionDetailBenefitDocCode)
            HttpContext.Current.Session.Remove(SESS.TransactionDetailBenefitDocNo)
            HttpContext.Current.Session.Remove(SESS.TransactionDetailBenefit)
        End Sub
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieve All Benefit (TransactionDetail) of the recipient
        ''' </summary>
        ''' <param name="strDocCode"></param>
        ''' <param name="strIdentityNum"></param>
        ''' <param name="enumSource"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getTransactionDetailBenefit(ByVal strDocCode As String, ByVal strIdentityNum As String, Optional ByVal enumSource As Source = Source.GetFromSession, Optional ByVal udtDB As Database = Nothing) As TransactionDetailModelCollection
            Dim udtTransactionDetailList As New TransactionDetailModelCollection()
            Dim udtTranDetailModel As TransactionDetailModel = Nothing
            Dim blnDiffDoc As Boolean = False

            If udtDB Is Nothing Then udtDB = New Database()

            If HttpContext.Current.Session(SESS.TransactionDetailBenefitDocCode) Is Nothing OrElse _
                CType(HttpContext.Current.Session(SESS.TransactionDetailBenefitDocCode), String) <> strDocCode Then
                blnDiffDoc = True
            End If

            If HttpContext.Current.Session(SESS.TransactionDetailBenefitDocNo) Is Nothing OrElse _
                CType(HttpContext.Current.Session(SESS.TransactionDetailBenefitDocNo), String) <> strIdentityNum Then
                blnDiffDoc = True
            End If

            HttpContext.Current.Session(SESS.TransactionDetailBenefitDocCode) = strDocCode
            HttpContext.Current.Session(SESS.TransactionDetailBenefitDocNo) = strIdentityNum

            If HttpContext.Current.Session(SESS.TransactionDetailBenefit) Is Nothing OrElse enumSource = Source.GetFromDB OrElse blnDiffDoc Then
                Try
                    Dim dt As DataTable = getTransactionDetailBenefitDataTable(strDocCode, strIdentityNum, udtDB)
                    For Each dr As DataRow In dt.Rows
                        udtTranDetailModel = Me.FillTransactionDetail(dr)
                        udtTranDetailModel.ServiceReceiveDtm = CDate(dr("Service_Receive_Dtm"))
                        udtTranDetailModel.DOB = CDate(dr("DOB"))
                        udtTranDetailModel.ExactDOB = CStr(dr("Exact_DOB")).Trim()
                        udtTransactionDetailList.Add(udtTranDetailModel)
                    Next

                    HttpContext.Current.Session(SESS.TransactionDetailBenefit) = udtTransactionDetailList

                Catch eSQL As SqlException
                    Throw eSQL
                Catch ex As Exception
                    Throw
                End Try
            Else
                Dim udtNewTranDetailList As New TransactionDetailModelCollection

                For Each udtTranDetail As TransactionDetailModel In CType(HttpContext.Current.Session(SESS.TransactionDetailBenefit), TransactionDetailModelCollection)
                    udtNewTranDetailList.Add(udtTranDetail)
                Next

                udtTransactionDetailList = udtNewTranDetailList
            End If

            Return udtTransactionDetailList

        End Function
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieve All Benefit (TransactionDetail) of the recipient
        ''' </summary>
        ''' <param name="strDocCode"></param>
        ''' <param name="strIdentityNum"></param>
        ''' <param name="enumSource"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getTransactionDetailBenefitForIVRS(ByVal strDocCode As String, ByVal strIdentityNum As String, Optional ByVal enumSource As Source = Source.GetFromSession, Optional ByVal udtDB As Database = Nothing) As TransactionDetailModelCollection
            Dim udtTransactionDetailList As New TransactionDetailModelCollection()
            Dim udtTranDetailModel As TransactionDetailModel = Nothing
            Dim blnDiffDoc As Boolean = False

            If udtDB Is Nothing Then udtDB = New Database()

            Try
                Dim dt As DataTable = getTransactionDetailBenefitDataTable(strDocCode, strIdentityNum, udtDB)
                For Each dr As DataRow In dt.Rows
                    udtTranDetailModel = Me.FillTransactionDetail(dr)
                    udtTranDetailModel.ServiceReceiveDtm = CDate(dr("Service_Receive_Dtm"))
                    udtTranDetailModel.DOB = CDate(dr("DOB"))
                    udtTranDetailModel.ExactDOB = CStr(dr("Exact_DOB")).Trim()
                    udtTransactionDetailList.Add(udtTranDetailModel)
                Next

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return udtTransactionDetailList

        End Function
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        ''' <summary>
        ''' Retrieve All Benefit (TransactionDetail) of the recipient
        ''' </summary>
        ''' <param name="strDocCode"></param>
        ''' <param name="strIdentityNum"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getTransactionDetailBenefitDataTable(ByVal strDocCode As String, ByVal strIdentityNum As String, Optional ByVal udtDB As Database = Nothing) As DataTable
            'Dim udtTransactionDetailList As New TransactionDetailModelCollection()
            'Dim udtTranDetailModel As TransactionDetailModel = Nothing


            If udtDB Is Nothing Then udtDB = New Database()
            Dim dt As New DataTable()

            'strIdentityNum = Me._udtFormatter.formatDocumentIdentityNumber(strDocCode, strIdentityNum)

            Try
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Doc_Code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocCode), _
                    udtDB.MakeInParam("@identity", EHSAccount.EHSAccountModel.IdentityNum_DataType, EHSAccount.EHSAccountModel.IdentityNum_DataSize, strIdentityNum)}

                udtDB.RunProc("proc_TransactionDetail_get_byDocCodeDocID", prams, dt)

                '    For Each dr As DataRow In dt.Rows

                '        udtTranDetailModel = Me.FillTransactionDetail(dr)
                '        udtTranDetailModel.ServiceReceiveDtm = CDate(dr("Service_Receive_Dtm"))
                '        udtTransactionDetailList.Add(udtTranDetailModel)
                '    Next

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return dt

            'Return udtTransactionDetailList
        End Function

        ' CRE20-005 (Providing users' data in HCVS to eHR Patient Portal) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Function getPatientPortalVoucherTransactionHistory(ByVal strDocCode As String, ByVal strIdentityNum As String, _
                                                                    ByVal strSchemeCode As String, ByVal strSubsidizeCode As String, _
                                                                    ByVal strAccType As String, _
                                                                    Optional ByVal udtDB As Database = Nothing) As DataTable

            Dim udtTransactionDetailList As New TransactionDetailModelCollection()
            Dim udtTranDetailModel As TransactionDetailModel = Nothing

            If udtDB Is Nothing Then udtDB = New Database()
            Dim dt As New DataTable()

            Try
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Doc_Code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocCode), _
                    udtDB.MakeInParam("@identity", EHSAccount.EHSAccountModel.IdentityNum_DataType, EHSAccount.EHSAccountModel.IdentityNum_DataSize, strIdentityNum), _
                    udtDB.MakeInParam("@Scheme_Code", SchemeClaimModel.Scheme_Code_DataType, SchemeClaimModel.Scheme_Code_DataSize, IIf(strSchemeCode = String.Empty, DBNull.Value, strSchemeCode)), _
                    udtDB.MakeInParam("@Subsidize_Code", SubsidizeGroupClaimModel.Subsidize_Code_DataType, SubsidizeGroupClaimModel.Subsidize_Code_DataSize, IIf(strSubsidizeCode = String.Empty, DBNull.Value, strSubsidizeCode)), _
                    udtDB.MakeInParam("@Acc_Type", SqlDbType.Char, 1, IIf(strAccType = String.Empty, DBNull.Value, strAccType)) _
                }

                udtDB.RunProc("proc_PatientPortalVoucherTransactionHistory_get", prams, dt)

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return dt

        End Function
        ' CRE20-005 (Providing users' data in HCVS to eHR Patient Portal) [End][Chris YIM]

        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
        ' ------------------------------------------------------------------------
        Public Function getVoucherTransactionHistory(ByVal strDocCode As String, ByVal strIdentityNum As String, _
                                                                 ByVal strSchemeCode As String, ByVal strSubsidizeCode As String, _
                                                                 ByVal strAccType As String, _
                                                                 ByVal dtmPeriodFrom As DateTime?, ByVal dtmPeriodTo As DateTime?, _
                                                                 Optional ByVal udtDB As Database = Nothing) As DataTable
            ' CRE19-026 (HCVS hotline service) [End][Winnie]

            Dim udtTransactionDetailList As New TransactionDetailModelCollection()
            Dim udtTranDetailModel As TransactionDetailModel = Nothing

            If udtDB Is Nothing Then udtDB = New Database()
            Dim dt As New DataTable()

            Try

                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Doc_Code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocCode), _
                    udtDB.MakeInParam("@identity", EHSAccount.EHSAccountModel.IdentityNum_DataType, EHSAccount.EHSAccountModel.IdentityNum_DataSize, strIdentityNum), _
                    udtDB.MakeInParam("@Scheme_Code", SchemeClaimModel.Scheme_Code_DataType, SchemeClaimModel.Scheme_Code_DataSize, IIf(strSchemeCode = String.Empty, DBNull.Value, strSchemeCode)), _
                    udtDB.MakeInParam("@Subsidize_Code", SubsidizeGroupClaimModel.Subsidize_Code_DataType, SubsidizeGroupClaimModel.Subsidize_Code_DataSize, IIf(strSubsidizeCode = String.Empty, DBNull.Value, strSubsidizeCode)), _
                    udtDB.MakeInParam("@Acc_Type", SqlDbType.Char, 1, IIf(strAccType = String.Empty, DBNull.Value, strAccType)), _
                    udtDB.MakeInParam("@Period_From", SqlDbType.DateTime, 8, IIf(dtmPeriodFrom.HasValue, dtmPeriodFrom, DBNull.Value)), _
                    udtDB.MakeInParam("@Period_To", SqlDbType.DateTime, 8, IIf(dtmPeriodTo.HasValue, dtmPeriodTo, DBNull.Value)) _
                }

                udtDB.RunProc("proc_VoucherTransactionHistory_get", prams, dt)

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return dt

        End Function
        ' CRE19-026 (HCVS hotline service) [End][Winnie]

        ' INT13-0012 - Fix EHAPP concurrent claim checking [Start][Tommy L]
        ' -------------------------------------------------------------------------------------
        ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        'Public Function getAvailableSubsidizeItem_Registration(ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, ByVal udtSubsidizeGroupClaim_Registration As SubsidizeGroupClaimModel) As Integer
        Public Function getAvailableSubsidizeItem_Registration(ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, ByVal udtSubsidizeGroupClaim_Registration As SubsidizeGroupClaimModel, Optional ByRef udtDB As Database = Nothing) As Integer
            ' CRE13-006 - HCVS Ceiling [End][Tommy L]
            Dim udtSchemeDetailBLL As New SchemeDetails.SchemeDetailBLL

            Dim udtSubsidizeItemDetailList As SchemeDetails.SubsidizeItemDetailsModelCollection = udtSchemeDetailBLL.getSubsidizeItemDetails(udtSubsidizeGroupClaim_Registration.SubsidizeItemCode)
            Dim udtSubsidizeItemDetail As SchemeDetails.SubsidizeItemDetailsModel = udtSubsidizeItemDetailList(0)

            ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            'Dim udtTransDetailBenefitList As TransactionDetailModelCollection = Me.getTransactionDetailBenefit(udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, udtSubsidizeGroupClaim_Registration.SchemeCode)
            Dim udtTransDetailBenefitList As TransactionDetailModelCollection = Me.getTransactionDetailBenefit(udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, udtSubsidizeGroupClaim_Registration.SchemeCode, udtDB)
            ' CRE13-006 - HCVS Ceiling [End][Tommy L]
            Dim udtTransDetailBenefitListByAvailItem As TransactionDetailModelCollection

            Dim intNumSubsidize_Total As Integer
            Dim intNumSubsidize_Used As Integer

            intNumSubsidize_Total = udtSubsidizeItemDetail.AvailableItemNum

            intNumSubsidize_Used = 0
            udtTransDetailBenefitListByAvailItem = udtTransDetailBenefitList.FilterBySubsidizeItemDetail(udtSubsidizeGroupClaim_Registration.SchemeCode, udtSubsidizeGroupClaim_Registration.SchemeSeq, udtSubsidizeGroupClaim_Registration.SubsidizeCode, udtSubsidizeGroupClaim_Registration.SubsidizeItemCode, udtSubsidizeItemDetail.AvailableItemCode)
            For Each udtTransDetailBenefitByAvailItem As TransactionDetailModel In udtTransDetailBenefitListByAvailItem
                intNumSubsidize_Used += udtTransDetailBenefitByAvailItem.Unit.Value
            Next

            Return intNumSubsidize_Total - intNumSubsidize_Used
        End Function
        ' INT13-0012 - Fix EHAPP concurrent claim checking [End][Tommy L]

        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Function getAvailableSubsidizeItem_SSSCMC(ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, _
                                                         ByVal udtSubsidizeGroupClaimList As SubsidizeGroupClaimModelCollection, _
                                                         Optional ByRef udtDB As Database = Nothing) As Decimal

            Dim udtSchemeDetailBLL As New SchemeDetails.SchemeDetailBLL

            Dim udtSubsidizeItemDetailList As SchemeDetails.SubsidizeItemDetailsModelCollection = udtSchemeDetailBLL.getSubsidizeItemDetails(udtSubsidizeGroupClaimList(0).SubsidizeItemCode)
            Dim udtSubsidizeItemDetail As SchemeDetails.SubsidizeItemDetailsModel = udtSubsidizeItemDetailList(0)

            Dim udtTransDetailBenefitList As TransactionDetailModelCollection = Me.getTransactionDetail_SSSCMC(udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, udtSubsidizeGroupClaimList(0).SchemeCode, udtDB)
            Dim udtTransDetailBenefitListByAvailItem As TransactionDetailModelCollection

            Dim decNumSubsidize_Total As Decimal = CDec(udtSubsidizeGroupClaimList(0).NumSubsidize)
            Dim decNumSubsidize_Used As Decimal = 0.0

            For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtSubsidizeGroupClaimList
                udtTransDetailBenefitListByAvailItem = udtTransDetailBenefitList.FilterBySubsidizeItemDetail(udtSubsidizeGroupClaim.SchemeCode, _
                                                                                                             udtSubsidizeGroupClaim.SchemeSeq, _
                                                                                                             udtSubsidizeGroupClaim.SubsidizeCode, _
                                                                                                             udtSubsidizeGroupClaim.SubsidizeItemCode, _
                                                                                                             udtSubsidizeItemDetail.AvailableItemCode)

                For Each udtTransDetailBenefitByAvailItem As TransactionDetailModel In udtTransDetailBenefitListByAvailItem
                    decNumSubsidize_Used += CDec(udtTransDetailBenefitByAvailItem.TotalAmountRMB)
                Next
            Next

            Return decNumSubsidize_Total - decNumSubsidize_Used

        End Function
        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

#End Region

#Region "Transaction Checking"

        Public Function chkEHSTranVaildForVoid(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtSP As ServiceProvider.ServiceProviderModel, ByVal udtDataEntry As DataEntryUser.DataEntryUserModel) As Common.ComObject.SystemMessage
            Dim systemMessage As SystemMessage = Nothing
            Dim isValid As Boolean = True

            If udtEHSTransaction Is Nothing Then
                'Show system message
                isValid = False
                systemMessage = New SystemMessage("020302", "E", "00002")
            Else

                systemMessage = Me.chkVailDateForVoid(udtEHSTransaction.ConfirmDate)
                If Not systemMessage Is Nothing Then
                    isValid = False
                End If

                If isValid Then
                    systemMessage = Me.chkAllowVoid(udtEHSTransaction)
                    If Not systemMessage Is Nothing Then
                        isValid = False
                    End If
                End If

                If isValid Then
                    systemMessage = Me.chkValidRoleForVoid(udtEHSTransaction, udtSP, udtDataEntry)
                    If Not systemMessage Is Nothing Then
                        isValid = False
                    End If
                End If
            End If
            Return systemMessage
        End Function

        Private Function chkVailDateForVoid(ByVal confirmDate As Nullable(Of DateTime)) As SystemMessage
            Dim systemMessage As SystemMessage = Nothing
            If confirmDate.HasValue Then

                Dim datConfirmDate As Date = CType(confirmDate, DateTime)
                Dim generalFunction As GeneralFunction = New GeneralFunction()

                If datConfirmDate.AddHours(24) < generalFunction.GetSystemDateTime() Then
                    systemMessage = New SystemMessage("020302", "E", "00003")
                End If
            End If

            Return systemMessage
        End Function

        Private Function chkAllowVoid(ByVal udtEHSTransaction As EHSTransactionModel) As SystemMessage
            Dim systemMessage As SystemMessage = Nothing

            If udtEHSTransaction.RecordStatus.Equals(Common.Component.ClaimTransStatus.Inactive) Then
                systemMessage = New SystemMessage("020302", "E", "00006")
            Else

                ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
                ' -----------------------------------------------------------------------------------------

                'Allow void status
                'P = Create by DataEntry need SP to confirm
                'V = Create by SP or SP confirm the record, but the related temp account need to be validate
                'A = Active (Ready to re-imburse)
                'U = Imcomplete

                'CRE13-001 EHAPP [Start][Karl] 
                'Add status
                ' ----------------------------------------------------------------------------
                'J = Joined
                'CRE13-001 EHAPP [End][Karl] 

                If Not udtEHSTransaction.RecordStatus.Equals(EHSTransactionModel.TransRecordStatusClass.Pending) AndAlso _
                    Not udtEHSTransaction.RecordStatus.Equals(EHSTransactionModel.TransRecordStatusClass.Active) AndAlso _
                    Not udtEHSTransaction.RecordStatus.Equals(EHSTransactionModel.TransRecordStatusClass.PendingVRValidate) AndAlso _
                    Not udtEHSTransaction.RecordStatus.Equals(EHSTransactionModel.TransRecordStatusClass.Incomplete) AndAlso _
                    Not udtEHSTransaction.RecordStatus.Equals(EHSTransactionModel.TransRecordStatusClass.Joined) Then

                    systemMessage = New SystemMessage("020302", "E", "00003")
                End If

                ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

                ' The claim transaction is created by back office user.
                If IsNothing(systemMessage) Then
                    If udtEHSTransaction.ManualReimburse Then
                        systemMessage = New SystemMessage("020302", "E", "00003")
                    End If
                End If

            End If

            Return systemMessage
        End Function

        Private Function chkValidRoleForVoid(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtSP As ServiceProvider.ServiceProviderModel, ByVal udtDataEntry As DataEntryUser.DataEntryUserModel) As SystemMessage

            Dim systemMessage As SystemMessage = Nothing
            Dim vailResult As Boolean = True

            'Check using account
            If udtDataEntry Is Nothing Then

                If Not udtEHSTransaction.ServiceProviderID.Equals(udtSP.SPID) Then
                    vailResult = False
                End If
            Else
                'Data Entry no right to void transaction, if Record Status is not "P" and transaction is not created by him/her
                If Not udtEHSTransaction.DataEntryBy.Equals(udtDataEntry.DataEntryAccount) OrElse Not (udtEHSTransaction.RecordStatus = Common.Component.ClaimTransStatus.Pending Or udtEHSTransaction.RecordStatus = Common.Component.ClaimTransStatus.Incomplete) Then
                    vailResult = False
                End If

            End If

            If Not vailResult Then
                systemMessage = New SystemMessage("020302", "E", "00004")
            End If

            Return systemMessage
        End Function

        '''' <summary>
        '''' Check if more than one claim transactions are made for the same voucher recipient by the same enrolled service provider within period of transaction time, 
        '''' and with same voucher amount claimed and same service date
        '''' </summary>
        ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [Start][Winnie]
        ' If any duplicated claim is found, return true, otherwise return false
        Public Function CheckDuplicateClaim(ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal udtEHSTransaction As EHSTransactionModel, ByVal intCheckMinute As Integer, ByVal blnCheckPractice As Boolean, Optional ByVal udtDB As Database = Nothing) As Boolean
            If udtDB Is Nothing Then udtDB = New Database()
            Dim dt As New DataTable()

            strIdentityNum = _udtFormatter.formatDocumentIdentityNumber(strDocCode, strIdentityNum)

            With udtEHSTransaction

                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, .ServiceProviderID), _
                    udtDB.MakeInParam("@Practice_Display_Seq", SqlDbType.SmallInt, 2, IIf(blnCheckPractice, .PracticeID, DBNull.Value)), _
                    udtDB.MakeInParam("@Doc_Code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocCode), _
                    udtDB.MakeInParam("@Identity", EHSAccount.EHSAccountModel.IdentityNum_DataType, EHSAccount.EHSAccountModel.IdentityNum_DataSize, strIdentityNum), _
                    udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, .SchemeCode.Trim()), _
                    udtDB.MakeInParam("@Service_Receive_Dtm", SqlDbType.DateTime, 8, .ServiceDate), _
                    udtDB.MakeInParam("@Total_Amount", SqlDbType.Money, 4, .VoucherClaim), _
                    udtDB.MakeInParam("@Total_Amount_RMB", SqlDbType.Money, 4, IIf(.VoucherClaimRMB.HasValue, .VoucherClaimRMB, DBNull.Value)), _
                    udtDB.MakeInParam("@Check_Minute", SqlDbType.Int, 4, intCheckMinute)
                    }

                udtDB.RunProc("proc_VoucherTransaction_check_duplicate", prams, dt)
            End With

            If dt.Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
        End Function
        ' CRE16-007 (Pop-up message to avoid duplicate voucher claim) [End][Winnie]
#End Region

#Region "EHSAccount TSMP"
        'CRE13-006 HCVS Ceiling [Start][Karl]
        Public Function getEHSAccountTSMP(ByRef udtDB As Database, ByVal strDocCode As String, ByVal strIdentityNum As String) As DataRow
            'Private Function getEHSAccountTSMP(ByRef udtDB As Database, ByVal strDocCode As String, ByVal strIdentityNum As String) As DataRow
            'CRE13-006 HCVS Ceiling [End][Karl]
            If strDocCode.Trim = DocType.DocTypeModel.DocTypeCode.HKBC Then
                strDocCode = DocType.DocTypeModel.DocTypeCode.HKIC
            End If

            Dim dt As New DataTable()

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@identity", EHSAccount.EHSAccountModel.IdentityNum_DataType, EHSAccount.EHSAccountModel.IdentityNum_DataSize, Me._udtFormatter.formatDocumentIdentityNumber(strDocCode, strIdentityNum)), _
                udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, strDocCode)}

            udtDB.RunProc("proc_EHSAccountTSMP_get", prams, dt)

            If dt.Rows.Count > 0 Then
                Return dt.Rows(0)
            Else
                Return Nothing
            End If

        End Function
        'CRE13-006 HCVS Ceiling [Start][Karl]
        Public Sub insertEHSAccountTSMP(ByRef udtDB As Database, ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal strUpdateBy As String)
            'Private Sub insertEHSAccountTSMP(ByRef udtDB As Database, ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal strUpdateBy As String)
            'CRE13-006 HCVS Ceiling [End][Karl]
            If strDocCode.Trim = DocType.DocTypeModel.DocTypeCode.HKBC Then
                strDocCode = DocType.DocTypeModel.DocTypeCode.HKIC
            End If

            Dim parms() As SqlParameter = { _
                udtDB.MakeInParam("@identity", EHSAccount.EHSAccountModel.IdentityNum_DataType, EHSAccount.EHSAccountModel.IdentityNum_DataSize, Me._udtFormatter.formatDocumentIdentityNumber(strDocCode, strIdentityNum)), _
                udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, strDocCode), _
                udtDB.MakeInParam("@update_by", SqlDbType.VarChar, 20, strUpdateBy)}

            udtDB.RunProc("proc_EHSAccountTSMP_add", parms)
        End Sub
        'CRE13-006 HCVS Ceiling [Start][Karl]
        Public Sub updateEHSAccountTSMP(ByRef udtDB As Database, ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal strUpdateBy As String, ByVal byteTSMP As Byte())
            'Private Sub updateEHSAccountTSMP(ByRef udtDB As Database, ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal strUpdateBy As String, ByVal byteTSMP As Byte())
            'CRE13-006 HCVS Ceiling [End][Karl]
            If strDocCode.Trim = DocType.DocTypeModel.DocTypeCode.HKBC Then
                strDocCode = DocType.DocTypeModel.DocTypeCode.HKIC
            End If

            Dim parms() As SqlParameter = { _
                udtDB.MakeInParam("@identity", EHSAccount.EHSAccountModel.IdentityNum_DataType, EHSAccount.EHSAccountModel.IdentityNum_DataSize, Me._udtFormatter.formatDocumentIdentityNumber(strDocCode, strIdentityNum)), _
                udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, strDocCode), _
                udtDB.MakeInParam("@update_by", SqlDbType.VarChar, 20, strUpdateBy), _
                udtDB.MakeInParam("@TSMP", SqlDbType.Timestamp, 8, byteTSMP)}

            udtDB.RunProc("proc_EHSAccountTSMP_upd", parms)
        End Sub

#End Region

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieve All vaccine (TransactionDetail) of the recipient
        ''' </summary>
        ''' <param name="oRequestPatient">Personal Information</param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getTransactionDetailVaccine(ByVal oRequestPatient As EHSAccountModel.EHSPersonalInformationModel, Optional ByVal enumSource As Source = Source.GetFromDB, Optional ByVal udtDB As Database = Nothing) As TransactionDetailVaccineModelCollection
            ' Reformat the identity numuber (make sure the format is correct (with space), e.g. " A1234563"
            Dim formatter As New Common.Format.Formatter
            Dim udtTransactionDetailVaccineList As TransactionDetailVaccineModelCollection = Nothing

            If enumSource = Source.NoSession Then
                udtTransactionDetailVaccineList = getTransactionDetailVaccineForStudentFile(oRequestPatient.DocCode, formatter.formatDocumentIdentityNumber(oRequestPatient.DocCode, oRequestPatient.IdentityNum), enumSource, udtDB)

            Else
                udtTransactionDetailVaccineList = getTransactionDetailVaccine(oRequestPatient.DocCode, formatter.formatDocumentIdentityNumber(oRequestPatient.DocCode, oRequestPatient.IdentityNum), enumSource, udtDB)

            End If

            Return udtTransactionDetailVaccineList
        End Function
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Sub ClearSessionTransactionDetailVaccine()
            HttpContext.Current.Session.Remove(SESS.TransactionDetailVaccineDocCode)
            HttpContext.Current.Session.Remove(SESS.TransactionDetailVaccineDocNo)
            HttpContext.Current.Session.Remove(SESS.TransactionDetailVaccine)
        End Sub
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieve All Benefit (TransactionDetail) of the recipient
        ''' </summary>
        ''' <param name="strDocCode"></param>
        ''' <param name="strIdentityNum"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getTransactionDetailVaccine(ByVal strDocCode As String, ByVal strIdentityNum As String, Optional ByVal enumSource As Source = Source.GetFromDB, Optional ByVal udtDB As Database = Nothing) As Component.EHSTransaction.TransactionDetailVaccineModelCollection
            Dim udtTranDetailList As New Component.EHSTransaction.TransactionDetailVaccineModelCollection()
            Dim udtTranDetailModel As Component.EHSTransaction.TransactionDetailVaccineModel = Nothing
            Dim blnDiffDoc As Boolean = False

            If udtDB Is Nothing Then udtDB = New Database()
            Dim dt As New DataTable()

            'strIdentityNum = Me._udtFormatter.formatDocumentIdentityNumber(strDocCode, strIdentityNum)

            If HttpContext.Current.Session(SESS.TransactionDetailVaccineDocCode) Is Nothing OrElse _
                CType(HttpContext.Current.Session(SESS.TransactionDetailVaccineDocCode), String) <> strDocCode Then
                blnDiffDoc = True
            End If

            If HttpContext.Current.Session(SESS.TransactionDetailVaccineDocNo) Is Nothing OrElse _
                CType(HttpContext.Current.Session(SESS.TransactionDetailVaccineDocNo), String) <> strIdentityNum Then
                blnDiffDoc = True
            End If

            HttpContext.Current.Session(SESS.TransactionDetailVaccineDocCode) = strDocCode
            HttpContext.Current.Session(SESS.TransactionDetailVaccineDocNo) = strIdentityNum

            If HttpContext.Current.Session(SESS.TransactionDetailVaccine) Is Nothing OrElse enumSource = Source.GetFromDB OrElse blnDiffDoc Then
                Try
                    Dim prams() As SqlParameter = { _
                        udtDB.MakeInParam("@Doc_Code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocCode), _
                        udtDB.MakeInParam("@identity", EHSAccount.EHSAccountModel.IdentityNum_DataType, EHSAccount.EHSAccountModel.IdentityNum_DataSize, strIdentityNum)}

                    udtDB.RunProc("proc_TransactionDetail_vaccine_get_byDocCodeDocID", prams, dt)

                    For Each dr As DataRow In dt.Rows
                        udtTranDetailModel = Me.FillTransactionDetailVaccine(dr, strDocCode, strIdentityNum)
                        udtTranDetailList.Add(udtTranDetailModel)
                    Next

                    HttpContext.Current.Session(SESS.TransactionDetailVaccine) = udtTranDetailList

                Catch eSQL As SqlException
                    Throw eSQL
                Catch ex As Exception
                    Throw
                End Try
            Else
                Dim udtNewTranDetailVaccineList As New TransactionDetailVaccineModelCollection

                For Each udtTranDetailVaccine As TransactionDetailVaccineModel In CType(HttpContext.Current.Session(SESS.TransactionDetailVaccine), TransactionDetailVaccineModelCollection)
                    udtNewTranDetailVaccineList.Add(udtTranDetailVaccine)
                Next

                udtTranDetailList = udtNewTranDetailVaccineList
            End If

            Return udtTranDetailList

        End Function
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieve All Benefit (TransactionDetail) of the recipient
        ''' </summary>
        ''' <param name="strDocCode"></param>
        ''' <param name="strIdentityNum"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getTransactionDetailVaccineForStudentFile(ByVal strDocCode As String, ByVal strIdentityNum As String, Optional ByVal enumSource As Source = Source.GetFromDB, Optional ByVal udtDB As Database = Nothing) As Component.EHSTransaction.TransactionDetailVaccineModelCollection
            Dim udtTranDetailList As New Component.EHSTransaction.TransactionDetailVaccineModelCollection()
            Dim udtTranDetailModel As Component.EHSTransaction.TransactionDetailVaccineModel = Nothing
            Dim blnDiffDoc As Boolean = False

            If udtDB Is Nothing Then udtDB = New Database()
            Dim dt As New DataTable()

            Try
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Doc_Code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocCode), _
                    udtDB.MakeInParam("@identity", EHSAccount.EHSAccountModel.IdentityNum_DataType, EHSAccount.EHSAccountModel.IdentityNum_DataSize, strIdentityNum)}

                udtDB.RunProc("proc_TransactionDetail_vaccine_get_byDocCodeDocID", prams, dt)

                For Each dr As DataRow In dt.Rows
                    udtTranDetailModel = Me.FillTransactionDetailVaccine(dr, strDocCode, strIdentityNum)
                    udtTranDetailList.Add(udtTranDetailModel)
                Next

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return udtTranDetailList

        End Function
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="drSource"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function FillTransactionDetailVaccine(ByVal drSource As DataRow, ByVal strDocCode As String, ByVal strIdentityNum As String) As TransactionDetailVaccineModel

            Dim strSubsidizeItemCode As String = String.Empty
            Dim strAvailableItemCode As String = String.Empty
            Dim strAvailableItemDesc As String = String.Empty
            Dim strAvailableItemDescChi As String = String.Empty
            Dim strAvailableItemDescCN As String = String.Empty
            Dim strRemark As String = String.Empty

            If Not drSource.IsNull("Subsidize_Item_Code") Then strSubsidizeItemCode = drSource("Subsidize_Item_Code").ToString().Trim()
            If Not drSource.IsNull("Available_Item_Code") Then strAvailableItemCode = drSource("Available_Item_Code").ToString().Trim()
            If Not drSource.IsNull("Available_Item_Desc") Then strAvailableItemDesc = drSource("Available_Item_Desc").ToString().Trim()
            If Not drSource.IsNull("Available_Item_Desc_Chi") Then strAvailableItemDescChi = drSource("Available_Item_Desc_Chi").ToString().Trim()
            If Not drSource.IsNull("Available_Item_Desc_CN") Then strAvailableItemDescCN = drSource("Available_Item_Desc_CN").ToString().Trim()
            If Not drSource.IsNull("Remark") Then strRemark = drSource("Remark").ToString().Trim()

            Dim intUnit As Nullable(Of Integer) = Nothing
            Dim dblPerUnitValue As Nullable(Of Double) = Nothing
            Dim dblTotalAmount As Nullable(Of Double) = Nothing

            If Not drSource.IsNull("Unit") Then intUnit = CInt(drSource("Unit"))
            If Not drSource.IsNull("Per_Unit_Value") Then dblPerUnitValue = CDbl(drSource("Per_Unit_Value"))
            If Not drSource.IsNull("Total_Amount") Then dblTotalAmount = CDbl(drSource("Total_Amount"))

            Dim strSchemeCode As String = String.Empty
            Dim strSubsidizeDesc As String = String.Empty
            Dim strSubsidizeDescChi As String = String.Empty
            Dim strPracticeName As String = String.Empty
            Dim strPracticeNameChi As String = String.Empty

            Dim dtmTransactionDtm As DateTime
            Dim dtmServiceReceiveDtm As DateTime

            Dim strEngName As String = String.Empty
            Dim strExactDOB As String = String.Empty
            Dim strGender As String = String.Empty
            Dim dtmDOB As DateTime

            If Not drSource.IsNull("Scheme_Code") Then strSchemeCode = drSource("Scheme_Code").ToString().Trim()
            If Not drSource.IsNull("Subsidize_Desc") Then strSubsidizeDesc = drSource("Subsidize_Desc").ToString().Trim()
            If Not drSource.IsNull("Subsidize_Desc_Chi") Then strSubsidizeDescChi = drSource("Subsidize_Desc_Chi").ToString().Trim()
            If Not drSource.IsNull("Practice_Name") Then strPracticeName = drSource("Practice_Name").ToString().Trim()
            If Not drSource.IsNull("Practice_Name_Chi") Then strPracticeNameChi = drSource("Practice_Name_Chi").ToString().Trim()
            dtmTransactionDtm = CDate(drSource("Transaction_Dtm"))
            dtmServiceReceiveDtm = CDate(drSource("Service_Receive_Dtm"))

            If Not drSource.IsNull("Eng_Name") Then strEngName = drSource("Eng_Name").ToString().Trim()
            If Not drSource.IsNull("Exact_DOB") Then strExactDOB = drSource("Exact_DOB").ToString().Trim()
            If Not drSource.IsNull("Sex") Then strGender = drSource("Sex").ToString().Trim()
            dtmDOB = CDate(drSource("DOB"))

            Dim strHighRisk As String = String.Empty
            If Not drSource.IsNull("High_Risk") Then strHighRisk = drSource("High_Risk").ToString().Trim()

            ' CRE20-0023 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Dim strVaccineBrand As String = String.Empty
            If Not drSource.IsNull("Vaccine_Brand") Then strVaccineBrand = drSource("Vaccine_Brand").ToString().Trim()

            Dim strVaccineLotNo As String = String.Empty
            If Not drSource.IsNull("Vaccine_Lot_No") Then strVaccineLotNo = drSource("Vaccine_Lot_No").ToString().Trim()

            Dim strVaccineTradeName As String = String.Empty
            If Not drSource.IsNull("Brand_Trade_Name") Then strVaccineTradeName = drSource("Brand_Trade_Name").ToString().Trim()

            Dim strVaccineTradeNameChi As String = String.Empty
            If Not drSource.IsNull("Brand_Trade_Name_Chi") Then strVaccineTradeNameChi = drSource("Brand_Trade_Name_Chi").ToString().Trim()

            Dim strClinicType As String = String.Empty
            If Not drSource.IsNull("Clinic_Type") Then strClinicType = drSource("Clinic_Type").ToString().Trim()

            Dim udtTransactionDetailVaccineModel As New TransactionDetailVaccineModel( _
                drSource("Transaction_ID").ToString(), _
                drSource("Scheme_Code").ToString(), _
                CInt(drSource("Scheme_Seq")), _
                drSource("Subsidize_Code").ToString(), _
                strSubsidizeItemCode, _
                strAvailableItemCode, _
                intUnit, _
                dblPerUnitValue, _
                dblTotalAmount, _
                strRemark, _
                dtmTransactionDtm, _
                strAvailableItemDesc, _
                strAvailableItemDescChi)
            ' CRE20-0023 (Immu record) [End][Chris YIM]

            udtTransactionDetailVaccineModel.AvailableItemDescCN = strAvailableItemDescCN
            udtTransactionDetailVaccineModel.ServiceReceiveDtm = dtmServiceReceiveDtm
            udtTransactionDetailVaccineModel.SchemeCode = strSchemeCode
            udtTransactionDetailVaccineModel.SubsidizeDesc = strSubsidizeDesc
            udtTransactionDetailVaccineModel.SubsidizeDescChi = strSubsidizeDescChi
            udtTransactionDetailVaccineModel.PracticeName = strPracticeName
            udtTransactionDetailVaccineModel.PracticeNameChi = strPracticeNameChi
            udtTransactionDetailVaccineModel.DOB = dtmDOB
            udtTransactionDetailVaccineModel.ExactDOB = strExactDOB
            udtTransactionDetailVaccineModel.PersonalInformationDemographic = New EHSAccountModel.EHSPersonalInformationDemographicModel(dtmDOB, strExactDOB, strGender, strIdentityNum, strDocCode, strEngName)
            udtTransactionDetailVaccineModel.HighRisk = strHighRisk

            udtTransactionDetailVaccineModel.VaccineBrand = strVaccineBrand.Trim
            udtTransactionDetailVaccineModel.VaccineLotNo = strVaccineLotNo.Trim
            udtTransactionDetailVaccineModel.VaccineTradeName = strVaccineTradeName.Trim
            udtTransactionDetailVaccineModel.VaccineTradeNameChi = strVaccineTradeNameChi.Trim

            Select Case strClinicType.Trim
                Case "C"
                    udtTransactionDetailVaccineModel.ClinicType = PracticeSchemeInfo.PracticeSchemeInfoModel.ClinicTypeEnum.Clinic
                Case "N"
                    udtTransactionDetailVaccineModel.ClinicType = PracticeSchemeInfo.PracticeSchemeInfoModel.ClinicTypeEnum.NonClinic
                Case Else
                    udtTransactionDetailVaccineModel.ClinicType = PracticeSchemeInfo.PracticeSchemeInfoModel.ClinicTypeEnum.NA
            End Select

            Return udtTransactionDetailVaccineModel

        End Function

        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieve all sub-specialities mapping, and put in cache
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetAllSubSpecialitiesMapCache(Optional ByVal udtDB As Database = Nothing) As DataTable

            Dim CACHE_ALL_SubSpecialitiesMap As String = "EHSTransactionBLL_ALL_HAServiceSubSpecialitiesMapping"

            Dim dt As DataTable = Nothing

            If Not IsNothing(HttpRuntime.Cache(CACHE_ALL_SubSpecialitiesMap)) Then
                dt = CType(HttpRuntime.Cache(CACHE_ALL_SubSpecialitiesMap), DataTable)
            Else

                If udtDB Is Nothing Then udtDB = New Database()
                dt = New DataTable()
                Try
                    udtDB.RunProc("proc_HAServiceSubSpecialitiesMapping_get", dt)
                    If dt.Rows.Count > 0 Then
                        Common.ComObject.CacheHandler.InsertCache(CACHE_ALL_SubSpecialitiesMap, dt)
                    End If
                Catch ex As Exception
                    Throw
                End Try
            End If

            Return dt

        End Function

        ''' <summary>
        ''' Retrieve all active sub-specialities mapping, and put in cache
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetActiveSubSpecialitiesMapCache(Optional ByVal udtDB As Database = Nothing) As DataTable

            Dim CACHE_ACTIVE_HAServiceSubSpecialitiesMapping As String = "EHSTransactionBLL_ACTIVE_HAServiceSubSpecialitiesMapping"

            Dim dtRes As DataTable = Nothing

            If Not IsNothing(HttpRuntime.Cache(CACHE_ACTIVE_HAServiceSubSpecialitiesMapping)) Then
                dtRes = CType(HttpRuntime.Cache(CACHE_ACTIVE_HAServiceSubSpecialitiesMapping), DataTable)
            Else

                If udtDB Is Nothing Then udtDB = New Database()
                Dim dt As DataTable = Me.GetAllSubSpecialitiesMapCache

                If dt.Rows.Count > 0 Then
                    Dim dr() As DataRow = dt.Select("Record_Status = 'A'")
                    If dr.Length > 0 Then
                        dtRes = dr.CopyToDataTable
                        Common.ComObject.CacheHandler.InsertCache(CACHE_ACTIVE_HAServiceSubSpecialitiesMapping, dtRes)
                    End If
                End If

            End If

            Return dtRes

        End Function
        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieve list of sub-specialities mapping by practice display seq.
        ''' </summary>
        ''' <param name="intPracticeDisplaySeq"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetActiveSubSpecialitiesByPractice(ByVal intPracticeDisplaySeq As Integer) As DataTable
            Dim dtRes As DataTable = Nothing
            Dim dt As DataTable = Me.GetActiveSubSpecialitiesMapCache

            Dim dr() As DataRow = dt.Select(String.Format("Practice_Display_Seq = {0}", intPracticeDisplaySeq))

            If dr.Length > 0 Then
                dtRes = dr.CopyToDataTable
            End If

            Return dtRes

        End Function
        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieve list of sub-specialities mapping by practice display seq.
        ''' </summary>
        ''' <param name="intPracticeDisplaySeq"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAllSubSpecialitiesByPractice(ByVal intPracticeDisplaySeq As Integer) As DataTable
            Dim dtRes As DataTable = Nothing
            Dim dt As DataTable = Me.GetAllSubSpecialitiesMapCache

            Dim dr() As DataRow = dt.Select(String.Format("Practice_Display_Seq = {0}", intPracticeDisplaySeq))

            If dr.Length > 0 Then
                dtRes = dr.CopyToDataTable
            End If

            Return dtRes

        End Function
        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieve a sub-specialities by code
        ''' </summary>
        ''' <param name="strCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetSubSpecialitiesByCode(ByVal strCode As String, Optional ByVal strLang As String = CultureLanguage.SimpChinese) As String
            Dim strRes As String = String.Empty
            Dim dt As DataTable = Me.GetAllSubSpecialitiesMapCache

            Dim dr() As DataRow = dt.Select(String.Format("SubSpecialities_Code = '{0}'", strCode))

            If dr.Length = 1 Then
                Select Case strLang
                    Case CultureLanguage.TradChinese
                        strRes = dr(0)("Name_CHI")
                    Case CultureLanguage.SimpChinese
                        strRes = dr(0)("Name_CN")
                    Case Else
                        strRes = dr(0)("Name_CN")
                End Select

            End If

            Return strRes

        End Function
        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

    End Class
End Namespace