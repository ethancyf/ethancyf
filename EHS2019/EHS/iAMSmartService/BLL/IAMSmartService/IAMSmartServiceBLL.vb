Imports System.IO
Imports System.Runtime.Serialization
Imports System.Web
Imports eService.Common
Imports eID.Bussiness
Imports eID.Bussiness.Impl
Imports eID.Bussiness.Interface
Imports eService.DTO.Enum
Imports eService.DTO.Request
Imports eService.DTO.Response
Imports System.Security.Cryptography.X509Certificates
Imports System.Security.Cryptography
Imports Org.BouncyCastle.Crypto
Imports Org.BouncyCastle.Crypto.Parameters
Imports Org.BouncyCastle.Math
Imports Org.BouncyCastle.Asn1.X509
Imports Org.BouncyCastle.X509
Imports Org.BouncyCastle.Asn1.Pkcs
Imports Org.BouncyCastle.Pkcs
Imports Org.BouncyCastle.Asn1

Imports Common.Component.iAMSmart
Imports iAMSmartService.Log
Imports iAMSmartService.Service
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.ServiceProvider
Imports Common.DataAccess
Imports Common.Component.UserAC
Imports Newtonsoft.Json.Linq

Namespace BLL.IAMSmartService

    Public Class IAMSmartServiceBLL
        Inherits BaseService

        Private blnTest = False

#Region "Constants"
        Public Class AuditLogMsg
            Public Const MSG00003 As String = "[iAMSmart>EHS] GetState from direct login start"
            Public Const MSG00004 As String = "[iAMSmart>EHS] GetState: redirectURI: "
            Public Const MSG00005 As String = "[iAMSmart>EHS] GetState: Pass to iAMSmart"
            Public Const MSG00006 As String = "[iAMSmart>EHS] GetState: GetState from direct login end "
            Public Const MSG00007 As String = "[iAMSmart>EHS] GetState: redirect URI is empty."
            Public Const MSG00008 As String = "[iAMSmart>EHS] GetState from direct login End"

            Public Const MSG00010 As String = "[iAMSmart>EHS] Handle AuthCallback Start"
            Public Const MSG00011 As String = "[iAMSmart>EHS] AuthCallback: Request body"
            Public Const MSG00012 As String = "[iAMSmart>EHS] AuthCallback: Handle AuthCallback fail"
            Public Const MSG00013 As String = "[iAMSmart>EHS] AuthCallback: Request success"
            Public Const MSG00014 As String = "[iAMSmart>EHS] AuthCallback: Handle direct login"
            Public Const MSG00015 As String = "[iAMSmart>EHS] AuthCallback: Handle direct login fail"
            Public Const MSG00016 As String = "[iAMSmart>EHS] AuthCallback: Handle normal iAMSmart login"
            Public Const MSG00017 As String = "[iAMSmart>EHS] AuthCallback: Handle AuthCallback fail"
            Public Const MSG00018 As String = "[iAMSmart>EHS] Handle AuthCallback End"
            Public Const MSG00019 As String = "[iAMSmart>EHS] Handle AuthCallback End (Tester)"

            Public Const MSG00020 As String = "[iAMSmart>EHS] Handle ProfileCallback Start"
            Public Const MSG00021 As String = "[iAMSmart>EHS] ProfileCallback: udtCallBackDto"
            Public Const MSG00022 As String = "[iAMSmart>EHS] ProfileCallback: Receive body"
            Public Const MSG00023 As String = "[iAMSmart>EHS] ProfileCallback: Key"
            Public Const MSG00024 As String = "[iAMSmart>EHS] ProfileCallback: Update Profile (Return Code)"
            Public Const MSG00025 As String = "[iAMSmart>EHS] ProfileCallback: Cancel Update Profile"
            Public Const MSG00026 As String = "[iAMSmart>EHS] Handle ProfileCallBack End"
        End Class
#End Region

