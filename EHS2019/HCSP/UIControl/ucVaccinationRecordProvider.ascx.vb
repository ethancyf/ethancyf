Imports Common.Component.DocType
Imports HCSP.BLL

Partial Public Class ucVaccinationRecordProvider
    Inherits System.Web.UI.UserControl

#Region "Private Class"

    Private Class SESS
        Public Const DocTypeList As String = "ucVaccinationRecordProvider_DocTypeList"
    End Class

#End Region

    ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Public Sub Build()
        Dim blnChinese As Boolean = LCase((New SessionHandler).Language) = "zh-tw"

        Dim dt As New DataTable
        dt.Columns.Add("Document_Code", GetType(String))
        dt.Columns.Add("Document_Name", GetType(String))

        ' Document Accept
        Dim udtDocTypeBLL As New DocTypeBLL
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Dim udtDocTypeList As DocTypeModelCollection = udtDocTypeBLL.getAllDocType().FilterForVaccinationRecordEnquriySearch
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        ' Save the Document Type List to session for row data bind
        Session(SESS.DocTypeList) = udtDocTypeList

        For Each udtDocType As DocTypeModel In udtDocTypeList
            Dim dr As DataRow = dt.NewRow
            dr("Document_Code") = udtDocType.DocCode
            dr("Document_Name") = IIf(blnChinese, udtDocType.DocNameChi, udtDocType.DocName)
            dt.Rows.Add(dr)
        Next

        ' Provider
        gvDocument.Columns(0).HeaderText = Me.GetGlobalResourceObject("Text", "DocumentType")
        gvDocument.Columns(1).HeaderText = Me.GetGlobalResourceObject("Text", "eHealthSystem")
        gvDocument.Columns(2).HeaderText = Me.GetGlobalResourceObject("Text", "HospitalAuthority")
        gvDocument.Columns(3).HeaderText = Me.GetGlobalResourceObject("Text", "DepartmentOfHealth")

        gvDocument.DataSource = dt
        gvDocument.DataBind()

        ' Add the extra header row at the beginning
        Dim gvHeaderRow As New GridViewRow(-1, -1, DataControlRowType.Header, DataControlRowState.Normal)

        Dim tblCell As TableCell = Nothing

        ' Empty cell for first column
        tblCell = New TableCell
        tblCell.BackColor = Drawing.Color.White
        tblCell.BorderStyle = BorderStyle.None

        gvHeaderRow.Cells.Add(tblCell)

        ' Cell: Provider
        tblCell = New TableCell
        tblCell.Text = Me.GetGlobalResourceObject("Text", "InformationProvider")
        tblCell.ColumnSpan = 3
        tblCell.BorderStyle = BorderStyle.Solid
        tblCell.BorderColor = Drawing.Color.Black
        tblCell.Style.Add("border-width", "1px 1px 0px 1px")
        tblCell.Height = IIf(blnChinese, Unit.Percentage(100), Unit.Pixel(34))
        tblCell.VerticalAlign = VerticalAlign.Middle

        gvHeaderRow.Cells.Add(tblCell)

        gvDocument.Controls(0).Controls.AddAt(0, gvHeaderRow)

        ' Hide the gridview border (so as to hide the border at the top-left cell)
        gvDocument.BorderStyle = BorderStyle.None
        gvDocument.Style.Add("border-collapse", "separate")

    End Sub
    ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

    Protected Sub gvDocument_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Then
            ' eHealth System
            Dim imgProvider1 As Image = e.Row.FindControl("imgProvider1")
            imgProvider1.Visible = True

            ' Hospital Authority
            Dim hfDocumentCode As HiddenField = e.Row.FindControl("hfDocumentCode")

            Dim udtDocTypeList As DocTypeModelCollection = Session(SESS.DocTypeList)

            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Dim udtDocTypeBLL As New DocTypeBLL
            If udtDocTypeBLL.CheckVaccinationRecordAvailable(hfDocumentCode.Value.Trim, "HA") Then
                Dim imgProvider2 As Image = e.Row.FindControl("imgProvider2")
                imgProvider2.Visible = True
            End If

            If udtDocTypeBLL.CheckVaccinationRecordAvailable(hfDocumentCode.Value.Trim, "DH") Then
                Dim imgProvider3 As Image = e.Row.FindControl("imgProvider3")
                imgProvider3.Visible = True
            End If
            ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        End If

    End Sub

End Class