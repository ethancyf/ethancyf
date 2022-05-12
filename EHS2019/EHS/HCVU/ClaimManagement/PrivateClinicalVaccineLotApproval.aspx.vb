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
Imports Common.Component.Scheme
Imports Common.ComFunction
Imports Common.Component.UserRole

Partial Public Class PrivateClinicalVaccineLotApproval
    Inherits BasePageWithControl

#Region "Constants"
    Public Enum ActionType
        Approval = 0
        Reject = 1

    End Enum

    Public Enum ViewIndex
        SearchCriteria = 0
        SearchResult = 1
        Detail = 2
        MsgPage = 3
        ErrorPage = 4
    End Enum

#End Region

#Region "Fields"
    Private udtVaccineLotBLL As New VaccineLotBLL
    Private udtHCVUUserBLL As New HCVUUserBLL

    Private udtFormatter As New Formatter
    Private udtValidator As New Validator

    Private udtUserRoleBLL As New UserRoleBLL
    Private udtSchemeClaimBLL As New SchemeClaimBLL

    'Private SM As SystemMessage
    Private udtAuditLogEntry As AuditLogEntry
    Private Const SESS_SearchResultList As String = FunctCode.FUNT010428 & "VaccineLotList_SearchResultList"
    Private Const SESS_VaccineLotListModal As String = FunctCode.FUNT010428 & "VaccineLotList_Modal"
#End Region

