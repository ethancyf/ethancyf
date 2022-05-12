Imports System.Web.Security.AntiXss
Imports Common.Component
Imports Common.Component.HCVUUser
Imports Common.ComObject
Imports Common.DataAccess
Imports Common.Format
Imports Common.Validation
Imports Common.Component.Status
Imports Common.Component.StaticData
Imports HCVU.BLL
Imports Common.Component.VaccineLot
Imports Common.ComFunction
Imports Common.Component.VaccineLot.VaccineLotBLL
Imports Common.Component.COVID19

Partial Public Class VaccineLotManagement
    Inherits BasePageWithControl


    Public Enum StateType
        LOADED = 0
        EDIT = 1
        ADD = 2
        BatchRemove = 3
        Remove = 4
    End Enum

    Public Enum ViewIndex
        SearchSummary = 0
        Confirm = 1
        MsgPage = 2
        NewEditRecord = 3
        EditRecordView = 4
    End Enum


#Region "Fields"

    Private udtHCVUUserBLL As New HCVUUserBLL
    Private udtVaccineLotBLL As New VaccineLotBLL
    Private udtCOVID19BLL As New COVID19BLL

    Private udtFormatter As New Formatter
    Private udtValidator As New Validator
    'Private udtGeneralFunction As New GeneralFunction

    'Private SM As SystemMessage
    Private udtAuditLogEntry As AuditLogEntry

    Private Const SESS_SearchResultList As String = FunctCode.FUNT010422 & "VaccineLotList_SearchResultList"
    Private Const SESS_VaccineLotSummary As String = FunctCode.FUNT010422 & "VaccineLotList_Summary"
    Private Const SESS_BatchRemoveBrandLotList As String = FunctCode.FUNT010422 & "VaccineLotList_BatchRemoveBrandLotList"
    Private Const SESS_Confirm_HistoryView As String = FunctCode.FUNT010422 & "VaccineLotList_HistoryView"
    Private Const BoothClassCode As String = "VaccineCentreBooth"
#End Region

#Region "Audit Log Message"
    Private Const AuditMsg00000 As String = "Vaccine Lot Management loaded"
    Private Const AuditMsg00001 As String = "Centre Dropdown list is changed"
    Private Const AuditMsg00002 As String = "Search Start"
    Private Const AuditMsg00003 As String = "Search completed"
    Private Const AuditMsg00004 As String = "Search failed"
    Private Const AuditMsg00005 As String = "Batch Assign Click"
    Private Const AuditMsg00006 As String = "Batch Assign - Cancel Click"
    Private Const AuditMsg00007 As String = "Batch Assign - Save Click"
    Private Const AuditMsg00008 As String = "Batch Assign - Save Click - Completed"
    Private Const AuditMsg00009 As String = "Batch Assign - Save Click - Failed"
    Private Const AuditMsg00010 As String = "Batch Assign - Back Click"
    Private Const AuditMsg00011 As String = "Batch Assign - Confirm Click"
    Private Const AuditMsg00012 As String = "Batch Assign - Confirm Click - Completed"
    Private Const AuditMsg00013 As String = "Batch Assign - Confirm Click - Failed"
    Private Const AuditMsg00014 As String = "Batch Remove Click"
    Private Const AuditMsg00015 As String = "Batch Remove - Cancel Click"
    Private Const AuditMsg00016 As String = "Batch Remove - Save Click"
    Private Const AuditMsg00017 As String = "Batch Remove - Save Click - Completed"
    Private Const AuditMsg00018 As String = "Batch Remove - Save Click - Failed"
    Private Const AuditMsg00019 As String = "Batch Remove - Back Click"
    Private Const AuditMsg00020 As String = "Batch Remove - Confirm Click"
    Private Const AuditMsg00021 As String = "Batch Remove - Confirm Click - Completed"
    Private Const AuditMsg00022 As String = "Batch Remove - Confirm Click - Failed"
    Private Const AuditMsg00023 As String = "View Vaccine Lot Record Click"
    Private Const AuditMsg00024 As String = "View Vaccine Lot Record - Back Click"
    Private Const AuditMsg00025 As String = "View Vaccine Lot Record - Edit Click"
    Private Const AuditMsg00026 As String = "Edit - Cancel Click"
    Private Const AuditMsg00027 As String = "Edit - Save Click"
    Private Const AuditMsg00028 As String = "Edit - Save Click - Completed"
    Private Const AuditMsg00029 As String = "Edit - Save Click - Fail"
    Private Const AuditMsg00030 As String = "Edit - Back Click"
    Private Const AuditMsg00031 As String = "Edit - Confirm Click"
    Private Const AuditMsg00032 As String = "Edit - Confirm Click - Completed"
    Private Const AuditMsg00033 As String = "Edit - Confirm Click - Failed"
    Private Const AuditMsg00034 As String = "Remove Click - Yes"
    Private Const AuditMsg00035 As String = "Remove Click - No"
    Private Const AuditMsg00036 As String = "Remove - Completed"
    Private Const AuditMsg00037 As String = "Remove - Failed"
    Private Const AuditMsg00038 As String = "Validation failed"
    Private Const AuditMsg00039 As String = "Msg Page - Return"
    Private Const AuditMsg00040 As String = "View Vaccine Lot Record - Remove Click"

#End Region



#Region "SF Search"
    Protected Overrides Sub SF_InitSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry)
    End Sub

    Protected Overrides Function SF_ValidateSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry) As Boolean
        If Not SearchValidation() Then
            Return False
        Else
            Return True
        End If
    End Function

    Protected Overrides Function SF_Search(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry, ByVal blnOverrideResultLimit As Boolean) As Common.Component.BaseBLL.BLLSearchResult

    End Function

    Protected Overrides Function SF_BindSearchResult(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry, ByVal udtBLLSearchResult As Common.Component.BaseBLL.BLLSearchResult) As Integer

    End Function

    Protected Overrides Sub SF_FinalizeSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry)
    End Sub

    Protected Overrides Sub SF_ConfirmSearch_Click()
    End Sub

    Protected Overrides Sub SF_CancelSearch_Click()
    End Sub

#End Region

#Region "Page Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        If Not Page.IsPostBack Then
            FunctionCode = FunctCode.FUNT010422

            ' Write Audit Log
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, AuditMsg00000)
            BindCentre(ddlVaccCentre)

            If ddlVaccCentre.Items.Count > 1 OrElse ddlVaccCentre.Items.Count = 0 Then
                btnBatchAssign.Enabled = False
                btnBatchRemove.Enabled = False

                btnBatchAssign.ImageUrl = GetGlobalResourceObject("ImageUrl", "BatchAssignDisableBtn")
                btnBatchRemove.ImageUrl = GetGlobalResourceObject("ImageUrl", "BatchRemoveDisableBtn")
            Else
                GetVaccineLotSummary()
            End If

            ResetAllHiddenField()
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

    End Sub

#End Region

    Private Sub GetVaccineLotSummary()
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Centre Name", ddlVaccCentre.SelectedItem.ToString)
        udtAuditLogEntry.AddDescripton("Centre Id", ddlVaccCentre.SelectedValue.ToString)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00002, AuditMsg00002)
        Try
            Dim VaccineLotSummaryDataTable As DataTable = udtVaccineLotBLL.GetVaccineLotListSummaryByAny(ddlVaccCentre.SelectedValue, String.Empty, String.Empty, String.Empty)

            Session(SESS_VaccineLotSummary) = VaccineLotSummaryDataTable

            GridViewDataBind(gvSummary, VaccineLotSummaryDataTable, "Booth_Order", "ASC", False)

            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditMsg00003)
        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception")
            udtAuditLogEntry.AddDescripton("Message", ex.Message)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00004, AuditMsg00004)
            Throw
        End Try

    End Sub


    'Protected Sub ibtnSearchResultBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
    '    udtAuditLogEntry.WriteLog(LogID.LOG00008, AuditMsg00005)

    '    'BackToSearchCriteriaView(True)
    '    udcInfoBox.Visible = False
    '    ChangeViewIndex(ViewIndex.SearchCriteria)
    'End Sub

    'Protected Sub ibtnBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
    '    udtAuditLogEntry.WriteLog(LogID.LOG00009, AuditMsg00006)

    '    msgBox.Visible = False
    '    udcInfoBox.Visible = False

    '    ChangeViewIndex(ViewIndex.SearchResult)
    'End Sub



    Protected Sub ibtnMsgBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00039, AuditMsg00039)

        msgBox.Visible = False
        udcInfoBox.Visible = False
        'BackToSearchCriteriaView(True)

        'Refresh Summary Page
        If ddlVaccCentre.SelectedValue <> String.Empty Then
            GetVaccineLotSummary()
        End If

        ChangeViewIndex(ViewIndex.SearchSummary)
    End Sub

    Protected Sub ibtnBatchAssign_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00005, AuditMsg00005)
        ViewState("state") = StateType.ADD
        bindEditView()
    End Sub

    Protected Sub ibtnBatchRemove_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00014, AuditMsg00014)
        ViewState("state") = StateType.BatchRemove
        bindEditView()
    End Sub

    'Private Sub WriteUpdateFailedAuditLog(ByVal strVaccineLotID As String)
    '    If msgBox.GetCodeTable.Rows.Count = 0 Then
    '        msgBox.Visible = False
    '    Else
    '        Dim strAction As String = ViewState("action")
    '        Dim udtExAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
    '        udtExAuditLogEntry.AddDescripton("Vaccine Lot ID", strVaccineLotID)
    '        Select Case strAction
    '            Case ActionType.Update
    '                msgBox.BuildMessageBox("UpdateFail", udtExAuditLogEntry, LogID.LOG00022, AuditMsg00011)
    '            Case ActionType.Confirm
    '                msgBox.BuildMessageBox("UpdateFail", udtExAuditLogEntry, LogID.LOG00039, AuditMsg00017)
    '            Case ActionType.Remove
    '                msgBox.BuildMessageBox("UpdateFail", udtExAuditLogEntry, LogID.LOG00037, AuditMsg00015)
    '        End Select

    '    End If
    'End Sub


    Protected Sub ddlVaccCentre_SelectedIndexChanged(sender As Object, e As EventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00001, AuditMsg00001)
        Dim dt = New DataTable
        If ddlVaccCentre.SelectedValue = String.Empty Then

            dt = Nothing
            GridViewDataBind(gvSummary, dt, "Booth", "Asc", False)
            btnBatchAssign.Enabled = False
            btnBatchRemove.Enabled = False
            btnBatchAssign.ImageUrl = GetGlobalResourceObject("ImageUrl", "BatchAssignDisableBtn")
            btnBatchRemove.ImageUrl = GetGlobalResourceObject("ImageUrl", "BatchRemoveDisableBtn")
        Else
            Dim dtVaccinCentreSP As DataTable = udtCOVID19BLL.GetVaccineCentreSPMapping()
            Dim dr() As DataRow

            dr = dtVaccinCentreSP.Select(String.Format("Centre_ID = '{0}'", ddlVaccCentre.SelectedValue))

            If dr.Length > 0 Then
                GetVaccineLotSummary()
                btnBatchAssign.Enabled = True
                btnBatchRemove.Enabled = True
                btnBatchAssign.ImageUrl = GetGlobalResourceObject("ImageUrl", "BatchAssignBtn")
                btnBatchRemove.ImageUrl = GetGlobalResourceObject("ImageUrl", "BatchRemoveBtn")
                udcInfoBox.Visible = False
            Else

                GetVaccineLotSummary()
                btnBatchAssign.Enabled = False
                btnBatchRemove.Enabled = False
                btnBatchAssign.ImageUrl = GetGlobalResourceObject("ImageUrl", "BatchAssignDisableBtn")
                btnBatchRemove.ImageUrl = GetGlobalResourceObject("ImageUrl", "BatchRemoveDisableBtn")

                udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00006)
                udcInfoBox.Type = CustomControls.InfoMessageBoxType.Information
                udcInfoBox.BuildMessageBox()
            End If
        End If

    End Sub



