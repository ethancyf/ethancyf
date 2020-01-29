Imports common.Component

Public Class AuditLogPCDCreatePCDSPAcct
    Inherits AuditLogBase

    Public Sub New(ByVal strFunctionCode As String, ByVal strDBFlag As String)
        MyBase.New(strFunctionCode, strDBFlag)
    End Sub

    Protected Overrides Sub CreateMessage()
        _hashMessage = New Hashtable(6)

        ' ===========================================
        ' Method geteHSVaccineRecord
        ' ===========================================
        _hashMessage.Add(LogID.LOG00000, "PCDCreatePCDSPAcct - Start")
        _hashMessage.Add(LogID.LOG00001, "PCDCreatePCDSPAcct - Invoke PCD Start")
        _hashMessage.Add(LogID.LOG00002, "PCDCreatePCDSPAcct - eHS Request XML")
        _hashMessage.Add(LogID.LOG00003, "PCDCreatePCDSPAcct - PCD Result XML")
        _hashMessage.Add(LogID.LOG00004, "PCDCreatePCDSPAcct - Invoke PCD End")
        _hashMessage.Add(LogID.LOG00005, "PCDCreatePCDSPAcct - End")
    End Sub
End Class
