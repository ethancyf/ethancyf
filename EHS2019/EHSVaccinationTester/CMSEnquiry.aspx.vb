Imports System.Xml
Imports System.io

Partial Class _CMSImmuEnquiry
    Inherits System.Web.UI.Page


    Private Sub LoadRequest(ByVal sPath As String)
        Dim fr As IO.StreamReader
        Try
            Dim a As IO.FileStream = New IO.FileStream(sPath, IO.FileMode.Open, IO.FileAccess.Read)
            fr = New IO.StreamReader(a)

            txtRequest.Text = ""
            While Not fr.EndOfStream
                txtRequest.Text += fr.ReadLine() + vbCrLf
            End While
            fr.Close()
        Catch ex As Exception
            If fr IsNot Nothing Then
                fr.Close()
            End If
            Throw ex
        End Try
    End Sub

    Protected Sub BtnQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnQuery.Click
        Dim dtmStart As DateTime = DateTime.MinValue
        Dim dtmEnd As DateTime = DateTime.MinValue

        Try
            Me.lblException.Text = ""
            Dim sResult As String = String.Empty
            Dim xml As New XmlDocument()
            'xml.LoadXml(txtRequest.Text)

            Dim callback As New Net.Security.RemoteCertificateValidationCallback(AddressOf ValidateCertificate)
            System.Net.ServicePointManager.ServerCertificateValidationCallback = callback

            'sResult = WSProxyCMS.GetVaccine(xml.InnerXml)

            dtmStart = Now
            sResult = WSProxyCMS.GetVaccine(txtRequest.Text)
            dtmEnd = Now


            ' CRE18-001 CIMS Vaccination Sharing [Start][Dickson]
            Dim strTag As String() = Split(sResult, "><")
            Dim strFinResult As String = String.Empty
            Dim intIdx As Integer = 0
            Dim intTabs As Integer = 0
            Dim pattern As String = "/"
            Dim ex As New System.Text.RegularExpressions.Regex(pattern)

            For Each strTagElement As String In strTag
                Dim blnShowTabs As Boolean = False

                If intIdx = 0 Then
                    strFinResult = strTagElement
                Else
                    If strTagElement.IndexOf("/") = 0 Then
                        intTabs = intTabs - 3
                    End If

                    strFinResult = String.Concat(strFinResult, ">", Environment.NewLine, Space(intTabs), "<", strTagElement)

                    If ex.Matches(strTagElement).Count = 0 Then
                        intTabs = intTabs + 3
                    End If

                End If

                intIdx = intIdx + 1
            Next

            txtResult.Text = strFinResult
            'txtResult.Text = sResult
            ' CRE18-001 CIMS Vaccination Sharing [End][Dickson]


        Catch ex As Exception
            dtmEnd = Now
            Me.lblException.Text = Me.lblException.Text + Environment.NewLine + ex.Message & "<br/>" & ex.StackTrace
        End Try

        Dim dtmDiff As DateTime = New DateTime(dtmEnd.Subtract(dtmStart).Ticks)
        Me.lblEnquiryTime.Text = dtmDiff.ToString("mm:ss:fff ") & "(" & dtmDiff.Second * 1000 + dtmDiff.Millisecond & ")"
    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack Then Exit Sub

        Try
            ' CRE18-001 CIMS Vaccination Sharing [Start][Dickson]
            'Dim strPath As String = Web.Configuration.WebConfigurationManager.AppSettings("EHSSamplePath")
            'If Not Directory.Exists(strPath) Then Exit Sub
            '
            '
            'Dim di As New DirectoryInfo(strPath)
            'For Each fi As FileInfo In di.GetFiles("*.xml")
            '    ddlSample.Items.Add(fi.Name)
            'Next
            '
            'LoadSampleXml(ddlSample.Items(0).Text

            SwitchRequestFormat()
            ' CRE18-001 CIMS Vaccination Sharing [End][Dickson]
        Catch ex As Exception

        End Try

        Me.lblUsername.Text = WSProxyCMS.GetWSUsername
        Me.lblPassword.Text = WSProxyCMS.GetWSPassword
        Me.lblURL.Text = WSProxyCMS.GetWSUrl
    End Sub

    Private Sub ddlSample_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSample.SelectedIndexChanged
        ' CRE18-001 CIMS Vaccination Sharing [Start][Dickson]
        'LoadSampleXml(ddlSample.Text)
        Dim strPath As String = GetExamplePath()
        LoadSampleXml(strPath, ddlSample.Text)
        ' CRE18-001 CIMS Vaccination Sharing [End][Dickson]
    End Sub

    Private Sub LoadSampleXml(ByVal strPath As String, ByVal strFileName As String)
        LoadRequest(IO.Path.Combine(strPath, strFileName))  ' CRE18-001 CIMS Vaccination Sharing [Dickson]
    End Sub

    Private Function ValidateCertificate(ByVal sender As Object, ByVal certificate As System.Security.Cryptography.X509Certificates.X509Certificate, ByVal chain As System.Security.Cryptography.X509Certificates.X509Chain, ByVal sslPolicyErrors As Net.Security.SslPolicyErrors) As Boolean
        'Return True to force the certificate to be accepted.
        Me.lblException.Text = Me.lblException.Text + Environment.NewLine + "<BR/>ValidateCertificate:" + sslPolicyErrors.ToString()
        Return True
    End Function

    Protected Sub rbgRequestFormat_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbNewFormat.CheckedChanged, rbExistFormat.CheckedChanged
        SwitchRequestFormat()
    End Sub

    ' CRE18-001 CIMS Vaccination Sharing [Start][Dickson]
    Private Sub SwitchRequestFormat()
        Dim strPath As String = GetExamplePath()
        If Not Directory.Exists(strPath) Then Exit Sub

        ddlSample.Items.Clear()

        Dim di As New DirectoryInfo(strPath)
        For Each fi As FileInfo In di.GetFiles("*.xml")
            ddlSample.Items.Add(fi.Name)
        Next
        LoadSampleXml(strPath, ddlSample.Items(0).Text)

    End Sub

    Private Function GetExamplePath() As String
        Dim strPath As String = ""
        strPath = Web.Configuration.WebConfigurationManager.AppSettings("SamplePath_CallCMS")
        If rbNewFormat.Checked Then
            strPath = IO.Path.Combine(strPath, "NewFormat")
        ElseIf rbExistFormat.Checked Then
            strPath = IO.Path.Combine(strPath, "ExistFormat")
        End If
        Return strPath
    End Function
    ' CRE18-001 CIMS Vaccination Sharing [End][Dickson]
End Class
