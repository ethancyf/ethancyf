Namespace Component.UserRole
    <Serializable()> Public Class UserRoleModel

        Private _strUserID As String
        Private _intRoleType As Integer
        Private _strSchemeCode As String

        Public Const UserIDDataType As SqlDbType = SqlDbType.VarChar
        Public Const UserIDDataSize As Integer = 20

        Public Const RoleTypeDataType As SqlDbType = SqlDbType.SmallInt
        Public Const RoleTypeDataSize As Integer = 2

        Public Const SchemeCodeDataType As SqlDbType = SqlDbType.Char
        Public Const SchemeCodeDataSize As Integer = 10

        Public Sub New()

        End Sub

        Public Property UserID() As String
            Get
                Return _strUserID
            End Get
            Set(ByVal value As String)
                _strUserID = value
            End Set
        End Property

        Public Property RoleType() As Integer
            Get
                Return _intRoleType
            End Get
            Set(ByVal value As Integer)
                _intRoleType = value
            End Set
        End Property

        Public Property SchemeCode() As String
            Get
                Return _strSchemeCode
            End Get
            Set(ByVal value As String)
                _strSchemeCode = value
            End Set
        End Property

        Public Sub New(ByVal udtUserRoleModel As UserRoleModel)
            Me._strUserID = udtUserRoleModel.UserID
            Me._intRoleType = udtUserRoleModel.RoleType
            _strSchemeCode = udtUserRoleModel.SchemeCode
        End Sub

        Public Sub New(ByVal strUserID As String, ByVal intRoleType As Integer, ByVal strSchemeCode As String)
            _strUserID = strUserID
            _intRoleType = intRoleType
            _strSchemeCode = strSchemeCode
        End Sub

    End Class

End Namespace
