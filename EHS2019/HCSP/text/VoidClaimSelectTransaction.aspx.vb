Imports Common.Component.ServiceProvider
Imports Common.Component.DataEntryUser
Imports Common.Component.ClaimTrans
Imports Common.Component.UserAC
Imports Common.Component.EHSTransaction
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Format
Imports Common.Validation
Imports HCSP.BLL
Imports Common.Component.Scheme
Imports Common.Component

Partial Public Class VoidClaimSelectTransaction
    Inherits TextOnlyBasePage

    Public Class AublitLogDescription
        Public Const SelectTransaction As String = "Select Transaction"
        Public Const SelectTransactionSuccess As String = "Complete Select Transaction"
        Public Const SelectTransactionFail As String = "Select Transaction Fail"
    End Class

    Private Const strImgArrowUp As String = "~/Images/others/arrowup.png"
    Private Const strImgArrowDown As String = "~/Images/others/arrowdown.png"
    Private Const strImgArrowBlank As String = "~/Images/others/arrowblank.png"

    'Dim udtClaimVoucherBLL As ClaimVoucherBLL
    'Dim transactionMaintenance As TransactionMaintenanceBLL
    'Dim udtVoidableClaimTrans As VoidableClaimTranModelCollection
    'Dim udfMessagBox As CustomControls.TextOnlyMessageBox
    Dim _udtFormatter As Formatter = New Formatter()
    Dim _udtSessionHandler As BLL.SessionHandler = New BLL.SessionHandler()
    Dim _strValidationFail As String = "ValidationFail"

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    ' -----------------------------------------------------------------------------------------

    Public Const FunctCode As String = Common.Component.FunctCode.FUNT020303

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Set no Cache
        Response.Cache.SetNoStore()
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetExpires(Now.AddDays(-1))

        'Initialize MasterPage
        CType(Me.Master.FindControl(ClaimVoucherMaster.ControlName.LblClaimVoucherStep), Label).Visible = False
        CType(Me.Master.FindControl(ClaimVoucherMaster.ControlName.lblTitle), Label).Text = Me.GetGlobalResourceObject("Text", "EVoucherSystem")

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------

        CType(Me.Master.FindControl(ClaimVoucherMaster.ControlName.lblSubTitle), Label).Text = Me.GetGlobalResourceObject("Text", "ClaimTransactionManagement")

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        Dim masterPage As ClaimVoucherMaster = CType(Me.Master, ClaimVoucherMaster)

        'Get Current USer Account for check Session Expired
        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC

        masterPage.BuildMenu(FunctCode, Me._udtSessionHandler.Language)

        AddHandler masterPage.MenuChanged, AddressOf MasterPage_MenuChanged

        Me.lblItemTextFormat.Text = String.Format("{0} \ {1}", Me.GetGlobalResourceObject("Text", "TransactionTime"), Me.GetGlobalResourceObject("Text", "TransactionNo"))

        'Initialize Voidable transaction
        'LoadTransaction()
    End Sub

    Protected Sub MasterPage_MenuChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me._udtSessionHandler.EHSTransactionSearchByNoRemoveFromSession(FunctCode)
        Me._udtSessionHandler.TextOnlyVersionSearchTypeRemoveFromSession()
        Me._udtSessionHandler.EHSClaimSessionRemove(FunctCode)
    End Sub

    Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)

        GridViewDataBind(gvTranList, LoadTransaction(), "Date", "ASC")
        MyBase.OnPreRender(e)

    End Sub

    Private Sub SetPageInfo(ByRef gvSort As GridView, ByVal dtTrans As DataTable)
        If gvSort.Rows.Count > 0 Then

            Dim lblPageInfo As New Label
            Dim dt As DataTable
            dt = CType(dtTrans, DataTable)
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

    Protected Function LoadTransaction() As DataTable
        Dim udtEHSTransactions As EHSTransactionModelCollection = Me._udtSessionHandler.EHSTransactionListGetFromSession(FunctCode)
        Dim strLanguage As String = _udtSessionHandler.Language()
        Dim strFormattedDate As String
        Dim strFormattedTranNo As String
        Dim listItem As ListItem
        Dim strSelectedValue As String = rbSelectTransaction.SelectedValue
        Dim blnSelectedValueExists As Boolean = False

        Dim tblTransNo As DataTable
        tblTransNo = New DataTable("Transaction")

        Dim strID As DataColumn = New DataColumn("ID")
        strID.DataType = System.Type.GetType("System.String")
        tblTransNo.Columns.Add(strID)

        Dim strDate As DataColumn = New DataColumn("Date")
        strDate.DataType = System.Type.GetType("System.String")
        tblTransNo.Columns.Add(strDate)

        Dim drTransNo As DataRow

        Me.rbSelectTransaction.Items.Clear()
        For Each udtEHSTransaction As EHSTransactionModel In udtEHSTransactions
            strFormattedDate = Me._udtFormatter.formatDateTime(udtEHSTransaction.TransactionDtm, strLanguage)
            strFormattedTranNo = Me._udtFormatter.formatSystemNumber(udtEHSTransaction.TransactionID.Trim())

            drTransNo = tblTransNo.NewRow
            drTransNo.Item("ID") = strFormattedTranNo
            drTransNo.Item("Date") = strFormattedDate

            tblTransNo.Rows.Add(drTransNo)

            listItem = New ListItem
            listItem.Value = strFormattedTranNo
            listItem.Text = String.Format("{0} \ {1}", strFormattedDate.Trim, strFormattedTranNo)
            Me.rbSelectTransaction.Items.Add(listItem)

            If strFormattedTranNo = strSelectedValue Then
                blnSelectedValueExists = True
            End If
        Next

        ' Retain selection after rebuild the list
        'If blnSelectedValueExists Then
        '    Me.rbSelectTransaction.SelectedValue = strSelectedValue
        'End If
        Return tblTransNo
    End Function

    'Protected Sub btnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelect.Click
    '    Dim udtEHSTransaction As EHSTransactionModel = Nothing
    '    Dim udtEHSTransactionBLL As EHSTransactionBLL = New EHSTransactionBLL
    '    Dim strFormattedTranNo As String = Me.rbSelectTransaction.SelectedValue

    '    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    '    ' -----------------------------------------------------------------------------------------

    '    Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(Common.Component.FunctCode.FUNT020303, Me)

    '    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

    '    Dim strDocType As String = String.Empty
    '    Dim strDocNum As String = String.Empty
    '    Dim udtFormatter As New Common.Format.Formatter

    '    AuditLogTransactionSelectionStart(udtAuditLogEntry)

    '    If Not String.IsNullOrEmpty(strFormattedTranNo) Then
    '        Me.lblSelectTransactionError.Visible = False


    '        udtEHSTransaction = udtEHSTransactionBLL.LoadClaimTran(Formatter.ReverseSystemNumber(strFormattedTranNo))
    '        Me._udtSessionHandler.EHSTransactionSaveToSession(udtEHSTransaction, FunctCode)
    '        Me._udtSessionHandler.EHSTransactionOrginalSaveToSession(udtEHSTransaction, FunctCode)
    '        Me._udtSessionHandler.EHSTransactionListRemoveFromSession(FunctCode)
    '        Me._udtSessionHandler.eHSAccDocTypeAndDocNumRemoveFromSession(FunctCode)

    '        AuditLogTransactionSelectionComplete(udtAuditLogEntry, udtEHSTransaction)


    '        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

    '        'set Page
    '        RedirectHandler.ToURL(ClaimVoucherMaster.ChildPage.ConfirmTransaction)

    '        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

    '    Else
    '        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    '        ' -----------------------------------------------------------------------------------------

    '        'Log Select Fail
    '        Me.udcMsgBoxErr.AddMessage(New SystemMessage("020303", "E", "00008"))

    '        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

    '        _udtSessionHandler.eHSAccDocTypeAndDocNumGetFromSession(strDocType, strDocNum, FunctCode)

    '        strDocNum = udtFormatter.formatDocumentIdentityNumber(strDocType, strDocNum)

    '    End If
    '    Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail, udtAuditLogEntry, Common.Component.LogID.LOG00009, "Transaction Selection Failed", New Common.ComObject.AuditLogInfo("", "", "", "", strDocType, strDocNum))
    'End Sub

    Protected Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me._udtSessionHandler.EHSClaimSessionRemove(FunctCode)

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        RedirectHandler.ToURL(ClaimVoucherMaster.ChildPage.SearchTransation)

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

    End Sub

