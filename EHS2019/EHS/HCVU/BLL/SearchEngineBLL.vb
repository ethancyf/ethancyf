Imports System.Data
Imports System.Data.SqlClient
Imports Common.Component
Imports Common.DataAccess
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel.EHSPersonalInformationModel

Public Class SearchEngineBLL
    Private AuthorizeLevel As String
    Private dtTxn As New DataTable
    Dim claimdr As DataRow
    Dim db As New Common.DataAccess.Database
    Dim df As New Common.Format.Formatter
    Dim transStatus As New ClaimTransStatus
    Dim reimbStatus As New ReimbursementStatus
    Dim bankAcctStatus As New BankAcctVerifyStatus
    Dim udcValidator As New Common.Validation.Validator
    Dim udcFormater As New Common.Format.Formatter

    ''' <summary>
    ''' Get the transactions based on the search criteria for the reimbursement in the format of Authorization Summary (first level)
    ''' </summary>
    ''' <param name="criteria"></param>
    ''' <returns>Data table for the Reimbursement Authorization Summary (first level)</returns>
    ''' <remarks></remarks>
    Public Function SearchAuthorizationSummary(ByVal criteria As Common.SearchCriteria.SearchCriteria, ByVal authorised_status As Object, ByVal strReimburseID As String, ByVal strSchemeCode As String) As DataTable
        Dim dt, dtall As New DataTable
        Dim dr As DataRow

        Dim totalVoucher, numberSP, totalAmount As Double
        Dim tmpSPName As String

        totalVoucher = 0
        numberSP = 0
        totalAmount = 0
        tmpSPName = ""

        dt = New DataTable()

        dt.Columns.Add(New DataColumn("details", GetType(String)))
        dt.Columns.Add(New DataColumn("reimburseID", GetType(String)))
        dt.Columns.Add(New DataColumn("noTran", GetType(Integer)))
        dt.Columns.Add(New DataColumn("noSP", GetType(Integer)))
        dt.Columns.Add(New DataColumn("vouchersClaimed", GetType(Integer)))
        'I-CRP15-002 (Fix the display problem of total voucher amount (RMB) in reimbursement UI) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'dt.Columns.Add(New DataColumn("totalAmount", GetType(Single)))
        dt.Columns.Add(New DataColumn("totalAmount", GetType(Decimal)))
        'dt.Columns.Add(New DataColumn("TotalAmountRMB", GetType(Single))) ' CRE13-019-02 Extend HCVS to China [Lawrence]
        dt.Columns.Add(New DataColumn("TotalAmountRMB", GetType(Decimal)))
        'I-CRP15-002 (Fix the display problem of total voucher amount (RMB) in reimbursement UI) [End][Chris YIM]


        Try
            ' create data object and params
            Dim prams() As SqlParameter = {db.MakeInParam("@cutoff_dtm", SqlDbType.DateTime, 8, criteria.CutoffDate), _
                                            db.MakeInParam("@scheme_code", SqlDbType.Char, 10, strSchemeCode) _
                                            }
            ' run the stored procedure
            db.RunProc("proc_ReimbursementSummary_get", prams, dtall)

        Catch eSQL As SqlException
            db.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            db.RollBackTranscation()
            Throw ex
        End Try

        If dtall.Rows.Count > 0 Then
            dr = dt.NewRow()
            dr("details") = "Details"
            dr("reimburseID") = strReimburseID
            dr("noTran") = dtall.Rows(0)("noTran")
            dr("noSP") = dtall.Rows(0)("noSP")
            dr("vouchersClaimed") = dtall.Rows(0)("vouchersClaimed")
            dr("totalAmount") = dtall.Rows(0)("totalAmount")
            dr("TotalAmountRMB") = dtall.Rows(0)("TotalAmountRMB") ' CRE13-019-02 Extend HCVS to China [Lawrence]
            dt.Rows.Add(dr)
        End If

        Return dt

    End Function

    Public Function SearchHoldAuthorizationSummary(ByVal strSchemeCode As String) As DataTable
        Dim dt, dtall As New DataTable
        Dim dr As DataRow

        Dim totalVoucher, numberSP, totalAmount As Double
        Dim tmpSPName As String

        totalVoucher = 0
        numberSP = 0
        totalAmount = 0
        tmpSPName = ""

        dt = New DataTable()

        dt.Columns.Add(New DataColumn("details", GetType(String)))
        dt.Columns.Add(New DataColumn("reimburseID", GetType(String)))
        dt.Columns.Add(New DataColumn("noTran", GetType(Integer)))
        dt.Columns.Add(New DataColumn("noSP", GetType(Integer)))
        dt.Columns.Add(New DataColumn("vouchersClaimed", GetType(Integer)))
        'I-CRP15-002 (Fix the display problem of total voucher amount (RMB) in reimbursement UI) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'dt.Columns.Add(New DataColumn("totalAmount", GetType(Single)))
        dt.Columns.Add(New DataColumn("totalAmount", GetType(Decimal)))
        'dt.Columns.Add(New DataColumn("TotalAmountRMB", GetType(Single))) ' CRE13-019-02 Extend HCVS to China [Lawrence]
        dt.Columns.Add(New DataColumn("TotalAmountRMB", GetType(Decimal)))
        'I-CRP15-002 (Fix the display problem of total voucher amount (RMB) in reimbursement UI) [End][Chris YIM]
        dt.Columns.Add(New DataColumn("AuthorisedCutoffTime", GetType(Date)))
        dt.Columns.Add(New DataColumn("AuthorisedCutoffBy", GetType(String)))

        Try
            ' create data object and params
            Dim prams() As SqlParameter = {db.MakeInParam("@scheme_code", SqlDbType.Char, 10, strSchemeCode)}
            ' run the stored procedure
            db.RunProc("proc_ReimbursementHoldSummary_get", prams, dtall)
        Catch eSQL As SqlException
            db.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            db.RollBackTranscation()
            Throw ex
        End Try

        If dtall.Rows.Count > 0 Then
            dr = dt.NewRow()
            dr("details") = "Details"
            dr("reimburseID") = dtall.Rows(0)("reimburseID")
            dr("noTran") = dtall.Rows(0)("noTran")
            dr("noSP") = dtall.Rows(0)("noSP")
            dr("vouchersClaimed") = dtall.Rows(0)("vouchersClaimed")
            dr("totalAmount") = dtall.Rows(0)("totalAmount")
            dr("TotalAmountRMB") = dtall.Rows(0)("TotalAmountRMB") ' CRE13-019-02 Extend HCVS to China [Lawrence]
            dr("AuthorisedCutoffTime") = dtall.Rows(0)("AuthorisedCutoffTime")
            dr("AuthorisedCutoffBy") = dtall.Rows(0)("AuthorisedCutoffBy")
            dt.Rows.Add(dr)
        End If

        Return dt

    End Function

    ''' <summary>
    ''' Get the transactions based on the search criteria for the reimbursement by service provider id and name
    ''' </summary>
    ''' <param name="criteria"></param>
    ''' <returns>Data table for the Reimbursement Authorization Summary by service provider id and name</returns>
    ''' <remarks></remarks>
    Public Function SearchAuthorizationSummaryBySP(ByVal criteria As Common.SearchCriteria.SearchCriteria, ByVal strAuthorisedStatus As Object, ByVal strReimburseID As String, ByVal strSchemeCode As String, ByVal bWithFirstAuthorization As Boolean) As DataTable
        Dim dtResult As New DataTable

        dtResult.Columns.Add(New DataColumn("lineNum", GetType(Integer)))
        dtResult.Columns.Add(New DataColumn("spNum", GetType(String)))
        dtResult.Columns.Add(New DataColumn("spName", GetType(String)))
        dtResult.Columns.Add(New DataColumn("noTran", GetType(Integer)))
        dtResult.Columns.Add(New DataColumn("vouchersClaimed", GetType(Integer)))
        'I-CRP15-002 (Fix the display problem of total voucher amount (RMB) in reimbursement UI) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'dtResult.Columns.Add(New DataColumn("totalAmount", GetType(Single)))
        dtResult.Columns.Add(New DataColumn("totalAmount", GetType(Decimal)))
        'dtResult.Columns.Add(New DataColumn("TotalAmountRMB", GetType(Single))) ' CRE13-019-02 Extend HCVS to China [Lawrence]
        dtResult.Columns.Add(New DataColumn("TotalAmountRMB", GetType(Decimal)))
        'I-CRP15-002 (Fix the display problem of total voucher amount (RMB) in reimbursement UI) [End][Chris YIM]


        Try
            Dim dt As New DataTable

            If bWithFirstAuthorization Then
                ' For Authorised_Status = '1'
                Dim prams() As SqlParameter = {db.MakeInParam("@reimburse_id", SqlDbType.Char, 15, strReimburseID), _
                                                db.MakeInParam("@scheme_code", SqlDbType.Char, 10, strSchemeCode)}

                db.RunProc("proc_ReimbursementSummarySP_get_byBankFirstAuth", prams, dt)

            Else
                ' For Authorised_Status = 'P' or '2'
                Dim prams() As SqlParameter = {db.MakeInParam("@cutoff_dtm", SqlDbType.DateTime, 8, criteria.CutoffDate), _
                                                db.MakeInParam("@scheme_code", SqlDbType.Char, 10, strSchemeCode), _
                                                db.MakeInParam("@authorised_status", SqlDbType.Char, 1, strAuthorisedStatus)}

                db.RunProc("proc_ReimbursementSummarySP_get_byBank", prams, dt)
            End If

            For i As Integer = 0 To dt.Rows.Count - 1
                Dim drResult As DataRow = dtResult.NewRow()
                drResult("lineNum") = i + 1
                drResult("spNum") = dt.Rows(i)("spID") & " (" & dt.Rows(i)("practiceNo") & ")"
                drResult("spName") = dt.Rows(i)("spName")
                drResult("noTran") = dt.Rows(i)("noTran")
                drResult("vouchersClaimed") = dt.Rows(i)("vouchersClaimed")
                drResult("totalAmount") = dt.Rows(i)("totalAmount")
                drResult("TotalAmountRMB") = dt.Rows(i)("TotalAmountRMB") ' CRE13-019-02 Extend HCVS to China [Lawrence]
                dtResult.Rows.Add(drResult)
            Next

            Return dtResult

        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    ''' <summary>
    ''' Get the transactions based on the search criteria for the reimbursement by bank account and practice
    ''' </summary>
    ''' <param name="criteria"></param>
    ''' <returns>Data table for the Reimbursement Authorization Summary by bank account and practice</returns>
    ''' <remarks></remarks>
    Public Function SearchAuthorizationSummaryByBankAcct(ByVal criteria As Common.SearchCriteria.SearchCriteria, ByVal strAuthorisedStatus As Object, ByVal strSchemeCode As String, ByVal bWithFirstAuthorization As Boolean) As DataTable
        Dim dtResult As New DataTable

        dtResult.Columns.Add(New DataColumn("blockNo", GetType(String)))
        dtResult.Columns.Add(New DataColumn("bankAccount", GetType(String)))
        dtResult.Columns.Add(New DataColumn("practice", GetType(String)))
        dtResult.Columns.Add(New DataColumn("noTran", GetType(Integer)))
        dtResult.Columns.Add(New DataColumn("vouchersClaimed", GetType(Integer)))
        'I-CRP15-002 (Fix the display problem of total voucher amount (RMB) in reimbursement UI) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'dtResult.Columns.Add(New DataColumn("totalAmount", GetType(Single)))
        dtResult.Columns.Add(New DataColumn("totalAmount", GetType(Decimal)))
        'dtResult.Columns.Add(New DataColumn("TotalAmountRMB", GetType(Single))) ' CRE13-019-02 Extend HCVS to China [Lawrence]
        dtResult.Columns.Add(New DataColumn("TotalAmountRMB", GetType(Decimal)))
        'I-CRP15-002 (Fix the display problem of total voucher amount (RMB) in reimbursement UI) [End][Chris YIM]


        Try
            Dim dt As New DataTable

            If bWithFirstAuthorization Then
                ' For Authorised_Status = '1'
                Dim prams() As SqlParameter = {db.MakeInParam("@sp_id", SqlDbType.Char, 8, criteria.ServiceProviderID), _
                                                db.MakeInParam("@practice_display_seq", SqlDbType.SmallInt, 2, criteria.SPPracticeDisplaySeq), _
                                                db.MakeInParam("@first_authorized_dtm", SqlDbType.DateTime, 8, criteria.FirstAuthorizedDate), _
                                                db.MakeInParam("@first_authorized_by", SqlDbType.VarChar, 20, criteria.FirstAuthorizedBy), _
                                                db.MakeInParam("@scheme_code", SqlDbType.Char, 10, strSchemeCode)}

                db.RunProc("proc_ReimbursementSummaryBank_get_bySPFirstAuth", prams, dt)
            Else
                ' For Authorised_Status = 'P' or '2'
                Dim prams() As SqlParameter = {db.MakeInParam("@sp_id", SqlDbType.Char, 8, criteria.ServiceProviderID), _
                                                db.MakeInParam("@practice_display_seq", SqlDbType.SmallInt, 2, criteria.SPPracticeDisplaySeq), _
                                                db.MakeInParam("@cutoff_dtm", SqlDbType.DateTime, 8, criteria.CutoffDate), _
                                                db.MakeInParam("@scheme_code", SqlDbType.Char, 10, strSchemeCode), _
                                                db.MakeInParam("@authorised_status", SqlDbType.Char, 1, strAuthorisedStatus)}

                db.RunProc("proc_ReimbursementSummaryBank_get_bySP", prams, dt)
            End If

            For j As Integer = 0 To dt.Rows.Count - 1
                Dim drResult As DataRow = dtResult.NewRow()
                drResult("blockNo") = j + 1
                drResult("bankAccount") = dt.Rows(j)("bankAccountid")
                drResult("practice") = dt.Rows(j)("practiceid")
                drResult("noTran") = dt.Rows(j)("noTran")
                drResult("vouchersClaimed") = dt.Rows(j)("vouchersClaimed")
                drResult("totalAmount") = dt.Rows(j)("totalAmount")
                drResult("TotalAmountRMB") = dt.Rows(j)("TotalAmountRMB") ' CRE13-019-02 Extend HCVS to China [Lawrence]
                dtResult.Rows.Add(drResult)
            Next

            Return dtResult
        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    ''' <summary>
    ''' Get the transactions based on the search criteria for the reimbursement
    ''' </summary>
    ''' <param name="criteria"></param>
    ''' <returns>Data table for the Reimbursement transactions</returns>
    ''' <remarks></remarks>
    Public Function SearchAuthorizationSummaryByTxn(ByVal criteria As Common.SearchCriteria.SearchCriteria, ByVal strAuthorisedStatus As Object, ByVal strSchemeCode As String, ByVal bWithFirstAuthorization As Boolean) As DataTable
        Dim i As Integer
        Dim resultCount As Integer
        Dim dtall As New DataTable
        Dim drxml As DataRow
        Dim dt As DataTable
        Dim dr As DataRow
        Dim deleteIndex As New ArrayList
        Dim strStatusDesc As String = ""
        Dim strDummy As String = ""

        'dtall = SearchTxn(criteria)

        dt = New DataTable()
        dt.Columns.Add(New DataColumn("lineNum", GetType(Integer)))
        dt.Columns.Add(New DataColumn("selected", GetType(CheckBox)))
        dt.Columns.Add(New DataColumn("transNum", GetType(String)))
        dt.Columns.Add(New DataColumn("transDate", GetType(Date)))
        dt.Columns.Add(New DataColumn("serviceProvider", GetType(String)))
        dt.Columns.Add(New DataColumn("spID", GetType(String)))
        dt.Columns.Add(New DataColumn("bankAccount", GetType(String)))
        dt.Columns.Add(New DataColumn("practice", GetType(String)))
        dt.Columns.Add(New DataColumn("voucherRedeem", GetType(Integer)))
        'dt.Columns.Add(New DataColumn("voucherValue", GetType(Integer)))
        'I-CRP15-002 (Fix the display problem of total voucher amount (RMB) in reimbursement UI) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'dt.Columns.Add(New DataColumn("totalAmount", GetType(Single)))
        dt.Columns.Add(New DataColumn("totalAmount", GetType(Decimal)))
        'dt.Columns.Add(New DataColumn("TotalAmountRMB", GetType(Single))) ' CRE13-019-02 Extend HCVS to China [Lawrence]
        dt.Columns.Add(New DataColumn("TotalAmountRMB", GetType(Decimal)))
        'I-CRP15-002 (Fix the display problem of total voucher amount (RMB) in reimbursement UI) [End][Chris YIM]
        dt.Columns.Add(New DataColumn("transStatus", GetType(String)))

        Try
            If bWithFirstAuthorization Then
                ' For Authorised_Status = '1'
                Dim prams() As SqlParameter = {db.MakeInParam("@sp_id", SqlDbType.Char, 8, criteria.ServiceProviderID), _
                                                db.MakeInParam("@practice_display_seq", SqlDbType.SmallInt, 2, criteria.SPPracticeDisplaySeq), _
                                                db.MakeInParam("@bank_acc_no", SqlDbType.Char, 30, criteria.BankAcctNo), _
                                                db.MakeInParam("@cutoff_dtm", SqlDbType.DateTime, 8, criteria.CutoffDate), _
                                                db.MakeInParam("@first_authorized_dtm", SqlDbType.DateTime, 8, criteria.FirstAuthorizedDate), _
                                                db.MakeInParam("@first_authorized_by", SqlDbType.VarChar, 20, criteria.FirstAuthorizedBy), _
                                                db.MakeInParam("@scheme_code", SqlDbType.Char, 10, strSchemeCode)}

                db.RunProc("proc_ReimbursementSummaryTxn_get_bySPCutoffStatusBankFirstAuth", prams, dtall)
            Else
                ' For Authorised_Status = 'P' or '2'
                Dim prams() As SqlParameter = {db.MakeInParam("@sp_id", SqlDbType.Char, 8, criteria.ServiceProviderID), _
                                                db.MakeInParam("@practice_display_seq", SqlDbType.SmallInt, 2, criteria.SPPracticeDisplaySeq), _
                                                db.MakeInParam("@bank_acc_no", SqlDbType.Char, 30, criteria.BankAcctNo), _
                                                db.MakeInParam("@cutoff_dtm", SqlDbType.DateTime, 8, criteria.CutoffDate), _
                                                db.MakeInParam("@scheme_code", SqlDbType.Char, 10, strSchemeCode), _
                                                db.MakeInParam("@authorised_status", SqlDbType.Char, 1, strAuthorisedStatus)}

                db.RunProc("proc_ReimbursementSummaryTxn_get_bySPCutoffStatusBank", prams, dtall)
            End If

        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
        End Try

        resultCount = 0

        For i = 1 To dtall.Rows.Count
            drxml = CType(dtall.Rows(i - 1), DataRow)

            resultCount = resultCount + 1
            dr = dt.NewRow()
            dr("lineNum") = resultCount
            'dr("transNum") = df.formatSystemNumber(Trim(drxml("tranNum")))
            dr("transNum") = Trim(drxml("tranNum"))
            dr("transDate") = drxml("tranDate")
            dr("serviceProvider") = drxml("SPName")
            dr("spID") = drxml("SPID")
            dr("bankAccount") = drxml("bankAccountID")
            dr("practice") = drxml("practiceID")
            dr("voucherRedeem") = drxml("voucherRedeem")
            'dr("voucherValue") = 0 'drxml("voucherAmount")
            dr("totalAmount") = drxml("totalAmount")
            dr("TotalAmountRMB") = drxml("TotalAmountRMB") ' CRE13-019-02 Extend HCVS to China [Lawrence]
            If IsDBNull(drxml("authorised_status")) Then
                ClaimTransStatus.GetDescriptionFromDBCode(ClaimTransStatus.ClassCode, drxml("status"), strStatusDesc, strDummy)
                dr("transStatus") = strStatusDesc
            Else
                ReimbursementStatus.GetDescriptionFromDBCode(ReimbursementStatus.ClassCode, drxml("authorised_status"), strStatusDesc, strDummy)
                dr("transStatus") = strStatusDesc
            End If

            dt.Rows.Add(dr)
        Next

        Return dt
    End Function

    ''' <summary>
    ''' Get the transactions based on the search criteria for the reimbursement by service provider id and name
    ''' </summary>
    ''' <returns>Data table for the Reimbursement Authorization Summary by service provider id and name</returns>
    ''' <remarks></remarks>
    Public Function SearchAuthorizationSummaryBySP_PaymentFile(ByVal strReimburseID As String, ByVal strSchemeCode As String) As DataTable
        Dim dt, dtall As New DataTable
        Dim dr, drall As DataRow
        Dim i, j As Integer
        Dim spList, spIDList, spKeyList, practiceNo As New ArrayList

        Dim arrTotalVoucher, arrTotalAmount, arrTotalTran As New ArrayList

        Dim totalVoucher, totalAmount As Double
        Dim totalTran As Integer

        totalVoucher = 0
        totalAmount = 0
        totalTran = 0

        dt = New DataTable()

        dt.Columns.Add(New DataColumn("lineNum", GetType(Integer)))
        dt.Columns.Add(New DataColumn("spNum", GetType(String)))
        dt.Columns.Add(New DataColumn("spName", GetType(String)))
        dt.Columns.Add(New DataColumn("noTran", GetType(Integer)))
        dt.Columns.Add(New DataColumn("vouchersClaimed", GetType(Integer)))
        'I-CRP15-002 (Fix the display problem of total voucher amount (RMB) in reimbursement UI) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'dt.Columns.Add(New DataColumn("totalAmount", GetType(Single)))
        dt.Columns.Add(New DataColumn("totalAmount", GetType(Decimal)))
        'dt.Columns.Add(New DataColumn("totalAmountRMB", GetType(Single))) ' CRE13-019-02 Extend HCVS to China [Lawrence]
        dt.Columns.Add(New DataColumn("totalAmountRMB", GetType(Decimal)))
        'I-CRP15-002 (Fix the display problem of total voucher amount (RMB) in reimbursement UI) [End][Chris YIM]


        Try

            ' create data object and params
            Dim prams() As SqlParameter = {db.MakeInParam("@reimburse_id", SqlDbType.Char, 15, strReimburseID), _
                                            db.MakeInParam("@scheme_code", SqlDbType.Char, 10, strSchemeCode)}

            ' run the stored procedure
            db.RunProc("proc_ReimbursementSummarySP_get_byBankForPaymentFile", prams, dtall)

            For j = 0 To dtall.Rows.Count - 1
                dr = dt.NewRow()
                dr("lineNum") = j + 1
                dr("spNum") = dtall.Rows(j)("spID") & " (" & dtall.Rows(j)("practiceNo") & ")"
                dr("spName") = dtall.Rows(j)("spName")
                dr("noTran") = dtall.Rows(j)("noTran")
                dr("vouchersClaimed") = dtall.Rows(j)("vouchersClaimed")
                dr("totalAmount") = dtall.Rows(j)("totalAmount")
                dr("totalAmountRMB") = dtall.Rows(j)("totalAmountRMB") ' CRE13-019-02 Extend HCVS to China [Lawrence]
                dt.Rows.Add(dr)
            Next

            Return dt
        Catch eSQL As SqlException
            db.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            db.RollBackTranscation()
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Get the transactions based on the search criteria for the reimbursement by bank account and practice
    ''' </summary>
    ''' <param name="criteria"></param>
    ''' <returns>Data table for the Reimbursement Authorization Summary by bank account and practice</returns>
    ''' <remarks></remarks>
    Public Function SearchAuthorizationSummaryByBankAcct_PaymentFile(ByVal criteria As Common.SearchCriteria.SearchCriteria, ByVal strReimburseID As String, ByVal strSchemeCode As String) As DataTable
        Dim dt, dtall As New DataTable
        Dim dr As DataRow
        Dim j As Integer
        Dim bankAccList, practiceList As New ArrayList

        Dim totalVoucher, totalAmount As Double
        Dim totalTran As Integer
        Dim deleteIndex As ArrayList

        deleteIndex = New ArrayList
        totalVoucher = 0
        totalAmount = 0
        totalTran = 0

        dt = New DataTable()

        dt.Columns.Add(New DataColumn("blockNo", GetType(String)))
        dt.Columns.Add(New DataColumn("bankAccount", GetType(String)))
        dt.Columns.Add(New DataColumn("practice", GetType(String)))
        dt.Columns.Add(New DataColumn("noTran", GetType(Integer)))
        dt.Columns.Add(New DataColumn("vouchersClaimed", GetType(Integer)))
        'I-CRP15-002 (Fix the display problem of total voucher amount (RMB) in reimbursement UI) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'dt.Columns.Add(New DataColumn("totalAmount", GetType(Single)))
        dt.Columns.Add(New DataColumn("totalAmount", GetType(Decimal)))
        'dt.Columns.Add(New DataColumn("totalAmountRMB", GetType(Single))) ' CRE13-019-02 Extend HCVS to China [Lawrence]
        dt.Columns.Add(New DataColumn("totalAmountRMB", GetType(Decimal)))
        'I-CRP15-002 (Fix the display problem of total voucher amount (RMB) in reimbursement UI) [End][Chris YIM]


        Try

            ' create data object and params
            Dim prams() As SqlParameter = {db.MakeInParam("@reimburse_id", SqlDbType.Char, 15, strReimburseID.Trim), _
                                            db.MakeInParam("@sp_id", SqlDbType.Char, 8, criteria.ServiceProviderID), _
                                            db.MakeInParam("@practice_display_seq", SqlDbType.SmallInt, 2, criteria.SPPracticeDisplaySeq), _
                                            db.MakeInParam("@scheme_code", SqlDbType.Char, 10, strSchemeCode)}

            ' run the stored procedure
            db.RunProc("proc_ReimbursementSummaryBank_get_bySPForPaymentFile", prams, dtall)

            For j = 0 To dtall.Rows.Count - 1
                dr = dt.NewRow()
                dr("blockNo") = j + 1
                dr("bankAccount") = dtall.Rows(j)("bankAccountid")
                dr("practice") = dtall.Rows(j)("practiceid")
                dr("noTran") = dtall.Rows(j)("noTran")
                dr("vouchersClaimed") = dtall.Rows(j)("vouchersClaimed")
                dr("totalAmount") = dtall.Rows(j)("totalAmount")
                dr("totalAmountRMB") = dtall.Rows(j)("totalAmountRMB") ' CRE13-019-02 Extend HCVS to China [Lawrence]
                dt.Rows.Add(dr)
            Next

            Return dt
        Catch eSQL As SqlException
            db.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            db.RollBackTranscation()
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Get the transactions based on the search criteria for the reimbursement
    ''' </summary>
    ''' <param name="criteria"></param>
    ''' <returns>Data table for the Reimbursement transactions</returns>
    ''' <remarks></remarks>
    Public Function SearchAuthorizationSummaryByTxn_PaymentFile(ByVal criteria As Common.SearchCriteria.SearchCriteria, ByVal strReimburseID As String, ByVal strSchemeCode As String) As DataTable
        Dim i As Integer
        Dim resultCount As Integer
        Dim dtall As New DataTable
        Dim drxml As DataRow
        Dim dt As DataTable
        Dim dr As DataRow
        Dim deleteIndex As New ArrayList
        Dim strStatusDesc As String = ""
        Dim strDummy As String = ""

        dt = New DataTable()
        dt.Columns.Add(New DataColumn("lineNum", GetType(Integer)))
        dt.Columns.Add(New DataColumn("selected", GetType(CheckBox)))
        dt.Columns.Add(New DataColumn("transNum", GetType(String)))
        dt.Columns.Add(New DataColumn("transDate", GetType(Date)))
        dt.Columns.Add(New DataColumn("serviceProvider", GetType(String)))
        dt.Columns.Add(New DataColumn("spID", GetType(String)))
        dt.Columns.Add(New DataColumn("bankAccount", GetType(String)))
        dt.Columns.Add(New DataColumn("practice", GetType(String)))
        dt.Columns.Add(New DataColumn("voucherRedeem", GetType(Integer)))
        dt.Columns.Add(New DataColumn("voucherValue", GetType(Integer)))
        'I-CRP15-002 (Fix the display problem of total voucher amount (RMB) in reimbursement UI) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'dt.Columns.Add(New DataColumn("totalAmount", GetType(Single)))
        dt.Columns.Add(New DataColumn("totalAmount", GetType(Decimal)))
        'dt.Columns.Add(New DataColumn("totalAmountRMB", GetType(Single))) ' CRE13-019-02 Extend HCVS to China [Lawrence]
        dt.Columns.Add(New DataColumn("totalAmountRMB", GetType(Decimal)))
        'I-CRP15-002 (Fix the display problem of total voucher amount (RMB) in reimbursement UI) [End][Chris YIM]
        dt.Columns.Add(New DataColumn("transStatus", GetType(String)))

        Try
            Dim prams() As SqlParameter = { _
                                            db.MakeInParam("@reimburse_id", SqlDbType.Char, 15, strReimburseID.Trim), _
                                            db.MakeInParam("@sp_id", SqlDbType.Char, 8, criteria.ServiceProviderID.Trim), _
                                            db.MakeInParam("@practice_display_seq", SqlDbType.SmallInt, 4, CInt(criteria.SPPracticeDisplaySeq.Trim)), _
                                            db.MakeInParam("@bank_acc_no", SqlDbType.VarChar, 30, criteria.BankAcctNo), _
                                            db.MakeInParam("@scheme_code", SqlDbType.Char, 10, strSchemeCode) _
                                            }

            db.RunProc("proc_ReimbursementSummaryTxn_get_byCutoffForPaymentFile", prams, dtall)

        Catch eSQL As SqlException
            db.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            db.RollBackTranscation()
            Throw ex
        End Try

        resultCount = 0

        For i = 1 To dtall.Rows.Count
            drxml = CType(dtall.Rows(i - 1), DataRow)

            resultCount = resultCount + 1
            dr = dt.NewRow()
            dr("lineNum") = resultCount
            'dr("transNum") = df.formatSystemNumber(Trim(drxml("tranNum")))
            dr("transNum") = Trim(drxml("tranNum"))
            dr("transDate") = drxml("tranDate")
            dr("serviceProvider") = drxml("SPName")
            dr("spID") = drxml("SPID")
            dr("bankAccount") = drxml("bankAccountID")  '0041234567
            dr("practice") = drxml("practiceID")    'Quality Health Care
            dr("voucherRedeem") = drxml("voucherRedeem")
            dr("voucherValue") = drxml("voucherAmount")
            'dr("totalAmount") = dr("voucherRedeem") * dr("voucherValue")
            dr("totalAmount") = drxml("totalAmount")
            dr("totalAmountRMB") = drxml("totalAmountRMB") ' CRE13-019-02 Extend HCVS to China [Lawrence]

            If IsDBNull(drxml("authorised_status")) Then
                ClaimTransStatus.GetDescriptionFromDBCode(ClaimTransStatus.ClassCode, drxml("status"), strStatusDesc, strDummy)
                dr("transStatus") = strStatusDesc
            Else
                ReimbursementStatus.GetDescriptionFromDBCode(ReimbursementStatus.ClassCode, drxml("authorised_status"), strStatusDesc, strDummy)
                dr("transStatus") = strStatusDesc
            End If

            dt.Rows.Add(dr)
        Next

        Return dt
    End Function

    ''' <summary>
    ''' Get the transaction for displaying the transaction detail page. Will join the trasnaction and voucher account information table
    ''' </summary>
    ''' <param name="criteria"></param>
    ''' <returns>Datarow with information from transaction and voucher account tables</returns>
    ''' <remarks></remarks>
    Public Function GetSingleTxnRow(ByVal criteria As Common.SearchCriteria.SearchCriteria, ByVal strSchemeCode As String) As DataRow
        Dim dt As New DataTable
        Dim db As New Common.DataAccess.Database

        Try
            ' create data object and params
            Dim prams() As SqlParameter = {db.MakeInParam("@tran_id", SqlDbType.Char, 20, criteria.TransNum), _
                                            db.MakeInParam("@scheme_code", SqlDbType.Char, 10, strSchemeCode)}

            ' run the stored procedure
            db.RunProc("proc_ReimbursementSummaryTxnRow_get_byTranID", prams, dt)
        Catch eSQL As SqlException
            db.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            db.RollBackTranscation()
            Throw ex
        End Try

        Return dt.Rows(0)

    End Function

    ''' <summary>
    ''' Get the transactions based on the search criteria for the reimbursement by first authorization date and user
    ''' </summary>
    ''' <param name="criteria"></param>
    ''' <returns>Data table for the Reimbursement Authorization Summary by first authorization date and user</returns>
    ''' <remarks></remarks>
    Public Function SearchAuthorizationSummaryByFirstAuthorization(ByVal criteria As Common.SearchCriteria.SearchCriteria, ByVal strUserID As String) As DataTable
        Dim dt, dtall As New DataTable
        Dim dr As DataRow
        Dim j As Integer
        Dim reimburseIDList, v1PairList, v1DateList, v1ByList As New ArrayList

        Dim totalVoucher, totalAmount As Double
        Dim totalTran As Integer

        totalVoucher = 0
        totalAmount = 0
        totalTran = 0

        dt = New DataTable()

        dt.Columns.Add(New DataColumn("lineNum", GetType(Integer)))
        dt.Columns.Add(New DataColumn("reimburseID", GetType(String)))
        dt.Columns.Add(New DataColumn("v1Date", GetType(String)))
        dt.Columns.Add(New DataColumn("v1By", GetType(String)))
        dt.Columns.Add(New DataColumn("noTran", GetType(Integer)))
        dt.Columns.Add(New DataColumn("noSP", GetType(Integer)))
        dt.Columns.Add(New DataColumn("vouchersClaimed", GetType(Integer)))
        'I-CRP15-002 (Fix the display problem of total voucher amount (RMB) in reimbursement UI) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'dt.Columns.Add(New DataColumn("totalAmount", GetType(Single)))
        dt.Columns.Add(New DataColumn("totalAmount", GetType(Decimal)))
        'dt.Columns.Add(New DataColumn("TotalAmountRMB", GetType(Single))) ' CRE13-019-02 Extend HCVS to China [Lawrence]
        dt.Columns.Add(New DataColumn("TotalAmountRMB", GetType(Decimal)))
        'I-CRP15-002 (Fix the display problem of total voucher amount (RMB) in reimbursement UI) [End][Chris YIM]
        dt.Columns.Add(New DataColumn("Scheme_Code", GetType(String)))
        dt.Columns.Add(New DataColumn("Display_Code", GetType(String)))

        Try
            ' create data object and params
            Dim prams() As SqlParameter = {db.MakeInParam("@tran_status", SqlDbType.Char, 1, criteria.TransStatus), _
            db.MakeInParam("@user_id", SqlDbType.VarChar, 20, strUserID)}
            ' run the stored procedure
            'db.RunProc("proc_ReimbursementSummary1stAuthorisation_get_byCutoffStatus", prams, dtall)
            db.RunProc("proc_ReimbursementSummary1stAuthorisation_get_byStatus", prams, dtall)

            For j = 0 To dtall.Rows.Count - 1
                dr = dt.NewRow()
                dr("lineNum") = j + 1
                dr("reimburseID") = dtall.Rows(j)("reimburseID")
                dr("v1By") = dtall.Rows(j)("firstAuthorizedBy")
                dr("v1Date") = dtall.Rows(j)("firstAuthorizedDate")
                dr("noTran") = dtall.Rows(j)("noTran")
                dr("noSP") = dtall.Rows(j)("noSP")
                dr("vouchersClaimed") = dtall.Rows(j)("voucherRedeem")
                dr("totalAmount") = dtall.Rows(j)("voucherAmount")
                dr("TotalAmountRMB") = dtall.Rows(j)("TotalAmountRMB") ' CRE13-019-02 Extend HCVS to China [Lawrence]
                dr("Scheme_Code") = dtall.Rows(j)("Scheme_Code")
                dr("Display_Code") = dtall.Rows(j)("Display_Code")
                dt.Rows.Add(dr)
            Next

            Return dt
        Catch eSQL As SqlException
            db.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            db.RollBackTranscation()
            Throw ex
        End Try


    End Function

    ''' <summary>
    ''' Get the transaction ready for generate the payment file in the format of the grid view
    ''' </summary>
    ''' <param name="dtmStartDate"></param>
    ''' <param name="dtmCutoffDate"></param>
    ''' <param name="strUserID"></param>
    ''' <returns>Data table with the required format</returns>
    ''' <remarks></remarks>
    Public Function SearchReimbursementPaymentList(ByVal strReimbursementID As String, ByVal dtmStartDate As Nullable(Of DateTime), ByVal dtmCutoffDate As Nullable(Of DateTime), ByVal strUserID As String) As DataTable
        Dim dtResult As New DataTable
        dtResult.Columns.Add(New DataColumn("lineNum", GetType(Integer)))
        dtResult.Columns.Add(New DataColumn("fileIconURL", GetType(String)))
        dtResult.Columns.Add(New DataColumn("filePath", GetType(String)))
        dtResult.Columns.Add(New DataColumn("createDate", GetType(Date)))
        dtResult.Columns.Add(New DataColumn("reimburseID", GetType(String)))
        dtResult.Columns.Add(New DataColumn("noTran", GetType(Integer)))
        dtResult.Columns.Add(New DataColumn("vouchersClaimed", GetType(Integer)))
        'I-CRP15-002 (Fix the display problem of total voucher amount (RMB) in reimbursement UI) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'dtResult.Columns.Add(New DataColumn("totalAmount", GetType(Single)))
        dtResult.Columns.Add(New DataColumn("totalAmount", GetType(Decimal)))
        'dtResult.Columns.Add(New DataColumn("totalAmountRMB", GetType(Single))) ' CRE13-019-02 Extend HCVS to China [Lawrence]
        dtResult.Columns.Add(New DataColumn("totalAmountRMB", GetType(Decimal)))
        'I-CRP15-002 (Fix the display problem of total voucher amount (RMB) in reimbursement UI) [End][Chris YIM]


        dtResult.Columns.Add(New DataColumn("completionTime", GetType(Date)))
        dtResult.Columns.Add(New DataColumn("Scheme_Code", GetType(String)))
        dtResult.Columns.Add(New DataColumn("Display_Code", GetType(String)))
        dtResult.Columns.Add(New DataColumn("Verification_Case_Available", GetType(String))) ' CRE17-004 Generate a new DPAR on EHCP basis [Dickson]

        Dim dt As New DataTable

        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
        Dim objReimburseID As Object = DBNull.Value
        If strReimbursementID <> String.Empty Then objReimburseID = strReimbursementID

        Dim objStartDtm As Object = DBNull.Value
        If dtmStartDate.HasValue Then objStartDtm = dtmStartDate.Value

        Dim objCutoffDtm As Object = DBNull.Value
        If dtmCutoffDate.HasValue Then objCutoffDtm = dtmCutoffDate.Value

        Dim prams() As SqlParameter = { _
            db.MakeInParam("@submitted_by", SqlDbType.VarChar, 20, strUserID), _
            db.MakeInParam("@Reimburse_ID", SqlDbType.Char, 15, objReimburseID), _
            db.MakeInParam("@start_dtm", SqlDbType.DateTime, 8, objStartDtm), _
            db.MakeInParam("@cutoff_dtm", SqlDbType.DateTime, 8, objCutoffDtm) _
        }
        ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

        db.RunProc("proc_BankIn_get_byStartCutoffDate", prams, dt)
    
        For i As Integer = 0 To dt.Rows.Count - 1
            Dim dr As DataRow = dt.Rows(i)

            Dim drResult As DataRow = dtResult.NewRow

            drResult("lineNum") = i + 1

            If IsDBNull(dr("filePath")) Then
                drResult("fileIconURL") = "~/Images/others/processing.png"
                drResult("completionTime") = DBNull.Value
            Else
                drResult("fileIconURL") = "~/Images/others/floopy.png"
                drResult("completionTime") = dr("completionTime")
            End If

            drResult("filePath") = "../ReportAndDownload/Datadownload.aspx"
            drResult("createDate") = dr("createDate")
            drResult("reimburseID") = dr("reimburseID")
            drResult("noTran") = dr("noTran")
            drResult("vouchersClaimed") = dr("vouchersClaimed")
            drResult("totalAmount") = dr("totalAmount")
            drResult("totalAmountRMB") = dr("totalAmountRMB") ' CRE13-019-02 Extend HCVS to China [Lawrence]
            drResult("Scheme_Code") = dr("Scheme_Code")
            drResult("Display_Code") = dr("Display_Code")
            drResult("Verification_Case_Available") = dr("Verification_Case_Available") ' CRE17-004 Generate a new DPAR on EHCP basis [Dickson]

            dtResult.Rows.Add(drResult)
        Next

        Return dtResult
    End Function

    ''' <summary>
    ''' Get the transaction from transaction table based on the general search criteria
    ''' </summary>
    ''' <param name="criteria"></param>
    ''' <returns>Data table with the information required</returns>
    ''' <remarks></remarks>
    Public Function SearchTxn(ByVal criteria As Common.SearchCriteria.SearchCriteria) As DataTable
        Dim ds As New DataSet
        Dim db As New Common.DataAccess.Database
        Dim dtxml As DataTable
        Dim drxml As DataRow
        Dim i As Integer
        Dim deleteIndex As New ArrayList

        'Call DB store proc
        CallDBProc(criteria.ServiceProviderID, criteria.ServiceProviderHKIC, criteria.ServiceProviderName, criteria.ServiceProviderProfRegNo, criteria.HealthProf, _
                criteria.EnrolmentRefNo, criteria.BankAcctNo, criteria.Practice, criteria.BankAccountOwner, criteria.BankName, criteria.BranchName, criteria.BankAcctSubmissionDate, _
                criteria.BankStatus, criteria.FromDate, criteria.CutoffDate, criteria.TransStatus, criteria.TransNum, criteria.VoucherRecipientHKIC, criteria.VoucherRecipientName)

        db.RunProc("GetTxn", ds)

        dtxml = ds.Tables(0)

        For i = 1 To dtxml.Rows.Count
            drxml = CType(dtxml.Rows(i - 1), DataRow)

            If Not IsMatchSearchCriteria(criteria, drxml) Then
                deleteIndex.Add(i - 1)
            End If
        Next

        'remove the not matched entry
        For i = deleteIndex.Count - 1 To 0 Step -1
            dtxml.Rows(CInt(deleteIndex(i))).Delete()
        Next
        dtxml.AcceptChanges()

        Return dtxml

    End Function

    ''' <summary>
    ''' Search the bank records to be verified / void based on the search criteria
    ''' </summary>
    ''' <param name="criteria"></param>
    ''' <returns>Data table for the bank records to be verificed / void</returns>
    ''' <remarks></remarks>

    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
    'Public Function SearchBank(ByVal criteria As Common.SearchCriteria.SearchCriteria, ByVal strContactNo As String, ByVal strSchemeCode As String) As DataTable
    Public Function SearchBank(ByVal strFunctionCode As String, ByVal criteria As Common.SearchCriteria.SearchCriteria, ByVal strContactNo As String, ByVal strSchemeCode As String, ByVal blnOverrideResultLimit As Boolean) As BaseBLL.BLLSearchResult
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
        Dim db As New Common.DataAccess.Database
        Dim dtall As New DataTable
        Dim drall As DataRow
        Dim i, rowindex As Integer
        Dim deleteIndex As New ArrayList
        Dim strStatusDesc As String = ""
        Dim strDummy As String = ""

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
        ' -------------------------------------------------------------------------
        Dim udtBLLSearchResult As BaseBLL.BLLSearchResult
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

        Try
            ' create data object and params
            Dim prams() As SqlParameter = {db.MakeInParam("@record_status", SqlDbType.Char, 1, criteria.BankStatus), _
                                            db.MakeInParam("@enrolment_ref_no", SqlDbType.Char, 15, criteria.EnrolmentRefNo), _
                                            db.MakeInParam("@sp_id", SqlDbType.Char, 8, criteria.ServiceProviderID), _
                                            db.MakeInParam("@sp_hkid", SqlDbType.Char, 9, criteria.ServiceProviderHKIC), _
                                            db.MakeInParam("@sp_name", SqlDbType.VarChar, 40, criteria.ServiceProviderName), _
                                            db.MakeInParam("@service_type", SqlDbType.Char, 5, criteria.HealthProf), _
                                            db.MakeInParam("@contact_no", SqlDbType.VarChar, 20, strContactNo), _
                                            db.MakeInParam("@scheme_code", SqlDbType.VarChar, 100, strSchemeCode) _
                                           }

            'db.MakeInParam("@bank_acc_no", SqlDbType.VarChar, 30, criteria.BankAcctNo), _
            '                                db.MakeInParam("@bank_name", SqlDbType.NVarChar, 20, Trim(criteria.BankName)), _
            '                                db.MakeInParam("@branch_name", SqlDbType.NVarChar, 20, Trim(criteria.BranchName)), _

            'db.MakeInParam("@bank_submission_dtm", SqlDbType.DateTime, 8, DBNull.Value) _
            'IIf(isnull(criteria.BankAcctSubmissionDate), DBNull.Value, criteria.BankAcctSubmissionDate

            'db.MakeInParam("@bank_submission_dtm", SqlDbType.DateTime, 8, IIf(criteria.BankAcctSubmissionDate.Equals(String.Empty), Nothing, criteria.BankAcctSubmissionDate)) _

            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
            ' -------------------------------------------------------------------------
            ' run the stored procedure
            'db.RunProc("proc_BankAccountVerification_get_byAny", prams, dtall)

            udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_BankAccountVerification_get_byAny", prams, blnOverrideResultLimit, db)

            If udtBLLSearchResult.SqlErrorMessage = BaseBLL.EnumSqlErrorMessage.Normal Then
                dtall = CType(udtBLLSearchResult.Data, DataTable)
            Else
                udtBLLSearchResult.Data = Nothing
                Return udtBLLSearchResult
            End If
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

        Catch eSQL As SqlException
            db.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            db.RollBackTranscation()
            Throw ex
        End Try

        Dim dt As DataTable
        Dim dr As DataRow

        dt = New DataTable()
        dt.Columns.Add(New DataColumn("lineNum", GetType(Integer)))
        dt.Columns.Add(New DataColumn("selected", GetType(CheckBox)))
        dt.Columns.Add(New DataColumn("enrolRefNo", GetType(String)))
        dt.Columns.Add(New DataColumn("spName", GetType(String)))
        'dt.Columns.Add(New DataColumn("practiceName", GetType(String)))
        'dt.Columns.Add(New DataColumn("practiceType", GetType(String)))
        'dt.Columns.Add(New DataColumn("bankAccOwner", GetType(String)))
        'dt.Columns.Add(New DataColumn("bankName", GetType(String)))
        'dt.Columns.Add(New DataColumn("branchName", GetType(String)))
        'dt.Columns.Add(New DataColumn("bankAccNo", GetType(String)))
        'dt.Columns.Add(New DataColumn("busRegNo", GetType(String)))
        dt.Columns.Add(New DataColumn("bankStatus", GetType(String)))
        'dt.Columns.Add(New DataColumn("displaySeq", GetType(Integer)))
        'dt.Columns.Add(New DataColumn("tsmp", GetType(Byte())))
        'dt.Columns.Add(New DataColumn("BankAccVerTSMP", GetType(Byte())))
        'dt.Columns.Add(New DataColumn("BankAccStagingTSMP", GetType(Byte())))
        'dt.Columns.Add(New DataColumn("SP_Practice_Display_Seq", GetType(String)))
        dt.Columns.Add(New DataColumn("spChiName", GetType(String)))
        dt.Columns.Add(New DataColumn("SPID", GetType(String)))
        dt.Columns.Add(New DataColumn("SPHKID", GetType(String)))
        dt.Columns.Add(New DataColumn("DaytimeContact", GetType(String)))
        'dt.Columns.Add(New DataColumn("HealthProf", GetType(String)))
        dt.Columns.Add(New DataColumn("Vetting_Dtm", GetType(DateTime)))
        dt.Columns.Add(New DataColumn("Scheme_Code", GetType(String)))

        rowindex = 1

        For i = 1 To dtall.Rows.Count
            drall = CType(dtall.Rows(i - 1), DataRow)

            dr = dt.NewRow()
            dr("lineNum") = rowindex
            dr("enrolRefNo") = df.formatSystemNumber(drall("appNum"))

            dr("spName") = drall("spName")

            If IsDBNull(drall("spChiName")) Then
                dr("spChiName") = ""
            Else
                dr("spChiName") = udcFormater.formatChineseName(drall("spChiName"))
            End If

            'dr("practiceName") = drall("practiceName")
            'dr("practiceType") = drall("practiceType")
            'dr("bankAccOwner") = drall("bankAccountOwner")
            'dr("bankName") = drall("bankName")
            'dr("branchName") = drall("branchName")
            'dr("bankAccNo") = drall("bankAccountNum")
            'dr("busRegNo") = drall("businessRegNum")

            BankAcctVerifyStatus.GetDescriptionFromDBCode(BankAcctVerifyStatus.ClassCode, Trim(drall("bankStatus")), strStatusDesc, strDummy)
            dr("bankStatus") = strStatusDesc

            'BankAcctStagingStatus.GetDescriptionFromDBCode(BankAcctStagingStatus.ClassCode, Trim(drall("status")), strStatusDesc, strDummy)
            'dr("bankStatus") = strStatusDesc

            'dr("displaySeq") = drall("displaySeq")
            'dr("tsmp") = drall("SPAccTSMP")
            'dr("BankAccVerTSMP") = drall("BankAccVerTSMP")
            'dr("BankAccStagingTSMP") = drall("BankAccStagingTSMP")
            'dr("SP_Practice_Display_Seq") = drall("SP_Practice_Display_Seq")

            dr("SPID") = drall("SPID")
            dr("SPHKID") = drall("SPHKID")
            dr("DaytimeContact") = drall("DaytimeContact")
            'dr("HealthProf") = drall("HealthProf")
            dr("Vetting_Dtm") = drall("Vetting_Dtm")
            dr("Scheme_Code") = drall("Scheme_Code")
            dt.Rows.Add(dr)

            rowindex = rowindex + 1
        Next

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
        ' -------------------------------------------------------------------------
        'Return dt
        udtBLLSearchResult.Data = dt
        Return udtBLLSearchResult
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

    End Function

    Public Function SearchBankTSMP(ByVal strERN As String) As DataTable
        Dim db As New Common.DataAccess.Database
        Dim dtall As New DataTable
        Dim drall As DataRow
        Dim i, rowindex As Integer
        Dim deleteIndex As New ArrayList
        Dim strStatusDesc As String = ""
        Dim strDummy As String = ""

        Try
            ' create data object and params
            Dim prams() As SqlParameter = {db.MakeInParam("@enrolment_ref_no", SqlDbType.Char, 15, strERN) _
                                           }

            ' run the stored procedure
            db.RunProc("proc_BankAccountVerificationTimeStamp_get_byERN", prams, dtall)
        Catch eSQL As SqlException
            db.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            db.RollBackTranscation()
            Throw ex
        End Try

        Dim dt As DataTable
        Dim dr As DataRow

        dt = New DataTable()
        dt.Columns.Add(New DataColumn("lineNum", GetType(Integer)))
        dt.Columns.Add(New DataColumn("selected", GetType(CheckBox)))
        dt.Columns.Add(New DataColumn("enrolRefNo", GetType(String)))
        dt.Columns.Add(New DataColumn("spName", GetType(String)))
        dt.Columns.Add(New DataColumn("practiceName", GetType(String)))
        'dt.Columns.Add(New DataColumn("practiceType", GetType(String)))
        dt.Columns.Add(New DataColumn("bankAccOwner", GetType(String)))
        dt.Columns.Add(New DataColumn("bankName", GetType(String)))
        dt.Columns.Add(New DataColumn("branchName", GetType(String)))
        dt.Columns.Add(New DataColumn("bankAccNo", GetType(String)))
        dt.Columns.Add(New DataColumn("busRegNo", GetType(String)))
        dt.Columns.Add(New DataColumn("bankStatus", GetType(String)))
        dt.Columns.Add(New DataColumn("displaySeq", GetType(Integer)))
        dt.Columns.Add(New DataColumn("TSMP", GetType(Byte())))
        dt.Columns.Add(New DataColumn("BankAccVerTSMP", GetType(Byte())))
        dt.Columns.Add(New DataColumn("BankAccStagingTSMP", GetType(Byte())))
        dt.Columns.Add(New DataColumn("SP_Practice_Display_Seq", GetType(String)))
        dt.Columns.Add(New DataColumn("spChiName", GetType(String)))
        dt.Columns.Add(New DataColumn("SPID", GetType(String)))
        dt.Columns.Add(New DataColumn("SPHKID", GetType(String)))
        dt.Columns.Add(New DataColumn("DaytimeContact", GetType(String)))
        dt.Columns.Add(New DataColumn("HealthProf", GetType(String)))

        rowindex = 1

        For i = 1 To dtall.Rows.Count
            drall = CType(dtall.Rows(i - 1), DataRow)

            dr = dt.NewRow()
            dr("lineNum") = rowindex
            dr("enrolRefNo") = df.formatSystemNumber(drall("appNum"))

            dr("spName") = drall("spName")

            If IsDBNull(drall("spChiName")) Then
                dr("spChiName") = ""
            Else
                dr("spChiName") = udcFormater.formatChineseName(drall("spChiName"))
            End If

            dr("practiceName") = drall("practiceName")
            'dr("practiceType") = drall("practiceType")
            dr("bankAccOwner") = drall("bankAccountOwner")
            dr("bankName") = drall("bankName")
            dr("branchName") = drall("branchName")
            dr("bankAccNo") = drall("bankAccountNum")
            dr("busRegNo") = drall("businessRegNum")

            BankAcctVerifyStatus.GetDescriptionFromDBCode(BankAcctVerifyStatus.ClassCode, Trim(drall("bankStatus")), strStatusDesc, strDummy)
            dr("bankStatus") = strStatusDesc

            BankAcctStagingStatus.GetDescriptionFromDBCode(BankAcctStagingStatus.ClassCode, Trim(drall("status")), strStatusDesc, strDummy)
            dr("bankStatus") = strStatusDesc

            dr("displaySeq") = drall("displaySeq")
            dr("TSMP") = drall("TSMP")
            dr("BankAccVerTSMP") = drall("BankAccVerTSMP")
            dr("BankAccStagingTSMP") = drall("BankAccStagingTSMP")
            dr("SP_Practice_Display_Seq") = drall("SP_Practice_Display_Seq")

            dr("SPID") = drall("SPID")
            dr("SPHKID") = drall("SPHKID")
            dr("DaytimeContact") = drall("DaytimeContact")
            dr("HealthProf") = drall("HealthProf")

            dt.Rows.Add(dr)

            rowindex = rowindex + 1
        Next

        Return dt

    End Function

    ''' <summary>
    ''' Get the service provider information from service provider table with the give search criteria
    ''' </summary>
    ''' <param name="criteria"></param>
    ''' <returns>Data table with the service provider information</returns>
    ''' <remarks></remarks>
    Private Function SearchServiceProvider(ByVal criteria As Common.SearchCriteria.SearchCriteria) As DataTable
        Dim ds As New DataSet
        Dim db As New Common.DataAccess.Database
        Dim dtxml As DataTable
        Dim drxml As DataRow
        Dim i As Integer
        Dim deleteIndex As New ArrayList

        db.RunProc("GetServiceProvider", ds)

        dtxml = ds.Tables(0)

        For i = 1 To dtxml.Rows.Count
            drxml = CType(dtxml.Rows(i - 1), DataRow)

            If Not drxml.Item("checked").Equals("False") Then
                deleteIndex.Add(i - 1)
            End If
        Next

        'remove the not matched entry
        For i = deleteIndex.Count - 1 To 0 Step -1
            dtxml.Rows(CInt(deleteIndex(i))).Delete()
        Next
        dtxml.AcceptChanges()

        Return dtxml

    End Function

    ''' <summary>
    ''' Get the transactions for list view in Claim Transaction Management based on the search criteria
    ''' </summary>
    ''' <param name="criteria"></param>
    ''' <returns>Data table of transactions in the list view</returns>
    ''' <remarks></remarks>
    Public Function SearchTxnListView(ByVal criteria As Common.SearchCriteria.SearchCriteria) As DataTable
        Dim ds As New DataSet
        Dim db As New Common.DataAccess.Database

        db.RunProc("GetTxnList", ds)

        Dim i As Integer
        Dim resultCount As Integer
        Dim dtxml, dtnow As DataTable
        Dim drxml As DataRow
        Dim dt As DataTable
        Dim dr As DataRow

        dtxml = ds.Tables(0)
        dtnow = dtxml.Copy

        dt = New DataTable()
        dt.Columns.Add(New DataColumn("lineNum", GetType(Integer)))
        dt.Columns.Add(New DataColumn("selected", GetType(CheckBox)))
        dt.Columns.Add(New DataColumn("transNum", GetType(String)))
        dt.Columns.Add(New DataColumn("transDate", GetType(String)))
        dt.Columns.Add(New DataColumn("serviceProvider", GetType(String)))
        dt.Columns.Add(New DataColumn("spID", GetType(String)))
        dt.Columns.Add(New DataColumn("bankAccount", GetType(String)))
        dt.Columns.Add(New DataColumn("practice", GetType(String)))
        dt.Columns.Add(New DataColumn("voucherRedeem", GetType(Integer)))
        dt.Columns.Add(New DataColumn("voucherValue", GetType(Integer)))
        dt.Columns.Add(New DataColumn("totalAmount", GetType(Integer)))
        dt.Columns.Add(New DataColumn("transStatus", GetType(String)))

        resultCount = 0

        For i = 1 To dtxml.Rows.Count
            drxml = CType(dtxml.Rows(i - 1), DataRow)

            If IsMatchSearchCriteria(criteria, drxml) Then
                resultCount = resultCount + 1
                dr = dt.NewRow()
                dr("lineNum") = resultCount
                dr("transNum") = drxml("tranNum")
                dr("transDate") = df.formatDateTime(drxml("tranDate"), "E")
                dr("serviceProvider") = drxml("SPName")
                dr("spID") = drxml("SPID")
                dr("bankAccount") = drxml("bankAccountID")
                dr("practice") = drxml("practiceID")
                dr("voucherRedeem") = drxml("voucherRedeem")
                dr("voucherValue") = drxml("voucherAmount")
                'dr("totalAmount") = dr("voucherRedeem") * dr("voucherValue")
                dr("totalAmount") = drxml("totalAmount")
                dr("transStatus") = drxml("status")

                dt.Rows.Add(dr)
            End If
        Next

        Return dt

    End Function

    Public Function SearchLatestThreeReimbursement(ByVal strUserID As String) As DataTable
        Dim udtDB As New Database
        Dim dt As New DataTable

        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@submitted_by", SqlDbType.VarChar, 20, strUserID)}
            udtDB.RunProc("proc_BankIn_get", prams, dt)

        Catch ex As Exception
            Throw ex
        End Try

        Return dt

    End Function

    Public Function SearchVoucherTransactionClaimCreationApproval(ByVal udtSearchCriteria As Common.SearchCriteria.SearchCriteria, ByVal strUserID As String) As DataTable

        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("lineNum", GetType(Integer)))
        dt.Columns.Add(New DataColumn("selected", GetType(CheckBox)))
        dt.Columns.Add(New DataColumn("transNum", GetType(String)))
        dt.Columns.Add(New DataColumn("transDate", GetType(Date)))
        dt.Columns.Add(New DataColumn("serviceProvider", GetType(String)))
        dt.Columns.Add(New DataColumn("spChiName", GetType(String)))
        dt.Columns.Add(New DataColumn("spID", GetType(String)))
        dt.Columns.Add(New DataColumn("bankAccount", GetType(String)))
        dt.Columns.Add(New DataColumn("practice", GetType(String)))
        dt.Columns.Add(New DataColumn("voucherRedeem", GetType(Integer)))
        dt.Columns.Add(New DataColumn("totalAmount", GetType(Integer)))
        dt.Columns.Add(New DataColumn("authorizedStatus", GetType(String)))
        dt.Columns.Add(New DataColumn("transStatus", GetType(String)))
        dt.Columns.Add(New DataColumn("tsmp", GetType(Byte())))
        dt.Columns.Add(New DataColumn("scheme_code", GetType(String)))
        dt.Columns.Add(New DataColumn("display_code", GetType(String)))
        dt.Columns.Add(New DataColumn("Voucher_Acc_ID", GetType(String)))
        dt.Columns.Add(New DataColumn("Temp_Voucher_Acc_ID", GetType(String)))
        dt.Columns.Add(New DataColumn("Temp_Voucher_Acc_TSMP", GetType(Byte())))
        dt.Columns.Add(New DataColumn("Special_Acc_ID", GetType(String)))
        dt.Columns.Add(New DataColumn("Special_Acc_TSMP", GetType(Byte())))
        dt.Columns.Add(New DataColumn("Invalid_Acc_ID", GetType(String)))
        dt.Columns.Add(New DataColumn("Invalidation", GetType(String)))
        dt.Columns.Add(New DataColumn("Invalidation_TSMP", GetType(Byte())))

        Dim strIdentityNo1Formatted As String = udcFormater.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, udtSearchCriteria.DocumentNo1)
        Dim strIdentityNo2Formatted As String = String.Empty
        If udtSearchCriteria.DocumentNo2 <> String.Empty Then strIdentityNo2Formatted = udcFormater.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, udtSearchCriteria.DocumentNo2)

        Dim dtAll As New DataTable

        Try
            Dim prams() As SqlParameter = {db.MakeInParam("@scheme_code", SqlDbType.Char, 10, IIf(udcValidator.IsEmpty(udtSearchCriteria.SchemeCode), DBNull.Value, udtSearchCriteria.SchemeCode)), _
                                            db.MakeInParam("@user_id", SqlDbType.VarChar, 20, strUserID), _
                                            db.MakeInParam("@identity_no1", SqlDbType.VarChar, 20, IIf(udcValidator.IsEmpty(udtSearchCriteria.DocumentNo1), DBNull.Value, udtSearchCriteria.DocumentNo1)), _
                                            db.MakeInParam("@identity_no1_formatted", SqlDbType.VarChar, 20, IIf(udcValidator.IsEmpty(strIdentityNo1Formatted), DBNull.Value, strIdentityNo1Formatted)), _
                                            db.MakeInParam("@identity_no2", SqlDbType.VarChar, 20, IIf(udcValidator.IsEmpty(strIdentityNo2Formatted), DBNull.Value, strIdentityNo2Formatted)), _
                                            db.MakeInParam("@Invalidation", SqlDbType.Char, 1, IIf(udcValidator.IsEmpty(udtSearchCriteria.Invalidation), DBNull.Value, udtSearchCriteria.Invalidation))}

            db.RunProc("proc_VoucherTransaction_NonReimburseClaim_get_byStatus", prams, dtAll)

        Catch eSQL As SqlException
            db.RollBackTranscation()
            Throw eSQL

        Catch ex As Exception
            db.RollBackTranscation()
            Throw ex
        End Try

        For i As Integer = 0 To dtAll.Rows.Count - 1
            Dim drAll As DataRow = dtAll.Rows(i)

            Dim dr As DataRow = dt.NewRow()
            dr("lineNum") = i + 1
            dr("transNum") = Trim(drAll("tranNum"))
            dr("transDate") = drAll("tranDate")
            dr("serviceProvider") = drAll("SPName")
            If IsDBNull(drAll("SPChiName")) Then
                dr("spChiName") = ""
            Else
                dr("spChiName") = udcFormater.formatChineseName(drAll("SPChiName"))
            End If
            dr("spID") = drAll("SPID")
            dr("bankAccount") = drAll("BankAccountNo")
            dr("practice") = drAll("PracticeName")
            dr("voucherRedeem") = drAll("voucherRedeem")
            dr("totalAmount") = drAll("totalAmount")

            dr("transStatus") = drAll("status")

            If IsDBNull(drAll("Authorised_status")) Then
                dr("authorizedStatus") = ""
            Else
                dr("authorizedStatus") = drAll("Authorised_status")
            End If

            dr("tsmp") = drAll("tsmp")
            dr("scheme_code") = drAll("scheme_code")
            dr("display_code") = drAll("Display_Code")

            dr("Voucher_Acc_ID") = CStr(IIf(IsDBNull(drAll("Voucher_Acc_ID")), String.Empty, drAll("Voucher_Acc_ID"))).Trim
            dr("Temp_Voucher_Acc_ID") = CStr(IIf(IsDBNull(drAll("Temp_Voucher_Acc_ID")), String.Empty, drAll("Temp_Voucher_Acc_ID"))).Trim
            dr("Temp_Voucher_Acc_TSMP") = drAll("Temp_Voucher_Acc_TSMP")
            dr("Special_Acc_ID") = CStr(IIf(IsDBNull(drAll("Special_Acc_ID")), String.Empty, drAll("Special_Acc_ID"))).Trim
            dr("Special_Acc_TSMP") = drAll("Special_Acc_TSMP")
            dr("Invalid_Acc_ID") = CStr(IIf(IsDBNull(drAll("Invalid_Acc_ID")), String.Empty, drAll("Invalid_Acc_ID"))).Trim
            dr("Invalidation") = drAll("Invalidation")
            dr("Invalidation_TSMP") = drAll("Invalidation_TSMP")

            dt.Rows.Add(dr)

        Next

        Return dt

    End Function

    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
    'Public Function SearchVoucherTransactionManualReimbursedByStatus(ByVal udtSearchCriteria As Common.SearchCriteria.SearchCriteria, ByVal strUserID As String) As DataTable
    Public Function SearchVoucherTransactionManualReimbursedByStatus(ByVal strFunctionCode As String, ByVal udtSearchCriteria As Common.SearchCriteria.SearchCriteria, ByVal strUserID As String, ByVal blnOverrideResultLimit As Boolean) As BaseBLL.BLLSearchResult
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
        Dim dt As New DataTable

        dt.Columns.Add(New DataColumn("lineNum", GetType(Integer)))
        dt.Columns.Add(New DataColumn("selected", GetType(CheckBox)))
        dt.Columns.Add(New DataColumn("transNum", GetType(String)))
        dt.Columns.Add(New DataColumn("transDate", GetType(Date)))
        dt.Columns.Add(New DataColumn("serviceProvider", GetType(String)))
        dt.Columns.Add(New DataColumn("spChiName", GetType(String)))
        dt.Columns.Add(New DataColumn("spID", GetType(String)))

        'CRE12-015 - Add the respective practice number in ．Practice・ in the functions under ．Reimbursement・ in eHS [Start] [Tommy Tse]

        dt.Columns.Add(New DataColumn("practiceid", GetType(String)))

        dt.Columns.Add(New DataColumn("spid_practiceid", GetType(String)))

        'CRE12-015 - Add the respective practice number in ．Practice・ in the functions under ．Reimbursement・ in eHS [End] [Tommy Tse]

        dt.Columns.Add(New DataColumn("bankAccount", GetType(String)))
        dt.Columns.Add(New DataColumn("practice", GetType(String)))
        dt.Columns.Add(New DataColumn("voucherRedeem", GetType(Integer)))
        dt.Columns.Add(New DataColumn("totalAmount", GetType(Integer)))
        'dt.Columns.Add(New DataColumn("authorizedStatus", GetType(String)))
        'dt.Columns.Add(New DataColumn("transStatus", GetType(String)))
        dt.Columns.Add(New DataColumn("tsmp", GetType(Byte())))
        dt.Columns.Add(New DataColumn("scheme_code", GetType(String)))
        dt.Columns.Add(New DataColumn("display_code", GetType(String)))
        dt.Columns.Add(New DataColumn("Voucher_Acc_ID", GetType(String)))
        dt.Columns.Add(New DataColumn("Temp_Voucher_Acc_ID", GetType(String)))
        dt.Columns.Add(New DataColumn("Temp_Voucher_Acc_TSMP", GetType(Byte())))
        dt.Columns.Add(New DataColumn("Special_Acc_ID", GetType(String)))
        'dt.Columns.Add(New DataColumn("Special_Acc_TSMP", GetType(Byte())))
        dt.Columns.Add(New DataColumn("Invalid_Acc_ID", GetType(String)))
        dt.Columns.Add(New DataColumn("Invalidation", GetType(String)))
        'dt.Columns.Add(New DataColumn("Invalidation_TSMP", GetType(Byte())))
        dt.Columns.Add(New DataColumn("Create_By", GetType(String)))
        dt.Columns.Add(New DataColumn("Create_Dtm", GetType(Date)))
        dt.Columns.Add(New DataColumn("Service_Receive_Dtm", GetType(Date)))
        dt.Columns.Add(New DataColumn("Creation_Reason", GetType(String)))
        dt.Columns.Add(New DataColumn("Override_Reason", GetType(String)))

        'Dim strIdentityNo1Formatted As String = udcFormater.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, udtSearchCriteria.DocumentNo1)
        'Dim strIdentityNo2Formatted As String = String.Empty
        'If udtSearchCriteria.DocumentNo2 <> String.Empty Then strIdentityNo2Formatted = udcFormater.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, udtSearchCriteria.DocumentNo2)

        Dim dtAll As New DataTable

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
        ' -------------------------------------------------------------------------
        Dim udtBLLSearchResult As BaseBLL.BLLSearchResult
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

        Try
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
            ' -----------------------------------------------------------------------------------------
            Dim prams() As SqlParameter = {db.MakeInParam("@status", SqlDbType.Char, 1, EHSTransaction.EHSTransactionModel.TransRecordStatusClass.PendingApprovalForNonReimbursedClaim), _
                                            db.MakeInParam("@user_id", SqlDbType.VarChar, 20, strUserID)}

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
            ' -------------------------------------------------------------------------
            'db.RunProc("proc_VoucherTransaction_ManualReimbursedClaim_get_byStatus", prams, dtAll)

            udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_VoucherTransaction_ManualReimbursedClaim_get_byStatus", prams, blnOverrideResultLimit, db)

            If udtBLLSearchResult.SqlErrorMessage = BaseBLL.EnumSqlErrorMessage.Normal Then
                dtAll = CType(udtBLLSearchResult.Data, DataTable)
            Else
                udtBLLSearchResult.Data = Nothing
                Return udtBLLSearchResult
            End If
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

        Catch eSQL As SqlException
            db.RollBackTranscation()
            Throw eSQL

        Catch ex As Exception
            db.RollBackTranscation()
            Throw ex
        End Try

        For i As Integer = 0 To dtAll.Rows.Count - 1
            Dim drAll As DataRow = dtAll.Rows(i)

            Dim dr As DataRow = dt.NewRow()
            dr("lineNum") = i + 1
            dr("transNum") = Trim(drAll("tranNum"))
            dr("transDate") = drAll("tranDate")
            dr("serviceProvider") = drAll("SPName")
            If IsDBNull(drAll("SPChiName")) Then
                dr("spChiName") = ""
            Else
                dr("spChiName") = udcFormater.formatChineseName(drAll("SPChiName"))
            End If
            dr("spID") = drAll("SPID")

            'CRE12-015 - Add the respective practice number in ．Practice・ in the functions under ．Reimbursement・ in eHS [Start] [Tommy Tse]

            dr("practiceid") = drAll("practiceid")

            dr("spid_practiceid") = drAll("SPID").ToString + "(" + drAll("practiceid").ToString + ")"

            'CRE12-015 - Add the respective practice number in ．Practice・ in the functions under ．Reimbursement・ in eHS [End] [Tommy Tse]

            dr("bankAccount") = drAll("BankAccountNo")
            dr("practice") = drAll("PracticeName")
            dr("voucherRedeem") = drAll("voucherRedeem")
            dr("totalAmount") = drAll("totalAmount")

            'dr("transStatus") = drAll("status")

            'If IsDBNull(drAll("Authorised_status")) Then
            '    dr("authorizedStatus") = ""
            'Else
            '    dr("authorizedStatus") = drAll("Authorised_status")
            'End If

            dr("tsmp") = drAll("tsmp")
            dr("scheme_code") = drAll("scheme_code")
            dr("display_code") = drAll("Display_Code")

            dr("Voucher_Acc_ID") = CStr(IIf(IsDBNull(drAll("Voucher_Acc_ID")), String.Empty, drAll("Voucher_Acc_ID"))).Trim
            dr("Temp_Voucher_Acc_ID") = CStr(IIf(IsDBNull(drAll("Temp_Voucher_Acc_ID")), String.Empty, drAll("Temp_Voucher_Acc_ID"))).Trim
            'dr("Temp_Voucher_Acc_TSMP") = drAll("Temp_Voucher_Acc_TSMP")
            dr("Special_Acc_ID") = CStr(IIf(IsDBNull(drAll("Special_Acc_ID")), String.Empty, drAll("Special_Acc_ID"))).Trim
            'dr("Special_Acc_TSMP") = drAll("Special_Acc_TSMP")
            dr("Invalid_Acc_ID") = CStr(IIf(IsDBNull(drAll("Invalid_Acc_ID")), String.Empty, drAll("Invalid_Acc_ID"))).Trim
            dr("Invalidation") = drAll("Invalidation")
            'dr("Invalidation_TSMP") = drAll("Invalidation_TSMP")
            dr("Create_By") = drAll("Create_By")
            dr("Create_Dtm") = drAll("Create_Dtm")
            dr("Service_Receive_Dtm") = drAll("Service_Receive_Dtm")
            dr("Creation_Reason") = drAll("Creation_Reason")
            dr("Override_Reason") = drAll("Override_Reason")

            dt.Rows.Add(dr)

        Next

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
        ' -------------------------------------------------------------------------
        'Return dt
        udtBLLSearchResult.Data = dt
        Return udtBLLSearchResult
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

    End Function

    Public Function SearchVoucherTransactionByAny(ByVal strFunctionCode As String, ByVal udtSearchCriteria As Common.SearchCriteria.SearchCriteria, ByVal strUserID As String, ByVal blnOverrideResultLimit As Boolean, ByVal EnumSelectedStoredProc As Aspect) As BaseBLL.BLLSearchResult
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("lineNum", GetType(Integer)))
        dt.Columns.Add(New DataColumn("selected", GetType(CheckBox)))
        dt.Columns.Add(New DataColumn("transNum", GetType(String)))
        dt.Columns.Add(New DataColumn("transDate", GetType(Date)))
        dt.Columns.Add(New DataColumn("serviceProvider", GetType(String)))
        dt.Columns.Add(New DataColumn("spChiName", GetType(String)))
        dt.Columns.Add(New DataColumn("spID", GetType(String)))
        dt.Columns.Add(New DataColumn("practiceid", GetType(String)))
        dt.Columns.Add(New DataColumn("spid_practiceid", GetType(String)))
        dt.Columns.Add(New DataColumn("bankAccount", GetType(String)))
        dt.Columns.Add(New DataColumn("practice", GetType(String)))
        dt.Columns.Add(New DataColumn("voucherRedeem", GetType(Integer)))
        dt.Columns.Add(New DataColumn("totalAmount", GetType(Integer)))
        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        dt.Columns.Add(New DataColumn("totalAmountRMB", GetType(Decimal)))
        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]
        dt.Columns.Add(New DataColumn("authorizedStatus", GetType(String)))
        dt.Columns.Add(New DataColumn("transStatus", GetType(String)))
        dt.Columns.Add(New DataColumn("tsmp", GetType(Byte())))
        dt.Columns.Add(New DataColumn("scheme_code", GetType(String)))
        dt.Columns.Add(New DataColumn("display_code", GetType(String)))
        dt.Columns.Add(New DataColumn("Voucher_Acc_ID", GetType(String)))
        dt.Columns.Add(New DataColumn("Temp_Voucher_Acc_ID", GetType(String)))
        dt.Columns.Add(New DataColumn("Temp_Voucher_Acc_TSMP", GetType(Byte())))
        dt.Columns.Add(New DataColumn("Special_Acc_ID", GetType(String)))
        dt.Columns.Add(New DataColumn("Special_Acc_TSMP", GetType(Byte())))
        dt.Columns.Add(New DataColumn("Invalid_Acc_ID", GetType(String)))
        dt.Columns.Add(New DataColumn("Invalidation", GetType(String)))
        dt.Columns.Add(New DataColumn("Invalidation_TSMP", GetType(Byte())))
        dt.Columns.Add(New DataColumn("Manual_Reimburse", GetType(String)))
        dt.Columns.Add(New DataColumn("Means_Of_Input", GetType(String)))
        'CRE20-003 (add search criteria) [Start][Martin]
        dt.Columns.Add(New DataColumn("SchoolOrRCH_code", GetType(String)))
        dt.Columns.Add(New DataColumn("Subsidize_Item_Code", GetType(String)))
        dt.Columns.Add(New DataColumn("Dose_Code", GetType(String)))
        'CRE20-003 (add search criteria) [End][Martin]
        Dim objManualReimburse As Object = DBNull.Value

        If udtSearchCriteria.ReimbursementMethod.Trim.Equals(Common.Component.EHSTransaction.EHSTransactionModel.ReimbursementMethodStatusClass.InEHS) Then
            objManualReimburse = "N"
        ElseIf udtSearchCriteria.ReimbursementMethod.Trim.Equals(Common.Component.EHSTransaction.EHSTransactionModel.ReimbursementMethodStatusClass.OutsideEHS) Then
            objManualReimburse = "Y"
        End If

        Dim dtAll As New DataTable

        'CRE20-003 (add search criteria) [Start][Martin]
        '-----------------------------------------------------------------------------------------
        Dim udtBLLSearchResult As BaseBLL.BLLSearchResult = Nothing

        Try
            Select Case EnumSelectedStoredProc
                Case Aspect.Transaction
                    Dim prams() As SqlParameter = {db.MakeInParam("@transaction_id", SqlDbType.Char, 20, IIf(udcValidator.IsEmpty(udtSearchCriteria.TransNum), DBNull.Value, udtSearchCriteria.TransNum)), _
                                                    db.MakeInParam("@service_type", SqlDbType.Char, 5, IIf(udcValidator.IsEmpty(udtSearchCriteria.HealthProf), DBNull.Value, udtSearchCriteria.HealthProf)), _
                                                    db.MakeInParam("@from_date", SqlDbType.DateTime, 8, IIf(udcValidator.IsEmpty(udtSearchCriteria.FromDate), DBNull.Value, udtSearchCriteria.FromDate + " 00:00:00")), _
                                                    db.MakeInParam("@to_date", SqlDbType.DateTime, 8, IIf(udcValidator.IsEmpty(udtSearchCriteria.CutoffDate), DBNull.Value, udtSearchCriteria.CutoffDate + " 23:59:59")), _
                                                    db.MakeInParam("@Service_Receive_Dtm_From", SqlDbType.DateTime, 8, IIf(udcValidator.IsEmpty(udtSearchCriteria.ServiceDateFrom), DBNull.Value, udtSearchCriteria.ServiceDateFrom)), _
                                                    db.MakeInParam("@Service_Receive_Dtm_To", SqlDbType.DateTime, 8, IIf(udcValidator.IsEmpty(udtSearchCriteria.ServiceDateTo), DBNull.Value, udtSearchCriteria.ServiceDateTo)), _
                                                    db.MakeInParam("@scheme_code", SqlDbType.Char, 10, IIf(udcValidator.IsEmpty(udtSearchCriteria.SchemeCode), DBNull.Value, udtSearchCriteria.SchemeCode)), _
                                                    db.MakeInParam("@status", SqlDbType.Char, 1, IIf(udcValidator.IsEmpty(udtSearchCriteria.TransStatus), DBNull.Value, udtSearchCriteria.TransStatus)), _
                                                    db.MakeInParam("@authorised_status", SqlDbType.Char, 1, IIf(udcValidator.IsEmpty(udtSearchCriteria.AuthorizedStatus), DBNull.Value, udtSearchCriteria.AuthorizedStatus)), _
                                                    db.MakeInParam("@Invalidation", SqlDbType.Char, 1, IIf(udcValidator.IsEmpty(udtSearchCriteria.Invalidation), DBNull.Value, udtSearchCriteria.Invalidation)), _
                                                    db.MakeInParam("@reimbursement_method", SqlDbType.VarChar, 1, objManualReimburse), _
                                                    db.MakeInParam("@Means_Of_Input", SqlDbType.Char, 1, IIf(udtSearchCriteria.MeansOfInput = String.Empty, DBNull.Value, udtSearchCriteria.MeansOfInput)), _
                                                    db.MakeInParam("@SchoolOrRCH_code", SqlDbType.Char, 50, IIf(udcValidator.IsEmpty(udtSearchCriteria.SchoolOrRCHCode), DBNull.Value, udtSearchCriteria.SchoolOrRCHCode)), _
                                                    db.MakeInParam("@Subsidize_Item_Code", SqlDbType.Char, 10, IIf(udcValidator.IsEmpty(udtSearchCriteria.SubsidizeItemCode), DBNull.Value, udtSearchCriteria.SubsidizeItemCode)), _
                                                    db.MakeInParam("@Dose_Code", SqlDbType.Char, 20, IIf(udcValidator.IsEmpty(udtSearchCriteria.DoseCode), DBNull.Value, udtSearchCriteria.DoseCode)), _
                                                    db.MakeInParam("@user_id", SqlDbType.VarChar, 20, strUserID) _
                                                    }

                    udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_VoucherTransaction_get_byTranAspect", prams, blnOverrideResultLimit, db)
                Case Aspect.ServiceProvider
                    ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
                    Dim prams() As SqlParameter = {db.MakeInParam("@sp_id", SqlDbType.Char, 8, IIf(udcValidator.IsEmpty(udtSearchCriteria.ServiceProviderID), DBNull.Value, udtSearchCriteria.ServiceProviderID)), _
                                                    db.MakeInParam("@sp_name", SqlDbType.VarChar, 40, IIf(udcValidator.IsEmpty(udtSearchCriteria.ServiceProviderName), DBNull.Value, udtSearchCriteria.ServiceProviderName)), _
                                                    db.MakeInParam("@sp_chi_name", SqlDbType.NVarChar, 6, IIf(udcValidator.IsEmpty(udtSearchCriteria.ServiceProviderChiName), DBNull.Value, udtSearchCriteria.ServiceProviderChiName)), _
                                                    db.MakeInParam("@sp_hkid", SqlDbType.Char, 9, IIf(udcValidator.IsEmpty(udtSearchCriteria.ServiceProviderHKIC), DBNull.Value, udtSearchCriteria.ServiceProviderHKIC)), _
                                                    db.MakeInParam("@bank_acc", SqlDbType.VarChar, 30, IIf(udcValidator.IsEmpty(udtSearchCriteria.BankAcctNo), DBNull.Value, udtSearchCriteria.BankAcctNo)), _
                                                    db.MakeInParam("@from_date", SqlDbType.DateTime, 8, IIf(udcValidator.IsEmpty(udtSearchCriteria.FromDate), DBNull.Value, udtSearchCriteria.FromDate + " 00:00:00")), _
                                                    db.MakeInParam("@to_date", SqlDbType.DateTime, 8, IIf(udcValidator.IsEmpty(udtSearchCriteria.CutoffDate), DBNull.Value, udtSearchCriteria.CutoffDate + " 23:59:59")), _
                                                    db.MakeInParam("@Service_Receive_Dtm_From", SqlDbType.DateTime, 8, IIf(udcValidator.IsEmpty(udtSearchCriteria.ServiceDateFrom), DBNull.Value, udtSearchCriteria.ServiceDateFrom)), _
                                                    db.MakeInParam("@Service_Receive_Dtm_To", SqlDbType.DateTime, 8, IIf(udcValidator.IsEmpty(udtSearchCriteria.ServiceDateTo), DBNull.Value, udtSearchCriteria.ServiceDateTo)), _
                                                    db.MakeInParam("@scheme_code", SqlDbType.Char, 10, IIf(udcValidator.IsEmpty(udtSearchCriteria.SchemeCode), DBNull.Value, udtSearchCriteria.SchemeCode)), _
                                                    db.MakeInParam("@status", SqlDbType.Char, 1, IIf(udcValidator.IsEmpty(udtSearchCriteria.TransStatus), DBNull.Value, udtSearchCriteria.TransStatus)), _
                                                    db.MakeInParam("@authorised_status", SqlDbType.Char, 1, IIf(udcValidator.IsEmpty(udtSearchCriteria.AuthorizedStatus), DBNull.Value, udtSearchCriteria.AuthorizedStatus)), _
                                                    db.MakeInParam("@Invalidation", SqlDbType.Char, 1, IIf(udcValidator.IsEmpty(udtSearchCriteria.Invalidation), DBNull.Value, udtSearchCriteria.Invalidation)), _
                                                    db.MakeInParam("@reimbursement_method", SqlDbType.VarChar, 1, objManualReimburse), _
                                                    db.MakeInParam("@Means_Of_Input", SqlDbType.Char, 1, IIf(udtSearchCriteria.MeansOfInput = String.Empty, DBNull.Value, udtSearchCriteria.MeansOfInput)), _
                                                    db.MakeInParam("@SchoolOrRCH_code", SqlDbType.Char, 50, IIf(udcValidator.IsEmpty(udtSearchCriteria.SchoolOrRCHCode), DBNull.Value, udtSearchCriteria.SchoolOrRCHCode)), _
                                                    db.MakeInParam("@user_id", SqlDbType.VarChar, 20, strUserID) _
                                                    }

                    udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_VoucherTransaction_get_bySPAspect", prams, blnOverrideResultLimit, db)

                Case Aspect.eHSAccount
                    Dim prams() As SqlParameter = {db.MakeInParam("@doc_code", SqlDbType.Char, 20, IIf(udcValidator.IsEmpty(udtSearchCriteria.DocumentType), DBNull.Value, udtSearchCriteria.DocumentType)), _
                                                    db.MakeInParam("@identity_no1", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, IIf(udcValidator.IsEmpty(udtSearchCriteria.DocumentNo1), DBNull.Value, udtSearchCriteria.DocumentNo1)), _
                                                    db.MakeInParam("@Adoption_Prefix_Num", SqlDbType.Char, 7, IIf(udcValidator.IsEmpty(udtSearchCriteria.DocumentNo2), DBNull.Value, udtSearchCriteria.DocumentNo2)), _
                                                    db.MakeInParam("@voucher_acc_id", SqlDbType.VarChar, 15, IIf(udcValidator.IsEmpty(udtSearchCriteria.VoucherAccID), DBNull.Value, udtSearchCriteria.VoucherAccID)), _
                                                    db.MakeInParam("@from_date", SqlDbType.DateTime, 8, IIf(udcValidator.IsEmpty(udtSearchCriteria.FromDate), DBNull.Value, udtSearchCriteria.FromDate + " 00:00:00")), _
                                                    db.MakeInParam("@to_date", SqlDbType.DateTime, 8, IIf(udcValidator.IsEmpty(udtSearchCriteria.CutoffDate), DBNull.Value, udtSearchCriteria.CutoffDate + " 23:59:59")), _
                                                    db.MakeInParam("@Service_Receive_Dtm_From", SqlDbType.DateTime, 8, IIf(udcValidator.IsEmpty(udtSearchCriteria.ServiceDateFrom), DBNull.Value, udtSearchCriteria.ServiceDateFrom)), _
                                                    db.MakeInParam("@Service_Receive_Dtm_To", SqlDbType.DateTime, 8, IIf(udcValidator.IsEmpty(udtSearchCriteria.ServiceDateTo), DBNull.Value, udtSearchCriteria.ServiceDateTo)), _
                                                    db.MakeInParam("@scheme_code", SqlDbType.Char, 10, IIf(udcValidator.IsEmpty(udtSearchCriteria.SchemeCode), DBNull.Value, udtSearchCriteria.SchemeCode)), _
                                                    db.MakeInParam("@status", SqlDbType.Char, 1, IIf(udcValidator.IsEmpty(udtSearchCriteria.TransStatus), DBNull.Value, udtSearchCriteria.TransStatus)), _
                                                    db.MakeInParam("@authorised_status", SqlDbType.Char, 1, IIf(udcValidator.IsEmpty(udtSearchCriteria.AuthorizedStatus), DBNull.Value, udtSearchCriteria.AuthorizedStatus)), _
                                                    db.MakeInParam("@Invalidation", SqlDbType.Char, 1, IIf(udcValidator.IsEmpty(udtSearchCriteria.Invalidation), DBNull.Value, udtSearchCriteria.Invalidation)), _
                                                    db.MakeInParam("@reimbursement_method", SqlDbType.VarChar, 1, objManualReimburse), _
                                                    db.MakeInParam("@Means_Of_Input", SqlDbType.Char, 1, IIf(udtSearchCriteria.MeansOfInput = String.Empty, DBNull.Value, udtSearchCriteria.MeansOfInput)), _
                                                    db.MakeInParam("@SchoolOrRCH_code", SqlDbType.Char, 50, IIf(udcValidator.IsEmpty(udtSearchCriteria.SchoolOrRCHCode), DBNull.Value, udtSearchCriteria.SchoolOrRCHCode)), _
                                                    db.MakeInParam("@user_id", SqlDbType.VarChar, 20, strUserID), _
                                                    db.MakeInParam("@eHA_name", SqlDbType.VarChar, SProcParameter.EngNameDataSize, IIf(udcValidator.IsEmpty(udtSearchCriteria.VoucherRecipientName), DBNull.Value, udtSearchCriteria.VoucherRecipientName)), _
                                                    db.MakeInParam("@eHA_chi_name", SqlDbType.NVarChar, 30, IIf(udcValidator.IsEmpty(udtSearchCriteria.VoucherRecipientChiName), DBNull.Value, udtSearchCriteria.VoucherRecipientChiName)), _
                                                    db.MakeInParam("@RawIdentityNum", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, IIf(udcValidator.IsEmpty(udtSearchCriteria.RawIdentityNum), DBNull.Value, udtSearchCriteria.RawIdentityNum)) _
                                                    }

                    udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_VoucherTransaction_get_byEHAAspect", prams, blnOverrideResultLimit, db)

                Case Else
                    Dim prams() As SqlParameter = {db.MakeInParam("@transaction_id", SqlDbType.Char, 20, IIf(udcValidator.IsEmpty(udtSearchCriteria.TransNum), DBNull.Value, udtSearchCriteria.TransNum)), _
                                                  db.MakeInParam("@status", SqlDbType.Char, 1, IIf(udcValidator.IsEmpty(udtSearchCriteria.TransStatus), DBNull.Value, udtSearchCriteria.TransStatus)), _
                                                  db.MakeInParam("@authorised_status", SqlDbType.Char, 1, IIf(udcValidator.IsEmpty(udtSearchCriteria.AuthorizedStatus), DBNull.Value, udtSearchCriteria.AuthorizedStatus)), _
                                                  db.MakeInParam("@sp_id", SqlDbType.Char, 8, IIf(udcValidator.IsEmpty(udtSearchCriteria.ServiceProviderID), DBNull.Value, udtSearchCriteria.ServiceProviderID)), _
                                                  db.MakeInParam("@sp_name", SqlDbType.VarChar, SProcParameter.EngNameDataSize, IIf(udcValidator.IsEmpty(udtSearchCriteria.ServiceProviderName), DBNull.Value, udtSearchCriteria.ServiceProviderName)), _
                                                  db.MakeInParam("@sp_hkid", SqlDbType.Char, 9, IIf(udcValidator.IsEmpty(udtSearchCriteria.ServiceProviderHKIC), DBNull.Value, udtSearchCriteria.ServiceProviderHKIC)), _
                                                  db.MakeInParam("@bank_acc", SqlDbType.VarChar, 30, IIf(udcValidator.IsEmpty(udtSearchCriteria.BankAcctNo), DBNull.Value, udtSearchCriteria.BankAcctNo)), _
                                                  db.MakeInParam("@service_type", SqlDbType.Char, 5, IIf(udcValidator.IsEmpty(udtSearchCriteria.HealthProf), DBNull.Value, udtSearchCriteria.HealthProf)), _
                                                  db.MakeInParam("@from_date", SqlDbType.DateTime, 8, IIf(udcValidator.IsEmpty(udtSearchCriteria.FromDate), DBNull.Value, udtSearchCriteria.FromDate + " 00:00:00")), _
                                                  db.MakeInParam("@to_date", SqlDbType.DateTime, 8, IIf(udcValidator.IsEmpty(udtSearchCriteria.CutoffDate), DBNull.Value, udtSearchCriteria.CutoffDate + " 23:59:59")), _
                                                  db.MakeInParam("@scheme_code", SqlDbType.Char, 10, IIf(udcValidator.IsEmpty(udtSearchCriteria.SchemeCode), DBNull.Value, udtSearchCriteria.SchemeCode)), _
                                                  db.MakeInParam("@user_id", SqlDbType.VarChar, 20, strUserID), _
                                                  db.MakeInParam("@doc_code", SqlDbType.Char, 20, IIf(udcValidator.IsEmpty(udtSearchCriteria.DocumentType), DBNull.Value, udtSearchCriteria.DocumentType)), _
                                                  db.MakeInParam("@identity_no1", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, IIf(udcValidator.IsEmpty(udtSearchCriteria.DocumentNo1), DBNull.Value, udtSearchCriteria.DocumentNo1)), _
                                                  db.MakeInParam("@Adoption_Prefix_Num", SqlDbType.Char, 7, IIf(udcValidator.IsEmpty(udtSearchCriteria.DocumentNo2), DBNull.Value, udtSearchCriteria.DocumentNo2)), _
                                                  db.MakeInParam("@Invalidation", SqlDbType.Char, 1, IIf(udcValidator.IsEmpty(udtSearchCriteria.Invalidation), DBNull.Value, udtSearchCriteria.Invalidation)), _
                                                  db.MakeInParam("@voucher_acc_id", SqlDbType.VarChar, 15, IIf(udcValidator.IsEmpty(udtSearchCriteria.VoucherAccID), DBNull.Value, udtSearchCriteria.VoucherAccID)), _
                                                  db.MakeInParam("@reimbursement_method", SqlDbType.VarChar, 1, objManualReimburse), _
                                                  db.MakeInParam("@Means_Of_Input", SqlDbType.Char, 1, IIf(udtSearchCriteria.MeansOfInput = String.Empty, DBNull.Value, udtSearchCriteria.MeansOfInput)), _
                                                  db.MakeInParam("@Service_Receive_Dtm_From", SqlDbType.DateTime, 8, IIf(udcValidator.IsEmpty(udtSearchCriteria.ServiceDateFrom), DBNull.Value, udtSearchCriteria.ServiceDateFrom)), _
                                                  db.MakeInParam("@Service_Receive_Dtm_To", SqlDbType.DateTime, 8, IIf(udcValidator.IsEmpty(udtSearchCriteria.ServiceDateTo), DBNull.Value, udtSearchCriteria.ServiceDateTo)), _
                                                  db.MakeInParam("@SchoolOrRCH_code", SqlDbType.Char, 50, IIf(udcValidator.IsEmpty(udtSearchCriteria.SchoolOrRCHCode), DBNull.Value, udtSearchCriteria.SchoolOrRCHCode)), _
                                                  db.MakeInParam("@RawIdentityNum", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, IIf(udcValidator.IsEmpty(udtSearchCriteria.RawIdentityNum), DBNull.Value, udtSearchCriteria.RawIdentityNum)) _
                                              }

                    udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_VoucherTransaction_get_byAny", prams, blnOverrideResultLimit, db)

            End Select

            If Not udtBLLSearchResult Is Nothing Then
                If udtBLLSearchResult.SqlErrorMessage = BaseBLL.EnumSqlErrorMessage.Normal Then
                    dtAll = CType(udtBLLSearchResult.Data, DataTable)
                Else
                    udtBLLSearchResult.Data = Nothing
                    Return udtBLLSearchResult
                End If
            Else
                Throw New Exception("Error: SearchEngineBLL - SearchVoucherTransactionByAny - StoredProc is not executed.")
            End If

        Catch eSQL As SqlException
            db.RollBackTranscation()
            Throw eSQL

        Catch ex As Exception
            db.RollBackTranscation()
            Throw ex
        End Try

        For i As Integer = 0 To dtAll.Rows.Count - 1
            Dim drAll As DataRow = dtAll.Rows(i)

            Dim dr As DataRow = dt.NewRow()
            dr("lineNum") = i + 1
            dr("transNum") = Trim(drAll("tranNum"))
            dr("transDate") = drAll("tranDate")
            dr("serviceProvider") = drAll("SPName")
            If IsDBNull(drAll("SPChiName")) Then
                dr("spChiName") = ""
            Else
                dr("spChiName") = udcFormater.formatChineseName(drAll("SPChiName"))
            End If
            dr("spID") = drAll("SPID")
            dr("practiceid") = drAll("practiceid")
            dr("spid_practiceid") = drAll("SPID").ToString + "(" + drAll("practiceid").ToString + ")"
            dr("bankAccount") = drAll("BankAccountNo")
            dr("practice") = drAll("PracticeName")
            dr("voucherRedeem") = drAll("voucherRedeem")
            dr("totalAmount") = drAll("totalAmount")
            ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            dr("totalAmountRMB") = drAll("totalAmountRMB")
            ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

            dr("transStatus") = drAll("status")

            If IsDBNull(drAll("Authorised_status")) Then
                dr("authorizedStatus") = ""
            Else
                dr("authorizedStatus") = drAll("Authorised_status")
            End If

            dr("tsmp") = drAll("tsmp")
            dr("scheme_code") = drAll("scheme_code")
            dr("display_code") = drAll("Display_Code")

            dr("Voucher_Acc_ID") = CStr(IIf(IsDBNull(drAll("Voucher_Acc_ID")), String.Empty, drAll("Voucher_Acc_ID"))).Trim
            dr("Temp_Voucher_Acc_ID") = CStr(IIf(IsDBNull(drAll("Temp_Voucher_Acc_ID")), String.Empty, drAll("Temp_Voucher_Acc_ID"))).Trim
            dr("Temp_Voucher_Acc_TSMP") = drAll("Temp_Voucher_Acc_TSMP")
            dr("Special_Acc_ID") = CStr(IIf(IsDBNull(drAll("Special_Acc_ID")), String.Empty, drAll("Special_Acc_ID"))).Trim
            dr("Special_Acc_TSMP") = drAll("Special_Acc_TSMP")
            dr("Invalid_Acc_ID") = CStr(IIf(IsDBNull(drAll("Invalid_Acc_ID")), String.Empty, drAll("Invalid_Acc_ID"))).Trim
            dr("Invalidation") = drAll("Invalidation")
            dr("Invalidation_TSMP") = drAll("Invalidation_TSMP")
            dr("Manual_Reimburse") = drAll("Manual_Reimburse")
            dr("Means_Of_Input") = drAll("Means_Of_Input")
            dr("SchoolOrRCH_code") = CStr(IIf(IsDBNull(drAll("SchoolOrRCH_code")), String.Empty, drAll("SchoolOrRCH_code"))).Trim


            dt.Rows.Add(dr)

        Next
        'CRE20-003 (add search criteria) [End][Martin]

        udtBLLSearchResult.Data = dt

        Return udtBLLSearchResult

    End Function

    Public Function SearchVoucherTransactionByAnyReadOnly(ByVal udtSearchCriteria As Common.SearchCriteria.SearchCriteria, ByVal strUserID As String) As DataTable
        Dim dt, dtall As New DataTable
        Dim dr, drall As DataRow
        Dim i As Integer

        dt = New DataTable()
        dt.Columns.Add(New DataColumn("lineNum", GetType(Integer)))
        dt.Columns.Add(New DataColumn("selected", GetType(CheckBox)))
        dt.Columns.Add(New DataColumn("transNum", GetType(String)))
        dt.Columns.Add(New DataColumn("transDate", GetType(String)))
        dt.Columns.Add(New DataColumn("serviceProvider", GetType(String)))
        dt.Columns.Add(New DataColumn("spChiName", GetType(String)))
        dt.Columns.Add(New DataColumn("spID", GetType(String)))
        dt.Columns.Add(New DataColumn("bankAccount", GetType(String)))
        dt.Columns.Add(New DataColumn("practice", GetType(String)))
        dt.Columns.Add(New DataColumn("voucherRedeem", GetType(Integer)))
        dt.Columns.Add(New DataColumn("voucherValue", GetType(Integer)))
        dt.Columns.Add(New DataColumn("totalAmount", GetType(Integer)))
        dt.Columns.Add(New DataColumn("authorizedStatus", GetType(String)))
        dt.Columns.Add(New DataColumn("transStatus", GetType(String)))
        dt.Columns.Add(New DataColumn("tsmp", GetType(Byte())))
        dt.Columns.Add(New DataColumn("Scheme_Code", GetType(String)))
        dt.Columns.Add(New DataColumn("Display_Code", GetType(String)))

        Try
            ' create data object and params
            Dim prams() As SqlParameter = {db.MakeInParam("@transaction_id", SqlDbType.Char, 20, IIf(udcValidator.IsEmpty(udtSearchCriteria.TransNum), DBNull.Value, udtSearchCriteria.TransNum)), _
                                            db.MakeInParam("@status", SqlDbType.Char, 1, IIf(udcValidator.IsEmpty(udtSearchCriteria.TransStatus), DBNull.Value, udtSearchCriteria.TransStatus)), _
                                            db.MakeInParam("@authorised_status", SqlDbType.Char, 1, IIf(udcValidator.IsEmpty(udtSearchCriteria.AuthorizedStatus), DBNull.Value, udtSearchCriteria.AuthorizedStatus)), _
                                            db.MakeInParam("@sp_id", SqlDbType.Char, 8, IIf(udcValidator.IsEmpty(udtSearchCriteria.ServiceProviderID), DBNull.Value, udtSearchCriteria.ServiceProviderID)), _
                                            db.MakeInParam("@sp_name", SqlDbType.VarChar, SProcParameter.EngNameDataSize, IIf(udcValidator.IsEmpty(udtSearchCriteria.ServiceProviderName), DBNull.Value, udtSearchCriteria.ServiceProviderName)), _
                                            db.MakeInParam("@sp_hkid", SqlDbType.Char, 9, IIf(udcValidator.IsEmpty(udtSearchCriteria.ServiceProviderHKIC), DBNull.Value, udtSearchCriteria.ServiceProviderHKIC)), _
                                            db.MakeInParam("@bank_acc", SqlDbType.VarChar, 30, IIf(udcValidator.IsEmpty(udtSearchCriteria.BankAcctNo), DBNull.Value, udtSearchCriteria.BankAcctNo)), _
                                            db.MakeInParam("@service_type", SqlDbType.Char, 5, IIf(udcValidator.IsEmpty(udtSearchCriteria.HealthProf), DBNull.Value, udtSearchCriteria.HealthProf)), _
                                            db.MakeInParam("@vr_hkid", SqlDbType.Char, 9, IIf(udcValidator.IsEmpty(udtSearchCriteria.VoucherRecipientHKIC), DBNull.Value, udtSearchCriteria.VoucherRecipientHKIC)), _
                                            db.MakeInParam("@from_date", SqlDbType.DateTime, 8, IIf(udcValidator.IsEmpty(udtSearchCriteria.FromDate), DBNull.Value, udtSearchCriteria.FromDate & " 00:00:00")), _
                                            db.MakeInParam("@to_date", SqlDbType.DateTime, 8, IIf(udcValidator.IsEmpty(udtSearchCriteria.CutoffDate), DBNull.Value, udtSearchCriteria.CutoffDate & " 23:59:59")), _
                                            db.MakeInParam("@scheme_code", SqlDbType.Char, 10, IIf(udcValidator.IsEmpty(udtSearchCriteria.SchemeCode), DBNull.Value, udtSearchCriteria.SchemeCode)), _
                                            db.MakeInParam("@user_id", SqlDbType.VarChar, 20, strUserID)}

            ' run the stored procedure
            db.RunProc("proc_VoucherTransaction_get_byAnyReadOnly", prams, dtall)
        Catch eSQL As SqlException
            db.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            db.RollBackTranscation()
            Throw ex
        End Try

        Dim udcCTS As New Common.Component.ClaimTransStatus
        Dim strEng, strChi As String
        strEng = ""
        strChi = ""

        For i = 0 To dtall.Rows.Count - 1
            drall = CType(dtall.Rows(i), DataRow)
            dr = dt.NewRow()
            dr("lineNum") = i + 1
            dr("transNum") = Trim(drall("tranNum"))
            dr("transDate") = udcFormater.formatDateTime(drall("tranDate"), "EN")
            dr("serviceProvider") = drall("SPName")
            If IsDBNull(drall("SPChiName")) Then
                dr("spChiName") = ""
            Else
                dr("spChiName") = udcFormater.formatChineseName(drall("SPChiName"))
            End If
            dr("spID") = drall("SPID")
            dr("bankAccount") = drall("BankAccountNo")
            dr("practice") = drall("PracticeName")
            dr("voucherRedeem") = drall("voucherRedeem")
            dr("voucherValue") = drall("voucherAmount")
            dr("totalAmount") = drall("totalAmount")

            ClaimTransStatus.GetDescriptionFromDBCode(ClaimTransStatus.ClassCode, drall("status"), strEng, strChi)
            dr("transStatus") = strEng
            strEng = ""

            If IsDBNull(drall("Authorised_status")) Then
                dr("authorizedStatus") = ""
            Else
                If udcValidator.IsEmpty(drall("Authorised_status")) Then
                    dr("authorizedStatus") = ""
                Else
                    ReimbursementStatus.GetDescriptionFromDBCode(ReimbursementStatus.ClassCode, drall("Authorised_status"), strEng, strChi)
                    dr("authorizedStatus") = strEng
                End If
            End If

            dr("tsmp") = drall("tsmp")
            dr("scheme_code") = drall("scheme_code")
            dr("Display_Code") = drall("Display_Code")

            dt.Rows.Add(dr)
        Next

        Return dt
    End Function

    ''' <summary>
    ''' Get The suspend history of a transaction given the transaction number
    ''' </summary>
    ''' <param name="criteria">criteria object</param>
    ''' <returns>Data table contains the history</returns>
    ''' <remarks></remarks>
    Public Function SearchSuspendHistory(ByVal criteria As Common.SearchCriteria.SearchCriteria) As DataTable
        Dim dt As New DataTable

        Try
            Dim prams() As SqlParameter = {db.MakeInParam("@tran_id", SqlDbType.Char, 20, criteria.TransNum)}
            db.RunProc("proc_VoucherTranSuspendLog_get_byTranID", prams, dt)

        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
        End Try

        Return dt

    End Function


    Public Function GetFilteredTxn(ByVal dtSrc As DataTable, ByVal fieldname As ArrayList, ByVal fieldType As ArrayList, ByVal fieldValue As ArrayList, Optional ByVal distinctField As String = "") As DataTable
        Dim dtnew As New DataTable
        Dim i, j As Integer
        Dim drnew As DataRow
        Dim removeindex As New ArrayList
        Dim distinctGroupBy As New ArrayList
        Dim passed As Boolean = True

        dtnew = dtSrc.Copy()

        If Not IsNothing(fieldname) Then
            For j = 0 To dtSrc.Rows.Count - 1
                drnew = CType(dtnew.Rows(j), DataRow)

                passed = True

                For i = 0 To fieldname.Count - 1
                    If fieldType(i).ToString.ToUpper.Equals("S") Then
                        If Not drnew(fieldname(i)).Equals(fieldValue(i)) Then
                            If Not removeindex.Contains(j) Then removeindex.Add(j)
                            passed = False
                        End If
                    Else    'Date field
                        If Not DateTime.ParseExact(drnew(fieldname(i)), "M/dd/yyyy HH:mm", Nothing).ToString("dd MMM yyyy HH:mm").Equals(fieldValue(i)) Then
                            If Not removeindex.Contains(j) Then removeindex.Add(j)
                            passed = False
                        End If

                    End If
                Next i
            Next j

            For i = removeindex.Count - 1 To 0 Step -1
                dtnew.Rows(removeindex(i)).Delete()
            Next

            dtnew.AcceptChanges()
        End If

        'Session("v2_Level" & level & "_data") = dtnew
        Return dtnew
    End Function

    Private Function IsMatchSearchCriteria(ByVal criteria As Common.SearchCriteria.SearchCriteria, ByVal drxml As DataRow) As Boolean
        Dim IsStillProceed As Boolean = False

        'To be removed
        If Not IsNothing(criteria.TransNum) Then
            If drxml("tranNum").ToString.ToUpper.Equals(criteria.TransNum.ToUpper) Then
                IsStillProceed = True
            Else
                Return False
            End If
        End If

        If Not IsNothing(criteria.TransStatus) Then
            If drxml("status").ToString.ToUpper.Equals(criteria.TransStatus.ToUpper) Then
                IsStillProceed = True
            Else
                Return False
            End If
        End If

        If Not IsNothing(criteria.FirstAuthorizedDate) Then
            If DateTime.ParseExact(drxml("firstAuthorizedDate").ToString, "M/dd/yyyy HH:mm", Nothing).ToString("dd MMM yyyy HH:mm").Equals(criteria.FirstAuthorizedDate) Then
                IsStillProceed = True
            Else
                Return False
            End If
        End If

        If Not IsNothing(criteria.ServiceProviderID) Then
            If drxml("SPID").ToString.Equals(criteria.ServiceProviderID) Then
                IsStillProceed = True
            Else
                Return False
            End If
        End If

        If Not IsNothing(criteria.BankAcctNo) Then
            If drxml("bankAccountID").ToString.Equals(criteria.BankAcctNo) Then
                IsStillProceed = True
            Else
                Return False
            End If
        End If

        If Not IsNothing(criteria.Practice) Then
            If drxml("practiceID").ToString.Equals(criteria.Practice) Then
                IsStillProceed = True
            Else
                Return False
            End If
        End If

        Return IsStillProceed

    End Function

    'Private Function FormatDate2(ByVal tm As Date) As String
    '    Return tm.ToString("dd MMM yyyy HH:mm")
    'End Function

    Private Function CallDBProc(ByVal spid As String, ByVal sphkic As String, ByVal spname As String, ByVal spRegNo As String, ByVal healthprof As String, ByVal enrolmentNo As String, _
    ByVal bankAcctNo As String, ByVal practice As String, ByVal bankAcctOwner As String, ByVal bankName As String, ByVal branchName As String, ByVal bankSubmissionDate As String, _
    ByVal bankStatus As String, ByVal transFromDate As String, ByVal transCutoffDate As String, ByVal transStatus As String, ByVal transNum As String, ByVal voucherRecipientHKIC As String, ByVal voucherRecipientName As String) As String

        Return ""

    End Function

    Private Sub UpdateMultipleTranStatusToDB_delete(ByVal newStatus As String, ByVal dtSrc As DataTable)
        'Dim claimtrans As EVoucher_Common.claimTrans = New EVoucher_Common.claimTrans
        'Dim db As New Common.DataAccess.Database
        'Dim activedt As New DataTable
        'Dim i As Integer
        'Dim updatetime As String

        'activedt = dtSrc
        'updatetime = Now().ToString("M/dd/yyyy HH:mm")

        'For i = 0 To activedt.Rows.Count - 1
        '    claimdr = CType(activedt.Rows(i), DataRow)

        '    claimtrans.TranNum = claimdr.Item("tranNum")
        '    claimtrans.TranDate = claimdr.Item("tranDate")
        '    claimtrans.AcctHKID = claimdr.Item("acctHKID")
        '    claimtrans.AcctDOB = claimdr.Item("acctDOB")
        '    claimtrans.ExactDOB = claimdr.Item("exactDOB")
        '    claimtrans.VoucherAmount = claimdr.Item("voucherAmount")
        '    claimtrans.VoucherRedeem = claimdr.Item("voucherRedeem")
        '    claimtrans.ServiceDate = claimdr.Item("serviceDate")
        '    claimtrans.ServiceProviderID = claimdr.Item("SPID")
        '    claimtrans.ServiceProviderName = claimdr.Item("SPName")
        '    claimtrans.PracticeID = claimdr.Item("practiceID")
        '    claimtrans.BankAccountID = claimdr.Item("bankAccountID")
        '    claimtrans.VisitReason_L1 = claimdr.Item("visitReason_L1")
        '    claimtrans.VisitReason_L2 = claimdr.Item("visitReason_L2")
        '    claimtrans.DataEntryAcct = claimdr.Item("dataEntryAcct")

        '    If newStatus.ToUpper.Equals("1ST AUTHORIZED") Then
        '        claimtrans.FirstAuthorizedBy = "HCVU Reimbursement 1st Authorizer"  'claimdr.Item("firstAuthorizedBy")
        '        claimtrans.FirstAuthorizedDate = updatetime 'claimdr.Item("firstAuthorizedDate")    'Now.ToString("dd MMM yyyy")
        '    Else
        '        claimtrans.FirstAuthorizedBy = claimdr.Item("firstAuthorizedBy")
        '        claimtrans.FirstAuthorizedDate = claimdr.Item("firstAuthorizedDate")    'Now.ToString("dd MMM yyyy")
        '    End If

        '    If newStatus.ToUpper.Equals("2ND AUTHORIZED") Then
        '        claimtrans.SecondAuthorizedBy = "HCVU Reimbursement 2nd Authorizer" 'claimdr.Item("secondAuthorizedBy")
        '        claimtrans.SecondAuthorizedDate = updatetime    'claimdr.Item("secondAuthorizedDate")
        '    Else
        '        claimtrans.SecondAuthorizedBy = claimdr.Item("secondAuthorizedBy")
        '        claimtrans.SecondAuthorizedDate = claimdr.Item("secondAuthorizedDate")
        '    End If

        '    If newStatus.ToUpper.Equals("REIMBURSED") Then
        '        claimtrans.ReimbursedBy = "HCVU Reimbursement Payment File User"
        '        claimtrans.ReimbursedDate = updatetime
        '    Else
        '        claimtrans.ReimbursedBy = ""
        '        claimtrans.ReimbursedDate = ""
        '    End If

        '    claimtrans.VoucherAcctID = claimdr.Item("vaid")
        '    claimtrans.ServiceType = claimdr.Item("serviceType")
        '    claimtrans.Status = newStatus
        '    claimtrans.ConfirmDate = updatetime
        '    claimtrans.ConfirmSP = claimdr.Item("SPID") 'userAcct.Rep_serviceProviderID

        '    'dm.saveClaimTrans(claimtrans)
        '    db.RunProc("UpdateMultipleTranStatusToDB")
        'Next
    End Sub

    ''' <summary>
    ''' Check if that payment file status (ready for download or not...)
    ''' </summary>
    ''' <param name="createDate"></param>
    ''' <returns>True = ready for download; False = not ready</returns>
    ''' <remarks></remarks>
    Private Function IsReadyForDownload(ByVal createDate As DateTime) As Boolean
        If (Now - createDate).Days > 1 Then
            IsReadyForDownload = True
        Else
            IsReadyForDownload = False
        End If
    End Function
End Class
