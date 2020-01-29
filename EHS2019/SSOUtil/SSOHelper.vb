Imports System
Imports System.Web
Imports System.Configuration
Imports System.Xml

Public Class SSOHelper

    Private Shared strLocalSSOAppId As String = String.Empty
    Private Shared strSSORelatedAppIds As String = String.Empty
    Private Shared strSSOEnableInfoLog As String = String.Empty

    Private Shared Sub loadConfig()

        Dim strSSOErrCode As String = ""

        Dim objSSOSetting As System.Configuration.ClientSettingsSection = CType(System.Configuration.ConfigurationManager.GetSection("applicationSettings/SSO.Properties.Settings"), System.Configuration.ClientSettingsSection)

        If (strLocalSSOAppId = String.Empty) Then

            If (objSSOSetting.Settings.Get("SSO_App_Id") Is Nothing) Then
                strSSOErrCode = "SSO_APP_ID_NOT_DEFINED"
                SSOUtil.SSOHelper.writeAppErrLog(System.DateTime.Now + ": " + strSSOErrCode + ". Error at SSOInterfacingLib.SSOHelper.loadConfig().")
            Else
                strLocalSSOAppId = objSSOSetting.Settings.Get("SSO_App_Id").Value.ValueXml.InnerText
            End If
        End If

        'Enable SSO Info Log
        Dim strSSOEnableInfoLogFromDB As String = Nothing
        strSSOEnableInfoLogFromDB = getAppConfig("SSO_Enable_Info_Log")
        If strSSOEnableInfoLogFromDB Is Nothing Then
            If (strSSOEnableInfoLog = String.Empty) Then
                If (objSSOSetting.Settings.Get("SSO_Enable_Info_Log") Is Nothing) Then
                    strSSOErrCode = "SSO_ENABLE_INFO_LOG_NOT_DEFINED"
                    SSOUtil.SSOHelper.writeAppErrLog(System.DateTime.Now + ": " + strSSOErrCode + ". Error at SSOInterfacingLib.SSOHelper.loadConfig().")
                Else
                    strSSOEnableInfoLog = objSSOSetting.Settings.Get("SSO_Enable_Info_Log").Value.ValueXml.InnerText
                End If
            End If
        Else
            strSSOEnableInfoLog = strSSOEnableInfoLogFromDB
        End If


        'Tailored for SSO_Related_App_Ids
        If (strSSORelatedAppIds = String.Empty) Then
            If (objSSOSetting.Settings.Get("SSO_Related_App_Ids") Is Nothing) Then
                strSSOErrCode = "SSO_RELATED_APP_IDS_NOT_DEFINED"
                SSOUtil.SSOHelper.writeAppErrLog(System.DateTime.Now + ": " + strSSOErrCode + ". Error at SSOInterfacingLib.SSOHelper.loadConfig().")
            Else
                strSSORelatedAppIds = objSSOSetting.Settings.Get("SSO_Related_App_Ids").Value.ValueXml.InnerText
            End If
        End If

    End Sub

    ''' <summary>
    ''' For each Replying Application, it must implement this function to provide customized data
    ''' to the target applications
    ''' </summary>
    ''' <param name="strSSOTargetSiteSSOAppId">the target application the local application want to Single Sign-On to</param>
    ''' <returns>
    ''' an non-empty objSSOCustomizedContent for other applicaton.
    ''' Returning an null value or SSOCustomizedContent containing empty content will make accessing to the tarhet application failed
    ''' </returns>
    ''' <remarks></remarks>
    Public Shared Function generateSSOCustomizedContent(ByVal strSSOTargetSiteSSOAppId As String) As SSODataType.SSOCustomizedContent

        loadConfig()

        Dim objSSOCustomizedContent As SSODataType.SSOCustomizedContent = New SSODataType.SSOCustomizedContent()

        'If (strSSOTargetSiteSSOAppId.Trim().ToUpper() = "PPI_APP") Then

        '    objSSOCustomizedContent.addEntry("RedirectTicket", System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_RedirectTicket"))
        '    objSSOCustomizedContent.addEntry("UserHKID", System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_HKID"))
        '    objSSOCustomizedContent.addEntry("UserTokenSerialNo", System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_TokenSerialNo"))
        '    objSSOCustomizedContent.addEntry("UserCommonInfo", System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_Language"))
        'ElseIf (strSSOTargetSiteSSOAppId.Trim().ToUpper() = "EHS") Then

        '    objSSOCustomizedContent.addEntry("RedirectTicket", System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_RedirectTicket"))
        '    objSSOCustomizedContent.addEntry("UserHKID", System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_HKID"))
        '    objSSOCustomizedContent.addEntry("UserTokenSerialNo", System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_TokenSerialNo"))
        '    objSSOCustomizedContent.addEntry("UserCommonInfo", System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_Language"))
        'Else
        objSSOCustomizedContent.addEntry("SSOAuthRedirectTicket", System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_RedirectTicket"))
        objSSOCustomizedContent.addEntry("UserHKID", System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_HKID"))
        objSSOCustomizedContent.addEntry("UserTokenSerialNo", System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_TokenSerialNo"))
        objSSOCustomizedContent.addEntry("UserCommonInfo", System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_Language"))
        'End If

        Return objSSOCustomizedContent

    End Function

    ''' <summary>
    ''' Get application configuration value by application specific logic
    ''' The configuration value get from this function (System Parameters) will override that in web.config
    ''' </summary>
    ''' <param name="strParaName">strParaName is the name of the parameter</param>
    ''' <returns>value of the parameter / return null value will use the configuration in web.config</returns>
    ''' <remarks></remarks>
    Public Shared Function getAppConfig(ByVal strParaName As String) As String

        'There are following variables are retrieved from database. They have higher priority over that retrieved from web config.
        '1. SSO_<SSOAppId>_IDP_WS_URL
        '2. SSO_<SSOAppId>_SP_WS_URL
        '3. SSO_<SSOAppId>_Server_Certificate_Thumbprint 
        '4. SSO_Enable_Info_Log

        Dim strSystemParaValue As String = String.Empty
        Dim udcGeneralF As New Common.ComFunction.GeneralFunction

        'Dim arrRelatedAppIds As String() = strSSORelatedAppIds.Split(",")
        If strParaName = "SSO_Related_App_Ids" Then
            Return Nothing
        End If
        Dim arrRelatedAppIds As String() = SSOAppConfigMgr.getSSORelatedAppIds()

        Dim intCounter As Integer = 0
        While intCounter < arrRelatedAppIds.Length
            'For For "SSO_<SSOAppId>_IDP_WS_URL"
            If strParaName.Trim.ToUpper = "SSO_" + arrRelatedAppIds(intCounter).Trim.ToUpper + "_IDP_WS_URL" Then
                udcGeneralF.getSystemParameter(strParaName, strSystemParaValue, String.Empty)

                If Not strSystemParaValue.Trim = "" Then
                    Return strSystemParaValue
                End If
            End If

            'For For "SSO_<SSOAppId>_SP_WS_URL"
            If strParaName.Trim.ToUpper = "SSO_" + arrRelatedAppIds(intCounter).Trim.ToUpper + "_SP_WS_URL" Then
                udcGeneralF.getSystemParameter(strParaName, strSystemParaValue, String.Empty)

                If Not strSystemParaValue.Trim = "" Then
                    Return strSystemParaValue
                End If
            End If

            'For SSO_<SSOAppId>_Server_Certificate_Thumbprint 
            If strParaName.Trim.ToUpper = "SSO_" + arrRelatedAppIds(intCounter).Trim.ToUpper + "_SERVER_CERTIFICATE_THUMBPRINT" Then
                udcGeneralF.getSystemParameter(strParaName, strSystemParaValue, String.Empty)

                If Not strSystemParaValue.Trim = "" Then
                    Return strSystemParaValue
                End If
            End If

            intCounter = intCounter + 1

        End While

        'For enable SSO audit log 'SSO_Enable_Info_Log'
        If strParaName.Trim.ToUpper = "SSO_ENABLE_INFO_LOG" Then
            udcGeneralF.getSystemParameter(strParaName, strSystemParaValue, String.Empty)

            If Not strSystemParaValue.Trim = "" Then
                Return strSystemParaValue
            End If

        End If

        Return Nothing

    End Function

    'Description:
    'This checking will be called when accessing page in SSOModule
    'Replying Application should modify accoring to their page checking practice
    'Return:
    'true if page access is allowed, false if page access is not allowed
    Public Shared Function check() As Boolean

        loadConfig()

        'Dim strUserId As String = Nothing
        Dim strHKID As String = Nothing
        Dim strTokenSerialNo As String = Nothing

        'If Not IsNothing(System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_UserId")) Then
        '    strUserId = System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_UserId").ToString()
        'End If

        If Not IsNothing(System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_HKID")) Then
            strHKID = System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_HKID").ToString()
        End If

        If Not IsNothing(System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_TokenSerialNo")) Then
            strTokenSerialNo = System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_TokenSerialNo").ToString()
        End If

        'If (((Not strUserId Is Nothing) AndAlso strUserId.Trim() <> String.Empty) And _
        '    ((Not strHKID Is Nothing) AndAlso strHKID.Trim() <> String.Empty) And _
        '    ((Not strTokenSerialNo Is Nothing) AndAlso strTokenSerialNo.Trim() <> String.Empty)) Then

        '    Return True
        'End If
        'If strUserId & "" <> "" And strHKID & "" <> "" And strTokenSerialNo & "" <> "" Then
        If strHKID & "" <> "" And strTokenSerialNo & "" <> "" Then
            Return True
        End If


        Return False

    End Function

#Region "SSO application log"
    ''' <summary>
    ''' Write application logs to a persistent storage when the "Enable Information Log" 
    ''' flag is turned on, as indicated in the parameter SSO_Enable_Info_Log
    ''' </summary>
    ''' <param name="strMsg">the message to be logged</param>
    ''' <remarks></remarks>
    Public Shared Sub writeAppInfoLog(ByVal strMsg As String)

        If (strSSOEnableInfoLog = String.Empty) Then
            strSSOEnableInfoLog = "Y"
        End If

        If (strSSOEnableInfoLog.Trim().ToUpper() = "Y") Then
            WriteAuditLogToDB(strMsg)
        End If
    End Sub

    'Application Log
    Private Shared Sub WriteAuditLogToDB(ByVal strDescription As String)

        Dim strPlatform As String = ConfigurationManager.AppSettings("Platform")
        Dim strClientIP As String
        Dim strSessionID As String = String.Empty

        'Client IP & Session ID
        Try
            strClientIP = HttpContext.Current.Request.UserHostAddress
            strSessionID = HttpContext.Current.Session.SessionID.ToString
        Catch ex As Exception
            strClientIP = String.Empty
            strSessionID = String.Empty
        End Try

        ' Browser & OS
        Dim strBrowser As String = String.Empty
        Dim strOS As String = String.Empty

        Try
            If Not HttpContext.Current.Request.Browser Is Nothing Then
                strBrowser = HttpContext.Current.Request.Browser.Type
                strOS = HttpContext.Current.Request.Browser.Platform.Trim()
            End If
        Catch ex As Exception
            strBrowser = String.Empty
            strOS = String.Empty
        End Try


        Dim strSPID As String = String.Empty
        Try
            If Not IsNothing(System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_UserId")) Then
                strSPID = System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_UserId").ToString()
            End If
        Catch
            strSPID = String.Empty
        End Try

        SSODAL.SSOLogDAL.getInstance().AddApplicationLogSSO(strClientIP, strSPID, strDescription, strSessionID, strBrowser, strOS)
        'SSODAL.SSOLogDAL.getInstance().AddAuditLogSSO(strClientIP, strSPID, strDescription, strSessionID, strBrowser, strOS)
    End Sub

#End Region

#Region "SSO Audit log"

    Public Shared Sub WriteSSOAuditLogToDB(ByVal strLogID As String, ByVal strLogType As String, ByVal strDescription As String)

        Dim strPlatform As String = ConfigurationManager.AppSettings("Platform")
        Dim strClientIP As String
        Dim strSessionID As String = String.Empty

        'Client IP & Session ID
        Try
            strClientIP = HttpContext.Current.Request.UserHostAddress
            strSessionID = HttpContext.Current.Session.SessionID.ToString
        Catch ex As Exception
            strClientIP = String.Empty
            strSessionID = String.Empty
        End Try

        ' Browser & OS
        Dim strBrowser As String = String.Empty
        Dim strOS As String = String.Empty

        Try
            If Not HttpContext.Current.Request.Browser Is Nothing Then
                strBrowser = HttpContext.Current.Request.Browser.Type
                strOS = HttpContext.Current.Request.Browser.Platform.Trim()
            End If
        Catch ex As Exception
            strBrowser = String.Empty
            strOS = String.Empty
        End Try


        Dim strSPID As String = String.Empty
        Try
            If Not IsNothing(System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_UserId")) Then
                strSPID = System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_UserId").ToString()
            End If
        Catch
            strSPID = String.Empty
        End Try

        SSODAL.SSOLogDAL.getInstance().AddAuditLogSSO(strClientIP, strSPID, strDescription, strSessionID, strBrowser, strOS, strLogID, strLogType)
    End Sub

#End Region

#Region "SSO system log (error log)"

    ''' <summary>
    ''' Write application logs to a persistent storage when error occurs 
    ''' </summary>
    ''' <param name="strMsg">the message to be logged</param>
    ''' <remarks></remarks>
    Public Shared Sub writeAppErrLog(ByVal strMsg As String)

        'Dim objSystemMsgLogger As SSOUtil.SystemMsgLogger = New SSOUtil.SystemMsgLogger()
        'objSystemMsgLogger.LogError(strMsg)
        WriteSystemLogToDB(strMsg)

    End Sub

    Private Shared Sub WriteSystemLogToDB(ByVal strDescription As String)

        Dim strPlatform As String = ConfigurationManager.AppSettings("Platform")
        Dim strClientIP As String
        Dim strSessionID As String = String.Empty

        'Client IP & Session ID
        Try
            strClientIP = HttpContext.Current.Request.UserHostAddress
            strSessionID = HttpContext.Current.Session.SessionID.ToString
        Catch ex As Exception
            strClientIP = String.Empty
            strSessionID = String.Empty
        End Try

        ' Browser & OS
        Dim strBrowser As String = String.Empty
        Dim strOS As String = String.Empty

        Try
            If Not HttpContext.Current.Request.Browser Is Nothing Then
                strBrowser = HttpContext.Current.Request.Browser.Type
                strOS = HttpContext.Current.Request.Browser.Platform.Trim()
            End If
        Catch ex As Exception
            strBrowser = String.Empty
            strOS = String.Empty
        End Try


        Dim strSPID As String = String.Empty
        Try
            If Not IsNothing(System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_UserId")) Then
                strSPID = System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_UserId").ToString()
            End If
        Catch
            strSPID = String.Empty
        End Try

        SSODAL.SSOLogDAL.getInstance().AddSystemLogSSO(strClientIP, strSPID, strSessionID, strBrowser, strOS, strDescription)
    End Sub

#End Region


    Public Shared AuthenTicketSystemDtmDateFormat As String = "yyyy-MM-dd HH:mm:ss.fff"
    Public Shared AuthenTagName As String = "Authentication"
    Public Shared SignAuthenTicketElementId As String = "#SSOAuthen"
    Public Shared SignAuthenTicketElementIdValue As String = "SSOAuthen"
    Public Shared AuthenTicketTagName As String = "AuthenTicket"
    Public Shared SystemDtmTagName As String = "SystemDtm"

    Public Shared Function getAuthenTicketXML(ByVal strAuthenTicket As String, ByVal dtmSystemDtm As DateTime) As String

        '<Authentication id='SSOAuthen'><AuthenTicket>jTLWtYTFI3Ycxc1Ym9PLrn8ucS5HK8NqCiJXO04c</AuthenTicket><SystemDtm>2010-05-27 19:56:21.182</SystemDtm></Authentication>

        '<Authentication id='SSOAuthen'>
        '<AuthenTicket>
        'jTLWtYTFI3Ycxc1Ym9PLrn8ucS5HK8NqCiJXO04c
        '</AuthenTicket>
        '<SystemDtm>
        '2010-05-27 19:56:21.182
        '</SystemDtm>
        '</Authentication>

        Dim strResult As String = String.Empty
        strResult = strResult + "<" + AuthenTagName + " id='" + SignAuthenTicketElementIdValue + "'>"
        strResult = strResult + "<" + AuthenTicketTagName + ">"
        strResult = strResult + strAuthenTicket.Trim()
        strResult = strResult + "</" + AuthenTicketTagName + ">"
        strResult = strResult + "<" + SystemDtmTagName + ">"
        strResult = strResult + dtmSystemDtm.ToString(AuthenTicketSystemDtmDateFormat)
        strResult = strResult + "</" + SystemDtmTagName + ">"
        strResult = strResult + "</" + AuthenTagName + ">"
        Return strResult

    End Function

    Public Shared Function extractAuthentTicket(ByVal strXMLAuthenTicket As String, ByRef strAuthenTicket As String, ByRef dtmSystemDtm As DateTime) As Boolean
        Dim xmlDoc As New XmlDocument()
        xmlDoc.LoadXml(strXMLAuthenTicket)

        Dim ticketNodeList As XmlNodeList = xmlDoc.GetElementsByTagName(AuthenTicketTagName)
        Dim dtmNodeList As XmlNodeList = xmlDoc.GetElementsByTagName(SystemDtmTagName)

        If ticketNodeList.Count = 1 AndAlso dtmNodeList.Count = 1 Then
            Dim strValue As String = ticketNodeList(0).InnerText
            strAuthenTicket = strValue
            'strValue = dtmNodeList(0).InnerText
            'dtmSystemDtm = CType(strValue, DateTime)
            dtmSystemDtm = CType(dtmNodeList(0).InnerText, DateTime)
        Else
            Return False
        End If
        Return True
    End Function

End Class
