<Serializable()> Public Class voBalanceEnquiry
    Private _result As voResult = Nothing
    Private _info As voBalanceEnquiryInfo = Nothing
    Private _arrVoucherInfo As voBalanceEnquiryVoucher_Info() = Nothing

    Property Result() As voResult
        Get
            Return Me._result
        End Get
        Set(ByVal value As voResult)
            Me._result = value
        End Set
    End Property

    Property Info() As voBalanceEnquiryInfo
        Get
            Return Me._info
        End Get
        Set(ByVal value As voBalanceEnquiryInfo)
            Me._info = value
        End Set
    End Property

    Property VoucherInfoList() As voBalanceEnquiryVoucher_Info()
        Get
            Return Me._arrVoucherInfo
        End Get
        Set(ByVal value As voBalanceEnquiryVoucher_Info())
            Me._arrVoucherInfo = value
        End Set
    End Property

    Public Sub New()

    End Sub

    Public Sub New(ByVal _dsBalanceEnquiry As dsBalanceEnquiry)
        _result = New voResult()
        _result.Return = _dsBalanceEnquiry.Result(0)._Return

        If _dsBalanceEnquiry.Info.Rows.Count > 0 Then
            _info = New voBalanceEnquiryInfo()
            _info.HKID = _dsBalanceEnquiry.Info(0).HKID
            _info.Status = _dsBalanceEnquiry.Info(0).Status
        End If

        If _dsBalanceEnquiry.Voucher_Info.Rows.Count > 0 Then
            ReDim Me._arrVoucherInfo(_dsBalanceEnquiry.Voucher_Info.Rows.Count - 1)
            Dim i As Integer = 0
            For Each drVoucherInfo As dsBalanceEnquiry.Voucher_InfoRow In _dsBalanceEnquiry.Voucher_Info
                Me._arrVoucherInfo(i) = New voBalanceEnquiryVoucher_Info
                Me._arrVoucherInfo(i).HKID = drVoucherInfo.HKID
                Me._arrVoucherInfo(i).Left = drVoucherInfo.Left
                Me._arrVoucherInfo(i).Value = drVoucherInfo.Value

                If _dsBalanceEnquiry.Forfeit.Rows.Count > 0 Then
                    Dim drForfeit As dsBalanceEnquiry.ForfeitRow = _dsBalanceEnquiry.Forfeit(0)

                    Me._arrVoucherInfo(i).Forfeit = New Forfeit
                    Me._arrVoucherInfo(i).Forfeit.Next_Deposit_Amount = drForfeit.Next_Deposit_Amount
                    Me._arrVoucherInfo(i).Forfeit.Next_Capping_Amount = drForfeit.Next_Capping_Amount
                    Me._arrVoucherInfo(i).Forfeit.Next_Forfeit_Date = drForfeit.Next_Forfeit_Date
                    Me._arrVoucherInfo(i).Forfeit.Next_Forfeit_Amount = drForfeit.Next_Forfeit_Amount

                End If

                If _dsBalanceEnquiry.Quota.Rows.Count > 0 Then
                    Dim arrQuota(_dsBalanceEnquiry.Quota.Rows.Count - 1) As Quota
                    Dim j As Integer = 0

                    For Each drQuota As dsBalanceEnquiry.QuotaRow In _dsBalanceEnquiry.Quota
                        arrQuota(j) = New Quota
                        arrQuota(j).Quota_Professional = drQuota.Quota_Professional
                        arrQuota(j).Quota_Balance = drQuota.Quota_Balance
                        arrQuota(j).Quota_MaxUsableBalance = drQuota.Quota_MaxUsableBalance
                        arrQuota(j).Quota_Capping = drQuota.Quota_Capping
                        arrQuota(j).Quota_ExpiryDate = drQuota.Quota_ExpiryDate

                        j = j + 1
                    Next

                    Me._arrVoucherInfo(i).QuotaList = arrQuota

                End If

                i = i + 1
            Next

        End If

    End Sub

End Class

<Serializable()> Public Class voBalanceEnquiryInfo
    Private strHKID As String
    Private strStatus As String

    Property HKID() As String
        Get
            Return Me.strHKID
        End Get
        Set(ByVal value As String)
            Me.strHKID = value
        End Set
    End Property

    Property Status() As String
        Get
            Return Me.strStatus
        End Get
        Set(ByVal value As String)
            Me.strStatus = value
        End Set
    End Property

    Public Sub New()

    End Sub
End Class

<Serializable()> Public Class voBalanceEnquiryVoucher_Info
    Private strHKID As String
    Private strLeft As String
    Private strValue As String
    Private udtForfeit As Forfeit = Nothing
    Private arrQuota As Quota() = Nothing

    Property HKID() As String
        Get
            Return Me.strHKID
        End Get
        Set(ByVal value As String)
            Me.strHKID = value
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

    Property Forfeit() As Forfeit
        Get
            Return Me.udtForfeit
        End Get
        Set(ByVal value As Forfeit)
            Me.udtForfeit = value
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

<Serializable()> Public Class Quota
    Private strProfessional As String
    Private strBalance As String
    Private strMaxUsableBalance As String
    Private strCapping As String
    Private strExpiryDate As String

    Property Quota_Professional() As String
        Get
            Return Me.strProfessional
        End Get
        Set(ByVal value As String)
            Me.strProfessional = value
        End Set
    End Property

    Property Quota_Balance() As String
        Get
            Return Me.strBalance
        End Get
        Set(ByVal value As String)
            Me.strBalance = value
        End Set
    End Property

    Property Quota_MaxUsableBalance() As String
        Get
            Return Me.strMaxUsableBalance
        End Get
        Set(ByVal value As String)
            Me.strMaxUsableBalance = value
        End Set
    End Property

    Property Quota_Capping() As String
        Get
            Return Me.strCapping
        End Get
        Set(ByVal value As String)
            Me.strCapping = value
        End Set
    End Property

    Property Quota_ExpiryDate() As String
        Get
            Return Me.strExpiryDate
        End Get
        Set(ByVal value As String)
            Me.strExpiryDate = value
        End Set
    End Property

    Public Sub New()

    End Sub
End Class

<Serializable()> Public Class Forfeit
    Private strDepositAmount As String
    Private strCappingAmount As String
    Private strForfeitDate As String
    Private strForfeitAmount As String

    Property Next_Deposit_Amount() As String
        Get
            Return Me.strDepositAmount
        End Get
        Set(ByVal value As String)
            Me.strDepositAmount = value
        End Set
    End Property

    Property Next_Capping_Amount() As String
        Get
            Return Me.strCappingAmount
        End Get
        Set(ByVal value As String)
            Me.strCappingAmount = value
        End Set
    End Property

    Property Next_Forfeit_Date() As String
        Get
            Return Me.strForfeitDate
        End Get
        Set(ByVal value As String)
            Me.strForfeitDate = value
        End Set
    End Property

    Property Next_Forfeit_Amount() As String
        Get
            Return Me.strForfeitAmount
        End Get
        Set(ByVal value As String)
            Me.strForfeitAmount = value
        End Set
    End Property

    Public Sub New()

    End Sub
End Class
