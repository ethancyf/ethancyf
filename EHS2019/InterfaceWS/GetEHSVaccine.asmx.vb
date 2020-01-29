Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Xml
Imports InterfaceWS.EHSVaccination
Imports Common.ComObject
Imports Common.ComFunction
Imports Common.Component
Imports System.IO

Namespace EHSVaccination

    <System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
    <System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
    <ToolboxItem(False)> _
    Public Class GetEHSVaccine
        Inherits System.Web.Services.WebService

#Region "Constants"
        Public Class REQUEST_SYSTEM
            Public Const CMS As String = "CMS"
            Public Const CIMS As String = "CIMS"
        End Class

#End Region

        Public CustomSoapHeader As ServiceAuthHeader
        Public objAuditLog As AuditLogEntry = ComFunction.GetAuditLogEntry()

        <WebMethod()> _
        <SoapHeader("CustomSoapHeader")> _
        Public Function geteHSVaccineRecord(ByVal xmlRequest As String) As String
            'CRE14-002 PPIEPF Migration [Start][Karl]  -- eVaccination text file log feature (Log CMS enquiry information) is replaced by audit log and no longer useful
            'LogRequest(xmlRequest)
            'CRE14-002 PPIEPF Migration [End][Karl]  

            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
            ' ----------------------------------------------------------
            Dim request As CMSRequest = Nothing
            Dim result As eHSResult = New eHSResult(True) ' Default error result
            ' CRE18-004 (CIMS Vaccination Sharing) [End][Koala CHENG]

            Dim objAuditLog As AuditLogBase = CreateAuditLog()
            Dim strRequestSystem As String = String.Empty

            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Dim xml As XmlDocument = Nothing
            ' CRE18-001(CIMS Vaccination Sharing) [End][Chris YIM]

            Dim strUserID As String = String.Empty

            
            Try

                ' WebMethod geteHSVaccineRecord Start
                If (CustomSoapHeader Is Nothing) Then
                    objAuditLog.WriteStartLog(LogID.LOG00000, "")
                Else
                    objAuditLog.WriteStartLog(LogID.LOG00000, CustomSoapHeader.Username)
                End If

                Try
                    If ServiceAuthHeaderValidation.Validate(CustomSoapHeader, strRequestSystem) Then
                        strUserID = CustomSoapHeader.Username
                        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
                        ' ----------------------------------------------------------
                        objAuditLog.SetRequestSystem(strRequestSystem)
                        ' CRE18-004 (CIMS Vaccination Sharing) [End][Koala CHENG]

                        ' Login Success
                        objAuditLog.AddDescripton("Username", CustomSoapHeader.Username)
                        objAuditLog.AddDescripton("Password", CustomSoapHeader.Password)
                        objAuditLog.WriteLog(LogID.LOG00002)
                    End If
                Catch ex As Exception
                    ' CMS Request
                    objAuditLog.WriteLogData(LogID.LOG00004, xmlRequest)

                    ' WebMethod geteHSVaccineRecord Error
                    objAuditLog.AddDescripton(ex)
                    ' INT11-0004 (Obsolete: Remove meaningless log)
                    'objAuditLog.WriteEndLog(LogID.LOG00011) 

                    ' Login Fail
                    If Not CustomSoapHeader Is Nothing Then
                        objAuditLog.AddDescripton("Username", CustomSoapHeader.Username)
                        objAuditLog.AddDescripton("Password", CustomSoapHeader.Password)
                    End If
                    objAuditLog.WriteLog(LogID.LOG00001)

                    xml = result.GenErrorXMLResult
                    ' EHS Result
                    objAuditLog.WriteLogData(LogID.LOG00005, xml.InnerXml)

                    ' CRE18-004 (CIMS Vaccination Sharing) [Start][Winnie SUEN]
                    ' ----------------------------------------------------------
                    ' Write System Log
                    objAuditLog.WriteSystemLog(ex, strUserID)
                    ' CRE18-004 (CIMS Vaccination Sharing) [End][Winnie SUEN]

                    ' WebMethod geteHSVaccineRecord End
                    objAuditLog.EndEvent()
                    ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
                    ' ----------------------------------------------------------
                    AddResultDescription(objAuditLog, request, result)
                    ' CRE18-004 (CIMS Vaccination Sharing) [End][Koala CHENG]
                    objAuditLog.WriteEndLog(LogID.LOG00003)

                    Return xml.InnerXml
                End Try


                ' CMS Request
                objAuditLog.WriteLogData(LogID.LOG00004, xmlRequest)

                ' Read CMS Request
                objAuditLog.WriteLog(LogID.LOG00006)

                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
                ' ----------------------------------------------------------
                request = New CMSRequest(xmlRequest, strRequestSystem)
                'Dim request As New CMSRequest(xmlRequest, strRequestSystem)
                ' CRE18-004 (CIMS Vaccination Sharing) [End][Koala CHENG]

                If Not request.IsValid Then
                    ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
                    ' ----------------------------------------------------------
                    objAuditLog.WriteSystemLog(request.Exception, String.Empty)
                    ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

                    ' Read CMS Request Error
                    objAuditLog.AddDescripton(request.Exception)
                    objAuditLog.WriteLog(LogID.LOG00007)
                End If

                ' Process CMS Request
                objAuditLog.WriteLog(LogID.LOG00008)

                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
                ' ----------------------------------------------------------
                result = New eHSResult
                ' Dim result As New eHSResult
                ' CRE18-004 (CIMS Vaccination Sharing) [End][Koala CHENG]

                If Not result.ProcessRequest(request) Then
                    ' Process CMS Request Error
                    If result.Exception IsNot Nothing Then
                        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
                        ' ----------------------------------------------------------
                        objAuditLog.WriteSystemLog(result.Exception, String.Empty)
                        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

                        objAuditLog.AddDescripton(result.Exception)
                        ' INT11-0004 (logging only on exception)
                        objAuditLog.WriteLog(LogID.LOG00009)
                    End If
                End If

                ' Render EHS Result
                objAuditLog.WriteLog(LogID.LOG00010)
                xml = result.GenXMLResult()

                ' EHS Result
                objAuditLog.WriteLogData(LogID.LOG00005, xml.InnerXml)
                ' WebMethod geteHSVaccineRecord End
                objAuditLog.EndEvent()
                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
                ' ----------------------------------------------------------
                AddResultDescription(objAuditLog, request, result)
                ' CRE18-004 (CIMS Vaccination Sharing) [End][Koala CHENG]
                objAuditLog.WriteEndLog(LogID.LOG00003)

                Return xml.InnerXml
            Catch ex As Exception
                ' WebMethod geteHSVaccineRecord Error
                objAuditLog.AddDescripton(ex)
                objAuditLog.WriteEndLog(LogID.LOG00011)

                objAuditLog.WriteSystemLog(ex, strUserID)

                ' EHS Result
                objAuditLog.WriteLogData(LogID.LOG00005, xml.InnerXml)

                ' WebMethod geteHSVaccineRecord End
                objAuditLog.EndEvent()
                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
                ' ----------------------------------------------------------
                AddResultDescription(objAuditLog, request, result)
                ' CRE18-004 (CIMS Vaccination Sharing) [End][Koala CHENG]
                objAuditLog.WriteEndLog(LogID.LOG00003)

                Return xml.InnerXml
            End Try
        End Function

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
        ' ----------------------------------------------------------
        ''' <summary>
        ''' Add request and result information to Audit Log Description
        ''' </summary>
        ''' <param name="objAuditLog"></param>
        ''' <param name="objRequest"></param>
        ''' <param name="objResult"></param>
        ''' <remarks></remarks>
        Private Sub AddResultDescription(ByVal objAuditLog As AuditLogBase, ByVal objRequest As CMSRequest, ByVal objResult As eHSResult)
            If objRequest IsNot Nothing AndAlso objRequest.IsValid Then
                objAuditLog.AddDescripton("RequestSystem", objRequest.RequestSystem)
                objAuditLog.AddDescripton("MessageID", objRequest.MessageID)
                objAuditLog.AddDescripton("HealthCheck", IIf(objRequest.HealthCheck, "Y", "N"))
                objAuditLog.AddDescripton("BatchEnquiry", IIf(objRequest.PatientCount > 1, "Y", "N"))
                objAuditLog.AddDescripton("ReturnCode", objResult.ReturnCode)
                objAuditLog.AddDescripton("Success", IIf(objResult.IsSuccessReturnCode, "Y", "N"))
            Else
                objAuditLog.AddDescripton("RequestSystem", "Unknown")
                objAuditLog.AddDescripton("MessageID", "Unknown")
                objAuditLog.AddDescripton("HealthCheck", "Unknown")
                objAuditLog.AddDescripton("BatchEnquiry", "Unknown")
                objAuditLog.AddDescripton("ReturnCode", objResult.ReturnCode)
                objAuditLog.AddDescripton("Success", "N")
            End If
        End Sub
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Koala CHENG]

        Private Function CreateAuditLog() As AuditLogBase
            Dim objAuditLog As AuditLogBase = AuditLogInterface.GetAuditLogEntry(AuditLogInterface.EnumAuditLogModule.EHSVaccination)
            Return objAuditLog
        End Function

        Public Class ServiceAuthHeader
            Inherits SoapHeader

            Public Username As String
            Public Password As String
        End Class

        Public Class ServiceAuthHeaderValidation

            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Public Shared Function Validate(ByVal soapHeader As ServiceAuthHeader, ByRef strRequestSystem As String) As Boolean

                strRequestSystem = String.Empty

                'If soapHeader.Username = GetWSUsername() AndAlso soapHeader.Password = GetWSPassword() Then Return True
                'Return False

                If soapHeader Is Nothing Then
                    Throw New NullReferenceException("No soap header was specified.")
                End If


                If soapHeader.Username Is Nothing Then
                    Throw New NullReferenceException("Username was not supplied for authentication in SoapHeader.")
                End If

                If (soapHeader.Password Is Nothing) Then
                    Throw New NullReferenceException("Password was not supplied for authentication in SoapHeader.")
                End If


                Dim blnAuthResult As Boolean = False
                If (soapHeader.Username = GetWSUsernameCMS() And soapHeader.Password = GetWSPasswordCMS()) Then
                    blnAuthResult = True
                    strRequestSystem = REQUEST_SYSTEM.CMS
                End If

                If Not blnAuthResult Then
                    If (soapHeader.Username = GetWSUsernameCIMS() And soapHeader.Password = GetWSPasswordCIMS()) Then
                        blnAuthResult = True
                        strRequestSystem = REQUEST_SYSTEM.CIMS
                    End If
                End If

                If Not blnAuthResult Then
                    Throw New Exception("Please pass the proper username and password for this service.")
                End If

                Return True

            End Function
            ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

#Region "Get System Parameter"

            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Private Const SYS_PARAM_USERNAME_CMS As String = "EHS_Get_Vaccine_WS_Username_CMS"
            Private Const SYS_PARAM_PASSWORD_CMS As String = "EHS_Get_Vaccine_WS_Password_CMS"
            Private Const SYS_PARAM_USERNAME_CIMS As String = "EHS_Get_Vaccine_WS_Username_CIMS"
            Private Const SYS_PARAM_PASSWORD_CIMS As String = "EHS_Get_Vaccine_WS_Password_CIMS"
            ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

            Private Shared Function GetWSUsernameCMS() As String
                Dim oGenFunc As New GeneralFunction()
                Dim sValue As String = String.Empty
                oGenFunc.getSystemParameter(SYS_PARAM_USERNAME_CMS, sValue, Nothing)
                Return sValue
            End Function

            Private Shared Function GetWSPasswordCMS() As String
                Dim oGenFunc As New GeneralFunction()
                Dim sValue As String = String.Empty
                oGenFunc.getSystemParameterPassword(SYS_PARAM_PASSWORD_CMS, sValue)
                Return sValue
            End Function

            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Private Shared Function GetWSUsernameCIMS() As String
                Dim oGenFunc As New GeneralFunction()
                Dim sValue As String = String.Empty
                oGenFunc.getSystemParameter(SYS_PARAM_USERNAME_CIMS, sValue, Nothing)
                Return sValue
            End Function

            Private Shared Function GetWSPasswordCIMS() As String
                Dim oGenFunc As New GeneralFunction()
                Dim sValue As String = String.Empty
                oGenFunc.getSystemParameterPassword(SYS_PARAM_PASSWORD_CIMS, sValue)
                Return sValue
            End Function
            ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

#End Region

        End Class


    End Class

End Namespace