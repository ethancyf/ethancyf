Imports common.Component

Public Class AuditLogPCDCheckIsActiveSP
    Inherits AuditLogBase

    Public Sub New(ByVal strFunctionCode As String, ByVal strDBFlag As String)
        MyBase.New(strFunctionCode, strDBFlag)
    End Sub

    Protected Overrides Sub CreateMessage()
        _hashMessage = New Hashtable(6)

        ' ===========================================
        ' Method geteHSVaccineRecord
        ' ===========================================
        _hashMessage.Add(LogID.LOG00000, "PCDCheckIsActiveSP - Start")
        _hashMessage.Add(LogID.LOG00001, "PCDCheckIsActiveSP - Invoke PCD Start")
        _hashMessage.Add(LogID.LOG00002, "PCDCheckIsActiveSP - eHS Request XML")
        _hashMessage.Add(LogID.LOG00003, "PCDCheckIsActiveSP - PCD Result XML")
        _hashMessage.Add(LogID.LOG00004, "PCDCheckIsActiveSP - Invoke PCD End")
        _hashMessage.Add(LogID.LOG00005, "PCDCheckIsActiveSP - End")
    End Sub
End Class
