'---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

Public Class RedirectHandler
    Public Shared Sub ToURL(ByVal URL As String)
        Dim strURL As String
        strURL = String.Empty
        If Not URL.ToString Is Nothing Then
            strURL = URL.ToString
        End If
        HttpContext.Current.Response.Redirect(AppendPageKeyToURL(strURL))
    End Sub

    Public Shared Function AppendPageKeyToURL(ByVal URL As String) As String
        Dim strSessionPageKey As String
        strSessionPageKey = String.Empty
        If Not HttpContext.Current.Session(BasePage.SESS_PageKey) Is Nothing Then
            strSessionPageKey = HttpContext.Current.Session(BasePage.SESS_PageKey).ToString()
        End If
        Return AppendParameterToURL(URL, "PageKey", strSessionPageKey)
    End Function

    Public Shared Function AppendParameterToURL(ByVal URL As String, ByVal name As String, ByVal value As String) As String

        If Not RedirectHandler.IsTurnOnConcurrentBrowserHandling Then
            Return URL
        End If

        Dim tempURL As String
        tempURL = String.Empty
        If Not URL.ToString Is Nothing Then
            tempURL = URL.ToString
        End If
        'Prevent duplicate parameter
        If Not tempURL.Contains(name) Then
            'Check if there is any other parameter in the query string
            If tempURL.Contains("?") Then
                tempURL = tempURL + "&"
            Else
                tempURL = tempURL + "?"
            End If
            tempURL = tempURL + name.ToString + "=" + value.ToString
        End If
        Return tempURL

    End Function

    Public Shared Function IsTurnOnConcurrentBrowserHandling() As Boolean

        Dim isTurnOn As Boolean = False

        Dim strConcurrentBrowserHandling As String = String.Empty

        strConcurrentBrowserHandling = ConfigurationManager.AppSettings("ConcurrentBrowserHandling")

        If strConcurrentBrowserHandling.Trim.Equals("Y") Then
            isTurnOn = True
        End If

        Return isTurnOn

    End Function

    Public Shared Function GetLinkButtonPopupJS(ByVal strLinkNameEn As String, ByVal strLinkNameZh As String, ByVal strLinkNameCn As String)
        Dim strURL As String = String.Empty

        Dim generalFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction()
        Dim strLinkZhJS As String = String.Empty
        Dim strLinkEnJS As String = String.Empty

        If LCase(HttpContext.Current.Session("language")) = Common.Component.CultureLanguage.TradChinese Then
            generalFunction.getSystemParameter(strLinkNameZh, strLinkZhJS, String.Empty)
            strURL = String.Format("javascript:openNewWin('{0}'); return false;", strLinkZhJS)
        ElseIf LCase(HttpContext.Current.Session("language")) = Common.Component.CultureLanguage.SimpChinese Then
            strURL = String.Format("javascript:openNewWin('{0}'); return false;", generalFunction.getSystemParameter(strLinkNameCn))
        Else
            generalFunction.getSystemParameter(strLinkNameEn, strLinkEnJS, String.Empty)
            strURL = String.Format("javascript:openNewWin('{0}'); return false;", strLinkEnJS)
        End If
        Return strURL
    End Function
End Class

'---[CRE11-016] Concurrent Browser Handling [2010-02-01] End