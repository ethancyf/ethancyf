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

Partial Public Class PasswordUpdate
    Inherits System.Web.UI.Page

#Region "Properties and Fields"

    Private Const VS_IDSOURCE As String = "IDSOURCE"

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

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitRSA7()
        End If

    End Sub

    Private Sub InitRSA7()
        Try
            'set up remote certificate validation
            ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertificate)

            ' establish a connected session with given credentials from arguments passed
            Dim conn As New SOAPCommandTarget(ConfigurationManager.AppSettings("RSA7Link"), ConfigurationManager.AppSettings("RSA7WebLogicUsername"), ConfigurationManager.AppSettings("RSA7WebLogicPassword"))

            If Not conn.Login(ConfigurationManager.AppSettings("RSA7AMUsername"), ConfigurationManager.AppSettings("RSA7AMPassword")) Then
                Throw New Exception("Error: Unable to connect to the remote server. Please make sure your credentials are correct.")
            End If

            ' make all commands execute imports this target automatically
            CommandTargetPolicy.setDefaultCommandTarget(conn)

            Dim c1 As New GetIdentitySourcesCommand
            c1.execute()

            IdentitySource = c1.identitySources(0)

        Catch ex As Exception
            ShowError(lblEResult, ex.Message)

        End Try

    End Sub

    Private Function ValidateServerCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
        Return True
    End Function

    Protected Sub btnEEnquire_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        gvE.Visible = False
        lblEUserIDError.Text = String.Empty

        If cboEPasswordDate.Checked Then
            If DateTime.TryParseExact(txtEPasswordDate.Text.Trim, "yyyy-MM-dd HH:mm:ss", Nothing, Globalization.DateTimeStyles.None, Nothing) = False Then
                ShowError(lblEResult, "Incorrect date format")
                Return
            End If

        End If

        ' Enquire RSA
        Dim c1 As New SearchPrincipalsCommand

        c1.filter = Filter.empty
        c1.limit = Integer.MaxValue
        c1.identitySourceGuid = IdentitySource.guid
        c1.securityDomainGuid = IdentitySource.securityDomainGuid

        c1.execute()

        Dim dt1 As New DataTable
        dt1.Columns.Add("PasswordChangeDate", GetType(String))
        dt1.Columns.Add("UserID", GetType(String))
        dt1.Columns.Add("Value", GetType(PrincipalDTO))

        Dim dr1 As DataRow = Nothing
        Dim llstPrincipal As New List(Of String)

        For Each p As PrincipalDTO In c1.principals
            dr1 = dt1.NewRow

            If p.passwordChangeDate.HasValue Then
                dr1("PasswordChangeDate") = p.passwordChangeDate.Value.ToString("yyyy-MM-dd HH:mm:ss")
            Else
                dr1("PasswordChangeDate") = "1900-01-01 00:00:00"
            End If
            dr1("UserID") = p.userID
            dr1("Value") = p

            dt1.Rows.Add(dr1)
            llstPrincipal.Add(p.userID.ToUpper)

        Next

        ' Filter the records
        Dim lstrPasswordDateFilter As String = String.Empty
        Dim lstrUserIDFilter As String = String.Empty

        If cboEPasswordDate.Checked Then
            lstrPasswordDateFilter = String.Format("PasswordChangeDate {0} '{1}'", rblEPasswordDate.SelectedValue, txtEPasswordDate.Text.Trim)
        End If

        If cboEUserID.Checked Then
            Dim llstUserID As New List(Of String)
            Dim llstUserIDNotFound As New List(Of String)

            For Each lstrUserID As String In txtEUserID.Text.Trim.Split(New String() {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                lstrUserID = lstrUserID.Trim

                llstUserID.Add(String.Format("'{0}'", lstrUserID))

                If llstPrincipal.Contains(lstrUserID.ToUpper) = False Then
                    llstUserIDNotFound.Add(lstrUserID)
                End If
            Next

            lstrUserIDFilter = String.Format("UserID IN ({0})", String.Join(",", llstUserID.ToArray))

            If llstUserIDNotFound.Count <> 0 Then
                lblEUserIDError.Text = String.Format("User ID not found: {0}", String.Join(", ", llstUserIDNotFound.ToArray))
            End If

        End If

        Dim lstrFilter As String = lstrPasswordDateFilter

        If lstrUserIDFilter <> String.Empty Then
            If lstrFilter = String.Empty Then
                lstrFilter = lstrUserIDFilter
            Else
                lstrFilter += String.Format(" AND {0}", lstrUserIDFilter)
            End If
        End If


        Dim lstrSort As String = String.Empty

        Select Case rblESort.SelectedValue
            Case "P"
                lstrSort = String.Format("PasswordChangeDate {0}, UserID", rblESortDirection.SelectedValue)

            Case "U"
                lstrSort = String.Format("UserID {0}, PasswordChangeDate", rblESortDirection.SelectedValue)

        End Select

        Dim dt2 As DataTable = dt1.Clone

        For Each dr1 In dt1.Select(lstrFilter, lstrSort)
            dt2.ImportRow(dr1)
        Next

        gvE.DataSource = dt2
        gvE.DataBind()
        gvE.Visible = True

        ShowComplete(lblEResult, String.Format("Showing {0} records", dt2.Rows.Count))

    End Sub

    Protected Sub btnEClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        gvE.Visible = False
    End Sub

    Protected Sub gvE_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim p As PrincipalDTO = DirectCast(e.Row.DataItem, DataRowView)("Value")

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
            ElseIf p.failedPasswordCount >= 3 Then
                lblSPGFailCount.ForeColor = Drawing.Color.Red
            End If

            ' Lockout
            Dim lblSPGLockout As Label = e.Row.FindControl("lblSPGLockout")

            If p.lockoutStatus Then
                lblSPGLockout.Text = "Yes"
                lblSPGLockout.ForeColor = Drawing.Color.Red
            Else
                lblSPGLockout.Text = "No"
            End If

            ' Password Change Date
            If p.passwordChangeDate.HasValue Then
                DirectCast(e.Row.FindControl("lblSPGPasswordDate"), Label).Text = p.passwordChangeDate.Value.ToString("yyyy-MM-dd HH:mm:ss")
            Else
                DirectCast(e.Row.FindControl("lblSPGPasswordDate"), Label).Text = "NA"
            End If

        End If

    End Sub

    '

    'Protected Sub btnUUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    gvU.Visible = False

    '    Dim dt As New DataTable
    '    dt.Columns.Add("UserID", GetType(String))
    '    dt.Columns.Add("Result", GetType(String))
    '    Dim dr As DataRow = Nothing

    '    Dim lintDone As Integer = 0
    '    Dim lintFail As Integer = 0

    '    For Each lstrUserID As String In txtUUserID.Text.Trim.Split(New String() {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
    '        lstrUserID = lstrUserID.Trim

    '        dr = dt.NewRow
    '        dr("UserID") = lstrUserID

    '        Dim c1 As New SearchPrincipalsCommand

    '        c1.filter = Filter.equal(PrincipalDTO._LOGINUID, lstrUserID)
    '        c1.limit = Integer.MaxValue
    '        c1.identitySourceGuid = IdentitySource.guid
    '        c1.securityDomainGuid = IdentitySource.securityDomainGuid

    '        c1.execute()

    '        If c1.principals.Length = 0 Then
    '            dr("Result") = "User ID not found"
    '            dt.Rows.Add(dr)

    '            lintFail += 1

    '            Continue For

    '        ElseIf c1.principals.Length > 1 Then
    '            dr("Result") = "User ID found more than once"
    '            dt.Rows.Add(dr)

    '            lintFail += 1

    '            Continue For

    '        End If

    '        Dim p As PrincipalDTO = c1.principals(0)

    '        Dim c2 As New UpdatePrincipalCommand
    '        c2.identitySourceGuid = IdentitySource.guid

    '        Dim u As New UpdatePrincipalDTO
    '        u.guid = p.guid

    '        ' Copy the rowVersion to satisfy optimistic locking requirements
    '        u.rowVersion = p.rowVersion

    '        ' collect all modifications here
    '        Dim lstM As New List(Of ModificationDTO)
    '        Dim m As ModificationDTO

    '        ' Password
    '        m = New ModificationDTO
    '        m.operation = ModificationDTO._REPLACE_ATTRIBUTE
    '        m.name = PrincipalDTO._PASSWORD
    '        m.values = New Object() {"eHSSPPass1234!"}
    '        lstM.Add(m)

    '        u.modifications = lstM.ToArray()
    '        c2.principalModification = u

    '        Try
    '            c2.execute()

    '            'dr("Result") = "Done"
    '            'dt.Rows.Add(dr)

    '            lintDone += 1

    '        Catch ex As Exception
    '            dr("Result") = String.Format("Exception: {0}", ex.Message)
    '            dt.Rows.Add(dr)

    '            lintFail += 1

    '        End Try

    '    Next

    '    gvU.DataSource = dt
    '    gvU.DataBind()
    '    gvU.Visible = True

    '    ShowComplete(lblUResult, String.Format("You have input {0} cases<br />Result: {1} done, {2} fail<br />", lintDone + lintFail, lintDone, lintFail))
    '    lblUResult.ForeColor = Drawing.Color.Black

    'End Sub

    'Protected Sub gvU_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        Dim dr As DataRowView = e.Row.DataItem

    '        ' User ID
    '        DirectCast(e.Row.FindControl("lblUGUserID"), Label).Text = dr("UserID")

    '        ' Result
    '        Dim lblUGResult As Label = e.Row.FindControl("lblUGResult")

    '        lblUGResult.Text = dr("Result")

    '        If lblUGResult.Text <> "Done" Then
    '            lblUGResult.ForeColor = Drawing.Color.Red
    '        End If

    '    End If

    'End Sub

    '

    Private Sub ShowComplete(ByRef lbl As Label, ByVal strMessage As String)
        lbl.Text = String.Format("{0} ({1})", strMessage, DateTime.Now.ToString("HH:mm:ss"))

        lbl.ForeColor = Drawing.Color.Blue

    End Sub

    Private Sub ShowError(ByRef lbl As Label, ByVal strMessage As String)
        lbl.Text = String.Format("{0} ({1})", strMessage, DateTime.Now.ToString("HH:mm:ss"))

        lbl.ForeColor = Drawing.Color.Red

    End Sub

End Class
