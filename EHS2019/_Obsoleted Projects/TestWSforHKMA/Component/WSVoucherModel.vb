

Namespace Component

    <Serializable()> _
    Public Class WSVoucherModel

#Region "Properties"

        Private _intVoucherClaimed As String
        Public Property VoucherClaimed() As String
            Get
                Return Me._intVoucherClaimed
            End Get
            Set(ByVal value As String)
                Me._intVoucherClaimed = value
            End Set
        End Property

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

        ' -----------------------------------------------------------------------------------------

        Private _intCoPaymentFee As String
        Public Property CoPaymentFee() As String
            Get
                Return Me._intCoPaymentFee
            End Get
            Set(ByVal value As String)
                Me._intCoPaymentFee = value
            End Set
        End Property

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        'Primary

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

        Private _intL2Code As String
        Public Property L2Code() As String
            Get
                Return Me._intL2Code
            End Get
            Set(ByVal value As String)
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



        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

        ' -----------------------------------------------------------------------------------------

        'S1

        Private _strProfCode_S1 As String
        Public Property ProfCode_S1() As String
            Get
                Return Me._strProfCode_S1
            End Get
            Set(ByVal value As String)
                Me._strProfCode_S1 = value
            End Set
        End Property

        Private _strPriorityCode_S1 As String
        Public Property PriorityCode_S1() As String
            Get
                Return Me._strPriorityCode_S1
            End Get
            Set(ByVal value As String)
                Me._strPriorityCode_S1 = value
            End Set
        End Property

        Private _intL1Code_S1 As Integer
        Public Property L1Code_S1() As Integer
            Get
                Return Me._intL1Code_S1
            End Get
            Set(ByVal value As Integer)
                Me._intL1Code_S1 = value
            End Set
        End Property

        Private _strL1DescEng_S1 As String
        Public Property L1DescEng_S1() As String
            Get
                Return Me._strL1DescEng_S1
            End Get
            Set(ByVal value As String)
                Me._strL1DescEng_S1 = value
            End Set
        End Property

        Private _intL2Code_S1 As String
        Public Property L2Code_S1() As String
            Get
                Return Me._intL2Code_S1
            End Get
            Set(ByVal value As String)
                Me._intL2Code_S1 = value
            End Set
        End Property

        Private _strL2DescEng_S1 As String
        Public Property L2DescEng_S1() As String
            Get
                Return Me._strL2DescEng_S1
            End Get
            Set(ByVal value As String)
                Me._strL2DescEng_S1 = value
            End Set
        End Property

        'S2

        Private _strProfCode_S2 As String
        Public Property ProfCode_S2() As String
            Get
                Return Me._strProfCode_S2
            End Get
            Set(ByVal value As String)
                Me._strProfCode_S2 = value
            End Set
        End Property

        Private _strPriorityCode_S2 As String
        Public Property PriorityCode_S2() As String
            Get
                Return Me._strPriorityCode_S2
            End Get
            Set(ByVal value As String)
                Me._strPriorityCode_S2 = value
            End Set
        End Property

        Private _intL1Code_S2 As Integer
        Public Property L1Code_S2() As Integer
            Get
                Return Me._intL1Code_S2
            End Get
            Set(ByVal value As Integer)
                Me._intL1Code_S2 = value
            End Set
        End Property

        Private _strL1DescEng_S2 As String
        Public Property L1DescEng_S2() As String
            Get
                Return Me._strL1DescEng_S2
            End Get
            Set(ByVal value As String)
                Me._strL1DescEng_S2 = value
            End Set
        End Property

        Private _intL2Code_S2 As String
        Public Property L2Code_S2() As String
            Get
                Return Me._intL2Code_S2
            End Get
            Set(ByVal value As String)
                Me._intL2Code_S2 = value
            End Set
        End Property

        Private _strL2DescEng_S2 As String
        Public Property L2DescEng_S2() As String
            Get
                Return Me._strL2DescEng_S2
            End Get
            Set(ByVal value As String)
                Me._strL2DescEng_S2 = value
            End Set
        End Property

        'S3

        Private _strProfCode_S3 As String
        Public Property ProfCode_S3() As String
            Get
                Return Me._strProfCode_S3
            End Get
            Set(ByVal value As String)
                Me._strProfCode_S3 = value
            End Set
        End Property

        Private _strPriorityCode_S3 As String
        Public Property PriorityCode_S3() As String
            Get
                Return Me._strPriorityCode_S3
            End Get
            Set(ByVal value As String)
                Me._strPriorityCode_S3 = value
            End Set
        End Property

        Private _intL1Code_S3 As Integer
        Public Property L1Code_S3() As Integer
            Get
                Return Me._intL1Code_S3
            End Get
            Set(ByVal value As Integer)
                Me._intL1Code_S3 = value
            End Set
        End Property

        Private _strL1DescEng_S3 As String
        Public Property L1DescEng_S3() As String
            Get
                Return Me._strL1DescEng_S3
            End Get
            Set(ByVal value As String)
                Me._strL1DescEng_S3 = value
            End Set
        End Property

        Private _intL2Code_S3 As String
        Public Property L2Code_S3() As String
            Get
                Return Me._intL2Code_S3
            End Get
            Set(ByVal value As String)
                Me._intL2Code_S3 = value
            End Set
        End Property

        Private _strL2DescEng_S3 As String
        Public Property L2DescEng_S3() As String
            Get
                Return Me._strL2DescEng_S3
            End Get
            Set(ByVal value As String)
                Me._strL2DescEng_S3 = value
            End Set
        End Property

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        Private _blVoucherClaimed_Received As Boolean = False
        Public Property VoucherClaimed_Included() As Boolean
            Get
                Return _blVoucherClaimed_Received
            End Get
            Set(ByVal value As Boolean)
                _blVoucherClaimed_Received = value
            End Set
        End Property

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

        ' -----------------------------------------------------------------------------------------

        Private _blCoPaymentFee_Received As Boolean = False
        Public Property CoPaymentFee_Included() As Boolean
            Get
                Return _blCoPaymentFee_Received
            End Get
            Set(ByVal value As Boolean)
                _blCoPaymentFee_Received = value
            End Set
        End Property

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        'Primary

        Private _blReasonForVisit_Received As Boolean = False
        Public Property ReasonForVisit_Included() As Boolean
            Get
                Return _blReasonForVisit_Received
            End Get
            Set(ByVal value As Boolean)
                _blReasonForVisit_Received = value
            End Set
        End Property


        Private _blProfCode_Received As Boolean = False
        Public Property ProfCode_Included() As Boolean
            Get
                Return _blProfCode_Received
            End Get
            Set(ByVal value As Boolean)
                _blProfCode_Received = value
            End Set
        End Property

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

        ' -----------------------------------------------------------------------------------------

        Private _blPriorityCode_Received As Boolean = False
        Public Property PriorityCode_Included() As Boolean
            Get
                Return _blPriorityCode_Received
            End Get
            Set(ByVal value As Boolean)
                _blPriorityCode_Received = value
            End Set
        End Property

        Private _blL1Code_Received As Boolean = False
        Public Property L1Code_Included() As Boolean
            Get
                Return _blL1Code_Received
            End Get
            Set(ByVal value As Boolean)
                _blL1Code_Received = value
            End Set
        End Property

        Private _blL1DescEng_Received As Boolean = False
        Public Property L1DescEng_Included() As Boolean
            Get
                Return _blL1DescEng_Received
            End Get
            Set(ByVal value As Boolean)
                _blL1DescEng_Received = value
            End Set
        End Property

        Private _blL2Code_Received As Boolean = False
        Public Property L2Code_Included() As Boolean
            Get
                Return _blL2Code_Received
            End Get
            Set(ByVal value As Boolean)
                _blL2Code_Received = value
            End Set
        End Property

        Private _blL2DescEng_Received As Boolean = False
        Public Property L2DescEng_Included() As Boolean
            Get
                Return _blL2DescEng_Received
            End Get
            Set(ByVal value As Boolean)
                _blL2DescEng_Received = value
            End Set
        End Property

        'S1

        Private _blReasonForVisit_S1_Received As Boolean = False
        Public Property ReasonForVisit_S1_Included() As Boolean
            Get
                Return _blReasonForVisit_S1_Received
            End Get
            Set(ByVal value As Boolean)
                _blReasonForVisit_S1_Received = value
            End Set
        End Property

        Private _blProfCode_S1_Received As Boolean = False
        Public Property ProfCode_S1_Included() As Boolean
            Get
                Return _blProfCode_S1_Received
            End Get
            Set(ByVal value As Boolean)
                _blProfCode_S1_Received = value
            End Set
        End Property

        Private _blPriorityCode_S1_Received As Boolean = False
        Public Property PriorityCode_S1_Included() As Boolean
            Get
                Return _blPriorityCode_S1_Received
            End Get
            Set(ByVal value As Boolean)
                _blPriorityCode_S1_Received = value
            End Set
        End Property

        Private _blL1Code_S1_Received As Boolean = False
        Public Property L1Code_S1_Included() As Boolean
            Get
                Return _blL1Code_S1_Received
            End Get
            Set(ByVal value As Boolean)
                _blL1Code_S1_Received = value
            End Set
        End Property

        Private _blL1DescEng_S1_Received As Boolean = False
        Public Property L1DescEng_S1_Included() As Boolean
            Get
                Return _blL1DescEng_S1_Received
            End Get
            Set(ByVal value As Boolean)
                _blL1DescEng_S1_Received = value
            End Set
        End Property

        Private _blL2Code_S1_Received As Boolean = False
        Public Property L2Code_S1_Included() As Boolean
            Get
                Return _blL2Code_S1_Received
            End Get
            Set(ByVal value As Boolean)
                _blL2Code_S1_Received = value
            End Set
        End Property

        Private _blL2DescEng_S1_Received As Boolean = False
        Public Property L2DescEng_S1_Included() As Boolean
            Get
                Return _blL2DescEng_S1_Received
            End Get
            Set(ByVal value As Boolean)
                _blL2DescEng_S1_Received = value
            End Set
        End Property

        'S2

        Private _blReasonForVisit_S2_Received As Boolean = False
        Public Property ReasonForVisit_S2_Included() As Boolean
            Get
                Return _blReasonForVisit_S2_Received
            End Get
            Set(ByVal value As Boolean)
                _blReasonForVisit_S2_Received = value
            End Set
        End Property

        Private _blProfCode_S2_Received As Boolean = False
        Public Property ProfCode_S2_Included() As Boolean
            Get
                Return _blProfCode_S2_Received
            End Get
            Set(ByVal value As Boolean)
                _blProfCode_S2_Received = value
            End Set
        End Property

        Private _blPriorityCode_S2_Received As Boolean = False
        Public Property PriorityCode_S2_Included() As Boolean
            Get
                Return _blPriorityCode_S2_Received
            End Get
            Set(ByVal value As Boolean)
                _blPriorityCode_S2_Received = value
            End Set
        End Property

        Private _blL1Code_S2_Received As Boolean = False
        Public Property L1Code_S2_Included() As Boolean
            Get
                Return _blL1Code_S2_Received
            End Get
            Set(ByVal value As Boolean)
                _blL1Code_S2_Received = value
            End Set
        End Property

        Private _blL1DescEng_S2_Received As Boolean = False
        Public Property L1DescEng_S2_Included() As Boolean
            Get
                Return _blL1DescEng_S2_Received
            End Get
            Set(ByVal value As Boolean)
                _blL1DescEng_S2_Received = value
            End Set
        End Property

        Private _blL2Code_S2_Received As Boolean = False
        Public Property L2Code_S2_Included() As Boolean
            Get
                Return _blL2Code_S2_Received
            End Get
            Set(ByVal value As Boolean)
                _blL2Code_S2_Received = value
            End Set
        End Property

        Private _blL2DescEng_S2_Received As Boolean = False
        Public Property L2DescEng_S2_Included() As Boolean
            Get
                Return _blL2DescEng_S2_Received
            End Get
            Set(ByVal value As Boolean)
                _blL2DescEng_S2_Received = value
            End Set
        End Property

        'S3

        Private _blReasonForVisit_S3_Received As Boolean = False
        Public Property ReasonForVisit_S3_Included() As Boolean
            Get
                Return _blReasonForVisit_S3_Received
            End Get
            Set(ByVal value As Boolean)
                _blReasonForVisit_S3_Received = value
            End Set
        End Property

        Private _blProfCode_S3_Received As Boolean = False
        Public Property ProfCode_S3_Included() As Boolean
            Get
                Return _blProfCode_S3_Received
            End Get
            Set(ByVal value As Boolean)
                _blProfCode_S3_Received = value
            End Set
        End Property

        Private _blPriorityCode_S3_Received As Boolean = False
        Public Property PriorityCode_S3_Included() As Boolean
            Get
                Return _blPriorityCode_S3_Received
            End Get
            Set(ByVal value As Boolean)
                _blPriorityCode_S3_Received = value
            End Set
        End Property

        Private _blL1Code_S3_Received As Boolean = False
        Public Property L1Code_S3_Included() As Boolean
            Get
                Return _blL1Code_S3_Received
            End Get
            Set(ByVal value As Boolean)
                _blL1Code_S3_Received = value
            End Set
        End Property

        Private _blL1DescEng_S3_Received As Boolean = False
        Public Property L1DescEng_S3_Included() As Boolean
            Get
                Return _blL1DescEng_S3_Received
            End Get
            Set(ByVal value As Boolean)
                _blL1DescEng_S3_Received = value
            End Set
        End Property

        Private _blL2Code_S3_Received As Boolean = False
        Public Property L2Code_S3_Included() As Boolean
            Get
                Return _blL2Code_S3_Received
            End Get
            Set(ByVal value As Boolean)
                _blL2Code_S3_Received = value
            End Set
        End Property

        Private _blL2DescEng_S3_Received As Boolean = False
        Public Property L2DescEng_S3_Included() As Boolean
            Get
                Return _blL2DescEng_S3_Received
            End Get
            Set(ByVal value As Boolean)
                _blL2DescEng_S3_Received = value
            End Set
        End Property

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]
#End Region




