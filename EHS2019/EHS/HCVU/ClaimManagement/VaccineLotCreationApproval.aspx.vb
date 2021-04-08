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
#End Region

#Region "Audit Log Message"
    Private Const AuditMsg00000 As String = "Vaccine Lot Approval loaded"
    Private Const AuditMsg00001 As String = "Generate the approval grid view"
    Private Const AuditMsg00002 As String = "Search completed. No record found"
    Private Const AuditMsg00003 As String = "Search completed"
    Private Const AuditMsg00004 As String = "Search failed"
    Private Const AuditMsg00005 As String = "Select: <vaccine lot creation records under pending>"
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
        'If MultiViewEnquiry.ActiveViewIndex = ViewIndex.SearchCriteria Then
        '    'pnlEnquiry.DefaultButton = ibtnSearch.ID
        'End If

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
        If IsNothing(strVLNo) OrElse strVLNo.Trim = String.Empty Then Return

        Dim blnNotFind As Boolean = False

        Dim udtVaccineLotList As VaccineLotCreationModel = Nothing

        udtVaccineLotList = udtVaccineLotBLL.GetVaccineLotCreationModalByLotNo(strVLNo, strVLBrand)

        If IsNothing(udtVaccineLotList) Then
            blnNotFind = True
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
        Else
            'check user 
            Dim udtHCVUUser As HCVUUserModel
            Dim udtHCVUUserBLL As New HCVUUserBLL
            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            'disable the approval and reject button 
            If (udtHCVUUser.UserID = udtVaccineLotList.RequestBy.Trim) Or (udtVaccineLotList.RequestType = String.Empty) Or udtVaccineLotList.NewRecordStatus = "A" Then

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
        End If

    End Sub

    Protected Sub ibtnBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00008, AuditMsg00008)

        msgBox.Visible = False
        udcInfoBox.Visible = False

        ChangeViewIndex(ViewIndex.SearchResult)
    End Sub

    Protected Sub ibtnErrorBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00009, AuditMsg00009)

        msgBox.Visible = False
        udcInfoBox.Visible = False

        'BackToSearchCriteriaView(True)
        Dim dt As New DataTable

        dt = Session(SESS_SearchResultList)


        gvResult.DataSource = dt
        gvResult.DataBind()
        gvResult.Visible = True



        Me.GridViewDataBind(gvResult, dt, "Vaccine_Lot_No", "ASC", False)

        ChangeViewIndex(ViewIndex.SearchResult)
    End Sub

    Protected Sub ibtnMsgBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00012, AuditMsg00012)

        BackToSearchCriteriaView(True)
    End Sub

    Protected Sub ibtnApproval_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Vaccine Lot ID", lblDetailVaccineLotNo.Text.Trim)
        udtAuditLogEntry.WriteLog(LogID.LOG00010, AuditMsg00010)

        Me.ModalPopupConfirmApproval.Show()
    End Sub


    Protected Sub ibtnReject_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Vaccine Lot NO", lblDetailVaccineLotNo.Text.Trim)
        udtAuditLogEntry.WriteLog(LogID.LOG00011, AuditMsg00011)

        Me.ModalPopupConfirmReject.Show()
    End Sub

    Private Sub WriteUpdateFailedAuditLog(ByVal strVaccineLotID As String)
        If msgBox.GetCodeTable.Rows.Count = 0 Then
            msgBox.Visible = False
        Else
            Dim strAction As String = ViewState("action")
            Dim udtExAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtExAuditLogEntry.AddDescripton("Vaccine Lot ID", strVaccineLotID)
            Select Case strAction

                Case ActionType.Approval
                    msgBox.BuildMessageBox("UpdateFail", udtExAuditLogEntry, LogID.LOG00017, AuditMsg00015)
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00015, AuditMsg00015)
                Case ActionType.Reject
                    msgBox.BuildMessageBox("UpdateFail", udtExAuditLogEntry, LogID.LOG00018, AuditMsg00019)
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00019, AuditMsg00019)
            End Select

        End If
    End Sub

    Private Sub BackToSearchCriteriaView(ByVal blnIsReset As Boolean)
        udcInfoBox.Visible = False

        ChangeViewIndex(ViewIndex.SearchResult)

        SearchCreationList()
    End Sub

    Private Sub BindVaccineLotDetail(ByVal udtVaccineLotList As VaccineLotCreationModel)
        Session(SESS_VaccineLotListModal) = udtVaccineLotList


        lblDetailBrandName.Text = udtVaccineLotList.BrandName
        lblDetailVaccineLotNo.Text = udtVaccineLotList.VaccineLotNo
        lblDetailExpiryD.Text = udtFormatter.formatDisplayDate(CDate(udtVaccineLotList.VaccineExpiryDate))


        If (udtVaccineLotList.NewExpiryDate IsNot String.Empty) Then
            lblDetailNewExpiryD.Text = udtFormatter.formatDisplayDate(CDate(udtVaccineLotList.NewExpiryDate))
        End If



        lblDetailRecordStatus.Text = udtVaccineLotList.RecordStatus
        Status.GetDescriptionFromDBCode(VaccineLotDetailRecordStatus.ClassCode, lblDetailRecordStatus.Text, lblDetailRecordStatus.Text, String.Empty)

        lblDetailRequestType.Text = udtVaccineLotList.RequestType
        Status.GetDescriptionFromDBCode(VaccineLotRequestStatus.RequstTypeClassCode, lblDetailRequestType.Text, lblDetailRequestType.Text, String.Empty)

        'lblDetailRequestedBy.Text = udtVaccineLotList.UpdateBy + " (" + Convert.ToDateTime(udtVaccineLotList.UpdateDtm) + ")"

        lblDetailRequestedBy.Text = udtVaccineLotList.UpdateBy + " (" + Format(CDate(udtVaccineLotList.UpdateDtm), "dd MMM yyyy HH:mm") + ")"
        lblDetailCreatedBy.Text = udtVaccineLotList.CreateBy + " (" + Format(CDate(udtVaccineLotList.CreateDtm), "dd MMM yyyy HH:mm") + ")"

    End Sub



