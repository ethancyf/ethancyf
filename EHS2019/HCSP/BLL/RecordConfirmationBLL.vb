Imports Common.DataAccess
Imports System.Data.SqlClient
Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction


Namespace BLL

    Public Class RecordConfirmationBLL

        Private udtGF As New GeneralFunction
        Private udtEHSAccountBLL As New EHSAccountBLL


        Public Sub New()

        End Sub

#Region "Search Function"

        Public Function GetEHSAccountConfirmation(ByVal strSPID As String, ByVal intPracticeDisplaySeq As Nullable(Of Integer), ByVal strDataEntryBy As String, ByVal dtmCutOffDate As DateTime, ByVal strSchemeCode As String, ByVal enumSubPlatform As [Enum]) As DataTable
            Dim dtTempAcc As New DataTable
            Dim db As New Database

            Dim objPracticeDisplaySeq As Object = DBNull.Value
            If intPracticeDisplaySeq.HasValue Then
                objPracticeDisplaySeq = intPracticeDisplaySeq.Value
            End If

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim strSubPlatform As String = String.Empty
            If Not enumSubPlatform Is Nothing Then
                strSubPlatform = enumSubPlatform.ToString
            End If

            'Dim parms() As SqlParameter = { _
            '    db.MakeInParam("@SP_ID", SqlDbType.VarChar, 8, strSPID), _
            '    db.MakeInParam("@Practice_Display_Seq", SqlDbType.SmallInt, 2, objPracticeDisplaySeq), _
            '    db.MakeInParam("@DataEntry_By", SqlDbType.VarChar, 20, IIf(strDataEntryBy.Trim.Equals(String.Empty), DBNull.Value, strDataEntryBy)), _
            '    db.MakeInParam("@Create_Dtm", SqlDbType.DateTime, 8, dtmCutOffDate), _
            '    db.MakeInParam("@SchemeCode", SqlDbType.Char, 10, IIf(strSchemeCode.Trim.Equals(String.Empty), DBNull.Value, strSchemeCode))}
            Dim parms() As SqlParameter = { _
                db.MakeInParam("@SP_ID", SqlDbType.VarChar, 8, strSPID), _
                db.MakeInParam("@Practice_Display_Seq", SqlDbType.SmallInt, 2, objPracticeDisplaySeq), _
                db.MakeInParam("@DataEntry_By", SqlDbType.VarChar, 20, IIf(strDataEntryBy.Trim.Equals(String.Empty), DBNull.Value, strDataEntryBy)), _
                db.MakeInParam("@Create_Dtm", SqlDbType.DateTime, 8, dtmCutOffDate), _
                db.MakeInParam("@SchemeCode", SqlDbType.Char, 10, IIf(strSchemeCode.Trim.Equals(String.Empty), DBNull.Value, strSchemeCode)), _
                db.MakeInParam("@Available_HCSP_SubPlatform", SqlDbType.VarChar, 2, IIf(strSubPlatform.Trim.Equals(String.Empty), DBNull.Value, strSubPlatform))}
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            db.RunProc("proc_TempVoucherAccountConfirm_get_bySPID", parms, dtTempAcc)
            Return dtTempAcc
        End Function

        Public Function GetTransactionConfirmation(ByVal strSPID As String, ByVal intPracticeDisplaySeq As Nullable(Of Integer), ByVal strDataEntryBy As String, ByVal dtmCutOffDate As DateTime, ByVal strSchemeCode As String, ByVal enumSubPlatform As [Enum], Optional ByVal strIncludeIncompleteClaim As String = "N") As DataTable
            Dim db As New Database
            Dim objPracticeDisplaySeq As Object = DBNull.Value
            If intPracticeDisplaySeq.HasValue Then
                objPracticeDisplaySeq = intPracticeDisplaySeq.Value
            End If

            Dim strSubPlatform As String = String.Empty
            If Not enumSubPlatform Is Nothing Then
                strSubPlatform = enumSubPlatform.ToString
            End If

            Dim parms() As SqlParameter = { _
                db.MakeInParam("@SP_ID", SqlDbType.VarChar, 8, strSPID), _
                db.MakeInParam("@Practice_Display_Seq", SqlDbType.SmallInt, 2, objPracticeDisplaySeq), _
                db.MakeInParam("@DataEntry_By", SqlDbType.VarChar, 20, IIf(strDataEntryBy.Trim.Equals(String.Empty), DBNull.Value, strDataEntryBy)), _
                db.MakeInParam("@Transaction_Dtm", SqlDbType.DateTime, 8, dtmCutOffDate), _
                db.MakeInParam("@SchemeCode", SqlDbType.Char, 10, IIf(strSchemeCode.Trim.Equals(String.Empty), DBNull.Value, strSchemeCode)), _
                db.MakeInParam("@Transaction_ID", SqlDbType.Char, 20, DBNull.Value), _
                db.MakeInParam("@IncludeIncompleteClaim", SqlDbType.Char, 1, strIncludeIncompleteClaim), _
                db.MakeInParam("@Available_HCSP_SubPlatform", SqlDbType.VarChar, 2, IIf(strSubPlatform.Trim.Equals(String.Empty), DBNull.Value, strSubPlatform))}

            Dim dtClaimRecord As DataTable = CallGetTransactionConfirmation(parms, db)

            Return dtClaimRecord

        End Function

        Public Function GetTransactionConfirmation(ByVal strSPID As String, ByVal strTransactionID As String, ByVal enumSubPlatform As [Enum], Optional ByVal udtDB As Database = Nothing) As DataTable
            If udtDB Is Nothing Then udtDB = New Database

            Dim strSubPlatform As String = String.Empty
            If Not enumSubPlatform Is Nothing Then
                strSubPlatform = enumSubPlatform.ToString
            End If

            Dim parms() As SqlParameter = { _
                udtDB.MakeInParam("@SP_ID", SqlDbType.VarChar, 8, strSPID), _
                udtDB.MakeInParam("@Practice_Display_Seq", SqlDbType.SmallInt, 2, DBNull.Value), _
                udtDB.MakeInParam("@DataEntry_By", SqlDbType.VarChar, 20, DBNull.Value), _
                udtDB.MakeInParam("@Transaction_Dtm", SqlDbType.DateTime, 8, Now()), _
                udtDB.MakeInParam("@SchemeCode", SqlDbType.Char, 10, DBNull.Value), _
                udtDB.MakeInParam("@Transaction_ID", SqlDbType.Char, 20, IIf(strTransactionID.Trim.Equals(String.Empty), DBNull.Value, strTransactionID)), _
                udtDB.MakeInParam("@Available_HCSP_SubPlatform", SqlDbType.VarChar, 2, IIf(strSubPlatform.Trim.Equals(String.Empty), DBNull.Value, strSubPlatform))}

            Dim dtClaimRecord As DataTable = CallGetTransactionConfirmation(parms, udtDB)

            Return dtClaimRecord

        End Function

        Private Function CallGetTransactionConfirmation(ByVal parms() As SqlParameter, ByVal udtDB As Database) As DataTable
            Dim ds As New DataSet

            udtDB.RunProc("proc_VoucherTransactionConfirm_get", parms, ds)

            ' Massage the data
            Dim dtData As DataTable = ds.Tables(0)
            Dim dtOtherInfo As DataTable = ds.Tables(1)

            dtData.Columns.Add("Details", GetType(String))
            dtData.Columns.Add("Details_Chi", GetType(String))
            dtData.Columns.Add("Details_CN", GetType(String))

            Dim lstOtherInfoEN As List(Of String) = Nothing
            Dim lstOtherInfoTC As List(Of String) = Nothing
            Dim lstOtherInfoSC As List(Of String) = Nothing

            For Each drData As DataRow In dtData.Rows
                lstOtherInfoEN = New List(Of String)
                lstOtherInfoTC = New List(Of String)
                lstOtherInfoSC = New List(Of String)

                For Each drOtherInfo As DataRow In dtOtherInfo.Select(String.Format("Transaction_ID = '{0}'", drData("Transaction_ID").ToString.Trim), _
                                                                      "Item_Group_Seq, Display_Seq")
                    lstOtherInfoEN.Add(drOtherInfo("Content_EN").ToString.Trim)
                    lstOtherInfoTC.Add(drOtherInfo("Content_TC").ToString.Trim)
                    lstOtherInfoSC.Add(drOtherInfo("Content_SC").ToString.Trim)

                Next

                ' INT16-0032 Long term fix for HCSP Record Confirmation [Start][Winnie]
                If lstOtherInfoEN.Count > 0 Then
                    drData("Details") = String.Join(vbCrLf, lstOtherInfoEN.ToArray)
                Else
                    drData("Details") = String.Empty
                End If

                If lstOtherInfoTC.Count > 0 Then
                    drData("Details_Chi") = String.Join(vbCrLf, lstOtherInfoTC.ToArray)
                Else
                    drData("Details_Chi") = String.Empty
                End If

                If lstOtherInfoSC.Count > 0 Then
                    drData("Details_CN") = String.Join(vbCrLf, lstOtherInfoSC.ToArray)
                Else
                    drData("Details_CN") = String.Empty
                End If
                ' INT16-0032 Long term fix for HCSP Record Confirmation [End][Winnie]

            Next

            Return dtData

        End Function

