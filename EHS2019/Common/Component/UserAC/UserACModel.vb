Namespace Component.UserAC
    <Serializable()> Public Class UserACModel

        'Protected _strUserID As String
        Protected _dtmLastLoginDtm As Nullable(Of DateTime)
        Protected _dtmLastUnsuccessLoginDtm As Nullable(Of DateTime)

        Protected _strUserType As String
        Protected _UserACTSMP As Byte()

        Private _dtmLastPwdChangeDtm As Nullable(Of DateTime)
        Private _intLastPwdChangeDuration As Nullable(Of Integer)
        Private _strUserACRecordStatus As String
        Private _strTokenSerialNo As String
        Private _strTokenSerialNoSSO As String = String.Empty
        Private _intSPTokenCnt As Integer
        Private _strPrintOption As String 
        Protected _strDefaultLanguage As String

        Public Sub New()
            _strUserType = ""
            '_strUserDescription = ""
            '_dtmLastLoginDtm = DateTime.MinValue
            '_dtmLastUnsuccessLoginDtm = DateTime.MinValue
        End Sub

        Public Property UserType() As String
            Get
                Return _strUserType
            End Get
            Set(ByVal value As String)
                _strUserType = value
            End Set
        End Property

        Public Property LastLoginDtm() As Nullable(Of DateTime)
            Get
                Return _dtmLastLoginDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmLastLoginDtm = value
            End Set
        End Property

        Public Property LastUnsuccessLoginDtm() As Nullable(Of DateTime)
            Get
                Return _dtmLastUnsuccessLoginDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmLastUnsuccessLoginDtm = value
            End Set
        End Property

        Public Property UserACTSMP() As Byte()
            Get
                Return _UserACTSMP
            End Get
            Set(ByVal value As Byte())
                _UserACTSMP = value
            End Set
        End Property

        Public Property DefaultLanguage() As String
            Get
                Return _strDefaultLanguage
            End Get
            Set(ByVal value As String)
                _strDefaultLanguage = value
            End Set
        End Property

        Public Property LastPwdChangeDtm() As Nullable(Of DateTime)
            Get
                Return _dtmLastPwdChangeDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmLastPwdChangeDtm = value
            End Set
        End Property

        Public Property LastPwdChangeDuration() As Nullable(Of Integer)
            Get
                Return _intLastPwdChangeDuration
            End Get
            Set(ByVal value As Nullable(Of Integer))
                _intLastPwdChangeDuration = value
            End Set
        End Property

        Public Property UserACRecordStatus() As String
            Get
                Return _strUserACRecordStatus
            End Get
            Set(ByVal value As String)
                _strUserACRecordStatus = value
            End Set
        End Property

        Public Property TokenSerialNo() As String
            Get
                Return _strTokenSerialNo
            End Get
            Set(ByVal value As String)
                _strTokenSerialNo = value
            End Set
        End Property

        Public Property SPTokenCnt() As Integer
            Get
                Return _intSPTokenCnt
            End Get
            Set(ByVal value As Integer)
                _intSPTokenCnt = value
            End Set
        End Property

        Public Property PrintOption() As String
            Get
                Return Me._strPrintOption
            End Get
            Set(ByVal value As String)
                Me._strPrintOption = value
            End Set
        End Property

        'Unqiue for Single Sign On
        Public Property TokenSerialNoForSSO() As String
            Get
                Return _strTokenSerialNoSSO
            End Get
            Set(ByVal value As String)
                _strTokenSerialNoSSO = value
            End Set
        End Property

    End Class
End Namespace

