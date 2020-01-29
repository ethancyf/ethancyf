Imports System.IO
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.ServiceModel
Imports System.Xml
Imports System.Net

Public Class CallEHS
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ' Load sample list
            Dim lstFile As New List(Of ListItem)

            For Each filePath As String In Directory.GetFiles(Server.MapPath("./Sample/CallEHS"))
                lstFile.Add(New ListItem(Path.GetFileName(filePath), filePath))
            Next

            ddlLoadSample.DataSource = lstFile
            ddlLoadSample.DataBind()

            ddlLoadSample.Items.Insert(0, New ListItem("--- Please select ---", String.Empty))

            lblLoadSampleInstruction.Text = String.Format("From {0}", Server.MapPath("./Sample/CallEHS"))

            InitControlOnce()

        End If

    End Sub

    Private Sub InitControlOnce()
        txtEndpointURL.Text = ConfigurationManager.AppSettings("DefaultEHSUrl")

    End Sub

    Protected Sub ddlLoadSample_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim sr As New StreamReader(Path.Combine(Server.MapPath("./Sample/CallEHS"), ddlLoadSample.SelectedItem.Text))
        txtRequest.Text = sr.ReadToEnd
        sr.Close()

        ' Replace some special text
        txtRequest.Text = txtRequest.Text.Replace("{Timestamp}", String.Format("{0}{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), DateTime.Now.ToString("zzzz").Replace(":", String.Empty)))

        ddlLoadSample.SelectedIndex = 0

    End Sub

    '

    Protected Sub btnSubmitRequest_Click(sender As Object, e As EventArgs)
        System.Net.ServicePointManager.ServerCertificateValidationCallback = AddressOf ValidateRemoteCertificate

        Try
            Dim wcf As New ExampleWebS
            wcf.Url = txtEndpointURL.Text.Trim
            txtResult.Text = wcf.getExternalWebS(txtRequest.Text)

            If rblSubmitOption.SelectedValue = "B" Then
                txtResult.Text = BeautifyText(txtResult.Text)

            End If

        Catch ex As System.Exception
            txtResult.Text = ex.Message + vbCrLf + ex.StackTrace

        End Try

    End Sub

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
