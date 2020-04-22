Imports System
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Net
Imports System.Net.Security
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

Partial Public Class Compare
    Inherits System.Web.UI.Page

#Region "Fields"

    Private Class Principal

        Public Sub New()
            UserID = String.Empty
            RSAA_Exist = String.Empty
            RSAA_Token = String.Empty
            RSAA_Token_Enable = String.Empty
            RSAA_Token_NTM = String.Empty
            RSAB_Exist = String.Empty
            RSAB_Token = String.Empty
            RSAB_Token_Enable = String.Empty
            RSAB_Token_NTM = String.Empty
        End Sub

        Public UserID As String
        Public RSAA_Exist As String
        Public RSAA_Token As String
        Public RSAA_Token_Enable As String
        Public RSAA_Token_NTM As String
        Public RSAB_Exist As String
        Public RSAB_Token As String
        Public RSAB_Token_Enable As String
        Public RSAB_Token_NTM As String

    End Class

    Private Class Token

        Public Sub New()
            SerialNumber = String.Empty
            RSAA_Exist = String.Empty
            RSAA_Enable = String.Empty
            RSAA_NTM = String.Empty
            RSAA_UserID = String.Empty
            RSAB_Exist = String.Empty
            RSAB_Enable = String.Empty
            RSAB_NTM = String.Empty
            RSAB_UserID = String.Empty
        End Sub

        Public SerialNumber As String
        Public RSAA_Exist As String
        Public RSAA_Enable As String
        Public RSAA_NTM As String
        Public RSAA_UserID As String
        Public RSAB_Exist As String
        Public RSAB_Enable As String
        Public RSAB_NTM As String
        Public RSAB_UserID As String

    End Class

    Private gudtRSAAIdentitySource As IdentitySourceDTO = Nothing
    Private gudtRSABIdentitySource As IdentitySourceDTO = Nothing

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        tHeader.Text = String.Format("Compare RSA {0} vs {1}",
                                   ConfigurationManager.AppSettings("RSASite").Split(New String() {"|||"}, StringSplitOptions.None)(0),
                                   ConfigurationManager.AppSettings("RSASite").Split(New String() {"|||"}, StringSplitOptions.None)(1))

    End Sub

    '

    Protected Sub btnUCompare_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        lblUError.Text = String.Empty
        gvU.Visible = False

        Try
            Dim llstPrincipal As New List(Of Principal)

            ' RSA A
            InitRSAA()

            For Each lstrUserID As String In txtUUserID.Text.Trim.Split(New String() {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                lstrUserID = lstrUserID.Trim
                Dim p As New Principal

                p.UserID = lstrUserID


                Dim c1 As New SearchPrincipalsCommand
                c1.filter = Filter.equal(PrincipalDTO._LOGINUID, lstrUserID)
                c1.limit = Integer.MaxValue
                c1.identitySourceGuid = gudtRSAAIdentitySource.guid
                c1.securityDomainGuid = gudtRSAAIdentitySource.securityDomainGuid

                c1.execute()

                If c1.principals.Length = 0 Then
                    p.RSAA_Exist = "N"
                Else
                    p.RSAA_Exist = "Y"

                    ' Find token
                    Dim c2 As New ListTokensByPrincipalCommand
                    c2.principalId = c1.principals(0).guid
                    c2.execute()

                    If c2.tokenDTOs.Length = 0 Then
                        p.RSAA_Token = "No Token"

                    Else
                        Dim llstToken As New List(Of String)
                        Dim llstTokenEnable As New List(Of String)
                        Dim llstTokenNTM As New List(Of String)

                        For Each t As ListTokenDTO In c2.tokenDTOs
                            If t.replacementMode <> TokenDTO._IS_REPLACEMENT_TKN Then
                                llstToken.Add(t.serialNumber)
                                llstTokenEnable.Add(IIf(t.enable, "Y", "N"))
                                llstTokenNTM.Add(IIf(t.nextTokenMode, "Y", "N"))
                            End If
                        Next

                        If c2.tokenDTOs.Length > 1 Then
                            For Each t As ListTokenDTO In c2.tokenDTOs
                                If t.replacementMode = TokenDTO._IS_REPLACEMENT_TKN Then
                                    llstToken.Add(t.serialNumber)
                                    llstTokenEnable.Add(IIf(t.enable, "Y", "N"))
                                    llstTokenNTM.Add(IIf(t.nextTokenMode, "Y", "N"))
                                End If
                            Next
                        End If

                        p.RSAA_Token = String.Join(",", llstToken.ToArray)
                        p.RSAA_Token_Enable = String.Join(",", llstTokenEnable.ToArray)
                        p.RSAA_Token_NTM = String.Join(",", llstTokenNTM.ToArray)

                    End If

                End If

                llstPrincipal.Add(p)
            Next

            ' RSA B
            InitRSAB()

            For Each p As Principal In llstPrincipal

                Dim c3 As New SearchPrincipalsCommand
                c3.filter = Filter.equal(PrincipalDTO._LOGINUID, p.UserID)
                c3.limit = Integer.MaxValue
                c3.identitySourceGuid = gudtRSABIdentitySource.guid
                c3.securityDomainGuid = gudtRSABIdentitySource.securityDomainGuid

                c3.execute()

                If c3.principals.Length = 0 Then
                    p.RSAB_Exist = "N"
                Else
                    p.RSAB_Exist = "Y"

                    ' Find token
                    Dim c4 As New ListTokensByPrincipalCommand
                    c4.principalId = c3.principals(0).guid
                    c4.execute()

                    If c4.tokenDTOs.Length = 0 Then
                        p.RSAB_Token = "No Token"

                    Else
                        Dim llstToken As New List(Of String)
                        Dim llstTokenEnable As New List(Of String)
                        Dim llstTokenNTM As New List(Of String)

                        For Each t As ListTokenDTO In c4.tokenDTOs
                            If t.replacementMode <> TokenDTO._IS_REPLACEMENT_TKN Then
                                llstToken.Add(t.serialNumber)
                                llstTokenEnable.Add(IIf(t.enable, "Y", "N"))
                                llstTokenNTM.Add(IIf(t.nextTokenMode, "Y", "N"))
                            End If
                        Next

                        If c4.tokenDTOs.Length > 1 Then
                            For Each t As ListTokenDTO In c4.tokenDTOs
                                If t.replacementMode = TokenDTO._IS_REPLACEMENT_TKN Then
                                    llstToken.Add(t.serialNumber)
                                    llstTokenEnable.Add(IIf(t.enable, "Y", "N"))
                                    llstTokenNTM.Add(IIf(t.nextTokenMode, "Y", "N"))
                                End If
                            Next
                        End If

                        p.RSAB_Token = String.Join(",", llstToken.ToArray)
                        p.RSAB_Token_Enable = String.Join(",", llstTokenEnable.ToArray)
                        p.RSAB_Token_NTM = String.Join(",", llstTokenNTM.ToArray)

                    End If

                End If

            Next


            Dim llstFilterPrincipal As New List(Of Principal)

            For Each p As Principal In llstPrincipal
                If cboUHide.Checked Then
                    If p.RSAA_Exist <> p.RSAB_Exist OrElse p.RSAA_Token <> p.RSAB_Token OrElse p.RSAA_Token_Enable <> p.RSAB_Token_Enable OrElse p.RSAA_Token_NTM <> p.RSAB_Token_NTM Then
                        llstFilterPrincipal.Add(p)
                    End If

                Else
                    llstFilterPrincipal.Add(p)

                End If

            Next

            If llstFilterPrincipal.Count = 0 Then
                ShowComplete(lblUError, "No unmatched records")

            Else
                gvU.DataSource = llstFilterPrincipal
                gvU.DataBind()
                gvU.Visible = True

            End If

        Catch ex As Exception
            ShowError(lblUError, ex.ToString)

        End Try

    End Sub



    Protected Sub gvU_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim p As Principal = e.Row.DataItem

            DirectCast(e.Row.FindControl("lblUGUserID"), Label).Text = p.UserID

            Dim lblUGRSAAExist As Label = e.Row.FindControl("lblUGRSAAExist")
            Dim lblUGRSAAToken As Label = e.Row.FindControl("lblUGRSAAToken")
            Dim lblUGRSAATokenEnable As Label = e.Row.FindControl("lblUGRSAATokenEnable")
            Dim lblUGRSAANTM As Label = e.Row.FindControl("lblUGRSAANTM")
            Dim lblUGRSABExist As Label = e.Row.FindControl("lblUGRSABExist")
            Dim lblUGRSABToken As Label = e.Row.FindControl("lblUGRSABToken")
            Dim lblUGRSABTokenEnable As Label = e.Row.FindControl("lblUGRSABTokenEnable")
            Dim lblUGRSABNTM As Label = e.Row.FindControl("lblUGRSABNTM")

            lblUGRSAAExist.Text = p.RSAA_Exist
            lblUGRSAAToken.Text = p.RSAA_Token
            lblUGRSAATokenEnable.Text = p.RSAA_Token_Enable
            lblUGRSAANTM.Text = p.RSAA_Token_NTM
            lblUGRSABExist.Text = p.RSAB_Exist
            lblUGRSABToken.Text = p.RSAB_Token
            lblUGRSABTokenEnable.Text = p.RSAB_Token_Enable
            lblUGRSABNTM.Text = p.RSAB_Token_NTM

            If lblUGRSAAExist.Text <> lblUGRSABExist.Text Then
                lblUGRSAAExist.ForeColor = Drawing.Color.Red
                lblUGRSABExist.ForeColor = Drawing.Color.Red
            End If

            If lblUGRSAAToken.Text <> lblUGRSABToken.Text Then
                lblUGRSAAToken.ForeColor = Drawing.Color.Red
                lblUGRSABToken.ForeColor = Drawing.Color.Red
            End If

            If lblUGRSAATokenEnable.Text <> lblUGRSABTokenEnable.Text Then
                lblUGRSAATokenEnable.ForeColor = Drawing.Color.Red
                lblUGRSABTokenEnable.ForeColor = Drawing.Color.Red
            End If

            If lblUGRSAANTM.Text <> lblUGRSABNTM.Text Then
                lblUGRSAANTM.ForeColor = Drawing.Color.Red
                lblUGRSABNTM.ForeColor = Drawing.Color.Red
            End If

        ElseIf e.Row.RowType = DataControlRowType.Header Then
            Dim strVersionA As String = ConfigurationManager.AppSettings("RSASite").Split(New String() {"|||"}, StringSplitOptions.None)(0)
            Dim strVersionB As String = ConfigurationManager.AppSettings("RSASite").Split(New String() {"|||"}, StringSplitOptions.None)(1)

            Dim lblUGRSAAExistHeader As Label = e.Row.FindControl("lblUGRSAAExistHeader")
            Dim lblUGRSAATokenHeader As Label = e.Row.FindControl("lblUGRSAATokenHeader")
            Dim lblUGRSAATokenEnableHeader As Label = e.Row.FindControl("lblUGRSAATokenEnableHeader")
            Dim lblUGRSAANTMHeader As Label = e.Row.FindControl("lblUGRSAANTMHeader")
            Dim lblUGRSABExistHeader As Label = e.Row.FindControl("lblUGRSABExistHeader")
            Dim lblUGRSABTokenHeader As Label = e.Row.FindControl("lblUGRSABTokenHeader")
            Dim lblUGRSABTokenEnableHeader As Label = e.Row.FindControl("lblUGRSABTokenEnableHeader")
            Dim lblUGRSABNTMHeader As Label = e.Row.FindControl("lblUGRSABNTMHeader")

            If strVersionA <> String.Empty Then
                lblUGRSAAExistHeader.Text = String.Format("RSA{0} Exist", strVersionA)
                lblUGRSAATokenHeader.Text = String.Format("RSA{0} Token", strVersionA)
                lblUGRSAATokenEnableHeader.Text = String.Format("RSA{0} Token Enable", strVersionA)
                lblUGRSAANTMHeader.Text = String.Format("RSA{0} Next Token Mode", strVersionA)
            End If

            If strVersionB <> String.Empty Then
                lblUGRSABExistHeader.Text = String.Format("RSA{0} Exist", strVersionB)
                lblUGRSABTokenHeader.Text = String.Format("RSA{0} Token", strVersionB)
                lblUGRSABTokenEnableHeader.Text = String.Format("RSA{0} Token Enable", strVersionB)
                lblUGRSABNTMHeader.Text = String.Format("RSA{0} Next Token Mode", strVersionB)
            End If

        End If

    End Sub

    Protected Sub lbtnUDownload_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    '

    Protected Sub btnTCompare_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        lblTError.Text = String.Empty
        gvT.Visible = False

        Try
            Dim llstToken As New List(Of Token)

            ' RSA A
            InitRSAA()

            For Each lstrToken As String In txtTToken.Text.Trim.Split(New String() {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                lstrToken = ConvertTokenNumber(lstrToken)
                Dim t As New Token

                t.SerialNumber = lstrToken

                Dim c1 As New LookupTokenCommand
                c1.serialNumber = lstrToken

                Try
                    c1.execute()

                    t.RSAA_Exist = "Y"

                Catch ex As DataNotFoundException
                    t.RSAA_Exist = "N"

                End Try

                If t.RSAA_Exist = "Y" Then
                    t.RSAA_Enable = IIf(c1.token.tokenEnabled, "Y", "N")
                    t.RSAA_NTM = IIf(c1.token.nextTokenCodeMode, "Y", "N")

                    If IsNothing(c1.token.assignedUserId) Then
                        t.RSAA_UserID = "No User"
                    Else
                        t.RSAA_UserID = c1.token.assignedUserId
                    End If

                End If

                llstToken.Add(t)
            Next


            ' RSA B
            InitRSAB()

            For Each t As Token In llstToken
                Dim c2 As New LookupTokenCommand
                c2.serialNumber = t.SerialNumber

                Try
                    c2.execute()

                    t.RSAB_Exist = "Y"

                Catch ex As DataNotFoundException
                    t.RSAB_Exist = "N"

                End Try

                If t.RSAB_Exist = "Y" Then
                    t.RSAB_Enable = IIf(c2.token.tokenEnabled, "Y", "N")
                    t.RSAB_NTM = IIf(c2.token.nextTokenCodeMode, "Y", "N")

                    If IsNothing(c2.token.assignedUserId) Then
                        t.RSAB_UserID = "No User"
                    Else
                        t.RSAB_UserID = c2.token.assignedUserId
                    End If

                End If
            Next

            Dim llstFilterToken As New List(Of Token)

            For Each t As Token In llstToken
                If cboTHide.Checked Then
                    If t.RSAA_Exist <> t.RSAB_Exist OrElse t.RSAA_Enable <> t.RSAB_Enable OrElse t.RSAA_NTM <> t.RSAB_NTM OrElse t.RSAA_UserID <> t.RSAB_UserID Then
                        llstFilterToken.Add(t)
                    End If

                Else
                    llstFilterToken.Add(t)

                End If

            Next

            If llstFilterToken.Count = 0 Then
                ShowComplete(lblTError, "No unmatched records")

            Else
                gvT.DataSource = llstFilterToken
                gvT.DataBind()
                gvT.Visible = True

            End If

        Catch ex As Exception
            ShowError(lblTError, ex.ToString)

        End Try

    End Sub

    Protected Sub gvT_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim t As Token = e.Row.DataItem

            DirectCast(e.Row.FindControl("lblTGSerialNumber"), Label).Text = t.SerialNumber

            Dim lblTGRSAAExist As Label = e.Row.FindControl("lblTGRSAAExist")
            Dim lblTGRSAAEnable As Label = e.Row.FindControl("lblTGRSAAEnable")
            Dim lblTGRSAANTM As Label = e.Row.FindControl("lblTGRSAANTM")
            Dim lblTGRSAAUserID As Label = e.Row.FindControl("lblTGRSAAUserID")
            Dim lblTGRSABExist As Label = e.Row.FindControl("lblTGRSABExist")
            Dim lblTGRSABEnable As Label = e.Row.FindControl("lblTGRSABEnable")
            Dim lblTGRSABNTM As Label = e.Row.FindControl("lblTGRSABNTM")
            Dim lblTGRSABUserID As Label = e.Row.FindControl("lblTGRSABUserID")

            lblTGRSAAExist.Text = t.RSAA_Exist
            lblTGRSAAEnable.Text = t.RSAA_Enable
            lblTGRSAANTM.Text = t.RSAA_NTM
            lblTGRSAAUserID.Text = t.RSAA_UserID
            lblTGRSABExist.Text = t.RSAB_Exist
            lblTGRSABEnable.Text = t.RSAB_Enable
            lblTGRSABNTM.Text = t.RSAB_NTM
            lblTGRSABUserID.Text = t.RSAB_UserID

            If lblTGRSAAExist.Text <> lblTGRSABExist.Text Then
                lblTGRSAAExist.ForeColor = Drawing.Color.Red
                lblTGRSABExist.ForeColor = Drawing.Color.Red
            End If

            If lblTGRSAAEnable.Text <> lblTGRSABEnable.Text Then
                lblTGRSAAEnable.ForeColor = Drawing.Color.Red
                lblTGRSABEnable.ForeColor = Drawing.Color.Red
            End If

            If lblTGRSAANTM.Text <> lblTGRSABNTM.Text Then
                lblTGRSAANTM.ForeColor = Drawing.Color.Red
                lblTGRSABNTM.ForeColor = Drawing.Color.Red
            End If

            If lblTGRSAAUserID.Text <> lblTGRSABUserID.Text Then
                lblTGRSAAUserID.ForeColor = Drawing.Color.Red
                lblTGRSABUserID.ForeColor = Drawing.Color.Red
            End If

        ElseIf e.Row.RowType = DataControlRowType.Header Then
            Dim strVersionA As String = ConfigurationManager.AppSettings("RSASite").Split(New String() {"|||"}, StringSplitOptions.None)(0)
            Dim strVersionB As String = ConfigurationManager.AppSettings("RSASite").Split(New String() {"|||"}, StringSplitOptions.None)(1)

            Dim lblTGRSAAExistHeader As Label = e.Row.FindControl("lblTGRSAAExistHeader")
            Dim lblTGRSAAEnableHeader As Label = e.Row.FindControl("lblTGRSAAEnableHeader")
            Dim lblTGRSAANTMHeader As Label = e.Row.FindControl("lblTGRSAANTMHeader")
            Dim lblTGRSAAUserIDHeader As Label = e.Row.FindControl("lblTGRSAAUserIDHeader")
            Dim lblTGRSABExistHeader As Label = e.Row.FindControl("lblTGRSABExistHeader")
            Dim lblTGRSABEnableHeader As Label = e.Row.FindControl("lblTGRSABEnableHeader")
            Dim lblTGRSABNTMHeader As Label = e.Row.FindControl("lblTGRSABNTMHeader")
            Dim lblTGRSABUserIDHeader As Label = e.Row.FindControl("lblTGRSABUserIDHeader")

            If strVersionA <> String.Empty Then
                lblTGRSAAExistHeader.Text = String.Format("RSA{0} Exist", strVersionA)
                lblTGRSAAEnableHeader.Text = String.Format("RSA{0} Enable", strVersionA)
                lblTGRSAANTMHeader.Text = String.Format("RSA{0} Next Token Mode", strVersionA)
                lblTGRSAAUserIDHeader.Text = String.Format("RSA{0} Assigned User", strVersionA)
            End If

            If strVersionB <> String.Empty Then
                lblTGRSABExistHeader.Text = String.Format("RSA{0} Exist", strVersionB)
                lblTGRSABEnableHeader.Text = String.Format("RSA{0} Enable", strVersionB)
                lblTGRSABNTMHeader.Text = String.Format("RSA{0} Next Token Mode", strVersionB)
                lblTGRSABUserIDHeader.Text = String.Format("RSA{0} Assigned User", strVersionB)
            End If

        End If

    End Sub

    Protected Sub lbtnTDownload_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

#Region "Supporting Functions"

    Private Function ConvertTokenNumber(ByVal strToken As String) As String
        If strToken.Trim = String.Empty Then Return String.Empty

        Return strToken.Trim.PadLeft(12, "0")
    End Function

    Private Sub InitRSAA()
        Dim strVersion As String = ConfigurationManager.AppSettings("RSASite").Split(New String() {"|||"}, StringSplitOptions.None)(0)

        If strVersion = String.Empty Then
            Throw New Exception("Error: InitRSAA fail. The version is empty.")
        End If

        'set up remote certificate validation
        ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertificate)

        ' establish a connected session with given credentials from arguments passed
        Dim conn As New SOAPCommandTarget(ConfigurationManager.AppSettings(String.Format("RSA{0}Link", strVersion)),
                                          ConfigurationManager.AppSettings(String.Format("RSA{0}WebLogicUsername", strVersion)),
                                          ConfigurationManager.AppSettings(String.Format("RSA{0}WebLogicPassword", strVersion)))

        If Not conn.Login(ConfigurationManager.AppSettings(String.Format("RSA{0}AMUsername", strVersion)),
                          ConfigurationManager.AppSettings(String.Format("RSA{0}AMPassword", strVersion))) Then
            Throw New Exception("Error: Unable to connect to the remote server. Please make sure your credentials are correct.")
        End If

        ' make all commands execute imports this target automatically
        CommandTargetPolicy.setDefaultCommandTarget(conn)

        Dim c1 As New GetIdentitySourcesCommand
        c1.execute()

        gudtRSAAIdentitySource = c1.identitySources(0)

    End Sub

    Private Sub InitRSAB()
        Dim strVersion As String = ConfigurationManager.AppSettings("RSASite").Split(New String() {"|||"}, StringSplitOptions.None)(1)

        If strVersion = String.Empty Then
            Throw New Exception("Error: InitRSAB fail. The version is empty.")
        End If

        'set up remote certificate validation
        ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertificate)

        ' establish a connected session with given credentials from arguments passed
        Dim conn As New SOAPCommandTarget(ConfigurationManager.AppSettings(String.Format("RSA{0}Link", strVersion)),
                                          ConfigurationManager.AppSettings(String.Format("RSA{0}WebLogicUsername", strVersion)),
                                          ConfigurationManager.AppSettings(String.Format("RSA{0}WebLogicPassword", strVersion)))

        If Not conn.Login(ConfigurationManager.AppSettings(String.Format("RSA{0}AMUsername", strVersion)),
                          ConfigurationManager.AppSettings(String.Format("RSA{0}AMPassword", strVersion))) Then
            Throw New Exception("Error: Unable to connect to the remote server. Please make sure your credentials are correct.")
        End If

        ' make all commands execute imports this target automatically
        CommandTargetPolicy.setDefaultCommandTarget(conn)

        Dim c1 As New GetIdentitySourcesCommand
        c1.execute()

        gudtRSABIdentitySource = c1.identitySources(0)

    End Sub

    Private Function ValidateServerCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
        Return True
    End Function

    Private Sub ShowComplete(ByRef lbl As Label, ByVal strMessage As String)
        lbl.Text = String.Format("{0} ({1})", strMessage, DateTime.Now.ToString("HH:mm:ss"))

        lbl.ForeColor = Drawing.Color.Blue

    End Sub

    Private Sub ShowError(ByRef lbl As Label, ByVal strMessage As String)
        lbl.Text = String.Format("{0} ({1})", strMessage, DateTime.Now.ToString("HH:mm:ss"))

        lbl.ForeColor = Drawing.Color.Red

    End Sub

#End Region

End Class
