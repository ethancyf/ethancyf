Imports Common.Component
Imports Common.Component.Token.TokenBLL
Imports Common.eHRIntegration.BLL
Imports Common.eHRIntegration.BLL.eHRServiceBLL
Imports Common.eHRIntegration.DAL
Imports Common.eHRIntegration.Model.Xml.eHRService
Imports System.Configuration
Imports System.Xml


Module Core

    Sub Main()
        Dim objScheduleJob As New ScheduleJob
        objScheduleJob.Start()
    End Sub

    Public Class ScheduleJob
        Inherits CommonScheduleJob.BaseScheduleJob

        Private Class DB_FIELD
            Public Const QUEUE_ID As String = "Queue_ID"
            Public Const QUEUE_TYPE As String = "Queue_Type"
            Public Const QUEUE_CONTENT As String = "Queue_Content"
            Public Const USER_ID As String = "User_ID"
            Public Const TOKEN_SERIAL_NO As String = "Token_Serial_No"
            Public Const TOKEN_SERIAL_NO_REPLACEMENT As String = "Token_Serial_No_Replacement"
            Public Const ACTION_REMARK As String = "Action_Remark"
            Public Const ACTION_DTM As String = "Action_Dtm"
            Public Const MESSAGE_TIMESTAMP As String = "Message_Timestamp"
        End Class

        Private Class Action_Result_Type
            Public Const Complete As String = "C"
            Public Const Fail As String = "F"
        End Class

#Region "AlertType"
        Private Enum AlertType
            NA
            PagerAlert
            EmailAlert
        End Enum

#End Region

#Region "Audit Log Description"
        Public Class AuditLogDesc
            Public Const JobProcessStart_ID As String = LogID.LOG00001
            Public Const JobProcessStart As String = "Process Start"

            Public Const TaskStart_ID As String = LogID.LOG00002
            Public Const TaskStart As String = "Queue ID [{0}]: Action {1} Start"

            Public Const TaskSuccess_ID As String = LogID.LOG00003
            Public Const TaskSuccess As String = "Queue ID [{0}]: Action {1} Completed"

            Public Const TaskFail_ID As String = LogID.LOG00004
            Public Const TaskFail As String = "Queue ID [{0}]: Action {1} Failed{2}"
            Public Const TaskFailException As String = " <Exception: {0}>"

            Public Const TaskEnd_ID As String = LogID.LOG00005
            Public Const TaskEnd As String = "Queue ID [{0}]: Action {1} End"

            Public Const NoTask_ID As String = LogID.LOG00006
            Public Const NoTask As String = "No re-run outstanding token job"

            Public Const Alert_ReRun_Fail_ID As String = LogID.LOG00007
            Public Const Alert_ReRun_Fail As String = "{0}/{1} re-run outstanding token job fail"

            Public Const Alert_OutSync_ID As String = LogID.LOG00008
            Public Const Alert_OutSync As String = "{0} out-sync cases found in eHS(S) in {1}"

            Public Const JobProcessEnd_ID As String = LogID.LOG00009
            Public Const JobProcessEnd As String = "Process End"

            Public Const Nothing_Error_ID As String = LogID.LOG00010
            Public Const Nothing_Error As String = "<Exception: No result table is returned>"
        End Class
