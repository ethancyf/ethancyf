Imports System.Web.Security.AntiXss
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.ERNProcessed
Imports Common.Component.HCVUUser
Imports Common.Component.InternetMail
Imports Common.Component.MedicalOrganization
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.RSA_Manager
Imports Common.Component.Scheme
Imports Common.Component.SchemeInformation
Imports Common.Component.ServiceProvider
Imports Common.Component.Token
Imports Common.DataAccess
Imports Common.Format
Imports Common.Validation
Imports HCVU.AccountChangeMaintenance
Imports HCVU.spPrintFunction
Imports HCVU.spSummaryView
Imports Common.ComFunction.GeneralFunction
Imports Common.ComFunction.AccountSecurity
Imports Common.PCD
Imports Common.PCD.WebService.Interface

Partial Public Class spMaintenance
    Inherits BasePageWithControl

#Region "Fields"

    Private udtAccountChangeMaintenanceBLL As New AccountChangeMaintenanceBLL
    Private udtFormatter As New Formatter
    Private udtHCVUUserBLL As New HCVUUserBLL
    Private udtInternetMailBLL As New InternetMailBLL
    Private udtSchemeBackOfficeBLL As New SchemeBackOfficeBLL
    Private udtSPProfileBLL As New SPProfileBLL
    Private udtServiceProviderBLL As New ServiceProviderBLL
    Private udtMOBLL As New MedicalOrganizationBLL
    Private udtValidator As New Validator

    Private udtAuditLogEntry As AuditLogEntry
    Private SM As SystemMessage

    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
    Private _strERN As String = String.Empty
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

#End Region

#Region "Constants"

    Private Const CssIbtnEnabled As String = "ibtnEnabled"
    Private Const CssIbtnDisabled As String = "ibtnDisabled"

    Private Const ViewIndexSearchCriteria As Integer = 0
    Private Const ViewIndexSearchResult As Integer = 1
    Private Const ViewIndexDetails As Integer = 2
    Private Const ViewIndexSPActionDetails As Integer = 3
    Private Const ViewIndexPracticeActionDetails As Integer = 4
    Private Const ViewIndexComplete As Integer = 5
    Private Const ViewIndexError As Integer = 6

    Private Const strMONotAvailable As String = "0"



#End Region

#Region "Session Constants"

    Public Const SESS_ERN As String = "Enrol_Ref_No"
    Public Const SESS_SPID As String = "SP_ID"
    Public Const SESS_PPIStatus As String = "PPI_Status"
    Public Const SESS_SearchResultList As String = "MaintenanceResult"
    Public Const SESS_SelectedPractice As String = "SelectedPractice"
    Public Const SESS_MaintenanceTokenSerialNo As String = "MaintenanceTokenSerialNo"
    Public Const SESS_TokenProject As String = "TokenProject"
    Public Const SESS_SPDelisted As String = "SPDelisted"
    Public Const SESS_ShowReactivatePracticeBtn As String = "ShowReactivatePracticeBtn"
    Public Const SESS_ShowSuspendPracticeBtn As String = "ShowSuspendPracticeBtn"
    Public Const SESS_ShowDelistPracticeBtn As String = "ShowDelistPracticeBtn"
    Public Const SESS_SPSchemeInformation As String = "SPSchemeInformation"
    Private Const SESS_SPMaintenanceHandledScheme As String = "SPMaintenanceHandledScheme"
    Private Const SESS_SPMaintenancePracticeSchemeInfoList As String = "SESS_SPMaintenancePracticeSchemeInfoList"

    ' CRE13-029 - RSA Server Upgrade [Start][Lawrence]
    Private Const VS_RSARepairTokenStatus As String = "VS_RSARepairTokenStatus"
    Private Const VS_RSARepairTokenSessionIDMain As String = "VS_RSARepairTokenSessionIDMain"
    Private Const VS_RSARepairTokenSessionIDSub As String = "VS_RSARepairTokenSessionIDSub"
    ' CRE13-029 - RSA Server Upgrade [End][Lawrence]

    Private Const SESS_SPMaintenancePracticeEnrolledDHCList As String = "SPMaintenancePracticeEnrolledDHCList" 'CRE20-006 DHC Integration [Nichole]
#End Region

#Region "Enums"

    ' CRE13-029 - RSA Server Upgrade [Start][Lawrence]
    Private Enum RSARepairTokenStatus
        NA = 0
        WaitForFirstPasscode = 1
        WaitForSecondPasscode = 2
    End Enum
    ' CRE13-029 - RSA Server Upgrade [End][Lawrence]

#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"

        Response.AddHeader("Pragma", "no-cache")

        FunctionCode = FunctCode.FUNT010201
        ' Write Audit Log
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        If Not IsPostBack Then
            udtAuditLogEntry.WriteLog(LogID.LOG00000, "Service Provider Maintenance loaded")

            ' Bind Health Profession
            ddlSPHealthProf.DataSource = udtSPProfileBLL.GetHealthProf

            ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

            ' -----------------------------------------------------------------------------------------

            ddlSPHealthProf.DataValueField = "ServiceCategoryCode"
            ddlSPHealthProf.DataTextField = "ServiceCategoryDesc"

            ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

            ddlSPHealthProf.DataBind()

            ' Bind Scheme
            ddlScheme.DataSource = udtSPProfileBLL.GetMasterScheme
            ddlScheme.DataValueField = "SchemeCode"
            ddlScheme.DataTextField = "DisplayCode"
            ddlScheme.DataBind()

            panActionBtn.Visible = True

            ' Handle double post-back
            Dim strBrowser As String = String.Empty
            Try
                If Not HttpContext.Current.Request.Browser Is Nothing Then
                    strBrowser = HttpContext.Current.Request.Browser.Type
                End If
            Catch ex As Exception
                strBrowser = String.Empty
            End Try

            MyBase.preventMultiImgClick(Me.Page.ClientScript, Me.ibtnResendEmailSend)
            MyBase.preventMultiImgClick(Me.Page.ClientScript, Me.ibtnReprintLetterPrint)

        Else
            ' CRE12-001 eHS and PCD integration [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            If Me.ucTypeOfPracticePopup.Showing Then
                Me.ModalPopupExtenderTypeOfPractice.Show()
            End If
            ' CRE12-001 eHS and PCD integration [End][Koala]
        End If

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Select Case MultiViewMaintenance.ActiveViewIndex
            Case ViewIndexSearchCriteria
                pnlMaintenance.DefaultButton = ibtnSearch.ID

            Case ViewIndexDetails
                ibtnAmendedRecord.Visible = hfUnderModify.Value.Trim <> String.Empty

                Select Case Session(SESS_PrintFunctionStatus)
                    Case String.Empty
                        ' Nothing here
                    Case PrintFunctionStatus.ActivePrintFunction
                        ' Nothing here
                    Case PrintFunctionStatus.ClosePrintFunction
                        Session.Remove(SESS_PrintFunctionStatus)
                        hfPopupStatus.Value = Math.DivRem(CInt(hfPopupStatus.Value), 10, Nothing) * 10
                    Case PrintFunctionStatus.FinishPrintFunction
                        Session.Remove(SESS_PrintFunctionStatus)

                        ' CRE16-018 Display SP tentative email in HCVU [Start][Winnie]
                        Select Case hfPopupStatus.Value
                            Case SPMaintenancePopupStatus.ResendActivationEmail, _
                                SPMaintenancePopupStatus.ResendSchemeEnrolmentEmail, _
                                SPMaintenancePopupStatus.ResendDelistEmail

                                ' Refresh the data after resend email
                                btnSpDetails_Click(Nothing, Nothing)
                        End Select
                        ' CRE16-018 Display SP tentative email in HCVU [End][Winnie]

                        ShowCompleteSendMessage(CInt(hfPopupStatus.Value))
                        hfPopupStatus.Value = SPMaintenancePopupStatus.Closed

                        ' CRE16-018 Display SP tentative email in HCVU [Start][Winnie]
                    Case PrintFunctionStatus.ErrorPrintFunction
                        Session.Remove(SESS_PrintFunctionStatus)

                        ' Display general error msg
                        msgBox.AddMessage(New SystemMessage("990001", "D", LogID.LOG00011))
                        msgBox.BuildMessageBox("UpdateFail")

                        hfPopupStatus.Value = SPMaintenancePopupStatus.Closed
                        ' CRE16-018 Display SP tentative email in HCVU [End][Winnie]
                End Select

                Select Case hfPopupStatus.Value
                    Case String.Empty, SPMaintenancePopupStatus.Closed
                        ' Nothing here
                    Case SPMaintenancePopupStatus.ResendEmail
                        popupResendEmail.Show()
                    Case SPMaintenancePopupStatus.ResendActivationEmail
                        popupPrintFunction.Show()
                    Case SPMaintenancePopupStatus.ResendSchemeEnrolmentEmail
                        popupPrintFunction.Show()
                    Case SPMaintenancePopupStatus.ResendDelistEmail
                        popupPrintFunction.Show()
                    Case SPMaintenancePopupStatus.ReprintLetter
                        popupReprintLetter.Show()
                    Case SPMaintenancePopupStatus.ReprintAcknowledgementLetter
                        popupPrintFunction.Show()
                    Case SPMaintenancePopupStatus.ReprintSchemeEnrolmentLetter
                        popupPrintFunction.Show()
                        ' CRE13-029 - RSA Server Upgrade [Start][Lawrence]
                    Case SPMaintenancePopupStatus.CheckToken
                        popupCheckToken.Show()
                        ' CRE13-029 - RSA Server Upgrade [End][Lawrence]
                End Select

        End Select

    End Sub

    Private Sub ShowCompleteSendMessage(ByVal intPopupStatus As Integer)
        Dim strMsgCode As String = String.Empty

        Select Case intPopupStatus
            Case SPMaintenancePopupStatus.ResendActivationEmail
                strMsgCode = MsgCode.MSG00008
            Case SPMaintenancePopupStatus.ResendSchemeEnrolmentEmail
                strMsgCode = MsgCode.MSG00013
            Case SPMaintenancePopupStatus.ResendDelistEmail
                strMsgCode = MsgCode.MSG00009
            Case Else
                Return

        End Select

        CompleteMsgBox.AddMessage(FunctionCode, "I", strMsgCode)
        CompleteMsgBox.BuildMessageBox()
        CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Complete
    End Sub

#End Region

