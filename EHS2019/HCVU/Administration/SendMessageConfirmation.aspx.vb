Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.Profession
Imports Common.ComObject
Imports Common.Format
Imports Common.Component.HCVUUser
Imports Common.Validation
Imports Common.Component.SentOutMessage

Partial Public Class SendMessageConfirmation
    'Inherits System.Web.UI.Page
    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][KarlL]
    ' -------------------------------------------------------------------------
    Inherits BasePageWithControl
    'Inherits BasePageWithGridView
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][KarlL]


#Region "Variables"

    Private udtSentOutMessageBLL As New SentOutMessageBLL
    Private udtFormatter As New Formatter
    Private udtHCVUUser As New HCVUUserBLL
    Private udtValidator As New Validator
    Private _udtAuditLogEntry As AuditLogEntry
#End Region

#Region "Session and constants"

    Private Const FUNCTION_CODE As String = "010901"
    Private Const STATUS_Pending = "P"
    Private Const StatusData_Class As String = "SentOutMessageStatus"
    Private Const RecipientGrp_Prof As String = "PROF"
    Private Const RecipientGrp_Scm As String = "SCM"

    Private Const SESS_PendingMessage As String = "SESS_PendingMessage"
    Private Const SESS_PendingMessageID As String = "SESS_PendingMessageID"
    Private Const SESS_PendingMessageTSMP As String = "SESS_PendingMessageTSMP"
    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][KarlL]
    Private Const SESS_PendingMessageOverridded As String = "SESS_PendingMessageOverridded" 'boolean True/False
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][KarlL]

    Private Class CurrentView
        Public Const PendingMessage As Integer = 0
        Public Const PendingMessageDetail As Integer = 1
        Public Const CompletePendingMessage As Integer = 2
    End Class

#End Region

#Region "Audit Log Description"

    Public Class AuditLogDesc
        Public Const PageLoad As String = "Message Draft Approval loaded"
        Public Const PageLoad_ID As String = LogID.LOG00006

        Public Const MsgListLoad As String = "Pending Approval Message List loaded"
        Public Const MsgListLoad_ID As String = LogID.LOG00007

        Public Const MsgListLoadSuccess As String = "Pending Approval Message List loaded successful"
        Public Const MsgListLoadSuccess_ID As String = LogID.LOG00008

        Public Const SelectMsg As String = "Select Message"
        Public Const SelectMsg_ID As String = LogID.LOG00009

        Public Const ViewMessage As String = "View Message"
        Public Const ViewMessage_ID As String = LogID.LOG00010

        Public Const ApproveClick As String = "Click Approve"
        Public Const ApproveClick_ID As String = LogID.LOG00011

        Public Const ConfirmApprovePopupDisplay As String = "Confirm Approve Pop Up display"
        Public Const ConfirmApprovePopupDisplay_ID As String = LogID.LOG00012

        Public Const ConfirmApproveClick As String = "Click Confirm Approve"
        Public Const ConfirmApproveClick_ID As String = LogID.LOG00013

        Public Const ConfirmApproveClickSuccess As String = "Click Confirm Approve Success"
        Public Const ConfirmApproveClickSuccess_ID As String = LogID.LOG00014

        Public Const ConfirmApproveClickFailByOthers As String = "Click Confirm Approve fail - Update by others"
        Public Const ConfirmApproveClickFailByOthers_ID As String = LogID.LOG00015

        Public Const CancelApproveClick As String = "Click Cancel Approve"
        Public Const CancelApproveClick_ID As String = LogID.LOG00016

        Public Const RejectClick As String = "Click Reject"
        Public Const RejectClick_ID As String = LogID.LOG00017

        Public Const ConfirmRejectPopupDisplay As String = "Confirm Reject Pop Up display"
        Public Const ConfirmRejectPopupDisplay_ID As String = LogID.LOG00018

        Public Const ConfirmRejectClick As String = "Click Confirm Reject"
        Public Const ConfirmRejectClick_ID As String = LogID.LOG00019

        Public Const ConfirmRejectClickSuccess As String = "Click Confirm Reject Success"
        Public Const ConfirmRejectClickSuccess_ID As String = LogID.LOG00020

        Public Const CancelRejectClick As String = "Click Cancel Reject"
        Public Const CancelRejectClick_ID As String = LogID.LOG00021

        Public Const ConfirmRejectClickFailNoReason As String = "Click Confirm Reject fail - No reject reason"
        Public Const ConfirmRejectClickFailNoReason_ID As String = LogID.LOG00022

        Public Const ConfirmRejectClickFailByOthers As String = "Click Confirm Reject fail - Update by others"
        Public Const ConfirmRejectClickFailByOthers_ID As String = LogID.LOG00023

        Public Const BackClick As String = "Click Back"
        Public Const BackClick_ID As String = LogID.LOG00024

        Public Const CompletePageShow As String = "Completion Page loaded"
        Public Const CompletePageShow_ID As String = LogID.LOG00025

        Public Const RejectPageShow As String = "Rejected Page loaded"
        Public Const RejectPageShow_ID As String = LogID.LOG00026

        Public Const FailUpdatePageShow As String = "Fail to update page loaded"
        Public Const FailUpdatePageShow_ID As String = LogID.LOG00027

        Public Const ReturnClick As String = "Click Return"
        Public Const ReturnClick_ID As String = LogID.LOG00028

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
        ' -------------------------------------------------------------------------
        Public Const SearchFail As String = "Search fail"
        Public Const SearchFail_ID As String = LogID.LOG00029

        Public Const SearchFailOverFirstLimit As String = "Search fail - Over 1st limit"
        Public Const SearchFailOverFirstLimit_ID As String = LogID.LOG00030

        Public Const SearchFailOverOverrideLimit As String = "Search fail - Over override limit"
        Public Const SearchFailOverOverrideLimit_ID As String = LogID.LOG00031
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    End Class

