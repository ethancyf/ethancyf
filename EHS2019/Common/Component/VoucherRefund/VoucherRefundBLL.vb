' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]

Imports System.Data
Imports System.Data.SqlClient
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.DataAccess
Imports Common.Component
Imports Common.Component.StaticData
Imports Common.ComFunction.ParameterFunction
Imports Common.Format
Imports Common.Component.EHSAccount
Imports Common.Component.VoucherInfo

Namespace Component.VoucherRefund

    Public Class VoucherRefundBLL

#Region "Private Member"
        Private udtDB As New Database()

        Public Const DOB_DataType As SqlDbType = SqlDbType.DateTime
        Public Const DOB_DataSize As Integer = 8

        Public Const Exact_DOB_DataType As SqlDbType = SqlDbType.Char
        Public Const Exact_DOB_DataSize As Integer = 1
#End Region

#Region "Constructor"
        Public Sub New()
        End Sub
#End Region

#Region "Method for Voucher Refund"

        'Get Voucher Refund for particular eHA
        Public Function GetVoucherRefundByDocID(ByVal strIdentityNum As String, Optional ByVal udtDB As Database = Nothing) As VoucherRefundModelCollection

            Dim udtVoucherRefundList As VoucherRefundModelCollection = New VoucherRefundModelCollection()
            Dim udtVoucherRefundModal As VoucherRefundModel = Nothing

            Dim dt As New DataTable

            If udtDB Is Nothing Then udtDB = New Database()

            Try
                Dim params() As SqlParameter = { _
                                     udtDB.MakeInParam("@identity", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, strIdentityNum)}
                udtDB.RunProc("proc_VoucherRefund_Get_byDocID", params, dt)

                For Each dr As DataRow In dt.Rows

                    udtVoucherRefundModal = New VoucherRefundModel(CType(dr.Item("Refund_ID"), String), _
                                                                 CType(dr.Item("Voucher_Acc_ID"), String), _
                                                                 CType(dr.Item("Doc_Code"), String), _
                                                                 CType(dr.Item("Doc_No"), String), _
                                                                 CType(dr.Item("Refund_Amt"), Integer), _
                                                                 CType(IIf(dr.Item("Refund_Dtm") Is DBNull.Value, Nothing, dr.Item("Refund_Dtm")), DateTime), _
                                                                 CType(IIf(dr.Item("Scheme_Seq") Is DBNull.Value, Nothing, dr.Item("Scheme_Seq")), Integer), _
                                                                 CType(dr.Item("Record_Status"), String), _
                                                                 CType(dr.Item("Create_By"), String), _
                                                                 CType(dr.Item("Create_Dtm"), DateTime), _
                                                                 CType(dr.Item("Update_By"), String), _
                                                                 CType(dr.Item("Update_Dtm"), DateTime), _
                                                                 CType(dr.Item("TSMP"), Byte()))

                    udtVoucherRefundList.Add(udtVoucherRefundModal)

                Next

                Return udtVoucherRefundList

            Catch ex As Exception
                Throw
            End Try

        End Function

        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieve the Total amount of voucher refunded
        ''' dtmServiceDate refer to the claim service date or current date
        ''' </summary>
        ''' <param name="strIdentityNum"></param>
        ''' <param name="dtmServiceDate"></param>
        ''' <param name="strSpecificSchemeSeqOnly"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getTotalRefundedVoucher(ByVal strIdentityNum As String, _
                                                ByVal dtmServiceDate As Date, _
                                                Optional ByVal strSpecificSchemeSeqOnly As String = Nothing, _
                                                Optional ByVal udtDB As Database = Nothing) As VoucherDetailModelCollection

            Dim udtVoucherDetailList As New VoucherDetailModelCollection
            Dim dicRefund As New Dictionary(Of Integer, Integer)

            If udtDB Is Nothing Then udtDB = New Database()

            Dim intRefundedVoucher As Integer = 0

            Dim udtVoucherRefundList As VoucherRefundModelCollection = Me.GetVoucherRefundByDocID(strIdentityNum, udtDB)

            For Each udtVoucherRefund As VoucherRefundModel In udtVoucherRefundList

                If String.IsNullOrEmpty(strSpecificSchemeSeqOnly) = True Or _
                    (Not udtVoucherRefund.SchemeSeq Is Nothing AndAlso udtVoucherRefund.SchemeSeq = CInt(strSpecificSchemeSeqOnly)) Then

                    If udtVoucherRefund.RefundDtm <= dtmServiceDate Then
                        If Not dicRefund.ContainsKey(udtVoucherRefund.SchemeSeq) Then
                            dicRefund.Add(udtVoucherRefund.SchemeSeq, udtVoucherRefund.RefundAmt)

                        Else
                            Dim intRefund As Integer = dicRefund.Item(udtVoucherRefund.SchemeSeq)

                            intRefund = intRefund + udtVoucherRefund.RefundAmt

                            dicRefund.Remove(udtVoucherRefund.SchemeSeq)

                            dicRefund.Add(udtVoucherRefund.SchemeSeq, intRefund)

                        End If
                    End If
                End If

            Next

            For Each intkey As Integer In dicRefund.Keys
                Dim udtVoucherDetail As New VoucherDetailModel

                udtVoucherDetail.SchemeSeq = intkey
                udtVoucherDetail.Refund = dicRefund.Item(intkey)

                udtVoucherDetail.VoucherRefundList = udtVoucherRefundList.FilterBySchemeSeq(intkey)

                udtVoucherDetailList.Add(udtVoucherDetail)
            Next

            Return udtVoucherDetailList

        End Function
        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

#End Region

    End Class

End Namespace