#Region "SP Buttons"

    ' Edit Return Info

    Protected Sub ibtnEditReturnInfo_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00049, "Edit Return Info starts")

        CompleteMsgBox.Visible = False

        Dim udtSchemeBackOfficeModelCollection As SchemeBackOfficeModelCollection = udtSchemeBackOfficeBLL.GetAllEffectiveSchemeBackOfficeWithSubsidizeGroupFromCache()
        gvInputDtm.DataSource = udtSchemeBackOfficeModelCollection
        gvInputDtm.DataBind()

        Dim dicSchemeRow As New Dictionary(Of Integer, String)
        Dim dicSchemeToReturnDate As New Dictionary(Of String, DateTime)
        Dim aryRowVisible As New ArrayList

        For i As Integer = 0 To udtSchemeBackOfficeModelCollection.Count - 1
            dicSchemeRow.Add(i, udtSchemeBackOfficeModelCollection(i).SchemeCode.Trim)
        Next

        If IsNothing(Session(SESS_SPDelisted)) Then
            txtInputTokenReturn.Text = String.Empty

        Else
            Dim dt As New DataTable
            dt = Session(SESS_SPDelisted)

            If dt.Rows.Count = 1 Then
                If IsDBNull(dt.Rows(0).Item("Logo_Return_Dtm")) OrElse IsNothing(dt.Rows(0).Item("Logo_Return_Dtm")) Then
                    'txtInputLogoReturnHCVS.Text = String.Empty
                Else
                    'txtInputLogoReturnHCVS.Text = Convert.ToDateTime(dt.Rows(0).Item("Logo_Return_Dtm")).ToString(Formatter.EnterDateFormat)
                End If

                If IsDBNull(dt.Rows(0).Item("Token_Return_Dtm")) OrElse IsNothing(dt.Rows(0).Item("Token_Return_Dtm")) Then
                    txtInputTokenReturn.Text = String.Empty
                Else
                    txtInputTokenReturn.Text = Convert.ToDateTime(dt.Rows(0).Item("Token_Return_Dtm")).ToString(udtFormatter.EnterDateFormat)
                End If
            End If
        End If

        ' Handle the Enable of the controls
        Dim udtSchemeInfoList As SchemeInformationModelCollection = CType(Session(SESS_SPSchemeInformation), SchemeInformationModelCollection)

        For Each udtSchemeInfo As SchemeInformationModel In udtSchemeInfoList.Values
            If udtSchemeInfo.RecordStatus = SchemeInformationMaintenanceDisplayStatus.DelistedVoluntary _
                            OrElse udtSchemeInfo.RecordStatus = SchemeInformationMaintenanceDisplayStatus.DelistedInvoluntary _
                            OrElse udtSchemeInfo.RecordStatus = SchemeInformationMaintenanceDisplayStatus.ActivePendingDelist _
                            OrElse udtSchemeInfo.RecordStatus = SchemeInformationMaintenanceDisplayStatus.SuspendedPendingDelist Then
                aryRowVisible.Add(udtSchemeInfo.SchemeCode)
                If udtSchemeInfo.LogoReturnDtm.HasValue Then dicSchemeToReturnDate.Add(udtSchemeInfo.SchemeCode.Trim, udtSchemeInfo.LogoReturnDtm)
            End If
        Next

        If udtSchemeInfoList.Values.Count = aryRowVisible.Count Then
            txtInputTokenReturn.Enabled = True
            ibtnInputTokenReturn.Enabled = True
        Else
            txtInputTokenReturn.Enabled = False
            ibtnInputTokenReturn.Enabled = False
        End If

        For i As Integer = 0 To gvInputDtm.Rows.Count - 1
            If Not aryRowVisible.Contains(dicSchemeRow(i)) Then
                gvInputDtm.Rows(i).Visible = False
            Else
                ' Pre-input the Logo Return Date (if any)
                If dicSchemeToReturnDate.ContainsKey(dicSchemeRow(i)) Then CType(gvInputDtm.Rows(i).FindControl("txtInputLogoReturn"), TextBox).Text = dicSchemeToReturnDate(dicSchemeRow(i)).ToString("dd-MM-yyyy")

                ' Handle schemes having no logos
                ' CRE16-022 (Add optional field "Remarks") [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                'If Not udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup().Filter(CType(gvInputDtm.Rows(i).FindControl("hfSchemeCodeReal"), HiddenField).Value.Trim).ReturnLogoEnabled Then gvInputDtm.Rows(i).Visible = False
                If Not udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup().Filter(CType(gvInputDtm.Rows(i).FindControl("lblSchemeCode"), Label).Attributes("SchemeCodeLogoReturn").Trim).ReturnLogoEnabled Then gvInputDtm.Rows(i).Visible = False
                ' CRE16-022 (Add optional field "Remarks") [End][Chris YIM]
            End If
        Next

        ' Pre-input the Token Return Date (if any)
        If lblTokenReturn.Visible Then txtInputTokenReturn.Text = DateTime.ParseExact(lblTokenReturn.Text, udtFormatter.DisplayDateFormat, Nothing).ToString("dd-MM-yyyy")

        panInputDtm.Visible = True
        panActionBtn.Visible = False

    End Sub

    Protected Sub ibtnEditReturnInfoSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CompleteMsgBox.Visible = False
        msgBox.Visible = False

        imgAlertInputTokenReturn.Visible = False

        Dim udtSchemeList As SchemeInformationModelCollection = CType(Session(SESS_SPSchemeInformation), SchemeInformationModelCollection)

        Dim aryLogoReturnChanged As New ArrayList

        ' Create a SchemeBackOfficeModelCollection (to avoid over-accessing SQL or cache)
        Dim udtSchemeBOList As SchemeBackOfficeModelCollection = udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup()

        For Each r As GridViewRow In gvInputDtm.Rows
            If r.Visible = False Then Continue For

            Dim imgAlertInputLogoReturn As Image = CType(r.FindControl("imgAlertInputLogoReturn"), Image)
            Dim txtInputLogoReturn As TextBox = CType(r.FindControl("txtInputLogoReturn"), TextBox)

            imgAlertInputLogoReturn.Visible = False

            SM = udtValidator.chkOptionalInputDate("010201", txtInputLogoReturn.Text.Trim, "00005", "00006")
            If Not IsNothing(SM) Then
                imgAlertInputLogoReturn.Visible = True
                msgBox.AddMessage(SM)
            End If

            ' CRE16-022 (Add optional field "Remarks") [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            'Dim strSchemeCodeReal As String = CType(r.FindControl("hfSchemeCodeReal"), HiddenField).Value.Trim
            Dim strSchemeCodeReal As String = CType(r.FindControl("lblSchemeCode"), Label).Attributes("SchemeCodeLogoReturn").Trim
            ' CRE16-022 (Add optional field "Remarks") [End][Chris YIM]

            Dim intDisplaySeq As Integer = udtSchemeBOList.Filter(strSchemeCodeReal).DisplaySeq

            If Not udtSchemeList(strSchemeCodeReal, intDisplaySeq).LogoReturnDtm.HasValue _
                    AndAlso txtInputLogoReturn.Text.Trim <> String.Empty Then aryLogoReturnChanged.Add(strSchemeCodeReal)

            If Not aryLogoReturnChanged.Contains(strSchemeCodeReal) _
                    AndAlso Not udtValidator.IsEmpty(txtInputLogoReturn.Text.Trim) _
                    AndAlso udtSchemeList(strSchemeCodeReal, intDisplaySeq).LogoReturnDtm <> DateTime.ParseExact(txtInputLogoReturn.Text.Trim, udtFormatter.EnterDateFormat, Nothing) Then aryLogoReturnChanged.Add(strSchemeCodeReal)
        Next

        SM = udtValidator.chkOptionalInputDate("010201", txtInputTokenReturn.Text.Trim, "00003", "00004")
        If Not IsNothing(SM) Then
            imgAlertInputTokenReturn.Visible = True
            msgBox.AddMessage(SM)
        End If

        Dim blnTokenReturnChanged As Boolean = False
        If txtInputTokenReturn.Text.Trim <> String.Empty _
            AndAlso lblTokenReturn.Text.Trim <> udtFormatter.convertDate(txtInputTokenReturn.Text.Trim, String.Empty) Then blnTokenReturnChanged = True

        ' No fields have been changed
        If aryLogoReturnChanged.Count = 0 AndAlso Not blnTokenReturnChanged Then
            msgBox.AddMessage(New SystemMessage("010201", SeverityCode.SEVE, "00015"))
        End If

        If msgBox.GetCodeTable.Rows.Count = 0 Then
            BindSPActionDetails()

            ' Construct the Edit Return Info table
            Dim dsReturn As New DataSet
            Dim dtReturn As DataTable
            Dim drReturn As DataRow

            dsReturn.Tables.Add(New DataTable("TempTable"))
            dtReturn = dsReturn.Tables.Item("TempTable")
            dtReturn.Columns.Add(Me.GetGlobalResourceObject("Text", "SchemeName"))
            dtReturn.Columns.Add(Me.GetGlobalResourceObject("Text", "LogoReturnDate"))
            dtReturn.Columns.Add("SchemeNameReal")

            Const intSchemeCodeColumn As Integer = 0
            Const intLogoReturnDateColumn As Integer = 1
            Const intSchemeCodeRealColumn As Integer = 2

            If aryLogoReturnChanged.Count <> 0 Then
                For Each r As GridViewRow In gvInputDtm.Rows
                    ' CRE16-022 (Add optional field "Remarks") [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                    If r.Visible Then
                        Dim lblSchemeCode As Label = CType(r.FindControl("lblSchemeCode"), Label)
                        Dim txtInputLogoReturn As TextBox = CType(r.FindControl("txtInputLogoReturn"), TextBox)

                        'Dim hfSchemeCodeReal As HiddenField = CType(r.FindControl("hfSchemeCodeReal"), HiddenField)
                        Dim strSchemeCodeReal As String = CType(r.FindControl("lblSchemeCode"), Label).Attributes("SchemeCodeLogoReturn")

                        ' Filter out the unchanged Logo Return Date
                        'If Not aryLogoReturnChanged.Contains(hfSchemeCodeReal.Value.Trim) Then Continue For
                        If Not aryLogoReturnChanged.Contains(strSchemeCodeReal.Trim) Then Continue For

                        drReturn = dsReturn.Tables.Item("TempTable").NewRow()
                        drReturn.Item(intSchemeCodeColumn) = lblSchemeCode.Text

                        If udtValidator.IsEmpty(txtInputLogoReturn.Text.Trim) Then
                            drReturn.Item(intLogoReturnDateColumn) = Me.GetGlobalResourceObject("Text", "N/A")
                        Else
                            drReturn.Item(intLogoReturnDateColumn) = udtFormatter.convertDate(txtInputLogoReturn.Text.Trim, String.Empty)
                        End If

                        'drReturn.Item(intSchemeCodeRealColumn) = hfSchemeCodeReal.Value.Trim
                        drReturn.Item(intSchemeCodeRealColumn) = strSchemeCodeReal.Trim
                        dsReturn.Tables.Item("TempTable").Rows.Add(drReturn)
                    End If
                    ' CRE16-022 (Add optional field "Remarks") [End][Chris YIM]
                Next

                gvActionInputLogoReturn.DataSource = dsReturn
                gvActionInputLogoReturn.DataBind()

                lblActionInputLogoReturnText.Visible = True
                gvActionInputLogoReturn.Visible = True

                ' Hide the SchemeRealName column
                gvActionInputLogoReturn.HeaderRow.Cells(intSchemeCodeRealColumn).Visible = False
                For Each r As GridViewRow In gvActionInputLogoReturn.Rows
                    r.Cells(intSchemeCodeRealColumn).Visible = False
                Next

                ' Control the width of the columns
                gvActionInputLogoReturn.Rows(0).Cells(intSchemeCodeColumn).Width = 200
                gvActionInputLogoReturn.Rows(0).Cells(intLogoReturnDateColumn).Width = 150

            Else
                lblActionInputLogoReturnText.Visible = False
                gvActionInputLogoReturn.Visible = False
            End If

            If blnTokenReturnChanged Then
                If udtValidator.IsEmpty(txtInputTokenReturn.Text.Trim) Then
                    lblActionInputTokenReturn.Text = Me.GetGlobalResourceObject("Text", "N/A")
                Else
                    lblActionInputTokenReturn.Text = udtFormatter.convertDate(txtInputTokenReturn.Text.Trim, String.Empty)
                End If

                lblActionInputTokenReturnText.Visible = True
                lblActionInputTokenReturn.Visible = True

            Else
                lblActionInputTokenReturnText.Visible = False
                lblActionInputTokenReturn.Visible = False
            End If

            CompleteMsgBox.AddMessage("010201", "I", "00005")
            CompleteMsgBox.BuildMessageBox()
            CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information

            panInputDtm.Visible = False
            panActionInputDtm.Visible = True

            MultiViewMaintenance.ActiveViewIndex = ViewIndexSPActionDetails

        Else
            msgBox.BuildMessageBox("ValidationFail")

        End If

    End Sub

    Protected Sub ibtnEditReturnInfoCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        msgBox.Visible = False
        CompleteMsgBox.Visible = False

        panInputDtm.Visible = False
        panActionBtn.Visible = True
    End Sub

    Protected Sub ibtnActionInputDtmConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CompleteMsgBox.Visible = False

        Dim udtDB As Database = New Database
        Dim strUserID As String = udtHCVUUserBLL.GetHCVUUser.UserID.Trim

        Dim udtAuditLogEntryCollection(gvActionInputLogoReturn.Rows.Count) As AuditLogEntry
        Dim intLogCount As Integer = 0

        udtAuditLogEntry = Nothing

        Try
            If udtServiceProviderBLL.Exist Then
                Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP

                Dim blnSuccess As Boolean = True

                udtDB.BeginTransaction()

                ' Update Logo return date
                If gvActionInputLogoReturn.Visible Then
                    For Each r As GridViewRow In gvActionInputLogoReturn.Rows
                        Dim dtmLogoReturn As Nullable(Of DateTime)
                        Dim strSchemeNameReal As String = r.Cells(2).Text.Trim

                        If r.Cells(1).Text.Trim.Equals(Me.GetGlobalResourceObject("Text", "N/A")) Then
                            Continue For
                        Else
                            dtmLogoReturn = DateTime.ParseExact(r.Cells(1).Text.Trim, udtFormatter.DisplayDateFormat, Nothing)
                        End If

                        ' Write Audit Log
                        udtAuditLogEntryCollection(intLogCount) = New AuditLogEntry(FunctionCode, Me)
                        udtAuditLogEntryCollection(intLogCount).AddDescripton("ERN", udtSP.EnrolRefNo)
                        udtAuditLogEntryCollection(intLogCount).AddDescripton("SPID", udtSP.SPID)
                        udtAuditLogEntryCollection(intLogCount).AddDescripton("Scheme", strSchemeNameReal)
                        udtAuditLogEntryCollection(intLogCount).AddDescripton("LogoReturnDate", r.Cells(1).Text.Trim)
                        udtAuditLogEntryCollection(intLogCount).WriteStartLog(LogID.LOG00016, "Edit Return Info confirms")
                        intLogCount += 1

                        Dim byteTargetTSMP As Byte() = Nothing

                        For Each udtScheme As SchemeInformationModel In udtSP.SchemeInfoList.Values
                            If udtScheme.SchemeCode = strSchemeNameReal Then
                                byteTargetTSMP = udtScheme.TSMP
                                Exit For
                            End If
                        Next

                        If Not udtAccountChangeMaintenanceBLL.UpdateLogoReturnDate(udtSP.SPID, strSchemeNameReal, dtmLogoReturn, strUserID, byteTargetTSMP, udtDB) Then
                            blnSuccess = False
                            Exit For
                        End If

                    Next
                End If

                ' Update Token Return date
                If blnSuccess AndAlso lblActionInputTokenReturn.Visible Then
                    udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
                    udtAuditLogEntry.AddDescripton("ERN", udtSP.EnrolRefNo)
                    udtAuditLogEntry.AddDescripton("SPID", udtSP.SPID)
                    udtAuditLogEntry.AddDescripton("TokenReturnDate", lblActionInputTokenReturn.Text.Trim)
                    udtAuditLogEntry.WriteStartLog(LogID.LOG00063, "Edit Return Info - Update Token Return Date")

                    Dim dtmTokenReturn As Nullable(Of DateTime)

                    dtmTokenReturn = DateTime.ParseExact(lblActionInputTokenReturn.Text.Trim, udtFormatter.DisplayDateFormat, Nothing)
                    blnSuccess = udtAccountChangeMaintenanceBLL.UpdateTokenReturnDate(udtSP.SPID, dtmTokenReturn, strUserID, udtSP.TSMP, udtDB)

                End If

                If blnSuccess Then
                    udtDB.CommitTransaction()

                    For i As Integer = 0 To intLogCount - 1
                        udtAuditLogEntryCollection(i).WriteEndLog(LogID.LOG00017, "Edit Return Info successful")
                    Next
                    If Not IsNothing(udtAuditLogEntry) Then udtAuditLogEntry.WriteEndLog(LogID.LOG00064, "Edit Return Info - Update Token Return Date successful")

                    Dim strOld As String() = {"%s"}
                    Dim strNew As String() = {""}

                    strNew(0) = udtSP.EnglishName + udtFormatter.formatChineseName(udtSP.ChineseName) + " [" + udtSP.SPID + "] "

                    CompleteMsgBox.AddMessage(FunctionCode, "I", MsgCode.MSG00011, strOld, strNew)
                    CompleteMsgBox.BuildMessageBox()
                    CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Complete

                    MultiViewMaintenance.ActiveViewIndex = ViewIndexComplete

                    panActionInputDtm.Visible = False

                Else
                    udtDB.RollBackTranscation()

                    For i As Integer = 0 To intLogCount - 1
                        udtAuditLogEntryCollection(i).WriteEndLog(LogID.LOG00018, "Edit Return Info failed")
                    Next
                    If Not IsNothing(udtAuditLogEntry) Then udtAuditLogEntry.WriteEndLog(LogID.LOG00065, "Edit Return Info - Update Token Return Date failed")

                    msgBox.AddMessage(New SystemMessage("990001", "D", "Edit Return Info failed"))
                    msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, Common.Component.LogID.LOG00018, "Edit Return Info failed")

                End If
            End If

        Catch eSQL As SqlClient.SqlException
            udtDB.RollBackTranscation()

            For i As Integer = 0 To intLogCount - 1
                udtAuditLogEntryCollection(i).WriteEndLog(LogID.LOG00018, "Edit Return Info failed")
            Next
            If Not IsNothing(udtAuditLogEntry) Then udtAuditLogEntry.WriteEndLog(LogID.LOG00065, "Edit Return Info - Update Token Return Date failed")

            If eSQL.Number = 50000 Then
                msgBox.AddMessage(New SystemMessage("990001", "D", eSQL.Message))

                If msgBox.GetCodeTable.Rows.Count = 0 Then
                    msgBox.Visible = False
                Else
                    msgBox.BuildMessageBox("UpdateFail")
                End If

            Else
                Throw eSQL
            End If

        Catch ex As Exception
            For i As Integer = 0 To intLogCount - 1
                udtAuditLogEntryCollection(i).WriteEndLog(LogID.LOG00018, "Edit Return Info failed")
            Next
            If Not IsNothing(udtAuditLogEntry) Then udtAuditLogEntry.WriteEndLog(LogID.LOG00065, "Edit Return Info - Update Token Return Date failed")

            Throw ex

        End Try

    End Sub

    Protected Sub ibtnActionInputDtmBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CompleteMsgBox.Visible = False
        panActionInputDtm.Visible = False
        panInputDtm.Visible = True

        MultiViewMaintenance.ActiveViewIndex = ViewIndexDetails
    End Sub

    ' Reactivate

    Protected Sub ibtnReactivate_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00050, "Reactivate SP Scheme starts")

        CompleteMsgBox.Visible = False
        panActionBtn.Visible = False
        panReactivate.Visible = True

        Dim udtSchemeBackOfficeModelCollection As SchemeBackOfficeModelCollection = udtSchemeBackOfficeBLL.GetAllEffectiveSchemeBackOfficeWithSubsidizeGroupFromCache()
        gvReactivateScheme.DataSource = udtSchemeBackOfficeModelCollection
        gvReactivateScheme.DataBind()

        Dim dicSchemeRow As New Dictionary(Of Integer, String)
        Dim aryRowVisible As New ArrayList

        For i As Integer = 0 To udtSchemeBackOfficeModelCollection.Count - 1
            dicSchemeRow.Add(i, udtSchemeBackOfficeModelCollection(i).SchemeCode.Trim)
        Next

        ' Handle the Enable of the controls
        For Each udtSchemeInfo As SchemeInformationModel In CType(Session(SESS_SPSchemeInformation), SchemeInformationModelCollection).Values
            If udtSchemeInfo.RecordStatus = SchemeInformationMaintenanceDisplayStatus.Suspended Then
                aryRowVisible.Add(udtSchemeInfo.SchemeCode)
            End If
        Next

        Dim intRowNoVisible As Integer

        For i As Integer = 0 To gvReactivateScheme.Rows.Count - 1
            If aryRowVisible.Contains(dicSchemeRow(i)) Then
                intRowNoVisible = i
            Else
                gvReactivateScheme.Rows(i).Visible = False
            End If
        Next

        ' Auto-check the checkbox if there is only one
        If aryRowVisible.Count = 1 Then
            Dim r As GridViewRow = gvReactivateScheme.Rows(intRowNoVisible)

            Dim cboSchemeCode As CheckBox = CType(r.FindControl("cboSchemeCode"), CheckBox)
            cboSchemeCode.Checked = True
            cboSchemeCode.Attributes.Add("onclick", "return false;")
        End If

    End Sub

    Protected Sub ibtnReactivateSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CompleteMsgBox.Visible = False
        msgBox.Visible = False

        Dim intChecked As Integer = 0

        For Each r As GridViewRow In gvReactivateScheme.Rows
            CType(r.FindControl("imgAlertSchemeCode"), Image).Visible = False

            If CType(r.FindControl("cboSchemeCode"), CheckBox).Checked Then
                intChecked += 1
            End If
        Next

        If intChecked = 0 Then
            msgBox.AddMessage(New SystemMessage(FunctionCode, "E", MsgCode.MSG00013))

            For Each r As GridViewRow In gvReactivateScheme.Rows
                CType(r.FindControl("imgAlertSchemeCode"), Image).Visible = True
            Next
        End If

        If msgBox.GetCodeTable.Rows.Count = 0 Then
            msgBox.Visible = False
            BindSPActionDetails()

            ' Build the Reactivate Scheme strings (front-end and back-end)
            lblActionReactivateScheme.Text = String.Empty
            hfActionReactivateScheme.Value = String.Empty

            ' CRE16-022 (Add optional field "Remarks") [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            For Each r As GridViewRow In gvReactivateScheme.Rows
                Dim cboSchemeCode As CheckBox = CType(r.FindControl("cboSchemeCode"), CheckBox)
                'Dim hfSchemeCodeReal As HiddenField = CType(r.FindControl("hfSchemeCodeReal"), HiddenField)
                Dim strSchemeCodeReal As String = cboSchemeCode.Attributes("SchemeCodeReactive")

                If cboSchemeCode.Checked Then
                    lblActionReactivateScheme.Text += ", " + cboSchemeCode.Text
                    'hfActionReactivateScheme.Value += ", " + AntiXssEncoder.HtmlEncode(hfSchemeCodeReal.Value, True)
                    hfActionReactivateScheme.Value += ", " + AntiXssEncoder.HtmlEncode(strSchemeCodeReal, True)
                End If
            Next
            ' CRE16-022 (Add optional field "Remarks") [End][Chris YIM]


            If lblActionReactivateScheme.Text.Length > 2 Then
                lblActionReactivateScheme.Text = lblActionReactivateScheme.Text.Substring(2)
                ' I-CRE16-003 Fix XSS [Start][Lawrence]
                hfActionReactivateScheme.Value = AntiXssEncoder.HtmlEncode(hfActionReactivateScheme.Value.Substring(2), True)
                ' I-CRE16-003 Fix XSS [End][Lawrence]
            End If

            CompleteMsgBox.AddMessage(FunctionCode, "I", MsgCode.MSG00004)
            CompleteMsgBox.BuildMessageBox()
            CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information

            panReactivate.Visible = False
            panActionReactivate.Visible = True
            MultiViewMaintenance.ActiveViewIndex = ViewIndexSPActionDetails

        Else
            msgBox.BuildMessageBox("ValidationFail")

        End If

    End Sub

    Protected Sub ibtnReactivateCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        msgBox.Visible = False
        CompleteMsgBox.Visible = False
        panReactivate.Visible = False
        panActionBtn.Visible = True

    End Sub

    Protected Sub ibtnActionReactivateConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CompleteMsgBox.Visible = False

        Dim udtDB As Database = New Database
        Dim aryActionReactivateScheme() As String = hfActionReactivateScheme.Value.Split(",")
        Dim udtAuditLogEntryCollection(aryActionReactivateScheme.Length) As AuditLogEntry
        Dim intLogCount As Integer = 0

        Try
            If udtServiceProviderBLL.Exist Then
                Dim udtHCVUUser As HCVUUserModel = udtHCVUUserBLL.GetHCVUUser
                Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP

                Dim blnSuccess As Boolean = True
                udtDB.BeginTransaction()

                ' Tokenize the string to array
                For Each strReactivateScheme As String In aryActionReactivateScheme
                    strReactivateScheme = strReactivateScheme.Trim

                    ' Write Audit Log
                    udtAuditLogEntryCollection(intLogCount) = New AuditLogEntry(FunctionCode, Me)
                    udtAuditLogEntryCollection(intLogCount).AddDescripton("ERN", udtSP.EnrolRefNo)
                    udtAuditLogEntryCollection(intLogCount).AddDescripton("SPID", udtSP.SPID)
                    udtAuditLogEntryCollection(intLogCount).AddDescripton("Scheme", strReactivateScheme)
                    udtAuditLogEntryCollection(intLogCount).WriteStartLog(LogID.LOG00013, "Reactivate SP Scheme confirms")
                    intLogCount += 1

                    ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                    ' -------------------------------------------------------------------------
                    'Dim udtAcctChangeMaintenanceModel As AccountChangeMaintenanceModel = New AccountChangeMaintenanceModel(udtSP.SPID, _
                    '                                        SPAccountMaintenanceUpdTypeStatus.SPReactivate, Nothing, String.Empty, String.Empty, _
                    '                                        String.Empty, 0, String.Empty, udtHCVUUser.UserID.Trim, String.Empty, Nothing, _
                    '                                        SPAccountMaintenanceRecordStatus.Active, strReactivateScheme, Nothing)

                    'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    'Dim udtAcctChangeMaintenanceModel As AccountChangeMaintenanceModel = New AccountChangeMaintenanceModel(udtSP.SPID, _
                    '                    SPAccountMaintenanceUpdTypeStatus.SPReactivate, Nothing, String.Empty, String.Empty, _
                    '                    String.Empty, 0, String.Empty, udtHCVUUser.UserID.Trim, String.Empty, Nothing, _
                    '                    SPAccountMaintenanceRecordStatus.Active, strReactivateScheme, Nothing, udtHCVUUser.UserID.Trim)

                    Dim udtAcctChangeMaintenanceModel As AccountChangeMaintenanceModel = New AccountChangeMaintenanceModel(udtSP.SPID, _
                                        SPAccountMaintenanceUpdTypeStatus.SPReactivate, Nothing, String.Empty, String.Empty, _
                                        String.Empty, 0, String.Empty, udtHCVUUser.UserID.Trim, String.Empty, Nothing, _
                                        SPAccountMaintenanceRecordStatus.Active, strReactivateScheme, Nothing, udtHCVUUser.UserID.Trim, Nothing, Nothing, Nothing, Nothing, Nothing)

                    'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
                    ' INT13-0028 - SP Amendment Report [End][Tommy L]

                    If Not udtAccountChangeMaintenanceBLL.AddRecord(udtAcctChangeMaintenanceModel, udtDB) Then blnSuccess = False

                Next

                If blnSuccess Then
                    udtDB.CommitTransaction()
                    For i As Integer = 0 To intLogCount - 1
                        udtAuditLogEntryCollection(i).WriteEndLog(LogID.LOG00014, "Reactivate SP Scheme successful")
                    Next

                    Dim strOld As String() = {"%s"}
                    Dim strNew As String() = {""}

                    strNew(0) = udtSP.EnglishName + udtFormatter.formatChineseName(udtSP.ChineseName) + " [" + udtSP.SPID + "] "

                    CompleteMsgBox.AddMessage(FunctionCode, "I", MsgCode.MSG00001, strOld, strNew)
                    CompleteMsgBox.BuildMessageBox()
                    CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Complete
                    MultiViewMaintenance.ActiveViewIndex = ViewIndexComplete

                    panActionReactivate.Visible = False

                Else
                    udtDB.RollBackTranscation()
                    For i As Integer = 0 To intLogCount - 1
                        udtAuditLogEntryCollection(i).WriteEndLog(LogID.LOG00015, "Reactivate SP Scheme failed")
                    Next

                End If
            End If

        Catch eSQL As SqlClient.SqlException
            udtDB.RollBackTranscation()
            For i As Integer = 0 To intLogCount - 1
                udtAuditLogEntryCollection(i).WriteEndLog(LogID.LOG00015, "Reactivate SP Scheme failed")
            Next

            If eSQL.Number = 50000 Then
                msgBox.AddMessage(New SystemMessage("990001", "D", eSQL.Message))

                If msgBox.GetCodeTable.Rows.Count = 0 Then
                    msgBox.Visible = False
                Else
                    msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, Common.Component.LogID.LOG00015, "Reactivate SP Scheme failed")
                End If

            Else
                Throw eSQL
            End If

        Catch ex As Exception
            udtDB.RollBackTranscation()
            For i As Integer = 0 To intLogCount - 1
                udtAuditLogEntryCollection(i).WriteEndLog(LogID.LOG00015, "Reactivate Service Provider Scheme failed")
            Next

            Throw ex

        End Try
    End Sub

    Protected Sub ibtnActionReactivateBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CompleteMsgBox.Visible = False
        panActionReactivate.Visible = False
        panReactivate.Visible = True
        MultiViewMaintenance.ActiveViewIndex = ViewIndexDetails
    End Sub

    ' Delisting

    Protected Sub ibtnDelisting_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00051, "Delist SP Scheme starts")

        CompleteMsgBox.Visible = False
        panDelisting.Visible = True
        panActionBtn.Visible = False

        Dim udtSchemeBackOfficeModelCollection As SchemeBackOfficeModelCollection = udtSchemeBackOfficeBLL.GetAllEffectiveSchemeBackOfficeWithSubsidizeGroupFromCache()
        gvDelistScheme.DataSource = udtSchemeBackOfficeModelCollection
        gvDelistScheme.DataBind()

        Dim dicSchemeRow As New Dictionary(Of Integer, String)
        Dim aryRowVisible As New ArrayList

        For i As Integer = 0 To udtSchemeBackOfficeModelCollection.Count - 1
            dicSchemeRow.Add(i, udtSchemeBackOfficeModelCollection(i).SchemeCode.Trim)
        Next

        ' Reset controls content
        txtTokenReturn.Text = String.Empty
        txtTokenReturn.Enabled = False
        ibtnTokenReturn.Enabled = False
        imgAlertTokenReturn.Visible = False

        ' Handle the Enable of the controls
        For Each udtSchemeInfo As SchemeInformationModel In CType(Session(SESS_SPSchemeInformation), SchemeInformationModelCollection).Values
            If udtSchemeInfo.RecordStatus = SchemeInformationMaintenanceDisplayStatus.Active _
                    OrElse udtSchemeInfo.RecordStatus = SchemeInformationMaintenanceDisplayStatus.Suspended Then
                aryRowVisible.Add(udtSchemeInfo.SchemeCode)
            End If
        Next

        Dim intRowNoVisible As Integer

        For i As Integer = 0 To gvDelistScheme.Rows.Count - 1
            If aryRowVisible.Contains(dicSchemeRow(i)) Then
                intRowNoVisible = i

                ' Specially handle for schemes having no Logo

                ' CRE16-022 (Add optional field "Remarks") [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                'If Not udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup().Filter(CType(gvDelistScheme.Rows(i).FindControl("hfSchemeCodeReal"), HiddenField).Value.Trim).ReturnLogoEnabled Then
                If Not udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup().Filter(CType(gvDelistScheme.Rows(i).FindControl("cboSchemeCode"), CheckBox).Attributes("SchemeCodeDelist").Trim).ReturnLogoEnabled Then
                    Dim r As GridViewRow = gvDelistScheme.Rows(i)
                    CType(r.FindControl("txtLogoReturn"), TextBox).Visible = False
                    CType(r.FindControl("ibtnLogoReturn"), ImageButton).Visible = False
                End If
                ' CRE16-022 (Add optional field "Remarks") [End][Chris YIM]

            Else
                gvDelistScheme.Rows(i).Visible = False
            End If
        Next

        ' Auto-check the checkbox if there is only one
        If aryRowVisible.Count = 1 Then
            Dim r As GridViewRow = gvDelistScheme.Rows(intRowNoVisible)

            Dim cboSchemeCode As CheckBox = CType(r.FindControl("cboSchemeCode"), CheckBox)
            cboSchemeCode.Checked = True
            cboSchemeCode.Attributes.Add("onclick", "return false;")

            CType(r.FindControl("rbDelistType"), RadioButtonList).Enabled = True
            CType(r.FindControl("txtRemark"), TextBox).Enabled = True
            CType(r.FindControl("txtLogoReturn"), TextBox).Enabled = True
            CType(r.FindControl("ibtnLogoReturn"), ImageButton).Enabled = True

            txtTokenReturn.Enabled = True
            ibtnTokenReturn.Enabled = True

        End If

    End Sub

    Protected Sub ibtnDelistingSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CompleteMsgBox.Visible = False
        msgBox.Visible = False

        Dim intChecked As Integer = 0

        For Each r As GridViewRow In gvDelistScheme.Rows
            Dim cboSchemeCode As CheckBox = CType(r.FindControl("cboSchemeCode"), CheckBox)
            Dim rbDelistType As RadioButtonList = CType(r.FindControl("rbDelistType"), RadioButtonList)
            Dim txtRemark As TextBox = CType(r.FindControl("txtRemark"), TextBox)
            Dim txtLogoReturn As TextBox = CType(r.FindControl("txtLogoReturn"), TextBox)

            Dim imgAlertSchemeCode As Image = CType(r.FindControl("imgAlertSchemeCode"), Image)
            Dim imgAlertDelistType As Image = CType(r.FindControl("imgAlertDelistType"), Image)
            Dim imgAlertRemark As Image = CType(r.FindControl("imgAlertRemark"), Image)
            Dim imgAlertLogoReturn As Image = CType(r.FindControl("imgAlertLogoReturn"), Image)

            imgAlertSchemeCode.Visible = False
            imgAlertDelistType.Visible = False
            imgAlertRemark.Visible = False
            imgAlertLogoReturn.Visible = False

            If cboSchemeCode.Checked Then
                intChecked += 1

                ' Check "Voluntary" or "Involuntary" selected
                If udtValidator.IsEmpty(rbDelistType.SelectedValue) Then
                    imgAlertDelistType.Visible = True
                    msgBox.AddMessage(New SystemMessage("010201", "E", "00001"))
                End If

                ' Check Remarks input
                If udtValidator.IsEmpty(txtRemark.Text.Trim) Then
                    imgAlertRemark.Visible = True
                    msgBox.AddMessage(New SystemMessage("010201", "E", "00002"))
                End If

                ' Check valid Logo Return Date
                SM = udtValidator.chkOptionalInputDate("010201", txtLogoReturn.Text.Trim, "00005", "00006")
                If Not IsNothing(SM) Then
                    imgAlertLogoReturn.Visible = True
                    msgBox.AddMessage(SM)
                End If
            End If
        Next

        ' Check at least one checkbox checked
        If intChecked = 0 Then
            msgBox.AddMessage(New SystemMessage(FunctionCode, "E", MsgCode.MSG00011))

            For Each r As GridViewRow In gvDelistScheme.Rows
                CType(r.FindControl("imgAlertSchemeCode"), Image).Visible = True
            Next
        End If

        ' Check valid Token Return Date
        SM = udtValidator.chkOptionalInputDate("010201", txtTokenReturn.Text.Trim, "00003", "00004")
        If Not IsNothing(SM) Then
            imgAlertTokenReturn.Visible = True
            msgBox.AddMessage(SM)
        End If

        If msgBox.GetCodeTable.Rows.Count = 0 Then
            msgBox.Visible = False
            BindSPActionDetails()
            panActionDelisting.Visible = True

            ' Construct the Delist Scheme table
            Dim dsDelist As New DataSet
            Dim dtDelist As DataTable
            Dim drDelist As DataRow

            dsDelist.Tables.Add(New DataTable("TempTable"))
            dtDelist = dsDelist.Tables.Item("TempTable")
            dtDelist.Columns.Add(Me.GetGlobalResourceObject("Text", "SchemeName"))
            dtDelist.Columns.Add(Me.GetGlobalResourceObject("Text", "DelistingType"))
            dtDelist.Columns.Add(Me.GetGlobalResourceObject("Text", "Remarks"))
            dtDelist.Columns.Add(Me.GetGlobalResourceObject("Text", "LogoReturnDate"))
            dtDelist.Columns.Add("SchemeNameReal")
            dtDelist.Columns.Add("DelistingTypeReal")

            Const intSchemeCodeColumn As Integer = 0
            Const intDelistTypeColumn As Integer = 1
            Const intRemarkColumn As Integer = 2
            Const intLogoReturnColumn As Integer = 3
            Const intSchemeCodeRealColumn As Integer = 4
            Const intDelistTypeRealColumn As Integer = 5

            For Each r As GridViewRow In gvDelistScheme.Rows
                Dim cboSchemeCode As CheckBox = CType(r.FindControl("cboSchemeCode"), CheckBox)

                If cboSchemeCode.Checked Then
                    drDelist = dsDelist.Tables.Item("TempTable").NewRow()
                    drDelist.Item(intSchemeCodeColumn) = cboSchemeCode.Text

                    Dim rbDelistType As RadioButtonList = CType(r.FindControl("rbDelistType"), RadioButtonList)
                    drDelist.Item(intDelistTypeColumn) = rbDelistType.SelectedItem.Text

                    drDelist.Item(intRemarkColumn) = CType(r.FindControl("txtRemark"), TextBox).Text

                    Dim strLogoReturn As String = CType(r.FindControl("txtLogoReturn"), TextBox).Text.Trim
                    If udtValidator.IsEmpty(strLogoReturn) Then
                        drDelist.Item(intLogoReturnColumn) = Me.GetGlobalResourceObject("Text", "N/A")
                    Else
                        drDelist.Item(intLogoReturnColumn) = udtFormatter.convertDate(strLogoReturn, String.Empty)
                    End If

                    ' CRE16-022 (Add optional field "Remarks") [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                    'drDelist.Item(intSchemeCodeRealColumn) = CType(r.FindControl("hfSchemeCodeReal"), HiddenField).Value
                    drDelist.Item(intSchemeCodeRealColumn) = CType(r.FindControl("cboSchemeCode"), CheckBox).Attributes("SchemeCodeDelist")
                    ' CRE16-022 (Add optional field "Remarks") [End][Chris YIM]

                    drDelist.Item(intDelistTypeRealColumn) = rbDelistType.SelectedValue

                    dsDelist.Tables.Item("TempTable").Rows.Add(drDelist)
                End If
            Next

            gvActionDelistScheme.DataSource = dsDelist
            gvActionDelistScheme.DataBind()

            ' Hide the SchemeNameReal and DelistingTypeReal columns
            gvActionDelistScheme.HeaderRow.Cells(intSchemeCodeRealColumn).Visible = False
            gvActionDelistScheme.HeaderRow.Cells(intDelistTypeRealColumn).Visible = False
            For Each r As GridViewRow In gvActionDelistScheme.Rows
                r.Cells(intSchemeCodeRealColumn).Visible = False
                r.Cells(intDelistTypeRealColumn).Visible = False
            Next

            ' Control the width of the "Remarks" column
            gvActionDelistScheme.Rows(0).Cells(intRemarkColumn).Width = 250

            If udtValidator.IsEmpty(txtTokenReturn.Text.Trim) Then
                lblActionTokenReturnDate.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                lblActionTokenReturnDate.Text = udtFormatter.convertDate(txtTokenReturn.Text.Trim, String.Empty)
            End If

            CompleteMsgBox.AddMessage("010201", "I", "00012")
            CompleteMsgBox.BuildMessageBox()
            CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information

            MultiViewMaintenance.ActiveViewIndex = ViewIndexSPActionDetails

            panDelisting.Visible = False
            panActionDelisting.Visible = True

        Else
            msgBox.BuildMessageBox("ValidationFail")
        End If

    End Sub

    Protected Sub ibtnDelistingCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        msgBox.Visible = False
        CompleteMsgBox.Visible = False
        panInputDtm.Visible = False
        panSuspend.Visible = False
        panDelisting.Visible = False
        panActionBtn.Visible = True
    End Sub

    Protected Sub ibtnActionDelistingConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CompleteMsgBox.Visible = False

        Dim udtDB As Database = New Database
        Dim udtAuditLogEntryCollection(gvActionDelistScheme.Rows.Count) As AuditLogEntry
        Dim intLogCount As Integer = 0

        Try
            If udtServiceProviderBLL.Exist Then
                Dim udtHCVUUser As HCVUUserModel = udtHCVUUserBLL.GetHCVUUser
                Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP

                udtAuditLogEntry = Nothing

                udtDB.BeginTransaction()

                Dim blnSuccess As Boolean = True

                For Each r As GridViewRow In gvActionDelistScheme.Rows
                    Dim strSchemeName As String = r.Cells(0).Text.Trim
                    Dim strSchemeNameReal As String = r.Cells(4).Text.Trim
                    Dim strDelistTypeReal As String = r.Cells(5).Text.Trim
                    Dim strRemark As String = r.Cells(2).Text.Trim
                    Dim strLogoReturnDtm As String = r.Cells(3).Text.Trim

                    ' Write Audit Log
                    udtAuditLogEntryCollection(intLogCount) = New AuditLogEntry(FunctionCode, Me)
                    udtAuditLogEntryCollection(intLogCount).AddDescripton("ERN", udtSP.EnrolRefNo)
                    udtAuditLogEntryCollection(intLogCount).AddDescripton("SPID", udtSP.SPID)
                    udtAuditLogEntryCollection(intLogCount).AddDescripton("Scheme", strSchemeNameReal)
                    udtAuditLogEntryCollection(intLogCount).AddDescripton("DelistingType", strDelistTypeReal)
                    udtAuditLogEntryCollection(intLogCount).AddDescripton("Remark", strRemark)
                    udtAuditLogEntryCollection(intLogCount).AddDescripton("LogoReturnDate", strLogoReturnDtm)
                    udtAuditLogEntryCollection(intLogCount).WriteStartLog(LogID.LOG00007, "Delist SP Scheme confirms")
                    intLogCount += 1

                    ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                    ' -------------------------------------------------------------------------
                    'Dim udtAcctChangeMaintenanceModel As AccountChangeMaintenanceModel = New AccountChangeMaintenanceModel(udtSP.SPID, _
                    '                                                 SPAccountMaintenanceUpdTypeStatus.SPDelist, Nothing, strRemark, String.Empty, _
                    '                                                 String.Empty, 0, strDelistTypeReal, udtHCVUUser.UserID.Trim, String.Empty, _
                    '                                                 Nothing, SPAccountMaintenanceRecordStatus.Active, strSchemeNameReal, Nothing)

                    'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    'Dim udtAcctChangeMaintenanceModel As AccountChangeMaintenanceModel = New AccountChangeMaintenanceModel(udtSP.SPID, _
                    '                             SPAccountMaintenanceUpdTypeStatus.SPDelist, Nothing, strRemark, String.Empty, _
                    '                             String.Empty, 0, strDelistTypeReal, udtHCVUUser.UserID.Trim, String.Empty, _
                    '                             Nothing, SPAccountMaintenanceRecordStatus.Active, strSchemeNameReal, Nothing, udtHCVUUser.UserID.Trim)

                    Dim udtAcctChangeMaintenanceModel As AccountChangeMaintenanceModel = New AccountChangeMaintenanceModel(udtSP.SPID, _
                             SPAccountMaintenanceUpdTypeStatus.SPDelist, Nothing, strRemark, String.Empty, _
                             String.Empty, 0, strDelistTypeReal, udtHCVUUser.UserID.Trim, String.Empty, _
                             Nothing, SPAccountMaintenanceRecordStatus.Active, strSchemeNameReal, Nothing, udtHCVUUser.UserID.Trim, Nothing, Nothing, Nothing, Nothing, Nothing)

                    'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
                    ' INT13-0028 - SP Amendment Report [End][Tommy L]

                    If Not udtAccountChangeMaintenanceBLL.AddRecord(udtAcctChangeMaintenanceModel, udtDB) Then blnSuccess = False

                    ' Update the Logo Return Date
                    If blnSuccess AndAlso Not strLogoReturnDtm.Equals(Me.GetGlobalResourceObject("Text", "N/A")) Then
                        strLogoReturnDtm = DateTime.ParseExact(strLogoReturnDtm, udtFormatter.DisplayDateFormat, Nothing)

                        Dim byteTargetTSMP As Byte() = Nothing

                        For Each udtNode As SchemeInformationModel In udtSP.SchemeInfoList.Values
                            If udtNode.SchemeCode.Trim = strSchemeNameReal Then
                                byteTargetTSMP = udtNode.TSMP
                                Exit For
                            End If
                        Next

                        If Not udtAccountChangeMaintenanceBLL.UpdateLogoReturnDate(udtSP.SPID, strSchemeNameReal, strLogoReturnDtm, udtHCVUUser.UserID.Trim, byteTargetTSMP, udtDB) Then blnSuccess = False

                    End If

                Next

                ' Update the Token Return Date - Only run if all the schemes above are successfully delisted, otherwise the transaction will be rolled back
                If blnSuccess AndAlso Not lblActionTokenReturnDate.Text.Trim.Equals(GetGlobalResourceObject("Text", "N/A")) Then
                    ' Write Audit Log
                    udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
                    udtAuditLogEntry.AddDescripton("ERN", udtSP.EnrolRefNo)
                    udtAuditLogEntry.AddDescripton("SPID", udtSP.SPID)
                    udtAuditLogEntry.AddDescripton("TokenReturnDate", lblActionTokenReturnDate.Text.Trim)
                    udtAuditLogEntry.WriteStartLog(LogID.LOG00052, "Delist SP Scheme - Update Token Return Date")

                    Dim dtmTokenReturn As Nullable(Of DateTime) = DateTime.ParseExact(lblActionTokenReturnDate.Text.Trim, udtFormatter.DisplayDateFormat, Nothing)

                    If udtAccountChangeMaintenanceBLL.UpdateTokenReturnDate(udtSP.SPID, dtmTokenReturn, udtHCVUUser.UserID.Trim, udtSP.TSMP, udtDB) Then
                        ' Nothing here
                    Else
                        blnSuccess = False
                    End If

                End If

                If blnSuccess Then
                    udtDB.CommitTransaction()
                    For i As Integer = 0 To intLogCount - 1
                        udtAuditLogEntryCollection(i).WriteEndLog(LogID.LOG00008, "Delist SP Scheme successful")
                    Next
                    If Not IsNothing(udtAuditLogEntry) Then udtAuditLogEntry.WriteEndLog(LogID.LOG00053, "Delist SP Scheme - Update Token Return Date successful")

                    Dim strOld As String() = {"%s"}
                    Dim strNew As String() = {""}

                    strNew(0) = udtSP.EnglishName + udtFormatter.formatChineseName(udtSP.ChineseName) + " [" + udtSP.SPID + "] "

                    CompleteMsgBox.AddMessage(FunctionCode, "I", MsgCode.MSG00001, strOld, strNew)
                    CompleteMsgBox.BuildMessageBox()
                    CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Complete
                    MultiViewMaintenance.ActiveViewIndex = ViewIndexComplete

                    panActionDelisting.Visible = False

                Else
                    udtDB.RollBackTranscation()
                    For i As Integer = 0 To intLogCount - 1
                        udtAuditLogEntryCollection(i).WriteEndLog(LogID.LOG00009, "Delist SP Scheme failed")
                    Next
                    If Not IsNothing(udtAuditLogEntry) Then udtAuditLogEntry.WriteEndLog(LogID.LOG00066, "Delist SP Scheme - Update Token Return Date failed")

                End If

            End If
        Catch eSQL As SqlClient.SqlException
            udtDB.RollBackTranscation()
            For i As Integer = 0 To intLogCount - 1
                udtAuditLogEntryCollection(i).WriteEndLog(LogID.LOG00009, "Delist SP Scheme failed")
            Next
            If Not IsNothing(udtAuditLogEntry) Then udtAuditLogEntry.WriteEndLog(LogID.LOG00066, "Delist SP Scheme - Update Token Return Date failed")

            If eSQL.Number = 50000 Then
                msgBox.AddMessage(New SystemMessage("990001", "D", eSQL.Message))

                If msgBox.GetCodeTable.Rows.Count = 0 Then
                    msgBox.Visible = False
                Else
                    msgBox.BuildMessageBox("UpdateFail")
                    If Not IsNothing(udtAuditLogEntry) Then udtAuditLogEntry.WriteEndLog(LogID.LOG00066, "Delist SP Scheme - Update Token Return Date failed")
                End If

            Else
                Throw eSQL
            End If

        Catch ex As Exception
            udtDB.RollBackTranscation()
            For i As Integer = 0 To intLogCount - 1
                udtAuditLogEntryCollection(i).WriteEndLog(LogID.LOG00009, "Delist SP Scheme failed")
            Next
            If Not IsNothing(udtAuditLogEntry) Then udtAuditLogEntry.WriteEndLog(LogID.LOG00066, "Delist SP Scheme - Update Token Return Date failed")

            Throw ex
        End Try

    End Sub

    Protected Sub ibtnActionDelistingBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CompleteMsgBox.Visible = False
        'Session(SESS_SelectedPractice) = Nothing
        'Session.Remove(SESS_SelectedPractice)
        MultiViewMaintenance.ActiveViewIndex = ViewIndexDetails
        panActionDelisting.Visible = False
        panDelisting.Visible = True

    End Sub

    Protected Sub cboSchemeCode_DelistScheme_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim r As GridViewRow = CType(sender, CheckBox).NamingContainer
        Dim cboSchemeCode As CheckBox = CType(sender, CheckBox)

        CType(r.FindControl("rbDelistType"), RadioButtonList).Enabled = cboSchemeCode.Checked
        CType(r.FindControl("txtRemark"), TextBox).Enabled = cboSchemeCode.Checked

        Dim txtLogoReturn As TextBox = r.FindControl("txtLogoReturn")
        If Not IsNothing(txtLogoReturn) Then
            CType(txtLogoReturn, TextBox).Enabled = cboSchemeCode.Checked
            CType(r.FindControl("ibtnLogoReturn"), ImageButton).Enabled = cboSchemeCode.Checked
        End If

        Dim blnAllChecked As Boolean = True
        For Each row As GridViewRow In gvDelistScheme.Rows
            Dim cbo As CheckBox = row.FindControl("cboSchemeCode")

            If cbo.Visible = True And cbo.Checked = False Then
                blnAllChecked = False
                Exit For
            End If
        Next

        If blnAllChecked Then
            txtTokenReturn.Enabled = True
            ibtnTokenReturn.Enabled = True

        Else
            txtTokenReturn.Enabled = False
            ibtnTokenReturn.Enabled = False
            txtTokenReturn.Text = String.Empty

        End If

    End Sub

    ' Suspend

    Protected Sub ibtnSuspend_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00055, "Suspend SP Scheme starts")

        CompleteMsgBox.Visible = False
        panSuspend.Visible = True
        panActionBtn.Visible = False

        Dim udtSchemeBackOfficeModelCollection As SchemeBackOfficeModelCollection = udtSchemeBackOfficeBLL.GetAllEffectiveSchemeBackOfficeWithSubsidizeGroupFromCache()
        gvSuspendScheme.DataSource = udtSchemeBackOfficeModelCollection
        gvSuspendScheme.DataBind()

        Dim dicSchemeRow As New Dictionary(Of Integer, String)
        Dim aryRowVisible As New ArrayList

        For i As Integer = 0 To udtSchemeBackOfficeModelCollection.Count - 1
            dicSchemeRow.Add(i, udtSchemeBackOfficeModelCollection(i).SchemeCode.Trim)
        Next

        ' Handle the Enable of the controls
        For Each udtSchemeInfo As SchemeInformationModel In CType(Session(SESS_SPSchemeInformation), SchemeInformationModelCollection).Values
            If udtSchemeInfo.RecordStatus = SPAccountMaintenanceRecordStatus.Active Then
                aryRowVisible.Add(udtSchemeInfo.SchemeCode)
            End If
        Next

        Dim intRowNoVisible As Integer

        For i As Integer = 0 To gvSuspendScheme.Rows.Count - 1
            If aryRowVisible.Contains(dicSchemeRow(i)) Then
                intRowNoVisible = i
            Else
                gvSuspendScheme.Rows(i).Visible = False
            End If
        Next

        ' Auto-check the checkbox if there is only one
        If aryRowVisible.Count = 1 Then
            Dim r As GridViewRow = gvSuspendScheme.Rows(intRowNoVisible)

            Dim cboSchemeCode As CheckBox = CType(r.FindControl("cboSchemeCode"), CheckBox)
            cboSchemeCode.Checked = True
            cboSchemeCode.Attributes.Add("onclick", "return false;")

            Dim txtRemark As TextBox = CType(r.FindControl("txtRemark"), TextBox)
            txtRemark.Enabled = True
        End If

    End Sub

    Protected Sub ibtnSuspendSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CompleteMsgBox.Visible = False
        msgBox.Visible = False

        Dim intChecked As Integer = 0

        For Each r As GridViewRow In gvSuspendScheme.Rows
            Dim cboSchemeCode As CheckBox = CType(r.FindControl("cboSchemeCode"), CheckBox)
            Dim txtRemark As TextBox = CType(r.FindControl("txtRemark"), TextBox)
            Dim imgAlertSchemeCode As Image = CType(r.FindControl("imgAlertSchemeCode"), Image)
            Dim imgAlertRemark As Image = CType(r.FindControl("imgAlertRemark"), Image)

            imgAlertSchemeCode.Visible = False
            imgAlertRemark.Visible = False

            If cboSchemeCode.Checked Then
                intChecked += 1

                If udtValidator.IsEmpty(txtRemark.Text.Trim) Then
                    imgAlertRemark.Visible = True
                    msgBox.AddMessage(New SystemMessage(FunctionCode, "E", MsgCode.MSG00007))
                End If
            End If
        Next

        ' Check at least one checkbox checked
        If intChecked = 0 Then
            msgBox.AddMessage(New SystemMessage(FunctionCode, "E", MsgCode.MSG00012))

            For Each r As GridViewRow In gvSuspendScheme.Rows
                CType(r.FindControl("imgAlertSchemeCode"), Image).Visible = True
            Next
        End If

        If msgBox.GetCodeTable.Rows.Count = 0 Then
            msgBox.Visible = False
            BindSPActionDetails()
            panActionSuspend.Visible = True

            ' Construct the Suspend Scheme table
            Dim dsSuspend As New DataSet
            Dim dtSuspend As DataTable
            Dim drSuspend As DataRow

            dsSuspend.Tables.Add(New DataTable("TempTable"))
            dtSuspend = dsSuspend.Tables.Item("TempTable")
            dtSuspend.Columns.Add(Me.GetGlobalResourceObject("Text", "SchemeName"))
            dtSuspend.Columns.Add(Me.GetGlobalResourceObject("Text", "Remarks"))
            dtSuspend.Columns.Add("SchemeNameReal")

            Const intSchemeCodeColumn As Integer = 0
            Const intRemarkColumn As Integer = 1
            Const intSchemeCodeRealColumn As Integer = 2

            For Each r As GridViewRow In gvSuspendScheme.Rows
                Dim cboSchemeCode As CheckBox = CType(r.FindControl("cboSchemeCode"), CheckBox)

                If cboSchemeCode.Checked Then
                    drSuspend = dsSuspend.Tables.Item("TempTable").NewRow()
                    drSuspend.Item(intSchemeCodeColumn) = cboSchemeCode.Text
                    drSuspend.Item(intRemarkColumn) = CType(r.FindControl("txtRemark"), TextBox).Text
                    ' CRE16-022 (Add optional field "Remarks") [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                    'drSuspend.Item(intSchemeCodeRealColumn) = CType(r.FindControl("hfSchemeCodeReal"), HiddenField).Value
                    drSuspend.Item(intSchemeCodeRealColumn) = CType(r.FindControl("cboSchemeCode"), CheckBox).Attributes("SchemeCodeSuspend")
                    ' CRE16-022 (Add optional field "Remarks") [End][Chris YIM]

                    dsSuspend.Tables.Item("TempTable").Rows.Add(drSuspend)
                End If
            Next

            gvActionSuspendScheme.DataSource = dsSuspend
            gvActionSuspendScheme.DataBind()

            ' Hide the SchemeRealName column
            gvActionSuspendScheme.HeaderRow.Cells(intSchemeCodeRealColumn).Visible = False
            For Each r As GridViewRow In gvActionSuspendScheme.Rows
                r.Cells(intSchemeCodeRealColumn).Visible = False
            Next

            ' Control the width of the Remarks column
            gvActionSuspendScheme.Rows(0).Cells(intRemarkColumn).Width = 250

            CompleteMsgBox.AddMessage(FunctionCode, "I", MsgCode.MSG00002)
            CompleteMsgBox.BuildMessageBox()
            CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information

            MultiViewMaintenance.ActiveViewIndex = ViewIndexSPActionDetails

            panSuspend.Visible = False
            panActionSuspend.Visible = True

        Else
            msgBox.BuildMessageBox("ValidationFail")

        End If

    End Sub

    Protected Sub ibtnSuspendCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        msgBox.Visible = False
        CompleteMsgBox.Visible = False
        panInputDtm.Visible = False
        panSuspend.Visible = False
        panDelisting.Visible = False
        panActionBtn.Visible = True
    End Sub

    Protected Sub ibtnActionSuspendConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CompleteMsgBox.Visible = False

        Dim udtDB As Database = New Database
        Dim udtAuditLogEntryCollection(gvActionSuspendScheme.Rows.Count) As AuditLogEntry
        Dim intLogCount As Integer = 0

        Try
            If udtServiceProviderBLL.Exist Then
                Dim udtHCVUUser As HCVUUserModel = udtHCVUUserBLL.GetHCVUUser
                Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP

                Dim blnSuccess As Boolean = True
                udtDB.BeginTransaction()

                For Each r As GridViewRow In gvActionSuspendScheme.Rows
                    Dim strSchemeNameReal As String = r.Cells(2).Text.Trim
                    Dim strRemark As String = r.Cells(1).Text.Trim

                    ' Write Audit Log
                    udtAuditLogEntryCollection(intLogCount) = New AuditLogEntry(FunctionCode, Me)
                    udtAuditLogEntryCollection(intLogCount).AddDescripton("ERN", udtSP.EnrolRefNo)
                    udtAuditLogEntryCollection(intLogCount).AddDescripton("SPID", udtSP.SPID)
                    udtAuditLogEntryCollection(intLogCount).AddDescripton("Scheme", strSchemeNameReal)
                    udtAuditLogEntryCollection(intLogCount).AddDescripton("Remark", strRemark)
                    udtAuditLogEntryCollection(intLogCount).WriteStartLog(LogID.LOG00010, "Suspend SP Scheme")
                    intLogCount += 1

                    ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                    ' -------------------------------------------------------------------------
                    'Dim udtAcctChangeMaintenanceModel As AccountChangeMaintenanceModel = New AccountChangeMaintenanceModel(udtSP.SPID, _
                    '                                    SPAccountMaintenanceUpdTypeStatus.SPSuspend, Nothing, strRemark, String.Empty, _
                    '                                    String.Empty, 0, String.Empty, udtHCVUUser.UserID.Trim, String.Empty, Nothing, _
                    '                                    SPAccountMaintenanceRecordStatus.Active, strSchemeNameReal, Nothing)

                    'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    'Dim udtAcctChangeMaintenanceModel As AccountChangeMaintenanceModel = New AccountChangeMaintenanceModel(udtSP.SPID, _
                    '                SPAccountMaintenanceUpdTypeStatus.SPSuspend, Nothing, strRemark, String.Empty, _
                    '                String.Empty, 0, String.Empty, udtHCVUUser.UserID.Trim, String.Empty, Nothing, _
                    '                SPAccountMaintenanceRecordStatus.Active, strSchemeNameReal, Nothing, udtHCVUUser.UserID.Trim)

                    Dim udtAcctChangeMaintenanceModel As AccountChangeMaintenanceModel = New AccountChangeMaintenanceModel(udtSP.SPID, _
                                    SPAccountMaintenanceUpdTypeStatus.SPSuspend, Nothing, strRemark, String.Empty, _
                                    String.Empty, 0, String.Empty, udtHCVUUser.UserID.Trim, String.Empty, Nothing, _
                                    SPAccountMaintenanceRecordStatus.Active, strSchemeNameReal, Nothing, udtHCVUUser.UserID.Trim, _
                                    Nothing, Nothing, Nothing, Nothing, Nothing)

                    'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
                    ' INT13-0028 - SP Amendment Report [End][Tommy L]

                    If Not udtAccountChangeMaintenanceBLL.AddRecord(udtAcctChangeMaintenanceModel, udtDB) Then blnSuccess = False

                Next

                If blnSuccess Then
                    udtDB.CommitTransaction()
                    For i As Integer = 0 To intLogCount - 1
                        udtAuditLogEntryCollection(i).WriteEndLog(LogID.LOG00011, "Suspend SP Scheme successful")
                    Next

                    Dim strOld As String() = {"%s"}
                    Dim strNew As String() = {""}

                    strNew(0) = udtSP.EnglishName + udtFormatter.formatChineseName(udtSP.ChineseName) + " [" + udtSP.SPID + "] "

                    CompleteMsgBox.AddMessage(FunctionCode, "I", MsgCode.MSG00001, strOld, strNew)
                    CompleteMsgBox.BuildMessageBox()
                    CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Complete

                    panActionSuspend.Visible = False
                    MultiViewMaintenance.ActiveViewIndex = ViewIndexComplete

                Else
                    udtDB.RollBackTranscation()
                    For i As Integer = 0 To intLogCount - 1
                        udtAuditLogEntryCollection(i).WriteEndLog(LogID.LOG00012, "Suspend SP Scheme failed")
                    Next

                End If

            End If

        Catch eSQL As SqlClient.SqlException
            udtDB.RollBackTranscation()
            For i As Integer = 0 To intLogCount - 1
                udtAuditLogEntryCollection(i).WriteEndLog(LogID.LOG00012, "Suspend SP Scheme failed")
            Next

            If eSQL.Number = 50000 Then
                msgBox.AddMessage(New SystemMessage("990001", "D", eSQL.Message))

                If msgBox.GetCodeTable.Rows.Count = 0 Then
                    msgBox.Visible = False
                Else
                    msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, Common.Component.LogID.LOG00012, "Suspend SP Scheme failed")
                End If

            Else
                Throw eSQL
            End If

        Catch ex As Exception
            udtDB.RollBackTranscation()
            For i As Integer = 0 To intLogCount - 1
                udtAuditLogEntryCollection(i).WriteEndLog(LogID.LOG00012, "Suspend SP Scheme failed")
            Next

            Throw ex
        End Try

    End Sub

    Protected Sub ibtnActionSuspendBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CompleteMsgBox.Visible = False
        MultiViewMaintenance.ActiveViewIndex = ViewIndexDetails
        panActionSuspend.Visible = False
        panSuspend.Visible = True

    End Sub

    Protected Sub cboSchemeCode_SuspendScheme_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim r As GridViewRow = CType(sender, CheckBox).NamingContainer
        Dim cboSchemeCode As CheckBox = CType(sender, CheckBox)
        Dim txtRemark As TextBox = CType(r.FindControl("txtRemark"), TextBox)

        txtRemark.Enabled = cboSchemeCode.Checked

    End Sub

    ' Unlock

    Protected Sub ibtnUnlock_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim userACBLL As New UserAC.UserACBLL

        Dim gFunction As New Common.ComFunction.GeneralFunction
        'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtTokenBLL As New TokenBLL
        Dim udtTokenModel As New TokenModel
        'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
        Dim udtDB As Database = New Database
        Dim udtSP As ServiceProviderModel

        Dim udtHCVUUserBLL As New HCVUUserBLL
        Dim udtHCVUUser As HCVUUserModel = udtHCVUUserBLL.GetHCVUUser

        Dim blnSuccess As Boolean = False

        If udtServiceProviderBLL.Exist Then
            udtSP = udtServiceProviderBLL.GetSP

            'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            udtTokenModel = udtTokenBLL.GetTokenProfileByUserID(udtSP.SPID, DBNull.Value.ToString, udtDB)

            If Not udtTokenModel Is Nothing Then
                'CRE14-002 - PPI-ePR Migration [End][Chris YIM]

                'Write Audit Log
                udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
                udtAuditLogEntry.AddDescripton("ERN", udtSP.EnrolRefNo)
                udtAuditLogEntry.AddDescripton("SPID", udtSP.SPID)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00019, "Unlock UserAC")

                Try
                    udtDB.BeginTransaction()
                    blnSuccess = userACBLL.UpdateRecordStatusPasswordFailCount(udtSP.SPID, udtHCVUUser.UserID, udtDB)


                    If blnSuccess Then
                        ' CRE13-029 - RSA server upgrade [Start][Lawrence]
                        If udtTokenBLL.IsEnableToken Then
                            Dim udtRSA As New Common.Component.RSA_Manager.RSAServerHandler

                            ' CRE15-001 RSA Server Upgrade [Start][Winnie]
                            If udtRSA.IsParallelRun Then
                                udtTokenBLL.UpdateRSASingletonTSMP(udtDB)
                            End If
                            ' CRE15-001 RSA Server Upgrade [End][Winnie]

                            ' INT12-0007 Fix reset token with PPI token serial no. [Start][Koala]
                            ' -----------------------------------------------------------------------------------------
                            Dim strTokenSerialNo As String = (New SPProfileBLL).GetTokenModelBySPID(udtSP.SPID, True).TokenSerialNo
                            blnSuccess = udtRSA.resetRSAUserToken(strTokenSerialNo)
                            ' INT12-0007 Fix reset token with PPI token serial no. [End][Koala]
                        End If
                        ' CRE13-029 - RSA server upgrade [End][Lawrence]
                    End If

                    If blnSuccess Then
                        udtDB.CommitTransaction()
                    Else
                        udtDB.RollBackTranscation()
                    End If

                Catch ex As Exception
                    udtDB.RollBackTranscation()
                    Throw ex
                End Try
                'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
            Else
                msgBox.AddMessage(New SystemMessage(FunctionCode, "E", MsgCode.MSG00016))
                msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00117, "Unlock fail: <SP ID: " + udtSP.SPID + ">")
            End If
            'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
        End If

        If blnSuccess Then
            ' CRE13-029 - RSA server upgrade [Start][Lawrence]
            udtAuditLogEntry.WriteEndLog(LogID.LOG00020, "Unlock UserAC Successful")

            Dim lblnNextTokenMode As Boolean = False

            ' CRE15-001 RSA Server Upgrade [Start][Winnie]
            If (New RSAServerHandler).GetRSAAPIVersionMain <> String.Empty Then
                Try
                    lblnNextTokenMode = (New TokenBLL).IsTokenInNextTokenMode(Session(SESS_MaintenanceTokenSerialNo))
                Catch ex As Exception

                End Try
            End If
            ' CRE15-001 RSA Server Upgrade [End][Winnie]

            If lblnNextTokenMode Then
                ' Refresh the data to be the SP is unlocked
                btnSpDetails_Click(Nothing, Nothing)

                lblCTResult.Text = Me.GetGlobalResourceObject("Text", "Fail")
                ibtnCheckTokenRepair.Visible = True

                udcCTInfoMessageBox.AddMessage(FunctionCode, "I", MsgCode.MSG00015)
                udcCTInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
                udcCTInfoMessageBox.BuildMessageBox()

                ' I-CRE16-003 Fix XSS [Start][Lawrence]
                lblCTSPID.Text = AntiXssEncoder.HtmlEncode(hfSPID.Value, True)
                ' I-CRE16-003 Fix XSS [End][Lawrence]
                lblCTSerialNo.Text = Session(SESS_MaintenanceTokenSerialNo)

                hfPopupStatus.Value = SPMaintenancePopupStatus.CheckToken
                lblCheckTokenHeading.Text = Me.GetGlobalResourceObject("Text", "CheckToken")
                mvCT.SetActiveView(vCTC)

            Else
                CompleteMsgBox.AddMessage(FunctionCode, "I", MsgCode.MSG00010)
                CompleteMsgBox.BuildMessageBox()
                CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Complete

                MultiViewMaintenance.ActiveViewIndex = ViewIndexComplete

            End If
            ' CRE13-029 - RSA server upgrade [End][Lawrence]

        Else
            udtAuditLogEntry.WriteEndLog(LogID.LOG00021, "Unlock UserAC Failed")
        End If

    End Sub

    ' CRE13-029 - RSA Server Upgrade [Start][Lawrence]

    ' Check Token

    Protected Sub ibtnCheckToken_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        If Not IsNothing(sender) Then
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.AddDescripton("TokenSerialNo", Session(SESS_MaintenanceTokenSerialNo))
            udtAuditLogEntry.WriteStartLog(LogID.LOG00068, "Check Token click")
        End If

        msgBox.Visible = False
        udcCTInfoMessageBox.Visible = False
        udcCTMessageBox.Visible = False

        Dim udtTokenBLL As New TokenBLL

        Dim lblnNextTokenMode As Boolean = False
        Dim blnIsLockout As Boolean = False
        Try

            ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            ' Reset Lockout Status to allow repair token when token is lockout
            Dim strSPID As String = udtServiceProviderBLL.GetSP().SPID

            blnIsLockout = udtTokenBLL.IsUserLockout(strSPID)
            If blnIsLockout Then
                Call udtTokenBLL.ResetLockoutStatus(strSPID)
            End If
            ' CRE16-004 (Enable SP to unlock account) [End][Winnie]

            lblnNextTokenMode = (New TokenBLL).IsTokenInNextTokenMode(Session(SESS_MaintenanceTokenSerialNo))

        Catch ex As Exception
            ' Token service is temporary not available. Please try again later!
            msgBox.AddMessage(New SystemMessage(FunctionCode, "E", MsgCode.MSG00017))
            msgBox.BuildMessageBox("UpdateFail")

            If Not IsNothing(sender) Then
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Unknown exception: {0}", ex.ToString))
                udtAuditLogEntry.WriteEndLog(LogID.LOG00070, "Check Token fail")
            End If

            Return

        End Try

        If lblnNextTokenMode Then
            lblCTResult.Text = Me.GetGlobalResourceObject("Text", "Fail")
            ibtnCheckTokenRepair.Visible = True

            udcCTInfoMessageBox.AddMessage(FunctionCode, "I", MsgCode.MSG00014)
            udcCTInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
            udcCTInfoMessageBox.BuildMessageBox()

        Else
            lblCTResult.Text = Me.GetGlobalResourceObject("Text", "Pass")
            ibtnCheckTokenRepair.Visible = False

        End If

        ' I-CRE16-003 Fix XSS [Start][Lawrence]
        lblCTSPID.Text = AntiXssEncoder.HtmlEncode(hfSPID.Value, True)
        ' I-CRE16-003 Fix XSS [End][Lawrence]
        lblCTSerialNo.Text = Session(SESS_MaintenanceTokenSerialNo)

        hfPopupStatus.Value = SPMaintenancePopupStatus.CheckToken
        lblCheckTokenHeading.Text = Me.GetGlobalResourceObject("Text", "CheckToken")
        mvCT.SetActiveView(vCTC)

        If Not IsNothing(sender) Then
            udtAuditLogEntry.AddDescripton("IsNextTokenMode", IIf(lblnNextTokenMode, "Y", "N"))
            udtAuditLogEntry.WriteEndLog(LogID.LOG00069, "Check Token success")
        End If

    End Sub

    Protected Sub ibtnCheckTokenRepair_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00071, "Check Token - Repair Token click")

        udcCTInfoMessageBox.Visible = False
        udcCTMessageBox.Visible = False

        lblCheckTokenHeading.Text = Me.GetGlobalResourceObject("Text", "RepairToken")
        lblCTRMessage.Text = Me.GetGlobalResourceObject("Text", "RepairTokenWaitForFirstPasscodeMessage")
        ' I-CRE16-003 Fix XSS [Start][Lawrence]
        lblCTRServiceProviderID.Text = AntiXssEncoder.HtmlEncode(hfSPID.Value, True)
        ' I-CRE16-003 Fix XSS [End][Lawrence]
        lblCTRTokenSerialNo.Text = Session(SESS_MaintenanceTokenSerialNo)

        mvCT.SetActiveView(vCTR)

        txtCTRTokenPasscode.Visible = True
        imgCTRTokenPasscode.Visible = False
        imgCTRTokenPasscodeOK.Visible = False
        lblCTRNextPasscodeText.Visible = False
        txtCTRNextPasscode.Visible = False
        imgCTRNextPasscode.Visible = False
        ibtnCTRNext.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "NextBtn")

        ViewState(VS_RSARepairTokenStatus) = RSARepairTokenStatus.WaitForFirstPasscode

        ' CRE15-001 RSA Server Upgrade [Start][Winnie]
        ViewState(VS_RSARepairTokenSessionIDMain) = Nothing
        ViewState(VS_RSARepairTokenSessionIDSub) = Nothing
        ' CRE15-001 RSA Server Upgrade [End][Winnie]

        ScriptManager1.SetFocus(txtCTRTokenPasscode)

        udtAuditLogEntry.WriteEndLog(LogID.LOG00072, "Check Token - Repair Token success")

    End Sub

    Protected Sub ibtnCheckTokenClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00073, "Check Token - Close click")

        hfPopupStatus.Value = SPMaintenancePopupStatus.Closed

        udtAuditLogEntry.WriteEndLog(LogID.LOG00074, "Check Token - Close success")

    End Sub

    Protected Sub ibtnCTRNext_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        udcCTInfoMessageBox.Visible = False
        udcCTMessageBox.Visible = False
        imgCTRTokenPasscode.Visible = False
        imgCTRNextPasscode.Visible = False

        Select Case ViewState(VS_RSARepairTokenStatus)
            Case RSARepairTokenStatus.WaitForFirstPasscode
                udtAuditLogEntry.AddDescripton("FirstTokenPasscode", txtCTRTokenPasscode.Text)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00075, "Check Token - Repair Token - Next click")

                ' --- Validation ---
                If txtCTRTokenPasscode.Text.Trim = String.Empty Then
                    ' Message: Please input "First Token Passcode".
                    udcCTMessageBox.AddMessage(New SystemMessage(FunctionCode, "E", MsgCode.MSG00018))
                    udcCTMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00077, "Check Token - Repair Token - Next fail")

                    imgCTRTokenPasscode.Visible = True

                    Return
                End If

                If IsNumeric(txtCTRTokenPasscode.Text.Trim) = False Then
                    ' Message: Incorrect "First Token Passcode".
                    udcCTMessageBox.AddMessage(New SystemMessage(FunctionCode, "E", MsgCode.MSG00019))
                    udcCTMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00077, "Check Token - Repair Token - Next fail")

                    imgCTRTokenPasscode.Visible = True

                    Return
                End If
                ' --- End of Validation ---

                ' CRE15-001 RSA Server Upgrade [Start][Winnie]
                Dim lstrSessionIDMain As String = String.Empty
                Dim lstrSessionIDSub As String = String.Empty
                ' CRE15-001 RSA Server Upgrade [End][Winnie]

                Dim lintResult As Integer = (New TokenBLL).AuthWithNextTokenMode(hfSPID.Value, txtCTRTokenPasscode.Text.Trim, lstrSessionIDMain, lstrSessionIDSub)

                udtAuditLogEntry.AddDescripton("AuthResult", lintResult.ToString)

                Select Case lintResult
                    Case 0
                        ' Actually this should not be happening

                        ibtnCheckToken_Click(Nothing, Nothing)

                        ' Message: Token has been repaired.
                        udcCTInfoMessageBox.AddMessage(FunctionCode, "I", MsgCode.MSG00016)
                        udcCTInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                        udcCTInfoMessageBox.BuildMessageBox()

                        udtAuditLogEntry.WriteEndLog(LogID.LOG00076, "Check Token - Repair Token - Next success")

                    Case 1
                        ' Message: Incorrect "First Token Passcode".
                        udcCTMessageBox.AddMessage(New SystemMessage(FunctionCode, "E", MsgCode.MSG00019))
                        udcCTMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00077, "Check Token - Repair Token - Next fail")

                        imgCTRTokenPasscode.Visible = True

                        ScriptManager1.SetFocus(txtCTRTokenPasscode)

                    Case 2
                        ' CRE15-001 RSA Server Upgrade [Start][Winnie]
                        ViewState(VS_RSARepairTokenSessionIDMain) = lstrSessionIDMain
                        ViewState(VS_RSARepairTokenSessionIDSub) = lstrSessionIDSub
                        ' CRE15-001 RSA Server Upgrade [End][Winnie]

                        txtCTRTokenPasscode.Visible = False
                        imgCTRTokenPasscodeOK.Visible = True

                        lblCTRNextPasscodeText.Visible = True
                        txtCTRNextPasscode.Visible = True

                        lblCTRMessage.Text = Me.GetGlobalResourceObject("Text", "RepairTokenWaitForSecondPasscodeMessage")
                        ibtnCTRNext.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmBtn")

                        ViewState(VS_RSARepairTokenStatus) = RSARepairTokenStatus.WaitForSecondPasscode

                        ScriptManager1.SetFocus(txtCTRNextPasscode)

                        udtAuditLogEntry.WriteEndLog(LogID.LOG00076, "Check Token - Repair Token - Next success")

                End Select

            Case RSARepairTokenStatus.WaitForSecondPasscode
                udtAuditLogEntry.AddDescripton("SecondTokenPasscode", txtCTRNextPasscode.Text)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00078, "Check Token - Repair Token - Confirm click")

                ' --- Validation ---
                If txtCTRNextPasscode.Text.Trim = String.Empty Then
                    ' Message: Please input "Second Token Passcode".
                    udcCTMessageBox.AddMessage(New SystemMessage(FunctionCode, "E", MsgCode.MSG00020))
                    udcCTMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00080, "Check Token - Repair Token - Confirm fail")

                    imgCTRNextPasscode.Visible = True

                    Return
                End If

                If IsNumeric(txtCTRNextPasscode.Text.Trim) = False Then
                    ' Message: Incorrect "Second Token Passcode".
                    udcCTMessageBox.AddMessage(New SystemMessage(FunctionCode, "E", MsgCode.MSG00021))
                    udcCTMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00080, "Check Token - Repair Token - Confirm fail")

                    imgCTRNextPasscode.Visible = True

                    Return
                End If
                ' --- End of Validation ---

                Dim lintResult As Integer = (New TokenBLL).AuthWithNextTokenMode(hfSPID.Value, txtCTRNextPasscode.Text.Trim, ViewState(VS_RSARepairTokenSessionIDMain), ViewState(VS_RSARepairTokenSessionIDSub))

                udtAuditLogEntry.AddDescripton("AuthResult", lintResult.ToString)

                Select Case lintResult
                    Case 0
                        ibtnCheckToken_Click(Nothing, Nothing)

                        ' Message: Token has been repaired.
                        udcCTInfoMessageBox.AddMessage(FunctionCode, "I", MsgCode.MSG00016)
                        udcCTInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                        udcCTInfoMessageBox.BuildMessageBox()

                        udtAuditLogEntry.WriteEndLog(LogID.LOG00079, "Check Token - Repair Token - Confirm success")

                    Case 1
                        ' Message: Incorrect token passcode, please restart the repair process.
                        udcCTMessageBox.AddMessage(New SystemMessage(FunctionCode, "E", MsgCode.MSG00022))
                        udcCTMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00080, "Check Token - Repair Token - Confirm fail")

                        mvCT.SetActiveView(vCTC)

                End Select

        End Select

    End Sub

    Protected Sub ibtnCTRCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00081, "Check Token - Repair Token - Cancel click")

        hfPopupStatus.Value = SPMaintenancePopupStatus.Closed

        udtAuditLogEntry.WriteEndLog(LogID.LOG00082, "Check Token - Repair Token - Cancel success")

    End Sub

    ' CRE13-029 - RSA Server Upgrade [End][Lawrence]

    ' Release IVSS Claim

    Protected Sub ibtnReleaseIVSSClaim_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CompleteMsgBox.Visible = False
        panActionBtn.Visible = False
        panReleaseIVSSClaim.Visible = True

        txtReleaseDtmFrom.Text = String.Empty
        txtReleaseDtmTo.Text = String.Empty
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'lblCompletedBefore.Text = udtFormatter.formatDate(DateTime.Today.Date.AddDays(3))
        lblCompletedBefore.Text = udtFormatter.formatDisplayDate(DateTime.Today.Date.AddDays(3))
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
    End Sub

    Protected Sub ibtnReleaseIVSSClaimSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CompleteMsgBox.Visible = False
        msgBox.Visible = False

        imgAlertReleaseDtm.Visible = False

        SM = udtValidator.chkOptionalInputDate("010201", txtReleaseDtmFrom.Text.Trim, "00003", "00004")
        If Not IsNothing(SM) Then
            imgAlertReleaseDtm.Visible = True
            msgBox.AddMessage(SM)
        End If

        SM = udtValidator.chkOptionalInputDate("010201", txtReleaseDtmTo.Text.Trim, "00005", "00006")
        If Not IsNothing(SM) Then
            imgAlertReleaseDtm.Visible = True
            msgBox.AddMessage(SM)
        End If

        If msgBox.GetCodeTable.Rows.Count = 0 Then
            BindSPActionDetails()

            If udtValidator.IsEmpty(txtReleaseDtmFrom.Text.Trim) Then
                lblActionServiceDateFrom.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                lblActionServiceDateFrom.Text = udtFormatter.convertDate(txtReleaseDtmFrom.Text.Trim, String.Empty)
            End If

            If udtValidator.IsEmpty(txtReleaseDtmTo.Text.Trim) Then
                lblActionServiceDateTo.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                lblActionServiceDateTo.Text = udtFormatter.convertDate(txtReleaseDtmTo.Text.Trim, String.Empty)
            End If

            lblActionCompleteDateBefore.Text = lblCompletedBefore.Text

            CompleteMsgBox.AddMessage("010201", "I", "00018")
            CompleteMsgBox.BuildMessageBox()
            CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information

            panReleaseIVSSClaim.Visible = False
            panActionReleaseIVSSClaim.Visible = True
            MultiViewMaintenance.ActiveViewIndex = ViewIndexSPActionDetails

        Else
            msgBox.BuildMessageBox("ValidationFail")
        End If

    End Sub

    Protected Sub ibtnReleaseIVSSClaimCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        msgBox.Visible = False
        CompleteMsgBox.Visible = False
        panReleaseIVSSClaim.Visible = False
        panActionBtn.Visible = True
    End Sub

    Protected Sub ibtnActionReleaseIVSSClaimConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CompleteMsgBox.Visible = False

        Try
            If udtServiceProviderBLL.Exist Then
                Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP

                Dim strOld As String() = {"%s"}
                Dim strNew As String() = {""}

                strNew(0) = udtSP.EnglishName + udtFormatter.formatChineseName(udtSP.ChineseName) + " [" + udtSP.SPID + "] "

                CompleteMsgBox.AddMessage(FunctionCode, "I", MsgCode.MSG00011, strOld, strNew)
                CompleteMsgBox.BuildMessageBox()
                CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Complete

                MultiViewMaintenance.ActiveViewIndex = ViewIndexComplete

            End If

        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00018, "Edit Return Info Failed")
            Throw ex
        End Try
    End Sub

    Protected Sub ibtnActionReleaseIVSSClaimBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CompleteMsgBox.Visible = False
        panActionReleaseIVSSClaim.Visible = False
        panReleaseIVSSClaim.Visible = True
        MultiViewMaintenance.ActiveViewIndex = ViewIndexDetails
    End Sub

    ' Resend Email

    Protected Sub ibtnResendEmail_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00059, "Resend Email starts")

        msgBox.Visible = False
        CompleteMsgBox.Visible = False

        hfPopupStatus.Value = SPMaintenancePopupStatus.ResendEmail

        ibtnResendEmailSend.Enabled = False

        ' Activation Email
        rblResendEmail.Items(0).Enabled = hfAccountStatus.Value = SPAccountStatus.PendingForActivation

        ' Confirmation Email
        If imgEditEmail.Visible AndAlso hfAccountStatus.Value <> SPAccountStatus.Delisted Then
            rblResendEmail.Items(2).Enabled = True
        Else
            rblResendEmail.Items(2).Enabled = False
        End If

        ' Scheme Enrolment Email (1) / Delist Email (3)
        rblResendEmail.Items(1).Enabled = True
        rblResendEmail.Items(3).Enabled = False

        For Each udtScheme As SchemeInformationModel In CType(Session(SESS_SPSchemeInformation), SchemeInformationModelCollection).Values
            If udtScheme.RecordStatus = SchemeInformationMaintenanceDisplayStatus.DelistedVoluntary _
                    OrElse udtScheme.RecordStatus = SchemeInformationMaintenanceDisplayStatus.DelistedInvoluntary Then
                rblResendEmail.Items(1).Enabled = False
                rblResendEmail.Items(3).Enabled = True
            End If
        Next

        Dim rboSelected As ListItem = Nothing
        Dim intEnabled As Integer = 0

        rblResendEmail.ClearSelection()
        For Each rbo As ListItem In rblResendEmail.Items
            If rbo.Enabled Then
                intEnabled += 1
                rboSelected = rbo
            End If
        Next

        If intEnabled = 1 Then
            rboSelected.Selected = True
            ibtnResendEmailSend.Enabled = True
        End If

        ibtnResendEmailSend.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", IIf(ibtnResendEmailSend.Enabled, "SendBtn", "SendDisableBtn"))

    End Sub

    Protected Sub ibtnResendEmailSend_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Email", rblResendEmail.SelectedValue)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00060, "Resend Email selects")

        Select Case rblResendEmail.SelectedValue
            Case "A"
                lblPrintFunction.Text = Me.GetGlobalResourceObject("Text", "Resend") + " " + Me.GetGlobalResourceObject("Text", "ActivationEmail")
                udcPrintFunction.PrepareThePanel("SchemeSelection", ActionSendActivationEmail, udtServiceProviderBLL.GetSP, True, FunctionCode)
                hfPopupStatus.Value = SPMaintenancePopupStatus.ResendActivationEmail

            Case "S"
                lblPrintFunction.Text = Me.GetGlobalResourceObject("Text", "Resend") + " " + Me.GetGlobalResourceObject("Text", "SchemeEnrolmentEmail")
                udcPrintFunction.PrepareThePanel("SchemeSelection", ActionSendSchemeEnrolmentEmail, udtServiceProviderBLL.GetSP, False, FunctionCode)
                hfPopupStatus.Value = SPMaintenancePopupStatus.ResendSchemeEnrolmentEmail

            Case "C"
                ResendConfirmEmail()
                hfPopupStatus.Value = SPMaintenancePopupStatus.Closed

            Case "D"
                lblPrintFunction.Text = Me.GetGlobalResourceObject("Text", "Resend") + " " + Me.GetGlobalResourceObject("Text", "DelistEmail")
                udcPrintFunction.PrepareThePanel("SchemeSelection", ActionSendDelistEmail, udtServiceProviderBLL.GetSP, True, FunctionCode)
                hfPopupStatus.Value = SPMaintenancePopupStatus.ResendDelistEmail

        End Select

    End Sub

    Protected Sub ibtnResendEmailCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        hfPopupStatus.Value = SPMaintenancePopupStatus.Closed
    End Sub

    Protected Sub rblResendEmail_SelectedIndexChange(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not ibtnResendEmailSend.Enabled Then
            ibtnResendEmailSend.Enabled = True
            ibtnResendEmailSend.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SendBtn")
        End If
    End Sub

    ' --> Resend Confirmation Email

    Private Sub ResendConfirmEmail()
        CompleteMsgBox.Visible = False

        Dim udtInternetMailBLL As New InternetMailBLL
        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction

        Dim strActivationCode As String = udtGeneralFunction.generateAccountActivationCode()
        Dim udtDB As Database = New Database
        Dim udtSP As ServiceProviderModel
        Dim strUserID As String = udtHCVUUserBLL.GetHCVUUser.UserID.Trim

        Dim blnsuccess As Boolean = False

        If udtServiceProviderBLL.Exist Then
            udtSP = udtServiceProviderBLL.GetSP

            'Write Audit Log
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.AddDescripton("ERN", udtSP.EnrolRefNo)
            udtAuditLogEntry.AddDescripton("SPID", udtSP.SPID)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00023, "Resend Email Address Change Confirmation Email")
            Try
                udtDB.BeginTransaction()

                ' CRE16-018 Display SP tentative email in HCVU [Start][Winnie]
                'udtServiceProviderBLL.UpdateServiceProviderEmailActivationCode(udtDB, udtSP.SPID, strUserID, Common.Encryption.Encrypt.MD5hash(strActivationCode), _
                '                                                            udtSP.TSMP, True)

                ' I-CRE16-007-02 (Refine system from CheckMarx findings) [Start][Winnie]
                udtServiceProviderBLL.UpdateServiceProviderEmailActivationCode(udtDB, udtSP.SPID, strUserID, Hash(strActivationCode), _
                                                                            udtSP.TSMP, False)
                ' I-CRE16-007-02 (Refine system from CheckMarx findings) [End][Winnie]

                ' CRE16-018 Display SP tentative email in HCVU [End][Winnie]

                blnsuccess = udtInternetMailBLL.SubmitEmailAddressChangeConfirmationEmail(udtDB, Me.hfSPID.Value.Trim, strActivationCode)

                If blnsuccess Then
                    udtDB.CommitTransaction()

                    ' CRE16-018 Display SP tentative email in HCVU [Start][Winnie]
                    ' Refresh the data since Activation Code is updated
                    btnSpDetails_Click(Nothing, Nothing)
                    ' CRE16-018 Display SP tentative email in HCVU [End][Winnie]

                    CompleteMsgBox.AddMessage(FunctionCode, "I", MsgCode.MSG00007)
                    CompleteMsgBox.BuildMessageBox()
                    CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Complete

                    udtAuditLogEntry.WriteEndLog(LogID.LOG00024, "Resend Email Address Change Confirmation Email successful")

                Else
                    udtDB.RollBackTranscation()
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00025, "Resend Email Address Change Confirmation Email failed")
                End If

            Catch eSQL As SqlClient.SqlException
                udtDB.RollBackTranscation()

                If eSQL.Number = 50000 Then
                    msgBox.AddMessage(New SystemMessage("990001", SeverityCode.SEVD, eSQL.Message))
                    msgBox.BuildMessageBox("UpdateFail")
                Else
                    Throw
                End If
            Catch ex As Exception
                udtDB.RollBackTranscation()

                udtAuditLogEntry.WriteEndLog(LogID.LOG00025, "Resend Email Address Change Confirmation Email failed")
                Throw
            End Try
        End If

    End Sub

    ' Reprint Letter

    Protected Sub ibtnReprintLetter_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtTokenBLL As New TokenBLL
        ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        Dim strSPID As String = udtServiceProviderBLL.GetSP().SPID
        Dim udtTokenModel As TokenModel = udtSPProfileBLL.GetTokenModelBySPID(strSPID, False)
        ' CRE14-002 - PPI-ePR Migration [End][Tommy L]

        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00061, "Reprint Letter starts")

        msgBox.Visible = False
        CompleteMsgBox.Visible = False

        hfPopupStatus.Value = SPMaintenancePopupStatus.ReprintLetter

        ibtnReprintLetterPrint.Enabled = False

        ' Acknowledgement Letter
        rblReprintLetter.Items(0).Enabled = hfAccountStatus.Value = SPAccountStatus.PendingForActivation

        ' Scheme Enrolment Letter
        rblReprintLetter.Items(1).Enabled = False

        For Each udtScheme As SchemeInformationModel In CType(Session(SESS_SPSchemeInformation), SchemeInformationModelCollection).Values
            If udtScheme.RecordStatus <> SchemeInformationMaintenanceDisplayStatus.DelistedVoluntary _
                    AndAlso udtScheme.RecordStatus <> SchemeInformationMaintenanceDisplayStatus.DelistedInvoluntary Then
                rblReprintLetter.Items(1).Enabled = True
                Exit For
            End If
        Next

        ' CRE13-003 - Token Replacement [Start][Tommy L]
        ' -------------------------------------------------------------------------------------
        ' Token Replacement Letter
        rblReprintLetter.Items(2).Enabled = False

        ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        'If udtTokenBLL.IsRequiredActivateAfterTokenReplaced(udtServiceProviderBLL.GetSP().SPID) Then
        '    rblReprintLetter.Items(2).Enabled = True
        'End If
        If udtTokenBLL.IsRequiredActivateAfterTokenReplaced(strSPID) Then
            If udtTokenModel.ProjectReplacement.Equals(TokenProjectType.EHCVS) Then
                rblReprintLetter.Items(2).Enabled = True
            End If
        End If
        ' CRE14-002 - PPI-ePR Migration [End][Tommy L]
        ' CRE13-003 - Token Replacement [End][Tommy L]

        Dim rboSelected As ListItem = Nothing
        Dim intEnabled As Integer = 0

        rblReprintLetter.ClearSelection()
        For Each rbo As ListItem In rblReprintLetter.Items
            If rbo.Enabled Then
                intEnabled += 1
                rboSelected = rbo
            End If
        Next

        If intEnabled = 1 Then
            rboSelected.Selected = True
            ibtnReprintLetterPrint.Enabled = True
        End If

        ibtnReprintLetterPrint.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", IIf(ibtnReprintLetterPrint.Enabled, "PrintBtn", "PrintDisableBtn"))

    End Sub

    Protected Sub ibtnReprintLetterPrint_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Letter", rblReprintLetter.SelectedValue)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00062, "Reprint Letter selects")

        Select Case rblReprintLetter.SelectedValue
            Case "A"
                Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP
                udtSP.TokenSerialNo = Session(SESS_MaintenanceTokenSerialNo)

                lblPrintFunction.Text = Me.GetGlobalResourceObject("Text", "Reprint") + " " + Me.GetGlobalResourceObject("Text", "AcknowledgementLetter")
                udcPrintFunction.PrepareThePanel("SchemeSelection", ActionPrintNewEnrolmentLetter, udtSP, True, FunctionCode)
                Session(SESS_PrintFunctionStatus) = PrintFunctionStatus.ActivePrintFunction

                hfPopupStatus.Value = SPMaintenancePopupStatus.ReprintAcknowledgementLetter

            Case "S"
                lblPrintFunction.Text = Me.GetGlobalResourceObject("Text", "Reprint") + " " + Me.GetGlobalResourceObject("Text", "SchemeEnrolmentLetter")
                udcPrintFunction.PrepareThePanel("SchemeSelection", ActionPrintSchemeEnrolmentLetter, udtServiceProviderBLL.GetSP, True, FunctionCode)
                Session(SESS_PrintFunctionStatus) = PrintFunctionStatus.ActivePrintFunction

                hfPopupStatus.Value = SPMaintenancePopupStatus.ReprintSchemeEnrolmentLetter

                ' CRE13-003 - Token Replacement [Start][Tommy L]
                ' -------------------------------------------------------------------------------------
            Case "T"
                udcPrintFunction.PrepareThePanel(String.Empty, ActionPrintTokenReplacementLetter, udtServiceProviderBLL.GetSP, True, FunctionCode)
                udcPrintFunction.PrintLetterWithoutPopup()

                Session(SESS_PrintFunctionStatus) = PrintFunctionStatus.FinishPrintFunction
                hfPopupStatus.Value = SPMaintenancePopupStatus.Closed
                ' CRE13-003 - Token Replacement [End][Tommy L]

        End Select

    End Sub

    Protected Sub ibtnReprintLetterCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        hfPopupStatus.Value = SPMaintenancePopupStatus.Closed
    End Sub

    Protected Sub rblReprintLetter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not ibtnReprintLetterPrint.Enabled Then
            ibtnReprintLetterPrint.Enabled = True
            ibtnReprintLetterPrint.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "PrintBtn")
        End If
    End Sub

