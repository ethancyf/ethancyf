' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

' -----------------------------------------------------------------------------------------

Namespace Component

    <Serializable()> _
    Public Class ReasonForVisitModel

#Region "Properties"

        Private _strProfCode As String
        Public Property ProfCode() As String
            Get
                Return Me._strProfCode
            End Get
            Set(ByVal value As String)
                Me._strProfCode = value
            End Set
        End Property

        Private _strPriorityCode As String
        Public Property PriorityCode() As String
            Get
                Return Me._strPriorityCode
            End Get
            Set(ByVal value As String)
                Me._strPriorityCode = value
            End Set
        End Property

        Private _intL1Code As Integer
        Public Property L1Code() As Integer
            Get
                Return Me._intL1Code
            End Get
            Set(ByVal value As Integer)
                Me._intL1Code = value
            End Set
        End Property

        Private _strL1DescEng As String
        Public Property L1DescEng() As String
            Get
                Return Me._strL1DescEng
            End Get
            Set(ByVal value As String)
                Me._strL1DescEng = value
            End Set
        End Property

        Private _intL2Code As Integer
        Public Property L2Code() As Integer
            Get
                Return Me._intL2Code
            End Get
            Set(ByVal value As Integer)
                Me._intL2Code = value
            End Set
        End Property

        Private _strL2DescEng As String
        Public Property L2DescEng() As String
            Get
                Return Me._strL2DescEng
            End Get
            Set(ByVal value As String)
                Me._strL2DescEng = value
            End Set
        End Property

        Private _blnProfCode_Received As Boolean = False
        Public Property ProfCode_Received() As Boolean
            Get
                Return Me._blnProfCode_Received
            End Get
            Set(ByVal value As Boolean)
                Me._blnProfCode_Received = value
            End Set
        End Property

        Private _blnPriorityCode_Received As Boolean = False
        Public Property PriorityCode_Received() As Boolean
            Get
                Return Me._blnPriorityCode_Received
            End Get
            Set(ByVal value As Boolean)
                Me._blnPriorityCode_Received = value
            End Set
        End Property

        Private _blnL1Code_Received As Boolean = False
        Public Property L1Code_Received() As Boolean
            Get
                Return Me._blnL1Code_Received
            End Get
            Set(ByVal value As Boolean)
                Me._blnL1Code_Received = value
            End Set
        End Property

        Private _blnL1DescEng_Received As Boolean = False
        Public Property L1DescEng_Received() As Boolean
            Get
                Return Me._blnL1DescEng_Received
            End Get
            Set(ByVal value As Boolean)
                Me._blnL1DescEng_Received = value
            End Set
        End Property

        Private _blnL2Code_Received As Boolean = False
        Public Property L2Code_Received() As Boolean
            Get
                Return Me._blnL2Code_Received
            End Get
            Set(ByVal value As Boolean)
                Me._blnL2Code_Received = value
            End Set
        End Property

        Private _blnL2DescEng_Received As Boolean = False
        Public Property L2DescEng_Received() As Boolean
            Get
                Return Me._blnL2DescEng_Received
            End Get
            Set(ByVal value As Boolean)
                Me._blnL2DescEng_Received = value
            End Set
        End Property

#End Region

#Region "Constructors"

        Public Sub New()

        End Sub

        Public Sub New(ByVal strProfCode As String, _
                        ByVal strPriorityCode As String, _
                        ByVal intL1Code As Integer, _
                        ByVal strL1DescEng As String, _
                        ByVal intL2Code As Integer, _
                        ByVal strL2DescEng As String)

            _strProfCode = strProfCode
            _strPriorityCode = strPriorityCode
            _intL1Code = intL1Code
            _strL1DescEng = strL1DescEng
            _intL2Code = intL2Code
            _strL2DescEng = strL2DescEng

        End Sub

#End Region

    End Class

End Namespace

' CRE11-024-02 HCVS Pilot Extension Part 2 [End]
