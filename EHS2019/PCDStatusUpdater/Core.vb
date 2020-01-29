' [CRE13-006] HCVS Ceiling

Imports Common.Component
Imports Common.PCD
Imports Common.PCD.Component.PCDStatus
Imports Common.PCD.WebService.Interface
Imports Common.DataAccess

Module Core
    Sub Main()
        Dim objScheduleJob As New ScheduleJob
        objScheduleJob.Start()
    End Sub

End Module

Public Class ScheduleJob
    Inherits CommonScheduleJob.BaseScheduleJob

#Region "Audit Log Description"
    Public Class AuditLogDesc
        Public Const RetrieveQueueStart_ID As String = LogID.LOG00001
        Public Const RetrieveQueueStart As String = "Retrieve Queue Start"

        Public Const RetrieveQueueEnd_ID As String = LogID.LOG00002
        Public Const RetrieveQueueEnd As String = "Retrieve Queue End"

        Public Const ProcessStart_ID As String = LogID.LOG00003
        Public Const ProcessStart As String = "Check PCD Account and Enrolment Status Start"

        Public Const ProcessEnd_ID As String = LogID.LOG00004
        Public Const ProcessEnd As String = "Check PCD Account and Enrolment Status End"

        Public Const ConfigError_ID As String = LogID.LOG00005
        Public Const ConfigError As String = "Config Error"

        Public Const Exception_ID As String = LogID.LOG00006
        Public Const Exception As String = "Exception"
    End Class
#End Region

#Region "Abstract Property of [CommonScheduleJob.BaseScheduleJob]"
    Protected Overrides ReadOnly Property FunctionCode() As String
        Get
            Return Common.Component.ScheduleJobFunctionCode.PCDStatusUpdater
        End Get
    End Property
#End Region

#Region "Abstract Property of [CommonScheduleJob.BaseScheduleJob]"
    Public Overrides ReadOnly Property ScheduleJobID() As String
        Get
            Return Common.Component.ScheduleJobID.PCDStatusUpdater
        End Get
    End Property
#End Region

