Imports Common.Component.DocType

Public Class DocumentInfo

    Private _strDocCode As String
    Private _strDocName As String
    Private _strDocNameChi As String
    Private _strDocNameCN As String
    Private _strDocDisplayCode As String
    Private _isEnable As Boolean

    Public Property DocCode() As String
        Get
            Return Me._strDocCode
        End Get
        Set(ByVal value As String)
            Me._strDocCode = value
        End Set
    End Property

    Public Property DocName() As String
        Get
            Return Me._strDocName
        End Get
        Set(ByVal value As String)
            Me._strDocName = value
        End Set
    End Property

    Public Property DocNameChi() As String
        Get
            Return Me._strDocNameChi
        End Get
        Set(ByVal value As String)
            Me._strDocNameChi = value
        End Set
    End Property

    Public Property DocNameCN() As String
        Get
            Return Me._strDocNameCN
        End Get
        Set(ByVal value As String)
            Me._strDocNameCN = value
        End Set
    End Property

    Public Property DocDisplayCode() As String
        Get
            Return Me._strDocDisplayCode
        End Get
        Set(ByVal value As String)
            Me._strDocDisplayCode = value
        End Set
    End Property

    Public Property IsEnable() As Boolean
        Get
            Return Me._isEnable
        End Get
        Set(ByVal value As Boolean)
            Me._isEnable = value
        End Set
    End Property

    Public Sub New(ByVal udtDocTypeModel As DocTypeModel)
        Me._strDocCode = udtDocTypeModel.DocCode
        Me._strDocName = udtDocTypeModel.DocName
        Me._strDocNameChi = udtDocTypeModel.DocNameChi
        Me._strDocNameCN = udtDocTypeModel.DocNameCN
        Me._strDocDisplayCode = udtDocTypeModel.DocDisplayCode
        Me._isEnable = False
    End Sub

End Class

