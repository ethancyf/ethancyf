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
Imports Common.Component.VaccineLot.VaccineLotBLL

Partial Public Class VaccineLotCreationApproval
    Inherits BasePageWithControl

    Public Enum ActionType
        Approval = 0
        Reject = 1

    End Enum

    Public Enum ViewIndex
        SearchResult = 0
        Detail = 1
        MsgPage = 2
        ErrorPage = 3
    End Enum

#Region "Fields"
    Private udtVaccineLotBLL As New VaccineLotBLL
    Private udtHCVUUserBLL As New HCVUUserBLL

    Private udtFormatter As New Formatter
    Private udtValidator As New Validator

    Private SM As SystemMessage
    Private udtAuditLogEntry As AuditLogEntry

    Private Const SESS_SearchResultList As String = FunctCode.FUNT010425 & "VaccineLotCreationList_SearchResultList"
    Private Const SESS_VaccineLotListModal As String = FunctCode.FUNT010425 & "VaccineLotCreationList_Modal"
    Private Const SESS_LotAssignedCentreList As String = FunctCode.FUNT010425 & "VaccineLotCreationList_AssignedCentreList"

    Private Const CentreServiceType As String = "CENTRE"
    Private Const PrivateServiceType As String = "PRIVATE"
    Private Const RVPServiceType As String = "RVP"
#End Region

#Region "Audit Log Message"
    Private Const AuditMsg00000 As String = "Vaccine Lot Approval loaded"
    Private Const AuditMsg00001 As String = "Generate the approval grid view"
    Private Const AuditMsg00002 As String = "Search completed. No record found"
    Private Const AuditMsg00003 As String = "Search completed"
    Private Const AuditMsg00004 As String = "Search failed"
    Private Const AuditMsg00005 As String = "Search Result Page -  Click View Detail"
    Private Const AuditMsg00006 As String = "Select failed: Vaccine Lot Creation List object not found"
    Private Const AuditMsg00007 As String = "Select completed"
    Private Const AuditMsg00008 As String = "Detail Page - Back Click"
    Private Const AuditMsg00009 As String = "Detail Page - Back Click failed"
    Private Const AuditMsg00010 As String = "Approval Click"
    Private Const AuditMsg00011 As String = "Reject Click"
    Private Const AuditMsg00012 As String = "Return Click"
    Private Const AuditMsg00013 As String = "Confirm Approval - Yes Click"
    Private Const AuditMsg00014 As String = "Confirm Approval - Yes Click successed"
    Private Const AuditMsg00015 As String = "Approval updated failed"
    Private Const AuditMsg00016 As String = "Confirm Approval - No Click"
    Private Const AuditMsg00017 As String = "Confirm Reject - Yes Click"
    Private Const AuditMsg00018 As String = "Confirm Reject  - Yes Click successed"
    Private Const AuditMsg00019 As String = "Reject updated failed"
    Private Const AuditMsg00020 As String = "Confirm Reject - No Click"

    Private Const AuditMsg00021 As String = "Approval for Remove Lot failed (used in Tx)"
    Private Const AuditMsg00022 As String = "Expiry date should not be early than today"
    Private Const AuditMsg00023 As String = "Failed Validation on Approval"

    Private Const AuditMsg00024 As String = "Validation failed"
    Private Const AuditMsg00025 As String = "Select failed: Vaccine Lot Creation List object not under pending"
    Private Const AuditMsg00026 As String = "Failed Validation on Reject"
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
        Dim bllSearchResult As Common.Component.BaseBLL.BLLSearchResult

        bllSearchResult = udtVaccineLotBLL.GetVaccineLotCreationSearch(String.Empty, _
                                                               String.Empty, String.Empty, _
                                                             String.Empty, _
                                                               VaccineLotRecordConstantType.VaccineLotRecordStatus_PendingApproval)

        Return bllSearchResult
    End Function

    Protected Overrides Function SF_BindSearchResult(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry, ByVal udtBLLSearchResult As Common.Component.BaseBLL.BLLSearchResult) As Integer
        Dim dt As DataTable
        Dim intRowCount As Integer
        Dim blnShowResultList As Boolean = False

        Try
            dt = CType(udtBLLSearchResult.Data, DataTable)
            Session(SESS_SearchResultList) = dt
        Catch ex As Exception
            Throw
        End Try

        intRowCount = dt.Rows.Count

        Select Case dt.Rows.Count
            Case 0
                ' No record found
                blnShowResultList = False
                udtAuditLogEntry.WriteEndLog(LogID.LOG00002, AuditMsg00002)
                ibtnMsgBack.Visible = False
                gvResult.DataSource = dt
                gvResult.DataBind()
            Case Else
                blnShowResultList = True

        End Select

        If blnShowResultList Then
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


        If Not Page.IsPostBack Then
            FunctionCode = FunctCode.FUNT010425

            ' Write Audit Log
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, AuditMsg00000)

            SearchCreationList()


        End If

        Me.ModalPopupConfirmApproval.PopupDragHandleControlID = Me.ucNoticePopUpConfirmApproval.Header.ClientID
        Me.ModalPopupConfirmReject.PopupDragHandleControlID = Me.ucNoticePopUpConfirmReject.Header.ClientID


    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

    End Sub

