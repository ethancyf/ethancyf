Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.HATransaction
Imports Common.DataAccess
Imports Common.WebService
Imports Common.WebService.Interface
Imports Microsoft.Web.Services3.Design
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security
Imports System.Xml
Imports CommonScheduleJob.Component.ScheduleJobSuspend

Module Core

    Public Sub Main()

        Dim objScheduleJob As New ScheduleJob
        objScheduleJob.Start()
    End Sub

End Module

Public Class ScheduleJob
    Inherits CommonScheduleJob.BaseScheduleJob

    Private Const strInterfaceCode As String = "EVACC_SB"
    Private _objAuditLog As New Common.ComObject.ScheduleJobLogEntry(ScheduleJobID)

    Private Const APP_SETTING_CMS_MODE As String = "CMS_MODE"
    Private Const APP_SETTING_NORMAL_CASE_IN_SECONDS As String = "NORMAL_CASE_IN_SECONDS"
    Private Const APP_SETTING_EVENT_LOG_SOURCE As String = "EventLogSource"
    Private Const APP_SETTING_EVENT_ID_EMAIL As String = "EventLogID_Email"

    Private Const CMSHEALTHCHECKBYCONFIG_FUNC_CODE As String = "HEALTH_SB%i"

    Private mstrSuspendedLinks() As String

    Public Overrides ReadOnly Property ScheduleJobID() As String
        Get
            Return "CMSHEALTHCHECKBYCONFIG" 'Hard Code since the program only used for temporary measuse, will be removed after half year
        End Get
    End Property


    Public Function GetUsingEndpointURLListByConfiguredMode(ByVal strCMSMode As String, Optional ByRef cllnFuncCodeList As Collection = Nothing) As Collection
        Dim strUsingMode As String = String.Empty

        Dim strUsingLink As String = String.Empty

        Dim strStandbyLink() As String = Nothing

        Dim intCount As Integer
        Dim intCountFuncCode As Integer
        Dim strHealthCheckFuncCode As String = CMSHEALTHCHECKBYCONFIG_FUNC_CODE

        Dim strWEBLOGICCurrentStandby As String = String.Empty
        Dim udtGeneralFunction As New GeneralFunction
        Dim cllnURL As New Collection
        Dim cllnFuncCode As New Collection


        strUsingMode = strCMSMode

        udtGeneralFunction.getSystemParameter(String.Format("CMS_Get_Vaccine_WS_{0}_Url", strUsingMode), strUsingLink, Nothing)

        udtGeneralFunction.GetInternalSystemParameterByLikeClause(String.Format("CMS_Get_Vaccine_WS_{0}_Url%[0-9]", strUsingMode), Nothing, strStandbyLink, Nothing)

        cllnURL.Add(strUsingLink)

        If Not strStandbyLink Is Nothing AndAlso strStandbyLink.Length > 0 Then
            For intCount = 0 To strStandbyLink.Length - 1
                If strUsingLink.Trim <> strStandbyLink(intCount).Trim Then
                    cllnURL.Add(strStandbyLink(intCount).Trim)
                End If
            Next
        End If

        If cllnURL.Count > strStandbyLink.Length Then
            For intCountFuncCode = 1 To cllnURL.Count
                cllnFuncCode.Add(strHealthCheckFuncCode.Replace("%i", intCountFuncCode.ToString))
            Next
        Else
            For intCountFuncCode = 1 To strStandbyLink.Length
                cllnFuncCode.Add(strHealthCheckFuncCode.Replace("%i", intCountFuncCode.ToString))
            Next
        End If

        cllnFuncCodeList = cllnFuncCode

        Return cllnURL
    End Function

    Private Function GetSuspendedLink() As String()
        Dim strSuspendedLink As String() = Nothing
        Dim liCount As Integer
        Dim strSuspendLinkPrefix As String = "Suspend_link%s"
        Dim strSuspendLink As String
        Dim strSuspendURL As String

        For liCount = 1 To 100 'Avoid infinite loop
            strSuspendLink = strSuspendLinkPrefix.Replace("%s", liCount.ToString)
            strSuspendURL = System.Configuration.ConfigurationManager.AppSettings(strSuspendLink)

            If String.IsNullOrEmpty(strSuspendURL) = False Then
                ReDim Preserve strSuspendedLink(liCount - 1)

                strSuspendedLink(liCount - 1) = strSuspendURL
            End If
        Next

        Return strSuspendedLink
    End Function


    Private Function IsSuspendedLink(ByVal strURL As String) As Boolean
        Dim liCount As Integer
        'Dim strSuspendLinkPrefix As String = "Suspend_link%s"
        'Dim strSuspendLink As String
        'Dim strSuspendURL As String
        Dim objStartKey As AuditLogStartKey = Nothing

        If IsNothing(mstrSuspendedLinks) Then
            mstrSuspendedLinks = GetSuspendedLink()
        End If

        If Not IsNothing(mstrSuspendedLinks) Then
            For liCount = 0 To mstrSuspendedLinks.Length - 1 'Avoid infinite loop

                If String.IsNullOrEmpty(mstrSuspendedLinks(liCount)) = False Then
                    If mstrSuspendedLinks(liCount).ToUpper.Trim = strURL.ToUpper.Trim Then
                        MyBase.AuditLog.AddDescripton("Suspended Link", strURL)
                        objStartKey = MyBase.AuditLog.WriteStartLog(LogID.LOG00002, "Health Check Suspended")
                        Return True
                    End If
                End If
            Next
        End If

    End Function

    Protected Overrides Sub Process()
        Dim strCMSMode As String = System.Configuration.ConfigurationManager.AppSettings(APP_SETTING_CMS_MODE)

        Dim udtHAVaccineResult As HAVaccineResult = Nothing
        Dim cllnEndPointURL As Collection = Nothing
        ' Dim objAuditLogInterface As AuditLogBase = AuditLogInterface.GetAuditLogEntry(AuditLogInterface.EnumAuditLogModule.CMSHealthCheck)
        Dim objStartKey As AuditLogStartKey = Nothing
        Dim strMessageID As String = String.Empty
        Dim strRequest As String = String.Empty

        Dim objWSProxyCMS As New WSProxyCMS(Nothing)

        Dim HealthCheckFunctionCodeList As Collection = Nothing
        Dim strHealthCheckFunctionCode As String = String.Empty
        Dim udtGeneralFunction As New GeneralFunction
        Dim intCount As Integer
        Dim strFunctionCode As String = String.Empty
        Dim strSiteName As String = String.Empty
        Dim strSiteSerial As String = String.Empty
        Dim strURL As String = String.Empty
        Dim udtCMSHealthCheckBLL As New CMSHealthCheck.CMSHealthCheckBLL

        cllnEndPointURL = GetUsingEndpointURLListByConfiguredMode(strCMSMode, HealthCheckFunctionCodeList)        

        If Not cllnEndPointURL Is Nothing And cllnEndPointURL.Count > 0 Then
            For intCount = 1 To cllnEndPointURL.Count
                strFunctionCode = String.Empty
                strSiteName = String.Empty
                strSiteSerial = String.Empty
                strURL = cllnEndPointURL(intCount)
                strHealthCheckFunctionCode = HealthCheckFunctionCodeList(intCount)

                If strURL <> String.Empty andAlso _
                                       IsSuspendedLink(strURL) = False Then

                    If intCount = 1 Then
                        'Primary Site
                        strSiteSerial = intCount
                        strFunctionCode = strHealthCheckFunctionCode
                        strSiteName = "Primary Site " & strSiteSerial
                        Call HealthCheck(strFunctionCode, strSiteName, strURL, strCMSMode)
                    Else
                        'Secondary Site
                        strSiteSerial = (intCount - 1).ToString
                        strFunctionCode = strHealthCheckFunctionCode
                        strSiteName = "Secondary Site " & strSiteSerial
                        Call HealthCheck(strFunctionCode, strSiteName, strURL, strCMSMode)
                    End If
                End If
            Next
        End If

    End Sub

    'CRE13-015 Add URL for new EAI server [Start][Karl]
    Private Sub HealthCheck(ByVal strFunctionCode As String, ByVal strSiteName As String, ByVal strURL As String, ByVal strCMSMode As String)
        Dim strRequest As String = String.Empty
        Dim strMessageID As String = String.Empty
        Dim objStartKey As AuditLogStartKey = Nothing
        '  Dim objAuditLogInterface As AuditLogBase = AuditLogInterface.GetAuditLogEntry(AuditLogInterface.EnumAuditLogModule.CMSHealthCheck)
        Dim udtHAVaccineResult As HAVaccineResult = Nothing
        Dim objWSProxyCMS As New WSProxyCMS(Nothing)
        Dim dtmStart As DateTime = DateTime.MinValue
        Dim dtmEnd As DateTime = DateTime.MinValue
        Dim dtmDiff As DateTime = New DateTime(dtmEnd.Subtract(dtmStart).Ticks)
        Dim strNormalCaseInSeconds As String = System.Configuration.ConfigurationManager.AppSettings(APP_SETTING_NORMAL_CASE_IN_SECONDS)
        Dim enumUsingEndPoint As EndpointEnum
        Dim udtCMSHealthCheckBLL As New CMSHealthCheck.CMSHealthCheckBLL

        enumUsingEndPoint = GetUsingEndpoint(strCMSMode)
        Try

            InitServicePointManager()

            If CheckScheduleJobRunnable(GetScheduleJobSuspendFilePath(strFunctionCode)) Then
                strRequest = GetRequestXml(strMessageID).InnerXml

                MyBase.AuditLog.AddDescripton("Site", strSiteName)
                MyBase.AuditLog.AddDescripton("MessageID", strMessageID)
                MyBase.AuditLog.AddDescripton("Url", strURL)
                objStartKey = MyBase.AuditLog.WriteStartLog(LogID.LOG00000, "Health Check Start")

                'objAuditLogInterface.AddDescripton("Site", strSiteName)
                'objAuditLogInterface.WriteStartLog(LogID.LOG00000, Nothing, strMessageID)
                'objAuditLogInterface.WriteLogData(LogID.LOG00001, "CMSHealthCheckByConfig - eHS Request XML", strRequest, Nothing, Nothing, strMessageID)

                udtHAVaccineResult = Nothing

                'If blnPrimarySite = True Then
                '    udtHAVaccineResult = New HAVaccineResult(objWSProxyCMS.GetVaccineInvoke(strRequest, WSProxyCMS.enumEndpointSite.Primary))
                'Else
                dtmStart = Now
                udtHAVaccineResult = New HAVaccineResult(objWSProxyCMS.GetVaccineInvoke(strRequest, strURL, enumUsingEndPoint))

                'For siumulate slow response case
                'Threading.Thread.Sleep(1000 * CInt(System.Configuration.ConfigurationManager.AppSettings("WAITSECOND")))

                dtmEnd = Now
                'End If

                'objAuditLogInterface.WriteLogData(LogID.LOG00002, "CMSHealthCheckByConfig - CMS Result XML", udtHAVaccineResult.Result, Nothing, Nothing, strMessageID)
                'objAuditLogInterface.AddDescripton("ReturnCode", udtHAVaccineResult.ReturnCode)
                'objAuditLogInterface.WriteEndLog(LogID.LOG00003, Nothing, strMessageID)

                MyBase.AuditLog.AddDescripton("Site", strSiteName)
                MyBase.AuditLog.AddDescripton("MessageID", strMessageID)
                MyBase.AuditLog.AddDescripton("ReturnCode", udtHAVaccineResult.ReturnCode)

                dtmDiff = New DateTime(dtmEnd.Subtract(dtmStart).Ticks)
                MyBase.AuditLog.AddDescripton("Time Elapsed", dtmDiff.ToString("mm:ss:fff"))

                MyBase.AuditLog.WriteEndLog(objStartKey, LogID.LOG00001, "Health Check End")


                If udtHAVaccineResult.ReturnCode = HAVaccineResult.enumReturnCode.ReturnForHealthCheck Then

                    If IsNumeric(strNormalCaseInSeconds) = True Then
                        If dtmEnd > DateAdd(DateInterval.Second, CInt(strNormalCaseInSeconds), dtmStart) Then
                            Log(String.Format("(" & strSiteName & ") slow response : > {0} Seconds", CInt(strNormalCaseInSeconds)))
                            DatabaseLogBLL.AddInterfaceHealthCheckLog(DateTime.MinValue, strInterfaceCode, strFunctionCode, "00001", String.Format("Slow Response > {2}s ({0}) ; {1}", strCMSMode, strURL, strNormalCaseInSeconds), Nothing)
                            Call TriggerEmailPatrolAlert(strSiteName, dtmDiff, True, Nothing)

                        Else
                            DatabaseLogBLL.AddInterfaceHealthCheckLog(DateTime.MinValue, strInterfaceCode, strFunctionCode, "00001", String.Format("OK ({0}) ; {1}", strCMSMode, strURL), Nothing)
                            Log(String.Format("(" & strSiteName & ") ReturnCode {0} = ReturnForHealthCheck, OK", udtHAVaccineResult.ReturnCode))
                        End If
                    Else
                        DatabaseLogBLL.AddInterfaceHealthCheckLog(DateTime.MinValue, strInterfaceCode, strFunctionCode, "00001", String.Format("OK ({0}) ; {1}", strCMSMode, strURL), Nothing)
                        Log(String.Format("(" & strSiteName & ") ReturnCode {0} = ReturnForHealthCheck, OK", udtHAVaccineResult.ReturnCode))
                    End If

                Else
                    Log(String.Format("(" & strSiteName & ") ReturnCode {0} <> ReturnForHealthCheck, error", udtHAVaccineResult.ReturnCode))
                    DatabaseLogBLL.AddInterfaceHealthCheckLog(DateTime.MinValue, strInterfaceCode, strFunctionCode, "00002", String.Format("Connect Fail ({0}) ; {1}", strCMSMode, strURL), _
                        String.Format("ReturnCode: {0}", udtHAVaccineResult.ReturnCode))

                    Call TriggerEmailPatrolAlert(strSiteName, dtmDiff, False, String.Format("ReturnCode {0} <> ReturnForHealthCheck", udtHAVaccineResult.ReturnCode))
                End If

            Else
                Log("(" & strSiteName & ") Suspended")
            End If

        Catch ex As Exception
            Log(String.Format("(" & strSiteName & ") Exception: {0}", ex.Message))
            DatabaseLogBLL.AddInterfaceHealthCheckLog(DateTime.MinValue, strInterfaceCode, strFunctionCode, "00002", String.Format("Connect Fail ({0}) ; {1}", strCMSMode, strURL), _
                   String.Format("Exception: {0}", ex.Message))
            dtmEnd = Now
            dtmDiff = New DateTime(dtmEnd.Subtract(dtmStart).Ticks)

            Call TriggerEmailPatrolAlert(strSiteName, dtmDiff, False, String.Format("Exception: {0}", ex.Message))

            If udtHAVaccineResult Is Nothing Then
                'objAuditLogInterface.WriteLogData(LogID.LOG00002, "CMSHealthCheckByConfig - CMS Result XML", "", Nothing, Nothing, strMessageID)
                'objAuditLogInterface.AddDescripton("ReturnCode", String.Empty)
                'objAuditLogInterface.WriteEndLog(LogID.LOG00003, Nothing, strMessageID)          
                MyBase.AuditLog.AddDescripton("Site", strSiteName)
                MyBase.AuditLog.AddDescripton("MessageID", strMessageID)
                MyBase.AuditLog.AddDescripton("ReturnCode", String.Empty)
                MyBase.AuditLog.AddDescripton("Time Elapsed", dtmDiff.ToString("mm:ss:fff"))
                MyBase.AuditLog.WriteEndLog(objStartKey, LogID.LOG00001, "Health Check End")
            Else
                'objAuditLogInterface.WriteLogData(LogID.LOG00002, "CMSHealthCheckByConfig - CMS Result XML", udtHAVaccineResult.Result, Nothing, Nothing, strMessageID)
                'objAuditLogInterface.AddDescripton("ReturnCode", udtHAVaccineResult.ReturnCode)
                'objAuditLogInterface.WriteEndLog(LogID.LOG00003, Nothing, strMessageID)
                MyBase.AuditLog.AddDescripton("Site", strSiteName)
                MyBase.AuditLog.AddDescripton("MessageID", strMessageID)
                MyBase.AuditLog.AddDescripton("ReturnCode", udtHAVaccineResult.ReturnCode)
                MyBase.AuditLog.AddDescripton("Time Elapsed", dtmDiff.ToString("mm:ss:fff"))
                MyBase.AuditLog.WriteEndLog(objStartKey, LogID.LOG00001, "Health Check End")
            End If
        End Try

    End Sub
    'CRE13-015 Add URL for new EAI server [End][Karl]

    ''' <summary>
    ''' CRE11-006
    ''' Override default check, because health check support 3 schedule job suspend ID,
    ''' Each schedule job suspend ID will be handled inside [Process] function
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overrides Function CheckScheduleJobRunnable() As Boolean
        Return True
    End Function

    ''' <summary>
    ''' CRE11-006
    ''' Get schedule job suspend file path by app setting [SJSuspendFile_HEALTH1, SJSuspendFile_HEALTH2, SJSuspendFile_HEALTH3],
    ''' if app setting is empty then use default file in execute path [suspend_1.txt, suspend_2.txt, suspend_3.txt]
    ''' </summary>
    ''' <param name="strFunctionCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetScheduleJobSuspendFilePath(ByVal strFunctionCode As String) As String
        Dim strPath As String = System.Configuration.ConfigurationManager.AppSettings("SJSuspendFile_" + strFunctionCode)
        If strPath Is Nothing OrElse strPath.Trim = String.Empty Then
            strPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory(), "Suspend_" + strFunctionCode.Substring(strFunctionCode.Length - 1, 1).ToString() + ".txt")
        End If

        Return strPath
    End Function

    Private Sub InitServicePointManager()
        Dim callback As New RemoteCertificateValidationCallback(AddressOf ValidateCertificate)
        System.Net.ServicePointManager.ServerCertificateValidationCallback = callback
    End Sub

    Private Function ValidateCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
        ' Return True to force the certificate to be accepted
        Return True
    End Function

