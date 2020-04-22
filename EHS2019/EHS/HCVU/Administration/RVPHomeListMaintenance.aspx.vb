Imports System.Web.Security.AntiXss
Imports Common.Component
Imports Common.Component.RVPHomeList
Imports Common.Component.HCVUUser
Imports Common.ComObject
Imports Common.DataAccess
Imports Common.Format
Imports Common.Validation
Imports Common.Component.Status
Imports Common.Component.StaticData
Imports HCVU.BLL

Partial Public Class RVPHomeListMaintenance
    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    ' -------------------------------------------------------------------------
    'Inherits BasePageWithGridView
    Inherits BasePageWithControl
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

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
    End Enum

    Public Enum ViewIndex
        SearchCriteria = 0
        SearchResult = 1
        Detail = 2
        Confirm = 3
        MsgPage = 4
        ErrorPage = 5
    End Enum

#Region "Fields"

    Private udtRVPHomeListBLL As New RVPHomeListBLL
    Private udtHCVUUserBLL As New HCVUUserBLL

    Private udtFormatter As New Formatter
    Private udtValidator As New Validator

    Private SM As SystemMessage
    Private udtAuditLogEntry As AuditLogEntry

    Private Const SESS_SearchResultList As String = FunctCode.FUNT011001 & "RVPHomeList_SearchResultList"
    Private Const SESS_RVPHomeListModal As String = FunctCode.FUNT011001 & "RVPHomeList_Modal"
#End Region

#Region "Audit Log Message"
    Private Const AuditMsg00000 As String = "RCH List Maintenance loaded"
    Private Const AuditMsg00001 As String = "Search"
    Private Const AuditMsg00002 As String = "Search completed. No record found"
    Private Const AuditMsg00003 As String = "Search completed"
    Private Const AuditMsg00004 As String = "Search failed"
    Private Const AuditMsg00005 As String = "Select: <RCH Code>"
    Private Const AuditMsg00006 As String = "Select failed: RCH List object not found"
    Private Const AuditMsg00007 As String = "Select completed"
    Private Const AuditMsg00008 As String = "Search Result Page -  Back Click"
    Private Const AuditMsg00009 As String = "Detail Page - Back Click"
    Private Const AuditMsg00010 As String = "Detail Page - Back Click failed"
    Private Const AuditMsg00011 As String = "New RCH Record Click"
    Private Const AuditMsg00012 As String = "Edit Click"
    Private Const AuditMsg00013 As String = "Save Click"
    Private Const AuditMsg00014 As String = "Cancel Click"
    Private Const AuditMsg00015 As String = "Activate Click"
    Private Const AuditMsg00016 As String = "Deactivate Click"
    Private Const AuditMsg00017 As String = "Confirm Click"
    Private Const AuditMsg00018 As String = "New RCH record successful"
    Private Const AuditMsg00019 As String = "Edit RCH record successful"
    Private Const AuditMsg00020 As String = "Activate RCH record successful"
    Private Const AuditMsg00021 As String = "Deactivate RCH record successful"
    Private Const AuditMsg00022 As String = "Update failed"
    Private Const AuditMsg00023 As String = "Confirm Page - Back Click"
    Private Const AuditMsg00024 As String = "Confirm Cancel - Yes Click"
    Private Const AuditMsg00025 As String = "Confirm Cancel - No Click"
    Private Const AuditMsg00026 As String = "Search failed"
    Private Const AuditMsg00027 As String = "Validation failed"
    Private Const AuditMsg00028 As String = "Validation failed"
    Private Const AuditMsg00029 As String = "Completion Page - Return Click"
    Private Const AuditMsg00030 As String = "Remove Click"
    Private Const AuditMsg00031 As String = "Confirm Remove - Yes Click"
    Private Const AuditMsg00032 As String = "Confirm Remove - No Click"
    Private Const AuditMsg00033 As String = "Confirm Activate - Yes Click"
    Private Const AuditMsg00034 As String = "Confirm Activate - No Click"
    Private Const AuditMsg00035 As String = "Confirm Deactivate - Yes Click"
    Private Const AuditMsg00036 As String = "Confirm Deactivate - No Click"
    Private Const AuditMsg00037 As String = "Remove failed"
    Private Const AuditMsg00038 As String = "Activate failed"
    Private Const AuditMsg00039 As String = "Reactivate failed"
    Private Const AuditMsg00040 As String = "Remove RCH record successful"
    Private Const AuditMsg00041 As String = "Validation failed"
#End Region

#Region "Page Events"

    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    ' -------------------------------------------------------------------------
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

        If blnOverrideResultLimit Then
            bllSearchResult = udtRVPHomeListBLL.GetRVPHomeListEnquirySearch(FunctCode.FUNT011001, _
                                                txtRCHCode.Text.Trim.ToUpper, _
                                                ddlRCHType.SelectedValue.Trim, _
                                                txtRCHName.Text.Trim.ToUpper, _
                                                txtRCHAddr.Text.Trim.ToUpper, _
                                                ddlRCHStatus.SelectedValue.Trim, True)
        Else
            bllSearchResult = udtRVPHomeListBLL.GetRVPHomeListEnquirySearch(FunctCode.FUNT011001, _
                                                            txtRCHCode.Text.Trim.ToUpper, _
                                                            ddlRCHType.SelectedValue.Trim, _
                                                            txtRCHName.Text.Trim.ToUpper, _
                                                            txtRCHAddr.Text.Trim.ToUpper, _
                                                            ddlRCHStatus.SelectedValue.Trim)
        End If

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

            Me.GridViewDataBind(gvResult, dt, "RCH_code", "ASC", False)

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
            enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, msgBox, CompleteMsgBox, False, True)

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
                Throw New Exception("Error: Class = [HCVU.RVPHomeListMaintenance], Method = [SF_ConfirmSearch_Click], Message = The unexpected error for the result of re-search")

        End Select
    End Sub

    Protected Overrides Sub SF_CancelSearch_Click()
    End Sub