#End Region

#Region "Transaction"

        Public Function ConfirmTransaction(ByVal strSPID As String, ByVal strTransactionID As String, ByVal enumSubPlatform As [Enum], Optional ByVal udtDB As Database = Nothing) As DateTime
            Dim dt As DataTable = GetTransactionConfirmation(strSPID, strTransactionID, enumSubPlatform, udtDB)
            Return ConfirmTransaction(dt, strSPID, udtDB)
        End Function

        Public Function ConfirmTransaction(ByRef dt As DataTable, ByVal strSPID As String, Optional ByVal udtDB As Database = Nothing) As DateTime
            Dim strTransactionID As String
            Dim strTempVoucherAccID As String
            Dim dtmConfirm As DateTime
            Dim tsmp As Byte()
            Dim VoucherAccTsmp As Byte()
            dtmConfirm = udtGF.GetSystemDateTime

            Dim strOriginalAmendAccID As String
            Dim originalTSMP As Byte()
            Dim srValidatedAccID As String

            Dim strDocCode As String

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

                    Throw

                Finally
                    If blnLocalDB AndAlso Not udtDB Is Nothing Then
                        udtDB.Dispose()
                    End If
                    ' I-CRE18-002 Enhance batch confirmation in HCSP [End][Winnie]                
                End Try

            Next

            Return dtmConfirm

        End Function

        Public Function ConfirmTempEHSAccount(ByRef dt As DataTable, ByVal strSPID As String) As DateTime
            Dim strTempVoucherAccID As String
            Dim tsmp As Byte()

            Dim dtmConfirm As DateTime
            dtmConfirm = udtGF.GetSystemDateTime()

            Dim udtDB As New Database

            Dim strOriginalAmendAccID As String
            Dim originalTSMP As Byte()
            Dim srValidatedAccID As String
            Dim strDocCode As String
            Dim strNeedImmDValidation As String

            Dim udtImmDBLL As Common.Component.ImmD.ImmDBLL = New Common.Component.ImmD.ImmDBLL

            For Each dr As DataRow In dt.Rows
                Try
                    udtDB.BeginTransaction()

                    strTempVoucherAccID = dr.Item("Voucher_Acc_ID")
                    tsmp = dr.Item("tsmp")
                    strDocCode = CStr(dr.Item("Doc_Code")).Trim

                    '==================================================================== Code for SmartID ============================================================================
                    ' Get the information of temp account with account purpose = 'O'
                    If IsDBNull(dr.Item("original_amend_acc_id")) Then
                        strOriginalAmendAccID = String.Empty
                        srValidatedAccID = String.Empty
                        originalTSMP = Nothing
                        strNeedImmDValidation = String.Empty
                    Else
                        strOriginalAmendAccID = CStr(dr.Item("original_amend_acc_id")).Trim
                        srValidatedAccID = CStr(dr.Item("Validated_Acc_ID")).Trim
                        originalTSMP = CType(dr.Item("original_TSMP"), Byte())
                        strNeedImmDValidation = CStr(dr.Item("Send_To_ImmD")).Trim.ToUpper
                    End If
                    '==================================================================================================================================================================

                    udtEHSAccountBLL.UpdateTempEHSAccountConfirmation(udtDB, strTempVoucherAccID, strSPID, dtmConfirm, tsmp)

                    '==================================================================== Code for SmartID ============================================================================
                    ' 1. Update the temp account with account purpose = 'O' as confirmated
                    ' 2. Insert amendment history
                    If Not strOriginalAmendAccID.Equals(String.Empty) AndAlso Not srValidatedAccID.Equals(String.Empty) AndAlso Not IsNothing(originalTSMP) Then
                        udtEHSAccountBLL.UpdateTempEHSAccountConfirmation(udtDB, strOriginalAmendAccID, strSPID, dtmConfirm, originalTSMP)
                        udtEHSAccountBLL.InsertPersonalInfoAmendHistoryByTempAccInConfirmation(udtDB, strTempVoucherAccID, strDocCode, strSPID)

                        If strNeedImmDValidation.Equals("N") Then
                            udtImmDBLL.ValidateAccountEHSModelWithoutImmDValidation(srValidatedAccID, strTempVoucherAccID, "A", strSPID, False, udtDB)
                        End If

                    End If
                    '==================================================================================================================================================================


                    udtDB.CommitTransaction()

                    ' I-CRE18-002 Enhance batch confirmation in HCSP [Start][Winnie]
                Catch ex As Exception
                    udtDB.RollBackTranscation()

                    Throw
                    ' I-CRE18-002 Enhance batch confirmation in HCSP [End][Winnie]                
                Finally
                     If Not udtDB Is Nothing Then udtDB.Dispose()

                End Try
            Next

            Return dtmConfirm
        End Function

        Public Function RejectTempEHSAccount(ByRef dt As DataTable, ByVal strspID As String) As DateTime
            Dim strTempVoucherAccID As String
            Dim tsmp As Byte()

            Dim dtmReject As DateTime
            dtmReject = udtGF.GetSystemDateTime()

            Dim udtImmDBLL As New Common.Component.ImmD.ImmDBLL

            Dim udtDB As New Database

            Dim strOriginalAmendAccID As String
            Dim originalTSMP As Byte()
            Dim srValidatedAccID As String

            For Each dr As DataRow In dt.Rows
                Try
                    udtDB.BeginTransaction()

                    strTempVoucherAccID = dr.Item("Voucher_Acc_ID")
                    tsmp = dr.Item("tsmp")

                    '==================================================================== Code for SmartID ============================================================================
                    ' Get the information of temp account with account purpose = 'O'
                    If IsDBNull(dr.Item("original_amend_acc_id")) Then
                        strOriginalAmendAccID = String.Empty
                        srValidatedAccID = String.Empty
                        originalTSMP = Nothing
                    Else
                        strOriginalAmendAccID = CStr(dr.Item("original_amend_acc_id")).Trim
                        srValidatedAccID = CStr(dr.Item("Validated_Acc_ID")).Trim
                        originalTSMP = CType(dr.Item("original_TSMP"), Byte())
                    End If
                    '==================================================================================================================================================================

                    udtEHSAccountBLL.RejectTempEHSAccountConfirmation(udtDB, strTempVoucherAccID, strspID, dtmReject, tsmp)
                    'udtImmDBLL.DeleteTempVRAcctInPendingTable(udtDB, strTempVoucherAccID)

                    '==================================================================== Code for SmartID ============================================================================
                    ' 1. Update the temp account with account purpose = 'O' as confirmated
                    If Not strOriginalAmendAccID.Equals(String.Empty) AndAlso Not srValidatedAccID.Equals(String.Empty) AndAlso Not IsNothing(originalTSMP) Then
                        udtEHSAccountBLL.RejectTempEHSAccountConfirmation(udtDB, strOriginalAmendAccID, strspID, dtmReject, originalTSMP)
                    End If
                    '==================================================================================================================================================================

                    udtDB.CommitTransaction()

                    ' I-CRE18-002 Enhance batch confirmation in HCSP [Start][Winnie]
                Catch ex As Exception
                    udtDB.RollBackTranscation()

                    Throw
                    ' I-CRE18-002 Enhance batch confirmation in HCSP [End][Winnie]
                Finally
                     If Not udtDB Is Nothing Then udtDB.Dispose()

                End Try
            Next

            Return dtmReject

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
