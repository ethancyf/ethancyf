Public Class VerifySourceFileModel
    Private _fileName As String
    Private _status As String
        Public Property fileName() As String
            Get
                Return _fileName
            End Get
            Set(ByVal value As String)
                _fileName = value
            End Set
    End Property
    Public Property status() As String
        Get
            Return _status
        End Get
        Set(ByVal value As String)
            _status = value
        End Set
    End Property

    End Class