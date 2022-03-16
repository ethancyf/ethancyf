Imports Common.DataAccess
Imports Common.ComFunction
Imports System.Data.SqlClient


Namespace Component.VoucherRecipientAccount
    Public Class VoucherRecipientAccountBLL
        <Serializable()> Public Class RectifyListResultModel
            Private _dtResult As DataTable
            Private _blnExceedLimit As Boolean = False

            Public Property Result() As DataTable
                Get
                    Return _dtResult
                End Get
                Set(ByVal Value As DataTable)
                    _dtResult = Value
                End Set
            End Property

            Public Property ExceedLimit() As Boolean
                Get
                    Return _blnExceedLimit
                End Get
                Set(ByVal Value As Boolean)
                    _blnExceedLimit = Value
                End Set
            End Property

            Public Sub New()

            End Sub

            Public Sub New(ByVal dtResult As DataTable, ByVal blnExceedLimit As Boolean)

                _dtResult = dtResult
                _blnExceedLimit = blnExceedLimit

            End Sub

        End Class

        Public Function getRectifyVRAcctCnt(ByVal strSchemeCode As String, ByVal strSPID As String, ByVal enumSubPlatform As [Enum]) As Integer
            Dim intRes As Integer = 0
            Dim dt As DataTable = New DataTable
            Dim udtDB As Database = New Database

            Dim strSubPlatform As String = String.Empty
            If Not enumSubPlatform Is Nothing Then
                strSubPlatform = enumSubPlatform.ToString
            End If

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 15, strSPID), _
                udtDB.MakeInParam("@Available_HCSP_SubPlatform", SqlDbType.VarChar, 2, IIf(strSubPlatform.Trim.Equals(String.Empty), DBNull.Value, strSubPlatform))}

            udtDB.RunProc("proc_TempVoucherAccountRectifyCode_get", prams, dt)
            intRes = dt.Rows(0).Item("Record_Count")

            Return intRes

        End Function

        Public Function LoadRectifyTempVRAcct(ByVal strSPID As String, ByVal strDataEntry As String, ByVal strStatus As String, _
                                              ByVal enumSubPlatform As [Enum], ByVal dtmCreateDate As Nullable(Of DateTime)) As RectifyListResultModel

            Dim dt As New DataTable
            Dim udtDB As New Database
            Dim udtRectifyListResult As RectifyListResultModel = Nothing

            Dim strSubPlatform As String = String.Empty

            If Not enumSubPlatform Is Nothing Then
                strSubPlatform = enumSubPlatform.ToString
            End If

            Dim dtmCreateDateToDB As DateTime

            If dtmCreateDate IsNot Nothing Then
                dtmCreateDateToDB = dtmCreateDate
            End If

            Try
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
                udtDB.MakeInParam("@DataEntry_By", SqlDbType.VarChar, 20, IIf(strDataEntry.Trim.Equals(String.Empty), DBNull.Value, strDataEntry)), _
                udtDB.MakeInParam("@Status", SqlDbType.Char, 1, IIf(strStatus.Trim.Equals(String.Empty), DBNull.Value, strStatus)), _
                udtDB.MakeInParam("@Available_HCSP_SubPlatform", SqlDbType.VarChar, 2, IIf(strSubPlatform.Trim.Equals(String.Empty), DBNull.Value, strSubPlatform)), _
                udtDB.MakeInParam("@Create_Date", SqlDbType.DateTime, 8, IIf(dtmCreateDate Is Nothing, DBNull.Value, dtmCreateDateToDB)) _
                }

                udtDB.RunProc("proc_TempVoucherAccount_get_BySchCodeSPInfoStatus", prams, dt)

                udtRectifyListResult = New RectifyListResultModel()
                udtRectifyListResult.Result = dt
                udtRectifyListResult.ExceedLimit = False

            Catch eSQL As SqlClient.SqlException
                If eSQL.Number = 50000 AndAlso eSQL.Message = "00009" Then
                    'Fail
                    dt = Nothing

                    udtRectifyListResult = New RectifyListResultModel()
                    udtRectifyListResult.Result = dt
                    udtRectifyListResult.ExceedLimit = True

                Else
                    Throw eSQL
                End If

            Catch ex As Exception
                Throw

            End Try

            Return udtRectifyListResult

        End Function

        Public Function GetVoucherAccEnquiryFailCount(ByVal strVRAccID As String, Optional ByVal udtDB As Database = Nothing) As Integer
            Dim intRes As Integer = 0

            Try
                If IsNothing(udtDB) Then
                    udtDB = New Database()
                End If

                Dim dt As DataTable = New DataTable
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strVRAccID)}

                udtDB.RunProc("proc_VoucherAccEnquiryFailRecord_get_byVRAccID", prams, dt)

                If dt.Rows.Count > 0 Then
                    intRes = dt.Rows(0).Item("Enquiry_Fail_Count")
                End If
            Catch ex As Exception
                Throw ex
            End Try


            Return intRes

        End Function

        Public Function UpdateVoucherAccEnquiryFailCount(ByVal strVRAccID As String, Optional ByVal udtDB As Database = Nothing) As Boolean
            Dim blnRes As Boolean = False

            If IsNothing(udtDB) Then
                udtDB = New Database()
            End If

            Try
                udtDB.BeginTransaction()
                Dim prams() As SqlParameter = { _
                            udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.VarChar, 15, strVRAccID)}

                udtDB.RunProc("proc_VoucherAccEnquiryFailRecord_add", prams)
                udtDB.CommitTransaction()
                blnRes = True
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw ex
            End Try

            Return blnRes
        End Function

        Public Function UpdatePublicEnquiryStatus(ByVal strVRAccID As String, Optional ByVal udtDB As Database = Nothing) As Boolean
            Dim blnRes As Boolean = False

            If IsNothing(udtDB) Then
                udtDB = New Database()
            End If

            Try
                udtDB.BeginTransaction()
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.VarChar, 15, strVRAccID)}

                udtDB.RunProc("proc_VoucherAccount_upd_PublicEnquiryStatus", prams)
                udtDB.CommitTransaction()
                blnRes = True
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw ex
            End Try

            Return blnRes
        End Function

        Public Function GetPublicEnquiryStatus(ByVal strVRAccID As String, Optional ByVal udtDB As Database = Nothing) As String
            Dim strRes As String = String.Empty
            If IsNothing(udtDB) Then
                udtDB = New Database
            End If

            Try
                Dim dt As DataTable = New DataTable
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.VarChar, 15, strVRAccID)}

                udtDB.RunProc("proc_VoucherAccountEnquiryStatus_get_byVRAccID", prams, dt)
                If dt.Rows.Count > 0 Then
                    strRes = dt.Rows(0).Item("Public_Enquiry_Status")
                End If

                Return strRes
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GetTempVRAcctWithoutTransCnt(ByVal strSPID As String, ByVal enumSubPlatform As [Enum]) As Integer
            Dim intTempAccCnt As Integer
            Dim dtTempAccCnt As New DataTable
            Dim db As New Database

            Dim strSubPlatform As String = String.Empty
            If Not enumSubPlatform Is Nothing Then
                strSubPlatform = enumSubPlatform.ToString
            End If

            Dim parms() As SqlParameter = { _
                db.MakeInParam("@SP_ID", SqlDbType.VarChar, 8, strSPID), _
                db.MakeInParam("@Available_HCSP_SubPlatform", SqlDbType.VarChar, 2, IIf(strSubPlatform.Trim.Equals(String.Empty), DBNull.Value, strSubPlatform))}

            db.RunProc("proc_TempVoucherAccountConfirm_get_cnt", parms, dtTempAccCnt)
            intTempAccCnt = CInt(dtTempAccCnt.Rows(0).Item(0))

            Return intTempAccCnt
        End Function

    End Class

End Namespace
