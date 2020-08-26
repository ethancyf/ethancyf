' [CRE13-006] HCVS Ceiling

Imports Common.Component
Imports Common.Component.EHSAccount

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

        Public Const CalculationStart_ID As String = LogID.LOG00003
        Public Const CalculationStart As String = "Calculation Start"

        Public Const CalculationEnd_ID As String = LogID.LOG00004
        Public Const CalculationEnd As String = "Calculation End"

        Public Const ConfigError_ID As String = LogID.LOG00005
        Public Const ConfigError As String = "Config Error"

        Public Const Exception_ID As String = LogID.LOG00006
        Public Const Exception As String = "Exception"
    End Class
#End Region

    Protected Overrides ReadOnly Property FunctionCode() As String
        Get
            Return Common.Component.ScheduleJobFunctionCode.SubsidizeWriteOffGenerator
        End Get
    End Property

#Region "Abstract Property of [CommonScheduleJob.BaseScheduleJob]"
    Public Overrides ReadOnly Property ScheduleJobID() As String
        Get
            Return Common.Component.ScheduleJobID.SubsidizeWriteOffGenerator
        End Get
    End Property
#End Region

#Region "Abstract Method of [CommonScheduleJob.BaseScheduleJob]"
    'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Protected Overrides Sub Process()
        Dim udtSubsidizeWriteOffBLL As New SubsidizeWriteOffBLL()

        Dim intPage As Integer = CInt(System.Configuration.ConfigurationManager.AppSettings("GeneratorID").Trim())
        Dim intTotalPage As Integer = CInt(System.Configuration.ConfigurationManager.AppSettings("TotalGenerator").Trim())
        'Minute of Duration
        Dim intScheduleDuration As Integer = CInt(System.Configuration.ConfigurationManager.AppSettings("Duration").Trim())
        Dim dtmScheduleEndTime As Date = DateAdd(DateInterval.Minute, intScheduleDuration, Now())
        Dim blnIsWithinSchedule As Boolean = True

        Dim intMaxFailCount As Integer = CInt(System.Configuration.ConfigurationManager.AppSettings("MaxFailCount").Trim())

        Dim intCount As Integer = 0 'for calculation
        Dim intCompletedRecord As Integer = 0   'for DB update of queue
        Dim intFailCount As Integer = 0   'for record throw exception

        Dim udtSubsidizeWriteOffGeneratorQueue_P As SubsidizeWriteOffGeneratorQueue
        Dim udtSubsidizeWriteOffGeneratorQueueItem As SubsidizeWriteOffGeneratorQueueItem

        Dim dtmCalculation_Start As Date
        Dim dtmCalculation_End As Date
        Dim lngTotalDuration As Long    'second
        Dim intAverDuration As Integer     'ms

        ' Input Validation
        'INT20-0013 (Fix VBE and HCSP timeout during DeathRecordMatching) [Start][Winnie]
        If intPage <= 0 OrElse intTotalPage <= 0 OrElse intScheduleDuration <= 0 OrElse intMaxFailCount <= 0 Then
            'INT20-0013 (Fix VBE and HCSP timeout during DeathRecordMatching) [End][Winnie]
            MyBase.AuditLog.AddDescripton("Page", CStr(intPage))
            MyBase.AuditLog.AddDescripton("Message", "Config value must be greater than 0")
            MyBase.AuditLog.WriteLog(AuditLogDesc.ConfigError_ID, AuditLogDesc.ConfigError)
            Return
        End If

        If intPage > intTotalPage Then
            MyBase.AuditLog.AddDescripton("Page", CStr(intPage))
            MyBase.AuditLog.AddDescripton("Message", "Page cannot be greater than Total Page")
            MyBase.AuditLog.WriteLog(AuditLogDesc.ConfigError_ID, AuditLogDesc.ConfigError)
            Return
        End If

        ' Retrieve Pending Record from "SubsidizeWriteOffGeneratorQueue"
        MyBase.AuditLog.AddDescripton("Page", CStr(intPage))
        MyBase.AuditLog.AddDescripton("Total Page", CStr(intTotalPage))
        MyBase.AuditLog.AddDescripton("Schedule Duration (min)", intScheduleDuration)
        MyBase.AuditLog.AddDescripton("Maximum Fail Count", intMaxFailCount)
        MyBase.AuditLog.WriteLog(AuditLogDesc.RetrieveQueueStart_ID, AuditLogDesc.RetrieveQueueStart)

        Try

            udtSubsidizeWriteOffGeneratorQueue_P = udtSubsidizeWriteOffBLL.GetSubsidizeWriteOffGeneratorQueue(SubsidizeWriteOffGeneratorQueueItem.RECORD_STATUS_P, intPage, intTotalPage)

        Catch ex As Exception

            MyBase.AuditLog.AddDescripton("Page", CStr(intPage))
            MyBase.AuditLog.AddDescripton("Message", ex.Message)
            MyBase.AuditLog.WriteLog(AuditLogDesc.Exception_ID, AuditLogDesc.Exception)
            Throw

        End Try

        MyBase.AuditLog.AddDescripton("Page", CStr(intPage))
        MyBase.AuditLog.AddDescripton("No. of Record Retrieved", CStr(udtSubsidizeWriteOffGeneratorQueue_P.Count))
        MyBase.AuditLog.WriteLog(AuditLogDesc.RetrieveQueueEnd_ID, AuditLogDesc.RetrieveQueueEnd)

        dtmCalculation_Start = Now()

        ' Check Schedule End Time
        If dtmScheduleEndTime <= Now() Then
            blnIsWithinSchedule = False
        End If

        ' Calculate Subsidize Write Off
        MyBase.AuditLog.AddDescripton("Page", CStr(intPage))
        MyBase.AuditLog.WriteLog(AuditLogDesc.CalculationStart_ID, AuditLogDesc.CalculationStart)

        'INT20-0013 (Fix VBE and HCSP timeout during DeathRecordMatching) [Start][Winnie]        
        While (udtSubsidizeWriteOffGeneratorQueue_P.Count > 0 AndAlso blnIsWithinSchedule AndAlso intMaxFailCount > intFailCount)
            'INT20-0013 (Fix VBE and HCSP timeout during DeathRecordMatching) [End][Winnie]

            udtSubsidizeWriteOffGeneratorQueueItem = udtSubsidizeWriteOffGeneratorQueue_P.Dequeue()

            Try
                ' CRE20-005 (Providing users' data in HCVS to eHR Patient Portal) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                'Get Write Off Detail
                udtSubsidizeWriteOffBLL.GetTotalWriteOff(udtSubsidizeWriteOffGeneratorQueueItem.DocCode, _
                                                         udtSubsidizeWriteOffGeneratorQueueItem.DocID, _
                                                         udtSubsidizeWriteOffGeneratorQueueItem.DOB, _
                                                         udtSubsidizeWriteOffGeneratorQueueItem.ExactDOB, _
                                                         udtSubsidizeWriteOffGeneratorQueueItem.DOD, _
                                                         udtSubsidizeWriteOffGeneratorQueueItem.ExactDOD, _
                                                         udtSubsidizeWriteOffGeneratorQueueItem.SchemeCode, _
                                                         udtSubsidizeWriteOffGeneratorQueueItem.SubsidizeCode, _
                                                         eHASubsidizeWriteOff_CreateReason.TxEnquiry, _
                                                         WriteOff.UpdateDB
                                                         )
                ' CRE20-005 (Providing users' data in HCVS to eHR Patient Portal) [End][Chris YIM]	

                intCount += 1

                ' Update Record Status to "SubsidizeWriteOffGeneratorQueue" & write log to "SubsidizeWriteOffGeneratorQueueLog"
                If udtSubsidizeWriteOffBLL.UpdateSubsidizeWriteOffGeneratorQueue(udtSubsidizeWriteOffGeneratorQueueItem, SubsidizeWriteOffGeneratorQueueItem.RECORD_STATUS_C) Then
                    intCompletedRecord += 1
                End If

            Catch ex As Exception

                'INT20-0013 (Fix VBE and HCSP timeout during DeathRecordMatching) [Start][Winnie]
                '-----------------------------------------------------------------------------------------
                ' Continue next record
                MyBase.AuditLog.AddDescripton("Page", CStr(intPage))
                MyBase.AuditLog.AddDescripton("Message", ex.Message)
                MyBase.AuditLog.AddDescripton("ExceptionRowID", CStr(udtSubsidizeWriteOffGeneratorQueueItem.RowID))
                MyBase.AuditLog.WriteLog(AuditLogDesc.Exception_ID, AuditLogDesc.Exception)

                intFailCount += 1

                'MyBase.AuditLog.AddDescripton("No. of Record Calculated", CStr(intCount))
                'MyBase.AuditLog.AddDescripton("No. of Record Completed", CStr(intCompletedRecord))
                'Throw
                'INT20-0013 (Fix VBE and HCSP timeout during DeathRecordMatching) [End][Winnie]
            End Try

            ' Check Schedule End Time
            If dtmScheduleEndTime <= Now() Then
                blnIsWithinSchedule = False
            End If
        End While

        ' Get Result Summary
        dtmCalculation_End = Now()
        lngTotalDuration = DateDiff(DateInterval.Second, dtmCalculation_Start, dtmCalculation_End)
        If intCompletedRecord > 0 Then
            intAverDuration = CInt((CDbl(lngTotalDuration) / intCompletedRecord) * 1000)
        Else
            intAverDuration = 0
        End If

        MyBase.AuditLog.AddDescripton("Page", CStr(intPage))
        MyBase.AuditLog.AddDescripton("No. of Record Calculated", CStr(intCount))
        MyBase.AuditLog.AddDescripton("No. of Record Completed", CStr(intCompletedRecord))
        'INT20-0013 (Fix VBE and HCSP timeout during DeathRecordMatching) [Start][Winnie]
        MyBase.AuditLog.AddDescripton("No. of Record Throw Exception", CStr(intFailCount))
        'INT20-0013 (Fix VBE and HCSP timeout during DeathRecordMatching) [End][Winnie]
        MyBase.AuditLog.AddDescripton("Total Duration (min)", CStr(CInt(lngTotalDuration / 60)))
        MyBase.AuditLog.AddDescripton("Average Duration (ms)", CStr(intAverDuration))
        MyBase.AuditLog.WriteLog(AuditLogDesc.CalculationEnd_ID, AuditLogDesc.CalculationEnd)
    End Sub
    'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]
#End Region

End Class
