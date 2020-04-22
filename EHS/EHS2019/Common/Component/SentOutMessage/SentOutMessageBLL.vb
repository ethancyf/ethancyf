' [CRE12-012] Infrastructure on Sending Messages through eHealth System Inbox

Imports System.Data
Imports System.Data.SqlClient
Imports Common.ComObject
Imports Common.DataAccess
Imports Common.Component
Imports Common.Component.StaticData
Imports Common.ComFunction.ParameterFunction

Namespace Component.SentOutMessage

    Public Class SentOutMessageBLL

#Region "DB Stored Procedure List"
        ' Sent Out Message Creation
        Private Const SP_LIST_ACTIVE_MSG_TEMPLATE As String = "proc_InboxMsgTemplate_IBMT_List"
        Private Const SP_GET_MSG_TEMPLATE As String = "proc_InboxMsgTemplate_IBMT_Get"
        Private Const SP_GET_MSG_PARAM As String = "proc_InboxMsgParameterSetup_IBMP_Get"
        Private Const SP_ADD_SO_MSG_RECIPIENT As String = "proc_SentOutMsgRecipient_SOMR_Add"
        Private Const SP_ADD_SO_MSG As String = "proc_SentOutMsg_SOMS_Add"

        ' Sent Out Message Approval
        Private Const SP_GET_ROW_COUNT_BY_RECORD_STATUS As String = "proc_SentOutMsg_SOMS_GetRowCount_ByRecordStatus"
        Private Const strSP_GetSentOutMessageBySentOutMsgID As String = "proc_SentOutMsg_SOMS_get_bySentOutMsgID"
        Private Const strSP_GetSentOutMessageByRecordStatus As String = "proc_SentOutMsg_SOMS_get_byRecordStatus"
        Private Const strSP_GetSentOutMessageRecipientBySentOutMsgID As String = "proc_SentOutMsgRecipient_SOMR_get_bySentOutMsgID"
        Private Const strSP_UpdateSentOutMessageRecordStatusToRejected As String = "proc_SentOutMsg_SOMS_upd_RecordStatusRejected"
        Private Const strSP_UpdateSentOutMessageRecordStatusToReadyToSend As String = "proc_SentOutMsg_SOMS_upd_RecordStatusReadyToSend"
        Private Const strSP_GetSentOutMessageByUserRecordStatus As String = "proc_SentOutMsg_SOMS_get_byUserRecordStatus"
        Private Const strSP_GetSentOutMessageOutboxCountByUser As String = "proc_SentOutMsg_SOMS_get_OutBoxCountByUser"

        ' Sent Out Message Schedule Job (Send Message to Service Provider)
        Private Const SP_GET_MSG_ID_BY_RECORD_STATUS As String = "proc_SentOutMsg_SOMS_GetID_ByRecordStatus"
        Private Const SP_SEND_MSG As String = "proc_SentOutMsg_SOMS_SendMsg"
#End Region

#Region "DB Table Field Schema - [InboxMsgTemplate_IBMT]"
        Public Class Table_InboxMsgTemplate_IBMT
            Public Const IBMT_MsgTemplate_ID As String = "IBMT_MsgTemplate_ID"
            Public Const IBMT_MsgTemplateSubject As String = "IBMT_MsgTemplateSubject"
            Public Const IBMT_MsgTemplateContent As String = "IBMT_MsgTemplateContent"
            Public Const IBMT_MsgTemplateCategory As String = "IBMT_MsgTemplateCategory"
            Public Const IBMT_Record_Status As String = "IBMT_Record_Status"
            Public Const IBMT_Create_Dtm As String = "IBMT_Create_Dtm"
        End Class
#End Region

#Region "DB Table Field Schema - [InboxMsgParameterSetup_IBMP]"
        Public Class Table_InboxMsgParameterSetup_IBMP
            Public Const IBMP_MsgTemplate_ID As String = "IBMP_MsgTemplate_ID"
            Public Const IBMP_MsgParameter_ID As String = "IBMP_MsgParameter_ID"
            Public Const IBMP_MsgParameterType As String = "IBMP_MsgParameterType"
            Public Const IBMP_MsgParameterArgument As String = "IBMP_MsgParameterArgument"
            Public Const IBMP_MsgParameterDisplayName As String = "IBMP_MsgParameterDisplayName"
        End Class
#End Region