#Region "Common Function"

    Protected Sub chkUpToExpiryDate_Click(sender As Object, e As EventArgs)
        If (chkUpToExpiryDate.Checked) Then
            txtVaccineLotEffectiveDateTo.Text = String.Empty
            txtVaccineLotEffectiveDateTo.Enabled = False
            btnVaccineLotEffectiveDateTo.Enabled = False
        Else
            txtVaccineLotEffectiveDateTo.Enabled = True
            btnVaccineLotEffectiveDateTo.Enabled = True
        End If
    End Sub


    Private Sub BindCentre(ByVal ddl As DropDownList)

        Dim centreDataTable As DataTable = udtVaccineLotBLL.GetCOVID19VaccineCentreHCVUMapping(udtHCVUUserBLL.GetHCVUUser.UserID)

        If centreDataTable IsNot Nothing AndAlso centreDataTable.Rows.Count > 0 Then
            ddl.Items.Clear()
            ddl.DataSource = centreDataTable
            ddl.DataTextField = "Centre_Name"
            ddl.DataValueField = "Centre_ID"
            ddl.DataBind()

            If ddl.Items.Count > 1 Then
                ddl.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))
            End If

            ddl.SelectedIndex = 0
            ddl.Enabled = True
        Else
            udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00005)
            udcInfoBox.Type = CustomControls.InfoMessageBoxType.Information
            udcInfoBox.BuildMessageBox()
            ddl.Enabled = False
        End If
    End Sub

    Protected Sub ddlVaccineCentre_SelectedIndexChanged(sender As Object, e As EventArgs)
        If (ddlVaccineCentre.SelectedValue <> String.Empty) Then
            BindBoothByCentre(Me.lbVaccineCentreBooth, ddlVaccineCentre.SelectedValue)
        Else
            Me.lbVaccineCentreBooth.Enabled = False
            Me.lbVaccineCentreBooth.Items.Clear()
        End If
    End Sub

    Private Sub BindBoothByCentre(ByVal bl As ListBox, ByVal CentreId As String)

        Dim dtBooth As DataTable = udtVaccineLotBLL.GetCOVID19VaccineBooth().DefaultView.ToTable(True, {"Centre_ID", "Booth", "Data_Value", "Display_Order"})
        Dim dvBooth As DataView = New DataView(dtBooth)
        dvBooth.RowFilter = "[Centre_ID] = '" + CentreId + "'"
        dvBooth.Sort = "Display_Order asc"

        'dtAllBrand.DefaultView.ToTable(True, "Brand_Name")

        bl.Items.Clear()
        If dvBooth IsNot Nothing AndAlso dvBooth.Count > 0 Then
            bl.DataSource = dvBooth
            bl.DataTextField = "Data_Value"
            bl.DataValueField = "Booth"
            bl.DataBind()

            'for testing
            'bl.Items.Insert(dvBooth.Count, New ListItem("outreach", String.Empty))

            'bl.SelectedIndex = 0
            bl.Enabled = True
        Else
            bl.Enabled = False
        End If

    End Sub

    'Private Sub BindBoothBase(ByVal ddl As DropDownList, ByVal blnIsEnabled As Boolean)

    '    ddl.Items.Clear()
    '    ddl.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))
    '    'ddl.SelectedIndex = 0
    '    ddl.Enabled = blnIsEnabled
    'End Sub

    Private Sub BuildBooth(ByVal lb As ListBox, ByVal blnIsAny As Boolean)
        If (ddlVaccineCentre.SelectedValue <> String.Empty) Then
            BindBoothByCentre(Me.lbVaccineCentreBooth, ddlVaccineCentre.SelectedValue)
        Else
            Me.lbVaccineCentreBooth.Enabled = False
            Me.lbVaccineCentreBooth.Items.Clear()
        End If
    End Sub

    Private Sub BuildBrand(ByVal ddl As DropDownList, ByVal blnIsAny As Boolean)

        Dim dtAllBrand As DataTable = Nothing
        Dim dvAllBrand As DataView = Nothing
        Select Case ViewState("state")
            Case StateType.ADD, StateType.EDIT

                dtAllBrand = udtCOVID19BLL.GetCOVID19VaccineBrandLotDetail()
                dvAllBrand = New DataView(dtAllBrand)
                'filter out the active brand
                dvAllBrand.RowFilter = udtCOVID19BLL.FilterActiveBrand
            Case StateType.BatchRemove
                dtAllBrand = udtVaccineLotBLL.GetCOVID19VaccineBrandDetailByCentre(ddlVaccCentre.SelectedItem.Value.Trim)
                Session(SESS_BatchRemoveBrandLotList) = dtAllBrand
                dvAllBrand = New DataView(dtAllBrand)
        End Select

        dtAllBrand = dvAllBrand.ToTable(True, {"Brand_Trade_Name", "Brand_ID"})

        If dtAllBrand IsNot Nothing AndAlso dtAllBrand.Rows.Count > 0 Then
            ddl.Items.Clear()
            ddl.DataSource = dtAllBrand
            ddl.DataTextField = "Brand_Trade_Name"
            ddl.DataValueField = "Brand_ID"
            ddl.DataBind()

            If blnIsAny = True Then
                ddl.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), String.Empty))
            Else
                If ddl.Items.Count > 1 Then
                    ddl.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))
                End If
            End If


            ddl.SelectedIndex = 0
            ddl.Enabled = True
        Else
            ddl.Enabled = False
        End If


    End Sub

   

    Protected Sub ddlVaccineBrandName_SelectedIndexChanged(sender As Object, e As EventArgs)
        If (ddlVaccineBrandName.SelectedValue <> String.Empty) Then
            BuildVaccineLotNo(Me.ddlVaccineLotNo, ddlVaccineBrandName.SelectedValue)
        Else
            Me.ddlVaccineLotNo.Enabled = False
            Me.ddlVaccineLotNo.Items.Clear()
            SetlblExpiryDateText()
        End If

    End Sub

    Private Sub BuildVaccineLotNo(ByVal ddl As DropDownList, ByVal strBrandId As String)

        Dim dtAllVaccine As DataTable = Nothing
        Dim dvVaccineLotNo As DataView = Nothing
        Select Case ViewState("state")
            Case StateType.ADD, StateType.EDIT
                dtAllVaccine = udtCOVID19BLL.GetCOVID19VaccineBrandLotDetail()

                dvVaccineLotNo = New DataView(dtAllVaccine)

                dvVaccineLotNo.RowFilter = udtCOVID19BLL.FilterActiveVaccineLot(strBrandId)
            Case StateType.BatchRemove
                If (Not IsNothing(Session(SESS_BatchRemoveBrandLotList))) Then
                    dtAllVaccine = CType(Session(SESS_BatchRemoveBrandLotList), DataTable)
                Else
                    dtAllVaccine = udtVaccineLotBLL.GetCOVID19VaccineBrandDetailByCentre(ddlVaccCentre.SelectedItem.Value.Trim)
                End If
                dvVaccineLotNo = New DataView(dtAllVaccine)
                dvVaccineLotNo.RowFilter = "[Brand_ID] = '" + strBrandId + "'"
        End Select

        If dvVaccineLotNo IsNot Nothing AndAlso dvVaccineLotNo.Count > 0 Then
            ddl.Items.Clear()
            ddl.DataSource = dvVaccineLotNo
            ddl.DataTextField = "Vaccine_Lot_No"
            ddl.DataValueField = "Vaccine_Lot_No"
            ddl.DataBind()

            If ddl.Items.Count > 1 Then
                ddl.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))
            End If

            ddl.SelectedIndex = 0
            ddl.Enabled = True
            SetlblExpiryDateText()
        Else
            ddl.Enabled = False
        End If

    End Sub



    'Private Sub BuildVaccineLotRecordStatus(ByVal ddl As DropDownList)
    '    Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL
    '    ddl.DataSource = udtStaticDataBLL.GetStaticDataListByColumnName("VaccineLotRecordStatus")
    '    ddl.DataValueField = "ItemNo"
    '    ddl.DataTextField = "DataValue"
    '    ddl.DataBind()
    '    ddl.Items.Remove(ddl.Items.FindByValue(VaccineLotMappingRecordStatus.Remove))
    '    ddl.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))
    '    ddl.SelectedIndex = 0
    'End Sub

    Protected Sub ddlVaccineLotNo_SelectedIndexChanged(sender As Object, e As EventArgs)
        SetlblExpiryDateText()
    End Sub

    Private Sub SetlblExpiryDateText()

        If (ddlVaccineLotNo.SelectedValue <> String.Empty And ddlVaccineLotNo.Enabled <> False) Then
            Dim dvVaccineWithExpiryDate As DataView = New DataView(udtCOVID19BLL.GetCOVID19VaccineBrandLotDetail())
            dvVaccineWithExpiryDate.RowFilter = "[Brand_Id] = '" + ddlVaccineBrandName.SelectedValue + "' and [Vaccine_Lot_NO] = '" + ddlVaccineLotNo.SelectedValue + "'"

            If dvVaccineWithExpiryDate IsNot Nothing AndAlso dvVaccineWithExpiryDate.Count = 1 Then
                Me.lblVaccineExpiryDateText.Text = Format(dvVaccineWithExpiryDate(0)("Expiry_Date"), "dd MMM yyyy")
            End If
        Else
            Me.lblVaccineExpiryDateText.Text = Me.GetGlobalResourceObject("Text", "NA")
        End If
    End Sub

    Private Sub InitializeDataValue()
        ResetAllInputPageBind()
        ResetAllInputPageErrImage()
        BuildBrand(Me.ddlVaccineBrandName, False)

        If (Not Me.ddlVaccineBrandName.SelectedValue = String.Empty And Me.ddlVaccineBrandName.Items.Count = 1) Then
            BuildVaccineLotNo(Me.ddlVaccineLotNo, ddlVaccineBrandName.SelectedValue)
        End If

        'set centre from search criteria page
        ddlVaccineCentre.Items.Insert(0, New ListItem(ddlVaccCentre.SelectedItem.Text.Trim, ddlVaccCentre.SelectedItem.Value.Trim))
        ddlVaccineCentre.SelectedIndex = 0
        ddlVaccineCentre.Enabled = True


        'set booth after get centre from search criteria
        BuildBooth(Me.lbVaccineCentreBooth, False)

        If Me.ddlVaccineCentre.SelectedValue = String.Empty Then
            lbVaccineCentreBooth.Enabled = False
        End If
        If Me.ddlVaccineBrandName.SelectedValue = String.Empty Then
            ddlVaccineLotNo.Enabled = False
        End If

    End Sub

    Private Sub ResetAllInputPageBind()
        ddlVaccineCentre.Items.Clear()
        lbVaccineCentreBooth.Items.Clear()
        ddlVaccineBrandName.Items.Clear()
        ddlVaccineLotNo.Items.Clear()
        txtVaccineLotEffectiveDateFrom.Text = String.Empty
        txtVaccineLotEffectiveDateTo.Text = String.Empty
        txtVaccineLotEffectiveDateTo.Enabled = True
        btnVaccineLotEffectiveDateTo.Enabled = True
        chkUpToExpiryDate.Checked = False
        'Set Expiry Label before changed by lot number
        Me.lblVaccineExpiryDateText.Text = Me.GetGlobalResourceObject("Text", "NA")
        Session.Remove(SESS_BatchRemoveBrandLotList)
        Session.Remove(SESS_Confirm_HistoryView)

    End Sub

    Private Sub ResetAllInputPageErrImage()
        imgVaccineCentreErr.Visible = False
        imgVaccineCentreBoothErr.Visible = False
        imgVaccineBrandNameErr.Visible = False
        imgVaccineLotNoErr.Visible = False
        imgVaccineExpiryDateTextErr.Visible = False
        imgVaccineLotEffectiveDateFromErr.Visible = False
        imgVaccineLotEffectiveDateToErr.Visible = False

    End Sub

    Private Sub ResetAllHiddenField()
        hfConVaccineCentreId.Text = String.Empty
        hfConVaccineLotEffectiveDateFrom.Text = String.Empty
        hfNewConVaccineLotEffectiveDateFrom.Text = String.Empty
        hfConVaccineLotEffectiveDateTo.Text = String.Empty
        hfNewConVaccineLotEffectiveDateTo.Text = String.Empty
        hflEConVaccineBoothId.Text = String.Empty

        hfPERConVaccineCentreID.Text = String.Empty
        hfPERConVaccineBoothId.Text = String.Empty
        hfVaccineLotEffectiveDateTo.Text = String.Empty
        hfVaccineLotEffectiveDateFrom.Text = String.Empty
        hfEVaccineLotId.Text = String.Empty

        Session.Remove(SESS_BatchRemoveBrandLotList)
        Session.Remove(SESS_Confirm_HistoryView)
    End Sub

    Private Sub ResetConfirmPageField()
        Select Case ViewState("state")
            Case StateType.ADD, StateType.EDIT
                trConEffectiveDateFrom.Visible = True
                trConEffectiveDateTo.Visible = True
            Case StateType.BatchRemove
                trConEffectiveDateFrom.Visible = False
                trConEffectiveDateTo.Visible = False
        End Select

        'lblNewConVaccineLotEffectiveDateFrom.Text = String.Empty
        'lblNewConVaccineLotEffectiveDateTo.Text = String.Empty
    End Sub



