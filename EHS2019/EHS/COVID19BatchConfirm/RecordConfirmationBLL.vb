Imports Common.DataAccess
Imports System.Data.SqlClient
Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.EHSClaim.EHSClaimBLL


Namespace BLL

    Public Class RecordConfirmationBLL

        Private udtGF As New GeneralFunction
        Private udtEHSAccountBLL As New EHSAccountBLL


        Public Sub New()

        End Sub

#Region "Search Function"

        Public Function GetTransactionConfirmation(ByVal dtmCutOffDate As DateTime) As DataTable
            Dim db As New Database
            Dim dtTransaction As New DataTable

            Dim parms() As SqlParameter = { _
                db.MakeInParam("@Transaction_Dtm", SqlDbType.DateTime, 8, dtmCutOffDate) _
            }

            db.RunProc("proc_VoucherTransactionConfirm_get_For_COVID19BatchConfirm", parms, dtTransaction)

            Return dtTransaction

        End Function

#End Region

#Region "Transaction"

        Public Function ConfirmTransaction(ByRef dt As DataTable, Optional ByVal udtDB As Database = Nothing) As Boolean
            Dim strTransactionID As String = String.Empty
            Dim strTempVoucherAccID As String
            Dim dtmConfirm As DateTime
            Dim tsmp As Byte()
            Dim VoucherAccTsmp As Byte()
            Dim blnAllSuccess As Boolean = True

            dtmConfirm = udtGF.GetSystemDateTime

            Dim strOriginalAmendAccID As String
            Dim originalTSMP As Byte()
            Dim srValidatedAccID As String

            Dim strDocCode As String
            Dim strSPID As String

            Dim i As Integer

            ' I-CRE18-002 Enhance batch confirmation in HCSP [Start][Winnie]
            Dim blnLocalDB As Boolean

            If udtDB Is Nothing Then
                udtDB = New Database()
                blnLocalDB = True
            Else
                blnLocalDB = False
            End If
            ' I-CRE18-002 Enhance batch confirmation in HCSP [End][Winnie]

            Dim udtEHSClaimBLL As New EHSClaimBLL

            Dim strNeedImmDValidation As String

            Dim udtImmDBLL As Common.Component.ImmD.ImmDBLL = New Common.Component.ImmD.ImmDBLL


            For i = 0 To dt.Rows.Count - 1
                Try
                    ' I-CRE18-002 Enhance batch confirmation in HCSP [Start][Winnie]
                    If blnLocalDB Then
                        udtDB.BeginTransaction()
                    End If
                    ' I-CRE18-002 Enhance batch confirmation in HCSP [End][Winnie]

                    strTransactionID = CStr(dt.Rows(i).Item("Transaction_ID")).Trim
                    strTempVoucherAccID = CStr(dt.Rows(i).Item("Temp_Voucher_Acc_ID")).Trim
                    strDocCode = CStr(dt.Rows(i).Item("Doc_Code")).Trim
                    strSPID = CStr(dt.Rows(i).Item("SP_ID")).Trim

                    BatchConfirmLogger.LogLine(String.Format("Processing Transaction: <Transaction ID: {0}>", strTransactionID))

                    '==================================================================== Code for SmartID ============================================================================
                    ' Get the information of temp account with account purpose = 'O'
                    If IsDBNull(dt.Rows(i).Item("original_amend_acc_id")) Then
                        strOriginalAmendAccID = String.Empty
                        srValidatedAccID = String.Empty
                        originalTSMP = Nothing
                        strNeedImmDValidation = String.Empty
                    Else
                        strOriginalAmendAccID = CStr(dt.Rows(i).Item("original_amend_acc_id")).Trim
                        srValidatedAccID = CStr(dt.Rows(i).Item("Validated_Acc_ID")).Trim
                        originalTSMP = CType(dt.Rows(i).Item("original_TSMP"), Byte())
                        strNeedImmDValidation = CStr(dt.Rows(i).Item("Send_To_ImmD")).Trim.ToUpper
                    End If
                    '==================================================================================================================================================================

                    tsmp = CType(dt.Rows(i).Item("TSMP"), Byte())
                    If Not dt.Rows(i).Item("Voucher_Acc_TSMP") Is DBNull.Value Then
                        VoucherAccTsmp = CType(dt.Rows(i).Item("Voucher_Acc_TSMP"), Byte())
                    Else
                        VoucherAccTsmp = Nothing
                    End If

                    If strTempVoucherAccID = "" Then
                        ' CRE13-001 EHAPP [Start][Karl]
                        ' -----------------------------------------------------------------------------------------
                        'UpdateTransactionStatus(strTransactionID, dtmConfirm, strSPID, EHSTransactionModel.TransRecordStatusClass.Active, tsmp, udtDB)
                        UpdateTransactionConfirmedTransactionStatus(strTransactionID, dtmConfirm, strSPID, tsmp, udtDB)
                        ' CRE13-001 EHAPP [End][Karl]
                        ' -----------------------------------------------------------------------------------------
                    Else
                        UpdateTransactionStatus(strTransactionID, dtmConfirm, strSPID, EHSTransactionModel.TransRecordStatusClass.PendingVRValidate, tsmp, udtDB)
                        udtEHSAccountBLL.UpdateTempEHSAccountConfirmation(udtDB, strTempVoucherAccID, strSPID, dtmConfirm, VoucherAccTsmp)

                        '==================================================================== Code for SmartID ============================================================================
                        ' 1. Update the temp account with account purpose = 'O' as confirmated
                        ' 2. Insert amendment history
                        If Not strOriginalAmendAccID.Equals(String.Empty) AndAlso Not srValidatedAccID.Equals(String.Empty) AndAlso Not IsNothing(originalTSMP) Then
                            udtEHSAccountBLL.UpdateTempEHSAccountConfirmation(udtDB, strOriginalAmendAccID, strSPID, dtmConfirm, originalTSMP)
                            udtEHSAccountBLL.InsertPersonalInfoAmendHistoryByTempAccInConfirmation(udtDB, strTempVoucherAccID, strDocCode, strSPID)

                            If strNeedImmDValidation.Equals("N") Then
                                udtImmDBLL.ValidateAccountEHSModelWithoutImmDValidation(srValidatedAccID, strTempVoucherAccID, "A", strSPID, True, udtDB)
                            End If
                        End If
                        '==================================================================================================================================================================
                    End If

                    ' I-CRE18-002 Enhance batch confirmation in HCSP [Start][Winnie]
                    If blnLocalDB Then
                        udtDB.CommitTransaction()
                    End If

                Catch ex As Exception
                    If blnLocalDB Then
                        udtDB.RollBackTranscation()
                    End If

                    blnAllSuccess = False
                    BatchConfirmLogger.LogLine(String.Format("Failed to confirm record: <Transaction ID: {0}> <Exception:{1}>", _
                                                    strTransactionID, _
                                                    ex.ToString))

                    BatchConfirmLogger.Log(Common.Component.LogID.LOG00005, Nothing, String.Format("Failed to confirm record: <Transaction ID: {0}> <Exception:{1}>", _
                                                    strTransactionID, _
                                                    ex.ToString))
                    ' Continuous process next record

                Finally
                    If blnLocalDB AndAlso Not udtDB Is Nothing Then
                        udtDB.Dispose()
                    End If
                    ' I-CRE18-002 Enhance batch confirmation in HCSP [End][Winnie]                
                End Try

            Next

            Return blnAllSuccess

        End Function

        ' CRE13-001 EHAPP [Start][Karl]
        ' -----------------------------------------------------------------------------------------
        Public Sub UpdateTransactionConfirmedTransactionStatus(ByVal strTransactionID As String, ByVal dtmClaimConfirmationDate As DateTime, ByVal strSPID As String, _
                   ByVal tsmp As Byte(), ByRef db As Database)

            Dim parms() As SqlParameter = { _
                            db.MakeInParam("@Transaction_ID", SqlDbType.Char, 20, strTransactionID), _
                            db.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
                            db.MakeInParam("@Confirmed_Dtm", SqlDbType.DateTime, 8, dtmClaimConfirmationDate), _
                            db.MakeInParam("@tsmp", SqlDbType.Timestamp, 8, tsmp)}
            db.RunProc("proc_VoucherTransactionConfirm_upd_ConfirmedTransactionStatus", parms)

        End Sub
        ' CRE13-001 EHAPP [End][Karl]

        Public Sub UpdateTransactionStatus(ByVal strTransactionID As String, ByVal dtmClaimConfirmationDate As DateTime, ByVal strSPID As String, _
                ByVal strRecordStatus As String, ByVal tsmp As Byte(), ByRef db As Database)

            Dim parms() As SqlParameter = { _
                            db.MakeInParam("@Transaction_ID", SqlDbType.Char, 20, strTransactionID), _
                            db.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
                            db.MakeInParam("@Confirmed_Dtm", SqlDbType.DateTime, 8, dtmClaimConfirmationDate), _
                            db.MakeInParam("@Record_Status", SqlDbType.Char, 8, strRecordStatus), _
                            db.MakeInParam("@tsmp", SqlDbType.Timestamp, 8, tsmp)}
            db.RunProc("proc_VoucherTransactionConfirm_upd", parms)

        End Sub

#End Region

    End Class

End Namespace
