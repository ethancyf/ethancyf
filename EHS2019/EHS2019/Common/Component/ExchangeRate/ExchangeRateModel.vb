' [CRE13-019-02] Extend HCVS to China

Imports Common.Component
Imports Common.Format

Namespace Component.ExchangeRate

    <Serializable()> Public Class ExchangeRateModel

#Region "DB Table Schema"
        'DB Table [ExchangeRate]"
        '--------------------------------------------------
        '[ExchangeRate_ID] [varchar](8) NOT NULL,
        '[Effective_Date] [datetime] NOT NULL,
        '[ExchangeRateValue] [decimal](5) NOT NULL,
        '[Record_Status] [char](1) NOT NULL,
        '[Create_Dtm] [datetime] NOT NULL,
        '[Create_By] [varchar](20) NOT NULL,
        '[Update_Dtm] [datetime] NOT NULL,
        '[Update_By] [varchar](20) NOT NULL,
        '[TSMP] [timestamp] NOT NULL

        'PRIMARY KEY ([ExchangeRate_ID])
        '==================================================

        'DB Table [ExchangeRateStaging]"
        '--------------------------------------------------
        '[ExchangeRateStaging_ID] [varchar](9) NOT NULL,
        '[Effective_Date] [datetime] NOT NULL,
        '[ExchangeRateValue] [decimal](5) NOT NULL,
        '[Record_Status] [char](1) NOT NULL,
        '[Record_Type] [char](1) NOT NULL,
        '[ExchangeRate_ID] [varchar](8) NULL,
        '[Create_Dtm] [datetime] NOT NULL,
        '[Create_By] [varchar](20) NOT NULL,
        '[Update_Dtm] [datetime] NOT NULL,
        '[Update_By] [varchar](20) NOT NULL,
        '[Reject_Dtm] [datetime] NULL,
        '[Reject_By] [varchar](20) NULL,
        '[Delete_Dtm] [datetime] NULL,
        '[Delete_By] [varchar](20) NULL,
        '[TSMP] [timestamp] NOT NULL

        'PRIMARY KEY ([ExchangeRateStaging_ID])
        '==================================================
#End Region

#Region "DB Data Value of the Field"
        'Exchange Rate - Record_Status
        Public Const ER_RECORD_STATUS_A As Char = "A"     'Approved
        Public Const ER_RECORD_STATUS_D As Char = "D"     'Deleted
        'Exchange Rate Staging - Record_Status
        Public Const ERS_RECORD_STATUS_P As Char = "P"     'Pending Approval
        Public Const ERS_RECORD_STATUS_A As Char = "A"     'Approved
        Public Const ERS_RECORD_STATUS_R As Char = "R"     'Rejected
        Public Const ERS_RECORD_STATUS_D As Char = "D"     'Deleted
        'Exchange Rate Staging - Action
        Public Const ERS_ACTION_I As Char = "I"     'Insert
        Public Const ERS_ACTION_D As Char = "D"     'Delete
        'Exchange Rate - Info Type
        Public Const ER_INFO_TYPE_N As Char = "N"     'Next Exchange Rate
        Public Const ER_INFO_TYPE_T As Char = "T"     'Today Exchange Rate
#End Region

