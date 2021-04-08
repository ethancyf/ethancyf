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

Partial Public Class VaccineLotCreation
    Inherits BasePageWithControl


    Public Enum StateType
        LOADED = 0
        EDIT = 1
        ADD = 2
    End Enum

    Public Enum ActionType
        Create = 0
        Update = 1
        Active = 2
        Deactive = 3
        Remove = 4
        Cancel = 5
    End Enum

    Public Enum ViewIndex
        SearchCriteria = 0
        SearchResult = 1
        Confirm = 2
        MsgPage = 3
        ErrorPage = 4
        NewEditRecord = 5
        EditRecordView = 6

    End Enum


#Region "Fields"

    Private udtHCVUUserBLL As New HCVUUserBLL
    Private udtVaccineLotBLL As New VaccineLotBLL

    Private udtFormatter As New Formatter
    Private udtValidator As New Validator
    Private udtGeneralFunction As New GeneralFunction

    Private SM As SystemMessage
    Private udtAuditLogEntry As AuditLogEntry

    Private Const SESS_SearchResultList As String = FunctCode.FUNT010424 & "VaccineLotCreationList_SearchResultList"
    Private Const SESS_VaccineLotListModal As String = FunctCode.FUNT010424 & "VaccineLotcreationList_Modal"



#End Region

#Region "Audit Log Message"
    Private Const AuditMsg00000 As String = "Vaccine Lot Creation loaded"
    Private Const AuditMsg00001 As String = "Search"
    Private Const AuditMsg00002 As String = "Search completed. No record found"
    Private Const AuditMsg00003 As String = "Search completed"
    Private Const AuditMsg00004 As String = "Search failed"
    Private Const AuditMsg00005 As String = "Search Result Page -  Click View Detail"
    Private Const AuditMsg00006 As String = "Search Result Page -  Back Click"
    Private Const AuditMsg00007 As String = "Detail Page - Back Click"
    Private Const AuditMsg00008 As String = "Cancel Click"
    Private Const AuditMsg00009 As String = "Confirm Cancel - Yes Click"
    Private Const AuditMsg00010 As String = "Confirm Cancel - No Click"
    Private Const AuditMsg00011 As String = "Cancel Vaccine Lot Creation successful"
    Private Const AuditMsg00012 As String = "Cancel Vaccine Lot Creation failed"
    Private Const AuditMsg00013 As String = "Remove Click"
    Private Const AuditMsg00014 As String = "Confirm Remove - Yes Click"
    Private Const AuditMsg00015 As String = "Confirm Remove - No Click"
    Private Const AuditMsg00016 As String = "Remove Vaccine Lot Creation successful"
    Private Const AuditMsg00017 As String = "Remove Vaccine Lot Creation failed"
    Private Const AuditMsg00018 As String = "New Click"
    Private Const AuditMsg00019 As String = "New Record Page - Save Click"
    Private Const AuditMsg00020 As String = "New Record Page - Back Click"
    Private Const AuditMsg00021 As String = "Confirm Page - Confirm Click"
    Private Const AuditMsg00022 As String = "Confirm Page - Back Click"
    Private Const AuditMsg00023 As String = "New Vaccine Lot Creation successful"
    Private Const AuditMsg00024 As String = "New Vaccine Lot Creation failed"
    Private Const AuditMsg00025 As String = "Completion Page - Return Click"
    Private Const AuditMsg00026 As String = "Validation failed"
#End Region

#Region "Page Events"

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
        'Return bllSearchResult
        Dim bllSearchResult As Common.Component.BaseBLL.BLLSearchResult
        Dim dtToday As DateTime = udtGeneralFunction.GetSystemDateTime
        ' Dim dtExpiryFrom As Date = dtToday
        '  Dim dtExpiryFrom As DateTime = Convert.ToDateTime(txtExpiryFrom.Text.Trim)
        ' Dim dtExpiryTo As DateTime = Convert.ToDateTime(txtExpiryFrom.Text.Trim)


        bllSearchResult = udtVaccineLotBLL.GetVaccineLotCreationSearch(ddlBrand.SelectedValue.Trim, _
                                                                txtVaccLotNo.Text.Trim.ToUpper,
                                                                txtExpiryFrom.Text.Trim, _
                                                                txtExpiryTo.Text.Trim, _
                                                                ddlNewRecordStatus.SelectedValue.Trim)

        Return bllSearchResult
    End Function

    Protected Overrides Function SF_BindSearchResult(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry, ByVal udtBLLSearchResult As Common.Component.BaseBLL.BLLSearchResult) As Integer
        Dim dt As DataTable
        Dim intRowCount As Integer
        Dim blnShowResultList As Boolean = False

        Try
            dt = CType(udtBLLSearchResult.Data, DataTable)
        Catch ex As Exception
            Throw
        End Try

        intRowCount = dt.Rows.Count

        Select Case dt.Rows.Count
            Case 0
                ' No record found
                blnShowResultList = False
                udtAuditLogEntry.WriteEndLog(LogID.LOG00002, AuditMsg00002)

            Case Else
                blnShowResultList = True

        End Select

        If blnShowResultList Then
            FillSearchCriteria()
            gvResult.DataSource = dt
            gvResult.DataBind()

            Session(SESS_SearchResultList) = dt

            Me.GridViewDataBind(gvResult, dt, "Vaccine_Lot_No", "ASC", False)

            ChangeViewIndex(ViewIndex.SearchResult)

            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditMsg00003)

        End If

        Return intRowCount
    End Function

    Protected Overrides Sub SF_FinalizeSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry)
    End Sub

    Protected Overrides Sub SF_ConfirmSearch_Click()
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim enumSearchResult As SearchResultEnum

        udtAuditLogEntry.WriteStartLog(LogID.LOG00001, AuditMsg00001)

        Try
            enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, msgBox, udcInfoBox, False, True)

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                msgBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, eSQL.Message))

                If msgBox.GetCodeTable.Rows.Count = 0 Then
                    msgBox.Visible = False
                Else
                    msgBox.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00004, AuditMsg00004)
                End If

            Else
                Throw eSQL
            End If

        Catch ex As Exception
            Throw ex
        End Try

        Select Case enumSearchResult
            Case SearchResultEnum.Success
                udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditMsg00003)

            Case Else
                Throw New Exception("Error: Class = [HCVU.Vaccine Lot Approval], Method = [SF_ConfirmSearch_Click], Message = The unexpected error for the result of re-search")

        End Select
    End Sub

    Protected Overrides Sub SF_CancelSearch_Click()
    End Sub

