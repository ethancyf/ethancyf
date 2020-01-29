' [CRE13-006] HCVS Ceiling

Namespace Component.EHSAccount

    Public Class SubsidizeWriteOffGeneratorQueue
        Inherits Queue(Of SubsidizeWriteOffGeneratorQueueItem)

    End Class

    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
    ' -----------------------------------------------------------------------------------------
    ' Add [DOD], [Exact_DOD]
    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

    Public Class SubsidizeWriteOffGeneratorQueueItem

#Region "DB Table Schema - [SubsidizeWriteOffGeneratorQueue]"
        '[Row_ID] [int] NOT NULL,
        '[Doc_Code] [char](20) NOT NULL,
        '[Encrypt_Field1] [varbinary](100) NOT NULL,
        '[DOB] [datetime] NOT NULL,
        '[Exact_DOB] [char](1) NOT NULL,
        '[Scheme_Code] [char](10) NOT NULL,
        '[Subsidize_Code] [char](10) NOT NULL,
        '[Create_Dtm] [datetime] NOT NULL,
        '[Update_Dtm] [datetime] NOT NULL,
        '[Record_Status] [char](1) NOT NULL,
        '[TSMP] [timestamp] NOT NULL
        '[DOD] [datetime] NULL,
        '[Exact_DOD] [char](1) NULL,
        'PRIMARY KEY ([Row_ID])
#End Region

#Region "DB Data Value of the Field - [Record_Status]"
        Public Const RECORD_STATUS_P As String = "P"     'Pending
        Public Const RECORD_STATUS_C As String = "C"     'Completed
#End Region

#Region "DB Data Type Mapping"
        Public Const DATA_TYPE_ROW_ID As SqlDbType = SqlDbType.Int
        Public Const DATA_SIZE_ROW_ID As Integer = 4

        Public Const DATA_TYPE_RECORD_STATUS As SqlDbType = SqlDbType.Char
        Public Const DATA_SIZE_RECORD_STATUS As Integer = 1
#End Region

#Region "Private Member"
        Private _intRowID As Integer
        Private _strDocCode As String
        Private _strDocID As String
        Private _dtmDOB As Date
        Private _strExactDOB As String
        Private _strSchemeCode As String
        Private _strSubsidizeCode As String
        Private _strRecordStatus As String
        Private _byteTSMP As Byte()
        Private _dtmDOD As Nullable(Of Date)
        Private _strExactDOD As String

#End Region

#Region "Property"
        Public Property RowID() As Integer
            Get
                Return _intRowID
            End Get
            Set(ByVal value As Integer)
                _intRowID = value
            End Set
        End Property

        Public Property DocCode() As String
            Get
                Return _strDocCode
            End Get
            Set(ByVal value As String)
                _strDocCode = value
            End Set
        End Property

        Public Property DocID() As String
            Get
                Return _strDocID
            End Get
            Set(ByVal value As String)
                _strDocID = value
            End Set
        End Property

        Public Property DOB() As Date
            Get
                Return _dtmDOB
            End Get
            Set(ByVal value As Date)
                _dtmDOB = value
            End Set
        End Property

        Public Property ExactDOB() As String
            Get
                Return _strExactDOB
            End Get
            Set(ByVal value As String)
                _strExactDOB = value
            End Set
        End Property

        Public Property SchemeCode() As String
            Get
                Return _strSchemeCode
            End Get
            Set(ByVal value As String)
                _strSchemeCode = value
            End Set
        End Property

        Public Property SubsidizeCode() As String
            Get
                Return _strSubsidizeCode
            End Get
            Set(ByVal value As String)
                _strSubsidizeCode = value
            End Set
        End Property

        Public Property RecordStatus() As String
            Get
                Return _strRecordStatus
            End Get
            Set(ByVal value As String)
                _strRecordStatus = value
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

        Public Property DOD() As Nullable(Of DateTime)
            Get
                Return _dtmDOD
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmDOD = value
            End Set
        End Property

        Public Property ExactDOD() As String
            Get
                Return _strExactDOD
            End Get
            Set(ByVal value As String)
                _strExactDOD = value
            End Set
        End Property

#End Region

#Region "Constructor"
        Public Sub New(ByVal intRowID As Integer, _
                       ByVal strDocCode As String, _
                       ByVal strDocID As String, _
                       ByVal dtmDOB As Date, _
                       ByVal strExactDOB As String, _
                       ByVal strSchemeCode As String, _
                       ByVal strSubsidizeCode As String, _
                       ByVal strRecordStatus As String, _
                       ByVal byteTSMP As Byte(), _
                       ByVal dtmDOD As Nullable(Of Date),
                       ByVal strExactDOD As String)

            _intRowID = intRowID
            _strDocCode = strDocCode
            _strDocID = strDocID
            _dtmDOB = dtmDOB
            _strExactDOB = strExactDOB
            _strSchemeCode = strSchemeCode
            _strSubsidizeCode = strSubsidizeCode
            _strRecordStatus = strRecordStatus
            _byteTSMP = byteTSMP
            _dtmDOD = dtmDOD
            _strExactDOD = strExactDOD

        End Sub
#End Region

    End Class

End Namespace