#Region "DB Data Type Mapping"
        Public Const DATA_TYPE_ER_ID As SqlDbType = SqlDbType.VarChar
        Public Const DATA_SIZE_ER_ID As Integer = 8

        Public Const DATA_TYPE_ERS_ID As SqlDbType = SqlDbType.VarChar
        Public Const DATA_SIZE_ERS_ID As Integer = 9

        Public Const DATA_TYPE_EFFECTIVE_DATE As SqlDbType = SqlDbType.DateTime
        Public Const DATA_SIZE_EFFECTIVE_DATE As Integer = 8

        Public Const DATA_TYPE_EXCHANGE_RATE As SqlDbType = SqlDbType.Decimal
        Public Const DATA_SIZE_EXCHANGE_RATE As Integer = 5

        Public Const DATA_TYPE_RECORD_STATUS As SqlDbType = SqlDbType.Char
        Public Const DATA_SIZE_RECORD_STATUS As Integer = 1

        Public Const DATA_TYPE_RECORD_TYPE As SqlDbType = SqlDbType.Char
        Public Const DATA_SIZE_RECORD_TYPE As Integer = 1

        Public Const DATA_TYPE_CREATE_BY As SqlDbType = SqlDbType.VarChar
        Public Const DATA_SIZE_CREATE_BY As Integer = 20

        Public Const DATA_TYPE_CREATE_DTM As SqlDbType = SqlDbType.DateTime
        Public Const DATA_SIZE_CREATE_DTM As Integer = 8

        Public Const DATA_TYPE_UPDATE_BY As SqlDbType = SqlDbType.VarChar
        Public Const DATA_SIZE_UPDATE_BY As Integer = 20

        Public Const DATA_TYPE_UPDATE_DTM As SqlDbType = SqlDbType.DateTime
        Public Const DATA_SIZE_UPDATE_DTM As Integer = 8

        Public Const DATA_TYPE_APPROVE_BY As SqlDbType = SqlDbType.VarChar
        Public Const DATA_SIZE_APPROVE_BY As Integer = 20

        Public Const DATA_TYPE_APPROVE_DTM As SqlDbType = SqlDbType.DateTime
        Public Const DATA_SIZE_APPROVE_DTM As Integer = 8

        Public Const DATA_TYPE_REJECT_BY As SqlDbType = SqlDbType.VarChar
        Public Const DATA_SIZE_REJECT_BY As Integer = 20

        Public Const DATA_TYPE_REJECT_DTM As SqlDbType = SqlDbType.DateTime
        Public Const DATA_SIZE_REJECT_DTM As Integer = 8

        Public Const DATA_TYPE_DELETE_BY As SqlDbType = SqlDbType.VarChar
        Public Const DATA_SIZE_DELETE_BY As Integer = 20

        Public Const DATA_TYPE_DELETE_DTM As SqlDbType = SqlDbType.DateTime
        Public Const DATA_SIZE_DELETE_DTM As Integer = 8

        Public Const DATA_TYPE_TSMP As SqlDbType = SqlDbType.Timestamp
        Public Const DATA_SIZE_TSMP As Integer = 8
#End Region

#Region "[Enum_Class] of DB Table - [StatusData]"
        ' To map the display text of the Field - [Record_Status]
        Public Const STATUS_DATA_CLASS_ER As String = "ConversionRateStatus"
        Public Const STATUS_DATA_CLASS_ERS As String = "ConversionRatesStagingStatus"
        Public Const STATUS_DATA_CLASS_ERS_ACTION As String = "ConversionRateAction"

#End Region

#Region "Private Member"
        Private _strExchangeRateID As String
        Private _strExchangeRateStagingID As String
        Private _dtmEffectiveDate As DateTime
        Private _decExchangeRate As Decimal
        Private _chrRecordStatus As Char
        Private _chrRecordType As Char
        Private _strExchangeRateIDRefByStaging As String
        Private _strCreateBy As String
        Private _dtmCreateDtm As DateTime
        Private _strUpdateBy As String
        Private _dtmUpdateDtm As DateTime
        Private _strApproveBy As String
        Private _dtmApproveDtm As DateTime
        Private _strRejectBy As String
        Private _dtmRejectDtm As DateTime
        Private _strDeleteBy As String
        Private _dtmDeleteDtm As DateTime
        Private _byteTSMP As Byte()
#End Region

