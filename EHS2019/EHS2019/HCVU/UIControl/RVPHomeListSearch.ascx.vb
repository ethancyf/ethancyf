Imports Common.Component.Scheme
Imports Common.Component
Imports HCVU.BLL

Partial Public Class RVPHomeListSearch
    Inherits System.Web.UI.UserControl

    Private Const Sess_RVPHomeList = "Grid_RVPHomeList"
    Private Const Sess_RVPSelectedCode = "Grid_RVPSelectedRCHCode"

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Const FunctCode As String = Common.Component.FunctCode.FUNT010418
    Private _strScheme As String

    Private _udtSessionHandler As New SessionHandlerBLL
    'CRE16-002 (Revamp VSS) [End][Chris YIM]

    'Events 
    Public Event RCHSelected(ByVal strRCHCode As String, ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)

    Public Event RCHSelectedChanged(ByVal blnSelected As Boolean, ByVal sender As System.Object)

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
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
    'CRE16-002 (Revamp VSS) [End][Chris YIM]

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            Me.gvSelectedRCHRecord.PageSize = 8
            Session(Sess_RVPSelectedCode) = Nothing
            Me.gvSelectedRCHRecord.SelectedIndex = -1
        End If

        Me.Page.MaintainScrollPositionOnPostBack = True

        'Dim udtSelectedScheme As SchemeClaimModel = _udtSessionHandler.SelectSchemeGetFromSession(FunctCode)
        Dim udtEHSTransaction As EHSTransaction.EHSTransactionModel = _udtSessionHandler.EHSTransactionWithoutTransactionDetailGetFromSession(FunctCode)

        If Not udtEHSTransaction Is Nothing Then
            Me.Scheme = udtEHSTransaction.SchemeCode.Trim
        End If
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Select Case Me.Scheme
            Case SchemeClaimModel.EnumControlType.RVP.ToString.Trim
                lblRCHListFilter.Text = GetGlobalResourceObject("Text", "RCHNameFilter")
            Case SchemeClaimModel.EnumControlType.VSS.ToString.Trim
                lblRCHListFilter.Text = GetGlobalResourceObject("Text", "PIDInstitutionNameFilter")
            Case Else
                lblRCHListFilter.Text = GetGlobalResourceObject("Text", "RCHNameFilter")
        End Select
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

        ' Register the Post Back Argument Eg. Select$1, Select$2...
        For i As Integer = 0 To Me.gvSelectedRCHRecord.PageSize - 1
            Me.Page.ClientScript.RegisterForEventValidation(Me.gvSelectedRCHRecord.UniqueID, "Select$" + i.ToString())
        Next
        MyBase.Render(writer)
    End Sub

