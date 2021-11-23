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

Partial Public Class VaccineLotCreation
    Inherits BasePageWithControl


    Public Enum StateType
        LOADED = 0
        EDIT = 1
        ADD = 2
    End Enum

    Public Enum ActionType
        Create = 0
        Edit = 1
        Remove = 2
        Cancel = 3
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
    Private udtCOVID19BLL As New COVID19BLL

    Private udtFormatter As New Formatter
    Private udtValidator As New Validator
    Private udtGeneralFunction As New GeneralFunction

    Private SM As SystemMessage
    Private udtAuditLogEntry As AuditLogEntry

    Private Const SESS_SearchResultList As String = FunctCode.FUNT010424 & "VaccineLotCreationList_SearchResultList"
    Private Const SESS_VaccineLotListModal As String = FunctCode.FUNT010424 & "VaccineLotcreationList_Modal"
    Private Const SESS_LotAssignedCentreList As String = FunctCode.FUNT010424 & "VaccineLotCreationList_AssignedCentreList"

    Private Const CentreServiceType As String = "CENTRE"
    Private Const PrivateServiceType As String = "PRIVATE"
    Private Const RVPServiceType As String = "RVP"


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
    Private Const AuditMsg00027 As String = "Edit Click"
    Private Const AuditMsg00028 As String = "Edit Record Page - Save Click"
    Private Const AuditMsg00029 As String = "Edit Record Page - Back Click"
    Private Const AuditMsg00030 As String = "Edit Vaccine Lot Creation successful"
    Private Const AuditMsg00031 As String = "Edit Vaccine Lot Creation failed"
    Private Const AuditMsg00032 As String = "Edit Record Page "

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
        'Return bllSearchResult
        Dim bllSearchResult As Common.Component.BaseBLL.BLLSearchResult

        bllSearchResult = udtVaccineLotBLL.GetVaccineLotCreationSearch(ddlBrand.SelectedValue.Trim, _
                                                                txtVaccLotNo.Text.Trim.ToUpper,
                                                                txtExpiryFrom.Text.Trim, _
                                                                txtExpiryTo.Text.Trim, _
                                                                ddlRecordStatus.SelectedValue.Trim)

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

            Me.GridViewDataBind(gvResult, dt, "Brand_Trade_Name", "ASC", False)

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
                Throw New Exception("Error: Class = [HCVU.Vaccine Lot Creation], Method = [SF_ConfirmSearch_Click], Message = The unexpected error for the result of re-search")

        End Select
    End Sub

    Protected Overrides Sub SF_CancelSearch_Click()
    End Sub

#End Region

#Region "Page Events"

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
            BuildVaccineLotRecordStatus(ddlRecordStatus)
            BuildVaccineLotAssignStatus(ddlVaccineLotAssignStatus)
            CalendarExtender3.StartDate = DateAdd(DateInterval.Day, 0, Today)
            ResetAllHiddenField()

            ''set the popup message box
            'popupMsgBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00005)
            'popupMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00026, AuditMsg00026)
        End If

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
            udtAuditLogEntry.AddDescripton("Record Status", ddlRecordStatus.SelectedValue)
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
                        Throw New Exception("Error: Class = [HCVU.VacinneLotCreation], Method = [ibtnSearch_Click], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")

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

        If ddlBrand.SelectedValue.Trim.Equals(String.Empty) Then
            lblResultBrand.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else
            lblResultBrand.Text = ddlBrand.SelectedItem.Text.Trim
        End If

        If txtVaccLotNo.Text.Trim.Equals(String.Empty) Then
            lblResultLotNo.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else
            lblResultLotNo.Text = txtVaccLotNo.Text.Trim.ToUpper
        End If

        If txtExpiryFrom.Text.Trim.Equals(String.Empty) Then
            lblResultExpDtm.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else
            lblResultExpDtm.Text = udtFormatter.convertDate(txtExpiryFrom.Text, String.Empty) + " To " + udtFormatter.convertDate(txtExpiryTo.Text, String.Empty)
        End If

        If ddlRecordStatus.SelectedValue.Trim.Equals(String.Empty) Then
            lblNewResultRecordS.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else
            lblNewResultRecordS.Text = ddlRecordStatus.SelectedItem.Text.Trim
        End If

    End Sub

    Protected Sub ibtnSearchResultBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00006, AuditMsg00006)

        'BackToSearchCriteriaView(True)
        udcInfoBox.Visible = False
        msgBox.Visible = False
        ChangeViewIndex(ViewIndex.SearchCriteria)
    End Sub

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

    Protected Sub ibtnEditRecordEdit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00018, AuditMsg00027)
        ViewState("state") = StateType.EDIT
        'ChangeViewIndex(ViewIndex.ViewEditLotRecordDetail)
        ChangeViewIndex(ViewIndex.NewEditRecord)
        bindEditView()
    End Sub

    Private Sub WriteUpdateFailedAuditLog(ByVal strVaccineLotNo As String)
        If msgBox.GetCodeTable.Rows.Count = 0 Then
            msgBox.Visible = False
        Else
            Dim strAction As String = ViewState("action")
            Dim udtExAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtExAuditLogEntry.AddDescripton("Vaccine Lot No", strVaccineLotNo)
            Select Case strAction
                Case ActionType.Edit
                    msgBox.BuildMessageBox("UpdateFail", udtExAuditLogEntry, LogID.LOG00022, AuditMsg00011)
                Case ActionType.Remove
                    msgBox.BuildMessageBox("UpdateFail", udtExAuditLogEntry, LogID.LOG00038, AuditMsg00016)
                Case ActionType.Cancel
                    msgBox.BuildMessageBox("UpdateFail", udtExAuditLogEntry, LogID.LOG00039, AuditMsg00011)

            End Select

        End If
    End Sub

    Private Sub BackToSearchCriteriaView(ByVal blnIsReset As Boolean)
        udcInfoBox.Visible = False
        msgBox.Visible = False

        ChangeViewIndex(ViewIndex.SearchCriteria)
        If blnIsReset Then
            ResetSearchCriteria()
        Else
            ibtnSearch_Click(Nothing, Nothing)
        End If
    End Sub


