Imports common.Component

Public Class AuditLogPCDCheckAccountStatus
    Inherits AuditLogBase

    Public Sub New(ByVal strFunctionCode As String, ByVal strDBFlag As String)
        MyBase.New(strFunctionCode, strDBFlag)
    End Sub

    Protected Overrides Sub CreateMessage()
        _hashMessage = New Hashtable(6)

        ' ===========================================
        ' Method geteHSVaccineRecord
        ' ===========================================
        _hashMessage.Add(LogID.LOG00000, "PCDCheckAccountStatus - Start")
        _hashMessage.Add(LogID.LOG00001, "PCDCheckAccountStatus - Invoke PCD Start")
        _hashMessage.Add(LogID.LOG00002, "PCDCheckAccountStatus - eHS Request XML")
        _hashMessage.Add(LogID.LOG00003, "PCDCheckAccountStatus - PCD Result XML")
        _hashMessage.Add(LogID.LOG00004, "PCDCheckAccountStatus - Invoke PCD End")
        _hashMessage.Add(LogID.LOG00005, "PCDCheckAccountStatus - End")
    End Sub
End Class
