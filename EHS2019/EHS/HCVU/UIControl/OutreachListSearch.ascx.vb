Imports Common.Component.Scheme
Imports Common.Component
Imports HCVU.BLL

Partial Public Class OutreachListSearch
    Inherits System.Web.UI.UserControl

    Private Const Sess_OutreachList = "Grid_OutreachList"
    Private Const Sess_SelectedOutreachCode = "Grid_SelectedOutreachCode"

    Private _strScheme As String

    Private _udtSessionHandler As New SessionHandlerBLL

    Public Const FunctCode As String = Common.Component.FunctCode.FUNT010418

    'Private _enumClaimMode As ClaimMode

    'Events 
    Public Event Selected(ByVal strOutreachCode As String, ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)

    Public Event SelectedChanged(ByVal blnSelected As Boolean, ByVal sender As System.Object)

#Region "Property"
    Public Property Scheme() As String
        Get
            Return Me._strScheme
        End Get
        Set(ByVal value As String)
            Me._strScheme = value
        End Set
    End Property
#End Region

#Region "Property"
    'Public Property ClaimMode() As String
    '    Get
    '        Return Me._enumClaimMode
    '    End Get
    '    Set(ByVal value As String)
    '        Me._enumClaimMode = value
    '    End Set
    'End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            Me.gvSelectedOutreachRecord.PageSize = 8
            Session(Sess_SelectedOutreachCode) = Nothing
            Me.gvSelectedOutreachRecord.SelectedIndex = -1
        End If

        Me.Page.MaintainScrollPositionOnPostBack = True

        'Dim udtSelectedScheme As SchemeClaimModel = _udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
        Dim udtEHSTransaction As EHSTransaction.EHSTransactionModel = _udtSessionHandler.EHSTransactionWithoutTransactionDetailGetFromSession(FunctCode)

        If Not udtEHSTransaction Is Nothing Then
            Me.Scheme = udtEHSTransaction.SchemeCode.Trim
        End If

    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)

        lblOutreachListFilter.Text = GetGlobalResourceObject("Text", "OutreachNameFilter")

        ' Register the Post Back Argument Eg. Select$1, Select$2...
        For i As Integer = 0 To Me.gvSelectedOutreachRecord.PageSize - 1
            Me.Page.ClientScript.RegisterForEventValidation(Me.gvSelectedOutreachRecord.UniqueID, "Select$" + i.ToString())
        Next
        MyBase.Render(writer)
    End Sub

