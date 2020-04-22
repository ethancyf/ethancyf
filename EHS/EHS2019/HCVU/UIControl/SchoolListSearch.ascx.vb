Imports Common.Component.Scheme
Imports Common.Component
Imports HCVU.BLL

Partial Public Class SchoolListSearch
    Inherits System.Web.UI.UserControl

#Region "Constants"
    Private Const SESS_SchoolList = "Grid_SchoolList"
    Private Const SESS_SelectedSchoolCode = "Grid_SelectedSchoolCode"
    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Const FunctCode As String = Common.Component.FunctCode.FUNT010418
    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

#End Region

#Region "Private Members"
    Private _strScheme As String
    Private _udtSessionHandler As New SessionHandlerBLL

#End Region

#Region "Add Event Handlers"
    'Events 
    'Public Event SchoolSelected(ByVal strSchoolCode As String, ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
    Public Event SchoolSelectedChanged(ByVal blnSelected As Boolean, ByVal sender As System.Object)

#End Region

#Region "Properties"

    ' CRE19-001 (VSS 2019) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Public Property Scheme() As String
        Get
            'Return Me._strScheme
            Return Me.ViewState("Scheme")
        End Get
        Set(ByVal value As String)
            'Me._strScheme = value
            Me.ViewState("Scheme") = value
        End Set
    End Property
    ' CRE19-001 (VSS 2019) [End][Winnie]

#End Region

#Region "Pages"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            Me.gvSelectedSchoolRecord.PageSize = 8
            Session(SESS_SelectedSchoolCode) = Nothing
            Me.gvSelectedSchoolRecord.SelectedIndex = -1
        End If

        Me.Page.MaintainScrollPositionOnPostBack = True

        Dim udtSelectedScheme As SchemeClaimModel = _udtSessionHandler.SelectSchemeGetFromSession(FunctCode)

        If Not udtSelectedScheme Is Nothing Then
            Me.Scheme = udtSelectedScheme.SchemeCode.Trim
        End If
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)

        lblSchoolListFilter.Text = GetGlobalResourceObject("Text", "SchoolSearch")

        ' Register the Post Back Argument Eg. Select$1, Select$2...
        For i As Integer = 0 To Me.gvSelectedSchoolRecord.PageSize - 1
            Me.Page.ClientScript.RegisterForEventValidation(Me.gvSelectedSchoolRecord.UniqueID, "Select$" + i.ToString())
        Next
        MyBase.Render(writer)
    End Sub

#End Region

