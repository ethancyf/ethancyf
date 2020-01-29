Imports System
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Net
Imports System.Net.Security
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates

Imports com.rsa.admin
Imports com.rsa.admin.data
Imports com.rsa.authmgr.admin.agentmgt
Imports com.rsa.authmgr.admin.agentmgt.data
Imports com.rsa.authmgr.admin.hostmgt.data
Imports com.rsa.authmgr.admin.principalmgt
Imports com.rsa.authmgr.admin.principalmgt.data
Imports com.rsa.authmgr.admin.tokenmgt
Imports com.rsa.authmgr.admin.tokenmgt.data
Imports com.rsa.authmgr.common
Imports com.rsa.authn
Imports com.rsa.authn.data
Imports com.rsa.common
Imports com.rsa.command
Imports com.rsa.command.exception
Imports com.rsa.common.search
Imports com.rsa.ucm.am


Partial Public Class _Default
    Inherits System.Web.UI.Page

#Region "Properties and Fields"

    Private Const VS_IDSOURCE As String = "IDSOURCE"
    Private Const SESS_SPPRINCIPAL As String = "SESS_SPPRINCIPAL"

    Public Property IdentitySource() As IdentitySourceDTO
        Get
            Dim objIDSource As Object = ViewState(VS_IDSOURCE)

            If IsNothing(objIDSource) Then
                Throw New Exception("IdentitySource is nothing")
            Else
                Return DirectCast(ViewState(VS_IDSOURCE), IdentitySourceDTO)
            End If

        End Get
        Set(ByVal value As IdentitySourceDTO)
            ViewState(VS_IDSOURCE) = value
        End Set
    End Property

    Public Property SPPrincipalList() As SortedList
        Get
            Dim lst As SortedList = Session(SESS_SPPRINCIPAL)

            If IsNothing(lst) Then
                Throw New Exception("Session(SESS_SPPRINCIPAL) is nothing")
            End If

            Return lst
        End Get
        Set(ByVal value As SortedList)
            Session(SESS_SPPRINCIPAL) = value
        End Set
    End Property

