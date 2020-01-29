Imports Common.ComObject
Imports Common.Component.UserAC
Imports Common.Component
Imports Common.Component.ServiceProvider
Imports Common.Component.DataEntryUser
Imports Common.Validation
Imports Common.Encryption
Imports Common.ComFunction
Imports HCSP.BLL
Imports Common.ComFunction.AccountSecurity

Partial Public Class loginchangepassword1
    Inherits TextOnlyBasePage

    Public Class AuditLogDescription
        Public Const LoadChangePassword As String = "Login Change Password (Text only version)"
        Public Const ConfirmChangePassword As String = "Confirm Change Password "
        Public Const ChangePasswordSuccess As String = "Change Password Success"
        Public Const ChangePasswordFail As String = "Change Password Fail"
        'Public Const ExitChangePassword As String = "Exit Change Password"
    End Class

    Private SESS_FirstChangePassword As String = "FirstChangePassword"
    Private VS_FirstChangePassword As String = "FirstChangePassword"
    Private Const SESS_ChangePasswordUserAC As String = "ChangePasswordUserAC"
    Dim udtAuditLogEntry As AuditLogEntry
    Dim strFuncCode As String = Common.Component.FunctCode.FUNT020004

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        Dim strLogSPID As String = ""
        Dim strLogDataEntryAccount As String = ""

        If Not IsPostBack Then
            Dim selectedValue As String
            selectedValue = Session("language")

            'Me.ScriptManager1.SetFocus(Me.txtOldPassword)
            ResetAlertLabel()

            btnBack.Attributes.Add("OnClick", "javascript:window.opener='X';window.open('','_parent','');window.close(); return false;")

            If Not Session(SESS_FirstChangePassword) Is Nothing Then
                ViewState(VS_FirstChangePassword) = Session(SESS_FirstChangePassword)
            Else
                Throw New Exception("Session Expired!")
            End If

            Dim udtUserAC As UserACModel
            Dim udtUserACBLL As New UserACBLL

            udtUserAC = CType(Session(SESS_ChangePasswordUserAC), UserACModel)

            If Session("FirstChangePassword") = "N" Then
                'ViewState("FirstChangePassword") = "N"
                Dim udtGeneralFunction As New GeneralFunction
                Dim strChgPwdDay As String = ""

                If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                    udtGeneralFunction.getSystemParameter("DaysOfChangePasswordHCSPUser", strChgPwdDay, String.Empty)
                Else
                    udtGeneralFunction.getSystemParameter("DaysOfChangePasswordDataEntry", strChgPwdDay, String.Empty)
                End If

                Me.lblStatement.Text = Me.GetGlobalResourceObject("Text", "60DaysChgPwdStatement").Replace("%s", strChgPwdDay)
                btnLogin.Enabled = True
                Me.pnlAgreement.Visible = False
            Else
                'ViewState("FirstChangePassword") = "Y"
                Me.lblStatement.Text = Me.GetGlobalResourceObject("Text", "1stLoginChgPwdStatement")
                'chkAccept.Attributes.Item("onclick") = "chkAccept_check()"
                btnLogin.Enabled = True
                Me.pnlAgreement.Visible = True
            End If

            If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                Dim udtServiceProvider As ServiceProviderModel
                udtServiceProvider = CType(udtUserAC, ServiceProviderModel)
                Me.lblUsername.Text = udtServiceProvider.AliasAccount

                If udtServiceProvider.AliasAccount = "" Then
                    Me.lblUsername.Text = "--"
                Else
                    Me.lblUsername.Text = udtServiceProvider.AliasAccount
                End If

                strLogSPID = udtServiceProvider.SPID
                Me.lblSPID.Text = udtServiceProvider.SPID
                Me.pnlSPID.Visible = True
                Me.lblWebPasswordTips1.Text = Me.GetGlobalResourceObject("Text", "WebPasswordTips1")
            Else
                Dim udtDataEntryUser As DataEntryUserModel
                udtDataEntryUser = CType(udtUserAC, DataEntryUserModel)

                strLogSPID = udtDataEntryUser.SPID
                strLogDataEntryAccount = udtDataEntryUser.DataEntryAccount
                Me.lblUsername.Text = udtDataEntryUser.DataEntryAccount
                Me.lblSPID.Text = ""
                Me.pnlSPID.Visible = False
                Me.lblWebPasswordTips1.Text = Me.GetGlobalResourceObject("Text", "WebPasswordTips1-3Rule")
            End If

            'Log Page Load 
            Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode)
            Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00004, AuditLogDescription.LoadChangePassword, strLogSPID, strLogDataEntryAccount)
        End If

        SetLangage()
        ReRenderPage()

    End Sub

    Private Sub SetLangage()
        Dim selectedValue As String

        selectedValue = Session("language")

        Select Case selectedValue
            Case Common.Component.CultureLanguage.English
                lnkbtnEnglish.Enabled = False
                lnkbtnTradChinese.Enabled = True
            Case Common.Component.CultureLanguage.TradChinese
                lnkbtnEnglish.Enabled = True
                lnkbtnTradChinese.Enabled = False
            Case Else
                lnkbtnEnglish.Enabled = False
                lnkbtnTradChinese.Enabled = True
        End Select
    End Sub

    Protected Overrides Sub InitializeCulture()
        Dim selectedValue As String = String.Empty

        If Not Request(PostBackEventTarget) Is Nothing Then
            'Dim controlID As String = Request.Form(PostBackEventTarget)
            Dim controlID As String = Page.Request.Params.Get(PostBackEventTarget)
            If controlID.Equals(_SelectTradChinese) Then
                selectedValue = Common.Component.CultureLanguage.TradChinese
                Session("language") = selectedValue
            ElseIf controlID.Equals(_SelectEnglish) Then
                selectedValue = Common.Component.CultureLanguage.English
                Session("language") = selectedValue
            End If
        End If

        'Session("language") = 0
        MyBase.InitializeCulture()

    End Sub

    Private Sub ReRenderPage()
        'If ViewState("FirstChangePassword") = "N" Then
        '    Me.lblStatement.Text = Me.GetGlobalResourceObject("Text", "60DaysChgPwdStatement")
        'Else
        '    Me.lblStatement.Text = Me.GetGlobalResourceObject("Text", "1stLoginChgPwdStatement")
        'End If
        Me.PageTitle.Text = Me.GetGlobalResourceObject("Title", "ChangePassword")
    End Sub

    Private Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogin.Click

        Dim udtUserAC As UserACModel
        Dim udtUserACBLL As New UserACBLL
        Dim udtValidator As New Validator
        Dim udtChangePasswordBLL As New ChangePasswordBLL
        Dim strMsgBoxDesc As String = "ValidationFail"
        Dim blnAcceptErr As Boolean = False
        Dim strLogSPID As String = ""
        Dim strLogDataEntryAccount As String = ""

        udtUserAC = CType(Session(SESS_ChangePasswordUserAC), UserACModel)

        Dim dtUserAC As DataTable = Nothing

        ResetAlertLabel()
        'Start  Log Change passord
        Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode, Me)
        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            Dim udtServiceProvider As ServiceProviderModel
            udtServiceProvider = CType(udtUserAC, ServiceProviderModel)
            dtUserAC = udtUserACBLL.GetUserACForLogin(udtServiceProvider.SPID, "", udtUserAC.UserType)
            Me.udtAuditLogEntry.AddDescripton("User Account type", "Service Provider")
            Me.udtAuditLogEntry.AddDescripton("SPID/User Name", udtServiceProvider.SPID)
            strLogSPID = udtServiceProvider.SPID
        Else
            Dim udtDataEntryUser As DataEntryUserModel
            udtDataEntryUser = CType(udtUserAC, DataEntryUserModel)
            dtUserAC = udtUserACBLL.GetUserACForLogin(udtDataEntryUser.DataEntryAccount, udtDataEntryUser.SPID, udtUserAC.UserType)
            Me.udtAuditLogEntry.AddDescripton("User Account type", "Data Entry")
            Me.udtAuditLogEntry.AddDescripton("SPID/Username", udtDataEntryUser.SPID)
            Me.udtAuditLogEntry.AddDescripton("Data Entry User name", udtDataEntryUser.DataEntryAccount)
            strLogSPID = udtDataEntryUser.SPID
            strLogDataEntryAccount = udtDataEntryUser.DataEntryAccount
        End If
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00005, AuditLogDescription.ConfirmChangePassword, strLogSPID, strLogDataEntryAccount)
        If udtValidator.IsEmpty(Me.txtOldPassword.Text) Then
            ' Please input Login ID
            Me.udcTextOnlyMessageBox.AddMessage("990000", "E", "00048")
            'Me.imgOldPasswordAlert.Visible = True
            Me.lblOldPasswordAlert.Visible = True

        End If
        If udtValidator.IsEmpty(Me.txtNewPassword.Text) Then
            ' Please input Password
            Me.udcTextOnlyMessageBox.AddMessage("990000", "E", "00049")
            Me.lblNewPasswordAlert.Visible = True

        End If
        If udtValidator.IsEmpty(Me.txtNewPasswordConfirm.Text) Then
            Me.udcTextOnlyMessageBox.AddMessage("990000", "E", "00050")
            Me.lblNewPasswordConfirmAlert.Visible = True
        End If

        If Me.udcTextOnlyMessageBox.GetCodeTable.Rows.Count = 0 Then

            If Me.txtOldPassword.Text = Me.txtNewPassword.Text Then
                ' New Password cannot be same as Old Password
                Me.udcTextOnlyMessageBox.AddMessage("990000", "E", "00052")
                Me.lblNewPasswordAlert.Visible = True
            Else
                If Me.txtNewPassword.Text <> Me.txtNewPasswordConfirm.Text Then
                    ' New Password is not same as Confirm New Password
                    Me.udcTextOnlyMessageBox.AddMessage("990000", "E", "00054")
                    Me.lblNewPasswordConfirmAlert.Visible = True
                Else
                    If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                        If Not udtValidator.ValidatePassword(Me.txtNewPassword.Text) Then
                            ' New Password does not match the password criteria
                            Me.udcTextOnlyMessageBox.AddMessage("990000", "E", "00053")
                            Me.lblNewPasswordAlert.Visible = True
                        End If
                    Else
                        If Not udtValidator.ValidatePassword(Me.txtNewPassword.Text, False) Then
                            ' New Password does not match the password criteria
                            Me.udcTextOnlyMessageBox.AddMessage("990000", "E", "00053")
                            Me.lblNewPasswordAlert.Visible = True
                        End If
                    End If

                End If

            End If


            If Me.udcTextOnlyMessageBox.GetCodeTable.Rows.Count = 0 Then
                ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
                'If Encrypt.MD5hash(Me.txtOldPassword.Text) <> CStr(dtUserAC.Rows(0).Item("User_Password")) Then
                Dim udtType As EnumPlatformType
                If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                    udtType = EnumPlatformType.SP
                Else
                    udtType = EnumPlatformType.DE
                End If

                Dim udtVerifyPassword As VerifyPasswordResultModel = VerifyPassword(udtType, dtUserAC, Me.txtOldPassword.Text)
                If udtVerifyPassword.VerifyResult = EnumVerifyPasswordResult.Incorrect Then
                    ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---
                    Me.udcTextOnlyMessageBox.AddMessage("990000", "E", "00051")
                End If
            End If

            If Me.udcTextOnlyMessageBox.GetCodeTable.Rows.Count = 0 Then
                If Me.pnlAgreement.Visible AndAlso Me.chkAccept.Checked = False Then
                    Me.udcTextOnlyMessageBox.AddMessage("020004", "E", "00002")
                    Me.lblAcceptAlert.Visible = True
                End If
            End If

            If Me.udcTextOnlyMessageBox.GetCodeTable.Rows.Count = 0 Then
                ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
                'Dim strPassword As String = Encrypt.MD5hash(Me.txtNewPassword.Text.Trim)
                Dim udtPasswordModel As HashModel = Hash(Me.txtNewPassword.Text.Trim)
                ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---
                Try
                    ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
                    'udtChangePasswordBLL.UpdatePassword(udtUserAC, strPassword)
                    udtChangePasswordBLL.UpdatePassword(udtUserAC, udtPasswordModel)
                    ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---
                Catch eSQL As SqlClient.SqlException
                    If eSQL.Number = 50000 Then
                        Dim strmsg As String
                        strmsg = eSQL.Message
                        Dim udtSytemMessage As Common.ComObject.SystemMessage
                        udtSytemMessage = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                        Me.udcTextOnlyMessageBox.AddMessage(udtSytemMessage)
                        strMsgBoxDesc = "UpdateFail"
                    Else
                        Throw eSQL
                    End If
                Catch ex As Exception
                    Throw ex

                End Try
                'udtAuditLogEntry.WriteLog("010001", "00001", Nothing, "Login Success")
                'Response.Redirect("~/Home/home.aspx")

                If udcTextOnlyMessageBox.GetCodeTable.Rows.Count = 0 Then
                    If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                        Dim udtServiceProvider As ServiceProviderModel
                        udtServiceProvider = CType(udtUserAC, ServiceProviderModel)
                        dtUserAC = udtUserACBLL.GetUserACForLogin(udtServiceProvider.SPID, "", udtUserAC.UserType)
                        Dim drUserAC As DataRow
                        drUserAC = dtUserAC.Rows(0)
                        udtServiceProvider.LastPwdChangeDtm = IIf(drUserAC.Item("Last_Pwd_Change_Dtm") Is DBNull.Value, Nothing, drUserAC.Item("Last_Pwd_Change_Dtm"))
                        udtServiceProvider.LastPwdChangeDuration = IIf(drUserAC.Item("Last_Pwd_Change_Duration") Is DBNull.Value, Nothing, drUserAC.Item("Last_Pwd_Change_Duration"))
                        Session.Remove(SESS_FirstChangePassword)
                        Session.Remove(SESS_ChangePasswordUserAC)
                        udtUserACBLL.SaveToSession(udtServiceProvider)

                        Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode)
                        Me.udtAuditLogEntry.AddDescripton("User Account type", "Service Provider")
                        Me.udtAuditLogEntry.AddDescripton("SPID/User Name", udtServiceProvider.SPID)
                        Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00006, AuditLogDescription.ChangePasswordSuccess, strLogSPID, strLogDataEntryAccount)
                    Else
                        Dim udtDataEntryUser As DataEntryUserModel
                        udtDataEntryUser = CType(udtUserAC, DataEntryUserModel)
                        dtUserAC = udtUserACBLL.GetUserACForLogin(udtDataEntryUser.DataEntryAccount, udtDataEntryUser.SPID, udtUserAC.UserType)
                        Dim drUserAC As DataRow
                        drUserAC = dtUserAC.Rows(0)
                        udtDataEntryUser.LastPwdChangeDtm = IIf(drUserAC.Item("Last_Pwd_Change_Dtm") Is DBNull.Value, Nothing, drUserAC.Item("Last_Pwd_Change_Dtm"))
                        udtDataEntryUser.LastPwdChangeDuration = IIf(drUserAC.Item("Last_Pwd_Change_Duration") Is DBNull.Value, Nothing, drUserAC.Item("Last_Pwd_Change_Duration"))
                        Session.Remove(SESS_FirstChangePassword)
                        Session.Remove(SESS_ChangePasswordUserAC)
                        udtUserACBLL.SaveToSession(udtDataEntryUser)

                        Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode)
                        Me.udtAuditLogEntry.AddDescripton("User Account type", "Data Entry")
                        Me.udtAuditLogEntry.AddDescripton("SPID/Username", udtDataEntryUser.SPID)
                        Me.udtAuditLogEntry.AddDescripton("Data Entry User name", udtDataEntryUser.DataEntryAccount)
                        Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00006, AuditLogDescription.ChangePasswordSuccess, strLogSPID, strLogDataEntryAccount)
                    End If
                    'If ViewState(VS_FirstChangePassword) = "N" Then
                    'udtAuditLogEntry.WriteEndLog(strSuccessLogID, strAuditLogDesc & " successful")
                    'Else
                    '    udtAuditLogEntry.WriteEndLog(strSuccessLogID, strAuditLogDesc & " successful")
                    'End If
                    ' Response.Redirect("~/text/claimvoucherV2.aspx")

                    '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

                    RedirectHandler.ToURL(ClaimVoucherMaster.ChildPage.EHSClaim)

                    '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End
                End If

            End If
        End If


        Me.udcTextOnlyMessageBox.BuildMessageBox("ValidationFail", Me.udtAuditLogEntry, AuditLogDescription.ChangePasswordFail, Common.Component.LogID.LOG00007, strLogSPID, strLogDataEntryAccount)

    End Sub

    Private Sub ResetAlertLabel()
        Me.lblOldPasswordAlert.Visible = False
        Me.lblNewPasswordAlert.Visible = False
        Me.lblNewPasswordConfirmAlert.Visible = False
        Me.lblAcceptAlert.Visible = False
    End Sub

    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click

        ' (CRE11-004)
        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC
        Dim strLogSPID As String = ""
        Dim strLogDataEntryAccount As String = ""
        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry("020004", Me)

        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            Dim udtServiceProvider As ServiceProviderModel
            udtServiceProvider = CType(udtUserAC, ServiceProviderModel)
            strLogSPID = udtServiceProvider.SPID
        Else
            Dim udtDataEntryUser As DataEntryUserModel
            udtDataEntryUser = CType(udtUserAC, DataEntryUserModel)
            strLogSPID = udtDataEntryUser.SPID
            strLogDataEntryAccount = udtDataEntryUser.DataEntryAccount
        End If
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00009, "Exit Change Password", strLogSPID, strLogDataEntryAccount)

        'Response.Redirect("~/text/login.aspx")
        Response.Redirect(ClaimVoucherMaster.ChildPage.Login)
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