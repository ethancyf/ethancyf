Public Class ControlBLL

    Private udtEFormBLL As eFormBLL = New eFormBLL

    Public Sub New()

    End Sub

    Public Sub bindDistrict(ByVal ddl As DropDownList, ByVal strAreaCode As String, ByVal blnReset As Boolean)
        ddl.Items.Clear()
        ddl.DataSource = udtEFormBLL.GetDistrict(strAreaCode)
        ddl.DataValueField = "District_ID"
        ddl.DataTextField = "District_Name"
        ddl.DataBind()
        ddl.Items.Insert(0, New ListItem(HttpContext.GetGlobalResourceObject("Text", "SelectDistrict"), ""))
        If blnReset Then
            ddl.SelectedIndex = 0
        End If

    End Sub

    Public Sub bindMORelationship(ByVal rbo As RadioButtonList)
        rbo.DataSource = udtEFormBLL.GetPracticeType
        rbo.DataValueField = "ItemNo"

        If HttpContext.Current.Session("language") = "zh-tw" Then
            rbo.DataTextField = "DataValueChi"
        Else
            rbo.DataTextField = "DataValue"
        End If
        rbo.DataBind()

    End Sub
End Class
