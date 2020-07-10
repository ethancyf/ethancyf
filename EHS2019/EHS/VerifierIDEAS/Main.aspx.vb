Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates

Partial Public Class _Default
    Inherits System.Web.UI.Page


#Region "Session"

    Private Enum EnumIdeasVersion
        One
        Two
        TwoGender
        Combo
    End Enum

    Private Property SessIdeasTokenResponse() As IdeasRM.TokenResponse
        Get
            Return Session("SessIdeasTokenResponse")
        End Get
        Set(ByVal value As IdeasRM.TokenResponse)
            Session("SessIdeasTokenResponse") = IIf(value IsNot Nothing, value, Nothing)
        End Set
    End Property

    Private Property SessIdeasVersion() As EnumIdeasVersion
        Get
            Return Session("SessIdeasVersion")
        End Get
        Set(ByVal value As EnumIdeasVersion)
            Session("SessIdeasVersion") = value
        End Set
    End Property

    Private Property SessIdeasSamlResponse() As IdeasRM.IdeasResponse
        Get
            Return Session("SessIdeasSamlResponse")
        End Get
        Set(ByVal value As IdeasRM.IdeasResponse)
            Session("SessIdeasSamlResponse") = IIf(value IsNot Nothing, value, Nothing)
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

#Region "Event"

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        AddHandler ucIDEASCombo.ShowResult, AddressOf ShowResult
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InitServicePointManager()
        Dim ideasSamlResponse As IdeasRM.IdeasResponse = Nothing

        ClearAllResult()

        If Not Me.IsPostBack Then

            LoadIDEASUrl()

            'Enable IDEAS2 with Gender
            Dim strIDEAS2_ReadGender As String = String.Empty
            strIDEAS2_ReadGender = ConfigurationManager.AppSettings("IDEAS2_ReadGender")

            If strIDEAS2_ReadGender.Equals("Y") Then
                btnReadSmartIC2_5.Visible = True
            Else
                btnReadSmartIC2_5.Visible = False
            End If

            ' Read Token server URL for IDEAS application setting
            Dim objSetting As ClientSettingsSection = ConfigurationManager.GetSection("applicationSettings/IdeasRM.Properties.Settings")
            Me.lblTokenServerURL1.Text = objSetting.Settings.Get("IdeasRM_TokenServiceURLList").Value.ValueXml.InnerText()
            Me.lblTokenServerURL2.Text = objSetting.Settings.Get("Ideas2RM_TokenServiceURLList").Value.ValueXml.InnerText()
            Me.lblBrokerServerURL1.Text = objSetting.Settings.Get("IdeasRM_BrokerServiceURLList").Value.ValueXml.InnerText()
            Me.lblBrokerServerURL2.Text = objSetting.Settings.Get("Ideas2RM_BrokerServiceURLList").Value.ValueXml.InnerText()


            ' If connectted to token server already and page load by redirect from IDEAS after read card (QueryString > 0)
            If SessIdeasTokenResponse IsNot Nothing AndAlso _
                ((SessIdeasVersion <> EnumIdeasVersion.Combo And SessIdeasTokenResponse.IdeasMAURL IsNot Nothing) Or _
                 (SessIdeasVersion = EnumIdeasVersion.Combo And SessIdeasTokenResponse.BrokerURL IsNot Nothing)) _
                And Request.QueryString.Count > 0 Then

                Logger.Log("IdeasTester loaded (Redirect from IDEAS after read SmartIC)")

                Try
                    ' TokenResponse no error status
                    If Request.QueryString("status") Is Nothing Then
                        Logger.Log("IDEAS Version: " + ConvertIdeasVersion(SessIdeasVersion))
                        Logger.Log("Read SmartIC status: ")
                        Logger.Log("Start get CardFaceDate")
                        ' Try to get card face data
                        Dim ideasHelper As IdeasRM.IHelper = IdeasRM.HelpFactory.createHelper()

                        Select Case SessIdeasVersion
                            Case EnumIdeasVersion.One, EnumIdeasVersion.Two, EnumIdeasVersion.TwoGender
                                ideasSamlResponse = ideasHelper.getCardFaceData(SessIdeasTokenResponse, Artifact, ConvertIdeasVersion(SessIdeasVersion))
                            Case EnumIdeasVersion.Combo
                                ideasSamlResponse = ideasHelper.getCardFaceData(SessIdeasTokenResponse, Artifact, False)
                        End Select

                        IDEASCombo.SessIdeasSamlResponse = ideasSamlResponse
                    Else ' TokenResponse has error status
                        Logger.Log("Read SmartIC status: " + Request.QueryString("status"))
                        Me.lblQueryStringStatus.Text = Request.QueryString("status")
                    End If

                Catch ex As Exception
                    ShowUnknownException(ex)
                End Try
            Else
                Logger.Log("IdeasTester loaded")
                SessIdeasTokenResponse = Nothing
                SessIdeasVersion = Nothing
            End If

            ' Show all status/result
            Me.ShowResult(IDEASCombo.SessIdeasSamlResponse)
            Me.ShowIdeasTokenResponse()
            Me.ShowIdeasSamlResponse(IDEASCombo.SessIdeasSamlResponse)
            Me.ShowCardFaceData(IDEASCombo.SessIdeasSamlResponse)
        Else
            ' Clear session on start (Not redirect from IDEAS server)
            'SessIdeasTokenResponse = Nothing
            'SessIdeasVersion = Nothing
        End If

        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "checkIdeasComboClient", "checkIdeasComboClient(checkIdeasComboClientSuccessEHS, checkIdeasComboClientFailureEHS);", True)
    End Sub

    Protected Sub btnReadSmartIC2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReadSmartIC2.Click
        Logger.Log("Read Smart ID Card (IDEAS2) Clicked")
        ReadSmartIC(EnumIdeasVersion.Two)
    End Sub

    Protected Sub btnReadSmartIC2_5_Click(sender As Object, e As EventArgs) Handles btnReadSmartIC2_5.Click
        Logger.Log("Read Smart ID Card (IDEAS2 with Gender) Clicked")
        ReadSmartIC(EnumIdeasVersion.TwoGender)
    End Sub

    Private Sub btnReadSmartIC1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReadSmartIC1.Click
        Logger.Log("Read Smart ID Card (IDEAS1) Clicked")
        ReadSmartIC(EnumIdeasVersion.One)
    End Sub

    Private Sub btnReadSmartICCombo_Click(sender As Object, e As EventArgs) Handles btnReadSmartICCombo.Click
        Logger.Log("Read Smart ID Card (IDEAS Combo) Clicked")
        ReadSmartIC(EnumIdeasVersion.Combo)
    End Sub

    Protected Sub btnReadSmartICComboiframe_Click(sender As Object, e As EventArgs) Handles btnReadSmartICComboiframe.Click
        ucIDEASCombo.ReadSmartIC(IDEASCombo.EnumIdeasVersion.Combo)

        Me.ClearAllResult()
        ' Show all status/result
        If SessIdeasTokenResponse Is Nothing OrElse SessIdeasTokenResponse.BrokerURL Is Nothing Then
            Me.ShowResult(Nothing)
            Me.ShowIdeasTokenResponse()
        End If

    End Sub

    Private Sub ReadSmartIC(ByVal eIdeasVersion As EnumIdeasVersion)
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
        SessIdeasSamlResponse = Nothing
        SessIdeasTokenResponse = Nothing
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
                SessIdeasTokenResponse = ideasHelper.registerBrokerService(ideasHelper.getProperty("IdeasRM_DeptCode"), _
                                                          ideasHelper.getProperty("IdeasRM_RaCode"), AppRefId, _
                                                          Me.Page.Request.Url.GetLeftPart(UriPartial.Path), "Target", "", "", "", "", strLang, "", strRemoveCard, "", False)
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
                    Response.Redirect(SessIdeasTokenResponse.BrokerURL)
                End If
        End Select


        Me.ClearAllResult()
        ' Show all status/result
        Me.ShowResult(Nothing)
        Me.ShowIdeasTokenResponse()

    End Sub

    Private Function ConvertIdeasVersion(ByVal eIdeasVersion As EnumIdeasVersion) As String
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
#End Region