#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            For Each strSite As String In ConfigurationManager.AppSettings("RSASite").Split(New String() {"|||"}, StringSplitOptions.None)

                'Check the link exist in web config
                Dim strSiteLink As String = ConfigurationManager.AppSettings(String.Format("RSA{0}Link", strSite))
                Dim strSiteName As String = ConfigurationManager.AppSettings(String.Format("RSA{0}SiteName", strSite))

                If Not IsNothing(strSiteLink) Then
                    Dim item As New ListItem
                    item.Text = strSiteName & " - " & strSiteLink
                    item.Value = strSite

                    rblLink.Items.Add(item)
                End If
            Next

            rblLink.SelectedIndex = 0

            'Add Web Service
            Dim strRSAAgentWSLink As String = ConfigurationManager.AppSettings("RSAAgentWSLink")

            If Not IsNothing(strRSAAgentWSLink) Then
                Dim item As New ListItem
                item.Text = "RSA Web Service"
                item.Value = "WS"

                rblLink.Items.Add(item)
            End If

            ScriptManager1.SetFocus(txtLoginID)

        End If

    End Sub

    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lstrCorrectStaffID As String = ConfigurationManager.AppSettings("StaffID")
        Dim lstrCorrectPassword As String = ConfigurationManager.AppSettings("Password")

        ' I-CRE16-007-02 (Refine system from CheckMarx findings) [Start][Winnie]
        If Hash(txtLoginID.Text) <> lstrCorrectStaffID OrElse Hash(txtPassword.Text) <> lstrCorrectPassword Then
            ' I-CRE16-007-02 (Refine system from CheckMarx findings) [End][Winnie]

            ShowError(lblLoginResult, "Incorrect login!")
            Return

        End If

        Try

            'set up remote certificate validation
            ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertificate)

            If rblLink.SelectedValue.Equals("WS") Then
                InitControlOnce()
                mvCore.SetActiveView(vContent)

            Else
                'Clear RSA setting stored in memory
                clearRSA()

                InitControlOnce()

                mvCore.SetActiveView(vContent)
            End If


        Catch ex As Exception
            ShowError(lblLoginResult, ex.Message)

        End Try

    End Sub

    Private Sub InitControlOnce()
        tdANextPasscodeText.Style("display") = "none"
        tdANextPasscode.Style("display") = "none"
        btnACancel.Visible = False
        lblACancelSpace.Visible = False
        SPPrincipalList = Nothing
        gvSP.Visible = False
        trUPC.Visible = False

        lblServer.Text = rblLink.SelectedItem.Text

        If Not IsNothing(ConfigurationManager.AppSettings("EnableUATDataReset")) AndAlso ConfigurationManager.AppSettings("EnableUATDataReset") = "Y" Then
            panRUD.Visible = True
        Else
            panRUD.Visible = False
        End If

        lblDAppPool.Text = HttpContext.Current.Request.ServerVariables("APP_POOL_ID")
        lblDConfPath.Text = ConfigurationManager.AppSettings(String.Format("RSA{0}AgentConfPath", rblLink.SelectedValue))

        If rblLink.SelectedValue.Equals("WS") Then
            panSearchToken.Visible = False
        Else
            panSearchToken.Visible = True
        End If

    End Sub

    Private Function ValidateServerCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
        Return True
    End Function

    ' I-CRE16-007-02 (Refine system from CheckMarx findings) [Start][Winnie]
    'Public Shared Function MD5hash(ByVal strSourceText As String) As String
    '    Dim objUnicodeEncoding As New UnicodeEncoding
    '    Dim bytSourceText() As Byte = objUnicodeEncoding.GetBytes(strSourceText)
    '    Dim Md5 As New MD5CryptoServiceProvider
    '    Dim bytHash() As Byte = Md5.ComputeHash(bytSourceText)
    '    Return Convert.ToBase64String(bytHash)
    'End Function

    Private Shared Function Hash(ByVal strSourceText As String) As String
        ' Hash by SHA512
        Dim objUnicodeEncoding As New UnicodeEncoding
        Dim bytSourceText() As Byte = objUnicodeEncoding.GetBytes(strSourceText)
        Dim sha512 As New SHA512CryptoServiceProvider
        Dim bytHash() As Byte = sha512.ComputeHash(bytSourceText)
        Return Convert.ToBase64String(bytHash)
    End Function
    ' I-CRE16-007-02 (Refine system from CheckMarx findings) [End][Winnie]

    '

    Protected Sub btnSPExecute_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' --- Init ---
        btnSPClear_Click(Nothing, Nothing)
        InitRSA()
        ' --- End of Init ---

        Dim c1 As New SearchPrincipalsCommand

        Select Case ddlSPUserID.SelectedValue
            Case "contain"
                c1.filter = Filter.contains(PrincipalDTO._LOGINUID, txtSPUserID.Text.Trim)
            Case "start"
                c1.filter = Filter.startsWith(PrincipalDTO._LOGINUID, txtSPUserID.Text.Trim)
            Case "equal"
                c1.filter = Filter.equal(PrincipalDTO._LOGINUID, txtSPUserID.Text.Trim)
            Case Else
                c1.filter = Filter.empty
        End Select

        c1.limit = Integer.MaxValue
        c1.identitySourceGuid = IdentitySource.guid
        c1.securityDomainGuid = IdentitySource.securityDomainGuid

        c1.execute()

        If c1.principals.Length = 0 Then
            gvSP.Visible = False

            ShowError(lblSPResult, "No records found")
            Return

        End If

        Dim lstPrincipal As New SortedList

        For Each p As PrincipalDTO In c1.principals
            Select Case ddlSPPEnable.SelectedValue
                Case "any"
                    lstPrincipal.Add(p.userID, p)
                Case "yes"
                    If p.enabled Then lstPrincipal.Add(p.userID, p)
                Case "no"
                    If p.enabled = False Then lstPrincipal.Add(p.userID, p)
            End Select

        Next

        If lstPrincipal.Count = 0 Then
            gvSP.Visible = False

            ShowError(lblSPResult, "No records found")
            Return

        End If

        If cboSPListToken.Checked AndAlso lstPrincipal.Count > CInt(ConfigurationManager.AppSettings("EnquireTokenWarningLimit")) Then
            lblSPQToken.Text = lstPrincipal.Count
            mvSPContent.SetActiveView(vSPQuestion)
            SPPrincipalList = lstPrincipal

        Else
            BindGvSP(lstPrincipal)

        End If

    End Sub

    Protected Sub btnSPQYes_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        BindGvSP(SPPrincipalList)

        mvSPContent.SetActiveView(vSPContent)
        SPPrincipalList = Nothing

    End Sub

    Protected Sub btnSPQNo_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        mvSPContent.SetActiveView(vSPContent)
        SPPrincipalList = Nothing
    End Sub

    Protected Sub btnSPClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        gvSP.Visible = False
        lblSPResult.Text = String.Empty
        SPPrincipalList = Nothing
    End Sub

    Private Sub BindGvSP(ByVal lstPrincipal As SortedList)
        gvSP.DataSource = lstPrincipal.Values
        gvSP.DataBind()

        gvSP.Visible = True

        gvSP.Columns(4).Visible = cboSPListToken.Checked
        gvSP.Columns(5).Visible = cboSPListToken.Checked
        gvSP.Columns(6).Visible = cboSPListToken.Checked
        gvSP.Columns(7).Visible = cboSPListToken.Checked
        gvSP.Columns(8).Visible = cboSPListToken.Checked
        gvSP.Columns(9).Visible = cboSPListToken.Checked

    End Sub

    Protected Sub gvSP_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim p As PrincipalDTO = e.Row.DataItem

            ' User ID
            DirectCast(e.Row.FindControl("lblSPGUserID"), Label).Text = p.userID

            ' Enable
            Dim lblSPGEnable As Label = e.Row.FindControl("lblSPGEnable")

            If p.enabled Then
                lblSPGEnable.Text = "Yes"
            Else
                lblSPGEnable.Text = "No"
                lblSPGEnable.ForeColor = Drawing.Color.Red
            End If

            ' Fail Count
            Dim lblSPGFailCount As Label = e.Row.FindControl("lblSPGFailCount")
            lblSPGFailCount.Text = p.failedPasswordCount

            If p.failedPasswordCount = 0 Then
                lblSPGFailCount.ForeColor = Drawing.Color.LightGray
            End If

            ' Lockout
            Dim lblSPGLockout As Label = e.Row.FindControl("lblSPGLockout")

            If p.lockoutStatus Then
                lblSPGLockout.Text = "Yes"
                lblSPGLockout.ForeColor = Drawing.Color.Red
                lblSPGFailCount.ForeColor = Drawing.Color.Red
            Else
                lblSPGLockout.Text = "No"
            End If

            ' List Token
            If cboSPListToken.Checked Then
                Dim c2 As New ListTokensByPrincipalCommand
                c2.principalId = p.guid
                c2.execute()

                If c2.tokenDTOs.Length = 0 Then
                    Dim lblSPGToken As Label = e.Row.FindControl("lblSPGToken")
                    lblSPGToken.Text = "-- NA --"
                    lblSPGToken.ForeColor = Drawing.Color.LightGray

                    If ddlSPTEnable.SelectedValue = "yes" OrElse ddlSPTEnable.SelectedValue = "no" Then
                        e.Row.Visible = False
                    End If

                Else
                    ' Exclude replacement token
                    Dim intToken As Integer = 0
                    Dim strTokenGuid As String = String.Empty

                    For Each t As ListTokenDTO In c2.tokenDTOs
                        If t.replacementMode <> TokenDTO._IS_REPLACEMENT_TKN Then
                            intToken += 1
                            strTokenGuid = t.guid
                        End If
                    Next

                    If intToken = 1 Then
                        Dim c3 As New LookupTokenCommand
                        c3.guid = strTokenGuid
                        c3.execute()

                        Dim t As TokenDTO = c3.token

                        ' Visible
                        Select Case ddlSPTEnable.SelectedValue
                            Case "yes"
                                If t.tokenEnabled = False Then e.Row.Visible = False
                            Case "no"
                                If t.tokenEnabled Then e.Row.Visible = False
                        End Select

                        ' Token Serial No.
                        DirectCast(e.Row.FindControl("lblSPGToken"), Label).Text = t.serialNumber

                        ' Enable
                        Dim lblSPGTokenEnable As Label = e.Row.FindControl("lblSPGTokenEnable")

                        If t.tokenEnabled Then
                            lblSPGTokenEnable.Text = "Yes"
                        Else
                            lblSPGTokenEnable.Text = "No"
                            lblSPGTokenEnable.ForeColor = Drawing.Color.Red
                        End If

                        ' Fail Count
                        Dim lblSPGTokenFailCount As Label = e.Row.FindControl("lblSPGTokenFailCount")
                        lblSPGTokenFailCount.Text = t.badTokenCodeCount

                        If t.badTokenCodeCount = 0 Then
                            lblSPGTokenFailCount.ForeColor = Drawing.Color.LightGray
                        End If

                        ' Next Token Mode
                        Dim lblSPGTokenNextTokenMode As Label = e.Row.FindControl("lblSPGTokenNextTokenMode")

                        If t.nextTokenCodeMode Then
                            lblSPGTokenNextTokenMode.Text = "Yes"
                            lblSPGTokenNextTokenMode.ForeColor = Drawing.Color.Red
                            lblSPGTokenFailCount.ForeColor = Drawing.Color.Red
                        Else
                            lblSPGTokenNextTokenMode.Text = "No"
                        End If

                        ' Replacement Mode
                        Dim lblSPGTokenReplacementMode As Label = e.Row.FindControl("lblSPGTokenReplacementMode")
                        lblSPGTokenReplacementMode.Text = t.replacementMode

                        If t.replacementMode = 0 Then
                            lblSPGTokenReplacementMode.ForeColor = Drawing.Color.LightGray
                        End If

                        ' Replacement Token
                        DirectCast(e.Row.FindControl("lblSPGTokenReplacementToken"), Label).Text = t.replaceTknSN

                    Else
                        Dim lblSPGToken As Label = e.Row.FindControl("lblSPGToken")
                        lblSPGToken.Text = "-- MORETHAN1 --"
                        lblSPGToken.ForeColor = Drawing.Color.Red

                        Dim lstToken As New List(Of String)
                        For Each t As ListTokenDTO In c2.tokenDTOs
                            lstToken.Add(t.serialNumber)
                        Next

                        lblSPGToken.Attributes("tt") = String.Join(",", lstToken.ToArray)
                        lblSPGToken.Style("cursor") = "help"

                    End If

                End If

            End If

        End If

    End Sub

    Protected Sub gvSP_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
        If gvSP.Visible = False Then Return

        Dim lintRowCount As Integer = 0

        For Each gv As GridViewRow In gvSP.Rows
            If gv.Visible Then lintRowCount += 1
        Next

        If lintRowCount = 0 Then
            ShowError(lblSPResult, "No records found")
            gvSP.Visible = False
        Else
            ShowComplete(lblSPResult, String.Format("Showing {0} records", lintRowCount))
        End If

    End Sub

    '

    Protected Sub btnAExecute_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' --- Validation ---
        If txtAUserID.Text.Trim = String.Empty OrElse txtAPasscode.Text.Trim = String.Empty Then
            ShowError(lblAResult, "Please complete all fields!")
            Return
        End If

        If IsNumeric(txtAPasscode.Text) = False Then
            ShowError(lblAResult, "Invalid Token Passcode!")
            Return
        End If
        ' --- End of Validation ---

        InitRSA()

        Dim c1 As New LoginCommand
        c1.authenticationMethodId = SecurIDAuthenticationConstants.METHOD_ID

        c1.identitySourceGuid = IdentitySource.guid

        Dim lstParm As New List(Of FieldParameterDTO)
        Dim p As FieldParameterDTO = Nothing

        If hfASessionId.Value <> String.Empty Then
            ' Next Token Mode

            ' --- Validation ---
            If txtANextPasscode.Text.Trim = String.Empty Then
                ShowError(lblAResult, "Please complete all fields!")
                Return
            End If
            ' --- End of Validation ---

            c1.sessionId = hfASessionId.Value
            hfASessionId.Value = String.Empty

            p = New FieldParameterDTO
            p.promptKey = "NEXT_TOKENCODE"
            p.value = txtANextPasscode.Text.Trim

            lstParm.Add(p)

        Else
            ' Normal authentication
            p = New FieldParameterDTO
            p.promptKey = AuthenticationConstants.PRINCIPAL_ID
            p.value = txtAUserID.Text.Trim

            lstParm.Add(p)

            p = New FieldParameterDTO
            p.promptKey = AuthenticationConstants.PASSCODE
            p.value = txtAPasscode.Text.Trim

            lstParm.Add(p)

        End If

        c1.parameters = lstParm.ToArray

        Dim t1 As DateTime = DateTime.Now

        Try
            c1.execute()

        Catch ex As AuthenticationCommandException
            ' Check principal
            Dim c2 As New SearchPrincipalsCommand

            c2.filter = Filter.equal(PrincipalDTO._LOGINUID, txtAUserID.Text.Trim)
            c2.limit = Integer.MaxValue
            c2.identitySourceGuid = IdentitySource.guid
            c2.securityDomainGuid = IdentitySource.securityDomainGuid
            c2.execute()

            If c2.principals.Length = 0 Then
                ShowError(lblAResult, String.Format("User {0} not found!", txtAUserID.Text.Trim))
                Return
            End If

            ' Unknown error
            Throw

        End Try

        Dim t2 As DateTime = DateTime.Now

        Dim strTimeUsed As String = String.Empty

        If IsEnablePerformanceMonitor() Then
            strTimeUsed = String.Format(" ({0}ms)", CInt((t2.Ticks - t1.Ticks) / 10000))
        End If

        Select Case c1.authenticationState
            Case "authenticated"
                ShowComplete(lblAResult, String.Format("Authenticated{0}", strTimeUsed))
                txtAUserID.Enabled = True
                txtAPasscode.Enabled = True
                txtAPasscode.Text = String.Empty
                txtANextPasscode.Text = String.Empty
                tdANextPasscodeText.Style("display") = "none"
                tdANextPasscode.Style("display") = "none"
                btnACancel.Visible = False
                lblACancelSpace.Visible = False

                Dim c3 As New LogoutCommand
                c3.sessionId = c1.sessionId
                c3.execute()

            Case "failed"
                ShowError(lblAResult, String.Format("Failed{0}", strTimeUsed))
                txtAUserID.Enabled = True
                txtAPasscode.Enabled = True
                tdANextPasscodeText.Style("display") = "none"
                tdANextPasscode.Style("display") = "none"
                btnACancel.Visible = False
                lblACancelSpace.Visible = False

            Case "in_progress"
                ShowComplete(lblAResult, String.Format("Please supply next passcode{0}", strTimeUsed))
                hfASessionId.Value = c1.sessionId
                txtAUserID.Enabled = False
                txtAPasscode.Enabled = False
                tdANextPasscodeText.Style("display") = "inline"
                tdANextPasscode.Style("display") = "inline"
                btnACancel.Visible = True
                lblACancelSpace.Visible = True
                txtANextPasscode.Text = String.Empty

                ScriptManager.GetCurrent(Me.Page).SetFocus(txtANextPasscode)

        End Select

    End Sub

    Protected Sub btnACancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ShowComplete(lblAResult, "Cancelled by user")
        txtAUserID.Enabled = True
        txtAPasscode.Enabled = True
        tdANextPasscodeText.Style("display") = "none"
        tdANextPasscode.Style("display") = "none"
        btnACancel.Visible = False
        lblACancelSpace.Visible = False
        hfASessionId.Value = String.Empty
    End Sub

    '

    ''' <summary>
    ''' !! Deployment Notice !!
    ''' You must place the following dll (RSAAuthAgent.dll) together with the acecInt.dll and sdmsg.dll into the specific project bin folder.
    ''' Alternatively, you may change the following path to an absolute path.
    ''' </summary>
    <DllImport("RSAAuthAgent.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Function auth(ByVal strConfig As String, ByVal strUserID As String, ByVal strPasscode As String, ByRef strStackTrace As StringBuilder) As Integer
    End Function

    <DllImport("RSAAuthAgent.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Private Shared Sub clearRSA()
    End Sub

    Protected Sub btnACExecute_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' --- Validation ---
        If txtACUserID.Text.Trim = String.Empty OrElse txtACPasscode.Text.Trim = String.Empty Then
            ShowError(lblACResult, "Please complete all fields!")
            Return
        End If

        If IsNumeric(txtACPasscode.Text) = False Then
            ShowError(lblACResult, "Invalid Token Passcode!")
            Return
        End If
        ' --- End of Validation ---

        Dim lstrStackTrace As String = String.Empty

        Dim t1, t2 As DateTime
        Dim lintResult As Integer
        Dim strTimeUsed As String = String.Empty
        Dim strMethod As String = String.Empty
        Dim strAgentConfPath As String = String.Empty

        strAgentConfPath = ConfigurationManager.AppSettings(String.Format("RSA{0}AgentConfPath", rblLink.SelectedValue))

        t1 = DateTime.Now

        Dim lsbStackTrace As New StringBuilder(200)

        lintResult = auth(strAgentConfPath, txtACUserID.Text.Trim, txtACPasscode.Text.Trim, lsbStackTrace)
        lstrStackTrace = lsbStackTrace.ToString

        t2 = DateTime.Now

        If IsEnablePerformanceMonitor() Then
            strTimeUsed = String.Format(" ({0}ms)", CInt((t2.Ticks - t1.Ticks) / 10000))
        End If

        If lintResult = "0" Then
            txtACPasscode.Text = String.Empty

            ShowComplete(lblACResult, String.Format("Authenticated{0}", strTimeUsed))

        Else
            If lstrStackTrace <> String.Empty Then
                ShowError(lblACResult, String.Format("Failed (ReturnCode={0},StackTrace={1}){2}", lintResult, lstrStackTrace, strTimeUsed))
            Else
                ShowError(lblACResult, String.Format("Failed (ReturnCode={0}){1}", lintResult, strTimeUsed))
            End If

        End If

    End Sub

    Protected Sub btnAWExecute_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' --- Validation ---
        If txtAWUserID.Text.Trim = String.Empty OrElse txtAWPasscode.Text.Trim = String.Empty Then
            ShowError(lblAWResult, "Please complete all fields!")
            Return
        End If

        If IsNumeric(txtAWPasscode.Text) = False Then
            ShowError(lblAWResult, "Invalid Token Passcode!")
            Return
        End If
        ' --- End of Validation ---

        'set up remote certificate validation
        ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertificate)

        Dim lstrStackTrace As String = String.Empty

        Dim t1, t2 As DateTime
        Dim lintResult As Integer
        Dim strTimeUsed As String = String.Empty
        Dim strMethod As String = String.Empty
        Dim strAgentConfPath As String = String.Empty

        strAgentConfPath = ConfigurationManager.AppSettings("RSAAgentWSConfPath")

        Dim ws As New AuthService
        Dim strResult As String

        ws.Url = ConfigurationManager.AppSettings("RSAAgentWSLink")

        t1 = DateTime.Now

        strResult = ws.Authenticate(strAgentConfPath, txtAWUserID.Text.Trim, txtAWPasscode.Text.Trim)
        t2 = DateTime.Now

        If Not strResult.Contains("|||") Then
            lintResult = strResult
        Else
            lintResult = strResult.Split(New String() {"|||"}, StringSplitOptions.None)(0)
            lstrStackTrace = strResult.Split(New String() {"|||"}, StringSplitOptions.None)(1)
        End If


        If IsEnablePerformanceMonitor() Then
            strTimeUsed = String.Format(" ({0}ms)", CInt((t2.Ticks - t1.Ticks) / 10000))
        End If

        If lintResult = "0" Then
            txtAWPasscode.Text = String.Empty

            ShowComplete(lblAWResult, String.Format("Authenticated{0}", strTimeUsed))

        Else
            If lstrStackTrace <> String.Empty Then
                ShowError(lblAWResult, String.Format("Failed (ReturnCode={0},StackTrace={1}){2}", lintResult, lstrStackTrace, strTimeUsed))
            Else
                ShowError(lblAWResult, String.Format("Failed (ReturnCode={0}){1}", lintResult, strTimeUsed))
            End If

        End If


    End Sub
    '

    Protected Sub btnAPExecute_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' --- Validation ---
        If txtAPUserID.Text.Trim = String.Empty OrElse txtAPToken.Text.Trim = String.Empty Then
            ShowError(lblAPResult, "Please complete all fields!")
            Return
        End If
        ' --- End of Validation ---

        mvAP.SetActiveView(vAPConfirm)

    End Sub

    Protected Sub btnAPEYes_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' --- Validation ---
        If txtAPUserID.Text.Trim = String.Empty OrElse txtAPToken.Text.Trim = String.Empty Then
            ShowError(lblAPResult, "Please complete all fields!")
            mvAP.SetActiveView(vAPButton)
            Return
        End If

        InitRSA()

        ' User already exist
        Dim c1 As New SearchPrincipalsCommand
        c1.filter = Filter.equal(PrincipalDTO._LOGINUID, txtAPUserID.Text.Trim)
        c1.limit = Integer.MaxValue
        c1.identitySourceGuid = IdentitySource.guid
        c1.securityDomainGuid = IdentitySource.securityDomainGuid

        c1.execute()

        If c1.principals.Length <> 0 Then
            ShowError(lblAPResult, "User ID already exists!")
            mvAP.SetActiveView(vAPButton)
            Return
        End If

        If IsNumeric(txtAPToken.Text) = False Then
            ShowError(lblAPResult, "Invalid Token Passcode!")
            mvAP.SetActiveView(vAPButton)
            Return
        End If

        ' Token already assigned
        Dim c2 As New LookupTokenCommand

        c2.serialNumber = ConvertTokenNumber(txtAPToken.Text)

        Try
            c2.execute()

        Catch ex As DataNotFoundException
            ShowError(lblAPResult, "Token not found!")
            mvAP.SetActiveView(vAPButton)
            Return

        End Try

        If c2.token.assignedUserId <> String.Empty Then
            ShowError(lblAPResult, String.Format("Token already assigned to {0}!", c2.token.assignedUserId))
            mvAP.SetActiveView(vAPButton)
            Return
        End If
        ' --- End of Validation ---

        ' Add Principal
        Dim p As New PrincipalDTO
        p.userID = txtAPUserID.Text.Trim
        p.firstName = "NA"
        p.lastName = "NA"
        p.password = "eHSSPPass1234!"

        p.enabled = True
        p.accountStartDate = DateTime.Now
        p.canBeImpersonated = False
        p.trustToImpersonate = False

        p.securityDomainGuid = IdentitySource.securityDomainGuid
        p.identitySourceGuid = IdentitySource.guid
        p.passwordExpired = False

        Dim c3 As New AddPrincipalsCommand
        c3.principals = New PrincipalDTO() {p}

        Try
            c3.execute()
        Catch ex As DuplicateDataException
            ShowError(lblAPResult, "User ID already exists!")
            mvAP.SetActiveView(vAPButton)
            Return
        End Try

        ' Enquire the Principal again to get GUID
        Dim c4 As New SearchPrincipalsCommand
        c4.filter = Filter.equal(PrincipalDTO._LOGINUID, txtAPUserID.Text.Trim)
        c4.limit = Integer.MaxValue
        c4.identitySourceGuid = IdentitySource.guid
        c4.securityDomainGuid = IdentitySource.securityDomainGuid

        c4.execute()

        ' Assign Token
        Dim c5 As New LinkTokensWithPrincipalCommand
        c5.tokenGuids = New String() {c2.token.id}
        c5.principalGuid = c4.principals(0).guid

        Try
            c5.execute()

        Catch ex As Exception
            ' Exception, rollback to delete the principal
            Dim c6 As New DeletePrincipalsCommand
            c6.guids = New String() {c5.principalGuid}
            c6.identitySourceGuid = IdentitySource.guid
            c6.execute()

            ShowError(lblAPResult, String.Format("Exception: {0}", ex.Message))
            Return

        End Try

        ShowComplete(lblAPResult, "OK")
        mvAP.SetActiveView(vAPButton)

    End Sub

    Protected Sub btnAPENo_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        lblAPResult.Text = String.Empty
        mvAP.SetActiveView(vAPButton)
    End Sub

    '

    Protected Sub btnUPFind_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' --- Validation ---
        If txtUPUserID.Text.Trim = String.Empty Then
            ShowError(lblUPResult, "Please complete all fields!")
            Return
        End If

        InitRSA()

        Dim c1 As New SearchPrincipalsCommand

        c1.filter = Filter.equal(PrincipalDTO._LOGINUID, txtUPUserID.Text.Trim)
        c1.limit = Integer.MaxValue
        c1.identitySourceGuid = IdentitySource.guid
        c1.securityDomainGuid = IdentitySource.securityDomainGuid

        c1.execute()

        If c1.principals.Length = 0 Then
            ShowError(lblUPResult, "Principal not found!")
            Return
        End If

        ' --- End of Validation ---
        Dim p As PrincipalDTO = c1.principals(0)

        txtUPUserID.Enabled = False
        btnUPFind.Visible = False
        lblUPResult.Text = String.Empty
        trUPC.Visible = True

        hfUPCPrincipalGuid.Value = p.guid
        hfUPCPrincipalRowVersion.Value = p.rowVersion
        cboUPCEnable.Checked = p.enabled
        cboUPCLockout.Checked = p.lockoutStatus

        If p.lockoutStatus Then
            trUPCLockout.Visible = True
        Else
            trUPCLockout.Visible = False
        End If

        ' Token
        Dim c2 As New ListTokensByPrincipalCommand
        c2.principalId = p.guid
        c2.execute()

        Dim t2 As ListTokenDTO = Nothing

        For Each t As ListTokenDTO In c2.tokenDTOs
            If t.replacementMode <> TokenDTO._IS_REPLACEMENT_TKN Then
                t2 = t
                Exit For
            End If
        Next

        If Not IsNothing(t2) Then
            txtUPCToken.Text = t2.serialNumber
            hfUPCToken.Value = t2.serialNumber
            cboUPCTokenEnable.Checked = t2.enable
            hfUPCTokenEnable.Value = IIf(t2.enable, "Y", "N")
        End If

    End Sub

    Protected Sub btnUPCCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        txtUPUserID.Enabled = True
        btnUPFind.Visible = True
        lblUPResult.Text = String.Empty
        trUPC.Visible = False
    End Sub

    Protected Sub btnUPCExecute_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        mvUPC.SetActiveView(vUPCConfirm)
    End Sub

    Protected Sub btnUPCEYes_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' --- Validation ---
        If IsNumeric(txtUPCToken.Text.Trim) = False Then
            ShowError(lblUPResult, "Invalid Token Serial No.!")
            mvUPC.SetActiveView(vUPCButton)
            Return
        End If

        InitRSA()

        Dim c1 As New LookupTokenCommand
        c1.serialNumber = ConvertTokenNumber(txtUPCToken.Text)

        Try
            c1.execute()

        Catch ex As DataNotFoundException
            ShowError(lblUPResult, "Token not found!")
            mvUPC.SetActiveView(vUPCButton)
            Return

        End Try
        ' --- End of Validation ---

        Dim c2 As New UpdatePrincipalCommand
        c2.identitySourceGuid = IdentitySource.guid

        Dim u As New UpdatePrincipalDTO
        u.guid = hfUPCPrincipalGuid.Value

        ' Copy the rowVersion to satisfy optimistic locking requirements
        u.rowVersion = CInt(hfUPCPrincipalRowVersion.Value)

        ' collect all modifications here
        Dim lstM As New List(Of ModificationDTO)
        Dim m As ModificationDTO

        ' Enable
        m = New ModificationDTO
        m.operation = ModificationDTO._REPLACE_ATTRIBUTE
        m.name = PrincipalDTO._ENABLE_FLAG
        m.values = New Object() {cboUPCEnable.Checked}
        lstM.Add(m)

        ' Lockout
        If cboUPCLockout.Checked = False Then
            m = New ModificationDTO
            m.operation = ModificationDTO._REPLACE_ATTRIBUTE
            m.name = PrincipalDTO._LOCKOUT_FLAG
            m.values = New Object() {cboUPCLockout.Checked}
            lstM.Add(m)

        End If

        u.modifications = lstM.ToArray()
        c2.principalModification = u
        c2.execute()

        ' Change token
        If ConvertTokenNumber(txtUPCToken.Text) <> ConvertTokenNumber(hfUPCToken.Value) Then
            ' Unlink old token
            Dim c4 As New LookupTokenCommand
            c4.serialNumber = ConvertTokenNumber(hfUPCToken.Value)

            Try
                c4.execute()

            Catch ex As DataNotFoundException
                Throw New Exception(String.Format("btnUPCExecute_Click: Unable to find token {0}", c4.serialNumber))

            End Try

            Dim c5 As New UnlinkTokensFromPrincipalsCommand

            c5.tokenGuids = New String() {c4.token.id}
            c5.execute()

            ' Link new token
            Dim c6 As New LinkTokensWithPrincipalCommand
            c6.tokenGuids = New String() {c1.token.id}
            c6.principalGuid = hfUPCPrincipalGuid.Value

            c6.execute()

        Else
            If IIf(cboUPCTokenEnable.Checked, "Y", "N") <> hfUPCTokenEnable.Value Then
                Dim c7 As New LookupTokenCommand

                c7.serialNumber = hfUPCToken.Value

                Try
                    c7.execute()

                Catch ex As DataNotFoundException
                    Throw New Exception(String.Format("btnUPCExecute_Click: Unable to find token {0}", c7.serialNumber))

                End Try

                Dim c8 As New EnableTokensCommand
                c8.tokenGuids = New String() {c7.token.id}
                c8.enable = cboUPCTokenEnable.Checked

                c8.execute()

            End If

        End If


        txtUPUserID.Enabled = True
        btnUPFind.Visible = True
        trUPC.Visible = False
        ShowComplete(lblUPResult, "OK")
        mvUPC.SetActiveView(vUPCButton)

    End Sub

    Protected Sub btnUPCENo_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        mvUPC.SetActiveView(vUPCButton)
    End Sub

    '

    Protected Sub btnDPExecute_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' --- Validation ---
        If txtDPUserID.Text.Trim = String.Empty Then
            ShowError(lblDPResult, "Please complete all fields!")
            Return
        End If
        ' --- End of Validation ---

        mvDP.SetActiveView(vDPConfirm)

    End Sub

    Protected Sub btnDPEYes_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' --- Validation ---
        If txtDPUserID.Text.Trim = String.Empty Then
            ShowError(lblDPResult, "Please complete all fields!")
            mvDP.SetActiveView(vDPButton)
            Return
        End If

        InitRSA()

        Dim c1 As New SearchPrincipalsCommand

        c1.filter = Filter.equal(PrincipalDTO._LOGINUID, txtDPUserID.Text.Trim)
        c1.limit = Integer.MaxValue
        c1.identitySourceGuid = IdentitySource.guid
        c1.securityDomainGuid = IdentitySource.securityDomainGuid

        c1.execute()

        If c1.principals.Length = 0 Then
            ShowError(lblDPResult, "Principal not found!")
            mvDP.SetActiveView(vDPButton)
            Return
        End If

        ' --- End of Validation ---

        Dim c2 As New DeletePrincipalsCommand
        c2.guids = New String() {c1.principals(0).guid}
        c2.identitySourceGuid = IdentitySource.guid
        c2.execute()

        ShowComplete(lblDPResult, "OK")
        mvDP.SetActiveView(vDPButton)

    End Sub

    Protected Sub btnDPENo_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        lblDPResult.Text = String.Empty
        mvDP.SetActiveView(vDPButton)
    End Sub
    '

    Protected Sub btnCTExecute_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' --- Validation ---
        If txtCTUserID.Text.Trim = String.Empty Then
            ShowError(lblCTResult, "Please complete all fields!")
            Return
        End If
        ' --- End of Validation ---

        mvCT.SetActiveView(vCTConfirm)

    End Sub

    Protected Sub btnCTEYes_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' --- Validation ---
        If txtCTUserID.Text.Trim = String.Empty Then
            ShowError(lblCTResult, "Please complete all fields!")
            mvCT.SetActiveView(vCTButton)
            Return
        End If

        InitRSA()

        Dim c1 As New SearchPrincipalsCommand

        c1.filter = Filter.equal(PrincipalDTO._LOGINUID, txtCTUserID.Text.Trim)
        c1.limit = Integer.MaxValue
        c1.identitySourceGuid = IdentitySource.guid
        c1.securityDomainGuid = IdentitySource.securityDomainGuid

        c1.execute()

        If c1.principals.Length = 0 Then
            ShowError(lblCTResult, "Principal not found!")
            mvCT.SetActiveView(vCTButton)
            Return
        End If

        ' --- End of Validation ---

        ' Clear Lockout status
        Dim c2 As New UpdatePrincipalCommand
        c2.identitySourceGuid = IdentitySource.guid

        Dim u As New UpdatePrincipalDTO
        u.guid = c1.principals(0).guid

        ' Copy the rowVersion to satisfy optimistic locking requirements
        u.rowVersion = c1.principals(0).rowVersion

        ' Collect all modifications here
        Dim llstM As New List(Of ModificationDTO)
        Dim m As ModificationDTO

        ' Lockout
        m = New ModificationDTO
        m.operation = ModificationDTO._REPLACE_ATTRIBUTE
        m.name = PrincipalDTO._LOCKOUT_FLAG
        m.values = New Object() {False}
        llstM.Add(m)

        u.modifications = llstM.ToArray()
        c2.principalModification = u
        c2.execute()

        ' Clear Token Fail Count
        Dim c3 As New ClearBadTokenCountCommand
        c3.principalGuid = c1.principals(0).guid
        c3.execute()

        ShowComplete(lblCTResult, "OK")
        mvCT.SetActiveView(vCTButton)

    End Sub

    Protected Sub btnCTENo_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        lblCTResult.Text = String.Empty
        mvCT.SetActiveView(vCTButton)
    End Sub

    '

    Protected Sub btnLTExecute_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' --- Init ---
        btnLTClear_Click(Nothing, Nothing)
        ' --- End of Init ---

        ' --- Validation ---
        If txtLTToken.Text.Trim = String.Empty Then
            ShowError(lblLTResult, "Please complete all fields!")
            Return
        End If
        ' --- End of Validation ---

        InitRSA()

        Dim lstToken As New List(Of TokenDTO)

        If cboLTAdvMode.Checked = False Then
            ' Single query

            ' --- Validation ---
            If IsNumeric(txtLTToken.Text) = False Then
                ShowError(lblLTResult, "Invalid Token Serial No.!")
                Return
            End If
            ' --- End of Validation ---

            Dim c1 As New LookupTokenCommand
            c1.serialNumber = ConvertTokenNumber(txtLTToken.Text)

            Try
                c1.execute()

            Catch ex As DataNotFoundException
                ShowError(lblLTResult, "Token not found!")
                Return

            End Try

            lstToken.Add(c1.token)

        Else
            ' Multiple query
            For Each strToken As String In txtLTToken.Text.Trim.Split(New String() {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                ' --- Validation ---
                If IsNumeric(strToken) = False Then
                    Dim t As New TokenDTO
                    t.serialNumber = strToken
                    t.id = String.Empty

                    lstToken.Add(t)

                    Continue For

                End If
                ' --- End of Validation ---

                Dim c1 As New LookupTokenCommand
                c1.serialNumber = ConvertTokenNumber(strToken)

                Try
                    c1.execute()
                    lstToken.Add(c1.token)

                Catch ex As DataNotFoundException
                    Dim t As New TokenDTO
                    t.serialNumber = c1.serialNumber
                    t.id = String.Empty

                    lstToken.Add(t)

                End Try

            Next

        End If

        gvLT.DataSource = lstToken
        gvLT.DataBind()
        gvLT.Visible = True

        ShowComplete(lblLTResult, "OK")

    End Sub

    Protected Sub btnLTClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        lblLTResult.Text = String.Empty
        gvLT.Visible = False
    End Sub

    Protected Sub gvLT_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim t As TokenDTO = e.Row.DataItem

            ' Serial No.
            Dim lblLTGToken As Label = e.Row.FindControl("lblLTGToken")
            lblLTGToken.Text = t.serialNumber

            ' Token exist or not
            If t.id = String.Empty Then
                lblLTGToken.ForeColor = Drawing.Color.Red
                lblLTGToken.ToolTip = "Not Found"
                lblLTGToken.Style("cursor") = "not-allowed"

            Else
                ' Enable
                Dim lblLTGEnable As Label = e.Row.FindControl("lblLTGEnable")

                If t.tokenEnabled Then
                    lblLTGEnable.Text = "Yes"
                Else
                    lblLTGEnable.Text = "No"
                    lblLTGEnable.ForeColor = Drawing.Color.Red
                End If

                ' User ID
                DirectCast(e.Row.FindControl("lblLTGUserID"), Label).Text = t.assignedUserId

                ' Fail Count
                Dim lblLTGFailCount As Label = e.Row.FindControl("lblLTGFailCount")
                lblLTGFailCount.Text = t.badTokenCodeCount

                If t.badTokenCodeCount = 0 Then
                    lblLTGFailCount.ForeColor = Drawing.Color.LightGray
                End If

                ' Next Token Mode
                Dim lblLTGNextTokenMode As Label = e.Row.FindControl("lblLTGNextTokenMode")

                If t.nextTokenCodeMode Then
                    lblLTGNextTokenMode.Text = "Yes"
                    lblLTGNextTokenMode.ForeColor = Drawing.Color.Red
                    lblLTGFailCount.ForeColor = Drawing.Color.Red
                Else
                    lblLTGNextTokenMode.Text = "No"
                End If

                ' Replacement Mode
                Dim lblLTGReplacementMode As Label = e.Row.FindControl("lblLTGReplacementMode")
                lblLTGReplacementMode.Text = t.replacementMode

                If t.replacementMode = 0 Then
                    lblLTGReplacementMode.ForeColor = Drawing.Color.LightGray
                End If

                ' Replacement Token
                DirectCast(e.Row.FindControl("lblLTGReplacementToken"), Label).Text = t.replaceTknSN

            End If

        End If

    End Sub

    Protected Sub cboLTAdvMode_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If cboLTAdvMode.Checked Then
            txtLTToken.TextMode = TextBoxMode.MultiLine
            txtLTToken.Height = 80
            txtLTToken.Attributes("token") = "2"

        Else
            txtLTToken.TextMode = TextBoxMode.SingleLine
            txtLTToken.Height = 14
            txtLTToken.Attributes("token") = "1"

        End If

    End Sub

    '

    Protected Sub btnRTExecute_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' --- Validation ---
        If txtRTOldToken.Text.Trim = String.Empty OrElse txtRTReplaceToken.Text.Trim = String.Empty Then
            ShowError(lblRTResult, "Please complete all fields!")
            Return
        End If
        ' --- End of Validation ---

        mvRT.SetActiveView(vRTConfirm)

    End Sub

    Protected Sub btnRTEYes_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' --- Validation ---
        If txtRTOldToken.Text.Trim = String.Empty OrElse txtRTReplaceToken.Text.Trim = String.Empty Then
            ShowError(lblRTResult, "Please complete all fields!")
            mvRT.SetActiveView(vRTButton)
            Return
        End If

        If txtRTOldToken.Text.Trim = txtRTReplaceToken.Text.Trim Then
            ShowError(lblRTResult, "No. cannot be the same!")
            mvRT.SetActiveView(vRTButton)
            Return
        End If

        If IsNumeric(txtRTOldToken.Text) = False Then
            ShowError(lblRTResult, "Invalid Token Serial No.!")
            mvRT.SetActiveView(vRTButton)
            Return
        End If

        If IsNumeric(txtRTReplaceToken.Text) = False Then
            ShowError(lblRTResult, "Invalid Replacement Token Serial No.!")
            mvRT.SetActiveView(vRTButton)
            Return
        End If

        InitRSA()

        ' Validate current token
        Dim c1 As New LookupTokenCommand
        c1.serialNumber = ConvertTokenNumber(txtRTOldToken.Text)

        Try
            c1.execute()
        Catch ex As DataNotFoundException
            ShowError(lblRTResult, "Token Serial No. not found!")
            mvRT.SetActiveView(vRTButton)
            Return
        End Try

        If c1.token.assignedUserId = String.Empty Then
            ShowError(lblRTResult, "Current Token is not assigned to any user!")
            mvRT.SetActiveView(vRTButton)
            Return
        End If

        If c1.token.replacementMode = TokenDTO._IS_REPLACEMENT_TKN Then
            ShowError(lblRTResult, String.Format("Current Token is already a replacement token for {0}!", c1.token.replaceTknSN))
            mvRT.SetActiveView(vRTButton)
            Return
        End If

        ' Validate replacement token
        Dim c2 As New LookupTokenCommand
        c2.serialNumber = ConvertTokenNumber(txtRTReplaceToken.Text)

        Try
            c2.execute()
        Catch ex As DataNotFoundException
            ShowError(lblRTResult, "Replacement Token Serial No. not found!")
            mvRT.SetActiveView(vRTButton)
            Return
        End Try

        If c2.token.assignedUserId <> String.Empty AndAlso c2.token.assignedUserId <> c1.token.assignedUserId Then
            ShowError(lblRTResult, String.Format("Replacement Token is assigned to {0}!", c2.token.assignedUserId))
            mvRT.SetActiveView(vRTButton)
            Return
        End If
        ' --- End of Validation ---

        ' Unassign the previous replacement token
        If c1.token.replacementMode = TokenDTO._HAS_REPLACEMENT_TKN Then
            Dim c4 As New LookupTokenCommand
            c4.serialNumber = ConvertTokenNumber(c1.token.replaceTknSN)
            c4.execute()

            Dim c5 As New UnlinkTokensFromPrincipalsCommand
            c5.tokenGuids = New String() {c4.token.id}
            c5.execute()

        End If

        ' Replace token
        Dim c3 As New ReplaceTokensCommand
        Dim r3 As New ReplaceTokenDTO
        r3.replacingTokenGuid = c1.token.id
        r3.replacementTokenGuid = c2.token.id

        c3.repTknDTO = New ReplaceTokenDTO() {r3}
        c3.execute()

        ShowComplete(lblRTResult, "OK")
        mvRT.SetActiveView(vRTButton)

    End Sub

    Protected Sub btnRTENo_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        lblRTResult.Text = String.Empty
        mvRT.SetActiveView(vRTButton)
    End Sub

    '

    Protected Sub btnRUDExecute_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' --- Validation ---
        If txtRUDData.Text.Trim = String.Empty Then
            ShowError(lblRUDResult, "Please complete all fields!")
            Return
        End If
        ' --- End of Validation ---

        mvRUD.SetActiveView(vRUDConfirm)

    End Sub

    Protected Sub btnRUDEYes_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dt As New DataTable
        dt.Columns.Add("PrincipalID", GetType(String))
        dt.Columns.Add("Token", GetType(String))
        dt.Columns.Add("TokenGuid", GetType(String))
        dt.Columns.Add("TokenUserID", GetType(String))
        dt.Columns.Add("ReplacementToken", GetType(String))
        dt.Columns.Add("ReplacementTokenGuid", GetType(String))
        dt.Columns.Add("ReplacementTokenUserID", GetType(String))

        InitRSA()

        ' --- Validation ---
        For Each strLine As String In txtRUDData.Text.Trim.Split(New String() {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
            ' Not 3 items each line
            If strLine.Split(",".ToCharArray).Length <> 3 Then
                ShowError(lblRUDResult, String.Format("Format error near {0}", strLine))
                mvRUD.SetActiveView(vRUDButton)
                Return
            End If

            Dim aryLine As String() = strLine.Split(",".ToCharArray)

            ' Empty Principal ID or Token Serial No.
            If aryLine(0).Trim = String.Empty OrElse aryLine(1).Trim = String.Empty Then
                ShowError(lblRUDResult, String.Format("Format error near {0}", strLine))
                mvRUD.SetActiveView(vRUDButton)
                Return
            End If

            ' Token exist
            Dim c1 As New LookupTokenCommand
            c1.serialNumber = ConvertTokenNumber(aryLine(1).Trim)

            Try
                c1.execute()

            Catch ex As DataNotFoundException
                ShowError(lblRUDResult, String.Format("Token Serial No. {0} not found", aryLine(1).Trim))
                mvRUD.SetActiveView(vRUDButton)
                Return

            End Try

            ' Replacement Token exist
            Dim strReplacementTokenGuid As String = String.Empty
            Dim strReplacementTokenUserID As String = String.Empty

            If aryLine(2).Trim <> String.Empty Then
                Dim c2 As New LookupTokenCommand
                c2.serialNumber = ConvertTokenNumber(aryLine(2).Trim)

                Try
                    c2.execute()
                    strReplacementTokenGuid = c2.token.id
                    strReplacementTokenUserID = c2.token.assignedUserId

                Catch ex As DataNotFoundException
                    ShowError(lblRUDResult, String.Format("Replacement Token Serial No. {0} not found", aryLine(2).Trim))
                    mvRUD.SetActiveView(vRUDButton)
                    Return

                End Try

            End If

            Dim dr As DataRow = dt.NewRow
            dr("PrincipalID") = aryLine(0).Trim
            dr("Token") = aryLine(1).Trim
            dr("TokenGuid") = c1.token.id
            dr("TokenUserID") = IIf(IsNothing(c1.token.assignedUserId), String.Empty, c1.token.assignedUserId)
            dr("ReplacementToken") = aryLine(2).Trim
            dr("ReplacementTokenGuid") = strReplacementTokenGuid
            dr("ReplacementTokenUserID") = IIf(IsNothing(strReplacementTokenUserID), String.Empty, strReplacementTokenUserID)
            dt.Rows.Add(dr)

        Next

        ' --- End of Validation ---

        Try
            ' Delete all principals and unlink all tokens
            For Each dr As DataRow In dt.Rows
                ' Delete principal if exist
                Dim c3 As New SearchPrincipalsCommand

                c3.filter = Filter.equal(PrincipalDTO._LOGINUID, dr("PrincipalID"))
                c3.limit = Integer.MaxValue
                c3.identitySourceGuid = IdentitySource.guid
                c3.securityDomainGuid = IdentitySource.securityDomainGuid

                c3.execute()

                If c3.principals.Length > 0 Then
                    Dim c4 As New DeletePrincipalsCommand
                    c4.guids = New String() {c3.principals(0).guid}
                    c4.identitySourceGuid = IdentitySource.guid
                    c4.execute()

                End If

                Try
                    Dim c5 As New UnlinkTokensFromPrincipalsCommand

                    c5.tokenGuids = New String() {dr("TokenGuid")}
                    c5.execute()

                Catch ex As InvalidArgumentException
                End Try

                If dr("ReplacementTokenGuid") <> String.Empty Then
                    Try
                        Dim c6 As New UnlinkTokensFromPrincipalsCommand

                        c6.tokenGuids = New String() {dr("ReplacementTokenGuid")}
                        c6.execute()

                    Catch ex As InvalidArgumentException
                    End Try

                End If

            Next

            ' Insert new data
            For Each dr As DataRow In dt.Rows
                ' Add Principal
                Dim p As New PrincipalDTO
                p.userID = dr("PrincipalID")
                p.firstName = "NA"
                p.lastName = "NA"
                p.password = "eHSSPPass1234!"

                p.enabled = True
                p.accountStartDate = DateTime.Now
                p.canBeImpersonated = False
                p.trustToImpersonate = False

                p.securityDomainGuid = IdentitySource.securityDomainGuid
                p.identitySourceGuid = IdentitySource.guid
                p.passwordExpired = False

                Dim c7 As New AddPrincipalsCommand
                c7.principals = New PrincipalDTO() {p}

                c7.execute()

                ' Enquire the Principal again to get GUID
                Dim c8 As New SearchPrincipalsCommand
                c8.filter = Filter.equal(PrincipalDTO._LOGINUID, dr("PrincipalID"))
                c8.limit = Integer.MaxValue
                c8.identitySourceGuid = IdentitySource.guid
                c8.securityDomainGuid = IdentitySource.securityDomainGuid

                c8.execute()

                ' Assign Token
                Dim c9 As New LinkTokensWithPrincipalCommand
                c9.tokenGuids = New String() {dr("TokenGuid")}
                c9.principalGuid = c8.principals(0).guid

                c9.execute()

                ' Assign Replacement Token
                If dr("ReplacementToken") <> String.Empty Then
                    Dim c10 As New ReplaceTokensCommand
                    Dim r10 As New ReplaceTokenDTO
                    r10.replacingTokenGuid = dr("TokenGuid")
                    r10.replacementTokenGuid = dr("ReplacementTokenGuid")

                    c10.repTknDTO = New ReplaceTokenDTO() {r10}
                    c10.execute()

                End If

            Next

        Catch ex As Exception
            ShowError(lblRUDResult, String.Format("Exception: {0}", ex.ToString))
            mvRUD.SetActiveView(vRUDButton)

            Return

        End Try

        ShowComplete(lblRUDResult, String.Format("{0} records updated", dt.Rows.Count))
        mvRUD.SetActiveView(vRUDButton)

    End Sub

    Protected Sub btnRUDENo_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        lblRUDResult.Text = String.Empty
        mvRUD.SetActiveView(vRUDButton)
    End Sub

    '

    Protected Sub btnAFExecute_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        lblAFResult.Text = String.Empty

        ' I-CRE16-007-02 (Refine system from CheckMarx findings) [Start][Winnie]
        If Hash(txtAFPassword.Text) <> ConfigurationManager.AppSettings("AdvancedFeaturePassword") Then
            ' I-CRE16-007-02 (Refine system from CheckMarx findings) [End][Winnie]
            ShowError(lblAFResult, "Incorrect password!")

            Return

        End If

        mvAF.SetActiveView(vAFShow)

        If rblLink.SelectedValue.Equals("WS") Then

            lblWSConfPath.Text = ConfigurationManager.AppSettings("RSAAgentWSConfPath")
            lblWSLink.Text = ConfigurationManager.AppSettings("RSAAgentWSLink")

            Dim ws As New AuthService

            'Set timeout to 5s
            ws.Timeout = 5000
            ws.Url = lblWSLink.Text
            lblWSAppPool.Text = ws.GetAppPool()

            mvAFShow.SetActiveView(vAFWSShow)
        Else
            mvAFShow.SetActiveView(vAFRSAShow)
        End If

    End Sub

    Protected Sub btnAFHide_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        mvAF.SetActiveView(vAFHide)
    End Sub

    '

    Protected Sub btnLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("Default.aspx")
    End Sub

    '

    Private Sub ShowComplete(ByRef lbl As Label, ByVal strMessage As String)
        lbl.Text = String.Format("{0} ({1})", strMessage, DateTime.Now.ToString("HH:mm:ss"))

        lbl.ForeColor = Drawing.Color.Blue

    End Sub

    Private Sub ShowError(ByRef lbl As Label, ByVal strMessage As String)
        lbl.Text = String.Format("{0} ({1})", strMessage, DateTime.Now.ToString("HH:mm:ss"))

        lbl.ForeColor = Drawing.Color.Red

    End Sub

    Private Function ConvertTokenNumber(ByVal strToken As String) As String
        If strToken.Trim = String.Empty Then Return String.Empty

        Return strToken.Trim.PadLeft(12, "0")
    End Function

    Private Function IsNumeric(ByVal strInput As String) As Boolean
        Return New Regex("^[0-9]+$").IsMatch(strInput.Trim)
    End Function

    Private Function IsEnablePerformanceMonitor() As Boolean
        If Not IsNothing(ConfigurationManager.AppSettings("EnablePerformanceMonitor")) AndAlso ConfigurationManager.AppSettings("EnablePerformanceMonitor") = "Y" Then
            Return True
        Else
            Return False
        End If
    End Function

    '

    Protected Sub ScriptManager1_AsyncPostBackError(ByVal sender As Object, ByVal e As AsyncPostBackErrorEventArgs)
        Session("LastError") = e.Exception.ToString
        Response.Redirect("Error.aspx")
    End Sub

    Private Sub InitRSA()

        ' establish a connected session with given credentials from arguments passed            
        Dim Link As String = ConfigurationManager.AppSettings(String.Format("RSA{0}Link", rblLink.SelectedValue))
        Dim WebLogicUsername As String = ConfigurationManager.AppSettings(String.Format("RSA{0}WebLogicUsername", rblLink.SelectedValue))
        Dim WebLogicPassword As String = ConfigurationManager.AppSettings(String.Format("RSA{0}WebLogicPassword", rblLink.SelectedValue))
        Dim AMUsername As String = ConfigurationManager.AppSettings(String.Format("RSA{0}AMUsername", rblLink.SelectedValue))
        Dim AMPassword As String = ConfigurationManager.AppSettings(String.Format("RSA{0}AMPassword", rblLink.SelectedValue))

        Dim conn As New SOAPCommandTarget(Link, WebLogicUsername, WebLogicPassword)

        If Not conn.Login(AMUsername, AMPassword) Then
            Throw New Exception("Error: Unable to connect to the remote server. Please make sure your credentials are correct.")
        End If

        ' make all commands execute imports this target automatically
        CommandTargetPolicy.setDefaultCommandTarget(conn)

        Dim c1 As New GetIdentitySourcesCommand
        c1.execute()

        IdentitySource = c1.identitySources(0)
    End Sub
End Class
