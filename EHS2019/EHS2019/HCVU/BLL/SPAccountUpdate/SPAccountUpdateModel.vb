<Serializable()> Public Class SPAccountUpdateModel
    Private _strEnrolRefNo As String
    Private _strSPID As String
    Private _blnUpdSPInfo As Boolean
    Private _blnUpdBankAccount As Boolean
    Private _blnUpdProfessional As Boolean
    Private _blnIssueToken As Boolean
    Private _blnSchemeConfirm As Boolean
    Private _strProgressStatus As String
    Private _strUpdateBy As String
    Private _dtmUpdateDtm As Nullable(Of DateTime)
    Private _byteTSMP As Byte()
    ' INT13-0028 - SP Amendment Report [Start][Tommy L]
    ' -------------------------------------------------------------------------
    Private _strDataInputBy As String
    ' INT13-0028 - SP Amendment Report [End][Tommy L]

    Public Const UpdateByDataType As SqlDbType = SqlDbType.VarChar
    Public Const UpdateByDataSize As Integer = 20

    Public Const UpdSPInofDataType As SqlDbType = SqlDbType.Char
    Public Const UpdSPInofDataSize As Integer = 1

    Public Const UpdBankAcctDataType As SqlDbType = SqlDbType.Char
    Public Const UpdBankAcctDataSize As Integer = 1

    Public Const UpdProfessionalDataType As SqlDbType = SqlDbType.Char
    Public Const UpdProfessionalDataSize As Integer = 1

    Public Const IssueTokenDataType As SqlDbType = SqlDbType.Char
    Public Const IssueTokenDataSize As Integer = 1

    Public Const SchemeConfirmDataType As SqlDbType = SqlDbType.Char
    Public Const SchemeConfirmDataSize As Integer = 1

    Public Const ProgressStatusDataType As SqlDbType = SqlDbType.Char
    Public Const ProgressStatusDataSize As Integer = 1

    ' INT13-0028 - SP Amendment Report [Start][Tommy L]
    ' -------------------------------------------------------------------------
    Public Const DataInputByDataType As SqlDbType = SqlDbType.VarChar
    Public Const DataInputByDataSize As Integer = 20
    ' INT13-0028 - SP Amendment Report [End][Tommy L]

    Public Property EnrolRefNo() As String
        Get
            Return _strEnrolRefNo
        End Get
        Set(ByVal value As String)
            _strEnrolRefNo = value
        End Set
    End Property

    Public Property SPID() As String
        Get
            Return _strSPID
        End Get
        Set(ByVal value As String)
            _strSPID = value
        End Set
    End Property

    Public Property UpdateDtm() As Nullable(Of DateTime)
        Get
            Return _dtmUpdateDtm
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _dtmUpdateDtm = value
        End Set
    End Property

    Public Property UpdateBy() As String
        Get
            Return _strUpdateBy
        End Get
        Set(ByVal value As String)
            _strUpdateBy = value
        End Set
    End Property

    Public Property UpdateSPInfo() As Boolean
        Get
            Return _blnUpdSPInfo
        End Get
        Set(ByVal value As Boolean)
            _blnUpdSPInfo = value
        End Set
    End Property

    Public Property UpdateBankAcct() As Boolean
        Get
            Return _blnUpdBankAccount
        End Get
        Set(ByVal value As Boolean)
            _blnUpdBankAccount = value
        End Set
    End Property

    Public Property UpdateProfessional() As Boolean
        Get
            Return _blnUpdProfessional
        End Get
        Set(ByVal value As Boolean)
            _blnUpdProfessional = value
        End Set
    End Property

    Public Property IssueToken() As Boolean
        Get
            Return _blnIssueToken
        End Get
        Set(ByVal value As Boolean)
            _blnIssueToken = value
        End Set
    End Property

    Public Property SchemeConfirm() As Boolean
        Get
            Return _blnSchemeConfirm
        End Get
        Set(ByVal value As Boolean)
            _blnSchemeConfirm = value
        End Set
    End Property

    Public Property ProgressStatus() As String
        Get
            Return _strProgressStatus
        End Get
        Set(ByVal value As String)
            _strProgressStatus = value
        End Set
    End Property

    Public Property TSMP() As Byte()
        Get
            Return _byteTSMP
        End Get
        Set(ByVal value As Byte())
            _byteTSMP = value
        End Set
    End Property

    ' INT13-0028 - SP Amendment Report [Start][Tommy L]
    ' -------------------------------------------------------------------------
    Public Property DataInputBy() As String
        Get
            Return _strDataInputBy
        End Get
        Set(ByVal value As String)
            _strDataInputBy = value
        End Set
    End Property
    ' INT13-0028 - SP Amendment Report [End][Tommy L]

    Public Sub New()

    End Sub

    Public Sub New(ByVal udtSPAccountUpdateModel As SPAccountUpdateModel)
        _strEnrolRefNo = udtSPAccountUpdateModel.EnrolRefNo
        _strSPID = udtSPAccountUpdateModel.SPID
        _blnUpdSPInfo = udtSPAccountUpdateModel.UpdateSPInfo
        _blnUpdBankAccount = udtSPAccountUpdateModel.UpdateBankAcct
        _blnUpdProfessional = udtSPAccountUpdateModel.UpdateProfessional
        _blnIssueToken = udtSPAccountUpdateModel.IssueToken
        _blnSchemeConfirm = udtSPAccountUpdateModel.SchemeConfirm
        _strProgressStatus = udtSPAccountUpdateModel.ProgressStatus
        _strUpdateBy = udtSPAccountUpdateModel.UpdateBy
        _dtmUpdateDtm = udtSPAccountUpdateModel.UpdateDtm
        _byteTSMP = udtSPAccountUpdateModel.TSMP
        ' INT13-0028 - SP Amendment Report [Start][Tommy L]
        ' -------------------------------------------------------------------------
        _strDataInputBy = udtSPAccountUpdateModel.DataInputBy
        ' INT13-0028 - SP Amendment Report [End][Tommy L]
    End Sub

    ' INT13-0028 - SP Amendment Report [Start][Tommy L]
    ' -------------------------------------------------------------------------
    'Public Sub New(ByVal strEnrolRefNo As String, ByVal strSPID As String, ByVal blnUpdSPInfo As Boolean, _
    '                ByVal blnUpdBankAccount As Boolean, ByVal blnUpdProfessional As Boolean, ByVal blnIssueToken As Boolean, _
    '                ByVal blnSchemeConfirm As Boolean, ByVal strProgressStatus As String, ByVal strUpdateBy As String, ByVal dtmUpdateDtm As Nullable(Of DateTime), _
    '                ByVal byteTSMP As Byte())
    Public Sub New(ByVal strEnrolRefNo As String, ByVal strSPID As String, ByVal blnUpdSPInfo As Boolean, _
                ByVal blnUpdBankAccount As Boolean, ByVal blnUpdProfessional As Boolean, ByVal blnIssueToken As Boolean, _
                ByVal blnSchemeConfirm As Boolean, ByVal strProgressStatus As String, ByVal strUpdateBy As String, ByVal dtmUpdateDtm As Nullable(Of DateTime), _
                ByVal byteTSMP As Byte(), ByVal strDataInputBy As String)
        ' INT13-0028 - SP Amendment Report [End][Tommy L]

        _strEnrolRefNo = strEnrolRefNo
        _strSPID = strSPID
        _blnUpdSPInfo = blnUpdSPInfo
        _blnUpdBankAccount = blnUpdBankAccount
        _blnUpdProfessional = blnUpdProfessional
        _blnIssueToken = blnIssueToken
        _blnSchemeConfirm = blnSchemeConfirm
        _strProgressStatus = strProgressStatus
        _strUpdateBy = strUpdateBy
        _dtmUpdateDtm = dtmUpdateDtm
        _byteTSMP = byteTSMP
        ' INT13-0028 - SP Amendment Report [Start][Tommy L]
        ' -------------------------------------------------------------------------
        _strDataInputBy = strDataInputBy
        ' INT13-0028 - SP Amendment Report [End][Tommy L]
    End Sub
End Class
