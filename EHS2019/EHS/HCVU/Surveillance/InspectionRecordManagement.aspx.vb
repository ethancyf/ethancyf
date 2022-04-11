Imports System.Threading
Imports System.Globalization
Imports System.Xml
Imports Common.ComObject
Imports Common.Component.EHSAccount
Imports Common.Component
Imports Common.Component.Scheme
Imports Common.ComFunction
Imports Common.DataAccess
Imports System.Data.SqlClient
Imports Common.Component.StaticData
Imports Common.Component.HCVUUser
Imports Common.Validation
Imports CustomControls
Imports System.IO
Imports Common.Component.UserRole
Imports Common.Component.FileGeneration
Imports Common.Component.Address
Imports Common
Imports System.Reflection
Imports Microsoft.Office.Interop

Partial Class InspectionRecordManagement
    Inherits BasePageWithControl

#Region "Property"
    Dim udtAuditLogEntry As AuditLogEntry
    Dim udtSM As Common.ComObject.SystemMessage
    Dim udtValidator As New Validator
    Dim udtformatter As New Common.Format.Formatter
    Dim udtHCVUUserBLL As New HCVUUserBLL
    Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
    Private udtSessionHandlerBLL As New BLL.SessionHandlerBLL
    Private udtInspectionRecordBLL As New InspectionRecordBLL
    Public Const SEARCH As Integer = 0
    Public Const RESULT As Integer = 1
    Public Const INSPECTIONDETAIL As Integer = 2
    Public Const EDITVISIT As Integer = 3
    Public Const EDITVISITCONFIRM As Integer = 4
    Public Const SUBMIT As Integer = 5
    Public Const INSPECTIONCONFIRM As Integer = 6
#End Region

#Region "Audit Log Description"
    Public Class AuditLogDesc
        Public Const Load As String = "Inspection Record Management - Load" '0

        Public Const IRM_SearchResult_Fail As String = "Inspection Record Management - Search Inspection Record Fail" '01

        Public Const IRM_SearchSP As String = "Inspection Record Management - Search SP click" '02
        Public Const IRM_SearchSP_Success As String = "Inspection Record Management - Search SP Successful" '03
        Public Const IRM_SearchSP_Fail As String = "Inspection Record Management - Search SP Fail" '04

        Public Const IRM_Result_LinkButton_Click As String = "Inspection Record Management - Result Link Button Click" '05

        Public Const IRM_Print_Button_Click As String = "Inspection Record Management - Print Button Click" '06

        Public Const IRM_Print_PDF_Button_Click As String = "Inspection Record Management - Print PDF Button Click" '07

        Public Const IRM_New_Confirm_Click As String = "Inspection Record Management - New Inspection Record Confirm click" '08
        Public Const IRM_New_Confirm_Success As String = "Inspection Record Management - New Inspection Record Confirm Successful" '09
        Public Const IRM_New_Confirm_Fail As String = "Inspection Record Management - New Inspection Record Confirm Fail" '10

        Public Const IRM_Print_WORD_Button_Click As String = "Inspection Record Management - Print WORD Button Click" '11
        Public Const IRM_Print_Cancel_Button_Click As String = "Inspection Record Management - Print Cancel Button Click" '12

        Public Const IRM_SP_Clear_Button_Click As String = "Inspection Record Management - SP Clear Button Click" '13

        Public Const IRM_DownloadNow_Button_Click As String = "Inspection Record Management - DownloadNow Button Click" '14
        Public Const IRM_DownloadLater_Button_Click As String = "Inspection Record Management - DownloadLater Button Click" '15

        Public Const IRM_ResultBack_Button_Click As String = "Inspection Record Management - Search Result Back Button Click" '16
        Public Const IRM_DetailBack_Button_Click As String = "Inspection Record Management - Detail Back Button Click" '17
        Public Const IRM_NewInspectionRecordBack_Button_Click As String = "Inspection Record Management - New Inspection Record Back Button Click" '18
        Public Const IRM_NewRecordConfirmPageBack_Button_Click As String = "Inspection Record Management New Inspection Record Confirm page Back Button Click" '19

        Public Const IRM_ViewInspectionDetail_Click As String = "Inspection Record Management View Inspection Detail click" '20
        Public Const IRM_ViewInspectionDetail_Successful As String = "Inspection Record Management View Inspection Detail Successful" '21
        Public Const IRM_ViewInspectionDetail_Fail As String = "Inspection Record Management View Inspection Detail Fail" '22

        Public Const IRM_NewInspection_Click As String = "Inspection Record Management New Inspection click" '23
        Public Const IRM_NewInspection_Successful As String = "Inspection Record Management New Inspection click Successful" '24
        Public Const IRM_NewInspection_Fail As String = "Inspection Record Management New Inspection Fail" '25

        Public Const IRM_CompletePageBack_Button_Click As String = "Inspection Record Management - Complete page - Back Button Click" '26
        Public Const IRM_EditVisitDetailBack_Button_Click As String = "Inspection Record Management - Edit Visit Detail Back Button Click" '27
        Public Const IRM_CompletePageReturn_Button_Click As String = "Inspection Record Management - Complete page - Return Button Click" '28

        Public Const IRM_NewInspectionSubmit_Click As String = "Inspection Record Management NewInspectionSubmit click" '29
        Public Const IRM_NewInspectionSubmit_Successful As String = "Inspection Record Management NewInspectionSubmit Successful" '30
        Public Const IRM_NewInspectionSubmit_Fail As String = "Inspection Record Management NewInspectionSubmit Fail" '31

        Public Const IRM_EditVisitSave_Click As String = "Inspection Record Management EditVisitSave click" '32
        Public Const IRM_EditVisitSave_Successful As String = "Inspection Record Management EditVisitSave Successful" '33
        Public Const IRM_EditVisitSave_Fail As String = "Inspection Record Management EditVisitSave Fail" '34

        Public Const IRM_EditVisitConfirm_Click As String = "Inspection Record Management EditVisitConfirm click" '35
        Public Const IRM_EditVisitConfirm_Successful As String = "Inspection Record Management EditVisitConfirm Successful" '36
        Public Const IRM_EditVisitConfirm_Fail As String = "Inspection Record Management EditVisitConfirm Fail" '37

        Public Const IRM_EditDetail_Click As String = "Inspection Record Management EditDetail click" '38
        Public Const IRM_EditDetail_Successful As String = "Inspection Record Management EditDetail Successful" '39
        Public Const IRM_EditDetail_Fail As String = "Inspection Record Management EditDetail Fail" '40

        Public Const IRM_DownloadReportClose_Button_Click As String = "Inspection Record Management Download Report Close Button Click" '41
        Public Const IRM_InspectionResultBack_Button_Click As String = "Inspection Record Management Inspection Result Back Button Click" '42
        Public Const IRM_ResultConfirmPageBack_Button_Click As String = "Inspection Record Management Inspection Result Confirm Page - Back Button Click" '43

        Public Const IRM_Search_Click As String = "Inspection Record Management Search click" '44
        Public Const IRM_Search_Successful As String = "Inspection Record Management Search Successful" '45
        Public Const IRM_Search_Fail As String = "Inspection Record Management Search Fail" '46

        Public Const IRM_InputInspectionResult_Click As String = "Inspection Record Management InputInspectionResult click" '47
        Public Const IRM_InputInspectionResult_Successful As String = "Inspection Record Management InputInspectionResult Successful" '48
        Public Const IRM_InputInspectionResult_Fail As String = "Inspection Record Management InputInspectionResult Fail" '49

        Public Const IRM_InspectionResultSave_Click As String = "Inspection Record Management InspectionResult Save click" '50
        Public Const IRM_InspectionResultSave_Successful As String = "Inspection Record Management InspectionResult Save Successful" '51
        Public Const IRM_InspectionResultSave_Fail As String = "Inspection Record Management InspectionResult Save Fail" '52

        Public Const IRM_IRConfirmSave_Click As String = "Inspection Record Management IRConfirmSave click" '53
        Public Const IRM_IRConfirmSave_Successful As String = "Inspection Record Management IRConfirmSave Successful" '54
        Public Const IRM_IRConfirmSave_Fail As String = "Inspection Record Management IRConfirmSave Fail" '55

        Public Const IRM_EditInspectionResult_Click As String = "Inspection Record Management EditInspectionResult click" '56
        Public Const IRM_EditInspectionResult_Successful As String = "Inspection Record Management EditInspectionResult Successful" '57
        Public Const IRM_EditInspectionResult_Fail As String = "Inspection Record Management EditInspectionResult Fail" '58

        Public Const IRM_RequestRemove_Start As String = "Inspection Record Management RequestRemove Start" '59
        Public Const IRM_RequestRemove_Successful As String = "Inspection Record Management RequestRemove Successful" '60
        Public Const IRM_RequestRemove_Fail As String = "Inspection Record Management RequestRemove Fail" '61

        Public Const IRM_RequestClose_Start As String = "Inspection Record Management RequestClose Start" '62
        Public Const IRM_RequestClose_Successful As String = "Inspection Record Management RequestClose Successful" '63
        Public Const IRM_RequestClose_Fail As String = "Inspection Record Management RequestClose Fail" '64

        Public Const IRM_RequestReopen_Start As String = "Inspection Record Management RequestReopen Start" '65
        Public Const IRM_RequestReopen_Successful As String = "Inspection Record Management RequestReopen Successful" '66
        Public Const IRM_RequestReopen_Fail As String = "Inspection Record Management RequestReopen Fail" '67

        Public Const IRM_EditVisitDetailConfirm_Back_Click As String = "Inspection Record Management Edit Visit Detail Confirm page - Back Click" '68

        Public Const IRM_Confirm_Popup_No_Click As String = "Inspection Record Management Confirm popup - No Click" '69
        Public Const IRM_Confirm_Popup_Yes_Click As String = "Inspection Record Management Confirm popup - Yes Click" '70

        Public Const IRM_Download_Click As String = "Inspection Record Management ibtnDownload Click" '71
        Public Const IRM_Download_Click_Successful As String = "Inspection Record Management ibtnDownload Successful" '72
        Public Const IRM_Download_Click_Fail As String = "Inspection Record Management ibtnDownload Fail" '73

        Public Const IRM_Generate_Report_Load As String = "Inspection Record Management Generate Report Load" '74
        Public Const IRM_Generate_Report_Successful As String = "Inspection Record Management Generate Report Successful" '75
        Public Const IRM_Generate_Report_Fail As String = "Inspection Record Management Generate Report Fail" '76

        Public Const IRM_RequestRemove_Click As String = "Inspection Record Management Remove Click " '77
        Public Const IRM_RequestClose_Click As String = "Inspection Record Management Close Case Click " '78
        Public Const IRM_RequestReopen_Click As String = "Inspection Record Management Reopen Case Click " '79

        Public Const IRM_Confirm_Popup_Confirm_Click As String = "Inspection Record Management Confirm popup - Confirm Click" '80
        Public Const IRM_Confirm_Popup_Cancel_Click As String = "Inspection Record Management Confirm popup - Cancel Click" '81

        Public Const IRM_Export_Excel_Report_Click As String = "Inspection Record Management Export Excel Report Click" '82
        Public Const IRM_Export_Excel_Report_Start As String = "Inspection Record Management Export Excel Report Start" '83
        Public Const IRM_Export_Excel_Report_Successful As String = "Inspection Record Management Export Excel Report Successful" '84
        Public Const IRM_Export_Excel_Report_Fail As String = "Inspection Record Management Export Excel Report Fail" '85

        Public Const IRM_ReferredReferenceNo_Click As String = "Inspection Record Management - Referred File Reference No Click" '86

    End Class
#End Region

#Region "Constant Value"
    Private Const SESS_Language As String = "language"
    Private Const SESS_ServiceProvider As String = "Surveillance_InspectionRecord_Management_ServiceProviderModel"
    Private Const SESS_SearchResultDataTable As String = "Surveillance_InspectionRecord_Management_SearchResultDataTable"
    Private Const SESS_SearchApprovalResultDataTable As String = "Surveillance_InspectionRecord_Approval_SearchResultDataTable"
    Private Const SESS_InspectionRecordModelNew As String = "Surveillance_InspectionRecord_Management_New_Model"
    Private Const SESS_InspectionRecordModel As String = "Surveillance_InspectionRecord_Management_Model"
    Private Const SESS_InspectionRecordModelEdit As String = "Surveillance_InspectionRecord_Management_Model_Edit"
    Private Const SESS_FollowupAction_DataTable As String = "Surveillance_InspectionRecord_Management_FollowupAction_DataTable"
    Private Const SESS_FollowupAction_DataTable_Edit As String = "Surveillance_InspectionRecord_Management_FollowupAction_DataTable_Edit"

    Private Const FuncCode As String = FunctCode.FUNT011101
    Private Const CommonFunctionCode As String = FunctCode.FUNT990000

    Public Const SESS_dictionaryTimeStampSessKey As String = "011101_Dictionary_Timestamp_Path"


#End Region

#Region "Page function"

    Private Overloads Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        ' Get HCVU User to check session expire
        Dim udtHCVUUserBLL As New HCVUUserBLL
        Dim user As HCVUUserModel = udtHCVUUserBLL.GetHCVUUser()
        Dim inspectionRole As InspectionRole = udtInspectionRecordBLL.GetInspectionRole(user)
        'user.UserRoleCollection
        FunctionCode = FuncCode

        If Not IsPostBack Then
            SetSearchTypeofInspection()
            SetSearchStatus()

            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, AuditLogDesc.Load)

        Else
            Dim activeViewID As String = MultiViewIRM.GetActiveView().ID

            If activeViewID = vNewInspection.ID Then
                HighlightTypeOfInspectionMultiple(rdoListAddMainTypeofInspection.ClientID, chkListAddTypeofInspection.ClientID)
                SetTxtReferNoOnKeyPress(PageMode.ModeNew)
            End If
            If activeViewID = vEditVisit.ID Then
                HighlightTypeOfInspection(chkListEdtTypeofInspection.ClientID)
                SetTxtReferNoOnKeyPress(PageMode.ModeEdit)
            End If
            If activeViewID = vInputInspectionResult.ID Then
                lblIIRTotalCheck.Text = udtInspectionRecordBLL.ConvertStringToInt(txtIIRInOrder.Text) + udtInspectionRecordBLL.ConvertStringToInt(txtIIRMissingForm.Text) + udtInspectionRecordBLL.ConvertStringToInt(txtIIRInconsistent.Text)
            End If
        End If
        If inspectionRole.IsOfficer Then
            udtGeneralFunction.UpdateImageURL(ibtnNewInspection, True)
        Else
            udtGeneralFunction.UpdateImageURL(ibtnNewInspection, False)
        End If
        'Add onKeyUp Function
        SetPrefixLabel()
        Me.ModalPopupConfirmCloseCase.PopupDragHandleControlID = Me.ucNoticePopUpConfirmCloseCase.Header.ClientID
        Me.ModalPopupConfirmRemove.PopupDragHandleControlID = Me.ucNoticePopUpConfirmRemove.Header.ClientID
        HiddenTypeOfInspectionCheckBox()
        checkPasswordValidation()

    End Sub
#Region "       1. Search Page & Result"
    'Set Criteria
    Private Sub SetSearchTypeofInspection()
        Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL
        Me.ddlTypeofInspection.Items.Clear()
        Me.ddlTypeofInspection.DataSource = udtStaticDataBLL.GetStaticDataListByColumnName("TypeOfInspection") '  ClaimCreationReason
        Me.ddlTypeofInspection.DataTextField = "DataValue"
        Me.ddlTypeofInspection.DataValueField = "ItemNo"
        Me.ddlTypeofInspection.DataBind()
        Me.ddlTypeofInspection.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))
        Me.ddlTypeofInspection.SelectedIndex = 0
    End Sub
    Private Sub SetSearchStatus()
        Me.ddlStatus.Items.Clear()
        Me.ddlStatus.DataSource = Status.GetDescriptionListFromDBEnumCode("InspectionStatus") ' HCVUClaimTransManagementStatus
        Me.ddlStatus.DataTextField = "Status_Description"
        Me.ddlStatus.DataValueField = "Status_Value"
        Me.ddlStatus.DataBind()
        Me.ddlStatus.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))
        Me.ddlStatus.SelectedIndex = 0
    End Sub
    'Search Function
    Protected Sub ibtnSearch_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnSearch.Click
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00044, AuditLogDesc.IRM_Search_Click)

        hfEGenerationID.Value = String.Empty
        Me.udcInfoMsgBox.Visible = False
        Me.udcMsgBox.Visible = False
        Dim enumSearchResult As SearchResultEnum 'Result Status        
        Try

            If sender Is Nothing Then
                enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, udcMsgBox, udcInfoMsgBox, False, True)
            Else
                enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, udcMsgBox, udcInfoMsgBox)
            End If
            Select Case enumSearchResult
                Case SearchResultEnum.Success '0
                    MultiViewIRM.SetActiveView(vSearchResult)
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00045, AuditLogDesc.IRM_Search_Successful)
                Case SearchResultEnum.ValidationFail '1
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00046, AuditLogDesc.IRM_Search_Fail)
                Case SearchResultEnum.NoRecordFound '2
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00046, AuditLogDesc.IRM_Search_Fail)
                Case SearchResultEnum.OverResultList1stLimit_PopUp
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00046, AuditLogDesc.IRM_Search_Fail)
                Case SearchResultEnum.OverResultList1stLimit_Alert
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00046, AuditLogDesc.IRM_Search_Fail)
                Case SearchResultEnum.OverResultListOverrideLimit
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00046, AuditLogDesc.IRM_Search_Fail)
                Case Else
                    Throw New Exception("Error: Class = [HCVU.InspectionRecordManagement], Method = [ibtnSearch_Click], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")
            End Select

        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00046, AuditLogDesc.IRM_Search_Fail)
        End Try
    End Sub
    'Result Table Function
    Protected Sub gvSearchResult_Prerender(sender As Object, e As EventArgs) Handles gvSearchResult.PreRender
        GridViewPreRenderHandler(sender, e, SESS_SearchResultDataTable)
    End Sub
    Protected Sub gvSearchResult_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvSearchResult.Sorting
        GridViewSortingHandler(sender, e, SESS_SearchResultDataTable)
    End Sub
    Protected Sub gvSearchResult_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvSearchResult.PageIndexChanging
        GridViewPageIndexChangingHandler(sender, e, SESS_SearchResultDataTable)
    End Sub

    Private Sub gvSearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSearchResult.RowDataBound
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

    'New Inspection Record Click -> 2.1 New Inspection Record - Detail
    Protected Sub ibtnNewInspection_Click(sender As Object, e As ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00023, AuditLogDesc.IRM_NewInspection_Click)

        Dim checkRoleMsg = udtInspectionRecordBLL.CheckButtonEnable(ibtnEditDetail)
        If IsNothing(checkRoleMsg) Then
            HideErrorImg()
            hdnStatus.Value = InspectionStatus.Creating

            MultiViewIRM.SetActiveView(vNewInspection)
            Me.udcInfoMsgBox.Visible = False
            Me.udcMsgBox.Visible = False

            Dim dataField As DetailDataField = GetDetailDataFieldByMode(PageMode.ModeNew)
            'Set Main Type Of Inspection
            SetTypeofInspectionRadioButtonList(dataField.rdoListMainTypeOfInspection)
            rdoListAddMainTypeofInspection_SelectedIndexChanged(dataField.rdoListMainTypeOfInspection, Nothing)
            dataField.rdoListMainTypeOfInspection.Attributes.Add("onclick", "HighlightType(this);")
            'Set Type Of Inspection
            SetTypeofInspectionCheckBoxList(dataField.chkListTypeOfInspection)
            dataField.chkListTypeOfInspection.Attributes.Add("onclick", "HighlightType(this);")
            'Set File Reference Type
            SetFileType(dataField.rdoFileReferenceType)
            rdoFileReferenceType_SelectedIndexChanged(dataField.rdoFileReferenceType, Nothing)
            'Set Form Condition
            SetFormCondition(dataField.ddlFormCondition)
            ddlFormCondition_SelectedIndexChanged(dataField.ddlFormCondition, Nothing)
            'Set MeansofCommunication
            SetMeansofCommunication(dataField.ddlMeansOfCommunication)
            ddlMeansofCommunication_SelectedIndexChanged(dataField.ddlMeansOfCommunication, Nothing)
            'Set Officer List
            SetOfficerList(dataField)
            'Set Refer No 
            SetTxtReferNoOnKeyPress(PageMode.ModeNew)

            clearNewInspectionForm()

            udtAuditLogEntry.WriteEndLog(LogID.LOG00024, AuditLogDesc.IRM_NewInspection_Successful)
        Else
            Me.udcMsgBox.AddMessage(checkRoleMsg)
            Me.udcMsgBox.BuildMessageBox("Warning", udtAuditLogEntry, LogID.LOG00025, AuditLogDesc.IRM_NewInspection_Fail)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00025, AuditLogDesc.IRM_NewInspection_Fail)
        End If
    End Sub
    'Result Click -> 3. Inspection Record - Detail Show
    Protected Sub ResultLbtn_Click(sender As Object, e As EventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode)
        Dim btn As LinkButton = CType(sender, LinkButton)
        udtAuditLogEntry.AddDescripton("Inspection ID", btn.Text)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00023, AuditLogDesc.IRM_Result_LinkButton_Click)

        GetDetail(btn.Text)

    End Sub
    'Result Back -> 1. Search Page
    Protected Sub ibtnResultBack_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnResultBack.Click
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00016, AuditLogDesc.IRM_ResultBack_Button_Click)

        MultiViewIRM.SetActiveView(vSEARCH)
    End Sub
    'Export Report Click (Generate Excel)
    Protected Sub ibtnEExportReport_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteStartLog(LogID.LOG00082, AuditLogDesc.IRM_Export_Excel_Report_Click)

        If hfEGenerationID.Value <> String.Empty Then
            ' File has been previously generated
            mpeExportReport.Show()
            udtAuditLog.AddDescripton("Generation ID", hfEGenerationID.Value)
            udtAuditLog.WriteEndLog(LogID.LOG00084, AuditLogDesc.IRM_Export_Excel_Report_Successful)
            Return
        End If
        udtAuditLog.WriteStartLog(LogID.LOG00083, AuditLogDesc.IRM_Export_Excel_Report_Start)
        Dim resultData As DataTable = Session(SESS_SearchResultDataTable)
        Dim dt As New DataTable
        dt.Columns.Add("Inspection_ID", GetType(String))
        dt.Columns.Add("File_Reference_No", GetType(String))
        dt.Columns.Add("SP_ID", GetType(String))
        dt.Columns.Add("SP_Name", GetType(String))
        dt.Columns.Add("Practice_Name", GetType(String))
        dt.Columns.Add("Main_Type_Of_Inspection", GetType(String))
        dt.Columns.Add("Visit_Date", GetType(String))
        dt.Columns.Add("Case_Officer", GetType(String))
        dt.Columns.Add("Subject_Officer", GetType(String))
        dt.Columns.Add("Follow_Up_Action", GetType(String))
        dt.Columns.Add("Status", GetType(String))
        ' Header row
        Dim drHeader As DataRow = dt.NewRow
        drHeader("Inspection_ID") = Me.GetGlobalResourceObject("Text", "InspectionRecordID")
        drHeader("File_Reference_No") = Me.GetGlobalResourceObject("Text", "FileReferenceNo")
        drHeader("SP_ID") = Me.GetGlobalResourceObject("Text", "SPIDPracticeID")
        drHeader("SP_Name") = Me.GetGlobalResourceObject("Text", "SPNameShort")
        drHeader("Practice_Name") = Me.GetGlobalResourceObject("Text", "Practice")
        drHeader("Main_Type_Of_Inspection") = Me.GetGlobalResourceObject("Text", "MainTypeOfInspection")
        drHeader("Visit_Date") = Me.GetGlobalResourceObject("Text", "VisitDate")
        drHeader("Case_Officer") = Me.GetGlobalResourceObject("Text", "CaseOfficer")
        drHeader("Subject_Officer") = Me.GetGlobalResourceObject("Text", "SubjectOfficer")
        drHeader("Follow_Up_Action") = Me.GetGlobalResourceObject("Text", "FollowUpAction")
        drHeader("Status") = Me.GetGlobalResourceObject("Text", "Status")
        dt.Rows.Add(drHeader)

        For Each Item As DataRow In resultData.Rows
            Dim dr As DataRow = dt.NewRow
            dr("Inspection_ID") = Item("Inspection_ID")
            dr("File_Reference_No") = Item("File_Reference_No")
            dr("SP_ID") = Item("SP_ID") + joinParenthesis(Item("Practice_Display_Seq"))
            dr("SP_Name") = Item("SP_Eng_Name") + Environment.NewLine

            If Not IsDBNull(Item("SP_Chi_Name")) Then
                Dim strSPChiName As String = Item("SP_Chi_Name")
                If Not String.IsNullOrEmpty(strSPChiName) Then
                    dr("SP_Name") += joinParenthesis(Item("SP_Chi_Name"))
                End If
            End If
            dr("Practice_Name") = Item("Practice_Name")
            dr("Main_Type_Of_Inspection") = Item("Main_Type_Of_Inspection_Value")
            dr("Visit_Date") = Item("Visit_Date_Format")

            Dim strSubjectOfficer As String = Item("Subject_Officer_Value")
            If Not String.IsNullOrEmpty(strSubjectOfficer) Then
                dr("Subject_Officer") = strSubjectOfficer
            Else
                dr("Subject_Officer") = Me.GetGlobalResourceObject("Text", "NA")
            End If

            Dim strCaseOfficer As String = Item("Case_Officer_Value")
            If Not String.IsNullOrEmpty(strCaseOfficer) Then
                dr("Case_Officer") = strCaseOfficer
            Else
                dr("Case_Officer") = Me.GetGlobalResourceObject("Text", "NA")
            End If

            dr("Follow_Up_Action") = Item("Follow_Up_Action")
            dr("Status") = Item("Status_Description")
            dt.Rows.Add(dr)
        Next
        Dim ds As New DataSet

        ' Content
        Dim dtContent As New DataTable
        dtContent.Columns.Add("A", GetType(String))
        Dim drContent As DataRow = dtContent.NewRow
        drContent("A") = String.Format("{0}: {1}", Me.GetGlobalResourceObject("Text", "SearchTime"), DateTime.Now.ToString("yyyy/MM/dd HH:mm"))
        dtContent.Rows.Add(drContent)

        'Criteria
        Dim dtCriteria As New DataTable
        dtCriteria.Columns.Add("A", GetType(String))
        dtCriteria.Columns.Add("B", GetType(String))
        drHeader = dtCriteria.NewRow
        drHeader("A") = Me.GetGlobalResourceObject("Text", "InspectionRecord")
        drHeader("B") = rdlOwner.SelectedItem.Text
        dtCriteria.Rows.Add(drHeader)
        drHeader = dtCriteria.NewRow
        drHeader("A") = Me.GetGlobalResourceObject("Text", "InspectionRecordID")
        drHeader("B") = If(txtInspectionID.Text = "", Me.GetGlobalResourceObject("Text", "Any"), udtGeneralFunction.getSeqNo_Prefix_Without_Update_ProfileNum("INSID", "ALL") + txtInspectionID.Text)
        dtCriteria.Rows.Add(drHeader)
        drHeader = dtCriteria.NewRow
        drHeader("A") = Me.GetGlobalResourceObject("Text", "FileReferenceNo") + joinParenthesis(udtGeneralFunction.getSystemParameter("Inspection_FileRefNo_Prefix"))
        drHeader("B") = If(txtFileReferenceNo.Text = "", Me.GetGlobalResourceObject("Text", "Any"), txtFileReferenceNo.Text)
        dtCriteria.Rows.Add(drHeader)
        drHeader = dtCriteria.NewRow
        drHeader("A") = Me.GetGlobalResourceObject("Text", "SPID")
        drHeader("B") = If(txtSPID.Text = "", Me.GetGlobalResourceObject("Text", "Any"), txtSPID.Text)
        dtCriteria.Rows.Add(drHeader)
        drHeader = dtCriteria.NewRow
        drHeader("A") = Me.GetGlobalResourceObject("Text", "VisitDate")
        drHeader("B") = If(txtStartVisitDate.Text = "", Me.GetGlobalResourceObject("Text", "Any"), txtStartVisitDate.Text) + " " + Me.GetGlobalResourceObject("Text", "To") + " " + If(txtEndVisitDate.Text = "", Me.GetGlobalResourceObject("Text", "Any"), txtEndVisitDate.Text)
        dtCriteria.Rows.Add(drHeader)
        drHeader = dtCriteria.NewRow
        drHeader("A") = Me.GetGlobalResourceObject("Text", "MainTypeOfInspection")
        drHeader("B") = If(ddlTypeofInspection.SelectedValue = "", Me.GetGlobalResourceObject("Text", "Any"), ddlTypeofInspection.SelectedItem.Text)
        dtCriteria.Rows.Add(drHeader)
        drHeader = dtCriteria.NewRow
        drHeader("A") = Me.GetGlobalResourceObject("Text", "Status")
        drHeader("B") = If(ddlStatus.SelectedValue = "", Me.GetGlobalResourceObject("Text", "Any"), ddlStatus.SelectedItem.Text)
        dtCriteria.Rows.Add(drHeader)

        ds.Tables.Add(dtContent)
        ds.Tables.Add(dtCriteria)
        ds.Tables.Add(dt)

        Dim strTemplateFolder As String = udtGeneralFunction.getSystemParameter("StatisticsTemplateFolder")
        Dim strFolderPath As String = udtGeneralFunction.getSystemParameter("ExcelWithTemplateDownloadStoragePath")

        Dim blnSuccess As Boolean = True
        Dim udtDB As New Database

        Try
            Dim udtFileGenerationBLL As New FileGenerationBLL
            Dim udtFileGeneration As FileGenerationModel = udtFileGenerationBLL.RetrieveFileGeneration(udtDB, DataDownloadFileID.INSP0001)

            Dim udtQueue As New FileGenerationQueueModel
            udtQueue.GenerationID = udtGeneralFunction.generateFileSeqNo
            udtQueue.FileID = DataDownloadFileID.INSP0001
            udtQueue.InParm = String.Empty
            udtQueue.OutputFile = udtFileGeneration.FileNameWithDateTimeStamp
            udtQueue.Status = DataDownloadStatus.Pending
            udtQueue.FilePassword = String.Empty
            udtQueue.RequestDtm = DateTime.Now
            udtQueue.RequestBy = (New HCVUUserBLL).GetHCVUUser.UserID
            udtQueue.FileDescription = udtFileGeneration.FileDesc + " - " + udtQueue.OutputFile
            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            udtQueue.ScheduleGenDtm = Nothing
            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]

            hfEGenerationID.Value = udtQueue.GenerationID

            udtAuditLog.AddDescripton("Generation ID", udtQueue.GenerationID)
            udtAuditLog.AddDescripton("File Name", udtQueue.OutputFile)

            'Generate output file
            If blnSuccess Then
                blnSuccess = udtFileGenerationBLL.ConstructExcelFile(ds, strFolderPath, udtQueue.OutputFile, udtQueue.FilePassword, strTemplateFolder + udtFileGeneration.ReportTemplate, udtFileGeneration.XLS_Parameter)
            End If

            udtDB.BeginTransaction()

            'Add record to table FileGenerationQueue
            If blnSuccess Then
                blnSuccess = udtFileGenerationBLL.AddFileGenerationQueue(udtDB, udtQueue)
            End If

            'Update record in table FileGenerationQueue
            If blnSuccess Then
                blnSuccess = udtFileGenerationBLL.UpdateFileGenerationQueueStart(udtDB, udtQueue.GenerationID)
            End If

            ' Save output file Into File Database
            If blnSuccess Then
                udtQueue.FileContent = File.ReadAllBytes(strFolderPath + udtQueue.OutputFile)
                blnSuccess = udtFileGenerationBLL.UpdateFileContent(udtDB, udtQueue.GenerationID, udtQueue.FileContent)
            End If

            'Add record to table FileDownloads
            If blnSuccess Then
                blnSuccess = udtFileGenerationBLL.AddFileDownload(udtDB, udtQueue.GenerationID, udtQueue.RequestBy)
            End If

            'Update record in table FileGenerationQueue
            If blnSuccess Then
                blnSuccess = udtFileGenerationBLL.UpdateFileGenerationQueueStatus(udtDB, udtQueue.GenerationID, FileGenerationQueueStatus.Completed)
            End If

            'Show popup for File Download redirection
            If blnSuccess Then
                udtDB.CommitTransaction()
            Else
                udtDB.RollBackTranscation()
            End If

        Catch ex As Exception
            udtDB.RollBackTranscation()
            udtAuditLog.AddDescripton("Exception", ex.Message)
            udtAuditLog.WriteEndLog(LogID.LOG00085, AuditLogDesc.IRM_Export_Excel_Report_Fail)
            Throw
        Finally
            Call (New FileGenerationBLL).ClearTempFolder(strFolderPath, 15)
        End Try
        mpeExportReport.Show()
        udtAuditLog.WriteEndLog(LogID.LOG00084, AuditLogDesc.IRM_Export_Excel_Report_Successful)
    End Sub
#End Region
#Region "       2.1 New Inspection Record - Detail"
    'Init
    Protected Sub clearNewInspectionForm()
        ClearTargetVisitByMode(PageMode.ModeNew)
        txtVisitDate.Text = ""
        txtStartVisitTime.Text = ""
        txtEndVisitTime.Text = ""
        txtConfirmationWith.Text = ""
        txtConfirmDate.Text = ""
        ddlFormCondition.SelectedIndex = 0
        pnlFormConditionRemarks.Visible = False
        txtFormConditionRm.Text = ""
        ddlMeansofCommunication.Text = ""
        txtMeansofCommunicationFax.Text = ""
        txtMeansofCommunicationEmail.Text = ""
        rdoAddLowRiskClaim.ClearSelection()
        txtRemarks.Text = ""
        hfCaseOfficer.Value = ""
        txtCaseOfficer.Text = ""
        txtCaseContactNo.Text = ""
        txtSubjectContactNo.Text = ""
        hfSubjectOfficer.Value = ""
        txtSubjectOfficer.Text = ""

        rdoListAddMainTypeofInspection.ClearSelection()

        txtFRN1.Text = ""
        txtFRN2.Text = ""
        txtFRN3.Text = ""
        txtFRN4.Text = ""
        txtFRN5.Text = ""

        txtRRNA1.Text = ""
        txtRRNA2.Text = ""
        txtRRNA3.Text = ""
        txtRRNA4.Text = ""
        txtRRNA5.Text = ""

        txtRRNB1.Text = ""
        txtRRNB2.Text = ""
        txtRRNB3.Text = ""
        txtRRNB4.Text = ""
        txtRRNB5.Text = ""

        txtRRNC1.Text = ""
        txtRRNC2.Text = ""
        txtRRNC3.Text = ""
        txtRRNC4.Text = ""
        txtRRNC5.Text = ""
    End Sub
    'Visit Target Function
    Protected Sub ibtnSearchVisitTarget_Click(sender As Object, e As ImageClickEventArgs)
        Me.udcMsgBox.Visible = False
        Me.udcInfoMsgBox.Visible = False
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        Dim objAuditLogInfo As AuditLogInfo = New AuditLogInfo(Me.txtSPIDNew.Text.Trim, Nothing, Nothing, Nothing, Nothing, Nothing)
        imgtxtSPIDNewErr.Visible = False

        udtAuditLogEntry.AddDescripton("SP ID", Me.txtSPIDNew.Text.Trim)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00002, AuditLogDesc.IRM_SearchSP)

        Dim strCode = txtSPIDNew.Text.Trim
        Dim dtResult As New DataTable()
        'Init Practice
        initDropDownList(ddlPractice)
        If Me.txtSPIDNew.Text.Trim = "" Then
            imgtxtSPIDNewErr.Visible = True
            udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00316)
            udcMsgBox.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00004, AuditLogDesc.IRM_SearchSP_Fail)
            Return
        End If

        Dim sm As SystemMessage = Me.udtValidator.chkSPID(Me.txtSPIDNew.Text.Trim)
        If IsNothing(sm) Then
            If Me.txtSPIDNew.Text.Trim.Length = 8 Then
                Dim udtAccountChangeMaintBLL As New AccountChangeMaintenance.AccountChangeMaintenanceBLL
                If Not GetReadyServiceProvider(Me.txtSPIDNew.Text.Trim, PageMode.ModeNew) Then
                    ClearVisitTargetForm(PageMode.ModeNew)
                    ' No Record Found
                    udcMsgBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001, "{SPID}", Me.txtSPIDNew.Text.Trim)
                    imgtxtSPIDNewErr.Visible = True
                    udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00004, AuditLogDesc.IRM_SearchSP_Fail, objAuditLogInfo)
                Else
                    setAfterSearchTarget(PageMode.ModeNew)
                End If
            End If
        Else
            udtAuditLogEntry.AddDescripton("SP ID", Me.txtSPIDNew.Text.Trim)
            udtAuditLogEntry.AddDescripton("No of record", "-")
            imgtxtSPIDNewErr.Visible = True
            Me.udcMsgBox.AddMessage(sm)
            Me.udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00004, AuditLogDesc.IRM_SearchSP_Fail, objAuditLogInfo)
        End If
    End Sub
    Protected Sub ibtnClear_Click(sender As Object, e As ImageClickEventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00013, AuditLogDesc.IRM_SP_Clear_Button_Click)

        ClearTargetVisitByMode(PageMode.ModeNew)
    End Sub

    'Submit Function -> 2.2 New Inspection Record - Confirm
    Protected Sub ibtnNewInspectionSubmit_Click(sender As Object, e As ImageClickEventArgs)
        HideErrorImg()
        Dim blnError As Boolean = False

        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        Me.udtAuditLogEntry.AddDescripton("Main Type of Inspection", Me.rdoListAddMainTypeofInspection.SelectedValue)
        Me.udtAuditLogEntry.AddDescripton("Other Type of Inspection", udtInspectionRecordBLL.GetTypeofInspectionStringFromInput(Me.chkListAddTypeofInspection))
        Me.udtAuditLogEntry.AddDescripton("File Ref Type", Me.rdoFileReferenceType.SelectedValue)
        Me.udtAuditLogEntry.AddDescripton("File Ref. No.", String.Format("(P1:{0}, P2:{1}, P3:{2}, P4:{3}, P5:{4})", Me.txtFRN1.Text, txtFRN2.Text, txtFRN3.Text, txtFRN4.Text, txtFRN5.Text))
        Me.udtAuditLogEntry.AddDescripton("Referred File Ref. No. (1)", String.Format("(P1:{0}, P2:{1}, P3:{2}, P4:{3}, P5:{4})", Me.txtRRNA1.Text, txtRRNA2.Text, txtRRNA3.Text, txtRRNA4.Text, txtRRNA5.Text))
        Me.udtAuditLogEntry.AddDescripton("Referred File Ref. No. (2)", String.Format("(P1:{0}, P2:{1}, P3:{2}, P4:{3}, P5:{4})", Me.txtRRNB1.Text, txtRRNB2.Text, txtRRNB3.Text, txtRRNB4.Text, txtRRNB5.Text))
        Me.udtAuditLogEntry.AddDescripton("Referred File Ref. No. (3)", String.Format("(P1:{0}, P2:{1}, P3:{2}, P4:{3}, P5:{4})", Me.txtRRNC1.Text, txtRRNC2.Text, txtRRNC3.Text, txtRRNC4.Text, txtRRNC5.Text))
        Me.udtAuditLogEntry.AddDescripton("Case Officer", Me.hfCaseOfficer.Value)
        Me.udtAuditLogEntry.AddDescripton("Case Officer Contact No.", Me.txtCaseContactNo.Text)
        Me.udtAuditLogEntry.AddDescripton("Subject Officer", Me.hfSubjectOfficer.Value)
        Me.udtAuditLogEntry.AddDescripton("Subject Officer Contact No.", Me.txtSubjectContactNo.Text)
        Me.udtAuditLogEntry.AddDescripton("SP ID", Me.txtSPIDNew.Text)
        Me.udtAuditLogEntry.AddDescripton("Practice", Me.ddlPractice.SelectedValue)
        Me.udtAuditLogEntry.AddDescripton("Visit Date", Me.txtVisitDate.Text)
        Me.udtAuditLogEntry.AddDescripton("Visit Time From", Me.txtStartVisitTime.Text)
        Me.udtAuditLogEntry.AddDescripton("Visit Time To", Me.txtEndVisitTime.Text)
        Me.udtAuditLogEntry.AddDescripton("Confirmation With", Me.txtConfirmationWith.Text)
        Me.udtAuditLogEntry.AddDescripton("Confirm Date", Me.txtConfirmDate.Text)
        Me.udtAuditLogEntry.AddDescripton("Form Condition", Me.ddlFormCondition.SelectedValue)
        Me.udtAuditLogEntry.AddDescripton("Form Condition Remarks", Me.txtFormConditionRm.Text)
        Me.udtAuditLogEntry.AddDescripton("Means of Communication", Me.ddlMeansofCommunication.SelectedValue)
        Me.udtAuditLogEntry.AddDescripton("Means of Communication Fax", Me.txtMeansofCommunicationFax.Text)
        Me.udtAuditLogEntry.AddDescripton("Means of Communication Email", Me.txtMeansofCommunicationEmail.Text)
        Me.udtAuditLogEntry.AddDescripton("Low Risk Claim", Me.rdoAddLowRiskClaim.SelectedValue)
        Me.udtAuditLogEntry.AddDescripton("Remarks", Me.txtRemarks.Text)

        udtAuditLogEntry.WriteStartLog(LogID.LOG00029, AuditLogDesc.IRM_NewInspectionSubmit_Click)

        Dim dataField As DetailDataField = GetDetailDataFieldByMode(PageMode.ModeNew)

        Dim targetName = Me.GetGlobalResourceObject("Text", "FileReferenceNo")
        Dim fileNoValid As Boolean = True
        Select Case rdoFileReferenceType.SelectedValue
            Case FileReferenceType.NewFile
                If txtFRN1.Text.Trim = "" And txtFRN2.Text.Trim = "" And txtFRN3.Text.Trim = "" Then
                    udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00028, "%s", targetName)
                    blnError = True
                    fileNoValid = False
                Else
                    'Partial reference no. is empty
                    If txtFRN1.Text.Trim = "" Or txtFRN2.Text.Trim = "" Or txtFRN3.Text.Trim = "" Then
                        udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00364, "%s", targetName)
                        blnError = True
                        fileNoValid = False
                    Else
                        'Check Value for Main Type of Inspection
                        If Not IsNumeric(txtFRN1.Text) Then
                            udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00365, "%s", " Part 1 of " + targetName)
                            blnError = True
                            fileNoValid = False
                        Else
                            Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL
                            If Not String.IsNullOrEmpty(rdoListAddMainTypeofInspection.SelectedValue.Trim) Then
                                Dim seletedItem As StaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("TypeOfInspection", rdoListAddMainTypeofInspection.SelectedValue)
                                If txtFRN1.Text <> seletedItem.DisplayOrder(0) Then
                                    udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00365, "%s", " Part 1 of " + targetName)
                                    blnError = True
                                    fileNoValid = False
                                End If
                            End If
                        End If
                        'Check Year
                        If txtFRN2.Text.Length <> 2 Or Not IsNumeric(txtFRN2.Text) Then
                            udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00365, "%s", " Part 2 of " + targetName)
                            blnError = True
                            fileNoValid = False
                        End If
                        'Check Month
                        If txtFRN3.Text.Length <> 2 Or Not IsNumeric(txtFRN3.Text) Or CInt(txtFRN3.Text) < 1 Or CInt(txtFRN3.Text) > 12 Then
                            udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00365, "%s", " Part 3 of " + targetName)
                            blnError = True
                            fileNoValid = False
                        End If

                        'Check Date After 2020-09
                        If fileNoValid Then
                            ' Over Date invalid
                            Dim dateFileNo = CDate("20" + txtFRN2.Text + "-" + txtFRN3.Text + "-01"), dateMinMonth As Date = CDate(udtGeneralFunction.getSystemParameter("Inspection_NewType_MinMonth") + "-01")
                            If dateFileNo < dateMinMonth Then
                                Dim strTitle As String = String.Format("The Year and Month of {0}", Me.GetGlobalResourceObject("Text", "FileReferenceNo"))
                                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00381, New String() {"%s", "%t"}, New String() {strTitle, dateMinMonth.ToString("MMM yyyy")})
                                blnError = True
                                fileNoValid = False
                            End If
                        End If
                    End If
                End If
            Case FileReferenceType.Existing
                'Whole reference no. is empty
                If String.IsNullOrEmpty(txtFRN5.Text) Then
                    udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00364, "%s", targetName)
                    blnError = True
                    fileNoValid = False
                Else
                    txtFRN5.Text = txtFRN5.Text.ToUpper
                    fileNoValid = FileReferenceNoValidation(targetName, txtFRN1.Text, txtFRN2.Text, txtFRN3.Text, txtFRN4.Text, txtFRN5.Text, imgAddFRNErr, blnError, True)
                    If fileNoValid Then
                        Dim strFileNo = udtInspectionRecordBLL.HandleFileRefNo(txtFRN1.Text, txtFRN2.Text, txtFRN3.Text, txtFRN4.Text, txtFRN5.Text)
                        Dim record As DataTable = udtInspectionRecordBLL.SearchInspectionRecordByAny(New GetInspectionParameter With {
                                                                                                       .FileReferenceNo = udtInspectionRecordBLL.HandleFileRefNo(txtFRN1.Text, txtFRN2.Text, txtFRN3.Text, txtFRN4.Text, txtFRN5.Text),
                                                                                                       .OnlyForOwner = 0,
                                                                                                       .UserId = udtHCVUUserBLL.GetHCVUUser.UserID
                                                                                                       })
                        If record.Rows.Count > 0 Then
                            'Duplicate
                            udcMsgBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00007, "{FileRefNo}", strFileNo)
                            blnError = True
                            fileNoValid = False
                        Else
                            strFileNo = udtInspectionRecordBLL.HandleFileRefNo(txtFRN1.Text, txtFRN2.Text, txtFRN3.Text, txtFRN4.Text, "")
                            Dim strSPID As String = txtSPIDNew.Text.Trim
                            record = udtInspectionRecordBLL.SearchInspectionRecordByAny(New GetInspectionParameter With {
                                                                                                     .FileReferenceNo = strFileNo,
                                                                                                     .OnlyForOwner = 0,
                                                                                                     .UserId = udtHCVUUserBLL.GetHCVUUser.UserID
                                                                                                     })
                            'Filter File Reference No
                            Dim recordDv As DataView = record.DefaultView
                            recordDv.RowFilter = "File_Reference_No ='" + strFileNo + "'"
                            record = recordDv.ToTable

                            If record.Rows.Count = 0 Then
                                'Not Exist
                                udcMsgBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00006, "{FileRefNo}", strFileNo)
                                blnError = True
                                fileNoValid = False
                            Else
                                'Filter SP_ID
                                recordDv = record.DefaultView
                                recordDv.RowFilter = "SP_ID ='" + strSPID + "'"
                                record = recordDv.ToTable

                                'SP ID Not Match
                                If record.Rows.Count = 0 Then
                                    udcMsgBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00010, "{FileRefNo}", strFileNo)
                                    blnError = True
                                    fileNoValid = False
                                    imgtxtSPIDNewErr.Visible = True
                                End If
                            End If
                        End If
                    End If
                End If

            Case FileReferenceType.History
                'Whole reference no. is empty
                fileNoValid = FileReferenceNoValidation(targetName, txtFRN1.Text, txtFRN2.Text, txtFRN3.Text, txtFRN4.Text, "", imgAddFRNErr, blnError, True)
                If fileNoValid Then

                    Dim dateFileNo = CDate("20" + txtFRN2.Text + "-" + txtFRN3.Text + "-01"), dateMinMonth As Date = CDate(udtGeneralFunction.getSystemParameter("Inspection_NewType_MinMonth") + "-01")
                    If dateFileNo >= dateMinMonth Then
                        Dim strTitle As String = String.Format("The Year and Month of {0}", Me.GetGlobalResourceObject("Text", "FileReferenceNo"))
                        udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00382, New String() {"%s", "%t"}, New String() {strTitle, dateMinMonth.ToString("MMM yyyy")})
                        blnError = True
                        fileNoValid = False
                    Else
                        Dim strFileNo = udtInspectionRecordBLL.HandleFileRefNo(txtFRN1.Text, txtFRN2.Text, txtFRN3.Text, txtFRN4.Text, "")
                        'Check Duplicate
                        Dim record As DataTable = udtInspectionRecordBLL.SearchInspectionRecordByAny(New GetInspectionParameter With {
                                                                                                     .FileReferenceNo = strFileNo,
                                                                                                     .OnlyForOwner = 0,
                                                                                                     .UserId = udtHCVUUserBLL.GetHCVUUser.UserID
                                                                                                     })
                        If record.Rows.Count > 0 Then
                            'Duplicate
                            udcMsgBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00007, "{FileRefNo}", strFileNo)
                            fileNoValid = False
                            blnError = True
                        End If
                    End If
                End If
        End Select
        imgAddFRNErr.Visible = Not fileNoValid
        'Check if Routine (Follow-up) is selected
        Dim isFollowUpInspectionTypeselected As Boolean = rdoListAddMainTypeofInspection.SelectedValue.Trim = TypeOfInspection.RoutineFollowUp

        Dim strReferredFileRefNoText As String = Me.GetGlobalResourceObject("Text", "ReferredFileReferenceNo")

        HandleReferredFileReferenceNoList(dataField)

        Dim strReferNoA As String = udtInspectionRecordBLL.HandleFileRefNo(txtRRNA1.Text, txtRRNA2.Text, txtRRNA3.Text, txtRRNA4.Text, txtRRNA5.Text),
     strReferNoB As String = udtInspectionRecordBLL.HandleFileRefNo(txtRRNB1.Text, txtRRNB2.Text, txtRRNB3.Text, txtRRNB4.Text, txtRRNB5.Text),
     strReferNoC As String = udtInspectionRecordBLL.HandleFileRefNo(txtRRNC1.Text, txtRRNC2.Text, txtRRNC3.Text, txtRRNC4.Text, txtRRNC5.Text)

        'Referred File RefNo 1
        FileReferenceNoValidation(strReferredFileRefNoText + "(1)", txtRRNA1.Text, txtRRNA2.Text, txtRRNA3.Text, txtRRNA4.Text, txtRRNA5.Text, imgAddRRNAErr, blnError, isFollowUpInspectionTypeselected)
        'Referred File RefNo 2
        FileReferenceNoValidation(strReferredFileRefNoText + "(2)", txtRRNB1.Text, txtRRNB2.Text, txtRRNB3.Text, txtRRNB4.Text, txtRRNB5.Text, imgAddRRNBErr, blnError)
        'Referred File RefNo 3
        FileReferenceNoValidation(strReferredFileRefNoText + "(3)", txtRRNC1.Text, txtRRNC2.Text, txtRRNC3.Text, txtRRNC4.Text, txtRRNC5.Text, imgAddRRNCErr, blnError)

        ' Refer No A is not empty and A = B
        Dim AequalB As Boolean = Not String.IsNullOrEmpty(strReferNoA) And strReferNoA = strReferNoB
        ' Refer No C is not empty and A = C
        Dim AequalC As Boolean = Not String.IsNullOrEmpty(strReferNoC) And strReferNoA = strReferNoC
        ' Refer No B is not empty and B = C
        Dim BequalC As Boolean = Not String.IsNullOrEmpty(strReferNoB) And strReferNoB = strReferNoC

        imgAddRRNAErr.Visible = IIf(AequalB Or AequalC, True, imgAddRRNAErr.Visible)
        imgAddRRNBErr.Visible = IIf(AequalB Or BequalC, True, imgAddRRNBErr.Visible)
        imgAddRRNCErr.Visible = IIf(AequalC Or BequalC, True, imgAddRRNCErr.Visible)

        If AequalB Or AequalC Or BequalC Then
            udcMsgBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00009, "%s", IIf(AequalB Or AequalC, strReferNoA, strReferNoB))
            blnError = True
        End If

        'check Main Type Of Inspection
        If rdoListAddMainTypeofInspection.SelectedValue.Trim = "" Then
            udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00367, "%s", GetGlobalResourceObject("Text", "MainTypeOfInspection"))
            imgrdoListAddMainTypeofInspectionErr.Visible = True
            blnError = True
        Else
            For i As Integer = 0 To chkListAddTypeofInspection.Items.Count - 1
                If chkListAddTypeofInspection.Items(i).Selected Then
                    If chkListAddTypeofInspection.Items(i).Value = "RF" Then
                        isFollowUpInspectionTypeselected = True
                    End If
                End If
            Next
            For Each Item As ListItem In chkListAddTypeofInspection.Items
                If Item.Selected Then
                    If Item.Value = rdoListAddMainTypeofInspection.SelectedValue Then
                        udcMsgBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00011)
                        imgchkListAddTypeofInspectionErr.Visible = True
                        blnError = True
                        Exit For
                    End If
                End If
            Next
            If rdoListAddMainTypeofInspection.SelectedValue = TypeOfInspection.RoutineNew Then
                'Not allow to input Referred File Ref. No
                If Not String.IsNullOrEmpty(strReferNoA) Or Not String.IsNullOrEmpty(strReferNoB) Or Not String.IsNullOrEmpty(strReferNoC) Then
                    udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00367, "%s", GetGlobalResourceObject("Text", "MainTypeOfInspection"))
                    imgAddFRNErr.Visible = True
                    blnError = True
                End If
            ElseIf rdoListAddMainTypeofInspection.SelectedValue = TypeOfInspection.Routine Then
                'Not allow to input Other type of inspection
                If chkListAddTypeofInspection.SelectedIndex <> -1 Then
                    udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00367, "%s", GetGlobalResourceObject("Text", "MainTypeOfInspection"))
                    imgchkListAddTypeofInspectionErr.Visible = True
                    blnError = True
                End If
            End If
        End If
        'check Visit Target
        Dim noTarget As Boolean = txtSPIDNew.Enabled
        If noTarget Then
            txtSPIDNew.Text = ""
            udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00316)
            blnError = True
            imgtxtSPIDNewErr.Visible = True
        Else
            If ddlPractice.SelectedValue = "" Then
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00367, "%s", GetGlobalResourceObject("Text", "Practice"))
                imgddlPracticeErr.Visible = True
                blnError = True
            End If
        End If


        'add by golden 644
        Dim blnNeedValidation As Boolean = True
        If hdnStatus.Value = InspectionStatus.Creating Or hdnStatus.Value = InspectionStatus.Incomplete Or hdnStatus.Value = InspectionStatus.PendingForSiteVisit Then
            blnNeedValidation = False
        End If

        Dim blnInComplete As Boolean = False

        CheckDetailValidation(blnNeedValidation, blnError, blnInComplete, PageMode.ModeNew)

        If hdnStatus.Value = InspectionStatus.Creating Or hdnStatus.Value = InspectionStatus.Incomplete Or hdnStatus.Value = InspectionStatus.PendingForSiteVisit Then
            If blnInComplete Then
                hdnStatus.Value = InspectionStatus.Incomplete
            Else
                hdnStatus.Value = InspectionStatus.PendingForSiteVisit
            End If
        End If

        If blnError Then
            Me.udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00031, AuditLogDesc.IRM_NewInspectionSubmit_Fail)
            Return
        Else
            udcMsgBox.Visible = False
            udcInfoMsgBox.Visible = False
        End If

        Dim newInspectionRecord As InspectionRecordModel = FillNewInspectionDataModel()

        Session(SESS_InspectionRecordModelNew) = newInspectionRecord

        Dim newInspectionRecordViewModel As InspectionRecordViewModel = FillNewInspectionViewModel(newInspectionRecord)

        'Visit Date Past Date Checking
        If (newInspectionRecord.VisitDate <> Date.MinValue And newInspectionRecord.VisitDate < Date.Today) Then
            newInspectionRecordViewModel.VisitDateFormat += " <font color=""red"">" + Me.GetGlobalResourceObject("Text", "PastDate") + "</font>"
        End If

        With newInspectionRecordViewModel
            .CaseOfficer = txtCaseOfficer.Text
            .SubjectOfficer = txtSubjectOfficer.Text
        End With
        If (newInspectionRecord.FileReferenceType = FileReferenceType.NewFile) Then
            newInspectionRecordViewModel.FileReferenceNo += "-" + lblFileSeqNoAlert.Text
        End If

        FillSnapshotFieldToViewModel(newInspectionRecordViewModel)

        FillDataNewConfrimDetailByModel(newInspectionRecordViewModel)
        'vInspectionConfirm
        MultiViewIRM.SetActiveView(vInspectionConfirm)
        udtSM = New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001)
        Me.udcInfoMsgBox.AddMessage(udtSM)

        Me.udcInfoMsgBox.BuildMessageBox()
        udtAuditLogEntry.WriteEndLog(LogID.LOG00030, AuditLogDesc.IRM_NewInspectionSubmit_Successful)

    End Sub
    Private Function FillNewInspectionDataModel() As InspectionRecordModel
        Dim prefixFileRefNo As String = udtGeneralFunction.getSystemParameter("Inspection_FileRefNo_Prefix")
        Dim fileRefType As String = rdoFileReferenceType.SelectedValue
        Dim fileRefNo As String = prefixFileRefNo + txtFRN1.Text + "/" + txtFRN2.Text + "-" + txtFRN3.Text

        Select Case fileRefType
            Case FileReferenceType.NewFile

            Case FileReferenceType.Existing
                fileRefNo += String.Format("-{0}-({1})", txtFRN4.Text, txtFRN5.Text)

            Case FileReferenceType.History
                fileRefNo += "-" + txtFRN4.Text
        End Select

        Dim inspectionRecord As InspectionRecordModel = CollectDataFromFieldByMode(PageMode.ModeNew)
        inspectionRecord.FileReferenceNo = fileRefNo

        inspectionRecord.MainTypeOfInspectionID = rdoListAddMainTypeofInspection.SelectedValue
        inspectionRecord.MainTypeOfInspectionValue = rdoListAddMainTypeofInspection.SelectedItem.Text

        Return inspectionRecord
    End Function
    Private Function FillNewInspectionViewModel(model As InspectionRecordModel) As InspectionRecordViewModel

        Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL
        Dim typeOfInspectionFormat As String = ""
        If (Not String.IsNullOrEmpty(model.OtherTypeOfInspectionID)) Then
            For Each Item As String In model.OtherTypeOfInspectionID.Split(",")
                Dim selectItem As StaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("TypeOfInspection", Item)
                typeOfInspectionFormat += IIf(typeOfInspectionFormat = "", selectItem.DataValue, "," + selectItem.DataValue)
            Next
        End If

        Dim visitTimeFormat As String = ""
        If (model.VisitBeginDtm <> Date.MinValue And model.VisitEndDtm <> Date.MinValue) Then
            visitTimeFormat = udtInspectionRecordBLL.GetTimeFromDate(model.VisitBeginDtm) + " - " + udtInspectionRecordBLL.GetTimeFromDate(model.VisitEndDtm)
        End If

        Dim formConditionFormat As String = ""
        If Not String.IsNullOrEmpty(model.FormConditionID) Then
            Dim dt As StaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("FormCondition", model.FormConditionID)
            Dim formConditionValue As String = dt.DataValue
            If (model.FormConditionID = FormCondition.Others) Then
                formConditionFormat = formConditionValue + joinParenthesis(model.FormConditionRemark)
            Else
                formConditionFormat = formConditionValue
            End If
        End If

        Dim meansOfCommunicationValue As String = "", meansOfCommunicationContact As String = ""
        If (Not String.IsNullOrEmpty(model.MeansOfCommunicationID)) Then
            Dim dt As StaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("MeansOfCommunication", model.MeansOfCommunicationID)
            meansOfCommunicationValue = dt.DataValue
        End If
        If Not String.IsNullOrEmpty(model.MeansOfCommunicationEmail) Or Not String.IsNullOrEmpty(model.MeansOfCommunicationFax) Then
            meansOfCommunicationContact = joinParenthesis(Me.GetGlobalResourceObject("Text", "FaxNo") + ": " + IIf(Not String.IsNullOrEmpty(model.MeansOfCommunicationFax), model.MeansOfCommunicationFax, Me.GetGlobalResourceObject("Text", "Empty")) + " / " +
                                                          Me.GetGlobalResourceObject("Text", "Email") + ": " + IIf(Not String.IsNullOrEmpty(model.MeansOfCommunicationEmail), model.MeansOfCommunicationEmail, Me.GetGlobalResourceObject("Text", "Empty")))
        End If
        Dim fileReferenceValue As String = ""
        If (Not String.IsNullOrEmpty(model.FileReferenceType)) Then
            Dim dt As StaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("InspectionFileType", model.FileReferenceType)
            fileReferenceValue = dt.DataValue
        End If
        Dim strLowRiskCliam As String = ""
        If Not String.IsNullOrEmpty(model.LowRiskClaim) Then
            strLowRiskCliam = IIf(model.LowRiskClaim = "Y", "Yes", "No")
        End If
        Dim viewModel As New InspectionRecordViewModel
        With viewModel
            .FileReferenceType = fileReferenceValue
            .FileReferenceNo = model.FileReferenceNo
            .TypeOfInspection = typeOfInspectionFormat
            .MainTypeOfInspection = model.MainTypeOfInspectionValue
            .ReferredReferenceNo1 = model.ReferredReferenceNo1
            .ReferredReferenceNo2 = model.ReferredReferenceNo2
            .ReferredReferenceNo3 = model.ReferredReferenceNo3
            .SPID = model.SPID
            .VisitDateFormat = udtInspectionRecordBLL.FormatOutputDate(model.VisitDate)
            .VisitTimeFormat = visitTimeFormat
            .ConfirmationWith = model.ConfirmationWith
            .ConfirmationDtmFormat = udtInspectionRecordBLL.FormatOutputDate(model.ConfirmationDtm)
            .FormCondition = formConditionFormat
            .MeansOfCommunication = meansOfCommunicationValue
            .MeansOfCommunicationContact = meansOfCommunicationContact
            .LowRiskClaim = strLowRiskCliam
            .Remarks = model.Remarks
            .CaseOfficer = model.CaseOfficerValue
            .CaseOfficerContactNo = model.CaseOfficerContactNo
            .SubjectOfficer = model.SubjectOfficerValue
            .SubjectOfficerContactNo = model.SubjectOfficerContactNo
        End With
        Return viewModel
    End Function
    Private Sub FillSnapshotFieldToViewModel(ByRef model As InspectionRecordViewModel)
        With model
            .SPStatus = lblSPStatus.Text.Trim
            .SPName = lblServiceProviderName.Text.Trim
            .HealthProfession = lblHealthProfession.Text.Trim
            .SPTelNo = lblSPTelNo.Text.Trim
            .SPFaxNo = lblSPFaxNo.Text.Trim
            .SPEmail = lblSPEmail.Text.Trim
            .HCVSEffectiveDtm = lblHCVSEffectiveDate.Text
            .HCVSDHCEffectiveDtm = lblHCVSDHCEffectiveDate.Text
            .HCVSCHNEffectiveDtm = lblHCVSCHNEffectiveDate.Text
            .PracticeName = lblPractice.Text.Trim
            .PracticeStatus = lblPracticeStatus.Text.Trim
            .PracticeNameChi = lblPractice_Ci.Text.Trim
            .PracticeAddress = lblPracticeAddress.Text.Trim
            .PracticeAddressChi = lblPracticeAddress_Ci.Text.Trim
            .PracticeDisplaySeq = hdfPracticeSeq.Value
            .PracticePhoneDaytime = lblPracticePhoneDaytime.Text.Trim
            .LastVisitDateFormat = lblLastVisitDate.Text.Trim
        End With
    End Sub
    'Back Function -> 1. Search Page
    Protected Sub ibtnSubmitBack_Click(sender As Object, e As ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00018, AuditLogDesc.IRM_NewInspectionRecordBack_Button_Click)

        MultiViewIRM.SetActiveView(vSEARCH)
        Me.udcMsgBox.Visible = False
        Me.udcInfoMsgBox.Visible = False
    End Sub
    'Selected Value Change Function
    Protected Sub rdoFileReferenceType_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim rdoFileRefType As RadioButtonList = sender
        imgAddFRNErr.Visible = False
        txtFRN5.Text = ""
        Select Case rdoFileRefType.SelectedValue.Trim
            Case FileReferenceType.NewFile
                txtFRN4.Text = ""
                txtFRN4.Visible = False
                txtFRN5.Visible = False
                lblFilePartNoLeft.Visible = False
                lblFilePratNoRight.Visible = False
                lblFileSeqNoAlert.Visible = True
            Case FileReferenceType.Existing
                txtFRN4.Visible = True
                txtFRN5.Visible = True
                lblFilePartNoLeft.Visible = True
                lblFilePratNoRight.Visible = True
                lblFileSeqNoAlert.Visible = False
            Case FileReferenceType.History
                txtFRN4.Visible = True
                txtFRN5.Visible = False
                lblFilePartNoLeft.Visible = False
                lblFilePratNoRight.Visible = False
                lblFileSeqNoAlert.Visible = False
        End Select
    End Sub
    Protected Sub rdoListAddMainTypeofInspection_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim rdoListMainTypeOfInspection As RadioButtonList = sender

        'Handle Main Type of Inspection Selected Value
        udtInspectionRecordBLL.HandleMainTypeOfInspectionSelectedValue(rdoListMainTypeOfInspection.SelectedValue.Trim, GetDetailDataFieldByMode(PageMode.ModeNew))

        If rdoFileReferenceType.SelectedValue = FileReferenceType.NewFile And Not String.IsNullOrEmpty(rdoListMainTypeOfInspection.SelectedValue.Trim) Then
            Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL
            Dim seletedItem As StaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("TypeOfInspection", rdoListMainTypeOfInspection.SelectedValue.Trim)
            txtFRN1.Text = seletedItem.DisplayOrder(0)
        End If
    End Sub
    Protected Sub ddlPractice_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As DropDownList = sender
        Dim val = ddl.SelectedValue
        Dim udtFormatter As New Common.Format.Formatter

        hdfPracticeSeq.Value = val
        If val.Trim.Equals(String.Empty) Then
            ClearVisitTargetFormSub(PageMode.ModeNew)
            ddlPractice.Visible = True
            lblPractice.Visible = False
            Me.lblPracticeStatus.Text = String.Empty
            Me.lblPracticeStatus.Visible = False
        Else
            imgddlPracticeErr.Visible = False

            Dim udtPracticeDisplayList As Practice.PracticeBLL.PracticeDisplayModelCollection
            Dim udtPracticeDisplay As Practice.PracticeBLL.PracticeDisplayModel

            udtPracticeDisplayList = Me.udtSessionHandlerBLL.PracticeDisplayListGetFromSession(FunctionCode)
            udtPracticeDisplay = udtPracticeDisplayList.Filter(CInt(val))

            If udtPracticeDisplay.PracticeStatus.Trim.Equals(PracticeStatus.Active) Then
                Me.lblPracticeStatus.Text = String.Empty
                Me.lblPracticeStatus.Visible = False
            Else
                Status.GetDescriptionFromDBCode(PracticeStatus.ClassCode, udtPracticeDisplay.PracticeStatus, Me.lblPracticeStatus.Text, String.Empty)
                Me.lblPracticeStatus.Text = " (" + Me.lblPracticeStatus.Text + ")"
                Me.lblPracticeStatus.Visible = True
            End If

            lblPractice.Text = udtPracticeDisplay.PracticeName
            lblPractice_Ci.Text = udtFormatter.formatChineseName(udtPracticeDisplay.PracticeNameChi)

            Dim udtAddress As AddressModel = New AddressModel(udtPracticeDisplay.Room, udtPracticeDisplay.Floor, udtPracticeDisplay.Block, udtPracticeDisplay.Building, udtPracticeDisplay.BuildingChi, udtPracticeDisplay.District, Nothing)
            lblPracticeAddress.Text = udtFormatter.formatAddress(udtAddress)
            lblPracticeAddress_Ci.Text = udtFormatter.formatChineseName(udtFormatter.formatAddressChi(udtAddress))

            Dim strProfession = If(udtPracticeDisplay.Profession Is Nothing, Me.GetGlobalResourceObject("Text", "Empty"), udtPracticeDisplay.Profession.ServiceCategoryDesc)
            lblHealthProfession.Text = strProfession + joinParenthesis(udtPracticeDisplay.RegistrationCode)
            lblPracticePhoneDaytime.Text = udtPracticeDisplay.PhoneDaytime

            lblPractice_Ci.Visible = True
        End If
    End Sub
    Protected Sub ddlFormCondition_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddlFormCondition As DropDownList = sender
        pnlFormConditionRemarks.Visible = ddlFormCondition.SelectedValue = FormCondition.Others
        If ddlFormCondition.SelectedValue <> FormCondition.Others Then txtFormConditionRm.Text = ""
    End Sub
    Protected Sub ddlMeansofCommunication_SelectedIndexChanged(sender As Object, e As EventArgs)
    End Sub
#End Region
#Region "       2.2 New Inspection Record - Confirm"
    'Show Confirm Detail by View Model
    Private Sub FillDataNewConfrimDetailByModel(model As InspectionRecordViewModel)
        'View Inpsection Record Part
        lblConMainTypeofInspection.Text = model.MainTypeOfInspection
        lblConTypeofInspection.Text = model.TypeOfInspection
        lblConFileReferenceType.Text = model.FileReferenceType

        lblFileRefNoConfirm.Text = model.FileReferenceNo
        lblRefRefNo1Confirm.Text = model.ReferredReferenceNo1
        lblRefRefNo2Confirm.Text = model.ReferredReferenceNo2
        lblRefRefNo3Confirm.Text = model.ReferredReferenceNo3

        SetLableText(lblRefRefNo1Confirm, True)
        SetLableText(lblRefRefNo2Confirm, True)
        SetLableText(lblRefRefNo3Confirm, True)

        lblConCaseOfficer.Text = model.CaseOfficer
        lblConCaseContactNo.Text = model.CaseOfficerContactNo
        lblConSubjectOfficer.Text = model.SubjectOfficer
        lblConSubjectContactNo.Text = model.SubjectOfficerContactNo

        'View Visit Target Part
        lblConSPID.Text = model.SPID
        lblConSPStatus.Text = model.SPStatus
        lblConSPName.Text = model.SPName

        Dim isEmptySPTelNo As Boolean = String.IsNullOrEmpty(model.SPTelNo),
           isEmptySPFaxNo As Boolean = String.IsNullOrEmpty(model.SPFaxNo),
           isEmptySPEmail As Boolean = String.IsNullOrEmpty(model.SPEmail)

        trConSPContactInfo.Visible = Not (isEmptySPTelNo And isEmptySPFaxNo And isEmptySPEmail)
        pnlConSPTelNo.Visible = Not isEmptySPTelNo
        pnlConSPFaxNo.Visible = Not isEmptySPFaxNo
        pnlConSPEmail.Visible = Not isEmptySPEmail

        lblConSPTelNo.Text = model.SPTelNo
        lblConSPFaxNo.Text = model.SPFaxNo
        lblConSPEmail.Text = model.SPEmail
        lblConHCVSEffectiveDate.Text = model.HCVSEffectiveDtm
        lblConHCVSDHCEffectiveDate.Text = model.HCVSDHCEffectiveDtm
        lblConHCVSCHNEffectiveDate.Text = model.HCVSCHNEffectiveDtm
        lblConPractice.Text = model.PracticeName
        lblConPracticeStatus.Text = model.PracticeStatus
        lblConPractice_Ci.Text = model.PracticeNameChi
        lblConPracticeAddress.Text = model.PracticeAddress
        lblConPracticeAddress_Ci.Text = model.PracticeAddressChi
        lblConHP.Text = model.HealthProfession
        lblConPracticePhoneDaytime.Text = model.PracticePhoneDaytime
        lblConLastVisitDate.Text = model.LastVisitDateFormat

        'View Visit Detail Part
        lblConVisitDate.Text = model.VisitDateFormat
        lblConVisitTime.Text = model.VisitTimeFormat
        lblConConfirmationWith.Text = model.ConfirmationWith
        lblConConfirmDate.Text = model.ConfirmationDtmFormat
        lblConFormCondition.Text = model.FormCondition
        lblConMeansofCommunication.Text = model.MeansOfCommunication
        lblConMeansofCommunicationContact.Text = model.MeansOfCommunicationContact
        lblConLowRiskClaim.Text = model.LowRiskClaim
        lblConRemarks.Text = model.Remarks

        ShowDetailConfirmNew(PageMode.ModeNew)
    End Sub
    'Confirm -> Success Page
    Protected Sub ibtnConfirmNew_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtDB As New Database
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00008, AuditLogDesc.IRM_New_Confirm_Click)
        Try
            udtDB.BeginTransaction()
            Dim udtPracticeDisplayList As Practice.PracticeBLL.PracticeDisplayModelCollection
            Dim udtPracticeDisplay As Practice.PracticeBLL.PracticeDisplayModel


            Dim inspectionRecordNew As InspectionRecordModel = CType(Session(SESS_InspectionRecordModelNew), InspectionRecordModel)
            ' Gen XML Other Type of Inspection
            inspectionRecordNew.OtherTypeOfInspectionID = udtInspectionRecordBLL.GenXMLTypeOfInspectionByString(inspectionRecordNew.OtherTypeOfInspectionID)

            udtPracticeDisplayList = Me.udtSessionHandlerBLL.PracticeDisplayListGetFromSession(FunctionCode)
            udtPracticeDisplay = udtPracticeDisplayList.Filter(inspectionRecordNew.PracticeDisplaySeq)
            ' Set Service Category Code
            inspectionRecordNew.ServiceCategoryCode = udtPracticeDisplay.ServiceCategoryCode

            'Create New Inspection Record
            udtInspectionRecordBLL.CreateInspectionRecord(inspectionRecordNew, udtDB)

            udtAuditLogEntry.AddDescripton("Inspection ID", inspectionRecordNew.InspectionID)
            udtAuditLogEntry.AddDescripton("File Ref. No.", inspectionRecordNew.FileReferenceNo)

            Me.udtAuditLogEntry.WriteEndLog(LogID.LOG00009, AuditLogDesc.IRM_New_Confirm_Success)
            udtDB.CommitTransaction()

            SearchInspectionRecord()

            ShowSuccessMessage(boxInfoMessage, MsgCode.MSG00002, inspectionRecordNew.InspectionID, inspectionRecordNew.FileReferenceNo)
            MultiViewIRM.SetActiveView(vActionResultBox)
        Catch eSQL As SqlException
            udtAuditLogEntry.WriteEndLog(LogID.LOG00010, AuditLogDesc.IRM_New_Confirm_Fail)
            udtDB.RollBackTranscation()

            If eSQL.Number = 50000 Then
                Dim strmsg As String
                strmsg = eSQL.Message
                If strmsg = MsgCode.MSG00007 Then
                    Dim inspectionRecordNew As InspectionRecordModel = CType(Session(SESS_InspectionRecordModelNew), InspectionRecordModel)
                    ErrorHandler.Log("", SeverityCode.SEVE, "99999", HttpContext.Current.Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, eSQL.Message)
                    Me.udcMsgBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00007, "{FileRefNo}", inspectionRecordNew.FileReferenceNo)
                    Me.udcMsgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00010, AuditLogDesc.IRM_New_Confirm_Fail)
                End If
            Else
                Throw
            End If
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00010, AuditLogDesc.IRM_New_Confirm_Fail)
            udtDB.RollBackTranscation()
            Throw
        End Try
    End Sub
    'Back -> 2.1 New Inspection Record - Detail
    Protected Sub ibtnInspectionConBack_Click(sender As Object, e As ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00019, AuditLogDesc.IRM_NewRecordConfirmPageBack_Button_Click)

        HighlightTypeOfInspectionMultiple(rdoListAddMainTypeofInspection.ClientID, chkListAddTypeofInspection.ClientID)
        SetTxtReferNoOnKeyPress(PageMode.ModeNew)
        MultiViewIRM.SetActiveView(vNewInspection)
        Me.udcInfoMsgBox.Visible = False
        Me.udcMsgBox.Visible = False
    End Sub
#End Region
#Region "       3. Inspection Record - Detail Show"
    'Get Detail
    Protected Sub GetDetail(id As String, Optional isRedirected As Boolean = False)
        HideErrorImg()
        MultiViewIRM.SetActiveView(vViewInspectionDetail)

        Dim inspectionRecord As InspectionRecordModel = udtInspectionRecordBLL.GetInspectionRecord(id)
        Session(SESS_InspectionRecordModel) = inspectionRecord
        If (Not IsNothing(inspectionRecord)) Then
            FillDataVisitDetailByModel(inspectionRecord)
            Dim recordStatus As String = inspectionRecord.RecordStatus
            Dim originalStatus As String = inspectionRecord.OriginalStatus
            'Add by golden 644
            hdnStatus.Value = recordStatus
            hdnIsRedirect.Value = IIf(isRedirected, "Y", "N")
            recordStatus = IIf(recordStatus = InspectionStatus.Removed Or recordStatus = InspectionStatus.RemovePendingApproval, originalStatus, recordStatus)
            ' Result Inputted
            If recordStatus <> InspectionStatus.Incomplete And recordStatus <> InspectionStatus.PendingForSiteVisit Then
                divInspectionResultDetail.Visible = True
                FillDataInspectionResult(inspectionRecord)
                headVisitDetail.Visible = True
                trFreezeDate.Visible = True

                rblPrintContent.ClearSelection()
                'Enable SummaryReport Only
                rblPrintContent.Items(0).Enabled = False
                rblPrintContent.Items(1).Enabled = False
                rblPrintContent.Items(2).Enabled = True

                udtGeneralFunction.UpdateImageURL(ibtPdf, False)
                'Defualt radio button choosed
                rblPrintContent.Items(2).Selected = True
            Else
                ' Not yet input result
                divInspectionResultDetail.Visible = False
                headVisitDetail.Visible = True
                trFreezeDate.Visible = False

                rblPrintContent.ClearSelection()
                'Disable SummaryReport Only
                rblPrintContent.Items(0).Enabled = True
                rblPrintContent.Items(1).Enabled = True
                rblPrintContent.Items(2).Enabled = False

                udtGeneralFunction.UpdateImageURL(ibtPdf, True)
                'Defualt radio button choosed
                rblPrintContent.Items(0).Selected = True

            End If
            'Determine button visible .
            DetermineButtonVisible(inspectionRecord)
        Else

        End If
    End Sub
    Private Sub FillDataVisitDetailByModel(model As InspectionRecordModel)
        lblDetInspectionID.Text = model.InspectionID
        lblIIRInspectionID.Text = model.InspectionID

        lblDetFileNo.Text = model.FileReferenceNo
        lblIIRFileNo.Text = model.FileReferenceNo

        If Not String.IsNullOrEmpty(model.ReferredReferenceNo1) Or Not String.IsNullOrEmpty(model.ReferredReferenceNo2) Or Not String.IsNullOrEmpty(model.ReferredReferenceNo3) Then
            Me.spanRefNo.Visible = True
            lbDetReferredReferenceNo1.Text = model.ReferredReferenceNo1
            lbDetReferredReferenceNo2.Text = model.ReferredReferenceNo2
            lbDetReferredReferenceNo3.Text = model.ReferredReferenceNo3
            hfDetReferredInspectionID1.Value = model.ReferredInspectionID1
            hfDetReferredInspectionID2.Value = model.ReferredInspectionID2
            hfDetReferredInspectionID3.Value = model.ReferredInspectionID3
            lbDetReferredReferenceNo1.Enabled = Not String.IsNullOrEmpty(model.ReferredInspectionID1)
            lbDetReferredReferenceNo2.Enabled = Not String.IsNullOrEmpty(model.ReferredInspectionID2)
            lbDetReferredReferenceNo3.Enabled = Not String.IsNullOrEmpty(model.ReferredInspectionID3)
        Else
            Me.spanRefNo.Visible = False
            lbDetReferredReferenceNo1.Text = ""
            lbDetReferredReferenceNo2.Text = ""
            lbDetReferredReferenceNo3.Text = ""
            hfDetReferredInspectionID1.Value = ""
            hfDetReferredInspectionID2.Value = ""
            hfDetReferredInspectionID3.Value = ""
        End If

        lblDetSPID.Text = model.SPID
        lblIIRSPID.Text = model.SPID

        If Not String.IsNullOrEmpty(model.SPStatus) Then
            lblDetSPStatus.Text = joinParenthesis(model.SPStatus)
        Else
            lblDetSPStatus.Text = ""
        End If

        lblDetSPName.Text = String.Format("{0} {1}", model.SPEngName, udtformatter.formatChineseName(model.SPChiName))
        lblIIRSPName.Text = String.Format("{0} {1}", model.SPEngName, udtformatter.formatChineseName(model.SPChiName))

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

    'Referred File Reference No Redirect
    Protected Sub lbDetReferredReferenceNo1_Click(sender As Object, e As EventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode)

        If Not String.IsNullOrEmpty(hfDetReferredInspectionID1.Value) Then
            udtAuditLogEntry.AddDescripton("Inspection ID", hfDetReferredInspectionID1.Value)
            udtAuditLogEntry.AddDescripton("Referred File Ref No. index", "1")
            udtAuditLogEntry.WriteLog(LogID.LOG00086, AuditLogDesc.IRM_ReferredReferenceNo_Click)

            GetDetail(hfDetReferredInspectionID1.Value, True)
        End If
    End Sub
    Protected Sub lbDetReferredReferenceNo2_Click(sender As Object, e As EventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode)

        If Not String.IsNullOrEmpty(hfDetReferredInspectionID2.Value) Then
            udtAuditLogEntry.AddDescripton("Inspection ID", hfDetReferredInspectionID2.Value)
            udtAuditLogEntry.AddDescripton("Referred File Ref No. index", "2")
            udtAuditLogEntry.WriteLog(LogID.LOG00086, AuditLogDesc.IRM_ReferredReferenceNo_Click)

            GetDetail(hfDetReferredInspectionID2.Value, True)
        End If
    End Sub
    Protected Sub lbDetReferredReferenceNo3_Click(sender As Object, e As EventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode)

        If Not String.IsNullOrEmpty(hfDetReferredInspectionID3.Value) Then
            udtAuditLogEntry.AddDescripton("Inspection ID", hfDetReferredInspectionID3.Value)
            udtAuditLogEntry.AddDescripton("Referred File Ref No. index", "3")
            udtAuditLogEntry.WriteLog(LogID.LOG00086, AuditLogDesc.IRM_ReferredReferenceNo_Click)

            GetDetail(hfDetReferredInspectionID3.Value, True)
        End If
    End Sub
    'Edit Detail Click -> 4.1 Edit Visit Detail - Detail
    Protected Sub ibtnEditDetail_Click(sender As Object, e As EventArgs) Handles ibtnEditDetail.Click
        Dim checkRoleMsg = udtInspectionRecordBLL.CheckButtonEnable(ibtnEditDetail)
        If IsNothing(checkRoleMsg) Then
            Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00038, AuditLogDesc.IRM_EditDetail_Click)
            Try
                Dim dataField As DetailDataField = GetDetailDataFieldByMode(PageMode.ModeEdit)
                HideErrorImg()
                Me.udtAuditLogEntry.AddDescripton("File Ref No.", lblDetFileNo.Text)
                MultiViewIRM.SetActiveView(vEditVisit)
                'Set Other Type of Inspection 
                SetTypeofInspectionCheckBoxList(dataField.chkListTypeOfInspection)
                dataField.chkListTypeOfInspection.Attributes.Add("onclick", "HighlightType(this);")
                'Set Form Condition
                SetFormCondition(dataField.ddlFormCondition)
                ddlEdtFormCondition_SelectedIndexChanged(dataField.ddlFormCondition, Nothing)
                'Set Means of Communication
                SetMeansofCommunication(dataField.ddlMeansOfCommunication)
                ddlEdtMeansofCommunication_SelectedIndexChanged(dataField.ddlMeansOfCommunication, Nothing)
                'Set Officer List
                SetOfficerList(dataField)

                Dim inspectionRecord As InspectionRecordModel = CType(Session(SESS_InspectionRecordModel), InspectionRecordModel)
                If Not IsNothing(inspectionRecord) Then
                    lblEdtInspectionID.Text = inspectionRecord.InspectionID
                    lblEdtMainTypeOfInspection.Text = inspectionRecord.MainTypeOfInspectionValue

                    udtInspectionRecordBLL.HandleMainTypeOfInspectionSelectedValue(inspectionRecord.MainTypeOfInspectionID, dataField)

                    lblEdtSPID.Text = inspectionRecord.SPID
                    lblEdtSPName.Text = String.Format("{0} {1}", inspectionRecord.SPEngName, udtformatter.formatChineseName(inspectionRecord.SPChiName))

                    Dim isEmptySPContactNo As Boolean = String.IsNullOrEmpty(inspectionRecord.SPTelNo),
               isEmptySPFaxNo As Boolean = String.IsNullOrEmpty(inspectionRecord.SPFaxNo),
               isEmptySPEmail As Boolean = String.IsNullOrEmpty(inspectionRecord.SPEmail)

                    lblEdtSPTelNo.Text = inspectionRecord.SPTelNo
                    lblEdtSPFaxNo.Text = inspectionRecord.SPFaxNo
                    lblEdtSPEmail.Text = inspectionRecord.SPEmail

                    trEdtSPContactInfo.Visible = Not (isEmptySPContactNo And isEmptySPFaxNo And isEmptySPEmail)
                    pnlEdtSPTelNo.Visible = Not isEmptySPContactNo
                    pnlEdtSPFaxNo.Visible = Not isEmptySPFaxNo
                    pnlEdtSPEmail.Visible = Not isEmptySPEmail

                    If (inspectionRecord.SPHCVSEffectiveDtm = Date.MinValue) Then
                        trEdtHCVSEffectiveDate.Visible = False
                        lblEdtHCVSEffectiveDate.Text = ""
                    Else
                        trEdtHCVSEffectiveDate.Visible = True
                        lblEdtHCVSEffectiveDate.Text = udtInspectionRecordBLL.FormatOutputDate(inspectionRecord.SPHCVSEffectiveDtm)
                        If inspectionRecord.SPHCVSDelistDtm <> Date.MinValue Then
                            lblEdtHCVSEffectiveDate.Text += " <font color=""red"">" + joinParenthesis(Me.GetGlobalResourceObject("Text", "DelistedOn") + " " + udtInspectionRecordBLL.FormatOutputDate(inspectionRecord.SPHCVSDelistDtm)) + "</font>"
                        End If
                    End If

                    If (inspectionRecord.SPHCVSDHCEffectiveDtm = Date.MinValue) Then
                        trEdtHCVSDHCEffectiveDate.Visible = False
                        lblEdtHCVSDHCEffectiveDate.Text = ""
                    Else
                        trEdtHCVSDHCEffectiveDate.Visible = True
                        lblEdtHCVSDHCEffectiveDate.Text = udtInspectionRecordBLL.FormatOutputDate(inspectionRecord.SPHCVSDHCEffectiveDtm)
                        If inspectionRecord.SPHCVSDHCDelistDtm <> Date.MinValue Then
                            lblEdtHCVSDHCEffectiveDate.Text += " <font color=""red"">" + joinParenthesis(Me.GetGlobalResourceObject("Text", "DelistedOn") + " " + udtInspectionRecordBLL.FormatOutputDate(inspectionRecord.SPHCVSDHCDelistDtm)) + "</font>"
                        End If
                    End If

                    If (inspectionRecord.SPHCVSCHNEffectiveDtm = Date.MinValue) Then
                        trEdtHCVSCHNEffectiveDate.Visible = False
                        lblEdtHCVSCHNEffectiveDate.Text = ""
                    Else
                        trEdtHCVSCHNEffectiveDate.Visible = True
                        lblEdtHCVSCHNEffectiveDate.Text = udtInspectionRecordBLL.FormatOutputDate(inspectionRecord.SPHCVSCHNEffectiveDtm)
                        If inspectionRecord.SPHCVSCHNDelistDtm <> Date.MinValue Then
                            lblEdtHCVSCHNEffectiveDate.Text += " <font color=""red"">" + joinParenthesis(Me.GetGlobalResourceObject("Text", "DelistedOn") + " " + udtInspectionRecordBLL.FormatOutputDate(inspectionRecord.SPHCVSCHNDelistDtm)) + "</font>"
                        End If
                    End If

                    lblEdtPractice.Text = inspectionRecord.PracticeName
                    lblEdtPractice_Ci.Text = udtformatter.formatChineseName(inspectionRecord.PracticeNameChi)
                    lblEdtPracticeAddress.Text = inspectionRecord.PracticeAddress
                    lblEdtPracticeAddress_Ci.Text = udtformatter.formatChineseName(inspectionRecord.PracticeAddressChi)
                    lblEdtPracticePhoneDaytime.Text = inspectionRecord.PracticePhoneNo
                    lblEdtHealthProfession.Text = lblDetHealthProfession.Text
                    If (inspectionRecord.SPLastVisitDate <> Date.MinValue) Then
                        lblEdtLastVisitDate.Text = udtInspectionRecordBLL.FormatOutputDate(inspectionRecord.SPLastVisitDate) + joinParenthesis(Me.GetGlobalResourceObject("Text", "FileReferenceNo") + ": " + inspectionRecord.FileReferenceNo)
                    Else
                        lblEdtLastVisitDate.Text = Me.GetGlobalResourceObject("Text", "Empty")
                    End If

                    FillDataForTypeOfInspection(inspectionRecord.OtherTypeOfInspectionID)
                    txtEdtVisitDate.Text = udtInspectionRecordBLL.FormatInputDate(inspectionRecord.VisitDate)
                    txtEdtStartVisitTime.Text = udtInspectionRecordBLL.GetTimeFromDate(inspectionRecord.VisitBeginDtm)
                    txtEdtEndVisitTime.Text = udtInspectionRecordBLL.GetTimeFromDate(inspectionRecord.VisitEndDtm)
                    txtEdtConfirmationWith.Text = inspectionRecord.ConfirmationWith
                    txtEdtConfirmDate.Text = udtInspectionRecordBLL.FormatInputDate(inspectionRecord.ConfirmationDtm)
                    hfEdtCaseOfficer.Value = inspectionRecord.CaseOfficerValue
                    txtEdtCaseOfficer.Text = inspectionRecord.CaseOfficerValue
                    txtEdtCaseContactNo.Text = inspectionRecord.CaseOfficerContactNo
                    hfEdtSubjectOfficer.Value = inspectionRecord.SubjectOfficerValue
                    txtEdtSubjectOfficer.Text = inspectionRecord.SubjectOfficerValue
                    txtEdtSubjectContactNo.Text = inspectionRecord.SubjectOfficerContactNo
                    'File No and reference No
                    FillDataForReferenceNo(inspectionRecord.ReferredReferenceNo1, inspectionRecord.ReferredReferenceNo2, inspectionRecord.ReferredReferenceNo3)
                    lblEdtRefNo.Visible = True
                    lblEdtRefNo.Text = inspectionRecord.FileReferenceNo
                    If hdnStatus.Value = InspectionStatus.Incomplete Or hdnStatus.Value = InspectionStatus.PendingForSiteVisit Then
                        tableVisitTargetForEdit.Visible = True
                        tableVisitTargetReadOnly.Visible = False
                        txtEditSPID.Text = lblEdtSPID.Text
                        SearchSPEdit()
                        'ibtnEditSearchVisitTarget_Click(Nothing, Nothing)
                        'Practice_Display_Seq
                        ddlEditPractice.SelectedValue = inspectionRecord.PracticeDisplaySeq.ToString()
                        ddlEditPractice_SelectedIndexChanged(ddlEditPractice, Nothing)
                    Else
                        tableVisitTargetForEdit.Visible = False
                        tableVisitTargetReadOnly.Visible = True
                    End If

                    ddlEdtFormCondition.SelectedValue = inspectionRecord.FormConditionID
                    ddlEdtFormCondition_SelectedIndexChanged(ddlEdtFormCondition, Nothing)
                    txtEdtFormConditionRm.Text = inspectionRecord.FormConditionRemark
                    ddlEdtMeansofCommunication.SelectedValue = inspectionRecord.MeansOfCommunicationID
                    txtEdtMeansofCommunicationEmail.Text = inspectionRecord.MeansOfCommunicationEmail
                    txtEdtMeansofCommunicationFax.Text = inspectionRecord.MeansOfCommunicationFax

                    rdoEdtLowRiskClaim.ClearSelection()
                    If Not String.IsNullOrEmpty(inspectionRecord.LowRiskClaim) Then
                        rdoEdtLowRiskClaim.SelectedValue = inspectionRecord.LowRiskClaim
                    End If

                    txtEdtRemarks.Text = inspectionRecord.Remarks
                    HighlightTypeOfInspection(chkListEdtTypeofInspection.ClientID)
                    SetTxtReferNoOnKeyPress(PageMode.ModeEdit)
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00039, AuditLogDesc.IRM_EditDetail_Successful)
                Else
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00040, AuditLogDesc.IRM_EditDetail_Fail)
                End If
            Catch ex As Exception
                udtAuditLogEntry.WriteEndLog(LogID.LOG00040, AuditLogDesc.IRM_EditDetail_Fail)
            End Try
        Else
            Me.udcMsgBox.AddMessage(checkRoleMsg)
            Me.udcMsgBox.BuildMessageBox("Warning", udtAuditLogEntry, LogID.LOG00040, AuditLogDesc.IRM_EditDetail_Fail)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00040, AuditLogDesc.IRM_EditDetail_Fail)
        End If
    End Sub
    'Edit Result Click -> 5.1 Edit Result - Detail
    Protected Sub ibtnInputInspectionResult_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnInputInspectionResult.Click
        Dim checkRoleMsg = udtInspectionRecordBLL.CheckButtonEnable(ibtnInputInspectionResult)
        If IsNothing(checkRoleMsg) Then
            udtAuditLogEntry = New AuditLogEntry(FunctionCode)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00047, AuditLogDesc.IRM_InputInspectionResult_Click)
            Try
                GetVisitDetailForInputInspectionResult()
                InitEditDataInspectionResult()
                txtIIRInOrder.Attributes.Add("onblur", "checkNoOfClaim();")
                txtIIRMissingForm.Attributes.Add("onblur", "checkNoOfClaim();")
                txtIIRInconsistent.Attributes.Add("onblur", "checkNoOfClaim();")
                MultiViewIRM.SetActiveView(vInputInspectionResult)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00048, AuditLogDesc.IRM_InputInspectionResult_Successful)
            Catch ex As Exception
                udtAuditLogEntry.WriteEndLog(LogID.LOG00049, AuditLogDesc.IRM_InputInspectionResult_Fail)
            End Try
        Else
            Me.udcMsgBox.AddMessage(checkRoleMsg)
            Me.udcMsgBox.BuildMessageBox("Warning", udtAuditLogEntry, LogID.LOG00049, AuditLogDesc.IRM_InputInspectionResult_Fail)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00049, AuditLogDesc.IRM_InputInspectionResult_Fail)
        End If
    End Sub
    Protected Sub ibtnEditInspectionResult_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnEditInspectionResult.Click
        Dim checkRoleMsg = udtInspectionRecordBLL.CheckButtonEnable(ibtnEditInspectionResult)
        If IsNothing(checkRoleMsg) Then
            udtAuditLogEntry = New AuditLogEntry(FunctionCode)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00056, AuditLogDesc.IRM_EditInspectionResult_Click)
            Try
                LoadInspectionResultView()
                FillEditDataInspectionResult()
                udtAuditLogEntry.WriteEndLog(LogID.LOG00057, AuditLogDesc.IRM_EditInspectionResult_Successful)
            Catch ex As Exception
                udtAuditLogEntry.WriteEndLog(LogID.LOG00058, AuditLogDesc.IRM_EditInspectionResult_Fail)
            End Try
        Else
            Me.udcMsgBox.AddMessage(checkRoleMsg)
            Me.udcMsgBox.BuildMessageBox("Warning", udtAuditLogEntry, LogID.LOG00058, AuditLogDesc.IRM_EditInspectionResult_Fail)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00058, AuditLogDesc.IRM_EditInspectionResult_Fail)
        End If
    End Sub
    'Print Click
    Protected Sub ibtnPrint_Click(sender As Object, e As EventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        Me.udtAuditLogEntry.WriteStartLog(LogID.LOG00006, AuditLogDesc.IRM_Print_Button_Click)
        Me.mpePrintReport.Show()
    End Sub
    'Close Case Click Yes - Close Case Request
    Protected Sub ibtnCloseCase_Click(sender As Object, e As ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00078, AuditLogDesc.IRM_RequestClose_Click)
        Me.ModalPopupConfirmCloseCase.Show()
    End Sub
    Private Sub ucNoticePopUpConfirmCloseCase_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUpConfirmCloseCase.ButtonClick
        udcInfoMsgBox.Visible = False
        udcMsgBox.Visible = False

        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        Dim checkRoleMsg = udtInspectionRecordBLL.CheckButtonEnable(ibtnCloseCase)
        If IsNothing(checkRoleMsg) Then
            Select Case e
                Case ucNoticePopUp.enumButtonClick.OK

                    udtAuditLogEntry.WriteLog(LogID.LOG00070, AuditLogDesc.IRM_Confirm_Popup_Yes_Click)

                    Try
                        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

                        Dim inspectionModel As InspectionRecordModel = CType(Session(SESS_InspectionRecordModel), InspectionRecordModel)
                        udtAuditLogEntry.WriteStartLog(LogID.LOG00062, AuditLogDesc.IRM_RequestClose_Start)
                        udtAuditLogEntry.AddDescripton("Inspection ID", inspectionModel.InspectionID)

                        Dim model As New InspectionRecordModel
                        With model
                            .InspectionID = inspectionModel.InspectionID
                            .UserID = udtHCVUUserBLL.GetHCVUUser.UserID
                            .RecordStatus = InspectionStatus.ClosePendingApproval
                            .TSMP = inspectionModel.TSMP
                        End With
                        If udtInspectionRecordBLL.UpdateRecord(model, "UpdateStatus") Then
                            ShowSuccessMessage(boxInfoMessage, Common.Component.MsgCode.MSG00004, inspectionModel.InspectionID, inspectionModel.FileReferenceNo)
                            MultiViewIRM.SetActiveView(vActionResultBox)
                            udtAuditLogEntry.WriteEndLog(LogID.LOG00063, AuditLogDesc.IRM_RequestClose_Successful)
                        Else
                            Me.udcMsgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
                            Me.udcMsgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00060, AuditLogDesc.IRM_RequestClose_Fail)
                        End If
                    Catch ex As Exception
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00064, AuditLogDesc.IRM_RequestClose_Fail)
                    End Try

                Case Else
                    udtAuditLogEntry.WriteLog(LogID.LOG00069, AuditLogDesc.IRM_Confirm_Popup_No_Click)
            End Select
        Else
            Me.udcMsgBox.AddMessage(checkRoleMsg)
            Me.udcMsgBox.BuildMessageBox("Warning", udtAuditLogEntry, LogID.LOG00064, AuditLogDesc.IRM_RequestClose_Fail)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00064, AuditLogDesc.IRM_RequestClose_Fail)
        End If
    End Sub
    'Remove Click Yes - Remove Request
    Protected Sub ibtnRemove_Click(sender As Object, e As ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00077, AuditLogDesc.IRM_RequestRemove_Click)
        Me.ModalPopupConfirmRemove.Show()
    End Sub
    Private Sub ucNoticePopUpConfirmRemove_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUpConfirmRemove.ButtonClick
        udcInfoMsgBox.Visible = False
        udcMsgBox.Visible = False

        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        Dim checkRoleMsg = udtInspectionRecordBLL.CheckButtonEnable(ibtnRemove)
        If IsNothing(checkRoleMsg) Then
            Select Case e
                Case ucNoticePopUp.enumButtonClick.OK
                    udtAuditLogEntry.WriteLog(LogID.LOG00070, AuditLogDesc.IRM_Confirm_Popup_Yes_Click)

                    Try
                        Dim inspectionModel As InspectionRecordModel = CType(Session(SESS_InspectionRecordModel), InspectionRecordModel)
                        Dim model As New InspectionRecordModel

                        With model
                            .InspectionID = inspectionModel.InspectionID
                            .UserID = udtHCVUUserBLL.GetHCVUUser.UserID
                            .RecordStatus = InspectionStatus.RemovePendingApproval
                            .OriginalStatus = inspectionModel.RecordStatus
                            .TSMP = inspectionModel.TSMP
                        End With

                        udtAuditLogEntry.WriteStartLog(LogID.LOG00059, AuditLogDesc.IRM_RequestRemove_Start)
                        udtAuditLogEntry.AddDescripton("Inspection ID", inspectionModel.InspectionID)

                        If udtInspectionRecordBLL.UpdateRecord(model, "UpdateStatus") Then
                            ShowSuccessMessage(boxInfoMessage, Common.Component.MsgCode.MSG00005, inspectionModel.InspectionID, inspectionModel.FileReferenceNo)
                            MultiViewIRM.SetActiveView(vActionResultBox)
                            udtAuditLogEntry.WriteEndLog(LogID.LOG00060, AuditLogDesc.IRM_RequestRemove_Successful)
                        Else
                            Me.udcMsgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
                            Me.udcMsgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00060, AuditLogDesc.IRM_RequestRemove_Fail)
                        End If

                    Catch ex As Exception
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00061, AuditLogDesc.IRM_RequestRemove_Fail)
                    End Try

                Case Else
                    udtAuditLogEntry.WriteLog(LogID.LOG00069, AuditLogDesc.IRM_Confirm_Popup_No_Click)
            End Select
        Else
            Me.udcMsgBox.AddMessage(checkRoleMsg)
            Me.udcMsgBox.BuildMessageBox("Warning", udtAuditLogEntry, LogID.LOG00061, AuditLogDesc.IRM_RequestRemove_Fail)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00061, AuditLogDesc.IRM_RequestRemove_Fail)
        End If

    End Sub
    'Reopen Click - Reopen Request
    Protected Sub ibtnReopen_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnReopen.Click
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00079, AuditLogDesc.IRM_RequestReopen_Click)

        txtReopenReason.Text = String.Empty

        Dim checkRoleMsg = udtInspectionRecordBLL.CheckButtonEnable(ibtnRemove)
        If IsNothing(checkRoleMsg) Then

            ModalPopupConfirmReopen.Show()
            reopenInfoMsgBox.Visible = False
            reopenMsgBox.Visible = False
            reopenInfoMsgBox.Clear()
            reopenMsgBox.Clear()
            imgtxtReopenReasonErr.Visible = False
        Else
            Me.udcMsgBox.AddMessage(checkRoleMsg)
            Me.udcMsgBox.BuildMessageBox("Warning", udtAuditLogEntry, LogID.LOG00067, AuditLogDesc.IRM_RequestReopen_Fail)
        End If
    End Sub
    Protected Sub ibtnReopenConfirmPopup_Click(sender As Object, e As ImageClickEventArgs)
        udcInfoMsgBox.Visible = False
        udcMsgBox.Visible = False

        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Reopen Reason", txtReopenReason.Text)
        udtAuditLogEntry.WriteLog(LogID.LOG00080, AuditLogDesc.IRM_Confirm_Popup_Confirm_Click)

        reopenMsgBox.Visible = False
        reopenInfoMsgBox.Visible = False
        Dim reopenReason As String = txtReopenReason.Text.Trim
        If String.IsNullOrEmpty(reopenReason) Then
            imgtxtReopenReasonErr.Visible = True

            reopenMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, Common.Component.MsgCode.MSG00028, "%s", GetGlobalResourceObject("Text", "ReopenReason"))
            reopenMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00060, AuditLogDesc.IRM_RequestReopen_Fail)
            ModalPopupConfirmReopen.Show()
        Else
            Dim inspectionModel As InspectionRecordModel = CType(Session(SESS_InspectionRecordModel), InspectionRecordModel)
            Dim model As New InspectionRecordModel
            Dim newStatus As String = hdnStatus.Value
            Dim strMessageCode As String = String.Empty
            If hdnStatus.Value = InspectionStatus.Closed Then
                newStatus = InspectionStatus.ReopenPendingApproval
                strMessageCode = MsgCode.MSG00006
            End If

            With model
                .FileReferenceNo = inspectionModel.FileReferenceNo
                .InspectionID = inspectionModel.InspectionID
                .UserID = udtHCVUUserBLL.GetHCVUUser.UserID
                .RecordStatus = newStatus
                .ReopenRequestReason = reopenReason
                .TSMP = inspectionModel.TSMP
            End With
            udtAuditLogEntry.WriteStartLog(LogID.LOG00065, AuditLogDesc.IRM_RequestReopen_Start)
            udtAuditLogEntry.AddDescripton("Inspection ID", inspectionModel.InspectionID)
            If udtInspectionRecordBLL.UpdateRecord(model, "UpdateStatus") Then
                udtAuditLogEntry.WriteEndLog(LogID.LOG00066, AuditLogDesc.IRM_RequestReopen_Successful)
                ShowSuccessMessage(boxInfoMessage, strMessageCode, inspectionModel.InspectionID, inspectionModel.FileReferenceNo)
                MultiViewIRM.SetActiveView(vActionResultBox)
            Else
                Me.udcMsgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
                Me.udcMsgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00060, AuditLogDesc.IRM_RequestReopen_Fail)
            End If
        End If
    End Sub
    Protected Sub ibtnReopenCancel_Click(sender As Object, e As ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00081, AuditLogDesc.IRM_Confirm_Popup_Cancel_Click)

        reopenInfoMsgBox.Visible = False
        reopenMsgBox.Visible = False
        reopenInfoMsgBox.Clear()
        reopenMsgBox.Clear()
        imgtxtReopenReasonErr.Visible = False
    End Sub
    'Back -> 1. Search Page
    Protected Sub ibtnDetailBack_Click(sender As Object, e As ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00017, AuditLogDesc.IRM_DetailBack_Button_Click)

        If (hdnIsRedirect.Value = "Y") Then
            MultiViewIRM.SetActiveView(vSEARCH)
        Else
            SearchInspectionRecord()
            'ibtnSearch_Click(Nothing, Nothing)
        End If
    End Sub
    'Determine Button
    Public Sub DetermineButtonVisible(model As InspectionRecordModel)
        Dim udtHCVUUserBLL As New HCVUUserBLL
        Dim user As HCVUUserModel = udtHCVUUserBLL.GetHCVUUser()

        Dim inspectionRole As InspectionRole = udtInspectionRecordBLL.GetInspectionRole(user)

        Dim recordStatus As String = model.RecordStatus
        Dim originalStatus As String = model.OriginalStatus
        hdnStatus.Value = recordStatus

        ibtnEditDetail.Visible = False
        ibtnInputInspectionResult.Visible = False
        ibtnEditInspectionResult.Visible = False
        ibtnCloseCase.Visible = False
        ibtnPrint.Visible = False
        ibtnRemove.Visible = False
        ibtnReopen.Visible = False

        udtGeneralFunction.UpdateImageURL(ibtnEditDetail, True)
        udtGeneralFunction.UpdateImageURL(ibtnInputInspectionResult, True)
        udtGeneralFunction.UpdateImageURL(ibtnEditInspectionResult, True)
        udtGeneralFunction.UpdateImageURL(ibtnCloseCase, True)
        udtGeneralFunction.UpdateImageURL(ibtnRemove, True)
        udtGeneralFunction.UpdateImageURL(ibtnReopen, True)

        Dim isPersonForThisRecord As Boolean = False
        If user.UserID = model.CaseOfficerID Or user.UserID = model.SubjectOfficerID Then
            isPersonForThisRecord = True
        End If

        Select Case hdnStatus.Value
            Case InspectionStatus.Incomplete 'Incomplete
                ibtnEditDetail.Visible = True
                ibtnRemove.Visible = True

                If Not ((inspectionRole.IsOfficer And isPersonForThisRecord) Or inspectionRole.IsSEO) Then
                    udtGeneralFunction.UpdateImageURL(ibtnEditDetail, False)
                    udtGeneralFunction.UpdateImageURL(ibtnRemove, False)
                End If

            Case InspectionStatus.PendingForSiteVisit 'Pending for Site Visit
                ibtnPrint.Visible = True
                ibtnEditDetail.Visible = True
                ibtnRemove.Visible = True
                ibtnInputInspectionResult.Visible = True

                If Not ((inspectionRole.IsOfficer And isPersonForThisRecord) Or inspectionRole.IsSEO) Then
                    udtGeneralFunction.UpdateImageURL(ibtnEditDetail, False)
                    udtGeneralFunction.UpdateImageURL(ibtnRemove, False)
                    udtGeneralFunction.UpdateImageURL(ibtnInputInspectionResult, False)
                End If

                ' Visit Date is future date Dim input result
                If model.VisitDate <> Date.MinValue And model.VisitDate.Date > Date.Now.Date Then
                    udtGeneralFunction.UpdateImageURL(ibtnInputInspectionResult, False)
                End If

            Case InspectionStatus.ClosePendingApproval 'Close Case (Pending Approval)

            Case InspectionStatus.Closed 'Closed
                ibtnPrint.Visible = True
                ibtnReopen.Visible = True

                If Not ((inspectionRole.IsOfficer And isPersonForThisRecord) Or inspectionRole.IsSEO Or inspectionRole.IsEndorser) Then
                    udtGeneralFunction.UpdateImageURL(ibtnReopen, False)
                End If

            Case InspectionStatus.InspectionResultInputted 'Inspection Result Inputted
                ibtnEditDetail.Visible = True
                ibtnEditInspectionResult.Visible = True
                ibtnCloseCase.Visible = True
                ibtnRemove.Visible = True
                ibtnPrint.Visible = True

                If Not ((inspectionRole.IsOfficer And isPersonForThisRecord) Or inspectionRole.IsSEO) Then
                    udtGeneralFunction.UpdateImageURL(ibtnEditDetail, False)
                    udtGeneralFunction.UpdateImageURL(ibtnEditInspectionResult, False)
                    udtGeneralFunction.UpdateImageURL(ibtnCloseCase, False)
                    udtGeneralFunction.UpdateImageURL(ibtnRemove, False)
                End If

            Case InspectionStatus.RemovePendingApproval 'Remove (Pending Approval)

            Case InspectionStatus.Removed 'Removed

            Case InspectionStatus.ReopenPendingApproval 'Reopen (Pending Approval)

        End Select

    End Sub

#End Region
#Region "       4.1 Edit Visit Detail - Detail"
    'Fill Data
    Private Sub FillDataForReferenceNo(ReferNo1 As String, ReferNo2 As String, ReferNo3 As String)
        Dim strReferA As String = "", strReferB As String = "", strReferC As String = "", strReferD As String = "", strReferE As String = ""
        txtEdtRRNA1.Text = ""
        txtEdtRRNA2.Text = ""
        txtEdtRRNA3.Text = ""
        txtEdtRRNA4.Text = ""
        txtEdtRRNA5.Text = ""
        txtEdtRRNB1.Text = ""
        txtEdtRRNB2.Text = ""
        txtEdtRRNB3.Text = ""
        txtEdtRRNB4.Text = ""
        txtEdtRRNB5.Text = ""
        txtEdtRRNC1.Text = ""
        txtEdtRRNC2.Text = ""
        txtEdtRRNC3.Text = ""
        txtEdtRRNC4.Text = ""
        txtEdtRRNC5.Text = ""
        If ReferNo1 <> "" Then
            udtInspectionRecordBLL.SplitReferNo(ReferNo1, strReferA, strReferB, strReferC, strReferD, strReferE)
            txtEdtRRNA1.Text = strReferA
            txtEdtRRNA2.Text = strReferB
            txtEdtRRNA3.Text = strReferC
            txtEdtRRNA4.Text = strReferD
            txtEdtRRNA5.Text = strReferE
        End If
        If ReferNo2 <> "" Then
            udtInspectionRecordBLL.SplitReferNo(ReferNo2, strReferA, strReferB, strReferC, strReferD, strReferE)
            txtEdtRRNB1.Text = strReferA
            txtEdtRRNB2.Text = strReferB
            txtEdtRRNB3.Text = strReferC
            txtEdtRRNB4.Text = strReferD
            txtEdtRRNB5.Text = strReferE
        End If
        If ReferNo3 <> "" Then
            udtInspectionRecordBLL.SplitReferNo(ReferNo3, strReferA, strReferB, strReferC, strReferD, strReferE)
            txtEdtRRNC1.Text = strReferA
            txtEdtRRNC2.Text = strReferB
            txtEdtRRNC3.Text = strReferC
            txtEdtRRNC4.Text = strReferD
            txtEdtRRNC5.Text = strReferE
        End If
    End Sub
    Private Sub FillDataForTypeOfInspection(strTypeOfInspection As String)
        Dim arrTypeOfInspection As String() = strTypeOfInspection.Split(","c)
        For i As Integer = 0 To arrTypeOfInspection.Length - 1
            For Each Item As ListItem In chkListEdtTypeofInspection.Items
                If Item.Value = arrTypeOfInspection(i).Trim Then
                    Item.Selected = True
                    Exit For
                End If
            Next
        Next
    End Sub
    'Visit Target Function
    Protected Sub ibtnEditSearchVisitTarget_Click(sender As Object, e As ImageClickEventArgs)
        Me.udcMsgBox.Visible = False
        Me.udcInfoMsgBox.Visible = False

        imgtxtEditSPIDErr.Visible = False

        Dim objAuditLogInfo As AuditLogInfo = New AuditLogInfo(Me.txtEditSPID.Text.Trim, Nothing, Nothing, Nothing, Nothing, Nothing)
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("SP ID", txtEditSPID.Text.Trim)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00002, AuditLogDesc.IRM_SearchSP)

        Dim strSPID As String = txtEditSPID.Text.Trim
        Dim dtResult As New DataTable()

        'Init Edit Practice
        initDropDownList(ddlEditPractice)
        If True Then
            'Service Provider ID

            If strSPID = "" Then
                imgtxtEditSPIDErr.Visible = True
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00316)
                udcMsgBox.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00004, AuditLogDesc.IRM_SearchSP_Fail)
                Return
            End If

            Dim sm As SystemMessage = Me.udtValidator.chkSPID(strSPID)
            If IsNothing(sm) Then
                Dim udtAccountChangeMaintBLL As New AccountChangeMaintenance.AccountChangeMaintenanceBLL
                Dim inspectionRecord As InspectionRecordModel = CType(Session(SESS_InspectionRecordModel), InspectionRecordModel)
                If Not GetReadyServiceProvider(strSPID, PageMode.ModeEdit, inspectionRecord.InspectionID) Then
                    ' No Record Found
                    udcMsgBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001, "{SPID}", strSPID)

                    imgtxtEditSPIDErr.Visible = True
                    Me.udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00004, AuditLogDesc.IRM_SearchSP_Fail, objAuditLogInfo)
                Else
                    setAfterSearchTarget(PageMode.ModeEdit)
                End If
            Else
                imgtxtEditSPIDErr.Visible = True
                Me.udcMsgBox.AddMessage(sm)
                Me.udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00004, AuditLogDesc.IRM_SearchSP_Fail, objAuditLogInfo)
            End If

        End If
    End Sub
    Protected Sub ibtnEditClear_Click(sender As Object, e As ImageClickEventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00013, AuditLogDesc.IRM_SP_Clear_Button_Click)

        ClearTargetVisitByMode(PageMode.ModeEdit)
    End Sub
    'Submit Function -> 4.2 Edit Visit Detail - Confirm
    Protected Sub ibtnEditVisitSave_Click(sender As Object, e As ImageClickEventArgs)
        HideErrorImg()

        Dim inspectionRecord As InspectionRecordModel = CType(Session(SESS_InspectionRecordModel), InspectionRecordModel)

        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        Me.udtAuditLogEntry.AddDescripton("Inspection ID", inspectionRecord.InspectionID)
        Me.udtAuditLogEntry.AddDescripton("Record Status", inspectionRecord.RecordStatus)
        Me.udtAuditLogEntry.AddDescripton("Main Type of Inspection", Me.lblEdtMainTypeOfInspection.Text)
        Me.udtAuditLogEntry.AddDescripton("Other Type of Inspection", udtInspectionRecordBLL.GetTypeofInspectionStringFromInput(Me.chkListEdtTypeofInspection))
        Me.udtAuditLogEntry.AddDescripton("Referred File Ref. No. (1)", String.Format("(P1:{0}, P2:{1}, P3:{2}, P4:{3}, P5:{4})", Me.txtEdtRRNA1.Text, txtEdtRRNA2.Text, txtEdtRRNA3.Text, txtEdtRRNA4.Text, txtEdtRRNA5.Text))
        Me.udtAuditLogEntry.AddDescripton("Referred File Ref. No. (2)", String.Format("(P1:{0}, P2:{1}, P3:{2}, P4:{3}, P5:{4})", Me.txtEdtRRNB1.Text, txtEdtRRNB2.Text, txtEdtRRNB3.Text, txtEdtRRNB4.Text, txtEdtRRNB5.Text))
        Me.udtAuditLogEntry.AddDescripton("Referred File Ref. No. (3)", String.Format("(P1:{0}, P2:{1}, P3:{2}, P4:{3}, P5:{4})", Me.txtEdtRRNC1.Text, txtEdtRRNC2.Text, txtEdtRRNC3.Text, txtEdtRRNC4.Text, txtEdtRRNC5.Text))
        Me.udtAuditLogEntry.AddDescripton("Case Officer", Me.hfEdtCaseOfficer.Value)
        Me.udtAuditLogEntry.AddDescripton("Case Officer Contact No.", Me.txtEdtCaseContactNo.Text)
        Me.udtAuditLogEntry.AddDescripton("Subject Officer", Me.hfSubjectOfficer.Value)
        Me.udtAuditLogEntry.AddDescripton("Subject Officer Contact No.", Me.txtEdtSubjectContactNo.Text)
        Me.udtAuditLogEntry.AddDescripton("SP ID", Me.txtEditSPID.Text)
        Me.udtAuditLogEntry.AddDescripton("Practice", Me.ddlEditPractice.SelectedValue)
        Me.udtAuditLogEntry.AddDescripton("Visit Date", Me.txtEdtVisitDate.Text)
        Me.udtAuditLogEntry.AddDescripton("Visit Time From", Me.txtEdtStartVisitTime.Text)
        Me.udtAuditLogEntry.AddDescripton("Visit Time To", Me.txtEdtEndVisitTime.Text)
        Me.udtAuditLogEntry.AddDescripton("Confirmation With", Me.txtEdtConfirmationWith.Text)
        Me.udtAuditLogEntry.AddDescripton("Confirm Date", Me.txtEdtConfirmDate.Text)
        Me.udtAuditLogEntry.AddDescripton("Form Condition", Me.ddlEdtFormCondition.SelectedValue)
        Me.udtAuditLogEntry.AddDescripton("Form Condition Remarks", Me.txtEdtFormConditionRm.Text)
        Me.udtAuditLogEntry.AddDescripton("Means of Communication", Me.ddlEdtMeansofCommunication.SelectedValue)
        Me.udtAuditLogEntry.AddDescripton("Means of Communication Fax", Me.txtEdtMeansofCommunicationFax.Text)
        Me.udtAuditLogEntry.AddDescripton("Means of Communication Email", Me.txtEdtMeansofCommunicationEmail.Text)
        Me.udtAuditLogEntry.AddDescripton("Low Risk Claim", Me.rdoEdtLowRiskClaim.SelectedValue)
        Me.udtAuditLogEntry.AddDescripton("Remarks", Me.txtEdtRemarks.Text)

        udtAuditLogEntry.WriteStartLog(LogID.LOG00032, AuditLogDesc.IRM_EditVisitSave_Click)

        Dim dataField As DetailDataField = GetDetailDataFieldByMode(PageMode.ModeEdit)
        Dim blnError As Boolean = False

        'Check if Routine (Follow-up) is selected
        Dim isFollowUpInspectionTypeselected As Boolean = inspectionRecord.MainTypeOfInspectionID = TypeOfInspection.RoutineFollowUp

        'Check File Reference Type
        If inspectionRecord.FileReferenceType = FileReferenceType.Existing Then
            Dim strFileNo As String = inspectionRecord.FileReferenceNo
            Dim strNo1 = "", strNo2 = "", strNo3 = "", strNo4 = "", strNo5 = ""
            udtInspectionRecordBLL.SplitReferNo(strFileNo, strNo1, strNo2, strNo3, strNo4, strNo5)
            strFileNo = udtInspectionRecordBLL.HandleFileRefNo(strNo1, strNo2, strNo3, strNo4, "")
            Dim strSPID As String = IIf(inspectionRecord.RecordStatus = InspectionStatus.Incomplete Or inspectionRecord.RecordStatus = InspectionStatus.PendingForSiteVisit, txtEditSPID.Text.Trim, inspectionRecord.SPID)

            ' Check SP ID match with existing File Ref No. record
            If strSPID <> String.Empty Then

                Dim existRecord As DataTable = udtInspectionRecordBLL.SearchInspectionRecordByAny(New GetInspectionParameter With {
                                                                                         .FileReferenceNo = strFileNo,
                                                                                         .OnlyForOwner = 0,
                                                                                         .UserId = udtHCVUUserBLL.GetHCVUUser.UserID
                                                                                         })
                'Filter File Reference No (because Search inspection record use 'File_Reference_No like strFileNo')
                Dim recordDv As DataView = existRecord.DefaultView
                recordDv.RowFilter = "File_Reference_No ='" + strFileNo + "'"
                existRecord = recordDv.ToTable

                If existRecord.Rows.Count = 0 Then
                    'Not Exist
                    udcMsgBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00006, "{FileRefNo}", strFileNo)
                    blnError = True
                Else
                    recordDv = existRecord.DefaultView
                    recordDv.RowFilter = "SP_ID ='" + strSPID + "'"
                    existRecord = recordDv.ToTable
                    ' SP ID not match
                    If existRecord.Rows.Count = 0 Then
                        imgtxtEditSPIDErr.Visible = True
                        udcMsgBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00010, "{FileRefNo}", strFileNo)
                        blnError = True
                    End If
                End If
            End If
        End If
        'Check selected Other Type of Inspection
        For Each Item As ListItem In chkListEdtTypeofInspection.Items
            If Item.Selected Then
                If Item.Value = inspectionRecord.MainTypeOfInspectionID Then
                    imgchkListEdtTypeofInspectionErr.Visible = True
                    udcMsgBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00011)
                    blnError = True
                    Exit For
                End If
            End If
        Next

        HandleReferredFileReferenceNoList(dataField)

        Dim strReferredFileRefNoText As String = Me.GetGlobalResourceObject("Text", "ReferredFileReferenceNo")

        'Referred File RefNo 1
        FileReferenceNoValidation(strReferredFileRefNoText + "(1)", txtEdtRRNA1.Text, txtEdtRRNA2.Text, txtEdtRRNA3.Text, txtEdtRRNA4.Text, txtEdtRRNA5.Text, imgEdtRRNAErr, blnError, isFollowUpInspectionTypeselected)
        'Referred File RefNo 2
        FileReferenceNoValidation(strReferredFileRefNoText + "(2)", txtEdtRRNB1.Text, txtEdtRRNB2.Text, txtEdtRRNB3.Text, txtEdtRRNB4.Text, txtEdtRRNB5.Text, imgEdtRRNBErr, blnError)
        'Referred File RefNo 3
        FileReferenceNoValidation(strReferredFileRefNoText + "(3)", txtEdtRRNC1.Text, txtEdtRRNC2.Text, txtEdtRRNC3.Text, txtEdtRRNC4.Text, txtEdtRRNC5.Text, imgEdtRRNCErr, blnError)

        Dim strReferNoA As String = udtInspectionRecordBLL.HandleFileRefNo(txtEdtRRNA1.Text, txtEdtRRNA2.Text, txtEdtRRNA3.Text, txtEdtRRNA4.Text, txtEdtRRNA5.Text),
           strReferNoB As String = udtInspectionRecordBLL.HandleFileRefNo(txtEdtRRNB1.Text, txtEdtRRNB2.Text, txtEdtRRNB3.Text, txtEdtRRNB4.Text, txtEdtRRNB5.Text),
           strReferNoC As String = udtInspectionRecordBLL.HandleFileRefNo(txtEdtRRNC1.Text, txtEdtRRNC2.Text, txtEdtRRNC3.Text, txtEdtRRNC4.Text, txtEdtRRNC5.Text)

        ' Refer No A is not empty and A = B
        Dim AequalB As Boolean = Not String.IsNullOrEmpty(strReferNoA) And strReferNoA = strReferNoB
        ' Refer No C is not empty and A = C
        Dim AequalC As Boolean = Not String.IsNullOrEmpty(strReferNoC) And strReferNoA = strReferNoC
        ' Refer No B is not empty and B = C
        Dim BequalC As Boolean = Not String.IsNullOrEmpty(strReferNoB) And strReferNoB = strReferNoC

        imgEdtRRNAErr.Visible = IIf(AequalB Or AequalC, True, imgEdtRRNAErr.Visible)
        imgEdtRRNBErr.Visible = IIf(AequalB Or BequalC, True, imgEdtRRNBErr.Visible)
        imgEdtRRNCErr.Visible = IIf(AequalC Or BequalC, True, imgEdtRRNCErr.Visible)

        If AequalB Or AequalC Or BequalC Then
            udcMsgBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00009, "%s", IIf(AequalB Or AequalC, strReferNoA, strReferNoB))
            blnError = True
        End If

        If hdnStatus.Value = InspectionStatus.Incomplete Or hdnStatus.Value = InspectionStatus.PendingForSiteVisit Then
            'check Visit Target
            Dim noTarget = txtEditSPID.Enabled
            If noTarget Then
                txtEditSPID.Text = ""
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00316)
                imgtxtEditSPIDErr.Visible = True
                blnError = True
            Else
                If ddlEditPractice.SelectedValue = "" Then
                    udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00367, "%s", GetGlobalResourceObject("Text", "Practice"))
                    imgddlEditPracticeErr.Visible = True
                    blnError = True
                End If
            End If
        End If

        'add by golden 644
        Dim blnNeedValidation As Boolean = True
        Dim blnInComplete As Boolean = False
        If hdnStatus.Value = InspectionStatus.Creating Or hdnStatus.Value = InspectionStatus.Incomplete Or hdnStatus.Value = InspectionStatus.PendingForSiteVisit Then
            blnNeedValidation = False
        End If

        CheckDetailValidation(blnNeedValidation, blnError, blnInComplete, PageMode.ModeEdit)

        If hdnStatus.Value = InspectionStatus.Creating Or hdnStatus.Value = InspectionStatus.Incomplete Or hdnStatus.Value = InspectionStatus.PendingForSiteVisit Then
            If blnInComplete Then
                hdnStatus.Value = InspectionStatus.Incomplete
            Else
                hdnStatus.Value = InspectionStatus.PendingForSiteVisit
            End If
        End If

        If blnError Then
            Me.udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00034, AuditLogDesc.IRM_EditVisitSave_Fail)
            Return
        Else
            udcMsgBox.Visible = False
            udcInfoMsgBox.Visible = False
        End If
        MultiViewIRM.SetActiveView(vEditVisitConfirm)
        udtSM = New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001)
        Me.udcInfoMsgBox.AddMessage(udtSM)
        Me.udcInfoMsgBox.BuildMessageBox()


        If Not IsNothing(inspectionRecord) Then
            Dim udtStaticDataBLL As New StaticDataBLL
            lblEVCInspectionID.Text = inspectionRecord.InspectionID
            Dim typeOfInspectionFormat As String = ""
            For Each Item As String In udtInspectionRecordBLL.GetTypeofInspectionStringFromInput(Me.chkListEdtTypeofInspection).Split(",")
                Dim selectItem As StaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("TypeOfInspection", Item)
                typeOfInspectionFormat += IIf(typeOfInspectionFormat = "", selectItem.DataValue, "," + selectItem.DataValue)
            Next

            lblEVCMainTypeofInspection.Text = inspectionRecord.MainTypeOfInspectionValue
            lblEVCTypeofInspection.Text = typeOfInspectionFormat
            lblEVCFileReferenceNo.Text = inspectionRecord.FileReferenceNo
            lblEVCReferenceNo1.Text = udtInspectionRecordBLL.HandleFileRefNo(txtEdtRRNA1.Text, txtEdtRRNA2.Text, txtEdtRRNA3.Text, txtEdtRRNA4.Text, txtEdtRRNA5.Text)
            lblEVCReferenceNo2.Text = udtInspectionRecordBLL.HandleFileRefNo(txtEdtRRNB1.Text, txtEdtRRNB2.Text, txtEdtRRNB3.Text, txtEdtRRNB4.Text, txtEdtRRNB5.Text)
            lblEVCReferenceNo3.Text = udtInspectionRecordBLL.HandleFileRefNo(txtEdtRRNC1.Text, txtEdtRRNC2.Text, txtEdtRRNC3.Text, txtEdtRRNC4.Text, txtEdtRRNC5.Text)

            SetLableText(lblEVCReferenceNo1, True)
            SetLableText(lblEVCReferenceNo2, True)
            SetLableText(lblEVCReferenceNo3, True)

            lblEVCCaseOfficer.Text = txtEdtCaseOfficer.Text
            lblEVCCaseContactNo.Text = txtEdtCaseContactNo.Text
            lblEVCSubjectOfficer.Text = txtEdtSubjectOfficer.Text
            lblEVCSubjectContactNo.Text = txtEdtSubjectContactNo.Text

            If inspectionRecord.RecordStatus = InspectionStatus.Incomplete Or inspectionRecord.RecordStatus = InspectionStatus.PendingForSiteVisit Then
                lblEVCSPID.Text = txtEditSPID.Text
                lblEVCSPName.Text = lblEditServiceProviderName.Text
                lblEVCSPStatus.Text = lblEditSPStatus.Text
                Dim isEmptySPContactNo As Boolean = String.IsNullOrEmpty(lblEditSPTelNo.Text),
           isEmptySPFaxNo As Boolean = String.IsNullOrEmpty(lblEditSPFaxNo.Text),
           isEmptySPEmail As Boolean = String.IsNullOrEmpty(lblEditSPEmail.Text)

                trEVCSPContactInfo.Visible = Not (isEmptySPContactNo And isEmptySPFaxNo And isEmptySPEmail)
                pnlEVCSPTelNo.Visible = Not isEmptySPContactNo
                pnlEVCSPFaxNo.Visible = Not isEmptySPFaxNo
                pnlEVCSPEmail.Visible = Not isEmptySPEmail

                lblEVCSPTelNo.Text = lblEditSPTelNo.Text
                lblEVCSPFaxNo.Text = lblEditSPFaxNo.Text
                lblEVCSPEmail.Text = lblEditSPEmail.Text
                lblEVCHCVSEffectiveDate.Text = lblEditHCVSEffectiveDate.Text
                lblEVCHCVSDHCEffectiveDate.Text = lblEditHCVSDHCEffectiveDate.Text
                lblEVCHCVSCHNEffectiveDate.Text = lblEditHCVSCHNEffectiveDate.Text
                lblEVCPractice.Text = lblEditPractice.Text
                lblEVCPracticeStatus.Text = lblEditPracticeStatus.Text
                lblEVCPractice_Ci.Text = lblEditPractice_Ci.Text
                lblEVCPracticeAddress.Text = lblEditPracticeAddress.Text
                lblEVCPracticeAddress_Ci.Text = lblEditPracticeAddress_Ci.Text
                lblEVCHealthProfession.Text = lblEditHealthProfession.Text
                lblEVCPracticePhoneDaytime.Text = lblEditPracticePhoneDaytime.Text
                lblEVCLastVisitDate.Text = lblEditLastVisitDate.Text
            Else
                lblEVCSPID.Text = inspectionRecord.SPID
                lblEVCSPName.Text = lblEdtSPName.Text
                lblEVCSPStatus.Text = ""
                Dim isEmptySPContactNo As Boolean = String.IsNullOrEmpty(lblEdtSPTelNo.Text),
          isEmptySPFaxNo As Boolean = String.IsNullOrEmpty(lblEdtSPFaxNo.Text),
          isEmptySPEmail As Boolean = String.IsNullOrEmpty(lblEdtSPEmail.Text)

                trEVCSPContactInfo.Visible = Not (isEmptySPContactNo And isEmptySPFaxNo And isEmptySPEmail)
                pnlEVCSPTelNo.Visible = Not isEmptySPContactNo
                pnlEVCSPFaxNo.Visible = Not isEmptySPFaxNo
                pnlEVCSPEmail.Visible = Not isEmptySPEmail

                lblEVCSPTelNo.Text = lblEdtSPTelNo.Text
                lblEVCSPFaxNo.Text = lblEdtSPFaxNo.Text
                lblEVCSPEmail.Text = lblEdtSPEmail.Text
                lblEVCHCVSEffectiveDate.Text = lblEdtHCVSEffectiveDate.Text
                lblEVCHCVSDHCEffectiveDate.Text = lblEdtHCVSDHCEffectiveDate.Text
                lblEVCHCVSCHNEffectiveDate.Text = lblEdtHCVSCHNEffectiveDate.Text
                lblEVCPractice.Text = lblEdtPractice.Text
                lblEVCPractice_Ci.Text = lblEdtPractice_Ci.Text
                lblEVCPracticeAddress.Text = lblEdtPracticeAddress.Text
                lblEVCPracticeAddress_Ci.Text = lblEdtPracticeAddress_Ci.Text
                lblEVCHealthProfession.Text = lblEdtHealthProfession.Text
                lblEVCPracticePhoneDaytime.Text = lblEdtPracticePhoneDaytime.Text
                lblEVCLastVisitDate.Text = lblEdtLastVisitDate.Text
            End If

            Dim dateVisit As Date = udtInspectionRecordBLL.ConvertDate(txtEdtVisitDate.Text)

            lblEVCVisitDate.Text = udtInspectionRecordBLL.FormatOutputDate(dateVisit)

            If (inspectionRecord.RecordStatus = InspectionStatus.Incomplete Or inspectionRecord.RecordStatus = InspectionStatus.PendingForSiteVisit) And dateVisit <> Date.MinValue And dateVisit < Date.Now.Date Then
                lblEVCVisitDate.Text += " <font color=""red"">" + Me.GetGlobalResourceObject("Text", "PastDate") + "</font>"
            End If

            Dim visitTimeFormat As String = ""
            If (txtEdtStartVisitTime.Text.Trim <> "" And txtEdtEndVisitTime.Text.Trim <> "") Then
                visitTimeFormat = txtEdtStartVisitTime.Text.Trim + " - " + txtEdtEndVisitTime.Text.Trim
            End If

            lblEVCVisitTime.Text = visitTimeFormat

            lblEVCConfirmationWith.Text = txtEdtConfirmationWith.Text
            lblEVCLowRiskClaim.Text = IIf(String.IsNullOrEmpty(rdoEdtLowRiskClaim.SelectedValue.Trim), "", IIf(rdoEdtLowRiskClaim.SelectedValue.Trim = "Y", "Yes", "No"))
            lblEVCRemarks.Text = txtEdtRemarks.Text

            Dim dateConfirm As Date = udtInspectionRecordBLL.ConvertDate(txtEdtConfirmDate.Text)
            lblEVCConfirmDate.Text = udtInspectionRecordBLL.FormatOutputDate(dateConfirm)

            Dim formConditionFormat As String = IIf(String.IsNullOrEmpty(dataField.ddlFormCondition.SelectedValue), "", dataField.ddlFormCondition.SelectedItem.Text)

            If (dataField.ddlFormCondition.SelectedValue = FormCondition.Others) Then
                formConditionFormat += joinParenthesis(dataField.txtFormConditionRm.Text.Trim)
            End If

            lblEVCFormCondition.Text = formConditionFormat

            lblEVCMeansofCommunication.Text = IIf(String.IsNullOrEmpty(dataField.ddlMeansOfCommunication.SelectedValue), "", dataField.ddlMeansOfCommunication.SelectedItem.Text)

            If Not String.IsNullOrEmpty(dataField.txtMeansOfCommunicationEmail.Text.Trim) Or Not String.IsNullOrEmpty(dataField.txtMeansOfCommunicationFax.Text.Trim) Then
                lblEVCMeansofCommunicationContact.Text = joinParenthesis(Me.GetGlobalResourceObject("Text", "FaxNo") + ": " + IIf(Not String.IsNullOrEmpty(dataField.txtMeansOfCommunicationFax.Text.Trim), dataField.txtMeansOfCommunicationFax.Text.Trim, Me.GetGlobalResourceObject("Text", "Empty")) + " / " +
                                                          Me.GetGlobalResourceObject("Text", "Email") + ": " + IIf(Not String.IsNullOrEmpty(dataField.txtMeansOfCommunicationEmail.Text.Trim), dataField.txtMeansOfCommunicationEmail.Text.Trim, Me.GetGlobalResourceObject("Text", "Empty")))
            Else
                lblEVCMeansofCommunicationContact.Text = ""
            End If


            ShowDetailConfirmNew(PageMode.ModeEdit)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00033, AuditLogDesc.IRM_EditVisitSave_Successful)
        Else
            udtAuditLogEntry.WriteEndLog(LogID.LOG00034, AuditLogDesc.IRM_EditVisitSave_Fail)
        End If
    End Sub
    'Back Function -> 3. Inspection Record - Detail Show
    Protected Sub ibtnEditVisitBack_Click(sender As Object, e As ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00027, AuditLogDesc.IRM_EditVisitDetailBack_Button_Click)

        MultiViewIRM.SetActiveView(vViewInspectionDetail)
        Me.udcMsgBox.Visible = False
        Me.udcInfoMsgBox.Visible = False
    End Sub
    'Selected Value Change Function
    Protected Sub ddlEditPractice_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim udtFormatter As New Common.Format.Formatter
        Dim ddl As DropDownList = sender
        Dim val = ddl.SelectedValue
        hdfEditPracticeSeq.Value = val
        If val.Trim.Equals(String.Empty) Then
            ClearVisitTargetFormSub(PageMode.ModeEdit)
            ddlEditPractice.Visible = True
            lblEditPractice.Visible = False
            lblEditPracticeStatus.Text = String.Empty
            lblEditPracticeStatus.Visible = False
        Else
            imgddlEditPracticeErr.Visible = False

            Dim udtPracticeDisplayList As Practice.PracticeBLL.PracticeDisplayModelCollection
            Dim udtPracticeDisplay As Practice.PracticeBLL.PracticeDisplayModel

            udtPracticeDisplayList = Me.udtSessionHandlerBLL.PracticeDisplayListGetFromSession(FunctionCode)
            udtPracticeDisplay = udtPracticeDisplayList.Filter(CInt(val))

            If udtPracticeDisplay.PracticeStatus.Trim.Equals(PracticeStatus.Active) Then
                Me.lblEditPracticeStatus.Text = String.Empty
                Me.lblEditPracticeStatus.Visible = False
            Else
                Status.GetDescriptionFromDBCode(PracticeStatus.ClassCode, udtPracticeDisplay.PracticeStatus, Me.lblEditPracticeStatus.Text, String.Empty)
                Me.lblEditPracticeStatus.Text = joinParenthesis(Me.lblEditPracticeStatus.Text)
                Me.lblEditPracticeStatus.Visible = True
            End If

            lblEditPractice.Text = udtPracticeDisplay.PracticeName
            lblEditPractice_Ci.Text = udtFormatter.formatChineseName(udtPracticeDisplay.PracticeNameChi)

            Dim udtAddress As AddressModel = New AddressModel(udtPracticeDisplay.Room, udtPracticeDisplay.Floor, udtPracticeDisplay.Block, udtPracticeDisplay.Building, udtPracticeDisplay.BuildingChi, udtPracticeDisplay.District, Nothing)
            lblEditPracticeAddress.Text = udtFormatter.formatAddress(udtAddress)
            lblEditPracticeAddress_Ci.Text = udtFormatter.formatChineseName(udtFormatter.formatAddressChi(udtAddress))

            Dim strProfession As String = If(udtPracticeDisplay.Profession Is Nothing, Me.GetGlobalResourceObject("Text", "Empty"), udtPracticeDisplay.Profession.ServiceCategoryDesc + joinParenthesis(udtPracticeDisplay.RegistrationCode))
            lblEditHealthProfession.Text = strProfession
            lblEditPracticePhoneDaytime.Text = udtPracticeDisplay.PhoneDaytime
            lblEditPractice_Ci.Visible = True
        End If
    End Sub
    Protected Sub ddlEdtFormCondition_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddlFormCondition As DropDownList = sender
        pnlEdtFormConditionRm.Visible = ddlFormCondition.SelectedValue = FormCondition.Others
        If ddlFormCondition.SelectedValue <> FormCondition.Others Then txtEdtFormConditionRm.Text = ""
    End Sub
    Protected Sub ddlEdtMeansofCommunication_SelectedIndexChanged(sender As Object, e As EventArgs)
    End Sub
#End Region
#Region "       4.2 Edit Visit Detail - Confirm"
    'Confirm -> Success Page
    Protected Sub ibtnEditVisitConfirm_Click(sender As Object, e As ImageClickEventArgs)
        udcInfoMsgBox.Visible = False
        udcMsgBox.Visible = False
        Dim inspectionModel As InspectionRecordModel = CType(Session(SESS_InspectionRecordModel), InspectionRecordModel)

        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00035, AuditLogDesc.IRM_EditVisitConfirm_Click)
        Me.udtAuditLogEntry.AddDescripton("Inspection ID", inspectionModel.InspectionID)

        Dim xmlTypeOfInspection As String = String.Empty
        Dim dtTypeOfInspection As DataTable = udtInspectionRecordBLL.GetTypeofInspectionFromInput(chkListEdtTypeofInspection)
        xmlTypeOfInspection = udtInspectionRecordBLL.DataTableToXml(dtTypeOfInspection, "TypeOfInspection")

        Dim strCaseOfficer As String = String.Empty
        Dim strSubjectOfficer As String = String.Empty

        strCaseOfficer = hfEdtCaseOfficer.Value.Split("-")(0)
        strSubjectOfficer = hfEdtSubjectOfficer.Value.Split("-")(0)

        Dim model As New InspectionRecordModel
        With model
            .InspectionID = inspectionModel.InspectionID
            .FileReferenceNo = inspectionModel.FileReferenceNo
            .ReferredReferenceNo1 = udtInspectionRecordBLL.HandleFileRefNo(txtEdtRRNA1.Text, txtEdtRRNA2.Text, txtEdtRRNA3.Text, txtEdtRRNA4.Text, txtEdtRRNA5.Text)
            .ReferredReferenceNo2 = udtInspectionRecordBLL.HandleFileRefNo(txtEdtRRNB1.Text, txtEdtRRNB2.Text, txtEdtRRNB3.Text, txtEdtRRNB4.Text, txtEdtRRNB5.Text)
            .ReferredReferenceNo3 = udtInspectionRecordBLL.HandleFileRefNo(txtEdtRRNC1.Text, txtEdtRRNC2.Text, txtEdtRRNC3.Text, txtEdtRRNC4.Text, txtEdtRRNC5.Text)
            .OtherTypeOfInspectionID = xmlTypeOfInspection
            .VisitDate = IIf(txtEdtVisitDate.Text.Trim = "", Date.MinValue, udtformatter.convertDate(Me.txtEdtVisitDate.Text.Trim, String.Empty))
            .VisitBeginDtm = IIf(txtEdtStartVisitTime.Text.Trim = "", Date.MinValue, udtformatter.ConvertToDate(udtformatter.convertDate(Me.txtEdtVisitDate.Text.Trim, String.Empty) + " " + txtEdtStartVisitTime.Text.Trim))
            .VisitEndDtm = IIf(txtEdtEndVisitTime.Text.Trim = "", Date.MinValue, udtformatter.ConvertToDate(udtformatter.convertDate(Me.txtEdtVisitDate.Text.Trim, String.Empty) + " " + txtEdtEndVisitTime.Text.Trim))
            .ConfirmationWith = txtEdtConfirmationWith.Text.Trim
            .ConfirmationDtm = IIf(txtEdtConfirmDate.Text.Trim = "", Date.MinValue, udtformatter.convertDate(Me.txtEdtConfirmDate.Text.Trim, String.Empty))
            .FormConditionID = ddlEdtFormCondition.SelectedValue.Trim
            .FormConditionRemark = txtEdtFormConditionRm.Text.Trim
            .MeansOfCommunicationID = ddlEdtMeansofCommunication.SelectedValue.Trim
            .MeansOfCommunicationEmail = txtEdtMeansofCommunicationEmail.Text.Trim
            .MeansOfCommunicationFax = txtEdtMeansofCommunicationFax.Text.Trim
            .LowRiskClaim = rdoEdtLowRiskClaim.SelectedValue.Trim
            .Remarks = txtEdtRemarks.Text.Trim
            .CaseOfficerID = strCaseOfficer.Trim
            .CaseOfficerContactNo = txtEdtCaseContactNo.Text.Trim
            .SubjectOfficerID = strSubjectOfficer.Trim
            .SubjectOfficerContactNo = txtEdtSubjectContactNo.Text.Trim
            .RecordStatus = hdnStatus.Value
            .UserID = udtHCVUUserBLL.GetHCVUUser.UserID
            .SPLastVisitDate = inspectionModel.SPLastVisitDate
            .TSMP = inspectionModel.TSMP
        End With

        If hdnStatus.Value = InspectionStatus.Incomplete Or hdnStatus.Value = InspectionStatus.PendingForSiteVisit Then
            model.SPID = lblEVCSPID.Text
            model.PracticeDisplaySeq = ddlEditPractice.SelectedValue

            Dim udtPracticeDisplayList As Practice.PracticeBLL.PracticeDisplayModelCollection
            Dim udtPracticeDisplay As Practice.PracticeBLL.PracticeDisplayModel

            udtPracticeDisplayList = Me.udtSessionHandlerBLL.PracticeDisplayListGetFromSession(FunctionCode)
            udtPracticeDisplay = udtPracticeDisplayList.Filter(model.PracticeDisplaySeq)
            ' Set Service Category Code
            model.ServiceCategoryCode = udtPracticeDisplay.ServiceCategoryCode
        End If
        'UpdateInspectionRecord
        If udtInspectionRecordBLL.UpdateRecord(model, "UpdateVisitDetail") Then

            ShowSuccessMessage(InfoMessageBoxUpdateIIRSuccess, Common.Component.MsgCode.MSG00003, model.InspectionID, model.FileReferenceNo)
            MultiViewIRM.SetActiveView(vViewUpdateIIRSuccess)
            Me.udtAuditLogEntry.WriteEndLog(LogID.LOG00036, AuditLogDesc.IRM_EditVisitConfirm_Successful)
        Else
            Me.udcMsgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
            Me.udcMsgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00060, AuditLogDesc.IRM_EditVisitConfirm_Fail)
        End If

    End Sub
    'Back -> 4.1 Edit Visit Detail - Detail
    Protected Sub ibtnEditVisitConfirmBack_Click(sender As Object, e As ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00068, AuditLogDesc.IRM_EditVisitDetailConfirm_Back_Click)

        HighlightTypeOfInspection(chkListEdtTypeofInspection.ClientID)
        SetTxtReferNoOnKeyPress(PageMode.ModeEdit)
        MultiViewIRM.SetActiveView(vEditVisit)
        Me.udcMsgBox.Visible = False
        Me.udcInfoMsgBox.Visible = False
    End Sub
#End Region
#Region "       5.1 Edit Result - Detail"
    'Init
    Private Sub InitEditDataInspectionResult()
        txtIIRInOrder.Text = ""
        txtIIRMissingForm.Text = ""
        txtIIRInconsistent.Text = ""
        lblIIRTotalCheck.Text = ""
        rdoIIROverMajor.ClearSelection()
        rdoIIAnomalousClaim.ClearSelection()
        txtIINoofAnomalousClaim.Text = ""
        txtIINoofAnomalousClaim.Enabled = False
        txtIINoofIsOverMajor.Text = ""
        txtIINoofIsOverMajor.Enabled = False
        txtIIRCheckingDate.Text = ""
        dateIIRAletter.Text = ""
        dateIIRDletter.Text = ""
        dateIIRWletter.Text = ""
        dateIIRSPletter.Text = ""
        dateIIRSEAletter.Text = ""
        dateIIRSWDepart.Text = ""
        dateIIRHKCAEDepart.Text = ""
        dateIIRIDepart.Text = ""
        dateIIRLDepart.Text = ""
        dateIIROther.Text = ""
        txtIIROthers.Text = ""
        dateIIRBoard.Text = ""
        dateIIRPolice.Text = ""
        dateIIROthers.Text = ""
        txtIIROtherTP.Text = ""
        dateIIRSuspendEHCP.Text = ""
        dateIIRDelist.Text = ""
        dateIIRRecovery.Text = ""

        imgtxtIIRCheckingDateErr.Visible = False
        imgtxtIIRInOrderErr.Visible = False
        imgtxtIIRMissingFormErr.Visible = False
        imgtxtIIRInconsistentErr.Visible = False
        imgtxtIINoofAnomalousClaimErr.Visible = False
        imgtxtIINoofIsOverMajorErr.Visible = False
        imgrdoIIAnomalousClaimErr.Visible = False
        imgrdoIIROverMajorErr.Visible = False

        dateIIRAletterError.Visible = False
        dateIIRWletterError.Visible = False
        dateIIRDletterError.Visible = False
        dateIIRSPletterError.Visible = False
        dateIIRSEAletterError.Visible = False
        dateIIRSWDepartError.Visible = False
        dateIIRHKCAEDepartError.Visible = False
        dateIIRIDepartError.Visible = False
        dateIIRLDepartError.Visible = False
        dateIIRSuspendEHCPError.Visible = False
        dateIIROtherError.Visible = False
        dateIIRBoardError.Visible = False
        dateIIRPoliceError.Visible = False
        dateIIROthersError.Visible = False
        dateIIRDelistError.Visible = False
        dateIIRRecoveryError.Visible = False
        txtIIROthersError.Visible = False
        txtIIROtherTPError.Visible = False

        Dim dtFollowupAction As DataTable = udtInspectionRecordBLL.GetFollowUpActionStructure()
        Dim dr As DataRow = dtFollowupAction.NewRow
        dr("Followup_Action_Seq") = 1
        dtFollowupAction.Rows.Add(dr)

        repFollowUpAction.DataSource = dtFollowupAction

        repFollowUpAction.DataBind()
    End Sub
    'Fill Data
    Private Sub GetVisitDetailForInputInspectionResult()
        'Get value from the View vViewInspectionDetail
        lblIIRInspectionID.Text = lblDetInspectionID.Text
        lblIIRFileNo.Text = lblDetFileNo.Text
        lblIIRInspectionID.Text = lblDetInspectionID.Text
        lblIIRSPID.Text = lblDetSPID.Text
        lblIIRSPName.Text = lblDetSPName.Text
        lblIIRPractice.Text = lblDetPractice.Text
        lblIIRPracticeChi.Text = lblDetPractice_Ci.Text
        lblIIRPracticeAddress.Text = lblDetPracticeAddress.Text
        lblIIRPracticeAddressChi.Text = lblDetPracticeAddress_Ci.Text
        lblIIRMainTypeofInspection.Text = lblDetMainTypeofInspection.Text
        lblIIRTypeofInspection.Text = lblDetTypeofInspection.Text
        lblIIRVisitDate.Text = lblDetVisitDate.Text
        lblIIRVisitTime.Text = lblDetStartVisitTime.Text + " - " + lblDetEndVisitTime.Text
    End Sub
    Private Sub LoadInspectionResultView()
        GetVisitDetailForInputInspectionResult()
        InitEditDataInspectionResult()
        txtIIRInOrder.Attributes.Add("onblur", "checkNoOfClaim();")
        txtIIRMissingForm.Attributes.Add("onblur", "checkNoOfClaim();")
        txtIIRInconsistent.Attributes.Add("onblur", "checkNoOfClaim();")

        MultiViewIRM.SetActiveView(vInputInspectionResult)
    End Sub
    Private Sub FillEditDataInspectionResult()
        'Fill Inspection result Edit form
        Dim inspectionRecord As InspectionRecordModel = CType(Session(SESS_InspectionRecordModel), InspectionRecordModel)

        txtIIRInOrder.Text = inspectionRecord.NoOfInOrder
        txtIIRMissingForm.Text = inspectionRecord.NoOfMissingForm
        txtIIRInconsistent.Text = inspectionRecord.NoOfInconsistent
        lblIIRTotalCheck.Text = inspectionRecord.NoOfTotalCheck
        rdoIIROverMajor.SelectedValue = inspectionRecord.IsOverMajor
        txtIINoofIsOverMajor.Enabled = inspectionRecord.IsOverMajor = "Y"
        txtIINoofIsOverMajor.Text = IIf(inspectionRecord.NoOfIsOverMajor > 0, inspectionRecord.NoOfIsOverMajor, "")
        rdoIIAnomalousClaim.SelectedValue = inspectionRecord.AnomalousClaims
        txtIINoofAnomalousClaim.Enabled = inspectionRecord.AnomalousClaims = "Y"
        txtIINoofAnomalousClaim.Text = IIf(inspectionRecord.NoOfAnomalousClaims > 0, inspectionRecord.NoOfAnomalousClaims, "")

        txtIIRCheckingDate.Text = udtInspectionRecordBLL.FormatInputDate(inspectionRecord.CheckingDate)

        'Issue Letter
        dateIIRAletter.Text = udtInspectionRecordBLL.FormatInputDate(inspectionRecord.AdvisoryLetterDate)
        dateIIRDletter.Text = udtInspectionRecordBLL.FormatInputDate(inspectionRecord.DelistLetterDate)
        dateIIRWletter.Text = udtInspectionRecordBLL.FormatInputDate(inspectionRecord.WarningLetterDate)
        dateIIRSPletter.Text = udtInspectionRecordBLL.FormatInputDate(inspectionRecord.SuspendPaymentLetterDate)
        dateIIRSEAletter.Text = udtInspectionRecordBLL.FormatInputDate(inspectionRecord.SuspendEHCPAccountLetterDate)
        dateIIROther.Text = udtInspectionRecordBLL.FormatInputDate(inspectionRecord.OtherLetterDate)
        txtIIROthers.Text = inspectionRecord.OtherLetterRemark

        ' Refer Parties
        dateIIRBoard.Text = udtInspectionRecordBLL.FormatInputDate(inspectionRecord.BoardAndCouncilDate)
        dateIIRPolice.Text = udtInspectionRecordBLL.FormatInputDate(inspectionRecord.PoliceDate)
        dateIIRSWDepart.Text = udtInspectionRecordBLL.FormatInputDate(inspectionRecord.SocialWelfareDepartmentDate)
        dateIIRHKCAEDepart.Text = udtInspectionRecordBLL.FormatInputDate(inspectionRecord.HKCustomsandExciseDepartmentDate)
        dateIIRIDepart.Text = udtInspectionRecordBLL.FormatInputDate(inspectionRecord.ImmigrationDepartmentDate)
        dateIIRLDepart.Text = udtInspectionRecordBLL.FormatInputDate(inspectionRecord.LabourDeparmentDate)
        dateIIROthers.Text = udtInspectionRecordBLL.FormatInputDate(inspectionRecord.OtherPartyDate)
        txtIIROtherTP.Text = inspectionRecord.OtherPartyRemark

        ' Action to EHCP
        dateIIRSuspendEHCP.Text = udtInspectionRecordBLL.FormatInputDate(inspectionRecord.SuspendEHCPDate)
        dateIIRDelist.Text = udtInspectionRecordBLL.FormatInputDate(inspectionRecord.DelistEHCPDate)
        dateIIRRecovery.Text = udtInspectionRecordBLL.FormatInputDate(inspectionRecord.PaymentRecoverySuspensionDate)

        Dim dtFollowupAction As DataTable = udtInspectionRecordBLL.GetFollowupActionFromXML(inspectionRecord.FollowupAction)

        If IsNothing(dtFollowupAction) Then
            dtFollowupAction = udtInspectionRecordBLL.GetFollowUpActionStructure()
        End If
        If dtFollowupAction.Rows.Count = 0 Then
            Dim dr As DataRow = dtFollowupAction.NewRow
            dr("Followup_Action_Seq") = 1
            dtFollowupAction.Rows.Add(dr)
        End If
        Session(SESS_FollowupAction_DataTable_Edit) = dtFollowupAction
        repFollowUpAction.DataSource = dtFollowupAction
        repFollowUpAction.DataBind()
    End Sub
    'Follow-up Action Function
    Protected Sub lnkAddFollowUp_Click(sender As Object, e As EventArgs)
        Dim dr As DataRow
        Dim dt As System.Data.DataTable

        ' Get from System Parameter 
        Dim actionLimit As Integer = CInt(udtGeneralFunction.getSystemParameter("Inspection_Max_Followup_Action"))
        'Add a new row
        dt = udtInspectionRecordBLL.GetFollowActionFromInput(repFollowUpAction, True, False)
        If dt.Rows.Count < actionLimit Then
            dr = dt.NewRow()
            dr("Followup_Action_Seq") = dt.Rows.Count + 1
            dr("Action_Date") = ""
            dr("Action_Desc") = ""
            dr("Action_Date_Format") = ""
            dt.Rows.Add(dr)
            Session(SESS_FollowupAction_DataTable_Edit) = dt
            repFollowUpAction.DataSource = dt
            repFollowUpAction.DataBind()
        End If
    End Sub
    Protected Sub lnkDelFollowUp_Click(sender As Object, e As EventArgs)
        Dim arg As String = (CType(sender, ImageButton)).CommandArgument
        Dim dt As DataTable = udtInspectionRecordBLL.GetFollowActionFromInput(repFollowUpAction, True, False)
        Dim indexFind As Integer = -1
        For i As Integer = 0 To dt.Rows.Count - 1
            If dt.Rows(i)("Followup_Action_Seq").ToString() = arg Then
                indexFind = i
                Exit For
            End If
        Next
        If indexFind <> -1 Then
            dt.Rows.RemoveAt(indexFind)
        End If
        For i As Integer = 0 To dt.Rows.Count - 1
            dt.Rows(i)("Followup_Action_Seq") = i + 1
        Next
        If dt.Rows.Count = 0 Then
            Dim dr As DataRow = dt.NewRow()
            dr("Followup_Action_Seq") = "1"
            dr("Action_Date") = ""
            dr("Action_Desc") = ""
            dr("Action_Date_Format") = ""
            dt.Rows.Add(dr)
        End If
        Session(SESS_FollowupAction_DataTable_Edit) = dt
        repFollowUpAction.DataSource = dt
        repFollowUpAction.DataBind()
    End Sub

    'Validation
    Private Sub ValidateIntegerTextBox(textBox As TextBox, imgErr As Image, name As String, limit As Integer, ByRef blnError As Boolean, Optional ByVal mustGreaterZero As Boolean = True, Optional ByRef intValue As Integer = 0)
        Dim strInput As String = textBox.Text.Trim
        Dim valid As Boolean = True
        If String.IsNullOrEmpty(strInput) Then
            'Empty
            udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00028, "%s", name)
            valid = False
        Else
            If Not IsNumeric(strInput) Then
                'Invalid
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00365, "%s", name)
                valid = False
            Else
                intValue = CInt(strInput)
                If mustGreaterZero And intValue <= 0 Then
                    'must large than 0
                    udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00446, New String() {"%s", "%d"}, New String() {name, 0})
                    valid = False
                ElseIf limit > 0 And intValue > limit Then
                    'Over the limit(equal 0 mean No limit)
                    udcMsgBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00004, "%s", name)
                    valid = False
                Else
                    textBox.Text = intValue.ToString
                End If
            End If
        End If
        If Not valid Then
            blnError = True
            imgErr.Visible = True
        End If
    End Sub
    Private Function ValidateInspectionResult() As Boolean
        imgtxtIIRCheckingDateErr.Visible = False
        imgtxtIIRInOrderErr.Visible = False
        imgtxtIIRMissingFormErr.Visible = False
        imgtxtIIRInconsistentErr.Visible = False
        imgtxtIINoofAnomalousClaimErr.Visible = False
        imgtxtIINoofIsOverMajorErr.Visible = False
        imgrdoIIAnomalousClaimErr.Visible = False
        imgrdoIIROverMajorErr.Visible = False

        dateIIRAletterError.Visible = False
        dateIIRWletterError.Visible = False
        dateIIRDletterError.Visible = False
        dateIIRSPletterError.Visible = False
        dateIIRSEAletterError.Visible = False
        dateIIRSWDepartError.Visible = False
        dateIIRHKCAEDepartError.Visible = False
        dateIIRIDepartError.Visible = False
        dateIIRLDepartError.Visible = False
        dateIIRSuspendEHCPError.Visible = False
        dateIIROtherError.Visible = False
        dateIIRBoardError.Visible = False
        dateIIRPoliceError.Visible = False
        dateIIROthersError.Visible = False
        dateIIRDelistError.Visible = False
        dateIIRRecoveryError.Visible = False
        txtIIROthersError.Visible = False
        txtIIROtherTPError.Visible = False



        Dim blnError As Boolean = False
        Dim maxClaim As Integer = CInt(udtGeneralFunction.getSystemParameter("Inspection_Max_Claim_By_Category"))
        Dim inspectionRecord As InspectionRecordModel = CType(Session(SESS_InspectionRecordModel), InspectionRecordModel)

        udtAuditLogEntry.AddDescripton("Inspection ID", inspectionRecord.InspectionID)
        udtAuditLogEntry.AddDescripton("In Order", txtIIRInOrder.Text)
        udtAuditLogEntry.AddDescripton("Missing Form", txtIIRMissingForm.Text)
        udtAuditLogEntry.AddDescripton("Inconsistent", txtIIRInconsistent.Text)
        udtAuditLogEntry.AddDescripton("Anomalous Claims", rdoIIAnomalousClaim.SelectedValue)
        udtAuditLogEntry.AddDescripton("No. of Record(Anomalous Claims)", txtIINoofAnomalousClaim.Text)
        udtAuditLogEntry.AddDescripton("Over 40% major irregularities", rdoIIROverMajor.SelectedValue)
        udtAuditLogEntry.AddDescripton("No. of Record(Over 40% major irregularities)", txtIINoofIsOverMajor.Text)
        udtAuditLogEntry.AddDescripton("Checking Date", txtIIRCheckingDate.Text)
        udtAuditLogEntry.AddDescripton("AdvisoryLetter", dateIIRAletter.Text)
        udtAuditLogEntry.AddDescripton("WarningLetter", dateIIRWletter.Text)
        udtAuditLogEntry.AddDescripton("DelistLetter", dateIIRDletter.Text)
        udtAuditLogEntry.AddDescripton("SuspendPaymentLetter", dateIIRSPletter.Text)
        udtAuditLogEntry.AddDescripton("SuspendEHCPAccountLetter", dateIIRSEAletter.Text)
        udtAuditLogEntry.AddDescripton("Issue Letter Others", txtIIROthers.Text + joinParenthesis(dateIIROther.Text))
        udtAuditLogEntry.AddDescripton("BoardAndCouncil", dateIIRBoard.Text)
        udtAuditLogEntry.AddDescripton("Police", dateIIRPolice.Text)
        udtAuditLogEntry.AddDescripton("SocialWelfareDepartment", dateIIRSWDepart.Text)
        udtAuditLogEntry.AddDescripton("HKCustomsAndExciseDepartment", dateIIRHKCAEDepart.Text)
        udtAuditLogEntry.AddDescripton("ImmigrationDepartment", dateIIRIDepart.Text)
        udtAuditLogEntry.AddDescripton("LabourDepartment", dateIIRLDepart.Text)
        udtAuditLogEntry.AddDescripton("Refer Parties Others", txtIIROtherTP.Text + joinParenthesis(dateIIROthers.Text))
        udtAuditLogEntry.AddDescripton("SuspendTheEHCPFromHCVS", dateIIRSuspendEHCP.Text)
        udtAuditLogEntry.AddDescripton("DelistTheEHCP", dateIIRDelist.Text)
        udtAuditLogEntry.AddDescripton("RecoveryOrSuspensionPayment", dateIIRRecovery.Text)
        udtAuditLogEntry.AddDescripton("FollowUpAction", udtInspectionRecordBLL.GetFollowActionFromInput(repFollowUpAction))

        Dim intNoOfInOrder As Integer = Nothing,
            intNoOfMissingForm As Integer = Nothing,
            intNoOfInconsistent As Integer = Nothing,
            intNoOfTotalCheck As Integer = 0
        'No of In Order Validation
        ValidateIntegerTextBox(txtIIRInOrder, imgtxtIIRInOrderErr, Me.GetGlobalResourceObject("Text", "InOrder"), maxClaim, blnError, False, intNoOfInOrder)
        'No of MissingForm Validation
        ValidateIntegerTextBox(txtIIRMissingForm, imgtxtIIRMissingFormErr, Me.GetGlobalResourceObject("Text", "MissingForm"), maxClaim, blnError, False, intNoOfMissingForm)
        'No of Inconsitent Validation
        ValidateIntegerTextBox(txtIIRInconsistent, imgtxtIIRInconsistentErr, Me.GetGlobalResourceObject("Text", "Inconsistent"), maxClaim, blnError, False, intNoOfInconsistent)

        intNoOfTotalCheck = intNoOfInOrder + intNoOfMissingForm + intNoOfInconsistent

        If rdoIIAnomalousClaim.SelectedIndex = -1 Then
            blnError = True
            udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00367, "%s", Me.GetGlobalResourceObject("Text", "AnomalousClaims"))
            imgrdoIIAnomalousClaimErr.Visible = True
        ElseIf rdoIIAnomalousClaim.SelectedValue = "Y" Then
            Dim intNoofAnomalousClaim As Integer = Nothing
            'No of Anomalous Claim Validation
            ValidateIntegerTextBox(txtIINoofAnomalousClaim, imgtxtIINoofAnomalousClaimErr, Me.GetGlobalResourceObject("Text", "AnomalousClaims"), maxClaim, blnError, True, intNoofAnomalousClaim)
            'No of Anomalous Clalm <= No of Total Check
            If intNoOfTotalCheck > 0 And intNoofAnomalousClaim > intNoOfTotalCheck Then
                blnError = True
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00449, New String() {"%s", "%d"}, New String() {Me.GetGlobalResourceObject("Text", "AnomalousClaims"), Me.GetGlobalResourceObject("Text", "TotalChecked")})
                imgtxtIINoofAnomalousClaimErr.Visible = True
            End If
        End If
        If rdoIIROverMajor.SelectedIndex = -1 Then
            blnError = True
            udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00367, "%s", Me.GetGlobalResourceObject("Text", "OverMajorIrregularities"))
            imgrdoIIROverMajorErr.Visible = True
        ElseIf rdoIIROverMajor.SelectedValue = "Y" Then
            Dim intNoofIsOverMajor As Integer = Nothing
            'No of Is Over Major Validation
            ValidateIntegerTextBox(txtIINoofIsOverMajor, imgtxtIINoofIsOverMajorErr, Me.GetGlobalResourceObject("Text", "OverMajorIrregularities"), maxClaim, blnError, True, intNoofIsOverMajor)
            'No of Is Over Major Validation <= No of  Absent + Inconsistent
            Dim noOfIrregularities As Integer = intNoOfMissingForm + intNoOfInconsistent
            If noOfIrregularities > 0 And intNoofIsOverMajor > noOfIrregularities Then
                blnError = True
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00449, New String() {"%s", "%d"}, New String() {Me.GetGlobalResourceObject("Text", "OverMajorIrregularities"), Me.GetGlobalResourceObject("Text", "Irregularities")})
                imgtxtIINoofIsOverMajorErr.Visible = True
            End If
        End If
        Dim strCheckingDate = IIf(udtformatter.formatInputDate(txtIIRCheckingDate.Text.Trim) <> String.Empty, udtformatter.formatInputDate(txtIIRCheckingDate.Text.Trim), txtIIRCheckingDate.Text.Trim)
        Dim MsgCheckingDate As SystemMessage = udtValidator.chkInputDate(strCheckingDate, True, True)
        If txtIIRCheckingDate.Text.Trim <> "" Then
            If Not IsNothing(MsgCheckingDate) Then
                blnError = True
                imgtxtIIRCheckingDateErr.Visible = True
                Me.udcMsgBox.AddMessage(MsgCheckingDate, "%s", Me.GetGlobalResourceObject("Text", "CheckingDate"))
            Else
                Dim dateCheckingDate As Date = udtInspectionRecordBLL.ConvertDate(strCheckingDate)
                'Checking Date must be on or after Visit Date
                If (inspectionRecord.VisitDate <> Date.MinValue And inspectionRecord.VisitDate.Date > dateCheckingDate.Date) Then
                    blnError = True
                    imgtxtIIRCheckingDateErr.Visible = True
                    Me.udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00381, New String() {"%s", "%t"}, New String() {Me.GetGlobalResourceObject("Text", "CheckingDate"), Me.GetGlobalResourceObject("Text", "VisitDate")})
                End If
                txtIIRCheckingDate.Text = strCheckingDate
            End If
        Else
            blnError = True
            imgtxtIIRCheckingDateErr.Visible = True
            udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00028, "%s", Me.GetGlobalResourceObject("Text", "CheckingDate"))
        End If

        'Issue Letter
        ActionListDateValidation(dateIIRAletter, dateIIRAletterError, Me.GetGlobalResourceObject("Text", "AdvisoryLetter"), blnError)
        ActionListDateValidation(dateIIRWletter, dateIIRWletterError, Me.GetGlobalResourceObject("Text", "WarningLetter"), blnError)
        ActionListDateValidation(dateIIRDletter, dateIIRDletterError, Me.GetGlobalResourceObject("Text", "DelistLetter"), blnError)
        ActionListDateValidation(dateIIRSPletter, dateIIRSPletterError, Me.GetGlobalResourceObject("Text", "SuspendPaymentLetter"), blnError)
        ActionListDateValidation(dateIIRSEAletter, dateIIRSEAletterError, Me.GetGlobalResourceObject("Text", "SuspendEHCPAccountLetter"), blnError)

        Dim strIssueLetterOther As String = String.Format("{0} ({1})", Me.GetGlobalResourceObject("Text", "IssueLetter"), Me.GetGlobalResourceObject("Text", "Others"))
        If Not String.IsNullOrEmpty(dateIIROther.Text) Then
            ActionListDateValidation(dateIIROther, dateIIROtherError, strIssueLetterOther, blnError)
            If String.IsNullOrEmpty(txtIIROthers.Text) Then
                blnError = True
                txtIIROthersError.Visible = True
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00364, "%s", strIssueLetterOther)
            End If

        ElseIf Not String.IsNullOrEmpty(txtIIROthers.Text) Then
            blnError = True
            dateIIROtherError.Visible = True
            udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00364, "%s", strIssueLetterOther)
        End If

        ' Refer Parties
        ActionListDateValidation(dateIIRBoard, dateIIRBoardError, Me.GetGlobalResourceObject("Text", "BoardAndCouncil"), blnError)
        ActionListDateValidation(dateIIRPolice, dateIIRPoliceError, Me.GetGlobalResourceObject("Text", "Police"), blnError)
        ActionListDateValidation(dateIIRSWDepart, dateIIRSWDepartError, Me.GetGlobalResourceObject("Text", "SocialWelfareDepartment"), blnError)
        ActionListDateValidation(dateIIRHKCAEDepart, dateIIRHKCAEDepartError, Me.GetGlobalResourceObject("Text", "HKCustomsAndExciseDepartment"), blnError)
        ActionListDateValidation(dateIIRIDepart, dateIIRIDepartError, Me.GetGlobalResourceObject("Text", "ImmigrationDepartment"), blnError)
        ActionListDateValidation(dateIIRLDepart, dateIIRLDepartError, Me.GetGlobalResourceObject("Text", "LabourDepartment"), blnError)

        Dim strReferPartyOther As String = String.Format("{0} ({1})", Me.GetGlobalResourceObject("Text", "ReferParties"), Me.GetGlobalResourceObject("Text", "Others"))

        If Not String.IsNullOrEmpty(dateIIROthers.Text) Then

            ActionListDateValidation(dateIIROthers, dateIIROthersError, strReferPartyOther, blnError)

            If String.IsNullOrEmpty(txtIIROtherTP.Text) Then
                blnError = True
                txtIIROtherTPError.Visible = True
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00364, "%s", strReferPartyOther)
            End If

        ElseIf Not String.IsNullOrEmpty(txtIIROtherTP.Text) Then
            blnError = True
            dateIIROthersError.Visible = True
            udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00364, "%s", strReferPartyOther)
        End If


        'Action to EHCP
        ActionListDateValidation(dateIIRSuspendEHCP, dateIIRSuspendEHCPError, Me.GetGlobalResourceObject("Text", "SuspendTheEHCPFromHCVS"), blnError)
        ActionListDateValidation(dateIIRDelist, dateIIRDelistError, Me.GetGlobalResourceObject("Text", "DelistTheEHCP"), blnError)
        ActionListDateValidation(dateIIRRecovery, dateIIRRecoveryError, Me.GetGlobalResourceObject("Text", "RecoveryOrSuspensionPayment"), blnError)

        'Get data from repeater
        Dim txtActionDate As TextBox
        Dim txtActionDesc As TextBox
        Dim txtActionDateErr As Image
        Dim txtActionDescErr As Image
        For i As Integer = 0 To repFollowUpAction.Items.Count - 1
            txtActionDate = CType(repFollowUpAction.Items(i).FindControl("txtFollowUpDate"), TextBox)
            txtActionDesc = CType(repFollowUpAction.Items(i).FindControl("txtFollowUpAction"), TextBox)
            txtActionDateErr = CType(repFollowUpAction.Items(i).FindControl("txtFollowUpDateError"), Image)
            txtActionDescErr = CType(repFollowUpAction.Items(i).FindControl("txtFollowUpActionError"), Image)
            txtActionDateErr.Visible = False
            txtActionDescErr.Visible = False

            If Not String.IsNullOrEmpty(txtActionDate.Text) Or Not String.IsNullOrEmpty(txtActionDesc.Text) Then
                If String.IsNullOrEmpty(txtActionDate.Text) Then
                    blnError = True
                    txtActionDateErr.Visible = True
                    Me.udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00364, "%s", String.Format("{0} ({1})", Me.GetGlobalResourceObject("Text", "FollowUpAction"), i + 1))
                Else
                    Dim strDate = IIf(udtformatter.formatInputDate(txtActionDate.Text.Trim) <> String.Empty, udtformatter.formatInputDate(txtActionDate.Text.Trim), txtActionDate.Text.Trim)
                    MsgCheckingDate = udtValidator.chkInputDate(strDate, False, False)
                    If Not IsNothing(MsgCheckingDate) Then
                        blnError = True
                        txtActionDateErr.Visible = True
                        Me.udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00029, "%s", String.Format("Date of {0} ({1})", Me.GetGlobalResourceObject("Text", "FollowUpAction"), i + 1))
                    Else
                        Dim dateAction As Date = udtInspectionRecordBLL.ConvertDate(strDate)

                        txtActionDate.Text = strDate
                        If dateAction.Date < inspectionRecord.VisitDate.Date Then
                            blnError = True
                            txtActionDateErr.Visible = True
                            udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00381, New String() {"%s", "%t"}, New String() {String.Format("Date of {0} ({1})", Me.GetGlobalResourceObject("Text", "FollowUpAction"), i + 1), Me.GetGlobalResourceObject("Text", "VisitDate")})
                        End If
                    End If
                End If
                If String.IsNullOrEmpty(txtActionDesc.Text) Then
                    blnError = True
                    txtActionDescErr.Visible = True
                    Me.udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00364, "%s", String.Format("{0} ({1})", Me.GetGlobalResourceObject("Text", "FollowUpAction"), i + 1))
                End If
            End If
        Next



        Return blnError
    End Function
    Private Sub ActionListDateValidation(ByVal inputBox As TextBox, ByVal dateErr As Image, ByVal name As String, ByRef blnError As Boolean)
        Dim MsgDate As SystemMessage
        Dim strDate = IIf(udtformatter.formatInputDate(inputBox.Text.Trim) <> String.Empty, udtformatter.formatInputDate(inputBox.Text.Trim), inputBox.Text.Trim)

        If Not String.IsNullOrEmpty(strDate) Then
            MsgDate = udtValidator.chkInputDate(strDate, False, False)
            If Not IsNothing(MsgDate) Then
                blnError = True
                dateErr.Visible = True
                Me.udcMsgBox.AddMessage(MsgDate, "%s", "Date of " + name)
            Else
                inputBox.Text = strDate
            End If
        End If
    End Sub
    'Submit Function -> 5.2 Edit Result - Confirm 
    Protected Sub ibtnIIRSave_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnIIRSave.Click
        udtAuditLogEntry = New AuditLogEntry(FunctionCode)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00050, AuditLogDesc.IRM_InspectionResultSave_Click)

        If ValidateInspectionResult() Then
            Me.udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00052, AuditLogDesc.IRM_InspectionResultSave_Fail)
        Else
            'Save Inspection result.
            udcMsgBox.Visible = False
            udcInfoMsgBox.Visible = False
            udtSM = New Common.ComObject.SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001)
            Me.udcInfoMsgBox.AddMessage(udtSM)
            Me.udcInfoMsgBox.BuildMessageBox()
            GetVisitDetailForInputInspectionResultConfirm()

            lblIIRInOrder.Text = txtIIRInOrder.Text
            lblIIRMissingForm.Text = txtIIRMissingForm.Text
            lblIIRInconsistent.Text = txtIIRInconsistent.Text
            lblIIRTotalConfirm.Text = Convert.ToInt32(lblIIRInOrder.Text) + Convert.ToInt32(lblIIRMissingForm.Text) + Convert.ToInt32(lblIIRInconsistent.Text)
            lblIIRCheckingDate.Text = udtformatter.convertDate(txtIIRCheckingDate.Text, "")
            lblIIRAnomalous.Text = rdoIIAnomalousClaim.SelectedItem.Text + IIf(rdoIIAnomalousClaim.SelectedValue = "Y", joinParenthesis(Me.GetGlobalResourceObject("Text", "NoOfRecord") + " " + txtIINoofAnomalousClaim.Text), "")
            lblIIROverMajor.Text = rdoIIROverMajor.SelectedItem.Text + IIf(rdoIIROverMajor.SelectedValue = "Y", joinParenthesis(Me.GetGlobalResourceObject("Text", "NoOfRecord") + " " + txtIINoofIsOverMajor.Text), "")

            ' Further Action Data Collection
            Dim AList As New List(Of FurtherActionItem)
            Dim IList As New List(Of FurtherActionItem)
            Dim RList As New List(Of FurtherActionItem)

            'Issue Letter
            If Not String.IsNullOrEmpty(dateIIRAletter.Text) Then
                IList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "AdvisoryLetter"), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(udtInspectionRecordBLL.ConvertDate(dateIIRAletter.Text))})
            End If
            If Not String.IsNullOrEmpty(dateIIRWletter.Text) Then
                IList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "WarningLetter"), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(udtInspectionRecordBLL.ConvertDate(dateIIRWletter.Text))})
            End If
            If Not String.IsNullOrEmpty(dateIIRDletter.Text) Then
                IList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "DelistLetter"), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(udtInspectionRecordBLL.ConvertDate(dateIIRDletter.Text))})
            End If
            If Not String.IsNullOrEmpty(dateIIRSPletter.Text) Then
                IList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "SuspendPaymentLetter"), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(udtInspectionRecordBLL.ConvertDate(dateIIRSPletter.Text))})
            End If
            If Not String.IsNullOrEmpty(dateIIRSEAletter.Text) Then
                IList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "SuspendEHCPAccountLetter"), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(udtInspectionRecordBLL.ConvertDate(dateIIRSEAletter.Text))})
            End If
            If Not String.IsNullOrEmpty(dateIIROther.Text) Then
                IList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "Others") + "(" + txtIIROthers.Text + ")", .ActionDate = udtInspectionRecordBLL.FormatOutputDate(udtInspectionRecordBLL.ConvertDate(dateIIROther.Text))})
            End If

            'Refer Parties
            If Not String.IsNullOrEmpty(dateIIRBoard.Text) Then
                RList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "BoardAndCouncil"), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(udtInspectionRecordBLL.ConvertDate(dateIIRBoard.Text))})
            End If
            If Not String.IsNullOrEmpty(dateIIRPolice.Text) Then
                RList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "Police"), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(udtInspectionRecordBLL.ConvertDate(dateIIRPolice.Text))})
            End If
            If Not String.IsNullOrEmpty(dateIIRSWDepart.Text) Then
                RList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "SocialWelfareDepartment"), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(udtInspectionRecordBLL.ConvertDate(dateIIRSWDepart.Text))})
            End If
            If Not String.IsNullOrEmpty(dateIIRHKCAEDepart.Text) Then
                RList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "HKCustomsAndExciseDepartment"), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(udtInspectionRecordBLL.ConvertDate(dateIIRHKCAEDepart.Text))})
            End If
            If Not String.IsNullOrEmpty(dateIIRIDepart.Text) Then
                RList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "ImmigrationDepartment"), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(udtInspectionRecordBLL.ConvertDate(dateIIRIDepart.Text))})
            End If
            If Not String.IsNullOrEmpty(dateIIRLDepart.Text) Then
                RList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "LabourDepartment"), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(udtInspectionRecordBLL.ConvertDate(dateIIRLDepart.Text))})
            End If
            If Not String.IsNullOrEmpty(dateIIROthers.Text) Then
                RList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "Others") + joinParenthesis(txtIIROtherTP.Text), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(udtInspectionRecordBLL.ConvertDate(dateIIROthers.Text))})
            End If

            'Action to EHCP
            If Not String.IsNullOrEmpty(dateIIRSuspendEHCP.Text) Then
                AList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "SuspendTheEHCPFromHCVS"), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(udtInspectionRecordBLL.ConvertDate(dateIIRSuspendEHCP.Text))})
            End If
            If Not String.IsNullOrEmpty(dateIIRDelist.Text) Then
                AList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "DelistTheEHCP"), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(udtInspectionRecordBLL.ConvertDate(dateIIRDelist.Text))})
            End If
            If Not String.IsNullOrEmpty(dateIIRRecovery.Text) Then
                AList.Add(New FurtherActionItem With {.ActionType = Me.GetGlobalResourceObject("Text", "RecoveryOrSuspensionPayment"), .ActionDate = udtInspectionRecordBLL.FormatOutputDate(udtInspectionRecordBLL.ConvertDate(dateIIRRecovery.Text))})
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

            FurtherActionConfirm.DataSource = repeatDt
            FurtherActionConfirm.DataBind()
            If (repeatDt.Rows.Count > 0) Then
                lblFurtherActionConfirmEmpty.Visible = False
                headerFurtherActionConfirm.Visible = True
            Else
                lblFurtherActionConfirmEmpty.Visible = True
                headerFurtherActionConfirm.Visible = False
            End If
            repeatDt = udtInspectionRecordBLL.GetValidFollowActionFromInput(repFollowUpAction)
            repFollowUpActionConfirm.DataSource = repeatDt
            repFollowUpActionConfirm.DataBind()

            repFollowUpAction.DataSource = repeatDt
            repFollowUpAction.DataBind()
            Session(SESS_FollowupAction_DataTable_Edit) = repeatDt

            If (repeatDt.Rows.Count > 0) Then
                lblFollowUpActionConfirmEmpty.Visible = False
                headerFollowUpActionConfirm.Visible = True
            Else
                lblFollowUpActionConfirmEmpty.Visible = True
                headerFollowUpActionConfirm.Visible = False
            End If
            MultiViewIRM.SetActiveView(vConfirmInspectionResult)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00051, AuditLogDesc.IRM_InspectionResultSave_Successful)
        End If
    End Sub
    'Back Function -> 3. Inspection Record - Detail Show
    Protected Sub ibtnIIRBack_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnIIRBack.Click
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00042, AuditLogDesc.IRM_InspectionResultBack_Button_Click)

        udcMsgBox.Visible = False
        udcInfoMsgBox.Visible = False
        MultiViewIRM.SetActiveView(vViewInspectionDetail)

    End Sub
    'Selected Value Change Function
    Protected Sub rdoIIAnomalousClaim_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim rdoList As RadioButtonList = sender
        imgrdoIIAnomalousClaimErr.Visible = False
        imgtxtIINoofAnomalousClaimErr.Visible = False
        If (rdoList.SelectedValue = "Y") Then
            txtIINoofAnomalousClaim.Enabled = True
        Else
            txtIINoofAnomalousClaim.Text = ""
            txtIINoofAnomalousClaim.Enabled = False
        End If
    End Sub
    Protected Sub rdoIIROverMajor_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim rdoList As RadioButtonList = sender
        imgrdoIIROverMajorErr.Visible = False
        imgtxtIINoofIsOverMajorErr.Visible = False
        If (rdoList.SelectedValue = "Y") Then
            txtIINoofIsOverMajor.Enabled = True
        Else
            txtIINoofIsOverMajor.Text = ""
            txtIINoofIsOverMajor.Enabled = False
        End If
    End Sub
#End Region
#Region "       5.2 Edit Result - Confirm"
    'Fill Data
    Private Sub GetVisitDetailForInputInspectionResultConfirm()
        'Get value from the View vViewInspectionDetail
        lblIIRFileNoConfirm.Text = lblIIRFileNo.Text
        lblIIRInspectionIDConfirm.Text = lblIIRInspectionID.Text
        lblIIRSPIDConfirm.Text = lblIIRSPID.Text
        lblIIRSPNameConfirm.Text = lblIIRSPName.Text
        lblIIRPracticeConfirm.Text = lblIIRPractice.Text
        lblIIRPracticeChiConfirm.Text = lblIIRPracticeChi.Text
        lblIIRPracticeAddressConfirm.Text = lblIIRPracticeAddress.Text
        lblIIRPracticeAddressChiConfirm.Text = lblIIRPracticeAddressChi.Text
        lblIIRMainTypeofInspectionConfirm.Text = lblIIRMainTypeofInspection.Text
        lblIIRTypeofInspectionConfirm.Text = lblIIRTypeofInspection.Text
        lblIIRVisitDateConfirm.Text = lblIIRVisitDate.Text
        lblIIRVisitTimeConfirm.Text = lblIIRVisitTime.Text
    End Sub
    'Confirm -> Success Page
    Protected Sub ibtnIIRConfirmSave_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnIIRConfirmSave.Click
        udcInfoMsgBox.Visible = False
        udcMsgBox.Visible = False

        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00053, AuditLogDesc.IRM_IRConfirmSave_Click)

        Dim model As InspectionRecordModel = CType(Session(SESS_InspectionRecordModel), InspectionRecordModel)
        Dim xmlFollowUp As String = String.Empty
        Dim followUpDT As DataTable = Session(SESS_FollowupAction_DataTable_Edit)
        followUpDT.Columns.Remove("Action_Date_Format")
        followUpDT.Columns.Remove("Action_Date_Value")

        If (followUpDT.Rows.Count = 0) Then
            followUpDT = Nothing
        End If
        xmlFollowUp = udtInspectionRecordBLL.DataTableToXml(followUpDT, "FollowupAction")
        'Action - Issue Letter
        Dim AdvisoryLetterDate As Date = udtInspectionRecordBLL.ConvertDate(dateIIRAletter.Text)
        Dim WarningLetterDate As Date = udtInspectionRecordBLL.ConvertDate(dateIIRWletter.Text)
        Dim DelistLetterDate As Date = udtInspectionRecordBLL.ConvertDate(dateIIRDletter.Text)
        Dim SuspendPaymentLetterDate As Date = udtInspectionRecordBLL.ConvertDate(dateIIRSPletter.Text)
        Dim SuspendEHCPAccountLetterDate As Date = udtInspectionRecordBLL.ConvertDate(dateIIRSEAletter.Text)
        Dim OtherLetterDate As Date = udtInspectionRecordBLL.ConvertDate(dateIIROther.Text)

        'Action - Refer Parties
        Dim BoardAndCouncilDate As Date = udtInspectionRecordBLL.ConvertDate(dateIIRBoard.Text)
        Dim PoliceDate As Date = udtInspectionRecordBLL.ConvertDate(dateIIRPolice.Text)
        Dim SocialWelfareDepartmentDate As Date = udtInspectionRecordBLL.ConvertDate(dateIIRSWDepart.Text)
        Dim HKCustomsandExciseDepartmentDate As Date = udtInspectionRecordBLL.ConvertDate(dateIIRHKCAEDepart.Text)
        Dim ImmigrationDepartmentDate As Date = udtInspectionRecordBLL.ConvertDate(dateIIRIDepart.Text)
        Dim LabourDeparmentDate As Date = udtInspectionRecordBLL.ConvertDate(dateIIRLDepart.Text)
        Dim OtherPartyDate As Date = udtInspectionRecordBLL.ConvertDate(dateIIROthers.Text)

        'Action - Actions to EHCP
        Dim SuspendEHCPDate As Date = udtInspectionRecordBLL.ConvertDate(dateIIRSuspendEHCP.Text)
        Dim DelistEHCPDate As Date = udtInspectionRecordBLL.ConvertDate(dateIIRDelist.Text)
        Dim PaymentRecoverySuspensionDate As Date = udtInspectionRecordBLL.ConvertDate(dateIIRRecovery.Text)

        ' Integer
        Dim intNoOfInOrder As Integer = udtInspectionRecordBLL.ConvertStringToInt(txtIIRInOrder.Text)
        Dim intNoOfMissingForm As Integer = udtInspectionRecordBLL.ConvertStringToInt(txtIIRMissingForm.Text)
        Dim intNoOfInconsistent As Integer = udtInspectionRecordBLL.ConvertStringToInt(txtIIRInconsistent.Text)
        Dim intNoOfTotalCheck As Integer = intNoOfMissingForm + intNoOfInconsistent + intNoOfInOrder
        Dim intNoOfAnomalousClaims As Integer = udtInspectionRecordBLL.ConvertStringToInt(txtIINoofAnomalousClaim.Text)
        Dim intNoOfIsOverMajor As Integer = udtInspectionRecordBLL.ConvertStringToInt(txtIINoofIsOverMajor.Text)

        With model
            .FileReferenceNo = model.FileReferenceNo
            .InspectionID = model.InspectionID
            .NoOfInOrder = intNoOfInOrder
            .NoOfMissingForm = intNoOfMissingForm
            .NoOfInconsistent = intNoOfInconsistent
            .NoOfTotalCheck = intNoOfTotalCheck
            .AnomalousClaims = rdoIIAnomalousClaim.SelectedValue.Trim
            .NoOfAnomalousClaims = intNoOfAnomalousClaims
            .CheckingDate = IIf(txtIIRCheckingDate.Text.Trim = "", DateTime.MinValue, udtformatter.convertDate(Me.txtIIRCheckingDate.Text.Trim, String.Empty))
            .IsOverMajor = rdoIIROverMajor.SelectedValue.Trim
            .NoOfIsOverMajor = intNoOfIsOverMajor
            .AdvisoryLetterDate = AdvisoryLetterDate
            .WarningLetterDate = WarningLetterDate
            .DelistLetterDate = DelistLetterDate
            .SuspendPaymentLetterDate = SuspendPaymentLetterDate
            .SuspendEHCPAccountLetterDate = SuspendEHCPAccountLetterDate
            .OtherLetterDate = OtherLetterDate
            .BoardAndCouncilDate = BoardAndCouncilDate
            .PoliceDate = PoliceDate
            .SocialWelfareDepartmentDate = SocialWelfareDepartmentDate
            .HKCustomsandExciseDepartmentDate = HKCustomsandExciseDepartmentDate
            .ImmigrationDepartmentDate = ImmigrationDepartmentDate
            .LabourDeparmentDate = LabourDeparmentDate
            .OtherPartyDate = OtherPartyDate
            .SuspendEHCPDate = SuspendEHCPDate
            .DelistEHCPDate = DelistEHCPDate
            .PaymentRecoverySuspensionDate = PaymentRecoverySuspensionDate
            .FollowupAction = xmlFollowUp
            .OtherLetterRemark = txtIIROthers.Text.Trim
            .OtherPartyRemark = txtIIROtherTP.Text.Trim
            .UserID = udtHCVUUserBLL.GetHCVUUser.UserID
            .RecordStatus = InspectionStatus.InspectionResultInputted
            .SPID = lblIIRSPIDConfirm.Text
            .PracticeDisplaySeq = hdfIIRPracticeSeq.Value
        End With
        'UpdateInspectionResult
        If udtInspectionRecordBLL.UpdateRecord(model, "UpdateInspectionResult") Then
            ShowSuccessMessage(InfoMessageBoxUpdateIIRSuccess, Common.Component.MsgCode.MSG00003, model.InspectionID, model.FileReferenceNo)

            MultiViewIRM.SetActiveView(vViewUpdateIIRSuccess)

            udtAuditLogEntry.AddDescripton("Inspection ID", model.InspectionID)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00054, AuditLogDesc.IRM_IRConfirmSave_Successful)
        Else
            Me.udcMsgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
            Me.udcMsgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00060, AuditLogDesc.IRM_IRConfirmSave_Fail)
        End If
    End Sub
    'Back -> 5.1 Edit Result - Detail
    Protected Sub ibtnIIRConfirmBack_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnIIRConfirmBack.Click
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00043, AuditLogDesc.IRM_ResultConfirmPageBack_Button_Click)

        MultiViewIRM.SetActiveView(vInputInspectionResult)
        Me.udcMsgBox.Visible = False
        Me.udcInfoMsgBox.Visible = False

        Dim dtFollowupAction As DataTable = Session(SESS_FollowupAction_DataTable_Edit)
        If IsNothing(dtFollowupAction) Then
            dtFollowupAction = udtInspectionRecordBLL.GetFollowUpActionStructure()
        End If
        If dtFollowupAction.Rows.Count = 0 Then
            Dim dr As DataRow = dtFollowupAction.NewRow
            dr("Followup_Action_Seq") = 1
            dtFollowupAction.Rows.Add(dr)
        End If
        repFollowUpAction.DataSource = dtFollowupAction
        repFollowUpAction.DataBind()
    End Sub
#End Region
#Region "       6 Print"
    'Report
    Protected Sub ibtnPdf_Click(sender As Object, e As ImageClickEventArgs) Handles ibtPdf.Click
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        Me.udtAuditLogEntry.AddDescripton("Selection of Report", rblPrintContent.SelectedItem.Text)

        Me.udtAuditLogEntry.WriteStartLog(LogID.LOG00007, AuditLogDesc.IRM_Print_PDF_Button_Click)

        Me.mpeDownload.Show()
        lblReportName.Text = ""
        lblReportType.Text = ""

        udcDownloadErrorMessage.Clear()

        Dim strInspectionID As String = lblDetInspectionID.Text
        Dim report As String = rblPrintContent.SelectedValue
        Dim showType As String = rblPrintContent.SelectedItem.ToString
        Dim fileRefNum As String = lblDetFileNo.Text.ToString.Replace("/", "_")



        lblReportType.Text = showType
        lblReportName.Text = report + "_" + fileRefNum + ".pdf"

        txtNewPassword.Focus()

        'ScriptManager.RegisterStartupScript(Me, Page.GetType, String.Empty, "javascript:openNewWin('irmInspectionPrintOutViewer.aspx?rpt=" + report + "&inspectionid=" + Me.Server.UrlEncode(strInspectionID) + "&prtType=pdf')", True)
    End Sub
    Protected Sub ibtnWord_Click(sender As Object, e As ImageClickEventArgs) Handles ibtWord.Click
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        Me.udtAuditLogEntry.AddDescripton("Selection of Report", rblPrintContent.SelectedItem.Text)

        Me.udtAuditLogEntry.WriteStartLog(LogID.LOG00011, AuditLogDesc.IRM_Print_WORD_Button_Click)

        Me.mpeDownload.Show()

        lblReportName.Text = ""
        lblReportType.Text = ""

        udcDownloadErrorMessage.Clear()

        Dim strInspectionID As String = lblDetInspectionID.Text
        Dim report As String = rblPrintContent.SelectedValue
        Dim showType As String = rblPrintContent.SelectedItem.ToString
        Dim fileRefNum As String = lblDetFileNo.Text.ToString.Replace("/", "_")

        Me.udtAuditLogEntry.AddDescripton("rpt", report)

        lblReportType.Text = showType
        lblReportName.Text = report + "_" + fileRefNum + ".docx"

        txtNewPassword.Focus()

    End Sub
    Protected Sub ibtnCancel_Click(sender As Object, e As ImageClickEventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00012, AuditLogDesc.IRM_Print_Cancel_Button_Click)

    End Sub
    'Download
    Protected Sub ibtnDownload_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnDownload.Click

        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00071, AuditLogDesc.IRM_Download_Click)

        Try
            If Not IsEmpty(Me.txtNewPassword.Text) Then

                If ValidateFileDownloadPassword(Me.txtNewPassword.Text) Then

                    Dim downloadFileType = System.IO.Path.GetExtension(lblReportName.Text.ToString)
                    Dim strInspectionID As String = lblDetInspectionID.Text
                    Dim report As String = rblPrintContent.SelectedValue
                    Dim password As String = txtNewPassword.Text
                    GenerateReport(report, strInspectionID, downloadFileType, password)

                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00072, AuditLogDesc.IRM_Download_Click_Successful)

                Else
                    udcDownloadErrorMessage.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00028, "%s", "correct format password")
                    Me.udcDownloadErrorMessage.BuildMessageBox("DownloadFail", udtAuditLogEntry, LogID.LOG00073, AuditLogDesc.IRM_Download_Click_Fail & Session("PathError"))
                    Me.mpeDownload.Show()
                End If
            Else
                udcDownloadErrorMessage.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00028, "%s", "correct format password")
                Me.udcDownloadErrorMessage.BuildMessageBox("DownloadFail", udtAuditLogEntry, LogID.LOG00073, AuditLogDesc.IRM_Download_Click_Fail & Session("PathError"))
                Me.mpeDownload.Show()
            End If

        Catch ex As Exception
            ErrorHandler.Log(udtAuditLogEntry.FunctionCode, SeverityCode.SEVE, "99999", _
                             Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, ex.ToString)

            udtAuditLogEntry.AddDescripton("DownloadFailException", ex.ToString)
            Me.udcDownloadErrorMessage.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00447)
            Me.udcDownloadErrorMessage.BuildMessageBox("DownloadFail", udtAuditLogEntry, LogID.LOG00073, AuditLogDesc.IRM_Download_Click_Fail & Session("PathError"))
            Me.mpeDownload.Show()
        End Try

    End Sub
    Protected Sub ibtnDownloadClose_Click(sender As Object, e As ImageClickEventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00041, AuditLogDesc.IRM_DownloadReportClose_Button_Click)
    End Sub
    Public Sub GenerateReport(RptType As String, InspectionID As String, downloadFileType As String, password As String)
        ' Load
        udtAuditLogEntry = New AuditLogEntry(FunctionCode)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00001, AuditLogDesc.IRM_Generate_Report_Load)

        Dim printType As String = IIf(String.Compare(downloadFileType, ".docx") = 0, "word", "pdf")

        udtAuditLogEntry.AddDescripton("rpt", RptType)
        udtAuditLogEntry.AddDescripton("inspectionid", InspectionID)
        udtAuditLogEntry.AddDescripton("prtType", printType)

        Dim dt As System.Data.DataTable = udtInspectionRecordBLL.GetInspectionRecordByID_ForReport(InspectionID)
        Dim fileRefNo As String
        Dim serviceCode As String
        'Store result table
        udtAuditLogEntry.WriteEndLog(LogID.LOG00001, AuditLogDesc.IRM_Generate_Report_Successful)
        Dim xmlData As String = udtInspectionRecordBLL.DataTableToXml(dt, "inspectionVisitInfo")
        Dim index As Integer = 1
        Dim limit As Integer = 950
        Do
            If (xmlData.Length < limit) Then
                limit = xmlData.Length
            End If
            udtAuditLogEntry.AddDescripton(String.Format("({0})", index.ToString()), xmlData.Substring(0, limit))
            udtAuditLogEntry.WriteEndLog(FunctionCode, "ReportData")
            index += 1
            If limit = 950 Then
                xmlData = xmlData.Substring(950)
            Else
                xmlData = ""
            End If
        Loop While xmlData.Length <> 0

        fileRefNo = getFileReferenceNo(dt)
        serviceCode = getServiceCategoryCode(dt)
        Dim fileName As String = RptType + "_" + fileRefNo


        'Copy a new file from template
        Dim newFile As String = String.Empty
        newFile = fileName + ".docx"
        newFile = CopyNewFile(newFile, RptType, serviceCode)

        fileName += IIf(printType = "word", ".docx", ".pdf")
        'getset seesion Dictionary_Timestamp_Path object
        Dim dictTSPath As Dictionary(Of String, String)

        If Session(SESS_dictionaryTimeStampSessKey) Is Nothing Then
            dictTSPath = New Dictionary(Of String, String)
        Else
            dictTSPath = Session(SESS_dictionaryTimeStampSessKey)
        End If

        dataTableImprove(dt)
        'Exten the datatable for special case

        Dim lngTimeStamp As Long = DateTime.Now.Subtract(New DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local)).TotalMilliseconds
        Dim strTimeStamp As String = lngTimeStamp.ToString

        dictTSPath.Add(strTimeStamp, newFile + "," + fileName)
        Session(SESS_dictionaryTimeStampSessKey) = dictTSPath
        'Generate report
        GenerateWord2(dt, newFile, printType)

        EncrytFile(newFile, printType, fileRefNo, password)

        ScriptManager.RegisterStartupScript(Me, Page.GetType, String.Empty, "javascript:openNewWin('irmInspectionPrintOutViewer.aspx?SessDicObjKey=" + Me.Server.UrlEncode(strTimeStamp) + "')", True)

        udtAuditLogEntry.WriteEndLog(LogID.LOG00001, AuditLogDesc.IRM_Generate_Report_Successful)
    End Sub

    'Update dataTable for improve report and special case
    Private Function dataTableImprove(dataTable As System.Data.DataTable)

        If dataTable.Rows.Count > 0 Then

            'Case Officer Chi Name
            If dataTable.Columns.Contains("Case_Officer_ChiName_Value") Then
                If dataTable.Rows(0)("Case_Officer_ChiName_Value").ToString = String.Empty Then
                    ' if empty, show eng name
                    dataTable.Rows(0)("Case_Officer_ChiName") = dataTable.Rows(0)("Case_Officer_Name").ToString
                End If
            End If

            ' SP Chinese Name
            If dataTable.Columns.Contains("SP_Chi_Name") Then
                If dataTable.Rows(0)("SP_Chi_Name").ToString = String.Empty Then
                    dataTable.Rows(0)("SP_Chi_Name") = dataTable.Rows(0)("SP_Eng_Name").ToString
                End If
            End If

            ' Practice Name
            If dataTable.Columns.Contains("Practice_Name_Chi") Then
                If dataTable.Rows(0)("Practice_Name_Chi").ToString = String.Empty Then
                    dataTable.Rows(0)("Practice_Name_Chi") = dataTable.Rows(0)("Practice_Name").ToString
                End If
            End If

            ' Practice Address
            If dataTable.Columns.Contains("Practice_Address_Chi") Then
                If dataTable.Rows(0)("Practice_Address_Chi").ToString = String.Empty Then
                    dataTable.Rows(0)("Practice_Address_Chi") = dataTable.Rows(0)("Practice_Address").ToString
                End If
            End If

            'Calculate the number of claim consent form collected = number of in order + number of inconsistent
            If Not dataTable.Columns.Contains("No_Of_Collected") Then
                Dim no_of_inorder As Integer = 0
                Dim no_of_inconsistent As Integer = 0

                Integer.TryParse(dataTable.Rows(0)("No_Of_InOrder").ToString, no_of_inorder)
                Integer.TryParse(dataTable.Rows(0)("No_Of_Inconsistent").ToString, no_of_inconsistent)
                dataTable.Columns.Add("No_Of_Collected", GetType(Integer))
                dataTable.Rows(0)("No_Of_Collected") = no_of_inorder + no_of_inconsistent
            End If

            'Calculate the number of irregularities = number of MissingForm + number of inconsistent
            If Not dataTable.Columns.Contains("No_Of_Irregularities") Then
                Dim no_of_missingform As Integer = 0
                Dim no_of_inconsistent As Integer = 0

                Integer.TryParse(dataTable.Rows(0)("No_Of_MissingForm").ToString, no_of_missingform)
                Integer.TryParse(dataTable.Rows(0)("No_Of_Inconsistent").ToString, no_of_inconsistent)
                dataTable.Columns.Add("No_Of_Irregularities", GetType(Integer))
                dataTable.Rows(0)("No_Of_Irregularities") = no_of_missingform + no_of_inconsistent
            End If
            'Get System Parameter from generalfunction and add to datatable
            Dim systemParm = New String() {"ConfirmLetterAddress_Voucher_CHI", "ConfirmLetterAddress_Voucher_EN", "ConfirmLetterName_Voucher_CHI", "ConfirmLetterName_Voucher_EN"}
            Dim sysParamFunction As GeneralFunction = New GeneralFunction

            For i As Integer = 0 To UBound(systemParm)

                If Not dataTable.Columns.Contains(systemParm.ToString) Then
                    Dim systemParamValue As String = sysParamFunction.getSystemParameterValue1(systemParm(i))
                    dataTable.Columns.Add(systemParm(i), GetType(String))
                    dataTable.Rows(0)(systemParm(i)) = systemParamValue.Replace("<BR>", "∑")
                End If
            Next
        End If
    End Function
    Private Function getFileReferenceNo(dataTable As System.Data.DataTable) As String
        If dataTable.Rows.Count > 0 Then

            If dataTable.Columns.Contains("File_Reference_No") Then
                Return dataTable.Rows(0)("File_Reference_No").ToString.Replace("/", "_")
            End If

        End If
        Return ""
    End Function

    Private Function getServiceCategoryCode(dataTable As System.Data.DataTable) As String
        If dataTable.Rows.Count > 0 Then

            If dataTable.Columns.Contains("Service_Category_Code") Then

                If Not IsDBNull(dataTable.Rows(0)("Service_Category_Code")) Then
                    Return dataTable.Rows(0)("Service_Category_Code").ToString
                End If

            End If

        End If
        Return ""
    End Function

    Private Function CopyNewFile(newFile As String, rptType As String, serviceCode As String) As String
        Dim templateFilePath As String = "\PrintOut\InspectionReport\"
        Dim OutputFolder As String = "\DataDownload\"
        Dim InspectionInternalReferenceReport As String = "InternalReference.docx"
        Dim InspectionConfirmationLetterReport_chi As String = "ConfirmationLetter_chi.docx"
        Dim InspectionConfirmationLetterReport_eng As String = "ConfirmationLetter_eng.docx"
        Dim InspectionInspectionSummaryReport As String = "SummaryOfInspectionVisit.docx"

        templateFilePath = AppDomain.CurrentDomain.BaseDirectory + templateFilePath
        OutputFolder = AppDomain.CurrentDomain.BaseDirectory + OutputFolder
        Dim folderName As String = Guid.NewGuid().ToString().Replace("-", "")
        'Delete old file and folder
        Dim directory As New IO.DirectoryInfo(OutputFolder) '\DataDownload\
        For Each folder As IO.DirectoryInfo In directory.GetDirectories
            If (Now - folder.CreationTime).Days >= 2 Then
                Try
                    IO.Directory.Delete(folder.FullName, True)
                Catch ex As Exception

                End Try
            End If
        Next
        For Each file As IO.FileInfo In directory.GetFiles
            If (Now - file.CreationTime).Days >= 2 Then
                Try
                    file.Delete()
                Catch ex As Exception

                End Try
            End If
        Next

        OutputFolder += folderName
        IO.Directory.CreateDirectory(OutputFolder)

        newFile = OutputFolder + "\" + newFile
        Select Case rptType
            Case InspectionReportType.InternalReference
                templateFilePath += InspectionInternalReferenceReport
                rptType = "InternalReference"
            Case InspectionReportType.ConfirmationLetter
                If String.Compare(serviceCode.Trim, "RCM") = 0 Then
                    templateFilePath += InspectionConfirmationLetterReport_chi
                Else
                    templateFilePath += InspectionConfirmationLetterReport_eng
                End If
                rptType = "ConfirmationLetter"
            Case InspectionReportType.InspectionSummary
                templateFilePath += InspectionInspectionSummaryReport
                rptType = "SummaryOfInspectionVisit"
        End Select
        File.Copy(templateFilePath, newFile)

        Return newFile
    End Function
    'modified by Raiman
    Private Sub GenerateWord2(dataTable As System.Data.DataTable, filePath As String, printType As String)
        'Dim dataTable As System.Data.DataTable = GetDataSourceForInternalReport()
        Dim oMissing As Object = Missing.Value
        Dim oWord As Word.Application
        Dim aDoc As Word.Document

        Try
            'Start Word and open the document template.
            oWord = CreateObject("Word.Application")
            Dim myTempFile As String = filePath

            aDoc = oWord.Documents.Open(filePath, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing)

            Dim reg As Regex = New Regex("({#(.[^#]*)#})")
            Dim mc As MatchCollection

            mc = reg.Matches(aDoc.Range.Text)

            For k As Integer = 0 To mc.Count - 1

                Dim fieldName As String = mc(k).Value.Replace("{#", "").Replace("#}", "")

                If dataTable.Columns.Contains(fieldName) Then
                    If dataTable.Rows.Count > 0 Then
                        If dataTable.Rows(0)(fieldName) IsNot DBNull.Value Then
                            oWord.Selection.Find.ClearFormatting()
                            oWord.Selection.Find.Replacement.ClearFormatting()

                            ' Split and repalce string for every 100 chars as cannot replace at once if string > 255 chars
                            Dim strReplace As String = dataTable.Rows(0)(fieldName).ToString
                            Do
                                Dim subLen = IIf(strReplace.Length > 100, 100, strReplace.Length)
                                Dim strTarget = IIf(strReplace.Length > 100, mc(k).Value, "")
                                Dim txt As String = strReplace.Substring(0, subLen) + strTarget
                                strReplace = strReplace.Substring(subLen)

                                ' Handle if user input special char "^" and handle new line for specified character "∑"
                                With oWord.Selection.Find
                                    .Text = mc(k).Value
                                    .Replacement.Text = txt.ToString.Replace("^", "^^").Replace("∑", "^p")
                                    .Forward = True
                                    .Format = False
                                    .MatchCase = False
                                    .MatchWholeWord = False
                                    .MatchWildcards = False
                                    .MatchSoundsLike = False
                                    .MatchAllWordForms = False
                                End With
                                oWord.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)
                            Loop While strReplace.Length > 0

                        Else
                            oWord.Selection.Find.ClearFormatting()
                            oWord.Selection.Find.Replacement.ClearFormatting()
                            With oWord.Selection.Find
                                .Text = mc(k).Value
                                .Replacement.Text = ""
                                .Forward = True
                                .Format = False
                                .MatchCase = False
                                .MatchWholeWord = False
                                .MatchWildcards = False
                                .MatchSoundsLike = False
                                .MatchAllWordForms = False
                            End With
                            oWord.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)
                        End If
                    End If
                Else
                    oWord.Selection.Find.ClearFormatting()
                    oWord.Selection.Find.Replacement.ClearFormatting()
                    With oWord.Selection.Find
                        .Text = mc(k).Value
                        .Replacement.Text = ""
                        .Forward = True
                        .Format = False
                        .MatchCase = False
                        .MatchWholeWord = False
                        .MatchWildcards = False
                        .MatchSoundsLike = False
                        .MatchAllWordForms = False
                    End With
                    oWord.Selection.Find.Execute(Replace:=Word.WdReplace.wdReplaceAll)

                End If
            Next



        Catch ex As Exception
            Throw ex
        End Try

        aDoc.Save()

        If String.Compare(printType, "pdf") = 0 Then
            Dim outputFilename As String = System.IO.Path.ChangeExtension(filePath, "pdf")

            If Not aDoc Is Nothing Then
                aDoc.ExportAsFixedFormat(outputFilename, Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF, False, Microsoft.Office.Interop.Word.WdExportOptimizeFor.wdExportOptimizeForOnScreen, Microsoft.Office.Interop.Word.WdExportRange.wdExportAllDocument, 0, 0, Microsoft.Office.Interop.Word.WdExportItem.wdExportDocumentContent, True, True, Microsoft.Office.Interop.Word.WdExportCreateBookmarks.wdExportCreateNoBookmarks, True, False, False)
            End If

        End If

        If String.Compare(printType, "pdf") = 0 Then
            Dim outputFilePathName As String = System.IO.Path.ChangeExtension(filePath, "pdf")
            If Not aDoc Is Nothing Then
                aDoc.ExportAsFixedFormat(outputFilePathName, Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF, False, Microsoft.Office.Interop.Word.WdExportOptimizeFor.wdExportOptimizeForOnScreen, Microsoft.Office.Interop.Word.WdExportRange.wdExportAllDocument, 0, 0, Microsoft.Office.Interop.Word.WdExportItem.wdExportDocumentContent, True, True, Microsoft.Office.Interop.Word.WdExportCreateBookmarks.wdExportCreateNoBookmarks, True, False, False)
            End If
        End If

        aDoc.Close(oMissing, oMissing, oMissing)
        aDoc = Nothing
        oWord.Quit(oMissing, oMissing, oMissing)
        oWord = Nothing
    End Sub
    Private Sub EncrytFile(filePath As String, printType As String, fileRefNo As String, password As String)
        Try
            If String.Compare(printType, "word") = 0 Then
                Dim oMissing As Object = Missing.Value
                Dim oWord As Word.Application = CreateObject("Word.Application")
                Dim myTempFile As String = filePath
                Dim aDoc As Word.Document = oWord.Documents.Open(filePath, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing)

                aDoc.Password = password

                aDoc.Save()
                aDoc.Close(oMissing, oMissing, oMissing)
                aDoc = Nothing
                oWord.Quit(oMissing, oMissing, oMissing)
                oWord = Nothing

            ElseIf String.Compare(printType, "pdf") = 0 Then

                'Get the pdf encryter object for setpassword
                Dim encryter As Common.Encryption.Encrypt = New Common.Encryption.Encrypt

                'Change fileRpath file type name from .docx to .pdf
                Dim outputFilePathWithFileName As String = System.IO.Path.ChangeExtension(filePath, "pdf")
                'Get the xxxxxx.pdf file name
                Dim pdfFileName As String = System.IO.Path.GetFileName(outputFilePathWithFileName)
                Dim pdfFolderPathOnly As String = System.IO.Path.GetDirectoryName(outputFilePathWithFileName) + "\"
                Debug.WriteLine(pdfFolderPathOnly)

                If Not encryter.EncryptPDF(password, pdfFolderPathOnly, pdfFileName) Then
                    Throw New Exception("PDF file cannot encryt")
                End If
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Function ValidateFileDownloadPassword(ByVal pwd As String) As Boolean
        Dim udtcomfunct As ComFunction.GeneralFunction = New ComFunction.GeneralFunction
        Dim intMinLength, intMaxLength, intRuleNum As Integer
        Dim strvalue1, strvalue2 As String
        strvalue1 = String.Empty
        strvalue2 = String.Empty
        udtcomfunct.getSystemParameter("FilePasswordLengthRange", strvalue1, strvalue2)
        intMinLength = CInt(strvalue1)
        intMaxLength = CInt(strvalue2)

        strvalue1 = String.Empty
        strvalue2 = String.Empty
        udtcomfunct.getSystemParameter("PasswordRuleNumber", strvalue1, strvalue2)
        intRuleNum = CInt(strvalue2)

        ' Replace [A-Z] with \p{Lu}, to allow for Unicode uppercase letters.
        Dim upper As New System.Text.RegularExpressions.Regex("[A-Z]")
        Dim lower As New System.Text.RegularExpressions.Regex("[a-z]")
        Dim number As New System.Text.RegularExpressions.Regex("[0-9]")

        ' Special is "none of the above".
        Dim special As New System.Text.RegularExpressions.Regex("[^a-zA-Z0-9]")

        Dim okCounter As Integer
        okCounter = 0

        ' Check the length.
        If Len(pwd) < intMinLength Then Return False
        If Len(pwd) > intMaxLength Then Return False

        ' Check for minimum number of occurrences.
        If upper.Matches(pwd).Count > 0 Then okCounter = okCounter + 1
        If lower.Matches(pwd).Count > 0 Then okCounter = okCounter + 1
        If number.Matches(pwd).Count > 0 Then okCounter = okCounter + 1
        If special.Matches(pwd).Count > 0 Then okCounter = okCounter + 1

        ' Passed all checks.
        Return (okCounter >= intRuleNum)

    End Function
    Public Function IsEmpty(ByVal StrOriField As String) As Boolean
        Dim blnres As Boolean
        blnres = False
        If StrOriField.Length > 0 Then
            blnres = False
        Else
            blnres = True
        End If
        Return blnres
    End Function
    'Export
    Protected Sub ibtnERDownloadNow_Click(sender As Object, e As ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00014, AuditLogDesc.IRM_DownloadNow_Button_Click)
        Session("FileGenerateID") = hfEGenerationID.Value
        RedirectHandler.ToURL((New Component.Menu.MenuBLL).GetURLByFunctionCode(FunctCode.FUNT010702))
    End Sub
    Protected Sub ibtnERDownloadLater_Click(sender As Object, e As ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00015, AuditLogDesc.IRM_DownloadLater_Button_Click)
    End Sub
    'Add onkeyDown event when press enter will Click download button
    Private Sub checkPasswordValidation()

        Dim _udtGeneralFunction As New GeneralFunction
        'Add client side event trigger
        Dim strvalue1 As String = String.Empty
        Dim strvalue2 As String = String.Empty

        _udtGeneralFunction.getSystemParameter("PasswordRuleNumber", strvalue1, strvalue2)

        Me.txtNewPassword.Attributes.Remove("onKeyUp")
        Me.txtNewPassword.Attributes.Add("onKeyUp", "checkPassword(this.value," & _
                                                                        "'" & CInt(strvalue2.Trim) & "'," & _
                                                                        "'" & CInt(strvalue2.Trim) & "'," & _
                                                                        "'strength1','strength2','strength3','progressBar'," & _
                                                                        "'" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthPoor") & "'," & _
                                                                        "'" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthModerate") & "'," & _
                                                                        "'" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthStrong") & "'," & _
                                                                        "'direction2','direction1');")

        Me.pnlDownload.Attributes.Add("onKeyDown", "clickDownloadButton(event)")
        Me.txtNewPassword.Attributes.Add("onKeyDown", "clickDownloadButton(event)")

    End Sub
#End Region
#Region "       7 Success"
    'Back -> 1. Search Page
    Protected Sub ibtnMsgBoxBack_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnMsgBoxBack.Click
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00026, AuditLogDesc.IRM_CompletePageBack_Button_Click)
        MultiViewIRM.SetActiveView(vSEARCH)
    End Sub
    'Back -> 3. Inspection Record - Detail Show
    Protected Sub ibtnReturnIIRSuccess_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnReturnIIRSuccess.Click
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00028, AuditLogDesc.IRM_CompletePageReturn_Button_Click)

        Dim inspectionRecord As InspectionRecordModel = CType(Session(SESS_InspectionRecordModel), InspectionRecordModel)
        GetDetail(inspectionRecord.InspectionID)
    End Sub
#End Region
#End Region

#Region "Base Flow"
    Protected Overrides Sub SF_InitSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry)
    End Sub

    Protected Overrides Function SF_ValidateSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry) As Boolean
        Dim blnReturn As Boolean = True
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        udtAuditLogEntry.AddDescripton("Inspection Record", Me.rdlOwner.SelectedItem.Text)
        udtAuditLogEntry.AddDescripton("Inspection ID", Me.txtInspectionID.Text)
        udtAuditLogEntry.AddDescripton("File Reference No.", Me.txtFileReferenceNo.Text)
        udtAuditLogEntry.AddDescripton("Service Provider ID", Me.txtSPID.Text)
        udtAuditLogEntry.AddDescripton("Visit Date From", Me.txtStartVisitDate.Text)
        udtAuditLogEntry.AddDescripton("Visit Date To", Me.txtEndVisitDate.Text)
        udtAuditLogEntry.AddDescripton("Main Type of Inspection", Me.ddlTypeofInspection.SelectedValue)
        udtAuditLogEntry.AddDescripton("Status", Me.ddlStatus.SelectedValue)


        Dim sm As SystemMessage = Me.udtValidator.chkSPID(Me.txtSPID.Text.Trim)
        If sm IsNot Nothing Then
            blnReturn = False
            Me.udcMsgBox.AddMessage(sm)
        End If

        Dim sm1 As Common.ComObject.SystemMessage = Nothing
        Dim sm2 As Common.ComObject.SystemMessage = Nothing

        Dim strVisitDateFrom = IIf(udtformatter.formatInputDate(txtStartVisitDate.Text.Trim) <> String.Empty, udtformatter.formatInputDate(txtStartVisitDate.Text.Trim), txtStartVisitDate.Text.Trim)
        Dim strVisitDateTo = IIf(udtformatter.formatInputDate(txtEndVisitDate.Text.Trim) <> String.Empty, udtformatter.formatInputDate(txtEndVisitDate.Text.Trim), txtEndVisitDate.Text.Trim)

        If Not String.IsNullOrWhiteSpace(strVisitDateFrom) Then
            sm1 = udtValidator.chkInputDate(strVisitDateFrom, False, False)
            If Not IsNothing(sm1) Then
                blnReturn = False
                Me.udcMsgBox.AddMessage(sm1, "%s", Me.GetGlobalResourceObject("Text", "VisitDate") + " From")
            End If
        End If

        If Not String.IsNullOrWhiteSpace(strVisitDateTo) Then
            sm2 = udtValidator.chkInputDate(strVisitDateTo, False, False)
            If Not IsNothing(sm2) Then
                blnReturn = False
                Me.udcMsgBox.AddMessage(sm2, "%s", Me.GetGlobalResourceObject("Text", "VisitDate") + " To")
            End If
        End If

        If IsNothing(sm1) And IsNothing(sm2) And strVisitDateFrom <> "" And strVisitDateTo <> "" Then
            sm1 = udtValidator.chkInputValidFromDateCutoffDate(CommonFunctionCode, MsgCode.MSG00374, udtformatter.convertDate(strVisitDateFrom, "E"), udtformatter.convertDate(strVisitDateTo, "E"))

            If Not IsNothing(sm1) Then
                blnReturn = False
                udcMsgBox.AddMessage(sm1, "%s", Me.GetGlobalResourceObject("Text", "VisitDate"))
            End If
        End If

        If Not blnReturn Then
            Me.udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00001, AuditLogDesc.IRM_SearchResult_Fail)
        Else
            txtStartVisitDate.Text = strVisitDateFrom
            txtEndVisitDate.Text = strVisitDateTo
        End If

        Return blnReturn
    End Function

    Protected Overrides Function SF_Search(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry, ByVal blnOverrideResultLimit As Boolean) As Common.Component.BaseBLL.BLLSearchResult
        Try
            Dim bllSearchResult As BaseBLL.BLLSearchResult = GetInspectionRecordList(blnOverrideResultLimit, False)
            Return bllSearchResult
        Catch ex As Exception
            Me.udtAuditLogEntry.AddDescripton("Error Message", ex.ToString)
            Me.udtAuditLogEntry.WriteEndLog(LogID.LOG00001, AuditLogDesc.IRM_SearchResult_Fail)
            Me.udcMsgBox.BuildMessageBox("SearchFail", udtAuditLogEntry, Common.Component.LogID.LOG00001, AuditLogDesc.IRM_SearchResult_Fail)
            Return Nothing
        End Try
    End Function

    Protected Overrides Function SF_BindSearchResult(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry, ByVal udtBLLSearchResult As Common.Component.BaseBLL.BLLSearchResult) As Integer
        Dim dt As New DataTable
        Dim intRowCount As Integer
        Dim blnShowResultList As Boolean = False
        Try
            dt = CType(udtBLLSearchResult.Data, DataTable)
            dt.DefaultView.Sort = "Visit_Date DESC"
            dt = dt.DefaultView.ToTable()

            'Save Session
            Session(SESS_SearchResultDataTable) = dt
        Catch ex As Exception
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
            Me.GridViewDataBind(Me.gvSearchResult, dt, "Visit_Date", "DESC", False)
        End If
        Return intRowCount
    End Function

    Protected Overrides Sub SF_FinalizeSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry)
    End Sub

    Protected Overrides Sub SF_ConfirmSearch_Click()
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00044, AuditLogDesc.IRM_Search_Click)

        Me.udcInfoMsgBox.Visible = False
        Me.udcMsgBox.Visible = False
        Dim enumSearchResult As SearchResultEnum 'Result Status        
        Try
            enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, udcMsgBox, udcInfoMsgBox, False, True)

            Select Case enumSearchResult
                Case SearchResultEnum.Success '0
                    MultiViewIRM.SetActiveView(vSearchResult)
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00045, AuditLogDesc.IRM_Search_Successful)
                Case Else
                    Throw New Exception("Error: Class = [HCVU.InspectionRecordManagement], Method = [SF_ConfirmSearch_Click], Message = The unexpected error for the result of re-search")
            End Select

        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00046, AuditLogDesc.IRM_Search_Fail)
        End Try
    End Sub

    Protected Overrides Sub SF_CancelSearch_Click()
    End Sub
#End Region

#Region "Common Function"
    'Init
    Private Sub SetOfficerList(dataField As DetailDataField)
        Dim typeofInspectionList As DataTable = udtInspectionRecordBLL.GetInspectionOfficerList()

        Dim builder As New StringBuilder
        Dim userID As String = udtHCVUUserBLL.GetHCVUUser.UserID
        For Each Item As DataRow In typeofInspectionList.Rows
            Dim strDisplayValue As String = String.Format("{0} - {1}{2}", Item("User_ID"), Item("Gender_Title"), Item("User_Name"))
            ' Set login user as 1st item
            If Item("User_ID").ToString().Trim = userID Then
                Dim newBuilder As New StringBuilder
                newBuilder.Append(String.Format("<option value='{0}'  no='{1}' key='{2}'>", strDisplayValue, Item("Contact_No"), Item("User_ID")))
                newBuilder.Append(builder)
                builder = newBuilder
            Else
                builder.Append(String.Format("<option value='{0}'  no='{1}' key='{2}'>", strDisplayValue, Item("Contact_No"), Item("User_ID")))
            End If
        Next
        dataField.officerList.InnerHtml = builder.ToString()

        dataField.txtCaseOfficer.Attributes.Add("list", dataField.officerList.ClientID)
        dataField.txtCaseOfficer.Attributes.Add("onKeyDown", "OfficerOnKeyDown(event)")
        dataField.txtCaseOfficer.Attributes.Add("onInput", "officerOnInput(this,'Case','" + dataField.pageMode + "')")
        dataField.txtCaseOfficer.Attributes.Add("onBlur", "OfficerLostFocus(this,'Case','" + dataField.pageMode + "')")

        dataField.txtSubjectOfficer.Attributes.Add("list", dataField.officerList.ClientID)
        dataField.txtSubjectOfficer.Attributes.Add("onKeyDown", "OfficerOnKeyDown(event)")
        dataField.txtSubjectOfficer.Attributes.Add("onInput", "officerOnInput(this,'Subject','" + dataField.pageMode + "')")
        dataField.txtSubjectOfficer.Attributes.Add("onBlur", "OfficerLostFocus(this,'Subject','" + dataField.pageMode + "')")

    End Sub
    Private Sub SetPrefixLabel()
        Dim prefixRefNo As String = udtGeneralFunction.getSystemParameter("Inspection_FileRefNo_Prefix")
        prefixInspectionRecordID.Text = udtGeneralFunction.getSeqNo_Prefix_Without_Update_ProfileNum("INSID", "ALL")
        prefixFileReferNo.Text = prefixRefNo
        lblRRNAPrefix.Text = prefixRefNo
        lblRRNBPrefix.Text = prefixRefNo
        lblRRNCPrefix.Text = prefixRefNo
        lblFRNPrefix.Text = prefixRefNo
        lblEdtRRNAPrefix.Text = prefixRefNo
        lblEdtRRNBPrefix.Text = prefixRefNo
        lblEdtRRNCPrefix.Text = prefixRefNo
    End Sub
    Private Sub SetTypeofInspectionRadioButtonList(ByVal rdoBtnList As RadioButtonList)
        Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL
        Dim dt As StaticDataModelCollection = udtStaticDataBLL.GetStaticDataListByColumnName("TypeOfInspection")

        rdoBtnList.Items.Clear()
        rdoBtnList.DataSource = dt
        rdoBtnList.DataTextField = "DataValue"
        rdoBtnList.DataValueField = "ItemNo"
        rdoBtnList.DataBind()
        rdoBtnList.CssClass = "typIns"

        Dim toiList As New List(Of TypeOfInspectionItem)
        Dim title As String = ""
        Dim count As Integer = 0
        Dim max As Integer = 0

        For Each Item As StaticDataModel In dt
            If (String.IsNullOrEmpty(title)) Then
                title = Item.DisplayOrder(0)
                count += 1
            Else
                If Item.DisplayOrder(0) <> title Then
                    toiList.Add(New TypeOfInspectionItem With {.Title = title, .Count = count})
                    If (count >= max) Then max = count
                    title = Item.DisplayOrder(0)
                    count = 1
                Else
                    count += 1
                End If
            End If
        Next
        toiList.Add(New TypeOfInspectionItem With {.Title = title, .Count = count})
        rdoBtnList.RepeatColumns = toiList.Count
        Dim index As Integer = 0
        For Each Item As TypeOfInspectionItem In toiList
            Dim no = max - Item.Count
            index += Item.Count
            For i As Integer = 1 To no
                rdoBtnList.Items.Insert(index, "")
                index += 1
            Next
        Next
        For Each item As ListItem In rdoBtnList.Items
            If (String.IsNullOrEmpty(item.Text)) Then
                item.Enabled = False
                item.Attributes.CssStyle.Add("display", "none")
            Else
                Dim typeOfInspectionItem As StaticDataModel = dt.Item("TypeOfInspection", item.Value)
                item.Attributes.Add("Type", typeOfInspectionItem.DisplayOrder(0))
            End If
        Next
    End Sub

    Private Sub SetTypeofInspectionCheckBoxList(ByVal chkBoxList As CheckBoxList)
        Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL
        Dim dt As StaticDataModelCollection = udtStaticDataBLL.GetStaticDataListByColumnName("TypeOfInspection")
        ' Remove Routine
        Dim routineItem As StaticDataModel = dt.Item("TypeOfInspection", TypeOfInspection.Routine)
        dt.remove(routineItem)

        chkBoxList.Items.Clear()
        chkBoxList.DataSource = dt
        chkBoxList.DataTextField = "DataValue"
        chkBoxList.DataValueField = "ItemNo"
        chkBoxList.DataBind()
        chkBoxList.CssClass = "typIns"

        Dim toiList As New List(Of TypeOfInspectionItem)
        Dim title As String = ""
        Dim count As Integer = 0
        Dim max As Integer = 0

        For Each Item As StaticDataModel In dt
            If (String.IsNullOrEmpty(title)) Then
                title = Item.DisplayOrder(0)
                count += 1
            Else
                If Item.DisplayOrder(0) <> title Then
                    toiList.Add(New TypeOfInspectionItem With {.Title = title, .Count = count})
                    If (count >= max) Then max = count
                    title = Item.DisplayOrder(0)
                    count = 1
                Else
                    count += 1
                End If
            End If
        Next
        toiList.Add(New TypeOfInspectionItem With {.Title = title, .Count = count})
        chkBoxList.RepeatColumns = toiList.Count
        Dim index As Integer = 0
        For Each Item As TypeOfInspectionItem In toiList
            Dim no = max - Item.Count
            index += Item.Count
            For i As Integer = 1 To no
                chkBoxList.Items.Insert(index, "")
                index += 1
            Next
        Next
        For Each item As ListItem In chkBoxList.Items
            If (String.IsNullOrEmpty(item.Text)) Then
                item.Enabled = False
                item.Attributes.CssStyle.Add("display", "none")
            End If
        Next
    End Sub
    Private Sub SetFileType(radioBtnList As RadioButtonList)
        Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL
        radioBtnList.Items.Clear()
        radioBtnList.DataSource = udtStaticDataBLL.GetStaticDataListByColumnName("InspectionFileType")
        radioBtnList.DataTextField = "DataValue"
        radioBtnList.DataValueField = "ItemNo"
        radioBtnList.DataBind()
        radioBtnList.SelectedValue = FileReferenceType.NewFile
    End Sub
    Private Sub SetFormCondition(ddList As DropDownList)
        Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL
        ddList.Items.Clear()
        ddList.DataSource = udtStaticDataBLL.GetStaticDataListByColumnName("FormCondition") '  ClaimCreationReason 
        ddList.DataTextField = "DataValue"
        ddList.DataValueField = "ItemNo"
        ddList.DataBind()
        ddList.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), ""))
        ddList.SelectedIndex = 0
    End Sub
    Private Sub SetMeansofCommunication(ddList As DropDownList)
        Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL
        ddList.Items.Clear()
        ddList.DataSource = udtStaticDataBLL.GetStaticDataListByColumnName("MeansOfCommunication") '  ClaimCreationReason 
        ddList.DataTextField = "DataValue"
        ddList.DataValueField = "ItemNo"
        ddList.DataBind()
        ddList.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), ""))
        ddList.SelectedIndex = 0
    End Sub
    Protected Sub initDropDownList(ddl As DropDownList)
        ddl.Items.Clear()
        ddl.Items.Add(New ListItem() With {.Text = Me.GetGlobalResourceObject("Text", "PleaseSelect"), .Value = ""})
        ddl.SelectedIndex = 0
    End Sub
    'Validation
    Private Sub HideErrorImg()
        imgAddFRNErr.Visible = False
        imgAddRRNAErr.Visible = False
        imgAddRRNBErr.Visible = False
        imgAddRRNCErr.Visible = False
        imgrdoAddLowRiskClaimErr.Visible = False
        imgrdoListAddMainTypeofInspectionErr.Visible = False

        imgchkListAddTypeofInspectionErr.Visible = False

        imgddlEditPracticeErr.Visible = False
        imgtxtEditSPIDErr.Visible = False
        imgtxtEdtSubjectOfficerErr.Visible = False
        imgddlEdtFormConditionErr.Visible = False
        imgddlEdtMeansofCommunicationErr.Visible = False
        imgtxtEdtCaseOfficerErr.Visible = False
        imgchkListEdtTypeofInspectionErr.Visible = False
        imgtxtEdtConfirmationWithErr.Visible = False
        imgtxtEdtConfirmDateErr.Visible = False
        imgtxtEdtCaseContactNoErr.Visible = False
        imgtxtEdtSubjectContactNoErr.Visible = False
        imgtxtEdtEndVisitTimeErr.Visible = False
        imgtxtEdtFormConditionRmErr.Visible = False
        imgtxtEdtRemarksErr.Visible = False
        imgtxtEdtStartVisitTimeErr.Visible = False
        imgtxtEdtVisitDateErr.Visible = False
        imgddlFormConditionErr.Visible = False
        imgddlMeansofCommunicationErr.Visible = False
        imgtxtMeansofCommunicationEmailErr.Visible = False
        imgtxtEdtMeansofCommunicationEmailErr.Visible = False
        ImgtxtMeansofCommunicationFaxErr.Visible = False
        imgtxtEdtMeansofCommunicationFaxErr.Visible = False
        imgtxtCaseOfficerErr.Visible = False

        imgEdtRRNAErr.Visible = False
        imgEdtRRNBErr.Visible = False
        imgEdtRRNCErr.Visible = False
        imgtxtFormConditionRmErr.Visible = False
        imgtxtConfirmationWithErr.Visible = False
        imgtxtConfirmDateErr.Visible = False

        imgtxtIIRCheckingDateErr.Visible = False
        imgtxtIIRInOrderErr.Visible = False
        imgtxtIIRMissingFormErr.Visible = False
        imgtxtIIRInconsistentErr.Visible = False
        imgtxtIINoofAnomalousClaimErr.Visible = False
        imgtxtIINoofIsOverMajorErr.Visible = False
        imgrdoIIAnomalousClaimErr.Visible = False
        imgrdoIIROverMajorErr.Visible = False

        imgtxtRemarksErr.Visible = False
        imgtxtSPIDNewErr.Visible = False
        imgtxtStartVisitTimeErr.Visible = False
        imgtxtVisitDateErr.Visible = False
        imgddlPracticeErr.Visible = False
        imgtxtEndVisitTimeErr.Visible = False
        imgtxtSubjectOfficerErr.Visible = False

    End Sub
    Public Sub CheckDetailValidation(ByRef blnNeedValidation As Boolean, ByRef blnError As Boolean, ByRef blnInComplete As Boolean, mode As String)
        Dim dataField As DetailDataField = GetDetailDataFieldByMode(mode)

        dataField.imgVisitDateErr.Visible = False
        dataField.imgtxtVisitTimeFromErr.Visible = False
        dataField.imgtxtVisitTimeToErr.Visible = False
        dataField.imgtxtConfirmationWithErr.Visible = False
        dataField.imgtxtConfirmDateErr.Visible = False
        dataField.imgddlFormConditionErr.Visible = False
        dataField.imgtxtFormConditionRmErr.Visible = False
        dataField.imgddlMeansofCommunicationErr.Visible = False
        dataField.imgtxtMeansOfCommunicationEmailErr.Visible = False
        dataField.imgtxtMeansOfCommunicationFaxErr.Visible = False
        dataField.imgtxtRemarksErr.Visible = False
        dataField.imgtxtCaseOfficerErr.Visible = False
        dataField.imgtxtCaseContactNoErr.Visible = False
        dataField.imgtxtSubjectOfficerErr.Visible = False
        dataField.imgtxtSubjectContactNoErr.Visible = False
        dataField.imgrdoLowRiskClaimErr.Visible = False

        Dim strVisitDate = IIf(udtformatter.formatInputDate(dataField.txtVisitDate.Text.Trim) <> String.Empty, udtformatter.formatInputDate(dataField.txtVisitDate.Text.Trim), dataField.txtVisitDate.Text.Trim)
        Dim chkVisitDateSM As SystemMessage = udtValidator.chkInputDate(strVisitDate, True, mode = PageMode.ModeEdit And hdnStatus.Value = InspectionStatus.InspectionResultInputted)
        Dim dateVisit As Date = Date.MinValue
        If dataField.txtVisitDate.Text.Trim <> "" Then
            If Not IsNothing(chkVisitDateSM) Then
                blnError = True
                dataField.imgVisitDateErr.Visible = True
                Me.udcMsgBox.AddMessage(chkVisitDateSM, "%s", GetGlobalResourceObject("Text", "VisitDate"))
            Else
                dateVisit = udtInspectionRecordBLL.ConvertDate(strVisitDate)
                dataField.txtVisitDate.Text = strVisitDate
                Dim dateFileReferenceNo As Date = Date.MinValue
                Select Case mode
                    Case PageMode.ModeNew
                        If Not String.IsNullOrEmpty(dataField.txtFileReferenceNo2.Text) And Not String.IsNullOrEmpty(dataField.txtFileReferenceNo3.Text) Then
                            If Not Date.TryParse("20" + dataField.txtFileReferenceNo2.Text + "-" + dataField.txtFileReferenceNo3.Text + "-01", dateFileReferenceNo) Then
                                dateFileReferenceNo = Date.MinValue
                            End If
                        End If
                    Case PageMode.ModeEdit
                        Dim inspectionRecord As InspectionRecordModel = CType(Session(SESS_InspectionRecordModel), InspectionRecordModel)
                        Dim strReferB As String = "", strReferC As String = ""
                        udtInspectionRecordBLL.SplitReferNo(inspectionRecord.FileReferenceNo, "", strReferB, strReferC, "", "")
                        dateFileReferenceNo = CDate("20" + strReferB + "-" + strReferC + "-01")
                End Select

                'Visit date should be >= 1st day of the month year of the file ref. no.
                If dateFileReferenceNo <> Date.MinValue And dateVisit < dateFileReferenceNo Then
                    blnError = True
                    dataField.imgVisitDateErr.Visible = True
                    udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00381, New String() {"%s", "%t"}, New String() {Me.GetGlobalResourceObject("Text", "VisitDate"), String.Format("The Year and Month of {0} ({1})", Me.GetGlobalResourceObject("Text", "FileReferenceNo"), dateFileReferenceNo.ToString("MMM yyyy"))})
                End If

                'Result inputted
                If hdnStatus.Value = InspectionStatus.InspectionResultInputted Then
                    Dim inspectionRecord As InspectionRecordModel = CType(Session(SESS_InspectionRecordModel), InspectionRecordModel)
                    If dateVisit <> Date.MinValue Then
                        'visit date should not be later than checking date
                        If inspectionRecord.CheckingDate <> Date.MinValue And dateVisit.Date > inspectionRecord.CheckingDate.Date Then
                            blnError = True
                            dataField.imgVisitDateErr.Visible = True
                            udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00382, New String() {"%s", "%t"}, New String() {Me.GetGlobalResourceObject("Text", "VisitDate"), Me.GetGlobalResourceObject("Text", "CheckingDate") + " (" + udtInspectionRecordBLL.FormatOutputDate(inspectionRecord.CheckingDate) + ")"})
                        End If
                        'visit date should not be later than follow up action date
                        Dim dtFollowUpAction As DataTable = udtInspectionRecordBLL.GetFollowupActionFromXML(inspectionRecord.FollowupAction)
                        If dtFollowUpAction.Rows.Count > 0 Then
                            For Each dr As DataRow In dtFollowUpAction.Rows
                                Dim dateAction As Date = udtInspectionRecordBLL.ConvertDate(dr("Action_Date").ToString())
                                If dateVisit.Date > dateAction.Date Then
                                    blnError = True
                                    dataField.imgVisitDateErr.Visible = True
                                    udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00382, New String() {"%s", "%t"}, New String() {Me.GetGlobalResourceObject("Text", "VisitDate"), String.Format("Date of {0}", Me.GetGlobalResourceObject("Text", "FollowUpAction")) + " (" + udtInspectionRecordBLL.FormatOutputDate(dateAction) + ")"})
                                    Exit For
                                End If
                            Next
                        End If
                    End If

                End If
            End If
        Else
            If blnNeedValidation Then
                blnError = True
                dataField.imgVisitDateErr.Visible = True
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00028, "%s", GetGlobalResourceObject("Text", "VisitDate"))
            End If
            blnInComplete = True
        End If

        'check Visit Time from - to
        Dim invalidVisitTimeFrom As Boolean = False, invalidVisitTimeTo As Boolean = False
        If dataField.txtVisitTimeFrom.Text.Trim = "" Then
            If blnNeedValidation Then
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00028, "%s", GetGlobalResourceObject("Text", "VisitTime") + " From")
                blnError = True
                dataField.imgtxtVisitTimeFromErr.Visible = True
                invalidVisitTimeFrom = True
            End If
            blnInComplete = True
        End If
        If dataField.txtVisitTimeTo.Text.Trim = "" Then
            If blnNeedValidation Then
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00028, "%s", GetGlobalResourceObject("Text", "VisitTime") + " To")
                blnError = True
                dataField.imgtxtVisitTimeToErr.Visible = True
                invalidVisitTimeTo = True
            End If
            blnInComplete = True
        End If

        If dataField.txtVisitTimeFrom.Text.Trim <> "" Or dataField.txtVisitTimeTo.Text.Trim <> "" Then
            Dim sdt = DateTime.Now
            Dim edt = DateTime.Now
            Dim startVali As Boolean = False
            Dim endVali As Boolean = False
            If dataField.txtVisitTimeFrom.Text.Trim = "" And Not invalidVisitTimeFrom Then
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00028, "%s", GetGlobalResourceObject("Text", "VisitTime") + " From")
                blnError = True
                dataField.imgtxtVisitTimeFromErr.Visible = True
            ElseIf Not DateTime.TryParse(DateTime.Now.ToString("yyyy-MM-dd ") + dataField.txtVisitTimeFrom.Text.Trim, sdt) Then
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00365, "%s", GetGlobalResourceObject("Text", "VisitTime") + " From")
                blnError = True
                dataField.imgtxtVisitTimeFromErr.Visible = True
            ElseIf Not invalidVisitTimeFrom Then
                startVali = True
            End If

            If dataField.txtVisitTimeTo.Text.Trim = "" And Not invalidVisitTimeTo Then
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00028, "%s", GetGlobalResourceObject("Text", "VisitTime") + " To")
                blnError = True
                dataField.imgtxtVisitTimeToErr.Visible = True
            ElseIf Not DateTime.TryParse(DateTime.Now.ToString("yyyy-MM-dd ") + dataField.txtVisitTimeTo.Text.Trim, edt) Then
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00365, "%s", GetGlobalResourceObject("Text", "VisitTime") + " To")
                blnError = True
                dataField.imgtxtVisitTimeToErr.Visible = True
            ElseIf Not invalidVisitTimeTo Then
                endVali = True
            End If
            If startVali And endVali Then
                If sdt > edt Then
                    blnError = True
                    dataField.imgtxtVisitTimeFromErr.Visible = True
                    dataField.imgtxtVisitTimeToErr.Visible = True
                    udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00382, New String() {"%s", "%t"}, New String() {Me.GetGlobalResourceObject("Text", "VisitTime") + " From", Me.GetGlobalResourceObject("Text", "VisitTime") + " To"})
                End If
            End If
        End If

        'check Confirmation With
        If dataField.txtConfirmationWith.Text.Trim = "" Then
            If blnNeedValidation Then
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00028, "%s", GetGlobalResourceObject("Text", "ConfirmationWith"))
                blnError = True
                dataField.imgtxtConfirmationWithErr.Visible = True
            End If
            blnInComplete = True
        End If

        'check Confirm Date
        Dim strConfirmDate As String = IIf(udtformatter.formatInputDate(dataField.txtConfirmDate.Text.Trim) <> String.Empty, udtformatter.formatInputDate(dataField.txtConfirmDate.Text.Trim), dataField.txtConfirmDate.Text.Trim)
        Dim chkConfirmDateSM = udtValidator.chkInputDate(strConfirmDate, True, False)
        Dim dateConfirm As Date = Date.MinValue
        If dataField.txtConfirmDate.Text.Trim <> "" Then
            If Not IsNothing(chkConfirmDateSM) Then
                blnError = True
                dataField.imgtxtConfirmDateErr.Visible = True
                Me.udcMsgBox.AddMessage(chkConfirmDateSM, "%s", GetGlobalResourceObject("Text", "ConfirmDate"))
            Else
                dataField.txtConfirmDate.Text = strConfirmDate
                dateConfirm = udtInspectionRecordBLL.ConvertDate(strConfirmDate)
                If dateConfirm.Date > Date.Now.Date Then
                    blnError = True
                    dataField.imgtxtConfirmDateErr.Visible = True
                    udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00022, "%s", Me.GetGlobalResourceObject("Text", "ConfirmDate"))
                End If
                If (dateVisit <> Date.MinValue And dateConfirm.Date > dateVisit) Then
                    blnError = True
                    dataField.imgVisitDateErr.Visible = True
                    dataField.imgtxtConfirmDateErr.Visible = True
                    udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00382, New String() {"%s", "%t"}, New String() {Me.GetGlobalResourceObject("Text", "ConfirmDate"), Me.GetGlobalResourceObject("Text", "VisitDate")})
                End If

            End If
        Else
            If blnNeedValidation Then
                blnError = True
                dataField.imgtxtConfirmDateErr.Visible = True
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00028, "%s", GetGlobalResourceObject("Text", "ConfirmDate"))
            End If
            blnInComplete = True
        End If

        'check Form Conditio,Condition Remarks
        If dataField.ddlFormCondition.SelectedValue = "" Then
            If blnNeedValidation Then
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00367, "%s", GetGlobalResourceObject("Text", "FormCondition"))
                blnError = True
                dataField.imgddlFormConditionErr.Visible = True
            End If
            blnInComplete = True
        ElseIf dataField.ddlFormCondition.SelectedValue = FormCondition.Others Then
            If dataField.txtFormConditionRm.Text.Trim = "" Then
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00028, "%s",
                                     GetGlobalResourceObject("Text", "FormCondition") + " " + GetGlobalResourceObject("Text", "Remarks"))
                blnError = True
                dataField.imgtxtFormConditionRmErr.Visible = True
                blnInComplete = True
            End If
        End If

        'check Means of Comunication
        If dataField.ddlMeansOfCommunication.SelectedValue = "" Then
            If blnNeedValidation Then
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00367, "%s", GetGlobalResourceObject("Text", "MeansOfCommunication"))
                blnError = True
                dataField.imgddlMeansofCommunicationErr.Visible = True
            End If
            blnInComplete = True
        ElseIf dataField.ddlMeansOfCommunication.SelectedValue = MeansofCommunication.Email Then
            If dataField.txtMeansOfCommunicationEmail.Text.Trim = "" Then
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00028, "%s",
                                     GetGlobalResourceObject("Text", "MeansOfCommunication") + " " + GetGlobalResourceObject("Text", "Email"))
                blnError = True
                dataField.imgtxtMeansOfCommunicationEmailErr.Visible = True
                blnInComplete = True
            End If
        ElseIf dataField.ddlMeansOfCommunication.SelectedValue = MeansofCommunication.FaxNo Then
            If dataField.txtMeansOfCommunicationFax.Text.Trim = "" Then

                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00028, "%s",
                                     GetGlobalResourceObject("Text", "MeansOfCommunication") + " " + GetGlobalResourceObject("Text", "FaxNo"))
                blnError = True
                dataField.imgtxtMeansOfCommunicationFaxErr.Visible = True
                blnInComplete = True
            Else
                Dim msg = udtValidator.chkContactNo(dataField.txtMeansOfCommunicationFax.Text)
                If Not IsNothing(msg) Then
                    udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00029, "%s",
                                    GetGlobalResourceObject("Text", "MeansOfCommunication") + " " + GetGlobalResourceObject("Text", "FaxNo"))
                    blnError = True
                    dataField.imgtxtMeansOfCommunicationFaxErr.Visible = True
                    blnInComplete = True
                End If
            End If
        End If

        If Not String.IsNullOrEmpty(dataField.txtMeansOfCommunicationEmail.Text.Trim) Then
            Dim msg = udtValidator.chkEmailAddress(dataField.txtMeansOfCommunicationEmail.Text.Trim)
            If Not IsNothing(msg) Then
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00029, "%s",
                                 GetGlobalResourceObject("Text", "MeansOfCommunication") + " " + GetGlobalResourceObject("Text", "Email"))
                blnError = True
                dataField.imgtxtMeansOfCommunicationEmailErr.Visible = True
                blnInComplete = True
            End If
        End If

        'check Low Risk Claim
        If dataField.rdoLowRiskClaim.SelectedValue.Trim = "" Then
            If blnNeedValidation Then
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00028, "%s", GetGlobalResourceObject("Text", "LowRiskClaim"))
                blnError = True
                dataField.imgrdoLowRiskClaimErr.Visible = True
            End If
            blnInComplete = True
        End If

        'check Case officer and Subject officer
        Dim isEmptyHfCaseO, isEmptyHfCO, isEmptyInputSO, isEmptyInputCO As Boolean
        'Input Case Officer
        isEmptyInputSO = String.IsNullOrEmpty(dataField.txtCaseOfficer.Text)
        'Input Subject Officer
        isEmptyInputCO = String.IsNullOrEmpty(dataField.txtSubjectOfficer.Text)
        'Hidden Field Case Officer
        isEmptyHfCaseO = String.IsNullOrEmpty(dataField.hfCaseOfficer.Value)
        'Hidden Field Subject Officer
        isEmptyHfCO = String.IsNullOrEmpty(dataField.hfSubjectOfficer.Value)

        Dim isCaseUser As Boolean = False, isSubjectUser As Boolean = False, strUserID = udtHCVUUserBLL.GetHCVUUser.UserID.Trim
        Dim inspectionRole As InspectionRole = udtInspectionRecordBLL.GetInspectionRole(udtHCVUUserBLL.GetHCVUUser)

        If String.IsNullOrEmpty(dataField.hfCaseOfficer.Value) Then
            If blnNeedValidation Then
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00367, "%s", GetGlobalResourceObject("Text", "CaseOfficer"))
                blnError = True
                dataField.imgtxtCaseOfficerErr.Visible = True
            End If
            blnInComplete = True
        Else
            If dataField.hfCaseOfficer.Value.Split("-")(0).Trim = strUserID Then
                isCaseUser = True
            End If
        End If

        If String.IsNullOrEmpty(dataField.txtCaseOfficerContactNo.Text) Then
            If blnNeedValidation Then
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00028, "%s", GetGlobalResourceObject("Text", "ContactNo2"))
                blnError = True
                dataField.imgtxtCaseContactNoErr.Visible = True
            End If
            blnInComplete = True
        Else
            Dim msg = udtValidator.chkContactNo(dataField.txtCaseOfficerContactNo.Text)
            If Not IsNothing(msg) Then
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00029, "%s", GetGlobalResourceObject("Text", "ContactNo2"))
                blnError = True
                dataField.imgtxtCaseContactNoErr.Visible = True
                blnInComplete = True
            End If
        End If

        If String.IsNullOrEmpty(dataField.hfSubjectOfficer.Value) Then
            If blnNeedValidation Then
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00367, "%s", GetGlobalResourceObject("Text", "SubjectOfficer"))
                blnError = True
                dataField.imgtxtSubjectOfficerErr.Visible = True
            End If
            blnInComplete = True
        Else
            If dataField.hfSubjectOfficer.Value.Split("-")(0).Trim = strUserID Then
                isSubjectUser = True
            End If
        End If

        If String.IsNullOrEmpty(dataField.txtSubjectOfficerContactNo.Text) Then
            If blnNeedValidation Then
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00028, "%s", GetGlobalResourceObject("Text", "ContactNo2"))
                blnError = True
                dataField.imgtxtSubjectContactNoErr.Visible = True
            End If
            blnInComplete = True
        Else
            Dim msg = udtValidator.chkContactNo(dataField.txtSubjectOfficerContactNo.Text)
            If Not IsNothing(msg) Then
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00029, "%s", GetGlobalResourceObject("Text", "ContactNo2"))
                blnError = True
                dataField.imgtxtSubjectContactNoErr.Visible = True
                blnInComplete = True
            End If
        End If

        If dataField.txtCaseOfficer.Text <> dataField.hfCaseOfficer.Value Then
            udcMsgBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002)
            blnError = True
            dataField.imgtxtCaseContactNoErr.Visible = True
            blnInComplete = True
        End If
        If dataField.txtSubjectOfficer.Text <> dataField.hfSubjectOfficer.Value Then
            udcMsgBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00003)
            blnError = True
            dataField.imgtxtSubjectContactNoErr.Visible = True
            blnInComplete = True
        End If
        'check Case officer and Subject officer
        If Not String.IsNullOrEmpty(dataField.hfCaseOfficer.Value) And Not String.IsNullOrEmpty(dataField.hfSubjectOfficer.Value) And dataField.hfCaseOfficer.Value = dataField.hfSubjectOfficer.Value Then
            udcMsgBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00005)
            blnError = True
            dataField.imgtxtCaseOfficerErr.Visible = True
            dataField.imgtxtSubjectOfficerErr.Visible = True
        End If
        If (dataField.pageMode = PageMode.ModeEdit And inspectionRole.IsSEO) Then
            'Do nothing
        Else
            If (Not isCaseUser And Not isSubjectUser) Then
                udcMsgBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00008)
                blnError = True
                dataField.imgtxtCaseOfficerErr.Visible = True
                dataField.imgtxtSubjectOfficerErr.Visible = True
            End If
        End If

    End Sub
    Private Function FileReferenceNoValidation(targetName As String, strType As String, strYear As String, strMonth As String, strSeqNo As String, strPartNo As String, imgErr As Image, ByRef blnError As Boolean, Optional isRequired As Boolean = False) As Boolean
        Dim valid As Boolean = True
        Dim isEmptyType = String.IsNullOrEmpty(strType),
            isEmptyYear = String.IsNullOrEmpty(strYear),
            isEmptyMonth = String.IsNullOrEmpty(strMonth),
            isEmptySeqNo = String.IsNullOrEmpty(strSeqNo),
            isEmptyPartNo = String.IsNullOrEmpty(strPartNo)

        If isEmptyType And isEmptyYear And isEmptyMonth And isEmptySeqNo Then
            If Not isEmptyPartNo Then
                valid = False
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00364, "%s", targetName)
            Else
                valid = Not isRequired
                If isRequired Then
                    udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00028, "%s", targetName)
                End If
            End If
        Else
            ' Complete
            If isEmptyType Or isEmptyYear Or isEmptyMonth Or isEmptySeqNo Then
                valid = False
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00364, "%s", targetName)
            Else
                If Not IsNumeric(strType) Or CInt(strType) < 1 Or CInt(strType) > 3 Then
                    ' Type is invalid
                    valid = False
                    udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00365, "%s", " Part 1 of " + targetName)
                End If
                If strYear.Length <> 2 Or Not IsNumeric(strYear) Then
                    ' Year is invalid
                    valid = False
                    udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00365, "%s", " Part 2 of " + targetName)
                End If
                If strMonth.Length <> 2 Or Not IsNumeric(strMonth) Or CInt(strMonth) < 1 Or CInt(strMonth) > 12 Then
                    ' Month is invalid
                    valid = False
                    udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00365, "%s", " Part 3 of " + targetName)
                End If
                If strSeqNo.Length <> 3 Or Not IsNumeric(strSeqNo) Then
                    ' Seq No is invalid
                    valid = False
                    udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00365, "%s", " Part 4 of " + targetName)
                End If
                If Not isEmptyPartNo Then
                    Dim partNoValid = True
                    ' Part No is invalid
                    If strPartNo.Length > 2 Then
                        valid = False
                        partNoValid = False
                    Else
                        If IsNumeric(strPartNo) Then
                            '2-99
                            Dim intPartNo = CInt(strPartNo)
                            If intPartNo < 2 Or intPartNo.ToString().Length <> strPartNo.Length Then
                                valid = False
                                partNoValid = False
                            End If
                            'A(CharCode 65) - Z(CharCode 90)
                        ElseIf strPartNo.Length > 1 Or Asc(strPartNo(0)) < 65 Or Asc(strPartNo(0)) > 90 Then
                            valid = False
                            partNoValid = False
                        End If
                    End If
                    If Not partNoValid Then
                        udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00365, "%s", " Part 5 of " + targetName)
                    End If
                End If
            End If
        End If
        imgErr.Visible = Not valid
        blnError = IIf(Not valid, True, blnError)
        Return valid
    End Function

    Private Sub SetTxtReferNoOnKeyPress(mode As String)
        Dim dataField As DetailDataField = GetDetailDataFieldByMode(mode)
        If (mode = PageMode.ModeNew) Then
            dataField.txtFileReferenceNo1.Attributes.Add("onKeyUp", "autoFocusNextInput(event,this,'" + dataField.txtFileReferenceNo1.ID + "','" + dataField.txtFileReferenceNo2.ID + "')")
            dataField.txtFileReferenceNo2.Attributes.Add("onKeyUp", "autoFocusNextInput(event,this,'" + dataField.txtFileReferenceNo2.ID + "','" + dataField.txtFileReferenceNo3.ID + "')")
            dataField.txtFileReferenceNo3.Attributes.Add("onKeyUp", "autoFocusNextInput(event,this,'" + dataField.txtFileReferenceNo3.ID + "','" + dataField.txtFileReferenceNo4.ID + "')")
            dataField.txtFileReferenceNo4.Attributes.Add("onKeyUp", "autoFocusNextInput(event,this,'" + dataField.txtFileReferenceNo4.ID + "','" + dataField.txtFileReferenceNo5.ID + "')")
        End If
        dataField.txtReferFileRefNoA1.Attributes.Add("onKeyUp", "autoFocusNextInput(event,this,'" + dataField.txtReferFileRefNoA1.ID + "','" + dataField.txtReferFileRefNoA2.ID + "')")
        dataField.txtReferFileRefNoA2.Attributes.Add("onKeyUp", "autoFocusNextInput(event,this,'" + dataField.txtReferFileRefNoA2.ID + "','" + dataField.txtReferFileRefNoA3.ID + "')")
        dataField.txtReferFileRefNoA3.Attributes.Add("onKeyUp", "autoFocusNextInput(event,this,'" + dataField.txtReferFileRefNoA3.ID + "','" + dataField.txtReferFileRefNoA4.ID + "')")
        dataField.txtReferFileRefNoA4.Attributes.Add("onKeyUp", "autoFocusNextInput(event,this,'" + dataField.txtReferFileRefNoA4.ID + "','" + dataField.txtReferFileRefNoA5.ID + "')")

        dataField.txtReferFileRefNoB1.Attributes.Add("onKeyUp", "autoFocusNextInput(event,this,'" + dataField.txtReferFileRefNoB1.ID + "','" + dataField.txtReferFileRefNoB2.ID + "')")
        dataField.txtReferFileRefNoB2.Attributes.Add("onKeyUp", "autoFocusNextInput(event,this,'" + dataField.txtReferFileRefNoB2.ID + "','" + dataField.txtReferFileRefNoB3.ID + "')")
        dataField.txtReferFileRefNoB3.Attributes.Add("onKeyUp", "autoFocusNextInput(event,this,'" + dataField.txtReferFileRefNoB3.ID + "','" + dataField.txtReferFileRefNoB4.ID + "')")
        dataField.txtReferFileRefNoB4.Attributes.Add("onKeyUp", "autoFocusNextInput(event,this,'" + dataField.txtReferFileRefNoB4.ID + "','" + dataField.txtReferFileRefNoB5.ID + "')")

        dataField.txtReferFileRefNoC1.Attributes.Add("onKeyUp", "autoFocusNextInput(event,this,'" + dataField.txtReferFileRefNoC1.ID + "','" + dataField.txtReferFileRefNoC2.ID + "')")
        dataField.txtReferFileRefNoC2.Attributes.Add("onKeyUp", "autoFocusNextInput(event,this,'" + dataField.txtReferFileRefNoC2.ID + "','" + dataField.txtReferFileRefNoC3.ID + "')")
        dataField.txtReferFileRefNoC3.Attributes.Add("onKeyUp", "autoFocusNextInput(event,this,'" + dataField.txtReferFileRefNoC3.ID + "','" + dataField.txtReferFileRefNoC4.ID + "')")
        dataField.txtReferFileRefNoC4.Attributes.Add("onKeyUp", "autoFocusNextInput(event,this,'" + dataField.txtReferFileRefNoC4.ID + "','" + dataField.txtReferFileRefNoC5.ID + "')")
    End Sub
    Private Sub HandleReferredFileReferenceNoList(dataField As DetailDataField)
        Dim isEmptyReferredFileRefNoA As Boolean = String.IsNullOrEmpty(dataField.txtReferFileRefNoA1.Text) And
            String.IsNullOrEmpty(dataField.txtReferFileRefNoA2.Text) And String.IsNullOrEmpty(dataField.txtReferFileRefNoA3.Text) And
            String.IsNullOrEmpty(dataField.txtReferFileRefNoA4.Text) And String.IsNullOrEmpty(dataField.txtReferFileRefNoA5.Text)
        Dim isEmptyReferredFileRefNoB As Boolean = String.IsNullOrEmpty(dataField.txtReferFileRefNoB1.Text) And
           String.IsNullOrEmpty(dataField.txtReferFileRefNoB2.Text) And String.IsNullOrEmpty(dataField.txtReferFileRefNoB3.Text) And
           String.IsNullOrEmpty(dataField.txtReferFileRefNoB4.Text) And String.IsNullOrEmpty(dataField.txtReferFileRefNoB5.Text)
        Dim isEmptyReferredFileRefNoC As Boolean = String.IsNullOrEmpty(dataField.txtReferFileRefNoC1.Text) And
           String.IsNullOrEmpty(dataField.txtReferFileRefNoC2.Text) And String.IsNullOrEmpty(dataField.txtReferFileRefNoC3.Text) And
           String.IsNullOrEmpty(dataField.txtReferFileRefNoC4.Text) And String.IsNullOrEmpty(dataField.txtReferFileRefNoC5.Text)

        If isEmptyReferredFileRefNoA Then
            If Not isEmptyReferredFileRefNoC Then
                'NoA Empty NoC Not Empty NoC -> NoA
                dataField.txtReferFileRefNoA1.Text = dataField.txtReferFileRefNoC1.Text
                dataField.txtReferFileRefNoA2.Text = dataField.txtReferFileRefNoC2.Text
                dataField.txtReferFileRefNoA3.Text = dataField.txtReferFileRefNoC3.Text
                dataField.txtReferFileRefNoA4.Text = dataField.txtReferFileRefNoC4.Text
                dataField.txtReferFileRefNoA5.Text = dataField.txtReferFileRefNoC5.Text
                dataField.txtReferFileRefNoC1.Text = ""
                dataField.txtReferFileRefNoC2.Text = ""
                dataField.txtReferFileRefNoC3.Text = ""
                dataField.txtReferFileRefNoC4.Text = ""
                dataField.txtReferFileRefNoC5.Text = ""
            ElseIf Not isEmptyReferredFileRefNoB Then
                'NoA Empty NoB Not Empty NoC Empty NoB -> NoA
                dataField.txtReferFileRefNoA1.Text = dataField.txtReferFileRefNoB1.Text
                dataField.txtReferFileRefNoA2.Text = dataField.txtReferFileRefNoB2.Text
                dataField.txtReferFileRefNoA3.Text = dataField.txtReferFileRefNoB3.Text
                dataField.txtReferFileRefNoA4.Text = dataField.txtReferFileRefNoB4.Text
                dataField.txtReferFileRefNoA5.Text = dataField.txtReferFileRefNoB5.Text
                dataField.txtReferFileRefNoB1.Text = ""
                dataField.txtReferFileRefNoB2.Text = ""
                dataField.txtReferFileRefNoB3.Text = ""
                dataField.txtReferFileRefNoB4.Text = ""
                dataField.txtReferFileRefNoB5.Text = ""
            End If
        ElseIf isEmptyReferredFileRefNoB And Not isEmptyReferredFileRefNoC Then
            'NoA Not Empty NoB Empty NoC Not Empty NoC -> NoB
            dataField.txtReferFileRefNoB1.Text = dataField.txtReferFileRefNoC1.Text
            dataField.txtReferFileRefNoB2.Text = dataField.txtReferFileRefNoC2.Text
            dataField.txtReferFileRefNoB3.Text = dataField.txtReferFileRefNoC3.Text
            dataField.txtReferFileRefNoB4.Text = dataField.txtReferFileRefNoC4.Text
            dataField.txtReferFileRefNoB5.Text = dataField.txtReferFileRefNoC5.Text
            dataField.txtReferFileRefNoC1.Text = ""
            dataField.txtReferFileRefNoC2.Text = ""
            dataField.txtReferFileRefNoC3.Text = ""
            dataField.txtReferFileRefNoC4.Text = ""
            dataField.txtReferFileRefNoC5.Text = ""
        End If
    End Sub
    'Type of Inspection
    Private Sub HiddenTypeOfInspectionCheckBox()
        For Each item As ListItem In rdoListAddMainTypeofInspection.Items
            If (String.IsNullOrEmpty(item.Text)) Then
                item.Enabled = False
                item.Selected = False
                item.Attributes.CssStyle.Add("display", "none")
            Else
            End If
        Next
        For Each item As ListItem In chkListAddTypeofInspection.Items
            If (String.IsNullOrEmpty(item.Text)) Then
                item.Enabled = False
                item.Selected = False
                item.Attributes.CssStyle.Add("display", "none")
            Else
            End If
        Next
        For Each item As ListItem In chkListEdtTypeofInspection.Items
            If (String.IsNullOrEmpty(item.Text)) Then
                item.Enabled = False
                item.Selected = False
                item.Attributes.CssStyle.Add("display", "none")
            Else
            End If
        Next
    End Sub
    Private Sub HighlightTypeOfInspection(ByVal listClientID As String)
        ScriptManager.RegisterStartupScript(Me, Page.GetType, String.Empty, "HighlightType(document.getElementById('" + listClientID + "'));", True)
    End Sub
    Private Sub HighlightTypeOfInspectionMultiple(ByVal listClientID1 As String, ByVal listClientID2 As String)
        ScriptManager.RegisterStartupScript(Me, Page.GetType, String.Empty, "HighlightTypeMultiple('" + listClientID1 + "','" + listClientID2 + "');", True)
    End Sub

    'Visit Target
    Private Function GetReadyServiceProvider(ByVal strSPID As String, mode As String, Optional ByVal strInspectionID As String = "") As Boolean
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        'udtAuditLogEntry.WriteStartLog(LogID.LOG00002, AuditLogDesc.IRM_SearchSP)
        udtAuditLogEntry.AddDescripton("SP ID", strSPID)
        udtAuditLogEntry.AddDescripton("Mode", mode)
        udtAuditLogEntry.AddDescripton("Inspection ID", strInspectionID)
        Try
            Dim udtSPBLL As New ServiceProvider.ServiceProviderBLL
            Dim udtSP As ServiceProvider.ServiceProviderModel
            Dim HCVSEffectiveDate, HCVSDHCEffectiveDate, HCVSCHNEffectiveDate, HCVSDelistDate, HCVSDHCDelistDate, HCVSCHNDelistDate As Date

            udtSP = udtSPBLL.GetServiceProviderPermanentProfileWithMaintenanceBySPID(strSPID, New Common.DataAccess.Database)

            If udtSP Is Nothing Then
                Return False
            End If
            Dim HCVSItem As Common.Component.SchemeInformation.SchemeInformationModel = udtSP.SchemeInfoList.Filter("HCVS")
            Dim HCVSDHCItem As Common.Component.SchemeInformation.SchemeInformationModel = udtSP.SchemeInfoList.Filter("HCVSDHC")
            Dim HCVSCHNItem As Common.Component.SchemeInformation.SchemeInformationModel = udtSP.SchemeInfoList.Filter("HCVSCHN")

            If IsNothing(HCVSItem) And IsNothing(HCVSDHCItem) And IsNothing(HCVSCHNItem) Then
                Return False
            End If
            If Not IsNothing(HCVSItem) Then
                HCVSEffectiveDate = udtInspectionRecordBLL.ConvertNullableDate(HCVSItem.EffectiveDtm)
                HCVSDelistDate = udtInspectionRecordBLL.ConvertNullableDate(HCVSItem.DelistDtm)
            Else
                HCVSEffectiveDate = Date.MinValue
                HCVSDelistDate = Date.MinValue
            End If
            If Not IsNothing(HCVSDHCItem) Then
                HCVSDHCEffectiveDate = udtInspectionRecordBLL.ConvertNullableDate(HCVSDHCItem.EffectiveDtm)
                HCVSDHCDelistDate = udtInspectionRecordBLL.ConvertNullableDate(HCVSDHCItem.DelistDtm)
            Else
                HCVSDHCEffectiveDate = Date.MinValue
                HCVSDHCDelistDate = Date.MinValue
            End If
            If Not IsNothing(HCVSCHNItem) Then
                HCVSCHNEffectiveDate = udtInspectionRecordBLL.ConvertNullableDate(HCVSCHNItem.EffectiveDtm)
                HCVSCHNDelistDate = udtInspectionRecordBLL.ConvertNullableDate(HCVSCHNItem.DelistDtm)
            Else
                HCVSCHNEffectiveDate = Date.MinValue
                HCVSCHNDelistDate = Date.MinValue
            End If

            Dim lastRecord As InspectionRecordModel = udtInspectionRecordBLL.GetLatestRecordBySPID(strSPID, strInspectionID)

            Dim _lblServiceProviderName As Label = Nothing
            Dim _lblSPStatus As Label = Nothing
            Dim _ddlPractice As DropDownList = Nothing
            Dim _lblPractice As Label = Nothing
            Dim _lblServiceProviderID As Label = Nothing
            Dim _lblTelNo As Label = Nothing
            Dim _lblFaxNo As Label = Nothing
            Dim _lblEmailAddress As Label = Nothing
            Dim _lblHCVSEffectiveDate As Label = Nothing
            Dim _trHCVSEffectiveDate As HtmlTableRow = Nothing
            Dim _lblHCVSDHCEffectiveDate As Label = Nothing
            Dim _trHCVSDHCEffectiveDate As HtmlTableRow = Nothing
            Dim _lblHCVSCHNEffectiveDate As Label = Nothing
            Dim _trHCVSCHNEffectiveDate As HtmlTableRow = Nothing
            Dim _trContactInfo As HtmlTableRow = Nothing
            Dim _lblSPContactInfo As Label = Nothing
            Dim _pnlSPTelNo As Panel = Nothing
            Dim _pnlSPFaxNo As Panel = Nothing
            Dim _pnlSPEmail As Panel = Nothing
            Dim _lblLastVisitDate As Label = Nothing
            Dim _ddlFormCondition As DropDownList = Nothing
            Dim _txtFormConditionRm As TextBox = Nothing
            Dim _ddlMeansOfCommunication As DropDownList = Nothing
            Dim _txtMeansOfCommunicationFax As TextBox = Nothing
            Dim _txtMeansOfCommunicationEmail As TextBox = Nothing

            If mode = PageMode.ModeNew Then
                _lblServiceProviderName = lblServiceProviderName
                _lblSPStatus = lblSPStatus
                _ddlPractice = ddlPractice
                _lblPractice = lblPractice
                _lblServiceProviderID = lblServiceProviderID
                _trContactInfo = trSPContactInfo
                _lblSPContactInfo = lblSPContactInfo
                _pnlSPEmail = pnlSPEmail
                _pnlSPTelNo = pnlSPTelNo
                _pnlSPFaxNo = pnlSPFaxNo
                _lblTelNo = lblSPTelNo
                _lblFaxNo = lblSPFaxNo
                _lblEmailAddress = lblSPEmail
                _lblHCVSEffectiveDate = lblHCVSEffectiveDate
                _trHCVSEffectiveDate = trHCVSEffectiveDate
                _lblHCVSDHCEffectiveDate = lblHCVSDHCEffectiveDate
                _trHCVSDHCEffectiveDate = trHCVSDHCEffectiveDate
                _lblHCVSCHNEffectiveDate = lblHCVSCHNEffectiveDate
                _trHCVSCHNEffectiveDate = trHCVSCHNEffectiveDate
                _lblLastVisitDate = lblLastVisitDate
                _ddlFormCondition = ddlFormCondition
                _txtFormConditionRm = txtFormConditionRm
                _ddlMeansOfCommunication = ddlMeansofCommunication
                _txtMeansOfCommunicationEmail = txtMeansofCommunicationEmail
                _txtMeansOfCommunicationFax = txtMeansofCommunicationFax
            Else
                _lblServiceProviderName = lblEditServiceProviderName
                _lblSPStatus = lblEditSPStatus
                _ddlPractice = ddlEditPractice
                _lblPractice = lblEditPractice
                _lblServiceProviderID = lblEditServiceProviderID
                _trContactInfo = trEditSPContactInfo
                _lblSPContactInfo = lblEditSPContactInfo
                _pnlSPEmail = pnlEditSPEmail
                _pnlSPTelNo = pnlEditSPTelNo
                _pnlSPFaxNo = pnlEditSPFaxNo
                _lblTelNo = lblEditSPTelNo
                _lblFaxNo = lblEditSPFaxNo
                _lblEmailAddress = lblEditSPEmail
                _lblHCVSEffectiveDate = lblEditHCVSEffectiveDate
                _trHCVSEffectiveDate = trEditHCVSEffectiveDate
                _lblHCVSDHCEffectiveDate = lblEditHCVSDHCEffectiveDate
                _trHCVSDHCEffectiveDate = trEditHCVSDHCEffectiveDate
                _lblHCVSCHNEffectiveDate = lblEditHCVSCHNEffectiveDate
                _trHCVSCHNEffectiveDate = trEditHCVSCHNEffectiveDate
                _lblLastVisitDate = lblEditLastVisitDate
                _ddlFormCondition = ddlEdtFormCondition
                _txtFormConditionRm = txtEdtFormConditionRm
                _ddlMeansOfCommunication = ddlEdtMeansofCommunication
                _txtMeansOfCommunicationEmail = txtEdtMeansofCommunicationEmail
                _txtMeansOfCommunicationFax = txtEdtMeansofCommunicationFax
            End If

            _lblServiceProviderName.Text = String.Format("{0} {1}", udtSP.EnglishName, udtformatter.formatChineseName(udtSP.ChineseName))
            ' Check SP Status
            If Not udtSP.RecordStatus.Trim.Equals(ServiceProviderStatus.Active) Then
                Status.GetDescriptionFromDBCode(ServiceProviderStatus.ClassCode, udtSP.RecordStatus, _lblSPStatus.Text, String.Empty)
                _lblSPStatus.Text = joinParenthesis(_lblSPStatus.Text)
                _lblSPStatus.Visible = True
            Else
                _lblSPStatus.Text = ""
                _lblSPStatus.Visible = False
            End If

            _ddlPractice.Visible = True
            _lblPractice.Visible = False
            trServiceProviderID.Visible = False
            _lblServiceProviderID.Text = strSPID
            setAfterSearchTarget(PageMode.ModeEdit)
            Dim isEmptySPContactNo As Boolean = String.IsNullOrEmpty(udtSP.Phone),
                isEmptySPFaxNo As Boolean = String.IsNullOrEmpty(udtSP.Fax),
                isEmptySPEmail As Boolean = String.IsNullOrEmpty(udtSP.Email)

            _lblTelNo.Text = udtSP.Phone
            _lblFaxNo.Text = udtSP.Fax
            _lblEmailAddress.Text = udtSP.Email
            _lblSPContactInfo.Visible = (isEmptySPContactNo And isEmptySPFaxNo And isEmptySPEmail)
            _pnlSPTelNo.Visible = Not isEmptySPContactNo
            _pnlSPFaxNo.Visible = Not isEmptySPFaxNo
            _pnlSPEmail.Visible = Not isEmptySPEmail

            If HCVSEffectiveDate <> Date.MinValue Then
                _trHCVSEffectiveDate.Visible = True
                _lblHCVSEffectiveDate.Text = udtInspectionRecordBLL.FormatOutputDate(HCVSEffectiveDate)
                If HCVSDelistDate <> Date.MinValue Then
                    _lblHCVSEffectiveDate.Text += " <font color=""red"">" + joinParenthesis(Me.GetGlobalResourceObject("Text", "DelistedOn") + " " + udtInspectionRecordBLL.FormatOutputDate(HCVSDelistDate)) + "</font>"
                End If
            Else
                _trHCVSEffectiveDate.Visible = False
                _lblHCVSEffectiveDate.Text = Me.GetGlobalResourceObject("Text", "Empty")
            End If
            If HCVSDHCEffectiveDate <> Date.MinValue Then
                _trHCVSDHCEffectiveDate.Visible = True
                _lblHCVSDHCEffectiveDate.Text = udtInspectionRecordBLL.FormatOutputDate(HCVSDHCEffectiveDate)
                If HCVSDHCDelistDate <> Date.MinValue Then
                    _lblHCVSDHCEffectiveDate.Text += " <font color=""red"">" + joinParenthesis(Me.GetGlobalResourceObject("Text", "DelistedOn") + " " + udtInspectionRecordBLL.FormatOutputDate(HCVSDHCDelistDate)) + "</font>"
                End If
            Else
                _trHCVSDHCEffectiveDate.Visible = False
                _lblHCVSDHCEffectiveDate.Text = Me.GetGlobalResourceObject("Text", "Empty")
            End If
            If HCVSCHNEffectiveDate <> Date.MinValue Then
                _trHCVSCHNEffectiveDate.Visible = True
                _lblHCVSCHNEffectiveDate.Text = udtInspectionRecordBLL.FormatOutputDate(HCVSCHNEffectiveDate)
                If HCVSCHNDelistDate <> Date.MinValue Then
                    _lblHCVSCHNEffectiveDate.Text += "<font color=""red"">" + Me.GetGlobalResourceObject("Text", "DelistedOn") + joinParenthesis(udtInspectionRecordBLL.FormatOutputDate(HCVSCHNDelistDate)) + "</font>"
                End If
            Else
                _trHCVSCHNEffectiveDate.Visible = False
                _lblHCVSCHNEffectiveDate.Text = Me.GetGlobalResourceObject("Text", "Empty")
            End If
            Dim dtPracticeList As DataTable = udtInspectionRecordBLL.GetAllPractice(udtSP.SPID, Practice.PracticeBLL.PracticeDisplayType.Practice)
            'Filter Scheme Code (HCVS/HCVSDHC/HCVSCHN)
            Dim filterPractice As DataTable = dtPracticeList.Clone
            For Each dr As DataRow In dtPracticeList.Rows
                Dim practiceID = dr("PracticeID")
                Dim index As Integer = CInt(practiceID)
                Dim practice As Practice.PracticeModel = udtSP.PracticeList.Item(index)
                If Not IsNothing(practice.PracticeSchemeInfoList.Filter("HCVS", "EHCVS")) Or
               Not IsNothing(practice.PracticeSchemeInfoList.Filter("HCVSDHC", "EHCVS")) Or
              Not IsNothing(practice.PracticeSchemeInfoList.Filter("HCVSCHN", "EHCVS")) Then
                    Dim filterDr As DataRow = filterPractice.NewRow
                    filterDr("SP_ID") = dr("SP_ID")
                    filterDr("PracticeID") = dr("PracticeID")
                    filterDr("Practice_Name") = dr("Practice_Name")
                    filterDr("Practice_Name_Chi") = dr("Practice_Name_Chi")
                    filterDr("Practice_Status") = dr("Practice_Status")
                    filterDr("BankAccountKey") = dr("BankAccountKey")
                    filterDr("Room") = dr("Room")
                    filterDr("Floor") = dr("Floor")
                    filterDr("Block") = dr("Block")
                    filterDr("Building") = dr("Building")
                    filterDr("Building_Chi") = dr("Building_Chi")
                    filterDr("District") = dr("District")
                    filterDr("Address_Code") = dr("Address_Code")
                    filterDr("Phone_Daytime") = dr("Phone_Daytime")
                    filterDr("Mobile_Clinic") = dr("Mobile_Clinic")
                    filterDr("Remarks_Desc") = dr("Remarks_Desc")
                    filterDr("Remarks_Desc_Chi") = dr("Remarks_Desc_Chi")
                    filterDr("Service_Category_Code") = dr("Service_Category_Code")
                    filterDr("Registration_Code") = dr("Registration_Code")
                    filterDr("BankAcctID") = dr("BankAcctID")
                    filterDr("Bank_Account_No") = dr("Bank_Account_No")
                    filterDr("Bank_Acc_Holder") = dr("Bank_Acc_Holder")
                    filterDr("BankAcct_Status") = dr("BankAcct_Status")
                    filterDr("Display_Name") = dr("Display_Name")
                    filterDr("Display_Name_Chi") = dr("Display_Name_Chi")
                    filterPractice.Rows.Add(filterDr)
                End If
            Next
            dtPracticeList = filterPractice

            Dim udtPracticeBLL As New Practice.PracticeBLL

            Me.udtSessionHandlerBLL.PracticeDisplayListRemoveFromSession(FunctionCode)
            Me.udtSessionHandlerBLL.PracticeDisplayListSaveToSession(udtPracticeBLL.convertPractice(dtPracticeList), FunctionCode)

            _ddlPractice.DataSource = dtPracticeList

            _ddlPractice.DataTextField = Practice.PracticeBLL.PracticeDisplayField.Display_Eng
            _ddlPractice.DataValueField = "PracticeID"
            _ddlPractice.DataBind()

            _ddlPractice.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), ""))
            _ddlPractice.SelectedIndex = 0

            If Not IsNothing(lastRecord) Then
                _ddlFormCondition.SelectedValue = lastRecord.FormConditionID
                _ddlMeansOfCommunication.SelectedValue = lastRecord.MeansOfCommunicationID
                If (mode = PageMode.ModeNew) Then
                    ddlFormCondition_SelectedIndexChanged(_ddlFormCondition, Nothing)
                    ddlMeansofCommunication_SelectedIndexChanged(_ddlMeansOfCommunication, Nothing)
                Else
                    ddlEdtFormCondition_SelectedIndexChanged(_ddlFormCondition, Nothing)
                    ddlEdtMeansofCommunication_SelectedIndexChanged(_ddlMeansOfCommunication, Nothing)
                End If
                _txtFormConditionRm.Text = lastRecord.FormConditionRemark
                _txtMeansOfCommunicationEmail.Text = lastRecord.MeansOfCommunicationEmail
                _txtMeansOfCommunicationFax.Text = lastRecord.MeansOfCommunicationFax
                _lblLastVisitDate.Text = udtInspectionRecordBLL.FormatOutputDate(lastRecord.VisitDate) + joinParenthesis(Me.GetGlobalResourceObject("Text", "FileReferenceNo") + ": " + lastRecord.FileReferenceNo)
            Else
                _lblLastVisitDate.Text = Me.GetGlobalResourceObject("Text", "NA")
                _ddlFormCondition.SelectedValue = ""
                _ddlMeansOfCommunication.SelectedValue = ""
                If (mode = PageMode.ModeNew) Then
                    ddlFormCondition_SelectedIndexChanged(_ddlFormCondition, Nothing)
                    ddlMeansofCommunication_SelectedIndexChanged(_ddlMeansOfCommunication, Nothing)
                Else
                    ddlEdtFormCondition_SelectedIndexChanged(_ddlFormCondition, Nothing)
                    ddlEdtMeansofCommunication_SelectedIndexChanged(_ddlMeansOfCommunication, Nothing)
                End If
                _txtFormConditionRm.Text = ""
                _txtMeansOfCommunicationEmail.Text = ""
                _txtMeansOfCommunicationFax.Text = ""
            End If

            Session(SESS_ServiceProvider) = udtSP
            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditLogDesc.IRM_SearchSP_Success)
            Return True
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00004, AuditLogDesc.IRM_SearchSP_Fail)
        End Try

        Return False
    End Function
    Protected Sub setAfterSearchTarget(mode As String)
        If mode = PageMode.ModeNew Then
            txtSPIDNew.Enabled = False
            ibtnClear.Enabled = True
            udtGeneralFunction.UpdateImageURL(ibtnSearchVisitTarget, False)
            udtGeneralFunction.UpdateImageURL(ibtnClear, True)
        Else
            txtEditSPID.Enabled = False
            udtGeneralFunction.UpdateImageURL(ibtnEditSearchVisitTarget, False)
            udtGeneralFunction.UpdateImageURL(ibtnEditClear, True)
        End If
    End Sub
    Protected Sub setAfterClearTarget(mode As String)
        If mode = PageMode.ModeNew Then
            txtSPIDNew.Enabled = True
            udtGeneralFunction.UpdateImageURL(ibtnSearchVisitTarget, True)
            udtGeneralFunction.UpdateImageURL(ibtnClear, False)
        Else
            txtEditSPID.Enabled = True
            udtGeneralFunction.UpdateImageURL(ibtnEditSearchVisitTarget, True)
            udtGeneralFunction.UpdateImageURL(ibtnEditClear, False)
        End If
    End Sub
    Protected Sub ClearVisitTargetForm(mode As String)
        If mode = PageMode.ModeNew Then
            lblServiceProviderName.Text = Me.GetGlobalResourceObject("Text", "Empty")
            lblSPTelNo.Text = Me.GetGlobalResourceObject("Text", "Empty")
            lblSPFaxNo.Text = ""
            lblSPEmail.Text = ""
            lblHCVSEffectiveDate.Text = Me.GetGlobalResourceObject("Text", "Empty")
            lblHCVSDHCEffectiveDate.Text = Me.GetGlobalResourceObject("Text", "Empty")
            lblHCVSCHNEffectiveDate.Text = Me.GetGlobalResourceObject("Text", "Empty")
            trHCVSCHNEffectiveDate.Visible = False
            trHCVSDHCEffectiveDate.Visible = False

            lblLastVisitDate.Text = Me.GetGlobalResourceObject("Text", "Empty")

            lblSPContactInfo.Visible = True
            pnlSPTelNo.Visible = False
            pnlSPFaxNo.Visible = False
            pnlSPEmail.Visible = False

        Else
            lblEditServiceProviderName.Text = Me.GetGlobalResourceObject("Text", "Empty")
            lblEditSPTelNo.Text = Me.GetGlobalResourceObject("Text", "Empty")
            lblEditSPFaxNo.Text = ""
            lblEditSPEmail.Text = ""
            lblEditHCVSEffectiveDate.Text = Me.GetGlobalResourceObject("Text", "Empty")
            lblEditHCVSDHCEffectiveDate.Text = Me.GetGlobalResourceObject("Text", "Empty")
            lblEditHCVSCHNEffectiveDate.Text = Me.GetGlobalResourceObject("Text", "Empty")
            trEditHCVSDHCEffectiveDate.Visible = False
            trEditHCVSCHNEffectiveDate.Visible = False

            lblEditLastVisitDate.Text = Me.GetGlobalResourceObject("Text", "Empty")

            lblEditSPContactInfo.Visible = True
            pnlEditSPTelNo.Visible = False
            pnlEditSPFaxNo.Visible = False
            pnlEditSPEmail.Visible = False

        End If
        ClearVisitTargetFormSub(mode)
    End Sub
    Protected Sub ClearVisitTargetFormSub(mode As String)
        If mode = PageMode.ModeNew Then
            lblPractice.Text = Me.GetGlobalResourceObject("Text", "Empty")
            lblPractice_Ci.Text = ""
            lblPracticeAddress.Text = Me.GetGlobalResourceObject("Text", "Empty")
            lblPracticeAddress_Ci.Text = ""
            lblHealthProfession.Text = Me.GetGlobalResourceObject("Text", "Empty")
            lblPracticePhoneDaytime.Text = Me.GetGlobalResourceObject("Text", "Empty")
            ddlPractice.Visible = False
            lblPractice.Visible = True
            ddlPractice.SelectedValue = ""
        Else
            lblEditPractice.Text = Me.GetGlobalResourceObject("Text", "Empty")
            lblEditPractice_Ci.Text = ""
            lblEditPracticeAddress.Text = Me.GetGlobalResourceObject("Text", "Empty")
            lblEditPracticeAddress_Ci.Text = ""
            lblEditHealthProfession.Text = Me.GetGlobalResourceObject("Text", "Empty")
            lblEditPracticePhoneDaytime.Text = Me.GetGlobalResourceObject("Text", "Empty")
            ddlEditPractice.Visible = False
            lblEditPractice.Visible = True
            ddlEditPractice.SelectedValue = ""
        End If

    End Sub
    'UI
    Private Sub SetLabelcolor(control As Label, Optional ByVal isOptionalField As Boolean = False)
        If control.Text.Trim = "" Then
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
    Private Function joinParenthesis(str As String) As String
        Return String.Format(" ({0})", str)
    End Function
    Private Sub SetLableText(control As Label, Optional ByVal isOptionalField As Boolean = False)
        If control.Text.Trim = "" Then
            If isOptionalField Then
                control.Text = Me.GetGlobalResourceObject("Text", "Empty")
                control.Style.Remove("color")
            Else
                control.Text = Me.GetGlobalResourceObject("Text", "ToBeProvided")
                control.Style.Add("color", "red")
            End If
        Else
            control.Style.Remove("color")
        End If
    End Sub
    Private Sub SetLableText(control As Label, srcControl As Control, isDate As Boolean, Optional ByVal pastDateInvalid As Boolean = False, Optional ByVal isOptionalField As Boolean = False)
        Dim text As String = "'"
        Dim value As String = ""
        If TypeOf srcControl Is TextBox Then
            value = CType(srcControl, TextBox).Text
            text = CType(srcControl, TextBox).Text
        End If
        If TypeOf srcControl Is DropDownList Then
            value = CType(srcControl, DropDownList).SelectedValue
            text = CType(srcControl, DropDownList).SelectedItem.Text
        End If
        If TypeOf srcControl Is HiddenField Then
            Dim splitValue As String() = CType(srcControl, HiddenField).Value.Split("-")
            If (splitValue.Length > 1) Then
                value = splitValue(0)
                text = splitValue(1)
            End If
        End If
        If value.Trim = "" Then
            If isOptionalField Then
                control.Text = Me.GetGlobalResourceObject("Text", "Empty")
                control.Style.Remove("color")
            Else
                control.Text = Me.GetGlobalResourceObject("Text", "ToBeProvided")
                control.Style.Add("color", "red")
            End If

        Else
            If isDate Then
                control.Text = udtformatter.convertDate(text, "")
                If (pastDateInvalid) Then
                    Dim dateControl As Date = CDate(control.Text)
                    If dateControl.Date <= Date.Now.Date Then
                        control.Text = control.Text + GetGlobalResourceObject("Text", "PastDate")
                    End If
                End If
            Else
                control.Text = text
            End If
            control.Style.Remove("color")
        End If
    End Sub

    Public Sub ShowSuccessMessage(box As CustomControls.InfoMessageBox, msgCode As String, InspectionID As String, FileRefNo As String)
        udcInfoMsgBox.Visible = False
        box.Type = CustomControls.InfoMessageBoxType.Complete
        Dim strOldCharList() As String
        Dim objNewCharList() As String

        strOldCharList = New String() {"%s", "%f"}
        objNewCharList = New String() {InspectionID, FileRefNo}
        box.AddMessage(FunctionCode, SeverityCode.SEVI, msgCode, strOldCharList, objNewCharList)
        box.BuildMessageBox()
        box.Visible = True
    End Sub

    Private Sub ShowDetailConfirmNew(mode As String)
        Dim _lblConTypeofInspection As New Label
        Dim _lblConVisitDate As New Label
        Dim _lblConVisitTime As New Label
        Dim _lblConConfirmationWith As New Label
        Dim _lblConConfirmDate As New Label
        Dim _lblConFormCondition As New Label
        Dim _lblConMeansofCommunication As New Label
        Dim _lblConRemarks As New Label
        Dim _lblConCaseOfficer As New Label
        Dim _lblConCaseContactNo As New Label
        Dim _lblConSubjectContactNo As New Label
        Dim _lblConSubjectOfficer As New Label
        Dim _lblConLowRiskClaim As New Label

        Select Case mode
            Case PageMode.ModeNew
                _lblConTypeofInspection = lblConTypeofInspection
                _lblConVisitDate = lblConVisitDate
                _lblConVisitTime = lblConVisitTime
                _lblConConfirmationWith = lblConConfirmationWith
                _lblConConfirmDate = lblConConfirmDate
                _lblConFormCondition = lblConFormCondition
                _lblConMeansofCommunication = lblConMeansofCommunication
                _lblConLowRiskClaim = lblConLowRiskClaim
                _lblConRemarks = lblConRemarks
                _lblConCaseOfficer = lblConCaseOfficer
                _lblConCaseContactNo = lblConCaseContactNo
                _lblConSubjectContactNo = lblConSubjectContactNo
                _lblConSubjectOfficer = lblConSubjectOfficer
            Case PageMode.ModeEdit
                _lblConTypeofInspection = lblEVCTypeofInspection
                _lblConVisitDate = lblEVCVisitDate
                _lblConVisitTime = lblEVCVisitTime
                _lblConConfirmationWith = lblEVCConfirmationWith
                _lblConConfirmDate = lblEVCConfirmDate
                _lblConFormCondition = lblEVCFormCondition
                _lblConMeansofCommunication = lblEVCMeansofCommunication
                _lblConLowRiskClaim = lblEVCLowRiskClaim
                _lblConRemarks = lblEVCRemarks
                _lblConCaseOfficer = lblEVCCaseOfficer
                _lblConCaseContactNo = lblEVCCaseContactNo
                _lblConSubjectContactNo = lblEVCSubjectContactNo
                _lblConSubjectOfficer = lblEVCSubjectOfficer
        End Select
        SetLableText(_lblConTypeofInspection, True)
        SetLableText(_lblConVisitDate)
        SetLableText(_lblConVisitTime)
        SetLableText(_lblConConfirmationWith)
        SetLableText(_lblConConfirmDate)
        SetLableText(_lblConFormCondition)
        SetLableText(_lblConMeansofCommunication)
        SetLableText(_lblConLowRiskClaim)
        SetLableText(_lblConRemarks, True)
        SetLableText(_lblConCaseOfficer)
        SetLableText(_lblConCaseContactNo)
        SetLableText(_lblConSubjectOfficer)
        SetLableText(_lblConSubjectContactNo)
    End Sub

    Private Function CollectDataFromFieldByMode(mode As String) As InspectionRecordModel
        Dim model As New InspectionRecordModel
        Dim dataField As DetailDataField = GetDetailDataFieldByMode(mode)
        Dim strCaseOfficer = dataField.hfCaseOfficer.Value.Split("-")(0).Trim,
        strSubjectOfficer = dataField.hfSubjectOfficer.Value.Split("-")(0).Trim,
        referFileRefNoA = udtInspectionRecordBLL.HandleFileRefNo(dataField.txtReferFileRefNoA1.Text, dataField.txtReferFileRefNoA2.Text, dataField.txtReferFileRefNoA3.Text, dataField.txtReferFileRefNoA4.Text, dataField.txtReferFileRefNoA5.Text),
        referFileRefNoB = udtInspectionRecordBLL.HandleFileRefNo(dataField.txtReferFileRefNoB1.Text, dataField.txtReferFileRefNoB2.Text, dataField.txtReferFileRefNoB3.Text, dataField.txtReferFileRefNoB4.Text, dataField.txtReferFileRefNoB5.Text),
        referFileRefNoD = udtInspectionRecordBLL.HandleFileRefNo(dataField.txtReferFileRefNoC1.Text, dataField.txtReferFileRefNoC2.Text, dataField.txtReferFileRefNoC3.Text, dataField.txtReferFileRefNoC4.Text, dataField.txtReferFileRefNoC5.Text)

        If String.IsNullOrEmpty(referFileRefNoA) Then
            If Not String.IsNullOrEmpty(referFileRefNoD) Then
                'No1 Empty No3 Not Empty No3 -> No1
                referFileRefNoA = referFileRefNoD
                dataField.txtReferFileRefNoA1.Text = dataField.txtReferFileRefNoC1.Text
                dataField.txtReferFileRefNoA2.Text = dataField.txtReferFileRefNoC2.Text
                dataField.txtReferFileRefNoA3.Text = dataField.txtReferFileRefNoC3.Text
                dataField.txtReferFileRefNoA4.Text = dataField.txtReferFileRefNoC4.Text
                dataField.txtReferFileRefNoA5.Text = dataField.txtReferFileRefNoC5.Text
                dataField.txtReferFileRefNoC1.Text = ""
                dataField.txtReferFileRefNoC2.Text = ""
                dataField.txtReferFileRefNoC3.Text = ""
                dataField.txtReferFileRefNoC4.Text = ""
                dataField.txtReferFileRefNoC5.Text = ""
            ElseIf Not String.IsNullOrEmpty(referFileRefNoB) Then
                'No1 Empty No2 Not Empty No3 Empty No2 -> No1
                referFileRefNoA = referFileRefNoB
                dataField.txtReferFileRefNoA1.Text = dataField.txtReferFileRefNoB1.Text
                dataField.txtReferFileRefNoA2.Text = dataField.txtReferFileRefNoB2.Text
                dataField.txtReferFileRefNoA3.Text = dataField.txtReferFileRefNoB3.Text
                dataField.txtReferFileRefNoA4.Text = dataField.txtReferFileRefNoB4.Text
                dataField.txtReferFileRefNoA5.Text = dataField.txtReferFileRefNoB5.Text
                dataField.txtReferFileRefNoB1.Text = ""
                dataField.txtReferFileRefNoB2.Text = ""
                dataField.txtReferFileRefNoB3.Text = ""
                dataField.txtReferFileRefNoB4.Text = ""
                dataField.txtReferFileRefNoB5.Text = ""
            End If
        ElseIf String.IsNullOrEmpty(referFileRefNoB) And Not String.IsNullOrEmpty(referFileRefNoD) Then
            'No1 Not Empty No2 Empty No3 Not Empty No3 -> No2
            referFileRefNoB = referFileRefNoD
            dataField.txtReferFileRefNoB1.Text = dataField.txtReferFileRefNoC1.Text
            dataField.txtReferFileRefNoB2.Text = dataField.txtReferFileRefNoC2.Text
            dataField.txtReferFileRefNoB3.Text = dataField.txtReferFileRefNoC3.Text
            dataField.txtReferFileRefNoB4.Text = dataField.txtReferFileRefNoC4.Text
            dataField.txtReferFileRefNoB5.Text = dataField.txtReferFileRefNoC5.Text
            dataField.txtReferFileRefNoC1.Text = ""
            dataField.txtReferFileRefNoC2.Text = ""
            dataField.txtReferFileRefNoC3.Text = ""
            dataField.txtReferFileRefNoC4.Text = ""
            dataField.txtReferFileRefNoC5.Text = ""
        End If

        With model
            .FileReferenceType = dataField.rdoFileReferenceType.SelectedValue
            .ReferredReferenceNo1 = referFileRefNoA
            .ReferredReferenceNo2 = referFileRefNoB
            .ReferredReferenceNo3 = referFileRefNoD
            .SPID = dataField.txtSPID.Text.Trim
            .OtherTypeOfInspectionID = udtInspectionRecordBLL.GetTypeofInspectionStringFromInput(dataField.chkListTypeOfInspection)
            .PracticeDisplaySeq = dataField.hfPracticeSeqNo.Value
            .VisitDate = udtInspectionRecordBLL.ConvertDate(dataField.txtVisitDate.Text.Trim)
            .VisitBeginDtm = IIf(txtStartVisitTime.Text.Trim = "", Date.MinValue, udtformatter.ConvertToDate(udtformatter.convertDate(Me.txtVisitDate.Text.Trim, String.Empty) + " " + txtStartVisitTime.Text.Trim))
            .VisitEndDtm = IIf(txtEndVisitTime.Text.Trim = "", Date.MinValue, udtformatter.ConvertToDate(udtformatter.convertDate(Me.txtVisitDate.Text.Trim, String.Empty) + " " + txtEndVisitTime.Text.Trim))
            .ConfirmationWith = dataField.txtConfirmationWith.Text.Trim
            .ConfirmationDtm = udtInspectionRecordBLL.ConvertDate(dataField.txtConfirmDate.Text.Trim)
            .FormConditionID = dataField.ddlFormCondition.SelectedValue.Trim
            .FormConditionRemark = dataField.txtFormConditionRm.Text.Trim
            .MeansOfCommunicationID = dataField.ddlMeansOfCommunication.SelectedValue.Trim
            .MeansOfCommunicationEmail = dataField.txtMeansOfCommunicationEmail.Text.Trim
            .MeansOfCommunicationFax = dataField.txtMeansOfCommunicationFax.Text.Trim
            .LowRiskClaim = IIf(dataField.rdoLowRiskClaim.SelectedValue.Trim = "", Nothing, dataField.rdoLowRiskClaim.SelectedValue.Trim)
            .Remarks = dataField.txtRemarks.Text.Trim
            .CaseOfficerID = strCaseOfficer
            .CaseOfficerValue = dataField.hfCaseOfficer.Value
            .CaseOfficerContactNo = dataField.txtCaseOfficerContactNo.Text.Trim
            .SubjectOfficerID = strSubjectOfficer
            .SubjectOfficerValue = dataField.hfSubjectOfficer.Value
            .SubjectOfficerContactNo = dataField.txtSubjectOfficerContactNo.Text.Trim
            .RecordStatus = hdnStatus.Value
            .UserID = udtHCVUUserBLL.GetHCVUUser.UserID
        End With
        Return model
    End Function

    Private Function GetDetailDataFieldByMode(mode As String) As DetailDataField
        Dim DetailDataField As New DetailDataField
        Select Case mode
            Case PageMode.ModeNew
                With DetailDataField
                    .rdoListMainTypeOfInspection = rdoListAddMainTypeofInspection
                    .chkListTypeOfInspection = chkListAddTypeofInspection
                    .rdoFileReferenceType = rdoFileReferenceType
                    .txtFileReferenceNo1 = txtFRN1
                    .txtFileReferenceNo2 = txtFRN2
                    .txtFileReferenceNo3 = txtFRN3
                    .txtFileReferenceNo4 = txtFRN4
                    .txtFileReferenceNo5 = txtFRN5
                    .txtReferFileRefNoA1 = txtRRNA1
                    .txtReferFileRefNoA2 = txtRRNA2
                    .txtReferFileRefNoA3 = txtRRNA3
                    .txtReferFileRefNoA4 = txtRRNA4
                    .txtReferFileRefNoA5 = txtRRNA5
                    .txtReferFileRefNoB1 = txtRRNB1
                    .txtReferFileRefNoB2 = txtRRNB2
                    .txtReferFileRefNoB3 = txtRRNB3
                    .txtReferFileRefNoB4 = txtRRNB4
                    .txtReferFileRefNoB5 = txtRRNB5
                    .txtReferFileRefNoC1 = txtRRNC1
                    .txtReferFileRefNoC2 = txtRRNC2
                    .txtReferFileRefNoC3 = txtRRNC3
                    .txtReferFileRefNoC4 = txtRRNC4
                    .txtReferFileRefNoC5 = txtRRNC5

                    .officerList = officerList
                    .hfCaseOfficer = hfCaseOfficer
                    .txtCaseOfficer = txtCaseOfficer
                    .imgtxtCaseOfficerErr = imgtxtCaseOfficerErr
                    .txtCaseOfficerContactNo = txtCaseContactNo
                    .imgtxtCaseContactNoErr = imgtxtCaseContactNoErr
                    .hfSubjectOfficer = hfSubjectOfficer
                    .txtSubjectOfficer = txtSubjectOfficer
                    .imgtxtSubjectContactNoErr = imgtxtSubjectContactNoErr
                    .txtSubjectOfficerContactNo = txtSubjectContactNo
                    .imgtxtSubjectOfficerErr = imgtxtSubjectOfficerErr

                    .txtSPID = txtSPIDNew
                    .hfPracticeSeqNo = hdfPracticeSeq

                    .txtVisitDate = txtVisitDate
                    .imgVisitDateErr = imgtxtVisitDateErr
                    .txtVisitTimeFrom = txtStartVisitTime
                    .imgtxtVisitTimeFromErr = imgtxtStartVisitTimeErr
                    .txtVisitTimeTo = txtEndVisitTime
                    .imgtxtVisitTimeToErr = imgtxtEndVisitTimeErr

                    .txtConfirmDate = txtConfirmDate
                    .imgtxtConfirmDateErr = imgtxtConfirmDateErr
                    .txtConfirmationWith = txtConfirmationWith
                    .imgtxtConfirmationWithErr = imgtxtConfirmationWithErr

                    .ddlFormCondition = ddlFormCondition
                    .imgddlFormConditionErr = imgddlFormConditionErr
                    .txtFormConditionRm = txtFormConditionRm
                    .imgtxtFormConditionRmErr = imgtxtFormConditionRmErr

                    .ddlMeansOfCommunication = ddlMeansofCommunication
                    .imgddlMeansofCommunicationErr = imgddlMeansofCommunicationErr
                    .txtMeansOfCommunicationEmail = txtMeansofCommunicationEmail
                    .imgtxtMeansOfCommunicationEmailErr = imgtxtMeansofCommunicationEmailErr
                    .txtMeansOfCommunicationFax = txtMeansofCommunicationFax
                    .imgtxtMeansOfCommunicationFaxErr = ImgtxtMeansofCommunicationFaxErr

                    .rdoLowRiskClaim = rdoAddLowRiskClaim
                    .imgrdoLowRiskClaimErr = imgrdoAddLowRiskClaimErr

                    .txtRemarks = txtRemarks
                    .imgtxtRemarksErr = imgtxtRemarksErr
                End With
            Case PageMode.ModeEdit
                With DetailDataField
                    .chkListTypeOfInspection = chkListEdtTypeofInspection
                    .txtReferFileRefNoA1 = txtEdtRRNA1
                    .txtReferFileRefNoA2 = txtEdtRRNA2
                    .txtReferFileRefNoA3 = txtEdtRRNA3
                    .txtReferFileRefNoA4 = txtEdtRRNA4
                    .txtReferFileRefNoA5 = txtEdtRRNA5
                    .txtReferFileRefNoB1 = txtEdtRRNB1
                    .txtReferFileRefNoB2 = txtEdtRRNB2
                    .txtReferFileRefNoB3 = txtEdtRRNB3
                    .txtReferFileRefNoB4 = txtEdtRRNB4
                    .txtReferFileRefNoB5 = txtEdtRRNB5
                    .txtReferFileRefNoC1 = txtEdtRRNC1
                    .txtReferFileRefNoC2 = txtEdtRRNC2
                    .txtReferFileRefNoC3 = txtEdtRRNC3
                    .txtReferFileRefNoC4 = txtEdtRRNC4
                    .txtReferFileRefNoC5 = txtEdtRRNC5

                    .officerList = edtOfficerList
                    .hfCaseOfficer = hfEdtCaseOfficer
                    .txtCaseOfficer = txtEdtCaseOfficer
                    .imgtxtCaseOfficerErr = imgtxtEdtCaseOfficerErr
                    .txtCaseOfficerContactNo = txtEdtCaseContactNo
                    .imgtxtCaseContactNoErr = imgtxtEdtCaseContactNoErr
                    .hfSubjectOfficer = hfEdtSubjectOfficer
                    .txtSubjectOfficer = txtEdtSubjectOfficer
                    .imgtxtSubjectContactNoErr = imgtxtEdtSubjectContactNoErr
                    .txtSubjectOfficerContactNo = txtEdtSubjectContactNo
                    .imgtxtSubjectOfficerErr = imgtxtEdtSubjectOfficerErr

                    .txtSPID = txtEditSPID
                    .hfPracticeSeqNo = hdfEditPracticeSeq

                    .txtVisitDate = txtEdtVisitDate
                    .imgVisitDateErr = imgtxtEdtVisitDateErr
                    .txtVisitTimeFrom = txtEdtStartVisitTime
                    .imgtxtVisitTimeFromErr = imgtxtEdtStartVisitTimeErr
                    .txtVisitTimeTo = txtEdtEndVisitTime
                    .imgtxtVisitTimeToErr = imgtxtEdtEndVisitTimeErr
                    .txtConfirmDate = txtEdtConfirmDate
                    .imgtxtConfirmDateErr = imgtxtEdtConfirmDateErr
                    .txtConfirmationWith = txtEdtConfirmationWith
                    .imgtxtConfirmationWithErr = imgtxtEdtConfirmationWithErr

                    .ddlFormCondition = ddlEdtFormCondition
                    .imgddlFormConditionErr = imgddlEdtFormConditionErr
                    .txtFormConditionRm = txtEdtFormConditionRm
                    .imgtxtFormConditionRmErr = imgtxtEdtFormConditionRmErr
                    .ddlMeansOfCommunication = ddlEdtMeansofCommunication
                    .imgddlMeansofCommunicationErr = imgddlEdtMeansofCommunicationErr
                    .txtMeansOfCommunicationEmail = txtEdtMeansofCommunicationEmail
                    .imgtxtMeansOfCommunicationEmailErr = imgtxtEdtMeansofCommunicationEmailErr
                    .txtMeansOfCommunicationFax = txtEdtMeansofCommunicationFax
                    .imgtxtMeansOfCommunicationFaxErr = imgtxtEdtMeansofCommunicationFaxErr

                    .rdoLowRiskClaim = rdoEdtLowRiskClaim
                    .imgrdoLowRiskClaimErr = imgrdoEdtLowRiskClaimErr
                    .txtRemarks = txtEdtRemarks
                    .imgtxtRemarksErr = imgtxtEdtRemarksErr
                End With
        End Select

        DetailDataField.pageMode = mode
        Return DetailDataField
    End Function

    Public Sub SearchInspectionRecord()
        MultiViewIRM.SetActiveView(vSearchResult)
        Dim udtBLLSearchResult As BaseBLL.BLLSearchResult = GetInspectionRecordList(True, True)

        Select Case udtBLLSearchResult.SqlErrorMessage
            Case BaseBLL.EnumSqlErrorMessage.Normal
                BindInspectionRecord(udtBLLSearchResult)

            Case BaseBLL.EnumSqlErrorMessage.OverResultList1stLimit
                Me.udcMsgBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, MsgCode.MSG00009))
                Me.udcMsgBox.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00003, "Search Fail")
                Return

            Case BaseBLL.EnumSqlErrorMessage.OverResultListOverrideLimit
                Me.udcMsgBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, MsgCode.MSG00017))
                Me.udcMsgBox.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00003, "Search Fail")
                Return
            Case Else
                Throw New Exception("Error: Class = [HCVU.InspectionRecordManagement], Method = [ibtnSearch_Click], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")
        End Select
    End Sub

    Private Function GetInspectionRecordList(ByVal blnOverrideResultLimit As Boolean, ByVal blnForceUnlimitResult As Boolean) As BaseBLL.BLLSearchResult
        Dim bllSearchResult As BaseBLL.BLLSearchResult = Nothing
        lblInspectionID.Text = If(txtInspectionID.Text = "", Me.GetGlobalResourceObject("Text", "Any"), udtGeneralFunction.getSeqNo_Prefix_Without_Update_ProfileNum("INSID", "ALL") + txtInspectionID.Text)
        lblFileReferenceNo.Text = If(txtFileReferenceNo.Text = "", Me.GetGlobalResourceObject("Text", "Any"), udtGeneralFunction.getSystemParameter("Inspection_FileRefNo_Prefix") + txtFileReferenceNo.Text)
        lblSPID.Text = If(txtSPID.Text = "", Me.GetGlobalResourceObject("Text", "Any"), txtSPID.Text)
        lblTypeofInspection.Text = If(ddlTypeofInspection.SelectedValue = "", Me.GetGlobalResourceObject("Text", "Any"), ddlTypeofInspection.SelectedItem.Text)
        lblStartVisitDate.Text = If(txtStartVisitDate.Text = "", Me.GetGlobalResourceObject("Text", "Any"), txtStartVisitDate.Text)
        lblEndVisitDate.Text = If(txtEndVisitDate.Text = "", Me.GetGlobalResourceObject("Text", "Any"), txtEndVisitDate.Text)
        lblStatus.Text = If(ddlStatus.SelectedValue = "", Me.GetGlobalResourceObject("Text", "Any"), ddlStatus.SelectedItem.Text)
        lblOwner.Text = rdlOwner.SelectedItem.Text

        Dim inspectionid = txtInspectionID.Text.Trim
        Dim frNo = txtFileReferenceNo.Text.Trim
        Dim spid = txtSPID.Text.Trim
        Dim toi = ddlTypeofInspection.SelectedValue
        Dim status = ddlStatus.SelectedValue
        Dim startDate = txtStartVisitDate.Text.Trim
        Dim endDate = txtEndVisitDate.Text.Trim
        Dim OnlyForOwner As Integer = rdlOwner.SelectedValue

        ' Search Data by Input Value
        Dim startDateFilter As DateTime = DateTime.MinValue
        Dim endDateFilter As DateTime = DateTime.MinValue
        Dim startDateValid As Boolean = False, endDateValid As Boolean = False

        If Not String.IsNullOrWhiteSpace(startDate) Then
            startDateValid = DateTime.TryParseExact(startDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, startDateFilter)
        End If
        If Not String.IsNullOrWhiteSpace(endDate) Then
            endDateValid = DateTime.TryParseExact(endDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, endDateFilter)
        End If
        Dim param As New GetInspectionParameter With {
            .InspectionID = If(inspectionid = "", "", udtGeneralFunction.getSeqNo_Prefix_Without_Update_ProfileNum("INSID", "ALL") + inspectionid),
            .FileReferenceNo = frNo,
            .ReferredReferenceNo = "",
            .SPID = spid,
            .MainTypeOfInspection = toi,
            .RecordStatus = status,
            .StartDtm = IIf(startDateValid, startDateFilter, Nothing),
            .EndDtm = IIf(endDateValid, endDateFilter, Nothing),
            .UserId = udtHCVUUserBLL.GetHCVUUser.UserID,
            .OnlyForOwner = OnlyForOwner
            }
        bllSearchResult = udtInspectionRecordBLL.SearchInspectionRecordByAny(FunctionCode, param, blnOverrideResultLimit, blnForceUnlimitResult)
        Return bllSearchResult
    End Function
    Public Sub BindInspectionRecord(ByVal udtBLLSearchResult As BaseBLL.BLLSearchResult)
        Dim dt As New DataTable
        Dim intRowCount As Integer
        Dim blnShowResultList As Boolean = False
        Try
            dt = CType(udtBLLSearchResult.Data, DataTable)
            dt.DefaultView.Sort = "Visit_Date DESC"
            dt = dt.DefaultView.ToTable()

            'Save Session
            Session(SESS_SearchResultDataTable) = dt
        Catch ex As Exception
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
            Me.GridViewDataBind(Me.gvSearchResult, dt, "Visit_Date", "DESC", False)
        End If
    End Sub

    Private Sub ClearTargetVisitByMode(ByVal mode As String)
        Select Case mode
            Case PageMode.ModeNew
                txtSPIDNew.Text = ""
                lblSPStatus.Text = ""
                lblPracticeStatus.Text = ""
                lblPracticeStatus.Visible = False
                imgtxtSPIDNewErr.Visible = False
                imgddlPracticeErr.Visible = False
                setAfterClearTarget(PageMode.ModeNew)
                ClearVisitTargetForm(PageMode.ModeNew)
            Case PageMode.ModeEdit
                txtEditSPID.Text = ""
                lblEditSPStatus.Text = ""
                lblEditPracticeStatus.Text = ""
                lblEditPracticeStatus.Visible = False
                imgtxtEditSPIDErr.Visible = False
                imgddlEditPracticeErr.Visible = False
                setAfterClearTarget(PageMode.ModeEdit)
                ClearVisitTargetForm(PageMode.ModeEdit)
        End Select
    End Sub
    Protected Sub SearchSPEdit()
        Me.udcMsgBox.Visible = False
        Me.udcInfoMsgBox.Visible = False

        imgtxtEditSPIDErr.Visible = False

        Dim strSPID As String = txtEditSPID.Text.Trim
        Dim dtResult As New DataTable()

        'Init Edit Practice
        initDropDownList(ddlEditPractice)
        If True Then

            If strSPID = "" Then
                imgtxtEditSPIDErr.Visible = True
                udcMsgBox.AddMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00316)
                udcMsgBox.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00004, AuditLogDesc.IRM_SearchSP_Fail)
                Return
            End If

            Dim sm As SystemMessage = Me.udtValidator.chkSPID(strSPID)
            If IsNothing(sm) Then
                Dim udtAccountChangeMaintBLL As New AccountChangeMaintenance.AccountChangeMaintenanceBLL
                Dim inspectionRecord As InspectionRecordModel = CType(Session(SESS_InspectionRecordModel), InspectionRecordModel)
                If Not GetReadyServiceProvider(strSPID, PageMode.ModeEdit, inspectionRecord.InspectionID) Then
                    ' No Record Found
                    udcMsgBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001, "{SPID}", strSPID)

                    imgtxtEditSPIDErr.Visible = True
                    Me.udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00004, AuditLogDesc.IRM_SearchSP_Fail)
                Else
                    setAfterSearchTarget(PageMode.ModeEdit)
                End If
            Else
                imgtxtEditSPIDErr.Visible = True
                Me.udcMsgBox.AddMessage(sm)
                Me.udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00004, AuditLogDesc.IRM_SearchSP_Fail)
            End If

        End If
    End Sub
#End Region

#Region "Download Report"

#End Region

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