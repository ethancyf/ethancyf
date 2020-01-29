Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.Xml
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.CMSHealthCheck
Imports Common.Component.HATransaction
Imports Common.DataAccess
Imports Common.eHRIntegration.BLL
Imports Common.eHRIntegration.BLL.eHRServiceBLL
Imports Common.eHRIntegration.Model.Xml.eHRService
Imports Common.eHRIntegration.XmlFunction
Imports Common.WebService
Imports Common.WebService.Interface
Imports CommonScheduleJob.Component.ScheduleJobSuspend
Imports Common.OCSSS
Imports Common.OCSSS.OCSSSServiceBLL
Imports Microsoft.Web.Services3.Design

Module Core

    Public Sub Main()
        ' CRE11-006
        Dim objScheduleJob As New ScheduleJob
        objScheduleJob.Start()
    End Sub

End Module

''' <summary>
''' CRE11-006
''' </summary>
''' <remarks></remarks>
Public Class ScheduleJob
    Inherits CommonScheduleJob.BaseScheduleJob

#Region "Enum Constants"

    ' For InterfaceHealthCheckLog
    Public Enum InterfaceCode
        NA
        EVACC           ' CMS
        EVACC_CIMS      ' CRE18-004 (CIMS Vaccination Sharing)
        EHRTOKEN
        OCSSS
    End Enum

    Public Enum HealthCheckResult
        NA
        Success
        Fail
        Suspend
    End Enum

#End Region

#Region "Private Variables"

    Private Const CONST_SYS_PARAM_OCSSS_WS_Link As String = "OCSSS_WS_Link"
    Private Const CONST_OCSSS_FUNC_CODE As String = "HEALTH1"

