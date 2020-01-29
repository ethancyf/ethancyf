Public Partial Class DistrictLegend
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

    Public Sub BindDistrict(ByVal strLang As String)
        ' Bind scheme
        Dim dt As New DataTable

        dt.Columns.Add(Me.GetGlobalResourceObject("Text", "Abbreviation"))
        dt.Columns.Add(Me.GetGlobalResourceObject("Text", "Description"))

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        'Dim dtSDDistrictList As DataTable = (New StatisticsBLL).GetSDDistrictAll
        Dim dtSDDistrictList As DataTable = (New StatisticsBLL).GetDistrictBoardList()
        'CRE13-019-02 Extend HCVS to China [End][Winnie]

        For Each row As DataRow In dtSDDistrictList.Rows
            Dim dr As DataRow = dt.NewRow()
            dr.Item(0) = row.Item("district_board_shortname_SD").ToString.Trim
            If strLang = English Then
                dr.Item(1) = row.Item("district_board").ToString.Trim
            Else
                dr.Item(1) = row.Item("district_board_chi").ToString.Trim
            End If
            dt.Rows.Add(dr)
        Next

        ' For Department of Health
        Dim drHealth As DataRow = dt.NewRow
        Dim strDepartmentOfHealth As String = Me.GetGlobalResourceObject("Text", "DepartmentOfHealth")
        drHealth.Item(0) = "DH"
        drHealth.Item(1) = strDepartmentOfHealth.ToUpper()
        dt.Rows.Add(drHealth)

        ' Perform sorting on Scheme Name column
        dt.DefaultView.Sort = "Abbreviation Asc"

        'dt.Columns.RemoveAt(2)

        gvDistrict.DataSource = dt
        gvDistrict.DataBind()

        gvDistrict.Rows(0).Cells(0).Width = 100
        gvDistrict.Rows(0).Cells(1).Width = 200
        'gvProfession.Rows(0).Cells(2).Width = 120
        For Each r As GridViewRow In gvDistrict.Rows
            r.Cells(0).VerticalAlign = VerticalAlign.Top
        Next

    End Sub

End Class