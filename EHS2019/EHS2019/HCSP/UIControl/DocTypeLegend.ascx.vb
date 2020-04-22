Imports Common.Component
Imports Common.Component.DocType

Partial Public Class DocTypeLegend
    Inherits System.Web.UI.UserControl

    'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
#Region "Private Member"

    Private _enumDocTypeLegendSubPlatform As [Enum]

#End Region

#Region "Property"

    Public Property DocTypeLegendSubPlatform() As [Enum]
        Get
            Return Me._enumDocTypeLegendSubPlatform
        End Get
        Set(ByVal value As [Enum])
            Me._enumDocTypeLegendSubPlatform = value
        End Set
    End Property

#End Region
    'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

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

        Dim udtDocTypeModelCollection As DocTypeModelCollection = (New DocTypeBLL).getAllDocType()

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'For Each udtDocTypeModel As DocTypeModel In udtDocTypeModelCollection
        For Each udtDocTypeModel As DocTypeModel In udtDocTypeModelCollection.FilterByHCSPSubPlatform(DocTypeLegendSubPlatform)
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            Dim dr As DataRow = dt.NewRow()
            dr.Item(0) = udtDocTypeModel.DocDisplayCode.Trim
            dr.Item(1) = udtDocTypeModel.DocName(strLang)
            dr.Item(3) = udtDocTypeModel.DocIdentityDesc(strLang)
            dr.Item(2) = udtDocTypeModel.DisplaySeq
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
            r.Cells(1).VerticalAlign = VerticalAlign.Top
            r.Cells(2).VerticalAlign = VerticalAlign.Top
        Next

    End Sub
End Class