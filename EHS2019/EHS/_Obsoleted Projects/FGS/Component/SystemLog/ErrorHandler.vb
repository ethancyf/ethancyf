Imports System.Data
Imports System.Data.SqlClient

Namespace SystemLog

    <Serializable()> Public Class ErrorHandler

        Private Const ERRORHANDLER_URL As String = "ERRORHANDLER_URL"
        Private Const ERRORHANDLER_MESSAGE As String = "ERRORHANDLER_MESSAGE"
        Private Const ERRORHANDLER_STACKTRACE As String = "ERRORHANDLER_STACKTRACE"
        Private Const ERRORHANDLER_FUNCTIONCODE As String = "ERRORHANDLER_FUNCTIONCODE"
        Private Const ERRORHANDLER_SEVERITYCODE As String = "ERRORHANDLER_SEVERITYCODE"
        Private Const ERRORHANDLER_MESSAGECODE As String = "ERRORHANDLER_MESSAGECODE"

        Private Const SESS_ERRORHANDLER_MSG_SHOWED As String = "SESS_ERRORHANDLER_MSG_SHOWED"
        Private Const SESS_ERRORHANDLER_CODETABLE As String = "SESS_ERRORHANDLER_CODETABLE"
        Private Const SESS_ERRORHANDLER_HEADER As String = "SESS_ERRORHANDLER_HEADER"

        Public Enum EnumSeverityCode
            AsyncPostBack = 65
            Data = 68
            Unknown = 69
        End Enum

        Public Sub New()

        End Sub

        '========================
        'Message for error page
        '========================
        Public Shared Property URL() As String
            Get
                'If HttpContext.Current.Session(ERRORHANDLER_URL) Is Nothing Then
                '    Return Nothing
                'End If
                If HttpContext.Current.Session Is Nothing Then
                    Return Nothing
                End If
                Return CStr(HttpContext.Current.Session(ERRORHANDLER_URL)) 'strURL
            End Get
            Set(ByVal Value As String)
                HttpContext.Current.Session(ERRORHANDLER_URL) = Value
            End Set
        End Property

        Public Shared Property Message() As String
            Get
                If HttpContext.Current.Session Is Nothing Then
                    Return HttpContext.Current.Server.GetLastError().GetBaseException().Message
                End If
                Return CStr(HttpContext.Current.Session(ERRORHANDLER_MESSAGE))
            End Get
            Set(ByVal Value As String)
                HttpContext.Current.Session(ERRORHANDLER_MESSAGE) = Value
            End Set
        End Property

        Public Shared Property StackTrace() As String
            Get
                If HttpContext.Current.Session Is Nothing Then
                    Return HttpContext.Current.Server.GetLastError().GetBaseException().StackTrace
                End If
                Return CStr(HttpContext.Current.Session(ERRORHANDLER_STACKTRACE))
            End Get
            Set(ByVal Value As String)
                HttpContext.Current.Session(ERRORHANDLER_STACKTRACE) = Value
            End Set
        End Property

        Public Shared Property FunctionCode() As String
            Get
                Dim strFunctionCode As String
                strFunctionCode = ""
                If HttpContext.Current.Session Is Nothing Then
                    Return String.Empty
                End If
                If Not HttpContext.Current.Session(ERRORHANDLER_FUNCTIONCODE) Is Nothing Then
                    strFunctionCode = CStr(HttpContext.Current.Session(ERRORHANDLER_FUNCTIONCODE))
                End If
                Return strFunctionCode
            End Get
            Set(ByVal value As String)
                HttpContext.Current.Session(ERRORHANDLER_FUNCTIONCODE) = value
            End Set
        End Property

        Public Shared Property SeverityCode() As String
            Get
                If HttpContext.Current.Session Is Nothing Then
                    Return String.Empty
                End If
                Return CStr(HttpContext.Current.Session(ERRORHANDLER_SEVERITYCODE))
            End Get
            Set(ByVal value As String)
                HttpContext.Current.Session(ERRORHANDLER_SEVERITYCODE) = value
            End Set
        End Property

        Public Shared Property MessageCode() As String
            Get
                If HttpContext.Current.Session Is Nothing Then
                    Return String.Empty
                End If
                Return CStr(HttpContext.Current.Session(ERRORHANDLER_MESSAGECODE))
            End Get
            Set(ByVal value As String)
                HttpContext.Current.Session(ERRORHANDLER_MESSAGECODE) = value
            End Set
        End Property

        ''' <summary>
        ''' Clear error in session
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub ClearLastError()
            If Not HttpContext.Current.Session Is Nothing Then
                HttpContext.Current.Session(ERRORHANDLER_URL) = Nothing
                HttpContext.Current.Session(ERRORHANDLER_MESSAGE) = Nothing
                HttpContext.Current.Session(ERRORHANDLER_STACKTRACE) = Nothing
                HttpContext.Current.Session(ERRORHANDLER_FUNCTIONCODE) = Nothing
                HttpContext.Current.Session(ERRORHANDLER_SEVERITYCODE) = Nothing
                HttpContext.Current.Session(ERRORHANDLER_MESSAGECODE) = Nothing
            End If
        End Sub

        Public Shared Sub AddSystemInterfaceLog(ByVal db As Database, ByVal strFunctionCode As String, ByVal strSeverityCode As String, _
                           ByVal strMessageCode As String, ByVal strPageLink As String, ByVal strClientIP As String, ByVal strUserID As String, ByVal strDataEntryAccount As String, ByVal strSessionID As String, ByVal strBrowser As String, ByVal strOS As String, Optional ByVal strUserDefinedMessage As String = Nothing)

            Try
                Dim prams(10) As SqlParameter
                prams(0) = db.MakeInParam("@function_code", SqlDbType.VarChar, 6, strFunctionCode)
                prams(1) = db.MakeInParam("@severity_code", SqlDbType.VarChar, 1, strSeverityCode)
                prams(2) = db.MakeInParam("@message_code", SqlDbType.VarChar, 5, strMessageCode)
                prams(3) = db.MakeInParam("@client_ip", SqlDbType.VarChar, 20, strClientIP)
                prams(4) = db.MakeInParam("@user_id", SqlDbType.VarChar, 20, IIf(strUserID Is Nothing, DBNull.Value, strUserID))
                prams(5) = db.MakeInParam("@data_entry_account", SqlDbType.VarChar, 20, IIf(strDataEntryAccount Is Nothing, DBNull.Value, strDataEntryAccount))
                prams(6) = db.MakeInParam("@url", SqlDbType.VarChar, 255, strPageLink)
                prams(7) = db.MakeInParam("@system_message", SqlDbType.NText, 0, IIf(strUserDefinedMessage Is Nothing, DBNull.Value, strUserDefinedMessage))
                prams(8) = db.MakeInParam("@session_id", SqlDbType.VarChar, 40, strSessionID)
                'prams(7) = CType(IIf(Not strUserDefinedMessage Is Nothing, db.MakeInParam("@system_message", SqlDbType.Text, 0, strUserDefinedMessage), db.MakeInParam("@system_message", SqlDbType.Text, 16, DBNull.Value)), SqlParameter)
                prams(9) = db.MakeInParam("@browser", SqlDbType.VarChar, 20, strBrowser)
                prams(10) = db.MakeInParam("@os", SqlDbType.VarChar, 20, strOS)

                db.RunProc("proc_SystemLogInterface_add", prams)
                db = Nothing
                If Not HttpContext.Current.Session Is Nothing Then
                    HttpContext.Current.Session(ERRORHANDLER_FUNCTIONCODE) = Nothing
                End If

            Finally
                If Not db Is Nothing Then db.Dispose()
                ErrorHandler.ClearLastError()
            End Try

        End Sub

        Public Shared Sub HandleError(Optional ByVal e As System.Web.UI.AsyncPostBackErrorEventArgs = Nothing, Optional ByVal strFunctionCode As String = "")

            Dim strSeverityCode As String = Chr(EnumSeverityCode.AsyncPostBack)
            Dim strMessageCode As String = "99999"
            Dim objErr As Exception
            Dim innerErr As Exception

            If Not e Is Nothing Then
                objErr = e.Exception
                innerErr = e.Exception
            Else
                objErr = HttpContext.Current.Server.GetLastError().GetBaseException()
                innerErr = HttpContext.Current.Server.GetLastError().InnerException()
            End If

            Dim tmpMessage As String = "GetBaseException Message: " & objErr.Message.ToString
            Dim tmpStackTrace As String = "GetBaseException StackTrace: " & objErr.StackTrace.ToString

            If Not innerErr Is Nothing Then
                tmpMessage = tmpMessage & vbCrLf & vbCrLf & " InnerException Message: " & innerErr.Message.ToString
                tmpStackTrace = tmpStackTrace & vbCrLf & vbCrLf & " InnerException StackTrace: " & innerErr.StackTrace.ToString
            End If

            If TypeOf objErr Is System.Data.SqlClient.SqlException Then
                strSeverityCode = Chr(EnumSeverityCode.Data)
                Dim intExpNum As Integer
                intExpNum = CType(objErr, System.Data.SqlClient.SqlException).Number
                If intExpNum = 50000 Then
                    Dim strExptMsg As String

                    strExptMsg = objErr.Message

                    'Function code Length = 5
                    If strExptMsg.Length > 5 Then
                        strFunctionCode = strExptMsg.Substring(0, 6)
                    Else
                        strFunctionCode = strExptMsg.Substring(0, strExptMsg.Length)
                    End If

                End If
            Else
                strSeverityCode = Chr(EnumSeverityCode.AsyncPostBack)
            End If

            If Not HttpContext.Current.Session Is Nothing Then
                ErrorHandler.FunctionCode = strFunctionCode
                ErrorHandler.URL = HttpContext.Current.Request.Url.ToString
                ErrorHandler.Message = tmpMessage
                ErrorHandler.StackTrace = tmpStackTrace
                ErrorHandler.SeverityCode = strSeverityCode
            End If

        End Sub
    End Class

End Namespace