#Region "Methods"
        Public Function HandleAuthCallback(ByVal context As HttpContext) As String
            Dim udtAuditLog As New AuditLogEntry(FunctCode.FUNT070501, DBFlagStr.DBFlagInterfaceLog)
            Dim udtiAMSmartBLL As New iAMSmartBLL
            Dim stateUtils = New StateUtils()
            Dim strURL As StringBuilder = New StringBuilder()

            Dim strRequestUrl As String = context.Request.Url.PathAndQuery.ToLower.Trim.Split("/".ToCharArray, StringSplitOptions.RemoveEmptyEntries).Last
            Dim strRequest As String = String.Empty

            Dim strCode As String = String.Empty
            Dim strState As String = String.Empty
            Dim strErrorCode As String = String.Empty
            Dim strRedirectURL As String = String.Empty

            Dim dtState As Data.DataTable = Nothing

            Dim strDemoVersion As String = ConfigurationManager.AppSettings("DemoVersion")
            Dim strTesterRedirectURL As String = ConfigurationManager.AppSettings("TesterRedirectLink")
            Dim blnDemo As Boolean = False

            Me.OnInit()
            udtAuditLog.WriteStartLog(LogID.LOG00010, AuditLogMsg.MSG00010)

            If Not String.IsNullOrEmpty(strDemoVersion) AndAlso strDemoVersion = YesNo.Yes Then
                blnDemo = True
            End If

            If context.Request.ServerVariables("query_string").Length > 0 Then
                strState = context.Request.QueryString(Constants.PARAM_STATE)
                strCode = context.Request.QueryString(Constants.PARAM_CODE)
                strErrorCode = context.Request.QueryString(Constants.PARAM_ERROR_CODE)

                If strState.Contains("Demo") Then
                    blnDemo = True
                End If

            End If

            udtAuditLog.AddDescripton("State", strState)
            udtAuditLog.AddDescripton("Code", strCode)
            udtAuditLog.AddDescripton("ErrorCode", strErrorCode)
            udtAuditLog.AddDescripton("context", strRequestUrl)
            If blnDemo Then
                udtAuditLog.AddDescripton("DemoVersion", "Y")
            End If

            udtAuditLog.WriteLog(LogID.LOG00011, AuditLogMsg.MSG00011)

            '-------------------------------
            '--- Check State whether exists
            '-------------------------------
            If Not blnDemo Then
                If strState IsNot Nothing Then
                    'Check state code whether issued from EHS
                    dtState = udtiAMSmartBLL.GetiAMSmartState(strState)

                    'If state code is not existed, terminates the process
                    If dtState Is Nothing Then
                        udtAuditLog.AddDescripton("State", strState)
                        udtAuditLog.AddDescripton("Info", "State is not issued at EHS")
                        udtAuditLog.WriteLog(LogID.LOG00012, AuditLogMsg.MSG00012)

                        Return Nothing
                    End If

                End If
            End If

            '-----------------------------
            '--- Check Return Value
            '-----------------------------
            If Not String.IsNullOrEmpty(strState) Then strURL.Append(Constants.PARAM_STATE).Append(Constants.URL_EQUAL).Append(strState)
            If Not String.IsNullOrEmpty(strCode) Then strURL.Append(Constants.URL_APPEND).Append(Constants.PARAM_CODE).Append(Constants.URL_EQUAL).Append(strCode)
            If Not String.IsNullOrEmpty(strErrorCode) Then strURL.Append(Constants.URL_APPEND).Append(Constants.PARAM_ERROR_CODE).Append(Constants.URL_EQUAL).Append(strErrorCode)

            '---------------------------------
            '--- Start Process Auth Callback
            '---------------------------------
            Dim blnValid As Boolean = True

            udtAuditLog.WriteLog(LogID.LOG00013, AuditLogMsg.MSG00013)

            '---------------------------------
            '--- Demo Version
            '---------------------------------
            If blnDemo Then
                udtAuditLog.WriteLog(LogID.LOG00019, AuditLogMsg.MSG00019)

                If strDemoVersion = YesNo.Yes Then
                    ShowRedirectLink(strURL, strRedirectURL)

                Else
                    strURL.Append(Constants.URL_APPEND).Append(Constants.PARAM_COOKIE).Append(Constants.URL_EQUAL).Append("0")
                    strTesterRedirectURL = strTesterRedirectURL & Constants.URL_ACTION & strURL.ToString
                    HttpContext.Current.Response.Redirect(strTesterRedirectURL, False)
                    context.ApplicationInstance.CompleteRequest()

                End If

                Return String.Empty

            End If

            '---------------------------------
            '--- Validation
            '---------------------------------
            If String.IsNullOrEmpty(strCode) OrElse String.IsNullOrEmpty(strState) Then
                blnValid = False

                udtAuditLog.AddDescripton("Failed reason", "State / Code is empty")
                udtAuditLog.WriteLog(LogID.LOG00017, AuditLogMsg.MSG00017)

            End If

            If blnValid Then
                Dim blnDirectLogin As Boolean = IIf(dtState.Rows(0).Item("CookieKey").ToString = "1", True, False)

                If blnDirectLogin Then
                    'iAM Smart Apps direct login
                    Dim dtmNow As DateTime = DateTime.UtcNow
                    Dim dtm1970 As DateTime = New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                    Dim ts As TimeSpan = dtmNow - dtm1970

                    Dim strTokenHMAC As String = String.Empty
                    Dim strTokenTimestamp As String = String.Empty
                    Dim strHMAC As String = String.Empty


                    strTokenHMAC = stateUtils.GetTokenCookieHMAC()
                    strTokenTimestamp = stateUtils.GetTokenCookieTimestamp()

                    udtAuditLog.AddDescripton("CookieState", strState)
                    udtAuditLog.AddDescripton("CookieTokenHMAC", strTokenHMAC)
                    udtAuditLog.AddDescripton("CookieTimestamp", strTokenTimestamp)

                    udtAuditLog.WriteLog(LogID.LOG00014, AuditLogMsg.MSG00014)

                    strHMAC = stateUtils.GenerateHMAC(strState, strTokenTimestamp)

                    ' Check the token generated by state from query string and timestamp from cookie
                    If blnValid Then
                        If String.IsNullOrEmpty(strHMAC) Then
                            blnValid = False

                            udtAuditLog.AddDescripton("Failed reason", "TokenHMAC is empty")
                            udtAuditLog.WriteLog(LogID.LOG00015, AuditLogMsg.MSG00015)
                        End If
                    End If

                    ' Check the generated token whether is the same as token from cookie
                    If blnValid Then
                        If strHMAC <> strTokenHMAC Then
                            blnValid = False

                            udtAuditLog.AddDescripton("Failed reason", "TokenHMAC is not matched")
                            udtAuditLog.AddDescripton("CookieTokenHMAC", strTokenHMAC)
                            udtAuditLog.AddDescripton("GeneerateTokenHMAC", strHMAC)
                            udtAuditLog.WriteLog(LogID.LOG00015, AuditLogMsg.MSG00015)

                            strURL.Append(Constants.URL_APPEND).Append(Constants.PARAM_COOKIE).Append(Constants.URL_EQUAL).Append("0")
                        End If
                    End If

                    ' Check interval called from iAM Smart apps to EHS(S) iAM Smart web service within 30 seconds
                    If blnValid Then
                        If (CLng(ts.TotalMilliseconds) - CLng(strTokenTimestamp)) > 30000 Then
                            blnValid = False

                            udtAuditLog.AddDescripton("Failed reason", "Timeout")
                            udtAuditLog.AddDescripton("Expected Interval(second)", "30")
                            udtAuditLog.AddDescripton("Actual Interval(second)", CStr((CLng(ts.TotalMilliseconds) - CLng(strTokenTimestamp)) / 1000.0))
                            udtAuditLog.WriteLog(LogID.LOG00015, AuditLogMsg.MSG00015)

                            strURL.Append(Constants.URL_APPEND).Append(Constants.PARAM_COOKIE).Append(Constants.URL_EQUAL).Append("0")
                        End If
                    End If

                    ' strHMAC = strTokenHMAC AndAlso (CLng(ts.TotalMilliseconds) - CLng(strTokenTimestamp)) < 30000 
                    If blnValid Then
                        strURL.Append(Constants.URL_APPEND).Append(Constants.PARAM_COOKIE).Append(Constants.URL_EQUAL).Append("1")
                    End If

                Else
                    'Website Login
                    udtAuditLog.WriteLog(LogID.LOG00016, AuditLogMsg.MSG00016)

                    strURL.Append(Constants.URL_APPEND).Append(Constants.PARAM_COOKIE).Append(Constants.URL_EQUAL).Append("0")

                End If

            End If

            udtAuditLog.AddDescripton("RedirectURL", Constants.ESERVICE_CORE_URL & Constants.ESERVICE_CORE_Page & Constants.URL_ACTION & strURL.ToString)
            udtAuditLog.WriteLog(LogID.LOG00018, AuditLogMsg.MSG00018)

            If strDemoVersion = YesNo.No Then
                'e.g. strRedirectURL = "https://ehss2.hadev.org.hk/HCSP_iamSmart" & Constants.ESERVICE_CORE_Page & Constants.URL_ACTION & strURL.ToString
                strRedirectURL = Constants.ESERVICE_CORE_URL & Constants.ESERVICE_CORE_Page & Constants.URL_ACTION & strURL.ToString

                HttpContext.Current.Response.Redirect(strRedirectURL, False)
                context.ApplicationInstance.CompleteRequest()
            End If

            Return String.Empty

            '' Error Code: D40000 - User cancelled authentication request. Go back to Online Service
            'If Not String.IsNullOrEmpty(strErrorCode) AndAlso strErrorCode = Constants.FAILED_TO_AUTH_USER_CANCELD Then

            '    'strRedirectURL = Constants.ESERVICE_CORE_PAGE & Constants.URL_ACTION & Constants.PARAM_CODE & Constants.URL_EQUAL & strErrorCode & Constants.URL_APPEND & Constants.PARAM_STATE & Constants.URL_EQUAL & strState
            '    If strDemoVersion = YesNo.No Then
            '        Dim list As List(Of KeyValuePair(Of String, String)) = New List(Of KeyValuePair(Of String, String))
            '        list.Add(New KeyValuePair(Of String, String)("ChrisYIM", "https://eh5-chrisyimso/HCSP"))
            '        list.Add(New KeyValuePair(Of String, String)("NicholeIP", "https://eh5-nicholeip/HCSP_Dev"))

            '        For Each pair As KeyValuePair(Of String, String) In list
            '            HttpContext.Current.Response.Write("<br><br> Rediect to <b>" & pair.Key & "</b> machine <br> <a href='" & pair.Value & strRedirectURL & "'>" & pair.Value & strRedirectURL & "</a>")
            '        Next
            '        udtAuditLog.WriteLog(LogID.LOG00000, "[iAMSmart>EHS] AuthCallback: Demo version")
            '    Else
            '        strRedirectURL = Constants.ESERVICE_CORE_URL & Constants.ESERVICE_CORE_Page & strURL.ToString
            '        udtAuditLog.WriteLog(LogID.LOG00000, "[iAMSmart>EHS] AuthCallback: Process function fail")
            '    End If
            '    udtAuditLog.AddDescripton("RedirectURL", strRedirectURL)

            'Else 'D00000 or others
            '    If Not String.IsNullOrEmpty(strCode) AndAlso Not String.IsNullOrEmpty(strState) Then
            '        Dim dtmNow As DateTime = DateTime.UtcNow
            '        Dim dtm1970 As DateTime = New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            '        Dim timespan As TimeSpan = dtmNow - dtm1970

            '        Dim strTokenTimestamp As String = String.Empty
            '        Dim strHMAC As String = String.Empty
            '        Dim stateUtils = New StateUtils()
            '        strTokenTimestamp = stateUtils.GetTokenCookieTimestamp()
            '        strHMAC = stateUtils.GenerateHMAC(strState, strTokenTimestamp)

            '        Dim strTokenHMAC As String = String.Empty
            '        strTokenHMAC = stateUtils.GetTokenCookieHMAC()

            '        udtAuditLog.AddDescripton("HMAC", strHMAC)
            '        udtAuditLog.AddDescripton("TokenHMAC", strTokenHMAC)
            '        udtAuditLog.AddDescripton("TokenTimestamp", strTokenTimestamp)
            '        udtAuditLog.AddDescripton("TimespanTotalMilliseconds", timespan.TotalMilliseconds)
            '        udtAuditLog.WriteLog(LogID.LOG00000, "[iAMSmart>EHS] AuthCallback: Prepare HMAC & Token HMAC")

            '        If String.IsNullOrEmpty(strHMAC) = False AndAlso String.IsNullOrEmpty(strTokenHMAC) = False Then
            '            '30 seconds
            '            If strHMAC = strTokenHMAC AndAlso (CLng(timespan.TotalMilliseconds) - CLng(strTokenTimestamp)) < 30000 Then
            '                'strRedirectURL = Constants.ESERVICE_CORE_PAGE & Constants.URL_ACTION & Constants.PARAM_CODE & Constants.URL_EQUAL & strCode & Constants.URL_APPEND & Constants.PARAM_STATE & Constants.URL_EQUAL & strState & Constants.URL_APPEND & Constants.PARAM_COOKIE & Constants.URL_EQUAL & "1"
            '                strURL.Append(Constants.URL_APPEND).Append(Constants.PARAM_COOKIE).Append(Constants.URL_EQUAL).Append("1")
            '            Else
            '                'strRedirectURL = Constants.ESERVICE_CORE_PAGE & Constants.URL_ACTION & Constants.PARAM_CODE & Constants.URL_EQUAL & strCode & Constants.URL_APPEND & Constants.PARAM_STATE & Constants.URL_EQUAL & strState & Constants.URL_APPEND & Constants.PARAM_COOKIE & Constants.URL_EQUAL & "0"
            '                strURL.Append(Constants.URL_APPEND).Append(Constants.PARAM_COOKIE).Append(Constants.URL_EQUAL).Append("0")
            '            End If
            '        Else
            '            'private mode/ iOS with Chrome
            '            ' strRedirectURL = Constants.ESERVICE_CORE_PAGE & Constants.URL_ACTION & Constants.PARAM_CODE & Constants.URL_EQUAL & strCode & Constants.URL_APPEND & Constants.PARAM_STATE & Constants.URL_EQUAL & strState & Constants.URL_APPEND & Constants.PARAM_COOKIE & Constants.URL_EQUAL & "0"
            '            strURL.Append(Constants.URL_APPEND).Append(Constants.PARAM_COOKIE).Append(Constants.URL_EQUAL).Append("0")
            '        End If

            '        If strDemoVersion = YesNo.Yes Then
            '            Dim list As List(Of KeyValuePair(Of String, String)) = New List(Of KeyValuePair(Of String, String))
            '            list.Add(New KeyValuePair(Of String, String)("ChrisYIM", "https://eh5-chrisyimso/HCSP"))
            '            list.Add(New KeyValuePair(Of String, String)("NicholeIP", "https://eh5-nicholeip/HCSP_Dev"))

            '            For Each pair As KeyValuePair(Of String, String) In list
            '                HttpContext.Current.Response.Write("<br><br> Rediect to <b>" & pair.Key & "</b> machine <br> <a href='" & pair.Value & strRedirectURL & "'>" & pair.Value & strRedirectURL & "</a>")
            '            Next

            '        Else
            '            'strRedirectURL = Constants.ESERVICE_CORE_URL & strRedirectURL
            '            strRedirectURL = Constants.ESERVICE_CORE_URL & Constants.ESERVICE_CORE_Page & strURL.ToString
            '        End If

            '        udtAuditLog.AddDescripton("RedirectURL", strRedirectURL)
            '        udtAuditLog.WriteLog(LogID.LOG00000, "[iAMSmart>EHS] AuthCallback: Build redriect URI")
            '    End If

            'End If

            'udtAuditLog.WriteLog(LogID.LOG00000, "[iAMSmart>EHS] Handle AuthCallback End")
            'If strDemoVersion <> YesNo.Yes Then

            '    context.Response.Redirect(strRedirectURL)
            'End If
            ''context.Response.AppendHeader("Refresh", "5;" & strRedirectURL)
            'Return String.Empty

        End Function

        Public Function GetState(ByVal context As HttpContext) As String
            Dim udtAuditLog As New AuditLogEntry(FunctCode.FUNT070501, DBFlagStr.DBFlagInterfaceLog)
            Dim udtiAMSmartBLL As New iAMSmart.iAMSmartBLL

            Me.OnInit()
            udtAuditLog.WriteStartLog(LogID.LOG00003, AuditLogMsg.MSG00003)

            Dim redirectURI = context.Request.QueryString("redirectURI")
            Dim eserviceName = context.Request.QueryString("eserviceName")
            Dim ClientID = context.Request.QueryString("ClientID")
            Dim scope = context.Request.QueryString("scope")

            Dim stateUtils = New StateUtils()
            Dim state As String = String.Empty

            'udtAuditLog.WriteLog("State", state)

            'If Not String.IsNullOrEmpty(redirectURI) Then
            'state = stateUtils.HandleDirectLoginState()
            'udtiAMSmartBLL.AddiAMSmartState(state, 1, DateTime.Now)

            'redirectURI = CommonHelper.UrlDecode(redirectURI)
            'Dim containsAction = redirectURI.Contains(Constants.URL_ACTION)

            'Dim redirectUrl As String = String.Empty
            'Dim redirect As StringBuilder = New StringBuilder()

            'redirect.Append(redirectURI).Append(If(containsAction, Constants.URL_APPEND, Constants.URL_ACTION)).Append(Constants.PARAM_STATE).Append(Constants.URL_EQUAL).Append(state) _
            '    .Append(Constants.URL_APPEND).Append("clientID").Append(Constants.URL_EQUAL).Append(If(String.IsNullOrEmpty(ClientID), AuthConstants.CLIENTID, ClientID))

            'If Not String.IsNullOrEmpty(scope) Then
            '    redirect.Append(Constants.URL_APPEND).Append("scope").Append(Constants.URL_EQUAL).Append(scope)
            'End If

            'If Not String.IsNullOrEmpty(eserviceName) Then
            '    redirect.Append(Constants.URL_APPEND).Append("eserviceName").Append(Constants.URL_EQUAL).Append(eserviceName)
            'End If

            'udtAuditLog.WriteLog(LogID.LOG00000, "[iAMSmart>EHS] GetState: redirect" + redirect.ToString())

            'redirectUrl = redirectUrl + URLEncoderUtils.GetSaveStateTargetUrl(redirect.ToString())

            'udtAuditLog.AddDescripton("redirectURI", redirectURI)
            'udtAuditLog.AddDescripton("eserviceName", eserviceName)
            'udtAuditLog.AddDescripton("ClientID", ClientID)
            'udtAuditLog.AddDescripton("scope", scope)
            'udtAuditLog.AddDescripton("state", state)
            'udtAuditLog.WriteLog(LogID.LOG00000, "[iAMSmart>EHS] GetState: Pass to OGCIO")

            'udtAuditLog.WriteLog("RedirectUrl", redirectUrl)
            'udtAuditLog.WriteEndLog("End GetState")

            'udtAuditLog.WriteEndLog(LogID.LOG00000, "[iAMSmart>EHS] GetState: GetState from direct login end ")

            'HttpContext.Current.Response.Redirect(redirectUrl)
            'Else
            '    udtAuditLog.WriteLog(LogID.LOG00000, "[iAMSmart>EHS] GetState: redirect URI is empty.")
            '    udtAuditLog.WriteEndLog(LogID.LOG00000, "[iAMSmart>EHS] GetState from direct login End")
            'End If

            If Not String.IsNullOrEmpty(redirectURI) Then
                state = stateUtils.HandleDirectLoginState()
                udtiAMSmartBLL.AddiAMSmartState(state, "1", DateTime.Now)

                udtAuditLog.WriteStartLog(LogID.LOG00004, AuditLogMsg.MSG00004 & redirectURI)

                Dim containsAction = redirectURI.Contains(Constants.URL_ACTION)

                Dim redirectUrl As String = String.Empty
                Dim redirect As StringBuilder = New StringBuilder()

                redirect.Append(redirectURI).Append(If(containsAction, Constants.URL_APPEND, Constants.URL_ACTION)).Append(Constants.PARAM_STATE).Append(Constants.URL_EQUAL).Append(state) _
                    .Append(Constants.URL_APPEND).Append("clientID").Append(Constants.URL_EQUAL).Append(If(String.IsNullOrEmpty(ClientID), AuthConstants.CLIENTID, ClientID))

                If Not String.IsNullOrEmpty(scope) Then
                    redirect.Append(Constants.URL_APPEND).Append("scope").Append(Constants.URL_EQUAL).Append(scope)
                End If

                If Not String.IsNullOrEmpty(eserviceName) Then
                    redirect.Append(Constants.URL_APPEND).Append("eserviceName").Append(Constants.URL_EQUAL).Append(eserviceName)
                End If
 
                redirectUrl = redirectUrl + URLEncoderUtils.GetSaveStateTargetUrl(redirect.ToString())

                udtAuditLog.AddDescripton("redirectURI", redirectURI)
                udtAuditLog.AddDescripton("eserviceName", eserviceName)
                udtAuditLog.AddDescripton("ClientID", ClientID)
                udtAuditLog.AddDescripton("scope", scope)
                udtAuditLog.AddDescripton("state", state)
                udtAuditLog.WriteLog(LogID.LOG00005, AuditLogMsg.MSG00005)

               
                udtAuditLog.WriteEndLog(LogID.LOG00006, AuditLogMsg.MSG00006)
                HttpContext.Current.Response.Redirect(redirectUrl, False)
                context.ApplicationInstance.CompleteRequest()
            Else
                udtAuditLog.WriteLog(LogID.LOG00007, AuditLogMsg.MSG00007)
                udtAuditLog.WriteEndLog(LogID.LOG00008, AuditLogMsg.MSG00008)
            End If

            Return state

        End Function

        Public Function HandleProfileCallback(ByVal context As HttpContext) As String
            ' WriteToFile(context.ToString)
            Dim blnValid As Boolean = True
            Dim udtiAMSmartBLL As iAMSmartBLL = New iAMSmartBLL()
            Dim udtAuditLog As New AuditLogEntry(FunctCode.FUNT070501, DBFlagStr.DBFlagInterfaceLog)
            Dim strRequestContent As String = String.Empty
            Dim udtCallBackDto As eService.DTO.Request.ResCallbackDTO = Nothing

            Dim TxID As String = String.Empty
            Dim Code As String = String.Empty
            Dim Message As String = String.Empty
            Dim Content As String = String.Empty
            Dim secretKey As String = String.Empty
            Dim aesKey As String = String.Empty
            Dim DecryptedData As String = String.Empty

            Me.OnInit()
            udtAuditLog.WriteStartLog(LogID.LOG00020, AuditLogMsg.MSG00020)

            '------------------------------
            '-- Get the whole request
            '------------------------------
            Using sr As New System.IO.StreamReader(context.Request.InputStream)
                strRequestContent = sr.ReadToEnd
            End Using

            '-----------------------------
            '-- Fill the CallbackDTO
            '-----------------------------
            udtCallBackDto = JsonUtils.Deserialize(Of ResCallbackDTO)(strRequestContent)
            'udtAuditLog.AddDescripton("udtCallBackDto", udtCallBackDto)
            'udtAuditLog.WriteStartLog(LogID.LOG00021, AuditLogMsg.MSG00021)
            If udtCallBackDto IsNot Nothing Then
                TxID = udtCallBackDto.TxID
                Code = udtCallBackDto.Code
                Message = udtCallBackDto.Message
                Content = udtCallBackDto.Content
                secretKey = udtCallBackDto.SecretKey

                udtAuditLog.AddDescripton("TxID", TxID)
                udtAuditLog.AddDescripton("Code", Code)
                udtAuditLog.AddDescripton("Message", Message)
                udtAuditLog.AddDescripton("Content", Content)
                udtAuditLog.AddDescripton("secretKey", secretKey)
                udtAuditLog.WriteStartLog(LogID.LOG00022, AuditLogMsg.MSG00022)

                'Content = "AAAADHTk5u4DBNALmHSrLzosRLlSa+3GA8bpZ2EQNVczDaLthzFh+Zx9G6q2ucyY3Tp8VDfWpix2mPQnz0+GWXdSZUgOyhy03gXfMd6iChlsd/23DSk44IEztbWguZAG4i8tjZbeAZu+hNITdEW2O0s3JYgxviem/HfzTFwCAfRI9uVsTQQ9FUYT+FlH+CIxaf5z+A4/CAwOdT4HABo6bliObra0F+mCdn2En5Es5Z9zbMS8thfBS/rQsqGZkoItbJfNtbNba3TXGotSNqgTk7SNQ5cQZ7H+jCbmfHud65UWFQ/obGteM5BzWXo90dqXj/ZAoh6nFi+DCw+AF+C0WglYU9v/SwlnyiW41/VaoQ=="
                ' secretKey = "xbd/xLQA+NcbxiL3HU1fNc5S3RJOz/cpf8yfwmSbJr82bg6LKaC8PTh3QxM5anhWnkgIC+qJkGH7JsFdnlXkZXpH/H1GxKKjV3lHWKus/PWkue6+0uFXeIOTFHo8ilKrL75iLqJhcIwWsBAcllguNcJ4NeOnU3m/JoqfRtjH4k9ym1w804e6SHplK0PCIMF4EZG3L+i8zVHldTUXGb1ZNVA2DihTg+vvDOuUPtzNsUu1Y9AvXWouZ6CFTxbqQFdN7vItkfaR6ExeEYxPNfysY4bdyp2DAzDneCKfmhA+8B5KZlKlfQy6IitV1S4a03c9mGMFLHstZ1v13Io0N6vOIQ=="

                '-----------------------------
                '-- Get the AESKey & decrypted the content
                '----------------------------
                'Dim privatekey As String = "MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQDXmmnrJwencBEJCtrU62dpMQdXw+IAlGub17VAYuzMbf6MzbXPg/m1rHSpyGCbkTr2WDtzXRvisSLAKf8Rcventd5p2cc1IPwVR8rTRaUZ/BYKUHfajsAoMv3BWk/smgBnKuMRUa9GfXB3beMzaUvaG2PT5ir11z7TCtUpFuMqxlFzNt9MYFY5t24k11abL4f6U8UeCPLI2zxntL/aw6d+gsX8Y/rwUscD2kg3hQ5X37xA8fDwwIQ/rEFy/YGL0uM/6N55wG7HdmZyborVot1iVuNzvZUcx0wIw2kr7siyyt7nZHj94iqirobIZie9bx2lB5CdGunjHGu3okPsF0IlAgMBAAECggEAQEu0OoQxLCWnbuLlz4lEFYKhhfLMew/H53m0e7cElxJWTvcpuPDKjvsW72rqJHpy8vPtBsJFhpz8mAHuJu36HPGHmllvptBnXJFEUyz5x339tKW6mFrpwxMxyW99Vjm21Q1dGdUaSBMwbu0+TB6imDCC3LZPKXE/MwCQAOjqZd6h2rnC8SWv1G6Oy32y63t6mFgAPLuhVJV5/2uGZlqgLGjhv6iSmkFlPzUsH4/QEV1k45lR+6dAHEWpiMhJynrmNCMDjvzQZLH8yqSY5Ag99LbpcHXaf+HEtRsc9wlmwss6hvEmnMjDS0L5ecekdrvlYClnUUbVxn0hxMeXoB1qIQKBgQD/vYud6TTF2YncUsDEmLueo1MVucAmGmyWfRq9t50wVwEDGs+SerEhz85V6dO19GhI/KEj9lnr6Ec8F1lSb4qm74wjoWxT5Wdzse+ZosBBU404ZYtSDDx1FMvCVb6K9XcENVztGwVTk9LHzv5zYpXNR7bhxbn2zRf/ITqBp2vI2wKBgQDX0nBKJs2o+TD8QnfV5cLynbONe5oJWfzdmaHKkw07BJknQxupSXZ6i9QM26FsEs/qS1wmTFRq0RWDwQ2sTBwBb10nK7xg3Mmpwgys+QbXMuBTq2OvBvNb8fqPBXhCnBVUiW2J0UQpQ8yy90tvNxxuEk1WJ5EPagUngaOj/O2Q/wKBgGrvy03AkjyY9jiWsdyDMRDoonlXLq3AJt/WeDQUbzojZ1zw/RL3EtJcnp72I3zWMjUyzf7HqFooyoCqEsORuJyNVkAf47hBGL0cDmBj2Zh3Y3nNnDA2xaD/jSQ5zm80rQupdK4Aun10NpMuTt/YvWMVNbe6gkRdBke8kUZdx4g5AoGBAMzzjqccSlmYqygjhuGdm5ACqqxFIHaY+kUGzFovY3UUXMyoMfCFTvn5J5o+SAD1rHR2tD+6ZMA7Zg8EmhNxA3dpZjD4m5/wi6GmIZtxsUlTiyxpqfRgyWIbXTHMo/O9fZQWHeuuyufU9uNNbFllIJ52yvJyCJN2vwZRWjE00/XJAoGAJictzDgv8c5lxP00QTBf1ygebgl4vtPHULirdrXvC8srWzaMvSQ6LHBiBYO7SnHP9YmpXylHSzDkGmbMmhZdB7/Fujx3qGaDCzVrE8lhjfWZ/OTiE/83oyGEAYQyma3T00jRekR8NCeRTg/9tx4r3FusuqawtUO898NjGok5ckk="

                aesKey = EncryptUtils.DecryptByPrivateKey(secretKey, eService.Common.GetPrivateKeyUtils.GetPrivateKey)
                ' aesKey = EncryptUtils.DecryptByPrivateKey(secretKey, privatekey)
                udtAuditLog.AddDescripton("aesKey", aesKey)
                udtAuditLog.WriteStartLog(LogID.LOG00023, AuditLogMsg.MSG00023)

                DecryptedData = EncryptUtils.AESDecrypt(Content, aesKey, 4)

                Dim DecryptResDTO As ResponseDTO(Of String) = New ResponseDTO(Of String)(TxID, DecryptedData)

                Dim callBackData = DecryptResDTO.Content
                Dim callBackDataObj = JObject.Parse(callBackData)
                Dim callBackDataContent = JsonUtils.GetJsonStringValue(callBackDataObj, "content")
                Dim callBackDataDTO = JsonUtils.Deserialize(Of ReqProfileCallbackDTO)(callBackDataContent)

                ' Error Code: D40000 - User cancelled authentication request. Go back to Online Service
                If Not String.IsNullOrEmpty(Code) AndAlso Not Code = Constants.FAILED_TO_AUTH_USER_CANCELD Then
                    udtAuditLog.AddDescripton("Code", Code)
                    udtAuditLog.WriteStartLog(LogID.LOG00024, AuditLogMsg.MSG00024)

                    blnValid = udtiAMSmartBLL.UpdateiAMSmartProfileLog(callBackDataDTO.BusinessID, callBackDataDTO.State, Code, Message, callBackDataContent)
                    If blnValid Then
                        'D00000 or others
                        'wait long polling
                    Else
                        'update iamSmartProfile fail 
                        'redirect to iamsmartlogin.aspx & go to login
                    End If
                Else
                    udtAuditLog.AddDescripton("Code", Code)
                    udtAuditLog.WriteStartLog(LogID.LOG00025, AuditLogMsg.MSG00025)
                    'cancel submit profile
                End If
            End If

            udtAuditLog.WriteEndLog(LogID.LOG00026, AuditLogMsg.MSG00026)

            Return String.Empty
        End Function

