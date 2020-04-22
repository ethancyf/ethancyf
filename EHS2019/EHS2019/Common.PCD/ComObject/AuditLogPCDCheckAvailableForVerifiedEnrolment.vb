Imports common.Component

Public Class AuditLogPCDCheckAvailableForVerifiedEnrolment
    Inherits AuditLogBase

    Public Sub New(ByVal strFunctionCode As String, ByVal strDBFlag As String)
        MyBase.New(strFunctionCode, strDBFlag)
    End Sub

    Protected Overrides Sub CreateMessage()
        _hashMessage = New Hashtable(6)

        ' ===========================================
        ' Method geteHSVaccineRecord
        ' ===========================================
        _hashMessage.Add(LogID.LOG00000, "PCDCheckAvailableForVerifiedEnrolment - Start")
        _hashMessage.Add(LogID.LOG00001, "PCDCheckAvailableForVerifiedEnrolment - Invoke PCD Start")
        _hashMessage.Add(LogID.LOG00002, "PCDCheckAvailableForVerifiedEnrolment - eHS Request XML")
        _hashMessage.Add(LogID.LOG00003, "PCDCheckAvailableForVerifiedEnrolment - PCD Result XML")
        _hashMessage.Add(LogID.LOG00004, "PCDCheckAvailableForVerifiedEnrolment - Invoke PCD End")
        _hashMessage.Add(LogID.LOG00005, "PCDCheckAvailableForVerifiedEnrolment - End")
    End Sub
End Class