#End Region
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        'Me.ibtnBack.Visible = True

        If Not Page.IsPostBack Then
            FunctionCode = FunctCode.FUNT011001

            ' Write Audit Log
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, AuditMsg00000)

            ddlRCHType.DataSource = (New StaticDataBLL).GetStaticDataListByColumnName("RCH_TYPE")
            ddlRCHType.DataValueField = "ItemNo"
            ddlRCHType.DataTextField = "DataValue"
            ddlRCHType.DataBind()

            ddlRCHType.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))

            ' Bind RCH Status
            ddlRCHStatus.DataSource = GetDescriptionListFromDBEnumCode("RCHStatus")
            ddlRCHStatus.DataValueField = "Status_Value"
            ddlRCHStatus.DataTextField = "Status_Description"
            ddlRCHStatus.DataBind()

            ddlRCHStatus.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))
        End If

        Me.ModalPopupConfirmCancel.PopupDragHandleControlID = Me.ucNoticePopUpConfirm.Header.ClientID
        'Me.ModalPopupConfirmCancel.CancelControlID = Me.ucNoticePopUpConfirm.ButtonCancel.ClientID
        Me.ModalPopupConfirmActivate.PopupDragHandleControlID = Me.ucNoticePopUpConfirmActivate.Header.ClientID
        'Me.ModalPopupConfirmActivate.CancelControlID = Me.ucNoticePopUpConfirmActivate.ButtonCancel.ClientID
        Me.ModalPopupConfirmDeactivate.PopupDragHandleControlID = Me.ucNoticePopUpConfirmDeactivate.Header.ClientID
        'Me.ModalPopupConfirmDeactivate.CancelControlID = Me.ucNoticePopUpConfirmDeactivate.ButtonCancel.ClientID

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If MultiViewEnquiry.ActiveViewIndex = ViewIndex.SearchCriteria Then
            pnlEnquiry.DefaultButton = ibtnSearch.ID
        End If

    End Sub

