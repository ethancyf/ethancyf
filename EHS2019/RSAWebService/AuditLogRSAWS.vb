Imports common.Component

Public Class AuditLogRSAWS
    Inherits AuditLogBase

    Public Sub New(ByVal strFunctionCode As String, ByVal strDBFlag As String)
        MyBase.New(strFunctionCode, strDBFlag)
    End Sub

    Protected Overrides Sub CreateMessage()
        _hashMessage = New Hashtable(100)

        ' ===========================================
        ' Method auth
        ' ===========================================
        _hashMessage.Add(LogID.LOG00000, "RSAWS Authenticate Start")
        _hashMessage.Add(LogID.LOG00001, "RSAWS Authenticate End")
        _hashMessage.Add(LogID.LOG00002, "RSAWS Authenticate Fail")
    End Sub
End Class
