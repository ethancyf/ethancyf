Namespace Component.Scheme

    <Serializable()> Public Class SchemeSetupModel

#Region "DB Table Schema - [SchemeSetup_SESU]"
        '[SESU_Scheme_Code] [char](10) NOT NULL,
        '[SESU_TransactionStatus] [char](1) NOT NULL,
        '[SESU_SetupType] [varchar](100) NOT NULL,
        '[SESU_SetupValue] [varchar](100) NOT NULL,
        '[SESU_Create_Dtm] [datetime] NOT NULL,
        '[SESU_Create_By] [varchar](20) NOT NULL,
        '[SESU_Update_Dtm] [datetime] NOT NULL,
        '[SESU_Update_By] [varchar](20) NOT NULL

        'PRIMARY KEY ([SESU_Scheme_Code], [SESU_TransactionStatus], [SESU_SetupType])
#End Region

#Region "DB Data Type Mapping"
        Public Const DATA_TYPE_SCHEME_CODE As SqlDbType = SqlDbType.Char
        Public Const DATA_SIZE_SCHEME_CODE As Integer = 10

        Public Const DATA_TYPE_TRANS_STATUS As SqlDbType = SqlDbType.Char
        Public Const DATA_SIZE_TRANS_STATUS As Integer = 1

        Public Const DATA_TYPE_SETUP_TYPE As SqlDbType = SqlDbType.VarChar
        Public Const DATA_SIZE_SETUP_TYPE As Integer = 100

        Public Const DATA_TYPE_SETUP_VALUE As SqlDbType = SqlDbType.VarChar
        Public Const DATA_SIZE_SETUP_VALUE As Integer = 100
#End Region

#Region "Setup Type of DB Field - [SESU_SetupType]"
        Public Const SetupType_ClaimCompletionMessage As String = "ClaimCompletionMessage"
        'CRE15-003 System-generated Form [Start][Philip Chau]
        Public Const SetupType_ClaimCompletionMessageOutdateTxNo As String = "ClaimCompletionMessageOutdateTxNo"
        'CRE15-003 System-generated Form [End][Philip Chau]
#End Region

#Region "Private Member"
        Private _strSchemeCode As String
        Private _strTransactionStatus As Char
        Private _strSetupType As String
        Private _strSetupValue As String
#End Region

#Region "Property"
        Public ReadOnly Property SchemeCode() As String
            Get
                Return _strSchemeCode
            End Get
        End Property

        Public ReadOnly Property TransactionStatus() As Char
            Get
                Return _strTransactionStatus
            End Get
        End Property

        Public ReadOnly Property SetupType() As String
            Get
                Return _strSetupType
            End Get
        End Property

        Public ReadOnly Property SetupValue() As String
            Get
                Return _strSetupValue
            End Get
        End Property
#End Region

#Region "Constructor"
        Public Sub New(ByVal strSchemeCode As String, ByVal strTransactionStatus As Char, ByVal strSetupType As String, ByVal strSetupValue As String)
            _strSchemeCode = strSchemeCode
            _strTransactionStatus = strTransactionStatus
            _strSetupType = strSetupType
            _strSetupValue = strSetupValue
        End Sub
#End Region

    End Class

End Namespace
