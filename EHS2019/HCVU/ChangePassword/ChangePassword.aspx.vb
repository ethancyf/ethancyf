Imports Common.Component.HCVUUser
Imports Common.Validation
Imports Common.ComObject
Imports Common.DataAccess
Imports Common.Encryption
Imports Common.ComFunction
Imports Common.Component
Imports Common.ComFunction.AccountSecurity

Partial Public Class ChangePassword
    Inherits BasePage

    Private Const SESS_CHANGEPASSWORD_HCVUUSER As String = "SESS_CHANGEPASSWORD_HCVUUSER"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        FunctionCode = FunctCode.FUNT010801

        If Not IsPostBack Then

            Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT010801, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, "Change Password loaded")

            Dim udtHCVUUserBLL As New HCVUUserBLL
            Dim udtHCVUUser As HCVUUserModel

            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser()
            Me.lblUserName.Text = udtHCVUUser.UserID.ToUpper()

            Dim udtUpdUser As HCVUUserModel
            udtUpdUser = udtHCVUUserBLL.GetHCVUUserInfo(udtHCVUUser.UserID)
            Session(SESS_CHANGEPASSWORD_HCVUUSER) = udtUpdUser

            ScriptManager1.SetFocus(Me.txtOldPassword)

        End If

        Dim strvalue1 As String = String.Empty
        Dim strvalue2 As String = String.Empty
        Dim udtcomfunct As New Common.ComFunction.GeneralFunction

        udtcomfunct.getSystemParameter("PasswordRuleNumber", strvalue1, strvalue2)

        Me.txtNewPassword.Attributes.Add("onKeyUp", "checkPassword(this.value,'" & CInt(strvalue1.Trim) & "', '" & CInt(strvalue2.Trim) & "', 'strength1','strength2','strength3','progressBar', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthPoor") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthModerate") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthStrong") & "','direction2','direction1');")
        'Me.txtNewPassword.Attributes.Add("onkeyup", "checkPassword(this.value,'strength1','strength2','strength3','progressBar', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthPoor") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthModerate") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthStrong") & "','direction2','direction1');")

    End Sub

    Private Sub ResetAlertImage()
        Me.imgOldPasswordAlert.Visible = False
        Me.imgNewPasswordAlert.Visible = False
        Me.imgNewPasswordConfirmAlert.Visible = False
    End Sub

    Private Sub ibtnConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnConfirm.Click
        Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT010801, Me)
        Me.udcInfoMessageBox.Visible = False
        Me.udcMessageBox.Visible = False
        ResetAlertImage()

        Dim udtHCVUUserBLL As New HCVUUserBLL
        Dim dtHCVUUser As DataTable = Nothing
        Dim udtHCVUUser As HCVUUserModel
        Dim udtValidator As New Validator
        Dim strMsgBoxDesc As String = "ValidationFail"
        Dim strUserID As String = String.Empty

        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser()
        dtHCVUUser = udtHCVUUserBLL.GetHCVUUserForLogin(udtHCVUUser.UserID)

        strUserID = udtHCVUUser.UserID
        udtAuditLogEntry.AddDescripton("User_ID", udtHCVUUser.UserID)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Change Password")

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

            Dim udtUpdUser As HCVUUserModel
            udtUpdUser = CType(Session(SESS_CHANGEPASSWORD_HCVUUSER), HCVUUserModel)

            If Me.udcMessageBox.GetCodeTable.Rows.Count = 0 Then
                ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
                Dim udtPassword As HashModel = Hash(Me.txtNewPassword.Text.Trim)
                ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---

                Dim db As New Database
                Try
                    db.BeginTransaction()
                    ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
                    'udtHCVUUserBLL.UpdatePassword(udtHCVUUser.UserID, Encrypt.MD5hash(Me.txtNewPassword.Text), udtHCVUUser.UserID, udtUpdUser.TSMP, db)
                    udtHCVUUserBLL.UpdatePassword(udtHCVUUser.UserID, udtPassword, udtHCVUUser.UserID, udtUpdUser.TSMP, db)
                    ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---

                    'udtHCVUUserBLL.UpdateLoginDtm(udtHCVUUser.UserID, "S", db)
                    db.CommitTransaction()
                Catch eSQL As SqlClient.SqlException
                    db.RollBackTranscation()
                    If eSQL.Number = 50000 Then
                        Dim strmsg As String
                        strmsg = eSQL.Message
                        Dim udtDBSytemMessage As Common.ComObject.SystemMessage = Nothing
                        udtDBSytemMessage = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                        Me.udcMessageBox.AddMessage(udtDBSytemMessage)
                        'udcMessageBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, "Change Password failed")
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
                    udtHCVUUserBLL.SaveToSession(udtHCVUUser)
                    udcInfoMessageBox.AddMessage("010801", "I", "00001")
                    udcInfoMessageBox.BuildMessageBox()
                    pnlChangePasswordContent.Visible = False
                    Me.ibtnConfirm.Visible = False
                    Me.ibtnBack.Visible = True

                    udtAuditLogEntry.AddDescripton("User_ID", strUserID)
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Change Password successful")
                End If
            End If
        End If

        udtAuditLogEntry.AddDescripton("User_ID", strUserID)
        Me.udcMessageBox.BuildMessageBox(strMsgBoxDesc, udtAuditLogEntry, LogID.LOG00003, "Change Password fail")

    End Sub

    Private Sub ibtnBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnBack.Click
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtHCVUUserBLL As New HCVUUserBLL
        Dim udtHCVUUser As HCVUUserModel = udtHCVUUserBLL.GetHCVUUser()
     
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("User_ID", udtHCVUUser.UserID)
        udtAuditLogEntry.WriteLog(LogID.LOG00004, "Back Click")
        ' CRE11-021 log the missed essential information [End]

        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
        Dim udtMenuBLL As New Component.Menu.MenuBLL
        Dim strEnquiryCallCentre_FuncCode As String = FunctCode.FUNT010309

        ' eHealth Account Enquiry (Call Centre) as default page
        If Me.SubPlatform = EnumHCVUSubPlatform.CC AndAlso
            udtHCVUUser.AccessRightCollection.Item(strEnquiryCallCentre_FuncCode).Allow() Then
            RedirectHandler.ToURL(udtMenuBLL.GetURLByFunctionCode(strEnquiryCallCentre_FuncCode))
        Else
            RedirectHandler.ToURL("~/Home/home.aspx")
        End If
        ' CRE19-026 (HCVS hotline service) [End][Winnie]
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