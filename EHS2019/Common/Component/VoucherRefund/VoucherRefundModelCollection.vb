Namespace Component.VoucherRefund
    <Serializable()> Public Class VoucherRefundModelCollection
        Inherits ArrayList

        Sub New()
            MyBase.New()
        End Sub

        Public Overloads Function Add(ByVal udtVoucherRefund As VoucherRefundModel) As Boolean
            MyBase.Add(udtVoucherRefund)
        End Function

        Public Overloads Sub Remove(ByVal udtVoucherRefund As VoucherRefundModel)
            MyBase.Remove(udtVoucherRefund)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As VoucherRefundModel
            Get
                Return CType(MyBase.Item(intIndex), VoucherRefundModel)
            End Get
        End Property

        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Function FilterBySchemeSeq(ByVal intSchemeSeq As Integer) As VoucherRefundModelCollection
            Dim udtResVoucherRefundList As New VoucherRefundModelCollection()
            For Each udtVoucherRefund As VoucherRefundModel In Me

                If udtVoucherRefund.SchemeSeq = intSchemeSeq Then
                    udtResVoucherRefundList.Add(New VoucherRefundModel(udtVoucherRefund))
                End If
            Next
            Return udtResVoucherRefundList
        End Function

        Public Function FilterByRefundDtm(ByVal dtmDate As Date) As VoucherRefundModelCollection
            Dim udtResVoucherRefundList As New VoucherRefundModelCollection()
            For Each udtVoucherRefund As VoucherRefundModel In Me

                If udtVoucherRefund.RefundDtm <= dtmDate Then
                    udtResVoucherRefundList.Add(New VoucherRefundModel(udtVoucherRefund))
                End If
            Next
            Return udtResVoucherRefundList
        End Function
        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]


    End Class

End Namespace
