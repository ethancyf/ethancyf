

Namespace Component.EHSAccount

    <Serializable()> Public Class SubsidizeWriteOffModel

#Region "Private Member"

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        ' Add [PValue_TotalRefund], [PValue_SeasonRefund]
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

        Private _strDocCode As String
        Private _strDocID As String
        Private _dtmDOB As DateTime
        Private _strExactDOB As String
        Private _strSchemeCode As String
        Private _intSchemeSeq As Integer
        Private _strSubsidizeCode As String
        Private _intWriteOffUnit As Integer
        Private _dblWriteOffPerUnitValue As Double
        Private _dtmCreateDtm As DateTime
        Private _strCreateReason As String
        Private _byteTSMP As Byte()

        Public Const TSMPDataType As SqlDbType = SqlDbType.VarBinary
        Public Const DocCodeDataType As SqlDbType = SqlDbType.Char
        Public Const DocIDDataType As SqlDbType = SqlDbType.VarChar
        Public Const DOBDataType As SqlDbType = SqlDbType.DateTime
        Public Const ExactDOBDataType As SqlDbType = SqlDbType.Char
        Public Const SchemeCodeDataType As SqlDbType = SqlDbType.Char
        Public Const SchemeSeqDataType As SqlDbType = SqlDbType.SmallInt
        Public Const SubsidizeCodeDataType As SqlDbType = SqlDbType.Char
        Public Const WriteOffUnitDataType As SqlDbType = SqlDbType.Int
        Public Const WriteOffPerUnitValueDataType As SqlDbType = SqlDbType.Money
        Public Const CreateDtmDataType As SqlDbType = SqlDbType.DateTime
        Public Const CreateReasonDataType As SqlDbType = SqlDbType.VarChar

        Public Const DocCodeDataSize As Integer = 20
        Public Const DocIDDataSize As Integer = 20
        Public Const DOBDataSize As Integer = 8
        Public Const ExactDOBDataSize As Integer = 1
        Public Const SchemeCodeDataSize As Integer = 10
        Public Const SchemeSeqDataSize As Integer = 2
        Public Const SubsidizeCodeDataSize As Integer = 10
        Public Const WriteOffUnitDataSize As Integer = 4
        Public Const WriteOffPerUnitValueDataSize As Integer = 8        
        Public Const CreateDtmDataSize As Integer = 8
        Public Const CreateReasonDataSize As Integer = 2
        Public Const TSMPDataSize As Integer = 8

        'Processing values
        Private _intPValueCeiling As Nullable(Of Integer)
        Private _intPValueTotalEntitlement As Integer
        Private _intPValueSeasonEntitlement As Integer
        Private _intPValueTotalUsed As Integer
        Private _intPValueSeasonUsed As Integer
        Private _intPValueAvailable As Integer

        Public Const PValueCeilingDataType As SqlDbType = SqlDbType.Int
        Public Const PValueTotalEntitlementDataType As SqlDbType = SqlDbType.Int
        Public Const PValueSeasonEntitlementDataType As SqlDbType = SqlDbType.Int
        Public Const PValueTotalUsedDataType As SqlDbType = SqlDbType.Int
        Public Const PValueSeasonUsedDataType As SqlDbType = SqlDbType.Int
        Public Const PValueAvailableDataType As SqlDbType = SqlDbType.Int

        Public Const PValueCeilingDataSize As Integer = 8
        Public Const PValueTotalEntitlementDataSize As Integer = 8
        Public Const PValueSeasonEntitlementDataSize As Integer = 8
        Public Const PValueTotalUsedDataSize As Integer = 8
        Public Const PValueSeasonUsedDataSize As Integer = 8
        Public Const PValueAvailableDataSize As Integer = 8

        ' Refund
        Private _intPValueTotalRefund As Integer
        Private _intPValueSeasonRefund As Integer

        Public Const PValueTotalRefundDataType As SqlDbType = SqlDbType.Int
        Public Const PValueSeasonRefundDataType As SqlDbType = SqlDbType.Int

        Public Const PValueTotalRefundDataSize As Integer = 8
        Public Const PValueSeasonRefundDataSize As Integer = 8
        
#End Region