#Region "DB Table Field Schema - [SentOutMsg_SOMS]"
        Public Class Table_SentOutMsg_SOMS
            Public Const SOMS_SentOutMsg_ID As String = "SOMS_SentOutMsg_ID"
            Public Const SOMS_Record_Status As String = "SOMS_Record_Status"
            Public Const SOMS_SentOutMsgSubject As String = "SOMS_SentOutMsgSubject"
            Public Const SOMS_SentOutMsgContent As String = "SOMS_SentOutMsgContent"
            Public Const SOMS_SentOutMsgCategory As String = "SOMS_SentOutMsgCategory"
            Public Const SOMS_Create_By As String = "SOMS_Create_By"
            Public Const SOMS_Create_Dtm As String = "SOMS_Create_Dtm"
            Public Const SOMS_Confirm_By As String = "SOMS_Confirm_By"
            Public Const SOMS_Confirm_Dtm As String = "SOMS_Confirm_Dtm"
            Public Const SOMS_Reject_By As String = "SOMS_Reject_By"
            Public Const SOMS_Reject_Dtm As String = "SOMS_Reject_Dtm"
            Public Const SOMS_Reject_Reason As String = "SOMS_Reject_Reason"
            Public Const SOMS_Sent_Dtm As String = "SOMS_Sent_Dtm"
            Public Const SOMS_Message_ID As String = "SOMS_Message_ID"
            Public Const SOMS_TSMP As String = "SOMS_TSMP"
        End Class
#End Region

#Region "DB Table Field Schema - [SentOutMsgRecipient_SOMR]"
        Public Class Table_SentOutMsgRecipient_SOMR
            Public Const SOMR_SentOutMsg_ID As String = "SOMR_SentOutMsg_ID"
            Public Const SOMR_Profession As String = "SOMR_Profession"
            Public Const SOMR_Scheme As String = "SOMR_Scheme"
        End Class
#End Region

#Region "Private Member"
        Private udtDB As New Database()
#End Region

#Region "Constructor"
        Public Sub New()
        End Sub
#End Region