#Region "Audit Log Message"
    Private Const AuditMsg00000 As String = "Vaccine Lot Approval loaded"
    Private Const AuditMsg00001 As String = "Search Start"
    Private Const AuditMsg00002 As String = "Search completed"
    Private Const AuditMsg00003 As String = "Search completed with no record"
    Private Const AuditMsg00004 As String = "Search failed"
    Private Const AuditMsg00005 As String = "Back Click"
    Private Const AuditMsg00006 As String = "Select request Id in grid"
    Private Const AuditMsg00007 As String = "Vaccine Lot Record - back Click"
    Private Const AuditMsg00008 As String = "Vaccine Lot Record - Approve Click"
    Private Const AuditMsg00009 As String = "Vaccine Lot Record - Approve Click - Yes"
    Private Const AuditMsg00010 As String = "Vaccine Lot Record - Approve Click - No"
    Private Const AuditMsg00011 As String = "Vaccine Lot Record - Approve Click - Completed"
    Private Const AuditMsg00012 As String = "Vaccine Lot Record - Approve Click - Failed"
    Private Const AuditMsg00013 As String = "Vaccine Lot Record - Reject Click"
    Private Const AuditMsg00014 As String = "Vaccine Lot Record - Reject Click - Yes"
    Private Const AuditMsg00015 As String = "Vaccine Lot Record - Reject Click - No"
    Private Const AuditMsg00016 As String = "Vaccine Lot Record - Reject Click - Completed"
    Private Const AuditMsg00017 As String = "Vaccine Lot Record - Reject Click - Failed"
    Private Const AuditMsg00018 As String = "Msg Page - Return"
    Private Const AuditMsg00019 As String = "Error Msg Page - Back"
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

        Dim udtHCVUUser As HCVUUserModel = udtHCVUUserBLL.GetHCVUUser

        'search from store port
        ' bllSearchResult = udtVaccineLotBLL.GetVaccineLotSearch(ddlVaccCentre.SelectedValue.ToString())
        bllSearchResult = udtVaccineLotBLL.GetVaccineLotRequestInScheme(VaccineLotRecordServiceType.Scheme,
                                                                        ddlVaccScheme.SelectedValue.Trim, _
                                                                        VaccineLotRecordConstantType.VaccineLotRecordStatus_PendingApproval,
                                                                        udtHCVUUser.UserID)
        'VaccineLotRecordServiceType.Scheme, ddlVaccScheme.SelectedValue.Trim, _ VaccineLotRecordConstantType.VaccineLotRecordStatus_PendingApproval

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

            Me.GridViewDataBind(gvResult, dt, "Request_ID", "ASC", False)

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
            FunctionCode = FunctCode.FUNT010428

            ' Write Audit Log
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, AuditMsg00000)

            'new
            Dim udtSchemeClaimModelListFilter As New SchemeClaimModelCollection
            Dim udtUserRoleCollection As UserRoleModelCollection = udtUserRoleBLL.GetUserRoleCollection(udtHCVUUserBLL.GetHCVUUser.UserID)
            Dim udtSchemeCList As SchemeClaimModelCollection = udtSchemeClaimBLL.getAllDistinctSchemeClaim()
            'Dim udtGen As New Common.ComFunction.GeneralFunction

            Dim strSchemeCode() As String = Split((New GeneralFunction).getSystemParameter("VaccineLotManagementSchemeSetting_PrivateClinic"), ";")
            If strSchemeCode.Length > 0 Then
                For Each udtschemeclaim As SchemeClaimModel In udtSchemeCList
                    For intCt As Integer = 0 To strSchemeCode.Length - 1
                        If udtschemeclaim.SchemeCode.Trim() <> strSchemeCode(intCt) Then
                            Continue For   'if not equal, wont do anything until it finds the equal one
                        End If

                        For Each udtUserRoleModel As UserRoleModel In udtUserRoleCollection.Values
                            If udtUserRoleModel.SchemeCode.Trim = udtschemeclaim.SchemeCode.Trim Then
                                If Not udtSchemeClaimModelListFilter.Contains(udtschemeclaim) Then udtSchemeClaimModelListFilter.Add(udtschemeclaim)
                            End If

                        Next
                    Next
                Next
            End If

            'Dim centreDataTable As DataTable = udtVaccineLotBLL.GetCOVID19VaccineCentreHCVUMapping(udtHCVUUserBLL.GetHCVUUser.UserID)

            If udtSchemeClaimModelListFilter IsNot Nothing AndAlso udtSchemeClaimModelListFilter.Count > 0 Then
                'ddlVaccScheme.Items.Clear()
                ddlVaccScheme.DataSource = udtSchemeClaimModelListFilter
                ddlVaccScheme.DataTextField = "SchemeDesc"
                ddlVaccScheme.DataValueField = "SchemeCode"
                ddlVaccScheme.DataBind()

                If udtSchemeClaimModelListFilter.Count > 1 Then
                    ddlVaccScheme.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))
                    ddlVaccScheme.SelectedIndex = 0
                Else
                    ddlVaccScheme.SelectedIndex = 0
                    ibtnSearch_Click(Nothing, Nothing)
                End If
                pnlEnquiry.Visible = True
            Else
                udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00004)
                udcInfoBox.Type = CustomControls.InfoMessageBoxType.Information
                udcInfoBox.BuildMessageBox()
                ddlVaccScheme.Enabled = False
                ibtnSearch.Enabled = False
                ibtnSearch.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "SearchBtnDisabled")
                pnlEnquiry.Visible = False
            End If
        End If

        Me.ModalPopupConfirmApproval.PopupDragHandleControlID = Me.ucNoticePopUpConfirmApproval.Header.ClientID
        Me.ModalPopupConfirmReject.PopupDragHandleControlID = Me.ucNoticePopUpConfirmReject.Header.ClientID


    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If MultiViewEnquiry.ActiveViewIndex = ViewIndex.SearchCriteria Then
            pnlEnquiry.DefaultButton = ibtnSearch.ID
        End If

    End Sub

    Private Sub ChangeViewIndex(ByVal udtViewIndex As ViewIndex)
        MultiViewEnquiry.ActiveViewIndex = udtViewIndex

        Select Case udtViewIndex
            Case ViewIndex.SearchCriteria
                Session.Remove(SESS_VaccineLotListModal)
            Case ViewIndex.SearchResult
                Session.Remove(SESS_VaccineLotListModal)
            Case ViewIndex.Detail
            Case ViewIndex.MsgPage
                ibtnConfirmBack.Visible = True
                ibtnNoRecordBack.Visible = False
            Case ViewIndex.ErrorPage
        End Select
    End Sub

    Private Sub BindVLSummaryView(ByVal strRequestID As String)
        udcInfoBox.Visible = False
        msgBox.Visible = False
        If IsNothing(strRequestID) OrElse strRequestID.Trim = String.Empty Then Return

        Dim blnNotFind As Boolean = False

        Dim udtHCVUUser As HCVUUserModel = udtHCVUUserBLL.GetHCVUUser

        Dim udtVaccineLotList As VaccineLotRequestModel = Nothing 'VaccineLotRequestModelInClinics

        udtVaccineLotList = udtVaccineLotBLL.GetVaccineLotModalByRequestID(strRequestID,
                                                                           VaccineLotRecordServiceType.Scheme,
                                                                           VaccineLotRecordConstantType.VaccineLotRecordStatus_PendingApproval,
                                                                           udtHCVUUser.UserID)

        If IsNothing(udtVaccineLotList) Then
            blnNotFind = True
        End If

        ' Write Audit Log
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Request Id", strRequestID)
        udtAuditLogEntry.WriteLog(LogID.LOG00006, AuditMsg00006)

        If blnNotFind Then
            msgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
            msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00019, AuditMsg00019)

            ChangeViewIndex(ViewIndex.ErrorPage)
        Else
            'check user 

            'disable the approval and reject button 
            If (udtHCVUUser.UserID = udtVaccineLotList.RequestBy.Trim) Or (udtVaccineLotList.RequestType = String.Empty) Then

                ibtnApproval.ImageUrl = GetGlobalResourceObject("ImageUrl", "ApproveDisableBtn")
                ibtnReject.ImageUrl = GetGlobalResourceObject("ImageUrl", "RejectDisabledBtn")
                ibtnApproval.Enabled = False
                ibtnReject.Enabled = False

                udcInfoBox.AddMessage(FunctCode.FUNT010423, SeverityCode.SEVI, MsgCode.MSG00003)
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

        End If

    End Sub
