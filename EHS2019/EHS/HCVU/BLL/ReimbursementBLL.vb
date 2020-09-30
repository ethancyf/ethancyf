Imports Common.ComFunction
Imports Common.ComFunction.ParameterFunction
Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.EHSTransaction
Imports Common.Component.FileGeneration
Imports Common.Component.HCVUUser
Imports Common.Component.Scheme
Imports Common.Component.Scheme.SchemeClaimModel
Imports Common.DataAccess
Imports Common.Format
Imports Common.SearchCriteria
Imports System.Data
Imports System.Data.SqlClient

Public Class ReimbursementBLL

#Region "Fields"

    Private AuthorizeLevel As String
    Private dtTxn As New DataTable
    Dim claimdr As DataRow
    Private udtEHSAccountBLL As New EHSAccountBLL
    Private udtFormatter As New Formatter
    Private udtGeneralFunction As New GeneralFunction
    Private udtHCVUUserBLL As New HCVUUserBLL
    Private udtSearchEngineBLL As New SearchEngineBLL

#End Region

#Region "Classes"

    ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
    <Serializable> _
    Public Class ReimbursementDataTable
        Inherits DataTable

        Public Sub New()
            Me.Columns.Add("Display_Seq")
            Me.Columns.Add("Scheme_Code")
            Me.Columns.Add("Display_Code")
            Me.Columns.Add("Reimbursement_Mode")
            Me.Columns.Add("Hold_Dtm")
            Me.Columns.Add("Hold_By")
            Me.Columns.Add("First_Authorised_Dtm")
            Me.Columns.Add("First_Authorised_By")
            Me.Columns.Add("Second_Authorised_Dtm")
            Me.Columns.Add("Second_Authorised_By")
            Me.Columns.Add("Reimbursed")
        End Sub

        Public Sub New(ByVal dtReimAuth As DataTable)
            Me.New()

            ' Get the Scheme Claim list
            For Each udtSchemeC As SchemeClaimModel In (New SchemeClaimBLL).getAllDistinctSchemeClaim()
                If udtSchemeC.ReimbursementMode = EnumReimbursementMode.NoReimbursement Then Continue For

                Dim dr As DataRow = Me.NewRow
                dr("Display_Seq") = udtSchemeC.DisplaySeq
                dr("Scheme_Code") = udtSchemeC.SchemeCode
                dr("Display_Code") = udtSchemeC.DisplayCode
                dr("Reimbursement_Mode") = CInt(udtSchemeC.ReimbursementMode)
                Me.Rows.Add(dr)
            Next

            For Each drReimAuth As DataRow In dtReimAuth.Rows
                Dim strScheme As String = CStr(drReimAuth("Scheme_Code")).Trim

                Dim dr As DataRow = Me.FindByScheme(strScheme)

                Select Case drReimAuth("Authorised_Status")
                    Case ReimbursementStatus.HoldForFirstAuthorisation
                        dr("Hold_Dtm") = drReimAuth("Authorised_Dtm")
                        dr("Hold_By") = drReimAuth("Authorised_By")

                    Case ReimbursementStatus.FirstAuthorised
                        dr("First_Authorised_Dtm") = drReimAuth("Authorised_Dtm")
                        dr("First_Authorised_By") = drReimAuth("Authorised_By")

                    Case ReimbursementStatus.SecondAuthorised
                        dr("Second_Authorised_Dtm") = drReimAuth("Authorised_Dtm")
                        dr("Second_Authorised_By") = drReimAuth("Authorised_By")

                    Case ReimbursementStatus.Reimbursed
                        dr("Reimbursed") = "Y"

                End Select
            Next
        End Sub

        Protected Sub New(ByVal info As Runtime.Serialization.SerializationInfo, ByVal context As Runtime.Serialization.StreamingContext)
            MyBase.New(info, context)
        End Sub

        '

        Public Function FindByScheme(ByVal strScheme As String) As DataRow
            For Each dr As DataRow In Me.Rows
                If CStr(dr("Scheme_Code")).Trim = strScheme Then Return dr
            Next

            Return Nothing
        End Function

        Public Function FilterByScheme(ByVal strScheme As String) As DataTable
            Dim dt As ReimbursementDataTable = Me.Clone

            For Each dr As DataRow In Me.Rows
                If CStr(dr("Scheme_Code")).Trim = strScheme Then
                    dt.ImportRow(dr)
                    Return dt
                End If
            Next

            Return Nothing
        End Function

        Public Function FilterByReimbursementMode(ByVal eReimbursementMode As EnumReimbursementMode) As DataTable
            Dim dt As DataTable = Me.Clone

            For Each dr As DataRow In Me.Rows
                If dr("Reimbursement_Mode") = eReimbursementMode Then
                    dt.ImportRow(dr)
                End If
            Next

            Return dt

        End Function

        '

        Public ReadOnly Property AtLeastOneSchemeHold() As Boolean
            Get
                For Each dr As DataRow In Me.Rows
                    If Not IsDBNull(dr("Hold_By")) Then
                        Return True
                    End If
                Next

                Return False
            End Get
        End Property

        Public ReadOnly Property AllSchemeIsReimbursed() As Boolean
            Get
                If Me.AtLeastOneSchemeHold = False Then Return False

                For Each dr As DataRow In Me.Rows
                    If Not IsDBNull(dr("Hold_By")) AndAlso IsDBNull(dr("Reimbursed")) Then
                        Return False
                    End If
                Next

                Return True
            End Get
        End Property

        Public ReadOnly Property AbleToGenerateBankFile() As Boolean
            Get
                ' If any of the row's [Hold_By] is not DB null and [Second_Authorised_By] is DB null, the payment file cannot be generated
                For Each dr As DataRow In Me.Rows
                    If Not IsDBNull(dr("Hold_By")) AndAlso IsDBNull(dr("Second_Authorised_By")) Then
                        Return False
                    End If
                Next

                Return True
            End Get
        End Property

        Public ReadOnly Property HoldSchemeCount() As Integer
            Get
                Dim intCount As Integer = 0

                For Each dr As DataRow In Me.Rows
                    If Not IsDBNull(dr("Hold_By")) Then
                        intCount += 1
                    End If
                Next

                Return intCount
            End Get
        End Property

    End Class
    ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

#End Region

#Region "Constructor"
    Public Sub New(ByVal authorizationLevel As String, ByVal searchCriteria As Common.SearchCriteria.SearchCriteria)
        AuthorizeLevel = authorizationLevel
        dtTxn = udtSearchEngineBLL.SearchTxn(searchCriteria)
    End Sub

    Public Sub New(ByVal authorizationLevel As String)
        AuthorizeLevel = authorizationLevel
    End Sub

    Public Sub New()

    End Sub
#End Region

    ''' <summary>
    ''' Deduce the default Cutoff date for reimbursement authorisation
    ''' </summary>
    ''' <returns>a string with date format dd-mm-yyyy</returns>
    ''' <remarks>
    ''' 1. The default date will be the last day of last month
    ''' 2. If the current day is the first day (1st) of the month, the default day will be last day of 2 months before.
    ''' </remarks>
    Public Function DeduceDefaultCutoffDate() As String
        Dim year, mth, day As Integer
        Dim newyear, newmth, newday As Integer

        year = Now.Year
        mth = Now.Month
        day = Now.Day

        If day <> 1 Then
            If mth > 1 Then
                newyear = year
                newmth = mth - 1
            ElseIf mth = 1 Then
                newyear = year - 1
                newmth = 12
            End If
        Else
            If mth > 2 Then
                newyear = year
                newmth = mth - 2
            ElseIf mth = 2 Then
                newyear = year - 1
                newmth = 12
            ElseIf mth = 1 Then
                newyear = year - 1
                newmth = 11
            End If
        End If

        newday = DateTime.DaysInMonth(newyear, newmth)

        Return IIf(newday > 9, newday, "0" & newday) & "-" & IIf(newmth > 9, newmth, "0" & newmth) & "-" & newyear

    End Function

    Public Function GetAuthorizationSummary(ByVal criteria As Common.SearchCriteria.SearchCriteria, ByVal authorised_status As Object, ByVal strReimburseID As String, ByVal strSchemeCode As String) As DataTable
        Dim dt As DataTable

        dt = udtSearchEngineBLL.SearchAuthorizationSummary(criteria, authorised_status, strReimburseID, strSchemeCode)

        Return dt
    End Function

    Public Function GetAuthorizationSummaryBySP(ByVal criteria As Common.SearchCriteria.SearchCriteria, ByVal authorised_Status As Object, ByVal strReimburseID As String, ByVal strSchemeCode As String, Optional ByVal bWithFirstAuthorization As Boolean = False) As DataTable
        Dim dt As DataTable

        dt = udtSearchEngineBLL.SearchAuthorizationSummaryBySP(criteria, authorised_Status, strReimburseID, strSchemeCode, bWithFirstAuthorization)

        Return dt
    End Function

    Public Function GetAuthorizationSummaryByBankAcct(ByVal criteria As Common.SearchCriteria.SearchCriteria, ByVal authorised_Status As Object, ByVal strSchemeCode As String, Optional ByVal bWithFirstAuthorization As Boolean = False) As DataTable
        Dim dt As DataTable

        dt = udtSearchEngineBLL.SearchAuthorizationSummaryByBankAcct(criteria, authorised_Status, strSchemeCode, bWithFirstAuthorization)

        Return dt
    End Function

    Public Function GetAuthorizationSummaryByTxn(ByVal criteria As Common.SearchCriteria.SearchCriteria, ByVal authorised_Status As Object, ByVal strSchemeCode As String, Optional ByVal bWithFirstAuthorization As Boolean = False) As DataTable
        Dim dt As DataTable

        dt = udtSearchEngineBLL.SearchAuthorizationSummaryByTxn(criteria, authorised_Status, strSchemeCode, bWithFirstAuthorization)

        Return dt
    End Function

    Public Function GetAuthorizationSummaryBySP_PaymentFile(ByVal strReimburseID As String, ByVal strSchemeCode As String) As DataTable
        Dim dt As DataTable

        dt = udtSearchEngineBLL.SearchAuthorizationSummaryBySP_PaymentFile(strReimburseID, strSchemeCode)

        Return dt
    End Function

    Public Function GetAuthorizationSummaryByBankAcct_PaymentFile(ByVal criteria As Common.SearchCriteria.SearchCriteria, ByVal strReimburseID As String, ByVal strSchemeCode As String) As DataTable
        Dim dt As DataTable

        dt = udtSearchEngineBLL.SearchAuthorizationSummaryByBankAcct_PaymentFile(criteria, strReimburseID, strSchemeCode)

        Return dt
    End Function

    Public Function GetAuthorizationSummaryByTxn_PaymentFile(ByVal criteria As Common.SearchCriteria.SearchCriteria, ByVal strReimburseID As String, ByVal strSchemeCode As String) As DataTable
        Dim dt As DataTable

        dt = udtSearchEngineBLL.SearchAuthorizationSummaryByTxn_PaymentFile(criteria, strReimburseID, strSchemeCode)

        Return dt
    End Function

    Public Function GetAuthorizationRowByTxnNo(ByVal criteria As Common.SearchCriteria.SearchCriteria, ByVal strSchemeCode As String) As DataRow
        Return udtSearchEngineBLL.GetSingleTxnRow(criteria, strSchemeCode)
    End Function

    Public Function GetAuthorizationSummaryByFirstAuthorization(ByVal criteria As Common.SearchCriteria.SearchCriteria, ByVal strUserID As String) As DataTable
        Dim dt As DataTable

        dt = udtSearchEngineBLL.SearchAuthorizationSummaryByFirstAuthorization(criteria, strUserID)

        Return dt
    End Function

#Region "Cancel Authorization"

    Public Function GetReimbursementCancelAuthorizationTransactionSummary(ByVal strReimID As String, ByVal strUserID As String, Optional ByVal udtDB As Database = Nothing)
        If IsNothing(udtDB) Then udtDB = New Database

        Dim dt As New DataTable

        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@reimburse_id", SqlDbType.Char, 15, strReimID), _
                                            udtDB.MakeInParam("@user_id", SqlDbType.Char, 20, strUserID)}
            udtDB.RunProc("proc_ReimbursementCancelAuthorization_get_byReimbursementID", prams, dt)

        Catch ex As Exception
            Throw ex
        End Try

        Return dt

    End Function

#End Region

