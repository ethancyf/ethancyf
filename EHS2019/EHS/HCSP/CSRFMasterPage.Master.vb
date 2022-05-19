Imports Common.ComFunction
Imports Common.Component

Partial Public Class CSRFMasterPage
    Inherits System.Web.UI.MasterPage

    Protected ReadOnly Property PageLanguage() As String
        Get
            If SubPlatform() = EnumHCSPSubPlatform.CN Then
                Return "lang=""zh"""
            Else
                Return String.Empty
            End If
        End Get
    End Property

    Protected ReadOnly Property SubPlatform() As EnumHCSPSubPlatform
        Get
            Dim strSubPlatform As String = ConfigurationManager.AppSettings("SubPlatform")
            Dim enumSubPlatform As EnumHCSPSubPlatform = EnumHCSPSubPlatform.HK

            If Not IsNothing(strSubPlatform) Then
                enumSubPlatform = [Enum].Parse(GetType(EnumHCSPSubPlatform), strSubPlatform)
            End If

            Return enumSubPlatform

        End Get

    End Property

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim udcGeneralFun = New Common.ComFunction.GeneralFunction()
        Me.basetag.Attributes("href") = udcGeneralFun.getPageBasePath()

        'Find SubPlatform code
        Dim enumSubPlatform As EnumHCSPSubPlatform = SubPlatform()

        'Set CSS file by SubPlatform code       
        Dim strVersion As String = (New GeneralFunction).GetSystemParameterParmValue1("CSS_Style_Version")

        Dim arrFilePath As New ArrayList()

        If enumSubPlatform = EnumHCSPSubPlatform.CN Then
            arrFilePath.Add("./CSS/CommonStyle_cn.css?ver=")
            arrFilePath.Add("./CSS/MenuStyle_cn.css?ver=")
        Else
            Dim strPath As String = HttpContext.Current.Request.Path

            If Not strPath.Contains("/text/") Then
                arrFilePath.Add("./CSS/CommonStyle.css?ver=")
                arrFilePath.Add("./CSS/MenuStyle.css?ver=")
            End If

        End If

        'Create HTML control for: <link href="../CSS/CommonStyle_cn.css" rel="stylesheet" type="text/css" />
        For Each strItem As String In arrFilePath
            Dim hlCSS As HtmlLink = New HtmlLink
            hlCSS.Href = strItem & strVersion
            hlCSS.Attributes.Add("rel", "stylesheet")
            hlCSS.Attributes.Add("type", "text/css")
            Page.Header.Controls.Add(hlCSS)
        Next

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        CSRFToken.Text = CSRFTokenHelper.doCSRF(CSRFTokenHelper.EnumMasterPage.CSRF)

    End Sub

End Class