#End Region

#Region "Practice Buttons"

    ' Delisting Practice

    Protected Sub ibtnSPracticeDelisting_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00056, "Delist Practice Scheme starts")

        CompleteMsgBox.Visible = False
        msgBox.Visible = False

        ' Change the Heading Text -> Delisting Practice Scheme
        lblActionPracticeHeading.Text = Me.GetGlobalResourceObject("Text", "DelistingPracticeScheme")

        Dim udtSchemeBackOfficeModelCollection As SchemeBackOfficeModelCollection = udtSchemeBackOfficeBLL.GetAllEffectiveSchemeBackOfficeWithSubsidizeGroupFromCache()
        gvDelistPracticeScheme.DataSource = udtSchemeBackOfficeModelCollection
        gvDelistPracticeScheme.DataBind()

        Dim dicRowToScheme As New Dictionary(Of Integer, String)
        Dim aryVisibleScheme As New ArrayList

        For i As Integer = 0 To udtSchemeBackOfficeModelCollection.Count - 1
            dicRowToScheme.Add(i, udtSchemeBackOfficeModelCollection(i).SchemeCode.Trim)
        Next

        Dim dicSPSchemeToStatus As New Dictionary(Of String, String)
        For Each udtScheme As SchemeInformationModel In udtServiceProviderBLL.GetSP.SchemeInfoList.Values
            dicSPSchemeToStatus.Add(udtScheme.SchemeCode, udtScheme.RecordStatus)
        Next

        ' Get the PracticeSchemeInfo the selected row
        Dim r As GridViewRow = CType(sender, ImageButton).NamingContainer
        Dim gvPracticeSchemeInfo As GridView = CType(r.FindControl("gvPracticeSchemeInfo"), GridView)

        For Each row As GridViewRow In gvPracticeSchemeInfo.Rows
            ' CRE16-022 (Add optional field "Remarks") [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            'Dim strSchemeCodeReal As String = CType(row.FindControl("hfPracticeSchemeCodeReal"), HiddenField).Value.Trim
            'Dim strStatusReal As String = CType(row.FindControl("hfPracticeSchemeStatusReal"), HiddenField).Value.Trim
            Dim strSchemeCodeReal As String = CType(row.FindControl("lblPracticeSchemeCode"), Label).Attributes("SchemeCodeReal").Trim
            Dim strStatusReal As String = CType(row.FindControl("lblPracticeSchemeStatus"), Label).Attributes("StatusReal").Trim
            ' CRE16-022 (Add optional field "Remarks") [End][Chris YIM]


            ' If the Scheme is Pending Delist, does not allow any control on the Practice Scheme
            If dicSPSchemeToStatus.ContainsKey(strSchemeCodeReal) _
                    AndAlso (dicSPSchemeToStatus(strSchemeCodeReal) = SchemeInformationMaintenanceDisplayStatus.ActivePendingDelist _
                         OrElse dicSPSchemeToStatus(strSchemeCodeReal) = SchemeInformationMaintenanceDisplayStatus.SuspendedPendingDelist) Then
                Continue For
            End If

            If (strStatusReal = PracticeSchemeInfoMaintenanceDisplayStatus.Active _
                     OrElse strStatusReal = PracticeSchemeInfoMaintenanceDisplayStatus.Suspended) _
                     AndAlso Not aryVisibleScheme.Contains(strSchemeCodeReal) Then
                aryVisibleScheme.Add(strSchemeCodeReal)
            End If
        Next

        Dim intRowNoVisible As Integer

        For i As Integer = 0 To gvDelistPracticeScheme.Rows.Count - 1
            If aryVisibleScheme.Contains(dicRowToScheme(i)) Then
                intRowNoVisible = i
            Else
                gvDelistPracticeScheme.Rows(i).Visible = False
            End If
        Next

        ' Auto-check the checkbox if there is only one
        If aryVisibleScheme.Count = 1 Then
            Dim row As GridViewRow = gvDelistPracticeScheme.Rows(intRowNoVisible)

            Dim cboSchemeCode As CheckBox = CType(row.FindControl("cboSchemeCode"), CheckBox)
            cboSchemeCode.Checked = True
            cboSchemeCode.Attributes.Add("onclick", "return false;")

            CType(row.FindControl("rbDelistType"), RadioButtonList).Enabled = True
            CType(row.FindControl("txtRemark"), TextBox).Enabled = True

        End If

        ' Bind the grid view
        If udtServiceProviderBLL.Exist Then
            Dim udtNewPractice As Practice.PracticeModel = udtServiceProviderBLL.GetSP.PracticeList.Item(r.RowIndex + 1)
            Session(SESS_SelectedPractice) = udtNewPractice

            Dim udtNewPracticeList As Practice.PracticeModelCollection = New Practice.PracticeModelCollection
            udtNewPracticeList.Add(udtNewPractice)

            Session(SESS_SPMaintenanceHandledScheme) = New ArrayList()

            gvActionPracticeBank.DataSource = udtNewPracticeList.Values
            gvActionPracticeBank.DataBind()

            Session.Remove(SESS_SPMaintenanceHandledScheme)

        End If

        MultiViewMaintenance.ActiveViewIndex = ViewIndexPracticeActionDetails
        panPracticeDelisting.Visible = True

    End Sub

    Protected Sub ibtnPracticeDelistingSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CompleteMsgBox.Visible = False
        msgBox.Visible = False

        Dim intChecked As Integer = 0

        For Each r As GridViewRow In gvDelistPracticeScheme.Rows
            Dim cboSchemeCode As CheckBox = CType(r.FindControl("cboSchemeCode"), CheckBox)
            Dim rbDelistType As RadioButtonList = CType(r.FindControl("rbDelistType"), RadioButtonList)
            Dim txtRemark As TextBox = CType(r.FindControl("txtRemark"), TextBox)

            Dim imgAlertSchemeCode As Image = CType(r.FindControl("imgAlertSchemeCode"), Image)
            Dim imgAlertDelistType As Image = CType(r.FindControl("imgAlertDelistType"), Image)
            Dim imgAlertRemark As Image = CType(r.FindControl("imgAlertRemark"), Image)

            imgAlertSchemeCode.Visible = False
            imgAlertDelistType.Visible = False
            imgAlertRemark.Visible = False

            If cboSchemeCode.Checked Then
                intChecked += 1

                ' Check "Voluntary" or "Involuntary" selected
                If udtValidator.IsEmpty(rbDelistType.SelectedValue) Then
                    imgAlertDelistType.Visible = True
                    msgBox.AddMessage(New SystemMessage("010201", "E", "00001"))
                End If

                ' Check Remarks input
                If udtValidator.IsEmpty(txtRemark.Text.Trim) Then
                    imgAlertRemark.Visible = True
                    msgBox.AddMessage(New SystemMessage("010201", "E", "00002"))
                End If
            End If
        Next

        ' Check at least one checkbox checked
        If intChecked = 0 Then
            msgBox.AddMessage(New SystemMessage(FunctionCode, "E", MsgCode.MSG00011))

            For Each r As GridViewRow In gvDelistPracticeScheme.Rows
                CType(r.FindControl("imgAlertSchemeCode"), Image).Visible = True
            Next
        End If

        If msgBox.GetCodeTable.Rows.Count = 0 Then
            msgBox.Visible = False

            If udtServiceProviderBLL.Exist Then
                panPracticeDelisting.Visible = False
                panActionPracticeDelisting.Visible = True

                ' Construct the Delist Scheme table
                Dim dsDelist As New DataSet
                Dim dtDelist As DataTable
                Dim drDelist As DataRow

                dsDelist.Tables.Add(New DataTable("TempTable"))
                dtDelist = dsDelist.Tables.Item("TempTable")
                dtDelist.Columns.Add(Me.GetGlobalResourceObject("Text", "SchemeName"))
                dtDelist.Columns.Add(Me.GetGlobalResourceObject("Text", "DelistingType"))
                dtDelist.Columns.Add(Me.GetGlobalResourceObject("Text", "Remarks"))
                dtDelist.Columns.Add("SchemeNameReal")
                dtDelist.Columns.Add("DelistingTypeReal")

                Const intSchemeCodeColumn As Integer = 0
                Const intDelistTypeColumn As Integer = 1
                Const intRemarkColumn As Integer = 2
                Const intSchemeCodeRealColumn As Integer = 3
                Const intDelistTypeRealColumn As Integer = 4

                For Each r As GridViewRow In gvDelistPracticeScheme.Rows
                    Dim cboSchemeCode As CheckBox = CType(r.FindControl("cboSchemeCode"), CheckBox)

                    If cboSchemeCode.Checked Then
                        drDelist = dsDelist.Tables.Item("TempTable").NewRow()
                        drDelist.Item(intSchemeCodeColumn) = cboSchemeCode.Text

                        Dim rbDelistType As RadioButtonList = CType(r.FindControl("rbDelistType"), RadioButtonList)
                        drDelist.Item(intDelistTypeColumn) = rbDelistType.SelectedItem.Text

                        drDelist.Item(intRemarkColumn) = CType(r.FindControl("txtRemark"), TextBox).Text
                        ' CRE16-022 (Add optional field "Remarks") [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                        'drDelist.Item(intSchemeCodeRealColumn) = CType(r.FindControl("hfSchemeCodeReal"), HiddenField).Value
                        drDelist.Item(intSchemeCodeRealColumn) = CType(r.FindControl("cboSchemeCode"), CheckBox).Attributes("SchemeCodePracticeDelist")
                        ' CRE16-022 (Add optional field "Remarks") [End][Chris YIM]
                        drDelist.Item(intDelistTypeRealColumn) = rbDelistType.SelectedValue

                        dsDelist.Tables.Item("TempTable").Rows.Add(drDelist)
                    End If
                Next

                gvActionPDelistScheme.DataSource = dsDelist
                gvActionPDelistScheme.DataBind()

                ' Hide the SchemeNameReal and DelistingTypeReal columns
                gvActionPDelistScheme.HeaderRow.Cells(intSchemeCodeRealColumn).Visible = False
                gvActionPDelistScheme.HeaderRow.Cells(intDelistTypeRealColumn).Visible = False
                For Each r As GridViewRow In gvActionPDelistScheme.Rows
                    r.Cells(intSchemeCodeRealColumn).Visible = False
                    r.Cells(intDelistTypeRealColumn).Visible = False
                Next

                ' Control the width of the "Remarks" column
                gvActionPDelistScheme.Rows(0).Cells(intRemarkColumn).Width = 250

                ' Build the complete message box
                CompleteMsgBox.AddMessage(FunctionCode, "I", MsgCode.MSG00003)
                CompleteMsgBox.BuildMessageBox()
                CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information

                MultiViewMaintenance.ActiveViewIndex = ViewIndexComplete
                MultiViewMaintenance.ActiveViewIndex = ViewIndexPracticeActionDetails

            End If

        Else
            msgBox.BuildMessageBox("ValidationFail")
        End If

    End Sub

    Protected Sub ibtnPracticeDelistingCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        msgBox.Visible = False
        CompleteMsgBox.Visible = False
        MultiViewMaintenance.ActiveViewIndex = ViewIndexDetails
        panPracticeDelisting.Visible = False
        Session.Remove(SESS_SelectedPractice)
    End Sub

    Protected Sub ibtnActionPracticeDelistingConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CompleteMsgBox.Visible = False

        If udtServiceProviderBLL.Exist Then
            Dim udtHCVUUser As HCVUUserModel = udtHCVUUserBLL.GetHCVUUser
            Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP
            Dim udtPractice As PracticeModel = CType(Session(SESS_SelectedPractice), PracticeModel)

            Dim i As Integer = 0
            Dim udtAuditLogEntryCollection(gvActionPDelistScheme.Rows.Count) As AuditLogEntry

            Dim udtDB As New Database
            udtDB.BeginTransaction()

            Try
                Dim blnSuccess As Boolean = True

                ' Write Audit Log
                For Each r As GridViewRow In gvActionPDelistScheme.Rows
                    Dim strRemark As String = r.Cells(2).Text.Trim
                    Dim strSchemeCodeReal As String = r.Cells(3).Text.Trim
                    Dim strDelistTypeReal As String = r.Cells(4).Text.Trim

                    udtAuditLogEntryCollection(i) = New AuditLogEntry(FunctionCode, Me)
                    udtAuditLogEntryCollection(i).AddDescripton("ERN", udtSP.EnrolRefNo)
                    udtAuditLogEntryCollection(i).AddDescripton("SPID", udtSP.SPID)
                    udtAuditLogEntryCollection(i).AddDescripton("PracticeSeq", udtPractice.DisplaySeq)
                    udtAuditLogEntryCollection(i).AddDescripton("Scheme", strSchemeCodeReal)
                    udtAuditLogEntryCollection(i).AddDescripton("DelistingType", strDelistTypeReal)
                    udtAuditLogEntryCollection(i).AddDescripton("Remark", strRemark)
                    udtAuditLogEntryCollection(i).WriteStartLog(LogID.LOG00035, "Delist Practice Scheme")
                    i = i + 1

                    ' Just a quite dummy code here - because the function "AddSPAccMaintenanceByPracticeList" needs a PracticeModelCollection,
                    '   therefore create a new PracticeModelCollection and add the ONLY ONE practice to it
                    Dim udtNewPracticeList As New PracticeModelCollection
                    udtNewPracticeList.Add(udtPractice)

                    If Not udtAccountChangeMaintenanceBLL.AddSPAccMaintenanceByPracticeList(udtSP.SPID, udtNewPracticeList, strRemark, strDelistTypeReal, _
                                                                                            udtHCVUUser.UserID.Trim, strSchemeCodeReal, udtDB) Then
                        blnSuccess = False
                    End If

                Next

                Session.Remove(SESS_SelectedPractice)

                If blnSuccess Then
                    udtDB.CommitTransaction()

                    Dim strOld As String() = {"%s"}
                    Dim strNew As String() = {""}

                    strNew(0) = udtSP.EnglishName + udtFormatter.formatChineseName(udtSP.ChineseName) + " [" + udtSP.SPID + "] "

                    CompleteMsgBox.AddMessage("010201", "I", "00001", strOld, strNew)
                    CompleteMsgBox.BuildMessageBox()
                    CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Complete

                    MultiViewMaintenance.ActiveViewIndex = ViewIndexComplete
                    panActionPracticeDelisting.Visible = False

                    For j As Integer = 0 To i - 1
                        udtAuditLogEntryCollection(j).WriteEndLog(LogID.LOG00036, "Delist Practice Scheme successful")
                    Next

                Else
                    udtDB.RollBackTranscation()

                    For j As Integer = 0 To i - 1
                        udtAuditLogEntryCollection(j).WriteEndLog(LogID.LOG00037, "Delist Practice Scheme failed")
                    Next

                End If

            Catch eSQL As SqlClient.SqlException
                udtDB.RollBackTranscation()

                If eSQL.Number = 50000 Then
                    msgBox.AddMessage(New SystemMessage("990001", "D", eSQL.Message))

                    If msgBox.GetCodeTable.Rows.Count = 0 Then
                        msgBox.Visible = False
                    Else
                        msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, Common.Component.LogID.LOG00037, "Delist Practice Scheme failed")
                    End If

                Else
                    Throw eSQL
                End If

            Catch ex As Exception
                udtDB.RollBackTranscation()

                For j As Integer = 0 To i - 1
                    udtAuditLogEntryCollection(j).WriteEndLog(LogID.LOG00037, "Delist Practice Scheme failed")
                Next

                Throw ex
            End Try
        Else
            Session.Remove(SESS_SelectedPractice)
        End If

    End Sub

    Protected Sub ibtnActionPracticeDelistingBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CompleteMsgBox.Visible = False
        panActionPracticeDelisting.Visible = False
        panPracticeDelisting.Visible = True
    End Sub

    ' Suspend Practice

    Protected Sub ibtnSPracticeSuspend_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00057, "Suspend Practice Scheme starts")

        CompleteMsgBox.Visible = False
        msgBox.Visible = False

        ' Change the Heading Text -> Suspend Practice Scheme
        lblActionPracticeHeading.Text = Me.GetGlobalResourceObject("Text", "SuspendPracticeScheme")

        Dim udtSchemeBackOfficeModelCollection As SchemeBackOfficeModelCollection = udtSchemeBackOfficeBLL.GetAllEffectiveSchemeBackOfficeWithSubsidizeGroupFromCache()
        gvSuspendPracticeScheme.DataSource = udtSchemeBackOfficeModelCollection
        gvSuspendPracticeScheme.DataBind()

        Dim dicRowToScheme As New Dictionary(Of Integer, String)
        Dim aryVisibleScheme As New ArrayList

        For i As Integer = 0 To udtSchemeBackOfficeModelCollection.Count - 1
            dicRowToScheme.Add(i, udtSchemeBackOfficeModelCollection(i).SchemeCode.Trim)
        Next

        Dim dicSPSchemeToStatus As New Dictionary(Of String, String)
        For Each udtScheme As SchemeInformationModel In udtServiceProviderBLL.GetSP.SchemeInfoList.Values
            dicSPSchemeToStatus.Add(udtScheme.SchemeCode, udtScheme.RecordStatus)
        Next

        ' Get the PracticeSchemeInfo the selected row
        Dim r As GridViewRow = CType(sender, ImageButton).NamingContainer
        Dim gvPracticeSchemeInfo As GridView = CType(r.FindControl("gvPracticeSchemeInfo"), GridView)

        For Each row As GridViewRow In gvPracticeSchemeInfo.Rows
            ' CRE16-022 (Add optional field "Remarks") [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            'Dim strSchemeCodeReal As String = CType(row.FindControl("hfPracticeSchemeCodeReal"), HiddenField).Value.Trim
            'Dim strStatusReal As String = CType(row.FindControl("hfPracticeSchemeStatusReal"), HiddenField).Value.Trim
            Dim strSchemeCodeReal As String = CType(row.FindControl("lblPracticeSchemeCode"), Label).Attributes("SchemeCodeReal").Trim
            Dim strStatusReal As String = CType(row.FindControl("lblPracticeSchemeStatus"), Label).Attributes("StatusReal").Trim
            ' CRE16-022 (Add optional field "Remarks") [End][Chris YIM]

            ' If the Scheme is Pending Delist, does not allow any control on the Practice Scheme
            If dicSPSchemeToStatus.ContainsKey(strSchemeCodeReal) _
                    AndAlso (dicSPSchemeToStatus(strSchemeCodeReal) = SchemeInformationMaintenanceDisplayStatus.ActivePendingDelist _
                        OrElse dicSPSchemeToStatus(strSchemeCodeReal) = SchemeInformationMaintenanceDisplayStatus.SuspendedPendingDelist) Then
                Continue For
            End If

            If strStatusReal = PracticeSchemeInfoMaintenanceDisplayStatus.Active AndAlso Not aryVisibleScheme.Contains(strSchemeCodeReal) Then
                aryVisibleScheme.Add(strSchemeCodeReal)
            End If
        Next

        Dim intRowNoVisible As Integer

        For i As Integer = 0 To gvSuspendPracticeScheme.Rows.Count - 1
            If aryVisibleScheme.Contains(dicRowToScheme(i)) Then
                intRowNoVisible = i
            Else
                gvSuspendPracticeScheme.Rows(i).Visible = False
            End If
        Next

        ' Auto-check the checkbox if there is only one
        If aryVisibleScheme.Count = 1 Then
            Dim row As GridViewRow = gvSuspendPracticeScheme.Rows(intRowNoVisible)

            Dim cboSchemeCode As CheckBox = CType(row.FindControl("cboSchemeCode"), CheckBox)
            cboSchemeCode.Checked = True
            cboSchemeCode.Attributes.Add("onclick", "return false;")

            Dim txtRemark As TextBox = CType(row.FindControl("txtRemark"), TextBox)
            txtRemark.Enabled = True
        End If

        ' Bind the grid view
        If udtServiceProviderBLL.Exist Then
            Dim udtNewPractice As Practice.PracticeModel = udtServiceProviderBLL.GetSP.PracticeList.Item(r.RowIndex + 1)
            Session(SESS_SelectedPractice) = udtNewPractice

            Dim udtNewPracticeList As Practice.PracticeModelCollection = New Practice.PracticeModelCollection
            udtNewPracticeList.Add(udtNewPractice)

            Session(SESS_SPMaintenanceHandledScheme) = New ArrayList()

            gvActionPracticeBank.DataSource = udtNewPracticeList.Values
            gvActionPracticeBank.DataBind()

            Session.Remove(SESS_SPMaintenanceHandledScheme)

        End If

        MultiViewMaintenance.ActiveViewIndex = ViewIndexPracticeActionDetails
        panPracticeSuspend.Visible = True

    End Sub

    Protected Sub ibtnPracticeSuspendSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CompleteMsgBox.Visible = False
        msgBox.Visible = False

        Dim intChecked As Integer = 0

        For Each r As GridViewRow In gvSuspendPracticeScheme.Rows
            Dim cboSchemeCode As CheckBox = CType(r.FindControl("cboSchemeCode"), CheckBox)
            Dim txtRemark As TextBox = CType(r.FindControl("txtRemark"), TextBox)
            Dim imgAlertSchemeCode As Image = CType(r.FindControl("imgAlertSchemeCode"), Image)
            Dim imgAlertRemark As Image = CType(r.FindControl("imgAlertRemark"), Image)

            imgAlertSchemeCode.Visible = False
            imgAlertRemark.Visible = False

            If cboSchemeCode.Checked Then
                intChecked += 1

                If udtValidator.IsEmpty(txtRemark.Text.Trim) Then
                    imgAlertRemark.Visible = True
                    msgBox.AddMessage(New SystemMessage(FunctionCode, "E", MsgCode.MSG00007))
                End If
            End If
        Next

        ' Check at least one checkbox checked
        If intChecked = 0 Then
            msgBox.AddMessage(New SystemMessage(FunctionCode, "E", MsgCode.MSG00012))

            For Each r As GridViewRow In gvSuspendScheme.Rows
                CType(r.FindControl("imgAlertSchemeCode"), Image).Visible = True
            Next
        End If

        If msgBox.GetCodeTable.Rows.Count = 0 Then
            msgBox.Visible = False

            If udtServiceProviderBLL.Exist Then
                panPracticeSuspend.Visible = False
                panActionPracticeSuspend.Visible = True

                ' Construct the Suspend Scheme table
                Dim dsSuspend As New DataSet
                Dim dtSuspend As DataTable
                Dim drSuspend As DataRow

                dsSuspend.Tables.Add(New DataTable("TempTable"))
                dtSuspend = dsSuspend.Tables.Item("TempTable")
                dtSuspend.Columns.Add(Me.GetGlobalResourceObject("Text", "SchemeName"))
                dtSuspend.Columns.Add(Me.GetGlobalResourceObject("Text", "Remarks"))
                dtSuspend.Columns.Add("SchemeNameReal")

                Const intSchemeCodeColumn As Integer = 0
                Const intRemarkColumn As Integer = 1
                Const intSchemeCodeRealColumn As Integer = 2

                For Each r As GridViewRow In gvSuspendPracticeScheme.Rows
                    Dim cboSchemeCode As CheckBox = CType(r.FindControl("cboSchemeCode"), CheckBox)

                    If cboSchemeCode.Checked Then
                        drSuspend = dsSuspend.Tables.Item("TempTable").NewRow()
                        drSuspend.Item(intSchemeCodeColumn) = cboSchemeCode.Text
                        drSuspend.Item(intRemarkColumn) = CType(r.FindControl("txtRemark"), TextBox).Text
                        ' CRE16-022 (Add optional field "Remarks") [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                        'drSuspend.Item(intSchemeCodeRealColumn) = CType(r.FindControl("hfSchemeCodeReal"), HiddenField).Value
                        drSuspend.Item(intSchemeCodeRealColumn) = CType(r.FindControl("cboSchemeCode"), CheckBox).Attributes("SchemeCodePracticeSuspend")
                        ' CRE16-022 (Add optional field "Remarks") [End][Chris YIM]
                        dsSuspend.Tables.Item("TempTable").Rows.Add(drSuspend)
                    End If
                Next

                gvActionPSuspendScheme.DataSource = dsSuspend
                gvActionPSuspendScheme.DataBind()

                ' Hide the SchemeRealName column
                gvActionPSuspendScheme.HeaderRow.Cells(intSchemeCodeRealColumn).Visible = False
                For Each r As GridViewRow In gvActionPSuspendScheme.Rows
                    r.Cells(2).Visible = False
                Next

                ' Control the width of the Remarks column
                gvActionPSuspendScheme.Rows(0).Cells(intRemarkColumn).Width = 250

                ' Build the complete message box
                CompleteMsgBox.AddMessage(FunctionCode, "I", MsgCode.MSG00002)
                CompleteMsgBox.BuildMessageBox()
                CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information

            End If
        Else
            msgBox.BuildMessageBox("ValidationFail")

        End If

    End Sub

    Protected Sub ibtnPracticeSuspendCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        msgBox.Visible = False
        CompleteMsgBox.Visible = False
        MultiViewMaintenance.ActiveViewIndex = ViewIndexDetails
        panPracticeSuspend.Visible = False
        Session.Remove(SESS_SelectedPractice)
    End Sub

    Protected Sub ibtnActionPracitceSuspendConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CompleteMsgBox.Visible = False

        If udtServiceProviderBLL.Exist Then
            Dim udtHCVUUser As HCVUUserModel = udtHCVUUserBLL.GetHCVUUser
            Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP
            Dim udtPractice As PracticeModel = CType(Session(SESS_SelectedPractice), PracticeModel)

            Dim i As Integer = 0
            Dim udtAuditLogEntryCollection(gvActionPSuspendScheme.Rows.Count) As AuditLogEntry

            Dim udtDB As New Database
            udtDB.BeginTransaction()

            Try
                Dim blnSuccess As Boolean = True

                ' Write Audit Log
                For Each r As GridViewRow In gvActionPSuspendScheme.Rows
                    Dim strRemark As String = r.Cells(1).Text
                    Dim strSchemeCodeReal As String = r.Cells(2).Text

                    udtAuditLogEntryCollection(i) = New AuditLogEntry(FunctionCode, Me)
                    udtAuditLogEntryCollection(i).AddDescripton("ERN", udtSP.EnrolRefNo)
                    udtAuditLogEntryCollection(i).AddDescripton("SPID", udtSP.SPID)
                    udtAuditLogEntryCollection(i).AddDescripton("PracticeSeq", udtPractice.DisplaySeq)
                    udtAuditLogEntryCollection(i).AddDescripton("Scheme", strSchemeCodeReal)
                    udtAuditLogEntryCollection(i).AddDescripton("Remark", strRemark)
                    udtAuditLogEntryCollection(i).WriteStartLog(LogID.LOG00032, "Suspend Practice Scheme confirms")
                    i = i + 1

                    Dim udtNewPracticeList As New PracticeModelCollection
                    udtNewPracticeList.Add(udtPractice)

                    If Not udtAccountChangeMaintenanceBLL.AddSPAccMaintenanceByPracticeList(udtSP.SPID, udtNewPracticeList, strRemark, String.Empty, _
                                                                                                udtHCVUUser.UserID.Trim, strSchemeCodeReal, udtDB) Then
                        blnSuccess = False
                    End If

                Next

                Session.Remove(SESS_SelectedPractice)

                If blnSuccess Then
                    udtDB.CommitTransaction()

                    Dim strOld As String() = {"%s"}
                    Dim strNew As String() = {""}

                    strNew(0) = udtSP.EnglishName + udtFormatter.formatChineseName(udtSP.ChineseName) + " [" + udtSP.SPID + "] "

                    CompleteMsgBox.AddMessage(FunctionCode, "I", MsgCode.MSG00001, strOld, strNew)
                    CompleteMsgBox.BuildMessageBox()
                    CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Complete

                    MultiViewMaintenance.ActiveViewIndex = ViewIndexComplete
                    panActionPracticeSuspend.Visible = False

                    ' End the log
                    For j As Integer = 0 To i - 1
                        udtAuditLogEntryCollection(j).WriteEndLog(LogID.LOG00033, "Suspend Practice Scheme successful")
                    Next

                Else
                    udtDB.RollBackTranscation()

                End If

            Catch eSQL As SqlClient.SqlException
                udtDB.RollBackTranscation()

                If eSQL.Number = 50000 Then
                    msgBox.AddMessage(New SystemMessage("990001", "D", eSQL.Message))

                    If msgBox.GetCodeTable.Rows.Count = 0 Then
                        msgBox.Visible = False
                    Else
                        msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00034, "Suspend Practice Scheme failed")
                    End If

                    For j As Integer = 0 To i - 1
                        udtAuditLogEntryCollection(j).WriteEndLog(LogID.LOG00034, "Suspend Practice Scheme failed")
                    Next

                Else
                    Throw eSQL
                End If

            Catch ex As Exception
                udtDB.RollBackTranscation()

                For j As Integer = 0 To i - 1
                    udtAuditLogEntryCollection(j).WriteEndLog(LogID.LOG00034, "Suspend Practice Scheme failed")
                Next

                Throw ex
            End Try
        Else
            Session.Remove(SESS_SelectedPractice)
        End If
    End Sub

    Protected Sub ibtnActionPracitceSuspendBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CompleteMsgBox.Visible = False
        panActionPracticeSuspend.Visible = False
        panPracticeSuspend.Visible = True
    End Sub

    ' Reactivate Practice

    Protected Sub ibtnSPracticeReactivate_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00058, "Reactivate Practice Scheme starts")

        CompleteMsgBox.Visible = False
        msgBox.Visible = False

        ' Change the Heading Text -> Reactivate Practice Scheme
        lblActionPracticeHeading.Text = Me.GetGlobalResourceObject("Text", "ReactivatePracticeScheme")

        Dim udtSchemeBackOfficeModelCollection As SchemeBackOfficeModelCollection = udtSchemeBackOfficeBLL.GetAllEffectiveSchemeBackOfficeWithSubsidizeGroupFromCache()
        gvReactivatePracticeScheme.DataSource = udtSchemeBackOfficeModelCollection
        gvReactivatePracticeScheme.DataBind()

        Dim dicRowToScheme As New Dictionary(Of Integer, String)
        Dim aryVisibleScheme As New ArrayList

        For i As Integer = 0 To udtSchemeBackOfficeModelCollection.Count - 1
            dicRowToScheme.Add(i, udtSchemeBackOfficeModelCollection(i).SchemeCode.Trim)
        Next

        Dim dicSPSchemeToStatus As New Dictionary(Of String, String)
        For Each udtScheme As SchemeInformationModel In udtServiceProviderBLL.GetSP.SchemeInfoList.Values
            dicSPSchemeToStatus.Add(udtScheme.SchemeCode, udtScheme.RecordStatus)
        Next

        ' Get the PracticeSchemeInfo the selected row
        Dim r As GridViewRow = CType(sender, ImageButton).NamingContainer
        Dim gvPracticeSchemeInfo As GridView = CType(r.FindControl("gvPracticeSchemeInfo"), GridView)

        For Each row As GridViewRow In gvPracticeSchemeInfo.Rows
            ' CRE16-022 (Add optional field "Remarks") [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            'Dim strSchemeCodeReal As String = CType(row.FindControl("hfPracticeSchemeCodeReal"), HiddenField).Value.Trim
            'Dim strStatusReal As String = CType(row.FindControl("hfPracticeSchemeStatusReal"), HiddenField).Value.Trim
            Dim strSchemeCodeReal As String = CType(row.FindControl("lblPracticeSchemeCode"), Label).Attributes("SchemeCodeReal").Trim
            Dim strStatusReal As String = CType(row.FindControl("lblPracticeSchemeStatus"), Label).Attributes("StatusReal").Trim
            ' CRE16-022 (Add optional field "Remarks") [End][Chris YIM]

            ' If the Scheme is Pending Delist, does not allow any control on the Practice Scheme
            If dicSPSchemeToStatus.ContainsKey(strSchemeCodeReal) _
                    AndAlso (dicSPSchemeToStatus(strSchemeCodeReal) = SchemeInformationMaintenanceDisplayStatus.ActivePendingDelist _
                        OrElse dicSPSchemeToStatus(strSchemeCodeReal) = SchemeInformationMaintenanceDisplayStatus.SuspendedPendingDelist) Then
                Continue For
            End If

            If strStatusReal = PracticeSchemeInfoMaintenanceDisplayStatus.Suspended AndAlso Not aryVisibleScheme.Contains(strSchemeCodeReal) Then
                aryVisibleScheme.Add(strSchemeCodeReal)
            End If
        Next

        Dim intRowNoVisible As Integer

        For i As Integer = 0 To gvReactivatePracticeScheme.Rows.Count - 1
            If aryVisibleScheme.Contains(dicRowToScheme(i)) Then
                intRowNoVisible = i
            Else
                gvReactivatePracticeScheme.Rows(i).Visible = False
            End If
        Next

        ' Auto-check the checkbox if there is only one
        If aryVisibleScheme.Count = 1 Then
            Dim row As GridViewRow = gvReactivatePracticeScheme.Rows(intRowNoVisible)

            Dim cboSchemeCode As CheckBox = CType(row.FindControl("cboSchemeCode"), CheckBox)
            cboSchemeCode.Checked = True
            cboSchemeCode.Attributes.Add("onclick", "return false;")
        End If

        ' Bind the grid view
        If udtServiceProviderBLL.Exist Then
            Dim udtNewPractice As PracticeModel = udtServiceProviderBLL.GetSP.PracticeList.Item(r.RowIndex + 1)
            Session(SESS_SelectedPractice) = udtNewPractice

            Dim udtNewPracticeList As New PracticeModelCollection
            udtNewPracticeList.Add(udtNewPractice)

            Session(SESS_SPMaintenanceHandledScheme) = New ArrayList()

            gvActionPracticeBank.DataSource = udtNewPracticeList.Values
            gvActionPracticeBank.DataBind()

            Session.Remove(SESS_SPMaintenanceHandledScheme)

        End If

        MultiViewMaintenance.ActiveViewIndex = ViewIndexPracticeActionDetails
        panPracticeReactivate.Visible = True

    End Sub

    Protected Sub ibtnPracticeReactivateSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CompleteMsgBox.Visible = False
        msgBox.Visible = False

        Dim intChecked As Integer = 0

        For Each r As GridViewRow In gvReactivatePracticeScheme.Rows
            CType(r.FindControl("imgAlertSchemeCode"), Image).Visible = False

            If CType(r.FindControl("cboSchemeCode"), CheckBox).Checked Then
                intChecked += 1
            End If
        Next

        If intChecked = 0 Then
            msgBox.AddMessage(New SystemMessage(FunctionCode, "E", MsgCode.MSG00013))

            For Each r As GridViewRow In gvReactivateScheme.Rows
                CType(r.FindControl("imgAlertSchemeCode"), Image).Visible = True
            Next
        End If

        If msgBox.GetCodeTable.Rows.Count = 0 Then
            panPracticeReactivate.Visible = False
            panActionPracticeReactivate.Visible = True

            ' Construct the Reactivate Scheme string
            lblActionPReactivateScheme.Text = String.Empty
            hfActionPReactivateScheme.Value = String.Empty

            ' CRE16-022 (Add optional field "Remarks") [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            For Each r As GridViewRow In gvReactivatePracticeScheme.Rows
                Dim cboSchemeCode As CheckBox = CType(r.FindControl("cboSchemeCode"), CheckBox)

                'Dim hfSchemeCodeReal As HiddenField = CType(r.FindControl("hfSchemeCodeReal"), HiddenField)
                Dim strSchemeCodeReal As String = CType(r.FindControl("cboSchemeCode"), CheckBox).Attributes("SchemeCodePracticeReactivate")

                If cboSchemeCode.Checked Then
                    lblActionPReactivateScheme.Text += ", " + cboSchemeCode.Text

                    'hfActionPReactivateScheme.Value += ", " + AntiXssEncoder.HtmlEncode(hfSchemeCodeReal.Value, True)
                    hfActionPReactivateScheme.Value += ", " + AntiXssEncoder.HtmlEncode(strSchemeCodeReal, True)

                End If
            Next
            ' CRE16-022 (Add optional field "Remarks") [End][Chris YIM]

            lblActionPReactivateScheme.Text = lblActionPReactivateScheme.Text.Substring(2)
            ' I-CRE16-003 Fix XSS [Start][Lawrence]
            hfActionPReactivateScheme.Value = AntiXssEncoder.HtmlEncode(hfActionPReactivateScheme.Value.Substring(2), True)
            ' I-CRE16-003 Fix XSS [End][Lawrence]

            ' Build the complete message box
            CompleteMsgBox.AddMessage(FunctionCode, "I", MsgCode.MSG00004)
            CompleteMsgBox.BuildMessageBox()
            CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information

        Else
            msgBox.BuildMessageBox("ValidationFail")

        End If

    End Sub

    Protected Sub ibtnPracticeReactivateCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        msgBox.Visible = False
        CompleteMsgBox.Visible = False
        MultiViewMaintenance.ActiveViewIndex = ViewIndexDetails
        panPracticeReactivate.Visible = False
        Session.Remove(SESS_SelectedPractice)
    End Sub

    Protected Sub ibtnActionPracticeReactivateConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CompleteMsgBox.Visible = False

        If udtServiceProviderBLL.Exist Then
            Dim udtHCVUUser As HCVUUserModel = udtHCVUUserBLL.GetHCVUUser
            Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP
            Dim udtPractice As PracticeModel = CType(Session(SESS_SelectedPractice), PracticeModel)

            Dim aryScheme As String() = hfActionPReactivateScheme.Value.Split(",")

            Dim i As Integer = 0
            Dim udtAuditLogEntryCollection(aryScheme.Length) As AuditLogEntry

            Dim udtDB As New Database
            udtDB.BeginTransaction()

            Try
                Dim blnSuccess As Boolean = True

                ' Write Audit Log
                For Each strScheme As String In aryScheme
                    strScheme = strScheme.Trim

                    udtAuditLogEntryCollection(i) = New AuditLogEntry(FunctionCode, Me)
                    udtAuditLogEntryCollection(i).AddDescripton("ERN", udtSP.EnrolRefNo)
                    udtAuditLogEntryCollection(i).AddDescripton("SPID", udtSP.SPID)
                    udtAuditLogEntryCollection(i).AddDescripton("PracticeSeq", udtPractice.DisplaySeq)
                    udtAuditLogEntryCollection(i).AddDescripton("Scheme", strScheme)
                    udtAuditLogEntryCollection(i).WriteStartLog(LogID.LOG00038, "Reactivate Practice Scheme confirms")
                    i = i + 1

                    Dim udtNewPracticeList As New PracticeModelCollection
                    udtNewPracticeList.Add(udtPractice)

                    If Not udtAccountChangeMaintenanceBLL.AddSPAccMaintenanceByPracticeList(udtSP.SPID, udtNewPracticeList, String.Empty, _
                                                                                            String.Empty, udtHCVUUser.UserID.Trim, strScheme, udtDB) Then
                        blnSuccess = False
                    End If

                Next

                Session.Remove(SESS_SelectedPractice)

                If blnSuccess Then
                    udtDB.CommitTransaction()

                    Dim strOld As String() = {"%s"}
                    Dim strNew As String() = {""}

                    strNew(0) = udtSP.EnglishName + udtFormatter.formatChineseName(udtSP.ChineseName) + " [" + udtSP.SPID + "] "

                    CompleteMsgBox.AddMessage(FunctionCode, "I", MsgCode.MSG00001, strOld, strNew)
                    CompleteMsgBox.BuildMessageBox()
                    CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Complete

                    MultiViewMaintenance.ActiveViewIndex = ViewIndexComplete
                    panActionPracticeReactivate.Visible = False

                    ' End the log
                    For j As Integer = 0 To i - 1
                        udtAuditLogEntryCollection(j).WriteEndLog(LogID.LOG00039, "Reactivate Practice Scheme successful")
                    Next

                Else
                    udtDB.RollBackTranscation()

                End If

            Catch eSQL As SqlClient.SqlException
                If eSQL.Number = 50000 Then
                    msgBox.AddMessage(New SystemMessage("990001", "D", eSQL.Message))

                    If msgBox.GetCodeTable.Rows.Count = 0 Then
                        msgBox.Visible = False
                    Else
                        msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00040, "Reactivate Practice Scheme failed")
                    End If

                    For j As Integer = 0 To i - 1
                        udtAuditLogEntryCollection(j).WriteEndLog(LogID.LOG00040, "Reactivate Practice Scheme failed")
                    Next

                Else
                    Throw eSQL
                End If

            Catch ex As Exception
                udtDB.RollBackTranscation()

                For j As Integer = 0 To i - 1
                    udtAuditLogEntryCollection(j).WriteEndLog(LogID.LOG00040, "Reactivate Practice Scheme failed")
                Next

                Throw ex
            End Try
        Else
            Session.Remove(SESS_SelectedPractice)
        End If

    End Sub

    Protected Sub ibtnActionPracticeReactivateBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CompleteMsgBox.Visible = False
        panActionPracticeReactivate.Visible = False
        panPracticeReactivate.Visible = True
    End Sub