#End Region

#Region "Supporting Functions"
        Sub ShowRedirectLink(ByVal strURL As StringBuilder, ByVal strRedirectURL As String)
            Dim strDemoVersion As String = ConfigurationManager.AppSettings("DemoVersion")
            Dim strTesterRedirectLink As String = ConfigurationManager.AppSettings("TesterRedirectLink")

            strURL.Append(Constants.URL_APPEND).Append(Constants.PARAM_COOKIE).Append(Constants.URL_EQUAL).Append("0")

            Dim list As List(Of KeyValuePair(Of String, String)) = New List(Of KeyValuePair(Of String, String))

            list.Add(New KeyValuePair(Of String, String)("Tester", strTesterRedirectLink))

            For intCt As Integer = 1 To 10
                Dim strVarName As String = String.Format("DemoVersionRedirectLink_{0}", intCt)
                Dim strVarLink As String = ConfigurationManager.AppSettings(strVarName)

                If Not String.IsNullOrEmpty(strVarLink) Then
                    Dim strVar() As String = strVarLink.Split("|")

                    If strVar.Length > 0 Then
                        list.Add(New KeyValuePair(Of String, String)(strVar(0), strVar(1)))
                    End If
                End If

            Next

            'list.Add(New KeyValuePair(Of String, String)("ChrisYIM", "https://eh3-chrisyim/HCSP"))
            'list.Add(New KeyValuePair(Of String, String)("WinnieSuen", "https://eh5-winniesuen/HCSP_Dev"))
            'list.Add(New KeyValuePair(Of String, String)("23X", "https://ehss1.hadev.org.hk/HCSP_iamSmart"))
            'list.Add(New KeyValuePair(Of String, String)("24X", "https://ehss2.hadev.org.hk/HCSP_iamSmart"))

            For Each pair As KeyValuePair(Of String, String) In list
                'HttpContext.Current.Response.Write("<br><br> Rediect to <b>" & pair.Key & "</b> machine <br> <a href='" & pair.Value & strRedirectURL & Constants.ESERVICE_CORE_Page & Constants.URL_ACTION & strURL.ToString & "'>" & pair.Value & strRedirectURL & Constants.ESERVICE_CORE_Page & Constants.URL_ACTION & strURL.ToString & "</a>")
                HttpContext.Current.Response.Write("<br><br> Rediect to <b>" & pair.Key & "</b> machine <br> <a href='" & pair.Value & strRedirectURL & Constants.URL_ACTION & strURL.ToString & "'>" & _
                                                   pair.Value & strRedirectURL & Constants.URL_ACTION & strURL.ToString & "</a>")
            Next

        End Sub
#End Region

    End Class

End Namespace
