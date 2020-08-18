Imports Common.ComFunction
Imports System.IO
Imports System.Net
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.ServiceModel
Imports System.Xml

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
        txtPatientPortalEndpointURL.Text = ConfigurationManager.AppSettings("DefaultEHSPatientPortalUrl")

    End Sub

    Protected Sub ddlLoadSample_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim sr As New StreamReader(Path.Combine(Server.MapPath("./Sample/CallEHS"), ddlLoadSample.SelectedItem.Text))
        txtRequest.Text = sr.ReadToEnd
        sr.Close()

        btnDownload.Visible = False

        ' Replace some special text
        Select Case Left(ddlLoadSample.SelectedItem.Text, 1)
            Case "1", "2", "3", "4", "5", "6"
                txtRequest.Text = txtRequest.Text.Replace("{Timestamp}", String.Format("{0}{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), DateTime.Now.ToString("zzzz").Replace(":", String.Empty)))

            Case "7"
                txtRequest.Text = txtRequest.Text.Replace("{Timestamp}", String.Format("{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")))
                btnDownload.Visible = True

            Case "8"
                txtRequest.Text = txtRequest.Text.Replace("{Timestamp}", String.Format("{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")))

            Case Else
                txtRequest.Text = "Wrong Sample!"
                Return

        End Select

        '' Reset drop-down list
        'ddlLoadSample.SelectedIndex = 0

    End Sub

    '

    Protected Sub btnSubmitRequest_Click(sender As Object, e As EventArgs)
        System.Net.ServicePointManager.ServerCertificateValidationCallback = AddressOf ValidateRemoteCertificate

        Try
            Dim wcf As New ExampleWebS

            'wcf.Url = txtEndpointURL.Text.Trim

            ' Set Endpoint URL
            Select Case Left(ddlLoadSample.SelectedItem.Text, 1)
                Case "1", "2", "3", "4", "5", "6"
                    wcf.Url = txtEndpointURL.Text
                Case "7", "8"
                    wcf.Url = txtPatientPortalEndpointURL.Text
                Case Else
                    txtResult.Text = "Wrong Sample!"
                    Return
            End Select

            txtResult.Text = wcf.getExternalWebS(txtRequest.Text)

            If rblSubmitOption.SelectedValue = "B" Then
                txtResult.Text = BeautifyText2(txtResult.Text)

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

    Public Function BeautifyText2(s As String) As String
        Dim strTag As String() = Split(s, "><")
        Dim strFinResult As String = String.Empty
        Dim intIdx As Integer = 0
        Dim intTabs As Integer = 0
        Dim blnCDATA As Boolean = False
        Dim pattern As String = "/"
        Dim ex As New System.Text.RegularExpressions.Regex(pattern)

        For Each strTagElement As String In strTag
            Dim blnShowTabs As Boolean = False

            If intIdx = 0 Then
                strFinResult = strTagElement
            Else
                If strTagElement.IndexOf("/") = 0 Then
                    intTabs = intTabs - 2
                End If

                If strTagElement.Contains("version") Then
                    blnCDATA = True
                Else
                    blnCDATA = False
                End If

                strFinResult = String.Concat(strFinResult, ">", Environment.NewLine, Space(intTabs), "<", strTagElement)

                If ex.Matches(strTagElement).Count = 0 AndAlso Not blnCDATA Then
                    intTabs = intTabs + 2
                End If

            End If

            intIdx = intIdx + 1
        Next

        Return strFinResult

    End Function

    Protected Sub btnDownload_Click(sender As Object, e As EventArgs)
        '-----------------------------------
        'Test: convert byte to file
        '-----------------------------------
        If txtResult.Text.IndexOf("<Result>") <> -1 Then
            Dim intStartPosition As Integer = txtResult.Text.IndexOf("<Result>") + 8
            Dim intEndPosition As Integer = txtResult.Text.IndexOf("</Result>")
            Dim intStrLength As Integer = txtResult.Text.Length - intStartPosition - (txtResult.Text.Length - intEndPosition)

            Try
                Dim byteXML() As Byte = Convert.FromBase64String(txtResult.Text.Substring(intStartPosition, intStrLength))

                'Dim byteXML((txtByte.Text.Length / 2) - 1) As Byte

                'For intPos As Integer = 0 To txtByte.Text.Length - 2 Step 2
                '    byteXML(intPos / 2) = Convert.ToByte(txtByte.Text.Substring(intPos, 2), 16)
                'Next

                If Not byteXML Is Nothing Then
                    Dim strArchiveFormat As String = (New GeneralFunction).getSystemParameter("EHRSS_PP_DoctorList_ArchiveFormat")
                    Dim strPath As String = ConfigurationManager.AppSettings("DownloadDoctorListPath")

                    System.IO.File.WriteAllBytes(String.Format("{0}sd.{1}", strPath, strArchiveFormat.ToLower), byteXML)

                    Dim file As IO.FileInfo = New IO.FileInfo(String.Format("{0}sd.{1}", strPath, strArchiveFormat.ToLower))

                    With HttpContext.Current
                        If file.Exists Then
                            .Response.Clear()
                            .Response.AddHeader("Content-Disposition", "attachment; filename=" & file.Name)
                            .Response.AddHeader("Content-Length", file.Length.ToString())
                            .Response.ContentType = "application/octet-stream"
                            .Response.WriteFile(file.FullName)
                            .Response.Flush()
                            System.IO.File.Delete(String.Format("{0}sd.{1}", strPath, strArchiveFormat.ToLower))
                            ''Remove the temp file path
                            'If System.IO.Directory.Exists(file.Directory.FullName) Then
                            '    System.IO.Directory.Delete(file.Directory.FullName, True)
                            'End If
                            .Response.End()
                        Else
                            HttpContext.Current.Response.Clear()
                            HttpContext.Current.Response.End()
                        End If
                    End With

                End If

            Catch ex As Exception
                Throw
            End Try

        End If
        ''-----------------------------------
    End Sub
End Class
