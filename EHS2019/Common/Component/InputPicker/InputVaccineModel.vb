Namespace Component.InputPicker

#Region "Class: InputVaccineModel"
    Public Class InputVaccineModel
        Private _strSchemeCode As String
        Private _strSchemeSeq As String
        Private _strSubsidizeCode As String
        Private _strSubsidizeItemCode As String
        Private _strAvailableItemCode As String

        Private _intDisplaySeq As Integer
        Private _strDisplayCodeForClaim As String

#Region "Property"
        Public Property SchemeCode() As String
            Get
                Return _strSchemeCode
            End Get
            Set(ByVal Value As String)
                _strSchemeCode = Value
            End Set
        End Property

        Public Property SchemeSeq() As String
            Get
                Return _strSchemeSeq
            End Get
            Set(ByVal Value As String)
                _strSchemeSeq = Value
            End Set
        End Property

        Public Property SubsidizeCode() As String
            Get
                Return _strSubsidizeCode
            End Get
            Set(ByVal Value As String)
                _strSubsidizeCode = Value
            End Set
        End Property

        Public Property SubsidizeItemCode() As String
            Get
                Return _strSubsidizeItemCode
            End Get
            Set(ByVal Value As String)
                _strSubsidizeItemCode = Value
            End Set
        End Property

        Public Property AvailableItemCode() As String
            Get
                Return _strAvailableItemCode
            End Get
            Set(ByVal Value As String)
                _strAvailableItemCode = Value
            End Set
        End Property

        Public Property DisplaySeq() As Integer
            Get
                Return _intDisplaySeq
            End Get
            Set(ByVal Value As Integer)
                _intDisplaySeq = Value
            End Set
        End Property

        Public Property DisplayCodeForClaim() As String
            Get
                Return _strDisplayCodeForClaim
            End Get
            Set(ByVal Value As String)
                _strDisplayCodeForClaim = Value
            End Set
        End Property

#End Region

#Region "Constructor"

        Public Sub New()
            _strSchemeCode = String.Empty
            _strSchemeSeq = String.Empty
            _strSubsidizeCode = String.Empty
            _strSubsidizeItemCode = String.Empty
            _strAvailableItemCode = String.Empty
            _intDisplaySeq = 0
            _strDisplayCodeForClaim = String.Empty
        End Sub

        Public Sub New(ByVal strSchemeCode As String, ByVal strSchemeSeq As String, ByVal strSubsidizeCode As String, _
                       ByVal strSubsidizeItemCode As String, ByVal strAvailableItemCode As String, _
                       ByVal intDisplaySeq As Integer, ByVal strDisplayCodeForClaim As String)
            _strSchemeCode = strSchemeCode
            _strSchemeSeq = strSchemeSeq
            _strSubsidizeCode = strSubsidizeCode
            _strSubsidizeItemCode = strSubsidizeItemCode
            _strAvailableItemCode = strAvailableItemCode
            _intDisplaySeq = intDisplaySeq
            _strDisplayCodeForClaim = strDisplayCodeForClaim
        End Sub

#End Region

    End Class
#End Region

End Namespace