#End Region

    Protected Sub SearchCreationList()
        udcInfoBox.Visible = False
        msgBox.Visible = False

        Dim enumSearchResult As SearchResultEnum


        ' Try-catch to avoid search over row limit (500)
        Try
            ' Write Audit Log
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.AddDescripton("Vaccine Lot Creation Approval", "Generate grid view")

            udtAuditLogEntry.WriteStartLog(LogID.LOG00001, AuditMsg00001)


            enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, msgBox, udcInfoBox, False, True)


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

                Case Else
                    Throw New Exception("Error: Class = [HCVU.Vaccine Lot Creation Approval], Method = [ibtnSearch_Click], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")

            End Select

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                msgBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, eSQL.Message))

                If msgBox.GetCodeTable.Rows.Count = 0 Then
                    msgBox.Visible = False
                Else
                    msgBox.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00004, AuditMsg00004)
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00004, AuditMsg00004)
                End If

            Else
                Throw eSQL
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub BindVLSummaryView(ByVal strVLNo As String, ByVal strVLBrand As String)
        udcInfoBox.Visible = False
        msgBox.Visible = False
        If IsNothing(strVLNo) OrElse strVLNo.Trim = String.Empty Then Return

        Dim blnNotFind As Boolean = False
        Dim blnValid As Boolean = True

        Dim udtVaccineLotList As VaccineLotCreationModel = Nothing

        udtVaccineLotList = udtVaccineLotBLL.GetVaccineLotCreationModalByLotNo(strVLNo, strVLBrand)
        'Not found
        If IsNothing(udtVaccineLotList) Then
            blnNotFind = True
        End If

        'not in pending request
        If Not blnNotFind Then
            If udtVaccineLotList.RequestType Is String.Empty Then
                blnValid = False
            End If
        End If

        ' Write Audit Log
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Vaccine Lot No", strVLNo)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00005, AuditMsg00005)

        If blnNotFind Then
            msgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
            msgBox.BuildMessageBox("SelectFail", udtAuditLogEntry, LogID.LOG00006, AuditMsg00006)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00006, AuditMsg00006)
            ChangeViewIndex(ViewIndex.ErrorPage)
        ElseIf blnValid Then
            'check user 
            Dim udtHCVUUser As HCVUUserModel
            Dim udtHCVUUserBLL As New HCVUUserBLL
            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            'disable the approval and reject button 
            If (udtHCVUUser.UserID = udtVaccineLotList.RequestBy.Trim) Or (udtVaccineLotList.RequestType = String.Empty) Or udtVaccineLotList.NewRecordStatus = VaccineLotDetailRequestType.REQUESTTYPE_AMEND Then

                ibtnApproval.ImageUrl = GetGlobalResourceObject("ImageUrl", "ApproveDisableBtn")
                ibtnReject.ImageUrl = GetGlobalResourceObject("ImageUrl", "RejectDisabledBtn")
                ibtnApproval.Enabled = False
                ibtnReject.Enabled = False

                udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003)
                udcInfoBox.Type = CustomControls.InfoMessageBoxType.Information
                udcInfoBox.BuildMessageBox()

                ChangeViewIndex(ViewIndex.MsgPage)

            Else
                ibtnApproval.ImageUrl = GetGlobalResourceObject("ImageUrl", "ApproveBtn")
                ibtnReject.ImageUrl = GetGlobalResourceObject("ImageUrl", "RejectBtn")
                ibtnApproval.Enabled = True
                ibtnReject.Enabled = True
            End If


            BindVaccineLotDetail(udtVaccineLotList)

            ChangeViewIndex(ViewIndex.Detail)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00007, AuditMsg00007)
        Else
            msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00005))
            msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00025, AuditMsg00025)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00025, AuditMsg00025)
            ChangeViewIndex(ViewIndex.ErrorPage)
        End If

    End Sub

    Protected Sub ibtnBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00008, AuditMsg00008)

        msgBox.Visible = False
        udcInfoBox.Visible = False
        BackToSearchCriteriaView(True)
        ChangeViewIndex(ViewIndex.SearchResult)
    End Sub

    Protected Sub ibtnErrorBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00009, AuditMsg00009)

        msgBox.Visible = False
        udcInfoBox.Visible = False

        BackToSearchCriteriaView(True)
        'Dim dt As New DataTable
        'dt = Session(SESS_SearchResultList)
        'gvResult.DataSource = dt
        'gvResult.DataBind()
        'gvResult.Visible = True
        'Me.GridViewDataBind(gvResult, dt, "Brand_Trade_Name", "ASC", False)

        ChangeViewIndex(ViewIndex.SearchResult)
    End Sub

    Protected Sub ibtnMsgBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00012, AuditMsg00012)

        BackToSearchCriteriaView(True)
    End Sub

    Protected Sub ibtnApproval_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Vaccine Lot No", lblDetailVaccineLotNo.Text.Trim)
        udtAuditLogEntry.WriteLog(LogID.LOG00010, AuditMsg00010)

        Me.ModalPopupConfirmApproval.Show()
    End Sub


    Protected Sub ibtnReject_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Vaccine Lot No", lblDetailVaccineLotNo.Text.Trim)
        udtAuditLogEntry.WriteLog(LogID.LOG00011, AuditMsg00011)

        Me.ModalPopupConfirmReject.Show()
    End Sub

    Private Sub WriteUpdateFailedAuditLog(ByVal strVaccineLotNo As String)
        If msgBox.GetCodeTable.Rows.Count = 0 Then
            msgBox.Visible = False
        Else
            Dim strAction As String = ViewState("action")
            Dim udtExAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtExAuditLogEntry.AddDescripton("Vaccine Lot No", strVaccineLotNo)
            Select Case strAction

                Case ActionType.Approval
                    msgBox.BuildMessageBox("UpdateFail", udtExAuditLogEntry, LogID.LOG00017, AuditMsg00015)
                Case ActionType.Reject
                    msgBox.BuildMessageBox("UpdateFail", udtExAuditLogEntry, LogID.LOG00018, AuditMsg00019)
            End Select

        End If
    End Sub

    Private Sub BackToSearchCriteriaView(ByVal blnIsReset As Boolean)
        udcInfoBox.Visible = False
        msgBox.Visible = False
        SF_Search(Nothing, False)
        'ChangeViewIndex(ViewIndex.SearchResult)

        SearchCreationList()
    End Sub

    Private Sub BindVaccineLotDetail(ByVal udtVaccineLotList As VaccineLotCreationModel)
        Dim udtStaticDataBLL As Common.Component.StaticData.StaticDataBLL
        Dim udtStaticData As Common.Component.StaticData.StaticDataModel

        Session(SESS_VaccineLotListModal) = udtVaccineLotList


        lblDetailBrandName.Text = udtVaccineLotList.BrandTradeName
        lblDetailVaccineLotNo.Text = udtVaccineLotList.VaccineLotNo
        lblDetailExpiryD.Text = udtFormatter.formatDisplayDate(CDate(udtVaccineLotList.VaccineExpiryDate))

        'record status
        lblDetailRecordStatus.Text = udtVaccineLotList.NewRecordStatus
        Status.GetDescriptionFromDBCode(VaccineLotDetailRecordStatus.ClassCode, lblDetailRecordStatus.Text, lblDetailRecordStatus.Text, String.Empty)
        
        lblDetailRequestType.Text = udtVaccineLotList.RequestType
        udtStaticDataBLL = New Common.Component.StaticData.StaticDataBLL()
        udtStaticData = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("VaccineLotCreationRequestType", lblDetailRequestType.Text)
        lblDetailRequestType.Text = CStr(udtStaticData.DataValue)

        'lot assign status
        lblDetailLotAssignStatus.Text = udtVaccineLotList.VaccineLotAssignStatus
        Status.GetDescriptionFromDBCode(VaccineLotDetailLotAssignStatus.ClassCode, lblDetailLotAssignStatus.Text.Trim, lblDetailLotAssignStatus.Text, String.Empty)

        'new lot assign status
        lblDetailNewLotAssignStatus.Text = udtVaccineLotList.NewVaccineLotAssignStatus
        If udtVaccineLotList.NewVaccineLotAssignStatus IsNot String.Empty Then
            Status.GetDescriptionFromDBCode(VaccineLotDetailLotAssignStatus.ClassCode, lblDetailNewLotAssignStatus.Text.Trim, lblDetailNewLotAssignStatus.Text, String.Empty)

            If lblDetailLotAssignStatus.Text.Trim <> lblDetailNewLotAssignStatus.Text.Trim Then
                lblDetailNewLotAssignStatus.Text = " >> " + lblDetailNewLotAssignStatus.Text
                lblDetailNewLotAssignStatus.Visible = True
                lblDetailNewLotAssignStatus.Style.Add("color", "red")
            End If
        End If

        lblDetailRequestedBy.Text = udtVaccineLotList.RequestBy + " (" + Format(CDate(udtVaccineLotList.RequestDtm), "dd MMM yyyy HH:mm") + ")"
        lblDetailCreatedBy.Text = udtVaccineLotList.CreateBy + " (" + Format(CDate(udtVaccineLotList.CreateDtm), "dd MMM yyyy HH:mm") + ")"

    End Sub



