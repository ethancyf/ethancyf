Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports Common.PCD.WebService.Interface
Imports Common.Component

Namespace PCD

    <System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
    <System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
    <ToolboxItem(False)> _
    Public Class PCDInterface
        Inherits System.Web.Services.WebService

        Public CustomSoapHeader As ServiceAuthHeader

        <WebMethod()> _
        <SoapHeader("CustomSoapHeader")> _
        Public Function GetEHSPracticeScheme(ByVal strXMLRequest As String, ByVal strRequestSystem As String) As String

            Dim objAuditLog As AuditLogBase = CreateAuditLog()
            Dim ReadThing As New Common.PCD.WebService.Interface.EHSPracticeSchemeResult(EHSPracticeSchemeResult.enumReturnCode.SuccessWithData)
            Dim strResult As String = String.Empty

            Try
                If CustomSoapHeader Is Nothing Then
                    objAuditLog.AddDescripton("Username", String.Empty)
                    objAuditLog.AddDescripton("Password", String.Empty)
                Else
                    objAuditLog.AddDescripton("Username", CustomSoapHeader.Username)
                    objAuditLog.AddDescripton("Password", CustomSoapHeader.Password)
                End If

                objAuditLog.AddDescripton("RequestSystem", strRequestSystem)
                objAuditLog.WriteStartLog(LogID.LOG00000)

                objAuditLog.WriteLogData(LogID.LOG00001, "GetEHSPracticeScheme - PCD Request XML", strXMLRequest, Nothing, Nothing, Nothing)

                If CustomSoapHeader Is Nothing OrElse Not ServiceAuthHeaderValidation.Validate(CustomSoapHeader, strRequestSystem) Then
                    objAuditLog.WriteLog(LogID.LOG00002)
                    strResult = ReadThing.GenXMLResult(String.Empty, EHSPracticeSchemeResult.enumReturnCode.AuthenticationFailed, Nothing)
                End If

            Catch ex As Exception
                objAuditLog.WriteSystemLog(ex, Nothing)

                objAuditLog.AddDescripton(ex)
                objAuditLog.WriteLog(LogID.LOG00004)

                strResult = ReadThing.GenXMLResult(String.Empty, EHSPracticeSchemeResult.enumReturnCode.AuthenticationFailed, Nothing)
            End Try

            Try
                If ReadThing.ReturnCode = EHSPracticeSchemeResult.enumReturnCode.SuccessWithData Then
                    ' XML reader to read the request
                    ReadThing = New EHSPracticeSchemeResult(strXMLRequest)
                    strResult = ReadThing.GenResponseXML()
                End If

                objAuditLog.WriteLogData(LogID.LOG00003, "GetEHSPracticeScheme - EHS Result XML", strResult, Nothing, Nothing, ReadThing.MessageID)

                'GetEHSPracticeScheme - End
                objAuditLog.EndEvent()
                objAuditLog.AddDescripton("ReturnCode", ReadThing.ReturnCode)
                If ReadThing.Exception IsNot Nothing Then
                    objAuditLog.WriteSystemLog(ReadThing.Exception, Nothing)
                    objAuditLog.AddDescripton(ReadThing.Exception)
                End If
                objAuditLog.WriteEndLog(LogID.LOG00005, Nothing, ReadThing.MessageID)
                Return strResult
            Catch ex As Exception
                objAuditLog.WriteSystemLog(ex, Nothing)
                Return ReadThing.GenXMLResult(ReadThing.MessageID, EHSPracticeSchemeResult.enumReturnCode.ErrorAllUnexpected, Nothing)
            End Try
        End Function

        Private Function CreateAuditLog() As AuditLogBase
            Dim objAuditLog As AuditLogBase = AuditLogInterface.GetAuditLogEntry(AuditLogInterface.EnumAuditLogModule.PCDInterfaceGetEHSPracticeScheme)
            Return objAuditLog
        End Function

        Public Class ServiceAuthHeader
            Inherits SoapHeader

            Public Username As String
            Public Password As String
        End Class


        Public Class ServiceAuthHeaderValidation

            Public Shared Function Validate(ByVal soapHeader As ServiceAuthHeader, ByVal strRequestSystem As String) As Boolean

                'If soapHeader.Username = GetWSUsername() AndAlso soapHeader.Password = GetWSPassword() Then Return True
                'Return False

                If soapHeader Is Nothing Then
                    'Throw New NullReferenceException("No soap header was specified.")
                    Return False
                End If

                If soapHeader.Username Is Nothing Then
                    'Throw New NullReferenceException("Username was not supplied for authentication in SoapHeader.")
                    Return False
                End If

                If (soapHeader.Password Is Nothing) Then
                    'Throw New NullReferenceException("Password was not supplied for authentication in SoapHeader.")
                    Return False
                End If

                If (soapHeader.Username <> GetWSUsername(strRequestSystem) Or soapHeader.Password <> GetWSPassword(strRequestSystem)) Then
                    'Throw New Exception("Please pass the proper username and password for this service." + GetWSUsername() + "|" + GetWSPassword())
                    Return False
                End If

                Return True
            End Function

            Private Const _ENQUIRE_EHS_WS_USER As String = "_ENQUIRE_EHS_WS_USER"
            Private Const _ENQUIRE_EHS_WS_PASSWORD As String = "_ENQUIRE_EHS_WS_PASSWORD"

            Private Shared Function GetWSUsername(ByVal strRequestSystem As String) As String
                Dim oGenFunc As New Common.ComFunction.GeneralFunction()
                Dim sValue As String = String.Empty
                oGenFunc.getSystemParameter(strRequestSystem & _ENQUIRE_EHS_WS_USER, sValue, Nothing)
                Return sValue
            End Function


            Private Shared Function GetWSPassword(ByVal strRequestSystem As String) As String
                Dim oGenFunc As New Common.ComFunction.GeneralFunction()
                Dim sValue As String = String.Empty
                oGenFunc.getSystemParameterPassword(strRequestSystem & _ENQUIRE_EHS_WS_PASSWORD, sValue)
                Return sValue
            End Function

        End Class

    End Class

End Namespace