Imports Microsoft.VisualBasic
Imports Microsoft.Web.Services3.Design
Imports System.Data
Imports System.Data.SqlClient
Imports System.Xml
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security
Imports Common.Format
Imports Common.DataAccess
Imports Common.ComObject
Imports Common.Component
Imports Common.ComFunction
Imports Common.Component.DocType
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.EHSTransaction
Imports Common.Component.HATransaction

Namespace WebService.Interface

    ''' <summary>
    ''' A class act as middle tier to handle communication between eHS and HA by web service
    ''' </summary>
    ''' <remarks></remarks>
    Public Class WSProxyCMS

#Region "Constants"
        Private Const SYS_PARAM_ENDPOINT As String = "CMS_Get_Vaccine_WS_Endpoint"

        Private Const SYS_PARAM_WEBLOGIC_URL As String = "CMS_Get_Vaccine_WS_WEBLOGIC_Url"
        Private Const SYS_PARAM_WEBLOGIC_USERNAME As String = "CMS_Get_Vaccine_WS_WEBLOGIC_Username"
        Private Const SYS_PARAM_WEBLOGIC_PASSWORD As String = "CMS_Get_Vaccine_WS_WEBLOGIC_Password"
        Private Const SYS_PARAM_WEBLOGIC_TIMEOUT As String = "CMS_Get_Vaccine_WS_WEBLOGIC_TimeLimit"
        Private Const SYS_PARAM_WEBLOGIC_USE_PROXY As String = "CMS_Get_Vaccine_WS_WEBLOGIC_Use_Proxy"

        Private Const SYS_PARAM_EMULATE_URL As String = "CMS_Get_Vaccine_WS_EMULATE_Url"
        Private Const SYS_PARAM_EMULATE_USERNAME As String = "CMS_Get_Vaccine_WS_EMULATE_Username"
        Private Const SYS_PARAM_EMULATE_PASSWORD As String = "CMS_Get_Vaccine_WS_EMULATE_Password"
        Private Const SYS_PARAM_EMULATE_TIMEOUT As String = "CMS_Get_Vaccine_WS_EMULATE_TimeLimit"
        Private Const SYS_PARAM_EMULATE_USE_PROXY As String = "CMS_Get_Vaccine_WS_EMULATE_Use_Proxy"

        Private Const SYS_PARAM_EAIWSPROXY_URL As String = "CMS_Get_Vaccine_WS_EAIWSProxy_Url"
        Private Const SYS_PARAM_EAIWSPROXY_USERNAME As String = "CMS_Get_Vaccine_WS_EAIWSProxy_Username"
        Private Const SYS_PARAM_EAIWSPROXY_PASSWORD As String = "CMS_Get_Vaccine_WS_EAIWSProxy_Password"
        Private Const SYS_PARAM_EAIWSPROXY_TIMEOUT As String = "CMS_Get_Vaccine_WS_EAIWSProxy_TimeLimit"
        Private Const SYS_PARAM_EAIWSPROXY_USE_PROXY As String = "CMS_Get_Vaccine_WS_EAIWSPROXY_Use_Proxy"

        Private Const SYS_PARAM_PATIENTLIMIT As String = "CMS_Get_Vaccine_WS_{0}_PatientLimit"

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Const SYS_PARAM_CMS_ID_NO_EXCEPTION As String = "CMS_Get_Vaccine_WS_ID_No_Exception"
        Private Const SYS_PARAM_CMS_WS_VER As String = "CMS_Get_Vaccine_WS_Version"

        Public Enum CMS_XML_Version
            ONE = 1
            TWO = 2
        End Enum
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ''' <summary>
        ''' CRE11-002
        ''' Type of web service endpoint site, primary/secondary
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum enumEndpointSite
            Primary
            Secondary
        End Enum

#End Region

#Region "Private Members"
        Private _udtAuditLogEntry As AuditLogEntry
        Private _udtGenFunc As New GeneralFunction()

#End Region

#Region "Constructors"
        Public Sub New(ByVal udtAuditLogEntry As AuditLogEntry)
            Me._udtAuditLogEntry = udtAuditLogEntry
        End Sub
#End Region

#Region "Function: Call HA CMS web service"
        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        ''' <summary>
        ''' Retrieve HA vaccination record through CMS web service by patient ID, english name, DOB, gender
        ''' </summary>
        ''' <param name="udtEHSAccount">Account information</param>
        ''' <returns>Object represent CMS result</returns>
        ''' <remarks></remarks>
        Public Function GetVaccine(ByVal udtEHSAccount As EHSAccountModel) As HAVaccineResult
            Dim udtEHSPersonalInfo As EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)

            If VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.Y Then
                Return New HAVaccineResult(HAVaccineResult.enumReturnCode.VaccinationRecordOff)
            End If

            If IsDocNoInExceptionList(udtEHSPersonalInfo.IdentityNum) Then
                Me._udtAuditLogEntry.AddDescripton("Doc No.", udtEHSPersonalInfo.IdentityNum)
                Me._udtAuditLogEntry.WriteLog(LogID.LOG01020) ' Patient is in exception list, will not invoke CMS Web service

                Return HAVaccineResult.CustomHAVaccineResultForDocNoException

            End If

            'Only use one document info. to get vaccine
            Dim udtEHSPersonalInformationList As New EHSPersonalInformationModelCollection
            udtEHSPersonalInformationList.Add(udtEHSPersonalInfo)

            Return GetVaccine(udtEHSPersonalInformationList)

        End Function
        ' CRE18-004(CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        ''' <summary>
        ''' Retrieve HA vaccination record through CMS web service by patient ID, english name, DOB, gender
        ''' </summary>
        ''' <param name="udtRequestPatientList">Personal information</param>
        ''' <returns>Object represent CMS result</returns>
        ''' <remarks></remarks>
        Public Function GetVaccine(ByVal udtRequestPatientList As EHSPersonalInformationModelCollection) As HAVaccineResult

            Dim xmlRequest As XmlDocument
            Dim objXmlGenerator As XmlGenerator.WSVaccineQueryRequestXmlGenerator = Nothing

            ' ------------------------------------------------------------------------------ 
            ' Generate XML request to enquiry HA CMS vaccination record
            ' ------------------------------------------------------------------------------ 
            _udtAuditLogEntry.AddDescripton("Return Data", "Get from CMS")
            Me._udtAuditLogEntry.WriteLog(LogID.LOG01021) ' Generate CMS Web service request: Start
            Try
                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
                ' ----------------------------------------------------------
                ' Convert personal information to xml request
                objXmlGenerator = New XmlGenerator.WSVaccineQueryRequestXmlGenerator(udtRequestPatientList, GetCMSWSVersion())
                ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

                xmlRequest = objXmlGenerator.Convert
                ' Log request xml
                _udtAuditLogEntry.WriteLog(LogID.LOG01024, xmlRequest.InnerXml)
            Catch ex As Exception
                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Winnie SUEN]
                ' ----------------------------------------------------------
                ' Add System Log
                ErrorHandler.Log(_udtAuditLogEntry.FunctionCode, SeverityCode.SEVE, "99999", _
                                 HttpContext.Current.Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, _
                                 String.Format("(EHS -> CMS) Vaccination enquiry request generation fail. {0}", ex.ToString))
                ' CRE18-004 (CIMS Vaccination Sharing) [End][Winnie SUEN]

                Me._udtAuditLogEntry.AddDescripton(ex)
                Me._udtAuditLogEntry.WriteLog(LogID.LOG01022) ' Generate CMS Web service request: Exception
                ' TODO: Log exception

                Return New HAVaccineResult(HAVaccineResult.enumReturnCode.InternalError, ex.ToString)

            End Try
            Me._udtAuditLogEntry.WriteLog(LogID.LOG01022) ' Generate CMS Web service request: End


            ' ------------------------------------------------------------------------------ 
            ' Call CMS web service to query HA vaccination record (xml)
            ' ------------------------------------------------------------------------------
            Me._udtAuditLogEntry.AddDescripton("Doc Code", udtRequestPatientList(0).DocCode)
            Me._udtAuditLogEntry.AddDescripton("Doc No.", udtRequestPatientList(0).IdentityNum)

            ' INT11-0021
            ' Add message id as description in  auditlog
            Me._udtAuditLogEntry.AddDescripton("Message ID", objXmlGenerator.MessageID)
            Me._udtAuditLogEntry.WriteLog(LogID.LOG01014) ' Invoke CMS Web service: Start

            ' CRE18-004 Add InterfaceLog
            Dim udtAuditLogInterface As AuditLogBase = AuditLogInterface.GetAuditLogEntry(AuditLogInterface.EnumAuditLogModule.EVACC_CMS)
            udtAuditLogInterface.SetCallSystem(VaccinationBLL.VaccineRecordSystem.CMS.ToString)

            Dim udtResult As HAVaccineResult = Nothing
            Dim sResult As String = String.Empty
            Try
                ' bypass cert validation on callback
                InitServicePointManager()

                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Winnie SUEN]
                ' ----------------------------------------------------------
                udtAuditLogInterface.AddDescripton("Site", enumEndpointSite.Primary.ToString)
                udtAuditLogInterface.AddDescripton("Link", GetWS_Url(enumEndpointSite.Primary))
                udtAuditLogInterface.WriteStartLog(LogID.LOG00001, Nothing, objXmlGenerator.MessageID)
                udtAuditLogInterface.WriteLogData(LogID.LOG00002, xmlRequest.InnerXml)
                ' CRE18-004 (CIMS Vaccination Sharing) [End][Winnie SUEN]

                ' CRE11-002
                ' Connect OC4J or WebLogic Endpoint by system parameter setting
                sResult = GetVaccineInvoke(xmlRequest.InnerXml)

                ' Call CMS web service (DMZ server)
                'sResult = CreateWebService().getCmsVaccination(xmlRequest.InnerXml)

                ' Log result xml
                _udtAuditLogEntry.WriteLog(LogID.LOG01025, sResult)

                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Winnie SUEN]
                ' ----------------------------------------------------------
                'Add Result to Interface log
                udtAuditLogInterface.WriteLogData(LogID.LOG00003, sResult)

                AddRequestDescription(udtAuditLogInterface, objXmlGenerator.MessageID, udtRequestPatientList)
                udtAuditLogInterface.WriteEndLog(LogID.LOG00005)
                ' CRE18-004 (CIMS Vaccination Sharing) [End][Winnie SUEN]

            Catch exWeb As System.Net.WebException
                ' INT11-0021
                ' Add message id as description in  auditlog
                Me._udtAuditLogEntry.AddDescripton("Message ID", objXmlGenerator.MessageID)
                Me._udtAuditLogEntry.AddDescripton("StackTrace", exWeb.Message)
                Me._udtAuditLogEntry.WriteLog(LogID.LOG01016) ' Invoke CMS Web service: Exception

                ' Log result xml (empty string on exception)
                _udtAuditLogEntry.WriteLog(LogID.LOG01025, String.Empty)

                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Winnie SUEN]
                ' ----------------------------------------------------------
                'Add Result to Interface log
                udtAuditLogInterface.WriteLogData(LogID.LOG00003, sResult)

                AddRequestDescription(udtAuditLogInterface, objXmlGenerator.MessageID, udtRequestPatientList)
                udtAuditLogInterface.AddDescripton("Exception", exWeb.ToString)
                udtAuditLogInterface.WriteEndLog(LogID.LOG00004)

                '' Add System Log
                'If HttpContext.Current IsNot Nothing Then
                '    ErrorHandler.Log(_udtAuditLogEntry.FunctionCode, SeverityCode.SEVE, "99999", _
                '                HttpContext.Current.Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, _
                '                String.Format("(EHS -> CMS) Vaccination enquiry fail with exception. {0}", exWeb.ToString))
                'Else
                '    ErrorHandler.Log(_udtAuditLogEntry.FunctionCode, SeverityCode.SEVE, "99999", _
                '                Environment.CommandLine, String.Empty, _
                '                String.Format("(EHS -> CMS) Vaccination enquiry fail with exception. {0}", exWeb.ToString))
                'End If
               
                ' CRE18-004 (CIMS Vaccination Sharing) [End][Winnie SUEN]

                ' TODO: Log exception
                udtResult = New HAVaccineResult(HAVaccineResult.enumReturnCode.CommunicationLinkError, exWeb.ToString, objXmlGenerator.MessageID)
                udtResult.Request = xmlRequest.InnerXml

                Return udtResult
            Catch ex As Exception
                ' INT11-0021
                ' Add message id as description in  auditlog
                Me._udtAuditLogEntry.AddDescripton("Message ID", objXmlGenerator.MessageID)
                Me._udtAuditLogEntry.AddDescripton(ex)
                Me._udtAuditLogEntry.WriteLog(LogID.LOG01016) ' Invoke CMS Web service: Exception

                ' Log result xml (empty string on exception)
                _udtAuditLogEntry.WriteLog(LogID.LOG01025, String.Empty)

                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Winnie SUEN]
                ' ----------------------------------------------------------
                'Add Result to Interface log
                udtAuditLogInterface.WriteLogData(LogID.LOG00003, sResult)

                AddRequestDescription(udtAuditLogInterface, objXmlGenerator.MessageID, udtRequestPatientList)
                udtAuditLogInterface.AddDescripton("Exception", ex.ToString)
                udtAuditLogInterface.WriteEndLog(LogID.LOG00004)

                '' Add System Log
                'If HttpContext.Current IsNot Nothing Then
                '    ErrorHandler.Log(_udtAuditLogEntry.FunctionCode, SeverityCode.SEVE, "99999", _
                '                     HttpContext.Current.Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, _
                '                     String.Format("(EHS -> CMS) Vaccination enquiry fail with exception. {0}", ex.ToString))
                'Else
                '    ErrorHandler.Log(_udtAuditLogEntry.FunctionCode, SeverityCode.SEVE, "99999", _
                '                    Environment.CommandLine, String.Empty, _
                '                    String.Format("(EHS -> CMS) Vaccination enquiry fail with exception. {0}", ex.ToString))
                'End If
                ' CRE18-004 (CIMS Vaccination Sharing) [End][Winnie SUEN]

                ' TODO: Log exception
                udtResult = New HAVaccineResult(HAVaccineResult.enumReturnCode.InternalError, ex.ToString, objXmlGenerator.MessageID)
                udtResult.Request = xmlRequest.InnerXml

                Return udtResult
            End Try

            ' INT11-0021
            ' Add message id as description in  auditlog
            Me._udtAuditLogEntry.AddDescripton("Message ID", objXmlGenerator.MessageID)
            Me._udtAuditLogEntry.WriteLog(LogID.LOG01015) ' Invoke CMS Web service: End


            ' ------------------------------------------------------------------------------ 
            ' Convert xml result to object and return
            ' ------------------------------------------------------------------------------
            Me._udtAuditLogEntry.WriteLog(LogID.LOG01017) ' Read CMS Web service result: Start
            Try
                ' Convert xml result to object and return
                udtResult = New HAVaccineResult(sResult, objXmlGenerator.MessageID) ' CRE10-035: Handle message ID mismatch case
                udtResult.Request = xmlRequest.InnerXml
            Catch ex As Exception
                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Winnie SUEN]
                ' ----------------------------------------------------------
                ' Add System Log
                ErrorHandler.Log(_udtAuditLogEntry.FunctionCode, SeverityCode.SEVE, "99999", _
                                 HttpContext.Current.Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, _
                                 String.Format("(EHS -> CMS) Vaccination enquiry convert result fail. {0}", ex.ToString))
                ' CRE18-004 (CIMS Vaccination Sharing) [End][Winnie SUEN]

                Me._udtAuditLogEntry.AddDescripton(ex)
                Me._udtAuditLogEntry.WriteLog(LogID.LOG01019) ' Read CMS Web service result: Exception

                ' TODO: Log exception
                udtResult = New HAVaccineResult(HAVaccineResult.enumReturnCode.InternalError, ex.ToString, objXmlGenerator.MessageID)
                udtResult.Request = xmlRequest.InnerXml
            End Try

            Me._udtAuditLogEntry.WriteLog(LogID.LOG01018) ' Read CMS Web service result: End

            Return udtResult
        End Function
        ' CRE18-001(CIMS Vaccination Sharing) [End][Chris YIM]

        ''' <summary>
        ''' CRE11-002
        ''' Invoke web service endpoint (primary site)
        ''' </summary>
        ''' <param name="strXmlRequest">XML request for enquiry CMS vaccination</param>
        ''' <returns>CMS xml result</returns>
        ''' <remarks></remarks>
        Public Function GetVaccineInvoke(ByVal strXmlRequest As String) As String
            Return GetVaccineInvoke(strXmlRequest, enumEndpointSite.Primary)
        End Function

        ''' <summary>
        ''' CRE11-002
        ''' Invoke web service endpoint (primary/secondary site)
        ''' </summary>
        ''' <param name="strXmlRequest">XML request for enquiry CMS vaccination</param>
        ''' <param name="eEndpointSite">Use primary/secondary site for invoke</param>
        ''' <returns>CMS xml result</returns>
        ''' <remarks></remarks>
        Public Function GetVaccineInvoke(ByVal strXmlRequest As String, ByVal eEndpointSite As enumEndpointSite) As String
            Dim strResult As String = String.Empty

            ' Connect OC4J or WebLogic Endpoint by system parameter setting
            Select Case GetWSEndpointType()
                'CRE14-020 New EAI infrastructure [Start][Karl]
                'Case EndpointEnum.OC4J
                Case EndpointEnum.EMULATE
                    ' Call EAI EndPoint web service OC4J
                    strResult = CreateWebServiceEndPoint_Emulate(eEndpointSite).submitTextMessage(strXmlRequest)
                    'strResult = CreateWebServiceEndPoint_OC4J(eEndpointSite).submitTextMessage(strXmlRequest)
                    'CRE14-020 New EAI infrastructure [End][Karl]
                Case EndpointEnum.WEBLOGIC
                    ' Call EAI EndPoint web service WebLogic
                    strResult = CreateWebServiceEndPoint_WebLogic(eEndpointSite).submitTextMessage(strXmlRequest)

                    'CRE14-020 New EAI infrastructure [Start][Karl]
                Case EndpointEnum.EAIWSPROXY
                    strResult = CreateWebServiceEndPoint_EAIWSProxy(eEndpointSite).getCmsVaccination(strXmlRequest)
                    'CRE14-020 New EAI infrastructure [End][Karl]
                Case Else
                    Throw New Exception(String.Format("Not supported (EHS->CMS) web service endpoint type({0}))", GetWSEndpointType()))
            End Select

            Return strResult
        End Function

        ''' <summary>
        ''' CRE11-002
        ''' Invoke web service endpoint (Specified URL)
        ''' </summary>
        ''' <param name="strXmlRequest">XML request for enquiry CMS vaccination</param>
        ''' <param name="strURL">Specified web service URL</param>
        ''' <returns>CMS xml result</returns>
        ''' <remarks></remarks>
        Public Function GetVaccineInvoke(ByVal strXmlRequest As String, ByVal strURL As String, Optional enumSpecificEndPoint As Nullable(Of EndpointEnum) = Nothing) As String
            'CRE14-020 New EAI infrastructure [Start][Karl]
            'Public Function GetVaccineInvoke(ByVal strXmlRequest As String, ByVal strURL As String) As String
            'CRE14-020 New EAI infrastructure [End][Karl]
            Dim strResult As String = String.Empty

            'CRE14-020 New EAI infrastructure [Start][Karl]
            If enumSpecificEndPoint.HasValue = False Then
                enumSpecificEndPoint = GetWSEndpointType()
            End If
            'CRE14-020 New EAI infrastructure [End][Karl]

            ' Connect OC4J or WebLogic Endpoint by system parameter setting
            'CRE14-020 New EAI infrastructure [Start][Karl]
            'Select Case GetWSEndpointType()
            Select Case enumSpecificEndPoint
                'Case EndpointEnum.OC4J
                Case EndpointEnum.EMULATE
                    ' Call EAI EndPoint web service OC4J
                    'strResult = CreateWebServiceEndPoint_OC4J(strURL).submitTextMessage(strXmlRequest)
                    strResult = CreateWebServiceEndPoint_Emulate(strURL).submitTextMessage(strXmlRequest)
                    'CRE14-020 New EAI infrastructure [End][Karl]
                Case EndpointEnum.WEBLOGIC
                    ' Call EAI EndPoint web service WebLogic
                    strResult = CreateWebServiceEndPoint_WebLogic(strURL).submitTextMessage(strXmlRequest)

                    'CRE14-020 New EAI infrastructure [Start][Karl]
                Case EndpointEnum.EAIWSPROXY
                    strResult = CreateWebServiceEndPoint_EAIWSProxy(strURL).getCmsVaccination(strXmlRequest)
                    'CRE14-020 New EAI infrastructure [End][Karl]
                Case Else
                    Throw New Exception(String.Format("Not supported (EHS->CMS) web service endpoint type({0}))", GetWSEndpointType()))
            End Select

            Return strResult
        End Function
        'CRE14-020 New EAI infrastructure [Start][Karl]
        ' ''' <summary>
        ' ''' Common function for create web service to process request
        ' ''' </summary>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Private Function CreateWebService() As ImmuEnquiryWebS

        '    Dim ws As ImmuEnquiryWebS = New ImmuEnquiryWebS
        '    ws.Url = GetWS_OC4J_Url(enumEndpointSite.Primary)
        '    ws.Credentials = New System.Net.NetworkCredential(GetWS_OC4J_Username(), GetWS_OC4J_Password())

        '    ws.Url = ws.Url
        '    ws.Timeout = GetWS_OC4J_Timeout()
        '    Return ws
        'End Function
        'CRE14-020 New EAI infrastructure [End][Karl]
        ''' <summary>
        ''' CRE11-002
        ''' Common function for create web service (emulate, Soap 1.1) to process request
        ''' </summary>
        ''' <param name="eEndpointSite">Use primary/secondary site URL for connect endpoint</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        '''     'CRE14-020 New EAI infrastructure [Start][Karl]
        Private Function CreateWebServiceEndPoint_Emulate(ByVal eEndpointSite As enumEndpointSite) As ImmuEnquiryWebS_EndPoint
            Return CreateWebServiceEndPoint_Emulate(GetWS_EMULATE_Url(eEndpointSite))
        End Function

        'Private Function CreateWebServiceEndPoint_OC4J(ByVal eEndpointSite As enumEndpointSite) As ImmuEnquiryWebS_EndPoint
        '    Return CreateWebServiceEndPoint_OC4J(GetWS_OC4J_Url(eEndpointSite))
        'End Function
        'CRE14-020 New EAI infrastructure [End][Karl]
        ''' <summary>
        ''' CRE11-002
        ''' Common function for create web service (emulate, Soap 1.1) to process request
        ''' </summary>
        ''' <param name="strURL">Specified web service URL</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        '''    'CRE14-020 New EAI infrastructure [Start][Karl]
        Private Function CreateWebServiceEndPoint_Emulate(ByVal strURL As String) As ImmuEnquiryWebS_EndPoint
            'Private Function CreateWebServiceEndPoint_OC4J(ByVal strURL As String) As ImmuEnquiryWebS_EndPoint
            'CRE14-020 New EAI infrastructure [End][Karl]
            Dim ws As ImmuEnquiryWebS_EndPoint = New ImmuEnquiryWebS_EndPoint
            ws.Url = strURL

            ' Create Client Policy (Specify that the policy uses the username over transport security assertion)
            'CRE14-020 New EAI infrastructure [Start][Karl]
            'Dim strUserName As String = GetWS_OC4J_Username()
            'Dim strPassword As String = GetWS_OC4J_Password()
            Dim strUserName As String = GetWS_EMULATE_Username()
            Dim strPassword As String = GetWS_EMULATE_Password()
            'CRE14-020 New EAI infrastructure [End][Karl]
            If strUserName <> String.Empty And strPassword <> String.Empty Then
                Dim oClientPolicy As New Microsoft.Web.Services3.Design.Policy()
                Dim oAssertion As UsernameOverTransportAssertion = New UsernameOverTransportAssertion()
                oAssertion.UsernameTokenProvider = New Microsoft.Web.Services3.Design.UsernameTokenProvider(strUserName, strPassword)
                oClientPolicy.Assertions.Add(oAssertion)
                ws.SetPolicy(oClientPolicy)

                ' Use windows proxy for access endpoint
                'CRE14-020 New EAI infrastructure [Start][Karl]
                ' If GetWS_OC4J_UseProxy() Then
                If GetWS_EMULATE_UseProxy() Then
                    'CRE14-020 New EAI infrastructure [End][Karl]
                    ws.Proxy = New System.Net.WebProxy()
                End If
            End If
            'CRE14-020 New EAI infrastructure [Start][Karl]
            ws.Timeout = GetWS_EMULATE_Timeout()
            'ws.Timeout = GetWS_OC4J_Timeout()
            'CRE14-020 New EAI infrastructure [End][Karl]
            Return ws
        End Function

        ''' <summary>
        ''' CRE11-002
        ''' Common function for create web service (WebLogic, Soap 1.2) to process request
        ''' </summary>
        ''' <param name="eEndpointSite">Use primary/secondary site URL for connect endpoint</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function CreateWebServiceEndPoint_WebLogic(ByVal eEndpointSite As enumEndpointSite) As ImmuEnquiryWebS_EndPoint_WebLogic
            Return CreateWebServiceEndPoint_WebLogic(GetWS_WEBLOGIC_Url(eEndpointSite))
        End Function

        ''' <summary>
        ''' CRE11-002
        ''' Common function for create web service (WebLogic, Soap 1.2) to process request
        ''' </summary>
        ''' <param name="strURL">Specified web service URL</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function CreateWebServiceEndPoint_WebLogic(ByVal strURL As String) As ImmuEnquiryWebS_EndPoint_WebLogic
            Dim ws As ImmuEnquiryWebS_EndPoint_WebLogic = New ImmuEnquiryWebS_EndPoint_WebLogic
            ws.Url = strURL

            ' Create Client Policy (Specify that the policy uses the username over transport security assertion)
            Dim strUserName As String = GetWS_WEBLOGIC_Username()
            Dim strPassword As String = GetWS_WEBLOGIC_Password()
            If strUserName <> String.Empty And strPassword <> String.Empty Then
                Dim oClientPolicy As New Microsoft.Web.Services3.Design.Policy()
                Dim oAssertion As UsernameOverTransportAssertion = New UsernameOverTransportAssertion()
                oAssertion.UsernameTokenProvider = New Microsoft.Web.Services3.Design.UsernameTokenProvider(strUserName, strPassword)
                oClientPolicy.Assertions.Add(oAssertion)
                ws.SetPolicy(oClientPolicy)

                ' Use windows proxy for access endpoint
                If GetWS_WEBLOGIC_UseProxy() Then
                    ws.Proxy = New System.Net.WebProxy()
                End If
            End If

            ws.Timeout = GetWS_WEBLOGIC_Timeout()
            Return ws
        End Function

        'CRE14-020 New EAI infrastructure [Start][Karl]
        Private Function CreateWebServiceEndPoint_EAIWSProxy(ByVal eEndpointSite As enumEndpointSite) As ImmuEnquiryWebS_EndPoint_EAIWSProxy
            Return CreateWebServiceEndPoint_EAIWSProxy(GetWS_EAIWSProxy_Url(eEndpointSite))
        End Function

        Private Function CreateWebServiceEndPoint_EAIWSProxy(ByVal strURL As String) As ImmuEnquiryWebS_EndPoint_EAIWSProxy
            Dim ws As ImmuEnquiryWebS_EndPoint_EAIWSProxy = New ImmuEnquiryWebS_EndPoint_EAIWSProxy
            ws.Url = strURL

            ' Create Client Policy (Specify that the policy uses the username over transport security assertion)
            Dim strUserName As String = GetWS_EAIWSProxy_Username()
            Dim strPassword As String = GetWS_EAIWSProxy_Password()
            If strUserName <> String.Empty And strPassword <> String.Empty Then
                Dim oClientPolicy As New Microsoft.Web.Services3.Design.Policy()
                Dim oAssertion As UsernameOverTransportAssertion = New UsernameOverTransportAssertion()
                oAssertion.UsernameTokenProvider = New Microsoft.Web.Services3.Design.UsernameTokenProvider(strUserName, strPassword)
                oClientPolicy.Assertions.Add(oAssertion)
                ws.SetPolicy(oClientPolicy)

                ' Use windows proxy for access endpoint
                'Must set to 'Y' in development environment, otherwise console job such as "CMSHealthCheck" cannot connect
                'suggested to set as 'N' in production environment

                If GetWS_EAIWSProxy_UseProxy() Then
                    ws.Proxy = New System.Net.WebProxy()
                End If
            End If

            ws.Timeout = GetWS_EAIWSProxy_Timeout()
            Return ws
        End Function
        'CRE14-020 New EAI infrastructure [End][Karl]

        Private Sub InitServicePointManager()
            Dim callback As New RemoteCertificateValidationCallback(AddressOf ValidateCertificate)
            System.Net.ServicePointManager.ServerCertificateValidationCallback = callback
        End Sub

        Private Function ValidateCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
            'Return True to force the certificate to be accepted.
            Return True
        End Function

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Winnie SUEN]
        ' ----------------------------------------------------------
        Public Function HealthCheck(ByVal strSiteName As String, ByVal blnPrimarySite As Boolean, ByVal strURL As String, _
                                ByVal strRequest As String, ByVal strMessageID As String) As HAVaccineResult

            Dim udtAuditLogInterface As AuditLogBase = AuditLogInterface.GetAuditLogEntry(AuditLogInterface.EnumAuditLogModule.CMSHealthCheck)

            udtAuditLogInterface.SetCallSystem(VaccinationBLL.VaccineRecordSystem.CMS.ToString)

            Dim udtHAVaccineResult As HAVaccineResult = Nothing

            Try
                udtAuditLogInterface.AddDescripton("Site", strSiteName)
                udtAuditLogInterface.AddDescripton("Link", strURL)
                udtAuditLogInterface.WriteStartLog(LogID.LOG00000, Nothing, strMessageID)
                udtAuditLogInterface.WriteLogData(LogID.LOG00001, strRequest)

                udtHAVaccineResult = Nothing

                If blnPrimarySite = True Then
                    udtHAVaccineResult = New HAVaccineResult(GetVaccineInvoke(strRequest, WSProxyCMS.enumEndpointSite.Primary))
                Else
                    udtHAVaccineResult = New HAVaccineResult(GetVaccineInvoke(strRequest, strURL))
                End If

                udtAuditLogInterface.WriteLogData(LogID.LOG00002, udtHAVaccineResult.Result)

                udtAuditLogInterface.AddDescripton("CallSystem", VaccinationBLL.VaccineRecordSystem.CMS)
                udtAuditLogInterface.AddDescripton("MessageID", strMessageID)
                udtAuditLogInterface.AddDescripton("HealthCheck", YesNo.Yes)
                udtAuditLogInterface.AddDescripton("ReturnCode", udtHAVaccineResult.ReturnCode)
                udtAuditLogInterface.WriteEndLog(LogID.LOG00003, Nothing, strMessageID)

            Catch ex As Exception

                If udtHAVaccineResult Is Nothing Then
                    udtAuditLogInterface.WriteLogData(LogID.LOG00002, "")
                    udtAuditLogInterface.AddDescripton("CallSystem", VaccinationBLL.VaccineRecordSystem.CMS)
                    udtAuditLogInterface.AddDescripton("MessageID", strMessageID)
                    udtAuditLogInterface.AddDescripton("HealthCheck", YesNo.Yes)
                    udtAuditLogInterface.AddDescripton("ReturnCode", String.Empty)
                    udtAuditLogInterface.WriteEndLog(LogID.LOG00004, Nothing, strMessageID)

                Else
                    udtAuditLogInterface.WriteLogData(LogID.LOG00002, udtHAVaccineResult.Result)
                    udtAuditLogInterface.AddDescripton("CallSystem", VaccinationBLL.VaccineRecordSystem.CMS)
                    udtAuditLogInterface.AddDescripton("MessageID", strMessageID)
                    udtAuditLogInterface.AddDescripton("ReturnCode", udtHAVaccineResult.ReturnCode)
                    udtAuditLogInterface.AddDescripton("HealthCheck", YesNo.Yes)
                    udtAuditLogInterface.WriteEndLog(LogID.LOG00004, Nothing, strMessageID)
                End If

                Throw
            End Try

            Return udtHAVaccineResult
        End Function
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Winnie SUEN]

