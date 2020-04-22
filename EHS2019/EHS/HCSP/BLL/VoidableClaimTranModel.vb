<Serializable()> Public Class VoidableClaimTranModel
    Private _dtmConfirmedDtm As DateTime
    Private _dtmTranDate As DateTime
    Private _strTranNo As String
    Private _strVoucherAccID As String
    Private _strSPID As String
    Private _strDataEntryBy As String
    Private _strRecordStatus As String

    Public Sub New()
    End Sub

    Public Property ConfirmedDtm() As DateTime
        Get
            Return Me._dtmConfirmedDtm
        End Get
        Set(ByVal value As DateTime)
            Me._dtmConfirmedDtm = value
        End Set
    End Property

    Public Property TranDate() As DateTime
        Get
            Return Me._dtmTranDate
        End Get
        Set(ByVal value As DateTime)
            Me._dtmTranDate = value
        End Set
    End Property

    Public Property TranNo() As String
        Get
            Return Me._strTranNo
        End Get
        Set(ByVal value As String)
            Me._strTranNo = value
        End Set
    End Property

    Public Property VoucherAccID() As String
        Get
            Return Me._strVoucherAccID
        End Get
        Set(ByVal value As String)
            Me._strVoucherAccID = value
        End Set
    End Property

    Public Property SPID() As String
        Get
            Return Me._strSPID
        End Get
        Set(ByVal value As String)
            Me._strSPID = value
        End Set
    End Property

    Public Property DataEntryBy() As String
        Get
            Return Me._strDataEntryBy
        End Get
        Set(ByVal value As String)
            Me._strDataEntryBy = value
        End Set
    End Property

    Public Property RecordStatus() As String
        Get
            Return Me._strRecordStatus
        End Get
        Set(ByVal value As String)
            Me._strRecordStatus = value
        End Set
    End Property
End Class