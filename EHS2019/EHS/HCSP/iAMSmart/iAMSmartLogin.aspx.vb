Imports Common.ComFunction
Imports Common.Encryption
Imports Common.ComObject
Imports Common.Component
Imports Common.Format
Imports Common.ComFunction.AccountSecurity
Imports Common.DataAccess
Imports Common.Component.Token
Imports Common.iAMSmart
Imports Common.Component.UserAC
Imports Common.Component.ServiceProvider
Imports HCSP.BLL
Imports System.ComponentModel
Imports System.Reflection
Imports System.Web.Services
Imports System.IO
Imports System.Xml.Serialization

Partial Public Class iAMSmartLogin
    Inherits BasePage

    Dim udcGeneralFun As New GeneralFunction
    Dim udcValidator As New Common.Validation.Validator
    Dim udcSessionHandler As New BLL.SessionHandler
    Dim udcLoginBll As New BLL.LoginBLL
    Dim udtUserACBLL As New Common.Component.UserAC.UserACBLL
    Dim udtiAMSmartSPBLL As New Common.iAMSmart.iAMSmartSPBLL
    Dim udtiAMSmartBLL As New Common.Component.iAMSmart.iAMSmartBLL
    Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL
    Dim DemoSampleData As New DemoSample()

    Const FUNCTION_CODE As String = Common.Component.FunctCode.FUNT020009
    Private Const SESS_Show4thLevelAlertD28 As String = "Show4thLevelAlertD28"
    Private Const SESS_FirstChangePassword As String = "FirstChangePassword"
    Private Const SESS_ChangePasswordUserAC As String = "ChangePasswordUserAC"
    Private Const SESS_MustChangePassword As String = "MustChangePassword"

#Region "Constants"
    Public Class ActiveViewIndex
        Public Const BackToLogin As Integer = 0
        Public Const GetProfile As Integer = 1
        Public Const LinkupSPAccount As Integer = 2
        Public Const NoSPAccountFound As Integer = 3
        Public Const MsgPage As Integer = 5
        Public Const SimulateMode As Integer = 4
    End Class

    Public Class SESS_iAMSmart
        Public Const TokenID As String = "iAMSmart_TokenID"
        Public Const BusinessID As String = "iAMSmart_BusinessID"
        Public Const TicketID As String = "iAMSmart_TicketID"
        Public Const State As String = "iAMSmart_State"
        Public Const OpenID As String = "iAMSmart_OpenID"
        Public Const AccessTokenTimeout As String = "iAMSmart_AccessTokenTimeout"
        Public Const ProfileRequestTimeout As String = "iAMSmart_ProfileRequestTimeout"
        Public Const ProfileRequestPopupShown As String = "iAMSmart_ProfileRequestPopupShown"
        Public Const BrowserType As String = "iAMSmart_BrowserType"
        Public Const AuthByQRCode As String = "iAMSmart_AuthByQRCode"
    End Class

    Public Class Profile
        Public Enum Fields
            <Description("HKIC")>
                idNo
            <Description("NameEN")>
                enName
            <Description("NameCHI")>
                chName
            <Description("Gender")>
                gender
            <Description("DOB")>
                birthDate
        End Enum

        Public Shared ReadOnly Property List As List(Of String)
            Get
                Return New List(Of String) From {Profile.Fields.idNo.ToString, _
                                                 Profile.Fields.enName.ToString, _
                                                 Profile.Fields.chName.ToString, _
                                                 Profile.Fields.gender.ToString, _
                                                 Profile.Fields.birthDate.ToString}
            End Get
        End Property

        Public Shared ReadOnly Property ListDesc As List(Of String)
            Get
                Return New List(Of String) From {iAMSmartSPBLL.GetEnumDescription(Profile.Fields.idNo), _
                                                 iAMSmartSPBLL.GetEnumDescription(Profile.Fields.enName), _
                                                 iAMSmartSPBLL.GetEnumDescription(Profile.Fields.chName), _
                                                 iAMSmartSPBLL.GetEnumDescription(Profile.Fields.gender), _
                                                 iAMSmartSPBLL.GetEnumDescription(Profile.Fields.birthDate)}
            End Get
        End Property

    End Class

    Public Class AuditLog
        Public Const MSG00001 As String = "iAM Smart Login Page Load"
        'Public Const MSG00002 As String = "iAM Smart Stimulate Mode"
        'Public Const MSG00003 As String = "iAM Smart Live Mode"
        Public Const MSG00004 As String = "Generate the AES key"
        Public Const MSG00005 As String = "State not found"
        Public Const MSG00006 As String = "Cookie not found / not matched"
        Public Const MSG00009 As String = "iAM Smart Fail"

        Public Const MSG00030 As String = "Confirm Cancel - Yes Click"
        Public Const MSG00031 As String = "Confirm Cancel - No Click"
        Public Const MSG00032 As String = "Open iAM Smart App Click"
        Public Const MSG00033 As String = ""
        Public Const MSG00034 As String = "Profile Callback timeout"
        Public Const MSG00035 As String = "Prfoile Callback error"
        Public Const MSG00036 As String = "Go to Enrolment Click"
        Public Const MSG00037 As String = "Go to Enrolment Page - Back to Login Click"
        Public Const MSG00038 As String = "Go to Home Click"

        Public Const MSG00043 As String = "Improper access iAM Smart Login page"
        Public Const MSG00044 As String = "Back to login Click"
        Public Const MSG00045 As String = "Redirect to Login page, without any alerts"

        Public Const MSG00050 As String = "Handle Web Site Login start"
        Public Const MSG00051 As String = "Handle Web Site Login complete"
        Public Const MSG00052 As String = "Handle Web Site Login fail"
        Public Const MSG00053 As String = "Handle Web Site Login fail - Service Provider is not linked with iAM Smart and redirect to get profile"
        Public Const MSG00054 As String = "Handle Web Site Login fail - Get Access Token Error"
        Public Const MSG00055 As String = "HandleWebSiteLogin - Pass Login Validation"
        Public Const MSG00056 As String = "HandleWebSiteLogin - Fail Login Validation"

        Public Const MSG00060 As String = "Request Profile Click"
        Public Const MSG00061 As String = "Request Profile - Authorise success"
        Public Const MSG00062 As String = "Request Profile - Authorise fail"
        Public Const MSG00063 As String = "Request Profile - Long Polling start"
        Public Const MSG00064 As String = "Request Profile - AccessToken timeout"
        Public Const MSG00065 As String = "Request Profile - Long Polling check record"
        Public Const MSG00066 As String = "Request Profile - Long Polling complete"
        Public Const MSG00067 As String = "Profile Callback: HKID"
        Public Const MSG00068 As String = "Profile Callback: HKID not match"
        Public Const MSG00069 As String = "Profile Callback: HKID match"
        Public Const MSG00077 As String = "Profile Callback Error"

        Public Const MSG00070 As String = "Stimulate Random No Click"
        Public Const MSG00071 As String = "Stimulate Connect Click"
        Public Const MSG00072 As String = "Stimulate Handle Web Site Login start"
        Public Const MSG00073 As String = "Stimulate Handle Web Site Login success"
        Public Const MSG00074 As String = "Stimulate Handle Web Site Login fail"
        Public Const MSG00075 As String = "Stimulate Handle Web Site Login fail - Service Provider is not connect to iAM Smart"
        Public Const MSG00076 As String = "Stimulate HKIC Click"
    End Class

    Public Class DemoSample
        Private strOpenID As String
        Private strBusinessID As String
        Private strHKIC As String

        Public Sub New()
        End Sub

        Public Property OpenID() As String
            Get
                Return Me.strOpenID
            End Get
            Set(ByVal value As String)
                Me.strOpenID = value
            End Set

        End Property

        Public Property BusinessID() As String
            Get
                Return Me.strBusinessID
            End Get
            Set(ByVal value As String)
                Me.strBusinessID = value
            End Set

        End Property

        Public Property HKIC() As String
            Get
                Return Me.strHKIC
            End Get
            Set(ByVal value As String)
                Me.strHKIC = value
            End Set

        End Property
    End Class

#End Region

#Region "Properties"
    Public ReadOnly Property PollingInterval() As Integer
        Get
            Return udcGeneralFun.GetSystemParameterParmValue1("IAMSmartLongPollingInterval")
        End Get
    End Property

    Public ReadOnly Property OpeniAMSmartAppLink() As String
        Get
            Return Me.GenerateOpeniAMSmartAppLink()
        End Get
    End Property
#End Region

