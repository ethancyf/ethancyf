Public Partial Class thankyou1
    Inherits TextOnlyBasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strLanguage As String = String.Empty

        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        Dim strlink As String = String.Empty
        If Not HttpContext.Current.Request.CurrentExecutionFilePath.Equals(HttpContext.Current.Request.Path) Then
            If HttpContext.Current.Session("language") Is Nothing Then
                strlink = "~/text/en/invalidlink.aspx"
            Else
                If HttpContext.Current.Session("language") = "zh-tw" Then
                    strlink = "~/text/zh/invalidlink.aspx"
                Else
                    strlink = "~/text/en/invalidlink.aspx"
                End If
            End If
            Response.Redirect(strlink)
        End If

        Me.PageTitle.Text = Me.GetGlobalResourceObject("Title", "VoucherBalanceEnquiry")

        strLanguage = Session("language")
        Session.RemoveAll()
        Session("language") = strLanguage

        SetLangage()
    End Sub

    Private Sub SetLangage()
        Dim selectedValue As String

        selectedValue = Session("language")

        Select Case selectedValue
            Case English
                lnkbtnEnglish.Enabled = False
                lnkbtnTradChinese.Enabled = True
            Case TradChinese
                lnkbtnEnglish.Enabled = True
                lnkbtnTradChinese.Enabled = False
            Case Else
                lnkbtnEnglish.Enabled = True
                lnkbtnTradChinese.Enabled = False
        End Select
    End Sub
End Class