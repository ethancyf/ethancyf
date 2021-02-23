Imports System.Data
Imports Common.Component.SortedGridviewHeader
Imports Common.ComFunction
Imports Common.Component
Imports Common.ComObject

Public MustInherit Class BasePageWithGridView
    Inherits BasePage

    Private Const strImgArrowUp As String = "~/Images/others/arrowup.png"
    Private Const strImgArrowDown As String = "~/Images/others/arrowdown.png"
    Private Const strImgArrowBlank As String = "~/Images/others/arrowblank.png"

    Public Sub GridViewSortingHandler(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs, ByVal strDataSource As String)
        Dim gvSort As GridView = CType(sender, GridView)
        Dim gvFunction As Common.ComFunction.GridviewFunction = New Common.ComFunction.GridviewFunction(ViewState("SortDirection_" & gvSort.ID), ViewState("SortExpression_" & gvSort.ID))

        gvFunction.GridViewSortExpression = e.SortExpression
        If ViewState("SortExpression_" & gvSort.ID) <> e.SortExpression Then
            If ViewState("SortDirection_" & gvSort.ID) = "ASC" Then
                gvFunction.GridViewSortDirection = "DESC"
            Else
                gvFunction.GridViewSortDirection = "ASC"
            End If
        End If

        If MyBase.CommonFunctCode IsNot Nothing Then
            Dim udtAuditLogEntry As New AuditLogEntry(MyBase.CommonFunctCode, Me)
            udtAuditLogEntry.AddDescripton("GridView", gvSort.ID)
            udtAuditLogEntry.AddDescripton("SortExpression", e.SortExpression)
            udtAuditLogEntry.AddDescripton("SortDirection", gvFunction.GridViewSortDirection)
            udtAuditLogEntry.WriteLog(LogID.LOG00009, "GridView Sorting")
        End If

        Dim pageIndex As Integer = gvSort.PageIndex

        Dim dv As DataView = CType(gvFunction.SortDataTable(Session(strDataSource), False), DataView)
        ViewState("SortDirection_" & gvSort.ID) = gvFunction.GridViewSortDirection
        ViewState("SortExpression_" & gvSort.ID) = gvFunction.GridViewSortExpression

        gvSort.DataSource = dv
        gvSort.DataBind()
        gvSort.PageIndex = pageIndex

    End Sub

    Public Sub GridViewPageIndexChangingHandler(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs, ByVal strDataSource As String)
        Dim gvSort As GridView = CType(sender, GridView)
        Dim gvFunction As Common.ComFunction.GridviewFunction = New Common.ComFunction.GridviewFunction(ViewState("SortDirection_" & gvSort.ID), ViewState("SortExpression_" & gvSort.ID))
        ViewState("SortDirection_" & gvSort.ID) = gvFunction.GridViewSortDirection
        ViewState("SortExpression_" & gvSort.ID) = gvFunction.GridViewSortExpression

        gvSort.DataSource = gvFunction.SortDataTable(Session(strDataSource), True)
        gvSort.PageIndex = e.NewPageIndex
        gvSort.DataBind()
    End Sub

    Public Sub GridViewDataBind(ByRef gvSort As GridView, ByRef dt As Object, ByVal strSortExpression As String, ByVal strSortDirection As String, ByVal blnKeepPageIndex As Boolean)
        Dim intPageIndex As Integer = 0
        If blnKeepPageIndex = False Then
            'intPageIndex = gvSort.PageIndex
            intPageIndex = 0
        End If

        ViewState("SortDirection_" & gvSort.ID) = strSortDirection
        ViewState("SortExpression_" & gvSort.ID) = strSortExpression

        gvSort.PageSize = (New GeneralFunction).GetPageSizeHCSP()
        gvSort.PageIndex = intPageIndex
        gvSort.DataSource = dt
        gvSort.DataBind()

    End Sub

    Public Sub GridViewDataBind(ByRef gvSort As GridView, ByRef dt As Object, _
                                ByVal strSortExpression As String, ByVal strSortDirection As String, _
                                ByVal blnKeepPageIndex As Boolean, ByVal intPageSize As Integer)

        Dim intPageIndex As Integer = 0
        If blnKeepPageIndex = False Then
            'intPageIndex = gvSort.PageIndex
            intPageIndex = 0
        End If

        ViewState("SortDirection_" & gvSort.ID) = strSortDirection
        ViewState("SortExpression_" & gvSort.ID) = strSortExpression

        gvSort.PageSize = intPageSize
        gvSort.PageIndex = intPageIndex
        gvSort.DataSource = dt
        gvSort.DataBind()

    End Sub

    Public Sub GridViewDataBind(ByRef gvSort As GridView, ByRef dt As Object)
        Dim intPageIndex As Integer = 0
        intPageIndex = gvSort.PageIndex
        Dim gvFunction As Common.ComFunction.GridviewFunction = New Common.ComFunction.GridviewFunction(ViewState("SortDirection_" & gvSort.ID), ViewState("SortExpression_" & gvSort.ID))

        gvFunction.GridViewSortDirection = ViewState("SortDirection_" & gvSort.ID)
        gvFunction.GridViewSortExpression = ViewState("SortExpression_" & gvSort.ID)

        gvSort.PageSize = (New GeneralFunction).GetPageSizeHCSP()
        gvSort.PageIndex = intPageIndex
        gvSort.DataSource = gvFunction.SortDataTable(dt, True)
        gvSort.DataBind()
    End Sub

    Public Sub GridViewDataBind(ByRef gvSort As GridView, ByRef dt As Object, ByVal intPageSize As Integer)
        Dim intPageIndex As Integer = 0
        intPageIndex = gvSort.PageIndex
        Dim gvFunction As Common.ComFunction.GridviewFunction = New Common.ComFunction.GridviewFunction(ViewState("SortDirection_" & gvSort.ID), ViewState("SortExpression_" & gvSort.ID))

        gvFunction.GridViewSortDirection = ViewState("SortDirection_" & gvSort.ID)
        gvFunction.GridViewSortExpression = ViewState("SortExpression_" & gvSort.ID)

        gvSort.PageSize = intPageSize
        gvSort.PageIndex = intPageIndex
        gvSort.DataSource = gvFunction.SortDataTable(dt, True)
        gvSort.DataBind()
    End Sub

    Public Sub GridViewPreRenderHandler(ByVal sender As Object, ByVal e As System.EventArgs, ByVal strDataSource As String)
        Dim gvSort As GridView = CType(sender, GridView)
        SetSortImg(gvSort)
        'CRE20-0022 (Immu record) [Start][martin]
        If gvSort.AllowPaging = True Then
            SetPageInfo(gvSort, strDataSource)
        End If
        'CRE20-0022 (Immu record) [End][martin]
    End Sub

    Public Sub GridViewCustomPreRenderHandler(ByVal sender As Object, ByVal e As System.EventArgs, ByVal strDataSource As String, ByRef lstTblCell As List(Of TableCell))
        Dim gvSort As GridView = CType(sender, GridView)
        SetSortImg(gvSort, lstTblCell)
        SetPageInfo(gvSort, strDataSource)
    End Sub

    Public Sub GirdViewRowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs, Optional ByVal udtSortedGridviewHeaderList As SortedGridviewHeaderModelCollection = Nothing)
        If e.Row.RowType = DataControlRowType.Header Then
            Dim i As Integer = 0

            For Each cell As TableCell In e.Row.Cells
                Dim udtSortedGridviewHeader As SortedGridviewHeaderModel = Nothing

                If Not IsNothing(udtSortedGridviewHeaderList) Then
                    udtSortedGridviewHeader = udtSortedGridviewHeaderList.Filter(i)
                End If

                If Not IsNothing(udtSortedGridviewHeader) Then
                    udtSortedGridviewHeader.imgHeaderImage.ID = "ibtnGridviewHeader_" + i.ToString.PadLeft(2, "0").Trim()
                    udtSortedGridviewHeader.imgHeaderImage.ImageAlign = ImageAlign.Middle
                    AddHandler udtSortedGridviewHeader._HeaderImageClick, AddressOf Me.GridViewHeaderImage_Click

                    If Not cell.Text.Equals(String.Empty) Then
                        Dim lblHeader As New Label
                        lblHeader.Text = cell.Text
                        cell.Text = String.Empty

                        cell.Controls.Add(lblHeader)
                        'cell.Controls.Add(lblHeaderBR)
                        'cell.Controls.Add(imgHeader)
                    End If

                    cell.Controls.Add(udtSortedGridviewHeader.imgHeaderImage)

                End If
                i = i + 1
            Next
        End If
    End Sub

    Public Function GetGridViewSortExpression(ByRef gvSort As GridView) As String
        Return ViewState("SortExpression_" & gvSort.ID)
    End Function

    Public Function GetGridViewSortDirection(ByRef gvSort As GridView) As String
        Return ViewState("SortDirection_" & gvSort.ID)
    End Function

    Private Sub SetSortImg(ByRef gvSort As GridView)
        If gvSort.Rows.Count > 0 Then

            Dim gvrHeaderRow As GridViewRow
            gvrHeaderRow = gvSort.HeaderRow

            Dim cell As TableCell
            For Each cell In gvrHeaderRow.Cells
                If cell.HasControls Then
                    If TypeOf cell.Controls(0) Is LinkButton Then

                        Dim lbtnHeader As LinkButton = CType(cell.Controls(0), LinkButton)

                        If Not lbtnHeader Is Nothing Then

                            Dim imgHeader As New Image
                            Dim lblHeader As New Label
                            lblHeader.Text = "<br>"
                            imgHeader.ImageUrl = strImgArrowBlank

                            If ViewState("SortExpression_" & gvSort.ID) = lbtnHeader.CommandArgument Then
                                If ViewState("SortDirection_" & gvSort.ID) = "ASC" Then
                                    imgHeader.ImageUrl = strImgArrowUp
                                ElseIf ViewState("SortDirection_" & gvSort.ID) = "DESC" Then
                                    imgHeader.ImageUrl = strImgArrowDown
                                End If
                            End If

                            cell.Controls.Add(lblHeader)
                            cell.Controls.Add(imgHeader)
                        End If
                    Else
                        If cell.Controls.Count > 1 Then
                            If TypeOf cell.Controls(1) Is CheckBox Then

                                Dim imgHeader As New Image
                                imgHeader.ImageUrl = strImgArrowBlank
                                cell.Controls.Add(imgHeader)

                            End If
                            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                            '-----------------------------------------------------------------------------------------
                            If TypeOf cell.Controls(1) Is Image Then

                                Dim lblHeaderBR As New Label
                                lblHeaderBR.Text = "<br>"

                                Dim imgHeader As New Image
                                imgHeader.ImageUrl = strImgArrowBlank
                                cell.Controls.Add(lblHeaderBR)
                                cell.Controls.Add(imgHeader)

                            End If
                            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
                        End If
                    End If
                Else
                    If Not cell.Text.Trim.Equals(String.Empty) Then
                        Dim lblHeader As New Label
                        lblHeader.Text = cell.Text

                        Dim lblHeaderBR As New Label
                        lblHeaderBR.Text = "<br>"

                        Dim imgHeader As New Image
                        imgHeader.ImageUrl = strImgArrowBlank

                        cell.Controls.Add(lblHeader)
                        cell.Controls.Add(lblHeaderBR)
                        cell.Controls.Add(imgHeader)
                    End If
                End If

            Next
        End If
    End Sub

    Private Sub SetSortImg(ByVal gvSort As GridView, ByRef lstTblCell As List(Of TableCell))
        If gvSort.Rows.Count > 0 Then

            For Each cell As TableCell In lstTblCell
                If cell.HasControls Then
                    If TypeOf cell.Controls(0) Is LinkButton Then

                        Dim lbtnHeader As LinkButton = CType(cell.Controls(0), LinkButton)

                        If Not lbtnHeader Is Nothing Then

                            Dim imgHeader As New Image
                            Dim lblHeader As New Label
                            lblHeader.Text = "<br>"
                            imgHeader.ImageUrl = strImgArrowBlank

                            If ViewState("SortExpression_" & gvSort.ID) = lbtnHeader.CommandArgument Then
                                If ViewState("SortDirection_" & gvSort.ID) = "ASC" Then
                                    imgHeader.ImageUrl = strImgArrowUp
                                ElseIf ViewState("SortDirection_" & gvSort.ID) = "DESC" Then
                                    imgHeader.ImageUrl = strImgArrowDown
                                End If
                            End If

                            cell.Controls.Add(lblHeader)
                            cell.Controls.Add(imgHeader)
                        End If
                    Else
                        If cell.Controls.Count > 1 Then
                            If TypeOf cell.Controls(1) Is CheckBox Then

                                Dim imgHeader As New Image
                                imgHeader.ImageUrl = strImgArrowBlank
                                cell.Controls.Add(imgHeader)

                            End If
                            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                            '-----------------------------------------------------------------------------------------
                            If TypeOf cell.Controls(1) Is Image Then

                                Dim lblHeaderBR As New Label
                                lblHeaderBR.Text = "<br>"

                                Dim imgHeader As New Image
                                imgHeader.ImageUrl = strImgArrowBlank
                                cell.Controls.Add(lblHeaderBR)
                                cell.Controls.Add(imgHeader)

                            End If
                            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
                        End If
                    End If
                Else
                    If Not cell.Text.Trim.Equals(String.Empty) Then
                        Dim lblHeader As New Label
                        lblHeader.Text = cell.Text

                        Dim lblHeaderBR As New Label
                        lblHeaderBR.Text = "<br>"

                        Dim imgHeader As New Image
                        imgHeader.ImageUrl = strImgArrowBlank

                        cell.Controls.Add(lblHeader)
                        cell.Controls.Add(lblHeaderBR)
                        cell.Controls.Add(imgHeader)
                    End If
                End If

            Next
        End If
    End Sub

    Private Sub SetPageInfo(ByRef gvSort As GridView, ByVal strDataSource As String)
        If gvSort.Rows.Count > 0 Then

            Dim lblPageInfo As New Label
            Dim dt As DataTable
            dt = CType(Session(strDataSource), DataTable)
            Dim intPageIndex As Integer
            intPageIndex = gvSort.PageIndex + 1

            Dim strPageInfo As String

            strPageInfo = Me.GetGlobalResourceObject("Text", "GridPageInfo")

            strPageInfo = strPageInfo.Replace("%d", CStr(intPageIndex))
            strPageInfo = strPageInfo.Replace("%e", CStr(gvSort.PageCount))
            strPageInfo = strPageInfo.Replace("%f", CStr(dt.Rows.Count))
            lblPageInfo.Text = strPageInfo

            Dim grv As GridViewRow = gvSort.BottomPagerRow
            grv.Visible = True
            Dim i As Integer
            i = grv.Cells(0).Controls.Count - 1

            Dim tc As TableCell
            Dim tr As TableRow
            tr = CType(grv.Cells(0).Controls(0).Controls(0), TableRow)

            If gvSort.PageCount = 1 Then
                tc = tr.Cells(0)

                Dim lblPage As Label
                lblPage = CType(tc.Controls(0), Label)
                lblPage.Visible = False

                tc.Controls.Add(lblPageInfo)
                tr.Cells.Add(tc)

            Else
                tc = New TableCell
                tc.Width = Unit.Pixel(20)
                tr.Cells.Add(tc)

                tc = New TableCell
                tc.Controls.Add(lblPageInfo)
                tr.Cells.Add(tc)
            End If
        End If
    End Sub

    Public Overridable Sub GridViewHeaderImage_Click(ByVal sender As Object, ByVal e As SortedGridviewHeaderModel.GridViewHeaderImageEventArgs)
        'e.intColumn
    End Sub

End Class
