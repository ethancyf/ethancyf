Imports System.Web.Security.AntiXss
Imports Common.Component
Imports Common.Component.COVID19
Imports Common.Component.HCVUUser
Imports Common.ComObject
Imports Common.DataAccess
Imports Common.Format
Imports Common.Validation
Imports Common.Component.Status
Imports Common.Component.StaticData
Imports Common.Component.EHSTransaction
Imports HCVU.BLL

Public Class OutreachListMaintenance
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

    Private udtOutreachListBLL As New OutreachListBLL
    Private udtHCVUUserBLL As New HCVUUserBLL

    Private udtFormatter As New Formatter
    Private udtValidator As New Validator

    Private SM As SystemMessage
    Private udtAuditLogEntry As AuditLogEntry

    Private Const SESS_SearchResultList As String = FunctCode.FUNT011002 & "OutreachList_SearchResultList"
    Private Const SESS_OutreachListModal As String = FunctCode.FUNT011002 & "OutreachList_Modal"
#End Region


#Region "Audit Log Message"
    Private Const AuditMsg00000 As String = "Outreach List Maintenance loaded"
    Private Const AuditMsg00001 As String = "Search"
    Private Const AuditMsg00002 As String = "Search completed. No record found"
    Private Const AuditMsg00003 As String = "Search completed"
    Private Const AuditMsg00004 As String = "Search failed"
    Private Const AuditMsg00005 As String = "Select: <Outreach Code>"
    Private Const AuditMsg00006 As String = "Select failed: Outreach List object not found"
    Private Const AuditMsg00007 As String = "Select completed"
    Private Const AuditMsg00008 As String = "Search Result Page -  Back Click"
    Private Const AuditMsg00009 As String = "Detail Page - Back Click"
    Private Const AuditMsg00010 As String = "Detail Page - Back Click failed"
    Private Const AuditMsg00011 As String = "New Outreach Record Click"
    Private Const AuditMsg00012 As String = "Edit Click"
    Private Const AuditMsg00013 As String = "Save Click"
    Private Const AuditMsg00014 As String = "Cancel Click"
    Private Const AuditMsg00015 As String = "Activate Click"
    Private Const AuditMsg00016 As String = "Deactivate Click"
    Private Const AuditMsg00017 As String = "Confirm Click"
    Private Const AuditMsg00018 As String = "New Outreach record successful"
    Private Const AuditMsg00019 As String = "Edit Outreach record successful"
    Private Const AuditMsg00020 As String = "Activate Outreach record successful"
    Private Const AuditMsg00021 As String = "Deactivate Outreach record successful"
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
    Private Const AuditMsg00040 As String = "Remove Outreach record successful"
    Private Const AuditMsg00041 As String = "Validation failed"

#End Region

#Region "Page Events"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")



        If Not Page.IsPostBack Then
            FunctionCode = FunctCode.FUNT011002

            ' Write Audit Log
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, AuditMsg00000)
            ' Bind Outreach Status
            ddlOutreachStatus.DataSource = GetDescriptionListFromDBEnumCode("OutreachStatus")
            ddlOutreachStatus.DataValueField = "Status_Value"
            ddlOutreachStatus.DataTextField = "Status_Description"
            ddlOutreachStatus.DataBind()

            ddlOutreachStatus.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))
        End If

        Me.ModalPopupConfirmCancel.PopupDragHandleControlID = Me.ucNoticePopUpConfirm.Header.ClientID
        Me.ModalPopupConfirmActivate.PopupDragHandleControlID = Me.ucNoticePopUpConfirmActivate.Header.ClientID
        Me.ModalPopupConfirmDeactivate.PopupDragHandleControlID = Me.ucNoticePopUpConfirmDeactivate.Header.ClientID

    End Sub
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
        Dim bllSearchResult As Common.Component.BaseBLL.BLLSearchResult

        If blnOverrideResultLimit Then
            bllSearchResult = udtOutreachListBLL.GetOutreachListEnquirySearch(txtOutreachCode.Text.Trim, _
                                                txtOutreachName.Text.Trim, _
                                                txtOutreachAddr.Text.Trim, _
                                                ddlOutreachStatus.SelectedValue.Trim, True)
        Else
            bllSearchResult = udtOutreachListBLL.GetOutreachListEnquirySearch(txtOutreachCode.Text.Trim, _
                                                            txtOutreachName.Text.Trim, _
                                                            txtOutreachAddr.Text.Trim, _
                                                            ddlOutreachStatus.SelectedValue.Trim)
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
                'udtAuditLogEntry.WriteEndLog(LogID.LOG00002, AuditMsg00002)

            Case Else
                blnShowResultList = True

        End Select

        If blnShowResultList Then
            FillSearchCriteria()
            'gvResult.DataSource = dt
            'gvResult.DataBind()

            Session(SESS_SearchResultList) = dt

            Me.GridViewDataBind(gvResult, dt, "Outreach_code", "ASC", False)

            ChangeViewIndex(ViewIndex.SearchResult)

            'udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditMsg00003)

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
                Throw New Exception("Error: Class = [HCVU.OutreachListMaintenance], Method = [SF_ConfirmSearch_Click], Message = The unexpected error for the result of re-search")

        End Select
    End Sub

    Protected Overrides Sub SF_CancelSearch_Click()
    End Sub