#End Region

#Region "Page Event"

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        _udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Set all message box visible to False first
        'Set label pending message on first page to True first
        udcInfoMessageBox.Visible = False
        udcMessageBox.Visible = False
        panHeadingText.Visible = True

        If Not IsPostBack Then
            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][KarlL]
            FunctionCode = FUNCTION_CODE
            Me.Session(SESS_PendingMessageOverridded) = False
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][KarlL]

            _udtAuditLogEntry.WriteLog(AuditLogDesc.PageLoad_ID, AuditLogDesc.PageLoad)
            ClearSession()
            RetrievePendingMessage()
        End If

    End Sub

#End Region


    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][KarlL]
    ' -------------------------------------------------------------------------
#Region "SF Search"

    Protected Overrides Sub SF_InitSearch(ByRef udtAuditLogEntry As AuditLogEntry)

    End Sub

    Protected Overrides Function SF_ValidateSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry) As Boolean
    End Function

    Protected Overrides Function SF_Search(ByRef udtAuditLogEntry As AuditLogEntry, ByVal blnOverrideResultLimit As Boolean) As Common.Component.BaseBLL.BLLSearchResult
        If blnOverrideResultLimit Then
            Return GetPendingMessage(True)
        Else
            Return GetPendingMessage()
        End If
    End Function

    Protected Overrides Function SF_BindSearchResult(ByRef udtAuditLogEntry As AuditLogEntry, ByVal udtBLLSearchResult As Common.Component.BaseBLL.BLLSearchResult) As Integer
        Dim dt As DataTable = New DataTable()

        Try
            dt = CType(udtBLLSearchResult.Data, DataTable)

        Catch ex As Exception
            Throw

        End Try

        Session(SESS_PendingMessage) = dt

        If dt.Rows.Count > 0 Then
            Me.GridViewDataBind(gvPendingMessage, dt, "SOMS_Create_Dtm", "DESC", False)
        Else
            gvPendingMessage.DataSource = Nothing
            gvPendingMessage.DataBind()
        End If

        Return dt.Rows.Count
    End Function

    Protected Overrides Sub SF_ConfirmSearch_Click()
        'Write Start Log
        _udtAuditLogEntry.WriteStartLog(AuditLogDesc.MsgListLoad_ID, AuditLogDesc.MsgListLoad)

        Dim enumSearchResult As SearchResultEnum

        Try
            enumSearchResult = StartSearchFlow(FUNCTION_CODE, _udtAuditLogEntry, udcMessageBox, udcInfoMessageBox, False, True)

        Catch eSQL As SqlClient.SqlException
            _udtAuditLogEntry.AddDescripton("StackTrace", "Unknown SqlException")
            _udtAuditLogEntry.AddDescripton("Message", eSQL.Message)
            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
            ' -------------------------------------------------------------------------
            '_udtAuditLogEntry.WriteEndLog(LogID.LOG00007, SF_AuditLogDescription.SearchFail)
            _udtAuditLogEntry.WriteEndLog(LogID.LOG00029, SF_AuditLogDescription.SearchFail)
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]
            Throw

        Catch ex As Exception
            _udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception")
            _udtAuditLogEntry.AddDescripton("Message", ex.Message)
            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
            ' -------------------------------------------------------------------------
            '_udtAuditLogEntry.WriteEndLog(LogID.LOG00007, SF_AuditLogDescription.SearchFail)
            _udtAuditLogEntry.WriteEndLog(LogID.LOG00029, SF_AuditLogDescription.SearchFail)
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]
            Throw

        End Try

        Select Case enumSearchResult
            Case SearchResultEnum.Success
                _udtAuditLogEntry.WriteEndLog(AuditLogDesc.MsgListLoadSuccess_ID, AuditLogDesc.MsgListLoadSuccess)
                Me.Session(SESS_PendingMessageOverridded) = True

            Case Else
                Throw New Exception("Error: Class = [HCVU.SendMessageConfirmation], Method = [SF_ConfirmSearch_Click], Message = The unexpected error for the result of re-search")

        End Select
    End Sub
    Protected Overrides Sub SF_FinalizeSearch(ByRef udtAuditLogEntry As AuditLogEntry)

    End Sub
    Protected Overrides Sub SF_CancelSearch_Click()
        Select Case Me.MultiViewSendMessageConfirmation.ActiveViewIndex
            Case CurrentView.PendingMessage, CurrentView.PendingMessageDetail, CurrentView.CompletePendingMessage
                Dim udtSM2 As Common.ComObject.SystemMessage
                udtSM2 = New SystemMessage("990001", SeverityCode.SEVD, MsgCode.MSG00016)
                udcMessageBox.AddMessage(udtSM2)
                udcMessageBox.BuildMessageBox("SearchFail")

                'Set panel heading text visible to False
                panHeadingText.Visible = False
        End Select

    End Sub

    Private Function GetPendingMessage(Optional ByVal blnOverrideResultLimit As Boolean = False) As BaseBLL.BLLSearchResult

        Dim udtBLLSearchResult As BaseBLL.BLLSearchResult

        udtBLLSearchResult = udtSentOutMessageBLL.GetSentOutMessageByRecordStatus(FUNCTION_CODE, blnOverrideResultLimit, STATUS_Pending)

        Return udtBLLSearchResult
    End Function
