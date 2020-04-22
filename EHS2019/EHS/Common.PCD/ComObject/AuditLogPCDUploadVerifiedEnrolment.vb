Imports common.Component

Public Class AuditLogPCDUploadVerifiedEnrolment
    Inherits AuditLogBase

    Public Sub New(ByVal strFunctionCode As String, ByVal strDBFlag As String)
        MyBase.New(strFunctionCode, strDBFlag)
    End Sub

    Protected Overrides Sub CreateMessage()
        _hashMessage = New Hashtable(6)

        ' ===========================================
        ' Method geteHSVaccineRecord
        ' ===========================================
        _hashMessage.Add(LogID.LOG00000, "PCDUploadVerifiedEnrolment - Start")
        _hashMessage.Add(LogID.LOG00001, "PCDUploadVerifiedEnrolment - Invoke PCD Start")
        _hashMessage.Add(LogID.LOG00002, "PCDUploadVerifiedEnrolment - eHS Request XML")
        _hashMessage.Add(LogID.LOG00003, "PCDUploadVerifiedEnrolment - PCD Result XML")
        _hashMessage.Add(LogID.LOG00004, "PCDUploadVerifiedEnrolment - Invoke PCD End")
        _hashMessage.Add(LogID.LOG00005, "PCDUploadVerifiedEnrolment - End")
    End Sub
End Class