#Region "Page Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim striAMSmartMode As String = ConfigurationManager.AppSettings("iAMSmartMode")

        Response.Buffer = True
        Response.ExpiresAbsolute = Now().Subtract(New TimeSpan(1, 0, 0, 0))
        Response.Expires = 0
        Response.CacheControl = "no-cache"

        Page.Response.Cache.SetCacheability(HttpCacheability.NoCache)

        MyBase.FunctionCode = FUNCTION_CODE

        Session(SESS_MustChangePassword) = Nothing

        If Not IsPostBack Then

            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.AddDescripton("Mode", IIf(striAMSmartMode = iAMSmartMode.Stimulate, "Stimulate", "Live"))
            udtAuditLogEntry.WriteLog(LogID.LOG00001, AuditLog.MSG00001)

            udcErrorMessage.Clear()
            udcInfoMessageBox.Clear()

            Dim strState As String = String.Empty
            Dim strDBState As String = String.Empty
            Dim strErrorCode As String = String.Empty
            Dim strCode As String = String.Empty
            Dim strCookies As String = String.Empty

            Session(SESS_iAMSmart.ProfileRequestPopupShown) = Nothing

            Select Case striAMSmartMode
                Case iAMSmartMode.Stimulate
                    Me.mvStepForiAMSmart.ActiveViewIndex = ActiveViewIndex.SimulateMode

                    Return

                Case iAMSmartMode.Live
                    '---------------------------------
                    '  Get AES key
                    '---------------------------------
                    'Get AES Key from system variable 
                    Dim strAESKey As String = udtiAMSmartBLL.GetAESKey()

                    If String.IsNullOrEmpty(strAESKey) Then
                        'Generate AES Key
                        strAESKey = (New iAMSmartSPBLL).GetAESKey(FunctionCode)
                        'Update the AES Key to system variable
                        udtiAMSmartBLL.AddAESKey(strAESKey)
                    End If

                    '-------------------------------------------
                    '  Classify the step by querystring values
                    '-------------------------------------------
                    Dim blnMatchCase As Boolean = False

                    ' Case 0 - Error
                    If Not blnMatchCase Then
                        strErrorCode = Context.Request.QueryString(iAMSmartSPBLL.PARAM_ERROR_CODE)
                        If Not String.IsNullOrEmpty(strErrorCode) Then
                            'Error, back to login
                            BackToLogin()

                            'Dim strMessage As String = iAMSmartSPBLL.GetReturnCodeMessage(strErrorCode)

                            Select Case strErrorCode
                                'D40000 - Cancel Request
                                'D40001 - Reject Request
                                Case "D40000", "D40001"
                                    'udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001, "%s", strErrorCode + " - " + strMessage)
                                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00009)
                                    udcInfoMessageBox.BuildMessageBox()
                                Case Else
                                    udcErrorMessage.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002, "%s", String.Format("({0})", strErrorCode))
                                    udcErrorMessage.BuildMessageBox("ValidationFail")
                            End Select

                            blnMatchCase = True

                            udtAuditLogEntry.AddDescripton("Return Code", strErrorCode)
                            udtAuditLogEntry.WriteLog(LogID.LOG00009, AuditLog.MSG00009)
                        End If
                    End If

                    ' Case 1 - Callback from get QR code
                    If Not blnMatchCase Then
                        strCode = Context.Request.QueryString(iAMSmartSPBLL.PARAM_CODE)
                        strState = Context.Request.QueryString(iAMSmartSPBLL.PARAM_STATE)
                        strCookies = Context.Request.QueryString(iAMSmartSPBLL.PARAM_COOKIE)

                        If Not String.IsNullOrEmpty(strCode) AndAlso Not String.IsNullOrEmpty(strState) Then
                            'Compare the state code from query string to state code stored in DB, whether the state code come from Website or direct login. 
                            If CheckStateCode(strState, strCookies, udtAuditLogEntry) Then
                                Session(SESS_iAMSmart.State) = strState
                                Me.HandleWebSiteLogin(strCode, strState)

                            Else
                                'udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00008) 'Improper access.
                                'udcInfoMessageBox.BuildMessageBox()

                                'Error - state code not matched, back to login
                                RedirectToLogin()

                                udtAuditLogEntry.WriteLog(LogID.LOG00045, AuditLog.MSG00045)

                            End If

                            blnMatchCase = True

                        End If

                    End If

                    If Not blnMatchCase Then
                        'Default case when not match any cases
                        RedirectToLogin()

                        udtAuditLogEntry.WriteLog(LogID.LOG00045, AuditLog.MSG00045)

                    End If

            End Select
            

        Else
            Select Case Me.mvStepForiAMSmart.ActiveViewIndex
                Case ActiveViewIndex.GetProfile
                    If Session(SESS_iAMSmart.ProfileRequestPopupShown) IsNot Nothing AndAlso Session(SESS_iAMSmart.ProfileRequestPopupShown) = YesNo.Yes Then
                        Dim blnAuthByQRCode As Nullable(Of Boolean) = Session(iAMSmartLogin.SESS_iAMSmart.AuthByQRCode)

                        'Default show the mobile image
                        imgiAMSmartApp.Style.Remove("display")

                        'Default invisible the "Open iAM Smart" button
                        btnCalliAMSmartApp.Style.Add("display", "none")

                        If blnAuthByQRCode IsNot Nothing AndAlso Not blnAuthByQRCode Then
                            imgiAMSmartApp.Style.Add("display", "none")
                            btnCalliAMSmartApp.Style.Remove("display")
                        End If

                        If striAMSmartMode = iAMSmartMode.Live Then
                            ModalPopupAuthoriseiAMSmart.Show()
                        End If

                    End If
            End Select

        End If

        Dim selectedLang As String
        selectedLang = LCase(Session("language"))

        Dim strPrivacyPolicyLink As String = String.Empty

        If selectedLang.Equals(English) Then
            udcGeneralFun.getSystemParameter("PrivacyPolicyLink", strPrivacyPolicyLink, String.Empty)
        ElseIf selectedLang.Equals(TradChinese) Then
            udcGeneralFun.getSystemParameter("PrivacyPolicyLink_CHI", strPrivacyPolicyLink, String.Empty)
        End If

        Dim strDisclaimerPolicyLink As String = String.Empty

        If selectedLang.Equals(English) Then
            udcGeneralFun.getSystemParameter("DisclaimerLink", strDisclaimerPolicyLink, String.Empty)
        ElseIf selectedLang.Equals(TradChinese) Then
            udcGeneralFun.getSystemParameter("DisclaimerLink_CHI", strDisclaimerPolicyLink, String.Empty)
        End If

        Dim strSysMaintLink As String = String.Empty

        If selectedLang.Equals(English) Then
            udcGeneralFun.getSystemParameter("SysMaintLink", strSysMaintLink, String.Empty)
        ElseIf selectedLang.Equals(TradChinese) Then
            udcGeneralFun.getSystemParameter("SysMaintLink_CHI", strSysMaintLink, String.Empty)
        End If

        Me.ModalPopupConfirmCancel.PopupDragHandleControlID = Me.ucNoticePopUpConfirm.Header.ClientID

    End Sub

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Select Case mvStepForiAMSmart.ActiveViewIndex
            Case ActiveViewIndex.GetProfile
                Dim lstProfileFields As List(Of String) = Me.GetProfileFieldsDesc

                Dim strHTML As String = String.Empty

                For i As Integer = 0 To lstProfileFields.Count - 1
                    strHTML = strHTML + "<span style='font-size:18px'> - " + GetGlobalResourceObject("Text", "iAMSmart" + lstProfileFields(i)) + "</span><br><br>"
                Next

                divRequestProfileFields.InnerHtml = strHTML

                Select Case LCase(Session("language"))
                    Case English
                        btniAMSmartRequestProfile.Style.Add("width", "280px")
                        btniAMSmartRequestProfile.Style.Add("position", "relative")
                        btniAMSmartRequestProfile.Style.Add("left", "260px")

                    Case TradChinese
                        btniAMSmartRequestProfile.Style.Add("width", "180px")
                        btniAMSmartRequestProfile.Style.Add("position", "relative")
                        btniAMSmartRequestProfile.Style.Add("left", "310px")

                    Case SimpChinese
                        btniAMSmartRequestProfile.Style.Add("width", "180px")
                        btniAMSmartRequestProfile.Style.Add("position", "relative")
                        btniAMSmartRequestProfile.Style.Add("left", "210px")

                End Select

            Case Else

        End Select
    End Sub

    Private Sub mvStepForiAMSmart_ActiveViewChanged(sender As Object, e As EventArgs) Handles mvStepForiAMSmart.ActiveViewChanged
        Select Case mvStepForiAMSmart.ActiveViewIndex
            Case ActiveViewIndex.GetProfile
                Dim lstProfileFields As List(Of String) = Me.GetProfileFieldsDesc

                Dim strHTML As String = String.Empty

                For i As Integer = 0 To lstProfileFields.Count - 1
                    strHTML = strHTML + "<span style='font-size:18px'> - " + GetGlobalResourceObject("Text", "iAMSmart" + lstProfileFields(i)) + "</span><br><br>"
                Next

                divRequestProfileFields.InnerHtml = strHTML

            Case Else

        End Select
    End Sub

#End Region

    Protected Overrides Sub InitializeCulture()
        Dim selectedValue As String = String.Empty

        If Not Request(PostBackEventTarget) Is Nothing Then
            'Dim controlID As String = Request.Form(PostBackEventTarget)
            Dim controlID As String = Page.Request.Params.Get(PostBackEventTarget)
            If controlID.Equals(_SelectTradChinese) Then
                selectedValue = TradChinese
                Session("language") = selectedValue
            ElseIf controlID.Equals(_SelectSimpChinese) Then
                selectedValue = SimpChinese
                Session("language") = selectedValue
            ElseIf controlID.Equals(_SelectEnglish) Then
                selectedValue = English
                Session("language") = selectedValue
            End If
        End If

        MyBase.InitializeCulture()

    End Sub