#End Region
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][KarlL]

#Region "Method for whole page"

    Private Sub RetrievePendingMessage()
        'Write Start Log
        _udtAuditLogEntry.WriteStartLog(AuditLogDesc.MsgListLoad_ID, AuditLogDesc.MsgListLoad)

        Dim enumSearchResult As SearchResultEnum

        Try

            If Me.Session(SESS_PendingMessageOverridded) = True Then
                enumSearchResult = StartSearchFlow(FUNCTION_CODE, _udtAuditLogEntry, udcMessageBox, udcInfoMessageBox, False, True)
            Else
                enumSearchResult = StartSearchFlow(FUNCTION_CODE, _udtAuditLogEntry, udcMessageBox, udcInfoMessageBox, False)
            End If


        Catch eSQL As SqlClient.SqlException
            _udtAuditLogEntry.AddDescripton("StackTrace", "Unknown SqlException")
            _udtAuditLogEntry.AddDescripton("Message", eSQL.Message)
            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
            ' -------------------------------------------------------------------------
            '_udtAuditLogEntry.WriteEndLog(LogID.LOG00007, SF_AuditLogDescription.SearchFail)
            _udtAuditLogEntry.WriteEndLog(LogID.LOG00029, SF_AuditLogDescription.SearchFail)
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]
            Throw

        Catch ex As Exception
            _udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception")
            _udtAuditLogEntry.AddDescripton("Message", ex.Message)
            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
            ' -------------------------------------------------------------------------
            '_udtAuditLogEntry.WriteEndLog(LogID.LOG00007, SF_AuditLogDescription.SearchFail)
            _udtAuditLogEntry.WriteEndLog(LogID.LOG00029, SF_AuditLogDescription.SearchFail)
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]
            Throw

        End Try

        Select Case enumSearchResult
            Case SearchResultEnum.Success

                _udtAuditLogEntry.WriteEndLog(AuditLogDesc.MsgListLoadSuccess_ID, AuditLogDesc.MsgListLoadSuccess)

            Case SearchResultEnum.ValidationFail
                'Audit Log has been handled in [SF_ValidateSearch] method

            Case SearchResultEnum.NoRecordFound

                Select Case Me.MultiViewSendMessageConfirmation.ActiveViewIndex
                    Case CurrentView.PendingMessage, CurrentView.PendingMessageDetail, CurrentView.CompletePendingMessage
                        Dim udtSM2 As Common.ComObject.SystemMessage
                        udtSM2 = New SystemMessage("990000", SeverityCode.SEVI, MsgCode.MSG00001)
                        udcInfoMessageBox.AddMessage(udtSM2)
                        udcInfoMessageBox.BuildMessageBox()


                        'Set panel heading text visible to False
                        panHeadingText.Visible = False
                End Select

            Case SearchResultEnum.OverResultList1stLimit_PopUp
                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
                ' -------------------------------------------------------------------------
                '_udtAuditLogEntry.WriteEndLog(LogID.LOG00007, SF_AuditLogDescription.SearchFail_Over1stLimit)
                _udtAuditLogEntry.WriteEndLog(LogID.LOG00030, SF_AuditLogDescription.SearchFail_Over1stLimit)
                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

            Case SearchResultEnum.OverResultList1stLimit_Alert
                Select Case Me.MultiViewSendMessageConfirmation.ActiveViewIndex
                    Case CurrentView.PendingMessage, CurrentView.PendingMessageDetail, CurrentView.CompletePendingMessage
                        Dim udtSM2 As Common.ComObject.SystemMessage
                        udtSM2 = New SystemMessage("990001", SeverityCode.SEVD, MsgCode.MSG00016)
                        udcMessageBox.AddMessage(udtSM2)
                        udcMessageBox.BuildMessageBox()

                        'Set panel heading text visible to False
                        panHeadingText.Visible = False
                End Select

            Case SearchResultEnum.OverResultListOverrideLimit
                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
                ' -------------------------------------------------------------------------
                '_udtAuditLogEntry.WriteEndLog(LogID.LOG00007, SF_AuditLogDescription.SearchFail_OverOverrideLimit)
                _udtAuditLogEntry.WriteEndLog(LogID.LOG00031, SF_AuditLogDescription.SearchFail_OverOverrideLimit)
                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]
                Select Case Me.MultiViewSendMessageConfirmation.ActiveViewIndex
                    Case CurrentView.PendingMessage, CurrentView.PendingMessageDetail, CurrentView.CompletePendingMessage
                        Dim udtSM2 As Common.ComObject.SystemMessage
                        udtSM2 = New SystemMessage("990001", SeverityCode.SEVD, MsgCode.MSG00016)
                        udcMessageBox.AddMessage(udtSM2)
                        udcMessageBox.BuildMessageBox()

                        'Set panel heading text visible to False
                        panHeadingText.Visible = False
                End Select

            Case Else
                Throw New Exception("Error: Class = [HCVU.SendMessageConfirmation], Method = [ibtnSearch_Click], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")

        End Select

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][KarlL]

        'Get data, but now use datatable as template
        'Dim dt As DataTable = New DataTable()

        'dt = Me.udtSentOutMessageBLL.GetSentOutMessageByRecordStatus(STATUS_Pending)
        'Session(SESS_PendingMessage) = dt
        'Me.GridViewDataBind(gvPendingMessage, dt, "SOMS_Create_Dtm", "DESC", False)

        'Select Case Me.MultiViewSendMessageConfirmation.ActiveViewIndex
        '    Case CurrentView.PendingMessage, CurrentView.PendingMessageDetail, CurrentView.CompletePendingMessage
        '        If dt.Rows.Count = 0 Then
        '            Dim udtSM2 As Common.ComObject.SystemMessage
        '            udtSM2 = New SystemMessage("990000", SeverityCode.SEVI, MsgCode.MSG00001)
        '            udcInfoMessageBox.AddMessage(udtSM2)
        '            udcInfoMessageBox.BuildMessageBox()
        '            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information

        '            'Set panel heading text visible to False
        '            panHeadingText.Visible = False
        '        End If
        'End Select

        '  _udtAuditLogEntry.WriteEndLog(AuditLogDesc.MsgListLoadSuccess_ID, AuditLogDesc.MsgListLoadSuccess)
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][KarlL]
    End Sub

    Private Sub ClearSession()

        'Clear Session
        Session(SESS_PendingMessage) = Nothing
        Session.Remove(SESS_PendingMessage)
        Session(SESS_PendingMessageID) = Nothing
        Session.Remove(SESS_PendingMessageID)
        Session(SESS_PendingMessageTSMP) = Nothing
        Session.Remove(SESS_PendingMessageTSMP)

    End Sub

    Private Sub BindPendingMessageDetail(ByVal msgID As String)

        Dim dt As DataTable = New DataTable()
        Dim udtSentOutMessageModel As SentOutMessageModel = Nothing

        If Not IsNothing(msgID) Then
            _udtAuditLogEntry.AddDescripton("SentOutMessageID", msgID)
            _udtAuditLogEntry.WriteLog(AuditLogDesc.SelectMsg_ID, AuditLogDesc.SelectMsg)

            udtSentOutMessageModel = Me.udtSentOutMessageBLL.GetSentOutMessageBySentOutMsgID(msgID)
            ucMessageDetails.LoadMessage(udtSentOutMessageModel)
            Session(SESS_PendingMessageTSMP) = udtSentOutMessageModel.TSMP

            Select Case udtSentOutMessageModel.RecordStatus
                Case STATUS_Pending
                    'Change multiview index
                    Me.MultiViewSendMessageConfirmation.ActiveViewIndex = CurrentView.PendingMessageDetail
                    _udtAuditLogEntry.AddDescripton("SentOutMessageID", msgID)
                    _udtAuditLogEntry.WriteLog(AuditLogDesc.ViewMessage_ID, AuditLogDesc.ViewMessage)
                Case Else
                    'Show has been updated by other message
                    Dim udtSM3 As Common.ComObject.SystemMessage
                    udtSM3 = New SystemMessage("990000", SeverityCode.SEVI, MsgCode.MSG00022)
                    udcInfoMessageBox.AddMessage(udtSM3)
                    udcInfoMessageBox.BuildMessageBox()
                    udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information

                    'Clear session
                    ClearSession()

                    'Set view index to complete page
                    Me.MultiViewSendMessageConfirmation.ActiveViewIndex = CurrentView.CompletePendingMessage
                    _udtAuditLogEntry.AddDescripton("SentOutMessageID", msgID)
                    _udtAuditLogEntry.WriteLog(AuditLogDesc.FailUpdatePageShow_ID, AuditLogDesc.FailUpdatePageShow)
            End Select

        End If

    End Sub

