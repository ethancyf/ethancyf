﻿Imports Common.Component.HCVUUser
Imports Common.Component.UserAC
Imports Common.ComObject
Imports Common.Component.UserRole
Imports Common.Component.AccessRight
Imports HCVU.Component.Menu
Imports HCVU.Component.RoleSecurity
Imports Common.Validation
Imports Common.Encryption
Imports HCVU.BLL
Imports Common.Component
Imports Common.Component.RSA_Manager
Imports Common.ComFunction
Imports Common.Component.NewsMessage
Imports Common.Format
Imports System.Threading
Imports Common.ComFunction.AccountSecurity
Imports HCVU.Component.RoleType

Partial Public Class login
    Inherits BasePage

    Private Const SESS_ChangePasswordHCVUUser As String = "ChangePasswordHCVUUser"
    Dim udtGeneralFunction As New GeneralFunction

    ' CRE19-026 (HCVS hotline service) [Start][Winnie]
    ' ------------------------------------------------------------------------
    Public Class PageURL
        Public Const Home As String = "~/Home/home.aspx"
        Public Const ChangePassword As String = "~/ChangePassword/LoginChangePassword.aspx"
    End Class

    Private Class AuditLogDesc
        Public Const Msg00023 As String = "Concurrent login detected, Prompt to confirm login"
        Public Const Msg00024 As String = "Confirm login click"
        Public Const Msg00025 As String = "Cancel login click"
    End Class

    Private Class SystemParameters
        Public Const FAQsLinkHCVU As String = "FAQsLink_HCVU"
        Public Const DaysOfChangePassword As String = "DaysOfChangePassword"
    End Class

    Private Const SESS_Password As String = "Password"
    Private Const SESS_Passcode As String = "Passcode"

    ' CRE19-026 (HCVS hotline service) [End][Winnie]

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.basetag.Attributes("href") = udtGeneralFunction.getPageBasePath()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        '[CRE11-016] Concurrent Browser Handling [2010-02-01] Start
        Me.LinkButtonExplainConcurrentAccess.Text = Me.GetGlobalResourceObject("Text", "ExplainConcurrentAccess")
        Me.LinkButtonExplainConcurrentAccess.OnClientClick = RedirectHandler.GetLinkButtonPopupJS("ConcurrentFAQsLink", "", "")
        '[CRE11-016] Concurrent Browser Handling [2010-02-01] End

        Me.FunctionCode = FunctCode.FUNT010001
        If Not IsPostBack Then
            Me.PageTitle.Text = Me.GetGlobalResourceObject("Title", "SystemLogin")

            ' CRE19-026 (HCVS hotline service) [Start][Winnie]
            ' ------------------------------------------------------------------------
            If Not Session Is Nothing Then
                If Not RedirectHandler.IsTurnOnConcurrentBrowserHandling Then
                    ' Do not remove session, only remove session when perform login successfully
                    Session.RemoveAll()
                End If
            End If
            ' CRE19-026 (HCVS hotline service) [End][Winnie]

            Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT010001, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, "Login loaded")
            Session("language") = "en-us"
        End If

        ' CRE15-006 Rename of eHS [Start][Lawrence]
        lblAppEnvironment.Text = (New GeneralFunction).getSystemParameter("AppEnvironment")

        If lblAppEnvironment.Text.ToLower = "production" Then lblAppEnvironment.Text = String.Empty
        ' CRE15-006 Rename of eHS [End][Lawrence]

        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
        Select Case Me.SubPlatform
            Case EnumHCVUSubPlatform.CC
                Me.tblBanner.Style.Item("background-image") = "url(" + Me.GetGlobalResourceObject("ImageUrl", "IndexBannerCallCentre").ToString + ")"
            Case Else
                Me.tblBanner.Style.Item("background-image") = "url(" + Me.GetGlobalResourceObject("ImageUrl", "IndexBanner").ToString + ")"
        End Select
        ' CRE19-026 (HCVS hotline service) [End][Winnie]

        Dim strPleaseWaitScript As New StringBuilder
        strPleaseWaitScript.Append("function ModalUpdProgInitialize(sender, args) {")
        strPleaseWaitScript.Append("var upd = $find('" & Me.UpdateProgress1.ClientID & "');")
        strPleaseWaitScript.Append("upd._endRequestHandlerDelegate = Function.createDelegate(upd, ModalUpdProgEndRequest);")
        strPleaseWaitScript.Append("upd._startDelegate = Function.createDelegate(upd, ModalUpdProgStartRequest);")
        strPleaseWaitScript.Append("upd._pageRequestManager.add_endRequest(upd._endRequestHandlerDelegate);}")
        strPleaseWaitScript.Append("function ModalUpdProgStartRequest() {")
        strPleaseWaitScript.Append("document.getElementById('" + Me.pnlPleaseWait.ClientID + "').style.visibility='hidden';")
        strPleaseWaitScript.Append("if (this._pageRequestManager.get_isInAsyncPostBack()) {")
        strPleaseWaitScript.Append("$find('" & Me.ModalPopupExtender1.ClientID & "').show();")
        strPleaseWaitScript.Append("setTimeout(""document.getElementById('" + Me.pnlPleaseWait.ClientID + "').style.visibility='visible'"", 1000);}")
        strPleaseWaitScript.Append("this._timerCookie = null;}")
        strPleaseWaitScript.Append("function ModalUpdProgEndRequest(sender, arg) {")
        strPleaseWaitScript.Append("document.getElementById('" + Me.pnlPleaseWait.ClientID + "').style.visibility='hidden';")
        strPleaseWaitScript.Append(" $find('" & ModalPopupExtender1.ClientID & "').hide();")
        strPleaseWaitScript.Append("if (this._timerCookie) {")
        strPleaseWaitScript.Append("window.clearTimeout(this._timerCookie);")
        strPleaseWaitScript.Append("this._timerCookie = null;}}")
        strPleaseWaitScript.Append("Sys.Application.add_load(ModalUpdProgInitialize);")

        ScriptManager.RegisterStartupScript(Page, Me.GetType, "ModalUpdProg", strPleaseWaitScript.ToString, True)

        ' Privacy Policy
        Dim strHost As String = Request.Url.ToString.Substring(0, Request.Url.ToString.IndexOf(Request.Path))
        Dim strPrivacyPolicyLink As String = strHost & Request.ApplicationPath
        If strPrivacyPolicyLink.Substring(strPrivacyPolicyLink.Length - 1, 1) <> "/" Then
            strPrivacyPolicyLink = strPrivacyPolicyLink & "/PrivacyPolicy/PrivacyPolicy.htm"
        Else
            strPrivacyPolicyLink = strPrivacyPolicyLink & "PrivacyPolicy/PrivacyPolicy.htm"
        End If

        Me.lnkBtnPrivacyPolicy.OnClientClick = "javascript:openNewHTML('" + strPrivacyPolicyLink + "');return false;"

        ' Important Notices
        Dim strDisclaimerLink As String = strHost & Request.ApplicationPath
        If strDisclaimerLink.Substring(strDisclaimerLink.Length - 1, 1) <> "/" Then
            strDisclaimerLink = strDisclaimerLink & "/Disclaimer/Disclaimer.htm"
        Else
            strDisclaimerLink = strDisclaimerLink & "Disclaimer/Disclaimer.htm"
        End If

        Me.lnkBtnDisclaimer.OnClientClick = "javascript:openNewHTML('" + strDisclaimerLink + "');return false;"

        ' System Maintenances
        Dim strSysMaintLink As String = strHost & Request.ApplicationPath
        If strSysMaintLink.Substring(strSysMaintLink.Length - 1, 1) <> "/" Then
            strSysMaintLink = strSysMaintLink & "/SystemMaint/SysMaint.htm"
        Else
            strSysMaintLink = strSysMaintLink & "SystemMaint/SysMaint.htm"
        End If
        Me.lnkBtnSysMaint.OnClientClick = "javascript:openNewHTML('" + strSysMaintLink + "');return false;"

        Dim udtGeneralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

        ' FAQ
        Dim strFAQLink As String = String.Empty
        udtGeneralFunction.getSystemParameter(SystemParameters.FAQsLinkHCVU, strFAQLink, String.Empty)

        ibtnFAQ.OnClientClick = "javascript:openNewHTML('" + strFAQLink + "');return false;"

        ' Contact Us
        Dim strContactUsLink As String = strHost & Request.ApplicationPath
        If strContactUsLink.Substring(strContactUsLink.Length - 1, 1) <> "/" Then
            strContactUsLink = strContactUsLink & "/ContactUs/ContactUs.htm"
        Else
            strContactUsLink = strContactUsLink & "ContactUs/ContactUs.htm"
        End If

        ibtnContactUs.OnClientClick = "javascript:openNewHTML('" + strContactUsLink + "');return false;"

        ' User Manual
        Dim strUserManualLink As String = strHost & Request.ApplicationPath
        If strUserManualLink.Substring(strUserManualLink.Length - 1, 1) <> "/" Then
            strUserManualLink = strUserManualLink & "/UserManual/UserManual.pdf"
        Else
            strUserManualLink = strUserManualLink & "UserManual/UserManual.pdf"
        End If

        ibtnUserManual.OnClientClick = "window.open('" & strUserManualLink & "', '_blank', 'resizable=yes,status=yes,toolbar=no,location=no,scrollbars=yes,left=0,top=0'); return false;"

        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
        ' ------------------------------------------------------------------------
        If Me.SubPlatform = EnumHCVUSubPlatform.CC Then
            ibtnUserManual.Visible = False
            ibtnContactUs.Visible = False
            ibtnFAQ.Visible = False
        Else
            ibtnUserManual.Visible = True
            ibtnContactUs.Visible = True
            ibtnFAQ.Visible = True
        End If
        ' CRE19-026 (HCVS hotline service) [End][Winnie]
    End Sub

    Private Sub ibtnLogin_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnLogin.Click

        If Not ValidateLoginInput() Then
            ' Empty input, should be block by logic inside "LoginAction"
            LoginAction(False, False)
            Return
        End If

        If RedirectHandler.IsTurnOnConcurrentBrowserHandling AndAlso KeyGenerator.IsConcurrentAccessDetected Then

            Dim udtAuditLogEntry As New AuditLogEntry(Me.FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00023, AuditLogDesc.Msg00023)

            'Save Password and PassCode into Session for LogionAction to Access
            Me.SavePasswordAndTokenPassCodeToSession(Me.txtPassword.Text, Me.txtPinCode.Text)

            'Show popup message
            Me.ModalPopupExtenderConfirm.Show()

            'Prevent multiple login click
            Me.ibtnLogin.Enabled = False
        Else
            LoginAction(False, True)
        End If
    End Sub

    Private Sub ResetAlertImage()
        Me.imgUserNameAlert.Visible = False
        Me.imgPasswordAlert.Visible = False
        Me.imgPinCodeAlert.Visible = False
    End Sub

    Private Sub login_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not Me.udcMessageBox.Visible Then
            ScriptManager2.SetFocus(Me.txtUsername)
        End If

        Dim udtNewsMessageBLL As New NewsMessageBLL
        Dim udtNewsMessageCollection As NewsMessageModelCollection = udtNewsMessageBLL.GetNewsMessageModelCollectionFromXML()
        If udtNewsMessageCollection.Count > 0 Then
            Me.pnlNewsMessage.Visible = True
            Me.dlNewsMessage.DataSource = udtNewsMessageCollection
            Me.dlNewsMessage.DataBind()
        Else
            Me.pnlNewsMessage.Visible = False
        End If

    End Sub

    Private Sub dlNewsMessage_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlNewsMessage.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim udtNewsMessage As NewsMessageModel = CType(e.Item.DataItem, NewsMessageModel)
            Dim lblCreateDate As Label = CType(e.Item.FindControl("lblCreateDate"), Label)
            Dim lblDescription As Label = CType(e.Item.FindControl("lblDescription"), Label)

            Dim udtFormatter As New Formatter

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'lblCreateDate.Text = udtFormatter.formatDate(udtNewsMessage.CreateDtm)
            lblCreateDate.Text = udtFormatter.formatDisplayDate(udtNewsMessage.CreateDtm)
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            If Thread.CurrentThread.CurrentUICulture.Name.ToUpper = "ZH-TW" Then
                lblDescription.Text = udtNewsMessage.ChiDescription
            Else
                lblDescription.Text = udtNewsMessage.Description
            End If
        End If
    End Sub