#End Region

    Protected Sub ibtnSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CompleteMsgBox.Visible = False
        msgBox.Visible = False
        imgRCHCodeAlert.Visible = False

        udcCollapsibleSearchCriteriaReview.Collapsed = True
        udcCollapsibleSearchCriteriaReview.ClientState = "True"

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
        ' -------------------------------------------------------------------------
        Dim enumSearchResult As SearchResultEnum
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

        ' Try-catch to avoid search over row limit (500)
        Try
            ' Write Audit Log
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.AddDescripton("RCH Code", txtRCHCode.Text.Trim.ToUpper)
            udtAuditLogEntry.AddDescripton("RCH Type", ddlRCHType.SelectedValue)
            udtAuditLogEntry.AddDescripton("RCH Name", txtRCHName.Text.Trim.ToUpper)
            udtAuditLogEntry.AddDescripton("RCH Addr", txtRCHAddr.Text.Trim.ToUpper)
            udtAuditLogEntry.AddDescripton("RCH Status", ddlRCHStatus.SelectedValue)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00001, AuditMsg00001)

            If sender Is Nothing Then
                enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, msgBox, CompleteMsgBox, False, True)
            Else
                enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, msgBox, CompleteMsgBox)
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
                    Throw New Exception("Error: Class = [HCVU.RVPHomeListMaintenance], Method = [ibtnSearch_Click], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")

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
            Throw ex
        End Try
    End Sub

    Private Sub FillSearchCriteria()
        If txtRCHCode.Text.Trim.Equals(String.Empty) Then
            lblResultRCHCode.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else
            lblResultRCHCode.Text = txtRCHCode.Text.Trim.ToUpper
        End If

        If ddlRCHType.SelectedValue.Trim.Equals(String.Empty) Then
            lblResultRCHType.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else
            lblResultRCHType.Text = ddlRCHType.SelectedItem.Text.Trim
        End If

        If txtRCHName.Text.Trim.Equals(String.Empty) Then
            lblResultRCHName.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else
            lblResultRCHName.Text = txtRCHName.Text.Trim.ToUpper
        End If

        If txtRCHAddr.Text.Trim.Equals(String.Empty) Then
            lblResultRCHAddr.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else
            lblResultRCHAddr.Text = txtRCHAddr.Text.Trim.ToUpper
        End If

        If ddlRCHStatus.SelectedValue.Trim.Equals(String.Empty) Then
            lblResultRCHStatus.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else
            lblResultRCHStatus.Text = ddlRCHStatus.SelectedItem.Text.Trim
        End If
    End Sub

    Private Sub BindRCHSummaryView(ByVal strRCHCode As String)
        If IsNothing(strRCHCode) OrElse strRCHCode.Trim = String.Empty Then Return

        Dim blnNotFind As Boolean = False

        Dim udtRVPHomeList As RVPHomeListModel = Nothing

        udtRVPHomeList = udtRVPHomeListBLL.GetRVPHomeListModalByCode(strRCHCode)

        If IsNothing(udtRVPHomeList) Then
            blnNotFind = True
        End If

        ' Write Audit Log
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("RCH Code", strRCHCode)
        udtAuditLogEntry.WriteLog(LogID.LOG00005, AuditMsg00005)

        If blnNotFind Then
            msgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
            msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00006, AuditMsg00006)

            ChangeViewIndex(ViewIndex.ErrorPage)
        Else

            BindRCHDetail(udtRVPHomeList)

            ChangeViewIndex(ViewIndex.Detail)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00007, AuditMsg00007)
        End If

    End Sub

    Protected Sub ibtnSearchResultBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00008, AuditMsg00008)

        'BackToSearchCriteriaView(True)
        CompleteMsgBox.Visible = False
        ChangeViewIndex(ViewIndex.SearchCriteria)
    End Sub

    Protected Sub ibtnBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00009, AuditMsg00009)

        msgBox.Visible = False
        CompleteMsgBox.Visible = False

        ChangeViewIndex(ViewIndex.SearchResult)
    End Sub

    Protected Sub ibtnErrorBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00010, AuditMsg00010)

        msgBox.Visible = False
        CompleteMsgBox.Visible = False

        BackToSearchCriteriaView(True)
    End Sub

    Protected Sub ibtnMsgBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00029, AuditMsg00029)

        BackToSearchCriteriaView(True)
    End Sub

    Protected Sub ibtnNew_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("RCH Code", txtRCHCode.Text.Trim.ToUpper)
        udtAuditLogEntry.WriteLog(LogID.LOG00011, AuditMsg00011)

        msgBox.Visible = False
        CompleteMsgBox.Visible = False

        ViewState("state") = StateType.ADD

        Dim strRCHCode As String = txtRCHCode.Text.Trim.ToUpper

        If (RCHCodeValidation(strRCHCode)) Then
            StartCreateMode(strRCHCode, False)
            ChangeViewIndex(ViewIndex.Detail)
        End If
    End Sub

    Protected Sub ibtnEdit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("RCH Code", lblDetailRCHCode.Text.Trim)
        udtAuditLogEntry.WriteLog(LogID.LOG00012, AuditMsg00012)

        msgBox.Visible = False
        CompleteMsgBox.Visible = False

        ViewState("state") = StateType.EDIT
        StartEditMode(True, False)
    End Sub

    Protected Sub ibtnSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim strRCHCode As String = String.Empty
        strRCHCode = lblDetailRCHCode.Text.Trim

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("RCH Code", strRCHCode)
        udtAuditLogEntry.WriteLog(LogID.LOG00013, AuditMsg00013)

        If RCHCodeValidation(strRCHCode) AndAlso RCHInfoValidation() Then
            If (ViewState("state") = StateType.ADD) Then
                ViewState("action") = ActionType.Create
                ' I-CRE16-003 Fix XSS [Start][Lawrence]
                hfDetailRCHStatus.Value = AntiXssEncoder.HtmlEncode(ddlDetailRCHStatus.SelectedValue, True)
                ' I-CRE16-003 Fix XSS [End][Lawrence]
            Else
                ViewState("action") = ActionType.Update
            End If

            ' I-CRE16-003 Fix XSS [Start][Lawrence]
            BindRCHConfirm(AntiXssEncoder.HtmlEncode(hfDetailRCHStatus.Value, True))
            ' I-CRE16-003 Fix XSS [End][Lawrence]
            ChangeViewIndex(ViewIndex.Confirm)
        End If
    End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("RCH Code", lblDetailRCHCode.Text.Trim)
        udtAuditLogEntry.WriteLog(LogID.LOG00014, AuditMsg00014)

        Me.ModalPopupConfirmCancel.Show()
    End Sub

    Protected Sub ibtnActive_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("RCH Code", lblDetailRCHCode.Text.Trim)
        udtAuditLogEntry.WriteLog(LogID.LOG00015, AuditMsg00015)

        'ViewState("state") = StateType.EDIT
        'ViewState("action") = ActionType.Active
        'BindRCHConfirm(RVPHomeListStatus.Active)
        'ChangeViewIndex(ViewIndex.Confirm)

        Me.ModalPopupConfirmActivate.Show()
    End Sub

    Protected Sub ibtnDeactive_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("RCH Code", lblDetailRCHCode.Text.Trim)
        udtAuditLogEntry.WriteLog(LogID.LOG00016, AuditMsg00016)

        'ViewState("state") = StateType.EDIT
        'ViewState("action") = ActionType.Deactive
        'BindRCHConfirm(RVPHomeListStatus.Inactive)
        'ChangeViewIndex(ViewIndex.Confirm)

        Me.ModalPopupConfirmDeactivate.Show()
    End Sub

    Protected Sub ibtnRemove_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("RCH Code", lblDetailRCHCode.Text.Trim)
        udtAuditLogEntry.WriteLog(LogID.LOG00030, AuditMsg00030)

        Dim udtEHSTransactionBLL As New EHSTransaction.EHSTransactionBLL

        If udtEHSTransactionBLL.CheckTransactionAdditionalFieldByRCHCode(lblDetailRCHCode.Text) Then
            msgBox.AddMessage(New SystemMessage(FunctCode.FUNT011001, SeverityCode.SEVE, MsgCode.MSG00006))
            msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00041, AuditMsg00041)
        Else
            ViewState("action") = ActionType.Remove
            Me.ModalPopupConfirmRemove.PopupDragHandleControlID = Me.ucNoticePopUpConfirmRemove.Header.ClientID
            'Me.ModalPopupConfirmRemove.CancelControlID = Me.ucNoticePopUpConfirmRemove.ButtonCancel.ClientID
            Me.ModalPopupConfirmRemove.TargetControlID = Me.ibtnRemove.ID
            Me.ModalPopupConfirmRemove.Show()
        End If

    End Sub

    Protected Sub ibtnConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        msgBox.Visible = False
        CompleteMsgBox.Visible = False

        Try
            ' Audit log
            udtAuditLogEntry.AddDescripton("RCH code", lblConfirmRCHCode.Text.Trim)
            udtAuditLogEntry.AddDescripton("Type", hfConfirmRCHType.Value)
            ' INT16-0008 (Fix Chinese in AntiXss change) [Start][Lawrence]
            udtAuditLogEntry.AddDescripton("Homename_Eng", txtDetailRCHNameEng.Text.Trim.ToUpper)
            udtAuditLogEntry.AddDescripton("Homename_Chi", txtDetailRCHNameChi.Text.Trim)
            udtAuditLogEntry.AddDescripton("Address_Eng", txtDetailRCHAddrEng.Text.Trim.ToUpper)
            udtAuditLogEntry.AddDescripton("Address_Chi", txtDetailRCHAddrChi.Text.Trim)
            ' INT16-0008 (Fix Chinese in AntiXss change) [End][Lawrence]
            udtAuditLogEntry.AddDescripton("Record_Status", hfConfirmRCHStatus.Value)

            udtAuditLogEntry.WriteStartLog(LogID.LOG00017, AuditMsg00017)

            Dim blnResult As New Boolean
            Dim udtRVPHomeList As New RVPHomeListModel
            Dim strUserID As String = udtHCVUUserBLL.GetHCVUUser.UserID.Trim

            If (ViewState("state") = StateType.EDIT) Then
                udtRVPHomeList = Session(SESS_RVPHomeListModal)
            End If

            udtRVPHomeList.RCHCode = lblConfirmRCHCode.Text.Trim
            udtRVPHomeList.Type = hfConfirmRCHType.Value
            ' INT16-0008 (Fix Chinese in RCH List Maintenance) [Start][Lawrence]
            udtRVPHomeList.HomenameEng = txtDetailRCHNameEng.Text.Trim.ToUpper
            udtRVPHomeList.HomenameChi = txtDetailRCHNameChi.Text.Trim
            udtRVPHomeList.AddressEng = txtDetailRCHAddrEng.Text.Trim.ToUpper
            udtRVPHomeList.AddressChi = txtDetailRCHAddrChi.Text.Trim
            ' INT16-0008 (Fix Chinese in RCH List Maintenance) [End][Lawrence]
            udtRVPHomeList.Status = hfConfirmRCHStatus.Value
            udtRVPHomeList.UpdateBy = strUserID

            udtAuditLogEntry.AddDescripton("RCH code", lblConfirmRCHCode.Text.Trim)
            If (ViewState("state") = StateType.ADD) Then
                blnResult = udtRVPHomeListBLL.AddRVPHomeList(udtRVPHomeList)
                If (blnResult) Then
                    CompleteMsgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT011001, SeverityCode.SEVI, MsgCode.MSG00001))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00018, AuditMsg00018)
                End If
            Else
                blnResult = udtRVPHomeListBLL.UpdateRVPHomeList(udtRVPHomeList)
                If (blnResult) Then
                    If (ViewState("action") = ActionType.Update) Then
                        CompleteMsgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT011001, SeverityCode.SEVI, MsgCode.MSG00002))
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00019, AuditMsg00019)
                    ElseIf (ViewState("action") = ActionType.Active) Then
                        CompleteMsgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT011001, SeverityCode.SEVI, MsgCode.MSG00003))
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00020, AuditMsg00020)
                    ElseIf (ViewState("action") = ActionType.Deactive) Then
                        CompleteMsgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT011001, SeverityCode.SEVI, MsgCode.MSG00004))
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00021, AuditMsg00021)
                    End If
                Else
                    msgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))

                End If
            End If

            If (blnResult) Then
                CompleteMsgBox.BuildMessageBox()
                CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Complete
                ChangeViewIndex(ViewIndex.MsgPage)
            Else
                WriteUpdateFailedAuditLog(lblConfirmRCHCode.Text.Trim)
            End If
        Catch ex As Exception
            ErrorHandler.Log(FunctCode.FUNT011001, SeverityCode.SEVE, "99999", Request.PhysicalPath, Request.UserHostAddress, ex.Message)
            msgBox.AddMessage(New SystemMessage(FunctCode.FUNT011001, SeverityCode.SEVE, MsgCode.MSG00005))
            WriteUpdateFailedAuditLog(lblConfirmRCHCode.Text.Trim)
        End Try
    End Sub

    Protected Sub ibtnConfirmBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00023, AuditMsg00023)

        msgBox.Visible = False
        CompleteMsgBox.Visible = False

        ChangeViewIndex(ViewIndex.Detail)

        Dim blnUpdateStatus As Boolean = True
        If (ViewState("action") = ActionType.Create Or ViewState("action") = ActionType.Update) Then
            blnUpdateStatus = False
        End If

        If (ViewState("state") = StateType.ADD) Then
            StartCreateMode(lblConfirmRCHCode.Text, True)
        Else
            StartEditMode(Not blnUpdateStatus, True)
        End If

    End Sub

    Private Sub WriteUpdateFailedAuditLog(ByVal strRCHCode As String)
        If msgBox.GetCodeTable.Rows.Count = 0 Then
            msgBox.Visible = False
        Else
            Dim strAction As String = ViewState("action")
            Dim udtExAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtExAuditLogEntry.AddDescripton("RCH code", strRCHCode)
            Select Case strAction
                Case ActionType.Update
                    msgBox.BuildMessageBox("UpdateFail", udtExAuditLogEntry, LogID.LOG00022, AuditMsg00022)
                Case ActionType.Active
                    msgBox.BuildMessageBox("UpdateFail", udtExAuditLogEntry, LogID.LOG00038, AuditMsg00038)
                Case ActionType.Deactive
                    msgBox.BuildMessageBox("UpdateFail", udtExAuditLogEntry, LogID.LOG00039, AuditMsg00039)
                Case ActionType.Remove
                    msgBox.BuildMessageBox("UpdateFail", udtExAuditLogEntry, LogID.LOG00037, AuditMsg00037)
            End Select

        End If
    End Sub

    Private Sub BackToSearchCriteriaView(ByVal blnIsReset As Boolean)
        CompleteMsgBox.Visible = False

        ChangeViewIndex(ViewIndex.SearchCriteria)
        If blnIsReset Then
            ResetSearchCriteria()
        Else
            ibtnSearch_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub BindRCHDetail(ByVal udtRVPHomeList As RVPHomeListModel)
        Session(SESS_RVPHomeListModal) = udtRVPHomeList

        lblDetailRCHCode.Text = udtRVPHomeList.RCHCode
        Dim udtStaticDataModel As StaticDataModel = (New StaticDataBLL).GetStaticDataByColumnNameItemNo("RCH_TYPE", udtRVPHomeList.Type)
        If udtStaticDataModel.ItemNo <> String.Empty Then
            lblDetailRCHType.Text = udtStaticDataModel.DataValue
        End If

        lblDetailRCHNameEng.Text = udtRVPHomeList.HomenameEng
        lblDetailRCHNameChi.Text = udtRVPHomeList.HomenameChi
        lblDetailRCHAddrEng.Text = udtRVPHomeList.AddressEng
        lblDetailRCHAddrChi.Text = udtRVPHomeList.AddressChi
        Status.GetDescriptionFromDBCode(RVPHomeListStatus.ClassCode, udtRVPHomeList.Status, lblDetailRCHStatus.Text, String.Empty)

        ddlDetailRCHType.SelectedValue = udtRVPHomeList.Type
        txtDetailRCHNameEng.Text = udtRVPHomeList.HomenameEng
        txtDetailRCHNameChi.Text = udtRVPHomeList.HomenameChi
        txtDetailRCHAddrEng.Text = udtRVPHomeList.AddressEng
        txtDetailRCHAddrChi.Text = udtRVPHomeList.AddressChi

        hfDetailRCHType.Value = udtRVPHomeList.Type
        hfDetailRCHStatus.Value = udtRVPHomeList.Status

        StartEditMode(False, False)

    End Sub

    Private Sub BindRCHConfirm(ByVal strRCHStatus As String)
        Dim strAction As String = ViewState("action")
        Select Case strAction
            Case ActionType.Create, ActionType.Update
                lblConfirmRCHCode.Text = lblDetailRCHCode.Text
                ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [Start][Lawrence]
                lblConfirmRCHType.Text = AntiXssEncoder.HtmlEncode(ddlDetailRCHType.SelectedItem.Text, True)
                ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [End][Lawrence]
                ' I-CRE16-003 Fix XSS [Start][Lawrence]
                lblConfirmRCHNameEng.Text = AntiXssEncoder.HtmlEncode(txtDetailRCHNameEng.Text, True).ToUpper()
                lblConfirmRCHNameChi.Text = AntiXssEncoder.HtmlEncode(txtDetailRCHNameChi.Text, True).ToUpper()
                lblConfirmRCHAddrEng.Text = AntiXssEncoder.HtmlEncode(txtDetailRCHAddrEng.Text, True).ToUpper()
                lblConfirmRCHAddrChi.Text = AntiXssEncoder.HtmlEncode(txtDetailRCHAddrChi.Text, True).ToUpper()
                Status.GetDescriptionFromDBCode(RVPHomeListStatus.ClassCode, strRCHStatus, lblConfirmRCHStatus.Text, String.Empty)

                hfConfirmRCHType.Value = AntiXssEncoder.HtmlEncode(ddlDetailRCHType.SelectedValue, True)
                ' I-CRE16-003 Fix XSS [End][Lawrence]

            Case ActionType.Active, ActionType.Deactive
                lblConfirmRCHCode.Text = lblDetailRCHCode.Text
                lblConfirmRCHType.Text = lblDetailRCHType.Text
                lblConfirmRCHNameEng.Text = lblDetailRCHNameEng.Text
                lblConfirmRCHNameChi.Text = lblDetailRCHNameChi.Text
                lblConfirmRCHAddrEng.Text = lblDetailRCHAddrEng.Text
                lblConfirmRCHAddrChi.Text = lblDetailRCHAddrChi.Text
                Status.GetDescriptionFromDBCode(RVPHomeListStatus.ClassCode, strRCHStatus, lblConfirmRCHStatus.Text, String.Empty)

                ' I-CRE16-003 Fix XSS [Start][Lawrence]
                hfConfirmRCHType.Value = AntiXssEncoder.HtmlEncode(hfDetailRCHType.Value, True)
                ' I-CRE16-003 Fix XSS [End][Lawrence]

            Case Else
                ' Do nothing
        End Select

        hfConfirmRCHStatus.Value = strRCHStatus

        CompleteMsgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00021))
        CompleteMsgBox.BuildMessageBox()
        CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information
    End Sub

