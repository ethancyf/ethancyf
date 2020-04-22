Imports common.Component

Public Class AuditLogMaster

    Private Shared _udtInstance As AuditLogMaster

    Private _hashMessage As Hashtable

    Public Sub New()
        _hashMessage = New Hashtable(100)

        ' *******************************************
        ' VaccinationBLL
        ' *******************************************
        _hashMessage.Add(LogID.LOG01000, "Get EHS Vaccination")
        _hashMessage.Add(LogID.LOG01001, "Get EHS Vaccination complete")
        _hashMessage.Add(LogID.LOG01002, "Get CMS Vaccination")
        _hashMessage.Add(LogID.LOG01003, "Get CMS Vaccination fail: CMS vaccination record unavailable for current Doc Code")
        _hashMessage.Add(LogID.LOG01004, "Get CMS Vaccination complete")
        _hashMessage.Add(LogID.LOG01005, "Get CMS Vaccination complete: No record found")
        _hashMessage.Add(LogID.LOG01006, "Get CMS Vaccination complete: Partial record found")
        _hashMessage.Add(LogID.LOG01007, "Get CMS Vaccination fail: Patient not found")
        _hashMessage.Add(LogID.LOG01008, "Get CMS Vaccination fail: Patient not match")
        _hashMessage.Add(LogID.LOG01009, "Get CMS Vaccination fail: Invalid parameter")
        _hashMessage.Add(LogID.LOG01010, "Get CMS Vaccination fail: Unknown error")
        _hashMessage.Add(LogID.LOG01011, "Get CMS Vaccination fail: Communication link error")
        _hashMessage.Add(LogID.LOG01012, "Get CMS Vaccination fail: EHS internal error")
        _hashMessage.Add(LogID.LOG01013, "Get CMS Vaccination fail: Vaccination Record service is turned off in EHS")
        ' CRE10-035
        _hashMessage.Add(LogID.LOG01026, "Get CMS Vaccination fail: CMS result Message ID mismatch with EHS request Message ID")
        ' CRE11-002 (Part of CRE11-003)
        _hashMessage.Add(LogID.LOG01027, "Get CMS Vaccination fail: EAI Service Interruption")
        ' CRE11-002
        _hashMessage.Add(LogID.LOG01028, "Get CMS Vaccination fail: Returned health check result incorrect (Return Code: 100)")

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        _hashMessage.Add(LogID.LOG01102, "Get CIMS Vaccination")
        _hashMessage.Add(LogID.LOG01103, "Get CIMS Vaccination fail: CIMS vaccination record unavailable for current Doc Code")
        _hashMessage.Add(LogID.LOG01104, "Get CIMS Vaccination complete")
        _hashMessage.Add(LogID.LOG01105, "Get CIMS Vaccination complete: No record found")
        _hashMessage.Add(LogID.LOG01106, "Get CIMS Vaccination complete: Partial record found")
        _hashMessage.Add(LogID.LOG01107, "Get CIMS Vaccination fail: Client not found")
        _hashMessage.Add(LogID.LOG01108, "Get CIMS Vaccination fail: Client not match")
        _hashMessage.Add(LogID.LOG01109, "Get CIMS Vaccination fail: Invalid parameter")
        _hashMessage.Add(LogID.LOG01110, "Get CIMS Vaccination fail: Unknown error")
        _hashMessage.Add(LogID.LOG01111, "Get CIMS Vaccination fail: Communication link error")
        _hashMessage.Add(LogID.LOG01112, "Get CIMS Vaccination fail: EHS internal error")
        _hashMessage.Add(LogID.LOG01113, "Get CIMS Vaccination fail: Vaccination Record service is turned off in EHS")
        _hashMessage.Add(LogID.LOG01126, "Get CIMS Vaccination fail: CIMS result client mismatch with EHS request client")
        _hashMessage.Add(LogID.LOG01128, "Get CIMS Vaccination fail: Returned health check result incorrect (Return Code: 10001)")
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' *******************************************
        ' WebService
        ' *******************************************
        _hashMessage.Add(LogID.LOG01014, "Invoke CMS Web service: Start")
        _hashMessage.Add(LogID.LOG01015, "Invoke CMS Web service: End")
        _hashMessage.Add(LogID.LOG01016, "Invoke CMS Web service: Exception")
        _hashMessage.Add(LogID.LOG01017, "Read CMS Web service result: Start")
        _hashMessage.Add(LogID.LOG01018, "Read CMS Web service result: End")
        _hashMessage.Add(LogID.LOG01019, "Read CMS Web service result: Exception")
        _hashMessage.Add(LogID.LOG01020, "Patient is in exception list, will not invoke CMS Web service")
        _hashMessage.Add(LogID.LOG01021, "Generate CMS Web service request: Start")
        _hashMessage.Add(LogID.LOG01022, "Generate CMS Web service request: End")
        _hashMessage.Add(LogID.LOG01023, "Generate CMS Web service request: Exception")
        '_hashMessage.Add(LogID.LOG01024, "") EHS -> CMS request xml data
        '_hashMessage.Add(LogID.LOG01025, "") EHS -> CMS result xml data

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        _hashMessage.Add(LogID.LOG01114, "Invoke CIMS Web service: Start")
        _hashMessage.Add(LogID.LOG01115, "Invoke CIMS Web service: End")
        _hashMessage.Add(LogID.LOG01116, "Invoke CIMS Web service: Exception")
        _hashMessage.Add(LogID.LOG01117, "Read CIMS Web service result: Start")
        _hashMessage.Add(LogID.LOG01118, "Read CIMS Web service result: End")
        _hashMessage.Add(LogID.LOG01119, "Read CIMS Web service result: Exception")
        _hashMessage.Add(LogID.LOG01120, "Patient is in exception list, will not invoke CIMS Web service")
        _hashMessage.Add(LogID.LOG01121, "Generate CIMS Web service request: Start")
        _hashMessage.Add(LogID.LOG01122, "Generate CIMS Web service request: End")
        _hashMessage.Add(LogID.LOG01123, "Generate CIMS Web service request: Exception")
        '_hashMessage.Add(LogID.LOG01124, "") EHS -> CIMS request xml data
        '_hashMessage.Add(LogID.LOG01125, "") EHS -> CIMS result xml data        
		' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]
    End Sub

    Public Shared Function Instance() As AuditLogMaster
        If _udtInstance Is Nothing Then
            _udtInstance = New AuditLogMaster
        End If

        Return _udtInstance
    End Function

    Public Shared Function Messages(ByVal strLogID As String) As String
        Return Instance().GetMessages(strLogID)
    End Function

    Protected Function GetMessages(ByVal strLogID As String)
        Return _hashMessage(strLogID)
    End Function
End Class