#End Region

#Region "Function: Get System Parameter"

#Region "WEBLOGIC"
        ''' <summary>
        ''' Get WebLogic web service endpoint URL
        ''' </summary>
        ''' <param name="eEndpointSite">The primary/secondary site URL will be got</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetWS_WEBLOGIC_Url(ByVal eEndpointSite As enumEndpointSite) As String
            Dim oGenFunc As New GeneralFunction()
            Dim strPrimary As String = String.Empty
            Dim strSecondary As String = String.Empty
            oGenFunc.getSystemParameter(SYS_PARAM_WEBLOGIC_URL, strPrimary, strSecondary)

            Select Case eEndpointSite
                Case enumEndpointSite.Primary
                    If strPrimary.Trim = String.Empty Then
                        Throw New Exception("Web service endpoint WebLogic (EHS->CMS) primary site URL is empty")
                    End If
                    Return strPrimary
                Case enumEndpointSite.Secondary
                    If strSecondary.Trim = String.Empty Then
                        Throw New Exception("Web service endpoint WebLogic (EHS->CMS) secondary site URL is empty")
                    End If
                    Return strSecondary
                Case Else
                    Throw New Exception(String.Format("Not supported web service endpoint (EHS->CMS) site ({0})", eEndpointSite.ToString()))
            End Select
        End Function

        Private Function GetWS_WEBLOGIC_Username() As String
            Dim oGenFunc As New GeneralFunction()
            Dim sValue As String = String.Empty
            oGenFunc.getSystemParameter(SYS_PARAM_WEBLOGIC_USERNAME, sValue, Nothing)
            Return sValue
        End Function

        Private Function GetWS_WEBLOGIC_Password() As String
            Dim oGenFunc As New GeneralFunction()
            Dim sValue As String = String.Empty
            oGenFunc.getSystemParameterPassword(SYS_PARAM_WEBLOGIC_PASSWORD, sValue)
            Return sValue
        End Function

        Private Function GetWS_WEBLOGIC_Timeout() As Integer
            Dim oGenFunc As New GeneralFunction()
            Dim sValue As String = String.Empty
            oGenFunc.getSystemParameter(SYS_PARAM_WEBLOGIC_TIMEOUT, sValue, Nothing)
            Return CInt(sValue) * 1000
        End Function

        Private Function GetWS_WEBLOGIC_UseProxy() As Boolean
            Dim oGenFunc As New GeneralFunction()
            Dim sValue As String = String.Empty
            oGenFunc.getSystemParameter(SYS_PARAM_WEBLOGIC_USE_PROXY, sValue, String.Empty)

            Return sValue = "Y"
        End Function

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Winnie SUEN]
        ' ----------------------------------------------------------
        Public Function GetWS_Url(eEndpointSite As enumEndpointSite) As String
            Dim strUrl As String = String.Empty

            Select Case GetWSEndpointType()
                Case EndpointEnum.EMULATE
                    strUrl = GetWS_EMULATE_Url(eEndpointSite)

                Case EndpointEnum.WEBLOGIC
                    strUrl = GetWS_WEBLOGIC_Url(eEndpointSite)

                Case EndpointEnum.EAIWSPROXY
                    strUrl = GetWS_EAIWSProxy_Url(eEndpointSite)

                Case Else
                    Throw New Exception(String.Format("Not supported (EHS->CMS) web service endpoint type({0}))", GetWSEndpointType()))
            End Select

            Return strUrl
        End Function
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Winnie SUEN]
#End Region

