Imports Common.Component.UserAC
Imports Common.Component.UserRole
Imports Common.Component.AccessRight
Imports Common.Component.Token

Namespace Component.HCVUUser
    <Serializable()> Public Class HCVUUserModel


        Private _strUserID As String
        Private _strUserName As String
        Private _strHKID As String

        ' CRE19-022 - Inspection [Begin][Golden]
        Private _strChineseName As String
        Private _strGender As String
        Private _strContactNo As String
        ' CRE19-022 - Inspection [End][Golden]

        Private _dtmExpiryDate As Nullable(Of DateTime)
        Private _dtmEffectiveDate As DateTime

        Private _udtUserRoleCollection As UserRoleModelCollection
        Private _udtAccessRightCollection As AccessRightModelCollection

        Private _dtmLastPwdChangeDtm As Nullable(Of DateTime)

        Private _dtmLastLoginDtm As Nullable(Of DateTime)
        Private _dtmLastUnsuccessLoginDtm As Nullable(Of DateTime)

        Private _blnSuspended As Boolean
        Private _blnLocked As Boolean
        Private _intLastPwdChangeDuration As Nullable(Of Integer)
        Private _tsmp As Byte()

        'Private _strTokenSerialNo As String

        Private _intTokenCnt As Integer

        Private _intPasswordLevel As Integer

        Private _udtToken As Token.TokenModel



        Public Sub New()
            '_udtUserRoleCollection = New UserRoleModelCollection
        End Sub

        Public Property UserID() As String
            Get
                Return _strUserID
            End Get
            Set(ByVal value As String)
                _strUserID = value
            End Set
        End Property

        Public Property UserName() As String
            Get
                Return _strUserName
            End Get
            Set(ByVal value As String)
                _strUserName = value
            End Set
        End Property

        Public Property UserRoleCollection() As UserRoleModelCollection
            Get
                Return _udtUserRoleCollection
            End Get
            Set(ByVal value As UserRoleModelCollection)
                _udtUserRoleCollection = value
            End Set
        End Property

        Public Property AccessRightCollection() As AccessRightModelCollection
            Get
                Return _udtAccessRightCollection
            End Get
            Set(ByVal value As AccessRightModelCollection)
                _udtAccessRightCollection = value
            End Set
        End Property

        Public Property Token() As TokenModel
            Get
                Return _udtToken
            End Get
            Set(ByVal value As TokenModel)
                _udtToken = value
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

        Public Property EffectiveDate() As DateTime
            Get
                Return _dtmEffectiveDate
            End Get
            Set(ByVal value As DateTime)
                _dtmEffectiveDate = value
            End Set
        End Property

        Public Property ExpiryDate() As Nullable(Of DateTime)
            Get
                Return _dtmExpiryDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmExpiryDate = value
            End Set
        End Property

        Public Property HKID() As String
            Get
                Return _strHKID
            End Get
            Set(ByVal value As String)
                _strHKID = value
            End Set
        End Property

        ' CRE19-022 - Inspection [Begin][Golden]
        Public Property ChineseName() As String
            Get
                Return _strChineseName
            End Get
            Set(ByVal value As String)
                _strChineseName = value
            End Set
        End Property

        Public Property Gender() As String
            Get
                Return _strGender
            End Get
            Set(ByVal value As String)
                _strGender = value
            End Set
        End Property

        Public Property ContactNo() As String
            Get
                Return _strContactNo
            End Get
            Set(ByVal value As String)
                _strContactNo = value
            End Set
        End Property

        ' CRE19-022 - Inspection [End][Golden]



        Public Property Suspended() As Boolean
            Get
                Return _blnSuspended
            End Get
            Set(ByVal value As Boolean)
                _blnSuspended = value
            End Set
        End Property

        Public Property Locked() As Boolean
            Get
                Return _blnLocked
            End Get
            Set(ByVal value As Boolean)
                _blnLocked = value
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

        Public Property TSMP() As Byte()
            Get
                Return _TSMP
            End Get
            Set(ByVal Value As Byte())
                _TSMP = Value
            End Set
        End Property

        Public Property TokenCnt() As Integer
            Get
                Return _intTokenCnt
            End Get
            Set(ByVal value As Integer)
                _intTokenCnt = value
            End Set
        End Property

        ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
        Public Property PasswordLevel() As Integer
            Get
                Return _intPasswordLevel
            End Get
            Set(ByVal value As Integer)
                _intPasswordLevel = value
            End Set
        End Property
        ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]

        'Public Property TokenSerialNo() As String
        '    Get
        '        Return _strTokenSerialNo
        '    End Get
        '    Set(ByVal value As String)
        '        _strTokenSerialNo = value
        '    End Set
        'End Property

        'Public Property ExpiryDate() As DateTime
        '    Get
        '        Return _dtmExpiryDate
        '    End Get
        '    Set(ByVal value As DateTime)
        '        _dtmExpiryDate = value
        '    End Set
        'End Property

    End Class
End Namespace