#Region "Constructors"

        Public Sub New()

        End Sub

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

        ' -----------------------------------------------------------------------------------------

        Public Sub New(ByVal intVoucherClaimed As Integer, _
                        ByVal intCoPaymentFee As String, _
                        ByVal strProfCode As String, _
                        ByVal strPriorityCode As String, _
                        ByVal intL1Code As Integer, _
                        ByVal strL1DescEng As String, _
                        ByVal intL2Code As Integer, _
                        ByVal strL2DescEng As String, _
                        ByVal strProfCode_S1 As String, _
                        ByVal strPriorityCode_S1 As String, _
                        ByVal intL1Code_S1 As Integer, _
                        ByVal strL1DescEng_S1 As String, _
                        ByVal intL2Code_S1 As Integer, _
                        ByVal strL2DescEng_S1 As String, _
                        ByVal strProfCode_S2 As String, _
                        ByVal strPriorityCode_S2 As String, _
                        ByVal intL1Code_S2 As Integer, _
                        ByVal strL1DescEng_S2 As String, _
                        ByVal intL2Code_S2 As Integer, _
                        ByVal strL2DescEng_S2 As String, _
                        ByVal strProfCode_S3 As String, _
                        ByVal strPriorityCode_S3 As String, _
                        ByVal intL1Code_S3 As Integer, _
                        ByVal strL1DescEng_S3 As String, _
                        ByVal intL2Code_S3 As Integer, _
                        ByVal strL2DescEng_S3 As String)

            _intVoucherClaimed = intVoucherClaimed
            _intCoPaymentFee = intCoPaymentFee

            _strProfCode = strProfCode
            _strPriorityCode = strPriorityCode
            _intL1Code = intL1Code
            _strL1DescEng = strL1DescEng
            _intL2Code = intL2Code
            _strL2DescEng = strL2DescEng

            _strProfCode_S1 = strProfCode_S1
            _strPriorityCode_S1 = strPriorityCode_S1
            _intL1Code_S1 = intL1Code_S1
            _strL1DescEng_S1 = strL1DescEng_S1
            _intL2Code_S1 = intL2Code_S1
            _strL2DescEng_S1 = strL2DescEng_S1

            _strProfCode_S2 = strProfCode_S2
            _strPriorityCode_S2 = strPriorityCode_S2
            _intL1Code_S2 = intL1Code_S2
            _strL1DescEng_S2 = strL1DescEng_S2
            _intL2Code_S2 = intL2Code_S2
            _strL2DescEng_S2 = strL2DescEng_S2

            _strProfCode_S3 = strProfCode_S3
            _strPriorityCode_S3 = strPriorityCode_S3
            _intL1Code_S3 = intL1Code_S3
            _strL1DescEng_S3 = strL1DescEng_S3
            _intL2Code_S3 = intL2Code_S3
            _strL2DescEng_S3 = strL2DescEng_S3

        End Sub

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

#End Region









    End Class

End Namespace