#Region "Event"

    Private Sub ibtnRCHListFilter_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnRCHListFilter.Click
        Dim udtRVPHomeListBLL As New Common.Component.RVPHomeList.RVPHomeListBLL()

        Dim udtRVPHomeListSearch As RVPHomeListSearch = CType(CType(sender, ImageButton).NamingContainer, RVPHomeListSearch)

        Dim strRCHType As String

        Select Case udtRVPHomeListSearch.Scheme
            Case SchemeClaimModel.EnumControlType.RVP.ToString.Trim
                strRCHType = String.Empty
            Case SchemeClaimModel.EnumControlType.VSS.ToString.Trim
                ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                strRCHType = RCH_TYPE.IPID
                ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]
            Case Else
                strRCHType = String.Empty
        End Select

        Dim dtRVPHomeList As DataTable = udtRVPHomeListBLL.searchRVHHomeListByHomeName(Me.txtRCHListFilterCriteria.Text.Trim(), strRCHType)

        If dtRVPHomeList.Rows.Count = 0 Then
            Me.udcMsgBoxInfo.AddMessage("990000", "I", "00001")
            Me.udcMsgBoxInfo.BuildMessageBox()
        Else
            Me.udcMsgBoxInfo.Clear()
        End If

        Me.BindRVPHomeList(dtRVPHomeList)
        Me.ClearSelection(sender)
    End Sub

    Private Sub gvSelectedRCHRecord_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSelectedRCHRecord.PageIndexChanging

        Dim gvSort As GridView = CType(sender, GridView)
        Dim gvFunction As Common.ComFunction.GridviewFunction = New Common.ComFunction.GridviewFunction()
        gvSort.DataSource = gvFunction.SortDataTable(Session(Sess_RVPHomeList), True)
        gvSort.PageIndex = e.NewPageIndex
        gvSort.DataBind()

        'Clear the Selected Value when page index changed
        Me.gvSelectedRCHRecord.SelectedIndex = -1
        Session(Sess_RVPSelectedCode) = Nothing
        RaiseEvent RCHSelectedChanged(False, sender)

    End Sub

    Private Sub gvSelectedRCHRecord_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvSelectedRCHRecord.Sorting

    End Sub

    Private Sub gvSelectedRCHRecord_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSelectedRCHRecord.RowCommand
        'If TypeOf e.CommandSource Is LinkButton Then
        '    RaiseEvent RCHSelected(e.CommandArgument.ToString().Trim(), sender, e)
        'End If
    End Sub

    Private Sub gvSelectedRCHRecord_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSelectedRCHRecord.RowCreated
        ' For Merge the Header Column
        If e.Row.RowType = DataControlRowType.Header Then
            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim udtSelectedScheme As SchemeClaimModel = _udtSessionHandler.SelectSchemeGetFromSession(FunctCode)
            Dim strSelectedScheme As String = String.Empty

            If Not Me.Scheme Is Nothing Then
                strSelectedScheme = Me.Scheme
            ElseIf Not udtSelectedScheme Is Nothing Then
                strSelectedScheme = udtSelectedScheme.SchemeCode.Trim
            End If
            'CRE16-002 (Revamp VSS) [End][Chris YIM]


            Dim gvHeader As GridView = CType(sender, GridView)
            Dim gvHeaderRow As GridViewRow = New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal)

            ' First Column
            Dim tcRCHCode As TableCell = New TableCell()
            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Select Case strSelectedScheme
                Case SchemeClaimModel.EnumControlType.RVP.ToString.Trim
                    tcRCHCode.Text = GetGlobalResourceObject("Text", "RCHCode")
                Case SchemeClaimModel.EnumControlType.VSS.ToString.Trim
                    tcRCHCode.Text = GetGlobalResourceObject("Text", "PIDInstitutionCode")
                Case Else
                    'tcRCHCode.Text = GetGlobalResourceObject("Text", "RCHCode")
            End Select
            'CRE16-002 (Revamp VSS) [End][Chris YIM]
            tcRCHCode.ColumnSpan = 1
            gvHeaderRow.Cells.Add(tcRCHCode)

            Dim tcRCName As TableCell = New TableCell()
            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Select Case strSelectedScheme
                Case SchemeClaimModel.EnumControlType.RVP.ToString.Trim
                    tcRCName.Text = GetGlobalResourceObject("Text", "RCHName")
                Case SchemeClaimModel.EnumControlType.VSS.ToString.Trim
                    tcRCName.Text = GetGlobalResourceObject("Text", "PIDInstitutionName")
                Case Else
                    'tcRCName.Text = GetGlobalResourceObject("Text", "RCHName")
            End Select
            'CRE16-002 (Revamp VSS) [End][Chris YIM]
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

    Private Sub gvSelectedRCHRecord_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSelectedRCHRecord.RowDataBound

        ' Add Post Back Script for Post back when click on a Grid Row
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("onclick", Me.Page.ClientScript.GetPostBackEventReference(Me.gvSelectedRCHRecord, "Select$" + e.Row.RowIndex.ToString(), False))
            e.Row.Style.Add("cursor", "hand")
        End If
    End Sub

    Private Sub gvSelectedRCHRecord_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSelectedRCHRecord.SelectedIndexChanged

        If Not Me.gvSelectedRCHRecord.SelectedRow Is Nothing Then
            Dim lblRCHCode As Label = CType(Me.gvSelectedRCHRecord.SelectedRow.FindControl("lblRCHCode"), Label)
            If Not lblRCHCode Is Nothing Then
                Session(Sess_RVPSelectedCode) = lblRCHCode.Text.Trim()
                RaiseEvent RCHSelectedChanged(True, sender)
            Else
                Session(Sess_RVPSelectedCode) = Nothing
                RaiseEvent RCHSelectedChanged(False, sender)
            End If
        Else
            Session(Sess_RVPSelectedCode) = Nothing
            RaiseEvent RCHSelectedChanged(False, sender)
        End If
    End Sub

#End Region

#Region "Supporting Function"

    Public Function getSelectedCode() As String

        If Session(Sess_RVPSelectedCode) Is Nothing Then
            Return ""
        Else
            Return Session(Sess_RVPSelectedCode).ToString().Trim()
        End If

    End Function

    Public Sub ClearFilter()
        Me.udcMsgBoxInfo.Clear()
        Me.txtRCHListFilterCriteria.Text = ""
    End Sub

    Public Sub BindRVPHomeList(ByVal dtSource As DataTable)

        Session(Sess_RVPHomeList) = dtSource
        Me.gvSelectedRCHRecord.PageIndex = 0
        Me.gvSelectedRCHRecord.DataSource = dtSource
        Me.gvSelectedRCHRecord.DataBind()

    End Sub

    Private Sub ClearSelection(ByVal sender As Object)
        'Clear the Selected Value when Search
        Me.gvSelectedRCHRecord.SelectedIndex = -1
        Session(Sess_RVPSelectedCode) = Nothing
        RaiseEvent RCHSelectedChanged(False, sender)
    End Sub
#End Region

End Class