#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        Dim dtToday As DateTime = udtGeneralFunction.GetSystemDateTime
        Dim dtExpiryFrom As Date = dtToday
        Dim dtExpiryTo As Date = dtToday

        Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL
        Dim strClaimDayLimit As String = String.Empty

        If Not Page.IsPostBack Then
            FunctionCode = FunctCode.FUNT010424

            ' Write Audit Log
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, AuditMsg00000)

            'Bind 
            BuildBrand(ddlBrand, True)
            BuildVaccineLotRecordStatus(ddlNewRecordStatus)


            'txtExpiryFrom.Text = Format(dtExpiryFrom, "dd-MM-yyyy")
            'txtExpiryTo.Text = Format(dtExpiryTo, "dd-MM-yyyy")
 
            CalendarExtender3.StartDate = DateAdd(DateInterval.Day, 0, Today)
            ResetAllHiddenField()
        End If

        ' Me.ModalPopupConfirmCancel.PopupDragHandleControlID = Me.ucNoticePopUpConfirm.Header.ClientID
        'Me.ModalPopupConfirmActivate.PopupDragHandleControlID = Me.ucNoticePopUpConfirmActivate.Header.ClientID
        'Me.ModalPopupConfirmDeactivate.PopupDragHandleControlID = Me.ucNoticePopUpConfirmDeactivate.Header.ClientID
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If MultiViewEnquiry.ActiveViewIndex = ViewIndex.SearchCriteria Then
            pnlEnquiry.DefaultButton = ibtnSearch.ID
        End If

    End Sub

#End Region

    Protected Sub ibtnSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcInfoBox.Visible = False
        msgBox.Visible = False
        'imgRCHCodeAlert.Visible = False
        ResetSearchCriteriaErrImage()
        udcCollapsibleSearchCriteriaReview.Collapsed = True
        udcCollapsibleSearchCriteriaReview.ClientState = "True"

        Dim enumSearchResult As SearchResultEnum


        ' Try-catch to avoid search over row limit (500)
        Try
            ' Write Audit Log
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.AddDescripton("Brand Name", ddlBrand.SelectedValue)
            udtAuditLogEntry.AddDescripton("Vaccine Lot No", txtVaccLotNo.Text.Trim.ToUpper)
            udtAuditLogEntry.AddDescripton("Expiry Date", txtExpiryFrom.Text.Trim.ToString() + "-" + txtExpiryTo.Text.Trim.ToString())
            udtAuditLogEntry.AddDescripton("Record Status", ddlNewRecordStatus.SelectedValue)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00001, AuditMsg00001)

            If CheckExpiryDate() Then
                If sender Is Nothing Then
                    enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, msgBox, udcInfoBox, False, True)
                Else
                    enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, msgBox, udcInfoBox)
                End If

                Select Case enumSearchResult
                    Case SearchResultEnum.Success
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditMsg00003)

                    Case SearchResultEnum.ValidationFail
                        ' Audit Log has been handled in [SF_ValidateSearch] method

                    Case SearchResultEnum.NoRecordFound
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00002, AuditMsg00002)

                    Case SearchResultEnum.OverResultList1stLimit_PopUp
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00004, AuditMsg00004)

                    Case SearchResultEnum.OverResultList1stLimit_Alert
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00004, AuditMsg00004)

                    Case SearchResultEnum.OverResultListOverrideLimit
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00004, AuditMsg00004)

                    Case Else
                        Throw New Exception("Error: Class = [HCVU.VacinneLotManagement], Method = [ibtnSearch_Click], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")

                End Select
            End If

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                msgBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, eSQL.Message))

                If msgBox.GetCodeTable.Rows.Count = 0 Then
                    msgBox.Visible = False
                Else
                    msgBox.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00004, AuditMsg00004)
                End If

            Else
                Throw eSQL
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub FillSearchCriteria()
        'If ddlVaccCentre.SelectedValue.Trim.Equals(String.Empty) Then
        '    lblResultCentre.Text = Me.GetGlobalResourceObject("Text", "Any")
        'Else
        '    lblResultCentre.Text = ddlVaccCentre.SelectedItem.ToString.Trim
        'End If

        If ddlBrand.SelectedValue.Trim.Equals(String.Empty) Then
            lblResultBrand.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else
            lblResultBrand.Text = ddlBrand.SelectedItem.Text.Trim
        End If

        'If ddlBooth.SelectedValue.Trim.Equals(String.Empty) Then
        '    lblResultBooth.Text = Me.GetGlobalResourceObject("Text", "Any")
        'Else
        '    lblResultBooth.Text = ddlBooth.SelectedItem.Text.Trim
        'End If

        If txtVaccLotNo.Text.Trim.Equals(String.Empty) Then
            lblResultLotNo.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else
            lblResultLotNo.Text = txtVaccLotNo.Text.Trim.ToUpper
        End If

        If txtExpiryFrom.Text.Trim.Equals(String.Empty) Then
            lblResultExpDtm.Text = ""
        Else
            lblResultExpDtm.Text = txtExpiryFrom.Text + " To " + txtExpiryTo.Text
        End If

        'If ddlLotStatus.SelectedValue.Trim.Equals(String.Empty) Then
        '    lblResultLotStatus.Text = Me.GetGlobalResourceObject("Text", "Any")
        'Else
        '    lblResultLotStatus.Text = ddlLotStatus.SelectedItem.Text.Trim
        'End If

        If ddlNewRecordStatus.SelectedValue.Trim.Equals(String.Empty) Then
            lblNewResultRecordS.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else
            lblNewResultRecordS.Text = ddlNewRecordStatus.SelectedItem.Text.Trim
        End If

    End Sub

    Protected Sub ibtnSearchResultBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00006, AuditMsg00006)

        'BackToSearchCriteriaView(True)
        udcInfoBox.Visible = False
        ChangeViewIndex(ViewIndex.SearchCriteria)
    End Sub

    'Protected Sub ibtnBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
    '    udtAuditLogEntry.WriteLog(LogID.LOG00009, AuditMsg00006)

    '    msgBox.Visible = False
    '    udcInfoBox.Visible = False

    '    ChangeViewIndex(ViewIndex.SearchResult)
    'End Sub

    Protected Sub ibtnErrorBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00007, AuditMsg00007)

        msgBox.Visible = False
        udcInfoBox.Visible = False
        BackToSearchCriteriaView(True)
    End Sub

    Protected Sub ibtnMsgBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00025, AuditMsg00025)
        msgBox.Visible = False
        udcInfoBox.Visible = False
        'BackToSearchCriteriaView(True)
        ChangeViewIndex(ViewIndex.SearchCriteria)
    End Sub

    Protected Sub ibtnNew_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00018, AuditMsg00018)
        ViewState("state") = StateType.ADD
        bindEditView()
    End Sub

    Private Sub WriteUpdateFailedAuditLog(ByVal strVaccineLotID As String)
        If msgBox.GetCodeTable.Rows.Count = 0 Then
            msgBox.Visible = False
        Else
            Dim strAction As String = ViewState("action")
            Dim udtExAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtExAuditLogEntry.AddDescripton("Vaccine Lot ID", strVaccineLotID)
            Select Case strAction
                Case ActionType.Update
                    msgBox.BuildMessageBox("UpdateFail", udtExAuditLogEntry, LogID.LOG00022, AuditMsg00011)
                Case ActionType.Active
                    msgBox.BuildMessageBox("UpdateFail", udtExAuditLogEntry, LogID.LOG00038, AuditMsg00016)
                Case ActionType.Deactive
                    msgBox.BuildMessageBox("UpdateFail", udtExAuditLogEntry, LogID.LOG00039, AuditMsg00017)
                Case ActionType.Remove
                    msgBox.BuildMessageBox("UpdateFail", udtExAuditLogEntry, LogID.LOG00037, AuditMsg00015)
            End Select

        End If
    End Sub

    Private Sub BackToSearchCriteriaView(ByVal blnIsReset As Boolean)
        udcInfoBox.Visible = False

        ChangeViewIndex(ViewIndex.SearchCriteria)
        If blnIsReset Then
            ResetSearchCriteria()
        Else
            ibtnSearch_Click(Nothing, Nothing)
        End If
    End Sub


