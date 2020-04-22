Imports System.Data.SqlClient
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.eHealthAccountDeathRecord
Imports Common.Format
Imports Common.Component.HCVUUser
Imports Common.Component.RedirectParameter
Imports Common.ComFunction
Imports HCVU.Component.Menu

Partial Public Class eHealthAccountDeathRecordMaint
    Inherits System.Web.UI.UserControl

#Region "Private Class"
    Private Class SESS
        Public Const DeathRecordEnquiryResult As String = "010306_DeathRecordEnquiryResult"
    End Class

    Private Class VS
        Public Const UnmaskPopup As String = "010307_UnmaskPopup"
    End Class

    Private Class PopupStatus
        Public Const Active As String = "A"
        Public Const Closed As String = "C"
    End Class

    Public Enum enumMode
        Enquiry
        Maintenance
    End Enum

#End Region

#Region "Audit Log Description"
    Public Class AuditLogDesc
        Public Const PageLoad As String = "Death Record Enquiry/Maintenacne  Page Load"
        Public Const PageLoad_ID As String = LogID.LOG00001

        Public Const ShowSearchView As String = "Search - Show search view"
        Public Const ShowSearchView_ID As String = LogID.LOG00002

        Public Const SearchClick As String = "Search - Search button click"
        Public Const SearchClick_ID As String = LogID.LOG00003

        Public Const NoSearchCriteria As String = "Search - Search criteria is empty"
        Public Const NoSearchCriteria_ID As String = LogID.LOG00004

        Public Const SearchFail As String = "Search - Search Fail"
        Public Const SearchFail_ID As String = LogID.LOG00005

        Public Const SearchSuccess As String = "Search - Search Success"
        Public Const SearchSuccess_ID As String = LogID.LOG00006

        Public Const ShowDetailView As String = "Detail - Show detail view"
        Public Const ShowDetailView_ID As String = LogID.LOG00007

        Public Const BackClick As String = "Detail - Back click"
        Public Const BackClick_ID As String = LogID.LOG00008

        Public Const ShowConfirmRemove As String = "Detail - Show confirm remove death record"
        Public Const ShowConfirmRemove_ID As String = LogID.LOG00009

        Public Const RemoveClick As String = "Detail - Remove click"
        Public Const RemoveClick_ID As String = LogID.LOG00010

        Public Const ConfirmRemovePopupShow As String = "Detail - Confirm remove popup show"
        Public Const ConfirmRemovePopupShow_ID As String = LogID.LOG00011

        Public Const ConfirmRemoveClick As String = "Remove Popup - Confirm remove click"
        Public Const ConfirmRemoveClick_ID As String = LogID.LOG00012

        Public Const CancelRemoveClick As String = "Remove Popup - Cancel remove click"
        Public Const CancelRemoveClick_ID As String = LogID.LOG00013

        Public Const ShowRemoveCompleteView As String = "Show remove complete view"
        Public Const ShowRemoveCompleteView_ID As String = LogID.LOG00014

        Public Const ReturnClick As String = "Remove Complete - Return click"
        Public Const ReturnClick_ID As String = LogID.LOG00015

        Public Const ManagementClick As String = "Detail - Management click"
        Public Const ManagementClick_ID As String = LogID.LOG00016

        Public Const UnmaskCheckClick As String = "Detail - Mask Identity Document No. click"
        Public Const UnmaskCheckClick_ID As String = LogID.LOG00017

        Public Const UnmaskCheckSuccess As String = "Detail - Unmask Identity Document No. success"
        Public Const UnmaskCheckSuccess_ID As String = LogID.LOG00018

        Public Const MaskCheckSuccess As String = "Detail - Mask Identity Document No. success"
        Public Const MaskCheckSuccess_ID As String = LogID.LOG00019

    End Class
#End Region

#Region "Private Variable"
    Private _udtAuditLogEntry As AuditLogEntry