#End Region

#Region "Event handler for View - View Pending Message"

#End Region

#Region "Event handler for View - View Pending Message Detail"


    Public Sub ibtnBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        _udtAuditLogEntry.AddDescripton("SentOutMessageID", CType(Session(SESS_PendingMessageID), String))
        _udtAuditLogEntry.WriteLog(AuditLogDesc.BackClick_ID, AuditLogDesc.BackClick)

        ClearSession()
        RetrievePendingMessage()
        Me.MultiViewSendMessageConfirmation.ActiveViewIndex = CurrentView.PendingMessage
    End Sub

    Public Sub ibtnApproveToSend_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        _udtAuditLogEntry.AddDescripton("SentOutMessageID", CType(Session(SESS_PendingMessageID), String))
        _udtAuditLogEntry.WriteLog(AuditLogDesc.ApproveClick_ID, AuditLogDesc.ApproveClick)

        popupApproveToSend.Show()

        _udtAuditLogEntry.AddDescripton("SentOutMessageID", CType(Session(SESS_PendingMessageID), String))
        _udtAuditLogEntry.WriteLog(AuditLogDesc.ConfirmApprovePopupDisplay_ID, AuditLogDesc.ConfirmApprovePopupDisplay)

    End Sub

    Public Sub ibtnReject_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        _udtAuditLogEntry.AddDescripton("SentOutMessageID", CType(Session(SESS_PendingMessageID), String))
        _udtAuditLogEntry.WriteLog(AuditLogDesc.RejectClick_ID, AuditLogDesc.RejectClick)

        udcRejectReasonMsgBox.Visible = False
        imgErrorReason.Visible = False
        popupReject.Show()

        _udtAuditLogEntry.AddDescripton("SentOutMessageID", CType(Session(SESS_PendingMessageID), String))
        _udtAuditLogEntry.WriteLog(AuditLogDesc.ConfirmRejectPopupDisplay_ID, AuditLogDesc.ConfirmRejectPopupDisplay)

    End Sub