#Region "EMULATE"
        ''' <summary>
        ''' Get OC4J web service endpoint URL
        ''' </summary>
        ''' <param name="eEndpointSite">The primary/secondary site URL will be got</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetWS_EMULATE_Url(ByVal eEndpointSite As enumEndpointSite) As String
            Dim oGenFunc As New GeneralFunction()
            Dim strPrimary As String = String.Empty
            Dim strSecondary As String = String.Empty

            oGenFunc.getSystemParameter(SYS_PARAM_EMULATE_URL, strPrimary, strSecondary)

            Select Case eEndpointSite
                Case enumEndpointSite.Primary
                    If strPrimary.Trim = String.Empty Then
                        Throw New Exception("Web service endpoint EMULATE (EHS->CMS) primary site URL is empty")
                    End If
                    Return strPrimary
                Case enumEndpointSite.Secondary
                    If strSecondary.Trim = String.Empty Then
                        Throw New Exception("Web service endpoint EMULATE (EHS->CMS) secondary site URL is empty")
                    End If
                    Return strSecondary
                Case Else
                    Throw New Exception(String.Format("Not supported web service endpoint (EHS->CMS) site ({0})", eEndpointSite.ToString()))
            End Select
        End Function

        Private Function GetWS_EMULATE_Username() As String
            Dim oGenFunc As New GeneralFunction()
            Dim sValue As String = String.Empty
            oGenFunc.getSystemParameter(SYS_PARAM_EMULATE_USERNAME, sValue, Nothing)
            Return sValue
        End Function

        Private Function GetWS_EMULATE_Password() As String
            Dim oGenFunc As New GeneralFunction()
            Dim sValue As String = String.Empty
            oGenFunc.getSystemParameterPassword(SYS_PARAM_EMULATE_PASSWORD, sValue)
            Return sValue
        End Function

        Private Function GetWS_EMULATE_Timeout() As Integer
            Dim oGenFunc As New GeneralFunction()
            Dim sValue As String = String.Empty
            oGenFunc.getSystemParameter(SYS_PARAM_EMULATE_TIMEOUT, sValue, Nothing)
            Return CInt(sValue) * 1000
        End Function

        Private Function GetWS_EMULATE_UseProxy() As Boolean
            Dim oGenFunc As New GeneralFunction()
            Dim sValue As String = String.Empty
            oGenFunc.getSystemParameter(SYS_PARAM_EMULATE_USE_PROXY, sValue, String.Empty)

            Return sValue = "Y"
        End Function

