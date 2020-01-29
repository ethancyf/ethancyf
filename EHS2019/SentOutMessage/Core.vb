' [CRE12-012] Infrastructure on Sending Messages through eHealth System Inbox

Imports System.Collections
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.SentOutMessage

Module Core

    ' Start the Schedule Job of "Sent Out Message"
    Public Sub Main()
        Dim objScheduleJob As New ScheduleJob
        objScheduleJob.Start()
    End Sub

End Module

' For "Sent Out Message", to send all approved message(s) to Service Provider in non-peak hour
Public Class ScheduleJob
    Inherits CommonScheduleJob.BaseScheduleJob

#Region "Audit Log Description"
    Public Class AuditLogDesc
        Public Const JobStarted_ID As String = LogID.LOG00001
        Public Const JobStarted As String = "Process Start"

        Public Const GetSentOutMsgIDByT_ID As String = LogID.LOG00002
        Public Const GetSentOutMsgIDByT As String = "Get all [SOMS_SentOutMsg_ID] with [T] Status"

        Public Const GenerateMessageID_ID As String = LogID.LOG00003
        Public Const GenerateMessageID As String = "Generate [Message_ID]"

        Public Const ExecSPtoSendOutMsg_ID As String = LogID.LOG00004
        Public Const ExecSPtoSendOutMsg As String = "Execute Stored Procedure to send out message"

        Public Const JobEnded_ID As String = LogID.LOG00005
        Public Const JobEnded As String = "Process End"
    End Class
#End Region

    Protected Overrides ReadOnly Property FunctionCode() As String
        Get
            Return FunctCode.FUNT019916
        End Get
    End Property

#Region "Abstract Property of [CommonScheduleJob.BaseScheduleJob]"
    Public Overrides ReadOnly Property ScheduleJobID() As String
        Get
            Return FunctCode.FUNT019916
        End Get
    End Property
#End Region

#Region "Abstract Method of [CommonScheduleJob.BaseScheduleJob]"
    Protected Overrides Sub Process()
        MyBase.AuditLog.WriteLog(AuditLogDesc.JobStarted_ID, AuditLogDesc.JobStarted)

        Dim udtSentOutMessageBLL As SentOutMessageBLL = New SentOutMessageBLL()
        Dim udtGeneralFunction As GeneralFunction = New GeneralFunction()
        Dim i As Integer = 0
        Dim strSentOutMsgIDList As ArrayList
        Dim strSentOutMsgID As String
        Dim strInboxMsgID As String

        strSentOutMsgIDList = udtSentOutMessageBLL.GetSentOutMsgIDByRecordStatus(SentOutMessageModel.SO_MSG_RECORD_STATUS_T)
        MyBase.AuditLog.AddDescripton("Total Record", strSentOutMsgIDList.Count.ToString().Trim())
        MyBase.AuditLog.WriteLog(AuditLogDesc.GetSentOutMsgIDByT_ID, AuditLogDesc.GetSentOutMsgIDByT)

        If strSentOutMsgIDList.Count > 0 Then

            For Each strSentOutMsgID In strSentOutMsgIDList

                i += 1

                strInboxMsgID = udtGeneralFunction.generateInboxMsgID()
                MyBase.AuditLog.AddDescripton("SOMS_SentOutMsg_ID", strSentOutMsgID)
                MyBase.AuditLog.AddDescripton("Message_ID", strInboxMsgID)
                MyBase.AuditLog.WriteLog(AuditLogDesc.GenerateMessageID_ID, AuditLogDesc.GenerateMessageID)

                If udtSentOutMessageBLL.SendMessage(strSentOutMsgID, strInboxMsgID) Then

                    ' Success to send message
                    MyBase.AuditLog.AddDescripton("SOMS_SentOutMsg_ID", strSentOutMsgID)
                    MyBase.AuditLog.AddDescripton("Message_ID", strInboxMsgID)
                    MyBase.AuditLog.WriteLog(AuditLogDesc.ExecSPtoSendOutMsg_ID, AuditLogDesc.ExecSPtoSendOutMsg)

                Else

                    ' Error to send message

                End If

            Next

        Else

            ' No message is required to be sent out

        End If

        MyBase.AuditLog.AddDescripton("Total Message Sent", i.ToString())
        MyBase.AuditLog.WriteLog(AuditLogDesc.JobEnded_ID, AuditLogDesc.JobEnded)
    End Sub
#End Region

End Class
