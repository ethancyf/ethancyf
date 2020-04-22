Public Class BuildFileModel

    Private _strBuildFileName As String = String.Empty
    Private _strBuildFileCopyFrom As String = String.Empty

    Public ReadOnly Property BuildFileName() As String
        Get
            Return _strBuildFileName
        End Get
    End Property

    Public ReadOnly Property BuildFileCopyFrom() As String
        Get
            Return _strBuildFileCopyFrom
        End Get
    End Property

    Public Sub New(ByVal strBuildFileName As String, ByVal strBuildFileCopyFrom As String)
        Me._strBuildFileName = strBuildFileName
        Me._strBuildFileCopyFrom = strBuildFileCopyFrom
    End Sub
End Class