#Region "Property"
        Public Property ExchangeRateID() As String
            Get
                Return _strExchangeRateID
            End Get
            Set(ByVal value As String)
                _strExchangeRateID = value
            End Set
        End Property

        Public Property ExchangeRateStagingID() As String
            Get
                Return _strExchangeRateStagingID
            End Get
            Set(ByVal value As String)
                _strExchangeRateStagingID = value
            End Set
        End Property

        Public Property EffectiveDate() As DateTime
            Get
                Return _dtmEffectiveDate
            End Get
            Set(ByVal value As DateTime)
                _dtmEffectiveDate = value
            End Set
        End Property

        Public Property ExchangeRate() As Decimal
            Get
                Return _decExchangeRate
            End Get
            Set(ByVal value As Decimal)
                _decExchangeRate = value
            End Set
        End Property

        Public Property RecordStatus() As Char
            Get
                Return _chrRecordStatus
            End Get
            Set(ByVal value As Char)
                _chrRecordStatus = value
            End Set
        End Property

        Public Property RecordType() As Char
            Get
                Return _chrRecordType
            End Get
            Set(ByVal value As Char)
                _chrRecordType = value
            End Set
        End Property

        Public Property ExchangeRateIDRefByStaging() As String
            Get
                Return _strExchangeRateIDRefByStaging
            End Get
            Set(ByVal value As String)
                _strExchangeRateIDRefByStaging = value
            End Set
        End Property

        Public Property CreateBy() As String
            Get
                Return _strCreateBy
            End Get
            Set(ByVal value As String)
                _strCreateBy = value
            End Set
        End Property

        Public Property CreateDtm() As DateTime
            Get
                Return _dtmCreateDtm
            End Get
            Set(ByVal value As DateTime)
                _dtmCreateDtm = value
            End Set
        End Property

        Public Property UpdateBy() As String
            Get
                Return _strUpdateBy
            End Get
            Set(ByVal value As String)
                _strUpdateBy = value
            End Set
        End Property

        Public Property UpdateDtm() As DateTime
            Get
                Return _dtmUpdateDtm
            End Get
            Set(ByVal value As DateTime)
                _dtmUpdateDtm = value
            End Set
        End Property

        Public Property ApproveBy() As String
            Get
                Return _strApproveBy
            End Get
            Set(ByVal value As String)
                _strApproveBy = value
            End Set
        End Property

        Public Property ApproveDtm() As DateTime
            Get
                Return _dtmApproveDtm
            End Get
            Set(ByVal value As DateTime)
                _dtmApproveDtm = value
            End Set
        End Property

        Public Property RejectBy() As String
            Get
                Return _strRejectBy
            End Get
            Set(ByVal value As String)
                _strRejectBy = value
            End Set
        End Property

        Public Property RejectDtm() As DateTime
            Get
                Return _dtmRejectDtm
            End Get
            Set(ByVal value As DateTime)
                _dtmRejectDtm = value
            End Set
        End Property

        Public Property DeleteBy() As String
            Get
                Return _strDeleteBy
            End Get
            Set(ByVal value As String)
                _strDeleteBy = value
            End Set
        End Property

        Public Property DeleteDtm() As DateTime
            Get
                Return _dtmDeleteDtm
            End Get
            Set(ByVal value As DateTime)
                _dtmDeleteDtm = value
            End Set
        End Property

        Public Property TSMP() As Byte()
            Get
                Return _byteTSMP
            End Get
            Set(ByVal value As Byte())
                _byteTSMP = value
            End Set
        End Property

#End Region

