Namespace Component.PCDStatus

    Public Class PCDStatusUpdaterQueue
        Inherits Queue(Of PCDStatusUpdaterQueueModel)

    End Class

    Public Class PCDStatusUpdaterQueueModel

#Region "DB Table Schema - [PCDStatusUpdateQueue]"
        '[SP_ID] [char](8) NOT NULL,
        '[Encrypt_Field1] [varbinary](100) NOT NULL,
        '[Record_Status] [char](1) NOT NULL,
        '[Create_Dtm] [datetime] NOT NULL,
        '[Update_Dtm] [datetime] NOT NULL,

#End Region

#Region "DB Data Value of the Field - [Record_Status]"
        Public Class DBRecordStatus
            Public Const Pending As String = "P"     'Pending
            Public Const Completed As String = "C"     'Completed
        End Class

#End Region

#Region "DB Data Mapping"
        Public Class DBDataType
            Public Const SPID As SqlDbType = SqlDbType.Char
            Public Const DocID As SqlDbType = SqlDbType.VarChar
            Public Const RecordStatus As SqlDbType = SqlDbType.Char
            Public Const CreateDtm As SqlDbType = SqlDbType.DateTime
            Public Const UpdateDtm As SqlDbType = SqlDbType.DateTime

        End Class

        Public Class DBDataSize
            Public Const SPID As Integer = 8
            Public Const DocID As Integer = 20
            Public Const RecordStatus As Integer = 1
            Public Const CreateDtm As Integer = 8
            Public Const UpdateDtm As Integer = 8

        End Class

#End Region

#Region "Private Members"
        Private _strSPID As String
        Private _strDocID As String
        Private _strRecordStatus As String
        Private _dtmCreateDtm As DateTime
        Private _dtmUpdateDtm As DateTime

#End Region

#Region "Property"
        Public Property SPID() As String
            Get
                Return Me._strSPID
            End Get
            Set(ByVal value As String)
                Me._strSPID = value
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

        Public Property RecordStatus() As String
            Get
                Return Me._strRecordStatus
            End Get
            Set(ByVal value As String)
                Me._strRecordStatus = value
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

        Public Property UpdateDtm() As DateTime
            Get
                Return Me._dtmUpdateDtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmUpdateDtm = value
            End Set
        End Property

#End Region

#Region "Constructor"
        Public Sub New(ByVal strSPID As String, ByVal strDocID As String, ByVal strRecordStatus As String, _
                       ByVal dtmCreateDtm As DateTime, ByVal dtmUpdateDtm As DateTime)

            _strSPID = strSPID
            _strDocID = strDocID
            _strRecordStatus = strRecordStatus
            _dtmCreateDtm = dtmCreateDtm
            _dtmUpdateDtm = dtmUpdateDtm

        End Sub

#End Region

    End Class

End Namespace