#End Region

#Region "Support Function"

    Private Sub ChangeViewIndex(ByVal udtViewIndex As ViewIndex)
        MultiViewEnquiry.ActiveViewIndex = udtViewIndex

        Select Case udtViewIndex
            Case ViewIndex.SearchCriteria
                Session.Remove(SESS_OutreachListModal)
            Case ViewIndex.SearchResult
                Session.Remove(SESS_OutreachListModal)
            Case ViewIndex.Detail
            Case ViewIndex.Confirm
            Case ViewIndex.MsgPage
            Case ViewIndex.ErrorPage
        End Select
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

    Private Sub StartCreateMode(ByVal strOutreachCode As String, ByVal blnIsBack As Boolean)
        txtDetailOutreachNameEng.Visible = True
        txtDetailOutreachNameChi.Visible = True
        txtDetailOutreachAddrEng.Visible = True
        txtDetailOutreachAddrChi.Visible = True
        ddlDetailOutreachStatus.Visible = True

        lblDetailOutreachNameEng.Visible = False
        lblDetailOutreachNameChi.Visible = False
        lblDetailOutreachAddrEng.Visible = False
        lblDetailOutreachAddrChi.Visible = False
        lblDetailOutreachStatus.Visible = False

        ibtnEdit.Visible = False
        ibtnSave.Visible = True
        ibtnCancel.Visible = True
        ibtnBack.Visible = False
        ibtnActive.Visible = False
        ibtnDeactive.Visible = False
        ibtnRemove.Visible = False

        If (Not blnIsBack) Then
            ddlDetailOutreachStatus.Items.Clear()
            ddlDetailOutreachStatus.DataSource = GetDescriptionListFromDBEnumCode("OutreachStatus")
            ddlDetailOutreachStatus.DataValueField = "Status_Value"
            ddlDetailOutreachStatus.DataTextField = "Status_Description"
            ddlDetailOutreachStatus.DataBind()
            ddlDetailOutreachStatus.Items.Insert(0, New ListItem(CStr(GetGlobalResourceObject("Text", "PleaseSelect")), ""))

            lblDetailOutreachCode.Text = strOutreachCode.ToUpper()
            txtDetailOutreachNameEng.Text = String.Empty
            txtDetailOutreachNameChi.Text = String.Empty
            txtDetailOutreachAddrEng.Text = String.Empty
            txtDetailOutreachAddrChi.Text = String.Empty

            hfDetailOutreachStatus.Value = OutreachListStatus.Active
        End If
    End Sub

    Private Sub StartEditMode(ByVal blnIsEdit As Boolean, ByVal blnIsBack As Boolean)
        txtDetailOutreachNameEng.Visible = blnIsEdit
        txtDetailOutreachNameChi.Visible = blnIsEdit
        txtDetailOutreachAddrEng.Visible = blnIsEdit
        txtDetailOutreachAddrChi.Visible = blnIsEdit
        ddlDetailOutreachStatus.Visible = False

        lblDetailOutreachNameEng.Visible = Not blnIsEdit
        lblDetailOutreachNameChi.Visible = Not blnIsEdit
        lblDetailOutreachAddrEng.Visible = Not blnIsEdit
        lblDetailOutreachAddrChi.Visible = Not blnIsEdit
        lblDetailOutreachStatus.Visible = True

        ibtnEdit.Visible = Not blnIsEdit
        ibtnSave.Visible = blnIsEdit
        ibtnCancel.Visible = blnIsEdit
        ibtnBack.Visible = Not blnIsEdit

        If (blnIsEdit) Then
            ibtnActive.Visible = Not blnIsEdit
            ibtnDeactive.Visible = Not blnIsEdit
            ibtnRemove.Visible = Not blnIsEdit

            If (Not blnIsBack) Then
                txtDetailOutreachNameEng.Text = lblDetailOutreachNameEng.Text
                txtDetailOutreachNameChi.Text = lblDetailOutreachNameChi.Text
                txtDetailOutreachAddrEng.Text = lblDetailOutreachAddrEng.Text
                txtDetailOutreachAddrChi.Text = lblDetailOutreachAddrChi.Text
            End If
        Else
            ibtnRemove.Visible = True
            Select Case hfDetailOutreachStatus.Value
                Case OutreachListStatus.Active
                    ibtnActive.Visible = False
                    ibtnDeactive.Visible = True
                Case OutreachListStatus.Inactive
                    ibtnActive.Visible = True
                    ibtnDeactive.Visible = False
            End Select
        End If
    End Sub


    Private Sub ResetSearchCriteria()
        txtOutreachCode.Text = String.Empty
        txtOutreachName.Text = String.Empty
        txtOutreachAddr.Text = String.Empty
        ddlOutreachStatus.SelectedValue = String.Empty
    End Sub

    Private Sub FillSearchCriteria()
        If txtOutreachCode.Text.Trim.Equals(String.Empty) Then
            lblResultOutreachCode.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else
            lblResultOutreachCode.Text = txtOutreachCode.Text.Trim.ToUpper
        End If

        If txtOutreachName.Text.Trim.Equals(String.Empty) Then
            lblResultOutreachName.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else
            lblResultOutreachName.Text = txtOutreachName.Text.Trim
        End If

        If txtOutreachAddr.Text.Trim.Equals(String.Empty) Then
            lblResultOutreachAddr.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else
            lblResultOutreachAddr.Text = txtOutreachAddr.Text.Trim
        End If

        If ddlOutreachStatus.SelectedValue.Trim.Equals(String.Empty) Then
            lblResultOutreachStatus.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else
            lblResultOutreachStatus.Text = ddlOutreachStatus.SelectedItem.Text.Trim
        End If
    End Sub

    Private Sub BindOutreachSummaryView(ByVal strOutreachCode As String)
        If IsNothing(strOutreachCode) OrElse strOutreachCode.Trim = String.Empty Then Return

        Dim blnNotFind As Boolean = False

        Dim udtOutreachList As OutreachListModel = Nothing

        udtOutreachList = udtOutreachListBLL.GetOutreachListModalByCode(strOutreachCode)

        If IsNothing(udtOutreachList) Then
            blnNotFind = True
        End If

        ' Write Audit Log
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Outreach Code", strOutreachCode)
        udtAuditLogEntry.WriteLog(LogID.LOG00005, AuditMsg00005)

        If blnNotFind Then
            msgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
            msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00006, AuditMsg00006)

            ChangeViewIndex(ViewIndex.ErrorPage)
        Else

            BindOutreachDetail(udtOutreachList)

            ChangeViewIndex(ViewIndex.Detail)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00007, AuditMsg00007)
        End If

    End Sub


    Private Sub BindOutreachDetail(ByVal udtOutreachList As OutreachListModel)
        Session(SESS_OutreachListModal) = udtOutreachList

        lblDetailOutreachCode.Text = udtOutreachList.OutreachCode
        lblDetailOutreachNameEng.Text = udtOutreachList.OutreachNameEng
        lblDetailOutreachNameChi.Text = udtOutreachList.OutreachNameChi
        lblDetailOutreachAddrEng.Text = udtOutreachList.AddressEng
        lblDetailOutreachAddrChi.Text = udtOutreachList.AddressChi
        Status.GetDescriptionFromDBCode(OutreachListStatus.ClassCode, udtOutreachList.Status, lblDetailOutreachStatus.Text, String.Empty)


        txtDetailOutreachNameEng.Text = udtOutreachList.OutreachNameEng
        txtDetailOutreachNameChi.Text = udtOutreachList.OutreachNameChi
        txtDetailOutreachAddrEng.Text = udtOutreachList.AddressEng
        txtDetailOutreachAddrChi.Text = udtOutreachList.AddressChi


        hfDetailOutreachStatus.Value = udtOutreachList.Status

        StartEditMode(False, False)

    End Sub

    Private Sub BindOutreachConfirm(ByVal strOutreachStatus As String)
        Dim strAction As String = ViewState("action")
        Select Case strAction
            Case ActionType.Create, ActionType.Update
                lblConfirmOutreachCode.Text = lblDetailOutreachCode.Text
                lblConfirmOutreachNameEng.Text = AntiXssEncoder.HtmlEncode(txtDetailOutreachNameEng.Text, True)
                lblConfirmOutreachNameChi.Text = AntiXssEncoder.HtmlEncode(txtDetailOutreachNameChi.Text, True)
                lblConfirmOutreachAddrEng.Text = AntiXssEncoder.HtmlEncode(txtDetailOutreachAddrEng.Text, True)
                lblConfirmOutreachAddrChi.Text = AntiXssEncoder.HtmlEncode(txtDetailOutreachAddrChi.Text, True)
                Status.GetDescriptionFromDBCode(OutreachListStatus.ClassCode, strOutreachStatus, lblConfirmOutreachStatus.Text, String.Empty)

            Case ActionType.Active, ActionType.Deactive
                lblConfirmOutreachCode.Text = lblDetailOutreachCode.Text
                lblConfirmOutreachNameEng.Text = lblDetailOutreachNameEng.Text
                lblConfirmOutreachNameChi.Text = lblDetailOutreachNameChi.Text
                lblConfirmOutreachAddrEng.Text = lblDetailOutreachAddrEng.Text
                lblConfirmOutreachAddrChi.Text = lblDetailOutreachAddrChi.Text
                Status.GetDescriptionFromDBCode(OutreachListStatus.ClassCode, strOutreachStatus, lblConfirmOutreachStatus.Text, String.Empty)
            Case Else
                ' Do nothing
        End Select

        hfConfirmOutreachStatus.Value = strOutreachStatus

        CompleteMsgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00021))
        CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information
        CompleteMsgBox.BuildMessageBox()

    End Sub

