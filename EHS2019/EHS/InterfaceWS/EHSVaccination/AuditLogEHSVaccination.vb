Imports common.Component

Public Class AuditLogEHSVaccination
    Inherits AuditLogBase

    Public Sub New(ByVal strFunctionCode As String, ByVal strDBFlag As String)
        MyBase.New(strFunctionCode, strDBFlag)
    End Sub

    Protected Overrides Sub CreateMessage()
        _hashMessage = New Hashtable(100)

        ' ===========================================
        ' Method geteHSVaccineRecord
        ' ===========================================

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
        ' ----------------------------------------------------------
        _hashMessage.Add(LogID.LOG00000, "WebMethod geteHSVaccineRecord Start")
        _hashMessage.Add(LogID.LOG00001, "[{0}>EHS] Login Fail")
        _hashMessage.Add(LogID.LOG00002, "[{0}>EHS] Login Success")
        _hashMessage.Add(LogID.LOG00003, "[{0}>EHS] WebMethod geteHSVaccineRecord End")
        _hashMessage.Add(LogID.LOG00004, "[{0}>EHS] Request body")
        _hashMessage.Add(LogID.LOG00005, "[{0}>EHS] Response body")
        _hashMessage.Add(LogID.LOG00006, "[{0}>EHS] Read Request")
        _hashMessage.Add(LogID.LOG00007, "[{0}>EHS] Read Request Error")
        _hashMessage.Add(LogID.LOG00008, "[{0}>EHS] Process Request")
        _hashMessage.Add(LogID.LOG00009, "[{0}>EHS] Process Request Error")
        _hashMessage.Add(LogID.LOG00010, "[{0}>EHS] Render EHS Response")
        _hashMessage.Add(LogID.LOG00011, "[{0}>EHS] WebMethod geteHSVaccineRecord Error")

        '_hashMessage.Add(LogID.LOG00001, "Login Fail")
        '_hashMessage.Add(LogID.LOG00002, "Login Success")
        '_hashMessage.Add(LogID.LOG00003, "WebMethod geteHSVaccineRecord End")
        '_hashMessage.Add(LogID.LOG00004, "CMS Request")
        '_hashMessage.Add(LogID.LOG00005, "EHS Result")
        '_hashMessage.Add(LogID.LOG00006, "Read CMS Request")
        '_hashMessage.Add(LogID.LOG00007, "Read CMS Request Error")
        '_hashMessage.Add(LogID.LOG00008, "Process CMS Request")
        '_hashMessage.Add(LogID.LOG00009, "Process CMS Request Error")
        '_hashMessage.Add(LogID.LOG00010, "Render EHS Result")
        '_hashMessage.Add(LogID.LOG00011, "WebMethod geteHSVaccineRecord Error")

        ' CRE18-004 (CIMS Vaccination Sharing) [End][Koala CHENG]
    End Sub
End Class
