Imports System.Data.SqlClient

Imports Common.DataAccess

Namespace ComObject
    <Serializable()> Public Class ScheduleJobLogEntry
        Inherits BaseAuditLogEntry

        Private _dtDescription As DataTable = Nothing

#Region "Property"

        Private _strFunctionCode As String = String.Empty

        Public ReadOnly Property FunctionCode() As String
            Get
                Return _strFunctionCode
            End Get
        End Property

        Private _strActionKey As String = String.Empty

        Public ReadOnly Property ActionKey() As String
            Get
                Return _strActionKey
            End Get
        End Property

        Private _dtmActionTime As Nullable(Of DateTime) = Nothing

        Public ReadOnly Property ActionTime() As Nullable(Of DateTime)
            Get
                Return _dtmActionTime
            End Get
        End Property

        Private _blnIsWriteConsole As Boolean = False

        Public Property IsWriteConsole() As Boolean
            Get
                Return _blnIsWriteConsole
            End Get
            Set(ByVal value As Boolean)
                _blnIsWriteConsole = value
            End Set
        End Property
#End Region

#Region "Constructor"

        Public Sub New(ByVal strFunctionCode As String)
            _strFunctionCode = strFunctionCode
            _strActionKey = MyBase.GetUniqueKey
            _dtmActionTime = Now
        End Sub

#End Region

#Region "Public Function"

        Public Sub WriteLog(ByVal strLogID As String, ByVal strDesc As String)
            WriteLog(strLogID, strDesc, Nothing, Nothing)
        End Sub

        Public Sub WriteLog(ByVal strLogID As String, ByVal strDesc As String, ByVal dtmStart As Nullable(Of DateTime), ByVal dtmEnd As Nullable(Of DateTime))
            WriteLog(strLogID, strDesc, Nothing, Nothing, Nothing, Nothing)
        End Sub

        Public Function WriteStartLog(ByVal strLogID As String, ByVal strDesc As String) As AuditLogStartKey
            Dim objStartKey As New AuditLogStartKey(Me, Now)

            WriteLog(strLogID, strDesc, objStartKey.StartTime, Nothing)

            Return objStartKey
        End Function

        Public Function WriteStartLog(ByVal strLogID As String, ByVal strDesc As String, ByVal strAction As String, ByVal strStatus As String) As AuditLogStartKey
            Dim objStartKey As New AuditLogStartKey(Me, Now)

            WriteLog(strLogID, strDesc, objStartKey.StartTime, Nothing, strAction, strStatus)

            Return objStartKey
        End Function

        Public Sub WriteEndLog(ByVal objStartKey As AuditLogStartKey, ByVal strLogID As String, ByVal strDesc As String)
            WriteLog(strLogID, strDesc, objStartKey.StartTime, Now)
        End Sub

        Public Sub WriteEndLog(ByVal objStartKey As AuditLogStartKey, ByVal strLogID As String, ByVal strDesc As String, ByVal strAction As String, ByVal strStatus As String)
            WriteLog(strLogID, strDesc, objStartKey.StartTime, Now, strAction, strStatus)
        End Sub

        Public Sub WriteLog(ByVal strLogID As String, ByVal strDesc As String, ByVal dtmStart As Nullable(Of DateTime), ByVal dtmEnd As Nullable(Of DateTime), ByVal strAction As String, ByVal strStatus As String)
            ' Write Database log
            Dim udtDB As Database = MyBase.CreateDatabase()

            Dim objStartDtm As Object = DBNull.Value
            Dim objEndDtm As Object = DBNull.Value

            If dtmStart.HasValue Then
                objStartDtm = dtmStart.Value
            End If

            If dtmEnd.HasValue Then
                objEndDtm = dtmEnd.Value
            End If

            Try
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Action_Dtm", SqlDbType.DateTime, 8, _dtmActionTime.Value), _
                    udtDB.MakeInParam("@Client_IP", SqlDbType.VarChar, 20, GetIPAddress()), _
                    udtDB.MakeInParam("@Program_ID", SqlDbType.VarChar, 30, _strFunctionCode), _
                    udtDB.MakeInParam("@Action", SqlDbType.VarChar, 30, IIf(String.IsNullOrEmpty(strAction), DBNull.Value, strAction)), _
                    udtDB.MakeInParam("@Status", SqlDbType.VarChar, 20, IIf(String.IsNullOrEmpty(strStatus), DBNull.Value, strStatus)), _
                    udtDB.MakeInParam("@Return_Description", SqlDbType.NText, 2147483647, DBNull.Value), _
                    udtDB.MakeInParam("@Description", SqlDbType.NText, 2147483647, strDesc & GetAdditionalDescription()), _
                    udtDB.MakeInParam("@Start_Dtm", SqlDbType.DateTime, 8, objStartDtm), _
                    udtDB.MakeInParam("@End_Dtm", SqlDbType.DateTime, 8, objEndDtm), _
                    udtDB.MakeInParam("@Log_ID", SqlDbType.VarChar, 20, strLogID), _
                    udtDB.MakeInParam("@Action_key", SqlDbType.VarChar, 20, _strActionKey)}

                udtDB.RunProc("proc_ScheduleJobLog_add", prams)
                ClearDescTable()
            Finally
                If Not udtDB Is Nothing Then udtDB.Dispose()
            End Try

            ' Write Console log
            Try
                If Me.IsWriteConsole Then
                    WriteConsoleLog(strDesc)
                End If
            Catch ex As Exception
                ' Do Nothing
            End Try
        End Sub

        Public Sub WriteConsoleLog(ByVal strText As String)
            Console.WriteLine(String.Format("<{0}> {1}", Now.ToString("yyyy-MM-dd HH:mm:ss"), strText))
        End Sub

