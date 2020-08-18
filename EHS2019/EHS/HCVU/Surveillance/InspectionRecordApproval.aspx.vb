Imports System.Threading
Imports System.Globalization
Imports System.Xml
Imports Common.ComObject
Imports Common.Component.EHSAccount
Imports Common.Component
Imports Common.Component.Scheme
Imports Common.ComFunction
Imports Common.Component.EHSTransaction
Imports Common.DataAccess
Imports System.Data.SqlClient
Imports Common.Component.StaticData
Imports Common.Component.HCVUUser
Imports Common.Validation
Imports CustomControls
Imports System.IO

Partial Class InspectionRecordApproval
    Inherits BasePageWithControl

#Region "Enum"
    Public Enum EnumSqlErrorMessage
        Normal = 0
        OverResultList1stLimit = 9
        OverResultListOverrideLimit = 17
        Unhandled = 99999
    End Enum
#End Region

#Region "Property"
    Dim udtAuditLogEntry As AuditLogEntry
    Dim udtSM As Common.ComObject.SystemMessage
    Dim udtValidator As New Validator
    Dim udtformatter As New Common.Format.Formatter
    Dim udtHCVUUserBLL As New HCVUUserBLL
    Dim udtInspectionRecordBLL As New InspectionRecordBLL
    Private udtSessionHandlerBLL As New BLL.SessionHandlerBLL
#End Region

#Region "Audit Log Description"
    Public Class AuditLogDesc
        Public Const Load As String = "Inspection Record Approval - Load" '00

        Public Const IRM_SearchResult_Fail As String = "Inspection Record Approval - Search Inspection Record Fail" '01

        Public Const IRM_ViewInspectionDetail_click As String = "Inspection Record Approval View Inspection Detail click" '02
        Public Const IRM_ViewInspectionDetail_Successful As String = "Inspection Record Approval View Inspection Detail Successful" '03
        Public Const IRM_ViewInspectionDetail_Fail As String = "Inspection Record Approval View Inspection Detail Fail" '04

        Public Const IRM_Search_click As String = "Inspection Record Approval Search click" '05
        Public Const IRM_Search_Successful As String = "Inspection Record Approval Search Successful" '06
        Public Const IRM_Search_Fail As String = "Inspection Record Approval Search Fail" '07

        Public Const IRM_Reject_click As String = "Inspection Record Approval Reject click" '08
        Public Const IRM_Reject_Successful As String = "Inspection Record Approval Reject Successful" '09
        Public Const IRM_Reject_Fail As String = "Inspection Record Approval Reject Fail" '10

        Public Const IRM_Approve_click As String = "Inspection Record Approval Approve click" '11
        Public Const IRM_Approve_Successful As String = "Inspection Record Approval Approve Successful" '12
        Public Const IRM_Approve_Fail As String = "Inspection Record Approval Approve Fail" '13

        Public Const IRM_ConfirmationPopupConfirm_Click As String = "Inspection Record Approval Confirmation Popup Confirm Button Click" '14
        Public Const IRM_ConfirmationPopupCancel_Click As String = "Inspection Record Approval Confirmation Popup Cancel Button Click" '15
        Public Const IRM_Reject_Start As String = "Inspection Record Approval Reject Start" '16

        Public Const IRM_Search_Action_Change As String = "Inspection Record Approval Search Action changed" '17
        Public Const IRM_Search_Subject_Officer_Change As String = "Inspection Record Approval Search Subject Officer changed" '18
        Public Const IRM_DetailBack_Click As String = "Inspection Record Approval Detail Page Back Click" '19

        Public Const IRM_Approve_Start As String = "Inspection Record Approval Approve Start" '20
        Public Const IRM_CompletePageBack_Button_Click As String = "Inspection Record Approval Complete page - Back Button Click" '21

    End Class

#End Region

#Region "Constant Value"

    Private Const SESS_SearchApprovalResultDataTable As String = "eHSAccount_InspectionRecord_Approval_SearchResultDataTable"
    Private Const SESS_SearchApprovalResultDataTableForFilter As String = "eHSAccount_InspectionRecord_Approval_SearchResultDataTable_ForFilter"
    Private Const SESS_InspectionRecordModel As String = "eHSAccount_InspectionRecord_Approval_Inspection_Record_Model"

    Private Const FuncCode As String = FunctCode.FUNT011102
    Private Const CommonFunctionCode As String = FunctCode.FUNT990000

    Public Class ActionType
        Public Const Close As String = "C"
        Public Const Remove As String = "D"
        Public Const Reopen As String = "O"
    End Class
#End Region

#Region "Page function"

    Private Overloads Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        ' Get HCVU User to check session expire
        Dim udtHCVUUserBLL As New HCVUUserBLL
        udtHCVUUserBLL.GetHCVUUser()

        FunctionCode = FuncCode ' FunctCode.FUNT010302

        If Not IsPostBack Then
            SetSearchAction()
            SetSubjectOfficer(Nothing)
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, AuditLogDesc.Load)
        End If

        Me.ModalPopupConfirmApprove.PopupDragHandleControlID = Me.ucNoticePopUpConfirmApprove.Header.ClientID
        Me.ModalPopupConfirmReject.PopupDragHandleControlID = Me.ucNoticePopUpConfirmReject.Header.ClientID

    End Sub