#End Region

#Region "Popup box events"

    Private Sub ibtnDialogConfirm_Click(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUpConfirmRemove.ButtonClick
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        Dim blnResult As New Boolean
        Dim strAction As String = String.Empty

        Select Case e
            Case ucNoticePopUp.enumButtonClick.OK
                udtAuditLogEntry.WriteLog(LogID.LOG00034, AuditMsg00034)
                AddSingleRemoveRecordRequest()

            Case Else
                udtAuditLogEntry.WriteLog(LogID.LOG00035, AuditMsg00035)
                udcInfoBox.Visible = False
        End Select
    End Sub

    'Private Sub ibtnDialogCancel_Click(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUpConfirmCancel.ButtonClick
    '    Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
    '    '  Dim udtVaccineLotList As VaccineLotModel = Nothing
    '    Dim blnResult As New Boolean
    '    Dim strAction As String = String.Empty

    '    Select Case e
    '        Case ucNoticePopUp.enumButtonClick.OK
    '            EditRecordCancelRequest()
    '            'udtAuditLogEntry.WriteLog(LogID.LOG00036, AuditMsg00017)
    '        Case Else
    '            udcInfoBox.Visible = False
    '            ViewState("action") = ActionType.Cancel
    '            udtAuditLogEntry.WriteLog(LogID.LOG00036, AuditMsg00017)
    '    End Select

    'End Sub

#End Region



#Region "Vaccine Lot Record Event"

    Protected Sub ibtnRecordCancel_Click(sender As Object, e As ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        msgBox.Visible = False
        udcInfoBox.Visible = False

        If ViewState("state") = StateType.ADD Then
            udtAuditLogEntry.WriteEndLog(LogID.LOG00006, AuditMsg00006)
            GetVaccineLotSummary()
            ChangeViewIndex(ViewIndex.SearchSummary)

        ElseIf ViewState("state") = StateType.EDIT Then
            udtAuditLogEntry.WriteEndLog(LogID.LOG00027, AuditMsg00027)
            ChangeViewIndex(ViewIndex.EditRecordView)

        ElseIf ViewState("state") = StateType.BatchRemove Then

            udtAuditLogEntry.WriteEndLog(LogID.LOG00015, AuditMsg00015)
            GetVaccineLotSummary()
            ChangeViewIndex(ViewIndex.SearchSummary)
        End If




    End Sub

    Private Sub GetVaccineLotForConfirmPage()

        Dim dtVaccineeLotForDisplay As DataTable = Nothing

        If (Not IsNothing(Session(SESS_Confirm_HistoryView))) Then
            dtVaccineeLotForDisplay = CType(Session(SESS_Confirm_HistoryView), DataTable)
        Else
            dtVaccineeLotForDisplay = udtVaccineLotBLL.GetVaccineLotListDetailConfirm(hfConVaccineCentreId.Text.Trim, lblConVaccineBoothID.Text.Trim, lblConVaccineLotNo.Text.Trim)
        End If

        Select Case ViewState("state")
            Case StateType.ADD, StateType.EDIT
                gvConfirmDetail.Columns(3).Visible = False
            Case StateType.BatchRemove
                gvConfirmDetail.Columns(3).Visible = True
        End Select

        gvConfirmDetail.DataSource = dtVaccineeLotForDisplay
        gvConfirmDetail.DataBind()
    End Sub


    Protected Sub ibtnRecordSave_Click(sender As Object, e As ImageClickEventArgs)
        Dim strBooth As String = String.Empty
        Dim strBoothID As String = String.Empty
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        Try
            msgBox.Visible = False
            udcInfoBox.Visible = False
            ResetAllInputPageErrImage()
            ResetConfirmPageField()
            If ViewState("state") = StateType.ADD Or ViewState("state") = StateType.BatchRemove Then
                If lbVaccineCentreBooth.SelectedValue <> String.Empty Then
                    For i As Integer = 0 To lbVaccineCentreBooth.Items.Count - 1
                        If (lbVaccineCentreBooth.Items(i).Selected) Then
                            strBooth += lbVaccineCentreBooth.Items(i).Text.Trim
                            strBooth += ","
                            strBoothID += lbVaccineCentreBooth.Items(i).Value.Trim
                            strBoothID += ","
                        End If
                    Next
                    strBooth = strBooth.Remove(strBooth.Length - 1)
                    strBoothID = strBoothID.Remove(strBoothID.Length - 1)
                End If
            End If

            If ViewState("state") = StateType.ADD Then
                ibtnRecordConfirm.Enabled = True
                ibtnRecordConfirm.ImageUrl = GetGlobalResourceObject("ImageUrl", "ConfirmBtn")

                udtAuditLogEntry.AddDescripton("Centre Id", ddlVaccineCentre.SelectedValue.ToString)
                udtAuditLogEntry.AddDescripton("Booth", strBooth)
                udtAuditLogEntry.AddDescripton("Lot Number", ddlVaccineLotNo.SelectedValue.ToString)
                udtAuditLogEntry.AddDescripton("Effective Date From", txtVaccineLotEffectiveDateFrom.Text.Trim)
                udtAuditLogEntry.AddDescripton("Effective Date To", IIf(chkUpToExpiryDate.Checked, String.Empty, txtVaccineLotEffectiveDateTo.Text.Trim))
                udtAuditLogEntry.AddDescripton("chk Up To Expiry Date", IIf(chkUpToExpiryDate.Checked, YesNo.Yes, YesNo.No))
                udtAuditLogEntry.WriteStartLog(LogID.LOG00007, AuditMsg00007)

            ElseIf ViewState("state") = StateType.EDIT Then

                udtAuditLogEntry.AddDescripton("Lot Id", hfEVaccineLotId.Text)
                udtAuditLogEntry.AddDescripton("New Effective Date From", txtVaccineLotEffectiveDateFrom.Text.Trim)
                udtAuditLogEntry.AddDescripton("New Effective Date To", IIf(chkUpToExpiryDate.Checked, String.Empty, txtVaccineLotEffectiveDateTo.Text.Trim))
                udtAuditLogEntry.AddDescripton("chk Up To Expiry Date", IIf(chkUpToExpiryDate.Checked, YesNo.Yes, YesNo.No))
                udtAuditLogEntry.WriteStartLog(LogID.LOG00027, AuditMsg00027)

            ElseIf ViewState("state") = StateType.BatchRemove Then

                udtAuditLogEntry.AddDescripton("Centre Id", ddlVaccineCentre.SelectedValue.ToString)
                udtAuditLogEntry.AddDescripton("Booth", strBooth)
                udtAuditLogEntry.AddDescripton("Lot Number", ddlVaccineLotNo.SelectedValue.ToString)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00016, AuditMsg00016)
            End If


            If (SaveRecordInfoValidation()) Then

                If ViewState("state") = StateType.ADD Then
                    'Confirm Details page
                    lblConVaccineCentre.Text = ddlVaccineCentre.SelectedItem.ToString
                    lblConVaccineBooth.Text = strBooth
                    lblConVaccineBoothID.Text = strBoothID
                    lblConVaccineBrandName.Text = ddlVaccineBrandName.SelectedItem.ToString
                    lblConVaccineLotNo.Text = ddlVaccineLotNo.SelectedValue.ToString
                    lblConVaccineExpiryDateText.Text = lblVaccineExpiryDateText.Text.Trim
                    lblConVaccineLotEffectiveDateFrom.Text = udtFormatter.convertDate(txtVaccineLotEffectiveDateFrom.Text.Trim, String.Empty)
                    lblConVaccineLotEffectiveDateTo.Text = IIf(chkUpToExpiryDate.Checked, Me.GetGlobalResourceObject("Text", "UpToVacExpDate") + " " + lblConVaccineExpiryDateText.Text, udtFormatter.convertDate(txtVaccineLotEffectiveDateTo.Text.Trim, String.Empty))
                    'lblConVaccineLotEffectiveDateTo.Text = udtFormatter.convertDate(txtVaccineLotEffectiveDateTo.Text.Trim, String.Empty)


                    'sethiddentLabel for spro
                    hfConVaccineCentreId.Text = ddlVaccineCentre.SelectedValue.ToString
                    hfConVaccineLotEffectiveDateFrom.Text = udtFormatter.convertDate(txtVaccineLotEffectiveDateFrom.Text.Trim, String.Empty)
                    hfConVaccineLotEffectiveDateTo.Text = IIf(chkUpToExpiryDate.Checked, String.Empty, udtFormatter.convertDate(txtVaccineLotEffectiveDateTo.Text.Trim, String.Empty))
                    hfUpToVacExpDateTo.Text = IIf(chkUpToExpiryDate.Checked, YesNo.Yes, YesNo.No)
                    'hfConVaccineLotEffectiveDateTo.Value = udtFormatter.convertDate(txtVaccineLotEffectiveDateTo.Text.Trim, String.Empty)

                    udtAuditLogEntry.WriteEndLog(LogID.LOG00008, AuditMsg00008)
                ElseIf ViewState("state") = StateType.EDIT Then
                    Dim udtVaccineLotList As VaccineLotModel = Nothing
                    udtVaccineLotList = udtVaccineLotBLL.GetVaccineLotModalByVaccineLotId(hfEVaccineLotId.Text, VaccineLotRecordServiceType.Centre)
                    lblConVaccineCentre.Text = udtVaccineLotList.CentreName
                    lblConVaccineBooth.Text = udtVaccineLotList.Booth
                    lblConVaccineBoothID.Text = udtVaccineLotList.BoothId
                    lblConVaccineBrandName.Text = udtVaccineLotList.BrandTradeName
                    lblConVaccineLotNo.Text = udtVaccineLotList.VaccineLotNo
                    lblConVaccineExpiryDateText.Text = udtFormatter.formatDisplayDate(CDate(udtVaccineLotList.VaccineExpiryDate))

                    'only Two field could be changed
                    lblConVaccineLotEffectiveDateFrom.Text = udtFormatter.convertDate(txtVaccineLotEffectiveDateFrom.Text.Trim, String.Empty)
                    lblConVaccineLotEffectiveDateTo.Text = IIf(chkUpToExpiryDate.Checked, Me.GetGlobalResourceObject("Text", "UpToVacExpDate") + " " + lblConVaccineExpiryDateText.Text, udtFormatter.formatDisplayDate(txtVaccineLotEffectiveDateTo.Text.Trim, String.Empty))
                    'lblConVaccineLotEffectiveDateTo.Text = udtFormatter.formatDisplayDate(CDate(udtVaccineLotList.VaccineLotEffectiveDTo))

                    'sethiddentLabel for spro
                    hfConVaccineCentreId.Text = udtVaccineLotList.CentreId
                    hfConVaccineLotEffectiveDateFrom.Text = udtFormatter.formatDisplayDate(CDate(udtVaccineLotList.VaccineLotEffectiveDFrom))
                    hfConVaccineLotEffectiveDateTo.Text = IIf(chkUpToExpiryDate.Checked, String.Empty, udtFormatter.formatDisplayDate(CDate(udtVaccineLotList.VaccineLotEffectiveDFrom)))
                    hfUpToVacExpDateTo.Text = IIf(chkUpToExpiryDate.Checked, YesNo.Yes, YesNo.No)

                    hfNewConVaccineLotEffectiveDateFrom.Text = udtFormatter.convertDate(txtVaccineLotEffectiveDateFrom.Text.Trim, String.Empty)
                    hfNewConVaccineLotEffectiveDateTo.Text = IIf(chkUpToExpiryDate.Checked, String.Empty, udtFormatter.convertDate(txtVaccineLotEffectiveDateTo.Text.Trim, String.Empty))


                    'Set Confirm HistroyView Table by Detail page
                    Dim dtconfirmPageDisplay As New DataTable
                    dtconfirmPageDisplay.Columns.Add("Booth", GetType(String))
                    dtconfirmPageDisplay.Columns.Add("Service_Period_From", GetType(DateTime))
                    dtconfirmPageDisplay.Columns.Add("Service_Period_To", GetType(DateTime))
                    dtconfirmPageDisplay.Columns.Add("Use_Of_ExpiryDtm", GetType(String))
                    dtconfirmPageDisplay.Columns.Add("Record_Status", GetType(String))

                    Dim strTempEffectiveDtm As New Nullable(Of Date)

                    If udtVaccineLotList.VaccineLotEffectiveDTo IsNot String.Empty Then
                        strTempEffectiveDtm = DateTime.Parse(udtVaccineLotList.VaccineLotEffectiveDTo)
                    End If
                    dtconfirmPageDisplay.Rows.Add(udtVaccineLotList.Booth, DateTime.Parse(udtVaccineLotList.VaccineLotEffectiveDFrom), strTempEffectiveDtm, udtVaccineLotList.UpToExpiryDtm, udtVaccineLotList.RecordStatus)
                    Session(SESS_Confirm_HistoryView) = dtconfirmPageDisplay

                    udtAuditLogEntry.WriteEndLog(LogID.LOG00028, AuditMsg00028)

                ElseIf ViewState("state") = StateType.BatchRemove Then
                    lblConVaccineCentre.Text = ddlVaccineCentre.SelectedItem.ToString
                    lblConVaccineBooth.Text = strBooth
                    lblConVaccineBoothID.Text = strBoothID
                    lblConVaccineBrandName.Text = ddlVaccineBrandName.SelectedItem.ToString
                    lblConVaccineLotNo.Text = ddlVaccineLotNo.SelectedValue.ToString
                    lblConVaccineExpiryDateText.Text = lblVaccineExpiryDateText.Text.Trim
                    lblConVaccineLotEffectiveDateFrom.Text = udtFormatter.convertDate(txtVaccineLotEffectiveDateFrom.Text.Trim, String.Empty)
                    lblConVaccineLotEffectiveDateTo.Text = udtFormatter.convertDate(txtVaccineLotEffectiveDateTo.Text.Trim, String.Empty)


                    'sethiddentLabel for spro
                    hfConVaccineCentreId.Text = ddlVaccineCentre.SelectedValue.ToString
                    hfConVaccineLotEffectiveDateFrom.Text = udtFormatter.convertDate(txtVaccineLotEffectiveDateFrom.Text.Trim, String.Empty)
                    hfConVaccineLotEffectiveDateTo.Text = udtFormatter.convertDate(txtVaccineLotEffectiveDateTo.Text.Trim, String.Empty)
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00017, AuditMsg00017)

                End If

                GetVaccineLotForConfirmPage()
                ChangeViewIndex(ViewIndex.Confirm)
                udcInfoBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00021)
                udcInfoBox.Type = CustomControls.InfoMessageBoxType.Information
                udcInfoBox.BuildMessageBox()
            Else
                If ViewState("state") = StateType.ADD Then
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00009, AuditMsg00009)
                ElseIf ViewState("state") = StateType.EDIT Then
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00029, AuditMsg00029)
                ElseIf ViewState("state") = StateType.BatchRemove Then
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00018, AuditMsg00018)
                End If

            End If
        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception")
            udtAuditLogEntry.AddDescripton("Message", ex.Message)
            If ViewState("state") = StateType.ADD Then
                udtAuditLogEntry.WriteEndLog(LogID.LOG00009, AuditMsg00009)
            ElseIf ViewState("state") = StateType.EDIT Then
                udtAuditLogEntry.WriteEndLog(LogID.LOG00029, AuditMsg00029)
            ElseIf ViewState("state") = StateType.BatchRemove Then
                udtAuditLogEntry.WriteEndLog(LogID.LOG00018, AuditMsg00018)
            End If
            Throw
        End Try

    End Sub

    Protected Sub ibtnRecordBackToInput_Click(sender As Object, e As ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        If ViewState("state") = StateType.ADD Then
            udtAuditLogEntry.WriteEndLog(LogID.LOG00010, AuditMsg00010)
        ElseIf ViewState("state") = StateType.EDIT Then
            udtAuditLogEntry.WriteEndLog(LogID.LOG00030, AuditMsg00030)
        ElseIf ViewState("state") = StateType.BatchRemove Then
            udtAuditLogEntry.WriteEndLog(LogID.LOG00019, AuditMsg00019)
        End If

        msgBox.Visible = False
        udcInfoBox.Visible = False
        ChangeViewIndex(ViewIndex.NewEditRecord)

    End Sub

    Protected Sub ibtnRecordConfirm_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtVaccineLotList As VaccineLotModel = New VaccineLotModel()
        Dim blnValid As Boolean = True
        Dim dtLotDetail As DataTable = Nothing
        Dim dvLotDetailFilterWithRecordStatus As DataView = Nothing
        Dim dvLotDetailFilterWithAssignStatus As DataView = Nothing

        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)


        Dim strRequestPendingBoothList As String = Nothing

        strRequestPendingBoothList = ckPendingRequestRecordExist(hfConVaccineCentreId.Text, lblConVaccineBoothID.Text.Trim, lblConVaccineLotNo.Text.Trim)

        If Not strRequestPendingBoothList Is Nothing Then
            msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00004), "%s", strRequestPendingBoothList)
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00038, AuditMsg00038)
            udcInfoBox.Visible = False
            blnValid = False
        End If

        If ViewState("state") = StateType.ADD Then
            dtLotDetail = udtVaccineLotBLL.CheckVaccineLotDetailExist(lblConVaccineLotNo.Text.Trim)
            'check the lot detail is removed  
            If (blnValid) Then

                dvLotDetailFilterWithRecordStatus = New DataView(dtLotDetail)
                dvLotDetailFilterWithRecordStatus.RowFilter = udtVaccineLotBLL.FilterLotDetailByRecordStatus(VaccineLotDetailRecordStatus.Remove)

                If dvLotDetailFilterWithRecordStatus.Count > 0 Then
                    msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00009), "%s", lblConVaccineLotNo.Text.Trim)
                    blnValid = False
                End If
            End If

            'check the lot detail is marked at unavailable
            If (blnValid) Then
                dvLotDetailFilterWithAssignStatus = New DataView(dtLotDetail)
                dvLotDetailFilterWithAssignStatus.RowFilter = udtVaccineLotBLL.FilterLotDetailByLotAssignStatus(VaccineLotDetailLotAssignStatus.Unavailable)

                If dvLotDetailFilterWithAssignStatus.Count > 0 Then
                    msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00010), "%s", lblConVaccineLotNo.Text.Trim)
                    blnValid = False
                End If
            End If
        End If

        Try
            If (blnValid) Then
                msgBox.Visible = False
                udcInfoBox.Visible = True
                ibtnRecordConfirm.Enabled = True
                ibtnRecordConfirm.ImageUrl = GetGlobalResourceObject("ImageUrl", "ConfirmBtn")

                udtVaccineLotList.ServiceType = VaccineLotRecordServiceType.Centre
                udtVaccineLotList.CentreId = hfConVaccineCentreId.Text
                udtVaccineLotList.BoothId = lblConVaccineBoothID.Text.Trim
                udtVaccineLotList.BrandTradeName = lblConVaccineBrandName.Text.Trim
                udtVaccineLotList.VaccineLotNo = lblConVaccineLotNo.Text.Trim
                udtVaccineLotList.VaccineExpiryDate = lblConVaccineExpiryDateText.Text
                udtVaccineLotList.VaccineLotEffectiveDFrom = hfConVaccineLotEffectiveDateFrom.Text
                udtVaccineLotList.VaccineLotEffectiveDTo = hfConVaccineLotEffectiveDateTo.Text
                udtVaccineLotList.CreateBy = udtHCVUUserBLL.GetHCVUUser.UserID
                udtVaccineLotList.UpdateBy = udtHCVUUserBLL.GetHCVUUser.UserID
                udtVaccineLotList.RequestBy = udtHCVUUserBLL.GetHCVUUser.UserID
                udtVaccineLotList.UpToExpiryDtm = hfUpToVacExpDateTo.Text

                udtAuditLogEntry.AddDescripton("Centre ID", udtVaccineLotList.CentreId)
                udtAuditLogEntry.AddDescripton("Booth ID", udtVaccineLotList.BoothId)
                udtAuditLogEntry.AddDescripton("Lot Number", udtVaccineLotList.VaccineLotNo)
                udtAuditLogEntry.AddDescripton("Effective Date From", hfConVaccineLotEffectiveDateFrom.Text)
                udtAuditLogEntry.AddDescripton("Effective Date To", hfConVaccineLotEffectiveDateTo.Text)
                udtAuditLogEntry.AddDescripton("Create By", udtVaccineLotList.CreateBy)
                udtAuditLogEntry.AddDescripton("chk Up To Expiry Date", udtVaccineLotList.UpToExpiryDtm)


                If ViewState("state") = StateType.ADD Then
                    udtVaccineLotList.RequestType = VaccineLotRequestType.REQUESTTYPE_NEW

                    udtAuditLogEntry.WriteStartLog(LogID.LOG00011, AuditMsg00011)
                ElseIf ViewState("state") = StateType.EDIT Then
                    udtVaccineLotList.RequestType = VaccineLotRequestType.REQUESTTYPE_AMEND
                    udtVaccineLotList.VaccineLotID = hfEVaccineLotId.Text
                    udtVaccineLotList.NewVaccineLotEffectiveDFrom = hfNewConVaccineLotEffectiveDateFrom.Text
                    udtVaccineLotList.NewVaccineLotEffectiveDTo = hfNewConVaccineLotEffectiveDateTo.Text

                    udtAuditLogEntry.AddDescripton("Lot Id", udtVaccineLotList.VaccineLotID)
                    udtAuditLogEntry.AddDescripton("New Effective Date From", udtVaccineLotList.NewVaccineLotEffectiveDFrom)
                    udtAuditLogEntry.AddDescripton("New Effective Date To", udtVaccineLotList.NewVaccineLotEffectiveDTo)
                    udtAuditLogEntry.WriteStartLog(LogID.LOG00031, AuditMsg00031)
                ElseIf (ViewState("state") = StateType.BatchRemove) Then
                    udtVaccineLotList.RequestType = VaccineLotRequestType.REQUESTTYPE_REMOVE
                    udtVaccineLotList.VaccineLotEffectiveDFrom = Nothing
                    udtVaccineLotList.VaccineLotEffectiveDTo = Nothing
                    udtAuditLogEntry.WriteStartLog(LogID.LOG00020, AuditMsg00020)
                End If

                Dim strRequestId As String = String.Empty

                If (udtVaccineLotBLL.AddCOVID19VaccineLotMappingRequest(udtVaccineLotList, strRequestId)) Then
                    udcInfoBox.Type = CustomControls.InfoMessageBoxType.Complete
                    udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001, {"%s"}, {strRequestId})
                    udcInfoBox.BuildMessageBox()
                    ChangeViewIndex(ViewIndex.MsgPage)
                    If ViewState("state") = StateType.ADD Then
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00012, AuditMsg00012)
                    ElseIf ViewState("state") = StateType.EDIT Then
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00032, AuditMsg00032)
                    ElseIf ViewState("state") = StateType.BatchRemove Then
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00021, AuditMsg00021)
                    End If
                Else
                    If ViewState("state") = StateType.ADD Then
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00013, AuditMsg00013)
                    ElseIf ViewState("state") = StateType.EDIT Then
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00033, AuditMsg00033)
                    ElseIf ViewState("state") = StateType.BatchRemove Then
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00022, AuditMsg00022)
                    End If
                End If

                Session.Remove(SESS_BatchRemoveBrandLotList)
                Session.Remove(SESS_Confirm_HistoryView)
            Else
                msgBox.Visible = True
                udcInfoBox.Visible = False
                ibtnRecordConfirm.Enabled = False
                ibtnRecordConfirm.ImageUrl = GetGlobalResourceObject("ImageUrl", "ConfirmDisableBtn")
            End If
        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception")
            udtAuditLogEntry.AddDescripton("Message", ex.Message)
            If ViewState("state") = StateType.ADD Then
                udtAuditLogEntry.WriteEndLog(LogID.LOG00013, AuditMsg00013)
            ElseIf ViewState("state") = StateType.EDIT Then
                udtAuditLogEntry.WriteEndLog(LogID.LOG00033, AuditMsg00033)
            ElseIf ViewState("state") = StateType.BatchRemove Then
                udtAuditLogEntry.WriteEndLog(LogID.LOG00022, AuditMsg00022)
            End If
            Throw
        End Try


    End Sub

    Protected Sub ibtnEditRecordBack_Click(sender As Object, e As ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00024, AuditMsg00024)
        msgBox.Visible = False
        udcInfoBox.Visible = False
        ChangeViewIndex(ViewIndex.SearchSummary)
    End Sub

    Protected Sub ibtnEditRecordEdit_Click(sender As Object, e As ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00025, AuditMsg00025)
        ViewState("state") = StateType.EDIT
        bindEditView()
    End Sub

    Protected Sub ibtnEditRecordRemove_Click(sender As Object, e As ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00040, AuditMsg00040)
        ModalPopupConfirmRemove.Show()

    End Sub

    Protected Sub AddSingleRemoveRecordRequest()

        Dim blnResult As New Boolean
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        Try
            msgBox.Visible = False
            udcInfoBox.Visible = False

            If RemoveRecordInfoValidation(hflblEConVaccineCentreId.Text, hflEConVaccineBoothId.Text, lblEConVaccineLotNo.Text) Then
                Dim udtVaccineLotList As VaccineLotModel = New VaccineLotModel()

                udtVaccineLotList.ServiceType = VaccineLotRecordServiceType.Centre
                udtVaccineLotList.CentreId = hflblEConVaccineCentreId.Text
                udtVaccineLotList.BoothId = hflEConVaccineBoothId.Text
                udtVaccineLotList.BrandTradeName = lblEConVaccineBrandName.Text
                udtVaccineLotList.VaccineLotNo = lblEConVaccineLotNo.Text
                udtVaccineLotList.VaccineExpiryDate = lblEConVaccineExpiryDateText.Text
                udtVaccineLotList.VaccineLotEffectiveDFrom = Nothing
                udtVaccineLotList.VaccineLotEffectiveDTo = Nothing
                udtVaccineLotList.CreateBy = udtHCVUUserBLL.GetHCVUUser.UserID
                udtVaccineLotList.UpdateBy = udtHCVUUserBLL.GetHCVUUser.UserID
                udtVaccineLotList.RequestBy = udtHCVUUserBLL.GetHCVUUser.UserID
                udtVaccineLotList.RequestType = VaccineLotRequestType.REQUESTTYPE_REMOVE

                udtAuditLogEntry.AddDescripton("Centre Id", udtVaccineLotList.CentreId)
                udtAuditLogEntry.AddDescripton("Booth", udtVaccineLotList.BoothId)
                udtAuditLogEntry.AddDescripton("Lot Number", udtVaccineLotList.VaccineLotNo)

                Dim strRequestId As String = String.Empty
                blnResult = udtVaccineLotBLL.AddCOVID19VaccineLotMappingRequest(udtVaccineLotList, strRequestId)
                If (blnResult) Then

                    udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001, {"%s"}, {strRequestId})
                    udcInfoBox.Type = CustomControls.InfoMessageBoxType.Complete
                    udcInfoBox.BuildMessageBox()
                    ChangeViewIndex(ViewIndex.MsgPage)

                    udtAuditLogEntry.WriteLog(LogID.LOG00036, AuditMsg00036)
                Else
                    udtAuditLogEntry.WriteLog(LogID.LOG00037, AuditMsg00037)
                End If
            Else
                msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00038, AuditMsg00038)
            End If
        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception")
            udtAuditLogEntry.AddDescripton("Message", ex.Message)
            udtAuditLogEntry.WriteLog(LogID.LOG00037, AuditMsg00037)
            Throw
        End Try

    End Sub

