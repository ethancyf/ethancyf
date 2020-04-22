Imports Common.Encryption
Imports Common.Component
Imports Common.ComObject

Partial Public Class ChangeEmail
    Inherits BasePage
    'Inherits System.Web.UI.Page

    Private udcGeneralF As New Common.ComFunction.GeneralFunction
    Dim udcChangeEmailBll As New BLL.ChangeEmailBLL
    Dim udcValidator As New Common.Validation.Validator
    Const FUNCTION_CODE As String = "020006"
    Const IsPageSessionAlive As String = "IsConfirmChangeEmailPageSessionAlive"

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim udcGeneralFun As New Common.ComFunction.GeneralFunction
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


        If Not IsPostBack Then
            Session(IsPageSessionAlive) = "Y"

            Session("strChangeEmailCode") = IIf(IsNothing(Request.QueryString("code")), "", Request.QueryString("code"))

            'add audit log
            Dim udtAuditLogEntry0 As New AuditLogEntry(FUNCTION_CODE, Me)
            udtAuditLogEntry0.WriteLog(LogID.LOG00000, "Change Email")

            ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
            If Me.SubPlatform = EnumHCSPSubPlatform.CN Then
                udtAuditLogEntry0.WriteLog(LogID.LOG00011, "Change Email not available in CN Sub Platform")
                Response.Redirect(ClaimVoucherMaster.FullVersionPage.Login)
                Return
            End If
            ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

            Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
            udtAuditLogEntry.AddDescripton("Activation Code", Session("strChangeEmailCode"))
            udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Link Validate")

            If Not udcValidator.IsEmpty(Session("strChangeEmailCode")) Then
                Dim dt As New DataTable

                ' I-CRE16-007-02 (Refine system from CheckMarx findings) [Start][Winnie]
                dt = udcChangeEmailBll.CheckEmailLinkByCode(Common.ComFunction.AccountSecurity.Hash(Session("strChangeEmailCode")).HashedValue)
                ' I-CRE16-007-02 (Refine system from CheckMarx findings) [End][Winnie]

                If CInt(dt.Rows(0)(0)) = 0 Then
                    Me.udcErrorMessage.AddMessage("990000", "E", "00065")
                    Me.MultiView1.ActiveViewIndex = 2
                    udtAuditLogEntry.AddDescripton("Activation Code", Session("strChangeEmailCode"))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Link Validate fail")
                Else
                    udtAuditLogEntry.AddDescripton("Activation Code", Session("strChangeEmailCode"))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Link Validate successful")
                End If
            Else
                Me.udcErrorMessage.AddMessage("990000", "E", "00065")
                Me.MultiView1.ActiveViewIndex = 2
                udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Link Validate fail")
            End If

            Me.udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00011, "Show Link Validate Fail MessageBox")
        End If

        SetLanguage()
        Dim strSelectedLang As String
        strSelectedLang = IIf(IsNothing(Request.QueryString("lang")), "", Request.QueryString("lang")).ToString().Trim
        If Not strSelectedLang.Trim.Equals(String.Empty) Then

            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

            RedirectHandler.ToURL(HttpContext.Current.Request.Path & "?code=" & Session("strChangeEmailCode"))

            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

        End If
        strSelectedLang = LCase(Session("language"))


        'Dim strHost As String = Request.Url.ToString.Substring(0, Request.Url.ToString.IndexOf(Request.Path))
        'Dim strPrivacyPolicyLink As String = strHost & Request.ApplicationPath
        'If strPrivacyPolicyLink.Substring(strPrivacyPolicyLink.Length - 1, 1) <> "/" Then
        '    strPrivacyPolicyLink = strPrivacyPolicyLink & "/PrivacyPolicy/PrivacyPolicy.htm"
        'Else
        '    strPrivacyPolicyLink = strPrivacyPolicyLink & "PrivacyPolicy/PrivacyPolicy.htm"

        'End If

        'Me.lnkBtnPrivacyPolicy.OnClientClick = "javascript:openNewHTML('" + strPrivacyPolicyLink + "');return false;"

        'Dim strDisclaimerPolicyLink As String = strHost & Request.ApplicationPath
        'If strDisclaimerPolicyLink.Substring(strDisclaimerPolicyLink.Length - 1, 1) <> "/" Then
        '    strDisclaimerPolicyLink = strDisclaimerPolicyLink & "/Disclaimer/Disclaimer.htm"
        'Else
        '    strDisclaimerPolicyLink = strDisclaimerPolicyLink & "Disclaimer/Disclaimer.htm"
        'End If

        'Me.lnkBtnDisclaimer.OnClientClick = "javascript:openNewHTML('" + strDisclaimerPolicyLink + "');return false;"

        Me.IsSessionExpired()

    End Sub

    Private Sub SetLanguage()
        Dim selectedValue As String

        selectedValue = Session("language")

        Dim strHCSPLink As String = String.Empty
        udcGeneralF.getSystemParameter("HCSPAppPath", strHCSPLink, String.Empty)
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

        Dim strSPID, strEmail, strERN As String

        'add audit log
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.AddDescripton("Activation Code", Session("strChangeEmailCode"))
        udtAuditLogEntry.AddDescripton("User_ID_Name", Me.txt_accAct_SPID.Text)
        udtAuditLogEntry.AddDescripton("Token PIN", Me.txt_accAct_tokenPIN.Text)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00004, "Validate Account Info and update Email", Me.txt_accAct_SPID.Text, Nothing)

        strSPID = ""
        strEmail = ""
        strERN = ""

        Me.img_err_spid.Visible = False
        Me.img_err_tokenPIN.Visible = False
        Me.img_err_Password.Visible = False

        Try
            If udcValidator.IsEmpty(Me.txt_accAct_SPID.Text) Then
                err = True
                Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00001")
                Me.img_err_spid.Visible = True
            Else
                If Not IsSpIdQualified(Me.txt_accAct_SPID.Text) Then
                    err = True
                    Me.udcErrorMessage.AddMessage("990000", "E", "00066")
                    Me.img_err_spid.Visible = True
                    Me.img_err_tokenPIN.Visible = True
                    'udcLoginBll.UpdateLoginDtmInNonLoginPage(Me.txt_accAct_SPID.Text, LoginStatus.Fail)

                    udtAuditLogEntry.AddDescripton("SPID", Me.txt_accAct_SPID.Text)
                    udtAuditLogEntry.WriteLog(LogID.LOG00007, "Validate fail: Incorrect SPID (SPID not match with Activation Code or SPID is not ACTIVE)", Me.txt_accAct_SPID.Text, Nothing)
                End If

                If Not err Then
                    If Not GetTentativeEmail(Me.txt_accAct_SPID.Text, strEmail, strSPID, strERN) Then
                        Session("strChangeEmailSPID") = ""
                        err = True
                        Me.udcErrorMessage.AddMessage("990000", "E", "00066")
                        Me.img_err_spid.Visible = True
                        Me.img_err_tokenPIN.Visible = True

                        udtAuditLogEntry.AddDescripton("SPID", Me.txt_accAct_SPID.Text)
                        udtAuditLogEntry.WriteLog(LogID.LOG00010, "Validate fail: Incorrect SPID (No tentative Email found for this SP)", Me.txt_accAct_SPID.Text, Nothing)
                    Else
                        Me.lbl_final2.Text = strEmail
                        Session("strChangeEmailSPID") = strSPID
                    End If
                End If
            End If

            '20090115 - Remove Password Field
            'If udcValidator.IsEmpty(Me.txt_password.Text) Then
            '    err = True
            '    Me.udcErrorMessage.AddMessage("990000", "E", "00043")
            '    Me.img_err_Password.Visible = True
            'Else
            '    If Not err AndAlso Not IsSpIdPasswordMatch(strSPID, Me.txt_password.Text) Then
            '        err = True
            '        Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00014")
            '        Me.img_err_spid.Visible = True
            '        Me.img_err_Password.Visible = True
            '        Me.img_err_tokenPIN.Visible = True
            '        udcLoginBll.UpdateLoginDtmInNonLoginPage(Me.txt_accAct_SPID.Text, LoginStatus.Fail)

            '        udtAuditLogEntry.WriteLog(LogID.LOG00008, "Validate fail: Incorrect Password")
            '    End If
            'End If

            If udcValidator.IsEmpty(Me.txt_accAct_tokenPIN.Text) Then
                err = True
                Me.udcErrorMessage.AddMessage("990000", "E", "00044")
                Me.img_err_tokenPIN.Visible = True
            End If

            If (err = False) AndAlso Not IsSpIdTokenPinMatch(strSPID, Me.txt_accAct_tokenPIN.Text) Then
                err = True
                Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00013")
                Me.img_err_spid.Visible = True
                '20090115 - Remove Password Field
                'Me.img_err_Password.Visible = True
                Me.img_err_tokenPIN.Visible = True
                'udcLoginBll.UpdateLoginDtmInNonLoginPage(Me.txt_accAct_SPID.Text, LoginStatus.Fail)
                udtAuditLogEntry.AddDescripton("Token PIN", Me.txt_accAct_tokenPIN.Text)
                udtAuditLogEntry.WriteLog(LogID.LOG00009, "Validate fail: Incorrect Token Passcode", Me.txt_accAct_SPID.Text, Nothing)
            End If

            If Not err Then
                Try
                    udcChangeEmailBll.UpdateSPEmailAddress(Session("strChangeEmailSPID"), Session("ChangeEmailSPtsmp"), strERN, strEmail)
                    Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                    Me.udcInfoMessageBox.AddMessage(FUNCTION_CODE, "I", "00001")
                    Me.udcInfoMessageBox.BuildMessageBox()
                    Me.MultiView1.ActiveViewIndex = 1
                    udtAuditLogEntry.AddDescripton("Activation Code", Session("strChangeEmailCode"))
                    udtAuditLogEntry.AddDescripton("User_ID_Name", Me.txt_accAct_SPID.Text)
                    udtAuditLogEntry.AddDescripton("Token PIN", Me.txt_accAct_tokenPIN.Text)
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00005, "Validate Account Info and update Email successful", Session("strChangeEmailSPID").ToString.Trim, Nothing)
                    udcLoginBll.UpdateLoginDtmInNonLoginPage(Me.txt_accAct_SPID.Text, LoginStatus.Success)

                    'Obtain the latest Timestamp
                    GetLatestTSMPtoSession(strSPID)
                Catch eSql As SqlClient.SqlException
                    If eSql.Number = 50000 Then
                        Dim sm As Common.ComObject.SystemMessage
                        sm = New Common.ComObject.SystemMessage("990001", "D", eSql.Message)
                        Me.udcErrorMessage.AddMessage(sm)
                        Me.udcErrorMessage.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00006, "Validate Account Info and update Email fail", Me.txt_accAct_SPID.Text, Nothing)
                    Else
                        udtAuditLogEntry.AddDescripton("Activation Code", Session("strChangeEmailCode"))
                        udtAuditLogEntry.AddDescripton("User_ID_Name", Me.txt_accAct_SPID.Text)
                        udtAuditLogEntry.AddDescripton("Token PIN", Me.txt_accAct_tokenPIN.Text)
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00006, "Validate Account Info and update Email fail", Me.txt_accAct_SPID.Text, Nothing)
                        Throw eSql
                    End If
                Catch ex As Exception
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00006, "Validate Account Info and update Email fail", Me.txt_accAct_SPID.Text, Nothing)
                    Throw ex
                End Try
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
                    ElseIf dtUserAC.Rows(0)(0).ToString.Trim.Equals("S") Then 'locked (Status in HCSPUserAC)
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
                    'Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00013")
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
                    '    udtAuditLogEntry.WriteEndLog(LogID.LOG00006, "Validate Account Info and update Email fail", Me.txt_accAct_SPID.Text, Nothing)
                End If
            End If
            Me.udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00006, "Validate Account Info and update Email fail")
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00006, "Validate Account Info and update Email fail", Me.txt_accAct_SPID.Text, Nothing)
            Throw ex
        End Try
    End Sub

    Private Function GetTentativeEmail(ByVal strIDUsername As String, ByRef strEmail As String, ByRef strSPID As String, ByRef strERN As String) As Boolean
        Dim dt As New DataTable

        dt = udcChangeEmailBll.GetTentativeEmail(strIDUsername)

        If dt.Rows.Count = 0 Then
            Session("ChangeEmailSPtsmp") = Nothing
            Return False
        Else
            strEmail = Trim(dt.Rows(0)(0))
            strSPID = Trim(dt.Rows(0)(1))
            strERN = Trim(dt.Rows(0)(3))
            Session("ChangeEmailSPtsmp") = dt.Rows(0)(2)
            Return True
        End If
    End Function

    Private Sub GetLatestTSMPtoSession(ByVal strSPID As String)
        Dim dt As New DataTable
        Dim udtUserACBLL As New Common.Component.UserAC.UserACBLL
        Try
            ' CRE16-026 (Change email for locked SP) [Start][Winnie]
            dt = udtUserACBLL.GetTSMPBySPID(strSPID)
            ' CRE16-026 (Change email for locked SP) [End][Winnie]

            If dt.Rows.Count <= 0 Then
                Session("ChangeEmailSPtsmp") = Nothing
            Else
                Session("ChangeEmailSPtsmp") = dt.Rows(0)("TSMP")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function IsSpIdTokenPinMatch(ByVal strSPID As String, ByVal strTokenPin As String) As Boolean

        Try
            If udcValidator.IsEmpty(strSPID) Or udcValidator.IsEmpty(strTokenPin) Then
                Return False
            Else
                If udcChangeEmailBll.IsSpIdTokenPinMatch(strSPID, strTokenPin) Then
                    Return True
                Else
                    Return False
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
    'Private Function IsSpIdPasswordMatch(ByVal strSPID As String, ByVal strPassword As String) As Boolean
    '    Try
    '        If udcValidator.IsEmpty(strSPID) Or udcValidator.IsEmpty(strPassword) Then
    '            Return False
    '        Else
    '            If udcChangeEmailBll.IsSpIdPasswordMatch(strSPID, strPassword) Then
    '                Return True
    '            Else
    '                Return False
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function

    ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---

    Private Function IsSpIdQualified(ByVal strSPID As String) As Boolean
        Dim dt As New DataTable

        Try            
            ' I-CRE16-007-02 (Refine system from CheckMarx findings) [Start][Winnie]
            dt = udcChangeEmailBll.GetInfoBySPIDActivationCode(strSPID, Common.ComFunction.AccountSecurity.Hash(Session("strChangeEmailCode")).HashedValue)
            ' I-CRE16-007-02 (Refine system from CheckMarx findings) [End][Winnie]

            If dt.Rows.Count <= 0 Then
                Session("SP_TSMP") = Nothing
                Return False
            Else
                Session("SP_TSMP") = dt.Rows(0)("TSMP")
                Return True
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub IsSessionExpired()
        If IsNothing(Session(IsPageSessionAlive)) Then
            Response.Redirect("~/en/sessionexpired.aspx")
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
End Class