#Region "Popup box events"

    '---------------------------------------------------------------------------------------------------------
    'Confirmation Box
    '---------------------------------------------------------------------------------------------------------
    Private Sub ucNoticePopUpConfirm_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUpConfirm.ButtonClick
        Select Case e
            Case ucNoticePopUp.enumButtonClick.OK
                Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
                udtAuditLogEntry.WriteLog(LogID.LOG00024, AuditMsg00024)

                msgBox.Visible = False
                CompleteMsgBox.Visible = False

                imgRCHTypeAlert.Visible = False
                imgRCHNameEngAlert.Visible = False
                imgRCHNameChiAlert.Visible = False
                imgRCHAddrEngAlert.Visible = False
                imgRCHAddrChiAlert.Visible = False
                imgRCHStatusAlert.Visible = False

                If (ViewState("state") = StateType.ADD) Then
                    ChangeViewIndex(ViewIndex.SearchCriteria)
                ElseIf (ViewState("state") = StateType.EDIT) Then
                    StartEditMode(False, False)
                    ChangeViewIndex(ViewIndex.Detail)
                End If

            Case Else
                Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
                udtAuditLogEntry.WriteLog(LogID.LOG00025, AuditMsg00025)
                ' Do nothing
        End Select

    End Sub

    Private Sub ucNoticePopUpConfirmActivate_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUpConfirmActivate.ButtonClick

        msgBox.Visible = False
        CompleteMsgBox.Visible = False

        Select Case e
            Case ucNoticePopUp.enumButtonClick.OK
                Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
                udtAuditLogEntry.WriteLog(LogID.LOG00033, AuditMsg00033)

                Try
                    Dim blnResult As New Boolean
                    Dim udtRVPHomeList As New RVPHomeListModel

                    udtRVPHomeList = Session(SESS_RVPHomeListModal)
                    udtRVPHomeList.Status = RVPHomeListStatus.Active
                    udtRVPHomeList.UpdateBy = udtHCVUUserBLL.GetHCVUUser.UserID.Trim

                    udtAuditLogEntry.AddDescripton("RCH code", lblConfirmRCHCode.Text.Trim)

                    blnResult = udtRVPHomeListBLL.UpdateRVPHomeList(udtRVPHomeList)
                    If (blnResult) Then

                        CompleteMsgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT011001, SeverityCode.SEVI, MsgCode.MSG00003))
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00020, AuditMsg00020)

                    Else
                        msgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
                    End If

                    If (blnResult) Then
                        CompleteMsgBox.BuildMessageBox()
                        CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Complete
                        ChangeViewIndex(ViewIndex.MsgPage)
                    Else
                        ViewState("action") = ActionType.Active
                        WriteUpdateFailedAuditLog(lblConfirmRCHCode.Text.Trim)
                    End If
                Catch ex As Exception
                    ErrorHandler.Log(FunctCode.FUNT011001, SeverityCode.SEVE, "99999", Request.PhysicalPath, Request.UserHostAddress, ex.Message)
                    msgBox.AddMessage(New SystemMessage(FunctCode.FUNT011001, SeverityCode.SEVE, MsgCode.MSG00005))
                    WriteUpdateFailedAuditLog(lblConfirmRCHCode.Text.Trim)
                End Try

            Case Else
                Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
                udtAuditLogEntry.WriteLog(LogID.LOG00034, AuditMsg00034)

        End Select

    End Sub

    Private Sub ucNoticePopUpConfirmDeactivate_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUpConfirmDeactivate.ButtonClick

        msgBox.Visible = False
        CompleteMsgBox.Visible = False

        Select Case e
            Case ucNoticePopUp.enumButtonClick.OK

                Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
                udtAuditLogEntry.WriteLog(LogID.LOG00035, AuditMsg00035)

                Try
                    Dim blnResult As New Boolean
                    Dim udtRVPHomeList As New RVPHomeListModel

                    udtRVPHomeList = Session(SESS_RVPHomeListModal)
                    udtRVPHomeList.Status = RVPHomeListStatus.Inactive
                    udtRVPHomeList.UpdateBy = udtHCVUUserBLL.GetHCVUUser.UserID.Trim

                    udtAuditLogEntry.AddDescripton("RCH code", lblConfirmRCHCode.Text.Trim)

                    blnResult = udtRVPHomeListBLL.UpdateRVPHomeList(udtRVPHomeList)
                    If (blnResult) Then
                        CompleteMsgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT011001, SeverityCode.SEVI, MsgCode.MSG00004))
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00021, AuditMsg00021)
                    Else
                        msgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
                    End If

                    If (blnResult) Then
                        CompleteMsgBox.BuildMessageBox()
                        CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Complete
                        ChangeViewIndex(ViewIndex.MsgPage)
                    Else
                        ViewState("action") = ActionType.Deactive
                        WriteUpdateFailedAuditLog(lblConfirmRCHCode.Text.Trim)
                    End If
                Catch ex As Exception
                    ErrorHandler.Log(FunctCode.FUNT011001, SeverityCode.SEVE, "99999", Request.PhysicalPath, Request.UserHostAddress, ex.Message)
                    msgBox.AddMessage(New SystemMessage(FunctCode.FUNT011001, SeverityCode.SEVE, MsgCode.MSG00005))
                    WriteUpdateFailedAuditLog(lblConfirmRCHCode.Text.Trim)
                End Try

            Case Else
                Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
                udtAuditLogEntry.WriteLog(LogID.LOG00036, AuditMsg00036)
        End Select

    End Sub

    Private Sub ucNoticePopUpConfirmRemove_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUpConfirmRemove.ButtonClick

        msgBox.Visible = False
        CompleteMsgBox.Visible = False

        Select Case e
            Case ucNoticePopUp.enumButtonClick.OK
                Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
                udtAuditLogEntry.WriteLog(LogID.LOG00031, AuditMsg00031)

                Try
                    Dim blnResult As New Boolean
                    Dim udtRVPHomeList As New RVPHomeListModel

                    udtRVPHomeList = Session(SESS_RVPHomeListModal)

                    udtAuditLogEntry.AddDescripton("RCH code", lblConfirmRCHCode.Text.Trim)

                    blnResult = udtRVPHomeListBLL.DeleteRVPHomeList(udtRVPHomeList)
                    If (blnResult) Then
                        CompleteMsgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT011001, SeverityCode.SEVI, MsgCode.MSG00005))
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00040, AuditMsg00040)
                    Else
                        msgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
                    End If

                    If (blnResult) Then
                        CompleteMsgBox.BuildMessageBox()
                        CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Complete
                        ChangeViewIndex(ViewIndex.MsgPage)
                    Else
                        ViewState("action") = ActionType.Remove
                        WriteUpdateFailedAuditLog(lblConfirmRCHCode.Text.Trim)
                    End If
                Catch ex As Exception
                    ErrorHandler.Log(FunctCode.FUNT011001, SeverityCode.SEVE, "99999", Request.PhysicalPath, Request.UserHostAddress, ex.Message)
                    msgBox.AddMessage(New SystemMessage(FunctCode.FUNT011001, SeverityCode.SEVE, MsgCode.MSG00005))
                    WriteUpdateFailedAuditLog(lblConfirmRCHCode.Text.Trim)
                End Try

            Case Else
                Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
                udtAuditLogEntry.WriteLog(LogID.LOG00032, AuditMsg00032)
                ' Do nothing
        End Select

    End Sub