#Region "       1. Search Page & Result"
    'Set Criteria
    Private Sub SetSearchAction()

        Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL
        Dim strRole As String = String.Empty
        Dim udtHCVUUserBLL As New HCVUUserBLL
        Dim user As HCVUUserModel = udtHCVUUserBLL.GetHCVUUser()

        Dim inspectionRole As InspectionRole = udtInspectionRecordBLL.GetInspectionRole(user)

        If inspectionRole.IsSEO Then
            strRole = "SEOApprovalAction"
        ElseIf inspectionRole.IsEndorser Then
            strRole = "EndorserApprovalAction"
        End If

        Me.ddlAction.Items.Clear()
        If strRole <> String.Empty Then
            Me.ddlAction.DataSource = udtStaticDataBLL.GetStaticDataListByColumnName(strRole)
            Me.ddlAction.DataTextField = "DataValue"
            Me.ddlAction.DataValueField = "ItemNo"
            Me.ddlAction.DataBind()
            Me.ddlAction.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), ""))
            Me.ddlAction.SelectedIndex = 0
        End If

    End Sub

    Private Sub SetSubjectOfficer(ByVal dt As DataTable, Optional selectedValue As String = "")
        Me.ddlSubjectOfficer.Items.Clear()
        Me.ddlSubjectOfficer.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))
        Me.ddlSubjectOfficer.SelectedIndex = 0
        trSubjectOfficer.Visible = False
        If Not IsNothing(dt) Then
            If dt.Rows.Count > 0 Then
                trSubjectOfficer.Visible = True
                Me.ddlSubjectOfficer.DataSource = dt
                Me.ddlSubjectOfficer.DataTextField = "Subject_Officer_Value"
                Me.ddlSubjectOfficer.DataValueField = "Subject_Officer_ID"
                Me.ddlSubjectOfficer.DataBind()
                Me.ddlSubjectOfficer.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))
                Me.ddlSubjectOfficer.SelectedIndex = 0

                For Each item As ListItem In Me.ddlSubjectOfficer.Items
                    If (item.Value = selectedValue) Then
                        Me.ddlSubjectOfficer.SelectedValue = selectedValue
                        Exit For
                    End If
                Next
            End If
        End If
    End Sub
    'Search Function
    Protected Sub ibtnSearch_Click(sender As Object, e As ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00005, AuditLogDesc.IRM_Search_click)

        Me.udcInfoMsgBox.Visible = False
        Me.udcMsgBox.Visible = False
        Dim enumSearchResult As SearchResultEnum 'Result Status

        Try
            If sender Is Nothing Then
                enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, udcMsgBox, udcInfoMsgBox, True, True)
            Else
                enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, udcMsgBox, udcInfoMsgBox)
            End If
            Select Case enumSearchResult
                Case SearchResultEnum.Success '0
                    'If MultiViewIRM.ActiveViewIndex = 0 Then
                    'MultiViewIRM.ActiveViewIndex = RESULT
                    MultiViewIRM.SetActiveView(vSEARCH)
                    'End If
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00006, AuditLogDesc.IRM_Search_Successful)
                Case SearchResultEnum.ValidationFail '1
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00007, AuditLogDesc.IRM_Search_Fail)
                Case SearchResultEnum.NoRecordFound '2
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00007, AuditLogDesc.IRM_Search_Fail)
                Case SearchResultEnum.OverResultList1stLimit_PopUp
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00007, AuditLogDesc.IRM_Search_Fail)
                Case SearchResultEnum.OverResultList1stLimit_Alert
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00007, AuditLogDesc.IRM_Search_Fail)
                Case SearchResultEnum.OverResultListOverrideLimit
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00007, AuditLogDesc.IRM_Search_Fail)
                Case Else
                    Throw New Exception("Error: Class = [HCVU.InspectionRecordApproval], Method = [ibtnSearch_Click], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")

            End Select
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00007, AuditLogDesc.IRM_Search_Fail)
            Throw ex
        End Try
    End Sub
    'Result Table Function
    Protected Sub gvApprovalResult_Prerender(sender As Object, e As EventArgs) Handles gvApprovalResult.PreRender
        GridViewPreRenderHandler(sender, e, SESS_SearchApprovalResultDataTable)
    End Sub
    Protected Sub gvApprovalResult_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvApprovalResult.PageIndexChanging
        GridViewPageIndexChangingHandler(sender, e, SESS_SearchApprovalResultDataTable)
    End Sub
    Protected Sub gvApprovalResult_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvApprovalResult.Sorting
        GridViewSortingHandler(sender, e, SESS_SearchApprovalResultDataTable)
    End Sub

    Private Sub gvApprovalResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvApprovalResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRowView = e.Row.DataItem

            ' File Ref No.
            Dim lblRFileRefNo As Label = e.Row.FindControl("lblRFileRefNo")
            lblRFileRefNo.Text = String.Format("<div style='width:110px;overflow-wrap:break-word;word-break:break-all'>{0}</div>", CStr(dr("File_Reference_No")).Trim)

            ' SP Name
            Dim lblRCname As Label = CType(e.Row.FindControl("lblRCname"), Label)
            lblRCname.Text = udtformatter.formatChineseName(lblRCname.Text.Trim)

            ' Subject Officer
            Dim lblRSubjectOfficer As Label = CType(e.Row.FindControl("lblRSubjectOfficer"), Label)
            If lblRSubjectOfficer.Text.Trim = String.Empty Then
                lblRSubjectOfficer.Text = Me.GetGlobalResourceObject("Text", "NA")
            End If

            ' Case Officer
            Dim lblRCaseOfficer As Label = CType(e.Row.FindControl("lblRCaseOfficer"), Label)
            If lblRCaseOfficer.Text.Trim = String.Empty Then
                lblRCaseOfficer.Text = Me.GetGlobalResourceObject("Text", "NA")
            End If
        End If
    End Sub

    'Result Click -> 2. Inspection Record - Detail Show
    Protected Sub ResultLbtn_Click(sender As Object, e As EventArgs)
        Dim btn As LinkButton = CType(sender, LinkButton)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00002, AuditLogDesc.IRM_ViewInspectionDetail_click)
        GetDetail(btn.Text)
        udtAuditLogEntry.AddDescripton("Inspection ID", btn.Text)
        udtAuditLogEntry.WriteEndLog(LogID.LOG00002, AuditLogDesc.IRM_ViewInspectionDetail_Successful)
    End Sub
    'Selected Value Change Function
    Protected Sub ddlAction_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddlAction As DropDownList = sender
        SetSubjectOfficer(Nothing)

        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Action", ddlAction.SelectedItem.Text)
        udtAuditLogEntry.AddDescripton("SubjectOfficer", Me.ddlSubjectOfficer.SelectedItem.Text)
        udtAuditLogEntry.WriteLog(LogID.LOG00017, AuditLogDesc.IRM_Search_Action_Change)

        ibtnSearch_Click(Nothing, Nothing)

    End Sub
    Protected Sub ddlSubjectOfficer_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddlSubjectOfficer As DropDownList = sender

        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Action", Me.ddlAction.SelectedItem.Text)
        udtAuditLogEntry.AddDescripton("SubjectOfficer", ddlSubjectOfficer.SelectedItem.Text)

        udtAuditLogEntry.WriteStartLog(LogID.LOG00018, AuditLogDesc.IRM_Search_Subject_Officer_Change)

        SubjectOfficer_SeletedIndexChanged()

    End Sub
    Private Sub SubjectOfficer_SeletedIndexChanged()
        If Not String.IsNullOrEmpty(ddlAction.SelectedValue.Trim) Then
            Dim dt As DataTable = CType(Session(SESS_SearchApprovalResultDataTableForFilter), DataTable).Copy
            If Not String.IsNullOrEmpty(ddlSubjectOfficer.SelectedValue.Trim) Then
                Dim dv As DataView = dt.DefaultView
                Dim selectedValue As String = ddlSubjectOfficer.SelectedValue.Trim
                If selectedValue = "NA" Then
                    dv.RowFilter = "Subject_Officer_Value = '" + "" + "'"
                Else
                    dv.RowFilter = "Subject_Officer_ID = '" + selectedValue + "'"
                End If
                dt = dv.ToTable()
            End If
            'After Filtering
            Session(SESS_SearchApprovalResultDataTable) = dt

            Me.GridViewDataBind(Me.gvApprovalResult, dt, "Visit_Date", "DESC", False)
        End If
    End Sub
