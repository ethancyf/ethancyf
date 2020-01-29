Namespace ComObject

    ''' <summary>
    ''' CRE11-004
    ''' Additional log information for audit log entry, 
    ''' e.g. Search critiera, Pure working data (Non object model)
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> Public Class AuditLogInfo

        Private _strSPID As String
        Public ReadOnly Property SPID() As String
            Get
                Return _strSPID
            End Get
        End Property

        Private _strSPDocNo As String
        Public ReadOnly Property SPDocNo() As String
            Get
                Return _strSPDocNo
            End Get
        End Property

        Private _strAccType As String
        Public ReadOnly Property AccType() As String
            Get
                Return _strAccType
            End Get
        End Property

        Private _strAccID As String
        Public ReadOnly Property AccID() As String
            Get
                Return _strAccID
            End Get
        End Property

        Private _strDocCode As String
        Public ReadOnly Property DocCode() As String
            Get
                Return _strDocCode
            End Get
        End Property

        Private _strDocNo As String
        Public ReadOnly Property DocNo() As String
            Get
                Return _strDocNo
            End Get
        End Property

        Public Sub New(ByVal strSPID As String, ByVal strSPDocNo As String, _
                       ByVal strAccType As String, ByVal strAccID As String, _
                       ByVal strDocCode As String, ByVal strDocNo As String)
            _strSPID = strSPID
            _strSPDocNo = strSPDocNo
            _strAccType = strAccType
            _strAccID = strAccID
            _strDocCode = strDocCode
            _strDocNo = strDocNo
        End Sub
    End Class

End Namespace