#Region "Payment File Enquiry"
    '''' <summary>
    '''' Prepare for the latest 3 distinct generate payment file date
    '''' </summary>
    '''' <returns>The latest 3 dates in an ArrayList</returns>
    '''' <remarks></remarks>
    'Public Function GetLatestThreeReimbursementDate(ByVal strUserID As String) As ArrayList
    '    Dim dt As New DataTable
    '    Dim dr As DataRow
    '    Dim i As Integer
    '    Dim dateList As New ArrayList
    '    Dim udtcomfunct As New Common.ComFunction.GeneralFunction

    '    Dim intFileCount As Integer
    '    Dim strvalue As String = String.Empty
    '    udtcomfunct.getSystemParameter("ShowPaymentFileCount", strvalue, String.Empty)
    '    intFileCount = CInt(strvalue)

    '    dt = udtSearchEngineBLL.SearchLatestThreeReimbursement(strUserID)

    '    'Determin the number of distinct submission Date
    '    For i = 0 To dt.Rows.Count - 1
    '        dr = CType(dt.Rows(i), DataRow)

    '        If Not dateList.Contains(udtFormatter.formatDateTime(dr("createDate"), "EN")) Then
    '            If dateList.Count < intFileCount Then dateList.Add(udtFormatter.formatDateTime(dr("createDate"), "EN"))
    '        End If
    '    Next

    '    dateList.Insert(0, "Please Select")

    '    Return dateList
    'End Function

    Public Function GetLatestThreeReimbursementID(ByVal strUserID As String) As ListItemCollection
        Dim ddlItemCollection As New ListItemCollection()
        Dim aryItemList As New ArrayList

        Dim strParamValue As String = String.Empty
        udtGeneralFunction.getSystemParameter("ShowPaymentFileCount", strParamValue, String.Empty)
        Dim intFileCount As Integer = CInt(strParamValue)

        Dim dt As DataTable = udtSearchEngineBLL.SearchLatestThreeReimbursement(strUserID)

        ' Add the "--- Please Select ---"
        ddlItemCollection.Add(New ListItem(HttpContext.GetGlobalResourceObject("Text", "PleaseSelect")))

        For Each dr As DataRow In dt.Rows
            Dim strReimburseID As String = CStr(dr("Reimburse_ID")).Trim

            ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
            If Not aryItemList.Contains(strReimburseID) Then
                If aryItemList.Count < intFileCount Then
                    aryItemList.Add(strReimburseID)

                    ddlItemCollection.Add(New ListItem(strReimburseID, strReimburseID))
                End If
            End If
            ' CRE13-019-02 Extend HCVS to China [End][Lawrence]
        Next

        Return ddlItemCollection

    End Function

    Public Function GetReimbursementPaymentFileLists(ByVal strReimbursementID As String, ByVal dtmStartDate As Nullable(Of DateTime), ByVal dtmCutoffDate As Nullable(Of DateTime), ByVal strUserID As String) As DataTable
        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
        Return udtSearchEngineBLL.SearchReimbursementPaymentList(strReimbursementID, dtmStartDate, dtmCutoffDate, strUserID)
        ' CRE13-019-02 Extend HCVS to China [End][Lawrence]
    End Function

#End Region

    Private Function IsMatchSearchCriteria(ByVal status As String, ByVal drxml As DataRow) As Boolean
        If drxml("status").ToString.ToUpper.Equals(status.ToUpper) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub Authorize(ByVal criteria As Common.SearchCriteria.SearchCriteria, ByVal strUserID As String, ByVal strSchemeCode As String, Optional ByVal bWithFirstAuthorization As Boolean = False)
        Dim status As String

        If Me.AuthorizeLevel.Equals("1st Authorization") Then
            status = ReimbursementStatus.FirstAuthorised
        Else
            status = ReimbursementStatus.SecondAuthorised
        End If

        Me.UpdateMultipleTranStatusToDBForFirstAuthorization(status, criteria, strUserID, strSchemeCode)
    End Sub

    Public Sub VoidAuthorization(ByVal strUserID As String, ByVal strVoidRemark As String, ByVal strReimID As String, ByVal strScheme As String, Optional ByVal udtDB As Database = Nothing)
        If IsNothing(udtDB) Then udtDB = New Database

        Try
            udtDB.BeginTransaction()

            Dim prams() As SqlParameter = {udtDB.MakeInParam("@user_id", SqlDbType.VarChar, 20, strUserID), _
                                            udtDB.MakeInParam("@void_remark", SqlDbType.NVarChar, 255, strVoidRemark), _
                                            udtDB.MakeInParam("@reimburse_id", SqlDbType.Char, 15, strReimID), _
                                            udtDB.MakeInParam("@scheme_code", SqlDbType.Char, 10, strScheme)}

            udtDB.RunProc("proc_ReimbursementVoid_update_byTranIDAuthoriseStatus", prams)
            udtDB.CommitTransaction()

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex
        End Try

        'Return VoidMultipleAuthorisation(newStatus, criteria, voidReason, strUserID, strSchemeCode, strReimburseID)
    End Sub

    Private Sub UpdateVoucherTransactionStatus(ByRef udcDB As Database, ByVal strTranID As String, ByVal strStatus As String, ByVal tsmp As Byte())

        Dim dt As New DataTable

        Try
            ' create data object and params
            Dim prams() As SqlParameter = {udcDB.MakeInParam("@tran_id", SqlDbType.Char, 20, strTranID), _
                                            udcDB.MakeInParam("@record_status", SqlDbType.Char, 1, strStatus), _
                                            udcDB.MakeInParam("@tsmp", SqlDbType.Timestamp, 8, tsmp)}

            ' run the stored procedure
            udcDB.RunProc("proc_VoucherTransaction_update", prams, dt)
        Catch eSQL As SqlException
            'db.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            'db.RollBackTranscation()
            Throw ex
        End Try
    End Sub

    Public Function GetHoldAuthorizationSummary(ByVal strSchemeCode As String) As DataTable
        Return udtSearchEngineBLL.SearchHoldAuthorizationSummary(strSchemeCode)
    End Function

    Private Sub UpdateReimbursementAuthTran(ByRef udcDB As Database, ByVal strTranID As String, ByVal dtmAuthorisedDtm As DateTime, ByVal strAuthorisedBy As String, ByVal strAuthorisedStatus As String, ByVal tsmp As Byte())

        Dim dt As New DataTable

        Try
            ' create data object and params
            Dim prams() As SqlParameter = {udcDB.MakeInParam("@tran_id", SqlDbType.Char, 20, strTranID), _
                                            udcDB.MakeInParam("@authorised_dtm", SqlDbType.DateTime, 8, dtmAuthorisedDtm), _
                                            udcDB.MakeInParam("@authorised_by", SqlDbType.VarChar, 20, strAuthorisedBy), _
                                            udcDB.MakeInParam("@authorised_status", SqlDbType.Char, 1, strAuthorisedStatus), _
                                            udcDB.MakeInParam("@tsmp", SqlDbType.Timestamp, 8, tsmp)}

            ' run the stored procedure
            udcDB.RunProc("proc_ReimbursementAuthTran_update", prams, dt)
        Catch eSQL As SqlException
            'db.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            'db.RollBackTranscation()
            Throw ex
        End Try
    End Sub

    Private Sub UpdateReimbursementAuthTranForPaymentFile(ByRef udcDB As Database, ByVal strTranID As String, ByVal strReimburseID As String)

        Dim dt As New DataTable

        Try
            ' create data object and params
            Dim prams() As SqlParameter = {udcDB.MakeInParam("@tran_id", SqlDbType.Char, 20, strTranID), _
                                            udcDB.MakeInParam("@reimburse_id", SqlDbType.Char, 15, strReimburseID) _
                                            }

            ' run the stored procedure
            udcDB.RunProc("proc_ReimbursementAuthTran_update_forPaymentFile", prams, dt)
        Catch eSQL As SqlException
            'db.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            'db.RollBackTranscation()
            Throw ex
        End Try
    End Sub

    Private Sub InsertReimbursementAuthTran(ByRef udcDB As Database, ByVal strTranID As String, ByVal dtmAuthorisedDtm As DateTime, ByVal strAuthorisedBy As String, ByVal strReimbuserID As String, ByVal strAuthorisedStatus As String)

        Dim dt As New DataTable

        Try
            ' create data object and params
            Dim prams() As SqlParameter = {udcDB.MakeInParam("@tran_id", SqlDbType.Char, 20, strTranID), _
                                            udcDB.MakeInParam("@authorised_dtm", SqlDbType.DateTime, 8, dtmAuthorisedDtm), _
                                            udcDB.MakeInParam("@authorised_by", SqlDbType.VarChar, 20, strAuthorisedBy), _
                                            udcDB.MakeInParam("@reimburse_id", SqlDbType.Char, 15, strReimbuserID), _
                                            udcDB.MakeInParam("@authorised_status", SqlDbType.Char, 1, strAuthorisedStatus)}

            ' run the stored procedure
            udcDB.RunProc("proc_ReimbursementAuthTran_add", prams, dt)
        Catch eSQL As SqlException
            'udcDB.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            'udcDB.RollBackTranscation()
            Throw ex
        End Try
    End Sub

    Public Sub InsertReimbursementAuthorisation(ByRef udcDB As Database, ByVal strTranStatus As String, ByVal strAuthorisedBy As String, ByVal strAuthorisedStatus As String, ByVal strReimburseID As String, ByVal strUserID As String, ByVal dtmCutoff As Date, ByVal strSchemeCode As String, ByVal strVerfiyCaseAvail As String)

        Dim dt As New DataTable

        Try
            ' Force the cutoff date to have a time of 23:59:59
            If Not IsNothing(dtmCutoff) Then dtmCutoff = dtmCutoff.AddHours(23).AddMinutes(59).AddSeconds(59)

            Dim prams() As SqlParameter = {udcDB.MakeInParam("@tran_status", SqlDbType.Char, 1, strTranStatus), _
                                            udcDB.MakeInParam("@authorised_status", SqlDbType.Char, 1, strAuthorisedStatus), _
                                            udcDB.MakeInParam("@authorised_by", SqlDbType.VarChar, 20, strAuthorisedBy), _
                                            udcDB.MakeInParam("@reimburse_id", SqlDbType.Char, 15, strReimburseID), _
                                            udcDB.MakeInParam("@current_user", SqlDbType.VarChar, 20, strUserID), _
                                            udcDB.MakeInParam("@cutoff_dtm", SqlDbType.DateTime, 8, dtmCutoff), _
                                            udcDB.MakeInParam("@scheme_code", SqlDbType.Char, 10, strSchemeCode), _
                                            udcDB.MakeInParam("@Verification_Case_Available", SqlDbType.Char, 1, strVerfiyCaseAvail)} ' CRE17-004 Generate a new DPAR on EHCP basis [Dickson]

            udcDB.RunProc("proc_ReimbursementAuthorisation_add", prams, dt)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub UpdateReimbursementAuthorisationByBackAndRelease(ByRef udcDB As Database, ByVal strReimburseID As String, ByVal strUserID As String)

        Dim dt As New DataTable

        Try
            ' create data object and params
            Dim prams() As SqlParameter = {udcDB.MakeInParam("@reimburse_id", SqlDbType.Char, 15, strReimburseID), _
                                            udcDB.MakeInParam("@current_user", SqlDbType.VarChar, 20, strUserID) _
                                            }

            ' run the stored procedure
            udcDB.RunProc("proc_ReimbursementAuthorisation_upd_byBankNRelease", prams, dt)
        Catch eSQL As SqlException
            'db.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            'db.RollBackTranscation()
            Throw ex
        End Try
    End Sub

    'Private Sub UpdateMultipleTranStatusToDB(ByVal newStatus As String, ByVal criteria As Common.SearchCriteria.SearchCriteria, ByVal strUserID As String, ByVal strSchemeCode As String, ByVal bWithFirstAuthorization As Boolean)
    '    Dim dt As New DataTable
    '    Dim updatetime As String
    '    Dim db As New Database

    '    updatetime = udcGeneralF.GetSystemDateTime

    '    'Call store proc to update transaction
    '    Try
    '        db.BeginTransaction()
    '        If bWithFirstAuthorization Then
    '            ' create data object and params
    '            Dim prams() As SqlParameter = {db.MakeInParam("@tran_status", SqlDbType.Char, 1, criteria.TransStatus), _
    '                                            db.MakeInParam("@cutoff_dtm", SqlDbType.DateTime, 1, criteria.CutoffDate), _
    '                                            db.MakeInParam("@authorised_dtm", SqlDbType.DateTime, 1, updatetime), _
    '                                            db.MakeInParam("@authorised_status", SqlDbType.Char, 1, newStatus), _
    '                                            db.MakeInParam("@authorised_by", SqlDbType.VarChar, 20, strUserID), _
    '                                            db.MakeInParam("@current_user", SqlDbType.VarChar, 20, strUserID), _
    '                                            db.MakeInParam("@first_authorized_dtm", SqlDbType.DateTime, 8, criteria.FirstAuthorizedDate), _
    '                                            db.MakeInParam("@first_authorized_by", SqlDbType.VarChar, 20, criteria.FirstAuthorizedBy), _
    '                                            db.MakeInParam("@scheme_code", SqlDbType.Char, 10, strSchemeCode)}
    '            ' run the stored procedure
    '            db.RunProc("proc_ReimbursementAuthorise_insert_update", prams, dt)
    '        Else
    '            ' create data object and params
    '            Dim prams() As SqlParameter = {db.MakeInParam("@tran_status", SqlDbType.Char, 1, criteria.TransStatus), _
    '                                            db.MakeInParam("@cutoff_dtm", SqlDbType.DateTime, 1, criteria.CutoffDate), _
    '                                            db.MakeInParam("@authorised_dtm", SqlDbType.DateTime, 1, updatetime), _
    '                                            db.MakeInParam("@authorised_status", SqlDbType.Char, 1, newStatus), _
    '                                            db.MakeInParam("@authorised_by", SqlDbType.VarChar, 20, strUserID), _
    '                                            db.MakeInParam("@current_user", SqlDbType.VarChar, 20, strUserID), _
    '                                            db.MakeInParam("@scheme_code", SqlDbType.Char, 10, strSchemeCode)}
    '            ' run the stored procedure
    '            db.RunProc("proc_ReimbursementFirstAuthorise_add_update", prams, dt)
    '        End If

    '        db.CommitTransaction()
    '    Catch eSQL As SqlException
    '        db.RollBackTranscation()
    '        Throw eSQL
    '    Catch ex As Exception
    '        db.RollBackTranscation()
    '        Throw ex
    '    End Try
    'End Sub

    ''' <summary>
    ''' First Authorize the transactions
    ''' </summary>
    ''' <param name="newStatus"></param>
    ''' <param name="criteria"></param>
    ''' <param name="strUserID"></param>
    ''' <param name="strSchemeCode"></param>
    ''' <remarks></remarks>
    Private Sub UpdateMultipleTranStatusToDBForFirstAuthorization(ByVal newStatus As String, ByVal criteria As Common.SearchCriteria.SearchCriteria, ByVal strUserID As String, ByVal strSchemeCode As String)
        Dim dt As New DataTable
        Dim updatetime As String
        Dim db As New Database

        updatetime = udtGeneralFunction.GetSystemDateTime

        'Call store proc to update transaction
        Try
            'db.CommandTimeout = 300
            db.BeginTransaction()

            Me.ReimbursementFirstAuthorization(db, strUserID, strSchemeCode)

            'db.RollBackTranscation()
            db.CommitTransaction()
        Catch eSQL As SqlException
            db.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            db.RollBackTranscation()
            Throw ex
        End Try
    End Sub

    'Private Function UpdateMultipleTranStatusToDBAndGeneratePaymentFile(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry, ByVal newStatus As String, ByVal criteria As Common.SearchCriteria.SearchCriteria, ByVal strUserID As String, ByVal strSchemeCode As String, ByVal strReimbursementID As String, ByVal dtmBankPaymentDay As DateTime, ByVal bWithFirstAuthorization As Boolean, ByVal strCutoffDate As String) As Boolean
    '    Dim dt As New DataTable
    '    Dim updatetime As String
    '    Dim i, j As Integer
    '    Dim db As New Database
    '    Dim strTransNum As String
    '    Dim intVouchersCount As Integer = 0
    '    Dim dblTotalAmt As Double = 0
    '    Dim strSecondPartyIdentifier As String
    '    Dim alReimbursementID As New ArrayList
    '    Dim alDataKey As New ArrayList
    '    Dim alHolder As New ArrayList
    '    Dim alBankCode As New ArrayList
    '    Dim alBranchCode As New ArrayList
    '    Dim alBankAccountCode As New ArrayList
    '    Dim alAmount As New ArrayList

    '    updatetime = udtGeneralFunction.GetSystemDateTime

    '    'Call store proc to update transaction
    '    Try
    '        'db.CommandTimeout = 300
    '        db.BeginTransaction()

    '        ''Replace Generate payment file start
    '        'dt = Me.GetTransactionIDForSecondAuth(db, criteria.TransStatus, Common.Component.ReimbursementStatus.SecondAuthorised, criteria.FirstAuthorizedDate, criteria.FirstAuthorizedBy, strSchemeCode, strReimbursementID)
    '        'udtAuditLogEntry.WriteLog("99901", "Tran list count:" & dt.Rows.Count)
    '        'For i = 0 To dt.Rows.Count - 1
    '        '    strTransNum = formater.formatSystemNumberReverse(Trim(dt.Rows(i)(0)))
    '        '    udtAuditLogEntry.WriteLog("99902", "Trans_ID:" & strTransNum)
    '        '    intVouchersCount = Int(Trim(dt.Rows(i)("voucherRedeem"))) + intVouchersCount
    '        '    'dblTotalAmt = dblTotalAmt + (Int(Trim(dt.Rows(i)("voucherRedeem"))) * Int(Trim(dt.Rows(i)("voucherAmount"))))
    '        '    dblTotalAmt = dblTotalAmt + Int(Trim(dt.Rows(i)("totalAmount")))

    '        '    Me.UpdateReimbursementAuthTran(db, strTransNum, updatetime, strUserID, Common.Component.ReimbursementStatus.SecondAuthorised, dt.Rows(i)("RTTSMP"))
    '        '    udtAuditLogEntry.WriteLog("99903", "After update ReimbursementAuthTran")
    '        '    Me.UpdateVoucherTransactionAuthorisedStatus(db, strTransNum, Common.Component.ReimbursementStatus.SecondAuthorised, strSchemeCode, dt.Rows(i)("TSMP"))
    '        '    udtAuditLogEntry.WriteLog("99904", "After update Voucher Transaction Authorized status")
    '        '    'Next

    '        '    'dt = Me.GetAuthorizationSummaryByTxn_PaymentFile(criteria, Common.Component.ReimbursementStatus.SecondAuthorised, strSchemeCode)

    '        '    'For i = 0 To dt.Rows.Count - 1
    '        '    '    strTransNum = formater.formatSystemNumberReverse(Trim(dt.Rows(i)("transNum")))

    '        '    'Insert into BankInTransaction table
    '        '    'InsertBankInTransaction(db, strTransNum, strReimbursementID)

    '        '    Dim strBankValues(2) As String
    '        '    strBankValues = formater.splitBankAcct(Trim(dt.Rows(i)("bankAccount")))

    '        '    strSecondPartyIdentifier = dt.Rows(i)("SP_ID").ToString.Trim & " " & FillLeftZero(CInt(dt.Rows(i)("practice_display_seq").ToString.Trim), 2)

    '        '    ''Insert into BankInDataFile table
    '        '    'InsertBankInDataFile(db, strReimbursementID, i + 1, FillRightSpace(strSecondPartyIdentifier, 12), FillRightSpace(Left(dt.Rows(i)("SP_Eng_Name").ToString.Trim, 20), 20), FillRightSpace(strBankValues(0), 3), FillRightSpace(strBankValues(1), 3), FillRightSpace(strBankValues(2), 9), FillLeftZero(CInt(Trim(dt.Rows(i)("totalAmount"))) * 100, 10), FillRightSpace("", 6), FillRightSpace("", 12))
    '        '    'Prepare BankInData
    '        '    If alDataKey.Contains(strSecondPartyIdentifier) Then
    '        '        For j = 0 To alDataKey.Count - 1
    '        '            If alDataKey(j).ToString.Equals(strSecondPartyIdentifier) Then
    '        '                alAmount(j) = alAmount(j) + CInt(Trim(dt.Rows(i)("totalAmount")))
    '        '                Exit For
    '        '            End If
    '        '        Next
    '        '    Else
    '        '        alDataKey.Add(strSecondPartyIdentifier)
    '        '        alReimbursementID.Add(strReimbursementID)
    '        '        alHolder.Add(Left(dt.Rows(i)("SP_Eng_Name").ToString.Trim, 20))
    '        '        alBankCode.Add(strBankValues(0))
    '        '        alBranchCode.Add(strBankValues(1))
    '        '        alBankAccountCode.Add(strBankValues(2))
    '        '        alAmount.Add(CInt(Trim(dt.Rows(i)("totalAmount"))))
    '        '    End If

    '        '    'Update the VoucherTransaction table
    '        '    UpdateVoucherTransactionStatus(db, strTransNum, ClaimTransStatus.Reimbursed, dt.Rows(i)("TSMP"))
    '        '    udtAuditLogEntry.WriteLog("99905", "After update Voucher Transaction record status")
    '        '    'Delete the ReimbursementAuthTran table
    '        '    'DeleteReimbursementAuthTran(strTransNum)

    '        '    'Update the ReimbursementAuthTran table with Paymentfile info
    '        '    UpdateReimbursementAuthTranForPaymentFile(db, strTransNum, strReimbursementID)
    '        '    udtAuditLogEntry.WriteLog("99906", "After update ReimbursementAuthTranForPaymentFile")
    '        'Next

    '        ''insert into Reimbursement Authorisation for 2nd Authorisation
    '        'Me.InsertReimbursementAuthorisation(db, criteria.TransStatus, updatetime, strUserID, Common.Component.ReimbursementStatus.SecondAuthorised, strReimbursementID, strUserID, Now.Date)    'Second Authorization won't handle the cutoff date
    '        'udtAuditLogEntry.WriteLog("99907", "After InsertReimbursementAuthorisation")
    '        ''insert into BankIn table
    '        'InsertBankIn(db, strReimbursementID, strUserID, dt.Rows.Count, intVouchersCount, dblTotalAmt, dtmBankPaymentDay)
    '        'udtAuditLogEntry.WriteLog("99908", "After InsertBankIn")
    '        ''Insert into BankInDataFile table
    '        'For j = 0 To alDataKey.Count - 1
    '        '    InsertBankInDataFile(db, strReimbursementID, j + 1, FillRightSpace(alDataKey(j).ToString.Trim, 12), FillRightSpace(Left(alHolder(j).ToString.Trim, 20), 20), FillRightSpace(alBankCode(j).ToString.Trim, 3), FillRightSpace(alBranchCode(j).ToString.Trim, 3), FillRightSpace(alBankAccountCode(j).ToString.Trim, 9), FillLeftZero(CInt(alAmount(j).ToString.Trim) * 100, 10), FillRightSpace("", 6), FillRightSpace("", 12))
    '        'Next
    '        'udtAuditLogEntry.WriteLog("99909", "After InsertBankInDataFile, count:" & alDataKey.Count)

    '        ''Get the Bank account information
    '        'Dim strFirstPartyAccount As String = String.Empty
    '        'udcGeneralF.getSystemParameter("BankFileFirstPartyAccount", strFirstPartyAccount, String.Empty)

    '        'Dim strPaymentCode As String = String.Empty
    '        'udcGeneralF.getSystemParameter("BankFilePaymentCode", strPaymentCode, String.Empty)

    '        ''Insert into BankInHeaderFile table
    '        'InsertBankInHeaderFile(db, strReimbursementID, FillRightSpace("F", 1), FillRightSpace(strFirstPartyAccount.Trim, 12), FillRightSpace(strPaymentCode.Trim, 3), FillRightSpace("HCVU pmt", 12), formater.formatBankPaymentDay(formater.formatEnterDate(dtmBankPaymentDay)), "********", FillRightSpace("", 5), FillRightSpace("", 10), FillLeftZero(alDataKey.Count.ToString, 7), FillLeftZero(CInt(dblTotalAmt) * 100, 12))
    '        'udtAuditLogEntry.WriteLog("99910", "After InsertBankInHeaderFile")
    '        ''Replace Generate payment file end
    '        'Get the Bank account information
    '        Dim strFirstPartyAccount As String = String.Empty
    '        udtGeneralFunction.getSystemParameter("BankFileFirstPartyAccount", strFirstPartyAccount, String.Empty)

    '        Dim strPaymentCode As String = String.Empty
    '        udtGeneralFunction.getSystemParameter("BankFilePaymentCode", strPaymentCode, String.Empty)
    '        'Me.ReimbursementSecondAuthorization(db, strUserID, strReimbursementID, FillRightSpace("F", 1), FillRightSpace(strFirstPartyAccount.Trim, 12), FillRightSpace(strPaymentCode.Trim, 3), FillRightSpace("HCVU pmt", 12), udtFormatter.formatBankPaymentDay(udtFormatter.formatEnterDate(dtmBankPaymentDay)), "********", dtmBankPaymentDay)

    '        ' ----------------------------------------------------------------------------------
    '        ' Add FileGenerationQueue Record For bank file and super download file
    '        If Not (AddFileGenerationQueue(db, strReimbursementID.Trim, strReimbursementID.Trim & ".txt", Common.Component.DataDownloadFileID.BankPaymentFile, strUserID, strCutoffDate) And _
    '            AddFileGenerationQueue(db, strReimbursementID.Trim, strReimbursementID.Trim & ".xls", Common.Component.DataDownloadFileID.SuperDownload, strUserID, strCutoffDate)) Then
    '            'Either 1 or 2 fail
    '            udtAuditLogEntry.WriteLog("99912", "After Adding file generation queue fail: No associated user found.")
    '            db.RollBackTranscation()
    '            Return False
    '        Else
    '            udtAuditLogEntry.WriteLog("99911", "After Adding file generation queue OK")
    '            db.CommitTransaction()
    '            Return True
    '        End If
    '    Catch eSQL As SqlException
    '        udtAuditLogEntry.WriteLog("99913", "SQL Exception:" & eSQL.ToString)
    '        db.RollBackTranscation()
    '        Throw eSQL
    '    Catch ex As Exception
    '        udtAuditLogEntry.WriteLog("99914", "General Exception:" & ex.ToString)
    '        db.RollBackTranscation()
    '        Throw ex
    '    End Try
    'End Function

    ' --- CRE16-026-04 (Add PCV13) [Start] (Marco) ---
    Public Sub GenerateReimbursementReport(ByVal strReimbursementID As String, ByVal strSchemeCode As String, Optional ByVal udtDB As Database = Nothing)
        If IsNothing(udtDB) Then udtDB = New Database

        Select Case strSchemeCode
            Case SchemeClaimModel.VSS
                Dim udtCommon As New Common.ComFunction.GeneralFunction()
                Dim udtParamFunction As New Common.ComFunction.ParameterFunction()
                Dim udtSpParamCollection As New StoreProcParamCollection()
                Dim udtFileGenerationBLL As New FileGeneration.FileGenerationBLL()
                Dim udtFileGenerationQueueModel As New FileGeneration.FileGenerationQueueModel()
                Dim strUserID As String = udtHCVUUserBLL.GetHCVUUser.UserID.Trim
                Dim strFileID As String = DataDownloadFileID.eHSM0007

                Dim udtFileGenerationModel As FileGenerationModel = (New FileGenerationBLL).RetrieveFileGeneration(udtDB, strFileID)
                Dim strOutputFileName As String = udtFileGenerationModel.OutputFileName(DateTime.Now)

                udtSpParamCollection.AddParam("@reimburse_id", SqlDbType.Char, 15, strReimbursementID)

                udtFileGenerationQueueModel.GenerationID = udtCommon.generateFileSeqNo()
                udtFileGenerationQueueModel.FileID = udtFileGenerationModel.FileID
                udtFileGenerationQueueModel.InParm = udtParamFunction.GetSPParamString(udtSpParamCollection)
                udtFileGenerationQueueModel.OutputFile = udtCommon.generateFileOutputPath(strOutputFileName)
                udtFileGenerationQueueModel.Status = Common.Component.DataDownloadStatus.Pending
                udtFileGenerationQueueModel.FilePassword = ""
                udtFileGenerationQueueModel.RequestBy = strUserID
                udtFileGenerationQueueModel.FileDescription = udtFileGenerationModel.FileName + "-" + strOutputFileName
                ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                udtFileGenerationQueueModel.ScheduleGenDtm = Nothing
                ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]

                udtFileGenerationBLL.AddFileGenerationQueue(udtDB, udtFileGenerationQueueModel)

                Dim udtDataDownloadBLL As New DatadownloadBLL()
                udtDataDownloadBLL.InsertFileDownloadRecordsToUsersForFileGeneration(udtDB, udtFileGenerationQueueModel.GenerationID)
        End Select

    End Sub
    ' --- CRE16-026-04 (Add PCV13) [End] (Marco) ---

    Public Sub GeneratePaymentFile(ByVal strSchemeCode As String, ByVal strSchemeDisplayCode As String, ByVal strReimbursementID As String, ByVal dtmPaymentDate As DateTime, Optional ByVal udtDB As Database = Nothing)
        If IsNothing(udtDB) Then udtDB = New Database

        Try
            ' Get the cut off date from [ReimbursementAuthorisation] using the reimbursement ID
            Dim dt As DataTable = GetReimbursementAuthorisationByIDStatus(strReimbursementID, ReimbursementStatus.StartReimbursement, ReimbursementAuthorisationStatus.Active, Nothing)

            Dim strCutoffDate As String = String.Empty
            If dt.Rows.Count = 1 Then
                strCutoffDate = dt.Rows(0)("CutOff_Date")
            End If

            ' Prepare data
            Dim strTempParaValue As String = String.Empty

            Dim strUserID As String = udtHCVUUserBLL.GetHCVUUser.UserID.Trim
            Dim strAutoPlanCode As String = "F"

            udtGeneralFunction.getSystemParameter("BankFileFirstPartyAccount", strTempParaValue, String.Empty)
            Dim strFirstPartyAccount As String = strTempParaValue.Trim.PadRight(12, " ")

            udtGeneralFunction.getSystemParameter("BankFilePaymentCode", strTempParaValue, String.Empty, strSchemeCode)
            Dim strPaymentCode As String = strTempParaValue.Trim.PadRight(3, " ")

            udtGeneralFunction.getSystemParameter("BankFileFirstPartyReference", strTempParaValue, String.Empty, strSchemeCode)
            Dim strFirstPartyReference As String = strTempParaValue.Trim.PadRight(12, " ")

            Dim strValueDate As String = udtFormatter.formatBankPaymentDay(udtFormatter.formatInputDate(dtmPaymentDate))
            Dim strFileName As String = "********"

            udtDB.BeginTransaction()

            ' Generate payment file
            GeneratePaymentFileToDatabase(strUserID, strReimbursementID, dtmPaymentDate, strSchemeCode, strAutoPlanCode, strFirstPartyAccount, strPaymentCode, _
                                            strFirstPartyReference, strValueDate, strFileName, udtDB)

            ' Add to file queue
            Dim udtFileGenerationBLL As New FileGeneration.FileGenerationBLL()
            Dim udtSchemeClaim As SchemeClaimModel = (New SchemeClaimBLL).getAllDistinctSchemeClaim.Filter(strSchemeCode)
            Dim strSuperFileID As String = String.Empty

            If udtSchemeClaim.ReimbursementCurrency = EnumReimbursementCurrency.HKDRMB Then
                strSuperFileID = DataDownloadFileID.SuperDownloadRMB
            Else
                strSuperFileID = DataDownloadFileID.SuperDownload
            End If

            Dim udtFileGenerationModelSuper As FileGenerationModel = udtFileGenerationBLL.RetrieveFileGeneration(udtDB, strSuperFileID)
            Dim strSuperFileName As String = strReimbursementID + "-" + strSchemeDisplayCode + (New Common.Format.Formatter).FormatFileExt(udtFileGenerationModelSuper.FileType)

            ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            AddFileGenerationQueue(udtDB, strReimbursementID, strSuperFileName, strSuperFileID, strUserID, strCutoffDate, strSchemeCode)
            ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

            If udtSchemeClaim.ReimbursementMode = EnumReimbursementMode.All Then
                Dim udtFileGenerationModelBank As Common.Component.FileGeneration.FileGenerationModel = udtFileGenerationBLL.RetrieveFileGeneration(udtDB, DataDownloadFileID.BankPaymentFile)
                Dim strBankFileName As String = strReimbursementID + "-" + strSchemeDisplayCode + (New Common.Format.Formatter).FormatFileExt(udtFileGenerationModelBank.FileType)

                ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                AddFileGenerationQueue(udtDB, strReimbursementID, strBankFileName, DataDownloadFileID.BankPaymentFile, strUserID, strCutoffDate, strSchemeCode)
                ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

            End If

            GenerateReimbursementReport(strReimbursementID, strSchemeCode)

            udtDB.CommitTransaction()

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw
        End Try

    End Sub

    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Sub GeneratePreAuthorizationCheckingFile(ByVal strSchemeCode As String, ByVal strSchemeDisplayCode As String, ByVal strReimbursementID As String, ByRef udtDB As Database)

        ' Get the cut off date from [ReimbursementAuthorisation] using the reimbursement ID
        Dim dt As DataTable = GetReimbursementAuthorisationByIDStatus(strReimbursementID, ReimbursementStatus.StartReimbursement, ReimbursementAuthorisationStatus.Active, Nothing, udtDB)

        Dim strCutoffDate As String = String.Empty
        If dt.Rows.Count = 1 Then
            strCutoffDate = dt.Rows(0)("CutOff_Date")
        End If

        ' Get current UserID
        Dim strUserID As String = udtHCVUUserBLL.GetHCVUUser.UserID.Trim

        Dim udtSchemeClaim As SchemeClaimModel = (New SchemeClaimBLL).getAllDistinctSchemeClaim.Filter(strSchemeCode)
        Dim strFileID As String = String.Empty

        If udtSchemeClaim.ReimbursementCurrency = EnumReimbursementCurrency.HKDRMB Then
            strFileID = DataDownloadFileID.PreAuthorizationCheckingRMB
        Else
            strFileID = DataDownloadFileID.PreAuthorizationChecking
        End If

        Dim udtFileGenerationModel As FileGenerationModel = (New FileGenerationBLL).RetrieveFileGeneration(udtDB, strFileID)

        ' Add to file queue
        Dim strPreAuthorizationFileName As String = strReimbursementID + "-" + strSchemeDisplayCode + "-PA" + Year(DateTime.Now).ToString().Substring(2, 2) + Month(DateTime.Now).ToString().PadLeft(2, "0") + Day(DateTime.Now).ToString().PadLeft(2, "0") + Hour(DateTime.Now).ToString().PadLeft(2, "0") + Minute(DateTime.Now).ToString().PadLeft(2, "0") + (New Common.Format.Formatter).FormatFileExt(udtFileGenerationModel.FileType)

        AddFileGenerationQueue(udtDB, strReimbursementID, strPreAuthorizationFileName, strFileID, strUserID, strCutoffDate, strSchemeCode)

    End Sub
    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

    Private Sub GeneratePaymentFileToDatabase(ByVal strUserID As String, ByVal strReimID As String, ByVal dtmPaymentDate As DateTime, ByVal strScheme As String, _
                                                ByVal strAutoPlanCode As String, ByVal strFirstPartyAccNo As String, ByVal strPaymentCode As String, _
                                                ByVal strFirstPartyReference As String, ByVal strValueDate As String, ByVal strFileName As String, _
                                                ByVal udtDB As Database)
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@current_user", SqlDbType.VarChar, 20, strUserID), _
                                            udtDB.MakeInParam("@reimburse_ID", SqlDbType.Char, 15, strReimID), _
                                            udtDB.MakeInParam("@bank_payment_dtm", SqlDbType.DateTime, 8, dtmPaymentDate), _
                                            udtDB.MakeInParam("@scheme_code", SqlDbType.Char, 10, strScheme), _
                                            udtDB.MakeInParam("@Auto_Plan_Code", SqlDbType.Char, 1, strAutoPlanCode), _
                                            udtDB.MakeInParam("@First_Party_Acc_No", SqlDbType.Char, 12, strFirstPartyAccNo), _
                                            udtDB.MakeInParam("@Payment_Code", SqlDbType.Char, 3, strPaymentCode), _
                                            udtDB.MakeInParam("@First_Party_Reference", SqlDbType.Char, 12, strFirstPartyReference), _
                                            udtDB.MakeInParam("@Value_Date", SqlDbType.Char, 6, strValueDate), _
                                            udtDB.MakeInParam("@File_Name", SqlDbType.Char, 8, strFileName) _
                                            }

            udtDB.RunProc("proc_ReimbursementGeneratePaymentFile_add", prams)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function AddFileGenerationQueue(ByRef db As Database, ByVal strReimbursementID As String, ByVal strFileName As String, ByVal strFileID As String, ByVal strUserID As String, ByVal strCutoffDate As String, ByVal strScheme As String) As Boolean
        ' ----------------------------------------------------------------------------------
        ' Add FileGenerationQueue Record For bank file
        Dim udtFileGenerationBLL As New FileGeneration.FileGenerationBLL()
        Dim udtFileGenerationQueueModel As New FileGeneration.FileGenerationQueueModel()

        Dim udtFileGenerationModel As FileGeneration.FileGenerationModel = udtFileGenerationBLL.RetrieveFileGeneration(db, strFileID)

        Dim udtParamFunction As New Common.ComFunction.ParameterFunction()
        Dim udtCommon As New Common.ComFunction.GeneralFunction()

        Dim udtSpParamCollection As New StoreProcParamCollection()
        udtSpParamCollection.AddParam("@reimburse_id", SqlDbType.Char, 15, strReimbursementID)
        udtSpParamCollection.AddParam("@scheme_code", SqlDbType.Char, 10, strScheme)

        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
        If strFileID.Equals(DataDownloadFileID.SuperDownload) _
                OrElse strFileID.Equals(DataDownloadFileID.SuperDownloadRMB) _
                OrElse strFileID.Equals(DataDownloadFileID.PreAuthorizationChecking) _
                OrElse strFileID.Equals(DataDownloadFileID.PreAuthorizationCheckingRMB) Then
            udtSpParamCollection.AddParam("@cutoff_Date_str", SqlDbType.Char, 11, udtFormatter.convertDateTime(strCutoffDate))
        End If
        ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

        udtFileGenerationQueueModel.GenerationID = udtCommon.generateFileSeqNo()
        udtFileGenerationQueueModel.FileID = strFileID
        udtFileGenerationQueueModel.InParm = udtParamFunction.GetSPParamString(udtSpParamCollection)
        udtFileGenerationQueueModel.OutputFile = udtCommon.generateFileOutputPath(strFileName)
        udtFileGenerationQueueModel.Status = Common.Component.DataDownloadStatus.Pending
        udtFileGenerationQueueModel.FilePassword = udtCommon.generateSystemPassword(strUserID)
        udtFileGenerationQueueModel.RequestBy = strUserID
        udtFileGenerationQueueModel.FileDescription = udtFileGenerationModel.FileName + "-" + strFileName
        ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        udtFileGenerationQueueModel.ScheduleGenDtm = Nothing
        ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]

        udtFileGenerationBLL.AddFileGenerationQueue(db, udtFileGenerationQueueModel)

        Dim udtDataDownloadBLL As New DatadownloadBLL()
        Return udtDataDownloadBLL.InsertFileDownloadRecordsToUsersForFileGeneration(db, udtFileGenerationQueueModel.GenerationID)
    End Function

    'Private Function VoidMultipleAuthorisation(ByVal currentAuthorisationStatus As String, ByVal criteria As Common.SearchCriteria.SearchCriteria, ByVal voidReason As String, ByVal strUserID As String, ByVal strSchemeCode As String, ByVal strReimburseID As String) As Boolean
    '    Dim dt As New DataTable
    '    Dim updatetime As String
    '    Dim udcDB As New Database
    '    Dim blnRes As Boolean

    '    updatetime = udtGeneralFunction.GetSystemDateTime

    '    'Call store proc to update transaction
    '    Try
    '        'udcDB.CommandTimeout = 300
    '        udcDB.BeginTransaction()
    '        ' create data object and params
    '        Dim prams() As SqlParameter = {udcDB.MakeInParam("@tran_status", SqlDbType.Char, 1, Left(criteria.TransStatus, 1).ToUpper), _
    '                                        udcDB.MakeInParam("@authorised_dtm", SqlDbType.DateTime, 1, IIf(currentAuthorisationStatus.Equals(ReimbursementStatus.FirstAuthorised), criteria.FirstAuthorizedDate, criteria.SecondAuthorizedDate)), _
    '                                        udcDB.MakeInParam("@authorised_status", SqlDbType.Char, 1, currentAuthorisationStatus), _
    '                                        udcDB.MakeInParam("@authorised_by", SqlDbType.VarChar, 20, strUserID), _
    '                                        udcDB.MakeInParam("@current_user", SqlDbType.VarChar, 20, strUserID), _
    '                                        udcDB.MakeInParam("@void_remark", SqlDbType.NVarChar, 255, voidReason), _
    '                                        udcDB.MakeInParam("@reimburse_id", SqlDbType.Char, 15, strReimburseID), _
    '                                        udcDB.MakeInParam("@scheme_code", SqlDbType.Char, 10, strSchemeCode)}
    '        ' run the stored procedure
    '        udcDB.RunProc("proc_ReimbursementVoid_update_byTranIDAuthoriseStatus", prams, dt)
    '        udcDB.CommitTransaction()
    '        blnRes = True
    '    Catch eSQL As SqlException
    '        udcDB.RollBackTranscation()
    '        blnRes = False
    '        Throw eSQL
    '    Catch ex As Exception
    '        udcDB.RollBackTranscation()
    '        blnRes = False
    '        Throw ex
    '    End Try
    'End Function

#Region "Generate Payment File"

    ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
    Public Function GetBankIn(ByVal strReimbursementID As String, ByVal strSchemeCode As String) As DataTable
        Dim udtDB As New Database
        Dim dt As New DataTable

        Dim prams() As SqlParameter = { _
            udtDB.MakeInParam("@Reimburse_ID", SqlDbType.Char, 15, strReimbursementID), _
            udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, strSchemeCode) _
        }
        udtDB.RunProc("proc_BankIn_get_byKey", prams, dt)

        Return dt

    End Function
    ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

    Private Function CreatePhysicalBankInFile(ByVal strReimburseID As String) As String
        'Dim db As New Database

        Try
            Return "\DataFile\" & strReimburseID & ".txt"
        Catch eSQL As SqlException
            'db.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            'db.RollBackTranscation()
            Throw ex
        End Try
    End Function

    Private Function GetBankFileValueDate() As String
        Return IIf(Now.Day > 9, Now.Day.ToString, "0" & Now.Day.ToString) & IIf(Now.Month > 9, Now.Month.ToString, "0" & Now.Month.ToString) & Right(Now.Year.ToString, 2)
    End Function

    Public Function ReimbursementSecondAuthorization(ByVal strUserID As String, ByVal strReimbursementID As String, ByVal strSchemeCode As String) As Boolean
        Return ReimbursementSecondAuthorizationCore(strUserID, strReimbursementID, strSchemeCode)
    End Function

    'Public Sub GeneratePaymentFile(ByVal criteria As Common.SearchCriteria.SearchCriteria, ByVal strReimbursementID As String, ByVal strUserID As String, ByVal strSchemeCode As String)
    '    Dim i As Integer
    '    Dim dt As New DataTable
    '    Dim strTransNum As String
    '    Dim intVouchersCount As Integer = 0
    '    Dim dblTotalAmt As Double = 0
    '    Dim db As New Database

    '    Try
    '        'db.CommandTimeout = 300
    '        db.BeginTransaction()
    '        dt = Me.GetAuthorizationSummaryByTxn_PaymentFile(criteria, Common.Component.ReimbursementStatus.SecondAuthorised, strSchemeCode)

    '        For i = 0 To dt.Rows.Count - 1
    '            strTransNum = udtFormatter.formatSystemNumberReverse(Trim(dt.Rows(i)("transNum")))
    '            intVouchersCount = Int(Trim(dt.Rows(i)("voucherRedeem"))) + intVouchersCount
    '            'dblTotalAmt = dblTotalAmt + (Int(Trim(dt.Rows(i)("voucherRedeem"))) * Int(Trim(dt.Rows(i)("voucherAmount"))))
    '            dblTotalAmt = dblTotalAmt + Int(Trim(dt.Rows(i)("totalAmount")))

    '            'Insert into BankInTransaction table
    '            'InsertBankInTransaction(db, strTransNum, strReimbursementID)

    '            Dim strBankValues(2) As String
    '            strBankValues = udtFormatter.splitBankAcct(Trim(dt.Rows(i)("bankAccount")))

    '            'Insert into BankInDataFile table
    '            InsertBankInDataFile(db, strReimbursementID, i + 1, FillRightSpace("", 12), FillRightSpace("", 20), strBankValues(0), strBankValues(1), strBankValues(2), FillLeftZero(Trim(dt.Rows(i)("totalAmount")), 10), FillRightSpace("", 6), FillRightSpace("", 12))

    '            'Update the VoucherTransaction table
    '            UpdateVoucherTransactionStatus(db, strTransNum, ClaimTransStatus.Reimbursed, dt.Rows(i)("tsmp"))

    '            'The table is changed to no need to delete
    '            'Delete the ReimbursementAuthTran table
    '            'DeleteReimbursementAuthTran(strTransNum)

    '        Next

    '        'insert into BankIn table
    '        InsertBankIn(db, strReimbursementID, strUserID, dt.Rows.Count, intVouchersCount, dblTotalAmt, "bankpaymentdate")

    '        'Insert into BankInHeaderFile table
    '        InsertBankInHeaderFile(db, strReimbursementID, FillRightSpace("F", 1), FillRightSpace("", 12), FillRightSpace("", 3), FillRightSpace("HCVU pmt", 12), GetBankFileValueDate, "********", FillLeftZero(dt.Rows.Count.ToString, 5), FillLeftZero(CInt(dblTotalAmt), 10), FillRightSpace("", 7), FillRightSpace("", 12))

    '        'db.CommitTransaction()
    '        db.RollBackTranscation()
    '    Catch eSQL As SqlException
    '        db.RollBackTranscation()
    '        Throw eSQL
    '    Catch ex As Exception
    '        db.RollBackTranscation()
    '        Throw ex
    '    End Try
    'End Sub

    Private Sub InsertBankInTransaction(ByRef udcDB As Database, ByVal strTranID As String, ByVal strReimburseID As String)

        Dim dt As New DataTable

        Try
            ' create data object and params
            Dim prams() As SqlParameter = {udcDB.MakeInParam("@tran_id", SqlDbType.Char, 20, strTranID), _
                                            udcDB.MakeInParam("@reimburse_id", SqlDbType.Char, 15, strReimburseID)}

            ' run the stored procedure
            udcDB.RunProc("proc_BankInTransaction_add", prams, dt)
        Catch eSQL As SqlException
            'db.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            'db.RollBackTranscation()
            Throw ex
        End Try
    End Sub

    Private Sub InsertBankInDataFile(ByRef udcDB As Database, ByVal strReimburseID As String, ByVal intSeqNum As Integer, ByVal strSecondPartyIdentifier As String, ByVal strSecondPartyBankAccName As String, _
        ByVal strSecondPartyBankNo As String, ByVal strSecondPartyBranchNo As String, ByVal strSecondPartyAccount As String, ByVal strAmount As String, _
        ByVal strSecondPartyIDContinuation As String, ByVal strSecondPartyReference As String)

        Dim dt As New DataTable

        Try
            ' create data object and params
            Dim prams() As SqlParameter = {udcDB.MakeInParam("@reimburse_id", SqlDbType.Char, 15, strReimburseID), _
                                            udcDB.MakeInParam("@seq_num", SqlDbType.Int, 4, intSeqNum), _
                                            udcDB.MakeInParam("@Second_Party_Identifier", SqlDbType.Char, 12, strSecondPartyIdentifier), _
                                            udcDB.MakeInParam("@Second_Party_Bank_Acc_Name", SqlDbType.Char, 20, strSecondPartyBankAccName), _
                                            udcDB.MakeInParam("@Second_Party_Bank_No", SqlDbType.Char, 3, strSecondPartyBankNo), _
                                            udcDB.MakeInParam("@Second_Party_Branch_No", SqlDbType.Char, 3, strSecondPartyBranchNo), _
                                            udcDB.MakeInParam("@Second_Party_Account", SqlDbType.Char, 9, strSecondPartyAccount), _
                                            udcDB.MakeInParam("@Amount", SqlDbType.Char, 10, strAmount), _
                                            udcDB.MakeInParam("@Second_Party_ID_Continuation", SqlDbType.Char, 6, strSecondPartyIDContinuation), _
                                            udcDB.MakeInParam("@Second_Party_Reference", SqlDbType.Char, 12, strSecondPartyReference) _
                                            }

            ' run the stored procedure
            udcDB.RunProc("proc_BankInDataFile_add", prams, dt)
        Catch eSQL As SqlException
            'db.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            'db.RollBackTranscation()
            Throw ex
        End Try
    End Sub

    Private Sub InsertBankInHeaderFile(ByRef udcDB As Database, ByVal strReimburseID As String, ByVal strAutoPlanCode As String, ByVal strFirstPartyAccNo As String _
       , ByVal strPaymentCode As String, ByVal strFirstPartyReference As String, ByVal strValueDate As String _
       , ByVal strFileName As String, ByVal strTotalNoInstruction As String, ByVal strTotalAmtInstruction As String _
       , ByVal strOverflowTotalNoInstruction As String, ByVal strOverflowTotalAmtInstruction As String)

        Dim dt As New DataTable

        Try
            ' create data object and params
            Dim prams() As SqlParameter = {udcDB.MakeInParam("@reimburse_id", SqlDbType.Char, 15, strReimburseID), _
                                            udcDB.MakeInParam("@Auto_Plan_Code", SqlDbType.Char, 1, strAutoPlanCode), _
                                            udcDB.MakeInParam("@First_Party_Acc_No", SqlDbType.Char, 12, strFirstPartyAccNo), _
                                            udcDB.MakeInParam("@Payment_Code", SqlDbType.Char, 3, strPaymentCode), _
                                            udcDB.MakeInParam("@First_Party_Reference", SqlDbType.Char, 12, strFirstPartyReference), _
                                            udcDB.MakeInParam("@Value_Date", SqlDbType.Char, 6, strValueDate), _
                                            udcDB.MakeInParam("@File_Name", SqlDbType.Char, 8, strFileName), _
                                            udcDB.MakeInParam("@Total_No_Instruction", SqlDbType.Char, 5, strTotalNoInstruction), _
                                            udcDB.MakeInParam("@Total_Amt_Instruction", SqlDbType.Char, 10, strTotalAmtInstruction), _
                                            udcDB.MakeInParam("@Overflow_Total_No_Instruction", SqlDbType.Char, 7, strOverflowTotalNoInstruction), _
                                            udcDB.MakeInParam("@Overflow_Total_Amt_Instruction", SqlDbType.Char, 12, strOverflowTotalAmtInstruction) _
                                            }

            ' run the stored procedure
            udcDB.RunProc("proc_BankInHeaderFile_add", prams, dt)
        Catch eSQL As SqlException
            'db.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            'db.RollBackTranscation()
            Throw ex
        End Try
    End Sub

    Private Sub DeleteReimbursementAuthTran(ByVal strTranID As String)

        Dim dt As New DataTable
        Dim db As New Database

        Try
            'db.CommandTimeout = 300
            db.BeginTransaction()
            ' create data object and params
            Dim prams() As SqlParameter = {db.MakeInParam("@tran_id", SqlDbType.Char, 20, strTranID)}

            ' run the stored procedure
            db.RunProc("proc_ReimbursementAuthTran_delete", prams, dt)
            db.CommitTransaction()
        Catch eSQL As SqlException
            db.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            db.RollBackTranscation()
            Throw ex
        End Try
    End Sub

    Private Sub InsertBankIn(ByRef udcDB As Database, ByVal strReimburseID As String, ByVal strUserID As String, ByVal intTransCount As Integer, ByVal intVouchersCount As Integer, ByVal dblTransactionAmt As Double, ByVal dtmBankPaymentDay As DateTime)

        Dim dt As New DataTable
        Dim dtmNow As DateTime
        dtmNow = udtGeneralFunction.GetSystemDateTime

        Try
            ' create data object and params
            Dim prams() As SqlParameter = {udcDB.MakeInParam("@reimbursement_id", SqlDbType.Char, 15, strReimburseID), _
                                           udcDB.MakeInParam("@submission_dtm", SqlDbType.DateTime, 8, dtmNow), _
                                           udcDB.MakeInParam("@submitted_by", SqlDbType.VarChar, 20, strUserID), _
                                           udcDB.MakeInParam("@trans_count", SqlDbType.Int, 4, intTransCount), _
                                           udcDB.MakeInParam("@vouchers_count", SqlDbType.Int, 4, intVouchersCount), _
                                           udcDB.MakeInParam("@total_amt", SqlDbType.Money, 10, dblTransactionAmt), _
                                           udcDB.MakeInParam("@bank_payment_dtm", SqlDbType.DateTime, 8, dtmBankPaymentDay)}

            ' run the stored procedure
            udcDB.RunProc("proc_BankIn_add", prams, dt)
        Catch eSQL As SqlException
            'db.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            'db.RollBackTranscation()
            Throw ex
        End Try
    End Sub

#End Region

#Region "Claim Trans Management"
    Public Function GetTransactionByAny(ByVal strFunctionCode As String, ByVal udtSearchCriteria As Common.SearchCriteria.SearchCriteria, ByVal strUserID As String, ByVal blnOverrideResultLimit As Boolean, ByVal EnumSelectedStoredProc As Aspect) As BaseBLL.BLLSearchResult

        Return udtSearchEngineBLL.SearchVoucherTransactionByAny(strFunctionCode, udtSearchCriteria, strUserID, blnOverrideResultLimit, EnumSelectedStoredProc)

    End Function

    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
    'Public Function GetTransactionManualReimbursedByStatus(ByVal udtSearchCriteria As Common.SearchCriteria.SearchCriteria, ByVal strUserID As String) As DataTable
    Public Function GetTransactionManualReimbursedByStatus(ByVal strFunctionCode As String, ByVal udtSearchCriteria As Common.SearchCriteria.SearchCriteria, ByVal strUserID As String, ByVal blnOverrideResultLimit As Boolean) As BaseBLL.BLLSearchResult
        Try
            'Return udtSearchEngineBLL.SearchVoucherTransactionManualReimbursedByStatus(udtSearchCriteria, strUserID)
            Return udtSearchEngineBLL.SearchVoucherTransactionManualReimbursedByStatus(strFunctionCode, udtSearchCriteria, strUserID, blnOverrideResultLimit)
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
        Catch eSQL As SqlClient.SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'Public Function UpdateClaimCreationApprovalStatus(ByVal strTranID As String, ByVal strStatus As String, ByVal strUserID As String, ByVal tsmp As Byte())

    '    Dim udcDB As New Database
    '    Dim dt As New DataTable

    '    Dim dtmUpdateTime As String = udtGeneralFunction.GetSystemDateTime

    '    udcDB.BeginTransaction()
    '    Try
    '        ' create data object and params
    '        Dim prams() As SqlParameter = {udcDB.MakeInParam("@tran_id", SqlDbType.Char, 20, strTranID), _
    '                                        udcDB.MakeInParam("@record_status", SqlDbType.Char, 1, strStatus), _
    '                                        udcDB.MakeInParam("@update_by", SqlDbType.VarChar, 20, strUserID), _
    '                                        udcDB.MakeInParam("@update_dtm", SqlDbType.DateTime, 8, dtmUpdateTime), _
    '                                        udcDB.MakeInParam("@tsmp", SqlDbType.Timestamp, 8, tsmp)}

    '        ' run the stored procedure
    '        udcDB.RunProc("proc_VoucherTransaction_update_ClaimCreationApproval", prams, dt)
    '        udcDB.CommitTransaction()
    '    Catch eSQL As SqlException
    '        udcDB.RollBackTranscation()
    '        Throw eSQL
    '    Catch ex As Exception
    '        udcDB.RollBackTranscation()
    '        Throw ex
    '    End Try
    'End Function


    Public Function UpdateVoucherTransactionStatus(ByVal dtTransaction As DataTable, ByVal dtSelected As DataTable, ByVal strUpdateType As String, ByVal strUserID As String, ByVal strRemark As String) As Boolean
        Dim udtDB As New Database

        ' Record the update time so that all the transactions will have the same update time
        Dim dtmUpdateTime As String = udtGeneralFunction.GetSystemDateTime

        Dim udtEHSTransactionBLL As New EHSTransaction.EHSTransactionBLL

        Try
            udtDB.BeginTransaction()

            For Each drSelected As DataRow In dtSelected.Rows
                Dim strTransactionNo As String = CStr(drSelected.Item("transNum")).Trim
                Dim strOriginalRecordStatus As String = String.Empty
                Dim strNewRecordStatus As String = String.Empty
                Dim strRemarkToDB As String = String.Empty

                Select Case strUpdateType
                    Case ClaimTransStatus.Active
                        strOriginalRecordStatus = ClaimTransStatus.Suspended

                        Dim udtSearchCriteria As New SearchCriteria
                        udtSearchCriteria.TransNum = strTransactionNo

                        Dim dtSuspend As DataTable = udtSearchEngineBLL.SearchSuspendHistory(udtSearchCriteria)

                        ' Find the original record status
                        Dim arySuspend As DataRow() = dtSuspend.Select("Record_Status = 'S'", "System_Dtm DESC")

                        If arySuspend.Length > 0 Then
                            strNewRecordStatus = CStr(arySuspend(0)("Original_Record_Status")).Trim
                        Else
                            Throw New Exception("Public Function UpdateVoucherTransactionStatus: Original_Record_Status cannot be found")
                        End If

                    Case ClaimTransStatus.Suspended
                        strOriginalRecordStatus = CStr(drSelected("transStatus")).Trim
                        strNewRecordStatus = ClaimTransStatus.Suspended
                        strRemarkToDB = strRemark

                End Select

                udtEHSTransactionBLL.UpdateEHSTransactionStatus(strTransactionNo, strNewRecordStatus, strUserID, dtmUpdateTime, drSelected("tsmp"), udtDB)

                ' CRE19-001 (VSS 2019) [Start][Winnie]
                udtEHSTransactionBLL.InsertVoucherTranSuspendLOG(udtDB, strTransactionNo, dtmUpdateTime, strUserID, strOriginalRecordStatus, strNewRecordStatus, strRemarkToDB)
                ' CRE19-001 (VSS 2019) [End][Winnie]
            Next

            udtDB.CommitTransaction()

        Catch eSQL As SqlException
            udtDB.RollBackTranscation()
            Throw eSQL

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex
        End Try

    End Function

    Public Sub VoidVoucherTransaction(ByVal dtTransaction As DataTable, ByRef dtSelected As DataTable, ByVal strUserID As String, ByVal strRemark As String)
        Dim udtDB As New Database
        Dim dtmVoid As DateTime = udtGeneralFunction.GetSystemDateTime()

        Dim blnErasedAmendHistroy As Boolean = False

        Try
            udtDB.BeginTransaction()

            For Each drSelected As DataRow In dtSelected.Rows
                blnErasedAmendHistroy = False

                Dim drTransaction As DataRow = dtTransaction.Rows(CInt(drSelected("lineNum")) - 1)

                Dim strVoidReferenceNo As String = udtGeneralFunction.generateSystemNum("M")

                'CRE13-006 HCVS Ceiling [Start][Karl]
                Dim udtEHSTransactionBLL As New EHSTransactionBLL()
                Dim udtEHSTransactionModel As EHSTransactionModel = udtEHSTransactionBLL.LoadClaimTran(CStr(drTransaction("transNum")).Trim(), False, False, udtDB)
                Dim udtEHSPersonalInfo As EHSPersonalInformationModel = udtEHSTransactionModel.EHSAcct.EHSPersonalInformationList.Filter(udtEHSTransactionModel.DocCode)

                Dim drTSMPRow As DataRow = udtEHSTransactionBLL.getEHSAccountTSMP(udtDB, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum)
                'CRE13-006 HCVS Ceiling [End][Karl]

                drSelected("voidRef") = udtFormatter.formatSystemNumber(strVoidReferenceNo)
                drSelected("void_dtm") = udtFormatter.formatDateTime(dtmVoid)

                VoidVoucherTransaction(CStr(drTransaction("transNum")).Trim, strVoidReferenceNo, strRemark, strUserID, dtmVoid, drTransaction("tsmp"), udtDB)

                Dim udtSubsidizeWriteOffBLL As New SubsidizeWriteOffBLL()

                'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                udtSubsidizeWriteOffBLL.UpdateWriteOff(udtEHSTransactionModel.ServiceDate, _
                                                       udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, _
                                                       udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, _
                                                       udtEHSPersonalInfo.DOD, udtEHSPersonalInfo.ExactDOD, _
                                                       udtEHSTransactionModel.SchemeCode, udtEHSTransactionModel.TransactionDetails(0).SubsidizeCode, _
                                                       eHASubsidizeWriteOff_CreateReason.TxRemoval, udtDB)
                'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

                'CRE13-006 HCVS Ceiling [Start][Karl]
                If drTSMPRow Is Nothing Then
                    udtEHSTransactionBLL.insertEHSAccountTSMP(udtDB, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, strUserID)
                Else
                    udtEHSTransactionBLL.updateEHSAccountTSMP(udtDB, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, strUserID, CType(drTSMPRow("TSMP"), Byte()))
                End If
                'CRE13-006 HCVS Ceiling [End][Karl]

                ' Remove temporary or special eHealth account
                If drTransaction("Voucher_Acc_ID") = String.Empty AndAlso drTransaction("Invalid_Acc_ID") = String.Empty Then
                    If drTransaction("Special_Acc_ID") = String.Empty Then
                        ' Temporary account
                        Dim udtEHSAccount As EHSAccountModel = udtEHSAccountBLL.LoadTempEHSAccountByVRID(CStr(drTransaction("Temp_Voucher_Acc_ID")).Trim)
                        udtEHSAccount.TSMP = drTransaction("Temp_Voucher_Acc_TSMP")

                        '==================================================================== Code for SmartID ============================================================================
                        ' Get the temp account with account purpose = 'O'
                        Dim udtTempEHSAccount_Original As EHSAccountModel = Nothing
                        If udtEHSAccount.AccountPurpose.Trim = EHSAccountModel.AccountPurposeClass.ForAmendment AndAlso Not udtEHSAccount.OriginalAmendAccID.Trim.Equals(String.Empty) Then
                            udtTempEHSAccount_Original = udtEHSAccountBLL.LoadTempEHSAccountByVRID(udtEHSAccount.OriginalAmendAccID.Trim)
                            blnErasedAmendHistroy = True
                        End If

                        ' Also remove the temp account with account purpose = 'O'
                        If Not IsNothing(udtTempEHSAccount_Original) AndAlso udtTempEHSAccount_Original.AccountPurpose.Trim = EHSAccountModel.AccountPurposeClass.ForAmendmentOld Then
                            udtEHSAccountBLL.UpdateTempEHSAccountReject(udtDB, udtTempEHSAccount_Original, strUserID, dtmVoid)
                        End If

                        If blnErasedAmendHistroy Then
                            ' Update PersonalInfoAmendHistory RecordStats = 'E' (Erased) and SubmitToVerify = 'N' (Doesn't verify)
                            If Not IsNothing(udtEHSAccount.ValidatedAccID) AndAlso Not udtEHSAccount.ValidatedAccID.Equals(String.Empty) Then
                                udtEHSAccountBLL.UpdatePersonalInfoAmendHistoryWithdrawAmendment(udtDB, udtEHSAccount, strUserID)
                            End If
                        End If
                        '==================================================================================================================================================================
                        udtEHSAccountBLL.UpdateTempEHSAccountReject(udtDB, udtEHSAccount, strUserID, dtmVoid)

                    Else
                        ' Special account
                        Dim udtEHSAccount As EHSAccountModel = udtEHSAccountBLL.LoadSpecialEHSAccountByVRID(CStr(drTransaction("Special_Acc_ID")).Trim)
                        udtEHSAccount.TSMP = drTransaction("Special_Acc_TSMP")

                        udtEHSAccountBLL.UpdateSpecialEHSAccountReject(udtDB, udtEHSAccount, strUserID, dtmVoid)

                    End If
                End If

            Next

            udtDB.CommitTransaction()

        Catch eSQL As SqlException
            udtDB.RollBackTranscation()
            Throw eSQL

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex

        End Try

    End Sub

    Public Sub VoidVoucherTransaction(ByVal strTransactionNo As String, ByRef strVoidReferenceNo As String, ByVal strRemark As String, ByVal strUserID As String, ByVal dtmVoid As Date, ByVal bytTSMP As Byte(), Optional ByVal udtDB As Database = Nothing)
        Dim blnDBSupplied As Boolean = False

        If IsNothing(udtDB) Then
            udtDB = New Database
            blnDBSupplied = False
        Else
            blnDBSupplied = True
        End If

        If IsNothing(strVoidReferenceNo) OrElse strVoidReferenceNo.Trim = String.Empty Then
            strVoidReferenceNo = udtGeneralFunction.generateSystemNum("M")
        End If

        If IsNothing(dtmVoid) Then
            dtmVoid = udtGeneralFunction.GetSystemDateTime()
        End If

        Try
            If Not blnDBSupplied Then udtDB.BeginTransaction()

            Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@transaction_id", SqlDbType.Char, 20, strTransactionNo), _
                               udtDB.MakeInParam("@void_transaction_id", SqlDbType.Char, 20, strVoidReferenceNo), _
                               udtDB.MakeInParam("@void_remark", SqlDbType.NVarChar, 255, strRemark), _
                               udtDB.MakeInParam("@void_by", SqlDbType.Char, 20, strUserID), _
                               udtDB.MakeInParam("@void_dtm", SqlDbType.DateTime, 8, dtmVoid), _
                               udtDB.MakeInParam("@tsmp", SqlDbType.Timestamp, 20, bytTSMP)}

            udtDB.RunProc("proc_VoucherTransaction_upd_void", prams)

            If Not blnDBSupplied Then udtDB.CommitTransaction()

        Catch eSQL As SqlException
            If Not blnDBSupplied Then udtDB.RollBackTranscation()
            Throw eSQL

        Catch ex As Exception
            If Not blnDBSupplied Then udtDB.RollBackTranscation()
            Throw ex

        End Try

    End Sub

    Public Function GetPageIndexInRecordNavigation(ByVal intPageSize As Integer, ByVal intRecordNo As Integer) As Integer
        Dim intPageIndex As Integer = 0

        Do While intRecordNo > intPageSize * intPageIndex
            intPageIndex = intPageIndex + 1
        Loop

        Return intPageIndex - 1
    End Function

    '

    Public Sub MarkInvalid(ByVal strTransactionID As String, ByVal bytTransactionTSMP As Byte(), ByVal strUserID As String, ByVal strRemarkType As String, ByVal strRemark As String)
        Dim udtDB As New Database

        Try
            udtDB.BeginTransaction()

            Dim prams() As SqlParameter = {udtDB.MakeInParam("@Transaction_ID", SqlDbType.Char, 20, strTransactionID), _
                                            udtDB.MakeInParam("@TSMP", SqlDbType.Timestamp, 8, bytTransactionTSMP), _
                                            udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUserID), _
                                            udtDB.MakeInParam("@Invalidation_Type", SqlDbType.Char, 2, strRemarkType), _
                                            udtDB.MakeInParam("@Invalidation_Remark", SqlDbType.VarChar, 255, strRemark)}

            udtDB.RunProc("proc_VoucherTransaction_MarkInvalid", prams)

            udtDB.CommitTransaction()

        Catch eSQL As SqlException
            udtDB.RollBackTranscation()
            Throw eSQL

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex

        End Try

    End Sub

    Public Sub CancelInvalidation(ByVal strTransactionID As String, ByVal bytTransactionTSMP As Byte(), ByVal bytTransactionInvalidationTSMP As Byte(), ByVal strUserID As String)
        Dim udtDB As New Database

        Try
            udtDB.BeginTransaction()

            Dim prams() As SqlParameter = {udtDB.MakeInParam("@Transaction_ID", SqlDbType.Char, 20, strTransactionID), _
                                            udtDB.MakeInParam("@VoucherTransaction_TSMP", SqlDbType.Timestamp, 8, bytTransactionTSMP), _
                                            udtDB.MakeInParam("@TransactionInvalidation_TSMP", SqlDbType.Timestamp, 8, bytTransactionInvalidationTSMP), _
                                            udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUserID)}

            udtDB.RunProc("proc_VoucherTransaction_CancelInvalidation", prams)

            udtDB.CommitTransaction()

        Catch eSQL As SqlException
            udtDB.RollBackTranscation()
            Throw eSQL

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex

        End Try

    End Sub

    Public Sub ConfirmInvalid(ByVal strTransactionID As String, ByVal bytTransactionTSMP As Byte(), ByVal bytTransactionInvalidationTSMP As Byte(), ByVal strUserID As String)
        Dim udtDB As New Database

        Try
            udtDB.BeginTransaction()

            ConfirmInvalid(strTransactionID, bytTransactionTSMP, bytTransactionInvalidationTSMP, strUserID, udtDB)

            udtDB.CommitTransaction()

        Catch eSQL As SqlException
            udtDB.RollBackTranscation()
            Throw eSQL

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex

        End Try

    End Sub

    Private Sub ConfirmInvalid(ByVal strTransactionID As String, ByVal bytTransactionTSMP As Byte(), ByVal bytTransactionInvalidationTSMP As Byte(), ByVal strUserID As String, ByVal udtDB As Database)
        Dim dtmDBNow As Date = udtGeneralFunction.GetSystemDateTime

        ' Step 1: Get a new Invalid Account No.
        Dim strInvalidAccID As String = udtGeneralFunction.generateSystemNum("I")


        ' Step 2: Insert [InvalidAccount], [InvalidPersonalInformation] and [VoucherAccountCreationLog]
        Dim udtEHSTransactionBLL As New EHSTransaction.EHSTransactionBLL
        Dim udtEHSAccountBLL As New EHSAccountBLL

        Dim udtEHSTransaction As EHSTransaction.EHSTransactionModel = udtEHSTransactionBLL.LoadClaimTran(strTransactionID)
        Dim udtOldEHSAccount As EHSAccountModel = udtEHSTransaction.EHSAcct

        ' Construct the new invalid account
        Dim udtNewEHSAccount As EHSAccountModel = New EHSAccountModel( _
            SysAccountSource.InvalidAccount, _
            strInvalidAccID, _
            udtOldEHSAccount.SchemeCode, _
            EHSAccountModel.InvalidAccountRecordStatusClass.Active, _
            EHSAccountModel.AccountPurposeClass.ForRemark, _
            dtmDBNow, _
            strUserID, _
            dtmDBNow, _
            strUserID, _
            Nothing, _
            udtOldEHSAccount.VoucherAccID, _
            "00000000", _
            0, _
            Nothing, _
            "N", _
            udtOldEHSAccount.AccountSourceString _
        )

        ' Check System Parameter [InvalidPersonalInformationCopyRealInformation]
        Dim strParm As String = String.Empty
        udtGeneralFunction.getSystemParameter("InvalidPersonalInformationCopyRealInformation", strParm, Nothing)

        If strParm.Trim = "Y" Then
            udtNewEHSAccount = GetRealInvalidPersonalInformation(udtNewEHSAccount, udtOldEHSAccount.EHSPersonalInformationList(0))
        Else
            udtNewEHSAccount = GetFakeInvalidPersonalInformation(udtNewEHSAccount, strUserID, dtmDBNow)
        End If

        udtEHSAccountBLL.InsertInvalidEHSAccount(udtDB, udtNewEHSAccount)

        'CRE13-006 HCVS Ceiling [Start][Karl]
        Dim udtEHSPersonalInfo As EHSPersonalInformationModel = udtOldEHSAccount.EHSPersonalInformationList.Filter(udtEHSTransaction.DocCode)
        Dim udtTransactionBLL As New EHSTransactionBLL

        Dim drTSMPRow As DataRow = udtTransactionBLL.getEHSAccountTSMP(udtDB, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum)
        'CRE13-006 HCVS Ceiling [End][Karl]

        ' Step 3: Update [TransactionInvalidation] and [VoucherTransaction]
        Dim param() As SqlParameter = {udtDB.MakeInParam("@Transaction_ID", SqlDbType.Char, 20, strTransactionID), _
                                            udtDB.MakeInParam("@VoucherTransaction_TSMP", SqlDbType.Timestamp, 8, bytTransactionTSMP), _
                                            udtDB.MakeInParam("@TransactionInvalidation_TSMP", SqlDbType.Timestamp, 8, bytTransactionInvalidationTSMP), _
                                            udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUserID), _
                                            udtDB.MakeInParam("@Invalid_Acc_ID", SqlDbType.Char, 15, strInvalidAccID)}

        udtDB.RunProc("proc_VoucherTransaction_ConfirmInvalid", param)


        ' Step 3b: Update [eHASubsidizeWriteOff]
        Dim udtSubsidizeWriteOffBLL As New SubsidizeWriteOffBLL()

        'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        udtSubsidizeWriteOffBLL.UpdateWriteOff(udtEHSTransaction.ServiceDate, _
                                               udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, _
                                               udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, _
                                               udtEHSPersonalInfo.DOD, udtEHSPersonalInfo.ExactDOD, _
                                               udtEHSTransaction.SchemeCode, udtEHSTransaction.TransactionDetails(0).SubsidizeCode, _
                                               eHASubsidizeWriteOff_CreateReason.TxRemoval, udtDB)
        'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

        'CRE13-006 HCVS Ceiling [Start][Karl]
        If drTSMPRow Is Nothing Then
            udtTransactionBLL.insertEHSAccountTSMP(udtDB, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, strUserID)
        Else
            udtTransactionBLL.updateEHSAccountTSMP(udtDB, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, strUserID, CType(drTSMPRow("TSMP"), Byte()))
        End If
        'CRE13-006 HCVS Ceiling [End][Karl]

        ' INT20-0014 (Fix unable to open invalidated PPP transaction) [Start][Winnie]
        ' ---------------------------------------------------------------------------
        ' Step 4: Remove [TempAccount] & [SpecialAccount] (if any)
        Select Case udtOldEHSAccount.AccountSource
            Case EHSAccount.EHSAccountModel.SysAccountSource.TemporaryAccount
                Dim blnErasedAmendHistroy As Boolean = False

                '==================================================================== Code for SmartID ============================================================================
                ' Get the temp account with account purpose = 'O'
                Dim udtTempEHSAccount_Original As EHSAccountModel = Nothing
                If udtOldEHSAccount.AccountPurpose.Trim = EHSAccountModel.AccountPurposeClass.ForAmendment AndAlso Not udtOldEHSAccount.OriginalAmendAccID.Trim.Equals(String.Empty) Then
                    udtTempEHSAccount_Original = udtEHSAccountBLL.LoadTempEHSAccountByVRID(udtOldEHSAccount.OriginalAmendAccID.Trim)
                    blnErasedAmendHistroy = True
                End If

                ' Also remove the temp account with account purpose = 'O'
                If Not IsNothing(udtTempEHSAccount_Original) AndAlso udtTempEHSAccount_Original.AccountPurpose.Trim = EHSAccount.EHSAccountModel.AccountPurposeClass.ForAmendmentOld Then
                    udtEHSAccountBLL.UpdateTempEHSAccountReject(udtDB, udtTempEHSAccount_Original, udtEHSTransaction.ServiceProviderID, dtmDBNow)
                End If

                If blnErasedAmendHistroy Then
                    ' Update PersonalInfoAmendHistory RecordStats = 'E' (Erased) and SubmitToVerify = 'N' (Doesn't verify)
                    If Not IsNothing(udtOldEHSAccount.ValidatedAccID) AndAlso Not udtOldEHSAccount.ValidatedAccID.Equals(String.Empty) Then
                        udtEHSAccountBLL.UpdatePersonalInfoAmendHistoryWithdrawAmendment(udtDB, udtOldEHSAccount, udtEHSTransaction.ServiceProviderID)
                    End If
                End If
                '==================================================================================================================================================================

                udtEHSAccountBLL.UpdateTempEHSAccountReject(udtDB, udtOldEHSAccount, udtEHSTransaction.ServiceProviderID, dtmDBNow)

            Case EHSAccount.EHSAccountModel.SysAccountSource.SpecialAccount
                udtEHSAccountBLL.UpdateSpecialEHSAccountReject(udtDB, udtOldEHSAccount, strUserID, dtmDBNow)

        End Select
        ' INT20-0014 (Fix unable to open invalidated PPP transaction) [End][Winnie]

    End Sub

    Private Function GetRealInvalidPersonalInformation(ByVal udtNewEHSAccount As EHSAccountModel, ByVal udtOldEHSPersonalInformation As EHSPersonalInformationModel) As EHSAccountModel

        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ' Add [SmartID_Ver]
        udtNewEHSAccount.AddPersonalInformation( _
            udtNewEHSAccount.VoucherAccID, _
            udtOldEHSPersonalInformation.DOB, _
            udtOldEHSPersonalInformation.ExactDOB, _
            udtOldEHSPersonalInformation.Gender, _
            udtOldEHSPersonalInformation.DateofIssue, _
            udtOldEHSPersonalInformation.CreateBySmartID, _
            udtOldEHSPersonalInformation.RecordStatus, _
            udtOldEHSPersonalInformation.CreateDtm, _
            udtOldEHSPersonalInformation.CreateBy, _
            udtOldEHSPersonalInformation.UpdateDtm, _
            udtOldEHSPersonalInformation.UpdateBy, _
            udtOldEHSPersonalInformation.DataEntryBy, _
            udtOldEHSPersonalInformation.IdentityNum, _
            udtOldEHSPersonalInformation.ENameSurName, _
            udtOldEHSPersonalInformation.ENameFirstName, _
            udtOldEHSPersonalInformation.CName, _
            udtOldEHSPersonalInformation.CCCode1, _
            udtOldEHSPersonalInformation.CCCode2, _
            udtOldEHSPersonalInformation.CCCode3, _
            udtOldEHSPersonalInformation.CCCode4, _
            udtOldEHSPersonalInformation.CCCode5, _
            udtOldEHSPersonalInformation.CCCode6, _
            udtOldEHSPersonalInformation.TSMP, _
            udtOldEHSPersonalInformation.ECSerialNo, _
            udtOldEHSPersonalInformation.ECReferenceNo, _
            udtOldEHSPersonalInformation.ECAge, _
            udtOldEHSPersonalInformation.ECDateOfRegistration, _
            udtOldEHSPersonalInformation.DocCode, _
            udtOldEHSPersonalInformation.Foreign_Passport_No, _
            udtOldEHSPersonalInformation.PermitToRemainUntil, _
            udtOldEHSPersonalInformation.AdoptionPrefixNum, _
            udtOldEHSPersonalInformation.OtherInfo, _
            udtOldEHSPersonalInformation.ECSerialNoNotProvided, _
            udtOldEHSPersonalInformation.ECReferenceNoOtherFormat, _
            udtOldEHSPersonalInformation.Deceased, _
            udtOldEHSPersonalInformation.DOD, _
            udtOldEHSPersonalInformation.ExactDOD, _
            udtOldEHSPersonalInformation.SmartIDVer _
        )
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

        Return udtNewEHSAccount

    End Function

    Private Function GetFakeInvalidPersonalInformation(ByVal udtEHSAccount As EHSAccountModel, ByVal strUserID As String, ByVal dtmDBNow As Date) As EHSAccountModel
        Dim strParm As String = String.Empty
        udtGeneralFunction.getSystemParameter("InvalidPersonalInformationFakeInformation", strParm, Nothing)

        ' 00: Doc_Code|DOB|Exact_DOB|Sex|Date_of_Issue|Create_By_SmartID|EC_Serial_No|EC_Reference_No|EC_Age|EC_Date_of_Registration|
        ' 10: Foreign_Passport_No|Permit_To_Remain_Until|Record_Status|Other_Info|DataEntry_By|EF1|EF2|EF3|EF4|EF5|
        ' 20: EF6|EF7|EF8|EF9|EF10|EF11
        Dim aryParm As String() = strParm.Split("|")

        Dim strEName As String = CheckNull(aryParm(16))
        Dim strENameSurName As String = String.Empty
        Dim strENameFirstName As String = String.Empty
        If Not IsNothing(strEName) Then udtFormatter.seperateEName(strEName, strENameSurName, strENameFirstName)

        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ' Add dummy [SmartID_Ver]
        udtEHSAccount.AddPersonalInformation( _
            udtEHSAccount.VoucherAccID, _
            CheckNull(aryParm(1), True, dtmDBNow), _
            CheckNull(aryParm(2)), _
            CheckNull(aryParm(3)), _
            CheckNull(aryParm(4)), _
            CheckNull(aryParm(5)), _
            CheckNull(aryParm(12)), _
            dtmDBNow, _
            strUserID, _
            dtmDBNow, _
            strUserID, _
            CheckNull(aryParm(14)), _
            CheckNull(aryParm(15)), _
            strENameSurName, _
            strENameFirstName, _
            CheckNull(aryParm(17)), _
            CheckNull(aryParm(18)), _
            CheckNull(aryParm(19)), _
            CheckNull(aryParm(20)), _
            CheckNull(aryParm(21)), _
            CheckNull(aryParm(22)), _
            CheckNull(aryParm(23)), _
            Nothing, _
            CheckNull(aryParm(6)), _
            CheckNull(aryParm(7)), _
            CheckNullInteger(aryParm(8)), _
            CheckNull(aryParm(9)), _
            CheckNull(aryParm(0)), _
            CheckNull(aryParm(10)), _
            CheckNull(aryParm(11)), _
            CheckNull(aryParm(25)), _
            CheckNull(aryParm(13)), _
            Nothing, _
            False, _
            String.Empty,
            Nothing,
            String.Empty, _
            String.Empty
        )
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

        Return udtEHSAccount

    End Function

    Private Function CheckNull(ByVal strValue As String, Optional ByVal blnDateField As Boolean = False, Optional ByVal dtmDBNow As Date = Nothing) As Object
        strValue = strValue.Trim
        If strValue = "NULL" OrElse strValue = "GETDATE()" Then
            If blnDateField Then
                Return dtmDBNow
            Else
                Return Nothing
            End If
        End If

        Return strValue
    End Function

    Private Function CheckNullInteger(ByVal strValue As String) As Integer
        Dim intResult As Integer = Nothing
        Integer.TryParse(strValue, intResult)
        Return intResult
    End Function

#End Region

#Region "Detailed Payment Analysis Report"
    'Public Function GetDetailedAnalysisReportByReimburseID(ByVal strReimburseID As String) As DataTable

    '    Dim dt As New DataTable
    '    Dim db As New Database

    '    Try
    '        'db.CommandTimeout = 300
    '        db.BeginTransaction()
    '        ' create data object and params
    '        Dim prams() As SqlParameter = {db.MakeInParam("@reimburse_id", SqlDbType.Char, 15, strReimburseID)}

    '        ' run the stored procedure
    '        db.RunProc("proc_SuperDownload_get_byReimbID", prams, dt)
    '        db.CommitTransaction()
    '        Return dt
    '    Catch eSQL As SqlException
    '        db.RollBackTranscation()
    '        Throw eSQL
    '    Catch ex As Exception
    '        db.RollBackTranscation()
    '        Throw ex
    '    End Try
    'End Function

    Public Function ExtractPracticeDisplaySeqFromSPIDPracticeDisplaySeq(ByVal strSPIDPracticeDisplaySeq As String) As String
        Dim intStartOpenPos, intClosePos As Integer

        intStartOpenPos = strSPIDPracticeDisplaySeq.LastIndexOf("(")
        intClosePos = strSPIDPracticeDisplaySeq.LastIndexOf(")")

        If intStartOpenPos > 0 Then
            Return strSPIDPracticeDisplaySeq.Substring(intStartOpenPos + 1, intClosePos - intStartOpenPos - 1)
        Else
            Return "0"
        End If
    End Function



    ' CRE17-004 Generate a new DPAR on EHCP basis [Start][Dickson]
    Public Function GetListOfEHCPsSelected(ByVal strRimbeID As String, ByVal strCutoffDate As String, ByVal strSchemeCode As String) As DataSet
        Dim udtDB As New Database
        Dim ds As New DataSet

        Dim prams() As SqlParameter = New SqlParameter() { _
            udtDB.MakeInParam("@reimburse_id", SqlDbType.Char, 15, strRimbeID), _
            udtDB.MakeInParam("@cutoff_Date_str", SqlDbType.Char, 11, strCutoffDate), _
            udtDB.MakeInParam("@scheme_code", SqlDbType.Char, 10, strSchemeCode)
        }

        udtDB.RunProc("proc_DPAReport_EHCPs_SelectedList_get", prams, ds)

        Return ds

    End Function
    ' CRE17-004 Generate a new DPAR on EHCP basis [End][Dickson]


#End Region

    ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
    Public Function GetReimbursementProgress(ByVal strReimburseID As String) As ReimbursementDataTable
        Return New ReimbursementDataTable(GetReimbursementAuthorisationByIDStatus(strReimburseID, Nothing, ReimbursementAuthorisationStatus.Active, Nothing))
    End Function
    ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

    ' I-CRE18-002 Enhance batch confirmation in HCSP [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    'Public Function GetReimbursementAuthorisationByIDStatus(ByVal strReimburseID As String, ByVal strAuthorisedStatus As String, ByVal strRecordStatus As String, ByVal strSchemeCode As String) As DataTable
    Public Function GetReimbursementAuthorisationByIDStatus(ByVal strReimburseID As String, ByVal strAuthorisedStatus As String, ByVal strRecordStatus As String, ByVal strSchemeCode As String, Optional ByVal udtDB As Database = Nothing) As DataTable

        Dim dt As New DataTable
        'Dim udtDB As New Database        
        If IsNothing(udtDB) Then udtDB = New Database
        ' I-CRE18-002 Enhance batch confirmation in HCSP [End][Winnie]

        udtDB.CommandTimeout = 300
        udtDB.ConnetionTimeout = 300
        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@reimburse_id", SqlDbType.Char, 15, IIf(IsNothing(strReimburseID), DBNull.Value, strReimburseID)), _
                                            udtDB.MakeInParam("@authorised_status", SqlDbType.Char, 1, IIf(IsNothing(strAuthorisedStatus), DBNull.Value, strAuthorisedStatus)), _
                                            udtDB.MakeInParam("@record_status", SqlDbType.Char, 1, IIf(IsNothing(strRecordStatus), DBNull.Value, strRecordStatus)), _
                                            udtDB.MakeInParam("@scheme_code", SqlDbType.Char, 10, IIf(IsNothing(strSchemeCode), DBNull.Value, strSchemeCode)) _
                                            }

            udtDB.RunProc("proc_ReimbursementAuthorisation_get_byReimbursementIDRecordStatus", prams, dt)
        Catch eSQL As SqlException
            Throw
        End Try

        Return dt

    End Function

    Public Sub UpdateReimbursementAuthorisationByReimbursementID(ByVal strReimID As String, ByVal strRecordStatus As String, ByVal strUpdateBy As String, ByVal TSMP As Byte(), Optional ByVal udtDB As Database = Nothing)
        If IsNothing(udtDB) Then udtDB = New Database

        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@reimburse_id", SqlDbType.Char, 15, strReimID), _
                                            udtDB.MakeInParam("@record_status", SqlDbType.Char, 1, strRecordStatus), _
                                            udtDB.MakeInParam("@update_by", SqlDbType.VarChar, 20, strUpdateBy), _
                                            udtDB.MakeInParam("@tsmp", SqlDbType.Timestamp, 8, TSMP) _
                                            }

            udtDB.RunProc("proc_ReimbursementAuthorisation_upd_byReimbursementID", prams)
        Catch eSQL As SqlException
            Throw eSQL
        End Try
    End Sub

    Private Sub ReimbursementFirstAuthorization(ByRef udcDB As Database, ByVal strUser As String, ByVal strSchemeCode As String)

        Try
            ' create data object and params
            Dim prams() As SqlParameter = {udcDB.MakeInParam("@current_user", SqlDbType.VarChar, 20, strUser), _
                                            udcDB.MakeInParam("@scheme_code", SqlDbType.Char, 10, strSchemeCode) _
                                            }

            ' run the stored procedure
            udcDB.RunProc("proc_ReimbursementFirstAuthorise_add_update", prams)
        Catch eSQL As SqlException
            'udcDB.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            'udcDB.RollBackTranscation()
            Throw ex
        End Try
    End Sub

    Private Function ReimbursementSecondAuthorizationCore(ByVal strUser As String, ByVal strReimburseID As String, ByVal strSchemeCode As String) As Boolean

        Dim udtDB As New Database
        Try
            udtDB.BeginTransaction()
            ' create data object and params
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@current_user", SqlDbType.VarChar, 20, strUser), _
                                            udtDB.MakeInParam("@reimburse_ID", SqlDbType.Char, 15, strReimburseID), _
                                            udtDB.MakeInParam("@scheme_code", SqlDbType.Char, 10, strSchemeCode) _
                                            }

            ' run the stored procedure
            udtDB.RunProc("proc_ReimbursementSecondAuthorise_add_update", prams)
            udtDB.CommitTransaction()
            Return True
        Catch eSQL As SqlException
            udtDB.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex
        End Try
    End Function

    ' I-CRE18-002 Enhance batch confirmation in HCSP [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    'Public Function ReimbursementAuthorizationHold(ByVal strUser As String, ByVal dtmCutoffDtm As DateTime, ByVal strSchemeCode As String, ByVal strReimburseID As String) As Boolean
    Public Sub ReimbursementAuthorizationHold(ByVal strUser As String, ByVal dtmCutoffDtm As DateTime, ByVal strSchemeCode As String, ByVal strReimburseID As String, ByRef udtDB As Database)
        'Dim udtDB As New Database

        ' create data object and params
        Dim prams() As SqlParameter = {udtDB.MakeInParam("@cutoff_dtm", SqlDbType.DateTime, 8, dtmCutoffDtm), _
                                   udtDB.MakeInParam("@current_user", SqlDbType.VarChar, 20, strUser), _
                                   udtDB.MakeInParam("@scheme_code", SqlDbType.Char, 10, strSchemeCode), _
                                   udtDB.MakeInParam("@reimburse_id", SqlDbType.Char, 15, strReimburseID) _
                                   }

        ' run the stored procedure
        udtDB.RunProc("proc_ReimbursementHold_upd", prams)
    End Sub
    ' I-CRE18-002 Enhance batch confirmation in HCSP [End][Winnie]

    Public Function ReimbursementAuthorizationRelease(ByVal strUser As String, ByVal strSchemeCode As String, ByVal strReimburseID As String) As Boolean
        Dim udtDB As New Database

        Try
            udtDB.BeginTransaction()
            ' create data object and params
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@current_user", SqlDbType.VarChar, 20, strUser), _
                                            udtDB.MakeInParam("@scheme_code", SqlDbType.Char, 10, strSchemeCode), _
                                            udtDB.MakeInParam("@reimburse_id", SqlDbType.Char, 15, strReimburseID) _
                                            }

            ' run the stored procedure
            udtDB.RunProc("proc_ReimbursementRelease_upd", prams)
            udtDB.CommitTransaction()
            Return True
        Catch eSQL As SqlException
            udtDB.RollBackTranscation()
            Throw eSQL
            Return False
        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex
            Return False
        End Try
    End Function

    Public Function GetReimbursementDetailByTransactionID(ByVal strTransactionID As String) As DataTable
        Dim udtDB As New Database
        Dim dt As New DataTable

        Try
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@transaction_id", SqlDbType.Char, 20, strTransactionID)}

            udtDB.RunProc("proc_ReimbursementDetail_get_byTransactionID", prams, dt)

        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
        End Try

        Return dt

    End Function

End Class
