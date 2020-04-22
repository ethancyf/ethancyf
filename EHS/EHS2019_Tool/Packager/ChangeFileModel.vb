<Serializable()> Public Class ChangeFileModel
    Private _strFileName As String
    Private _aryCR As ArrayList
    Private _aryUpdater As ArrayList
    Private _blnNewFile As Boolean = False
    Private _strStatus As String
    Private _LastWriteTime As String
    Private _check As Boolean = False
    Private Sub New()
        ' Not accessible
    End Sub

    Public Sub New(ByVal strFilePath As String)
        _strFileName = strFilePath
        _aryCR = New ArrayList
        _aryUpdater = New ArrayList
    End Sub

    Public ReadOnly Property FileName() As String
        Get
            Return _strFileName
        End Get
    End Property

    Public ReadOnly Property AryCR() As ArrayList
        Get
            Return _aryCR
        End Get
    End Property

    Public ReadOnly Property AryUpdater() As ArrayList
        Get
            Return _aryUpdater
        End Get
    End Property

    Public Property NewFile() As Boolean
        Get
            Return _blnNewFile
        End Get
        Set(ByVal value As Boolean)
            _blnNewFile = value
        End Set
    End Property

    Public Property Status() As String
        Get
            Return _strStatus
        End Get
        Set(ByVal value As String)
            _strStatus = value
        End Set
    End Property

    Public Property LastWriteTime() As String
        Get
            Return _LastWriteTime
        End Get
        Set(ByVal value As String)
            _LastWriteTime = value
        End Set
    End Property

    Public ReadOnly Property Updater() As String
        Get
            If _aryUpdater.Count = 0 Then Return String.Empty

            Dim strResult As String = String.Empty

            For Each strUpdater As String In _aryUpdater
                strResult += ", " + strUpdater
            Next

            Return strResult.Substring(2)

        End Get
    End Property

    Public ReadOnly Property CR() As String
        Get
            If _aryCR.Count = 0 Then Return String.Empty

            Dim strResult As String = String.Empty

            For Each strCR As String In _aryCR
                strResult += ", " + strCR
            Next

            Return strResult.Substring(2)

        End Get
    End Property
    Public Property check() As Boolean
        Get
            Return _check
        End Get
        Set(ByVal value As Boolean)
            _check = value
        End Set
    End Property
End Class