#Region "Popup box events"

    Private Sub ucNoticePopUpConfirmApproval_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUpConfirmApproval.ButtonClick

        'msgBox.Visible = True
        udcInfoBox.Visible = False
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim blnResult As New Boolean
        Dim strAction As String = String.Empty
        Dim blnValidDate As Boolean = True
        Dim udtSystemMessage As SystemMessage
        Dim strToday As String = DateTime.Now.ToString("MM/dd/yyyy")


        Select Case e
            Case ucNoticePopUp.enumButtonClick.OK
                ' 1: Check date : Expiry date > Today (new)
                'udtFormatter.formatInputDate(lblDetailExpiryD.Text.Trim)
                If blnValidDate Then
                    udtSystemMessage = udtValidator.chkInputValidFromDateCutoffDate(FunctCode.FUNT990000, MsgCode.MSG00381, _
                        udtFormatter.formatInputDate(strToday), udtFormatter.formatInputDate(lblDetailExpiryD.Text.Trim))

                    ' The Expiry Date should not be later than Today.
                    If Not IsNothing(udtSystemMessage) Then
                        blnValidDate = False
                        'msgBox.AddMessage(udtSystemMessage, {"%t", "%s"}, {GetGlobalResourceObject("Text", "VaccineLotExpiryDate")})
                        udcInfoBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002))
                        udcInfoBox.BuildMessageBox()

                        udtAuditLogEntry.WriteLog(LogID.LOG00022, AuditMsg00022)
                        ' msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00041, AuditMsg00021)
                        'msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00025), {"%t", "%s"}, {GetGlobalResourceObject("Text", "VaccineLotEffectiveDateFrom"), GetGlobalResourceObject("Text", "VaccineLotEffectiveDateTo")})
                    End If
                End If

                If blnValidDate Then
                    'check the transaction existed
                    If udtVaccineLotBLL.CheckTransactionAdditionalFieldByVaccineLotNo(lblDetailVaccineLotNo.Text) Then
                        'show message for invalid
                        udcInfoBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001))
                        udcInfoBox.BuildMessageBox()
                        udtAuditLogEntry.WriteLog(LogID.LOG00021, AuditMsg00021)
                        'msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00041, AuditMsg00021)
                    End If
                Else
                    udcInfoBox.Visible = False
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00016, AuditMsg00016)
                End If

                If blnValidDate Then
                    ApproveLotRecord()
                End If
            Case Else 'cancel click
                udcInfoBox.Visible = False
                udtAuditLogEntry.WriteEndLog(LogID.LOG00016, AuditMsg00016)
        End Select


    End Sub

    Private Sub ucNoticePopUpConfirmReject_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUpConfirmReject.ButtonClick
        Dim udtVaccineLotCreation As VaccineLotCreationModel = Session(SESS_VaccineLotListModal)

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim blnResult As New Boolean
        udcInfoBox.Visible = False

        Select Case e
            Case ucNoticePopUp.enumButtonClick.OK
                udtAuditLogEntry.WriteStartLog(LogID.LOG00017, AuditMsg00017)
                blnResult = udtVaccineLotBLL.VaccineLotCreationAction(lblDetailVaccineLotNo.Text, VACCINELOT_ACTIONTYPE.ACTION_REJECT, udtVaccineLotCreation.TSMP)

                If (blnResult) Then
                    udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00002)
                    udcInfoBox.Type = CustomControls.InfoMessageBoxType.Complete
                    udcInfoBox.BuildMessageBox()
                    ChangeViewIndex(ViewIndex.MsgPage)
                End If
                udtAuditLogEntry.WriteEndLog(LogID.LOG00018, AuditMsg00018)
            Case Else
                udcInfoBox.Visible = False
                ViewState("action") = ActionType.Reject
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
        If e.Row.RowType = DataControlRowType.DataRow Then

            'Record Status 
            Dim lblVLRecordStatus As Label = CType(e.Row.FindControl("lblVLRecordStatus"), Label)
            Status.GetDescriptionFromDBCode(VaccineLotDetailRecordStatus.ClassCode, lblVLRecordStatus.Text.Trim, lblVLRecordStatus.Text, String.Empty)

            'Request Type
            Dim lblVLRequestType As Label = CType(e.Row.FindControl("lblVLRequestType"), Label)
            If lblVLRequestType.Text = String.Empty Then
                lblVLRequestType.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                Status.GetDescriptionFromDBCode(VaccineLotRequestStatus.RequstTypeClassCode, lblVLRequestType.Text.Trim, lblVLRequestType.Text, String.Empty)
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
            Dim strVLNo As String = CType(r.FindControl("lnkbtnVLNo"), LinkButton).CommandArgument.Trim
            Dim strVLBrand As String = CType(r.FindControl("hfVLBrandId"), HiddenField).Value.Trim
            BindVLSummaryView(strVLNo, strVLBrand)
        End If
    End Sub

    Private Sub gvResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvResult.Sorting
        Me.GridViewSortingHandler(sender, e, SESS_SearchResultList)
    End Sub

    Private Sub ApproveLotRecord()
        Dim blnResult As New Boolean
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim udtVaccineLotCreation As VaccineLotCreationModel = Session(SESS_VaccineLotListModal)

        udtAuditLogEntry.WriteStartLog(LogID.LOG00013, AuditMsg00013)
        blnResult = udtVaccineLotBLL.VaccineLotCreationAction(lblDetailVaccineLotNo.Text, VACCINELOT_ACTIONTYPE.ACTION_APPROVE, udtVaccineLotCreation.TSMP)

        If (blnResult) Then
            'udcInfoBox.BuildMessageBox()
            'udcInfoBox.Type = CustomControls.InfoMessageBoxType.Complete

            udtAuditLogEntry.WriteEndLog(LogID.LOG00014, AuditMsg00014)
            udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001)
            udcInfoBox.Type = CustomControls.InfoMessageBoxType.Complete
            udcInfoBox.BuildMessageBox()
            ChangeViewIndex(ViewIndex.MsgPage)

        Else
            ViewState("action") = ActionType.Approval
            udtAuditLogEntry.WriteEndLog(LogID.LOG00014, AuditMsg00014)
            WriteUpdateFailedAuditLog(lblDetailTitleVaccineLotNo.Text.Trim)
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