' [CRE12-012] Infrastructure on Sending Messages through eHealth System Inbox

Namespace Component.SentOutMessage

    <Serializable()> Public Class SentOutMsgRecipientModel

#Region "DB Table Schema - [SentOutMsgRecipient_SOMR]"
        '[SOMR_SentOutMsg_ID] [varchar](15) NOT NULL,
        '[SOMR_Profession] [varchar](3) NOT NULL,
        '[SOMR_Scheme] [varchar](10) NOT NULL,
        '[SOMR_Create_By] [varchar](20) NOT NULL,
        '[SOMR_Create_Dtm] [datetime] NOT NULL

        'PRIMARY KEY ([SOMR_SentOutMsg_ID], [SOMR_Profession], [SOMR_Scheme])
#End Region

#Region "DB Data Value of the Field - [SOMR_Profession]"
        Public Const PROFESSION_NA As String = "NA"     'N/A
#End Region

#Region "DB Data Value of the Field - [SOMR_Scheme]"
        Public Const SCHEME_NA As String = "NA"     'N/A
#End Region

#Region "DB Data Type Mapping"
        Public Const DATA_TYPE_SO_MSG_ID As SqlDbType = SqlDbType.VarChar
        Public Const DATA_SIZE_SO_MSG_ID As Integer = 15

        Public Const DATA_TYPE_PROFESSION As SqlDbType = SqlDbType.VarChar
        Public Const DATA_SIZE_PROFESSION As Integer = 3

        Public Const DATA_TYPE_SCHEME As SqlDbType = SqlDbType.VarChar
        Public Const DATA_SIZE_SCHEME As Integer = 10
#End Region

#Region "Private Member"
        Private _strSentOutMsgID As String
        Private _strProfession As String
        Private _strScheme As String
#End Region

#Region "Property"
        Public Property SentOutMsgID() As String
            Get
                Return _strSentOutMsgID
            End Get
            Set(ByVal value As String)
                _strSentOutMsgID = value
            End Set
        End Property

        Public Property Profession() As String
            Get
                Return _strProfession
            End Get
            Set(ByVal value As String)
                _strProfession = value
            End Set
        End Property

        Public Property Scheme() As String
            Get
                Return _strScheme
            End Get
            Set(ByVal value As String)
                _strScheme = value
            End Set
        End Property
#End Region

#Region "Constructor"
        Public Sub New(ByVal strSentOutMsgID As String, _
                       ByVal strProfession As String, _
                       ByVal strScheme As String)

            _strSentOutMsgID = strSentOutMsgID
            _strProfession = strProfession
            _strScheme = strScheme

        End Sub
#End Region

    End Class

End Namespace