#Region "Search Criteria Function"

    'Protected Sub ddlVaccCentre_SelectedIndexChanged(sender As Object, e As EventArgs)
    '    Dim dtBooth As DataTable = udtVaccineLotBLL.GetVaccineCentreSPMapping().DefaultView.ToTable(True, {"Centre_ID", "Booth"})
    '    Dim dtboothClone As DataTable = dtBooth.Clone
    '    dtboothClone.Columns("Booth").DataType = System.Type.GetType("System.Int32")
    '    For Each dr As DataRow In dtBooth.Rows
    '        dtboothClone.ImportRow(dr)
    '    Next
    '    dtboothClone.AcceptChanges()
    '    Dim dvBooth As DataView = New DataView(dtboothClone)

    '    If (ddlVaccCentre.SelectedValue.Trim <> String.Empty) Then

    '        dvBooth.RowFilter = "[Centre_ID] like '" + ddlVaccCentre.SelectedValue.Trim + "'"
    '        dvBooth.Sort = "Booth asc"
    '        ddlBooth.Items.Clear()
    '        If dvBooth IsNot Nothing AndAlso dvBooth.Count > 0 Then
    '            ddlBooth.DataSource = dvBooth
    '            ddlBooth.DataTextField = "Booth"
    '            ddlBooth.DataValueField = "Booth"
    '            ddlBooth.DataBind()

    '            'If ddlBooth.Items.Count > 1 Then
    '            '    ddlBooth.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))
    '            'End If
    '            ddlBooth.SelectedIndex = 0

    '            If ddlBooth.Items.Count >= 1 Then
    '                ddlBooth.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "ALL"), "ALL"))
    '                ddlBooth.SelectedIndex = 1
    '            End If

    '            ddlBooth.Enabled = True
    '        Else
    '            BindBoothBase(ddlBooth, True)
    '        End If
    '    Else
    '        BindBoothBase(ddlBooth, False)
    '    End If

    'End Sub



    Private Function CheckExpiryDate() As Boolean
        Dim blnValidDate As Boolean = True
        Dim udtSystemMessage As SystemMessage

        If (txtExpiryFrom.Text.Trim.Equals(String.Empty) And txtExpiryTo.Text.Trim.Equals(String.Empty)) Then
            Return True
        End If

        ' Check Date
        ' 2: Check the date format
        Dim strExpiryDateFrom As String = IIf(udtFormatter.formatInputDate(txtExpiryFrom.Text.Trim) <> String.Empty, udtFormatter.formatInputDate(txtExpiryFrom.Text.Trim), txtExpiryFrom.Text.Trim)
        Dim strExpiryDateTo As String = IIf(udtFormatter.formatInputDate(txtExpiryTo.Text.Trim) <> String.Empty, udtFormatter.formatInputDate(txtExpiryTo.Text.Trim), txtExpiryTo.Text.Trim)

        If blnValidDate Then
            ' Format the input date (Date From / To)
            udtSystemMessage = udtValidator.chkInputDate(strExpiryDateFrom, True, False)
            If IsNothing(udtSystemMessage) Then udtSystemMessage = udtValidator.chkInputDate(strExpiryDateTo, True, False)

            If Not IsNothing(udtSystemMessage) Then
                msgBox.AddMessage(udtSystemMessage, "%s", lblExpiryDate.Text.Trim)
                imgExpiryDateFromErr.Visible = True
                blnValidDate = False
            Else
                txtExpiryFrom.Text = strExpiryDateFrom
                txtExpiryTo.Text = strExpiryDateTo
            End If
        End If

        ' 3: Check date dependency: From < To
        If blnValidDate Then
            udtSystemMessage = udtValidator.chkInputValidFromDateCutoffDate(FunctCode.FUNT990000, MsgCode.MSG00381, _
                    udtFormatter.convertDate(strExpiryDateFrom, String.Empty), udtFormatter.convertDate(strExpiryDateTo, String.Empty))

            'From Date should not be later than the To Date in "Date".
            If Not IsNothing(udtSystemMessage) Then
                blnValidDate = False
                msgBox.AddMessage(udtSystemMessage, {"%t", "%s"}, {GetGlobalResourceObject("Text", "VaccineExpiryDateFrom"), GetGlobalResourceObject("Text", "VaccineExpiryDateTo")})
            End If
        End If

        If msgBox.GetCodeTable.Rows.Count = 0 Then
            msgBox.Visible = False
        Else
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00026, AuditMsg00026)
        End If

        Return blnValidDate
    End Function
#End Region

