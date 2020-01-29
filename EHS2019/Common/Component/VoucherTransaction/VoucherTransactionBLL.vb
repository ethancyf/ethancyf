Imports Common.DataAccess
Imports System.Data.SqlClient
Imports Common.ComFunction
Imports Common.Component.VoucherRecipientAccount

Namespace Component.VoucherTransaction
    Public Class VoucherTransactionBLL

        Private _udtGeneralFunction As GeneralFunction

        Public Sub New()
            _udtGeneralFunction = New GeneralFunction
        End Sub

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Public Function GetClaimRecord(ByVal strSPID As String, ByVal intPracticeDisplaySeq As Integer, ByVal strDataEntryBy As String, ByVal dtmCutOffDate As DateTime, ByVal strSchemeCode As String) As DataTable

        '    Dim dtClaimRecord As New DataTable
        '    Dim db As New Database
        '    Dim parms() As SqlParameter = { _
        '        db.MakeInParam("@SP_ID", SqlDbType.VarChar, 8, strSPID), _
        '        db.MakeInParam("@Practice_Display_Seq", SqlDbType.SmallInt, 2, IIf(intPracticeDisplaySeq = -1, DBNull.Value, intPracticeDisplaySeq)), _
        '        db.MakeInParam("@DataEntry_By", SqlDbType.VarChar, 20, IIf(strDataEntryBy = "", DBNull.Value, strDataEntryBy)), _
        '        db.MakeInParam("@Transaction_Dtm", SqlDbType.DateTime, 8, dtmCutOffDate), _
        '        db.MakeInParam("@SchemeCode", SqlDbType.Char, 10, IIf(strSchemeCode = "", DBNull.Value, strSchemeCode))}
        '    db.RunProc("proc_VoucherTransactionConfirm_get", parms, dtClaimRecord)

        '    Return dtClaimRecord
        'End Function
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        ''' <summary>
        ''' Get Claim record count by Vouchertransaction.Record_Status
        ''' </summary>
        ''' <param name="strSPID"></param>
        ''' <param name="strRecordStatus"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetClaimRecordCnt(ByVal strSPID As String, ByVal strDataEntry As String, ByVal strRecordStatus As String, ByRef dtmFrom As DateTime, ByVal enumSubPlatform As [Enum]) As Integer

            Dim intClaimRecordCnt As Integer
            Dim dtClaimRecordCnt As New DataTable
            Dim db As New Database
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'Dim parms() As SqlParameter = { _
            '    db.MakeInParam("@SP_ID", SqlDbType.VarChar, 8, strSPID), _
            '    db.MakeInParam("@DataEntry", SqlDbType.VarChar, 20, IIf(strDataEntry = String.Empty, DBNull.Value, strDataEntry)), _
            '    db.MakeInParam("@Record_Status", SqlDbType.Char, 1, strRecordStatus), _
            '    db.MakeOutParam("@Record_From_Dtm", SqlDbType.DateTime, 8)}
            Dim strSubPlatform As String = String.Empty
            If Not enumSubPlatform Is Nothing Then
                strSubPlatform = enumSubPlatform.ToString
            End If

            Dim parms() As SqlParameter = { _
                db.MakeInParam("@SP_ID", SqlDbType.VarChar, 8, strSPID), _
                db.MakeInParam("@DataEntry", SqlDbType.VarChar, 20, IIf(strDataEntry = String.Empty, DBNull.Value, strDataEntry)), _
                db.MakeInParam("@Record_Status", SqlDbType.Char, 1, strRecordStatus), _
                db.MakeInParam("@Available_HCSP_SubPlatform", SqlDbType.VarChar, 2, IIf(strSubPlatform.Trim.Equals(String.Empty), DBNull.Value, strSubPlatform)), _
                db.MakeOutParam("@Record_From_Dtm", SqlDbType.DateTime, 8)}
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            db.RunProc("proc_VoucherTransactionConfirm_get_cnt", parms, dtClaimRecordCnt)

            If Not parms(4).Value Is DBNull.Value Then
                dtmFrom = parms(4).Value
            Else
                dtmFrom = DateTime.MinValue
            End If
            intClaimRecordCnt = CInt(dtClaimRecordCnt.Rows(0).Item(0))

            Return intClaimRecordCnt

        End Function

        Public Function GetPendingConfirmationClaimRecordCnt(ByVal strSPID As String, ByRef dtmFrom As DateTime, ByVal enumSubPlatform As [Enum]) As Integer
            Return GetClaimRecordCnt(strSPID, String.Empty, Common.Component.ClaimTransStatus.Pending, dtmFrom, enumSubPlatform)
        End Function

        Public Function GetIncompleteClaimRecordCnt(ByVal strSPID As String, ByVal strDataEntry As String, ByRef dtmFrom As DateTime, ByVal enumSubPlatform As [Enum]) As Integer
            Return GetClaimRecordCnt(strSPID, strDataEntry, Common.Component.ClaimTransStatus.Incomplete, dtmFrom, enumSubPlatform)
        End Function

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' Obsolete functions that are no longer used
        ' -----------------------------------------------------------------------------------------  

        'Public Function ClaimConfirmation(ByRef dt As DataTable, ByVal strSPID As String) As DateTime
        '    Dim strTransactionID As String
        '    Dim strTempVoucherAccID As String
        '    Dim dtmClaimConfirmationDate As DateTime
        '    Dim tsmp As Byte()
        '    Dim VoucherAccTsmp As Byte()
        '    dtmClaimConfirmationDate = _udtGeneralFunction.GetSystemDateTime()

        '    Dim i As Integer
        '    Dim db As New Database
        '    Try
        '        db.BeginTransaction()
        '        For i = 0 To dt.Rows.Count - 1
        '            strTransactionID = CStr(dt.Rows(i).Item("Transaction_ID")).Trim
        '            strTempVoucherAccID = CStr(dt.Rows(i).Item("Temp_Voucher_Acc_ID")).Trim
        '            tsmp = CType(dt.Rows(i).Item("TSMP"), Byte())
        '            If Not dt.Rows(i).Item("Voucher_Acc_TSMP") Is DBNull.Value Then
        '                VoucherAccTsmp = CType(dt.Rows(i).Item("Voucher_Acc_TSMP"), Byte())
        '            Else
        '                VoucherAccTsmp = Nothing
        '            End If
        '            If strTempVoucherAccID = "" Then
        '                TransactionConfirmWithVoucherAcc(strTransactionID, dtmClaimConfirmationDate, strSPID, tsmp, db)
        '            Else
        '                TransactionConfirmWithTempVoucherAcc(strTransactionID, dtmClaimConfirmationDate, strSPID, tsmp, strTempVoucherAccID, VoucherAccTsmp, db)
        '            End If
        '        Next
        '        db.CommitTransaction()
        '    Catch ex As Exception
        '        db.RollBackTranscation()
        '    Finally
        '        If Not db Is Nothing Then db.Dispose()
        '    End Try

        '    Return dtmClaimConfirmationDate
        'End Function

        'Public Sub TransactionConfirmWithVoucherAcc(ByVal strTransactionID As String, ByVal dtmClaimConfirmationDate As DateTime, ByVal strSPID As String, ByVal tsmp As Byte(), ByRef db As Database)
        '    UpdateTransactionStatus(strTransactionID, dtmClaimConfirmationDate, strSPID, "A", tsmp, db)
        'End Sub

        'Public Sub TransactionConfirmWithTempVoucherAcc(ByVal strTransactionID As String, ByVal dtmClaimConfirmationDate As DateTime, _
        '            ByVal strSPID As String, ByVal tsmp As Byte(), ByVal strTempVoucherAccID As String, ByVal VoucherAccTsmp As Byte(), ByRef db As Database)
        '    Dim udtVoucherRecipientAccountBLL As New VoucherRecipientAccountBLL
        '    UpdateTransactionStatus(strTransactionID, dtmClaimConfirmationDate, strSPID, "V", tsmp, db)
        '    udtVoucherRecipientAccountBLL.TempVRAcctConfirmation(strTempVoucherAccID, strSPID, dtmClaimConfirmationDate, VoucherAccTsmp, db)
        'End Sub

        'Public Sub UpdateTransactionStatus(ByVal strTransactionID As String, ByVal dtmClaimConfirmationDate As DateTime, ByVal strSPID As String, _
        '            ByVal strRecordStatus As String, ByVal tsmp As Byte(), ByRef db As Database)

        '    Dim parms() As SqlParameter = { _
        '                    db.MakeInParam("@Transaction_ID", SqlDbType.Char, 20, strTransactionID), _
        '                    db.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
        '                    db.MakeInParam("@Confirmed_Dtm", SqlDbType.DateTime, 8, dtmClaimConfirmationDate), _
        '                    db.MakeInParam("@Record_Status", SqlDbType.Char, 8, strRecordStatus), _
        '                    db.MakeInParam("@tsmp", SqlDbType.Timestamp, 8, tsmp)}
        '    db.RunProc("proc_VoucherTransactionConfirm_upd", parms)

        'End Sub

        'Public Sub RejectTransaction(ByRef dr As DataRow, ByVal strRefNo As String, ByRef dtmReject As DateTime, ByVal strRemark As String, ByVal strSPID As String)

        '    Dim strTransactionID As String
        '    Dim tsmp As Byte()
        '    Dim VoucherAccTsmp As Byte()
        '    Dim strTempVoucherAccID As String

        '    strTransactionID = CStr(dr.Item("Transaction_ID")).Trim
        '    tsmp = CType(dr.Item("tsmp"), Byte())
        '    strTempVoucherAccID = CStr(dr.Item("Temp_Voucher_Acc_ID")).Trim
        '    If Not dr.Item("Voucher_Acc_TSMP") Is DBNull.Value Then
        '        VoucherAccTsmp = CType(dr.Item("Voucher_Acc_TSMP"), Byte())
        '    Else
        '        VoucherAccTsmp = Nothing
        '    End If

        '    dtmReject = _udtGeneralFunction.GetSystemDateTime()

        '    Dim db As New Database
        '    Try
        '        db.BeginTransaction()
        '        If strTempVoucherAccID = "" Then
        '            RejectTransactionWithVoucherAcc(strTransactionID, strRefNo, dtmReject, strRemark, strSPID, tsmp, db)
        '        Else
        '            RejectTransactionWithTempVoucherAcc(strTransactionID, strRefNo, dtmReject, strRemark, strSPID, tsmp, strTempVoucherAccID, VoucherAccTsmp, db)
        '        End If
        '        db.CommitTransaction()
        '    Catch ex As Exception
        '        db.RollBackTranscation()
        '        Throw ex
        '    Finally
        '        If Not db Is Nothing Then db.Dispose()
        '    End Try
        'End Sub

        'Public Sub RejectTransactionWithVoucherAcc(ByVal strTransactionID As String, ByVal strRefNo As String, ByVal dtmReject As DateTime, ByVal strRemark As String, ByVal strSPID As String, _
        '            ByVal tsmp As Byte(), ByRef db As Database)
        '    UpdateTransactionVoid(strTransactionID, strRefNo, dtmReject, strRemark, strSPID, tsmp, "S", db)
        'End Sub

        'Public Sub RejectTransactionWithTempVoucherAcc(ByVal strTransactionID As String, ByVal strRefNo As String, ByVal dtmReject As DateTime, ByVal strRemark As String, ByVal strSPID As String, _
        '            ByVal tsmp As Byte(), ByVal strTempVoucherAcctID As String, ByVal VoucherAcctTsmp As Byte(), ByRef db As Database)
        '    Dim udtVoucherRecipientAccountBLL As New VoucherRecipientAccountBLL
        '    UpdateTransactionVoid(strTransactionID, strRefNo, dtmReject, strRemark, strSPID, tsmp, "S", db)
        '    udtVoucherRecipientAccountBLL.TempVRAcctReject(strTempVoucherAcctID, strSPID, dtmReject, VoucherAcctTsmp, db)
        'End Sub

        'Public Sub UpdateTransactionVoid(ByVal strTransactionID As String, ByVal strRefNo As String, ByVal dtmVoid As DateTime, ByVal strRemark As String, ByVal strVoidBy As String, _
        '            ByVal tsmp As Byte(), ByVal strStatus As String, ByRef db As Database)

        '    Dim parms() As SqlParameter = { _
        '                                db.MakeInParam("@Transaction_ID", SqlDbType.Char, 20, strTransactionID), _
        '                                db.MakeInParam("@Void_Transaction_ID", SqlDbType.Char, 20, strRefNo), _
        '                                db.MakeInParam("@Void_Dtm", SqlDbType.DateTime, 8, dtmVoid), _
        '                                db.MakeInParam("@Void_Remark", SqlDbType.Char, 8, strRemark), _
        '                                db.MakeInParam("@Void_By", SqlDbType.Char, 8, strVoidBy), _
        '                                db.MakeInParam("@Status", SqlDbType.Char, 8, strStatus), _
        '                                db.MakeInParam("@tsmp", SqlDbType.Timestamp, 8, tsmp)}
        '    db.RunProc("proc_VoucherTransactionVoid_upd", parms)

        'End Sub
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

    End Class
End Namespace

