Imports Common.ComFunction
Imports HCSP.BLL
Imports System.Net

Public Class IDEASCombo
    Inherits System.Web.UI.UserControl

#Region "Session"

    Public Shared Property SessSmartIDContent() As BLL.SmartIDContentModel
        Get
            Return HttpContext.Current.Session("SessSmartIDContent")
        End Get
        Set(ByVal value As BLL.SmartIDContentModel)
            HttpContext.Current.Session("SessSmartIDContent") = IIf(value IsNot Nothing, value, Nothing)
        End Set
    End Property

    Public Shared Property SessIdeasTokenResponse() As IdeasRM.TokenResponse
        Get
            Return HttpContext.Current.Session("SessIdeasTokenResponse")
        End Get
        Set(ByVal value As IdeasRM.TokenResponse)
            HttpContext.Current.Session("SessIdeasTokenResponse") = IIf(value IsNot Nothing, value, Nothing)
        End Set
    End Property

    Public Shared Property SessIdeasVersion() As IdeasBLL.EnumIdeasVersion
        Get
            Return HttpContext.Current.Session("SessIdeasVersion")
        End Get
        Set(ByVal value As IdeasBLL.EnumIdeasVersion)
            HttpContext.Current.Session("SessIdeasVersion") = value
        End Set
    End Property

    Public Shared Property SessIdeasSamlResponse() As IdeasRM.IdeasResponse
        Get
            Return HttpContext.Current.Session("SessIdeasSamlResponse")
        End Get
        Set(ByVal value As IdeasRM.IdeasResponse)
            HttpContext.Current.Session("SessIdeasSamlResponse") = IIf(value IsNot Nothing, value, Nothing)
        End Set
    End Property

    Public Shared Property SessIdeasException() As Exception
        Get
            Return HttpContext.Current.Session("SessIdeasException")
        End Get
        Set(ByVal value As Exception)
            HttpContext.Current.Session("SessIdeasException") = IIf(value IsNot Nothing, value, Nothing)
        End Set
    End Property

    Public Shared Property SessFunctCode() As String
        Get
            Return HttpContext.Current.Session("SessFunctionCode")
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session("SessFunctionCode") = value
        End Set
    End Property

#End Region

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

#Region "Property"

    ''' <summary>
    ''' Show result after IDEAS return to IDEASComboReader.aspx
    ''' </summary>
    ''' <remarks></remarks>
    Public Event ShowResult()

#End Region

    Public Sub ReadSmartIC(ByVal eIdeasVersion As IdeasBLL.EnumIdeasVersion, ByVal udtIDEASTokenResponse As IdeasRM.TokenResponse, ByVal strFunctCode As String)
        'Logger.Log("IdeasReader loaded")
        SessIdeasTokenResponse = Nothing
        SessIdeasTokenResponse = udtIDEASTokenResponse
        SessIdeasVersion = eIdeasVersion
        SessFunctCode = strFunctCode
        ReadSmartICInternal(eIdeasVersion)
    End Sub

    Private Sub ReadSmartICInternal(ByVal eIdeasVersion As IdeasBLL.EnumIdeasVersion)
        Select Case eIdeasVersion
            Case IdeasBLL.EnumIdeasVersion.One, IdeasBLL.EnumIdeasVersion.Two, IdeasBLL.EnumIdeasVersion.TwoGender
                If SessIdeasTokenResponse.IdeasMAURL IsNot Nothing Then
                    'Logger.Log("Redirect page to " + SessIdeasTokenResponse.IdeasMAURL)
                    Response.Redirect(SessIdeasTokenResponse.IdeasMAURL)
                End If
            Case IdeasBLL.EnumIdeasVersion.Combo, IdeasBLL.EnumIdeasVersion.ComboGender
                If SessIdeasTokenResponse.BrokerURL IsNot Nothing Then
                    'Logger.Log("Redirect page to " + SessIdeasTokenResponse.BrokerURL)
                    'Response.Redirect(SessIdeasTokenResponse.BrokerURL)
                    iframeIDEASComboReader.Attributes("src") = SessIdeasTokenResponse.BrokerURL
                    'iframeIDEASComboReader.Attributes("src") = "~/IDEASComboReader.aspx"
                    Me.ModalPopupExtenderIDEASComboReader.Show()
                End If
        End Select

    End Sub

    'Public Shared Function ConvertIdeasVersion(ByVal eIdeasVersion As IdeasBLL.EnumIdeasVersion) As String
    '    Select Case eIdeasVersion
    '        Case IdeasBLL.EnumIdeasVersion.One
    '            Return "1"
    '        Case IdeasBLL.EnumIdeasVersion.Two
    '            Return "2"
    '        Case IdeasBLL.EnumIdeasVersion.TwoGender
    '            Return "2.5"
    '        Case IdeasBLL.EnumIdeasVersion.Combo
    '            Return "Combo"
    '        Case Else
    '            Return String.Empty
    '    End Select
    'End Function

    Private Sub btnDisplayResult_Click(sender As Object, e As EventArgs) Handles btnDisplayResult.Click
        'RaiseEvent ShowResult()

        Dim _udtSessionHandler As New SessionHandler
        Dim udtSmartIDContent As SmartIDContentModel = IDEASCombo.SessSmartIDContent
        _udtSessionHandler.SmartIDContentSaveToSession(IDEASCombo.SessFunctCode, udtSmartIDContent)

        ' Redirect to search claim page, and add page key
        Response.Redirect(RedirectHandler.AppendPageKeyToURL(Me.Page.Request.Url.GetLeftPart(UriPartial.Path)))
    End Sub
End Class