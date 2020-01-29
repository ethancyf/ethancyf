Imports System.Xml
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.io
Imports Microsoft.Web.Services3.Design

Partial Public Class _Default
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
            'Dim ws As EHSVaccination_EndPoint = New EHSVaccination_EndPoint
            'ws.Url = "http://cdceap11:7500/EAI-eVac-Mainflow-context-root/EAI_eVac_EhsCaller_WSSSoapHttpPort"

            '' Create Client Policy (Specify that the policy uses the username over transport security assertion)
            'Dim oClientPolicy As New Microsoft.Web.Services3.Design.Policy()
            'Dim oAssertion As UsernameOverTransportAssertion = New UsernameOverTransportAssertion()
            'oAssertion.UsernameTokenProvider = New Microsoft.Web.Services3.Design.UsernameTokenProvider(Web.Configuration.WebConfigurationManager.AppSettings("WS_EHSVaccination_LoginID"), _
            '                                                                                            Web.Configuration.WebConfigurationManager.AppSettings("WS_EHSVaccination_Password"))
            'oClientPolicy.Assertions.Add(oAssertion)
            'ws.SetPolicy(oClientPolicy)
            'ws.Proxy = New System.Net.WebProxy()
            'ws.Timeout = 10000


            'Dim sResult As String = String.Empty
            'Dim xml As New XmlDocument()
            'xml.LoadXml(txtRequest.Text)
            'sResult = ws.getEhsVaccine(xml.InnerXml.ToString())
            'txtResult.Text = sResult


            Dim ws As GetEHSVaccine.GetEHSVaccine = New GetEHSVaccine.GetEHSVaccine
            ws.ServiceAuthHeaderValue = New GetEHSVaccine.ServiceAuthHeader
            ' CRE18-001 CIMS Vaccination Sharing [Start][Dickson]
            'ws.ServiceAuthHeaderValue.Username = Web.Configuration.WebConfigurationManager.AppSettings("WS_EHSVaccination_LoginID")
            'ws.ServiceAuthHeaderValue.Password = Web.Configuration.WebConfigurationManager.AppSettings("WS_EHSVaccination_Password")
            'ws.Url = Web.Configuration.WebConfigurationManager.AppSettings("WS_EHSVaccination_Url")
            ws.ServiceAuthHeaderValue.Username = Me.lblUsername.Text
            ws.ServiceAuthHeaderValue.Password = Me.lblPassword.Text
            ws.Url = Me.lblURL.Text
            ' CRE18-001 CIMS Vaccination Sharing [End][Dickson]
            'ws.Url = "http://localhost/WSINT/GetEHSVaccine.asmx"
            'ws.Url = "http://160.36.91.92/wsint/getehsvaccine.asmx"

            'ws.Url = "http://localhost/EHSVaccination/getEHSVaccine.asmx"
            'ws.Url = "https://hocmissv02/EHSVaccination/geteHSVaccine.asmx"
            'ws.Url = "http://160.36.90.3/EHSVaccinationLoadTest/GetEHSVaccine.asmx"
            'ws.Url = "https://192.168.99.152/EHSVaccination/GetEHSVaccine.asmx"
            'ws.Url = "http://eai-ppt-j2ee.server.ha.org.hk:7900/EAI-eVac-Mainflow-context-root/EAI_eVac_EhsCaller_WSSSoapHttpPort"
            Dim sResult As String = String.Empty
            Dim xml As New XmlDocument()
            xml.LoadXml(txtRequest.Text)

            dtmStart = Now
            sResult = ws.geteHSVaccineRecord(xml.InnerXml.ToString())
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

    Private Sub _Default_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim callback As New RemoteCertificateValidationCallback(AddressOf ValidateCertificate)
        System.Net.ServicePointManager.ServerCertificateValidationCallback = callback
    End Sub

    Private Function ValidateCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
        'Return True to force the certificate to be accepted.
        Return True
    End Function

    ' CRE18-001 CIMS Vaccination Sharing [Start][Dickson]
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack Then Exit Sub
        'Dim strPath As String = Web.Configuration.WebConfigurationManager.AppSettings("SamplePath")
        'If Not Directory.Exists(strPath) Then Exit Sub
        '
        '
        'Dim di As New DirectoryInfo(strPath)
        'For Each fi As FileInfo In di.GetFiles("*.xml")
        '    ddlSample.Items.Add(fi.Name)
        'Next
        '
        'LoadSampleXml(ddlSample.Items(0).Text)
        '
        'Me.lblUsername.Text = Web.Configuration.WebConfigurationManager.AppSettings("WS_EHSVaccination_LoginID")
        'Me.lblPassword.Text = Web.Configuration.WebConfigurationManager.AppSettings("WS_EHSVaccination_Password")
        'Me.lblURL.Text = Web.Configuration.WebConfigurationManager.AppSettings("WS_EHSVaccination_Url")
        SwitchRequestSystem()
    End Sub

    Private Sub ddlSample_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSample.SelectedIndexChanged
        '   LoadSampleXml(ddlSample.Text)
        Dim strPath As String = GetExamplePath()
        LoadSampleXml(strPath, ddlSample.Text)
    End Sub

    Private Sub LoadSampleXml(ByVal strPath As String, ByVal strFileName As String)
        'LoadRequest(IO.Path.Combine(Web.Configuration.WebConfigurationManager.AppSettings("SamplePath"), strFileName))
        LoadRequest(IO.Path.Combine(strPath, strFileName))
    End Sub

    Protected Sub rbRequestSystem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbCMSRequest.CheckedChanged, rbCIMSRequest.CheckedChanged
        SwitchRequestSystem()
    End Sub


    Protected Sub rbgRequestFormat_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbNewFormat.CheckedChanged, rbExistFormat.CheckedChanged
        SwitchRequestFormat()
    End Sub


    Private Sub SwitchRequestSystem()

        If rbCMSRequest.Checked Then
            Me.rbExistFormat.Visible = True
            Me.rbNewFormat.Checked = True

            SwitchRequestFormat()

            Me.lblUsername.Text = Web.Configuration.WebConfigurationManager.AppSettings("WS_EHSVaccination_LoginID_CMS")
            Me.lblPassword.Text = Web.Configuration.WebConfigurationManager.AppSettings("WS_EHSVaccination_Password_CMS")
            Me.lblURL.Text = Web.Configuration.WebConfigurationManager.AppSettings("WS_EHSVaccination_Url_CMS")
        End If

        If rbCIMSRequest.Checked Then
            Me.rbExistFormat.Visible = False
            Me.rbNewFormat.Checked = True

            SwitchRequestFormat()
            Me.lblUsername.Text = Web.Configuration.WebConfigurationManager.AppSettings("WS_EHSVaccination_LoginID_CIMS")
            Me.lblPassword.Text = Web.Configuration.WebConfigurationManager.AppSettings("WS_EHSVaccination_Password_CIMS")
            Me.lblURL.Text = Web.Configuration.WebConfigurationManager.AppSettings("WS_EHSVaccination_Url_CIMS")
        End If
    End Sub

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

        If rbCMSRequest.Checked Then
            strPath = Web.Configuration.WebConfigurationManager.AppSettings("SamplePath_CallEHS_CMS")
            If rbNewFormat.Checked Then
                strPath = IO.Path.Combine(strPath, "NewFormat")
            ElseIf rbExistFormat.Checked Then
                strPath = IO.Path.Combine(strPath, "ExistFormat")
            End If
        End If

        If rbCIMSRequest.Checked Then
            strPath = Web.Configuration.WebConfigurationManager.AppSettings("SamplePath_CallEHS_CIMS")
            strPath = IO.Path.Combine(strPath, "NewFormat")
        End If
        Return strPath
    End Function
    ' CRE18-001 CIMS Vaccination Sharing [End][Dickson]

End Class