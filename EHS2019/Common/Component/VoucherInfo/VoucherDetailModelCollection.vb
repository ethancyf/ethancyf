Namespace Component.VoucherInfo
    <Serializable()> Public Class VoucherDetailModelCollection
        Inherits ArrayList

        Private _dicVoucherInfoDetailList As Dictionary(Of Integer, VoucherDetailModel) = Nothing
        Private _blnCache As Boolean = False

        Public Enum VoucherDetailPart
            NA
            Entitlement
            Used
            WriteOff
            Refund
        End Enum

        Public ReadOnly Property GetTotalEntitlement() As Integer
            Get
                Dim intRes As Integer = 0

                For Each udtVoucherDetail As VoucherDetailModel In Me
                    intRes += udtVoucherDetail.Entitlement
                Next

                Return intRes

            End Get
        End Property

        Public ReadOnly Property GetTotalUsed() As Integer
            Get
                Dim intRes As Integer = 0

                For Each udtVoucherDetail As VoucherDetailModel In Me
                    intRes += udtVoucherDetail.Used
                Next

                Return intRes

            End Get
        End Property

        Public ReadOnly Property GetTotalUsed(ByVal strSchemeCode As String) As Integer
            Get
                Dim intRes As Integer = 0

                For Each udtVoucherDetail As VoucherDetailModel In Me
                    intRes += udtVoucherDetail.Used(strSchemeCode)
                Next

                Return intRes

            End Get
        End Property

        Public ReadOnly Property GetTotalRefund() As Integer
            Get
                Dim intRes As Integer = 0

                For Each udtVoucherDetail As VoucherDetailModel In Me
                    intRes += udtVoucherDetail.Refund
                Next

                Return intRes

            End Get
        End Property

        Public ReadOnly Property GetTotalWriteOff() As Integer
            Get
                Dim intRes As Integer = 0

                For Each udtVoucherDetail As VoucherDetailModel In Me
                    intRes += udtVoucherDetail.WriteOff
                Next

                Return intRes

            End Get
        End Property

        Public ReadOnly Property GetAllTransaction() As EHSTransaction.TransactionDetailModelCollection
            Get
                ' Group all season transaction
                Dim udtTransactionDetailList As New EHSTransaction.TransactionDetailModelCollection

                For Each udtVoucherDetail As VoucherDetailModel In Me
                    udtTransactionDetailList.AddRange(udtVoucherDetail.TransactionDetails)
                Next

                Return udtTransactionDetailList

            End Get
        End Property

        Private ReadOnly Property VoucherDetailList() As Dictionary(Of Integer, VoucherDetailModel)
            Get
                If Me.Count > 0 Then
                    If Not _blnCache Then
                        Dim dicVoucherInfoDetailList As New Dictionary(Of Integer, VoucherDetailModel)

                        For Each udtVoucherInfoDetail As VoucherDetailModel In Me
                            dicVoucherInfoDetailList.Add(udtVoucherInfoDetail.SchemeSeq, udtVoucherInfoDetail)
                        Next

                        _dicVoucherInfoDetailList = dicVoucherInfoDetailList
                        _blnCache = True

                    End If

                    Return _dicVoucherInfoDetailList

                Else
                    Return Nothing

                End If

            End Get
        End Property

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As VoucherDetailModel
            Get
                Return CType(MyBase.Item(intIndex), VoucherDetailModel)
            End Get
        End Property

        Sub New()
            MyBase.New()
        End Sub

        Public Overloads Sub Add(ByVal udtVoucherDetail As VoucherDetailModel)
            MyBase.Add(udtVoucherDetail)
            _blnCache = False
        End Sub

        Public Overloads Sub Remove(ByVal intSchemeSeq As Integer)
            Dim udtResVoucherDetail As VoucherDetailModel = Me.Find(intSchemeSeq)

            If Not udtResVoucherDetail Is Nothing Then
                MyBase.Remove(udtResVoucherDetail)

                _blnCache = False
            End If

        End Sub

        Public Sub Merge(ByVal udtImportVoucherDetailList As VoucherDetailModelCollection, Optional ByVal enumVoucherDetail As VoucherDetailPart = VoucherDetailPart.NA)
            For Each udtImportVoucherDetail As VoucherDetailModel In udtImportVoucherDetailList
                Merge(udtImportVoucherDetail, enumVoucherDetail)
                _blnCache = False
            Next

        End Sub

        Private Sub Merge(ByVal udtImportVoucherDetail As VoucherDetailModel, Optional ByVal enumVoucherDetail As VoucherDetailPart = VoucherDetailPart.NA)
            Dim udtCurrentVoucherDetail As VoucherDetailModel = Nothing

            udtCurrentVoucherDetail = Me.Find(udtImportVoucherDetail.SchemeSeq)

            If udtCurrentVoucherDetail Is Nothing Then
                'New voucher info detail  
                Dim udtNewVoucherDetail As New VoucherDetailModel(udtImportVoucherDetail)

                MyBase.Add(udtNewVoucherDetail)

            Else
                'Update voucher info detail 

                Select Case enumVoucherDetail
                    Case VoucherDetailPart.Entitlement
                        With udtCurrentVoucherDetail
                            .PeriodStart = udtImportVoucherDetail.PeriodStart
                            .PeriodEnd = udtImportVoucherDetail.PeriodEnd
                            .Entitlement = udtImportVoucherDetail.Entitlement
                            .Ceiling = udtImportVoucherDetail.Ceiling
                        End With

                    Case VoucherDetailPart.Used
                        With udtCurrentVoucherDetail
                            .UsedByHCVS = udtImportVoucherDetail.Used(Scheme.SchemeClaimModel.HCVS)
                            .UsedByHCVSCHN = udtImportVoucherDetail.Used(Scheme.SchemeClaimModel.HCVSCHN)
                            ' CRE19-006 (DHC) [Start][Winnie]
                            ' ----------------------------------------------------------------------------------------
                            .UsedByHCVSDHC = udtImportVoucherDetail.Used(Scheme.SchemeClaimModel.HCVSDHC)
                            ' CRE19-006 (DHC) [End][Winnie]
                            .TransactionDetails = udtImportVoucherDetail.TransactionDetails
                        End With

                    Case VoucherDetailPart.WriteOff
                        With udtCurrentVoucherDetail
                            .WriteOff = udtImportVoucherDetail.WriteOff
                        End With

                    Case VoucherDetailPart.Refund
                        With udtCurrentVoucherDetail
                            .Refund = udtImportVoucherDetail.Refund
                            .VoucherRefundList = udtImportVoucherDetail.VoucherRefundList
                        End With

                    Case Else
                        Throw New Exception(String.Format("Invalid Enum of Voucher Detail({0})", enumVoucherDetail.ToString))
                End Select

            End If
        End Sub

        Public Function Find(ByVal intSchemeSeq As Integer) As VoucherDetailModel
            Dim dicVoucherDetailList As Dictionary(Of Integer, VoucherDetailModel) = Nothing
            Dim udtResVoucherInfoDetail As VoucherDetailModel = Nothing

            dicVoucherDetailList = Me.VoucherDetailList

            If Not dicVoucherDetailList Is Nothing AndAlso dicVoucherDetailList.ContainsKey(intSchemeSeq) Then
                udtResVoucherInfoDetail = dicVoucherDetailList.Item(intSchemeSeq)
            End If

            Return udtResVoucherInfoDetail

        End Function

        Public Function Copy() As VoucherDetailModelCollection
            Dim udtResVoucherDetailList As New VoucherDetailModelCollection

            For Each udtVoucherDetail As VoucherDetailModel In Me
                udtResVoucherDetailList.Add(New VoucherDetailModel(udtVoucherDetail))
            Next

            Return udtResVoucherDetailList

        End Function

        Public Function FilterByServiceDate(ByVal dtmServiceDate As Date) As VoucherDetailModelCollection
            Dim udtResVoucherDetailList As New VoucherDetailModelCollection

            For Each udtVoucherDetail As VoucherDetailModel In Me
                If udtVoucherDetail.PeriodStart <= dtmServiceDate And dtmServiceDate < udtVoucherDetail.PeriodEnd Then
                    udtResVoucherDetailList.Add(New VoucherDetailModel(udtVoucherDetail))
                End If
            Next

            If udtResVoucherDetailList.Count <> 1 Then
                Throw New Exception(String.Format("Invalid period(s) is/are found."))
            End If

            Return udtResVoucherDetailList

        End Function

        Public Function FilterListAfterSchemeSeq(ByVal intSchemeSeq As Integer) As VoucherDetailModelCollection
            Dim udtResVoucherDetailList As New VoucherDetailModelCollection

            For Each udtVoucherDetail As VoucherDetailModel In Me
                If udtVoucherDetail.SchemeSeq >= intSchemeSeq Then
                    udtResVoucherDetailList.Add(New VoucherDetailModel(udtVoucherDetail))
                End If
            Next

            Return udtResVoucherDetailList

        End Function

    End Class

End Namespace
