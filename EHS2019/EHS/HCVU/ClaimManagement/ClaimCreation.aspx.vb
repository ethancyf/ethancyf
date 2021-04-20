Imports AjaxControlToolkit
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.ClaimRules
Imports Common.Component.DocType
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.HAServicePatient
Imports Common.Component.HATransaction
Imports Common.Component.HCVUUser
Imports Common.Component.InputPicker
Imports Common.Component.ReasonForVisit
Imports Common.Component.Scheme
Imports Common.Component.SortedGridviewHeader
Imports Common.Component.StaticData
Imports Common.Component.UserRole
Imports Common.Component.VoucherInfo
Imports Common.Format
Imports Common.Validation
Imports Common.WebService.Interface
Imports System.Web.Services

<System.Web.Script.Services.ScriptService()> _
Partial Public Class ClaimCreation
    Inherits BasePageWithGridView

#Region "Private Variables"
    Private udtDocTypeBLL As New DocTypeBLL
    Private udtEHSTransactionBLL As New EHSTransactionBLL
    Private udtFormatter As New Formatter
    Private udtGeneralFunction As New GeneralFunction
    Private udtHCVUUserBLL As New HCVUUserBLL
    Private udtSchemeClaimBLL As New SchemeClaimBLL
    Private udtSPProfileBLL As New SPProfileBLL
    Private udtStaticDataBLL As New StaticDataBLL
    Private udtValidator As New Validator
    Private udtSessionHandlerBLL As New BLL.SessionHandlerBLL
    Private udtEHSClaimBLL As New BLL.EHSClaimBLL
    Private udtEHSAccountMaintBLL As New eHSAccountMaintBLL

    Private udtSM As Common.ComObject.SystemMessage
    Private udtAuditLogEntry As AuditLogEntry

#End Region

#Region "Constants"

    Private Class AuditLogDescription
        Public Const Load As String = "Claim Creation Load" '00000

        ' New Claim Transaction
        ' - Search SP
        Public Const NewClaimTransaction_SearchSP As String = "New Claim Transaction - Search SP click" '00001
        Public Const NewClaimTransaction_SearchSP_Success As String = "New Claim Transaction - Search SP Successful" '00002
        Public Const NewClaimTransaction_SearchSP_Fail As String = "New Claim Transaction - Search SP Fail" '00003
        Public Const NewClaimTransaction_SearchSP_Clear As String = "New Claim Transaction - Search SP - Clear click" '00004

        ' - Advanced Search SP
        Public Const NewClaimTransaction_AdvancedSearchSP As String = "New Claim Transaction - Advanced Search SP click" '00005
        Public Const NewClaimTransaction_AdvancedSearchSP_Success As String = "New Claim Transaction - Advanced Search SP Successful" '00006
        Public Const NewClaimTransaction_AdvancedSearchSP_Fail As String = "New Claim Transaction - Advanced Search SP Fail" '00007
        Public Const NewClaimTransaction_AdvancedSearchSP_Close As String = "New Claim Transaction - Advanced Search SP - Close click" '00008

        ' - Advanced Select SP
        Public Const NewClaimTransaction_AdvancedSelectSP As String = "New Claim Transaction - Advanced Select SP" '00009
        Public Const NewClaimTransaction_AdvancedSelectSP_Back As String = "New Claim Transaction - Advanced Select SP - Back click" '00010

        ' - Enter Creation Detail
        Public Const NewClaimTransaction_EnterCreationDetail As String = "New Claim Transaction - Enter Creation Detail - New Claim Transaction click" '00011
        Public Const NewClaimTransaction_EnterCreationDetail_Success As String = "New Claim Transaction - Enter Creation Detail Successful" '00012
        Public Const NewClaimTransaction_EnterCreationDetail_Fail As String = "New Claim Transaction - Enter Creation Detail Fail" '00013

        ' - Search Account
        Public Const NewClaimTransaction_SearchAccountClick As String = "New Claim Transaction - Search Account" '00014
        Public Const NewClaimTransaction_SearchAccountClick_Success As String = "New Claim Transaction - Search Account Successful" '00015
        Public Const NewClaimTransaction_SearchAccountClick_Fail As String = "New Claim Transaction - Search Account Fail" '00016

        ' - Select Account
        Public Const NewClaimTransaction_SelectAccount = "New Claim Transaction - Select Account" '00017
        Public Const NewClaimTransaction_SelectAccount_Fail = "New Claim Transaction - Select Account Fail" '00041
        Public Const NewClaimTransaction_SelectAccount_Back = "New Claim Transaction - Select Account - Back Click" '00018

        ' - Vaccination Record
        Public Const HAConnectionFail As String = "Fail to obtain HA vaccine result " '00019
        Public Const DHConnectionFail As String = "Fail to obtain DH vaccine result " '00020
        Public Const HADHConnectionFail As String = "Fail to obtain HA & DH vaccine result " '00021
        Public Const VaccinationRecordClick As String = "Vaccination record button click" '00022
        Public Const VaccinationRecordCloseClick As String = "Vaccination record close button click" '00023

        ' - Change Service Date
        Public Const NewClaimTransaction_ChangeServiceDate As String = "New Claim Transaction - Change Service Date" '00024
        Public Const NewClaimTransaction_ChangeServiceDate_Success As String = "New Claim Transaction - Change Service Date Successful" '00025
        Public Const NewClaimTransaction_ChangeServiceDate_Fail As String = "New Claim Transaction - Change Service Date Fail" '00026

        ' - Enter Claim Detail
        Public Const NewClaimTransaction_EnterClaimDetail As String = "New Claim Transaction - Enter Claim Detail - Save click" '00027
        Public Const NewClaimTransaction_EnterClaimDetail_Success As String = "New Claim Transaction - Enter Claim Detail Successful" '00028
        Public Const NewClaimTransaction_EnterClaimDetail_Fail As String = "New Claim Transaction - Enter Claim Detail Fail" '00029
        Public Const NewClaimTransaction_EnterClaimDetail_Back As String = "New Claim Transaction - Enter Claim Detail - Back click" '00030
        Public Const NewClaimTransaction_EnterClaimDetail_WarningMsg As String = "New Claim Transaction - Enter Claim Detail - Warning Msg" '00031

        ' - Enter Override Reason
        Public Const NewClaimTransaction_EnterOverrideReason As String = "New Claim Transaction - Enter Override Reason - Confirm click" '00032
        Public Const NewClaimTransaction_EnterOverrideReason_Success As String = "New Claim Transaction - Enter Override Reason Successful" '00033
        Public Const NewClaimTransaction_EnterOverrideReason_Fail As String = "New Claim Transaction - Enter Override Reason Fail" '00034
        Public Const NewClaimTransaction_EnterOverrideReason_Cancel As String = "New Claim Transaction - Enter Override Reason - Cancel click" '00035

        ' - Confirm Claim Transaction Creation
        Public Const NewClaimTransaction_ConfirmClaim As String = "New Claim Transaction - Confirm Claim - Confirm click" '00036
        Public Const NewClaimTransaction_ConfirmClaim_Success As String = "New Claim Transaction - Confirm Claim Successful" '00037
        Public Const NewClaimTransaction_ConfirmClaim_Fail As String = "New Claim Transaction - Confirm Claim Fail" '00038
        Public Const NewClaimTransaction_ConfirmClaim_Cancel As String = "New Claim Transaction - Confirm Claim - Cancel click" '00039

        ' - Complete Claim Transaction Creation
        Public Const NewClaimTransaction_CompleteReturn As String = "New Claim Transaction - Complete Claim - Return click" '00040

    End Class

    Private Class ViewIndex
        Public Const ViewNewClaimTransaction As Integer = 0
    End Class

    Private Class ErrorMessageBoxHeaderKey
        Public Const SearchFail As String = "SearchFail"
        Public Const ValidationFail As String = "ValidationFail"
        Public Const UpdateFail As String = "UpdateFail"
        Public Const ValidationWarning As String = "Warning"
        Public Const ConnectionFail As String = "ConnectionFail"
    End Class

    Private Class NewClaimTransaction
        Public Const SearchAccountResults As Integer = 0
        Public Const EnterClaimDetails As Integer = 1
        Public Const Confirm As Integer = 2
        Public Const Complete As Integer = 3
    End Class

    Private Class InputTransactionDetails
        Public Const CreationDetails = 0
        Public Const ClaimDetails = 1
    End Class

    Private Class VaccinationRecordPopupStatusClass
        Public Const Active As String = "A"
    End Class

    Private Class VaccinationRecordPopupShownClass
        Public Const Active As String = "A"
    End Class

    Private Class VS
        Public Const VaccinationRecordPopupStatus As String = "VaccinationRecordPopupStatus"
        Public Const VaccinationRecordPopupShown As String = "VaccinationRecordPopupShown"
    End Class

    Private Class AccountType
        Public Const Validated As String = "Validated"
        Public Const Temporary As String = "Temporary"
    End Class

#Region "Session Constants"
    Private Const SESS_SearchAccount As String = "010418_SearchAccountDataTable"
    Private Const SESS_AdvancedSearchSP As String = "010418_AdvancedSearchSP"
    Private Const SESS_ServiceProvider As String = "010418_ServiceProvider"
    Private Const SESS_SearchRVPHomeList As String = "010418_SearchRVPHomeList"
    Private Const SESS_SearchSchoolList As String = "010418_SearchSchoolList"
    Private Const SESS_SearchOutreachList As String = "010418_SearchOutreachList"

#End Region

#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        If Not IsPostBack Then
            FunctionCode = FunctCode.FUNT010418

            ' Set the gridview page size
            Dim strParmValue As String = String.Empty
            Dim intPageSize As Integer = udtGeneralFunction.GetPageSize() ' CRE11-007

            gvSearchAccount.PageSize = intPageSize

            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, AuditLogDescription.Load)

            ' eHealth Account Prefix
            Dim strParm1 As String = String.Empty
            Dim strParm2 As String = String.Empty
            If udtGeneralFunction.getSystemParameter("eHealthAccountPrefix", strParm1, strParm2) Then
                'lblTabeHSAccountIDPrefix.Text = strParm1
                'lblTabAdvancedSearchAccountIDPrefix.Text = strParm1
            Else
                Throw New ArgumentNullException("Parameter: eHealthAccountPrefix not found")
            End If

            Me.mvNewClaimTransaction.ActiveViewIndex = NewClaimTransaction.EnterClaimDetails
            Me.mvEnterDetails.ActiveViewIndex = InputTransactionDetails.CreationDetails

            ' Initial setting for claim creation
            InitialEnterCreationDetail()

            ' Turn On / Off the making new claim function
            Dim strTurnOnNewClaim As String = String.Empty
            udtGeneralFunction.getSystemParameter("TurnOnOutsidePaymentClaim", strTurnOnNewClaim, String.Empty)

            If strTurnOnNewClaim.Trim.Equals("Y") Then
                'Me.ibtnNewClaimTransaction.Visible = True
            Else
                'Me.ibtnNewClaimTransaction.Visible = False
                Me.ibtnSearchSP.Enabled = False
                Me.ibtnClearSearchSP.Enabled = False
                Me.ibtnNewClaimTransaction.Enabled = False
                udcInfoMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT010418, SeverityCode.SEVI, MsgCode.MSG00001))
                udcInfoMessageBox.BuildMessageBox()
            End If

            ' Prevent double-click 
            Dim strBrowser As String = String.Empty
            Try
                If Not HttpContext.Current.Request.Browser Is Nothing Then
                    strBrowser = HttpContext.Current.Request.Browser.Type
                End If
            Catch ex As Exception
                strBrowser = String.Empty
            End Try

            MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnConfirmClaimCreationConfirm)
            MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnWarningMessageConfirm)
            MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnWarningMessageCancel)

            '----------------------------------
            ' Initial session variables
            '----------------------------------
            ' Clear the eVaccination Record
            Dim udtSession As New BLL.SessionHandlerBLL()
            udtSession.CMSVaccineResultRemoveFromSession(FunctionCode)
            ViewState.Remove(VS.VaccinationRecordPopupStatus)
            ViewState.Remove(VS.VaccinationRecordPopupShown)

            ' Clear related session
            ClearSessionForNewClaim(True)

            'HandleRedirectAction()

        Else
            If Me.mvNewClaimTransaction.ActiveViewIndex = NewClaimTransaction.EnterClaimDetails Then
                If mvEnterDetails.ActiveViewIndex = InputTransactionDetails.ClaimDetails Then
                    Dim udteHSAccountMaintBLL As New eHSAccountMaintBLL
                    Dim udtEHSAccount As EHSAccount.EHSAccountModel

                    udtEHSAccount = udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)

                    If Not IsNothing(udtEHSAccount) Then
                        BindPersonalInfo(udtEHSAccount)
                    End If

                    If Not IsNothing(ViewState(VS.VaccinationRecordPopupStatus)) _
                            AndAlso ViewState(VS.VaccinationRecordPopupStatus) = VaccinationRecordPopupStatusClass.Active Then
                        popupVaccinationRecord.Show()
                        ucVaccinationRecord.BuildEHSAccount(udtEHSAccount)
                    End If

                    SetUpEnterClaimDetails(True)
                End If

            ElseIf Me.mvNewClaimTransaction.ActiveViewIndex = NewClaimTransaction.Confirm Then
                Dim udtEHSTransaction As EHSTransactionModel
                udtEHSTransaction = Me.udtSessionHandlerBLL.EHSTransactionGetFromSession(FunctionCode)
                Me.udcConfirmClaimCreation.EnableVaccinationRecordChecking = False
                Me.udcConfirmClaimCreation.LoadTranInfo(udtEHSTransaction, New DataTable())

            End If

        End If

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Select Case Me.mvEnterDetails.ActiveViewIndex
            Case InputTransactionDetails.ClaimDetails
                ScriptManager1.SetFocus(ibtnEnterClaimDetailBack)

        End Select

    End Sub

#Region "View Change Events"
    Private Sub mvNewClaimTransaction_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mvNewClaimTransaction.ActiveViewChanged
        If mvNewClaimTransaction.ActiveViewIndex <> NewClaimTransaction.Confirm And Me.mvNewClaimTransaction.ActiveViewIndex <> NewClaimTransaction.Complete Then
            Me.udcInfoMessageBox.Visible = False
            Me.udcMessageBox.Visible = False
        End If
    End Sub

    Private Sub mvEnterDetails_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mvEnterDetails.ActiveViewChanged
        Select Case mvEnterDetails.ActiveViewIndex
            Case InputTransactionDetails.CreationDetails
                ClearEnterCreationDetailsErrorImage()
        End Select
    End Sub

#End Region

#End Region

#Region "Claim Creation"

#Region "Claim Creation Events"
    Protected Sub ddlEnterCreationDetailPractice_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlEnterCreationDetailPractice.SelectedIndexChanged
        If Me.ddlEnterCreationDetailPractice.SelectedValue.Trim.Equals(String.Empty) Then
            Me.lblEnterCreationDetailPracticeStatus.Text = String.Empty

            Me.ddlEnterCreationDetailScheme.Items.Clear()
            Me.ddlEnterCreationDetailScheme.Enabled = False
            Me.imgEnterCreationDetailScheme.Visible = False

        Else
            Dim udtPracticeDisplayList As Practice.PracticeBLL.PracticeDisplayModelCollection
            Dim udtPracticeDisplay As Practice.PracticeBLL.PracticeDisplayModel

            udtPracticeDisplayList = Me.udtSessionHandlerBLL.PracticeDisplayListGetFromSession(FunctionCode)
            udtPracticeDisplay = udtPracticeDisplayList.Filter(CInt(Me.ddlEnterCreationDetailPractice.SelectedValue.Trim))

            If udtPracticeDisplay.PracticeStatus.Trim.Equals(PracticeStatus.Active) Then
                Me.lblEnterCreationDetailPracticeStatus.Text = String.Empty
            Else
                Status.GetDescriptionFromDBCode(PracticeStatus.ClassCode, udtPracticeDisplay.PracticeStatus, Me.lblEnterCreationDetailPracticeStatus.Text, String.Empty)
                Me.lblEnterCreationDetailPracticeStatus.Text = " (" + Me.lblEnterCreationDetailPracticeStatus.Text + ")"
            End If

            Me.ddlEnterCreationDetailScheme.Items.Clear()
            Me.ddlEnterCreationDetailScheme.Enabled = True

            Me.BindEnterCreationDetailScheme()

        End If

    End Sub

    Protected Sub ddlEnterCreationDetailScheme_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlEnterCreationDetailScheme.SelectedIndexChanged
        Dim udtSchemeClaimList As SchemeClaimModelCollection
        Dim udtSchemeClaim As SchemeClaimModel

        udtSchemeClaimList = Me.udtSessionHandlerBLL.SchemeListGetFromSession(FunctionCode)
        udtSchemeClaim = udtSchemeClaimList.Filter(Me.ddlEnterCreationDetailScheme.SelectedValue.Trim)

        lblEnterCreationDetailSchemeStatus.Visible = False
        lblEnterCreationDetailSchemeStatus.Text = String.Empty
        Dim ddlScheme As DropDownList = CType(sender, DropDownList)

        If Not ddlScheme Is Nothing Then
            If ddlScheme.SelectedIndex <> 0 Then
                Dim strSchemeCodeEnrol As String = New SchemeClaimBLL().ConvertSchemeEnrolFromSchemeClaimCode(ddlScheme.SelectedValue.Trim)

                Dim udtPracticeSchemeInfoBLL As New PracticeSchemeInfo.PracticeSchemeInfoBLL
                Dim udtPracticeSchemeInfoModelCollection As PracticeSchemeInfo.PracticeSchemeInfoModelCollection = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListPermanentBySPIDPracticeDisplaySeq(Me.txtEnterCreationDetailSPID.Text.Trim, CInt(Me.ddlEnterCreationDetailPractice.SelectedValue), New Common.DataAccess.Database)
                Dim udtResPracticeSchemeInfoModelCollection As PracticeSchemeInfo.PracticeSchemeInfoModelCollection = udtPracticeSchemeInfoModelCollection.FilterByPracticeScheme(CInt(Me.ddlEnterCreationDetailPractice.SelectedValue), strSchemeCodeEnrol)

                If Not udtResPracticeSchemeInfoModelCollection Is Nothing Then
                    For Each udtPracticeSchemeInfoModel As PracticeSchemeInfo.PracticeSchemeInfoModel In udtResPracticeSchemeInfoModelCollection.Values
                        'If one of practice scheme is delisted, the label "delisted" will be shown.
                        If udtPracticeSchemeInfoModel.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.DelistedInvoluntary Or _
                            udtPracticeSchemeInfoModel.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.DelistedVoluntary Then
                            Status.GetDescriptionFromDBCode(PracticeStatus.ClassCode, PracticeSchemeInfoStatus.Delisted, Me.lblEnterCreationDetailSchemeStatus.Text, String.Empty)
                            lblEnterCreationDetailSchemeStatus.Text = "(" + lblEnterCreationDetailSchemeStatus.Text + ")"
                            lblEnterCreationDetailSchemeStatus.Visible = True
                        End If
                    Next

                    If udtResPracticeSchemeInfoModelCollection.IsNonClinic Then
                        Me.panEnterCreationDetailNonClinicSetting.Visible = True
                        Me.lblEnterCreationDetailNonClinicSetting.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting"))
                        udtSessionHandlerBLL.NonClinicSettingSaveToSession(True, FunctionCode)
                    Else
                        Me.panEnterCreationDetailNonClinicSetting.Visible = False
                        udtSessionHandlerBLL.NonClinicSettingSaveToSession(False, FunctionCode)
                    End If

                End If

            End If
        End If

    End Sub

#Region "SP Search"
    Protected Sub ibtnSearchSP_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim objAuditLogInfo As AuditLogInfo = Nothing
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        'Dim udtEHSAccount As EHSAccount.EHSAccountModel
        'Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
        'udtEHSAccount = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)

        ' CRE1-004
        objAuditLogInfo = New AuditLogInfo(Me.txtEnterCreationDetailSPID.Text.Trim, Nothing, Nothing, Nothing, Nothing, Nothing)

        udtAuditLogEntry.AddDescripton("SP ID", Me.txtEnterCreationDetailSPID.Text.Trim)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00001, AuditLogDescription.NewClaimTransaction_SearchSP, objAuditLogInfo)

        Me.udcMessageBox.Visible = False
        Me.udcInfoMessageBox.Visible = False
        Me.imgEnterCreationDetailSPIDError.Visible = False

        Dim sm As SystemMessage

        sm = Me.udtValidator.chkSPID(Me.txtEnterCreationDetailSPID.Text.Trim)

        If IsNothing(sm) Then
            If Me.txtEnterCreationDetailSPID.Text.Trim.Length = 8 Then
                Dim udtAccountChangeMaintBLL As New AccountChangeMaintenance.AccountChangeMaintenanceBLL

                Dim dt As DataTable

                Dim udtBLLSearchResult As BaseBLL.BLLSearchResult

                udtBLLSearchResult = udtAccountChangeMaintBLL.MaintenanceSearch(FunctionCode, String.Empty, Me.txtEnterCreationDetailSPID.Text.Trim, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty)

                dt = CType(udtBLLSearchResult.Data, DataTable)
                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

                If dt.Rows.Count = 0 Then
                    ' No Record Found
                    udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
                    udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
                    udcInfoMessageBox.BuildMessageBox()

                    'udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
                    udtAuditLogEntry.AddDescripton("SP ID", Me.txtEnterCreationDetailSPID.Text.Trim)
                    udtAuditLogEntry.AddDescripton("No of record", "0")
                    udtAuditLogEntry.AddDescripton("Go To Advanced Search", "N")
                    ' ' CRE1-004
                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00002, AuditLogDescription.NewClaimTransaction_SearchSP_Success, objAuditLogInfo)

                Else
                    GetReadyServiceProvider(CStr(dt.Rows(0)("SP_ID")))
                    'udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
                    udtAuditLogEntry.AddDescripton("SP ID", Me.txtEnterCreationDetailSPID.Text.Trim)
                    udtAuditLogEntry.AddDescripton("No of record", "1")
                    udtAuditLogEntry.AddDescripton("Go To Advanced Search", "N")
                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00002, AuditLogDescription.NewClaimTransaction_SearchSP_Success, objAuditLogInfo)
                End If

            ElseIf Me.txtEnterCreationDetailSPID.Text.Trim.Length = 0 Then
                Me.udcInfoMsgAdvancedSearch.Visible = False
                Me.udcSystemMsgAdvancedSearch.Visible = False

                Me.txtAdvancedSearchHKIC.Text = String.Empty
                Me.txtAdvancedSearchName.Text = String.Empty
                Me.txtAdvancedSearchPhone.Text = String.Empty
                Me.txtAdvancedSearchSPID.Text = String.Empty

                Me.pnlAdvancedSearchCritieria.Visible = True
                Me.pnlAdvancedSearchResult.Visible = False

                Me.imgAdvancedSearchSPIDErr.Visible = False
                Me.imgAdvancedSearchHKICErr.Visible = False

                Session.Remove(SESS_AdvancedSearchSP)

                ModalPopupSearchSP.Show()

                'udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
                udtAuditLogEntry.AddDescripton("SP ID", Me.txtEnterCreationDetailSPID.Text.Trim)
                udtAuditLogEntry.AddDescripton("No of record", "-")
                udtAuditLogEntry.AddDescripton("Go To Advanced Search", "Y")
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00002, AuditLogDescription.NewClaimTransaction_SearchSP_Success, objAuditLogInfo)
            End If
        Else
            Me.imgEnterCreationDetailSPIDError.Visible = True
            'udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
            udtAuditLogEntry.AddDescripton("SP ID", Me.txtEnterCreationDetailSPID.Text.Trim)
            udtAuditLogEntry.AddDescripton("No of record", "-")
            udtAuditLogEntry.AddDescripton("Go To Advanced Search", "N")

            Me.udcMessageBox.AddMessage(sm)
            Me.udcMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00003, AuditLogDescription.NewClaimTransaction_SearchSP_Fail, objAuditLogInfo)
        End If

    End Sub

    Protected Sub ibtnClearSearchSP_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        'Dim udtEHSAccount As EHSAccount.EHSAccountModel
        'Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
        'udtEHSAccount = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        'udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)

        Me.txtEnterCreationDetailSPID.Text = String.Empty
        Me.lblEnterCreationDetailSPName.Text = String.Empty
        Me.lblEnterCreationDetailSPStatus.Text = String.Empty
        Me.lblEnterCreationDetailPracticeStatus.Text = String.Empty

        Me.txtEnterCreationDetailSPID.Enabled = True
        Me.ibtnSearchSP.Enabled = True
        Me.ibtnClearSearchSP.Enabled = False

        Me.ibtnSearchSP.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchSBtn")
        Me.ibtnClearSearchSP.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ClearDisableSBtn")

        Me.ddlEnterCreationDetailPractice.Items.Clear()
        Me.ddlEnterCreationDetailPractice.Enabled = False

        Me.ddlEnterCreationDetailScheme.Items.Clear()
        Me.ddlEnterCreationDetailScheme.Enabled = False

        Me.ibtnNewClaimTransaction.Enabled = False
        Me.ibtnNewClaimTransaction.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "NewClaimTransactionDisableBtn")

        Me.udcMessageBox.Visible = False
        ClearEnterCreationDetailsErrorImage()

        Session(SESS_ServiceProvider) = Nothing

        Me.udtSessionHandlerBLL.EHSTransactionRemoveFromSession(FunctionCode)

        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00004, AuditLogDescription.NewClaimTransaction_SearchSP_Clear)

    End Sub

#End Region