#Region "Method for Sent Out Message Creation"
        ' Get all active Message Template for Data Binding
        Public Function GetAllActiveMsgTemplate() As DataTable
            Dim dt As DataTable

            If udtDB Is Nothing Then udtDB = New Database()

            dt = New DataTable()

            Try

                udtDB.RunProc(SP_LIST_ACTIVE_MSG_TEMPLATE, dt)

            Catch ex As Exception

                Throw ex

            End Try

            Return dt
        End Function

        ' Get a Message Template by Message Template ID
        Public Function GetMsgTemplate(ByVal strMsgTemplateID As String) As MessageTemplateModel
            Dim udtMessageTemplateModel As MessageTemplateModel = Nothing
            Dim udtMessageParameterModelCollection As MessageParameterModelCollection
            Dim dt As DataTable

            If udtDB Is Nothing Then udtDB = New Database()

            dt = New DataTable()

            Try

                Dim params() As SqlParameter = {udtDB.MakeInParam("@msg_template_id", MessageTemplateModel.DATA_TYPE_MSG_TEMPLATE_ID, MessageTemplateModel.DATA_SIZE_MSG_TEMPLATE_ID, strMsgTemplateID)}

                udtDB.RunProc(SP_GET_MSG_TEMPLATE, params, dt)
                If dt.Rows.Count = 1 Then
                    udtMessageParameterModelCollection = GetMsgParameters(strMsgTemplateID)

                    udtMessageTemplateModel = New MessageTemplateModel( _
                    CStr(dt.Rows(0).Item(Table_InboxMsgTemplate_IBMT.IBMT_MsgTemplate_ID)).Trim(), _
                    CStr(dt.Rows(0).Item(Table_InboxMsgTemplate_IBMT.IBMT_MsgTemplateSubject)).Trim(), _
                    CStr(dt.Rows(0).Item(Table_InboxMsgTemplate_IBMT.IBMT_MsgTemplateContent)).Trim(), _
                    CStr(dt.Rows(0).Item(Table_InboxMsgTemplate_IBMT.IBMT_MsgTemplateCategory)).Trim(), _
                    CType(dt.Rows(0).Item(Table_InboxMsgTemplate_IBMT.IBMT_Record_Status), Char), _
                    CType(dt.Rows(0).Item(Table_InboxMsgTemplate_IBMT.IBMT_Create_Dtm), DateTime), _
                    udtMessageParameterModelCollection)

                End If

            Catch ex As Exception

                Throw

            End Try

            Return udtMessageTemplateModel
        End Function

        ' Get all Message Parameter by Message Template ID
        Public Function GetMsgParameters(ByVal strMsgTemplateID As String) As MessageParameterModelCollection
            Dim udtMessageParameterModelCollection As MessageParameterModelCollection = Nothing
            Dim udtMessageParameterModel As MessageParameterModel
            Dim dt As DataTable

            If udtDB Is Nothing Then udtDB = New Database()

            dt = New DataTable()

            Try

                Dim params() As SqlParameter = {udtDB.MakeInParam("@msg_template_id", MessageParameterModel.DATA_TYPE_MSG_TEMPLATE_ID, MessageParameterModel.DATA_SIZE_MSG_TEMPLATE_ID, strMsgTemplateID)}

                udtDB.RunProc(SP_GET_MSG_PARAM, params, dt)
                If dt.Rows.Count > 0 Then
                    udtMessageParameterModelCollection = New MessageParameterModelCollection()

                    For Each dr As DataRow In dt.Rows

                        udtMessageParameterModel = New MessageParameterModel( _
                        CStr(dr.Item(Table_InboxMsgParameterSetup_IBMP.IBMP_MsgTemplate_ID)).Trim(), _
                        CStr(dr.Item(Table_InboxMsgParameterSetup_IBMP.IBMP_MsgParameter_ID)).Trim(), _
                        CStr(dr.Item(Table_InboxMsgParameterSetup_IBMP.IBMP_MsgParameterType)).Trim(), _
                        IIf(dr.Item(Table_InboxMsgParameterSetup_IBMP.IBMP_MsgParameterArgument) Is DBNull.Value, Nothing, CStr(dr.Item(Table_InboxMsgParameterSetup_IBMP.IBMP_MsgParameterArgument)).Trim()), _
                        CStr(dr.Item(Table_InboxMsgParameterSetup_IBMP.IBMP_MsgParameterDisplayName)).Trim())

                        udtMessageParameterModelCollection.Add(udtMessageParameterModel)

                    Next
                End If

            Catch ex As Exception

                Throw

            End Try

            Return udtMessageParameterModelCollection
        End Function

        ' Write the created [SentOutMessageModel] into DB
        Public Sub WriteSentOutMsgToDB(ByVal udtSentOutMessageModel As SentOutMessageModel)
            If udtDB Is Nothing Then udtDB = New Database()

            Try

                Dim params() As SqlParameter = {udtDB.MakeInParam("@sent_out_msg_id", SentOutMessageModel.DATA_TYPE_SO_MSG_ID, SentOutMessageModel.DATA_SIZE_SO_MSG_ID, udtSentOutMessageModel.SentOutMsgID), _
                                                udtDB.MakeInParam("@record_status", SentOutMessageModel.DATA_TYPE_RECORD_STATUS, SentOutMessageModel.DATA_SIZE_RECORD_STATUS, udtSentOutMessageModel.RecordStatus), _
                                                udtDB.MakeInParam("@sent_out_msg_subject", SentOutMessageModel.DATA_TYPE_SO_MSG_SUBJECT, SentOutMessageModel.DATA_SIZE_SO_MSG_SUBJECT, udtSentOutMessageModel.SentOutMsgSubject), _
                                                udtDB.MakeInParam("@sent_out_msg_content", SentOutMessageModel.DATA_TYPE_SO_MSG_CONTENT, SentOutMessageModel.DATA_SIZE_SO_MSG_CONTENT, udtSentOutMessageModel.SentOutMsgContent), _
                                                udtDB.MakeInParam("@sent_out_msg_category", SentOutMessageModel.DATA_TYPE_SO_MSG_CATEGORY, SentOutMessageModel.DATA_SIZE_SO_MSG_CATEGORY, udtSentOutMessageModel.SentOutMsgCategory), _
                                                udtDB.MakeInParam("@create_by", SentOutMessageModel.DATA_TYPE_CREATE_BY, SentOutMessageModel.DATA_SIZE_CREATE_BY, udtSentOutMessageModel.CreateBy)}

                udtDB.BeginTransaction()

                udtDB.RunProc(SP_ADD_SO_MSG, params)

                WriteSentOutMsgRecipientsToDB(udtSentOutMessageModel.SentOutMsgRecipients, udtSentOutMessageModel.CreateBy)

                udtDB.CommitTransaction()

            Catch ex As Exception

                udtDB.RollBackTranscation()
                Throw

            End Try
        End Sub

        ' Write the [SentOutMsgRecipientModelCollection] into DB
        Private Sub WriteSentOutMsgRecipientsToDB(ByVal udtSentOutMsgRecipientModelCollection As SentOutMsgRecipientModelCollection, ByVal strCreateBy As String)
            Dim params(4) As SqlParameter
            Dim udtSentOutMsgRecipientModel As SentOutMsgRecipientModel

            If udtDB Is Nothing Then udtDB = New Database()

            Try

                For Each udtSentOutMsgRecipientModel In udtSentOutMsgRecipientModelCollection

                    params(0) = udtDB.MakeInParam("@sent_out_msg_id", SentOutMsgRecipientModel.DATA_TYPE_SO_MSG_ID, SentOutMsgRecipientModel.DATA_SIZE_SO_MSG_ID, udtSentOutMsgRecipientModel.SentOutMsgID)
                    params(1) = udtDB.MakeInParam("@profession", SentOutMsgRecipientModel.DATA_TYPE_PROFESSION, SentOutMsgRecipientModel.DATA_SIZE_PROFESSION, udtSentOutMsgRecipientModel.Profession)
                    params(2) = udtDB.MakeInParam("@scheme", SentOutMsgRecipientModel.DATA_TYPE_SCHEME, SentOutMsgRecipientModel.DATA_SIZE_SCHEME, udtSentOutMsgRecipientModel.Scheme)
                    params(3) = udtDB.MakeInParam("@create_by", SentOutMessageModel.DATA_TYPE_CREATE_BY, SentOutMessageModel.DATA_SIZE_CREATE_BY, strCreateBy)

                    udtDB.RunProc(SP_ADD_SO_MSG_RECIPIENT, params)

                Next

            Catch ex As Exception

                Throw

            End Try
        End Sub
