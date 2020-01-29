Imports common.Component

Public Class AuditLogGetEHSPracticeScheme
    Inherits AuditLogBase

    Public Sub New(ByVal strFunctionCode As String, ByVal strDBFlag As String)
        MyBase.New(strFunctionCode, strDBFlag)
    End Sub

    Protected Overrides Sub CreateMessage()
        _hashMessage = New Hashtable(6)

        ' ===========================================
        ' Method geteHSVaccineRecord
        ' ===========================================
        _hashMessage.Add(LogID.LOG00000, "GetEHSPracticeScheme - Start")
        _hashMessage.Add(LogID.LOG00001, "GetEHSPracticeScheme - PCD Request XML")
        _hashMessage.Add(LogID.LOG00002, "GetEHSPracticeScheme - Authentication Failed")
        _hashMessage.Add(LogID.LOG00003, "GetEHSPracticeScheme - EHS Result XML")
        _hashMessage.Add(LogID.LOG00004, "GetEHSPracticeScheme - Error")
        _hashMessage.Add(LogID.LOG00005, "GetEHSPracticeScheme - End")
    End Sub
End Class
