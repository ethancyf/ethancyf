Public Class ImmDImportXmlModel
    Private _strDocCode As String
    Private _strImportKeyTag As String
    Private _strImportMatchTag As String
    Private _strImportTagNature As String
    Private _strImportTable As String
    Private _strImportKeyDBColumn As String
    Private _strExportKeyTag As String
    Private _strRecordCountTag As String
    Private _strRecordCountTable As String

    Public Const IMPORT_KEY_ACCID As String = "KEY_ACCID"
    Public Const IMPORT_KEY_SEQNO As String = "KEY_SEQNO"
    Public Const IMPORT_MATCH As String = "MATCH"

    Property DocCode() As String
        Get
            Return Me._strDocCode
        End Get
        Set(ByVal value As String)
            Me._strDocCode = value
        End Set
    End Property

    Property ImportKeyTag() As String
        Get
            Return Me._strImportKeyTag
        End Get
        Set(ByVal value As String)
            Me._strImportKeyTag = value
        End Set
    End Property

    Property ImportMatchTag() As String
        Get
            Return Me._strImportMatchTag
        End Get
        Set(ByVal value As String)
            Me._strImportMatchTag = value
        End Set
    End Property

    Property ImportTagNature() As String
        Get
            Return Me._strImportTagNature
        End Get
        Set(ByVal value As String)
            Me._strImportTagNature = value
        End Set
    End Property

    Property ImportKeyDBColumn() As String
        Get
            Return Me._strImportKeyDBColumn
        End Get
        Set(ByVal value As String)
            Me._strImportKeyDBColumn = value
        End Set
    End Property

    Property ImportTable() As String
        Get
            Return Me._strImportTable
        End Get
        Set(ByVal value As String)
            Me._strImportTable = value
        End Set
    End Property

    Property ExportKeyTag() As String
        Get
            Return Me._strExportKeyTag
        End Get
        Set(ByVal value As String)
            Me._strExportKeyTag = value
        End Set
    End Property

    Property RecordCountTag() As String
        Get
            Return Me._strRecordCountTag
        End Get
        Set(ByVal value As String)
            Me._strRecordCountTag = value
        End Set
    End Property

    Property RecordCountTable() As String
        Get
            Return Me._strRecordCountTable
        End Get
        Set(ByVal value As String)
            Me._strRecordCountTable = value
        End Set
    End Property

End Class