#End Region

#Region "Method for Sent Out Message Approval"
        ' Get the Row Count of Table - [SentOutMsg_SOMS] by the Field - [SOMS_Record_Status]
        Public Function GetSentOutMsgRowCountByRecordStatus(ByVal chrRecordStatus As Char) As Integer
            Dim dt As DataTable

            If udtDB Is Nothing Then udtDB = New Database()

            dt = New DataTable()

            Try

                Dim params() As SqlParameter = {udtDB.MakeInParam("@record_status", SentOutMessageModel.DATA_TYPE_RECORD_STATUS, SentOutMessageModel.DATA_SIZE_RECORD_STATUS, chrRecordStatus)}

                udtDB.RunProc(SP_GET_ROW_COUNT_BY_RECORD_STATUS, params, dt)

                If dt.Rows.Count = 1 Then

                    For Each dr As DataRow In dt.Rows

                        Return CInt(dr.Item(0))

                    Next

                End If

            Catch ex As Exception

                Throw

            End Try
        End Function
        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Karl L]
        ' -------------------------------------------------------------------------
        Public Function GetSentOutMessageByRecordStatus(ByVal strFunctionCode As String, ByVal blnOverrideResultLimit As Boolean, ByVal strStatus As String) As BaseBLL.BLLSearchResult
            Dim udtBLLSearchResult As BaseBLL.BLLSearchResult

            Dim parms() As SqlParameter = { _
                      udtDB.MakeInParam("@record_status", SqlDbType.Char, 1, strStatus)}

            udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, strSP_GetSentOutMessageByRecordStatus, parms, blnOverrideResultLimit, udtDB)

            If udtBLLSearchResult.SqlErrorMessage = BaseBLL.EnumSqlErrorMessage.Normal Then
                Return udtBLLSearchResult
            Else
                udtBLLSearchResult.Data = Nothing
                Return udtBLLSearchResult
            End If

        End Function

        'Public Function GetSentOutMessageByRecordStatus(ByVal strStatus As String) As DataTable
        '    Dim dtRes As DataTable = New DataTable

        '    Try
        '        Dim parms() As SqlParameter = { _
        '                    udtDB.MakeInParam("@record_status", SqlDbType.Char, 1, strStatus)}
        '        udtDB.RunProc(strSP_GetSentOutMessageByRecordStatus, parms, dtRes)
        '    Catch ex As Exception
        '        dtRes = Nothing
        '        Throw
        '    End Try
        'End Function
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Karl L]

        Public Function GetSentOutMessageByUserRecordStatus(ByVal strStatus As String, ByVal strCreateBy As String) As DataTable
            Dim dtRes As DataTable = New DataTable
            Try
                Dim parms() As SqlParameter = { _
                            udtDB.MakeInParam("@record_status", SqlDbType.Char, 1, strStatus), _
                            udtDB.MakeInParam("@create_by", SqlDbType.VarChar, 20, strCreateBy)}
                udtDB.RunProc(strSP_GetSentOutMessageByUserRecordStatus, parms, dtRes)
            Catch ex As Exception
                dtRes = Nothing
                Throw
            End Try

            Return dtRes
        End Function

        Public Function GetSentOutMessageBySentOutMsgID(ByVal strMsgID As String) As SentOutMessageModel
            Dim udtSentOutMessageModel As SentOutMessageModel = Nothing
            Dim udtSentOutMsgRecipientModelCollection As SentOutMsgRecipientModelCollection = Nothing
            Dim dtRes As DataTable = New DataTable
            Try
                Dim parms() As SqlParameter = { _
                            udtDB.MakeInParam("@sent_out_msg_id", SqlDbType.VarChar, 15, strMsgID)}
                udtDB.RunProc(strSP_GetSentOutMessageBySentOutMsgID, parms, dtRes)

                If dtRes.Rows.Count = 1 Then
                    udtSentOutMsgRecipientModelCollection = GetSentOutMessageRecipientBySentOutMsgID(strMsgID)

                    Dim strTempConfirmBy As String = String.Empty
                    Dim strTempConfirmDtm As New DateTime
                    Dim strTempRejectBy As String = String.Empty
                    Dim strTempRejectDtm As New DateTime
                    Dim strTempRejectReason As String = String.Empty
                    Dim strTempSentDtm As New DateTime
                    Dim strTempMessageID As String = String.Empty
                    Dim byteTSMP As Byte()

                    'Confirm By
                    If Not IsDBNull(dtRes.Rows(0).Item(Table_SentOutMsg_SOMS.SOMS_Confirm_By)) Then
                        strTempConfirmBy = dtRes.Rows(0).Item(Table_SentOutMsg_SOMS.SOMS_Confirm_By)
                    Else
                        strTempConfirmBy = String.Empty
                    End If

                    'Confirm Dtm
                    If Not IsDBNull(dtRes.Rows(0).Item(Table_SentOutMsg_SOMS.SOMS_Confirm_Dtm)) Then
                        strTempConfirmDtm = dtRes.Rows(0).Item(Table_SentOutMsg_SOMS.SOMS_Confirm_Dtm)
                    Else
                        strTempConfirmDtm = New DateTime
                    End If

                    'Reject By
                    If Not IsDBNull(dtRes.Rows(0).Item(Table_SentOutMsg_SOMS.SOMS_Reject_By)) Then
                        strTempRejectBy = dtRes.Rows(0).Item(Table_SentOutMsg_SOMS.SOMS_Reject_By)
                    Else
                        strTempRejectBy = String.Empty
                    End If

                    'Reject Dtm
                    If Not IsDBNull(dtRes.Rows(0).Item(Table_SentOutMsg_SOMS.SOMS_Reject_Dtm)) Then
                        strTempRejectDtm = dtRes.Rows(0).Item(Table_SentOutMsg_SOMS.SOMS_Reject_Dtm)
                    Else
                        strTempRejectDtm = New DateTime
                    End If

                    'Reject Reason
                    If Not IsDBNull(dtRes.Rows(0).Item(Table_SentOutMsg_SOMS.SOMS_Reject_Reason)) Then
                        strTempRejectReason = dtRes.Rows(0).Item(Table_SentOutMsg_SOMS.SOMS_Reject_Reason)
                    Else
                        strTempRejectReason = String.Empty
                    End If

                    'Sent Dtm
                    If Not IsDBNull(dtRes.Rows(0).Item(Table_SentOutMsg_SOMS.SOMS_Sent_Dtm)) Then
                        strTempSentDtm = dtRes.Rows(0).Item(Table_SentOutMsg_SOMS.SOMS_Sent_Dtm)
                    Else
                        strTempSentDtm = New DateTime
                    End If

                    'Message ID
                    If Not IsDBNull(dtRes.Rows(0).Item(Table_SentOutMsg_SOMS.SOMS_Message_ID)) Then
                        strTempMessageID = dtRes.Rows(0).Item(Table_SentOutMsg_SOMS.SOMS_Message_ID)
                    Else
                        strTempMessageID = String.Empty
                    End If

                    'Timestamp
                    If Not IsDBNull(dtRes.Rows(0).Item(Table_SentOutMsg_SOMS.SOMS_TSMP)) Then
                        byteTSMP = dtRes.Rows(0).Item(Table_SentOutMsg_SOMS.SOMS_TSMP)
                    Else
                        byteTSMP = Nothing
                    End If

                    udtSentOutMessageModel = New SentOutMessageModel( _
                    CStr(dtRes.Rows(0).Item(Table_SentOutMsg_SOMS.SOMS_SentOutMsg_ID)).Trim(), _
                    CType(dtRes.Rows(0).Item(Table_SentOutMsg_SOMS.SOMS_Record_Status), Char), _
                    CStr(dtRes.Rows(0).Item(Table_SentOutMsg_SOMS.SOMS_SentOutMsgSubject)).Trim(), _
                    CStr(dtRes.Rows(0).Item(Table_SentOutMsg_SOMS.SOMS_SentOutMsgContent)).Trim(), _
                    CStr(dtRes.Rows(0).Item(Table_SentOutMsg_SOMS.SOMS_SentOutMsgCategory)).Trim(), _
                    CStr(dtRes.Rows(0).Item(Table_SentOutMsg_SOMS.SOMS_Create_By)).Trim(), _
                    CType(dtRes.Rows(0).Item(Table_SentOutMsg_SOMS.SOMS_Create_Dtm), DateTime), _
                    DirectCast(strTempConfirmBy, String).Trim(), _
                    DirectCast(strTempConfirmDtm, DateTime), _
                    DirectCast(strTempRejectBy, String).Trim(), _
                    DirectCast(strTempRejectDtm, DateTime), _
                    DirectCast(strTempRejectReason, String).Trim(), _
                    DirectCast(strTempSentDtm, DateTime), _
                    DirectCast(strTempMessageID, String).Trim(), _
                    CType(byteTSMP, Byte()), _
                    udtSentOutMsgRecipientModelCollection)
                End If

            Catch ex As Exception
                Throw
            End Try

            Return udtSentOutMessageModel
        End Function

        Public Function GetSentOutMessageRecipientBySentOutMsgID(ByVal strMsgID As String) As SentOutMsgRecipientModelCollection
            Dim udtSentOutMsgRecipientModelCollection As SentOutMsgRecipientModelCollection = Nothing
            Dim udtSentOutMsgRecipientModel As SentOutMsgRecipientModel = Nothing
            Dim dtRes As DataTable = New DataTable
            Try
                Dim parms() As SqlParameter = { _
                            udtDB.MakeInParam("@sent_out_msg_id", SqlDbType.VarChar, 15, strMsgID)}
                udtDB.RunProc(strSP_GetSentOutMessageRecipientBySentOutMsgID, parms, dtRes)

                If dtRes.Rows.Count > 0 Then
                    udtSentOutMsgRecipientModelCollection = New SentOutMsgRecipientModelCollection
                    For Each dr As DataRow In dtRes.Rows
                        udtSentOutMsgRecipientModel = New SentOutMsgRecipientModel( _
                        CStr(dr.Item(Table_SentOutMsgRecipient_SOMR.SOMR_SentOutMsg_ID)).Trim(), _
                        CStr(dr.Item(Table_SentOutMsgRecipient_SOMR.SOMR_Profession)).Trim(), _
                        CStr(dr.Item(Table_SentOutMsgRecipient_SOMR.SOMR_Scheme)).Trim())
                        udtSentOutMsgRecipientModelCollection.Add(udtSentOutMsgRecipientModel)
                    Next
                End If
            Catch ex As Exception
                Throw
            End Try

            Return udtSentOutMsgRecipientModelCollection
        End Function

        Public Function UpdateSentOutMessageRecordStatusToRejected(ByVal strMsgID As String, ByVal strRejectBy As String, ByVal strRejectReason As String, ByVal TSMP As Byte()) As Boolean
            Dim blnRes As Boolean = False
            Try
                Dim parms() As SqlParameter = { _
                            udtDB.MakeInParam("@sent_out_msg_id", SqlDbType.VarChar, 15, strMsgID), _
                            udtDB.MakeInParam("@reject_by", SqlDbType.VarChar, 20, strRejectBy), _
                            udtDB.MakeInParam("@reject_reason", SqlDbType.NVarChar, 1000, strRejectReason), _
                            udtDB.MakeInParam("@tsmp", SqlDbType.Timestamp, 8, TSMP)}
                udtDB.RunProc(strSP_UpdateSentOutMessageRecordStatusToRejected, parms)
                blnRes = True
            Catch ex As Exception
                blnRes = False
                Throw
            End Try

            Return True
        End Function

        Public Function UpdateSentOutMessageRecordStatusToReadyToSend(ByVal strMsgID As String, ByVal strConfirmBy As String, ByVal TSMP As Byte()) As Boolean
            Dim blnRes As Boolean = False
            Try
                Dim parms() As SqlParameter = { _
                            udtDB.MakeInParam("@sent_out_msg_id", SqlDbType.VarChar, 15, strMsgID), _
                            udtDB.MakeInParam("@confirm_by", SqlDbType.VarChar, 20, strConfirmBy), _
                            udtDB.MakeInParam("@tsmp", SqlDbType.Timestamp, 8, TSMP)}
                udtDB.RunProc(strSP_UpdateSentOutMessageRecordStatusToReadyToSend, parms)
                blnRes = True
            Catch ex As Exception
                blnRes = False
                Throw
            End Try

            Return True
        End Function

        Public Function GetSentOutMessageOutboxCountByUser(ByVal strCreateBy As String) As Integer
            Dim dtRes As DataTable = New DataTable
            Try
                Dim parms() As SqlParameter = { _
                                            udtDB.MakeInParam("@create_by", SqlDbType.VarChar, 20, strCreateBy)}
                udtDB.RunProc(strSP_GetSentOutMessageOutboxCountByUser, parms, dtRes)
                Return CType(dtRes.Rows(0)(0), Integer)
            Catch ex As Exception
                Throw
            End Try

        End Function

