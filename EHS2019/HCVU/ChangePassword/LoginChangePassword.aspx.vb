Imports Common.Component.HCVUUser
Imports Common.Validation
Imports Common.ComObject
Imports Common.DataAccess
Imports Common.Encryption
Imports Common.ComFunction
Imports Common.Component
Imports Common.ComFunction.AccountSecurity

Partial Public Class LoginChangePassword
    Inherits BasePage

    Private SESS_FirstChangePassword As String = "FirstChangePassword"
    Private VS_FirstChangePassword As String = "FirstChangePassword"
    Private Const SESS_ChangePasswordHCVUUser As String = "ChangePasswordHCVUUser"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        FunctionCode = FunctCode.FUNT010001

        If Not IsPostBack Then
            Me.PageTitle.Text = Me.GetGlobalResourceObject("Title", "ChangePassword")

            Dim udtHCVUUser As HCVUUserModel
            Dim udtHCVUUserBLL As New HCVUUserBLL
            Dim strLogUserID As String = ""

            udtHCVUUser = CType(Session(SESS_ChangePasswordHCVUUser), HCVUUserModel)
            lblUserName.Text = udtHCVUUser.UserID.ToUpper()
            strLogUserID = udtHCVUUser.UserID.Trim

            ibtnExit.Attributes.Add("OnClick", "javascript:window.opener='X';window.open('','_parent','');window.close(); return false;")

            Me.ScriptManager1.SetFocus(Me.txtOldPassword)
            ResetAlertImage()

            If Not Session(SESS_FirstChangePassword) Is Nothing Then
                ViewState(VS_FirstChangePassword) = Session(SESS_FirstChangePassword)
            Else
                Throw New Exception("Session Expired!")
            End If

            If ViewState(VS_FirstChangePassword) = "N" Then
                Dim udtGeneralFunction As New GeneralFunction
                Dim strChgPwdDay As String = ""
                udtGeneralFunction.getSystemParameter("DaysOfChangePassword", strChgPwdDay, String.Empty)
                Me.lblStatement.Text = CStr(Me.GetGlobalResourceObject("Text", "60DaysChgPwdStatement")).Replace("%s", strChgPwdDay)

                'Me.lblStatement.Text = Me.GetGlobalResourceObject("Text", "60DaysChgPwdStatement")

                ibtnConfirm.Attributes.CssStyle.Item("cursor") = "hand"
                ibtnConfirm.Enabled = True
                ibtnConfirm.ImageUrl = "~/Images/button/btn_confirm.png"

                Me.pnlAgreement.Visible = False

                Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT010001, Me)
                udtAuditLogEntry.WriteLog(LogID.LOG00011, "Force change password loaded", strLogUserID)
            ElseIf ViewState(VS_FirstChangePassword) = "R" Then

                Me.lblStatement.Text = Me.GetGlobalResourceObject("Text", "ResetPwdChgPwdStatement")

                ibtnConfirm.Attributes.CssStyle.Item("cursor") = "hand"
                ibtnConfirm.Enabled = True
                ibtnConfirm.ImageUrl = "~/Images/button/btn_confirm.png"

                Me.pnlAgreement.Visible = False

                Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT010001, Me)
                udtAuditLogEntry.WriteLog(LogID.LOG00022, "Reset Password Force change password loaded", strLogUserID)
            Else
                Me.lblStatement.Text = Me.GetGlobalResourceObject("Text", "1stLoginChgPwdStatement")

                ibtnConfirm.Attributes.CssStyle.Item("cursor") = "default"
                chkAccept.Attributes.Item("onclick") = "chkAccept_check()"

                ibtnConfirm.Attributes.CssStyle.Item("cursor") = "default"
                ibtnConfirm.Enabled = False
                ibtnConfirm.ImageUrl = "~/Images/button/btn_confirm_D.png"

                Me.pnlAgreement.Visible = True

                Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT010001, Me)
                udtAuditLogEntry.WriteLog(LogID.LOG00007, "First logon change password loaded", strLogUserID)
            End If


        End If

        ' CRE15-006 Rename of eHS [Start][Lawrence]
        lblAppEnvironment.Text = (New GeneralFunction).getSystemParameter("AppEnvironment")

        If lblAppEnvironment.Text.ToLower = "production" Then lblAppEnvironment.Text = String.Empty
        ' CRE15-006 Rename of eHS [End][Lawrence]

        If Me.pnlAgreement.Visible Then
            If chkAccept.Checked Then
                ibtnConfirm.Attributes.CssStyle.Item("cursor") = "hand"
                ibtnConfirm.Enabled = True
                ibtnConfirm.ImageUrl = "~/Images/button/btn_confirm.png"
            Else
                ibtnConfirm.Attributes.CssStyle.Item("cursor") = "default"
                ibtnConfirm.Enabled = False
                ibtnConfirm.ImageUrl = "~/Images/button/btn_confirm_D.png"
            End If
        End If


        Dim strvalue1 As String = String.Empty
        Dim strvalue2 As String = String.Empty
        Dim udtcomfunct As New Common.ComFunction.GeneralFunction

        udtcomfunct.getSystemParameter("PasswordRuleNumber", strvalue1, strvalue2)

        Me.txtNewPassword.Attributes.Add("onKeyUp", "checkPassword(this.value,'" & CInt(strvalue1.Trim) & "', '" & CInt(strvalue2.Trim) & "', 'strength1','strength2','strength3','progressBar', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthPoor") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthModerate") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthStrong") & "','direction2','direction1');")
        'Me.txtNewPassword.Attributes.Add("onkeyup", "checkPassword(this.value,'strength1','strength2','strength3','progressBar', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthPoor") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthModerate") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthStrong") & "','direction2','direction1');")

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

    End Sub

    Private Sub ResetAlertImage()
        Me.imgOldPasswordAlert.Visible = False
        Me.imgNewPasswordAlert.Visible = False
        Me.imgNewPasswordConfirmAlert.Visible = False
    End Sub

    Private Sub ibtnExit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnExit.Click
        Response.Redirect("~/login.aspx")
    End Sub

    Private Sub ibtnConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnConfirm.Click

        Dim udtAuditLogEntry As AuditLogEntry
        Dim strLogUserID As String = ""
        Dim strLogID As String
        Dim strSuccessLogID As String
        Dim strFailLogID As String
        If ViewState(VS_FirstChangePassword) = "R" Then
            strLogID = LogID.LOG00018
            strSuccessLogID = LogID.LOG00019
            strFailLogID = LogID.LOG00020
        ElseIf ViewState(VS_FirstChangePassword) = "N" Then
            strLogID = LogID.LOG00008
            strSuccessLogID = LogID.LOG00009
            strFailLogID = LogID.LOG00010
        Else
            strLogID = LogID.LOG00012
            strSuccessLogID = LogID.LOG00013
            strFailLogID = LogID.LOG00014
        End If
        udtAuditLogEntry = New AuditLogEntry(FunctCode.FUNT010001, Me)

        Dim strAuditLogDesc As String = ""
        Dim strMsgBoxDesc As String = "ValidationFail"

        If Me.pnlAgreement.Visible AndAlso Me.chkAccept.Checked = False Then
            Exit Sub
        End If

        ResetAlertImage()

        Dim udtHCVUUserBLL As New HCVUUserBLL
        Dim dtHCVUUser As DataTable = Nothing
        Dim udtHCVUUser As HCVUUserModel
        Dim udtValidator As New Validator

        udtHCVUUser = CType(Session(SESS_ChangePasswordHCVUUser), HCVUUserModel)
        dtHCVUUser = udtHCVUUserBLL.GetHCVUUserForLogin(udtHCVUUser.UserID)
        strLogUserID = udtHCVUUser.UserID.Trim

        udtAuditLogEntry.AddDescripton("User_ID", udtHCVUUser.UserID.Trim)

        If ViewState(VS_FirstChangePassword) = "R" Then
            strAuditLogDesc = "Reset password Force change password"
        ElseIf ViewState(VS_FirstChangePassword) = "N" Then
            strAuditLogDesc = "Force change password"
        Else
            strAuditLogDesc = "First logon change password"
        End If
        udtAuditLogEntry.WriteStartLog(strLogID, strAuditLogDesc, strLogUserID)

        If udtValidator.IsEmpty(Me.txtOldPassword.Text) Then
            ' Please input Login ID
            Me.udcMessageBox.AddMessage("990000", "E", "00048")
            Me.imgOldPasswordAlert.Visible = True

        End If
        If udtValidator.IsEmpty(Me.txtNewPassword.Text) Then
            ' Please input Password
            Me.udcMessageBox.AddMessage("990000", "E", "00049")
            Me.imgNewPasswordAlert.Visible = True

        End If
        If udtValidator.IsEmpty(Me.txtNewPasswordConfirm.Text) Then
            Me.udcMessageBox.AddMessage("990000", "E", "00050")
            Me.imgNewPasswordConfirmAlert.Visible = True
        End If

        If Me.udcMessageBox.GetCodeTable.Rows.Count = 0 Then

            If Me.txtOldPassword.Text = Me.txtNewPassword.Text Then
                ' New Password cannot be same as Old Password
                Me.udcMessageBox.AddMessage("990000", "E", "00052")
                Me.imgNewPasswordAlert.Visible = True
            Else
                If Me.txtNewPassword.Text <> Me.txtNewPasswordConfirm.Text Then
                    ' New Password is not same as Confirm New Password
                    Me.udcMessageBox.AddMessage("990000", "E", "00054")
                    Me.imgNewPasswordConfirmAlert.Visible = True
                Else
                    If Not udtValidator.ValidatePassword(Me.txtNewPassword.Text) Then
                        ' New Password does not match the password criteria
                        Me.udcMessageBox.AddMessage("990000", "E", "00053")
                        Me.imgNewPasswordAlert.Visible = True
                    End If
                End If

            End If

            If Me.udcMessageBox.GetCodeTable.Rows.Count = 0 Then
                ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
                'If Encrypt.MD5hash(Me.txtOldPassword.Text) <> CStr(dtHCVUUser.Rows(0).Item("User_Password")) Then
                Dim udtVerifyPassword As VerifyPasswordResultModel = VerifyPassword(EnumPlatformType.VU, dtHCVUUser, Me.txtOldPassword.Text)
                If udtVerifyPassword.VerifyResult = EnumVerifyPasswordResult.Incorrect Then
                    ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---
                    Me.udcMessageBox.AddMessage("990000", "E", "00051")
                End If
            End If

            If Me.udcMessageBox.GetCodeTable.Rows.Count = 0 Then
                ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
                Dim udtPassword As HashModel = Hash(Me.txtNewPassword.Text.Trim)
                ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---

                Dim db As New Database
                Try
                    db.BeginTransaction()
                    ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
                    'udtHCVUUserBLL.UpdatePassword(udtHCVUUser.UserID, Encrypt.MD5hash(Me.txtNewPassword.Text), udtHCVUUser.UserID, udtHCVUUser.TSMP, db)
                    udtHCVUUserBLL.UpdatePassword(udtHCVUUser.UserID, udtPassword, udtHCVUUser.UserID, udtHCVUUser.TSMP, db)
                    ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---
                    udtHCVUUserBLL.UpdateLoginDtm(udtHCVUUser.UserID, "S", db)
                    udtHCVUUserBLL.UpdateForcePwdChange(udtHCVUUser.UserID, "N", db)
                    db.CommitTransaction()
                Catch eSQL As SqlClient.SqlException
                    db.RollBackTranscation()
                    If eSQL.Number = 50000 Then
                        Dim strmsg As String
                        strmsg = eSQL.Message
                        Dim udtSytemMessage As Common.ComObject.SystemMessage
                        udtSytemMessage = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                        Me.udcMessageBox.AddMessage(udtSytemMessage)
                        strMsgBoxDesc = "UpdateFail"
                    Else
                        Throw eSQL
                    End If
                Catch ex As Exception
                    db.RollBackTranscation()
                    Throw ex
                Finally
                    If Not db Is Nothing Then db.Dispose()
                End Try

                If Me.udcMessageBox.GetCodeTable.Rows.Count = 0 Then
                    dtHCVUUser = udtHCVUUserBLL.GetHCVUUserForLogin(udtHCVUUser.UserID)
                    If dtHCVUUser.Rows.Count = 1 Then
                        Dim drHCVUUser As DataRow
                        drHCVUUser = dtHCVUUser.Rows(0)
                        udtHCVUUser.LastPwdChangeDtm = IIf(drHCVUUser.Item("Last_Pwd_Change_Dtm") Is DBNull.Value, Nothing, drHCVUUser.Item("Last_Pwd_Change_Dtm"))
                        udtHCVUUser.LastPwdChangeDuration = IIf(drHCVUUser.Item("Last_Pwd_Change_Duration") Is DBNull.Value, Nothing, drHCVUUser.Item("Last_Pwd_Change_Duration"))
                    End If
                    Session.Remove(SESS_ChangePasswordHCVUUser)
                    Session.Remove(SESS_FirstChangePassword)
                    udtHCVUUserBLL.SaveToSession(udtHCVUUser)
                    udtAuditLogEntry.AddDescripton("User_ID", udtHCVUUser.UserID)

                    If ViewState(VS_FirstChangePassword) = "R" Then
                        udtAuditLogEntry.WriteEndLog(strSuccessLogID, "Reset password Force change password successful")
                    ElseIf ViewState(VS_FirstChangePassword) = "N" Then
                        udtAuditLogEntry.WriteEndLog(strSuccessLogID, "Force change password successful")
                    Else
                        udtAuditLogEntry.WriteEndLog(strSuccessLogID, "First logon change password successful")
                    End If
                    Response.Redirect("~/Home/home.aspx")
                End If
            End If
        End If

        If ViewState(VS_FirstChangePassword) = "R" Then
            udtAuditLogEntry.AddDescripton("User_ID", strLogUserID)
            Me.udcMessageBox.BuildMessageBox(strMsgBoxDesc, udtAuditLogEntry, "Reset password Force change password fail", strFailLogID, strLogUserID)
        ElseIf ViewState(VS_FirstChangePassword) = "N" Then
            udtAuditLogEntry.AddDescripton("User_ID", strLogUserID)
            Me.udcMessageBox.BuildMessageBox(strMsgBoxDesc, udtAuditLogEntry, "Force change password fail", strFailLogID, strLogUserID)
        Else
            udtAuditLogEntry.AddDescripton("User_ID", strLogUserID)
            Me.udcMessageBox.BuildMessageBox(strMsgBoxDesc, udtAuditLogEntry, "First logon change password fail", strFailLogID, strLogUserID)
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

End Class