#Region "UI"


    Private Sub ShowIdeasTokenResponse()
        If SessIdeasTokenResponse Is Nothing Then Exit Sub

        Me.lblIdeasTokenResponse_ErrorCode.Text = IIf(SessIdeasTokenResponse.ErrorCode IsNot Nothing, SessIdeasTokenResponse.ErrorCode, "")
        Me.lblIdeasTokenResponse_ErrorDetail.Text = IIf(SessIdeasTokenResponse.ErrorDetail IsNot Nothing, SessIdeasTokenResponse.ErrorDetail, "")
        Me.lblIdeasTokenResponse_ErrorMessage.Text = IIf(SessIdeasTokenResponse.ErrorMessage IsNot Nothing, SessIdeasTokenResponse.ErrorMessage, "")
        Me.lblIdeasTokenResponse_IdeasMAURL.Text = IIf(SessIdeasTokenResponse.IdeasMAURL IsNot Nothing, SessIdeasTokenResponse.IdeasMAURL, "")
    End Sub

    Private Sub ShowIdeasSamlResponse(ByVal ideasSamlResponse As IdeasRM.IdeasResponse)
        If ideasSamlResponse Is Nothing Then Exit Sub

        Me.lblStatusCode.Text = ideasSamlResponse.StatusCode
        Me.lblStatusDetail.Text = ideasSamlResponse.StatusDetail
        Me.lblStatusMessage.Text = ideasSamlResponse.StatusMessage
        Me.lblStackTrace.Text = ideasSamlResponse.StackTrace
    End Sub

    Private Sub ShowCardFaceData(ByVal ideasSamlResponse As IdeasRM.IdeasResponse)
        If ideasSamlResponse Is Nothing Then Exit Sub
        If Not ideasSamlResponse.StatusCode.Equals("samlp:Success") Then Exit Sub

        Me.lblPersionInfo_HKICver.Text = ideasSamlResponse.CardFaceDate.CardVersion
        Me.lblPersionInfo_HKID.Text = ideasSamlResponse.CardFaceDate.HKIDNumberDetails.Identifier.Value & "(" & ideasSamlResponse.CardFaceDate.HKIDNumberDetails.CheckDigit.Value & ")"
        Me.lblPersionInfo_NameEng.Text = ideasSamlResponse.CardFaceDate.PersonalEnglishNameDetails.UnstructuredName.Value.ToUpper

        If Not IsNothing(ideasSamlResponse.CardFaceDate.PersonalChineseNameHKSCSEncodeDetails) Then

            Me.lblPersionInfo_NameChi.Text = ideasSamlResponse.CardFaceDate.PersonalChineseNameHKSCSEncodeDetails.Name.Value

            If Not IsNothing(ideasSamlResponse.CardFaceDate.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode) Then
                If ideasSamlResponse.CardFaceDate.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode.Length > 0 Then
                    Me.lblPersionInfo_CCCode1.Text = ideasSamlResponse.CardFaceDate.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(0).FourDigitCode.Value + ideasSamlResponse.CardFaceDate.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(0).ExtensionNumber.Value
                End If

                If ideasSamlResponse.CardFaceDate.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode.Length > 1 Then
                    Me.lblPersionInfo_CCCode2.Text = ideasSamlResponse.CardFaceDate.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(1).FourDigitCode.Value + ideasSamlResponse.CardFaceDate.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(1).ExtensionNumber.Value
                End If

                If ideasSamlResponse.CardFaceDate.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode.Length > 2 Then
                    Me.lblPersionInfo_CCCode3.Text = ideasSamlResponse.CardFaceDate.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(2).FourDigitCode.Value + ideasSamlResponse.CardFaceDate.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(2).ExtensionNumber.Value
                End If

                If ideasSamlResponse.CardFaceDate.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode.Length > 3 Then
                    Me.lblPersionInfo_CCCode4.Text = ideasSamlResponse.CardFaceDate.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(3).FourDigitCode.Value + ideasSamlResponse.CardFaceDate.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(3).ExtensionNumber.Value
                End If

                If ideasSamlResponse.CardFaceDate.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode.Length > 4 Then
                    Me.lblPersionInfo_CCCode5.Text = ideasSamlResponse.CardFaceDate.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(4).FourDigitCode.Value + ideasSamlResponse.CardFaceDate.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(4).ExtensionNumber.Value
                End If

                If ideasSamlResponse.CardFaceDate.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode.Length > 5 Then
                    Me.lblPersionInfo_CCCode6.Text = ideasSamlResponse.CardFaceDate.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(5).FourDigitCode.Value + ideasSamlResponse.CardFaceDate.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(5).ExtensionNumber.Value
                End If
            End If
        End If

        Me.lblPersionInfo_DOB.Text = ideasSamlResponse.CardFaceDate.DateOfBirth
        Me.lblPersionInfo_Gender.Text = ideasSamlResponse.CardFaceDate.Gender
        Me.lblPersionInfo_DOI.Text = ideasSamlResponse.CardFaceDate.DateOfIssue.ToString("yyyy-MM-dd HH:mm:ss")
    End Sub

    Private Sub ShowResult(ByVal ideasSamlResponse As IdeasRM.IdeasResponse)


        Dim blnResult As Boolean = True

        ' Not show status when first enter this page
        If IDEASCombo.SessIdeasTokenResponse Is Nothing And Me.IsPostBack = False Then Exit Sub

        ' Get ideas token fail
        If blnResult Then If IDEASCombo.SessIdeasTokenResponse Is Nothing Then blnResult = False

        ' Get ideas token fail (with error code)
        If blnResult Then
            If ((SessIdeasVersion <> EnumIdeasVersion.Combo And IDEASCombo.SessIdeasTokenResponse.IdeasMAURL Is Nothing) Or _
                 (SessIdeasVersion = EnumIdeasVersion.Combo And IDEASCombo.SessIdeasTokenResponse.BrokerURL Is Nothing)) Then blnResult = False
        End If


        ' Get Card Face Data fail
        If blnResult Then If ideasSamlResponse Is Nothing Then blnResult = False

        If blnResult Then If Not ideasSamlResponse.StatusCode.Equals("samlp:Success") Then blnResult = False

        If ideasSamlResponse IsNot Nothing Then
            Logger.Log("Ideas SAML Response (StatusCode): " + ideasSamlResponse.StatusCode)
        Else
            Logger.Log("Ideas SAML Response (StatusCode): [No Response]")
        End If

        ' Fill result data

        Select Case SessIdeasVersion
            Case EnumIdeasVersion.One, EnumIdeasVersion.Two, EnumIdeasVersion.TwoGender
                If SessIdeasTokenResponse.IdeasMAURL IsNot Nothing Then
                    lblResult_URL.Text = SessIdeasTokenResponse.IdeasMAURL.Substring(0, SessIdeasTokenResponse.IdeasMAURL.IndexOf("?"))
                Else
                    lblResult_URL.Text = "Nothing"
                End If
            Case EnumIdeasVersion.Combo
                If SessIdeasTokenResponse.BrokerURL IsNot Nothing Then
                    lblResult_URL.Text = SessIdeasTokenResponse.BrokerURL.Substring(0, SessIdeasTokenResponse.BrokerURL.IndexOf("?"))
                Else
                    lblResult_URL.Text = "Nothing"
                End If
        End Select

        If SessIdeasSamlResponse IsNot Nothing AndAlso SessIdeasSamlResponse.CardFaceDate IsNot Nothing Then
            lblResult_HKICVer.Text = SessIdeasSamlResponse.CardFaceDate.CardVersion

            Select Case SessIdeasVersion
                Case EnumIdeasVersion.One
                    lblResult_HKICVer.Text = lblResult_HKICVer.Text + " 1 (IDEAS 1)"
                Case EnumIdeasVersion.Two
                    lblResult_HKICVer.Text = lblResult_HKICVer.Text + " 2 (IDEAS 2)"
                Case EnumIdeasVersion.TwoGender
                    lblResult_HKICVer.Text = lblResult_HKICVer.Text + " 2 (IDEAS 2 with gender)"
                Case EnumIdeasVersion.Combo
                    lblResult_HKICVer.Text = lblResult_HKICVer.Text + " (IDEAS Combo)"
            End Select
        Else
            lblResult_HKICVer.Text = "Nothing"

            Select Case SessIdeasVersion
                Case EnumIdeasVersion.One
                    lblResult_HKICVer.Text = lblResult_HKICVer.Text + " (IDEAS 1)"
                Case EnumIdeasVersion.Two
                    lblResult_HKICVer.Text = lblResult_HKICVer.Text + " (IDEAS 2)"
                Case EnumIdeasVersion.TwoGender
                    lblResult_HKICVer.Text = lblResult_HKICVer.Text + " (IDEAS 2 with gender)"
                Case EnumIdeasVersion.Combo
                    lblResult_HKICVer.Text = lblResult_HKICVer.Text + " (IDEAS Combo)"
            End Select
        End If

         
        If blnResult Then
            Logger.Log("Ideas SAML Response (CardFaceDate.DateOfIssue): " + ideasSamlResponse.CardFaceDate.DateOfIssue)

            Me.lblResult_Status.Text = "Success"
            Me.lblResult_Status.CssClass = "tableTitle"
            Me.lblResult_DOI.Text = ideasSamlResponse.CardFaceDate.DateOfIssue.ToString("dd-MM-yyyy")
            Me.lblResult_Dtm.Text = Now.ToString("dd-MM-yyyy HH:mm:ss")
        Else
            Me.lblResult_Status.Text = "Fail"
            Me.lblResult_Status.CssClass = "tableTitleAlert"
            Me.lblResult_Dtm.Text = Now.ToString("dd-MM-yyyy HH:mm:ss")
        End If

    End Sub

    Private Sub ShowUnknownException(ByVal ex As Exception)
        Logger.Log(ex)
        Me.lblError.Text = ex.ToString
    End Sub

    Private Sub ClearAllResult()
        ' Result
        Me.lblResult_Status.Text = String.Empty
        Me.lblResult_HKICVer.Text = String.Empty
        Me.lblResult_URL.Text = String.Empty
        Me.lblResult_DOI.Text = String.Empty
        Me.lblResult_Dtm.Text = String.Empty

        ' Read Smart IC
        Me.lblQueryStringStatus.Text = String.Empty

        ' Get Card Face Data Result
        Me.lblStatusCode.Text = String.Empty
        Me.lblStatusDetail.Text = String.Empty
        Me.lblStatusMessage.Text = String.Empty
        Me.lblStackTrace.Text = String.Empty

        ' Unknown Exception
        Me.lblError.Text = String.Empty

        ' Card Face Data
        Me.lblPersionInfo_HKID.Text = String.Empty
        Me.lblPersionInfo_NameEng.Text = String.Empty
        Me.lblPersionInfo_NameChi.Text = String.Empty
        Me.lblPersionInfo_CCCode1.Text = String.Empty
        Me.lblPersionInfo_CCCode2.Text = String.Empty
        Me.lblPersionInfo_CCCode3.Text = String.Empty
        Me.lblPersionInfo_CCCode4.Text = String.Empty
        Me.lblPersionInfo_CCCode5.Text = String.Empty
        Me.lblPersionInfo_CCCode6.Text = String.Empty
        Me.lblPersionInfo_DOB.Text = String.Empty
        Me.lblPersionInfo_Gender.Text = String.Empty
        Me.lblPersionInfo_DOI.Text = String.Empty
    End Sub
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

    Protected Sub ShowResult()
        ' Show all status/result
        Me.ShowResult(IDEASCombo.SessIdeasSamlResponse)
        Me.ShowIdeasTokenResponse()
        Me.ShowIdeasSamlResponse(IDEASCombo.SessIdeasSamlResponse)
        Me.ShowCardFaceData(IDEASCombo.SessIdeasSamlResponse)
    End Sub

    Private Sub LoadIDEASUrl()
        ' Read Token server URL for IDEAS application setting
        Dim objSetting As ClientSettingsSection = ConfigurationManager.GetSection("applicationSettings/IdeasRM.Properties.Settings")
        'Me.lblTokenServerURL1.Text = objSetting.Settings.Get("IdeasRM_TokenServiceURLList").Value.ValueXml.InnerText()
        'Me.lblTokenServerURL2.Text = objSetting.Settings.Get("Ideas2RM_TokenServiceURLList").Value.ValueXml.InnerText()
        'Me.lblBrokerServerURL2.Text = objSetting.Settings.Get("Ideas2RM_BrokerServiceURLList").Value.ValueXml.InnerText()


        'Dim config As System.Configuration.Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        Dim strIDEASurl As String
        Dim strIDEASurlList() As String

        ' IDEAS 1
        strIDEASurl = System.Web.Configuration.WebConfigurationManager.AppSettings("IDEAS1_url")
        strIDEASurlList = strIDEASurl.Split(",")

        ddlIDEAS1Url.Items.Clear()
        For Each strURL As String In strIDEASurlList
            ddlIDEAS1Url.Items.Add(New ListItem(strURL.Trim, strURL.Trim))
        Next
        ddlIDEAS1Url.SelectedValue = objSetting.Settings.Get("IdeasRM_TokenServiceURLList").Value.ValueXml.InnerText()

        ' IDEAS 2
        strIDEASurl = System.Web.Configuration.WebConfigurationManager.AppSettings("IDEAS2_url")
        strIDEASurlList = strIDEASurl.Split(",")

        ddlIDEAS2Url.Items.Clear()
        For Each strURL As String In strIDEASurlList
            ddlIDEAS2Url.Items.Add(New ListItem(strURL.Trim, strURL.Trim))
        Next
        ddlIDEAS2Url.SelectedValue = objSetting.Settings.Get("Ideas2RM_TokenServiceURLList").Value.ValueXml.InnerText()


        ' IDEAS Combo
        strIDEASurl = System.Web.Configuration.WebConfigurationManager.AppSettings("IDEASCombo_url")
        strIDEASurlList = strIDEASurl.Split(",")

        ddlIDEASComboUrl.Items.Clear()
        For Each strURL As String In strIDEASurlList
            ddlIDEASComboUrl.Items.Add(New ListItem(strURL.Trim, strURL.Trim))
        Next
        ddlIDEASComboUrl.SelectedValue = objSetting.Settings.Get("Ideas2RM_BrokerServiceURLList").Value.ValueXml.InnerText()

    End Sub

    Private Sub ddlIDEASComboUrl_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlIDEASComboUrl.SelectedIndexChanged

        If ddlIDEASComboUrl.SelectedValue = String.Empty Then Return

        Dim xmlDoc As System.Xml.XmlDocument = New System.Xml.XmlDocument()
        xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile)
        xmlDoc.SelectSingleNode("//applicationSettings/IdeasRM.Properties.Settings/setting[@name='Ideas2RM_BrokerServiceURLList']/value").InnerText = ddlIDEASComboUrl.SelectedValue
        xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile)
        ConfigurationManager.RefreshSection("applicationSettings/IdeasRM.Properties.Settings/Ideas2RM_BrokerServiceURLList")

    End Sub

    Private Sub ddlIDEAS1Url_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlIDEAS1Url.SelectedIndexChanged
        If ddlIDEASComboUrl.SelectedValue = String.Empty Then Return

        Dim xmlDoc As System.Xml.XmlDocument = New System.Xml.XmlDocument()
        xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile)
        xmlDoc.SelectSingleNode("//applicationSettings/IdeasRM.Properties.Settings/setting[@name='IdeasRM_TokenServiceURLList']/value").InnerText = ddlIDEAS1Url.SelectedValue
        xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile)
        ConfigurationManager.RefreshSection("applicationSettings/IdeasRM.Properties.Settings/IdeasRM_TokenServiceURLList")

    End Sub

    Private Sub ddlIDEAS2Url_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlIDEAS2Url.SelectedIndexChanged
        If ddlIDEASComboUrl.SelectedValue = String.Empty Then Return

        Dim xmlDoc As System.Xml.XmlDocument = New System.Xml.XmlDocument()
        xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile)
        xmlDoc.SelectSingleNode("//applicationSettings/IdeasRM.Properties.Settings/setting[@name='Ideas2RM_TokenServiceURLList']/value").InnerText = ddlIDEAS2Url.SelectedValue
        xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile)
        ConfigurationManager.RefreshSection("applicationSettings/IdeasRM.Properties.Settings/Ideas2RM_TokenServiceURLList")

    End Sub
End Class