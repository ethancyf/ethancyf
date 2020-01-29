Imports System.Runtime.Serialization

Namespace Component.UserRole
    <Serializable()> Public Class UserRoleModelCollection
        Inherits System.Collections.Hashtable

        Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            MyBase.New(info, context)
        End Sub

        Public Overloads Function Add(ByVal udtUserRole As UserRoleModel) As Boolean
            If MyBase.ContainsKey(udtUserRole.UserID.Trim & "-" & udtUserRole.RoleType.ToString & udtUserRole.SchemeCode.Trim) Then
                Return False
            Else
                MyBase.Add(udtUserRole.UserID.Trim & "-" & udtUserRole.RoleType.ToString & udtUserRole.SchemeCode.Trim, udtUserRole)
                Return True
            End If
        End Function

        Public Overloads Sub Remove(ByVal udtUserRole As UserRoleModel)
            If MyBase.ContainsKey(udtUserRole.UserID.Trim & "-" & udtUserRole.RoleType.ToString & udtUserRole.SchemeCode.Trim) Then
                MyBase.Remove(udtUserRole.UserID.Trim & "-" & udtUserRole.RoleType.ToString & udtUserRole.SchemeCode.Trim)
            End If
        End Sub

        'Public Overloads Sub Remove(ByVal RoleType As String)
        '    If MyBase.ContainsKey(RoleType) Then
        '        MyBase.Remove(RoleType)
        '    End If
        'End Sub

        'Default Public Overloads Property Item(ByVal key As String) As UserRoleModel
        '    Get
        '        Return CType(MyBase.Item(key), UserRoleModel)
        '    End Get
        '    Set(ByVal Value As UserRoleModel)
        '        MyBase.Item(key) = Value
        '    End Set
        'End Property

        Default Public Overloads Property Item(ByVal strUserID As String, ByVal intRoleType As Integer, ByVal strSchemeCode As String) As UserRoleModel
            Get
                Return CType(MyBase.Item(strUserID.Trim & "-" & intRoleType.ToString & strSchemeCode.Trim), UserRoleModel)
            End Get
            Set(ByVal Value As UserRoleModel)
                MyBase.Item(strUserID.Trim & "-" & intRoleType.ToString & strSchemeCode.Trim) = Value
            End Set
        End Property


        'Inherits ArrayList

        'Public Overloads Sub Add(ByVal udtUserRole As UserRoleModel)
        '    MyBase.Add(udtUserRole)
        'End Sub

        'Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As UserRoleModel
        '    Get
        '        Return CType(MyBase.Item(intIndex), UserRoleModel)
        '    End Get
        'End Property

        'Public Overloads Sub Remove(ByVal udtUserRole As UserRoleModel)
        '    MyBase.Remove(udtUserRole)
        'End Sub

    End Class
End Namespace