#End Region

#Region "Click Events"
    Protected Sub ibtnSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcInfoBox.Visible = False
        msgBox.Visible = False
        imgVaccCentreAlert.Visible = False

        udcCollapsibleSearchCriteriaReview.Collapsed = True
        udcCollapsibleSearchCriteriaReview.ClientState = "True"
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        Dim enumSearchResult As SearchResultEnum


        ' Try-catch to avoid search over row limit (500)
        Try
            ' Write Audit Log
            udtAuditLogEntry.AddDescripton("Vaccine Scheme", ddlVaccScheme.SelectedValue)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00001, AuditMsg00001)

            If sender Is Nothing Then
                enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, msgBox, udcInfoBox, False, True)
            Else
                enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, msgBox, udcInfoBox)
            End If

            Select Case enumSearchResult
                Case SearchResultEnum.Success
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00002, AuditMsg00002)

                Case SearchResultEnum.ValidationFail
                    ' Audit Log has been handled in [SF_ValidateSearch] method

                Case SearchResultEnum.NoRecordFound
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditMsg00003)
                    ibtnNoRecordBack.Visible = True
                    ibtnConfirmBack.Visible = False
                Case SearchResultEnum.OverResultList1stLimit_PopUp
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00004, AuditMsg00004)

                Case SearchResultEnum.OverResultList1stLimit_Alert
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00004, AuditMsg00004)

                Case SearchResultEnum.OverResultListOverrideLimit
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00004, AuditMsg00004)

                Case Else
                    Throw New Exception("Error: Class = [HCVU.Vaccine Lot Approval], Method = [ibtnSearch_Click], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")

            End Select

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
            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception")
            udtAuditLogEntry.AddDescripton("Message", ex.Message)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00004, AuditMsg00004)
            Throw
        End Try
    End Sub

    Protected Sub ibtnSearchResultBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00005, AuditMsg00005)

        'BackToSearchCriteriaView(True)
        udcInfoBox.Visible = False
        msgBox.Visible = False
        ChangeViewIndex(ViewIndex.SearchCriteria)
    End Sub

    Protected Sub ibtnBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00007, AuditMsg00007)

        msgBox.Visible = False
        udcInfoBox.Visible = False

        ChangeViewIndex(ViewIndex.SearchResult)
    End Sub

    Protected Sub ibtnConfirmBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00019, AuditMsg00019)

        msgBox.Visible = False
        udcInfoBox.Visible = False

        ibtnSearch_Click(Nothing, Nothing)
        'ChangeViewIndex(ViewIndex.SearchCriteria)
    End Sub

    Protected Sub ibtnNoRecordBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00019, AuditMsg00019)

        msgBox.Visible = False
        udcInfoBox.Visible = False

        'ibtnSearch_Click(Nothing, Nothing)
        ChangeViewIndex(ViewIndex.SearchCriteria)
    End Sub

    Protected Sub ibtnReturn_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00018, AuditMsg00018)

        'BackToSearchCriteriaView(True)
        msgBox.Visible = False
        udcInfoBox.Visible = False

        ibtnSearch_Click(Nothing, Nothing)
        'ChangeViewIndex(ViewIndex.SearchCriteria)
    End Sub

    Protected Sub ibtnApproval_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim blnValid As Boolean = True
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00008, AuditMsg00008)


        Me.ModalPopupConfirmApproval.Show()

    End Sub

    Protected Sub ibtnReject_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00013, AuditMsg00013)

        Me.ModalPopupConfirmReject.Show()
    End Sub

    Private Sub WriteUpdateFailedAuditLog(ByVal strRequestId As String)
        If msgBox.GetCodeTable.Rows.Count = 0 Then
            msgBox.Visible = False
        Else
            Dim strAction As String = ViewState("action")
            Dim udtExAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtExAuditLogEntry.AddDescripton("Request ID", strRequestId)
            Select Case strAction
                Case ActionType.Approval
                    msgBox.BuildMessageBox("UpdateFail", udtExAuditLogEntry, LogID.LOG00012, AuditMsg00012)
                Case ActionType.Reject
                    msgBox.BuildMessageBox("UpdateFail", udtExAuditLogEntry, LogID.LOG00017, AuditMsg00017)

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

    Private Sub BindVaccineLotDetail(ByVal udtVaccineLotList As VaccineLotRequestModel)
        'Dim udtStaticDataBLL As StaticData.StaticDataBLL = New StaticData.StaticDataBLL
        'Dim udtStaticDataModel As StaticData.StaticDataModel = Nothing

        Session(SESS_VaccineLotListModal) = udtVaccineLotList
        Dim udtSchemeBackOfficeBLL As New Scheme.SchemeBackOfficeBLL
        Dim udtSchemeBackOfficeModel As Scheme.SchemeBackOfficeModel
        Dim SchemeName As String = String.Empty
        udtSchemeBackOfficeModel = udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup().Filter(udtVaccineLotList.SchemeCode) '(ddlSearchVaccScheme.SelectedItem.Value.ToString)
        SchemeName = udtSchemeBackOfficeModel.SchemeDesc.Trim

        lblDetailScheme.Text = SchemeName
        lblDetailRequestID.Text = udtVaccineLotList.RequestID
        'lblDetailBooth.Text = udtVaccineLotList.Booth

        'udtStaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("VaccineCentreBooth", "OR")
        'lblDetailBooth.Text = lblDetailBooth.Text.Replace("OR", udtStaticDataModel.DataValue.ToString.Trim())

        'Dim strBoothID As String() = lblDetailBooth.Text.Split(",")
        'Dim strBoothName As String = String.Empty

        'For i As Integer = 0 To strBoothID.Length - 1
        '    udtStaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("VaccineCentreBooth", strBoothID(i).Trim)
        '    strBoothName += strBoothID(i).Trim.Replace(strBoothID(i).Trim, udtStaticDataModel.DataValue.ToString.Trim()) + ","
        'Next
        'strBoothName = strBoothName.Remove(strBoothName.Length - 1)
        'lblDetailBooth.Text = strBoothName


        lblDetailBrandName.Text = udtVaccineLotList.BrandName
        lblDetailBrandTradeName.Text = udtVaccineLotList.BrandTradeName
        lblDetailVaccineLotNo.Text = udtVaccineLotList.VaccineLotNo
        lblDetailExpiryD.Text = udtFormatter.formatDisplayDate(CDate(udtVaccineLotList.ExpiryDate))

        If udtVaccineLotList.EffectiveFrom IsNot String.Empty Then
            lblDetailEffectiveFrom.Text = udtFormatter.formatDisplayDate(CDate(udtVaccineLotList.EffectiveFrom))

            If udtVaccineLotList.UpToExpiryDtm = YesNo.Yes Then
                lblDetailEffectiveTo.Text = Me.GetGlobalResourceObject("Text", "UpToVacExpDate") + " " + udtFormatter.formatDisplayDate(CDate(udtVaccineLotList.ExpiryDate))
            Else
                lblDetailEffectiveTo.Text = udtFormatter.formatDisplayDate(CDate(udtVaccineLotList.EffectiveTo))
            End If

            trEffectiveFrom.Visible = True
            trEffectiveTo.Visible = True

        Else
            lblDetailEffectiveFrom.Text = String.Empty
            lblDetailEffectiveTo.Text = String.Empty
            trEffectiveFrom.Visible = False
            trEffectiveTo.Visible = False
        End If

        lblDetailNewRecordStatus.Text = udtVaccineLotList.RecordStatus
        Status.GetDescriptionFromDBCode(VaccineLotMappingRecordStatus.ClassCode, lblDetailNewRecordStatus.Text, lblDetailNewRecordStatus.Text, String.Empty)

        lblDetailRequestType.Text = udtVaccineLotList.RequestType
        Status.GetDescriptionFromDBCode(VaccineLotRequestStatus.RequstTypeClassCode, lblDetailRequestType.Text, lblDetailRequestType.Text, String.Empty)


        lblDetailRequestedBy.Text = udtVaccineLotList.RequestBy + " (" + Format(CDate(udtVaccineLotList.RequestDtm), "dd MMM yyyy HH:mm") + ")"


        lblDetailUpToExpiryDtm.Text = udtVaccineLotList.UpToExpiryDtm

        Dim dt = New DataTable

        dt = udtVaccineLotBLL.GetVaccineLotSummaryByRequestID(lblDetailRequestID.Text)
        gvSummary.DataSource = dt
        gvSummary.DataBind()

    End Sub

