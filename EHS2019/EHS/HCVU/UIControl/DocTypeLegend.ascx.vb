Imports Common.Component.DocType

Partial Public Class DocTypeLegend
    Inherits System.Web.UI.UserControl

    Public Const TradChinese As String = "zh-tw"
    Public Const English As String = "en-us"

#Region "Private Member"
    Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL
#End Region

#Region "Constructor"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
#End Region

    Public Sub BindDocType(ByVal strLang As String)
        ' Bind scheme
        Dim dt As New DataTable

        dt.Columns.Add(Me.GetGlobalResourceObject("Text", "Abbreviation"))
        dt.Columns.Add(Me.GetGlobalResourceObject("Text", "DocumentType"))
        dt.Columns.Add("DisplaySeq")
        dt.Columns.Add(Me.GetGlobalResourceObject("Text", "IdentityDocNo"))

        Dim udtDocTypeModelCollection As DocTypeModelCollection = udtDocTypeBLL.getAllDocType()

        For Each udtDocTypeModel As DocTypeModel In udtDocTypeModelCollection
            Dim dr As DataRow = dt.NewRow()
            dr.Item(0) = udtDocTypeModel.DocDisplayCode.Trim
            dr.Item(1) = udtDocTypeModel.DocName(strLang).Trim
            dr.Item(2) = udtDocTypeModel.DisplaySeq
            dr.Item(3) = udtDocTypeModel.DocIdentityDesc(strLang)
            dt.Rows.Add(dr)
        Next

        ' Perform sorting on Scheme Name column
        dt.DefaultView.Sort = "DisplaySeq Asc"

        dt.Columns.RemoveAt(2)

        gvDocType.DataSource = dt
        gvDocType.DataBind()

        gvDocType.Rows(0).Cells(0).Width = 100
        gvDocType.Rows(0).Cells(2).Width = 140
        'gvDocType.Rows(0).Cells(2).Width = 120
        For Each r As GridViewRow In gvDocType.Rows
            r.Cells(0).VerticalAlign = VerticalAlign.Top
        Next

    End Sub
End Class