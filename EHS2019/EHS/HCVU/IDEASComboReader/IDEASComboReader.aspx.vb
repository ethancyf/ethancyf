Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates

Partial Public Class IDEASComboReader
    Inherits System.Web.UI.Page


#Region "Property"

    ReadOnly Property AppRefId() As String
        Get
            If Not System.Web.HttpContext.Current Is Nothing Then
                Return System.Web.HttpContext.Current.Session.SessionID.Trim().Substring(0, 20) + DateTime.Now.ToString("yyyyMMddHHmmss")
            Else
                Return DateTime.Now.ToString("yyyyMMddHHmmss")
            End If
        End Get
    End Property

    Public ReadOnly Property Artifact() As String
        Get
            Return Web.HttpContext.Current.Request.QueryString("SAMLart").Replace("/E", "+")
        End Get
    End Property

#End Region

#Region "Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InitServicePointManager()
        Dim ideasSamlResponse As IdeasRM.IdeasResponse = Nothing

        If Not Me.IsPostBack Then

            ' If connectted to token server already and page load by redirect from IDEAS after read card (QueryString > 0)
            If IDEASCombo.SessIdeasTokenResponse IsNot Nothing AndAlso _
                ((IDEASCombo.SessIdeasVersion <> BLL.IdeasBLL.EnumIdeasVersion.Combo And IDEASCombo.SessIdeasTokenResponse.IdeasMAURL IsNot Nothing) Or _
                 (IDEASCombo.SessIdeasVersion = BLL.IdeasBLL.EnumIdeasVersion.Combo And IDEASCombo.SessIdeasTokenResponse.BrokerURL IsNot Nothing)) _
                And Request.QueryString.Count > 0 Then

                'Logger.Log("IdeasReader loaded (Redirect from IDEAS after read SmartIC)")

                Try
                    ' TokenResponse no error status
                    If Request.QueryString("status") Is Nothing Then
                        'Logger.Log("IDEAS Version: " + BLL.IdeasBLL.ConvertIdeasVersion(IDEASCombo.SessIdeasVersion))
                        'Logger.Log("Read SmartIC status: ")
                        'Logger.Log("Start get CardFaceDate")
                        ' Try to get card face data
                        Dim ideasHelper As IdeasRM.IHelper = IdeasRM.HelpFactory.createHelper()

                        Select Case IDEASCombo.SessIdeasVersion
                            Case BLL.IdeasBLL.EnumIdeasVersion.One, BLL.IdeasBLL.EnumIdeasVersion.Two, BLL.IdeasBLL.EnumIdeasVersion.TwoGender
                                ideasSamlResponse = ideasHelper.getCardFaceData(IDEASCombo.SessIdeasTokenResponse, Artifact, BLL.IdeasBLL.ConvertIdeasVersion(IDEASCombo.SessIdeasVersion))
                            Case BLL.IdeasBLL.EnumIdeasVersion.Combo
                                ideasSamlResponse = ideasHelper.getCardFaceData(IDEASCombo.SessIdeasTokenResponse, Artifact, False)
                        End Select

                        IDEASCombo.SessIdeasSamlResponse = ideasSamlResponse
                    Else ' TokenResponse has error status
                        'Logger.Log("Read SmartIC status: " + Request.QueryString("status"))
                        'Me.lblQueryStringStatus.Text = Request.QueryString("status")
                    End If

                Catch ex As Exception
                    'Logger.Log("IdeasReader exception (Message)" + ex.Message)
                    'Logger.Log("IdeasReader exception (StackTrace)" + ex.StackTrace)
                    IDEASCombo.SessIdeasException = ex
                End Try

                Dim _udtSessionHandler As New BLL.SessionHandlerBLL
                Dim udtSmartIDContent As BLL.SmartIDContentModel = _udtSessionHandler.SmartIDContentGetFormSession(IDEASCombo.SessFunctCode)

                udtSmartIDContent.Artifact = Artifact
                udtSmartIDContent.IdeasSamlResponse = ideasSamlResponse

                IDEASCombo.SessSmartIDContent = udtSmartIDContent

                DisplayResultInParent()
                'Else
                '    Logger.Log("IdeasReader loaded")
                '    IDEASCombo.SessIdeasTokenResponse = Nothing
                '    IDEASCombo.SessIdeasVersion = Nothing

                '    ReadSmartIC(EnumIdeasVersion.Combo)
            End If
        Else
            ' Clear session on start (Not redirect from IDEAS server)
            IDEASCombo.SessIdeasTokenResponse = Nothing
            IDEASCombo.SessIdeasVersion = Nothing

        End If
    End Sub

    ''' <summary>
    ''' Call parent page's function to display read card result in parent page
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DisplayResultInParent()
        Dim strBuilder As StringBuilder = New StringBuilder

        strBuilder.Append("<script language='javascript'>")
        strBuilder.Append("displayIDEASResult();")
        strBuilder.Append("</script>")

        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "DisplayResultInParent", "displayIDEASResult();", True)
    End Sub
#End Region

#Region "UI"

#End Region

#Region "Web Service"
    Private Sub InitServicePointManager()
        Dim callback As New RemoteCertificateValidationCallback(AddressOf ValidateCertificate)
        System.Net.ServicePointManager.ServerCertificateValidationCallback = callback
    End Sub

    Private Function ValidateCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
        'Return True to force the certificate to be accepted.
        Return True
    End Function
#End Region




End Class