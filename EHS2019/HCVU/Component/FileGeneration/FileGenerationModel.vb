Namespace Component.FileGeneration
    <Serializable()> Public Class FileGenerationModel

        Private _strFileID As String
        Private _strFileName As String

        Public Property FileID() As String
            Get
                Return _strFileID
            End Get
            Set(ByVal value As String)
                _strFileID = value
            End Set
        End Property

        Public Property FileName() As String
            Get
                Return _strFileName
            End Get
            Set(ByVal value As String)
                _strFileName = value
            End Set
        End Property

    End Class
End Namespace

