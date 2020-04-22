Imports System.IO
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports Common.eHRIntegration
Imports Common.eHRIntegration.BLL
Imports Common.eHRIntegration.Model.Xml.eHRService

Public Class CallEHR
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ' Load sample list
            Dim lstFile As New List(Of ListItem)

            For Each filePath As String In Directory.GetFiles(Server.MapPath("./Sample/CallEHR/VerifySystem"))
                lstFile.Add(New ListItem(Path.GetFileName(filePath), filePath))
            Next

            ddlVLoadSample.DataSource = lstFile
            ddlVLoadSample.DataBind()

            ddlVLoadSample.Items.Insert(0, New ListItem("--- Please select ---", String.Empty))

            lblVLoadSampleInstruction.Text = String.Format("From {0}", Server.MapPath("./Sample/CallEHR/VerifySystem"))

            ' Load sample list
            lstFile = New List(Of ListItem)

            For Each filePath As String In Directory.GetFiles(Server.MapPath("./Sample/CallEHR/GetEhrWebS"))
                lstFile.Add(New ListItem(Path.GetFileName(filePath), filePath))
            Next

            ddlGLoadSample.DataSource = lstFile
            ddlGLoadSample.DataBind()

            ddlGLoadSample.Items.Insert(0, New ListItem("--- Please select ---", String.Empty))

            lblGLoadSampleInstruction.Text = String.Format("From {0}", Server.MapPath("./Sample/CallEHR/GetEhrWebS"))

            ' Init
            InitControlOnce()

        End If

    End Sub

    Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Select Case mvFunction.GetActiveView.ID
            Case vVerifySystem.ID
                lblVerifySystem.Visible = True
                lbtnVerifySystem.Visible = False
                lblGetEhrWebS.Visible = False
                lbtnGetEhrWebS.Visible = True

            Case vGetEhrWebS.ID
                lblVerifySystem.Visible = False
                lbtnVerifySystem.Visible = True
                lblGetEhrWebS.Visible = True
                lbtnGetEhrWebS.Visible = False

        End Select

        txtVEndpointURL.Enabled = rblVMode.SelectedValue = "WS"
        txtGEndPointURL.Enabled = rblGMode.SelectedValue = "WS"

    End Sub

    Private Sub InitControlOnce()
        mvFunction.ActiveViewIndex = 0

        txtVEndpointURL.Text = ConfigurationManager.AppSettings("DefaultEHRVPUrl")
        txtGEndPointURL.Text = ConfigurationManager.AppSettings("DefaultEHRGetWSUrl")

    End Sub

    '

    Protected Sub lbtnVerifySystem_Click(sender As Object, e As EventArgs)
        mvFunction.SetActiveView(vVerifySystem)
    End Sub

    Protected Sub lbtnGetEhrWebS_Click(sender As Object, e As EventArgs)
        mvFunction.SetActiveView(vGetEhrWebS)
    End Sub

    '

    Protected Sub btnVSubmit_Click(sender As Object, e As EventArgs)
        System.Net.ServicePointManager.ServerCertificateValidationCallback = AddressOf ValidateRemoteCertificate

        Try
            Dim strRequest As String = txtVRequest.Text

            If cboVRemoveWhiteSpace.Checked Then
                strRequest = (New Regex(">\s*<")).Replace(strRequest, "><")
            End If

            Dim udteHRServiceBLL As New eHRServiceBLL(strMode:=rblVMode.SelectedValue, strVPLink:=txtVEndpointURL.Text)
            txtVResult.Text = udteHRServiceBLL.VerifySystem(strRequest)

            If rblVSubmitOption.SelectedValue = "B" Then
                txtVResult.Text = BeautifyText(txtVResult.Text)
            End If

            ' Find the verification pass
            Dim strVP As String = New Regex("(?<=<VerificationPass>)(.*?)(?=</VerificationPass>)").Match(txtVResult.Text).ToString

            Session("VP") = strVP

        Catch ex As Exception
            txtVResult.Text = ex.ToString

        End Try

    End Sub

    Protected Sub ddlVLoadSample_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim sr As New StreamReader(Path.Combine(Server.MapPath("./Sample/CallEHR/VerifySystem"), ddlVLoadSample.SelectedItem.Text))
        txtVRequest.Text = sr.ReadToEnd

        sr.Close()

        ' Replace some special text
        txtVRequest.Text = txtVRequest.Text.Replace("{Timestamp}", String.Format("{0}{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), DateTime.Now.ToString("zzzz").Replace(":", String.Empty)))

        ddlVLoadSample.SelectedIndex = 0

    End Sub

    Protected Sub btnGSubmit_Click(sender As Object, e As EventArgs)
        System.Net.ServicePointManager.ServerCertificateValidationCallback = AddressOf ValidateRemoteCertificate

        Try
            Dim strRequest As String = txtGRequest.Text

            If cboGRemoveWhiteSpace.Checked Then
                strRequest = (New Regex(">\s*<")).Replace(strRequest, "><")
            End If

            Dim udteHRServiceBLL As New eHRServiceBLL(strMode:=rblGMode.SelectedValue, strGetWebSLink:=txtGEndPointURL.Text)
            txtGResult.Text = udteHRServiceBLL.GetEhrWebS(strRequest, "From tester")

            If rblGSubmitOption.SelectedValue = "B" Then
                txtGResult.Text = BeautifyText(txtGResult.Text)
            End If

        Catch ex As Exception
            txtGResult.Text = ex.ToString

        End Try

    End Sub

    Protected Sub ddlGLoadSample_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim sr As New StreamReader(Path.Combine(Server.MapPath("./Sample/CallEHR/GetEhrWebS"), ddlGLoadSample.SelectedItem.Text))
        txtGRequest.Text = sr.ReadToEnd

        sr.Close()

        ' Replace some special text
        Dim strVP As String = String.Empty
        If Not IsNothing(Session("VP")) Then strVP = Session("VP")

        txtGRequest.Text = txtGRequest.Text.Replace("{VP}", strVP)
        txtGRequest.Text = txtGRequest.Text.Replace("{Timestamp}", String.Format("{0}{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), DateTime.Now.ToString("zzzz").Replace(":", String.Empty)))

        ddlGLoadSample.SelectedIndex = 0

    End Sub

    '

    Public Function ValidateRemoteCertificate(sender As Object, certification As X509Certificate, chain As X509Chain, sslPolicyErrors As SslPolicyErrors) As Boolean
        Return True
    End Function

    Public Function BeautifyText(s As String) As String
        Dim r As String = String.Empty

        Dim intIndent As Integer = 0

        Dim ary As String() = s.Split(New String() {"><"}, StringSplitOptions.RemoveEmptyEntries)
        Dim count As Integer = 0

        For Each a As String In ary
            count += 1

            If count <> 1 Then
                r += Environment.NewLine
            End If

            If a.StartsWith("/") Then
                intIndent -= 1
            End If

            If intIndent >= 0 Then
                r += StrDup(2 * intIndent, " ")
            End If

            If a.StartsWith("/") = False AndAlso a.Contains("version") = False AndAlso a.Contains("</") = False Then
                intIndent += 1
            End If

            If count <> 1 Then
                r += "<"
            End If

            r += a

            If count <> ary.Length Then
                r += ">"

            End If

        Next

        Return r

    End Function

End Class