#Region "Common Function"


    Private Sub BuildBrand(ByVal ddl As DropDownList, ByVal blnIsAny As Boolean)

        Dim dtAllBrand As DataTable = udtVaccineLotBLL.GetCOVID19VaccineBrandDetail()
        dtAllBrand = dtAllBrand.DefaultView.ToTable(True, {"Brand_Name", "Brand_ID"})

        If dtAllBrand IsNot Nothing AndAlso dtAllBrand.Rows.Count > 0 Then
            ddl.Items.Clear()
            ddl.DataSource = dtAllBrand
            ddl.DataTextField = "Brand_Name"
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


    Private Sub BuildVaccineLotRecordStatus(ByVal ddl As DropDownList)
        Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL
        ddl.DataSource = udtStaticDataBLL.GetStaticDataListByColumnName("VaccineLotRecordCreationStatus")
        ddl.DataValueField = "ItemNo"
        ddl.DataTextField = "DataValue"
        ddl.DataBind()
        ddl.Items.Remove(ddl.Items.FindByValue(VaccineLotDetailRecordStatus.Remove))
        ddl.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))
        ddl.SelectedIndex = 0
    End Sub



    Private Sub InitializeDataValue()
        ResetAllInputPageBind()
        ResetAllInputPageErrImage()


        BuildBrand(Me.ddlVaccineBrandName, False)
    End Sub

    Private Sub ResetAllInputPageBind()
        ddlVaccineBrandName.Items.Clear()
        txtVaccineLotNo.Text = String.Empty
        txtVaccineExpiryDateText.Text = String.Empty


    End Sub

    Private Sub ResetAllInputPageErrImage()
        imgVaccineBrandNameErr.Visible = False
        imgVaccineLotNoErr.Visible = False
        imgVaccineExpiryDateTextErr.Visible = False
    End Sub

    Private Sub ResetAllHiddenField()
        hfConVaccineBrandId.Value = String.Empty
        hfConVaccineLotNo.Value = String.Empty
        hfConVaccineExpiryDateText.Value = String.Empty
        hfEVaccineLotNo.Value = String.Empty
        hfEVaccineLotBrand.Value = String.Empty
    End Sub

#End Region

#Region "Popup box events"

    Private Sub ibtnDialogConfirm_Click(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUpConfirmRemove.ButtonClick

        'udcInfoBox.Visible = True
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        '  Dim udtVaccineLotList As VaccineLotModel = Nothing
        Dim blnResult As New Boolean
        Dim strAction As String = String.Empty

        Select Case e
            Case ucNoticePopUp.enumButtonClick.OK
                udtAuditLogEntry.AddDescripton("Vaccine Lot No.", hfEVaccineLotNo.Value.Trim)
                udtAuditLogEntry.AddDescripton("Vaccine Brand.", hfEVaccineLotBrand.Value.Trim)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00014, AuditMsg00014)
                EditRecordRemove()
            Case Else
                udtAuditLogEntry.WriteLog(LogID.LOG00015, AuditMsg00015)
                udcInfoBox.Visible = False
        End Select
    End Sub

    Private Sub ibtnDialogCancel_Click(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUpConfirmCancel.ButtonClick
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        '  Dim udtVaccineLotList As VaccineLotModel = Nothing
        Dim blnResult As New Boolean
        Dim strAction As String = String.Empty

        Select Case e
            Case ucNoticePopUp.enumButtonClick.OK
                udtAuditLogEntry.AddDescripton("Vaccine Lot No.", hfEVaccineLotNo.Value.Trim)
                udtAuditLogEntry.AddDescripton("Vaccine Brand.", hfEVaccineLotBrand.Value.Trim)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00009, AuditMsg00009)
                EditRecordCancelRequest()
            Case Else
                udtAuditLogEntry.WriteLog(LogID.LOG00010, AuditMsg00010)
                udcInfoBox.Visible = False
        End Select
    End Sub

#End Region