#End Region

#Region "Event handler for View - View Complete Message Return"

    Public Sub ibtnCompletePendingMessageReturn_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        _udtAuditLogEntry.WriteLog(AuditLogDesc.ReturnClick_ID, AuditLogDesc.ReturnClick)

        RetrievePendingMessage()
        Me.MultiViewSendMessageConfirmation.ActiveViewIndex = CurrentView.PendingMessage
    End Sub

#End Region

#Region "Popup function - popupApproveToSend"

    Public Sub ibtnPopupConfirmToSend_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        _udtAuditLogEntry.AddDescripton("SentOutMessageID", CType(Session(SESS_PendingMessageID), String))
        _udtAuditLogEntry.WriteLog(AuditLogDesc.ConfirmApproveClick_ID, AuditLogDesc.ConfirmApproveClick)

        'Update record status
        Dim strUserID As String = udtHCVUUser.GetHCVUUser.UserID.Trim
        Dim byteTSMP As Byte() = CType(Session(SESS_PendingMessageTSMP), Byte())
        Dim blnRes As Boolean = False

        Try
            blnRes = Me.udtSentOutMessageBLL.UpdateSentOutMessageRecordStatusToReadyToSend(CType(Session(SESS_PendingMessageID), String), strUserID, byteTSMP)
            If blnRes = True Then
                'Show complete message
                Dim udtSM2 As Common.ComObject.SystemMessage
                udtSM2 = New SystemMessage(FUNCTION_CODE, SeverityCode.SEVI, MsgCode.MSG00002)
                udcInfoMessageBox.AddMessage(udtSM2)
                udcInfoMessageBox.BuildMessageBox()
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete

                _udtAuditLogEntry.AddDescripton("SentOutMessageID", CType(Session(SESS_PendingMessageID), String))
                _udtAuditLogEntry.WriteLog(AuditLogDesc.ConfirmApproveClickSuccess_ID, AuditLogDesc.ConfirmApproveClickSuccess)
            End If
        Catch ex As Exception
            'Show has been updated by other message
            Dim udtSM3 As Common.ComObject.SystemMessage
            udtSM3 = New SystemMessage("990000", SeverityCode.SEVI, MsgCode.MSG00022)
            udcInfoMessageBox.AddMessage(udtSM3)
            udcInfoMessageBox.BuildMessageBox()
            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information

            _udtAuditLogEntry.AddDescripton("SentOutMessageID", CType(Session(SESS_PendingMessageID), String))
            _udtAuditLogEntry.WriteLog(AuditLogDesc.ConfirmApproveClickFailByOthers_ID, AuditLogDesc.ConfirmApproveClickFailByOthers)
        End Try

        _udtAuditLogEntry.AddDescripton("SentOutMessageID", CType(Session(SESS_PendingMessageID), String))

        'Clear session
        ClearSession()

        'Set view index to complete page
        Me.MultiViewSendMessageConfirmation.ActiveViewIndex = CurrentView.CompletePendingMessage

        If blnRes = True Then
            _udtAuditLogEntry.WriteLog(AuditLogDesc.CompletePageShow_ID, AuditLogDesc.CompletePageShow)
        Else
            _udtAuditLogEntry.WriteLog(AuditLogDesc.FailUpdatePageShow_ID, AuditLogDesc.FailUpdatePageShow)
        End If

    End Sub

    Public Sub ibtnPopupConfirmToSendCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        _udtAuditLogEntry.AddDescripton("SentOutMessageID", CType(Session(SESS_PendingMessageID), String))
        _udtAuditLogEntry.WriteLog(AuditLogDesc.CancelApproveClick_ID, AuditLogDesc.CancelApproveClick)
        Me.popupApproveToSend.Hide()
    End Sub