#End Region

    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
#Region "Abstract Method of [HCVU.BasePageWithControl]"
    Protected Overrides Sub SF_InitSearch(ByRef udtAuditLogEntry As AuditLogEntry)
    End Sub

    Protected Overrides Function SF_ValidateSearch(ByRef udtAuditLogEntry As AuditLogEntry) As Boolean
        Return True
    End Function

    Protected Overrides Function SF_Search(ByRef udtAuditLogEntry As AuditLogEntry, ByVal blnOverrideResultLimit As Boolean) As BaseBLL.BLLSearchResult
        ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
        'Return udtAccountChangeMaintenanceBLL.MaintenanceSearch(FunctionCode, _strERN, txtSPID.Text.Trim, _
        '                                                        udtFormatter.formatHKIDInternal(txtSPHKID.Text.Trim), txtSPName.Text.Trim, txtPhone.Text.Trim, _
        '                                                        ddlSPHealthProf.SelectedValue.Trim, ddlScheme.SelectedValue.Trim, blnOverrideResultLimit)
        Return udtAccountChangeMaintenanceBLL.MaintenanceSearch(FunctionCode, _strERN, txtSPID.Text.Trim, _
                                                                udtFormatter.formatHKIDInternal(txtSPHKID.Text.Trim), txtSPName.Text.Trim, txtSPChiName.Text.Trim, txtPhone.Text.Trim, _
                                                                ddlSPHealthProf.SelectedValue.Trim, ddlScheme.SelectedValue.Trim, blnOverrideResultLimit)
        ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]
    End Function

    Protected Overrides Function SF_BindSearchResult(ByRef udtAuditLogEntry As AuditLogEntry, ByVal udtBLLSearchResult As BaseBLL.BLLSearchResult) As Integer
        Dim dt As DataTable
        Dim intRowCount As Integer

        Try
            dt = CType(udtBLLSearchResult.Data, DataTable)

        Catch ex As Exception
            Throw

        End Try

        intRowCount = dt.Rows.Count

        If intRowCount = 0 Then
            ' No record found

        ElseIf intRowCount = 1 AndAlso (txtEnrolRefNo.Text.Trim <> String.Empty OrElse txtSPID.Text.Trim <> String.Empty OrElse txtSPHKID.Text.Trim <> String.Empty) Then
            ' 1 record found, with key fields search
            hfSPID.Value = CStr(dt.Rows(0)("SP_ID")).Trim
            btnSpDetails_Click(Nothing, Nothing)
            Session.Remove(SESS_SearchResultList)

        Else
            If txtEnrolRefNo.Text.Trim.Equals(String.Empty) Then
                lblResultERN.Text = Me.GetGlobalResourceObject("Text", "Any")
            Else
                lblResultERN.Text = txtEnrolRefNo.Text.Trim
            End If

            If txtSPID.Text.Trim.Equals(String.Empty) Then
                lblResultSPID.Text = Me.GetGlobalResourceObject("Text", "Any")
            Else
                lblResultSPID.Text = txtSPID.Text.Trim
            End If

            If txtSPHKID.Text.Trim.Equals(String.Empty) Then
                lblResultSPHKID.Text = Me.GetGlobalResourceObject("Text", "Any")
            Else
                lblResultSPHKID.Text = txtSPHKID.Text.Trim
            End If

            If txtSPName.Text.Trim.Equals(String.Empty) Then
                lblResultSPName.Text = Me.GetGlobalResourceObject("Text", "Any")
            Else
                lblResultSPName.Text = txtSPName.Text.Trim
            End If

            ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
            If txtSPChiName.Text.Trim.Equals(String.Empty) Then
                lblResultSPChiName.Text = Me.GetGlobalResourceObject("Text", "Any")
            Else
                lblResultSPChiName.Text = txtSPChiName.Text.Trim
            End If
            ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]

            If txtPhone.Text.Trim.Equals(String.Empty) Then
                lblResultPhone.Text = Me.GetGlobalResourceObject("Text", "Any")
            Else
                lblResultPhone.Text = txtPhone.Text.Trim
            End If

            If ddlSPHealthProf.SelectedValue.Trim.Equals(String.Empty) Then
                lblResultHealthProf.Text = Me.GetGlobalResourceObject("Text", "Any")
            Else
                lblResultHealthProf.Text = GetHealthProfName(ddlSPHealthProf.SelectedValue)
            End If

            If ddlScheme.SelectedValue.Trim.Equals(String.Empty) Then
                lblResultScheme.Text = Me.GetGlobalResourceObject("Text", "Any")
            Else
                lblResultScheme.Text = ddlScheme.SelectedItem.Text.Trim
            End If

            gvResult.DataSource = dt
            gvResult.DataBind()

            Session(SESS_SearchResultList) = dt
            GridViewDataBind(gvResult, dt, "Enrolment_Ref_No", "ASC", False)

            MultiViewMaintenance.ActiveViewIndex = ViewIndexSearchResult
        End If

        Return intRowCount
    End Function

    Protected Overrides Sub SF_FinalizeSearch(ByRef udtAuditLogEntry As AuditLogEntry)
    End Sub

    Protected Overrides Sub SF_ConfirmSearch_Click()
        ibtnSearch_Click(Nothing, Nothing)
    End Sub

    Protected Overrides Sub SF_CancelSearch_Click()
    End Sub