#End Region
#Region "       2. Inspection Record - Detail Show"
    'Get Detail
    Protected Sub GetDetail(id As String, Optional isRedirected As Boolean = False)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode)

        MultiViewIRM.SetActiveView(vViewInspectionDetail)
        Dim inspectionRecord As InspectionRecordModel = udtInspectionRecordBLL.GetInspectionRecord(id)
        Session(SESS_InspectionRecordModel) = inspectionRecord
        If (Not IsNothing(inspectionRecord)) Then
            FillDataVisitDetailByModel(inspectionRecord)
            Dim recordStatus As String = inspectionRecord.RecordStatus
            Dim originalStatus As String = inspectionRecord.OriginalStatus
            'Add by golden 644
            hdnStatus.Value = recordStatus

            recordStatus = IIf(recordStatus = InspectionStatus.Removed Or recordStatus = InspectionStatus.RemovePendingApproval, originalStatus, recordStatus)
            If recordStatus <> InspectionStatus.Incomplete And recordStatus <> InspectionStatus.PendingForSiteVisit Then
                divInspectionResultDetail.Visible = True
                FillDataInspectionResult(inspectionRecord)
                headVisitDetail.Visible = True
                trFreezeDate.Visible = True
            Else
                divInspectionResultDetail.Visible = False
                headVisitDetail.Visible = True
                trFreezeDate.Visible = False
            End If
            'Determine button visible .
            DetermineButtonVisible(inspectionRecord)
        Else

        End If
    End Sub
    Private Sub FillDataVisitDetailByModel(model As InspectionRecordModel)
        lblDetInspectionID.Text = model.InspectionID
        lblDetFileNo.Text = model.FileReferenceNo

        If Not String.IsNullOrEmpty(model.ReferredReferenceNo1) Or Not String.IsNullOrEmpty(model.ReferredReferenceNo2) Or Not String.IsNullOrEmpty(model.ReferredReferenceNo3) Then
            Me.spanRefNo.Visible = True
            lbDetReferredReferenceNo1.Text = model.ReferredReferenceNo1
            lbDetReferredReferenceNo2.Text = model.ReferredReferenceNo2
            lbDetReferredReferenceNo3.Text = model.ReferredReferenceNo3
            'hfDetReferredInspectionID1.Value = model.ReferredInspectionID1
            'hfDetReferredInspectionID2.Value = model.ReferredInspectionID2
            'hfDetReferredInspectionID3.Value = model.ReferredInspectionID3
            'lbDetReferredReferenceNo1.Enabled = Not String.IsNullOrEmpty(model.ReferredInspectionID1)
            'lbDetReferredReferenceNo2.Enabled = Not String.IsNullOrEmpty(model.ReferredInspectionID2)
            'lbDetReferredReferenceNo3.Enabled = Not String.IsNullOrEmpty(model.ReferredInspectionID3)
        Else
            Me.spanRefNo.Visible = False
            lbDetReferredReferenceNo1.Text = ""
            lbDetReferredReferenceNo2.Text = ""
            lbDetReferredReferenceNo3.Text = ""
            'hfDetReferredInspectionID1.Value = ""
            'hfDetReferredInspectionID2.Value = ""
            'hfDetReferredInspectionID3.Value = ""
        End If

        lblDetSPID.Text = model.SPID

        If Not String.IsNullOrEmpty(model.SPStatus) Then
            lblDetSPStatus.Text = joinParenthesis(model.SPStatus)
        Else
            lblDetSPStatus.Text = ""
        End If

        lblDetSPName.Text = String.Format("{0} {1}", model.SPEngName, udtformatter.formatChineseName(model.SPChiName))

        lblDetHealthProfession.Text = model.ServiceCategoryDesc + joinParenthesis(model.PracticeRegCode)

        Dim isEmptySPContactNo As Boolean = String.IsNullOrEmpty(model.SPTelNo),
            isEmptySPFaxNo As Boolean = String.IsNullOrEmpty(model.SPFaxNo),
            isEmptySPEmail As Boolean = String.IsNullOrEmpty(model.SPTelNo)

        lblDetSPTelNo.Text = model.SPTelNo
        lblDetSPFaxNo.Text = model.SPFaxNo
        lblDetSPEmail.Text = model.SPEmail
        trDetSPContactInfo.Visible = Not (isEmptySPContactNo And isEmptySPFaxNo And isEmptySPEmail)
        pnlDetSPTelNo.Visible = Not isEmptySPContactNo
        pnlDetSPFaxNo.Visible = Not isEmptySPFaxNo
        pnlDetSPEmail.Visible = Not isEmptySPEmail

        If (model.SPHCVSEffectiveDtm = Date.MinValue) Then
            trDetHCVSEffectiveDate.Visible = False
            lblDetHCVSEffectiveDate.Text = ""
        Else
            trDetHCVSEffectiveDate.Visible = True
            lblDetHCVSEffectiveDate.Text = udtInspectionRecordBLL.FormatOutputDate(model.SPHCVSEffectiveDtm)
            If model.SPHCVSDelistDtm <> Date.MinValue Then
                lblDetHCVSEffectiveDate.Text += " <font color=""red"">" + joinParenthesis(Me.GetGlobalResourceObject("Text", "DelistedOn") + " " + udtInspectionRecordBLL.FormatOutputDate(model.SPHCVSDelistDtm)) + "</font>"
            End If
        End If

        If (model.SPHCVSDHCEffectiveDtm = Date.MinValue) Then
            trDetHCVSDHCEffectiveDate.Visible = False
            lblDetHCVSDHCEffectiveDate.Text = ""
        Else
            trDetHCVSDHCEffectiveDate.Visible = True
            lblDetHCVSDHCEffectiveDate.Text = udtInspectionRecordBLL.FormatOutputDate(model.SPHCVSDHCEffectiveDtm)
            If model.SPHCVSDHCDelistDtm <> Date.MinValue Then
                lblDetHCVSDHCEffectiveDate.Text += " <font color=""red"">" + joinParenthesis(Me.GetGlobalResourceObject("Text", "DelistedOn") + " " + udtInspectionRecordBLL.FormatOutputDate(model.SPHCVSDHCDelistDtm)) + "</font>"
            End If
        End If

        If (model.SPHCVSCHNEffectiveDtm = Date.MinValue) Then
            trDetHCVSCHNEffectiveDate.Visible = False
            lblDetHCVSCHNEffectiveDate.Text = ""
        Else
            trDetHCVSCHNEffectiveDate.Visible = True
            lblDetHCVSCHNEffectiveDate.Text = udtInspectionRecordBLL.FormatOutputDate(model.SPHCVSCHNEffectiveDtm)
            If model.SPHCVSCHNDelistDtm <> Date.MinValue Then
                lblDetHCVSCHNEffectiveDate.Text += " <font color=""red"">" + joinParenthesis(Me.GetGlobalResourceObject("Text", "DelistedOn") + " " + udtInspectionRecordBLL.FormatOutputDate(model.SPHCVSCHNDelistDtm)) + "</font>"
            End If
        End If

        lblDetPractice.Text = model.PracticeName
        If Not String.IsNullOrEmpty(model.PracticeStatus) Then
            lblDetPracticeStatus.Text = joinParenthesis(model.PracticeStatus)
        Else
            lblDetPracticeStatus.Text = ""
        End If

        hdfIIRPracticeSeq.Value = model.PracticeDisplaySeq
        lblDetPractice_Ci.Text = udtformatter.formatChineseName(model.PracticeNameChi)
        lblDetPracticeAddress.Text = model.PracticeAddress
        lblDetPracticeAddress_Ci.Text = udtformatter.formatChineseName(model.PracticeAddressChi)
        lblDetPracticePhoneDaytime.Text = model.PracticePhoneNo
        If (model.SPLastVisitDate <> Date.MinValue) Then
            lblDetLastVisitDate.Text = udtInspectionRecordBLL.FormatOutputDate(model.SPLastVisitDate) + joinParenthesis(Me.GetGlobalResourceObject("Text", "FileReferenceNo") + ": " + model.SPLastVisitFileRefNo)
        Else
            lblDetLastVisitDate.Text = Me.GetGlobalResourceObject("Text", "NA")
        End If

        'Golden Add for freeze date
        lblFreezeDate.Text = Me.GetGlobalResourceObject("Text", "InformationAsAtDate").ToString().Replace("%d", udtInspectionRecordBLL.FormatOutputDate(model.FreezeDate))
        lblDetMainTypeofInspection.Text = model.MainTypeOfInspectionValue
        lblDetTypeofInspection.Text = model.OtherTypeOfInspectionValue
        SetLabelcolor(lblDetTypeofInspection, True)
        lblDetVisitDate.Text = udtInspectionRecordBLL.FormatOutputDate(model.VisitDate)
        SetLabelcolor(lblDetVisitDate)

        lblDetStartVisitTime.Text = udtInspectionRecordBLL.GetTimeFromDate(model.VisitBeginDtm)
        lblDetEndVisitTime.Text = udtInspectionRecordBLL.GetTimeFromDate(model.VisitEndDtm)
        If model.VisitBeginDtm = DateTime.MinValue And model.VisitEndDtm = DateTime.MinValue Then
            lblDetTo.Text = Me.GetGlobalResourceObject("Text", "ToBeProvided")
            lblDetTo.Style.Add("color", "red")
        Else
            lblDetTo.Text = "-"
            lblDetTo.Style.Remove("color")
        End If

        lblDetConfirmationWith.Text = model.ConfirmationWith
        SetLabelcolor(lblDetConfirmationWith)
        lblDetConfirmDate.Text = udtInspectionRecordBLL.FormatOutputDate(model.ConfirmationDtm)
        SetLabelcolor(lblDetConfirmDate)
        lblDetFormCondition.Text = model.FormConditionValue
        SetLabelcolor(lblDetFormCondition)
        lblDetFormConditionRm.Text = model.FormConditionRemark
        lblDetMeansofCommunication.Text = model.MeansOfCommunicationValue

        If Not String.IsNullOrEmpty(model.MeansOfCommunicationFax) Or Not String.IsNullOrEmpty(model.MeansOfCommunicationEmail) Then
            lblDetMeansofCommunicationContact.Text = joinParenthesis(Me.GetGlobalResourceObject("Text", "FaxNo") + ": " + IIf(Not String.IsNullOrEmpty(model.MeansOfCommunicationFax), model.MeansOfCommunicationFax, Me.GetGlobalResourceObject("Text", "Empty")) + " / " +
                                                             Me.GetGlobalResourceObject("Text", "Email") + ": " + IIf(Not String.IsNullOrEmpty(model.MeansOfCommunicationEmail), model.MeansOfCommunicationEmail, Me.GetGlobalResourceObject("Text", "Empty")))
        Else
            lblDetMeansofCommunicationContact.Text = ""
        End If
        SetLabelcolor(lblDetMeansofCommunication)
        lblDetLowRiskClaim.Text = IIf(String.IsNullOrEmpty(model.LowRiskClaim), "", IIf(model.LowRiskClaim = "Y", "Yes", "No"))
        SetLabelcolor(lblDetLowRiskClaim)
        lblDetRemarks.Text = model.Remarks
        SetLabelcolor(lblDetRemarks, True)
        lblDetCaseOfficer.Text = model.CaseOfficerValue
        SetLabelcolor(lblDetCaseOfficer)
        lblDetCaseContactNo.Text = model.CaseOfficerContactNo
        SetLabelcolor(lblDetCaseContactNo)
        lblDetSubjectOfficer.Text = model.SubjectOfficerValue
        SetLabelcolor(lblDetSubjectOfficer)
        lblDetSubjectContactNo.Text = model.SubjectOfficerContactNo
        SetLabelcolor(lblDetSubjectContactNo)
        lblDetStatus.Text = model.RecordStatusValue

        lblDetCreatedBy.Text = String.Format("{0} ({1})", model.CreateBy, model.CreateDtm.ToString("dd MMM yyyy HH:mm"))
        lblDetUpdatedBy.Text = String.Format("{0} ({1})", model.UpdateBy, model.UpdateDtm.ToString("dd MMM yyyy HH:mm"))

        trRemoveRequest.Visible = False
        trRemoveApprove.Visible = False
        trCloseRequest.Visible = False
        trCloseApprove.Visible = False
        trReopenRequest.Visible = False
        trReopenApprove.Visible = False
        trReopenReason.Visible = False

        Select Case model.RecordStatus
            Case InspectionStatus.Incomplete, InspectionStatus.PendingForSiteVisit
                ' Do Nothing
            Case InspectionStatus.RemovePendingApproval
                trRemoveRequest.Visible = True
                lblDetRemoveRequestBy.Text = String.Format("{0} ({1})", model.RemoveRequestBy, model.RemoveRequestDtm.ToString("dd MMM yyyy HH:mm"))
            Case InspectionStatus.Removed
                trRemoveRequest.Visible = True
                lblDetRemoveRequestBy.Text = String.Format("{0} ({1})", model.RemoveRequestBy, model.RemoveRequestDtm.ToString("dd MMM yyyy HH:mm"))
                trRemoveApprove.Visible = True
                lblDetRemoveApproveBy.Text = String.Format("{0} ({1})", model.RemoveApproveBy, model.RemoveApproveDtm.ToString("dd MMM yyyy HH:mm"))

            Case InspectionStatus.ClosePendingApproval
                trCloseRequest.Visible = True
                lblDetCloseRequestBy.Text = String.Format("{0} ({1})", model.CloseRequestBy, model.CloseRequestDtm.ToString("dd MMM yyyy HH:mm"))
            Case InspectionStatus.Closed
                trCloseRequest.Visible = True
                lblDetCloseRequestBy.Text = String.Format("{0} ({1})", model.CloseRequestBy, model.CloseRequestDtm.ToString("dd MMM yyyy HH:mm"))
                trCloseApprove.Visible = True
                lblDetCloseApproveBy.Text = String.Format("{0} ({1})", model.CloseApproveBy, model.CloseApproveDtm.ToString("dd MMM yyyy HH:mm"))

            Case InspectionStatus.ReopenPendingApproval
                trCloseRequest.Visible = True
                lblDetCloseRequestBy.Text = String.Format("{0} ({1})", model.CloseRequestBy, model.CloseRequestDtm.ToString("dd MMM yyyy HH:mm"))
                trCloseApprove.Visible = True
                lblDetCloseApproveBy.Text = String.Format("{0} ({1})", model.CloseApproveBy, model.CloseApproveDtm.ToString("dd MMM yyyy HH:mm"))
                trReopenRequest.Visible = True
                lblDetReopenRequestBy.Text = String.Format("{0} ({1})", model.ReopenRequestBy, model.ReopenRequestDtm.ToString("dd MMM yyyy HH:mm"))
                trReopenReason.Visible = True
                lblDetReopenRequestReason.Text = model.ReopenRequestReason

            Case InspectionStatus.InspectionResultInputted
                ' Check Reopen
                If Not String.IsNullOrEmpty(model.ReopenRequestBy) Then
                    trReopenRequest.Visible = True
                    lblDetReopenRequestBy.Text = String.Format("{0} ({1})", model.ReopenRequestBy, model.ReopenRequestDtm.ToString("dd MMM yyyy HH:mm"))
                    trReopenApprove.Visible = True
                    lblDetReopenApproveBy.Text = String.Format("{0} ({1})", model.ReopenApproveBy, model.ReopenApproveDtm.ToString("dd MMM yyyy HH:mm"))
                    trReopenReason.Visible = True
                    lblDetReopenRequestReason.Text = model.ReopenRequestReason
                End If
        End Select

    End Sub
    Private Sub FillDataInspectionResult(model As InspectionRecordModel)
        lblDetInOrder.Text = model.NoOfInOrder
        lblDetMissingForm.Text = model.NoOfMissingForm
        lblDetInconsistent.Text = model.NoOfInconsistent
        lblDetTotalCheck.Text = model.NoOfTotalCheck
        lblDetCheckingDate.Text = udtInspectionRecordBLL.FormatOutputDate(model.CheckingDate)

        lblDetAnomalous.Text = IIf(model.AnomalousClaims = "Y", "Yes", "No") + IIf(model.AnomalousClaims = "Y", joinParenthesis(Me.GetGlobalResourceObject("Text", "NoOfRecord") + " " + model.NoOfAnomalousClaims.ToString()), "")
        lblDetOverMajor.Text = IIf(model.IsOverMajor = "Y", "Yes", "No") + IIf(model.IsOverMajor = "Y", joinParenthesis(Me.GetGlobalResourceObject("Text", "NoOfRecord") + " " + model.NoOfIsOverMajor.ToString()), "")

        ' Further Action Data Collection
        Dim AList As New List(Of FurtherActionItem)
        Dim IList As New List(Of FurtherActionItem)
        Dim RList As New List(Of FurtherActionItem)

        'Issue Letter
        If model.AdvisoryLetterDate <> Date.MinValue Then
            IList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "AdvisoryLetter"), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(model.AdvisoryLetterDate)})
        End If
        If model.WarningLetterDate <> Date.MinValue Then
            IList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "WarningLetter"), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(model.WarningLetterDate)})
        End If
        If model.DelistLetterDate <> Date.MinValue Then
            IList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "DelistLetter"), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(model.DelistLetterDate)})
        End If
        If model.SuspendPaymentLetterDate <> Date.MinValue Then
            IList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "SuspendPaymentLetter"), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(model.SuspendPaymentLetterDate)})
        End If
        If model.SuspendEHCPAccountLetterDate <> Date.MinValue Then
            IList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "SuspendEHCPAccountLetter"), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(model.SuspendEHCPAccountLetterDate)})
        End If
        If model.OtherLetterDate <> Date.MinValue Then
            IList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "Others") + joinParenthesis(model.OtherLetterRemark), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(model.OtherLetterDate)})
        End If

        'Refer Parties
        If model.BoardAndCouncilDate <> Date.MinValue Then
            RList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "BoardAndCouncil"), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(model.BoardAndCouncilDate)})
        End If
        If model.PoliceDate <> Date.MinValue Then
            RList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "Police"), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(model.PoliceDate)})
        End If
        If model.SocialWelfareDepartmentDate <> Date.MinValue Then
            RList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "SocialWelfareDepartment"), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(model.SocialWelfareDepartmentDate)})
        End If
        If model.HKCustomsandExciseDepartmentDate <> Date.MinValue Then
            RList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "HKCustomsAndExciseDepartment"), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(model.HKCustomsandExciseDepartmentDate)})
        End If
        If model.ImmigrationDepartmentDate <> Date.MinValue Then
            RList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "ImmigrationDepartment"), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(model.ImmigrationDepartmentDate)})
        End If
        If model.LabourDeparmentDate <> Date.MinValue Then
            RList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "LabourDepartment"), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(model.LabourDeparmentDate)})
        End If
        If model.OtherPartyDate <> Date.MinValue Then
            RList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "Others") + joinParenthesis(model.OtherPartyRemark), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(model.OtherPartyDate)})
        End If

        ' Action to EHCP
        If model.SuspendEHCPDate <> Date.MinValue Then
            AList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "SuspendTheEHCPFromHCVS"), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(model.SuspendEHCPDate)})
        End If
        If model.DelistEHCPDate <> Date.MinValue Then
            AList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "DelistTheEHCP"), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(model.DelistEHCPDate)})
        End If
        If model.PaymentRecoverySuspensionDate <> Date.MinValue Then
            AList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "RecoveryOrSuspensionPayment"), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(model.PaymentRecoverySuspensionDate)})
        End If

        For Each Item As FurtherActionItem In IList
            Item.Action = Me.GetGlobalResourceObject("Text", "IssueLetter")
        Next

        For Each Item As FurtherActionItem In RList
            Item.Action = Me.GetGlobalResourceObject("Text", "ReferParties")
        Next

        For Each Item As FurtherActionItem In AList
            Item.Action = Me.GetGlobalResourceObject("Text", "ActionToEHCP")
        Next

        Dim repeatDt As DataTable = udtInspectionRecordBLL.GenFurtherActionTableData(AList, RList, IList)

        FurtherActionDetail.DataSource = repeatDt
        FurtherActionDetail.DataBind()
        If (repeatDt.Rows.Count > 0) Then
            lblFurtherActionDetailEmpty.Visible = False
            headerFurtherActionDetail.Visible = True
        Else
            lblFurtherActionDetailEmpty.Visible = True
            headerFurtherActionDetail.Visible = False
        End If

        repeatDt = udtInspectionRecordBLL.GetFollowupActionFromXML(model.FollowupAction)

        repDetFollowUpAction.DataSource = repeatDt
        repDetFollowUpAction.DataBind()
        If (repeatDt.Rows.Count > 0) Then
            lblFollowUpActionDetailEmpty.Visible = False
            headerFollowUpActionDetail.Visible = True
        Else
            lblFollowUpActionDetailEmpty.Visible = True
            headerFollowUpActionDetail.Visible = False
        End If

    End Sub

    'Approval Click
    Private Sub ucNoticePopUpConfirmApprove_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUpConfirmApprove.ButtonClick
        udcInfoMsgBox.Visible = False
        udcMsgBox.Visible = False
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        Dim checkRoleMsg = udtInspectionRecordBLL.CheckButtonEnable(ibtnApprove)
        If IsNothing(checkRoleMsg) Then
            Select Case e
                Case ucNoticePopUp.enumButtonClick.OK

                    udtAuditLogEntry.WriteStartLog(LogID.LOG00014, AuditLogDesc.IRM_ConfirmationPopupConfirm_Click)
                    Try
                        Dim model As New InspectionRecordModel
                        Dim newStatus As String = hdnStatus.Value
                        Dim strMessageCode As String = String.Empty
                        Dim FailMessageCode As String = ""
                        Dim inspectionModel As InspectionRecordModel = CType(Session(SESS_InspectionRecordModel), InspectionRecordModel)
                        Dim strAction As String = ""
                        Select Case hdnStatus.Value
                            Case InspectionStatus.ClosePendingApproval
                                newStatus = InspectionStatus.Closed
                                strMessageCode = MsgCode.MSG00001
                                strAction = "Close Case"
                                ' test
                            Case InspectionStatus.RemovePendingApproval
                                newStatus = InspectionStatus.Removed
                                strMessageCode = MsgCode.MSG00002
                                strAction = "Remove"
                            Case InspectionStatus.ReopenPendingApproval
                                newStatus = InspectionStatus.InspectionResultInputted
                                strMessageCode = MsgCode.MSG00003
                                strAction = "Reopen"
                        End Select
                        'Audit log
                        udtAuditLogEntry.AddDescripton("Inspection ID", inspectionModel.InspectionID)
                        udtAuditLogEntry.AddDescripton("Action", strAction)
                        udtAuditLogEntry.WriteStartLog(LogID.LOG00020, AuditLogDesc.IRM_Approve_Start)

                        With model
                            .InspectionID = lblDetInspectionID.Text.Trim()
                            .FileReferenceNo = lblDetFileNo.Text.Trim()
                            .UserID = udtHCVUUserBLL.GetHCVUUser.UserID
                            .RecordStatus = newStatus
                            .OriginalStatus = inspectionModel.RecordStatus
                            .TSMP = inspectionModel.TSMP
                        End With

                        If udtInspectionRecordBLL.UpdateRecord(model, "UpdateStatus") Then

                            ShowSuccessMessage(boxInfoMessage, strMessageCode, model.InspectionID, model.FileReferenceNo)
                            MultiViewIRM.SetActiveView(vActionResultBox)

                            udtAuditLogEntry.WriteEndLog(LogID.LOG00012, AuditLogDesc.IRM_Approve_Successful)
                        Else
                            Me.udcMsgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
                            Me.udcMsgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00013, AuditLogDesc.IRM_Approve_Fail)
                        End If
                        'Refresh Detail

                    Catch ex As Exception
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00013, AuditLogDesc.IRM_Approve_Fail)
                        Throw ex

                    End Try

                Case Else
                    udtAuditLogEntry.WriteStartLog(LogID.LOG00015, AuditLogDesc.IRM_ConfirmationPopupCancel_Click)
            End Select
        Else
            Me.udcMsgBox.AddMessage(checkRoleMsg)
            Me.udcMsgBox.BuildMessageBox("Warning", udtAuditLogEntry, LogID.LOG00013, AuditLogDesc.IRM_Approve_Fail)
        End If
    End Sub
    'Reject Click
    Private Sub ucNoticePopUpConfirmReject_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUpConfirmReject.ButtonClick
        udcInfoMsgBox.Visible = False
        udcMsgBox.Visible = False
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        Dim checkRoleMsg = udtInspectionRecordBLL.CheckButtonEnable(ibtnReject)
        If IsNothing(checkRoleMsg) Then
            Select Case e
                Case ucNoticePopUp.enumButtonClick.OK

                    udtAuditLogEntry.WriteStartLog(LogID.LOG00014, AuditLogDesc.IRM_ConfirmationPopupConfirm_Click)
                    Try
                        Dim model As New InspectionRecordModel
                        Dim newStatus As String = hdnStatus.Value
                        Dim strMessageCode As String = String.Empty

                        Dim inspectionModel As InspectionRecordModel = CType(Session(SESS_InspectionRecordModel), InspectionRecordModel)
                        Dim strAction As String = ""
                        Select Case hdnStatus.Value
                            Case InspectionStatus.ClosePendingApproval
                                newStatus = InspectionStatus.InspectionResultInputted
                                strMessageCode = MsgCode.MSG00004
                                strAction = "Close Case"
                            Case InspectionStatus.RemovePendingApproval
                                newStatus = inspectionModel.OriginalStatus 'Return to original status
                                strMessageCode = MsgCode.MSG00005
                                strAction = "Remove"
                            Case InspectionStatus.ReopenPendingApproval
                                newStatus = InspectionStatus.Closed
                                strMessageCode = MsgCode.MSG00006
                                strAction = "Reopen"
                        End Select

                        'Audit log
                        udtAuditLogEntry.AddDescripton("Inspection ID", inspectionModel.InspectionID)
                        udtAuditLogEntry.AddDescripton("Action", strAction)
                        udtAuditLogEntry.WriteStartLog(LogID.LOG00016, AuditLogDesc.IRM_Reject_Start)

                        With model
                            .InspectionID = lblDetInspectionID.Text.Trim
                            .FileReferenceNo = lblDetFileNo.Text.Trim()
                            .UserID = udtHCVUUserBLL.GetHCVUUser.UserID
                            .RecordStatus = newStatus
                            .OriginalStatus = String.Empty ' Clear original status
                            .TSMP = inspectionModel.TSMP
                        End With

                        If udtInspectionRecordBLL.UpdateRecord(model, "UpdateStatus") Then
                            ShowSuccessMessage(boxInfoMessage, strMessageCode, model.InspectionID, model.FileReferenceNo)
                            MultiViewIRM.SetActiveView(vActionResultBox)
                            udtAuditLogEntry.WriteEndLog(LogID.LOG00009, AuditLogDesc.IRM_Reject_Successful)
                        Else
                            Me.udcMsgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
                            Me.udcMsgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00010, AuditLogDesc.IRM_Reject_Fail)
                        End If

                    Catch ex As Exception
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00010, AuditLogDesc.IRM_Reject_Fail)
                        Throw ex
                    End Try

                Case Else
                    udtAuditLogEntry.WriteStartLog(LogID.LOG00015, AuditLogDesc.IRM_ConfirmationPopupCancel_Click)
            End Select
        Else
            Me.udcMsgBox.AddMessage(checkRoleMsg)
            Me.udcMsgBox.BuildMessageBox("Warning", udtAuditLogEntry, LogID.LOG00010, AuditLogDesc.IRM_Reject_Fail)
        End If
    End Sub
    'Back -> 1. Search Page & Result
    Protected Sub ibtnDetailBack_Click(sender As Object, e As ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00019, AuditLogDesc.IRM_DetailBack_Click)
        SearchInspectionRecord()
        SubjectOfficer_SeletedIndexChanged()
    End Sub
    'Determine Button
    Public Sub DetermineButtonVisible(model As InspectionRecordModel)
        Dim udtGeneralFunction As New GeneralFunction
        Dim udtHCVUUserBLL As New HCVUUserBLL
        Dim user As HCVUUserModel = udtHCVUUserBLL.GetHCVUUser()
        Dim strRequestUser As String = String.Empty

        ibtnDetailBack.Visible = True
        ibtnApprove.Visible = False
        ibtnReject.Visible = False

        udtGeneralFunction.UpdateImageURL(ibtnApprove, True)
        udtGeneralFunction.UpdateImageURL(ibtnReject, True)

        Select Case hdnStatus.Value

            Case InspectionStatus.ClosePendingApproval 'Close Case (Pending Approval)
                ibtnApprove.Visible = True
                ibtnReject.Visible = True
                strRequestUser = model.CloseRequestBy

            Case InspectionStatus.RemovePendingApproval 'Remove (Pending Approval)
                ibtnApprove.Visible = True
                ibtnReject.Visible = True
                strRequestUser = model.RemoveRequestBy

            Case InspectionStatus.ReopenPendingApproval 'Reopen (Pending Approval)
                ibtnApprove.Visible = True
                ibtnReject.Visible = True
                strRequestUser = model.ReopenRequestBy

        End Select

        ' Disable button if Requester and Approver is the same person
        If strRequestUser.Trim = user.UserID Then
            udtGeneralFunction.UpdateImageURL(ibtnApprove, False)
            udtGeneralFunction.UpdateImageURL(ibtnReject, False)

            Select Case hdnStatus.Value
                Case InspectionStatus.ClosePendingApproval 'Close Case (Pending Approval)
                    Me.udcInfoMsgBox.AddMessage(New SystemMessage(FuncCode, SeverityCode.SEVI, MsgCode.MSG00007))

                Case InspectionStatus.RemovePendingApproval 'Remove (Pending Approval)
                    Me.udcInfoMsgBox.AddMessage(New SystemMessage(FuncCode, SeverityCode.SEVI, MsgCode.MSG00008))

                Case InspectionStatus.ReopenPendingApproval 'Reopen (Pending Approval)
                    Me.udcInfoMsgBox.AddMessage(New SystemMessage(FuncCode, SeverityCode.SEVI, MsgCode.MSG00009))
            End Select

            Me.udcInfoMsgBox.BuildMessageBox()
        End If
    End Sub
