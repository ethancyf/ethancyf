Imports common.Component

Public Class AuditLogEVacc
    Inherits AuditLogBase


    Public Sub New(ByVal strFunctionCode As String, ByVal strDBFlag As String)
        MyBase.New(strFunctionCode, strDBFlag)
    End Sub


    Protected Overrides Sub CreateMessage()
        _hashMessage = New Hashtable(6)

        ' ===========================================
        ' AuditLogInterfaceXX
        ' ===========================================
        _hashMessage.Add(LogID.LOG00001, "[EHS>{0}] Send request")
        _hashMessage.Add(LogID.LOG00002, "[EHS>{0}] Request body")
        _hashMessage.Add(LogID.LOG00003, "[EHS>{0}] Response body")
        _hashMessage.Add(LogID.LOG00004, "[EHS>{0}] Receive response fail")
        _hashMessage.Add(LogID.LOG00005, "[EHS>{0}] Receive response success")

    End Sub
End Class
