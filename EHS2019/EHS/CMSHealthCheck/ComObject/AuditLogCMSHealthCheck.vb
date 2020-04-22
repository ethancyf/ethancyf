Imports common.Component

Public Class AuditLogCMSHealthCheck
    Inherits AuditLogBase

    Public Sub New(ByVal strFunctionCode As String, ByVal strDBFlag As String)
        MyBase.New(strFunctionCode, strDBFlag)
    End Sub

    Protected Overrides Sub CreateMessage()
        _hashMessage = New Hashtable(6)

        ' ===========================================
        ' CMS Health Check
        ' ===========================================
        _hashMessage.Add(LogID.LOG00000, "CMSHealthCheck - Invoke Start")
        _hashMessage.Add(LogID.LOG00001, "CMSHealthCheck - eHS Request XML")
        _hashMessage.Add(LogID.LOG00002, "CMSHealthCheck - CMS Result XML")
        _hashMessage.Add(LogID.LOG00003, "CMSHealthCheck - Invoke End")
    End Sub
End Class