#End Region
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

    Protected Sub ibtnSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CompleteMsgBox.Visible = False
        msgBox.Visible = False

        ' CRE12-015 Add the respective practice number in “Practice” in the functions under “Reimbursement” in eHS [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------------------------------------------
        ' Implement Collapsible Search Criteria Review
        udcCollapsibleSearchCriteriaReview.Collapsed = True
        udcCollapsibleSearchCriteriaReview.ClientState = "True"
        ' CRE12-015 Add the respective practice number in “Practice” in the functions under “Reimbursement” in eHS [End][Tommy L]

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
        ' -------------------------------------------------------------------------
        'Dim dt As New DataTable
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

        Try
            _strERN = String.Empty

            ' I-CRE16-003 Fix XSS [Start][Lawrence]
            txtEnrolRefNo.Text = UCase(AntiXssEncoder.HtmlEncode(txtEnrolRefNo.Text, True))
            ' I-CRE16-003 Fix XSS [End][Lawrence]

            If Not txtEnrolRefNo.Text.Trim.Equals(String.Empty) Then
                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
                ' -------------------------------------------------------------------------
                If udtValidator.chkSystemNumber(txtEnrolRefNo.Text.Trim) Then
                    'strERN = Formatter.ReverseSystemNumber(txtEnrolRefNo.Text.Trim)
                    _strERN = Formatter.ReverseSystemNumber(txtEnrolRefNo.Text.Trim)
                Else
                    'strERN = txtEnrolRefNo.Text.Trim
                    _strERN = txtEnrolRefNo.Text.Trim
                End If
                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
            End If

            'Write Audit Log
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
            ' -------------------------------------------------------------------------
            'udtAuditLogEntry.AddDescripton("ERN", strERN)
            udtAuditLogEntry.AddDescripton("ERN", _strERN)
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
            udtAuditLogEntry.AddDescripton("SPID", txtSPID.Text.Trim)
            udtAuditLogEntry.AddDescripton("SP HKID", txtSPHKID.Text.Trim)
            udtAuditLogEntry.AddDescripton("SP Name", txtSPName.Text.Trim)
            ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
            udtAuditLogEntry.AddDescripton("SP ChiName", txtSPName.Text.Trim)
            ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]
            udtAuditLogEntry.AddDescripton("Phone", txtPhone.Text.Trim)
            udtAuditLogEntry.AddDescripton("Profession", ddlSPHealthProf.SelectedValue.Trim)
            udtAuditLogEntry.AddDescripton("Scheme", ddlScheme.SelectedValue.Trim)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Search", New AuditLogInfo(txtSPID.Text.Trim, txtSPHKID.Text.Trim, "", "", "", ""))

            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
            ' -------------------------------------------------------------------------
            Dim enumSearchResult As SearchResultEnum

            If IsNothing(sender) Then
                enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, msgBox, CompleteMsgBox, False, True)
            Else
                enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, msgBox, CompleteMsgBox)
            End If

            Select Case enumSearchResult
                Case SearchResultEnum.Success
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search completed")

                Case SearchResultEnum.ValidationFail
                    ' No Validation
                    Throw New Exception("Error: Class = [HCVU.spMaintenance], Method = [ibtnSearch_Click], Message = The method - [SF_ValidateSearch] should not return [False]")

                Case SearchResultEnum.NoRecordFound
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search completed. No record found")

                Case SearchResultEnum.OverResultList1stLimit_PopUp
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00004, "Search failed")

                Case SearchResultEnum.OverResultList1stLimit_Alert
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00004, "Search failed")

                Case SearchResultEnum.OverResultListOverrideLimit
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00004, "Search failed")

                Case Else
                    Throw New Exception("Error: Class = [HCVU.spMaintenance], Method = [ibtnSearch_Click], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")

            End Select

            'dt = udtAccountChangeMaintenanceBLL.MaintenanceSearch(strERN, txtSPID.Text.Trim, _
            '        udtFormatter.formatHKIDInternal(txtSPHKID.Text.Trim), txtSPName.Text.Trim, txtPhone.Text.Trim, _
            '        ddlSPHealthProf.SelectedValue.Trim, ddlScheme.SelectedValue.Trim)

            'If dt.Rows.Count = 0 Then
            ' No record found
            'CompleteMsgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, "I", MsgCode.MSG00001))
            'CompleteMsgBox.BuildMessageBox()
            'CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information

            'udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search completed. No record found")

            'ElseIf dt.Rows.Count = 1 AndAlso (txtEnrolRefNo.Text.Trim <> String.Empty OrElse txtSPID.Text.Trim <> String.Empty OrElse txtSPHKID.Text.Trim <> String.Empty) Then
            ' 1 record found, with key fields search
            'hfSPID.Value = CStr(dt.Rows(0)("SP_ID")).Trim
            'btnSpDetails_Click(Nothing, Nothing)
            'Session.Remove(SESS_SearchResultList)

            'Else
            'If txtEnrolRefNo.Text.Trim.Equals(String.Empty) Then
            'lblResultERN.Text = Me.GetGlobalResourceObject("Text", "Any")
            'Else
            'lblResultERN.Text = txtEnrolRefNo.Text.Trim
            'End If

            'If txtSPID.Text.Trim.Equals(String.Empty) Then
            'lblResultSPID.Text = Me.GetGlobalResourceObject("Text", "Any")
            'Else
            'lblResultSPID.Text = txtSPID.Text.Trim
            'End If

            'If txtSPHKID.Text.Trim.Equals(String.Empty) Then
            'lblResultSPHKID.Text = Me.GetGlobalResourceObject("Text", "Any")
            'Else
            'lblResultSPHKID.Text = txtSPHKID.Text.Trim
            'End If

            'If txtSPName.Text.Trim.Equals(String.Empty) Then
            'lblResultSPName.Text = Me.GetGlobalResourceObject("Text", "Any")
            'Else
            'lblResultSPName.Text = txtSPName.Text.Trim
            'End If

            'If txtPhone.Text.Trim.Equals(String.Empty) Then
            'lblResultPhone.Text = Me.GetGlobalResourceObject("Text", "Any")
            'Else
            'lblResultPhone.Text = txtPhone.Text.Trim
            'End If

            'If ddlSPHealthProf.SelectedValue.Trim.Equals(String.Empty) Then
            'lblResultHealthProf.Text = Me.GetGlobalResourceObject("Text", "Any")
            'Else
            'lblResultHealthProf.Text = GetHealthProfName(ddlSPHealthProf.SelectedValue)
            'End If

            'If ddlScheme.SelectedValue.Trim.Equals(String.Empty) Then
            'lblResultScheme.Text = Me.GetGlobalResourceObject("Text", "Any")
            'Else
            'lblResultScheme.Text = ddlScheme.SelectedItem.Text.Trim
            'End If

            'gvResult.DataSource = dt
            'gvResult.DataBind()

            'Session(SESS_SearchResultList) = dt
            'GridViewDataBind(gvResult, dt, "Enrolment_Ref_No", "ASC", False)

            'MultiViewMaintenance.ActiveViewIndex = ViewIndexSearchResult

            'udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search completed")
            'End If

        Catch eSQL As SqlClient.SqlException
            'If eSQL.Number = 50000 Then
            'msgBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, eSQL.Message))

            'If msgBox.GetCodeTable.Rows.Count = 0 Then
            'msgBox.Visible = False
            'Else
            'msgBox.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00004, "Search failed")
            'End If

            'Else
            Throw eSQL
            'End If
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub btnSpDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If hfSPID.Value.Trim <> String.Empty Then
            Dim udtSP As ServiceProviderModel = udtSPProfileBLL.GetServiceProviderPermanentProfileWithMaintenance(hfSPID.Value.Trim)

            ' Write Audit Log
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.AddDescripton("SPID", hfSPID.Value.Trim)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00005, "Select")

            If IsNothing(udtSP) Then
                msgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
                msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00041, "Select failed: ServiceProviderModel object not found")

                MultiViewMaintenance.ActiveViewIndex = ViewIndexError

                Return
            End If

            BindSPDetails(udtSP)

            panActionBtn.Visible = True
            panInputDtm.Visible = False
            panReactivate.Visible = False
            panDelisting.Visible = False
            panSuspend.Visible = False

            MultiViewMaintenance.ActiveViewIndex = ViewIndexDetails

            udtAuditLogEntry.WriteEndLog(LogID.LOG00006, "Select completed")
        End If

    End Sub

    Private Sub BindSPDetails(ByVal udtSP As ServiceProviderModel)
        Dim dt As DataTable 'CRE20-006 DHC integration [Nichole]

        ' --------------- Personal Particulars ---------------

        ' Enrolment Reference No.
        lblERN.Text = udtFormatter.formatSystemNumber(udtSP.EnrolRefNo)
        hfERN.Value = udtSP.EnrolRefNo

        ' Service Provider ID
        hfUnderModify.Value = udtSP.UnderModification
        lblDetailsSPID.Text = udtSP.SPID + IIf(hfUnderModify.Value.Trim <> String.Empty, " (Under Amendment)", "")

        If udtSP.AliasAccount = String.Empty Then
            lblSPUsernameText.Visible = False
            lblSPUsername.Visible = False
        Else
            lblSPUsernameText.Visible = True
            lblSPUsername.Visible = True
            lblSPUsername.Text = udtSP.AliasAccount
        End If

        ' Effective Date
        lblEffectivateDate.Text = udtFormatter.convertDateTime(udtSP.EffectiveDtm)

        ' Name
        lblEname.Text = udtSP.EnglishName
        lblCname.Text = udtFormatter.formatChineseName(udtSP.ChineseName)

        ' HKIC No.
        lblHKID.Text = udtFormatter.formatHKID(udtSP.HKID, False)

        ' Corresponding Address
        lblAddress.Text = udtFormatter.formatAddress(udtSP.SpAddress.Room, udtSP.SpAddress.Floor, udtSP.SpAddress.Block, udtSP.SpAddress.Building, _
                                                        udtSP.SpAddress.District, udtSP.SpAddress.AreaCode)

        ' CRE16-018 Display SP tentative email in HCVU [Start][Winnie]
        ' Email Address
        lblEmail.Text = udtSP.Email

        ' Pending Email Address
        If udtSP.EmailChanged = EmailChanged.Changed Then
            trPendingEmail.Visible = True
            lblPendingEmail.Text = udtSP.TentativeEmail
            imgEditEmail.Visible = True

        Else
            trPendingEmail.Visible = False
            lblPendingEmail.Text = String.Empty
            imgEditEmail.Visible = False
        End If
        ' CRE16-018 Display SP tentative email in HCVU [End][Winnie]

        ' Daytime Contact Phone No.
        lblContactNo.Text = udtSP.Phone

        ' Fax No.
        If udtSP.Fax.Equals(String.Empty) Then
            lblFax.Text = Me.GetGlobalResourceObject("Text", "N/A")
        Else
            lblFax.Text = udtSP.Fax
        End If

        ' Service Provider Status
        hfRecordStatus.Value = udtSP.RecordStatus
        Status.GetDescriptionFromDBCode(ServiceProviderStatus.ClassCode, udtSP.RecordStatus, lblRecordStatus.Text, String.Empty)

        ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        ' Account Status
        Dim dtUserAC As DataTable = udtSPProfileBLL.GetHCSPUserACStatus(udtSP.SPID, New Database)
        hfAccountStatus.Value = dtUserAC.Rows(0).Item("UserAcc_RecordStatus").ToString.Trim
        'hfAccountStatus.Value = udtSPProfileBLL.GetHCSPUserACStatus(udtSP.SPID, New Database).Rows(0).Item("UserAcc_RecordStatus").ToString.Trim

        ' Handle IVRS account locked status
        If dtUserAC.Rows(0).Item("UserAcc_IVRS_Locked").ToString.Trim = YesNo.Yes Then
            hfAccountStatus.Value = SPAccountStatus.Suspended
        End If
        ' CRE16-004 (Enable SP to unlock account) [End][Winnie]

        Status.GetDescriptionFromDBCode(SPAccountStatus.ClassCode, hfAccountStatus.Value, lblAccountStatus.Text, String.Empty)

        ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [Start][Tommy]

        ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [Start] (Marco) ---
        'PCD Status
        UpdatePCDStatusLabel(udtSP)
        ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [End]   (Marco) ---

        'CRE20-006 DHC Integration [Start][Nichole]
        DisplayEnrolledDHCStatus(udtSP.SPID)
        dt = udtServiceProviderBLL.GetPracticeEnrolledDHC(udtSP.SPID)
        Session(SESS_SPMaintenancePracticeEnrolledDHCList) = dt
        'CRE20-006 DHC Integration [End][Nichole]

        Dim udtTokenModel As TokenModel = udtSPProfileBLL.GetTokenModelBySPID(udtSP.SPID, False)
        'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        imgShareToken.ToolTip = String.Empty
        imgShareToken.Visible = False
        imgTokenActivateDate.ToolTip = String.Empty
        imgTokenActivateDate.Visible = False
        'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
        If Not IsNothing(udtTokenModel) Then
            Session(SESS_MaintenanceTokenSerialNo) = udtTokenModel.TokenSerialNo
            'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'lblTokenSN.Text = TokenModel.DisplayTokenSerialNo(udtTokenModel.TokenSerialNo, udtTokenModel.Project)

            '' CRE13-003 - Token Replacement [Start][Tommy L]
            '' -------------------------------------------------------------------------------------
            'If udtTokenModel.Project = TokenProjectType.EHCVS Then
            '    lblTokenIssueDate.Text = " (" + udtFormatter.convertDateTime(udtTokenModel.IssueDtm.Value) + ")"
            'Else
            '    lblTokenIssueDate.Text = ""
            'End If
            '' CRE13-003 - Token Replacement [End][Tommy L]

            If udtTokenModel.Project = TokenProjectType.EHCVS Then
                lblTokenSN.Text = TokenModel.DisplayTokenSerialNo(udtTokenModel.TokenSerialNo, udtTokenModel.Project, False, False, True)
                imgTokenActivateDate.ToolTip = HttpContext.GetGlobalResourceObject("AlternateText", "TokenActivateDate").ToString.Replace("%s", udtFormatter.convertDateTime(udtTokenModel.IssueDtm.Value).ToString)
                imgTokenActivateDate.Style.Add("vertical-align", "text-top")
                imgTokenActivateDate.Visible = True
            Else
                lblTokenSN.Text = TokenModel.DisplayTokenSerialNo(udtTokenModel.TokenSerialNo, udtTokenModel.Project, False, True, True)
                imgTokenActivateDate.Visible = False
            End If

            If udtTokenModel.IsShareToken Then
                imgShareToken.ToolTip = HttpContext.GetGlobalResourceObject("AlternateText", "ShareToken").ToString
                imgShareToken.Style.Add("vertical-align", "text-top")
                imgShareToken.Visible = True
            Else
                imgShareToken.Visible = False
            End If
            'CRE14-002 - PPI-ePR Migration [End][Chris YIM]

        Else
            Session(SESS_MaintenanceTokenSerialNo) = Nothing
            lblTokenSN.Text = Me.GetGlobalResourceObject("Text", "N/A")
            ' CRE13-003 - Token Replacement [Start][Tommy L]
            ' -------------------------------------------------------------------------------------
            'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'lblTokenIssueDate.Text = ""
            'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
            ' CRE13-003 - Token Replacement [End][Tommy L]
        End If


        ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [End][Tommy]

        ' Check the token status (Pending Deactivate and Pending Activate)
        Dim strTokenUpdateStatus As String = String.Empty

        For Each dr As DataRow In udtAccountChangeMaintenanceBLL.GetRecordDataTableByKeyValue(udtSP.SPID, "DT").Rows
            strTokenUpdateStatus = TokenPendingStatus.PendingDeactivate
        Next

        If strTokenUpdateStatus = String.Empty Then
            For Each dr As DataRow In udtAccountChangeMaintenanceBLL.GetRecordDataTableByKeyValue(udtSP.SPID, "AT").Rows
                strTokenUpdateStatus = TokenPendingStatus.PendingReactivate
            Next
        End If

        ' CRE13-003 - Token Replacement [Start][Tommy L]
        ' -------------------------------------------------------------------------------------
        lblTokenRemark.Text = ""
        ' CRE13-003 - Token Replacement [End][Tommy L]

        Select Case strTokenUpdateStatus
            Case TokenPendingStatus.PendingDeactivate, TokenPendingStatus.PendingReactivate
                Dim strTokenUpdateStatusText As String = Nothing
                Status.GetDescriptionFromDBCode(TokenPendingStatus.Classcode, strTokenUpdateStatus, strTokenUpdateStatusText, String.Empty)
                ' CRE13-003 - Token Replacement [Start][Tommy L]
                ' -------------------------------------------------------------------------------------
                'lblTokenSN.Text += " (" + strTokenUpdateStatusText + " Token)"
                lblTokenRemark.Text = " (" + strTokenUpdateStatusText + " Token)"
                ' CRE13-003 - Token Replacement [End][Tommy L]
        End Select

        ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [Start][Tommy]

        'Dim udtTokenModel As TokenModel = udtSPProfileBLL.GetTokenModelBySPID(udtSP.SPID)

        ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [End][Tommy]

        'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        imgShareTokenReplacement.ToolTip = String.Empty
        imgShareTokenReplacement.Visible = False
        imgTokenAssignDate.ToolTip = String.Empty
        imgTokenAssignDate.Visible = False
        'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
        If IsNothing(udtTokenModel) OrElse udtTokenModel.TokenSerialNoReplacement = String.Empty Then
            lblTokenReplacedSNText.Visible = False
            lblTokenReplacedSN.Visible = False
            ' CRE13-003 - Token Replacement [Start][Tommy L]
            ' -------------------------------------------------------------------------------------
            'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'lblTokenReplacedDate.Visible = False
            'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
            ' CRE13-003 - Token Replacement [End][Tommy L]
        Else
            lblTokenReplacedSNText.Visible = True
            lblTokenReplacedSN.Visible = True
            ' INT12-0012 HCSP Record Confirmation, CTM error on grid and RCH code input problem [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            ' Not to display PPI-ePR token serial no.
            'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'lblTokenReplacedSN.Text = TokenModel.DisplayTokenSerialNo(udtTokenModel.TokenSerialNoReplacement.Trim, udtTokenModel.Project)

            '' INT12-0012 HCSP Record Confirmation, CTM error on grid and RCH code input problem [End][Koala]
            '' CRE13-003 - Token Replacement [Start][Tommy L]
            '' -------------------------------------------------------------------------------------
            'If udtTokenModel.Project = TokenProjectType.EHCVS Then
            '    lblTokenReplacedDate.Visible = True
            '    lblTokenReplacedDate.Text = " (" + udtFormatter.convertDateTime(udtTokenModel.LastReplacementDtm.Value) + ")"
            'Else
            '    lblTokenReplacedDate.Visible = False
            'End If
            '' CRE13-003 - Token Replacement [End][Tommy L]

            If udtTokenModel.ProjectReplacement = TokenProjectType.EHCVS Then
                lblTokenReplacedSN.Text = TokenModel.DisplayTokenSerialNo(udtTokenModel.TokenSerialNoReplacement.Trim, udtTokenModel.ProjectReplacement, False, False, True)
                imgTokenAssignDate.ToolTip = HttpContext.GetGlobalResourceObject("AlternateText", "TokenAssignDate").ToString.Replace("%s", udtFormatter.convertDateTime(udtTokenModel.LastReplacementDtm.Value).ToString)
                imgTokenAssignDate.Style.Add("vertical-align", "text-top")
                imgTokenAssignDate.Visible = True
            Else
                lblTokenReplacedSN.Text = TokenModel.DisplayTokenSerialNo(udtTokenModel.TokenSerialNoReplacement.Trim, udtTokenModel.ProjectReplacement, False, True, True)
                imgTokenAssignDate.Visible = False
            End If

            If udtTokenModel.IsShareTokenReplacement Then
                imgShareTokenReplacement.ToolTip = HttpContext.GetGlobalResourceObject("AlternateText", "ShareToken").ToString
                imgShareTokenReplacement.Style.Add("vertical-align", "text-top")
                imgShareTokenReplacement.Visible = True
            Else
                imgShareTokenReplacement.Visible = False
            End If
            'CRE14-002 - PPI-ePR Migration [End][Chris YIM]

        End If

        ' Scheme Information
        gvEnrolledScheme.DataSource = udtSP.SchemeInfoList.Values
        gvEnrolledScheme.DataBind()

        ' Token Return Date
        If IsNothing(udtSP.TokenReturnDtm) Then
            lblTokenReturnText.Visible = False
            lblTokenReturn.Visible = False
        Else
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'lblTokenReturn.Text = udtFormatter.formatDate(Convert.ToDateTime(udtSP.TokenReturnDtm)).Trim
            lblTokenReturn.Text = udtFormatter.formatDisplayDate(Convert.ToDateTime(udtSP.TokenReturnDtm)).Trim
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            lblTokenReturnText.Visible = True
            lblTokenReturn.Visible = True
        End If

        ' Handle the Enable of the action buttons
        HandleEnableOfActionButton(udtSP)

        ' Save the Scheme Information to session, for controling the schemes can be selected in delisting / suspending
        Session(SESS_SPSchemeInformation) = udtSP.SchemeInfoList

        ' --------------- Medical Organization Information ---------------

        ' Medical Organization Information
        If IsNothing(udtSP.MOList) OrElse udtSP.MOList.Values.Count = 0 Then
            gvMO.Visible = False
            lblMONA.Visible = True
        Else
            lblMONA.Visible = False
            gvMO.Visible = True
            gvMO.DataSource = udtSP.MOList.Values
            gvMO.DataBind()
        End If

        ' --------------- Practice and Bank Information ---------------

        Session(SESS_SPMaintenanceHandledScheme) = New ArrayList()

        ' Practice and Bank Information
        gvPracticeBank.DataSource = udtSP.PracticeList.Values
        gvPracticeBank.DataBind()

        Session.Remove(SESS_SPMaintenanceHandledScheme)

        ' --------------- End of all information ---------------

        ' Message Box - This record is being amended.
        If udtSP.UnderModification.Trim <> String.Empty Then
            CompleteMsgBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00006)
            CompleteMsgBox.BuildMessageBox()
            CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information
        End If

        ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Dim udtPracticeList As PracticeModelCollection = udtSP.PracticeList.FilterByPCD(TableLocation.Permanent)

        If udtPracticeList.Count > 0 Then
            'If udtSP.PracticeList.FilterByPCD.Count > 0 Then
            ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]
            ibtnJoinPCD.Visible = True
        Else
            ibtnJoinPCD.Visible = False
        End If

    End Sub

    Private Sub HandleEnableOfActionButton(ByVal udtSP As ServiceProviderModel)
        ibtnEditReturnInfo.Enabled = False
        ibtnReactivate.Enabled = False
        ibtnDelisting.Enabled = False
        ibtnSuspend.Enabled = False
        ibtnUnlock.Enabled = False
        'ibtnReleaseIVSSClaim.Enabled = False
        ibtnReleaseIVSSClaim.Visible = False
        ibtnResendEmail.Enabled = False
        ibtnReprintLetter.Enabled = False

        Select Case hfAccountStatus.Value
            Case SPAccountStatus.Suspended
                ' Unlock
                ibtnUnlock.Enabled = True
            Case SPAccountStatus.PendingForActivation
                ' Resend Activation Email & Reprint Acknowledgement Letter
                ibtnResendEmail.Enabled = True
                ibtnReprintLetter.Enabled = True
        End Select

        ' Resend Confirmation Email
        If udtSP.EmailChanged = EmailChanged.Changed AndAlso hfAccountStatus.Value <> SPAccountStatus.Delisted Then
            ibtnResendEmail.Enabled = True
        End If

        ' Resend Email is always enabled
        ibtnResendEmail.Enabled = True

        ' Scheme-involved buttons
        For Each udtScheme As SchemeInformationModel In udtSP.SchemeInfoList.Values
            If udtScheme.SchemeCode = SchemeCode.IVSS Then
                ' AndAlso not in IVSS scheme period
                'AndAlso Not (Date.Now > d1 AndAlso Date.Now < d2) Then
                'ibtnReleaseIVSSClaim.Enabled = True
            End If

            Select Case udtScheme.RecordStatus
                Case SchemeInformationMaintenanceDisplayStatus.Active
                    ibtnDelisting.Enabled = True
                    ibtnSuspend.Enabled = True
                    ibtnReprintLetter.Enabled = True
                    ibtnReprintLetter.Enabled = True

                Case SchemeInformationMaintenanceDisplayStatus.Suspended
                    ibtnReactivate.Enabled = True
                    ibtnDelisting.Enabled = True
                    ibtnReprintLetter.Enabled = True

                Case SchemeInformationMaintenanceDisplayStatus.DelistedVoluntary, SchemeInformationMaintenanceDisplayStatus.DelistedInvoluntary
                    If udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup().Filter(udtScheme.SchemeCode.Trim).ReturnLogoEnabled Then ibtnEditReturnInfo.Enabled = True

                Case SchemeInformationMaintenanceDisplayStatus.ActivePendingSuspend
                    ibtnReprintLetter.Enabled = True

                Case SchemeInformationMaintenanceDisplayStatus.ActivePendingDelist, SchemeInformationMaintenanceDisplayStatus.SuspendedPendingDelist
                    ibtnEditReturnInfo.Enabled = True
                    ibtnReprintLetter.Enabled = True

                Case SchemeInformationMaintenanceDisplayStatus.SuspendedPendingReactivate
                    ibtnReprintLetter.Enabled = True

            End Select

        Next

        ' CRE15-001 RSA Server Upgrade [Start][Winnie]
        ' CRE13-029 - RSA Server Upgrade [Start][Lawrence]
        ' Check Token
        If (New RSAServerHandler).GetRSAAPIVersionMain <> String.Empty Then
            ibtnCheckToken.Visible = True

            If IsNothing(Session(SESS_MaintenanceTokenSerialNo)) Then
                ibtnCheckToken.Enabled = False
            Else
                ibtnCheckToken.Enabled = True
            End If

            ibtnCheckToken.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", IIf(ibtnCheckToken.Enabled, "CheckTokenBtn", "CheckTokenDisableBtn"))
        Else
            ibtnCheckToken.Visible = False
        End If
        ' CRE13-029 - RSA Server Upgrade [End][Lawrence]
        ' CRE15-001 RSA Server Upgrade [End][Winnie]

        ' Handle the images of the buttons
        ibtnEditReturnInfo.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", IIf(ibtnEditReturnInfo.Enabled, "ReturnDtmBtn", "ReturnDtmDisableBtn"))
        ibtnReactivate.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", IIf(ibtnReactivate.Enabled, "ReactivateBtn", "ReactivateDisableBtn"))
        ibtnDelisting.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", IIf(ibtnDelisting.Enabled, "DelistingBtn", "DelistingDisableBtn"))
        ibtnSuspend.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", IIf(ibtnSuspend.Enabled, "SuspendBtn", "SuspendDisableBtn"))
        ibtnUnlock.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", IIf(ibtnUnlock.Enabled, "UnlockBtn", "UnlockDisableBtn"))
        ibtnReleaseIVSSClaim.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", IIf(ibtnReleaseIVSSClaim.Enabled, "ReleaseIVSSClaimBtn", "ReleaseIVSSClaimDisableBtn"))
        ibtnResendEmail.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", IIf(ibtnResendEmail.Enabled, "ResendEmailBtn", "ResendEmailDisableBtn"))
        ibtnReprintLetter.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", IIf(ibtnReprintLetter.Enabled, "ReprintLetterBtn", "ReprintLetterDisableBtn"))

        ' Handle the cursor of the buttons
        ibtnEditReturnInfo.CssClass = IIf(ibtnEditReturnInfo.Enabled, CssIbtnEnabled, CssIbtnDisabled)
        ibtnReactivate.CssClass = IIf(ibtnReactivate.Enabled, CssIbtnEnabled, CssIbtnDisabled)
        ibtnDelisting.CssClass = IIf(ibtnDelisting.Enabled, CssIbtnEnabled, CssIbtnDisabled)
        ibtnSuspend.CssClass = IIf(ibtnSuspend.Enabled, CssIbtnEnabled, CssIbtnDisabled)
        ibtnUnlock.CssClass = IIf(ibtnUnlock.Enabled, CssIbtnEnabled, CssIbtnDisabled)
        ibtnReleaseIVSSClaim.CssClass = IIf(ibtnReleaseIVSSClaim.Enabled, CssIbtnEnabled, CssIbtnDisabled)
        ibtnResendEmail.CssClass = IIf(ibtnResendEmail.Enabled, CssIbtnEnabled, CssIbtnDisabled)
        ibtnReprintLetter.CssClass = IIf(ibtnReprintLetter.Enabled, CssIbtnEnabled, CssIbtnDisabled)

    End Sub

    Protected Sub ibtnSearchResultBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00067, "Back Click")
        ' CRE11-021 log the missed essential information [End]

        udtSPProfileBLL.ClearSession()
        MultiViewMaintenance.ActiveViewIndex = ViewIndexSearchCriteria
    End Sub

    Protected Sub ibtnBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtServiceProviderBLL.ClearSession()
        MultiViewMaintenance.ActiveViewIndex = ViewIndexSearchCriteria
        ibtnSearch_Click(Nothing, Nothing)
        CompleteMsgBox.Visible = False

        If MultiViewMaintenance.ActiveViewIndex = ViewIndexSearchCriteria Then
            ResetSearchCriteria()
        End If
    End Sub

    Protected Sub ibtnDetailsBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        msgBox.Visible = False
        'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
        panInputDtm.Visible = False
        panDelisting.Visible = False
        panSuspend.Visible = False
        panReactivate.Visible = False
        panReleaseIVSSClaim.Visible = False

        CompleteMsgBox.Visible = False

        udtServiceProviderBLL.ClearSession()
        MultiViewMaintenance.ActiveViewIndex = ViewIndexSearchCriteria

        If Not IsNothing(Session(SESS_SearchResultList)) Then ibtnSearch_Click(Nothing, Nothing)

    End Sub

    Private Sub ResetSearchCriteria()
        txtEnrolRefNo.Text = String.Empty
        txtSPID.Text = String.Empty
        txtSPHKID.Text = String.Empty
        txtSPName.Text = String.Empty
        ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
        txtSPChiName.Text = String.Empty
        ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]
        txtPhone.Text = String.Empty
        ddlSPHealthProf.SelectedValue = String.Empty
        ddlScheme.SelectedValue = String.Empty
    End Sub

    Protected Function GetHealthProfName(ByVal strHealthProfCode As String) As String
        If IsNothing(strHealthProfCode) OrElse strHealthProfCode = String.Empty Then
            Return String.Empty
        End If

        ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

        ' -----------------------------------------------------------------------------------------

        If Session("language") = "zh-tw" Then
            Return udtSPProfileBLL.GetHealthProfName(strHealthProfCode).ServiceCategoryDescChi
        Else
            Return udtSPProfileBLL.GetHealthProfName(strHealthProfCode).ServiceCategoryDesc
        End If

        ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

        Return Nothing

    End Function

    Protected Sub ibtnErrorBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        MultiViewMaintenance.ActiveViewIndex = ViewIndexSearchCriteria
        CompleteMsgBox.Visible = False
    End Sub

    ' Gridview - Result

    Protected Sub gvResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Enrolment Reference No.
            Dim lnkbtnERN As LinkButton = CType(e.Row.FindControl("lnkbtnERN"), LinkButton)
            Dim lnkbtnRSPID As LinkButton = CType(e.Row.FindControl("lnkbtnRSPID"), LinkButton)

            lnkbtnERN.Text = udtFormatter.formatSystemNumber(lnkbtnERN.Text)
            lnkbtnERN.OnClientClick = "javascript:getKey('" & lnkbtnRSPID.CommandArgument.Trim & "')"
            lnkbtnERN.Attributes.Add("onclick", "return false;")

            ' Service Provider ID
            lnkbtnRSPID.OnClientClick = "javascript:getKey('" & lnkbtnRSPID.CommandArgument.Trim & "')"
            lnkbtnRSPID.Attributes.Add("onclick", "return false;")

            ' Service Provider HKIC No.
            Dim lblRSPHKID As Label = CType(e.Row.FindControl("lblRSPHKID"), Label)
            lblRSPHKID.Text = udtFormatter.formatHKID(lblRSPHKID.Text, False)

            ' Service Provider Name (Chinese)
            Dim lblRCname As Label = CType(e.Row.FindControl("lblRCname"), Label)
            lblRCname.Text = udtFormatter.formatChineseName(lblRCname.Text.Trim)

            ' Service Provider Status
            Dim lblRSPStatus As Label = CType(e.Row.FindControl("lblRSPStatus"), Label)
            Status.GetDescriptionFromDBCode(ServiceProviderStatus.ClassCode, lblRSPStatus.Text.Trim, lblRSPStatus.Text, String.Empty)
        End If
    End Sub

    Protected Sub gvResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvResult.PageIndexChanging
        Me.GridViewPageIndexChangingHandler(sender, e, SESS_SearchResultList)
    End Sub

    Protected Sub gvResult_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvResult.PreRender
        Me.GridViewPreRenderHandler(sender, e, SESS_SearchResultList)
    End Sub

    Protected Sub gvResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvResult.Sorting
        Me.GridViewSortingHandler(sender, e, SESS_SearchResultList)
    End Sub

    ' Gridview - Scheme Information

    Protected Sub gvEnrolledScheme_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvEnrolledScheme.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Scheme Name
            Dim lblESchemeName As Label = e.Row.FindControl("lblESchemeName")
            For Each udtSchemeBackOfficeModel As SchemeBackOfficeModel In udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup
                If udtSchemeBackOfficeModel.SchemeCode.Trim = lblESchemeName.Text.Trim Then
                    lblESchemeName.Text = udtSchemeBackOfficeModel.DisplayCode.Trim
                    Exit For
                End If
            Next

            ' Status [Remarks]
            gvEnrolledScheme.HeaderRow.Cells(1).Text = Me.GetGlobalResourceObject("Text", "Status") + " [" + Me.GetGlobalResourceObject("Text", "Remarks") + "]"

            Dim lblERecordStatus As Label = CType(e.Row.FindControl("lblERecordStatus"), Label)
            Status.GetDescriptionFromDBCode(SchemeInformationMaintenanceDisplayStatus.ClassCode, lblERecordStatus.Text.Trim, lblERecordStatus.Text, String.Empty)

            Dim lblERemark As Label = CType(e.Row.FindControl("lblERemark"), Label)
            If lblERemark.Text.Trim <> String.Empty Then
                lblERecordStatus.Text += " [" + lblERemark.Text.Trim + "]"
            End If

            ' Effective Date
            Dim lblEEffectiveDtm As Label = CType(e.Row.FindControl("lblEEffectiveDtm"), Label)
            If lblEEffectiveDtm.Text.Trim = String.Empty Then
                lblEEffectiveDtm.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                lblEEffectiveDtm.Text = udtFormatter.convertDateTime(lblEEffectiveDtm.Text.Trim)
            End If

            ' Delisting Date
            Dim lblEDelistDtm As Label = CType(e.Row.FindControl("lblEDelistDtm"), Label)
            If lblEDelistDtm.Text.Trim = String.Empty Then
                lblEDelistDtm.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                lblEDelistDtm.Text = udtFormatter.convertDateTime(lblEDelistDtm.Text.Trim)
            End If

            ' Logo Return Date
            Dim lblELogoReturnDate As Label = CType(e.Row.FindControl("lblELogoReturnDate"), Label)
            If lblELogoReturnDate.Text.Trim = String.Empty Then
                lblELogoReturnDate.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'lblELogoReturnDate.Text = udtFormatter.formatDate(Convert.ToDateTime(lblELogoReturnDate.Text.Trim))
                lblELogoReturnDate.Text = udtFormatter.formatDisplayDate(Convert.ToDateTime(lblELogoReturnDate.Text.Trim))
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            End If

        End If

    End Sub

    ' Show Amendment Record

    Protected Sub ibtnAmendedRecord_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Try
            Dim udtSP As ServiceProviderModel = udtSPProfileBLL.GetServiceProviderStagingProfileNoSession(hfERN.Value.Trim)

            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00042, "Show Pending Amendment")

            If IsNothing(udtSP) Then
                CompleteMsgBox.AddMessage("990000", "I", "00015")
                CompleteMsgBox.BuildMessageBox()
                CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information
                udtAuditLogEntry.WriteEndLog(LogID.LOG00044, "Show Pending Amendment failed: Service Provider status changed")

            Else
                udcExistingSPProfile.buildSpProfileObject(udtSP, TableLocation.Staging)
                udcExistingSPProfile.DisplayRecordStatus(True, TableLocation.Staging)

                ModalPopupExtenderSPProfile.Show()

                udtAuditLogEntry.WriteEndLog(LogID.LOG00043, "Show Pending Amendment completed")

            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub ibtnExistingSPProfileClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ModalPopupExtenderSPProfile.Hide()
    End Sub

    '

    Private Sub BindSPActionDetails()
        lblActionERN.Text = lblERN.Text
        lblActionDetailsSPID.Text = lblDetailsSPID.Text

        If lblSPUsername.Text = String.Empty Then
            lblActionSPUsername.Visible = False
            lblActionSPUsernameText.Visible = False
        Else
            lblActionSPUsername.Text = lblSPUsername.Text
            lblActionSPUsername.Visible = True
            lblActionSPUsernameText.Visible = True
        End If
        lblActionEffectivateDate.Text = lblEffectivateDate.Text
        lblActionEname.Text = lblEname.Text
        lblActionCname.Text = lblCname.Text
        lblActionHKID.Text = lblHKID.Text
        lblActionAddress.Text = lblAddress.Text
        lblActionEmail.Text = lblEmail.Text

        ' CRE16-018 Display SP tentative email in HCVU [Start][Winnie]
        If lblPendingEmail.Text = String.Empty Then
            trActionPendingEmail.Visible = False
        Else
            trActionPendingEmail.Visible = True
            lblActionPendingEmail.Text = lblPendingEmail.Text
        End If
        ' CRE16-018 Display SP tentative email in HCVU [End][Winnie]

        lblActionContactNo.Text = lblContactNo.Text
        lblActionFax.Text = lblFax.Text
        lblActionRecordStatus.Text = lblRecordStatus.Text
        lblActionAccountStatus.Text = lblAccountStatus.Text
        lblActionTokenSN.Text = lblTokenSN.Text
        ' CRE13-003 - Token Replacement [Start][Tommy L]
        ' -------------------------------------------------------------------------------------
        'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'lblActionTokenIssueDate.Text = lblTokenIssueDate.Text
        imgActionShareToken.ToolTip = imgShareToken.ToolTip.ToString
        imgActionShareToken.Style.Add("vertical-align", "text-top")
        imgActionShareToken.Visible = imgShareToken.Visible
        imgActionTokenActivateDate.ToolTip = imgTokenActivateDate.ToolTip.ToString
        imgActionTokenActivateDate.Style.Add("vertical-align", "text-top")
        imgActionTokenActivateDate.Visible = imgTokenActivateDate.Visible
        ImgActionShareTokenReplacement.Visible = False
        ImgActionTokenAssignDate.Visible = False
        'CRE14-002 - PPI-ePR Migration [End][Chris YIM]

        lblActionTokenRemark.Text = lblTokenRemark.Text
        ' CRE13-003 - Token Replacement [End][Tommy L]

        If Not lblTokenReplacedSN.Visible OrElse lblTokenReplacedSN.Equals(String.Empty) Then
            lblActionTokenReplacedSNText.Visible = False
            lblActionTokenReplacedSN.Visible = False
            'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'lblActionTokenReplacedDate.Visible = False
            ImgActionShareTokenReplacement.Visible = False
            ImgActionTokenAssignDate.Visible = False
            'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
        Else
            lblActionTokenReplacedSNText.Visible = True
            lblActionTokenReplacedSN.Visible = True
            lblActionTokenReplacedSN.Text = lblTokenReplacedSN.Text
            'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            ImgActionShareTokenReplacement.ToolTip = imgShareTokenReplacement.ToolTip.ToString
            ImgActionShareTokenReplacement.Style.Add("vertical-align", "text-top")
            ImgActionShareTokenReplacement.Visible = imgShareTokenReplacement.Visible
            ImgActionTokenAssignDate.ToolTip = imgTokenAssignDate.ToolTip.ToString
            ImgActionTokenAssignDate.Style.Add("vertical-align", "text-top")
            ImgActionTokenAssignDate.Visible = imgTokenAssignDate.Visible
            'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
            ' CRE13-003 - Token Replacement [Start][Tommy L]
            ' -------------------------------------------------------------------------------------
            'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'If lblTokenReplacedDate.Visible = True Then
            '    lblActionTokenReplacedDate.Visible = True
            '    lblActionTokenReplacedDate.Text = lblTokenReplacedDate.Text
            'Else
            '    lblActionTokenReplacedDate.Visible = False
            'End If
            'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
            ' CRE13-003 - Token Replacement [End][Tommy L]
        End If

    End Sub

    ' Gridview - Medical Organization

    Protected Sub gvMO_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMO.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Email Address
            Dim lblMOEmail As Label = CType(e.Row.FindControl("lblMOEmail"), Label)
            If lblMOEmail.Text.Trim = String.Empty Then
                lblMOEmail.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            ' Fax No.
            Dim lblMOFax As Label = CType(e.Row.FindControl("lblMOFax"), Label)
            If lblMOFax.Text.Trim = String.Empty Then
                lblMOFax.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            ' Medical Organization Status
            Dim lblMOStatus As Label = CType(e.Row.FindControl("lblMOStatus"), Label)
            Status.GetDescriptionFromDBCode(MedicalOrganizationStatus.ClassCode, lblMOStatus.Text.Trim, lblMOStatus.Text, String.Empty)
        End If
    End Sub

    ' Gridview - Practice

    Protected Sub gvPracticeBank_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPracticeBank.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dt As DataTable = Nothing 'CRE20-006 DHC Integration [Nichole]
            Dim dvPractice As DataView = Nothing 'CRE20-006 DHC Integration [Nichole]

            Dim udtPractice As PracticeModel = DirectCast(e.Row.DataItem, PracticeModel)

            ' Convert Medical Organization No. to Medical Organization Name
            Dim lblPracticeMO As Label = CType(e.Row.FindControl("lblPracticeMO"), Label)
            Dim intMODisplaySeq As Integer = CInt(lblPracticeMO.Text.Trim)

            Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP
            For Each udtMO As MedicalOrganization.MedicalOrganizationModel In udtSP.MOList.Values
                If udtMO.DisplaySeq.Value = intMODisplaySeq Then
                    lblPracticeMO.Text = udtMO.DisplaySeqMOName
                    Exit For
                End If
            Next

            If lblPracticeMO.Text = strMONotAvailable Then lblPracticeMO.Text = Me.GetGlobalResourceObject("Text", "N/A")

            'CRE20-006 DHC integartion [Start][Nichole]
            ' Enrolled DHC 
            Dim lblEnrolledDHCPrac As Label = e.Row.FindControl("lblEnrolledDHCPrac")
            Dim lblEnrolledDHCPracText As Label = e.Row.FindControl("lblEnrolledDHCPracText")


            dt = Session(SESS_SPMaintenancePracticeEnrolledDHCList)
            dvPractice = New DataView(dt)
            dvPractice.RowFilter = "[Professional_Seq] ='" + udtPractice.ProfessionalSeq.ToString() + "'"
            If dvPractice.Count > 0 Then
                lblEnrolledDHCPrac.Text = dvPractice(0)("DistrictName").ToString()
            Else
                If lblEnrolledDHCPrac.Text = String.Empty Then lblEnrolledDHCPrac.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            'If lblEnrolledDHCPrac.Text = String.Empty Then lblEnrolledDHCPrac.Text = Me.GetGlobalResourceObject("Text", "N/A")
            
            'CRE20-006 DHC integartion [End][Nichole]

            ' Phone No. of Practice
            Dim lblPracticePhone As Label = e.Row.FindControl("lblPracticePhone")
            If lblPracticePhone.Text = String.Empty Then lblPracticePhone.Text = Me.GetGlobalResourceObject("Text", "N/A")

            ' CRE16-022 (Add optional field "Remarks") [Start][Winnie]
            ' ------------------------------------------------------------------------
            ' Mobile Clinic
            Dim lblPracticeMobileClinic As Label = CType(e.Row.FindControl("lblPracticeMobileClinic"), Label)
            If lblPracticeMobileClinic.Text = YesNo.Yes Then
                lblPracticeMobileClinic.Text = Me.GetGlobalResourceObject("Text", "Yes")
            Else
                lblPracticeMobileClinic.Text = Me.GetGlobalResourceObject("Text", "No")
            End If

            ' Practice Remarks
            Dim lblPracticeRemarks As Label = e.Row.FindControl("lblPracticeRemarks")
            Dim lblPracticeRemarksChi As Label = e.Row.FindControl("lblPracticeRemarksChi")
            Dim strRemarksDescEng As String = udtPractice.RemarksDesc
            Dim strRemarksDescChi As String = udtPractice.RemarksDescChi

            If strRemarksDescEng = String.Empty AndAlso strRemarksDescChi = String.Empty Then
                lblPracticeRemarks.Text = Me.GetGlobalResourceObject("Text", "N/A")

            ElseIf strRemarksDescEng <> String.Empty Then
                lblPracticeRemarks.Text = strRemarksDescEng
                lblPracticeRemarksChi.Text = formatChineseString(strRemarksDescChi)

            Else
                lblPracticeRemarks.Text = strRemarksDescChi
            End If
            ' CRE16-022 (Add optional field "Remarks") [End][Winnie]

            ' Status of Practice
            Dim lblPracticeStatus As Label = CType(e.Row.FindControl("lblPracticeStatus"), Label)
            Status.GetDescriptionFromDBCode(PracticeStatus.ClassCode, lblPracticeStatus.Text.Trim, lblPracticeStatus.Text, String.Empty)

            ' Practice Scheme Info
            Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = udtPractice.PracticeSchemeInfoList
            Dim gvPracticeSchemeInfo As GridView = e.Row.FindControl("gvPracticeSchemeInfo")

            Session(SESS_SPMaintenancePracticeSchemeInfoList) = udtPracticeSchemeInfoList

            gvPracticeSchemeInfo.DataSource = udtSchemeBackOfficeBLL.GetAllEffectiveSubsidizeGroupFromCache.ToSPProfileDataTable
            gvPracticeSchemeInfo.DataBind()

            ' Control the Enable of the "Delisting", "Suspend", "Reactivate" buttons
            If Not Session(SESS_ShowDelistPracticeBtn) Then
                Dim ibtnSPracticeDelisting As ImageButton = CType(e.Row.FindControl("ibtnSPracticeDelisting"), ImageButton)
                ibtnSPracticeDelisting.Enabled = False
                ibtnSPracticeDelisting.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "DelistingSDisableBtn")
            End If

            If Not Session(SESS_ShowSuspendPracticeBtn) Then
                Dim ibtnSPracticeSuspend As ImageButton = CType(e.Row.FindControl("ibtnSPracticeSuspend"), ImageButton)
                ibtnSPracticeSuspend.Enabled = False
                ibtnSPracticeSuspend.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SuspendSDisableBtn")
            End If

            If Not Session(SESS_ShowReactivatePracticeBtn) Then
                Dim ibtnSPracticeReactivate As ImageButton = CType(e.Row.FindControl("ibtnSPracticeReactivate"), ImageButton)
                ibtnSPracticeReactivate.Enabled = False
                ibtnSPracticeReactivate.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ReactivateSDisableBtn")
            End If

            Session.Remove(SESS_ShowReactivatePracticeBtn)
            Session.Remove(SESS_ShowSuspendPracticeBtn)
            Session.Remove(SESS_ShowDelistPracticeBtn)

            CType(Session(SESS_SPMaintenanceHandledScheme), ArrayList).Clear()

        End If
    End Sub

    Protected Sub gvPracticeSchemeInfo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Select Case e.Row.RowType
            Case DataControlRowType.DataRow
                ' Don't Remove, work around, under testing
                If Me.Page Is Nothing OrElse Me.udtSchemeBackOfficeBLL Is Nothing Then
                    Return
                End If

                ' CRE16-022 (Add optional field "Remarks") [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                'Dim strSchemeCode As String = DirectCast(e.Row.FindControl("hfPracticeSchemeCodeReal"), HiddenField).Value
                'Dim strSubsidizeCode As String = DirectCast(e.Row.FindControl("hfPracticeSubsidizeCodeReal"), HiddenField).Value
                Dim strSchemeCode As String = DirectCast(e.Row.FindControl("lblPracticeSchemeCode"), Label).Attributes("SchemeCodeReal")
                Dim strSubsidizeCode As String = DirectCast(e.Row.FindControl("lblPracticeSubsidizeCode"), Label).Attributes("SubsidizeCodeReal")

                Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = Session(SESS_SPMaintenancePracticeSchemeInfoList)
                Dim udtPracticeSchemeInfo As PracticeSchemeInfoModel = udtPracticeSchemeInfoList.Filter(strSchemeCode, strSubsidizeCode)

                e.Row.Visible = False

                If udtPracticeSchemeInfo Is Nothing Then
                    Return
                End If

                ' Hide the row if not enrolled or not providing service
                'If DirectCast(e.Row.FindControl("hfGIsCategoryHeader"), HiddenField).Value = "Y" Then
                If DirectCast(e.Row.FindControl("lblPracticeSchemeCode"), Label).Attributes("IsCategoryHeaderReal") = "Y" Then
                    For Each udtPSINode As PracticeSchemeInfoModel In udtPracticeSchemeInfoList.Values
                        If udtPSINode.SchemeCode = strSchemeCode Then
                            udtPracticeSchemeInfo = udtPSINode
                            Exit For
                        End If
                    Next

                    If IsNothing(udtPracticeSchemeInfo) Then
                        Return
                    End If

                Else
                    If IsNothing(udtPracticeSchemeInfoList.Filter(strSchemeCode)) Then
                        Return
                    End If

                    ' Check all not provide service
                    Dim blnAllNotProvideService As Boolean = True

                    For Each udtPSINode As PracticeSchemeInfoModel In udtPracticeSchemeInfoList.Filter(strSchemeCode).Values
                        If udtPSINode.ProvideService Then
                            blnAllNotProvideService = False
                            Exit For
                        End If
                    Next

                    If blnAllNotProvideService Then
                        'DirectCast(e.Row.FindControl("hfGAllNotProvideService"), HiddenField).Value = "Y"
                        DirectCast(e.Row.FindControl("lblPracticeSchemeCode"), Label).Attributes.Add("AllNotProvideServiceReal", "Y")

                    Else
                        If IsNothing(udtPracticeSchemeInfo) OrElse udtPracticeSchemeInfo.ProvideService = False Then
                            Return

                        End If

                    End If

                End If

                e.Row.Visible = True

                ' Scheme Code
                If udtPracticeSchemeInfo.ClinicType = PracticeSchemeInfoModel.ClinicTypeEnum.NonClinic Then
                    Dim lblPracticeSchemeCode As Label = e.Row.FindControl("lblPracticeSchemeCode")
                    lblPracticeSchemeCode.Text += String.Format("<br />({0})", Me.GetGlobalResourceObject("Text", "NonClinic"))
                End If

                ' Service Fee
                Dim udtSubsidizeGpBO As SubsidizeGroupBackOfficeModel = udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup.Filter(strSchemeCode).SubsidizeGroupBackOfficeList.Filter(strSchemeCode, strSubsidizeCode)

                Dim lblPracticeServiceFee As Label = CType(e.Row.FindControl("lblPracticeServiceFee"), Label)

                If udtPracticeSchemeInfo.ProvideServiceFee.HasValue Then
                    If udtPracticeSchemeInfo.ProvideServiceFee.Value AndAlso udtPracticeSchemeInfo.ServiceFee.HasValue Then
                        lblPracticeServiceFee.Text = udtFormatter.formatMoney(udtPracticeSchemeInfo.ServiceFee, True)

                    Else
                        lblPracticeServiceFee.Text = udtSubsidizeGpBO.ServiceFeeCompulsoryWording

                    End If

                Else
                    If udtSubsidizeGpBO.ServiceFeeEnabled Then
                        lblPracticeServiceFee.Text = "--"
                    Else
                        lblPracticeServiceFee.Text = Me.GetGlobalResourceObject("Text", "ServiceFeeN/A")
                    End If

                End If

                ' Status [Remark]
                Dim lblPracticeSchemeStatus As Label = CType(e.Row.FindControl("lblPracticeSchemeStatus"), Label)
                'Dim hfPracticeSchemeStatusReal As HiddenField = e.Row.FindControl("hfPracticeSchemeStatusReal")

                ' Keep the real status for later use
                'hfPracticeSchemeStatusReal.Value = udtPracticeSchemeInfo.RecordStatus
                lblPracticeSchemeStatus.Attributes.Add("StatusReal", udtPracticeSchemeInfo.RecordStatus)

                Dim intTargetPracticeSeq As Integer = udtPracticeSchemeInfo.PracticeDisplaySeq
                Dim udtTargetPractice As PracticeModel = Nothing

                For Each udtPractice As PracticeModel In gvPracticeBank.DataSource
                    If udtPractice.DisplaySeq = intTargetPracticeSeq Then
                        udtTargetPractice = udtPractice
                        Exit For
                    End If
                Next

                lblPracticeSchemeStatus.Text = udtSPProfileBLL.GetPracticeSchemeInfoStatus(udtTargetPractice, strSchemeCode, TableLocation.Permanent)

                Status.GetDescriptionFromDBCode(PracticeSchemeInfoMaintenanceDisplayStatus.ClassCode, lblPracticeSchemeStatus.Text.Trim, lblPracticeSchemeStatus.Text, String.Empty)

                If udtPracticeSchemeInfo.Remark <> String.Empty Then
                    lblPracticeSchemeStatus.Text += " [" + udtPracticeSchemeInfo.Remark + "]"
                End If

                ' Effective Time & Delisting Time
                Dim lblPracticeSchemeEffectiveDtm As Label = CType(e.Row.FindControl("lblPracticeSchemeEffectiveDtm"), Label)
                Dim lblPracticeSchemeDelistDtm As Label = CType(e.Row.FindControl("lblPracticeSchemeDelistDtm"), Label)

                udtSPProfileBLL.GetPracticeSchemeInfoEarliestTime(udtPracticeSchemeInfoList, strSchemeCode,
                                                                  lblPracticeSchemeEffectiveDtm.Text, lblPracticeSchemeDelistDtm.Text)

                If lblPracticeSchemeEffectiveDtm.Text.Equals(String.Empty) Then
                    lblPracticeSchemeEffectiveDtm.Text = Me.GetGlobalResourceObject("Text", "N/A")
                End If

                If lblPracticeSchemeDelistDtm.Text.Equals(String.Empty) Then
                    lblPracticeSchemeDelistDtm.Text = Me.GetGlobalResourceObject("Text", "N/A")
                End If

                ' Control the Enable of the "Delisting", "Suspend", "Reactivate" buttons
                Dim strSPSchemeStatus As String = String.Empty
                For Each udtScheme As SchemeInformationModel In udtServiceProviderBLL.GetSP.SchemeInfoList.Values
                    If udtScheme.SchemeCode = strSchemeCode Then
                        strSPSchemeStatus = udtScheme.RecordStatus
                        Exit For
                    End If
                Next

                ' Don't Remove, work around, under testing
                If Me.Page Is Nothing OrElse Me.udtSchemeBackOfficeBLL Is Nothing Then
                    Return
                End If

                ' If the Scheme is Pending Delist, does not allow any control on the Practice Scheme
                If strSPSchemeStatus <> SchemeInformationMaintenanceDisplayStatus.ActivePendingDelist AndAlso strSPSchemeStatus <> SchemeInformationMaintenanceDisplayStatus.SuspendedPendingDelist Then
                    Select Case udtPracticeSchemeInfo.RecordStatus
                        Case PracticeSchemeInfoMaintenanceDisplayStatus.Active
                            Session(SESS_ShowDelistPracticeBtn) = True
                            Session(SESS_ShowSuspendPracticeBtn) = True
                        Case PracticeSchemeInfoMaintenanceDisplayStatus.Suspended
                            Session(SESS_ShowDelistPracticeBtn) = True
                            Session(SESS_ShowReactivatePracticeBtn) = True
                        Case PracticeSchemeInfoMaintenanceDisplayStatus.ActivePendingDelist
                            ' Nothing here
                        Case PracticeSchemeInfoMaintenanceDisplayStatus.ActivePendingSuspend
                            ' Nothing here
                        Case PracticeSchemeInfoMaintenanceDisplayStatus.SuspendedPendingReactivate
                            ' Nothing here
                        Case PracticeSchemeInfoMaintenanceDisplayStatus.SuspendedPendingDelist
                            ' Nothing here
                    End Select
                End If
                ' CRE16-022 (Add optional field "Remarks") [End][Chris YIM]

            Case DataControlRowType.Header
                e.Row.Cells(1).ColumnSpan = 2
                e.Row.Cells(2).Visible = False

                ' Change Header Text: Status -> Status [Remark]
                e.Row.Cells(3).Text = Me.GetGlobalResourceObject("Text", "Status") + " [" + Me.GetGlobalResourceObject("Text", "Remarks") + "]"

        End Select

    End Sub

    ' CRE16-022 (Add optional field "Remarks") [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Protected Sub gvPracticeSchemeInfo_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim gvPracticeSchemeInfo As GridView = sender

        ' Handle Category
        For Each gvr As GridViewRow In gvPracticeSchemeInfo.Rows
            'If DirectCast(gvr.FindControl("hfGIsCategoryHeader"), HiddenField).Value = "Y" Then
            If DirectCast(gvr.FindControl("lblPracticeSchemeCode"), Label).Attributes("IsCategoryHeaderReal") = "Y" Then
                ' Check whether this category is visible
                'Dim strSchemeCode As String = DirectCast(gvr.FindControl("hfPracticeSchemeCodeReal"), HiddenField).Value
                'Dim strCategoryName As String = DirectCast(gvr.FindControl("hfGCategoryName"), HiddenField).Value
                Dim strSchemeCode As String = DirectCast(gvr.FindControl("lblPracticeSchemeCode"), Label).Attributes("SchemeCodeReal")
                Dim strCategoryName As String = DirectCast(gvr.FindControl("lblPracticeSchemeCode"), Label).Attributes("CategoryNameReal")
                Dim blnVisible As Boolean = False

                For Each r As GridViewRow In gvPracticeSchemeInfo.Rows
                    'If DirectCast(r.FindControl("hfGIsCategoryHeader"), HiddenField).Value = "N" _
                    '        AndAlso DirectCast(r.FindControl("hfPracticeSchemeCodeReal"), HiddenField).Value = strSchemeCode _
                    '        AndAlso DirectCast(r.FindControl("hfGCategoryName"), HiddenField).Value = strCategoryName _
                    '        AndAlso r.Visible Then
                    If DirectCast(r.FindControl("lblPracticeSchemeCode"), Label).Attributes("IsCategoryHeaderReal") = "N" _
                            AndAlso DirectCast(r.FindControl("lblPracticeSchemeCode"), Label).Attributes("SchemeCodeReal") = strSchemeCode _
                            AndAlso DirectCast(r.FindControl("lblPracticeSchemeCode"), Label).Attributes("CategoryNameReal") = strCategoryName _
                            AndAlso r.Visible Then
                        blnVisible = True
                        Exit For
                    End If

                Next

                If blnVisible Then
                    'gvr.Cells(1).Text = AntiXssEncoder.HtmlEncode(DirectCast(gvr.FindControl("hfGCategoryName"), HiddenField).Value, True)
                    gvr.Cells(1).Text = AntiXssEncoder.HtmlEncode(DirectCast(gvr.FindControl("lblPracticeSchemeCode"), Label).Attributes("CategoryNameReal"), True)
                    gvr.Cells(1).ColumnSpan = 2
                    gvr.Cells(1).CssClass = "SubsidizeCategoryHeader"
                    gvr.Cells(2).Visible = False

                Else
                    gvr.Visible = False

                End If

            End If

        Next

        ' End of Handle Category

        Dim udtSchemeBackOfficeList As SchemeBackOfficeModelCollection = udtSchemeBackOfficeBLL.GetAllEffectiveSchemeBackOfficeWithSubsidizeGroupFromCache
        Dim strPreviousScheme As String = String.Empty

        For Each gvr As GridViewRow In gvPracticeSchemeInfo.Rows
            If Not gvr.Visible Then
                Continue For
            End If

            'Dim strSchemeCode As String = DirectCast(gvr.FindControl("hfPracticeSchemeCodeReal"), HiddenField).Value
            Dim strSchemeCode As String = DirectCast(gvr.FindControl("lblPracticeSchemeCode"), Label).Attributes("SchemeCodeReal")

            If Not udtSchemeBackOfficeList.Filter(strSchemeCode).DisplaySubsidizeDesc Then
                gvr.Cells(1).ColumnSpan = 2
                gvr.Cells(2).Visible = False
                gvr.Cells(1).Text = Me.GetGlobalResourceObject("Text", "N/A")

            End If

            ' Grouping depends on gridview instead of subsidizelist
            Dim RowCount As Integer = 0

            If Not strPreviousScheme.Equals(strSchemeCode) Then

                For Each gvrow As GridViewRow In gvPracticeSchemeInfo.Rows
                    If gvrow.Visible Then
                        'If DirectCast(gvrow.FindControl("hfPracticeSchemeCodeReal"), HiddenField).Value = strSchemeCode Then
                        If DirectCast(gvrow.FindControl("lblPracticeSchemeCode"), Label).Attributes("SchemeCodeReal") = strSchemeCode Then
                            RowCount += 1
                        End If
                    End If
                Next

                gvr.Cells(0).RowSpan = RowCount
                gvr.Cells(3).RowSpan = RowCount
                gvr.Cells(4).RowSpan = RowCount
                gvr.Cells(5).RowSpan = RowCount

                Dim blnAllNotProvideService As Boolean = False

                For Each gvrow As GridViewRow In gvPracticeSchemeInfo.Rows
                    'If DirectCast(gvrow.FindControl("hfPracticeSchemeCodeReal"), HiddenField).Value = strSchemeCode AndAlso _
                    '    DirectCast(gvrow.FindControl("hfGAllNotProvideService"), HiddenField).Value = "Y" Then
                    If DirectCast(gvrow.FindControl("lblPracticeSchemeCode"), Label).Attributes("SchemeCodeReal") = strSchemeCode AndAlso _
                        DirectCast(gvrow.FindControl("lblPracticeSchemeCode"), Label).Attributes("AllNotProvideServiceReal") = "Y" Then

                        blnAllNotProvideService = True
                        Exit For
                    End If

                Next

                If blnAllNotProvideService Then
                    gvr.Cells(1).Text = Me.GetGlobalResourceObject("Text", "NoServiceFeesProvided")
                    gvr.Cells(1).CssClass = "tableText"
                    gvr.Cells(1).RowSpan = RowCount
                    gvr.Cells(1).ColumnSpan = 2
                    gvr.Cells(2).Visible = False
                End If

            Else
                gvr.Cells(0).Visible = False
                gvr.Cells(3).Visible = False
                gvr.Cells(4).Visible = False
                gvr.Cells(5).Visible = False

                Dim blnAllNotProvideService As Boolean = False

                For Each gvrow As GridViewRow In gvPracticeSchemeInfo.Rows
                    'If DirectCast(gvrow.FindControl("hfPracticeSchemeCodeReal"), HiddenField).Value = strSchemeCode AndAlso _
                    '    DirectCast(gvrow.FindControl("hfGAllNotProvideService"), HiddenField).Value = "Y" Then
                    If DirectCast(gvrow.FindControl("lblPracticeSchemeCode"), Label).Attributes("SchemeCodeReal") = strSchemeCode AndAlso _
                        DirectCast(gvrow.FindControl("lblPracticeSchemeCode"), Label).Attributes("AllNotProvideServiceReal") = "Y" Then

                        blnAllNotProvideService = True
                        Exit For
                    End If

                Next

                If blnAllNotProvideService Then
                    gvr.Cells(1).Visible = False
                    gvr.Cells(2).Visible = False
                End If

            End If

            strPreviousScheme = strSchemeCode

        Next
        ' CRE16-022 (Add optional field "Remarks") [End][Chris YIM]

    End Sub

    ' Gridview - Action - Practice

    Protected Sub gvActionPracticeBank_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvActionPracticeBank.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim udtPractice As PracticeModel = DirectCast(e.Row.DataItem, PracticeModel)

            ' Convert Medical Organization No. to No. + Name
            Dim lblPracticeMO As Label = CType(e.Row.FindControl("lblActionPracticeMO"), Label)
            Dim intMODisplaySeq As Integer = CInt(lblPracticeMO.Text.Trim)

            Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP
            For Each udtMO As MedicalOrganization.MedicalOrganizationModel In udtSP.MOList.Values
                If udtMO.DisplaySeq.Value = intMODisplaySeq Then
                    lblPracticeMO.Text = udtMO.DisplaySeqMOName
                    Exit For
                End If
            Next

            If lblPracticeMO.Text = strMONotAvailable Then lblPracticeMO.Text = Me.GetGlobalResourceObject("Text", "N/A")

            ' Phone No. of Practice
            Dim lblPracticePhone As Label = e.Row.FindControl("lblPracticePhone")
            If lblPracticePhone.Text = String.Empty Then lblPracticePhone.Text = Me.GetGlobalResourceObject("Text", "N/A")

            ' CRE16-022 (Add optional field "Remarks") [Start][Winnie]
            ' ------------------------------------------------------------------------
            ' Mobile Clinic
            Dim lblPracticeMobileClinic As Label = CType(e.Row.FindControl("lblActionPracticeMobileClinic"), Label)
            If lblPracticeMobileClinic.Text = YesNo.Yes Then
                lblPracticeMobileClinic.Text = Me.GetGlobalResourceObject("Text", "Yes")
            Else
                lblPracticeMobileClinic.Text = Me.GetGlobalResourceObject("Text", "No")
            End If

            ' Practice Remarks
            Dim lblPracticeRemarks As Label = e.Row.FindControl("lblActionPracticeRemarks")
            Dim lblPracticeRemarksChi As Label = e.Row.FindControl("lblActionPracticeRemarksChi")
            Dim strRemarksDescEng As String = udtPractice.RemarksDesc
            Dim strRemarksDescChi As String = udtPractice.RemarksDescChi

            If strRemarksDescEng = String.Empty AndAlso strRemarksDescChi = String.Empty Then
                lblPracticeRemarks.Text = Me.GetGlobalResourceObject("Text", "N/A")

            ElseIf strRemarksDescEng <> String.Empty Then
                lblPracticeRemarks.Text = strRemarksDescEng
                lblPracticeRemarksChi.Text = formatChineseString(strRemarksDescChi)

            Else
                lblPracticeRemarks.Text = strRemarksDescChi
            End If
            ' CRE16-022 (Add optional field "Remarks") [End][Winnie]

            ' Status of Practice
            Dim lblPracticeStatus As Label = CType(e.Row.FindControl("lblPracticeStatus"), Label)
            Status.GetDescriptionFromDBCode(PracticeStatus.ClassCode, lblPracticeStatus.Text.Trim, lblPracticeStatus.Text, String.Empty)

            CType(Session(SESS_SPMaintenanceHandledScheme), ArrayList).Clear()

            ' Practice Scheme Info            
            Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = udtPractice.PracticeSchemeInfoList
            Dim gvActionPracticeSchemeInfo As GridView = e.Row.FindControl("gvActionPracticeSchemeInfo")

            Session(SESS_SPMaintenancePracticeSchemeInfoList) = udtPracticeSchemeInfoList

            gvActionPracticeSchemeInfo.DataSource = udtSchemeBackOfficeBLL.GetAllEffectiveSubsidizeGroupFromCache.ToSPProfileDataTable
            gvActionPracticeSchemeInfo.DataBind()

        End If
    End Sub

    Protected Sub gvActionPracticeSchemeInfo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Select Case e.Row.RowType
            Case DataControlRowType.DataRow
                ' CRE16-022 (Add optional field "Remarks") [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                'Dim strSchemeCode As String = DirectCast(e.Row.FindControl("hfPracticeSchemeCode"), HiddenField).Value
                'Dim strSubsidizeCode As String = DirectCast(e.Row.FindControl("hfPracticeSubsidizeCode"), HiddenField).Value
                Dim strSchemeCode As String = DirectCast(e.Row.FindControl("lblPracticeSchemeCode"), Label).Attributes("SchemeCode")
                Dim strSubsidizeCode As String = DirectCast(e.Row.FindControl("lblPracticeSubsidizeCode"), Label).Attributes("SubsidizeCode")

                Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = Session(SESS_SPMaintenancePracticeSchemeInfoList)
                Dim udtPracticeSchemeInfo As PracticeSchemeInfoModel = udtPracticeSchemeInfoList.Filter(strSchemeCode, strSubsidizeCode)

                e.Row.Visible = False

                If udtPracticeSchemeInfo Is Nothing Then
                    Return
                End If

                ' Hide the row if not enrolled or not providing service
                'If DirectCast(e.Row.FindControl("hfGIsCategoryHeader"), HiddenField).Value = "Y" Then
                If DirectCast(e.Row.FindControl("lblPracticeSchemeCode"), Label).Attributes("IsCategoryHeader") = "Y" Then
                    For Each udtPSINode As PracticeSchemeInfoModel In udtPracticeSchemeInfoList.Values
                        If udtPSINode.SchemeCode = strSchemeCode Then
                            udtPracticeSchemeInfo = udtPSINode
                            Exit For
                        End If
                    Next

                    If IsNothing(udtPracticeSchemeInfo) Then
                        Return
                    End If

                Else
                    If IsNothing(udtPracticeSchemeInfoList.Filter(strSchemeCode)) Then
                        Return
                    End If

                    ' Check all not provide service
                    Dim blnAllNotProvideService As Boolean = True

                    For Each udtPSINode As PracticeSchemeInfoModel In udtPracticeSchemeInfoList.Filter(strSchemeCode).Values
                        If udtPSINode.ProvideService Then
                            blnAllNotProvideService = False
                            Exit For
                        End If
                    Next

                    If blnAllNotProvideService Then
                        'DirectCast(e.Row.FindControl("hfGAllNotProvideService"), HiddenField).Value = "Y"
                        DirectCast(e.Row.FindControl("lblPracticeSchemeCode"), Label).Attributes.Add("AllNotProvideService", "Y")

                    Else
                        If IsNothing(udtPracticeSchemeInfo) OrElse udtPracticeSchemeInfo.ProvideService = False Then
                            Return

                        End If

                    End If

                End If

                e.Row.Visible = True

                ' Scheme Code
                If udtPracticeSchemeInfo.ClinicType = PracticeSchemeInfoModel.ClinicTypeEnum.NonClinic Then
                    Dim lblPracticeSchemeCode As Label = e.Row.FindControl("lblPracticeSchemeCode")
                    lblPracticeSchemeCode.Text += String.Format("<br />({0})", Me.GetGlobalResourceObject("Text", "NonClinic"))
                End If

                ' Service Fee
                Dim udtSubsidizeGpBO As SubsidizeGroupBackOfficeModel = udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup.Filter(strSchemeCode).SubsidizeGroupBackOfficeList.Filter(strSchemeCode, strSubsidizeCode)

                Dim lblPracticeServiceFee As Label = CType(e.Row.FindControl("lblPracticeServiceFee"), Label)

                If udtPracticeSchemeInfo.ProvideServiceFee.HasValue Then
                    If udtPracticeSchemeInfo.ProvideServiceFee.Value Then
                        lblPracticeServiceFee.Text = udtFormatter.formatMoney(udtPracticeSchemeInfo.ServiceFee, True)

                    Else
                        lblPracticeServiceFee.Text = udtSubsidizeGpBO.ServiceFeeCompulsoryWording

                    End If

                Else
                    If udtSubsidizeGpBO.ServiceFeeEnabled Then
                        lblPracticeServiceFee.Text = "--"
                    Else
                        lblPracticeServiceFee.Text = Me.GetGlobalResourceObject("Text", "ServiceFeeN/A")
                    End If

                End If

                ' Status [Remark]
                Dim lblPracticeSchemeStatus As Label = CType(e.Row.FindControl("lblPracticeSchemeStatus"), Label)
                'Dim hfPracticeSchemeStatusReal As HiddenField = e.Row.FindControl("hfPracticeSchemeStatusReal")

                ' Keep the real status for later use
                'hfPracticeSchemeStatusReal.Value = udtPracticeSchemeInfo.RecordStatus
                lblPracticeSchemeStatus.Attributes.Add("Status", udtPracticeSchemeInfo.RecordStatus)

                Dim intTargetPracticeSeq As Integer = udtPracticeSchemeInfo.PracticeDisplaySeq
                Dim udtTargetPractice As PracticeModel = Nothing

                For Each udtPractice As PracticeModel In gvActionPracticeBank.DataSource
                    If udtPractice.DisplaySeq = intTargetPracticeSeq Then
                        udtTargetPractice = udtPractice
                        Exit For
                    End If
                Next

                lblPracticeSchemeStatus.Text = udtSPProfileBLL.GetPracticeSchemeInfoStatus(udtTargetPractice, strSchemeCode, TableLocation.Permanent)

                Status.GetDescriptionFromDBCode(PracticeSchemeInfoMaintenanceDisplayStatus.ClassCode, lblPracticeSchemeStatus.Text.Trim, lblPracticeSchemeStatus.Text, String.Empty)

                If udtPracticeSchemeInfo.Remark <> String.Empty Then
                    lblPracticeSchemeStatus.Text += " [" + udtPracticeSchemeInfo.Remark + "]"
                End If

                ' Effective Time & Delisting Time
                Dim lblPracticeSchemeEffectiveDtm As Label = CType(e.Row.FindControl("lblPracticeSchemeEffectiveDtm"), Label)
                Dim lblPracticeSchemeDelistDtm As Label = CType(e.Row.FindControl("lblPracticeSchemeDelistDtm"), Label)

                udtSPProfileBLL.GetPracticeSchemeInfoEarliestTime(udtPracticeSchemeInfoList, strSchemeCode,
                                                                  lblPracticeSchemeEffectiveDtm.Text, lblPracticeSchemeDelistDtm.Text)

                If lblPracticeSchemeEffectiveDtm.Text.Equals(String.Empty) Then
                    lblPracticeSchemeEffectiveDtm.Text = Me.GetGlobalResourceObject("Text", "N/A")
                End If

                If lblPracticeSchemeDelistDtm.Text.Equals(String.Empty) Then
                    lblPracticeSchemeDelistDtm.Text = Me.GetGlobalResourceObject("Text", "N/A")
                End If
                ' CRE16-022 (Add optional field "Remarks") [End][Chris YIM]

            Case DataControlRowType.Header
                e.Row.Cells(1).ColumnSpan = 2
                e.Row.Cells(2).Visible = False

                ' Change Header Text: Status -> Status [Remark]
                e.Row.Cells(3).Text = Me.GetGlobalResourceObject("Text", "Status") + " [" + Me.GetGlobalResourceObject("Text", "Remarks") + "]"

        End Select

    End Sub


    Protected Sub gvActionPracticeSchemeInfo_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim gvPracticeSchemeInfo As GridView = sender

        ' Handle Category

        ' CRE16-022 (Add optional field "Remarks") [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        For Each gvr As GridViewRow In gvPracticeSchemeInfo.Rows
            'If DirectCast(gvr.FindControl("hfGIsCategoryHeader"), HiddenField).Value = "Y" Then
            If DirectCast(gvr.FindControl("lblPracticeSchemeCode"), Label).Attributes("IsCategoryHeader") = "Y" Then
                ' Check whether this category is visible
                'Dim strSchemeCode As String = DirectCast(gvr.FindControl("hfPracticeSchemeCode"), HiddenField).Value
                'Dim strCategoryName As String = DirectCast(gvr.FindControl("hfGCategoryName"), HiddenField).Value
                Dim strSchemeCode As String = DirectCast(gvr.FindControl("lblPracticeSchemeCode"), Label).Attributes("SchemeCode")
                Dim strCategoryName As String = DirectCast(gvr.FindControl("lblPracticeSchemeCode"), Label).Attributes("CategoryName")
                Dim blnVisible As Boolean = False

                For Each r As GridViewRow In gvPracticeSchemeInfo.Rows
                    'If DirectCast(r.FindControl("hfGIsCategoryHeader"), HiddenField).Value = "N" _
                    '        AndAlso DirectCast(r.FindControl("hfPracticeSchemeCode"), HiddenField).Value = strSchemeCode _
                    '        AndAlso DirectCast(r.FindControl("hfGCategoryName"), HiddenField).Value = strCategoryName _
                    '        AndAlso r.Visible Then

                    If DirectCast(r.FindControl("lblPracticeSchemeCode"), Label).Attributes("IsCategoryHeader") = "N" _
                            AndAlso DirectCast(r.FindControl("lblPracticeSchemeCode"), Label).Attributes("SchemeCode") = strSchemeCode _
                            AndAlso DirectCast(r.FindControl("lblPracticeSchemeCode"), Label).Attributes("CategoryName") = strCategoryName _
                            AndAlso r.Visible Then

                        blnVisible = True
                        Exit For
                    End If

                Next

                If blnVisible Then
                    'gvr.Cells(1).Text = AntiXssEncoder.HtmlEncode(DirectCast(gvr.FindControl("hfGCategoryName"), HiddenField).Value, True)
                    gvr.Cells(1).Text = AntiXssEncoder.HtmlEncode(DirectCast(gvr.FindControl("lblPracticeSchemeCode"), Label).Attributes("CategoryName"), True)
                    gvr.Cells(1).ColumnSpan = 2
                    gvr.Cells(1).CssClass = "SubsidizeCategoryHeader"
                    gvr.Cells(2).Visible = False

                Else
                    gvr.Visible = False

                End If

            End If

        Next

        ' End of Handle Category

        Dim udtSchemeBackOfficeList As SchemeBackOfficeModelCollection = udtSchemeBackOfficeBLL.GetAllEffectiveSchemeBackOfficeWithSubsidizeGroupFromCache
        Dim strPreviousScheme As String = String.Empty

        For Each gvr As GridViewRow In gvPracticeSchemeInfo.Rows
            If Not gvr.Visible Then
                Continue For
            End If

            'Dim strSchemeCode As String = DirectCast(gvr.FindControl("hfPracticeSchemeCode"), HiddenField).Value
            Dim strSchemeCode As String = DirectCast(gvr.FindControl("lblPracticeSchemeCode"), Label).Attributes("SchemeCode")

            If Not udtSchemeBackOfficeList.Filter(strSchemeCode).DisplaySubsidizeDesc Then
                gvr.Cells(1).ColumnSpan = 2
                gvr.Cells(2).Visible = False
                gvr.Cells(1).Text = Me.GetGlobalResourceObject("Text", "N/A")

            End If

            ' Grouping depends on gridview instead of subsidizelist
            Dim RowCount As Integer = 0

            If Not strPreviousScheme.Equals(strSchemeCode) Then

                For Each gvrow As GridViewRow In gvPracticeSchemeInfo.Rows
                    If gvrow.Visible Then
                        'If DirectCast(gvrow.FindControl("hfPracticeSchemeCode"), HiddenField).Value = strSchemeCode Then
                        If DirectCast(gvrow.FindControl("lblPracticeSchemeCode"), Label).Attributes("SchemeCode") = strSchemeCode Then
                            RowCount += 1
                        End If
                    End If
                Next

                gvr.Cells(0).RowSpan = RowCount
                gvr.Cells(3).RowSpan = RowCount
                gvr.Cells(4).RowSpan = RowCount
                gvr.Cells(5).RowSpan = RowCount

                Dim blnAllNotProvideService As Boolean = False

                For Each gvrow As GridViewRow In gvPracticeSchemeInfo.Rows
                    'If DirectCast(gvrow.FindControl("hfPracticeSchemeCode"), HiddenField).Value = strSchemeCode AndAlso _
                    '    DirectCast(gvrow.FindControl("hfGAllNotProvideService"), HiddenField).Value = "Y" Then
                    If DirectCast(gvrow.FindControl("lblPracticeSchemeCode"), Label).Attributes("SchemeCode") = strSchemeCode AndAlso _
                        DirectCast(gvrow.FindControl("lblPracticeSchemeCode"), Label).Attributes("AllNotProvideService") = "Y" Then

                        blnAllNotProvideService = True
                        Exit For
                    End If

                Next

                If blnAllNotProvideService Then
                    gvr.Cells(1).Text = Me.GetGlobalResourceObject("Text", "NoServiceFeesProvided")
                    gvr.Cells(1).CssClass = "tableText"
                    gvr.Cells(1).RowSpan = RowCount
                    gvr.Cells(1).ColumnSpan = 2
                    gvr.Cells(2).Visible = False
                End If

            Else
                gvr.Cells(0).Visible = False
                gvr.Cells(3).Visible = False
                gvr.Cells(4).Visible = False
                gvr.Cells(5).Visible = False

                Dim blnAllNotProvideService As Boolean = False

                For Each gvrow As GridViewRow In gvPracticeSchemeInfo.Rows
                    'If DirectCast(gvrow.FindControl("hfPracticeSchemeCode"), HiddenField).Value = strSchemeCode AndAlso _
                    '    DirectCast(gvrow.FindControl("hfGAllNotProvideService"), HiddenField).Value = "Y" Then
                    If DirectCast(gvrow.FindControl("lblPracticeSchemeCode"), Label).Attributes("SchemeCode") = strSchemeCode AndAlso _
                        DirectCast(gvrow.FindControl("lblPracticeSchemeCode"), Label).Attributes("AllNotProvideService") = "Y" Then

                        blnAllNotProvideService = True
                        Exit For
                    End If

                Next

                If blnAllNotProvideService Then
                    gvr.Cells(1).Visible = False
                    gvr.Cells(2).Visible = False
                End If

            End If

            strPreviousScheme = strSchemeCode

        Next
        ' CRE16-022 (Add optional field "Remarks") [End][Chris YIM]
    End Sub


    ' Used in .aspx

    Protected Function formatChineseString(ByVal strChineseString) As String
        Return udtFormatter.formatChineseName(strChineseString)
    End Function

    Protected Function formatAddress(ByVal udtAddressModel As Common.Component.Address.AddressModel) As String
        Return udtFormatter.formatAddress(udtAddressModel)
    End Function

    Protected Function formatChiAddress(ByVal udtAddressModel As Common.Component.Address.AddressModel) As String
        Return udtFormatter.formatAddressChi(udtAddressModel)
    End Function

    Protected Function formatAddress(ByVal strRoom As String, ByVal strFloor As String, ByVal strBlock As String, ByVal strBuilding As String, ByVal strDistrict As String, ByVal strArea As String) As String
        Return udtFormatter.formatAddress(strRoom, strFloor, strBlock, strBuilding, strDistrict, strArea)
    End Function

    Protected Function formatDate(ByVal d As Object) As String
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Return udtFormatter.formatDate(Convert.ToDateTime(d))
        Return udtFormatter.formatDisplayDate(Convert.ToDateTime(d))
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
    End Function

    Protected Function GetPracticeTypeName(ByVal strPracticeCode As String) As String
        Dim strPracticeTypeName As String

        If IsNothing(strPracticeCode) Then
            strPracticeTypeName = String.Empty
        Else
            If strPracticeCode.Equals(String.Empty) Then
                strPracticeTypeName = String.Empty
            Else
                If Session("language") = "zh-tw" Then
                    strPracticeTypeName = udtSPProfileBLL.GetPracticeTypeName(strPracticeCode).DataValueChi
                Else
                    strPracticeTypeName = udtSPProfileBLL.GetPracticeTypeName(strPracticeCode).DataValue
                End If
            End If
        End If
        Return strPracticeTypeName
    End Function

    ' Duplicated MO / Practices popup

    Protected Sub ibtnDuplicateMO_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00045, "Show Duplicated MO")

        Dim ibtnDuplicateMO As ImageButton = CType(sender, ImageButton)
        Dim row As GridViewRow = ibtnDuplicateMO.NamingContainer

        If Not row Is Nothing Then
            Dim lblMOIndex As Label = CType(row.FindControl("lblMOIndex"), Label)

            MOPracticeLists1.buildMOObject(udtServiceProviderBLL.GetSP.MOList, CInt(lblMOIndex.Text.Trim), TableLocation.Permanent)

            ModalPopupExtenderDuplicated.Show()

            udtAuditLogEntry.WriteEndLog(LogID.LOG00046, "Show Duplicated MO completed")

        End If
    End Sub

    Protected Sub ibtnDuplicatePractice_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00047, "Show Duplicated Practices")

        Dim ibtnDuplicatePractice As ImageButton = CType(sender, ImageButton)
        Dim row As GridViewRow = ibtnDuplicatePractice.NamingContainer

        If Not row Is Nothing Then
            Dim lblPracticeBankIndex As Label = CType(row.FindControl("lblPracticeBankIndex"), Label)

            MOPracticeLists1.buildPracticeObject(udtServiceProviderBLL.GetSP.PracticeList, CInt(lblPracticeBankIndex.Text.Trim), TableLocation.Permanent)

            Me.ModalPopupExtenderDuplicated.Show()

            udtAuditLogEntry.WriteEndLog(LogID.LOG00048, "Show Duplicated Practices completed")

        End If
    End Sub

    Protected Sub ibtnDuplicatedClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.ModalPopupExtenderDuplicated.Hide()
    End Sub