#Region "Events"

    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub ibtnSchoolListFilter_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnSchoolListFilter.Click

        Dim udtSchoolListBLL As New Common.Component.School.SchoolBLL()

        'Dim udtSchoolListSearch As SchoolListSearch = CType(CType(sender, ImageButton).NamingContainer, SchoolListSearch)

        Dim dtSchoolList As DataTable = udtSchoolListBLL.SearchSchoolListByName(Me.txtSchoolListFilterCriteria.Text.Trim(), Me.Scheme)

        If dtSchoolList.Rows.Count = 0 Then
            Me.udcMsgBoxInfo.AddMessage("990000", "I", "00001")
            Me.udcMsgBoxInfo.BuildMessageBox()
        Else
            Me.udcMsgBoxInfo.Clear()
        End If

        Me.BindSchoolList(dtSchoolList)
        Me.ClearSelection(sender)

    End Sub
    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

    Private Sub gvSelectedSchoolRecord_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSelectedSchoolRecord.PageIndexChanging

        Dim gvSort As GridView = CType(sender, GridView)
        Dim gvFunction As Common.ComFunction.GridviewFunction = New Common.ComFunction.GridviewFunction()
        gvSort.DataSource = gvFunction.SortDataTable(Session(SESS_SchoolList), True)
        gvSort.PageIndex = e.NewPageIndex
        gvSort.DataBind()

        'Clear the Selected Value when page index changed
        Me.gvSelectedSchoolRecord.SelectedIndex = -1
        Session(SESS_SelectedSchoolCode) = Nothing
        RaiseEvent SchoolSelectedChanged(False, sender)

    End Sub

    Private Sub gvSelectedSchoolRecord_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSelectedSchoolRecord.RowCreated
        ' For Merge the Header Column
        If e.Row.RowType = DataControlRowType.Header Then

            Dim udtSelectedScheme As SchemeClaimModel = _udtSessionHandler.SelectSchemeGetFromSession(FunctCode)
            Dim strSelectedScheme As String = String.Empty

            If Not Me.Scheme Is Nothing Then
                strSelectedScheme = Me.Scheme
            ElseIf Not udtSelectedScheme Is Nothing Then
                strSelectedScheme = udtSelectedScheme.SchemeCode.Trim
            End If

            Dim gvHeader As GridView = CType(sender, GridView)
            Dim gvHeaderRow As GridViewRow = New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal)

            ' First Column
            Dim tcSchoolCode As TableCell = New TableCell()
            tcSchoolCode.Text = GetGlobalResourceObject("Text", "SchoolCode")
            tcSchoolCode.ColumnSpan = 1
            gvHeaderRow.Cells.Add(tcSchoolCode)

            Dim tcSchoolName As TableCell = New TableCell()
            tcSchoolName.Text = GetGlobalResourceObject("Text", "SchoolName")
            tcSchoolName.ColumnSpan = 2
            gvHeaderRow.Cells.Add(tcSchoolName)

            Dim tcAddress As TableCell = New TableCell()
            tcAddress.Text = HttpContext.GetGlobalResourceObject("Text", "Address")
            tcAddress.ColumnSpan = 2
            gvHeaderRow.Cells.Add(tcAddress)

            gvHeaderRow.CssClass = gvHeader.HeaderStyle.CssClass
            gvHeaderRow.ForeColor = gvHeader.HeaderStyle.ForeColor
            gvHeaderRow.BackColor = gvHeader.HeaderStyle.BackColor
            gvHeaderRow.Height = gvHeader.HeaderStyle.Height

            gvHeader.Controls(0).Controls.AddAt(0, gvHeaderRow)
        End If
    End Sub

    Private Sub gvSelectedSchoolRecord_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSelectedSchoolRecord.RowDataBound

        ' Add Post Back Script for Post back when click on a Grid Row
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("onclick", Me.Page.ClientScript.GetPostBackEventReference(Me.gvSelectedSchoolRecord, "Select$" + e.Row.RowIndex.ToString(), False))
            e.Row.Style.Add("cursor", "hand")
        End If
    End Sub

    Private Sub gvSelectedSchoolRecord_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSelectedSchoolRecord.SelectedIndexChanged

        If Not Me.gvSelectedSchoolRecord.SelectedRow Is Nothing Then
            Dim lblSchoolCode As Label = CType(Me.gvSelectedSchoolRecord.SelectedRow.FindControl("lblSchoolCode"), Label)
            If Not lblSchoolCode Is Nothing Then
                Session(SESS_SelectedSchoolCode) = lblSchoolCode.Text.Trim()
                RaiseEvent SchoolSelectedChanged(True, sender)
            Else
                Session(SESS_SelectedSchoolCode) = Nothing
                RaiseEvent SchoolSelectedChanged(False, sender)
            End If
        Else
            Session(SESS_SelectedSchoolCode) = Nothing
            RaiseEvent SchoolSelectedChanged(False, sender)
        End If
    End Sub

#End Region

#Region "Supporting Function"

    Public Function GetSelectedCode() As String

        If Session(SESS_SelectedSchoolCode) Is Nothing Then
            Return ""
        Else
            Return Session(SESS_SelectedSchoolCode).ToString().Trim()
        End If

    End Function

    Public Sub ClearFilter()
        Me.udcMsgBoxInfo.Clear()
        Me.txtSchoolListFilterCriteria.Text = ""

    End Sub

    Public Sub BindSchoolList(ByVal dtSource As DataTable)

        Session(SESS_SchoolList) = dtSource
        Me.gvSelectedSchoolRecord.PageIndex = 0
        Me.gvSelectedSchoolRecord.DataSource = dtSource
        Me.gvSelectedSchoolRecord.DataBind()

    End Sub

    Private Sub ClearSelection(ByVal sender As Object)
        'Clear the Selected Value when Search
        Me.gvSelectedSchoolRecord.SelectedIndex = -1
        Session(SESS_SelectedSchoolCode) = Nothing
        RaiseEvent SchoolSelectedChanged(False, sender)

    End Sub

#End Region

End Class