#Region "SP Advanaced Search"
    Protected Sub ibtnAdvancedSearchSP_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim objAuditLogInfo As AuditLogInfo = Nothing
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        'Dim udtEHSAccount As EHSAccount.EHSAccountModel
        'Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
        'udtEHSAccount = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        'udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)

        udtAuditLogEntry.AddDescripton("SP ID", Me.txtAdvancedSearchSPID.Text.Trim)
        udtAuditLogEntry.AddDescripton("SP HKIC No.", Me.txtAdvancedSearchHKIC.Text.Trim)
        udtAuditLogEntry.AddDescripton("SP Name", Me.txtAdvancedSearchName.Text.Trim)
        udtAuditLogEntry.AddDescripton("SP Phone No.", Me.txtAdvancedSearchPhone.Text.Trim)

        objAuditLogInfo = New AuditLogInfo(Me.txtAdvancedSearchSPID.Text.Trim, Nothing, Nothing, Nothing, Nothing, Nothing)

        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00005, AuditLogDescription.NewClaimTransaction_AdvancedSearchSP)

        Me.udcSystemMsgAdvancedSearch.Visible = False
        Me.udcInfoMsgAdvancedSearch.Visible = False

        Me.imgAdvancedSearchSPIDErr.Visible = False
        Me.imgAdvancedSearchHKICErr.Visible = False

        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        Dim strHKIC As String = String.Empty

        If Me.txtAdvancedSearchHKIC.Text.Trim.Equals(String.Empty) AndAlso Me.txtAdvancedSearchName.Text.Trim.Equals(String.Empty) AndAlso _
           Me.txtAdvancedSearchPhone.Text.Trim.Equals(String.Empty) AndAlso Me.txtAdvancedSearchSPID.Text.Trim.Equals(String.Empty) Then
            'udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
            udtAuditLogEntry.AddDescripton("SP ID", Me.txtAdvancedSearchSPID.Text.Trim)
            udtAuditLogEntry.AddDescripton("SP HKIC No.", Me.txtAdvancedSearchHKIC.Text.Trim)
            udtAuditLogEntry.AddDescripton("SP Name", Me.txtAdvancedSearchName.Text.Trim)
            udtAuditLogEntry.AddDescripton("SP Phone No.", Me.txtAdvancedSearchPhone.Text.Trim)

            Me.ModalPopupSearchSP.Show()
            udtSM = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00257)
            Me.udcSystemMsgAdvancedSearch.AddMessage(udtSM)
            Me.udcSystemMsgAdvancedSearch.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00007, AuditLogDescription.NewClaimTransaction_AdvancedSearchSP_Fail, objAuditLogInfo)

        Else
            Dim blnKeywordSearch As Boolean = False
            Dim blnError As Boolean = False

            If Not Me.txtAdvancedSearchSPID.Text.Trim.Equals(String.Empty) AndAlso Me.txtAdvancedSearchSPID.Text.Trim.Length = 8 Then
                blnKeywordSearch = True
            Else
                udtSM = Me.udtValidator.chkSPID(Me.txtAdvancedSearchSPID.Text.Trim)
                If Not IsNothing(udtSM) Then
                    Me.udcSystemMsgAdvancedSearch.AddMessage(udtSM)
                    Me.imgAdvancedSearchSPIDErr.Visible = True
                    blnError = True
                End If
            End If

            If Not Me.txtAdvancedSearchHKIC.Text.Trim.Equals(String.Empty) Then
                udtSM = Me.udtValidator.chkIdentityNumber(DocTypeModel.DocTypeCode.HKIC, Me.txtAdvancedSearchHKIC.Text.Trim, String.Empty)
                If IsNothing(udtSM) Then
                    strHKIC = Me.udtFormatter.formatHKID(Me.txtAdvancedSearchSPID.Text, False)
                    blnKeywordSearch = True
                Else
                    Me.udcSystemMsgAdvancedSearch.AddMessage(udtSM)
                    Me.imgAdvancedSearchHKICErr.Visible = True
                    blnError = True
                End If

            End If

            If blnError Then
                Me.ModalPopupSearchSP.Show()

                'udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
                udtAuditLogEntry.AddDescripton("SP ID", Me.txtAdvancedSearchSPID.Text.Trim)
                udtAuditLogEntry.AddDescripton("SP HKIC No.", Me.txtAdvancedSearchHKIC.Text.Trim)
                udtAuditLogEntry.AddDescripton("SP Name", Me.txtAdvancedSearchName.Text.Trim)
                udtAuditLogEntry.AddDescripton("SP Phone No.", Me.txtAdvancedSearchPhone.Text.Trim)

                Me.udcSystemMsgAdvancedSearch.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00007, AuditLogDescription.NewClaimTransaction_AdvancedSearchSP_Fail, objAuditLogInfo)
            Else
                Dim udtAccountChangeMaintBLL As New AccountChangeMaintenance.AccountChangeMaintenanceBLL

                Dim dt As DataTable

                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
                ' -------------------------------------------------------------------------
                'dt = udtAccountChangeMaintBLL.MaintenanceSearch(String.Empty, Me.txtAdvancedSearchSPID.Text.Trim, _
                '        udtFormatter.formatHKIDInternal(Me.txtAdvancedSearchHKIC.Text), Me.txtAdvancedSearchName.Text.Trim, _
                '        Me.txtAdvancedSearchPhone.Text.Trim, String.Empty, String.Empty)

                Dim udtBLLSearchResult As BaseBLL.BLLSearchResult

                ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
                udtBLLSearchResult = udtAccountChangeMaintBLL.MaintenanceSearch(FunctionCode, String.Empty, Me.txtAdvancedSearchSPID.Text.Trim, _
                                                                                udtFormatter.formatHKIDInternal(Me.txtAdvancedSearchHKIC.Text), Me.txtAdvancedSearchName.Text.Trim, String.Empty, _
                                                                                Me.txtAdvancedSearchPhone.Text.Trim, String.Empty, String.Empty)
                ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]

                dt = CType(udtBLLSearchResult.Data, DataTable)
                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

                ' Sort the datatable by SPID
                dt = SortServiceProvider(dt, "SP_ID")

                If dt.Rows.Count = 0 Then
                    Me.ModalPopupSearchSP.Show()
                    Me.pnlAdvancedSearchCritieria.Visible = True
                    Me.pnlAdvancedSearchResult.Visible = False

                    Me.udcInfoMsgAdvancedSearch.Type = CustomControls.InfoMessageBoxType.Information
                    udcInfoMsgAdvancedSearch.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
                    udcInfoMsgAdvancedSearch.BuildMessageBox()

                    'udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
                    udtAuditLogEntry.AddDescripton("SP ID", Me.txtAdvancedSearchSPID.Text.Trim)
                    udtAuditLogEntry.AddDescripton("SP HKIC No.", Me.txtAdvancedSearchHKIC.Text.Trim)
                    udtAuditLogEntry.AddDescripton("SP Name", Me.txtAdvancedSearchName.Text.Trim)
                    udtAuditLogEntry.AddDescripton("SP Phone No.", Me.txtAdvancedSearchPhone.Text.Trim)
                    udtAuditLogEntry.AddDescripton("No of record", "0")
                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00006, AuditLogDescription.NewClaimTransaction_AdvancedSearchSP_Success, objAuditLogInfo)

                ElseIf dt.Rows.Count = 1 AndAlso blnKeywordSearch Then
                    'Return to Enter Creation Details and close the popup
                    Me.GetReadyServiceProvider(CStr(dt.Rows(0)("SP_ID")))
                    Me.ModalPopupSearchSP.Hide()

                    Session.Remove(SESS_AdvancedSearchSP)

                    'udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
                    udtAuditLogEntry.AddDescripton("SP ID", Me.txtAdvancedSearchSPID.Text.Trim)
                    udtAuditLogEntry.AddDescripton("SP HKIC No.", Me.txtAdvancedSearchHKIC.Text.Trim)
                    udtAuditLogEntry.AddDescripton("SP Name", Me.txtAdvancedSearchName.Text.Trim)
                    udtAuditLogEntry.AddDescripton("SP Phone No.", Me.txtAdvancedSearchPhone.Text.Trim)
                    udtAuditLogEntry.AddDescripton("No of record", "1")
                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00006, AuditLogDescription.NewClaimTransaction_AdvancedSearchSP_Success, objAuditLogInfo)
                Else
                    Me.ModalPopupSearchSP.Show()

                    If Me.txtAdvancedSearchSPID.Text.Trim.Equals(String.Empty) Then
                        Me.lblAdvancedSearchResultSPID.Text = Me.GetGlobalResourceObject("Text", "Any")
                    Else
                        Me.lblAdvancedSearchResultSPID.Text = Me.txtAdvancedSearchSPID.Text.Trim
                    End If

                    If Me.txtAdvancedSearchHKIC.Text.Trim.Equals(String.Empty) Then
                        Me.lblAdvancedSearchResultHKIC.Text = Me.GetGlobalResourceObject("Text", "Any")
                    Else
                        Me.lblAdvancedSearchResultHKIC.Text = strHKIC
                    End If

                    If Me.txtAdvancedSearchName.Text.Trim.Equals(String.Empty) Then
                        Me.lblAdvancedSearchResultName.Text = Me.GetGlobalResourceObject("Text", "Any")
                    Else
                        Me.lblAdvancedSearchResultName.Text = Me.txtAdvancedSearchName.Text.Trim
                    End If

                    If Me.txtAdvancedSearchPhone.Text.Trim.Equals(String.Empty) Then
                        Me.lblAdvancedSearchResultPhone.Text = Me.GetGlobalResourceObject("Text", "Any")
                    Else
                        Me.lblAdvancedSearchResultPhone.Text = Me.txtAdvancedSearchPhone.Text.Trim
                    End If


                    Me.pnlAdvancedSearchCritieria.Visible = False
                    Me.pnlAdvancedSearchResult.Visible = True

                    If dt.Rows.Count >= 10 Then
                        pnlAdvancedSearchResult.Attributes.Remove("style")
                        pnlAdvancedSearchResult.Attributes.Add("style", "height: 500px;overflow: auto;")
                    Else
                        pnlAdvancedSearchResult.Attributes.Remove("style")
                        pnlAdvancedSearchResult.Attributes.Add("style", "height: auto;overflow: auto;")
                    End If

                    Session(SESS_AdvancedSearchSP) = dt
                    Me.GridViewDataBind(gvAdvancedSearchSP, dt, "SP_ID", "ASC", False)

                    'udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
                    udtAuditLogEntry.AddDescripton("SP ID", Me.txtAdvancedSearchSPID.Text.Trim)
                    udtAuditLogEntry.AddDescripton("SP HKIC No.", Me.txtAdvancedSearchHKIC.Text.Trim)
                    udtAuditLogEntry.AddDescripton("SP Name", Me.txtAdvancedSearchName.Text.Trim)
                    udtAuditLogEntry.AddDescripton("SP Phone No.", Me.txtAdvancedSearchPhone.Text.Trim)
                    udtAuditLogEntry.AddDescripton("No of record", dt.Rows.Count)
                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00006, AuditLogDescription.NewClaimTransaction_AdvancedSearchSP_Success)

                End If
            End If
        End If
    End Sub

    Protected Sub ibtnAdvancedSearchSPClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        Me.ModalPopupSearchSP.Hide()
        Session.Remove(SESS_AdvancedSearchSP)

        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00008, AuditLogDescription.NewClaimTransaction_AdvancedSearchSP_Close)
    End Sub

    Protected Sub ibtnAdvancedSearchResultBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        Me.pnlAdvancedSearchCritieria.Visible = True
        Me.pnlAdvancedSearchResult.Visible = False
        Me.ModalPopupSearchSP.Show()
        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00010, AuditLogDescription.NewClaimTransaction_AdvancedSelectSP_Back)
    End Sub

    Private Sub gvAdvancedSearchSP_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvAdvancedSearchSP.PageIndexChanging
        Me.GridViewPageIndexChangingHandler(sender, e, SESS_AdvancedSearchSP)
        Me.ModalPopupSearchSP.Show()
    End Sub

    Private Sub gvAdvancedSearchSP_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvAdvancedSearchSP.PreRender
        Me.GridViewPreRenderHandler(sender, e, SESS_AdvancedSearchSP)
        Me.ModalPopupSearchSP.Show()
    End Sub

    Private Sub gvAdvancedSearchSP_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvAdvancedSearchSP.RowCommand
        Dim objAuditLogInfo As AuditLogInfo = Nothing
        If TypeOf e.CommandSource Is LinkButton Then
            Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

            Dim strSPID As String = String.Empty

            Dim strCommandArgument As String = e.CommandArgument.ToString.Trim
            strSPID = strCommandArgument
            Me.GetReadyServiceProvider(strSPID)

            Session.Remove(SESS_AdvancedSearchSP)

            Me.ModalPopupSearchSP.Hide()

            'Dim udtEHSAccount As EHSAccount.EHSAccountModel
            'Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
            'udtEHSAccount = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
            'udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)
            Me.udtAuditLogEntry.AddDescripton("SP ID", strSPID)

            objAuditLogInfo = New AuditLogInfo(strSPID, Nothing, Nothing, Nothing, Nothing, Nothing)

            Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00009, AuditLogDescription.NewClaimTransaction_AdvancedSelectSP, objAuditLogInfo)
        End If
    End Sub

    Private Sub gvAdvancedSearchSP_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAdvancedSearchSP.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblAdvancedSearchCname As Label = CType(e.Row.FindControl("lblAdvancedSearchCname"), Label)
            lblAdvancedSearchCname.Text = udtFormatter.formatChineseName(lblAdvancedSearchCname.Text.Trim)

            Dim lblAdvancedSearchSPHKID As Label = CType(e.Row.FindControl("lblAdvancedSearchSPHKID"), Label)
            lblAdvancedSearchSPHKID.Text = Me.udtFormatter.FormatDocIdentityNoForDisplay(DocTypeModel.DocTypeCode.HKIC, lblAdvancedSearchSPHKID.Text.Trim, False)
        End If
    End Sub

    Private Sub gvAdvancedSearchSP_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvAdvancedSearchSP.Sorting
        Me.GridViewSortingHandler(sender, e, SESS_AdvancedSearchSP)
        Me.ModalPopupSearchSP.Show()
    End Sub

    Private Function SortServiceProvider(ByVal dt As DataTable, ByVal strField As String) As DataTable
        Dim dtResult As DataTable = dt.Clone

        For Each dr As DataRow In dt.Select(String.Empty, strField)
            dtResult.ImportRow(dr)
        Next

        Return dtResult

    End Function

#End Region

#Region "New Claim Transaction"
    Protected Sub ibtnNewClaimTransaction_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNewClaimTransaction.Click
        Dim udtAuditLogInfo As AuditLogInfo = Nothing
        Dim udtManualEHSTransaction As EHSTransactionModel = Nothing

        Dim blnError As Boolean = False

        Dim strOriginalDocID As String = String.Empty
        Dim strDocIDSet() As String

        Dim strAdoptionPrefixNum As String = String.Empty
        Dim strDocID As String = String.Empty
        Dim strDocType As String = String.Empty
        Dim strValidatedAccID As String = String.Empty
        Dim strReferenceID As String = String.Empty

        '--------------------------------------------------------
        ' Initial session variables, message box & alert images
        '--------------------------------------------------------
        ' Clear eVaccination Record
        Dim udtSession As New BLL.SessionHandlerBLL()
        udtSession.CMSVaccineResultRemoveFromSession(FunctionCode)
        udtSession.CIMSVaccineResultRemoveFromSession(FunctionCode)
        ViewState.Remove(VS.VaccinationRecordPopupShown)

        ' Clear selected category
        Me.udtSessionHandlerBLL.ClaimCategoryRemoveFromSession(FunctionCode)


        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ' Clear EHS Transaction Model without detail (Inputted in Claim Creation Information)
        Me.udtSessionHandlerBLL.EHSTransactionWithoutTransactionDetailRemoveFromSession(FunctionCode)

        ' Clear EHS Transaction (Inputted in claim page)
        Me.udtSessionHandlerBLL.EHSTransactionRemoveFromSession(FunctionCode)
        ' CRE19-006 (DHC) [End][Winnie]

        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Me.udtSessionHandlerBLL.HAPatientRemoveFromSession()
        Me.udtSessionHandlerBLL.NewClaimTransactionSaveToSession(True)
        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

        ' Clear all message boxes
        Me.udcMessageBox.Visible = False
        Me.udcInfoMessageBox.Visible = False

        ' Clear all alert images
        ClearEnterCreationDetailsErrorImage()

        If Not blnError Then
            '---------------------------------------------
            ' Write "Enter Creation Detial" log
            '---------------------------------------------
            Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

            Dim strSPStatus As String = String.Empty

            If Me.lblEnterCreationDetailSPStatus.Text.Trim.Equals(String.Empty) Then
                strSPStatus = "Active"
            Else
                strSPStatus = Me.lblEnterCreationDetailSPStatus.Text.Trim.Replace("(", "").Replace(")", "")
            End If

            Dim strPracticeStatus As String = String.Empty

            If Me.lblEnterCreationDetailPracticeStatus.Text.Trim.Equals(String.Empty) Then
                strPracticeStatus = "Active"
            Else
                strPracticeStatus = Me.lblEnterCreationDetailPracticeStatus.Text.Replace("(", "").Replace(")", "")
            End If

            Dim strSchemeStatus As String = String.Empty

            If Me.lblEnterCreationDetailSchemeStatus.Text.Trim.Equals(String.Empty) Then
                strSchemeStatus = "Active"
            Else
                strSchemeStatus = Me.lblEnterCreationDetailSchemeStatus.Text.Replace("(", "").Replace(")", "")
            End If

            Me.udtAuditLogEntry.AddDescripton("Doc Type", Me.ddlEnterCreationDetaileHSAccountType.SelectedValue)
            Me.udtAuditLogEntry.AddDescripton("Doc ID", Me.txtEnterCreationDetaileHSAccountDocNo.Text.Trim)
            Me.udtAuditLogEntry.AddDescripton("Account ID", Me.txtEnterCreationDetaileHSAccountID.Text.Trim)
            Me.udtAuditLogEntry.AddDescripton("Ref No", Me.txtEnterCreationDetaileHSAccountRefNo.Text.Trim)
            Me.udtAuditLogEntry.AddDescripton("SP ID", Me.txtEnterCreationDetailSPID.Text.Trim)
            Me.udtAuditLogEntry.AddDescripton("SP Name", Me.lblEnterCreationDetailSPName.Text.Trim)
            Me.udtAuditLogEntry.AddDescripton("SP Status", strSPStatus)
            Me.udtAuditLogEntry.AddDescripton("Practice ID", Me.ddlEnterCreationDetailPractice.SelectedValue.Trim)
            Me.udtAuditLogEntry.AddDescripton("Practice Status", strPracticeStatus)
            Me.udtAuditLogEntry.AddDescripton("Scheme", Me.ddlEnterCreationDetailScheme.SelectedValue.Trim)
            Me.udtAuditLogEntry.AddDescripton("Scheme Status", strSchemeStatus)
            Me.udtAuditLogEntry.AddDescripton("Creation Reason", Me.ddlEnterCreationDetailCreationReason.SelectedValue.Trim)
            Me.udtAuditLogEntry.AddDescripton("Creation Reason Remark", Me.txtEnterCreationDetailRemarks.Text.Trim)
            Me.udtAuditLogEntry.AddDescripton("Payment Settlement", Me.ddlEnterCreationDetailPaymentSettlement.SelectedValue.Trim)
            Me.udtAuditLogEntry.AddDescripton("Payment Settlement Remark", Me.txtEnterCreationDetailPaymentRemarks.Text.Trim)
            Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00011, AuditLogDescription.NewClaimTransaction_EnterCreationDetail)

            '------------------------------------
            ' Validation: Enter Creation Detial
            '------------------------------------
            ' 1. eHS AccountDoc No
            If txtEnterCreationDetaileHSAccountDocNo.Text.Trim.Equals(String.Empty) And _
                txtEnterCreationDetaileHSAccountID.Text.Trim.Equals(String.Empty) And _
                txtEnterCreationDetaileHSAccountRefNo.Text.Trim.Equals(String.Empty) Then
                imgEnterCreationDetaileHSAccountDocNoErr.Visible = True
                imgEnterCreationDetaileHSAccountIDErr.Visible = True
                imgEnterCreationDetaileHSAccountRefNoErr.Visible = True
                udtSM = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00257)
                Me.udcMessageBox.AddMessage(udtSM)
                blnError = True
            End If

            ' 2. Practice
            If Me.ddlEnterCreationDetailPractice.SelectedValue.Trim = String.Empty Then
                blnError = True
                imgEnterCreationDetailPractice.Visible = True
                udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002)
                Me.udcMessageBox.AddMessage(udtSM, "%s", lblEnterCreationDetailPracticeText.Text)
            End If

            ' 3. Scheme
            If Me.ddlEnterCreationDetailScheme.Items.Count > 0 AndAlso Me.ddlEnterCreationDetailScheme.SelectedValue.Trim = String.Empty Then
                blnError = True
                imgEnterCreationDetailScheme.Visible = True
                udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002)
                Me.udcMessageBox.AddMessage(udtSM, "%s", lblEnterCreationDetailSchemeText.Text)
            End If

            ' 4. Creation Reason
            If Me.ddlEnterCreationDetailCreationReason.SelectedValue.Trim = String.Empty Then
                blnError = True
                imgEnterCreationDetailCreationReason.Visible = True
                udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002)
                Me.udcMessageBox.AddMessage(udtSM, "%s", lblEnterCreationDetailCreationReasonText.Text)
            End If

            ' 5. Creation Reason - Remarks if "Others" is selected
            If Me.ddlEnterCreationDetailCreationReason.SelectedValue.Trim = "O" AndAlso _
                Me.txtEnterCreationDetailRemarks.Text.Trim = String.Empty Then
                Me.imgEnterCreationDetailRemarks.Visible = True
                udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001)
                Me.udcMessageBox.AddMessage(udtSM, "%s", lblEnterCreationDetailRemarksText.Text)
                blnError = True
            End If

            ' 6. Payment Settlement
            If Me.ddlEnterCreationDetailPaymentSettlement.SelectedValue.Trim = String.Empty Then
                imgEnterCreationDetailPaymentSettlement.Visible = True
                udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002)
                Me.udcMessageBox.AddMessage(udtSM, "%s", lblEnterCreationDetailPaymentSettlementText.Text)
                blnError = True
            End If

            ' 7. Payment Settlement - Remarks if "Others" is selected
            If Me.ddlEnterCreationDetailPaymentSettlement.SelectedValue.Trim = "O" AndAlso _
                Me.txtEnterCreationDetailPaymentRemarks.Text.Trim = String.Empty Then
                Me.imgEnterCreationDetailPaymentRemarks.Visible = True
                udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001)
                Me.udcMessageBox.AddMessage(udtSM, "%s", lblEnterCreationDetailPaymentRemarksText.Text)
                blnError = True
            End If
        End If

        If blnError Then
            Me.udcMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00013, AuditLogDescription.NewClaimTransaction_EnterCreationDetail_Fail, udtAuditLogInfo)
            Return
        Else
            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00012, AuditLogDescription.NewClaimTransaction_EnterCreationDetail_Success, udtAuditLogInfo)
        End If

        If Not blnError Then
            '----------------------------------
            ' Construct EHS Transaction Model
            '----------------------------------
            udtManualEHSTransaction = New EHSTransactionModel

            Dim udtPracticeDisplayList As Practice.PracticeBLL.PracticeDisplayModelCollection
            Dim udtPracticeDisplay As Practice.PracticeBLL.PracticeDisplayModel

            udtPracticeDisplayList = Me.udtSessionHandlerBLL.PracticeDisplayListGetFromSession(FunctionCode)
            udtPracticeDisplay = udtPracticeDisplayList.Filter(CInt(Me.ddlEnterCreationDetailPractice.SelectedValue.Trim))

            udtManualEHSTransaction.ServiceProviderID = Me.txtEnterCreationDetailSPID.Text.Trim
            udtManualEHSTransaction.ServiceProviderName = Me.lblEnterCreationDetailSPName.Text.Trim
            'udtManualEHSTransaction.EHSAcct = udtEHSAccount
            udtManualEHSTransaction.PracticeID = udtPracticeDisplay.PracticeID
            udtManualEHSTransaction.PracticeName = udtPracticeDisplay.PracticeName
            udtManualEHSTransaction.PracticeNameChi = udtPracticeDisplay.PracticeNameChi
            udtManualEHSTransaction.BankAccountID = udtPracticeDisplay.BankAcctID
            udtManualEHSTransaction.BankAccountNo = udtPracticeDisplay.BankAccountNo
            udtManualEHSTransaction.BankAccountOwner = udtPracticeDisplay.BankAccHolder
            udtManualEHSTransaction.ServiceType = udtPracticeDisplay.ServiceCategoryCode
            udtManualEHSTransaction.SchemeCode = Me.ddlEnterCreationDetailScheme.SelectedValue.Trim
            udtManualEHSTransaction.CreationReason = Me.ddlEnterCreationDetailCreationReason.SelectedValue.Trim
            udtManualEHSTransaction.CreationRemarks = Me.txtEnterCreationDetailRemarks.Text.Trim
            udtManualEHSTransaction.PaymentMethod = Me.ddlEnterCreationDetailPaymentSettlement.SelectedValue.Trim
            udtManualEHSTransaction.PaymentRemarks = Me.txtEnterCreationDetailPaymentRemarks.Text.Trim

            'Save Session
            Me.udtSessionHandlerBLL.PracticeDisplaySaveToSession(udtPracticeDisplay, FunctionCode)
            Me.udtSessionHandlerBLL.EHSTransactionWithoutTransactionDetailSaveToSession(udtManualEHSTransaction, FunctionCode)

        End If

        If Not blnError Then
            '---------------------------------------------
            ' Write "Search Account" log
            '---------------------------------------------
            udtAuditLogInfo = New AuditLogInfo(Me.txtEnterCreationDetailSPID.Text.Trim, Nothing, Nothing, Nothing, Nothing, Me.txtEnterCreationDetaileHSAccountDocNo.Text.Trim)

            Me.udtAuditLogEntry.AddDescripton("Doc Type", Me.ddlEnterCreationDetaileHSAccountType.SelectedValue.Trim)
            Me.udtAuditLogEntry.AddDescripton("Doc ID", Me.txtEnterCreationDetaileHSAccountDocNo.Text.Trim)
            Me.udtAuditLogEntry.AddDescripton("Account ID", Me.txtEnterCreationDetaileHSAccountID.Text.Trim)
            Me.udtAuditLogEntry.AddDescripton("Ref No", Me.txtEnterCreationDetaileHSAccountRefNo.Text.Trim)
            Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00014, AuditLogDescription.NewClaimTransaction_SearchAccountClick, udtAuditLogInfo)

            '----------------------
            ' Validation: Account
            '----------------------
            'Assign UI inputted values to variables
            strDocType = Me.ddlEnterCreationDetaileHSAccountType.SelectedValue.Trim

            strOriginalDocID = Me.txtEnterCreationDetaileHSAccountDocNo.Text.Trim.ToUpper.Replace("-", "").Replace("(", "").Replace(")", "")

            strDocIDSet = strOriginalDocID.Trim.Split("/")

            If strDocIDSet.Length > 1 Then
                'ADOPC
                strDocID = strDocIDSet(1)
                strAdoptionPrefixNum = strDocIDSet(0)
            Else
                'Other 7 Doc. ID
                strDocID = strOriginalDocID
            End If

            strValidatedAccID = Me.txtEnterCreationDetaileHSAccountID.Text.Trim
            strReferenceID = Me.txtEnterCreationDetaileHSAccountRefNo.Text.Trim

            'If inputted doc type, check whether is correct format
            If strOriginalDocID <> String.Empty Then
                Me.udtSM = Me.udtValidator.chkIdentityNumber(strDocType, strDocID, strAdoptionPrefixNum)
                If Not IsNothing(udtSM) Then
                    Me.udcMessageBox.AddMessage(udtSM)

                    Me.imgEnterCreationDetaileHSAccountDocNoErr.Visible = True

                    blnError = True

                End If
            End If

            'If inputted validated account ID, check whether is correct format
            If strValidatedAccID <> String.Empty AndAlso Not Me.udtValidator.chkValidatedEHSAccountNumber(strValidatedAccID) Then
                udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00003)
                Me.udcMessageBox.AddMessage(udtSM, "%s", lblEnterCreationDetaileHSAccountIDText.Text)

                Me.imgEnterCreationDetaileHSAccountIDErr.Visible = True

                blnError = True

            End If

            'If inputted validated account ID, check whether is correct format
            If strReferenceID <> String.Empty Then
                If Me.udtValidator.chkSystemNumber(strReferenceID) Then
                    'udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00003)
                    'Me.udcMessageBox.AddMessage(udtSM, "%s", lblEnterCreationDetaileHSAccountRefNoText.Text)

                    'Me.imgEnterCreationDetaileHSAccountRefNoErr.Visible = True

                    'blnError = True
                    strReferenceID = Formatter.ReverseSystemNumber(strReferenceID)
                End If
            End If

        End If

        If blnError Then
            udtAuditLogEntry.AddDescripton("Doc Type", Me.ddlEnterCreationDetaileHSAccountType.SelectedValue.Trim)
            udtAuditLogEntry.AddDescripton("Doc ID", Me.txtEnterCreationDetaileHSAccountDocNo.Text.Trim)
            udtAuditLogEntry.AddDescripton("Account ID", Me.txtEnterCreationDetaileHSAccountID.Text.Trim)
            udtAuditLogEntry.AddDescripton("Ref No", Me.txtEnterCreationDetaileHSAccountRefNo.Text.Trim)
            udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", Me.txtEnterCreationDetaileHSAccountID.Text.Trim)
            udtAuditLogEntry.AddDescripton("Reference No.", Me.txtEnterCreationDetaileHSAccountRefNo.Text.Trim)
            Me.udcMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00016, AuditLogDescription.NewClaimTransaction_SearchAccountClick_Fail)
            Return
        End If

        If Not blnError Then
            '-----------------
            ' Get EHS Account
            '-----------------
            Dim dtResValidated As DataTable = Nothing
            Dim dtResTemporary As DataTable = Nothing
            Dim dtRes As DataTable = Nothing
            Dim blnAccountMerged As Boolean = False
            Dim blnNoRecord As Boolean = False

            Dim udtEHSAccountBLL As New EHSAccount.EHSAccountBLL

            'If both use validated account ID and Ref. No. to search, return "No records found"
            If Me.txtEnterCreationDetaileHSAccountID.Text.Trim <> String.Empty And txtEnterCreationDetaileHSAccountRefNo.Text.Trim <> String.Empty Then
                dtRes = Nothing
            Else
                'Get Validated Account, and claims with all schemes (Not include PPP scheme)
                If (strOriginalDocID <> String.Empty Or _
                   strValidatedAccID <> String.Empty) And _
                   strReferenceID = String.Empty Then
                    'Remove check digit
                    If strValidatedAccID <> String.Empty Then
                        strValidatedAccID = strValidatedAccID.Substring(0, strValidatedAccID.Length - 1)
                    End If

                    dtResValidated = udtEHSAccountBLL.LoadEHSAccountByIdentityVRID(strDocID, strAdoptionPrefixNum, strDocType, strValidatedAccID)
                End If

                'Get Temp Account(Not for ImmD Validation), and claims with PPP scheme only  
                If udtManualEHSTransaction.SchemeCode = SchemeClaimModel.PPP Then
                    If (strOriginalDocID <> String.Empty Or _
                        strReferenceID <> String.Empty) And _
                        strValidatedAccID = String.Empty Then
                        dtResTemporary = udtEHSAccountBLL.LoadTempEHSAccountByIdentityVRID(strDocID, strAdoptionPrefixNum, strDocType, strReferenceID)
                    End If
                End If

                If Not dtResValidated Is Nothing AndAlso dtResValidated.Rows.Count > 0 Then
                    If Not dtResTemporary Is Nothing AndAlso dtResTemporary.Rows.Count > 0 Then
                        dtRes = New DataTable
                        blnAccountMerged = True

                        dtRes = dtResValidated.Clone
                        dtRes.Merge(dtResValidated)

                        For Each dr As DataRow In dtResTemporary.Rows
                            'Filter account with transaction
                            If dr("With_Transaction") = YesNo.Yes Then
                                Continue For
                            End If

                            'Filter account not created by Back-Office
                            If dr("Create_By_BO") = YesNo.No Then
                                Continue For
                            End If

                            dtRes.ImportRow(dr)
                        Next

                    End If
                End If

                If Not blnAccountMerged Then
                    'If only validated account(s) is/are found, return it.
                    If Not dtResValidated Is Nothing AndAlso dtResValidated.Rows.Count > 0 Then
                        dtRes = dtResValidated
                    End If

                    'If only temporary account(s) is/are found, return it.
                    If Not dtResTemporary Is Nothing AndAlso dtResTemporary.Rows.Count > 0 Then
                        dtRes = dtResTemporary.Clone

                        For Each dr As DataRow In dtResTemporary.Rows
                            'Filter account with transaction
                            If dr("With_Transaction") = YesNo.Yes Then
                                Continue For
                            End If

                            'Filter account not created by Back-Office
                            If dr("Create_By_BO") = YesNo.No Then
                                Continue For
                            End If

                            dtRes.ImportRow(dr)
                        Next

                    End If
                End If

                ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                ' Check Patient whether is on list
                If udtManualEHSTransaction.SchemeCode = SchemeClaimModel.SSSCMC Then
                    If Not dtResValidated Is Nothing AndAlso dtResValidated.Rows.Count > 0 Then
                        Dim blnFoundHAPatient As Boolean = False

                        For Each drResValidated As DataRow In dtResValidated.Rows
                            Dim strDocCode As String = drResValidated("Doc_Code").ToString.Trim
                            Dim strIdentityNum As String = drResValidated("IdentityNum").ToString.Trim

                            If (New ClaimRulesBLL).CheckIsHAPatient(udtManualEHSTransaction.SchemeCode, strDocCode, strIdentityNum) = String.Empty Then
                                blnFoundHAPatient = True
                                Exit For
                            End If
                        Next

                        If Not blnFoundHAPatient Then
                            dtRes = Nothing
                        End If

                    Else
                        dtRes = Nothing
                    End If

                End If
                ' CRE20-015 (Special Support Scheme) [End][Chris YIM]
            End If

            If IsNothing(dtRes) Then
                blnNoRecord = True
            Else
                If dtRes.Rows.Count = 0 Then
                    blnNoRecord = True
                End If
            End If

            If blnNoRecord Then
                Dim blnMatched = False
                Dim blnWithTransaction As Boolean = False
                Dim blnCreatedBySP As Boolean = False
                '------------------------------------------------------
                ' Account retrieved but it is not eligible to claim
                '------------------------------------------------------
                If Not dtResTemporary Is Nothing AndAlso dtResTemporary.Rows.Count > 0 Then
                    For Each dr As DataRow In dtResTemporary.Rows
                        If dr("With_Transaction") = YesNo.Yes Then
                            blnWithTransaction = True
                        End If

                        If dr("Create_By_BO") = YesNo.No Then
                            blnCreatedBySP = True
                        End If
                    Next

                    If Not blnMatched AndAlso blnWithTransaction Then
                        'Info: Temporary eHealth (Subsidies) Account found but has claimed transaction. Please create an account before making claim.
                        Me.udcInfoMessageBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00005))
                        blnMatched = True
                    End If

                    If Not blnMatched AndAlso blnCreatedBySP Then
                        'Info: Temporary eHealth (Subsidies) Account found but not created in Back Office platform. Please create an account before making claim.
                        Me.udcInfoMessageBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00006))
                        blnMatched = True
                    End If
                End If

                ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                '-----------------------------------
                ' No account retrieved
                '-----------------------------------
                If Not blnMatched Then

                    Select Case udtManualEHSTransaction.SchemeCode
                        Case SchemeClaimModel.PPP
                            'Info: No eHealth (Subsidies) Account found.
                            Me.udcInfoMessageBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00002))

                        Case SchemeClaimModel.SSSCMC
                            'Error: The service recipient is not eligible for the selected scheme.
                            Me.udcMessageBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00004))

                        Case Else
                            'Info: No validated eHealth (Subsidies) Account found. Please create an account before making claim.
                            Me.udcInfoMessageBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00004))

                    End Select


                End If

                Me.udcMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00013, AuditLogDescription.NewClaimTransaction_EnterCreationDetail_Fail, udtAuditLogInfo)

                Me.udcInfoMessageBox.BuildMessageBox()
                Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
                ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

                udtAuditLogEntry.AddDescripton("Doc Type", Me.ddlEnterCreationDetaileHSAccountType.SelectedValue.Trim)
                udtAuditLogEntry.AddDescripton("Doc ID", Me.txtEnterCreationDetaileHSAccountDocNo.Text.Trim)
                udtAuditLogEntry.AddDescripton("Account ID", Me.txtEnterCreationDetaileHSAccountID.Text.Trim)
                udtAuditLogEntry.AddDescripton("Ref No", Me.txtEnterCreationDetaileHSAccountRefNo.Text.Trim)
                udtAuditLogEntry.AddDescripton("No Of Record Found", "0")
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00015, AuditLogDescription.NewClaimTransaction_SearchAccountClick_Success, udtAuditLogInfo)

            Else

                If dtRes.Rows.Count = 1 Then
                    '-----------------------------------
                    ' Go to Enter Claim Detail
                    '-----------------------------------
                    Dim udtEHSAccount As EHSAccount.EHSAccountModel = Nothing

                    Me.GetReadyEHSAccount(dtRes.Rows(0)("Voucher_Acc_ID"), dtRes.Rows(0)("Doc_Code"), udtEHSAccount)

                    udtAuditLogEntry.AddDescripton("Same Doc Type", IIf(dtRes.Rows(0)("Doc_Code").ToString.Trim = Me.ddlEnterCreationDetaileHSAccountType.SelectedValue.Trim, "Y", "N"))
                    udtAuditLogEntry.AddDescripton("Doc Type", Me.ddlEnterCreationDetaileHSAccountType.SelectedValue.Trim)
                    udtAuditLogEntry.AddDescripton("Doc ID", Me.txtEnterCreationDetaileHSAccountDocNo.Text.Trim)
                    udtAuditLogEntry.AddDescripton("Account ID", Me.txtEnterCreationDetaileHSAccountID.Text.Trim)
                    udtAuditLogEntry.AddDescripton("Ref No", Me.txtEnterCreationDetaileHSAccountRefNo.Text.Trim)
                    udtAuditLogEntry.AddDescripton("No Of Record Found", "1")
                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00015, AuditLogDescription.NewClaimTransaction_SearchAccountClick_Success)

                    Me.mvNewClaimTransaction.ActiveViewIndex = NewClaimTransaction.EnterClaimDetails
                    Me.mvEnterDetails.ActiveViewIndex = InputTransactionDetails.ClaimDetails

                    '--------------------------------
                    ' Bulid Claim Detail
                    '--------------------------------
                    BuildClaimDetail(udtEHSAccount, udtManualEHSTransaction)

                Else

                    '-----------------------------------
                    ' Go to Select eHS Account
                    '-----------------------------------
                    lblSerachAccountResultDocType.Text = IIf(ddlEnterCreationDetaileHSAccountType.SelectedValue.Trim = String.Empty, Me.GetGlobalResourceObject("Text", "Any"), ddlEnterCreationDetaileHSAccountType.SelectedValue.Trim)
                    lblSerachAccountResultIdentityNum.Text = IIf(txtEnterCreationDetaileHSAccountDocNo.Text.Trim = String.Empty, Me.GetGlobalResourceObject("Text", "Any"), txtEnterCreationDetaileHSAccountDocNo.Text.Trim)
                    lblSerachAccountResultEHSAccountID.Text = IIf(txtEnterCreationDetaileHSAccountID.Text.Trim = String.Empty, Me.GetGlobalResourceObject("Text", "Any"), lblEnterCreationDetaileHSAccountIDPrefix.Text & txtEnterCreationDetaileHSAccountID.Text.Trim)
                    lblSerachAccountResultEHSAccountRefNo.Text = IIf(txtEnterCreationDetaileHSAccountRefNo.Text.Trim = String.Empty, Me.GetGlobalResourceObject("Text", "Any"), txtEnterCreationDetaileHSAccountRefNo.Text.Trim)

                    Session(SESS_SearchAccount) = dtRes
                    Me.GridViewDataBind(Me.gvSearchAccount, dtRes, "Doc_Code", "ASC", False)

                    Me.mvNewClaimTransaction.ActiveViewIndex = NewClaimTransaction.SearchAccountResults

                    udtAuditLogEntry.AddDescripton("Doc Type", Me.ddlEnterCreationDetaileHSAccountType.SelectedValue.Trim)
                    udtAuditLogEntry.AddDescripton("Doc ID", Me.txtEnterCreationDetaileHSAccountDocNo.Text.Trim)
                    udtAuditLogEntry.AddDescripton("Account ID", Me.txtEnterCreationDetaileHSAccountID.Text.Trim)
                    udtAuditLogEntry.AddDescripton("Ref No", Me.txtEnterCreationDetaileHSAccountRefNo.Text.Trim)
                    udtAuditLogEntry.AddDescripton("No Of Record Found", dtRes.Rows.Count)
                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00015, AuditLogDescription.NewClaimTransaction_SearchAccountClick_Success)
                End If

            End If

        End If

    End Sub