#End Region


    Private Sub bindEditView()
        msgBox.Visible = False
        udcInfoBox.Visible = False
        ChangeViewIndex(ViewIndex.NewEditRecord)

        If ViewState("state") = StateType.ADD Then
            InitializeDataValue()

            SetCalenderVisible()
            panNewRecord.Visible = True
            panEditRecord.Visible = False

        ElseIf ViewState("state") = StateType.EDIT Then
            ResetAllInputPageErrImage()
            ResetAllInputPageBind()
            SetCalenderVisible()
            panNewRecord.Visible = False
            panEditRecord.Visible = True

            Dim udtVaccineLotList As VaccineLotModel = Nothing
            udtVaccineLotList = udtVaccineLotBLL.GetVaccineLotModalByVaccineLotId(hfEVaccineLotId.Text, VaccineLotRecordServiceType.Centre)
            lblPERConVaccineCentre.Text = udtVaccineLotList.CentreName
            lblPERConVaccineBooth.Text = udtVaccineLotList.Booth
            hfPERConVaccineBoothId.Text = udtVaccineLotList.BoothId
            lblPERConVaccineBrand.Text = udtVaccineLotList.BrandTradeName
            lblPERConVaccineLotNo.Text = udtVaccineLotList.VaccineLotNo
            lblVaccineExpiryDateText.Text = Format(CDate(udtVaccineLotList.VaccineExpiryDate), "dd MMM yyyy")
            chkUpToExpiryDate.Checked = IIf(udtVaccineLotList.UpToExpiryDtm = YesNo.Yes, True, False)

            txtVaccineLotEffectiveDateFrom.Text = Format(CDate(udtVaccineLotList.VaccineLotEffectiveDFrom), "dd-MM-yyyy")
            hfVaccineLotEffectiveDateFrom.Text = Format(CDate(udtVaccineLotList.VaccineLotEffectiveDFrom), "dd-MM-yyyy")

            If udtVaccineLotList.UpToExpiryDtm = YesNo.Yes Then
                txtVaccineLotEffectiveDateTo.Text = String.Empty
                txtVaccineLotEffectiveDateTo.Enabled = False
                btnVaccineLotEffectiveDateTo.Enabled = False
                hfVaccineLotEffectiveDateTo.Text = String.Empty
            Else
                txtVaccineLotEffectiveDateTo.Text = Format(CDate(udtVaccineLotList.VaccineLotEffectiveDTo), "dd-MM-yyyy")
                btnVaccineLotEffectiveDateTo.Enabled = True
                hfVaccineLotEffectiveDateTo.Text = Format(CDate(udtVaccineLotList.VaccineLotEffectiveDTo), "dd-MM-yyyy")
            End If

            hfPERConVaccineCentreID.Text = udtVaccineLotList.CentreId

        ElseIf ViewState("state") = StateType.BatchRemove Then
            InitializeDataValue()

            SetCalenderVisible()
            panNewRecord.Visible = True
            panEditRecord.Visible = False
        End If
    End Sub

    Private Sub SetCalenderVisible()

        If ViewState("state") = StateType.ADD Or ViewState("state") = StateType.EDIT Then
            penEffectiveDateCalender.Visible = True

        ElseIf ViewState("state") = StateType.BatchRemove Then
            penEffectiveDateCalender.Visible = False
        End If
    End Sub




    Private Sub ChangeViewIndex(ByVal udtViewIndex As ViewIndex)
        MultiViewEnquiry.ActiveViewIndex = udtViewIndex

        Select Case udtViewIndex
            Case ViewIndex.SearchSummary
                'Session.Remove(SESS_VaccineLotListModal)
                'Case ViewIndex.SearchResult
                '    Session.Remove(SESS_VaccineLotListModal)
            Case ViewIndex.NewEditRecord
                ChangeViewTitle(lblNewEditRecordHeading)
            Case ViewIndex.Confirm
                ChangeViewTitle(lblNewRecordConfirmHeading)
            Case ViewIndex.MsgPage
        End Select
    End Sub


    Private Sub ChangeViewTitle(ByVal viewTitle As Label)

        Select Case ViewState("state")
            Case StateType.ADD
                viewTitle.Text = Me.GetGlobalResourceObject("Text", "VaccineLotRecordBatchAssign")
            Case StateType.BatchRemove
                viewTitle.Text = Me.GetGlobalResourceObject("Text", "VaccineLotRecordBatchRemove")
            Case Else
                viewTitle.Text = Me.GetGlobalResourceObject("Text", "VaccineLotRecord")
        End Select
    End Sub


    Private Sub gvConfirmDetail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvConfirmDetail.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblDetailBooth As Label = CType(e.Row.FindControl("lblVLDetailBooth"), Label)
            Dim lblDetailServicePeriodFrom As Label = CType(e.Row.FindControl("lblVLDetailEffFrom"), Label)
            Dim lblDetailServicePeriodTo As Label = CType(e.Row.FindControl("lblVLDetailEffTo"), Label)
            Dim lblDetailRecord_Status As Label = CType(e.Row.FindControl("lblVLDetailRecord_Status"), Label)
            Dim hfUseOfEpiryDtm As Label = CType(e.Row.FindControl("hfUseOfEpiryDtm"), Label)

            Dim lblNewDetailServicePeriodFrom As Label = CType(e.Row.FindControl("lblNewVLDetailEffFrom"), Label)
            Dim lblNewDetailServicePeriodTo As Label = CType(e.Row.FindControl("lblNewVLDetailEffTo"), Label)
            Dim lblNewDetailRecord_Status As Label = CType(e.Row.FindControl("lblNewVLDetailRecord_Status"), Label)


            'Booth
            If lblDetailBooth.Text.Trim.Equals(String.Empty) Then
                lblDetailBooth.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            'lblDetailServicePeriodFrom
            If lblDetailServicePeriodFrom.Text.Trim.Equals(String.Empty) Then
                lblDetailServicePeriodFrom.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            'lblDetailServicePeriodTo
            If lblDetailServicePeriodTo.Text.Trim.Equals(String.Empty) Then
                If hfUseOfEpiryDtm.Text = YesNo.Yes Then
                    lblDetailServicePeriodTo.Text = Me.GetGlobalResourceObject("Text", "UpToVacExpDate") + " " + lblConVaccineExpiryDateText.Text
                Else
                    lblDetailServicePeriodTo.Text = Me.GetGlobalResourceObject("Text", "N/A")
                End If
            Else

                Dim udtSystemMessage As SystemMessage = udtValidator.chkInputValidFromDateCutoffDate(FunctCode.FUNT990000, MsgCode.MSG00381, _
                 Date.Today().ToString("dd MMM yyyy"), lblDetailServicePeriodTo.Text)

                'servicedayTo < today =  cutoff day
                If (Not IsNothing(udtSystemMessage)) Then
                    lblDetailServicePeriodFrom.Text = Me.GetGlobalResourceObject("Text", "N/A")
                    lblDetailServicePeriodTo.Text = Me.GetGlobalResourceObject("Text", "N/A")
                End If
            End If

            'Record Original Status
            If lblDetailRecord_Status.Text.Trim.Equals(String.Empty) Then
                lblDetailRecord_Status.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                Dim strOrigEngStatusDesc As String = String.Empty
                Status.GetDescriptionFromDBCode(VaccineLotMappingRecordStatus.ClassCode, lblDetailRecord_Status.Text, strOrigEngStatusDesc, "", "")
                lblDetailRecord_Status.Text = strOrigEngStatusDesc
            End If


            'Select Case ViewState("state")
            '    Case StateType.ADD

            'set >> new record
            If Not lblDetailServicePeriodFrom.Text.Trim.Equals(lblConVaccineLotEffectiveDateFrom.Text.Trim) Then
                If ViewState("state") = StateType.ADD Or ViewState("state") = StateType.EDIT Then
                    lblNewDetailServicePeriodFrom.Text = " >> " + lblConVaccineLotEffectiveDateFrom.Text.Trim
                End If
            End If

            If Not lblDetailServicePeriodTo.Text.Trim.Equals(lblConVaccineLotEffectiveDateTo.Text.Trim) Then
                If ViewState("state") = StateType.ADD Or ViewState("state") = StateType.EDIT Then
                    lblNewDetailServicePeriodTo.Text = " >> " + lblConVaccineLotEffectiveDateTo.Text.Trim
                End If
            End If


            '    Case StateType.EDIT
            '        'set >> new record
            '        If Not lblDetailServicePeriodFrom.Text.Trim.Equals(lblConVaccineLotEffectiveDateTo.Text.Trim, String.Empty)) Then
            '            lblNewDetailServicePeriodFrom.Text = " >> " + udtFormatter.convertDate(txtVaccineLotEffectiveDateFrom.Text.Trim, String.Empty)
            '        End If

            '        If Not lblDetailServicePeriodTo.Text.Trim.Equals(udtFormatter.convertDate(txtVaccineLotEffectiveDateTo.Text.Trim, String.Empty)) Then
            '            lblNewDetailServicePeriodTo.Text = " >> " + IIf(txtVaccineLotEffectiveDateTo.Text.Trim.Equals(String.Empty), Me.GetGlobalResourceObject("Text", "UpToVacExpDate") + " " + lblConVaccineExpiryDateText.Text, udtFormatter.convertDate(txtVaccineLotEffectiveDateTo.Text.Trim, String.Empty))

            '        End If
            'End Select


            'Record New Status 
            Dim strNewEngStatusDesc As String = String.Empty
            Status.GetDescriptionFromDBCode("VaccineLotRequestType", "R", strNewEngStatusDesc, "", "")

            lblNewDetailRecord_Status.Text = " >> " + strNewEngStatusDesc
        End If
    End Sub


    Private Sub gvConfirmDetail_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvConfirmDetail.PageIndexChanging
        Me.GridViewPageIndexChangingHandler(sender, e, SESS_SearchResultList)
    End Sub

    Private Sub gvConfirmDetail_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvConfirmDetail.PreRender
        Me.GridViewPreRenderHandler(sender, e, SESS_SearchResultList)
    End Sub

    Private Sub gvConfirmDetail_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvConfirmDetail.Sorting
        Me.GridViewSortingHandler(sender, e, SESS_SearchResultList)
    End Sub


    Private Sub gvSummary_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSummary.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'Dim lblBrandName As Label = CType(e.Row.FindControl("lblSVLBrandName"), Label)
            Dim lblBrandTradeName As Label = CType(e.Row.FindControl("lblSVLBrandTradeName"), Label)
            Dim lblVLNo As LinkButton = CType(e.Row.FindControl("lnSkbtnVLNo"), LinkButton)
            Dim lblFromSymbol As Label = CType(e.Row.FindControl("lblSVLEffFromSymbol"), Label)
            Dim lblServicePeriodFrom As Label = CType(e.Row.FindControl("lblSVLEffFrom"), Label)
            Dim lblNewServicePeriodFrom As Label = CType(e.Row.FindControl("lblSVLNewEffFrom"), Label)
            Dim lblToSymbol As Label = CType(e.Row.FindControl("lblSVLEffToSymbol"), Label)
            Dim lblServicePeriodTo As Label = CType(e.Row.FindControl("lblSVLEffTo"), Label)
            Dim lblNewServicePeriodTo As Label = CType(e.Row.FindControl("lblSVLNewEffTo"), Label)
            Dim lblRequestType As Label = CType(e.Row.FindControl("lblSRequestType"), Label)
            Dim lblRecordStatus As Label = CType(e.Row.FindControl("lblSVLStatus"), Label)
            Dim lblSVLCreateBy As Label = CType(e.Row.FindControl("lblSVLCreateBy"), Label)
            Dim lblSVLRequestedBy As Label = CType(e.Row.FindControl("lblSVLRequestedBy"), Label)
            Dim lblVLDRecordStatus As Label = CType(e.Row.FindControl("lblLotDetailRecordStatus"), Label)


            'Brand
            If lblBrandTradeName.Text = String.Empty Then
                lblBrandTradeName.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            'Vaccine lot no
            If lblVLNo.Text = String.Empty Then
                lblVLNo.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lblVLNo.Enabled = False
            ElseIf lblRecordStatus.Text = VaccineLotMappingRecordStatus.Pending Then
                lblVLNo.Enabled = False
            End If



            'Service_Period_From
            If lblRecordStatus.Text = VaccineLotMappingRecordStatus.Pending And lblNewServicePeriodFrom.Text.Trim <> lblServicePeriodFrom.Text.Trim Then
                lblFromSymbol.Visible = True
                lblNewServicePeriodFrom.Visible = True

            Else
                lblFromSymbol.Visible = False
                lblNewServicePeriodFrom.Visible = False

            End If

            If lblServicePeriodFrom.Text = String.Empty Then
                lblServicePeriodFrom.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            'Service_Period_To
            If lblRecordStatus.Text = VaccineLotMappingRecordStatus.Pending And lblNewServicePeriodTo.Text.Trim <> lblServicePeriodTo.Text.Trim Then
                lblToSymbol.Visible = True
                lblNewServicePeriodTo.Visible = True
            Else
                lblToSymbol.Visible = False
                lblNewServicePeriodTo.Visible = False
            End If

            If lblServicePeriodTo.Text = String.Empty Then
                lblServicePeriodTo.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If


            'Record Status 
            If lblRecordStatus.Text = String.Empty Then
                lblRecordStatus.Text = "No Active Record"  ' check later
            Else
                Status.GetDescriptionFromDBCode(VaccineLotMappingRecordStatus.ClassCode, lblRecordStatus.Text.Trim, lblRecordStatus.Text, String.Empty)
            End If

            'Request Type
            If lblRequestType.Text = String.Empty Then
                lblRequestType.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                Status.GetDescriptionFromDBCode(VaccineLotRequestStatus.RequstTypeClassCode, lblRequestType.Text.Trim, lblRequestType.Text, String.Empty)
                If lblRequestType.Text = "Remove" Then
                    lblToSymbol.Visible = False
                    lblFromSymbol.Visible = False
                    lblNewServicePeriodTo.Visible = False
                End If
                lblRecordStatus.Style.Add("color", "red")
            End If



            'Requested by
            If lblSVLRequestedBy.Text = String.Empty Then
                lblSVLRequestedBy.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            'created by
            If lblSVLCreateBy.Text = String.Empty Then
                lblSVLCreateBy.Text = lblSVLRequestedBy.Text
            End If
        End If
    End Sub

    Private Sub gvSummary_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSummary.PageIndexChanging
        Me.GridViewPageIndexChangingHandler(sender, e, SESS_VaccineLotSummary)
    End Sub

    Private Sub gvSummary_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSummary.PreRender
        Me.GridViewPreRenderHandler(sender, e, SESS_VaccineLotSummary)
    End Sub

    Private Sub gvSummary_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSummary.RowCommand
        If TypeOf e.CommandSource Is LinkButton Then
            Dim r As GridViewRow = CType(e.CommandSource, LinkButton).NamingContainer
            Dim strLotID As String = CType(r.FindControl("lnSkbtnVLNo"), LinkButton).CommandArgument.Trim

            BindVaccineInfoView(strLotID)
        End If
    End Sub

    Private Sub gvSummary_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvSummary.Sorting
        Me.GridViewSortingHandler(sender, e, SESS_VaccineLotSummary)
    End Sub

    Protected Sub BindVaccineInfoView(ByVal strVLID As String)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Lot Id", strVLID)
        udtAuditLogEntry.WriteLog(LogID.LOG00023, AuditMsg00023)

        'Initialize
        lblENewConVaccineLotEffectiveDateFrom.Text = ""
        lblENewConVaccineLotEffectiveDateTo.Text = ""
        'lblENewConVaccineLotStatus.Text = ""
        trinfoRequestedBy.Visible = False
        trinfoRequestType.Visible = False
        trinfoApprovedBy.Visible = False

        Dim udtVaccineLotList As VaccineLotModel = Nothing
        hfEVaccineLotId.Text = strVLID
        udtVaccineLotList = udtVaccineLotBLL.GetVaccineLotModalByVaccineLotId(strVLID, VaccineLotRecordServiceType.Centre)

        ChangeViewIndex(ViewIndex.EditRecordView)

        lblEConVaccineCentre.Text = udtVaccineLotList.CentreName
        hflblEConVaccineCentreId.Text = udtVaccineLotList.CentreId
        lblEConVaccineBooth.Text = udtVaccineLotList.Booth
        hflEConVaccineBoothId.Text = udtVaccineLotList.BoothId
        lblEConVaccineBrandName.Text = udtVaccineLotList.BrandTradeName
        lblEConVaccineLotNo.Text = udtVaccineLotList.VaccineLotNo
        lblEConVaccineExpiryDateText.Text = udtFormatter.formatDisplayDate(CDate(udtVaccineLotList.VaccineExpiryDate))
        lblEConVaccineLotEffectiveDateFrom.Text = udtFormatter.formatDisplayDate(CDate(udtVaccineLotList.VaccineLotEffectiveDFrom))

        If udtVaccineLotList.UpToExpiryDtm = YesNo.Yes Then
            lblEConVaccineLotEffectiveDateTo.Text = Me.GetGlobalResourceObject("Text", "UpToVacExpDate") + " " + udtFormatter.formatDisplayDate(CDate(udtVaccineLotList.VaccineExpiryDate))
        Else
            lblEConVaccineLotEffectiveDateTo.Text = udtFormatter.formatDisplayDate(CDate(udtVaccineLotList.VaccineLotEffectiveDTo))
        End If

        lblEConVaccineNewRecordStatus.Text = udtVaccineLotList.RecordStatus
        Status.GetDescriptionFromDBCode(VaccineLotMappingRecordStatus.ClassCode, lblEConVaccineNewRecordStatus.Text, lblEConVaccineNewRecordStatus.Text, String.Empty)
        lblCreatedBy.Text = udtVaccineLotList.CreateBy + " (" + Format(CDate(udtVaccineLotList.CreateDtm), "dd MMM yyyy HH:mm") + ")"

        'Pending record
        If udtVaccineLotList.RecordStatus = VaccineLotMappingRecordStatus.Pending Then
            trinfoRequestedBy.Visible = True

            If udtVaccineLotList.RequestType <> String.Empty Then
                trinfoRequestType.Visible = True
                lblERequestType.Text = udtVaccineLotList.RequestType
                Status.GetDescriptionFromDBCode(VaccineLotRequestStatus.RequstTypeClassCode, lblERequestType.Text, lblERequestType.Text, String.Empty)
            End If

            lblERequestedBy.Text = udtVaccineLotList.RequestBy + " (" + Format(CDate(udtVaccineLotList.RequestDtm), "dd MMM yyyy HH:mm") + ")"

            ibtnEditRecordEdit.Enabled = False
            ibtnEditRecordEdit.ImageUrl = GetGlobalResourceObject("ImageUrl", "EditDisableBtn")
            ibtnEditRecordRemove.Enabled = False
            ibtnEditRecordRemove.ImageUrl = GetGlobalResourceObject("ImageUrl", "RemoveDisableBtn")

        Else
            trinfoApprovedBy.Visible = True
            If udtVaccineLotList.ApproveBy IsNot String.Empty Then
                lblEApprovedBy.Text = udtVaccineLotList.ApproveBy + " (" + Format(CDate(udtVaccineLotList.ApproveDtm), "dd MMM yyyy HH:mm") + ")"
            Else
                lblEApprovedBy.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            ibtnEditRecordEdit.Enabled = True
            ibtnEditRecordEdit.ImageUrl = GetGlobalResourceObject("ImageUrl", "EditBtn")
            ibtnEditRecordRemove.Enabled = True
            ibtnEditRecordRemove.ImageUrl = GetGlobalResourceObject("ImageUrl", "RemoveBtn")

        End If
    End Sub