#End Region

    Private Sub ChangeViewIndex(ByVal udtViewIndex As ViewIndex)
        MultiViewEnquiry.ActiveViewIndex = udtViewIndex

        Select Case udtViewIndex
            Case ViewIndex.SearchCriteria
                Session.Remove(SESS_RVPHomeListModal)
            Case ViewIndex.SearchResult
                Session.Remove(SESS_RVPHomeListModal)
            Case ViewIndex.Detail
            Case ViewIndex.Confirm
            Case ViewIndex.MsgPage
            Case ViewIndex.ErrorPage
        End Select
    End Sub

    Private Sub StartCreateMode(ByVal strRCHCode As String, ByVal blnIsBack As Boolean)
        ddlDetailRCHType.Visible = True
        txtDetailRCHNameEng.Visible = True
        txtDetailRCHNameChi.Visible = True
        txtDetailRCHAddrEng.Visible = True
        txtDetailRCHAddrChi.Visible = True
        ddlDetailRCHStatus.Visible = True

        lblDetailRCHType.Visible = False
        lblDetailRCHNameEng.Visible = False
        lblDetailRCHNameChi.Visible = False
        lblDetailRCHAddrEng.Visible = False
        lblDetailRCHAddrChi.Visible = False
        lblDetailRCHStatus.Visible = False

        ibtnEdit.Visible = False
        ibtnSave.Visible = True
        ibtnCancel.Visible = True
        ibtnBack.Visible = False
        ibtnActive.Visible = False
        ibtnDeactive.Visible = False
        ibtnRemove.Visible = False

        If (Not blnIsBack) Then
            ddlDetailRCHType.Items.Clear()
            ddlDetailRCHType.DataSource = (New StaticDataBLL).GetStaticDataListByColumnName("RCH_TYPE")
            ddlDetailRCHType.DataValueField = "ItemNo"
            ddlDetailRCHType.DataTextField = "DataValue"
            ddlDetailRCHType.DataBind()
            ddlDetailRCHType.Items.Insert(0, New ListItem(CStr(GetGlobalResourceObject("Text", "PleaseSelect")), ""))

            ddlDetailRCHStatus.Items.Clear()
            ddlDetailRCHStatus.DataSource = GetDescriptionListFromDBEnumCode("RCHStatus")
            ddlDetailRCHStatus.DataValueField = "Status_Value"
            ddlDetailRCHStatus.DataTextField = "Status_Description"
            ddlDetailRCHStatus.DataBind()
            ddlDetailRCHStatus.Items.Insert(0, New ListItem(CStr(GetGlobalResourceObject("Text", "PleaseSelect")), ""))

            lblDetailRCHCode.Text = strRCHCode.ToUpper()
            txtDetailRCHNameEng.Text = String.Empty
            txtDetailRCHNameChi.Text = String.Empty
            txtDetailRCHAddrEng.Text = String.Empty
            txtDetailRCHAddrChi.Text = String.Empty

            'Status.GetDescriptionFromDBCode(RVPHomeListStatus.ClassCode, RVPHomeListStatus.Active, lblDetailRCHStatus.Text, String.Empty)
            hfDetailRCHStatus.Value = RVPHomeListStatus.Active
        End If
    End Sub

    Private Sub StartEditMode(ByVal blnIsEdit As Boolean, ByVal blnIsBack As Boolean)
        ddlDetailRCHType.Visible = blnIsEdit
        txtDetailRCHNameEng.Visible = blnIsEdit
        txtDetailRCHNameChi.Visible = blnIsEdit
        txtDetailRCHAddrEng.Visible = blnIsEdit
        txtDetailRCHAddrChi.Visible = blnIsEdit
        ddlDetailRCHStatus.Visible = False

        lblDetailRCHType.Visible = Not blnIsEdit
        lblDetailRCHNameEng.Visible = Not blnIsEdit
        lblDetailRCHNameChi.Visible = Not blnIsEdit
        lblDetailRCHAddrEng.Visible = Not blnIsEdit
        lblDetailRCHAddrChi.Visible = Not blnIsEdit
        lblDetailRCHStatus.Visible = True

        ibtnEdit.Visible = Not blnIsEdit
        ibtnSave.Visible = blnIsEdit
        ibtnCancel.Visible = blnIsEdit
        ibtnBack.Visible = Not blnIsEdit

        If (blnIsEdit) Then
            ibtnActive.Visible = Not blnIsEdit
            ibtnDeactive.Visible = Not blnIsEdit
            ibtnRemove.Visible = Not blnIsEdit

            If (Not blnIsBack) Then
                ddlDetailRCHType.Items.Clear()
                ddlDetailRCHType.DataSource = (New StaticDataBLL).GetStaticDataListByColumnName("RCH_TYPE")
                ddlDetailRCHType.DataValueField = "ItemNo"
                ddlDetailRCHType.DataTextField = "DataValue"
                ddlDetailRCHType.DataBind()

                ' I-CRE16-003 Fix XSS [Start][Lawrence]
                ddlDetailRCHType.SelectedValue = AntiXssEncoder.HtmlEncode(hfDetailRCHType.Value, True)
                ' I-CRE16-003 Fix XSS [End][Lawrence]
                txtDetailRCHNameEng.Text = lblDetailRCHNameEng.Text
                txtDetailRCHNameChi.Text = lblDetailRCHNameChi.Text
                txtDetailRCHAddrEng.Text = lblDetailRCHAddrEng.Text
                txtDetailRCHAddrChi.Text = lblDetailRCHAddrChi.Text
            End If
        Else
            ibtnRemove.Visible = True
            Select Case hfDetailRCHStatus.Value
                Case RVPHomeListStatus.Active
                    ibtnActive.Visible = False
                    ibtnDeactive.Visible = True
                Case RVPHomeListStatus.Inactive
                    ibtnActive.Visible = True
                    ibtnDeactive.Visible = False
            End Select
        End If
    End Sub

    Private Sub ResetSearchCriteria()
        txtRCHCode.Text = String.Empty
        ddlRCHType.SelectedValue = String.Empty
        txtRCHName.Text = String.Empty
        txtRCHAddr.Text = String.Empty
        ddlRCHStatus.SelectedValue = String.Empty
    End Sub

    Private Sub gvResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' RCH Type
            Dim lblRCHType As Label = CType(e.Row.FindControl("lblRCHType"), Label)
            Dim udtStaticDataModel As StaticDataModel = (New StaticDataBLL).GetStaticDataByColumnNameItemNo("RCH_TYPE", lblRCHType.Text.Trim)
            If udtStaticDataModel.ItemNo <> String.Empty Then
                lblRCHType.Text = udtStaticDataModel.DataValue
            End If

            ' Status
            Dim lblRCHStatus As Label = CType(e.Row.FindControl("lblRCHStatus"), Label)
            Status.GetDescriptionFromDBCode(RVPHomeListStatus.ClassCode, lblRCHStatus.Text.Trim, lblRCHStatus.Text, String.Empty)

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
            Dim strRCHCode As String = CType(r.FindControl("lnkbtnRCHCode"), LinkButton).CommandArgument.Trim

            BindRCHSummaryView(strRCHCode)
        End If
    End Sub

    Private Sub gvResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvResult.Sorting
        Me.GridViewSortingHandler(sender, e, SESS_SearchResultList)
    End Sub

