Imports System.Data
Imports System.Data.SqlClient
Imports Common.DataAccess
Imports Common.Component
Imports Common.Component.UserAC
Imports Common.Component.HCVUUser
Imports Common.Component.ServiceProvider
Imports Common.Component.DataEntryUser

Namespace ComObject
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
            If HttpContext.Current IsNot Nothing AndAlso HttpContext.Current.Session IsNot Nothing Then
                HttpContext.Current.Session(ERRORHANDLER_URL) = Nothing
                HttpContext.Current.Session(ERRORHANDLER_MESSAGE) = Nothing
                HttpContext.Current.Session(ERRORHANDLER_STACKTRACE) = Nothing
                HttpContext.Current.Session(ERRORHANDLER_FUNCTIONCODE) = Nothing
                HttpContext.Current.Session(ERRORHANDLER_SEVERITYCODE) = Nothing
                HttpContext.Current.Session(ERRORHANDLER_MESSAGECODE) = Nothing
            End If
        End Sub

        ' CRE17-010 (OCSSS integration) [Start][Winnie SUEN]
        ' ----------------------------------------------------------
        ''' <summary>
        ''' Add Interface log regardless the platform
        ''' </summary>
        Public Shared Sub AddSystemInterfaceLog(ByVal db As Database, ByVal strFunctionCode As String, ByVal strSeverityCode As String, _
                           ByVal strMessageCode As String, ByVal strPageLink As String, ByVal strClientIP As String, ByVal strUserID As String, Optional ByVal strUserDefinedMessage As String = Nothing)

            Dim strSessionID As String

            Try
                strSessionID = HttpContext.Current.Session.SessionID
                If strSessionID Is Nothing Then
                    strSessionID = String.Empty
                End If
            Catch ex As Exception
                strSessionID = String.Empty
            End Try

            ' Browser & OS
            Dim strBrowser As String = String.Empty
            Dim strOS As String = String.Empty

            If HttpContext.Current IsNot Nothing Then
                Try
                    strBrowser = UserAgentInfoMapping.GetBrowser()
                    strOS = UserAgentInfoMapping.GetOS()
                Catch ex As Exception
                    strBrowser = String.Empty
                    strOS = String.Empty
                End Try
            End If

            AddSystemInterfaceLog(db, strFunctionCode, strSeverityCode, strMessageCode, strPageLink, strClientIP, strUserID, "", strSessionID, strBrowser, strOS, strUserDefinedMessage)
        End Sub
        ' CRE17-010 (OCSSS integration) [End][Winnie SUEN]

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="db">Database for Internal/External Interface module logging, (NOT FOR OTHER MODULES)</param>
        ''' <param name="strFunctionCode"></param>
        ''' <param name="strSeverityCode"></param>
        ''' <param name="strMessageCode"></param>
        ''' <param name="strPageLink"></param>
        ''' <param name="strClientIP"></param>
        ''' <param name="strUserDefinedMessage"></param>
        ''' <remarks></remarks>
        Public Shared Sub Log(ByVal db As Database, ByVal strFunctionCode As String, ByVal strSeverityCode As String, _
                           ByVal strMessageCode As String, ByVal strPageLink As String, ByVal strClientIP As String, ByVal strUserID As String, Optional ByVal strUserDefinedMessage As String = Nothing)
            Dim strPlatform As String = ConfigurationManager.AppSettings("Platform")
            Dim strSessionID As String

            Try
                strSessionID = HttpContext.Current.Session.SessionID
                If strSessionID Is Nothing Then
                    strSessionID = String.Empty
                End If
            Catch ex As Exception
                strSessionID = String.Empty
            End Try

            'I-CRE16-006 (Capture detail client browser and OS information) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            ' Browser & OS
            Dim strBrowser As String = String.Empty
            Dim strOS As String = String.Empty

            'Try
            '    If Not HttpContext.Current.Request.Browser Is Nothing Then
            '        strBrowser = HttpContext.Current.Request.Browser.Type '+ "-" + HttpContext.Current.Request.Browser.Version
            '        strOS = HttpContext.Current.Request.Browser.Platform.Trim()
            '    End If
            'Catch ex As Exception
            '    strBrowser = String.Empty
            '    strOS = String.Empty
            'End Try

            If HttpContext.Current IsNot Nothing Then
                Try
                    strBrowser = UserAgentInfoMapping.GetBrowser()
                    strOS = UserAgentInfoMapping.GetOS()
                Catch ex As Exception
                    strBrowser = String.Empty
                    strOS = String.Empty
                End Try
            End If
            'I-CRE16-006 (Capture detail client browser and OS information) [End][Chris YIM]

            If strPlatform = EVSPlatform.InterfaceInternal OrElse strPlatform = EVSPlatform.InterfaceExternal Then
                AddSystemInterfaceLog(db, strFunctionCode, strSeverityCode, strMessageCode, strPageLink, strClientIP, strUserID, "", strSessionID, strBrowser, strOS, strUserDefinedMessage)
            Else
                Log(strFunctionCode, strSeverityCode, strMessageCode, strPageLink, strClientIP, strUserDefinedMessage)
            End If
        End Sub

        Public Shared Sub Log(ByVal strFunctionCode As String, ByVal strSeverityCode As String, _
                           ByVal strMessageCode As String, ByVal strPageLink As String, ByVal strClientIP As String, Optional ByVal strUserDefinedMessage As String = Nothing)

            Dim strPlatform As String = ConfigurationManager.AppSettings("Platform")
            Dim strSessionID As String

            Try
                strSessionID = HttpContext.Current.Session.SessionID
                If strSessionID Is Nothing Then
                    strSessionID = String.Empty
                End If
            Catch ex As Exception
                strSessionID = String.Empty
            End Try

            'I-CRE16-006 (Capture detail client browser and OS information) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            ' Browser & OS
            Dim strBrowser As String = String.Empty
            Dim strOS As String = String.Empty
            Dim udtUserAgentInfoMapping As UserAgentInfoMapping = New UserAgentInfoMapping

            'Try
            '    If Not HttpContext.Current.Request.Browser Is Nothing Then
            '        strBrowser = HttpContext.Current.Request.Browser.Type '+ "-" + HttpContext.Current.Request.Browser.Version
            '        strOS = HttpContext.Current.Request.Browser.Platform.Trim()
            '    End If
            'Catch ex As Exception
            '    strBrowser = String.Empty
            '    strOS = String.Empty
            'End Try

            If HttpContext.Current IsNot Nothing Then
                Try
                    strBrowser = udtUserAgentInfoMapping.GetBrowser()
                    strOS = udtUserAgentInfoMapping.GetOS()
                Catch ex As Exception
                    strBrowser = String.Empty
                    strOS = String.Empty
                End Try
            End If
            'I-CRE16-006 (Capture detail client browser and OS information) [End][Chris YIM]

            If strPlatform = EVSPlatform.HCVU Then
                If HCVUUserBLL.Exist Then
                    Dim udtHCVUUserBLL As New HCVUUserBLL
                    Dim udtHCVUUser As HCVUUserModel = udtHCVUUserBLL.GetHCVUUser()
                    AddSystemHCVULog(strFunctionCode, strSeverityCode, strMessageCode, strPageLink, strClientIP, udtHCVUUser.UserID, strSessionID, strBrowser, strOS, strUserDefinedMessage)
                Else
                    AddSystemHCVULog(strFunctionCode, strSeverityCode, strMessageCode, strPageLink, strClientIP, Nothing, strSessionID, strBrowser, strOS, strUserDefinedMessage)
                End If
            ElseIf strPlatform = EVSPlatform.HCSP Then
                If UserACBLL.Exist Then
                    Dim udtUserAC As UserACModel
                    udtUserAC = UserACBLL.GetUserAC
                    If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                        Dim udtServiceProvider As ServiceProviderModel = CType(udtUserAC, ServiceProviderModel)
                        AddSystemHCSPLog(strFunctionCode, strSeverityCode, strMessageCode, strPageLink, strClientIP, udtServiceProvider.SPID, Nothing, strSessionID, strBrowser, strOS, strUserDefinedMessage)
                    Else
                        Dim udtDataEntryUser As DataEntryUserModel = CType(udtUserAC, DataEntryUserModel)
                        AddSystemHCSPLog(strFunctionCode, strSeverityCode, strMessageCode, strPageLink, strClientIP, udtDataEntryUser.SPID, udtDataEntryUser.DataEntryAccount, strSessionID, strBrowser, strOS, strUserDefinedMessage)
                    End If
                Else
                    AddSystemHCSPLog(strFunctionCode, strSeverityCode, strMessageCode, strPageLink, strClientIP, Nothing, Nothing, strSessionID, strBrowser, strOS, strUserDefinedMessage)
                End If
            ElseIf strPlatform = EVSPlatform.PublicPlatform Then
                'Dim strSystemLogPlatform As String = ConfigurationManager.AppSettings("LogPlatform")
                'If strSystemLogPlatform = "03a" Then
                '    AddSystemHCVRLog(strFunctionCode, strSeverityCode, strMessageCode, strPageLink, strClientIP, strSessionID, strBrowser, strOS, strUserDefinedMessage)
                'Else
                AddSystemPublicLog(strFunctionCode, strSeverityCode, strMessageCode, strPageLink, strClientIP, strSessionID, strBrowser, strOS, strUserDefinedMessage)
                'End If
            ElseIf strPlatform = EVSPlatform.SDIR Then
                AddSystemHCVRLog(strFunctionCode, strSeverityCode, strMessageCode, strPageLink, strClientIP, strSessionID, strBrowser, strOS, strUserDefinedMessage)
            ElseIf strPlatform = EVSPlatform.InterfaceInternal Then
                Throw New Exception("Missing database parameter object for Internal/External Interface logging, Use overload function to provide database")
            End If

        End Sub

        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Shared Sub CriticalError(ByVal strFunctionCode As String, ByVal strUserDefinedMessage As String)
            Log(strFunctionCode, Common.Component.SeverityCode.SEVE, "88888", _
                HttpContext.Current.Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, _
                strUserDefinedMessage)
        End Sub
        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

        Public Shared Sub AddSystemHCVULog(ByVal strFunctionCode As String, ByVal strSeverityCode As String, _
                          ByVal strMessageCode As String, ByVal strPageLink As String, ByVal strClientIP As String, ByVal strUserID As String, ByVal strSessionID As String, ByVal strBrowser As String, ByVal strOS As String, Optional ByVal strUserDefinedMessage As String = Nothing)

            Dim db As Database = New Database
            Try
                Dim prams(9) As SqlParameter
                prams(0) = db.MakeInParam("@function_code", SqlDbType.VarChar, 6, strFunctionCode)
                prams(1) = db.MakeInParam("@severity_code", SqlDbType.VarChar, 1, strSeverityCode)
                prams(2) = db.MakeInParam("@message_code", SqlDbType.VarChar, 5, strMessageCode)
                prams(3) = db.MakeInParam("@client_ip", SqlDbType.VarChar, 20, strClientIP)
                prams(4) = db.MakeInParam("@user_id", SqlDbType.VarChar, 20, IIf(strUserID Is Nothing, DBNull.Value, strUserID))
                'prams(5) = db.MakeInParam("@data_entry_account", SqlDbType.VarChar, 20, IIf(strDataEntryAccount Is Nothing, DBNull.Value, strDataEntryAccount))
                prams(5) = db.MakeInParam("@url", SqlDbType.VarChar, 255, strPageLink)
                prams(6) = db.MakeInParam("@system_message", SqlDbType.NText, 0, IIf(strUserDefinedMessage Is Nothing, DBNull.Value, strUserDefinedMessage))
                prams(7) = db.MakeInParam("@session_id", SqlDbType.VarChar, 40, strSessionID)
                'prams(7) = CType(IIf(Not strUserDefinedMessage Is Nothing, db.MakeInParam("@system_message", SqlDbType.Text, 0, strUserDefinedMessage), db.MakeInParam("@system_message", SqlDbType.Text, 16, DBNull.Value)), SqlParameter)
                prams(8) = db.MakeInParam("@browser", SqlDbType.VarChar, 20, strBrowser)
                prams(9) = db.MakeInParam("@os", SqlDbType.VarChar, 20, strOS)

                db.RunProc("proc_SystemLogHCVU_add", prams)
                db = Nothing
                If HttpContext.Current IsNot Nothing AndAlso HttpContext.Current.Session IsNot Nothing Then
                    HttpContext.Current.Session(ERRORHANDLER_FUNCTIONCODE) = Nothing
                End If

            Finally
                If Not db Is Nothing Then db.Dispose()
                ErrorHandler.ClearLastError()
            End Try

        End Sub

        Public Shared Sub AddSystemHCSPLog(ByVal strFunctionCode As String, ByVal strSeverityCode As String, _
                           ByVal strMessageCode As String, ByVal strPageLink As String, ByVal strClientIP As String, ByVal strUserID As String, ByVal strDataEntryAccount As String, ByVal strSessionID As String, ByVal strBrowser As String, ByVal strOS As String, Optional ByVal strUserDefinedMessage As String = Nothing)

            Dim db As Database = New Database
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

                db.RunProc("proc_SystemLogHCSP_add", prams)
                db = Nothing
                If Not HttpContext.Current.Session Is Nothing Then
                    HttpContext.Current.Session(ERRORHANDLER_FUNCTIONCODE) = Nothing
                End If

            Finally
                If Not db Is Nothing Then db.Dispose()
                ErrorHandler.ClearLastError()
            End Try

        End Sub

        Public Shared Sub AddSystemPublicLog(ByVal strFunctionCode As String, ByVal strSeverityCode As String, _
                           ByVal strMessageCode As String, ByVal strPageLink As String, ByVal strClientIP As String, ByVal strSessionID As String, ByVal strBrowser As String, ByVal strOS As String, Optional ByVal strUserDefinedMessage As String = Nothing)
            Dim db As Database = New Database
            Try
                Dim prams(9) As SqlParameter
                prams(0) = db.MakeInParam("@function_code", SqlDbType.VarChar, 6, strFunctionCode)
                prams(1) = db.MakeInParam("@severity_code", SqlDbType.VarChar, 1, strSeverityCode)
                prams(2) = db.MakeInParam("@message_code", SqlDbType.VarChar, 5, strMessageCode)
                prams(3) = db.MakeInParam("@client_ip", SqlDbType.VarChar, 20, strClientIP)
                prams(4) = db.MakeInParam("@url", SqlDbType.VarChar, 255, strPageLink)
                prams(5) = db.MakeInParam("@system_message", SqlDbType.NText, 0, IIf(strUserDefinedMessage Is Nothing, DBNull.Value, strUserDefinedMessage))
                prams(6) = db.MakeInParam("@session_id", SqlDbType.VarChar, 40, strSessionID)

                prams(7) = db.MakeInParam("@browser", SqlDbType.VarChar, 20, strBrowser)
                prams(8) = db.MakeInParam("@os", SqlDbType.VarChar, 20, strOS)

                db.RunProc("proc_SystemLogPublic_add", prams)
                db = Nothing
                If Not HttpContext.Current.Session Is Nothing Then
                    HttpContext.Current.Session(ERRORHANDLER_FUNCTIONCODE) = Nothing
                End If

            Finally
                If Not db Is Nothing Then db.Dispose()
                ErrorHandler.ClearLastError()
            End Try
        End Sub


        Public Shared Sub AddSystemHCVRLog(ByVal strFunctionCode As String, ByVal strSeverityCode As String, _
                           ByVal strMessageCode As String, ByVal strPageLink As String, ByVal strClientIP As String, ByVal strSessionID As String, ByVal strBrowser As String, ByVal strOS As String, Optional ByVal strUserDefinedMessage As String = Nothing)
            Dim db As Database = New Database
            Try
                Dim prams(9) As SqlParameter
                prams(0) = db.MakeInParam("@function_code", SqlDbType.VarChar, 6, strFunctionCode)
                prams(1) = db.MakeInParam("@severity_code", SqlDbType.VarChar, 1, strSeverityCode)
                prams(2) = db.MakeInParam("@message_code", SqlDbType.VarChar, 5, strMessageCode)
                prams(3) = db.MakeInParam("@client_ip", SqlDbType.VarChar, 20, strClientIP)
                prams(4) = db.MakeInParam("@url", SqlDbType.VarChar, 255, strPageLink)
                prams(5) = db.MakeInParam("@system_message", SqlDbType.NText, 0, IIf(strUserDefinedMessage Is Nothing, DBNull.Value, strUserDefinedMessage))
                prams(6) = db.MakeInParam("@session_id", SqlDbType.VarChar, 40, strSessionID)

                prams(7) = db.MakeInParam("@browser", SqlDbType.VarChar, 20, strBrowser)
                prams(8) = db.MakeInParam("@os", SqlDbType.VarChar, 20, strOS)

                db.RunProc("proc_SystemLogHCVR_add", prams)
                db = Nothing
                If Not HttpContext.Current.Session Is Nothing Then
                    HttpContext.Current.Session(ERRORHANDLER_FUNCTIONCODE) = Nothing
                End If

            Finally
                If Not db Is Nothing Then db.Dispose()
                ErrorHandler.ClearLastError()
            End Try
        End Sub

        Private Shared Sub AddSystemInterfaceLog(ByVal db As Database, ByVal strFunctionCode As String, ByVal strSeverityCode As String, _
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
                If HttpContext.Current IsNot Nothing AndAlso HttpContext.Current.Session IsNot Nothing Then
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

            Dim tmpMessage As String = "GetBaseException Message: " & objErr.Message
            Dim tmpStackTrace As String = "GetBaseException StackTrace: " & objErr.StackTrace

            If Not innerErr Is Nothing Then
                tmpMessage = tmpMessage & vbCrLf & vbCrLf & " InnerException Message: " & innerErr.Message
                tmpStackTrace = tmpStackTrace & vbCrLf & vbCrLf & " InnerException StackTrace: " & innerErr.StackTrace
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

        Public Shared Sub HandleScriptManagerAsyncPostBackError(ByVal e As System.Web.UI.AsyncPostBackErrorEventArgs, Optional ByVal strFunctionCode As String = "")
            ErrorHandler.ClearLastError()
            ErrorHandler.HandleError(e, strFunctionCode)
            Throw e.Exception
        End Sub

    End Class
End Namespace