#Region "Property"

        Public Property DocCode() As String
            Get
                Return Me._strDocCode
            End Get
            Set(ByVal value As String)
                Me._strDocCode = value
            End Set
        End Property


        Public Property DocID() As String
            Get
                Return Me._strDocID
            End Get
            Set(ByVal value As String)
                Me._strDocID = value
            End Set
        End Property

        Public Property DOB() As DateTime
            Get
                Return Me._dtmDOB
            End Get
            Set(ByVal value As DateTime)
                Me._dtmDOB = value
            End Set
        End Property

        Public Property ExactDOB() As String
            Get
                Return Me._strExactDOB
            End Get
            Set(ByVal value As String)
                Me._strExactDOB = value
            End Set
        End Property

        Public Property SchemeCode() As String
            Get
                Return Me._strSchemeCode
            End Get
            Set(ByVal value As String)
                Me._strSchemeCode = value
            End Set
        End Property

        Public Property SubsidizeCode() As String
            Get
                Return Me._strSubsidizeCode
            End Get
            Set(ByVal value As String)
                Me._strSubsidizeCode = value
            End Set
        End Property

        Public Property SchemeSeq() As Integer
            Get
                Return Me._intSchemeSeq
            End Get
            Set(ByVal value As Integer)
                Me._intSchemeSeq = value
            End Set
        End Property
        Public Property WriteOffUnit() As Integer
            Get
                Return Me._intWriteOffUnit
            End Get
            Set(ByVal value As Integer)
                Me._intWriteOffUnit = value
            End Set
        End Property

        Public Property WriteOffPerUnitValue() As Double
            Get
                Return Me._dblWriteOffPerUnitValue
            End Get
            Set(ByVal value As Double)
                Me._dblWriteOffPerUnitValue = value
            End Set
        End Property


        Public Property CreateDtm() As DateTime
            Get
                Return Me._dtmCreateDtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmCreateDtm = value
            End Set
        End Property

        Public Property CreateReason() As String
            Get
                Return Me._strCreateReason
            End Get
            Set(ByVal value As String)
                Me._strCreateReason = value
            End Set
        End Property

        Public Property TSMP() As Byte()
            Get
                Return Me._byteTSMP
            End Get
            Set(ByVal value As Byte())
                Me._byteTSMP = value
            End Set
        End Property

        'Processing Value
        Public Property PValueCeiling() As Nullable(Of Integer)
            Get
                Return Me._intPValueCeiling
            End Get
            Set(ByVal value As Nullable(Of Integer))
                Me._intPValueCeiling = value
            End Set
        End Property

        Public Property PValueTotalEntitlement() As Integer
            Get
                Return Me._intPValueTotalEntitlement
            End Get
            Set(ByVal value As Integer)
                Me._intPValueTotalEntitlement = value
            End Set
        End Property

        Public Property PValueSeasonEntitlement() As Integer
            Get
                Return Me._intPValueSeasonEntitlement
            End Get
            Set(ByVal value As Integer)
                Me._intPValueSeasonEntitlement = value
            End Set
        End Property

        Public Property PValueTotalUsed() As Integer
            Get
                Return Me._intPValueTotalUsed
            End Get
            Set(ByVal value As Integer)
                Me._intPValueTotalUsed = value
            End Set
        End Property

        Public Property PValueSeasonUsed() As Integer
            Get
                Return Me._intPValueSeasonUsed
            End Get
            Set(ByVal value As Integer)
                Me._intPValueSeasonUsed = value
            End Set
        End Property

        Public Property PValueAvailable() As Integer
            Get
                Return Me._intPValueAvailable
            End Get
            Set(ByVal value As Integer)
                Me._intPValueAvailable = value
            End Set
        End Property

        Public Property PValueTotalRefund() As Integer
            Get
                Return Me._intPValueTotalRefund
            End Get
            Set(ByVal value As Integer)
                Me._intPValueTotalRefund = value
            End Set
        End Property

        Public Property PValueSeasonRefund() As Integer
            Get
                Return Me._intPValueSeasonRefund
            End Get
            Set(ByVal value As Integer)
                Me._intPValueSeasonRefund = value
            End Set
        End Property

