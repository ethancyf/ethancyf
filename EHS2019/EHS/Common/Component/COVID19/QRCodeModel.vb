Namespace Component.COVID19

#Region "Class: QRCodeModel"
    Public Class QRCodeModel

        'String
        Private _strQRCodeVersion As String
        Private _strKeyVersion As String
        Private _strSignature As String

#Region "Property"

        Public Property QRCodeVersion() As String
            Get
                Return _strQRCodeVersion
            End Get
            Set(ByVal Value As String)
                _strQRCodeVersion = Value
            End Set
        End Property

        Public Property KeyVersion() As String
            Get
                Return _strKeyVersion
            End Get
            Set(ByVal Value As String)
                _strKeyVersion = Value
            End Set
        End Property

        Public Property Signature() As String
            Get
                Return _strSignature
            End Get
            Set(ByVal Value As String)
                _strSignature = Value
            End Set
        End Property

#End Region

#Region "Constructor"

        Public Sub New()
            _strQRCodeVersion = String.Empty
            _strKeyVersion = String.Empty
            _strSignature = String.Empty

        End Sub

#End Region

    End Class

#End Region

End Namespace