#End Region

#Region "Search Account"

    Protected Sub ibtnSearchAccountResultBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        Me.mvNewClaimTransaction.ActiveViewIndex = NewClaimTransaction.EnterClaimDetails
        Me.mvEnterDetails.ActiveViewIndex = InputTransactionDetails.CreationDetails

        ClearEnterCreationDetailsErrorImage()

        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00018, AuditLogDescription.NewClaimTransaction_SelectAccount_Back)
    End Sub

    Private Sub gvSearchAccount_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSearchAccount.PageIndexChanging
        Me.GridViewPageIndexChangingHandler(sender, e, SESS_SearchAccount)
    End Sub

    Private Sub gvSearchAccount_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSearchAccount.PreRender
        Me.GridViewPreRenderHandler(sender, e, SESS_SearchAccount)
    End Sub

    Private Sub gvSearchAccount_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSearchAccount.RowCommand
        If TypeOf e.CommandSource Is LinkButton Then

            Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

            Dim udtEHSAccount As EHSAccountModel = Nothing
            Dim udtEHSTransaction As EHSTransactionModel = Nothing

            Dim strDocCode As String = String.Empty
            Dim strIdentityNum As String = String.Empty
            Dim strEHSAccountID As String = String.Empty
            Dim strAccountType As String = String.Empty

            Dim strCommandArgument As String = e.CommandArgument.ToString.Trim
            strIdentityNum = strCommandArgument.Split("|")(0).Trim
            strDocCode = strCommandArgument.Split("|")(1).Trim
            strEHSAccountID = strCommandArgument.Split("|")(2).Trim
            strAccountType = strCommandArgument.Split("|")(3).Trim

            udtAuditLogEntry.AddDescripton("Doc Type", strDocCode)
            udtAuditLogEntry.AddDescripton("Doc ID", strIdentityNum)
            udtAuditLogEntry.AddDescripton("Account ID", IIf(strAccountType = AccountType.Validated, strEHSAccountID, String.Empty))
            udtAuditLogEntry.AddDescripton("Ref No", IIf(strAccountType = AccountType.Temporary, strEHSAccountID, String.Empty))
            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00017, AuditLogDescription.NewClaimTransaction_SelectAccount)

            ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Dim udtManualEHSTransaction As EHSTransactionModel = Me.udtSessionHandlerBLL.EHSTransactionWithoutTransactionDetailGetFromSession(FunctionCode)

            ' Check Patient whether is on list
            If udtManualEHSTransaction.SchemeCode = SchemeClaimModel.SSSCMC Then
                Dim udtAuditLogInfo As AuditLogInfo = New AuditLogInfo(Me.txtEnterCreationDetailSPID.Text.Trim, Nothing, Nothing, Nothing, Nothing, Me.txtEnterCreationDetaileHSAccountDocNo.Text.Trim)

                If (New ClaimRulesBLL).CheckIsHAPatient(udtManualEHSTransaction.SchemeCode, strDocCode, strIdentityNum) <> String.Empty Then
                    Me.udcMessageBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00004))
                    Me.udcMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00041, AuditLogDescription.NewClaimTransaction_SelectAccount_Fail, udtAuditLogInfo)

                    Return

                End If

            End If
            ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

            Me.GetReadyEHSAccount(strEHSAccountID, strDocCode, udtEHSAccount)

            ' INT18-0033 (Fix selection of document for vaccination record enquiry in back-office platform) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Dim udtEHSAccountClone As EHSAccount.EHSAccountModel = Nothing
            Dim udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = Nothing

            'Clone EHSAccount
            udtEHSAccountClone = New EHSAccountModel(udtEHSAccount)
            udtEHSPersonalInfo = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)

            udtEHSAccountClone.SetPersonalInformation(udtEHSPersonalInfo)
            udtEHSAccountClone.SetSearchDocCode(udtEHSAccount.SearchDocCode)

            Me.mvNewClaimTransaction.ActiveViewIndex = NewClaimTransaction.EnterClaimDetails
            Me.mvEnterDetails.ActiveViewIndex = InputTransactionDetails.ClaimDetails

            '--------------------------------
            ' Bulid Claim Detail
            '--------------------------------
            udtEHSTransaction = Me.udtSessionHandlerBLL.EHSTransactionWithoutTransactionDetailGetFromSession(FunctionCode)

            'BuildClaimDetail(udtEHSAccount, udtEHSTransaction)
            BuildClaimDetail(udtEHSAccountClone, udtEHSTransaction)

            ' INT18-0033 (Fix selection of document for vaccination record enquiry in back-office platform) [End][Chris YIM]

        End If
    End Sub

    Private Sub gvSearchAccount_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSearchAccount.RowCreated
        Dim udtSortedGVHeaderList As SortedGridviewHeaderModelCollection = New SortedGridviewHeaderModelCollection
        Dim strImageURL As String = Me.GetGlobalResourceObject("ImageURL", "infoicon")

        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(2, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(3, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))

        Me.GirdViewRowCreated(sender, e, udtSortedGVHeaderList)
    End Sub

    Private Sub gvSearchAccount_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSearchAccount.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRow = CType(e.Row.DataItem, System.Data.DataRowView).Row

            Dim lbtnEHSAccountID As LinkButton = CType(e.Row.FindControl("lbtnEHSAccountID"), LinkButton)
            Dim lblDocType As Label = CType(e.Row.FindControl("lblDocType"), Label)
            Dim lbtnIdentityNum As LinkButton = CType(e.Row.FindControl("lbtnIdentityNum"), LinkButton)
            Dim lblName As Label = CType(e.Row.FindControl("lblName"), Label)
            Dim lblCName As Label = CType(e.Row.FindControl("lblCName"), Label)
            Dim lblDOB As Label = CType(e.Row.FindControl("lblDOB"), Label)
            Dim lblSex As Label = CType(e.Row.FindControl("lblSex"), Label)
            Dim lblAccountStatus As Label = CType(e.Row.FindControl("lblAccountStatus"), Label)
            Dim lblCreateDtm As Label = CType(e.Row.FindControl("lblCreateDtm"), Label)
            Dim lblCreateBy As Label = CType(e.Row.FindControl("lblCreateBy"), Label)
            Dim udtDocTypeBLL As New DocTypeBLL

            Dim strDocCode As String = CStr(dr.Item("Doc_Code")).Trim
            Dim strEHSAccountID As String = CStr(dr.Item("Voucher_Acc_ID")).Trim
            Dim strIdentityNum As String = CStr(dr.Item("IdentityNum")).Trim
            Dim strAdoptionPrefixNum As String = CStr(dr.Item("AdoptionPrefixNum")).Trim
            Dim strEName As String = CStr(dr.Item("EName")).Trim
            Dim strCName As String = CStr(dr.Item("CName")).Trim
            Dim dtmDOB As DateTime = CType(dr.Item("DOB"), DateTime)
            Dim strExactDOB As String = CStr(dr.Item("Exact_DOB")).Trim
            Dim strSex As String = CStr(dr.Item("Sex")).Trim
            Dim intAge As Nullable(Of Integer)
            Dim dtDOR As Nullable(Of Date)
            Dim strOtherInfo As String = String.Empty
            Dim strAccountType As String = CStr(dr.Item("Account_Type")).Trim

            If IsDBNull(dr.Item("EC_Age")) Then
                intAge = Nothing
            Else
                intAge = CInt(dr.Item("EC_Age"))
            End If

            If IsDBNull(dr.Item("EC_Date_of_Registration")) Then
                dtDOR = Nothing
            Else
                dtDOR = CType(dr.Item("EC_Date_of_Registration"), Date)
            End If

            If IsDBNull(dr.Item("other_info")) Then
                strOtherInfo = String.Empty
            Else
                strOtherInfo = CStr(dr.Item("other_info"))
            End If

            lblDocType.Text = udtDocTypeBLL.getAllDocType.Filter(strDocCode).DocDisplayCode.Trim

            If strAccountType = AccountType.Validated Then
                lbtnEHSAccountID.Text = udtFormatter.formatValidatedEHSAccountNumber(strEHSAccountID)
            End If

            If strAccountType = AccountType.Temporary Then
                lbtnEHSAccountID.Text = udtFormatter.formatSystemNumber(strEHSAccountID)
            End If

            lbtnEHSAccountID.CommandArgument = strIdentityNum & "|" & strDocCode & "|" & strEHSAccountID & "|" & strAccountType

            lbtnIdentityNum.Text = udtFormatter.FormatDocIdentityNoForDisplay(strDocCode, strIdentityNum, False, strAdoptionPrefixNum)
            lbtnIdentityNum.CommandArgument = strIdentityNum & "|" & strDocCode & "|" & strEHSAccountID & "|" & strAccountType

            lblName.Text = strEName
            lblCName.Text = udtFormatter.formatChineseName(strCName)

            lblDOB.Text = udtFormatter.formatDOB(strDocCode, dtmDOB, strExactDOB, "en-US", intAge, dtDOR, strOtherInfo)

            lblSex.Text = Me.GetGlobalResourceObject("Text", udtFormatter.formatGender(strSex))

            lblAccountStatus.Text = Me.udtEHSAccountMaintBLL.getAcctStatus(CStr(dr.Item("Record_Status")), CStr(dr.Item("Account_Source")))

            lblCreateDtm.Text = udtFormatter.formatDateTime(CType(dr.Item("Create_Dtm"), DateTime))

            If Not IsDBNull(dr.Item("Create_By_BO")) Then
                'has value
                If CStr(dr.Item("Create_By_BO")).Trim = "Y" Then
                    lblCreateBy.Text = CStr(dr.Item("Create_By")).Trim
                Else
                    lblCreateBy.Text = CStr(dr.Item("Create_By")).Trim + "(" + dr.Item("SP_Practice_Display_Seq").ToString().Trim + ")"
                End If
            Else
                lblCreateBy.Text = CStr(dr.Item("Create_By")).Trim + "(" + dr.Item("SP_Practice_Display_Seq").ToString().Trim + ")"
            End If

        End If
    End Sub

    Private Sub gvSearchAccount_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvSearchAccount.Sorting
        Me.GridViewSortingHandler(sender, e, SESS_SearchAccount)
    End Sub

#End Region

#Region "Document Type help popup"
    Public Overrides Sub GridViewHeaderImage_Click(ByVal sender As Object, ByVal e As Common.Component.SortedGridviewHeader.SortedGridviewHeaderModel.GridViewHeaderImageEventArgs)
        popupDocTypeHelp.Show()
        udcDocTypeLegend.BindDocType(Session("language"))
    End Sub

    Protected Sub ibtnCloseDocTypeHelp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCloseDocTypeHelp.Click
        popupDocTypeHelp.Hide()
    End Sub

#End Region

#End Region

#Region "Claim Creation Functions"
    Private Sub InitialEnterCreationDetail()
        'Clear Account Info.
        Me.ddlEnterCreationDetaileHSAccountType.Items.Clear()
        BindDocumentType(Me.ddlEnterCreationDetaileHSAccountType)
        Me.ddlEnterCreationDetaileHSAccountType.Enabled = True
        Me.txtEnterCreationDetaileHSAccountDocNo.Text = String.Empty
        Me.txtEnterCreationDetaileHSAccountID.Text = String.Empty
        Me.txtEnterCreationDetaileHSAccountRefNo.Text = String.Empty

        'eHealth Account Prefix: "EHA"
        Dim strParm1 As String = String.Empty
        Dim strParm2 As String = String.Empty
        If udtGeneralFunction.getSystemParameter("eHealthAccountPrefix", strParm1, strParm2) Then
            lblEnterCreationDetaileHSAccountIDPrefix.Text = strParm1
        End If

        'Clear SPID
        Me.txtEnterCreationDetailSPID.Text = String.Empty
        Me.txtEnterCreationDetailSPID.Enabled = True
        Me.lblEnterCreationDetailSPName.Text = String.Empty

        'Initial Search SPID Button
        Me.ibtnSearchSP.Enabled = True
        Me.ibtnClearSearchSP.Enabled = False
        Me.ibtnSearchSP.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchSBtn")
        Me.ibtnClearSearchSP.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ClearDisableSBtn")

        'Initial Practice DropDownList
        Me.ddlEnterCreationDetailPractice.Items.Clear()
        Me.ddlEnterCreationDetailPractice.Enabled = False

        'Initial Scheme DropDownList
        Me.ddlEnterCreationDetailScheme.Items.Clear()
        Me.ddlEnterCreationDetailScheme.Enabled = False
        Me.panEnterCreationDetailNonClinicSetting.Visible = False

        'Initial Creation Reason DropDownList
        Me.ddlEnterCreationDetailCreationReason.Items.Clear()
        BindCreationReason()
        Me.txtEnterCreationDetailRemarks.Text = String.Empty

        'Initial Payment Settlement DropDownList
        Me.ddlEnterCreationDetailPaymentSettlement.Items.Clear()
        BindPaymentMethod()
        Me.txtEnterCreationDetailPaymentRemarks.Text = String.Empty

        'Initial "New Claim Transaction" Button
        Me.ibtnNewClaimTransaction.Enabled = False
        Me.ibtnNewClaimTransaction.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "NewClaimTransactionDisableBtn")

    End Sub

    Private Sub ClearEnterCreationDetailsErrorImage()
        Me.imgEnterCreationDetaileHSAccountDocNoErr.Visible = False
        Me.imgEnterCreationDetaileHSAccountIDErr.Visible = False
        Me.imgEnterCreationDetaileHSAccountRefNoErr.Visible = False
        Me.imgEnterCreationDetailSPIDError.Visible = False
        Me.imgEnterCreationDetailPractice.Visible = False
        Me.imgEnterCreationDetailScheme.Visible = False
        Me.imgEnterCreationDetailCreationReason.Visible = False
        Me.imgEnterCreationDetailRemarks.Visible = False
        Me.imgEnterCreationDetailPaymentSettlement.Visible = False
        Me.imgEnterCreationDetailPaymentRemarks.Visible = False

    End Sub

    Private Sub BindDocumentType(ByVal ddlEHealthDocType As DropDownList)
        ddlEHealthDocType.DataSource = udtDocTypeBLL.getAllDocType()
        ddlEHealthDocType.DataTextField = "DocName"
        ddlEHealthDocType.DataValueField = "DocCode"
        ddlEHealthDocType.DataBind()

        ddlEHealthDocType.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))

        ddlEHealthDocType.SelectedIndex = 0

    End Sub

    Private Sub BindEnterCreationDetailScheme()
        Dim udtSchemeClaimList As SchemeClaimModelCollection
        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL

        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

        'Get all available List By Back Office User and SP's Practice
        udtSchemeClaimList = Me.udtSchemeClaimBLL.GetAllEnrolledSchemeClaimWithoutSubsidizeGroup(udtHCVUUser.UserID, _
                                                                                                 FunctionCode, _
                                                                                                 Me.txtEnterCreationDetailSPID.Text.Trim, _
                                                                                                 CInt(Me.ddlEnterCreationDetailPractice.SelectedValue.Trim)
                                                                                                 )

        Me.udtSessionHandlerBLL.SchemeListRemoveFromSession(FunctionCode)
        Me.udtSessionHandlerBLL.SchemeListSaveToSession(udtSchemeClaimList, FunctionCode)

        Me.ddlEnterCreationDetailScheme.DataSource = udtSchemeClaimList
        Me.ddlEnterCreationDetailScheme.DataTextField = "SchemeDesc"
        Me.ddlEnterCreationDetailScheme.DataValueField = "SchemeCode"

        Me.ddlEnterCreationDetailScheme.DataBind()

        ddlEnterCreationDetailScheme.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), ""))

        ddlEnterCreationDetailScheme.SelectedIndex = 0

        If udtSchemeClaimList.Count = 0 Then
            ddlEnterCreationDetailScheme.Enabled = False
        Else
            ddlEnterCreationDetailScheme.Enabled = True
        End If

        Me.lblEnterCreationDetailSchemeStatus.Visible = False
        Me.lblEnterCreationDetailSchemeStatus.Text = String.Empty

    End Sub

    Private Sub BindCreationReason()
        Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL

        Me.ddlEnterCreationDetailCreationReason.DataSource = udtStaticDataBLL.GetStaticDataListByColumnName("ClaimCreationReason")
        ddlEnterCreationDetailCreationReason.DataValueField = "ItemNo"
        ddlEnterCreationDetailCreationReason.DataTextField = "DataValue"
        ddlEnterCreationDetailCreationReason.DataBind()

        ddlEnterCreationDetailCreationReason.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), ""))

        ddlEnterCreationDetailCreationReason.SelectedIndex = 0

    End Sub

    Private Sub BindPaymentMethod()
        Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL
        Me.ddlEnterCreationDetailPaymentSettlement.DataSource = udtStaticDataBLL.GetStaticDataListByColumnName("ReimbursementPaymentMethod")
        ddlEnterCreationDetailPaymentSettlement.DataValueField = "ItemNo"
        ddlEnterCreationDetailPaymentSettlement.DataTextField = "DataValue"
        ddlEnterCreationDetailPaymentSettlement.DataBind()

        ddlEnterCreationDetailPaymentSettlement.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), ""))

        ddlEnterCreationDetailPaymentSettlement.SelectedIndex = 0
    End Sub

#End Region

#End Region

#Region "Enter Claim Detail"