#End Region

#Region "EAIWSProxy"
        Private Function GetWS_EAIWSProxy_Url(ByVal eEndpointSite As enumEndpointSite) As String
            Dim oGenFunc As New GeneralFunction()
            Dim strPrimary As String = String.Empty
            Dim strSecondary As String = String.Empty
            oGenFunc.getSystemParameter(SYS_PARAM_EAIWSPROXY_URL, strPrimary, strSecondary)

            Select Case eEndpointSite
                Case enumEndpointSite.Primary
                    If strPrimary.Trim = String.Empty Then
                        Throw New Exception("Web service endpoint EAI WS Proxy (EHS->CMS) primary site URL is empty")
                    End If
                    Return strPrimary
                Case enumEndpointSite.Secondary
                    If strSecondary.Trim = String.Empty Then
                        Throw New Exception("Web service endpoint EAI WS Proxy (EHS->CMS) secondary site URL is empty")
                    End If
                    Return strSecondary
                Case Else
                    Throw New Exception(String.Format("Not supported EAI WS Proxy endpoint (EHS->CMS) site ({0})", eEndpointSite.ToString()))
            End Select
        End Function

        Private Function GetWS_EAIWSProxy_Username() As String
            Dim oGenFunc As New GeneralFunction()
            Dim sValue As String = String.Empty
            oGenFunc.getSystemParameter(SYS_PARAM_EAIWSPROXY_USERNAME, sValue, Nothing)
            Return sValue
        End Function

        Private Function GetWS_EAIWSProxy_Password() As String
            Dim oGenFunc As New GeneralFunction()
            Dim sValue As String = String.Empty
            oGenFunc.getSystemParameterPassword(SYS_PARAM_EAIWSPROXY_PASSWORD, sValue)
            Return sValue
        End Function

        Private Function GetWS_EAIWSProxy_Timeout() As Integer
            Dim oGenFunc As New GeneralFunction()
            Dim sValue As String = String.Empty
            oGenFunc.getSystemParameter(SYS_PARAM_EAIWSPROXY_TIMEOUT, sValue, Nothing)
            Return CInt(sValue) * 1000
        End Function

        Private Function GetWS_EAIWSProxy_UseProxy() As Boolean
            Dim oGenFunc As New GeneralFunction()
            Dim sValue As String = String.Empty
            oGenFunc.getSystemParameter(SYS_PARAM_EAIWSPROXY_USE_PROXY, sValue, String.Empty)

            Return sValue = "Y"
        End Function