#Region "Popup box events"

    Private Sub ucNoticePopUpConfirmApproval_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUpConfirmApproval.ButtonClick

        'msgBox.Visible = True
        udcInfoBox.Visible = False
        msgBox.Visible = False
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim blnResult As New Boolean
        Dim strAction As String = String.Empty
        Dim blnValid As Boolean = True
        'Dim udtSystemMessage As SystemMessage
        Dim strToday As String = DateTime.Now.ToString("MM/dd/yyyy")
        Dim udtVaccineLotList As VaccineLotCreationModel
        Dim dtlotDetail As DataTable = Nothing
        Dim dvLotDetailFilterWithRecordStatus As DataView = Nothing
        Dim dvLotDetailFilterWithNewRecordStatus As DataView = Nothing

        udtVaccineLotList = Session(SESS_VaccineLotListModal)

        Select Case e
            Case ucNoticePopUp.enumButtonClick.OK
                dtlotDetail = udtVaccineLotBLL.CheckVaccineLotDetailExist(lblDetailVaccineLotNo.Text.Trim)

                'check the record still under pending request
                dvLotDetailFilterWithNewRecordStatus = New DataView(dtlotDetail)
                dvLotDetailFilterWithNewRecordStatus.RowFilter = udtVaccineLotBLL.FilterLotDetailByNewRecordStatus(VaccineLotDetailRecordStatus.Pending)

                If dvLotDetailFilterWithNewRecordStatus.Count < 1 Then
                    'show message for invalid
                    msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00005))
                    blnValid = False
                End If

                If udtVaccineLotList.RequestType = VaccineLotDetailRequestType.REQUESTTYPE_NEW And blnValid Then
                    'check exist in lotdetail table 
                    dvLotDetailFilterWithRecordStatus = New DataView(dtlotDetail)
                    dvLotDetailFilterWithRecordStatus.RowFilter = udtVaccineLotBLL.FilterLotDetailByRecordStatus(VaccineLotDetailRecordStatus.Active)

                    If dvLotDetailFilterWithRecordStatus.Count > 0 Then
                        'show message for invalid
                        msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001))
                        blnValid = False
                    End If
                End If

                'check the expiry date not past date
                If (udtVaccineLotList.RequestType = VaccineLotDetailRequestType.REQUESTTYPE_NEW Or udtVaccineLotList.RequestType = VaccineLotDetailRequestType.REQUESTTYPE_AMEND) And blnValid Then
                    Dim dtmDate As Date = udtFormatter.convertDate(Format(CDate(udtVaccineLotList.VaccineExpiryDate), "dd-MM-yyyy"), String.Empty)
                    If dtmDate < DateTime.Today.Date Then
                        msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00004))
                        blnValid = False
                    End If
                End If

                If udtVaccineLotList.RequestType = VaccineLotDetailRequestType.REQUESTTYPE_REMOVE And blnValid Then

                    'check any records of service type in private or rvp on mapping and mapping request table
                    Dim dtUsedCentre As DataTable = udtVaccineLotBLL.GetVaccineLotLotMappingInUseByLotNo(lblDetailVaccineLotNo.Text.Trim)
                    Dim dvUsedCentreFilterWithRVP As DataView = New DataView(dtUsedCentre)
                    dvUsedCentreFilterWithRVP.RowFilter = udtVaccineLotBLL.FilterCentreByServiceType(False)

                    If dvUsedCentreFilterWithRVP.Count > 0 Then
                        msgBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002))
                        blnValid = False
                    Else
                        'check any records of service type in centre on mapping and mapping request table
                        Dim dvFilterWithCentre As DataView = New DataView(dtUsedCentre)
                        dvFilterWithCentre.RowFilter = udtVaccineLotBLL.FilterCentreByServiceType(True) ' "[Service_Type] = 'CENTRE'"
                        Dim dtDistinctCentreName As DataTable = dvFilterWithCentre.ToTable(True, {"Centre_Name", "centre_service_type"})

                        If dtDistinctCentreName.Rows.Count > 0 Then
                            'set the popup message box
                            popupMsgBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00003)
                            popupMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00024, AuditMsg00024)

                            gvCentre.DataSource = dtDistinctCentreName
                            Session(SESS_LotAssignedCentreList) = dtDistinctCentreName
                            gvCentre.DataBind()
                            Me.GridViewDataBind(gvCentre, dtDistinctCentreName, "Centre_Name", "ASC", False)

                            blnValid = False
                            popupCentreList.Show()
                        Else
                            blnValid = True
                        End If
                    End If

                End If

                If blnValid Then
                    ApproveLotRecord()
                Else
                    msgBox.Visible = True
                    udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
                    msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00026, AuditMsg00023)

                End If
            Case Else 'cancel click             
                udtAuditLogEntry.WriteEndLog(LogID.LOG00016, AuditMsg00016)
        End Select


    End Sub

    Protected Sub ibtnCloseCentreList_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    End Sub

    Private Sub ucNoticePopUpConfirmReject_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUpConfirmReject.ButtonClick
        Dim udtVaccineLotCreation As VaccineLotCreationModel = Session(SESS_VaccineLotListModal)

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim dvLotDetailFilterWithNewRecordStatus As DataView = Nothing
        Dim blnResult As New Boolean
        Dim blnvalid As Boolean = True
        ViewState("action") = ActionType.Reject

        udcInfoBox.Visible = False
        msgBox.Visible = False

        Select Case e
            Case ucNoticePopUp.enumButtonClick.OK
                'check the record still under pending request
                dvLotDetailFilterWithNewRecordStatus = New DataView(udtVaccineLotBLL.CheckVaccineLotDetailExist(lblDetailVaccineLotNo.Text.Trim))
                dvLotDetailFilterWithNewRecordStatus.RowFilter = udtVaccineLotBLL.FilterLotDetailByNewRecordStatus(VaccineLotDetailRecordStatus.Pending)

                If dvLotDetailFilterWithNewRecordStatus.Count < 1 Then
                    'show message for invalid
                    msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00005))
                    blnvalid = False
                End If

                If blnvalid Then
                    udtAuditLogEntry.WriteStartLog(LogID.LOG00017, AuditMsg00017)
                    blnResult = udtVaccineLotBLL.VaccineLotCreationAction(lblDetailVaccineLotNo.Text, VACCINELOT_ACTIONTYPE.ACTION_REJECT, udtVaccineLotCreation.TSMP, String.Empty)

                    If (blnResult) Then
                        udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00002, "%s", lblDetailVaccineLotNo.Text.Trim)
                        udcInfoBox.Type = CustomControls.InfoMessageBoxType.Complete
                        udcInfoBox.BuildMessageBox()
                        ChangeViewIndex(ViewIndex.MsgPage)
                    Else
                        Me.msgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
                        WriteUpdateFailedAuditLog(lblDetailVaccineLotNo.Text.Trim)
                        ChangeViewIndex(ViewIndex.ErrorPage)
                    End If
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00018, AuditMsg00018)
                Else
                    udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
                    msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00026, AuditMsg00026)
                End If
            Case Else
                WriteUpdateFailedAuditLog(lblDetailVaccineLotNo.Text.Trim)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00020, AuditMsg00020)
        End Select

    End Sub


