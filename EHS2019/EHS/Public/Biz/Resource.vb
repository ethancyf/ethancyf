Imports Common.ComFunction
Imports Common.Component

Public Class Resource
    Shared Function Text(resourceKey As String) As String
        Dim txt = Nothing
        If XMLMain.DBLink Then
            Dim strLang = Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower
            Dim lang = IIf(strLang.ToLower = Common.Component.CultureLanguage.English, New System.Globalization.CultureInfo(CultureLanguage.English), New System.Globalization.CultureInfo(CultureLanguage.TradChinese))
            txt = HttpContext.GetGlobalResourceObject("Text", resourceKey, lang)
        Else
            Dim rm = New Global.System.Resources.ResourceManager("Resources.ResourceLanguage", Global.System.Reflection.[Assembly].Load("App_GlobalResources"))
            txt = rm.GetString(resourceKey)
        End If

        If txt Is Nothing Then
            txt = "<Value>"
        End If

        Return txt
    End Function

    Shared Function Text(resourceKey As String, strLang As String) As String
        Dim txt = Nothing
        If XMLMain.DBLink Then
            Dim lang = IIf(strLang.ToLower = Common.Component.CultureLanguage.English, New System.Globalization.CultureInfo(CultureLanguage.English), New System.Globalization.CultureInfo(CultureLanguage.TradChinese))
            txt = HttpContext.GetGlobalResourceObject("Text", resourceKey, lang)
        Else
            Dim rm = New Global.System.Resources.ResourceManager("Resources.ResourceLanguage", Global.System.Reflection.[Assembly].Load("App_GlobalResources"))
            txt = rm.GetString(resourceKey)
        End If

        If txt Is Nothing Then
            txt = "<Value>"
        End If

        Return txt
    End Function

    Shared Function Parameter(ByVal strResourceKey As String) As String
        Dim udtGeneralFunction As New GeneralFunction

        Dim strValue As String
        If XMLMain.DBLink Then
            strValue = udtGeneralFunction.getSystemParameter(strResourceKey)

            If strValue Is Nothing Then
                strValue = "<Value>"
            End If
        Else
            strValue = Text(strResourceKey)
        End If

        Return strValue

    End Function
End Class