#End Region

        Private Function GetWSEndpointType() As EndpointEnum
            Dim oGenFunc As New GeneralFunction()
            Dim sValue As String = String.Empty
            oGenFunc.getSystemParameter(SYS_PARAM_ENDPOINT, sValue, String.Empty)

            Return [Enum].Parse(GetType(EndpointEnum), sValue)
        End Function

        Private Function GetWS_PatientLimit() As Integer
            Dim strValue As String = String.Empty

            _udtGenFunc.getSystemParameter(String.Format(SYS_PARAM_PATIENTLIMIT, GetWSEndpointType.ToString), strValue, String.Empty)

            Return CInt(strValue)
        End Function

#End Region

#Region "Shared Function"
        ''' <summary>
        ''' Check document no prefix, if prefix in exception list, then will enquiry HA vaccination record and
        ''' direct return zero record result (prefix exception list: "UP")
        ''' </summary>
        ''' <param name="strDocNo">Patient document no.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function IsDocNoInExceptionList(ByVal strDocNo As String) As Boolean
            ' Check CMS Identity No. exception
            Dim strParm1 As String = String.Empty
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
            udtGeneralFunction.getSystemParameter(SYS_PARAM_CMS_ID_NO_EXCEPTION, strParm1, String.Empty)

            Dim blnException As Boolean = False
            For Each strValue As String In strParm1.Split(";".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
                If strDocNo.Trim.StartsWith(strValue) Then blnException = True
            Next

            Return blnException
        End Function

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Shared Function GetCMSWSVersion() As Integer
            Dim udtGenFunc As New Common.ComFunction.GeneralFunction()
            Dim strValue As String = String.Empty
            If udtGenFunc.getSystemParameter(SYS_PARAM_CMS_WS_VER, strValue, String.Empty) Then
                If strValue <> String.Empty AndAlso IsNumeric(strValue) Then
                    Return CInt(strValue)
                End If
            Else
                Throw New Exception(String.Format("Fail to query SystemParameters[{0}]", SYS_PARAM_CMS_WS_VER))
            End If

        End Function
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]
#End Region

#Region "Support Function"
        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Winnie SUEN]
        ' ----------------------------------------------------------
        ''' <summary>
        ''' Add request information to Audit Log Description
        ''' </summary>
        ''' <param name="objAuditLog"></param>
        ''' <param name="strMessageID"></param>
        ''' <remarks></remarks>
        Private Sub AddRequestDescription(ByVal objAuditLog As AuditLogEVacc, ByVal strMessageID As String, ByVal udtRequestPatientList As EHSPersonalInformationModelCollection)
            objAuditLog.AddDescripton("CallSystem", VaccinationBLL.VaccineRecordSystem.CMS)
            objAuditLog.AddDescripton("MessageID", strMessageID)
            objAuditLog.AddDescripton("HealthCheck", YesNo.No)
            objAuditLog.AddDescripton("BatchEnquiry", IIf(udtRequestPatientList.Count > 1, YesNo.Yes, YesNo.No))
            objAuditLog.AddDescripton("NumOfPatient", udtRequestPatientList.Count)
        End Sub
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Winnie SUEN]
#End Region

    End Class

End Namespace