#Region "Public Function : AddDescription"

        Public Sub AddDescripton(ByVal strField As String, ByVal strValue As String)
            If _dtDescription Is Nothing Then
                _dtDescription = InitDescTable()
            End If
            Dim dr As DataRow
            dr = _dtDescription.NewRow
            dr.Item("Field") = strField
            dr.Item("Value") = strValue
            _dtDescription.Rows.Add(dr)
        End Sub

        Public Sub AddDescripton(ByVal objException As Exception)
            If _dtDescription Is Nothing Then
                _dtDescription = InitDescTable()
            End If
            Dim dr As DataRow
            dr = _dtDescription.NewRow
            dr.Item("Field") = "StackTrace"
            dr.Item("Value") = String.Format("[{0}],[{1}]", objException.Message, objException.StackTrace)
            _dtDescription.Rows.Add(dr)
        End Sub

        ''' <summary>
        ''' INT11-0022
        ''' Log fix date format on date object, even culture changed
        ''' </summary>
        ''' <param name="strField"></param>
        ''' <param name="dtmValue"></param>
        ''' <remarks></remarks>
        Public Sub AddDescripton(ByVal strField As String, ByVal dtmValue As Date)
            If _dtDescription Is Nothing Then
                _dtDescription = InitDescTable()
            End If
            Dim dr As DataRow
            dr = _dtDescription.NewRow
            dr.Item("Field") = strField
            dr.Item("Value") = dtmValue.ToString(MyBase.FORMAT_DATE)
            _dtDescription.Rows.Add(dr)
        End Sub

        ''' <summary>
        ''' INT11-0022
        ''' Log fix date format on date object, even culture changed
        ''' </summary>
        ''' <param name="strField"></param>
        ''' <param name="dtmValue"></param>
        ''' <remarks></remarks>
        Public Sub AddDescripton(ByVal strField As String, ByVal dtmValue As System.Nullable(Of Date))
            If _dtDescription Is Nothing Then
                _dtDescription = InitDescTable()
            End If
            Dim dr As DataRow
            dr = _dtDescription.NewRow
            dr.Item("Field") = strField
            dr.Item("Value") = dtmValue.Value.ToString(MyBase.FORMAT_DATE)
            _dtDescription.Rows.Add(dr)
        End Sub

#End Region

#End Region

#Region "Support Function"

        Protected Function GetIPAddress() As String
            Dim strIPAddress As String = String.Empty

            Dim ipAddress() As System.Net.IPAddress = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList

            If ipAddress.Length > 0 Then
                strIPAddress = ipAddress(0).ToString
            End If

            Return strIPAddress

        End Function

        Private Function InitDescTable() As DataTable
            Dim dt As New DataTable
            dt.Columns.Add(New DataColumn("Field", GetType(System.String)))
            dt.Columns.Add(New DataColumn("Value", GetType(System.String)))
            Return dt
        End Function

        Private Sub ClearDescTable()
            If _dtDescription IsNot Nothing Then
                Me._dtDescription.Rows.Clear()
            End If
        End Sub

        Private Function GetAdditionalDescription() As String
            Dim sbDescription As New StringBuilder

            If Not _dtDescription Is Nothing Then
                If _dtDescription.Rows.Count > 0 Then
                    sbDescription.Append(": ")
                End If
                For i As Integer = 0 To _dtDescription.Rows.Count - 1
                    sbDescription.Append("<" & CStr(_dtDescription.Rows(i).Item("Field")))
                    If Not _dtDescription.Rows(i).Item(1) Is DBNull.Value Then
                        If CStr(_dtDescription.Rows(i).Item(1)) <> "" Then
                            sbDescription.Append(": ")
                            sbDescription.Append(CStr(_dtDescription.Rows(i).Item(1)))
                        End If
                    End If
                    sbDescription.Append(">")
                Next
            End If
            Return sbDescription.ToString()
        End Function
#End Region

#Region "Obsolete Function"

        ''' <summary>
        ''' Obsoleted function since schedule job log table enhanced with new columns
        ''' </summary>
        ''' <param name="dtmAction"></param>
        ''' <param name="strIP"></param>
        ''' <param name="strProgram"></param>
        ''' <param name="strAction"></param>
        ''' <param name="strStatus"></param>
        ''' <param name="strReturn"></param>
        ''' <param name="strDesc"></param>
        ''' <remarks></remarks>
        Public Shared Sub WriteLog(ByVal dtmAction As DateTime, ByVal strIP As String, ByVal strProgram As String, ByVal strAction As String, ByVal strStatus As String, ByVal strReturn As String, ByVal strDesc As String)

            Dim udtDB As Database = New Database()

            Dim objReturn As Object = Nothing
            If strReturn Is Nothing Then
                objReturn = DBNull.Value
            Else
                objReturn = strReturn
            End If

            Try
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Action_Dtm", SqlDbType.DateTime, 8, dtmAction), _
                    udtDB.MakeInParam("@Client_IP", SqlDbType.VarChar, 20, strIP), _
                    udtDB.MakeInParam("@Program_ID", SqlDbType.VarChar, 30, strProgram), _
                    udtDB.MakeInParam("@Action", SqlDbType.VarChar, 30, strAction), _
                    udtDB.MakeInParam("@Status", SqlDbType.VarChar, 20, strStatus), _
                    udtDB.MakeInParam("@Return_Description", SqlDbType.NText, 2147483647, objReturn), _
                    udtDB.MakeInParam("@Description", SqlDbType.NText, 2147483647, strDesc)}

                udtDB.RunProc("proc_ScheduleJobLog_add", prams)

            Finally
                If Not udtDB Is Nothing Then udtDB.Dispose()
            End Try
        End Sub

#End Region


    End Class
End Namespace