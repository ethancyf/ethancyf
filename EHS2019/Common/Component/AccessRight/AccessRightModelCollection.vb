Imports System.Runtime.Serialization

Namespace Component.AccessRight
    <Serializable()> Public Class AccessRightModelCollection
        Inherits System.Collections.Hashtable

        Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            MyBase.New(info, context)
        End Sub

        Public Overloads Function Add(ByVal FunctionCode As String, ByVal udtAccessRight As AccessRightModel) As Boolean
            If MyBase.ContainsKey(FunctionCode) Then
                Return False
            Else
                MyBase.Add(FunctionCode, udtAccessRight)
                Return True
            End If
        End Function

        Public Overloads Sub Remove(ByVal strFunctionCode As String)
            If MyBase.ContainsKey(strFunctionCode) Then
                MyBase.Remove(strFunctionCode)
            End If
        End Sub

        Public Shadows Function Item(ByVal strFunctionCode As String) As AccessRightModel
            If Not MyBase.Item(strFunctionCode) Is Nothing Then
                Return CType(MyBase.Item(strFunctionCode), AccessRightModel)
            Else
                Return New AccessRightModel(False)
            End If
        End Function

    End Class
End Namespace

