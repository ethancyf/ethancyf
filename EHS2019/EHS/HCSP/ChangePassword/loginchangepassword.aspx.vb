Imports Common.Component.UserAC
Imports Common.Component
Imports Common.Component.ServiceProvider
Imports Common.Component.DataEntryUser
Imports Common.Validation
Imports Common.Encryption
Imports Common.ComFunction
Imports HCSP.BLL
Imports Common.ComObject
Imports Common.ComFunction.AccountSecurity

Partial Public Class loginchangepassword
    Inherits BasePage

    Private SESS_FirstChangePassword As String = "FirstChangePassword"
    Private VS_FirstChangePassword As String = "FirstChangePassword"
    'Private VS_SPID As String = "SPID"
    'Private VS_DataEntryAccount As String = "DataEntryAccount"
    'Private VS_UserType As String = "UserType"
    Private Const SESS_ChangePasswordUserAC As String = "ChangePasswordUserAC"
    Private udcGeneralF As New Common.ComFunction.GeneralFunction

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        If Not IsPostBack Then

            Dim selectedValue As String
            selectedValue = Session("language")

            Dim strLogSPID As String = ""
            Dim strLogDataEntryAccount As String = Nothing

            Dim strLogID As String
            Dim strAuditLogDesc As String

            Dim udtScriptManager As ScriptManager

            udtScriptManager = CType(Me.Master.FindControl("ScriptManager1"), ScriptManager)

            udtScriptManager.SetFocus(Me.txtOldPassword)

            ResetAlertImage()

            'ibtnExit.Attributes.Add("OnClick", "javascript:window.opener='X';window.open('','_parent','');window.close(); return false;")

            Dim udtUserAC As UserACModel
            Dim udtUserACBLL As New UserACBLL

            udtUserAC = CType(Session(SESS_ChangePasswordUserAC), UserACModel)

            If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                Dim udtServiceProvider As ServiceProviderModel
                udtServiceProvider = CType(udtUserAC, ServiceProviderModel)
                strLogSPID = udtServiceProvider.SPID.Trim()
                strLogDataEntryAccount = Nothing
            Else
                Dim udtDataEntryUser As DataEntryUserModel
                udtDataEntryUser = CType(udtUserAC, DataEntryUserModel)
                strLogSPID = udtDataEntryUser.SPID.Trim
                strLogDataEntryAccount = udtDataEntryUser.DataEntryAccount.Trim
            End If
            ViewState("AuditLogSPID") = strLogSPID
            ViewState("AuditDataEntryAccount") = strLogDataEntryAccount

            If Not Session(SESS_FirstChangePassword) Is Nothing Then
                ViewState(VS_FirstChangePassword) = Session(SESS_FirstChangePassword)
            Else
                Throw New Exception("Session Expired!")
            End If
            'Session.Remove(SESS_FirstChangePassword)

            If ViewState(VS_FirstChangePassword) = "N" Then
                'ViewState("FirstChangePassword") = "N"

                Dim udtGeneralFunction As New GeneralFunction
                Dim strChgPwdDay As String = ""

                If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                    udtGeneralFunction.getSystemParameter("DaysOfChangePasswordHCSPUser", strChgPwdDay, String.Empty)
                Else
                    udtGeneralFunction.getSystemParameter("DaysOfChangePasswordDataEntry", strChgPwdDay, String.Empty)
                End If

                Me.lblStatement.Text = CStr(Me.GetGlobalResourceObject("Text", "60DaysChgPwdStatement")).Replace("%s", strChgPwdDay)

                ibtnConfirm.Attributes.CssStyle.Item("cursor") = "hand"
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'ibtnConfirm.Enabled = True
                'ibtnConfirm.ImageUrl = "~/Images/button/btn_confirm.png"
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]


                Me.pnlAgreement.Visible = False

                If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                    strLogID = LogID.LOG00012
                    strAuditLogDesc = "Servicer Provider "
                Else
                    strLogID = LogID.LOG00016
                    strAuditLogDesc = "Data Entry Account "
                End If

                Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT020001, Me)
                udtAuditLogEntry.WriteLog(strLogID, strAuditLogDesc & "Force change password loaded", strLogSPID, strLogDataEntryAccount)

            Else
                'ViewState("FirstChangePassword") = "Y"
                Me.lblStatement.Text = Me.GetGlobalResourceObject("Text", "1stLoginChgPwdStatement")

                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'chkAccept.Attributes.Item("onclick") = "chkAccept_check(document.getElementById('" + Me.chkAccept.ClientID + "'),document.getElementById('" + Me.ibtnConfirm.ClientID + "'))"
                chkAccept.Attributes.Item("onclick") = "chkAccept_check(document.getElementById('" + Me.chkAccept.ClientID + "'),document.getElementById('" + Me.tdConfirm.ClientID + "'),document.getElementById('" + Me.tdDisableConfirm.ClientID + "'))"

                'ibtnConfirm.Attributes.CssStyle.Item("cursor") = "default"
                'ibtnConfirm.Enabled = False
                ibtnDisableConfirm.Attributes.CssStyle.Item("cursor") = "default"
                ibtnDisableConfirm.Enabled = False

                'ibtnConfirm.ImageUrl = "~/Images/button/btn_confirm_D.png"
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]



                Me.pnlAgreement.Visible = True

                strLogID = LogID.LOG00020
                strAuditLogDesc = "Data Entry Account "

                Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT020001, Me)
                udtAuditLogEntry.WriteLog(strLogID, strAuditLogDesc & "First logon change password loaded", strLogSPID, strLogDataEntryAccount)
            End If

            'Dim udtUserAC As UserACModel
            'Dim udtUserACBLL As New UserACBLL

            ''udtUserAC = UserACBLL.GetUserAC
            'udtUserAC = CType(Session(SESS_ChangePasswordUserAC), UserACModel)

            Dim strvalue1 As String = String.Empty
            Dim strvalue2 As String = String.Empty
            Dim udtcomfunct As New Common.ComFunction.GeneralFunction

            udtcomfunct.getSystemParameter("PasswordRuleNumber", strvalue1, strvalue2)

            If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                Dim udtServiceProvider As ServiceProviderModel
                udtServiceProvider = CType(udtUserAC, ServiceProviderModel)
                If udtServiceProvider.AliasAccount = "" Then
                    Me.lblUsername.Text = "--"
                Else
                    Me.lblUsername.Text = udtServiceProvider.AliasAccount
                End If
                Me.lblSPID.Text = udtServiceProvider.SPID
                Me.pnlSPID.Visible = True

                Me.lblWebPasswordTips1.Text = Me.GetGlobalResourceObject("Text", "WebPasswordTips1")

                Me.txtNewPassword.Attributes.Add("onKeyUp", "checkPassword(this.value,'" & CInt(strvalue1.Trim) & "', '" & CInt(strvalue2.Trim) & "', 'strength1','strength2','strength3','progressBar', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthPoor") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthModerate") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthStrong") & "','direction2','direction1');")
                'Me.txtNewPassword.Attributes.Add("onkeyup", "checkPassword(this.value,'strength1','strength2','strength3','progressBar', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthPoor") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthModerate") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthStrong") & "','direction2','direction1');")
            Else
                Dim udtDataEntryUser As DataEntryUserModel
                udtDataEntryUser = CType(udtUserAC, DataEntryUserModel)
                Me.lblUsername.Text = udtDataEntryUser.DataEntryAccount
                Me.lblSPID.Text = ""
                Me.pnlSPID.Visible = False

                Me.lblWebPasswordTips1.Text = Me.GetGlobalResourceObject("Text", "WebPasswordTips1-3Rule")

                Me.txtNewPassword.Attributes.Add("onKeyUp", "checkPassword(this.value,'" & CInt(strvalue2.Trim) & "', '" & CInt(strvalue2.Trim) & "', 'strength1','strength2','strength3','progressBar', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthPoor") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthModerate") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthStrong") & "','direction2','direction1');")
                'Me.txtNewPassword.Attributes.Add("onkeyup", "checkPassword(this.value,'strength1','strength2','strength3','progressBar', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthPoor") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthModerate") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthStrong") & "','direction2','direction1');")
            End If

        End If

        If Me.pnlAgreement.Visible Then
            If chkAccept.Checked Then
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'ibtnConfirm.Attributes.CssStyle.Item("cursor") = "hand"
                'ibtnConfirm.Enabled = True
                'ibtnConfirm.ImageUrl = "~/Images/button/btn_confirm.png"
                tdConfirm.Style.Add("display", "initial")
                tdDisableConfirm.Style.Add("display", "none")
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            Else
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'ibtnConfirm.Attributes.CssStyle.Item("cursor") = "default"
                'ibtnConfirm.Enabled = False
                'ibtnConfirm.ImageUrl = "~/Images/button/btn_confirm_D.png"
                tdConfirm.Style.Add("display", "none")
                tdDisableConfirm.Style.Add("display", "initial")
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            End If
        End If

        ReRenderPage()


    End Sub

    Private Sub ResetAlertImage()
        Me.imgOldPasswordAlert.Visible = False
        Me.imgNewPasswordAlert.Visible = False
        Me.imgNewPasswordConfirmAlert.Visible = False
    End Sub



    Sub ReRenderPage()
        Dim udtGeneralFunction As New GeneralFunction
        'Me.tblBanner.Style.Item("background-image") = "url(" + Me.GetGlobalResourceObject("ImageUrl", "Banner").ToString + ")"

        Dim udtUserAC As UserACModel
        udtUserAC = CType(Session(SESS_ChangePasswordUserAC), UserACModel)

        If ViewState("FirstChangePassword") = "N" Then
            Dim strChgPwdDay As String = ""

            If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                udtGeneralFunction.getSystemParameter("DaysOfChangePasswordHCSPUser", strChgPwdDay, String.Empty)
            Else
                udtGeneralFunction.getSystemParameter("DaysOfChangePasswordDataEntry", strChgPwdDay, String.Empty)
            End If

            Me.lblStatement.Text = CStr(Me.GetGlobalResourceObject("Text", "60DaysChgPwdStatement")).Replace("%s", strChgPwdDay)
            'Me.lblStatement.Text = Me.GetGlobalResourceObject("Text", "60DaysChgPwdStatement")
        Else
            Me.lblStatement.Text = Me.GetGlobalResourceObject("Text", "1stLoginChgPwdStatement")
        End If
        Me.ibtnConfirm.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmBtn")
        'Dim udtGeneralFunction As New GeneralFunction
        udtGeneralFunction.UpdateImageURL(Me.ibtnConfirm)
        'Me.PageTitle.Text = Me.GetGlobalResourceObject("Title", "ChangePassword")


        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            Me.lblWebPasswordTips1.Text = Me.GetGlobalResourceObject("Text", "WebPasswordTips1")
        Else
            Me.lblWebPasswordTips1.Text = Me.GetGlobalResourceObject("Text", "WebPasswordTips1-3Rule")
        End If
    End Sub

    Private Sub ibtnExit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnExit.Click
        Response.Redirect("~/login.aspx")
    End Sub

    Private Sub ibtnConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnConfirm.Click
        If Me.pnlAgreement.Visible AndAlso Me.chkAccept.Checked = False Then
            Exit Sub
        End If
        Dim strAuditLogDesc As String
        Dim strLogID As String
        Dim strSuccessLogID As String
        Dim strFailLogID As String

        ResetAlertImage()

        Dim udtUserAC As UserACModel
        Dim udtUserACBLL As New UserACBLL
        Dim udtValidator As New Validator
        Dim udtChangePasswordBLL As New ChangePasswordBLL
        Dim strMsgBoxDesc As String = "ValidationFail"

        'udtUserAC = UserACBLL.GetUserAC
        udtUserAC = CType(Session(SESS_ChangePasswordUserAC), UserACModel)

        Dim udtAuditLogEntry As AuditLogEntry
        If ViewState(VS_FirstChangePassword) = "N" Then
            If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                strLogID = LogID.LOG00013
                strSuccessLogID = LogID.LOG00014
                strFailLogID = LogID.LOG00015
                strAuditLogDesc = "Service Provider Force change password"
            Else
                strLogID = LogID.LOG00017
                strSuccessLogID = LogID.LOG00018
                strFailLogID = LogID.LOG00019
                strAuditLogDesc = "Data Entry Account Force change password"
            End If
            udtAuditLogEntry = New AuditLogEntry(FunctCode.FUNT020001, Me)
        Else
            strLogID = LogID.LOG00021
            strSuccessLogID = LogID.LOG00022
            strFailLogID = LogID.LOG00023
            strAuditLogDesc = "Data Entry Account First logon change password"
            udtAuditLogEntry = New AuditLogEntry(FunctCode.FUNT020001, Me)
        End If

        Dim dtUserAC As DataTable = Nothing

        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            Dim udtServiceProvider As ServiceProviderModel
            udtServiceProvider = CType(udtUserAC, ServiceProviderModel)
            udtAuditLogEntry.AddDescripton("SPID", udtServiceProvider.SPID)
            'udtAuditLogEntry.WriteStartLog(strLogID, strAuditLogDesc, ViewState("AuditLogSPID"), ViewState("AuditDataEntryAccount"))
            dtUserAC = udtUserACBLL.GetUserACForLogin(udtServiceProvider.SPID, "", udtUserAC.UserType)
        Else
            Dim udtDataEntryUser As DataEntryUserModel
            udtDataEntryUser = CType(udtUserAC, DataEntryUserModel)
            udtAuditLogEntry.AddDescripton("SPID", udtDataEntryUser.SPID)
            udtAuditLogEntry.AddDescripton("Data_Entry_Account", udtDataEntryUser.DataEntryAccount)
            'udtAuditLogEntry.WriteStartLog(strLogID, strAuditLogDesc, ViewState("AuditLogSPID"), ViewState("AuditDataEntryAccount"))
            dtUserAC = udtUserACBLL.GetUserACForLogin(udtDataEntryUser.DataEntryAccount, udtDataEntryUser.SPID, udtUserAC.UserType)
        End If
        udtAuditLogEntry.WriteStartLog(strLogID, strAuditLogDesc, ViewState("AuditLogSPID"), ViewState("AuditDataEntryAccount"))

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
                    If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                        If Not udtValidator.ValidatePassword(Me.txtNewPassword.Text) Then
                            ' New Password does not match the password criteria
                            Me.udcMessageBox.AddMessage("990000", "E", "00053")
                            Me.imgNewPasswordAlert.Visible = True
                        End If
                    Else
                        If Not udtValidator.ValidatePassword(Me.txtNewPassword.Text, False) Then
                            ' New Password does not match the password criteria
                            Me.udcMessageBox.AddMessage("990000", "E", "00053")
                            Me.imgNewPasswordAlert.Visible = True
                        End If
                    End If
                End If

            End If

            If Me.udcMessageBox.GetCodeTable.Rows.Count = 0 Then
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
                    Me.udcMessageBox.AddMessage("990000", "E", "00051")
                End If
            End If

            If Me.udcMessageBox.GetCodeTable.Rows.Count = 0 Then
                ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
                'Dim strPassword As String = Encrypt.MD5hash(Me.txtNewPassword.Text.Trim)
                Dim udtPassword As HashModel = Hash(Me.txtNewPassword.Text.Trim)
                ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---
                Try
                    ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---                    
                    'udtChangePasswordBLL.UpdatePassword(udtUserAC, strPassword)
                    udtChangePasswordBLL.UpdatePassword(udtUserAC, udtPassword)
                    ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---
                Catch eSQL As SqlClient.SqlException
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
                    Throw ex
                End Try

                If udcMessageBox.GetCodeTable.Rows.Count = 0 Then
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
                    End If

                    udtAuditLogEntry.WriteEndLog(strSuccessLogID, strAuditLogDesc & " successful")

                    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                    Dim udtIdeasBLL As New IdeasBLL
                    Dim udtSessionHandler As New SessionHandler
                    udtIdeasBLL.UpdateIDEASComboInfo(udtUserAC, udtSessionHandler.IDEASComboClientGetFormSession(), udtSessionHandler.IDEASComboVersionGetFormSession())
                    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

                    RedirectHandler.ToURL(ClaimVoucherMaster.FullVersionPage.Home)

                End If
            End If
        End If

        Me.udcMessageBox.BuildMessageBox(strMsgBoxDesc, udtAuditLogEntry, strAuditLogDesc & " fail", strFailLogID, ViewState("AuditLogSPID"), ViewState("AuditDataEntryAccount"))

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

End Class