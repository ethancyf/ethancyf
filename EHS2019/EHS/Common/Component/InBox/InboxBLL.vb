Imports System.Data
Imports System.Data.SqlClient

Imports Common.DataAccess
Imports Common.ComFunction.ParameterFunction

Namespace Component.Inbox

    ''' <summary>
    ''' Inbox BLL To Handle [dbo].[Message] And [dbo].[MessageReader]
    ''' </summary>
    ''' <remarks></remarks>
    Public Class InboxBLL

        Private udcGeneralFunct As New Common.ComFunction.GeneralFunction()
        Private udtDB As New Database()

        Public Sub New()
        End Sub

        Public Function GetInboxMessageByStatusID(ByVal strStatus As String, ByVal strMsgReader As String) As DataTable

            Dim dt As New DataTable

            Try
                ' create data object and params
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@msg_status", SqlDbType.Char, 1, strStatus), _
                                                udtDB.MakeInParam("@message_reader", SqlDbType.VarChar, 20, strMsgReader)}

                ' run the stored procedure
                udtDB.RunProc("proc_InboxMessage_get_byStatusID", prams, dt)
                Return dt
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GetInboxMessageByMessageID(ByVal strMsgID As String) As DataTable

            Dim dt As New DataTable

            Try
                ' create data object and params
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@message_id", SqlDbType.VarChar, 20, strMsgID)}

                ' run the stored procedure
                udtDB.RunProc("proc_InboxMessage_get_byMessageID", prams, dt)
                Return dt
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GetNewMessageCount(ByVal strMsgReader As String) As Integer

            Dim dt As New DataTable

            Try
                ' create data object and params
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@message_reader", SqlDbType.VarChar, 20, strMsgReader)}

                ' run the stored procedure
                udtDB.RunProc("proc_InboxMessage_get_NewMessageCount", prams, dt)
                Return CType(dt.Rows(0)(0), Integer)
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function UpdateMessageStatus(ByVal strMessageID As String, ByVal strMsgReader As String, ByVal strStatus As String, ByVal dtmUpdateTime As DateTime) As Boolean

            Try
                ' create data object and params
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@message_id", SqlDbType.Char, 12, strMessageID), _
                                                udtDB.MakeInParam("@message_reader", SqlDbType.VarChar, 20, strMsgReader), _
                                                udtDB.MakeInParam("@record_status", SqlDbType.Char, 1, strStatus), _
                                                udtDB.MakeInParam("@update_by", SqlDbType.VarChar, 20, strMsgReader), _
                                                udtDB.MakeInParam("@update_dtm", SqlDbType.DateTime, 8, dtmUpdateTime)}

                ' run the stored procedure
                udtDB.RunProc("proc_MessageReader_upd_Status", prams)
                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        ''' <summary>
        ''' Call this function to add a new message to the user
        ''' </summary>
        ''' <param name="strMessageReader">Message Recipient ID</param>
        ''' <param name="strUserID">Sender ID</param>
        ''' <param name="strSubject">Message Subject</param>
        ''' <param name="strContent">Message Content</param>
        ''' <returns>True for successful create mail; Fail for unsuccessful case</returns>
        ''' <remarks></remarks>
        Public Function AddNewMessage(ByVal strMessageReader As String, ByVal strUserID As String, ByVal strSubject As String, ByVal strContent As String) As Boolean
            Dim bResult As Boolean = False
            Dim db As New Database
            Dim strMessageID As String
            Dim dtmUpdateTime As DateTime

            Try
                dtmUpdateTime = udcGeneralFunct.GetSystemDateTime
                strMessageID = udcGeneralFunct.generateInboxMsgID()

                udtDB.BeginTransaction()

                AddMessage(db, strMessageID, strSubject, strContent, strUserID, dtmUpdateTime)
                AddMessageReader(db, strMessageID, strMessageReader, strUserID, dtmUpdateTime)

                udtDB.CommitTransaction()
            Catch eSQL As SqlException
                udtDB.RollBackTranscation()
                Throw eSQL
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw ex
            End Try

            Return bResult
        End Function

        Private Sub AddMessageList(ByRef udtDB As Database, ByVal udtMessageCollection As MessageModelCollection)
            For Each udtMessage As MessageModel In udtMessageCollection.Values
                Me.AddMessage(udtDB, udtMessage)
            Next
        End Sub

        Private Sub AddMessage(ByRef udtDB As Database, ByVal udtMessage As MessageModel)
            Me.AddMessage(udtDB, udtMessage.MessageID, udtMessage.Subject, udtMessage.Message, udtMessage.CreateBy, udtMessage.CreateDtm)
        End Sub

        Private Sub AddMessageReaderList(ByRef udtDB As Database, ByVal udtMessageReaderCollection As MessageReaderModelCollection)
            For Each udtMessageReader As MessageReaderModel In udtMessageReaderCollection.Values
                Me.AddMessageReader(udtDB, udtMessageReader)
            Next
        End Sub

        Private Sub AddMessageReader(ByRef udtDB As Database, ByVal udtMessageReaderModel As MessageReaderModel)
            Me.AddMessageReader(udtDB, udtMessageReaderModel.MessageID, udtMessageReaderModel.MessageReader, udtMessageReaderModel.UpdateBy, udtMessageReaderModel.UpdateDtm)
        End Sub

        Public Function AddMessageAndMessageReaderList(ByRef udtDB As Database, ByVal udtMessageCollection As MessageModelCollection, ByVal udtMessageReaderCollection As MessageReaderModelCollection) As Boolean
            Try

                Me.AddMessageList(udtDB, udtMessageCollection)
                Me.AddMessageReaderList(udtDB, udtMessageReaderCollection)

                Return True
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        'Public Function AddMessageAndMessageReaderList(ByVal udtMessageCollection As MessageModelCollection, ByVal udtMessageReaderCollection As MessageReaderModelCollection) As Boolean

        '    udtDB.BeginTransaction()
        '    Try

        '        Me.AddMessageList(udtDB, udtMessageCollection)
        '        Me.AddMessageReaderList(udtDB, udtMessageReaderCollection)

        '        udtDB.CommitTransaction()
        '        Return True
        '    Catch ex As Exception
        '        udtDB.RollBackTranscation()
        '        Throw ex
        '        Return False
        '    End Try
        'End Function

        Public Function GetNewMessageCount(ByVal strMsgReader As String, ByVal dtmLastCheck As DateTime, ByVal dtmCurrentTime As DateTime) As Integer

            Dim dt As New DataTable

            Try
                ' create data object and params
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@message_reader", SqlDbType.VarChar, 20, strMsgReader), _
                                                udtDB.MakeInParam("@last_check_dtm", SqlDbType.DateTime, 8, dtmLastCheck), _
                                                udtDB.MakeInParam("@current_dtm", SqlDbType.DateTime, 8, dtmCurrentTime)}

                ' run the stored procedure
                udtDB.RunProc("proc_InboxMessage_get_NewMessageCountbyTimeRange", prams, dt)
                Return CType(dt.Rows(0)(0), Integer)
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Private Sub AddMessage(ByRef db As Database, ByVal strMessageID As String, ByVal strSubject As String, ByVal strContent As String, ByVal strUserID As String, ByVal dtmUpdateTime As DateTime)
            Try
                ' create data object and params
                Dim prams() As SqlParameter = {db.MakeInParam("@message_id", SqlDbType.Char, 12, strMessageID), _
                                                db.MakeInParam("@subject", SqlDbType.NVarChar, 500, strSubject), _
                                                db.MakeInParam("@content", SqlDbType.NText, 2147483647, strContent), _
                                                db.MakeInParam("@create_by", SqlDbType.VarChar, 20, strUserID), _
                                                db.MakeInParam("@create_dtm", SqlDbType.DateTime, 8, dtmUpdateTime)}

                ' run the stored procedure
                db.RunProc("proc_Message_add", prams)
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Private Sub AddMessageReader(ByRef db As Database, ByVal strMessageID As String, ByVal strMsgReader As String, ByVal strUserID As String, ByVal dtmUpdateTime As DateTime)
            Try
                ' create data object and params
                Dim prams() As SqlParameter = {db.MakeInParam("@message_id", SqlDbType.Char, 12, strMessageID), _
                                                db.MakeInParam("@message_reader", SqlDbType.VarChar, 20, strMsgReader), _
                                                db.MakeInParam("@update_by", SqlDbType.VarChar, 20, strUserID), _
                                                db.MakeInParam("@update_dtm", SqlDbType.DateTime, 8, dtmUpdateTime)}

                ' run the stored procedure
                db.RunProc("proc_MessageReader_add", prams)
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
    End Class
End Namespace