#End Region

#Region "Search Criteria"
    Private Sub FillSearchCriteria()
        If ddlVaccScheme.SelectedValue.Trim.Equals(String.Empty) Then
            lblResultSchemeVaccNo.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else
            lblResultSchemeVaccNo.Text = ddlVaccScheme.SelectedItem.Text.Trim
        End If

    End Sub

    Private Sub ResetSearchCriteria()
        ddlVaccScheme.SelectedValue = String.Empty
    End Sub

#End Region

#Region "Popup box events"
    Private Sub ucNoticePopUpConfirmApproval_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUpConfirmApproval.ButtonClick
        Dim blnValid As Boolean = True

        Dim dvLotDetailFilterWithLotAssign As DataView = Nothing

        udcInfoBox.Visible = False
        msgBox.Visible = False
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        ViewState("action") = ActionType.Approval

        Select Case e
            Case ucNoticePopUp.enumButtonClick.OK
                Try
                    udtAuditLogEntry.AddDescripton("Request Id", lblDetailRequestID.Text)
                    udtAuditLogEntry.WriteLog(LogID.LOG00009, AuditMsg00009)

                    'check the lot detail is marked at unavailable
                    If (blnValid) Then
                        dvLotDetailFilterWithLotAssign = New DataView(udtVaccineLotBLL.CheckVaccineLotDetailExist(lblDetailVaccineLotNo.Text.Trim))
                        dvLotDetailFilterWithLotAssign.RowFilter = udtVaccineLotBLL.FilterLotDetailByLotAssignStatus(VaccineLotDetailLotAssignStatus.Unavailable)
                        If dvLotDetailFilterWithLotAssign.Count > 0 Then
                            'If udtVaccineLotBLL.CheckVaccineLotDetailExist(lblDetailVaccineLotNo.Text.Trim, String.Empty, String.Empty, VaccineLotDetailLotAssignStatus.Unavailable) Then
                            msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00003), "%s", lblDetailVaccineLotNo.Text.Trim)
                            blnValid = False
                        End If
                    End If

                    If (blnValid) Then
                        ApproveLotRecord()
                    Else
                        WriteUpdateFailedAuditLog(lblDetailRequestID.Text.Trim)
                    End If

                Catch ex As Exception
                    udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception")
                    udtAuditLogEntry.AddDescripton("Message", ex.Message)
                    WriteUpdateFailedAuditLog(lblDetailRequestID.Text.Trim)
                End Try

            Case Else
                udtAuditLogEntry.WriteLog(LogID.LOG00010, AuditMsg00010)
        End Select
    End Sub

    Private Sub ApproveLotRecord()
        Dim blnResult As New Boolean
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        Dim udtVaccineLotList As VaccineLotRequestModel = Session(SESS_VaccineLotListModal)
        ViewState("action") = ActionType.Approval
        Try
            blnResult = udtVaccineLotBLL.VaccineLotEditApproveAction(lblDetailRequestID.Text, VACCINELOT_ACTIONTYPE.ACTION_APPROVE, VaccineLotRecordServiceType.Scheme, udtVaccineLotList.TSMP)

            If (blnResult) Then
                udcInfoBox.Type = CustomControls.InfoMessageBoxType.Complete
                udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001)
                udcInfoBox.BuildMessageBox()
                udcInfoBox.Type = CustomControls.InfoMessageBoxType.Complete
                ChangeViewIndex(ViewIndex.MsgPage)
                udtAuditLogEntry.WriteLog(LogID.LOG00011, AuditMsg00011)
            Else
                Me.msgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
                WriteUpdateFailedAuditLog(lblDetailRequestID.Text.Trim)
                ChangeViewIndex(ViewIndex.ErrorPage)
            End If
        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception")
            udtAuditLogEntry.AddDescripton("Message", ex.Message)
            WriteUpdateFailedAuditLog(lblDetailRequestID.Text.Trim)
            Throw
        End Try
    End Sub

    Private Sub ucNoticePopUpConfirmReject_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUpConfirmReject.ButtonClick

        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udcInfoBox.Visible = False
        msgBox.Visible = False
        Dim blnResult As New Boolean
        Dim udtVaccineLotList As VaccineLotRequestModel = Session(SESS_VaccineLotListModal)
        ViewState("action") = ActionType.Reject

        Select Case e
            Case ucNoticePopUp.enumButtonClick.OK
                udtAuditLogEntry.AddDescripton("Request Id", lblDetailRequestID.Text)
                udtAuditLogEntry.WriteLog(LogID.LOG00014, AuditMsg00014)
                Try
                    blnResult = udtVaccineLotBLL.VaccineLotEditApproveAction(lblDetailRequestID.Text, VACCINELOT_ACTIONTYPE.ACTION_REJECT, VaccineLotRecordServiceType.Scheme, udtVaccineLotList.TSMP)

                    If (blnResult) Then
                        udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00002)
                        udcInfoBox.Type = CustomControls.InfoMessageBoxType.Complete
                        udcInfoBox.BuildMessageBox()
                        udtAuditLogEntry.WriteLog(LogID.LOG00016, AuditMsg00016)
                        ChangeViewIndex(ViewIndex.MsgPage)
                    Else
                        Me.msgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
                        WriteUpdateFailedAuditLog(lblDetailRequestID.Text.Trim)
                        ChangeViewIndex(ViewIndex.ErrorPage)
                    End If
                Catch ex As Exception
                    udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception")
                    udtAuditLogEntry.AddDescripton("Message", ex.Message)
                    WriteUpdateFailedAuditLog(lblDetailRequestID.Text.Trim)

                End Try
            Case Else
                udtAuditLogEntry.WriteLog(LogID.LOG00015, AuditMsg00015)
        End Select

    End Sub