#Region "Validation"
    Private Function SearchValidation() As Boolean
        Dim blnValid As Boolean = True

        If msgBox.GetCodeTable.Rows.Count = 0 Then
            msgBox.Visible = False
        Else
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            msgBox.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00026, "Search failed")
        End If

        Return blnValid
    End Function
    Private Function RemoveRecordInfoValidation(ByVal strCentreId As String, ByVal strBoothList As String, ByVal strVaccLotNo As String) As Boolean
        Dim blnValid As Boolean = True
        Dim strRequestPendingBoothList As String = Nothing
        Dim strNonExistRecordBoothList As String = Nothing

        'VaccineLotSummaryDataTable = udtVaccineLotBLL.GetVaccineLotListSummaryByAny(strCentreId, strBoothList, String.Empty, strVaccLotNo)

        'check active record
        strNonExistRecordBoothList = ckNonExistRecord(strCentreId, strBoothList, strVaccLotNo)

        If Not strNonExistRecordBoothList Is Nothing Then
            msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00005), "%s", strNonExistRecordBoothList) ' change later 
            blnValid = False
        End If


        'Check any other pending approve
        If blnValid Then

            strRequestPendingBoothList = ckPendingRequestRecordExist(strCentreId, strBoothList, strVaccLotNo)

            If Not strRequestPendingBoothList Is Nothing Then
                msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00004), "%s", strRequestPendingBoothList)
                blnValid = False
            End If
        End If

        Return blnValid

    End Function

    Private Function SaveRecordInfoValidation() As Boolean
        Dim blnValid As Boolean = True
        Dim blnValidDate As Boolean = True
        Dim strBoothId As String = String.Empty
        Dim udtSystemMessage As SystemMessage
        imgVaccineCentreErr.Visible = False
        imgVaccineBrandNameErr.Visible = False
        imgVaccineLotNoErr.Visible = False
        imgVaccineExpiryDateTextErr.Visible = False
        imgVaccineLotEffectiveDateFromErr.Visible = False
        imgVaccineLotEffectiveDateToErr.Visible = False

        If ViewState("state") = StateType.ADD Then
            'Check Mandatory Fields
            If ddlVaccineCentre.SelectedValue = String.Empty Then
                msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00367), "%s", GetGlobalResourceObject("Text", "VaccineCentre"))
                imgVaccineCentreErr.Visible = True
                blnValid = False
            End If


            'remove??
            If lblVaccineExpiryDateText.Text.Trim = "N/A" Then
                'msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00367), "%s", GetGlobalResourceObject("Text", "VaccineExpiryDate"))
                'imgVaccineExpiryDateTextErr.Visible = True
                blnValid = False
            End If

        End If
        If ViewState("state") = StateType.ADD Or ViewState("state") = StateType.BatchRemove Then
            If lbVaccineCentreBooth.SelectedValue = String.Empty Then
                msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00367), "%s", GetGlobalResourceObject("Text", "BoothOutreach"))
                imgVaccineCentreBoothErr.Visible = True
                blnValid = False
            End If
            If ddlVaccineBrandName.Text.Trim = String.Empty Then
                msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00367), "%s", GetGlobalResourceObject("Text", "VaccineName"))
                imgVaccineBrandNameErr.Visible = True
                blnValid = False
            Else
                If ViewState("state") = StateType.ADD Or ViewState("state") = StateType.BatchRemove Then
                    If (Not lbVaccineCentreBooth.SelectedValue = String.Empty) Then
                        For i As Integer = 0 To lbVaccineCentreBooth.Items.Count - 1
                            If (lbVaccineCentreBooth.Items(i).Selected) Then
                                strBoothId += lbVaccineCentreBooth.Items(i).Value.Trim
                                strBoothId += ","
                            End If
                        Next
                        strBoothId = strBoothId.Remove(strBoothId.Length - 1)
                    End If
                End If
            End If
            If ddlVaccineLotNo.Text.Trim = String.Empty Then
                msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00367), "%s", GetGlobalResourceObject("Text", "VaccineLotNo"))
                imgVaccineLotNoErr.Visible = True
                blnValid = False
            End If
        End If




        ' Check Date
        If (ViewState("state") = StateType.ADD Or ViewState("state") = StateType.EDIT) Then

            ' 1: Check completeness
            Dim CheckDateCompletenessFrom As Boolean = CheckDateCompleteness(txtVaccineLotEffectiveDateFrom, lblVaccineLotEffectiveDateFrom, imgVaccineLotEffectiveDateFromErr, False)
            Dim CheckDateCompletenessTo As Boolean = CheckDateCompleteness(txtVaccineLotEffectiveDateTo, lblVaccineLotEffectiveDateTo, imgVaccineLotEffectiveDateToErr, chkUpToExpiryDate.Checked)
            blnValidDate = IIf(CheckDateCompletenessFrom And CheckDateCompletenessTo, True, False)

            ' 2: Check the date format
            If blnValidDate Then
                Dim CheckAndConvertDateFormatFrom As Boolean = CheckAndConvertDateFormat(txtVaccineLotEffectiveDateFrom, lblVaccineLotEffectiveDateFrom, imgVaccineLotEffectiveDateFromErr, False)
                Dim CheckAndConvertDateFormatTo As Boolean = CheckAndConvertDateFormat(txtVaccineLotEffectiveDateTo, lblVaccineLotEffectiveDateTo, imgVaccineLotEffectiveDateToErr, chkUpToExpiryDate.Checked)
                blnValidDate = IIf(CheckAndConvertDateFormatFrom And CheckAndConvertDateFormatTo, True, False)
            End If

            '2a: Check the date is not a past date
            If blnValidDate Then
                'check EffectiveTo past date only
                blnValidDate = CheckPastDate(txtVaccineLotEffectiveDateTo, lblVaccineLotEffectiveDateTo, imgVaccineLotEffectiveDateToErr, chkUpToExpiryDate.Checked)
            End If

            ' 3: Check date dependency: From < To
            If blnValidDate And Not chkUpToExpiryDate.Checked Then
                udtSystemMessage = udtValidator.chkInputValidFromDateCutoffDate(FunctCode.FUNT990000, MsgCode.MSG00381, _
                        udtFormatter.convertDate(txtVaccineLotEffectiveDateFrom.Text.Trim, String.Empty), udtFormatter.convertDate(txtVaccineLotEffectiveDateTo.Text.Trim, String.Empty))

                ' The From Date should not be later than the To Date in "Date".
                If Not IsNothing(udtSystemMessage) Then
                    imgVaccineLotEffectiveDateFromErr.Visible = True
                    imgVaccineLotEffectiveDateToErr.Visible = True
                    blnValidDate = False
                    msgBox.AddMessage(udtSystemMessage, {"%t", "%s"}, {GetGlobalResourceObject("Text", "VaccineLotEffectiveDateFrom"), GetGlobalResourceObject("Text", "VaccineLotEffectiveDateTo")})
                    'msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00025), {"%t", "%s"}, {GetGlobalResourceObject("Text", "VaccineLotEffectiveDateFrom"), GetGlobalResourceObject("Text", "VaccineLotEffectiveDateTo")})
                End If
            End If

            'Check Effective date from & to with expiry date 
            If blnValidDate AndAlso blnValid Then
                Dim CheckEffectiveDateWithExpiryDateFrom As Boolean = CheckEffectiveDateWithExpiryDate(txtVaccineLotEffectiveDateFrom, lblVaccineLotEffectiveDateFrom, imgVaccineLotEffectiveDateFromErr, False)
                Dim CheckEffectiveDateWithExpiryDateTo As Boolean = CheckEffectiveDateWithExpiryDate(txtVaccineLotEffectiveDateTo, lblVaccineLotEffectiveDateTo, imgVaccineLotEffectiveDateToErr, chkUpToExpiryDate.Checked)
                blnValidDate = IIf(CheckEffectiveDateWithExpiryDateFrom And CheckEffectiveDateWithExpiryDateTo, True, False)
            End If

            If blnValidDate = False Then
                blnValid = blnValidDate
            End If



            'Check any duplicated record
            If blnValid Then
                ' Dim VaccineLotSummaryDataTable As DataTable = Nothing
                Dim strDuplicateDateBoothList As String = Nothing
                Dim chkeffectiveDateTo As String = txtVaccineLotEffectiveDateTo.Text.Trim
                Dim formatedExpiryDate As String = Format(DateTime.ParseExact(lblVaccineExpiryDateText.Text.Trim, udtFormatter.DisplayDateFormat, Nothing), udtFormatter.EnterDateFormat)

                If (chkUpToExpiryDate.Checked) Then
                    chkeffectiveDateTo = formatedExpiryDate
                End If

                If ViewState("state") = StateType.ADD Then
                    strDuplicateDateBoothList = ckDuplicatedEffectiveDate(ddlVaccineCentre.SelectedValue.ToString, strBoothId, ddlVaccineLotNo.SelectedValue.ToString, txtVaccineLotEffectiveDateFrom.Text.Trim, chkeffectiveDateTo)
                ElseIf ViewState("state") = StateType.EDIT Then
                    'strDuplicateDateBoothList = ckDuplicatedEffectiveDate(hfPERConVaccineCentreID.Text, hfPERConVaccineBoothId.Text, lblPERConVaccineLotNo.Text, txtVaccineLotEffectiveDateFrom.Text.Trim, chkeffectiveDateTo)

                    If hfVaccineLotEffectiveDateFrom.Text = txtVaccineLotEffectiveDateFrom.Text.Trim Then
                        'check System effective date = inputted date and if orginal data = up to expiry date then inputted data cannot be expiry date
                        If (hfVaccineLotEffectiveDateTo.Text = txtVaccineLotEffectiveDateTo.Text.Trim Or _
                            (hfVaccineLotEffectiveDateTo.Text = String.Empty AndAlso txtVaccineLotEffectiveDateTo.Text.Trim = formatedExpiryDate) Or _
                            (hfVaccineLotEffectiveDateTo.Text = formatedExpiryDate AndAlso chkUpToExpiryDate.Checked)) Then
                            strDuplicateDateBoothList = lblPERConVaccineBooth.Text.Trim
                        End If
                    End If
                End If

                If Not strDuplicateDateBoothList Is Nothing Then
                    msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00008), {"%s", "%ef", "%et"}, {strDuplicateDateBoothList, GetGlobalResourceObject("Text", "VaccineLotEffectiveDateFrom"), GetGlobalResourceObject("Text", "VaccineLotEffectiveDateTo")})
                    blnValid = False
                End If
            End If


            'Check any other pending approve
            If blnValid Then
                ' Dim VaccineLotSummaryDataTable As DataTable = Nothing
                Dim strRequestPendingBoothList As String = Nothing
                If ViewState("state") = StateType.ADD Then
                    ' VaccineLotSummaryDataTable = udtVaccineLotBLL.GetVaccineLotListSummaryByAny(ddlVaccineCentre.SelectedValue.ToString, strBoothId, String.Empty, ddlVaccineLotNo.SelectedValue.ToString)
                    strRequestPendingBoothList = ckPendingRequestRecordExist(ddlVaccineCentre.SelectedValue.ToString, strBoothId, ddlVaccineLotNo.SelectedValue.ToString)
                ElseIf ViewState("state") = StateType.EDIT Then
                    'VaccineLotSummaryDataTable = udtVaccineLotBLL.GetVaccineLotListSummaryByAny(hfPERConVaccineCentreID.Text, lblPERConVaccineBooth.Text, String.Empty, lblPERConVaccineLotNo.Text)
                    strRequestPendingBoothList = ckPendingRequestRecordExist(hfPERConVaccineCentreID.Text, hfPERConVaccineBoothId.Text, lblPERConVaccineLotNo.Text)
                End If



                If Not strRequestPendingBoothList Is Nothing Then
                    msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00004), "%s", strRequestPendingBoothList)
                    blnValid = False
                End If
            End If





            If ViewState("state") = StateType.EDIT And blnValid Then
                If hfVaccineLotEffectiveDateFrom.Text = txtVaccineLotEffectiveDateFrom.Text.Trim AndAlso _
                    hfVaccineLotEffectiveDateTo.Text = txtVaccineLotEffectiveDateTo.Text.Trim Then

                    msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00003))
                    blnValid = False

                End If
            End If
        End If


        'The validation for Batch Remove
        If ViewState("state") = StateType.BatchRemove Then
            If blnValid Then
                If RemoveRecordInfoValidation(ddlVaccineCentre.SelectedValue.ToString, strBoothId, ddlVaccineLotNo.SelectedValue.ToString) = False Then
                    blnValid = False
                End If
            End If
        End If


        If msgBox.GetCodeTable.Rows.Count = 0 Then
            msgBox.Visible = False
        Else
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00038, AuditMsg00038)
        End If

        Return blnValid
    End Function

    Private Function CheckDateCompleteness(ByRef DateText As TextBox, ByRef lblErrMsg As Label, ByRef ErrImage As Image, ByRef UpToExpiryDate As Boolean) As Boolean
        If (UpToExpiryDate) Then
            Return True
        Else
            If (DateText.Text.Trim = String.Empty) Then
                ' Please complete "Date". 
                msgBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00364, "%s", lblErrMsg.Text.Trim)
                ErrImage.Visible = True
                Return False
            End If

            Return True
        End If
    End Function


    '2: Check the date format
    Private Function CheckAndConvertDateFormat(ByRef DateText As TextBox, ByRef lblErrMsg As Label, ByRef ErrImage As Image, ByRef UpToExpiryDate As Boolean) As Boolean
        If (UpToExpiryDate) Then
            Return True
        Else
            Dim strDateFrom As String = IIf(udtFormatter.formatInputDate(DateText.Text.Trim) <> String.Empty, udtFormatter.formatInputDate(DateText.Text.Trim), DateText.Text.Trim)
            ' Format the input date (Date From / To)
            Dim udtSystemMessage As SystemMessage = udtValidator.chkInputDate(strDateFrom, True, False)
            If Not IsNothing(udtSystemMessage) Then
                msgBox.AddMessage(udtSystemMessage, "%s", lblErrMsg.Text.Trim)
                ErrImage.Visible = True
                Return False
            End If

            DateText.Text = strDateFrom
            Return True
        End If
    End Function

    '2a: Check the date is not a past date
    Private Function CheckPastDate(ByRef DateText As TextBox, ByRef lblErrMsg As Label, ByRef ErrImage As Image, ByRef UpToExpiryDate As Boolean) As Boolean
        If (UpToExpiryDate) Then
            Return True
        Else
            Dim dtmDate As DateTime = DateTime.ParseExact(DateText.Text.Trim, udtFormatter.EnterDateFormat, Nothing)
            If dtmDate < DateTime.Today.Date Then
                msgBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00006) 'System Effective To Date" cannot be a past date. (only to date)
                ErrImage.Visible = True
                Return False
            End If

            Return True
        End If
    End Function

    'Check Effective date from & to with expiry date 
    Private Function CheckEffectiveDateWithExpiryDate(ByRef DateText As TextBox, ByRef lblErrMsg As Label, ByRef ErrImage As Image, ByRef UpToExpiryDate As Boolean) As Boolean
        If (UpToExpiryDate) Then
            Return True
        Else
            Dim udtSystemMessage As SystemMessage = udtValidator.chkInputValidFromDateCutoffDate(FunctCode.FUNT990000, MsgCode.MSG00381, _
            udtFormatter.convertDate(DateText.Text.Trim, String.Empty), lblVaccineExpiryDateText.Text.Trim)

            ' The From Date should not be later than the To Date in "Date".
            If Not IsNothing(udtSystemMessage) Then
                msgBox.AddMessage(udtSystemMessage, {"%t", "%s"}, {lblErrMsg.Text.Trim, GetGlobalResourceObject("Text", "VaccineExpiryDate")})
                ErrImage.Visible = True
                Return False
            End If

            Return True
        End If
    End Function

    Private Function ckPendingRequestRecordExist(ByVal strCentreId As String, ByVal strBoothList As String, ByVal strVaccLotNo As String) As String
        Dim boothDisplayItemList As StaticDataModelCollection = (New StaticDataBLL).GetStaticDataListByColumnName(BoothClassCode)
        Dim strRequestPendingBoothList As String = Nothing
        Dim dtVaccineLotPendingRequest As DataTable = Nothing

        dtVaccineLotPendingRequest = udtVaccineLotBLL.GetVaccineLotPendingRequestList(strCentreId, strBoothList, strVaccLotNo, VaccineLotRecordServiceType.Centre, "")

        For Each row As DataRow In dtVaccineLotPendingRequest.Rows
            Dim strBoothDisplayItem As String = boothDisplayItemList.Item(BoothClassCode, CType(row.Item("Booth"), String).Trim).DataValue
            strRequestPendingBoothList += strBoothDisplayItem & ","
        Next

        If Not strRequestPendingBoothList Is Nothing Then
            strRequestPendingBoothList = strRequestPendingBoothList.Substring(0, strRequestPendingBoothList.Length - 1)
        End If
        Return strRequestPendingBoothList
    End Function

    Private Function ckNonExistRecord(ByVal strCentreId As String, ByVal strBoothList As String, ByVal strVaccLotNo As String) As String
        Dim strNonExistBoothList As String = Nothing
        Dim nonExistBoothList As List(Of String) = New List(Of String)
        Dim blnExist As Boolean = False
        Dim dtVaccineLotExistLot As DataTable = Nothing
        Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL
        Dim boothDisplayItemList As StaticDataModelCollection = udtStaticDataBLL.GetStaticDataListByColumnName(BoothClassCode)

        dtVaccineLotExistLot = udtVaccineLotBLL.GetVaccineLotExistVaccineLot(strCentreId, strBoothList, strVaccLotNo)

        For Each booth As String In strBoothList.Split(",")
            blnExist = False
            For Each row As DataRow In dtVaccineLotExistLot.Rows
                If CType(row.Item("Booth"), String).Trim = booth Then
                    blnExist = True
                    Exit For
                End If
            Next

            If blnExist = False Then
                Dim boothDisplayItem As StaticDataModel = boothDisplayItemList.Item(BoothClassCode, booth)
                If Not nonExistBoothList.Contains(boothDisplayItem.DataValue) Then
                    nonExistBoothList.Add(boothDisplayItem.DataValue)
                End If
            End If
        Next

        If nonExistBoothList.Count > 0 Then
            If (nonExistBoothList.Count > 1) Then
                strNonExistBoothList = String.Join(",", nonExistBoothList.ToArray())
            Else
                strNonExistBoothList = nonExistBoothList(0)
            End If

        End If
        Return strNonExistBoothList
        'Dim blnExist As Boolean = False

        'For Each booth As String In BoothList.Split(",")
        '    blnExist = False
        '    For Each row As DataRow In VaccineLotSummaryDataTable.Rows
        '        If CType(row.Item("Booth"), String).Trim = booth And row.Item("Display_Record_Status").trim = RecordStatus.Active Then
        '            blnExist = True
        '            Exit For
        '        End If
        '    Next

        '    If blnExist = False Then
        '        strNonExistBoothList += strNonExistBoothList & booth & ","
        '    End If
        'Next

        'If Not strNonExistBoothList Is Nothing Then
        '    strNonExistBoothList = strNonExistBoothList.Substring(0, strNonExistBoothList.Length - 1)
        'End If
        'Return strNonExistBoothList
    End Function



    Private Function ckDuplicatedEffectiveDate(ByVal strCentreId As String, ByVal strBoothList As String, ByVal strVaccLotNo As String, ByVal strEffectiveFrom As String, ByVal strEffectiveTo As String) As String
        Dim boothDisplayItemList As StaticDataModelCollection = (New StaticDataBLL).GetStaticDataListByColumnName(BoothClassCode)
        Dim strDuplicatedEffectiveDateBoothList As String = Nothing

        Dim chkEffectiveFrom As String = Format(DateTime.ParseExact(strEffectiveFrom, udtFormatter.EnterDateFormat, Nothing), "MM/dd/yyyy")
        Dim chkEffectiveTo As String = Format(DateTime.ParseExact(strEffectiveTo, udtFormatter.EnterDateFormat, Nothing), "MM/dd/yyyy")

        'save to session for confirmPage Display
        Dim dtConfirmLotDetail As DataTable = udtVaccineLotBLL.GetVaccineLotListDetailConfirm(strCentreId, strBoothList, strVaccLotNo)
        Session(SESS_Confirm_HistoryView) = dtConfirmLotDetail

        Dim dvDuplicatedEffectiveDate As DataView = New DataView(dtConfirmLotDetail)



        dvDuplicatedEffectiveDate.RowFilter = "[Service_Period_From] = '" + chkEffectiveFrom + "' and [Service_Period_To_WithExpiryDate] = '" + chkEffectiveTo + "'"
        For Each row As DataRowView In dvDuplicatedEffectiveDate
            Dim strBoothDisplayItem As String = boothDisplayItemList.Item(BoothClassCode, CType(row.Item("Booth_id"), String).Trim).DataValue
            strDuplicatedEffectiveDateBoothList += strBoothDisplayItem & ","
        Next

        If Not strDuplicatedEffectiveDateBoothList Is Nothing Then
            strDuplicatedEffectiveDateBoothList = strDuplicatedEffectiveDateBoothList.Substring(0, strDuplicatedEffectiveDateBoothList.Length - 1)
        End If
        Return strDuplicatedEffectiveDateBoothList
    End Function

#End Region

End Class