#End Region

    Private _objAuditLog As New Common.ComObject.ScheduleJobLogEntry(ScheduleJobID)

    ''' <summary>
    ''' CRE11-006
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides ReadOnly Property ScheduleJobID() As String
        Get
            Return FunctCode.FUNT110101
        End Get
    End Property

    Protected Overrides ReadOnly Property FunctionCode() As String
        Get
            Return FunctCode.FUNT110101
        End Get
    End Property

    ''' <summary>
    ''' CRE11-006
    ''' Main process of schedule job
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub Process()

        '-----------------------------------------
        '1. Health Check for CMS EVaccineation 
        '-----------------------------------------
        Call CMSHealthCheck()

        '-----------------------------------------
        '2. Health Check for eHRSS Token Service
        '-----------------------------------------
        Call EHRSSTokenService()

        '-----------------------------------------
        '3. Health Check for CIMS EVaccineation
        '-----------------------------------------
        Call CIMSHealthCheck()

        '-----------------------------------------
        '4. Health Check for OCSSS
        '-----------------------------------------
        Call OCSSS_HealthCheck()

    End Sub

#Region "HA CMS"

    Private Sub CMSHealthCheck()
        Dim cllnEndPointURL As Collection = Nothing

        Dim HealthCheckFunctionCodeList As Collection = Nothing
        Dim strHealthCheckFunctionCode As String = String.Empty
        Dim udtGeneralFunction As New GeneralFunction
        Dim intCount As Integer
        Dim strFunctionCode As String = String.Empty
        Dim strSiteName As String = String.Empty
        Dim strSiteSerial As String = String.Empty
        Dim strURL As String = String.Empty
        Dim udtCMSHealthCheckBLL As New CMSHealthCheckBLL

        cllnEndPointURL = udtCMSHealthCheckBLL.GetUsingEndpointURLList(HealthCheckFunctionCodeList)

        If Not cllnEndPointURL Is Nothing And cllnEndPointURL.Count > 0 Then
            For intCount = 1 To cllnEndPointURL.Count
                strFunctionCode = String.Empty
                strSiteName = String.Empty
                strSiteSerial = String.Empty
                strURL = cllnEndPointURL(intCount)
                strHealthCheckFunctionCode = HealthCheckFunctionCodeList(intCount)

                If strURL <> String.Empty Then
                    If intCount = 1 Then
                        'Primary Site
                        strSiteSerial = intCount
                        strFunctionCode = strHealthCheckFunctionCode
                        strSiteName = "Primary Site " & strSiteSerial
                        Call HealthCheck_CMS(strFunctionCode, strSiteName, True, strURL)
                    Else
                        'Secondary Site
                        strSiteSerial = (intCount - 1).ToString
                        strFunctionCode = strHealthCheckFunctionCode
                        strSiteName = "Secondary Site " & strSiteSerial
                        Call HealthCheck_CMS(strFunctionCode, strSiteName, False, strURL)
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub HealthCheck_CMS(ByVal strFunctionCode As String, ByVal strSiteName As String, ByVal blnPrimarySite As Boolean, ByVal strURL As String)
        Dim strRequest As String = String.Empty
        Dim strMessageID As String = String.Empty
        Dim objStartKey As AuditLogStartKey = Nothing
        Dim objAuditLogInterface As AuditLogBase = AuditLogInterface.GetAuditLogEntry(AuditLogInterface.EnumAuditLogModule.CMSHealthCheck)

        Dim udtHAVaccineResult As HAVaccineResult = Nothing
        Dim objWSProxyCMS As New WSProxyCMS(Nothing)

        Try
            InitServicePointManager()

            If CheckScheduleJobRunnable(GetScheduleJobSuspendFilePath(InterfaceCode.EVACC.ToString, strFunctionCode)) Then
                strRequest = GetRequestXml(strMessageID).InnerXml

                MyBase.AuditLog.AddDescripton("Site", strSiteName)
                MyBase.AuditLog.AddDescripton("MessageID", strMessageID)
                MyBase.AuditLog.AddDescripton("Url", strURL)
                objStartKey = MyBase.AuditLog.WriteStartLog(LogID.LOG00000, "CMS Health Check Start")


                udtHAVaccineResult = objWSProxyCMS.HealthCheck(strSiteName, blnPrimarySite, strURL, strRequest, strMessageID)

                MyBase.AuditLog.AddDescripton("Site", strSiteName)
                MyBase.AuditLog.AddDescripton("MessageID", strMessageID)
                MyBase.AuditLog.AddDescripton("ReturnCode", udtHAVaccineResult.ReturnCode)
                MyBase.AuditLog.WriteEndLog(objStartKey, LogID.LOG00001, "CMS Health Check End")


                If udtHAVaccineResult.ReturnCode = HAVaccineResult.enumReturnCode.ReturnForHealthCheck Then
                    Log(String.Format("[EHS>CMS] (" & strSiteName & ") ReturnCode {0} = ReturnForHealthCheck, OK", udtHAVaccineResult.ReturnCode))
                    DatabaseLogBLL.AddInterfaceHealthCheckLog(DateTime.MinValue, InterfaceCode.EVACC.ToString, strFunctionCode, LogID.LOG00001, String.Format("OK ; {0}", strURL), Nothing)
                Else
                    Log(String.Format("[EHS>CMS] (" & strSiteName & ") ReturnCode {0} <> ReturnForHealthCheck, error", udtHAVaccineResult.ReturnCode))
                    DatabaseLogBLL.AddInterfaceHealthCheckLog(DateTime.MinValue, InterfaceCode.EVACC.ToString, strFunctionCode, LogID.LOG00002, String.Format("Connect Fail ; {0}", strURL), _
                        String.Format("ReturnCode: {0}", udtHAVaccineResult.ReturnCode))
                End If

            Else
                Log("[EHS>CMS] (" & strSiteName & ") Suspended")

                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Winnie SUEN]
                ' ----------------------------------------------------------
                'Write InterfaceHealthCheckLog
                DatabaseLogBLL.AddInterfaceHealthCheckLog(DateTime.MinValue, InterfaceCode.EVACC.ToString, strFunctionCode, LogID.LOG00003, String.Format("Suspend ; {0}", strURL), Nothing)
                ' CRE18-004 (CIMS Vaccination Sharing) [End][Winnie SUEN]
            End If

        Catch ex As Exception
            Log(String.Format("(" & strSiteName & ") Exception: {0}", ex.Message))
            DatabaseLogBLL.AddInterfaceHealthCheckLog(DateTime.MinValue, InterfaceCode.EVACC.ToString, strFunctionCode, "00002", String.Format("Connect Fail ; {0}", strURL), _
                   String.Format("Exception: {0}", ex.Message))

            If udtHAVaccineResult Is Nothing Then
                MyBase.AuditLog.AddDescripton("Site", strSiteName)
                MyBase.AuditLog.AddDescripton("MessageID", strMessageID)
                MyBase.AuditLog.AddDescripton("ReturnCode", String.Empty)
                MyBase.AuditLog.WriteEndLog(objStartKey, LogID.LOG00001, "CMS Health Check End")
            Else
                MyBase.AuditLog.AddDescripton("Site", strSiteName)
                MyBase.AuditLog.AddDescripton("MessageID", strMessageID)
                MyBase.AuditLog.AddDescripton("ReturnCode", udtHAVaccineResult.ReturnCode)
                MyBase.AuditLog.WriteEndLog(objStartKey, LogID.LOG00001, "CMS Health Check End")
            End If
        End Try

    End Sub

#End Region

#Region "DH CIMS"

    Private Sub CIMSHealthCheck()
        Dim cllnEndPointURL As Collection = Nothing
        Dim objWSProxyCIMS As New WSProxyDHCIMS(Nothing)

        Dim HealthCheckFunctionCodeList As Collection = Nothing
        Dim strHealthCheckFunctionCode As String = String.Empty
        Dim udtGeneralFunction As New GeneralFunction
        Dim intCount As Integer
        Dim strFunctionCode As String = String.Empty
        Dim strSiteName As String = String.Empty
        Dim strSiteSerial As String = String.Empty
        Dim strURL As String = String.Empty
        Dim udtCIMSHealthCheckBLL As New CMSHealthCheckBLL

        cllnEndPointURL = udtCIMSHealthCheckBLL.GetCIMSUsingEndpointURLList(HealthCheckFunctionCodeList)

        If Not cllnEndPointURL Is Nothing And cllnEndPointURL.Count > 0 Then
            For intCount = 1 To cllnEndPointURL.Count
                strFunctionCode = String.Empty
                strSiteName = String.Empty
                strSiteSerial = String.Empty
                strURL = cllnEndPointURL(intCount)
                strHealthCheckFunctionCode = HealthCheckFunctionCodeList(intCount)

                If strURL <> String.Empty Then
                    If intCount = 1 Then
                        'Primary Site
                        strSiteSerial = intCount
                        strFunctionCode = strHealthCheckFunctionCode
                        strSiteName = "Primary Site " & strSiteSerial
                        Call HealthCheck_CIMS(strFunctionCode, strSiteName, strURL)
                    Else
                        'Secondary Site
                        strSiteSerial = (intCount - 1).ToString
                        strFunctionCode = strHealthCheckFunctionCode
                        strSiteName = "Secondary Site " & strSiteSerial
                        Call HealthCheck_CIMS(strFunctionCode, strSiteName, strURL)
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub HealthCheck_CIMS(ByVal strFunctionCode As String, ByVal strSiteName As String, ByVal strURL As String)
        Dim strRequest As String = String.Empty
        Dim objStartKey As AuditLogStartKey = Nothing

        Dim udtDHVaccineResult As DHVaccineResult = Nothing
        Dim objWSProxyCIMS As New WSProxyDHCIMS(Nothing)

        Try
            InitServicePointManager()

            If CheckScheduleJobRunnable(GetScheduleJobSuspendFilePath(InterfaceCode.EVACC_CIMS.ToString, strFunctionCode)) Then

                MyBase.AuditLog.AddDescripton("Site", strSiteName)
                MyBase.AuditLog.AddDescripton("Url", strURL)
                objStartKey = MyBase.AuditLog.WriteStartLog(LogID.LOG00020, "CIMS Health Check Start")


                udtDHVaccineResult = objWSProxyCIMS.HealthCheck(strSiteName, strURL)

                MyBase.AuditLog.AddDescripton("Site", strSiteName)
                MyBase.AuditLog.AddDescripton("ReturnCode", udtDHVaccineResult.ReturnCode)
                MyBase.AuditLog.WriteEndLog(objStartKey, LogID.LOG00021, "CIMS Health Check End")


                If udtDHVaccineResult.ReturnCode = DHVaccineResult.enumReturnCode.HealthCheck Then
                    Log(String.Format("[EHS>CIMS] (" & strSiteName & ") ReturnCode {0} = HealthCheck, OK", udtDHVaccineResult.ReturnCode))
                    DatabaseLogBLL.AddInterfaceHealthCheckLog(DateTime.MinValue, InterfaceCode.EVACC_CIMS.ToString, strFunctionCode, LogID.LOG00001, String.Format("OK ; {0}", strURL), Nothing)
                Else
                    Log(String.Format("[EHS>CIMS] (" & strSiteName & ") ReturnCode {0} <> HealthCheck, error", udtDHVaccineResult.ReturnCode))
                    DatabaseLogBLL.AddInterfaceHealthCheckLog(DateTime.MinValue, InterfaceCode.EVACC_CIMS.ToString, strFunctionCode, LogID.LOG00002, String.Format("Connect Fail ; {0}", strURL), _
                        String.Format("ReturnCode: {0}", udtDHVaccineResult.ReturnCode))
                End If

            Else
                Log("[EHS>CIMS] (" & strSiteName & ") Suspended")

                'Write InterfaceHealthCheckLog
                DatabaseLogBLL.AddInterfaceHealthCheckLog(DateTime.MinValue, InterfaceCode.EVACC_CIMS.ToString, strFunctionCode, LogID.LOG00003, String.Format("Suspend ; {0}", strURL), Nothing)

            End If

        Catch ex As Exception
            Log(String.Format("[EHS>CIMS] (" & strSiteName & ") Exception: {0}", ex.Message))
            DatabaseLogBLL.AddInterfaceHealthCheckLog(DateTime.MinValue, InterfaceCode.EVACC_CIMS.ToString, strFunctionCode, LogID.LOG00002, String.Format("Connect Fail ; {0}", strURL), _
                   String.Format("Exception: {0}", ex.Message))

            If udtDHVaccineResult Is Nothing Then
                MyBase.AuditLog.AddDescripton("Site", strSiteName)
                MyBase.AuditLog.AddDescripton("ReturnCode", String.Empty)
                MyBase.AuditLog.WriteEndLog(objStartKey, LogID.LOG00021, "CIMS Health Check End")
            Else
                MyBase.AuditLog.AddDescripton("Site", strSiteName)
                MyBase.AuditLog.AddDescripton("ReturnCode", udtDHVaccineResult.ReturnCode)
                MyBase.AuditLog.WriteEndLog(objStartKey, LogID.LOG00021, "CIMS Health Check End")
            End If
        End Try

    End Sub
#End Region

#Region "EHRSS"

    Protected Sub EHRSSTokenService()

        Dim udtGeneralFunction As New GeneralFunction
        Dim dicVPList As Dictionary(Of String, String) = New Dictionary(Of String, String)
        Dim dicWSList As Dictionary(Of String, String) = New Dictionary(Of String, String)
        Dim dicResultList As Dictionary(Of String, HealthCheckResult) = New Dictionary(Of String, HealthCheckResult)

        Dim strMode As String = udtGeneralFunction.getSystemParameterValue1("eHRSS_Mode")
        Dim strPrimarySite As String = udtGeneralFunction.GetSystemVariableValue("eHRSS_PrimarySite")
        Dim strSystemID As String = udtGeneralFunction.getSystemParameterValue1(String.Format("eHRSS_{0}_SystemID", strMode))
        Dim strCertificationMark As String = udtGeneralFunction.getSystemParameterValue1(String.Format("eHRSS_{0}_EHRCertificationMark", strMode))
        Dim strServiceCode As String = udtGeneralFunction.getSystemParameterValue1(String.Format("eHRSS_{0}_ServiceCode", strMode))
        Dim strPriority As String = udtGeneralFunction.getSystemParameterValue1(String.Format("eHRSS_{0}_Priority", strMode))
        Dim strPriorityList() As String = Split(strPriority, ",")
        Dim strVPLink As String = String.Empty
        Dim strGetWebSLink As String = String.Empty

        Call GeteHRSSEndpointURLList(strMode, dicVPList, dicWSList)

        If strPrimarySite <> String.Empty Then
            Dim enumResult As HealthCheckResult = HealthCheckResult.NA

            Dim strFunctionCode As String = String.Empty

            For intCount As Integer = 1 To dicWSList.Count
                Dim strSite As String = strPriorityList(intCount - 1)

                strVPLink = String.Empty
                strGetWebSLink = String.Empty

                strFunctionCode = String.Format("HEALTH{0}", Replace(strSite, "DC", String.Empty))

                strVPLink = dicVPList(strSite)
                strGetWebSLink = dicWSList(strSite)

                enumResult = HealthCheckeHRSS(strMode, strSite, strVPLink, strGetWebSLink, _
                                             strSystemID, strCertificationMark, strServiceCode, _
                                             strFunctionCode)

                dicResultList.Add(strSite, enumResult)

            Next

            'Set Primary Site by priority
            Dim strHighestPriorityActiveSite As String = String.Empty

            If strPriorityList.Length > 1 Then
                For intCount As Integer = 0 To strPriorityList.Length - 1
                    If dicResultList(strPriorityList(intCount)) = HealthCheckResult.Success Then
                        strHighestPriorityActiveSite = strPriorityList(intCount)

                        Exit For
                    End If
                Next
            Else
                strHighestPriorityActiveSite = strPriorityList(0)
            End If

            'Reload Primary Site
            strPrimarySite = udtGeneralFunction.GetSystemVariableValue("eHRSS_PrimarySite")

            If strHighestPriorityActiveSite <> String.Empty AndAlso strPrimarySite <> strHighestPriorityActiveSite Then
                Dim udtAuditLogInterface As Common.eHRIntegration.AuditLogBase = New Common.eHRIntegration.AuditLogBase(FunctCode.FUNT070302, "DBFlag_dbEVS_InterfaceLog")

                Dim udteHRServiceBLL As New eHRServiceBLL
                udteHRServiceBLL.UpdateNewPrimarySite(strPrimarySite, strHighestPriorityActiveSite, udtAuditLogInterface)
            End If

        Else
            Throw New Exception("HealthCheck: No primary site retrieves from DB table SystemVariable.")
        End If

    End Sub

    Public Function HealthCheckeHRSS(ByVal strMode As String, ByVal strSite As String, ByVal strVPLink As String, ByVal strGetWebSLink As String, _
                      ByVal strSystemID As String, ByVal strEHRCertificationMark As String, ByVal strServiceCode As String, _
                      ByVal strFunctionCode As String) As String

        Dim udtInXml As InHealthCheckeHRSSXmlModel = Nothing
        Dim udtStartKey As AuditLogStartKey = Nothing
        Dim enumResult As HealthCheckResult = HealthCheckResult.NA

        Try
            'Write ScheduleJobLog
            MyBase.AuditLog.AddDescripton("Site", strSite)
            MyBase.AuditLog.AddDescripton("Url", strGetWebSLink)
            udtStartKey = MyBase.AuditLog.WriteStartLog(LogID.LOG00010, "EHR Health Check Start")

            If CheckScheduleJobRunnable(GetScheduleJobSuspendFilePath(InterfaceCode.EHRTOKEN.ToString, strFunctionCode)) AndAlso _
                (New GeneralFunction).GetSystemParameterParmValue1(String.Format("eHRSS_{0}_{1}_Enable", strMode, strSite)) = YesNo.Yes Then

                'Enquiry eHRSS Token Service
                Dim udteHRServiceBLL As New eHRServiceBLL(strMode, strSite, strVPLink, strGetWebSLink, _
                                              strSystemID, strEHRCertificationMark, strServiceCode)

                ' Prepare the Xml model
                Dim udtOutFunctionXml As New OutHealthCheckeHRSSXmlModel
                udtOutFunctionXml.Timestamp = eHRServiceBLL.GenerateTimestamp(DateTime.Now)

                Dim strData As String = SerializeXml(udtOutFunctionXml, blnCreateCDataSection:=True)

                ' Send to eHR
                udtInXml = udteHRServiceBLL.HandleGetEhrWebS(strData, enumEhrFunctionResult.healthCheckeHRSSResult)

                If Not udtInXml Is Nothing Then
                    If udtInXml.ResultCodeEnum = eHRResultCode.R1000_Success Then
                        'Write Console Log
                        Log(String.Format("[EHS>EHRSS] (" & strSite & ") ReturnCode {0}, OK", udtInXml.ResultCode))

                        'Write InterfaceHealthCheckLog
                        DatabaseLogBLL.AddInterfaceHealthCheckLog(DateTime.MinValue, InterfaceCode.EHRTOKEN.ToString, strFunctionCode, LogID.LOG00001, String.Format("OK ; {0}", strGetWebSLink), Nothing)

                        enumResult = HealthCheckResult.Success
                    Else
                        'Write Console Log
                        Log(String.Format("[EHS>EHRSS] (" & strSite & ") ReturnCode {0}, error", udtInXml.ResultCode))

                        'Write InterfaceHealthCheckLog
                        DatabaseLogBLL.AddInterfaceHealthCheckLog(DateTime.MinValue, InterfaceCode.EHRTOKEN.ToString, strFunctionCode, LogID.LOG00002, String.Format("Connect Fail ; {0}", strGetWebSLink), _
                            String.Format("ReturnCode: {0}", udtInXml.ResultDescription))

                        enumResult = HealthCheckResult.Fail
                    End If
                End If
            Else
                'Write Console Log
                Log(String.Format("[EHS>EHRSS] ({0}) Suspended", strSite))

                'Write InterfaceHealthCheckLog
                DatabaseLogBLL.AddInterfaceHealthCheckLog(DateTime.MinValue, InterfaceCode.EHRTOKEN.ToString, strFunctionCode, LogID.LOG00003, String.Format("Suspend ; {0}", strGetWebSLink), Nothing)

                enumResult = HealthCheckResult.Suspend
            End If

            'Write ScheduleJobLog
            MyBase.AuditLog.AddDescripton("Site", strSite)

            If Not udtInXml Is Nothing Then
                MyBase.AuditLog.AddDescripton("ReturnCode", udtInXml.ResultCode)
            Else
                MyBase.AuditLog.AddDescripton("ReturnCode", String.Empty)
            End If

            MyBase.AuditLog.WriteEndLog(udtStartKey, LogID.LOG00011, "EHR Health Check End")

            Return IIf(enumResult = HealthCheckResult.NA, HealthCheckResult.Fail, enumResult)

        Catch ex As Exception
            'Write Console Log
            Log(String.Format("[EHS>EHRSS] (" & strSite & ") Exception: {0}", ex.Message))

            'Write InterfaceHealthCheckLog
            DatabaseLogBLL.AddInterfaceHealthCheckLog(DateTime.MinValue, InterfaceCode.EHRTOKEN.ToString, strFunctionCode, LogID.LOG00002, String.Format("Connect Fail ; {0}", strGetWebSLink), _
                   String.Format("Exception: {0}", ex.Message))

            MyBase.AuditLog.AddDescripton("Site", strSite)
            If Not udtInXml Is Nothing Then
                MyBase.AuditLog.AddDescripton("ReturnCode", udtInXml.ResultCode)
            Else
                MyBase.AuditLog.AddDescripton("ReturnCode", String.Empty)
            End If
            MyBase.AuditLog.WriteEndLog(udtStartKey, LogID.LOG00011, "EHR Health Check End")

            Return HealthCheckResult.Fail
        End Try
    End Function

    Public Sub GeteHRSSEndpointURLList(ByVal strMode As String, ByRef dicVPList As Dictionary(Of String, String), ByRef dicWSList As Dictionary(Of String, String))

        Dim udtGeneralFunction As New GeneralFunction

        For intCount As Integer = 1 To 9
            Dim strSite As String = String.Format("DC{0}", intCount)
            Dim strVPURL As String = String.Empty
            Dim strWSURL As String = String.Empty

            udtGeneralFunction.getSystemParameter(String.Format("eHRSS_{0}_VerifySystemLink_{1}", strMode, strSite), strVPURL, Nothing)
            udtGeneralFunction.getSystemParameter(String.Format("eHRSS_{0}_GetEhrWebSLink_{1}", strMode, strSite), strWSURL, Nothing)

            If strVPURL <> String.Empty And strWSURL <> String.Empty Then
                dicVPList.Add(strSite, strVPURL)
                dicWSList.Add(strSite, strWSURL)
            Else
                Exit For
            End If
        Next

        If dicVPList.Count = 0 Or dicWSList.Count = 0 Or dicVPList.Count <> dicWSList.Count Then
            Throw New Exception("HealthCheck: No any endpoint retrieves from DB.")
        End If

    End Sub

#End Region

#Region "OCSSS"

    Private Sub OCSSS_HealthCheck()

        Dim udtStartKey As AuditLogStartKey = Nothing
        Dim udtOCSSSServiceBLL As New OCSSSServiceBLL()
        Dim udtOCSSSResult As OCSSSResult = Nothing

        Dim udtGeneralFunction As New GeneralFunction

        ' 1 site only for OCSSS
        Dim strWSLink As String = udtGeneralFunction.getSystemParameterValue1(CONST_SYS_PARAM_OCSSS_WS_Link)
        Dim strFunctionCode As String = CONST_OCSSS_FUNC_CODE

        Try
            'Write ScheduleJobLog
            MyBase.AuditLog.AddDescripton("Url", strWSLink)
            udtStartKey = MyBase.AuditLog.WriteStartLog(LogID.LOG00030, "OCSSS Health Check Start")

            If CheckScheduleJobRunnable(GetScheduleJobSuspendFilePath(InterfaceCode.OCSSS.ToString, strFunctionCode)) Then

                'Enquiry OCSSS Service
                udtOCSSSResult = udtOCSSSServiceBLL.HealthCheck()

                If Not udtOCSSSResult Is Nothing Then
                    Select Case udtOCSSSResult.ConnectionStatus
                        Case OCSSSResult.OCSSSConnection.Success
                            'Write Console Log
                            Log(String.Format("[EHS>OCSSS] ConnectionStatus: {0}, OK", udtOCSSSResult.ConnectionStatus))

                            'Write InterfaceHealthCheckLog
                            DatabaseLogBLL.AddInterfaceHealthCheckLog(DateTime.MinValue, InterfaceCode.OCSSS.ToString, strFunctionCode, LogID.LOG00001, String.Format("OK ; {0}", strWSLink), Nothing)

                        Case OCSSSResult.OCSSSConnection.Fail
                            'Write Console Log
                            Log(String.Format("[EHS>OCSSS] ConnectionStatus: {0}, error", udtOCSSSResult.ConnectionStatus))

                            'Write InterfaceHealthCheckLog
                            DatabaseLogBLL.AddInterfaceHealthCheckLog(DateTime.MinValue, InterfaceCode.OCSSS.ToString, strFunctionCode, LogID.LOG00002, String.Format("Connect Fail ; {0}", strWSLink), _
                                String.Format("ConnectionStatus: {0}", udtOCSSSResult.ConnectionStatus))

                        Case Else
                            'Write Console Log
                            Log(String.Format("[EHS>OCSSS] ConnectionStatus: {0}, Suspended", udtOCSSSResult.ConnectionStatus))

                            'Write InterfaceHealthCheckLog
                            DatabaseLogBLL.AddInterfaceHealthCheckLog(DateTime.MinValue, InterfaceCode.OCSSS.ToString, strFunctionCode, LogID.LOG00003, String.Format("Suspend ; {0}", strWSLink), Nothing)
                    End Select
                End If
            Else
                'Write Console Log
                Log("OCSSS Health Check Suspended")

                'Write InterfaceHealthCheckLog
                DatabaseLogBLL.AddInterfaceHealthCheckLog(DateTime.MinValue, InterfaceCode.OCSSS.ToString, strFunctionCode, LogID.LOG00003, String.Format("Suspend ; {0}", strWSLink), Nothing)
            End If

            'Write ScheduleJobLog
            If Not udtOCSSSResult Is Nothing Then
                MyBase.AuditLog.AddDescripton("ConnectionStatus", udtOCSSSResult.ConnectionStatus.ToString)
            Else
                MyBase.AuditLog.AddDescripton("ConnectionStatus", String.Empty)
            End If

            MyBase.AuditLog.WriteEndLog(udtStartKey, LogID.LOG00031, "OCSSS Health Check End")

        Catch ex As Exception
            'Write Console Log
            Log(String.Format("OCSSS Exception: {0}", ex.Message))

            'Write InterfaceHealthCheckLog
            DatabaseLogBLL.AddInterfaceHealthCheckLog(DateTime.MinValue, InterfaceCode.OCSSS.ToString, strFunctionCode, LogID.LOG00002, String.Format("Connect Fail ; {0}", strWSLink), _
                   String.Format("Exception: {0}", ex.Message))

            If Not udtOCSSSResult Is Nothing Then
                MyBase.AuditLog.AddDescripton("ConnectionStatus", udtOCSSSResult.ConnectionStatus.ToString)
            Else
                MyBase.AuditLog.AddDescripton("ConnectionStatus", String.Empty)
            End If

            MyBase.AuditLog.WriteEndLog(udtStartKey, LogID.LOG00031, "OCSSS Health Check End")
        End Try
    End Sub

#End Region

#Region "Supporting Functions"

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

    'CRE16-019 (To implement token sharing between eHS(S) and eHRSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    ''' <summary>
    ''' CRE16-019
    ''' Get schedule job suspend file for path by app setting: 
    ''' CMS       [SJSuspendFile_EVACC_HEALTH1, SJSuspendFile_EVACC_HEALTH2, SJSuspendFile_EVACC_HEALTH3]
    ''' EHRTOKEN  [SJSuspendFile_EHRTOKEN_HEALTH1, SJSuspendFile_EHRTOKEN_HEALTH2]   
    ''' OCSSS     [SJSuspendFile_OCSSS_HEALTH1]
    '''                                                  
    ''' if app setting is empty then use default file in execute path:
    ''' CMS       [suspend_EVACC_1.txt, suspend_EVACC_2.txt, suspend_EVACC_3.txt]
    ''' EHRTOKEN  [suspend_EHRTOKEN_1.txt, suspend_EHRTOKEN_2.txt]
    ''' OCSSS     [suspend_OCSSS_1.txt]
    ''' 
    ''' </summary>
    ''' <param name="strFunctionCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetScheduleJobSuspendFilePath(ByVal strInterfaceCode As String, ByVal strFunctionCode As String) As String
        Dim strPath As String = System.Configuration.ConfigurationManager.AppSettings(String.Format("SJSuspendFile_{0}_{1}", strInterfaceCode, strFunctionCode))
        If strPath Is Nothing OrElse strPath.Trim = String.Empty Then
            strPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory(), String.Format("Suspend_{0}_{1}.txt", strInterfaceCode, Replace(strFunctionCode, "HEALTH", "")))
        End If

        Return strPath
    End Function
    'CRE16-019 (To implement token sharing between eHS(S) and eHRSS) [End][Chris YIM]

    Private Sub InitServicePointManager()
        Dim callback As New RemoteCertificateValidationCallback(AddressOf ValidateCertificate)
        System.Net.ServicePointManager.ServerCertificateValidationCallback = callback
    End Sub

    Private Function ValidateCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
        ' Return True to force the certificate to be accepted
        Return True
    End Function

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

End Class