#End Region
#Region "       3. Success"
    'Back ->  1. Search Page & Result
    Protected Sub ibtnMsgBoxBack_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnMsgBoxBack.Click
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00021, AuditLogDesc.IRM_CompletePageBack_Button_Click)

        MultiViewIRM.SetActiveView(vSEARCH)
        SearchInspectionRecord()
        SubjectOfficer_SeletedIndexChanged()
    End Sub
#End Region

#End Region

#Region "Base Flow"
    Protected Overrides Sub SF_InitSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry)
    End Sub

    Protected Overrides Function SF_ValidateSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry) As Boolean
        Dim blnReturn As Boolean = True

        If ddlAction.SelectedIndex = 0 Then
            blnReturn = False
            gvApprovalResult.Visible = False
        End If

        Return blnReturn
    End Function

    Protected Overrides Function SF_Search(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry, ByVal blnOverrideResultLimit As Boolean) As Common.Component.BaseBLL.BLLSearchResult

        Dim bllSearchResult As Common.Component.BaseBLL.BLLSearchResult

        bllSearchResult = Nothing

        Dim strRecordStatus As String = String.Empty
        If ddlAction.SelectedIndex > 0 Then
            Select Case ddlAction.SelectedValue
                Case ActionType.Close
                    strRecordStatus = InspectionStatus.ClosePendingApproval
                Case ActionType.Remove
                    strRecordStatus = InspectionStatus.RemovePendingApproval
                Case ActionType.Reopen
                    strRecordStatus = InspectionStatus.ReopenPendingApproval
            End Select
        End If

        Dim param As New GetInspectionParameter With {
                .RecordStatus = strRecordStatus,
                .UserId = udtHCVUUserBLL.GetHCVUUser.UserID,
                .SubjectOfficerID = ddlSubjectOfficer.SelectedValue
            }
        Dim dt As New DataTable()
        dt = udtInspectionRecordBLL.SearchInspectionRecordByAny(param)
        bllSearchResult = New BaseBLL.BLLSearchResult(dt, True, True, EnumSqlErrorMessage.Normal)
        'bllSearchResult = udtInspectionRecordBLL.SearchInspectionRecordByAny(FunctionCode, param, udtHCVUUserBLL.GetHCVUUser.UserID, False)

        Return bllSearchResult
    End Function

    Protected Overrides Function SF_BindSearchResult(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry, ByVal udtBLLSearchResult As Common.Component.BaseBLL.BLLSearchResult) As Integer

        Return BindInspectionRecord(udtBLLSearchResult)
        'Dim dt As DataTable
        'Dim intRowCount As Integer
        'Dim blnShowResultList As Boolean = False

        'Try
        '    dt = CType(udtBLLSearchResult.Data, DataTable)
        '    Dim dv As DataView = dt.DefaultView
        '    dv.Sort = "Visit_Date DESC"
        '    dt = dv.ToTable()
        '    dv.Sort = "Subject_Officer_ID"
        '    Dim subjectOfficerDT As DataTable = dv.ToTable(True, {"Subject_Officer_Value", "Subject_Officer_ID"})

        '    For Each dr As DataRow In subjectOfficerDT.Rows
        '        If String.IsNullOrEmpty(dr("Subject_Officer_Value")) Then
        '            dr("Subject_Officer_Value") = Me.GetGlobalResourceObject("Text", "NA")
        '            dr("Subject_Officer_ID") = "NA"
        '        End If
        '    Next
        '    'Save Session
        '    Session(SESS_SearchApprovalResultDataTableForFilter) = dt
        '    Session(SESS_SearchApprovalResultDataTable) = dt

        '    SetSubjectOfficer(subjectOfficerDT, ddlSubjectOfficer.SelectedValue.Trim)


        'Catch ex As Exception
        '    Throw
        'End Try

        'intRowCount = dt.Rows.Count

        'Select Case dt.Rows.Count
        '    Case 0
        '        ' No record found
        '        blnShowResultList = False
        '    Case Else
        '        blnShowResultList = True
        'End Select

        'If blnShowResultList Then
        '    gvApprovalResult.Visible = True
        '    Me.GridViewDataBind(Me.gvApprovalResult, dt, "Visit_Date", "DESC", False)
        'Else
        '    gvApprovalResult.Visible = False
        'End If

        'Return intRowCount
    End Function

    Protected Overrides Sub SF_FinalizeSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry)
    End Sub

    Protected Overrides Sub SF_ConfirmSearch_Click()

    End Sub

    Protected Overrides Sub SF_CancelSearch_Click()
    End Sub
