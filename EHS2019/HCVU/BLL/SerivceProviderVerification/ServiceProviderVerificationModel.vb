<Serializable()> Public Class ServiceProviderVerificationModel
    Private _strEnrolRefNo As String
    Private _strSPID As String
    Private _strUpdateBy As String
    Private _dtmUpdateDtm As Nullable(Of DateTime)
    Private _blnSPConfirmed As Boolean
    Private _blnMOConfirmed As Boolean
    Private _blnPracitceConfirmed As Boolean
    Private _blnBankAcctConfirmed As Boolean
    Private _blnSchemeConfirmed As Boolean
    Private _strEnterConfirmBy As String
    Private _dtmEnterConfirmDtm As Nullable(Of DateTime)
    Private _strVettingBy As String
    Private _dtmVettingDtm As Nullable(Of DateTime)
    Private _strDeferBy As String
    Private _dtmDeferDtm As Nullable(Of DateTime)
    Private _strVoidBy As String
    Private _dtmVoidDtm As Nullable(Of DateTime)
    Private _strReturnForAmendmentBy As String
    Private _dtmReturnForAmendmentDtm As Nullable(Of DateTime)
    Private _strRecordStatus As String
    Private _byteTSMP As Byte()

    Public Const UpdateByDataType As SqlDbType = SqlDbType.VarChar
    Public Const UpdateByDataSize As Integer = 20

    Public Const SPConfirmedDataType As SqlDbType = SqlDbType.Char
    Public Const SPConfirmedDataSize As Integer = 1

    Public Const MOConfirmedDataType As SqlDbType = SqlDbType.Char
    Public Const MOConfirmedDataSize As Integer = 1

    Public Const PracticeConfirmedDataType As SqlDbType = SqlDbType.Char
    Public Const PracticeConfirmedDataSize As Integer = 1

    Public Const BankAcctConfirmedDataType As SqlDbType = SqlDbType.Char
    Public Const BankAcctConfirmedDataSize As Integer = 1

    Public Const SchemeConfirmedDataType As SqlDbType = SqlDbType.Char
    Public Const SchemeConfirmedDataSize As Integer = 1

    Public Const EnterByDataType As SqlDbType = SqlDbType.VarChar
    Public Const EnterByDataSize As Integer = 20

    Public Const VettingByDataType As SqlDbType = SqlDbType.VarChar
    Public Const VettingByDataSize As Integer = 20

    Public Const DeferByDataType As SqlDbType = SqlDbType.VarChar
    Public Const DeferByDataSize As Integer = 20

    Public Const VoidByDataType As SqlDbType = SqlDbType.VarChar
    Public Const VoidByDataSize As Integer = 20

    Public Const VoidDtmDataType As SqlDbType = SqlDbType.DateTime
    Public Const VoidDtmDataSize As Integer = 8

    Public Const ReturnForAmendmentByDataType As SqlDbType = SqlDbType.VarChar
    Public Const ReturnForAmendmentByDataSize As Integer = 20

    Public Const RecordStatusDataType As SqlDbType = SqlDbType.Char
    Public Const RecordStatusDataSize As Integer = 1

    Public Const TSMPDataType As SqlDbType = SqlDbType.Timestamp
    Public Const TSMPDataSize As Integer = 8

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

    Public Property EnterConfirmDtm() As Nullable(Of DateTime)
        Get
            Return _dtmEnterConfirmDtm
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _dtmEnterConfirmDtm = value
        End Set
    End Property

    Public Property EnterConfirmBy() As String
        Get
            Return _strEnterConfirmBy
        End Get
        Set(ByVal value As String)
            _strEnterConfirmBy = value
        End Set
    End Property

    Public Property VettingDtm() As Nullable(Of DateTime)
        Get
            Return _dtmVettingDtm
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _dtmVettingDtm = value
        End Set
    End Property

    Public Property VettingBy() As String
        Get
            Return _strVettingBy
        End Get
        Set(ByVal value As String)
            _strVettingBy = value
        End Set
    End Property

    Public Property DeferDtm() As Nullable(Of DateTime)
        Get
            Return _dtmDeferDtm
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _dtmDeferDtm = value
        End Set
    End Property

    Public Property DeferBy() As String
        Get
            Return _strDeferBy
        End Get
        Set(ByVal value As String)
            _strDeferBy = value
        End Set
    End Property

    Public Property VoidDtm() As Nullable(Of DateTime)
        Get
            Return _dtmVoidDtm
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _dtmVoidDtm = value
        End Set
    End Property

    Public Property VoidBy() As String
        Get
            Return _strVoidBy
        End Get
        Set(ByVal value As String)
            _strVoidBy = value
        End Set
    End Property

    Public Property ReturnForAmendmentDtm() As Nullable(Of DateTime)
        Get
            Return _dtmReturnForAmendmentDtm
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _dtmReturnForAmendmentDtm = value
        End Set
    End Property

    Public Property ReturnForAmendmentBy() As String
        Get
            Return _strReturnForAmendmentBy
        End Get
        Set(ByVal value As String)
            _strReturnForAmendmentBy = value
        End Set
    End Property

    Public Property SPConfirmed() As Boolean
        Get
            Return _blnSPConfirmed
        End Get
        Set(ByVal value As Boolean)
            _blnSPConfirmed = value
        End Set
    End Property

    Public Property MOConfirmed() As Boolean
        Get
            Return _blnMOConfirmed
        End Get
        Set(ByVal value As Boolean)
            _blnMOConfirmed = value
        End Set
    End Property

    Public Property PracticeConfirmed() As Boolean
        Get
            Return _blnPracitceConfirmed
        End Get
        Set(ByVal value As Boolean)
            _blnPracitceConfirmed = value
        End Set
    End Property

    Public Property BankAcctConfirmed() As Boolean
        Get
            Return _blnBankAcctConfirmed
        End Get
        Set(ByVal value As Boolean)
            _blnBankAcctConfirmed = value
        End Set
    End Property

    Public Property SchemeConfirmed() As Boolean
        Get
            Return _blnSchemeConfirmed
        End Get
        Set(ByVal value As Boolean)
            _blnSchemeConfirmed = value
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

    Public Property RecordStatus() As String
        Get
            Return _strRecordStatus
        End Get
        Set(ByVal value As String)
            _strRecordStatus = value
        End Set
    End Property

    Public Sub New()

    End Sub

    Public Sub New(ByVal udtSPVerificationModel As ServiceProviderVerificationModel)
        _strEnrolRefNo = udtSPVerificationModel.EnrolRefNo
        _strSPID = udtSPVerificationModel.SPID
        _strUpdateBy = udtSPVerificationModel.UpdateBy
        _dtmUpdateDtm = udtSPVerificationModel.UpdateDtm
        _blnSPConfirmed = udtSPVerificationModel.SPConfirmed
        _blnMOConfirmed = udtSPVerificationModel.MOConfirmed
        _blnPracitceConfirmed = udtSPVerificationModel.PracticeConfirmed
        _blnBankAcctConfirmed = udtSPVerificationModel.BankAcctConfirmed
        _blnSchemeConfirmed = udtSPVerificationModel.SchemeConfirmed
        _strEnterConfirmBy = udtSPVerificationModel.EnterConfirmBy
        _dtmEnterConfirmDtm = udtSPVerificationModel.EnterConfirmDtm
        _strVettingBy = udtSPVerificationModel.VettingBy
        _dtmVettingDtm = udtSPVerificationModel.VettingDtm
        _strVoidBy = udtSPVerificationModel.VoidBy
        _dtmVoidDtm = udtSPVerificationModel.VoidDtm
        _strDeferBy = udtSPVerificationModel.DeferBy
        _dtmDeferDtm = udtSPVerificationModel.DeferDtm
        _strReturnForAmendmentBy = udtSPVerificationModel.ReturnForAmendmentBy
        _dtmReturnForAmendmentDtm = udtSPVerificationModel.ReturnForAmendmentDtm
        _strRecordStatus = udtSPVerificationModel.RecordStatus
        _byteTSMP = udtSPVerificationModel.TSMP
    End Sub

    Public Sub New(ByVal strEnrolRefNo As String, ByVal strSPID As String, ByVal strUpdateBy As String, _
                    ByVal dtmUpdateDtm As Nullable(Of DateTime), ByVal blnSPConfirmed As Boolean, ByVal blnMOConfirmed As Boolean, ByVal blnPracitceConfirmed As Boolean, _
                    ByVal blnBankAcctConfirmed As Boolean, ByVal blnSchemeConfirmed As Boolean, ByVal strEnterConfirmBy As String, ByVal dtmEnterDtm As Nullable(Of DateTime), _
                    ByVal strVettingBy As String, ByVal dtmVettingDtm As Nullable(Of DateTime), ByVal strVoidBy As String, ByVal dtmVoidDtm As Nullable(Of DateTime), _
                    ByVal strDeferBy As String, ByVal dtmDeferDtm As Nullable(Of DateTime), ByVal strReturnForAmendmentBy As String, ByVal dtmReturnForAmendmentDtm As Nullable(Of DateTime), _
                    ByVal strRecordStatus As String, ByVal byteTSMP As Byte())

        _strEnrolRefNo = strEnrolRefNo
        _strSPID = strSPID
        _strUpdateBy = strUpdateBy
        _dtmUpdateDtm = dtmUpdateDtm
        _blnSPConfirmed = blnSPConfirmed
        _blnMOConfirmed = blnMOConfirmed
        _blnPracitceConfirmed = blnPracitceConfirmed
        _blnBankAcctConfirmed = blnBankAcctConfirmed
        _blnSchemeConfirmed = blnSchemeConfirmed
        _strEnterConfirmBy = strEnterConfirmBy
        _dtmEnterConfirmDtm = dtmEnterDtm
        _strVettingBy = strVettingBy
        _dtmVettingDtm = dtmVettingDtm
        _strVoidBy = strVoidBy
        _dtmVoidDtm = dtmVoidDtm
        _strDeferBy = strDeferBy
        _dtmDeferDtm = dtmDeferDtm
        _strReturnForAmendmentBy = strReturnForAmendmentBy
        _dtmReturnForAmendmentDtm = dtmReturnForAmendmentDtm
        _strRecordStatus = strRecordStatus
        _byteTSMP = byteTSMP
    End Sub

End Class