#End Region

    Private Sub ChangeViewIndex(ByVal udtViewIndex As ViewIndex)
        MultiViewEnquiry.ActiveViewIndex = udtViewIndex

        Select Case udtViewIndex
            Case ViewIndex.SearchResult
                Session.Remove(SESS_VaccineLotListModal)
            Case ViewIndex.Detail
            Case ViewIndex.MsgPage
            Case ViewIndex.ErrorPage
        End Select
    End Sub

    Private Sub gvResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvResult.RowDataBound
        Dim udtStaticDataBLL As Common.Component.StaticData.StaticDataBLL = New Common.Component.StaticData.StaticDataBLL()
        Dim udtStaticData As Common.Component.StaticData.StaticDataModel

        If e.Row.RowType = DataControlRowType.DataRow Then

            'Record Status 
            Dim lblVLRecordStatus As Label = CType(e.Row.FindControl("lblVLRecordStatus"), Label)
            Status.GetDescriptionFromDBCode(VaccineLotDetailRecordStatus.ClassCode, lblVLRecordStatus.Text.Trim, lblVLRecordStatus.Text, String.Empty)

            'New Expiry Date
            Dim lblVLNewExpiryDtm As Label = CType(e.Row.FindControl("lblVLNewExpiryDtm"), Label)
           
            'Lot Assign Status
            Dim lblVLLotAssignStatus As Label = CType(e.Row.FindControl("lblVLLotAssignStatus"), Label)
            Status.GetDescriptionFromDBCode(VaccineLotDetailLotAssignStatus.ClassCode, lblVLLotAssignStatus.Text.Trim, lblVLLotAssignStatus.Text, String.Empty)

            'Request Type
            Dim lblVLRequestType As Label = CType(e.Row.FindControl("lblVLRequestType"), Label)

            'New Lot assign status
            Dim lblVLNewLotAssignStatus As Label = CType(e.Row.FindControl("lblVLNewLotAssignStatus"), Label)
            Dim lblVLSymbol As Label = CType(e.Row.FindControl("lblVLSymbol"), Label)

            If lblVLNewLotAssignStatus.Text IsNot String.Empty Then
                Status.GetDescriptionFromDBCode(VaccineLotDetailLotAssignStatus.ClassCode, lblVLNewLotAssignStatus.Text.Trim, lblVLNewLotAssignStatus.Text, String.Empty)

                If lblVLLotAssignStatus.Text.Trim <> lblVLNewLotAssignStatus.Text.Trim Then
                    lblVLNewLotAssignStatus.Visible = True
                    lblVLSymbol.Visible = True
                End If
            End If

            'Request Type 
            If lblVLRequestType.Text = String.Empty Then
                lblVLRequestType.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                ' udtStaticDataBLL = New Common.Component.StaticData.StaticDataBLL()
                udtStaticData = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("VaccineLotCreationRequestType", lblVLRequestType.Text)
                lblVLRequestType.Text = CStr(udtStaticData.DataValue)
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
            Dim strLotBrand As String = CType(r.FindControl("hfVLBrandId"), HiddenField).Value.Trim

            BindVLSummaryView(strLotNo, strLotBrand)
        End If
    End Sub

    Private Sub gvResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvResult.Sorting
        Me.GridViewSortingHandler(sender, e, SESS_SearchResultList)
    End Sub

    Private Sub ApproveLotRecord()
        Dim blnResult As New Boolean
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim udtVaccineLotCreation As VaccineLotCreationModel = Session(SESS_VaccineLotListModal)

        udcInfoBox.Visible = False
        msgBox.Visible = False

        udtAuditLogEntry.WriteStartLog(LogID.LOG00013, AuditMsg00013)
        ViewState("action") = ActionType.Approval

        Try
            blnResult = udtVaccineLotBLL.VaccineLotCreationAction(lblDetailVaccineLotNo.Text, VACCINELOT_ACTIONTYPE.ACTION_APPROVE, udtVaccineLotCreation.TSMP, String.Empty)

            If (blnResult) Then
                udtAuditLogEntry.WriteEndLog(LogID.LOG00014, AuditMsg00014)
                udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001, "%s", lblDetailVaccineLotNo.Text.Trim)
                udcInfoBox.Type = CustomControls.InfoMessageBoxType.Complete
                udcInfoBox.BuildMessageBox()
                ChangeViewIndex(ViewIndex.MsgPage)
            Else
                Me.msgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
                WriteUpdateFailedAuditLog(lblDetailVaccineLotNo.Text.Trim)
                ChangeViewIndex(ViewIndex.ErrorPage)
            End If
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00014, AuditMsg00014)
            WriteUpdateFailedAuditLog(lblDetailTitleVaccineLotNo.Text.Trim)
            Throw
        End Try


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

#End Region

    ''' <summary>
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Return Nothing
    End Function

    ''' <summary>
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    ''' <summary>
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        Return Nothing
    End Function

    ''' <summary>
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetDocCode() As String
        Return Nothing
    End Function

End Class