#End Region

#Region "Event"
    Protected Sub ibtnSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CompleteMsgBox.Visible = False
        msgBox.Visible = False
        imgOutreachCodeAlert.Visible = False

        udcCollapsibleSearchCriteriaReview.Collapsed = True
        udcCollapsibleSearchCriteriaReview.ClientState = "True"
        Dim enumSearchResult As SearchResultEnum

        ' Try-catch to avoid search over row limit (500)
        Try
            ' Write Audit Log
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.AddDescripton("Outreach Code", txtOutreachCode.Text.Trim)
            udtAuditLogEntry.AddDescripton("Outreach Name", txtOutreachName.Text.Trim)
            udtAuditLogEntry.AddDescripton("Outreach Addr", txtOutreachAddr.Text.Trim)
            udtAuditLogEntry.AddDescripton("Outreach Status", ddlOutreachStatus.SelectedValue)
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
                    Throw New Exception("Error: Class = [HCVU.OutreachListMaintenance], Method = [ibtnSearch_Click], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")

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

    Protected Sub ibtnSearchResultBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00008, AuditMsg00008)

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
        udtAuditLogEntry.AddDescripton("Outreach Code", txtOutreachCode.Text.Trim.ToUpper)
        udtAuditLogEntry.WriteLog(LogID.LOG00011, AuditMsg00011)

        msgBox.Visible = False
        CompleteMsgBox.Visible = False

        ViewState("state") = StateType.ADD

        Dim strOutreachCode As String = txtOutreachCode.Text.Trim.ToUpper

        If (OutreachCodeValidation(strOutreachCode)) Then
            StartCreateMode(strOutreachCode, False)

            ChangeViewIndex(ViewIndex.Detail)
        End If
    End Sub

    Protected Sub ibtnEdit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Outreach Code", lblDetailOutreachCode.Text.Trim)
        udtAuditLogEntry.WriteLog(LogID.LOG00012, AuditMsg00012)

        msgBox.Visible = False
        CompleteMsgBox.Visible = False

        ViewState("state") = StateType.EDIT
        StartEditMode(True, False)
    End Sub

    Protected Sub ibtnSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim strOutreachCode As String = String.Empty
        strOutreachCode = lblDetailOutreachCode.Text.Trim

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Outreach Code", strOutreachCode)
        udtAuditLogEntry.WriteLog(LogID.LOG00013, AuditMsg00013)

        If OutreachCodeValidation(strOutreachCode) AndAlso OutreachInfoValidation() Then
            If (ViewState("state") = StateType.ADD) Then
                ViewState("action") = ActionType.Create
                hfDetailOutreachStatus.Value = AntiXssEncoder.HtmlEncode(ddlDetailOutreachStatus.SelectedValue, True)

            Else
                ViewState("action") = ActionType.Update
            End If

            BindOutreachConfirm(AntiXssEncoder.HtmlEncode(hfDetailOutreachStatus.Value, True))
            ChangeViewIndex(ViewIndex.Confirm)
        End If
    End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Outreach Code", lblDetailOutreachCode.Text.Trim)
        udtAuditLogEntry.WriteLog(LogID.LOG00014, AuditMsg00014)

        Me.ModalPopupConfirmCancel.Show()
    End Sub

    Protected Sub ibtnActive_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Outreach Code", lblDetailOutreachCode.Text.Trim)
        udtAuditLogEntry.WriteLog(LogID.LOG00015, AuditMsg00015)


        Me.ModalPopupConfirmActivate.Show()
    End Sub

    Protected Sub ibtnDeactive_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Outreach Code", lblDetailOutreachCode.Text.Trim)
        udtAuditLogEntry.WriteLog(LogID.LOG00016, AuditMsg00016)

        Me.ModalPopupConfirmDeactivate.Show()
    End Sub

    Protected Sub ibtnRemove_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Outreach Code", lblDetailOutreachCode.Text.Trim)
        udtAuditLogEntry.WriteLog(LogID.LOG00030, AuditMsg00030)

        Dim udtEHSTransactionBLL As New EHSTransaction.EHSTransactionBLL

        If udtEHSTransactionBLL.CheckTransactionAdditionalFieldByAny(TransactionAdditionalFieldModel.AdditionalFieldType.OutreachCode, lblDetailOutreachCode.Text) Then
            msgBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00004))
            msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00041, AuditMsg00041)
        Else
            ViewState("action") = ActionType.Remove
            Me.ModalPopupConfirmRemove.PopupDragHandleControlID = Me.ucNoticePopUpConfirmRemove.Header.ClientID
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
            udtAuditLogEntry.AddDescripton("Outreach code", lblConfirmOutreachCode.Text.Trim)
            udtAuditLogEntry.AddDescripton("OutreachName_Eng", txtDetailOutreachNameEng.Text.Trim)
            udtAuditLogEntry.AddDescripton("OutreachName_Chi", txtDetailOutreachNameChi.Text.Trim)
            udtAuditLogEntry.AddDescripton("OutreachAddress_Eng", txtDetailOutreachAddrEng.Text.Trim)
            udtAuditLogEntry.AddDescripton("OutreachAddress_Chi", txtDetailOutreachAddrChi.Text.Trim)
            udtAuditLogEntry.AddDescripton("Record_Status", hfConfirmOutreachStatus.Value)

            udtAuditLogEntry.WriteStartLog(LogID.LOG00017, AuditMsg00017)

            Dim blnResult As New Boolean
            Dim udtOutreachList As New OutreachListModel
            Dim strUserID As String = udtHCVUUserBLL.GetHCVUUser.UserID.Trim

            If (ViewState("state") = StateType.EDIT) Then
                udtOutreachList = Session(SESS_OutreachListModal)
            End If

            udtOutreachList.OutreachCode = lblConfirmOutreachCode.Text.Trim
            udtOutreachList.OutreachNameEng = txtDetailOutreachNameEng.Text.Trim
            udtOutreachList.OutreachNameChi = txtDetailOutreachNameChi.Text.Trim
            udtOutreachList.AddressEng = txtDetailOutreachAddrEng.Text.Trim
            udtOutreachList.AddressChi = txtDetailOutreachAddrChi.Text.Trim
            udtOutreachList.Status = hfConfirmOutreachStatus.Value
            udtOutreachList.UpdateBy = strUserID
            udtOutreachList.Type = "C"

            udtAuditLogEntry.AddDescripton("Outreach code", lblConfirmOutreachCode.Text.Trim)
            If (ViewState("state") = StateType.ADD) Then
                blnResult = udtOutreachListBLL.AddOutreachList(udtOutreachList)
                If (blnResult) Then
                    CompleteMsgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00018, AuditMsg00018)
                End If
            Else
                blnResult = udtOutreachListBLL.UpdateOutreachList(udtOutreachList)
                If (blnResult) Then
                    If (ViewState("action") = ActionType.Update) Then
                        CompleteMsgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00002))
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00019, AuditMsg00019)
                    ElseIf (ViewState("action") = ActionType.Active) Then
                        CompleteMsgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003))
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00020, AuditMsg00020)
                    ElseIf (ViewState("action") = ActionType.Deactive) Then
                        CompleteMsgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00004))
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00021, AuditMsg00021)
                    End If
                Else
                    msgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
                End If
            End If

            If (blnResult) Then
                CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Complete
                CompleteMsgBox.BuildMessageBox()
                ChangeViewIndex(ViewIndex.MsgPage)
            Else
                WriteUpdateFailedAuditLog(lblConfirmOutreachCode.Text.Trim)
                ChangeViewIndex(ViewIndex.ErrorPage)
            End If
        Catch ex As Exception
            ErrorHandler.Log(FunctionCode, SeverityCode.SEVE, "99999", Request.PhysicalPath, Request.UserHostAddress, ex.Message)
            msgBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002))
            WriteUpdateFailedAuditLog(lblConfirmOutreachCode.Text.Trim)
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
            StartCreateMode(lblConfirmOutreachCode.Text, True)
        Else
            StartEditMode(Not blnUpdateStatus, True)
        End If

    End Sub


    Private Sub WriteUpdateFailedAuditLog(ByVal strOutreachCode As String)
        If msgBox.GetCodeTable.Rows.Count = 0 Then
            msgBox.Visible = False
        Else
            Dim strAction As String = ViewState("action")
            Dim udtExAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtExAuditLogEntry.AddDescripton("Outreach code", strOutreachCode)
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
#End Region

