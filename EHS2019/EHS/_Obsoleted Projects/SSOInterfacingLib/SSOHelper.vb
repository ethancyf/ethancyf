Imports System
Imports Common.Component
Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess
Imports Common.Format
Imports Common.Component.Scheme
Imports Common
Imports Common.ComObject
Imports System.Web
Imports System.Configuration


Public Class SSOHelper

    Private Shared strLocalSSOAppId As String = String.Empty
    Private Shared strSSORelatedAppIds As String = String.Empty
    Private Shared strSSOEnableInfoLog As String = String.Empty
    Private Const ConnectionString_SSO As String = DBFlagStr.DBFlag4
    'Private Shared FunctionCode As String = FunctCode.FUNT050101

    'Private Shared Sub loadConfig()

    '    Dim strSSOErrCode As String = ""

    '    Dim objSSOSetting As System.Configuration.ClientSettingsSection = CType(System.Configuration.ConfigurationManager.GetSection("applicationSettings/SSO.Properties.Settings"), System.Configuration.ClientSettingsSection)

    '    If (strLocalSSOAppId = String.Empty) Then

    '        If (objSSOSetting.Settings.Get("SSO_App_Id") Is Nothing) Then
    '            strSSOErrCode = "SSO_APP_ID_NOT_DEFINED"
    '            SSOInterfacingLib.SSOHelper.writeAppErrLog(System.DateTime.Now + ": " + strSSOErrCode + ". Error at SSOInterfacingLib.SSOHelper.loadConfig().")
    '        Else
    '            strLocalSSOAppId = objSSOSetting.Settings.Get("SSO_App_Id").Value.ValueXml.InnerText
    '        End If
    '    End If

    '    'Enable SSO Info Log
    '    Dim strSSOEnableInfoLogFromDB As String = Nothing
    '    strSSOEnableInfoLogFromDB = getAppConfig("SSO_Enable_Info_Log")
    '    If strSSOEnableInfoLogFromDB Is Nothing Then
    '        If (strSSOEnableInfoLog = String.Empty) Then
    '            If (objSSOSetting.Settings.Get("SSO_Enable_Info_Log") Is Nothing) Then
    '                strSSOErrCode = "SSO_ENABLE_INFO_LOG_NOT_DEFINED"
    '                SSOInterfacingLib.SSOHelper.writeAppErrLog(System.DateTime.Now + ": " + strSSOErrCode + ". Error at SSOInterfacingLib.SSOHelper.loadConfig().")
    '            Else
    '                strSSOEnableInfoLog = objSSOSetting.Settings.Get("SSO_Enable_Info_Log").Value.ValueXml.InnerText
    '            End If
    '        End If
    '    Else
    '        strSSOEnableInfoLog = strSSOEnableInfoLogFromDB
    '    End If


    '    'Tailored for SSO_Related_App_Ids
    '    If (strSSORelatedAppIds = String.Empty) Then
    '        If (objSSOSetting.Settings.Get("SSO_Related_App_Ids") Is Nothing) Then
    '            strSSOErrCode = "SSO_RELATED_APP_IDS_NOT_DEFINED"
    '            SSOInterfacingLib.SSOHelper.writeAppErrLog(System.DateTime.Now + ": " + strSSOErrCode + ". Error at SSOInterfacingLib.SSOHelper.loadConfig().")
    '        Else
    '            strSSORelatedAppIds = objSSOSetting.Settings.Get("SSO_Related_App_Ids").Value.ValueXml.InnerText
    '        End If
    '    End If

    'End Sub


    '''' <summary>
    '''' For each Replying Application, it must implement this function to provide customized data
    '''' to the target applications
    '''' </summary>
    '''' <param name="strSSOTargetSiteSSOAppId">the target application the local application want to Single Sign-On to</param>
    '''' <returns>
    '''' an non-empty objSSOCustomizedContent for other applicaton.
    '''' Returning an null value or SSOCustomizedContent containing empty content will make accessing to the tarhet application failed
    '''' </returns>
    '''' <remarks></remarks>
    'Public Shared Function generateSSOCustomizedContent(ByVal strSSOTargetSiteSSOAppId As String) As SSODataType.SSOCustomizedContent

    '    loadConfig()

    '    Dim objSSOCustomizedContent As SSODataType.SSOCustomizedContent = New SSODataType.SSOCustomizedContent()

    '    If (strSSOTargetSiteSSOAppId.Trim().ToUpper() = "PPI_APP") Then

    '        'objSSOCustomizedContent.addEntry("UserID", System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_UserId") + strSSOTargetSiteSSOAppId.Substring(0, 1))
    '        objSSOCustomizedContent.addEntry("UserHKID", System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_HKID"))
    '        objSSOCustomizedContent.addEntry("UserTokenSerialNo", System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_TokenSerialNo"))
    '        objSSOCustomizedContent.addEntry("UserCommonInfo", System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_Language"))

    '    ElseIf (strSSOTargetSiteSSOAppId.Trim().ToUpper() = "EHS") Then

    '        objSSOCustomizedContent.addEntry("UserHKID", System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_HKID"))
    '        objSSOCustomizedContent.addEntry("UserTokenSerialNo", System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_TokenSerialNo"))
    '        objSSOCustomizedContent.addEntry("UserCommonInfo", System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_Language"))

    '    End If

    '    Return objSSOCustomizedContent

    'End Function

    '''' <summary>
    '''' Create auit logs for SSO
    '''' </summary>
    '''' <param name="objSSOAuditLog">an object containing the data to be logged</param>
    '''' <returns>
    '''' if success, return an integer >0
    '''' if failed, return an integer = 0 or less than 0
    '''' </returns>
    '''' <remarks></remarks>
    'Public Shared Function insertSSOAuditLogs(ByVal objSSOAuditLog As SSODataType.SSOAuditLog) As Integer

    '    Dim udtdb As Database = New Database(ConnectionString_SSO)
    '    Dim intStatus As Integer = -1

    '    Try

    '        Dim parms() As SqlParameter = { _
    '                                    udtdb.MakeInParam("@v_in_txn_id", SqlDbType.VarChar, 255, objSSOAuditLog.TxnId), _
    '                                    udtdb.MakeInParam("@v_in_msg_type", SqlDbType.VarChar, 255, objSSOAuditLog.MsgType), _
    '                                    udtdb.MakeInParam("@v_in_source_site", SqlDbType.VarChar, 255, objSSOAuditLog.SourceSite), _
    '                                    udtdb.MakeInParam("@v_in_target_site", SqlDbType.VarChar, 255, objSSOAuditLog.TargetSite), _
    '                                    udtdb.MakeInParam("@v_in_artifact", SqlDbType.VarChar, 255, convertDBNullPara(objSSOAuditLog.Artifact)), _
    '                                    udtdb.MakeInParam("@v_in_plain_assertion", SqlDbType.VarChar, 8000, convertDBNullPara(objSSOAuditLog.PlainAssertion)), _
    '                                    udtdb.MakeInParam("@v_in_secured_assertion", SqlDbType.Text, 200000, convertDBNullPara(objSSOAuditLog.SecuredAssertion)), _
    '                                    udtdb.MakeInParam("@v_in_plain_artifact_resolve_req", SqlDbType.Text, 200000, convertDBNullPara(objSSOAuditLog.PlainArtifactResolveReq)), _
    '                                    udtdb.MakeInParam("@v_in_secured_artifact_resolve_req", SqlDbType.Text, 200000, convertDBNullPara(objSSOAuditLog.SecuredArtifactResolveReq)), _
    '                                    udtdb.MakeInParam("@v_in_creation_datetime", SqlDbType.DateTime, 8, objSSOAuditLog.CreationDatetime)}


    '        udtdb.RunProc("proc_ins_sso_audit_logs", parms)
    '        intStatus = 1

    '    Catch ex As SqlException

    '        Throw ex

    '    Catch ex As Exception

    '        Throw ex

    '    End Try

    '    Return intStatus
    '    'Return 1

    'End Function

    'Private Shared Function convertDBNullPara(ByVal strPara As Object) As Object

    '    Dim objReturn As Object = Nothing

    '    If (strPara Is Nothing) Then

    '        objReturn = DBNull.Value

    '    Else

    '        objReturn = strPara

    '    End If

    '    Return objReturn

    'End Function


    '''' <summary>
    '''' Save assertion to a persistent storage for later verification in SSO
    '''' </summary>
    '''' <param name="strSSOTxnId">the transaction id of the current SSO operation</param>
    '''' <param name="objSSOActiveAssertion">an object containing the assertion data</param>
    '''' <returns>
    '''' if success, return an integer >0
    '''' if failed, return an integer = 0 or less than 0
    '''' </returns>
    '''' <remarks></remarks>
    'Public Shared Function saveSSOActiveAssertion(ByVal strSSOTxnId As String, ByVal objSSOActiveAssertion As SSODataType.SSOActiveAssertion) As Integer

    '    Dim udtdb As Database = New Database(ConnectionString_SSO)

    '    Dim intStatus As Integer = -1

    '    Try

    '        Dim parms() As SqlParameter = { _
    '                                        udtdb.MakeInParam("@v_in_txn_id", SqlDbType.VarChar, 255, strSSOTxnId), _
    '                                        udtdb.MakeInParam("@v_in_artifact", SqlDbType.VarChar, 255, objSSOActiveAssertion.Artifact), _
    '                                        udtdb.MakeInParam("@v_in_assertion", SqlDbType.Text, 200000, objSSOActiveAssertion.Assertion), _
    '                                        udtdb.MakeInParam("@v_in_read_count", SqlDbType.Int, 8, objSSOActiveAssertion.ReadCount), _
    '                                        udtdb.MakeInParam("@v_in_creation_datetime", SqlDbType.DateTime, 8, objSSOActiveAssertion.CreationDateTime)}

    '        udtdb.RunProc("proc_ins_sso_active_assertion", parms)
    '        intStatus = 1

    '    Catch ex As SqlException

    '        Throw ex

    '    Catch ex As Exception

    '        Throw ex

    '    End Try

    '    Return intStatus

    'End Function


    '''' <summary>
    '''' Get SSO assertion from persistent storage by artifact
    '''' </summary>
    '''' <param name="strSrchArtifact">the key to retrive the SSO assertion</param>
    '''' <returns>a SSOAssertion object associated with the artifact, or null if no SSO Assertion is found</returns>
    '''' <remarks></remarks>
    'Public Shared Function getSSOActiveAssertion(ByVal strSrchArtifact As String) As SSODataType.SSOActiveAssertion


    '    Dim objSSOActiveAssertion As SSODataType.SSOActiveAssertion = Nothing

    '    Dim udtdb As Database = New Database(ConnectionString_SSO)
    '    Dim dt As New DataTable

    '    Try

    '        Dim parms() As SqlParameter = { _
    '                        udtdb.MakeInParam("@v_in_artifact", SqlDbType.VarChar, 255, strSrchArtifact)}

    '        udtdb.RunProc("proc_get_sso_active_assertion", parms, dt)

    '        If dt.Rows.Count > 0 Then
    '            Dim dr As DataRow = dt.Rows(0)

    '            objSSOActiveAssertion = New SSODataType.SSOActiveAssertion(CType(dr.Item("txn_id"), String).Trim, _
    '                                                                        CType(dr.Item("artifact"), String).Trim, _
    '                                                                        CType(dr.Item("assertion"), String).Trim, _
    '                                                                        CType(dr.Item("read_count"), Integer), _
    '                                                                        CType(dr.Item("creation_datetime"), DateTime))

    '        End If
    '    Catch ex As SqlException
    '        Throw ex
    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    '    Return objSSOActiveAssertion

    'End Function


    '''' <summary>
    '''' Check if an Assertion Resolve Requet is valid
    '''' the asertion must exist in the persistent storage and the read count should be 0
    '''' </summary>
    '''' <param name="strSSOArtifact">the key to retrive the SSO assertion</param>
    '''' <returns>true for valid Assertion Resolve request, false otherwise</returns>
    '''' <remarks></remarks>
    'Public Shared Function chkSSOActiveAssertionIsValid(ByVal strSSOArtifact As String) As Boolean

    '    Dim intChkRst As Integer = 0
    '    Dim blnChkRst As Boolean = False

    '    Dim udtdb As Database = New Database(ConnectionString_SSO)
    '    Dim dt As New DataTable

    '    Dim parms() As SqlParameter = { _
    '        udtdb.MakeInParam("@v_in_artifact", SqlDbType.VarChar, 255, strSSOArtifact)}

    '    Try

    '        udtdb.RunProc("proc_chk_valid_sso_active_assertion_resolve_req", parms, dt)

    '        If dt.Rows.Count > 0 Then
    '            intChkRst = CType(dt.Rows(0).Item("chk_rst"), Integer)
    '        End If

    '        If (intChkRst = 1) Then
    '            blnChkRst = True
    '        Else
    '            blnChkRst = False
    '        End If

    '    Catch ex As SqlException

    '        Throw ex

    '    Catch ex As Exception

    '        Throw ex

    '    End Try

    '    Return blnChkRst

    'End Function

    '''' <summary>
    '''' Update the read count of an assertion stored in persistent storage by 1
    '''' This is done after a assertion has been read
    '''' </summary>
    '''' <param name="strSrchArtifact">he key to retrive the SSO assertion</param>
    '''' <returns>
    '''' if success, return an integer > 0
    '''' if failed, return an integer = 0 or less than 0
    '''' </returns>
    '''' <remarks></remarks>
    'Public Shared Function updateSSOActiveAssertionReadCountByOne(ByVal strSrchArtifact As String) As Integer

    '    Dim intStatus As Integer = -1

    '    Dim udtdb As Database = New Database(ConnectionString_SSO)

    '    Dim parms() As SqlParameter = { _
    '        udtdb.MakeInParam("@v_in_artifact", SqlDbType.VarChar, 255, strSrchArtifact)}

    '    Try

    '        udtdb.RunProc("proc_upd_sso_active_assertion_read_count", parms)
    '        intStatus = 1

    '    Catch ex As SqlException
    '        Throw ex
    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    '    Return intStatus

    'End Function

    '''' <summary>
    '''' Get application configuration value by application specific logic
    '''' The configuration value get from this function (System Parameters) will override that in web.config
    '''' </summary>
    '''' <param name="strParaName">strParaName is the name of the parameter</param>
    '''' <returns>value of the parameter / return null value will use the configuration in web.config</returns>
    '''' <remarks></remarks>
    'Public Shared Function getAppConfig(ByVal strParaName As String) As String

    '    'There are following variables are retrieved from database. They have higher priority over that retrieved from web config.
    '    '1. SSO_<SSOAppId>_IDP_WS_URL
    '    '2. SSO_<SSOAppId>_Server_Certificate_Thumbprint 
    '    '3. SSO_Enable_Info_Log

    '    Dim strSystemParaValue As String = String.Empty
    '    Dim udcGeneralF As New Common.ComFunction.GeneralFunction

    '    Dim arrRelatedAppIds As String() = strSSORelatedAppIds.Split(",")

    '    Dim intCounter As Integer = 0
    '    While intCounter < arrRelatedAppIds.Length
    '        'For For "SSO_<SSOAppId>_IDP_WS_URL"
    '        If strParaName.Trim.ToUpper = "SSO_" + arrRelatedAppIds(intCounter).Trim.ToUpper + "_IDP_WS_URL" Then
    '            udcGeneralF.getSystemParameter(strParaName, strSystemParaValue, String.Empty)

    '            If Not strSystemParaValue.Trim = "" Then
    '                Return strSystemParaValue
    '            End If
    '        End If

    '        'For SSO_<SSOAppId>_Server_Certificate_Thumbprint 
    '        If strParaName.Trim.ToUpper = "SSO_" + arrRelatedAppIds(intCounter).Trim.ToUpper + "_SERVER_CERTIFICATE_THUMBPRINT" Then
    '            udcGeneralF.getSystemParameter(strParaName, strSystemParaValue, String.Empty)

    '            If Not strSystemParaValue.Trim = "" Then
    '                Return strSystemParaValue
    '            End If
    '        End If

    '        intCounter = intCounter + 1

    '    End While

    '    'For enable SSO audit log 'SSO_Enable_Info_Log'
    '    If strParaName.Trim.ToUpper = "SSO_ENABLE_INFO_LOG" Then
    '        udcGeneralF.getSystemParameter(strParaName, strSystemParaValue, String.Empty)

    '        If Not strSystemParaValue.Trim = "" Then
    '            Return strSystemParaValue
    '        End If

    '    End If

    '    Return Nothing

    'End Function


    '#Region "SSO audit log"
    '    ''' <summary>
    '    ''' Write application logs to a persistent storage when the "Enable Information Log" 
    '    ''' flag is turned on, as indicated in the parameter SSO_Enable_Info_Log
    '    ''' </summary>
    '    ''' <param name="strMsg">the message to be logged</param>
    '    ''' <remarks></remarks>
    '    Public Shared Sub writeAppInfoLog(ByVal strMsg As String)

    '        If (strSSOEnableInfoLog = String.Empty) Then
    '            strSSOEnableInfoLog = "Y"
    '        End If

    '        If (strSSOEnableInfoLog.Trim().ToUpper() = "Y") Then
    '            WriteAuditLogToDB(strMsg)
    '        End If
    '    End Sub

    '    Private Shared Sub WriteAuditLogToDB(ByVal strDescription As String)

    '        Dim strPlatform As String = ConfigurationManager.AppSettings("Platform")
    '        Dim strClientIP As String
    '        Dim strSessionID As String = String.Empty

    '        'Client IP & Session ID
    '        Try
    '            strClientIP = HttpContext.Current.Request.UserHostAddress
    '            strSessionID = HttpContext.Current.Session.SessionID.ToString
    '        Catch ex As Exception
    '            strClientIP = String.Empty
    '            strSessionID = String.Empty
    '        End Try

    '        ' Browser & OS
    '        Dim strBrowser As String = String.Empty
    '        Dim strOS As String = String.Empty

    '        Try
    '            If Not HttpContext.Current.Request.Browser Is Nothing Then
    '                strBrowser = HttpContext.Current.Request.Browser.Type
    '                strOS = HttpContext.Current.Request.Browser.Platform.Trim()
    '            End If
    '        Catch ex As Exception
    '            strBrowser = String.Empty
    '            strOS = String.Empty
    '        End Try


    '        Dim strSPID As String = String.Empty
    '        Try
    '            If Not IsNothing(System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_UserId")) Then
    '                strSPID = System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_UserId").ToString()
    '            End If
    '        Catch
    '            strSPID = String.Empty
    '        End Try

    '        AddAuditLogSSO(strClientIP, strSPID, strDescription, strSessionID, strBrowser, strOS)
    '    End Sub

    '    Private Shared Sub AddAuditLogSSO(ByVal strClientIP As String, ByVal strUserID As String, ByVal strDescription As String, ByVal strSessionID As String, ByVal strBrowser As String, ByVal strOS As String)
    '        'Separate DB for SSO audit logs
    '        Dim db As Database = New Database(ConnectionString_SSO)
    '        Dim strHKID As String = ""

    '        Try
    '            Dim prams() As SqlParameter = { _
    '            db.MakeInParam("@client_ip", SqlDbType.VarChar, 20, strClientIP), _
    '            db.MakeInParam("@user_id", SqlDbType.VarChar, 20, strUserID), _
    '            db.MakeInParam("@description", SqlDbType.NVarChar, 1000, strDescription), _
    '            db.MakeInParam("@session_id", SqlDbType.VarChar, 40, strSessionID), _
    '            db.MakeInParam("@browser", SqlDbType.VarChar, 20, strBrowser), _
    '            db.MakeInParam("@os", SqlDbType.VarChar, 20, strOS)}

    '            db.RunProc("proc_AuditLogSSO_add", prams)

    '        Finally
    '            If Not db Is Nothing Then db.Dispose()
    '        End Try
    '    End Sub
    '#End Region

    '#Region "SSO system log (error log)"

    '    ''' <summary>
    '    ''' Write application logs to a persistent storage when error occurs 
    '    ''' </summary>
    '    ''' <param name="strMsg">the message to be logged</param>
    '    ''' <remarks></remarks>
    Public Shared Sub writeAppErrLog(ByVal strMsg As String)

        '        'Dim objSystemMsgLogger As SSOUtil.SystemMsgLogger = New SSOUtil.SystemMsgLogger()
        '        'objSystemMsgLogger.LogError(strMsg)
        '        WriteSystemLogToDB(strMsg)

    End Sub

    '    Private Shared Sub WriteSystemLogToDB(ByVal strDescription As String)

    '        Dim strPlatform As String = ConfigurationManager.AppSettings("Platform")
    '        Dim strClientIP As String
    '        Dim strSessionID As String = String.Empty

    '        'Client IP & Session ID
    '        Try
    '            strClientIP = HttpContext.Current.Request.UserHostAddress
    '            strSessionID = HttpContext.Current.Session.SessionID.ToString
    '        Catch ex As Exception
    '            strClientIP = String.Empty
    '            strSessionID = String.Empty
    '        End Try

    '        ' Browser & OS
    '        Dim strBrowser As String = String.Empty
    '        Dim strOS As String = String.Empty

    '        Try
    '            If Not HttpContext.Current.Request.Browser Is Nothing Then
    '                strBrowser = HttpContext.Current.Request.Browser.Type
    '                strOS = HttpContext.Current.Request.Browser.Platform.Trim()
    '            End If
    '        Catch ex As Exception
    '            strBrowser = String.Empty
    '            strOS = String.Empty
    '        End Try


    '        Dim strSPID As String = String.Empty
    '        Try
    '            If Not IsNothing(System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_UserId")) Then
    '                strSPID = System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_UserId").ToString()
    '            End If
    '        Catch
    '            strSPID = String.Empty
    '        End Try

    '        AddSystemLogSSO(strClientIP, strSPID, strSessionID, strBrowser, strOS, strDescription)
    '    End Sub

    '    Private Shared Sub AddSystemLogSSO(ByVal strClientIP As String, ByVal strUserID As String, ByVal strSessionID As String, ByVal strBrowser As String, ByVal strOS As String, Optional ByVal strUserDefinedMessage As String = Nothing)
    '        'Separate DB for SSO audit logs
    '        Dim db As Database = New Database(ConnectionString_SSO)
    '        Try
    '            Dim prams(5) As SqlParameter
    '            prams(0) = db.MakeInParam("@client_ip", SqlDbType.VarChar, 20, strClientIP)
    '            prams(1) = db.MakeInParam("@user_id", SqlDbType.VarChar, 20, IIf(strUserID Is Nothing, DBNull.Value, strUserID))
    '            prams(2) = db.MakeInParam("@system_message", SqlDbType.NText, 0, IIf(strUserDefinedMessage Is Nothing, DBNull.Value, strUserDefinedMessage))
    '            prams(3) = db.MakeInParam("@session_id", SqlDbType.VarChar, 40, strSessionID)
    '            prams(4) = db.MakeInParam("@browser", SqlDbType.VarChar, 20, strBrowser)
    '            prams(5) = db.MakeInParam("@os", SqlDbType.VarChar, 20, strOS)

    '            db.RunProc("proc_SystemLogSSO_add", prams)

    '        Finally
    '            If Not db Is Nothing Then db.Dispose()
    '        End Try

    '    End Sub
    '#End Region


End Class


