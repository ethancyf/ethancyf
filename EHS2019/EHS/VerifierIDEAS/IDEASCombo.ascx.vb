Public Class IDEASCombo
    Inherits System.Web.UI.UserControl

#Region "Session"

    Public Enum EnumIdeasVersion
        One
        Two
        TwoGender
        Combo
    End Enum

    Public Shared Property SessIdeasTokenResponse() As IdeasRM.TokenResponse
        Get
            Return HttpContext.Current.Session("SessIdeasTokenResponse")
        End Get
        Set(ByVal value As IdeasRM.TokenResponse)
            HttpContext.Current.Session("SessIdeasTokenResponse") = IIf(value IsNot Nothing, value, Nothing)
        End Set
    End Property

    Public Shared Property SessIdeasVersion() As EnumIdeasVersion
        Get
            Return HttpContext.Current.Session("SessIdeasVersion")
        End Get
        Set(ByVal value As EnumIdeasVersion)
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

    Public Sub ReadSmartIC(ByVal eIdeasVersion As EnumIdeasVersion)
        Logger.Log("IdeasReader loaded")
        SessIdeasTokenResponse = Nothing
        SessIdeasVersion = eIdeasVersion
        ReadSmartICInternal(eIdeasVersion)
    End Sub

    Private Sub ReadSmartICInternal(ByVal eIdeasVersion As EnumIdeasVersion)
        Dim ideasHelper As IdeasRM.IHelper = IdeasRM.HelpFactory.createHelper()

        Dim isDemoVersion As String = String.Empty
        Dim strLang As String = String.Empty

        strLang = ConfigurationManager.AppSettings("IDEASLang")
        'strLang = "zh_HK"
        'strLang = "en_US"

        ' Remove Card Setting Read From SystemParameters
        Dim strRemoveCard As String = String.Empty
        'Me._udtGeneralFunction.getSystemParameter("SmartID_RemoveCard", strRemoveCard, String.Empty)
        'If strRemoveCard = String.Empty Then
        strRemoveCard = "Y"
        'End If


        ' Get Token From Ideas, input: the return URL from Ideas to eHS
        SessIdeasVersion = eIdeasVersion
        Select Case eIdeasVersion
            Case EnumIdeasVersion.One
                SessIdeasTokenResponse = ideasHelper.getToken(ideasHelper.getProperty("IdeasRM_DeptCode"), _
                                                          ideasHelper.getProperty("IdeasRM_RaCode"), AppRefId, _
                                                          Me.Page.Request.Url.GetLeftPart(UriPartial.Path), "Target", strLang, strRemoveCard, ConvertIdeasVersion(eIdeasVersion))
            Case EnumIdeasVersion.Two
                SessIdeasTokenResponse = ideasHelper.getToken(ideasHelper.getProperty("IdeasRM_DeptCode"), _
                                                          ideasHelper.getProperty("IdeasRM_RaCode"), AppRefId, _
                                                          Me.Page.Request.Url.GetLeftPart(UriPartial.Path), "Target", strLang, strRemoveCard, ConvertIdeasVersion(eIdeasVersion))
            Case EnumIdeasVersion.TwoGender
                SessIdeasTokenResponse = ideasHelper.getToken(ideasHelper.getProperty("IdeasRM_DeptCode"), _
                                                          ideasHelper.getProperty("Ideas2RM_RaCode"), AppRefId, _
                                                          Me.Page.Request.Url.GetLeftPart(UriPartial.Path), "Target", strLang, strRemoveCard, ConvertIdeasVersion(eIdeasVersion))
            Case EnumIdeasVersion.Combo
                Dim strPageName As String = New IO.FileInfo(Me.Request.Url.LocalPath).Name
                Dim strComboReturnURL As String = Me.Page.Request.Url.GetLeftPart(UriPartial.Path)
                strComboReturnURL = strComboReturnURL.Replace("/" + strPageName, "/IDEASComboReader.aspx")
                SessIdeasTokenResponse = ideasHelper.registerBrokerService(ideasHelper.getProperty("IdeasRM_DeptCode"), _
                                                          ideasHelper.getProperty("IdeasRM_RaCode"), AppRefId, _
                                                          strComboReturnURL, "Target", "", "", "", "", strLang, "", strRemoveCard, "", False)
        End Select


        Logger.Log("Ideas Token Response (ErrorCode): " + SessIdeasTokenResponse.ErrorCode)
        Logger.Log("Ideas Token Response (ErrorMessage): " + SessIdeasTokenResponse.ErrorMessage)
        Logger.Log("Ideas Token Response (IdeasMAURL): " + SessIdeasTokenResponse.IdeasMAURL)

        Select Case eIdeasVersion
            Case EnumIdeasVersion.One, EnumIdeasVersion.Two, EnumIdeasVersion.TwoGender
                If SessIdeasTokenResponse.IdeasMAURL IsNot Nothing Then
                    Logger.Log("Redirect page to " + SessIdeasTokenResponse.IdeasMAURL)
                    Response.Redirect(SessIdeasTokenResponse.IdeasMAURL)
                End If
            Case EnumIdeasVersion.Combo
                If SessIdeasTokenResponse.BrokerURL IsNot Nothing Then
                    Logger.Log("Redirect page to " + SessIdeasTokenResponse.BrokerURL)
                    'Response.Redirect(SessIdeasTokenResponse.BrokerURL)
                    iframeIDEASComboReader.Attributes("src") = SessIdeasTokenResponse.BrokerURL
                    'iframeIDEASComboReader.Attributes("src") = "~/IDEASComboReader.aspx"
                    Me.ModalPopupExtenderIDEASComboReader.Show()
                End If
        End Select

    End Sub

    Public Shared Function ConvertIdeasVersion(ByVal eIdeasVersion As EnumIdeasVersion) As String
        Select Case eIdeasVersion
            Case EnumIdeasVersion.One
                Return "1"
            Case EnumIdeasVersion.Two
                Return "2"
            Case EnumIdeasVersion.TwoGender
                Return "2.5"
            Case EnumIdeasVersion.Combo
                Return "Combo"
            Case Else
                Return String.Empty
        End Select
    End Function

    Private Sub btnDisplayResult_Click(sender As Object, e As EventArgs) Handles btnDisplayResult.Click
        RaiseEvent ShowResult()
    End Sub
End Class