#Region "Grid Event"
    Private Sub gvResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Status
            Dim lblOutreachStatus As Label = CType(e.Row.FindControl("lblOutreachStatus"), Label)
            Status.GetDescriptionFromDBCode(OutreachListStatus.ClassCode, lblOutreachStatus.Text.Trim, lblOutreachStatus.Text, String.Empty)

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
            Dim strOutreachCode As String = CType(r.FindControl("lnkbtnOutreachCode"), LinkButton).CommandArgument.Trim

            BindOutreachSummaryView(strOutreachCode)
        End If
    End Sub

    Private Sub gvResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvResult.Sorting
        Me.GridViewSortingHandler(sender, e, SESS_SearchResultList)
    End Sub

#End Region

#Region "Validation"
    Private Function SearchValidation() As Boolean
        Dim blnValid As Boolean = True

        If (txtOutreachCode.Text.Trim = String.Empty AndAlso txtOutreachName.Text.Trim = String.Empty AndAlso txtOutreachAddr.Text.Trim = String.Empty _
            AndAlso ddlOutreachStatus.SelectedValue = String.Empty) Then
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


    Private Function OutreachCodeValidation(ByVal strOutreachCode As String) As Boolean
        msgBox.Visible = False
        imgOutreachCodeAlert.Visible = False

        Dim blnValid As Boolean = True

        ' Check empty of Outreach Code
        If strOutreachCode = String.Empty Then
            msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00003))
            blnValid = False
        Else
            ' Check duplication of Outreach Code
            If ViewState("state") = StateType.ADD Then
                Dim dt As DataTable = udtOutreachListBLL.GetOutreachListByCode(strOutreachCode)
                If dt.Rows.Count > 0 Then
                    msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001))
                    blnValid = False
                End If
            End If
        End If

        If msgBox.GetCodeTable.Rows.Count = 0 Then
            msgBox.Visible = False
        Else
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00027, AuditMsg00027)
            imgOutreachCodeAlert.Visible = True
        End If

        Return blnValid
    End Function

    Private Function OutreachInfoValidation() As Boolean
        Dim blnValid As Boolean = True
        imgOutreachNameEngAlert.Visible = False
        imgOutreachNameChiAlert.Visible = False
        imgOutreachAddrEngAlert.Visible = False
        imgOutreachAddrChiAlert.Visible = False
        imgOutreachStatusAlert.Visible = False

        ' Check Mandatory Fields        
        If txtDetailOutreachNameEng.Text.Trim = String.Empty Then
            msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00028), "%s", GetGlobalResourceObject("Text", "OutreachName") & " (" & GetGlobalResourceObject("text", "InEnglish") & ")")
            imgOutreachNameEngAlert.Visible = True
            blnValid = False
        End If
        If txtDetailOutreachNameChi.Text.Trim = String.Empty Then
            msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00028), "%s", GetGlobalResourceObject("Text", "OutreachName") & " (" & GetGlobalResourceObject("text", "InChinese") & ")")
            imgOutreachNameChiAlert.Visible = True
            blnValid = False
        End If
        If txtDetailOutreachAddrEng.Text.Trim = String.Empty Then
            msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00028), "%s", GetGlobalResourceObject("Text", "OutreachAddr") & " (" & GetGlobalResourceObject("text", "InEnglish") & ")")
            imgOutreachAddrEngAlert.Visible = True
            blnValid = False
        End If
        If txtDetailOutreachAddrChi.Text.Trim = String.Empty Then
            msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00028), "%s", GetGlobalResourceObject("Text", "OutreachAddr") & " (" & GetGlobalResourceObject("text", "InChinese") & ")")
            imgOutreachAddrChiAlert.Visible = True
            blnValid = False
        End If
        If ViewState("state") = StateType.ADD AndAlso ddlDetailOutreachStatus.SelectedValue = String.Empty Then
            msgBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00028), "%s", GetGlobalResourceObject("Text", "OutreachStatus"))
            imgOutreachStatusAlert.Visible = True
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

                imgOutreachNameEngAlert.Visible = False
                imgOutreachNameChiAlert.Visible = False
                imgOutreachAddrEngAlert.Visible = False
                imgOutreachAddrChiAlert.Visible = False
                imgOutreachStatusAlert.Visible = False

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
        ViewState("action") = ActionType.Active

        Select Case e
            Case ucNoticePopUp.enumButtonClick.OK
                Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
                udtAuditLogEntry.WriteLog(LogID.LOG00033, AuditMsg00033)

                Try
                    Dim blnResult As New Boolean
                    Dim udtOutreachList As New OutreachListModel

                    udtOutreachList = Session(SESS_OutreachListModal)
                    udtOutreachList.Status = OutreachListStatus.Active
                    udtOutreachList.UpdateBy = udtHCVUUserBLL.GetHCVUUser.UserID.Trim
                    udtAuditLogEntry.AddDescripton("Outreach code", lblConfirmOutreachCode.Text.Trim)

                    blnResult = udtOutreachListBLL.UpdateOutreachList(udtOutreachList)
                    If (blnResult) Then
                        CompleteMsgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003))
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00020, AuditMsg00020)
                        CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Complete
                        CompleteMsgBox.BuildMessageBox()
                        ChangeViewIndex(ViewIndex.MsgPage)
                    Else
                        msgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
                        WriteUpdateFailedAuditLog(lblConfirmOutreachCode.Text.Trim)
                        ChangeViewIndex(ViewIndex.ErrorPage)
                    End If


                Catch ex As Exception
                    ErrorHandler.Log(FunctionCode, SeverityCode.SEVE, "99999", Request.PhysicalPath, Request.UserHostAddress, ex.Message)
                    msgBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002))
                    WriteUpdateFailedAuditLog(lblConfirmOutreachCode.Text.Trim)
                End Try

            Case Else
                Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
                udtAuditLogEntry.WriteLog(LogID.LOG00034, AuditMsg00034)

        End Select

    End Sub

    Private Sub ucNoticePopUpConfirmDeactivate_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUpConfirmDeactivate.ButtonClick
        msgBox.Visible = False
        CompleteMsgBox.Visible = False
        ViewState("action") = ActionType.Deactive

        Select Case e
            Case ucNoticePopUp.enumButtonClick.OK
                Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
                udtAuditLogEntry.WriteLog(LogID.LOG00035, AuditMsg00035)

                Try
                    Dim blnResult As New Boolean
                    Dim udtOutreachList As New OutreachListModel

                    udtOutreachList = Session(SESS_OutreachListModal)
                    udtOutreachList.Status = OutreachListStatus.Inactive
                    udtOutreachList.UpdateBy = udtHCVUUserBLL.GetHCVUUser.UserID.Trim
                    udtAuditLogEntry.AddDescripton("Outreach code", lblConfirmOutreachCode.Text.Trim)

                    blnResult = udtOutreachListBLL.UpdateOutreachList(udtOutreachList)
                    If (blnResult) Then
                        CompleteMsgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00004))
                        CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Complete
                        CompleteMsgBox.BuildMessageBox()
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00021, AuditMsg00021)
                        ChangeViewIndex(ViewIndex.MsgPage)
                    Else
                        msgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
                        WriteUpdateFailedAuditLog(lblConfirmOutreachCode.Text.Trim)
                        ChangeViewIndex(ViewIndex.ErrorPage)
                    End If

                Catch ex As Exception
                    ErrorHandler.Log(FunctionCode, SeverityCode.SEVE, "99999", Request.PhysicalPath, Request.UserHostAddress, ex.Message)
                    msgBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002))
                    WriteUpdateFailedAuditLog(lblConfirmOutreachCode.Text.Trim)
                End Try

            Case Else
                Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
                udtAuditLogEntry.WriteLog(LogID.LOG00036, AuditMsg00036)
        End Select

    End Sub

    Private Sub ucNoticePopUpConfirmRemove_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUpConfirmRemove.ButtonClick
        msgBox.Visible = False
        CompleteMsgBox.Visible = False
        ViewState("action") = ActionType.Remove

        Select Case e
            Case ucNoticePopUp.enumButtonClick.OK
                Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
                udtAuditLogEntry.WriteLog(LogID.LOG00031, AuditMsg00031)

                Try
                    Dim blnResult As New Boolean
                    Dim udtOutreachList As New OutreachListModel

                    udtOutreachList = Session(SESS_OutreachListModal)

                    udtAuditLogEntry.AddDescripton("Outreach code", lblConfirmOutreachCode.Text.Trim)

                    blnResult = udtOutreachListBLL.DeleteOutreachList(udtOutreachList)
                    If (blnResult) Then
                        CompleteMsgBox.AddMessage(New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00005))
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00040, AuditMsg00040)
                        CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Complete
                        CompleteMsgBox.BuildMessageBox()
                        ChangeViewIndex(ViewIndex.MsgPage)

                    Else
                        msgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
                        WriteUpdateFailedAuditLog(lblConfirmOutreachCode.Text.Trim)
                        ChangeViewIndex(ViewIndex.ErrorPage)
                    End If

                Catch ex As Exception
                    ErrorHandler.Log(FunctionCode, SeverityCode.SEVE, "99999", Request.PhysicalPath, Request.UserHostAddress, ex.Message)
                    msgBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002))
                    WriteUpdateFailedAuditLog(lblConfirmOutreachCode.Text.Trim)
                End Try

            Case Else
                Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
                udtAuditLogEntry.WriteLog(LogID.LOG00032, AuditMsg00032)
                ' Do nothing
        End Select

    End Sub

#End Region

End Class