<Serializable()> Public Class voCheckClaimbyVRInfo

    Private _result As voResult = Nothing
    Private _info As voCCVRInfo = Nothing
    Private _arrVoucherInfo As voCCVRVoucher_Info() = Nothing

    Property Result() As voResult
        Get
            Return Me._result
        End Get
        Set(ByVal value As voResult)
            Me._result = value
        End Set
    End Property

    Property Info() As voCCVRInfo
        Get
            Return Me._info
        End Get
        Set(ByVal value As voCCVRInfo)
            Me._info = value
        End Set
    End Property

    Property VoucherInfoList() As voCCVRVoucher_Info()
        Get
            Return Me._arrVoucherInfo
        End Get
        Set(ByVal value As voCCVRVoucher_Info())
            Me._arrVoucherInfo = value
        End Set
    End Property

    Public Sub New()

    End Sub

    Public Sub New(ByVal _dsCheckClaimbyVRInfo As dsCheckClaimbyVRInfo)
        _result = New voResult()
        _result.Return = _dsCheckClaimbyVRInfo.Result(0)._Return

        If _dsCheckClaimbyVRInfo.Info.Rows.Count > 0 Then
            _info = New voCCVRInfo()
            _info.VRAcctID = _dsCheckClaimbyVRInfo.Info(0).VRAcctID
            _info.HKID = _dsCheckClaimbyVRInfo.Info(0).HKID
            _info.Name = _dsCheckClaimbyVRInfo.Info(0).Name
            _info.TSW = _dsCheckClaimbyVRInfo.Info(0).TSW
        End If

        If _dsCheckClaimbyVRInfo.Voucher_Info.Rows.Count > 0 Then
            ReDim Me._arrVoucherInfo(_dsCheckClaimbyVRInfo.Voucher_Info.Rows.Count - 1)
            Dim i As Integer = 0
            For Each drRow As dsCheckClaimbyVRInfo.Voucher_InfoRow In _dsCheckClaimbyVRInfo.Voucher_Info
                Me._arrVoucherInfo(i) = New voCCVRVoucher_Info
                Me._arrVoucherInfo(i).VRAcctID = drRow.VRAcctID
                Me._arrVoucherInfo(i).Left = drRow.Left
                Me._arrVoucherInfo(i).Value = drRow.Value
                Me._arrVoucherInfo(i).VoucherResult = drRow.VoucherResult

                If _dsCheckClaimbyVRInfo.Quota.Rows.Count > 0 Then
                    ReDim Me._arrVoucherInfo(i).QuotaList(_dsCheckClaimbyVRInfo.Quota.Rows.Count - 1)
                    Dim j As Integer = 0
                    For Each drQuota As dsCheckClaimbyVRInfo.QuotaRow In _dsCheckClaimbyVRInfo.Quota
                        Me._arrVoucherInfo(i).QuotaList(j) = New Quota
                        Me._arrVoucherInfo(i).QuotaList(j).Quota_Professional = drQuota.Quota_Professional
                        Me._arrVoucherInfo(i).QuotaList(j).Quota_Balance = drQuota.Quota_Balance
                        Me._arrVoucherInfo(i).QuotaList(j).Quota_MaxUsableBalance = drQuota.Quota_MaxUsableBalance
                        Me._arrVoucherInfo(i).QuotaList(j).Quota_Capping = drQuota.Quota_Capping
                        Me._arrVoucherInfo(i).QuotaList(j).Quota_ExpiryDate = drQuota.Quota_ExpiryDate
                        j = j + 1
                    Next
                End If

                i = i + 1
            Next
        End If
    End Sub

End Class

<Serializable()> Public Class voCCVRInfo
    Private strVRAcctID As String
    Private strHKID As String
    Private strName As String
    Private strTSW As String

    Property VRAcctID() As String
        Get
            Return Me.strVRAcctID
        End Get
        Set(ByVal value As String)
            Me.strVRAcctID = value
        End Set
    End Property

    Property HKID() As String
        Get
            Return Me.strHKID
        End Get
        Set(ByVal value As String)
            Me.strHKID = value
        End Set
    End Property

    Property Name() As String
        Get
            Return Me.strName
        End Get
        Set(ByVal value As String)
            Me.strName = value
        End Set
    End Property

    Property TSW() As String
        Get
            Return Me.strTSW
        End Get
        Set(ByVal value As String)
            Me.strTSW = value
        End Set
    End Property

    Public Sub New()

    End Sub
End Class

<Serializable()> Public Class voCCVRVoucher_Info
    Private strVRAcctID As String
    Private strLeft As String
    Private strValue As String
    Private strVoucherResult As String
    Private arrQuota As Quota() = Nothing

    Property VRAcctID() As String
        Get
            Return Me.strVRAcctID
        End Get
        Set(ByVal value As String)
            Me.strVRAcctID = value
        End Set
    End Property

    Property Left() As String
        Get
            Return Me.strLeft
        End Get
        Set(ByVal value As String)
            Me.strLeft = value
        End Set
    End Property

    Property Value() As String
        Get
            Return Me.strValue
        End Get
        Set(ByVal value As String)
            Me.strValue = value
        End Set
    End Property

    Property VoucherResult() As String
        Get
            Return Me.strVoucherResult
        End Get
        Set(ByVal value As String)
            Me.strVoucherResult = value
        End Set
    End Property

    Property QuotaList() As Quota()
        Get
            Return Me.arrQuota
        End Get
        Set(ByVal value As Quota())
            Me.arrQuota = value
        End Set
    End Property

    Public Sub New()

    End Sub
End Class




