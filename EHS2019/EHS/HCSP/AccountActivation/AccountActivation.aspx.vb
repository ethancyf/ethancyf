Imports Common.ComFunction
Imports Common.Encryption
Imports Common.Component
Imports Common.ComObject
Imports CustomControls
Imports HCSP.BLL
Imports Common.eHRIntegration.Model.Xml.eHRService
Imports Common.eHRIntegration.BLL
Imports Common.Component.ServiceProvider
Imports Common.ComFunction.AccountSecurity

Partial Public Class AccountActivation
    Inherits BasePage

#Region "Audit Log Description"
    Public Class AublitLogDescription
        ' CRE16-019 (To implement token sharing between eHS(S) and eHRSS) [Start][Winnie]
        Public Const CheckWebUserName As String = "Check Web User Name"
        Public Const CheckWebUserNameSuccess As String = "Check Web User Name successful "
        Public Const CheckWebUserNameFail As String = "Check Web User Name fail"

        Public Const GetEHRSSUsernameClick As String = "Get Username From eHRSS Click"
        Public Const GetEHRSSConsentConfirmClick As String = "Get Username From eHRSS Consent - Confirm Click"
        Public Const GetEHRSSConsentCancelClick As String = "Get Username From eHRSS Consent - Cancel Click"
        Public Const GetEHRSSUsername As String = "Get eHRSS Username"
        Public Const GetEHRSSUsernameSuccess As String = "Get eHRSS Username Success"
        Public Const GetEHRSSUsernameFail As String = "Get eHRSS Username eHRSS Fail"
        ' CRE16-019 (To implement token sharing between eHS(S) and eHRSS) [End][Winnie]
    End Class