#Region "Supporting Functions"

    Private Function GetRequestXml(ByRef strMessageID As String) As XmlDocument
        Dim strRequestXMLPath As String = System.Configuration.ConfigurationManager.AppSettings("RequestXML")
        Dim xmlRequest As New XmlDocument
        Dim lstNode As XmlNodeList = Nothing
        strMessageID = String.Empty

        xmlRequest.Load(strRequestXMLPath)

        ' CRE10-035
        ' Assign new message ID to request xml
        ' -------------------------------------------------------
        strMessageID = (New GeneralFunction).generateEVaccineMessageID()
        lstNode = xmlRequest.SelectNodes("/parameter/message_id")
        If lstNode IsNot Nothing AndAlso lstNode.Count > 0 Then
            lstNode(0).InnerText = strMessageID
        End If

        Return xmlRequest

    End Function

#End Region


#Region "Functions From Log Monitor"
    Private Sub WriteEventLog(ByVal strMessage As String, ByVal typeEntryType As EventLogEntryType, ByVal intEventID As Integer)
        Dim strAppName As String = System.Configuration.ConfigurationManager.AppSettings(APP_SETTING_EVENT_LOG_SOURCE)

        Try
            If Not EventLog.SourceExists(strAppName) Then EventLog.CreateEventSource(strAppName, "Application")

            Dim udtEventLog As New EventLog

            udtEventLog.Source = strAppName

            udtEventLog.WriteEntry(strMessage, typeEntryType, intEventID)

        Catch Ex As Exception
        End Try

    End Sub

    Private Function GetUsingEndpoint(ByVal strUsingEndpointValue As String) As EndpointEnum
        Dim strUsingMode As String

        strUsingMode = strUsingEndpointValue

        Return [Enum].Parse(GetType(EndpointEnum), strUsingMode)
    End Function

    Private Sub TriggerEmailPatrolAlert(ByVal strSiteName As String, ByVal dtmDiff As DateTime, ByVal blnSlowResponse As Boolean, ByVal strException As String)

        Dim strLog As String = Nothing
        Dim strLogDetails As String = Nothing
        Dim strLogIDEmail As String = System.Configuration.ConfigurationManager.AppSettings(APP_SETTING_EVENT_ID_EMAIL)
        Dim strNormalCaseInSeconds As String = System.Configuration.ConfigurationManager.AppSettings(APP_SETTING_NORMAL_CASE_IN_SECONDS)
        Dim strCMSMode As String = System.Configuration.ConfigurationManager.AppSettings(APP_SETTING_CMS_MODE)

        If blnSlowResponse = True Then
            strLogDetails = String.Format("(Stand By Health Check Mode:{1}) CMS IMMU Record Interface (EHS->CMS) Slow Response (> {0}s): ", strNormalCaseInSeconds, strCMSMode)
            strLogDetails = strLogDetails & String.Format(" [Time Elapsed]: {0}. ", dtmDiff.ToString("mm:ss:fff"))
        Else
            strLogDetails = String.Format("(Stand By Health Check Mode:{0}) CMS IMMU Record Interface (EHS->CMS) Connection Fail: ", strCMSMode)
            strLogDetails = strLogDetails & String.Format(" [Exception]: {0}. ", strException.Trim)
        End If

        strLogDetails = strLogDetails & String.Format(" [Site]: {0}. ", strSiteName)


        strLog = String.Format("[{0}] {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm"), strLogDetails)

        WriteEventLog(strLog, EventLogEntryType.Warning, strLogIDEmail)
    End Sub

#End Region
End Class