#Region "Implement IWorkingData"
    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Account which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Transaction which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Service Provider which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        Return Nothing
    End Function
    ''' <summary>
    ''' CRE11-004
    ''' Retrieve Document Code which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetDocCode() As String
        Return Nothing
    End Function

#End Region

    'Private Sub ChangeTimeLine(ByVal strStep As String)
    '    Dim strhightlight, strunhightlight As String

    '    strhightlight = "highlightTimeline"
    '    strunhightlight = "unhighlightTimeline"

    '    Me.panStep1.CssClass = strunhightlight
    '    Me.panStep2.CssClass = strunhightlight
    '    Me.panStep3.CssClass = strunhightlight
    '    Me.panStep4.CssClass = strunhightlight

    '    Select Case strStep
    '        Case ActiveViewIndex.Step1
    '            Me.panStep1.CssClass = strhightlight

    '        Case ActiveViewIndex.Step2a, ActiveViewIndex.Step2b
    '            Me.panStep2.CssClass = strhightlight

    '        Case ActiveViewIndex.Step3
    '            Me.panStep3.CssClass = strhightlight

    '        Case ActiveViewIndex.Step4
    '            Me.panStep4.CssClass = strhightlight

    '    End Select
    'End Sub

    Private Sub HandleWebSiteLogin(ByVal strCode As String, ByVal strState As String)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE)
        Dim udtLoginBLL As New LoginBLL
        Dim strTokenID As String = String.Empty
        Dim strOpenID As String = String.Empty
        Dim strReturnCode As String = String.Empty
        Dim strMessage As String = String.Empty

        '---------------------------------------------
        ' 1. Check code & state
        '---------------------------------------------
        If String.IsNullOrEmpty(strCode) OrElse String.IsNullOrEmpty(strState) Then
            Return
        End If

        udtAuditLogEntry.WriteLog(LogID.LOG00050, AuditLog.MSG00050)

        Dim blnResGetAccessToken As Boolean = False

        '---------------------------------------------
        ' 2. Get Access Token, Open ID
        '---------------------------------------------
        blnResGetAccessToken = udtiAMSmartSPBLL.GetAccessToken(strCode, strState, strTokenID, strOpenID, strReturnCode, FUNCTION_CODE)

        If Not blnResGetAccessToken Then
            strMessage = iAMSmartSPBLL.GetReturnCodeMessage(strReturnCode)

            'Error, back to login
            BackToLogin()

            udtAuditLogEntry.AddDescripton("ReturnCode", strReturnCode)
            udtAuditLogEntry.AddDescripton("Message", strMessage)
            udtAuditLogEntry.WriteLog(LogID.LOG00054, AuditLog.MSG00054)

            ErrorHandler.Log(udtAuditLogEntry.FunctionCode, SeverityCode.SEVE, "99999", _
                             Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, AuditLog.MSG00054)

            udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00004) 'System error. Please try again later.
            udcInfoMessageBox.BuildMessageBox()

            Return
        End If

        '---------------------------------------------
        ' 3. Check iAM Smart linked mapping
        '---------------------------------------------
        Session(SESS_iAMSmart.TokenID) = strTokenID
        Session(SESS_iAMSmart.OpenID) = strOpenID
        Session(SESS_iAMSmart.AccessTokenTimeout) = Date.Now

        'Check mapping by OpenID whether SP is linked up
        Dim dtiAMSmartSPMapping As DataTable = CheckiAMSmartLinkup(strOpenID)

        If dtiAMSmartSPMapping Is Nothing Then
            'Not found, go to get profile 
            udtAuditLogEntry.WriteLog(LogID.LOG00053, AuditLog.MSG00053)

            Me.mvStepForiAMSmart.ActiveViewIndex = ActiveViewIndex.GetProfile

            Return
        End If

        '---------------------------------------------
        ' 4. Check service provider status
        '---------------------------------------------
        Dim strSPID As String = String.Empty
        Dim blnValid As Boolean = False
        Dim dtUserAC As DataTable = Nothing

        For Each dr As DataRow In dtiAMSmartSPMapping.Rows
            strSPID = dr("SP_ID").ToString.Trim()

            udtLoginBLL.CheckSPExist(dtUserAC, SPAcctType.ServiceProvider, strSPID)

            If dtUserAC.Rows(0).Item("Record_Status").ToString.Trim <> Formatter.EnumToString(ServiceProvider.ServiceProviderModel.RecordStatusEnumClass.Delisted) Then
                'Service Provider status: Active or Suspended
                blnValid = True
                Exit For
            End If
        Next

        If Not blnValid Then
            'Service Provider status: Delisted

            'Not found, go to get profile 
            udtAuditLogEntry.WriteLog(LogID.LOG00053, AuditLog.MSG00053)

            Me.mvStepForiAMSmart.ActiveViewIndex = ActiveViewIndex.GetProfile

            Return
        End If

        '---------------------------------------------
        ' 5. Login Validation
        '---------------------------------------------
        Dim udtUserAC As UserACModel = Nothing
        Dim udtSystemMessage As SystemMessage = Nothing

        'Found, go to validation before login
        If LoginValidation(strSPID, udtUserAC, udtAuditLogEntry, udtSystemMessage) Then
            'Valid, and redirect page
            udtAuditLogEntry.WriteLog(LogID.LOG00055, AuditLog.MSG00055)
            ProceedLogin(udtUserAC, udtAuditLogEntry)

        Else
            'Error, back to login
            udtAuditLogEntry.WriteLog(LogID.LOG00056, AuditLog.MSG00056)

            HandleLoginFail(strSPID, Nothing, SPAcctType.ServiceProvider, udtAuditLogEntry)

            BackToLogin()

            If udtSystemMessage IsNot Nothing Then
                udcErrorMessage.AddMessage(udtSystemMessage)
                udcErrorMessage.BuildMessageBox(LoginBLL.AuditLogDesc.Header00001, udtAuditLogEntry, LogID.LOG00052, AuditLog.MSG00052)
            Else
                udcErrorMessage.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00134)
                udcErrorMessage.BuildMessageBox(LoginBLL.AuditLogDesc.Header00001, udtAuditLogEntry, AuditLog.MSG00052, LogID.LOG00052, strSPID, Nothing)

            End If
        End If

    End Sub

    Private Sub HandleWebSiteLoginDemo(ByVal strCode As String, ByVal strState As String)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE)
        Dim strTokenID As String = String.Empty
        Dim strOpenID As String = String.Empty
        Dim strReturnCode As String = String.Empty
        Dim strMessage As String = String.Empty

        If Not String.IsNullOrEmpty(strCode) AndAlso Not String.IsNullOrEmpty(strState) Then
            udtAuditLogEntry.WriteLog(LogID.LOG00050, AuditLog.MSG00050)

            Dim strDemoVersion As String = ConfigurationManager.AppSettings("iAMSmartMode")

            Dim blnResGetAccessToken As Boolean = False

            blnResGetAccessToken = True
            strOpenID = DemoSampleData.OpenID
            'strOpenID = "liR14%2BvX%2F5hSum5uf4ERczu0KcDnIJA5BM7FoM1ag9c%3DX"
            strReturnCode = "D00000"
            strMessage = "Success"

            If blnResGetAccessToken Then
                Session(SESS_iAMSmart.TokenID) = strTokenID
                Session(SESS_iAMSmart.OpenID) = strOpenID
                Session(SESS_iAMSmart.AccessTokenTimeout) = Date.Now

                'udtAuditLogEntry.AddDescripton("ReturnCode", strReturnCode)
                'udtAuditLogEntry.AddDescripton("Message", strMessage)

                'Check mapping by OpenID whether SP is linked up
                'udtAuditLogEntry.WriteLog(LogID.LOG00000, "iAM Smart endose service provider")

                Dim dtiAMSmartSPMapping As DataTable = CheckiAMSmartLinkup(strOpenID)

                If dtiAMSmartSPMapping IsNot Nothing Then
                    'Found, go to validation before login
                    Dim udtUserAC As UserACModel = Nothing
                    Dim udtSystemMessage As SystemMessage = Nothing

                    If LoginValidation(dtiAMSmartSPMapping.Rows(0)("SP_ID").ToString.Trim, udtUserAC, udtAuditLogEntry, udtSystemMessage) Then
                        'Valid, and redirect page
                        'udtAuditLogEntry.WriteLog(LogID.LOG00000, "iAM Smart - service provider linked up with iAM Smart")

                        ProceedLogin(udtUserAC, udtAuditLogEntry)

                    Else
                        'Error, back to login
                        'udtAuditLogEntry.WriteLog(LogID.LOG00000, "iAM Smart - failed to match service provider")
                        HandleLoginFail(dtiAMSmartSPMapping.Rows(0)("SP_ID").ToString.Trim, Nothing, SPAcctType.ServiceProvider, udtAuditLogEntry)

                        BackToLogin()

                        If udtSystemMessage IsNot Nothing Then
                            udcErrorMessage.AddMessage(udtSystemMessage)
                            udcErrorMessage.BuildMessageBox(LoginBLL.AuditLogDesc.Header00001, udtAuditLogEntry, LogID.LOG00052, AuditLog.MSG00052)
                        Else
                            'udtAuditLogEntry.WriteLog(LogID.LOG00052, AuditLog.MSG00052)
                            'udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001, "%s", "Login failed. Please try again later.")
                            'udcInfoMessageBox.BuildMessageBox()
                            udcErrorMessage.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00134)
                            udcErrorMessage.BuildMessageBox(LoginBLL.AuditLogDesc.Header00001, udtAuditLogEntry, AuditLog.MSG00052, LogID.LOG00052, dtiAMSmartSPMapping.Rows(0)("SP_ID").ToString.Trim, Nothing)

                        End If
                    End If

                Else
                    'Not found, go to get profile 
                    udtAuditLogEntry.WriteLog(LogID.LOG00053, AuditLog.MSG00053)

                    Me.mvStepForiAMSmart.ActiveViewIndex = ActiveViewIndex.GetProfile
                End If

            Else
                'Error, back to login
                BackToLogin()

                udtAuditLogEntry.AddDescripton("ReturnCode", strReturnCode)
                udtAuditLogEntry.AddDescripton("Message", strMessage)
                udtAuditLogEntry.WriteLog(LogID.LOG00054, AuditLog.MSG00054)

                ErrorHandler.Log(udtAuditLogEntry.FunctionCode, SeverityCode.SEVE, "99999", _
                                 Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, AuditLog.MSG00054)

                udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00004) 'System error. Please try again later.
                udcInfoMessageBox.BuildMessageBox()

            End If

        End If
    End Sub

    Private Function CheckiAMSmartLinkup(ByVal strOpenID As String) As DataTable
        Dim udtiAMSmartBLL As New Common.Component.iAMSmart.iAMSmartBLL
        Dim dtiAMSmartSPMapping As DataTable = Nothing

        '---------------------------------
        ' Check iAM Smart registration
        '---------------------------------
        'Check from DB mapping
        dtiAMSmartSPMapping = udtiAMSmartBLL.GetServiceProviderByOpenID_iAMSmart(strOpenID)

        If dtiAMSmartSPMapping IsNot Nothing AndAlso dtiAMSmartSPMapping.Rows.Count > 0 Then
            If (dtiAMSmartSPMapping.Rows(0).Item("SP_ID") IsNot Nothing) Then
                'SP already registered iAMSmart
                Return dtiAMSmartSPMapping
            Else
                'SP has not registered iamSmart yet or disconnected with iamSmart
                Return Nothing
            End If
        End If

        Return Nothing

    End Function

    Public Function LoginValidation(ByVal strSPID As String, ByRef udtUserAC As UserACModel, _
                                    ByRef udtAuditLogEntry As AuditLogEntry, ByRef udtSystemMessage As SystemMessage) As Boolean

        Dim dtUserAC As DataTable = Nothing
        Dim strLoginRole As String = SPAcctType.ServiceProvider
        Dim strLogSPID As String = strSPID
        Dim strLogDataEntryAccount As String = Nothing

        Dim udtLoginBLL As New LoginBLL

        Dim strSPStatus As String = Nothing
        Dim strPassword As String = String.Empty
        Dim strPassCode As String = String.Empty

        Dim blnPassLogin As Boolean = True

        'AuditLog for SP
        udtAuditLogEntry.AddDescripton(LoginBLL.AuditLogDesc.Field00001, strSPID)
        udtAuditLogEntry.AddDescripton(LoginBLL.AuditLogDesc.Field00002, String.Empty)
        udtAuditLogEntry.AddDescripton("IdeasComboClient", IIf(udcSessionHandler.IDEASComboClientGetFormSession() Is Nothing, YesNo.No, udcSessionHandler.IDEASComboClientGetFormSession()))
        udtAuditLogEntry.AddDescripton("IdeasComboVersion", IIf(udcSessionHandler.IDEASComboVersionGetFormSession() Is Nothing, String.Empty, udcSessionHandler.IDEASComboVersionGetFormSession()))

        '--------------------------------
        'Step 1: Check Down Service
        '--------------------------------
        If blnPassLogin Then
            If udtLoginBLL.CheckServiceDown() Then
                'Service under construction
                udtSystemMessage = New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00151) 'System is currently under maintenance. Please try later.
                blnPassLogin = False
            End If
        End If

        'Write the start log of login
        udtAuditLogEntry.WriteStartLog(LoginBLL.AuditLog_LogID.Start, LoginBLL.AuditLog_Prefix.SP_iAMSmart & LoginBLL.AuditLog.MSG00001, strLogSPID, strLogDataEntryAccount) 'Service Provider (iAM Smart) Login

        '--------------------------------
        'Step 2: Validation
        '--------------------------------
        If blnPassLogin Then
            'Check SP is existed or not
            If Not udtLoginBLL.CheckSPExist(dtUserAC, strLoginRole, strLogSPID) Then
                blnPassLogin = False
                udtAuditLogEntry.AddDescripton(LoginBLL.AuditLogDesc.Field00000, LoginBLL.AuditLogDesc.MSG00001) 'SPID/Username is not found

            End If
        End If

        If blnPassLogin Then
            ' If SP account not activated
            If Not udtLoginBLL.CheckSPActivation(dtUserAC) Then
                blnPassLogin = False
                udtAuditLogEntry.AddDescripton(LoginBLL.AuditLogDesc.Field00000, LoginBLL.AuditLogDesc.MSG00002) 'SP is not activated

            End If
        End If

        If blnPassLogin Then
            ' No token found
            If Not udtLoginBLL.CheckTokenExist(dtUserAC) Then
                blnPassLogin = False
                udtAuditLogEntry.AddDescripton(LoginBLL.AuditLogDesc.Field00000, LoginBLL.AuditLogDesc.MSG00003) 'No token found
                'udcErrorMessage.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001, "%s", MsgCode.MSG00001 + " - " + "The token of Service Provider is not found.")
                'udcErrorMessage.BuildMessageBox()
            End If
        End If

        If blnPassLogin Then
            'Check password
            Dim enumRes As EnumVerifyPasswordResult = EnumVerifyPasswordResult.Pass

            enumRes = udtLoginBLL.CheckPassword(dtUserAC, strPassword, strPassCode, strLogSPID, strLogDataEntryAccount, strLoginRole, Me.SubPlatform, strLogSPID, String.Empty)

            Select Case enumRes
                Case EnumVerifyPasswordResult.RequireUpdate
                    blnPassLogin = False
                    'Service Provider (iAM Smart) Login fail: Hash password expired, password level lower than system minimum password level
                    udtAuditLogEntry.WriteEndLog(LoginBLL.AuditLog_LogID.ForceResetPassword, LoginBLL.AuditLog_Prefix.SP_iAMSmart & LoginBLL.AuditLog.MSG00005, strLogSPID, strLogDataEntryAccount)

                    'Handle Web Site Login fail
                    udtAuditLogEntry.AddDescripton(LoginBLL.AuditLogDesc.Field00000, LoginBLL.AuditLogDesc.MSG00009)
                    udtAuditLogEntry.WriteLog(LogID.LOG00052, AuditLog.MSG00052, strLogSPID, strLogDataEntryAccount)

                    'Service Provider (iAM Smart) Hash password expired force reset password message loaded
                    udtAuditLogEntry.WriteLog(LogID.LOG00033, LoginBLL.AuditLog_Prefix.SP_iAMSmart & LoginBLL.AuditLog.MSG00006, strLogSPID, strLogDataEntryAccount)

                    Session(SESS_MustChangePassword) = YesNo.Yes
                    RedirectHandler.ToURL(ClaimVoucherMaster.FullVersionPage.Login)

                Case EnumVerifyPasswordResult.Incorrect
                    blnPassLogin = False
                    'Service Provider (iAM Smart) Login fail: Incorrect Password
                    udtAuditLogEntry.WriteLog(LogID.LOG00025, LoginBLL.AuditLog_Prefix.SP_iAMSmart & LoginBLL.AuditLog.MSG00003, strLogSPID, strLogDataEntryAccount)
                    'StackTrace: Incorrect password
                    udtAuditLogEntry.AddDescripton(LoginBLL.AuditLogDesc.Field00000, LoginBLL.AuditLogDesc.MSG00004)

                Case EnumVerifyPasswordResult.Pass, EnumVerifyPasswordResult.ByPass
                    blnPassLogin = True

                Case Else
                    blnPassLogin = True

            End Select
        End If

        '--------------------------------
        'Step 3: Check Active scheme
        '--------------------------------
        If blnPassLogin Then
            If Not udtLoginBLL.CheckActiveScheme(dtUserAC, strLogSPID, Me.SubPlatform, strSPStatus) Then
                blnPassLogin = False
                'StackTrace: No active scheme after filtering with SubPlatform {0}. Deduced SPStatus={1}
                udtAuditLogEntry.AddDescripton(LoginBLL.AuditLogDesc.Field00000, String.Format(LoginBLL.AuditLogDesc.MSG00005, Me.SubPlatform.ToString, strSPStatus))

            End If
        End If

        '-----------------------------------------------
        'Step 4: Clear session
        '-----------------------------------------------
        If blnPassLogin Then
            ' Remove all Session while press login
            If Not Session Is Nothing Then
                HandleSessionVariable()
            End If
        End If

        '-----------------------------------------------
        'Step 5: Check user acccount status
        '-----------------------------------------------
        Dim udtServiceProvider As ServiceProviderModel = Nothing

        If blnPassLogin Then
            If Not udtLoginBLL.CheckRecordStatus(dtUserAC, SPAcctType.ServiceProvider, strLogSPID, Me.SubPlatform, udtUserAC) Then
                blnPassLogin = False
                udtServiceProvider = CType(udtUserAC, ServiceProviderModel)
            End If
        End If

        '-----------------------------------------------
        'Handle system message if failed
        '-----------------------------------------------
        If Not blnPassLogin Then
            If (udtServiceProvider IsNot Nothing AndAlso udtServiceProvider.RecordStatus = ServiceProviderStatus.Delisted) OrElse _
                (strSPStatus IsNot Nothing AndAlso strSPStatus = ServiceProviderStatus.Delisted) Then

                'SP is delisted
                udtAuditLogEntry.AddDescripton(LoginBLL.AuditLogDesc.Field00000, LoginBLL.AuditLogDesc.MSG00006)

            ElseIf (udtServiceProvider IsNot Nothing AndAlso udtServiceProvider.RecordStatus = ServiceProviderStatus.Suspended) OrElse _
                (strSPStatus IsNot Nothing AndAlso strSPStatus = ServiceProviderStatus.Suspended) Then

                'Account was suspended.
                udtAuditLogEntry.AddDescripton(LoginBLL.AuditLogDesc.Field00000, LoginBLL.AuditLogDesc.MSG00010)

                'Account was suspended.
                udtSystemMessage = New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00060)

            ElseIf (udtServiceProvider IsNot Nothing AndAlso udtServiceProvider.UserACRecordStatus = "S") Then

                'Account was locked.
                udtAuditLogEntry.AddDescripton(LoginBLL.AuditLogDesc.Field00000, LoginBLL.AuditLogDesc.MSG00011)

                'Account was locked. Please back to login and click "Can't access to your account?" to recover your login.
                udtSystemMessage = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00003)

            End If

        End If

        Return blnPassLogin

    End Function

    Public Sub ProceedLogin(ByVal udtUserAC As UserACModel, ByRef udtAuditLogEntry As AuditLogEntry)
        Dim strLoginRole As String = udtUserAC.UserType
        Dim udtSP As ServiceProviderModel = Nothing
        Dim strSPID As String = String.Empty
        Dim strDataEntryAccount As String = Nothing

        Dim udtLoginBLL As New LoginBLL

        Dim strSPStatus As String = Nothing
        Dim strPassword As String = String.Empty
        Dim strPassCode As String = String.Empty

        '-----------------------------------------------
        'Step 6: Save validated user account in session
        '-----------------------------------------------
        udtUserACBLL.SaveToSession(udtUserAC)

        udtSP = UserACBLL.GetServiceProvider()

        If udtSP IsNot Nothing Then
            strSPID = udtSP.SPID
        End If

        '-----------------------------------------------
        'Step 7: Set language
        '-----------------------------------------------
        Session("language") = LoginBLL.LoadUserDefaultLanguage(udtUserAC.DefaultLanguage, Me.SubPlatform)
        MyBase.InitializeCulture()

        '-----------------------------------------------
        'Step 8: Generate page key if need
        '-----------------------------------------------
        If RedirectHandler.IsTurnOnConcurrentBrowserHandling Then
            KeyGenerator.RenewSessionPageKey()
        End If

        '-----------------------------------------------
        'Step 9: Go to change password page if need
        '-----------------------------------------------
        If udtLoginBLL.CheckChangePassword(udtUserAC) Then

            If Not udtUserAC.LastLoginDtm.HasValue Then
                'First change
                Session(SESS_FirstChangePassword) = YesNo.Yes

                Session(SESS_ChangePasswordUserAC) = udtUserAC

                'Service Provider (iAM Smart) Login successful(First logon change password)
                udtAuditLogEntry.WriteEndLog(LoginBLL.AuditLog_LogID.FirstChangePassword, LoginBLL.AuditLog_Prefix.SP_iAMSmart & LoginBLL.AuditLog.MSG00009, strSPID, strDataEntryAccount)
            Else
                'Force change
                Session(SESS_FirstChangePassword) = YesNo.No

                Session(SESS_ChangePasswordUserAC) = udtUserAC

                'Service Provider (iAM Smart) Login successful(Force logon change password)
                udtAuditLogEntry.WriteEndLog(LoginBLL.AuditLog_LogID.ForceChangePassword, LoginBLL.AuditLog_Prefix.SP_iAMSmart & LoginBLL.AuditLog.MSG00010, strSPID, strDataEntryAccount)

                Session.Remove(UserACBLL.SESS_USERAC)

            End If

            udtAuditLogEntry.WriteLog(LogID.LOG00051, AuditLog.MSG00051)

            RedirectHandler.ToURL(ClaimVoucherMaster.FullVersionPage.ChangePassword)

        End If

        '-----------------------------------------------
        'Step 10: Handle 4 level alert
        '-----------------------------------------------
        Session(SESS_Show4thLevelAlertD28) = udtLoginBLL.Handle4LevelAlert(strSPID, Me.SubPlatform)

        '-----------------------------------------------
        'Step 11: Go to home page
        '-----------------------------------------------
        udtLoginBLL.UpdateSuccessLoginDtm(udtUserAC)

        Dim udtIdeasBLL As New IdeasBLL
        udtIdeasBLL.UpdateIDEASComboInfo(udtUserAC, udcSessionHandler.IDEASComboClientGetFormSession(), udcSessionHandler.IDEASComboVersionGetFormSession())

        udtAuditLogEntry.WriteEndLog(LoginBLL.AuditLog_LogID.Success, LoginBLL.AuditLog_Prefix.SP_iAMSmart & LoginBLL.AuditLog.MSG00008, strSPID, strDataEntryAccount)

        udtAuditLogEntry.WriteLog(LogID.LOG00051, AuditLog.MSG00051)

        RedirectHandler.ToURL(ClaimVoucherMaster.FullVersionPage.Home)

    End Sub

    Public Sub HandleLoginFail(ByVal strSPID As String, ByVal strDataEntryAccount As String, ByVal strLoginRole As String, ByRef udtAuditLogEntry As AuditLogEntry)
        Dim udtLoginBLL As New LoginBLL

        udtLoginBLL.HandleUnsuccessLogin(strSPID, strDataEntryAccount, strLoginRole)

        udtAuditLogEntry.WriteEndLog(LoginBLL.AuditLog_LogID.Fail, LoginBLL.AuditLog_Prefix.SP_iAMSmart & LoginBLL.AuditLog.MSG00007, strSPID, strDataEntryAccount) 'Login failed. Please try again later.

    End Sub

#Region "Event"
    Protected Sub btnBack_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles ibtnGetProfileBackToLogin.Click, ibtnGoToEnrolmentBackToLogin.Click
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00044, AuditLog.MSG00044)

        'Me.ModalPopupConfirmCancel.Show()
        Response.Redirect(ClaimVoucherMaster.FullVersionPage.Login)
    End Sub

#Region "Confirm Cancel Popup function"
    Private Sub ucNoticePopUpConfirm_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUpConfirm.ButtonClick
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        Select Case e
            Case ucNoticePopUp.enumButtonClick.OK
                udtAuditLogEntry.WriteLog(LogID.LOG00030, AuditLog.MSG00030)
                Response.Redirect(ClaimVoucherMaster.FullVersionPage.Login)

            Case Else
                udtAuditLogEntry.WriteLog(LogID.LOG00031, AuditLog.MSG00031)
        End Select
    End Sub
#End Region

    Protected Sub btnBackToLoginBack_Click(sender As Object, e As EventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00044, AuditLog.MSG00044)

        Response.Redirect(ClaimVoucherMaster.FullVersionPage.Login)
    End Sub

    Protected Sub btniAMSmartOpenApp_Click(sender As Object, e As EventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00032, AuditLog.MSG00032)

        Dim btn As Button = CType(sender, Button)
        Dim strUrl As String = udcGeneralFun.GetSystemParameterParmValue1("IAMSmartAppLink")
        Dim strProfile As String = "://profile"

        Dim strAction As String = eService.Common.Constants.URL_ACTION '?
        Dim strAppend As String = eService.Common.Constants.URL_APPEND '&
        Dim strEqual As String = eService.Common.Constants.URL_EQUAL '=

        strUrl = strUrl & strProfile & strAction & "ticketID" & strEqual & btn.CommandArgument

        ScriptManager.RegisterStartupScript(Me, Page.GetType, "OpeniAMSmartAppScript", "OpeniAMSmartApp('" + strUrl + "');", True)

        'ModalPopupAuthoriseiAMSmart.Hide()
    End Sub

    Protected Sub ibtniAMSmartPopupCancel_Click(sender As Object, e As ImageClickEventArgs)
        Session.Remove(SESS_iAMSmart.ProfileRequestPopupShown)
        ModalPopupAuthoriseiAMSmart.Hide()

    End Sub

    Protected Sub btniAMSmartRequestProfile_Click(sender As Object, e As EventArgs)
        
        Dim striAMSmartMode As String = ConfigurationManager.AppSettings("iAMSmartMode")

        Select Case striAMSmartMode
            Case iAMSmartMode.Live
                ibtniAMSmartPopupCancel.Visible = False
                RequestProfile()
            Case iAMSmartMode.Stimulate
                ibtniAMSmartPopupCancel.Visible = True
                'RequestProfileDemo()
                RequestProfileStimulate()
        End Select
        
    End Sub

    Public Sub RequestProfile()
        Dim strState As String = CStr(HttpContext.Current.Session(SESS_iAMSmart.State))
        Dim strTokenID As String = CStr(HttpContext.Current.Session(SESS_iAMSmart.TokenID))
        Dim intAccessTokenTimeout = (New GeneralFunction).GetSystemParameterParmValue1("IAMSmartAccessTokenTimeout")
        Dim dtmAccessTokenStartTime As DateTime = HttpContext.Current.Session(SESS_iAMSmart.AccessTokenTimeout)

        Dim strLanguage As String = String.Empty
        Dim strTicketID As String = String.Empty
        Dim strReturnCode As String = Nothing
        Dim strMessage As String = String.Empty
        Dim blnAuthByQRCode As Nullable(Of Boolean) = Nothing

        Dim dtAccessToken As DataTable = Nothing

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00060, AuditLog.MSG00060)

        If DateDiff(DateInterval.Second, dtmAccessTokenStartTime, Date.Now) <= intAccessTokenTimeout Then ' 600 seconds = 10 minutes
            '--------------------------------------
            '---- Check the browser type 
            '--------------------------------------
            Dim strBrowser As String = iAMSmartSPBLL.CheckBrowserType(HttpContext.Current.Request.UserAgent)
            HttpContext.Current.Session(iAMSmartLogin.SESS_iAMSmart.BrowserType) = strBrowser

            '--------------------------------------
            '---- Generate Business ID
            '--------------------------------------
            Dim strBusinessID As String = iAMSmartSPBLL.GenerateBusinessID()

            '--------------------------------------------------
            '---- Store Business ID, State for authentication
            '--------------------------------------------------
            udtiAMSmartBLL.AddiAMSmartProfileLog(strBusinessID, strState, Nothing, Nothing, Nothing)

            HttpContext.Current.Session(SESS_iAMSmart.BusinessID) = strBusinessID

            '--------------------------------------------------
            '---- Store flag of AuthByQRCode
            '--------------------------------------------------
            HttpContext.Current.Session(SESS_iAMSmart.AuthByQRCode) = blnAuthByQRCode

            '---------------------------------
            '   Get Profile
            '---------------------------------
            'Get from IAMSmartAccessTokenLog table & fill into the model
            dtAccessToken = udtiAMSmartBLL.GetAccessTokenByOpenID_iAMSmart(strTokenID)

            If dtAccessToken IsNot Nothing Then
                strLanguage = GetSiteLanguage(HttpContext.Current.Session("language"))

                Dim lstProfileFields As List(Of String) = GetProfileFields()
                Dim udtProfileDto As eService.DTO.Response.ResponseDTO(Of eService.DTO.Response.ResProfileDTO) = Nothing

                udtProfileDto = (New iAMSmartSPBLL).RequestProfile(dtAccessToken, HttpContext.Current.Request.UserAgent, strState, strLanguage, strBusinessID, lstProfileFields, FUNCTION_CODE)

                If udtProfileDto IsNot Nothing Then
                    blnAuthByQRCode = udtProfileDto.Content.AuthByQR
                    strReturnCode = udtProfileDto.Code
                    strTicketID = udtProfileDto.Content.TicketID

                    'Update flag of AuthByQRCode
                    HttpContext.Current.Session(SESS_iAMSmart.AuthByQRCode) = blnAuthByQRCode
                End If

                'If return code is not nothing, get message by code
                If strReturnCode IsNot Nothing Then
                    strMessage = iAMSmartSPBLL.GetReturnCodeMessage(strReturnCode)
                End If

                If strReturnCode IsNot Nothing AndAlso iAMSmartSPBLL.GetReturnCodeEnum(strReturnCode) = eService.DTO.Enum.ReturnCode.SUCCESS Then
                    'Authorise success (call long polling)
                    udtAuditLogEntry.AddDescripton("Authorise By QR Code", IIf(blnAuthByQRCode, YesNo.Yes, YesNo.No))
                    udtAuditLogEntry.AddDescripton("Return Code", strReturnCode)
                    udtAuditLogEntry.AddDescripton("Return Code Message", strMessage)
                    udtAuditLogEntry.AddDescripton("TicketID", strTicketID)
                    udtAuditLogEntry.WriteLog(LogID.LOG00061, AuditLog.MSG00061)

                    HttpContext.Current.Session(SESS_iAMSmart.ProfileRequestTimeout) = Date.Now
                    HttpContext.Current.Session(SESS_iAMSmart.TicketID) = strTicketID

                    'Default show the mobile image
                    imgiAMSmartApp.Style.Remove("display")

                    'Default invisible the "Open iAM Smart" button
                    btnCalliAMSmartApp.Style.Add("display", "none")

                    'If strBrowser IsNot Nothing AndAlso strBrowser <> "PC_Browser" Then
                    If blnAuthByQRCode IsNot Nothing AndAlso Not blnAuthByQRCode Then
                        imgiAMSmartApp.Style.Add("display", "none")
                        btnCalliAMSmartApp.Style.Remove("display")
                        btnCalliAMSmartApp.CommandArgument = strTicketID
                    End If

                    'Show popup to guide user to open iAM Smart app
                    HttpContext.Current.Session(SESS_iAMSmart.ProfileRequestPopupShown) = YesNo.Yes
                    ModalPopupAuthoriseiAMSmart.Show()

                    'Set long polling for wait iAM Smart app callback
                    udtAuditLogEntry.AddDescripton("BusinessID", strBusinessID)
                    udtAuditLogEntry.WriteLog(LogID.LOG00063, AuditLog.MSG00063)
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "GetProfileScript", "SetPolling('" + strBusinessID + "');", True)

                Else
                    'Authorise fail, return code is not D00000
                    udtAuditLogEntry.AddDescripton("Return Code", strReturnCode)
                    udtAuditLogEntry.AddDescripton("Return Code Message", strMessage)
                    udtAuditLogEntry.WriteLog(LogID.LOG00062, AuditLog.MSG00062)

                    RedirectToLogin()

                    udtAuditLogEntry.WriteLog(LogID.LOG00045, AuditLog.MSG00045)

                    'udcErrorMessage.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00099)
                    'udcErrorMessage.BuildMessageBox("")

                End If

            Else
                udtAuditLogEntry.AddDescripton("Internal Exception, Access Token is not found by TokenID", strTokenID)
                udtAuditLogEntry.WriteLog(LogID.LOG00062, AuditLog.MSG00062)

                udcErrorMessage.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00099)
                udcErrorMessage.BuildMessageBox("")

            End If

        Else
            'Timeout
            udtAuditLogEntry.WriteLog(LogID.LOG00064, AuditLog.MSG00064)

            BackToLogin()

            udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00005) 'System timeout. Please try again later.
            udcInfoMessageBox.BuildMessageBox()

        End If

    End Sub

    Public Sub RequestProfileDemo()
        Dim strState As String = CStr(Session(SESS_iAMSmart.State))
        Dim strTokenID As String = CStr(Session(SESS_iAMSmart.TokenID))
        Dim intAccessTokenTimeout = (New GeneralFunction).GetSystemParameterParmValue1("IAMSmartAccessTokenTimeout")
        Dim dtmAccessTokenStartTime As DateTime = Session(SESS_iAMSmart.AccessTokenTimeout)

        Dim strLanguage As String = String.Empty
        Dim strTicketID As String = String.Empty
        Dim strReturnCode As String = Nothing
        Dim strMessage As String = String.Empty

        Dim dtAccessToken As DataTable = Nothing

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00000, "Request Profile Click")

        If DateDiff(DateInterval.Second, dtmAccessTokenStartTime, Date.Now) <= intAccessTokenTimeout Then ' 600 seconds = 10 minutes
            '--------------------------------------
            '---- Check the browser type 
            '--------------------------------------
            Dim strBrowser As String = iAMSmartSPBLL.CheckBrowserType(HttpContext.Current.Request.UserAgent)
            Session(iAMSmartLogin.SESS_iAMSmart.BrowserType) = strBrowser

            '--------------------------------------
            '---- Generate Business ID
            '--------------------------------------
            'Dim strBusinessID As String = DemoSampleData.BusinessID
            Dim strBusinessID As String = "bae67725fce34d4f816e2eb1aa3f1cd3"

            '--------------------------------------------------
            '---- Store Business ID, State for authentication
            '--------------------------------------------------
            'udtiAMSmartBLL.AddiAMSmartProfileLog(strBusinessID, strState, Nothing, Nothing, Nothing)

            HttpContext.Current.Session(SESS_iAMSmart.ProfileRequestTimeout) = Date.Now
            HttpContext.Current.Session(SESS_iAMSmart.TicketID) = "123456"

            'Default show the mobile image
            imgiAMSmartApp.Style.Remove("display")

            'Default invisible the "Open iAM Smart" button
            btnCalliAMSmartApp.Style.Add("display", "none")

            If strBrowser IsNot Nothing AndAlso strBrowser <> "PC_Browser" Then
                imgiAMSmartApp.Style.Add("display", "none")
                btnCalliAMSmartApp.Style.Remove("display")
                btnCalliAMSmartApp.CommandArgument = "123456"
            End If

            Session(SESS_iAMSmart.ProfileRequestPopupShown) = YesNo.Yes
            ModalPopupAuthoriseiAMSmart.Show()

            Session(SESS_iAMSmart.BusinessID) = strBusinessID

            ScriptManager.RegisterStartupScript(Me, Page.GetType, "GetProfileScript", "SetPolling('" + strBusinessID + "');", True)

        Else
            'Timeout
            BackToLogin()
            udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00005) 'System timeout. Please try again later.
            udcInfoMessageBox.BuildMessageBox()

        End If

    End Sub

    Protected Sub btnHandleProfileCallback_Click(sender As Object, e As EventArgs)
        Dim strDemoVersion As String = ConfigurationManager.AppSettings("iAMSmartMode")

        Dim udtDB As New Database
        Dim udtServiceProviderBLL As New ServiceProviderBLL
        Dim udtiAMSmartBLL As New iAMSmart.iAMSmartBLL

        Dim strBusinessID As String = CStr(Session(SESS_iAMSmart.BusinessID))
        Dim strOpenID As String = CStr(Session(SESS_iAMSmart.OpenID))

        Dim strHKID As String = String.Empty
        Dim strSPID As String = String.Empty
        Dim strStatus As String = String.Empty
        Dim strMsgCode As String = String.Empty
        Dim strMessage As String = "Message"
        Dim blnValid As Boolean = True

        Dim dtSPInfo As DataTable = Nothing
        Dim dtProfileDto As DataTable = Nothing

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00066, AuditLog.MSG00066)

        Session.Remove(SESS_iAMSmart.ProfileRequestPopupShown)
        ModalPopupAuthoriseiAMSmart.Hide()

        'udtAuditLogEntry.WriteLog(LogID.LOG00000, "Profile Callback:businessID" + strBusinessID)

        dtProfileDto = udtiAMSmartBLL.GetiAMSmartProfileLog(strBusinessID)
        'dtProfileDto = udtiAMSmartBLL.GetIAMSmartProfileLog("3e47be25-66a6-43fb-89f6-7e2dd138aff8")

        ' udtAuditLogEntry.WriteLog(LogID.LOG00000, "Profile Callback:dtProfileDto" + dtProfileDto.Rows(0).Item("Encrypted_Content"))

        If dtProfileDto IsNot Nothing Then
            If (iAMSmartSPBLL.GetReturnCodeEnum(dtProfileDto.Rows(0).Item("Return_Code").ToString) = eService.DTO.Enum.ReturnCode.SUCCESS) Then
                'check the SP's HKID
                strHKID = udtiAMSmartSPBLL.GetHKIDFromContent(dtProfileDto.Rows(0).Item("Encrypted_Content"))

                'If strDemoVersion = "L" Then
                '    'strHKID = DemoSampleData.HKIC
                '    strHKID = "UP9000480"
                '    strHKID = " G5034050"
                'End If

                If strHKID Is String.Empty Then
                    'SP cancel submit the profile
                    udtAuditLogEntry.WriteLog(LogID.LOG00062, AuditLog.MSG00062)
                    BackToLogin()

                    'iAM Smart login cancelled.
                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00009)
                    udcInfoMessageBox.BuildMessageBox()

                    ''Login failed. Please try again later.
                    'udcErrorMessage.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002, "%s", String.Format("({0})", "D40001")) 'D40001 - Reject Request
                    'udcErrorMessage.BuildMessageBox("ValidationFail")

                Else
                    'strHKID = (New Formatter).formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, udtiAMSmartSPBLL.GetHKIDFromContent(dtProfileDto.Rows(0).Item("Encrypted_Content")))
                    udtAuditLogEntry.AddDescripton("DocNo", strHKID)
                    udtAuditLogEntry.WriteLog(LogID.LOG00067, AuditLog.MSG00067)
                    dtSPInfo = udtServiceProviderBLL.GetServiceProviderParticulasPermanentByHKID(strHKID, udtDB)

                    If dtSPInfo IsNot Nothing Then
                        If dtSPInfo.Rows.Count > 1 Then
                            For Each dr As DataRow In dtSPInfo.Rows
                                If dr("Record_Status").ToString.Trim <> Formatter.EnumToString(ServiceProvider.ServiceProviderModel.RecordStatusEnumClass.Delisted) Then
                                    strStatus = dr("Record_Status").ToString.Trim
                                    strSPID = dr("SP_ID").ToString.Trim
                                    Exit For
                                End If
                            Next
                        Else
                            strStatus = dtSPInfo.Rows(0).Item("Record_Status").ToString.Trim
                            strSPID = dtSPInfo.Rows(0).Item("SP_ID").ToString.Trim
                        End If
                    End If

                    If dtSPInfo IsNot Nothing AndAlso _
                        strStatus <> Formatter.EnumToString(ServiceProvider.ServiceProviderModel.RecordStatusEnumClass.Delisted) Then

                        'Match SP HKID and update the IAMSmartSPMapping table
                        Dim dtSPMapping As DataTable = udtiAMSmartBLL.GetServiceProviderBySPID_iAMSmart(strSPID)

                        If dtSPMapping IsNot Nothing Then
                            blnValid = False
                            strMsgCode = MsgCode.MSG00010
                        Else
                            blnValid = udtiAMSmartBLL.AddiAMSmartAccountToIAMSmartSPMapping(strSPID, strOpenID)
                            strMsgCode = MsgCode.MSG00003
                        End If

                        If Not blnValid Then
                            'audit log
                            udtAuditLogEntry.WriteLog(LogID.LOG00068, AuditLog.MSG00068)
                            BackToLogin()

                            If strMsgCode = MsgCode.MSG00001 Then
                                udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, strMsgCode, "%s", strMessage)
                            Else
                                udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, strMsgCode)
                            End If

                            udcInfoMessageBox.BuildMessageBox()
                        Else
                            'complete the registration, go to home page
                            udtAuditLogEntry.WriteLog(LogID.LOG00069, AuditLog.MSG00069)
                            Me.mvStepForiAMSmart.ActiveViewIndex = ActiveViewIndex.LinkupSPAccount
                            Me.ibtnGoToHome.CommandArgument = strSPID
                            udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00002)
                            udcInfoMessageBox.BuildMessageBox()
                        End If
                    Else
                        'Not match SP HKID
                        udtAuditLogEntry.WriteLog(LogID.LOG00068, AuditLog.MSG00068)
                        Me.mvStepForiAMSmart.ActiveViewIndex = ActiveViewIndex.NoSPAccountFound

                        udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003)
                        udcInfoMessageBox.BuildMessageBox()

                    End If
	        	End If
            Else
                'Return code not D00000
                BackToLogin()

                udtAuditLogEntry.AddDescripton("Return Code", dtProfileDto.Rows(0).Item("Return_Code").ToString)
                udtAuditLogEntry.AddDescripton("Return Code Message", dtProfileDto.Rows(0).Item("Message").ToString)

                udtAuditLogEntry.WriteLog(LogID.LOG00077, AuditLog.MSG00077)

                udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00006) 'Submission of personal data failed, please try again.
                udcInfoMessageBox.BuildMessageBox()
            End If

        Else
            'Default 
            BackToLogin()

            udtAuditLogEntry.AddDescripton("Exception", "dtProfileDto is nothing")

            udtAuditLogEntry.WriteLog(LogID.LOG00077, AuditLog.MSG00077)

            udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00006) 'Submission of personal data failed, please try again.
            udcInfoMessageBox.BuildMessageBox()

        End If

    End Sub

    Protected Sub btnHandleProfileCallbackTimeout_Click(sender As Object, e As EventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00034, AuditLog.MSG00034)

        Session.Remove(SESS_iAMSmart.ProfileRequestPopupShown)
        ModalPopupAuthoriseiAMSmart.Hide()

        BackToLogin()

        udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00005) 'Submission operation timed out, please try again.
        udcInfoMessageBox.BuildMessageBox()

    End Sub

    Protected Sub btnHandleProfileCallbackError_Click(sender As Object, e As EventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00035, AuditLog.MSG00035)

        Session.Remove(SESS_iAMSmart.ProfileRequestPopupShown)
        ModalPopupAuthoriseiAMSmart.Hide()

        BackToLogin()

        udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00006) 'Submission of personal data failed, please try again.
        udcInfoMessageBox.BuildMessageBox()

    End Sub

    Protected Sub ibtnGoEnrol_Click(sender As Object, e As EventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00036, AuditLog.MSG00036)

        Dim strURL As String = udcGeneralFun.GetSystemParameterParmValue1("AppLink") + "/" + _
                               udcGeneralFun.GetSystemParameterParmValue1("eFormLink") + _
                               "?lang=" + Left(Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower, 2)
        ClearSession()

        Response.Redirect(strURL)

    End Sub

    Protected Sub btnGoToEnrolmentBackToLogin_Click(sender As Object, e As EventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00037, AuditLog.MSG00037)
        ClearSession()
        Response.Redirect(ClaimVoucherMaster.FullVersionPage.Login)
    End Sub

    Protected Sub btnGoToHome_Click(sender As Object, e As EventArgs) Handles ibtnGoToHome.Click
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE)
        Dim udtUserAC As UserACModel = Nothing
        Dim udtSystemMessage As SystemMessage = Nothing

        udtAuditLogEntry.WriteLog(LogID.LOG00038, AuditLog.MSG00038)

        If LoginValidation(ibtnGoToHome.CommandArgument, udtUserAC, udtAuditLogEntry, udtSystemMessage) Then
            'Valid, check change password if need

            ProceedLogin(udtUserAC, udtAuditLogEntry)

        Else
            'Error, back to login
            HandleLoginFail(ibtnGoToHome.CommandArgument, Nothing, SPAcctType.ServiceProvider, udtAuditLogEntry)

            udtAuditLogEntry.WriteLog(LogID.LOG00052, AuditLog.MSG00052)

            BackToLogin()

            If udtSystemMessage IsNot Nothing Then
                udcErrorMessage.AddMessage(udtSystemMessage)
                udcErrorMessage.BuildMessageBox(LoginBLL.AuditLogDesc.Header00001)
            Else
                'Login failed. Please try again later.
                udcErrorMessage.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002, "%s", String.Empty)
                udcErrorMessage.BuildMessageBox(LoginBLL.AuditLogDesc.Header00001)

            End If

        End If

    End Sub

#End Region

#Region "Supported Functions"
    Public Function GetProfileFields() As List(Of String)
        Dim strSelectedProfile As String = ConfigurationManager.AppSettings("ProfileList")
        Dim strSelectedProfileFields() As String = Split(strSelectedProfile, ",")
        Dim lstAllProfileFields As List(Of String) = Profile.List
        Dim lstAllProfileFieldsDesc As List(Of String) = Profile.ListDesc
        Dim lstSelectedProfileFields As New List(Of String)

        For intSelected As Integer = 0 To strSelectedProfileFields.Length - 1
            For intAll As Integer = 0 To lstAllProfileFieldsDesc.Count - 1
                If strSelectedProfileFields(intSelected).ToUpper.Trim = lstAllProfileFieldsDesc.Item(intAll).ToUpper.Trim Then
                    lstSelectedProfileFields.Add(lstAllProfileFields.Item(intAll))
                    Exit For
                End If
            Next
        Next

        Return lstSelectedProfileFields

    End Function

    Public Function GetProfileFieldsDesc() As List(Of String)
        Dim strSelectedProfile As String = ConfigurationManager.AppSettings("ProfileList")
        Dim strSelectedProfileFields() As String = Split(strSelectedProfile, ",")
        Dim lstAllProfileFields As List(Of String) = Profile.ListDesc
        Dim lstSelectedProfileFields As New List(Of String)

        For intSelected As Integer = 0 To strSelectedProfileFields.Length - 1
            For intAll As Integer = 0 To lstAllProfileFields.Count - 1
                If strSelectedProfileFields(intSelected).ToUpper.Trim = lstAllProfileFields.Item(intAll).ToUpper.Trim Then
                    lstSelectedProfileFields.Add(lstAllProfileFields.Item(intAll))
                    Exit For
                End If
            Next
        Next

        Return lstSelectedProfileFields

    End Function

    Public Shared Function GetSiteLanguage(ByVal strSystemLang As String) As String
        Dim strLanguage As String = String.Empty

        Select Case LCase(strSystemLang)
            Case CultureLanguage.English
                strLanguage = "en-US"
            Case CultureLanguage.TradChinese
                strLanguage = "zh-HK"
            Case CultureLanguage.SimpChinese
                strLanguage = "zh-CN"
        End Select

        Return strLanguage
    End Function

    Public Function CheckStateCode(ByVal strState As String, ByVal strCookie As String, ByRef udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim blnRes As Boolean = False
        Dim blnTurnOn As Boolean = IIf(udcGeneralFun.GetSystemParameterParmValue1("IAMSmartTurnOnCheckState") = YesNo.Yes, True, False)

        'strStateSession = eService.Common.SessionHelper.GetSession(eService.Common.Constants.ESERVICE_STATE)

        If blnTurnOn Then
            'Check state code whether issued from EHS
            Dim dt As DataTable = udtiAMSmartBLL.GetiAMSmartState(strState)

            If dt Is Nothing Then
                udtAuditLogEntry.AddDescripton("State code from query string", strState)
                udtAuditLogEntry.WriteLog(LogID.LOG00005, AuditLog.MSG00005)
                Return False
            End If

            'Check Cookie when Direct Login used
            If dt.Rows(0).Item("CookieKey") = "1" Then
                '"1" means Cookie is found and matched by message digest process; "0" failed the checking
                If strCookie = "1" Then
                    blnRes = True
                Else
                    udtAuditLogEntry.AddDescripton("Flag of Cookie from query string", strCookie)
                    udtAuditLogEntry.WriteLog(LogID.LOG00006, AuditLog.MSG00006)
                    blnRes = False
                End If
            Else
                'Not check Cookie when passed from website login
                blnRes = True

            End If

        Else
            blnRes = True 'Always true when the checking is turned off

        End If

        Return blnRes

    End Function

    Public Function GenerateOpeniAMSmartAppLink() As String
        Dim strUrl As String = udcGeneralFun.GetSystemParameterParmValue1("IAMSmartAppLink")
        Dim strProfile As String = "://profile"
        Dim strTicketID As String = String.Empty

        'If Session(SESS_iAMSmart.TicketID) IsNot Nothing Then
        '    strTicketID = Session(SESS_iAMSmart.TicketID)
        'End If

        Dim strAction As String = eService.Common.Constants.URL_ACTION '?
        Dim strAppend As String = eService.Common.Constants.URL_APPEND '&
        Dim strEqual As String = eService.Common.Constants.URL_EQUAL '=

        strUrl = strUrl & strProfile & strAction & "ticketID" & strEqual & strTicketID

        Return strUrl

    End Function

    Private Sub HandleSessionVariable()
        Dim Cache1 As String = Nothing
        Dim Cache2 As Boolean = Nothing
        Dim Cache3 As Boolean = Nothing
        Dim Cache4 As Boolean = False
        Dim Cache5 As String = Nothing
        Dim Cache6 As String = Nothing
        'CRE20-006 DHC Integration [Start][Nichole]
        Dim Cache7 As String = Nothing
        Dim Cache8 As New DHCClaim.DHCClaimBLL.DHCPersonalInformationModel
        'CRE20-006 DHC Integration [End][Nichole]

        '1a. language
        If Not Session("language") Is Nothing Then
            Cache1 = Session("language")
        End If

        '2a. Undefined User Agent
        Cache2 = CommonSessionHandler.AddedUndefinedUserAgent

        '3a. Popup for remind obsoleted OS
        Cache3 = CommonSessionHandler.ReminderForWindowsVersion

        '4a. Popup for enable to allow popup
        Cache4 = udcSessionHandler.PopupBlockerGetFromSession()

        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        '5a. IDEAS Combo Installation Result
        Cache5 = udcSessionHandler.IDEASComboClientGetFormSession()

        '6a. IDEAS Combo Version
        Cache6 = udcSessionHandler.IDEASComboVersionGetFormSession()
        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

        'CRE20-006 DHC Integration [Start][Nichole]
        '7. DHC parameters Artifact bypass function
        Cache7 = udcSessionHandler.ArtifactGetFromSession()

        '8 DHCCLAIM model information
        Cache8 = udcSessionHandler.DHCInfoForLoginGetFromSession()
        'CRE20-006 DHC Integration [End][Nichole]

        'Clear
        Session.RemoveAll()

        '1b. language
        If Not Cache1 Is Nothing Then
            Session("language") = Cache1
        End If

        '2b. Undefined User Agent
        CommonSessionHandler.AddedUndefinedUserAgent = Cache2

        '3b. Popup for remind obsoleted OS
        CommonSessionHandler.ReminderForWindowsVersion = Cache3

        '4b. Popup for enable to allow popup
        udcSessionHandler.PopupBlockerSaveToSession(Cache4)

        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        '5b. IDEAS Combo Installation Result
        udcSessionHandler.IDEASComboClientSaveToSession(Cache5)

        '6b. IDEAS Combo Version
        udcSessionHandler.IDEASComboVersionSaveToSession(Cache6)
        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

        'CRE20-006 DHC Integration [Start][Nichole]
        '7. DHC parameters Artifact bypass function
        udcSessionHandler.ArtifactSaveToSession(Cache7)

        '8. DHC Claim model
        udcSessionHandler.DHCInfoForLoginSaveToSession(Cache8)
        'CRE20-006 DHC Integration [End][Nichole]
    End Sub

    Private Sub GetSampleData()
        Dim path As String = ConfigurationManager.AppSettings("iAMSmartSampleFilePath")
        Dim xml As New XmlSerializer(GetType(DemoSample))

        If Not String.IsNullOrEmpty(path) Then
            If System.IO.File.Exists(path) Then
                Dim sr As New StreamReader(path)

                DemoSampleData = xml.Deserialize(sr)

                sr.Close()
            End If

        End If

    End Sub

    Private Sub ClearSession()
        Session(SESS_iAMSmart.OpenID) = Nothing
        Session(SESS_iAMSmart.BusinessID) = Nothing
        Session(SESS_iAMSmart.TokenID) = Nothing
        Session(SESS_iAMSmart.AccessTokenTimeout) = Nothing
        Session(SESS_iAMSmart.State) = Nothing
        Session(SESS_iAMSmart.ProfileRequestTimeout) = Nothing
        Session(SESS_iAMSmart.BrowserType) = Nothing
    End Sub

    Private Sub ShowInvalidMsg()
        udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001, "%s", "Your login is failed. Please backto login and use ""Can't access to your account?"" to recover your login first.")
        udcInfoMessageBox.BuildMessageBox()
    End Sub
#End Region

#Region "Stimuate Mode"
    Protected Sub btnSimulateRandom_Click(sender As Object, e As EventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00070, AuditLog.MSG00070)

        Dim validchars As String = "abcdefghijklmnopqrstuvwxyz1234567890"
        Dim sb As New StringBuilder()
        Dim rand As New Random()
        For i As Integer = 1 To 64
            Dim idx As Integer = rand.Next(0, validchars.Length)
            Dim randomChar As Char = validchars(idx)
            sb.Append(randomChar)
        Next i
        Dim randomString As String = sb.ToString()

        txtOpenID.Text = randomString

    End Sub

    Protected Sub btnSimulateConnect_Click(sender As Object, e As EventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00071, AuditLog.MSG00071)

        Dim strOpenID = txtOpenID.Text.Trim

        If String.IsNullOrEmpty(strOpenID) = False Then
            HandleWebSiteStimulate()
        End If


        If String.IsNullOrEmpty(strOpenID) = False Then

        End If
    End Sub


    Protected Sub HandleWebSiteStimulate()
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE)
        Dim strTokenID As String = String.Empty
        Dim strOpenID As String = String.Empty
        Dim strReturnCode As String = String.Empty
        Dim strMessage As String = String.Empty

        udtAuditLogEntry.WriteLog(LogID.LOG00072, AuditLog.MSG00072)

        Dim strDemoVersion As String = ConfigurationManager.AppSettings("DemoVersion")

        Dim blnResGetAccessToken As Boolean = False

        blnResGetAccessToken = True
        strOpenID = txtOpenID.Text
        'strOpenID = "liR14%2BvX%2F5hSum5uf4ERczu0KcDnIJA5BM7FoM1ag9c%3DX"
        strReturnCode = "D00000"
        strMessage = "Success"

        Session(SESS_iAMSmart.TokenID) = strTokenID
        Session(SESS_iAMSmart.OpenID) = strOpenID
        Session(SESS_iAMSmart.AccessTokenTimeout) = Date.Now

        Dim dtiAMSmartSPMapping As DataTable = CheckiAMSmartLinkup(strOpenID)

        If dtiAMSmartSPMapping IsNot Nothing Then
            'Found, go to validation before login
            Dim udtUserAC As UserACModel = Nothing
            Dim udtSystemMessage As SystemMessage = Nothing

            If LoginValidation(dtiAMSmartSPMapping.Rows(0)("SP_ID").ToString.Trim, udtUserAC, udtAuditLogEntry, udtSystemMessage) Then
                udtAuditLogEntry.WriteLog(LogID.LOG00073, AuditLog.MSG00073)

                ProceedLogin(udtUserAC, udtAuditLogEntry)

            Else
                'Error, back to login
                udtAuditLogEntry.WriteLog(LogID.LOG00074, AuditLog.MSG00074)

                'udtAuditLogEntry.WriteLog(LogID.LOG00000, "iAM Smart - failed to match service provider")
                HandleLoginFail(dtiAMSmartSPMapping.Rows(0)("SP_ID").ToString.Trim, Nothing, SPAcctType.ServiceProvider, udtAuditLogEntry)

                BackToLogin()

                If udtSystemMessage IsNot Nothing Then
                    udcErrorMessage.AddMessage(udtSystemMessage)
                    udcErrorMessage.BuildMessageBox(LoginBLL.AuditLogDesc.Header00001, udtAuditLogEntry, LogID.LOG00052, AuditLog.MSG00052)
                Else
                    If udcErrorMessage Is Nothing Then
                        udcErrorMessage.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00134)
                        udcErrorMessage.BuildMessageBox(LoginBLL.AuditLogDesc.Header00001, udtAuditLogEntry, AuditLog.MSG00052, LogID.LOG00052, dtiAMSmartSPMapping.Rows(0)("SP_ID").ToString.Trim, Nothing)
                    End If
                End If
            End If

        Else
            'Not found, go to get profile 
            udtAuditLogEntry.WriteLog(LogID.LOG00075, AuditLog.MSG00075)

            Me.mvStepForiAMSmart.ActiveViewIndex = ActiveViewIndex.GetProfile
        End If
    End Sub

    Protected Sub btnSimulateHKICSubmit_Click(sender As Object, e As EventArgs)

        Dim udtDB As New Database
        Dim udtServiceProviderBLL As New ServiceProviderBLL
        Dim udtiAMSmartBLL As New iAMSmart.iAMSmartBLL

        Dim strBusinessID As String = CStr(Session(SESS_iAMSmart.BusinessID))
        Dim strOpenID As String = CStr(Session(SESS_iAMSmart.OpenID))

        Dim strHKID As String = txtHKIC.Text.Trim
        Dim strSPID As String = String.Empty
        Dim strMsgCode As String = String.Empty
        Dim strMessage As String = "Message"
        Dim blnValid As Boolean = True

        Dim dtSPInfo As DataTable = Nothing
        Dim dtProfileDto As DataTable = Nothing

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00076, AuditLog.MSG00076)
        ModalPopupAuthoriseiAMSmart.Hide()

        If String.IsNullOrEmpty(strHKID) = False Then
            dtSPInfo = udtServiceProviderBLL.GetServiceProviderParticulasPermanentByHKID(strHKID, udtDB)


            If dtSPInfo IsNot Nothing Then
                strSPID = dtSPInfo.Rows(0).Item("SP_ID").ToString.Trim
                'Match SP HKID and update the IAMSmartSPMapping table
                Dim dtSPMapping As DataTable = udtiAMSmartBLL.GetServiceProviderBySPID_iAMSmart(strSPID)

                If dtSPMapping IsNot Nothing Then
                    blnValid = False
                    strMsgCode = MsgCode.MSG00010
                Else
                    blnValid = udtiAMSmartBLL.AddiAMSmartAccountToIAMSmartSPMapping(strSPID, strOpenID)
                    strMsgCode = MsgCode.MSG00003
                End If

                If Not blnValid Then
                    'audit log
                    BackToLogin()

                    If strMsgCode = MsgCode.MSG00001 Then
                        udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, strMsgCode, "%s", strMessage)
                    Else
                        udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, strMsgCode)
                    End If

                    udcInfoMessageBox.BuildMessageBox()
                Else
                    'complete the registration, go to home page
                    Me.mvStepForiAMSmart.ActiveViewIndex = ActiveViewIndex.LinkupSPAccount
                    Me.ibtnGoToHome.CommandArgument = dtSPInfo.Rows(0).Item("SP_ID").ToString.Trim
                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00002)
                    udcInfoMessageBox.BuildMessageBox()
                End If
            Else
                'Not match SP HKID
                Me.mvStepForiAMSmart.ActiveViewIndex = ActiveViewIndex.NoSPAccountFound

                udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003)
                udcInfoMessageBox.BuildMessageBox()
            End If
        End If
        ModalPopupStimulateProfile.Hide()

    End Sub

    Public Sub RequestProfileStimulate()
        Dim strState As String = CStr(Session(SESS_iAMSmart.State))
        Dim strTokenID As String = CStr(Session(SESS_iAMSmart.TokenID))
        Dim intAccessTokenTimeout = (New GeneralFunction).GetSystemParameterParmValue1("IAMSmartAccessTokenTimeout")
        Dim dtmAccessTokenStartTime As DateTime = Session(SESS_iAMSmart.AccessTokenTimeout)

        Dim strLanguage As String = String.Empty
        Dim strTicketID As String = String.Empty
        Dim strReturnCode As String = Nothing
        Dim strMessage As String = String.Empty

        Dim dtAccessToken As DataTable = Nothing

        'Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        'udtAuditLogEntry.WriteLog(LogID.LOG00000, "Request Profile Click")

        If DateDiff(DateInterval.Second, dtmAccessTokenStartTime, Date.Now) <= intAccessTokenTimeout Then ' 600 seconds = 10 minutes
            '--------------------------------------
            '---- Check the browser type 
            '--------------------------------------
            Dim strBrowser As String = iAMSmartSPBLL.CheckBrowserType(HttpContext.Current.Request.UserAgent)
            Session(iAMSmartLogin.SESS_iAMSmart.BrowserType) = strBrowser

            '--------------------------------------
            '---- Generate Business ID
            '--------------------------------------
            'Dim strBusinessID As String = DemoSampleData.BusinessID
            Dim strBusinessID As String = "bae67725fce34d4f816e2eb1aa3f1cd3"

            '--------------------------------------------------
            '---- Store Business ID, State for authentication
            '--------------------------------------------------

            HttpContext.Current.Session(SESS_iAMSmart.ProfileRequestTimeout) = Date.Now

            'Default show the mobile image
            imgiAMSmartApp.Style.Remove("display")

            'Default invisible the "Open iAM Smart" button
            btnCalliAMSmartApp.Style.Add("display", "none")

            If strBrowser IsNot Nothing AndAlso strBrowser <> "PC_Browser" Then
                imgiAMSmartApp.Style.Add("display", "none")
                btnCalliAMSmartApp.Style.Remove("display")
                btnCalliAMSmartApp.CommandArgument = "123456"
            End If

            Session(SESS_iAMSmart.ProfileRequestPopupShown) = YesNo.Yes
            ModalPopupStimulateProfile.Show()

            Session(SESS_iAMSmart.BusinessID) = strBusinessID
        Else
            'Timeout
            BackToLogin()
            udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00005) 'System timeout. Please try again later.
            udcInfoMessageBox.BuildMessageBox()

        End If

    End Sub

    Protected Sub btnSimulateBack_Click(sender As Object, e As EventArgs)
        RedirectHandler.ToURL(ClaimVoucherMaster.FullVersionPage.Login)
    End Sub

    Private Sub BackToLogin()
        Me.udcErrorMessage.Visible = False
        Me.udcInfoMessageBox.Visible = False
        Me.mvStepForiAMSmart.ActiveViewIndex = ActiveViewIndex.BackToLogin
    End Sub

    Protected Sub RedirectToLogin()
        RedirectHandler.ToURL(ClaimVoucherMaster.FullVersionPage.Login)
    End Sub

#End Region

#Region "Handle ajax callback"
    <WebMethod(Enablesession:=True)>
    Public Shared Function CheckBussinesID(ByVal BusinessID As String, ByVal intCount As String) As String
        Dim udtiAMSmartBLL As New Common.Component.iAMSmart.iAMSmartBLL
        Dim dtProfile As DataTable = Nothing
        Dim strStatus As String = String.Empty

        Dim dtmNow As DateTime = Date.Now
        Dim dtmPollingStartTime = HttpContext.Current.Session(SESS_iAMSmart.ProfileRequestTimeout)
        Dim intRequestProfileTimeout = (New GeneralFunction).GetSystemParameterParmValue1("IAMSmartProfileRequestTimeout")
        Dim udtAuditLogEntry As New AuditLogEntry(Common.Component.FunctCode.FUNT020009)

        If DateDiff(DateInterval.Second, dtmPollingStartTime, dtmNow) < intRequestProfileTimeout Then ' 660 seconds = 11 minutes
            'Check the table
            dtProfile = udtiAMSmartBLL.GetiAMSmartProfileLog(BusinessID)
            'dtProfile = udtiAMSmartBLL.GetIAMSmartProfileLog("3e47be25-66a6-43fb-89f6-7e2dd138aff8")

            If dtProfile IsNot Nothing AndAlso dtProfile.Rows.Count > 0 Then
                strStatus = "Found"
                udtAuditLogEntry.AddDescripton("Result", strStatus)
                udtAuditLogEntry.WriteLog(LogID.LOG00065, AuditLog.MSG00065 + " (" + intCount + ")")
            Else
                strStatus = "NotFound"
            End If

        Else
            'Timeout
            strStatus = "Timeout"

        End If

        Return strStatus

    End Function

    <WebMethod(Enablesession:=True)>
    Public Shared Function GetTicketID() As String
        Dim strTicketID = HttpContext.Current.Session(SESS_iAMSmart.TicketID)

        Return strTicketID

    End Function

#End Region

End Class