#End Region

#Region "gvResult events"
    Private Sub gvResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            'Record Status lblVLStatus
            Dim lblVLStatus As Label = CType(e.Row.FindControl("lblVLStatus"), Label)
            Status.GetDescriptionFromDBCode(VaccineLotMappingRecordStatus.ClassCode, lblVLStatus.Text.Trim, lblVLStatus.Text, String.Empty)

            'Request Type
            Dim lblRequestType As Label = CType(e.Row.FindControl("lblRequestType"), Label)
            Dim strRequestType As String = lblRequestType.Text.Trim

            If lblRequestType.Text = String.Empty Then
                lblRequestType.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                Status.GetDescriptionFromDBCode(VaccineLotRequestStatus.RequstTypeClassCode, lblRequestType.Text.Trim, lblRequestType.Text, String.Empty)
            End If

            'Effective From 
            Dim lblVLEffFrom As Label = CType(e.Row.FindControl("lblVLEffFrom"), Label)
            If lblVLEffFrom.Text.Trim = String.Empty Or strRequestType.Equals(VaccineLotRequestStatus.RequestRemove) Then
                lblVLEffFrom.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            'Effective To 
            Dim lblVLEffTo As Label = CType(e.Row.FindControl("lblVLEffTo"), Label)
            If lblVLEffTo.Text.Trim = String.Empty Or strRequestType.Equals(VaccineLotRequestStatus.RequestRemove) Then
                lblVLEffTo.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            'Scheme
            Dim udtSchemeBackOfficeBLL As New Scheme.SchemeBackOfficeBLL
            Dim udtSchemeBackOfficeModel As Scheme.SchemeBackOfficeModel
            Dim lblVLScheme As Label = CType(e.Row.FindControl("lblVLScheme"), Label)
            udtSchemeBackOfficeModel = udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup().Filter(lblVLScheme.Text.Trim)
            lblVLScheme.Text = udtSchemeBackOfficeModel.SchemeDesc.Trim

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
            Dim strRequestID As String = CType(r.FindControl("lnkbtnRequestID"), LinkButton).CommandArgument.Trim

            BindVLSummaryView(strRequestID)
        End If
    End Sub

    Private Sub gvResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvResult.Sorting
        Me.GridViewSortingHandler(sender, e, SESS_SearchResultList)
    End Sub

    Private Sub gvSummary_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSummary.RowDataBound
        ' Dim udtSystemMessage As SystemMessage

        If e.Row.RowType = DataControlRowType.DataRow Then

            'Dim lblSummaryBoothNo As Label = CType(e.Row.FindControl("lblSummaryBoothNo"), Label)
            Dim lblSummaryEffFrom As Label = CType(e.Row.FindControl("lblSummaryEffFrom"), Label)
            Dim lblSummaryNewEffFrom As Label = CType(e.Row.FindControl("lblSummaryNewEffFrom"), Label)
            Dim lblSymbolFrom As Label = CType(e.Row.FindControl("lblSymbolFrom"), Label)
            Dim lblSummaryEffTo As Label = CType(e.Row.FindControl("lblSummaryEffTo"), Label)
            Dim lblSummaryNewEffTo As Label = CType(e.Row.FindControl("lblSummaryNewEffTo"), Label)
            Dim lblSymbolTo As Label = CType(e.Row.FindControl("lblSymbolTo"), Label)
            Dim lblRequestType As Label = CType(e.Row.FindControl("lblRequestType"), Label)
            Dim lblSummaryUseExpiryDtm As Label = CType(e.Row.FindControl("lblSummaryUseExpiryDtm"), Label)
            Dim strUpToVacExpDate As String = Me.GetGlobalResourceObject("Text", "UpToVacExpDate") + " " + lblDetailExpiryD.Text

            If lblRequestType.Text.Trim = VaccineLotRequestType.REQUESTTYPE_REMOVE Then
                If lblSummaryEffFrom.Text.Trim = String.Empty Then
                    lblSummaryEffFrom.Text = "N/A"
                End If
                lblSummaryEffFrom.Visible = True
                lblSummaryNewEffFrom.Visible = False
                lblSymbolFrom.Visible = False

                If lblSummaryEffTo.Text.Trim = String.Empty Then
                    lblSummaryEffTo.Text = "N/A"
                End If
                lblSummaryEffTo.Visible = True
                lblSummaryNewEffTo.Visible = False
                lblSymbolTo.Visible = False
                If lblSummaryUseExpiryDtm.Text = YesNo.Yes Then
                    lblSummaryEffTo.Text = Me.GetGlobalResourceObject("Text", "UpToVacExpDate") + " " + lblDetailExpiryD.Text
                End If
            Else
                'batch assign /edit
                'new assign

                'lblDetailServicePeriodFrom
                If lblSummaryEffFrom.Text.Trim.Equals(String.Empty) Then
                    lblSummaryEffFrom.Text = Me.GetGlobalResourceObject("Text", "N/A")
                End If

                'lblSummaryEffTo
                If lblSummaryEffTo.Text.Trim.Equals(String.Empty) Then
                    lblSummaryEffTo.Text = Me.GetGlobalResourceObject("Text", "N/A")
                Else

                    Dim udtSystemMessage As SystemMessage = udtValidator.chkInputValidFromDateCutoffDate(FunctCode.FUNT990000, MsgCode.MSG00381, _
                     Date.Today().ToString("dd MMM yyyy"), lblSummaryEffTo.Text)

                    'servicedayTo < today =  cutoff day
                    If (Not IsNothing(udtSystemMessage)) Then
                        lblSummaryEffFrom.Text = Me.GetGlobalResourceObject("Text", "N/A")
                        lblSummaryEffTo.Text = Me.GetGlobalResourceObject("Text", "N/A")
                    End If
                End If



                'set >> new record
                If Not lblSummaryEffFrom.Text.Trim.Equals(lblSummaryNewEffFrom.Text.Trim) Then
                    lblSummaryNewEffFrom.Text = " >> " + lblSummaryNewEffFrom.Text.Trim
                Else
                    lblSummaryNewEffFrom.Text = String.Empty
                End If


                If lblSummaryUseExpiryDtm.Text = YesNo.Yes Then
                    lblSummaryEffTo.Text = strUpToVacExpDate
                End If

                If Not lblSummaryEffTo.Text.Trim.Equals(lblSummaryNewEffTo.Text.Trim) Then
                    If lblSummaryNewEffTo.Text.Trim.Equals(String.Empty) Then
                        If Not lblSummaryEffTo.Text.Trim.Equals(strUpToVacExpDate) Then
                            lblSummaryNewEffTo.Text = " >> " + Me.GetGlobalResourceObject("Text", "UpToVacExpDate") + " " + lblDetailExpiryD.Text
                        End If
                    Else
                        lblSummaryNewEffTo.Text = " >> " + lblSummaryNewEffTo.Text.Trim
                    End If

                Else
                    lblSummaryNewEffTo.Text = String.Empty
                End If

            End If



        End If

    End Sub

#End Region

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

#Region "Overrides Function"
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

#End Region

End Class