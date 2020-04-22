Imports common.Component

Public Class AuditLogCMSHealthCheck
    Inherits AuditLogBase

    Public Sub New(ByVal strFunctionCode As String, ByVal strDBFlag As String)
        MyBase.New(strFunctionCode, strDBFlag)
    End Sub

    Protected Overrides Sub CreateMessage()
        _hashMessage = New Hashtable(6)

        ' ===========================================
        ' CMS / CIMS Health Check, AuditLogInterfaceXX
        ' ===========================================

        _hashMessage.Add(LogID.LOG00000, "[EHS>{0}] CMSHealthCheck - Invoke Start")
        _hashMessage.Add(LogID.LOG00001, "[EHS>{0}] CMSHealthCheck - Request body")
        _hashMessage.Add(LogID.LOG00002, "[EHS>{0}] CMSHealthCheck - Response body")
        _hashMessage.Add(LogID.LOG00003, "[EHS>{0}] CMSHealthCheck - Invoke End") ' Success
        _hashMessage.Add(LogID.LOG00004, "[EHS>{0}] CMSHealthCheck - Invoke Error") ' Fail

    End Sub
End Class