#End Region

#Region "Popup function - popupReject"

    Public Sub ibtnConfirmReject_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        _udtAuditLogEntry.AddDescripton("SentOutMessageID", CType(Session(SESS_PendingMessageID), String))
        _udtAuditLogEntry.AddDescripton("RejectReason", Me.txtRejectReason.Text.Trim)
        _udtAuditLogEntry.WriteLog(AuditLogDesc.ConfirmRejectClick_ID, AuditLogDesc.ConfirmRejectClick)

        If udtValidator.IsEmpty(Me.txtRejectReason.Text.Trim) Then
            'No reject reason, raise message box
            Dim strUserID As String = udtHCVUUser.GetHCVUUser.UserID.Trim
            Dim udtSM As Common.ComObject.SystemMessage
            udtSM = New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00004)
            Me.udcRejectReasonMsgBox.AddMessage(udtSM)

            _udtAuditLogEntry.AddDescripton("SentOutMessageID", CType(Session(SESS_PendingMessageID), String))
            udcRejectReasonMsgBox.BuildMessageBox("ValidationFail", _udtAuditLogEntry, AuditLogDesc.ConfirmRejectClickFailNoReason, AuditLogDesc.ConfirmRejectClickFailNoReason_ID, strUserID)
            Me.imgErrorReason.Visible = True
            Me.popupReject.Show()

        Else
            'With reason, go to complete page
            'Wake up complete message box
            Dim strUserID As String = udtHCVUUser.GetHCVUUser.UserID.Trim
            Dim byteTSMP As Byte() = CType(Session(SESS_PendingMessageTSMP), Byte())
            Dim blnRes As Boolean = False

            Try
                blnRes = Me.udtSentOutMessageBLL.UpdateSentOutMessageRecordStatusToRejected(CType(Session(SESS_PendingMessageID), String), strUserID, Me.txtRejectReason.Text, byteTSMP)
                If blnRes = True Then
                    'Show complete message
                    Dim udtSM2 As Common.ComObject.SystemMessage
                    udtSM2 = New SystemMessage(FUNCTION_CODE, SeverityCode.SEVI, MsgCode.MSG00003)
                    udcInfoMessageBox.AddMessage(udtSM2)
                    udcInfoMessageBox.BuildMessageBox()
                    udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete

                    _udtAuditLogEntry.AddDescripton("SentOutMessageID", CType(Session(SESS_PendingMessageID), String))
                    _udtAuditLogEntry.AddDescripton("RejectReason", Me.txtRejectReason.Text.Trim)
                    _udtAuditLogEntry.WriteLog(AuditLogDesc.ConfirmRejectClickSuccess_ID, AuditLogDesc.ConfirmRejectClickSuccess)
                End If
            Catch ex As Exception
                'Show has been updated by other message
                Dim udtSM3 As Common.ComObject.SystemMessage
                udtSM3 = New SystemMessage("990000", SeverityCode.SEVI, MsgCode.MSG00022)
                udcInfoMessageBox.AddMessage(udtSM3)
                udcInfoMessageBox.BuildMessageBox()
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information

                _udtAuditLogEntry.AddDescripton("SentOutMessageID", CType(Session(SESS_PendingMessageID), String))
                _udtAuditLogEntry.WriteLog(AuditLogDesc.ConfirmRejectClickFailByOthers_ID, AuditLogDesc.ConfirmRejectClickFailByOthers)
            End Try

            Me.txtRejectReason.Text = ""
            Me.imgErrorReason.Visible = False

            _udtAuditLogEntry.AddDescripton("SentOutMessageID", CType(Session(SESS_PendingMessageID), String))

            'Clear session
            ClearSession()

            Me.MultiViewSendMessageConfirmation.ActiveViewIndex = CurrentView.CompletePendingMessage

            If blnRes = True Then
                _udtAuditLogEntry.WriteLog(AuditLogDesc.RejectPageShow_ID, AuditLogDesc.RejectPageShow)
            Else
                _udtAuditLogEntry.WriteLog(AuditLogDesc.FailUpdatePageShow_ID, AuditLogDesc.FailUpdatePageShow)
            End If

        End If

    End Sub

    Public Sub ibtnConfirmRejectCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        _udtAuditLogEntry.AddDescripton("SentOutMessageID", CType(Session(SESS_PendingMessageID), String))
        _udtAuditLogEntry.WriteLog(AuditLogDesc.CancelRejectClick_ID, AuditLogDesc.CancelRejectClick)

        Me.txtRejectReason.Text = ""
        Me.imgErrorReason.Visible = False
        Me.popupReject.Hide()
    End Sub