#Region "Audit Log"

    'Select Transaction : LOG00007 : Start
    Public Sub AuditLogTransactionSelectionStart(ByRef udtAuditLogEntry As AuditLogEntry)
        Dim strDocType As String = String.Empty
        Dim strDocNum As String = String.Empty

        _udtSessionHandler.eHSAccDocTypeAndDocNumGetFromSession(strDocType, strDocNum, FunctCode)

        Dim udtFormatter As New Common.Format.Formatter

        strDocNum = udtFormatter.formatDocumentIdentityNumber(strDocType, strDocNum)

        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00007, "Transaction Selection start", New Common.ComObject.AuditLogInfo("", "", "", "", strDocType, strDocNum))
    End Sub

    'Select Transaction : LOG00008 : Complete
    Public Shared Sub AuditLogTransactionSelectionComplete(ByRef udtAuditLogEntry As AuditLogEntry, ByVal udtEHSTransaction As EHSTransactionModel)

        udtAuditLogEntry.AddDescripton("Transaction No", udtEHSTransaction.TransactionID)
        udtAuditLogEntry.AddDescripton("Transaction Date", udtEHSTransaction.TransactionDtm)
        udtAuditLogEntry.AddDescripton("Doc Code", udtEHSTransaction.DocCode)
        udtAuditLogEntry.AddDescripton("Create By", udtEHSTransaction.CreateBy)
        udtAuditLogEntry.AddDescripton("Update By", udtEHSTransaction.UpdateBy)
        udtAuditLogEntry.AddDescripton("Data Entry By", udtEHSTransaction.DataEntryBy)
        udtAuditLogEntry.AddDescripton("Service Date", udtEHSTransaction.ServiceDate)

        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Select Case New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(udtEHSTransaction.SchemeCode)
            Case SchemeClaimModel.EnumControlType.VOUCHER
                udtAuditLogEntry = EHSClaimBasePage.AuditLogHCVS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.VOUCHERCHINA
                'no text version

            Case SchemeClaimModel.EnumControlType.EVSS
                udtAuditLogEntry = EHSClaimBasePage.AuditLogEVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.CIVSS
                udtAuditLogEntry = EHSClaimBasePage.AuditLogCIVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.HSIVSS
                udtAuditLogEntry = EHSClaimBasePage.AuditLogHSIVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.RVP
                udtAuditLogEntry = EHSClaimBasePage.AuditLogRVP(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.PIDVSS
                udtAuditLogEntry = EHSClaimBasePage.AuditLogPIDVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.VSS
                udtAuditLogEntry = EHSClaimBasePage.AuditLogVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.ENHVSSO
                udtAuditLogEntry = EHSClaimBasePage.AuditLogENHVSSO(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.PPP
                'no text version

        End Select
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00008, "Transaction Selected")
    End Sub

    'Select Transaction : LOG00009 : failed

#End Region

#Region "Implement IWorkingData (CRE11-004)"

    Public Overrides Function GetDocCode() As String
        Dim udtEHSTransaction As EHSTransactionModel = Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)
        If Not IsNothing(udtEHSTransaction) Then
            Return udtEHSTransaction.DocCode
        Else
            Return Nothing
        End If
    End Function

    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Dim udtEHSTransaction As EHSTransactionModel = Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)
        If Not IsNothing(udtEHSTransaction) Then
            Return udtEHSTransaction.EHSAcct
        Else
            Return Nothing
        End If
    End Function

    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)
    End Function

    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        Return Nothing
    End Function

    Private Sub gvTranList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTranList.RowCommand

        ' Transaction No.
        Dim udtEHSTransaction As EHSTransactionModel = Nothing
        Dim udtEHSTransactionBLL As EHSTransactionBLL = New EHSTransactionBLL
        Dim strFormattedTranNo As String = String.Empty
        Dim strPageParameter As String = String.Empty
        Dim gvSort As GridView = CType(sender, GridView)

        If TypeOf e.CommandSource Is System.Web.UI.WebControls.Button Then
            strPageParameter = String.Format("{0}|{1}|{2}", gvTranList.PageIndex.ToString, ViewState("SortDirection_" & gvSort.ID).ToString, ViewState("SortExpression_" & gvSort.ID).ToString)
            Me._udtSessionHandler.EHSTransactionListPageSaveToSession(strPageParameter, FunctCode)
            strFormattedTranNo = e.CommandArgument
        End If

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------

        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(Common.Component.FunctCode.FUNT020303, Me)

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        Dim strDocType As String = String.Empty
        Dim strDocNum As String = String.Empty
        Dim udtFormatter As New Common.Format.Formatter

        AuditLogTransactionSelectionStart(udtAuditLogEntry)

        If Not String.IsNullOrEmpty(strFormattedTranNo) Then
            Me.lblSelectTransactionError.Visible = False


            udtEHSTransaction = udtEHSTransactionBLL.LoadClaimTran(Formatter.ReverseSystemNumber(strFormattedTranNo))
            Me._udtSessionHandler.EHSTransactionSaveToSession(udtEHSTransaction, FunctCode)
            Me._udtSessionHandler.EHSTransactionOrginalSaveToSession(udtEHSTransaction, FunctCode)
            'Me._udtSessionHandler.EHSTransactionListRemoveFromSession(FunctCode)
            Me._udtSessionHandler.eHSAccDocTypeAndDocNumRemoveFromSession(FunctCode)

            AuditLogTransactionSelectionComplete(udtAuditLogEntry, udtEHSTransaction)


            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

            'set Page
            RedirectHandler.ToURL(ClaimVoucherMaster.ChildPage.ConfirmTransaction)

            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

            'Else
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
            ' -----------------------------------------------------------------------------------------

            'Log Select Fail
            'Me.udcMsgBoxErr.AddMessage(New SystemMessage("020303", "E", "00008"))

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            '_udtSessionHandler.eHSAccDocTypeAndDocNumGetFromSession(strDocType, strDocNum, FunctCode)

            'strDocNum = udtFormatter.formatDocumentIdentityNumber(strDocType, strDocNum)

        End If
        Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail, udtAuditLogEntry, Common.Component.LogID.LOG00009, "Transaction Selection Failed", New Common.ComObject.AuditLogInfo("", "", "", "", strDocType, strDocNum))

    End Sub

    Private Sub gvTranList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTranList.PageIndexChanging
        gvTranList.PageIndex = e.NewPageIndex
    End Sub

    Private Sub gvTranList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvTranList.Sorting
        Me.GridViewSortingHandler(sender, e, Me.LoadTransaction)
    End Sub

    Private Sub gvTranList_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvTranList.PreRender
        Me.GridViewPreRenderHandler(sender, e, Me.LoadTransaction)
    End Sub

    Public Sub GridViewPreRenderHandler(ByVal sender As Object, ByVal e As System.EventArgs, ByVal dtTrans As DataTable)
        Dim gvSort As GridView = CType(sender, GridView)
        SetPageInfo(gvSort, dtTrans)
        SetSortImg(gvSort)
    End Sub

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

    Public Sub GridViewSortingHandler(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs, ByVal dt As DataTable)
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

        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctCode, Me)
        udtAuditLogEntry.AddDescripton("GridView", gvSort.ID)
        udtAuditLogEntry.AddDescripton("SortExpression", e.SortExpression)
        udtAuditLogEntry.AddDescripton("SortDirection", gvFunction.GridViewSortDirection)
        udtAuditLogEntry.WriteLog(LogID.LOG00009, "GridView Sorting")
        ' CRE11-021 log the missed essential information [End]

        Dim pageIndex As Integer = gvSort.PageIndex

        Dim dv As DataView = CType(gvFunction.SortDataTable(dt, False), DataView)
        ViewState("SortDirection_" & gvSort.ID) = gvFunction.GridViewSortDirection
        ViewState("SortExpression_" & gvSort.ID) = gvFunction.GridViewSortExpression

        gvSort.DataSource = dv
        gvSort.DataBind()
        gvSort.PageIndex = pageIndex

    End Sub

    Public Sub GridViewDataBind(ByRef gvSort As GridView, ByRef dt As Object, ByVal DefaultSortExpression As String, ByVal DefaultSortDirection As String)

        Dim strPageParameter As String = String.Empty
        strPageParameter = Me._udtSessionHandler.EHSTransactionListPageGetFromSession(FunctCode)
        If Not IsPostBack And strPageParameter IsNot Nothing Then
            Dim PageParameterList As String() = strPageParameter.Split("|")
            If PageParameterList(0) IsNot Nothing Then
                gvSort.PageIndex = CType(PageParameterList(0), Integer)
            End If
            If PageParameterList(1) IsNot Nothing Then
                ViewState("SortDirection_" & gvSort.ID) = PageParameterList(1)
            End If
            If PageParameterList(2) IsNot Nothing Then
                ViewState("SortExpression_" & gvSort.ID) = PageParameterList(2)
            End If
        End If

        Dim intPageIndex As Integer = 0
        intPageIndex = gvSort.PageIndex
        Dim gvFunction As Common.ComFunction.GridviewFunction = New Common.ComFunction.GridviewFunction(ViewState("SortDirection_" & gvSort.ID), ViewState("SortExpression_" & gvSort.ID))

        If ViewState("SortExpression_" & gvSort.ID) Is Nothing Then
            ViewState("SortExpression_" & gvSort.ID) = DefaultSortExpression
        End If

        If ViewState("SortDirection_" & gvSort.ID) Is Nothing Then
            ViewState("SortDirection_" & gvSort.ID) = DefaultSortDirection
        End If

        gvFunction.GridViewSortDirection = ViewState("SortDirection_" & gvSort.ID)
        gvFunction.GridViewSortExpression = ViewState("SortExpression_" & gvSort.ID)

        gvSort.RowStyle.BackColor = Drawing.Color.White
        gvSort.AlternatingRowStyle.BackColor = Drawing.Color.White

        gvSort.PageSize = (New GeneralFunction).GetPageSizeHCSP()
        gvSort.PageIndex = intPageIndex
        gvSort.DataSource = gvFunction.SortDataTable(dt, True)
        gvSort.DataBind()
    End Sub

#End Region


End Class