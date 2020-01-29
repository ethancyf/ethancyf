Imports Common.Component.DocType
Imports Common.Component.Scheme

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

        dt.Columns.Add("DisplaySeq")
        dt.Columns.Add(Me.GetGlobalResourceObject("Text", "DocumentType"))

        'dt.Columns.Add(Me.GetGlobalResourceObject("Text", "Abbreviation"))
        'dt.Columns.Add(Me.GetGlobalResourceObject("Text", "IdentityDocNo"))

        Dim udtDocTypeModelCollection As DocTypeModelCollection = udtDocTypeBLL.getAllDocType()
        Dim udtDocTypeModelNew As DocTypeModelCollection = New DocTypeModelCollection
        Dim udtSchemeDocTypeList As SchemeDocTypeModelCollection = udtDocTypeBLL.getSchemeDocTypeByScheme(SchemeClaimModel.CIVSS)

        For Each udtDocTypeModel As DocTypeModel In udtDocTypeModelCollection
            'If udtDocTypeModel.DocCode.Trim.ToUpper() <> DocTypeModel.CertOfException Then
            '    udtDocTypeModelNew.Add(udtDocTypeModel)
            'End If

            For Each udtSchemeDocTypeModel As SchemeDocTypeModel In udtSchemeDocTypeList
                If udtDocTypeModel.DocCode.Trim.ToUpper() = udtSchemeDocTypeModel.DocCode.Trim.ToUpper() Then
                    udtDocTypeModelNew.Add(udtDocTypeModel)
                    Exit For
                End If
            Next
        Next

        Dim iSeq As Integer = 0

        For Each udtDocTypeModel As DocTypeModel In udtDocTypeModelNew
            Dim dr As DataRow = dt.NewRow()

            Select Case udtDocTypeModel.DocCode.Trim
                Case DocTypeModel.DocTypeCode.HKBC
                    iSeq = 1
                Case DocTypeModel.DocTypeCode.HKIC
                    iSeq = 2
                Case DocTypeModel.DocTypeCode.REPMT
                    iSeq = 3
                Case DocTypeModel.DocTypeCode.DI
                    iSeq = 4
                Case DocTypeModel.DocTypeCode.ID235B
                    iSeq = 5
                Case DocTypeModel.DocTypeCode.VISA
                    iSeq = 6
                Case DocTypeModel.DocTypeCode.ADOPC
                    iSeq = 7
            End Select

            dr.Item(0) = CStr(iSeq) & "."

            'dr.Item(0) = udtDocTypeModel.DocCode.Trim
            If strLang = English Then
                'dr.Item(1) = udtDocTypeModel.DocIdentityDesc
                dr.Item(1) = udtDocTypeModel.DocName.Trim
                'dr.Item(3) = udtDocTypeModel.DocIdentityDesc
            Else
                'dr.Item(1) = udtDocTypeModel.DocIdentityDescChi
                dr.Item(1) = udtDocTypeModel.DocNameChi.Trim
                'dr.Item(3) = udtDocTypeModel.DocIdentityDescChi
            End If
            'dr.Item(2) = udtDocTypeModel.DisplaySeq
            dt.Rows.Add(dr)

            'iSeq = iSeq + 1
        Next

        ' Perform sorting on Scheme Name column
        dt.DefaultView.Sort = "DisplaySeq Asc"

        'dt.Columns.RemoveAt(2)

        gvDocType.DataSource = dt
        gvDocType.DataBind()

        gvDocType.Rows(0).Cells(0).Width = 20
        'gvDocType.Rows(0).Cells(1).Width = 120
        'gvDocType.Rows(0).Cells(2).Width = 140
        'gvDocType.Rows(0).Cells(2).Width = 120
        For Each r As GridViewRow In gvDocType.Rows
            r.Cells(0).VerticalAlign = VerticalAlign.Top
            'r.Cells(0).VerticalAlign = VerticalAlign.Top
        Next

    End Sub
End Class