#End Region

        Protected Overrides ReadOnly Property FunctionCode() As String
            Get
                Return Common.Component.ScheduleJobID.TokenNotification
            End Get
        End Property

        Public Overrides ReadOnly Property ScheduleJobID() As String
            Get
                Return Common.Component.ScheduleJobID.TokenNotification
            End Get
        End Property

        Protected Overrides Sub Process()
            Dim dtmStartProcess As DateTime = DateTime.Now

            Log(AuditLogDesc.JobProcessStart_ID, AuditLogDesc.JobProcessStart)

            Dim dsTokenNotificationQueue As DataSet = Nothing
            Dim dtTokenNotificationQueue As DataTable = Nothing
            Dim udteHRServiceDAL As eHRServiceDAL = New eHRServiceDAL
            Dim udtTokenBLL As Token.TokenBLL = New Token.TokenBLL
            Dim enumActionType As Token.TokenBLL.EnumTokenActionActionType = Nothing

            Dim intTotalCount As Integer = 0
            Dim intFailCount As Integer = 0

            'Get the Queue with status "P" or "E"
            dsTokenNotificationQueue = udteHRServiceDAL.GeteHRInterfaceIntegrationQueue()

            If Not dsTokenNotificationQueue Is Nothing AndAlso dsTokenNotificationQueue.Tables.Count > 0 Then

                dtTokenNotificationQueue = dsTokenNotificationQueue.Tables(0)
                intTotalCount = dtTokenNotificationQueue.Rows.Count

                If intTotalCount > 0 Then

                    For Each dr As DataRow In dtTokenNotificationQueue.Rows
                        'Write log
                        Log(AuditLogDesc.TaskStart_ID, String.Format(AuditLogDesc.TaskStart, dr(DB_FIELD.QUEUE_ID), dr(DB_FIELD.QUEUE_TYPE)))

                        Dim strData As String = dr(DB_FIELD.QUEUE_CONTENT)

                        Dim blnFailure As Boolean = False
                        enumActionType = Nothing

                        Try
                            Select Case dr(DB_FIELD.QUEUE_TYPE)
                                Case enumEhrIntegrationInterfaceQueueType.SETSHARE.ToString
                                    enumActionType = EnumTokenActionActionType.NOTIFYSETSHARE

                                    'Send to eHR
                                    Dim udtInSeteHRSSTokenSharedXmlModel As InSeteHRSSTokenSharedXmlModel = Nothing

                                    udtInSeteHRSSTokenSharedXmlModel = (New eHRServiceBLL).GetEhrWebS(strData, enumEhrFunctionResult.seteHRSSTokenSharedResult) ', InSeteHRSSTokenSharedXmlModel)

                                    If udtInSeteHRSSTokenSharedXmlModel.ResultCodeEnum = eHRResultCode.R9999_UnexpectedFailure Then
                                        blnFailure = True
                                    End If

                                Case enumEhrIntegrationInterfaceQueueType.REPLACETOKEN.ToString
                                    enumActionType = EnumTokenActionActionType.NOTIFYREPLACETOKEN

                                    'Send to eHR
                                    Dim udtInReplaceeHRSSTokenXmlModel As InReplaceeHRSSTokenXmlModel = Nothing

                                    udtInReplaceeHRSSTokenXmlModel = (New eHRServiceBLL).GetEhrWebS(strData, enumEhrFunctionResult.replaceeHRSSTokenResult) ', InReplaceeHRSSTokenXmlModel)

                                    If udtInReplaceeHRSSTokenXmlModel.ResultCodeEnum = eHRResultCode.R9999_UnexpectedFailure Then
                                        blnFailure = True
                                    End If

                                Case enumEhrIntegrationInterfaceQueueType.DEACTIVATETOKEN.ToString
                                    enumActionType = EnumTokenActionActionType.NOTIFYDELETETOKEN

                                    'Send to eHR
                                    Dim udtInNotifyeHRSSTokenDeactivatedXmlModel As InNotifyeHRSSTokenDeactivatedXmlModel = Nothing

                                    udtInNotifyeHRSSTokenDeactivatedXmlModel = (New eHRServiceBLL).GetEhrWebS(strData, enumEhrFunctionResult.notifyeHRSSTokenDeactivatedResult) ', InNotifyeHRSSTokenDeactivatedXmlModel)

                                    If udtInNotifyeHRSSTokenDeactivatedXmlModel.ResultCodeEnum = eHRResultCode.R9999_UnexpectedFailure Then
                                        blnFailure = True
                                    End If

                                Case Else
                                    Throw New Exception("eHRInterfaceIntegrationQueue: An undefined Queue Type is found.")
                            End Select

                            If blnFailure Then
                                'Write log
                                Log(AuditLogDesc.TaskFail_ID, String.Format(AuditLogDesc.TaskFail, dr(DB_FIELD.QUEUE_ID), dr(DB_FIELD.QUEUE_TYPE), String.Empty))

                                'Update to queue
                                udteHRServiceDAL.UpdateeHRInterfaceIntegrationQueue(dr(DB_FIELD.QUEUE_ID), eHRServiceDAL.RECORD_STATUS_E, dtmStartProcess, Nothing, DateTime.Now)

                                intFailCount += 1
                            Else
                                'Write log
                                Log(AuditLogDesc.TaskSuccess_ID, String.Format(AuditLogDesc.TaskSuccess, dr(DB_FIELD.QUEUE_ID), dr(DB_FIELD.QUEUE_TYPE)))

                                'Update to queue
                                udteHRServiceDAL.UpdateeHRInterfaceIntegrationQueue(dr(DB_FIELD.QUEUE_ID), eHRServiceDAL.RECORD_STATUS_C, dtmStartProcess, DateTime.Now, Nothing)
                            End If

                        Catch ex As Exception
                            blnFailure = True

                            'Write log
                            Log(AuditLogDesc.TaskFail_ID, String.Format(AuditLogDesc.TaskFail, dr(DB_FIELD.QUEUE_ID), dr(DB_FIELD.QUEUE_TYPE), String.Format(AuditLogDesc.TaskFailException, ex.ToString)))

                            'Update to queue
                            udteHRServiceDAL.UpdateeHRInterfaceIntegrationQueue(dr(DB_FIELD.QUEUE_ID), eHRServiceDAL.RECORD_STATUS_E, dtmStartProcess, Nothing, DateTime.Now)

                            intFailCount += 1

                        End Try

                        'Write log
                        Log(AuditLogDesc.TaskEnd_ID, String.Format(AuditLogDesc.TaskEnd, dr(DB_FIELD.QUEUE_ID), dr(DB_FIELD.QUEUE_TYPE)))

                        'Add TokenAction
                        Dim enumResult As EnumTokenActionActionResult
                        If Not blnFailure Then
                            enumResult = EnumTokenActionActionResult.C
                        Else
                            enumResult = EnumTokenActionActionResult.F
                        End If

                        udtTokenBLL.AddTokenAction(EnumTokenActionParty.EHS, _
                                                    EnumTokenActionParty.EHR, _
                                                    enumActionType, _
                                                    IIf(IsDBNull(dr(DB_FIELD.USER_ID)), String.Empty, dr(DB_FIELD.USER_ID)), _
                                                    dr(DB_FIELD.TOKEN_SERIAL_NO), _
                                                    IIf(IsDBNull(dr(DB_FIELD.TOKEN_SERIAL_NO_REPLACEMENT)), String.Empty, dr(DB_FIELD.TOKEN_SERIAL_NO_REPLACEMENT)), _
                                                    IIf(IsDBNull(dr(DB_FIELD.ACTION_REMARK)), String.Empty, dr(DB_FIELD.ACTION_REMARK)), _
                                                    True, enumResult, dr(DB_FIELD.ACTION_DTM), DateTime.Now, dr(DB_FIELD.MESSAGE_TIMESTAMP), _
                                                    IIf(IsDBNull(dr(DB_FIELD.QUEUE_ID)), String.Empty, dr(DB_FIELD.QUEUE_ID)))

                    Next
                Else
                    Log(AuditLogDesc.NoTask_ID, AuditLogDesc.NoTask)
                End If
            Else
                Log(AuditLogDesc.Nothing_Error_ID, AuditLogDesc.Nothing_Error)
            End If

            'If Re-Run job failed, the alert is arised.
            If intFailCount > 0 Then
                Log(AuditLogDesc.Alert_ReRun_Fail_ID, String.Format(AuditLogDesc.Alert_ReRun_Fail, intFailCount, intTotalCount))
            End If

            'If OutSync case found between eHS(S) and eHRSS, the alert is arised.
            Dim intTokenOutSync As Integer = udtTokenBLL.GetTokenOutSyncCountBtwEHSEHR(dtmStartProcess)

            If intTokenOutSync > 0 Then
                Dim strS As String = String.Empty

                Log(AuditLogDesc.Alert_OutSync_ID, String.Format(AuditLogDesc.Alert_OutSync, intTokenOutSync, DateAdd(DateInterval.Day, -1, dtmStartProcess).ToString("yyyy-MM-dd")))
            End If

            Log(AuditLogDesc.JobProcessEnd_ID, AuditLogDesc.JobProcessEnd)
        End Sub

    End Class

End Module