#End Region

    Private ReadOnly Property FunctionCode() As String
        Get
            Return CType(Me.Page, BasePage).FunctionCode
        End Get
    End Property

    Private ReadOnly Property UserID() As String
        Get
            Return (New HCVUUserBLL).GetHCVUUser.UserID
        End Get
    End Property

    Private _enumMode As enumMode = enumMode.Enquiry
    Public Property Mode() As enumMode
        Get
            Return _enumMode
        End Get
        Set(ByVal value As enumMode)
            _enumMode = value
        End Set
    End Property

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        _udtAuditLogEntry = New AuditLogEntry(FunctionCode)
    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            _udtAuditLogEntry.WriteLog(AuditLogDesc.PageLoad_ID, AuditLogDesc.PageLoad)

            Me.chkDMaskDocumentNo.Visible = (New GeneralFunction).CheckTurnOnInstantUnmaskIdentityDocumentNo = GeneralFunction.EnumTurnOnStatus.Yes
        End If

        If Not IsPostBack Then
            GotoSearch()
            HandleRedirectAction()
        End If

        Me.InitializeButton()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Select Case ViewState(VS.UnmaskPopup)
            Case PopupStatus.Active
                popupUnmask.Show()

        End Select
    End Sub

    Protected Sub ibtnSSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        _udtAuditLogEntry.WriteLog(AuditLogDesc.SearchClick_ID, AuditLogDesc.SearchClick)

        Search(Me.txtSDocumentNo.Text)
    End Sub

    Protected Sub chkDMaskDocumentNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        _udtAuditLogEntry.AddDescripton("Checked change to", IIf(chkDMaskDocumentNo.Checked, "T", "F"))
        _udtAuditLogEntry.WriteLog(AuditLogDesc.UnmaskCheckClick_ID, AuditLogDesc.UnmaskCheckClick)

        If chkDMaskDocumentNo.Checked Then
            ' Unchecked -> Checked
            GotoDetail(Session(SESS.DeathRecordEnquiryResult), Me.chkDMaskDocumentNo.Checked)

            _udtAuditLogEntry.WriteLog(AuditLogDesc.MaskCheckSuccess_ID, AuditLogDesc.MaskCheckSuccess)
        Else
            ' Checked -> Unchecked
            popupUnmask.Show()
            ViewState(VS.UnmaskPopup) = PopupStatus.Active
            InitPopupUnmask()

        End If

    End Sub

    Private Sub InitPopupUnmask()
        ' CRE12-014 - Relax 500 row limit in back office platform [Start][Twinsen]
        popupUnmask.PopupDragHandleControlID = udcPUInputToken.Header.ClientID
        udcPUInputToken.HeaderText = Me.GetGlobalResourceObject("Text", "ConfirmUnmaskIdentityDocumentNo")
        ' CRE12-014 - Relax 500 row limit in back office platform [End] [Twinsen]
        udcPUInputToken.Message = Me.GetGlobalResourceObject("Text", "ConfirmUnmaskMessage")
        udcPUInputToken.Build()

    End Sub

    Protected Sub ibtnDBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        _udtAuditLogEntry.WriteLog(AuditLogDesc.BackClick_ID, AuditLogDesc.BackClick)

        GotoSearch()
    End Sub

    Protected Sub ibtnDRemove_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        _udtAuditLogEntry.WriteLog(AuditLogDesc.RemoveClick_ID, AuditLogDesc.RemoveClick)

        popupSRemoveFile.Show()

        _udtAuditLogEntry.WriteLog(AuditLogDesc.ShowConfirmRemove_ID, AuditLogDesc.ShowConfirmRemove)
    End Sub

    Protected Sub ibtnPopupSRemoveFileConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        _udtAuditLogEntry.WriteLog(AuditLogDesc.ConfirmRemoveClick_ID, AuditLogDesc.ConfirmRemoveClick)

        Dim objDeathRecordEntry As DeathRecordEntryModel = Session(SESS.DeathRecordEnquiryResult)

        Dim objeHealthAccountDeathRecordBLL As New eHealthAccountDeathRecordBLL
        objeHealthAccountDeathRecordBLL.UpdateDeathRecordEntryStatusToRemoved(objDeathRecordEntry.DocNo, UserID)

        GotoRemoveComplete()
    End Sub

    Protected Sub ibtnPopupSRemoveFileCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        _udtAuditLogEntry.WriteLog(AuditLogDesc.CancelRemoveClick_ID, AuditLogDesc.CancelRemoveClick)
    End Sub

    Protected Sub ibtnRCReturn_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        _udtAuditLogEntry.WriteLog(AuditLogDesc.ReturnClick_ID, AuditLogDesc.ReturnClick)

        GotoSearch()
    End Sub

    Protected Sub InitializeButton()

        ' Setup Management button 
        ' --------------------------------------------------------------------
        If Session(SESS.DeathRecordEnquiryResult) Is Nothing Then Exit Sub
        Dim objDeathRecordEntry As DeathRecordEntryModel = Session(SESS.DeathRecordEnquiryResult)
        BuildRedirectButton(ibtnDManagement, objDeathRecordEntry)

    End Sub

    Protected Function Search(ByVal strDocNo As String) As Boolean
        ' Reset
        udcMessageBox.Visible = False
        udcInfoMessageBox.Visible = False
        imgSDocumentNo.Visible = False
        Session(SESS.DeathRecordEnquiryResult) = Nothing

        ' Validate Input 
        If Not ValidateInput(strDocNo) Then
            imgSDocumentNo.Visible = True

            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001)
            Me.udcMessageBox.BuildMessageBox("ValidationFail", _udtAuditLogEntry, AuditLogDesc.NoSearchCriteria_ID, AuditLogDesc.NoSearchCriteria)
            Return False
        End If

        ' Retrieve data from database
        Dim udtFormatter As New Formatter
        Dim objDeathRecordEntry As DeathRecordEntryModel = Nothing

        objDeathRecordEntry = (New eHealthAccountDeathRecordBLL).GetDeathRecordEntry(udtFormatter.formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, udtFormatter.formatHKIDInternal(strDocNo)))

        If objDeathRecordEntry.IsDead Then
            _udtAuditLogEntry.AddDescripton("NoOfRecord", 1)
            _udtAuditLogEntry.WriteLog(AuditLogDesc.SearchSuccess_ID, AuditLogDesc.SearchSuccess)

            ' Show detail view
            Session(SESS.DeathRecordEnquiryResult) = objDeathRecordEntry

            ' Show death record entry detail with mask
            GotoDetail(objDeathRecordEntry, True)

            Return True
        Else
            _udtAuditLogEntry.AddDescripton("NoOfRecord", 0)
            _udtAuditLogEntry.WriteLog(AuditLogDesc.SearchFail_ID, AuditLogDesc.SearchFail)

            ' Show No record file
            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
            udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001)
            udcInfoMessageBox.BuildMessageBox()

            Return False
        End If

    End Function

    Public Sub GotoSearch()
        Me.mvCore.SetActiveView(Me.vSearchCriteria)
        udcMessageBox.Visible = False
        udcInfoMessageBox.Visible = False

        _udtAuditLogEntry.WriteLog(AuditLogDesc.ShowSearchView_ID, AuditLogDesc.ShowSearchView)
    End Sub

    Protected Sub GotoDetail(ByVal objDeathRecordEntry As DeathRecordEntryModel, ByVal blnMasked As Boolean)
        Me.mvCore.SetActiveView(Me.vDetail)

        Me.chkDMaskDocumentNo.Checked = blnMasked

        ' Set value
        Me.lblDDocumentNo.Text = objDeathRecordEntry.FormattedDocNo(blnMasked)
        Me.lblDName.Text = objDeathRecordEntry.EnglishName
        Me.lblDDateOfDeath.Text = objDeathRecordEntry.FormattedDOD
        Me.lblDDateOfRegistration.Text = objDeathRecordEntry.FormattedDOR

        InitializeButton()

        ' Handle different mode display item
        Select Case Mode
            Case enumMode.Enquiry
                Me.ibtnDRemove.Visible = False
                Me.ibtnDManagement.Visible = True
            Case enumMode.Maintenance
                Me.ibtnDRemove.Visible = True
                Me.ibtnDManagement.Visible = False
            Case Else
                Throw New Exception(String.Format("eHealthAccountDeathRecordMaint.GotoDetail: Unhandled Mode()", Mode.ToString()))
        End Select

        _udtAuditLogEntry.WriteLog(AuditLogDesc.ShowDetailView_ID, AuditLogDesc.ShowDetailView)
    End Sub

    Public Sub GotoRemoveComplete()
        Me.mvCore.SetActiveView(Me.vRemoveComplete)

        udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00002)
        udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
        udcInfoMessageBox.BuildMessageBox()

        _udtAuditLogEntry.WriteLog(AuditLogDesc.ShowRemoveCompleteView_ID, AuditLogDesc.ShowRemoveCompleteView)
    End Sub

    Private Function ValidateInput(ByVal strDocNo As String) As Boolean
        If strDocNo.Trim = String.Empty Then Return False

        Return True
    End Function

    Private Sub HandleRedirectAction()
        Dim udtRedirectParameterBLL As New RedirectParameterBLL
        Dim udtRedirectParameter As RedirectParameterModel = udtRedirectParameterBLL.GetFromSession()
        If IsNothing(udtRedirectParameter) Then Return

        udtRedirectParameterBLL.RemoveFromSession()
        udtRedirectParameterBLL.WriteAuditLog(FunctionCode, Me.Page, udtRedirectParameter)

        If udtRedirectParameter.ActionList.Contains(RedirectParameterModel.EnumRedirectAction.Search) Then
            If Search(udtRedirectParameter.EHealthAccountDocNo) Then
                Me.ibtnDBack.Visible = False
            Else
                Me.ibtnDBack.Visible = True
            End If
        End If
    End Sub

    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <param name="btn"></param>
    ''' <remarks></remarks>
    Private Sub BuildRedirectButton(ByVal btn As CustomControls.CustomImageButton, ByVal objDeathRecordEntry As DeathRecordEntryModel)
        btn.SourceFunctionCode = CType(Me.Page, BasePage).FunctionCode
        btn.TargetFunctionCode = FunctCode.FUNT010307
        btn.TargetUrl = RedirectHandler.AppendPageKeyToURL(GetURLByFunctionCode(FunctCode.FUNT010307))

        btn.Build()

        btn.ConstructNewRedirectParameter()
        btn.RedirectParameter.EHealthAccountDocNo = objDeathRecordEntry.DocNo
        btn.RedirectParameter.ActionList.Add(RedirectParameterModel.EnumRedirectAction.Search)
        btn.RedirectParameter.ActionList.Add(RedirectParameterModel.EnumRedirectAction.ViewDetail)
    End Sub

    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <param name="strFunctionCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetURLByFunctionCode(ByVal strFunctionCode As String) As String
        Dim dr() As DataRow = (New MenuBLL).GetMenuItemTable.Select(String.Format("Function_Code='{0}'", strFunctionCode))
        If dr.Length <> 1 Then Throw New Exception("eHealthAccountDeathRecordMatching.GetURLByFunctionCode: Unexpected no. of rows")
        Return dr(0)("URL")
    End Function

    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ibtnDManagement_Click(ByVal sender As System.Web.UI.WebControls.ImageButton, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDManagement.Click
        _udtAuditLogEntry.WriteLog(AuditLogDesc.ManagementClick_ID, AuditLogDesc.ManagementClick)

        Dim btn As CustomControls.CustomImageButton = sender.Parent
        btn.TargetUrl = RedirectHandler.AppendPageKeyToURL(GetURLByFunctionCode(FunctCode.FUNT010307))
        btn.Redirect()
    End Sub

    'Protected Sub ibtnDManagement_Click(ByVal sender As ImageButton, ByVal e As System.Web.UI.ImageClickEventArgs)

    'End Sub

    '

    Protected Sub ibtnPopupUnmaskConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcPUInputToken.Confirm_Click
        ViewState(VS.UnmaskPopup) = PopupStatus.Closed

        GotoDetail(Session(SESS.DeathRecordEnquiryResult), chkDMaskDocumentNo.Checked)

        _udtAuditLogEntry.WriteLog(AuditLogDesc.UnmaskCheckSuccess_ID, AuditLogDesc.UnmaskCheckSuccess)
    End Sub

    Protected Sub ibtnPopupUnmaskCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcPUInputToken.Cancel_Click
        ViewState(VS.UnmaskPopup) = PopupStatus.Closed

        chkDMaskDocumentNo.Checked = True

    End Sub


End Class