#End Region


        Public Sub New(ByVal udtSubsidizeWriteOffModel As SubsidizeWriteOffModel)
            _strDocCode = udtSubsidizeWriteOffModel.DocCode
            _strDocID = udtSubsidizeWriteOffModel.DocID
            _dtmDOB = udtSubsidizeWriteOffModel.DOB
            _strExactDOB = udtSubsidizeWriteOffModel.ExactDOB
            _strSchemeCode = udtSubsidizeWriteOffModel.SchemeCode
            _intSchemeSeq = udtSubsidizeWriteOffModel.SchemeSeq
            _strSubsidizeCode = udtSubsidizeWriteOffModel.SubsidizeCode
            _intWriteOffUnit = udtSubsidizeWriteOffModel.WriteOffUnit
            _dblWriteOffPerUnitValue = udtSubsidizeWriteOffModel.WriteOffPerUnitValue
            _dtmCreateDtm = udtSubsidizeWriteOffModel.CreateDtm
            _strCreateReason = udtSubsidizeWriteOffModel.CreateReason
            _byteTSMP = udtSubsidizeWriteOffModel.TSMP


            _intPValueCeiling = udtSubsidizeWriteOffModel.PValueCeiling
            _intPValueTotalEntitlement = udtSubsidizeWriteOffModel.PValueTotalEntitlement
            _intPValueSeasonEntitlement = udtSubsidizeWriteOffModel.PValueSeasonEntitlement
            _intPValueTotalUsed = udtSubsidizeWriteOffModel.PValueTotalUsed
            _intPValueSeasonUsed = udtSubsidizeWriteOffModel.PValueSeasonUsed
            _intPValueAvailable = udtSubsidizeWriteOffModel.PValueAvailable

            'Refund
            _intPValueTotalRefund = udtSubsidizeWriteOffModel.PValueTotalRefund
            _intPValueSeasonRefund = udtSubsidizeWriteOffModel.PValueSeasonRefund
        End Sub

        Public Sub New(ByVal strDocCode As String, ByVal strDocID As String, ByVal dtmDOB As DateTime, ByVal strExactDOB As String, _
                       ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String, ByVal intWriteOffUnit As Integer, _
                       ByVal dblWriteOffPerUnitValue As Double, ByVal intPValueCeiling As Nullable(Of Integer), ByVal intPValueTotalEntitlement As Integer, _
                       ByVal intPValueSeasonEntitlement As Integer, ByVal intPValueTotalUsed As Integer, ByVal intPValueSeasonUsed As Integer, _
                       ByVal intPValueAvailable As Integer, ByVal dtmCreateDtm As DateTime, _
                       ByVal strCreateReason As String, ByVal byteTSMP As Byte(), _
                       ByVal intPValueTotalRefund As Integer, ByVal intPValueSeasonRefund As Integer)

            _strDocCode = strDocCode
            _strDocID = strDocID
            _dtmDOB = dtmDOB
            _strExactDOB = strExactDOB
            _strSchemeCode = strSchemeCode
            _intSchemeSeq = intSchemeSeq
            _strSubsidizeCode = strSubsidizeCode
            _intWriteOffUnit = intWriteOffUnit
            _dblWriteOffPerUnitValue = dblWriteOffPerUnitValue
            _dtmCreateDtm = dtmCreateDtm
            _strCreateReason = strCreateReason
            _byteTSMP = byteTSMP

            _intPValueCeiling = intPValueCeiling
            _intPValueTotalEntitlement = intPValueTotalEntitlement
            _intPValueSeasonEntitlement = intPValueSeasonEntitlement
            _intPValueTotalUsed = intPValueTotalUsed
            _intPValueSeasonUsed = intPValueSeasonUsed
            _intPValueAvailable = intPValueAvailable

            'Refund
            _intPValueTotalRefund = intPValueTotalRefund
            _intPValueSeasonRefund = intPValueSeasonRefund
        End Sub

        Public Sub New(ByVal strDocCode As String, ByVal strDocID As String, ByVal dtmDOB As DateTime, ByVal strExactDOB As String, _
                       ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String, ByVal intWriteOffUnit As Integer, _
                       ByVal dblWriteOffPerUnitValue As Double, ByVal intPValueCeiling As Nullable(Of Integer), ByVal intPValueTotalEntitlement As Integer, _
                       ByVal intPValueSeasonEntitlement As Integer, ByVal intPValueTotalUsed As Integer, ByVal intPValueSeasonUsed As Integer, _
                       ByVal intPValueAvailable As Integer, ByVal dtmCreateDate As DateTime, ByVal strCreateReason As String, _
                       ByVal intPValueTotalRefund As Integer, ByVal intPValueSeasonRefund As Integer)

            _strDocCode = strDocCode
            _strDocID = strDocID
            _dtmDOB = dtmDOB
            _strExactDOB = strExactDOB
            _strSchemeCode = strSchemeCode
            _intSchemeSeq = intSchemeSeq
            _strSubsidizeCode = strSubsidizeCode
            _intWriteOffUnit = intWriteOffUnit
            _dblWriteOffPerUnitValue = dblWriteOffPerUnitValue
            _dtmCreateDtm = dtmCreateDate
            _strCreateReason = strCreateReason

            _intPValueCeiling = intPValueCeiling
            _intPValueTotalEntitlement = intPValueTotalEntitlement
            _intPValueSeasonEntitlement = intPValueSeasonEntitlement
            _intPValueTotalUsed = intPValueTotalUsed
            _intPValueSeasonUsed = intPValueSeasonUsed
            _intPValueAvailable = intPValueAvailable

            'Refund
            _intPValueTotalRefund = intPValueTotalRefund
            _intPValueSeasonRefund = intPValueSeasonRefund
        End Sub

    End Class

End Namespace