#Region "Constructor"
        ' For [ExchangeRate] Retrieval
        Public Sub New(ByVal strExchangeRateID As String, _
                        ByVal strExchangeRateStagingID As String, _
                        ByVal dtmEffectiveDate As DateTime, _
                        ByVal decExchangeRate As Decimal, _
                        ByVal chrRecordStatus As Char, _
                        ByVal chrRecordType As Char, _
                        ByVal strExchangeRateIDRefByStaging As String, _
                        ByVal strCreateBy As String, _
                        ByVal dtmCreateDtm As DateTime, _
                        ByVal strUpdateBy As String, _
                        ByVal dtmUpdateDtm As DateTime, _
                        ByVal strApproveBy As String, _
                        ByVal dtmApproveDtm As DateTime, _
                        ByVal strRejectBy As String, _
                        ByVal dtmRejectDtm As DateTime, _
                        ByVal strDeleteBy As String, _
                        ByVal dtmDeleteDtm As DateTime, _
                        ByVal byteTSMP As Byte())

            _strExchangeRateID = strExchangeRateID
            _strExchangeRateStagingID = strExchangeRateStagingID
            _dtmEffectiveDate = dtmEffectiveDate
            _decExchangeRate = decExchangeRate
            _chrRecordStatus = chrRecordStatus
            _chrRecordType = chrRecordType
            _strExchangeRateIDRefByStaging = strExchangeRateIDRefByStaging
            _strCreateBy = strCreateBy
            _dtmCreateDtm = dtmCreateDtm
            _strUpdateBy = strUpdateBy
            _dtmUpdateDtm = dtmUpdateDtm
            _strApproveBy = strApproveBy
            _dtmApproveDtm = dtmApproveDtm
            _strRejectBy = strRejectBy
            _dtmRejectDtm = dtmRejectDtm
            _strDeleteBy = strDeleteBy
            _dtmDeleteDtm = dtmDeleteDtm
            _byteTSMP = byteTSMP

        End Sub

        ' For [ExchangeRate] Creation
        Public Sub New(ByVal strExchangeRateID As String, _
                        ByVal dtmEffectiveDate As DateTime, _
                        ByVal decExchangeRate As Decimal, _
                        ByVal chrRecordStatus As Char, _
                        ByVal strCreateBy As String, _
                        ByVal dtmCreateDtm As DateTime, _
                        ByVal strApproveBy As String, _
                        ByVal dtmApproveDtm As DateTime)

            Me.New(strExchangeRateID, _
                        Nothing, _
                        dtmEffectiveDate, _
                        decExchangeRate, _
                        chrRecordStatus, _
                        Nothing, _
                        Nothing, _
                        strCreateBy, _
                        dtmCreateDtm, _
                        Nothing, _
                        Nothing, _
                        strApproveBy, _
                        dtmApproveDtm, _
                        Nothing, _
                        Nothing, _
                        Nothing, _
                        Nothing, _
                        Nothing)

        End Sub

        ' For [ExchangeRateStaging] Creation
        Public Sub New(ByVal strExchangeRateStagingID As String, _
                        ByVal dtmEffectiveDate As DateTime, _
                        ByVal decExchangeRate As Decimal, _
                        ByVal chrRecordStatus As Char, _
                        ByVal chrRecordType As Char, _
                        ByVal strCreateBy As String, _
                        ByVal strExchangeRateIDRefByStaging As String)

            Me.New(Nothing, _
                        strExchangeRateStagingID, _
                        dtmEffectiveDate, _
                        decExchangeRate, _
                        chrRecordStatus, _
                        chrRecordType, _
                        strExchangeRateIDRefByStaging, _
                        strCreateBy, _
                        Nothing, _
                        Nothing, _
                        Nothing, _
                        Nothing, _
                        Nothing, _
                        Nothing, _
                        Nothing, _
                        Nothing, _
                        Nothing, _
                        Nothing)

        End Sub
#End Region

#Region "Public Method"
        ' Get the display text of the Field - [Exchange Rate Record_Status]
        Public Function GetExchangeRateRecordStatusDisplayText() As String
            Dim strDisplayText As String = ""

            Status.GetDescriptionFromDBCode(STATUS_DATA_CLASS_ER, _chrRecordStatus.ToString(), strDisplayText, String.Empty)

            Return strDisplayText
        End Function

        ' Get the display text of the Field - [Exchange Rate Staging Record_Status]
        Public Function GetExchangeRateStagingRecordStatusDisplayText() As String
            Dim strDisplayText As String = ""

            Status.GetDescriptionFromDBCode(STATUS_DATA_CLASS_ERS, _chrRecordStatus.ToString(), strDisplayText, String.Empty)

            Return strDisplayText
        End Function

        ' Get the display text of the Field - [Exchange Rate Action]
        Public Function GetExchangeRateActionDisplayText() As String
            Dim strDisplayText As String = ""

            Status.GetDescriptionFromDBCode(STATUS_DATA_CLASS_ERS_ACTION, _chrRecordType.ToString(), strDisplayText, String.Empty)

            Return strDisplayText
        End Function
#End Region

    End Class

End Namespace