#End Region

#Region "Event handler for GridView - gvPendMessage"

    Protected Sub gvPendingMessage_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.gvPendingMessage.SelectedIndex = -1
        Me.GridViewPageIndexChangingHandler(sender, e, SESS_PendingMessage)
    End Sub

    Protected Sub gvPendingMessage_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.GridViewPreRenderHandler(sender, e, SESS_PendingMessage)
    End Sub

    Protected Sub gvPendingMessage_RowCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        'Write code for link button
        If TypeOf e.CommandSource Is LinkButton Then

            'Get command argument value (link button)
            Dim strCommandArgument As String
            strCommandArgument = e.CommandArgument
            Session(SESS_PendingMessageID) = strCommandArgument
            Me.BindPendingMessageDetail(Session(SESS_PendingMessageID))

        End If
    End Sub

    Protected Sub gvPendingMessage_RowCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        'Nothing
    End Sub

    Protected Sub gvPendingMessage_RowDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        'Write code for datarow
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRow = CType(e.Row.DataItem, System.Data.DataRowView).Row
            Dim lbtnMessageID As LinkButton

            lbtnMessageID = CType(e.Row.FindControl("lbtnMessageID"), LinkButton)

            Dim strMessageID = CStr(dr.Item("SOMS_SentOutMsg_ID"))
            lbtnMessageID.Text = strMessageID.Trim
            'set command argument
            lbtnMessageID.CommandArgument = strMessageID.Trim

            'Set category full text
            Dim lblCategory As Label = CType(e.Row.FindControl("lblCategory"), Label)
            Dim strMsgTemplateCategoryDisplayText As String = ""
            Status.GetDescriptionFromDBCode(MessageTemplateModel.STATUS_DATA_CLASS, lblCategory.Text.Trim(), strMsgTemplateCategoryDisplayText, String.Empty)
            lblCategory.Text = strMsgTemplateCategoryDisplayText

            'Set creation time, use formatter convertDateTime method
            Dim lblCreationTime As Label = CType(e.Row.FindControl("lblCreationTime"), Label)
            lblCreationTime.Text = udtFormatter.convertDateTime(lblCreationTime.Text)

        End If
    End Sub

    Protected Sub gvPendingMessage_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        Me.gvPendingMessage.SelectedIndex = -1
        Me.GridViewSortingHandler(sender, e, SESS_PendingMessage)
    End Sub

#End Region

#Region "Must override function - Master page"

    Public Overrides Function GetDocCode() As String
        Return Nothing
    End Function

    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Return Nothing
    End Function

    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        Return Nothing
    End Function

#End Region

End Class