#Region "Abstract Method of [CommonScheduleJob.BaseScheduleJob]"

    Protected Overrides Sub Process()
        Dim udtPCDStatusBLL As New PCDStatusBLL()

        'Minute of Duration
        Dim intScheduleDuration As Integer = CInt(System.Configuration.ConfigurationManager.AppSettings("Duration").Trim())
        Dim dtmScheduleEndTime As Date = DateAdd(DateInterval.Minute, intScheduleDuration, Now())
        Dim blnIsWithinSchedule As Boolean = True

        Dim intCount As Integer = 0 'for calculation
        Dim intCompletedRecord As Integer = 0   'for DB update of queue

        ' -------------------------------------------------------------
        ' 1. Retrieve Pending Record(s) from "PCDStatusUpdateQueue"
        ' -------------------------------------------------------------
        Dim udtPCDStatusUpdaterQueue As PCDStatusUpdaterQueue = Nothing
        Dim udtPCDStatusUpdaterQueueItem As PCDStatusUpdaterQueueModel = Nothing

        MyBase.AuditLog.AddDescripton("Schedule Duration (min)", intScheduleDuration)
        MyBase.AuditLog.WriteLog(AuditLogDesc.RetrieveQueueStart_ID, AuditLogDesc.RetrieveQueueStart)

        Try

            udtPCDStatusUpdaterQueue = udtPCDStatusBLL.GetPCDStatusUpdaterQueue(PCDStatusUpdaterQueueModel.DBRecordStatus.Pending)

        Catch ex As Exception
            MyBase.AuditLog.AddDescripton("Message", ex.Message)
            MyBase.AuditLog.WriteLog(AuditLogDesc.Exception_ID, AuditLogDesc.Exception)
            Throw

        End Try

        MyBase.AuditLog.AddDescripton("No. of Record Retrieved", CStr(udtPCDStatusUpdaterQueue.Count))
        MyBase.AuditLog.WriteLog(AuditLogDesc.RetrieveQueueEnd_ID, AuditLogDesc.RetrieveQueueEnd)

        ' -------------------------------------------------------------
        ' 2. Process Pending Record(s) to enquiry PCD Web Service
        ' -------------------------------------------------------------
        Dim dtmProcess_Start As DateTime = Now()

        ' Check Schedule End Time
        If dtmScheduleEndTime <= Now() Then
            blnIsWithinSchedule = False
        End If

        ' Start to process to enquiry PCD web service
        Dim udtPCDWebService As PCDWebService = New PCDWebService(Me.FunctionCode)
        Dim udtResult As WebService.Interface.PCDCheckAccountStatusResult = Nothing

        MyBase.AuditLog.AddDescripton("WebMethod", "PCDCheckAccountStatus")
        MyBase.AuditLog.WriteLog(AuditLogDesc.ProcessStart_ID, AuditLogDesc.ProcessStart)

        While (udtPCDStatusUpdaterQueue.Count > 0 AndAlso blnIsWithinSchedule)
            udtPCDStatusUpdaterQueueItem = udtPCDStatusUpdaterQueue.Dequeue()
            udtResult = Nothing

            Try
                udtResult = udtPCDWebService.PCDCheckAccountStatus(udtPCDStatusUpdaterQueueItem.DocID)

                intCount += 1

                Dim strMessage As String = String.Empty

                ' Update SP's Join PCD Status to "ServiceProvider"
                If udtResult.UpdateJoinPCDStatus(udtPCDStatusUpdaterQueueItem.SPID, "eHS", strMessage) Then

                    ' Update Record Status to "PCDStatusUpdaterQueue"
                    If udtPCDStatusBLL.UpdatePCDStatusUpdaterQueue(udtPCDStatusUpdaterQueueItem.SPID, PCDStatusUpdaterQueueModel.DBRecordStatus.Completed) Then
                        intCompletedRecord += 1
                    Else
                        strMessage = String.Format("Fail, SPID({0}) is not completed the queue.", udtPCDStatusUpdaterQueueItem.SPID)
                    End If

                End If

                ConsoleLog(strMessage)

            Catch ex As Exception
                MyBase.AuditLog.AddDescripton("Message", ex.Message)
                MyBase.AuditLog.AddDescripton("Exception on Processing SPID", CStr(udtPCDStatusUpdaterQueueItem.SPID))
                MyBase.AuditLog.AddDescripton("No. of Record Calculated", CStr(intCount))
                MyBase.AuditLog.AddDescripton("No. of Record Completed", CStr(intCompletedRecord))
                MyBase.AuditLog.WriteLog(AuditLogDesc.Exception_ID, AuditLogDesc.Exception)

                ConsoleLog(String.Format("Error, SPID({0}) is terminated to process. Exception: {1}", udtPCDStatusUpdaterQueueItem.SPID, ex.Message))

            End Try

            ' Check Schedule End Time
            If dtmScheduleEndTime <= Now() Then
                blnIsWithinSchedule = False
            End If

        End While

        ' -------------------------------------------------------------
        ' 3. Get Result Summary
        ' -------------------------------------------------------------
        Dim lngTotalDuration As Long = DateDiff(DateInterval.Second, dtmProcess_Start, Now())
        Dim intAverDuration As Integer

        If intCompletedRecord > 0 Then
            intAverDuration = CInt((CDbl(lngTotalDuration) / intCompletedRecord) * 1000)
        Else
            intAverDuration = 0
        End If

        MyBase.AuditLog.AddDescripton("No. of Record Calculated", CStr(intCount))
        MyBase.AuditLog.AddDescripton("No. of Record Completed", CStr(intCompletedRecord))
        MyBase.AuditLog.AddDescripton("Total Duration (min)", CStr(CInt(lngTotalDuration / 60)))
        MyBase.AuditLog.AddDescripton("Average Duration (ms)", CStr(intAverDuration))
        MyBase.AuditLog.WriteLog(AuditLogDesc.ProcessEnd_ID, AuditLogDesc.ProcessEnd)

    End Sub

#End Region

#Region "Console Log"
    Public Shared Sub ConsoleLog(ByVal strText As String)

        Console.WriteLine("<" + Now.ToString("yyyy-MM-dd HH:mm:ss") + "> " + strText)

    End Sub
#End Region


End Class
