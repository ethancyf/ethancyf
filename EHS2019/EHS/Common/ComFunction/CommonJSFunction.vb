Imports System.Web.UI

Namespace ComFunction

    Public Class CommonJSFunction
        Public Shared Sub SetDefaultFocus(ByRef objControl As Control)
            Dim strBuilder As StringBuilder = New StringBuilder

            Dim parent As Control = objControl.Parent()
            While Not TypeOf parent Is HtmlForm
                parent = parent.Parent
            End While

            strBuilder.Append("<script language='javascript'>")
            strBuilder.Append("function setFocus() {")
            strBuilder.Append("if (document." & parent.ClientID & "." & objControl.ClientID & ".isDisabled == false) {")
            strBuilder.Append("document." & parent.ClientID & "." & objControl.ClientID & ".focus();")
            If TypeOf objControl Is TextBox Then strBuilder.Append("document." & parent.ClientID & "." & objControl.ClientID & ".select();")
            strBuilder.Append("}")
            strBuilder.Append("}")
            'strBuilder.Append(" window.onload=setTimeout('setFocus();',500);")
            strBuilder.Append(" setTimeout('setFocus();',100);")
            strBuilder.Append("</script>")
            'objControl.Page.RegisterClientScriptBlock("Focus", strBuilder.ToString)

            ' ClientScript.RegisterStartupScript()
            ' objControl.Page.RegisterStartupScript("Focus", strBuilder.ToString)

        End Sub

        Public Shared Sub SetDefaultFocusWithoutChecking(ByRef objControl As Control)
            Dim strBuilder As StringBuilder = New StringBuilder

            strBuilder.Append("<script language='javascript'>")
            strBuilder.Append("function setFocus() {")
            strBuilder.Append("document.all." & objControl.ClientID & ".focus();")
            If TypeOf objControl Is TextBox Then strBuilder.Append("document.all." & objControl.ClientID & ".select();")
            strBuilder.Append("}")
            strBuilder.Append(" setTimeout('setFocus();',100);")
            strBuilder.Append("</script>")
            'objControl.Page.RegisterStartupScript("Focus", strBuilder.ToString)

        End Sub

        Public Shared Sub SetDefaultFocusWithCursorEnd(ByRef objControl As Control)
            Dim strBuilder As StringBuilder = New StringBuilder

            Dim parent As Control = objControl.Parent()
            While Not TypeOf parent Is HtmlForm
                parent = parent.Parent
            End While

            strBuilder.Append("<script language='javascript'>")
            strBuilder.Append("function setFocusWithCursorEnd() {")
            'strBuilder.Append("document." & parent.ClientID & "." & objControl.ClientID & ".focus();")
            strBuilder.Append("oTxtRange = document." & parent.ClientID & "." & objControl.ClientID & ".createTextRange();")
            strBuilder.Append("oTxtRange.move('character', document." & parent.ClientID & "." & objControl.ClientID & ".value.length);")
            strBuilder.Append("oTxtRange.select();")

            strBuilder.Append("}")
            strBuilder.Append("setTimeout('setFocusWithCursorEnd();',20);")
            strBuilder.Append("</script>")
            ' objControl.Page.RegisterStartupScript("FocusWithCursorEnd", strBuilder.ToString)

        End Sub
    End Class

End Namespace