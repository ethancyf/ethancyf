Imports System.Data.SqlClient
Imports System.Web.Security.AntiXss
Imports eID.Bussiness.Impl
Imports eService.Common
Imports eService.DTO.Enum
Imports eService.DTO.Response
Imports System.IO
Imports Common.iAMSmart

Partial Public Class IAS
    Inherits BasePage

    Dim udtiAMSmartBLL As New Common.Component.iAMSmart.iAMSmartBLL

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Buffer = True
        Response.ExpiresAbsolute = Now().Subtract(New TimeSpan(1, 0, 0, 0))
        Response.Expires = 0
        Response.CacheControl = "no-cache"

        Page.Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If Not IsPostBack Then
            Dim strState As String = String.Empty
            Dim strDBState As String = String.Empty
            Dim strErrorCode As String = String.Empty
            Dim strCode As String = String.Empty
            Dim strCookies As String = String.Empty

            ''---------------------------------
            ''  Get AES key
            ''---------------------------------
            ''Get AES Key from system variable 
            'Dim strAESKey As String = udtiAMSmartBLL.GetAESKey()

            'If String.IsNullOrEmpty(strAESKey) Then
            '    'Generate AES Key
            '    strAESKey = (New iAMSmartSPBLL).GetAESKey()
            '    'Update the AES Key to system variable
            '    udtiAMSmartBLL.AddAESKey(strAESKey)
            'End If

            '-------------------------------------------
            '  Classify the step by querystring values
            '-------------------------------------------
            Dim blnMatchCase As Boolean = False

            ' Case 0 - Error
            If Not blnMatchCase Then
                strErrorCode = Context.Request.QueryString(iAMSmartSPBLL.PARAM_ERROR_CODE)
                If Not String.IsNullOrEmpty(strErrorCode) Then
                    Select Case strErrorCode
                        'D40000 - Cancel Request
                        'D40001 - Reject Request
                        Case "D40000", "D40001"
                            lblReturnCode.Text = strErrorCode
                            lblReturnCodeMsg.Text = iAMSmartSPBLL.GetReturnCodeMessage(strErrorCode)
                            lblOpenID.Text = "N/A"
                        Case Else
                            lblReturnCode.Text = strErrorCode
                            lblReturnCodeMsg.Text = iAMSmartSPBLL.GetReturnCodeMessage(strErrorCode)
                            lblOpenID.Text = "N/A"
                    End Select

                    blnMatchCase = True

                End If
            End If

            ' Case 1 - Callback from get QR code
            If Not blnMatchCase Then
                strCode = Context.Request.QueryString(iAMSmartSPBLL.PARAM_CODE)
                strState = Context.Request.QueryString(iAMSmartSPBLL.PARAM_STATE)
                strCookies = Context.Request.QueryString(iAMSmartSPBLL.PARAM_COOKIE)

                If Not String.IsNullOrEmpty(strCode) AndAlso Not String.IsNullOrEmpty(strState) Then
                    '---------------------------------------------
                    ' 1. Get Access Token, Open ID
                    '---------------------------------------------
                    Dim udtLoginServiceImpl As eID.Bussiness.Impl.LoginServiceImpl = New eID.Bussiness.Impl.LoginServiceImpl()

                    Dim udtAccTokenDto As eService.DTO.Response.ResponseDTO(Of eService.DTO.Response.AccessTokenDTO) = Nothing

                    Dim udtiAMSmartBLL As New Common.Component.iAMSmart.iAMSmartBLL

                    Dim strAESKey As String = udtiAMSmartBLL.GetAESKey()
                    Dim strSecretKey As String = AuthConstants.SECRETKEY
                    Dim strClientID As String = AuthConstants.CLIENTID
                    Dim strAccessTokenURL As String = AuthConstants.ACCTOKEN_URL

                    lblAESKey.Text = strAESKey
                    lblSecretKey.Text = strSecretKey
                    lblClientID.Text = strClientID
                    lblAccTokenURL.Text = strAccessTokenURL

                    Try
                        'Get access token for authentication
                        udtAccTokenDto = udtLoginServiceImpl.GetAccessToken(strCode, strState, Nothing)

                        If udtAccTokenDto Is Nothing Then
                            lblReturnCode.Text = "N/A"
                            lblReturnCodeMsg.Text = "Return Nothing"
                            lblOpenID.Text = "N/A"
                            'Return False

                        ElseIf udtAccTokenDto.Code <> "D00000" OrElse udtAccTokenDto.Content Is Nothing OrElse String.IsNullOrEmpty(udtAccTokenDto.Content.AccessToken) Then
                            lblReturnCode.Text = udtAccTokenDto.Code
                            lblReturnCodeMsg.Text = iAMSmartSPBLL.GetReturnCodeMessage(udtAccTokenDto.Code)
                            lblOpenID.Text = "N/A"
                            'Return False
                        Else
                            lblReturnCode.Text = udtAccTokenDto.Code
                            lblReturnCodeMsg.Text = iAMSmartSPBLL.GetReturnCodeMessage(udtAccTokenDto.Code)
                            lblOpenID.Text = udtAccTokenDto.Content.OpenID
                        End If

                    Catch ex As Exception
                        lblReturnCode.Text = "N/A"
                        lblReturnCodeMsg.Text = "Unexpected Exception"
                        lblOpenID.Text = "N/A"

                    End Try

                    blnMatchCase = True

                End If

            End If

        End If

    End Sub

    Protected Sub btnQRCode_Click(sender As Object, e As EventArgs)

        '--------------------------------------
        '---- Get Browser & Language
        '--------------------------------------
        Dim strBrowser As String = iAMSmartSPBLL.CheckBrowserType(HttpContext.Current.Request.UserAgent)
        Dim strLanguage As String = iAMSmartSPBLL.CheckLanguage(Session("language"))
        '--------------------------------------
        '---- Go QR Code
        '--------------------------------------
        Dim strUrl As String = (New iAMSmartSPBLL).getQR(strBrowser, strLanguage, String.Empty)

        Dim intStartPos As Integer = strUrl.IndexOf("&state=")

        Dim strPart1 As String = Left(strUrl, intStartPos + Len("&state="))

        Dim strRemain As String = Mid(strUrl, intStartPos + Len("&state=") + 1)

        Dim intStartPos2 As Integer = strRemain.IndexOf("&")

        Dim strPart2 As String = Left(strRemain, intStartPos2)

        Dim strPart3 As String = Mid(strRemain, intStartPos2 + 1)

        strUrl = strPart1 & strPart2 & "Demo" & strPart3

        '--------------------------------------
        '---- Redirect QR code web page
        '--------------------------------------
        HttpContext.Current.Response.Redirect(strUrl)

    End Sub
  
    'Protected Sub btnProfile_Click(sender As Object, e As EventArgs)
    '    '---------------------------------
    '    '   Get AccessToken
    '    '---------------------------------
    '    Dim strTokenID As String = CStr(HttpContext.Current.Session(SESS_iAMSmart.TokenID))
    '    Dim dtAccessToken As DataTable = udtiAMSmartBLL.GetAccessTokenByOpenID_iAMSmart(strTokenID)
    '    '---------------------------------
    '    '   Get State
    '    '---------------------------------
    '    Dim strState As String = CStr(HttpContext.Current.Session(SESS_iAMSmart.State))
    '    '---------------------------------
    '    '   Get BusinessID
    '    '---------------------------------
    '    Dim strBusinessID As String = iAMSmartSPBLL.GenerateBusinessID()
    '    '---------------------------------
    '    '   Get lstProfileFields
    '    '---------------------------------
    '    Dim lstProfileFields As New List(Of String)
    '    lstProfileFields.Add("HKIC")
    '    '---------------------------------
    '    '   Get Profile
    '    '---------------------------------
    '    Dim udtProfileDto As eService.DTO.Response.ResponseDTO(Of eService.DTO.Response.ResProfileDTO) = Nothing
    '    udtProfileDto = (New iAMSmartSPBLL).GetProfile(dtAccessToken, HttpContext.Current.Request.UserAgent, strState, "en-US", strBusinessID, lstProfileFields)

    '    'If udtProfileDto IsNot Nothing Then
    '    '    blnAuthByQRCode = udtProfileDto.Content.AuthByQR
    '    '    strReturnCode = udtProfileDto.Code
    '    '    strTicketID = udtProfileDto.Content.TicketID

    '    'End If
    'End Sub

#Region "Necessary"
    Protected Overrides Sub InitializeCulture()
        Dim selectedValue As String = String.Empty

        If Not Request(PostBackEventTarget) Is Nothing Then
            'Dim controlID As String = Request.Form(PostBackEventTarget)
            Dim controlID As String = Page.Request.Params.Get(PostBackEventTarget)
            If controlID.Equals(_SelectTradChinese) Then
                selectedValue = TradChinese
                Session("language") = selectedValue
            ElseIf controlID.Equals(_SelectSimpChinese) Then
                selectedValue = SimpChinese
                Session("language") = selectedValue
            ElseIf controlID.Equals(_SelectEnglish) Then
                selectedValue = English
                Session("language") = selectedValue
            End If
        End If

        MyBase.InitializeCulture()

    End Sub

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Account which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Transaction which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Service Provider which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        Return Nothing
    End Function
    ''' <summary>
    ''' CRE11-004
    ''' Retrieve Document Code which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetDocCode() As String
        Return Nothing
    End Function

#End Region
End Class