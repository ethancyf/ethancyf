Namespace Component

    <Serializable()> _
    Public Class WSVoucherModel

#Region "Properties"

        Private _intVoucherClaimed As Integer
        Public Property VoucherClaimed() As Integer
            Get
                Return Me._intVoucherClaimed
            End Get
            Set(ByVal value As Integer)
                Me._intVoucherClaimed = value
            End Set
        End Property

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

        ' -----------------------------------------------------------------------------------------

        Private _intCoPaymentFee As Nullable(Of Integer)
        Public Property CoPaymentFee() As Nullable(Of Integer)
            Get
                Return Me._intCoPaymentFee
            End Get
            Set(ByVal value As Nullable(Of Integer))
                Me._intCoPaymentFee = value
            End Set
        End Property

        Private _udtReasonForVisitModelCollection As New ReasonForVisitModelCollection
        Public Property ReasonForVisitModelCollection() As ReasonForVisitModelCollection
            Get
                Return _udtReasonForVisitModelCollection
            End Get
            Set(ByVal value As ReasonForVisitModelCollection)
                _udtReasonForVisitModelCollection = value
            End Set
        End Property

        Public Function ProfCodeUnmatched() As Boolean
            For Each udtReasonForVisit1 As ReasonForVisitModel In _udtReasonForVisitModelCollection
                For Each udtReasonForVisit2 As ReasonForVisitModel In _udtReasonForVisitModelCollection
                    If Not udtReasonForVisit1.ProfCode = udtReasonForVisit2.ProfCode Then
                        Return True
                    End If
                Next
            Next
            Return False
        End Function

        Public Function PassPriorityCodeRangeCheck() As Boolean
            For Each udtReasonForVisit As ReasonForVisitModel In _udtReasonForVisitModelCollection
                If udtReasonForVisit.PriorityCode < 1 Or udtReasonForVisit.PriorityCode > _udtReasonForVisitModelCollection.Count Then
                    Return False
                End If
            Next
            Return True
        End Function

        Public Function PriorityCodeDistinct() As Boolean
            Dim count As Integer = 0
            For Each udtReasonForVisit1 As ReasonForVisitModel In _udtReasonForVisitModelCollection
                For Each udtReasonForVisit2 As ReasonForVisitModel In _udtReasonForVisitModelCollection
                    If udtReasonForVisit1.PriorityCode = udtReasonForVisit2.PriorityCode Then
                        count = count + 1
                    End If
                Next
            Next
            Return (count = _udtReasonForVisitModelCollection.Count)
        End Function

        Public Function ReasonForVisitedDuplicated() As Boolean
            Dim i As Integer = 0
            Dim isDuplicated As Boolean = False
            For Each udtReasonForVisit1 As ReasonForVisitModel In _udtReasonForVisitModelCollection
                i = 0
                For Each udtReasonForVisit2 As ReasonForVisitModel In _udtReasonForVisitModelCollection
                    If udtReasonForVisit1.ProfCode = udtReasonForVisit2.ProfCode And udtReasonForVisit1.L1Code = udtReasonForVisit2.L1Code Then
                        i += 1
                    End If
                Next
                If i > 1 Then
                    isDuplicated = True
                    Exit For
                End If
            Next
            Return isDuplicated
        End Function

        Public Function HasPrimaryReasonForVisit() As Boolean
            Return FoundReasonForVisitByPriorityCode("1")
        End Function

        Public Function HasSecondaryReasonForVisit() As Boolean
            Return (FoundReasonForVisitByPriorityCode("2") Or FoundReasonForVisitByPriorityCode("3") Or FoundReasonForVisitByPriorityCode("4"))
        End Function


        Public Function FoundReasonForVisitByPriorityCode(ByVal priorityCode As String) As Boolean
            Dim Found As Boolean = False
            For Each udtReasonForVisit As ReasonForVisitModel In _udtReasonForVisitModelCollection
                If udtReasonForVisit.PriorityCode = priorityCode Then
                    Found = True
                End If
            Next
            Return Found
        End Function


        'Private _intL1Code As Integer
        'Public Property L1Code() As Integer
        '    Get
        '        Return Me._intL1Code
        '    End Get
        '    Set(ByVal value As Integer)
        '        Me._intL1Code = value
        '    End Set
        'End Property

        'Private _strL1DescEng As String
        'Public Property L1DescEng() As String
        '    Get
        '        Return Me._strL1DescEng
        '    End Get
        '    Set(ByVal value As String)
        '        Me._strL1DescEng = value
        '    End Set
        'End Property

        'Private _intL2Code As Integer
        'Public Property L2Code() As Integer
        '    Get
        '        Return Me._intL2Code
        '    End Get
        '    Set(ByVal value As Integer)
        '        Me._intL2Code = value
        '    End Set
        'End Property

        'Private _strL2DescEng As String
        'Public Property L2DescEng() As String
        '    Get
        '        Return Me._strL2DescEng
        '    End Get
        '    Set(ByVal value As String)
        '        Me._strL2DescEng = value
        '    End Set
        'End Property

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End

        Private _blnVoucherClaimed_Received As Boolean = False
        Public Property VoucherClaimed_Received() As Boolean
            Get
                Return Me._blnVoucherClaimed_Received
            End Get
            Set(ByVal value As Boolean)
                Me._blnVoucherClaimed_Received = value
            End Set
        End Property

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

        ' -----------------------------------------------------------------------------------------

        Private _blnCoPaymentFee_Received As Boolean = False
        Public Property CoPaymentFee_Received() As Boolean
            Get
                Return Me._blnCoPaymentFee_Received
            End Get
            Set(ByVal value As Boolean)
                Me._blnCoPaymentFee_Received = value
            End Set
        End Property

        Private _blnReasonForVisitModelCollection_Received As Boolean = False
        Public Property ReasonForVisitModelCollection_Received() As Boolean
            Get
                Return Me._blnReasonForVisitModelCollection_Received
            End Get
            Set(ByVal value As Boolean)
                Me._blnReasonForVisitModelCollection_Received = value
            End Set
        End Property

        'Private _blnL1Code_Received As Boolean = False
        'Public Property L1Code_Received() As Boolean
        '    Get
        '        Return Me._blnL1Code_Received
        '    End Get
        '    Set(ByVal value As Boolean)
        '        Me._blnL1Code_Received = value
        '    End Set
        'End Property

        'Private _blnL1DescEng_Received As Boolean = False
        'Public Property L1DescEng_Received() As Boolean
        '    Get
        '        Return Me._blnL1DescEng_Received
        '    End Get
        '    Set(ByVal value As Boolean)
        '        Me._blnL1DescEng_Received = value
        '    End Set
        'End Property

        'Private _blnL2Code_Received As Boolean = False
        'Public Property L2Code_Received() As Boolean
        '    Get
        '        Return Me._blnL2Code_Received
        '    End Get
        '    Set(ByVal value As Boolean)
        '        Me._blnL2Code_Received = value
        '    End Set
        'End Property

        'Private _blnL2DescEng_Received As Boolean = False
        'Public Property L2DescEng_Received() As Boolean
        '    Get
        '        Return Me._blnL2DescEng_Received
        '    End Get
        '    Set(ByVal value As Boolean)
        '        Me._blnL2DescEng_Received = value
        '    End Set
        'End Property

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End

#End Region

#Region "Constructors"

        Public Sub New()

        End Sub

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

        ' -----------------------------------------------------------------------------------------

        Public Sub New(ByVal intVoucherClaimed As Integer, _
                        ByVal intCoPaymentFee As Integer, _
                        ByVal udtReasonForVisitModelCollection As ReasonForVisitModelCollection)

            _intVoucherClaimed = intVoucherClaimed
            _intCoPaymentFee = intCoPaymentFee
            _udtReasonForVisitModelCollection = udtReasonForVisitModelCollection

        End Sub

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End

#End Region

    End Class

End Namespace

