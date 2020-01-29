Namespace Component.VoucherInfo

    <Serializable()> Public Class VoucherDetailModel

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ' Handle new scheme [HCVSDHC]
        ' CRE19-006 (DHC) [End][Winnie]

#Region "Private Member"
        Private _intSchemeSeq As Nullable(Of Integer) = Nothing
        Private _dtmPeriodStart As Date = Nothing
        Private _dtmPeriodEnd As Date = Nothing
        Private _intEntitlement As Integer = 0
        Private _intCeiling As Nullable(Of Integer) = Nothing
        Private _intUsedByHCVS As Integer = 0
        Private _intUsedByHCVSCHN As Integer = 0
        Private _intUsedByHCVSDHC As Integer = 0
        Private _intRefund As Integer = 0
        Private _intWriteOff As Integer = 0
        Private _udtTransactionDetailModelCollection As EHSTransaction.TransactionDetailModelCollection = Nothing
        Private _udtVoucherRefundList As VoucherRefund.VoucherRefundModelCollection = Nothing
#End Region

#Region "Property"

        Public Property SchemeSeq() As Integer
            Get
                Return Me._intSchemeSeq
            End Get
            Set(ByVal value As Integer)
                Me._intSchemeSeq = value
            End Set
        End Property

        Public Property PeriodStart() As Date
            Get
                Return Me._dtmPeriodStart
            End Get
            Set(ByVal value As Date)
                Me._dtmPeriodStart = value
            End Set
        End Property

        Public Property PeriodEnd() As Date
            Get
                Return Me._dtmPeriodEnd
            End Get
            Set(ByVal value As Date)
                Me._dtmPeriodEnd = value
            End Set
        End Property

        Public Property Entitlement() As Integer
            Get
                Return Me._intEntitlement
            End Get
            Set(ByVal value As Integer)
                Me._intEntitlement = value
            End Set
        End Property

        Public Property Ceiling() As Nullable(Of Integer)
            Get
                Return Me._intCeiling
            End Get
            Set(ByVal value As Nullable(Of Integer))
                Me._intCeiling = value
            End Set
        End Property

        Public ReadOnly Property Used() As Integer
            Get
                Return Me._intUsedByHCVS + _intUsedByHCVSCHN + _intUsedByHCVSDHC
            End Get
        End Property

        Public ReadOnly Property Used(ByVal strSchemeCode As String) As Integer
            Get
                If strSchemeCode = Scheme.SchemeClaimModel.HCVS Then
                    Return Me._intUsedByHCVS
                End If

                If strSchemeCode = Scheme.SchemeClaimModel.HCVSCHN Then
                    Return Me._intUsedByHCVSCHN
                End If

                If strSchemeCode = Scheme.SchemeClaimModel.HCVSDHC Then
                    Return Me._intUsedByHCVSDHC
                End If

                Throw New Exception(String.Format("Invalid scheme code ({0}).", strSchemeCode))
            End Get
        End Property

        Public WriteOnly Property UsedByHCVS() As Integer
            Set(ByVal value As Integer)
                Me._intUsedByHCVS = value
            End Set
        End Property

        Public WriteOnly Property UsedByHCVSCHN() As Integer
            Set(ByVal value As Integer)
                Me._intUsedByHCVSCHN = value
            End Set
        End Property

        Public WriteOnly Property UsedByHCVSDHC() As Integer
            Set(ByVal value As Integer)
                Me._intUsedByHCVSDHC = value
            End Set
        End Property

        Public Property Refund() As Integer
            Get
                Return Me._intRefund
            End Get
            Set(ByVal value As Integer)
                Me._intRefund = value
            End Set
        End Property

        Public Property WriteOff() As Integer
            Get
                Return Me._intWriteOff
            End Get
            Set(ByVal value As Integer)
                Me._intWriteOff = value
            End Set
        End Property

        Public Property TransactionDetails() As EHSTransaction.TransactionDetailModelCollection
            Get
                Return Me._udtTransactionDetailModelCollection
            End Get
            Set(ByVal value As EHSTransaction.TransactionDetailModelCollection)
                Me._udtTransactionDetailModelCollection = value
            End Set
        End Property

        Public Property VoucherRefundList() As VoucherRefund.VoucherRefundModelCollection
            Get
                Return Me._udtVoucherRefundList
            End Get
            Set(ByVal value As VoucherRefund.VoucherRefundModelCollection)
                Me._udtVoucherRefundList = value
            End Set
        End Property

#End Region

#Region "Constructor"

        Public Sub New()
            _udtTransactionDetailModelCollection = New EHSTransaction.TransactionDetailModelCollection
            _udtVoucherRefundList = New VoucherRefund.VoucherRefundModelCollection
        End Sub

        Public Sub New(ByVal udtVoucherDetailModel As VoucherDetailModel)
            _intSchemeSeq = udtVoucherDetailModel.SchemeSeq
            _dtmPeriodStart = udtVoucherDetailModel.PeriodStart
            _dtmPeriodEnd = udtVoucherDetailModel.PeriodEnd
            _intEntitlement = udtVoucherDetailModel.Entitlement
            _intCeiling = udtVoucherDetailModel.Ceiling
            _intUsedByHCVS = udtVoucherDetailModel.Used(Scheme.SchemeClaimModel.HCVS)
            _intUsedByHCVSCHN = udtVoucherDetailModel.Used(Scheme.SchemeClaimModel.HCVSCHN)
            _intUsedByHCVSDHC = udtVoucherDetailModel.Used(Scheme.SchemeClaimModel.HCVSDHC)
            _intRefund = udtVoucherDetailModel.Refund
            _intWriteOff = udtVoucherDetailModel.WriteOff

            _udtTransactionDetailModelCollection = New EHSTransaction.TransactionDetailModelCollection
            For Each udtTransactionDetail As EHSTransaction.TransactionDetailModel In udtVoucherDetailModel.TransactionDetails
                _udtTransactionDetailModelCollection.Add(New EHSTransaction.TransactionDetailModel(udtTransactionDetail))
            Next

            _udtVoucherRefundList = New VoucherRefund.VoucherRefundModelCollection
            For Each udtVoucherRefund As VoucherRefund.VoucherRefundModel In udtVoucherDetailModel.VoucherRefundList
                _udtVoucherRefundList.Add(New VoucherRefund.VoucherRefundModel(udtVoucherRefund))
            Next

        End Sub

#End Region

    End Class

End Namespace