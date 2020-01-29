Imports System.Data.SqlClient

<Serializable()> Public Class AuditLogEntry

#Region "Field"

    Private _dtmActionTime As DateTime
    Private _dtDescription As DataTable
    Private _strFunctionCode As String

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Unaccessible constructor
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
    End Sub

    Public Sub New(ByVal strFunctionCode As String)
        _dtmActionTime = DateTime.Now
        _dtDescription = InitDescriptionTable()
        _strFunctionCode = strFunctionCode
    End Sub

#End Region

#Region "Public Function"

    Public Sub AddDescripton(ByVal strField As String, ByVal strValue As String)
        Dim dr As DataRow = _dtDescription.NewRow
        dr.Item("Field") = strField
        dr.Item("Value") = strValue
        _dtDescription.Rows.Add(dr)
    End Sub

    Public Sub WriteLog(ByVal strLogID As String, ByVal strDescription As String, Optional ByVal strData As String = "")
        WriteLogToDB(strLogID, strDescription, strData)
    End Sub

#End Region

#Region "Database Function"

    Private Sub WriteLogToDB(ByVal strLogID As String, ByVal strDescription As String, ByVal strData As String)
        ' Description
        Dim sbDescription As New StringBuilder

        If _dtDescription.Rows.Count > 0 Then
            sbDescription.Append(": ")

            For Each dr As DataRow In _dtDescription.Rows
                sbDescription.Append("<" + dr("Field").ToString.Trim)

                If dr("Value").ToString.Trim <> String.Empty Then
                    sbDescription.Append(": ")
                    sbDescription.Append(dr("Value").ToString.Trim)
                End If

                sbDescription.Append(">")

            Next
        End If

        strDescription += sbDescription.ToString()

        ' Client IP + Session ID
        Dim strClientIP As String = String.Empty
        Dim strSessionID As String = String.Empty

        Try
            strClientIP = HttpContext.Current.Request.UserHostAddress
            strSessionID = HttpContext.Current.Session.SessionID.ToString
        Catch ex As Exception
            strClientIP = String.Empty
            strSessionID = String.Empty
        End Try

        ' Browser + OS
        Dim strBrowser As String = String.Empty
        Dim strOS As String = String.Empty

        Try
            If Not IsNothing(HttpContext.Current.Request.Browser) Then
                strBrowser = HttpContext.Current.Request.Browser.Type
                strOS = HttpContext.Current.Request.Browser.Platform.Trim
            End If
        Catch ex As Exception
            strBrowser = String.Empty
            strOS = String.Empty
        End Try

        AddAuditLogInterface(_dtmActionTime, strClientIP, strLogID, _strFunctionCode, strDescription, strSessionID, strBrowser, strOS, strData)

        _dtDescription = InitDescriptionTable()

    End Sub

    Private Sub AddAuditLogInterface(ByVal dtmActionTime As DateTime, ByVal strClientIP As String, ByVal strLogID As String, _
                                        ByVal strFunctionCode As String, ByVal strDescription As String, ByVal strSessionID As String, _
                                        ByVal strBrowser As String, ByVal strOS As String, ByVal strData As String)
        Dim udtDB As New Database(DBFlag.dbEVS_InterfaceLog)

        Dim intDelimitor As Integer = strDescription.IndexOf("**********")
        Dim strMsgCode As String = String.Empty

        If intDelimitor > 0 Then
            strMsgCode = strDescription.Substring(intDelimitor + 12).Replace(">", "")
            strDescription = strDescription.Substring(0, intDelimitor - 1)
        End If

        Dim prams() As SqlParameter = { _
        udtDB.MakeInParam("@action_time", SqlDbType.DateTime, 8, dtmActionTime), _
        udtDB.MakeInParam("@end_time", SqlDbType.DateTime, 8, DBNull.Value), _
        udtDB.MakeInParam("@action_key", SqlDbType.VarChar, 20, String.Empty), _
        udtDB.MakeInParam("@client_ip", SqlDbType.VarChar, 20, strClientIP), _
        udtDB.MakeInParam("@user_id", SqlDbType.Char, 20, String.Empty), _
        udtDB.MakeInParam("@data_entry_account", SqlDbType.Char, 20, DBNull.Value), _
        udtDB.MakeInParam("@function_code", SqlDbType.Char, 6, strFunctionCode), _
        udtDB.MakeInParam("@log_id", SqlDbType.VarChar, 5, strLogID), _
        udtDB.MakeInParam("@description", SqlDbType.NVarChar, 1000, strDescription), _
        udtDB.MakeInParam("@session_id", SqlDbType.VarChar, 40, strSessionID), _
        udtDB.MakeInParam("@browser", SqlDbType.VarChar, 20, strBrowser), _
        udtDB.MakeInParam("@os", SqlDbType.VarChar, 20, strOS), _
        udtDB.MakeInParam("@message_code", SqlDbType.VarChar, 525, strMsgCode), _
        udtDB.MakeInParam("@Data", SqlDbType.VarChar, 8000, strData)}

        udtDB.RunProc("proc_AuditLogInterface_add", prams)

        If Not udtDB Is Nothing Then udtDB.Dispose()

    End Sub

#End Region

#Region "Supporting Function"

    Private Function InitDescriptionTable() As DataTable
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("Field", GetType(System.String)))
        dt.Columns.Add(New DataColumn("Value", GetType(System.String)))
        Return dt
    End Function

#End Region

End Class
