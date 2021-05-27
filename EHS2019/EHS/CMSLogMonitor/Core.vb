Imports System.Configuration
Imports System.Diagnostics
Imports System.Runtime.CompilerServices
Imports Common.Component.CMSHealthCheck
Imports CommonScheduleJob.Component.ScheduleJobSuspend
Imports Common.ComFunction

Module Core

    Sub Main()
        Call (New ScheduleJob).Start()
    End Sub

End Module

#Region "Extension Functions"

Module StringExtensions

    <Extension()>
    Public Function ContainsValue(ByVal s As String, ByVal strTargetValue As String) As Boolean
        If IsNothing(s) OrElse s.Trim = String.Empty Then Return False

        For Each strValue As String In s.Split(",".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
            If strValue.Trim = strTargetValue Then
                Return True
            End If
        Next

        Return False

    End Function

End Module

#End Region

Public Class ScheduleJob
    Inherits CommonScheduleJob.BaseScheduleJob

#Region "Fields and Properties"

    Public Overrides ReadOnly Property ScheduleJobID() As String
        Get
            Return ""
        End Get
    End Property

    Private Enum AlertType
        NA
        PagerAlert
        EmailAlert
    End Enum

    Private Class RaiseAlert
        Public Const TurnOn As String = "Y"
        Public Const TurnOff As String = "N"
        Public Const OnSchedule As String = "S"
    End Class

    Private Enum HealthCheckType
        CMSHealthCheckFail
        CMSHCSPFail
        CMSSlowResponse
        RSAFail
        EHRSSConnectFail
        EHRSSHealthCheckFail
        EHRSSAutoResilience
        EHRSSRerunJobFail
        EHRSSOutSyncCase
        CIMSHealthCheckFail
        CIMSHCSPFail
        CIMSSlowResponse
        OCSSSHealthCheckFail
        OCSSSSlowResponse
        OCSSSConnectFail
        EHRSSPatientPortalSlowResponse
        HAServicePatientImporter
        COVID19Exporter
        COVID19BatchConfirm
        COVID19DischargeImporter
    End Enum

#End Region

#Region "Supporting Functions"

    Private Sub WriteEventLog(ByVal strEventSource As String, ByVal strEventID As String, ByVal typeEntryType As EventLogEntryType, ByVal strMessage As String)
        strMessage = String.Format("[{0}] {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm"), strMessage)

        Try
            If Not EventLog.SourceExists(strEventSource) Then EventLog.CreateEventSource(strEventSource, "Application")

            Dim udtEventLog As New EventLog

            udtEventLog.Source = strEventSource

            udtEventLog.WriteEntry(strMessage, typeEntryType, CInt(strEventID))

        Catch Ex As Exception
            Console.WriteLine(Ex)
            ' Do nothing

        End Try

    End Sub

    Private Function GetAppSetting(ByVal strKeyName As String) As String
        Dim s As String = ConfigurationManager.AppSettings(strKeyName)

        If IsNothing(s) Then s = String.Empty

        Return s

    End Function

    Private Sub Drill()
        Log("Drill enabled")

        If GetAppSetting("HealthCheckFail_ForcePagerAlert") = "Y" Then
            Log("HealthCheckFail ForcePagerAlert")

            WriteHealthCheckFailAlert(AlertType.PagerAlert, ConfigurationManager.AppSettings("HealthCheckFail_ForceMessage"))
        End If

        If GetAppSetting("HealthCheckFail_ForceEmailAlert") = "Y" Then
            Log("HealthCheckFail ForceEmailAlert")

            WriteHealthCheckFailAlert(AlertType.EmailAlert, ConfigurationManager.AppSettings("HealthCheckFail_ForceMessage"))
        End If

        If GetAppSetting("HCSPFail_ForcePagerAlert") = "Y" Then
            Log("HCSPFail ForcePagerAlert")

            WriteHCSPFailAlert(AlertType.PagerAlert, ConfigurationManager.AppSettings("HCSPFail_ForceMessage"))
        End If

        If GetAppSetting("HCSPFail_ForceEmailAlert") = "Y" Then
            Log("HCSPFail ForceEmailAlert")

            WriteHCSPFailAlert(AlertType.EmailAlert, ConfigurationManager.AppSettings("HCSPFail_ForceMessage"))
        End If

        If GetAppSetting("SlowResponseLv1_ForcePagerAlert") = "Y" Then
            Log("SlowResponseLv1 ForcePagerAlert")

            WriteSlowResponseLv1Alert(AlertType.PagerAlert, ConfigurationManager.AppSettings("SlowResponseLv1_ForceMessage"))
        End If

        If GetAppSetting("SlowResponseLv1_ForceEmailAlert") = "Y" Then
            Log("SlowResponseLv1 ForceEmailAlert")

            WriteSlowResponseLv1Alert(AlertType.EmailAlert, ConfigurationManager.AppSettings("SlowResponseLv1_ForceMessage"))
        End If

        If GetAppSetting("SlowResponseLv2_ForcePagerAlert") = "Y" Then
            Log("SlowResponseLv2 ForcePagerAlert")

            WriteSlowResponseLv2Alert(AlertType.PagerAlert, ConfigurationManager.AppSettings("SlowResponseLv2_ForceMessage"))
        End If

        If GetAppSetting("SlowResponseLv2_ForceEmailAlert") = "Y" Then
            Log("SlowResponseLv2 ForceEmailAlert")

            WriteSlowResponseLv2Alert(AlertType.EmailAlert, ConfigurationManager.AppSettings("SlowResponseLv2_ForceMessage"))
        End If

        If GetAppSetting("RSAFail_ForcePagerAlert") = "Y" Then
            Log("RSAFail ForcePagerAlert")

            WriteRSAFailAlert(AlertType.PagerAlert, ConfigurationManager.AppSettings("RSAFail_ForceMessage"))
        End If

        If GetAppSetting("RSAFail_ForceEmailAlert") = "Y" Then
            Log("RSAFail ForceEmailAlert")

            WriteRSAFailAlert(AlertType.EmailAlert, ConfigurationManager.AppSettings("RSAFail_ForceMessage"))
        End If

        If GetAppSetting("EHRSS_ConnectFail_ForcePagerAlert") = "Y" Then
            Log("EHRSS_ConnectFail ForcePagerAlert")

            WriteEHRSS_ConnectFailAlert(AlertType.PagerAlert, ConfigurationManager.AppSettings("EHRSS_ConnectFail_ForceMessage"))
        End If

        If GetAppSetting("EHRSS_HealthCheckFail_ForcePagerAlert") = "Y" Then
            Log("EHRSS_HealthCheckFail ForcePagerAlert")

            WriteEHRSS_HealthCheckFailAlert(AlertType.PagerAlert, ConfigurationManager.AppSettings("EHRSS_HealthCheckFail_ForceMessage"))
        End If

        If GetAppSetting("EHRSS_AutoResilience_ForcePagerAlert") = "Y" Then
            Log("EHRSS_AutoResilience ForcePagerAlert")

            WriteEHRSS_AutoResilienceAlert(AlertType.PagerAlert, ConfigurationManager.AppSettings("EHRSS_AutoResilience_ForceMessage"))
        End If

        If GetAppSetting("EHRSS_RerunJobFail_ForcePagerAlert") = "Y" Then
            Log("EHRSS_RerunJobFail ForcePagerAlert")

            WriteEHRSS_RerunJobFailAlert(AlertType.PagerAlert, ConfigurationManager.AppSettings("EHRSS_RerunJobFail_ForceMessage"))
        End If

        If GetAppSetting("EHRSS_OutSyncCase_ForcePagerAlert") = "Y" Then
            Log("EHRSS_OutSyncCase ForcePagerAlert")

            WriteEHRSS_OutSyncCaseAlert(AlertType.PagerAlert, ConfigurationManager.AppSettings("EHRSS_OutSyncCase_ForceMessage"))
        End If

        If GetAppSetting("EHRSS_ConnectFail_ForceEmailAlert") = "Y" Then
            Log("EHRSS_ConnectFail ForceEmailAlert")

            WriteEHRSS_ConnectFailAlert(AlertType.EmailAlert, ConfigurationManager.AppSettings("EHRSS_ConnectFail_ForceMessage"))
        End If

        If GetAppSetting("EHRSS_HealthCheckFail_ForceEmailAlert") = "Y" Then
            Log("EHRSS_HealthCheckFail ForceEmailAlert")

            WriteEHRSS_HealthCheckFailAlert(AlertType.EmailAlert, ConfigurationManager.AppSettings("EHRSS_HealthCheckFail_ForceMessage"))
        End If

        If GetAppSetting("EHRSS_AutoResilience_ForceEmailAlert") = "Y" Then
            Log("EHRSS_AutoResilience ForceEmailAlert")

            WriteEHRSS_AutoResilienceAlert(AlertType.EmailAlert, ConfigurationManager.AppSettings("EHRSS_AutoResilience_ForceMessage"))
        End If

        If GetAppSetting("EHRSS_RerunJobFail_ForceEmailAlert") = "Y" Then
            Log("EHRSS_RerunJobFail ForceEmailAlert")

            WriteEHRSS_RerunJobFailAlert(AlertType.EmailAlert, ConfigurationManager.AppSettings("EHRSS_RerunJobFail_ForceMessage"))
        End If

        If GetAppSetting("EHRSS_OutSyncCase_ForceEmailAlert") = "Y" Then
            Log("EHRSS_OutSyncCase ForceEmailAlert")

            WriteEHRSS_OutSyncCaseAlert(AlertType.EmailAlert, ConfigurationManager.AppSettings("EHRSS_OutSyncCase_ForceMessage"))
        End If


        ' CIMS
        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Winnie SUEN]
        ' ----------------------------------------------------------
        If GetAppSetting("CIMS_HealthCheckFail_ForcePagerAlert") = "Y" Then
            Log("CIMS_HealthCheckFail ForcePagerAlert")

            WriteCIMS_HealthCheckFailAlert(AlertType.PagerAlert, ConfigurationManager.AppSettings("CIMS_HealthCheckFail_ForceMessage"))
        End If

        If GetAppSetting("CIMS_HealthCheckFail_ForceEmailAlert") = "Y" Then
            Log("CIMS_HealthCheckFail ForceEmailAlert")

            WriteCIMS_HealthCheckFailAlert(AlertType.EmailAlert, ConfigurationManager.AppSettings("CIMS_HealthCheckFail_ForceMessage"))
        End If

        If GetAppSetting("CIMS_HCSPFail_ForcePagerAlert") = "Y" Then
            Log("CIMS_HCSPFail ForcePagerAlert")

            WriteCIMS_HCSPFailAlert(AlertType.PagerAlert, ConfigurationManager.AppSettings("CIMS_HCSPFail_ForceMessage"))
        End If

        If GetAppSetting("CIMS_HCSPFail_ForceEmailAlert") = "Y" Then
            Log("CIMS_HCSPFail ForceEmailAlert")

            WriteCIMS_HCSPFailAlert(AlertType.EmailAlert, ConfigurationManager.AppSettings("CIMS_HCSPFail_ForceMessage"))
        End If

        If GetAppSetting("CIMS_SlowResponseLv1_ForcePagerAlert") = "Y" Then
            Log("CIMS_SlowResponseLv1 ForcePagerAlert")

            WriteCIMS_SlowResponseLv1Alert(AlertType.PagerAlert, ConfigurationManager.AppSettings("CIMS_SlowResponseLv1_ForceMessage"))
        End If

        If GetAppSetting("CIMS_SlowResponseLv1_ForceEmailAlert") = "Y" Then
            Log("CIMS_SlowResponseLv1 ForceEmailAlert")

            WriteCIMS_SlowResponseLv1Alert(AlertType.EmailAlert, ConfigurationManager.AppSettings("CIMS_SlowResponseLv1_ForceMessage"))
        End If

        If GetAppSetting("CIMS_SlowResponseLv2_ForcePagerAlert") = "Y" Then
            Log("CIMS_SlowResponseLv2 ForcePagerAlert")

            WriteCIMS_SlowResponseLv2Alert(AlertType.PagerAlert, ConfigurationManager.AppSettings("CIMS_SlowResponseLv2_ForceMessage"))
        End If

        If GetAppSetting("CIMS_SlowResponseLv2_ForceEmailAlert") = "Y" Then
            Log("CIMS_SlowResponseLv2 ForceEmailAlert")

            WriteCIMS_SlowResponseLv2Alert(AlertType.EmailAlert, ConfigurationManager.AppSettings("CIMS_SlowResponseLv2_ForceMessage"))
        End If
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Winnie SUEN]

        ' OCSSS
        If GetAppSetting("OCSSS_HealthCheckFail_ForcePagerAlert") = "Y" Then
            Log("OCSSS_HealthCheckFail ForcePagerAlert")

            WriteOCSSS_HealthCheckFailAlert(AlertType.PagerAlert, ConfigurationManager.AppSettings("OCSSS_HealthCheckFail_ForceMessage"))
        End If

        If GetAppSetting("OCSSS_HealthCheckFail_ForceEmailAlert") = "Y" Then
            Log("OCSSS_HealthCheckFail ForceEmailAlert")

            WriteOCSSS_HealthCheckFailAlert(AlertType.EmailAlert, ConfigurationManager.AppSettings("OCSSS_HealthCheckFail_ForceMessage"))
        End If

        If GetAppSetting("OCSSS_SlowResponse_ForcePagerAlert") = "Y" Then
            Log("OCSSS_SlowResponse ForcePagerAlert")

            WriteOCSSS_SlowResponseAlert(AlertType.PagerAlert, ConfigurationManager.AppSettings("OCSSS_SlowResponse_ForceMessage"))
        End If

        If GetAppSetting("OCSSS_SlowResponse_ForceEmailAlert") = "Y" Then
            Log("OCSSS_SlowResponse ForceEmailAlert")

            WriteOCSSS_SlowResponseAlert(AlertType.EmailAlert, ConfigurationManager.AppSettings("OCSSS_SlowResponse_ForceMessage"))
        End If

        If GetAppSetting("OCSSS_ConnectFail_ForcePagerAlert") = "Y" Then
            Log("OCSSS_ConnectFail ForcePagerAlert")

            WriteOCSSS_ConnectFailAlert(AlertType.PagerAlert, ConfigurationManager.AppSettings("OCSSS_ConnectFail_ForceMessage"))
        End If

        If GetAppSetting("OCSSS_ConnectFail_ForceEmailAlert") = "Y" Then
            Log("OCSSS_ConnectFail ForceEmailAlert")

            WriteOCSSS_ConnectFailAlert(AlertType.EmailAlert, ConfigurationManager.AppSettings("OCSSS_ConnectFail_ForceMessage"))
        End If

        Log("Drill end")
    End Sub

    ' CRE17-010 (OCSSS integration) [Start][Winnie SUEN]
    ' ----------------------------------------------------------
    ''' <summary>
    ''' Check whether to raise pager/email alert (on specified time)
    ''' </summary>
    ''' <param name="strRaiseAlert"></param>
    ''' <param name="strFromTime"></param>
    ''' <param name="strToTime"></param>
    ''' <remarks></remarks>
    Private Function IsRaiseAlert(ByVal strRaiseAlert As String,
                                  Optional ByVal strFromTime As String = "",
                                  Optional ByVal strToTime As String = "") As Boolean

        Select Case strRaiseAlert
            Case RaiseAlert.TurnOn
                Return True

            Case RaiseAlert.TurnOff
                Return False

            Case RaiseAlert.OnSchedule
                Dim dtmNow As DateTime = DateTime.Now

                ' From & To time is empty
                If strFromTime = String.Empty And strToTime = String.Empty Then
                    Return False
                End If

                If strFromTime < strToTime Then ' 04:00 - 17:00
                    If dtmNow.ToString("HH:mm") >= strFromTime AndAlso _
                       dtmNow.ToString("HH:mm") <= strToTime Then
                        Return True
                    Else
                        Return False
                    End If
                ElseIf strFromTime > strToTime Then ' 17:00 - 04:00
                    If dtmNow.ToString("HH:mm") >= strFromTime OrElse _
                       dtmNow.ToString("HH:mm") <= strToTime Then
                        Return True
                    Else
                        Return False
                    End If
                ElseIf strFromTime = strToTime Then ' 04:00 - 04:00
                    Return True
                End If
            Case Else
                Throw New Exception(String.Format("Unexpected value (strRaiseAlert={0})", strRaiseAlert))
        End Select

        Return False
    End Function
    ' CRE17-010 (OCSSS integration) [End][Winnie SUEN]

#End Region

    Protected Overrides Sub Process()
        ' Check drill
        If GetAppSetting("EnableDrill") = "Y" Then
            Drill()
            Return ' Exit after the drill
        End If

        Dim dtmNow As DateTime = DateTime.Now

        Dim dtmLastCheck As New List(Of DateTime)

        dtmLastCheck.Insert(HealthCheckType.CMSHealthCheckFail, dtmNow)
        dtmLastCheck.Insert(HealthCheckType.CMSHCSPFail, dtmNow)
        dtmLastCheck.Insert(HealthCheckType.CMSSlowResponse, dtmNow)
        dtmLastCheck.Insert(HealthCheckType.RSAFail, dtmNow)
        dtmLastCheck.Insert(HealthCheckType.EHRSSConnectFail, dtmNow)
        dtmLastCheck.Insert(HealthCheckType.EHRSSHealthCheckFail, dtmNow)
        dtmLastCheck.Insert(HealthCheckType.EHRSSAutoResilience, dtmNow)
        dtmLastCheck.Insert(HealthCheckType.EHRSSRerunJobFail, dtmNow)
        dtmLastCheck.Insert(HealthCheckType.EHRSSOutSyncCase, dtmNow)
        dtmLastCheck.Insert(HealthCheckType.CIMSHealthCheckFail, dtmNow)
        dtmLastCheck.Insert(HealthCheckType.CIMSHCSPFail, dtmNow)
        dtmLastCheck.Insert(HealthCheckType.CIMSSlowResponse, dtmNow)
        dtmLastCheck.Insert(HealthCheckType.OCSSSHealthCheckFail, dtmNow)
        dtmLastCheck.Insert(HealthCheckType.OCSSSSlowResponse, dtmNow)
        dtmLastCheck.Insert(HealthCheckType.OCSSSConnectFail, dtmNow)


        ' CRE20-005 (Providing users' data in HCVS to eHR Patient Portal) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        dtmLastCheck.Insert(HealthCheckType.EHRSSPatientPortalSlowResponse, dtmNow)
        ' CRE20-005 (Providing users' data in HCVS to eHR Patient Portal) [End][Chris YIM]	


        ' CRE20-015 (HA Scheme) check HAServicePatientImporter fail  log 00009 and 000010 [Start][Raiman Chong]
        ' ---------------------------------------------------------------------------------------------------------
        dtmLastCheck.Insert(HealthCheckType.HAServicePatientImporter, dtmNow)
        ' CRE20-015 (HA Scheme) check HAServicePatientImporter fail  log 00009 and 000010 [Start][Raiman Chong]

        'CRE20-0023 (Immu record) [Start][Winnie SUEN]
        ' ---------------------------------------------------------------------------------------------------------
        dtmLastCheck.Insert(HealthCheckType.COVID19Exporter, dtmNow)
        dtmLastCheck.Insert(HealthCheckType.COVID19BatchConfirm, dtmNow)
        'CRE20-0023 (Immu record) [End][Winnie SUEN]


        'CRE20-0XX (Immu record) [Start][Raiman Chong]
        dtmLastCheck.Insert(HealthCheckType.COVID19DischargeImporter, dtmNow)
        'CRE20-0XX (Immu record) [End][Raiman Chong]



        CheckTime.ReadCheckTime(dtmLastCheck)

        Try

            CheckHealthCheckFail(dtmLastCheck(HealthCheckType.CMSHealthCheckFail), dtmNow)

            CheckHCSPFail(dtmLastCheck(HealthCheckType.CMSHCSPFail), dtmNow)

            CheckSlowResponse(dtmLastCheck(HealthCheckType.CMSSlowResponse), dtmNow)

            CheckRSAFail(dtmLastCheck(HealthCheckType.RSAFail), dtmNow)

            CheckEHRSS_ConnectFail(dtmLastCheck(HealthCheckType.EHRSSConnectFail), dtmNow)

            CheckEHRSS_HealthCheckFail(dtmLastCheck(HealthCheckType.EHRSSHealthCheckFail), dtmNow)

            CheckEHRSS_AutoResilience(dtmLastCheck(HealthCheckType.EHRSSAutoResilience), dtmNow)

            CheckEHRSS_RerunJobFail(dtmLastCheck(HealthCheckType.EHRSSRerunJobFail), dtmNow)

            CheckEHRSS_OutSyncCase(dtmLastCheck(HealthCheckType.EHRSSOutSyncCase), dtmNow)

            CheckCIMS_HealthCheckFail(dtmLastCheck(HealthCheckType.CIMSHealthCheckFail), dtmNow)

            CheckCIMS_HCSPFail(dtmLastCheck(HealthCheckType.CIMSHCSPFail), dtmNow)

            CheckCIMS_SlowResponse(dtmLastCheck(HealthCheckType.CIMSSlowResponse), dtmNow)

            CheckOCSSS_HealthCheckFail(dtmLastCheck(HealthCheckType.OCSSSHealthCheckFail), dtmNow)

            CheckOCSSS_SlowResponse(dtmLastCheck(HealthCheckType.OCSSSSlowResponse), dtmNow)

            CheckOCSSS_ConnectFail(dtmLastCheck(HealthCheckType.OCSSSConnectFail), dtmNow)

            ' CRE20-005 (Providing users' data in HCVS to eHR Patient Portal) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            CheckEHRSS_PP_SlowResponse(dtmLastCheck(HealthCheckType.EHRSSPatientPortalSlowResponse), dtmNow)
            ' CRE20-005 (Providing users' data in HCVS to eHR Patient Portal) [End][Chris YIM]	



            ' CRE20-015 (HA Scheme) check HAServicePatientImporter fail  log 00009 and 000010 [Start][Raiman Chong]
            ' ---------------------------------------------------------------------------------------------------------
            CheckHASPImporter_ImportFail(dtmLastCheck(HealthCheckType.HAServicePatientImporter), dtmNow)
            ' CRE20-015 (HA Scheme) check HAServicePatientImporter fail  log 00009 and 000010 [Start][Raiman Chong]

            'CRE20-0022 (Immu record) [Start][Martin Tang]
            CheckCOVID19Exporter_ExportFail(dtmLastCheck(HealthCheckType.COVID19Exporter), dtmNow)
            'CRE20-0022 (Immu record) [End][Martin Tang]

            'CRE20-0023 (Immu record) [Start][Winnie SUEN]
            CheckCOVID19BatchConfirm_Fail(dtmLastCheck(HealthCheckType.COVID19BatchConfirm), dtmNow)
            'CRE20-0023 (Immu record) [End][Winnie SUEN]

            'CRE20-0XX (Immu record) [Start][Raiman Chong]
            CheckCOVID19DischargeImporter_ImportFail(dtmLastCheck(HealthCheckType.COVID19DischargeImporter), dtmNow)
            'CRE20-0XX (Immu record) [End][Raiman Chong]



            CheckTime.WriteCheckTime(dtmLastCheck)

            Log("Monitor Task Completed")

        Catch ex As Exception
            Log(String.Format("Exception : {0}", ex.Message))
        End Try

    End Sub

#Region "CMS"
    Private Sub CheckHealthCheckFail(ByRef dtmLastCheck As DateTime, ByVal dtmCurrent As DateTime)
        ' +-------------------------------------------------------------------------------+
        ' | For every [5] minutes, monitor the database table InterfaceHealthCheckLog.    |
        ' | If the number of fail count reaches [1], raise alert.                         |
        ' +-------------------------------------------------------------------------------+

        Log("Checking HealthCheckFail")

        ' Check whether need to run
        If DateDiff(DateInterval.Minute, dtmLastCheck, dtmCurrent) < CInt(ConfigurationManager.AppSettings("HealthCheckFail_CheckInterval")) Then
            Log("Smaller than CheckInterval, no need to run")

            Return
        End If

        ' Update now to be the new LastCheckTime
        dtmLastCheck = dtmCurrent

        ' Check logs in database
        Dim strHealthCheckInterfaceCode As String = ConfigurationManager.AppSettings("HealthCheckFail_InterfaceCode")
        Dim strHealthCheckFailLogID As String = ConfigurationManager.AppSettings("HealthCheckFail_FailLogID")
        Dim strPagerFunctionCode As String = ConfigurationManager.AppSettings("HealthCheckFail_PagerFunctionCode")
        Dim strEmailFunctionCode As String = ConfigurationManager.AppSettings("HealthCheckFail_EmailFunctionCode")

        Dim lstHealthCheckFunctionCodeList As Collection = Nothing
        Call (New CMSHealthCheckBLL).GetUsingEndpointURLList(lstHealthCheckFunctionCodeList)

        Dim strHealthCheckFailCount As Integer = CInt(ConfigurationManager.AppSettings("HealthCheckFail_FailCount"))
        Dim strMessage As String = String.Format("(EHS->CMS) Connection Fail count in last {0} Health Check:", strHealthCheckFailCount)
        Dim blnPagerAlert As Boolean = False
        Dim blnEmailAlert As Boolean = False

        For i As Integer = 1 To lstHealthCheckFunctionCodeList.Count
            Dim dt As DataTable = MonitorBLL.GetInterfaceHealthCheckLog(strHealthCheckInterfaceCode, lstHealthCheckFunctionCodeList(i), _
                                                                        Nothing, strHealthCheckFailCount)

            Dim drConnectionFail() As DataRow = dt.Select(String.Format("Log_ID = '{0}'", strHealthCheckFailLogID))

            If drConnectionFail.Length >= strHealthCheckFailCount Then
                Dim strSiteName As String = String.Empty

                If i = 1 Then
                    strSiteName = "Primary Site"
                Else
                    strSiteName = String.Format("Secondary Site {0}", i - 1)
                End If

                strMessage = strMessage & String.Format(" [EAI {0}] is {1}.", strSiteName, drConnectionFail.Length)

                If strPagerFunctionCode = "ALL" OrElse strPagerFunctionCode.ContainsValue(lstHealthCheckFunctionCodeList(i)) Then
                    blnPagerAlert = True
                End If

                If strEmailFunctionCode = "ALL" OrElse strEmailFunctionCode.ContainsValue(lstHealthCheckFunctionCodeList(i)) Then
                    blnEmailAlert = True
                End If

            End If
        Next

        If blnPagerAlert Then
            Log("HealthCheckFail pager alert")

            WriteHealthCheckFailAlert(AlertType.PagerAlert, strMessage)

        End If

        If blnEmailAlert Then
            Log("HealthCheckFail email alert")

            WriteHealthCheckFailAlert(AlertType.EmailAlert, strMessage)
        End If

        Log("Completed checking HealthCheckFail")

    End Sub

    Private Sub WriteHealthCheckFailAlert(ByVal eAlertType As AlertType, ByVal strMessage As String)
        Select Case eAlertType
            Case AlertType.PagerAlert
                WriteEventLog(ConfigurationManager.AppSettings("HealthCheckFail_EventSource"), _
                              ConfigurationManager.AppSettings("HealthCheckFail_PagerEventID"), _
                              EventLogEntryType.Error, strMessage)

            Case AlertType.EmailAlert
                WriteEventLog(ConfigurationManager.AppSettings("HealthCheckFail_EventSource"), _
                              ConfigurationManager.AppSettings("HealthCheckFail_EmailEventID"), _
                              EventLogEntryType.Warning, strMessage)

            Case Else
                Throw New NotImplementedException

        End Select

    End Sub

    '

    Private Sub CheckHCSPFail(ByRef dtmLastCheck As DateTime, ByVal dtmCurrent As DateTime)
        ' +-------------------------------------------------------------------------------+
        ' | For every [5] minutes, monitor the HCSP logs. If there are at least [3] fail  |
        ' | logs (no matter consecutive or not, raise error.                              |
        ' +-------------------------------------------------------------------------------+

        Log("Checking HCSPFail")

        ' Check whether need to run
        If DateDiff(DateInterval.Minute, dtmLastCheck, dtmCurrent) < CInt(ConfigurationManager.AppSettings("HCSPFail_CheckInterval")) Then
            Log("Smaller than CheckInterval, no need to run")

            Return
        End If

        ' Update now to be the new LastCheckTime
        dtmLastCheck = dtmCurrent

        ' Check logs
        Dim intAuditLogMinuteBefore As Integer = CInt(ConfigurationManager.AppSettings("HCSPFail_AuditLogMinuteBefore"))
        Dim intAuditLogEnquirySystem As String = ConfigurationManager.AppSettings("HCSPFail_EnquirySystem")

        Dim intFailCount As Integer = MonitorBLL.GetHCSPFailAuditLog(intAuditLogEnquirySystem, intAuditLogMinuteBefore)

        If intFailCount >= CInt(ConfigurationManager.AppSettings("HCSPFail_FailCount")) Then
            Dim strMessage As String = String.Format("(EHS->CMS) {0} fail logs found in HCSP vaccination enquiry in the past {1} mins", _
                                                     intFailCount, intAuditLogMinuteBefore)

            Log("HCSPFail pager and email alert")

            WriteHCSPFailAlert(AlertType.PagerAlert, strMessage)
            WriteHCSPFailAlert(AlertType.EmailAlert, strMessage)

        End If

        Log("Completed checking HCSPFail")

    End Sub

    Private Sub WriteHCSPFailAlert(ByVal eAlertType As AlertType, ByVal strMessage As String)
        Select Case eAlertType
            Case AlertType.PagerAlert
                WriteEventLog(ConfigurationManager.AppSettings("HCSPFail_EventSource"), _
                              ConfigurationManager.AppSettings("HCSPFail_PagerEventID"), _
                              EventLogEntryType.Error, strMessage)

            Case AlertType.EmailAlert
                WriteEventLog(ConfigurationManager.AppSettings("HCSPFail_EventSource"), _
                              ConfigurationManager.AppSettings("HCSPFail_EmailEventID"), _
                              EventLogEntryType.Warning, strMessage)

            Case Else
                Throw New NotImplementedException

        End Select

    End Sub

    '

    Private Sub CheckSlowResponse(ByRef dtmLastCheck As DateTime, ByVal dtmCurrent As DateTime)
        ' +-------------------------------------------------------------------------------+
        ' | For every [5] minutes, monitor the CMS->EHS InterfaceLog. If there are at     |
        ' | least [1] requests of:                                                        |
        ' |   >=[6] seconds, raise alert.                                                 |
        ' |   >=[9] seconds, raise another separate alert.                                |
        ' +-------------------------------------------------------------------------------+

        Log("Checking SlowResponse")

        ' Check whether need to run
        If DateDiff(DateInterval.Minute, dtmLastCheck, dtmCurrent) < CInt(ConfigurationManager.AppSettings("SlowResponse_CheckInterval")) Then
            Log("Smaller than CheckInterval, no need to run")

            Return
        End If

        ' Update now to be the new LastCheckTime
        dtmLastCheck = dtmCurrent

        ' Check logs
        Dim strAuditLogFunctionCode As String = ConfigurationManager.AppSettings("SlowResponse_AuditLogFunctionCode")
        Dim strAuditLogLogID As String = ConfigurationManager.AppSettings("SlowResponse_AuditLogLogID")
        Dim intAuditLogMinuteBefore As Integer = CInt(ConfigurationManager.AppSettings("SlowResponse_AuditLogMinuteBefore"))
        Dim intAuditLogRequestSystem As String = ConfigurationManager.AppSettings("SlowResponse_AuditLogRequestSystem")

        Dim dt As DataTable = MonitorBLL.GetInterfaceLogProcessTimeByMinuteBefore(strAuditLogFunctionCode, strAuditLogLogID, intAuditLogMinuteBefore, intAuditLogRequestSystem)

        Dim intSlowResponseCount As Integer = CInt(ConfigurationManager.AppSettings("SlowResponse_FailCount"))

        ' Level 1: If any process on CMS->EHS Interface >=[6] seconds, raise alert
        Dim intLv1Value As Integer = CInt(ConfigurationManager.AppSettings("SlowResponse_Level1Value"))

        Dim drLv1 As DataRow() = dt.Select(String.Format("Time_Diff >= {0}", intLv1Value * 1000))

        If drLv1.Length >= intSlowResponseCount Then
            Dim strMessage As String = String.Format("(CMS->EHS) {0}/{1} Slow Performance Record count (>={2}s) in the past {3} mins [threshold: {4}]", _
                                                     drLv1.Length, _
                                                     dt.Rows.Count, _
                                                     intLv1Value, _
                                                     intAuditLogMinuteBefore, _
                                                     intSlowResponseCount)

            Log("SlowResponse Lv1 pager and email alert")

            WriteSlowResponseLv1Alert(AlertType.PagerAlert, strMessage)
            WriteSlowResponseLv1Alert(AlertType.EmailAlert, strMessage)

        End If

        ' Level 2: If any process on CMS->EHS Interface >=[9] seconds, raise alert
        Dim intLv2Value As Integer = CInt(ConfigurationManager.AppSettings("SlowResponse_Level2Value"))

        Dim drLv2 As DataRow() = dt.Select(String.Format("Time_Diff >= {0}", intLv2Value * 1000))

        If drLv2.Length >= intSlowResponseCount Then
            Dim strMessage As String = String.Format("(CMS->EHS) {0}/{1} Slow Performance Record count (>={2}s) in the past {3} mins [threshold: {4}]", _
                                                     drLv2.Length, _
                                                     dt.Rows.Count, _
                                                     intLv2Value, _
                                                     intAuditLogMinuteBefore, _
                                                     intSlowResponseCount)

            Log("SlowResponse Lv2 pager and email alert")

            WriteSlowResponseLv2Alert(AlertType.PagerAlert, strMessage)
            WriteSlowResponseLv2Alert(AlertType.EmailAlert, strMessage)

        End If

        Log("Completed checking SlowResponse")

    End Sub

    Private Sub WriteSlowResponseLv1Alert(ByVal eAlertType As AlertType, ByVal strMessage As String)
        Select Case eAlertType
            Case AlertType.PagerAlert
                WriteEventLog(ConfigurationManager.AppSettings("SlowResponseLv1_EventSource"), _
                              ConfigurationManager.AppSettings("SlowResponseLv1_PagerEventID"), _
                              EventLogEntryType.Error, strMessage)

            Case AlertType.EmailAlert
                WriteEventLog(ConfigurationManager.AppSettings("SlowResponseLv1_EventSource"), _
                              ConfigurationManager.AppSettings("SlowResponseLv1_EmailEventID"), _
                              EventLogEntryType.Warning, strMessage)

            Case Else
                Throw New NotImplementedException

        End Select

    End Sub

    Private Sub WriteSlowResponseLv2Alert(ByVal eAlertType As AlertType, ByVal strMessage As String)
        Select Case eAlertType
            Case AlertType.PagerAlert
                WriteEventLog(ConfigurationManager.AppSettings("SlowResponseLv2_EventSource"), _
                              ConfigurationManager.AppSettings("SlowResponseLv2_PagerEventID"), _
                              EventLogEntryType.Error, strMessage)

            Case AlertType.EmailAlert
                WriteEventLog(ConfigurationManager.AppSettings("SlowResponseLv2_EventSource"), _
                              ConfigurationManager.AppSettings("SlowResponseLv2_EmailEventID"), _
                              EventLogEntryType.Warning, strMessage)

            Case Else
                Throw New NotImplementedException

        End Select

    End Sub

#End Region

    '
#Region "RSA"
    Private Sub CheckRSAFail(ByRef dtmLastCheck As DateTime, ByVal dtmCurrent As DateTime)
        ' +-------------------------------------------------------------------------------+
        ' | For every [5] minutes, monitor the HCSP and HCVU audit logs to see if any     |
        ' | RSA fail cases. If there is any case, raise alert.                            |
        ' | For the AceInitializeEx fail which can be solved by retry mechanism, it will  |
        ' | only raise email alert.                                                       |
        ' +-------------------------------------------------------------------------------+

        Log("Checking RSAFail")

        ' Check whether need to run
        If DateDiff(DateInterval.Minute, dtmLastCheck, dtmCurrent) < CInt(ConfigurationManager.AppSettings("RSAFail_CheckInterval")) Then
            Log("Smaller than CheckInterval, no need to run")

            Return
        End If

        ' Update now to be the new LastCheckTime
        dtmLastCheck = dtmCurrent

        ' Check logs
        Dim intAuditLogMinuteBefore As Integer = CInt(ConfigurationManager.AppSettings("RSAFail_AuditLogMinuteBefore"))
        Dim blnPagerAlert As Boolean = False
        Dim blnEmailAlert As Boolean = False
        Dim strMessage As String = String.Empty

        Dim dt As DataTable = MonitorBLL.GetRSAFailCount(dtmCurrent.Add(New TimeSpan(0, -1 * intAuditLogMinuteBefore, 0)), dtmCurrent)

        If dt.Rows.Count > 0 Then
            Dim lstContent As New List(Of String)

            For Each dr As DataRow In dt.Rows
                lstContent.Add(String.Format("{0} error in RSA {1} ({2}) in {3} platform", dr("Error_Count"), dr("RSA_Version"), dr("Main_Sub"), dr("Platform")))

                If dr("Raise_Pager_Alert") = "Y" Then blnPagerAlert = True
                If dr("Raise_Email_Alert") = "Y" Then blnEmailAlert = True

            Next

            strMessage = String.Format("RSA fail: {0} in the past {1} mins", String.Join(", ", lstContent.ToArray), intAuditLogMinuteBefore)

        End If

        If blnPagerAlert Then
            Log("RSAFail pager alert")

            WriteRSAFailAlert(AlertType.PagerAlert, strMessage)
        End If

        If blnEmailAlert Then
            Log("RSAFail email alert")

            WriteRSAFailAlert(AlertType.EmailAlert, strMessage)
        End If

        Log("Completed checking RSAFail")

    End Sub

    Private Sub WriteRSAFailAlert(ByVal eAlertType As AlertType, ByVal strMessage As String)
        Select Case eAlertType
            Case AlertType.PagerAlert
                WriteEventLog(ConfigurationManager.AppSettings("RSAFail_EventSource"), _
                              ConfigurationManager.AppSettings("RSAFail_PagerEventID"), _
                              EventLogEntryType.Error, strMessage)

            Case AlertType.EmailAlert
                WriteEventLog(ConfigurationManager.AppSettings("RSAFail_EventSource"), _
                              ConfigurationManager.AppSettings("RSAFail_EmailEventID"), _
                              EventLogEntryType.Warning, strMessage)

            Case Else
                Throw New NotImplementedException

        End Select

    End Sub
#End Region

#Region "eHRSS"
    ' CRE16-019 (To implement token sharing between eHS(S) and eHRSS) [Start][Winnie]
    Private Sub CheckEHRSS_ConnectFail(ByRef dtmLastCheck As DateTime, ByVal dtmCurrent As DateTime)
        ' +-------------------------------------------------------------------------------+
        ' | For every [5] minutes, monitor the interface audit logs to see if any eHRSS   |
        ' | connection fail cases. If there is any case, raise alert.                     |
        ' | May have 2 alert at one time (Primary Site & All endpoint)                    |
        ' +-------------------------------------------------------------------------------+

        Log("Checking EHRSS_ConnectFail")

        ' Check whether need to run
        If DateDiff(DateInterval.Minute, dtmLastCheck, dtmCurrent) < CInt(ConfigurationManager.AppSettings("EHRSS_ConnectFail_CheckInterval")) Then
            Log("Smaller than CheckInterval, no need to run")

            Return
        End If

        ' Update now to be the new LastCheckTime
        dtmLastCheck = dtmCurrent

        ' Check logs
        Dim intAuditLogMinuteBefore As Integer = CInt(ConfigurationManager.AppSettings("EHRSS_ConnectFail_AuditLogMinuteBefore"))
        Dim strPriFailPagerAlert As String = ConfigurationManager.AppSettings("EHRSS_ConnectFail_PriFail_PagerAlert")
        Dim strPriFailEmailAlert As String = ConfigurationManager.AppSettings("EHRSS_ConnectFail_PriFail_EmailAlert")
        Dim strAllFailPagerAlert As String = ConfigurationManager.AppSettings("EHRSS_ConnectFail_AllFail_PagerAlert")
        Dim strAllFailEmailAlert As String = ConfigurationManager.AppSettings("EHRSS_ConnectFail_AllFail_EmailAlert")

        Dim ds As DataSet = MonitorBLL.GetEHRConnectFailLog(dtmCurrent.Add(New TimeSpan(0, -1 * intAuditLogMinuteBefore, 0)), dtmCurrent)

        For intIndex As Integer = 0 To ds.Tables.Count() - 1
            Dim blnPagerAlert As Boolean = False
            Dim blnEmailAlert As Boolean = False
            Dim strMessage As String = String.Empty

            Dim dt As DataTable = ds.Tables(intIndex)

            If dt.Rows.Count > 0 Then
                If intIndex = 0 Then
                    ' Primary endpoint connection fail
                    strMessage = "eHRSS Token Service fail: {0} in the past {1} mins"

                    Dim lstContent As New List(Of String)

                    For Each dr As DataRow In dt.Rows
                        lstContent.Add(String.Format("{0} error in e{1} in {2} platform", dr("Error_Count"), dr("Site").ToString.Trim, dr("Platform").ToString.Trim))
                    Next

                    If strPriFailPagerAlert = "Y" Then blnPagerAlert = True
                    If strPriFailEmailAlert = "Y" Then blnEmailAlert = True

                    strMessage = String.Format(strMessage, String.Join(", ", lstContent.ToArray), intAuditLogMinuteBefore)

                Else
                    ' All endpoint connection fail
                    strMessage = "(All endpoint failure) eHRSS Token Service fail in the past {0} mins: {1} error"
                    If strAllFailPagerAlert = "Y" Then blnPagerAlert = True
                    If strAllFailEmailAlert = "Y" Then blnEmailAlert = True

                    strMessage = String.Format(strMessage, intAuditLogMinuteBefore, dt.Rows(0)("Error_Count"))
                End If
            End If

            If blnPagerAlert Then
                Log("EHRSS_ConnectFail pager alert")

                WriteEHRSS_ConnectFailAlert(AlertType.PagerAlert, strMessage)
            End If

            If blnEmailAlert Then
                Log("EHRSS_ConnectFail email alert")

                WriteEHRSS_ConnectFailAlert(AlertType.EmailAlert, strMessage)
            End If
        Next

        Log("Completed checking EHRSS_ConnectFail")

    End Sub

    Private Sub WriteEHRSS_ConnectFailAlert(ByVal eAlertType As AlertType, ByVal strMessage As String)
        Select Case eAlertType
            Case AlertType.PagerAlert
                WriteEventLog(ConfigurationManager.AppSettings("EHRSS_ConnectFail_EventSource"), _
                              ConfigurationManager.AppSettings("EHRSS_ConnectFail_PagerEventID"), _
                              EventLogEntryType.Error, strMessage)

            Case AlertType.EmailAlert
                WriteEventLog(ConfigurationManager.AppSettings("EHRSS_ConnectFail_EventSource"), _
                              ConfigurationManager.AppSettings("EHRSS_ConnectFail_EmailEventID"), _
                              EventLogEntryType.Warning, strMessage)

            Case Else
                Throw New NotImplementedException

        End Select

    End Sub

    Private Sub CheckEHRSS_HealthCheckFail(ByRef dtmLastCheck As DateTime, ByVal dtmCurrent As DateTime)
        ' +-------------------------------------------------------------------------------+
        ' | For every [5] minutes, monitor the database table InterfaceHealthCheckLog.    |
        ' | If any fail case, raise alert. If all active endpoint health check fail,      |
        ' | (All endpoint failure) would be shown on the message.                         |
        ' +-------------------------------------------------------------------------------+

        Log("Checking EHRSS_HealthCheckFail")

        ' Check whether need to run
        If DateDiff(DateInterval.Minute, dtmLastCheck, dtmCurrent) < CInt(ConfigurationManager.AppSettings("EHRSS_HealthCheckFail_CheckInterval")) Then
            Log("Smaller than CheckInterval, no need to run")

            Return
        End If

        ' Update now to be the new LastCheckTime
        dtmLastCheck = dtmCurrent

        ' Check logs in database
        Dim strEHRSS_HealthCheckInterfaceCode As String = ConfigurationManager.AppSettings("EHRSS_HealthCheckFail_InterfaceCode")
        Dim strEHRSS_HealthCheckSuccessLogID As String = ConfigurationManager.AppSettings("EHRSS_HealthCheckFail_SuccessLogID")
        Dim strEHRSS_HealthCheckFailLogID As String = ConfigurationManager.AppSettings("EHRSS_HealthCheckFail_FailLogID")

        Dim strPagerFunctionCode As String = ConfigurationManager.AppSettings("EHRSS_HealthCheckFail_PagerFunctionCode")
        Dim strEmailFunctionCode As String = ConfigurationManager.AppSettings("EHRSS_HealthCheckFail_EmailFunctionCode")
        Dim strSingleFailPagerAlert As String = ConfigurationManager.AppSettings("EHRSS_HealthCheckFail_SingleFail_PagerAlert")
        Dim strSingleFailEmailAlert As String = ConfigurationManager.AppSettings("EHRSS_HealthCheckFail_SingleFail_EmailAlert")

        Dim strAllFailPagerAlert As String = ConfigurationManager.AppSettings("EHRSS_HealthCheckFail_AllFail_PagerAlert")
        Dim strAllFailEmailAlert As String = ConfigurationManager.AppSettings("EHRSS_HealthCheckFail_AllFail_EmailAlert")

        Dim strValue As String = (New GeneralFunction).getSystemParameter("eHRSS_WS_Priority")

        Dim strMessage As String = "Connection fail in last Health Check: Fail site="
        Dim blnPagerAlert As Boolean = False
        Dim blnEmailAlert As Boolean = False
        Dim strFailSite As String = String.Empty
        Dim intSuccessCount As Integer = 0
        Dim intFailCount As Integer = 0


        For Each strSite As String In strValue.Split(",".ToCharArray, StringSplitOptions.RemoveEmptyEntries)

            Dim strFunctionCode As String = strSite.Replace("DC", "HEALTH") 'e.g. HEALTH1

            Dim dt As DataTable = MonitorBLL.GetInterfaceHealthCheckLog(strEHRSS_HealthCheckInterfaceCode, strFunctionCode, Nothing, 1)

            If dt.Rows.Count > 0 Then
                Select Case dt.Rows(0)("Log_ID")

                    Case strEHRSS_HealthCheckSuccessLogID
                        ' Health Check Success
                        intSuccessCount += 1

                    Case strEHRSS_HealthCheckFailLogID
                        ' Health Check Fail
                        If strSingleFailPagerAlert = "Y" Then
                            If strPagerFunctionCode = "ALL" OrElse strPagerFunctionCode.ContainsValue(strFunctionCode) Then
                                blnPagerAlert = True
                            End If
                        End If

                        If strSingleFailEmailAlert = "Y" Then
                            If strEmailFunctionCode = "ALL" OrElse strEmailFunctionCode.ContainsValue(strFunctionCode) Then
                                blnEmailAlert = True
                            End If
                        End If

                        strFailSite += String.Format("[e{0}],", strSite)
                        intFailCount += 1

                End Select
            End If

        Next

        If intFailCount > 0 Then
            strMessage = strMessage & strFailSite.Remove(strFailSite.Length - 1)

            ' All endpoint fail (Exclude suspended)
            If intSuccessCount = 0 Then
                strMessage = "(All endpoint failure) " & strMessage

                If strAllFailPagerAlert = "Y" Then
                    blnPagerAlert = True
                Else
                    blnPagerAlert = False
                End If

                If strAllFailEmailAlert = "Y" Then
                    blnEmailAlert = True
                Else
                    blnEmailAlert = False
                End If
            End If
        End If

        If blnPagerAlert Then
            Log("EHRSS_HealthCheckFail pager alert")

            WriteEHRSS_HealthCheckFailAlert(AlertType.PagerAlert, strMessage)

        End If

        If blnEmailAlert Then
            Log("EHRSS_HealthCheckFail email alert")

            WriteEHRSS_HealthCheckFailAlert(AlertType.EmailAlert, strMessage)
        End If

        Log("Completed checking EHRSS_HealthCheckFail")

    End Sub

    Private Sub WriteEHRSS_HealthCheckFailAlert(ByVal eAlertType As AlertType, ByVal strMessage As String)
        Select Case eAlertType
            Case AlertType.PagerAlert
                WriteEventLog(ConfigurationManager.AppSettings("EHRSS_HealthCheckFail_EventSource"), _
                              ConfigurationManager.AppSettings("EHRSS_HealthCheckFail_PagerEventID"), _
                              EventLogEntryType.Error, strMessage)

            Case AlertType.EmailAlert
                WriteEventLog(ConfigurationManager.AppSettings("EHRSS_HealthCheckFail_EventSource"), _
                              ConfigurationManager.AppSettings("EHRSS_HealthCheckFail_EmailEventID"), _
                              EventLogEntryType.Warning, strMessage)

            Case Else
                Throw New NotImplementedException

        End Select

    End Sub

    Private Sub CheckEHRSS_AutoResilience(ByRef dtmLastCheck As DateTime, ByVal dtmCurrent As DateTime)
        ' +-------------------------------------------------------------------------------+
        ' | For every [5] minutes, monitor the interface audit logs to see if any eHRSS   |
        ' | auto resilience cases. If there is any case, raise alert.                     |
        ' +-------------------------------------------------------------------------------+

        Log("Checking EHRSS_AutoResilience")

        ' Check whether need to run
        If DateDiff(DateInterval.Minute, dtmLastCheck, dtmCurrent) < CInt(ConfigurationManager.AppSettings("EHRSS_AutoResilience_CheckInterval")) Then
            Log("Smaller than CheckInterval, no need to run")

            Return
        End If

        ' Update now to be the new LastCheckTime
        dtmLastCheck = dtmCurrent

        ' Check logs
        Dim intAuditLogMinuteBefore As Integer = CInt(ConfigurationManager.AppSettings("EHRSS_AutoResilience_AuditLogMinuteBefore"))
        Dim strAuditLogFunctionCode As String = ConfigurationManager.AppSettings("EHRSS_AutoResilience_AuditLogFunctionCode")
        Dim strAuditLogLogID As String = ConfigurationManager.AppSettings("EHRSS_AutoResilience_AuditLogLogID")
        Dim strPagerAlert As String = ConfigurationManager.AppSettings("EHRSS_AutoResilience_PagerAlert")
        Dim strEmailAlert As String = ConfigurationManager.AppSettings("EHRSS_AutoResilience_EmailAlert")

        Dim dt As DataTable = MonitorBLL.GetInterfaceLogProcessTimeByMinuteBefore(strAuditLogFunctionCode, strAuditLogLogID, intAuditLogMinuteBefore, Nothing)
        Dim strMessage As String = String.Empty
        Dim blnPagerAlert As Boolean = False
        Dim blnEmailAlert As Boolean = False

        If dt.Rows.Count > 0 Then

            If strPagerAlert = "Y" Then blnPagerAlert = True
            If strEmailAlert = "Y" Then blnEmailAlert = True

            ' Get the latest switch status
            strMessage = Replace(dt.Rows(0)("Description"), "DC", "eDC")
        End If

        If blnPagerAlert Then
            Log("EHRSS_AutoResilience pager alert")

            WriteEHRSS_AutoResilienceAlert(AlertType.PagerAlert, strMessage)
        End If

        If blnEmailAlert Then
            Log("EHRSS_AutoResilience email alert")

            WriteEHRSS_AutoResilienceAlert(AlertType.EmailAlert, strMessage)
        End If

        Log("Completed checking EHRSS_AutoResilience")

    End Sub

    Private Sub WriteEHRSS_AutoResilienceAlert(ByVal eAlertType As AlertType, ByVal strMessage As String)
        Select Case eAlertType
            Case AlertType.PagerAlert
                WriteEventLog(ConfigurationManager.AppSettings("EHRSS_AutoResilience_EventSource"), _
                              ConfigurationManager.AppSettings("EHRSS_AutoResilience_PagerEventID"), _
                              EventLogEntryType.Error, strMessage)

            Case AlertType.EmailAlert
                WriteEventLog(ConfigurationManager.AppSettings("EHRSS_AutoResilience_EventSource"), _
                              ConfigurationManager.AppSettings("EHRSS_AutoResilience_EmailEventID"), _
                              EventLogEntryType.Warning, strMessage)

            Case Else
                Throw New NotImplementedException

        End Select

    End Sub

    Private Sub CheckEHRSS_RerunJobFail(ByRef dtmLastCheck As DateTime, ByVal dtmCurrent As DateTime)
        ' +-------------------------------------------------------------------------------+
        ' | For every [5] minutes, monitor the ScheduleJobLog to see if any eHRSS         |
        ' | rerun job fail case in every night. If there is any case, raise alert.        |
        ' +-------------------------------------------------------------------------------+

        Log("Checking EHRSS_RerunJobFail")

        ' Check whether need to run
        If DateDiff(DateInterval.Minute, dtmLastCheck, dtmCurrent) < CInt(ConfigurationManager.AppSettings("EHRSS_RerunJobFail_CheckInterval")) Then
            Log("Smaller than CheckInterval, no need to run")

            Return
        End If

        ' Update now to be the new LastCheckTime
        dtmLastCheck = dtmCurrent

        ' Check logs
        Dim intScheduleJobLogMinuteBefore As Integer = CInt(ConfigurationManager.AppSettings("EHRSS_RerunJobFail_ScheduleJobLogMinuteBefore"))
        Dim strScheduleJobLogProgramID As String = ConfigurationManager.AppSettings("EHRSS_RerunJobFail_ProgramID")
        Dim strScheduleJobLogID As String = ConfigurationManager.AppSettings("EHRSS_RerunJobFail_FailLogID")
        Dim strPagerAlert As String = ConfigurationManager.AppSettings("EHRSS_RerunJobFail_PagerAlert")
        Dim strEmailAlert As String = ConfigurationManager.AppSettings("EHRSS_RerunJobFail_EmailAlert")

        Dim dt As DataTable = MonitorBLL.GetScheduleJobLog(strScheduleJobLogProgramID, strScheduleJobLogID, _
                                                           dtmCurrent.Add(New TimeSpan(0, -1 * intScheduleJobLogMinuteBefore, 0)), _
                                                           dtmCurrent)
        Dim strMessage As String = String.Empty
        Dim blnPagerAlert As Boolean = False
        Dim blnEmailAlert As Boolean = False

        If dt.Rows.Count > 0 Then

            If strPagerAlert = "Y" Then blnPagerAlert = True
            If strEmailAlert = "Y" Then blnEmailAlert = True

            ' Get the latest description
            strMessage = dt.Rows(0)("Description")
        End If

        If blnPagerAlert Then
            Log("EHRSS_RerunJobFail pager alert")

            WriteEHRSS_RerunJobFailAlert(AlertType.PagerAlert, strMessage)
        End If

        If blnEmailAlert Then
            Log("EHRSS_RerunJobFail email alert")

            WriteEHRSS_RerunJobFailAlert(AlertType.EmailAlert, strMessage)
        End If

        Log("Completed checking EHRSS_RerunJobFail")

    End Sub

    Private Sub WriteEHRSS_RerunJobFailAlert(ByVal eAlertType As AlertType, ByVal strMessage As String)
        Select Case eAlertType
            Case AlertType.PagerAlert
                WriteEventLog(ConfigurationManager.AppSettings("EHRSS_RerunJobFail_EventSource"), _
                              ConfigurationManager.AppSettings("EHRSS_RerunJobFail_PagerEventID"), _
                              EventLogEntryType.Error, strMessage)

            Case AlertType.EmailAlert
                WriteEventLog(ConfigurationManager.AppSettings("EHRSS_RerunJobFail_EventSource"), _
                              ConfigurationManager.AppSettings("EHRSS_RerunJobFail_EmailEventID"), _
                              EventLogEntryType.Warning, strMessage)

            Case Else
                Throw New NotImplementedException

        End Select

    End Sub

    Private Sub CheckEHRSS_OutSyncCase(ByRef dtmLastCheck As DateTime, ByVal dtmCurrent As DateTime)
        ' +-------------------------------------------------------------------------------+
        ' | For every [5] minutes, monitor the ScheduleJobLog to see if any eHRSS         |
        ' | out sync case. If there is any case, raise alert.                             |
        ' +-------------------------------------------------------------------------------+

        Log("Checking EHRSS_OutSyncCase")

        ' Check whether need to run
        If DateDiff(DateInterval.Minute, dtmLastCheck, dtmCurrent) < CInt(ConfigurationManager.AppSettings("EHRSS_OutSyncCase_CheckInterval")) Then
            Log("Smaller than CheckInterval, no need to run")

            Return
        End If

        ' Update now to be the new LastCheckTime
        dtmLastCheck = dtmCurrent

        ' Check logs
        Dim intScheduleJobLogMinuteBefore As Integer = CInt(ConfigurationManager.AppSettings("EHRSS_OutSyncCase_ScheduleJobLogMinuteBefore"))
        Dim strScheduleJobLogProgramID As String = ConfigurationManager.AppSettings("EHRSS_OutSyncCase_ProgramID")
        Dim strScheduleJobLogID As String = ConfigurationManager.AppSettings("EHRSS_OutSyncCase_FailLogID")
        Dim strPagerAlert As String = ConfigurationManager.AppSettings("EHRSS_OutSyncCase_PagerAlert")
        Dim strEmailAlert As String = ConfigurationManager.AppSettings("EHRSS_OutSyncCase_EmailAlert")

        Dim dt As DataTable = MonitorBLL.GetScheduleJobLog(strScheduleJobLogProgramID, strScheduleJobLogID, _
                                                           dtmCurrent.Add(New TimeSpan(0, -1 * intScheduleJobLogMinuteBefore, 0)), _
                                                           dtmCurrent)
        Dim strMessage As String = String.Empty
        Dim blnPagerAlert As Boolean = False
        Dim blnEmailAlert As Boolean = False

        If dt.Rows.Count > 0 Then

            If strPagerAlert = "Y" Then blnPagerAlert = True
            If strEmailAlert = "Y" Then blnEmailAlert = True

            ' Get the latest description
            strMessage = dt.Rows(0)("Description")
            strMessage = strMessage.Substring(0, strMessage.LastIndexOf("in") - 1)
        End If

        If blnPagerAlert Then
            Log("EHRSS_OutSyncCase pager alert")

            WriteEHRSS_OutSyncCaseAlert(AlertType.PagerAlert, strMessage)
        End If

        If blnEmailAlert Then
            Log("EHRSS_OutSyncCase email alert")

            WriteEHRSS_OutSyncCaseAlert(AlertType.EmailAlert, strMessage)
        End If

        Log("Completed checking EHRSS_OutSyncCase")

    End Sub

    Private Sub WriteEHRSS_OutSyncCaseAlert(ByVal eAlertType As AlertType, ByVal strMessage As String)
        Select Case eAlertType
            Case AlertType.PagerAlert
                WriteEventLog(ConfigurationManager.AppSettings("EHRSS_OutSyncCase_EventSource"), _
                              ConfigurationManager.AppSettings("EHRSS_OutSyncCase_PagerEventID"), _
                              EventLogEntryType.Error, strMessage)

            Case AlertType.EmailAlert
                WriteEventLog(ConfigurationManager.AppSettings("EHRSS_OutSyncCase_EventSource"), _
                              ConfigurationManager.AppSettings("EHRSS_OutSyncCase_EmailEventID"), _
                              EventLogEntryType.Warning, strMessage)

            Case Else
                Throw New NotImplementedException

        End Select

    End Sub
    ' CRE16-019 (To implement token sharing between eHS(S) and eHRSS) [End][Winnie]
#End Region

#Region "CIMS"
    Private Sub CheckCIMS_HealthCheckFail(ByRef dtmLastCheck As DateTime, ByVal dtmCurrent As DateTime)
        ' +-------------------------------------------------------------------------------+
        ' | For every [5] minutes, monitor the database table InterfaceHealthCheckLog.    |
        ' | If the number of fail count reaches [1], raise alert.                         |
        ' +-------------------------------------------------------------------------------+

        Log("Checking CIMS_HealthCheckFail")

        ' Check whether need to run
        If DateDiff(DateInterval.Minute, dtmLastCheck, dtmCurrent) < CInt(ConfigurationManager.AppSettings("CIMS_HealthCheckFail_CheckInterval")) Then
            Log("Smaller than CheckInterval, no need to run")

            Return
        End If

        ' Update now to be the new LastCheckTime
        dtmLastCheck = dtmCurrent

        ' Check logs in database
        Dim strHealthCheckInterfaceCode As String = ConfigurationManager.AppSettings("CIMS_HealthCheckFail_InterfaceCode")
        Dim strHealthCheckFailLogID As String = ConfigurationManager.AppSettings("CIMS_HealthCheckFail_FailLogID")
        Dim strPagerFunctionCode As String = ConfigurationManager.AppSettings("CIMS_HealthCheckFail_PagerFunctionCode")
        Dim strEmailFunctionCode As String = ConfigurationManager.AppSettings("CIMS_HealthCheckFail_EmailFunctionCode")

        Dim lstHealthCheckFunctionCodeList As Collection = Nothing
        Call (New CMSHealthCheckBLL).GetCIMSUsingEndpointURLList(lstHealthCheckFunctionCodeList)

        Dim strHealthCheckFailCount As Integer = CInt(ConfigurationManager.AppSettings("CIMS_HealthCheckFail_FailCount"))
        Dim strMessage As String = String.Format("(EHS->CIMS) Connection Fail count in last {0} Health Check:", strHealthCheckFailCount)
        Dim blnPagerAlert As Boolean = False
        Dim blnEmailAlert As Boolean = False

        For i As Integer = 1 To lstHealthCheckFunctionCodeList.Count
            Dim dt As DataTable = MonitorBLL.GetInterfaceHealthCheckLog(strHealthCheckInterfaceCode, lstHealthCheckFunctionCodeList(i), _
                                                                        Nothing, strHealthCheckFailCount)

            Dim drConnectionFail() As DataRow = dt.Select(String.Format("Log_ID = '{0}'", strHealthCheckFailLogID))

            If drConnectionFail.Length >= strHealthCheckFailCount Then
                Dim strSiteName As String = String.Empty

                If i = 1 Then
                    strSiteName = "Primary Site"
                Else
                    strSiteName = String.Format("Secondary Site {0}", i - 1)
                End If

                strMessage = strMessage & String.Format(" [{0}] is {1}.", strSiteName, drConnectionFail.Length)

                If strPagerFunctionCode = "ALL" OrElse strPagerFunctionCode.ContainsValue(lstHealthCheckFunctionCodeList(i)) Then
                    blnPagerAlert = True
                End If

                If strEmailFunctionCode = "ALL" OrElse strEmailFunctionCode.ContainsValue(lstHealthCheckFunctionCodeList(i)) Then
                    blnEmailAlert = True
                End If

            End If
        Next

        If blnPagerAlert Then
            Log("CIMS_HealthCheckFail pager alert")

            WriteCIMS_HealthCheckFailAlert(AlertType.PagerAlert, strMessage)

        End If

        If blnEmailAlert Then
            Log("CIMS_HealthCheckFail email alert")

            WriteCIMS_HealthCheckFailAlert(AlertType.EmailAlert, strMessage)
        End If

        Log("Completed Checking CIMS_HealthCheckFail")

    End Sub

    Private Sub WriteCIMS_HealthCheckFailAlert(ByVal eAlertType As AlertType, ByVal strMessage As String)
        Select Case eAlertType
            Case AlertType.PagerAlert
                WriteEventLog(ConfigurationManager.AppSettings("CIMS_HealthCheckFail_EventSource"), _
                              ConfigurationManager.AppSettings("CIMS_HealthCheckFail_PagerEventID"), _
                              EventLogEntryType.Error, strMessage)

            Case AlertType.EmailAlert
                WriteEventLog(ConfigurationManager.AppSettings("CIMS_HealthCheckFail_EventSource"), _
                              ConfigurationManager.AppSettings("CIMS_HealthCheckFail_EmailEventID"), _
                              EventLogEntryType.Warning, strMessage)

            Case Else
                Throw New NotImplementedException

        End Select

    End Sub

    '

    Private Sub CheckCIMS_HCSPFail(ByRef dtmLastCheck As DateTime, ByVal dtmCurrent As DateTime)
        ' +-------------------------------------------------------------------------------+
        ' | For every [5] minutes, monitor the HCSP logs. If there are at least [3] fail  |
        ' | logs (no matter consecutive or not, raise error.                              |
        ' +-------------------------------------------------------------------------------+

        Log("Checking CIMS_HCSPFail")

        ' Check whether need to run
        If DateDiff(DateInterval.Minute, dtmLastCheck, dtmCurrent) < CInt(ConfigurationManager.AppSettings("CIMS_HCSPFail_CheckInterval")) Then
            Log("Smaller than CheckInterval, no need to run")

            Return
        End If

        ' Update now to be the new LastCheckTime
        dtmLastCheck = dtmCurrent

        ' Check logs
        Dim intAuditLogMinuteBefore As Integer = CInt(ConfigurationManager.AppSettings("CIMS_HCSPFail_AuditLogMinuteBefore"))
        Dim intAuditLogEnquirySystem As String = ConfigurationManager.AppSettings("CIMS_HCSPFail_EnquirySystem")

        Dim intFailCount As Integer = MonitorBLL.GetHCSPFailAuditLog(intAuditLogEnquirySystem, intAuditLogMinuteBefore)

        If intFailCount >= CInt(ConfigurationManager.AppSettings("CIMS_HCSPFail_FailCount")) Then
            Dim strMessage As String = String.Format("(EHS->CIMS) {0} fail logs found in HCSP vaccination enquiry in the past {1} mins", _
                                                     intFailCount, intAuditLogMinuteBefore)

            Log("CIMS_HCSPFail pager and email alert")

            WriteCIMS_HCSPFailAlert(AlertType.PagerAlert, strMessage)
            WriteCIMS_HCSPFailAlert(AlertType.EmailAlert, strMessage)

        End If

        Log("Completed Checking CIMS_HCSPFail")

    End Sub

    Private Sub WriteCIMS_HCSPFailAlert(ByVal eAlertType As AlertType, ByVal strMessage As String)
        Select Case eAlertType
            Case AlertType.PagerAlert
                WriteEventLog(ConfigurationManager.AppSettings("CIMS_HCSPFail_EventSource"), _
                              ConfigurationManager.AppSettings("CIMS_HCSPFail_PagerEventID"), _
                              EventLogEntryType.Error, strMessage)

            Case AlertType.EmailAlert
                WriteEventLog(ConfigurationManager.AppSettings("CIMS_HCSPFail_EventSource"), _
                              ConfigurationManager.AppSettings("CIMS_HCSPFail_EmailEventID"), _
                              EventLogEntryType.Warning, strMessage)

            Case Else
                Throw New NotImplementedException

        End Select

    End Sub

    '

    Private Sub CheckCIMS_SlowResponse(ByRef dtmLastCheck As DateTime, ByVal dtmCurrent As DateTime)
        ' +-------------------------------------------------------------------------------+
        ' | For every [5] minutes, monitor the CIMS->EHS InterfaceLog.                    |
        ' | For Single Mode, If there are at least [1] requests of:                       |
        ' |   >=[6] seconds, raise alert.                                                 |
        ' |   >=[9] seconds, raise another separate alert.                                |
        ' | For Batch Mode, If there are at least [1] requests of:                        |
        ' |   >=[15] seconds, raise alert.                                                |
        ' |   >=[20] seconds, raise another separate alert.                               |
        ' +-------------------------------------------------------------------------------+

        Log("Checking CIMS_SlowResponse")

        ' Check whether need to run
        If DateDiff(DateInterval.Minute, dtmLastCheck, dtmCurrent) < CInt(ConfigurationManager.AppSettings("CIMS_SlowResponse_CheckInterval")) Then
            Log("Smaller than CheckInterval, no need to run")

            Return
        End If

        ' Update now to be the new LastCheckTime
        dtmLastCheck = dtmCurrent

        ' Check logs
        Dim strAuditLogFunctionCode As String = ConfigurationManager.AppSettings("CIMS_SlowResponse_AuditLogFunctionCode")
        Dim strAuditLogLogID As String = ConfigurationManager.AppSettings("CIMS_SlowResponse_AuditLogLogID")
        Dim intAuditLogMinuteBefore As Integer = CInt(ConfigurationManager.AppSettings("CIMS_SlowResponse_AuditLogMinuteBefore"))
        Dim intAuditLogRequestSystem As String = ConfigurationManager.AppSettings("CIMS_SlowResponse_AuditLogRequestSystem")
        Dim strLevel1PagerAlert As String = ConfigurationManager.AppSettings("CIMS_SlowResponse_Level1_PagerAlert")
        Dim strLevel1EmailAlert As String = ConfigurationManager.AppSettings("CIMS_SlowResponse_Level1_EmailAlert")
        Dim strLevel2PagerAlert As String = ConfigurationManager.AppSettings("CIMS_SlowResponse_Level2_PagerAlert")
        Dim strLevel2EmailAlert As String = ConfigurationManager.AppSettings("CIMS_SlowResponse_Level2_EmailAlert")

        Dim dt As DataTable = MonitorBLL.GetInterfaceLogProcessTimeByMinuteBefore(strAuditLogFunctionCode, strAuditLogLogID, intAuditLogMinuteBefore, intAuditLogRequestSystem)
        Dim intSlowResponseCount As Integer = CInt(ConfigurationManager.AppSettings("CIMS_SlowResponse_FailCount"))

        Dim strSlowResponse_Message As String = "(CIMS->EHS) {0}/{1} Slow Performance Record count (>={2}s) in the past {3} mins [threshold: {4}]"

        ' Single Mode
        Dim dvSingle As DataView = New DataView(dt)
        dvSingle.RowFilter = String.Format("Description like '%<BatchEnquiry: {0}>%'", "N")

        ' Level 1: If any process on CIMS->EHS Interface >=[6] seconds, raise alert
        Dim intLv1Value As Integer = CInt(ConfigurationManager.AppSettings("CIMS_SlowResponse_Level1Value"))

        Dim drLv1 As DataRow() = dvSingle.ToTable.Select(String.Format("Time_Diff >= {0}", intLv1Value * 1000))

        If drLv1.Length >= intSlowResponseCount Then
            Dim strMessage As String = String.Format(strSlowResponse_Message, _
                                                     drLv1.Length, _
                                                     dvSingle.Count, _
                                                     intLv1Value, _
                                                     intAuditLogMinuteBefore, _
                                                     intSlowResponseCount)

            If strLevel1PagerAlert = "Y" Then
                Log("CIMS_SlowResponse Lv1 pager alert")
                WriteCIMS_SlowResponseLv1Alert(AlertType.PagerAlert, strMessage)
            End If

            If strLevel1EmailAlert = "Y" Then
                Log("CIMS_SlowResponse Lv1 email alert")
                WriteCIMS_SlowResponseLv1Alert(AlertType.EmailAlert, strMessage)
            End If

        End If

        ' Level 2: If any process on CIMS->EHS Interface >=[9] seconds, raise alert
        Dim intLv2Value As Integer = CInt(ConfigurationManager.AppSettings("CIMS_SlowResponse_Level2Value"))

        Dim drLv2 As DataRow() = dvSingle.ToTable.Select(String.Format("Time_Diff >= {0}", intLv2Value * 1000))

        If drLv2.Length >= intSlowResponseCount Then
            Dim strMessage As String = String.Format(strSlowResponse_Message, _
                                                     drLv2.Length, _
                                                     dvSingle.Count, _
                                                     intLv2Value, _
                                                     intAuditLogMinuteBefore, _
                                                     intSlowResponseCount)

            If strLevel2PagerAlert = "Y" Then
                Log("CIMS_SlowResponse Lv2 pager alert")
                WriteCIMS_SlowResponseLv2Alert(AlertType.PagerAlert, strMessage)
            End If

            If strLevel1EmailAlert = "Y" Then
                Log("CIMS_SlowResponse Lv2 email alert")
                WriteCIMS_SlowResponseLv2Alert(AlertType.EmailAlert, strMessage)
            End If
        End If

        ' Batch Mode
        Dim dvBatch As DataView = New DataView(dt)
        dvBatch.RowFilter = String.Format("Description like '%<BatchEnquiry: {0}>%'", "Y")

        ' Batch Level 1: If any process on CIMS->EHS Interface >=[BatchLevel1Value] seconds, raise alert
        Dim intBatchLv1Value As Integer = CInt(ConfigurationManager.AppSettings("CIMS_SlowResponse_BatchLevel1Value"))

        Dim drBatchLv1 As DataRow() = dvBatch.ToTable.Select(String.Format("Time_Diff >= {0}", intBatchLv1Value * 1000))

        If drBatchLv1.Length >= intSlowResponseCount Then
            Dim strMessage As String = String.Format(strSlowResponse_Message, _
                                                     drBatchLv1.Length, _
                                                     dvBatch.Count, _
                                                     intBatchLv1Value, _
                                                     intAuditLogMinuteBefore, _
                                                     intSlowResponseCount)

            If strLevel1PagerAlert = "Y" Then
                Log("CIMS_SlowResponse Batch Lv1 pager alert")
                WriteCIMS_SlowResponseBatchLv1Alert(AlertType.PagerAlert, strMessage)
            End If

            If strLevel1EmailAlert = "Y" Then
                Log("CIMS_SlowResponse Batch Lv1 email alert")
                WriteCIMS_SlowResponseBatchLv1Alert(AlertType.EmailAlert, strMessage)
            End If
        End If

        ' Batch Level 2: If any process on CIMS->EHS Interface >=[BatchLevel2Value] seconds, raise alert
        Dim intBatchLv2Value As Integer = CInt(ConfigurationManager.AppSettings("CIMS_SlowResponse_BatchLevel2Value"))

        Dim drBatchLv2 As DataRow() = dvBatch.ToTable.Select(String.Format("Time_Diff >= {0}", intBatchLv2Value * 1000))

        If drBatchLv2.Length >= intSlowResponseCount Then
            Dim strMessage As String = String.Format(strSlowResponse_Message, _
                                                     drBatchLv2.Length, _
                                                     dvBatch.Count, _
                                                     intBatchLv2Value, _
                                                     intAuditLogMinuteBefore, _
                                                     intSlowResponseCount)

            If strLevel2PagerAlert = "Y" Then
                Log("CIMS_SlowResponse Batch Lv2 pager alert")
                WriteCIMS_SlowResponseBatchLv2Alert(AlertType.PagerAlert, strMessage)
            End If

            If strLevel2EmailAlert = "Y" Then
                Log("CIMS_SlowResponse Batch Lv2 email alert")
                WriteCIMS_SlowResponseBatchLv2Alert(AlertType.EmailAlert, strMessage)
            End If
        End If

        Log("Completed Checking CIMS_SlowResponse")

    End Sub

    Private Sub WriteCIMS_SlowResponseLv1Alert(ByVal eAlertType As AlertType, ByVal strMessage As String)
        Select Case eAlertType
            Case AlertType.PagerAlert
                WriteEventLog(ConfigurationManager.AppSettings("CIMS_SlowResponseLv1_EventSource"), _
                              ConfigurationManager.AppSettings("CIMS_SlowResponseLv1_PagerEventID"), _
                              EventLogEntryType.Error, strMessage)

            Case AlertType.EmailAlert
                WriteEventLog(ConfigurationManager.AppSettings("CIMS_SlowResponseLv1_EventSource"), _
                              ConfigurationManager.AppSettings("CIMS_SlowResponseLv1_EmailEventID"), _
                              EventLogEntryType.Warning, strMessage)

            Case Else
                Throw New NotImplementedException

        End Select

    End Sub

    Private Sub WriteCIMS_SlowResponseLv2Alert(ByVal eAlertType As AlertType, ByVal strMessage As String)
        Select Case eAlertType
            Case AlertType.PagerAlert
                WriteEventLog(ConfigurationManager.AppSettings("CIMS_SlowResponseLv2_EventSource"), _
                              ConfigurationManager.AppSettings("CIMS_SlowResponseLv2_PagerEventID"), _
                              EventLogEntryType.Error, strMessage)

            Case AlertType.EmailAlert
                WriteEventLog(ConfigurationManager.AppSettings("CIMS_SlowResponseLv2_EventSource"), _
                              ConfigurationManager.AppSettings("CIMS_SlowResponseLv2_EmailEventID"), _
                              EventLogEntryType.Warning, strMessage)

            Case Else
                Throw New NotImplementedException

        End Select

    End Sub

    Private Sub WriteCIMS_SlowResponseBatchLv1Alert(ByVal eAlertType As AlertType, ByVal strMessage As String)
        Select Case eAlertType
            Case AlertType.PagerAlert
                WriteEventLog(ConfigurationManager.AppSettings("CIMS_SlowResponseBatchLv1_EventSource"), _
                              ConfigurationManager.AppSettings("CIMS_SlowResponseBatchLv1_PagerEventID"), _
                              EventLogEntryType.Error, strMessage)

            Case AlertType.EmailAlert
                WriteEventLog(ConfigurationManager.AppSettings("CIMS_SlowResponseBatchLv1_EventSource"), _
                              ConfigurationManager.AppSettings("CIMS_SlowResponseBatchLv1_EmailEventID"), _
                              EventLogEntryType.Warning, strMessage)

            Case Else
                Throw New NotImplementedException

        End Select

    End Sub

    Private Sub WriteCIMS_SlowResponseBatchLv2Alert(ByVal eAlertType As AlertType, ByVal strMessage As String)
        Select Case eAlertType
            Case AlertType.PagerAlert
                WriteEventLog(ConfigurationManager.AppSettings("CIMS_SlowResponseBatchLv2_EventSource"), _
                              ConfigurationManager.AppSettings("CIMS_SlowResponseBatchLv2_PagerEventID"), _
                              EventLogEntryType.Error, strMessage)

            Case AlertType.EmailAlert
                WriteEventLog(ConfigurationManager.AppSettings("CIMS_SlowResponseBatchLv2_EventSource"), _
                              ConfigurationManager.AppSettings("CIMS_SlowResponseBatchLv2_EmailEventID"), _
                              EventLogEntryType.Warning, strMessage)

            Case Else
                Throw New NotImplementedException

        End Select

    End Sub
#End Region

#Region "OCSSS"
    ' CRE17-010 (OCSSS integration) [Start][Winnie SUEN]
    ' ----------------------------------------------------------
    Private Sub CheckOCSSS_HealthCheckFail(ByRef dtmLastCheck As DateTime, ByVal dtmCurrent As DateTime)
        ' +-------------------------------------------------------------------------------+
        ' | For every [5] minutes, monitor the database table InterfaceHealthCheckLog.    |
        ' | If all of the last [1] health check are failed, raise alert.                  |
        ' +-------------------------------------------------------------------------------+

        Log("Checking OCSSS_HealthCheckFail")

        ' Check whether need to run
        If DateDiff(DateInterval.Minute, dtmLastCheck, dtmCurrent) < CInt(ConfigurationManager.AppSettings("OCSSS_HealthCheckFail_CheckInterval")) Then
            Log("Smaller than CheckInterval, no need to run")

            Return
        End If

        ' Update now to be the new LastCheckTime
        dtmLastCheck = dtmCurrent

        ' Check logs in database
        Dim strOCSSS_HealthCheckInterfaceCode As String = ConfigurationManager.AppSettings("OCSSS_HealthCheckFail_InterfaceCode")
        Dim strOCSSS_HealthCheckFunctionCode As String = ConfigurationManager.AppSettings("OCSSS_HealthCheckFail_FunctionCode")
        Dim strOCSSS_HealthCheckFailLogID As String = ConfigurationManager.AppSettings("OCSSS_HealthCheckFail_FailLogID")

        Dim strPagerAlert As String = ConfigurationManager.AppSettings("OCSSS_HealthCheckFail_PagerAlert")
        Dim strPagerAlert_FromTime As String = ConfigurationManager.AppSettings("OCSSS_HealthCheckFail_PagerAlert_FromTime")
        Dim strPagerAlert_ToTime As String = ConfigurationManager.AppSettings("OCSSS_HealthCheckFail_PagerAlert_ToTime")
        Dim strEmailAlert As String = ConfigurationManager.AppSettings("OCSSS_HealthCheckFail_EmailAlert")


        Dim intOCSSS_HealthCheckFailCount As Integer = CInt(ConfigurationManager.AppSettings("OCSSS_HealthCheckFail_FailCount"))
        Dim strMessage As String = String.Format("(EHS->OCSSS) Connection Fail count in last {0} Health Check.", intOCSSS_HealthCheckFailCount)

        ' Check [InterfaceHealthCheckLog]
        Dim dt As DataTable = MonitorBLL.GetInterfaceHealthCheckLog(strOCSSS_HealthCheckInterfaceCode, strOCSSS_HealthCheckFunctionCode, _
                                                                    Nothing, intOCSSS_HealthCheckFailCount)

        Dim drConnectionFail() As DataRow = dt.Select(String.Format("Log_ID = '{0}'", strOCSSS_HealthCheckFailLogID))

        If drConnectionFail.Length >= intOCSSS_HealthCheckFailCount Then

            ' Pager Alert
            If IsRaiseAlert(strPagerAlert, strPagerAlert_FromTime, strPagerAlert_ToTime) Then
                Log("OCSSS_HealthCheckFail pager alert")
                WriteOCSSS_HealthCheckFailAlert(AlertType.PagerAlert, strMessage)
            End If

            ' Email Alert
            If IsRaiseAlert(strEmailAlert) Then
                Log("OCSSS_HealthCheckFail email alert")
                WriteOCSSS_HealthCheckFailAlert(AlertType.EmailAlert, strMessage)
            End If

        End If

        Log("Completed checking OCSSS_HealthCheckFail")

    End Sub

    Private Sub WriteOCSSS_HealthCheckFailAlert(ByVal eAlertType As AlertType, ByVal strMessage As String)
        Select Case eAlertType
            Case AlertType.PagerAlert
                WriteEventLog(ConfigurationManager.AppSettings("OCSSS_HealthCheckFail_EventSource"), _
                              ConfigurationManager.AppSettings("OCSSS_HealthCheckFail_PagerEventID"), _
                              EventLogEntryType.Error, strMessage)

            Case AlertType.EmailAlert
                WriteEventLog(ConfigurationManager.AppSettings("OCSSS_HealthCheckFail_EventSource"), _
                              ConfigurationManager.AppSettings("OCSSS_HealthCheckFail_EmailEventID"), _
                              EventLogEntryType.Warning, strMessage)

            Case Else
                Throw New NotImplementedException

        End Select
    End Sub


    Private Sub CheckOCSSS_SlowResponse(ByRef dtmLastCheck As DateTime, ByVal dtmCurrent As DateTime)
        ' +-------------------------------------------------------------------------------+
        ' | For every [5] minutes, monitor the EHS->OCSSS InterfaceLog.                   |
        ' | If there are at least [1] request of >= [4] seconds, raise alert.             |
        ' +-------------------------------------------------------------------------------+

        Log("Checking OCSSS_SlowResponse")

        ' Check whether need to run
        If DateDiff(DateInterval.Minute, dtmLastCheck, dtmCurrent) < CInt(ConfigurationManager.AppSettings("OCSSS_SlowResponse_CheckInterval")) Then
            Log("Smaller than CheckInterval, no need to run")

            Return
        End If

        ' Update now to be the new LastCheckTime
        dtmLastCheck = dtmCurrent

        ' Check logs
        Dim strAuditLogFunctionCode As String = ConfigurationManager.AppSettings("OCSSS_SlowResponse_AuditLogFunctionCode")
        Dim strAuditLogLogID As String = ConfigurationManager.AppSettings("OCSSS_SlowResponse_AuditLogLogID")
        Dim intAuditLogMinuteBefore As Integer = CInt(ConfigurationManager.AppSettings("OCSSS_SlowResponse_AuditLogMinuteBefore"))

        Dim dt As DataTable = MonitorBLL.GetInterfaceLogProcessTimeByMinuteBefore(strAuditLogFunctionCode, strAuditLogLogID, intAuditLogMinuteBefore, Nothing)
        Dim intOCSSS_SlowResponseCount As Integer = CInt(ConfigurationManager.AppSettings("OCSSS_SlowResponse_FailCount"))

        ' If any process on EHS->OCSSS Interface >= [4] seconds, raise alert
        Dim intValue As Integer = CInt(ConfigurationManager.AppSettings("OCSSS_SlowResponse_Value"))

        Dim drSlow As DataRow() = dt.Select(String.Format("Time_Diff >= {0}", intValue * 1000))

        If drSlow.Length >= intOCSSS_SlowResponseCount Then
            Dim strMessage As String = String.Format("(EHS->OCSSS) {0}/{1} Slow Performance Record count (>={2}s) in the past {3} mins [threshold: {4}]", _
                                                     drSlow.Length, _
                                                     dt.Rows.Count, _
                                                     intValue, _
                                                     intAuditLogMinuteBefore, _
                                                     intOCSSS_SlowResponseCount)

            Dim strPagerAlert As String = ConfigurationManager.AppSettings("OCSSS_SlowResponse_PagerAlert")
            Dim strPagerAlert_FromTime As String = ConfigurationManager.AppSettings("OCSSS_SlowResponse_PagerAlert_FromTime")
            Dim strPagerAlert_ToTime As String = ConfigurationManager.AppSettings("OCSSS_SlowResponse_PagerAlert_ToTime")
            Dim strEmailAlert As String = ConfigurationManager.AppSettings("OCSSS_SlowResponse_EmailAlert")

            ' Pager Alert
            If IsRaiseAlert(strPagerAlert, strPagerAlert_FromTime, strPagerAlert_ToTime) Then
                Log("OCSSS_SlowResponse pager alert")
                WriteOCSSS_SlowResponseAlert(AlertType.PagerAlert, strMessage)
            End If

            ' Email Alert
            If IsRaiseAlert(strEmailAlert) Then
                Log("OCSSS_SlowResponse email alert")
                WriteOCSSS_SlowResponseAlert(AlertType.EmailAlert, strMessage)
            End If

        End If

        Log("Completed checking OCSSS_SlowResponse")

    End Sub

    Private Sub WriteOCSSS_SlowResponseAlert(ByVal eAlertType As AlertType, ByVal strMessage As String)
        Select Case eAlertType
            Case AlertType.PagerAlert
                WriteEventLog(ConfigurationManager.AppSettings("OCSSS_SlowResponse_EventSource"), _
                              ConfigurationManager.AppSettings("OCSSS_SlowResponse_PagerEventID"), _
                              EventLogEntryType.Error, strMessage)

            Case AlertType.EmailAlert
                WriteEventLog(ConfigurationManager.AppSettings("OCSSS_SlowResponse_EventSource"), _
                              ConfigurationManager.AppSettings("OCSSS_SlowResponse_EmailEventID"), _
                              EventLogEntryType.Warning, strMessage)

            Case Else
                Throw New NotImplementedException

        End Select

    End Sub

    Private Sub CheckOCSSS_ConnectFail(ByRef dtmLastCheck As DateTime, ByVal dtmCurrent As DateTime)
        ' +-------------------------------------------------------------------------------+
        ' | For every [5] minutes, monitor the EHS->OCSSS InterfaceLog.                   |
        ' | If there are any connection fail case, raise alert.                           |
        ' +-------------------------------------------------------------------------------+

        Log("Checking OCSSS_ConnectFail")

        ' Check whether need to run
        If DateDiff(DateInterval.Minute, dtmLastCheck, dtmCurrent) < CInt(ConfigurationManager.AppSettings("OCSSS_ConnectFail_CheckInterval")) Then
            Log("Smaller than CheckInterval, no need to run")

            Return
        End If

        ' Update now to be the new LastCheckTime
        dtmLastCheck = dtmCurrent

        ' Check logs
        Dim strAuditLogFunctionCode As String = ConfigurationManager.AppSettings("OCSSS_ConnectFail_AuditLogFunctionCode")
        Dim strAuditLogLogID As String = ConfigurationManager.AppSettings("OCSSS_ConnectFail_AuditLogLogID")
        Dim intAuditLogMinuteBefore As Integer = CInt(ConfigurationManager.AppSettings("OCSSS_ConnectFail_AuditLogMinuteBefore"))

        Dim dt As DataTable = MonitorBLL.GetInterfaceLogProcessTimeByMinuteBefore(strAuditLogFunctionCode, strAuditLogLogID, intAuditLogMinuteBefore, Nothing)

        If dt.Rows.Count > 0 Then
            Dim strMessage As String = String.Format("(EHS->OCSSS) {0} Connection Fail count in the past {1} mins", _
                                                     dt.Rows.Count, _
                                                     intAuditLogMinuteBefore)

            Dim strPagerAlert As String = ConfigurationManager.AppSettings("OCSSS_ConnectFail_PagerAlert")
            Dim strPagerAlert_FromTime As String = ConfigurationManager.AppSettings("OCSSS_ConnectFail_PagerAlert_FromTime")
            Dim strPagerAlert_ToTime As String = ConfigurationManager.AppSettings("OCSSS_ConnectFail_PagerAlert_ToTime")
            Dim strEmailAlert As String = ConfigurationManager.AppSettings("OCSSS_ConnectFail_EmailAlert")

            ' Pager Alert
            If IsRaiseAlert(strPagerAlert, strPagerAlert_FromTime, strPagerAlert_ToTime) Then
                Log("OCSSS_ConnectFail pager alert")
                WriteOCSSS_ConnectFailAlert(AlertType.PagerAlert, strMessage)
            End If

            ' Email Alert
            If IsRaiseAlert(strEmailAlert) Then
                Log("OCSSS_ConnectFail email alert")
                WriteOCSSS_ConnectFailAlert(AlertType.EmailAlert, strMessage)
            End If

        End If

        Log("Completed checking OCSSS_ConnectFail")

    End Sub

    Private Sub WriteOCSSS_ConnectFailAlert(ByVal eAlertType As AlertType, ByVal strMessage As String)
        Select Case eAlertType
            Case AlertType.PagerAlert
                WriteEventLog(ConfigurationManager.AppSettings("OCSSS_ConnectFail_EventSource"), _
                              ConfigurationManager.AppSettings("OCSSS_ConnectFail_PagerEventID"), _
                              EventLogEntryType.Error, strMessage)

            Case AlertType.EmailAlert
                WriteEventLog(ConfigurationManager.AppSettings("OCSSS_ConnectFail_EventSource"), _
                              ConfigurationManager.AppSettings("OCSSS_ConnectFail_EmailEventID"), _
                              EventLogEntryType.Warning, strMessage)

            Case Else
                Throw New NotImplementedException

        End Select
    End Sub
    ' CRE17-010 (OCSSS integration) [End][Winnie SUEN]
#End Region

#Region "eHRSS - Patient Portal"

    Private Sub CheckEHRSS_PP_SlowResponse(ByRef dtmLastCheck As DateTime, ByVal dtmCurrent As DateTime)
        ' +-------------------------------------------------------------------------------+
        ' | For every [5] minutes, monitor the EHRSS->EHS InterfaceLog.                   |
        ' | For "geteHSSVoucherBalance", If there are at least [1] requests of:           |
        ' |   >=[6] seconds, raise alert.                                                 |
        ' |   >=[9] seconds, raise another separate alert.                                |
        ' | For "geteHSSDoctorList", If there are at least [1] requests of:               |
        ' |   >=[15] seconds, raise alert.                                                |
        ' |   >=[20] seconds, raise another separate alert.                               |
        ' +-------------------------------------------------------------------------------+

        Log("Checking EHRSS_PatientPortal_SlowResponse")

        ' Check whether need to run
        If DateDiff(DateInterval.Minute, dtmLastCheck, dtmCurrent) < CInt(ConfigurationManager.AppSettings("EHRSS_PP_SlowResponse_CheckInterval")) Then
            Log("Smaller than CheckInterval, no need to run")

            Return
        End If

        ' Update now to be the new LastCheckTime
        dtmLastCheck = dtmCurrent

        ' Check logs
        Dim strAuditLogFunctionCode As String = ConfigurationManager.AppSettings("EHRSS_PP_SlowResponse_AuditLogFunctionCode")
        Dim strDoctorListLogID As String = ConfigurationManager.AppSettings("EHRSS_PP_DoctorList_SlowResponse_AuditLogLogID")
        Dim strVoucherBalanceLogID As String = ConfigurationManager.AppSettings("EHRSS_PP_VoucherBalance_SlowResponse_AuditLogLogID")
        Dim intAuditLogMinuteBefore As Integer = CInt(ConfigurationManager.AppSettings("EHRSS_PP_SlowResponse_AuditLogMinuteBefore"))

        Dim strLevel1PagerAlert As String = ConfigurationManager.AppSettings("EHRSS_PP_SlowResponse_Level1_PagerAlert")
        Dim strLevel1EmailAlert As String = ConfigurationManager.AppSettings("EHRSS_PP_SlowResponse_Level1_EmailAlert")
        Dim strLevel2PagerAlert As String = ConfigurationManager.AppSettings("EHRSS_PP_SlowResponse_Level2_PagerAlert")
        Dim strLevel2EmailAlert As String = ConfigurationManager.AppSettings("EHRSS_PP_SlowResponse_Level2_EmailAlert")

        Dim dtDoctorList As DataTable = MonitorBLL.GetInterfaceLogProcessTimeByMinuteBefore(strAuditLogFunctionCode, strDoctorListLogID, intAuditLogMinuteBefore, Nothing)
        Dim dtVoucherBalance As DataTable = MonitorBLL.GetInterfaceLogProcessTimeByMinuteBefore(strAuditLogFunctionCode, strVoucherBalanceLogID, intAuditLogMinuteBefore, Nothing)

        Dim intSlowResponseCount As Integer = CInt(ConfigurationManager.AppSettings("EHRSS_PP_SlowResponse_FailCount"))

        Dim strSlowResponse_Message As String = "(EHRSS->EHS) {0}/{1} {2} Slow Performance Record count (>={3}s) in the past {4} mins [threshold: {5}]"

        '---------------
        ' Doctor List
        '---------------
        Dim dvDoctorList As DataView = New DataView(dtDoctorList)

        ' Level 1: If any process on EHRSS->EHS Interface >=[15] seconds, raise alert
        Dim intLv1DoctorListValue As Integer = CInt(ConfigurationManager.AppSettings("EHRSS_PP_DoctorList_SlowResponse_Level1Value"))

        Dim drLv1DoctorList As DataRow() = dvDoctorList.ToTable.Select(String.Format("Time_Diff >= {0}", intLv1DoctorListValue * 1000))

        If drLv1DoctorList.Length >= intSlowResponseCount Then
            Dim strMessage As String = String.Format(strSlowResponse_Message, _
                                                     drLv1DoctorList.Length, _
                                                     dvDoctorList.Count, _
                                                     "Doctor List", _
                                                     intLv1DoctorListValue, _
                                                     intAuditLogMinuteBefore, _
                                                     intSlowResponseCount)

            If strLevel1PagerAlert = "Y" Then
                Log("EHRSS_PP_DoctorList_SlowResponse Lv1 pager alert")
                WriteEHRSS_PP_SlowResponseLv1Alert(AlertType.PagerAlert, strMessage)
            End If

            If strLevel1EmailAlert = "Y" Then
                Log("EHRSS_PP_DoctorList_SlowResponse Lv1 email alert")
                WriteEHRSS_PP_SlowResponseLv1Alert(AlertType.EmailAlert, strMessage)
            End If

        End If

        ' Level 2: If any process on CIMS->EHS Interface >=[20] seconds, raise alert
        Dim intLv2DoctorListValue As Integer = CInt(ConfigurationManager.AppSettings("EHRSS_PP_DoctorList_SlowResponse_Level2Value"))

        Dim drLv2DoctorList As DataRow() = dvDoctorList.ToTable.Select(String.Format("Time_Diff >= {0}", intLv2DoctorListValue * 1000))

        If drLv2DoctorList.Length >= intSlowResponseCount Then
            Dim strMessage As String = String.Format(strSlowResponse_Message, _
                                                     drLv2DoctorList.Length, _
                                                     dvDoctorList.Count, _
                                                     "Doctor List", _
                                                     intLv2DoctorListValue, _
                                                     intAuditLogMinuteBefore, _
                                                     intSlowResponseCount)

            If strLevel2PagerAlert = "Y" Then
                Log("EHRSS_PP_DoctorList_SlowResponse Lv2 pager alert")
                WriteEHRSS_PP_SlowResponseLv2Alert(AlertType.PagerAlert, strMessage)
            End If

            If strLevel1EmailAlert = "Y" Then
                Log("EHRSS_PP_DoctorList_SlowResponse Lv2 email alert")
                WriteEHRSS_PP_SlowResponseLv2Alert(AlertType.EmailAlert, strMessage)
            End If
        End If

        '------------------
        ' Voucher Balance
        '------------------
        Dim dvVoucherBalance As DataView = New DataView(dtVoucherBalance)

        ' Level 1: If any process on EHRSS->EHS Interface >=[6] seconds, raise alert
        Dim intLv1VoucherBalanceValue As Integer = CInt(ConfigurationManager.AppSettings("EHRSS_PP_VoucherBalance_SlowResponse_Level1Value"))

        Dim drLv1VoucherBalance As DataRow() = dvVoucherBalance.ToTable.Select(String.Format("Time_Diff >= {0}", intLv1VoucherBalanceValue * 1000))

        If drLv1VoucherBalance.Length >= intSlowResponseCount Then
            Dim strMessage As String = String.Format(strSlowResponse_Message, _
                                                     drLv1VoucherBalance.Length, _
                                                     dvVoucherBalance.Count, _
                                                     "Voucher Balance", _
                                                     intLv1VoucherBalanceValue, _
                                                     intAuditLogMinuteBefore, _
                                                     intSlowResponseCount)

            If strLevel1PagerAlert = "Y" Then
                Log("EHRSS_PP_VoucherBalance_SlowResponse Lv1 pager alert")
                WriteEHRSS_PP_SlowResponseLv1Alert(AlertType.PagerAlert, strMessage)
            End If

            If strLevel1EmailAlert = "Y" Then
                Log("EHRSS_PP_VoucherBalance_SlowResponse Lv1 email alert")
                WriteEHRSS_PP_SlowResponseLv1Alert(AlertType.EmailAlert, strMessage)
            End If
        End If

        ' Level 2: If any process on EHRSS->EHS Interface >=[9] seconds, raise alert
        Dim intLv2VoucherBalanceValue As Integer = CInt(ConfigurationManager.AppSettings("EHRSS_PP_VoucherBalance_SlowResponse_Level2Value"))

        Dim drLv2VoucherBalance As DataRow() = dvVoucherBalance.ToTable.Select(String.Format("Time_Diff >= {0}", intLv2VoucherBalanceValue * 1000))

        If drLv2VoucherBalance.Length >= intSlowResponseCount Then
            Dim strMessage As String = String.Format(strSlowResponse_Message, _
                                                     drLv2VoucherBalance.Length, _
                                                     dvVoucherBalance.Count, _
                                                     "Voucher Balance", _
                                                     intLv2VoucherBalanceValue, _
                                                     intAuditLogMinuteBefore, _
                                                     intSlowResponseCount)

            If strLevel2PagerAlert = "Y" Then
                Log("EHRSS_PP_VoucherBalance_SlowResponse Lv2 pager alert")
                WriteEHRSS_PP_SlowResponseLv2Alert(AlertType.PagerAlert, strMessage)
            End If

            If strLevel2EmailAlert = "Y" Then
                Log("EHRSS_PP_VoucherBalance_SlowResponse Lv2 email alert")
                WriteEHRSS_PP_SlowResponseLv2Alert(AlertType.EmailAlert, strMessage)
            End If
        End If

        Log("Completed Checking EHRSS_PP_SlowResponse")

    End Sub

    Private Sub WriteEHRSS_PP_SlowResponseLv1Alert(ByVal eAlertType As AlertType, ByVal strMessage As String)
        Select Case eAlertType
            Case AlertType.PagerAlert
                WriteEventLog(ConfigurationManager.AppSettings("EHRSS_PP_SlowResponseLv1_EventSource"), _
                              ConfigurationManager.AppSettings("EHRSS_PP_SlowResponseLv1_PagerEventID"), _
                              EventLogEntryType.Error, strMessage)

            Case AlertType.EmailAlert
                WriteEventLog(ConfigurationManager.AppSettings("EHRSS_PP_SlowResponseLv1_EventSource"), _
                              ConfigurationManager.AppSettings("EHRSS_PP_SlowResponseLv1_EmailEventID"), _
                              EventLogEntryType.Warning, strMessage)

            Case Else
                Throw New NotImplementedException

        End Select

    End Sub

    Private Sub WriteEHRSS_PP_SlowResponseLv2Alert(ByVal eAlertType As AlertType, ByVal strMessage As String)
        Select Case eAlertType
            Case AlertType.PagerAlert
                WriteEventLog(ConfigurationManager.AppSettings("EHRSS_PP_SlowResponseLv2_EventSource"), _
                              ConfigurationManager.AppSettings("EHRSS_PP_SlowResponseLv2_PagerEventID"), _
                              EventLogEntryType.Error, strMessage)

            Case AlertType.EmailAlert
                WriteEventLog(ConfigurationManager.AppSettings("EHRSS_PP_SlowResponseLv2_EventSource"), _
                              ConfigurationManager.AppSettings("EHRSS_PP_SlowResponseLv2_EmailEventID"), _
                              EventLogEntryType.Warning, strMessage)

            Case Else
                Throw New NotImplementedException

        End Select

    End Sub

#End Region


#Region "HASPImporter"
    Private Sub CheckHASPImporter_ImportFail(ByRef dtmLastCheck As DateTime, ByVal dtmCurrent As DateTime)
        ' +--------------------------------------------------------------------------------------------------+
        ' | For every [5] minutes, monitor the ScheduleJobLog to see if any HAServicePatientImporter         |
        ' | fail case. If there is error case, raise pager alert;If there is warning case, raise email alert |
        ' +--------------------------------------------------------------------------------------------------+

        Log("Checking HASPImporter_ImportFail")

        ' Check whether need to run
        If DateDiff(DateInterval.Minute, dtmLastCheck, dtmCurrent) < CInt(ConfigurationManager.AppSettings("HASPImporter_CheckInterval")) Then
            Log("Smaller than CheckInterval, no need to run")

            Return
        End If

        ' Update now to be the new LastCheckTime
        dtmLastCheck = dtmCurrent

        ' Check logs
        Dim intScheduleJobLogMinuteBefore As Integer = CInt(ConfigurationManager.AppSettings("HASPImporter_ScheduleJobLogMinuteBefore"))
        Dim strScheduleJobLogProgramID As String = ConfigurationManager.AppSettings("HASPImporter_ProgramID")
        Dim strScheduleJobPagerAlertLogID As String = ConfigurationManager.AppSettings("HASPImporter_PagerAlertLogID")
        Dim strScheduleJobEmailAlertLogID As String = ConfigurationManager.AppSettings("HASPImporter_EmailAlertLogID")


        Dim strPagerAlertLogID_PagerAlert As String = ConfigurationManager.AppSettings("HASPImporter_PagerAlertLogID_PagerAlert")

        Dim strEmailAlertLogID_EmailAlert As String = ConfigurationManager.AppSettings("HASPImporter_EmailAlertLogID_EmailAlert")


        Dim dtPagerAlertLogID As DataTable = MonitorBLL.GetScheduleJobLog(strScheduleJobLogProgramID, strScheduleJobPagerAlertLogID, _
                                                         dtmCurrent.Add(New TimeSpan(0, -1 * intScheduleJobLogMinuteBefore, 0)), _
                                                         dtmCurrent)

        Dim dtEmailAlertLogID As DataTable = MonitorBLL.GetScheduleJobLog(strScheduleJobLogProgramID, strScheduleJobEmailAlertLogID, _
                                                          dtmCurrent.Add(New TimeSpan(0, -1 * intScheduleJobLogMinuteBefore, 0)), _
                                                          dtmCurrent)

        Dim strMessage As String = String.Empty
        Dim blnPagerAlert As Boolean = False
        Dim blnEmailAlert As Boolean = False

        If dtPagerAlertLogID.Rows.Count > 0 Then

            If strPagerAlertLogID_PagerAlert = "Y" Then blnPagerAlert = True

            ' Get the latest description
            strMessage = dtPagerAlertLogID.Rows(0)("Description")

        ElseIf dtEmailAlertLogID.Rows.Count > 0 Then

            If strEmailAlertLogID_EmailAlert = "Y" Then blnEmailAlert = True

            ' Get the latest description
            strMessage = dtEmailAlertLogID.Rows(0)("Description")
        End If



        If blnPagerAlert Then
            Log("CheckHASPImporter_ImportFail pager alert")

            CheckHASPImporter_ImportFailAlert(AlertType.PagerAlert, strMessage)
        End If

        If blnEmailAlert Then
            Log("CheckHASPImporter_ImportFail email alert")

            CheckHASPImporter_ImportFailAlert(AlertType.EmailAlert, strMessage)
        End If

        Log("Completed checking CheckHASPImporter_ImportFail")

    End Sub


    'For CheckHASPImporter_ImportFail write event log
    Private Sub CheckHASPImporter_ImportFailAlert(ByVal eAlertType As AlertType, ByVal strMessage As String)
        Select Case eAlertType
            Case AlertType.PagerAlert
                WriteEventLog(ConfigurationManager.AppSettings("HASPImporter_EventSource"), _
                              ConfigurationManager.AppSettings("HASPImporter_PagerEventID"), _
                              EventLogEntryType.Error, strMessage)

            Case AlertType.EmailAlert
                WriteEventLog(ConfigurationManager.AppSettings("HASPImporter_EventSource"), _
                              ConfigurationManager.AppSettings("HASPImporter_EmailEventID"), _
                              EventLogEntryType.Warning, strMessage)

            Case Else
                Throw New NotImplementedException

        End Select

    End Sub

#End Region


#Region "COVID19Exporter"
    Private Sub CheckCOVID19Exporter_ExportFail(ByRef dtmLastCheck As DateTime, ByVal dtmCurrent As DateTime)
        ' +--------------------------------------------------------------------------------------------------+
        ' | For every [5] minutes, monitor the ScheduleJobLog to see if any COVID19Exporter                  |
        ' | If there are any connection error case, raise alert.                                              |
        ' +--------------------------------------------------------------------------------------------------+

        Log("Checking COVID19Exporter_ExportFail")

        ' Check whether need to run
        If DateDiff(DateInterval.Minute, dtmLastCheck, dtmCurrent) < CInt(ConfigurationManager.AppSettings("COVID19Exporter_CheckInterval")) Then
            Log("Smaller than CheckInterval, no need to run")

            Return
        End If

        ' Update now to be the new LastCheckTime
        dtmLastCheck = dtmCurrent

        ' Check logs
        Dim intScheduleJobLogMinuteBefore As Integer = CInt(ConfigurationManager.AppSettings("COVID19Exporter_ScheduleJobLogMinuteBefore"))
        Dim strScheduleJobLogProgramID As String = ConfigurationManager.AppSettings("COVID19Exporter_ProgramID")
        Dim strScheduleJobAlertLogID As String = ConfigurationManager.AppSettings("COVID19Exporter_AlertLogID")

        Dim strPagerAlertLogID_PagerAlert As String = ConfigurationManager.AppSettings("COVID19Exporter_PagerAlertLogID_PagerAlert")
        Dim strEmailAlertLogID_EmailAlert As String = ConfigurationManager.AppSettings("COVID19Exporter_EmailAlertLogID_EmailAlert")


        Dim dtAlertLogID As DataTable = MonitorBLL.GetScheduleJobLog(strScheduleJobLogProgramID, strScheduleJobAlertLogID, _
                                                         dtmCurrent.Add(New TimeSpan(0, -1 * intScheduleJobLogMinuteBefore, 0)), _
                                                         dtmCurrent)

        Dim strMessage As String = String.Empty
        Dim blnPagerAlert As Boolean = False
        Dim blnEmailAlert As Boolean = False

        If dtAlertLogID.Rows.Count > 0 Then

            If strPagerAlertLogID_PagerAlert = "Y" Then blnPagerAlert = True

            If strEmailAlertLogID_EmailAlert = "Y" Then blnEmailAlert = True

            ' Get the latest description
            strMessage = dtAlertLogID.Rows(0)("Description")
        End If



        If blnPagerAlert Then
            Log("CheckCOVID19Exporter_ExportFail pager alert")

            CheckCOVID19Exporter_ExportFailAlert(AlertType.PagerAlert, strMessage)
        End If

        If blnEmailAlert Then
            Log("CheckCOVID19Exporter_ExportFail email alert")

            CheckCOVID19Exporter_ExportFailAlert(AlertType.EmailAlert, strMessage)
        End If

        Log("Completed checking CheckCOVID19Exporter_ExportFail")

    End Sub


    'For CheckCOVID19Exporter_ImportFail write event log
    Private Sub CheckCOVID19Exporter_ExportFailAlert(ByVal eAlertType As AlertType, ByVal strMessage As String)
        Select Case eAlertType
            Case AlertType.PagerAlert
                WriteEventLog(ConfigurationManager.AppSettings("COVID19Exporter_EventSource"), _
                              ConfigurationManager.AppSettings("COVID19Exporter_PagerEventID"), _
                              EventLogEntryType.Error, strMessage)

            Case AlertType.EmailAlert
                WriteEventLog(ConfigurationManager.AppSettings("COVID19Exporter_EventSource"), _
                              ConfigurationManager.AppSettings("COVID19Exporter_EmailEventID"), _
                              EventLogEntryType.Warning, strMessage)

            Case Else
                Throw New NotImplementedException

        End Select

    End Sub

#End Region

#Region "COVID19BatchConfirm"
    Private Sub CheckCOVID19BatchConfirm_Fail(ByRef dtmLastCheck As DateTime, ByVal dtmCurrent As DateTime)
        ' +--------------------------------------------------------------------------------------------------+
        ' | For every [5] minutes, monitor the ScheduleJobLog to see if any COVID19BatchConfirm         |
        ' | fail case. If there is error case, raise pager alert;If there is warning case, raise email alert |
        ' +--------------------------------------------------------------------------------------------------+

        Log("Checking COVID19BatchConfirm_Fail")

        ' Check whether need to run
        If DateDiff(DateInterval.Minute, dtmLastCheck, dtmCurrent) < CInt(ConfigurationManager.AppSettings("COVID19BatchConfirm_CheckInterval")) Then
            Log("Smaller than CheckInterval, no need to run")

            Return
        End If

        ' Update now to be the new LastCheckTime
        dtmLastCheck = dtmCurrent

        ' Check logs
        Dim intScheduleJobLogMinuteBefore As Integer = CInt(ConfigurationManager.AppSettings("COVID19BatchConfirm_ScheduleJobLogMinuteBefore"))
        Dim strScheduleJobLogProgramID As String = ConfigurationManager.AppSettings("COVID19BatchConfirm_ProgramID")
        Dim strScheduleJobPagerAlertLogID As String = ConfigurationManager.AppSettings("COVID19BatchConfirm_PagerAlertLogID")
        Dim strScheduleJobEmailAlertLogID As String = ConfigurationManager.AppSettings("COVID19BatchConfirm_EmailAlertLogID")


        Dim strPagerAlertLogID_PagerAlert As String = ConfigurationManager.AppSettings("COVID19BatchConfirm_PagerAlertLogID_PagerAlert")

        Dim strEmailAlertLogID_EmailAlert As String = ConfigurationManager.AppSettings("COVID19BatchConfirm_EmailAlertLogID_EmailAlert")


        Dim dtPagerAlertLogID As DataTable = MonitorBLL.GetScheduleJobLog(strScheduleJobLogProgramID, strScheduleJobPagerAlertLogID, _
                                                         dtmCurrent.Add(New TimeSpan(0, -1 * intScheduleJobLogMinuteBefore, 0)), _
                                                         dtmCurrent)

        Dim dtEmailAlertLogID As DataTable = MonitorBLL.GetScheduleJobLog(strScheduleJobLogProgramID, strScheduleJobEmailAlertLogID, _
                                                          dtmCurrent.Add(New TimeSpan(0, -1 * intScheduleJobLogMinuteBefore, 0)), _
                                                          dtmCurrent)

        Dim strMessage As String = String.Empty
        Dim blnPagerAlert As Boolean = False
        Dim blnEmailAlert As Boolean = False

        If dtPagerAlertLogID.Rows.Count > 0 Then

            If strPagerAlertLogID_PagerAlert = "Y" Then blnPagerAlert = True

            ' Get the latest description
            strMessage = dtPagerAlertLogID.Rows(0)("Description")

        ElseIf dtEmailAlertLogID.Rows.Count > 0 Then

            If strEmailAlertLogID_EmailAlert = "Y" Then blnEmailAlert = True

            ' Get the latest description
            strMessage = dtEmailAlertLogID.Rows(0)("Description")
        End If



        If blnPagerAlert Then
            Log("COVID19BatchConfirm_Fail pager alert")

            CheckCOVID19BatchConfirm_FailAlert(AlertType.PagerAlert, strMessage)
        End If

        If blnEmailAlert Then
            Log("COVID19BatchConfirm_Fail email alert")

            CheckCOVID19BatchConfirm_FailAlert(AlertType.EmailAlert, strMessage)
        End If

        Log("Completed checking CheckCOVID19BatchConfirm_Fail")

    End Sub


    'For CheckCOVID19BatchConfirm_Fail write event log
    Private Sub CheckCOVID19BatchConfirm_FailAlert(ByVal eAlertType As AlertType, ByVal strMessage As String)
        Select Case eAlertType
            Case AlertType.PagerAlert
                WriteEventLog(ConfigurationManager.AppSettings("COVID19BatchConfirm_EventSource"), _
                              ConfigurationManager.AppSettings("COVID19BatchConfirm_PagerEventID"), _
                              EventLogEntryType.Error, strMessage)

            Case AlertType.EmailAlert
                WriteEventLog(ConfigurationManager.AppSettings("COVID19BatchConfirm_EventSource"), _
                              ConfigurationManager.AppSettings("COVID19BatchConfirm_EmailEventID"), _
                              EventLogEntryType.Warning, strMessage)

            Case Else
                Throw New NotImplementedException

        End Select

    End Sub

#End Region

#Region "COVID19DischargeImporter"
    Private Sub CheckCOVID19DischargeImporter_ImportFail(ByRef dtmLastCheck As DateTime, ByVal dtmCurrent As DateTime)
        ' +--------------------------------------------------------------------------------------------------+
        ' | For every [5] minutes, monitor the ScheduleJobLog to see if any COVID19DischargeImporter         |
        ' | fail case. If there is error case, raise pager alert;If there is warning case, raise email alert |
        ' +--------------------------------------------------------------------------------------------------+

        Log("Checking COVID19DischargeImporter_ImportFail")

        ' Check whether need to run
        If DateDiff(DateInterval.Minute, dtmLastCheck, dtmCurrent) < CInt(ConfigurationManager.AppSettings("COVID19DischargeImporter_CheckInterval")) Then
            Log("Smaller than CheckInterval, no need to run")

            Return
        End If

        ' Update now to be the new LastCheckTime
        dtmLastCheck = dtmCurrent

        ' Check logs
        Dim intScheduleJobLogMinuteBefore As Integer = CInt(ConfigurationManager.AppSettings("COVID19DischargeImporter_ScheduleJobLogMinuteBefore"))
        Dim strScheduleJobLogProgramID As String = ConfigurationManager.AppSettings("COVID19DischargeImporter_ProgramID")
        Dim strScheduleJobPagerAlertLogID As String = ConfigurationManager.AppSettings("COVID19DischargeImporter_PagerAlertLogID")
        Dim strScheduleJobEmailAlertLogID As String = ConfigurationManager.AppSettings("COVID19DischargeImporter_EmailAlertLogID")


        Dim strPagerAlertLogID_PagerAlert As String = ConfigurationManager.AppSettings("COVID19DischargeImporter_PagerAlertLogID_PagerAlert")

        Dim strEmailAlertLogID_EmailAlert As String = ConfigurationManager.AppSettings("COVID19DischargeImporter_EmailAlertLogID_EmailAlert")


        Dim dtPagerAlertLogID As DataTable = MonitorBLL.GetScheduleJobLog(strScheduleJobLogProgramID, strScheduleJobPagerAlertLogID, _
                                                         dtmCurrent.Add(New TimeSpan(0, -1 * intScheduleJobLogMinuteBefore, 0)), _
                                                         dtmCurrent)

        Dim dtEmailAlertLogID As DataTable = MonitorBLL.GetScheduleJobLog(strScheduleJobLogProgramID, strScheduleJobEmailAlertLogID, _
                                                          dtmCurrent.Add(New TimeSpan(0, -1 * intScheduleJobLogMinuteBefore, 0)), _
                                                          dtmCurrent)

        Dim strMessage As String = String.Empty
        Dim blnPagerAlert As Boolean = False
        Dim blnEmailAlert As Boolean = False

        If dtPagerAlertLogID.Rows.Count > 0 Then

            If strPagerAlertLogID_PagerAlert = "Y" Then blnPagerAlert = True

            ' Get the latest description
            strMessage = dtPagerAlertLogID.Rows(0)("Description")

        ElseIf dtEmailAlertLogID.Rows.Count > 0 Then

            If strEmailAlertLogID_EmailAlert = "Y" Then blnEmailAlert = True

            ' Get the latest description
            strMessage = dtEmailAlertLogID.Rows(0)("Description")
        End If



        If blnPagerAlert Then
            Log("CheckCOVID19DischargeImporter_ImportFail pager alert")

            CheckCOVID19DischargeImporter_ImportFailAlert(AlertType.PagerAlert, strMessage)
        End If

        If blnEmailAlert Then
            Log("CheckCOVID19DischargeImporter_ImportFail email alert")

            CheckCOVID19DischargeImporter_ImportFailAlert(AlertType.EmailAlert, strMessage)
        End If

        Log("Completed checking CheckCOVID19DischargeImporter_ImportFail")

    End Sub


    'For CheckCOVID19DischargeImporter_ImportFail write event log
    Private Sub CheckCOVID19DischargeImporter_ImportFailAlert(ByVal eAlertType As AlertType, ByVal strMessage As String)
        Select Case eAlertType
            Case AlertType.PagerAlert
                WriteEventLog(ConfigurationManager.AppSettings("COVID19DischargeImporter_EventSource"), _
                              ConfigurationManager.AppSettings("COVID19DischargeImporter_PagerEventID"), _
                              EventLogEntryType.Error, strMessage)

            Case AlertType.EmailAlert
                WriteEventLog(ConfigurationManager.AppSettings("COVID19DischargeImporter_EventSource"), _
                              ConfigurationManager.AppSettings("COVID19DischargeImporter_EmailEventID"), _
                              EventLogEntryType.Warning, strMessage)

            Case Else
                Throw New NotImplementedException

        End Select

    End Sub

#End Region


End Class