#Region "Implement IWorkingData (CRE11-004)"

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
        Try
            If IsNothing(Me.udtServiceProviderBLL.GetSP) Then
                Return Nothing
            Else
                Return Me.udtServiceProviderBLL.GetSP
            End If
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

#End Region

#Region "eHS and PCD integration (CRE12-001)"
    ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [Start] (Marco) ---

    Private Function PCDStatusDelistedWarningCheck(ByVal udtSPModel As ServiceProviderModel) As Boolean
        Dim blnRes As Boolean = True
        msgBox.Visible = False

        Dim strHKID As String = String.Empty
        Dim strSPID As String = String.Empty

        strHKID = udtSPModel.HKID
        strSPID = udtSPModel.SPID

        ' Enquire PCD
        Dim objWS As New Common.PCD.PCDWebService(Me.FunctionCode)
        Dim objResult As WebService.Interface.PCDCheckAccountStatusResult = Nothing

        udtAuditLogEntry.AddDescripton("WebMethod", "PCDCheckAccountStatus")
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00306, "CheckPCDAccountStatus Start")
        objResult = objWS.PCDCheckAccountStatus(strHKID)

        udtAuditLogEntry.AddDescripton("ReturnCode", objResult.ReturnCode.ToString)
        udtAuditLogEntry.AddDescripton("MessageID", objResult.MessageID.ToString)

        Select Case objResult.ReturnCode
            Case WebService.Interface.PCDCheckAccountStatusResult.enumReturnCode.Success
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00307, "CheckPCDAccountStatus Success")

                If strSPID <> String.Empty Then
                    Dim udtHCVUUser As HCVUUserModel
                    Dim udtHCVUUserBLL As New HCVUUserBLL
                    udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

                    Dim strMessage As String = String.Empty

                    If objResult.UpdateJoinPCDStatus(strSPID, udtHCVUUser.UserID, strMessage, udtSPModel) Then
                        UpdatePCDStatusLabel(udtSPModel)
                    Else
                        Throw New Exception(strMessage)
                    End If

                End If

                If objResult.AccountStatus = PCDCheckAccountStatusResult.enumAccountStatus.Delisted Then
                    Me.ucPCDWarningPopup.Build(ucPCDWarningPopup.WarningType.Delisted)
                    Me.ModalPopupExtenderPCDWarning.Show()

                    blnRes = False
                End If

            Case Else
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00308, "CheckPCDAccountStatus Fail")
                ' PCD service is temporary not available. Please try again later!
                Me.msgBox.AddMessage(objResult.SystemMessage)
                Me.msgBox.BuildMessageBox("ValidationFail")

                ShowNoticePopupForConnectionFail(objResult.SystemMessage)

                blnRes = False

        End Select

        Return blnRes
    End Function

    Protected Sub ucPCDWarningPopup_SuccessClick(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ucPCDWarningPopup.Success_Click

        Dim udtSPModel As ServiceProviderModel = (New ServiceProviderBLL).GetSP

        If udtSPModel IsNot Nothing Then
            JoinPCDInvokeWS(udtSPModel)
        End If

    End Sub

    Protected Sub ucPCDWarningPopup_FailureClick(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ucPCDWarningPopup.Failure_Click
        Me.ModalPopupExtenderPCDWarning.Show()
    End Sub

    Private Sub JoinPCDInvokeWS(ByVal udtSPModel As ServiceProviderModel)
        ' Check SP exist in PCD
        Me.ucTypeOfPracticePopup.InvokePCD_CheckExist()

        Select Case Me.ucTypeOfPracticePopup.ExistPCDResult.ReturnCode
            Case Common.PCD.WebService.Interface.PCDCheckAvailableForVerifiedEnrolmentResult.enumReturnCode.Available
                ' Display Type of Practice Popup
                ShowJoinPCDPanel(True)
            Case Common.PCD.WebService.Interface.PCDCheckAvailableForVerifiedEnrolmentResult.enumReturnCode.ServiceProviderAlreadyExisted, _
                  Common.PCD.WebService.Interface.PCDCheckAvailableForVerifiedEnrolmentResult.enumReturnCode.EnrolmentAlreadyExisted, _
                  Common.PCD.WebService.Interface.PCDCheckAvailableForVerifiedEnrolmentResult.enumReturnCode.VerifiedEnrolmentAlreadyExisted
                ShowNoticePopupForExistPCD(Me.ucTypeOfPracticePopup.ExistPCDResult.SystemMessage)
            Case Common.PCD.WebService.Interface.PCDCheckAvailableForVerifiedEnrolmentResult.enumReturnCode.InvalidParameter, _
                Common.PCD.WebService.Interface.PCDCheckAvailableForVerifiedEnrolmentResult.enumReturnCode.DataValidationFail, _
                Common.PCD.WebService.Interface.PCDCheckAvailableForVerifiedEnrolmentResult.enumReturnCode.ErrorAllUnexpected, _
                Common.PCD.WebService.Interface.PCDCheckAvailableForVerifiedEnrolmentResult.enumReturnCode.CommunicationLinkError, _
                Common.PCD.WebService.Interface.PCDCheckAvailableForVerifiedEnrolmentResult.enumReturnCode.AuthenticationFailed

                udtAuditLogEntry.WriteEndLog(LogID.LOG00111, "Join PCD - Invoke PCD End [Fail]")

                ShowNoticePopupForConnectionFail(Me.ucTypeOfPracticePopup.ExistPCDResult.SystemMessage)
            Case Else
                ShowNoticePopupForConnectionFail(Me.ucTypeOfPracticePopup.ExistPCDResult.SystemMessage)
        End Select
    End Sub

    Public Sub UpdatePCDStatusLabel(ByVal udtSP As ServiceProviderModel)
        Dim ctrlLblPCDStatus As Label = uplPCDRecordStatus.FindControl("lblPCDStatus")
        Dim ctrlLblPCDProfessional As Label = uplPCDRecordStatus.FindControl("lblPCDProfessional")

        If udtSP.PCDAccountStatus = PCDAccountStatus.Unavailable Then
            trPCDStatus.Visible = False
            trPCDProfessional.Visible = False
        Else
            trPCDStatus.Visible = True
            trPCDProfessional.Visible = True

            ' PCD Status
            Dim strOutputText As String = String.Empty

            If udtSP.PCDStatusLastCheckDtm IsNot Nothing Then
                strOutputText = String.Format("{0} ({1})", Common.PCD.WebService.Interface.PCDCheckAccountStatusResult.GetPCDStatusDescByValue(udtSP.PCDAccountStatus, udtSP.PCDEnrolmentStatus), Me.GetGlobalResourceObject("Text", "PCDStatusLastCheck"))
                strOutputText = strOutputText.Replace("%s", udtFormatter.convertDate(udtSP.PCDStatusLastCheckDtm))
            Else
                strOutputText = Common.PCD.WebService.Interface.PCDCheckAccountStatusResult.GetPCDStatusDescByValue(udtSP.PCDAccountStatus, udtSP.PCDEnrolmentStatus)
            End If

            ctrlLblPCDStatus.Text = strOutputText

            ' PCD Professional
            ctrlLblPCDProfessional.Text = Common.PCD.WebService.Interface.PCDCheckAccountStatusResult.GetPCDProfessionalDescByValue(udtSP.PCDProfessional)

        End If
        uplPCDRecordStatus.Update()
        uplPCDProfessional.Update()
    End Sub

    ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [End]   (Marco) ---

    'CRE20-006 DHC integration [Start][Nichole]
    Public Sub DisplayEnrolledDHCStatus(ByVal strSPID As String)
        
        'show SP Enrolled DHC status
        lblEnrolledDHCSP.Text = udtServiceProviderBLL.GetSPEnrolledDHC(strSPID)

        If lblEnrolledDHCSP.Text Is String.Empty Then
            ' trEnrolledDHC.Visible = False
            lblEnrolledDHCSP.Text = Me.GetGlobalResourceObject("Text", "N/A")
        End If

    End Sub
    'CRE20-006 DHC integration [End][Nichole]

    ' CRE12-001 eHS and PCD integration [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    Private Sub ibtnJoinPCD_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnJoinPCD.Click
        Me.udtAuditLogEntry.WriteLog(LogID.LOG00112, "Join PCD button click")

        Dim udtSPBLL As New ServiceProviderBLL
        ' Retrieve SP
        Dim udtSPModel As ServiceProviderModel = udtSPProfileBLL.GetServiceProviderPermanentProfileWithMaintenance(hfSPID.Value.Trim)
        udtSPBLL.SaveToSession(udtSPModel)

        ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [Start] (Marco) ---
        If PCDStatusDelistedWarningCheck(udtSPModel) Then
            JoinPCDInvokeWS(udtSPModel)
        End If
        ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [End]   (Marco) ---
    End Sub


    Private Sub ShowJoinPCDPanel(ByVal blnShow As Boolean)
        Me.udtAuditLogEntry.WriteLog(LogID.LOG00113, "Show Join PCD Popup")

        Me.ucTypeOfPracticePopup.Reset()
        Me.ucTypeOfPracticePopup.Showing = blnShow
        Me.ucTypeOfPracticePopup.Mode = ucTypeOfPracticeGrid.EnumMode.Transfer
        ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------        
        Dim udtSP As ServiceProviderModel = (New ServiceProviderBLL).GetSP
        Dim udtPracticeList As PracticeModelCollection = udtSP.PracticeList.FilterByPCD(TableLocation.Permanent)
        Me.ucTypeOfPracticePopup.LoadPractice(udtPracticeList)
        ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]
        Me.ModalPopupExtenderTypeOfPractice.PopupDragHandleControlID = Me.ucTypeOfPracticePopup.Header.ClientID
        Me.ModalPopupExtenderTypeOfPractice.Show()
    End Sub

    Private Sub ShowNoticePopupForJoinPCD()
        Me.udtAuditLogEntry.WriteLog(LogID.LOG00115, "Show join PCD result Popup")

        Me.ucTypeOfPracticePopup.Showing = False
        Me.ModalPopupExtenderTypeOfPractice.Hide()

        Select Case ucTypeOfPracticePopup.JoinPCDResult.ReturnCode
            Case Common.PCD.WebService.Interface.PCDUploadVerifiedEnrolmentResult.enumReturnCode.UploadedSuccessfully, _
                 Common.PCD.WebService.Interface.PCDUploadVerifiedEnrolmentResult.enumReturnCode.ServiceProviderAlreadyExisted, _
                 Common.PCD.WebService.Interface.PCDUploadVerifiedEnrolmentResult.enumReturnCode.EnrolmentAlreadyExisted, _
                 Common.PCD.WebService.Interface.PCDUploadVerifiedEnrolmentResult.enumReturnCode.VerifiedEnrolmentAlreadyExisted

                Me.ucNoticePopup.NoticeMode = HCVU.ucNoticePopUp.enumNoticeMode.Notification
            Case Common.PCD.WebService.Interface.PCDCheckIsActiveSPResult.enumReturnCode.InvalidParameter, _
                 Common.PCD.WebService.Interface.PCDCheckAvailableForVerifiedEnrolmentResult.enumReturnCode.DataValidationFail, _
                 Common.PCD.WebService.Interface.PCDCheckIsActiveSPResult.enumReturnCode.ErrorAllUnexpected, _
                 Common.PCD.WebService.Interface.PCDCheckIsActiveSPResult.enumReturnCode.CommunicationLinkError, _
                 Common.PCD.WebService.Interface.PCDCheckIsActiveSPResult.enumReturnCode.AuthenticationFailed

                Me.ucNoticePopup.NoticeMode = HCVU.ucNoticePopUp.enumNoticeMode.Custom
                Me.ucNoticePopup.ButtonMode = HCVU.ucNoticePopUp.enumButtonMode.OK
                Me.ucNoticePopup.IconMode = HCVU.ucNoticePopUp.enumIconMode.ExclamationIcon
                Me.ucNoticePopup.HeaderText = Me.GetGlobalResourceObject("Text", "Notification")
            Case Else
                Throw New Exception(String.Format("[spMaintenance] PCDUploadVerifiedEnrolment unhandled return code ({0})", Me.ucTypeOfPracticePopup.JoinPCDResult.ReturnCode.ToString()))
        End Select

        Me.ucNoticePopup.MessageText = ucTypeOfPracticePopup.JoinPCDResult.ReturnCodeDesc
        Me.ModalPopupExtenderNotice.PopupDragHandleControlID = Me.ucNoticePopup.Header.ClientID
        Me.ModalPopupExtenderNotice.Show()
    End Sub

    Private Sub ShowNoticePopupForExistPCD(ByVal objSystemMessage As SystemMessage)
        Me.udtAuditLogEntry.WriteLog(LogID.LOG00114, "Show PCD account exist Popup")

        Me.ucTypeOfPracticePopup.Showing = False
        Me.ModalPopupExtenderTypeOfPractice.Hide()

        Me.ucNoticePopup.NoticeMode = HCVU.ucNoticePopUp.enumNoticeMode.Notification
        Me.ucNoticePopup.MessageText = objSystemMessage.GetMessage 'Me.GetGlobalResourceObject("Text", "PCDAccountExist_Short")
        Me.ModalPopupExtenderNotice.PopupDragHandleControlID = Me.ucNoticePopup.Header.ClientID
        Me.ModalPopupExtenderNotice.Show()
    End Sub


    Private Sub ShowNoticePopupForConnectionFail(ByVal objSystemMessage As SystemMessage)
        Me.udtAuditLogEntry.WriteLog(LogID.LOG00116, "Show connection popup Popup")

        Me.ucTypeOfPracticePopup.Showing = False
        Me.ModalPopupExtenderTypeOfPractice.Hide()

        Me.ucNoticePopup.NoticeMode = HCVU.ucNoticePopUp.enumNoticeMode.Custom
        Me.ucNoticePopup.ButtonMode = HCVU.ucNoticePopUp.enumButtonMode.OK
        Me.ucNoticePopup.IconMode = HCVU.ucNoticePopUp.enumIconMode.ExclamationIcon
        Me.ucNoticePopup.HeaderText = Me.GetGlobalResourceObject("Text", "Notification")
        Me.ucNoticePopup.MessageText = objSystemMessage.GetMessage ' Me.GetGlobalResourceObject("Text", "PCDServiceUnavailable")
        Me.ModalPopupExtenderNotice.PopupDragHandleControlID = Me.ucNoticePopup.Header.ClientID
        Me.ModalPopupExtenderNotice.Show()
    End Sub

    Private Sub ShowEnrolmentCopyPopup()
        'Me.udtAuditLogEntry.WriteLog(LogID.LOG00116, "Show connection popup Popup")

        Me.ucTypeOfPracticePopup.Showing = True
        Me.ModalPopupExtenderTypeOfPractice.Show()
        Me.ModalPopupExtenderEnrolmentCopy.PopupDragHandleControlID = Me.ucEnrolmentCopyPopup.Header.ClientID
        Me.ModalPopupExtenderEnrolmentCopy.Show()
        Me.ucEnrolmentCopyPopup.LoadRecord()
    End Sub

    Private Sub ucTypeOfPracticePopup_ButtonClick(ByVal e As ucTypeOfPracticePopup.enumButtonClick) Handles ucTypeOfPracticePopup.ButtonClick
        Select Case e
            Case HCVU.ucTypeOfPracticePopup.enumButtonClick.Cancel

                Me.ucTypeOfPracticePopup.Showing = False
                Me.ModalPopupExtenderTypeOfPractice.Hide()

            Case HCVU.ucTypeOfPracticePopup.enumButtonClick.CreatePCDAccount
                ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [Start] (Marco) ---
                Dim objWS As New Common.PCD.PCDWebService(Me.FunctionCode)
                Dim objResult As WebService.Interface.PCDCheckAccountStatusResult
                Dim strMessage As String = String.Empty
                Dim udtHCVUUser As HCVUUserModel
                Dim udtHCVUUserBLL As New HCVUUserBLL
                udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

                Dim udtSPModel As ServiceProviderModel = (New ServiceProviderBLL).GetSP
                objResult = objWS.PCDCheckAccountStatus(udtSPModel.HKID)

                If objResult.UpdateJoinPCDStatus(udtSPModel.SPID, udtHCVUUser.UserID, strMessage, udtSPModel) Then
                    UpdatePCDStatusLabel(udtSPModel)
                Else
                    Throw New Exception(strMessage)
                End If
                ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [End]   (Marco) ---

                ShowNoticePopupForJoinPCD()

            Case HCVU.ucTypeOfPracticePopup.enumButtonClick.ERN
                ShowEnrolmentCopyPopup()
            Case Else
                Throw New Exception(String.Format("[spMaintenance] Unhandled ucTypeOfPracticePopup button click ({0})", e.ToString()))

        End Select
    End Sub

    Private Sub ucEnrolmentCopyPopup_ButtonClick(ByVal e As ucEnrolmentCopyPopup.enumButtonClick) Handles ucEnrolmentCopyPopup.ButtonClick
        Select Case e
            Case HCVU.ucEnrolmentCopyPopup.enumButtonClick.Close
                Me.ucTypeOfPracticePopup.Showing = True
                Me.ModalPopupExtenderTypeOfPractice.Show()
            Case Else
                Throw New Exception(String.Format("[spMaintenance] Unhandled ucEnrolmentCopyPopup button click ({0})", e.ToString()))
        End Select

    End Sub

    ' CRE12-001 eHS and PCD integration [End][Koala]
#End Region


End Class

Class SPMaintenancePopupStatus
    Public Const Closed As Integer = 0
    Public Const ResendEmail As Integer = 10
    Public Const ResendActivationEmail As Integer = 11
    Public Const ResendSchemeEnrolmentEmail As Integer = 12
    Public Const ResendDelistEmail As Integer = 13
    Public Const ReprintLetter As Integer = 20
    Public Const ReprintAcknowledgementLetter As Integer = 21
    Public Const ReprintSchemeEnrolmentLetter As Integer = 22
    ' CRE13-029 - RSA Server Upgrade [Start][Lawrence]
    Public Const CheckToken As Integer = 31
    Public Const RepairToken As Integer = 32
    ' CRE13-029 - RSA Server Upgrade [End][Lawrence]
End Class