#End Region

#Region "Common Function"
    'UI
    Private Function joinParenthesis(str As String) As String
        Return String.Format(" ({0})", str)
    End Function
    Private Sub SetLabelcolor(control As Label, Optional ByVal isOptionalField As Boolean = False)
        If control.Text.Trim() = "" Then
            If isOptionalField Then
                control.Text = Me.GetGlobalResourceObject("Text", "Empty")
                control.Style.Remove("color")
                control.Style.Remove("font-weight")
            Else
                control.Text = Me.GetGlobalResourceObject("Text", "ToBeProvided")
                control.Style.Add("color", "red")
                control.Style.Add("font-weight", "600")
            End If

        Else
            control.Style.Remove("color")
            control.Style.Remove("font-weight")
        End If
    End Sub

    Public Sub ShowSuccessMessage(box As CustomControls.InfoMessageBox, msgCode As String, InspectionID As String, FileRefNo As String)
        udcInfoMsgBox.Visible = False
        box.Type = CustomControls.InfoMessageBoxType.Complete
        Dim strOldCharList() As String
        Dim objNewCharList() As String

        strOldCharList = New String() {"%s", "%f"}
        objNewCharList = New String() {InspectionID, FileRefNo}
        box.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVI, msgCode), strOldCharList, objNewCharList)
        box.BuildMessageBox()
        box.Visible = True

    End Sub

    Public Sub SearchInspectionRecord()
        MultiViewIRM.SetActiveView(vSEARCH)
        Dim udtBLLSearchResult As BaseBLL.BLLSearchResult = GetInspectionRecordList()

        Select Case udtBLLSearchResult.SqlErrorMessage
            Case BaseBLL.EnumSqlErrorMessage.Normal
                BindInspectionRecord(udtBLLSearchResult)

            Case BaseBLL.EnumSqlErrorMessage.OverResultListOverrideLimit
                Me.udcMsgBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, MsgCode.MSG00017))
                Me.udcMsgBox.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00003, "Search Fail")
                Return
            Case Else
                Throw New Exception("Error: Class = [HCVU.InspectionRecordApproval], Method = [ibtnSearch_Click], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")
        End Select
    End Sub


    Private Function GetInspectionRecordList() As BaseBLL.BLLSearchResult
        Dim bllSearchResult As Common.Component.BaseBLL.BLLSearchResult

        bllSearchResult = Nothing

        Dim strRecordStatus As String = String.Empty
        If ddlAction.SelectedIndex > 0 Then
            Select Case ddlAction.SelectedValue
                Case ActionType.Close
                    strRecordStatus = InspectionStatus.ClosePendingApproval
                Case ActionType.Remove
                    strRecordStatus = InspectionStatus.RemovePendingApproval
                Case ActionType.Reopen
                    strRecordStatus = InspectionStatus.ReopenPendingApproval
            End Select
        End If

        Dim param As New GetInspectionParameter With {
                .RecordStatus = strRecordStatus,
                .UserId = udtHCVUUserBLL.GetHCVUUser.UserID
            }
        Dim dt As New DataTable()
        dt = udtInspectionRecordBLL.SearchInspectionRecordByAny(param)
        bllSearchResult = New Common.Component.BaseBLL.BLLSearchResult(dt, True, True, EnumSqlErrorMessage.Normal)
        Return bllSearchResult
    End Function
    Public Function BindInspectionRecord(ByVal udtBLLSearchResult As BaseBLL.BLLSearchResult) As Integer
        udcInfoMsgBox.Visible = False
        udcMsgBox.Visible = False

        Dim dt As DataTable
        Dim intRowCount As Integer
        Dim blnShowResultList As Boolean = False

        Try
            dt = CType(udtBLLSearchResult.Data, DataTable)
            Dim dv As DataView = dt.DefaultView
            dv.Sort = "Visit_Date DESC"
            dt = dv.ToTable()
            dv.Sort = "Subject_Officer_Value"
            Dim subjectOfficerDT As DataTable = dv.ToTable(True, {"Subject_Officer_Value", "Subject_Officer_ID"})
            Dim NAItem As DataRow = Nothing
            For index As Integer = 0 To subjectOfficerDT.Rows.Count - 1
                'Remove 'N/A' Item
                If String.IsNullOrEmpty(subjectOfficerDT.Rows(index)("Subject_Officer_Value")) Then
                    NAItem = subjectOfficerDT.NewRow
                    NAItem("Subject_Officer_Value") = Me.GetGlobalResourceObject("Text", "NA")
                    NAItem("Subject_Officer_ID") = "NA"
                    subjectOfficerDT.Rows.RemoveAt(index)
                    Exit For
                End If
            Next
            If Not IsNothing(NAItem) Then
                ' N/A be the last
                subjectOfficerDT.Rows.Add(NAItem)
            End If
            SetSubjectOfficer(subjectOfficerDT, ddlSubjectOfficer.SelectedValue)

            'Save Session
            Session(SESS_SearchApprovalResultDataTableForFilter) = dt
            Session(SESS_SearchApprovalResultDataTable) = dt
        Catch ex As Exception
            Throw
        End Try

        intRowCount = dt.Rows.Count

        Select Case dt.Rows.Count
            Case 0
                ' No record found
                blnShowResultList = False
            Case Else
                blnShowResultList = True
        End Select

        If blnShowResultList Then
            gvApprovalResult.Visible = True
            Me.GridViewDataBind(Me.gvApprovalResult, dt, "Visit_Date", "DESC", False)

        Else
            gvApprovalResult.Visible = False
            udcInfoMsgBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
            udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Information
            udcInfoMsgBox.BuildMessageBox()
        End If

        Return intRowCount
    End Function
#End Region

    Protected Sub ibtnApprove_Click(sender As Object, e As ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00011, AuditLogDesc.IRM_Approve_click)
        ModalPopupConfirmApprove.Show()
    End Sub

    Protected Sub ibtnReject_Click(sender As Object, e As ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00008, AuditLogDesc.IRM_Reject_click)
        ModalPopupConfirmReject.Show()
    End Sub

#Region "Must override function - Master Page"

    Public Overrides Function GetDocCode() As String
        Return Nothing
    End Function

    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Return Nothing
    End Function

    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        Return Nothing
    End Function

#End Region
End Class