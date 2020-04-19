Imports Common.ComFunction
Imports Common.Component

Public Class Resource
    Shared Function Text(resourceKey As String) As String
        'Dim rm = New Global.System.Resources.ResourceManager("Resources.ResourceLanguage", Global.System.Reflection.[Assembly].Load("App_GlobalResources"))
        Dim strLang = Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower
        Dim lang = IIf(strLang.ToLower = Common.Component.CultureLanguage.English, New System.Globalization.CultureInfo(CultureLanguage.English), New System.Globalization.CultureInfo(CultureLanguage.TradChinese))
        Dim txt = HttpContext.GetGlobalResourceObject("Text", resourceKey, lang)
        'Dim txt = rm.GetString(resourceKey)
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