#Region "Search Criteria Function"

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

        'Dim dtAllBrand As DataTable = udtVaccineLotBLL.GetCOVID19VaccineBrandDetail()
        Dim dtallbrand As DataTable = udtCOVID19BLL.GetCOVID19VaccineBrand()

        dtallbrand = dtallbrand.DefaultView.ToTable(True, {"Brand_Trade_Name", "Brand_ID"})

        If dtallbrand IsNot Nothing AndAlso dtallbrand.Rows.Count > 0 Then
            ddl.Items.Clear()
            ddl.DataSource = dtallbrand
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

    Private Sub BuildVaccineLotRecordStatus(ByVal ddl As DropDownList)

        ddl.DataSource = Status.GetDescriptionListFromDBEnumCode("VaccineLotCreationRecordStatus")
        ddl.DataValueField = "Status_Value"
        ddl.DataTextField = "Status_Description"
        ddl.DataBind()
        'ddl.Items.Remove(ddl.Items.FindByValue(VaccineLotDetailRecordStatus.Remove))
        ddl.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))
        ddl.SelectedIndex = 0
    End Sub

    Private Sub BuildVaccineLotAssignStatus(ByVal ddl As DropDownList)

        ddl.DataSource = Status.GetDescriptionListFromDBEnumCode("VaccineLotAssignStatus")
        ddl.DataValueField = "Status_Value"
        ddl.DataTextField = "Status_Description"
        ddl.DataBind()
        ddl.Items.Remove(ddl.Items.FindByValue(VaccineLotDetailRecordStatus.Remove))
        ddl.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), ""))
        ddl.SelectedIndex = 0
    End Sub

    Private Sub InitializeDataValue()
        ResetAllInputPageBind()
        ResetAllInputPageErrImage()

        BuildBrand(Me.ddlVaccineBrandName, False)
        BuildVaccineLotAssignStatus(ddlVaccineLotAssignStatus)
    End Sub

    Private Sub ResetAllInputPageBind()
        ddlVaccineBrandName.Items.Clear()
        txtVaccineLotNo.Text = String.Empty
        txtVaccineExpiryDateText.Text = String.Empty
        ddlVaccineLotAssignStatus.Items.Clear()
    End Sub

    Private Sub ResetAllInputPageErrImage()
        imgVaccineBrandNameErr.Visible = False
        imgVaccineLotNoErr.Visible = False
        imgVaccineExpiryDateTextErr.Visible = False
        imgLotAssignErr.Visible = False
    End Sub

    Private Sub ResetAllHiddenField()
        hfEVaccineLotNo.Text = String.Empty
        hfEVaccineLotBrand.Text = String.Empty
    End Sub

#End Region

#Region "Popup box events"

    Private Sub ibtnDialogConfirm_Click(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUpConfirmRemove.ButtonClick

        'udcInfoBox.Visible = True
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        '  Dim udtVaccineLotList As VaccineLotModel = Nothing
        Dim blnResult As New Boolean
        Dim strAction As String = String.Empty
        msgBox.Visible = False
        udcInfoBox.Visible = False

        Select Case e
            Case ucNoticePopUp.enumButtonClick.OK
                udtAuditLogEntry.AddDescripton("Vaccine Lot No.", hfEVaccineLotNo.Text.Trim)
                udtAuditLogEntry.AddDescripton("Vaccine Brand.", hfEVaccineLotBrand.Text.Trim)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00014, AuditMsg00014)
                EditRecordRemove()
            Case Else
                udtAuditLogEntry.WriteLog(LogID.LOG00015, AuditMsg00015)
        End Select
    End Sub

    Private Sub ibtnDialogCancel_Click(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUpConfirmCancel.ButtonClick
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        msgBox.Visible = False
        udcInfoBox.Visible = False

        Select Case e
            Case ucNoticePopUp.enumButtonClick.OK
                udtAuditLogEntry.AddDescripton("Vaccine Lot No.", hfEVaccineLotNo.Text.Trim)
                udtAuditLogEntry.AddDescripton("Vaccine Brand.", hfEVaccineLotBrand.Text.Trim)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00009, AuditMsg00009)
                EditRecordCancelRequest()
            Case Else
                udtAuditLogEntry.WriteLog(LogID.LOG00010, AuditMsg00010)
        End Select
    End Sub

#End Region