#Region "Vaccine Lot Record Event"
    Protected Sub ibtnRecordSave_Click(sender As Object, e As ImageClickEventArgs)


        msgBox.Visible = False
        udcInfoBox.Visible = False
        ResetAllInputPageErrImage()

        If (SaveRecordInfoValidation()) Then
            If ViewState("state") = StateType.ADD Then
                Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
                udtAuditLogEntry.AddDescripton("Brand", ddlVaccineBrandName.SelectedItem.ToString)
                udtAuditLogEntry.AddDescripton("Vaccine Lot No.", txtVaccineLotNo.Text)
                udtAuditLogEntry.AddDescripton("Vaccine Expiry Date", txtVaccineExpiryDateText.Text)
                udtAuditLogEntry.WriteLog(LogID.LOG00019, AuditMsg00019)

                lblConVaccineBrandName.Text = ddlVaccineBrandName.SelectedItem.ToString
                lblConVaccineLotNo.Text = txtVaccineLotNo.Text
                lblConVaccineExpiryDateText.Text = udtFormatter.convertDate(txtVaccineExpiryDateText.Text.Trim, String.Empty)

                'sethiddentLabel for spro
                hfConVaccineBrandId.Value = ddlVaccineBrandName.SelectedValue.ToString
                hfConVaccineLotNo.Value = txtVaccineLotNo.Text
                hfConVaccineExpiryDateText.Value = udtFormatter.convertDate(txtVaccineExpiryDateText.Text.Trim, String.Empty)

                'End If
            End If

            ChangeViewIndex(ViewIndex.Confirm)
            udcInfoBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00021)
            udcInfoBox.Type = CustomControls.InfoMessageBoxType.Information
            udcInfoBox.BuildMessageBox()
        End If

        'audit log
    End Sub

    Protected Sub ibtnRecordCancel_Click(sender As Object, e As ImageClickEventArgs)
        Dim strVaccineBrandName As String = ddlVaccineBrandName.SelectedItem.ToString
        Dim strVaccineLotNo As String = txtVaccineLotNo.Text
        Dim strVaccineExpiryDate As String = txtVaccineExpiryDateText.Text
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00020, AuditMsg00020)

        msgBox.Visible = False
        udcInfoBox.Visible = False
        ChangeViewIndex(ViewIndex.SearchCriteria)

    End Sub

    Protected Sub ibtnRecordBackToInput_Click(sender As Object, e As ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        msgBox.Visible = False
        udcInfoBox.Visible = False
        udtAuditLogEntry.WriteLog(LogID.LOG00022, AuditMsg00022)

        ChangeViewIndex(ViewIndex.NewEditRecord)

    End Sub

    Protected Sub ibtnRecordConfirm_Click(sender As Object, e As ImageClickEventArgs)

        Dim strVaccineBrandName As String = ddlVaccineBrandName.SelectedItem.ToString
        Dim strVaccineLotNo As String = txtVaccineLotNo.Text
        Dim strVaccineExpiryDate As String = txtVaccineExpiryDateText.Text
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Brand", strVaccineBrandName)
        udtAuditLogEntry.AddDescripton("Vaccine Lot No.", strVaccineLotNo)
        udtAuditLogEntry.AddDescripton("Vaccine Expiry Date", strVaccineExpiryDate)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00021, AuditMsg00021)


        udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001)
        udcInfoBox.BuildMessageBox()

        Dim udtVaccineLotCreationList As VaccineLotCreationModel = New VaccineLotCreationModel()

        udtVaccineLotCreationList.BrandId = hfConVaccineBrandId.Value
        udtVaccineLotCreationList.VaccineLotNo = hfConVaccineLotNo.Value
        udtVaccineLotCreationList.VaccineExpiryDate = hfConVaccineExpiryDateText.Value
        udtVaccineLotCreationList.CreateBy = udtHCVUUserBLL.GetHCVUUser.UserID
        udtVaccineLotCreationList.UpdateBy = udtHCVUUserBLL.GetHCVUUser.UserID
        udtVaccineLotCreationList.RequestBy = udtHCVUUserBLL.GetHCVUUser.UserID
        udtVaccineLotCreationList.RequestType = VaccineLotRequestType.REQUESTTYPE_NEW

        If ViewState("state") = StateType.ADD Then
            Dim blnResult = udtVaccineLotBLL.SaveVaccineLotCreationRecord(udtVaccineLotCreationList)
            If (blnResult) Then
                udcInfoBox.Type = CustomControls.InfoMessageBoxType.Complete
                udtAuditLogEntry.WriteEndLog(LogID.LOG00023, AuditMsg00023)
                ChangeViewIndex(ViewIndex.MsgPage)
            Else
                udtAuditLogEntry.WriteEndLog(LogID.LOG00024, AuditMsg00024)
            End If
        End If


    End Sub

    Protected Sub ibtnEditRecordBack_Click(sender As Object, e As ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00007, AuditMsg00007)
        msgBox.Visible = False
        udcInfoBox.Visible = False
        ChangeViewIndex(ViewIndex.SearchResult)
    End Sub


    Protected Sub ibtnEditRecordCancel_Click(sender As Object, e As ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00008, AuditMsg00008)
        ViewState("action") = ActionType.Cancel
        ModalPopupConfirmCancel.Show()


    End Sub

    Protected Sub ibtnEditRecordRemove_Click(sender As Object, e As ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00013, AuditMsg00013)
        ViewState("action") = ActionType.Remove
        ModalPopupConfirmRemove.Show()

    End Sub

    Protected Sub EditRecordRemove()

        Dim blnResult As New Boolean
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        msgBox.Visible = False
        udcInfoBox.Visible = False

        Dim vcModel As VaccineLotCreationModel = udtVaccineLotBLL.GetVaccineLotCreationModalByLotNo(hfEVaccineLotNo.Value.Trim, hfEVaccineLotBrand.Value.Trim)

        udtAuditLogEntry.AddDescripton("Vaccine Lot No.", hfEVaccineLotNo.Value.Trim)
        udtAuditLogEntry.AddDescripton("Vaccine Brand.", hfEVaccineLotBrand.Value.Trim)
        blnResult = udtVaccineLotBLL.VaccineLotCreationAction(hfEVaccineLotNo.Value, VACCINELOT_ACTIONTYPE.ACTION_EDIT, vcModel.TSMP)

        If (blnResult) Then
            udtAuditLogEntry.WriteEndLog(LogID.LOG00016, AuditMsg00016)
            udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00002)
            udcInfoBox.Type = CustomControls.InfoMessageBoxType.Complete
            udcInfoBox.BuildMessageBox()
            ChangeViewIndex(ViewIndex.MsgPage)
        Else
            'ViewState("action") = ActionType.Approval
            udtAuditLogEntry.WriteEndLog(LogID.LOG00017, AuditMsg00017)
            WriteUpdateFailedAuditLog(hfEVaccineLotNo.Value.Trim)
        End If

        'later
        'ChangeViewIndex(ViewIndex.SearchCriteria)
    End Sub

    Protected Sub EditRecordCancelRequest()

        Dim blnResult As New Boolean
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim vcModel As VaccineLotCreationModel = udtVaccineLotBLL.GetVaccineLotCreationModalByLotNo(hfEVaccineLotNo.Value.Trim, hfEVaccineLotBrand.Value.Trim)
        msgBox.Visible = False
        udcInfoBox.Visible = False

        blnResult = udtVaccineLotBLL.VaccineLotCreationAction(hfEVaccineLotNo.Value, VACCINELOT_ACTIONTYPE.ACTION_CANCELREQUEST, vcModel.TSMP)
        udtAuditLogEntry.AddDescripton("Vaccine Lot No.", hfEVaccineLotNo.Value.Trim)
        udtAuditLogEntry.AddDescripton("Vaccine Brand.", hfEVaccineLotBrand.Value.Trim)

        If (blnResult) Then
            udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003)
            udcInfoBox.Type = CustomControls.InfoMessageBoxType.Complete
            udcInfoBox.BuildMessageBox()
            udtAuditLogEntry.WriteEndLog(LogID.LOG00011, AuditMsg00011)
            ChangeViewIndex(ViewIndex.MsgPage)
        Else
            udtAuditLogEntry.WriteEndLog(LogID.LOG00012, AuditMsg00012)
        End If

    End Sub

