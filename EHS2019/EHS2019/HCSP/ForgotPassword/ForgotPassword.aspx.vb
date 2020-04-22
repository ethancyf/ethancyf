Imports Common.ComFunction
Imports Common.Encryption
Imports Common.Component
Imports Common.ComObject
Imports Common.ComFunction.AccountSecurity

Partial Public Class ForgotPassword
    Inherits BasePage

    Dim udcGeneralFun As New GeneralFunction
    Dim udcValidator As New Common.Validation.Validator
    Dim udcForgotPasswordBll As New BLL.ForgotPasswordBLL
    Dim strChangePasswordCode As String = "strChangePasswordCode"
    Const FUNCTION_CODE As String = "020002"
    Const IsPageSessionAlive As String = "IsForgotPwdPageSessionAlive"
    Const EnableIVSS As String = "IsServiceProviderIncludeIVSSenabledScheme"

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim strSelectedLang As String
        strSelectedLang = IIf(IsNothing(Request.QueryString("lang")), "", Request.QueryString("lang")).ToString().Trim
        If strSelectedLang = "ZH" Then
            Session("language") = TradChinese
        ElseIf strSelectedLang = "EN" Then
            Session("language") = English
        End If
        strSelectedLang = LCase(Session("language"))
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Buffer = True
        Response.ExpiresAbsolute = Now().Subtract(New TimeSpan(1, 0, 0, 0))
        Response.Expires = 0
        Response.CacheControl = "no-cache"

        Page.Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If Not IsPostBack Then
            Session(IsPageSessionAlive) = "Y"

            Dim udtAuditLogEntry0 As New AuditLogEntry(FUNCTION_CODE, Me)
            udtAuditLogEntry0.WriteLog(LogID.LOG00000, "Forgot Password")

            ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
            If Me.SubPlatform = EnumHCSPSubPlatform.CN Then
                udtAuditLogEntry0.WriteLog(LogID.LOG00022, "Forgot Password not available in CN Sub Platform")
                Response.Redirect(ClaimVoucherMaster.FullVersionPage.Login)
                Return
            End If
            ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

            Session(strChangePasswordCode) = IIf(IsNothing(Request.QueryString("code")), "", Request.QueryString("code"))

            'add audit log
            Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
            udtAuditLogEntry.AddDescripton("Activation Code", Session(strChangePasswordCode))
            udtAuditLogEntry.WriteStartLog(LogID.LOG00004, "Link Validate")

            Me.panStep1.Visible = False
            Me.panStep2.Visible = False
            Me.panStep3.Visible = False

            If Not udcValidator.IsEmpty(Session(strChangePasswordCode)) Then
                Dim dt As New DataTable

                ' I-CRE16-007-02 (Refine system from CheckMarx findings) [Start][Winnie]
                dt = udcForgotPasswordBll.CheckEmailLinkByCode(Hash(Session(strChangePasswordCode)).HashedValue)
                ' I-CRE16-007-02 (Refine system from CheckMarx findings) [End][Winnie]

                If CInt(dt.Rows(0)(0)) = 0 Then
                    Me.udcErrorMessage.AddMessage("990000", "E", "00065")
                    Me.MultiView1.ActiveViewIndex = 4
                    udtAuditLogEntry.AddDescripton("Activation Code", Session(strChangePasswordCode))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00006, "Link Validate fail")
                Else
                    udtAuditLogEntry.AddDescripton("Activation Code", Session(strChangePasswordCode))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00005, "Link Validate successful")
                End If
            Else
                Me.udcErrorMessage.AddMessage("990000", "E", "00065")
                Me.MultiView1.ActiveViewIndex = 4
                udtAuditLogEntry.WriteEndLog(LogID.LOG00006, "Link Validate fail")
            End If

            Me.udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00018, "Show Link Validate Fail Messagebox")
        End If

        SetLanguage()
        Dim strSelectedLang As String
        strSelectedLang = IIf(IsNothing(Request.QueryString("lang")), "", Request.QueryString("lang")).ToString().Trim
        If Not strSelectedLang.Trim.Equals(String.Empty) Then
            Response.Redirect(HttpContext.Current.Request.Path & "?code=" & Session(strChangePasswordCode))
        End If
        strSelectedLang = LCase(Session("language"))

        If Session(EnableIVSS) Then
            Me.lblClaimVoucherStep2.Text = Me.GetGlobalResourceObject("Text", "ForgotPasswordTimeLine2")
            Me.lblResetPasswordTitle.Text = Me.GetGlobalResourceObject("Text", "ResetPasswordStep2TitleText")
            If Me.txtIVRSEnabled.Text.Trim.Equals(String.Empty) Then
                Me.txt_accAct_newIVRPW.Enabled = False
                Me.txt_accAct_confirmIVRPW.Enabled = False
                Me.txt_accAct_newIVRPW.ReadOnly = True
                Me.txt_accAct_confirmIVRPW.ReadOnly = True
                Me.lblNEWIVRSPwText.Enabled = False
                Me.lblConfirmIVRSPWText.Enabled = False
                'Me.txt_accAct_newIVRPW.BackColor = Drawing.Color.Gray
                'Me.txt_accAct_confirmIVRPW.BackColor = Drawing.Color.Gray
            Else
                Me.txt_accAct_newIVRPW.Enabled = True
                Me.txt_accAct_confirmIVRPW.Enabled = True
                Me.txt_accAct_newIVRPW.ReadOnly = False
                Me.txt_accAct_confirmIVRPW.ReadOnly = False
                Me.lblNEWIVRSPwText.Enabled = True
                Me.lblConfirmIVRSPWText.Enabled = True
                'Me.txt_accAct_newIVRPW.BackColor = Drawing.Color.White
                'Me.txt_accAct_confirmIVRPW.BackColor = Drawing.Color.White
            End If
        Else
            Me.lblClaimVoucherStep2.Text = Me.GetGlobalResourceObject("Text", "ForgotPasswordTimeLine4")
            Me.lblResetPasswordTitle.Text = Me.GetGlobalResourceObject("Text", "ResetPasswordStep2TitleText2")
            Me.panIVRSPassword.Visible = False
            Me.txtIVRSEnabled.Text = ""
            Me.txt_accAct_newIVRPW.Enabled = False
            Me.txt_accAct_confirmIVRPW.Enabled = False
            Me.txt_accAct_newIVRPW.ReadOnly = False
            Me.txt_accAct_confirmIVRPW.ReadOnly = False
            Me.lblNEWIVRSPwText.Enabled = False
            Me.lblConfirmIVRSPWText.Enabled = False
        End If


        Me.IsSessionExpired()

    End Sub

    Private Sub ChangeTimeLine(ByVal strStep As String)
        Dim strhightlight, strunhightlight As String

        strhightlight = "highlightTimeline"
        strunhightlight = "unhighlightTimeline"

        Me.panStep1.CssClass = strunhightlight
        Me.panStep2.CssClass = strunhightlight
        Me.panStep3.CssClass = strunhightlight

        Select Case strStep
            Case "2"
                Me.panStep2.CssClass = strhightlight
            Case "3"
                Me.panStep3.CssClass = strhightlight
            Case Else
                Me.panStep1.CssClass = strhightlight
        End Select
    End Sub

    Private Sub SetLanguage()
        Dim selectedValue As String

        selectedValue = Session("language")


        Dim strvalue As String = String.Empty
        Dim strvalue2 As String = String.Empty

        udcGeneralFun.getSystemParameter("PasswordRuleNumber", strvalue, strvalue2)

        Me.txt_accAct_newPW.Attributes.Add("onKeyUp", "checkPassword(this.value,'" & CInt(strvalue.Trim) & "', '" & CInt(strvalue2.Trim) & "', 'strength1','strength2','strength3','progressBar', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthPoor") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthModerate") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthStrong") & "','direction2','direction1');")

        Dim strHCSPLink As String = String.Empty
        udcGeneralFun.getSystemParameter("HCSPAppPath", strHCSPLink, String.Empty)
    End Sub

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

        'Session("language") = 0
        MyBase.InitializeCulture()

    End Sub
    Private Function IsSpIdTokenPinMatch(ByVal strSPID As String, ByVal strTokenPin As String) As Boolean

        Try
            If udcForgotPasswordBll.IsSpIdTokenPinMatch(strSPID, strTokenPin) Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function IsSpIdQualified(ByVal strSPID As String) As Boolean
        Dim dt As New DataTable

        Try
            ' I-CRE16-007-02 (Refine system from CheckMarx findings) [Start][Winnie]
            dt = udcForgotPasswordBll.GetInfoBySPIDActivationCode(strSPID, Hash(Session(strChangePasswordCode)).HashedValue)
            ' I-CRE16-007-02 (Refine system from CheckMarx findings) [End][Winnie]

            If dt.Rows.Count <= 0 Then
                Session("SP_TSMP") = Nothing
                Return False
            Else
                If udcForgotPasswordBll.ServiceProviderIsActiveBySPID(strSPID) Then
                    Session("SP_TSMP") = dt.Rows(0)("TSMP")
                    Return True
                Else
                    Session("SP_TSMP") = Nothing
                    Return False
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub GetLatestTSMPtoSession(ByVal strSPID As String)
        Dim dt As New DataTable
        Dim udtUserACBLL As New Common.Component.UserAC.UserACBLL
        Try
            ' CRE16-026 (Change email for locked SP) [Start][Winnie]
            dt = udtUserACBLL.GetTSMPBySPID(strSPID)
            ' CRE16-026 (Change email for locked SP) [End][Winnie]

            If dt.Rows.Count <= 0 Then
                Session("SP_TSMP") = Nothing
            Else
                Session("SP_TSMP") = dt.Rows(0)("TSMP")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function IsAccountAliasDuplicated(ByVal strAlias As String) As Boolean

        Try
            If udcForgotPasswordBll.IsAccountAliasDuplicated(strAlias) Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function IsPasswordFormatOK(ByVal strPW As String) As Boolean
        Return udcValidator.ValidatePassword(strPW)
    End Function

    Private Function IsIVRPasswordFormatOK(ByVal strPW As String) As Boolean
        Return udcValidator.ValidateIVRSPassword(strPW)
    End Function

    Private Function IsUserNameFormatOK(ByVal strUsername As String) As Boolean
        Return udcValidator.ValidateAlias(strUsername)
    End Function

    Private Function IsConfirmPasswordMatch(ByVal strNewPW As String, ByVal strConfirmPW As String) As Boolean
        Return udcValidator.ChkIsIdenticial(strNewPW, strConfirmPW)
    End Function

    Private Sub ResetWebPassword(ByVal strNewPassword As String)

        Try
            ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
            'udcForgotPasswordBll.ResetWebPassword(Me.txt_accAct_SPID.Text, Encrypt.MD5hash(strNewPassword), Session("SP_TSMP"))
            udcForgotPasswordBll.ResetWebPassword(Me.txt_accAct_SPID.Text, Hash(strNewPassword), Session("SP_TSMP"))
            ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---
        Catch eSql As SqlClient.SqlException
            Throw eSql
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub ResetIVRSPassword(ByVal strNewIVRSPassword As String)

        Try
            udcForgotPasswordBll.ResetIVRSPassword(Me.txt_accAct_SPID.Text, Hash(strNewIVRSPassword), Session("SP_TSMP"))
        Catch eSql As SqlClient.SqlException
            Throw eSql
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub ResetBothPassword(ByVal strNewPassword As String, ByVal strNewIVRSPassword As String)

        Try
            udcForgotPasswordBll.ResetBothPassword(Me.txt_accAct_SPID.Text, Hash(strNewPassword), Hash(strNewIVRSPassword), Session("SP_TSMP"))
        Catch eSql As SqlClient.SqlException
            Throw eSql
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub btn_accAct_step1_next_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btn_accAct_step1_next.Click
        Dim err As Boolean = False
        Dim bIsDelisted As Boolean = False
        Dim bIsSuspended As Boolean = False
        Dim bIsLocked As Boolean = False
        Dim bIsActivated As Boolean = False

        Dim udcLoginBll As New BLL.LoginBLL
        Dim udtUserAC As Common.Component.UserAC.UserACModel = Nothing
        Dim udtUserACBLL As New Common.Component.UserAC.UserACBLL
        Dim dtUserAC As New DataTable

        Me.img_err_spid.Visible = False
        Me.img_err_tokenPIN.Visible = False

        'add audit log
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.AddDescripton("Activation Code", Session(strChangePasswordCode))
        udtAuditLogEntry.AddDescripton("SPID", Me.txt_accAct_SPID.Text)
        udtAuditLogEntry.AddDescripton("Token PIN", Me.txt_accAct_tokenPIN.Text)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00007, "Validate Account Info", Me.txt_accAct_SPID.Text, Nothing)

        If udcValidator.IsEmpty(Me.txt_accAct_SPID.Text) Then
            err = True
            Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00001")
            Me.img_err_spid.Visible = True
        Else
            If Not IsSpIdQualified(Me.txt_accAct_SPID.Text) Then
                err = True
                'Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00012")
                Me.udcErrorMessage.AddMessage("990000", "E", "00066")
                Me.img_err_spid.Visible = True
                Me.img_err_tokenPIN.Visible = True
                'udcLoginBll.UpdateLoginDtmInNonLoginPage(Me.txt_accAct_SPID.Text, LoginStatus.Fail)

                udtAuditLogEntry.AddDescripton("SPID", Me.txt_accAct_SPID.Text.Trim)
                udtAuditLogEntry.AddDescripton("ActivationCode", Session("strActivationCode"))
                udtAuditLogEntry.WriteLog(LogID.LOG00016, "Change Password Validate fail: SPID not match with Activation Code", Me.txt_accAct_SPID.Text.Trim, Nothing)
            End If
        End If

        If udcValidator.IsEmpty(Me.txt_accAct_tokenPIN.Text) Then
            err = True
            Me.udcErrorMessage.AddMessage("990000", "E", "00044")
            Me.img_err_tokenPIN.Visible = True
        End If

        If (err = False) AndAlso Not IsSpIdTokenPinMatch(Me.txt_accAct_SPID.Text, Me.txt_accAct_tokenPIN.Text) Then
            err = True
            Me.udcErrorMessage.AddMessage("990000", "E", "00066")
            Me.img_err_spid.Visible = True
            Me.img_err_tokenPIN.Visible = True
            'udcLoginBll.UpdateLoginDtmInNonLoginPage(Me.txt_accAct_SPID.Text, LoginStatus.Fail)

            udtAuditLogEntry.AddDescripton("Token Passcode", Me.txt_accAct_tokenPIN.Text.Trim)
            udtAuditLogEntry.WriteLog(LogID.LOG00017, "Change Password Validate fail: Incorrect Token Passcode", Me.txt_accAct_SPID.Text.Trim, Nothing)
        End If

        If Not err Then
            ChangeTimeLine("2")

            'Check the SP has activated the IVRS or not
            Dim udtSPProfileBLL As New BLL.SPProfileBLL()
            Dim dt As New DataTable
            'dt = udtSPProfileBLL.loadSPLoginProfile(Me.txt_accAct_SPID.Text.Trim, "EHCVS")
            dt = udtSPProfileBLL.loadSPLoginProfile(Me.txt_accAct_SPID.Text.Trim, TokenProjectType.EHCVS)

            Dim udtSchemeClaimBLL As New Scheme.SchemeClaimBLL()
            Session(EnableIVSS) = udtSchemeClaimBLL.CheckSchemeClaimIVRSEnabled(Me.txt_accAct_SPID.Text)
            'Session(EnableIVSS) = udcForgotPasswordBll.IncludeIVRSEnabledScheme(Me.txt_accAct_SPID.Text)

            Me.panStep2.Visible = True
            Me.panStep3.Visible = True

            If Session(EnableIVSS) Then
                Me.lblClaimVoucherStep2.Text = Me.GetGlobalResourceObject("Text", "ForgotPasswordTimeLine2")
                Me.lblResetPasswordTitle.Text = Me.GetGlobalResourceObject("Text", "ResetPasswordStep2TitleText")
                Me.panIVRSPassword.Visible = True
                If dt.Rows(0)("SP_IVRS_Password").ToString.Trim.Equals(String.Empty) Then
                    Me.txtIVRSEnabled.Text = ""
                    Me.txt_accAct_newIVRPW.Enabled = False
                    Me.txt_accAct_confirmIVRPW.Enabled = False
                    Me.txt_accAct_newIVRPW.ReadOnly = True
                    Me.txt_accAct_confirmIVRPW.ReadOnly = True
                    Me.lblNEWIVRSPwText.Enabled = False
                    Me.lblConfirmIVRSPWText.Enabled = False
                    'Me.txt_accAct_newIVRPW.BackColor = Drawing.Color.Gray
                    'Me.txt_accAct_confirmIVRPW.BackColor = Drawing.Color.Gray
                Else
                    Me.txtIVRSEnabled.Text = "Y"
                    Me.txt_accAct_newIVRPW.Enabled = True
                    Me.txt_accAct_confirmIVRPW.Enabled = True
                    Me.txt_accAct_newIVRPW.ReadOnly = False
                    Me.txt_accAct_confirmIVRPW.ReadOnly = False
                    Me.lblNEWIVRSPwText.Enabled = True
                    Me.lblConfirmIVRSPWText.Enabled = True
                    'Me.txt_accAct_newIVRPW.BackColor = Drawing.Color.White
                    'Me.txt_accAct_confirmIVRPW.BackColor = Drawing.Color.White
                End If
            Else
                Me.lblClaimVoucherStep2.Text = Me.GetGlobalResourceObject("Text", "ForgotPasswordTimeLine4")
                Me.lblResetPasswordTitle.Text = Me.GetGlobalResourceObject("Text", "ResetPasswordStep2TitleText2")
                Me.panIVRSPassword.Visible = False
                Me.txtIVRSEnabled.Text = ""
                Me.txt_accAct_newIVRPW.Enabled = False
                Me.txt_accAct_confirmIVRPW.Enabled = False
                Me.txt_accAct_newIVRPW.ReadOnly = False
                Me.txt_accAct_confirmIVRPW.ReadOnly = False
                Me.lblNEWIVRSPwText.Enabled = False
                Me.lblConfirmIVRSPWText.Enabled = False
            End If

            Me.MultiView1.ActiveViewIndex = 1
            udtAuditLogEntry.AddDescripton("Activation Code", Session(strChangePasswordCode))
            udtAuditLogEntry.AddDescripton("SPID", Me.txt_accAct_SPID.Text)
            udtAuditLogEntry.AddDescripton("Token PIN", Me.txt_accAct_tokenPIN.Text)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00008, "Validate Account Info successful", Me.txt_accAct_SPID.Text, Nothing)
            udcLoginBll.UpdateLoginDtmInNonLoginPage(Me.txt_accAct_SPID.Text, LoginStatus.Success)

            'Obtain the latest Timestamp
            GetLatestTSMPtoSession(Me.txt_accAct_SPID.Text)
        Else
            dtUserAC = udtUserACBLL.GetHCSPUserACStatus(Me.txt_accAct_SPID.Text)

            If dtUserAC.Rows.Count = 1 Then

                udcLoginBll.UpdateLoginDtmInNonLoginPage(Me.txt_accAct_SPID.Text, LoginStatus.Fail)

                If dtUserAC.Rows(0)(1).ToString.Trim.Equals(ServiceProviderStatus.Delisted) Then
                    bIsDelisted = True
                    bIsSuspended = False
                    bIsLocked = False
                    bIsActivated = False
                ElseIf dtUserAC.Rows(0)(1).ToString.Trim.Equals(ServiceProviderStatus.Suspended) Then
                    bIsDelisted = False
                    bIsSuspended = True
                    bIsLocked = False
                    bIsActivated = False
                ElseIf dtUserAC.Rows(0)(0).ToString.Trim.Equals("S") Then 'locked
                    bIsDelisted = False
                    bIsSuspended = False
                    bIsLocked = True
                    bIsActivated = False

                    ' CRE16-026 (Change email for locked SP) [Start][Winnie]                    
                ElseIf udtUserACBLL.IsUserACPendingActivation(Me.txt_accAct_SPID.Text) Then 'HCSPUserAC is not activated
                    'ElseIf dtUserAC.Rows(0)(0).ToString.Trim.Equals("P") Then
                    ' CRE16-026 (Change email for locked SP) [End][Winnie]
                    bIsDelisted = False
                    bIsSuspended = False
                    bIsLocked = False
                    bIsActivated = True
                End If
            End If

            If bIsDelisted Then
                Me.udcErrorMessage.BuildMessageBox()
                'Me.udcErrorMessage.AddMessage("990000", "E", "00066")
                Me.udcErrorMessage.AddMessage("990000", "E", "00061")
            ElseIf bIsSuspended Then
                Me.udcErrorMessage.BuildMessageBox()
                Me.udcErrorMessage.AddMessage("990000", "E", "00060")
            ElseIf bIsLocked Then
                Me.udcErrorMessage.BuildMessageBox()
                Me.udcErrorMessage.AddMessage("990000", "E", "00071")
            ElseIf bIsActivated Then
                Me.udcErrorMessage.BuildMessageBox()
                Me.udcErrorMessage.AddMessage("990000", "E", "00146")
                'Else
                '    udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Validate Account Info fail", Me.txt_accAct_SPID.Text, Nothing)
            End If
        End If

        udtAuditLogEntry.AddDescripton("Activation Code", Session(strChangePasswordCode))
        udtAuditLogEntry.AddDescripton("SPID", Me.txt_accAct_SPID.Text)
        udtAuditLogEntry.AddDescripton("Token PIN", Me.txt_accAct_tokenPIN.Text)
        Me.udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, "Validate Account Info fail", LogID.LOG00009, Me.txt_accAct_SPID.Text.Trim, Nothing)
    End Sub

    Protected Sub btn_accAct_step4_next_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btn_accAct_step4_next.Click
        Dim err As Boolean = False

        Me.img_err_webNewPW.Visible = False
        Me.img_err_webConfirmPW.Visible = False
        Me.img_err_ivrsNewPW.Visible = False
        Me.img_err_ivrsConfirmPW.Visible = False

        Dim strCase As String = String.Empty

        'add audit log
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.AddDescripton("Create New Password Next", "Y")
        udtAuditLogEntry.WriteStartLog(LogID.LOG00010, "Validate New Password", Me.txt_accAct_SPID.Text, Nothing)

        If udcValidator.IsEmpty(Me.txt_accAct_newPW.Text) And udcValidator.IsEmpty(Me.txt_accAct_confirmPW.Text) _
            And udcValidator.IsEmpty(Me.txt_accAct_newIVRPW.Text) And udcValidator.IsEmpty(Me.txt_accAct_confirmIVRPW.Text) Then

            If Me.txtIVRSEnabled.Text.Trim.Equals(String.Empty) Then
                'IVRS is disabled with EMPTY Web password
                strCase = "Web"
                err = Me.WebPasswordChecking()
            Else
                'Handle Both part is empty, with IVRS Enabled
                strCase = String.Empty
                Me.img_err_webNewPW.Visible = True
                Me.img_err_ivrsNewPW.Visible = True
                Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00003")
                err = True
            End If
        ElseIf (udcValidator.IsEmpty(Me.txt_accAct_newPW.Text) = False Or udcValidator.IsEmpty(Me.txt_accAct_confirmPW.Text) = False) _
            And udcValidator.IsEmpty(Me.txt_accAct_newIVRPW.Text) And udcValidator.IsEmpty(Me.txt_accAct_confirmIVRPW.Text) Then
            'Handle IVRS part is empty ONLY
            strCase = "Web"
            err = Me.WebPasswordChecking()
        ElseIf (udcValidator.IsEmpty(Me.txt_accAct_newIVRPW.Text) = False Or udcValidator.IsEmpty(Me.txt_accAct_confirmIVRPW.Text) = False) _
            And udcValidator.IsEmpty(Me.txt_accAct_newPW.Text) And udcValidator.IsEmpty(Me.txt_accAct_confirmPW.Text) Then
            'Handle Web part is empty ONLY
            strCase = "IVRS"
            err = Me.IVRSPasswordChecking()
        Else
            'Handle both part is not empty
            strCase = "Both"
            If Me.WebPasswordChecking() = False And Me.IVRSPasswordChecking() = False Then
                err = False
            Else
                err = True
            End If
        End If

        Me.udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00019, "Show Validate New Password Fail Messagebox")
        Me.udcInfoMessageBox.BuildMessageBox()

        If Not err Then
            Dim strFunctCode, strSeverityCode, strMsgCode As String
            Dim sm As Common.ComObject.SystemMessage

            Try
                ChangeTimeLine("3")
                If strCase.Equals("Web") Then
                    Me.ResetWebPassword(Me.txt_accAct_newPW.Text)
                ElseIf strCase.Equals("IVRS") Then
                    Me.ResetIVRSPassword(Me.txt_accAct_newIVRPW.Text)
                ElseIf strCase.Equals("Both") Then
                    Me.ResetBothPassword(Me.txt_accAct_newPW.Text, Me.txt_accAct_newIVRPW.Text)
                End If

                Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                Me.udcInfoMessageBox.AddMessage(FUNCTION_CODE, "I", "00001")
                Me.udcInfoMessageBox.BuildMessageBox()
                Me.MultiView1.ActiveViewIndex = 2
                udtAuditLogEntry.AddDescripton("Create New Password Next", "Y")
                udtAuditLogEntry.WriteEndLog(LogID.LOG00011, "Validate New Password successful", Me.txt_accAct_SPID.Text, Nothing)
            Catch eSql As SqlClient.SqlException
                If eSql.Number = 50000 Then
                    strFunctCode = "990001"
                    strSeverityCode = "D"
                    strMsgCode = eSql.Message
                    sm = New Common.ComObject.SystemMessage(strFunctCode, strSeverityCode, strMsgCode)
                    Me.udcErrorMessage.AddMessage(sm)
                    Me.udcErrorMessage.BuildMessageBox()
                Else
                    Throw eSql
                End If
            Catch ex As Exception
                Throw ex
            End Try
        Else
            udtAuditLogEntry.AddDescripton("Create New Password Next", "Y")
            udtAuditLogEntry.WriteEndLog(LogID.LOG00012, "Validate New Password fail", Me.txt_accAct_SPID.Text, Nothing)
        End If

        ''Web Part Start
        'If udcValidator.IsEmpty(Me.txt_accAct_newPW.Text) Then
        '    err = True
        '    Me.udcErrorMessage.AddMessage("990000", "E", "00056")
        '    Me.img_err_webNewPW.Visible = True
        'Else
        '    If Not IsPasswordFormatOK(Trim(Me.txt_accAct_newPW.Text)) Then
        '        err = True
        '        Me.udcErrorMessage.AddMessage("990000", "E", "00057")
        '        Me.img_err_webNewPW.Visible = True
        '    Else
        '        If udcValidator.IsEmpty(Me.txt_accAct_confirmPW.Text) Then
        '            err = True
        '            Me.udcErrorMessage.AddMessage("990000", "E", "00058")
        '            Me.img_err_webConfirmPW.Visible = True
        '        Else
        '            If Not IsConfirmPasswordMatch(Trim(Me.txt_accAct_newPW.Text), Trim(Me.txt_accAct_confirmPW.Text)) Then
        '                err = True
        '                Me.udcErrorMessage.AddMessage("990000", "E", "00059")
        '                Me.img_err_webNewPW.Visible = True
        '                Me.img_err_webConfirmPW.Visible = True
        '            End If
        '        End If
        '    End If
        'End If

        'Me.udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00012, "Validate New Password fail")
        'Me.udcInfoMessageBox.BuildMessageBox()

        'If Not err Then
        '    ChangeTimeLine("3")
        '    ResetPassword(Me.txt_accAct_newPW.Text)
        '    Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
        '    Me.udcInfoMessageBox.AddMessage(FUNCTION_CODE, "I", "00001")
        '    Me.udcInfoMessageBox.BuildMessageBox()
        '    Me.MultiView1.ActiveViewIndex = 2
        '    udtAuditLogEntry.WriteEndLog(LogID.LOG00011, "Validate New Password successful", Me.txt_accAct_SPID.Text, Nothing)
        'Else
        '    udtAuditLogEntry.WriteEndLog(LogID.LOG00012, "Validate New Password fail", Me.txt_accAct_SPID.Text, Nothing)
        'End If
        ''Web Part End

        ''IVRS Part Start  
        'If udcValidator.IsEmpty(Me.txt_accAct_newIVRPW.Text) Then
        '    err = True
        '    Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00009")
        '    Me.img_err_ivrsConfirmPW.Visible = True
        'Else
        '    If Not IsIVRPasswordFormatOK(Trim(Me.txt_accAct_newIVRPW.Text)) Then
        '        err = True
        '        Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00008")
        '        Me.img_err_ivrsNewPW.Visible = True
        '    Else
        '        If udcValidator.IsEmpty(Me.txt_accAct_confirmIVRPW.Text) Then
        '            err = True
        '            Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00006")
        '            Me.img_err_ivrsConfirmPW.Visible = True
        '        Else
        '            If Not IsConfirmPasswordMatch(Trim(Me.txt_accAct_newIVRPW.Text), Trim(Me.txt_accAct_confirmIVRPW.Text)) Then
        '                err = True
        '                Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00007")
        '                Me.img_err_ivrsNewPW.Visible = True
        '                Me.img_err_ivrsConfirmPW.Visible = True
        '            End If
        '        End If
        '    End If
        'End If

        'If Not err Then
        '    Dim strFunctCode, strSeverityCode, strMsgCode As String
        '    Dim sm As Common.ComObject.SystemMessage

        '    Session("strNewIVRSPassword") = Me.txt_accAct_newIVRPW.Text

        '    Try
        '        'ActivateAccount()
        '        Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
        '        Me.udcInfoMessageBox.AddMessage(FUNCTION_CODE, "I", "00002")
        '        Me.udcInfoMessageBox.BuildMessageBox()
        '        Me.MultiView1.ActiveViewIndex = 5
        '    Catch eSql As SqlClient.SqlException
        '        Session("strNewIVRSPassword") = ""
        '        If eSql.Number = 50000 Then
        '            strFunctCode = "990001"
        '            strSeverityCode = "D"
        '            strMsgCode = eSql.Message
        '            sm = New Common.ComObject.SystemMessage(strFunctCode, strSeverityCode, strMsgCode)
        '            Me.udcErrorMessage.AddMessage(sm)
        '            Me.udcErrorMessage.BuildMessageBox()
        '        Else
        '            Throw eSql
        '        End If
        '    Catch ex As Exception
        '        Session("strNewIVRSPassword") = ""
        '        Throw ex
        '    End Try
        'Else
        '    Session("strNewIVRSPassword") = ""
        'End If
        ''IVRS Part End
    End Sub

    Protected Sub btn_accAct_step5_next_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btn_accAct_step5_next.Click
        Dim err As Boolean = False

        Me.img_err_ivrsNewPW.Visible = False
        Me.img_err_ivrsConfirmPW.Visible = False

        If udcValidator.IsEmpty(Me.txt_accAct_newIVRPW.Text) Then
            err = True
            Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00009")
            Me.img_err_ivrsConfirmPW.Visible = True
        Else
            If Not IsIVRPasswordFormatOK(Trim(Me.txt_accAct_newIVRPW.Text)) Then
                err = True
                Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00008")
                Me.img_err_ivrsNewPW.Visible = True
            Else
                If udcValidator.IsEmpty(Me.txt_accAct_confirmIVRPW.Text) Then
                    err = True
                    Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00006")
                    Me.img_err_ivrsConfirmPW.Visible = True
                Else
                    If Not IsConfirmPasswordMatch(Trim(Me.txt_accAct_newIVRPW.Text), Trim(Me.txt_accAct_confirmIVRPW.Text)) Then
                        err = True
                        Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00007")
                        Me.img_err_ivrsNewPW.Visible = True
                        Me.img_err_ivrsConfirmPW.Visible = True
                    End If
                End If
            End If
        End If

        'Me.udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, "Validation Fail")

        If Not err Then
            Dim strFunctCode, strSeverityCode, strMsgCode As String
            Dim sm As Common.ComObject.SystemMessage

            Session("strNewIVRSPassword") = Me.txt_accAct_newIVRPW.Text

            Try
                'ActivateAccount()
                Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                Me.udcInfoMessageBox.AddMessage(FUNCTION_CODE, "I", "00002")
                Me.udcInfoMessageBox.BuildMessageBox()
                Me.MultiView1.ActiveViewIndex = 5
            Catch eSql As SqlClient.SqlException
                Session("strNewIVRSPassword") = ""
                If eSql.Number = 50000 Then
                    strFunctCode = "990001"
                    strSeverityCode = "D"
                    strMsgCode = eSql.Message
                    sm = New Common.ComObject.SystemMessage(strFunctCode, strSeverityCode, strMsgCode)
                    Me.udcErrorMessage.AddMessage(sm)
                    Me.udcErrorMessage.BuildMessageBox()
                Else
                    Throw eSql
                End If
            Catch ex As Exception
                Session("strNewIVRSPassword") = ""
                Throw ex
            End Try
        Else
            Session("strNewIVRSPassword") = ""
        End If
    End Sub

    Protected Sub btn_skipIVRS_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btn_skipIVRS.Click
        Session("strNewIVRSPassword") = ""
        'ActivateAccount()
        Me.udcErrorMessage.BuildMessageBox()
        Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
        Me.udcInfoMessageBox.AddMessage(FUNCTION_CODE, "I", "00003")
        Me.udcInfoMessageBox.BuildMessageBox()
        Me.MultiView1.ActiveViewIndex = 5
    End Sub

    Private Sub IsSessionExpired()
        If IsNothing(Session(IsPageSessionAlive)) Then
            Response.Redirect("~/en/sessionexpired.aspx")
        End If
    End Sub

    Private Function WebPasswordChecking() As Boolean
        Dim err As Boolean = False

        If udcValidator.IsEmpty(Me.txt_accAct_newPW.Text) Then
            err = True
            Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00008")
            Me.img_err_webNewPW.Visible = True
        Else
            If Not IsPasswordFormatOK(Trim(Me.txt_accAct_newPW.Text)) Then
                err = True
                Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00009")
                Me.img_err_webNewPW.Visible = True
            Else
                If udcValidator.IsEmpty(Me.txt_accAct_confirmPW.Text) Then
                    err = True
                    Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00010")
                    Me.img_err_webConfirmPW.Visible = True
                Else
                    If Not IsConfirmPasswordMatch(Trim(Me.txt_accAct_newPW.Text), Trim(Me.txt_accAct_confirmPW.Text)) Then
                        err = True
                        Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00011")
                        Me.img_err_webNewPW.Visible = True
                        Me.img_err_webConfirmPW.Visible = True
                    End If
                End If
            End If
        End If

        Return err
    End Function

    Private Function IVRSPasswordChecking() As Boolean
        Dim Err As Boolean = False

        If udcValidator.IsEmpty(Me.txt_accAct_newIVRPW.Text) Then
            Err = True
            Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00004")
            Me.img_err_ivrsNewPW.Visible = True
        Else
            If Not IsIVRPasswordFormatOK(Trim(Me.txt_accAct_newIVRPW.Text)) Then
                Err = True
                Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00005")
                Me.img_err_ivrsNewPW.Visible = True
            Else
                If udcValidator.IsEmpty(Me.txt_accAct_confirmIVRPW.Text) Then
                    Err = True
                    Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00007")
                    Me.img_err_ivrsConfirmPW.Visible = True
                Else
                    If Not IsConfirmPasswordMatch(Trim(Me.txt_accAct_newIVRPW.Text), Trim(Me.txt_accAct_confirmIVRPW.Text)) Then
                        Err = True
                        Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00006")
                        Me.img_err_ivrsNewPW.Visible = True
                        Me.img_err_ivrsConfirmPW.Visible = True
                    End If
                End If
            End If
        End If

        Return Err
    End Function

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
End Class