#Region "Implement IWorkingData (CRE11-004)"

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
    ''' Retrieve EHS Service Provider which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Transaction which user working on
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

#Region "Login"
    Private Sub LoginAction(ByVal IsCalledFromConcurrentAccessPopup As Boolean, ByVal blnClearAllSession As Boolean)

        Dim strPassword As String
        Dim strPassCode As String

        strPassword = String.Empty
        strPassCode = String.Empty

        If IsCalledFromConcurrentAccessPopup Then
            If Not Session(SESS_Password) Is Nothing Then
                strPassword = Session(SESS_Password).ToString
            End If

            If Not Session(SESS_Passcode) Is Nothing Then
                strPassCode = Session(SESS_Passcode).ToString
            End If
            SavePasswordAndTokenPassCodeToSession("", "")
        Else
            strPassword = Me.txtPassword.Text
            strPassCode = Me.txtPinCode.Text
        End If

        ' Login Checking
        Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT010001, Me)

        udtAuditLogEntry.AddDescripton("Login ID", Me.txtUsername.Text)
        udtAuditLogEntry.AddDescripton("Token PIN", strPassCode)

        Dim blnNoUnsuccessLog As Boolean = False
        Dim strLogUserID As String = ""
        Dim udtValidator As New Validator
        Dim udtHCVUUserBLL As New HCVUUserBLL
        Dim dtHCVUUser As DataTable = Nothing

        Dim blnLoginFail As Boolean = False
        Dim blnPassLogin As Boolean = True

        Dim strEnableToken As String = ""
        Dim udtLoginBLL As New LoginBLL

        ResetAlertImage()
        If udtValidator.IsEmpty(Me.txtUsername.Text.Trim) Then
            'Please input Login ID
            udcMessageBox.AddMessage("990000", "E", "00042")
            Me.imgUserNameAlert.Visible = True
        Else
            dtHCVUUser = udtHCVUUserBLL.GetHCVUUserForLogin(Me.txtUsername.Text)
        End If

        'INT11-0011
        'udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Login", strLogUserID)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Login", Me.txtUsername.Text)
        'End INT11-0011

        If Not dtHCVUUser Is Nothing AndAlso dtHCVUUser.Rows.Count = 1 Then
            strLogUserID = CStr(dtHCVUUser.Rows(0).Item("User_ID")).Trim
        Else
            strLogUserID = Me.txtUsername.Text

            udtAuditLogEntry.AddDescripton("Login ID", Me.txtUsername.Text)
            udtAuditLogEntry.AddDescripton("Token PIN", strPassCode)
            udtAuditLogEntry.WriteLog(LogID.LOG00015, "Login fail: Incorrect UserID[" & Me.txtUsername.Text.Trim & "]", Me.txtUsername.Text.Trim)
        End If

        If udtValidator.IsEmpty(strPassword) Then
            udcMessageBox.AddMessage("990000", "E", "00043")
            Me.imgPasswordAlert.Visible = True
        End If

        If udtValidator.IsEmpty(strPassCode) Then
            udcMessageBox.AddMessage("990000", "E", "00044")
            Me.imgPinCodeAlert.Visible = True
        End If

        If Me.udcMessageBox.GetCodeTable.Rows.Count = 0 Then
            If Not dtHCVUUser Is Nothing AndAlso dtHCVUUser.Rows.Count = 1 AndAlso blnPassLogin Then
                If CStr(dtHCVUUser.Rows(0).Item("User_ID")).Trim.ToUpper() = Me.txtUsername.Text.Trim.ToUpper() Then

                    ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
                    Dim udtVerifyPassword As VerifyPasswordResultModel = VerifyPassword(EnumPlatformType.VU, dtHCVUUser, strPassword)


                    If udtVerifyPassword.VerifyResult = EnumVerifyPasswordResult.RequireUpdate Then

                        Dim strUserID As String = dtHCVUUser.Rows(0).Item("User_ID")

                        If dtHCVUUser.Rows(0).Item("Suspended") Is DBNull.Value Then
                            ' check if there is entry in Token table
                            If dtHCVUUser.Rows(0).Item("Token_Cnt") > 0 Then
                                ' check token with RSA server
                                Dim udtTokenBLL As New Token.TokenBLL
                                If udtTokenBLL.AuthenTokenHCVU(strUserID, strPassCode) = False Then
                                    blnPassLogin = False
                                    udtAuditLogEntry.WriteLog(LogID.LOG00017, "Login fail: Incorrect Token Passcode[" & strPassCode.Trim & "]", strUserID)
                                    udtAuditLogEntry.AddDescripton("StackTrace", "Incorrect token passcode")
                                Else
                                    blnNoUnsuccessLog = True
                                    blnLoginFail = True
                                    Me.udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00407)
                                    udtAuditLogEntry.AddDescripton("StackTrace", "Hash password expired, password level lower than system minimum password level")
                                End If
                            Else
                                blnPassLogin = False
                                udtAuditLogEntry.AddDescripton("StackTrace", "No token found")
                            End If
                        End If
                    Else
                        ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
                        If udtVerifyPassword.VerifyResult = EnumVerifyPasswordResult.Pass Then
                            If udtVerifyPassword.TransitPassword Then
                                dtHCVUUser = udtHCVUUserBLL.GetHCVUUserForLogin(Me.txtUsername.Text)
                            End If
                            ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---

                            Dim strUserID As String = dtHCVUUser.Rows(0).Item("User_ID")

                            ' CRE19-026 (HCVS hotline service) [Start][Winnie]
                            ' ------------------------------------------------------------------------
                            ' Check whether User is available to login with SubPlatform
                            Dim blnValidUser As Boolean = False
                            Dim udtRoleTypeBLL As New RoleTypeBLL
                            Dim udtUserRoleBLL As New UserRoleBLL

                            Dim dtRoleType As DataTable
                            dtRoleType = udtRoleTypeBLL.GetRoleTypeTable()

                            For Each udtUserRole As UserRoleModel In udtUserRoleBLL.GetUserRoleCollection(strUserID).Values

                                Dim drRoleType As DataRow() = dtRoleType.Select("(Available_HCVU_SubPlatform = 'ALL' OR Available_HCVU_SubPlatform = '" + Me.SubPlatform.ToString + "')" _
                                                                                 + "AND Role_Type = " + udtUserRole.RoleType.ToString)

                                If drRoleType.Length > 0 Then
                                    blnValidUser = True
                                    Exit For
                                End If
                            Next

                            If Not blnValidUser Then
                                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("The user is not available to login with SubPlatform {0}.", Me.SubPlatform.ToString))
                                blnPassLogin = False
                            End If
                            ' CRE19-026 (HCVS hotline service) [End][Winnie]

                            ' CRE13-029 - RSA server upgrade [Start][Lawrence]
                            If blnPassLogin Then
                                If dtHCVUUser.Rows(0).Item("Suspended") Is DBNull.Value Then
                                    ' check if there is entry in Token table
                                    If dtHCVUUser.Rows(0).Item("Token_Cnt") > 0 Then
                                        ' check token with RSA server
                                        Dim udtTokenBLL As New Token.TokenBLL
                                        If udtTokenBLL.AuthenTokenHCVU(strUserID, strPassCode) = False Then
                                            blnPassLogin = False
                                            udtAuditLogEntry.WriteLog(LogID.LOG00017, "Login fail: Incorrect Token Passcode[" & strPassCode.Trim & "]", strUserID)
                                            udtAuditLogEntry.AddDescripton("StackTrace", "Incorrect token passcode")
                                        End If
                                    Else
                                        blnPassLogin = False
                                        udtAuditLogEntry.AddDescripton("StackTrace", "No token found")
                                    End If
                                End If
                            End If
                            ' CRE13-029 - RSA server upgrade [End][Lawrence]
                        Else
                            blnPassLogin = False
                            udtAuditLogEntry.AddDescripton("Login ID", Me.txtUsername.Text)
                            udtAuditLogEntry.AddDescripton("Token PIN", strPassCode)
                            udtAuditLogEntry.WriteLog(LogID.LOG00016, "Login fail: Incorrect Password", Me.txtUsername.Text)
                            udtAuditLogEntry.AddDescripton("StackTrace", "Incorrect password")
                        End If
                    End If
                    ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]

                Else
                    ' Actually this would not be happending
                    udtAuditLogEntry.AddDescripton("StackTrace", "User ID not match")
                    blnPassLogin = False
                End If
            Else
                blnPassLogin = False
                udtAuditLogEntry.AddDescripton("StackTrace", "User ID not found")
            End If
        End If

        Dim intChgPwdDay As Integer
        Dim strChgPwdDay As String = ""

        ' get the days of need to change password
        udtGeneralFunction.getSystemParameter(SystemParameters.DaysOfChangePassword, strChgPwdDay, String.Empty)
        intChgPwdDay = CInt(strChgPwdDay)

        If blnPassLogin AndAlso udcMessageBox.GetCodeTable.Rows.Count = 0 Then

            If Not dtHCVUUser Is Nothing AndAlso dtHCVUUser.Rows.Count = 1 Then

                ' CRE19-026 (HCVS hotline service) [Start][Winnie]
                ' ------------------------------------------------------------------------
                ' Remove all Session while press login
                If blnClearAllSession Then
                    If Not Session Is Nothing Then
                        Session.RemoveAll()
                    End If
                End If

                If RedirectHandler.IsTurnOnConcurrentBrowserHandling Then
                    KeyGenerator.RenewSessionPageKey()
                End If
                ' CRE19-026 (HCVS hotline service) [End][Winnie]

                If Not dtHCVUUser.Rows(0).Item("Force_Pwd_Change") Is DBNull.Value AndAlso dtHCVUUser.Rows(0).Item("Force_Pwd_Change") = "Y" Then
                    ' force to change password due to just reset password
                    Session("FirstChangePassword") = "R"
                    udtAuditLogEntry.AddDescripton("Login ID", Me.txtUsername.Text)
                    udtAuditLogEntry.AddDescripton("Token PIN", strPassCode)
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00021, "Login successful (Reset Password Force change password)", strLogUserID)
                    Session(SESS_ChangePasswordHCVUUser) = udtLoginBLL.LoginUserAC(Me.txtUsername.Text, dtHCVUUser, Me.SubPlatform)
                    Session.Remove(HCVUUserBLL.SESS_HCVUUSER)
                    ' CRE19-026 (HCVS hotline service) [Start][Winnie]                                                                        
                    RedirectHandler.ToURL(PageURL.ChangePassword)
                    ' CRE19-026 (HCVS hotline service) [End][Winnie]

                ElseIf dtHCVUUser.Rows(0).Item("Suspended") Is DBNull.Value AndAlso dtHCVUUser.Rows(0).Item("Account_Locked") = "N" Then
                    'If CStr(dtHCVUUser.Rows(0).Item("User_ID")).Trim.ToUpper() = Me.txtUsername.Text.Trim.ToUpper() AndAlso CStr(dtHCVUUser.Rows(0).Item("User_Password")) = Encrypt.MD5hash(Me.txtPassword.Text) Then
                    ' account not suspended
                    Dim udtHCVUUser As HCVUUserModel

                    ' get the HCVUUser object
                    udtHCVUUser = udtLoginBLL.LoginUserAC(Me.txtUsername.Text, dtHCVUUser, Me.SubPlatform)
                    If Not udtHCVUUser Is Nothing Then
                        udtHCVUUserBLL.SaveToSession(udtHCVUUser)
                        If udtHCVUUser.LastLoginDtm.HasValue Then
                            ' check if user changed password within the limited duration
                            If udtHCVUUser.LastPwdChangeDuration.HasValue AndAlso CInt(udtHCVUUser.LastPwdChangeDuration) < intChgPwdDay Then
                                Try
                                    ' log login success datetime
                                    udtLoginBLL.UpdateSuccessLoginDtm(CStr(dtHCVUUser.Rows(0).Item("User_ID")))
                                Catch eSQL As SqlClient.SqlException
                                    If eSQL.Number = 50000 Then
                                        Dim strmsg As String
                                        strmsg = eSQL.Message
                                        Dim udtSytemMessage As Common.ComObject.SystemMessage
                                        udtSytemMessage = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                                        Me.udcMessageBox.AddMessage(udtSytemMessage)
                                        blnLoginFail = True
                                    Else
                                        Throw eSQL
                                    End If
                                Catch ex As Exception
                                    Throw ex
                                End Try

                                Session("language") = "en-us"

                                If udcMessageBox.GetCodeTable.Rows.Count = 0 Then
                                    udtAuditLogEntry.AddDescripton("Login ID", Me.txtUsername.Text)
                                    udtAuditLogEntry.AddDescripton("Token PIN", strPassCode)
                                    udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Login successful", strLogUserID)

                                    ' CRE19-026 (HCVS hotline service) [Start][Winnie]
                                    Dim udtMenuBLL As New Component.Menu.MenuBLL
                                    Dim strEnquiryCallCentre_FuncCode As String = FunctCode.FUNT010309

                                    ' eHealth Account Enquiry (Call Centre) as default page
                                    If Me.SubPlatform = EnumHCVUSubPlatform.CC AndAlso
                                        udtHCVUUser.AccessRightCollection.Item(strEnquiryCallCentre_FuncCode).Allow() Then
                                        RedirectHandler.ToURL(udtMenuBLL.GetURLByFunctionCode(strEnquiryCallCentre_FuncCode))

                                    Else
                                        RedirectHandler.ToURL(PageURL.Home)
                                    End If
                                    ' CRE19-026 (HCVS hotline service) [End][Winnie]
                                End If
                            Else

                                Session("language") = "en-us"

                                ' force to change password
                                Session("FirstChangePassword") = "N"
                                udtAuditLogEntry.AddDescripton("Login ID", Me.txtUsername.Text)
                                udtAuditLogEntry.AddDescripton("Token PIN", strPassCode)
                                udtAuditLogEntry.WriteEndLog(LogID.LOG00004, "Login successful (Force change password)", strLogUserID)
                                Session(SESS_ChangePasswordHCVUUser) = udtLoginBLL.LoginUserAC(Me.txtUsername.Text, dtHCVUUser, Me.SubPlatform)
                                Session.Remove(HCVUUserBLL.SESS_HCVUUSER)

                                ' CRE19-026 (HCVS hotline service) [Start][Winnie]                                                                        
                                RedirectHandler.ToURL(PageURL.ChangePassword)
                                ' CRE19-026 (HCVS hotline service) [End][Winnie]
                            End If
                        Else

                            Session("language") = "en-us"

                            ' first login change password
                            Session("FirstChangePassword") = "Y"
                            udtAuditLogEntry.AddDescripton("Login ID", Me.txtUsername.Text)
                            udtAuditLogEntry.AddDescripton("Token PIN", strPassCode)
                            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Login successful (First logon change password)", strLogUserID)
                            Session.Remove(HCVUUserBLL.SESS_HCVUUSER)
                            Session(SESS_ChangePasswordHCVUUser) = udtLoginBLL.LoginUserAC(Me.txtUsername.Text, dtHCVUUser, Me.SubPlatform)
                            ' CRE19-026 (HCVS hotline service) [Start][Winnie]                                                                        
                            RedirectHandler.ToURL(PageURL.ChangePassword)
                            ' CRE19-026 (HCVS hotline service) [End][Winnie]
                        End If
                    Else
                        ' cannot get the HCVUUser object
                        blnPassLogin = False
                    End If
                Else
                    ' Account suspended
                    blnNoUnsuccessLog = True
                    blnLoginFail = True
                    If Not dtHCVUUser.Rows(0).Item("Suspended") Is DBNull.Value Then
                        Me.udcMessageBox.AddMessage("990000", "E", "00060")
                    Else
                        Me.udcMessageBox.AddMessage("990000", "E", "00071")
                    End If
                End If
                'Else
                '    blnPassLogin = False
                'End If
            Else
                ' not row in dtHCVUUser
                blnPassLogin = False
            End If
        End If

        If blnPassLogin = False Then
            ' Incorrect "Login ID", "Password" or "Token Passcode".
            Me.udcMessageBox.AddMessage(FunctCode.FUNT010001, SeverityCode.SEVE, MsgCode.MSG00001)
        End If

        If udcMessageBox.GetCodeTable.Rows.Count > 0 AndAlso blnNoUnsuccessLog = False Then
            If Not dtHCVUUser Is Nothing AndAlso dtHCVUUser.Rows.Count = 1 Then
                Try
                    ' log failed login datetime
                    udtLoginBLL.UpdateUnsuccessLoginDtm(CStr(dtHCVUUser.Rows(0).Item("User_ID")))
                Catch eSQL As SqlClient.SqlException
                    If eSQL.Number = 50000 Then

                    Else
                        Throw eSQL
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
            End If
        End If

        If blnLoginFail Then
            udtAuditLogEntry.AddDescripton("Login ID", Me.txtUsername.Text)
            udtAuditLogEntry.AddDescripton("Token PIN", strPassCode)
            udcMessageBox.BuildMessageBox("LoginFail", udtAuditLogEntry, "Login fail", LogID.LOG00005, strLogUserID)
        Else
            udtAuditLogEntry.AddDescripton("Login ID", Me.txtUsername.Text)
            udtAuditLogEntry.AddDescripton("Token PIN", strPassCode)
            udcMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, "Login fail", LogID.LOG00005, strLogUserID)
        End If

    End Sub

    Protected Sub ibtnLoginProceed_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim udtAuditLogEntry As New AuditLogEntry(Me.FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00024, AuditLogDesc.Msg00024)

        Me.ibtnLogin.Enabled = True
        LoginAction(True, True)

    End Sub

    Protected Sub ibtnLoginCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim udtAuditLogEntry As New AuditLogEntry(Me.FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00025, AuditLogDesc.Msg00025)

        Me.ibtnLogin.Enabled = True
        Me.SavePasswordAndTokenPassCodeToSession("", "")

    End Sub

    Private Sub SavePasswordAndTokenPassCodeToSession(ByVal strPassword As String, ByVal strPassCode As String)

        Session(SESS_Password) = strPassword
        Session(SESS_Passcode) = strPassCode

    End Sub

    Private Function ValidateLoginInput() As Boolean
        If txtUsername.Text.Trim <> String.Empty AndAlso txtPassword.Text.Trim <> String.Empty AndAlso txtPinCode.Text <> String.Empty Then
            Return True
        Else
            Return False
        End If
    End Function
#End Region

End Class