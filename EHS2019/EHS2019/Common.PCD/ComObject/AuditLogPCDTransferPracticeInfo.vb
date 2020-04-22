Imports common.Component

Public Class AuditLogPCDTransferPracticeInfo
    Inherits AuditLogBase

    Public Sub New(ByVal strFunctionCode As String, ByVal strDBFlag As String)
        MyBase.New(strFunctionCode, strDBFlag)
    End Sub

    Protected Overrides Sub CreateMessage()
        _hashMessage = New Hashtable(6)

        ' ===========================================
        ' Method geteHSVaccineRecord
        ' ===========================================
        _hashMessage.Add(LogID.LOG00000, "PCDTransferPracticeInfo - Start")
        _hashMessage.Add(LogID.LOG00001, "PCDTransferPracticeInfo - Invoke PCD Start")
        _hashMessage.Add(LogID.LOG00002, "PCDTransferPracticeInfo - eHS Request XML")
        _hashMessage.Add(LogID.LOG00003, "PCDTransferPracticeInfo - PCD Result XML")
        _hashMessage.Add(LogID.LOG00004, "PCDTransferPracticeInfo - Invoke PCD End")
        _hashMessage.Add(LogID.LOG00005, "PCDTransferPracticeInfo - End")
    End Sub
End Class