#Region "Enter Claim Detail Events"
    Protected Sub txtEnterClaimDetailServiceDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
        Dim udtEHSAccount As EHSAccount.EHSAccountModel = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        Dim udtEHSTransactionWithoutTransactionDetail As EHSTransactionModel = Me.udtSessionHandlerBLL.EHSTransactionWithoutTransactionDetailGetFromSession(FunctionCode)

        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton(IIf(udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount, "eHS Validated Acc ID", "eHS Temp Acc ID"), _
                                       IIf(udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount, udtEHSAccount.VoucherAccID, udtEHSAccount.VoucherAccID))
        udtAuditLogEntry.AddDescripton("Service Date", Me.txtEnterClaimDetailServiceDate.Text.Trim)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00024, AuditLogDescription.NewClaimTransaction_ChangeServiceDate)

        Me.udcMessageBox.Visible = False
        Me.udcInfoMessageBox.Visible = False
        Me.imgEnterClaimDetailServiceDateErr.Visible = False

        Me.udInputEHSClaim.Clear()

        Dim blnIsValid As Boolean = True
        Dim blnDisableSave As Boolean = False

        Dim strSelectedScheme As String = udtEHSTransactionWithoutTransactionDetail.SchemeCode

        Dim strServiceDate As String = Me.udtFormatter.formatInputDate(Me.txtEnterClaimDetailServiceDate.Text.Trim)
        Dim dtmServiceDate As DateTime


        Me.udtSM = udtValidator.chkServiceDate(Me.txtEnterClaimDetailServiceDate.Text.Trim)
        If Not Me.udtSM Is Nothing Then
            blnIsValid = False
            Me.imgEnterClaimDetailServiceDateErr.Visible = True
            Me.udcMessageBox.AddMessage(Me.udtSM)
            blnDisableSave = True
        Else
            strServiceDate = udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English)
            If DateTime.TryParse(strServiceDate, dtmServiceDate) Then
                'dtmServiceDate = udtFormatter.convertDate(Me.txtEnterClaimDetailServiceDate.Text.Trim, Common.Component.CultureLanguage.English)
            Else
                blnIsValid = False
                Me.imgEnterClaimDetailServiceDateErr.Visible = True
                Me.udtSM = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00120)
                Me.udcMessageBox.AddMessage(Me.udtSM)
                blnDisableSave = True
            End If
        End If

        Dim udtSchemeClaimList As SchemeClaimModelCollection = Nothing

        If blnIsValid Then
            Dim udtHCVUUser As HCVUUserModel
            Dim udtHCVUUserBLL As New HCVUUserBLL
            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            'udtSchemeClaimList = Me.udtSessionHandlerBLL.SchemeListGetFromSession(FunctionCode)
            udtSchemeClaimList = Me.udtSchemeClaimBLL.getSchemeClaimFromBackOfficeUserAndPractice(udtHCVUUser.UserID, _
                                                                                                  FunctionCode, _
                                                                                                  udtEHSTransactionWithoutTransactionDetail.ServiceProviderID, _
                                                                                                  udtEHSTransactionWithoutTransactionDetail.PracticeID, _
                                                                                                  dtmServiceDate)

            'Filter by date back claim min date
            udtSchemeClaimList = udtSchemeClaimList.FilterByDateBackClaimMinDate(dtmServiceDate)

            If IsNothing(udtSchemeClaimList) Then
                blnIsValid = False
                Me.imgEnterClaimDetailServiceDateErr.Visible = True
                Me.udtSM = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00279)
                Me.udcMessageBox.AddMessage(Me.udtSM)
                blnDisableSave = True
            Else
                'Filter by selected scheme
                If IsNothing(udtSchemeClaimList.Filter(strSelectedScheme)) Then
                    blnIsValid = False
                    Me.imgEnterClaimDetailServiceDateErr.Visible = True
                    Me.udtSM = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00279)
                    Me.udcMessageBox.AddMessage(Me.udtSM)
                    blnDisableSave = True
                End If
            End If

        End If

        If blnDisableSave Then
            Me.SetSaveButtonEnable(Me.ibtnEnterClaimDetailSave, False)
        End If

        'Me.ddlEnterClaimDetailsSchemeText_SelectedIndexChanged(Nothing, Nothing)

        'Show HKIC Symbol input if necessary
        If Not udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode.Trim) Is Nothing AndAlso _
            udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode.Trim).DocCode = DocTypeModel.DocTypeCode.HKIC Then
            EnableHKICSymbolRadioButtonList(True, strSelectedScheme, dtmServiceDate)
        End If

        If blnIsValid Then
            Me.udInputEHSClaim.ResetSchemeType()
            Me.udtSessionHandlerBLL.EHSClaimVaccineRemoveFromSession(FunctionCode)
            If Not IsNothing(udtSchemeClaimList.Filter(strSelectedScheme)) Then
                'The session ClaimCategory has been removed as before when trigger dropdownlist of scheme
                Dim udtSchemeClaim As SchemeClaimModel = udtSchemeClaimList.Filter(strSelectedScheme)

                Me.lblEnterClaimDetailScheme.Text = udtSchemeClaim.SchemeDesc

                ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
                ' --------------------------------------------------------------------------------------
                Select Case udtSchemeClaim.ControlType
                    Case SchemeClaimModel.EnumControlType.VSS
                        Dim udcInputVSS As ucInputVSS = Me.udInputEHSClaim.GetVSSControl()

                        If Not udcInputVSS Is Nothing Then
                            udcInputVSS.ClearClaimDetail()
                        End If

                    Case SchemeClaimModel.EnumControlType.ENHVSSO
                        Dim udcInputENHVSSO As ucInputENHVSSO = Me.udInputEHSClaim.GetENHVSSOControl()

                        If Not udcInputENHVSSO Is Nothing Then
                            udcInputENHVSSO.ClearClaimDetail()
                        End If

                    Case SchemeClaimModel.EnumControlType.PPP
                        Dim udcInputPPP As ucInputPPP = Me.udInputEHSClaim.GetPPPControl()

                        If Not udcInputPPP Is Nothing Then
                            udcInputPPP.ClearClaimDetail()
                        End If

                End Select
                ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

            End If

            'Me.udtSessionHandlerBLL.ClaimCategoryRemoveFromSession(FunctionCode)
            Me.SetUpEnterClaimDetails(False)

            'check exchange rate 
            Call CheckExchangeRateAbsence(dtmServiceDate)

            udtAuditLogEntry.AddDescripton(IIf(udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount, "eHS Validated Acc ID", "eHS Temp Acc ID"), _
                                           IIf(udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount, udtEHSAccount.VoucherAccID, udtEHSAccount.VoucherAccID))
            udtAuditLogEntry.AddDescripton("Service Date", Me.txtEnterClaimDetailServiceDate.Text.Trim)
            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00025, AuditLogDescription.NewClaimTransaction_ChangeServiceDate_Success)
        Else
            udtAuditLogEntry.AddDescripton(IIf(udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount, "eHS Validated Acc ID", "eHS Temp Acc ID"), _
                                           IIf(udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount, udtEHSAccount.VoucherAccID, udtEHSAccount.VoucherAccID))
            udtAuditLogEntry.AddDescripton("Service Date", Me.txtEnterClaimDetailServiceDate.Text.Trim)
            udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationFail, udtAuditLogEntry, LogID.LOG00026, AuditLogDescription.NewClaimTransaction_ChangeServiceDate_Fail)
        End If
    End Sub

    Private Sub udcInputEHSClaim_VaccineLegendClick(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    End Sub

    Private Sub udInputEHSClaim_CategorySelected(ByVal sender As Object, ByVal e As System.EventArgs) Handles udInputEHSClaim.CategorySelected
        Dim udtSchemeClaim As SchemeClaimModel = Me.udtSessionHandlerBLL.SelectSchemeGetFromSession(FunctionCode)
        Dim udtFormatter As Formatter = New Formatter
        Dim strCategory As String = Nothing

        ' Reset Error Message when Category changed
        Me.udcMessageBox.Clear()
        Me.udcInfoMessageBox.Clear()

        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Select Case udtSchemeClaim.ControlType
            Case SchemeClaimModel.EnumControlType.HSIVSS
                Dim udcInputHSIVSS As ucInputHSIVSS = Me.udInputEHSClaim.GetHSIVSSControl()
                strCategory = udcInputHSIVSS.Category

            Case SchemeClaimModel.EnumControlType.RVP
                Dim udcInputRVP As ucInputRVP = Me.udInputEHSClaim.GetRVPControl()
                strCategory = udcInputRVP.Category

            Case SchemeClaimModel.EnumControlType.VSS
                Dim udcInputVSS As ucInputVSS = Me.udInputEHSClaim.GetVSSControl()
                strCategory = udcInputVSS.Category

            Case SchemeClaimModel.EnumControlType.ENHVSSO
                Dim udcInputENHVSSO As ucInputENHVSSO = Me.udInputEHSClaim.GetENHVSSOControl()
                strCategory = udcInputENHVSSO.Category

            Case SchemeClaimModel.EnumControlType.PPP
                Dim udcInputPPP As ucInputPPP = Me.udInputEHSClaim.GetPPPControl()
                strCategory = udcInputPPP.Category

            Case SchemeClaimModel.EnumControlType.COVID19
                Dim udcInputCOVID19 As ucInputCOVID19 = Me.udInputEHSClaim.GetCOVID19Control()
                strCategory = udcInputCOVID19.Category

            Case SchemeClaimModel.EnumControlType.COVID19RVP
                Dim udcInputCOVID19RVP As ucInputCOVID19RVP = Me.udInputEHSClaim.GetCOVID19RVPControl()
                strCategory = udcInputCOVID19RVP.Category

            Case SchemeClaimModel.EnumControlType.COVID19OR
                Dim udcInputCOVID19OR As ucInputCOVID19OR = Me.udInputEHSClaim.GetCOVID19ORControl()
                strCategory = udcInputCOVID19OR.Category

        End Select
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        If String.IsNullOrEmpty(strCategory) Then
            Me.SetSaveButtonEnable(Me.ibtnEnterClaimDetailSave, False)
            Me.udtSessionHandlerBLL.ClaimCategoryRemoveFromSession(FunctionCode)
        Else
            Dim strServiceDate As String = udtFormatter.formatInputDate(Me.txtEnterClaimDetailServiceDate.Text)
            Dim dtmServiceDate As DateTime = udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English)
            Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
            Dim udtEHSAccount As EHSAccount.EHSAccountModel = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
            Dim udtPersonalInformation As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode)
            Dim udtClaimCategorys As ClaimCategory.ClaimCategoryModelCollection
            Dim udtClaimCategoryBLL As New ClaimCategory.ClaimCategoryBLL

            udtClaimCategorys = udtClaimCategoryBLL.getDistinctCategoryBySchemeOnly(udtSchemeClaim)

            Me.udtSessionHandlerBLL.ClaimCategorySaveToSession(udtClaimCategorys.FilterByCategoryCode(udtSchemeClaim.SchemeCode, strCategory), FunctionCode)
            Me.udtSessionHandlerBLL.EHSClaimVaccineRemoveFromSession(FunctionCode)
            Me.udInputEHSClaim.ResetSchemeType()

            Me.SetUpEnterClaimDetails(False)

        End If

        Me.udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationFail)

    End Sub

    Private Sub udInputEHSClaim_ClaimControlEventFired(ByVal strSchemeCode As String, ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udInputEHSClaim.ClaimControlEventFired

        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Select Case strSchemeCode
            Case SchemeClaimModel.VSS, SchemeClaimModel.RVP
                Session(SESS_SearchRVPHomeList) = True
                If CType(Me.udInputEHSClaim.GetRVPControl(), ucInputRVP).Category = CategoryCode.RVP_COVID19 Then
                    udtSessionHandlerBLL.ClaimCOVID19SaveToSession(True)
                End If
                Me.udcRVPHomeListSearch.BindRVPHomeList(Nothing)
                Me.udcRVPHomeListSearch.ClearFilter()

                Me.ibtnPopupRVPHomeListSearchSelect.Enabled = False
                Me.ibtnPopupRVPHomeListSearchSelect.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectDisableBtn")

                Me.ModalPopupExtenderRVPHomeListSearch.Show()

            Case SchemeClaimModel.PPP
                Session(SESS_SearchSchoolList) = True
                Me.udcSchoolListSearch.Scheme = SchemeClaimModel.PPP
                Me.udcSchoolListSearch.BindSchoolList(Nothing)
                Me.udcSchoolListSearch.ClearFilter()

                Me.ibtnPopupSchoolListSearchSelect.Enabled = False
                Me.ibtnPopupSchoolListSearchSelect.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectDisableBtn")

                Me.ModalPopupExtenderSchoolListSearch.Show()

            Case SchemeClaimModel.PPPKG
                Session(SESS_SearchSchoolList) = True
                Me.udcSchoolListSearch.Scheme = SchemeClaimModel.PPPKG
                Me.udcSchoolListSearch.BindSchoolList(Nothing)
                Me.udcSchoolListSearch.ClearFilter()

                Me.ibtnPopupSchoolListSearchSelect.Enabled = False
                Me.ibtnPopupSchoolListSearchSelect.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectDisableBtn")

                Me.ModalPopupExtenderSchoolListSearch.Show()

            Case SchemeClaimModel.COVID19RVP
                Session(SESS_SearchRVPHomeList) = True
                udtSessionHandlerBLL.ClaimCOVID19SaveToSession(True)
                Me.udcRVPHomeListSearch.BindRVPHomeList(Nothing)
                Me.udcRVPHomeListSearch.ClearFilter()

                Me.ibtnPopupRVPHomeListSearchSelect.Enabled = False
                Me.ibtnPopupRVPHomeListSearchSelect.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectDisableBtn")

                Me.ModalPopupExtenderRVPHomeListSearch.Show()

            Case Else
                Throw New Exception(String.Format("No available popup for scheme({0}).", strSchemeCode))

        End Select

        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

    End Sub

    Private Sub udInputEHSClaim_OutreachListSearchClicked(ByVal strSchemeCode As String, ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udInputEHSClaim.OutreachListSearchClicked

        Select Case strSchemeCode
            Case SchemeClaimModel.COVID19OR
                Session(SESS_SearchOutreachList) = True

                Me.udcOutreachSearch.BindOutreachList(Nothing)
                Me.udcOutreachSearch.ClearFilter()

                Me.btnPopupOutreachListSearchSelect.Enabled = False
                Me.btnPopupOutreachListSearchSelect.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectDisableBtn")

                Me.ModalPopupExtenderOutreachListSearch.Show()

        End Select

    End Sub

    Protected Sub ibtnEnterClaimDetailSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
        Dim udtEHSAccount As EHSAccount.EHSAccountModel = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        Dim udtEHSTransactionWithoutTransactionDetail As EHSTransactionModel = Me.udtSessionHandlerBLL.EHSTransactionWithoutTransactionDetailGetFromSession(FunctionCode)

        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton(IIf(udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount, "eHS Validated Acc ID", "eHS Temp Acc ID"), _
                                       IIf(udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount, udtEHSAccount.VoucherAccID, udtEHSAccount.VoucherAccID))
        udtAuditLogEntry.AddDescripton("Scheme Code", udtEHSTransactionWithoutTransactionDetail.SchemeCode)
        udtAuditLogEntry.AddDescripton("Service Date", Me.txtEnterClaimDetailServiceDate.Text.Trim)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00027, AuditLogDescription.NewClaimTransaction_EnterClaimDetail)

        ClearClaimControlErrorImage()

        Dim blnIsValid As Boolean = True
        Dim blnNeedOverrideReason As Boolean = False
        Dim udtEHSTransaction As EHSTransactionModel

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        'udtEHSTransaction = Me.udtSessionHandlerBLL.EHSTransactionGetFromSession(FunctionCode)
        'Dim udtEHSTransactionWithoutTransactionDetail As EHSTransactionModel = Me.udtSessionHandlerBLL.EHSTransactionWithoutTransactionDetailGetFromSession(FunctionCode)

        udtEHSTransaction = New EHSTransactionModel(udtEHSTransactionWithoutTransactionDetail)
        udtEHSTransaction.EHSAcct = udtEHSTransactionWithoutTransactionDetail.EHSAcct
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]
        udtEHSTransaction.OverrideReason = String.Empty
        udtEHSTransaction.WarningMessage = Nothing

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL
        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

        Dim udtSchemeClaim As SchemeClaimModel
        udtSchemeClaim = Me.udtSessionHandlerBLL.SelectSchemeGetFromSession(FunctionCode)

        Dim strServiceDate As String = Me.udtFormatter.formatInputDate(Me.txtEnterClaimDetailServiceDate.Text.Trim)
        Dim dtmServiceDate As DateTime
        Me.imgEnterClaimDetailServiceDateErr.Visible = False

        Me.udtSM = udtValidator.chkServiceDate(Me.txtEnterClaimDetailServiceDate.Text.Trim)
        If Not Me.udtSM Is Nothing Then
            blnIsValid = False
            Me.imgEnterClaimDetailServiceDateErr.Visible = True
            Me.udcMessageBox.AddMessage(Me.udtSM)
        Else
            strServiceDate = udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English)
            If Not DateTime.TryParse(strServiceDate, dtmServiceDate) Then
                dtmServiceDate = udtFormatter.convertDate(Me.txtEnterClaimDetailServiceDate.Text.Trim, Common.Component.CultureLanguage.English)
            End If
            ' Me.txtEnterClaimDetailServiceDate.Text = strServiceDate
        End If

        If blnIsValid Then
            udtEHSTransaction.DocCode = udtEHSAccount.SearchDocCode
            udtEHSTransaction.SchemeCode = udtSchemeClaim.SchemeCode.Trim
            udtEHSTransaction.ServiceDate = dtmServiceDate

            Dim udtEHSClaimBLL As New BLL.EHSClaimBLL
            ' Check Service date with profession claim period
            Me.udtSM = udtEHSClaimBLL.CheckServiceDateClaimPeriod(udtEHSTransaction.SchemeCode, udtEHSTransaction.ServiceDate.AddDays(1).AddMinutes(-1))
            If Not Me.udtSM Is Nothing Then
                blnIsValid = False
                Me.imgEnterClaimDetailServiceDateErr.Visible = True
                Me.udcMessageBox.AddMessage(Me.udtSM)
            End If
        End If

        ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]
        ' -----------------------------------------------------------------------------------------
        If blnIsValid Then
            ' Check Service date with profession claim period
            Dim udtPractice As Practice.PracticeBLL.PracticeDisplayModel = Me.udtSessionHandlerBLL.PracticeDisplayGetFromSession(FunctionCode)

            If Not udtPractice.Profession.IsClaimPeriod(udtEHSTransaction.ServiceDate) Then
                blnIsValid = False
                Me.imgEnterClaimDetailServiceDateErr.Visible = True
                Me.udtSM = New Common.ComObject.SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00150) ' The "Service Date" should not be earlier than %s
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Me.udcMessageBox.AddMessage(Me.udtSM, "%s", udtFormatter.formatDate(udtPractice.Profession.ClaimPeriodFrom.Value, Me.udtSessionHandlerBLL.Language))
                Me.udcMessageBox.AddMessage(Me.udtSM, "%s", udtFormatter.formatDisplayDate(udtPractice.Profession.ClaimPeriodFrom.Value))
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            End If
        End If
        ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        If blnIsValid Then
            'Default value on HKIC symbol
            Me.imgErrHKICSymbol.Visible = False
            udtEHSTransaction.EHSAcct.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode).HKICSymbol = String.Empty
            udtEHSTransaction.HKICSymbol = String.Empty
            udtEHSTransaction.OCSSSRefStatus = String.Empty

            'If HKIC, go to validation
            If udtEHSTransaction.EHSAcct.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode).DocCode = DocTypeModel.DocTypeCode.HKIC Then
                'If HKIC symbol selection is shown, check symbol whether is inputted
                If panHKICSymbol.Visible = True Then
                    'Collect value from HKIC symbol selection
                    udtEHSTransaction.EHSAcct.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode).HKICSymbol = rblHKICSymbol.SelectedValue.Trim
                    udtEHSTransaction.HKICSymbol = rblHKICSymbol.SelectedValue.Trim
                    udtEHSTransaction.OCSSSRefStatus = (New Common.OCSSS.OCSSSResult(Common.OCSSS.OCSSSResult.OCSSSConnection.SkipForChecking, Nothing)).OCSSSStatus

                    'If no input, arise the warning
                    If rblHKICSymbol.SelectedValue = String.Empty Then
                        blnIsValid = False
                        Me.imgErrHKICSymbol.Visible = True

                        Me.udtSM = New Common.ComObject.SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00028) ' Please input "%s".
                        Me.udcMessageBox.AddMessage(Me.udtSM, "%s", lblHKICSymbolText.Text)
                    End If

                End If

            End If

        End If
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        If blnIsValid Then
            Select Case udtSchemeClaim.ControlType
                Case SchemeClaimModel.EnumControlType.CIVSS
                    blnIsValid = Me.CIVSSValidation(udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.EVSS
                    blnIsValid = Me.EVSSValidation(udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.VOUCHER
                    blnIsValid = Me.HCVSValidation(udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.VOUCHERCHINA
                    blnIsValid = Me.HCVSChinaValidation(udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.HSIVSS
                    blnIsValid = Me.HSIVSSValidation(udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.RVP
                    blnIsValid = Me.RVPValidation(udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.EHAPP
                    blnIsValid = Me.EHAPPValidation(udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.PIDVSS
                    blnIsValid = Me.PIDVSSValidation(udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.VSS
                    blnIsValid = Me.VSSValidation(udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.ENHVSSO
                    blnIsValid = Me.ENHVSSOValidation(udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.PPP
                    blnIsValid = Me.PPPValidation(udtEHSTransaction)

                    ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                Case SchemeClaimModel.EnumControlType.SSSCMC
                    blnIsValid = Me.SSSCMCValidation(udtEHSTransaction)
                    ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

                    ' CRE20-0023 (Immu record) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                Case SchemeClaimModel.EnumControlType.COVID19
                    blnIsValid = COVID19Validation(udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.COVID19RVP
                    blnIsValid = COVID19RVPValidation(udtEHSTransaction)

                Case SchemeClaimModel.EnumControlType.COVID19OR
                    blnIsValid = COVID19ORValidation(udtEHSTransaction)

                    ' CRE20-0023 (Immu record) [End][Chris YIM]
            End Select

        End If

        Dim udtEHSClaimVaccine As EHSClaimVaccine.EHSClaimVaccineModel
        udtEHSClaimVaccine = Me.udtSessionHandlerBLL.EHSClaimVaccineGetFromSession(FunctionCode)

        Select Case udtSchemeClaim.ControlType
            Case SchemeClaimModel.EnumControlType.VOUCHER
                Me.AuditLogVoucher(udtAuditLogEntry, udtSchemeClaim.SchemeCode, dtmServiceDate)

            Case SchemeClaimModel.EnumControlType.VOUCHERCHINA
                Me.AuditLogChinaVoucher(udtAuditLogEntry, udtSchemeClaim.SchemeCode, dtmServiceDate)

            Case SchemeClaimModel.EnumControlType.CIVSS, SchemeClaimModel.EnumControlType.EVSS, SchemeClaimModel.EnumControlType.HSIVSS, _
                 SchemeClaimModel.EnumControlType.RVP, SchemeClaimModel.EnumControlType.PIDVSS, SchemeClaimModel.EnumControlType.VSS, _
                 SchemeClaimModel.EnumControlType.ENHVSSO, SchemeClaimModel.EnumControlType.PPP, SchemeClaimModel.EnumControlType.COVID19, _
                 SchemeClaimModel.EnumControlType.COVID19RVP, SchemeClaimModel.EnumControlType.COVID19OR

                Me.AuditLogVaccination(udtAuditLogEntry, udtEHSClaimVaccine, dtmServiceDate, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.EHAPP
                Me.AuditLogEHAPP(udtAuditLogEntry, udtSchemeClaim.SchemeCode, dtmServiceDate)

                ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
            Case SchemeClaimModel.EnumControlType.SSSCMC
                Me.AuditLogSSSCMC(udtAuditLogEntry, udtEHSTransaction, dtmServiceDate)
                ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

        End Select


        Dim udtValidationResults As EHSClaim.EHSClaimBLL.EHSClaimBLL.ValidationResults 'OutsideClaimValidation.OutsideClaimValidationModel
        Dim udtBlockMessage As EHSClaim.EHSClaimBLL.EHSClaimBLL.RuleResultList = Nothing
        Dim udtWarningMessage As EHSClaim.EHSClaimBLL.EHSClaimBLL.RuleResultList = Nothing

        If blnIsValid Then
            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim udtCommonEHSClaimBLL As New Common.Component.EHSClaim.EHSClaimBLL.EHSClaimBLL
            Dim udtHAVaccineResult As Common.WebService.Interface.HAVaccineResult = udtSessionHandlerBLL.CMSVaccineResultGetFromSession(FunctionCode)
            Dim udtDHVaccineResult As Common.WebService.Interface.DHVaccineResult = udtSessionHandlerBLL.CIMSVaccineResultGetFromSession(FunctionCode)

            'If nothing, get HA Vaccine through CMS 
            If udtHAVaccineResult Is Nothing Then
                Dim udtWSProxyCMS As New Common.WebService.Interface.WSProxyCMS(Me.udtAuditLogEntry)
                udtHAVaccineResult = udtWSProxyCMS.GetVaccine(udtEHSAccount)
                udtSessionHandlerBLL.CMSVaccineResultSaveToSession(udtHAVaccineResult, FunctionCode)
            End If

            'If nothing, get DH Vaccine through CIMS 
            If udtDHVaccineResult Is Nothing Then
                Dim udtWSProxyCIMS As New Common.WebService.Interface.WSProxyDHCIMS(Me.udtAuditLogEntry)
                udtDHVaccineResult = udtWSProxyCIMS.GetVaccine(udtEHSAccount)
                udtSessionHandlerBLL.CIMSVaccineResultSaveToSession(udtDHVaccineResult, FunctionCode)
            End If

            If udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVaccine Then
                '----------------
                ' Vaccine Type
                '----------------
                Dim udtTransactionBenefitDetailList As TransactionDetailVaccineModelCollection = Nothing
                'Get EHS Vaccine to Benefit List
                udtTransactionBenefitDetailList = udtEHSTransactionBLL.getTransactionDetailVaccine(udtEHSTransaction.DocCode, udtEHSAccount.getPersonalInformation(udtEHSTransaction.DocCode).IdentityNum)

                Dim objVaccinationBLL As New VaccinationBLL

                'Add HA Vaccine to Benefit List
                If objVaccinationBLL.SchemeContainVaccine(udtSchemeClaim) Then
                    If Not udtHAVaccineResult Is Nothing And Not udtDHVaccineResult Is Nothing Then
                        udtTransactionBenefitDetailList.JoinVaccineList(udtEHSAccount.getPersonalInformation(udtEHSTransaction.DocCode), udtHAVaccineResult.SinglePatient.VaccineList, udtAuditLogEntry, udtSchemeClaim.SchemeCode)
                        udtTransactionBenefitDetailList.JoinVaccineList(udtEHSAccount.getPersonalInformation(udtEHSTransaction.DocCode), udtDHVaccineResult.SingleClient.VaccineRecordList, udtAuditLogEntry, udtSchemeClaim.SchemeCode)
                    End If
                End If

                udtEHSTransaction.HAVaccineRefStatus = New EHSTransaction.EHSTransactionModel.ExtRefStatusClass(udtHAVaccineResult, udtEHSTransaction.DocCode).Code
                udtEHSTransaction.DHVaccineRefStatus = New EHSTransaction.EHSTransactionModel.ExtRefStatusClass(udtDHVaccineResult, udtEHSTransaction.DocCode).Code

                Dim dicVaccineRef As Dictionary(Of String, String) = EHSTransactionModel.GetVaccineRef(udtTransactionBenefitDetailList, udtEHSTransaction)
                udtEHSTransaction.EHSVaccineResult = dicVaccineRef(EHSTransactionModel.VaccineRefType.EHS)
                udtEHSTransaction.HAVaccineResult = dicVaccineRef(EHSTransactionModel.VaccineRefType.HA)
                udtEHSTransaction.DHVaccineResult = dicVaccineRef(EHSTransactionModel.VaccineRefType.DH)

                Me.udtEHSClaimBLL.ConstructEHSTransactionDetails(udtEHSTransaction, udtEHSAccount, Me.udtSessionHandlerBLL.EHSClaimVaccineGetFromSession(FunctionCode), udtHCVUUser.UserID)


            ElseIf udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeRegistration Then
                '-------------------
                ' Registration Type
                '-------------------
                Me.udtEHSClaimBLL.ConstructEHSTransDetail_Registration(udtEHSTransaction, udtEHSAccount, udtHCVUUser.UserID)

                ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
            ElseIf udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeType_HAService Then
                '-------------------
                ' HA Service Type
                '-------------------
                Me.udtEHSClaimBLL.ConstructEHSTransactionDetail_SSSCMC(udtEHSTransaction, udtEHSAccount, udtHCVUUser.UserID, Me.udtSessionHandlerBLL.HAPatientGetFromSession())

                ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

            Else
                '-------------------
                ' Voucher Type
                '-------------------
                udtEHSTransaction.PerVoucherValue = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVoucher, dtmServiceDate).SubsidizeFee

                Me.udtEHSClaimBLL.ConstructEHSTransactionDetails(udtEHSTransaction, udtEHSAccount, udtHCVUUser.UserID)
            End If

            udtValidationResults = udtCommonEHSClaimBLL.ValidateClaimCreation(EHSClaim.EHSClaimBLL.EHSClaimBLL.ClaimAction.HCVUClaim, udtEHSTransaction, udtHAVaccineResult, udtDHVaccineResult, udtAuditLogEntry)

            'Handle blocking message
            udtBlockMessage = udtValidationResults.BlockResults
            For Each udtBlock As EHSClaim.EHSClaimBLL.EHSClaimBLL.RuleResult In udtBlockMessage.RuleResults
                If Not IsNothing(udtBlock.MessageVariableNameArrayList) AndAlso Not IsNothing(udtBlock.MessageVariableValueArrayList) AndAlso _
                   udtBlock.MessageVariableNameArrayList.Count > 0 AndAlso udtBlock.MessageVariableValueArrayList.Count > 0 Then

                    Me.udcMessageBox.AddMessage(udtBlock.ErrorMessage, udtBlock.MessageVariableNameArrayList.ToArray(Type.GetType("System.String")), udtBlock.MessageVariableValueArrayList.ToArray(Type.GetType("System.String")))
                Else
                    Me.udcMessageBox.AddMessage(udtBlock.ErrorMessage)
                End If

                blnIsValid = False
            Next

            'Handle warning message
            If blnIsValid Then
                udtWarningMessage = udtValidationResults.WarningResults
                If udtWarningMessage.RuleResults.Count > 0 Then
                    udtEHSTransaction.WarningMessage = udtWarningMessage
                    For Each udtWarning As EHSClaim.EHSClaimBLL.EHSClaimBLL.RuleResult In udtWarningMessage.RuleResults
                        If Not IsNothing(udtWarning.MessageVariableNameArrayList) AndAlso Not IsNothing(udtWarning.MessageVariableValueArrayList) AndAlso _
                           udtWarning.MessageVariableNameArrayList.Count > 0 AndAlso udtWarning.MessageVariableValueArrayList.Count > 0 Then

                            Me.udcWarningMessageBox.AddMessage(udtWarning.ErrorMessage, udtWarning.MessageVariableNameArrayList.ToArray(Type.GetType("System.String")), udtWarning.MessageVariableValueArrayList.ToArray(Type.GetType("System.String")))
                        Else
                            Me.udcWarningMessageBox.AddMessage(udtWarning.ErrorMessage)
                        End If

                        blnNeedOverrideReason = True
                    Next
                End If
            End If


            'If udtClaimRuleResult IsNot Nothing Then
            '    Select Case (sm.FunctionCode.ToString + "-" + sm.SeverityCode.ToString + "-" + sm.MessageCode.ToString)
            '        Case "990000-E-00461"
            '            Me.udcMsgBoxErr.AddMessage(sm, "%s", udtClaimRuleResult.ResultParam("%DaysApart"))
            '        Case Else
            '            Me.udcMsgBoxErr.AddMessage(sm)
            '    End Select
            'Else
            '    Me.udcMsgBoxErr.AddMessage(sm)
            'End If

        End If

        If blnIsValid Then
            Me.udtSessionHandlerBLL.EHSTransactionRemoveFromSession(FunctionCode)
            Me.udtSessionHandlerBLL.EHSTransactionSaveToSession(udtEHSTransaction, FunctionCode)

            Me.udcConfirmClaimCreation.EnableVaccinationRecordChecking = False

            Me.udcConfirmClaimCreation.ShowHKICSymbol = True
            Me.udcConfirmClaimCreation.ShowOCSSSCheckingResult = False
            ' CRE20-0023 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Me.udcConfirmClaimCreation.ShowContactNoNotAbleToSMS = True
            ' CRE20-0023 (Immu record) [End][Chris YIM]
            Me.udcConfirmClaimCreation.LoadTranInfo(udtEHSTransaction, New DataTable())

            If blnNeedOverrideReason Then

                Me.txtConfirmClaimCreationOverrideReason.Text = String.Empty
                Me.imgConfirmClaimCreationOverrideReason.Visible = False

                If Not udtWarningMessage Is Nothing Then
                    If udtWarningMessage.RuleResults.Count > 6 Then
                        pnlWarningMsgContent.Attributes.Remove("style")
                        pnlWarningMsgContent.Attributes.Add("style", "height: 300px;overflow: auto;")
                    Else
                        pnlWarningMsgContent.Attributes.Remove("style")
                        pnlWarningMsgContent.Attributes.Add("style", "height: auto;overflow: auto;")
                    End If
                End If


                'udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationWarning, udtAuditLogEntry, LogID.LOG00064, AuditLogDescription.NewClaimTransaction_CreationDetailsClick)
                udcWarningMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationWarning, udtAuditLogEntry, LogID.LOG00031, AuditLogDescription.NewClaimTransaction_EnterClaimDetail_WarningMsg)
                Me.udcOverrideReasonMsgBox.Visible = False
                Me.ModalPopupExtenderWarningMessage.Show()

                udtAuditLogEntry.AddDescripton(IIf(udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount, "eHS Validated Acc ID", "eHS Temp Acc ID"), _
                                               IIf(udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount, udtEHSAccount.VoucherAccID, udtEHSAccount.VoucherAccID))
                udtAuditLogEntry.AddDescripton("Has Warning Msg", "Y")
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00028, AuditLogDescription.NewClaimTransaction_EnterClaimDetail_Success)


            Else

                Me.udcInfoMessageBox.AddMessage(New SystemMessage("990000", SeverityCode.SEVI, MsgCode.MSG00021))
                Me.udcInfoMessageBox.BuildMessageBox()
                Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
                Me.mvNewClaimTransaction.ActiveViewIndex = NewClaimTransaction.Confirm

                udtAuditLogEntry.AddDescripton(IIf(udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount, "eHS Validated Acc ID", "eHS Temp Acc ID"), _
                                               IIf(udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount, udtEHSAccount.VoucherAccID, udtEHSAccount.VoucherAccID))
                udtAuditLogEntry.AddDescripton("Has Warning Msg", "N")
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00028, AuditLogDescription.NewClaimTransaction_EnterClaimDetail_Success)

            End If
            'Me.mvEnterDetails.ActiveViewIndex = InputTransactionDetails.Confirm
        Else
            udtAuditLogEntry.AddDescripton(IIf(udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount, "eHS Validated Acc ID", "eHS Temp Acc ID"), _
                                           IIf(udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount, udtEHSAccount.VoucherAccID, udtEHSAccount.VoucherAccID))
            udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationFail, udtAuditLogEntry, LogID.LOG00029, AuditLogDescription.NewClaimTransaction_EnterClaimDetail_Fail)
        End If


    End Sub

    Protected Sub ibtnEnterClaimDetailBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        Me.udcMessageBox.Visible = False
        Me.udcInfoMessageBox.Visible = False

        'Dim udtEHSAccount As EHSAccount.EHSAccountModel
        'Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
        'udtEHSAccount = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        'udtAuditLogEntry.AddDescripton("eHS Validated Acc ID", udtEHSAccount.VoucherAccID)

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        ' Clear Inputted value if cancel claim
        Me.udInputEHSClaim.Clear()
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Koala]

        'CRE13-006 HCVS Ceiling [Start][Karl]
        Me.udInputEHSClaim.ClearErrorMessage()
        'CRE13-006 HCVS Ceiling [End][Karl]

        Me.mvNewClaimTransaction.ActiveViewIndex = NewClaimTransaction.EnterClaimDetails
        Me.mvEnterDetails.ActiveViewIndex = InputTransactionDetails.CreationDetails

        ClearEnterCreationDetailsErrorImage()

        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00030, AuditLogDescription.NewClaimTransaction_EnterClaimDetail_Back)
    End Sub

#Region "Voucher Reason for Visit"
    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    <WebMethod()> _
    <System.Web.Script.Services.ScriptMethod()> _
    Public Shared Function GetReasonForVisitL1(ByVal knownCategoryValues As String, ByVal category As String, ByVal contextKey As String) As CascadingDropDownNameValue()

        Dim udtReasonforVisitBLL As ReasonForVisitBLL = New ReasonForVisitBLL
        Dim dtReasonForVisit As DataTable

        dtReasonForVisit = udtReasonforVisitBLL.getReasonForVisitL1(category)

        Dim lst As New List(Of CascadingDropDownNameValue)
        For Each dr As DataRow In dtReasonForVisit.Rows
            lst.Add(New CascadingDropDownNameValue(dr("Reason_L1"), dr("Reason_L1_Code")))
        Next

        Return lst.ToArray
    End Function

    <WebMethod()> _
    <System.Web.Script.Services.ScriptMethod()> _
    Public Shared Function GetReasonForVisitL2(ByVal knownCategoryValues As String, ByVal category As String, ByVal contextKey As String) As CascadingDropDownNameValue()

        Dim udtReasonforVisitBLL As ReasonForVisitBLL = New ReasonForVisitBLL
        Dim dtReasonForVisit As DataTable
        Dim kv As StringDictionary

        Dim arrCategoryValues() As String = knownCategoryValues.Split(New Char() {";"}, StringSplitOptions.RemoveEmptyEntries)
        If arrCategoryValues.Length = 1 Then
            kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Else
            kv = CascadingDropDown.ParseKnownCategoryValuesString(arrCategoryValues(arrCategoryValues.Length - 1) + ";")
        End If

        dtReasonForVisit = udtReasonforVisitBLL.getReasonForVisitL2(category, kv(category))

        Dim lst As New List(Of CascadingDropDownNameValue)
        For Each dr As DataRow In dtReasonForVisit.Rows
            lst.Add(New CascadingDropDownNameValue(dr("Reason_L2"), dr("Reason_L2_Code")))
        Next

        Return lst.ToArray
    End Function

    Private Function CovertReasonForVisitToArray(ByVal dtReasonForVisit As DataTable) As CascadingDropDownNameValue()
        Dim lst As New List(Of CascadingDropDownNameValue)
        For Each dr As DataRow In dtReasonForVisit.Rows
            lst.Add(New CascadingDropDownNameValue(dr("Reason_L2"), dr("Reason_L2_Code")))
        Next

        Return lst.ToArray
    End Function
    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Koala]
#End Region

#Region "Vaccination Record"

    Protected Sub ibtnVaccinationRecord_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00022, AuditLogDescription.VaccinationRecordClick)

        Dim udtEHSAccount As EHSAccountModel = (New eHSAccountMaintBLL).EHSAccountGetFromSession(FunctionCode)

        ' INT18-0033 (Fix selection of document for vaccination record enquiry in back-office platform) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Dim udtEHSAccountClone As EHSAccount.EHSAccountModel = Nothing
        Dim udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = Nothing

        'Clone EHSAccount
        udtEHSAccountClone = New EHSAccountModel(udtEHSAccount)
        udtEHSPersonalInfo = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)

        udtEHSAccountClone.SetPersonalInformation(udtEHSPersonalInfo)
        udtEHSAccountClone.SetSearchDocCode(udtEHSAccount.SearchDocCode)

        'ucVaccinationRecord.Build(udtEHSAccount, New AuditLogEntry(FunctionCode, Me))
        ucVaccinationRecord.Build(udtEHSAccountClone, New AuditLogEntry(FunctionCode, Me))

        ' INT18-0033 (Fix selection of document for vaccination record enquiry in back-office platform) [End][Chris YIM]

        Dim udtSession As New BLL.SessionHandlerBLL()
        udtSession.CMSVaccineResultSaveToSession(ucVaccinationRecord.HAVaccineResult, FunctionCode)
        udtSession.CIMSVaccineResultSaveToSession(ucVaccinationRecord.DHVaccineResult, FunctionCode)

        popupVaccinationRecord.Show()
        ViewState(VS.VaccinationRecordPopupStatus) = VaccinationRecordPopupStatusClass.Active
        ViewState(VS.VaccinationRecordPopupShown) = VaccinationRecordPopupShownClass.Active

    End Sub

    Protected Sub ibtnVaccinationRecordClose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00023, AuditLogDescription.VaccinationRecordCloseClick)

        ViewState.Remove(VS.VaccinationRecordPopupStatus)
        popupVaccinationRecord.Hide()

    End Sub

    ''' <summary>
    ''' Get EHS Vaccination record and CMS Vaccination record, and Join together by current claiming scheme (no cache)
    ''' </summary>
    ''' <param name="udtEHSAccount"></param>
    ''' <param name="strSchemeCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetVaccinationRecord(ByVal udtEHSAccount As EHSAccountModel, Optional ByVal strSchemeCode As String = "") As TransactionDetailVaccineModelCollection
        Dim udtTranDetailVaccineList As TransactionDetailVaccineModelCollection = Nothing
        Dim udtHAVaccineResult As Common.WebService.Interface.HAVaccineResult = Nothing
        Dim udtDHVaccineResult As Common.WebService.Interface.DHVaccineResult = Nothing
        'Dim htRecordSummary As Hashtable = Nothing
        Dim htRecordSummary As New Hashtable

        Dim udtVaccinationBLL As New VaccinationBLL

        Dim udtVaccineResultBag As New VaccineResultCollection
        udtVaccineResultBag.DHVaccineResult = udtDHVaccineResult
        udtVaccineResultBag.HAVaccineResult = udtHAVaccineResult

        udtVaccinationBLL.GetVaccinationRecord(udtEHSAccount, udtTranDetailVaccineList, udtVaccineResultBag, htRecordSummary, New AuditLogEntry(FunctionCode, Me), strSchemeCode)
        'Dim enumDHStatus As VaccinationBLL.EnumVaccinationRecordReturnStatus = udtVaccinationBLL.GetVaccinationRecord(udtEHSAccount, udtTranDetailVaccineList, udtDHVaccineResult, htRecordSummary, New AuditLogEntry(FunctionCode, Me), strSchemeCode, Nothing, False)

        Return udtTranDetailVaccineList
    End Function

    ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Public Function GetVaccinationRecordFromSession(ByVal udtEHSAccount As EHSAccountModel, Optional ByVal strSchemeCode As String = "") As VaccineResultCollection
        Dim udtVaccinationBLL As New VaccinationBLL
        Dim udtEHSTransactionBLL As New EHSTransactionBLL
        Dim udtSession As New BLL.SessionHandlerBLL

        Dim htRecordSummary As New Hashtable
        Dim udtTranDetailVaccineList As TransactionDetailVaccineModelCollection = Nothing

        'HA CMS
        Dim udtHAVaccineResult As HAVaccineResult = New HAVaccineResult(HAVaccineResult.enumReturnCode.Error)
        Dim udtHAVaccineResultSession As HAVaccineResult = udtSession.CMSVaccineResultGetFromSession(FunctionCode)

        'DH CIMS
        Dim udtDHVaccineResult As DHVaccineResult = New DHVaccineResult(DHVaccineResult.enumReturnCode.UnexpectedError)
        Dim udtDHVaccineResultSession As DHVaccineResult = udtSession.CIMSVaccineResultGetFromSession(FunctionCode)

        Dim udtVaccineResultBag As New VaccineResultCollection
        udtVaccineResultBag.DHVaccineResult = udtDHVaccineResult
        udtVaccineResultBag.HAVaccineResult = udtHAVaccineResult

        Dim udtVaccineResultBagSession As New VaccineResultCollection
        udtVaccineResultBagSession.DHVaccineResult = udtDHVaccineResultSession
        udtVaccineResultBagSession.HAVaccineResult = udtHAVaccineResultSession

        'Enquiry vaccine record
        udtVaccinationBLL.GetVaccinationRecord(udtEHSAccount, udtTranDetailVaccineList, udtVaccineResultBag, htRecordSummary, New AuditLogEntry(FunctionCode, Me), strSchemeCode, udtVaccineResultBagSession)

        'Save Vaccine Result to Session Variables
        udtSession.CMSVaccineResultSaveToSession(udtVaccineResultBag.HAVaccineResult, FunctionCode)
        udtSession.CIMSVaccineResultSaveToSession(udtVaccineResultBag.DHVaccineResult, FunctionCode)

        Return udtVaccineResultBag

    End Function
    ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

    ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Private Sub BuildSystemMessage(ByVal udtVaccineResultBag As VaccineResultCollection)
        Dim udtSystemMessage As SystemMessage = Nothing
        Dim udtSystemMessageList As New List(Of SystemMessage)
        Dim blnHAError As Boolean = False
        Dim blnDHError As Boolean = False
        Dim strLogID As String = String.Empty
        Dim strDescription As String = String.Empty

        If VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N Then
            If udtVaccineResultBag.HAVaccineResult Is Nothing Then
                If udtVaccineResultBag.HAReturnStatus = VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail Then
                    udtSystemMessageList.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00272))
                    strLogID = LogID.LOG00019
                    strDescription = AuditLogDescription.HAConnectionFail

                    blnHAError = True
                End If
            Else
                Select Case udtVaccineResultBag.HAReturnStatus
                    Case VaccinationBLL.EnumVaccinationRecordReturnStatus.DocumentNotAccept
                        'Nothing to do
                    Case Else
                        If udtVaccineResultBag.HAVaccineResult.ReturnCode <> HAVaccineResult.enumReturnCode.SuccessWithData Then
                            udtSystemMessageList.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00272))
                            strLogID = LogID.LOG00019
                            strDescription = AuditLogDescription.HAConnectionFail

                            blnHAError = True
                        End If
                End Select
            End If
        End If

        If VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CIMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N Then
            If udtVaccineResultBag.DHVaccineResult Is Nothing Then
                If udtVaccineResultBag.DHReturnStatus = VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail Then
                    udtSystemMessageList.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00411))
                    strLogID = LogID.LOG00020
                    strDescription = AuditLogDescription.DHConnectionFail

                    blnDHError = True
                End If
            Else
                Select Case udtVaccineResultBag.DHReturnStatus
                    Case VaccinationBLL.EnumVaccinationRecordReturnStatus.DocumentNotAccept
                        'Nothing to do
                    Case Else
                        If udtVaccineResultBag.DHVaccineResult.ReturnCode <> DHVaccineResult.enumReturnCode.Success Then
                            udtSystemMessageList.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00411))
                            strLogID = LogID.LOG00020
                            strDescription = AuditLogDescription.DHConnectionFail

                            blnDHError = True
                        End If
                End Select
            End If
        End If

        If blnHAError And blnDHError Then
            udtSystemMessageList.Clear()
            udtSystemMessageList.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00412))
            strLogID = LogID.LOG00021
            strDescription = AuditLogDescription.HADHConnectionFail
        End If

        For Each udtSystemMessage In udtSystemMessageList
            If Not udtSystemMessage Is Nothing Then
                Select Case udtSystemMessage.SeverityCode
                    Case "E"
                        udcMessageBox.AddMessage(udtSystemMessage)
                        udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ConnectionFail, udtAuditLogEntry, strLogID, strDescription)
                    Case Else
                        'Not to show MessageBox
                End Select
            End If
        Next
    End Sub
    ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

#End Region

#Region "Override Reason"
    Protected Sub ibtnWarningMessageCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        Dim udtEHSAccount As EHSAccount.EHSAccountModel
        Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
        udtEHSAccount = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        udtAuditLogEntry.AddDescripton(IIf(udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount, "eHS Validated Acc ID", "eHS Temp Acc ID"), _
                                       IIf(udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount, udtEHSAccount.VoucherAccID, udtEHSAccount.VoucherAccID))

        Me.ModalPopupExtenderWarningMessage.Hide()

        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        ' Clear the saved Transaction Details and Transaction Additional Fields from the Transaction
        Dim udtEHSTransaction As EHSTransactionModel
        udtEHSTransaction = Me.udtSessionHandlerBLL.EHSTransactionGetFromSession(FunctionCode)

        If Not udtEHSTransaction.TransactionDetails Is Nothing Then
            udtEHSTransaction.TransactionDetails.Clear()
            udtEHSTransaction.TransactionDetails = Nothing
        End If

        If Not udtEHSTransaction.TransactionAdditionFields Is Nothing Then
            udtEHSTransaction.TransactionAdditionFields.Clear()
            udtEHSTransaction.TransactionAdditionFields = Nothing
        End If

        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        Me.udtSessionHandlerBLL.EHSTransactionSaveToSession(udtEHSTransaction, FunctionCode)

        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00035, AuditLogDescription.NewClaimTransaction_EnterOverrideReason_Cancel)
    End Sub

    Protected Sub ibtnWarningMessageConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        Dim udtEHSAccount As EHSAccount.EHSAccountModel
        Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
        udtEHSAccount = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        udtAuditLogEntry.AddDescripton(IIf(udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount, "eHS Validated Acc ID", "eHS Temp Acc ID"), _
                                       IIf(udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount, udtEHSAccount.VoucherAccID, udtEHSAccount.VoucherAccID))
        udtAuditLogEntry.AddDescripton("Override Reason", txtConfirmClaimCreationOverrideReason.Text.Trim)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00032, AuditLogDescription.NewClaimTransaction_EnterOverrideReason)

        Me.udcOverrideReasonMsgBox.Visible = False
        Me.udcInfoMessageBox.Visible = False

        Dim blnError As Boolean = False

        imgConfirmClaimCreationOverrideReason.Visible = False
        If Me.txtConfirmClaimCreationOverrideReason.Text.Trim = String.Empty Then
            imgConfirmClaimCreationOverrideReason.Visible = True

            ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            'udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00015)
            'Me.udcOverrideReasonMsgBox.AddMessage(Me.udtSM)
            udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001)
            Me.udcOverrideReasonMsgBox.AddMessage(udtSM, "%s", lblConfirmClaimCreationOverrideReasonText.Text)
            ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Winnie]
            Me.ModalPopupExtenderWarningMessage.Show()
            blnError = True
        End If

        If Not blnError Then

            Dim udtEHSTransaction As EHSTransactionModel
            udtEHSTransaction = Me.udtSessionHandlerBLL.EHSTransactionGetFromSession(FunctionCode)
            udtEHSTransaction.OverrideReason = Me.txtConfirmClaimCreationOverrideReason.Text.Trim

            Me.udtSessionHandlerBLL.EHSTransactionSaveToSession(udtEHSTransaction, FunctionCode)

            Me.udcConfirmClaimCreation.ClearDocumentType()
            Me.udcConfirmClaimCreation.ClearEHSClaim()
            Me.udcConfirmClaimCreation.EnableVaccinationRecordChecking = False
            Me.udcConfirmClaimCreation.ShowHKICSymbol = True
            Me.udcConfirmClaimCreation.ShowOCSSSCheckingResult = False
            Me.udcConfirmClaimCreation.LoadTranInfo(udtEHSTransaction, New DataTable(), True, True, True)

            Me.udcInfoMessageBox.AddMessage(New SystemMessage("990000", SeverityCode.SEVI, MsgCode.MSG00021))
            Me.udcInfoMessageBox.BuildMessageBox()
            Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information

            Me.mvNewClaimTransaction.ActiveViewIndex = NewClaimTransaction.Confirm

            udtAuditLogEntry.AddDescripton(IIf(udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount, "eHS Validated Acc ID", "eHS Temp Acc ID"), _
                                           IIf(udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount, udtEHSAccount.VoucherAccID, udtEHSAccount.VoucherAccID))
            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00033, AuditLogDescription.NewClaimTransaction_EnterOverrideReason_Success)

        Else
            udtAuditLogEntry.AddDescripton(IIf(udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount, "eHS Validated Acc ID", "eHS Temp Acc ID"), _
                                           IIf(udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount, udtEHSAccount.VoucherAccID, udtEHSAccount.VoucherAccID))
            udcOverrideReasonMsgBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationFail, udtAuditLogEntry, LogID.LOG00034, AuditLogDescription.NewClaimTransaction_EnterOverrideReason_Fail)
        End If
    End Sub

#End Region

#Region "RVP Home List"
    Private Sub udcRVPHomeListSearch_RCHSelectedChanged(ByVal blnSelected As Boolean, ByVal sender As Object) Handles udcRVPHomeListSearch.RCHSelectedChanged
        If blnSelected Then
            Me.ibtnPopupRVPHomeListSearchSelect.Enabled = True
            Me.ibtnPopupRVPHomeListSearchSelect.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectBtn")
        Else
            Me.ibtnPopupRVPHomeListSearchSelect.Enabled = False
            Me.ibtnPopupRVPHomeListSearchSelect.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectDisableBtn")
        End If
    End Sub

    Protected Sub ibtnPopupRVPHomeListSearchCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Session.Remove(SESS_SearchRVPHomeList)
        udtSessionHandlerBLL.ClaimCOVID19RemoveFromSession()
        Me.ModalPopupExtenderRVPHomeListSearch.Hide()
    End Sub

    Protected Sub ibtnPopupRVPHomeListSearchSelect_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim strRCHCode As String = Me.udcRVPHomeListSearch.getSelectedCode()

        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtSelectedScheme As SchemeClaimModel = udtSessionHandlerBLL.SelectSchemeGetFromSession(FunctionCode)

        Select Case udtSelectedScheme.SchemeCode.Trim
            Case SchemeClaimModel.RVP
                CType(Me.udInputEHSClaim.GetRVPControl(), ucInputRVP).SetRCHCode(strRCHCode)

            Case SchemeClaimModel.VSS
                CType(Me.udInputEHSClaim.GetVSSControl(), ucInputVSS).SetPIDCode(strRCHCode)

            Case SchemeClaimModel.COVID19RVP
                CType(Me.udInputEHSClaim.GetCOVID19RVPControl(), ucInputCOVID19RVP).SetRCHCode(strRCHCode)

            Case Else

        End Select
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

        Session.Remove(SESS_SearchRVPHomeList)
        udtSessionHandlerBLL.ClaimCOVID19RemoveFromSession()
        Me.ModalPopupExtenderRVPHomeListSearch.Hide()
    End Sub
#End Region

#Region "School List"

    Private Sub udcSchoolListSearch_SchoolSelectedChanged(ByVal blnSelected As Boolean, ByVal sender As Object) Handles udcSchoolListSearch.SchoolSelectedChanged
        If blnSelected Then
            Me.ibtnPopupSchoolListSearchSelect.Enabled = True
            Me.ibtnPopupSchoolListSearchSelect.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectBtn")
        Else
            Me.ibtnPopupSchoolListSearchSelect.Enabled = False
            Me.ibtnPopupSchoolListSearchSelect.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectDisableBtn")
        End If
    End Sub

    Protected Sub ibtnPopupSchoolListSearchCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Session.Remove(SESS_SearchSchoolList)
        Me.ModalPopupExtenderSchoolListSearch.Hide()
    End Sub

    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Protected Sub ibtnPopupSchoolListSearchSelect_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim strSchoolCode As String = Me.udcSchoolListSearch.GetSelectedCode()
        Dim udtSelectedScheme As SchemeClaimModel = udtSessionHandlerBLL.SelectSchemeGetFromSession(FunctionCode)

        Select Case udtSelectedScheme.SchemeCode.Trim
            Case SchemeClaimModel.PPP
                CType(Me.udInputEHSClaim.GetPPPControl(), ucInputPPP).SetSchoolCode(strSchoolCode, udtSelectedScheme.SchemeCode)

            Case Else
                Throw New Exception(String.Format("Invalid Scheme({0}.)", udtSelectedScheme.SchemeCode))
        End Select

        Session.Remove(SESS_SearchSchoolList)
        Me.ModalPopupExtenderSchoolListSearch.Hide()
    End Sub
    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

#End Region

#Region "Outreach List"
    '---------------------------------------------------------------------------------------------------------
    'Search Outreach
    '---------------------------------------------------------------------------------------------------------
    Private Sub udcOutreachListSearch_OutreachSelectedChanged(ByVal blnSelected As Boolean, ByVal sender As Object) Handles udcOutreachSearch.SelectedChanged
        If blnSelected Then
            Me.btnPopupOutreachListSearchSelect.Enabled = True
            Me.btnPopupOutreachListSearchSelect.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectBtn")
        Else
            Me.btnPopupOutreachListSearchSelect.Enabled = False
            Me.btnPopupOutreachListSearchSelect.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectDisableBtn")
        End If
    End Sub

    Protected Sub ibtnPopupOutreachListSearchCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Session.Remove(SESS_SearchOutreachList)
        Me.ModalPopupExtenderOutreachListSearch.Hide()
    End Sub

    Protected Sub ibtnPopupOutreachListSearchSelect_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim strOutreachCode As String = Me.udcOutreachSearch.GetSelectedCode()
        Dim udtSelectedScheme As SchemeClaimModel = udtSessionHandlerBLL.SelectSchemeGetFromSession(FunctionCode)

        Select Case udtSelectedScheme.SchemeCode.Trim
            Case SchemeClaimModel.COVID19OR
                CType(Me.udInputEHSClaim.GetCOVID19ORControl(), ucInputCOVID19OR).SetOutreachCode(strOutreachCode)

            Case Else
                Throw New Exception(String.Format("Invalid Scheme({0}.)", udtSelectedScheme.SchemeCode))
        End Select

        Session.Remove(SESS_SearchOutreachList)
        Me.ModalPopupExtenderOutreachListSearch.Hide()
    End Sub

#End Region

#End Region

#Region "Enter Claim Detail Functions"
    Private Sub BuildClaimDetail(ByVal udtEHSAccount As EHSAccountModel, ByVal udtEHSTransaction As EHSTransactionModel)
        '--------------------------------
        ' Bulid Claim Detail
        '--------------------------------
        ' 1. EHS Account Info
        Me.BindPersonalInfo(udtEHSAccount)

        'Save Session
        udtEHSTransaction.EHSAcct = udtEHSAccount
        Me.udtSessionHandlerBLL.EHSTransactionWithoutTransactionDetailSaveToSession(udtEHSTransaction, FunctionCode)

        ' 2. Claim Info
        Me.BindClaimInfo(udtEHSTransaction)

        ' 3. Check to show the Vaccination Record button
        If VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N Or _
            VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CIMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N Then
            ibtnVaccinationRecord.Visible = True

            ' Force Retrieve eVaccination Record once
            Dim udtSession As New BLL.SessionHandlerBLL()
            Dim udtHAResult As HAVaccineResult = Nothing
            Dim udtDHResult As DHVaccineResult = Nothing
            Dim udtVaccineResultBag As New VaccineResultCollection

            If udtSession.CMSVaccineResultGetFromSession(FunctionCode) Is Nothing Or _
                udtSession.CIMSVaccineResultGetFromSession(FunctionCode) Is Nothing Then

                udtVaccineResultBag = GetVaccinationRecordFromSession(udtEHSAccount, "")

                udtSession.CMSVaccineResultSaveToSession(udtVaccineResultBag.HAVaccineResult, FunctionCode)
                udtSession.CIMSVaccineResultSaveToSession(udtVaccineResultBag.DHVaccineResult, FunctionCode)

            End If

            BuildSystemMessage(udtVaccineResultBag)

        Else
            ibtnVaccinationRecord.Visible = False
        End If

        ' 4. Claim Input Control
        LoadClaimControl()

    End Sub

    Private Sub BindClaimInfo(ByVal udtEHSTransaction As EHSTransactionModel)

        Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
        Dim udtEHSAccount As EHSAccount.EHSAccountModel = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)

        'SP
        Me.lblEnterClaimDetailSPID.Text = "(" + udtEHSTransaction.ServiceProviderID + ")"
        Me.lblEnterClaimDetailSPName.Text = udtEHSTransaction.ServiceProviderName
        Me.lblEnterClaimDetailSPStatus.Text = Me.lblEnterCreationDetailSPStatus.Text.Trim

        'Practice
        Me.lblEnterClaimDetailPractice.Text = Me.ddlEnterCreationDetailPractice.SelectedItem.Text.Trim
        Me.lblEnterClaimDetailPracticeStatus.Text = Me.lblEnterCreationDetailPracticeStatus.Text.Trim

        'Scheme with Non-Clinic setting
        Me.lblEnterClaimDetailScheme.Text = Me.ddlEnterCreationDetailScheme.SelectedItem.Text.Trim
        Me.panEnterClaimDetailNonClinicSetting.Visible = False
        Me.lblEnterClaimDetailNonClinicSetting.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting"))

        'Claim Creation
        Me.lblEnterClaimDetailPaymentMethod.Text = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("ReimbursementPaymentMethod", udtEHSTransaction.PaymentMethod).DataValue
        Me.lblEnterClaimDetailCreationReason.Text = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("ClaimCreationReason", udtEHSTransaction.CreationReason).DataValue
        If Not udtEHSTransaction.CreationRemarks.Trim.Equals(String.Empty) Then
            Me.lblEnterClaimDetailCreationReason.Text = Me.lblEnterClaimDetailCreationReason.Text & " (" & udtEHSTransaction.CreationRemarks.Trim & ")"
        End If

        If Not udtEHSTransaction.PaymentRemarks.Trim.Equals(String.Empty) Then
            Me.lblEnterClaimDetailPaymentMethod.Text = Me.lblEnterClaimDetailPaymentMethod.Text & " (" & udtEHSTransaction.PaymentRemarks.Trim & ")"
        End If

        'HKIC Symbol
        Me.panHKICSymbol.Visible = False
        ClearHKICSymbolButtonList()

        'Service Date
        Me.txtEnterClaimDetailServiceDate.Text = udtFormatter.formatInputTextDate(Me.udtGeneralFunction.GetSystemDateTime())

        'Clear input control all alert images
        Me.ClearClaimControlErrorImage()

        'Set "Save" button disabled
        Me.SetSaveButtonEnable(Me.ibtnEnterClaimDetailSave, False)

    End Sub

    Private Sub LoadClaimControl()
        Dim udtSchemeClaimList As SchemeClaimModelCollection = Nothing
        Dim udtSchemeClaim As SchemeClaimModel = Nothing
        Dim blnWithoutConversionRate As Boolean = False

        'Dim udteHSAccountMaintBLL As New eHSAccountMaintBLL
        'Dim dtmServiceDate As Date = udtFormatter.convertDate(udtFormatter.formatInputDate(Me.txtEnterClaimDetailServiceDate.Text.Trim), Common.Component.CultureLanguage.English)
        Dim udtEHSTransaction As EHSTransactionModel = Nothing
        Dim udtEHSAccount As EHSAccount.EHSAccountModel = Nothing

        udtEHSTransaction = Me.udtSessionHandlerBLL.EHSTransactionWithoutTransactionDetailGetFromSession(FunctionCode)
        udtEHSAccount = udtEHSTransaction.EHSAcct

        udtSchemeClaimList = Me.udtSessionHandlerBLL.SchemeListGetFromSession(FunctionCode)
        udtSchemeClaim = udtSchemeClaimList.Filter(udtEHSTransaction.SchemeCode)

        Me.udtSessionHandlerBLL.SelectSchemeRemoveFromSession(FunctionCode)
        Me.udtSessionHandlerBLL.SelectSchemeSaveToSession(udtSchemeClaim, FunctionCode)
        'Me.udtSessionHandlerBLL.ChangeSchemeInPracticeSaveToSession(True, FunctionCode)
        Me.udInputEHSClaim.ResetSchemeType()

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        'Dim udtEHSTransaction As EHSTransactionModel = Me.udtSessionHandlerBLL.EHSTransactionGetFromSession(FunctionCode)
        'Dim udtEHSTransaction As EHSTransactionModel = Me.udtSessionHandlerBLL.EHSTransactionWithoutTransactionDetailGetFromSession(FunctionCode)
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        lblEnterClaimDetailSchemeStatus.Visible = False
        lblEnterClaimDetailSchemeStatus.Text = String.Empty

        'Dim ddlScheme As DropDownList = CType(sender, DropDownList)

        'If Not ddlScheme Is Nothing Then
        '    If ddlScheme.SelectedIndex <> 0 Then
        Dim strSchemeCodeEnrol As String = New SchemeClaimBLL().ConvertSchemeEnrolFromSchemeClaimCode(udtEHSTransaction.SchemeCode)

        Dim udtPracticeSchemeInfoBLL As New PracticeSchemeInfo.PracticeSchemeInfoBLL
        Dim udtPracticeSchemeInfoModelCollection As PracticeSchemeInfo.PracticeSchemeInfoModelCollection = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListPermanentBySPIDPracticeDisplaySeq(udtEHSTransaction.ServiceProviderID, udtEHSTransaction.PracticeID, New Common.DataAccess.Database)
        Dim udtResPracticeSchemeInfoModelCollection As PracticeSchemeInfo.PracticeSchemeInfoModelCollection = udtPracticeSchemeInfoModelCollection.FilterByPracticeScheme(udtEHSTransaction.PracticeID, strSchemeCodeEnrol)

        If Not udtResPracticeSchemeInfoModelCollection Is Nothing Then
            ' 1. Remark "Delist" behind the scheme, if need
            For Each udtPracticeSchemeInfoModel As PracticeSchemeInfo.PracticeSchemeInfoModel In udtResPracticeSchemeInfoModelCollection.Values
                'If one of practice scheme is delisted, the label "delisted" will be shown.
                If udtPracticeSchemeInfoModel.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.DelistedInvoluntary Or _
                    udtPracticeSchemeInfoModel.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.DelistedVoluntary Then

                    Status.GetDescriptionFromDBCode(PracticeStatus.ClassCode, PracticeSchemeInfoStatus.Delisted, Me.lblEnterClaimDetailSchemeStatus.Text, String.Empty)
                    lblEnterClaimDetailSchemeStatus.Text = "(" + lblEnterClaimDetailSchemeStatus.Text + ")"
                    lblEnterClaimDetailSchemeStatus.Visible = True

                End If
            Next

            ' 2. Remark "Non-clinic" under the scheme, if need
            If udtResPracticeSchemeInfoModelCollection.IsNonClinic Then
                Me.panEnterClaimDetailNonClinicSetting.Visible = True
                udtSessionHandlerBLL.NonClinicSettingSaveToSession(True, FunctionCode)
            Else
                Me.panEnterClaimDetailNonClinicSetting.Visible = False
                udtSessionHandlerBLL.NonClinicSettingSaveToSession(False, FunctionCode)
            End If

        End If

        ' 3. Show HKIC symbol input, if need
        If udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode.Trim).DocCode = DocTypeModel.DocTypeCode.HKIC Then
            EnableHKICSymbolRadioButtonList(True, strSchemeCodeEnrol, udtFormatter.convertDate(Me.txtEnterClaimDetailServiceDate.Text.Trim, Common.Component.CultureLanguage.English))
        End If

        '    End If
        'End If

        'udtEHSTransaction.TransactionDetails = Nothing
        'udtEHSTransaction.TransactionAdditionFields = Nothing

        ' 4. Show exchange rate, if need
        If New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(udtEHSTransaction.SchemeCode) = SchemeClaimModel.EnumControlType.VOUCHER OrElse _
            New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(udtEHSTransaction.SchemeCode) = SchemeClaimModel.EnumControlType.VOUCHERCHINA Then

            'check exchange rate
            If CheckExchangeRateAbsence(udtFormatter.convertDate(Me.txtEnterClaimDetailServiceDate.Text.Trim, Common.Component.CultureLanguage.English)) = True Then
                blnWithoutConversionRate = True
            End If

        End If

        If blnWithoutConversionRate = False Then
            'Me.udtSessionHandlerBLL.EHSTransactionSaveToSession(udtEHSTransaction, FunctionCode)

            Me.udtSessionHandlerBLL.EHSClaimVaccineRemoveFromSession(FunctionCode)

            'If Me.ddlEnterClaimDetailsSchemeText.SelectedIndex = 0 Then
            '    Me.SetSaveButtonEnable(Me.ibtnEnterClaimDetailSave, False)
            '    Me.udcMessageBox.Visible = False
            '    Me.panNonClinicSetting.Visible = False
            '    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            '    ' ----------------------------------------------------------
            '    Me.panHKICSymbol.Visible = False
            '    ' CRE17-010 (OCSSS integration) [End][Chris YIM]
            'End If

            'Me.udInputEHSClaim.Clear()
            Me.SetUpEnterClaimDetails(False)

            'Me.udtSessionHandlerBLL.ClaimCategoryRemoveFromSession(FunctionCode)

        Else
            Me.SetSaveButtonEnable(Me.ibtnEnterClaimDetailSave, False)
            Me.udcMessageBox.Visible = True

        End If

        ' CRE20-0023 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Select Case udtSchemeClaim.SchemeCode.Trim.ToUpper
            Case SchemeClaimModel.VSS
                Dim udcInputVSS As ucInputVSS = Me.udInputEHSClaim.GetVSSControl
                If udcInputVSS IsNot Nothing Then
                    udcInputVSS.InitialCOVID19ClaimDetail()
                End If
            Case Else
                'Nothing to do
        End Select
        ' CRE20-0023 (Immu record) [End][Chris YIM]

    End Sub

    Private Sub SetUpEnterClaimDetails(Optional ByVal blnPostbackRebuild As Boolean = True)
        Dim strServiceDate As String = Me.udtFormatter.formatInputDate(Me.txtEnterClaimDetailServiceDate.Text.Trim)
        Dim dtmServiceDate As Date

        Dim strValidatedServiceDate As String = "ValidatedServiceDate"
        strServiceDate = udtFormatter.convertDate(strServiceDate, Common.Component.CultureLanguage.English)

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtInputPicker As InputPickerModel = Nothing
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL
        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

        Dim udtEHSTransaction As EHSTransactionModel
        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        udtEHSTransaction = Me.udtSessionHandlerBLL.EHSTransactionWithoutTransactionDetailGetFromSession(FunctionCode)
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        ' Not necessary to clear inputted value when postback rebuild
        If Not blnPostbackRebuild Then Me.udInputEHSClaim.Clear()
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Koala]

        If Not udtEHSTransaction Is Nothing AndAlso DateTime.TryParse(strServiceDate, dtmServiceDate) Then

            Dim strSchemeCode As String = udtEHSTransaction.SchemeCode
            Dim blnNoCategory As Boolean = True
            Dim blnNotAvailableForClaim As Boolean = True
            Dim isEligibleForClaim As Boolean = True

            Dim udtClaimCategory As ClaimCategory.ClaimCategoryModel = Me.udtSessionHandlerBLL.ClaimCategoryGetFromSession(FunctionCode)

            'If Not DateTime.TryParse(strServiceDate, dtmServiceDate) OrElse Not IsValidServiceDate(Me.txtEnterClaimDetailServiceDate.Text, strSchemeCode) Then
            'If DateTime.TryParse(strServiceDate, dtmServiceDate) Then 'OrElse Not IsValidServiceDate(Me.txtEnterClaimDetailServiceDate.Text, strSchemeCode) Then
            dtmServiceDate = strServiceDate 'udtFormatter.convertDate(Me.txtEnterClaimDetailServiceDate.Text.Trim, Common.Component.CultureLanguage.English)
            'End If

            Dim dtmCurrentDate As Date = Me.udtGeneralFunction.GetSystemDateTime

            ' Scheme Information
            Dim udtSchemeClaimList As SchemeClaimModelCollection
            Dim udtSchemeClaim As SchemeClaimModel

            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
            ' Refresh Available Voucher Amount when Service Date changed
            'udtSchemeClaimList = Me.udtSessionHandlerBLL.SchemeListGetFromSession(FunctionCode)
            udtSchemeClaimList = Me.udtSchemeClaimBLL.getSchemeClaimFromBackOfficeUserAndPractice(udtHCVUUser.UserID, FunctionCode, udtEHSTransaction.ServiceProviderID, udtEHSTransaction.PracticeID, dtmServiceDate)
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]
            udtSchemeClaim = udtSchemeClaimList.Filter(strSchemeCode)

            Me.udtSessionHandlerBLL.SelectSchemeSaveToSession(udtSchemeClaim, FunctionCode)

            'EHS Account
            Dim udtEHSAccount As EHSAccount.EHSAccountModel
            Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
            udtEHSAccount = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)

            Dim udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel
            udtEHSPersonalInfo = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)

            'RVP Home List
            If Not IsNothing(Session(SESS_SearchRVPHomeList)) AndAlso CBool(Session(SESS_SearchRVPHomeList)) Then
                Me.ModalPopupExtenderRVPHomeListSearch.Show()
            Else
                Me.ModalPopupExtenderRVPHomeListSearch.Hide()
            End If

            'School List
            If Not IsNothing(Session(SESS_SearchSchoolList)) AndAlso CBool(Session(SESS_SearchSchoolList)) Then
                Me.ModalPopupExtenderSchoolListSearch.Show()
            Else
                Me.ModalPopupExtenderSchoolListSearch.Hide()
            End If

            'Outreach List
            If Not IsNothing(Session(SESS_SearchOutreachList)) AndAlso CBool(Session(SESS_SearchOutreachList)) Then
                Me.ModalPopupExtenderOutreachListSearch.Show()
            Else
                Me.ModalPopupExtenderOutreachListSearch.Hide()
            End If

            If Not strSchemeCode.Equals(String.Empty) AndAlso _
                Not udtSchemeClaim Is Nothing AndAlso _
                udtSchemeClaim.SubsidizeGroupClaimList.Filter(dtmServiceDate).Count > 0 Then

                Select Case New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(strSchemeCode)
                    Case SchemeClaimModel.EnumControlType.VOUCHER, SchemeClaimModel.EnumControlType.VOUCHERCHINA
                        'Get Voucher Info.

                        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                        Dim udtSelectedPractice As Common.Component.Practice.PracticeBLL.PracticeDisplayModel = Me.udtSessionHandlerBLL.PracticeDisplayGetFromSession(FunctionCode)

                        Dim udtVoucherInfo As New VoucherInfoModel(VoucherInfoModel.AvailableVoucher.Include, _
                                                                   VoucherInfoModel.AvailableQuota.Include)

                        udtVoucherInfo.GetInfo(dtmServiceDate, udtSchemeClaim, udtEHSPersonalInfo, udtSelectedPractice.ServiceCategoryCode)

                        udtEHSAccount.VoucherInfo = udtVoucherInfo

                        udtEHSAccountMaintBLL.EHSAccountSaveToSession(udtEHSAccount, FunctionCode)

                        If udtEHSAccount.VoucherInfo.GetAvailableVoucher() > 0 Then
                            blnNotAvailableForClaim = False
                        End If
                        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

                    Case SchemeClaimModel.EnumControlType.CIVSS, SchemeClaimModel.EnumControlType.EVSS, SchemeClaimModel.EnumControlType.HSIVSS, _
                         SchemeClaimModel.EnumControlType.RVP, SchemeClaimModel.EnumControlType.PIDVSS, SchemeClaimModel.EnumControlType.VSS, _
                         SchemeClaimModel.EnumControlType.ENHVSSO, SchemeClaimModel.EnumControlType.PPP, SchemeClaimModel.EnumControlType.COVID19, _
                         SchemeClaimModel.EnumControlType.COVID19RVP, SchemeClaimModel.EnumControlType.COVID19OR


                        Dim udtEHSClaimVaccine As EHSClaimVaccine.EHSClaimVaccineModel = Nothing
                        Dim blnNeedCreateVaccine As Boolean = False

                        udtEHSClaimVaccine = Me.udtSessionHandlerBLL.EHSClaimVaccineGetFromSession(FunctionCode)

                        If IsNothing(udtEHSClaimVaccine) OrElse Not udtEHSClaimVaccine.SchemeCode.Equals(strSchemeCode) Then
                            blnNeedCreateVaccine = True
                        End If

                        'Search available subsidy of Vaccine with different scheme 
                        Select Case udtSchemeClaim.ControlType
                            Case SchemeClaimModel.EnumControlType.HSIVSS, _
                                 SchemeClaimModel.EnumControlType.RVP, _
                                 SchemeClaimModel.EnumControlType.VSS, _
                                 SchemeClaimModel.EnumControlType.ENHVSSO, _
                                 SchemeClaimModel.EnumControlType.PPP, _
                                 SchemeClaimModel.EnumControlType.COVID19, _
                                 SchemeClaimModel.EnumControlType.COVID19RVP, _
                                 SchemeClaimModel.EnumControlType.COVID19OR

                                '--------------------
                                ' With Category
                                '--------------------
                                Dim udtClaimCategorys As ClaimCategory.ClaimCategoryModelCollection
                                Dim udtPersonalInformation As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode)
                                Dim strEnableClaimCategory As String = String.Empty

                                '--------------------------------------
                                'Part 1: Retrieve Claim Category
                                '--------------------------------------
                                'Retrieve Claim Category
                                Dim udtClaimCategoryBLL As ClaimCategory.ClaimCategoryBLL = New ClaimCategory.ClaimCategoryBLL()
                                udtClaimCategorys = udtClaimCategoryBLL.getDistinctCategoryBySchemeOnly(udtSchemeClaim)

                                'Assign Claim Category List to control
                                Me.udInputEHSClaim.ClaimCategorys = udtClaimCategorys

                                '--------------------------------------
                                'Part 2.1: Search Vaccine (RVP,HSIVSS)
                                '--------------------------------------
                                If udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.RVP) Then
                                    udtGeneralFunction.getSytemParameterByParameterNameSchemeCode("RVPEnableClaimCategory", strEnableClaimCategory, String.Empty, strSchemeCode)
                                End If


                                If strEnableClaimCategory = "Y" OrElse udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.HSIVSS) Then

                                    If Not udtClaimCategory Is Nothing AndAlso udtClaimCategory.SchemeCode = udtSchemeClaim.SchemeCode.Trim() Then
                                        'Category has been selected
                                        If blnNeedCreateVaccine Then
                                            udtInputPicker = New InputPickerModel
                                            udtInputPicker.CategoryCode = udtClaimCategory.CategoryCode
                                            udtEHSClaimVaccine = Me.udtEHSClaimBLL.SearchEHSClaimVaccine(udtSchemeClaim, udtEHSAccount.SearchDocCode, udtEHSAccount, dtmServiceDate, True, udtInputPicker)
                                        End If

                                    Else
                                        'Category not selected or categrory is not for this scheme
                                        If udtClaimCategorys.Count = 1 Then
                                            udtClaimCategory = udtClaimCategorys(0)
                                            Me.udtSessionHandlerBLL.ClaimCategorySaveToSession(udtClaimCategory, FunctionCode)

                                            If blnNeedCreateVaccine Then
                                                udtInputPicker = New InputPickerModel
                                                udtInputPicker.CategoryCode = udtClaimCategory.CategoryCode
                                                udtEHSClaimVaccine = Me.udtEHSClaimBLL.SearchEHSClaimVaccine(udtSchemeClaim, udtEHSAccount.SearchDocCode, udtEHSAccount, dtmServiceDate, True, udtInputPicker)
                                            End If

                                        Else
                                            'Scheme Change 
                                            '1) Remove category
                                            '2) no vaccine
                                            Me.udtSessionHandlerBLL.ClaimCategoryRemoveFromSession(FunctionCode)
                                            Me.udtSessionHandlerBLL.EHSClaimVaccineRemoveFromSession(FunctionCode)
                                            udtEHSClaimVaccine = Nothing
                                        End If

                                    End If

                                    '----------------------------------------------------------------------
                                    'Check Claim Category list
                                    '----------------------------------------------------------------------
                                    If Not udtClaimCategorys Is Nothing AndAlso udtClaimCategorys.Count > 0 Then
                                        blnNoCategory = False
                                    Else
                                        isEligibleForClaim = False
                                        Me.udtSessionHandlerBLL.ClaimCategoryRemoveFromSession(FunctionCode)
                                    End If

                                ElseIf strEnableClaimCategory = "N" Then
                                    'For RVP ONLY
                                    udtClaimCategory = udtClaimCategorys.FilterByCategoryCode(udtSchemeClaim.SchemeCode, "RESIDENT")

                                    Me.udtSessionHandlerBLL.ClaimCategorySaveToSession(udtClaimCategory, FunctionCode)

                                    If blnNeedCreateVaccine Then
                                        udtInputPicker = New InputPickerModel
                                        udtInputPicker.CategoryCode = udtClaimCategory.CategoryCode
                                        udtEHSClaimVaccine = Me.udtEHSClaimBLL.SearchEHSClaimVaccine(udtSchemeClaim, udtEHSAccount.SearchDocCode, udtEHSAccount, dtmServiceDate, True, udtInputPicker)
                                    End If

                                    blnNoCategory = False
                                End If

                                '--------------------------------------
                                'Part 2.2: Search Vaccine (VSS, ENHVSSO, PPP)
                                '--------------------------------------
                                If udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.VSS) OrElse _
                                   udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.ENHVSSO) OrElse _
                                   udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.PPP) OrElse _
                                   udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.COVID19) OrElse _
                                   udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.COVID19RVP) OrElse _
                                   udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.COVID19OR) _
                                   Then

                                    If Not udtClaimCategory Is Nothing AndAlso udtClaimCategory.SchemeCode = udtSchemeClaim.SchemeCode.Trim() Then
                                        'Category has been selected
                                        If blnNeedCreateVaccine Then
                                            udtInputPicker = New InputPickerModel
                                            udtInputPicker.CategoryCode = udtClaimCategory.CategoryCode
                                            udtEHSClaimVaccine = Me.udtEHSClaimBLL.SearchEHSClaimVaccine(udtSchemeClaim, udtEHSAccount.SearchDocCode, udtEHSAccount, dtmServiceDate, True, udtInputPicker)
                                        End If

                                    Else
                                        'Category not selected or categrory is not for this scheme
                                        If udtClaimCategorys.Count = 1 Then
                                            udtClaimCategory = udtClaimCategorys(0)
                                            Me.udtSessionHandlerBLL.ClaimCategorySaveToSession(udtClaimCategory, FunctionCode)

                                            If blnNeedCreateVaccine Then
                                                udtInputPicker = New InputPickerModel
                                                udtInputPicker.CategoryCode = udtClaimCategory.CategoryCode
                                                udtEHSClaimVaccine = Me.udtEHSClaimBLL.SearchEHSClaimVaccine(udtSchemeClaim, udtEHSAccount.SearchDocCode, udtEHSAccount, dtmServiceDate, True, udtInputPicker)
                                            End If

                                        Else
                                            'Scheme Change 
                                            '1) Remove category
                                            '2) no vaccine
                                            Me.udtSessionHandlerBLL.ClaimCategoryRemoveFromSession(FunctionCode)
                                            Me.udtSessionHandlerBLL.EHSClaimVaccineRemoveFromSession(FunctionCode)
                                            udtEHSClaimVaccine = Nothing
                                        End If

                                    End If

                                    '----------------------------------------------------------------------
                                    'Check Claim Category list
                                    '----------------------------------------------------------------------
                                    If Not udtClaimCategorys Is Nothing AndAlso udtClaimCategorys.Count > 0 Then
                                        blnNoCategory = False
                                    Else
                                        ' Me.udcMsgBoxInfo.AddMessage(New SystemMessage("990000", "E", "00106"))
                                        isEligibleForClaim = False
                                        Me.udtSessionHandlerBLL.ClaimCategoryRemoveFromSession(FunctionCode)
                                    End If

                                End If

                                '------------------------------------------------------------
                                ' Part 3: Determine whether it is available for claim
                                '------------------------------------------------------------
                                If Not udtEHSClaimVaccine Is Nothing AndAlso Not blnNoCategory AndAlso Not udtClaimCategory Is Nothing Then
                                    If Not udtEHSClaimVaccine.SubsidizeList Is Nothing Then
                                        'Check if no vaccine is avaliable for the recipient -> change "noAvailableForClaim" to false
                                        For Each udtEHSClaimSubsidize As EHSClaimVaccine.EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
                                            If udtEHSClaimSubsidize.Available Then
                                                blnNotAvailableForClaim = False
                                                Exit For
                                            End If
                                        Next
                                    Else
                                        udtEHSClaimVaccine = Nothing
                                        blnNotAvailableForClaim = True
                                    End If

                                ElseIf Not blnNoCategory Then
                                    blnNotAvailableForClaim = False

                                Else
                                    blnNotAvailableForClaim = True

                                End If

                            Case Else
                                '--------------------
                                ' Without Category
                                '--------------------
                                'For EVSS and CIVSS

                                blnNoCategory = False
                                'Default
                                If blnNeedCreateVaccine Then
                                    udtEHSClaimVaccine = Me.udtEHSClaimBLL.SearchEHSClaimVaccine(udtSchemeClaim, udtEHSAccount.SearchDocCode, udtEHSAccount, dtmServiceDate, True, Nothing)
                                End If

                                '----------------------------------------------------------------------
                                'Check Vaccine is available for Claim
                                '----------------------------------------------------------------------
                                If Not udtEHSClaimVaccine Is Nothing Then
                                    If Not udtEHSClaimVaccine.SubsidizeList Is Nothing Then
                                        'Check if no vaccine is avaliable for the recipient -> change "noAvailableForClaim" to false
                                        For Each udtEHSClaimSubsidize As EHSClaimVaccine.EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
                                            If udtEHSClaimSubsidize.Available Then
                                                blnNotAvailableForClaim = False
                                                Exit For
                                            End If
                                        Next
                                    Else
                                        udtEHSClaimVaccine = Nothing
                                        ' No available subsidize for Claim
                                        ' Case 1: Not Eligiblity
                                        ' Case 2: Out of period
                                        ' Case 3: The subsidizes is used
                                        For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtSchemeClaim.SubsidizeGroupClaimList
                                            If udtSubsidizeGroupClaim.LastServiceDtm >= dtmServiceDate Then
                                                isEligibleForClaim = False
                                            End If
                                        Next
                                    End If
                                Else
                                    ' No available subsidize for Claim
                                    ' Case 1: Not Eligiblity
                                    ' Case 2: Out of period
                                    ' Case 3: The subsidizes is used
                                    For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtSchemeClaim.SubsidizeGroupClaimList
                                        If udtSubsidizeGroupClaim.LastServiceDtm >= dtmServiceDate Then
                                            isEligibleForClaim = False
                                        End If
                                    Next
                                End If
                        End Select

                        Me.udtSessionHandlerBLL.EHSClaimVaccineSaveToSession(udtEHSClaimVaccine, FunctionCode)

                        Me.udInputEHSClaim.EHSClaimVaccine = udtEHSClaimVaccine

                        AddHandler Me.udInputEHSClaim.VaccineLegendClicked, AddressOf udcInputEHSClaim_VaccineLegendClick

                    Case SchemeClaimModel.EnumControlType.EHAPP
                        blnNotAvailableForClaim = False

                        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                    Case SchemeClaimModel.EnumControlType.SSSCMC
                        Dim blnValid As Boolean = True
                        Dim udtPersonalInformation As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)

                        ' Check Patient whether is on list
                        If (New ClaimRulesBLL).CheckIsHAPatient(udtSchemeClaim.SchemeCode, udtEHSAccount.SearchDocCode, udtPersonalInformation.IdentityNum) <> String.Empty Then
                            blnValid = False
                        End If

                        ' INT20-00XX (Allow back-office claim in scheme SSSCMC when zero balance) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                        '' Check Patient whether has available subsidy
                        'If Not udtEHSTransactionBLL.getAvailableSubsidizeItem_SSSCMC(udtPersonalInformation, udtSchemeClaim.SubsidizeGroupClaimList) > 0 Then
                        '    blnValid = False
                        'End If
                        ' INT20-00XX (Allow back-office claim in scheme SSSCMC when zero balance) [End][Chris YIM]

                        If blnValid Then
                            blnNotAvailableForClaim = False

                            'Sub-Patient Type
                            Dim dtHAPatient As DataTable = (New HAServicePatientBLL).getHAServicePatientByIdentityNum(udtPersonalInformation.DocCode, udtPersonalInformation.IdentityNum)

                            If dtHAPatient.Rows.Count = 0 Then
                                Throw New Exception(String.Format("Document No.({0}) of Document type({1}) is not found in DB table HAServicePatient.", _
                                                                  udtPersonalInformation.IdentityNum, _
                                                                  udtPersonalInformation.DocCode))
                            End If

                            Me.udtSessionHandlerBLL.HAPatientSaveToSession(dtHAPatient)

                        End If

                        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

                End Select

                Me.udcMessageBox.Clear()
                Me.SetSaveButtonEnable(Me.ibtnEnterClaimDetailSave, True)

                'Bulid Vaccine Input Control
                Me.udInputEHSClaim.AvaliableForClaim = True
                Me.udInputEHSClaim.CurrentPractice = Me.udtSessionHandlerBLL.PracticeDisplayGetFromSession(FunctionCode)
                Me.udInputEHSClaim.SchemeType = udtSchemeClaim.SchemeCode.Trim()
                Me.udInputEHSClaim.EHSAccount = udtEHSAccount
                Me.udInputEHSClaim.EHSTransaction = Me.udtSessionHandlerBLL.EHSTransactionGetFromSession(FunctionCode)
                Me.udInputEHSClaim.TableTitleWidth = 200
                Me.udInputEHSClaim.ServiceDate = dtmServiceDate
                Me.udInputEHSClaim.FunctionCode = FunctionCode
                Me.udInputEHSClaim.ShowLegend = False
                Me.udInputEHSClaim.NonClinic = Me.udtSessionHandlerBLL.NonClinicSettingGetFromSession(FunctionCode)

                Me.udInputEHSClaim.Built(blnPostbackRebuild)

                Select Case udtSchemeClaim.ControlType
                    Case SchemeClaimModel.EnumControlType.HSIVSS, SchemeClaimModel.EnumControlType.RVP, SchemeClaimModel.EnumControlType.VSS, _
                         SchemeClaimModel.EnumControlType.ENHVSSO, SchemeClaimModel.EnumControlType.PPP, SchemeClaimModel.EnumControlType.COVID19,
                         SchemeClaimModel.EnumControlType.COVID19RVP, SchemeClaimModel.EnumControlType.COVID19OR

                        If udtClaimCategory Is Nothing OrElse blnNotAvailableForClaim OrElse blnNoCategory Then
                            Me.SetSaveButtonEnable(Me.ibtnEnterClaimDetailSave, False)
                        Else
                            Me.SetSaveButtonEnable(Me.ibtnEnterClaimDetailSave, True)
                        End If

                    Case SchemeClaimModel.EnumControlType.SSSCMC
                        If blnNotAvailableForClaim Then
                            Me.SetSaveButtonEnable(Me.ibtnEnterClaimDetailSave, False)
                        Else
                            Me.SetSaveButtonEnable(Me.ibtnEnterClaimDetailSave, True)
                        End If

                End Select

            End If
        End If
    End Sub

    Private Function CheckExchangeRateAbsence(ByVal pdtmServiceDate As Date) As Boolean
        Dim udtEHSTransaction As EHSTransactionModel
        udtEHSTransaction = Me.udtSessionHandlerBLL.EHSTransactionWithoutTransactionDetailGetFromSession(FunctionCode)

        If New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(udtEHSTransaction.SchemeCode) = SchemeClaimModel.EnumControlType.VOUCHERCHINA Then
            'check exchange rate
            Dim decExchangeRate As Decimal = 0
            Dim udtExchangeRateBLL As ExchangeRate.ExchangeRateBLL = New ExchangeRate.ExchangeRateBLL()

            decExchangeRate = udtExchangeRateBLL.GetExchangeRateValue(pdtmServiceDate)
            If decExchangeRate <= 0 Then

                Me.udtSM = New Common.ComObject.SystemMessage(FunctCode.FUNT010404, SeverityCode.SEVE, MsgCode.MSG00018)
                Me.udcMessageBox.AddMessage(Me.udtSM)
                Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
                udtAuditLogEntry.AddDescripton("Error", "No conversion rate can be found")
                udcMessageBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationFail, udtAuditLogEntry, LogID.LOG00029, AuditLogDescription.NewClaimTransaction_EnterClaimDetail_Fail)

                Me.SetSaveButtonEnable(Me.ibtnEnterClaimDetailSave, False)
                Me.udcMessageBox.Visible = True

                Return True
            End If
        End If
    End Function

    Private Sub SetSaveButtonEnable(ByVal btnSave As ImageButton, ByVal blnEnable As Boolean)
        btnSave.Enabled = blnEnable
        If blnEnable Then
            btnSave.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "SaveBtn")
        Else
            btnSave.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "SaveDisableBtn")
        End If
        btnSave.AlternateText = Me.GetGlobalResourceObject("AlternateText", "SaveBtn")
    End Sub

    Private Sub ClearClaimControlErrorImage()
        Dim udtSchemeClaim As SchemeClaimModel
        udtSchemeClaim = Me.udtSessionHandlerBLL.SelectSchemeGetFromSession(FunctionCode)

        If Not udtSchemeClaim Is Nothing Then
            Select Case udtSchemeClaim.ControlType
                Case SchemeClaimModel.EnumControlType.CIVSS
                    Dim udcInputCIVSS As ucInputCIVSS = Me.udInputEHSClaim.GetCIVSSControl()
                    If Not udcInputCIVSS Is Nothing Then
                        udcInputCIVSS.SetDoseErrorImage(False)
                    End If

                Case SchemeClaimModel.EnumControlType.EVSS
                    Dim udcInputEVSS As ucInputEVSS = Me.udInputEHSClaim.GetEVSSControl()
                    If Not udcInputEVSS Is Nothing Then
                        udcInputEVSS.SetDoseErrorImage(False)
                    End If
                Case SchemeClaimModel.EnumControlType.VOUCHER
                    Dim udcInputHCVS As ucInputHCVS = Me.udInputEHSClaim.GetHCVSControl
                    If Not udcInputHCVS Is Nothing Then
                        udcInputHCVS.SetReasonForVisitError(False)
                        udcInputHCVS.SetVoucherredeemError(False)
                        udcInputHCVS.SetCoPaymentFeeError(False)
                    End If

                Case SchemeClaimModel.EnumControlType.VOUCHERCHINA
                    Dim udcInputHCVSChina As ucInputHCVSChina = Me.udInputEHSClaim.GetHCVSChinaControl
                    If Not udcInputHCVSChina Is Nothing Then
                        udcInputHCVSChina.SetVoucherredeemError(False)
                    End If

                Case SchemeClaimModel.EnumControlType.HSIVSS
                    Dim udcInputHSIVSS As ucInputHSIVSS = Me.udInputEHSClaim.GetHSIVSSControl()
                    If Not udcInputHSIVSS Is Nothing Then
                        udcInputHSIVSS.SetPreConditionError(False)
                        udcInputHSIVSS.SetDoseErrorImage(False)
                    End If

                Case SchemeClaimModel.EnumControlType.RVP
                    Dim udcInputRVP As ucInputRVP = Me.udInputEHSClaim.GetRVPControl()
                    If Not udcInputRVP Is Nothing Then
                        udcInputRVP.SetRCHCodeError(False)
                        udcInputRVP.SetCategoryError(False)
                        udcInputRVP.SetDoseErrorImage(False)
                        udcInputRVP.SetCOVID19DetailError(False)
                    End If

                Case SchemeClaimModel.EnumControlType.EHAPP
                    Dim udcInputEHAPP As ucInputEHAPP = Me.udInputEHSClaim.GetEHAPPControl()
                    If Not udcInputEHAPP Is Nothing Then
                        udcInputEHAPP.SetAllAlertVisible(False)
                    End If

                Case SchemeClaimModel.EnumControlType.PIDVSS
                    Dim udcInputPIDVSS As ucInputPIDVSS = Me.udInputEHSClaim.GetPIDVSSControl()
                    If Not udcInputPIDVSS Is Nothing Then
                        udcInputPIDVSS.SetDoseErrorImage(False)
                    End If

                Case SchemeClaimModel.EnumControlType.VSS
                    Dim udcInputVSS As ucInputVSS = Me.udInputEHSClaim.GetVSSControl()
                    If Not udcInputVSS Is Nothing Then
                        udcInputVSS.SetCategoryError(False)
                        udcInputVSS.SetDoseErrorImage(False)
                        udcInputVSS.SetCOVID19DetailError(False)
                    End If

                Case SchemeClaimModel.EnumControlType.ENHVSSO
                    Dim udcInputENHVSSO As ucInputENHVSSO = Me.udInputEHSClaim.GetENHVSSOControl()
                    If Not udcInputENHVSSO Is Nothing Then
                        udcInputENHVSSO.SetCategoryError(False)
                        udcInputENHVSSO.SetDoseErrorImage(False)
                    End If

                Case SchemeClaimModel.EnumControlType.PPP
                    Dim udcInputPPP As ucInputPPP = Me.udInputEHSClaim.GetPPPControl()
                    If Not udcInputPPP Is Nothing Then
                        udcInputPPP.SetCategoryError(False)
                        udcInputPPP.SetSchoolCodeError(False)
                        udcInputPPP.SetDoseErrorImage(False)
                    End If

                    ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                Case SchemeClaimModel.EnumControlType.SSSCMC
                    Dim udcInputSSSCMC As ucInputSSSCMC = Me.udInputEHSClaim.GetSSSCMCControl()
                    If Not udcInputSSSCMC Is Nothing Then
                        udcInputSSSCMC.SetError(False)
                    End If
                    ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

                    ' CRE20-0023 (Immu record) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                Case SchemeClaimModel.EnumControlType.COVID19
                    Dim udcInputCOVID19 As ucInputCOVID19 = Me.udInputEHSClaim.GetCOVID19Control()
                    If Not udcInputCOVID19 Is Nothing Then
                        udcInputCOVID19.SetCategoryError(False)
                        udcInputCOVID19.SetDoseErrorImage(False)
                        udcInputCOVID19.SetVaccineBrandError(False)
                        udcInputCOVID19.SetVaccineLotNoError(False)
                    End If

                Case SchemeClaimModel.EnumControlType.COVID19RVP
                    Dim udcInputCOVID19RVP As ucInputCOVID19RVP = Me.udInputEHSClaim.GetCOVID19RVPControl()
                    If Not udcInputCOVID19RVP Is Nothing Then
                        udcInputCOVID19RVP.SetCategoryError(False)
                        udcInputCOVID19RVP.SetDoseErrorImage(False)
                        udcInputCOVID19RVP.SetDetailError(False)
                    End If

                Case SchemeClaimModel.EnumControlType.COVID19OR
                    Dim udcInputCOVID19OR As ucInputCOVID19OR = Me.udInputEHSClaim.GetCOVID19ORControl()
                    If Not udcInputCOVID19OR Is Nothing Then
                        udcInputCOVID19OR.SetCategoryError(False)
                        udcInputCOVID19OR.SetDoseErrorImage(False)
                        udcInputCOVID19OR.SetDetailError(False)
                    End If
                    ' CRE20-0023 (Immu record) [End][Chris YIM]

            End Select
        End If



    End Sub

#Region "HKIC Symbol Input"
    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Public Sub EnableHKICSymbolRadioButtonList(ByVal blnShow As Boolean, ByVal strSchemeCode As String, ByVal dtmServiceDate As Date)
        If blnShow Then
            ' Check system parameters to see if enable
            If Common.OCSSS.OCSSSServiceBLL.EnableHKICSymbolInputForBackOffice(strSchemeCode, dtmServiceDate) Then
                Dim strSelectedValue As String = String.Empty

                'Store selected value into temp variable
                If rblHKICSymbol.SelectedValue <> String.Empty Then
                    strSelectedValue = rblHKICSymbol.SelectedValue
                End If

                'Clear radio button list
                ClearHKICSymbolButtonList()

                'Reload radio button list
                rblHKICSymbol.DataSource = Status.GetDescriptionListFromDBEnumCode("HKICSymbol")

                Select Case Session("language")
                    Case CultureLanguage.English
                        rblHKICSymbol.DataTextField = "Status_Description"
                    Case CultureLanguage.TradChinese
                        rblHKICSymbol.DataTextField = "Status_Description_Chi"
                    Case CultureLanguage.SimpChinese
                        rblHKICSymbol.DataTextField = "Status_Description_CN"
                    Case Else
                        rblHKICSymbol.DataTextField = "Status_Description"
                End Select

                rblHKICSymbol.DataValueField = "Status_Value"
                rblHKICSymbol.DataBind()

                'Restore selected value from temp variable
                If strSelectedValue <> String.Empty Then
                    rblHKICSymbol.SelectedValue = strSelectedValue
                End If

                panHKICSymbol.Visible = True

            Else
                panHKICSymbol.Visible = False

            End If

        Else
            panHKICSymbol.Visible = False

        End If

    End Sub

    ' CRE17-010 (OCSSS integration) [End][Chris YIM]

    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Private Sub rblHKICSymbol_DataBound(sender As Object, e As EventArgs) Handles rblHKICSymbol.DataBound
        Dim rbl As RadioButtonList = CType(sender, RadioButtonList)

        For idx As Integer = 0 To rbl.Items.Count - 1
            rbl.Items(idx).Value = rbl.Items(idx).Value.Trim
        Next

    End Sub
    ' CRE17-010 (OCSSS integration) [End][Chris YIM]

    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Private Sub ClearHKICSymbolButtonList()
        Me.rblHKICSymbol.Items.Clear()
        Me.rblHKICSymbol.SelectedIndex = -1
        Me.rblHKICSymbol.SelectedValue = Nothing
        Me.imgErrHKICSymbol.Visible = False
    End Sub
    ' CRE17-010 (OCSSS integration) [End][Chris YIM]
#End Region

#Region "Scheme Validation"

    Private Function CIVSSValidation(ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = False

        Dim udcInputCIVSS As ucInputCIVSS = Me.udInputEHSClaim.GetCIVSSControl()
        Dim udtEHSClaimVaccine As EHSClaimVaccine.EHSClaimVaccineModel = Me.udtSessionHandlerBLL.EHSClaimVaccineGetFromSession(FunctionCode)

        udcInputCIVSS.SetDoseErrorImage(False)

        ' -----------------------------------------------
        ' UI Input Validation
        '------------------------------------------------
        udtEHSClaimVaccine = udcInputCIVSS.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
        Me.udtSessionHandlerBLL.EHSClaimVaccineSaveToSession(udtEHSClaimVaccine, FunctionCode)

        Dim udtSMList As Dictionary(Of SystemMessage, List(Of String()))
        isValid = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, udtSMList)
        If Not isValid Then
            udcInputCIVSS.SetDoseErrorImage(True)
            For Each kvp As KeyValuePair(Of SystemMessage, List(Of String())) In udtSMList
                If IsNothing(kvp.Value) Then
                    Me.udcMessageBox.AddMessage(kvp.Key)
                Else
                    Dim s As List(Of String())
                    s = kvp.Value
                    Me.udcMessageBox.AddMessage(kvp.Key, s(0), s(1))
                End If

            Next

        End If

        Return isValid

    End Function

    Private Function EVSSValidation(ByRef udtEHSTransaction As EHSTransactionModel)
        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = False

        Dim udcInputEVSS As ucInputEVSS = Me.udInputEHSClaim.GetEVSSControl()
        Dim udtEHSClaimVaccine As EHSClaimVaccine.EHSClaimVaccineModel = Me.udtSessionHandlerBLL.EHSClaimVaccineGetFromSession(FunctionCode)

        udcInputEVSS.SetDoseErrorImage(False)

        ' -----------------------------------------------
        ' UI Input Validation
        '------------------------------------------------
        udtEHSClaimVaccine = udcInputEVSS.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
        Me.udtSessionHandlerBLL.EHSClaimVaccineSaveToSession(udtEHSClaimVaccine, FunctionCode)

        Dim udtSMList As Dictionary(Of SystemMessage, List(Of String()))
        isValid = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, udtSMList)
        If Not isValid Then
            udcInputEVSS.SetDoseErrorImage(True)
            For Each kvp As KeyValuePair(Of SystemMessage, List(Of String())) In udtSMList
                If IsNothing(kvp.Value) Then
                    Me.udcMessageBox.AddMessage(kvp.Key)
                Else
                    Dim s As List(Of String())
                    s = kvp.Value
                    Me.udcMessageBox.AddMessage(kvp.Key, s(0), s(1))
                End If

            Next


        End If


        Return isValid

    End Function

    Private Function HCVSValidation(ByRef udtEHSTransaction As EHSTransactionModel)
        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = True
        Dim strDOB As String = String.Empty

        'Dim systemMessage As SystemMessage = Nothing

        Dim udcInputHCVS As ucInputHCVS = Me.udInputEHSClaim.GetHCVSControl
        Dim udtValidator As Validator = New Validator()

        Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL

        Dim udtEHSAccount As EHSAccount.EHSAccountModel = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        Dim udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtSchemeClaim As SchemeClaimModel = Me.udtSessionHandlerBLL.SelectSchemeGetFromSession(FunctionCode)

        Dim intAvailableVoucher As Integer = 0

        udcInputHCVS.SetReasonForVisitError(False)
        udcInputHCVS.SetVoucherredeemError(False)

        ' -----------------------------------------------
        ' UI Input Validation
        '------------------------------------------------
        'systemMessage = udtValidator.chkVoucherRedeem(udcInputHCVS.VoucherRedeem, udtEHSAccount.AvailableVoucher())
        'If Not systemMessage Is Nothing Then
        '    isValid = False
        '    udcInputHCVS.SetVoucherredeemError(True)
        '    Me.udcMessageBox.AddMessage(systemMessage)
        'End If


        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        If Not udcInputHCVS.Validate(True, Me.udcMessageBox) Then
            isValid = False
        End If

        'Dim intVoucherRedeem As Integer = 0

        ''if radio button selected index is more then 6 and voucher redeem is not entered
        'If udcInputHCVS.VoucherRedeem.Equals(String.Empty) Then
        '    systemMessage = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00122)
        '    isValid = False
        '    udcInputHCVS.SetVoucherredeemError(True)
        '    Me.udcMessageBox.AddMessage(systemMessage)
        'Else
        '    If Integer.TryParse(udcInputHCVS.VoucherRedeem, intVoucherRedeem) Then
        '        If intVoucherRedeem < 1 Then
        '            systemMessage = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00124)
        '            isValid = False
        '            udcInputHCVS.SetVoucherredeemError(True)
        '            Me.udcMessageBox.AddMessage(systemMessage)
        '        End If
        '    Else
        '        systemMessage = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00124)
        '        isValid = False
        '        udcInputHCVS.SetVoucherredeemError(True)
        '        Me.udcMessageBox.AddMessage(systemMessage)
        '    End If
        'End If

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Koala]

        '' ------------------------------------------------------------------
        '' Check Last Service Date of SubsidizeGroupClaim
        '' ------------------------------------------------------------------
        'systemMessage = udtValidator.chkServiceDataSubsidizeGroupLastServiceData(udtEHSTransaction.ServiceDate, udtSchemeClaim.SubsidizeGroupClaimList(0))
        'If Not systemMessage Is Nothing Then
        '    isValid = False
        '    Me.imgStep2aServiceDateError.Visible = True
        '    Me.udcMsgBoxErr.AddMessage(systemMessage)
        'End If

        If isValid Then

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            udcInputHCVS.Save(udtEHSTransaction)
            ' -----------------------------------------------
            ' Set up Transaction Model: Addition Fields
            '------------------------------------------------
            'udtEHSTransaction.VoucherClaim = udcInputHCVS.VoucherRedeem
            'udtEHSTransaction.UIInput = udcInputHCVS.UIInput

            '' Reason For Visit Level1
            'Dim udtTransactAdditionfield As TransactionAdditionalFieldModel = New TransactionAdditionalFieldModel()
            'udtTransactAdditionfield.AdditionalFieldID = "Reason_for_Visit_L1"
            'udtTransactAdditionfield.AdditionalFieldValueCode = udcInputHCVS.ReasonForVisitFirst
            'udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
            'udtTransactAdditionfield.SchemeCode = SchemeClaimModel.HCVS
            'udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SchemeSeq
            'udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode
            'udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

            '' Reason For Visit Level2
            'udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            'udtTransactAdditionfield.AdditionalFieldID = "Reason_for_Visit_L2"
            'udtTransactAdditionfield.AdditionalFieldValueCode = udcInputHCVS.ReasonForVisitSecond
            'udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
            'udtTransactAdditionfield.SchemeCode = SchemeClaimModel.HCVS
            'udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SchemeSeq
            'udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode
            'udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Koala]
        End If

        Return isValid
    End Function
    'CRE13-019-02 Extend HCVS to China [Start][Karl]
    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
    Private Function HCVSChinaValidation(ByRef udtEHSTransaction As EHSTransactionModel)
        'CRE13-019-02 Extend HCVS to China [End][Karl]
        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = True
        Dim strDOB As String = String.Empty

        'CRE13-019-02 Extend HCVS to China [Start][Karl]
        Dim udcInputHCVSChina As ucInputHCVSChina = Me.udInputEHSClaim.GetHCVSChinaControl
        'CRE13-019-02 Extend HCVS to China [Start][Karl]

        Dim udtValidator As Validator = New Validator()

        Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL

        Dim udtEHSAccount As EHSAccount.EHSAccountModel = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        Dim udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtSchemeClaim As SchemeClaimModel = Me.udtSessionHandlerBLL.SelectSchemeGetFromSession(FunctionCode)

        Dim intAvailableVoucher As Integer = 0
        'CRE13-019-02 Extend HCVS to China [Start][Karl]
        udcInputHCVSChina.SetVoucherredeemError(False)
        'CRE13-019-02 Extend HCVS to China [End][Karl]
        ' -----------------------------------------------
        ' UI Input Validation
        '------------------------------------------------
        'CRE13-019-02 Extend HCVS to China [Start][Karl]
        If Not udcInputHCVSChina.Validate(True, Me.udcMessageBox) Then
            isValid = False
        End If

        If isValid Then
            udcInputHCVSChina.Save(udtEHSTransaction)
        End If
        'CRE13-019-02 Extend HCVS to China [End][Karl]
        Return isValid
    End Function
    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

    Private Function HSIVSSValidation(ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
        ' ---------------------------------------------
        ' Init
        '----------------------------------------------

        Dim isValid As Boolean = True
        Dim noDoseSelected As Boolean = True
        Dim dtmServiceDate As Date = udtEHSTransaction.ServiceDate


        Dim udtSchemeDetailBLL As New Common.Component.SchemeDetails.SchemeDetailBLL()
        Dim udcInputHSIVSS As ucInputHSIVSS = Me.udInputEHSClaim.GetHSIVSSControl()
        Dim udtEHSClaimVaccine As EHSClaimVaccine.EHSClaimVaccineModel = Me.udtSessionHandlerBLL.EHSClaimVaccineGetFromSession(FunctionCode)
        Dim udtEHSClaimSubsidize As EHSClaimVaccine.EHSClaimVaccineModel.EHSClaimSubsidizeModel = udtEHSClaimVaccine.SubsidizeList(0)

        Dim udtSchemeClaim As SchemeClaimModel = Me.udtSessionHandlerBLL.SelectSchemeGetFromSession(FunctionCode)
        Dim udtClaimCategory As ClaimCategory.ClaimCategoryModel = Me.udtSessionHandlerBLL.ClaimCategoryGetFromSession(FunctionCode)

        'Init Controls
        udcInputHSIVSS.SetPreConditionError(False)
        udcInputHSIVSS.SetDoseErrorImage(False)

        ' -----------------------------------------------
        ' UI Input Validation
        '------------------------------------------------

        If String.IsNullOrEmpty(udcInputHSIVSS.Category) Then
            isValid = False
            udcInputHSIVSS.SetCategoryError(True)
            Me.udtSM = New SystemMessage("990000", "E", "00238")
            Me.udcMessageBox.AddMessage(udtSM)
        Else
            If udtClaimCategory.IsMedicalCondition = "Y" AndAlso String.IsNullOrEmpty(udcInputHSIVSS.PreCondition) Then
                isValid = False
                udcInputHSIVSS.SetPreConditionError(True)
                Me.udtSM = New SystemMessage("990000", "E", "00196")
                Me.udcMessageBox.AddMessage(Me.udtSM)
            End If
        End If

        If isValid Then
            udtEHSClaimVaccine = udcInputHSIVSS.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
            Dim udtSMList As Dictionary(Of SystemMessage, List(Of String()))
            isValid = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, udtSMList)
            If Not isValid Then
                udcInputHSIVSS.SetDoseErrorImage(True)
                For Each kvp As KeyValuePair(Of SystemMessage, List(Of String())) In udtSMList
                    If IsNothing(kvp.Value) Then
                        Me.udcMessageBox.AddMessage(kvp.Key)
                    Else
                        Dim s As List(Of String())
                        s = kvp.Value
                        Me.udcMessageBox.AddMessage(kvp.Key, s(0), s(1))
                    End If
                Next

            End If
        End If

        If isValid Then

            Dim udtTransactAdditionfield As TransactionAdditionalFieldModel
            udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()

            '-------------------------------------------------
            ' Set up Transaction Model Addition Fields : Category
            '-------------------------------------------------
            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            udtEHSTransaction.CategoryCode = udcInputHSIVSS.Category
            'CRE16-002 (Revamp VSS) [End][Chris YIM]



            ' -----------------------------------------------
            ' Set up Transaction Model Addition Fields : PreCondition
            '------------------------------------------------
            If udtClaimCategory.IsMedicalCondition = "Y" Then
                udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                udtTransactAdditionfield.AdditionalFieldID = "PreCondition"
                udtTransactAdditionfield.AdditionalFieldValueCode = udcInputHSIVSS.PreCondition
                udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                'udtTransactAdditionfield.SchemeCode = SchemeClaimModel.HSIVSS
                'udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SchemeSeq
                udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
                udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
                udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode.Trim()
                udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
            End If

        End If

        Return isValid
    End Function

    Private Function RVPValidation(ByRef udtehstransaction As EHSTransactionModel) As Boolean

        ' ---------------------------------------------
        ' Init
        '----------------------------------------------

        Dim isValid As Boolean = True
        Dim noVaccineSelected As Boolean = True
        Dim noDoseSelected As Boolean = True
        Dim dtmServiceDate As Date = udtehstransaction.ServiceDate

        Dim udtSchemeDetailBLL As New Common.Component.SchemeDetails.SchemeDetailBLL()
        Dim udcInputRVP As ucInputRVP = Me.udInputEHSClaim.GetRVPControl()
        Dim udtEHSClaimVaccine As EHSClaimVaccine.EHSClaimVaccineModel = Me.udtSessionHandlerBLL.EHSClaimVaccineGetFromSession(FunctionCode)
        Dim udtGeneralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
        udtEHSClaimVaccine = udcInputRVP.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)

        Dim udtSchemeClaim As SchemeClaimModel = Me.udtSessionHandlerBLL.SelectSchemeGetFromSession(FunctionCode)

        Dim strEnableClaimCategory As String = Nothing
        udtGeneralFunction.getSytemParameterByParameterNameSchemeCode("RVPEnableClaimCategory", strEnableClaimCategory, String.Empty, SchemeClaimModel.RVP)

        'Init Controls
        udcInputRVP.SetRCHCodeError(False)
        udcInputRVP.SetCategoryError(False)
        udcInputRVP.SetDoseErrorImage(False)
        udcInputRVP.SetCOVID19DetailError(False)

        ' -----------------------------------------------
        ' UI Input Validation
        '------------------------------------------------

        'Claim Detial Part & Vaccine Part
        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        isValid = udcInputRVP.Validate(True, Me.udcMessageBox, strEnableClaimCategory)

        If isValid Then
            udcInputRVP.Save(udtehstransaction, udtEHSClaimVaccine, strEnableClaimCategory)
        End If
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        Return isValid

    End Function

    ' CRE13-001 - EHAPP [Start][Tommy L]
    ' -------------------------------------------------------------------------------------
    Private Function EHAPPValidation(ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = True

        Dim udcInputEHAPP As ucInputEHAPP = Me.udInputEHSClaim.GetEHAPPControl()

        ' -----------------------------------------------
        ' UI Input Validation
        '------------------------------------------------
        If Not udcInputEHAPP.Validate(Me.udcMessageBox) Then
            isValid = False
        End If

        If isValid Then
            udcInputEHAPP.Save(udtEHSTransaction)
        End If

        Return isValid
    End Function
    ' CRE13-001 - EHAPP [End][Tommy L]

    'CRE15-005-03 (New PIDVSS scheme) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Function PIDVSSValidation(ByRef udtEHSTransaction As EHSTransactionModel) As Boolean

        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = False

        Dim udcInputPIDVSS As ucInputPIDVSS = Me.udInputEHSClaim.GetPIDVSSControl()
        Dim udtEHSClaimVaccine As EHSClaimVaccine.EHSClaimVaccineModel = Me.udtSessionHandlerBLL.EHSClaimVaccineGetFromSession(FunctionCode)

        udcInputPIDVSS.SetDoseErrorImage(False)

        ' -----------------------------------------------
        ' UI Input Validation
        '------------------------------------------------
        Dim isValidDetail, isValidVaccineSelection As Boolean

        'Claim Detial Part
        If Not udcInputPIDVSS.Validate(True, Me.udcMessageBox) Then
            isValidDetail = False
        Else
            isValidDetail = True
        End If

        'Select Vaccine Part
        udtEHSClaimVaccine = udcInputPIDVSS.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
        Me.udtSessionHandlerBLL.EHSClaimVaccineSaveToSession(udtEHSClaimVaccine, FunctionCode)

        Dim udtSMList As Dictionary(Of SystemMessage, List(Of String())) = Nothing
        isValidVaccineSelection = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, udtSMList)
        If Not isValidVaccineSelection Then
            udcInputPIDVSS.SetDoseErrorImage(True)
        End If

        'Combine Result
        isValid = isValidDetail And isValidVaccineSelection

        If Not isValid Then
            For Each kvp As KeyValuePair(Of SystemMessage, List(Of String())) In udtSMList
                If IsNothing(kvp.Value) Then
                    Me.udcMessageBox.AddMessage(kvp.Key)
                Else
                    Dim s As List(Of String())
                    s = kvp.Value
                    Me.udcMessageBox.AddMessage(kvp.Key, s(0), s(1))
                End If

            Next

        End If

        If isValid Then
            udcInputPIDVSS.Save(udtEHSTransaction, udtEHSClaimVaccine)
        End If

        Return isValid

    End Function
    'CRE15-005-03 (New PIDVSS scheme) [End][Chris YIM]

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Function VSSValidation(ByRef udtEHSTransaction As EHSTransactionModel) As Boolean

        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = False

        Dim udcInputVSS As ucInputVSS = Me.udInputEHSClaim.GetVSSControl()
        Dim udtEHSClaimVaccine As EHSClaimVaccine.EHSClaimVaccineModel = Me.udtSessionHandlerBLL.EHSClaimVaccineGetFromSession(FunctionCode)

        udcInputVSS.SetDoseErrorImage(False)

        ' -----------------------------------------------
        ' UI Input Validation
        '------------------------------------------------
        Dim isValidDetail, isValidVaccineSelection As Boolean

        'Claim Detial Part
        If Not udcInputVSS.Validate(True, Me.udcMessageBox) Then
            isValidDetail = False
        Else
            isValidDetail = True
        End If

        'Select Vaccine Part
        udtEHSClaimVaccine = udcInputVSS.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
        Me.udtSessionHandlerBLL.EHSClaimVaccineSaveToSession(udtEHSClaimVaccine, FunctionCode)

        Dim udtSMList As Dictionary(Of SystemMessage, List(Of String())) = Nothing
        isValidVaccineSelection = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, udtSMList)
        If Not isValidVaccineSelection Then
            udcInputVSS.SetDoseErrorImage(True)
        End If

        'Combine Result
        isValid = isValidDetail And isValidVaccineSelection

        If Not isValid Then
            For Each kvp As KeyValuePair(Of SystemMessage, List(Of String())) In udtSMList
                If IsNothing(kvp.Value) Then
                    Me.udcMessageBox.AddMessage(kvp.Key)
                Else
                    Dim s As List(Of String())
                    s = kvp.Value
                    Me.udcMessageBox.AddMessage(kvp.Key, s(0), s(1))
                End If

            Next

        End If

        If isValid Then
            udcInputVSS.Save(udtEHSTransaction, udtEHSClaimVaccine)
        End If

        Return isValid

    End Function
    'CRE16-002 (Revamp VSS) [End][Chris YIM]

    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Function ENHVSSOValidation(ByRef udtEHSTransaction As EHSTransactionModel) As Boolean

        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = False

        Dim udcInputENHVSSO As ucInputENHVSSO = Me.udInputEHSClaim.GetENHVSSOControl()
        Dim udtEHSClaimVaccine As EHSClaimVaccine.EHSClaimVaccineModel = Me.udtSessionHandlerBLL.EHSClaimVaccineGetFromSession(FunctionCode)

        udcInputENHVSSO.SetDoseErrorImage(False)

        ' -----------------------------------------------
        ' UI Input Validation
        '------------------------------------------------
        Dim isValidDetail, isValidVaccineSelection As Boolean

        'Claim Detial Part
        If Not udcInputENHVSSO.Validate(True, Me.udcMessageBox) Then
            isValidDetail = False
        Else
            isValidDetail = True
        End If

        'Select Vaccine Part
        udtEHSClaimVaccine = udcInputENHVSSO.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
        Me.udtSessionHandlerBLL.EHSClaimVaccineSaveToSession(udtEHSClaimVaccine, FunctionCode)

        Dim udtSMList As Dictionary(Of SystemMessage, List(Of String())) = Nothing
        isValidVaccineSelection = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, udtSMList)
        If Not isValidVaccineSelection Then
            udcInputENHVSSO.SetDoseErrorImage(True)
        End If

        'Combine Result
        isValid = isValidDetail And isValidVaccineSelection

        If Not isValid Then
            For Each kvp As KeyValuePair(Of SystemMessage, List(Of String())) In udtSMList
                If IsNothing(kvp.Value) Then
                    Me.udcMessageBox.AddMessage(kvp.Key)
                Else
                    Dim s As List(Of String())
                    s = kvp.Value
                    Me.udcMessageBox.AddMessage(kvp.Key, s(0), s(1))
                End If

            Next

        End If

        If isValid Then
            udcInputENHVSSO.Save(udtEHSTransaction, udtEHSClaimVaccine)
        End If

        Return isValid

    End Function
    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Function PPPValidation(ByRef udtEHSTransaction As EHSTransactionModel) As Boolean

        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = False

        Dim udcInputPPP As ucInputPPP = Me.udInputEHSClaim.GetPPPControl()
        Dim udtEHSClaimVaccine As EHSClaimVaccine.EHSClaimVaccineModel = Me.udtSessionHandlerBLL.EHSClaimVaccineGetFromSession(FunctionCode)

        udcInputPPP.SetDoseErrorImage(False)

        ' -----------------------------------------------
        ' UI Input Validation
        '------------------------------------------------
        Dim isValidDetail, isValidVaccineSelection As Boolean

        'Claim Detial Part
        If Not udcInputPPP.Validate(True, Me.udcMessageBox) Then
            isValidDetail = False
        Else
            isValidDetail = True
        End If

        'Select Vaccine Part
        udtEHSClaimVaccine = udcInputPPP.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
        Me.udtSessionHandlerBLL.EHSClaimVaccineSaveToSession(udtEHSClaimVaccine, FunctionCode)

        Dim udtSMList As Dictionary(Of SystemMessage, List(Of String())) = Nothing

        isValidVaccineSelection = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, udtSMList)
        If Not isValidVaccineSelection Then
            udcInputPPP.SetDoseErrorImage(True)
        End If

        'Combine Result
        isValid = isValidDetail And isValidVaccineSelection

        If Not isValid Then
            For Each kvp As KeyValuePair(Of SystemMessage, List(Of String())) In udtSMList
                If IsNothing(kvp.Value) Then
                    Me.udcMessageBox.AddMessage(kvp.Key)
                Else
                    Dim s As List(Of String())
                    s = kvp.Value
                    Me.udcMessageBox.AddMessage(kvp.Key, s(0), s(1))
                End If

            Next

        End If

        If isValid Then
            udcInputPPP.Save(udtEHSTransaction, udtEHSClaimVaccine)
        End If

        Return isValid

    End Function
    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

    ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Function SSSCMCValidation(ByRef udtEHSTransaction As EHSTransactionModel) As Boolean

        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = True
        Dim strDOB As String = String.Empty
        Dim udtEligibleResult As Common.Component.ClaimRules.ClaimRulesBLL.EligibleResult = Nothing
        Dim udtSystemMessage As SystemMessage = Nothing

        Dim udcInputSSSCMC As ucInputSSSCMC = Me.udInputEHSClaim.GetSSSCMCControl()

        Dim udtValidator As Validator = New Validator()

        Dim udtEHSAccount As EHSAccountModel = Me.udtSessionHandlerBLL.EHSAccountGetFromSession(FunctionCode)
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtSchemeClaim As SchemeClaimModel = Me.udtSessionHandlerBLL.SelectSchemeGetFromSession(FunctionCode)

        Dim decAvailableAmount As Decimal = 0

        udcInputSSSCMC.SetError(False)

        Me.udcMessageBox.Clear()

        ' -----------------------------------------------
        ' UI Input Validation
        '------------------------------------------------
        If Not udcInputSSSCMC.Validate(True, Me.udcMessageBox) Then
            isValid = False
        End If

        'If isValid Then
        '    ' --------------------------------------------------------------
        '    ' Check Eligibility:
        '    ' --------------------------------------------------------------
        '    If udtEHSPersonalInfo.DocCode = DocTypeModel.DocTypeCode.EC AndAlso udtEHSPersonalInfo.ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
        '        strDOB = udtFormatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, udtEHSPersonalInfo.ECAge, udtEHSPersonalInfo.ECDateOfRegistration)
        '    Else
        '        strDOB = udtFormatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, Nothing, Nothing)
        '    End If

        '    udtSystemMessage = udtEHSClaimBLL.CheckEligibilityForEnterClaim(udtSchemeClaim, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo, Nothing, udtEligibleResult)

        '    If Not udtSystemMessage Is Nothing Then
        '        ' If Check Eligibility Block Show Error
        '        isValid = False
        '        Me.udcMessageBox.AddMessage(udtSystemMessage)
        '    End If
        'End If

        'If isValid Then
        '    ' --------------------------------------------------------------
        '    ' Check Document Limit:
        '    ' --------------------------------------------------------------
        '    udtSystemMessage = Me.udtEHSClaimBLL.CheckExceedDocumentLimitForEnterClaim(udtSchemeClaim.SchemeCode, udtEHSTransaction.ServiceDate, udtEHSPersonalInfo)

        '    If Not udtSystemMessage Is Nothing Then
        '        isValid = False
        '        Me.udcMessageBox.AddMessage(udtSystemMessage)
        '    End If
        'End If

        ' INT20-00XX (Allow back-office claim in scheme SSSCMC when zero balance) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        'If isValid Then
        '    ' --------------------------------------------------------------
        '    ' Check Benefit:
        '    ' --------------------------------------------------------------
        '    Dim udtEHSTransactionBLL As New EHSTransactionBLL

        '    decAvailableAmount = udtEHSTransactionBLL.getAvailableSubsidizeItem_SSSCMC(udtEHSPersonalInfo, udtSchemeClaim.SubsidizeGroupClaimList)

        '    If decAvailableAmount > 0 AndAlso decAvailableAmount >= udcInputSSSCMC.UsedRMB Then
        '        ' Subsidies for SSSCMC is available
        '    Else
        '        ' No available subsidies for SSSCMC
        '        isValid = False
        '        Me.udcMessageBox.AddMessage(New SystemMessage("990000", "E", "00107"))
        '    End If

        'End If
        ' INT20-00XX (Allow back-office claim in scheme SSSCMC when zero balance) [End][Chris YIM]

        If isValid Then
            udtEHSTransaction.VoucherBeforeRedeem = Nothing
            udtEHSTransaction.VoucherAfterRedeem = Nothing
            udtEHSTransaction.VoucherClaim = Nothing
            udtEHSTransaction.ExchangeRate = udcInputSSSCMC.ExchangeRate
            udtEHSTransaction.VoucherClaimRMB = udcInputSSSCMC.UsedRMB

            udcInputSSSCMC.Save(udtEHSTransaction)

        End If

        Return isValid
    End Function
    ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Function COVID19Validation(ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = False

        Dim udcInputCOVID19 As ucInputCOVID19 = Me.udInputEHSClaim.GetCOVID19Control
        Dim udtEHSClaimVaccine As EHSClaimVaccine.EHSClaimVaccineModel = Me.udtSessionHandlerBLL.EHSClaimVaccineGetFromSession(FunctionCode)

        udcInputCOVID19.SetDoseErrorImage(False)

        ' -----------------------------------------------
        ' UI Input Validation
        '------------------------------------------------
        Dim isValidDetail, isValidVaccineSelection As Boolean

        'Claim Detial Part & Vaccine Part
        If Not udcInputCOVID19.Validate(True, Me.udcMessageBox) Then
            isValidDetail = False
        Else
            isValidDetail = True
        End If

        'Select Vaccine Part
        udtEHSClaimVaccine = udcInputCOVID19.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
        Me.udtSessionHandlerBLL.EHSClaimVaccineSaveToSession(udtEHSClaimVaccine, FunctionCode)

        Dim udtSMList As Dictionary(Of SystemMessage, List(Of String())) = Nothing
        isValidVaccineSelection = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, udtSMList)
        If Not isValidVaccineSelection Then
            udcInputCOVID19.SetDoseErrorImage(True)
        End If

        'Combine Result
        isValid = isValidDetail And isValidVaccineSelection

        If Not isValid Then
            For Each kvp As KeyValuePair(Of SystemMessage, List(Of String())) In udtSMList
                If IsNothing(kvp.Value) Then
                    Me.udcMessageBox.AddMessage(kvp.Key)
                Else
                    Dim s As List(Of String())
                    s = kvp.Value
                    Me.udcMessageBox.AddMessage(kvp.Key, s(0), s(1))
                End If

            Next

        End If

        If isValid Then
            udcInputCOVID19.Save(udtEHSTransaction, udtEHSClaimVaccine)
        End If

        Return isValid
    End Function
    ' CRE20-0022 (Immu record) [End][Chris YIM]

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Function COVID19CBDValidation(ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = False

        Dim udcInputCOVID19 As ucInputCOVID19 = Me.udInputEHSClaim.GetCOVID19Control
        Dim udtEHSClaimVaccine As EHSClaimVaccine.EHSClaimVaccineModel = Me.udtSessionHandlerBLL.EHSClaimVaccineGetFromSession(FunctionCode)

        udcInputCOVID19.SetDoseErrorImage(False)

        ' -----------------------------------------------
        ' UI Input Validation
        '------------------------------------------------
        Dim isValidDetail, isValidVaccineSelection As Boolean

        'Claim Detial Part & Vaccine Part
        If Not udcInputCOVID19.Validate(True, Me.udcMessageBox) Then
            isValidDetail = False
        Else
            isValidDetail = True
        End If

        'Select Vaccine Part
        udtEHSClaimVaccine = udcInputCOVID19.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
        Me.udtSessionHandlerBLL.EHSClaimVaccineSaveToSession(udtEHSClaimVaccine, FunctionCode)

        Dim udtSMList As Dictionary(Of SystemMessage, List(Of String())) = Nothing
        isValidVaccineSelection = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, udtSMList)
        If Not isValidVaccineSelection Then
            udcInputCOVID19.SetDoseErrorImage(True)
        End If

        'Combine Result
        isValid = isValidDetail And isValidVaccineSelection

        If Not isValid Then
            For Each kvp As KeyValuePair(Of SystemMessage, List(Of String())) In udtSMList
                If IsNothing(kvp.Value) Then
                    Me.udcMessageBox.AddMessage(kvp.Key)
                Else
                    Dim s As List(Of String())
                    s = kvp.Value
                    Me.udcMessageBox.AddMessage(kvp.Key, s(0), s(1))
                End If

            Next

        End If

        If isValid Then
            udcInputCOVID19.Save(udtEHSTransaction, udtEHSClaimVaccine)
        End If

        Return isValid
    End Function
    ' CRE20-0022 (Immu record) [End][Chris YIM]

    ' CRE20-0023 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Function COVID19RVPValidation(ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = False

        Dim udcInputCOVID19RVP As ucInputCOVID19RVP = Me.udInputEHSClaim.GetCOVID19RVPControl
        Dim udtEHSClaimVaccine As EHSClaimVaccine.EHSClaimVaccineModel = Me.udtSessionHandlerBLL.EHSClaimVaccineGetFromSession(FunctionCode)

        udcInputCOVID19RVP.SetDoseErrorImage(False)

        ' -----------------------------------------------
        ' UI Input Validation
        '------------------------------------------------
        Dim isValidDetail, isValidVaccineSelection As Boolean

        'Claim Detial Part & Vaccine Part
        If Not udcInputCOVID19RVP.Validate(True, Me.udcMessageBox) Then
            isValidDetail = False
        Else
            isValidDetail = True
        End If

        'Select Vaccine Part
        udtEHSClaimVaccine = udcInputCOVID19RVP.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
        Me.udtSessionHandlerBLL.EHSClaimVaccineSaveToSession(udtEHSClaimVaccine, FunctionCode)

        Dim udtSMList As Dictionary(Of SystemMessage, List(Of String())) = Nothing
        isValidVaccineSelection = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, udtSMList)
        If Not isValidVaccineSelection Then
            udcInputCOVID19RVP.SetDoseErrorImage(True)
        End If

        'Combine Result
        isValid = isValidDetail And isValidVaccineSelection

        If Not isValid Then
            For Each kvp As KeyValuePair(Of SystemMessage, List(Of String())) In udtSMList
                If IsNothing(kvp.Value) Then
                    Me.udcMessageBox.AddMessage(kvp.Key)
                Else
                    Dim s As List(Of String())
                    s = kvp.Value
                    Me.udcMessageBox.AddMessage(kvp.Key, s(0), s(1))
                End If

            Next

        End If

        If isValid Then
            udcInputCOVID19RVP.Save(udtEHSTransaction, udtEHSClaimVaccine)
        End If

        Return isValid
    End Function
    ' CRE20-0023 (Immu record) [End][Chris YIM]

    ' CRE20-0023 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Function COVID19ORValidation(ByRef udtEHSTransaction As EHSTransactionModel) As Boolean
        ' ---------------------------------------------
        ' Init
        '----------------------------------------------
        Dim isValid As Boolean = False

        Dim udcInputCOVID19OR As ucInputCOVID19OR = Me.udInputEHSClaim.GetCOVID19ORControl
        Dim udtEHSClaimVaccine As EHSClaimVaccine.EHSClaimVaccineModel = Me.udtSessionHandlerBLL.EHSClaimVaccineGetFromSession(FunctionCode)

        udcInputCOVID19OR.SetDoseErrorImage(False)

        ' -----------------------------------------------
        ' UI Input Validation
        '------------------------------------------------
        Dim isValidDetail, isValidVaccineSelection As Boolean

        'Claim Detial Part & Vaccine Part
        If Not udcInputCOVID19OR.Validate(True, Me.udcMessageBox) Then
            isValidDetail = False
        Else
            isValidDetail = True
        End If

        'Select Vaccine Part
        udtEHSClaimVaccine = udcInputCOVID19OR.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
        Me.udtSessionHandlerBLL.EHSClaimVaccineSaveToSession(udtEHSClaimVaccine, FunctionCode)

        Dim udtSMList As Dictionary(Of SystemMessage, List(Of String())) = Nothing
        isValidVaccineSelection = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, udtSMList)
        If Not isValidVaccineSelection Then
            udcInputCOVID19OR.SetDoseErrorImage(True)
        End If

        'Combine Result
        isValid = isValidDetail And isValidVaccineSelection

        If Not isValid Then
            For Each kvp As KeyValuePair(Of SystemMessage, List(Of String())) In udtSMList
                If IsNothing(kvp.Value) Then
                    Me.udcMessageBox.AddMessage(kvp.Key)
                Else
                    Dim s As List(Of String())
                    s = kvp.Value
                    Me.udcMessageBox.AddMessage(kvp.Key, s(0), s(1))
                End If

            Next

        End If

        If isValid Then
            udcInputCOVID19OR.Save(udtEHSTransaction, udtEHSClaimVaccine)
        End If

        Return isValid
    End Function
    ' CRE20-0023 (Immu record) [End][Chris YIM]
#End Region

#Region "Scheme Audit Log"
    'Voucher Scheme
    Private Sub AuditLogVoucher(ByRef udtAuditLogEntry As AuditLogEntry, ByVal strSchemecode As String, ByVal dtmServiceDate As Date)
        'udtAuditLogEntry.AddDescripton("Scheme Code", SchemeClaimModel.HCVS)

        Dim udtSchemeClaimBLL As New SchemeClaimBLL
        Dim udtSchemeDetailBLL As New SchemeDetails.SchemeDetailBLL
        Dim udtSchemeClaimModel As SchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(strSchemecode, dtmServiceDate.AddDays(1).AddMinutes(-1))
        Dim udtSubsidizeItemDetailList As SchemeDetails.SubsidizeItemDetailsModelCollection = udtSchemeDetailBLL.getSubsidizeItemDetails(udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeItemCode)

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        'udtAuditLogEntry.AddDescripton("Scheme Seq", udtSchemeClaimModel.SchemeSeq)
        udtAuditLogEntry.AddDescripton("Scheme Seq", udtSchemeClaimModel.SubsidizeGroupClaimList(0).SchemeSeq)
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        udtAuditLogEntry.AddDescripton("Subsidize Code", udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeCode)
        udtAuditLogEntry.AddDescripton("Subsidize Item Code", udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeItemCode)
        udtAuditLogEntry.AddDescripton("Available Item Code", udtSubsidizeItemDetailList(0).AvailableItemCode)

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]        
        Dim udcInputHCVS As ucInputHCVS = Me.udInputEHSClaim.GetHCVSControl

        udtAuditLogEntry.AddDescripton("Voucher Redeem", udcInputHCVS.VoucherRedeem)
        udtAuditLogEntry.AddDescripton("Copayment Fee", udcInputHCVS.CoPaymentFee)

        udtAuditLogEntry.AddDescripton("Reason_for_Visit_L1", udcInputHCVS.ReasonForVisitFirst)
        udtAuditLogEntry.AddDescripton("Reason_for_Visit_L2", udcInputHCVS.ReasonForVisitSecond)

        If String.IsNullOrEmpty(udcInputHCVS.ReasonForVisitSecondaryL1(1)) = False Then
            udtAuditLogEntry.AddDescripton("ReasonforVisit_S1_L1", udcInputHCVS.ReasonForVisitSecondaryL1(1))
        End If

        If String.IsNullOrEmpty(udcInputHCVS.ReasonForVisitSecondaryL2(1)) = False Then
            udtAuditLogEntry.AddDescripton("ReasonforVisit_S1_L2", udcInputHCVS.ReasonForVisitSecondaryL2(1))
        End If

        If String.IsNullOrEmpty(udcInputHCVS.ReasonForVisitSecondaryL1(2)) = False Then
            udtAuditLogEntry.AddDescripton("ReasonforVisit_S2_L1", udcInputHCVS.ReasonForVisitSecondaryL1(2))
        End If

        If String.IsNullOrEmpty(udcInputHCVS.ReasonForVisitSecondaryL2(2)) = False Then
            udtAuditLogEntry.AddDescripton("ReasonforVisit_S2_L2", udcInputHCVS.ReasonForVisitSecondaryL2(2))
        End If

        If String.IsNullOrEmpty(udcInputHCVS.ReasonForVisitSecondaryL1(3)) = False Then
            udtAuditLogEntry.AddDescripton("ReasonforVisit_S3_L1", udcInputHCVS.ReasonForVisitSecondaryL1(3))
        End If

        If String.IsNullOrEmpty(udcInputHCVS.ReasonForVisitSecondaryL2(3)) = False Then
            udtAuditLogEntry.AddDescripton("ReasonforVisit_S3_L2", udcInputHCVS.ReasonForVisitSecondaryL2(3))
        End If
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

    End Sub
    'CRE13-019-02 Extend HCVS to China [Start][Karl]
    'China Voucher Scheme
    Private Sub AuditLogChinaVoucher(ByRef udtAuditLogEntry As AuditLogEntry, ByVal strSchemecode As String, ByVal dtmServiceDate As Date)
        'udtAuditLogEntry.AddDescripton("Scheme Code", SchemeClaimModel.HCVS)

        Dim udtSchemeClaimBLL As New SchemeClaimBLL
        Dim udtSchemeDetailBLL As New SchemeDetails.SchemeDetailBLL
        Dim udtSchemeClaimModel As SchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(strSchemecode, dtmServiceDate.AddDays(1).AddMinutes(-1))
        Dim udtSubsidizeItemDetailList As SchemeDetails.SubsidizeItemDetailsModelCollection = udtSchemeDetailBLL.getSubsidizeItemDetails(udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeItemCode)

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        'udtAuditLogEntry.AddDescripton("Scheme Seq", udtSchemeClaimModel.SchemeSeq)
        udtAuditLogEntry.AddDescripton("Scheme Seq", udtSchemeClaimModel.SubsidizeGroupClaimList(0).SchemeSeq)
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        udtAuditLogEntry.AddDescripton("Subsidize Code", udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeCode)
        udtAuditLogEntry.AddDescripton("Subsidize Item Code", udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeItemCode)
        udtAuditLogEntry.AddDescripton("Available Item Code", udtSubsidizeItemDetailList(0).AvailableItemCode)


        Dim udcInputHCVSChina As ucInputHCVSChina = Me.udInputEHSClaim.GetHCVSChinaControl
        Dim udtSchemeClaim As SchemeClaimModel
        Dim udtExchangeRate As New ExchangeRate.ExchangeRateBLL
        Dim dblSubsidizeFee As Double
        Dim intVoucherRedeem As Integer

        udtSchemeClaim = udtSchemeClaimBLL.getAllDistinctSchemeClaim_WithEffectiveSubsidizeGroup(udcInputHCVSChina.ServiceDate).Filter(strSchemecode)
        If Not udtSchemeClaim Is Nothing Then
            dblSubsidizeFee = (udtSchemeClaim.SubsidizeGroupClaimList.Filter(udcInputHCVSChina.ServiceDate))(0).SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVoucher, udcInputHCVSChina.ServiceDate).SubsidizeFee
        End If

        udtAuditLogEntry.AddDescripton("ExchangeRate", udcInputHCVSChina.ExchangeRate)

        If String.IsNullOrEmpty(udcInputHCVSChina.VoucherRedeem) = False Then
            intVoucherRedeem = udcInputHCVSChina.VoucherRedeem
            udtAuditLogEntry.AddDescripton("Voucher amount claimed (in HKD)", intVoucherRedeem)
            udtAuditLogEntry.AddDescripton("Voucher amount claimed (HKD * subsidizeFee)", intVoucherRedeem * dblSubsidizeFee)

            If String.IsNullOrEmpty(udcInputHCVSChina.VoucherRedeemRMB) = False Then
                If Double.TryParse(udcInputHCVSChina.VoucherRedeemRMB, Nothing) = True Then
                    udtAuditLogEntry.AddDescripton("Voucher amount claimed (in RMB)", udcInputHCVSChina.VoucherRedeemRMB)
                    udtAuditLogEntry.AddDescripton("Voucher amount claimed (RMB * subsidizeFee)", udcInputHCVSChina.VoucherRedeemRMB * dblSubsidizeFee)
                Else
                    udtAuditLogEntry.AddDescripton("Voucher amount claimed (in RMB)", "Invalid character : " & udcInputHCVSChina.VoucherRedeemRMB)
                End If
            End If
        End If

        'If String.IsNullOrEmpty(udcInputHCVSChina.CoPaymentFee) = False Then
        '    If Decimal.TryParse(udcInputHCVSChina.CoPaymentFee, Nothing) Then
        '        udtAuditLogEntry.AddDescripton("CoPaymentFeeHKD", udtExchangeRate.CalculateRMBtoHKD(udcInputHCVSChina.CoPaymentFee, udcInputHCVSChina.ExchangeRate))            
        '    End If
        'Else
        '    udtAuditLogEntry.AddDescripton("CoPaymentFeeHKD", String.Empty)
        'End If

        udtAuditLogEntry.AddDescripton("CoPaymentFeeRMB", udcInputHCVSChina.CoPaymentFee)

        udtAuditLogEntry.AddDescripton("PaymentType", udcInputHCVSChina.PaymentType)

        udtAuditLogEntry.AddDescripton("Reason_for_Visit_L1", udcInputHCVSChina.ReasonForVisitFirst)
        'udtAuditLogEntry.AddDescripton("Reason_for_Visit_L2", udcInputHCVSChina.ReasonForVisitSecond)

        If String.IsNullOrEmpty(udcInputHCVSChina.ReasonForVisitSecondaryL1(1)) = False Then
            udtAuditLogEntry.AddDescripton("ReasonforVisit_S1_L1", udcInputHCVSChina.ReasonForVisitSecondaryL1(1))
        End If

        'If String.IsNullOrEmpty(udcInputHCVSChina.ReasonForVisitSecondaryL2(1)) = False Then
        '    udtAuditLogEntry.AddDescripton("ReasonforVisit_S1_L2", udcInputHCVSChina.ReasonForVisitSecondaryL2(1))
        'End If

        If String.IsNullOrEmpty(udcInputHCVSChina.ReasonForVisitSecondaryL1(2)) = False Then
            udtAuditLogEntry.AddDescripton("ReasonforVisit_S2_L1", udcInputHCVSChina.ReasonForVisitSecondaryL1(2))
        End If

        'If String.IsNullOrEmpty(udcInputHCVSChina.ReasonForVisitSecondaryL2(2)) = False Then
        '    udtAuditLogEntry.AddDescripton("ReasonforVisit_S2_L2", udcInputHCVSChina.ReasonForVisitSecondaryL2(2))
        'End If

        If String.IsNullOrEmpty(udcInputHCVSChina.ReasonForVisitSecondaryL1(3)) = False Then
            udtAuditLogEntry.AddDescripton("ReasonforVisit_S3_L1", udcInputHCVSChina.ReasonForVisitSecondaryL1(3))
        End If

        'If String.IsNullOrEmpty(udcInputHCVSChina.ReasonForVisitSecondaryL2(3)) = False Then
        '    udtAuditLogEntry.AddDescripton("ReasonforVisit_S3_L2", udcInputHCVSChina.ReasonForVisitSecondaryL2(3))
        'End If

    End Sub
    'CRE13-019-02 Extend HCVS to China [End][Karl]

    'Vaccination Scheme
    'CRE15-005-03 (New PIDVSS scheme) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    'Private Sub AuditLogVaccination(ByRef udtAuditLogEntry As AuditLogEntry, ByVal udtEHSClaimVaccineModel As EHSClaimVaccine.EHSClaimVaccineModel, ByVal dtmServiceDate As Date)
    Private Sub AuditLogVaccination(ByRef udtAuditLogEntry As AuditLogEntry, ByVal udtEHSClaimVaccineModel As EHSClaimVaccine.EHSClaimVaccineModel, ByVal dtmServiceDate As Date, ByVal udtEHSTransaction As EHSTransactionModel)
        'udtAuditLogEntry.AddDescripton("Scheme Code", SchemeClaimModel.EVSS)

        For Each udtSubsidize As EHSClaimVaccine.EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccineModel.SubsidizeList

            If udtSubsidize.Selected Then

                Dim udtSchemeClaimBLL As New SchemeClaimBLL
                Dim udtSchemeClaimModel As SchemeClaimModel = Me.udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSClaimVaccineModel.SchemeCode, dtmServiceDate.AddDays(1).AddMinutes(-1))

                If udtSubsidize.SubsidizeDetailList.Count = 1 Then
                    udtAuditLogEntry.AddDescripton("Scheme Seq", udtSchemeClaimModel.SubsidizeGroupClaimList.Filter(udtEHSClaimVaccineModel.SchemeCode, udtSubsidize.SubsidizeCode).SchemeSeq)
                    udtAuditLogEntry.AddDescripton("Subsidize Code", udtSubsidize.SubsidizeCode)
                    udtAuditLogEntry.AddDescripton("Subsidize Item Code", udtSubsidize.SubsidizeItemCode)
                    udtAuditLogEntry.AddDescripton("Available Item Code", udtSubsidize.SubsidizeDetailList(0).AvailableItemCode)


                ElseIf udtSubsidize.SubsidizeDetailList.Count > 1 Then
                    For Each udtSubsidizeDetail As EHSClaimVaccine.EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtSubsidize.SubsidizeDetailList
                        If udtSubsidizeDetail.Selected Then
                            udtAuditLogEntry.AddDescripton("Scheme Seq", udtSchemeClaimModel.SubsidizeGroupClaimList.Filter(udtEHSClaimVaccineModel.SchemeCode, udtSubsidize.SubsidizeCode).SchemeSeq)
                            udtAuditLogEntry.AddDescripton("Subsidize Code", udtSubsidize.SubsidizeCode)
                            udtAuditLogEntry.AddDescripton("Subsidize Item Code", udtSubsidize.SubsidizeItemCode)
                            udtAuditLogEntry.AddDescripton("Available Item Code", udtSubsidize.SubsidizeDetailList(0).AvailableItemCode)
                        End If
                    Next
                End If
            End If
        Next

        If Not udtEHSTransaction Is Nothing AndAlso Not udtEHSTransaction.TransactionAdditionFields Is Nothing Then
            For Each udtTransactAdditionfield As TransactionAdditionalFieldModel In udtEHSTransaction.TransactionAdditionFields
                'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                If udtTransactAdditionfield.AdditionalFieldValueDesc Is Nothing OrElse udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty Then
                    udtAuditLogEntry.AddDescripton(udtTransactAdditionfield.AdditionalFieldID, udtTransactAdditionfield.AdditionalFieldValueCode)
                Else
                    udtAuditLogEntry.AddDescripton(udtTransactAdditionfield.AdditionalFieldID, udtTransactAdditionfield.AdditionalFieldValueCode + " - " + udtTransactAdditionfield.AdditionalFieldValueDesc)
                End If
                'CRE16-002 (Revamp VSS) [End][Chris YIM]
            Next
        End If
    End Sub
    'CRE15-005-03 (New PIDVSS scheme) [End][Chris YIM]

    ' CRE13-001 - EHAPP [Start][Tommy L]
    ' -------------------------------------------------------------------------------------
    'EHAPP Scheme
    Private Sub AuditLogEHAPP(ByRef udtAuditLogEntry As AuditLogEntry, ByVal strSchemeCode As String, ByVal dtmServiceDate As Date)
        Dim udtSchemeClaimBLL As New SchemeClaimBLL
        Dim udtSchemeDetailBLL As New SchemeDetails.SchemeDetailBLL
        Dim udtSchemeClaimModel As SchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(strSchemeCode, dtmServiceDate.AddDays(1).AddMinutes(-1))
        Dim udtSubsidizeItemDetailList As SchemeDetails.SubsidizeItemDetailsModelCollection = udtSchemeDetailBLL.getSubsidizeItemDetails(udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeItemCode)
        Dim udcInputEHAPP As ucInputEHAPP = Me.udInputEHSClaim.GetEHAPPControl()

        udtAuditLogEntry.AddDescripton("Scheme Seq", udtSchemeClaimModel.SubsidizeGroupClaimList(0).SchemeSeq)
        udtAuditLogEntry.AddDescripton("Subsidize Code", udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeCode)
        udtAuditLogEntry.AddDescripton("Subsidize Item Code", udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeItemCode)
        udtAuditLogEntry.AddDescripton("Available Item Code", udtSubsidizeItemDetailList(0).AvailableItemCode)
        udtAuditLogEntry.AddDescripton("Co-payment Item No", udcInputEHAPP.CoPayment)
        'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]      
        Select Case udcInputEHAPP.CoPayment
            Case EHAPP_Copayment.VOUCHER
                udtAuditLogEntry.AddDescripton("HCV Amount", udcInputEHAPP.HCVAmount)
                udtAuditLogEntry.AddDescripton("Net Service Fee", udcInputEHAPP.NetServiceFee)

            Case Else

        End Select
        'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]      
    End Sub
    ' CRE13-001 - EHAPP [End][Tommy L]

    ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub AuditLogSSSCMC(ByRef udtAuditLogEntry As AuditLogEntry, ByVal udtEHSTransaction As EHSTransactionModel, ByVal dtmServiceDate As Date)
        If udtEHSTransaction.TransactionAdditionFields Is Nothing OrElse udtEHSTransaction.TransactionAdditionFields(0) Is Nothing Then
            Return
        End If

        Dim udtTAF As TransactionAdditionalFieldModel = udtEHSTransaction.TransactionAdditionFields(0)

        Dim udtSchemeClaimBLL As New SchemeClaimBLL
        Dim udtSchemeClaimModel As SchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSTransaction.SchemeCode.Trim, dtmServiceDate.AddDays(1).AddMinutes(-1))

        Dim udtSchemeDetailBLL As New SchemeDetails.SchemeDetailBLL
        Dim udtSubsidizeItemDetailList As SchemeDetails.SubsidizeItemDetailsModelCollection = udtSchemeDetailBLL.getSubsidizeItemDetails(udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeItemCode)

        udtAuditLogEntry.AddDescripton("Scheme", udtEHSTransaction.SchemeCode.Trim)
        udtAuditLogEntry.AddDescripton("Scheme Seq", udtTAF.SchemeSeq)
        udtAuditLogEntry.AddDescripton("Subsidize Code", udtTAF.SubsidizeCode)
        udtAuditLogEntry.AddDescripton("Subsidize Item Code", udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeItemCode)
        udtAuditLogEntry.AddDescripton("Available Item Code", udtSubsidizeItemDetailList(0).AvailableItemCode)

        udtAuditLogEntry.AddDescripton("ExchangeRate", udtEHSTransaction.ExchangeRate)
        udtAuditLogEntry.AddDescripton("VoucherRedeemRMB", udtEHSTransaction.VoucherClaimRMB)

        For Each udtTransactAdditionfield As TransactionAdditionalFieldModel In udtEHSTransaction.TransactionAdditionFields
            udtAuditLogEntry.AddDescripton(udtTransactAdditionfield.AdditionalFieldID, udtTransactAdditionfield.AdditionalFieldValueCode)
        Next

    End Sub
    ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

#End Region

#End Region

#End Region

#Region "Confirm Claim Detail"
    ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Protected Sub ibtnConfirmClaimCreationConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        Dim udtEHSAccount As EHSAccount.EHSAccountModel
        Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
        udtEHSAccount = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)

        Dim udtEHSTransaction As EHSTransactionModel
        udtEHSTransaction = Me.udtSessionHandlerBLL.EHSTransactionGetFromSession(FunctionCode)

        Dim udtHAVaccineResult As Common.WebService.Interface.HAVaccineResult
        udtHAVaccineResult = Me.udtSessionHandlerBLL.CMSVaccineResultGetFromSession(FunctionCode)

        Dim udtDHVaccineResult As Common.WebService.Interface.DHVaccineResult
        udtDHVaccineResult = Me.udtSessionHandlerBLL.CIMSVaccineResultGetFromSession(FunctionCode)

        Dim udtSchemeClaim As SchemeClaimModel
        udtSchemeClaim = Me.udtSessionHandlerBLL.SelectSchemeGetFromSession(FunctionCode)

        ' ----------------------------
        ' Set Transaction ID
        ' ----------------------------
        Dim strTransactionID As String = Me.udtGeneralFunction.generateTransactionNumber(udtEHSTransaction.SchemeCode, True)

        udtAuditLogEntry.AddDescripton(IIf(udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount, "eHS Validated Acc ID", "eHS Temp Acc ID"), _
                                       IIf(udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount, udtEHSAccount.VoucherAccID, udtEHSAccount.VoucherAccID))
        udtAuditLogEntry.AddDescripton("Transaction ID", strTransactionID)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00036, AuditLogDescription.NewClaimTransaction_ConfirmClaim)

        Me.udcMessageBox.Visible = False
        Me.udcInfoMessageBox.Visible = False

        Dim blnValid As Boolean = False

        udtEHSTransaction.TransactionID = strTransactionID

        ' -------------------------------------
        ' Set VoucherAccID / TempVoucherAccID
        ' -------------------------------------
        ' Account information
        If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
            udtEHSTransaction.VoucherAccID = udtEHSAccount.VoucherAccID
            udtEHSTransaction.TempVoucherAccID = Nothing
        Else
            udtEHSTransaction.VoucherAccID = Nothing
            udtEHSTransaction.TempVoucherAccID = udtEHSAccount.VoucherAccID
        End If

        ' ----------------------------
        ' Set HA Vaccine Ref. Status
        ' ----------------------------
        Dim udtHAVaccineRefStatus As EHSTransaction.EHSTransactionModel.ExtRefStatusClass = Nothing
        If udtHAVaccineResult Is Nothing Then
            udtHAVaccineRefStatus = New EHSTransaction.EHSTransactionModel.ExtRefStatusClass()
        Else
            udtHAVaccineRefStatus = New EHSTransaction.EHSTransactionModel.ExtRefStatusClass(udtHAVaccineResult, udtEHSAccount.EHSPersonalInformationList(0).DocCode)
        End If

        udtHAVaccineRefStatus = EHSTransactionModel.ExtRefStatusClass.AmendExtRefStatus(udtSchemeClaim, udtHAVaccineRefStatus, VaccinationBLL.VaccineRecordProvider.HA)
        If udtHAVaccineRefStatus Is Nothing Then
            udtEHSTransaction.HAVaccineRefStatus = Nothing
        Else
            ' Change show flag if user no view vaccination record (HCVU available only)
            If ViewState(VS.VaccinationRecordPopupShown) Is Nothing Then
                udtHAVaccineRefStatus.ResultShown = EHSTransactionModel.ExtRefStatusClass.ResultShownEnum.No
            End If
            udtEHSTransaction.HAVaccineRefStatus = udtHAVaccineRefStatus.Code
        End If

        ' ----------------------------
        ' Set DH Vaccine Ref. Status
        ' ----------------------------
        Dim udtDHVaccineRefStatus As EHSTransaction.EHSTransactionModel.ExtRefStatusClass = Nothing
        If udtDHVaccineResult Is Nothing Then
            udtDHVaccineRefStatus = New EHSTransaction.EHSTransactionModel.ExtRefStatusClass()
        Else
            udtDHVaccineRefStatus = New EHSTransaction.EHSTransactionModel.ExtRefStatusClass(udtDHVaccineResult, udtEHSAccount.EHSPersonalInformationList(0).DocCode)
        End If

        udtDHVaccineRefStatus = EHSTransactionModel.ExtRefStatusClass.AmendExtRefStatus(udtSchemeClaim, udtDHVaccineRefStatus, VaccinationBLL.VaccineRecordProvider.DH)
        If udtDHVaccineRefStatus Is Nothing Then
            udtEHSTransaction.DHVaccineRefStatus = Nothing
        Else
            ' Change show flag if user no view vaccination record (HCVU available only)
            If ViewState(VS.VaccinationRecordPopupShown) Is Nothing Then
                udtDHVaccineRefStatus.ResultShown = EHSTransactionModel.ExtRefStatusClass.ResultShownEnum.No
            End If
            udtEHSTransaction.DHVaccineRefStatus = udtDHVaccineRefStatus.Code
        End If

        ' ----------------------------

        Dim udtWarningMessage As EHSClaim.EHSClaimBLL.EHSClaimBLL.RuleResultList = Nothing

        udtWarningMessage = udtEHSTransaction.WarningMessage

        If Not IsNothing(udtWarningMessage) Then
            udtEHSTransaction.OverrideReason = Me.txtConfirmClaimCreationOverrideReason.Text.Trim
        End If

        udtEHSTransaction.RecordStatus = EHSTransactionModel.TransRecordStatusClass.PendingApprovalForNonReimbursedClaim

        Try
            Me.udtSM = Me.udtEHSClaimBLL.CreateEHSTransaction(udtEHSTransaction, udtEHSAccount, udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode.Trim), udtSchemeClaim)
            blnValid = True

        Catch eSql As SqlClient.SqlException
            If eSql.Number = 50000 Then
                blnValid = False

                udtAuditLogEntry.AddDescripton(IIf(udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount, "eHS Validated Acc ID", "eHS Temp Acc ID"), _
                                               IIf(udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount, udtEHSAccount.VoucherAccID, udtEHSAccount.VoucherAccID))
                udtAuditLogEntry.AddDescripton("Transaction ID", strTransactionID)
                Me.udcMessageBox.AddMessage(New SystemMessage("990001", "D", eSql.Message))
                Me.udcMessageBox.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00038, AuditLogDescription.NewClaimTransaction_ConfirmClaim_Fail)
            Else
                Throw eSql
            End If
        Catch ex As Exception
            Throw
        End Try

        'if Success
        If blnValid = True Then
            udtEHSTransaction = udtEHSTransactionBLL.LoadEHSTransaction(udtEHSTransaction.TransactionID)

            Dim strOld As String() = {"%s"}
            Dim strNew As String() = {""}

            strNew(0) = Me.udtFormatter.formatSystemNumber(udtEHSTransaction.TransactionID)

            Me.udcInfoMessageBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003), strOld, strNew)
            Me.udcInfoMessageBox.BuildMessageBox()
            Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete

            Me.mvNewClaimTransaction.ActiveViewIndex = NewClaimTransaction.Complete

            udtAuditLogEntry.AddDescripton(IIf(udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount, "eHS Validated Acc ID", "eHS Temp Acc ID"), _
                                           IIf(udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount, udtEHSAccount.VoucherAccID, udtEHSAccount.VoucherAccID))
            udtAuditLogEntry.AddDescripton("Transaction ID", udtEHSTransaction.TransactionID)
            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00037, AuditLogDescription.NewClaimTransaction_ConfirmClaim_Success)

            'clear session
            ClearSessionForNewClaim(True)

        End If

    End Sub
    ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

    Protected Sub ibtnConfirmClaimCreationCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        Dim udtEHSAccount As EHSAccount.EHSAccountModel
        Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
        udtEHSAccount = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        udtAuditLogEntry.AddDescripton(IIf(udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount, "eHS Validated Acc ID", "eHS Temp Acc ID"), _
                                       IIf(udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount, udtEHSAccount.VoucherAccID, udtEHSAccount.VoucherAccID))

        Me.udcMessageBox.Visible = False
        Me.udcInfoMessageBox.Visible = False

        Dim udtEHSTransaction As EHSTransactionModel = Me.udtSessionHandlerBLL.EHSTransactionGetFromSession(FunctionCode)

        If Not IsNothing(udtEHSAccount) Then
            BindPersonalInfo(udtEHSAccount)
        End If

        Me.mvNewClaimTransaction.ActiveViewIndex = NewClaimTransaction.EnterClaimDetails
        Me.mvEnterDetails.ActiveViewIndex = InputTransactionDetails.ClaimDetails

        SetUpEnterClaimDetails(False)

        ' Clear the saved Transaction Details from the Transaction
        If Not udtEHSTransaction.TransactionDetails Is Nothing Then
            udtEHSTransaction.TransactionDetails.Clear()
            udtEHSTransaction.TransactionDetails = Nothing

        End If

        If Not udtEHSTransaction.TransactionAdditionFields Is Nothing Then
            udtEHSTransaction.TransactionAdditionFields.Clear()
            udtEHSTransaction.TransactionAdditionFields = Nothing
        End If

        Me.udtSessionHandlerBLL.EHSTransactionSaveToSession(udtEHSTransaction, FunctionCode)

        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00039, AuditLogDescription.NewClaimTransaction_ConfirmClaim_Cancel)

    End Sub

#End Region

#Region "Complete Claim Creation"
    Protected Sub ibtnCompleteClaimCreationReturn_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00040, AuditLogDescription.NewClaimTransaction_CompleteReturn)

        Me.udcInfoMessageBox.Visible = False

        Me.mvNewClaimTransaction.ActiveViewIndex = NewClaimTransaction.EnterClaimDetails
        Me.mvEnterDetails.ActiveViewIndex = InputTransactionDetails.CreationDetails

        ' Initial setting for claim creation
        InitialEnterCreationDetail()

    End Sub

#End Region

#Region "Supported Functions"

    Private Sub BindPersonalInfo(ByVal udtEHSAccount As EHSAccount.EHSAccountModel)
        udceHealthAccountInfo.Clear()
        udceHealthAccountInfo.DocumentType = udtEHSAccount.SearchDocCode.Trim
        udceHealthAccountInfo.MaskIdentityNo = True
        udceHealthAccountInfo.IsInvalidAccount = False
        udceHealthAccountInfo.EHSAccountModel = udtEHSAccount
        udceHealthAccountInfo.EHSPersonalInformation = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode.Trim)
        udceHealthAccountInfo.ShowAccountID = True
        udceHealthAccountInfo.ShowAccountStatus = True
        udceHealthAccountInfo.Build()

    End Sub

    Private Function GetAllPractice(ByVal strSPID As String, ByVal enumPracticeDisplayType As Practice.PracticeBLL.PracticeDisplayType) As DataTable
        Dim udtPracticeBLL As New Practice.PracticeBLL
        ' Get Practice Information
        Dim dtPractice As DataTable = udtPracticeBLL.getRawAllPracticeBankAcct(strSPID)

        Practice.PracticeBLL.ConcatePracticeDisplayColumn(dtPractice, Practice.PracticeBLL.PracticeDisplayType.Practice)

        Return dtPractice

    End Function

    Private Sub GetReadyServiceProvider(ByVal strSPID)
        Dim udtSPBLL As New ServiceProvider.ServiceProviderBLL

        Dim udtSP As ServiceProvider.ServiceProviderModel

        udtSP = udtSPBLL.GetServiceProviderPermanentProfileWithMaintenanceBySPID(strSPID, New Common.DataAccess.Database)

        Me.lblEnterCreationDetailSPName.Text = udtSP.EnglishName
        If Not udtSP.RecordStatus.Trim.Equals(ServiceProviderStatus.Active) Then
            Status.GetDescriptionFromDBCode(ServiceProviderStatus.ClassCode, udtSP.RecordStatus, Me.lblEnterCreationDetailSPStatus.Text, String.Empty)

            Me.lblEnterCreationDetailSPStatus.Text = " (" + Me.lblEnterCreationDetailSPStatus.Text + ")"
        End If

        Dim dtPracticeList = Me.GetAllPractice(udtSP.SPID, Practice.PracticeBLL.PracticeDisplayType.Practice)
        Dim udtPracticeBLL As New Practice.PracticeBLL

        Me.udtSessionHandlerBLL.PracticeDisplayListRemoveFromSession(FunctionCode)
        Me.udtSessionHandlerBLL.PracticeDisplayListSaveToSession(udtPracticeBLL.convertPractice(dtPracticeList), FunctionCode)

        Me.ddlEnterCreationDetailPractice.DataSource = dtPracticeList

        Me.ddlEnterCreationDetailPractice.DataTextField = Practice.PracticeBLL.PracticeDisplayField.Display_Eng
        Me.ddlEnterCreationDetailPractice.DataValueField = "PracticeID"
        Me.ddlEnterCreationDetailPractice.DataBind()

        ddlEnterCreationDetailPractice.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), ""))

        ddlEnterCreationDetailPractice.SelectedIndex = 0

        Me.ddlEnterCreationDetailPractice.Enabled = True

        Me.ibtnSearchSP.Enabled = False
        Me.ibtnClearSearchSP.Enabled = True

        Me.ibtnSearchSP.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchDisableSBtn")
        Me.ibtnClearSearchSP.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ClearSBtn")

        Me.ibtnNewClaimTransaction.Enabled = True
        Me.ibtnNewClaimTransaction.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "NewClaimTransactionBtn")

        Me.txtEnterCreationDetailSPID.Text = udtSP.SPID
        Me.txtEnterCreationDetailSPID.Enabled = False

        Session(SESS_ServiceProvider) = udtSP
    End Sub

    Private Sub GetReadyServiceProviderForAuditlog(ByVal strSPID)
        Dim udtSPBLL As New ServiceProvider.ServiceProviderBLL

        Dim udtSP As ServiceProvider.ServiceProviderModel

        udtSP = udtSPBLL.GetServiceProviderPermanentProfileWithMaintenanceBySPID(strSPID, New Common.DataAccess.Database)

        Session(SESS_ServiceProvider) = udtSP

    End Sub

    Private Sub GetReadyEHSAccount(ByVal strVoucherAccID As String, ByVal strDocCode As String, ByRef udtEHSAccount As EHSAccount.EHSAccountModel)

        Dim udtEHSAccountBLL As New EHSAccount.EHSAccountBLL

        'Get Validated Account
        udtEHSAccount = udtEHSAccountBLL.LoadEHSAccountByVRID(strVoucherAccID)

        'If no validated account, get temp account
        If udtEHSAccount Is Nothing Then
            udtEHSAccount = udtEHSAccountBLL.LoadTempEHSAccountByVRID(strVoucherAccID)

        End If

        ''clear related session
        'ClearSessionForNewClaim(blnClearSearchAccountList)

        'Bind the eHS Account Details        
        udtEHSAccount.SetSearchDocCode(strDocCode)

        Dim udteHSAccountMaintBLL As New eHSAccountMaintBLL

        udteHSAccountMaintBLL.EHSAccountSaveToSession(udtEHSAccount, FunctionCode)

        'BindCreationReason()
        'Me.txtEnterCreationDetailRemarks.Text = String.Empty

        'BindPaymentMethod()
        'Me.txtEnterCreationDetailPaymentRemarks.Text = String.Empty

        'txtEnterCreationDetailSPID.Text = String.Empty
        'txtEnterCreationDetailSPID.Enabled = True

        'Me.lblEnterCreationDetailSPName.Text = String.Empty
        'Me.lblEnterCreationDetailSPStatus.Text = String.Empty
        'Me.lblEnterCreationDetailPracticeStatus.Text = String.Empty

        'Me.ibtnSearchSP.Enabled = True
        'Me.ibtnClearSearchSP.Enabled = False

        'Me.ibtnSearchSP.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchSBtn")
        'Me.ibtnClearSearchSP.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ClearDisableSBtn")

        'Me.ddlEnterCreationDetailPractice.Items.Clear()
        'Me.ddlEnterCreationDetailPractice.Enabled = False

        'Me.ibtnNewClaimTransaction.Enabled = False
        'Me.ibtnNewClaimTransaction.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "NewClaimTransactionBtn")

        'Me.mvNewClaimTransaction.ActiveViewIndex = NewClaimTransaction.EnterClaimDetails
        'Me.mvEnterDetails.ActiveViewIndex = InputTransactionDetails.ClaimDetails

    End Sub

    Private Sub ClearSessionForNewClaim(ByVal blnClearSearchAccountList As Boolean)
        Me.udtSessionHandlerBLL.EHSClaimVaccineRemoveFromSession(FunctionCode)
        Me.udtSessionHandlerBLL.EHSTransactionRemoveFromSession(FunctionCode)
        Me.udtSessionHandlerBLL.PracticeDisplayListRemoveFromSession(FunctionCode)
        Me.udtSessionHandlerBLL.PracticeDisplayRemoveFromSession(FunctionCode)
        Me.udtSessionHandlerBLL.SchemeListRemoveFromSession(FunctionCode)
        Me.udtSessionHandlerBLL.SelectSchemeRemoveFromSession(FunctionCode)
        Me.udtSessionHandlerBLL.RVPRCHCodeRemoveFromSession(FunctionCode)
        Me.udtSessionHandlerBLL.ClaimCategoryRemoveFromSession(FunctionCode)

        Dim udteHSAccountMaintBLL As New eHSAccountMaintBLL
        udteHSAccountMaintBLL.EHSAccountRemoveFromSession(FunctionCode)

        Session.Remove(SESS_AdvancedSearchSP)
        Session.Remove(SESS_SearchRVPHomeList)
        Session.Remove(SESS_ServiceProvider)

        If blnClearSearchAccountList Then
            Session.Remove(SESS_SearchAccount)
        End If
    End Sub

#End Region

#Region "Implement IWorkingData (CRE11-004)"

    Public Overrides Function GetDocCode() As String
        If GetEHSAccount() Is Nothing Then Return Nothing
        If GetEHSAccount.SearchDocCode <> String.Empty Then
            Return GetEHSAccount.SearchDocCode
        End If

        If GetEHSTransaction() IsNot Nothing Then
            Return GetEHSTransaction.DocCode
        End If

        Return Nothing
    End Function

    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Dim udtEHSAccount As EHSAccountModel = (New eHSAccountMaintBLL).EHSAccountGetFromSession(FunctionCode)
        If udtEHSAccount IsNot Nothing Then
            Return udtEHSAccount
        Else
            If GetEHSTransaction() IsNot Nothing Then
                Return GetEHSTransaction().EHSAcct
            Else
                Return Nothing
            End If
        End If

    End Function

    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Me.udtSessionHandlerBLL.EHSTransactionGetFromSession(FunctionCode)

    End Function

    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        If GetEHSTransaction() IsNot Nothing Then
            Me.GetReadyServiceProviderForAuditlog(GetEHSTransaction().ServiceProviderID)
        End If

        Return Session(SESS_ServiceProvider)

    End Function

#End Region

End Class