#End Region

    Dim udcGeneralFun As New GeneralFunction
    Dim udcValidator As New Common.Validation.Validator
    Dim udcAccountActivateBll As New BLL.AccountActivationBLL
    Dim udtSPProfileBLL As SPProfileBLL = New SPProfileBLL
    Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL

    Dim strActivationCode As String
    Const FUNCTION_CODE As String = "020102"
    Const IsPageSessionAlive As String = "IsActivationPageSessionAlive"

    Const EnableIVSS As String = "IsServiceProviderIncludeIVSSenabledScheme"

    Private Sub Page_Init1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
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
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        Page.Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If Not IsPostBack Then

            Session(IsPageSessionAlive) = "Y"

            Dim udtAuditLogEntry0 As New AuditLogEntry(FUNCTION_CODE, Me)
            udtAuditLogEntry0.WriteLog(LogID.LOG00000, "Account Activation")

            ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
            If Me.SubPlatform = EnumHCSPSubPlatform.CN Then
                udtAuditLogEntry0.WriteLog(LogID.LOG00023, "Account Activation not available in CN Sub Platform")
                Response.Redirect(ClaimVoucherMaster.FullVersionPage.Login)
                Return
            End If
            ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

            Session("strActivationCode") = IIf(IsNothing(Request.QueryString("code")), "", Request.QueryString("code"))

            'add audit log
            Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
            udtAuditLogEntry.AddDescripton("Activation Code", Session("strActivationCode"))
            udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Link Validate")

            Me.panStep1.Visible = False
            Me.panStep2.Visible = False
            Me.panStep3.Visible = False
            Me.panStep4.Visible = False

            If Not udcValidator.IsEmpty(Session("strActivationCode")) Then
                Dim dt As New DataTable

                ' I-CRE16-007-02 (Refine system from CheckMarx findings) [Start][Winnie]
                dt = udcAccountActivateBll.CheckEmailLinkByCode(Hash(Session("strActivationCode")).HashedValue)
                ' I-CRE16-007-02 (Refine system from CheckMarx findings) [End][Winnie]

                If CInt(dt.Rows(0)(0)) = 0 Then
                    Me.udcErrorMessage.AddMessage("990000", "E", "00065")
                    Me.MultiView1.ActiveViewIndex = 6
                    udtAuditLogEntry.AddDescripton("Activation Code", Session("strActivationCode"))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Link Validate fail")
                Else
                    udtAuditLogEntry.AddDescripton("Activation Code", Session("strActivationCode"))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Link Validate successful")
                End If
            Else
                Me.udcErrorMessage.AddMessage("990000", "E", "00065")
                Me.MultiView1.ActiveViewIndex = 6
                udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Link Validate fail")
            End If

            Me.udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00003, "Link Validate fail")
        End If

        SetLanguage()

        Dim strSelectedLang As String
        strSelectedLang = IIf(IsNothing(Request.QueryString("lang")), "", Request.QueryString("lang")).ToString().Trim
        If Not strSelectedLang.Trim.Equals(String.Empty) Then

            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

            RedirectHandler.ToURL(HttpContext.Current.Request.Path & "?code=" & Session("strActivationCode"))

            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End
        End If
        strSelectedLang = LCase(Session("language"))

        ' CRE16-019 (To implement token sharing between eHS(S) and eHRSS) [Start][Winnie]
        Me.trGetUsernameFromEHRSS.Visible = False

        ' Check if able to Get Username from eHRSS
        Select Case Me.SubPlatform
            Case EnumHCSPSubPlatform.NA, EnumHCSPSubPlatform.HK
                If udcGeneralFun.CheckEnableEHRSSinHCSP = GeneralFunction.EnumTurnOnStatus.Yes Then
                    Me.trGetUsernameFromEHRSS.Visible = True
                End If
        End Select
        ' CRE16-019 (To implement token sharing between eHS(S) and eHRSS) [End][Winnie]

        Me.IsSessionExpired()

    End Sub

    Private Sub SetLanguage()
        Dim selectedValue As String

        selectedValue = Session("language")

        If Not IsNothing(Session(EnableIVSS)) Then
            If Session(EnableIVSS) Then
                'Me.panStep3.Visible = True
                lblClaimVoucherStep4.Text = Me.GetGlobalResourceObject("Text", "AccActTimeLine4")
            Else
                'Me.panStep3.Visible = False
                lblClaimVoucherStep4.Text = Me.GetGlobalResourceObject("Text", "AccActTimeLine4-2")
            End If
        End If

        Dim strvalue As String = String.Empty
        Dim strvalue2 As String = String.Empty

        udcGeneralFun.getSystemParameter("PasswordRuleNumber", strvalue, strvalue2)

        Me.txt_accAct_newPW.Attributes.Add("onKeyUp", "checkPassword(this.value,'" & CInt(strvalue.Trim) & "', '" & CInt(strvalue2.Trim) & "', 'strength1','strength2','strength3','progressBar', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthPoor") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthModerate") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthStrong") & "','direction2','direction1');")
        'Me.txt_accAct_newPW.Attributes.Add("onKeyUp", "checkPassword(this.value,'strength1','strength2','strength3','progressBar', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthPoor") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthModerate") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthStrong") & "','direction2','direction1');")

        Dim strHCSPLink As String = String.Empty
        udcGeneralFun.getSystemParameter("HCSPAppPath", strHCSPLink, String.Empty)
        'Me.tblBanner.Style.Item("background-image") = "url(/" & strHCSPLink.Trim & "/" + Me.GetGlobalResourceObject("ImageUrl", "IndexBanner").ToString + ")"
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
            If udcAccountActivateBll.IsSpIdTokenPinMatch(strSPID, strTokenPin) Then
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
            dt = udcAccountActivateBll.GetInfoBySPIDStatus(strSPID, Hash(Session("strActivationCode")).HashedValue)
            ' I-CRE16-007-02 (Refine system from CheckMarx findings) [End][Winnie]

            If dt.Rows.Count <= 0 Then
                Session("SP_TSMP") = Nothing
                Return False
            Else
                If udcAccountActivateBll.ServiceProviderIsActiveBySPID(strSPID) Then
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
            If udcAccountActivateBll.IsAccountAliasDuplicated(strAlias) Then
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

    Private Sub ChangeTimeLine(ByVal strStep As String)
        Dim strhightlight, strunhightlight As String

        strhightlight = "highlightTimeline"
        strunhightlight = "unhighlightTimeline"

        Me.panStep1.CssClass = strunhightlight
        Me.panStep2.CssClass = strunhightlight
        Me.panStep3.CssClass = strunhightlight
        Me.panStep4.CssClass = strunhightlight

        Me.panStep1.Visible = False
        Me.panStep2.Visible = True
        Me.panStep4.Visible = True
        If Session(EnableIVSS) Then
            Me.panStep3.Visible = True
            lblClaimVoucherStep4.Text = Me.GetGlobalResourceObject("Text", "AccActTimeLine4")
        Else
            Me.panStep3.Visible = False
            lblClaimVoucherStep4.Text = Me.GetGlobalResourceObject("Text", "AccActTimeLine4-2")
        End If

        Select Case strStep
            Case "2"
                Me.panStep2.CssClass = strhightlight
            Case "3"
                Me.panStep3.CssClass = strhightlight
            Case "4"
                Me.panStep4.CssClass = strhightlight
            Case Else
                Me.panStep1.CssClass = strhightlight
        End Select


    End Sub

    ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
    Private Sub ActivateAccount()

        'Try
        '    udcAccountActivateBll.ActivateAccount(Me.txt_accAct_SPID.Text, Encrypt.MD5hash(Session("strNewWebPassword")), IIf(udcValidator.IsEmpty(Session("strNewIVRSPassword")), DBNull.Value, Encrypt.MD5hash(Session("strNewIVRSPassword"))), IIf(udcValidator.IsEmpty(Session("strNewUsername")), DBNull.Value, Session("strNewUsername")), Session("SP_TSMP"))
        'Catch eSql As SqlClient.SqlException
        '    Throw eSql
        'Catch ex As Exception
        '    Throw ex
        'End Try
        Dim udtIVRSPassword As New HashModel
        If Not udcValidator.IsEmpty(Session("strNewIVRSPassword")) Then
            udtIVRSPassword = Hash(Session("strNewIVRSPassword"))
        End If

        Try
            udcAccountActivateBll.ActivateAccount(Me.txt_accAct_SPID.Text,Hash(Session("strNewWebPassword")),udtIVRSPassword,IIf(udcValidator.IsEmpty(Session("strNewUsername")), DBNull.Value, Session("strNewUsername")),Session("SP_TSMP"))
        Catch eSql As SqlClient.SqlException
            Throw eSql
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]

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

        'add audit log
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.AddDescripton("Activation Code", Session("strActivationCode"))
        udtAuditLogEntry.AddDescripton("User_ID", Me.txt_accAct_SPID.Text)
        udtAuditLogEntry.AddDescripton("Token PIN", Me.txt_accAct_tokenPIN.Text)

        ''add audit log
        'udtAuditLogEntry.AddDescripton("include EHCVS", Session(IsIncludeEHCVS).ToString())
        'udtAuditLogEntry.AddDescripton("include IVSS", Session(IsIncludeIVSS).ToString())
        Dim udtSchemeClaimBLL As New Scheme.SchemeClaimBLL()
        Session(EnableIVSS) = udtSchemeClaimBLL.CheckSchemeClaimIVRSEnabled(Me.txt_accAct_SPID.Text)
        'Session(EnableIVSS) = udcAccountActivateBll.IncludeIVRSEnabledScheme(Me.txt_accAct_SPID.Text)
        udtAuditLogEntry.AddDescripton("enable IVRS", Session(EnableIVSS).ToString())

        udtAuditLogEntry.WriteStartLog(LogID.LOG00004, "Validate Account Info", Me.txt_accAct_SPID.Text, Nothing)

        Me.img_err_spid.Visible = False
        Me.img_err_tokenPIN.Visible = False

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

                udtAuditLogEntry.AddDescripton("SPID", Me.txt_accAct_SPID.Text.Trim)
                udtAuditLogEntry.AddDescripton("Activation Code", Session("strActivationCode"))
                udtAuditLogEntry.WriteLog(LogID.LOG00019, "Validate Account fail: SPID not match with Activation Code", Me.txt_accAct_SPID.Text.Trim, Nothing)
            End If
        End If

        If udcValidator.IsEmpty(Me.txt_accAct_tokenPIN.Text) Then
            err = True
            Me.udcErrorMessage.AddMessage("990000", "E", "00044")
            Me.img_err_tokenPIN.Visible = True
            'udcLoginBll.UpdateLoginDtmInNonLoginPage(Me.txt_accAct_SPID.Text, LoginStatus.Fail)
        End If

        If (err = False) AndAlso Not IsSpIdTokenPinMatch(Me.txt_accAct_SPID.Text, Me.txt_accAct_tokenPIN.Text) Then
            err = True
            Me.udcErrorMessage.AddMessage("990000", "E", "00066")
            Me.img_err_spid.Visible = True
            Me.img_err_tokenPIN.Visible = True
            'udcLoginBll.UpdateLoginDtmInNonLoginPage(Me.txt_accAct_SPID.Text, LoginStatus.Fail)

            udtAuditLogEntry.AddDescripton("Token Passcode", Me.txt_accAct_tokenPIN.Text)
            udtAuditLogEntry.WriteLog(LogID.LOG00020, "Validate Account fail: Incorrect Token Passcode", Me.txt_accAct_SPID.Text.Trim, Nothing)
        End If

        If Not err Then

            ChangeTimeLine("2")
            Me.MultiView1.ActiveViewIndex = 2
            udtAuditLogEntry.AddDescripton("Activation Code", Session("strActivationCode"))
            udtAuditLogEntry.AddDescripton("User_ID", Me.txt_accAct_SPID.Text)
            udtAuditLogEntry.AddDescripton("Token PIN", Me.txt_accAct_tokenPIN.Text)
            udtAuditLogEntry.AddDescripton("enable IVRS", Session(EnableIVSS).ToString())
            udtAuditLogEntry.WriteEndLog(LogID.LOG00005, "Validate Account Info successful", Me.txt_accAct_SPID.Text, Nothing)
            udcLoginBll.UpdateLoginDtmInNonLoginPage(Me.txt_accAct_SPID.Text, LoginStatus.Success)

            'Obtain the latest Timestamp
            GetLatestTSMPtoSession(Me.txt_accAct_SPID.Text)

            ' CRE16-019 (To implement token sharing between eHS(S) and eHRSS) [Start][Winnie]
            Dim udtSP As ServiceProviderModel = udtSPProfileBLL.loadSP(Me.txt_accAct_SPID.Text)

            udtSP = udtSPProfileBLL.loadSP(Me.txt_accAct_SPID.Text)
            udtServiceProviderBLL.SaveToSession(udtSP)
            ' CRE16-019 (To implement token sharing between eHS(S) and eHRSS) [End][Winnie]
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
                ElseIf dtUserAC.Rows(0)(0).ToString.Trim.Equals("A") Then 'HCSPUserAC Record status is active
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
                Me.udcErrorMessage.AddMessage("990000", "E", "00145")
                'Else
                '    udtAuditLogEntry.WriteEndLog(LogID.LOG00006, "Validate Account Info fail", Me.txt_accAct_SPID.Text, Nothing)
            End If
        End If

        Me.udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, "Validate Account Info fail", LogID.LOG00006, Me.txt_accAct_SPID.Text, Nothing)
    End Sub

    Protected Sub btn_accAct_step3_next_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btn_accAct_step3_next.Click
        Dim err As Boolean = False

        Me.img_err_webalias.Visible = False

        'If Trim(Me.txt_accAlias.Text) = "" Then
        '    err = True

        '    Me.img_err_webalias.Visible = True
        'End If

        If IsAccountAliasDuplicated(Me.txt_accAlias.Text) Then
            err = True
            Me.udcErrorMessage.AddMessage("990000", "E", "00002")
            Me.img_err_webalias.Visible = True
        End If

        Me.udcErrorMessage.BuildMessageBox("ValidationFail")

        If Not err Then
            Me.MultiView1.ActiveViewIndex = 5
        End If
    End Sub

    Protected Sub btn_accAct_step2_next_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btn_accAct_step2_next.Click
        Dim err As Boolean = False

        Me.img_err_hkid.Visible = False
        Me.img_err_contact.Visible = False

        If udcValidator.IsEmpty(Me.txt_accAct_HKID_prefix.Text) Or udcValidator.IsEmpty(Me.txt_accAct_HKID_first3.Text) Then
            err = True

            Me.img_err_hkid.Visible = True
        End If

        If Not udcValidator.IsEmpty(Me.txt_accAct_HKID_first3.Text) And Len(Trim(Me.txt_accAct_HKID_first3.Text)) <> 3 Then
            err = True

            Me.img_err_hkid.Visible = True
        End If

        If udcValidator.IsEmpty(Me.txt_accAct_phone.Text) Then
            err = True

            Me.img_err_contact.Visible = True
        End If

        If err Then

        Else
            Me.MultiView1.ActiveViewIndex = 2
        End If
    End Sub

    Protected Sub btn_accAct_step4_next_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btn_accAct_step4_next.Click
        Dim err As Boolean = False

        Me.img_err_webNewPW.Visible = False
        Me.img_err_webConfirmPW.Visible = False
        Me.img_err_webalias.Visible = False

        'add audit log
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.AddDescripton("Username", Me.txt_accAlias.Text)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00007, "Validate Web Account Info", Me.txt_accAct_SPID.Text, Nothing)

        If Not udcValidator.IsEmpty(Me.txt_accAlias.Text) Then
            If Not IsUserNameFormatOK(Me.txt_accAlias.Text) Then
                err = True
                Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00011")
                Me.img_err_webalias.Visible = True
            End If

            If IsAccountAliasDuplicated(Me.txt_accAlias.Text) And err = False Then
                err = True
                Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00003")
                Me.img_err_webalias.Visible = True
            End If
        End If

        If udcValidator.IsEmpty(Me.txt_accAct_newPW.Text) Then
            err = True
            Me.udcErrorMessage.AddMessage("990000", "E", "00056")
            Me.img_err_webNewPW.Visible = True
        Else
            If Not IsPasswordFormatOK(Trim(Me.txt_accAct_newPW.Text)) Then
                err = True
                Me.udcErrorMessage.AddMessage("990000", "E", "00057")
                Me.img_err_webNewPW.Visible = True
            Else
                If udcValidator.IsEmpty(Me.txt_accAct_confirmPW.Text) Then
                    err = True
                    Me.udcErrorMessage.AddMessage("990000", "E", "00058")
                    Me.img_err_webConfirmPW.Visible = True
                Else
                    If Not IsConfirmPasswordMatch(Trim(Me.txt_accAct_newPW.Text), Trim(Me.txt_accAct_confirmPW.Text)) Then
                        err = True
                        Me.udcErrorMessage.AddMessage("990000", "E", "00059")
                        Me.img_err_webNewPW.Visible = True
                        Me.img_err_webConfirmPW.Visible = True
                    End If
                End If
            End If
        End If

        Me.udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, "Validate Web Account Info fail", LogID.LOG00009, Me.txt_accAct_SPID.Text.Trim, Nothing)
        Me.udcInfoMessageBox.BuildMessageBox()

        If Not err Then
            Session("strNewUsername") = Me.txt_accAlias.Text
            Session("strNewWebPassword") = Me.txt_accAct_newPW.Text

            udtAuditLogEntry.AddDescripton("Username", Me.txt_accAlias.Text)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00008, "Validate Web Account Info successful", Me.txt_accAct_SPID.Text, Nothing)

            'Newly added
            If Session(EnableIVSS) Then
                ChangeTimeLine("3")
                Me.MultiView1.ActiveViewIndex = 3
            Else
                ChangeTimeLine("4")
                Me.MultiView1.ActiveViewIndex = 4
                Session("strNewIVRSPassword") = ""
                ActivateAccount()
                Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                Me.udcInfoMessageBox.AddMessage(FUNCTION_CODE, "I", "00003")
                Me.udcInfoMessageBox.BuildMessageBox()
                ChangeTimeLine("4")
                Me.MultiView1.ActiveViewIndex = 5
            End If
        Else
            Session("strNewUsername") = ""
            Session("strNewWebPassword") = ""
            udtAuditLogEntry.AddDescripton("Username", Me.txt_accAlias.Text)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00021, "Validate Web Account Info fail", Me.txt_accAct_SPID.Text, Nothing)
        End If
    End Sub

    Protected Sub btn_accAct_step5_next_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btn_accAct_step5_next.Click
        Dim err As Boolean = False

        Me.img_err_ivrsNewPW.Visible = False
        Me.img_err_ivrsConfirmPW.Visible = False

        'add audit log
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.AddDescripton("IVRS", "Y")
        udtAuditLogEntry.WriteStartLog(LogID.LOG00013, "Validate IVRS Account and Activation", Me.txt_accAct_SPID.Text, Nothing)

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

        Me.udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, "Validate IVRS Account and Activation fail", LogID.LOG00015, Me.txt_accAct_SPID.Text.Trim, Nothing)

        If Not err Then
            Dim strFunctCode, strSeverityCode, strMsgCode As String
            Dim sm As Common.ComObject.SystemMessage

            Session("strNewIVRSPassword") = Me.txt_accAct_newIVRPW.Text

            Try
                ActivateAccount()
                Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete

                Me.udcInfoMessageBox.AddMessage(FUNCTION_CODE, "I", "00002")

                Me.udcInfoMessageBox.BuildMessageBox()
                ChangeTimeLine("4")
                Me.MultiView1.ActiveViewIndex = 5
                udtAuditLogEntry.WriteEndLog(LogID.LOG00014, "Validate IVRS Account and Activation successful", Me.txt_accAct_SPID.Text, Nothing)
            Catch eSql As SqlClient.SqlException
                Session("strNewIVRSPassword") = ""
                If eSql.Number = 50000 Then
                    strFunctCode = "990001"
                    strSeverityCode = "D"
                    strMsgCode = eSql.Message
                    sm = New Common.ComObject.SystemMessage(strFunctCode, strSeverityCode, strMsgCode)
                    Me.udcErrorMessage.AddMessage(sm)
                    Me.udcErrorMessage.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00015, "Validate IVRS Account and Activation fail")
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00015, "Validate IVRS Account and Activation fail", Me.txt_accAct_SPID.Text, Nothing)
                Else
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00015, "Validate IVRS Account and Activation fail", Me.txt_accAct_SPID.Text, Nothing)
                    Throw eSql
                End If
            Catch ex As Exception
                udtAuditLogEntry.WriteEndLog(LogID.LOG00015, "Validate IVRS Account and Activation fail", Me.txt_accAct_SPID.Text, Nothing)
                Session("strNewIVRSPassword") = ""
                Throw ex
            End Try
        Else
            udtAuditLogEntry.WriteEndLog(LogID.LOG00015, "Validate IVRS Account and Activation fail")
            Session("strNewIVRSPassword") = ""
        End If
    End Sub

    ' CRE16-019 (To implement token sharing between eHS(S) and eHRSS) [Start][Winnie]
    Protected Sub btnGetUsernameFromeHRSS_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00024, AublitLogDescription.GetEHRSSUsernameClick)

        chkGetUsernameFromEHRSS.Checked = False
        ModalPopupExtenderConfirmConsent.Show()
    End Sub

    Protected Sub ibtnConsentCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00026, AublitLogDescription.GetEHRSSConsentCancelClick)

        ModalPopupExtenderConfirmConsent.Hide()
    End Sub

    Protected Sub ibtnConsentConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udcErrorMessage.Visible = False
        Me.udcInfoMessageBox.Visible = False
        Me.img_err_webalias.Visible = False

        ModalPopupExtenderConfirmConsent.Hide()

        Dim udtSP As ServiceProviderModel = Me.udtServiceProviderBLL.GetSP()

        Dim strSPHKID As String = udtSP.HKID

        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00025, AublitLogDescription.GetEHRSSConsentConfirmClick)

        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00027, AublitLogDescription.GetEHRSSUsername)

        Dim udtInXml As InGeteHRSSLoginAliasXmlModel = Nothing

        Try
            udtInXml = (New eHRServiceBLL).GeteHRSSLoginAlias(strSPHKID)

        Catch ex As Exception
            ' Message: The username cannot be obtained from eHRSS, please try again later.
            udcErrorMessage.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00392)
            udcErrorMessage.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00029, AublitLogDescription.GetEHRSSUsernameFail & String.Format("StackTrace={0}", ex.Message))
            Return
        End Try


        udtAuditLogEntry.AddDescripton("Result Code", udtInXml.ResultCode)
        udtAuditLogEntry.AddDescripton("Result Description", udtInXml.ResultDescription)

        Select Case udtInXml.ResultCodeEnum

            Case eHRResultCode.R9002_UserNotFound
                ' Message: You are not a registered user in eHRSS.
                udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00039)
                udcInfoMessageBox.Type = InfoMessageBoxType.Information
                udcInfoMessageBox.BuildMessageBox()
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00029, AublitLogDescription.GetEHRSSUsernameFail)
                Return

            Case eHRResultCode.R1000_Success

            Case Else
                ' Message: The username cannot be obtained from eHRSS, please try again later.
                udcErrorMessage.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00392)
                udcErrorMessage.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00029, AublitLogDescription.GetEHRSSUsernameFail)
                Return
        End Select


        Dim strEHRSSUsername As String = String.Empty
        Dim blnErr As Boolean = False

        strEHRSSUsername = udtInXml.LoginAlias.Trim.ToUpper
        udtAuditLogEntry.AddDescripton("Username", strEHRSSUsername)

        ' Check username availability
        If Not udtSPProfileBLL.chkValidLoginID(strEHRSSUsername) Then
            blnErr = True
            Me.udcErrorMessage.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00393, "%s", strEHRSSUsername)

        ElseIf udtSPProfileBLL.chkDuplicateUsername(strEHRSSUsername) Then
            blnErr = True
            Me.udcErrorMessage.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00394, "%s", strEHRSSUsername)
        End If

        If blnErr Then
            Me.udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00029, AublitLogDescription.GetEHRSSUsernameFail)
        Else
            ' Fill the value
            Me.txt_accAlias.Text = strEHRSSUsername

            ' Message: The Username is retrieved from eHRSS successfully.
            udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00040)
            udcInfoMessageBox.Type = InfoMessageBoxType.Complete
            udcInfoMessageBox.BuildMessageBox()

            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00028, AublitLogDescription.GetEHRSSUsernameSuccess)
        End If
    End Sub
    ' CRE16-019 (To implement token sharing between eHS(S) and eHRSS) [End][Winnie]

    Protected Sub btn_checkAvailability_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btn_checkAvailability.Click
        Dim err As Boolean = False

        'add audit log
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.AddDescripton("Username", Me.txt_accAlias.Text)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00010, AublitLogDescription.CheckWebUserName, Me.txt_accAct_SPID.Text, Nothing)

        If udcValidator.IsEmpty(Me.txt_accAlias.Text) Then
            err = True
            Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00010")
            Me.img_err_webalias.Visible = True
        Else
            If Not IsUserNameFormatOK(Me.txt_accAlias.Text) Then
                err = True
                Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00011")
                Me.img_err_webalias.Visible = True
            Else
                If IsAccountAliasDuplicated(Me.txt_accAlias.Text) Then
                    err = True
                    Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00003")
                    Me.img_err_webalias.Visible = True
                End If
            End If
        End If

        If Not err Then
            Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
            Me.udcInfoMessageBox.AddMessage(FUNCTION_CODE, "I", "00001")
            udtAuditLogEntry.AddDescripton("Username", Me.txt_accAlias.Text)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00011, AublitLogDescription.CheckWebUserNameSuccess, Me.txt_accAct_SPID.Text, Nothing)
        Else
            udtAuditLogEntry.AddDescripton("Username", Me.txt_accAlias.Text)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00012, AublitLogDescription.CheckWebUserNameFail, Me.txt_accAct_SPID.Text, Nothing)
        End If

        Me.udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00022, AublitLogDescription.CheckWebUserNameFail)
        Me.udcInfoMessageBox.BuildMessageBox()
    End Sub

    Protected Sub btn_skipIVRS_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btn_skipIVRS.Click
        Dim strFunctCode, strSeverityCode, strMsgCode As String
        Dim sm As Common.ComObject.SystemMessage

        'add audit log
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.AddDescripton("IVRS", "N")
        udtAuditLogEntry.WriteStartLog(LogID.LOG00016, "Skip IVRS Account and Activation", Me.txt_accAct_SPID.Text, Nothing)

        Try
            Session("strNewIVRSPassword") = ""
            ActivateAccount()
            Me.udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00018, "Skip IVRS Account and Activation fail")
            Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete

            Me.udcInfoMessageBox.AddMessage(FUNCTION_CODE, "I", "00003")

            Me.udcInfoMessageBox.BuildMessageBox()
            ChangeTimeLine("4")
            Me.MultiView1.ActiveViewIndex = 5
            udtAuditLogEntry.WriteEndLog(LogID.LOG00017, "Skip IVRS Account and Activation successful", Me.txt_accAct_SPID.Text, Nothing)
        Catch eSql As SqlClient.SqlException
            Session("strNewIVRSPassword") = ""
            If eSql.Number = 50000 Then
                strFunctCode = "990001"
                strSeverityCode = "D"
                strMsgCode = eSql.Message
                sm = New Common.ComObject.SystemMessage(strFunctCode, strSeverityCode, strMsgCode)
                Me.udcErrorMessage.AddMessage(sm)
                Me.udcErrorMessage.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00018, "Skip IVRS Account and Activation fail")
                udtAuditLogEntry.WriteEndLog(LogID.LOG00018, "Skip IVRS Account and Activation fail", Me.txt_accAct_SPID.Text, Nothing)
            Else
                udtAuditLogEntry.WriteEndLog(LogID.LOG00018, "Skip IVRS Account and Activation fail", Me.txt_accAct_SPID.Text, Nothing)
                Throw eSql
            End If
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00018, "Skip IVRS Account and Activation fail", Me.txt_accAct_SPID.Text, Nothing)
            Session("strNewIVRSPassword") = ""
            Throw ex
        End Try
    End Sub

    Private Sub IsSessionExpired()
        If IsNothing(Session(IsPageSessionAlive)) Then
            Response.Redirect("~/en/sessionexpired.aspx")
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

    ''' <summary>
    ''' CRE11-004
    '''  Clear all working data
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub ClearWorkingData()
        MyBase.ClearWorkingData()

    End Sub
End Class