Imports Common.Component.Profession

Partial Public Class ProfessionLegend
    Inherits System.Web.UI.UserControl

    Public Const TradChinese As String = "zh-tw"
    Public Const English As String = "en-us"

#Region "Private Member"
    'Private udtProfessionBLL As ProfessionBLL = New ProfessionBLL
#End Region

#Region "Constructor"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
#End Region

    Public Sub BindProfession(ByVal strLang As String)
        ' Bind scheme
        Dim dt As New DataTable

        dt.Columns.Add(Me.GetGlobalResourceObject("Text", "Abbreviation"))
        dt.Columns.Add(Me.GetGlobalResourceObject("Text", "Description"))
        dt.Columns.Add("DisplaySeq")

        Dim udtProfessionModelCollection As ProfessionModelCollection = ProfessionBLL.GetProfessionList

        For Each udtProfessionModel As ProfessionModel In udtProfessionModelCollection
            Dim dr As DataRow = dt.NewRow()
            dr.Item(0) = udtProfessionModel.ServiceCategoryCode.ToString.Trim
            If strLang = English Then
                dr.Item(1) = udtProfessionModel.ServiceCategoryDesc.Trim
            Else
                dr.Item(1) = udtProfessionModel.ServiceCategoryDescChi.Trim
            End If
            dr.Item(2) = udtProfessionModel.SDDisplaySeq
            dt.Rows.Add(dr)
        Next

        ' For Department of Health
        Dim drHealth As DataRow = dt.NewRow
        Dim strDepartmentOfHealth As String = Me.GetGlobalResourceObject("Text", "DepartmentOfHealth")
        drHealth.Item(0) = "DH"
        drHealth.Item(1) = strDepartmentOfHealth
        drHealth.Item(2) = 1000
        dt.Rows.Add(drHealth)

        ' Perform sorting on Scheme Name column
        dt.DefaultView.Sort = "Abbreviation Asc"

        dt.Columns.RemoveAt(2)

        gvProfession.DataSource = dt
        gvProfession.DataBind()

        gvProfession.Rows(0).Cells(0).Width = 100
        gvProfession.Rows(0).Cells(1).Width = 350
        'gvProfession.Rows(0).Cells(2).Width = 120
        For Each r As GridViewRow In gvProfession.Rows
            r.Cells(0).VerticalAlign = VerticalAlign.Top
        Next

    End Sub

End Class