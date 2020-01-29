Imports common.Component

Public Class AuditLogPCDUploadEnrolInfo
    Inherits AuditLogBase

    Public Sub New(ByVal strFunctionCode As String, ByVal strDBFlag As String)
        MyBase.New(strFunctionCode, strDBFlag)
    End Sub

    Protected Overrides Sub CreateMessage()
        _hashMessage = New Hashtable(6)

        ' ===========================================
        ' Method geteHSVaccineRecord
        ' ===========================================
        _hashMessage.Add(LogID.LOG00000, "PCDUploadEnrolInfo - Start")
        _hashMessage.Add(LogID.LOG00001, "PCDUploadEnrolInfo - Invoke PCD Start")
        _hashMessage.Add(LogID.LOG00002, "PCDUploadEnrolInfo - eHS Request XML")
        _hashMessage.Add(LogID.LOG00003, "PCDUploadEnrolInfo - PCD Result XML")
        _hashMessage.Add(LogID.LOG00004, "PCDUploadEnrolInfo - Invoke PCD End")
        _hashMessage.Add(LogID.LOG00005, "PCDUploadEnrolInfo - End")
    End Sub
End Class
