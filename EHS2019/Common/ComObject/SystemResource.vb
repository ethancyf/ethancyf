Namespace ComObject

    <Serializable()> Public Class SystemResource

        Private _strObjectType As String
        Private _strObjectName As String

        Public ReadOnly Property ObjectType() As String
            Get
                Return _strObjectType
            End Get
        End Property

        Public ReadOnly Property ObjectName() As String
            Get
                Return _strObjectName
            End Get
        End Property

        Public Sub New(ByVal strObjectType As String, ByVal strObjectName As String)
            Me._strObjectType = strObjectType
            Me._strObjectName = strObjectName

        End Sub

    End Class


End Namespace