#Region "Vaccine Lot Record Event"
    Protected Sub ibtnRecordSave_Click(sender As Object, e As ImageClickEventArgs)


        msgBox.Visible = False
        udcInfoBox.Visible = False
        ResetAllInputPageErrImage()

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        If ViewState("state") = StateType.ADD Then
            udtAuditLogEntry.AddDescripton("Brand", ddlVaccineBrandName.SelectedItem.ToString)
            udtAuditLogEntry.AddDescripton("Vaccine Lot No.", txtVaccineLotNo.Text)
            udtAuditLogEntry.AddDescripton("Vaccine Expiry Date", txtVaccineExpiryDateText.Text)
            udtAuditLogEntry.AddDescripton("Vaccine Lot Assign Status", ddlVaccineLotAssignStatus.SelectedItem.ToString)
            udtAuditLogEntry.WriteLog(LogID.LOG00019, AuditMsg00019)
        Else
            udtAuditLogEntry.AddDescripton("Brand", lblEditBrandName.Text)
            udtAuditLogEntry.AddDescripton("Vaccine Lot No.", lblEditVaccineLotNo.Text)
            udtAuditLogEntry.AddDescripton("Vaccine Expiry Date", lblEditExpiryDateText.Text.Trim)
            udtAuditLogEntry.AddDescripton("Vaccine Lot Assign Status", ddlVaccineLotAssignStatus.SelectedItem.ToString)
            udtAuditLogEntry.WriteLog(LogID.LOG00019, AuditMsg00028)
        End If

        If (SaveRecordInfoValidation()) Then
            If ViewState("state") = StateType.ADD Then
                lblConVaccineBrandName.Text = ddlVaccineBrandName.SelectedItem.ToString
                lblConVaccineLotNo.Text = txtVaccineLotNo.Text
                lblConVaccineExpiryDateText.Text = udtFormatter.convertDate(txtVaccineExpiryDateText.Text.Trim, String.Empty)
                lblConLotAssignStatus.Text = ddlVaccineLotAssignStatus.SelectedItem.ToString
                lblConLotAssignStatusItem.Text = ddlVaccineLotAssignStatus.SelectedValue.ToString

                'sethiddentLabel for spro
                hfConVaccineBrandId.Text = ddlVaccineBrandName.SelectedValue.ToString
                lblConLotNewAssignStatus.Visible = False

                'show warning 
                lblLotNoWarning.Visible = True
                lblBrandWarning.Visible = True
                lblExpiryDateWarming.Visible = True

                ChangeViewIndex(ViewIndex.Confirm)
                udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003)
                udcInfoBox.Type = CustomControls.InfoMessageBoxType.Information
                udcInfoBox.BuildMessageBox()
            End If


            If ViewState("state") = StateType.EDIT Then
                'Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
                Dim udtVaccineLotCreationList As VaccineLotCreationModel = Nothing

                udtVaccineLotCreationList = Session(SESS_VaccineLotListModal)

                lblConVaccineBrandName.Text = lblEditBrandName.Text
                lblConVaccineLotNo.Text = lblEditVaccineLotNo.Text
                lblConVaccineExpiryDateText.Text = lblEditExpiryDateText.Text.Trim
                Status.GetDescriptionFromDBCode(VaccineLotDetailLotAssignStatus.ClassCode, udtVaccineLotCreationList.VaccineLotAssignStatus, lblConLotAssignStatus.Text, String.Empty)

                lblConLotNewAssignStatus.Visible = True

                lblConLotNewAssignStatus.Text = " >> " + ddlVaccineLotAssignStatus.SelectedItem.ToString
                lblConLotNewAssignStatus.Style.Add("color", "red")

                lblConLotAssignStatusItem.Text = ddlVaccineLotAssignStatus.SelectedValue.ToString

                'show warning 
                lblLotNoWarning.Visible = False
                lblBrandWarning.Visible = False
                lblExpiryDateWarming.Visible = False

                ChangeViewIndex(ViewIndex.Confirm)
                udcInfoBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00021)
                udcInfoBox.Type = CustomControls.InfoMessageBoxType.Information
                udcInfoBox.BuildMessageBox()

            End If



        End If

        'audit log
    End Sub

    Protected Sub ibtnRecordCancel_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        msgBox.Visible = False
        udcInfoBox.Visible = False

        If ViewState("state") = StateType.ADD Then
            Dim strVaccineBrandName As String = ddlVaccineBrandName.SelectedItem.ToString
            Dim strVaccineLotNo As String = txtVaccineLotNo.Text
            Dim strVaccineExpiryDate As String = txtVaccineExpiryDateText.Text
            udtAuditLogEntry.WriteLog(LogID.LOG00020, AuditMsg00020)
            ChangeViewIndex(ViewIndex.SearchCriteria)
        Else
            udtAuditLogEntry.WriteLog(LogID.LOG00020, AuditMsg00029)
            ChangeViewIndex(ViewIndex.EditRecordView)
        End If

    End Sub

    Protected Sub ibtnRecordBackToInput_Click(sender As Object, e As ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        msgBox.Visible = False
        udcInfoBox.Visible = False
        udtAuditLogEntry.WriteLog(LogID.LOG00022, AuditMsg00022)

        ChangeViewIndex(ViewIndex.NewEditRecord)

    End Sub

    Protected Sub ibtnRecordConfirm_Click(sender As Object, e As ImageClickEventArgs)
        Dim strVaccineLotNo As String = IIf(ViewState("state") = StateType.ADD, txtVaccineLotNo.Text, lblEditVaccineLotNo.Text)
        Dim strVaccineExpiryDate As String = String.Empty
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim blnValid As Boolean = True
        Dim strVaccineBrandName As String = String.Empty
        Dim dtLotDetail As DataTable = Nothing
        Dim dvLotDetailFilterWithRecordStatus As DataView = Nothing
        Dim dvLotDetailFilterWithPending As DataView = Nothing
        msgBox.Visible = False
        udcInfoBox.Visible = False

        If ViewState("state") = StateType.ADD Then
            strVaccineBrandName = ddlVaccineBrandName.SelectedItem.ToString()
            strVaccineExpiryDate = txtVaccineExpiryDateText.Text
        Else
            strVaccineBrandName = lblEditBrandName.Text
            strVaccineExpiryDate = lblConVaccineExpiryDateText.Text
        End If

        udtAuditLogEntry.AddDescripton("Brand", strVaccineBrandName)
        udtAuditLogEntry.AddDescripton("Vaccine Lot No.", strVaccineLotNo)
        udtAuditLogEntry.AddDescripton("Vaccine Expiry Date", strVaccineExpiryDate)
        udtAuditLogEntry.AddDescripton("Vaccine Lot Assign Status", lblConLotAssignStatusItem.Text.Trim)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00021, AuditMsg00021)

        If ViewState("state") = StateType.ADD Then
            dtLotDetail = udtVaccineLotBLL.CheckVaccineLotDetailExist(strVaccineLotNo)
            'check existing lot no in db.
            If blnValid Then
                dvLotDetailFilterWithRecordStatus = New DataView(dtLotDetail)
                'dvLotDetailFilterWithRecordStatus.RowFilter = udtVaccineLotBLL.FilterLotDetailByRecordStatus(VaccineLotDetailRecordStatus.Active)

                If dvLotDetailFilterWithRecordStatus.Count > 0 Then
                    'If udtVaccineLotBLL.CheckVaccineLotDetailExist(txtVaccineLotNo.Text.Trim, VaccineLotDetailRecordStatus.Active, String.Empty, String.Empty) Then
                    'show message for invalid
                    msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001))
                    imgVaccineLotNoErr.Visible = True
                    blnValid = False

                End If
            End If

            'check any pending record on the LotDetail
            If blnValid Then
                dvLotDetailFilterWithPending = New DataView(dtLotDetail)
                dvLotDetailFilterWithPending.RowFilter = udtVaccineLotBLL.FilterLotDetailByNewRecordStatus(VaccineLotDetailRecordStatus.Pending)

                If dvLotDetailFilterWithPending.Count > 0 Then
                    'If udtVaccineLotBLL.CheckVaccineLotDetailExist(strVaccineLotNo, String.Empty, VaccineLotDetailRecordStatus.Pending, String.Empty) Then
                    msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002))
                    imgVaccineLotNoErr.Visible = True
                    blnValid = False

                End If
            End If
        End If

        If blnValid Then
            Dim udtVaccineLotCreationList As VaccineLotCreationModel = New VaccineLotCreationModel()
            Dim udtExistedVaccineLot As VaccineLotCreationModel = New VaccineLotCreationModel()
            Dim udtVaccineLotCreationListPresent As VaccineLotCreationModel = Session(SESS_VaccineLotListModal)

            udtExistedVaccineLot = udtVaccineLotBLL.GetVaccineLotCreationModalByLotNo(lblConVaccineLotNo.Text, String.Empty)

            udtVaccineLotCreationList.BrandId = hfConVaccineBrandId.Text
            udtVaccineLotCreationList.VaccineLotNo = lblConVaccineLotNo.Text
            udtVaccineLotCreationList.VaccineExpiryDate = lblConVaccineExpiryDateText.Text
            udtVaccineLotCreationList.CreateBy = udtHCVUUserBLL.GetHCVUUser.UserID
            udtVaccineLotCreationList.UpdateBy = udtHCVUUserBLL.GetHCVUUser.UserID
            udtVaccineLotCreationList.RequestBy = udtHCVUUserBLL.GetHCVUUser.UserID
            udtVaccineLotCreationList.RequestType = VaccineLotRequestType.REQUESTTYPE_NEW
            udtVaccineLotCreationList.VaccineLotAssignStatus = lblConLotAssignStatusItem.Text.Trim

            If udtExistedVaccineLot IsNot Nothing Then
                udtVaccineLotCreationList.TSMP = udtExistedVaccineLot.TSMP

            End If

            If ViewState("state") = StateType.ADD Then
                Try
                    Dim blnResult = udtVaccineLotBLL.SaveVaccineLotCreationRecord(udtVaccineLotCreationList)
                    If (blnResult) Then
                        udcInfoBox.Type = CustomControls.InfoMessageBoxType.Complete
                        udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001, "%s", strVaccineLotNo)
                        udcInfoBox.BuildMessageBox()
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00023, AuditMsg00023)
                        ChangeViewIndex(ViewIndex.MsgPage)
                    End If
                Catch ex As Exception
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00024, AuditMsg00024)
                    Throw
                End Try


            ElseIf ViewState("state") = StateType.EDIT Then
                'Dim blnResult = udtVaccineLotBLL.EditVaccineLotCreationRecord(udtVaccineLotCreationList)
                Try
                    Dim blnResult = udtVaccineLotBLL.VaccineLotCreationAction(udtVaccineLotCreationList.VaccineLotNo, VACCINELOT_ACTIONTYPE.ACTION_UPDATE, udtVaccineLotCreationListPresent.TSMP, udtVaccineLotCreationList.VaccineLotAssignStatus)
                    If (blnResult) Then
                        udcInfoBox.Type = CustomControls.InfoMessageBoxType.Complete
                        udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001, "%s", strVaccineLotNo)
                        udcInfoBox.BuildMessageBox()
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00023, AuditMsg00030)
                        ChangeViewIndex(ViewIndex.MsgPage)
                    Else
                        ViewState("action") = ActionType.Edit
                        Me.msgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
                        WriteUpdateFailedAuditLog(udtVaccineLotCreationList.VaccineLotNo)
                        ChangeViewIndex(ViewIndex.ErrorPage)
                    End If
                Catch ex As Exception
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00024, AuditMsg00031)
                    Throw
                End Try
            End If
        Else
            msgBox.Visible = True
            udcInfoBox.Visible = False
            msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00041, AuditMsg00021)
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
        udcInfoBox.Visible = False
        msgBox.Visible = False

        'check any records of service type in private or rvp on mapping and mapping request table
        Dim dtUsedCentre As DataTable = udtVaccineLotBLL.GetVaccineLotLotMappingInUseByLotNo(lblEConVaccineLotNo.Text.Trim)
        Dim dvUsedCentreFilterWithRVP As DataView = New DataView(dtUsedCentre)
        dvUsedCentreFilterWithRVP.RowFilter = udtVaccineLotBLL.FilterCentreByServiceType(False) '[Service_Type] IN ('PRIVATE', 'RVP')

        If dvUsedCentreFilterWithRVP.Count > 0 Then
            msgBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00004))
            msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00026, AuditMsg00026)
        Else
            'check any records of service type in centre on mapping and mapping request table
            Dim dvFilterWithCentre As DataView = New DataView(dtUsedCentre)
            dvFilterWithCentre.RowFilter = udtVaccineLotBLL.FilterCentreByServiceType(True) ' "[Service_Type] = 'CENTRE'" + DH clinic
            Dim dtDistinctCentreName As DataTable = dvFilterWithCentre.ToTable(True, {"Centre_Name", "centre_service_type"})

            If dtDistinctCentreName.Rows.Count > 0 Then
                'set the popup message box
                popupMsgBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00005)
                popupMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00026, AuditMsg00026)

                gvCentre.DataSource = dtDistinctCentreName
                Session(SESS_LotAssignedCentreList) = dtDistinctCentreName
                gvCentre.DataBind()
                Me.GridViewDataBind(gvCentre, dtDistinctCentreName, "Centre_Name", "ASC", False)


                popupCentreList.Show()
            Else
                ModalPopupConfirmRemove.Show()
            End If
        End If

    End Sub

    Protected Sub ibtnCloseCentreList_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    End Sub

    Protected Sub EditRecordRemove()

        Dim blnResult As New Boolean
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        msgBox.Visible = False
        udcInfoBox.Visible = False

        'Dim vcModel As VaccineLotCreationModel = udtVaccineLotBLL.GetVaccineLotCreationModalByLotNo(hfEVaccineLotNo.Text.Trim, hfEVaccineLotBrand.Text.Trim)
        Dim udtEditModel As VaccineLotCreationModel = Session(SESS_VaccineLotListModal)

        udtAuditLogEntry.AddDescripton("Vaccine Lot No.", hfEVaccineLotNo.Text.Trim)
        udtAuditLogEntry.AddDescripton("Vaccine Brand.", hfEVaccineLotBrand.Text.Trim)

        Try
            blnResult = udtVaccineLotBLL.VaccineLotCreationAction(hfEVaccineLotNo.Text, VACCINELOT_ACTIONTYPE.ACTION_EDIT, udtEditModel.TSMP, String.Empty)

            If (blnResult) Then
                udtAuditLogEntry.WriteEndLog(LogID.LOG00016, AuditMsg00016)
                udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001, "%s", hfEVaccineLotNo.Text.Trim)
                udcInfoBox.Type = CustomControls.InfoMessageBoxType.Complete
                udcInfoBox.BuildMessageBox()
                ChangeViewIndex(ViewIndex.MsgPage)
            Else
                ViewState("action") = ActionType.Remove
                Me.msgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
                WriteUpdateFailedAuditLog(hfEVaccineLotNo.Text)
                ChangeViewIndex(ViewIndex.ErrorPage)
            End If
        Catch ex As Exception
            'ViewState("action") = ActionType.Approval
            udtAuditLogEntry.WriteEndLog(LogID.LOG00017, AuditMsg00017)
            WriteUpdateFailedAuditLog(hfEVaccineLotNo.Text.Trim)
            Throw
        End Try

        'ChangeViewIndex(ViewIndex.SearchCriteria)
    End Sub

    Protected Sub EditRecordCancelRequest()

        Dim blnResult As New Boolean
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim vcModel As VaccineLotCreationModel = udtVaccineLotBLL.GetVaccineLotCreationModalByLotNo(hfEVaccineLotNo.Text.Trim, hfEVaccineLotBrand.Text.Trim)
        Dim udtEditModel As VaccineLotCreationModel = Session(SESS_VaccineLotListModal)
        msgBox.Visible = False
        udcInfoBox.Visible = False

        If vcModel.RequestType = VaccineLotRequestType.REQUESTTYPE_NEW Or vcModel.RequestType = VaccineLotRequestType.REQUESTTYPE_AMEND Or vcModel.RequestType = VaccineLotRequestType.REQUESTTYPE_REMOVE Then
            Try
                blnResult = udtVaccineLotBLL.VaccineLotCreationAction(hfEVaccineLotNo.Text, VACCINELOT_ACTIONTYPE.ACTION_CANCELREQUEST, udtEditModel.TSMP, String.Empty)
                udtAuditLogEntry.AddDescripton("Vaccine Lot No.", hfEVaccineLotNo.Text.Trim)
                udtAuditLogEntry.AddDescripton("Vaccine Brand.", hfEVaccineLotBrand.Text.Trim)

                If (blnResult) Then
                    udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00002, "%s", hfEVaccineLotNo.Text.Trim)
                    udcInfoBox.Type = CustomControls.InfoMessageBoxType.Complete
                    udcInfoBox.BuildMessageBox()
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00011, AuditMsg00011)
                    ChangeViewIndex(ViewIndex.MsgPage)
                Else
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00012, AuditMsg00012)
                    ViewState("action") = ActionType.Cancel
                    Me.msgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
                    WriteUpdateFailedAuditLog(hfEVaccineLotNo.Text)
                    ChangeViewIndex(ViewIndex.ErrorPage)
                End If
            Catch ex As Exception

                udtAuditLogEntry.WriteEndLog(LogID.LOG00012, AuditMsg00012)
                WriteUpdateFailedAuditLog(hfEVaccineLotNo.Text.Trim)
                Throw
            End Try
        Else
            'check the request approved yet
            udtAuditLogEntry.WriteEndLog(LogID.LOG00016, AuditMsg00016)
            udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00004, "%s", hfEVaccineLotNo.Text.Trim)
            udcInfoBox.Type = CustomControls.InfoMessageBoxType.Information
            udcInfoBox.BuildMessageBox()
            ChangeViewIndex(ViewIndex.MsgPage)
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

            ddlVaccineBrandName.Visible = True
            txtVaccineLotNo.Visible = True
            lblEditVaccineLotNo.Visible = False
            lblEditBrandName.Visible = False
            imgExpiry.Visible = True
            txtVaccineExpiryDateText.Visible = True
            lblEditExpiryDateText.Visible = False

        ElseIf ViewState("state") = StateType.EDIT Then
            ResetAllInputPageErrImage()
            panNewRecord.Visible = True

            ddlVaccineBrandName.Visible = False
            txtVaccineLotNo.Visible = False
            lblEditVaccineLotNo.Visible = True
            lblEditBrandName.Visible = True
            txtVaccineExpiryDateText.Visible = False
            imgExpiry.Visible = False
            lblEditExpiryDateText.Visible = True

            Dim udtVaccineLotCreationList As VaccineLotCreationModel = Nothing

            udtVaccineLotCreationList = Session(SESS_VaccineLotListModal)

            lblEditBrandID.Text = udtVaccineLotCreationList.BrandId
            lblEditBrandName.Text = udtVaccineLotCreationList.BrandTradeName
            lblEditVaccineLotNo.Text = udtVaccineLotCreationList.VaccineLotNo
            lblEditExpiryDateText.Text = udtFormatter.convertDate(Format(CDate(udtVaccineLotCreationList.VaccineExpiryDate), "dd-MM-yyyy"), String.Empty)
            ddlVaccineLotAssignStatus.SelectedValue = udtVaccineLotCreationList.VaccineLotAssignStatus

            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.AddDescripton("Vaccine Lot No", udtVaccineLotCreationList.VaccineLotNo)
            udtAuditLogEntry.AddDescripton("Brand Id", udtVaccineLotCreationList.BrandId)
            udtAuditLogEntry.AddDescripton("Brand Name", udtVaccineLotCreationList.BrandTradeName)
            udtAuditLogEntry.AddDescripton("Expiry Date", udtFormatter.convertDate(Format(CDate(udtVaccineLotCreationList.VaccineExpiryDate), "dd-MM-yyyy"), String.Empty))
            udtAuditLogEntry.AddDescripton("Lot Assign Status", udtVaccineLotCreationList.VaccineLotAssignStatus)
            udtAuditLogEntry.WriteLog(LogID.LOG00032, AuditMsg00032)

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
            Case ViewIndex.NewEditRecord
            Case ViewIndex.EditRecordView
        End Select
    End Sub

    Private Sub ResetSearchCriteria()
        ddlBrand.SelectedValue = String.Empty
        txtVaccLotNo.Text = String.Empty
        txtExpiryTo.Text = String.Empty
        txtExpiryTo.Text = String.Empty
    End Sub

    Private Sub ResetSearchCriteriaErrImage()
        imgExpiryDateFromErr.Visible = False
        imgLotAssignErr.Visible = False

    End Sub

    Private Sub gvResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvResult.RowDataBound
        Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL
        Dim udtStaticData As Common.Component.StaticData.StaticDataModel


        If e.Row.RowType = DataControlRowType.DataRow Then

            'Record Status 
            Dim lblVLStatus As Label = CType(e.Row.FindControl("lblVLStatus"), Label)
            Status.GetDescriptionFromDBCode(VaccineLotDetailRecordStatus.ClassCode, lblVLStatus.Text.Trim, lblVLStatus.Text, String.Empty)

            'Request Type
            Dim lblRequestType As Label = CType(e.Row.FindControl("lblRequestType"), Label)
            If lblRequestType.Text = String.Empty Then
                lblRequestType.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                udtStaticData = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("VaccineLotCreationRequestType", lblRequestType.Text.Trim)
                lblRequestType.Text = CStr(udtStaticData.DataValue)
                lblVLStatus.Style.Add("color", "red")
            End If

            'Requested by
            Dim lblSVLRequestBy As Label = CType(e.Row.FindControl("lblVLRequestBy"), Label)
            If lblSVLRequestBy.Text = String.Empty Then
                lblSVLRequestBy.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            'Lot Assign Status
            Dim lblVLLotAssignStatus As Label = CType(e.Row.FindControl("lblVLLotAssignStatus"), Label)
            Status.GetDescriptionFromDBCode(VaccineLotDetailLotAssignStatus.ClassCode, lblVLLotAssignStatus.Text, lblVLLotAssignStatus.Text, String.Empty)

            'New Lot assign status
            Dim lblVLNewLotAssignStatus As Label = CType(e.Row.FindControl("lblVLNewLotAssignStatus"), Label)
            Dim lblVLSymbol As Label = CType(e.Row.FindControl("lblVLSymbol"), Label)

            If lblVLNewLotAssignStatus.Text IsNot String.Empty Then
                Status.GetDescriptionFromDBCode(VaccineLotDetailLotAssignStatus.ClassCode, lblVLNewLotAssignStatus.Text.Trim, lblVLNewLotAssignStatus.Text, String.Empty)

                'show the >> as lot assign status not equal to new lot assign status
                If lblVLLotAssignStatus.Text.Trim <> lblVLNewLotAssignStatus.Text.Trim Then
                    lblVLNewLotAssignStatus.Visible = True
                    lblVLSymbol.Visible = True
                End If
            End If

            'Requested Dtm
            Dim lblVLRequestDtm As Label = CType(e.Row.FindControl("lblVLRequestDtm"), Label)
            If lblVLRequestDtm.Text = String.Empty Then
                lblVLRequestDtm.Text = Me.GetGlobalResourceObject("Text", "N/A")
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
            Dim strBrandId As String = CType(r.FindControl("hfVLBrandId"), Label).Text.Trim
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.AddDescripton("Vaccine Lot No:", strLotNo)
            udtAuditLogEntry.WriteLog(LogID.LOG00005, AuditMsg00005)
            BindVaccineSummaryView(strLotNo, strBrandId)
        End If
    End Sub

    Private Sub gvResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvResult.Sorting
        Me.GridViewSortingHandler(sender, e, SESS_SearchResultList)
    End Sub

    Private Sub gvCentre_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvCentre.PageIndexChanging
        Me.GridViewPageIndexChangingHandler(sender, e, SESS_LotAssignedCentreList)
        popupCentreList.Show()
    End Sub

    Private Sub gvCentre_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvCentre.PreRender
        Me.GridViewPreRenderHandler(sender, e, SESS_LotAssignedCentreList)
    End Sub


    Private Sub gvCentre_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvCentre.Sorting
        Me.GridViewSortingHandler(sender, e, SESS_LotAssignedCentreList)
        popupCentreList.Show()
    End Sub


    Protected Sub BindVaccineSummaryView(ByVal strVLNo As String, ByVal strBrandId As String)
        Dim udtStaticDataBLL As Common.Component.StaticData.StaticDataBLL
        Dim udtStaticData As Common.Component.StaticData.StaticDataModel
        Dim blnExpired As Boolean = False

        'Initialize
        trinfoRequestedBy.Visible = False
        trinfoRequestType.Visible = False
        trinfoApprovedBy.Visible = False

        Dim udtVaccineLotCreationList As VaccineLotCreationModel = Nothing
        hfEVaccineLotNo.Text = strVLNo
        hfEVaccineLotBrand.Text = strBrandId
        udtVaccineLotCreationList = udtVaccineLotBLL.GetVaccineLotCreationModalByLotNo(strVLNo, strBrandId)

        Session(SESS_VaccineLotListModal) = udtVaccineLotCreationList

        ChangeViewIndex(ViewIndex.EditRecordView)


        lblEConVaccineBrandName.Text = udtVaccineLotCreationList.BrandName
        lblEConVaccineBrandTradeName.Text = udtVaccineLotCreationList.BrandTradeName
        lblEConVaccineLotNo.Text = udtVaccineLotCreationList.VaccineLotNo
        lblEConVaccineExpiryDateText.Text = udtFormatter.formatDisplayDate(CDate(udtVaccineLotCreationList.VaccineExpiryDate))

        'Lot assign status & new lot assign status
        Status.GetDescriptionFromDBCode(VaccineLotDetailLotAssignStatus.ClassCode, udtVaccineLotCreationList.VaccineLotAssignStatus, lblEConVaccineLotAssignStatus.Text, String.Empty)
        Status.GetDescriptionFromDBCode(VaccineLotDetailLotAssignStatus.ClassCode, udtVaccineLotCreationList.NewVaccineLotAssignStatus, lblEConNewVaccineLotAssignStatus.Text, String.Empty)

        If udtVaccineLotCreationList.NewVaccineLotAssignStatus IsNot String.Empty Then
            'check to show >> or not
            If lblEConNewVaccineLotAssignStatus.Text.Trim <> lblEConVaccineLotAssignStatus.Text.Trim Then
                lblEConNewVaccineLotAssignStatus.Text = " >> " + lblEConNewVaccineLotAssignStatus.Text
                lblEConNewVaccineLotAssignStatus.Style.Add("color", "red")
                lblEConNewVaccineLotAssignStatus.Visible = True
            Else
                lblEConNewVaccineLotAssignStatus.Visible = False
            End If
        Else
            lblEConNewVaccineLotAssignStatus.Visible = False
        End If

        lblCreatedBy.Text = udtVaccineLotCreationList.CreateBy + " (" + Format(CDate(udtVaccineLotCreationList.CreateDtm), "dd MMM yyyy HH:mm") + ")"
        lblEConVaccineNewRecordStatus.Text = udtVaccineLotCreationList.RecordStatus
        Status.GetDescriptionFromDBCode(VaccineLotDetailRecordStatus.ClassCode, lblEConVaccineNewRecordStatus.Text, lblEConVaccineNewRecordStatus.Text, String.Empty)

        'Pending record
        If udtVaccineLotCreationList.RecordStatus = VaccineLotDetailRecordStatus.Pending Then

            trinfoRequestedBy.Visible = True
            'Request Type
            If udtVaccineLotCreationList.RequestType <> String.Empty Then
                trinfoRequestType.Visible = True
                lblERequestType.Text = udtVaccineLotCreationList.RequestType
                udtStaticDataBLL = New Common.Component.StaticData.StaticDataBLL()
                udtStaticData = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("VaccineLotCreationRequestType", lblERequestType.Text)
                lblERequestType.Text = CStr(udtStaticData.DataValue)
            End If
            'Requested By
            lblERequestedBy.Text = udtVaccineLotCreationList.RequestBy + " (" + Format(CDate(udtVaccineLotCreationList.RequestDtm), "dd MMM yyyy HH:mm") + ")"

        Else
            If udtVaccineLotCreationList.ApproveBy IsNot String.Empty Then
                trinfoApprovedBy.Visible = True
            End If
            'approved by
            lblEApprovedBy.Text = udtVaccineLotCreationList.ApproveBy + " (" + Format(CDate(udtVaccineLotCreationList.ApproveDtm), "dd MMM yyyy HH:mm") + ")"
        End If

        'pending / remove / expired record
        If udtVaccineLotCreationList.RecordStatus = VaccineLotDetailRecordStatus.Remove Or udtVaccineLotCreationList.RecordStatus = VaccineLotDetailRecordStatus.Expired Or udtVaccineLotCreationList.RecordStatus = VaccineLotDetailRecordStatus.Pending Then
            ibtnEditRecordRemove.Enabled = False
            ibtnEditRecordRemove.ImageUrl = GetGlobalResourceObject("ImageUrl", "RemoveDisableBtn")
            If udtHCVUUserBLL.GetHCVUUser.UserID = udtVaccineLotCreationList.RequestBy And udtVaccineLotCreationList.RecordStatus = VaccineLotDetailRecordStatus.Pending Then
                ibtnEditRecordCancel.Enabled = True
                ibtnEditRecordCancel.ImageUrl = GetGlobalResourceObject("ImageUrl", "CancelRequestBtn")
            Else
                ibtnEditRecordCancel.Enabled = False
                ibtnEditRecordCancel.ImageUrl = GetGlobalResourceObject("ImageUrl", "CancelDisableRequestBtn")
            End If
            ibtnEditRecordEdit.Enabled = False
            ibtnEditRecordEdit.ImageUrl = GetGlobalResourceObject("ImageUrl", "EditDisableBtn")
        Else
            'normal active record
            ibtnEditRecordRemove.Enabled = True
            ibtnEditRecordRemove.ImageUrl = GetGlobalResourceObject("ImageUrl", "RemoveBtn")
            ibtnEditRecordCancel.Enabled = False
            ibtnEditRecordCancel.ImageUrl = GetGlobalResourceObject("ImageUrl", "CancelDisableRequestBtn")
            ibtnEditRecordEdit.Enabled = True
            ibtnEditRecordEdit.ImageUrl = GetGlobalResourceObject("ImageUrl", "EditBtn")
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
        Dim strTodayForCutOffChecking As String = (DateTime.Now).ToString("dd-MM-yyyy")
        Dim dtLotDetail As DataTable = Nothing
        Dim dvLotDetailFilterWithRecordStatus As DataView = Nothing
        Dim dvLotDetailFilterWithPending As DataView = Nothing

        imgVaccineBrandNameErr.Visible = False
        imgVaccineLotNoErr.Visible = False
        imgVaccineExpiryDateTextErr.Visible = False

        If ViewState("state") = StateType.ADD Then
            'Check Mandatory Fields
            If ddlVaccineBrandName.Text.Trim = String.Empty Then
                msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00367), "%s", GetGlobalResourceObject("Text", "VaccineName"))
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
            If ddlVaccineLotAssignStatus.Text.Trim = String.Empty Then
                msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00367), "%s", GetGlobalResourceObject("Text", "VaccineLotAssignStatus"))
                imgLotAssignErr.Visible = True
                blnValid = False
            End If

            'check the date format
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

            'check existing on lot detail to prevent duplicate
            dtLotDetail = udtVaccineLotBLL.CheckVaccineLotDetailExist(txtVaccineLotNo.Text.Trim)

            If (blnValid) Then
                If dtLotDetail.Rows.Count > 0 Then
                    'show message for invalid
                    msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001))
                    imgVaccineLotNoErr.Visible = True
                    blnValid = False
                End If

            End If

            'check pending on lot detail
            If (blnValid) Then
                dvLotDetailFilterWithPending = New DataView(dtLotDetail)
                dvLotDetailFilterWithPending.RowFilter = udtVaccineLotBLL.FilterLotDetailByNewRecordStatus(VaccineLotDetailRecordStatus.Pending)

                'If udtVaccineLotBLL.CheckVaccineLotDetailExist(txtVaccineLotNo.Text.Trim, String.Empty, VaccineLotDetailRecordStatus.Pending, String.Empty) Then
                If dvLotDetailFilterWithPending.Count > 0 Then
                    'show message for invalid
                    msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002))
                    imgVaccineLotNoErr.Visible = True
                    blnValid = False
                End If

            End If

        End If

        If ViewState("state") = StateType.EDIT Then
            If ddlVaccineLotAssignStatus.Text.Trim = String.Empty Then
                msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00367), "%s", GetGlobalResourceObject("Text", "VaccineLotAssignStatus"))
                imgLotAssignErr.Visible = True
                blnValid = False
            End If

            'check any changes on the assign status
            Dim udtVaccineLotCreationList As VaccineLotCreationModel = Nothing

            udtVaccineLotCreationList = Session(SESS_VaccineLotListModal)
            If blnValid Then
                If ddlVaccineLotAssignStatus.SelectedValue = udtVaccineLotCreationList.VaccineLotAssignStatus Then
                    msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00003))
                    imgLotAssignErr.Visible = True
                    blnValid = False
                    blnValidDate = False
                End If
            End If

        End If

        If blnValidDate = False Then
            blnValid = blnValidDate
        End If

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