#Region "Event"

    Private Sub ibtnOutreachListFilter_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnOutreachListFilter.Click
        Dim udtOutreachListBLL As New Common.Component.COVID19.OutreachListBLL

        Dim udtOutreachListSearch As OutreachListSearch = CType(CType(sender, ImageButton).NamingContainer, OutreachListSearch)

        Dim strType As String = String.Empty

        'Select Case udtRVPHomeListSearch.Scheme
        '    Case SchemeClaimModel.EnumControlType.RVP.ToString.Trim
        '        strRCHType = String.Empty
        '    Case SchemeClaimModel.EnumControlType.VSS.ToString.Trim
        '        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
        '        ' ---------------------------------------------------------------------------------------------------------
        '        strRCHType = RCH_TYPE.IPID
        '        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]
        '    Case Else
        '        strRCHType = String.Empty
        'End Select

        Dim dtOutreachList As DataTable = udtOutreachListBLL.SearchOutreachList(Me.txtOutreachListFilterCriteria.Text.Trim(), strType)
        Dim dtFilterOutreachList As DataTable = Nothing

        'Select Case _enumClaimMode
        '    Case Common.Component.ClaimMode.COVID19
        '        Dim drRVP() As DataRow = dtOutreachList.Select("Type IN ('E','D')")

        '        If drRVP.Length > 0 Then
        '            dtFilterOutreachList = drRVP.CopyToDataTable
        '        End If
        '    Case Else
        '       dtFilterOutreachList = dtOutreachList
        'End Select
        dtFilterOutreachList = dtOutreachList

        If dtFilterOutreachList Is Nothing OrElse dtFilterOutreachList.Rows.Count = 0 Then
            Me.udcMsgBoxInfo.AddMessage("990000", "I", "00001")
            Me.udcMsgBoxInfo.BuildMessageBox()
        Else
            Me.udcMsgBoxInfo.Clear()
        End If

        Me.BindOutreachList(dtFilterOutreachList)

        Me.ClearSelection(sender)
    End Sub

    Private Sub gvSelectedOutreachRecord_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSelectedOutreachRecord.PageIndexChanging

        Dim gvSort As GridView = CType(sender, GridView)
        Dim gvFunction As Common.ComFunction.GridviewFunction = New Common.ComFunction.GridviewFunction()
        gvSort.DataSource = gvFunction.SortDataTable(Session(Sess_OutreachList), True)
        gvSort.PageIndex = e.NewPageIndex
        gvSort.DataBind()

        'Clear the Selected Value when page index changed
        Me.gvSelectedOutreachRecord.SelectedIndex = -1
        Session(Sess_SelectedOutreachCode) = Nothing
        RaiseEvent SelectedChanged(False, sender)

    End Sub

    Private Sub gvSelectedOutreachRecord_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvSelectedOutreachRecord.Sorting

    End Sub

    Private Sub gvSelectedOutreachRecord_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSelectedOutreachRecord.RowCommand
        'If TypeOf e.CommandSource Is LinkButton Then
        '    RaiseEvent OutreachSelected(e.CommandArgument.ToString().Trim(), sender, e)
        'End If
    End Sub

    Private Sub gvSelectedOutreachRecord_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSelectedOutreachRecord.RowCreated
        ' For Merge the Header Column
        If e.Row.RowType = DataControlRowType.Header Then
            Dim gvHeader As GridView = CType(sender, GridView)
            Dim gvHeaderRow As GridViewRow = New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal)

            ' First Column
            Dim tcOutreachCode As TableCell = New TableCell()

            tcOutreachCode.Text = GetGlobalResourceObject("Text", "OutreachCode")

            tcOutreachCode.ColumnSpan = 1
            gvHeaderRow.Cells.Add(tcOutreachCode)

            Dim tcRCName As TableCell = New TableCell()

            tcRCName.Text = GetGlobalResourceObject("Text", "OutreachName")

            tcRCName.ColumnSpan = 2
            gvHeaderRow.Cells.Add(tcRCName)

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

    Private Sub gvSelectedOutreachRecord_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSelectedOutreachRecord.RowDataBound

        ' Add Post Back Script for Post back when click on a Grid Row
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("onclick", Me.Page.ClientScript.GetPostBackEventReference(Me.gvSelectedOutreachRecord, "Select$" + e.Row.RowIndex.ToString(), False))
            e.Row.Style.Add("cursor", "hand")

            'Outreach Code
            Dim lblOutreachCode As Label = CType(e.Row.FindControl("lblOutreachCode"), Label)
            lblOutreachCode.Text = e.Row.DataItem("Outreach_Code")

            Dim lnkOutreachCode As LinkButton = CType(e.Row.FindControl("lnkOutreachCode"), LinkButton)
            lnkOutreachCode.Text = e.Row.DataItem("Outreach_Code")
            lnkOutreachCode.CommandArgument = e.Row.DataItem("Outreach_Code")

            'Outreach Name Eng
            Dim lblOutreachNameEng As Label = CType(e.Row.FindControl("lblOutreachNameEng"), Label)
            lblOutreachNameEng.Text = e.Row.DataItem("Outreach_Name_Eng")

            'Outreach Name Chi
            Dim lblOutreachNameChi As Label = CType(e.Row.FindControl("lblOutreachNameChi"), Label)
            lblOutreachNameChi.Text = e.Row.DataItem("Outreach_Name_Chi")

            'Outreach Address Eng
            Dim lblOutreachAddressEng As Label = CType(e.Row.FindControl("lblOutreachAddressEng"), Label)
            lblOutreachAddressEng.Text = e.Row.DataItem("Address_Eng")

            'Outreach Address Chi
            Dim lblOutreachAddressChi As Label = CType(e.Row.FindControl("lblOutreachAddressChi"), Label)
            lblOutreachAddressChi.Text = e.Row.DataItem("Address_Chi")

        End If
    End Sub

    Private Sub gvSelectedOutreachRecord_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSelectedOutreachRecord.SelectedIndexChanged

        If Not Me.gvSelectedOutreachRecord.SelectedRow Is Nothing Then
            Dim lblOutreachCode As Label = CType(Me.gvSelectedOutreachRecord.SelectedRow.FindControl("lblOutreachCode"), Label)
            If Not lblOutreachCode Is Nothing Then
                Session(Sess_SelectedOutreachCode) = lblOutreachCode.Text.Trim()
                RaiseEvent SelectedChanged(True, sender)
            Else
                Session(Sess_SelectedOutreachCode) = Nothing
                RaiseEvent SelectedChanged(False, sender)
            End If
        Else
            Session(Sess_SelectedOutreachCode) = Nothing
            RaiseEvent SelectedChanged(False, sender)
        End If
    End Sub

#End Region

#Region "Supporting Function"

    Public Function GetSelectedCode() As String

        If Session(Sess_SelectedOutreachCode) Is Nothing Then
            Return ""
        Else
            Return Session(Sess_SelectedOutreachCode).ToString().Trim()
        End If

    End Function

    Public Sub ClearFilter()
        Me.udcMsgBoxInfo.Clear()
        Me.txtOutreachListFilterCriteria.Text = ""
    End Sub

    Public Sub BindOutreachList(ByVal dtSource As DataTable)

        Session(Sess_OutreachList) = dtSource

        Me.gvSelectedOutreachRecord.PageIndex = 0
        Me.gvSelectedOutreachRecord.DataSource = dtSource
        Me.gvSelectedOutreachRecord.DataBind()

    End Sub

    Private Sub ClearSelection(ByVal sender As Object)
        'Clear the Selected Value when SeaOutreach
        Me.gvSelectedOutreachRecord.SelectedIndex = -1
        Session(Sess_SelectedOutreachCode) = Nothing
        RaiseEvent SelectedChanged(False, sender)
    End Sub
#End Region

End Class