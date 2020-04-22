Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports ExternalInterfaceWS.Cryptography
Imports System.io
Imports ExternalInterfaceWS.ComObject

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class Service1
    Inherits System.Web.Services.WebService


    <WebMethod()> _
    Public Function RCHNameQuery(ByVal InputXML As String, ByVal SystemName As String) As String

        Try
            Return ProgramMgr.RCHNameEnquiry(InputXML, SystemName)
        Catch ex As Exception
            ErrorLogHandler.getInstance(EnumAuditLog.RCHNameQuery).WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
            Return String.Empty
        End Try

    End Function

    <WebMethod()> _
    Public Function GetReasonForVisitList(ByVal InputXML As String, ByVal SystemName As String) As String

        Try
            Return ProgramMgr.GetReasonForVisitList(InputXML, SystemName)
        Catch ex As Exception
            ErrorLogHandler.getInstance(EnumAuditLog.GetReasonForVisitList).WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
            Return String.Empty
        End Try

    End Function

    <WebMethod()> _
    Public Function SPPracticeValidation(ByVal InputXML As String, ByVal SystemName As String) As String

        Try
            Return ProgramMgr.SPPracticeValidation(InputXML, SystemName)
        Catch ex As Exception
            ErrorLogHandler.getInstance(EnumAuditLog.SPPracticeValidation).WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
            Return String.Empty
        End Try

    End Function

    <WebMethod()> _
    Public Function eHSValidatedAccountQuery(ByVal InputXML As String, ByVal SystemName As String) As String

        Try
            Return ProgramMgr.eHSValidatedAccountQuery(InputXML, SystemName)
        Catch ex As Exception
            ErrorLogHandler.getInstance(EnumAuditLog.eHSValidatedAccountQuery).WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
            Return String.Empty
        End Try

    End Function

    <WebMethod()> _
    Public Function eHSAccountVoucherQuery(ByVal InputXML As String, ByVal SystemName As String) As String

        Try
            Return ProgramMgr.eHSAccountVoucherQuery(InputXML, SystemName)
        Catch ex As Exception
            ErrorLogHandler.getInstance(EnumAuditLog.eHSAccountSubsidyQuery).WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
            Return String.Empty
        End Try

    End Function


    <WebMethod()> _
    Public Function UploadClaim(ByVal InputXML As String, ByVal SystemName As String) As String

        Try
            Return ProgramMgr.UploadClaim_HL7(InputXML, SystemName)
        Catch ex As Exception
            ErrorLogHandler.getInstance(EnumAuditLog.UploadClaim).WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
            Return String.Empty
        End Try

    End Function



End Class