#End Region


    Private Sub bindEditView()
        msgBox.Visible = False
        udcInfoBox.Visible = False
        ChangeViewIndex(ViewIndex.NewEditRecord)

        If ViewState("state") = StateType.ADD Then
            InitializeDataValue()
            panNewRecord.Visible = True
            'panEditRecord.Visible = False
        ElseIf ViewState("state") = StateType.EDIT Then
            'ResetAllInputPageErrImage()
            'panNewRecord.Visible = False
            'panEditRecord.Visible = True
            'BuildVaccineLotStatus(Me.ddlVaccineLotStatus, False)

            'Dim udtVaccineLotList As VaccineLotCreationModel = Nothing
            'udtVaccineLotList = udtVaccineLotBLL.GetVaccineLotModalByVaccineLotId(hfEVaccineLotNo.Value)
            'lblPERConVaccineCentre.Text = udtVaccineLotList.CentreName
            'lblPERConVaccineBooth.Text = udtVaccineLotList.Booth
            'lblPERConVaccineBrand.Text = udtVaccineLotList.BrandName
            'lblPERConVaccineLotNo.Text = udtVaccineLotList.VaccineLotNo
            'lblVaccineExpiryDateText.Text = Format(CDate(udtVaccineLotList.VaccineExpiryDate), "dd MMM yyyy")
            'txtExpiryFrom.Text = Format(CDate(udtVaccineLotList.VaccineLotExpiryDFrom), "dd-MM-yyyy")
            'txtVaccineLotExpiryDateTo.Text = Format(CDate(udtVaccineLotList.VaccineLotExpiryDTo), "dd-MM-yyyy")
            'ddlVaccineLotStatus.SelectedValue = udtVaccineLotList.LotStatus


            'hfPERConVaccineCentre.Value = udtVaccineLotList.CentreName
            'hfPERConVaccineBooth.Value = udtVaccineLotList.Booth
            'hfPERConVaccineBrand.Value = udtVaccineLotList.BrandName
            'hfPERConVaccineLotNo.Value = udtVaccineLotList.VaccineLotNo
            'hfVaccineLotExpiryDateFrom.Value = Format(CDate(udtVaccineLotList.VaccineLotExpiryDFrom), "dd-MM-yyyy")
            'hfVaccineLotExpiryDateTo.Value = Format(CDate(udtVaccineLotList.VaccineLotExpiryDTo), "dd-MM-yyyy")
            'hfVaccineLotStatus.Value = udtVaccineLotList.LotStatus

        End If
    End Sub

    Private Sub ChangeViewIndex(ByVal udtViewIndex As ViewIndex)
        MultiViewEnquiry.ActiveViewIndex = udtViewIndex

        Select Case udtViewIndex
            Case ViewIndex.SearchCriteria
                Session.Remove(SESS_VaccineLotListModal)
            Case ViewIndex.SearchResult
                Session.Remove(SESS_VaccineLotListModal)
            Case ViewIndex.Confirm
            Case ViewIndex.MsgPage
            Case ViewIndex.ErrorPage
        End Select
    End Sub




    Private Sub ResetSearchCriteria()
        'txtRCHCode.Text = String.Empty
        ddlBrand.SelectedValue = String.Empty
        txtVaccLotNo.Text = String.Empty
        'txtRCHName.Text = String.Empty
        txtExpiryTo.Text = String.Empty
        txtExpiryTo.Text = String.Empty
        'ddlRCHStatus.SelectedValue = String.Empty
    End Sub

    Private Sub ResetSearchCriteriaErrImage()
        imgExpiryDateFromErr.Visible = False

    End Sub

    Private Sub gvResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            'Record Status lblVLStatus
            Dim lblVLStatus As Label = CType(e.Row.FindControl("lblVLStatus"), Label)
            Status.GetDescriptionFromDBCode(VaccineLotDetailRecordStatus.ClassCode, lblVLStatus.Text.Trim, lblVLStatus.Text, String.Empty)

            'Request Type
            Dim lblRequestType As Label = CType(e.Row.FindControl("lblRequestType"), Label)
            If lblRequestType.Text = String.Empty Then
                lblRequestType.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                Status.GetDescriptionFromDBCode(VaccineLotRequestStatus.RequstTypeClassCode, lblRequestType.Text.Trim, lblRequestType.Text, String.Empty)
            End If
        End If
    End Sub

    Private Sub gvResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvResult.PageIndexChanging
        Me.GridViewPageIndexChangingHandler(sender, e, SESS_SearchResultList)
    End Sub

    Private Sub gvResult_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvResult.PreRender
        Me.GridViewPreRenderHandler(sender, e, SESS_SearchResultList)
    End Sub

    Private Sub gvResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvResult.RowCommand
        If TypeOf e.CommandSource Is LinkButton Then
            Dim r As GridViewRow = CType(e.CommandSource, LinkButton).NamingContainer
            Dim strLotNo As String = CType(r.FindControl("lnkbtnVLNo"), LinkButton).CommandArgument.Trim
            Dim strBrandId As String = CType(r.FindControl("hfVLBrandId"), HiddenField).Value.Trim
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00005, AuditMsg00005)
            BindVaccineSummaryView(strLotNo, strBrandId)
        End If
    End Sub

    Private Sub gvResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvResult.Sorting
        Me.GridViewSortingHandler(sender, e, SESS_SearchResultList)
    End Sub



    Protected Sub BindVaccineSummaryView(ByVal strVLNo As String, ByVal strBrandId As String)

        'Initialize
        trinfoRequestedBy.Visible = False
        trinfoRequestType.Visible = False
        trinfoApprovedBy.Visible = False

        Dim udtVaccineLotCreationList As VaccineLotCreationModel = Nothing
        hfEVaccineLotNo.Value = strVLNo
        hfEVaccineLotBrand.Value = strBrandId
        udtVaccineLotCreationList = udtVaccineLotBLL.GetVaccineLotCreationModalByLotNo(strVLNo, strBrandId)

        ChangeViewIndex(ViewIndex.EditRecordView)


        lblEConVaccineBrandName.Text = udtVaccineLotCreationList.BrandName
        lblEConVaccineLotNo.Text = udtVaccineLotCreationList.VaccineLotNo
        lblEConVaccineExpiryDateText.Text = udtFormatter.formatDisplayDate(CDate(udtVaccineLotCreationList.VaccineExpiryDate))
        lblCreatedBy.Text = udtVaccineLotCreationList.CreateBy + " (" + Format(CDate(udtVaccineLotCreationList.CreateDtm), "dd MMM yyyy HH:mm") + ")"
        lblEConVaccineNewRecordStatus.Text = udtVaccineLotCreationList.RecordStatus
        Status.GetDescriptionFromDBCode(VaccineLotDetailRecordStatus.ClassCode, lblEConVaccineNewRecordStatus.Text, lblEConVaccineNewRecordStatus.Text, String.Empty)

        'Pending record
        If udtVaccineLotCreationList.RecordStatus = VaccineLotDetailRecordStatus.Pending Then
            trinfoRequestedBy.Visible = True

            If udtVaccineLotCreationList.RequestType <> String.Empty Then
                trinfoRequestType.Visible = True
                lblERequestType.Text = udtVaccineLotCreationList.RequestType
                Status.GetDescriptionFromDBCode(VaccineLotRequestStatus.RequstTypeClassCode, lblERequestType.Text, lblERequestType.Text, String.Empty)
            End If

            lblERequestedBy.Text = udtVaccineLotCreationList.RequestBy + " (" + Format(CDate(udtVaccineLotCreationList.RequestDtm), "dd MMM yyyy HH:mm") + ")"

            'ibtnEditRecordEdit.Enabled = False
            'ibtnEditRecordEdit.ImageUrl = GetGlobalResourceObject("ImageUrl", "EditDisableBtn")
            ibtnEditRecordRemove.Enabled = False
            ibtnEditRecordRemove.ImageUrl = GetGlobalResourceObject("ImageUrl", "RemoveDisableBtn")
            ibtnEditRecordCancel.Enabled = True
            ibtnEditRecordCancel.ImageUrl = GetGlobalResourceObject("ImageUrl", "CancelRequestBtn")

            'udcInfoBox.BuildMessageBox()
            'udcInfoBox.Type = CustomControls.InfoMessageBoxType.Complete

            'udcInfoBox.Visible = True


            'udcInfoBox.AddMessage(FunctCode.FUNT010422, SeverityCode.SEVI, MsgCode.MSG00004)
            'udcInfoBox.BuildMessageBox()
            'udcInfoBox.Type = CustomControls.InfoMessageBoxType.Complete

            'ChangeViewIndex(ViewIndex.MsgPage)
        Else
            If udtVaccineLotCreationList.ApproveBy IsNot String.Empty Then
                trinfoApprovedBy.Visible = True
            End If

            lblEApprovedBy.Text = udtVaccineLotCreationList.ApproveBy + " (" + Format(CDate(udtVaccineLotCreationList.ApproveDtm), "dd MMM yyyy HH:mm") + ")"
            'ibtnEditRecordEdit.Enabled = True
            'ibtnEditRecordEdit.ImageUrl = GetGlobalResourceObject("ImageUrl", "EditBtn")
            ibtnEditRecordRemove.Enabled = True
            ibtnEditRecordRemove.ImageUrl = GetGlobalResourceObject("ImageUrl", "RemoveBtn")
            ibtnEditRecordCancel.Enabled = False
            ibtnEditRecordCancel.ImageUrl = GetGlobalResourceObject("ImageUrl", "CancelDisableRequestBtn")
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

    Private Function SaveRecordInfoValidation() As Boolean
        Dim blnValid As Boolean = True
        Dim blnValidDate As Boolean = True
        Dim udtSystemMessage As SystemMessage
        'Dim strTodayForDataViewCheck As String = DateTime.Now.ToString("MM/dd/yyyy")
        Dim strTodayForCutOffChecking As String = (DateTime.Now).ToString("dd-MM-yyyy")


        'imgVaccineCentreErr.Visible = False
        imgVaccineBrandNameErr.Visible = False
        imgVaccineLotNoErr.Visible = False
        imgVaccineExpiryDateTextErr.Visible = False
        'imgExpiryDateFromErr.Visible = False
        'imgVaccineLotExpiryDateToErr.Visible = False

        If ViewState("state") = StateType.ADD Then
            'Check Mandatory Fields
            If ddlVaccineBrandName.Text.Trim = String.Empty Then
                msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00367), "%s", GetGlobalResourceObject("Text", "VaccineBrandName"))
                imgVaccineBrandNameErr.Visible = True
                blnValid = False
            End If
            If txtVaccineLotNo.Text.Trim = String.Empty Then
                msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00028), "%s", GetGlobalResourceObject("Text", "VaccineLotNo"))
                imgVaccineLotNoErr.Visible = True
                blnValid = False
            End If
            If txtVaccineExpiryDateText.Text.Trim = String.Empty Then
                msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00028), "%s", GetGlobalResourceObject("Text", "VaccineExpiryDate"))
                imgVaccineExpiryDateTextErr.Visible = True
                blnValid = False
                blnValidDate = False
            End If



            Dim strVaccineExpiryDateText As String = IIf(udtFormatter.formatInputDate(txtVaccineExpiryDateText.Text.Trim) <> String.Empty, udtFormatter.formatInputDate(txtVaccineExpiryDateText.Text.Trim), txtVaccineExpiryDateText.Text.Trim)

            If blnValidDate Then

                udtSystemMessage = udtValidator.chkInputDate(strVaccineExpiryDateText, True, False)
                If Not IsNothing(udtSystemMessage) Then
                    msgBox.AddMessage(udtSystemMessage, "%s", GetGlobalResourceObject("Text", "VaccineExpiryDate"))
                    imgVaccineExpiryDateTextErr.Visible = True
                    blnValidDate = False
                Else
                    txtVaccineExpiryDateText.Text = strVaccineExpiryDateText
                End If
            End If

            'check expiry date not early than today
            If blnValidDate Then
                udtSystemMessage = udtValidator.chkInputValidFromDateCutoffDate(FunctCode.FUNT990000, MsgCode.MSG00381, _
                   udtFormatter.convertDate(strTodayForCutOffChecking, String.Empty), udtFormatter.convertDate(strVaccineExpiryDateText, String.Empty))
                If Not IsNothing(udtSystemMessage) Then
                    msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00375), "%s", GetGlobalResourceObject("Text", "VaccineExpiryDate"))
                    imgVaccineExpiryDateTextErr.Visible = True
                    blnValid = False
                    blnValidDate = False
                End If
            End If
        End If


        If blnValidDate = False Then
            blnValid = blnValidDate
        End If



        'check existing lot no in db.
        If (blnValid) Then

            Dim searchResultByLotId As BaseBLL.BLLSearchResult = udtVaccineLotBLL.GetVaccineLotCreationSearch(ddlVaccineBrandName.SelectedValue.Trim, _
                                                                                                                    txtVaccineLotNo.Text.Trim, _
                                                                                                                         String.Empty, _
                                                                                                                        String.Empty, _
                                                                                                                        String.Empty)


            If Not searchResultByLotId.Data Is Nothing Then
                Dim dvsearchResultByLotId As DataView = New DataView(CType(searchResultByLotId.Data, DataTable))

                'filter the result with record status='P'
                'dvsearchResultByLotId.RowFilter = "[Record_status] like '" + RecordStatus.Active + "' and Request_Type is NULL"

                If (dvsearchResultByLotId.Count <> 0) Then
                    msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002))
                    blnValid = False
                End If
            End If


        End If


        ''Check any pending + overlap date record
        'If blnValid Then

        '    Dim strCentreForSearch As String = String.Empty
        '    Dim strBoothForSearch As String = String.Empty
        '    Dim strVaccineLotNoForSearch As String = String.Empty
        '    Dim strCompareDateFrom As String = Format(CDate(udtFormatter.convertDate(strVaccineLotExpiryDateFrom, String.Empty)), "MM/dd/yyyy")
        '    Dim strCompareDateTo As String = Format(CDate(udtFormatter.convertDate(strVaccineLotExpiryDateTo, String.Empty)), "MM/dd/yyyy")

        '    Dim SerivceDateConditionForFiltering As String = String.Empty
        '    Dim SerivceDateCondition1 As String = "(#" + strCompareDateFrom + "# >= [VaccineLotExpiryDateFrom] and #" + strCompareDateFrom + "# <= [VaccineLotExpiryDateTo])"
        '    Dim SerivceDateCondition2 As String = "(#" + strCompareDateTo + "# >= [VaccineLotExpiryDateFrom] and #" + strCompareDateTo + "# <= [VaccineLotExpiryDateTo])"
        '    Dim SerivceDateCondition3 As String = "(#" + strCompareDateFrom + "# >= [VaccineLotExpiryDateFrom] and #" + strCompareDateTo + "# <= [VaccineLotExpiryDateTo])"
        '    Dim SerivceDateCondition4 As String = "(#" + strCompareDateFrom + "# <= [VaccineLotExpiryDateFrom] and #" + strCompareDateTo + "# >= [VaccineLotExpiryDateTo])"


        '    Dim SerivceDateCondition5 As String = "[Lot_Status] = 'A'"


        '    'define value for search
        '    'defind filtering confidion for checking record whether existing
        '    If ViewState("state") = StateType.ADD Then
        '        strCentreForSearch = ddlVaccineCentre.SelectedValue.Trim
        '        strBoothForSearch = ddlVaccineCentreBooth.SelectedValue.Trim
        '        strVaccineLotNoForSearch = ddlVaccineLotNo.SelectedValue.Trim

        '        SerivceDateConditionForFiltering = "(" + SerivceDateCondition1 + " or " + SerivceDateCondition2 + " or " + SerivceDateCondition3 + " or " + SerivceDateCondition4 + ") and " + SerivceDateCondition5

        '    ElseIf ViewState("state") = StateType.EDIT Then
        '        strCentreForSearch = hfPERConVaccineCentre.Value.Trim
        '        strBoothForSearch = hfPERConVaccineBooth.Value.Trim
        '        strVaccineLotNoForSearch = hfPERConVaccineLotNo.Value.Trim

        '        'Edit Record filter date cannot include itself
        '        Dim SerivceDateCondition6 As String = "[Vaccine_Lot_No] <> '" + hfEVaccineLotNo.Value + "'"
        '        SerivceDateConditionForFiltering = "(" + SerivceDateCondition1 + " or " + SerivceDateCondition2 + " or " + SerivceDateCondition3 + " or " + SerivceDateCondition4 + ") and " + SerivceDateCondition5 + " and " + SerivceDateCondition6

        '    End If


        '    'Search by Centre + SelectedBooth + Lotno inputted by user
        '    Dim searchResultBySelectedBooth As BaseBLL.BLLSearchResult = udtVaccineLotBLL.GetVaccineLotCreationSearch(strCentreForSearch, _
        '                                                                                                             String.Empty, _
        '                                                                                                             strBoothForSearch, _
        '                                                                                                            String.Empty, _
        '                                                                                                            strVaccineLotNoForSearch, _
        '                                                                                                             String.Empty, _
        '                                                                                                              String.Empty, _
        '                                                                                                               String.Empty, _
        '                                                                                                                String.Empty)



        '    If Not searchResultBySelectedBooth.Data Is Nothing Then
        '        Dim dvsearchResultBySelectedBooth As DataView = New DataView(CType(searchResultBySelectedBooth.Data, DataTable))

        '        'filter the result with record status='P'
        '        dvsearchResultBySelectedBooth.RowFilter = "[Record_status] like '" + RecordStatus.Pending + "' and  lot_status ='A'"

        '        If (dvsearchResultBySelectedBooth.Count <> 0) Then
        '            msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001))
        '            blnValid = False
        '        End If

        '        dvsearchResultBySelectedBooth.RowFilter = SerivceDateConditionForFiltering

        '        If (dvsearchResultBySelectedBooth.Count <> 0) Then
        '            msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002))
        '            blnValid = False
        '        End If
        '    End If

        '    'If Not searchResultByAllBooth.Data Is Nothing Then
        '    '    Dim dvsearchResultByAllBooth As DataView = New DataView(CType(searchResultByAllBooth.Data, DataTable))

        '    '    'filter the result with record status='P'
        '    '    dvsearchResultByAllBooth.RowFilter = "[New_Record_status] like '" + RecordStatus.Pending + "'"

        '    '    If (dvsearchResultByAllBooth.Count <> 0) Then
        '    '        msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001))
        '    '        blnValid = False
        '    '    End If

        '    '    dvsearchResultByAllBooth.RowFilter = SerivceDateConditionForFiltering

        '    '    If (dvsearchResultByAllBooth.Count <> 0) Then
        '    '        msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002))
        '    '        blnValid = False
        '    '    End If
        '    'End If


        '    'If ViewState("state") = StateType.EDIT Then

        '    '    If Not udtVaccineLotBLL.IsAllTransactionInPeriodByLotIdExpiryDate(hfEVaccineLotNo.Value, strVaccineLotExpiryDateFrom, strVaccineLotExpiryDateTo) Then
        '    '        msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00004))
        '    '        blnValid = False
        '    '    End If



        '    '    If hfVaccineLotExpiryDateFrom.Value = txtExpiryFrom.Text.Trim AndAlso _
        '    '        hfVaccineLotExpiryDateTo.Value = txtVaccineLotExpiryDateTo.Text.Trim AndAlso _
        '    '        hfVaccineLotStatus.Value = ddlVaccineLotStatus.SelectedValue.Trim Then

        '    '        msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00003))
        '    '        blnValid = False

        '    '    End If
        '    'End If
        'End If

        If msgBox.GetCodeTable.Rows.Count = 0 Then
            msgBox.Visible = False
        Else
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00026, AuditMsg00026)
        End If

        Return blnValid
    End Function

#End Region

#Region "Implement IWorkingData (CRE11-004)"
    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Account which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Transaction which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Service Provider which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve Document Code which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetDocCode() As String
        Return Nothing
    End Function
#End Region

End Class
