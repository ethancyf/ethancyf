Imports Common.ComFunction
Imports Common.Encryption
Imports Common.ComObject
Imports Common.Component
Imports Common.Format
Imports Common.ComFunction.AccountSecurity
Imports Common.DataAccess
Imports Common.Component.Token
Imports HCSP.BLL

Partial Public Class RecoverLogin
    Inherits BasePage

    Dim udcGeneralFun As New GeneralFunction
    Dim udcValidator As New Common.Validation.Validator
    Dim udcRecoverLoginBll As New BLL.RecoverLoginBLL
    Dim udcLoginBll As New BLL.LoginBLL
    Dim udtUserACBLL As New Common.Component.UserAC.UserACBLL

    Const FUNCTION_CODE As String = Common.Component.FunctCode.FUNT020008
    Const SESS_VerificationCode As String = "VerificationCode"
    Const SESS_VerificationCode_LastSent As String = "VerificationCode_LastSent"
    Const SESS_SP_TSMP As String = "SP_TSMP"
    Const SESS_ActiveViewIndex As String = "ActiveViewIndex"
    Const SESS_IsPageSessionAlive As String = "IsRecoverLoginPageSessionAlive"

    Public Class ActiveViewIndex
        Public Const Step1 As Integer = 0
        Public Const Step2a As Integer = 1
        Public Const Step2b As Integer = 2
        Public Const Step3 As Integer = 3
        Public Const Step4 As Integer = 4
    End Class


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Buffer = True
        Response.ExpiresAbsolute = Now().Subtract(New TimeSpan(1, 0, 0, 0))
        Response.Expires = 0
        Response.CacheControl = "no-cache"

        Page.Response.Cache.SetCacheability(HttpCacheability.NoCache)

        SetLanguage()

        If Not IsPostBack Then
            Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, "Recover Login Page Load")

            Me.RemoveSession()

            ' Step 1
            Me.MultiView1.ActiveViewIndex = ActiveViewIndex.Step1
            Me.InitControl()
        End If

        ' Check Session Expired
        If Me.MultiView1.ActiveViewIndex <> ActiveViewIndex.Step1 Then
            Me.IsSessionExpired()
        End If

        ' Redirect to Step 1 for Improper Access 
        If Session(SESS_ActiveViewIndex) IsNot Nothing Then
            If Session(SESS_ActiveViewIndex) <> Me.MultiView1.ActiveViewIndex Then
                Response.Redirect(ClaimVoucherMaster.FullVersionPage.RecoverLogin)
            End If
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

        ' Step2a
        If Me.MultiView1.ActiveViewIndex = ActiveViewIndex.Step2a Then
            If Session(SESS_VerificationCode_LastSent) Is Nothing Then
                'Session timeout
                Throw New Exception("Session Expired!")
            Else
                Dim dtmLastSent As DateTime = Session(SESS_VerificationCode_LastSent)
                lbl_step2a_VerCode_RefTime.Text = HttpContext.GetGlobalResourceObject("Text", "VerificationCodeLastSent").ToString.Replace("%s", dtmLastSent.ToString("HH:mm"))
            End If
        End If

        If Me.MultiView1.ActiveViewIndex = ActiveViewIndex.Step3 Then
            ChangeIVRSPassword(chkChgIVRSPwd.Checked)
        End If

        Me.ModalPopupConfirmCancel.PopupDragHandleControlID = Me.ucNoticePopUpConfirm.Header.ClientID
    End Sub

    Private Sub SetLanguage()
        Dim selectedValue As String

        selectedValue = Session("language")

        ' Password
        Dim strvalue As String = String.Empty
        Dim strvalue2 As String = String.Empty

        udcGeneralFun.getSystemParameter("PasswordRuleNumber", strvalue, strvalue2)

        Me.txt_Step3_newPW.Attributes.Add("onKeyUp", "checkPassword(this.value,'" & CInt(strvalue.Trim) & "', '" & CInt(strvalue2.Trim) & "', 'strength1','strength2','strength3','progressBar', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthPoor") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthModerate") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthStrong") & "','direction2','direction1');")


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

        MyBase.InitializeCulture()

    End Sub

    ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
    ' -----------------------------------------------------------------------------------------
    Private Function IsSpIdQualifiedToChangePassword(ByVal strSPID As String) As Boolean
        Dim blnIsQualified As Boolean = True

        Dim dtUserAC As New DataTable
        Dim udtUserACBLL As New Common.Component.UserAC.UserACBLL

        dtUserAC = udtUserACBLL.GetHCSPUserACStatus(strSPID)

        If dtUserAC.Rows.Count <= 0 Then
            Me.udcErrorMessage.AddMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00002)
            blnIsQualified = False
        Else
            If dtUserAC.Rows(0)(1).ToString.Trim.Equals(ServiceProviderStatus.Delisted) Then
                Me.udcErrorMessage.BuildMessageBox()
                Me.udcErrorMessage.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00061)
                blnIsQualified = False

            ElseIf dtUserAC.Rows(0)(1).ToString.Trim.Equals(ServiceProviderStatus.Suspended) Then
                Me.udcErrorMessage.BuildMessageBox()
                Me.udcErrorMessage.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00060)
                blnIsQualified = False

            ElseIf udtUserACBLL.IsUserACPendingActivation(Me.txt_step1_SPID.Text) Then 'HCSPUserAC Record status is not activated
                Me.udcErrorMessage.BuildMessageBox()
                Me.udcErrorMessage.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00146)
                blnIsQualified = False
            End If
        End If

        Return blnIsQualified
    End Function
    ' CRE16-004 (Enable SP to unlock account) [End][Winnie]

    Private Sub GetLatestTSMPtoSession(ByVal strSPID As String)
        Dim dt As New DataTable
        Dim udtUserACBLL As New Common.Component.UserAC.UserACBLL

        ' CRE16-026 (Change email for locked SP) [Start][Winnie]
        dt = udtUserACBLL.GetTSMPBySPID(strSPID)
        ' CRE16-026 (Change email for locked SP) [End][Winnie]

        If dt.Rows.Count <= 0 Then
            Session(SESS_SP_TSMP) = Nothing
        Else
            Session(SESS_SP_TSMP) = dt.Rows(0)("TSMP")
        End If
    End Sub

    Protected Sub btn_step1_next_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btn_step1_next.Click
        Dim err As Boolean = False
        Dim SM As Common.ComObject.SystemMessage
        Dim udtUserAC As Common.Component.UserAC.UserACModel = Nothing
        Dim udtUserACBLL As New Common.Component.UserAC.UserACBLL
        Dim dtUserAC As New DataTable

        Me.img_err_spid.Visible = False
        Me.img_err_tokenPIN.Visible = False
        Me.imgEmailAlert.Visible = False

        'add audit log
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.AddDescripton("SPID", Me.txt_step1_SPID.Text.Trim)
        udtAuditLogEntry.AddDescripton("Email", txt_step1_RegEmail.Text.Trim)
        udtAuditLogEntry.AddDescripton("Token PIN", Me.txt_step1_tokenPIN.Text)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Step1 Validate Account Info", Me.txt_step1_SPID.Text.Trim, Nothing)

        If udcValidator.IsEmpty(Me.txt_step1_SPID.Text) Then
            err = True
            Me.udcErrorMessage.AddMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00001)
            Me.img_err_spid.Visible = True
        End If

        If udcValidator.IsEmpty(Me.txt_step1_RegEmail.Text.Trim) Then
            err = True
            Me.udcErrorMessage.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00005)
            Me.imgEmailAlert.Visible = True
        Else
            SM = udcValidator.chkEmailAddress(txt_step1_RegEmail.Text.Trim)
            If Not SM Is Nothing Then
                err = True
                imgEmailAlert.Visible = True
                udcErrorMessage.AddMessage(SM)
            End If
        End If

        If udcValidator.IsEmpty(Me.txt_step1_tokenPIN.Text) Then
            err = True
            Me.udcErrorMessage.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00044)
            Me.img_err_tokenPIN.Visible = True
        End If


        If Not err AndAlso Not IsSpIdQualifiedToChangePassword(Me.txt_step1_SPID.Text) Then
            err = True
            Me.img_err_spid.Visible = True
            Me.img_err_tokenPIN.Visible = True
            Me.imgEmailAlert.Visible = True

            udtAuditLogEntry.AddDescripton("SPID", Me.txt_step1_SPID.Text)
            udtAuditLogEntry.WriteLog(LogID.LOG00004, "Step1 Validate Account Info fail: Incorrect SPID", Me.txt_step1_SPID.Text.Trim, "")
        End If

        'Check Email Address
        If Not err AndAlso Not udcRecoverLoginBll.IsSpIdRegisteredEmailMatch(Me.txt_step1_SPID.Text.Trim, txt_step1_RegEmail.Text.Trim) Then
            err = True
            Me.udcErrorMessage.AddMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00002)
            Me.img_err_spid.Visible = True
            Me.imgEmailAlert.Visible = True
            Me.img_err_tokenPIN.Visible = True

            udtAuditLogEntry.AddDescripton("SPID", Me.txt_step1_SPID.Text)
            udtAuditLogEntry.AddDescripton("Email", txt_step1_RegEmail.Text.Trim())
            udtAuditLogEntry.WriteLog(LogID.LOG00005, "Step1 Validate Account Info fail: Incorrect Email for this SPID", Me.txt_step1_SPID.Text.Trim, "")
        End If

        If Not err Then
            ' Check allow send email
            If Not udcRecoverLoginBll.AllowSubmitResetPasswordEmail(Me.txt_step1_SPID.Text.Trim) Then
                err = True
                Me.img_err_spid.Visible = True
                Me.img_err_tokenPIN.Visible = True
                Me.imgEmailAlert.Visible = True

                Dim strCheckMinute As String = udcRecoverLoginBll.GetVerificationCodeResendMinute()
                Me.udcErrorMessage.AddMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00018, "%s", strCheckMinute)
            End If
        End If

        ' Token
        If Not err Then
            ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            ' Handle Principle lockout case
            If err = False Then
                If (New Token.TokenBLL).IsUserLockout(Me.txt_step1_SPID.Text.Trim) Then
                    err = True
                    Me.udcErrorMessage.AddMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00017)
                    Me.img_err_tokenPIN.Visible = True

                    udtAuditLogEntry.AddDescripton("SPID", Me.txt_step1_SPID.Text)
                    udtAuditLogEntry.WriteLog(LogID.LOG00006, "Step1 Validate Account Info fail: Token Principle Lockout", Me.txt_step1_SPID.Text.Trim, "")
                End If
            End If

            ' Accept normal and locked token
            'If (err = False) AndAlso Not IsSpIdTokenPinMatch(Me.txt_step1_SPID.Text, Me.txt_step1_tokenPIN.Text) Then
            If (err = False) AndAlso Not udcRecoverLoginBll.IsSpIdTokenPinMatch(Me.txt_step1_SPID.Text, Me.txt_step1_tokenPIN.Text, True) Then
                ' CRE16-004 (Enable SP to unlock account) [End][Winnie]
                err = True
                Me.udcErrorMessage.AddMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00002)
                Me.img_err_spid.Visible = True
                Me.imgEmailAlert.Visible = True
                Me.img_err_tokenPIN.Visible = True

                udtAuditLogEntry.AddDescripton("TokenPassword", Me.txt_step1_tokenPIN.Text.Trim)
                udtAuditLogEntry.WriteLog(LogID.LOG00007, "Step1 Validate Account Info fail: Incorrect Token Passcode", Me.txt_step1_SPID.Text.Trim, "")
            End If
        End If


        If Not err Then

            udtAuditLogEntry.AddDescripton("SPID", Me.txt_step1_SPID.Text.Trim)
            udtAuditLogEntry.AddDescripton("Email", txt_step1_RegEmail.Text.Trim)
            udtAuditLogEntry.AddDescripton("Token PIN", Me.txt_step1_tokenPIN.Text)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Step1 Validate Account Info successful", Me.txt_step1_SPID.Text.Trim, Nothing)

            'Obtain the latest Timestamp
            GetLatestTSMPtoSession(Me.txt_step1_SPID.Text)

            udcLoginBll.UpdateLoginDtmInNonLoginPage(Me.txt_step1_SPID.Text, LoginStatus.Success)

            ' Send verification code
            If Me.SubmitResetPasswordEmail() Then

                Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete

                Me.udcInfoMessageBox.AddMessage(FUNCTION_CODE, SeverityCode.SEVI, MsgCode.MSG00002)
                Me.udcInfoMessageBox.BuildMessageBox()

                Me.MultiView1.ActiveViewIndex = ActiveViewIndex.Step2a
                Me.InitControl()

                ' From step2a, the whole process must be completed before session expired
                Session(SESS_IsPageSessionAlive) = "Y"
            End If

        Else
            dtUserAC = udtUserACBLL.GetHCSPUserACStatus(Me.txt_step1_SPID.Text)

            If dtUserAC.Rows.Count = 1 Then
                udcLoginBll.UpdateLoginDtmInNonLoginPage(Me.txt_step1_SPID.Text, LoginStatus.Fail)
            End If
        End If

        udtAuditLogEntry.AddDescripton("SPID", Me.txt_step1_SPID.Text.Trim)
        udtAuditLogEntry.AddDescripton("Email", txt_step1_RegEmail.Text.Trim)
        udtAuditLogEntry.AddDescripton("Token PIN", Me.txt_step1_tokenPIN.Text)
        Me.udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, "Step1 Validate Account Info fail", LogID.LOG00003, Me.txt_step1_SPID.Text.Trim, Nothing)
    End Sub

    ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
    ' -----------------------------------------------------------------------------------------
    Protected Sub btn_step2a_resend_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btn_step2a_Resend.Click
        Me.udcErrorMessage.Visible = False
        Me.udcInfoMessageBox.Visible = False
        Me.imgVerCodeAlert.Visible = False
        Me.imgHKIDAlert.Visible = False

        'add audit log
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00012, "Step2a Resend Verification Code Click", Me.txt_step1_SPID.Text.Trim, Nothing)

        udcErrorMessage.Clear()
        udcInfoMessageBox.Clear()

        ' Check allow send email
        If udcRecoverLoginBll.AllowSubmitResetPasswordEmail(Me.txt_step1_SPID.Text.Trim) Then

            If Me.SubmitResetPasswordEmail() Then
                ' Resend email with new code
                Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                Me.udcInfoMessageBox.AddMessage(FUNCTION_CODE, SeverityCode.SEVI, MsgCode.MSG00003)
                Me.udcInfoMessageBox.BuildMessageBox()

                udtAuditLogEntry.WriteEndLog(LogID.LOG00013, "Step2a Resend Verification Code Click successful", Me.txt_step1_SPID.Text.Trim, Nothing)
            End If
        Else
            Dim strCheckMinute As String = udcRecoverLoginBll.GetVerificationCodeResendMinute()
            Me.udcErrorMessage.AddMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00007, "%s", strCheckMinute)
        End If

        Me.udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, "Step2a Resend Verification Code Click fail", LogID.LOG00014, Me.txt_step1_SPID.Text.Trim, Nothing)
    End Sub
    ' CRE16-004 (Enable SP to unlock account) [End][Winnie]

    Protected Sub btn_step2a_next_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btn_step2a_next.Click
        Dim err As Boolean = False

        Me.imgVerCodeAlert.Visible = False
        udcErrorMessage.Visible = False
        udcInfoMessageBox.Visible = False

        Me.imgVerCodeAlert.Visible = False
        Me.imgHKIDAlert.Visible = False

        Dim strSPID As String = Me.txt_step1_SPID.Text.Trim
        Dim strHKID As String = String.Empty

        'add audit log
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00015, "Step2a Verification", strSPID, Nothing)

        Dim blnLocked As Boolean = udtUserACBLL.IsSPAccountLocked(strSPID)

        ' Check Verification Code
        If udcValidator.IsEmpty(Me.txt_step2a_VerCode.Text) Then
            err = True
            Me.udcErrorMessage.AddMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00003)
            Me.imgVerCodeAlert.Visible = True

        Else
            Dim dtmLastSent As DateTime = Session(SESS_VerificationCode_LastSent)

            If udcRecoverLoginBll.IsVerificationCodeExpired(dtmLastSent) Then
                ' Code Expired
                err = True
                Me.udcErrorMessage.AddMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00008)
                Me.imgVerCodeAlert.Visible = True

            Else
                Dim strVerificationCode As String = Session(SESS_VerificationCode)

                If Not Me.txt_step2a_VerCode.Text.Equals(strVerificationCode) Then
                    ' Not Match
                    err = True
                    Me.imgVerCodeAlert.Visible = True

                    If Not blnLocked Then
                        Me.udcErrorMessage.AddMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00005)
                    Else
                        Me.udcErrorMessage.AddMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00006)
                        Me.imgHKIDAlert.Visible = True
                    End If

                    udtAuditLogEntry.AddDescripton("Verification Code", Me.txt_step2a_VerCode.Text)
                    udtAuditLogEntry.WriteLog(LogID.LOG00018, "Step2a Verification fail: Verification Code not match", strSPID, Nothing)
                End If
            End If
        End If

        If blnLocked Then
            ' Check HKIC No.
            strHKID = UCase(txt_step2a_RegHKIDPrefix.Text.Trim) + txt_step2a_RegHKID.Text.Trim + UCase(txt_step2a_RegHKIDdigit.Text.Trim)

            ' Clear input display for HKIC No.
            txt_step2a_RegHKIDPrefix.Text = String.Empty
            txt_step2a_RegHKID.Text = String.Empty
            txt_step2a_RegHKIDdigit.Text = String.Empty

            If strHKID = String.Empty Then
                ' Empty SP HKID                
                err = True
                Me.imgHKIDAlert.Visible = True
                Me.udcErrorMessage.AddMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00004)

            Else
                ' Invalid format SP HKID
                If Not err Then
                    Dim udtSM As SystemMessage = udcValidator.chkHKID(strHKID)
                    If Not udtSM Is Nothing Then
                        err = True
                        Me.imgVerCodeAlert.Visible = True
                        Me.imgHKIDAlert.Visible = True
                        Me.udcErrorMessage.AddMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00006)

                        udtAuditLogEntry.AddDescripton("HKIC No.", strHKID)
                        udtAuditLogEntry.WriteLog(LogID.LOG00019, "Step2a Verification fail: HKIC not match", strSPID, Nothing)
                    End If
                End If

                If Not err Then
                    If Not udcRecoverLoginBll.IsSpIdHKIDCMatch(strSPID, strHKID) Then
                        ' HKID Not Match
                        err = True
                        Me.imgVerCodeAlert.Visible = True
                        Me.imgHKIDAlert.Visible = True
                        Me.udcErrorMessage.AddMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00006)

                        udtAuditLogEntry.AddDescripton("HKIC No.", strHKID)
                        udtAuditLogEntry.WriteLog(LogID.LOG00019, "Step2a Verification fail: HKIC not match", strSPID, Nothing)
                    End If
                End If
            End If
        End If

        If Not err Then
            ' Reset to "Normal" Token
            Me.ResetToken(strSPID)

            ' Go to Step 2b
            Me.MultiView1.ActiveViewIndex = ActiveViewIndex.Step2b
            Me.InitControl()

            udtAuditLogEntry.AddDescripton("Verification Code", Me.txt_step2a_VerCode.Text)
            udtAuditLogEntry.AddDescripton("HKIC No.", strHKID)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00016, "Step2a Verification successful", strSPID, Nothing)
        End If

        udtAuditLogEntry.AddDescripton("Verification Code", Me.txt_step2a_VerCode.Text)
        udtAuditLogEntry.AddDescripton("HKIC No.", strHKID)
        Me.udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, "Step2a Verification fail", LogID.LOG00017, strSPID, Nothing)
    End Sub

    Protected Sub btn_step2b_next_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btn_step2b_next.Click
        Dim err As Boolean = False

        Me.img_err_Step2b_tokenPIN.Visible = False
        udcErrorMessage.Visible = False

        'add audit log
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.AddDescripton("Token Passcode", Me.txt_Step2b_tokenPIN.Text)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00020, "Step2b Verification", Me.txt_step1_SPID.Text.Trim, Nothing)

        ' Check Token
        If udcValidator.IsEmpty(Me.txt_Step2b_tokenPIN.Text) Then
            err = True
            Me.udcErrorMessage.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00044)
            Me.img_err_Step2b_tokenPIN.Visible = True
        Else
            If (New Token.TokenBLL).IsUserLockout(Me.txt_step1_SPID.Text.Trim) Then
                err = True
                Me.udcErrorMessage.AddMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00017)
                Me.img_err_Step2b_tokenPIN.Visible = True
            End If

            ' Accpet normal token only
            If Not err AndAlso Not udcRecoverLoginBll.IsSpIdTokenPinMatch(Me.txt_step1_SPID.Text, Me.txt_Step2b_tokenPIN.Text, False) Then
                err = True
                Me.udcErrorMessage.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00308)
                Me.img_err_Step2b_tokenPIN.Visible = True
            End If
        End If

        If Not err Then
            ' Go to Step 3
            Me.MultiView1.ActiveViewIndex = ActiveViewIndex.Step3
            Me.InitControl()

            udtAuditLogEntry.AddDescripton("Token Passcode", Me.txt_Step2b_tokenPIN.Text)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00021, "Step2b Verification successful", Me.txt_step1_SPID.Text.Trim, Nothing)
            udcLoginBll.UpdateLoginDtmInNonLoginPage(Me.txt_step1_SPID.Text, LoginStatus.Success)

        Else
            udcLoginBll.UpdateLoginDtmInNonLoginPage(Me.txt_step1_SPID.Text, LoginStatus.Fail)
        End If

        udtAuditLogEntry.AddDescripton("Token Passcode", Me.txt_Step2b_tokenPIN.Text)
        Me.udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, "Step2b Verification fail", LogID.LOG00022, Me.txt_step1_SPID.Text.Trim, Nothing)
    End Sub

    Protected Sub btn_step3_next_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btn_step3_next.Click
        Dim err As Boolean = False

        Me.img_err_webNewPW.Visible = False
        Me.img_err_webConfirmPW.Visible = False
        Me.img_err_ivrsNewPW.Visible = False
        Me.img_err_ivrsConfirmPW.Visible = False

        Dim strSPID As String = Me.txt_step1_SPID.Text.Trim

        'add audit log
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00023, "Step3 Reset Password", strSPID, Nothing)

        ' Check Password
        If Me.WebPasswordChecking() = True Then
            err = True
        End If

        If chkChgIVRSPwd.Checked Then
            If IVRSPasswordChecking() = True Then
                err = True
            End If
        End If

        udtAuditLogEntry.AddDescripton("Change Web Password", "Y")
        udtAuditLogEntry.AddDescripton("Change IVRS Password", IIf(chkChgIVRSPwd.Checked, "Y", "N"))

        If Not err Then
            Dim strFunctCode, strSeverityCode, strMsgCode As String
            Dim sm As Common.ComObject.SystemMessage
            Me.udcErrorMessage.Clear()

            Dim udtUserACBLL As New Common.Component.UserAC.UserACBLL

            'Obtain the latest Timestamp
            GetLatestTSMPtoSession(Me.txt_step1_SPID.Text)

            Try
                ' Update Password
                If Not chkChgIVRSPwd.Checked Then
                    Me.ResetWebPassword(Me.txt_Step3_newPW.Text)
                Else
                    Me.ResetBothPassword(Me.txt_Step3_newPW.Text, Me.txt_Step3_newIVRPW.Text)
                End If

                ' Unlock Account
                Dim blnLocked As Boolean = udtUserACBLL.IsSPAccountLocked(strSPID)
                If blnLocked Then
                    udtUserACBLL.UpdateRecordStatusPasswordFailCount(strSPID, strSPID, New Database)
                    Me.udcInfoMessageBox.AddMessage(FUNCTION_CODE, "I", "00004")                    
                Else
                    Me.udcInfoMessageBox.AddMessage(FUNCTION_CODE, "I", "00001")
                End If

                ' Clear Token Fail Count
                Me.ResetToken(strSPID)

                Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                Me.udcInfoMessageBox.BuildMessageBox()

                udtAuditLogEntry.WriteEndLog(LogID.LOG00024, "Step3 Reset Password successful", strSPID, Nothing)

                ' Go to Step 4
                Me.MultiView1.ActiveViewIndex = ActiveViewIndex.Step4
                Me.InitControl()
                udcLoginBll.UpdateLoginDtmInNonLoginPage(strSPID, LoginStatus.Success)

                udtAuditLogEntry.AddDescripton("Unlock Account", IIf(blnLocked, "Y", "N"))
                udtAuditLogEntry.WriteLog(LogID.LOG00026, "Step4 Complete", strSPID, Nothing)

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
                Throw
            End Try
        Else
            Me.udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, "Step3 Reset Password fail", LogID.LOG00025, strSPID, Nothing)
        End If
    End Sub

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

    ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
    ' -----------------------------------------------------------------------------------------
    Private Function SubmitResetPasswordEmail() As Boolean
        ' Generate new verfication code        
        Dim strVerificationCode As String = udcGeneralFun.generateVerificationCode()
        Dim dtmLastSent As DateTime = System.DateTime.Now
        Dim strRefTime As String = dtmLastSent.ToString("HH:mm")

        'add audit log
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00009, "Submit Reset Password Email", Me.txt_step1_SPID.Text.Trim, Nothing)

        ' Send Email
        Try
            If udcRecoverLoginBll.SubmitResetPasswordEmail(Me.txt_step1_SPID.Text, strVerificationCode, strRefTime) Then
                ' Save to Session
                Session(SESS_VerificationCode) = strVerificationCode
                Session(SESS_VerificationCode_LastSent) = dtmLastSent

                ' Update Reference Time
                lbl_step2a_VerCode_RefTime.Text = HttpContext.GetGlobalResourceObject("Text", "VerificationCodeLastSent").ToString.Replace("%s", strRefTime)

                Me.txt_step2a_VerCode.Text = String.Empty

                udtAuditLogEntry.AddDescripton("Verification Code", strVerificationCode)
                udtAuditLogEntry.AddDescripton("Sending time", strRefTime)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00010, "Submit Reset Password Email successful", Me.txt_step1_SPID.Text.Trim, Nothing)

                Return True
            Else
                ' Fail
                udtAuditLogEntry.WriteEndLog(LogID.LOG00011, "Submit Reset Password Email fail", Me.txt_step1_SPID.Text.Trim, Nothing)
                Return False
            End If

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                Dim sm As Common.ComObject.SystemMessage
                sm = New Common.ComObject.SystemMessage("990001", "D", eSQL.Message)
                udcErrorMessage.AddMessage(sm)
            Else
                Throw eSQL
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub InitControl()

        Select Case Me.MultiView1.ActiveViewIndex
            Case ActiveViewIndex.Step1
                ' Nothing

            Case ActiveViewIndex.Step2a
                If udtUserACBLL.IsSPAccountLocked(Me.txt_step1_SPID.Text) Then
                    ' Require HKIC No.
                    trHKIC_Step2a.Visible = True
                Else
                    trHKIC_Step2a.Visible = False
                End If

            Case ActiveViewIndex.Step2b
                If udtUserACBLL.IsSPAccountLocked(Me.txt_step1_SPID.Text) Then
                    trHKIC_Step2b.Visible = True
                Else
                    trHKIC_Step2b.Visible = False
                End If

            Case ActiveViewIndex.Step3
                'Check the SP has activated the IVRS or not
                Dim blnEnableIVRS As Boolean = False

                Dim udtSPProfileBLL As New BLL.SPProfileBLL()
                Dim dt As New DataTable

                dt = udtSPProfileBLL.loadSPLoginProfile(Me.txt_step1_SPID.Text.Trim, String.Empty)

                Dim udtSchemeClaimBLL As New Scheme.SchemeClaimBLL()
                blnEnableIVRS = udtSchemeClaimBLL.CheckSchemeClaimIVRSEnabled(Me.txt_step1_SPID.Text)

                If blnEnableIVRS Then
                    ' Scheme With IVRS
                    Me.panIVRSPassword.Visible = True
                    If dt.Rows(0)("SP_IVRS_Password").ToString.Trim.Equals(String.Empty) Then
                        ' IVRS Not Activated
                        blnEnableIVRS = False
                    End If
                End If

                If blnEnableIVRS Then

                    If udtUserACBLL.IsSPAccountLocked(Me.txt_step1_SPID.Text.Trim, "IVRS") Then
                        ' Change IVRS password (Mandatory)
                        lbl_Step3_ChgIVRSPW.Visible = True
                        chkChgIVRSPwd.Visible = False
                        chkChgIVRSPwd.Checked = True

                    Else
                        ' Change IVRS password (Optional)
                        lbl_Step3_ChgIVRSPW.Visible = False
                        chkChgIVRSPwd.Visible = True
                        chkChgIVRSPwd.Checked = False
                    End If

                    ChangeIVRSPassword(chkChgIVRSPwd.Checked)

                Else
                    Me.panIVRSPassword.Visible = False
                End If

            Case ActiveViewIndex.Step4
                ' Nothing

        End Select

        Session(SESS_ActiveViewIndex) = Me.MultiView1.ActiveViewIndex

        ChangeTimeLine(Me.MultiView1.ActiveViewIndex)
    End Sub

    Private Sub ChangeIVRSPassword(ByVal blnChecked As Boolean)
        If blnChecked Then
            trIVRSPassword.Disabled = False
            txt_Step3_newIVRPW.Enabled = True
            txt_Step3_confirmIVRPW.Enabled = True
            'txt_Step3_newIVRPW.ReadOnly = False
            'txt_Step3_confirmIVRPW.ReadOnly = False
            txt_Step3_newIVRPW.Attributes.Remove("readOnly")
            txt_Step3_confirmIVRPW.Attributes.Remove("readOnly")

        Else
            trIVRSPassword.Disabled = True
            txt_Step3_newIVRPW.Enabled = False
            txt_Step3_confirmIVRPW.Enabled = False
            'txt_Step3_newIVRPW.ReadOnly = True  ' Value will be lost after postback
            'txt_Step3_confirmIVRPW.ReadOnly = True
            img_err_ivrsNewPW.Visible = False
            img_err_ivrsConfirmPW.Visible = False
            txt_Step3_newIVRPW.Attributes.Add("readOnly", "readOnly") ' Can retain value after postback
            txt_Step3_confirmIVRPW.Attributes.Add("readOnly", "readOnly")
        End If
    End Sub

    Private Sub ChangeTimeLine(ByVal strStep As String)
        Dim strhightlight, strunhightlight As String

        strhightlight = "highlightTimeline"
        strunhightlight = "unhighlightTimeline"

        Me.panStep1.CssClass = strunhightlight
        Me.panStep2.CssClass = strunhightlight
        Me.panStep3.CssClass = strunhightlight
        Me.panStep4.CssClass = strunhightlight

        Select Case strStep
            Case ActiveViewIndex.Step1
                Me.panStep1.CssClass = strhightlight

            Case ActiveViewIndex.Step2a, ActiveViewIndex.Step2b
                Me.panStep2.CssClass = strhightlight

            Case ActiveViewIndex.Step3
                Me.panStep3.CssClass = strhightlight

            Case ActiveViewIndex.Step4
                Me.panStep4.CssClass = strhightlight

        End Select
    End Sub

    Private Function WebPasswordChecking() As Boolean
        Dim err As Boolean = False
        Me.img_err_webNewPW.Visible = False
        Me.img_err_webConfirmPW.Visible = False

        If udcValidator.IsEmpty(Me.txt_Step3_newPW.Text) Then
            err = True
            Me.udcErrorMessage.AddMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00009)
            Me.img_err_webNewPW.Visible = True
        Else
            If Not IsPasswordFormatOK(Trim(Me.txt_Step3_newPW.Text)) Then
                err = True
                Me.udcErrorMessage.AddMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00010)
                Me.img_err_webNewPW.Visible = True
            Else
                If udcValidator.IsEmpty(Me.txt_Step3_confirmPW.Text) Then
                    err = True
                    Me.udcErrorMessage.AddMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00011)
                    Me.img_err_webConfirmPW.Visible = True
                Else
                    If Not IsConfirmPasswordMatch(Trim(Me.txt_Step3_newPW.Text), Trim(Me.txt_Step3_confirmPW.Text)) Then
                        err = True
                        Me.udcErrorMessage.AddMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00012)
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
        Me.img_err_ivrsNewPW.Visible = False
        Me.img_err_ivrsConfirmPW.Visible = False

        If udcValidator.IsEmpty(Me.txt_Step3_newIVRPW.Text) Then
            Err = True
            Me.udcErrorMessage.AddMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00013)
            Me.img_err_ivrsNewPW.Visible = True
        Else
            If Not IsIVRPasswordFormatOK(Trim(Me.txt_Step3_newIVRPW.Text)) Then
                Err = True
                Me.udcErrorMessage.AddMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00014)
                Me.img_err_ivrsNewPW.Visible = True
            Else
                If udcValidator.IsEmpty(Me.txt_Step3_confirmIVRPW.Text) Then
                    Err = True
                    Me.udcErrorMessage.AddMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00015)
                    Me.img_err_ivrsConfirmPW.Visible = True
                Else
                    If Not IsConfirmPasswordMatch(Trim(Me.txt_Step3_newIVRPW.Text), Trim(Me.txt_Step3_confirmIVRPW.Text)) Then
                        Err = True
                        Me.udcErrorMessage.AddMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00016)
                        Me.img_err_ivrsNewPW.Visible = True
                        Me.img_err_ivrsConfirmPW.Visible = True
                    End If
                End If
            End If
        End If

        Return Err
    End Function

    Private Function IsPasswordFormatOK(ByVal strPW As String) As Boolean
        Return udcValidator.ValidatePassword(strPW)
    End Function

    Private Function IsIVRPasswordFormatOK(ByVal strPW As String) As Boolean
        Return udcValidator.ValidateIVRSPassword(strPW)
    End Function

    Private Function IsConfirmPasswordMatch(ByVal strNewPW As String, ByVal strConfirmPW As String) As Boolean
        Return udcValidator.ChkIsIdenticial(strNewPW, strConfirmPW)
    End Function

    Private Sub ResetWebPassword(ByVal strNewPassword As String)
        udcRecoverLoginBll.ResetWebPassword(Me.txt_step1_SPID.Text, Hash(strNewPassword), Session("SP_TSMP"))
    End Sub

    Private Sub ResetBothPassword(ByVal strNewPassword As String, ByVal strNewIVRSPassword As String)
        udcRecoverLoginBll.ResetBothPassword(Me.txt_step1_SPID.Text, Hash(strNewPassword), Hash(strNewIVRSPassword), Session("SP_TSMP"))
    End Sub

    Private Sub ResetToken(ByVal strSPID As String)
        Dim udtTokenBLL As New TokenBLL
        Dim udtTokenModel As New TokenModel
        Dim udtRSA As New Common.Component.RSA_Manager.RSAServerHandler

        If udtTokenBLL.IsEnableToken() Then

            Dim udtDB As New Database
            Try
                udtDB.BeginTransaction()

                udtTokenModel = udtTokenBLL.GetTokenProfileByUserID(strSPID, DBNull.Value.ToString, udtDB)

                If Not udtTokenModel Is Nothing Then

                    If udtRSA.IsParallelRun Then
                        udtTokenBLL.UpdateRSASingletonTSMP(udtDB)
                    End If

                    udtRSA.clearRSAUserTokenFailCount(strSPID)
                End If

                udtDB.CommitTransaction()

            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try
        End If
    End Sub
    ' CRE16-004 (Enable SP to unlock account) [End][Winnie]

    Private Sub RemoveSession()
        Session.Remove(SESS_VerificationCode)
        Session.Remove(SESS_VerificationCode_LastSent)
        Session.Remove(SESS_SP_TSMP)
        Session.Remove(SESS_ActiveViewIndex)
        Session.Remove(SESS_IsPageSessionAlive)
    End Sub

    Private Sub IsSessionExpired()
        If IsNothing(Session(SESS_IsPageSessionAlive)) Then
            Response.Redirect("~/en/sessionexpired.aspx")
        End If
    End Sub

#Region "Confirm Cancel Popup function"

    Protected Sub btn_Cancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btn_step1_cancel.Click, btn_step2a_cancel.Click, btn_step2b_cancel.Click, btn_step3_cancel.Click
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00008, "Cancel Click")

        Me.ModalPopupConfirmCancel.Show()
    End Sub

    Private Sub ucNoticePopUpConfirm_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUpConfirm.ButtonClick
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)

        Select Case e
            Case ucNoticePopUp.enumButtonClick.OK
                udtAuditLogEntry.WriteLog(LogID.LOG00027, "Confirm Cancel - Yes Click")
                Response.Redirect(ClaimVoucherMaster.FullVersionPage.Login)

            Case Else
                udtAuditLogEntry.WriteLog(LogID.LOG00028, "Confirm Cancel - No Click")
        End Select
    End Sub
#End Region

End Class