#Region "Validation"
    Private Function SearchValidation() As Boolean
        Dim blnValid As Boolean = True

        If (txtRCHCode.Text.Trim = String.Empty AndAlso txtRCHName.Text.Trim = String.Empty AndAlso txtRCHAddr.Text.Trim = String.Empty _
            AndAlso ddlRCHType.SelectedValue = String.Empty AndAlso ddlRCHStatus.SelectedValue = String.Empty) Then
            msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00257))
            blnValid = False
        End If

        If msgBox.GetCodeTable.Rows.Count = 0 Then
            msgBox.Visible = False
        Else
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            msgBox.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00026, "Search failed")
        End If

        Return blnValid
    End Function

    Private Function RCHCodeValidation(ByVal strRCHCode As String) As Boolean
        msgBox.Visible = False
        imgRCHCodeAlert.Visible = False

        Dim blnValid As Boolean = True

        ' Check empty of RCH Code
        If strRCHCode = String.Empty Then
            msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT011001, SeverityCode.SEVE, MsgCode.MSG00003))
            blnValid = False
        Else
            ' Check the validity of RCH Code
            'Dim udtValidator As New Validator()
            'Dim SysMsg As SystemMessage = udtValidator.chkRCHCode(strRCHCode)

            'If Not SysMsg Is Nothing Then
            '    msgBox.AddMessage(SysMsg)
            '    blnValid = False
            ' Check duplication of RCH Code
            If ViewState("state") = StateType.ADD Then
                Dim dt As DataTable = udtRVPHomeListBLL.getRVPHomeListByCode(strRCHCode)
                If dt.Rows.Count > 0 Then
                    msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT011001, SeverityCode.SEVE, MsgCode.MSG00001))
                    blnValid = False
                End If
            End If
        End If

        If msgBox.GetCodeTable.Rows.Count = 0 Then
            msgBox.Visible = False
        Else
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00027, AuditMsg00027)
            imgRCHCodeAlert.Visible = True
        End If

        Return blnValid
    End Function

    Private Function RCHInfoValidation() As Boolean
        Dim blnValid As Boolean = True
        imgRCHTypeAlert.Visible = False
        imgRCHNameEngAlert.Visible = False
        imgRCHNameChiAlert.Visible = False
        imgRCHAddrEngAlert.Visible = False
        imgRCHAddrChiAlert.Visible = False
        imgRCHStatusAlert.Visible = False

        ' Check Mandatory Fields
        If ddlDetailRCHType.SelectedValue = String.Empty Then
            msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT011001, SeverityCode.SEVE, MsgCode.MSG00002), "%s", GetGlobalResourceObject("Text", "RCHType"))
            imgRCHTypeAlert.Visible = True
            blnValid = False
        End If
        If txtDetailRCHNameEng.Text.Trim = String.Empty Then
            msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT011001, SeverityCode.SEVE, MsgCode.MSG00002), "%s", GetGlobalResourceObject("Text", "RCHName") & " (" & GetGlobalResourceObject("text", "InEnglish") & ")")
            imgRCHNameEngAlert.Visible = True
            blnValid = False
        End If
        If txtDetailRCHNameChi.Text.Trim = String.Empty Then
            msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT011001, SeverityCode.SEVE, MsgCode.MSG00002), "%s", GetGlobalResourceObject("Text", "RCHName") & " (" & GetGlobalResourceObject("text", "InChinese") & ")")
            imgRCHNameChiAlert.Visible = True
            blnValid = False
        End If
        If txtDetailRCHAddrEng.Text.Trim = String.Empty Then
            msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT011001, SeverityCode.SEVE, MsgCode.MSG00002), "%s", GetGlobalResourceObject("Text", "RCHAddr") & " (" & GetGlobalResourceObject("text", "InEnglish") & ")")
            imgRCHAddrEngAlert.Visible = True
            blnValid = False
        End If
        If txtDetailRCHAddrChi.Text.Trim = String.Empty Then
            msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT011001, SeverityCode.SEVE, MsgCode.MSG00002), "%s", GetGlobalResourceObject("Text", "RCHAddr") & " (" & GetGlobalResourceObject("text", "InChinese") & ")")
            imgRCHAddrChiAlert.Visible = True
            blnValid = False
        End If
        If ViewState("state") = StateType.ADD AndAlso ddlDetailRCHStatus.SelectedValue = String.Empty Then
            msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT011001, SeverityCode.SEVE, MsgCode.MSG00002), "%s", GetGlobalResourceObject("Text", "RCHStatus"))
            imgRCHStatusAlert.Visible = True
            blnValid = False
        End If

        If msgBox.GetCodeTable.Rows.Count = 0 Then
            msgBox.Visible = False
        Else
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00028, AuditMsg00028)
        End If

        Return blnValid
    End Function
#End Region

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

End Class