#End Region

#Region "Method for Sent Out Message Schedule Job"
        ' Get all Sent Out Message ID of Table - [SentOutMsg_SOMS] by the Field - [SOMS_Record_Status]
        Public Function GetSentOutMsgIDByRecordStatus(ByVal chrRecordStatus As Char) As ArrayList
            Dim strSentOutMsgIDList As ArrayList = New ArrayList()
            Dim dt As DataTable

            If udtDB Is Nothing Then udtDB = New Database()

            dt = New DataTable()

            Try

                Dim params() As SqlParameter = {udtDB.MakeInParam("@record_status", SentOutMessageModel.DATA_TYPE_RECORD_STATUS, SentOutMessageModel.DATA_SIZE_RECORD_STATUS, chrRecordStatus)}

                udtDB.RunProc(SP_GET_MSG_ID_BY_RECORD_STATUS, params, dt)

                If dt.Rows.Count > 0 Then

                    For Each dr As DataRow In dt.Rows

                        strSentOutMsgIDList.Add(CStr(dr.Item(Table_SentOutMsg_SOMS.SOMS_SentOutMsg_ID)).Trim())

                    Next

                End If

            Catch ex As Exception

                Throw

            End Try

            Return strSentOutMsgIDList
        End Function

        ' Send the Message to Service Provider
        Public Function SendMessage(ByVal strSentOutMsgID As String, ByVal strInboxMsgID As String) As Boolean
            If udtDB Is Nothing Then udtDB = New Database()

            Try

                Dim params() As SqlParameter = {udtDB.MakeInParam("@sent_out_msg_id", SentOutMessageModel.DATA_TYPE_SO_MSG_ID, SentOutMessageModel.DATA_SIZE_SO_MSG_ID, strSentOutMsgID), _
                                                udtDB.MakeInParam("@inbox_msg_id", SqlDbType.Char, 12, strInboxMsgID)}

                udtDB.BeginTransaction()

                udtDB.RunProc(SP_SEND_MSG, params)

                udtDB.CommitTransaction()

            Catch ex As Exception

                udtDB.RollBackTranscation()
                Throw

            End Try

            Return True
        End Function
#End Region

    End Class

End Namespace
